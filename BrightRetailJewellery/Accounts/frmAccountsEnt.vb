Imports System.Data.OleDb
Public Class frmAccountsEnt
    'Calno 180114 :Vasanthan: Client- ALL
    Dim da As OleDbDataAdapter
    Dim strSql As String
    Dim dtGridView As New DataTable
    Dim searchDia As Boolean
    Dim vouchType As VoucherType = VoucherType.Receipt
    Dim searchSender As Control = Nothing
    Dim cmd As OleDbCommand
    Public BatchNo As String = Nothing
    Public TranNo As Integer = Nothing
    Public CostId As String = Nothing
    Public payMode As String = Nothing
    Public Edchqno As String = Nothing
    Public Edchqdate As Date = Nothing
    Public Edchqdetail As String = Nothing
    Dim BL_Acc As New BL_RetailBill
    Dim objTds As New frmAccTds
    Dim objBankDetail As New frmAccBankDetails
    Dim objOutStDt As New frmAccOutstanding
    Dim objInvNo As New frmInvdet
    Dim objSledger As New frmSubLedger
    Dim dsOutStDtCol As New DataSet
    Dim dsSl As New DataSet
    Dim AutoCompleteAccName As New AutoCompleteStringCollection
    Dim dtAccNames As New DataTable
    Dim narration As New DataTable
    Dim dtWeightDetail As New DataTable
    Dim tdsAc As String = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnStockDb & "..SOFTCONTROLTRAN WHERE CTLID = 'TDS'")
    Dim editFlag As Boolean = False
    Dim editDt As New DataTable

    Dim ctrlId As String
    Dim mnuId As String
    Dim _Title As String
    Dim DrCap As String = "Dr"
    Dim CrCap As String = "Cr"
    Dim Approval As Boolean = False
    Dim VouchFilteration As String
    Dim budval As String = ""
    Dim chkval As Double
    Dim Ischkbudget As Boolean = IIf(GetAdmindbSoftValue("BUDGETCONTROL", "N") = "Y", True, False)
    Dim IsdispBalance As Boolean = IIf(GetAdmindbSoftValue("ACCBALANCE_DISP", "Y") = "Y", True, False)
    'IIf(GetAdmindbSoftValue("ESTMSGBOX", "Y") = "Y", True, False)
    Dim OS_ADJ_COSTCENTRE As Boolean = IIf(GetAdmindbSoftValue("ACC_OSADJ_COSTCENTRE", "N") = "Y", True, False)
    Dim SubCostid As Boolean = IIf(GetAdmindbSoftValue("SCOSTID", "N") = "Y", True, False)
    Dim mtempacctran As String = "TEMP" & systemId & "ACCTRAN"
    Dim SRVT_COMPONENTS As String = GetAdmindbSoftValue("SRVT_COMPONENTS", "")
    Dim SrvTaxCode As String = ""
    Dim SRVTID As String
    Dim SRVTPER As Double
    Dim SRVTAX As Boolean = False
    Dim OTP_Access As Boolean = False
    Dim ACCENTRY_DATE As Boolean = IIf(GetAdmindbSoftValue("ACCENTRY_DATE", "G") = "S", True, False)
    Dim SMS_MSG_PAYMENT As String = objGPack.GetSqlValue("SELECT ISNULL(TEMPLATE_DESC,'') AS TEMPLATE_DESC FROM " & cnAdminDb & "..SMSTEMPLATE WHERE TEMPLATE_NAME='SMS_MSG_PAYMENT' AND ISNULL(ACTIVE,'Y')<>'N'", "TEMPLATE_DESC").ToString
    Dim InterStateBill As Boolean
    Dim DNCNGST_FLAG As Boolean
    Dim PCODE As String = ""
    Dim ACCENTRYACTYPE_LOCK As String = GetAdmindbSoftValue("ACCENTRYACTYPE_LOCK", "")
    Dim Auto_GST_DNCN As String = GetAdmindbSoftValue("AUTOGSTDNCN", "N")
    Dim ONETIMEFLAGINVOICENOANDDATE As Boolean = False
    Public Sub New(ByVal vouchType As VoucherType, ByVal title As String, ByVal ctrlId As String, ByVal mnuId As String, ByVal Approval As Boolean)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        Initialize()
        Dim dtCol As New DataColumn("KEYNO", GetType(Integer))
        dtCol.AutoIncrement = True
        dtCol.AutoIncrementSeed = 1
        dtCol.AutoIncrementStep = 1
        If Not dtGridView.Columns.Contains("KEYNO") Then dtGridView.Columns.Add(dtCol)
        Me.mnuId = mnuId
        Me.ctrlId = ctrlId
        Me.vouchType = vouchType
        Me.Approval = Approval
        lblTitle.Text = title
        Me._Title = title
        If title = "DEBIT NOTE" Then
            strSql = " SELECT 'SALES' TITLE,'SA' ENTRYTYPE "
            strSql += " UNION ALL"
            strSql += " SELECT 'PURCHASE RETURN' TITLE,'IPU' ENTRYTYPE "
        ElseIf title = "CREDIT NOTE" Then
            strSql = " SELECT 'SALES RETURN' TITLE,'SR' ENTRYTYPE "
            strSql += " UNION ALL"
            strSql += " SELECT 'PURCHASE ' TITLE,'PU' ENTRYTYPE "
        End If
        If title = "DEBIT NOTE" Or title = "CREDIT NOTE" Then
            strSql += " UNION ALL"
            strSql += " SELECT 'EXPENSE' TITLE,'PE' ENTRYTYPE "
            strSql += " UNION ALL"
            strSql += " SELECT 'JOBWORK' TITLE,'JB' ENTRYTYPE "
        Else
            strSql = " SELECT '' TITLE,'' ENTRYTYPE "
        End If
        Dim dtFilter As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtFilter)
        cmbAgainst_Own.DataSource = dtFilter
        cmbAgainst_Own.DisplayMember = "TITLE"
        cmbAgainst_Own.ValueMember = "ENTRYTYPE"
        cmbAgainst_Own.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cmbAgainst_Own.AutoCompleteSource = AutoCompleteSource.ListItems
        SetDrCrCaption()
    End Sub

    Private Sub SetDrCrCaption()
        Dim typ As String = Nothing
        Select Case vouchType
            Case VoucherType.Receipt
                typ = "R"
            Case VoucherType.Payment
                typ = "P"
            Case VoucherType.Journal
                typ = "J"
            Case VoucherType.DebitNote
                typ = "D"
            Case VoucherType.CreditNote
                typ = "C"
        End Select
        DrCap = objGPack.GetSqlValue("SELECT DRCAPTION FROM " & cnAdminDb & "..ACCENTRYMASTER WHERE TYPE = '" & typ & "' AND ISNULL(DRCAPTION,'') <> ''", , DrCap)
        CrCap = objGPack.GetSqlValue("SELECT CRCAPTION FROM " & cnAdminDb & "..ACCENTRYMASTER WHERE TYPE = '" & typ & "' AND ISNULL(CRCAPTION,'') <> ''", , CrCap)
        lstSearch.Items.Clear()
        lstSearch.Items.Add(DrCap)
        lstSearch.Items.Add(CrCap)
    End Sub

    Public Sub New(ByVal payMode As String, ByVal dtView As DataTable)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        Initialize()
        Dim dtCol As New DataColumn("KEYNO", GetType(Integer))
        dtCol.AutoIncrement = True
        dtCol.AutoIncrementSeed = 1
        dtCol.AutoIncrementStep = 1
        If Not dtGridView.Columns.Contains("KEYNO") Then dtGridView.Columns.Add(dtCol)

        dtCol = New DataColumn("KEYNO", GetType(Integer))
        dtCol.AutoIncrement = True
        dtCol.AutoIncrementSeed = 1
        dtCol.AutoIncrementStep = 1
        If Not dtView.Columns.Contains("KEYNO") Then dtView.Columns.Add(dtCol)

        strSql = " SELECT TYPE FROM " & cnAdminDb & "..ACCENTRYMASTER WHERE PAYMODE = '" & payMode & "'"
        'Dim type As String = objGPack.GetSqlValue(strSql)
        strSql = " SELECT DISPLAYTEXT FROM " & cnAdminDb & "..ACCENTRYMASTER WHERE PAYMODE = '" & payMode & "'"
        'Dim dispText As String = objGPack.GetSqlValue(strSql)

        strSql = " SELECT TYPE,DISPLAYTEXT,SNO  FROM " & cnAdminDb & "..ACCENTRYMASTER WHERE PAYMODE = '" & payMode & "'"
        Dim dritem As DataRow = GetSqlRow(strSql, cn)
        Dim type As String = dritem.Item(0)
        Dim dispText As String = dritem.Item(1)
        mnuId = dritem.Item(2)

        ctrlId = "GEN-" & payMode
        strSql = " SELECT 'CHECK' FROM " & cnStockDb & "..BILLCONTROL"
        strSql += vbCrLf + "WHERE CTLID = '" & ctrlId & "'"
        strSql += vbCrLf + "AND COMPANYID = '" & strCompanyId & "'"
        If Not objGPack.GetSqlValue(strSql).Length > 0 Then
            MsgBox(dispText & " Billno Generation Controlid Not Found", MsgBoxStyle.Information)
        End If
        lblTitle.Text = dispText
        Select Case type
            Case "R"
                Me.vouchType = VoucherType.Receipt
            Case "P"
                Me.vouchType = VoucherType.Payment
            Case "J"
                Me.vouchType = VoucherType.Journal
            Case "D"
                Me.vouchType = VoucherType.DebitNote
            Case "C"
                Me.vouchType = VoucherType.CreditNote
        End Select
        'mnuId = objGPack.GetSqlValue("SELECT SNO FROM " & cnAdminDb & "..ACCENTRYMASTER WHERE PAYMODE = '" & payMode & "'")
        SetDrCrCaption()
        editFlag = True
        btnNew_Click(Me, New EventArgs)
        btnNew.Enabled = False
        editDt = dtView
    End Sub

    'Public Sub New()
    '    ' This call is required by the Windows Form Designer.
    '    InitializeComponent()
    '    Initialize()
    'End Sub

    Private Sub LoadAcc()
        Dim ftr As String = Nothing
        Select Case vouchType
            Case VoucherType.Receipt
                If UCase(gridView_OWN.CurrentRow.Cells("type").Value.ToString) = CrCap.ToUpper Then
                    'ftr = "acgrpcode not in ('1','2')"
                Else
                    'ftr = "acgrpcode in ('1','2')"
                    ftr = "actype in ('B','H')"
                End If
            Case VoucherType.Payment
                If UCase(gridView_OWN.CurrentRow.Cells("type").Value.ToString) = DrCap.ToUpper Then
                    'ftr = "acgrpcode not in ('1','2')"
                Else
                    'ftr = "acgrpcode in ('1','2')"
                    ftr = "actype in ('B','H')"
                End If
            Case Else
                ftr = Nothing
        End Select
        VouchFilteration = ftr
        LoadAccNames(ftr)
    End Sub

    Private Sub LoadAccNames(ByVal ftr As String)
        'Dim dv As New DataView
        'dv = dtAccNames.DefaultView
        'dv.RowFilter = ftr
        'AutoCompleteAccName = New AutoCompleteStringCollection
        'For Each ro As DataRow In dv.ToTable.Rows
        '    AutoCompleteAccName.Add(ro.Item(0).ToString)
        'Next
    End Sub

    Public Enum VoucherType
        Receipt = 0
        Payment = 1
        Journal = 2
        DebitNote = 3
        CreditNote = 4
    End Enum

    Private Sub ACNAMEFILTER(ByVal _acgrpcode As Integer)
        strSql = " SELECT " & cnAdminDb & ".DBO.WORDCAPS(ACNAME)ACNAME,ACGRPCODE,ACTYPE FROM " & cnAdminDb & "..ACHEAD WHERE 1=1"
        strSql += vbCrLf + "AND ISNULL(MACCODE,'') = ''"
        If _acgrpcode > 0 Then strSql += vbCrLf + " and ACGRPCODE = '" & _acgrpcode & "'"
        strSql += GetAcNameQryFilteration()
        strSql += vbCrLf + "ORDER BY ACNAME"
        da = New OleDbDataAdapter(strSql, cn)
        dtAccNames = New DataTable
        da.Fill(dtAccNames)
        DgvSearch.DataSource = dtAccNames
        DgvSearch.Columns("ACGRPCODE").Visible = False
        DgvSearch.Columns("ACTYPE").Visible = False
        DgvSearch.ColumnHeadersVisible = False
        DgvSearch.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DgvSearch.Visible = False
    End Sub

    Private Sub Initialize()
        strSql = " SELECT ''TYPE,''DESCRIPTION,''BALANCE,''NARRATION1,''NARRATION2,''SCOSTCENTRE,''CHQNO"
        strSql += vbCrLf + ",CONVERT(SMALLDATETIME,NULL)CHQDATE,''CHQDETAIL"
        strSql += vbCrLf + ",CONVERT(NUMERIC(15,2),NULL)TDSPER,CONVERT(NUMERIC(15,2),NULL)TDSAMT,CONVERT(INT,NULL)TDSCATID"
        strSql += vbCrLf + ",CONVERT(NUMERIC(15,2),NULL)SRVTPER,CONVERT(NUMERIC(15,2),NULL)SRVTAMT"
        strSql += vbCrLf + ",CONVERT(NUMERIC(15,2),NULL)VATPER,CONVERT(NUMERIC(15,2),NULL)VATAMT,CONVERT(VARCHAR(10),NULL)VATCATID"
        strSql += vbCrLf + ",CONVERT(NUMERIC(15,2),NULL)SGSTPER,CONVERT(NUMERIC(15,2),NULL)SGST,CONVERT(VARCHAR(10),NULL)SGSTID"
        strSql += vbCrLf + ",CONVERT(NUMERIC(15,2),NULL)CGSTPER,CONVERT(NUMERIC(15,2),NULL)CGST,CONVERT(VARCHAR(10),NULL)CGSTID"
        strSql += vbCrLf + ",CONVERT(NUMERIC(15,2),NULL)IGSTPER,CONVERT(NUMERIC(15,2),NULL)IGST,CONVERT(VARCHAR(10),NULL)IGSTID"
        strSql += vbCrLf + ",CONVERT(NUMERIC(15,2),NULL)DEBIT,CONVERT(NUMERIC(15,2),NULL)CREDIT"
        strSql += vbCrLf + ",CONVERT(VARCHAR,NULL)GENBY WHERE 1<>1"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGridView)

        'strSql = " SELECT " & cnAdminDb & ".DBO.WORDCAPS(ACNAME)ACNAME,ACGRPCODE,ACTYPE FROM " & cnAdminDb & "..ACHEAD WHERE 1=1"
        'strSql += vbcrlf + "AND ISNULL(MACCODE,'') = ''"
        'strSql += GetAcNameQryFilteration()
        'strSql += vbcrlf + "ORDER BY ACNAME"
        'da = New OleDbDataAdapter(strSql, cn)
        'da.Fill(dtAccNames)
        'DgvSearch.DataSource = dtAccNames
        'DgvSearch.Columns("ACGRPCODE").Visible = False
        'DgvSearch.Columns("ACTYPE").Visible = False
        'DgvSearch.ColumnHeadersVisible = False
        'DgvSearch.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        'DgvSearch.Visible = False

        ACNAMEFILTER(0)

        If UCase(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COSTCENTRE'")) = "Y" Then
            strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE "
            strSql += vbCrLf + "WHERE  ISNULL(COMPANYID,'" & strCompanyId & "')LIKE'%" & strCompanyId & "%'"
            strSql += vbCrLf + "ORDER BY COSTNAME"
            objGPack.FillCombo(strSql, cmbCostCenter_MAN, False, False)
            cmbCostCenter_MAN.Enabled = True
            strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE "
            strSql += vbCrLf + "WHERE  ISNULL(COMPANYID,'" & strCompanyId & "')LIKE'%" & strCompanyId & "%'"
            strSql += vbCrLf + "ORDER BY COSTNAME"
            objGPack.FillCombo(strSql, cmbSubCostCenter_MAN, False, False)
            cmbSubCostCenter_MAN.Enabled = True
        Else
            cmbCostCenter_MAN.Enabled = False
            cmbSubCostCenter_MAN.Enabled = False
        End If

        If SubCostid = True Then
            lblSubCostcentre.Visible = True
            cmbSubCostCenter_MAN.Visible = True
        Else
            lblSubCostcentre.Visible = False
            cmbSubCostCenter_MAN.Visible = False
        End If

        dtWeightDetail = New DataTable
        With dtWeightDetail.Columns
            .Add("CATNAME", GetType(String))
            .Add("PCS", GetType(Integer))
            .Add("GRSWT", GetType(Decimal))
            .Add("NETWT", GetType(Decimal))
            .Add("PUREWT", GetType(Decimal))
            .Add("CALCMODE", GetType(String))
            .Add("UNIT", GetType(String))
            .Add("RATE", GetType(Decimal))
            .Add("AMOUNT", GetType(Decimal))
            .Add("KEYNO", GetType(Integer))
            .Add("TYPE", GetType(String))
            .Add("REFNO", GetType(String))
            .Add("REFDATE", GetType(String))
            .Add("VAT", GetType(Decimal))
            .Add("SGST", GetType(Decimal))
            .Add("CGST", GetType(Decimal))
            .Add("IGST", GetType(Decimal))
        End With
    End Sub

    Private Sub ClearDt(ByVal dt As DataTable)
        dt.Rows.Clear()
        For cnt As Integer = 1 To 50
            dt.Rows.Add()
        Next
        dt.AcceptChanges()
    End Sub

    Private Sub StyleGridView(ByVal grid As DataGridView)
        With grid
            .Columns("KEYNO").Visible = False
            .Columns("TYPE").Width = 130
            .Columns("TYPE").SortMode = DataGridViewColumnSortMode.NotSortable
            .Columns("DESCRIPTION").Width = 450
            .Columns("DESCRIPTION").SortMode = DataGridViewColumnSortMode.NotSortable
            .Columns("NARRATION1").Visible = False
            .Columns("NARRATION2").Visible = False
            .Columns("CHQNO").Visible = False
            .Columns("CHQDATE").Visible = False
            .Columns("CHQDETAIL").Visible = False
            .Columns("SRVTPER").Visible = False
            .Columns("SRVTAMT").Visible = False

            .Columns("TDSPER").Visible = False
            .Columns("TDSAMT").Visible = False
            .Columns("TDSCATID").Visible = False
            .Columns("VATPER").Visible = False
            .Columns("VATAMT").Visible = False
            .Columns("VATCATID").Visible = False
            .Columns("SCOSTCENTRE").Visible = False
            .Columns("GENBY").Visible = False
            .Columns("SGSTPER").Visible = False
            .Columns("SGST").Visible = False
            .Columns("SGSTID").Visible = False
            .Columns("CGSTPER").Visible = False
            .Columns("CGST").Visible = False
            .Columns("CGSTID").Visible = False
            .Columns("IGSTPER").Visible = False
            .Columns("IGST").Visible = False
            .Columns("IGSTID").Visible = False
            .Columns("BALANCE").Width = 152
            .Columns("BALANCE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("BALANCE").SortMode = DataGridViewColumnSortMode.NotSortable
            .Columns("BALANCE").DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            .Columns("BALANCE").DefaultCellStyle.ForeColor = Color.Red
            .Columns("DEBIT").Width = 120
            .Columns("DEBIT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("DEBIT").DefaultCellStyle.Format = "0.00"
            .Columns("DEBIT").SortMode = DataGridViewColumnSortMode.NotSortable
            .Columns("CREDIT").Width = 120
            .Columns("CREDIT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("CREDIT").DefaultCellStyle.Format = "0.00"
            .Columns("CREDIT").SortMode = DataGridViewColumnSortMode.NotSortable
            For cnt As Integer = 0 To .ColumnCount - 1
                .Columns(cnt).Resizable = DataGridViewTriState.False
            Next
        End With
    End Sub

    Private Sub frmAccountsEnt_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            gridView_OWN.Focus()
        End If
    End Sub

    Private Sub frmAccountsEnt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtGrid_OWN.Focused Then Exit Sub
            If DgvSearch.Focused Then Exit Sub
            'If Dgv1Search.Focused Then Exit Sub    
            If gridView_OWN.Focused Then Exit Sub
            'If cmbNarration.Focused Then Exit Sub  
            If txtNarration1.Focused Then Exit Sub
            If txtNarration2.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub ShowWeightDetailGST()
Loadwt:
        If objGPack.GetSqlValue("SELECT INVENTORY FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & txtGrid_OWN.Text & "'") <> "Y" Then
            Exit Sub
        End If
        Dim frm As New frmState
        If frm.ShowDialog() = Windows.Forms.DialogResult.OK Then
            If frm.rbtOwn.Checked Then
                InterStateBill = False
            Else
                InterStateBill = True
            End If
        End If
        Dim updFlag As Boolean = False
        Dim ro() As DataRow
        ro = dtWeightDetail.Select("KEYNO = " & Val(gridView_OWN.CurrentRow.Cells("KEYNO").Value.ToString) & "")
        Dim f As New AccEntryWeightDetailGST(gridView_OWN.CurrentRow.Cells("TYPE").Value.ToString)
        f.InterStateBill = InterStateBill
        If ro.Length > 0 Then
            f.cmbWCategory_MAN.Text = ro(0).Item("CATNAME").ToString
            f.txtWPcs_NUM.Text = IIf(Val(ro(0).Item("PCS").ToString) <> 0, Val(ro(0).Item("PCS").ToString), "")
            f.txtWGrsWt_WET.Text = IIf(Val(ro(0).Item("GRSWT").ToString) <> 0, Format(Val(ro(0).Item("GRSWT").ToString), "0.000"), "")
            f.txtWNetWt_WET.Text = IIf(Val(ro(0).Item("NETWT").ToString) <> 0, Format(Val(ro(0).Item("NETWT").ToString), "0.000"), "")
            f.txtPureWt_WET.Text = IIf(Val(ro(0).Item("PUREWT").ToString) <> 0, Format(Val(ro(0).Item("PUREWT").ToString), "0.000"), "")
            f.cmbWCalcMode.Text = ro(0).Item("CALCMODE").ToString
            f.txtWRate_AMT.Text = IIf(Val(ro(0).Item("RATE").ToString) <> 0, Format(Val(ro(0).Item("RATE").ToString), "0.00"), "")
            f.cmbWType.Text = ro(0).Item("TYPE").ToString
            f.dtpRefDate.Value = ro(0).Item("REFDATE").ToString
            f.txtRefno.Text = ro(0).Item("REFNO").ToString
            f.txtWamt_AMT.Text = ro(0).Item("AMOUNT").ToString
            f.txtVat_AMT.Text = ro(0).Item("VAT").ToString
            f.txtSgst_AMT.Text = ro(0).Item("SGST").ToString
            f.txtCgst_AMT.Text = ro(0).Item("CGST").ToString
            f.txtIgst_AMT.Text = ro(0).Item("IGST").ToString
            updFlag = True
        End If
        If f.ShowDialog() = Windows.Forms.DialogResult.OK Then
            If updFlag Then
                For cnt As Integer = 0 To dtWeightDetail.Rows.Count - 1
                    With dtWeightDetail.Rows(cnt)
                        If dtWeightDetail.Rows(cnt).Item("KEYNO").ToString <> gridView_OWN.CurrentRow.Cells("KEYNO").Value.ToString Then Continue For
                        .Item("CATNAME") = f.cmbWCategory_MAN.Text
                        .Item("PCS") = Val(f.txtWPcs_NUM.Text)
                        .Item("GRSWT") = Val(f.txtWGrsWt_WET.Text)
                        .Item("NETWT") = Val(f.txtWNetWt_WET.Text)
                        .Item("PUREWT") = Val(f.txtPureWt_WET.Text)
                        .Item("CALCMODE") = f.cmbWCalcMode.Text
                        .Item("RATE") = Val(f.txtWRate_AMT.Text)
                        .Item("TYPE") = f.cmbWType.Text
                        .Item("REFDATE") = f.dtpRefDate.Value
                        .Item("REFNO") = f.txtRefno.Text
                        .Item("AMOUNT") = f.txtWamt_AMT.Text
                        .Item("VAT") = Val(f.txtVat_AMT.Text)
                        .Item("SGST") = Val(f.txtSgst_AMT.Text)
                        .Item("CGST") = Val(f.txtCgst_AMT.Text)
                        .Item("IGST") = Val(f.txtIgst_AMT.Text)
                        Exit For
                    End With
                Next

            Else
                Dim row As DataRow = Nothing
                row = dtWeightDetail.NewRow
                row("KEYNO") = Val(gridView_OWN.CurrentRow.Cells("KEYNO").Value.ToString)
                row("CATNAME") = f.cmbWCategory_MAN.Text
                row("PCS") = Val(f.txtWPcs_NUM.Text)
                row("GRSWT") = Val(f.txtWGrsWt_WET.Text)
                row("NETWT") = Val(f.txtWNetWt_WET.Text)
                row("PUREWT") = Val(f.txtPureWt_WET.Text)
                row("CALCMODE") = f.cmbWCalcMode.Text
                row("RATE") = Val(f.txtWRate_AMT.Text)
                row("TYPE") = f.cmbWType.Text
                row("REFDATE") = f.dtpRefDate.Value
                row("REFNO") = f.txtRefno.Text
                row("VAT") = Val(f.txtVat_AMT.Text & "")
                row("SGST") = Val(f.txtSgst_AMT.Text & "")
                row("CGST") = Val(f.txtSgst_AMT.Text & "")
                row("IGST") = Val(f.txtSgst_AMT.Text & "")
                row("AMOUNT") = Val(f.txtWamt_AMT.Text)
                dtWeightDetail.Rows.Add(row)
            End If
            dtWeightDetail.AcceptChanges()

            'wt details loading

            gridView_OWN.CurrentRow.Cells("VATPER").Value = Val(f.txtVat_per.Text)
            gridView_OWN.CurrentRow.Cells("VATAMT").Value = Val(f.txtVat_AMT.Text)
            gridView_OWN.CurrentRow.Cells("SGSTPER").Value = Val(f.txtSgstPer_PER.Text)
            gridView_OWN.CurrentRow.Cells("CGSTPER").Value = Val(f.txtCgstPer_PER.Text)
            gridView_OWN.CurrentRow.Cells("IGSTPER").Value = Val(f.txtIgstPer_PER.Text)
            gridView_OWN.CurrentRow.Cells("SGST").Value = Val(f.txtSgst_AMT.Text)
            gridView_OWN.CurrentRow.Cells("CGST").Value = Val(f.txtCgst_AMT.Text)
            gridView_OWN.CurrentRow.Cells("IGST").Value = Val(f.txtIgst_AMT.Text)

            strSql = "SELECT * FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & f.cmbWCategory_MAN.Text & "'"
            Dim dr As DataRow = GetSqlRow(strSql, cn)
            If Not dr Is Nothing Then
                If f.cmbWType.Text = "SALES" Then
                    gridView_OWN.CurrentRow.Cells("SGSTID").Value = dr("S_SGSTID").ToString
                    gridView_OWN.CurrentRow.Cells("CGSTID").Value = dr("S_CGSTID").ToString
                    gridView_OWN.CurrentRow.Cells("IGSTID").Value = dr("S_IGSTID").ToString
                Else
                    gridView_OWN.CurrentRow.Cells("SGSTID").Value = dr("P_SGSTID").ToString
                    gridView_OWN.CurrentRow.Cells("CGSTID").Value = dr("P_CGSTID").ToString
                    gridView_OWN.CurrentRow.Cells("IGSTID").Value = dr("P_IGSTID").ToString
                End If
            End If

            If vouchType = VoucherType.Journal Then
                If gridView_OWN.CurrentRow.Cells("TYPE").Value.ToString.ToUpper = DrCap.ToUpper Then '"DR"
                    gridView_OWN.CurrentRow.Cells("DEBIT").Value = Format(Val(f.txtWamt_AMT.Text), "0.00")
                ElseIf gridView_OWN.CurrentRow.Cells("TYPE").Value.ToString.ToUpper = CrCap.ToUpper Then '"CR"
                    gridView_OWN.CurrentRow.Cells("CREDIT").Value = Format(Val(f.txtWamt_AMT.Text), "0.00")
                End If
            Else
                If gridView_OWN.CurrentRow.Cells("TYPE").Value.ToString.ToUpper = DrCap.ToUpper Then '"DR"
                    gridView_OWN.CurrentRow.Cells("DEBIT").Value = Format(Val(f.txtWamt_AMT.Text), "0.00")
                ElseIf gridView_OWN.CurrentRow.Cells("TYPE").Value.ToString.ToUpper = CrCap.ToUpper Then '"CR"
                    gridView_OWN.CurrentRow.Cells("CREDIT").Value = Format(Val(f.txtWamt_AMT.Text), "0.00")
                End If
            End If
            DNCNGST_FLAG = True
        End If

    End Sub
    Private Sub ShowAmountDetailGST()
Loadwt:
        If DNCNGST_FLAG = True Then
            Exit Sub
        End If
        ''Dim frm As New frmState
        ''If frm.ShowDialog() = Windows.Forms.DialogResult.OK Then
        ''    If frm.rbtOwn.Checked Then
        ''        InterStateBill = False
        ''    Else
        ''        InterStateBill = True
        ''    End If
        ''End If
        Dim AccGstNo As String = objGPack.GetSqlValue("SELECT GSTNO FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & txtGrid_OWN.Text & "'", "GSTNO", "")
        Dim CompGstNo As String = objGPack.GetSqlValue("SELECT GSTNO FROM " & cnAdminDb & "..COMPANY WHERE COMPANYID = '" & strCompanyId & "'", "GSTNO", "")
        If AccGstNo <> "" And CompGstNo <> "" Then
            If Mid(AccGstNo, 1, 2) <> Mid(CompGstNo, 1, 2) Then
                InterStateBill = True
            Else
                InterStateBill = False
            End If
        Else
            InterStateBill = False
        End If
        Dim updFlag As Boolean = False
        Dim ro() As DataRow
        ro = dtWeightDetail.Select("KEYNO = " & Val(gridView_OWN.CurrentRow.Cells("KEYNO").Value.ToString) & "")
        Dim f As New AccEntryAmountDetailGST(gridView_OWN.CurrentRow.Cells("TYPE").Value.ToString)
        f.InterStateBill = InterStateBill
        If ro.Length > 0 Then
            f.cmbWType.Text = ro(0).Item("TYPE").ToString
            f.dtpRefDate.Value = ro(0).Item("REFDATE").ToString
            f.txtRefno.Text = ro(0).Item("REFNO").ToString
            f.txtWamt_AMT.Text = ro(0).Item("AMOUNT").ToString
            f.txtVat_AMT.Text = ro(0).Item("VAT").ToString
            f.txtSgst_AMT.Text = ro(0).Item("SGST").ToString
            f.txtCgst_AMT.Text = ro(0).Item("CGST").ToString
            f.txtIgst_AMT.Text = ro(0).Item("IGST").ToString
            updFlag = True
        End If
        If f.ShowDialog() = Windows.Forms.DialogResult.OK Then
            If updFlag Then
                For cnt As Integer = 0 To dtWeightDetail.Rows.Count - 1
                    With dtWeightDetail.Rows(cnt)
                        If dtWeightDetail.Rows(cnt).Item("KEYNO").ToString <> gridView_OWN.CurrentRow.Cells("KEYNO").Value.ToString Then Continue For
                        .Item("CATNAME") = f.cmbWCategory_MAN.Text
                        .Item("PCS") = 0
                        .Item("GRSWT") = 0
                        .Item("NETWT") = 0
                        .Item("PUREWT") = 0
                        .Item("CALCMODE") = "A"
                        .Item("RATE") = 0
                        .Item("TYPE") = f.cmbWType.Text
                        .Item("REFDATE") = f.dtpRefDate.Value
                        .Item("REFNO") = f.txtRefno.Text
                        .Item("AMOUNT") = f.txtWamt_AMT.Text
                        .Item("VAT") = Val(f.txtVat_AMT.Text)
                        .Item("SGST") = Val(f.txtSgst_AMT.Text)
                        .Item("CGST") = Val(f.txtCgst_AMT.Text)
                        .Item("IGST") = Val(f.txtIgst_AMT.Text)
                        Exit For
                    End With
                Next

            Else
                Dim row As DataRow = Nothing
                row = dtWeightDetail.NewRow
                row("KEYNO") = Val(gridView_OWN.CurrentRow.Cells("KEYNO").Value.ToString)
                row("CATNAME") = f.cmbWCategory_MAN.Text
                row("PCS") = 0
                row("GRSWT") = 0
                row("NETWT") = 0
                row("PUREWT") = 0
                row("CALCMODE") = "A"
                row("RATE") = 0
                row("TYPE") = f.cmbWType.Text
                row("REFDATE") = f.dtpRefDate.Value
                row("REFNO") = f.txtRefno.Text
                row("VAT") = Val(f.txtVat_AMT.Text & "")
                row("SGST") = Val(f.txtSgst_AMT.Text & "")
                row("CGST") = Val(f.txtSgst_AMT.Text & "")
                row("IGST") = Val(f.txtSgst_AMT.Text & "")
                row("AMOUNT") = Val(f.txtWamt_AMT.Text)
                dtWeightDetail.Rows.Add(row)
            End If
            dtWeightDetail.AcceptChanges()

            'wt details loading

            gridView_OWN.CurrentRow.Cells("VATPER").Value = Val(f.txtVat_per.Text)
            gridView_OWN.CurrentRow.Cells("VATAMT").Value = Val(f.txtVat_AMT.Text)
            gridView_OWN.CurrentRow.Cells("SGSTPER").Value = Val(f.txtSgstPer_PER.Text)
            gridView_OWN.CurrentRow.Cells("CGSTPER").Value = Val(f.txtCgstPer_PER.Text)
            gridView_OWN.CurrentRow.Cells("IGSTPER").Value = Val(f.txtIgstPer_PER.Text)
            gridView_OWN.CurrentRow.Cells("SGST").Value = Val(f.txtSgst_AMT.Text)
            gridView_OWN.CurrentRow.Cells("CGST").Value = Val(f.txtCgst_AMT.Text)
            gridView_OWN.CurrentRow.Cells("IGST").Value = Val(f.txtIgst_AMT.Text)

            strSql = "SELECT * FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & f.cmbWCategory_MAN.Text & "'"
            Dim dr As DataRow = GetSqlRow(strSql, cn)
            If Not dr Is Nothing Then
                If f.cmbWType.Text = "SALES" Then
                    gridView_OWN.CurrentRow.Cells("SGSTID").Value = dr("S_SGSTID").ToString
                    gridView_OWN.CurrentRow.Cells("CGSTID").Value = dr("S_CGSTID").ToString
                    gridView_OWN.CurrentRow.Cells("IGSTID").Value = dr("S_IGSTID").ToString
                Else
                    gridView_OWN.CurrentRow.Cells("SGSTID").Value = dr("P_SGSTID").ToString
                    gridView_OWN.CurrentRow.Cells("CGSTID").Value = dr("P_CGSTID").ToString
                    gridView_OWN.CurrentRow.Cells("IGSTID").Value = dr("P_IGSTID").ToString
                End If
            End If

            If vouchType = VoucherType.Journal Then
                If gridView_OWN.CurrentRow.Cells("TYPE").Value.ToString.ToUpper = DrCap.ToUpper Then '"DR"
                    gridView_OWN.CurrentRow.Cells("DEBIT").Value = Format(Val(f.txtWamt_AMT.Text), "0.00")
                ElseIf gridView_OWN.CurrentRow.Cells("TYPE").Value.ToString.ToUpper = CrCap.ToUpper Then '"CR"
                    gridView_OWN.CurrentRow.Cells("CREDIT").Value = Format(Val(f.txtWamt_AMT.Text), "0.00")
                End If
            Else
                If gridView_OWN.CurrentRow.Cells("TYPE").Value.ToString.ToUpper = DrCap.ToUpper Then '"DR"
                    gridView_OWN.CurrentRow.Cells("DEBIT").Value = Format(Val(f.txtWamt_AMT.Text), "0.00")
                ElseIf gridView_OWN.CurrentRow.Cells("TYPE").Value.ToString.ToUpper = CrCap.ToUpper Then '"CR"
                    gridView_OWN.CurrentRow.Cells("CREDIT").Value = Format(Val(f.txtWamt_AMT.Text), "0.00")
                End If
            End If
            DNCNGST_FLAG = True
        End If

    End Sub
    Private Sub ShowWeightDetail()
Loadwt:
        If objGPack.GetSqlValue("SELECT INVENTORY FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & txtGrid_OWN.Text & "'") <> "Y" Then
            Exit Sub
        End If
        Dim updFlag As Boolean = False
        Dim ro() As DataRow
        ro = dtWeightDetail.Select("KEYNO = " & Val(gridView_OWN.CurrentRow.Cells("KEYNO").Value.ToString) & "")
        Dim f As New AccEntryWeightDetail(gridView_OWN.CurrentRow.Cells("TYPE").Value.ToString)
        If ro.Length > 0 Then
            f.cmbWCategory_MAN.Text = ro(0).Item("CATNAME").ToString
            f.txtWPcs_NUM.Text = IIf(Val(ro(0).Item("PCS").ToString) <> 0, Val(ro(0).Item("PCS").ToString), "")
            f.txtWGrsWt_WET.Text = IIf(Val(ro(0).Item("GRSWT").ToString) <> 0, Format(Val(ro(0).Item("GRSWT").ToString), "0.000"), "")
            f.txtWNetWt_WET.Text = IIf(Val(ro(0).Item("NETWT").ToString) <> 0, Format(Val(ro(0).Item("NETWT").ToString), "0.000"), "")
            f.txtPureWt_WET.Text = IIf(Val(ro(0).Item("PUREWT").ToString) <> 0, Format(Val(ro(0).Item("PUREWT").ToString), "0.000"), "")
            f.cmbWCalcMode.Text = ro(0).Item("CALCMODE").ToString
            f.txtWRate_AMT.Text = IIf(Val(ro(0).Item("RATE").ToString) <> 0, Format(Val(ro(0).Item("RATE").ToString), "0.00"), "")
            f.cmbWType.Text = ro(0).Item("TYPE").ToString
            f.dtpRefDate.Value = ro(0).Item("REFDATE").ToString
            f.txtRefno.Text = ro(0).Item("REFNO").ToString
            f.txtWamt_AMT.Text = ro(0).Item("AMOUNT").ToString
            f.txtVat_AMT.Text = ro(0).Item("VAT").ToString
            updFlag = True
        End If

        If f.ShowDialog() = Windows.Forms.DialogResult.OK Then
            If updFlag Then
                For cnt As Integer = 0 To dtWeightDetail.Rows.Count - 1
                    With dtWeightDetail.Rows(cnt)
                        If dtWeightDetail.Rows(cnt).Item("KEYNO").ToString <> gridView_OWN.CurrentRow.Cells("KEYNO").Value.ToString Then Continue For
                        .Item("CATNAME") = f.cmbWCategory_MAN.Text
                        .Item("PCS") = Val(f.txtWPcs_NUM.Text)
                        .Item("GRSWT") = Val(f.txtWGrsWt_WET.Text)
                        .Item("NETWT") = Val(f.txtWNetWt_WET.Text)
                        .Item("PUREWT") = Val(f.txtPureWt_WET.Text)
                        .Item("CALCMODE") = f.cmbWCalcMode.Text
                        .Item("RATE") = Val(f.txtWRate_AMT.Text)
                        .Item("TYPE") = f.cmbWType.Text
                        .Item("REFDATE") = f.dtpRefDate.Value
                        .Item("REFNO") = f.txtRefno.Text
                        .Item("AMOUNT") = f.txtWamt_AMT.Text
                        .Item("VAT") = f.txtVat_AMT.Text
                        Exit For
                    End With
                Next
            Else
                Dim row As DataRow = Nothing
                row = dtWeightDetail.NewRow
                row("KEYNO") = Val(gridView_OWN.CurrentRow.Cells("KEYNO").Value.ToString)
                row("CATNAME") = f.cmbWCategory_MAN.Text
                row("PCS") = Val(f.txtWPcs_NUM.Text)
                row("GRSWT") = Val(f.txtWGrsWt_WET.Text)
                row("NETWT") = Val(f.txtWNetWt_WET.Text)
                row("PUREWT") = Val(f.txtPureWt_WET.Text)
                row("CALCMODE") = f.cmbWCalcMode.Text
                row("RATE") = Val(f.txtWRate_AMT.Text)
                row("TYPE") = f.cmbWType.Text
                row("REFDATE") = f.dtpRefDate.Value
                row("REFNO") = f.txtRefno.Text
                row("VAT") = Val(f.txtVat_AMT.Text & "")
                row("AMOUNT") = Val(f.txtWamt_AMT.Text)
                dtWeightDetail.Rows.Add(row)
            End If
            dtWeightDetail.AcceptChanges()

            'wt details loading

            gridView_OWN.CurrentRow.Cells("VATPER").Value = Val(f.txtVat_per.Text)
            gridView_OWN.CurrentRow.Cells("VATAMT").Value = Val(f.txtVat_AMT.Text)

            strSql = "SELECT " & IIf(f.cmbWType.Text = "SALES", "STAXID", "PTAXID") & " FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & f.cmbWCategory_MAN.Text & "'"
            gridView_OWN.CurrentRow.Cells("VATCATID").Value = objGPack.GetSqlValue(strSql)

            If vouchType = VoucherType.Journal Then
                If gridView_OWN.CurrentRow.Cells("TYPE").Value.ToString.ToUpper = DrCap.ToUpper Then '"DR"

                    gridView_OWN.CurrentRow.Cells("DEBIT").Value = Format(Val(f.txtWamt_AMT.Text), "0.00")

                ElseIf gridView_OWN.CurrentRow.Cells("TYPE").Value.ToString.ToUpper = CrCap.ToUpper Then '"CR"

                    gridView_OWN.CurrentRow.Cells("CREDIT").Value = Format(Val(f.txtWamt_AMT.Text), "0.00")


                End If
            Else
                If gridView_OWN.CurrentRow.Cells("TYPE").Value.ToString.ToUpper = DrCap.ToUpper Then '"DR"
                    gridView_OWN.CurrentRow.Cells("DEBIT").Value = Format(Val(f.txtWamt_AMT.Text), "0.00")
                ElseIf gridView_OWN.CurrentRow.Cells("TYPE").Value.ToString.ToUpper = CrCap.ToUpper Then '"CR"
                    gridView_OWN.CurrentRow.Cells("CREDIT").Value = Format(Val(f.txtWamt_AMT.Text), "0.00")
                End If
            End If

        End If
    End Sub

    Private Sub frmAccountsEnt_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        txtNarration1.CharacterCasing = CharacterCasing.Normal
        txtNarration2.CharacterCasing = CharacterCasing.Normal
        txtGrid_OWN.CharacterCasing = CharacterCasing.Normal
        txtGrid_OWN.BringToFront()
        txtGrid_OWN.BackColor = grpGridView.BackgroundColor
        gridView_OWN.RowHeadersVisible = False
        gridView_OWN.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        gridView_OWN.BorderStyle = BorderStyle.None
        gridView_OWN.SelectionMode = DataGridViewSelectionMode.CellSelect
        gridView_OWN.BackgroundColor = Color.Lavender
        gridView_OWN.GridColor = Color.Lavender
        gridView_OWN.DefaultCellStyle.BackColor = Color.Lavender

        grpGridView.Controls.Add(txtGrid_OWN)
        grpGridView.Controls.Add(lstSearch)
        txtGrid_OWN.BringToFront()
        lstSearch.BringToFront()

        With gridView_OWN.ColumnHeadersDefaultCellStyle
            .BackColor = SystemColors.InactiveCaption
            .Font = New Font("VERDANA", 10, FontStyle.Bold)
        End With
        gridView_OWN.RowTemplate.Height = txtGrid_OWN.Height
        gridView_OWN.Font = Me.Font
        'cmbSearch_OWN.Sorted = True
        If Not editFlag Then dtpDate.Value = GetEntryDate(GetServerDate, , ACCENTRY_DATE)
        If Not editFlag Then btnNew_Click(Me, New EventArgs)
        If editFlag Then
            dtGridView = editDt
            dtGridView.Columns("KEYNO").AutoIncrement = True
            dtGridView.AcceptChanges()
            gridView_OWN.DataSource = dtGridView
            strSql = " SELECT "
            strSql += vbCrLf + "(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)CATNAME"
            strSql += vbCrLf + ",PCS,GRSWT,NETWT,PUREWT,CASE WHEN GRSNET = 'G' THEN 'GROSS' ELSE 'NETWT' END AS CALCMODE"
            strSql += vbCrLf + ",CASE WHEN STONEUNIT = 'C' THEN 'CARAT' ELSE 'GRAM' END AS UNIT"
            strSql += vbCrLf + ",RATE,WT_ENTORDER AS KEYNO,refno,Refdate,Amount"
            strSql += vbCrLf + ",CASE TRANTYPE WHEN 'SA' THEN 'SALES' WHEN 'ISS' THEN 'SMITH_ISSUE' WHEN 'IPU' THEN 'PURCHASE RETURN' ELSE 'SALES' END AS TYPE"
            strSql += vbCrLf + "FROM " & cnStockDb & ".." & IIf(Approval, "TISSUE", "ISSUE") & " I WHERE BATCHNO = '" & BatchNo & "'"
            strSql += vbCrLf + "UNION ALL"
            strSql += vbCrLf + "SELECT "
            strSql += vbCrLf + "(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)CATNAME"
            strSql += vbCrLf + ",PCS,GRSWT,NETWT,PUREWT,CASE WHEN GRSNET = 'G' THEN 'GROSS' ELSE 'NETWT' END AS CALCMODE"
            strSql += vbCrLf + ",CASE WHEN STONEUNIT = 'C' THEN 'CARAT' ELSE 'GRAM' END AS UNIT"
            strSql += vbCrLf + ",RATE,WT_ENTORDER AS KEYNO,refno,refdate,Amount"
            strSql += vbCrLf + ",CASE TRANTYPE WHEN 'RRE' THEN 'SMITH_RECEIPT' WHEN 'RPU' THEN 'SMITH_PURCHASE' ELSE 'PURCHASE' END AS TYPE"
            strSql += vbCrLf + "FROM " & cnStockDb & ".." & IIf(Approval, "TRECEIPT", "RECEIPT") & " I WHERE BATCHNO = '" & BatchNo & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtWeightDetail)
        End If
        strSql = "SELECT NARRATION FROM " & cnAdminDb & "..NARRATION WHERE MODULEID='F'"
        narration = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(narration)
        'BrighttechPack.GlobalMethods.FillCombo(chklstAcname, dtAccNames, "ACNAME", , "ALL")
        If narration.Rows.Count > 0 Then
            objGPack.FillCombo(strSql, cmbNarration, , False)
            Dgv1Search.DataSource = narration
            Dgv1Search.ColumnHeadersVisible = False
            Dgv1Search.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            Dgv1Search.Visible = False
            cmbNarration.Visible = False
        Else
            Dgv1Search.Visible = False
            cmbNarration.Visible = False
            Dgv1Search.Enabled = False
            cmbNarration.Enabled = False
        End If
        pnlContainer_OWN.BorderStyle = BorderStyle.Fixed3D
        'pnlContainer_OWN.Location = New Point((ScreenWid - pnlContainer_OWN.Width) / 2, ((ScreenHit - 128) - pnlContainer_OWN.Height) / 2)
        If dtpDate.Text = "" Then
            dtpDate.Value = GetEntryDate(GetServerDate, , ACCENTRY_DATE)
        End If
        DgvSearch.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DgvSearch.BackgroundColor = Color.Lavender

        Dim dtgroup As New DataTable
        strSql = " SELECT ACGRPCODE,ACGRPNAME FROM ( "
        strSql += vbCrLf + " select '0' ACGRPCODE,'ALL' ACGRPNAME, 0 RESULT  "
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " select ACGRPCODE,ACGRPNAME, 1 RESULT from " & cnAdminDb & "..ACGROUP "
        strSql += vbCrLf + " )X ORDER BY RESULT, ACGRPNAME"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtgroup)
        cmbAcgroup_Own.DataSource = Nothing
        cmbAcgroup_Own.DataSource = dtgroup
        cmbAcgroup_Own.ValueMember = "ACGRPCODE"
        cmbAcgroup_Own.DisplayMember = "ACGRPNAME"

    End Sub
    Private Sub cmbNarration_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbNarration.GotFocus
        txtNarration1.Visible = True
        txtNarration1.Location = New Point(cmbNarration.Location.X, cmbNarration.Location.Y)
        txtNarration1.Size = New Size(cmbNarration.Size.Width, cmbNarration.Size.Height)
        txtNarration1.BringToFront()
        txtNarration1.Select()
    End Sub

    Private Sub cmbNarration_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbNarration.LostFocus
        Dgv1Search.Visible = False
        txtNarration1.Visible = False
    End Sub
    Private Sub Dgv1Search_CellPainting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellPaintingEventArgs) Handles Dgv1Search.CellPainting
        If e.RowIndex >= 0 And e.ColumnIndex >= 0 Then

            e.Handled = True
            e.PaintBackground(e.CellBounds, True)
            Dim sw As String = txtNarration1.Text
            If Not String.IsNullOrEmpty(sw) Then
                'highlight search word

                Dim val As String = DirectCast(e.FormattedValue, String)

                Dim sindx As Integer = val.ToLower.IndexOf(sw.ToLower)
                If sindx >= 0 Then
                    'search word found

                    Dim sf As Font = New Font(e.CellStyle.Font, FontStyle.Bold)
                    Dim sbr As SolidBrush = New SolidBrush(Color.Red)

                    Dim br As SolidBrush
                    If e.State = DataGridViewElementStates.Selected Then
                        br = New SolidBrush(e.CellStyle.SelectionForeColor)
                    Else
                        br = New SolidBrush(e.CellStyle.ForeColor)
                    End If

                    Dim sBefore As String = val.Substring(0, sindx)
                    Dim sBeforeSize As SizeF = e.Graphics.MeasureString(sBefore, e.CellStyle.Font, e.CellBounds.Size)
                    Dim sWord As String = val.Substring(sindx, sw.Length)
                    Dim sWordSize As SizeF = e.Graphics.MeasureString(sWord, sf, e.CellBounds.Size)
                    Dim sAfter As String = val.Substring(sindx + sw.Length, val.Length - (sindx + sw.Length))

                    e.Graphics.DrawString(sBefore, e.CellStyle.Font, br, e.CellBounds)
                    e.Graphics.DrawString(sWord, sf, sbr, e.CellBounds.X + sBeforeSize.Width, e.CellBounds.Location.Y)
                    e.Graphics.DrawString(sAfter, e.CellStyle.Font, br, e.CellBounds.X + sBeforeSize.Width + sWordSize.Width, e.CellBounds.Location.Y)
                Else
                    'paint as usual
                    e.PaintContent(e.CellBounds)
                End If
            Else
                'paint as usual
                e.PaintContent(e.CellBounds)
            End If
        End If
    End Sub
    Private Sub Dgv1Search_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Dgv1Search.KeyDown
        If e.KeyCode = Keys.Up Then
            If Dgv1Search.CurrentRow Is Nothing Then Exit Sub
            If Dgv1Search.CurrentRow.Index = 0 Then
                txtNarration1.Select()
            End If
        ElseIf e.KeyCode = Keys.Enter Then
            If Dgv1Search.CurrentRow Is Nothing Then Exit Sub
            e.Handled = True
            cmbNarration.Text = Dgv1Search.CurrentRow.Cells("NARRATION").Value.ToString
            txtNarration1.Text = Dgv1Search.CurrentRow.Cells("NARRATION").Value.ToString
            Dgv1Search.Visible = False
            txtNarration1.Select()
        End If
    End Sub

    Private Sub txtNarration1_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNarration1.GotFocus
        If Dgv1Search.Rows.Count > 0 Then
            txtNarration1.Text = IIf(cmbNarration.Text <> "", cmbNarration.Text, txtNarration1.Text)
            Dgv1Search.Location = New Point(cmbNarration.Location.X, cmbNarration.Location.Y + cmbNarration.Height)
            Dgv1Search.Size = New Size(cmbNarration.Size.Width, 150)
            Dgv1Search.Columns(0).Width = cmbNarration.Size.Width
            Dgv1Search.BringToFront()
        End If
    End Sub

    Private Sub txtNarration1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtNarration1.KeyDown
        If e.KeyCode = Keys.Down Then
            If Dgv1Search.Visible Then
                If Dgv1Search.RowCount > 0 Then
                    Dgv1Search.CurrentCell = Dgv1Search.Rows(0).Cells(Dgv1Search.FirstDisplayedCell.ColumnIndex)
                    Dgv1Search.Select()
                End If
            Else
                'DownRow()
            End If
        ElseIf e.KeyCode = Keys.Up Then
            If Dgv1Search.Visible Then
            Else
                'UpperRow()
            End If
        ElseIf e.KeyCode = Keys.Delete Then
            'e.Handled = True
            'Exit Sub
            'cmbAcName.Text = ""
        ElseIf e.KeyCode = Keys.Enter Then
            'KeyEnter(e)
            Exit Sub
            'e.Handled = True
        End If
    End Sub

    Private Sub txtNarration1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtNarration1.KeyPress

        If e.KeyChar = Chr(Keys.Enter) Then
            cmbNarration.Text = txtNarration1.Text
            txtNarration1.Text = cmbNarration.Text
            txtNarration1.Visible = True
            Me.SelectNextControl(cmbNarration, True, True, True, True)
            gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("NARRATION1").Value = txtNarration1.Text
            SendKeys.Send("{TAB}")
            Dgv1Search.Visible = False
        ElseIf e.KeyChar = Chr(Keys.Escape) Then
            'cmbNarration.Text = txtNarration1.Text
            'txtNarration1.Text = cmbNarration.Text
            txtNarration1.Visible = True
            'Me.SelectNextControl(cmbNarration, True, True, True, True)
            gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("NARRATION1").Value = txtNarration1.Text
            SendKeys.Send("{TAB}")
            Dgv1Search.Visible = False
        End If
    End Sub


    Private Sub txtNarration1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNarration1.TextChanged
        If txtNarration1.Focused = False Then Exit Sub
        If txtNarration1.Text = "" Then
            Dgv1Search.Visible = False
            Exit Sub
        Else
            Dgv1Search.Visible = True
        End If
        If Dgv1Search.Rows.Count > 0 Then
            Dim sw As String = txtNarration1.Text
            Dim RowFilterStr As String
            RowFilterStr = "NARRATION LIKE '%" & sw & "%'"
            narration.DefaultView.RowFilter = RowFilterStr
        Else
            Dgv1Search.Visible = False
            Dgv1Search.Enabled = False
            cmbNarration.Enabled = False
        End If
    End Sub
    Private Function GetDefaultType() As String
        Dim type As String = Nothing
        If vouchType = VoucherType.Receipt Then
            type = CrCap ' "Cr"
        ElseIf vouchType = VoucherType.Payment Then
            type = DrCap '"Dr"
        ElseIf vouchType = VoucherType.Journal Then
            type = CrCap '"Cr"
        ElseIf vouchType = VoucherType.DebitNote Then
            type = DrCap '"Dr"
        ElseIf vouchType = VoucherType.CreditNote Then
            type = CrCap '"Cr"
        End If
        Return type
    End Function

    Private Sub SetDefaultType()
        If gridView_OWN.RowCount = 1 Then
            gridView_OWN.Rows(0).Cells("TYPE").Value = GetDefaultType()
        End If
    End Sub

    Private Sub gridView_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView_OWN.CellEnter
        Dim pt As Point = gridView_OWN.Location
        DgvSearch.Visible = False
        txtGrid_OWN.ReadOnly = False
        txtGrid_OWN.AutoCompleteSource = AutoCompleteSource.None
        'txtGrid_OWN.Clear()
        'If gridView_OWN.CurrentRow Is Nothing Then Exit Sub
        'If gridView_OWN.CurrentRow.Cells("GENBY").Value.ToString = "A" Then Exit Sub
        Select Case gridView_OWN.Columns(e.ColumnIndex).Name
            Case "TYPE"
                txtGrid_OWN.Size = New Size(gridView_OWN.Columns(e.ColumnIndex).Width, txtGrid_OWN.Height)
                txtGrid_OWN.Location = pt + gridView_OWN.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Location
                txtGrid_OWN.TextAlign = HorizontalAlignment.Left
                txtGrid_OWN.MaxLength = 2
                txtGrid_OWN.Text = gridView_OWN.CurrentCell.FormattedValue
                txtGrid_OWN.Visible = True
                lstSearch.Location = New Point(txtGrid_OWN.Location.X, txtGrid_OWN.Location.Y + txtGrid_OWN.Height + 1)
                lstSearch.Size = New Size(txtGrid_OWN.Width, lstSearch.Height)
                txtGrid_OWN.BringToFront()
                txtGrid_OWN.Focus()
            Case "DESCRIPTION"
                LoadAcc()
                'txtGrid_OWN.AutoCompleteMode = AutoCompleteMode.Append
                'txtGrid_OWN.AutoCompleteCustomSource = AutoCompleteAccName
                'txtGrid_OWN.AutoCompleteSource = AutoCompleteSource.CustomSource
                txtGrid_OWN.Size = New Size(gridView_OWN.Columns(e.ColumnIndex).Width, txtGrid_OWN.Height)
                txtGrid_OWN.Location = pt + gridView_OWN.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Location
                txtGrid_OWN.MaxLength = 55
                txtGrid_OWN.TextAlign = HorizontalAlignment.Left
                txtGrid_OWN.Text = gridView_OWN.CurrentCell.FormattedValue
                txtGrid_OWN.Visible = True
                txtGrid_OWN.BringToFront()

                DgvSearch.Location = New Point(txtGrid_OWN.Location.X, txtGrid_OWN.Location.Y + txtGrid_OWN.Height)
                DgvSearch.Size = New Size(txtGrid_OWN.Size.Width, 150)
                txtGrid_OWN.Focus()
            Case "BALANCE", "DEBIT", "CREDIT"
                txtGrid_OWN.Size = New Size(gridView_OWN.Columns(e.ColumnIndex).Width, txtGrid_OWN.Height)
                txtGrid_OWN.Location = pt + gridView_OWN.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Location
                txtGrid_OWN.MaxLength = 13
                txtGrid_OWN.TextAlign = HorizontalAlignment.Right
                txtGrid_OWN.Text = gridView_OWN.CurrentCell.FormattedValue
                txtGrid_OWN.Visible = True
                txtGrid_OWN.BringToFront()
                txtGrid_OWN.Focus()
        End Select
    End Sub

    Private Sub gridView_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView_OWN.CellLeave
        If gridView_OWN.CurrentRow Is Nothing Then Exit Sub
        If gridView_OWN.CurrentRow.Cells("GENBY").Value.ToString = "A" Then Exit Sub
        Select Case gridView_OWN.Columns(e.ColumnIndex).Name
            Case "TYPE", "BALANCE"
                gridView_OWN.CurrentCell.Value = txtGrid_OWN.Text
            Case "DESCRIPTION"
                gridView_OWN.CurrentCell.Value = txtGrid_OWN.Text
            Case "DEBIT", "CREDIT"
                gridView_OWN.CurrentCell.Value = IIf(Val(txtGrid_OWN.Text) <> 0, Val(txtGrid_OWN.Text), DBNull.Value)
        End Select
        txtGrid_OWN.Clear()
        txtGrid_OWN.Visible = False
        lstSearch.Visible = False
        DgvSearch.Visible = False
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView_OWN.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
        End If
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView_OWN.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            gridView_OWN.CurrentCell = gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells(gridView_OWN.CurrentCell.ColumnIndex)
        End If
    End Sub

    Private Function PreviousVisibleColIndex(ByVal colIndex As Integer) As Integer
        If colIndex = 0 Then
            Return colIndex
        End If
PrevCol: colIndex -= 1
        If gridView_OWN.Columns(colIndex).Visible = True Then
            Return colIndex
        Else
            GoTo PrevCol
        End If
    End Function

    Private Function NextVisibleColIndex(ByVal colIndex As Integer) As Integer
        Dim curIndex As Integer = colIndex
        If colIndex = gridView_OWN.ColumnCount - 1 Then
            Return colIndex
        End If
        If Not gridView_OWN.ColumnCount - 1 > colIndex + 1 Then
            Return colIndex
        End If
NextCol: colIndex += 1
        If gridView_OWN.Columns(colIndex).Visible = True Then
            Return colIndex
        ElseIf Not gridView_OWN.ColumnCount - 1 > colIndex Then
            Return curIndex
        Else
            GoTo NextCol
        End If
    End Function

    Private Function CheckType() As Boolean
        If gridView_OWN.Columns(gridView_OWN.CurrentCell.ColumnIndex).Name = "TYPE" Then
            If lstSearch.Items.Contains(txtGrid_OWN.Text) = False Then Return True
        End If
    End Function

    Private Function CheckDescription() As Boolean
        If gridView_OWN.Columns(gridView_OWN.CurrentCell.ColumnIndex).Name = "DESCRIPTION" Then
            If gridView_OWN.CurrentRow.Cells("TYPE").Value.ToString <> "" Then
                Dim ro() As DataRow = Nothing
                Dim RowFilteration As String = VouchFilteration
                If RowFilteration <> "" Then RowFilteration += " AND "
                RowFilteration += " ACNAME = '" & txtGrid_OWN.Text & "'"
                ro = dtAccNames.Select(RowFilteration)
                If ro IsNot Nothing Then
                    If ro.Length > 0 Then
                        Return True
                    End If
                End If
            End If
        End If
    End Function

    Private Function CheckDebit() As Boolean
        If gridView_OWN.Columns(gridView_OWN.CurrentCell.ColumnIndex).Name = "DEBIT" Then
            If Val(txtGrid_OWN.Text) = 0 Then Return True
        End If
    End Function

    Private Function CheckCredit() As Boolean
        If gridView_OWN.Columns(gridView_OWN.CurrentCell.ColumnIndex).Name = "CREDIT" Then
            If Val(txtGrid_OWN.Text) = 0 Then Return True
        End If
    End Function

    Private Sub LeftRow()
        With gridView_OWN
            'If CheckType() Then Exit Sub
            'If CheckDescription() Then Exit Sub
            'If CheckDebit() Then Exit Sub
            'If CheckCredit() Then Exit Sub
            If .CurrentCell Is Nothing Then Exit Sub
            .CurrentCell = .CurrentRow.Cells(PreviousVisibleColIndex(.CurrentCell.ColumnIndex))
            If .CurrentRow.Cells("TYPE").Value.ToString.ToUpper = DrCap.ToUpper Then
                .CurrentRow.Cells("CREDIT").Value = DBNull.Value
            Else
                .CurrentRow.Cells("DEBIT").Value = DBNull.Value
            End If
            txtGrid_OWN.SelectAll()
        End With
    End Sub

    Private Sub RightRow()
        With gridView_OWN
            'If CheckType() Then Exit Sub
            'If CheckDescription() Then Exit Sub
            'If CheckDebit() Then Exit Sub
            'If CheckCredit() Then Exit Sub
            If .CurrentCell Is Nothing Then Exit Sub
            If gridView_OWN.Columns(.CurrentCell.ColumnIndex).Name = "DESCRIPTION" Then
                Dim ro() As DataRow = Nothing
                Dim RowFilteration As String = ""
                RowFilteration = " ACNAME = '" & txtGrid_OWN.Text & "'"
                ro = dtAccNames.Select(RowFilteration)
                If Val(ro.Length) = 0 Then
                    .CurrentCell = .CurrentRow.Cells(.CurrentCell.ColumnIndex)
                    Exit Sub
                End If
            End If
            .CurrentCell = .CurrentRow.Cells(NextVisibleColIndex(.CurrentCell.ColumnIndex))
            If .CurrentRow.Cells("TYPE").Value.ToString.ToUpper = DrCap.ToUpper Then
                .CurrentRow.Cells("CREDIT").Value = DBNull.Value
            Else
                .CurrentRow.Cells("DEBIT").Value = DBNull.Value
            End If
            txtGrid_OWN.SelectAll()
        End With
    End Sub

    Private Sub UpperRow()
        With gridView_OWN
            'If CheckType() Then Exit Sub
            'If CheckDescription() Then Exit Sub
            'If CheckDebit() Then Exit Sub
            'If CheckCredit() Then Exit Sub
            If .CurrentCell Is Nothing Then Exit Sub
            If .CurrentRow.Index <> 0 Then
                .CurrentCell = .Rows(.CurrentRow.Index - 1).Cells(.CurrentCell.ColumnIndex)
                txtGrid_OWN.SelectAll()
            End If
            If .CurrentRow.Cells("TYPE").Value.ToString.ToUpper = DrCap.ToUpper Then
                .CurrentRow.Cells("CREDIT").Value = DBNull.Value
            Else
                .CurrentRow.Cells("DEBIT").Value = DBNull.Value
            End If
        End With
    End Sub

    Private Sub DownRow()
        With gridView_OWN
            'If CheckType() Then Exit Sub
            'If CheckDescription() Then Exit Sub
            'If CheckDebit() Then Exit Sub
            'If CheckCredit() Then Exit Sub
            If .CurrentRow Is Nothing Then Exit Sub
            If lstSearch.Visible Then
                searchSender = txtGrid_OWN
                lstSearch.Focus()
            Else
                If .CurrentRow.Index <> .RowCount - 1 Then
                    .CurrentCell = .Rows(.CurrentRow.Index + 1).Cells(.CurrentCell.ColumnIndex)
                    txtGrid_OWN.SelectAll()
                End If
                If .CurrentRow.Cells("TYPE").Value.ToString.ToUpper = DrCap.ToUpper Then
                    .CurrentRow.Cells("CREDIT").Value = DBNull.Value
                Else
                    .CurrentRow.Cells("DEBIT").Value = DBNull.Value
                End If
            End If
        End With
    End Sub

    Private Sub txtGrid_OWN_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtGrid_OWN.GotFocus
        txtGrid_OWN.BackColor = Color.LightGoldenrodYellow
    End Sub

    Private Sub txtGrid_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtGrid_OWN.KeyDown
        searchDia = False
        With gridView_OWN
            Select Case e.KeyCode
                Case Keys.Insert
                    'txtGrid_OWN.auto
                Case Keys.Down
                    If DgvSearch.Visible Then
                        If DgvSearch.RowCount > 0 Then
                            DgvSearch.CurrentCell = DgvSearch.Rows(0).Cells(DgvSearch.FirstDisplayedCell.ColumnIndex)
                            DgvSearch.Select()
                        End If
                    Else
                        DownRow()
                    End If
                Case Keys.Up
                    If DgvSearch.Visible Then
                    Else
                        UpperRow()
                    End If
                Case Keys.Left
                    LeftRow()
                Case Keys.Right
                    RightRow()
                Case Keys.Delete
                    If gridView_OWN.CurrentRow.Cells("GENBY").Value.ToString = "A" Then
                        e.Handled = True
                        Exit Sub
                    End If
                    txtGrid_OWN.Clear()
                    Dim tdsAmt As Decimal = Val(gridView_OWN.CurrentRow.Cells("TDSAMT").Value.ToString)
                    If gridView_OWN.RowCount > 1 Then
                        Dim rwIndex As Integer = gridView_OWN.CurrentRow.Index
                        If rwIndex <> 0 Then gridView_OWN.CurrentCell = gridView_OWN.Rows(gridView_OWN.CurrentRow.Index - 1).Cells("TYPE")
                        dtGridView.Rows.RemoveAt(rwIndex)
                        If tdsAmt <> 0 Then
                            rwIndex = gridView_OWN.CurrentRow.Index
                            If rwIndex <> 0 Then gridView_OWN.CurrentCell = gridView_OWN.Rows(gridView_OWN.CurrentRow.Index - 1).Cells("TYPE")
                            dtGridView.Rows.RemoveAt(rwIndex)
                        End If
                    Else
                        For cnt As Integer = 0 To gridView_OWN.ColumnCount - 1
                            gridView_OWN.Rows(0).Cells(cnt).Value = DBNull.Value
                        Next
                        SetDefaultType()
                        gridView_OWN.CurrentCell = gridView_OWN.Rows(0).Cells("TYPE")
                    End If
                    Dim srvtAmt As Decimal = Val(gridView_OWN.CurrentRow.Cells("SRVTAMT").Value.ToString)
                    If gridView_OWN.RowCount > 1 Then
                        Dim rwIndex As Integer = gridView_OWN.CurrentRow.Index
                        If rwIndex <> 0 Then gridView_OWN.CurrentCell = gridView_OWN.Rows(gridView_OWN.CurrentRow.Index - 1).Cells("TYPE")
                        dtGridView.Rows.RemoveAt(rwIndex)
                        If tdsAmt <> 0 Then
                            rwIndex = gridView_OWN.CurrentRow.Index
                            If rwIndex <> 0 Then gridView_OWN.CurrentCell = gridView_OWN.Rows(gridView_OWN.CurrentRow.Index - 1).Cells("TYPE")
                            dtGridView.Rows.RemoveAt(rwIndex)
                        End If
                    Else
                        For cnt As Integer = 0 To gridView_OWN.ColumnCount - 1
                            gridView_OWN.Rows(0).Cells(cnt).Value = DBNull.Value
                        Next
                        SetDefaultType()
                        gridView_OWN.CurrentCell = gridView_OWN.Rows(0).Cells("TYPE")
                    End If

                Case Keys.Enter
                    'KeyEnter(e)
                    Exit Sub
            End Select
        End With
        'e.Handled = True
        searchDia = True
    End Sub

    Private Sub KeyEnter(ByVal e As KeyPressEventArgs)
        With gridView_OWN
            If .CurrentCell Is Nothing Then Exit Sub
            Select Case .Columns(.CurrentCell.ColumnIndex).Name
                Case "TYPE"
                    If txtGrid_OWN.Text = "" And GetGridViewTotal() = 0 Then
                        Me.SelectNextControl(txtNarration1, True, True, True, True)
                        SendKeys.Send("{TAB}")
                        'btnSave.Select()
                        e.Handled = True
                        Exit Sub
                    End If
                    If .CurrentRow.Cells("TYPE").Value.ToString.ToUpper = DrCap.ToUpper Then
                        .CurrentRow.Cells("CREDIT").Value = DBNull.Value
                    Else
                        .CurrentRow.Cells("DEBIT").Value = DBNull.Value
                    End If
                    If Not lstSearch.Items.Contains(txtGrid_OWN.Text) Then Exit Sub
                    RightRow()
                Case "DESCRIPTION"
                    If Not lstSearch.Items.Contains(.CurrentRow.Cells("TYPE").Value.ToString) Then
                        .CurrentCell = .CurrentRow.Cells("TYPE")
                        txtGrid_OWN.SelectAll()
                        Exit Sub
                    End If
                    If CheckDescription() = False Then Exit Select
                    Dim bcostid As String = ""
                    bcostid = funBcostid()
                    Dim mPartycode As String = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME='" & txtGrid_OWN.Text.ToString() & "'")
                    If IsdispBalance Then
                        .CurrentRow.Cells("BALANCE").Value = GetPartyBalanceNew(mPartycode, txtGrid_OWN.Text.ToString())
                    Else
                        .CurrentRow.Cells("BALANCE").Value = ""
                    End If
                    Dim app_Achead As String = objGPack.GetSqlValue("SELECT ISNULL(ACTIVE,'') FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = '" & mPartycode & "'", , , Nothing)
                    If app_Achead = "" Then
                        MsgBox("Partycode not approve", MsgBoxStyle.Information)
                    End If
                    If Ischkbudget Then

                        budval = Budgetcheck(mPartycode, dtpDate.Value, IIf(.CurrentRow.Cells("TYPE").Value.ToString.ToUpper = DrCap.ToUpper, "D", "C"), , bcostid)
                        If budval <> "" And budval.Contains("Dr") Then lblbud.Text = "Budget :" + budval.ToString : lblbud.Visible = True : chkval = Val(budval.Substring(0, budval.Length - 3).ToString) : GoTo skip Else lblbud.Text = "" : lblbud.Visible = False
                        If budval <> "" And budval.Contains("Cr") Then lblbud.Text = "Budget :" + budval.ToString : lblbud.Visible = True : chkval = Val(budval.Substring(0, budval.Length - 3).ToString) Else lblbud.Text = "" : lblbud.Visible = False
                    End If
skip:
                    If gridView_OWN.CurrentRow.Cells("NARRATION1").Value.ToString <> "" Then
                        txtNarration1.Text = gridView_OWN.CurrentRow.Cells("NARRATION1").Value.ToString
                    End If
                    If gridView_OWN.CurrentRow.Cells("NARRATION2").Value.ToString <> "" Then
                        txtNarration2.Text = gridView_OWN.CurrentRow.Cells("NARRATION2").Value.ToString
                    End If
                    If gridView_OWN.CurrentRow.Cells("SCOSTCENTRE").Value.ToString <> "" Then
                        cmbSubCostCenter_MAN.Text = gridView_OWN.CurrentRow.Cells("SCOSTCENTRE").Value.ToString
                    End If
                    If ShowTds() Then
                        funLoadBankDetail(bcostid)
                        NextAtDescription()
                        Exit Sub
                    End If
                    If funLoadBankDetail(bcostid) = False Then
                        Exit Sub
                    End If
                    If GST Then
                        If (_Title = "DEBIT NOTE" Or _Title = "CREDIT NOTE") And Auto_GST_DNCN = "Y" Then
                            If DNCNGST_FLAG <> True Then
                                If objGPack.GetSqlValue("SELECT INVENTORY FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & txtGrid_OWN.Text & "'") <> "Y" Then
                                    ShowAmountDetailGST()
                                Else
                                    If DNCNGST_FLAG <> True Then
                                        ShowWeightDetailGST()
                                    End If
                                End If
                            End If
                        Else
                            ShowWeightDetailGST()
                        End If
                    Else
                        ShowWeightDetail()
                    End If
                    NextAtDescription()
                Case "DEBIT"
                    If CheckDebit() Then Exit Sub
                    objSledger.IsSubLedger = False
                    ShowSledger(Val(txtGrid_OWN.Text.ToString))
                    If objSledger.IsSubLedger Then
                        Dim dtv As DataTable
                        dtv = New DataTable
                        dtv = CType(objSledger.gridView_OWN.DataSource, DataTable).Copy
                        If Val(txtGrid_OWN.Text.ToString) <> Val(dtv.Compute("SUM(AMOUNT)", "AMOUNT IS NOT NULL").ToString) Then
                            txtGrid_OWN.Select()
                            Exit Sub
                        End If
                    End If
                    ShowOutStDt()
                    If ChkChqTranLimit(objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & gridView_OWN.CurrentRow.Cells("DESCRIPTION").Value.ToString & "' AND ACTYPE = 'B'"), Val(txtGrid_OWN.Text)) Then Exit Sub
                    ' CHECK BUDGET VALUE
                    If budval <> "" Then
                        If (Val(txtGrid_OWN.Text.ToString) > chkval) Then
                            MessageBox.Show("" & IIf(budval.Contains("Dr") = True, "Debit", "Credit") & " Limit is exceed.")
                            If Not OTPCHECK("BUDGETVALUE", cnCostId, userId) Then Exit Sub
                            'Exit Sub
                        End If
                    End If
                    'ShowTds()
                    If .CurrentRow.Cells("TYPE").Value.ToString.ToUpper = DrCap.ToUpper Then '"DR"
                        If SubCostid = True Then
                            cmbSubCostCenter_MAN.Select()
                        Else
                            txtNarration1.Select()
                        End If
                        'NextNewRow()
                    ElseIf .CurrentRow.Cells("TYPE").Value.ToString.ToUpper = CrCap.ToUpper Then '"CR"
                        Dim balAmt As Double = Math.Abs(GetGridViewTotal)
                        .CurrentRow.Cells("CREDIT").Value = IIf(balAmt <> 0, Format(balAmt, "0.00"), DBNull.Value)
                        .CurrentCell = .CurrentRow.Cells("CREDIT")
                    End If
                Case "CREDIT"
                    If CheckCredit() Then Exit Sub
                    objSledger.IsSubLedger = False
                    ShowSledger(Val(txtGrid_OWN.Text.ToString))
                    'objSledger.gridView_OWN.
                    If objSledger.IsSubLedger Then
                        Dim dtv As DataTable
                        dtv = New DataTable
                        dtv = CType(objSledger.gridView_OWN.DataSource, DataTable).Copy
                        If Val(txtGrid_OWN.Text.ToString) <> Val(dtv.Compute("SUM(AMOUNT)", "AMOUNT IS NOT NULL").ToString) Then
                            txtGrid_OWN.Select()
                            Exit Sub
                        End If
                    End If
                    ShowOutStDt()
                    If ChkChqTranLimit(objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & gridView_OWN.CurrentRow.Cells("DESCRIPTION").Value.ToString & "' AND ACTYPE = 'B'"), Val(txtGrid_OWN.Text)) Then Exit Sub
                    If budval <> "" Then
                        If (Val(txtGrid_OWN.Text.ToString) > chkval) Then
                            MessageBox.Show("" & IIf(budval.Contains("Dr") = True, "Debit", "Credit") & " Limit is exceed.")
                            If Not OTPCHECK("BUDGETVALUE", cnCostId, userId) Then Exit Sub
                            'Exit Sub
                        End If
                    End If
                    If SubCostid = True Then
                        cmbSubCostCenter_MAN.Select()
                    Else
                        txtNarration1.Select()
                    End If
            End Select
        End With
    End Sub
    Private Sub ShowInvno()
        If ONETIMEFLAGINVOICENOANDDATE = False Then
            objInvNo.oneTimeClear = ONETIMEFLAGINVOICENOANDDATE
            objInvNo.Text = ""
            objInvNo.ShowDialog()
        End If
    End Sub

    Private Function funBcostid() As String
        Dim bcostid As String = ""
        If cmbCostCenter_MAN.Text <> "" Then
            bcostid = objGPack.GetSqlValue("SELECT  COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCenter_MAN.Text & "'")
        Else
            bcostid = ""
        End If
        Return bcostid
    End Function

    Private Function funLoadBankDetail(ByVal bcostid As String) As Boolean
        If objGPack.GetSqlValue("SELECT ACTYPE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & txtGrid_OWN.Text & "'") = "B" Then
            If ACCENTRYACTYPE_LOCK <> "" Then
                Dim _Authorise As Boolean
                _Authorise = BrighttechPack.Methods.GetRights(_DtUserRights, mnuId & Me.Name, BrighttechPack.Methods.RightMode.Authorize)
                If _Authorise = True Then
                Else
                    MsgBox("Bank Entry Locked UnAuthorize", MsgBoxStyle.Information)
                    Return False
                    'Exit Sub
                End If
            End If
            objBankDetail.DefDate = dtpDate.Value
            If Me.vouchType = VoucherType.Payment Then
                objBankDetail.MANDATORY = True
            End If
            If ONETIMEFLAGINVOICENOANDDATE = False Then
                objBankDetail.ClearBankDetails()
                If gridView_OWN.CurrentRow.Cells("TYPE").Value.ToString.ToUpper = CrCap.ToUpper Then
                    Dim chqnumber As Integer = Nothing
                    Dim accode As String = ""
                    accode = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & txtGrid_OWN.Text.ToString & "'", "ACCODE", Nothing)
                    objBankDetail._accode = accode
                    chqnumber = Val(objGPack.GetSqlValue("SELECT TOP 1 CHQNUMBER FROM " & cnStockDb & "..CHEQUEBOOK WHERE " & IIf(bcostid <> "", "COSTID='" & bcostid & "' AND", "") & " BANKCODE='" & accode & "' AND ISNULL(CANCEL,'')='' AND CHQISSUEDATE IS NULL order by chqnumber", "CHQNUMBER", "0"))
                    objBankDetail.txtChqNo.Text = chqnumber
                Else
                    objBankDetail.txtChqNo.Text = gridView_OWN.CurrentRow.Cells("CHQNO").Value.ToString
                End If
                objBankDetail.CmbChqDetail_OWN.Text = gridView_OWN.CurrentRow.Cells("CHQDETAIL").Value.ToString
                If gridView_OWN.CurrentRow.Cells("CHQDATE").Value.ToString <> "" Then
                    objBankDetail.dtpBankDate.Value = gridView_OWN.CurrentRow.Cells("CHQDATE").Value
                End If
            End If
            If Edchqno <> Nothing Then objBankDetail.txtChqNo.Text = Edchqno
            If Edchqdate <> Nothing Then objBankDetail.dtpBankDate.Text = Format(Edchqdate, "dd/MM/yyyy")
            If Edchqdetail <> Nothing Then objBankDetail.CmbChqDetail_OWN.Text = Edchqdetail

            'e.Handled = True
            objBankDetail.txtChqNo.Select()
            If ONETIMEFLAGINVOICENOANDDATE = False Then
                objBankDetail.ShowDialog()
            End If
            gridView_OWN.CurrentRow.Cells("CHQNO").Value = objBankDetail.txtChqNo.Text
            gridView_OWN.CurrentRow.Cells("CHQDATE").Value = objBankDetail.dtpBankDate.Value.Date
            gridView_OWN.CurrentRow.Cells("CHQDETAIL").Value = objBankDetail.CmbChqDetail_OWN.Text
            Return True
        Else
            Return True
        End If
    End Function


    Private Function ChkChqTranLimit(ByVal BankCode As String, ByVal Amount As Double) As Boolean

        If BankCode <> "" Then
            Dim bcostid As String = Nothing
            If cmbCostCenter_MAN.Text <> "" Then
                bcostid = objGPack.GetSqlValue("SELECT  COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCenter_MAN.Text & "'")
            End If
            Dim TranLimit As Double = Val(objGPack.GetSqlValue("SELECT TRANLIMIT  FROM " & cnStockDb & "..CHEQUEBOOK WHERE " & IIf(bcostid <> Nothing, "COSTID='" & bcostid & "' AND", "") & " ISNULL(BANKCODE,'') = '" & BankCode & "' AND ISNULL(CHQNUMBER,'') = '" & objBankDetail.txtChqNo.Text & "'", "TRANLIMIT", "-1"))
            If TranLimit > -1 Then
                If TranLimit < Amount Then
                    MessageBox.Show("Cheque amount should be less or equal to " & TranLimit, "Message", MessageBoxButtons.OK)
                    Return True
                End If
            End If
        End If
        Return False
    End Function


    Private Function GetGridViewTotal() As Double
        Dim ftr As String = "DEBIT IS NOT NULL"
        Dim debit As Object = dtGridView.Compute("SUM(DEBIT)", ftr)
        Dim credit As Object = dtGridView.Compute("SUM(CREDIT)", "CREDIT IS NOT NULL")
        Return Val(debit.ToString) - Val(credit.ToString)
    End Function

    Private Sub NextNewRow()

        gridView_OWN.CurrentRow.Cells("NARRATION1").Value = txtNarration1.Text
        gridView_OWN.CurrentRow.Cells("NARRATION2").Value = txtNarration2.Text
        gridView_OWN.CurrentRow.Cells("SCOSTCENTRE").Value = cmbSubCostCenter_MAN.Text
        If gridView_OWN.CurrentRow.Index = gridView_OWN.RowCount - 1 Then
            AddNewRow()
            'dtGridView.Rows.Add()
        End If

        Dim lastInd As Integer = 0
CheckLastRow:
        For cnt As Integer = 0 To gridView_OWN.RowCount - 1
            If gridView_OWN.Rows(cnt).Cells("DESCRIPTION").Value.ToString = "" Then
                lastInd = cnt
            End If
        Next

        If gridView_OWN.Rows(lastInd).Cells("DESCRIPTION").Value.ToString <> "" Then
            AddNewRow()
            'dtGridView.Rows.Add()
            GoTo CheckLastRow
        End If

        'If gridView_OWN.CurrentRow.Index = gridView_OWN.RowCount - 1 Then
        '    dtGridView.Rows.Add()
        'ElseIf gridView_OWN.Rows(gridView_OWN.RowCount - 1).Cells("TYPE").Value.ToString <> "" Then
        '    dtGridView.Rows.Add()
        'End If

        gridView_OWN.CurrentCell = gridView_OWN.Rows(lastInd).Cells("TYPE")

        Dim balAmt As Double = GetGridViewTotal()
        If balAmt = 0 Then
            gridView_CellEnter(Me, New DataGridViewCellEventArgs(0, gridView_OWN.CurrentRow.Index))
            txtGrid_OWN.SelectAll()
            Exit Sub
        Else
            If Not balAmt > 0 Then
                dtGridView.Rows(lastInd).Item("TYPE") = DrCap '"Dr"
            Else
                dtGridView.Rows(lastInd).Item("TYPE") = CrCap ' "Cr"
            End If
        End If
        searchDia = False
        gridView_CellEnter(Me, New DataGridViewCellEventArgs(0, lastInd))
        txtGrid_OWN.SelectAll()
        searchDia = False
    End Sub

    Private Sub NextAtDescription()
        With gridView_OWN
            '.CurrentRow.Cells("NARRATION1").Value = txtNarration1.Text
            '.CurrentRow.Cells("NARRATION2").Value = txtNarration2.Text
            If .CurrentRow.Cells("TYPE").Value.ToString.ToUpper = DrCap.ToUpper Then
                Dim balAmt As Double = Math.Abs(GetGridViewTotal)
                If Val(.CurrentRow.Cells("DEBIT").Value.ToString) = 0 Then
                    If budval <> "" = True Then
                        balAmt = IIf(chkval > balAmt, balAmt, chkval)
                    End If
                    .CurrentRow.Cells("DEBIT").Value = IIf(balAmt <> 0, Format(balAmt, "0.00"), DBNull.Value)
                End If
                .CurrentCell = .CurrentRow.Cells("DEBIT")
            ElseIf .CurrentRow.Cells("TYPE").Value.ToString.ToUpper = CrCap.ToUpper Then '"CR"
                Dim balAmt As Double = Math.Abs(GetGridViewTotal)
                If Val(.CurrentRow.Cells("CREDIT").Value.ToString) = 0 Then
                    If budval <> "" = True Then
                        balAmt = IIf(chkval > balAmt, balAmt, chkval)
                    End If
                    .CurrentRow.Cells("CREDIT").Value = IIf(balAmt <> 0, Format(balAmt, "0.00"), DBNull.Value)
                End If
                .CurrentCell = .CurrentRow.Cells("CREDIT")
            End If
            gridView_OWN.Focus()
        End With
    End Sub

    Private Function GetPartyBalance(ByVal partyName As String) As String
        Dim retStr As String = Nothing
        strSql = " SELECT "
        strSql += vbCrLf + "ISNULL(SUM(DEBIT),0) - ISNULL(SUM(CREDIT),0) BALANCE"
        strSql += vbCrLf + "FROM"
        strSql += vbCrLf + "("
        strSql += vbCrLf + "SELECT DEBIT,CREDIT FROM  " & cnStockDb & "..OPENTRAILBALANCE"
        strSql += vbCrLf + "WHERE ACCODE = (SELECT ACCODE FROM  " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & partyName & "')"
        strSql += vbCrLf + "AND COMPANYID = '" & strCompanyId & "'"
        'strSql += vbcrlf + "AND COSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCenter_MAN.Text & "')"
        strSql += vbCrLf + "AND COSTID = (SELECT CASE WHEN (SELECT  COUNT(COSTID) FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCenter_MAN.Text & "' )< 1 THEN '' ELSE (SELECT  COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCenter_MAN.Text & "' )END) "
        strSql += vbCrLf + "UNION ALL"
        strSql += vbCrLf + "SELECT "
        strSql += vbCrLf + "ISNULL(SUM(CASE WHEN TRANMODE = 'D' THEN ISNULL(AMOUNT,0) ELSE 0 END),0) AS DEBIT"
        strSql += vbCrLf + ",ISNULL(SUM(CASE WHEN TRANMODE = 'C' THEN ISNULL(AMOUNT,0) ELSE 0 END),0) AS CREDIT"
        strSql += vbCrLf + "FROM  " & cnStockDb & "..ACCTRAN"
        strSql += vbCrLf + "WHERE TRANDATE <= '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' "
        strSql += vbCrLf + "AND ACCODE = (SELECT ACCODE FROM  " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & partyName & "')"
        strSql += vbCrLf + "AND COMPANYID = '" & strCompanyId & "'  AND ISNULL(CANCEL,'') = ''"
        'strSql += vbcrlf + "AND COSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCenter_MAN.Text & "')"
        strSql += vbCrLf + "AND COSTID = (SELECT CASE WHEN (SELECT  COUNT(COSTID) FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCenter_MAN.Text & "' )< 1 THEN '' ELSE (SELECT  COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCenter_MAN.Text & "' )END) "
        strSql += vbCrLf + "UNION ALL"
        strSql += vbCrLf + "SELECT "
        strSql += vbCrLf + "ISNULL(SUM(CASE WHEN TRANMODE = 'D' THEN ISNULL(AMOUNT,0) ELSE 0 END),0) AS DEBIT"
        strSql += vbCrLf + ",ISNULL(SUM(CASE WHEN TRANMODE = 'C' THEN ISNULL(AMOUNT,0) ELSE 0 END),0) AS CREDIT"
        strSql += vbCrLf + "FROM  " & cnStockDb & "..TACCTRAN"
        strSql += vbCrLf + "WHERE TRANDATE <= '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' "
        strSql += vbCrLf + "AND ACCODE = (SELECT ACCODE FROM  " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & partyName & "')"
        strSql += vbCrLf + "AND COMPANYID = '" & strCompanyId & "'  AND ISNULL(CANCEL,'') = ''"
        'strSql += vbcrlf + "AND COSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCenter_MAN.Text & "')"
        strSql += vbCrLf + "AND COSTID = (SELECT CASE WHEN (SELECT  COUNT(COSTID) FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCenter_MAN.Text & "' )< 1 THEN '' ELSE (SELECT  COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCenter_MAN.Text & "' )END) "
        strSql += vbCrLf + ")X"
        Dim bal As Double = Val(objGPack.GetSqlValue(strSql))

        Dim ftr As String = "DESCRIPTION = '" & partyName & "' AND DEBIT IS NOT NULL"
        Dim debit As Object = dtGridView.Compute("SUM(DEBIT)", "DESCRIPTION = '" & partyName & "' AND DEBIT IS NOT NULL AND KEYNO <> " & Val(gridView_OWN.CurrentRow.Cells("KEYNO").Value.ToString) & "")
        Dim credit As Object = dtGridView.Compute("SUM(CREDIT)", "DESCRIPTION = '" & partyName & "' AND CREDIT IS NOT NULL AND KEYNO <> " & Val(gridView_OWN.CurrentRow.Cells("KEYNO").Value.ToString) & "")

        bal += Val(debit.ToString) - Val(credit.ToString)

        If bal = 0 Then
            retStr = ""
        ElseIf bal > 0 Then
            retStr = Format(Math.Abs(bal), "0.00") & "  Dr"
        ElseIf bal < 0 Then
            retStr = Format(Math.Abs(bal), "0.00") & "  Cr"
        End If

        strSql = " SELECT "
        strSql += vbCrLf + "    ISNULL(SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1 * AMOUNT END),0) BALANCE"
        strSql += vbCrLf + "FROM  " & cnStockDb & "..TACCTRAN"
        strSql += vbCrLf + "WHERE TRANDATE <= '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' "
        strSql += vbCrLf + "AND ACCODE = (SELECT ACCODE FROM  " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & partyName & "')"
        strSql += vbCrLf + "AND COMPANYID = '" & strCompanyId & "'  AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + "AND COSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCenter_MAN.Text & "')"
        strSql += vbCrLf + "HAVING ISNULL(SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1 * AMOUNT END),0) <> 0"
        Dim dtPend As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtPend)
        If dtPend.Rows.Count > 0 Then
            lblPendAmt.Text = "Pending Amount : " & IIf(Val(dtPend.Rows(0).Item("BALANCE").ToString) > 0, Val(dtPend.Rows(0).Item("BALANCE").ToString).ToString + " Dr", Math.Abs(Val(dtPend.Rows(0).Item("BALANCE").ToString)).ToString + " Cr")
            lblPendAmt.Visible = True
        Else
            lblPendAmt.Visible = False
        End If

        Return retStr
    End Function

    Private Function GetPartyBalanceNew(ByVal Partycode As String, ByVal Partyname As String) As String
        Dim retStr As String = Nothing
        Dim costid As String
        If cmbCostCenter_MAN.Text <> "" Then
            costid = objGPack.GetSqlValue("SELECT  COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCenter_MAN.Text & "'")
        Else
            costid = ""
        End If
        strSql = " SELECT "
        strSql += vbCrLf + "ISNULL(SUM(DEBIT),0) - ISNULL(SUM(CREDIT),0) BALANCE"
        strSql += vbCrLf + "FROM"
        strSql += vbCrLf + "("
        strSql += vbCrLf + "SELECT DEBIT,CREDIT FROM  " & cnStockDb & "..OPENTRAILBALANCE"
        strSql += vbCrLf + "WHERE ACCODE = '" & Partycode & "'"
        strSql += vbCrLf + "AND COMPANYID = '" & strCompanyId & "'"
        If OS_ADJ_COSTCENTRE Then strSql += vbCrLf + "AND COSTID = '" & costid & "'"
        strSql += vbCrLf + "UNION ALL"
        strSql += vbCrLf + "SELECT "
        strSql += vbCrLf + "ISNULL(SUM(CASE WHEN TRANMODE = 'D' THEN ISNULL(AMOUNT,0) ELSE 0 END),0) AS DEBIT"
        strSql += vbCrLf + ",ISNULL(SUM(CASE WHEN TRANMODE = 'C' THEN ISNULL(AMOUNT,0) ELSE 0 END),0) AS CREDIT"
        strSql += vbCrLf + "FROM  " & cnStockDb & "..ACCTRAN"
        strSql += vbCrLf + "WHERE ACCODE = '" & Partycode & "' AND TRANDATE <= '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' "
        strSql += vbCrLf + "AND COMPANYID = '" & strCompanyId & "'  AND ISNULL(CANCEL,'') = ''"
        If OS_ADJ_COSTCENTRE Then strSql += vbCrLf + "AND isnull(COSTID,'') = '" & costid & "'"
        strSql += vbCrLf + "UNION ALL"
        strSql += vbCrLf + "SELECT "
        strSql += vbCrLf + "ISNULL(SUM(CASE WHEN TRANMODE = 'D' THEN ISNULL(AMOUNT,0) ELSE 0 END),0) AS DEBIT"
        strSql += vbCrLf + ",ISNULL(SUM(CASE WHEN TRANMODE = 'C' THEN ISNULL(AMOUNT,0) ELSE 0 END),0) AS CREDIT"
        strSql += vbCrLf + "FROM  " & cnStockDb & "..TACCTRAN"
        strSql += vbCrLf + "WHERE ACCODE = '" & Partycode & "' AND TRANDATE <= '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' "
        strSql += vbCrLf + "AND COMPANYID = '" & strCompanyId & "'  AND ISNULL(CANCEL,'') = ''"
        If OS_ADJ_COSTCENTRE Then strSql += vbCrLf + "AND COSTID = '" & costid & "'"
        strSql += vbCrLf + ")X"
        Dim bal As Double = Val(objGPack.GetSqlValue(strSql))

        Dim ftr As String = "DESCRIPTION = '" & Partyname & "' AND DEBIT IS NOT NULL"
        Dim debit As Object = dtGridView.Compute("SUM(DEBIT)", "DESCRIPTION = '" & Partyname & "' AND DEBIT IS NOT NULL AND KEYNO <> " & Val(gridView_OWN.CurrentRow.Cells("KEYNO").Value.ToString) & "")
        Dim credit As Object = dtGridView.Compute("SUM(CREDIT)", "DESCRIPTION = '" & Partyname & "' AND CREDIT IS NOT NULL AND KEYNO <> " & Val(gridView_OWN.CurrentRow.Cells("KEYNO").Value.ToString) & "")

        bal += Val(debit.ToString) - Val(credit.ToString)

        If bal = 0 Then
            retStr = ""
        ElseIf bal > 0 Then
            retStr = Format(Math.Abs(bal), "0.00") & "  Dr"
        ElseIf bal < 0 Then
            retStr = Format(Math.Abs(bal), "0.00") & "  Cr"
        End If

        strSql = " SELECT "
        strSql += vbCrLf + "    ISNULL(SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1 * AMOUNT END),0) BALANCE"
        strSql += vbCrLf + "FROM  " & cnStockDb & "..TACCTRAN"
        strSql += vbCrLf + "WHERE ACCODE = '" & Partycode & "' AND TRANDATE <= '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' "
        strSql += vbCrLf + "AND COMPANYID = '" & strCompanyId & "'  AND ISNULL(CANCEL,'') = ''"
        If OS_ADJ_COSTCENTRE Then strSql += vbCrLf + "AND COSTID = '" & costid & "'"
        strSql += vbCrLf + "HAVING ISNULL(SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1 * AMOUNT END),0) <> 0"
        Dim dtPend As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtPend)
        If dtPend.Rows.Count > 0 Then
            lblPendAmt.Text = "Pending Amount : " & IIf(Val(dtPend.Rows(0).Item("BALANCE").ToString) > 0, Val(dtPend.Rows(0).Item("BALANCE").ToString).ToString + " Dr", Math.Abs(Val(dtPend.Rows(0).Item("BALANCE").ToString)).ToString + " Cr")
            lblPendAmt.Visible = True
        Else
            lblPendAmt.Visible = False
        End If

        Return retStr
    End Function


    Private Sub txtGrid_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtGrid_OWN.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            KeyEnter(e)
        Else
            If gridView_OWN.CurrentRow.Cells("GENBY").Value.ToString = "A" Then
                e.Handled = True
                Exit Sub
            End If
            Select Case gridView_OWN.Columns(gridView_OWN.CurrentCell.ColumnIndex).Name
                Case "DESCRIPTION"
                    If Not lstSearch.Items.Contains(gridView_OWN.CurrentRow.Cells("TYPE").Value.ToString) Then
                        e.Handled = True
                    End If
                    If txtGrid_OWN.Text = "" Then
                        e.KeyChar = UCase(e.KeyChar)
                    Else
                        If txtGrid_OWN.SelectedText <> "" Then
                            If txtGrid_OWN.Text.Replace(txtGrid_OWN.SelectedText, "").EndsWith(" ") Then
                                e.KeyChar = UCase(e.KeyChar)
                            End If
                        End If
                    End If

                Case "DEBIT"
                    If vouchType = VoucherType.Journal Or vouchType = VoucherType.Payment Then
                        If objGPack.GetSqlValue("SELECT ISNULL(TDSFLAG,'') FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & gridView_OWN.CurrentRow.Cells("DESCRIPTION").Value.ToString & "'").ToUpper = "Y" Then
                            e.Handled = True
                            Exit Sub
                        End If
                    End If
                    If UCase(gridView_OWN.CurrentRow.Cells("TYPE").Value.ToString) <> DrCap.ToUpper Then
                        txtGrid_OWN.Clear()
                        e.Handled = True
                        Exit Sub
                    End If
                    Select Case e.KeyChar
                        Case "0" To "9", Chr(Keys.Back), "."
                        Case Else
                            e.Handled = True
                    End Select
                Case "CREDIT"
                    If vouchType = VoucherType.Journal Or vouchType = VoucherType.Payment Then
                        If objGPack.GetSqlValue("SELECT ISNULL(TDSFLAG,'') FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & gridView_OWN.CurrentRow.Cells("DESCRIPTION").Value.ToString & "'").ToUpper = "Y" Then
                            e.Handled = True
                            Exit Sub
                        End If
                    End If
                    If UCase(gridView_OWN.CurrentRow.Cells("TYPE").Value.ToString) <> CrCap.ToUpper Then '"CR"
                        txtGrid_OWN.Clear()
                        e.Handled = True
                    End If
                    Select Case e.KeyChar
                        Case "0" To "9", Chr(Keys.Back), "."
                        Case Else
                            e.Handled = True
                    End Select
                Case "BALANCE"
                    e.Handled = True
            End Select
        End If
    End Sub

    Private Function ShowTds() As Boolean
        If vouchType <> VoucherType.Journal And vouchType <> VoucherType.Payment Then Exit Function
LoadTds:
        If objGPack.GetSqlValue("SELECT ISNULL(TDSFLAG,'') FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & txtGrid_OWN.Text & "'").ToUpper = "Y" Then
            objTds.Accode = objGPack.GetSqlValue("SELECT ISNULL(ACCODE,'') FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & txtGrid_OWN.Text & "'")
            objTds.txtActualAmount_AMT.Text = IIf(Val(gridView_OWN.CurrentRow.Cells("DEBIT").Value.ToString) <> 0, Val(gridView_OWN.CurrentRow.Cells("DEBIT").Value.ToString), Val(gridView_OWN.CurrentRow.Cells("CREDIT").Value.ToString))
            objTds.Accode = objGPack.GetSqlValue("SELECT ISNULL(ACCODE,'') FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & txtGrid_OWN.Text & "'")
            Dim tdsAmt As Double = Val(gridView_OWN.CurrentRow.Cells("TDSAMT").Value.ToString)
            Dim srvtAmt As Double = Val(gridView_OWN.CurrentRow.Cells("SRVTAMT").Value.ToString)
            If vouchType = VoucherType.Journal Then
                objTds.txtActualAmount_AMT.Text = objTds.Amount + srvtAmt + tdsAmt
            End If
            Dim tdsPer As Double = Val(gridView_OWN.CurrentRow.Cells("TDSPER").Value.ToString)
            objTds.txtTdsPer_PER.Text = IIf(tdsPer <> 0, Format(tdsPer, "0.00"), Nothing)
            If tdsAmt <> 0 Then
                objTds.EditFlag = True
            Else
                objTds.EditFlag = False
            End If
            objTds.txtServTax_Amt.Text = IIf(srvtAmt <> 0, Format(srvtAmt, "0.00"), Nothing)
            If SRVTAX = True Then
                objTds.txtServTax_PER.Text = IIf(SRVTPER <> 0, Format(SRVTPER, "0.00"), Nothing)
            End If
            objTds.SRVTCODE = SrvTaxCode
            objTds.txtTdsAmt_AMT.Text = IIf(tdsAmt <> 0, Format(tdsAmt, "0.00"), Nothing)
            Dim tdsCategory As Integer = Val(gridView_OWN.CurrentRow.Cells("TDSCATID").Value.ToString)
            If tdsCategory <> 0 Then
                objTds.cmbTdsCategory_OWN.SelectedValue = tdsCategory.ToString
            Else
                'Dim TtdsCategory As String = objGPack.GetSqlValue("SELECT TDSCATNAME FROM " & cnAdminDb & "..TDSCATEGORY WHERE TDSCATID IN(SELECT ISNULL(TDSCATID,0) FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & txtGrid_OWN.Text & "')", , , )
                Dim TtdsCategory As Integer = objGPack.GetSqlValue("SELECT ISNULL(TDSCATID,0) FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & txtGrid_OWN.Text & "'", , , )
                If TtdsCategory <> 0 Then
                    objTds.cmbTdsCategory_OWN.SelectedValue = TtdsCategory.ToString
                Else
                    objTds.cmbTdsCategory_OWN.SelectedText = ""
                End If
            End If
            objTds.txtActualAmount_AMT.Select()
            objTds.ShowDialog()
            If SRVTAX Then
                gridView_OWN.CurrentRow.Cells("SRVTPER").Value = Val(objTds.txtServTax_PER.Text)
                gridView_OWN.CurrentRow.Cells("SRVTAMT").Value = Val(objTds.txtServTax_Amt.Text)
            End If

            gridView_OWN.CurrentRow.Cells("TDSPER").Value = Val(objTds.txtTdsPer_PER.Text)
            gridView_OWN.CurrentRow.Cells("TDSAMT").Value = Val(objTds.txtTdsAmt_AMT.Text)
            gridView_OWN.CurrentRow.Cells("TDSCATID").Value = Val(objTds.cmbTdsCategory_OWN.SelectedValue.ToString)
            If vouchType = VoucherType.Journal Then
                If gridView_OWN.CurrentRow.Cells("TYPE").Value.ToString.ToUpper = DrCap.ToUpper Then '"DR"
                    If budval <> "" Then
                        If (Val(objTds.txtNetAmount_AMT.Text) > chkval) Then
                            MessageBox.Show("" & IIf(budval.Contains("Dr") = True, "Debit", "Credit") & " Limit is exceed.")
                            GoTo LoadTds
                            Exit Function
                        Else
                            gridView_OWN.CurrentRow.Cells("DEBIT").Value = Format(Val(objTds.txtNetAmount_AMT.Text), "0.00")
                        End If
                    Else
                        gridView_OWN.CurrentRow.Cells("DEBIT").Value = Format(Val(objTds.txtNetAmount_AMT.Text), "0.00")
                    End If
                ElseIf gridView_OWN.CurrentRow.Cells("TYPE").Value.ToString.ToUpper = CrCap.ToUpper Then '"CR"
                    If budval <> "" Then
                        If (Val(objTds.txtNetAmount_AMT.Text) > chkval) Then
                            MessageBox.Show("" & IIf(budval.Contains("Dr") = True, "Debit", "Credit") & " Limit is exceed.")
                            GoTo LoadTds
                            Exit Function
                        Else
                            gridView_OWN.CurrentRow.Cells("CREDIT").Value = Format(Val(objTds.txtNetAmount_AMT.Text), "0.00")
                        End If
                    Else
                        gridView_OWN.CurrentRow.Cells("CREDIT").Value = Format(Val(objTds.txtNetAmount_AMT.Text), "0.00")
                    End If

                End If
            Else
                If gridView_OWN.CurrentRow.Cells("TYPE").Value.ToString.ToUpper = DrCap.ToUpper Then '"DR"
                    gridView_OWN.CurrentRow.Cells("DEBIT").Value = Format(Val(objTds.txtActualAmount_AMT.Text), "0.00")
                ElseIf gridView_OWN.CurrentRow.Cells("TYPE").Value.ToString.ToUpper = CrCap.ToUpper Then '"CR"
                    gridView_OWN.CurrentRow.Cells("CREDIT").Value = Format(Val(objTds.txtActualAmount_AMT.Text), "0.00")
                End If
            End If
            If SubCostid = True Then
                cmbSubCostCenter_MAN.Focus()
            Else
                txtNarration1.Focus()
            End If

            Return True
        End If
    End Function

    Private Sub ShowOutStDt()
        If objGPack.GetSqlValue("SELECT OUTSTANDING FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & gridView_OWN.CurrentRow.Cells("DESCRIPTION").Value.ToString & "'") = "Y" Then
            objOutStDt.adjAmt = Val(txtGrid_OWN.Text)
            If Not dsOutStDtCol.Tables.Contains(gridView_OWN.CurrentRow.Cells("DESCRIPTION").Value.ToString) Then
                Select Case UCase(gridView_OWN.CurrentRow.Cells("TYPE").Value.ToString)
                    Case CrCap.ToUpper  '"CR"
                        'Calno 180114
                        'strSql = " SELECT CASE WHEN SUBSTRING(RUNNO,1,1) = 'G' THEN RUNNO ELSE SUBSTRING(RUNNO,6,20) END RUNNO"
                        strSql = " SELECT CASE WHEN SUBSTRING(RUNNO,1,1) = 'G' THEN '" & GetCostId(cnCostId) & GetCompanyId(strCompanyId) & "'+RUNNO ELSE RUNNO END RUNNO"
                        strSql += vbCrLf + ",(SELECT TOP 1 TRANDATE FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO = O.RUNNO ORDER BY TRANDATE)AS TRANDATE"
                        strSql += vbCrLf + ",TRANTYPE"
                        strSql += vbCrLf + ",SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE 0 END) AS PAYMENT"
                        strSql += vbCrLf + ",SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE 0 END) AS RECEIPT"
                        strSql += vbCrLf + ",SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE -1*AMOUNT END) AS BALANCE"
                        strSql += vbCrLf + ",CONVERT(NUMERIC(15,2),NULL) AS ADJUST"
                        strSql += vbCrLf + "FROM ("
                        strSql += vbCrLf + "SELECT RUNNO,AMOUNT,TRANTYPE,RECPAY"
                        strSql += vbCrLf + "FROM " & cnAdminDb & "..OUTSTANDING AS O"
                        strSql += vbCrLf + "WHERE ACCODE = (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & gridView_OWN.CurrentRow.Cells("DESCRIPTION").Value.ToString & "')"
                        strSql += vbCrLf + "AND COMPANYID = '" & strCompanyId & "'"
                        strSql += vbCrLf + "AND BATCHNO <> '" & BatchNo & "'"
                        strSql += vbCrLf + "AND ISNULL(CANCEL,'') = ''"
                        If OS_ADJ_COSTCENTRE Then
                            strSql += vbCrLf + "AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCenter_MAN.Text & "'),'')"
                        End If
                        If Approval Then
                            strSql += vbCrLf + "UNION ALL"
                            'Calno 180114
                            'strSql += vbcrlf + "SELECT CASE WHEN SUBSTRING(RUNNO,1,1) = 'G' THEN RUNNO ELSE SUBSTRING(RUNNO,6,20) END RUNNO,AMOUNT,TRANTYPE,RECPAY"
                            strSql += vbCrLf + "SELECT CASE WHEN SUBSTRING(RUNNO,1,1) = 'G' THEN  '" & GetCostId(cnCostId) & GetCompanyId(strCompanyId) & "'+ RUNNO ELSE RUNNO END RUNNO,AMOUNT,TRANTYPE,RECPAY"
                            strSql += vbCrLf + "FROM " & cnStockDb & "..TOUTSTANDING AS O"
                            strSql += vbCrLf + "WHERE ACCODE = (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & gridView_OWN.CurrentRow.Cells("DESCRIPTION").Value.ToString & "')"
                            strSql += vbCrLf + "AND COMPANYID = '" & strCompanyId & "'"
                            strSql += vbCrLf + "AND BATCHNO <> '" & BatchNo & "'"
                            strSql += vbCrLf + "AND ISNULL(CANCEL,'') = ''"
                            If OS_ADJ_COSTCENTRE Then
                                strSql += vbCrLf + "AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCenter_MAN.Text & "'),'')"
                            End If

                        End If
                        strSql += vbCrLf + ")O"
                        strSql += vbCrLf + "GROUP BY RUNNO,TRANTYPE"
                        strSql += vbCrLf + "HAVING SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE -1*AMOUNT END) > 0"
                        strSql += vbCrLf + "ORDER BY TRANDATE"
                    Case DrCap.ToUpper
                        'Calno 180114
                        'strSql = " SELECT CASE WHEN SUBSTRING(RUNNO,1,1) = 'G' THEN RUNNO ELSE SUBSTRING(RUNNO,6,20) END RUNNO"
                        strSql = " SELECT CASE WHEN SUBSTRING(RUNNO,1,1) = 'G' THEN '" & GetCostId(cnCostId) & GetCompanyId(strCompanyId) & "'+RUNNO ELSE RUNNO END RUNNO"
                        strSql += vbCrLf + ",(SELECT TOP 1 TRANDATE FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO = O.RUNNO ORDER BY TRANDATE)AS TRANDATE"
                        strSql += vbCrLf + ",TRANTYPE"
                        strSql += vbCrLf + ",SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE 0 END) AS RECEIPT"
                        strSql += vbCrLf + ",SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE 0 END) AS PAYMENT"
                        strSql += vbCrLf + ",SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE -1*AMOUNT END) AS BALANCE"
                        strSql += vbCrLf + ",CONVERT(NUMERIC(15,2),NULL) AS ADJUST"
                        strSql += vbCrLf + "FROM ("
                        strSql += vbCrLf + "SELECT RUNNO,TRANTYPE,AMOUNT,RECPAY"
                        strSql += vbCrLf + "FROM " & cnAdminDb & "..OUTSTANDING AS O"
                        strSql += vbCrLf + "WHERE ACCODE = (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & gridView_OWN.CurrentRow.Cells("DESCRIPTION").Value.ToString & "')"
                        strSql += vbCrLf + "AND COMPANYID = '" & strCompanyId & "'"
                        strSql += vbCrLf + "AND BATCHNO <> '" & BatchNo & "'"
                        strSql += vbCrLf + "AND ISNULL(CANCEL,'') = ''"
                        If OS_ADJ_COSTCENTRE Then
                            strSql += vbCrLf + "AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCenter_MAN.Text & "'),'')"
                        End If
                        If Approval Then
                            strSql += vbCrLf + "UNION ALL"
                            'Calno 180114
                            'strSql += vbcrlf + "SELECT CASE WHEN SUBSTRING(RUNNO,1,1) = 'G' THEN RUNNO ELSE SUBSTRING(RUNNO,6,20) END RUNNO,TRANTYPE,AMOUNT,RECPAY"
                            strSql += vbCrLf + "SELECT CASE WHEN SUBSTRING(RUNNO,1,1) = 'G' THEN '" & GetCostId(cnCostId) & GetCompanyId(strCompanyId) & "'+RUNNO ELSE RUNNO END RUNNO,TRANTYPE,AMOUNT,RECPAY"
                            strSql += vbCrLf + "FROM " & cnStockDb & "..TOUTSTANDING AS O"
                            strSql += vbCrLf + "WHERE ACCODE = (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & gridView_OWN.CurrentRow.Cells("DESCRIPTION").Value.ToString & "')"
                            strSql += vbCrLf + "AND COMPANYID = '" & strCompanyId & "'"
                            strSql += vbCrLf + "AND BATCHNO <> '" & BatchNo & "'"
                            strSql += vbCrLf + "AND ISNULL(CANCEL,'') = ''"
                            If OS_ADJ_COSTCENTRE Then
                                strSql += vbCrLf + "AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCenter_MAN.Text & "'),'')"
                            End If
                        End If
                        strSql += vbCrLf + ")O"
                        strSql += vbCrLf + "GROUP BY RUNNO,TRANTYPE"
                        strSql += vbCrLf + "HAVING SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE -1*AMOUNT END) > 0"
                        strSql += vbCrLf + "ORDER BY TRANDATE"
                End Select
                Dim dtGrid As New DataTable(gridView_OWN.CurrentRow.Cells("DESCRIPTION").Value.ToString)
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtGrid)
                If dtGrid.Rows.Count > 0 Then
                    dsOutStDtCol.Tables.Add(dtGrid)
                    objOutStDt.LoadGridOutStDt(dsOutStDtCol.Tables(gridView_OWN.CurrentRow.Cells("DESCRIPTION").Value.ToString))
                    objOutStDt.AutoAdjust()
                    objOutStDt.ShowDialog()
                Else
                    ShowInvno()
                End If
            Else
                objOutStDt.LoadGridOutStDt(dsOutStDtCol.Tables(gridView_OWN.CurrentRow.Cells("DESCRIPTION").Value.ToString))
                objOutStDt.AutoAdjust()
                objOutStDt.ShowDialog()
            End If
        End If
    End Sub
    Private Sub ShowSledger(ByVal TotAmount As Decimal)
        Dim mCostid As String
        If cmbCostCenter_MAN.Text <> "" Then
            mCostid = objGPack.GetSqlValue("SELECT  COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCenter_MAN.Text & "'")
        Else
            mCostid = ""
        End If
con1:
        strSql = "SELECT ACNAME,0.00 AS AMOUNT FROM " & cnAdminDb & "..ACHEAD WHERE MACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & gridView_OWN.CurrentRow.Cells("DESCRIPTION").Value.ToString & "')"
        If Not dsSl.Tables.Contains(gridView_OWN.CurrentRow.Cells("DESCRIPTION").Value.ToString) Then
            Dim dtGrid As New DataTable(gridView_OWN.CurrentRow.Cells("DESCRIPTION").Value.ToString)
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtGrid)
            objSledger.Ischkbudget = Ischkbudget
            objSledger.Entrydate = dtpDate.Value
            objSledger.Bcostid = mCostid

            objSledger.Entrytype = IIf(gridView_OWN.CurrentRow.Cells("TYPE").Value.ToString.ToUpper = DrCap.ToUpper, "D", "C")
            If dtGrid.Rows.Count > 0 Then
                dsSl.Tables.Add(dtGrid)
                objSledger.LoadGridOutStDt(dsSl.Tables(gridView_OWN.CurrentRow.Cells("DESCRIPTION").Value.ToString))
                'objSledger.AutoAdjust()
                objSledger.Totalamt = TotAmount
continueee1:
                objSledger.ShowDialog()
                'If Not dsSl.Tables.Contains(dtGrid.ToString) Then Exit Sub
                dsSl.Tables.Remove(dtGrid)
                Dim dtsubledger As New DataTable(gridView_OWN.CurrentRow.Cells("DESCRIPTION").Value.ToString)
                dtsubledger = objSledger.gridView_OWN.DataSource
                dsSl.Tables.Add(dtsubledger)
                'If Val(dsSl.Tables(gridView_OWN.CurrentRow.Cells("DESCRIPTION").Value.ToString).Compute("SUM(AMOUNT)", "AMOUNT IS NOT NULL").ToString) <> TotAmount Then MsgBox("Your Subledger Value not tallied", MsgBoxStyle.Critical) : GoTo con1
                If Val(dsSl.Tables(gridView_OWN.CurrentRow.Cells("DESCRIPTION").Value.ToString).Compute("SUM(AMOUNT)", "AMOUNT IS NOT NULL").ToString) <> TotAmount Then MsgBox("Your Subledger Value not tallied", MsgBoxStyle.Critical) : txtGrid_OWN.Focus()
            End If
        Else
            Dim dtGrid As New DataTable(gridView_OWN.CurrentRow.Cells("DESCRIPTION").Value.ToString)
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtGrid)
            objSledger.Totalamt = TotAmount
            objSledger.LoadGridOutStDt(dtGrid, dsSl.Tables(gridView_OWN.CurrentRow.Cells("DESCRIPTION").Value.ToString))
continueee2:
            objSledger.ShowDialog()
            dsSl.Tables.Remove(dsSl.Tables(gridView_OWN.CurrentRow.Cells("DESCRIPTION").Value.ToString))
            Dim dtsubledger As New DataTable(gridView_OWN.CurrentRow.Cells("DESCRIPTION").Value.ToString)
            dtsubledger = objSledger.gridView_OWN.DataSource
            dsSl.Tables.Add(dtsubledger)
            'If Val(dsSl.Tables(gridView_OWN.CurrentRow.Cells("DESCRIPTION").Value.ToString).Compute("SUM(AMOUNT)", "AMOUNT IS NOT NULL").ToString) <> TotAmount Then MsgBox("Your Subledger Value not tallied", MsgBoxStyle.Critical) : GoTo continueee2
            If Val(dsSl.Tables(gridView_OWN.CurrentRow.Cells("DESCRIPTION").Value.ToString).Compute("SUM(AMOUNT)", "AMOUNT IS NOT NULL").ToString) <> TotAmount Then MsgBox("Your Subledger Value not tallied", MsgBoxStyle.Critical) : txtGrid_OWN.Focus()
        End If

    End Sub


    Private Sub InsertIntoOustanding _
  (ByVal tranNo As Integer, ByVal batchno As String,
  ByVal tType As String, ByVal RunNo As String, ByVal Amount As Double,
  ByVal RecPay As String,
  ByVal Paymode As String,
  Optional ByVal GrsWt As Double = 0,
  Optional ByVal NetWt As Double = 0,
  Optional ByVal CatCode As String = Nothing,
  Optional ByVal Rate As Double = Nothing,
  Optional ByVal Value As Double = Nothing,
  Optional ByVal refNo As String = Nothing,
  Optional ByVal refDate As String = Nothing,
  Optional ByVal purity As Double = Nothing,
  Optional ByVal proId As Integer = Nothing,
  Optional ByVal dueDate As String = Nothing,
  Optional ByVal Remark1 As String = Nothing,
  Optional ByVal Remark2 As String = Nothing,
  Optional ByVal accode As String = Nothing
      )
        If Amount = 0 And GrsWt = 0 Then Exit Sub

        BL_Acc.InsertOutstandingDetails(tran, IIf(Approval, "TOUTSTANDING", "OUTSTANDING"), GetNewSno(IIf(Approval, TranSnoType.TOUTSTANDINGCODE, TranSnoType.OUTSTANDINGCODE),
            tran, "GET_ADMINSNO_TRAN"), tranNo, dtpDate.Value.ToString("yyyy-MM-dd"), batchno, tType,
            RunNo, Math.Abs(Amount), RecPay, Paymode, Math.Abs(GrsWt), Math.Abs(NetWt), CatCode, Rate, Value, refNo, refDate,
            purity, proId, dueDate, Remark1, Remark2, accode, "A", , , , , , , CostId, "A")
        Exit Sub

        strSql = " INSERT INTO " & cnAdminDb & ".." & IIf(Approval, "TOUTSTANDING", "OUTSTANDING")
        strSql += vbCrLf + "("
        strSql += vbCrLf + "SNO,TRANNO,TRANDATE,TRANTYPE,RUNNO"
        strSql += vbCrLf + ",AMOUNT,GRSWT,NETWT,RECPAY"
        strSql += vbCrLf + ",REFNO,REFDATE,EMPID"
        strSql += vbCrLf + ",TRANSTATUS,PURITY,CATCODE,BATCHNO"
        strSql += vbCrLf + ",USERID,UPDATED,UPTIME,SYSTEMID"
        strSql += vbCrLf + ",RATE,VALUE,CASHID,REMARK1,REMARK2,ACCODE,CTRANCODE,DUEDATE,APPVER,COSTID,COMPANYID,PAYMODE,FROMFLAG)"
        strSql += vbCrLf + "VALUES"
        strSql += vbCrLf + "("
        strSql += vbCrLf + "'" & GetNewSno(IIf(Approval, TranSnoType.TOUTSTANDINGCODE, TranSnoType.OUTSTANDINGCODE), tran, "GET_ADMINSNO_TRAN") & "'" ''SNO
        strSql += vbCrLf + "," & tranNo & "" 'TRANNO
        strSql += vbCrLf + ",'" & dtpDate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
        strSql += vbCrLf + ",'" & tType & "'" 'TRANTYPE
        strSql += vbCrLf + ",'" & RunNo & "'" 'RUNNO
        strSql += vbCrLf + "," & Math.Abs(Amount) & "" 'AMOUNT
        strSql += vbCrLf + "," & Math.Abs(GrsWt) & "" 'GRSWT
        strSql += vbCrLf + "," & Math.Abs(NetWt) & "" 'NETWT
        strSql += vbCrLf + ",'" & RecPay & "'" 'RECPAY
        strSql += vbCrLf + ",'" & refNo & "'" 'REFNO
        If refDate <> Nothing Then
            strSql += vbCrLf + ",'" & refDate & "'" 'REFDATE
        Else
            strSql += vbCrLf + ",NULL" 'REFDATE
        End If

        strSql += vbCrLf + ",0" 'EMPID
        strSql += vbCrLf + ",''" 'TRANSTATUS
        strSql += vbCrLf + "," & purity & "" 'PURITY
        strSql += vbCrLf + ",'" & CatCode & "'" 'CATCODE
        strSql += vbCrLf + ",'" & batchno & "'" 'BATCHNO
        strSql += vbCrLf + "," & userId & "" 'USERID

        strSql += vbCrLf + ",'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        strSql += vbCrLf + ",'" & Date.Now.ToLongTimeString & "'" 'UPTIME
        strSql += vbCrLf + ",'" & systemId & "'" 'SYSTEMID
        strSql += vbCrLf + "," & Rate & "" 'RATE
        strSql += vbCrLf + "," & Value & "" 'VALUE
        strSql += vbCrLf + ",''" 'CASHID
        strSql += vbCrLf + ",'" & Remark1 & "'" 'REMARK1
        strSql += vbCrLf + ",'" & Remark2 & "'" 'REMARK1
        strSql += vbCrLf + ",'" & accode & "'" 'ACCODE
        strSql += vbCrLf + "," & proId & "" 'CTRANCODE
        If dueDate <> Nothing Then
            strSql += vbCrLf + ",'" & dueDate & "'" 'DUEDATE
        Else
            strSql += vbCrLf + ",NULL" 'DUEDATE
        End If
        strSql += vbCrLf + ",'" & VERSION & "'" 'APPVER
        strSql += vbCrLf + ",'" & CostId & "'" 'COSTID
        strSql += vbCrLf + ",'" & strCompanyId & "'" 'COMPANYID
        strSql += vbCrLf + ",'" & Paymode & "'" 'PAYMODE
        strSql += vbCrLf + ",'A'" 'FROMFLAG
        strSql += vbCrLf + ")"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, CostId)
    End Sub

    Private Sub lstSearch_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstSearch.GotFocus
        If lstSearch.Items.Count > 0 Then
            lstSearch.SelectedIndex = 0
        End If
    End Sub

    Private Sub lstSearch_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles lstSearch.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            searchSender.Text = lstSearch.Text
            If Not searchSender Is Nothing Then
                searchSender.Select()
            End If
            lstSearch.Visible = False
        End If
    End Sub

    Private Sub lstSearch_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstSearch.VisibleChanged
        If lstSearch.Visible = False Then
            searchSender = Nothing
        End If
    End Sub

    Private Sub txtGrid_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtGrid_OWN.KeyUp
        If gridView_OWN.CurrentCell Is Nothing Then Exit Sub
        If gridView_OWN.Columns(gridView_OWN.CurrentCell.ColumnIndex).Name = "DESCRIPTION" Then
            'TextScript(txtGrid_OWN, cmbSearch_OWN, e)
        ElseIf gridView_OWN.Columns(gridView_OWN.CurrentCell.ColumnIndex).Name = "TYPE" Then
            TextScript(txtGrid_OWN, lstSearch, e)
        End If
    End Sub

    Private Sub txtGrid_OWN_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtGrid_OWN.LostFocus
        txtGrid_OWN.BackColor = grpGridView.BackgroundColor
    End Sub

    Private Sub txtGrid_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtGrid_OWN.TextChanged
        lblAmountNWords.Text = ""
        If gridView_OWN.CurrentCell Is Nothing Then Exit Sub
        If gridView_OWN.Columns(gridView_OWN.CurrentCell.ColumnIndex).Name = "DEBIT" Or gridView_OWN.Columns(gridView_OWN.CurrentCell.ColumnIndex).Name = "CREDIT" Then
            Dim amt As Double = Val(txtGrid_OWN.Text)
            If amt > 0 Then
                lblAmountNWords.Text = ConvertRupees.RupeesToWord(amt, "", "")
            End If
        End If
        If gridView_OWN.Columns(gridView_OWN.CurrentCell.ColumnIndex).Name = "TYPE" And searchDia Then
            lstSearch.Visible = True
        ElseIf gridView_OWN.Columns(gridView_OWN.CurrentCell.ColumnIndex).Name = "DESCRIPTION" And searchDia Then
            If txtGrid_OWN.Text = "" Then
                DgvSearch.Visible = False
                Exit Sub
            Else
                DgvSearch.Visible = True
            End If
            If txtGrid_OWN.Focused = False Then Exit Sub


            'Dim _acgroupcode As Integer = 0
            'If cmbAcgroup_Own.Text <> "ALL" And cmbAcgroup_Own.Text <> "" Then
            '    'strSql = " SELECT " & cnAdminDb & ".DBO.WORDCAPS(ACNAME)ACNAME,ACGRPCODE,ACTYPE FROM " & cnAdminDb & "..ACHEAD WHERE 1=1"
            '    'strSql += vbCrLf + "AND ISNULL(MACCODE,'') = ''"
            '    'strSql += vbCrLf + " AND ACGRPCODE = '" & cmbAcgroup_Own.SelectedValue.ToString & "'"
            '    'strSql += GetAcNameQryFilteration()
            '    'strSql += vbCrLf + "ORDER BY ACNAME"
            '    'da = New OleDbDataAdapter(strSql, cn)
            '    'da.Fill(dtAccNames)
            '    _acgroupcode = cmbAcgroup_Own.SelectedValue.ToString
            'End If


            Dim sw As String = txtGrid_OWN.Text
            Dim RowFilterStr As String = VouchFilteration
            If RowFilterStr <> Nothing Then RowFilterStr += " AND "
            RowFilterStr += " ACNAME LIKE '%" & sw & "%'"
            'RowFilterStr += " AND ACGRPCODE = '" & _acgroupcode & "'"
            dtAccNames.DefaultView.RowFilter = RowFilterStr
        Else
            lstSearch.Visible = False
        End If
    End Sub



    Private Sub AddNewRow()
        Dim nRow As DataRow = dtGridView.NewRow
        If Val(nRow.Item("KEYNO").ToString) = 0 Then
            Dim maxVal As Object = dtGridView.Compute("MAX(KEYNO)", "KEYNO IS NOT NULL")
            nRow.Item("KEYNO") = Val(maxVal.ToString)
        End If
        dtGridView.Rows.Add()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        Dim costName As String = cmbCostCenter_MAN.Text

        Dim st() As String = SRVT_COMPONENTS.Split(",")
        If st.Length = 2 Then
            SRVTID = st(0)
            SrvTaxCode = st(1)
            SRVTPER = Val(objGPack.GetSqlValue("SELECT STAX FROM " & cnAdminDb & "..TAXMAST where TAXCODE ='" & SRVTID.ToString() & "'", , "0"))
            SRVTAX = True
        End If
        CostId = Nothing
        OTP_Access = False
        Dim dtDate As Date = dtpDate.Value
        objGPack.TextClear(Me)
        dtpDate.Value = dtDate
        cmbCostCenter_MAN.Text = costName
        BatchNo = Nothing
        dtWeightDetail.Rows.Clear()
        dtGridView.Rows.Clear()
        AddNewRow()
        'dtGridView.Rows.Add()
        dtGridView.AcceptChanges()
        dsOutStDtCol = New DataSet
        'dsOutStDtCol.Tables.Clear()
        If vouchType = VoucherType.Receipt Then
            dtGridView.Rows(0).Item("TYPE") = CrCap '"Cr"
        ElseIf vouchType = VoucherType.Payment Then
            dtGridView.Rows(0).Item("TYPE") = DrCap  '"Dr"
        ElseIf vouchType = VoucherType.Journal Then
            dtGridView.Rows(0).Item("TYPE") = CrCap '"Cr"
        ElseIf vouchType = VoucherType.DebitNote Then
            dtGridView.Rows(0).Item("TYPE") = DrCap '"Dr"
        ElseIf vouchType = VoucherType.CreditNote Then
            dtGridView.Rows(0).Item("TYPE") = CrCap '"Cr"
        End If
        'If cmbCostCenter_MAN.Items.Count > 0 Then
        '    cmbCostCenter_MAN.SelectedIndex = 0
        '    cmbCostCenter_MAN.Refresh()
        'End If
        gridView_OWN.DataSource = dtGridView
        StyleGridView(gridView_OWN)
        dsSl = New DataSet
        gridView_OWN.CurrentCell = gridView_OWN.Rows(0).Cells(0)
        lblPendAmt.Visible = False
        LoadAcc()
        lblbud.Visible = False
        InterStateBill = False
        dtpDate.Select()
        ONETIMEFLAGINVOICENOANDDATE = False
        objInvNo = New frmInvdet
        objBankDetail = New frmAccBankDetails
        PCODE = ""
        DNCNGST_FLAG = False
    End Sub

    Private Sub txtNarration2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtNarration2.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then

            Dim curIndexx As Integer = gridView_OWN.CurrentRow.Index
            Dim tdsAmt As Decimal = Val(gridView_OWN.Rows(curIndexx).Cells("TDSAMT").Value.ToString)
            Dim SrvtAmt As Decimal = Val(gridView_OWN.Rows(curIndexx).Cells("SRVTAMT").Value.ToString)
            Dim srvtFlag As Boolean = False
            Dim TdsFlag As Boolean = False
            If Val(gridView_OWN.Rows(curIndexx).Cells("SRVTAMT").Value.ToString) <> 0 Then
                Dim curIndex As Integer = gridView_OWN.CurrentRow.Index
                If curIndex = gridView_OWN.RowCount - 1 Then
                    NextNewRow()
                End If
                With gridView_OWN.Rows(curIndex)
                    strSql = " SELECT " & cnAdminDb & ".DBO.WORDCAPS(ACNAME)ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = '" & SrvTaxCode & "'"
                    dtGridView.Rows(curIndex + 1).Item("DESCRIPTION") = objGPack.GetSqlValue(strSql)
                    dtGridView.Rows(curIndex + 1).Item("DEBIT") = SrvtAmt
                    dtGridView.Rows(curIndex + 1).Item("TYPE") = DrCap
                    'If .Cells("TYPE").Value.ToString.ToUpper = DrCap.ToUpper Then
                    '    dtGridView.Rows(curIndex + 1).Item("CREDIT") = tdsAmt
                    'Else
                    '    dtGridView.Rows(curIndex + 1).Item("DEBIT") = tdsAmt
                    'End If
                    'dtGridView.Rows(curIndex + 1).Item("TYPE") = IIf(.Cells("TYPE").Value.ToString.ToUpper = DrCap.ToUpper, CrCap, DrCap)
                    dtGridView.Rows(curIndex + 1).Item("GENBY") = "A"
                    If editFlag Then
                        srvtFlag = True
                    End If
                End With
            End If


            If Val(gridView_OWN.Rows(curIndexx).Cells("TDSAMT").Value.ToString) <> 0 Then
                Dim curIndex As Integer = gridView_OWN.CurrentRow.Index
                If curIndex = gridView_OWN.RowCount - 1 Then
                    NextNewRow()
                End If
                With gridView_OWN.Rows(curIndex)
                    strSql = " SELECT " & cnAdminDb & ".DBO.WORDCAPS(ACNAME)ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = "
                    strSql += vbCrLf + "(SELECT TOP 1 ACCODE FROM " & cnAdminDb & "..TDSCATEGORY WHERE TDSCATID = " & Val(gridView_OWN.Rows(curIndexx).Cells("TDSCATID").Value.ToString) & ")"
                    Dim curInd As Integer = curIndexx
                    If srvtFlag Then
                        curInd += 1
                        dtGridView.Rows(curInd + 1).Item("DESCRIPTION") = objGPack.GetSqlValue(strSql)
                        dtGridView.Rows(curInd + 1).Item("CREDIT") = tdsAmt
                        dtGridView.Rows(curInd + 1).Item("TYPE") = CrCap
                        dtGridView.Rows(curInd + 1).Item("GENBY") = "A"
                    Else
                        dtGridView.Rows(curIndex + 1).Item("DESCRIPTION") = objGPack.GetSqlValue(strSql)
                        dtGridView.Rows(curIndex + 1).Item("CREDIT") = tdsAmt
                        dtGridView.Rows(curIndex + 1).Item("TYPE") = CrCap
                        dtGridView.Rows(curIndex + 1).Item("GENBY") = "A"
                    End If
                    'If .Cells("TYPE").Value.ToString.ToUpper = DrCap.ToUpper Then
                    '    dtGridView.Rows(curIndex + 1).Item("CREDIT") = tdsAmt
                    'Else
                    '    dtGridView.Rows(curIndex + 1).Item("DEBIT") = tdsAmt
                    'End If
                    'dtGridView.Rows(curIndex + 1).Item("TYPE") = IIf(.Cells("TYPE").Value.ToString.ToUpper = DrCap.ToUpper, CrCap, DrCap)
                    If editFlag Then
                        TdsFlag = True
                    End If
                End With
            End If
            If GST Then
                Dim Type As String = gridView_OWN.CurrentRow.Cells("TYPE").Value.ToString.ToUpper
                If Val(gridView_OWN.CurrentRow.Cells("IGST").Value.ToString) <> 0 Then
                    Dim curIndex As Integer = gridView_OWN.CurrentRow.Index
                    If curIndex = gridView_OWN.RowCount - 1 Then
                        NextNewRow()
                    End If
                    Dim vatAmt As Decimal = gridView_OWN.Rows(curIndex).Cells("IGST").Value
                    With gridView_OWN.Rows(curIndex)
                        strSql = " SELECT " & cnAdminDb & ".DBO.WORDCAPS(ACNAME)ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = '" & .Cells("IGSTID").Value.ToString & "'"
                        dtGridView.Rows(curIndex + 1).Item("DESCRIPTION") = objGPack.GetSqlValue(strSql)
                        If Type = "CR" Then
                            dtGridView.Rows(curIndex + 1).Item("CREDIT") = vatAmt
                            dtGridView.Rows(curIndex + 1).Item("TYPE") = CrCap
                        Else
                            dtGridView.Rows(curIndex + 1).Item("DEBIT") = vatAmt
                            dtGridView.Rows(curIndex + 1).Item("TYPE") = DrCap
                        End If
                        dtGridView.Rows(curIndex + 1).Item("GENBY") = "A"
                    End With
                Else
                    Dim Sgstflag As Boolean
                    Dim Sgst As Double = Val(gridView_OWN.CurrentRow.Cells("SGST").Value.ToString)
                    Dim Cgst As Double = Val(gridView_OWN.CurrentRow.Cells("CGST").Value.ToString)
                    Dim CgstCode As String = gridView_OWN.CurrentRow.Cells("CGSTID").Value.ToString
                    If Sgst <> 0 Then
                        Dim curIndex As Integer = gridView_OWN.CurrentRow.Index
                        If curIndex = gridView_OWN.RowCount - 1 Then
                            NextNewRow()
                        End If
                        With gridView_OWN.Rows(curIndex)
                            strSql = " SELECT " & cnAdminDb & ".DBO.WORDCAPS(ACNAME)ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = '" & .Cells("SGSTID").Value.ToString & "'"
                            dtGridView.Rows(curIndex + 1).Item("DESCRIPTION") = objGPack.GetSqlValue(strSql)
                            If Type = "CR" Then
                                dtGridView.Rows(curIndex + 1).Item("CREDIT") = Sgst
                                dtGridView.Rows(curIndex + 1).Item("TYPE") = CrCap
                            Else
                                dtGridView.Rows(curIndex + 1).Item("DEBIT") = Sgst
                                dtGridView.Rows(curIndex + 1).Item("TYPE") = DrCap
                            End If
                            dtGridView.Rows(curIndex + 1).Item("GENBY") = "A"
                            Sgstflag = True
                        End With
                    End If
                    If Cgst <> 0 Then
                        Dim curIndex As Integer = gridView_OWN.CurrentRow.Index
                        If curIndex = gridView_OWN.RowCount - 1 Then
                            NextNewRow()
                        End If
                        Dim curInd As Integer = curIndexx
                        If Sgstflag Then curInd += 1
                        With gridView_OWN.Rows(curIndex)
                            strSql = " SELECT " & cnAdminDb & ".DBO.WORDCAPS(ACNAME)ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = '" & CgstCode & "'"
                            dtGridView.Rows(curInd + 1).Item("DESCRIPTION") = objGPack.GetSqlValue(strSql)
                            If Type = "CR" Then
                                dtGridView.Rows(curInd + 1).Item("CREDIT") = Cgst
                                dtGridView.Rows(curInd + 1).Item("TYPE") = CrCap
                            Else
                                dtGridView.Rows(curInd + 1).Item("DEBIT") = Cgst
                                dtGridView.Rows(curInd + 1).Item("TYPE") = DrCap
                            End If
                            dtGridView.Rows(curInd + 1).Item("GENBY") = "A"
                        End With
                    End If
                End If
            Else
                If Val(gridView_OWN.CurrentRow.Cells("VATAMT").Value.ToString) <> 0 Then
                    Dim curIndex As Integer = gridView_OWN.CurrentRow.Index
                    If curIndex = gridView_OWN.RowCount - 1 Then
                        NextNewRow()
                    End If
                    Dim vatAmt As Decimal = gridView_OWN.Rows(curIndex).Cells("VATAMT").Value
                    With gridView_OWN.Rows(curIndex)
                        strSql = " SELECT " & cnAdminDb & ".DBO.WORDCAPS(ACNAME)ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = '" & .Cells("vatCATID").Value.ToString & "'"
                        dtGridView.Rows(curIndex + 1).Item("DESCRIPTION") = objGPack.GetSqlValue(strSql)
                        dtGridView.Rows(curIndex + 1).Item("CREDIT") = vatAmt
                        dtGridView.Rows(curIndex + 1).Item("TYPE") = CrCap
                        'If .Cells("TYPE").Value.ToString.ToUpper = DrCap.ToUpper Then
                        '    dtGridView.Rows(curIndex + 1).Item("CREDIT") = tdsAmt
                        'Else
                        '    dtGridView.Rows(curIndex + 1).Item("DEBIT") = tdsAmt
                        'End If
                        'dtGridView.Rows(curIndex + 1).Item("TYPE") = IIf(.Cells("TYPE").Value.ToString.ToUpper = DrCap.ToUpper, CrCap, DrCap)
                        dtGridView.Rows(curIndex + 1).Item("GENBY") = "A"
                    End With
                End If
            End If
            NextNewRow()
            objTds = New frmAccTds
        End If
        cmbNarration.Visible = False
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If editFlag Then
            If Not BrighttechPack.Methods.GetRights(_DtUserRights, mnuId & Me.Name, BrighttechPack.Methods.RightMode.Edit) Then Exit Sub
        Else
            If Not BrighttechPack.Methods.GetRights(_DtUserRights, mnuId & Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
        End If
        If editFlag = False Then
            If Not CheckBckDaysEntry(userId, "", dtpDate.Value, "Account Entry") Then
                dtpDate.Focus()
                Exit Sub
            End If
        End If
        If DemoLogin Then
            MsgBox(E0025, MsgBoxStyle.Exclamation)
            Exit Sub
        End If
        If checkBdate() = False Then Exit Sub
        If CheckTrialDate(dtpDate.Value) = False Then Exit Sub
        If Not CheckDayEnd(dtpDate.Value) Then Exit Sub
        Dim debit As Object = dtGridView.Compute("SUM(DEBIT)", "DEBIT IS NOT NULL")
        If Val(debit.ToString) = 0 Then
            MsgBox("There is no Record", MsgBoxStyle.Information)
            Exit Sub
        End If
        If GetGridViewTotal() <> 0 Then
            MsgBox("Debit Credit Not Tally", MsgBoxStyle.Information)
            gridView_OWN.CurrentCell = gridView_OWN.CurrentRow.Cells(0)
            gridView_OWN.Select()
            Exit Sub
        End If

        If cmbCostCenter_MAN.Enabled = True Then
            If cmbCostCenter_MAN.Text = "" Then
                MsgBox("Cost center Empty", MsgBoxStyle.Information)
                cmbCostCenter_MAN.Select()
                Exit Sub
            Else
                'If objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre.Text & "'") = "" Then
                If objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCenter_MAN.Text & "'") = "" Then MsgBox("Please select valid costcentre", MsgBoxStyle.Critical) : cmbCostCenter_MAN.Focus() : Exit Sub
            End If
        End If

        If cmbAgainst_Own.Text = "" And (vouchType = VoucherType.DebitNote Or vouchType = VoucherType.CreditNote) Then
            MsgBox("For Type Empty", MsgBoxStyle.Information)
            cmbAgainst_Own.Select()
            Exit Sub
        End If

        PCODE = "" '' FOR VBJ 
        If cmbAgainst_Own.Text <> "" Then
            PCODE = cmbAgainst_Own.SelectedValue.ToString()
        End If

        Try
            tran = Nothing
            tran = cn.BeginTransaction()
            strSql = " IF (SELECT COUNT(*) FROM " & cnStockDb & "..SYSOBJECTS WHERE NAME = '" & mtempacctran & "')> 0"
            strSql += vbCrLf + "DROP TABLE " & cnStockDb & ".." & mtempacctran
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()

            strSql = " SELECT  SNO, TRANNO, TRANDATE, TRANMODE, ACCODE, SACCODE, AMOUNT, BALANCE, PCS, GRSWT, NETWT, PUREWT, REFNO, REFDATE, PAYMODE, CHQCARDNO, CARDID, CHQCARDREF, CHQDATE, BRSFLAG, RELIASEDATE, FROMFLAG, REMARK1, REMARK2, CONTRA, BatchNo, userId, UPDATED, UPTIME, systemId, CANCEL, CASHID, COSTID, SCOSTID, APPVER, COMPANYID, TRANSFERED, WT_ENTORDER, Rate, TRANFLAG, PCODE, Disc_EmpId, CASHPOINTID, TDSCATID, TDSPER, TDSAMOUNT, FLAG, ENTREFNO, ESTBATCHNO"
            strSql += vbCrLf + "INTO " & cnStockDb & ".." & mtempacctran & " FROM " & cnStockDb & "..ACCTRAN WHERE 1=2"
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()

            'strSql = " SELECT * INTO " & cnStockDb & ".." & mtempacctran & " FROM " & cnStockDb & ".." & IIf(Approval, "TACCTRAN", "ACCTRAN") & " WHERE 1=2"
            'cmd = New OleDbCommand(strSql, cn, tran)
            'cmd.ExecuteNonQuery()

            If editFlag Then
                ''delete
                strSql = " DELETE FROM " & cnStockDb & ".." & IIf(Approval, "TISSUE", "ISSUE") & ""
                strSql += vbCrLf + "WHERE BATCHNO = '" & BatchNo & "'"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, CostId)
                strSql = " DELETE FROM " & cnStockDb & ".." & IIf(Approval, "TRECEIPT", "RECEIPT") & ""
                strSql += vbCrLf + "WHERE BATCHNO = '" & BatchNo & "'"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, CostId)
                strSql = " DELETE FROM " & cnStockDb & ".." & IIf(Approval, "TACCTRAN", "ACCTRAN") & ""
                strSql += vbCrLf + "WHERE BATCHNO = '" & BatchNo & "'"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, CostId)
                strSql = " DELETE FROM " & cnAdminDb & ".." & IIf(Approval, "TOUTSTANDING", "OUTSTANDING") & ""
                strSql += vbCrLf + "WHERE BATCHNO = '" & BatchNo & "'"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, CostId)
                strSql = " DELETE FROM " & cnStockDb & "..TAXTRAN "
                strSql += vbCrLf + "WHERE BATCHNO = '" & BatchNo & "'"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, CostId)
                Edchqno = Nothing
                Edchqdate = Nothing
                Edchqdetail = Nothing
                GoTo InsertEntry
            End If
            payMode = objGPack.GetSqlValue("SELECT PAYMODE FROM " & cnAdminDb & "..ACCENTRYMASTER WHERE SNO = '" & mnuId & "'", , , tran)
            Dim Isfirst As Boolean = True
GenBillNo:
            TranNo = Val(GetBillControlValue(ctrlId, tran, Not Isfirst))
            strSql = " UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT = '" & TranNo + 1 & "'"
            strSql += vbCrLf + "WHERE CTLID = '" & ctrlId & "' AND COMPANYID = '" & strCompanyId & "'"
            If Isfirst And strBCostid <> Nothing Then strSql += vbCrLf + "AND COSTID ='" & strBCostid & "'"
            strSql += vbCrLf + "AND CONVERT(INT,CTLTEXT) = '" & TranNo & "'"
            cmd = New OleDbCommand(strSql, cn, tran)
            If Not cmd.ExecuteNonQuery() > 0 Then
                If strBCostid <> Nothing Then MsgBox("Tran No. empty. Please check Bill control") : tran.Rollback() : tran.Dispose() : tran = Nothing : Exit Sub
                Isfirst = False
                GoTo GenBillNo
            End If
            TranNo += 1
            BatchNo = GetNewBatchno(cnCostId, dtpDate.Value.ToString("yyyy-MM-dd"), tran)
            If BatchNo = "" Then
                tran.Rollback()
                tran = Nothing
                MsgBox("Batchno is empty", MsgBoxStyle.Information)
                Exit Sub
            End If
InsertEntry:
            CostId = objGPack.GetSqlValue("SELECT ISNULL(COSTID,'') FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCenter_MAN.Text & "'", , , tran)
            Dim SingleMulty As String = Nothing
            Dim drRows() As DataRow = Nothing
            Dim crRows() As DataRow = Nothing
            If vouchType = VoucherType.Journal Then
                drRows = dtGridView.Select("TYPE = '" & DrCap & "' AND (GENBY = '' OR GENBY IS NULL)")
                crRows = dtGridView.Select("TYPE = '" & CrCap & "' AND (GENBY = '' OR GENBY IS NULL)")
            Else
                drRows = dtGridView.Select("TYPE = '" & DrCap & "'")
                crRows = dtGridView.Select("TYPE = '" & CrCap & "'")
            End If
            If drRows.Length <> crRows.Length Then
                If drRows.Length = 1 Then
                    SingleMulty = DrCap '"DR"
                ElseIf crRows.Length = 1 Then
                    SingleMulty = CrCap '"CR"
                End If
            End If
            Dim TdsCatId As Integer = Nothing
            Dim TdsPer As Decimal = Nothing
            Dim TdsAmt As Decimal = Nothing
            Dim MTdsCatId As Integer = Nothing
            Dim MTdsPer As Decimal = Nothing

            'If SingleMulty <> Nothing Then ''SingleToMulty System
            '    TdsCatId = Val(IIf(SingleMulty = DrCap, drRows(0)!TDSCATID.ToString, crRows(0)!TDSCATID.ToString))
            '    TdsPer = Val(IIf(SingleMulty = DrCap, drRows(0)!TDSPER.ToString, crRows(0)!TDSPER.ToString))
            '    TdsAmt = Val(IIf(SingleMulty = DrCap, drRows(0)!TDSAMT.ToString, crRows(0)!TDSAMT.ToString))
            '    If dsSl.Tables.Count > 0 Then
            '        tran.Rollback()
            '        tran.Dispose()
            '        tran = Nothing
            '        MsgBox("Using subledger single to multi type of entry not possible: ", MsgBoxStyle.Information)
            '        gridView_OWN.CurrentCell = gridView_OWN.CurrentRow.Cells(0)
            '        gridView_OWN.Select()
            '        Exit Sub
            '    End If
            'End If
            Dim chqNo As String = Nothing
            Dim chqDate As String = Nothing
            Dim chqDetail As String = Nothing
            For Each ro As DataRow In dtGridView.Rows
                If editFlag Then
                    'If objGPack.GetSqlValue("SELECT ACGRPCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & ro!description.ToString & "'") = "2" Then
                    If objGPack.GetSqlValue("SELECT ACTYPE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & ro!description.ToString & "'", , , tran) = "B" Then
                        chqNo = IIf(IsDBNull(ro!CHQNO.ToString), "", ro!CHQNO.ToString)
                        'If Val(chqNo.ToString) <> 0 Then
                        If chqNo.ToString <> "" Then
                            'FOR JMT
                            'chqDate = ro!CHQDATE
                            'chqDetail = ro!CHQDETAIL.ToString
                            chqDate = IIf(IsDBNull(ro!CHQDATE.ToString), "", ro!CHQDATE.ToString)
                            chqDetail = IIf(IsDBNull(ro!CHQDETAIL.ToString), "", ro!CHQDETAIL.ToString)
                            Exit For
                        End If
                    End If
                Else
                    chqNo = IIf(IsDBNull(ro!CHQNO.ToString), "", ro!CHQNO.ToString)
                    'If Val(chqNo.ToString) <> 0 Then
                    If chqNo.ToString <> "" Then
                        chqDate = IIf(IsDBNull(ro!CHQDATE.ToString), "", ro!CHQDATE.ToString)
                        chqDetail = IIf(IsDBNull(ro!CHQDETAIL.ToString), "", ro!CHQDETAIL.ToString)
                        Exit For
                    End If
                End If
            Next

            If chqNo Is Nothing Then chqNo = 0
            If Val("" & chqNo.ToString) = 0 And SingleMulty <> Nothing Then ''SingleToMulty System
                chqNo = Val(IIf(SingleMulty = DrCap, drRows(0)!CHQNO.ToString, crRows(0)!CHQNO.ToString))
                chqDate = IIf(SingleMulty = DrCap, drRows(0)!CHQDATE.ToString, crRows(0)!CHQDATE.ToString)
                chqDetail = Val(IIf(SingleMulty = DrCap, drRows(0)!CHQDETAIL.ToString, crRows(0)!CHQDETAIL.ToString))

            End If
            Dim totdramt As Decimal = 0
            Dim totcramt As Decimal = 0
            Dim gsaccode As String = ""
            Dim blankcode As Boolean = False
            For Each ro As DataRow In dtGridView.Rows
                If ro.Item("DESCRIPTION").ToString = "" Then Continue For
                'If ro.Item("GENBY").ToString = "A" Then Continue For
                If Val(ro!tdsamt.ToString) <> 0 Then
                    TdsAmt = Val(ro!tdsamt.ToString)
                    TdsPer = Val(ro!TDSPER.ToString)
                    TdsCatId = ro!TDSCATID.ToString
                    MTdsPer = TdsPer : MTdsCatId = TdsCatId
                End If

                Dim amt As Double = IIf(Val(ro!DEBIT.ToString) > 0, Val(ro!DEBIT.ToString), Val(ro!CREDIT.ToString))
                Dim tranMode As Char = IIf(Val(ro!DEBIT.ToString) > 0, "D", "C")
                Dim recPay As Char = IIf(Val(ro!DEBIT.ToString) > 0, "P", "R")
                Dim Contra As String = Nothing
                Dim keyNo As Integer = Val(ro.Item("KEYNO").ToString)
                Dim accode As String = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & ro!DESCRIPTION & "'", , , tran)
                If gsaccode = "" Then gsaccode = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & ro!DESCRIPTION & "' AND ACTYPE IN('G','S','D')", , , tran)
                Dim Scostid As String = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & ro!SCOSTCENTRE & "'", , , tran)
                Dim adjAmt As Double = amt
                If tranMode = "D" Then totdramt += amt Else totcramt += amt
                If dsOutStDtCol.Tables.Contains(ro!DESCRIPTION.ToString) Then
                    For Each roOutSt As DataRow In dsOutStDtCol.Tables(ro!DESCRIPTION.ToString).Rows
                        InsertIntoOustanding(TranNo, BatchNo, roOutSt!TRANTYPE.ToString, roOutSt!RUNNO.ToString, Val(roOutSt!ADJUST.ToString), recPay _
                        , IIf(recPay = "R", "DR", "DP"), , , , , , , , , , , , , accode)
                        adjAmt -= Val(roOutSt!ADJUST.ToString)
                    Next
                End If

                If objGPack.GetSqlValue("SELECT OUTSTANDING FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & ro.Item("DESCRIPTION").ToString & "'", , , tran) = "Y" And adjAmt <> 0 Then
                    ''BALANCE OUTST ADJUST AMT
                    Dim runNo As String = Nothing
                    runNo = GetCostId(cnCostId) & GetCompanyId(strCompanyId) & "G" + Mid(Format(cnTranToDate, "dd/MM/yyyy"), 9, 2).ToString + GetTranDbSoftControlValue("RUNNO_ACC", True, tran)
                    Dim mInvno As String = "99999"
                    If objInvNo.txtInvno.Text <> "" Then mInvno = objInvNo.txtInvno.Text
                    InsertIntoOustanding(TranNo, BatchNo, "T", runNo, adjAmt, recPay _
                    , IIf(recPay = "R", "DR", "DP"), , , , , , mInvno, Format(objInvNo.dtpInvDate.Value, "yyyy/MM/dd"), , , , , , accode)
                End If
                Dim RefInvno As String = IIf(objInvNo.txtInvno.Text <> "", objInvNo.txtInvno.Text, "")
                Dim Refinvdate As Date = IIf(objInvNo.txtInvno.Text <> "", Format(objInvNo.dtpInvDate.Value, "yyyy/MM/dd"), Nothing)
                If SingleMulty <> Nothing Then ''SingleToMulty System
                    If UCase(ro!TYPE.ToString) <> SingleMulty.ToUpper Then
                        ''Single Entry
                        Dim sChqNo As String = IIf(IsDBNull(ro!CHQNO.ToString), "", ro!CHQNO.ToString) 'ro!CHQNO.ToString
                        Dim sChqDate As String = Nothing
                        Dim sChqDetail As String = Nothing
                        'FOR JMT
                        'If Val(sChqNo) <> 0 Then sChqDate = ro!CHQDATE : sChqDetail = ro!CHQDETAIL.ToString
                        'If Val(sChqNo) = 0 Then sChqNo = chqNo : sChqDate = chqDate : sChqDetail = chqDetail
                        If sChqNo <> "" Then sChqDate = IIf(IsDBNull(ro!CHQDATE.ToString), "", ro!CHQDATE.ToString) : sChqDetail = IIf(IsDBNull(ro!CHQDETAIL.ToString), "", ro!CHQDETAIL.ToString)
                        If sChqNo = "" Then sChqNo = chqNo : sChqDate = chqDate : sChqDetail = chqDetail

                        Dim Contracode As String = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & IIf(SingleMulty = DrCap, drRows(0)!DESCRIPTION.ToString, crRows(0)!DESCRIPTION.ToString) & "'", , , tran)
                        Contra = Contracode
                        Dim sNarr1 As String = IIf(SingleMulty = DrCap, drRows(0)!NARRATION1.ToString, crRows(0)!NARRATION1.ToString) '"DR"
                        Dim sNarr2 As String = IIf(SingleMulty = DrCap, drRows(0)!NARRATION2.ToString, crRows(0)!NARRATION2.ToString) ' "DR"
                        Dim sTds As Double = IIf(SingleMulty = DrCap, Val(drRows(0)!TDSAMT.ToString), Val(crRows(0)!TDSAMT.ToString)) '"DR"
                        If dsSl.Tables.Contains(ro!DESCRIPTION.ToString) Then
                            For Each roSl As DataRow In dsSl.Tables(ro!DESCRIPTION.ToString).Rows
                                If Val(roSl!amount.ToString) = 0 Then Continue For
                                Dim SlAccode As String = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & roSl!ACNAME.ToString & "'", , , tran)
                                InsertIntoAccTran(Val(IIf(SingleMulty = DrCap, drRows(0)!KEYNO.ToString, crRows(0)!KEYNO.ToString)) _
                                                            , TranNo, tranMode _
                                                            , accode, roSl!amount, 0, 0, 0, payMode, Contracode, IIf(TdsAmt <> amt, TdsCatId, 0) _
                                                            , IIf(TdsAmt <> amt, TdsPer, 0) _
                                                            , IIf(TdsAmt <> amt, TdsAmt, 0), RefInvno, Refinvdate, sChqNo, sChqDate, , sChqDetail, sNarr1, sNarr2, , SlAccode, Scostid)
                            Next
                            InsertIntoAccTran(keyNo, TranNo, IIf(tranMode = "C", "D", "C") _
                        , Contra, amt, 0, 0, 0, payMode, accode, 0, 0, 0, RefInvno, Refinvdate, sChqNo, sChqDate, , sChqDetail, ro!NARRATION1.ToString, ro!NARRATION2.ToString, ro!GENBY.ToString, , Scostid)

                        Else
                            InsertIntoAccTran(Val(IIf(SingleMulty = DrCap, drRows(0)!KEYNO.ToString, crRows(0)!KEYNO.ToString)) _
                            , TranNo, IIf(tranMode = "C", "D", "C") _
                            , Contracode, amt, 0, 0, 0, payMode, accode, IIf(TdsAmt <> amt, TdsCatId, 0) _
                            , IIf(TdsAmt <> amt, TdsPer, 0) _
                            , IIf(TdsAmt <> amt, TdsAmt, 0), RefInvno, Refinvdate, sChqNo, sChqDate, , sChqDetail, sNarr1, sNarr2, , , Scostid)
                            If TdsAmt <> amt Then TdsCatId = 0 : TdsPer = 0 : TdsAmt = 0
                            ''Multi Entry
                            InsertIntoAccTran(keyNo, TranNo, tranMode _
                        , accode, amt, 0, 0, 0, payMode, Contra, 0, 0, 0, RefInvno, Refinvdate, sChqNo, sChqDate, , sChqDetail, ro!NARRATION1.ToString, ro!NARRATION2.ToString, ro!GENBY.ToString, , Scostid)

                        End If
                    Else
                        If dsSl.Tables.Contains(ro!DESCRIPTION.ToString) Then
                            tran.Rollback()
                            tran.Dispose()
                            tran = Nothing
                            MsgBox("Using subledger single to multi type of entry not possible: ", MsgBoxStyle.Information)
                            gridView_OWN.CurrentCell = gridView_OWN.CurrentRow.Cells(0)
                            gridView_OWN.Select()
                            Exit Sub
                        End If
                    End If
                Else ''multy to multy or single to single
                    If vouchType = VoucherType.Journal Then
                        If Val(ro.Item("TDSAMT").ToString) <> 0 Then
                            Dim tdsAccode As String = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..TDSCATEGORY WHERE TDSCATID = '" & ro.Item("TDSCATID").ToString & "'", , , tran)
                            ''Like Tds Entry
                            InsertIntoAccTran(keyNo, TranNo, tranMode _
                              , accode, amt + Val(ro.Item("TDSAMT").ToString), 0, 0, 0, payMode, Contra, Val(ro.Item("TDSCATID").ToString), Val(ro.Item("TDSPER").ToString), Val(ro.Item("TDSAMT").ToString), RefInvno, Refinvdate, chqNo, chqDate, , chqDetail, ro!NARRATION1.ToString, ro!NARRATION2.ToString, ro!GENBY.ToString, , Scostid)
                            InsertIntoAccTran(keyNo, TranNo, IIf(tranMode = "D", "C", "D") _
                               , accode, Val(ro.Item("TDSAMT").ToString), 0, 0, 0, payMode, tdsAccode, 0, 0, 0, RefInvno, Refinvdate, chqNo, chqDate, , chqDetail, ro!NARRATION1.ToString, ro!NARRATION2.ToString, "T", , Scostid)
                        Else
                            ''Normal Entry
                            If dsSl.Tables.Contains(ro!DESCRIPTION.ToString) Then
                                For Each roSl As DataRow In dsSl.Tables(ro!DESCRIPTION.ToString).Rows
                                    If Val(roSl!amount.ToString) = 0 Then Continue For
                                    Dim SlAccode As String = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & roSl!ACNAME.ToString & "'", , , tran)
                                    InsertIntoAccTran(keyNo, TranNo, tranMode _
                                   , accode, roSl!amount, 0, 0, 0, payMode, Contra, Val(ro.Item("TDSCATID").ToString), Val(ro.Item("TDSPER").ToString), Val(ro.Item("TDSAMT").ToString), RefInvno, Refinvdate, chqNo, chqDate, , chqDetail, ro!NARRATION1.ToString, ro!NARRATION2.ToString, ro!GENBY.ToString, SlAccode)
                                Next
                            Else
                                InsertIntoAccTran(keyNo, TranNo, tranMode _
                                , accode, amt, 0, 0, 0, payMode, Contra, Val(ro.Item("TDSCATID").ToString), Val(ro.Item("TDSPER").ToString), Val(ro.Item("TDSAMT").ToString), RefInvno, Refinvdate, chqNo, chqDate, , chqDetail, ro!NARRATION1.ToString, ro!NARRATION2.ToString, ro!GENBY.ToString, , Scostid)
                            End If
                        End If
                    Else
                        If dsSl.Tables.Contains(ro!DESCRIPTION.ToString) Then
                            For Each roSl As DataRow In dsSl.Tables(ro!DESCRIPTION.ToString).Rows
                                If Val(roSl!amount.ToString) = 0 Then Continue For
                                Dim SlAccode As String = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & roSl!ACNAME.ToString & "'", , , tran)
                                InsertIntoAccTran(keyNo, TranNo, tranMode _
                                  , accode, roSl!amount, 0, 0, 0, payMode, Contra, Val(ro.Item("TDSCATID").ToString), Val(ro.Item("TDSPER").ToString), Val(ro.Item("TDSAMT").ToString), RefInvno, Refinvdate, chqNo, chqDate, , chqDetail, ro!NARRATION1.ToString, ro!NARRATION2.ToString, ro!GENBY.ToString, SlAccode, Scostid)
                            Next

                        Else

                            InsertIntoAccTran(keyNo, TranNo, tranMode _
                             , accode, amt, 0, 0, 0, payMode, Contra, Val(ro.Item("TDSCATID").ToString), Val(ro.Item("TDSPER").ToString), Val(ro.Item("TDSAMT").ToString), RefInvno, Refinvdate, chqNo, chqDate, , chqDetail, ro!NARRATION1.ToString, ro!NARRATION2.ToString, ro!GENBY.ToString, , Scostid)
                        End If
                    End If

                End If

                ''issue/receipt
                If (_Title = "DEBIT NOTE" Or _Title = "CREDIT NOTE") And Auto_GST_DNCN = "Y" Then
                Else
                    For cnt As Integer = 0 To dtWeightDetail.Rows.Count - 1
                        With dtWeightDetail.Rows(cnt)
                            Dim type As String = ""
                            If tranMode = "C" Then
                                Select Case .Item("TYPE").ToString
                                    Case AccEntryWeightDetail.Type.SALES.ToString
                                        type = "SA"
                                    Case AccEntryWeightDetail.Type.SMITH_ISSUE.ToString
                                        type = "ISS"
                                    Case AccEntryWeightDetail.Type.PURCHASE_RETURN.ToString
                                        type = "IPU"
                                    Case Else
                                        type = "SA"
                                End Select
                            Else
                                Select Case .Item("TYPE").ToString
                                    Case AccEntryWeightDetail.Type.PURCHASE.ToString
                                        type = "RPU"
                                    Case AccEntryWeightDetail.Type.SMITH_RECEIPT.ToString
                                        type = "RRE"
                                    Case AccEntryWeightDetail.Type.SMITH_PURCHASE.ToString
                                        type = "RPU"
                                    Case Else
                                        type = "PU"
                                End Select
                            End If
                            If Val(.Item("KEYNO").ToString) = Val(ro.Item("KEYNO").ToString) Then
                                If gsaccode = "" Then blankcode = True Else blankcode = False
                                InsertIntoIssueReceipt(IIf(tranMode = "C", "ISSUE", "RECEIPT"), type _
                            , objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & .Item("CATNAME").ToString & "'", , , tran) _
                            , Val(.Item("PCS").ToString), Val(.Item("GRSWT").ToString), Val(.Item("NETWT").ToString), Val(.Item("PUREWT").ToString), 0, 0, Val(.Item("RATE").ToString) _
                            , amt, Val(.Item("VAT").ToString), 0, 0, 0, .Item("CALCMODE").ToString, .Item("UNIT").ToString, gsaccode, keyNo, .Item("Refno"), .Item("Refdate"))

                                If .Item("TYPE").ToString = AccEntryWeightDetail.Type.PURCHASE.ToString Then
                                    If type = "RPU" Then type = "ISS"
                                    InsertIntoIssueReceipt(IIf(tranMode = "C", "RECEIPT", "ISSUE"), type _
                                , objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & .Item("CATNAME").ToString & "'", , , tran) _
                                , Val(.Item("PCS").ToString), Val(.Item("GRSWT").ToString), Val(.Item("NETWT").ToString), Val(.Item("PUREWT").ToString), 0, 0, Val(.Item("RATE").ToString) _
                                , 0, 0, 0, 0, 0, .Item("CALCMODE").ToString, .Item("UNIT").ToString, gsaccode, keyNo, .Item("Refno"), .Item("Refdate"))
                                End If

                                Exit For
                            End If
                        End With
                    Next
                End If
                Dim Curchqno As String = ro!CHQNO.ToString
                If Curchqno <> "" Then
                    strSql = "UPDATE " & cnStockDb & "..CHEQUEBOOK SET CHQISSUEDATE='" & System.DateTime.Now.ToString("yyyy-MM-dd") & "' , AMOUNT=" & amt & " WHERE " & IIf(CostId <> Nothing, "COSTID='" & CostId & "' AND", "") & " BANKCODE='" & accode & "' AND CHQNUMBER='" & Curchqno & "'"
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, CostId)
                End If
                If objBankDetail.CmbChqDetail_OWN.Text <> "" Then
                    If Val(objGPack.GetSqlValue("SELECT 1 FROM " & cnAdminDb & "..ACCCHQFAVOUR WHERE  FAVNAME = '" & objBankDetail.CmbChqDetail_OWN.Text & "'", , , tran).ToString) <> 1 Then
                        strSql = "INSERT INTO " & cnAdminDb & "..ACCCHQFAVOUR(FAVNAME,ACTIVE,UPDATED) VALUES ('" & objBankDetail.CmbChqDetail_OWN.Text & "','Y','" & System.DateTime.Now.ToString("yyyy-MM-dd") & "') "
                        ExecQuery(SyncMode.Transaction, strSql, cn, tran, CostId)
                    End If
                End If



                If totdramt = totcramt Then
                    If SingleMulty = Nothing Then
                        If editFlag Then
                            strSql = " UPDATE " & cnStockDb & ".." & IIf(Approval, "TACCTRAN", "ACCTRAN") & " SET CONTRA  = (SELECT TOP 1 ACCODE FROM " & cnStockDb & ".." & IIf(Approval, "TACCTRAN", "ACCTRAN") & " WHERE TRANDATE = T.TRANDATE AND AMOUNT = T.AMOUNT AND BATCHNO = T.BATCHNO AND TRANMODE = 'D' AND ACCODE <> T.ACCODE AND COMPANYID = '" & strCompanyId & "' ORDER BY SNO)"
                            strSql += vbCrLf + "FROM " & cnStockDb & ".." & IIf(Approval, "TACCTRAN", "ACCTRAN") & " AS T WHERE TRANDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' AND BATCHNO = '" & BatchNo & "' AND  TRANMODE = 'C' AND COMPANYID = '" & strCompanyId & "' AND ISNULL(CONTRA,'') = ''"
                            ExecQuery(SyncMode.Transaction, strSql, cn, tran, CostId)

                            strSql = " UPDATE " & cnStockDb & ".." & IIf(Approval, "TACCTRAN", "ACCTRAN") & " SET CONTRA  = (SELECT TOP 1 ACCODE FROM " & cnStockDb & ".." & IIf(Approval, "TACCTRAN", "ACCTRAN") & " WHERE TRANDATE = T.TRANDATE AND AMOUNT =T.AMOUNT AND BATCHNO = T.BATCHNO AND TRANMODE = 'C' AND ACCODE <> T.ACCODE AND COMPANYID = '" & strCompanyId & "' ORDER BY SNO)"
                            strSql += vbCrLf + "FROM " & cnStockDb & ".." & IIf(Approval, "TACCTRAN", "ACCTRAN") & " AS T WHERE TRANDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' AND BATCHNO = '" & BatchNo & "' AND TRANMODE = 'D' AND COMPANYID = '" & strCompanyId & "' AND ISNULL(CONTRA,'') = ''"
                            ExecQuery(SyncMode.Transaction, strSql, cn, tran, CostId)
                        Else
                            strSql = " UPDATE " & cnStockDb & ".." & mtempacctran & " SET CONTRA  = (SELECT TOP 1 ACCODE FROM " & cnStockDb & ".." & mtempacctran & " WHERE TRANDATE = T.TRANDATE AND AMOUNT = T.AMOUNT AND BATCHNO = T.BATCHNO AND TRANMODE = 'D' AND ACCODE <> T.ACCODE AND COMPANYID = '" & strCompanyId & "' ORDER BY SNO)"
                            strSql += vbCrLf + "FROM " & cnStockDb & ".." & mtempacctran & " AS T WHERE TRANDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' AND BATCHNO = '" & BatchNo & "' AND  TRANMODE = 'C' AND COMPANYID = '" & strCompanyId & "' AND ISNULL(CONTRA,'') = ''"
                            cmd = New OleDbCommand(strSql, cn, tran)
                            cmd.ExecuteNonQuery()
                            'ExecQuery(SyncMode.Transaction, strSql, cn, tran, CostId)

                            strSql = " UPDATE " & cnStockDb & ".." & mtempacctran & " SET CONTRA  = (SELECT TOP 1 ACCODE FROM " & cnStockDb & ".." & mtempacctran & " WHERE TRANDATE = T.TRANDATE AND AMOUNT =T.AMOUNT AND BATCHNO = T.BATCHNO AND TRANMODE = 'C' AND ACCODE <> T.ACCODE AND COMPANYID = '" & strCompanyId & "' ORDER BY SNO)"
                            strSql += vbCrLf + "FROM " & cnStockDb & ".." & mtempacctran & " AS T WHERE TRANDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' AND BATCHNO = '" & BatchNo & "' AND TRANMODE = 'D' AND COMPANYID = '" & strCompanyId & "' AND ISNULL(CONTRA,'') = ''"
                            'ExecQuery(SyncMode.Transaction, strSql, cn, tran, CostId)
                            cmd = New OleDbCommand(strSql, cn, tran)
                            cmd.ExecuteNonQuery()
                        End If

                    End If
                    totdramt = 0 : totcramt = 0
                End If

            Next

            'Dim DRR As DataRow = dtGridView.Select("TDSAMT<>0 OR SRVTAMT<>0", Nothing)
            Dim _Amt As Double
            Dim SGSTAmt As Double
            Dim CGSTAmt As Double
            Dim IGSTAmt As Double
            Dim SGSTPer As Decimal
            Dim CGSTPer As Decimal
            Dim IGSTPer As Decimal
            TdsAmt = Val(dtGridView.Compute("sum(TDSAMT)", "TDSAMT <> 0").ToString)
            Dim SrvtAmt As Double = Val(dtGridView.Compute("sum(SRVTAMT)", "SRVTAMT <> 0").ToString)
            SGSTAmt = Val(dtGridView.Compute("SUM(SGST)", "SGST <> 0").ToString)
            CGSTAmt = Val(dtGridView.Compute("SUM(CGST)", "CGST <> 0").ToString)
            IGSTAmt = Val(dtGridView.Compute("SUM(IGST)", "IGST <> 0").ToString)
            SGSTPer = Val(dtGridView.Compute("SUM(SGSTPER)", "SGSTPER <> 0").ToString)
            CGSTPer = Val(dtGridView.Compute("SUM(CGSTPER)", "CGSTPER <> 0").ToString)
            IGSTPer = Val(dtGridView.Compute("SUM(IGSTPER)", "IGSTPER <> 0").ToString)
            Dim drr() As DataRow
            drr = dtGridView.Select("TDSAMT<>0 OR SRVTAMT <> 0 OR SGST <> 0 OR CGST <> 0 OR IGST <> 0")
            Dim Partycode As String = ""
            Dim PartyContra As String = ""
            If TdsAmt <> 0 Or SrvtAmt <> 0 Or SGSTAmt <> 0 Or CGSTAmt <> 0 Or IGSTAmt <> 0 Then
                Partycode = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & drr(0).Item("DESCRIPTION").ToString & "'", , , tran).ToString
                strSql = vbCrLf + " SELECT CONTRA FROM " & cnStockDb & ".." & mtempacctran & " WHERE ACCODE ='" & Partycode & "'"
                PartyContra = objGPack.GetSqlValue(strSql, "", "", tran)
                _Amt = Val(drr(0).Item("CREDIT").ToString)
                If _Amt = 0 Then
                    _Amt = Val(drr(0).Item("DEBIT").ToString)
                End If
            End If
            If SGSTAmt <> 0 Then
                strSql = " INSERT INTO " & cnStockDb & "..TAXTRAN"
                strSql += vbCrLf + "("
                strSql += vbCrLf + "SNO,ACCODE,CONTRA,TRANNO,TRANDATE,TRANTYPE,BATCHNO,TAXID"
                strSql += vbCrLf + ",AMOUNT,TAXPER,TAXAMOUNT,TSNO,COSTID,COMPANYID"
                strSql += vbCrLf + ")"
                strSql += vbCrLf + "VALUES("
                strSql += vbCrLf + "'" & GetNewSno(TranSnoType.TAXTRANCODE, tran) & "'" ''SNO
                strSql += vbCrLf + ",'" & PartyContra & "'"
                strSql += vbCrLf + ",'" & Partycode & "'"
                strSql += vbCrLf + "," & TranNo & "" 'TRANNO
                strSql += vbCrLf + ",'" & dtpDate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                strSql += vbCrLf + ",'" & payMode & "'"
                strSql += vbCrLf + ",'" & BatchNo & "'" 'BATCHNO
                strSql += vbCrLf + ",'SG'" 'TAXID
                strSql += vbCrLf + "," & _Amt & "" 'AMOUNT
                strSql += vbCrLf + "," & SGSTPer & "" 'TAXPER
                strSql += vbCrLf + "," & SGSTAmt
                strSql += vbCrLf + ",1" 'TSNO
                strSql += vbCrLf + ",'" & CostId & "'" 'COSTID
                strSql += vbCrLf + ",'" & strCompanyId & "'" 'COMPANYID
                strSql += vbCrLf + ")"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, CostId)
            End If
            If CGSTAmt <> 0 Then
                strSql = " INSERT INTO " & cnStockDb & "..TAXTRAN"
                strSql += vbCrLf + "("
                strSql += vbCrLf + "SNO,ACCODE,CONTRA,TRANNO,TRANDATE,TRANTYPE,BATCHNO,TAXID"
                strSql += vbCrLf + ",AMOUNT,TAXPER,TAXAMOUNT,TSNO,COSTID,COMPANYID"
                strSql += vbCrLf + ")"
                strSql += vbCrLf + "VALUES("
                strSql += vbCrLf + "'" & GetNewSno(TranSnoType.TAXTRANCODE, tran) & "'" ''SNO
                strSql += vbCrLf + ",'" & PartyContra & "'"
                strSql += vbCrLf + ",'" & Partycode & "'"
                strSql += vbCrLf + "," & TranNo & "" 'TRANNO
                strSql += vbCrLf + ",'" & dtpDate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                strSql += vbCrLf + ",'" & payMode & "'"
                strSql += vbCrLf + ",'" & BatchNo & "'" 'BATCHNO
                strSql += vbCrLf + ",'CG'" 'TAXID
                strSql += vbCrLf + "," & _Amt & "" 'AMOUNT
                strSql += vbCrLf + "," & CGSTPer & "" 'TAXPER
                strSql += vbCrLf + "," & CGSTAmt
                strSql += vbCrLf + ",2" 'TSNO
                strSql += vbCrLf + ",'" & CostId & "'" 'COSTID
                strSql += vbCrLf + ",'" & strCompanyId & "'" 'COMPANYID
                strSql += vbCrLf + ")"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, CostId)
            End If
            If IGSTAmt <> 0 Then
                strSql = " INSERT INTO " & cnStockDb & "..TAXTRAN"
                strSql += vbCrLf + "("
                strSql += vbCrLf + "SNO,ACCODE,CONTRA,TRANNO,TRANDATE,TRANTYPE,BATCHNO,TAXID"
                strSql += vbCrLf + ",AMOUNT,TAXPER,TAXAMOUNT,TSNO,COSTID,COMPANYID"
                strSql += vbCrLf + ")"
                strSql += vbCrLf + "VALUES("
                strSql += vbCrLf + "'" & GetNewSno(TranSnoType.TAXTRANCODE, tran) & "'" ''SNO
                strSql += vbCrLf + ",'" & PartyContra & "'"
                strSql += vbCrLf + ",'" & Partycode & "'"
                strSql += vbCrLf + "," & TranNo & "" 'TRANNO
                strSql += vbCrLf + ",'" & dtpDate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                strSql += vbCrLf + ",'" & payMode & "'"
                strSql += vbCrLf + ",'" & BatchNo & "'" 'BATCHNO
                strSql += vbCrLf + ",'IG'" 'TAXID
                strSql += vbCrLf + "," & _Amt & "" 'AMOUNT
                strSql += vbCrLf + "," & IGSTPer & "" 'TAXPER
                strSql += vbCrLf + "," & IGSTAmt
                strSql += vbCrLf + ",3" 'TSNO
                strSql += vbCrLf + ",'" & CostId & "'" 'COSTID
                strSql += vbCrLf + ",'" & strCompanyId & "'" 'COMPANYID
                strSql += vbCrLf + ")"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, CostId)
            End If
            If SrvtAmt <> 0 Then
                strSql = " INSERT INTO " & cnStockDb & "..TAXTRAN"
                strSql += vbCrLf + "("
                strSql += vbCrLf + "SNO,ACCODE,CONTRA,TRANNO,TRANDATE,TRANTYPE,BATCHNO,TAXID"
                strSql += vbCrLf + ",AMOUNT,TAXPER,TAXAMOUNT,TAXTYPE,TSNO,COSTID,COMPANYID"
                strSql += vbCrLf + ")"
                strSql += vbCrLf + "VALUES("
                strSql += vbCrLf + "'" & GetNewSno(TranSnoType.TAXTRANCODE, tran) & "'" ''SNO
                strSql += vbCrLf + ",'" & Partycode & "'"
                strSql += vbCrLf + ",'" & Partycode & "'"
                strSql += vbCrLf + "," & TranNo & "" 'TRANNO
                strSql += vbCrLf + ",'" & dtpDate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                strSql += vbCrLf + ",'" & payMode & "'"
                strSql += vbCrLf + ",'" & BatchNo & "'" 'BATCHNO
                strSql += vbCrLf + ",'" & SRVTID & "'" 'TAXID
                If SRVTPER = 0 Then
                    strSql += vbCrLf + ",0" 'AMOUNT
                Else
                    strSql += vbCrLf + "," & Math.Round(SrvtAmt / (SRVTPER / 100), 2) & "" 'AMOUNT
                End If
                strSql += vbCrLf + "," & SRVTPER & "" 'TAXPER
                strSql += vbCrLf + "," & SrvtAmt
                strSql += vbCrLf + ",'ST'"
                strSql += vbCrLf + ",1" 'TSNO
                strSql += vbCrLf + ",'" & CostId & "'" 'COSTID
                strSql += vbCrLf + ",'" & strCompanyId & "'" 'COMPANYID
                strSql += vbCrLf + ")"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, CostId)
            End If

            If TdsAmt <> 0 Then
                strSql = " INSERT INTO " & cnStockDb & "..TAXTRAN"
                strSql += vbCrLf + "("
                strSql += vbCrLf + "SNO,ACCODE,CONTRA,TRANNO,TRANDATE,TRANTYPE,BATCHNO,TAXID"
                strSql += vbCrLf + ",AMOUNT,TAXPER,TAXAMOUNT,TAXTYPE,TSNO,COSTID,COMPANYID"
                strSql += vbCrLf + ")"
                strSql += vbCrLf + "VALUES("
                strSql += vbCrLf + "'" & GetNewSno(TranSnoType.TAXTRANCODE, tran) & "'" ''SNO
                strSql += vbCrLf + ",'" & Partycode & "'"
                strSql += vbCrLf + ",'" & Partycode & "'"
                strSql += vbCrLf + "," & TranNo & "" 'TRANNO
                strSql += vbCrLf + ",'" & dtpDate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                strSql += vbCrLf + ",'" & payMode & "'"
                strSql += vbCrLf + ",'" & BatchNo & "'" 'BATCHNO
                strSql += vbCrLf + ",'" & MTdsCatId & "'" 'TAXID
                If MTdsPer = 0 Then
                    strSql += vbCrLf + ",0" ' AMOUNT
                Else
                    strSql += vbCrLf + "," & Math.Round(TdsAmt / (MTdsPer / 100), 2) & "" 'AMOUNT
                End If
                strSql += vbCrLf + "," & MTdsPer & "" 'TAXPER
                strSql += vbCrLf + "," & TdsAmt
                strSql += vbCrLf + ",'TD'"
                strSql += vbCrLf + ",1" 'TSNO
                strSql += vbCrLf + ",'" & CostId & "'" 'COSTID
                strSql += vbCrLf + ",'" & strCompanyId & "'" 'COMPANYID
                strSql += vbCrLf + ")"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, CostId)
            End If


            If SingleMulty = Nothing Then
                If editFlag Then
                    strSql = " UPDATE " & cnStockDb & ".." & IIf(Approval, "TACCTRAN", "ACCTRAN") & " SET CONTRA  = (SELECT TOP 1 ACCODE FROM " & cnStockDb & ".." & IIf(Approval, "TACCTRAN", "ACCTRAN") & " WHERE BATCHNO = T.BATCHNO AND TRANDATE = T.TRANDATE AND TRANMODE = 'D' AND ACCODE <> T.ACCODE AND COMPANYID = '" & strCompanyId & "' ORDER BY SNO)"
                    strSql += vbCrLf + "FROM " & cnStockDb & ".." & IIf(Approval, "TACCTRAN", "ACCTRAN") & " AS T WHERE BATCHNO = '" & BatchNo & "' AND TRANDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' AND TRANMODE = 'C' AND COMPANYID = '" & strCompanyId & "' AND ISNULL(CONTRA,'') = ''"
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, CostId)

                    strSql = " UPDATE " & cnStockDb & ".." & IIf(Approval, "TACCTRAN", "ACCTRAN") & " SET CONTRA  = (SELECT TOP 1 ACCODE FROM " & cnStockDb & ".." & IIf(Approval, "TACCTRAN", "ACCTRAN") & " WHERE BATCHNO = T.BATCHNO AND TRANDATE = T.TRANDATE AND TRANMODE = 'C' AND ACCODE <> T.ACCODE AND COMPANYID = '" & strCompanyId & "' ORDER BY SNO)"
                    strSql += vbCrLf + "FROM " & cnStockDb & ".." & IIf(Approval, "TACCTRAN", "ACCTRAN") & " AS T WHERE BATCHNO = '" & BatchNo & "' AND TRANDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' AND TRANMODE = 'D' AND COMPANYID = '" & strCompanyId & "' AND ISNULL(CONTRA,'') = ''"
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, CostId)
                Else
                    strSql = " UPDATE " & cnStockDb & ".." & mtempacctran & " SET CONTRA  = (SELECT TOP 1 ACCODE FROM " & cnStockDb & ".." & mtempacctran & " WHERE BATCHNO = T.BATCHNO AND TRANDATE = T.TRANDATE AND TRANMODE = 'D' AND ACCODE <> T.ACCODE AND COMPANYID = '" & strCompanyId & "' ORDER BY SNO)"
                    strSql += vbCrLf + "FROM " & cnStockDb & ".." & mtempacctran & " AS T WHERE BATCHNO = '" & BatchNo & "' AND TRANDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' AND TRANMODE = 'C' AND COMPANYID = '" & strCompanyId & "' AND ISNULL(CONTRA,'') = ''"
                    'ExecQuery(SyncMode.Transaction, strSql, cn, tran, CostId)
                    cmd = New OleDbCommand(strSql, cn, tran)
                    cmd.ExecuteNonQuery()

                    strSql = " UPDATE " & cnStockDb & ".." & mtempacctran & " SET CONTRA  = (SELECT TOP 1 ACCODE FROM " & cnStockDb & ".." & mtempacctran & " WHERE BATCHNO = T.BATCHNO AND TRANDATE = T.TRANDATE AND TRANMODE = 'C' AND ACCODE <> T.ACCODE AND COMPANYID = '" & strCompanyId & "' ORDER BY SNO)"
                    strSql += vbCrLf + "FROM " & cnStockDb & ".." & mtempacctran & " AS T WHERE BATCHNO = '" & BatchNo & "' AND TRANDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' AND TRANMODE = 'D' AND COMPANYID = '" & strCompanyId & "' AND ISNULL(CONTRA,'') = ''"
                    'ExecQuery(SyncMode.Transaction, strSql, cn, tran, CostId)
                    cmd = New OleDbCommand(strSql, cn, tran)
                    cmd.ExecuteNonQuery()
                End If
            End If

            If blankcode Then
                strSql = " UPDATE " & cnStockDb & "..RECEIPT SET ACCODE   = '" & gsaccode & "' "
                strSql += vbCrLf + "WHERE BATCHNO = '" & BatchNo & "' AND TRANDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' AND COMPANYID = '" & strCompanyId & "' AND ISNULL(ACCODE,'') = ''"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, CostId)

                strSql = " UPDATE " & cnStockDb & "..ISSUE SET ACCODE   = '" & gsaccode & "' "
                strSql += vbCrLf + "WHERE BATCHNO = '" & BatchNo & "' AND TRANDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' AND COMPANYID = '" & strCompanyId & "' AND ISNULL(ACCODE,'') = ''"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, CostId)
            End If

            strSql = vbCrLf + " SELECT * FROM " & cnStockDb & ".." & mtempacctran & ""
            Dim DtTempQry1 As New DataTable
            DtTempQry1 = New DataTable
            cmd = New OleDbCommand(strSql, cn, tran)
            da = New OleDbDataAdapter(cmd)
            da.Fill(DtTempQry1)

            If editFlag = False Then
                strSql = " EXEC " & cnStockDb & "..INSERTQRYGENERATOR_TABLE "
                strSql += vbCrLf + " @DBNAME = '" & cnStockDb & "',@TABLENAME = '" & mtempacctran & "',@MASK_TABLENAME = '" & IIf(Approval, "TACCTRAN", "ACCTRAN") & "'"
                Dim DtTempQry As New DataTable
                cmd = New OleDbCommand(strSql, cn, tran)
                da = New OleDbDataAdapter(cmd)
                da.Fill(DtTempQry)
                For Each ro As DataRow In DtTempQry.Rows
                    strSql = ro.Item(0).ToString
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, CostId)
                    strSql = " "
                Next
            Else
                If DtTempQry1.Rows.Count = 0 Then
                    strSql = " INSERT INTO " & cnStockDb & ".." & mtempacctran & ""
                    strSql += vbCrLf + "("
                    strSql += vbCrLf + "SNO,TRANNO,TRANDATE,TRANMODE,ACCODE,SACCODE"
                    strSql += vbCrLf + ",AMOUNT,PCS,GRSWT,NETWT,REFNO,REFDATE,PAYMODE,CHQCARDNO"
                    strSql += vbCrLf + ",CARDID,CHQCARDREF,CHQDATE,BRSFLAG,RELIASEDATE,FROMFLAG,REMARK1,REMARK2"
                    strSql += vbCrLf + ",CONTRA,BATCHNO,USERID,UPDATED,UPTIME,SYSTEMID,CASHID,COSTID,SCOSTID"
                    strSql += vbCrLf + ",APPVER,COMPANYID,WT_ENTORDER,TDSCATID,TDSPER,TDSAMOUNT,FLAG"
                    strSql += vbCrLf + ")"
                    strSql += vbCrLf + "SELECT SNO,TRANNO,TRANDATE,TRANMODE,ACCODE,SACCODE"
                    strSql += vbCrLf + ",AMOUNT,PCS,GRSWT,NETWT,REFNO,REFDATE,PAYMODE,CHQCARDNO"
                    strSql += vbCrLf + ",CARDID,CHQCARDREF,CHQDATE,BRSFLAG,RELIASEDATE,FROMFLAG,REMARK1,REMARK2"
                    strSql += vbCrLf + ",CONTRA,BATCHNO,USERID,UPDATED,UPTIME,SYSTEMID,CASHID,COSTID,SCOSTID"
                    strSql += vbCrLf + ",APPVER,COMPANYID,WT_ENTORDER,TDSCATID,TDSPER,TDSAMOUNT,FLAG"
                    strSql += vbCrLf + "  FROM " & cnStockDb & "..ACCTRAN AS T WHERE BATCHNO = '" & BatchNo & "' "
                    cmd = New OleDbCommand(strSql, cn, tran)
                    cmd.ExecuteNonQuery()
                End If

                strSql = " EXEC " & cnStockDb & "..INSERTQRYGENERATOR_TABLE "
                strSql += vbCrLf + " @DBNAME = '" & cnStockDb & "',@TABLENAME = '" & mtempacctran & "',@MASK_TABLENAME = '" & IIf(Approval, "TACCTRAN", "ACCTRAN") & "'"
                Dim DtTempQry As New DataTable
                cmd = New OleDbCommand(strSql, cn, tran)
                da = New OleDbDataAdapter(cmd)
                da.Fill(DtTempQry)
                For Each ro As DataRow In DtTempQry.Rows
                    strSql = ro.Item(0).ToString
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, CostId, , , , False)
                    strSql = " "
                Next
            End If




            Dim debCrNotTally As String = ""
            Dim balAmt As Double = Val(objGPack.GetSqlValue("SELECT SUM(CASE WHEN TRANMODE = 'C' THEN isnull(AMOUNT,0) ELSE isnull(AMOUNT,0)*(-1) END) AS BALANCE FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = '" & BatchNo & "'", , "0", tran))
            '            If GetAdmindbSoftValue("CHECKTRAN", "Y", tran) = "Y" Then
            If Math.Abs(balAmt) > 0 Then
                tran.Rollback()
                tran.Dispose()
                tran = Nothing
                MsgBox("Credit Debit Not Tally : " + balAmt.ToString, MsgBoxStyle.Information)
                gridView_OWN.CurrentCell = gridView_OWN.CurrentRow.Cells(0)
                gridView_OWN.Select()
                Exit Sub
            End If
            'End If
            strSql = " IF (SELECT COUNT(*) FROM " & cnStockDb & "..SYSOBJECTS WHERE NAME = '" & mtempacctran & "')> 0"
            strSql += vbCrLf + "DROP TABLE " & cnStockDb & ".." & mtempacctran
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()
            tran.Commit()
            tran = Nothing

            Dim vt As String
            If vouchType = VoucherType.Receipt Then
                vt = "CR"
            ElseIf vouchType = VoucherType.Payment Then
                vt = "CP"
            ElseIf vouchType = VoucherType.Journal Then
                vt = "JE"
            ElseIf vouchType = VoucherType.DebitNote Then
                vt = "DN"
            ElseIf vouchType = VoucherType.CreditNote Then
                vt = "CN"
            End If

            If editFlag Then
                MsgBox(TranNo & " Updated..")
                Me.DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()
                Exit Sub
            Else
                Dim result As String
                result = "TranType   :" + vt.ToString + vbCrLf
                result += "TranNo    :" + TranNo.ToString + vbCrLf
                result += "Generated.." + vbCrLf + vbCrLf
                MsgBox(result)
                'MsgBox(TranNo & " Generated..")
                SendmailToInternalTrfAccount(dtpDate.Value.ToString("yyyy-MM-dd"), BatchNo)
                If SMS_MSG_PAYMENT <> "" Then
                    SendsmsToDealer(dtpDate.Value.ToString("yyyy-MM-dd"), BatchNo, SMS_MSG_PAYMENT)
                End If
            End If
            Dim pBatchno As String = BatchNo
            Dim pBillDate As Date = dtpDate.Value
            Dim pParamStr As String = ""
            btnNew_Click(Me, New EventArgs)

            Dim prnmemsuffix As String = ""
            If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId
            Dim BillPrintExe As Boolean = IIf(GetAdmindbSoftValue("CALLBILLPRINTEXE", "Y") = "Y", True, False)
            Dim BillPrint_Format As String = GetAdmindbSoftValue("BILLPRINT_FORMAT", "")
            If GST And BillPrint_Format = "M1" Then
                Dim obj As New BrighttechREPORT.frmBillPrintDocA4N("ACC", pBatchno, pBillDate.ToString("yyyy-MM-dd"), "")
                ''Dim obj As New BrighttechREPORT.frmCashTransactionPrint(pPayMode, saledbName, pBillDate.Date, ptranno, pBatchno, "ACCTRAN", False, True, "OUTSTANDING", "", True, "")
            ElseIf GST And BillPrint_Format = "M2" Then
                Dim obj As New BrighttechREPORT.frmBillPrintDocB5("ACC", pBatchno, pBillDate.ToString("yyyy-MM-dd"), "")
            ElseIf GST And BillPrint_Format = "M3" Then
                Dim obj As New BrighttechREPORT.frmBillPrintDocA5("ACC", pBatchno, pBillDate.ToString("yyyy-MM-dd"), "")
            ElseIf GST And BillPrint_Format = "M4" Then
                Dim obj As New BrighttechREPORT.frmBillPrintDocB52cpy("ACC", pBatchno, pBillDate.ToString("yyyy-MM-dd"), "")
            ElseIf GST And BillPrintExe = False Then
                Dim billDoc As New frmBillPrintDoc("POS", pBatchno, pBillDate.ToString("yyyy-MM-dd"), "")
            Else
                If IO.File.Exists(Application.StartupPath & "\BillPrint.exe") Then
                    Dim write As IO.StreamWriter
                    ''Dim prnmemsuffix As String = ""
                    ''If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId
                    Dim memfile As String = "\BillPrint" + prnmemsuffix.Trim & ".mem"

                    write = IO.File.CreateText(Application.StartupPath & memfile)
                    write.WriteLine(LSet("TYPE", 15) & ":ACC")
                    pParamStr += LSet("TYPE", 15) & ":ACC" & ";"
                    write.WriteLine(LSet("BATCHNO", 15) & ":" & pBatchno)
                    pParamStr += LSet("BATCHNO", 15) & ":" & pBatchno & ";"
                    write.WriteLine(LSet("TRANDATE", 15) & ":" & pBillDate.ToString("yyyy-MM-dd"))
                    pParamStr += LSet("TRANDATE", 15) & ":" & pBillDate.ToString("yyyy-MM-dd") & ";"
                    write.WriteLine(LSet("DUPLICATE", 15) & ":N")
                    pParamStr += LSet("DUPLICATE", 15) & ":N"
                    write.Flush()
                    write.Close()
                    If EXE_WITH_PARAM = False Then
                        System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe")
                    Else
                        System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe", pParamStr)
                    End If
                End If
            End If
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        Finally
            If Not tran Is Nothing Then tran.Dispose()
        End Try
    End Sub
    Private Function SendsmsToDealer(ByVal trandate As String, ByVal BatchNo As String, ByVal Msg As String)
        Dim dtSms As New DataTable
        strSql = vbCrLf + "SELECT AH.ACNAME,AH.ACCODE,AC.COSTID,AH.MOBILE"
        strSql += vbCrLf + ",CONVERT(VARCHAR,AC.TRANDATE,105)TRANDATE,AC.TRANNO"
        strSql += vbCrLf + ",CASE WHEN AC.TRANMODE='C'THEN 'CR'ELSE 'DR' END TRANMODE,AC.AMOUNT,AC.CANCEL "
        strSql += vbCrLf + ",AC.CHQCARDNO,CONVERT(VARCHAR,AC.CHQDATE,105)CHQDATE"
        strSql += vbCrLf + "FROM " & cnStockDb & "..ACCTRAN AC "
        strSql += vbCrLf + "INNER JOIN  " & cnAdminDb & "..ACHEAD AS AH ON AC.ACCODE=AH.ACCODE "
        strSql += vbCrLf + "AND AH.ACTYPE IN('D','S') "
        strSql += vbCrLf + "WHERE AC.TRANDATE='" & dtpDate.Value.ToString("yyyy-MM-dd") & "' "
        strSql += vbCrLf + "AND AC.BATCHNO='" & BatchNo & "' "
        strSql += vbCrLf + "AND ISNULL(AH.MOBILE,'')<>''"
        strSql += vbCrLf + "AND PAYMODE='CP'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtSms)
        If dtSms.Rows.Count > 0 Then
            For cnt As Integer = 0 To dtSms.Rows.Count - 1
                With dtSms.Rows(cnt)
                    If .Item("TRANMODE").ToString = "CR" Then Continue For
                    Dim TempMsg As String
                    TempMsg = Msg
                    TempMsg = Replace(Msg, vbCrLf, "")
                    TempMsg = Replace(TempMsg, "<NAME>", .Item("ACNAME").ToString)
                    TempMsg = Replace(TempMsg, "<AMOUNT>", Val(.Item("AMOUNT").ToString))
                    TempMsg = Replace(TempMsg, "<CHQNO>", .Item("CHQCARDNO").ToString)
                    TempMsg = Replace(TempMsg, "<CHQDATE>", .Item("CHQDATE").ToString)
                    SmsSend(TempMsg, .Item("MOBILE").ToString)
                End With
            Next
        End If
    End Function

    Private Sub InsertIntoAccTran _
  (ByVal EntryOrder As Integer, ByVal tNo As Integer,
  ByVal tranMode As String,
  ByVal accode As String,
  ByVal amount As Double,
  ByVal pcs As Integer,
  ByVal grsWT As Double,
  ByVal netWT As Double,
  ByVal payMode As String,
  ByVal contra As String,
  ByVal TDSCATID As Integer,
  ByVal TDSPER As Decimal,
  ByVal TDSAMOUNT As Decimal,
  Optional ByVal refNo As String = Nothing, Optional ByVal refDate As String = Nothing,
  Optional ByVal chqCardNo As String = Nothing,
  Optional ByVal chqDate As String = Nothing,
  Optional ByVal chqCardId As Integer = Nothing,
  Optional ByVal chqCardRef As String = Nothing,
  Optional ByVal Remark1 As String = Nothing,
  Optional ByVal Remark2 As String = Nothing,
  Optional ByVal fLAG As String = Nothing,
  Optional ByVal SAccode As String = "",
  Optional ByVal SCostid As String = ""
  )
        If amount = 0 Then Exit Sub

        If editFlag Then
            BL_Acc.Insertacctran(tran, True, IIf(Approval, "TACCTRAN", "ACCTRAN"), GetNewSno(IIf(Approval, TranSnoType.TACCTRANCODE, TranSnoType.ACCTRANCODE), tran) _
            , tNo, dtpDate.Value.ToString("yyyy-MM-dd"), tranMode, accode, SAccode, Math.Abs(amount), 0, Math.Abs(pcs), Math.Abs(grsWT) _
            , Math.Abs(netWT), payMode, contra _
            , BatchNo, , CostId, SCostid, , , refNo, IIf((refDate = Nothing Or refDate = "12:00:00 AM"), "NULL", IIf((refDate = Nothing Or refDate = "12:00:00 AM"), "NULL", refDate)), chqCardNo _
            , IIf((chqDate = Nothing), "NULL", chqDate) _
            , chqCardId, chqCardRef, Mid(Remark1, 1, 100), Mid(Remark2, 1, 100), EntryOrder, TDSCATID, TDSPER, TDSAMOUNT, fLAG, PCODE)
        Else
            BL_Acc.Insertacctran(tran, False, mtempacctran, GetNewSno(IIf(Approval, TranSnoType.TACCTRANCODE, TranSnoType.ACCTRANCODE), tran) _
            , tNo, dtpDate.Value.ToString("yyyy-MM-dd"), tranMode, accode, SAccode, Math.Abs(amount), 0, Math.Abs(pcs), Math.Abs(grsWT) _
            , Math.Abs(netWT), payMode, contra _
            , BatchNo, , CostId, SCostid, , , refNo, IIf((refDate = Nothing Or refDate = "12:00:00 AM"), "NULL", refDate), chqCardNo _
            , IIf((chqDate = Nothing), "NULL", chqDate) _
            , chqCardId, chqCardRef, Mid(Remark1, 1, 100), Mid(Remark2, 1, 100), EntryOrder, TDSCATID, TDSPER, TDSAMOUNT, fLAG, PCODE)
        End If
        Exit Sub
        If editFlag Then
            strSql = " INSERT INTO " & cnStockDb & ".." & IIf(Approval, "TACCTRAN", "ACCTRAN") & ""
        Else
            strSql = " INSERT INTO " & cnStockDb & ".." & mtempacctran & ""
        End If
        strSql += vbCrLf + "("
        strSql += vbCrLf + "SNO,TRANNO,TRANDATE,TRANMODE,ACCODE,SACCODE"
        strSql += vbCrLf + ",AMOUNT,PCS,GRSWT,NETWT,REFNO,REFDATE,PAYMODE,CHQCARDNO"
        strSql += vbCrLf + ",CARDID,CHQCARDREF,CHQDATE,BRSFLAG,RELIASEDATE,FROMFLAG,REMARK1,REMARK2"
        strSql += vbCrLf + ",CONTRA,BATCHNO,USERID,UPDATED,UPTIME,SYSTEMID,CASHID,COSTID,SCOSTID"
        strSql += vbCrLf + ",APPVER,COMPANYID,WT_ENTORDER,TDSCATID,TDSPER,TDSAMOUNT,FLAG,PCODE"
        strSql += vbCrLf + ")"
        strSql += vbCrLf + "VALUES"
        strSql += vbCrLf + "("
        strSql += vbCrLf + "'" & GetNewSno(IIf(Approval, TranSnoType.TACCTRANCODE, TranSnoType.ACCTRANCODE), tran) & "'" ''SNO
        strSql += vbCrLf + "," & tNo & "" 'TRANNO 
        strSql += vbCrLf + ",'" & dtpDate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
        strSql += vbCrLf + ",'" & tranMode & "'" 'TRANMODE
        strSql += vbCrLf + ",'" & accode & "'" 'ACCODE
        strSql += vbCrLf + ",'" & SAccode & "'" 'ACCODE
        strSql += vbCrLf + "," & Math.Abs(amount) & "" 'AMOUNT
        strSql += vbCrLf + "," & Math.Abs(pcs) & "" 'PCS
        strSql += vbCrLf + "," & Math.Abs(grsWT) & "" 'GRSWT
        strSql += vbCrLf + "," & Math.Abs(netWT) & "" 'NETWT
        strSql += vbCrLf + ",'" & refNo & "'" 'REFNO
        If refDate = Nothing Or refDate = "12:00:00 AM" Then
            strSql += vbCrLf + ",NULL" 'REFDATE
        Else
            strSql += vbCrLf + ",'" & refDate & "'" 'REFDATE
        End If
        strSql += vbCrLf + ",'" & payMode & "'" 'PAYMODE
        strSql += vbCrLf + ",'" & chqCardNo & "'" 'CHQCARDNO
        strSql += vbCrLf + "," & chqCardId & "" 'CARDID
        strSql += vbCrLf + ",'" & chqCardRef & "'" 'CHQCARDREF
        If chqDate = Nothing Then
            strSql += vbCrLf + ",NULL" 'CHQDATE
        Else
            strSql += vbCrLf + ",'" & chqDate & "'" 'CHQDATE
        End If
        strSql += vbCrLf + ",''" 'BRSFLAG
        strSql += vbCrLf + ",NULL" 'RELIASEDATE
        strSql += vbCrLf + ",'A'" 'FROMFLAG
        strSql += vbCrLf + ",'" & Mid(Remark1, 1, 100) & "'" 'REMARK1
        strSql += vbCrLf + ",'" & Mid(Remark2, 1, 100) & "'" 'REMARK2
        strSql += vbCrLf + ",'" & contra & "'" 'CONTRA
        strSql += vbCrLf + ",'" & BatchNo & "'" 'BATCHNO
        strSql += vbCrLf + ",'" & userId & "'" 'USERID
        strSql += vbCrLf + ",'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        strSql += vbCrLf + ",'" & Date.Now.ToLongTimeString & "'" 'UPTIME
        strSql += vbCrLf + ",'" & systemId & "'" 'SYSTEMID
        strSql += vbCrLf + ",''" 'CASHID
        strSql += vbCrLf + ",'" & CostId & "'" 'COSTID
        strSql += vbCrLf + ",'" & SCostid & "'" 'FLAG
        strSql += vbCrLf + ",'" & VERSION & "'" 'APPVER
        strSql += vbCrLf + ",'" & strCompanyId & "'" 'COMPANYID
        strSql += vbCrLf + "," & EntryOrder & "" 'WT_ENTORDER
        strSql += vbCrLf + "," & TDSCATID & "" 'TDSCATID
        strSql += vbCrLf + "," & TDSPER & "" 'TDSPER
        strSql += vbCrLf + "," & TDSAMOUNT & "" 'TDSAMOUNT
        strSql += vbCrLf + ",'" & fLAG & "'" 'FLAG
        strSql += vbCrLf + ",'" & PCODE & "'" 'PCODE FOR VBJ ACC ENTRY
        strSql += vbCrLf + ")"
        If editFlag Then
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, CostId)
        Else
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()
        End If
        strSql = ""
        cmd = Nothing
    End Sub



    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        btnExit_Click(Me, New EventArgs)
    End Sub

    Public Sub InsertIntoIssueReceipt(
   ByVal tableName As String _
   , ByVal tranType As String, ByVal CATCODE As String, ByVal pcs As Integer, ByVal grsWt As Double, ByVal netWT As Double, ByVal PureWT As Double _
   , ByVal wast As Double _
   , ByVal mc As Double, ByVal rate As Double, ByVal amount As Double _
   , ByVal vat As Double, ByVal stnAmt As Double, ByVal miscAmt As Double, ByVal empId As Integer _
   , ByVal grsNet As String, ByVal wUnit As String, ByVal accode As String, ByVal Wt_EntOrder As Integer,
   Optional ByVal refno As String = Nothing, Optional ByVal refdate As Date = Nothing)
        Dim issSno As String

        If UCase(tableName) = "ISSUE" Then
            issSno = GetNewSno(IIf(Approval, TranSnoType.TISSUECODE, TranSnoType.ISSUECODE), tran)
        Else
            issSno = GetNewSno(IIf(Approval, TranSnoType.TRECEIPTCODE, TranSnoType.RECEIPTCODE), tran)
        End If
        Dim TNo As Integer = TranNo ' BillNo(TYPE, CATCODE)
        If UCase(tableName) = "ISSUE" Then
            strSql = " INSERT INTO " & cnStockDb & ".." & IIf(Approval, "TISSUE", "ISSUE") & ""
        Else
            strSql = " INSERT INTO " & cnStockDb & ".." & IIf(Approval, "TRECEIPT", "RECEIPT") & ""
        End If
        strSql += vbCrLf + "("
        strSql += vbCrLf + "SNO,TRANNO,TRANDATE,TRANTYPE,PCS"
        strSql += vbCrLf + ",GRSWT,NETWT,LESSWT,PUREWT"
        strSql += vbCrLf + ",TAGNO,ITEMID,SUBITEMID,WASTPER"
        strSql += vbCrLf + ",WASTAGE,MCGRM,MCHARGE,AMOUNT"
        strSql += vbCrLf + ",RATE,BOARDRATE,SALEMODE,GRSNET"
        strSql += vbCrLf + ",TRANSTATUS,REFNO,REFDATE,COSTID"
        strSql += vbCrLf + ",COMPANYID,FLAG,EMPID,TAGPCS,TAGGRSWT"
        strSql += vbCrLf + ",TAGNETWT,TAGRATEID,TAGSVALUE,TAGDESIGNER"
        strSql += vbCrLf + ",ITEMCTRID,ITEMTYPEID,PURITY,TABLECODE"
        strSql += vbCrLf + ",INCENTIVE,WEIGHTUNIT,CATCODE,OCATCODE"
        strSql += vbCrLf + ",ACCODE,ALLOY,BATCHNO,REMARK1"
        strSql += vbCrLf + ",REMARK2,USERID,UPDATED,UPTIME,SYSTEMID,DISCOUNT"
        strSql += vbCrLf + ",RUNNO,CASHID,TAX,STNAMT,MISCAMT,METALID,STONEUNIT,APPVER,WT_ENTORDER)"
        strSql += vbCrLf + "VALUES("
        strSql += vbCrLf + "'" & issSno & "'" ''SNO
        strSql += vbCrLf + "," & TNo & "" 'TRANNO
        strSql += vbCrLf + ",'" & dtpDate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
        strSql += vbCrLf + ",'" & tranType & "'" 'TRANTYPE
        strSql += vbCrLf + "," & pcs & "" 'PCS
        strSql += vbCrLf + "," & grsWt & "" 'GRSWT
        strSql += vbCrLf + "," & netWT & "" 'NETWT
        strSql += vbCrLf + ",0" 'LESSWT
        strSql += vbCrLf + "," & PureWT & "" 'PUREWT
        strSql += vbCrLf + ",''" 'TAGNO
        strSql += vbCrLf + ",0" 'ITEMID
        strSql += vbCrLf + ",0" 'SUBITEMID
        strSql += vbCrLf + ",0" 'WASTPER
        strSql += vbCrLf + "," & wast & "" 'WASTAGE
        strSql += vbCrLf + ",0" 'MCGRM
        strSql += vbCrLf + "," & mc & "" 'MCHARGE
        strSql += vbCrLf + "," & amount & "" 'AMOUNT
        strSql += vbCrLf + "," & rate & "" 'RATE
        strSql += vbCrLf + "," & GetRate(dtpDate.Value, CATCODE, tran) & "" 'BOARDRATE
        strSql += vbCrLf + ",''" 'SALEMODE
        strSql += vbCrLf + ",'" & Mid(grsNet, 1, 1) & "'" 'GRSNET
        strSql += vbCrLf + ",''" 'TRANSTATUS ''
        strSql += vbCrLf + ",'" & refno & "'" 'REFNO ''
        strSql += vbCrLf + ",'" & refdate & "'" 'REFDATE NULL
        strSql += vbCrLf + ",'" & CostId & "'" 'COSTID 
        strSql += vbCrLf + ",'" & strCompanyId & "'" 'COMPANYID
        strSql += vbCrLf + ",''" 'FLAG 
        strSql += vbCrLf + "," & empId & "" 'EMPID
        strSql += vbCrLf + ",0" 'TAGPCS
        strSql += vbCrLf + ",0" 'TAGGRSWT
        strSql += vbCrLf + ",0" 'TAGNETWT
        strSql += vbCrLf + ",0" 'TAGRATEID
        strSql += vbCrLf + ",0" 'TAGSVALUE
        strSql += vbCrLf + ",'0'" 'TAGDESIGNER
        strSql += vbCrLf + ",0" 'ITEMCTRID
        strSql += vbCrLf + ",0" 'ITEMTYPEID
        strSql += vbCrLf + "," & Val(objGPack.GetSqlValue("SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = (SELECT PURITYID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = '" & CATCODE & "')", , , tran)) & "" 'PURITY
        strSql += vbCrLf + ",''" 'TABLECODE
        strSql += vbCrLf + ",''" 'INCENTIVE
        strSql += vbCrLf + ",'" & Mid(wUnit, 1, 1) & "'" 'WEIGHTUNIT
        strSql += vbCrLf + ",'" & CATCODE & "'" 'CATCODE
        strSql += vbCrLf + ",'" & CATCODE & "'"
        strSql += vbCrLf + ",'" & accode & "'" 'ACCODE
        strSql += vbCrLf + ",0" 'ALLOY
        strSql += vbCrLf + ",'" & BatchNo & "'" 'BATCHNO
        strSql += vbCrLf + ",''" 'REMARK1
        strSql += vbCrLf + ",''" 'REMARK2
        strSql += vbCrLf + ",'" & userId & "'" 'USERID
        strSql += vbCrLf + ",'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        strSql += vbCrLf + ",'" & Date.Now.ToLongTimeString & "'" 'UPTIME
        strSql += vbCrLf + ",'" & systemId & "'" 'SYSTEMID
        strSql += vbCrLf + ",0" 'DISCOUNT
        strSql += vbCrLf + ",''" 'RUNNO
        strSql += vbCrLf + ",''" 'CASHID
        strSql += vbCrLf + "," & vat & "" 'TAX
        strSql += vbCrLf + "," & stnAmt & "" 'STONEAMT
        strSql += vbCrLf + "," & miscAmt & "" 'MISCAMT
        strSql += vbCrLf + ",'" & objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = '" & CATCODE & "'", , , tran) & "'" 'MTALID
        strSql += vbCrLf + ",''" 'STONEUNIT
        strSql += vbCrLf + ",'" & VERSION & "'" 'APPVER
        strSql += vbCrLf + "," & Wt_EntOrder & "" 'Wt_EntOrder
        strSql += vbCrLf + ")"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, CostId)
    End Sub

    '''''''''''''''''''''''''''*********************************'''''''''''''''''''''
    '''''''''''''''''''''''''''*********************************'''''''''''''''''''''
    '''''''''''''''''''''''''''*********************************'''''''''''''''''''''
    '''''''''''''''''''''''''''*********************************'''''''''''''''''''''
#Region " Constants "
    Private Enum DGVHeaderImageAlignments As Int32
        [Default] = 0
        FillCell = 1
        SingleCentered = 2
        SingleLeft = 3
        SingleRight = 4
        Stretch = [Default]
        Tile = 5
    End Enum
#End Region

#Region " Events "
#Region " Handlers "

    Private Sub dgvData_CellPainting(ByVal sender As Object,
    ByVal e As DataGridViewCellPaintingEventArgs) _
    Handles gridView_OWN.CellPainting
        ' Only the Header Row (which Index is -1) is to be affected.
        If e.RowIndex = -1 Then
            GridDrawCustomHeaderColumns(gridView_OWN, e,
             My.Resources.Button_Gray_Stripe_01_050,
             DGVHeaderImageAlignments.Stretch)
            'GridDrawCustomHeaderColumns(gridView_OWN, e, _
            ' My.Resources.AquaBall_Blue, DGVHeaderImageAlignments.FillCell)
            'GridDrawCustomHeaderColumns(gridView_OWN, e, _
            '             My.Resources.AquaBall_Blue, _
            ' DGVHeaderImageAlignments.SingleCentered)
            'GridDrawCustomHeaderColumns(gridView_OWN, e, _
            ' My.Resources.AquaBall_Blue, DGVHeaderImageAlignments.SingleLeft)
            'GridDrawCustomHeaderColumns(gridView_OWN, e, _
            ' My.Resources.AquaBall_Blue, DGVHeaderImageAlignments.SingleRight)
            'GridDrawCustomHeaderColumns(gridView_OWN, e, _
            ' My.Resources.AquaBall_Blue, DGVHeaderImageAlignments.Tile)
        End If

    End Sub
#End Region
#End Region

#Region " Methods "
    Private Sub GridDrawCustomHeaderColumns(ByVal dgv As DataGridView,
     ByVal e As DataGridViewCellPaintingEventArgs, ByVal img As Image,
     ByVal Style As DGVHeaderImageAlignments)
        ' All of the graphical Processing is done here.
        Dim gr As Graphics = e.Graphics
        ' Fill the BackGround with the BackGroud Color of Headers.
        ' This step is necessary, for transparent images, or what's behind
        ' would be painted instead.
        gr.FillRectangle(
         New SolidBrush(dgv.ColumnHeadersDefaultCellStyle.BackColor),
         e.CellBounds)
        If img IsNot Nothing Then
            Select Case Style
                Case DGVHeaderImageAlignments.FillCell
                    gr.DrawImage(
                     img, e.CellBounds.X, e.CellBounds.Y,
                     e.CellBounds.Width, e.CellBounds.Height)
                Case DGVHeaderImageAlignments.SingleCentered
                    gr.DrawImage(img,
                     ((e.CellBounds.Width - img.Width) \ 2) + e.CellBounds.X,
                     ((e.CellBounds.Height - img.Height) \ 2) + e.CellBounds.Y,
                     img.Width, img.Height)
                Case DGVHeaderImageAlignments.SingleLeft
                    gr.DrawImage(img, e.CellBounds.X,
                     ((e.CellBounds.Height - img.Height) \ 2) + e.CellBounds.Y,
                     img.Width, img.Height)
                Case DGVHeaderImageAlignments.SingleRight
                    gr.DrawImage(img,
                     (e.CellBounds.Width - img.Width) + e.CellBounds.X,
                     ((e.CellBounds.Height - img.Height) \ 2) + e.CellBounds.Y,
                     img.Width, img.Height)
                Case DGVHeaderImageAlignments.Tile
                    ' ********************************************************
                    ' To correct: It sould display just a stripe of images,
                    ' long as the whole header, but centered in the header's
                    ' height.
                    ' This code WON'T WORK.
                    ' Any one got any better solution?
                    'Dim rect As New Rectangle(e.CellBounds.X, _
                    ' ((e.CellBounds.Height - img.Height) \ 2), _
                    ' e.ClipBounds.Width, _
                    ' ((e.CellBounds.Height \ 2 + img.Height \ 2)))
                    'Dim br As New TextureBrush(img, Drawing2D.WrapMode.Tile, _
                    ' rect)
                    ' ********************************************************
                    ' This one works... but poorly (the image is repeated
                    ' vertically, too).
                    Dim br As New TextureBrush(img, Drawing2D.WrapMode.Tile)
                    gr.FillRectangle(br, e.ClipBounds)
                Case Else
                    gr.DrawImage(
                     img, e.CellBounds.X, e.CellBounds.Y,
                     e.ClipBounds.Width, e.CellBounds.Height)
            End Select
        End If
        'e.PaintContent(e.CellBounds)
        If e.Value Is Nothing Then
            e.Handled = True
            Return
        End If
        Using sf As New StringFormat
            With sf
                Select Case dgv.ColumnHeadersDefaultCellStyle.Alignment
                    Case DataGridViewContentAlignment.BottomCenter
                        .Alignment = StringAlignment.Center
                        .LineAlignment = StringAlignment.Far
                    Case DataGridViewContentAlignment.BottomLeft
                        .Alignment = StringAlignment.Near
                        .LineAlignment = StringAlignment.Far
                    Case DataGridViewContentAlignment.BottomRight
                        .Alignment = StringAlignment.Far
                        .LineAlignment = StringAlignment.Far
                    Case DataGridViewContentAlignment.MiddleCenter
                        .Alignment = StringAlignment.Center
                        .LineAlignment = StringAlignment.Center
                    Case DataGridViewContentAlignment.MiddleLeft
                        .Alignment = StringAlignment.Near
                        .LineAlignment = StringAlignment.Center
                    Case DataGridViewContentAlignment.MiddleRight
                        .Alignment = StringAlignment.Far
                        .LineAlignment = StringAlignment.Center
                    Case DataGridViewContentAlignment.TopCenter
                        .Alignment = StringAlignment.Center
                        .LineAlignment = StringAlignment.Near
                    Case DataGridViewContentAlignment.TopLeft
                        .Alignment = StringAlignment.Near
                        .LineAlignment = StringAlignment.Near
                    Case DataGridViewContentAlignment.TopRight
                        .Alignment = StringAlignment.Far
                        .LineAlignment = StringAlignment.Near
                End Select
                ' This part could be handled...
                'Select Case dgv.ColumnHeadersDefaultCellStyle.WrapMode
                '	Case DataGridViewTriState.False
                '		.FormatFlags = StringFormatFlags.NoWrap
                '	Case DataGridViewTriState.NotSet
                '		.FormatFlags = StringFormatFlags.NoWrap
                '	Case DataGridViewTriState.True
                '		.FormatFlags = StringFormatFlags.FitBlackBox
                'End Select
                .HotkeyPrefix = Drawing.Text.HotkeyPrefix.None
                .Trimming = StringTrimming.None
            End With
            With dgv.ColumnHeadersDefaultCellStyle
                gr.DrawString(e.Value.ToString, .Font,
                 New SolidBrush(.ForeColor), e.CellBounds, sf)
            End With
        End Using
        e.Handled = True
    End Sub
#End Region

    Private Sub gridView_OWN_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles gridView_OWN.DataError
        MsgBox(e.Exception.Message)
    End Sub

    Private Sub RefreshAcNameToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RefreshAcNameToolStripMenuItem.Click
        strSql = " SELECT " & cnAdminDb & ".DBO.WORDCAPS(ACNAME)ACNAME,ACGRPCODE,ACTYPE FROM " & cnAdminDb & "..ACHEAD where 1=1"
        strSql += vbCrLf + "AND ISNULL(MACCODE,'') = ''"
        strSql += GetAcNameQryFilteration()
        strSql += vbCrLf + "ORDER BY ACNAME"
        dtAccNames = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtAccNames)
        LoadAcc()
    End Sub

    Private Sub gridView_OWN_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView_OWN.SelectionChanged

    End Sub

    Private Sub gridView_OWN_RowEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView_OWN.RowEnter
        'If CellSelected Then
        '    CellSelected = False
        '    Exit Sub
        'End If
        'If gridView_OWN.Rows(e.RowIndex).Cells("GENBY").Value.ToString = "" Then Exit Sub
        'If gridView_OWN.Rows(e.RowIndex).Cells("GENBY").Value.ToString = "A" Then
        '    gridView_OWN.ClearSelection()
        '    CellSelected = True
        '    gridView_OWN.Rows(e.RowIndex - 1).Selected = True
        'End If
    End Sub
    Private Sub DGV_CellPainting(ByVal sender As Object, ByVal e As DataGridViewCellPaintingEventArgs) Handles DgvSearch.CellPainting

        If e.RowIndex >= 0 And e.ColumnIndex >= 0 Then

            e.Handled = True
            e.PaintBackground(e.CellBounds, True)
            Dim sw As String = txtGrid_OWN.Text
            If Not String.IsNullOrEmpty(sw) Then
                'highlight search word

                Dim val As String = DirectCast(e.FormattedValue, String)

                Dim sindx As Integer = val.ToLower.IndexOf(sw.ToLower)
                If sindx >= 0 Then
                    'search word found

                    Dim sf As Font = New Font(e.CellStyle.Font, FontStyle.Bold)
                    Dim sbr As SolidBrush = New SolidBrush(Color.Red)

                    Dim br As SolidBrush
                    If e.State = DataGridViewElementStates.Selected Then
                        br = New SolidBrush(e.CellStyle.SelectionForeColor)
                    Else
                        br = New SolidBrush(e.CellStyle.ForeColor)
                    End If

                    Dim sBefore As String = val.Substring(0, sindx)
                    Dim sBeforeSize As SizeF = e.Graphics.MeasureString(sBefore, e.CellStyle.Font, e.CellBounds.Size)
                    Dim sWord As String = val.Substring(sindx, sw.Length)
                    Dim sWordSize As SizeF = e.Graphics.MeasureString(sWord, sf, e.CellBounds.Size)
                    Dim sAfter As String = val.Substring(sindx + sw.Length, val.Length - (sindx + sw.Length))

                    e.Graphics.DrawString(sBefore, e.CellStyle.Font, br, e.CellBounds)
                    e.Graphics.DrawString(sWord, sf, sbr, e.CellBounds.X + sBeforeSize.Width, e.CellBounds.Location.Y)
                    e.Graphics.DrawString(sAfter, e.CellStyle.Font, br, e.CellBounds.X + sBeforeSize.Width + sWordSize.Width, e.CellBounds.Location.Y)
                Else
                    'paint as usual
                    e.PaintContent(e.CellBounds)
                End If
            Else
                'paint as usual
                e.PaintContent(e.CellBounds)
            End If
        End If
    End Sub
    Private Sub DgvSearch_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles DgvSearch.KeyDown
        If e.KeyCode = Keys.Up Then
            If DgvSearch.CurrentRow Is Nothing Then Exit Sub
            If DgvSearch.CurrentRow.Index = 0 Then
                txtGrid_OWN.Select()
            End If
        ElseIf e.KeyCode = Keys.Enter Then
            If DgvSearch.CurrentRow Is Nothing Then Exit Sub
            e.Handled = True
            txtGrid_OWN.Text = DgvSearch.CurrentRow.Cells("ACNAME").Value.ToString
            DgvSearch.Visible = False
            txtGrid_OWN.Select()
            txtGrid_KeyDown(Me, e)
        End If
    End Sub

    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView.Click, ViewF4ToolStripMenuItem.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, mnuId & Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        Dim f As New frmAccountsLedger
        Dim fm As New frmAccLedgerMore
        If vouchType = VoucherType.Receipt Then fm.cmbVoucherType.Text = "Receipt"
        If vouchType = VoucherType.Payment Then fm.cmbVoucherType.Text = "Payment"
        If vouchType = VoucherType.Journal Then fm.cmbVoucherType.Text = "Journal"
        If vouchType = VoucherType.CreditNote Then fm.cmbVoucherType.Text = "Credit Note"
        If vouchType = VoucherType.DebitNote Then fm.cmbVoucherType.Text = "Debit Note"
        f.ShowDialog()
    End Sub

    Private Sub cmbCostCenter_MAN_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCostCenter_MAN.Enter
        Dim serverDate As Date = GetActualEntryDate()
        If funcOTPaccess(Me.Name.ToString, "ACCOUNTS_ACC", cnCostId, userId) = True Then OTP_Access = True : Exit Sub
        Dim RestrictDays As String = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'DATECONTROL_ACC'")
        If RestrictDays.Contains(",") = False Then
            If Not (dtpDate.Value >= serverDate.AddDays(-1 * Val(RestrictDays))) Then
                MsgBox("Invalid Date", MsgBoxStyle.Information)
                dtpDate.Focus()
                Exit Sub
            End If
        Else
            Dim dayarray() As String = Split(RestrictDays, ",")
            Dim closeday As Integer = Val(dayarray(0).ToString)
            Dim mondiv As Integer = Val(dayarray(1).ToString)
            If closeday > serverDate.Day Then
                Dim mindate As Date = serverDate.AddMonths(-1 * mondiv)
                mindate = mindate.AddDays(-1 * (mindate.Day - 1))
                If Not (dtpDate.Value >= mindate) Then
                    MsgBox("Invalid Date", MsgBoxStyle.Information)
                    dtpDate.Focus()
                    Exit Sub
                End If
            Else
                Dim mindate As Date = serverDate.AddMonths(-1 * 0)
                mindate = mindate.AddDays(-1 * (mindate.Day - 1))
                If Not (dtpDate.Value >= mindate) Then
                    MsgBox("Invalid Bill Date", MsgBoxStyle.Information)
                    dtpDate.Focus()
                    Exit Sub
                End If
            End If
        End If
    End Sub



    Private Function checkBdate() As Boolean
        If OTP_Access = True Then Return True : Exit Function
        Dim serverDate As Date = GetEntryDate(GetServerDate()) 'GetActualEntryDate()
        Dim RestrictDays As String = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'DATECONTROL_ACC'")
        If RestrictDays.Contains(",") = False Then
            If Not (dtpDate.Value >= serverDate.AddDays(-1 * Val(RestrictDays))) Then
                MsgBox("Invalid Date", MsgBoxStyle.Information)
                dtpDate.Focus()
                Return False
                Exit Function
            End If
        Else
            Dim dayarray() As String = Split(RestrictDays, ",")
            Dim closeday As Integer = Val(dayarray(0).ToString)
            Dim mondiv As Integer = Val(dayarray(1).ToString)
            If closeday > serverDate.Day Then
                Dim mindate As Date = serverDate.AddMonths(-1 * mondiv)
                mindate = mindate.AddDays(-1 * (mindate.Day - 1))
                If Not (dtpDate.Value >= mindate) Then
                    MsgBox("Invalid Date", MsgBoxStyle.Information)
                    dtpDate.Focus()
                    Return False
                    Exit Function
                End If
            Else
                Dim mindate As Date = serverDate.AddMonths(-1 * 0)
                mindate = mindate.AddDays(-1 * (mindate.Day - 1))
                If Not (dtpDate.Value >= mindate) Then
                    MsgBox("Invalid Bill Date", MsgBoxStyle.Information)
                    dtpDate.Focus()
                    Return False
                    Exit Function
                End If
            End If

        End If

        Return True
    End Function

    Private Sub PreviousNarrationToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PreviousNarrationToolStripMenuItem.Click
        If gridView_OWN.Rows.Count > 0 Then
            Dim tAcName As String = ""
            Dim tAcCode As String = ""
            tAcName = gridView_OWN.Rows(gridView_OWN.Rows.Count - 1).Cells("DESCRIPTION").Value.ToString()
            If tAcName = "" Then Exit Sub
            tAcCode = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & tAcName & "'")
            If tAcCode = "" Then Exit Sub
            strSql = "SELECT DBNAME FROM " & cnAdminDb & "..DBMASTER ORDER BY DBNAME"
            da = New OleDbDataAdapter(strSql, cn)
            Dim dt As DataTable
            dt = New DataTable
            da.Fill(dt)
            strSql = " SELECT TOP 1 * FROM ( "
            For i As Integer = 0 To dt.Rows.Count - 1
                If Not i = 0 Then strSql = strSql + vbCrLf + " UNION ALL "
                strSql = strSql + vbCrLf + " SELECT REMARK1,REMARK2,TRANNO,TRANDATE FROM " & dt.Rows(i)("DBNAME").ToString & "..ACCTRAN WHERE ISNULL(CANCEL,'')=''  AND (ISNULL(REMARK1,'') <> '' OR ISNULL(REMARK2,'') <> '')  "
                Select Case vouchType
                    Case VoucherType.Receipt
                        strSql = strSql + vbCrLf + " AND PAYMODE='CR' "
                    Case VoucherType.Payment
                        strSql = strSql + vbCrLf + " AND PAYMODE='CP' "
                    Case VoucherType.Journal
                        strSql = strSql + vbCrLf + " AND PAYMODE='JE' "
                    Case VoucherType.DebitNote
                        strSql = strSql + vbCrLf + " AND PAYMODE='DN' "
                    Case VoucherType.CreditNote
                        strSql = strSql + vbCrLf + " AND PAYMODE='CN' "
                End Select
            Next
            strSql = strSql + vbCrLf + " )X ORDER BY TRANDATE DESC,TRANNO DESC "
            da = New OleDbDataAdapter(strSql, cn)
            Dim dtt As DataTable
            dtt = New DataTable
            da.Fill(dtt)
            If dtt.Rows.Count > 0 Then
                txtNarration1.Text = dtt.Rows(0)("REMARK1").ToString
                txtNarration2.Text = dtt.Rows(0)("REMARK2").ToString
            End If
        End If
    End Sub

    Private Sub AcPreviousNarrationToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AcPreviousNarrationToolStripMenuItem.Click
        If gridView_OWN.Rows.Count > 0 Then
            Dim tAcName As String = ""
            Dim tAcCode As String = ""
            tAcName = gridView_OWN.Rows(gridView_OWN.Rows.Count - 1).Cells("DESCRIPTION").Value.ToString()
            Dim tType As String = gridView_OWN.Rows(gridView_OWN.Rows.Count - 1).Cells("TYPE").Value.ToString()
            If tAcName = "" Then Exit Sub
            tAcCode = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & tAcName & "'")
            If tAcCode = "" Then Exit Sub
            strSql = "SELECT DBNAME FROM " & cnAdminDb & "..DBMASTER ORDER BY DBNAME"
            da = New OleDbDataAdapter(strSql, cn)
            Dim dt As DataTable
            dt = New DataTable
            da.Fill(dt)
            strSql = " SELECT TOP 1 * FROM ( "
            For i As Integer = 0 To dt.Rows.Count - 1
                If Not i = 0 Then strSql = strSql + vbCrLf + " UNION ALL "
                strSql = strSql + vbCrLf + " SELECT REMARK1,REMARK2,TRANNO,TRANDATE FROM " & dt.Rows(i)("DBNAME").ToString & "..ACCTRAN WHERE ACCODE='" & tAcCode & "' AND ISNULL(CANCEL,'')=''  AND (ISNULL(REMARK1,'') <> '' OR ISNULL(REMARK2,'') <> '')  "
                Select Case vouchType
                    Case VoucherType.Receipt
                        strSql = strSql + vbCrLf + " AND PAYMODE='CR' "
                        If tType = "Received From" Then
                            strSql = strSql + vbCrLf + " AND TRANMODE='C' "
                        Else
                            strSql = strSql + vbCrLf + " AND TRANMODE='D' "
                        End If
                    Case VoucherType.Payment
                        strSql = strSql + vbCrLf + " AND PAYMODE='CP' "
                        If tType = "Paid From" Then
                            strSql = strSql + vbCrLf + " AND TRANMODE='C' "
                        Else
                            strSql = strSql + vbCrLf + " AND TRANMODE='D' "
                        End If
                    Case VoucherType.Journal
                        strSql = strSql + vbCrLf + " AND PAYMODE='JE' "
                        If tType = "Cr" Then
                            strSql = strSql + vbCrLf + " AND TRANMODE='C' "
                        Else
                            strSql = strSql + vbCrLf + " AND TRANMODE='D' "
                        End If
                    Case VoucherType.DebitNote
                        strSql = strSql + vbCrLf + " AND PAYMODE='DN' "
                        If tType = "Cr" Then
                            strSql = strSql + vbCrLf + " AND TRANMODE='C' "
                        Else
                            strSql = strSql + vbCrLf + " AND TRANMODE='D' "
                        End If
                    Case VoucherType.CreditNote
                        strSql = strSql + vbCrLf + " AND PAYMODE='CN' "
                        If tType = "Cr" Then
                            strSql = strSql + vbCrLf + " AND TRANMODE='C' "
                        Else
                            strSql = strSql + vbCrLf + " AND TRANMODE='D' "
                        End If
                End Select
            Next
            strSql = strSql + vbCrLf + " )X ORDER BY TRANDATE DESC,TRANNO DESC "
            da = New OleDbDataAdapter(strSql, cn)
            Dim dtt As DataTable
            dtt = New DataTable
            da.Fill(dtt)
            If dtt.Rows.Count > 0 Then
                txtNarration1.Text = dtt.Rows(0)("REMARK1").ToString
                txtNarration2.Text = dtt.Rows(0)("REMARK2").ToString
            End If
        End If
    End Sub

    Private Sub cmbAcgroup_Own_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbAcgroup_Own.SelectedIndexChanged
        If cmbAcgroup_Own.DataSource Is Nothing Then
            Exit Sub
        End If
        ACNAMEFILTER(Val(cmbAcgroup_Own.SelectedValue.ToString))
    End Sub
    Private Sub dtpDate_KeyDown(sender As Object, e As KeyEventArgs) Handles dtpDate.KeyDown
        If e.KeyCode = Keys.Enter Then
            If Not CheckBckDaysEntry(userId, "", dtpDate.Value, "Account Entry") Then
                dtpDate.Focus()
                Exit Sub
            End If
        End If
    End Sub
    Private Sub InvoiceDateAndNoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles InvoiceDateAndNoToolStripMenuItem.Click
        objInvNo.ShowDialog()
        objBankDetail.ShowDialog()
        If objInvNo.Text.Trim <> "" Then
            ONETIMEFLAGINVOICENOANDDATE = True
            objInvNo.oneTimeClear = ONETIMEFLAGINVOICENOANDDATE
            objBankDetail.oneTimeClear = ONETIMEFLAGINVOICENOANDDATE
        End If
    End Sub
    Private Sub cmbAgainst_Own_KeyDown(sender As Object, e As KeyEventArgs) Handles cmbAgainst_Own.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
End Class