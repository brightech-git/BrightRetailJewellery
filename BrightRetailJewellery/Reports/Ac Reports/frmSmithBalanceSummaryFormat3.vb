Imports System.Data.OleDb
Public Class frmSmithBalanceSummaryFormat3
    Dim objGridShower As frmGridDispDia
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
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
        Dim LocalOutSt As String = ""
        If rbtLocal.Checked Then
            LocalOutSt = "L"
        ElseIf rbtOutstation.Checked Then
            LocalOutSt = "O"
        End If

        For cnt As Integer = 0 To chkCmbTranType.CheckedItems.Count - 1
            selTran += Trim(chkCmbTranType.CheckedItems.Item(cnt).ToString) & ","
        Next
        If selTran <> "" Then selTran = Mid(selTran, 1, selTran.Length - 1)
        trantype = GetTranType(selTran)
        If trantype = "" Then trantype = "''"
        Dim Apptrantype As String = ""

        If ChkApproval.Checked = False Then Apptrantype = "IAP,RAP,AI,AR"
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
        Dim chknb As String = IIf(chkWithNillBalance.Checked = True, "Y", "N")
        chkwt = ""
        If rbtGrsWt.Checked Then chkwt = "G"
        If rbtNetWt.Checked Then chkwt = "N"
        If rbtPureWt.Checked Then chkwt = "P"

        strSql = "EXEC " & cnAdminDb & "..PROC_SMITHABSTRACT_FORMAT3"
        strSql += vbCrLf + " @DBNAME='" & cnStockDb & "'"
        strSql += vbCrLf + ",@TEMPTAB= 'TEMP" & SYSID & "ABSTRACT'"
        strSql += vbCrLf + ",@FROMDATE='" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + ",@TODATE='" & dtpTodate.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + ",@WEIGHT='" & chkwt & "'"
        strSql += vbCrLf + ",@METALID='" & Selectedmetalid & "'"
        strSql += vbCrLf + ",@COMPANYID='" & strCompanyId & "'"
        strSql += vbCrLf + ",@COSTID='" & SelectedCostId & "'"
        If AcTypeFilteration = "" Then
            strSql += vbCrLf + ",@ACFILTER=" & IIf(AcTypeFilteration = "", "''", AcTypeFilteration) & ""
        Else
            strSql += vbCrLf + ",@ACFILTER=""" & IIf(AcTypeFilteration = "", "''", AcTypeFilteration) & """"
        End If
        strSql += vbCrLf + ",@TRANFILTER=" & Replace(trantype, "','", ",") & ""
        strSql += vbCrLf + ",@CATFILTER='" & Selectedcatid & "'"
        strSql += vbCrLf + ",@NILBALANCE='" & chknb & "'"
        strSql += vbCrLf + ",@LOCALOUT='" & LocalOutSt & "'"
        strSql += vbCrLf + ",@APPTRANFILTER='" & IIf(ChkApproval.Checked = False, "N", "") & "'"
        strSql += vbCrLf + ",@PUREWTPER=" & PurewtPer & ""
        strSql += vbCrLf + ",@ACCODE='" & Accode & "'"
        strSql += vbCrLf + ",@WITHWAST='" & IIf(ChkwithWast.Checked, "Y", "N") & "'"
        da = New OleDbDataAdapter(strSql, cn)
        Dim ds1 As New DataSet()
        da.Fill(ds1)
        dtGrid = ds1.Tables(0)
        Prop_Sets()
        Dim tit As String = ""
        tit += "SMITH ABSTRACT FROM " & dtpAsOnDate.Text & " TO " & dtpTodate.Text
        tit += " WITH"
        If rbtGrsWt.Checked Then
            tit += "  GROSS WEIGHT,"
        End If
        If rbtNetWt.Checked Then
            tit += "  NET WEIGHT,"
        End If
        If rbtPureWt.Checked Then
            tit += "  PURE WEIGHT,"
        End If
        tit = Mid(tit, 1, tit.Length - 1)
        If chkCostName <> "" Then tit += vbNewLine & " COSTCENTRE :" & Replace(chkCostName, "'", "")
        Dim objTitle As New CrystalDecisions.Shared.ParameterValues
        Dim objTitle1 As New CrystalDecisions.Shared.ParameterDiscreteValue
        objTitle1.Value = tit
        objTitle.Add(objTitle1)
        Dim objReport As New GiritechReport
        Dim objRptViewer As New frmReportViewer
        Dim objrptSmitSummary As New rptSmithSummary
        objrptSmitSummary.SetParameterValue("rptTitle", objTitle)
        objRptViewer.CrystalReportViewer1.ReportSource = objReport.ShowReport(objrptSmitSummary, cnDataSource)
        objRptViewer.MdiParent = Main
        objRptViewer.ShowInTaskbar = False
        objRptViewer.WindowState = FormWindowState.Maximized
        objRptViewer.Dock = DockStyle.Fill
        objRptViewer.Show()
        objRptViewer.CrystalReportViewer1.Select()
    End Sub

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

    Private Sub frmSmithBalanceSummaryReport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmSmithBalanceSummaryReport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
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
        strSql = " SELECT 'ALL' TTYPE,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT 'ISSUE',2 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT 'RECEIPT',3 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT 'INTERNAL TRANSFER',4 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT 'APPROVAL ISSUE',5 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT 'APPROVAL RECEIPT',6 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT 'OTHER ISSUE',7 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT 'OTHER RECEIPT',8 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT 'PURCHASE RETURN',9 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT 'PURCHASE',10 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT 'MISC ISSUE',11 RESULT"
        strSql += " ORDER BY result"
        DtTrantype = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(DtTrantype)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbTranType, DtTrantype, "TTYPE", , "ALL")
        'FillAcname()
        btnNew_Click(Me, New EventArgs)
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


        strSql = " SELECT 'ALL' ACNAME,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT ACNAME,2 RESULT FROM " & cnAdminDb & "..ACHEAD WHERE"
        'strSql += "  ISNULL(ACTIVE,'Y') <> 'H' "
        If ActIVE = "B" Then
            strSql += "  ISNULL(ACTIVE,'Y') <> 'H' "
        ElseIf ActIVE = "Y" Then
            strSql += "  ISNULL(ACTIVE,'') = 'Y' "
        Else
            strSql += "  ISNULL(ACTIVE,'') ='N' "
        End If
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
            strsql = " SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY "
            strSql += " WHERE ISNULL(LEDGERPRINT,'') <> 'N' AND METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalNames & "))"
            strsql += " ORDER BY CATNAME"
            FillCheckedListBox(strsql, chkLstCategory)
        End If
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        dtpAsOnDate.Value = GetServerDate()
        cmbactive.Items.Clear()
        cmbactive.Items.Add("BOTH VIEW")
        cmbactive.Items.Add("ACTIVE ONLY")
        cmbactive.Items.Add("IN ACTIVE ONLY")
        cmbactive.SelectedIndex = 0
        FillAcname()
        Prop_Gets()
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New frmSmithBalanceSummaryFormat3_Properties
        obj.p_dtpAsOnDate = dtpAsOnDate.Value.Date
        GetChecked_CheckedList(chkCmbTranType, obj.p_chkCmbTrantype)
        obj.p_chkCostCentreSelectAll = chkCostCentreSelectAll.Checked
        obj.p_chkMetalSelectAll = chkMetalSelectAll.Checked
        GetChecked_CheckedList(chkLstMetal, obj.p_chkLstMetal)
        obj.p_chkCategorySelectAll = chkCategorySelectAll.Checked
        GetChecked_CheckedList(chkLstCategory, obj.p_chkLstCategory)
        obj.p_chkDealer = chkDealer.Checked
        obj.p_chkSmith = chkSmith.Checked
        obj.p_chkOthers = chkOthers.Checked
        obj.p_chkCustomer = chkCustomer.Checked
        obj.p_rbtGrossWeight = rbtGrsWt.Checked
        obj.p_rbtNetWeight = rbtNetWt.Checked
        obj.p_rbtPureWeight = rbtPureWt.Checked
        obj.p_chkWithNillBalance = chkWithNillBalance.Checked
        obj.p_chkLocal = rbtLocal.Checked
        obj.p_chkout = rbtOutstation.Checked
        obj.p_chkBoth = rbtBothMU.Checked
        obj.p_chkWithWast = ChkwithWast.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmSmithBalanceSummaryFormat3_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmSmithBalanceSummaryFormat3_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmSmithBalanceSummaryFormat3_Properties))
        SetChecked_CheckedList(chkCmbTranType, obj.p_chkCmbTrantype, "ALL")
        chkCostCentreSelectAll.Checked = obj.p_chkCostCentreSelectAll
        chkMetalSelectAll.Checked = obj.p_chkMetalSelectAll
        SetChecked_CheckedList(chkLstMetal, obj.p_chkLstMetal, Nothing)
        chkCategorySelectAll.Checked = obj.p_chkCategorySelectAll
        SetChecked_CheckedList(chkLstCategory, obj.p_chkLstCategory, Nothing)
        chkDealer.Checked = obj.p_chkDealer
        chkSmith.Checked = obj.p_chkSmith
        chkOthers.Checked = obj.p_chkOthers
        chkCustomer.Checked = obj.p_chkCustomer
        rbtGrsWt.Checked = obj.p_rbtGrossWeight
        rbtNetWt.Checked = obj.p_rbtNetWeight
        rbtPureWt.Checked = obj.p_rbtPureWeight
        chkWithNillBalance.Checked = obj.p_chkWithNillBalance
        ChkwithWast.Checked = obj.p_chkWithWast
        rbtLocal.Checked = obj.p_chkLocal
        rbtOutstation.Checked = obj.p_chkout
        rbtBothMU.Checked = obj.p_chkBoth
    End Sub

    Private Sub chkCmbTranType_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCmbTranType.LostFocus
        If chkCmbTranType.Text = "ALL" Then
            ChkApproval.Enabled = True
            ChkApproval.Checked = True
        ElseIf chkCmbTranType.Text = "APPROVAL ISSUE" Or chkCmbTranType.Text = "APPROVAL RECEIPT" Then
            ChkApproval.Checked = True
        Else
            ChkApproval.Enabled = False
            ChkApproval.Checked = False
        End If
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

    Private Sub cmbactive_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbactive.SelectedIndexChanged
        If cmbactive.Text = "BOTH VIEW" Then
            ActIVE = "B"
        ElseIf cmbactive.Text = "ACTIVE ONLY" Then
            ActIVE = "Y"
        Else
            ActIVE = "N"
        End If
        CmbAcname.Items.Clear()
        FillAcname()
    End Sub
End Class

Public Class frmSmithBalanceSummaryFormat3_Properties
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
    Private chkWithAppBal As Boolean = False
    Public Property p_chkWithAppBal() As Boolean
        Get
            Return chkWithAppBal
        End Get
        Set(ByVal value As Boolean)
            chkWithAppBal = value
        End Set
    End Property
End Class