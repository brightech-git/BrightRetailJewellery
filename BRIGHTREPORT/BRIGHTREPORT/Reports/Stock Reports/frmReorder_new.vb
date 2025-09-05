Imports System.Data.OleDb
Imports System.Threading
Imports System.IO
Public Class frmReorder_new
    Dim strSql As String
    Dim dsGridView As New DataSet
    Dim dtcompany, dtCostCentre, dtmetal, dtitem, dtdesign, dtsubitem, dtrange As New DataTable
    Dim OLtran As OleDbTransaction
    Dim companyid As String = "ALL"
    Dim costids As String = "ALL"
    Dim metalids As String = "ALL"
    Dim itemid As String = "ALL"
    Dim subitemid As String = "ALL"
    Dim designerid As String = "ALL"
    Dim stktype As String = "T"
    Dim cmd As OleDbCommand
    Private Sub Generateselectedragetable()
        strSql = " IF OBJECT_ID('TEMPTABLEDB..TEMPSELECTRANGE') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPSELECTRANGE"
        strSql += " SELECT FROMWEIGHT,TOWEIGHT INTO TEMPTABLEDB..TEMPSELECTRANGE FROM " & cnAdminDb & "..RANGEMAST WHERE 1<>1 "
        cmd = New OleDbCommand(strSql, cn) : cmd.ExecuteNonQuery()
        If Trim(chkcmbrange.Text.ToString) = "ALL" Or Trim(chkcmbrange.Text.ToString) = "" Then
            For i As Integer = 1 To dtrange.Rows.Count - 1
                InsertRange(dtrange.Rows(i).Item("FROMWEIGHT").ToString, dtrange.Rows(i).Item("TOWEIGHT").ToString)
            Next
        Else
            For i As Integer = 0 To chkcmbrange.CheckedItems.Count - 1
                Dim range() As String
                range = chkcmbrange.CheckedItems.Item(i).ToString.Split("-")
                InsertRange(range(0), range(1))
            Next
        End If
    End Sub
    Private Sub InsertRange(ByVal frwt As String, ByVal towt As String) ', ByVal itemid As String, ByVal subid As String, ByVal cost As String
        strSql = "INSERT INTO TEMPTABLEDB..TEMPSELECTRANGE"
        strSql += " SELECT '" & Val(frwt) & "','" & Val(towt) & "'" ','" & itemid & "','" & subid & "','" & cost & "'"
        cmd = New OleDbCommand(strSql, cn) : cmd.ExecuteNonQuery()
    End Sub
    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        If Trim(chkcmbcompany.Text.ToString) = "ALL" Or Trim(chkcmbcompany.Text.ToString) = "" Then companyid = "ALL" Else companyid = GetSelectedCompanyId(chkcmbcompany, True)
        If Trim(chkcmbmetal.Text.ToString) = "ALL" Or Trim(chkcmbmetal.Text.ToString) = "" Then metalids = "ALL" Else metalids = GetSelectedMetalid(chkcmbmetal, True)
        If rbtFormat1.Checked = True Then
            If Trim(chkcmbitem.Text.ToString) = "ALL" Or Trim(chkcmbitem.Text.ToString) = "" Then itemid = "ALL" Else itemid = GetSelecteditemid(chkcmbitem, True)
            If Trim(chkCmbCostCentre.Text.ToString) = "ALL" Or Trim(chkCmbCostCentre.Text.ToString) = "" Then costids = "ALL" Else costids = GetSelectedCostId(chkCmbCostCentre, True)
        Else
            If cmbItemName_OWN.SelectedValue Is Nothing Then
                MsgBox("select itemname", MsgBoxStyle.Information)
                cmbItemName_OWN.SelectAll()
                cmbItemName_OWN.Focus()
                Exit Sub
            End If
            If Trim(cmbItemName_OWN.Text.ToString) = "ALL" Or Trim(cmbItemName_OWN.Text.ToString) = "" Then itemid = "ALL" Else itemid = "'" & cmbItemName_OWN.SelectedValue.ToString & "'"
            If itemid = "ALL" Then
                MsgBox("select any one itemid", MsgBoxStyle.Information)
                cmbItemName_OWN.SelectAll()
                Exit Sub
            End If
            If Trim(cmbCostCentre_OWN.Text.ToString) = "ALL" Or Trim(cmbCostCentre_OWN.Text.ToString) = "" Then costids = "ALL" Else costids = "'" & cmbCostCentre_OWN.SelectedValue.ToString & "'"
            If costids = "ALL" Then
                If cnCostId <> "" Then
                    MsgBox("select any one costid", MsgBoxStyle.Information)
                    cmbCostCentre_OWN.SelectAll()
                    Exit Sub
                End If
            End If
        End If
        If Trim(chkcmbsutitem.Text.ToString) = "ALL" Or Trim(chkcmbsutitem.Text.ToString) = "" Then subitemid = "ALL" Else subitemid = GetSelectedSubitemid(chkcmbsutitem, True)
        If Trim(chkcmbdesigner.Text.ToString) = "ALL" Or Trim(chkcmbdesigner.Text.ToString) = "" Then designerid = "ALL" Else designerid = GetSelectedDesignerid(chkcmbdesigner, True)
        Generateselectedragetable()
        If rbtboth.Checked Then : stktype = "B" : ElseIf rbttag.Checked Then : stktype = "T" : Else : stktype = "N" : End If
        HorizontalView()
    End Sub

    Private Sub HorizontalView()
        gridview.DataSource = Nothing
        gridheader1.DataSource = Nothing
        strSql = "  IF OBJECT_ID('TEMPTABLEDB..TEMPRANGEWISE','U')IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPRANGEWISE "
        strSql += vbCrLf + "  IF OBJECT_ID('TEMPTABLEDB..TEMPRANGEWISE_RES','U')IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPRANGEWISE_RES"
        cmd = New OleDbCommand(strSql, cn) : cmd.ExecuteNonQuery()

        If rbtFormat1.Checked = True Then
            strSql = " EXECUTE " & cnAdminDb & "..SP_RPT_REORDERSTOCK_NEW"
            strSql += vbCrLf + "@TEMPTABLE='TEMPTABLEDB..TEMPRANGEWISE'"
            strSql += vbCrLf + ",@ASONDATE ='" & dtpFrom.Value.ToString("yyyy/MM/dd") & "'"
            strSql += vbCrLf + ",@COMPANYID=""" & companyid.ToString & """"
            strSql += vbCrLf + ",@COSTID=""" & costids.ToString & """"
            'strSql += vbCrLf + ",@METALID=""" & metalids.ToString & """"
            strSql += vbCrLf + ",@ITEMID=""" & itemid.ToString & """"
            strSql += vbCrLf + ",@SUBITEMID=""" & subitemid.ToString & """"
            strSql += vbCrLf + ",@DESIGNERID=""" & designerid.ToString & """"
            strSql += vbCrLf + ",@WITHCUM='" & IIf(chkwithcum.Checked, "Y", "N") & "'"
            strSql += vbCrLf + ",@STKTYPE='" & stktype & "'"
            strSql += vbCrLf + ",@BASEDON='" & IIf(rbtitem.Checked, "I", "D") & "'"
            strSql += vbCrLf + ",@DISPTYPE='" & IIf(rbthorizontal.Checked, "H", "V") & "'"
        End If
        If rbtFormat2.Checked = True Then
            If chkWithSizeName.Checked Then
                strSql = " EXECUTE " & cnAdminDb & "..SP_RPT_REORDERSTOCK_NEW_FORMAT2_SIZEWISE"
            Else
                strSql = " EXECUTE " & cnAdminDb & "..SP_RPT_REORDERSTOCK_NEW_FORMAT2"
            End If
            strSql += vbCrLf + "@TEMPTABLE='TEMPTABLEDB..TEMPRANGEWISE'"
            strSql += vbCrLf + ",@ASONDATE ='" & dtpFrom.Value.ToString("yyyy/MM/dd") & "'"
            strSql += vbCrLf + ",@COMPANYID=""" & companyid.ToString & """"
            strSql += vbCrLf + ",@COSTID=""" & costids.ToString & """"
            strSql += vbCrLf + ",@ITEMID=""" & itemid.ToString & """"
            strSql += vbCrLf + ",@SUBITEMID=""" & subitemid.ToString & """"
            strSql += vbCrLf + ",@DESIGNERID=""" & designerid.ToString & """"
            strSql += vbCrLf + ",@WITHCUM='" & IIf(chkwithcum.Checked, "Y", "N") & "'"
            strSql += vbCrLf + ",@STKTYPE='" & stktype & "'"
            strSql += vbCrLf + ",@BASEDON='" & IIf(rbtitem.Checked, "I", "D") & "'"
            strSql += vbCrLf + ",@DISPTYPE='" & IIf(rbthorizontal.Checked, "H", "V") & "'"
            strSql += vbCrLf + ",@WITHOUTSTOCK='" & IIf(chkwithoutStockOnly.Checked, "Y", "N") & "'"
            strSql += vbCrLf + ",@EMPTYSTOCK='" & IIf(chkEmptyStock.Checked, "Y", "N") & "'"
            strSql += vbCrLf + ",@WITHCAPTION='" & IIf(chkWithCaption.Checked, "Y", "N") & "'"
            strSql += vbCrLf + ",@RANGETYPE='" & IIf(rbtRangeIn.Checked, "I", IIf(rbtRangeOut.Checked, "O", "A")) & "'"
            If chkWithSizeName.Checked Then
                Dim sizeid As Integer = 0
                If cmbSizename_OWN.Text <> "" Then
                    sizeid = Val(objGPack.GetSqlValue("SELECT SIZEID FROM " & cnAdminDb & "..ITEMSIZE WHERE SIZENAME = '" & cmbSizename_OWN.Text & "' AND ITEMID = " & itemid, , 0, tran))
                End If
                strSql += vbCrLf + ",@SIZENAME='" & Val(sizeid.ToString) & "'"
            End If
        End If
        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 1000
        Dim da As New OleDbDataAdapter(cmd)
        Dim dss As New DataSet
        da.Fill(dss)
        Dim dt As New DataTable
        Dim tempdt As New DataTable
        dt = dss.Tables(0)
        If dt.Rows.Count > 0 Then
            gridview.DataSource = Nothing
            gridview.DataSource = dt
        Else
            gridview.DataSource = Nothing
            gridheader1.DataSource = Nothing
            MsgBox("No Records found..")
            Exit Sub
        End If
        TabControl1.SelectedTab = TabPage2
        With gridview
            For i As Integer = 0 To gridview.Columns.Count - 1
                If .Columns(i).HeaderText.Contains("PCS") Then .Columns(i).Width = 90
                If .Columns(i).HeaderText.Contains("GRSWT") Then .Columns(i).Width = 120
                If .Columns(i).HeaderText.Contains("PARTICULAR") Then .Columns(i).Width = 250
                If Not chkwithsalvalue.Checked Then If .Columns(i).HeaderText.Contains("SALVALUE") Then .Columns(i).Visible = False
            Next
            .Columns("COLHEAD").Visible = False
            .Columns("RESULT").Visible = False
            .Columns("METALNAME").Visible = False
            .Columns("SUBITEMNAME").Visible = False
            .Columns("SUBITEMID").Visible = False
            .Columns("METALID").Visible = False
            If rbtFormat2.Checked = True And rbthorizontal.Checked = True Then
                .Columns("COSTID").Visible = False
            End If
            If rbtitem.Checked Then .Columns("ITEMID").Visible = False : .Columns("ITEMNAME").Visible = False
            If rbtdesigner.Checked Then .Columns("DESIGNERID").Visible = False : .Columns("DESIGNERNAME").Visible = False
            If rbtFormat2.Checked = True And chkWithSizeName.Checked Then
                .Columns("SIZEID").Visible = False
            End If
            If Not rbthorizontal.Checked Then
                .Columns("FROMWEIGHT").Visible = False
                .Columns("TOWEIGHT").Visible = False
                .Columns("DISPORDER").Visible = False
            End If
        End With
        GridViewHeaderStyle()
    End Sub

    Private Sub GridViewHeaderStyle()
        Dim dtrange As New DataTable("COST")
        If rbthorizontal.Checked Then
            Dim hro() As String = Nothing
            Dim range As String = "ALL"
            With dtrange
                .Columns.Add("PARTICULAR", GetType(String))
                Dim cdt As New DataTable
                strSql = " SELECT 'TOTAL'RANGE,0 TOWEIGHT,0 RESULT  UNION ALL "
                strSql += " SELECT 'OTHERS'RANGE,0 TOWEIGHT,2 RESULT  UNION ALL "
                strSql += "SELECT  CONVERT(VARCHAR,FROMWEIGHT) +'-'+ CONVERT(VARCHAR,TOWEIGHT)RANGE ,TOWEIGHT,1 RESULT FROM TEMPTABLEDB..TEMPRANGEWISE WHERE ISNULL(FROMWEIGHT,0)<>0 AND ISNULL(TOWEIGHT,0)<>0   GROUP BY TOWEIGHT,FROMWEIGHT ORDER BY RESULT,TOWEIGHT  "
                cdt = GetSqlTable(strSql, cn)
                If cdt.Rows.Count > 0 Then
                    range = ""
                    For i As Integer = 0 To cdt.Rows.Count - 1
                        range += cdt.Rows(i).Item("RANGE").ToString
                        If i < cdt.Rows.Count - 1 Then range += ","
                    Next
                End If
                If range <> "ALL" Then
                    range = range.Replace("'", "")
                    hro = range.Split(",")
                End If
                For i As Integer = 0 To hro.Length - 1
                    Dim txt As String = ""
                    Dim type As String = ""
                    For j As Integer = 2 To gridview.Columns.Count - 1
                        If gridview.Columns(j).HeaderText.Contains(hro(i).ToString) Then
                            txt += gridview.Columns(j).HeaderText
                            If j <= gridview.Columns.Count Then txt += "~"
                        End If
                    Next
                    If txt <> "" Then .Columns.Add(txt, GetType(String)) : .Columns(txt).Caption = hro(i).ToString
                Next
                dtrange.Columns.Add("SCROLL", GetType(String))
                dtrange.Columns("PARTICULAR").Caption = ""
                dtrange.Columns("SCROLL").Caption = ""
            End With
            With gridheader1
                .DataSource = dtrange
                For i As Integer = 0 To hro.Length - 1
                    Dim txt As String = ""
                    For j As Integer = 2 To gridview.Columns.Count - 1
                        If gridview.Columns(j).HeaderText.Contains(hro(i).ToString) Then
                            txt += gridview.Columns(j).HeaderText
                            If j <= gridview.Columns.Count Then txt += "~"
                        End If
                    Next
                    If txt <> "" Then .Columns(txt).HeaderText = hro(i).ToString
                Next
                .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                .Columns("PARTICULAR").Width = gridview.Columns("PARTICULAR").Width
                For i As Integer = 0 To hro.Length - 1
                    Dim txt As String = ""
                    Dim width As Integer = 0
                    Dim type As String = ""
                    Dim typwidth As Integer = 0
                    Dim metalwidth As Integer = 0
                    For j As Integer = 2 To gridview.Columns.Count - 1
                        If gridview.Columns(j).HeaderText.Contains(hro(i).ToString) Then
                            txt += gridview.Columns(j).HeaderText
                            If gridview.Columns(j).Visible Then width += gridview.Columns(j).Width
                            If j <= gridview.Columns.Count Then txt += "~"
                        End If
                    Next
                    If txt <> "" Then .Columns(txt).Width = width
                Next
                gridview.Focus()
                Dim colWid As Integer = 0
                For cnt As Integer = 0 To gridview.ColumnCount - 1
                    If gridview.Columns(cnt).Visible Then colWid += gridview.Columns(cnt).Width
                Next
                gridheader1.Columns("PARTICULAR").Frozen = True
                gridview.Columns("PARTICULAR").Frozen = True

                If colWid >= gridview.Width Then
                    gridheader1.Columns("SCROLL").Visible = CType(gridview.Controls(0), HScrollBar).Visible
                    gridheader1.Columns("SCROLL").Width = CType(gridview.Controls(1), VScrollBar).Width
                Else
                    gridheader1.Columns("SCROLL").Visible = False
                End If
            End With
            For i As Integer = 0 To gridview.Columns.Count - 1
                If gridview.Columns(i).HeaderText.Contains("PCS") Then gridview.Columns(i).HeaderText = "STOCK PCS"
                If gridview.Columns(i).HeaderText.Contains("REORDER") Then gridview.Columns(i).HeaderText = "REORDER PCS"
                If gridview.Columns(i).HeaderText.Contains("DIFF") Then gridview.Columns(i).HeaderText = "DIFF PCS"
                If gridview.Columns(i).HeaderText.Contains("SALVALUE") Then gridview.Columns(i).HeaderText = "SALVALUE"
                If gridview.Columns(i).HeaderText.Contains("AMOUNT") Then gridview.Columns(i).HeaderText = "AMOUNT"
                If gridview.Columns(i).HeaderText.Contains("SALVALUE") Or gridview.Columns(i).HeaderText.Contains("PCS") _
            Or gridview.Columns(i).HeaderText.Contains("REORDER") Or gridview.Columns(i).HeaderText.Contains("DIFF") Then gridview.Columns(i).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            Next

            gridview.Columns("PARTICULAR").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            gridview.Columns("PARTICULAR").HeaderText = ""
            Dim dt As DataTable = CType(gridview.DataSource, DataTable)
            Dim dt1 As DataTable = CType(gridheader1.DataSource, DataTable)
            gridheader1.Columns("SCROLL").Visible = False
            gridheader1.Columns("SCROLL").HeaderText = ""
            gridview.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            gridview.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        Else
            dtrange.Columns.Add("PARTICULAR", GetType(String))
            Dim headertxt1 As String = ""
            If chkwithsalvalue.Checked Then
                headertxt1 = "REORDER~DIFF~PCS~RANGE~SALVALUE"
            Else
                headertxt1 = "REORDER~DIFF~PCS~RANGE"
            End If
            dtrange.Columns.Add(headertxt1, GetType(String))
            Dim Width As Double = 0
            gridheader1.DataSource = dtrange
            gridheader1.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            gridheader1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            gridheader1.Columns("PARTICULAR").Width = gridview.Columns("PARTICULAR").Width
            Width = gridview.Columns("REORDER").Width + gridview.Columns("DIFF").Width + gridview.Columns("PCS").Width
            If chkwithsalvalue.Checked Then Width += gridview.Columns("SALVALUE").Width
            Width += gridview.Columns("RANGE").Width
            gridheader1.Columns(headertxt1).Width = Width
            gridheader1.Columns(headertxt1).HeaderText = "WEIGHT"
        End If
        For i As Integer = 0 To gridview.Rows.Count - 1

            If gridview.Rows(i).Cells("COLHEAD").Value = "G" Then
                gridview.Rows(i).DefaultCellStyle.BackColor = Color.LightYellow
                gridview.Rows(i).DefaultCellStyle.ForeColor = Color.Black
                gridview.Rows(i).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            ElseIf gridview.Rows(i).Cells("COLHEAD").Value = "T" Then
                gridview.Rows(i).DefaultCellStyle.BackColor = Color.Green
                gridview.Rows(i).DefaultCellStyle.ForeColor = Color.White
                gridview.Rows(i).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            ElseIf gridview.Rows(i).Cells("COLHEAD").Value = "T1" Then
                gridview.Rows(i).DefaultCellStyle.BackColor = Color.LightGreen
                gridview.Rows(i).DefaultCellStyle.ForeColor = Color.Red
                gridview.Rows(i).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            ElseIf gridview.Rows(i).Cells("COLHEAD").Value = "T2" Then
                gridview.Rows(i).DefaultCellStyle.BackColor = Color.LightBlue
                gridview.Rows(i).DefaultCellStyle.ForeColor = Color.Red
                gridview.Rows(i).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            ElseIf gridview.Rows(i).Cells("COLHEAD").Value = "S" Or gridview.Rows(i).Cells("COLHEAD").Value = "S1" Or gridview.Rows(i).Cells("COLHEAD").Value = "S2" Then
                gridview.Rows(i).DefaultCellStyle.BackColor = Color.White
                gridview.Rows(i).DefaultCellStyle.ForeColor = Color.Blue
                gridview.Rows(i).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            End If
        Next
        gridview.Columns("PARTICULAR").HeaderText = ""
    End Sub

    Private Sub frmSalesAbstract_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        ElseIf e.KeyChar = Chr(Keys.Escape) Then
            If TabControl1.SelectedTab Is TabPage1 = False Then
                TabControl1.SelectedTab = TabPage1
            End If
        End If
    End Sub

    Private Sub frmSalesAbstract_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        TabControl1.ItemSize = New Size(0, 1)
        TabControl1.SizeMode = TabSizeMode.Fixed
        strSql = " SELECT 'ALL'COSTNAME, 'ALL' COSTID,1 RESULT UNION ALL SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))

        strSql = " SELECT 'ALL'COMPANYNAME, 'ALL' COMPANYID,1 RESULT UNION ALL SELECT COMPANYNAME,CONVERT(VARCHAR,COMPANYID),2 RESULT FROM " & cnAdminDb & "..COMPANY"
        strSql += " ORDER BY RESULT,COMPANYNAME"
        dtcompany = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtcompany)
        BrighttechPack.GlobalMethods.FillCombo(chkcmbcompany, dtcompany, "COMPANYNAME", , "ALL")

        strSql = " SELECT 'ALL'METALNAME, 'ALL' METALID,1 RESULT UNION ALL SELECT METALNAME,CONVERT(VARCHAR,METALID),2 RESULT FROM " & cnAdminDb & "..METALMAST"
        'strSql += " WHERE METALID IN(SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE CALTYPE IN('R','B','F'))"
        strSql += " ORDER BY RESULT,METALNAME"
        dtmetal = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtmetal)
        BrighttechPack.GlobalMethods.FillCombo(chkcmbmetal, dtmetal, "METALNAME", , "ALL")
        strSql = " SELECT 'ALL'DESIGNERNAME, 'ALL' DESIGNERID,1 RESULT UNION ALL SELECT DESIGNERNAME,CONVERT(VARCHAR,DESIGNERID),2 RESULT FROM " & cnAdminDb & "..DESIGNER"
        strSql += " ORDER BY RESULT,DESIGNERNAME"
        dtdesign = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtdesign)
        BrighttechPack.GlobalMethods.FillCombo(chkcmbdesigner, dtdesign, "DESIGNERNAME", , "ALL")
        TabControl1.SelectedTab = TabPage1
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        Me.btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ExtToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExtToolStripMenuItem.Click
        Me.btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        dtpFrom.Value = System.DateTime.Now
        Panel3.Visible = False

        strSql = " SELECT 'ALL'ITEMNAME, 'ALL' ITEMID,'' METALID,1 RESULT "
        strSql += " UNION ALL SELECT ITEMNAME,CONVERT(VARCHAR,ITEMID),METALID,2 RESULT FROM " & cnAdminDb & "..ITEMMAST"
        strSql += " WHERE ISNULL(ACTIVE,'')<>'N' "
        strSql += " ORDER BY RESULT,ITEMNAME"
        dtitem = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtitem)
        BrighttechPack.GlobalMethods.FillCombo(chkcmbitem, dtitem, "ITEMNAME", , "ALL")

        strSql = " SELECT ITEMNAME,CONVERT(VARCHAR,ITEMID)ITEMID,METALID FROM " & cnAdminDb & "..ITEMMAST"
        strSql += " WHERE ISNULL(ACTIVE,'')<>'N' "
        strSql += " ORDER BY ITEMNAME "
        dtitem = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtitem)
        If dtitem.Rows.Count > 0 Then
            cmbItemName_OWN.DataSource = Nothing
            cmbItemName_OWN.DataSource = dtitem
            cmbItemName_OWN.DisplayMember = "ITEMNAME"
            cmbItemName_OWN.ValueMember = "ITEMID"
        End If


        strSql = "SELECT COSTID,COSTNAME FROM " & cnAdminDb & "..COSTCENTRE"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        If dtCostCentre.Rows.Count > 0 Then
            cmbCostCentre_OWN.DataSource = Nothing
            cmbCostCentre_OWN.DataSource = dtCostCentre
            cmbCostCentre_OWN.DisplayMember = "COSTNAME"
            cmbCostCentre_OWN.ValueMember = "COSTID"
        End If
        strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COSTCENTRE'"
        If UCase(objGPack.GetSqlValue(strSql, "CTLTEXT", "Y", tran)) = "N" Then
            'cmbCostCentre_OWN.Items.Clear()
            strSql = "SELECT '' COSTID,'ALL' COSTNAME"
            dtCostCentre = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtCostCentre)
            If dtCostCentre.Rows.Count > 0 Then
                cmbCostCentre_OWN.DataSource = Nothing
                cmbCostCentre_OWN.DataSource = dtCostCentre
                cmbCostCentre_OWN.DisplayMember = "COSTNAME"
                cmbCostCentre_OWN.ValueMember = "COSTID"
            End If
            cmbCostCentre_OWN.Text = ""
        End If

        strSql = " SELECT 'ALL'SUBITEMNAME, 'ALL' SUBITEMID,'' ITEMID,1 RESULT UNION ALL SELECT SUBITEMNAME,CONVERT(vARCHAR,SUBITEMID),ITEMID,2 RESULT FROM " & cnAdminDb & "..SUBITEMMAST"
        strSql += " WHERE ISNULL(ACTIVE,'')<>'N'"
        strSql += " ORDER BY RESULT,SUBITEMNAME"
        dtsubitem = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtsubitem)
        BrighttechPack.GlobalMethods.FillCombo(chkcmbsutitem, dtsubitem, "SUBITEMNAME", , "ALL")

        chkWithSizeName.Visible = rbtFormat2.Checked
        gridview.DataSource = Nothing
        btnView_Search.Enabled = True
        chkCmbCostCentre.Text = "ALL"
        chkcmbmetal.Text = "ALL"
        chkcmbrange.Text = "ALL"
        chkcmbdesigner.Text = "ALL"
        rbtFormat1.Focus()
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If gridview.Rows.Count > 0 Then
            Dim rpttitle As String
            rpttitle = "RANGEWISE STOCK"
            rpttitle += " ASON [" & dtpFrom.Value.ToString("dd-MM-yyyy") & "] "
            'BrightPosting.GExport.Post(Me.Name, rpttitle, lblTitle.Text, gridview, BrightPosting.GExport.GExportType.Export, gridheader1, Nothing, Nothing)
            BrightPosting.GExport.Post(Me.Name, rpttitle, lblTitle.Text, gridview, BrightPosting.GExport.GExportType.Export, gridheader1, Nothing, Nothing)
        End If

    End Sub

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridview.KeyPress
        If e.KeyChar = "X" Or e.KeyChar = "x" Then
            Me.btnExcel_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = "P" Then

        End If
    End Sub

    Private Sub ResizeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResizeToolStripMenuItem.Click
        If gridview.RowCount > 0 Then
            If ResizeToolStripMenuItem.Checked Then
                gridview.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                gridview.Invalidate()
                For Each dgvCol As DataGridViewColumn In gridview.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridview.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            Else
                For Each dgvCol As DataGridViewColumn In gridview.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridview.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End If
            'gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        End If
    End Sub

    Private Sub gridView_Scroll(ByVal sender As Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles gridview.Scroll
        If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
            gridheader1.HorizontalScrollingOffset = e.NewValue
        End If
        Try
            If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then

                gridheader1.HorizontalScrollingOffset = e.NewValue
                gridheader1.Columns("SCROLL").Visible = CType(gridview.Controls(0), HScrollBar).Visible
                gridheader1.Columns("SCROLL").Width = CType(gridview.Controls(1), VScrollBar).Width
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try
    End Sub

    Private Sub rbtFormat1_CheckedChanged(sender As Object, e As EventArgs) Handles rbtFormat1.CheckedChanged
        If rbtFormat1.Checked = True Then
            rbtdesigner.Visible = True
            cmbItemName_OWN.Visible = False
            cmbCostCentre_OWN.Visible = False
            chkcmbitem.Visible = True
            chkCmbCostCentre.Visible = True
            chkWithCaption.Visible = False
            chkWithSizeName.Visible = False
            chkWithSizeName.Checked = False
            cmbSizename_OWN.Visible = False
            cmbSizename_OWN.Text = ""
            cmbSizename_OWN.Enabled = False
            PanelRange.Visible = False
            rbtRangeAll.Checked = True
            lblRangeType.Visible = False
        End If
    End Sub

    Private Sub rbtFormat2_CheckedChanged(sender As Object, e As EventArgs) Handles rbtFormat2.CheckedChanged
        If rbtFormat2.Checked = True Then
            rbtdesigner.Visible = False
            chkWithSizeName.Visible = True
            chkWithSizeName.Checked = False
            cmbSizename_OWN.Visible = True
            chkWithCaption.Visible = True
            cmbSizename_OWN.Text = ""
            cmbItemName_OWN.Visible = True
            cmbCostCentre_OWN.Visible = True
            chkcmbitem.Visible = False
            chkCmbCostCentre.Visible = False
            cmbItemName_OWN.Location = chkcmbitem.Location
            cmbCostCentre_OWN.Location = chkCmbCostCentre.Location
            If cnCostId = "" Then
                cmbCostCentre_OWN.Text = ""
            End If
            If rbtvertical.Checked Then
                PanelRange.Visible = True
                rbtRangeAll.Checked = True
                lblRangeType.Visible = True
            End If
        End If
    End Sub

    Private Sub chkcmbmetal_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkcmbmetal.Leave
        'If chkcmbmetal.Text.Contains("ALL") Or chkcmbmetal.Text = "" Then chkcmbmetal.Text = "ALL"
        If chkcmbmetal.CheckedItems.Contains("ALL") Or chkcmbmetal.Text = "" Then
            chkcmbmetal.Text = "ALL"
            chkcmbmetal.SetItemCheckState(0, CheckState.Checked)
            For i As Integer = 1 To chkcmbmetal.Items.Count - 1
                If chkcmbmetal.GetItemCheckState(i) = CheckState.Checked Then
                    chkcmbmetal.SetItemCheckState(i, CheckState.Unchecked)
                End If
            Next
        End If

    End Sub

    Private Sub TabPage1_Click(sender As Object, e As EventArgs) Handles TabPage1.Click

    End Sub

    'Private Sub rbthorizontal_CheckedChanged(sender As Object, e As EventArgs) Handles rbthorizontal.CheckedChanged
    '    chkWithSizeName.Visible = False
    '    chkWithSizeName.Checked = False
    'End Sub

    'Private Sub rbtvertical_CheckedChanged(sender As Object, e As EventArgs) Handles rbtvertical.CheckedChanged
    '    chkWithSizeName.Visible = False
    '    chkWithSizeName.Checked = False
    'End Sub
    Private Sub chkCmbCostCentre_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCmbCostCentre.Leave
        If chkCmbCostCentre.CheckedItems.Contains("ALL") Or chkCmbCostCentre.Text = "" Then
            chkCmbCostCentre.Text = "ALL"
            chkCmbCostCentre.SetItemCheckState(0, CheckState.Checked)
            For i As Integer = 1 To chkCmbCostCentre.Items.Count - 1
                If chkCmbCostCentre.GetItemCheckState(i) = CheckState.Checked Then
                    chkCmbCostCentre.SetItemCheckState(i, CheckState.Unchecked)
                End If
            Next
        End If
    End Sub

    Private Sub chkWithSizeName_CheckedChanged(sender As Object, e As EventArgs) Handles chkWithSizeName.CheckedChanged
        If chkWithSizeName.Visible = True Then
            cmbSizename_OWN.Enabled = True
            cmbSizename_OWN.Text = ""
            If chkWithSizeName.Checked Then
                Dim dtsizename As DataTable
                strSql = "SELECT SIZENAME,CONVERT(VARCHAR,SIZEID)SIZEID FROM " & cnAdminDb & "..ITEMSIZE"
                strSql += " WHERE "
                strSql += " ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbItemName_OWN.Text & "' AND ISNULL(ACTIVE,'')<>'N')"
                strSql += " ORDER BY SIZENAME"
                dtsizename = New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtsizename)
                If dtsizename.Rows.Count > 0 Then
                    cmbSizename_OWN.DataSource = Nothing
                    cmbSizename_OWN.DataSource = dtsizename
                    cmbSizename_OWN.DisplayMember = "SIZENAME"
                    cmbSizename_OWN.ValueMember = "SIZEID"
                End If
            Else
                cmbSizename_OWN.Enabled = False
                cmbSizename_OWN.Text = ""
            End If
        Else
            cmbSizename_OWN.Text = ""
        End If
    End Sub

    Private Sub chkcmbrange_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkcmbrange.Enter
        If Trim(chkCmbCostCentre.Text.ToString) = "ALL" Or Trim(chkCmbCostCentre.Text.ToString) = "" Then costids = "ALL" Else costids = GetSelectedCostId(chkCmbCostCentre, True)
        If Trim(chkcmbitem.Text.ToString) = "ALL" Or Trim(chkcmbitem.Text.ToString) = "" Then itemid = "ALL" Else itemid = GetSelecteditemid(chkcmbitem, True)
        If Trim(chkcmbsutitem.Text.ToString) = "ALL" Or Trim(chkcmbsutitem.Text.ToString) = "" Then subitemid = "ALL" Else subitemid = GetSelectedSubitemid(chkcmbsutitem, True)
        strSql = " SELECT 'ALL' RANGE,0 FROMWEIGHT,0 TOWEIGHT,0 RESULT UNION ALL " ',0 ITEMID,0 SUBITEMID,'' COSTID
        strSql += "SELECT DISTINCT  CONVERT(VARCHAR,FROMWEIGHT)+'-'+CONVERT(VARCHAR,TOWEIGHT)RANGE,FROMWEIGHT,TOWEIGHT,1 RESULT FROM " & cnAdminDb & "..STKREORDER WHERE 1=1 " ',ITEMID,SUBITEMID,COSTID 
        If costids <> "ALL" Then strSql += vbCrLf + " AND COSTID IN(" & costids & ")"
        If itemid <> "ALL" Then strSql += vbCrLf + " AND ITEMID IN(" & itemid & ")"
        If subitemid <> "ALL" Then strSql += vbCrLf + " AND SUBITEMID IN(" & subitemid & ")"
        'strSql += vbCrLf + " AND ISNULL(RANGEMODE,'') IN('R')"
        strSql += vbCrLf + " ORDER BY RESULT,FROMWEIGHT"
        dtrange = New DataTable
        dtrange = GetSqlTable(strSql, cn)
        If dtrange.Rows.Count = 1 Then : MsgBox("No Ranges available.") : Exit Sub : End If
        BrighttechPack.GlobalMethods.FillCombo(chkcmbrange, dtrange, "RANGE", , "ALL")
    End Sub

    Private Sub rbtvertical_CheckedChanged(sender As Object, e As EventArgs) Handles rbtvertical.CheckedChanged
        If rbtvertical.Checked And rbtFormat2.Checked Then
            PanelRange.Visible = True
            rbtRangeAll.Checked = True
            lblRangeType.Visible = True
        Else
            PanelRange.Visible = False
            rbtRangeAll.Checked = True
            lblRangeType.Visible = False
        End If
    End Sub
    Private Sub chkcmbitem_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkcmbitem.Enter

        If Trim(chkcmbmetal.Text.ToString) <> "ALL" And Trim(chkcmbmetal.Text.ToString) <> "" Then
            If Trim(chkcmbmetal.Text.ToString) = "ALL" Or Trim(chkcmbmetal.Text.ToString) = "" Then metalids = "ALL" Else metalids = GetSelectedMetalid(chkcmbmetal, True)
            Dim dttemp As New DataTable
            Dim dv As New DataView
            dttemp = dtitem.Copy()
            dv = dttemp.DefaultView
            dv.RowFilter = "(METALID IN('P'))"
            dttemp = dv.ToTable()
            dtitem.Rows.Clear()
            Dim ro As DataRow
            ro = dtitem.NewRow()
            ro("ITEMNAME") = "ALL"
            ro("ITEMID") = "0"
            ro("METALID") = "ALL"
            ro("RESULT") = "0"
            dtitem.Rows.Add(ro)
            dtitem.Merge(dttemp)
            BrighttechPack.GlobalMethods.FillCombo(chkcmbitem, dtitem, "ITEMNAME", , "ALL")
        End If
    End Sub

    Private Sub chkcmbsutitem_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkcmbsutitem.Enter
        If Trim(chkcmbitem.Text.ToString) <> "ALL" And Trim(chkcmbitem.Text.ToString) <> "" Then
            If Trim(chkcmbitem.Text.ToString) = "ALL" Or Trim(chkcmbitem.Text.ToString) = "" Then itemid = "ALL" Else itemid = GetSelecteditemid(chkcmbitem, True)
            Dim dttemp As New DataTable
            Dim dv As New DataView
            dttemp = dtsubitem.Copy()
            dv = dttemp.DefaultView
            dv.RowFilter = "(ITEMID IN(" & itemid & "))"
            dttemp = dv.ToTable()
            dtsubitem.Rows.Clear()
            Dim ro As DataRow
            ro = dtsubitem.NewRow()
            ro("SUBITEMNAME") = "ALL"
            ro("SUBITEMID") = "0"
            ro("ITEMID") = "0"
            ro("RESULT") = "0"
            dtsubitem.Rows.Add(ro)
            dtsubitem.Merge(dttemp)
            BrighttechPack.GlobalMethods.FillCombo(chkcmbsutitem, dtsubitem, "SUBITEMNAME", , "ALL")
        End If
    End Sub

    Private Sub btnback_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnback.Click
        If Not TabControl1.SelectedTab Is TabPage1 Then TabControl1.SelectedTab = TabPage1
    End Sub

    Private Sub gridview_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles gridview.CellContentClick
        If rbtFormat2.Checked = True Then
            If gridview.Rows.Count > 0 Then

            End If
        End If
    End Sub
    Private Sub cmbItemName_OWN_LostFocus(sender As Object, e As EventArgs) Handles cmbItemName_OWN.LostFocus
        If cmbItemName_OWN.Text <> "" Then
            strSql = " SELECT 'ALL'SUBITEMNAME, 'ALL' SUBITEMID,'' ITEMID,1 RESULT "
            strSql += " UNION ALL SELECT SUBITEMNAME,CONVERT(vARCHAR,SUBITEMID),ITEMID,2 RESULT FROM " & cnAdminDb & "..SUBITEMMAST"
            strSql += " WHERE ISNULL(ACTIVE,'')<>'N'"
            strSql += " AND ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbItemName_OWN.Text & "' AND ISNULL(ACTIVE,'')<>'N')"
            strSql += " ORDER BY RESULT,SUBITEMNAME"
            dtsubitem = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtsubitem)
            BrighttechPack.GlobalMethods.FillCombo(chkcmbsutitem, dtsubitem, "SUBITEMNAME", , "ALL")

            cmbSizename_OWN.DataSource = Nothing
            Dim dtsizename As DataTable
            strSql = "SELECT SIZENAME,CONVERT(VARCHAR,SIZEID)SIZEID FROM " & cnAdminDb & "..ITEMSIZE"
            strSql += " WHERE "
            strSql += " ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbItemName_OWN.Text & "' AND ISNULL(ACTIVE,'')<>'N')"
            strSql += " ORDER BY SIZENAME"
            dtsizename = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtsizename)
            If dtsizename.Rows.Count > 0 Then
                cmbSizename_OWN.DataSource = Nothing
                cmbSizename_OWN.DataSource = dtsizename
                cmbSizename_OWN.DisplayMember = "SIZENAME"
                cmbSizename_OWN.ValueMember = "SIZEID"
            End If
        End If
    End Sub

    Private Sub cmbItemName_OWN_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbItemName_OWN.SelectedIndexChanged
        If cmbItemName_OWN.Text <> "" Then
            strSql = " SELECT 'ALL'SUBITEMNAME, 'ALL' SUBITEMID,'' ITEMID,1 RESULT "
            strSql += " UNION ALL SELECT SUBITEMNAME,CONVERT(vARCHAR,SUBITEMID),ITEMID,2 RESULT FROM " & cnAdminDb & "..SUBITEMMAST"
            strSql += " WHERE ISNULL(ACTIVE,'')<>'N'"
            strSql += " AND ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbItemName_OWN.Text & "' AND ISNULL(ACTIVE,'')<>'N')"
            strSql += " ORDER BY RESULT,SUBITEMNAME"
            dtsubitem = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtsubitem)
            BrighttechPack.GlobalMethods.FillCombo(chkcmbsutitem, dtsubitem, "SUBITEMNAME", , "ALL")

            cmbSizename_OWN.DataSource = Nothing
            Dim dtsizename As DataTable
            strSql = "SELECT SIZENAME,CONVERT(VARCHAR,SIZEID)SIZEID FROM " & cnAdminDb & "..ITEMSIZE"
            strSql += " WHERE "
            strSql += " ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbItemName_OWN.Text & "' AND ISNULL(ACTIVE,'')<>'N')"
            strSql += " ORDER BY SIZENAME"
            dtsizename = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtsizename)
            If dtsizename.Rows.Count > 0 Then
                cmbSizename_OWN.DataSource = Nothing
                cmbSizename_OWN.DataSource = dtsizename
                cmbSizename_OWN.DisplayMember = "SIZENAME"
                cmbSizename_OWN.ValueMember = "SIZEID"
            End If
        End If
    End Sub
End Class