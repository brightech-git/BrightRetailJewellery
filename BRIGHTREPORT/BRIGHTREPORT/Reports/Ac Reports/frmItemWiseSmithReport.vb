Imports System.Data.OleDb
Public Class frmItemWiseSmithReport
    Dim objGridShower As frmGridDispDia
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim Obj_MeFilterValues As frmItemWiseSmithReport_Properties
    Dim DtTrantype As DataTable
    Dim chkCategory As String
    Dim chkCostName As String
    Dim chkMetalName As String
    Dim Selectedcatid As String
    Dim Selectedmetalid As String
    Dim SelectedCostId As String
    Dim ActIVE As String = ""
    Dim trantype As String
    Dim chkwt As String = ""
    Dim selTran As String = ""
    Dim AcTypeFilteration As String = ""
    Dim dtCostCentre As New DataTable
    Dim dispflag As Boolean = True
    Dim PurewtPer As Decimal = GetAdmindbSoftValue("RPT_PUREWTPER", "0")
    Dim objGridDetailShower As frmSmithBalSummery_F1
    Dim RPT_SMITHABS_DAYS As Boolean = IIf(GetAdmindbSoftValue("RPT_SMITHABS_DAYS", "N") = "Y", True, False)

    Private Function GetTranType(ByVal selTran As String)
        Dim trantype As String = ""
        If selTran <> "ALL" And selTran <> "" Then
            selTran = "," & selTran
            If selTran.Contains(",ISSUE") Then
                trantype += "'IIS',"
            End If
            If selTran.Contains(",APPROVAL ISSUE") Then
                trantype += "'IAP',"
            End If
            If selTran.Contains(",OTHER ISSUE") Then
                trantype += "'IOT',"
            End If
            If selTran.Contains(",PURCHASE RETURN") Then
                trantype += "'IPU',"
            End If
            If selTran.Contains(",INTERNAL TRANSFER") Then
                trantype += "'IIN',"
                trantype += "'RIN',"
            End If
            If selTran.Contains(",RECEIPT") Then
                trantype += "'RRE',"
            End If
            If selTran.Contains(",APPROVAL RECEIPT") Then
                trantype += "'RAP',"
            End If
            If selTran.Contains(",OTHER RECEIPT") Then
                trantype += "'ROT',"
            End If
            If selTran.Contains(",MISC ISSUE") Then
                trantype += "'MI',"
            End If
            If selTran.Contains(",PURCHASE") Then
                trantype += "'RPU',"
            End If
        End If
        If trantype <> "" Then
            trantype = Mid(trantype, 1, trantype.Length - 1)
        End If
        Return trantype
    End Function

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim SYSID As String = Trim(LSet(Mid(Guid.NewGuid.ToString, 1, InStr(Guid.NewGuid.ToString, "-") - 1), 10))
        If ChkGrossWt.Checked = False And ChkNetWt.Checked = False And ChkPureWt.Checked = False Then MsgBox("Select GrsWt/NetWt/PureWt", MsgBoxStyle.Information) : ChkGrossWt.Focus() : Exit Sub
        If Not chkLstMetal.CheckedItems.Count > 0 Then
            chkMetalSelectAll.Checked = True
        End If
        Dim dtGrid As New DataTable("SUMMARY")
        AcTypeFilteration = ""
        If chkDealer.Checked Then
            AcTypeFilteration += "'D',"
        End If
        If chkSmith.Checked Then
            AcTypeFilteration += "'G',"
        End If
        If chkInternal.Checked Then
            AcTypeFilteration += "'I',"
        End If
        If chkOthers.Checked Then
            AcTypeFilteration += "'O',"
        End If
        If chkCustomer.Checked Then
            AcTypeFilteration += "'C',"
        End If
        If AcTypeFilteration <> "" Then
            AcTypeFilteration = Mid(AcTypeFilteration, 1, AcTypeFilteration.Length - 1)
        End If
        selTran = ""

        Dim Apptrantype As String = ""
        chkCategory = GetChecked_CheckedList(chkLstCategory)
        chkCostName = GetChecked_CheckedList(chkLstCostCentre)
        chkMetalName = GetChecked_CheckedList(chkLstMetal)
        dispflag = True
        Dim Accode As String
        If CmbAcname.Text.Trim <> "" Then
            Accode = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME='" & CmbAcname.Text.Trim & "'", , "", )
        Else
            Accode = ""
        End If

        If Apptrantype = "" Then Apptrantype = "''"
        Selectedcatid = GetSelectedcatId(chkLstCategory, False)
        Selectedmetalid = GetSelectedMETALId(chkLstMetal, False)
        SelectedCostId = GetSelectedcostId(chkLstCostCentre, False)
        If ChkGrossWt.Checked Then chkwt = "G"
        If ChkNetWt.Checked Then chkwt = "N"
        If ChkPureWt.Checked Then chkwt = "P"
        If ChkPureWt.Checked And ChkNetWt.Checked And ChkGrossWt.Checked Then chkwt = "A"

        strSql = vbCrLf + " EXEC " & cnAdminDb & "..SP_RPT_SMITHABSTRACT_DETAIL_2"
        strSql += vbCrLf + " @DBNAME='" & cnStockDb & "'"
        strSql += vbCrLf + ",@TEMPTAB= 'TEMP" & SYSID & "ABSTRACT'"
        strSql += vbCrLf + ",@FROMDATE='" & dtpFromDate.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + ",@TODATE='" & dtpTodate.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + ",@METALID='" & Selectedmetalid & "'"
        strSql += vbCrLf + ",@COMPANYID='" & strCompanyId & "'"
        strSql += vbCrLf + ",@COSTID='" & SelectedCostId & "'"
        If AcTypeFilteration = "" Then
            strSql += vbCrLf + ",@ACFILTER=" & IIf(AcTypeFilteration = "", "''", AcTypeFilteration) & ""
        Else
            strSql += vbCrLf + ",@ACFILTER=""" & IIf(AcTypeFilteration = "", "''", AcTypeFilteration) & """"
        End If
        strSql += vbCrLf + ",@TRANFILTER=''"
        strSql += vbCrLf + ",@CATFILTER='" & Selectedcatid & "'"
        strSql += vbCrLf + " ,@ACCODE='" & Accode & "'"
        strSql += vbCrLf + ",@PUREWTPER=" & PurewtPer & ""
        strSql += vbCrLf + ",@WITHWAST='" & IIf(ChkwithWast.Checked, "Y", "N") & "'"
        strSql += vbCrLf + ",@RPTTYPE='" & IIf(rbtDetailed.Checked, "D", "S") & "'"
        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 1000
        Dim da As New OleDbDataAdapter(cmd)
        Dim dss As New DataSet
        da.Fill(dss)
        Dim dt As New DataTable
        Dim tempdt As New DataTable
        dt = dss.Tables(0)
        If dt.Rows.Count > 0 Then
            ReSizeToolStripMenuItem.Checked = False
            gridviewDet.DataSource = Nothing
            gridviewDet.DataSource = dt
            tabMain.SelectedTab = tabView
            gridStyle()
            GridViewHeaderCreator(gridviewHdr)
            Dim tit As String = "ITEM WISE SMITH REPORT FROM " & dtpFromDate.Text & " TO " & dtpTodate.Text
            tit += vbCrLf + "FOR " & CmbAcname.Text.Trim
            lblTitle.Text = tit
        Else
            gridviewDet.DataSource = Nothing
            gridviewHdr.DataSource = Nothing
            MsgBox("No Records found..")
            Exit Sub
        End If
    End Sub

    Private Function gridStyle()
        For k As Integer = 0 To gridviewDet.Columns.Count - 1
            If Not gridviewDet.Columns(k).Name = "PARTICULAR" Then
                gridviewDet.Columns(k).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End If
        Next
        With gridviewDet
            .Columns("ACCODE").Visible = False
            .Columns("ITEMNAME").Visible = False
            .Columns("SUBITEMNAME").Visible = False
            .Columns("RESULT").Visible = False
            If rbtSummary.Checked Then
                .Columns("TRANDATE").Visible = False
                .Columns("TRANNO").Visible = False
                .Columns("REFNO").Visible = False
            Else
                .Columns("TRANDATE").Visible = False
                '.Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
            End If
            .Columns("OPGRSWT").HeaderText = "GRSWT"
            .Columns("OPNETWT").HeaderText = "NETWT"
            .Columns("OPPUREWT").HeaderText = "PUREWT"
            .Columns("RGRSWT").HeaderText = "GRSWT"
            .Columns("RNETWT").HeaderText = "NETWT"
            .Columns("RPUREWT").HeaderText = "PUREWT"
            .Columns("IGRSWT").HeaderText = "GRSWT"
            .Columns("INETWT").HeaderText = "NETWT"
            .Columns("IPUREWT").HeaderText = "PUREWT"
            .Columns("CGRSWT").HeaderText = "GRSWT"
            .Columns("CNETWT").HeaderText = "NETWT"
            .Columns("CPUREWT").HeaderText = "PUREWT"
        End With
        For Each gv As DataGridViewRow In gridviewDet.Rows
            With gv
                Select Case .Cells("RESULT").Value.ToString
                    Case "0"
                        .Cells("PARTICULAR").Style.BackColor = Color.LightBlue
                        .DefaultCellStyle.ForeColor = Color.DarkBlue
                        .DefaultCellStyle.Font = reportHeadStyle1.Font
                    Case "1"
                        .Cells("PARTICULAR").Style.BackColor = Color.LightGoldenrodYellow
                        .DefaultCellStyle.ForeColor = Color.Red
                        .DefaultCellStyle.Font = reportHeadStyle1.Font
                    Case "3"
                        .DefaultCellStyle.BackColor = Color.LightGoldenrodYellow
                        .DefaultCellStyle.ForeColor = Color.Red
                        .DefaultCellStyle.Font = reportHeadStyle1.Font
                    Case "4"
                        .DefaultCellStyle.BackColor = Color.LightGreen
                        .DefaultCellStyle.ForeColor = Color.DarkGreen
                        .DefaultCellStyle.Font = reportHeadStyle1.Font
                    Case "5"
                        .DefaultCellStyle.BackColor = Color.LightBlue
                        .DefaultCellStyle.ForeColor = Color.DarkBlue
                        .DefaultCellStyle.Font = reportHeadStyle1.Font
                End Select
            End With
        Next
        gridviewDet.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
        gridviewDet.Invalidate()
        For Each dgvCol As DataGridViewColumn In gridviewDet.Columns
            dgvCol.Width = dgvCol.Width
        Next
        gridviewDet.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
    End Function

    Public Function GetSelectedMETALId(ByVal chkLst As CheckedListBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Items.Count > 0 Then
            For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
                If WithQuotes Then retStr += "'"
                retStr += objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & chkLst.CheckedItems.Item(cnt).ToString & "'")
                If WithQuotes Then retStr += "'"
                If cnt <> chkLst.CheckedItems.Count - 1 Then
                    retStr += ","
                End If
            Next
        End If
        Return retStr
    End Function

    Public Function GetSelectedcatId(ByVal chkLst As CheckedListBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Items.Count > 0 Then
            For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
                If WithQuotes Then retStr += "'"
                retStr += objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & chkLst.CheckedItems.Item(cnt).ToString & "'")
                If WithQuotes Then retStr += "'"
                If cnt <> chkLst.CheckedItems.Count - 1 Then
                    retStr += ","
                End If
            Next
        End If
        Return retStr
    End Function

    Public Function GetSelectedcostId(ByVal chkLst As CheckedListBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Items.Count > 0 Then
            For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
                If WithQuotes Then retStr += "'"
                retStr += objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & chkLst.CheckedItems.Item(cnt).ToString & "'")
                If WithQuotes Then retStr += "'"
                If cnt <> chkLst.CheckedItems.Count - 1 Then
                    retStr += ","
                End If
            Next
        End If
        Return retStr
    End Function

    Function funcColWidthDetail() As Integer
        With gridviewHdr
            If gridviewHdr.DataSource Is Nothing Then Exit Function
            .Columns("SCROLL").HeaderText = ""
            If rbtSummary.Checked Then
                .Columns("PARTICULAR").Width = gridviewDet.Columns("PARTICULAR").Width
            Else
                .Columns("PARTICULAR~TRANNO~REFNO").Width = gridviewDet.Columns("PARTICULAR").Width
                .Columns("PARTICULAR~TRANNO~REFNO").Width += gridviewDet.Columns("TRANNO").Width
                .Columns("PARTICULAR~TRANNO~REFNO").Width += gridviewDet.Columns("REFNO").Width
            End If
            Dim strSepApp As String = ""
            .Columns("OPGRSWT~OPNETWT~OPPUREWT" & strSepApp).Width = gridviewdet.Columns("OPGRSWT").Width
            .Columns("OPGRSWT~OPNETWT~OPPUREWT" & strSepApp).Width += gridviewdet.Columns("OPNETWT").Width
            .Columns("OPGRSWT~OPNETWT~OPPUREWT" & strSepApp).Width += gridviewdet.Columns("OPPUREWT").Width
            .Columns("OPGRSWT~OPNETWT~OPPUREWT" & strSepApp).HeaderText = "OPENING"

            .Columns("RGRSWT~RNETWT~RPUREWT" & strSepApp).Width = gridviewdet.Columns("RGRSWT").Width
            .Columns("RGRSWT~RNETWT~RPUREWT" & strSepApp).Width += gridviewdet.Columns("RNETWT").Width
            .Columns("RGRSWT~RNETWT~RPUREWT" & strSepApp).Width += gridviewdet.Columns("RPUREWT").Width
            .Columns("RGRSWT~RNETWT~RPUREWT" & strSepApp).HeaderText = "RECEIPT"

            .Columns("IGRSWT~INETWT~IPUREWT" & strSepApp).Width = gridviewdet.Columns("IGRSWT").Width
            .Columns("IGRSWT~INETWT~IPUREWT" & strSepApp).Width += gridviewdet.Columns("INETWT").Width
            .Columns("IGRSWT~INETWT~IPUREWT" & strSepApp).Width += gridviewdet.Columns("IPUREWT").Width
            .Columns("IGRSWT~INETWT~IPUREWT" & strSepApp).HeaderText = "ISSUE"


            .Columns("CGRSWT~CNETWT~CPUREWT" & strSepApp).Width = gridviewdet.Columns("CGRSWT").Width
            .Columns("CGRSWT~CNETWT~CPUREWT" & strSepApp).Width += gridviewdet.Columns("CNETWT").Width
            .Columns("CGRSWT~CNETWT~CPUREWT" & strSepApp).Width += gridviewdet.Columns("CPUREWT").Width
            .Columns("CGRSWT~CNETWT~CPUREWT" & strSepApp).HeaderText = "CLOSING"

            .Columns("SCROLL").Visible = CType(gridviewDet.Controls(0), HScrollBar).Visible
            .Columns("SCROLL").Width = CType(gridviewDet.Controls(1), VScrollBar).Width
        End With
    End Function  '01

    Private Sub GridViewHeaderCreator(ByVal gridviewHdr As DataGridView)
        Dim dtHead As New DataTable
        If rbtSummary.Checked Then
            strSql = "SELECT ''[PARTICULAR]"
        Else
            strSql = "SELECT ''[PARTICULAR~TRANNO~REFNO]"
        End If
        strSql += " ,''[OPGRSWT~OPNETWT~OPPUREWT]"
        strSql += " ,''[RGRSWT~RNETWT~RPUREWT]"
        strSql += " ,''[IGRSWT~INETWT~IPUREWT]"
        strSql += " ,''[CGRSWT~CNETWT~CPUREWT]"
        strSql += " ,''SCROLL"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtHead)
        gridviewHdr.DataSource = Nothing
        gridviewHdr.DataSource = dtHead
        If rbtSummary.Checked Then
            gridviewHdr.Columns("PARTICULAR").HeaderText = ""
        Else
            gridviewHdr.Columns("PARTICULAR~TRANNO~REFNO").HeaderText = ""
        End If
        gridviewHdr.Columns("OPGRSWT~OPNETWT~OPPUREWT").HeaderText = "OPENING"
        gridviewHdr.Columns("RGRSWT~RNETWT~RPUREWT").HeaderText = "RECEIPT"
        gridviewHdr.Columns("IGRSWT~INETWT~IPUREWT").HeaderText = "ISSUE"
        gridviewHdr.Columns("CGRSWT~CNETWT~CPUREWT").HeaderText = "CLOSING"
        gridviewHdr.Columns("SCROLL").HeaderText = ""
        gridviewHdr.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        funcColWidthDetail()
    End Sub

    Private Sub frmItemWiseSmithReport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            tabMain.SelectedTab = tabGen
        End If
    End Sub

    Private Sub frmSmithBalanceSummaryReport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub


    Private Function FillAcname()


        Dim Actype As String = ""
        If chkDealer.Checked = False And chkSmith.Checked = False And chkInternal.Checked = False And chkOthers.Checked = False And chkCustomer.Checked = False Then
            Actype = "'D','G','I','O','C'"
        Else
            If chkDealer.Checked Then
                Actype += "'D',"
            End If
            If chkSmith.Checked Then
                Actype += "'G',"
            End If
            If chkInternal.Checked Then
                Actype += "'I',"
            End If
            If chkOthers.Checked Then
                Actype += "'O',"
            End If
            If chkCustomer.Checked Then
                Actype += "'C',"
            End If
            If Actype <> "" Then
                Actype = Mid(Actype, 1, Actype.Length - 1)
            End If
        End If


        strSql = " SELECT ACNAME,2 RESULT FROM " & cnAdminDb & "..ACHEAD WHERE 1=1 "
        'strSql += "  ISNULL(ACTIVE,'Y') <> 'H' "
        'If ActIVE = "B" Then
        '    strSql += "  ISNULL(ACTIVE,'Y') <> 'H' "
        'ElseIf ActIVE = "Y" Then
        '    strSql += "  ISNULL(ACTIVE,'') = 'Y' "
        'Else
        '    strSql += "  ISNULL(ACTIVE,'') ='N' "
        'End If
        strSql += "  AND ACTYPE IN (" & Actype & ")"
        strSql += " ORDER BY RESULT,ACNAME"
        Dim DtAcname As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(DtAcname)
        CmbAcname.Items.Clear()
        BrighttechPack.FillCombo(CmbAcname, DtAcname, "ACNAME", True)
    End Function

    Private Sub chkMetalSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkMetalSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstMetal, chkMetalSelectAll.Checked)
    End Sub

    Private Sub chkCostCentreSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCostCentreSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCostCentre, chkCostCentreSelectAll.Checked)
    End Sub

    Private Sub chkCategorySelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCategorySelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCategory, chkCategorySelectAll.Checked)
    End Sub

    Private Sub chkLstMetal_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLstMetal.Leave
        If Not chkLstMetal.CheckedItems.Count > 0 Then
            chkMetalSelectAll.Checked = True
        End If
    End Sub

    Private Sub chkLstMetal_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLstMetal.LostFocus
        Dim chkMetalNames As String = GetChecked_CheckedList(chkLstMetal)
        chkLstCategory.Items.Clear()
        If chkMetalNames <> "" Then
            strSql = " SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY "
            strSql += " WHERE ISNULL(LEDGERPRINT,'') <> 'N' AND METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalNames & "))"
            strSql += " ORDER BY CATNAME"
            FillCheckedListBox(strSql, chkLstCategory)
        End If
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        If rbtSummary.Checked = False And rbtDetailed.Checked = False Then
            rbtSummary.Checked = True
        End If
        dtpFromDate.Value = GetServerDate()
        dtpTodate.Value = GetServerDate()
        dtpTodate.Visible = True
        FillAcname()
        Prop_Gets()
        dtpFromDate.Select()
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New frmItemWiseSmithReport_Properties
        obj.p_dtpAsOnDate = dtpFromDate.Value.Date
        obj.p_chkCostCentreSelectAll = chkCostCentreSelectAll.Checked
        'GetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre)
        obj.p_chkMetalSelectAll = chkMetalSelectAll.Checked
        GetChecked_CheckedList(chkLstMetal, obj.p_chkLstMetal)
        obj.p_chkCategorySelectAll = chkCategorySelectAll.Checked
        GetChecked_CheckedList(chkLstCategory, obj.p_chkLstCategory)
        obj.p_chkDealer = chkDealer.Checked
        obj.p_chkSmith = chkSmith.Checked
        obj.p_chkOthers = chkOthers.Checked
        obj.p_chkCustomer = chkCustomer.Checked
        obj.p_rbtGrossWeight = ChkGrossWt.Checked
        obj.p_rbtNetWeight = ChkNetWt.Checked
        obj.p_rbtPureWeight = ChkPureWt.Checked
        obj.p_chkWithWast = ChkwithWast.Checked
        'obj.p_rbtSummary = rbtSummary.Checked
        'obj.p_rbtDetailed = rbtDetailed.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmSmithBalanceSummaryReport_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmItemWiseSmithReport_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmSmithBalanceSummaryReport_Properties))
        chkCostCentreSelectAll.Checked = obj.p_chkCostCentreSelectAll
        'SetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre, cnCostName)
        chkMetalSelectAll.Checked = obj.p_chkMetalSelectAll
        SetChecked_CheckedList(chkLstMetal, obj.p_chkLstMetal, Nothing)
        chkCategorySelectAll.Checked = obj.p_chkCategorySelectAll
        SetChecked_CheckedList(chkLstCategory, obj.p_chkLstCategory, Nothing)
        chkDealer.Checked = obj.p_chkDealer
        chkSmith.Checked = obj.p_chkSmith
        chkOthers.Checked = obj.p_chkOthers
        chkCustomer.Checked = obj.p_chkCustomer
        ChkGrossWt.Checked = obj.p_rbtGrossWeight
        ChkNetWt.Checked = obj.p_rbtNetWeight
        ChkPureWt.Checked = obj.p_rbtPureWeight
        ChkwithWast.Checked = obj.p_chkWithWast
        'rbtDetailed.Checked = obj.p_rbtDetailed
        'rbtSummary.Checked = obj.p_rbtSummary
    End Sub

    Private Sub chkDealer_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkDealer.CheckedChanged
        FillAcname()
    End Sub
    Private Sub chkInternal_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkInternal.CheckedChanged
        FillAcname()
    End Sub

    Private Sub chkOthers_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkOthers.CheckedChanged
        FillAcname()
    End Sub

    Private Sub chkSmith_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSmith.CheckedChanged
        FillAcname()
    End Sub

    Private Sub chkCustomer_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCustomer.CheckedChanged
        FillAcname()
    End Sub

    Private Sub frmItemWiseSmithReport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        tabMain.ItemSize = New Size(0, 1)
        tabMain.SizeMode = TabSizeMode.Fixed
        pnlContainer.Location = New Point((ScreenWid - pnlContainer.Width) / 2, ((ScreenHit - 128) - pnlContainer.Height) / 2)
        strSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE ACTIVE = 'Y' ORDER BY DISPLAYORDER"
        FillCheckedListBox(strSql, chkLstMetal)
        If GetAdmindbSoftValue("COSTCENTRE") = "Y" Then
            chkLstCostCentre.Enabled = True
            chkLstCostCentre.Items.Clear()
            strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            'FillCheckedListBox(strSql, chkLstCostCentre)
            Dim dt As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            For i As Integer = 0 To dt.Rows.Count - 1
                If cnCostName = dt.Rows(i).Item("COSTNAME").ToString And cnDefaultCostId = False Then
                    chkLstCostCentre.Items.Add(dt.Rows(i).Item("COSTNAME").ToString, True)
                Else
                    chkLstCostCentre.Items.Add(dt.Rows(i).Item("COSTNAME").ToString, False)
                End If
            Next
            If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkLstCostCentre.Enabled = False
        Else
            chkLstCostCentre.Enabled = False
            chkLstCostCentre.Items.Clear()
        End If
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ReSizeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReSizeToolStripMenuItem.Click
        If gridviewDet.RowCount > 0 Then
            If ReSizeToolStripMenuItem.Checked Then
                gridviewdet.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                gridviewdet.Invalidate()
                For Each dgvCol As DataGridViewColumn In gridviewdet.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridviewdet.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            Else
                For Each dgvCol As DataGridViewColumn In gridviewdet.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridviewDet.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End If
            GridViewHeaderCreator(gridviewHdr)
            'gridviewdet.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        End If
    End Sub

    Private Sub btnback_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnback.Click
        tabMain.SelectedTab = tabGen
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridviewDet.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridviewDet, BrightPosting.GExport.GExportType.Print, gridviewHdr)
        End If
    End Sub

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridviewDet.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridviewDet, BrightPosting.GExport.GExportType.Export, gridviewHdr)
        End If
    End Sub
End Class




Public Class frmItemWiseSmithReport_Properties
    Private dtpAsOnDate As Date = GetServerDate()
    Public Property p_dtpAsOnDate() As Date
        Get
            Return dtpAsOnDate
        End Get
        Set(ByVal value As Date)
            dtpAsOnDate = value
        End Set
    End Property

    Private chkCmbTrantype As New List(Of String)
    Public Property p_chkCmbTrantype() As List(Of String)
        Get
            Return chkCmbTrantype
        End Get
        Set(ByVal value As List(Of String))
            chkCmbTrantype = value
        End Set
    End Property
    Private chkCostCentreSelectAll As Boolean = False
    Public Property p_chkCostCentreSelectAll() As Boolean
        Get
            Return chkCostCentreSelectAll
        End Get
        Set(ByVal value As Boolean)
            chkCostCentreSelectAll = value
        End Set
    End Property

    Private chkLstCostCentre As New List(Of String)
    Public Property p_chkLstCostCentre() As List(Of String)
        Get
            Return chkLstCostCentre
        End Get
        Set(ByVal value As List(Of String))
            chkLstCostCentre = value
        End Set
    End Property
    Private chkMetalSelectAll As Boolean = False
    Public Property p_chkMetalSelectAll() As Boolean
        Get
            Return chkMetalSelectAll
        End Get
        Set(ByVal value As Boolean)
            chkMetalSelectAll = value
        End Set
    End Property
    Private chkLstMetal As New List(Of String)
    Public Property p_chkLstMetal() As List(Of String)
        Get
            Return chkLstMetal
        End Get
        Set(ByVal value As List(Of String))
            chkLstMetal = value
        End Set
    End Property
    Private chkCategorySelectAll As Boolean = False
    Public Property p_chkCategorySelectAll() As Boolean
        Get
            Return chkCategorySelectAll
        End Get
        Set(ByVal value As Boolean)
            chkCategorySelectAll = value
        End Set
    End Property
    Private chkLstCategory As New List(Of String)
    Public Property p_chkLstCategory() As List(Of String)
        Get
            Return chkLstCategory
        End Get
        Set(ByVal value As List(Of String))
            chkLstCategory = value
        End Set
    End Property
    Private chkDealer As Boolean = True
    Public Property p_chkDealer() As Boolean
        Get
            Return chkDealer
        End Get
        Set(ByVal value As Boolean)
            chkDealer = value
        End Set
    End Property
    Private chkSmith As Boolean = True
    Public Property p_chkSmith() As Boolean
        Get
            Return chkSmith
        End Get
        Set(ByVal value As Boolean)
            chkSmith = value
        End Set
    End Property
    Private chkOthers As Boolean = False
    Public Property p_chkOthers() As Boolean
        Get
            Return chkOthers
        End Get
        Set(ByVal value As Boolean)
            chkOthers = value
        End Set
    End Property
    Private chkCustomer As Boolean = False
    Public Property p_chkCustomer() As Boolean
        Get
            Return chkCustomer
        End Get
        Set(ByVal value As Boolean)
            chkCustomer = value
        End Set
    End Property
    Private rbtGrossWeight As Boolean = False
    Public Property p_rbtGrossWeight() As Boolean
        Get
            Return rbtGrossWeight
        End Get
        Set(ByVal value As Boolean)
            rbtGrossWeight = value
        End Set
    End Property
    Private rbtNetWeight As Boolean = False
    Public Property p_rbtNetWeight() As Boolean
        Get
            Return rbtNetWeight
        End Get
        Set(ByVal value As Boolean)
            rbtNetWeight = value
        End Set
    End Property
    Private rbtPureWeight As Boolean = True
    Public Property p_rbtPureWeight() As Boolean
        Get
            Return rbtPureWeight
        End Get
        Set(ByVal value As Boolean)
            rbtPureWeight = value
        End Set
    End Property
    Private chkWithNillBalance As Boolean = True
    Public Property p_chkWithNillBalance() As Boolean
        Get
            Return chkWithNillBalance
        End Get
        Set(ByVal value As Boolean)
            chkWithNillBalance = value
        End Set
    End Property
    Private chkAmtBal As Boolean = True
    Public Property p_chkAmtBal() As Boolean
        Get
            Return chkAmtBal
        End Get
        Set(ByVal value As Boolean)
            chkAmtBal = value
        End Set
    End Property
    Private chkRelatedTran As Boolean = True
    Public Property p_chkRelatedTran() As Boolean
        Get
            Return chkRelatedTran
        End Get
        Set(ByVal value As Boolean)
            chkRelatedTran = value
        End Set
    End Property
    Private chkSpecificFormat As Boolean = False
    Public Property p_chkSpecificFormat() As Boolean
        Get
            Return chkSpecificFormat
        End Get
        Set(ByVal value As Boolean)
            chkSpecificFormat = value
        End Set
    End Property
    Private chkLocal As Boolean = True
    Public Property p_chkLocal() As Boolean
        Get
            Return chkLocal
        End Get
        Set(ByVal value As Boolean)
            chkLocal = value
        End Set
    End Property
    Private chkout As Boolean = True
    Public Property p_chkout() As Boolean
        Get
            Return chkout
        End Get
        Set(ByVal value As Boolean)
            chkout = value
        End Set
    End Property
    Private chkBoth As Boolean = False
    Public Property p_chkBoth() As Boolean
        Get
            Return chkBoth
        End Get
        Set(ByVal value As Boolean)
            chkBoth = value
        End Set
    End Property
    Private chkWithWast As Boolean = True
    Public Property p_chkWithWast() As Boolean
        Get
            Return chkWithWast
        End Get
        Set(ByVal value As Boolean)
            chkWithWast = value
        End Set
    End Property
    Private chkStnWtConv As Boolean = True
    Public Property p_chkStnWtConv() As Boolean
        Get
            Return chkStnWtConv
        End Get
        Set(ByVal value As Boolean)
            chkStnWtConv = value
        End Set
    End Property
    Private chkWithAppBal As Boolean = False
    Public Property p_chkWithAppBal() As Boolean
        Get
            Return chkWithAppBal
        End Get
        Set(ByVal value As Boolean)
            chkWithAppBal = value
        End Set
    End Property
    Private rbtSummary As Boolean = False
    Public Property p_rbtSummary() As Boolean
        Get
            Return rbtSummary
        End Get
        Set(ByVal value As Boolean)
            rbtSummary = value
        End Set
    End Property
    Private rbtDetailed As Boolean = False
    Public Property p_rbtDetailed() As Boolean
        Get
            Return rbtDetailed
        End Get
        Set(ByVal value As Boolean)
            rbtDetailed = value
        End Set
    End Property


End Class