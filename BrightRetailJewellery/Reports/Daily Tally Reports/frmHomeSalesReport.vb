Imports System.Data.OleDb
Imports System.Threading
Public Class frmHomeSalesReport
    Inherits System.Windows.Forms.Form
    Dim ThisObject As Object
    Dim dtFlag As New DataTable
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim SelectedCompany As String
    Dim SelectedCosts As String
    Dim Selecteditems As String
    Dim dtCostCentre As New DataTable
    Dim Update_Tagno As Boolean = IIf(GetAdmindbSoftValue("HMTAGUPDATE_ENABLE", "N") = "Y", True, False)
    Dim Update_AI_Tagno As Boolean = IIf(GetAdmindbSoftValue("HMTAGUPDATE_AI", "N") = "Y", True, False)
    Dim SoftLock_Days As String = GetAdmindbSoftValue("SOFTLOCK_DAYS", "")
    Dim SERVERDATE As Date
    Dim VALUEDATE As Date
    Dim selectedCashid As String
    Dim selectedSystemId As String
    Private Sub ValidateDate()
        SERVERDATE = GetServerDate()
        If Val(SoftLock_Days.ToString) <> 0 Then
            VALUEDATE = SERVERDATE.AddDays(Val(SoftLock_Days * -1))
        End If
        If dtpFrom.Value.Date.ToString("yyyy-MM-dd") = SERVERDATE.ToString("yyyy-MM-dd") Then
            btnUpdateTagNo.Enabled = True
        ElseIf dtpFrom.Value.Date.ToString("yyyy-MM-dd") >= VALUEDATE.ToString("yyyy-MM-dd") Then
            btnUpdateTagNo.Enabled = True
        Else
            btnUpdateTagNo.Enabled = False
            MsgBox("You Don't Have Authenticate For Update Tagno", MsgBoxStyle.OkOnly)
        End If
    End Sub
    Private Sub frmHomeSalesReport_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        rbtBackOffice.Checked = True
        'Call function for Show the grid header without datas
        funcNew()
        funcAddMetalName()
        If cmbMetalName.Items.Count > 0 Then
            cmbMetalName.SelectedIndex = 0
        End If
        ValidateDate()
        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT COSTNAME,CONVERT(VARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += vbCrLf + " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False

        strSql = " SELECT 'ALL' ITEMNAME,'ALL' ITEMID,1 RESULT"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT ITEMNAME,CONVERT(VARCHAR,ITEMID),2 RESULT FROM " & cnAdminDb & "..ITEMMAST "
        strSql += vbCrLf + " ORDER BY RESULT,ITEMNAME"
        Dim dtitemname As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtitemname)
        BrighttechPack.GlobalMethods.FillCombo(chkcmbitemname, dtitemname, "ITEMNAME", , "ALL")


        strSql = " SELECT 'ALL' CASHNAME,'ALL' CASHID,1 RESULT"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT CASHNAME,CONVERT(VARCHAR,CASHID),2 RESULT FROM " & cnAdminDb & "..CASHCOUNTER "
        strSql += vbCrLf + " ORDER BY RESULT,CASHNAME"
        Dim dtcounterName As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtcounterName)
        BrighttechPack.GlobalMethods.FillCombo(chkcmbCounterName, dtcounterName, "CASHNAME", , "ALL")

        strSql = " SELECT 'ALL' NODEID,'ALL' NODEID,1 RESULT"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT NODEID,CONVERT(VARCHAR,NODEID),2 RESULT FROM " & cnAdminDb & "..CASHCOUNTER "
        Dim dtNODEID As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtNODEID)
        BrighttechPack.GlobalMethods.FillCombo(chkcmbNodeId, dtNODEID, "NODEID", , "ALL")
        LoadCompany(chkLstCompany)
        'call function for set style properties to Gridview control
        funcGridStyle()
        btnUpdateTagNo.Visible = BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Edit, False)
        btnNew_Click(Me, New EventArgs)
    End Sub
    Private Sub frmHomeSalesReport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If AscW(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
        If UCase(e.KeyChar) = Chr(Keys.X) Then
            btnExcel_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = Chr(Keys.P) Then
            btnPrint_Click(Me, New EventArgs)
        End If
    End Sub
    Private Sub btnView_Search_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        ValidateDate()
        btnView_Search.Enabled = False
        AutoSizeToolStripMenuItem.Checked = False
        Me.Refresh()
        'Call function for Add datas into Grid GridFlag
        funcView()
        btnView_Search.Enabled = True
        Prop_Sets()
    End Sub
    Private Sub btnNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNew.Click
        'btnNew.Enabled = False
        'rbtBackOffice.Checked = True
        funcNew()
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        If cmbMetalName.Items.Count > 0 Then
            cmbMetalName.SelectedIndex = 0
        End If
        btnNew.Enabled = True
        Prop_Gets()
        dtpFrom.Focus()
    End Sub
    Private Sub btnExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridFlag.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridFlag, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub
    Private Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridFlag.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridFlag, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub
    Private Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub
    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.btnExit_Click(Me, New EventArgs)
    End Sub
    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        Me.btnNew_Click(Me, New EventArgs)
    End Sub
    Private Sub gridFlag_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridFlag.KeyDown
        If e.KeyCode = Keys.Escape Then
            btnNew.Focus()
        End If
    End Sub
    Public Function funcNew() As Integer
        'Get the datagrid with column header
        Try
            lblTitle.Text = "TITLE"
            chkWithApproval.Visible = Update_AI_Tagno
            chkWithApproval.Checked = False
            Dim dtGridHeader As New DataTable
            strSql = "select i.tranno BILLNO,i.trandate BILLDATE,"
            strSql += vbcrlf + "''ITEMNAME,"
            strSql += vbcrlf + "''SUBITEMNAME,''TAGNO"
            strSql += vbcrlf + " ,i.pcs AS PCS,i.grswt AS GRSWT,i.lesswt AS LESSWT,i.netwt AS NETWT"
            strSql += vbcrlf + " ,CONVERT(NUMERIC(15,4),NULL) as DIAWT,i.amount AS AMOUNT,i.flag AS FLAG"
            strSql += vbcrlf + " ,1 result,I.TRANDATE,I.TRANNO,'' SNO"
            strSql += vbcrlf + " from " & cnStockDb & "..issue as i where 1<>1 "
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtGridHeader)
            gridFlag.DataSource = dtGridHeader
            lblTitle.Height = gridFlag.ColumnHeadersHeight
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
            Exit Function
        End Try
    End Function
    Public Function funcView() As Integer
        Try
            dtFlag.Clear()
            lblTitle.Text = "TITLE"
            Me.Refresh()
            SelectedCompany = GetSelectedCompanyId(chkLstCompany, False)
            SelectedCosts = GetSelectedCostId(chkCmbCostCentre, False)
            Selecteditems = GetSelecteditemid(chkcmbitemname, False)
            selectedCashid = "ALL"
            selectedSystemId = "ALL"

            If GetQryString(chkcmbCounterName.Text).Replace("'", "") <> "ALL" Then
                selectedCashid = GetQryString(chkcmbCounterName.Text).Replace("'", "")
            End If

            If GetQryString(chkcmbNodeId.Text).Replace("'", "") <> "ALL" Then
                selectedSystemId = GetQryString(chkcmbNodeId.Text, ",").Replace("'", "")
            End If


            Dim strFlag As String = Nothing
            If rbtBackOffice.Checked = True Then
                strFlag = "B"
            ElseIf rbtCounterSales.Checked = True Then
                strFlag = "C"
            ElseIf rbtBoth.Checked = True Then
                strFlag = "A"
            End If
            strSql = "EXEC " & cnStockDb & "..SP_RPT_COUNTERSALES"
            strSql += vbcrlf + " @DATEFROM = '" & Replace(dtpFrom.Value.Date.ToString("yyyy-MM-dd"), "'", "''") & "'"
            strSql += vbcrlf + " ,@DATETO ='" & Replace(dtpTo.Value.Date.ToString("yyyy-MM-dd"), "'", "''") & "'"
            strSql += vbcrlf + " ,@FLAG = '" & strFlag & "'"
            strSql += vbcrlf + " ,@METALNAME = '" & Replace(cmbMetalName.Text, "'", "''''") & "'"
            strSql += vbcrlf + " ,@SystemID='" & systemId & "',@cnAdminDB='" & cnAdminDb & "',@cnStockDB='" & cnStockDb & "'"
            strSql += vbcrlf + " ,@COMPANYID = '" & SelectedCompany & "'"
            strSql += vbcrlf + " ,@GRPITEM = '" & IIf(chkGroupItem.Checked, "Y", "N") & "'"
            'strSql += vbcrlf + " ,@COSTID = '" & IIf(SelectedCosts Is Nothing, "", SelectedCosts) & "'"
            strSql += vbcrlf + " ,@COSTNAME = '" & IIf(chkCmbCostCentre.Text.Trim <> "", GetQryString(chkCmbCostCentre.Text).Replace("'", ""), "ALL") & "'"
            strSql += vbCrLf + " ,@ITEMID = '" & Selecteditems & "'"
            strSql += vbCrLf + " ,@CASHID = '" & selectedCashid & "'"
            strSql += vbCrLf + " ,@SYSTEM = '" & selectedSystemId & "'"
            strSql += vbCrLf + " ,@APPROVAL = '" & IIf(chkWithApproval.Checked, "Y", "N") & "'"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = "UPDATE TEMPTABLEDB..TEMP" & systemId & "COUNTERREPORT SET PCS = NULL WHERE PCS = 0"
            strSql += vbcrlf + "UPDATE TEMPTABLEDB..TEMP" & systemId & "COUNTERREPORT SET GRSWT = NULL WHERE GRSWT = 0"
            strSql += vbcrlf + "UPDATE TEMPTABLEDB..TEMP" & systemId & "COUNTERREPORT SET LESSWT = NULL WHERE LESSWT = 0"
            strSql += vbcrlf + "UPDATE TEMPTABLEDB..TEMP" & systemId & "COUNTERREPORT SET NETWT = NULL WHERE NETWT = 0"
            strSql += vbcrlf + "UPDATE TEMPTABLEDB..TEMP" & systemId & "COUNTERREPORT SET AMOUNT = NULL WHERE AMOUNT = 0"
            strSql += vbcrlf + "UPDATE TEMPTABLEDB..TEMP" & systemId & "COUNTERREPORT SET DIAWT = NULL WHERE DIAWT = 0"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()


            dtFlag = New DataTable
            dtFlag.Columns.Add("KEYNO", GetType(Integer))
            dtFlag.Columns("KEYNO").AutoIncrement = True
            dtFlag.Columns("KEYNO").AutoIncrementSeed = 0
            dtFlag.Columns("KEYNO").AutoIncrementStep = 1

            If ChkOrderDelivery.Checked = True Then
                strSql = "SELECT * FROM TEMPTABLEDB..TEMP" & systemId & "COUNTERREPORT ORDER BY SEP,ITEMNAME,RESULT,FLAG,TRANDATE,TRANNO"
            Else
                strSql = "SELECT * FROM TEMPTABLEDB..TEMP" & systemId & "COUNTERREPORT WHERE RESULT<>'4' ORDER BY ITEMNAME,RESULT,FLAG,TRANDATE,TRANNO"
            End If
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtFlag)
            If dtFlag.Rows.Count < 1 Then
                funcNew()
                MessageBox.Show("Records not found..", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information)
                btnView_Search.Focus()
                Exit Function
            End If


            gridFlag.DataSource = dtFlag
            gridFlag.Rows(gridFlag.Rows.Count - 1).DefaultCellStyle.BackColor = Color.LightGoldenrodYellow
            gridFlag.Rows(gridFlag.Rows.Count - 1).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            FillGridGroupStyle_KeyNoWise(gridFlag, "ITEMNAME")
            gridFlag.Rows(gridFlag.Rows.Count - 1).Cells("ITEMNAME").Value = "GRAND TOTAL"
            gridFlag.Columns("COLHEAD").Visible = False
            gridFlag.Columns("RESULT").Visible = False
            gridFlag.Columns("KEYNO").Visible = False
            gridFlag.Columns("SEP").Visible = False
            Dim strTitle As String = Nothing
            strTitle = " HOME SALES REPORT"
            strTitle += " FROM " & dtpFrom.Text & " TO " & dtpTo.Text & ""
            If cmbMetalName.Text <> "ALL" And cmbMetalName.Text <> "" Then
                strTitle += " FOR " & cmbMetalName.Text & ""
            End If
            If rbtBackOffice.Checked = True Then
                strTitle += "(BACKOFFICE)"
            ElseIf rbtCounterSales.Checked = True Then
                strTitle += "(COUNTERSALES)"
            ElseIf rbtBoth.Checked = True Then
                strTitle += "(BACKOFFICE AND COUNTERSALES)"
            End If
            strTitle += IIf(chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "", " :" & chkCmbCostCentre.Text, "")
            lblTitle.Text = strTitle
            gridFlag.Focus()
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function
    Public Function funcGridStyle() As Integer
        With gridFlag
            .Columns("result").Visible = False
            .Columns("TRANDATE").Visible = False
            .Columns("TRANNO").Visible = False
            .Columns("SNO").Visible = False
            With .Columns("BILLNO")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("BILLDATE")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("ITEMNAME")
                .Width = 150
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("SUBITEMNAME")
                .Width = 150
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("TAGNO")
                .Width = 70
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("PCS")
                .Width = 60
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("GRSWT")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("LESSWT")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("NETWT")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("AMOUNT")
                .Width = 100
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With

            With .Columns("DIAWT")
                .Width = 100
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With

            With .Columns("FLAG")
                .Width = 120
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With

            gridFlag.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        End With
    End Function

    Public Function funcAddMetalName() As Integer
        strSql = "select DISTINCT metalname from  " & cnAdminDb & "..metalmast order by metalname"
        cmbMetalName.Items.Clear()
        cmbMetalName.Items.Add("ALL")
        objGPack.FillCombo(strSql, cmbMetalName, False, False)
        cmbMetalName.Text = "ALL"
    End Function
    'Function CRYSTALREPORT() As Integer
    'dtFlag.Clear()
    ''SQL Query for get the datas depends on Flag column
    'strsql = "if (select 1 from sysobjects where name='TEMP" & SystemId & "COUNTERREPORT') > 0"
    'strSql += vbcrlf + " drop table TEMP" & SystemId & "COUNTERREPORT"
    'strSql += vbcrlf + " select str(i.tranno) as 'TRANNO',i.trandate AS 'TRANDATE',"
    'strSql += vbcrlf + "(select itemname from " & cnAdminDb & "..itemmast as it where i.itemid = it.itemid)as ITEMNAME,"
    'strSql += vbcrlf + "(select subitemname  from " & cnAdminDb & "..subitemmast as si where i.subitemid =si.subitemid)as SUBITEMNAME,"
    'strSql += vbcrlf + "i.pcs AS PCS,i.grswt AS GRSWT,i.lesswt AS LESSWT,i.netwt AS NETWT,i.amount AS AMOUNT,"
    'strSql += vbcrlf + "case when i.flag ='c' then 'COUNTERSALES' when flag ='b' then 'BACKOFFICE' else '' end AS FLAG,1 result"
    'strSql += vbcrlf + " into TEMP" & SystemId & "COUNTERREPORT"
    'strSql += vbcrlf + " from " & cnStockDb & "..issue as i "
    'strSql += vbcrlf + " where trandate between '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' and '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
    'If cmbMetalName.Text <> "ALL" And cmbMetalName.Text <> "" Then
    '    strSql += vbcrlf + " and i.metalid=(select metalid from " & cnAdminDb & "..metalmast where metalname='" & cmbMetalName.Text & "')"
    'End If
    'If rbtBackOffice.Checked = True Then
    '    strSql += vbcrlf + " and flag = 'b'"
    'ElseIf rbtCounterSales.Checked = True Then
    '    strSql += vbcrlf + " and flag = 'c'"
    'ElseIf rbtBoth.Checked = True Then
    '    strSql += vbcrlf + " and flag in ('c','b')"
    'End If
    'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
    '
    'cmd.ExecuteNonQuery()
    '
    ''strsql = " IF (SELECT COUNT(*) FROM TEMP" & SystemId & "COUNTERREPORT)>0"
    ''strSql += vbcrlf + " BEGIN"
    ''strSql += vbcrlf + " insert into TEMP" & SystemId & "COUNTERREPORT"
    ''strSql += vbcrlf + " select ' ' as tranno,' ' as trandate,' ' as itemname,' ' as subitemname,"
    ''strSql += vbcrlf + "sum(pcs) as PCS,sum(grswt) as GRSWT,sum(lesswt) as LESSWT,sum(netwt) as NETWT,sum(amount) as AMOUNT,' ',2 result"
    ''strSql += vbcrlf + " from TEMP" & SystemId & "COUNTERREPORT"
    ''strSql += vbcrlf + " end"

    'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
    '
    'cmd.ExecuteNonQuery()
    '
    'strsql = " select * from TEMP" & SystemId & "COUNTERREPORT order by result,flag"
    ''Show the datas in datagrid
    'da = New OleDbDataAdapter(strsql, cn)
    'da.Fill(dtFlag)
    'If dtFlag.Rows.Count < 1 Then
    '    funcNew()
    '    MessageBox.Show("Records not found..", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '    btnView.Focus()
    '    Exit Sub
    'End If
    'Dim objactivereport As New CRYDATASET
    'Dim row As CRYDATASET.COUNTERSALESRPTRow
    'For cnt As Integer = 0 To dtFlag.Rows.Count - 1
    '    row = objactivereport.COUNTERSALESRPT.NewRow
    '    With dtFlag.Rows(cnt)
    '        row("TRANNO") = .Item("TRANNO")
    '        row("TRANDATE") = .Item("TRANDATE")
    '        row("ITEMNAME") = .Item("ITEMNAME")
    '        row("PCS") = .Item("PCS")
    '        row("AMOUNT") = .Item("AMOUNT")
    '        row("FLAG") = .Item("FLAG")
    '    End With
    '    objactivereport.COUNTERSALESRPT.AddCOUNTERSALESRPTRow(row)
    'Next
    'Dim OBJREPORT As New CRYCOUNTERRPT
    'OBJREPORT.SetDataSource(objactivereport)
    'Dim OBJREPORTFRM As New Form1
    'OBJREPORTFRM.CrystalReportViewer1.ReportSource = OBJREPORT
    'OBJREPORTFRM.Show()
    'End Function



    'Function QUERYFORWITHOUTSTOREDPROCEDURE() As Integer
    ''SQL Query for get the datas depends on Flag column
    'strsql = "if (select 1 from sysobjects where name='TEMP" & SystemId & "COUNTERREPORT') > 0"
    'strSql += vbcrlf + " drop table TEMP" & SystemId & "COUNTERREPORT"
    'strSql += vbcrlf + " select str(i.tranno) as 'TRANNO',convert(varchar,i.trandate,103) AS 'TRANDATE',"
    'strSql += vbcrlf + "(select itemname from " & cnAdminDb & "..itemmast as it where i.itemid = it.itemid)as ITEMNAME,"
    'strSql += vbcrlf + "(select subitemname  from " & cnAdminDb & "..subitemmast as si where i.subitemid =si.subitemid)as SUBITEMNAME,"
    'strSql += vbcrlf + "i.pcs AS PCS,i.grswt AS GRSWT,i.lesswt AS LESSWT,i.netwt AS NETWT,i.amount AS AMOUNT,"
    'strSql += vbcrlf + "case when i.flag ='c' then 'COUNTERSALES' when flag ='b' then 'BACKOFFICE' else '' end AS FLAG,1 result"
    'strSql += vbcrlf + " into TEMP" & SystemId & "COUNTERREPORT"
    'strSql += vbcrlf + " from " & cnStockDb & "..issue as i "
    'strSql += vbcrlf + " where trandate between '" & dtpDateFrom.Value.Date.ToString("yyyy-MM-dd") & "' and '" & dtpDateTo.Value.Date.ToString("yyyy-MM-dd") & "'"
    'If cmbMetalName.Text <> "ALL" And cmbMetalName.Text <> "" Then
    '    strSql += vbcrlf + " and i.metalid=(select metalid from " & cnAdminDb & "..metalmast where metalname='" & Replace(cmbMetalName.Text, "'", "''") & "')"
    'End If
    'If rbtBackOffice.Checked = True Then
    '    strSql += vbcrlf + " and flag = 'b'"
    'ElseIf rbtCounterSales.Checked = True Then
    '    strSql += vbcrlf + " and flag = 'c'"
    'ElseIf rbtBoth.Checked = True Then
    '    strSql += vbcrlf + " and flag in ('c','b')"
    'End If
    'strSql += vbcrlf + " ORDER BY TRANDATE"
    'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
    '
    'cmd.ExecuteNonQuery()
    '
    'strsql = " IF (SELECT COUNT(*) FROM TEMP" & SystemId & "COUNTERREPORT)>0"
    'strSql += vbcrlf + " BEGIN"
    'strSql += vbcrlf + " insert into TEMP" & SystemId & "COUNTERREPORT"
    'strSql += vbcrlf + " select ' ' as tranno,' ' as trandate,' ' as itemname,' ' as subitemname,"
    'strSql += vbcrlf + "sum(pcs) as PCS,sum(grswt) as GRSWT,sum(lesswt) as LESSWT,sum(netwt) as NETWT,sum(amount) as AMOUNT,' ',2 result"
    'strSql += vbcrlf + " from TEMP" & SystemId & "COUNTERREPORT"
    'strSql += vbcrlf + " end"

    'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
    '
    'cmd.ExecuteNonQuery()
    '
    'strsql = " select * from TEMP" & SystemId & "COUNTERREPORT order by result,flag,TRANDATE"
    'End Function

    Private Sub dtpFrom_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        dtpTo.MinimumDate = dtpFrom.Value
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        AddHandler chkLstCompany.LostFocus, AddressOf CheckedListCompany_Lostfocus
    End Sub
    Private Sub chkCompanySelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCompanySelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCompany, chkCompanySelectAll.Checked)
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmHomeSalesReport_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmHomeSalesReport_Properties))
        cmbMetalName.Text = obj.p_cmbMetalName
        chkCompanySelectAll.Checked = obj.p_chkCompanySelectAll
        SetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany, strCompanyName)
        chkGroupItem.Checked = obj.p_chkGroupItem
        rbtBackOffice.Checked = obj.p_rbtBackOffice
        rbtCounterSales.Checked = obj.p_rbtCounterSales
        rbtBoth.Checked = obj.p_rbtBoth
    End Sub
    Private Sub Prop_Sets()
        Dim obj As New frmHomeSalesReport_Properties
        obj.p_cmbMetalName = cmbMetalName.Text
        obj.p_chkCompanySelectAll = chkCompanySelectAll.Checked
        GetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany)
        obj.p_chkGroupItem = chkGroupItem.Checked
        obj.p_rbtBackOffice = rbtBackOffice.Checked
        obj.p_rbtCounterSales = rbtCounterSales.Checked
        obj.p_rbtBoth = rbtBoth.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmHomeSalesReport_Properties))
    End Sub

    Private Sub btnUpdateTagNo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdateTagNo.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Edit) Then Exit Sub
        If gridFlag.CurrentRow Is Nothing Then Exit Sub
        If gridFlag.CurrentRow.Cells("TAGNO").Value.ToString <> "" Then
            MsgBox("TagNo Alread Updated", MsgBoxStyle.Information)
            gridFlag.Focus()
            Exit Sub
        End If
        If gridFlag.CurrentRow.Cells("COLHEAD").Value.ToString <> "" Then Exit Sub
        Dim StockType As String = objGPack.GetSqlValue("SELECT STOCKTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & gridFlag.CurrentRow.Cells("ITEMNAME").Value.ToString & "'")
        If StockType <> "T" Then
            MsgBox("Updation Fails" + vbCrLf + "Update Tagged Stock Only", MsgBoxStyle.Information)
            gridFlag.Focus()
            Exit Sub
        End If
        Dim MINUSB4PLUS As Boolean = False
        If UCase(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'MINUSB4PLUS'", , "N")) = "Y" Then
            MINUSB4PLUS = True
        End If

        Dim dtIssInfo As New DataTable
        strSql = " SELECT SNO,COSTID,TRANDATE,BATCHNO,TRANNO,FLAG,PCS,GRSWT,NETWT,TRANTYPE,COMPANYID,ITEMID,TAGNO,SUBITEMID,ITEMCTRID,TAGDESIGNER,ITEMTYPEID"
        strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE WHERE SNO = '" & gridFlag.CurrentRow.Cells("SNO").Value.ToString & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtIssInfo)
        If Not dtIssInfo.Rows.Count > 0 Then
            MsgBox("Invalid Issue Info", MsgBoxStyle.Information)
            gridFlag.Focus()
            Exit Sub
        End If
        Dim RoIss As DataRow = dtIssInfo.Rows(0)
        strSql = " SELECT"
        strSql += vbCrLf + " TAGNO AS TAGNO,STYLENO,ITEMID AS ITEMID,"
        strSql += vbCrLf + " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS ITEMNAME,"
        strSql += vbCrLf + " PCS AS PCS,"
        strSql += vbCrLf + " GRSWT AS GRS_WT,NETWT AS NET_WT,RATE AS RATE,"
        strSql += vbCrLf + " SALVALUE AS SALVALUE,"
        strSql += vbCrLf + " ISNULL((SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID  = T.SUBITEMID),'')AS SUBITEM,"
        strSql += vbCrLf + " ISNULL((SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = T.DESIGNERID),'')AS DESIGNER,"
        strSql += vbCrLf + " CONVERT(VARCHAR,RECDATE,103) AS RECDATE,"
        strSql += vbCrLf + " CASE WHEN APPROVAL = 'A' THEN 'APPROVAL' WHEN APPROVAL = 'R' THEN 'RESERVED' ELSE NULL END AS STATUS,"
        strSql += vbCrLf + " CASE WHEN APPROVAL = 'A' THEN '" & Color.MistyRose.Name & "' WHEN APPROVAL = 'R' THEN '" & Color.PaleTurquoise.Name & "' ELSE NULL END AS COLOR_HIDE,"
        strSql += vbCrLf + " (SELECT SIZENAME FROM " & cnAdminDb & "..ITEMSIZE WHERE SIZEID = T.SIZEID)AS SIZE"
        strSql += vbCrLf + " FROM"
        strSql += vbCrLf + " " & cnAdminDb & "..ITEMTAG AS T"
        strSql += vbCrLf + " WHERE T.ITEMID = " & Val(RoIss.Item("ITEMID").ToString) & ""
        strSql += RetailBill.ShowTagFiltration()
        strSql += vbCrLf + " AND COSTID = '" & RoIss.Item("COSTID").ToString & "'"
        strSql += vbCrLf + " AND ISSDATE IS NULL"
        strSql += vbcrlf + " ORDER BY TAGNO"
        Dim SelectedRow As DataRow = Nothing
        SelectedRow = BrighttechPack.SearchDialog.Show_R("Find TagNo", strSql, cn, , , , , , , , False)
        If SelectedRow Is Nothing Then
            gridFlag.Focus()
            Exit Sub
        End If
        If SelectedRow.Item("STATUS").ToString <> "" Then
            MsgBox(SelectedRow.Item("STATUS").ToString & " Tag cannot update", MsgBoxStyle.Information)
            gridFlag.Focus()
            Exit Sub
        End If

        strSql = " SELECT RECDATE,PCS,GRSWT,NETWT,TAGNO,ITEMID,RATE,SALVALUE,DESIGNERID,ITEMCTRID,ITEMTYPEID,COSTID,SUBITEMID"
        strSql += vbcrlf + " FROM " & cnAdminDb & "..ITEMTAG "
        strSql += vbcrlf + " WHERE ITEMID = " & Val(RoIss.Item("ITEMID").ToString) & ""
        strSql += vbcrlf + " AND TAGNO = '" & SelectedRow.Item("TAGNO").ToString & "'"
        Dim DtTagInfo As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(DtTagInfo)
        Dim RoTag As DataRow = DtTagInfo.Rows(0)
        If MINUSB4PLUS = False Then
            If Not (CType(RoTag.Item("RECDATE"), Date) <= CType(RoIss.Item("TRANDATE"), Date)) Then
                MsgBox("Invalid Tag Receipt Date" + vbCrLf + "Tag Receipt Date : " & Format(RoTag.Item("RECDATE"), "dd/MM/yyyy") & vbCrLf + "Issue Tran Date : " & Format(RoIss.Item("TRANDATE"), "dd/MM/yyyy"), MsgBoxStyle.Information)
                gridFlag.Focus()
                Exit Sub
            End If
        End If
        If Not (Val(RoTag.Item("PCS").ToString) >= Val(RoIss.Item("PCS").ToString) And Val(RoTag.Item("GRSWT").ToString) >= Val(RoIss.Item("GRSWT").ToString) And Val(RoTag.Item("NETWT").ToString) >= Val(RoIss.Item("NETWT").ToString)) Then
            strSql = "Mismatch Quantity cannot update" + vbCrLf
            strSql += vbcrlf + " Tag's P:" & RoTag.Item("PCS").ToString & " G:" & RoTag.Item("GRSWT").ToString & " N:" & RoTag.Item("NETWT").ToString & vbCrLf
            strSql += vbcrlf + "Issued P:" & RoIss.Item("PCS").ToString & " G:" & RoIss.Item("GRSWT").ToString & " N:" & RoIss.Item("NETWT").ToString
            MsgBox(strSql, MsgBoxStyle.Information)
            gridFlag.Focus()
            Exit Sub
        End If
        If RoIss.Item("FLAG").ToString <> "C" Then
            If MessageBox.Show("This is backoffice entry, this updation will transfer this entry to countersale." + vbCrLf + "Sure You want update?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
                Exit Sub
            End If
        End If
        Dim objSecret As New frmAdminPassword()
        If objSecret.ShowDialog <> Windows.Forms.DialogResult.OK Then
            gridFlag.Focus()
            Exit Sub
        End If
        Try
            tran = cn.BeginTransaction
            strSql = vbCrLf + " INSERT INTO " & cnStockDb & "..HOMESALETRAN"
            strSql += vbCrLf + " ("
            strSql += vbCrLf + " BATCHNO,SNO,USERID,UPDATED,UPTIME,MODE,SUBITEMID,ITEMCTRID,DESIGNERID,ITEMTYPEID,FLAG"
            strSql += vbCrLf + " )"
            strSql += vbCrLf + " VALUES"
            strSql += vbCrLf + " ("
            strSql += vbCrLf + " '" & RoIss.Item("BATCHNO").ToString & "'"
            strSql += vbCrLf + " ,'" & RoIss.Item("SNO").ToString & "'"
            strSql += vbCrLf + " ," & userId & ""
            strSql += vbCrLf + " ,'" & GetServerDate(tran) & "'"
            strSql += vbCrLf + " ,'" & Date.Now.ToShortTimeString & "'"
            strSql += vbCrLf + " ,'HT'"
            strSql += vbCrLf + " ," & Val(RoIss.Item("SUBITEMID").ToString) & ""
            strSql += vbCrLf + " ," & Val(RoIss.Item("ITEMCTRID").ToString) & ""
            strSql += vbCrLf + " ," & Val(RoIss.Item("TAGDESIGNER").ToString) & ""
            strSql += vbCrLf + " ," & Val(RoIss.Item("ITEMTYPEID").ToString) & ""
            strSql += vbCrLf + " ,'" & RoIss.Item("FLAG").ToString & "'"
            strSql += vbCrLf + " )"
            strSql += vbCrLf + " UPDATE " & cnAdminDb & "..ITEMTAG "
            strSql += vbCrLf + " SET ISSDATE = '" & Format(RoIss.Item("TRANDATE"), "yyyy-MM-dd") & "'"
            If MINUSB4PLUS Then strSql += vbCrLf + " ,RECDATE = (CASE WHEN (RECDATE > '" & Format(RoIss.Item("TRANDATE"), "yyyy-MM-dd") & "') THEN '" & Format(RoIss.Item("TRANDATE"), "yyyy-MM-dd") & "' ELSE RECDATE END) "
            strSql += vbCrLf + " ,ISSREFNO = '" & RoIss.Item("TRANNO").ToString & "'"
            strSql += vbCrLf + " ,ISSPCS = " & Val(RoIss.Item("PCS").ToString) & ""
            strSql += vbCrLf + " ,ISSWT = " & Val(RoIss.Item("GRSWT").ToString) & ""
            strSql += vbCrLf + " ,TOFLAG = '" & RoIss.Item("TRANTYPE").ToString & "'"
            strSql += vbCrLf + " ,BATCHNO = '" & RoIss.Item("BATCHNO").ToString & "'"
            strSql += vbCrLf + " ,APPROVAL = ''"
            strSql += vbCrLf + " ,COMPANYID = '" & RoIss.Item("COMPANYID").ToString & "'"
            strSql += vbCrLf + " WHERE ITEMID = '" & Val(RoIss.Item("ITEMID").ToString) & "' AND TAGNO = '" & RoTag.Item("TAGNO").ToString & "'"
            strSql += vbCrLf + " UPDATE " & cnStockDb & "..ISSUE SET SUBITEMID = " & Val(RoTag.Item("SUBITEMID").ToString) & ""
            strSql += vbCrLf + " ,TAGNO = '" & RoTag.Item("TAGNO").ToString & "'"
            strSql += vbCrLf + " ,TAGPCS = " & (RoTag.Item("PCS").ToString)
            strSql += vbCrLf + " ,TAGGRSWT = " & (RoTag.Item("GRSWT").ToString)
            strSql += vbCrLf + " ,TAGNETWT = " & (RoTag.Item("NETWT").ToString)
            strSql += vbCrLf + " ,TAGRATEID = " & (RoTag.Item("RATE").ToString)
            strSql += vbCrLf + " ,TAGSVALUE = " & (RoTag.Item("SALVALUE").ToString)
            strSql += vbCrLf + " ,TAGDESIGNER = " & (RoTag.Item("DESIGNERID").ToString)
            strSql += vbCrLf + " ,ITEMCTRID = " & (RoTag.Item("ITEMCTRID").ToString)
            strSql += vbCrLf + " ,ITEMTYPEID = " & (RoTag.Item("ITEMTYPEID").ToString)
            strSql += vbCrLf + " ,FLAG = 'C'"
            strSql += vbCrLf + " WHERE SNO = '" & RoIss.Item("SNO").ToString & "'"
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, RoIss.Item("COSTID").ToString)
            tran.Commit()
            gridFlag.CurrentRow.Cells("TAGNO").Value = RoTag.Item("TAGNO").ToString
            MsgBox("Tagno updated", MsgBoxStyle.Information)
            gridFlag.Focus()
        Catch ex As Exception
            If tran IsNot Nothing Then
                If tran.Connection IsNot Nothing Then
                    tran.Rollback()
                End If
            End If
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub

    Private Sub cmbMetalName_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbMetalName.SelectedIndexChanged
        strSql = " SELECT 'ALL' ITEMNAME,'ALL' ITEMID,1 RESULT"
        strSql += vbcrlf + " UNION ALL"
        strSql += vbcrlf + " SELECT ITEMNAME,CONVERT(VARCHAR,ITEMID),2 RESULT FROM " & cnAdminDb & "..ITEMMAST "
        If cmbMetalName.Text <> "ALL" Then
            strSql += vbCrLf + " WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME='" & cmbMetalName.Text.ToString() & "')"
        End If
        strSql += vbCrLf + " ORDER BY RESULT,ITEMNAME"
        Dim dtitemname As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtitemname)
        BrighttechPack.GlobalMethods.FillCombo(chkcmbitemname, dtitemname, "ITEMNAME", , "ALL")
    End Sub

    Private Sub AutoSizeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AutoSizeToolStripMenuItem.Click
        If gridFlag.RowCount > 0 Then
            If AutoSizeToolStripMenuItem.Checked Then
                gridFlag.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                gridFlag.Invalidate()
                For Each dgvCol As DataGridViewColumn In gridFlag.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridFlag.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            Else
                For Each dgvCol As DataGridViewColumn In gridFlag.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridFlag.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End If
        End If
    End Sub
End Class

Public Class frmHomeSalesReport_Properties

    Private cmbMetalName As String = "ALL"
    Public Property p_cmbMetalName() As String
        Get
            Return cmbMetalName
        End Get
        Set(ByVal value As String)
            cmbMetalName = value
        End Set
    End Property
    Private chkCompanySelectAll As Boolean = False
    Public Property p_chkCompanySelectAll() As Boolean
        Get
            Return chkCompanySelectAll
        End Get
        Set(ByVal value As Boolean)
            chkCompanySelectAll = value
        End Set
    End Property
    Private chkLstCompany As New List(Of String)
    Public Property p_chkLstCompany() As List(Of String)
        Get
            Return chkLstCompany
        End Get
        Set(ByVal value As List(Of String))
            chkLstCompany = value
        End Set
    End Property
    Private chkGroupItem As Boolean = False
    Public Property p_chkGroupItem() As Boolean
        Get
            Return chkGroupItem
        End Get
        Set(ByVal value As Boolean)
            chkGroupItem = value
        End Set
    End Property

    Private rbtBackOffice As Boolean = True
    Public Property p_rbtBackOffice() As Boolean
        Get
            Return rbtBackOffice
        End Get
        Set(ByVal value As Boolean)
            rbtBackOffice = value
        End Set
    End Property
    Private rbtCounterSales As Boolean = False
    Public Property p_rbtCounterSales() As Boolean
        Get
            Return rbtCounterSales
        End Get
        Set(ByVal value As Boolean)
            rbtCounterSales = value
        End Set
    End Property
    Private rbtBoth As Boolean = False
    Public Property p_rbtBoth() As Boolean
        Get
            Return rbtBoth
        End Get
        Set(ByVal value As Boolean)
            rbtBoth = value
        End Set
    End Property
End Class
