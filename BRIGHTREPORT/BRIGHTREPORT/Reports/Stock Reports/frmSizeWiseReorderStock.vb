Imports System.Data.OleDb
Public Class frmSizeWiseReorderStock
    Dim objGridShower As frmGridDispDia
    Dim StrSql As String = Nothing
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim strFilter As String = Nothing
    Dim dtMetal As New DataTable
    Dim dtCounter As New DataTable
    Dim dtItem As New DataTable
    Dim dtSubItem As New DataTable
    Dim dtCostCentre As New DataTable
    Dim dtSize As New DataTable
    Dim designReorderFix As Boolean = IIf(GetAdmindbSoftValue("REORDERFIX", "Y") = "Y", True, False)

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Dim dt As New DataTable
        pnlView.Dock = DockStyle.Fill
        pnlView.Visible = False
        grpControls.BringToFront()
        cmbMetal.Focus()
        ''COSTCENTRE
        StrSql = vbCrLf + " SELECT 'ALL' COSTNAME,'ALL'COSTID,1 RESULT"
        StrSql += vbCrLf + " UNION ALL"
        StrSql += vbCrLf + " SELECT COSTNAME,COSTID,2 RESULT FROM " & CNADMINDB & "..COSTCENTRE"
        StrSql += vbCrLf + " ORDER BY RESULT,COSTNAME"
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(dtCostCentre)
        cmbCostCentre.Items.Clear()
        For Each Row As DataRow In dtCostCentre.Rows
            cmbCostCentre.Items.Add(Row.Item("COSTNAME").ToString)
        Next
        If GetAdmindbSoftValue("COSTCENTRE", "N") = "Y" Then
            cmbCostCentre.Enabled = True
            cmbCostCentre.Text = IIf(cnDefaultCostId, "ALL", cnCostName)
        Else
            cmbCostCentre.Enabled = False
        End If

        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then cmbCostCentre.Enabled = False

        '' LOAD METAL
        StrSql = vbCrLf + " SELECT 'ALL' METALNAME,'ALL' METALID,1 RESULT"
        StrSql += vbCrLf + " UNION ALL"
        StrSql += vbCrLf + " SELECT METALNAME,CONVERT(VARCHAR,METALID),2 RESULT FROM " & cnAdminDb & "..METALMAST"
        StrSql += vbCrLf + " ORDER BY RESULT,METALNAME"
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(dtMetal)
        cmbMetal.Items.Clear()
        For Each Row As DataRow In dtMetal.Rows
            cmbMetal.Items.Add(Row.Item("METALNAME").ToString)
        Next
        '' LOAD COUNTER
        StrSql = vbCrLf + " SELECT 'ALL' ITEMCTRNAME,'ALL' ITEMCTRID,1 RESULT"
        StrSql += vbCrLf + " UNION ALL"
        StrSql += vbCrLf + " SELECT ITEMCTRNAME,CONVERT(VARCHAR,ITEMCTRID),2 RESULT FROM " & cnAdminDb & "..ITEMCOUNTER"
        StrSql += vbCrLf + " ORDER BY RESULT,ITEMCTRNAME"
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(dtCounter)
        cmbCounter.Items.Clear()
        For Each Row As DataRow In dtCounter.Rows
            cmbCounter.Items.Add(Row.Item("ITEMCTRNAME").ToString)
        Next

        '' LOAD SIZE
        StrSql = vbCrLf + " SELECT 'ALL' SIZENAME,'ALL'SIZEID,1 RESULT"
        StrSql += vbCrLf + " UNION ALL"
        StrSql += vbCrLf + " SELECT SIZENAME,CONVERT(VARCHAR,SIZEID),2 RESULT FROM " & cnAdminDb & "..ITEMSIZE"
        StrSql += vbCrLf + " ORDER BY RESULT,SIZENAME"
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(dtSize)
        cmbSize.Items.Clear()
        For Each Row As DataRow In dtSize.Rows
            cmbSize.Items.Add(Row.Item("SIZENAME").ToString)
        Next

        'StrSql = " select "
        'StrSql += " distinct(select itemName from " & cnAdminDb & "..itemMast where itemid = s.itemId)itemName"
        'StrSql += " from " & cnAdminDb & "..stkreOrder as s order by itemName"
        'cmbItemName.Items.Clear()
        'cmbItemName.Items.Add("ALL")
        'objGPack.FillCombo(StrSql, cmbItemName, False)
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub frmSizeWiseReorderStock_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Me.WindowState = FormWindowState.Maximized
        Me.tabMain.ItemSize = New Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGen.Left, Me.tabGen.Top, Me.tabGen.Width, Me.tabGen.Height))
        Me.tabMain.SelectedTab = tabGen
        cmbCounter.SelectedIndex = -1
        cmbCounter.Select()
        btnNew_Click(Me, e)
    End Sub

    Function funcFiltration() As String
        strFilter = "where 1=1 "
        If chkCmbItem.Text <> "ALL" And Trim(chkCmbItem.Text) <> "" Then
            strFilter += " and r.itemId = (select itemId from " & cnAdminDb & "..itemMast where itemName = '" & chkCmbItem.Text & "')"
        End If
        If cmbSubItemName.Text <> "ALL" And Trim(cmbSubItemName.Text) <> "" Then
            strFilter += " and r.subItemId = (Select subItemId from " & cnAdminDb & "..subitemMast where subitemName = '" & cmbSubItemName.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & chkCmbItem.Text & "'))"
        End If
        If cmbCostCentre.Enabled = True Then
            If cmbCostCentre.Text <> "ALL" And Trim(cmbCostCentre.Text) = "" Then
                strFilter += " and r.COSTID = (select CostId from " & cnAdminDb & "..CostCentre where CostName = '" & cmbCostCentre.Text & "')"
            End If
        End If
        Return strFilter
    End Function

    Private Sub NewReport()
        If Not CheckBckDays(userId, Me.Name, dtpAsOnDate.Value) Then dtpAsOnDate.Focus() : Exit Sub
        Dim selMetalId As String = Nothing
        Dim selItemId As String = Nothing
        Dim selSubItemId As String = Nothing
        Dim selCostId As String = Nothing
        Dim selCounterId As String = Nothing
        Dim selSizeid As String = Nothing
        Dim rType As String = Nothing

        If chkCmbItem.Text = "ALL" Then
            selItemId = "ALL"
        ElseIf chkCmbItem.Text <> "" Then
            Dim sql As String = "SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & GetQryString(chkCmbItem.Text) & ")"
            Dim dtItem As New DataTable()
            da = New OleDbDataAdapter(sql, cn)
            da.Fill(dtItem)
            If dtItem.Rows.Count > 0 Then
                'selItemId = "'"
                For i As Integer = 0 To dtItem.Rows.Count - 1
                    selItemId += dtItem.Rows(i).Item("ITEMID").ToString + ","
                    'selItemId += dtItem.Rows(i).Item("ITEMID").ToString
                Next
                If selItemId <> "" Then
                    selItemId = Mid(selItemId, 1, selItemId.Length - 1)
                End If
                'selItemId += "'"
            End If
        End If

        If cmbCounter.Text = "ALL" Then
            selCounterId = "ALL"
        ElseIf cmbCounter.Text <> "" Then
            Dim sql As String = "SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME IN (" & GetQryString(cmbCounter.Text) & ")"
            Dim dtctr As New DataTable()
            da = New OleDbDataAdapter(sql, cn)
            da.Fill(dtctr)
            If dtctr.Rows.Count > 0 Then
                'selItemId = "'"
                For i As Integer = 0 To dtctr.Rows.Count - 1
                    selCounterId += dtctr.Rows(i).Item("ITEMCTRID").ToString + ","
                    'selItemId += dtItem.Rows(i).Item("ITEMID").ToString
                Next
                If selCounterId <> "" Then
                    selCounterId = Mid(selCounterId, 1, selCounterId.Length - 1)
                End If
                'selItemId += "'"
            End If
        End If
        If cmbCostCentre.Text = "ALL" Then
            selCostId = "ALL"
        ElseIf cmbCostCentre.Text <> "" Then
            Dim sql As String = "SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & GetQryString(cmbCostCentre.Text) & ")"
            Dim dtItem As New DataTable()
            da = New OleDbDataAdapter(sql, cn)
            da.Fill(dtItem)
            If dtItem.Rows.Count > 0 Then

                For i As Integer = 0 To dtItem.Rows.Count - 1
                    selCostId += dtItem.Rows(i).Item("COSTID").ToString + ","
                Next
                If selCostId <> "" Then
                    selCostId = Mid(selCostId, 1, selCostId.Length - 1)
                End If
                'selItemId += "'"
            End If
        End If
        If cmbMetal.Text <> "ALL" Then
            selMetalId = objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME='" & cmbMetal.Text.Trim & "'", , , )
        Else
            selMetalId = "ALL"
        End If

        If cmbSubItemName.SelectedIndex >= 0 Then
            selSubItemId = dtSubItem.Rows(cmbSubItemName.SelectedIndex).Item("SUBITEMID").ToString
        Else
            selSubItemId = "ALL"
        End If
        If cmbSize.SelectedIndex >= 0 Then
            selSizeid = dtSize.Rows(cmbSize.SelectedIndex).Item("SIZEID").ToString
        Else
            selSizeid = "ALL"
        End If
        If rbtExcess.Checked Then
            rType = "E"
        ElseIf rbtShort.Checked Then
            rType = "S"
        Else
            rType = "B"
        End If            
        StrSql = vbCrLf + "IF OBJECT_ID('TEMPTABLEDB..TBLRSTK02', 'U') IS NOT NULL DROP TABLE TEMPTABLEDB..TBLRSTK02"
        cmd = New OleDbCommand(StrSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        StrSql = vbCrLf + "SELECT  CONVERT(VARCHAR(50),'')ITEMNAMES,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID= RE.ITEMID)ITEMNAME"
        StrSql += vbCrLf + ",(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID= RE.SUBITEMID)SUBITEMNAME"
        StrSql += vbCrLf + ",(SELECT SIZENAME FROM " & cnAdminDb & "..ITEMSIZE WHERE SIZEID= RE.SIZEID)SIZENAME"
        StrSql += vbCrLf + " ,RE.PIECE REPCS,RE.WEIGHT AS REWT,CONVERT(INT,SUM(T.PCS))STKPCS,CONVERT(NUMERIC(15,3),SUM(T.GRSWT))STKWT"
        StrSql += vbCrLf + " ,CONVERT(INT,ISNULL(SUM(T.PCS),0)- ISNULL(RE.PIECE,0)) AS DIFFPCS"
        StrSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),ISNULL(SUM(T.GRSWT),0) - ISNULL(RE.WEIGHT,0)) AS DIFFWT"
        StrSql += vbCrLf + " ,CONVERT(INT,2) AS RESULT,CONVERT(VARCHAR(3),NULL)COLHEAD "
        StrSql += vbCrLf + " INTO TEMPTABLEDB..TBLRSTK02"
        StrSql += vbCrLf + " FROM " & cnAdminDb & "..STKREORDER AS RE"
        StrSql += vbCrLf + " LEFT OUTER JOIN " & cnAdminDb & "..ITEMTAG AS T ON T.RECDATE <='" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "' AND (T.ISSDATE IS NULL OR T.ISSDATE >='" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "')"
        StrSql += vbCrLf + " AND T.ITEMID = RE.ITEMID AND T.SIZEID=RE.SIZEID AND T.COMPANYID = '" & GetStockCompId() & "' WHERE 1 = 1 "

        If Trim(selItemId) <> "ALL" Then
            StrSql += vbCrLf + " AND RE.ITEMID IN (" & selItemId & ") "
        End If
        If Trim(selSizeid) <> "ALL" Then
            StrSql += vbCrLf + " AND RE.SIZEID IN(" & selSizeid & ")"
        End If

        StrSql += vbCrLf + " GROUP BY RE.ITEMID,RE.SUBITEMID,RE.PIECE,RE.WEIGHT,RE.SIZEID ORDER BY RE.ITEMID "
        cmd = New OleDbCommand(StrSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        StrSql = vbCrLf + " INSERT INTO  TEMPTABLEDB..TBLRSTK02(ITEMNAMES,ITEMNAME,RESULT,COLHEAD)"
        StrSql += vbCrLf + " SELECT DISTINCT ITEMNAME,ITEMNAME,0 RESULT,'T' COLHEAD FROM TEMPTABLEDB..TBLRSTK02"
        cmd = New OleDbCommand(StrSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        StrSql = vbCrLf + " INSERT INTO  TEMPTABLEDB..TBLRSTK02(ITEMNAMES,ITEMNAME,SUBITEMNAME,RESULT,COLHEAD)"
        StrSql += vbCrLf + " SELECT DISTINCT ITEMNAME,ITEMNAME,SUBITEMNAME,1 RESULT,'T1' COLHEAD FROM TEMPTABLEDB..TBLRSTK02 WHERE ISNULL(SUBITEMNAME,'')=SUBITEMNAME "
        cmd = New OleDbCommand(StrSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        StrSql = vbCrLf + "  INSERT INTO  TEMPTABLEDB..TBLRSTK02(ITEMNAMES,SUBITEMNAME, ITEMNAME,STKPCS,"
        StrSql += vbCrLf + " STKWT,RESULT,COLHEAD)"
        StrSql += vbCrLf + " SELECT  'SUB TOTAL','ZZZZZZ',ITEMNAME,SUM(STKPCS),SUM(STKWT),3 RESULT,'S' "
        StrSql += vbCrLf + " COLHEAD FROM TEMPTABLEDB..TBLRSTK02 GROUP BY ITEMNAME"
        cmd = New OleDbCommand(StrSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        StrSql = vbCrLf + "  UPDATE UPTB SET UPTB.ITEMNAMES=UPTB.SIZENAME  FROM TEMPTABLEDB..TBLRSTK02 "
        StrSql += vbCrLf + " UPTB,TEMPTABLEDB..TBLRSTK02 FTTB "
        StrSql += vbCrLf + " WHERE UPTB.ITEMNAME+UPTB.SIZENAME=FTTB.ITEMNAME+FTTB.SIZENAME"
        cmd = New OleDbCommand(StrSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()


        StrSql = vbCrLf + "  UPDATE UPTB SET UPTB.ITEMNAMES=UPTB.SUBITEMNAME  FROM"
        StrSql += vbCrLf + " TEMPTABLEDB..TBLRSTK02 UPTB,TEMPTABLEDB..TBLRSTK02 FTTB "
        StrSql += vbCrLf + " WHERE UPTB.ITEMNAME+UPTB.SUBITEMNAME=FTTB.ITEMNAME+FTTB.SUBITEMNAME AND UPTB.RESULT=1"
        cmd = New OleDbCommand(StrSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()


        StrSql = vbCrLf + " INSERT INTO  TEMPTABLEDB..TBLRSTK02(ITEMNAMES,ITEMNAME,STKPCS,STKWT,RESULT,COLHEAD)"
        StrSql += vbCrLf + " SELECT  'GRAND TOTAL','ZZZZZ',SUM(STKPCS),SUM(STKWT),"
        StrSql += vbCrLf + " 4 RESULT,'G' COLHEAD FROM TEMPTABLEDB..TBLRSTK02 WHERE RESULT=2"
        cmd = New OleDbCommand(StrSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        StrSql = " SELECT * FROM TEMPTABLEDB..TBLRSTK02 ORDER BY ITEMNAME,SUBITEMNAME,RESULT,SIZENAME"



        'StrSql = vbCrLf + "SELECT  CONVERT(VARCHAR(50),'')ITEMNAMES,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID= RE.ITEMID)ITEMNAME,RE.SIZENAME"
        'StrSql += vbCrLf + " ,RE.REORDPCS REPCS,RE.REORDWT AS REWT,CONVERT(INT,SUM(T.PCS))STKPCS,CONVERT(NUMERIC(15,3),SUM(T.GRSWT))STKWT"
        'StrSql += vbCrLf + " ,CONVERT(INT,ISNULL(SUM(T.PCS),0)- ISNULL(RE.REORDPCS,0)) AS DIFFPCS"
        'StrSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),ISNULL(SUM(T.GRSWT),0) - ISNULL(RE.REORDWT,0)) AS DIFFWT"
        'StrSql += vbCrLf + " ,CONVERT(INT,1) AS RESULT,CONVERT(VARCHAR(3),NULL)COLHEAD "
        'StrSql += vbCrLf + " INTO TEMPTABLEDB..TBLRSTK02"
        'StrSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMSIZE AS RE"
        'StrSql += vbCrLf + " LEFT OUTER JOIN " & cnAdminDb & "..ITEMTAG AS T ON T.RECDATE <='" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "' AND (T.ISSDATE IS NULL OR T.ISSDATE >='" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "')"
        'StrSql += vbCrLf + " AND T.ITEMID = RE.ITEMID AND T.SIZEID=RE.SIZEID AND T.COMPANYID = '" & GetStockCompId() & "' "
        'If Trim(selItemId) <> "ALL" Then
        '    StrSql += vbCrLf + " WHERE RE.ITEMID IN (" & selItemId & ") "
        'End If
        'If Trim(selSizeid) <> "ALL" Then
        '    StrSql += vbCrLf + " AND RE.SIZEID IN(" & selSizeid & ")"
        'End If
        'StrSql += vbCrLf + " GROUP BY RE.ITEMID,RE.REORDPCS,RE.REORDWT,RE.SIZENAME ORDER BY RE.ITEMID"
        'cmd = New OleDbCommand(StrSql, cn) : cmd.CommandTimeout = 1000
        'cmd.ExecuteNonQuery()
        'StrSql = vbCrLf + " INSERT INTO  TEMPTABLEDB..TBLRSTK02(ITEMNAMES,ITEMNAME,RESULT,COLHEAD)"
        'StrSql += vbCrLf + " SELECT DISTINCT ITEMNAME,ITEMNAME,0 RESULT,'T' COLHEAD FROM TEMPTABLEDB..TBLRSTK02"
        'cmd = New OleDbCommand(StrSql, cn) : cmd.CommandTimeout = 1000
        'cmd.ExecuteNonQuery()
        'StrSql = vbCrLf + " INSERT INTO  TEMPTABLEDB..TBLRSTK02(ITEMNAMES,ITEMNAME,STKPCS,STKWT,RESULT,COLHEAD)"
        'StrSql += vbCrLf + " SELECT  'SUB TOTAL',ITEMNAME,SUM(STKPCS),SUM(STKWT),2 RESULT,'S' COLHEAD FROM TEMPTABLEDB..TBLRSTK02 GROUP BY ITEMNAME"
        'cmd = New OleDbCommand(StrSql, cn) : cmd.CommandTimeout = 1000
        'cmd.ExecuteNonQuery()
        'StrSql = vbCrLf + " UPDATE UPTB SET UPTB.ITEMNAMES=UPTB.SIZENAME  FROM TEMPTABLEDB..TBLRSTK02 UPTB,TEMPTABLEDB..TBLRSTK02 FTTB WHERE UPTB.ITEMNAME+UPTB.SIZENAME=FTTB.ITEMNAME+FTTB.SIZENAME"
        'cmd = New OleDbCommand(StrSql, cn) : cmd.CommandTimeout = 1000
        'cmd.ExecuteNonQuery()
        'StrSql = vbCrLf + " INSERT INTO  TEMPTABLEDB..TBLRSTK02(ITEMNAMES,ITEMNAME,STKPCS,STKWT,RESULT,COLHEAD)"
        'StrSql += vbCrLf + " SELECT  'GRAND TOTAL','ZZZZZ',SUM(STKPCS),SUM(STKWT),3 RESULT,'G' COLHEAD FROM TEMPTABLEDB..TBLRSTK02 WHERE RESULT=1"
        'cmd = New OleDbCommand(StrSql, cn) : cmd.CommandTimeout = 1000
        'cmd.ExecuteNonQuery()
        'StrSql = vbCrLf + " SELECT * FROM TEMPTABLEDB..TBLRSTK02 ORDER BY ITEMNAME,RESULT,SIZENAME"


        Dim DtGrid As New DataTable("SUMMARY")
        DtGrid.Columns.Add("KEYNO", GetType(Integer))
        DtGrid.Columns("KEYNO").AutoIncrement = True
        DtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        DtGrid.Columns("KEYNO").AutoIncrementStep = 1
        cmd = New OleDbCommand(StrSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        da = New OleDbDataAdapter(cmd)
        da.Fill(DtGrid)
        If Not DtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        Dim dtMergeHeader As New DataTable()
        dtMergeHeader = New DataTable("~MERGEHEADER")
        With dtMergeHeader

            .Columns.Add("ITEMNAMES", GetType(String))
            .Columns.Add("REPCS~REWT", GetType(String))
            .Columns.Add("STKPCS~STKWT", GetType(String))
            .Columns.Add("DIFFPCS~DIFFWT", GetType(String))
            .Columns("ITEMNAMES").Caption = "DESCRIPTION"
            .Columns("REPCS~REWT").Caption = "REORDER STOCK "
            .Columns("STKPCS~STKWT").Caption = "AVAILABLE STOCK"
            .Columns("DIFFPCS~DIFFWT").Caption = "DIFFERENCE"
            .Columns.Add("SCROLL", GetType(String))
            .Columns("SCROLL").Caption = ""

        End With
        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        AddHandler objGridShower.gridView.KeyDown, AddressOf DataGridView_KeyDown
        Dim tit As String = ""
        objGridShower.Text = "STOCK REORDER"
        tit = "SIZE WISE REORDER STOCK" + vbCrLf
        tit += " AS ON " & dtpAsOnDate.Text & ""
        tit += IIf(cmbCostCentre.Text <> "" And cmbCostCentre.Text <> "ALL", " :" & cmbCostCentre.Text, cmbCostCentre.Text)
        objGridShower.lblTitle.Text = tit
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(dtMergeHeader)
        objGridShower.dsGrid.Tables.Add(DtGrid)
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(1)
        objGridShower.gridViewHeader.DataSource = objGridShower.dsGrid.Tables(0)
        objGridShower.FormReSize = False
        objGridShower.FormReLocation = False
        objGridShower.pnlFooter.Visible = False
        objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        DataGridView_SummaryFormatting(objGridShower.gridView)
        FormatGridColumns(objGridShower.gridView, False, False, , False)
        objGridShower.Show()
        objGridShower.FormReSize = True
        objGridShower.FormReLocation = True
        objGridShower.pnlFooter.Visible = True
        objGridShower.lblStatus.Text = ""
        FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
        objGridShower.gridView_ColumnWidthChanged(Me, New DataGridViewColumnEventArgs(objGridShower.gridView.Columns("ITEMNAME")))
        objGridShower.gridViewHeader.Visible = True
        GridHead()
    End Sub

    Private Sub GridHead()
        With objGridShower.gridView
            objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle = .ColumnHeadersDefaultCellStyle
            objGridShower.gridViewHeader.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised
            .ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Sunken
            'objGridShower.gridViewHeader.Columns("COL1").Width = .Columns("COL1").Width
            'objGridShower.gridViewHeader.Columns("COL1").HeaderText = ""
            Dim TEMPCOLWIDTH As Integer = 0
            TEMPCOLWIDTH += .Columns("ITEMNAMES").Width
            objGridShower.gridViewHeader.Columns("ITEMNAMES").Width = TEMPCOLWIDTH
            objGridShower.gridViewHeader.Columns("ITEMNAMES").HeaderText = "DESCRIPTION"
            TEMPCOLWIDTH = 0
            TEMPCOLWIDTH += .Columns("REPCS").Width + .Columns("REWT").Width
            objGridShower.gridViewHeader.Columns("REPCS~REWT").Width = TEMPCOLWIDTH
            objGridShower.gridViewHeader.Columns("REPCS~REWT").HeaderText = "REORDER STOCK "
            TEMPCOLWIDTH = 0
            TEMPCOLWIDTH += .Columns("STKPCS").Width + .Columns("STKWT").Width
            objGridShower.gridViewHeader.Columns("STKPCS~STKWT").Width = TEMPCOLWIDTH
            objGridShower.gridViewHeader.Columns("STKPCS~STKWT").HeaderText = "AVAILABLE STOCK"
            TEMPCOLWIDTH = 0
            TEMPCOLWIDTH += .Columns("DIFFPCS").Width + .Columns("DIFFWT").Width
            objGridShower.gridViewHeader.Columns("DIFFPCS~DIFFWT").Width = TEMPCOLWIDTH
            objGridShower.gridViewHeader.Columns("DIFFPCS~DIFFWT").HeaderText = "DIFFERENCE"
            objGridShower.gridViewHeader.Columns("SCROLL").Width = TEMPCOLWIDTH
            objGridShower.gridViewHeader.Columns("SCROLL").HeaderText = ""
            Dim colWid As Integer = 0
            For cnt As Integer = 0 To .ColumnCount - 1
                If .Columns(cnt).Visible Then colWid += .Columns(cnt).Width
                If rdbWeight.Checked Then
                    If UCase(Mid(.Columns(cnt).Name, Len(.Columns(cnt).Name) - 2)) = "PCS" Then .Columns(cnt).HeaderText = "PCS"
                    If UCase(Mid(.Columns(cnt).Name, Len(.Columns(cnt).Name) - 1)) = "WT" Then .Columns(cnt).HeaderText = "Weight"
                ElseIf rdbRate.Checked Then
                    If UCase(Mid(.Columns(cnt).Name, Len(.Columns(cnt).Name) - 2)) = "PCS" Then .Columns(cnt).HeaderText = "PCS"
                    If UCase(Mid(.Columns(cnt).Name, Len(.Columns(cnt).Name) - 1)) = "WT" Then .Columns(cnt).HeaderText = "Rate"
                End If
            Next          
        End With
    End Sub
    Private Sub DataGridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        Dim dt As New DataTable()
        If e.KeyCode = Keys.Escape Then
            Dim dgv As DataGridView = CType(sender, DataGridView)
            If Not dgv.RowCount > 0 Then Exit Sub
            If CType(dgv.DataSource, DataTable).TableName = "DETAILED" Then
                Dim f As frmGridDispDia
                f = objGPack.GetParentControl(dgv)
                f.gridViewHeader.Visible = False
                f.FormReSize = False
                f.gridView.DataSource = Nothing
                f.gridView.DataSource = f.dsGrid.Tables("SUMMARY")
                f.gridView.CurrentCell = f.gridView.FirstDisplayedCell
                DataGridView_SummaryFormatting(f.gridView)
                f.Text = "STOCK REORDER"
                Dim tit As String = "RANGE WISE REORDER STOCK" + vbCrLf
                tit += " AS ON " & dtpAsOnDate.Text & ""
                f.lblTitle.Text = tit
                'f.lblStatus.Text = "<Press [D] for Detail View>"
                f.FormReSize = True
                f.gridView.Columns(0).Width = f.gridView.Columns(0).Width + 1
            End If
        ElseIf e.KeyCode = Keys.D Then
            Dim dgv As DataGridView = CType(sender, DataGridView)
            Dim ItemName As String = dgv.CurrentRow.Cells("Item").Value.ToString
            Dim SubItem As String = dgv.CurrentRow.Cells("SubItemName").Value.ToString
            Dim FromWeight As String = dgv.CurrentRow.Cells("FromWeight").Value.ToString
            Dim ToWeight As String = dgv.CurrentRow.Cells("ToWeight").Value.ToString
            Dim SizeName As String = ""
            If chkSize.Checked = True Then SizeName = dgv.CurrentRow.Cells("SizeName").Value.ToString
            dgv.CurrentCell = dgv.Rows(dgv.CurrentRow.Index).Cells("RANGE")
            If dgv.CurrentRow.Cells("RANGE").Value = "OTHER RANGE" Then
                dt = FillOtherRangeDetailView(ItemName, SubItem, FromWeight, ToWeight, SizeName)
            Else
                dt = FillDetailView(ItemName, SubItem, FromWeight, ToWeight, SizeName)
            End If
            Dim f As frmGridDispDia
            f = objGPack.GetParentControl(dgv)
            If Not dt.Rows.Count > 0 Then
                MsgBox("Record not found", MsgBoxStyle.Information)
                Exit Sub
            End If
            If f.dsGrid.Tables.Contains(dt.TableName) Then f.dsGrid.Tables.Remove(dt.TableName)
            f.Text = "STOCK REORDER DETAILED"
            Dim tit As String = "STOCK REORDER DETAILED AS ON " & dtpAsOnDate.Text + vbCrLf
            tit += "(" + ItemName + ")" + "(" + SubItem + ")" + vbCrLf
            f.lblTitle.Text = tit
            f.dsGrid.Tables.Add(dt)
            f.FormReSize = False
            f.FormReLocation = True
            f.gridView.DataSource = Nothing
            f.gridView.DataSource = f.dsGrid.Tables("DETAILED")
            f.gridView.CurrentCell = f.gridView.FirstDisplayedCell
            DataGridView_DetailViewFormatting(f.gridView)
            f.FormReSize = True
            f.gridView.Columns(0).Width = f.gridView.Columns(0).Width + 1
            f.gridView.CurrentCell = f.gridView.FirstDisplayedCell
            f.gridView.Select()
        End If
    End Sub
    Private Function FillDetailView(ByVal ItemName As String, ByVal SubItem As String, ByVal FromWt As Double, ByVal ToWt As Double, ByVal SizeName As String) As DataTable
        Dim strSql As String = " SELECT TAGNO,T.PCS AS STKPCS,"
        If rdbWeight.Checked = True Then
            strSql += " T.GRSWT AS STKWT,"
        ElseIf rdbRate.Checked = True Then
            strSql += " T.GRSWT AS STKRT,"
        End If
        strSql += " T.DESIGNERID AS DESIGNER"
        strSql += " FROM " & cnAdminDb & "..ITEMTAG AS T  LEFT OUTER JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID = T.ITEMID  "
        strSql += " LEFT OUTER JOIN " & cnAdminDb & "..SUBITEMMAST SM ON SM.ITEMID = T.ITEMID AND SM.SUBITEMID =T.SUBITEMID  "
        If chkSize.Checked = True Then
            If SizeName <> "" Then strSql += " LEFT OUTER JOIN " & cnAdminDb & "..ITEMSIZE AS SI ON SI.SIZEID = T.SIZEID  "
        End If
        strSql += " WHERE T.RECDATE <= '" & dtpAsOnDate.Value.Date.ToString("yyyy/MM/dd") & "' AND (T.ISSDATE IS NULL OR T.ISSDATE > '" & dtpAsOnDate.Value.Date.ToString("yyyy/MM/dd") & "')"
        strSql += " AND EXISTS (SELECT 1 FROM " & cnAdminDb & "..STKREORDER RE WHERE T.ITEMID = RE.ITEMID AND T.SUBITEMID = RE.SUBITEMID AND T.COSTID=RE.COSTID AND FROMWEIGHT='" & FromWt & "' AND TOWEIGHT='" & ToWt & "' AND T.GRSWT BETWEEN FROMWEIGHT AND TOWEIGHT  AND ISNULL(T.SIZEID,0)= (CASE WHEN ISNULL(RE.SIZEID,0) <>0 THEN RE.SIZEID ELSE 0 END))"
        strSql += " AND T.GRSWT BETWEEN " & FromWt & " AND " & ToWt & ""
        strSql += " AND T.ITEMID=(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & ItemName & "') "
        If SubItem <> "" Then
            strSql += " AND T.SUBITEMID =(SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME='" & SubItem & "' AND ITEMID=(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & ItemName & "')) "
        Else
            strSql += " AND ISNULL(T.SUBITEMID,0) =0"
        End If

        If cmbCostCentre.Text <> "ALL" And cmbCostCentre.Text <> "" Then strSql += " AND T.COSTID=(SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME='" & cmbCostCentre.Text & "')"
        If chkSize.Checked = True Then
            If SizeName <> "" Then
                strSql += " AND SI.SIZENAME ='" & SizeName & "'"
            Else
                strSql += vbCrLf + " AND ISNULL(T.SIZEID,0) =0"
            End If


        End If
        strSql += " GROUP BY TAGNO,PCS,T.GRSWT,DESIGNERID"

        Dim dt As New DataTable()
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        Dim dtGrid As New DataTable("DETAILED")
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        Return dtGrid
    End Function
    Private Function FillOtherRangeDetailView(ByVal ItemName As String, ByVal SubItem As String, ByVal FromWt As Double, ByVal ToWt As Double, ByVal SizeName As String) As DataTable

        Dim strSql As String = " SELECT TAGNO,T.PCS AS STKPCS,"
        If rdbWeight.Checked = True Then
            strSql += vbCrLf + " T.GRSWT AS STKWT,"
        ElseIf rdbRate.Checked = True Then
            strSql += vbCrLf + " T.GRSWT AS STKRT,"
        End If
        strSql += vbCrLf + " T.DESIGNERID AS DESIGNER"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T  LEFT OUTER JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID = T.ITEMID  "
        strSql += vbCrLf + " LEFT OUTER JOIN " & cnAdminDb & "..SUBITEMMAST SM ON SM.ITEMID = T.ITEMID AND SM.SUBITEMID =T.SUBITEMID  "
        If chkSize.Checked = True Then
            If SizeName <> "" Then strSql += vbCrLf + " LEFT OUTER JOIN " & cnAdminDb & "..ITEMSIZE AS SI ON SI.SIZEID = T.SIZEID  "
        End If
        strSql += vbCrLf + " WHERE T.RECDATE <= '" & dtpAsOnDate.Value.Date.ToString("yyyy/MM/dd") & "' AND (T.ISSDATE IS NULL OR T.ISSDATE > '" & dtpAsOnDate.Value.Date.ToString("yyyy/MM/dd") & "')"
        strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..STKREORDER RE WHERE T.ITEMID = RE.ITEMID AND T.SUBITEMID = RE.SUBITEMID AND T.COSTID=RE.COSTID AND  T.GRSWT BETWEEN FROMWEIGHT AND TOWEIGHT AND ISNULL(T.SIZEID,0)= (CASE WHEN ISNULL(RE.SIZEID,0) <>0 THEN RE.SIZEID ELSE 0 END))"
        strSql += vbCrLf + " AND T.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & ItemName & "') "
        If SubItem <> "" Then
            strSql += vbCrLf + " AND T.SUBITEMID IN(SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME='" & SubItem & "' AND ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & ItemName & "')) "
        Else
            strSql += vbCrLf + " AND ISNULL(T.SUBITEMID,0)=0"
        End If

        If cmbCostCentre.Text <> "ALL" And cmbCostCentre.Text <> "" Then strSql += vbCrLf + " AND T.COSTID =(SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME='" & cmbCostCentre.Text & "')"
        If chkSize.Checked = True Then
            If SizeName <> "" Then
                strSql += vbCrLf + " AND SI.SIZENAME ='" & SizeName & "'"
            Else
                strSql += vbCrLf + " AND ISNULL(T.SIZEID,0) =0"
            End If

        End If
        strSql += vbCrLf + " GROUP BY TAGNO,PCS,T.GRSWT,DESIGNERID"

        Dim dt As New DataTable()
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        Dim dtGrid As New DataTable("DETAILED")
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        Return dtGrid
    End Function
    Private Sub DataGridView_DetailViewFormatting(ByVal dgv As DataGridView)
        With dgv
            '.Columns("ITEMNAME").Width = 250
            '.Columns("SUBITEMNAME").Width = 300
            .Columns("TAGNO").Width = 150
            .Columns("STKPCS").Width = 80
            If rdbWeight.Checked = True Then
                .Columns("STKWT").Width = 120
            ElseIf rdbRate.Checked = True Then
                .Columns("STKRT").Width = 120
            End If
            FormatGridColumns(dgv, False, False, , False)
            FillGridGroupStyle_KeyNoWise(dgv)
        End With
    End Sub


    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView)
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                'dgvCol.Visible = False
                dgvCol.SortMode = DataGridViewColumnSortMode.NotSortable
            Next
            .Columns("ITEMNAMES").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns("STKPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("STKWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("REPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("REWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("DIFFPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("DIFFWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("ITEMNAMES").HeaderText = "ITEMNAME"
            .Columns("SIZENAME").Visible = False
            .Columns("ITEMNAME").Visible = False
            .Columns("SUBITEMNAME").Visible = False
            .Columns("STKPCS").Visible = True
            .Columns("STKWT").Visible = True
            .Columns("REPCS").Visible = True
            .Columns("REWT").Visible = True
            .Columns("DIFFPCS").Visible = True
            .Columns("DIFFWT").Visible = True
            .Columns("RESULT").Visible = False
            .Columns("COLHEAD").Visible = False
            .Columns("KEYNO").Visible = False
            FillGridGroupStyle_KeyNoWise(dgv)
        End With
    End Sub


    Private Sub btnView_SearchClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        NewReport()
        Exit Sub
        Try
            'Me.Cursor = Cursors.WaitCursor
            StrSql = " IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "REORDERREP')"
            StrSql += vbCrLf + "  DROP TABLE TEMP" & systemId & "REORDERREP"
            cmd = New OleDbCommand(StrSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            StrSql = vbCrLf + "  CREATE TABLE TEMP" & systemId & "REORDERREP("
            StrSql += vbCrLf + "  PARTICULAR VARCHAR(500), "
            If chkSize.Checked Then
                StrSql += vbCrLf + " SIZE VARCHAR(75),"
            End If
            StrSql += vbCrLf + "  RANGE NUMERIC(15,3), "
            StrSql += vbCrLf + "  PIECE INT,  "
            StrSql += vbCrLf + "  WEIGHT NUMERIC(15,3), "
            StrSql += vbCrLf + "  STKPCS INT,"
            StrSql += vbCrLf + "  STKWEIGHT NUMERIC(15,3), "
            StrSql += vbCrLf + "  DIFFPCS INT, "
            StrSql += vbCrLf + "  DIFFWEIGHT NUMERIC(15,3), "
            StrSql += vbCrLf + "  RESULT INT,"
            StrSql += vbCrLf + "  COLHEAD VARCHAR(1),"
            StrSql += vbCrLf + "  SNO INT IDENTITY)"
            cmd = New OleDbCommand(StrSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            Dim ftrSql As String = funcFiltration()
            StrSql = "IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE ='U' AND NAME = 'TEMP" & systemId & "REORD')"
            StrSql += vbCrLf + "  DROP TABLE TEMP" & systemId & "REORD"
            StrSql += vbCrLf + "  Select * INTO TEMP" & systemId & "REORD from"
            StrSql += vbCrLf + "  ("
            StrSql += vbCrLf + "  select *"
            StrSql += vbCrLf + "  ,(StkPcs-piece)as DiffPcs"
            StrSql += vbCrLf + "  ,(StkWeight-weight)as DiffWeight"
            StrSql += vbCrLf + "  ,'1'Result"
            StrSql += vbCrLf + "  from"
            StrSql += vbCrLf + "  ("
            StrSql += vbCrLf + "  select "
            StrSql += vbCrLf + "  ((select itemName from " & cnAdminDb & "..itemMast where itemid = r.itemId)+'-'+"
            StrSql += vbCrLf + "  isnull((select subItemName from " & cnAdminDb & "..subItemMast where subItemId = r.subItemId),'')+'['+"
            StrSql += vbCrLf + "  convert(varchar(20),fromweight)+'.000 TO '+convert(varchar(20),toweight)+'.000]')as Particular,round((fromWeight+toWeight)/2,0)as Range,"
            If chkSize.Checked Then
                StrSql += vbCrLf + "  CONVERT(VARCHAR,(select sizename from " & cnAdminDb & "..itemsize where sizeid = r.sizeid)) as Size,"
            End If
            StrSql += vbCrLf + "  piece,weight,"
            StrSql += vbCrLf + "  sum(isnull(case when t.GrsWt between fromWeight and toWeight then Pcs end,0)) as StkPcs,"
            StrSql += vbCrLf + "  sum(isnull(case when t.GrsWt between fromWeight and toWeight then GrsWt end,0)) as StkWeight,"
            StrSql += vbCrLf + "  CONVERT(VARCHAR(1),' ')COLHEAD "
            StrSql += vbCrLf + "  from " & cnAdminDb & "..stkReorder as r"
            StrSql += vbCrLf + "  	left outer join "
            StrSql += vbCrLf + "  	" & cnAdminDb & "..ITEMTAG as t on t.itemid = r.itemId and t.subitemid = r.subitemid "
            If Not cnCentStock Then StrSql += " AND T.COMPANYID = '" & GetStockCompId() & "'"
            StrSql += vbCrLf + "      and t.RecDate <= '" & dtpAsOnDate.Value.Date.ToString("yyyy-MM-dd") & "' and (t.issdate is null or t.issdate >= '" & dtpAsOnDate.Value.Date.ToString("yyyy-MM-dd") & "')"
            StrSql += ftrSql
            StrSql += vbCrLf + "  group by r.itemId,r.subItemId,fromweight,toweight,piece,weight"
            If chkSize.Checked Then
                StrSql += vbCrLf + "  ,R.SIZEID"
            End If
            StrSql += vbCrLf + "  )as Xy"
            If rbtShort.Checked = True Then
                StrSql += vbCrLf + "  where (StkPcs-piece) < 0"
            ElseIf rbtExcess.Checked = True Then
                StrSql += vbCrLf + "  where (StkPcs-piece) > 0"
            End If
            StrSql += vbCrLf + "  UNION ALL"
            StrSql += vbCrLf + "  select "
            StrSql += vbCrLf + "  'TOTAL'PARTICULAR,"
            StrSql += vbCrLf + "  null Range,"
            If chkSize.Checked Then
                StrSql += " NULL SIZE,"
            End If
            StrSql += vbCrLf + "  sum(Piece)Piece,"
            StrSql += vbCrLf + "  sum(Weight)Weight,"
            StrSql += vbCrLf + "  sum(StkPcs)StkPcs,"
            StrSql += vbCrLf + "  sum(StkWeight)StkWeight,"
            StrSql += vbCrLf + "  'G' COLHEAD,"
            StrSql += vbCrLf + "  sum((StkPcs-piece))as DiffPcs,"
            StrSql += vbCrLf + "  sum((StkWeight-weight))as DiffWeight,"
            StrSql += vbCrLf + "  '2' Result"
            StrSql += vbCrLf + "  from"
            StrSql += vbCrLf + "  ("
            StrSql += vbCrLf + "  select "
            StrSql += vbCrLf + "  ((select itemName from " & cnAdminDb & "..itemMast where itemid = r.itemId)+'-'+"
            StrSql += vbCrLf + "  (select subItemName from " & cnAdminDb & "..subItemMast where subItemId = r.subItemId)+'['+"
            StrSql += vbCrLf + "  convert(varchar(20),fromweight)+'.000 TO '+convert(varchar(20),toweight)+'.000]')as Particular,round((fromWeight+toWeight)/2,0)as Range,piece,weight,"
            StrSql += vbCrLf + "  sum(isnull(case when t.GrsWt between fromWeight and toWeight then Pcs end,0)) as StkPcs,"
            StrSql += vbCrLf + "  sum(isnull(case when t.GrsWt between fromWeight and toWeight then GrsWt end,0)) as StkWeight"
            StrSql += vbCrLf + "  from " & cnAdminDb & "..stkReorder as r"
            StrSql += vbCrLf + "  	left outer join "
            StrSql += vbCrLf + "  	" & cnAdminDb & "..ITEMTAG as t on t.itemid = r.itemId and t.subitemid = r.subitemid "
            If Not cnCentStock Then StrSql += " AND T.COMPANYID = '" & GetStockCompId() & "'"
            StrSql += vbCrLf + "      and t.RecDate <= '" & dtpAsOnDate.Value.Date.ToString("yyyy-MM-dd") & "' and t.issdate is null or (t.issdate >= '" & dtpAsOnDate.Value.Date.ToString("yyyy-MM-dd") & "')"
            StrSql += ftrSql
            StrSql += vbCrLf + "  group by r.itemId,r.subItemId,fromweight,toweight,piece,weight"
            StrSql += vbCrLf + "  )as Total"
            If rbtShort.Checked = True Then
                StrSql += vbCrLf + "  having sum(StkPcs-piece) < 0 "
            ElseIf rbtExcess.Checked = True Then
                StrSql += vbCrLf + "  having sum(StkPcs-piece) > 0 "
            End If
            StrSql += vbCrLf + "  )as X "
            StrSql += vbCrLf + "  order by Result,Particular"
            cmd = New OleDbCommand(StrSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            StrSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "REORD)>0 "
            StrSql += vbCrLf + "  BEGIN "
            StrSql += vbCrLf + "  INSERT INTO TEMP" & systemId & "REORDERREP(PARTICULAR"
            If chkSize.Checked Then
                StrSql += " ,SIZE"
            End If
            StrSql += " , RANGE, PIECE, WEIGHT, "
            StrSql += vbCrLf + "  STKPCS, STKWEIGHT, DIFFPCS, DIFFWEIGHT, COLHEAD)"
            StrSql += vbCrLf + "  SELECT PARTICULAR, "
            If chkSize.Checked Then
                StrSql += " SIZE, "
            End If
            StrSql += " RANGE, PIECE, WEIGHT, "
            StrSql += vbCrLf + "  STKPCS, STKWEIGHT, DIFFPCS, DIFFWEIGHT, COLHEAD "
            StrSql += vbCrLf + "  FROM TEMP" & systemId & "REORD ORDER BY RESULT,PARTICULAR"
            StrSql += vbCrLf + "  END "
            cmd = New OleDbCommand(StrSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            StrSql = " SELECT * FROM TEMP" & systemId & "REORDERREP ORDER BY SNO "

            Dim dt As New DataTable
            da = New OleDbDataAdapter(StrSql, cn)
            da.Fill(dt)
            If Not dt.Rows.Count > 0 Then
                MsgBox("Record Not Found", MsgBoxStyle.Exclamation)
                Exit Sub
            End If
            Prop_Sets()
            gridView.DataSource = dt
            tabView.Show()
            GridViewFormat()
            With gridView
                .Columns("SNO").Visible = False
                .Columns("RESULT").Visible = False
                .Columns("COLHEAD").Visible = False
                If chkSize.Checked Then
                    .Columns("SIZE").Width = 80
                    .Columns("SIZE").SortMode = DataGridViewColumnSortMode.NotSortable
                End If
                With .Columns("Range")
                    .HeaderText = "RANGE"
                    .Width = 60
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Resizable = DataGridViewTriState.False
                End With
                With .Columns("Particular")
                    .HeaderText = "PARTICULR"
                    .Width = 290
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
                With .Columns("Piece")
                    .HeaderText = "REORD PCS"
                    .Width = 70
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Resizable = DataGridViewTriState.False
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
                With .Columns("Weight")
                    .HeaderText = "REORD WEIGHT"
                    .Width = 90
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Resizable = DataGridViewTriState.False
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
                With .Columns("StkPcs")
                    .HeaderText = "STK PCS"
                    .Width = 70
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Resizable = DataGridViewTriState.False
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
                With .Columns("StkWeight")
                    .HeaderText = "STK WEIGHT"
                    .Width = 90
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Resizable = DataGridViewTriState.False
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
                With .Columns("DiffPcs")
                    .HeaderText = "DIFF PCS"
                    .Width = 70
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Resizable = DataGridViewTriState.False
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
                With .Columns("DiffWeight")
                    .HeaderText = "DIFF WIGHT"
                    .Width = 90
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Resizable = DataGridViewTriState.False
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
            End With
            lblTitle.Text = "REORDER STATUS REPORT AS ON " & dtpAsOnDate.Value.ToString("dd-MM-yyyy")
            lblTitle.Refresh()
            pnlView.Visible = True
            pnlView.BringToFront()
            grpControls.Visible = False
            tabMain.SelectedTab = tabView
            gridView.Focus()
        Catch ex As Exception
            MsgBox(Err.Description, MsgBoxStyle.Information)
            btnNew_Click(Me, New EventArgs)
        Finally
            Me.Cursor = Cursors.Arrow
        End Try
        Prop_Sets()
    End Sub

    Private Sub frmSizeWiseReorderStock_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        ElseIf e.KeyChar = Chr(Keys.Escape) And tabMain.SelectedTab.Name = tabView.Name Then
            btnBack_Click(Me, New EventArgs)
        End If
    End Sub

   
    Function GridViewFormat() As Integer
        For Each dgvRow As DataGridViewRow In gridView.Rows
            With dgvRow
                Select Case .Cells("COLHEAD").Value.ToString
                    Case "G"
                        .DefaultCellStyle.BackColor = reportTotalStyle.BackColor
                        .DefaultCellStyle.Font = reportTotalStyle.Font
                End Select
            End With
        Next
    End Function
    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = Chr(Keys.Escape) Then
            pnlView.Visible = False
            grpControls.Visible = True
            grpControls.BringToFront()
            chkCmbItem.Focus()
        End If
        If e.KeyChar = "X" Or e.KeyChar = "x" Then
            Me.btnExcel_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = "P" Then
            btnPrint_Click(Me, New EventArgs)
        End If
        
    End Sub

    Private Sub cmbItemName_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCmbItem.SelectedIndexChanged

    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        'Me.Cursor = Cursors.WaitCursor
        If gridView.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export)
        End If
        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        If (cmbCostCentre.Enabled = True) Then cmbCostCentre.Text = IIf(cnDefaultCostId, "ALL", cnCostName)
        ' cmbSize.Enabled = False
        ' cmbItemName.Text = "ALL"
        ' cmbSubItemName.Text = "ALL"
        ' cmbSize.Text = "ALL"
        dtpAsOnDate.Value = GetServerDate()
        'rbtStock.Checked = True
        ' rbtBoth.Checked = True
        Prop_Gets()
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGen
        dtpAsOnDate.Focus()
    End Sub

    Private Sub chkSize_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSize.CheckedChanged
        If chkSize.Checked = True Then
            cmbSize.Text = "ALL"
            cmbSize.Enabled = True
        Else
            cmbSize.Text = "ALL"
            cmbSize.Enabled = False
        End If
    End Sub


    Private Sub Prop_Sets()
        Dim obj As New frmSizeWiseReorderStock_Properties
        obj.p_cmbMetal = cmbMetal.Text
        obj.p_cmbItemName = chkCmbItem.Text
        obj.p_cmbSubItemName = cmbSubItemName.Text
        'obj.p_cmbCostCentre = cmbCostCentre.Text
        obj.p_chkSize = chkSize.Checked
        obj.p_cmbSize = cmbSize.Text
        obj.p_rbtStock = rbtStock.Checked
        obj.p_rbtSales = rbtSales.Checked
        obj.p_rbtBoth = rbtBoth.Checked
        obj.p_rbtShort = rbtShort.Checked
        obj.p_rbtExcess = rbtExcess.Checked
        obj.p_chkDetailedOthers = chkDetailedOthers.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmSizeWiseReorderStock_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmSizeWiseReorderStock_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmSizeWiseReorderStock_Properties))
        cmbCounter.Text = obj.p_cmbcounter
        cmbMetal.Text = obj.p_cmbMetal
        chkCmbItem.Text = obj.p_cmbItemName
        cmbSubItemName.Text = obj.p_cmbSubItemName
        'cmbCostCentre.Text = obj.p_cmbCostCentre
        chkSize.Checked = obj.p_chkSize
        cmbSize.Text = obj.p_cmbSize
        rbtStock.Checked = obj.p_rbtStock
        rbtSales.Checked = obj.p_rbtSales
        rbtBoth.Checked = obj.p_rbtBoth
        rbtShort.Checked = obj.p_rbtShort
        rbtExcess.Checked = obj.p_rbtExcess
        chkDetailedOthers.Checked = obj.p_chkDetailedOthers
    End Sub
    Function funcLoadItemName() As Integer
        StrSql = " SELECT 'ALL' ITEMNAME,'ALL' ITEMID,1 RESULT"
        StrSql += " UNION ALL"
        StrSql += " SELECT ITEMNAME,CONVERT(VARCHAR,ITEMID),2 RESULT FROM " & cnAdminDb & "..ITEMMAST"
        StrSql += " WHERE ISNULL(ACTIVE,'') <> 'N' AND ISNULL(STOCKREPORT,'')<>'N'"
        If cmbMetal.Text <> "ALL" And cmbMetal.Text <> "" Then
            StrSql += " AND METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & GetQryString(cmbMetal.Text) & "))"
        End If
        StrSql += " ORDER BY RESULT,ITEMNAME"
        dtItem = New DataTable
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(dtItem)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbItem, dtItem, "ITEMNAME", , "ALL")
    End Function

    Private Sub chkCmbItem_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCmbItem.TextChanged
        cmbSubItemName.Items.Clear()
        StrSql = vbCrLf + " SELECT 'ALL' SUBITEMNAME,'ALL'SUBITEMID,1 RESULT,0 DISPLAYORDER"
        StrSql += vbCrLf + " UNION ALL"
        StrSql += vbCrLf + " SELECT SUBITEMNAME,CONVERT(VARCHAR,SUBITEMID),2 RESULT,DISPLAYORDER FROM " & cnAdminDb & "..SUBITEMMAST"
        StrSql += vbCrLf + " WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & chkCmbItem.Text & "')"
        If _SubItemOrderByName Then
            StrSql += vbCrLf + " ORDER BY RESULT,SUBITEMNAME"
        Else
            StrSql += vbCrLf + " ORDER BY RESULT,DISPLAYORDER,SUBITEMNAME"
        End If

        da = New OleDbDataAdapter(StrSql, cn)
        dtSubItem = New DataTable
        da.Fill(dtSubItem)
        For Each ro As DataRow In dtSubItem.Rows
            cmbSubItemName.Items.Add(ro.Item("SUBITEMNAME").ToString)
        Next

        If cmbMetal.Text = "ALL" And chkCmbItem.Text = "ALL" Then
            rdbRate.Enabled = True
            rdbWeight.Enabled = True
            rdbWeight.Checked = True
        End If

        Dim Sql As String = "Select RangeMode from " & cnAdminDb & "..StkReorder "
        Sql += " Where ItemId = (select ItemId from " & cnAdminDb & "..ItemMast where ItemName = '" & chkCmbItem.Text & "')"
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(Sql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            For i As Integer = 0 To dt.Rows.Count - 1
                If dt.Rows(i).Item("RangeMode").ToString = "R" Then
                    rdbRate.Enabled = True
                    rdbRate.Checked = True
                    rdbWeight.Enabled = False
                ElseIf dt.Rows(i).Item("RangeMode").ToString = "W" Then
                    rdbWeight.Enabled = True
                    rdbWeight.Checked = True
                    rdbRate.Enabled = False
                End If
            Next
        End If
    End Sub

    Private Sub cmbMetal_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbMetal.TextChanged
        funcLoadItemName()
    End Sub
End Class

Public Class frmSizeWiseReorderStock_Properties
    Private cmbMetal As String = "ALL"
    Public Property p_cmbMetal() As String
        Get
            Return cmbMetal
        End Get
        Set(ByVal value As String)
            cmbMetal = value
        End Set
    End Property
    Private cmbcounter As String = "ALL"
    Public Property p_cmbcounter() As String
        Get
            Return cmbcounter
        End Get
        Set(ByVal value As String)
            cmbcounter = value
        End Set
    End Property
    Private cmbItemName As String = "ALL"
    Public Property p_cmbItemName() As String
        Get
            Return cmbItemName
        End Get
        Set(ByVal value As String)
            cmbItemName = value
        End Set
    End Property
    Private cmbSubItemName As String = "ALL"
    Public Property p_cmbSubItemName() As String
        Get
            Return cmbSubItemName
        End Get
        Set(ByVal value As String)
            cmbSubItemName = value
        End Set
    End Property
    Private cmbCostCentre As String = "ALL"
    Public Property p_cmbCostCentre() As String
        Get
            Return cmbCostCentre
        End Get
        Set(ByVal value As String)
            cmbCostCentre = value
        End Set
    End Property
    Private chkSize As Boolean = False
    Public Property p_chkSize() As Boolean
        Get
            Return chkSize
        End Get
        Set(ByVal value As Boolean)
            chkSize = value
        End Set
    End Property
    Private cmbSize As String = "ALL"
    Public Property p_cmbSize() As String
        Get
            Return cmbSize
        End Get
        Set(ByVal value As String)
            cmbSize = value
        End Set
    End Property
    Private rbtStock As Boolean = True
    Public Property p_rbtStock() As Boolean
        Get
            Return rbtStock
        End Get
        Set(ByVal value As Boolean)
            rbtStock = value
        End Set
    End Property
    Private rbtSales As Boolean = False
    Public Property p_rbtSales() As Boolean
        Get
            Return rbtSales
        End Get
        Set(ByVal value As Boolean)
            rbtSales = value
        End Set
    End Property
    Private rbtBoth As Boolean = True
    Public Property p_rbtBoth() As Boolean
        Get
            Return rbtBoth
        End Get
        Set(ByVal value As Boolean)
            rbtBoth = value
        End Set
    End Property
    Private rbtShort As Boolean = False
    Public Property p_rbtShort() As Boolean
        Get
            Return rbtShort
        End Get
        Set(ByVal value As Boolean)
            rbtShort = value
        End Set
    End Property
    Private rbtExcess As Boolean = False
    Public Property p_rbtExcess() As Boolean
        Get
            Return rbtExcess
        End Get
        Set(ByVal value As Boolean)
            rbtExcess = value
        End Set
    End Property
    Private chkDetailedOthers As Boolean = False
    Public Property p_chkDetailedOthers() As Boolean
        Get
            Return chkDetailedOthers
        End Get
        Set(ByVal value As Boolean)
            chkDetailedOthers = value
        End Set
    End Property
End Class