Imports System.Data.OleDb
Public Class MaterialIssRecTran
    Dim acCode As String = ""
    Public Enum MaterialType
        Issue = 0
        Receipt = 1
    End Enum

    Public EditBatchno As String = Nothing
    Public EditTranNo As Integer = Nothing
    Public EditRunno As String = Nothing
    Public EditDueDays As Integer = Nothing
    Public EditCostId As String = Nothing
    Dim DtEditDet As New DataTable
    Dim DtEditIss As New DataTable
    Private Cmd As OleDbCommand
    Private Da As OleDbDataAdapter
    Private DtTran As New DataTable
    Private StrSql As String
    Private OMaterialType As MaterialType
    Private ObjMaterialDia As MaterialIssRec
    Dim objCheaque As New frmChequeAdj
    Dim objAddlCharge As New AddlChargesDia
    Dim objProcessType As New frmProcessTypeSelection
    Dim Transistno As Long = 0
    Dim TranNo As Integer = Nothing
    Dim TranNoApp As Integer = Nothing
    Dim objManualBill As frmManualBillNoGen
    Dim BatchNo As String = Nothing
    Dim CostCenterId As String = Nothing
    Dim _AccAudit As Boolean = IIf(GetAdmindbSoftValue("ACC_AUDIT", "N") = "Y", True, False)
    Public MANBILLNO As Boolean = IIf(GetAdmindbSoftValue("MRMI_MANUALNO" + systemId, "N") = "Y", True, False)
    Dim CatbaseRecIsstranno As Boolean = IIf(GetAdmindbSoftValue("ACC_RECISS_NO", "N") = "Y", True, False)
    Dim TdsAc As String = GetAdmindbSoftValue("TDS_AC", "TDSIN")
    Dim TdsCatId As Integer = Val(GetAdmindbSoftValue("TDS_CAT_MC", 1))
    Dim InclCusttype As String = GetAdmindbSoftValue("INCL_CUSTOMER_ISSREC", "N")
    Dim CstPurchsep As Boolean = IIf(GetAdmindbSoftValue("ACC_CSTPURTAXSEP", "Y") = "Y", True, False)
    Dim SepAccPost As Boolean = IIf(GetAdmindbSoftValue("SEPACCPOST_MRMI", "Y") = "Y", True, False)
    Dim SEPACCPOST_ITEM As String = GetAdmindbSoftValue("SEPACCPOST_ITEM_MRMI", "DSP")
    Dim Lotautopost As Boolean = IIf(GetAdmindbSoftValue("ACC_LOTAUTOPOST", "N") = "Y", True, False)
    Dim ACC_LOTAUTOPOST_TYPE As String = GetAdmindbSoftValue("ACC_LOTAUTOPOST_TYPE", "")
    Dim MultiLot As Boolean = IIf(GetAdmindbSoftValue("ACC_MULTILOTPOST", "N") = "Y", True, False)
    Dim _JobNoEnable As Boolean = IIf(GetAdmindbSoftValue("MRMIJOBNO", "N") = "Y", True, False)
    Dim CatbalanceDisplay As Boolean = IIf(GetAdmindbSoftValue("MRMI_CATEBALANCE", "Y") = "Y", True, False)
    Dim Lotautopost_CostId As String = GetAdmindbSoftValue("ACC_LOTAUTOPOST_COSTID", "")
    Dim Lotautopost_Narration As Boolean = IIf(GetAdmindbSoftValue("ACC_LOTAUTOPOST_NARRATION", "N") = "Y", True, False)

    Private RateRnd = Val(GetAdmindbSoftValue("ROUNDOFF-RATEACC", 2))

    Dim TdsPer As Decimal = Nothing
    Dim _Accode As String = Nothing
    Dim _Acctype As String = Nothing
    Dim CASHID As String = Nothing
    Public _CashCtr As String = ""
    Dim BANKID As String = Nothing
    Dim Remark1 As String = Nothing

    Dim Remark2 As String = Nothing
    Dim lotNo As Integer = 0
    Dim Refresh As Boolean = False
    Dim ACCENTRY_DATE As Boolean = IIf(GetAdmindbSoftValue("ACCENTRY_DATE", "G") = "S", True, False)
    Dim ExitToolStrip As Boolean = False
    Public xOordno, xMordno, xsordno As String
    Public xOProcessname, xMProcessname, xSProcessname As String
    Public xOMetalname, xMmetalName As String
    Public xOTdsname, xMTdsName, xSTdsname As String
    Public xomsNoflag As Boolean = False
    Public xOCategoryname, xMcategoryname, xOIssCatName, xMIsscatName, xOAcpostCatname, xMAcPostcatname As String
    Public xOpurityper, xMpurityper As Decimal
    Dim MRMIAPPACCPOST As Boolean = IIf(GetAdmindbSoftValue("MRMIAPPACCPOST", "Y") = "Y", True, False)
    Dim MRMI_VATSEPPOST As Boolean = IIf(GetAdmindbSoftValue("MRMI_VATSEPPOST", "Y") = "Y", True, False)
    Public MdtOdDet As DataTable
    Dim mDr As DataRow
    Dim SMS_MSG_MRMI As String = objGPack.GetSqlValue("SELECT ISNULL(TEMPLATE_DESC,'') AS TEMPLATE_DESC FROM " & cnAdminDb & "..SMSTEMPLATE WHERE TEMPLATE_NAME='SMS_MSG_MRMI' AND ISNULL(ACTIVE,'Y')<>'N'", "TEMPLATE_DESC").ToString
    Dim SMS_MSG_MIMR As String = objGPack.GetSqlValue("SELECT ISNULL(TEMPLATE_DESC,'') AS TEMPLATE_DESC FROM " & cnAdminDb & "..SMSTEMPLATE WHERE TEMPLATE_NAME='SMS_MSG_MIMR' AND ISNULL(ACTIVE,'Y')<>'N'", "TEMPLATE_DESC").ToString
    Dim MobileNo As String = ""
    Dim MIMR_BAGISSUE As Boolean = IIf(GetAdmindbSoftValue("MIMR_BAGISSUE", "N") = "Y", True, False)
    Dim MRMI_LOCK_BILLNODATE As Boolean = IIf(GetAdmindbSoftValue("MRMI_LOCK_BILLNODATE", "N") = "Y", True, False)
    Dim TagNos As String = ""
    Dim CatNames As String = ""
    Dim _JobNo As Boolean
    Dim MRMI_ORDERNO As Boolean = IIf(GetAdmindbSoftValue("MRMI_ORDERNO", "N") = "Y", True, False)
    Dim ROUNDOFF_ACC As String = GetAdmindbSoftValue("ROUNDOFF-ACC", "N").ToUpper
    Dim InterStateBill As Boolean
    Dim OnlyMc As Boolean
    Dim SRVT_COMPONENTS As String = GetAdmindbSoftValue("SRVT_COMPONENTS", "")
    Dim SrvTaxSGCode As String = ""
    Dim SrvTaxCGCode As String = ""
    Dim SrvTaxIGCode As String = ""
    Dim SRVTID As String
    Dim SRVTPER As Double
    Public GstFlag As Boolean = False
    Public __reciss As String = ""
    Dim MR_VALIDATESTOCK As Boolean = IIf(GetAdmindbSoftValue("MR_VALIDATESTOCK", "N") = "Y", True, False)
    Dim ORDER_MULTI_MIMR As Boolean = IIf(GetAdmindbSoftValue("ORDER_MULTI_MIMR", "N") = "Y", True, False)
    Dim LOCK_APPROVAL_MIMR As Boolean = IIf(GetAdmindbSoftValue("LOCK_APPROVAL_MIMR", "N") = "Y", True, False)
    Dim MREDIT_WITHLOT As Boolean = IIf(GetAdmindbSoftValue("MREDIT_WITHLOT", "N") = "Y", True, False)
    Dim MIMR_APPROXVALUE As Boolean = IIf(GetAdmindbSoftValue("MIMR_APRX_VALUE", "N") = "Y", True, False)
    Dim MIMR_VALIDATEBILLNO As Boolean = IIf(GetAdmindbSoftValue("MIMR_VALIDATEBILLNO", "N") = "Y", True, False)
    Dim MIMR_ESTIMATECALL As Boolean = IIf(GetAdmindbSoftValue("MIMR_ESTIMATECALL", "N") = "Y", True, False)
    Dim MIMR_EDITCOSTCENTRE As Boolean = IIf(GetAdmindbSoftValue("MIMR_EDITCOSTCENTRE", "Y") = "Y", True, False)
    Dim MIMR_TRANSFERTAGCALL As Boolean = IIf(GetAdmindbSoftValue("MIMR_TRANSFERTAGCALL", "Y") = "Y", True, False)
    Dim SPECIFICFORMAT As Boolean = GetAdmindbSoftValue("SPECIFICFORMAT", "0")
    Dim MIMR_DISPTOT_TDS As Boolean = IIf(GetAdmindbSoftValue("MIMR_DISPLAY_TOTALTDS", "N") = "Y", True, False)
    Dim MIMR_USER_RESTRICT As String = GetAdmindbSoftValue("MIMR_USER_RESTRICT", "")
    Dim _Tdsaccode As String = ""
    Dim MI_POPUP_PROCESSTYPE As Boolean = IIf(GetAdmindbSoftValue("MI_POPUP_PROCESSTYPE", "N") = "Y", True, False)



    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub


    Public Sub New(ByVal oMaterialType As MaterialType)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        If MIMR_DISPTOT_TDS Then
            lblTotalTds.Visible = True
        Else
            lblTotalTds.Visible = False
        End If
        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
        objGPack.TextClear(Me)
        Me.OMaterialType = oMaterialType
        If oMaterialType = MaterialType.Issue Then
            txtAdditionalCharges.Enabled = False
        End If
        Initializer()
    End Sub

    Private Sub Initializer()
        CASHID = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'CASH'", , "CASH")
        BANKID = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'BANK'", , "BANK")
        With DtTran
            .Columns.Add("JOBNO", GetType(String))
            .Columns.Add("DESCRIPTION", GetType(String))
            .Columns.Add("PCS", GetType(Integer))
            .Columns.Add("GRSWT", GetType(Decimal))
            .Columns.Add("NETWT", GetType(Decimal))
            .Columns.Add("RATE", GetType(Decimal))
            .Columns.Add("WASTAGE", GetType(Decimal))
            .Columns.Add("ALLOY", GetType(Decimal))
            .Columns.Add("MC", GetType(Decimal))
            .Columns.Add("TOUCH", GetType(Decimal))
            .Columns.Add("PUREWT", GetType(Decimal))
            .Columns.Add("TOTALWT", GetType(Decimal))
            .Columns.Add("AMOUNT", GetType(Decimal))

            .Columns.Add("TRANTYPE", GetType(String))
            .Columns.Add("METAL", GetType(String))
            .Columns.Add("CATNAME", GetType(String))
            .Columns.Add("ACCATNAME", GetType(String))
            .Columns.Add("ISSCATNAME", GetType(String))
            .Columns.Add("PURITY", GetType(Decimal))
            .Columns.Add("ITEM", GetType(String))
            .Columns.Add("SUBITEM", GetType(String))
            .Columns.Add("LESSWT", GetType(Decimal))
            .Columns.Add("GRSNET", GetType(String))
            .Columns.Add("WASTPER", GetType(Decimal))
            .Columns.Add("MCGRM", GetType(Decimal))
            .Columns.Add("GROSSAMT", GetType(Decimal))



            .Columns.Add("VAT", GetType(Decimal)) 'CHECKTDS
            .Columns.Add("VATPER", GetType(Decimal))
            .Columns.Add("STUDAMT", GetType(Decimal))
            .Columns.Add("UNIT", GetType(String))
            .Columns.Add("CALCMODE", GetType(String))

            .Columns.Add("TYPE", GetType(String))
            .Columns.Add("BOARDRATE", GetType(Decimal))
            .Columns.Add("KEYNO", GetType(Integer))
            .Columns.Add("REMARK1", GetType(String))
            .Columns.Add("REMARK2", GetType(String))
            .Columns.Add("ORDSTATE_NAME", GetType(String))
            .Columns.Add("ADDCHARGE", GetType(Decimal))
            .Columns.Add("METISSREC", GetType(MaterialIssRec))
            .Columns.Add("SEIVE", GetType(String))
            .Columns.Add("OGRSWT", GetType(Decimal))
            .Columns.Add("ONETWT", GetType(Decimal))
            .Columns.Add("ORSNO", GetType(String))
            .Columns.Add("RESNO", GetType(String))
            .Columns.Add("TAGNO", GetType(String))
            .Columns.Add("ACCODE", GetType(String))
            .Columns.Add("RFID", GetType(String))
            .Columns.Add("CUTID", GetType(String))
            .Columns.Add("COLORID", GetType(String))
            .Columns.Add("CLARITYID", GetType(String))
            .Columns.Add("SHAPEID", GetType(String))
            .Columns.Add("SETTYPEID", GetType(String))
            .Columns.Add("HEIGHT", GetType(String))
            .Columns.Add("WIDTH", GetType(String))
            .Columns.Add("STNGRPID", GetType(String))
            .Columns.Add("EDPER", GetType(Decimal))
            .Columns.Add("ED", GetType(Decimal))
            .Columns.Add("TCSPER", GetType(Decimal))
            .Columns.Add("TCS", GetType(Decimal))
            .Columns.Add("CALCON", GetType(String))
            .Columns.Add("SGSTPER", GetType(Decimal))
            .Columns.Add("CGSTPER", GetType(Decimal))
            .Columns.Add("IGSTPER", GetType(Decimal))
            .Columns.Add("GST", GetType(Decimal))
            .Columns.Add("SGST", GetType(Decimal))
            .Columns.Add("CGST", GetType(Decimal))
            .Columns.Add("IGST", GetType(Decimal))
            .Columns.Add("DISCOUNT", GetType(Decimal))
            .Columns.Add("APPROXAMT", GetType(Decimal))
            .Columns.Add("APPROXTAX", GetType(Decimal))
            .Columns.Add("RATEFIXED", GetType(String))
            .Columns.Add("HALLMARK", GetType(String))
            .Columns.Add("TDSACCODE", GetType(String))
            .Columns.Add("ROWINDEX", GetType(Int32))
            .Columns("KEYNO").AutoIncrement = True
        End With
        DgvTran.Columns.Clear()
        DgvTran.DataSource = DtTran
        ClearTran()
        GridStyle(DgvTran)
        DgvTran.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 9, FontStyle.Bold)
        DgvTran.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)

        Dim DtTranTotal As New DataTable
        DtTranTotal = DtTran.Clone
        DtTranTotal.Rows.Add()
        DtTranTotal.Rows(0).Item("DESCRIPTION") = "TOTAL"
        DgvTranTotal.DataSource = DtTranTotal
        GridStyle(DgvTranTotal)
        DgvTranTotal.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)

        MdtOdDet = New DataTable
        With MdtOdDet.Columns
            .Add("ORNO", GetType(String))
            .Add("ORSNO", GetType(String))
            .Add("PCS", GetType(Int32))
            .Add("GRSWT", GetType(Decimal))
            .Add("NETWT", GetType(Decimal))
            .Add("WASTAGE", GetType(Decimal))
            .Add("MC", GetType(Decimal))
            .Add("ROWINDEX", GetType(Int32))
        End With
        With dgvOrderDet
            .DataSource = Nothing
            MdtOdDet.Rows.Clear()
            .DataSource = MdtOdDet
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            .ScrollBars = ScrollBars.Vertical
            .Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("WASTAGE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("MC").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        End With

        ''COSTCENTRE
        If GetAdmindbSoftValue("COSTCENTRE", "N") = "Y" Then
            StrSql = "SELECT COSTNAME,COSTID FROM " & cnAdminDb & "..COSTCENTRE "
            StrSql += " WHERE  ISNULL(COMPANYID,'" & strCompanyId & "')LIKE'%" & strCompanyId & "%'"
            StrSql += " ORDER BY COSTNAME"
            Dim dtCostcentre As New DataTable
            Da = New OleDbDataAdapter(StrSql, cn)
            Da.Fill(dtCostcentre)
            cmbCostCentre.DataSource = dtCostcentre
            cmbCostCentre.AutoCompleteMode = AutoCompleteMode.SuggestAppend
            cmbCostCentre.AutoCompleteSource = AutoCompleteSource.ListItems
            cmbCostCentre.DisplayMember = "COSTNAME"
            cmbCostCentre.ValueMember = "COSTID"
            cmbCostCentre.Enabled = True
            If MIMR_EDITCOSTCENTRE = True Then
                cmbCostCentre.Enabled = True
            Else
                cmbCostCentre.Text = cnCostName
                cmbCostCentre.Enabled = False
            End If
        Else
            cmbCostCentre.Enabled = False
        End If

        ''ACCNAME
        StrSql = " SELECT ACNAME,ACCODE FROM " & cnAdminDb & "..ACHEAD "
        If InclCusttype = "Y" Then
            StrSql += " WHERE ACTYPE IN (SELECT DISTINCT TYPEID FROM " & cnAdminDb & "..ACCTYPE "
            StrSql += " WHERE ACTYPE IN ('G','D','I','C'))"
        Else
            StrSql += " WHERE ACTYPE IN (SELECT DISTINCT TYPEID FROM " & cnAdminDb & "..ACCTYPE "
            StrSql += " WHERE ACTYPE IN ('G','D','I'))"
        End If
        StrSql += GetAcNameQryFilteration()
        StrSql += " ORDER BY ACNAME"
        Dim dtAcName As New DataTable
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dtAcName)
        cmbAcName.DataSource = dtAcName
        cmbAcName.ValueMember = "ACCODE"
        cmbAcName.DisplayMember = "ACNAME"
        cmbAcName.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cmbAcName.AutoCompleteSource = AutoCompleteSource.ListItems
        ''TransactionType
        LoadTransactionType()
    End Sub

    Private Sub LoadTransactionType()
        cmbTransactionType.DropDownStyle = ComboBoxStyle.DropDownList
        cmbTransactionType.Items.Clear()
        Dim Row() As DataRow = DtTran.Select("TRANTYPE <> ''")
        Dim ExistingTranType As New List(Of String)

        For Each Ro As DataRow In Row
            ExistingTranType.Add(Ro.Item("TRANTYPE").ToString)
        Next

        Select Case OMaterialType
            Case MaterialType.Issue
                If ExistingTranType.Count > 0 Then
                    cmbTransactionType.Items.Add(ExistingTranType(0))
                    cmbTransactionType.SelectedIndex = 0
                Else
                    If MIMR_USER_RESTRICT.ToString <> "" Then
                        Dim _temptrantypeuser() As String = MIMR_USER_RESTRICT.ToString.Split(",")
                        Dim _tempcontains As Boolean = False
                        Dim _tempcontainstr As String = ""
                        For cnt As Integer = 0 To _temptrantypeuser.Length - 1
                            _tempcontainstr = ""
                            _tempcontains = False
                            If _temptrantypeuser(cnt).ToString.Contains(userId.ToString & ":") Then
                                Dim _tempuserstr() As String = _temptrantypeuser(cnt).ToString.Split(":")
                                If _tempuserstr.Length = 3 Then
                                    If _tempuserstr(0).ToString = userId.ToString Then
                                        _tempcontains = True
                                        _tempcontainstr = _temptrantypeuser(cnt).ToString
                                        Exit For
                                    End If
                                End If
                            End If
                        Next
                        If _tempcontains = True And _tempcontainstr.ToString <> "" Then
                            If _tempcontainstr.ToString.Contains("IIS") Then cmbTransactionType.Items.Add("ISSUE")
                            If _tempcontainstr.ToString.Contains("IPU") Then cmbTransactionType.Items.Add("PURCHASE RETURN")
                            If _tempcontainstr.ToString.Contains("IIN") Then cmbTransactionType.Items.Add("INTERNAL TRANSFER")
                            If _tempcontainstr.ToString.Contains("IAP") Then cmbTransactionType.Items.Add("APPROVAL ISSUE")
                            If _tempcontainstr.ToString.Contains("IOT") Then cmbTransactionType.Items.Add("OTHER ISSUE")
                            If _tempcontainstr.ToString.Contains("IPA") Then cmbTransactionType.Items.Add("PACKING")
                            If _tempcontainstr.ToString.Contains("IDN") Then cmbTransactionType.Items.Add("DELIVERY NOTE")
                        Else
                            GoTo MoveIss
                        End If
                    Else
MoveIss:
                        cmbTransactionType.Items.Add("ISSUE")
                        cmbTransactionType.Items.Add("PURCHASE RETURN")
                        cmbTransactionType.Items.Add("INTERNAL TRANSFER")
                        If LOCK_APPROVAL_MIMR = False Then cmbTransactionType.Items.Add("APPROVAL ISSUE")
                        cmbTransactionType.Items.Add("OTHER ISSUE")
                        cmbTransactionType.Items.Add("PACKING")
                        cmbTransactionType.Items.Add("DELIVERY NOTE")
                    End If
                    cmbTransactionType.SelectedIndex = 0
                End If
            Case MaterialType.Receipt
                If ExistingTranType.Count > 0 Then
                    cmbTransactionType.Items.Add(ExistingTranType(0))
                    cmbTransactionType.SelectedIndex = 0
                Else
                    If MIMR_USER_RESTRICT.ToString <> "" Then
                        Dim _temptrantypeuser() As String = MIMR_USER_RESTRICT.ToString.Split(",")
                        Dim _tempcontains As Boolean = False
                        Dim _tempcontainstr As String = ""
                        For cnt As Integer = 0 To _temptrantypeuser.Length - 1
                            _tempcontainstr = ""
                            _tempcontains = False
                            If _temptrantypeuser(cnt).ToString.Contains(userId.ToString & ":") Then
                                Dim _tempuserstr() As String = _temptrantypeuser(cnt).ToString.Split(":")
                                If _tempuserstr.Length = 3 Then
                                    If _tempuserstr(0).ToString = userId.ToString Then
                                        _tempcontains = True
                                        _tempcontainstr = _temptrantypeuser(cnt).ToString
                                        Exit For
                                    End If
                                End If
                            End If
                        Next
                        If _tempcontains = True And _tempcontainstr.ToString <> "" Then
                            If _tempcontainstr.ToString.Contains("RRE") Then cmbTransactionType.Items.Add("RECEIPT")
                            If _tempcontainstr.ToString.Contains("RPU") Then cmbTransactionType.Items.Add("PURCHASE")
                            If _tempcontainstr.ToString.Contains("RPU") Then cmbTransactionType.Items.Add("PURCHASE[APPROVAL]")
                            If _tempcontainstr.ToString.Contains("RIN") Then cmbTransactionType.Items.Add("INTERNAL TRANSFER")
                            If _tempcontainstr.ToString.Contains("RAP") Then cmbTransactionType.Items.Add("APPROVAL RECEIPT")
                            If _tempcontainstr.ToString.Contains("ROT") Then cmbTransactionType.Items.Add("OTHER RECEIPT")
                            If _tempcontainstr.ToString.Contains("RPA") Then cmbTransactionType.Items.Add("PACKING")
                            If _tempcontainstr.ToString.Contains("RDN") Then cmbTransactionType.Items.Add("RECEIPT NOTE")
                        Else
                            GoTo MoveRec
                        End If
                    Else
MoveRec:
                        cmbTransactionType.Items.Add("RECEIPT")
                        cmbTransactionType.Items.Add("PURCHASE")
                        cmbTransactionType.Items.Add("PURCHASE[APPROVAL]")
                        cmbTransactionType.Items.Add("INTERNAL TRANSFER")
                        If LOCK_APPROVAL_MIMR = False Then cmbTransactionType.Items.Add("APPROVAL RECEIPT")
                        cmbTransactionType.Items.Add("OTHER RECEIPT")
                        cmbTransactionType.Items.Add("PACKING")
                        cmbTransactionType.Items.Add("RECEIPT NOTE")
                    End If
                    cmbTransactionType.SelectedIndex = 0
                End If
        End Select


        Dim pcs As Integer = Nothing
        Dim grsWt As Decimal = Nothing
        Dim netWt As Decimal = Nothing
        Dim pureWt As Decimal = Nothing
        Dim wastage As Decimal = Nothing
        Dim Alloy As Decimal = Nothing
        Dim mc As Decimal = Nothing
        Dim Amt As Decimal = Nothing
        Dim GstAmt As Decimal = Nothing
        For Each Ro As DataRow In DtTran.Rows
            If Ro.RowState <> DataRowState.Deleted Then
                pcs += Val(Ro.Item("PCS").ToString)
                grsWt += Val(Ro.Item("GRSWT").ToString)
                netWt += Val(Ro.Item("NETWT").ToString)
                pureWt += Val(Ro.Item("PUREWT").ToString)
                wastage += Val(Ro.Item("WASTAGE").ToString)
                Alloy += Val(Ro.Item("ALLOY").ToString)
                mc += Val(Ro.Item("MC").ToString)
                Amt += Val(Ro.Item("AMOUNT").ToString)
                GstAmt += Val(Ro.Item("GST").ToString)
            End If
        Next
        If Alloy = 0 Then DgvTran.Columns("ALLOY").Visible = False : DgvTranTotal.Columns("ALLOY").Visible = False
        DgvTranTotal.Rows(0).Cells("PCS").Value = IIf(pcs <> 0, pcs, DBNull.Value)
        DgvTranTotal.Rows(0).Cells("GRSWT").Value = IIf(grsWt <> 0, grsWt, DBNull.Value)
        DgvTranTotal.Rows(0).Cells("NETWT").Value = IIf(netWt <> 0, netWt, DBNull.Value)
        DgvTranTotal.Rows(0).Cells("PUREWT").Value = IIf(pureWt <> 0, pureWt, DBNull.Value)
        DgvTranTotal.Rows(0).Cells("WASTAGE").Value = IIf(wastage <> 0, wastage, DBNull.Value)
        DgvTranTotal.Rows(0).Cells("ALLOY").Value = IIf(Alloy <> 0, Alloy, DBNull.Value)
        DgvTranTotal.Rows(0).Cells("MC").Value = IIf(mc <> 0, mc, DBNull.Value)
        DgvTranTotal.Rows(0).Cells("AMOUNT").Value = IIf(Amt <> 0, Amt, DBNull.Value)
        DgvTranTotal.Rows(0).Cells("GST").Value = IIf(GstAmt <> 0, GstAmt, DBNull.Value)
        CalcReceiveAmount()
    End Sub

    Private Sub CalcReceiveAmount()
        Dim rAmt As Decimal = Nothing
        rAmt = Val(DgvTranTotal.Rows(0).Cells("AMOUNT").Value.ToString)
        rAmt += Val(txtAdditionalCharges.Text)
        rAmt += Val(txtTCS_AMT.Text.ToString)
        txtAdjReceive_AMT.Text = IIf(rAmt <> 0, Format(rAmt, "0.00"), Nothing)
        If MIMR_DISPTOT_TDS Then
            Dim _TotTDS As Decimal = 0
            If Not ObjMaterialDia Is Nothing Then
                If ObjMaterialDia.lblOVat.Text = "Tds" Or ObjMaterialDia.lblMVat.Text = "Tds" _
                    Or ObjMaterialDia.lblSVat.Text = "Tds" Or ObjMaterialDia.lblOthVat.Text = "Tds" Then
                    _TotTDS = Val(DtTran.Compute("SUM(VAT)", "TRANTYPE <> ''").ToString)
                End If
            End If
            If Val(_TotTDS.ToString) <> 0 Then
                lblTotalTds.Visible = True
                lblTotalTds.Text = "Total Tds : " & Format(Val(_TotTDS.ToString), "0.00")
            Else
                lblTotalTds.Visible = False
                lblTotalTds.Text = "Total Tds : "
            End If

        End If
    End Sub

    Private Sub CalcCreditAmount(ByVal sender As Object, ByVal e As EventArgs) Handles _
    txtAdjReceive_AMT.TextChanged,
    txtAdjCash_AMT.TextChanged,
    txtAdjCheque_AMT.TextChanged,
    txtAdjRoundOff_AMT.TextChanged
        Dim creditAmt As Double = Nothing
        creditAmt = Val(txtAdjReceive_AMT.Text) - Val(txtAdjCash_AMT.Text) - Val(txtAdjCheque_AMT.Text) - Val(txtAdjRoundOff_AMT.Text)
        txtAdjCredit_AMT.Text = IIf(creditAmt <> 0, Format(creditAmt, "0.00"), Nothing)
    End Sub


    Private Sub ClearTran()
        DtTran.Rows.Clear()
        For cnt As Integer = 0 To 20
            DtTran.Rows.Add()
        Next
        DtTran.AcceptChanges()
    End Sub

    Private Sub GridStyle(ByVal Dgv As DataGridView)
        With Dgv
            For cnt As Integer = 0 To .ColumnCount - 1
                .Columns(cnt).Visible = True
                .Columns(cnt).SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns(cnt).Resizable = DataGridViewTriState.False
            Next
            .Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("RATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("WASTAGE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("MC").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("TOUCH").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("PUREWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("TOTALWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("GROSSAMT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("VAT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("ALLOY").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            .Columns("PURITY").DefaultCellStyle.Format = "0.00"
            .Columns("GRSWT").DefaultCellStyle.Format = "0.000"
            .Columns("NETWT").DefaultCellStyle.Format = "0.000"
            .Columns("LESSWT").DefaultCellStyle.Format = "0.000"
            .Columns("RATE").DefaultCellStyle.Format = FormatNumberStyle(RateRnd)
            .Columns("BOARDRATE").DefaultCellStyle.Format = "0.00"
            .Columns("WASTPER").DefaultCellStyle.Format = "0.00"
            .Columns("WASTAGE").DefaultCellStyle.Format = "0.000"
            .Columns("ALLOY").DefaultCellStyle.Format = "0.000"
            .Columns("MCGRM").DefaultCellStyle.Format = "0.00"
            .Columns("MC").DefaultCellStyle.Format = "0.00"
            .Columns("TOUCH").DefaultCellStyle.Format = "0.00"
            .Columns("PUREWT").DefaultCellStyle.Format = "0.000"
            .Columns("TOTALWT").DefaultCellStyle.Format = "0.000"
            .Columns("GROSSAMT").DefaultCellStyle.Format = "0.00"
            .Columns("VAT").DefaultCellStyle.Format = "0.00"
            .Columns("VATPER").DefaultCellStyle.Format = "0.00"
            .Columns("AMOUNT").DefaultCellStyle.Format = "0.00"
            .Columns("OGRSWT").DefaultCellStyle.Format = "0.000"
            .Columns("ONETWT").DefaultCellStyle.Format = "0.000"
            .Columns("GST").DefaultCellStyle.Format = "0.00"
            .Columns("APPROXAMT").DefaultCellStyle.Format = "0.00"
            .Columns("APPROXTAX").DefaultCellStyle.Format = "0.00"

            .Columns("WASTAGE").HeaderText = "WAST"
            .Columns("JOBNO").Width = 52
            .Columns("DESCRIPTION").Width = 200
            .Columns("PCS").Width = 40
            .Columns("GRSWT").Width = 80
            .Columns("NETWT").Width = 80
            .Columns("RATE").Width = 80
            .Columns("WASTAGE").Width = 70
            .Columns("ALLOY").Width = 70
            .Columns("MC").Width = 60
            .Columns("GST").Width = 60
            .Columns("TOUCH").Width = 60
            .Columns("PUREWT").Width = 80
            .Columns("TOTALWT").Width = 80
            .Columns("AMOUNT").Width = 100
            For cnt As Integer = 13 To .ColumnCount - 1
                .Columns(cnt).Visible = False
            Next
            .Columns("ALLOY").Visible = False
            .Columns("TOTALWT").Visible = False
            If GetAdmindbSoftValue("LOCKWOPCRATE", "N") = "Y" Then
                .Columns("GRSWT").Visible = False
                .Columns("NETWT").Visible = False
                .Columns("PUREWT").Visible = False
                .Columns("TOTALWT").Visible = False
                .Columns("ALLOY").Visible = False
                .Columns("WASTAGE").Visible = False
                .Columns("MC").Visible = False
                .Columns("TOUCH").Visible = False
                .Columns("GROSSAMT").Visible = True
                .Columns("VAT").DisplayIndex = 8
                .Columns("GROSSAMT").DisplayIndex = 7
                .Columns("VAT").Visible = True
            End If
            .Columns("GST").Visible = True
            .Columns("RATEFIXED").Visible = False
            .Columns("HALLMARK").Visible = False
        End With
    End Sub

    Private Sub MaterialIssRecTran_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.F5 Then
            txtAdjCash_AMT.Select()
        ElseIf e.KeyCode = Keys.F9 Then
            txtAdjCheque_AMT.Select()
        ElseIf e.KeyCode = Keys.F11 Then
            txtAdjCredit_AMT.Select()
        ElseIf e.KeyCode = Keys.F8 Then
            txtAdjRoundOff_AMT.Select()
        ElseIf e.KeyCode = Keys.F7 Then
            'txtAdditionalCharges.Select()
            funcAdditionalCharges()
        ElseIf e.KeyCode = Keys.B And e.Control = True Then
            If Lotautopost Then
                If OMaterialType = MaterialType.Receipt Then
                    gboxBulk.BringToFront()
                    gboxBulk.Visible = True
                End If
            End If
        ElseIf e.KeyCode = Keys.O And e.Control = True Then
            If txtRemark1.Focused And Lotautopost Then
                If OMaterialType = MaterialType.Receipt Then
                    gboxOrder.BringToFront()
                    gboxOrder.Visible = True
                    chkOrder.Focus()
                End If
            End If

        ElseIf e.KeyCode = Keys.Enter Then
            'If cmbCostCentre.Focused Then Exit Sub
            'If cmbAcName.Focused Then Exit Sub
            'If cmbTransactionType.Focused Then Exit Sub
            If dtpTrandate.Focused Then LoadBalanceWt()
            If DgvTran.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        ElseIf e.KeyCode = Keys.Escape Then
            If DgvTran.Focused Then txtAdjCash_AMT.Focus()
        End If
    End Sub

    Private Sub MaterialIssRecTran_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.BackColor = SystemColors.InactiveCaption
        objGPack.Validator_Object(Me)
        DgvTran.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Sunken
        DgvTranTotal.Rows(0).DefaultCellStyle.BackColor = Color.LightGoldenrodYellow
        DgvTranTotal.Rows(0).DefaultCellStyle.SelectionBackColor = Color.LightGoldenrodYellow
        DgvTranTotal.Rows(0).DefaultCellStyle.ForeColor = Color.Red
        DgvTranTotal.Rows(0).DefaultCellStyle.SelectionForeColor = Color.Red
        dtpTrandate.Value = GetEntryDate(GetServerDate, , ACCENTRY_DATE)
        dtpBillDate_OWN.Value = GetEntryDate(GetServerDate, , ACCENTRY_DATE)
        dtpEstdate_OWN.Value = GetEntryDate(GetServerDate, , ACCENTRY_DATE)
        If MRMI_LOCK_BILLNODATE Then
            Label19.Visible = False
            dtpBillDate_OWN.Visible = False
            Label20.Visible = False
            txtBillNo.Visible = False
        Else
            Label19.Visible = True
            dtpBillDate_OWN.Visible = True
            Label20.Visible = True
            txtBillNo.Visible = True
        End If
        If MRMI_ORDERNO And OMaterialType = MaterialType.Receipt Then
            lblOrdNo.Visible = True
            txtOrdNo.Visible = True
        End If
        If MIMR_ESTIMATECALL And OMaterialType = MaterialType.Issue And EditBatchno = Nothing Then
            lblEstDate.Visible = True
            lblEstNo.Visible = True
            dtpEstdate_OWN.Visible = True
            txtEstNo_NUM.Visible = True
        Else
            lblEstDate.Visible = False
            lblEstNo.Visible = False
            dtpEstdate_OWN.Visible = False
            txtEstNo_NUM.Visible = False
        End If

        If MIMR_TRANSFERTAGCALL And OMaterialType = MaterialType.Issue And EditBatchno = Nothing Then
            lbltransferno.Visible = True
            txttransferno.Visible = True
        Else
            lbltransferno.Visible = False
            txttransferno.Visible = False
        End If

        If Not BrighttechPack.Methods.GetRights(_DtUserRights, IIf(OMaterialType = MaterialType.Receipt, "REC", "ISS") & Me.Name, BrighttechPack.Methods.RightMode.View) Then btnView.Enabled = False
        btnNew_Click(Me, New EventArgs)
        If EditBatchno <> Nothing Then btnView.Enabled = False : LoadEditValues()
        If CompanyStateId = 0 And GST Then
            MsgBox("State not Updated in Company Master.", MsgBoxStyle.Information)
        End If
        Dim st() As String = SRVT_COMPONENTS.Split(",")
        If st.Length >= 2 Then
            SRVTID = st(0)
            If st.Length >= 2 Then SrvTaxSGCode = st(1)
            If st.Length >= 3 Then SrvTaxCGCode = st(2)
            If st.Length >= 4 Then SrvTaxIGCode = st(3)
            SRVTPER = Val(objGPack.GetSqlValue("SELECT STAX FROM " & cnAdminDb & "..TAXMAST WHERE TAXCODE ='" & SRVTID.ToString() & "'", , "0"))
        End If
        GstFlag = GST
    End Sub

    Private Sub LoadEditValues()
        Dim _ORDSTATE_NAME As String = ""
        btnNew.Enabled = False
        btnSave.Enabled = False
        _AccAudit = False
        StrSql = vbCrLf + "  SELECT COSTID,TRANNO"
        StrSql += vbCrLf + "  ,(SELECT TOP 1 RUNNO FROM " & cnAdminDb & "..OUTSTANDING WHERE BATCHNO = I.BATCHNO)AS RUNNO"
        StrSql += vbCrLf + "  ,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = I.COSTID)AS COSTNAME"
        StrSql += vbCrLf + "  ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = I.ACCODE)AS ACNAME"
        StrSql += vbCrLf + "  ,ACCODE,TRANDATE,TRANNO,REFNO,REFDATE"
        StrSql += vbCrLf + "  ,CASE TRANTYPE WHEN 'IIS' THEN 'ISSUE'"
        StrSql += vbCrLf + "  	WHEN 'IPU' THEN 'PURCHASE RETURN'"
        StrSql += vbCrLf + "  	WHEN 'IIN' THEN 'INTERNAL TRANSFER'"
        StrSql += vbCrLf + "  	WHEN 'IAP' THEN 'APPROVAL ISSUE'"
        StrSql += vbCrLf + "  	WHEN 'IOT' THEN 'OTHER ISSUE' "
        StrSql += vbCrLf + "  	WHEN 'RRE' THEN 'RECEIPT' "
        StrSql += vbCrLf + "  	WHEN 'RPU' THEN 'PURCHASE'"
        StrSql += vbCrLf + "  	WHEN 'RIN' THEN 'INTERNAL TRANSFER'"
        StrSql += vbCrLf + "  	WHEN 'RAP' THEN 'APPROVAL RECEIPT'"
        StrSql += vbCrLf + "  	WHEN 'ROT' THEN 'OTHER RECEIPT'"
        StrSql += vbCrLf + "  	WHEN 'IDN' THEN 'DELIVERY NOTE'"
        StrSql += vbCrLf + "  	WHEN 'RDN' THEN 'RECEIPT NOTE'"
        StrSql += vbCrLf + "  	ELSE TRANTYPE END AS TRANTYPE"
        StrSql += vbCrLf + "  ,DATEDIFF(DAY,TRANDATE,(SELECT TOP 1 DUEDATE FROM " & cnAdminDb & "..OUTSTANDING WHERE BATCHNO = I.BATCHNO AND DUEDATE IS NOT NULL ))AS DUEDAYS"
        StrSql += vbCrLf + "  ,(SELECT TOP 1 REMARK1 FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = I.BATCHNO AND REMARK1 <> '')AS REMARK1"
        StrSql += vbCrLf + "  ,(SELECT TOP 1 REMARK2 FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = I.BATCHNO AND REMARK2 <> '')AS REMARK2"
        StrSql += vbCrLf + "  ,DISCOUNT,(SELECT ORDSTATE_NAME FROM " & cnAdminDb & "..ORDERSTATUS WHERE ORDSTATE_ID = I.ORDSTATE_ID)AS ORDSTATE_NAME"
        If OMaterialType = MaterialType.Issue Then
            StrSql += vbCrLf + "  FROM " & cnStockDb & "..ISSUE AS I"
        Else
            StrSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT AS I"
        End If
        StrSql += vbCrLf + "  WHERE BATCHNO = '" & EditBatchno & "' AND LEN(TRANTYPE) > 2"
        Dim DtCommanInfo As New DataTable
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(DtCommanInfo)
        If Not DtCommanInfo.Rows.Count > 0 Then Exit Sub
        Dim Ro As DataRow = DtCommanInfo.Rows(0)
        _ORDSTATE_NAME = Ro.Item("ORDSTATE_NAME").ToString
        EditCostId = Ro.Item("COSTID").ToString
        cmbCostCentre.Text = Ro.Item("COSTNAME").ToString
        CostCenterId = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre.Text & "'", , , tran)
        cmbAcName.Text = Ro.Item("ACNAME").ToString
        _Accode = Ro.Item("ACCODE").ToString
        If Ro.Item("REFDATE").ToString <> "" Then dtpBillDate_OWN.Value = Ro.Item("REFDATE")
        If Ro.Item("REFNO").ToString <> "" Or Ro.Item("REFNO").ToString <> "0" Then txtBillNo.Text = Ro.Item("REFNO").ToString
        dtpTrandate.Value = Ro.Item("TRANDATE")
        cmbTransactionType.Text = Ro.Item("TRANTYPE").ToString
        txtRemark1.Text = Ro.Item("REMARK1").ToString
        txtRemark2.Text = Ro.Item("REMARK2").ToString
        EditTranNo = Val(Ro.Item("TRANNO").ToString)
        EditRunno = Ro.Item("RUNNO").ToString
        EditDueDays = Val(Ro.Item("DUEDAYS").ToString)



        StrSql = vbCrLf + "  SELECT "
        StrSql += vbCrLf + "  (SELECT CHARGENAME FROM " & cnAdminDb & "..ADDCHARGE WHERE CHARGEID = T.CARDID)AS CHARGENAME"
        StrSql += vbCrLf + "  ,AMOUNT"
        StrSql += vbCrLf + "  FROM " & cnStockDb & "..ACCTRAN AS T"
        StrSql += vbCrLf + "  WHERE BATCHNO = '" & EditBatchno & "' AND PAYMODE = 'AC' AND ACCODE <> '" & _Accode & "'"
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(objAddlCharge.dtGridAddlCharges)
        objAddlCharge.CalcAddlChargeTotal()
        Dim AddlAmt As Decimal = Val(objAddlCharge.gridAddChargeTotal.Rows(0).Cells("AMOUNT").Value.ToString)
        txtAdditionalCharges.Text = IIf(AddlAmt <> 0, Format(AddlAmt, "0.00"), Nothing)

        StrSql = vbCrLf + "  SELECT SUM(AMOUNT) FROM " & cnStockDb & "..ACCTRAN"
        StrSql += vbCrLf + "  WHERE BATCHNO = '" & EditBatchno & "' "
        StrSql += vbCrLf + "  AND PAYMODE = 'CA' AND ACCODE <> '" & _Accode & "'"
        Dim TValue As Decimal = Nothing
        TValue = Val(objGPack.GetSqlValue(StrSql))
        If TValue <> 0 Then txtAdjCash_AMT.Text = Format(TValue, "0.00")

        StrSql = vbCrLf + "  SELECT SUM(CASE WHEN TRANMODE='D' THEN ISNULL(AMOUNT,0) * -1 ELSE ISNULL(AMOUNT,0) END) AMOUNT " ''SUM(AMOUNT)
        StrSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN"
        StrSql += vbCrLf + "  WHERE BATCHNO = '" & EditBatchno & "' "
        StrSql += vbCrLf + "  AND PAYMODE = 'RO' AND ACCODE <> '" & _Accode & "'"
        TValue = Val(objGPack.GetSqlValue(StrSql))
        If TValue <> 0 Then txtAdjRoundOff_AMT.Text = Format(TValue, "0.00")

        StrSql = vbCrLf + "  SELECT TOP 1 TDSPER "
        StrSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN"
        StrSql += vbCrLf + "  WHERE BATCHNO = '" & EditBatchno & "' "
        StrSql += vbCrLf + "  AND ISNULL(TDSPER,0)<>0"
        Dim TdsPer As Decimal = Val(objGPack.GetSqlValue(StrSql))

        StrSql = vbCrLf + "  SELECT TOP 1 TAXPER "
        StrSql += vbCrLf + " FROM " & cnStockDb & "..TAXTRAN"
        StrSql += vbCrLf + "  WHERE BATCHNO = '" & EditBatchno & "' "
        StrSql += vbCrLf + "  AND ISNULL(TAXPER,0)<>0 AND TAXID='SG'"
        Dim SGPer As Decimal = Val(objGPack.GetSqlValue(StrSql))

        StrSql = vbCrLf + "  SELECT TOP 1 TAXPER "
        StrSql += vbCrLf + " FROM " & cnStockDb & "..TAXTRAN"
        StrSql += vbCrLf + "  WHERE BATCHNO = '" & EditBatchno & "' "
        StrSql += vbCrLf + "  AND ISNULL(TAXPER,0)<>0 AND TAXID='CG'"
        Dim CGPer As Decimal = Val(objGPack.GetSqlValue(StrSql))

        StrSql = vbCrLf + "  SELECT TOP 1 TAXPER "
        StrSql += vbCrLf + " FROM " & cnStockDb & "..TAXTRAN"
        StrSql += vbCrLf + "  WHERE BATCHNO = '" & EditBatchno & "' "
        StrSql += vbCrLf + "  AND ISNULL(TAXPER,0)<>0 AND TAXID='IG'"
        Dim IGPer As Decimal = Val(objGPack.GetSqlValue(StrSql))


        StrSql = vbCrLf + "  SELECT "
        StrSql += vbCrLf + "  CHQCARDREF AS BANKNAME"
        StrSql += vbCrLf + "  ,CHQDATE AS [DATE]"
        StrSql += vbCrLf + "  ,CHQCARDNO AS CHEQUENO"
        StrSql += vbCrLf + "  ,AMOUNT "
        StrSql += vbCrLf + "  ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.ACCODE)AS DEFAULTBANK"
        StrSql += vbCrLf + "  FROM " & cnStockDb & "..ACCTRAN AS T"
        StrSql += vbCrLf + "  WHERE BATCHNO = '" & EditBatchno & "'"
        StrSql += vbCrLf + "  AND PAYMODE = 'CH' AND ACCODE <> '" & _Accode & "'"
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(objCheaque.dtGridCheque)
        objCheaque.dtGridCheque.AcceptChanges()
        objCheaque.CalcGridChequeTotal()
        Dim chqAmt As Double = Val(objCheaque.gridChequeTotal.Rows(0).Cells("AMOUNT").Value.ToString)
        txtAdjCheque_AMT.Text = IIf(chqAmt <> 0, Format(chqAmt, "0.00"), Nothing)


        StrSql = vbCrLf + "  SELECT "
        StrSql += vbCrLf + "  I.SNO,I.FLAG"
        If _JobNoEnable = False Then
            If _ORDSTATE_NAME = "LOT" Then
                StrSql += vbCrLf + "  ,JOBNO"
            Else
                StrSql += vbCrLf + "  ,(SELECT TOP 1 SUBSTRING(ORNO,6,20) FROM " & cnAdminDb & "..ORMAST WHERE SNO = (SELECT TOP 1 ORSNO FROM " & cnAdminDb & "..ORIRDETAIL WHERE BATCHNO = I.BATCHNO))AS JOBNO"
            End If
        Else
            StrSql += vbCrLf + "  ,JOBNO"
        End If
        StrSql += vbCrLf + "  ,ME.METALID,ME.METALNAME AS METAL,CA.CATNAME,CA.CATCODE"
        StrSql += vbCrLf + "  ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.OCATCODE)AS ISSCATNAME"
        StrSql += vbCrLf + "  ,PURITY"
        StrSql += vbCrLf + "  ,IM.ITEMNAME AS ITEM,SM.SUBITEMNAME AS SUBITEM,I.ITEMID,I.SUBITEMID,I.PCS,I.GRSWT,I.LESSWT,I.NETWT"
        StrSql += vbCrLf + "  ,I.STONEUNIT AS UNIT,I.WEIGHTUNIT AS CALCMODE"
        StrSql += vbCrLf + "  ,I.GRSNET"
        StrSql += vbCrLf + "  ,I.WASTPER"
        StrSql += vbCrLf + "  ,I.WASTAGE AS WASTAGE"
        StrSql += vbCrLf + "  ,ISNULL(I.ALLOY,0) AS ALLOY"
        StrSql += vbCrLf + "  ,I.TOUCH"
        StrSql += vbCrLf + "  ,I.PUREWT"
        StrSql += vbCrLf + "  ,I.MCGRM,I.MCHARGE AS MC,I.RATE"
        StrSql += vbCrLf + "  ,I.BOARDRATE"
        StrSql += vbCrLf + "  ,(SELECT SUM(STNAMT) FROM " & cnStockDb & ".." & IIf(OMaterialType = MaterialType.Issue, "ISSSTONE", "RECEIPTSTONE") & " WHERE ISSSNO = I.SNO)AS STUDAMT"
        StrSql += vbCrLf + "  ,AMOUNT AS GROSSAMT"
        StrSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),0) VATPER"
        'StrSql += vbCrLf + "  ,CASE WHEN ISNULL(TAX,0) = 0 THEN TDS ELSE TAX END AS VAT"
        StrSql += vbCrLf + "  ,TAX AS VAT"
        StrSql += vbCrLf + "  ,TDS"
        StrSql += vbCrLf + "  ,(SELECT SUM(TAXAMOUNT) FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO=I.BATCHNO AND ISSSNO=I.SNO AND TRANTYPE=I.TRANTYPE AND TAXID='SG')SGST"
        StrSql += vbCrLf + "  ,(SELECT SUM(TAXAMOUNT) FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO=I.BATCHNO AND ISSSNO=I.SNO AND TRANTYPE=I.TRANTYPE AND TAXID='CG')CGST"
        StrSql += vbCrLf + "  ,(SELECT SUM(TAXAMOUNT) FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO=I.BATCHNO AND ISSSNO=I.SNO AND TRANTYPE=I.TRANTYPE AND TAXID='IG')IGST"
        StrSql += vbCrLf + "  ,(SELECT SUM(AMOUNT)  FROM " & cnStockDb & " ..ACCTRAN  WHERE BATCHNO=I.BATCHNO  AND ACCODE='EXDUTY' AND TRANMODE ='D')ED"
        StrSql += vbCrLf + "  ,(SELECT SUM(TAXAMOUNT) FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO=I.BATCHNO AND ISSSNO=I.SNO AND TRANTYPE=I.TRANTYPE AND TAXID='TC')TCS"
        StrSql += vbCrLf + "  ,CONVERT(NUMERIC(15),(SELECT TOP 1  TAXPER  FROM " & cnStockDb & "..TAXTRAN  WHERE BATCHNO =I.BATCHNO ),0)AS EDTAX"
        StrSql += vbCrLf + "  ,REMARK1,REMARK2"
        StrSql += vbCrLf + "  ,(SELECT ORDSTATE_NAME FROM " & cnAdminDb & "..ORDERSTATUS WHERE ORDSTATE_ID = I.ORDSTATE_ID)AS ORDSTATE_NAME"
        StrSql += vbCrLf + "  ,(SELECT SUM(AMOUNT) FROM " & cnStockDb & ".." & IIf(OMaterialType = MaterialType.Issue, "RECEIPTMISC", "RECEIPTMISC") & " WHERE ISSSNO = I.SNO)AS ADDCHARGE,ISNULL(SEIVE,'') AS SEIVE,"
        StrSql += vbCrLf + "  (CASE WHEN CALCON ='G' THEN 'GRS WT'   WHEN CALCON ='P' THEN 'PURE WT'   WHEN CALCON ='N' THEN'NET WT' ELSE '' END)CALCON"
        StrSql += vbCrLf + "  ,DISCOUNT"
        StrSql += vbCrLf + "  ,(SELECT GROUPNAME FROM " & cnAdminDb & "..STONEGROUP WHERE GROUPID = I.STNGRPID)STNGRPNAME,I.APRXAMT,I.APRXTAX,I.RATEFIXED"
        If OMaterialType = MaterialType.Issue Then
            StrSql += vbCrLf + "  FROM " & cnStockDb & "..ISSUE AS I"
        Else
            StrSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT AS I"
        End If
        StrSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = I.CATCODE"
        StrSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..METALMAST AS ME ON ME.METALID = CA.METALID"
        StrSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = I.ITEMID"
        StrSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = I.ITEMID AND SM.SUBITEMID = I.SUBITEMID"
        StrSql += vbCrLf + "  WHERE I.BATCHNO = '" & EditBatchno & "' AND LEN(I.TRANTYPE) > 2"
        Dim dtTranInfo As New DataTable
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dtTranInfo)
        If Not dtTranInfo.Rows.Count > 0 Then Exit Sub
        For Each Ro In dtTranInfo.Rows
            If _JobNoEnable = False Then
                If Ro.Item("JOBNO").ToString <> "" Then
                    If Ro.Item("ORDSTATE_NAME").ToString <> "LOT" Then
                        MsgBox("Job Issue/Receipt Cannot Edit", MsgBoxStyle.Information)
                        btnExit_Click(Me, New EventArgs)
                        Exit Sub
                    End If

                End If
            End If
        Next
        For Each Ro In dtTranInfo.Rows
            If Val(Ro.Item("PCS").ToString) = 0 Then Ro.Item("PCS") = DBNull.Value
            If Val(Ro.Item("GRSWT").ToString) = 0 Then Ro.Item("GRSWT") = DBNull.Value
            If Val(Ro.Item("LESSWT").ToString) = 0 Then Ro.Item("LESSWT") = DBNull.Value
            If Val(Ro.Item("NETWT").ToString) = 0 Then Ro.Item("NETWT") = DBNull.Value
            If Val(Ro.Item("WASTPER").ToString) = 0 Then Ro.Item("WASTPER") = DBNull.Value
            If Val(Ro.Item("WASTAGE").ToString) = 0 Then Ro.Item("WASTAGE") = DBNull.Value
            If Val(Ro.Item("ALLOY").ToString) = 0 Then Ro.Item("ALLOY") = DBNull.Value
            If Val(Ro.Item("TOUCH").ToString) = 0 Then Ro.Item("TOUCH") = DBNull.Value
            If Val(Ro.Item("PUREWT").ToString) = 0 Then Ro.Item("PUREWT") = DBNull.Value
            If Val(Ro.Item("MCGRM").ToString) = 0 Then Ro.Item("MCGRM") = DBNull.Value
            If Val(Ro.Item("MC").ToString) = 0 Then Ro.Item("MC") = DBNull.Value
            If Val(Ro.Item("RATE").ToString) = 0 Then Ro.Item("RATE") = DBNull.Value
            If Val(Ro.Item("GROSSAMT").ToString) = 0 Then Ro.Item("GROSSAMT") = DBNull.Value
            If Val(Ro.Item("VATPER").ToString) = 0 Then Ro.Item("VATPER") = DBNull.Value
            If Val(Ro.Item("VAT").ToString) = 0 Then Ro.Item("VAT") = DBNull.Value
            If Val(Ro.Item("ED").ToString) = 0 Then Ro.Item("ED") = DBNull.Value
            If Val(Ro.Item("EDTAX").ToString) = 0 Then Ro.Item("EDTAX") = DBNull.Value
            If Val(Ro.Item("TCS").ToString) = 0 Then Ro.Item("TCS") = DBNull.Value
            If Val(Ro.Item("DISCOUNT").ToString) = 0 Then Ro.Item("DISCOUNT") = DBNull.Value
            If Val(Ro.Item("APRXAMT").ToString) = 0 Then Ro.Item("APRXAMT") = DBNull.Value
            If Val(Ro.Item("APRXTAX").ToString) = 0 Then Ro.Item("APRXTAX") = DBNull.Value
            If TdsPer <> 0 Then Ro.Item("VATPER") = Val(TdsPer.ToString)
            ObjMaterialDia = New MaterialIssRec(OMaterialType, cmbTransactionType.Text, dtpTrandate.Value, _Accode)
            ObjMaterialDia.BillCostId = CostCenterId
            ObjMaterialDia.Text = "MATERIAL " & cmbTransactionType.Text
            With ObjMaterialDia
                StrSql = vbCrLf + "  SELECT "
                StrSql += vbCrLf + "  IM.ITEMNAME AS ITEM,SM.SUBITEMNAME AS SUBITEM"
                StrSql += vbCrLf + "  ,ST.STNPCS AS PCS,ST.STNWT AS WEIGHT"
                StrSql += vbCrLf + "  ,ST.STONEUNIT AS UNIT,ST.CALCMODE AS CALC,ST.STNRATE AS RATE,ST.STNAMT AS AMOUNT"
                StrSql += vbCrLf + "  ,IM.DIASTONE AS METALID,ISNULL(SEIVE,'') AS SEIVE"
                StrSql += vbCrLf + "  ,CASE WHEN ISNULL(ST.STUDDEDUCT,'Y')='Y' THEN 'YES' ELSE 'NO' END AS STUDDEDUCT"
                If OMaterialType = MaterialType.Receipt Then
                    StrSql += vbCrLf + "  ,CASE WHEN ST.TRANTYPE='RPU' THEN 'PURCHASE' "
                    StrSql += vbCrLf + "  WHEN ST.TRANTYPE='RAP' THEN 'APPROVAL' "
                    StrSql += vbCrLf + "  ELSE 'RECEIPT' "
                    StrSql += vbCrLf + "  END STNTYPE"
                Else
                    StrSql += vbCrLf + "  ,CASE WHEN ST.TRANTYPE='IPU' THEN 'PURCHASE RETURN' "
                    StrSql += vbCrLf + "  WHEN ST.TRANTYPE='IAP' THEN 'APPROVAL' "
                    StrSql += vbCrLf + "  ELSE 'ISSUE' "
                    StrSql += vbCrLf + "  END STNTYPE"
                End If
                StrSql += vbCrLf + "  ,STNGRPID"
                If OMaterialType = MaterialType.Issue Then
                    StrSql += vbCrLf + "  FROM " & cnStockDb & "..ISSSTONE AS ST"
                Else
                    StrSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPTSTONE AS ST"
                End If
                StrSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = ST.STNITEMID"
                StrSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = ST.STNITEMID AND SM.SUBITEMID = ST.STNSUBITEMID"
                StrSql += vbCrLf + "  WHERE ST.ISSSNO = '" & Ro.Item("SNO").ToString & "'"
                Dim dtStone As New DataTable
                Da = New OleDbDataAdapter(StrSql, cn)
                Da.Fill(dtStone)
                For Each RoStn As DataRow In dtStone.Rows
                    Dim RoStnIm As DataRow = .objStone.dtGridStone.NewRow
                    For Each Col As DataColumn In dtStone.Columns
                        RoStnIm.Item(Col.ColumnName) = RoStn.Item(Col.ColumnName)
                    Next
                    .objStone.dtGridStone.Rows.Add(RoStnIm)
                Next
                .objStone.dtGridStone.AcceptChanges()
                .objStone.CalcStoneWtAmount()

                'RAMESH 01.10.11
                StrSql = vbCrLf + " SELECT"
                StrSql += vbCrLf + " 	(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE ISNULL(METALID,'') = A.ALLOYID) ALLOY"
                StrSql += vbCrLf + " 	,WEIGHT"
                StrSql += vbCrLf + " FROM " & cnStockDb & "..ALLOYDETAILS AS A"
                StrSql += vbCrLf + "  WHERE A.ISSSNO = '" & Ro.Item("SNO").ToString & "'"
                Dim dtAlloy As New DataTable
                Da = New OleDbDataAdapter(StrSql, cn)
                Da.Fill(dtAlloy)
                For Each RoAlloy As DataRow In dtAlloy.Rows
                    Dim RoAlloyIm As DataRow = .ObjAlloy.dtGridAlloy.NewRow
                    For Each Col As DataColumn In dtAlloy.Columns
                        RoAlloyIm.Item(Col.ColumnName) = RoAlloy.Item(Col.ColumnName)
                    Next
                    .ObjAlloy.dtGridAlloy.Rows.Add(RoAlloyIm)
                Next
                .ObjAlloy.dtGridAlloy.AcceptChanges()
                .ObjAlloy.CalcAlloyTotalWt()


                StrSql = " SELECT"
                StrSql += " (SELECT MISCNAME FROM " & cnAdminDb & "..MISCCHARGES WHERE MISCID = I.MISCID)AS MISC"
                StrSql += " ,I.AMOUNT"
                If OMaterialType = MaterialType.Issue Then
                    StrSql += " FROM " & cnStockDb & "..ISSMISC AS I"
                Else
                    StrSql += " FROM " & cnStockDb & "..RECEIPTMISC AS I"
                End If
                StrSql += vbCrLf + "  WHERE I.ISSSNO = '" & Ro.Item("SNO").ToString & "'"
                Dim dtMisc As New DataTable
                Da = New OleDbDataAdapter(StrSql, cn)
                Da.Fill(dtMisc)
                For Each RoMisc As DataRow In dtMisc.Rows
                    Dim RoMiscIm As DataRow = .objMisc.dtGridMisc.NewRow
                    For Each Col As DataColumn In dtMisc.Columns
                        RoMiscIm.Item(Col.ColumnName) = RoMisc.Item(Col.ColumnName)
                    Next
                    .objMisc.dtGridMisc.Rows.Add(RoMiscIm)
                Next
                .objMisc.dtGridMisc.AcceptChanges()
                .objMisc.CalcMiscTotalAmount()

                .SrVtTax = SRVTPER
                If Ro.Item("FLAG").ToString = "O" Then
                    .rbtOrnament.Checked = True
                    If _JobNoEnable = False Then
                        .txtOOrdNo.Text = Ro.Item("JOBNO").ToString
                    ElseIf Ro.Item("ORDSTATE_NAME").ToString = "LOT" Then
                        .txtOOrdNo.Text = Ro.Item("JOBNO").ToString
                    Else
                        .txtOOrdNo.Text = Ro.Item("JOBNO").ToString.Replace((GetCostId(cnCostId) & GetCompanyId(strCompanyId)), "")
                    End If

                    .cmbOMetal.Text = Ro.Item("METAL").ToString
                    .cmbOMetal.SelectedValue = Ro.Item("METALID").ToString
                    .cmbOMetal_SelectedValueChanged(Me, New EventArgs)

                    .cmbOCategory.Text = Ro.Item("CATNAME").ToString
                    .cmbOCategory.SelectedValue = Ro.Item("CATCODE").ToString
                    .cmbOCategory_SelectedValueChanged(Me, New EventArgs)

                    .cmbOIssuedCategory.Text = "" & Ro.Item("ISSCATNAME")
                    .cmbOAcPostCategory.Text = "" & Ro.Item("CATNAME")
                    .CmbOPurity.Text = Ro.Item("PURITY").ToString
                    .cmbOItem.Text = Ro.Item("ITEM").ToString
                    .cmbOItem.SelectedValue = Ro.Item("ITEMID")
                    .cmbOItem_SelectedValueChanged(Me, New EventArgs)

                    .cmbOSubItem.Text = Ro.Item("SUBITEM").ToString
                    .txtOPcs_NUM.Text = Ro.Item("PCS").ToString
                    .txtOGrsWt_WET.Text = Ro.Item("GRSWT").ToString
                    .txtOLessWt_WET.Text = Ro.Item("LESSWT").ToString
                    .txtONetWt_WET.Text = Ro.Item("NETWT").ToString
                    .cmbOGrsNet.Text = IIf(Ro.Item("GRSNET").ToString = "G", "GRS WT", "NET WT")
                    .txtOWastPer.Text = Ro.Item("WASTPER").ToString
                    .txtOWast_WET.Text = Ro.Item("WASTAGE").ToString
                    .txtOalloy_WET.Text = Ro.Item("ALLOY").ToString
                    .txtOTouchAMT.Text = Ro.Item("TOUCH").ToString
                    .txtOStudAmt_AMT.Text = Ro.Item("STUDAMT").ToString
                    .txtOPureWt_WET.Text = Ro.Item("PUREWT").ToString
                    .txtOMcGrm_AMT.Text = Ro.Item("MCGRM").ToString
                    .txtOMc_AMT.Text = Ro.Item("MC").ToString
                    .txtORate_OWN.Text = Ro.Item("RATE").ToString
                    .oBoardRate = Val(Ro.Item("BOARDRATE").ToString)
                    .txtOAddlCharge_AMT.Text = Ro.Item("ADDCHARGE").ToString
                    .txtOGrsAmt_AMT.Text = Ro.Item("GROSSAMT").ToString
                    .txtOVatPer_PER.Text = Ro.Item("VATPER").ToString
                    .txtODisc.Text = Ro.Item("DISCOUNT").ToString
                    ''CHECKTDS
                    If Val(Ro.Item("TDS").ToString) > 0 Then
                        .txtOVat_AMT.Text = Ro.Item("TDS").ToString
                    Else
                        .txtOVat_AMT.Text = Ro.Item("VAT").ToString
                        If UCase(cmbTransactionType.Text) <> "PURCHASE RETURN" And UCase(cmbTransactionType.Text) <> "INTERNAL TRANSFER" And UCase(cmbTransactionType.Text) <> "APPROVAL RECEIPT" Then
                            .txtOVat_AMT.Text = 0
                        End If
                    End If
                    If Val(Ro.Item("SGST").ToString) > 0 Then
                        .txtOSG_AMT.Text = Ro.Item("SGST").ToString
                        .txtOCG_AMT.Text = Ro.Item("CGST").ToString
                        .txtOSgst_AMT.Text = SGPer
                        .txtOCgst_AMT.Text = CGPer
                    ElseIf Val(Ro.Item("IGST").ToString) > 0 Then
                        .txtOIG_AMT.Text = Ro.Item("IGST").ToString
                        .txtOIgst_AMT.Text = IGPer
                    End If

                    .txtOED_AMT.Text = Ro.Item("ED").ToString
                    .txtOTCS_AMT.Text = Ro.Item("TCS").ToString
                    .txtOedPer_AMT.Text = Ro.Item("EDTAX").ToString
                    .CalcONetAmt()
                    .txtORemark1.Text = Ro.Item("REMARK1").ToString
                    .txtORemark2.Text = Ro.Item("REMARK2").ToString
                    .cmbOProcess.Text = Ro.Item("ORDSTATE_NAME").ToString
                    '.txtOVat_AMT.Text = Ro.Item("VAT").ToString
                    .cmbOcalcon.Text = Ro.Item("CALCON").ToString
                    .txtOedPer_AMT.Text = Ro.Item("EDTAX").ToString
                    .txtOED_AMT.Text = Ro.Item("ED").ToString
                    .txtOVatPer_PER.Text = Ro.Item("VATPER").ToString
                    '.txtOVat_AMT.Text = Ro.Item("VAT").ToString
                    ''CHECKTDS
                    If Val(Ro.Item("TDS").ToString) > 0 Then
                        .txtOVat_AMT.Text = Ro.Item("TDS").ToString
                    Else
                        .txtOVat_AMT.Text = Ro.Item("VAT").ToString
                        If UCase(cmbTransactionType.Text) <> "PURCHASE RETURN" And UCase(cmbTransactionType.Text) <> "INTERNAL TRANSFER" And UCase(cmbTransactionType.Text) <> "APPROVAL RECEIPT" Then
                            .txtOVat_AMT.Text = 0
                        End If
                    End If
                    .txtOAprxAmount_AMT.Text = Ro.Item("APRXAMT").ToString
                    .txtOAprxTaxAmt_AMT.Text = Ro.Item("APRXTAX").ToString
                    If Ro.Item("RATEFIXED").ToString = "Y" Then
                        .chkORateFixed.Checked = True
                    Else
                        .chkORateFixed.Checked = False
                    End If
                ElseIf Ro.Item("FLAG").ToString = "M" Then
                    .rbtMetal.Checked = True
                    If _JobNoEnable = False Then
                        Ro.Item("JOBNO") = .txtMOrdNo.Text
                    ElseIf Ro.Item("ORDSTATE_NAME").ToString = "LOT" Then
                        Ro.Item("JOBNO") = .txtMOrdNo.Text
                    Else
                        .txtMOrdNo.Text = Ro.Item("JOBNO").ToString.Replace((GetCostId(cnCostId) & GetCompanyId(strCompanyId)), "")
                    End If

                    .cmbMMetal.Text = Ro.Item("METAL").ToString
                    .cmbMMetal.SelectedValue = Ro.Item("METALID").ToString
                    .cmbMMetal_SelectedValueChanged(Me, New EventArgs)
                    .cmbMCategory.SelectedValue = Ro.Item("CATCODE").ToString
                    .cmbMCategory.Text = Ro.Item("CATNAME").ToString
                    .cmbMCategory_SelectedValueChanged(Me, New EventArgs)
                    .cmbMIssuedCategory.Text = Ro.Item("ISSCATNAME").ToString
                    .cmbMAcPostCategory.Text = Ro.Item("CATNAME").ToString
                    .CmbMPurity.Text = Ro.Item("PURITY").ToString
                    .txtMPcs_NUM.Text = Ro.Item("PCS").ToString
                    .txtMGrsWt_WET.Text = Ro.Item("GRSWT").ToString
                    .txtMNetWt_WET.Text = Ro.Item("NETWT").ToString
                    .cmbMGrsNet.Text = IIf(Ro.Item("GRSNET").ToString = "G", "GRS WT", "NET WT")
                    .txtMWastPER.Text = Ro.Item("WASTPER").ToString
                    .txtMWast_WET.Text = Ro.Item("WASTAGE").ToString
                    .txtMAlloy_WET.Text = Ro.Item("ALLOY").ToString
                    .txtMTouchAMT.Text = Ro.Item("TOUCH").ToString
                    .txtMPureWt_WET.Text = Ro.Item("PUREWT").ToString
                    .txtMRate_OWN.Text = Ro.Item("RATE").ToString
                    .oBoardRate = Val(Ro.Item("BOARDRATE").ToString)
                    .txtMAddlCharge_AMT.Text = Ro.Item("ADDCHARGE").ToString
                    .txtMGrsAmt_AMT.Text = Ro.Item("GROSSAMT").ToString
                    .txtMVatPer_PER.Text = Ro.Item("VATPER").ToString
                    '.txtMVat_AMT.Text = Ro.Item("VAT").ToString
                    If Val(Ro.Item("TDS").ToString) > 0 Then
                        .txtMVat_AMT.Text = Ro.Item("TDS").ToString
                    Else
                        .txtMVat_AMT.Text = Ro.Item("VAT").ToString
                        If UCase(cmbTransactionType.Text) <> "PURCHASE RETURN" And UCase(cmbTransactionType.Text) <> "INTERNAL TRANSFER" And UCase(cmbTransactionType.Text) <> "APPROVAL RECEIPT" Then
                            .txtMVat_AMT.Text = 0
                        End If
                    End If
                    .txtMTCS_AMT.Text = Ro.Item("TCS").ToString
                    If Val(Ro.Item("SGST").ToString) > 0 Then
                        .txtMSG_AMT.Text = Ro.Item("SGST").ToString
                        .txtMCG_AMT.Text = Ro.Item("CGST").ToString
                        .txtMSgst_AMT.Text = SGPer
                        .txtMCgst_AMT.Text = CGPer
                    ElseIf Val(Ro.Item("IGST").ToString) > 0 Then
                        .txtMIG_AMT.Text = Ro.Item("IGST").ToString
                        .txtMIgst_AMT.Text = IGPer
                    End If
                    .CalcMNetAmt()
                    .txtMRemark1.Text = Ro.Item("REMARK1").ToString
                    .txtMRemark2.Text = Ro.Item("REMARK2").ToString
                    .cmbMProcess.Text = Ro.Item("ORDSTATE_NAME").ToString
                    .txtMAprxAmount_AMT.Text = Ro.Item("APRXAMT").ToString
                    .txtMAprxTaxAmt_AMT.Text = Ro.Item("APRXTAX").ToString
                    If Ro.Item("RATEFIXED").ToString = "Y" Then
                        .chkMRateFixed.Checked = True
                    Else
                        .chkMRateFixed.Checked = False
                    End If
                ElseIf Ro.Item("FLAG").ToString = "T" Then
                    .rbtStone.Checked = True
                    If _JobNoEnable = False Then
                        .txtSOrdNo.Text = Ro.Item("JOBNO").ToString
                    ElseIf Ro.Item("ORDSTATE_NAME").ToString = "LOT" Then
                        .txtSOrdNo.Text = Ro.Item("JOBNO").ToString
                    Else
                        .txtSOrdNo.Text = Ro.Item("JOBNO").ToString.Replace((GetCostId(cnCostId) & GetCompanyId(strCompanyId)), "")
                    End If
                    .cmbSMetal.Text = Ro.Item("METAL").ToString
                    .cmbSMetal.SelectedValue = Ro.Item("METALID").ToString
                    .cmbSMetal_SelectedValueChanged(Me, New EventArgs)
                    .cmbSCategory.Text = Ro.Item("CATNAME").ToString
                    .cmbSCategory.SelectedValue = Ro.Item("CATCODE").ToString
                    .cmbSCategory_SelectedValueChanged(Me, New EventArgs)
                    .cmbSIssuedCategory.Text = Ro.Item("ISSCATNAME").ToString
                    .cmbSAcPostCategory.Text = Ro.Item("CATNAME").ToString
                    .CmbSPurity.Text = Ro.Item("PURITY").ToString
                    .cmbSItem.Text = Ro.Item("ITEM").ToString
                    .cmbSItem.SelectedValue = Ro.Item("ITEMID")
                    .cmbSItem_SelectedValueChanged(Me, New EventArgs)
                    .cmbSSubItem.Text = Ro.Item("SUBITEM").ToString
                    .txtSPcs_NUM.Text = Ro.Item("PCS").ToString
                    .txtSGrsWt_WET.Text = Ro.Item("GRSWT").ToString
                    .txtSGrsWt_WET.Text = Ro.Item("NETWT").ToString
                    .cmbSUnit.Text = Ro.Item("UNIT").ToString
                    .cmbSCalcMode.Text = Ro.Item("CALCMODE").ToString
                    .txtSGrsWt_WET.Text = Ro.Item("PUREWT").ToString
                    .txtSRate_OWN.Text = Ro.Item("RATE").ToString
                    .oBoardRate = Val(Ro.Item("BOARDRATE").ToString)
                    .txtSAddlCharge_AMT.Text = Ro.Item("ADDCHARGE").ToString
                    .txtSGrsAmt_AMT.Text = Ro.Item("GROSSAMT").ToString
                    .txtSVatPer_PER.Text = Ro.Item("VATPER").ToString
                    .txtStnGrpName.Text = Ro.Item("STNGRPNAME").ToString
                    '.txtSVat_AMT.Text = Ro.Item("VAT").ToString
                    If Val(Ro.Item("TDS").ToString) > 0 Then
                        .txtSVat_AMT.Text = Ro.Item("TDS").ToString
                    Else
                        .txtSVat_AMT.Text = Ro.Item("VAT").ToString
                        If UCase(cmbTransactionType.Text) <> "PURCHASE RETURN" And UCase(cmbTransactionType.Text) <> "INTERNAL TRANSFER" And UCase(cmbTransactionType.Text) <> "APPROVAL RECEIPT" Then
                            .txtSVat_AMT.Text = 0
                        End If
                    End If
                    .txtSTCS_AMT.Text = Ro.Item("TCS").ToString
                    If Val(Ro.Item("SGST").ToString) > 0 Then
                        .txtSSG_AMT.Text = Ro.Item("SGST").ToString
                        .txtSCG_AMT.Text = Ro.Item("CGST").ToString
                        .txtSSgst_WET.Text = SGPer
                        .txtSCgst_WET.Text = CGPer
                    ElseIf Val(Ro.Item("IGST").ToString) > 0 Then
                        .txtSIG_AMT.Text = Ro.Item("IGST").ToString
                        .txtSIgst_WET.Text = IGPer
                    End If
                    .CalcSNetAmt()
                    .txtSRemark1.Text = Ro.Item("REMARK1").ToString
                    .txtSRemark2.Text = Ro.Item("REMARK2").ToString
                    .cmbSProcess.Text = Ro.Item("ORDSTATE_NAME").ToString
                    .cmbSSeive.Text = Ro.Item("SEIVE").ToString
                    If Ro.Item("RATEFIXED").ToString = "Y" Then
                        .chkSRateFixed.Checked = True
                    Else
                        .chkSRateFixed.Checked = False
                    End If
                ElseIf Ro.Item("FLAG").ToString = "H" Then
                    .rbtOthers.Checked = True
                    .cmbOthMetal.Text = Ro.Item("METAL").ToString
                    .cmbOthMetal.SelectedValue = Ro.Item("METALID").ToString
                    .cmbOthMetal_SelectedValueChanged(Me, New EventArgs)
                    .cmbOthCategory.Text = Ro.Item("CATNAME").ToString
                    .cmbOthCategory.SelectedValue = Ro.Item("CATCODE").ToString
                    .cmbOthCategory_SelectedValueChanged(Me, New EventArgs)
                    .cmbOthItem.Text = Ro.Item("ITEM").ToString
                    .cmbOthItem.SelectedValue = Ro.Item("ITEMID")
                    .cmbOthItem_SelectedValueChanged(Me, New EventArgs)

                    .cmbOthSubItem.Text = Ro.Item("SUBITEM").ToString
                    .txtOthPcs_NUM.Text = Ro.Item("PCS").ToString
                    .txtOthRate_AMT.Text = Ro.Item("RATE").ToString
                    .oBoardRate = Val(Ro.Item("BOARDRATE").ToString)
                    .txtOthGrsAmt_AMT.Text = Ro.Item("GROSSAMT").ToString
                    .txtOthVatPer_PER.Text = Ro.Item("VATPER").ToString
                    '.txtOthVat_AMT.Text = Ro.Item("VAT").ToString
                    If Val(Ro.Item("TDS").ToString) > 0 Then
                        .txtOthVat_AMT.Text = Ro.Item("TDS").ToString
                    Else
                        .txtOthVat_AMT.Text = Ro.Item("VAT").ToString
                        If UCase(cmbTransactionType.Text) <> "PURCHASE RETURN" And UCase(cmbTransactionType.Text) <> "INTERNAL TRANSFER" And UCase(cmbTransactionType.Text) <> "APPROVAL RECEIPT" Then
                            .txtOthVat_AMT.Text = 0
                        End If
                    End If
                    If Val(Ro.Item("SGST").ToString) > 0 Then
                        .txtOthSG_AMT.Text = Ro.Item("SGST").ToString
                        .txtOthCG_AMT.Text = Ro.Item("CGST").ToString
                        .txtOthSgst_AMT.Text = SGPer
                        .txtOthCgst_AMT.Text = CGPer
                    ElseIf Val(Ro.Item("IGST").ToString) > 0 Then
                        .txtOthIG_AMT.Text = Ro.Item("IGST").ToString
                        .txtOthIgst_AMT.Text = IGPer
                    End If
                    .CalcOthNetAmt()
                    .txtOthRemark1.Text = Ro.Item("REMARK1").ToString
                    .txtOthRemark2.Text = Ro.Item("REMARK2").ToString
                Else
                    MsgBox("Invalid Flag value in flag column", MsgBoxStyle.Information)
                    btnExit_Click(Me, New EventArgs)
                    Exit Sub
                End If

                If UCase(cmbTransactionType.Text) = "INTERNAL TRANSFER" Then
                    If .INTERNALTRANSFER_NONTAXABLE = True Then

                        .txtOSgst_AMT.Text = ""
                        .txtOCgst_AMT.Text = ""
                        .txtOIgst_AMT.Text = ""
                        .txtOSG_AMT.Text = ""
                        .txtOCG_AMT.Text = ""
                        .txtOIG_AMT.Text = ""

                        .txtMSgst_AMT.Text = ""
                        .txtMCgst_AMT.Text = ""
                        .txtMIgst_AMT.Text = ""
                        .txtMSG_AMT.Text = ""
                        .txtMCG_AMT.Text = ""
                        .txtMIG_AMT.Text = ""


                        .txtSSgst_WET.Text = ""
                        .txtSCgst_WET.Text = ""
                        .txtSIgst_WET.Text = ""
                        .txtSSG_AMT.Text = ""
                        .txtSCG_AMT.Text = ""
                        .txtSIG_AMT.Text = ""


                        .txtOthSgst_AMT.Text = ""
                        .txtOthCgst_AMT.Text = ""
                        .txtOthIgst_AMT.Text = ""
                        .txtOthSG_AMT.Text = ""
                        .txtOthCG_AMT.Text = ""
                        .txtOthIG_AMT.Text = ""

                    End If
                End If

            End With
            LoadTransaction(ObjMaterialDia)
        Next
        btnSave.Enabled = True
        txtCreditDays_NUM.Text = IIf(EditDueDays <> 0, EditDueDays, "")
    End Sub

    Private Sub btnNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNew.Click
        objGPack.TextClear(Me)
        ClearTran()
        xomsNoflag = False
        dtpEstdate_OWN.Value = GetEntryDate(GetServerDate, , ACCENTRY_DATE)
        txtEstNo_NUM.Enabled = True
        txttransferno.Enabled = True
        OnlyMc = False
        InterStateBill = False
        objCheaque = New frmChequeAdj
        objAddlCharge = New AddlChargesDia
        objProcessType = New frmProcessTypeSelection
        xOProcessname = ""
        xMProcessname = ""
        xSProcessname = ""
        xOTdsname = Nothing
        xMTdsName = Nothing
        xSTdsname = Nothing
        _Tdsaccode = ""
        dtCatBalance.Visible = False
        _JobNo = False
        MobileNo = ""
        Refresh = True
        TdsPer = Nothing
        TranNo = Nothing
        BatchNo = Nothing
        Remark1 = Nothing
        Remark2 = Nothing
        lotNo = 0
        gboxOrder.Visible = False
        chkOrder.Checked = False
        gboxBulk.Visible = False
        chkBulk.Checked = False
        chkMulti.Checked = False
        ExitToolStrip = False
        dgvOrderDet.DataSource = Nothing
        MdtOdDet.Clear()
        TagNos = ""
        CatNames = ""
        If GetAdmindbSoftValue("COSTCENTRE", "N") = "Y" Then
            cmbCostCentre.Enabled = True
            If MIMR_EDITCOSTCENTRE = True Then
                cmbCostCentre.Text = cnCostName
                cmbCostCentre.Enabled = True
            Else
                cmbCostCentre.Text = cnCostName
                cmbCostCentre.Enabled = False
            End If
        Else
            cmbCostCentre.Enabled = False
        End If
        LoadTransactionType()
        If OMaterialType = MaterialType.Receipt And Lotautopost Then lblLot.Visible = True Else lblLot.Visible = False
        If cmbCostCentre.Enabled Then cmbCostCentre.Select() Else Me.SelectNextControl(cmbCostCentre, True, True, True, True)
    End Sub

    Private Sub Combo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = Keys.Enter Then
            If CType(sender, ComboBox).FindStringExact(CType(sender, ComboBox).SelectedText) <> -1 Then
                SendKeys.Send("{TAB}")
            End If
        End If
    End Sub

    Private Sub OriDetail(ByVal ObjMaterialDia As MaterialIssRec, ByVal _Index As Integer)
        With ObjMaterialDia
            'ORIRDETAIL 
            If ObjMaterialDia.dgvOrder.Rows.Count > 0 Then
                dgvOrderDet.DataSource = Nothing
                For k As Integer = 0 To ObjMaterialDia.dgvOrder.Rows.Count - 1
                    Dim mdr As DataRow
                    If MdtOdDet.Columns.Contains(ObjMaterialDia.dgvOrder.Rows(k).Cells("ORSNO").Value.ToString) Then Continue For
                    mdr = MdtOdDet.NewRow
                    mdr("ORSNO") = ObjMaterialDia.dgvOrder.Rows(k).Cells("ORSNO").Value.ToString
                    mdr("ORNO") = ObjMaterialDia.dgvOrder.Rows(k).Cells("ORNO").Value.ToString
                    'mdr("GRSWT") = ObjMaterialDia.dgvOrder.Rows(k).Cells("GRSWT").Value.ToString
                    'mdr("NETWT") = ObjMaterialDia.dgvOrder.Rows(k).Cells("NETWT").Value.ToString
                    'FOR NSB
                    If .rbtOrnament.Checked Then
                        mdr("GRSWT") = Val(ObjMaterialDia.txtOGrsWt_WET.Text.ToString)
                        mdr("NETWT") = Val(ObjMaterialDia.txtONetWt_WET.Text.ToString)
                        mdr("WASTAGE") = Val(ObjMaterialDia.txtOWast_WET.Text.ToString)
                        mdr("MC") = Val(ObjMaterialDia.txtOMc_AMT.Text.ToString)
                    ElseIf .rbtMetal.Checked Then
                        mdr("GRSWT") = Val(ObjMaterialDia.txtMGrsWt_WET.Text.ToString)
                        mdr("NETWT") = Val(ObjMaterialDia.txtMNetWt_WET.Text.ToString)
                        mdr("WASTAGE") = Val(ObjMaterialDia.txtMWast_WET.Text.ToString)
                        mdr("MC") = Val(ObjMaterialDia.txtMMc_AMT.Text.ToString)
                    ElseIf .rbtStone.Checked Then
                        mdr("GRSWT") = Val(ObjMaterialDia.txtSGrsWt_WET.Text.ToString)
                        mdr("NETWT") = Val(ObjMaterialDia.txtSGrsWt_WET.Text.ToString)
                    End If
                    mdr("ROWINDEX") = _Index
                    MdtOdDet.Rows.Add(mdr)
                Next
                dgvOrderDet.DataSource = MdtOdDet
            End If
            'dgvOrderDet.DataSource = ObjMaterialDia.dgvOrder.DataSource
            If dgvOrderDet.Rows.Count > 0 Then
                dgvOrderDet.Columns("ORSNO").Visible = False
                dgvOrderDet.Columns("NETWT").Visible = False
                dgvOrderDet.Columns("WASTAGE").Visible = False
                dgvOrderDet.Columns("MC").Visible = False
                dgvOrderDet.Columns("ROWINDEX").Visible = False
                dgvOrderDet.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
                dgvOrderDet.Visible = True
                dgvOrderDet.Enabled = True
                dgvOrderDet.BringToFront()
            End If
        End With
    End Sub

    Private Sub LoadTransaction(ByVal ObjMaterialDia As MaterialIssRec)
        With ObjMaterialDia
            Dim index As Integer = Nothing
            If .oEditRowIndex = -1 Then
                For cnt As Integer = 0 To DtTran.Rows.Count - 1
                    If DtTran.Rows(cnt).RowState <> DataRowState.Deleted Then
                        If DtTran.Rows(cnt).Item("TRANTYPE").ToString = "" Then
                            index = cnt
                            DtTran.Rows.Add()
                            Exit For
                        End If
                    End If
                Next
            Else
                index = .oEditRowIndex
                'For Each ro As DataRow In dtStoneDetails.Rows
                '    If ro("KEYNO") = Val(DgvTran.Rows(index).Cells("KEYNO").Value.ToString) Then
                '        ro.Delete()
                '    End If
                'Next
                'dtStoneDetails.AcceptChanges()
            End If
            'ORIRDETAIL 
            'dgvOrderDet.DataSource = Nothing
            'If ObjMaterialDia.dgvOrder.Rows.Count > 0 Then
            '    For k As Integer = 0 To ObjMaterialDia.dgvOrder.Rows.Count - 1
            '        Dim mdr As DataRow
            '        If MdtOdDet.Columns.Contains(ObjMaterialDia.dgvOrder.Rows(k).Cells("ORSNO").Value.ToString) Then Continue For
            '        mdr = MdtOdDet.NewRow
            '        mdr("ORSNO") = ObjMaterialDia.dgvOrder.Rows(k).Cells("ORSNO").Value.ToString
            '        mdr("ORNO") = ObjMaterialDia.dgvOrder.Rows(k).Cells("ORNO").Value.ToString
            '        'mdr("GRSWT") = ObjMaterialDia.dgvOrder.Rows(k).Cells("GRSWT").Value.ToString
            '        'mdr("NETWT") = ObjMaterialDia.dgvOrder.Rows(k).Cells("NETWT").Value.ToString
            '        'FOR NSB
            '        If .rbtOrnament.Checked Then
            '            mdr("GRSWT") = Val(ObjMaterialDia.txtOGrsWt_WET.Text.ToString)
            '            mdr("NETWT") = Val(ObjMaterialDia.txtONetWt_WET.Text.ToString)
            '            mdr("WASTAGE") = Val(ObjMaterialDia.txtOWast_WET.Text.ToString)
            '            mdr("MC") = Val(ObjMaterialDia.txtOMc_AMT.Text.ToString)
            '        ElseIf .rbtMetal.Checked Then
            '            mdr("GRSWT") = Val(ObjMaterialDia.txtMGrsWt_WET.Text.ToString)
            '            mdr("NETWT") = Val(ObjMaterialDia.txtMNetWt_WET.Text.ToString)
            '            mdr("WASTAGE") = Val(ObjMaterialDia.txtMWast_WET.Text.ToString)
            '            mdr("MC") = Val(ObjMaterialDia.txtMMc_AMT.Text.ToString)
            '        ElseIf .rbtStone.Checked Then
            '            mdr("GRSWT") = Val(ObjMaterialDia.txtSGrsWt_WET.Text.ToString)
            '            mdr("NETWT") = Val(ObjMaterialDia.txtSGrsWt_WET.Text.ToString)
            '        End If
            '        MdtOdDet.Rows.Add(mdr)
            '    Next
            '    dgvOrderDet.DataSource = MdtOdDet
            'End If
            ''dgvOrderDet.DataSource = ObjMaterialDia.dgvOrder.DataSource
            'If dgvOrderDet.Rows.Count > 0 Then
            '    dgvOrderDet.Columns("ORSNO").Visible = False
            '    dgvOrderDet.Columns("NETWT").Visible = False
            '    dgvOrderDet.Columns("WASTAGE").Visible = False
            '    dgvOrderDet.Columns("MC").Visible = False
            '    dgvOrderDet.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            '    dgvOrderDet.Visible = True
            '    dgvOrderDet.Enabled = True
            '    dgvOrderDet.BringToFront()
            'End If
            'ABOVE LINE COMMENT ON 05-SEP-2018
            OriDetail(ObjMaterialDia, index)

            Dim StateId As Integer = Val(objGPack.GetSqlValue("SELECT STATEID FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbAcName.Text & "'").ToString)
            If StateId = 0 Then StateId = CompanyStateId
            If StateId <> CompanyStateId Then
                InterStateBill = True
            End If
            OnlyMc = .McBill
            Dim Desc As String = Nothing
            DtTran.Rows(index).Item("ROWINDEX") = index
            If .rbtOrnament.Checked Then
                DtTran.Rows(index).Item("METISSREC") = ObjMaterialDia
                DtTran.Rows(index).Item("TYPE") = "O"
                DtTran.Rows(index).Item("TRANTYPE") = cmbTransactionType.Text
                If _JobNoEnable = True Then
                    DtTran.Rows(index).Item("JOBNO") = GetCostId(cnCostId) & GetCompanyId(strCompanyId) & .txtOOrdNo.Text
                Else
                    DtTran.Rows(index).Item("JOBNO") = .txtOOrdNo.Text
                End If
                Desc = .cmbOCategory.Text
                If .cmbOItem.Text <> "" Then Desc += vbCrLf + .cmbOItem.Text
                DtTran.Rows(index).Item("DESCRIPTION") = Desc
                DtTran.Rows(index).Item("METAL") = .cmbOMetal.Text
                DtTran.Rows(index).Item("CATNAME") = .cmbOCategory.Text
                DtTran.Rows(index).Item("ACCATNAME") = .cmbOAcPostCategory.Text
                DtTran.Rows(index).Item("ISSCATNAME") = .cmbOIssuedCategory.Text
                DtTran.Rows(index).Item("PURITY") = IIf(Val(.CmbOPurity.Text) <> 0, Val(.CmbOPurity.Text), DBNull.Value)
                DtTran.Rows(index).Item("ITEM") = .cmbOItem.Text
                DtTran.Rows(index).Item("SUBITEM") = .cmbOSubItem.Text
                DtTran.Rows(index).Item("PCS") = IIf(Val(.txtOPcs_NUM.Text) <> 0, Val(.txtOPcs_NUM.Text), DBNull.Value)
                DtTran.Rows(index).Item("GRSWT") = IIf(Val(.txtOGrsWt_WET.Text) <> 0, Val(.txtOGrsWt_WET.Text), DBNull.Value)
                DtTran.Rows(index).Item("LESSWT") = IIf(Val(.txtOLessWt_WET.Text) <> 0, Val(.txtOLessWt_WET.Text), DBNull.Value)
                DtTran.Rows(index).Item("NETWT") = IIf(Val(.txtONetWt_WET.Text) <> 0, Val(.txtONetWt_WET.Text), DBNull.Value)
                DtTran.Rows(index).Item("GRSNET") = .cmbOGrsNet.Text
                DtTran.Rows(index).Item("WASTPER") = IIf(Val(.txtOWastPer.Text) <> 0, Val(.txtOWastPer.Text), DBNull.Value)
                DtTran.Rows(index).Item("WASTAGE") = IIf(Val(.txtOWast_WET.Text) <> 0, Val(.txtOWast_WET.Text), DBNull.Value)
                DtTran.Rows(index).Item("ALLOY") = IIf(Val(.txtOalloy_WET.Text) <> 0, Val(.txtOalloy_WET.Text), DBNull.Value)
                DtTran.Rows(index).Item("TOUCH") = IIf(Val(.txtOTouchAMT.Text) <> 0, Val(.txtOTouchAMT.Text), DBNull.Value)
                DtTran.Rows(index).Item("PUREWT") = IIf(Val(.txtOPureWt_WET.Text) <> 0, Val(.txtOPureWt_WET.Text), DBNull.Value)
                DtTran.Rows(index).Item("MCGRM") = IIf(Val(.txtOMcGrm_AMT.Text) <> 0, Val(.txtOMcGrm_AMT.Text), DBNull.Value)
                DtTran.Rows(index).Item("MC") = IIf(Val(.txtOMc_AMT.Text) <> 0, Val(.txtOMc_AMT.Text), DBNull.Value)
                DtTran.Rows(index).Item("RATE") = IIf(Val(.txtORate_OWN.Text) <> 0, Val(.txtORate_OWN.Text), DBNull.Value)
                DtTran.Rows(index).Item("BOARDRATE") = IIf(Val(.oBoardRate) <> 0, Val(.oBoardRate), DBNull.Value)
                DtTran.Rows(index).Item("STUDAMT") = IIf(Val(.txtOStudAmt_AMT.Text) <> 0, Val(.txtOStudAmt_AMT.Text), DBNull.Value)
                DtTran.Rows(index).Item("GROSSAMT") = IIf(Val(.txtOGrsAmt_AMT.Text) <> 0, Val(.txtOGrsAmt_AMT.Text), DBNull.Value) 'CHECKTDS

                DtTran.Rows(index).Item("VATPER") = IIf(Val(.txtOVatPer_PER.Text) <> 0, Val(.txtOVatPer_PER.Text), DBNull.Value)
                DtTran.Rows(index).Item("VAT") = IIf(Val(.txtOVat_AMT.Text) <> 0, Val(.txtOVat_AMT.Text), DBNull.Value)
                DtTran.Rows(index).Item("AMOUNT") = IIf(Val(.txtOAmount_AMT.Text) <> 0, Val(.txtOAmount_AMT.Text), DBNull.Value)
                DtTran.Rows(index).Item("SGSTPER") = IIf(Val(.txtOSgst_AMT.Text) <> 0, Val(.txtOSgst_AMT.Text), DBNull.Value)
                DtTran.Rows(index).Item("CGSTPER") = IIf(Val(.txtOCgst_AMT.Text) <> 0, Val(.txtOCgst_AMT.Text), DBNull.Value)
                DtTran.Rows(index).Item("IGSTPER") = IIf(Val(.txtOIgst_AMT.Text) <> 0, Val(.txtOIgst_AMT.Text), DBNull.Value)
                DtTran.Rows(index).Item("SGST") = IIf(Val(.txtOSG_AMT.Text) <> 0, Val(.txtOSG_AMT.Text), DBNull.Value)
                DtTran.Rows(index).Item("CGST") = IIf(Val(.txtOCG_AMT.Text) <> 0, Val(.txtOCG_AMT.Text), DBNull.Value)
                DtTran.Rows(index).Item("IGST") = IIf(Val(.txtOIG_AMT.Text) <> 0, Val(.txtOIG_AMT.Text), DBNull.Value)
                DtTran.Rows(index).Item("GST") = Val(DtTran.Rows(index).Item("SGST").ToString) + Val(DtTran.Rows(index).Item("CGST").ToString) + Val(DtTran.Rows(index).Item("IGST").ToString)
                DtTran.Rows(index).Item("REMARK1") = .txtORemark1.Text
                DtTran.Rows(index).Item("REMARK2") = .txtORemark2.Text
                DtTran.Rows(index).Item("ORDSTATE_NAME") = .cmbOProcess.Text
                DtTran.Rows(index).Item("ADDCHARGE") = Val(.txtOAddlCharge_AMT.Text)
                DtTran.Rows(index).Item("SEIVE") = ""
                DtTran.Rows(index).Item("OGRSWT") = .ONetwt
                DtTran.Rows(index).Item("ONETWT") = .OGrswt
                DtTran.Rows(index).Item("ORSNO") = .ORSNO
                DtTran.Rows(index).Item("RESNO") = .RESNO
                DtTran.Rows(index).Item("TAGNO") = .txtTagNo.Text.ToString.Trim
                DtTran.Rows(index).Item("EDPER") = IIf(Val(.txtOedPer_AMT.Text) <> 0, Val(.txtOedPer_AMT.Text), DBNull.Value)
                DtTran.Rows(index).Item("ED") = IIf(Val(.txtOED_AMT.Text) <> 0, Val(.txtOED_AMT.Text), DBNull.Value)
                DtTran.Rows(index).Item("TCS") = IIf(Val(.txtOTCS_AMT.Text) <> 0, Val(.txtOTCS_AMT.Text), DBNull.Value)
                DtTran.Rows(index).Item("CALCON") = .cmbOcalcon.Text
                DtTran.Rows(index).Item("VATPER") = IIf(Val(.txtOVatPer_PER.Text) <> 0, Val(.txtOVatPer_PER.Text), DBNull.Value)
                DtTran.Rows(index).Item("DISCOUNT") = IIf(Val(.txtODisc.Text) <> 0, Val(.txtODisc.Text), DBNull.Value)
                DtTran.Rows(index).Item("APPROXAMT") = IIf(Val(.txtOAprxAmount_AMT.Text) <> 0, Val(.txtOAprxAmount_AMT.Text), DBNull.Value)
                DtTran.Rows(index).Item("APPROXTAX") = IIf(Val(.txtOAprxTaxAmt_AMT.Text) <> 0, Val(.txtOAprxTaxAmt_AMT.Text), DBNull.Value)
                DtTran.Rows(index).Item("RATEFIXED") = IIf(.chkORateFixed.Checked, "Y", "N")
                If Lotautopost And OMaterialType = MaterialType.Receipt _
                And cmbTransactionType.Text <> "PURCHASE[APPROVAL]" _
                And cmbTransactionType.Text <> "APPROVAL RECEIPT" Then
                    DtTran.Rows(index).Item("HALLMARK") = IIf(.CmbOHallmark.Text = "YES", "Y", "N")
                Else
                    DtTran.Rows(index).Item("HALLMARK") = "N"
                End If
                DtTran.Rows(index).Item("TDSACCODE") = IIf(.txtotdsaccode.Text <> "", .txtotdsaccode.Text, DBNull.Value)
                DgvTran.Rows(index).DefaultCellStyle.WrapMode = DataGridViewTriState.True
                If .txtTagNo.Text.ToString.Trim <> "" Then
                    TagNos += .txtTagNo.Text.ToString.Trim + ","
                End If
                If .cmbOCategory.Text.ToString.Trim <> "" Then
                    CatNames += .cmbOCategory.Text.ToString.Trim + ","
                End If
                ' ''Stone
                'For rwIndex As Integer = 0 To .objStone.dtGridStone.Rows.Count - 1
                '    Dim ro As DataRow = dtStoneDetails.NewRow
                '    ro("KEYNO") = DtTran.Rows(index).Item("KEYNO").ToString
                '    ro("TRANTYPE") = cmbTransactionType.Text
                '    For colIndex As Integer = 2 To .objStone.dtGridStone.Columns.Count - 1
                '        ro(colIndex) = .objStone.dtGridStone.Rows(rwIndex).Item(colIndex)
                '    Next
                '    dtStoneDetails.Rows.Add(ro)
                'Next
                'dtStoneDetails.AcceptChanges()
                If Val(.txtOalloy_WET.Text) <> 0 Then DgvTran.Columns("ALLOY").Visible = True
                DgvTran.AutoResizeRow(index)
            ElseIf .rbtMetal.Checked Then
                DtTran.Rows(index).Item("METISSREC") = ObjMaterialDia
                DtTran.Rows(index).Item("TYPE") = "M"
                DtTran.Rows(index).Item("TRANTYPE") = cmbTransactionType.Text
                'DtTran.Rows(index).Item("JOBNO") = .txtMOrdNo.Text
                If _JobNoEnable = True Then
                    DtTran.Rows(index).Item("JOBNO") = GetCostId(cnCostId) & GetCompanyId(strCompanyId) & .txtMOrdNo.Text
                Else
                    If ObjMaterialDia.chkMulti.Checked Then
                        DtTran.Rows(index).Item("JOBNO") = .txtMOrdNo.Text
                    Else
                        DtTran.Rows(index).Item("JOBNO") = .txtMOrdNo.Text
                    End If
                End If

                Desc = .cmbMCategory.Text
                DtTran.Rows(index).Item("DESCRIPTION") = Desc
                DtTran.Rows(index).Item("METAL") = .cmbMMetal.Text
                DtTran.Rows(index).Item("CATNAME") = .cmbMCategory.Text
                DtTran.Rows(index).Item("ISSCATNAME") = .cmbMIssuedCategory.Text
                DtTran.Rows(index).Item("ACCATNAME") = .cmbMAcPostCategory.Text
                DtTran.Rows(index).Item("PURITY") = IIf(Val(.CmbMPurity.Text) <> 0, Val(.CmbMPurity.Text), DBNull.Value)
                DtTran.Rows(index).Item("PCS") = IIf(Val(.txtMPcs_NUM.Text) <> 0, Val(.txtMPcs_NUM.Text), DBNull.Value)
                DtTran.Rows(index).Item("GRSWT") = IIf(Val(.txtMGrsWt_WET.Text) <> 0, Val(.txtMGrsWt_WET.Text), DBNull.Value)
                DtTran.Rows(index).Item("LESSWT") = IIf(Val(.txtMLessWt_WET.Text) <> 0, Val(.txtMLessWt_WET.Text), DBNull.Value)
                DtTran.Rows(index).Item("NETWT") = IIf(Val(.txtMNetWt_WET.Text) <> 0, Val(.txtMNetWt_WET.Text), DBNull.Value)
                DtTran.Rows(index).Item("GRSNET") = .cmbMGrsNet.Text
                DtTran.Rows(index).Item("WASTPER") = IIf(Val(.txtMWastPER.Text) <> 0, Val(.txtMWastPER.Text), DBNull.Value)
                DtTran.Rows(index).Item("WASTAGE") = IIf(Val(.txtMWast_WET.Text) <> 0, Val(.txtMWast_WET.Text), DBNull.Value)
                DtTran.Rows(index).Item("ALLOY") = IIf(Val(.txtMAlloy_WET.Text) <> 0, Val(.txtMAlloy_WET.Text), DBNull.Value)
                DtTran.Rows(index).Item("TOTALWT") = IIf(Val(.txtTotalWt.Text) <> 0, Val(.txtTotalWt.Text), DBNull.Value)
                DtTran.Rows(index).Item("MCGRM") = IIf(Val(.txtMMcGrm_AMT.Text) <> 0, Val(.txtMMcGrm_AMT.Text), DBNull.Value)
                DtTran.Rows(index).Item("MC") = IIf(Val(.txtMMc_AMT.Text) <> 0, Val(.txtMMc_AMT.Text), DBNull.Value)
                DtTran.Rows(index).Item("TOUCH") = IIf(Val(.txtMTouchAMT.Text) <> 0, Val(.txtMTouchAMT.Text), DBNull.Value)
                DtTran.Rows(index).Item("PUREWT") = IIf(Val(.txtMPureWt_WET.Text) <> 0, Val(.txtMPureWt_WET.Text), DBNull.Value)
                DtTran.Rows(index).Item("RATE") = IIf(Val(.txtMRate_OWN.Text) <> 0, Val(.txtMRate_OWN.Text), DBNull.Value)
                DtTran.Rows(index).Item("BOARDRATE") = IIf(Val(.oBoardRate) <> 0, Val(.oBoardRate), DBNull.Value)
                DtTran.Rows(index).Item("GROSSAMT") = IIf(Val(.txtMGrsAmt_AMT.Text) <> 0, Val(.txtMGrsAmt_AMT.Text), DBNull.Value)
                DtTran.Rows(index).Item("VATPER") = IIf(Val(.txtMVatPer_PER.Text) <> 0, Val(.txtMVatPer_PER.Text), DBNull.Value)
                DtTran.Rows(index).Item("VAT") = IIf(Val(.txtMVat_AMT.Text) <> 0, Val(.txtMVat_AMT.Text), DBNull.Value)
                DtTran.Rows(index).Item("AMOUNT") = IIf(Val(.txtMAmount_AMT.Text) <> 0, Val(.txtMAmount_AMT.Text), DBNull.Value)
                DtTran.Rows(index).Item("SGSTPER") = IIf(Val(.txtMSgst_AMT.Text) <> 0, Val(.txtMSgst_AMT.Text), DBNull.Value)
                DtTran.Rows(index).Item("CGSTPER") = IIf(Val(.txtMCgst_AMT.Text) <> 0, Val(.txtMCgst_AMT.Text), DBNull.Value)
                DtTran.Rows(index).Item("IGSTPER") = IIf(Val(.txtMIgst_AMT.Text) <> 0, Val(.txtMIgst_AMT.Text), DBNull.Value)
                DtTran.Rows(index).Item("SGST") = IIf(Val(.txtMSG_AMT.Text) <> 0, Val(.txtMSG_AMT.Text), DBNull.Value)
                DtTran.Rows(index).Item("CGST") = IIf(Val(.txtMCG_AMT.Text) <> 0, Val(.txtMCG_AMT.Text), DBNull.Value)
                DtTran.Rows(index).Item("IGST") = IIf(Val(.txtMIG_AMT.Text) <> 0, Val(.txtMIG_AMT.Text), DBNull.Value)
                DtTran.Rows(index).Item("GST") = Val(DtTran.Rows(index).Item("SGST").ToString) + Val(DtTran.Rows(index).Item("CGST").ToString) + Val(DtTran.Rows(index).Item("IGST").ToString)
                DtTran.Rows(index).Item("TCS") = IIf(Val(.txtMTCS_AMT.Text) <> 0, Val(.txtMTCS_AMT.Text), DBNull.Value)
                DtTran.Rows(index).Item("REMARK1") = .txtMRemark1.Text
                DtTran.Rows(index).Item("REMARK2") = .txtMRemark2.Text
                DtTran.Rows(index).Item("ORDSTATE_NAME") = .cmbMProcess.Text
                DtTran.Rows(index).Item("ADDCHARGE") = Val(.txtMAddlCharge_AMT.Text)
                DtTran.Rows(index).Item("SEIVE") = ""
                DtTran.Rows(index).Item("RESNO") = .RESNO
                DtTran.Rows(index).Item("APPROXAMT") = IIf(Val(.txtMAprxAmount_AMT.Text) <> 0, Val(.txtMAprxAmount_AMT.Text), DBNull.Value)
                DtTran.Rows(index).Item("APPROXTAX") = IIf(Val(.txtMAprxTaxAmt_AMT.Text) <> 0, Val(.txtMAprxTaxAmt_AMT.Text), DBNull.Value)
                DtTran.Rows(index).Item("RATEFIXED") = IIf(.chkMRateFixed.Checked, "Y", "N")
                If cmbTransactionType.Text = "PURCHASE[APPROVAL]" Then DtTran.Rows(index).Item("ORSNO") = .ORSNO
                If Val(.txtMAlloy_WET.Text) <> 0 Then DgvTran.Columns("ALLOY").Visible = True : DgvTran.Columns("TOTALWT").Visible = True
                'dgvtran.
                DtTran.Rows(index).Item("TDSACCODE") = IIf(.txtotdsaccode.Text <> "", .txtotdsaccode.Text, DBNull.Value)
                DgvTran.Rows(index).DefaultCellStyle.WrapMode = DataGridViewTriState.True
                DgvTran.AutoResizeRow(index)

            ElseIf .rbtStone.Checked Then
                If (EditBatchno <> Nothing Or .oEditRowIndex <> -1) Then
                    DtTran.Rows(index).Item("METISSREC") = ObjMaterialDia
                    DtTran.Rows(index).Item("TYPE") = "T"
                    DtTran.Rows(index).Item("TRANTYPE") = cmbTransactionType.Text
                    'DtTran.Rows(index).Item("JOBNO") = .txtSOrdNo.Text
                    If _JobNoEnable = True Then
                        DtTran.Rows(index).Item("JOBNO") = GetCostId(cnCostId) & GetCompanyId(strCompanyId) & .txtSOrdNo.Text
                    Else
                        DtTran.Rows(index).Item("JOBNO") = .txtSOrdNo.Text
                    End If
                    Desc = .cmbSCategory.Text
                    If .cmbSItem.Text <> "" Then Desc += vbCrLf + .cmbSItem.Text
                    DtTran.Rows(index).Item("DESCRIPTION") = Desc
                    DtTran.Rows(index).Item("METAL") = .cmbSMetal.Text
                    DtTran.Rows(index).Item("CATNAME") = .cmbSCategory.Text
                    DtTran.Rows(index).Item("ISSCATNAME") = .cmbSIssuedCategory.Text
                    DtTran.Rows(index).Item("ACCATNAME") = .cmbSAcPostCategory.Text
                    DtTran.Rows(index).Item("PURITY") = IIf(Val(.CmbSPurity.Text) <> 0, Val(.CmbSPurity.Text), DBNull.Value)
                    DtTran.Rows(index).Item("ITEM") = .cmbSItem.Text
                    DtTran.Rows(index).Item("SUBITEM") = .cmbSSubItem.Text
                    DtTran.Rows(index).Item("PCS") = IIf(Val(.txtSPcs_NUM.Text) <> 0, Val(.txtSPcs_NUM.Text), DBNull.Value)
                    DtTran.Rows(index).Item("GRSWT") = IIf(Val(.txtSGrsWt_WET.Text) <> 0, Val(.txtSGrsWt_WET.Text), DBNull.Value)
                    DtTran.Rows(index).Item("NETWT") = IIf(Val(.txtSGrsWt_WET.Text) <> 0, Val(.txtSGrsWt_WET.Text), DBNull.Value)
                    DtTran.Rows(index).Item("WASTAGE") = IIf(Val(.txtSWast_WET.Text) <> 0, Val(.txtSWast_WET.Text), DBNull.Value)
                    DtTran.Rows(index).Item("UNIT") = .cmbSUnit.Text
                    DtTran.Rows(index).Item("CALCMODE") = .cmbSCalcMode.Text
                    DtTran.Rows(index).Item("PUREWT") = IIf(Val(.txtSGrsWt_WET.Text) <> 0, Val(.txtSGrsWt_WET.Text), DBNull.Value)
                    DtTran.Rows(index).Item("RATE") = IIf(Val(.txtSRate_OWN.Text) <> 0, Val(.txtSRate_OWN.Text), DBNull.Value)
                    DtTran.Rows(index).Item("BOARDRATE") = IIf(Val(.oBoardRate) <> 0, Val(.oBoardRate), DBNull.Value)
                    DtTran.Rows(index).Item("GROSSAMT") = IIf(Val(.txtSGrsAmt_AMT.Text) <> 0, Val(.txtSGrsAmt_AMT.Text), DBNull.Value)
                    DtTran.Rows(index).Item("VATPER") = IIf(Val(.txtSVatPer_PER.Text) <> 0, Val(.txtSVatPer_PER.Text), DBNull.Value)
                    DtTran.Rows(index).Item("VAT") = IIf(Val(.txtSVat_AMT.Text) <> 0, Val(.txtSVat_AMT.Text), DBNull.Value)
                    DtTran.Rows(index).Item("AMOUNT") = IIf(Val(.txtSAmount_AMT.Text) <> 0, Val(.txtSAmount_AMT.Text), DBNull.Value)
                    DtTran.Rows(index).Item("SGSTPER") = IIf(Val(.txtSSgst_WET.Text) <> 0, Val(.txtSSgst_WET.Text), DBNull.Value)
                    DtTran.Rows(index).Item("CGSTPER") = IIf(Val(.txtSCgst_WET.Text) <> 0, Val(.txtSCgst_WET.Text), DBNull.Value)
                    DtTran.Rows(index).Item("IGSTPER") = IIf(Val(.txtSIgst_WET.Text) <> 0, Val(.txtSIgst_WET.Text), DBNull.Value)
                    DtTran.Rows(index).Item("SGST") = IIf(Val(.txtSSG_AMT.Text) <> 0, Val(.txtSSG_AMT.Text), DBNull.Value)
                    DtTran.Rows(index).Item("CGST") = IIf(Val(.txtSCG_AMT.Text) <> 0, Val(.txtSCG_AMT.Text), DBNull.Value)
                    DtTran.Rows(index).Item("IGST") = IIf(Val(.txtSIG_AMT.Text) <> 0, Val(.txtSIG_AMT.Text), DBNull.Value)
                    DtTran.Rows(index).Item("GST") = Val(DtTran.Rows(index).Item("SGST").ToString) + Val(DtTran.Rows(index).Item("CGST").ToString) + Val(DtTran.Rows(index).Item("IGST").ToString)
                    DgvTran.Rows(index).DefaultCellStyle.WrapMode = DataGridViewTriState.True
                    DtTran.Rows(index).Item("TCS") = IIf(Val(.txtSTCS_AMT.Text) <> 0, Val(.txtSTCS_AMT.Text), DBNull.Value)
                    DtTran.Rows(index).Item("REMARK1") = .txtSRemark1.Text
                    DtTran.Rows(index).Item("REMARK2") = .txtSRemark2.Text
                    DtTran.Rows(index).Item("TAGNO") = .txtTagNo.Text.ToString.Trim
                    DtTran.Rows(index).Item("ORDSTATE_NAME") = .cmbSProcess.Text
                    DtTran.Rows(index).Item("ADDCHARGE") = Val(.txtSAddlCharge_AMT.Text)
                    DtTran.Rows(index).Item("SEIVE") = .cmbSSeive.Text
                    DtTran.Rows(index).Item("ORSNO") = .StnSno
                    DtTran.Rows(index).Item("RESNO") = .RESNO
                    DtTran.Rows(index).Item("ACCODE") = .ACCODE
                    DtTran.Rows(index).Item("RFID") = .txtRfId.Text.ToString

                    DtTran.Rows(index).Item("CUTID") = .txtRfId.Text.ToString
                    DtTran.Rows(index).Item("COLORID") = .txtRfId.Text.ToString
                    DtTran.Rows(index).Item("CLARITYID") = .txtRfId.Text.ToString
                    DtTran.Rows(index).Item("SHAPEID") = .txtRfId.Text.ToString
                    DtTran.Rows(index).Item("SETTYPEID") = .txtRfId.Text.ToString
                    DtTran.Rows(index).Item("HEIGHT") = .txtRfId.Text.ToString
                    DtTran.Rows(index).Item("WIDTH") = .txtRfId.Text.ToString
                    DtTran.Rows(index).Item("STNGRPID") = GetSqlValue(cn, "SELECT GROUPID FROM " & cnAdminDb & "..STONEGROUP WHERE GROUPNAME = '" & .txtStnGrpName.Text.ToString & "'")
                    DtTran.Rows(index).Item("TDSACCODE") = IIf(.txtotdsaccode.Text <> "", .txtotdsaccode.Text, DBNull.Value)
                    DgvTran.AutoResizeRow(index)
                Else
                    If .GridStuddStone.Rows.Count > 0 Then
                        Dim DT As New DataTable
                        DT = .GridStuddStone.DataSource
                        For i As Integer = 0 To .GridStuddStone.Rows.Count - 1
                            DtTran.Rows(index).Item("METISSREC") = ObjMaterialDia
                            DtTran.Rows(index).Item("TYPE") = "T"
                            DtTran.Rows(index).Item("TRANTYPE") = cmbTransactionType.Text
                            'DtTran.Rows(index).Item("JOBNO") = .txtSOrdNo.Text
                            If _JobNoEnable = True Then
                                DtTran.Rows(index).Item("JOBNO") = GetCostId(cnCostId) & GetCompanyId(strCompanyId) & .GridStuddStone.Rows(i).Cells("ORDNO").Value.ToString
                            Else
                                DtTran.Rows(index).Item("JOBNO") = .GridStuddStone.Rows(i).Cells("ORDNO").Value.ToString
                            End If
                            Desc = .GridStuddStone.Rows(i).Cells("CATEGORY").Value.ToString
                            If .GridStuddStone.Rows(i).Cells("ITEM").Value.ToString <> "" Then Desc += vbCrLf + .GridStuddStone.Rows(i).Cells("ITEM").Value.ToString
                            DtTran.Rows(index).Item("DESCRIPTION") = Desc
                            DtTran.Rows(index).Item("METAL") = .GridStuddStone.Rows(i).Cells("METAL").Value.ToString
                            DtTran.Rows(index).Item("CATNAME") = .GridStuddStone.Rows(i).Cells("CATEGORY").Value.ToString
                            DtTran.Rows(index).Item("ISSCATNAME") = .GridStuddStone.Rows(i).Cells("ISSCATEGORY").Value.ToString
                            DtTran.Rows(index).Item("ACCATNAME") = .GridStuddStone.Rows(i).Cells("CATEGORY").Value.ToString
                            DtTran.Rows(index).Item("PURITY") = IIf(Val(.GridStuddStone.Rows(i).Cells("PURITY").Value.ToString) <> 0, Val(.GridStuddStone.Rows(i).Cells("PURITY").Value.ToString), DBNull.Value)
                            DtTran.Rows(index).Item("ITEM") = .GridStuddStone.Rows(i).Cells("ITEM").Value.ToString
                            DtTran.Rows(index).Item("SUBITEM") = .GridStuddStone.Rows(i).Cells("SUBITEM").Value.ToString
                            DtTran.Rows(index).Item("PCS") = IIf(Val(.GridStuddStone.Rows(i).Cells("PCS").Value.ToString) <> 0, Val(.GridStuddStone.Rows(i).Cells("PCS").Value.ToString), DBNull.Value)
                            DtTran.Rows(index).Item("GRSWT") = IIf(Val(.GridStuddStone.Rows(i).Cells("WEIGHT").Value.ToString) <> 0, Val(.GridStuddStone.Rows(i).Cells("WEIGHT").Value.ToString), DBNull.Value)
                            DtTran.Rows(index).Item("NETWT") = IIf(Val(.GridStuddStone.Rows(i).Cells("WEIGHT").Value.ToString) <> 0, Val(.GridStuddStone.Rows(i).Cells("WEIGHT").Value.ToString), DBNull.Value)
                            DtTran.Rows(index).Item("WASTAGE") = IIf(Val(.GridStuddStone.Rows(i).Cells("WASTAGE").Value.ToString) <> 0, Val(.GridStuddStone.Rows(i).Cells("WASTAGE").Value.ToString), DBNull.Value)
                            DtTran.Rows(index).Item("UNIT") = .GridStuddStone.Rows(i).Cells("UNIT").Value.ToString
                            DtTran.Rows(index).Item("CALCMODE") = .GridStuddStone.Rows(i).Cells("CALC").Value.ToString
                            DtTran.Rows(index).Item("PUREWT") = IIf(Val(.GridStuddStone.Rows(i).Cells("WEIGHT").Value.ToString) <> 0, Val(.GridStuddStone.Rows(i).Cells("WEIGHT").Value.ToString), DBNull.Value)
                            DtTran.Rows(index).Item("RATE") = IIf(Val(.GridStuddStone.Rows(i).Cells("RATE").Value.ToString) <> 0, Val(.GridStuddStone.Rows(i).Cells("RATE").Value.ToString), DBNull.Value)
                            DtTran.Rows(index).Item("BOARDRATE") = IIf(Val(.GridStuddStone.Rows(i).Cells("BOARDRATE").Value.ToString) <> 0, Val(.GridStuddStone.Rows(i).Cells("BOARDRATE").Value.ToString), DBNull.Value)
                            DtTran.Rows(index).Item("GROSSAMT") = IIf(Val(.GridStuddStone.Rows(i).Cells("GROSSAMT").Value.ToString) <> 0, Val(.GridStuddStone.Rows(i).Cells("GROSSAMT").Value.ToString), DBNull.Value)
                            DtTran.Rows(index).Item("VATPER") = IIf(Val(.GridStuddStone.Rows(i).Cells("VATPER").Value.ToString) <> 0, Val(.GridStuddStone.Rows(i).Cells("VATPER").Value.ToString), DBNull.Value)
                            DtTran.Rows(index).Item("VAT") = IIf(Val(.GridStuddStone.Rows(i).Cells("VAT").Value.ToString) <> 0, Val(.GridStuddStone.Rows(i).Cells("VAT").Value.ToString), DBNull.Value)
                            DtTran.Rows(index).Item("SGSTPER") = IIf(Val(.GridStuddStone.Rows(i).Cells("SGSTPER").Value.ToString) <> 0, Val(.GridStuddStone.Rows(i).Cells("SGSTPER").Value.ToString), DBNull.Value)
                            DtTran.Rows(index).Item("SGST") = IIf(Val(.GridStuddStone.Rows(i).Cells("SGST").Value.ToString) <> 0, Val(.GridStuddStone.Rows(i).Cells("SGST").Value.ToString), DBNull.Value)
                            DtTran.Rows(index).Item("CGSTPER") = IIf(Val(.GridStuddStone.Rows(i).Cells("CGSTPER").Value.ToString) <> 0, Val(.GridStuddStone.Rows(i).Cells("CGSTPER").Value.ToString), DBNull.Value)
                            DtTran.Rows(index).Item("CGST") = IIf(Val(.GridStuddStone.Rows(i).Cells("CGST").Value.ToString) <> 0, Val(.GridStuddStone.Rows(i).Cells("CGST").Value.ToString), DBNull.Value)
                            DtTran.Rows(index).Item("IGSTPER") = IIf(Val(.GridStuddStone.Rows(i).Cells("IGSTPER").Value.ToString) <> 0, Val(.GridStuddStone.Rows(i).Cells("IGSTPER").Value.ToString), DBNull.Value)
                            DtTran.Rows(index).Item("IGST") = IIf(Val(.GridStuddStone.Rows(i).Cells("IGST").Value.ToString) <> 0, Val(.GridStuddStone.Rows(i).Cells("IGST").Value.ToString), DBNull.Value)
                            DtTran.Rows(index).Item("GST") = Val(DtTran.Rows(index).Item("SGST").ToString) + Val(DtTran.Rows(index).Item("CGST").ToString) + Val(DtTran.Rows(index).Item("IGST").ToString)
                            DtTran.Rows(index).Item("AMOUNT") = IIf(Val(.GridStuddStone.Rows(i).Cells("AMOUNT").Value.ToString) <> 0, Val(.GridStuddStone.Rows(i).Cells("AMOUNT").Value.ToString), DBNull.Value)
                            DtTran.Rows(index).Item("TCS") = IIf(Val(.GridStuddStone.Rows(i).Cells("TCS").Value.ToString) <> 0, Val(.GridStuddStone.Rows(i).Cells("TCS").Value.ToString), DBNull.Value)
                            DgvTran.Rows(index).DefaultCellStyle.WrapMode = DataGridViewTriState.True
                            DtTran.Rows(index).Item("REMARK1") = .GridStuddStone.Rows(i).Cells("REMARK1").Value.ToString
                            DtTran.Rows(index).Item("REMARK2") = .GridStuddStone.Rows(i).Cells("REMARK2").Value.ToString
                            DtTran.Rows(index).Item("ORDSTATE_NAME") = .GridStuddStone.Rows(i).Cells("ORDSTATE_NAME").Value.ToString
                            DtTran.Rows(index).Item("ADDCHARGE") = Val(.GridStuddStone.Rows(i).Cells("ADDCHARGE").Value.ToString)
                            DtTran.Rows(index).Item("SEIVE") = .GridStuddStone.Rows(i).Cells("SEIVE").Value.ToString
                            DtTran.Rows(index).Item("ORSNO") = .GridStuddStone.Rows(i).Cells("STNSNO").Value.ToString
                            DtTran.Rows(index).Item("RESNO") = .GridStuddStone.Rows(i).Cells("RESNO").Value.ToString
                            DtTran.Rows(index).Item("ACCODE") = .GridStuddStone.Rows(i).Cells("ACCODE").Value.ToString
                            DtTran.Rows(index).Item("RFID") = .GridStuddStone.Rows(i).Cells("RFID").Value.ToString
                            DtTran.Rows(index).Item("CUTID") = .GridStuddStone.Rows(i).Cells("CUTID").Value.ToString
                            DtTran.Rows(index).Item("COLORID") = .GridStuddStone.Rows(i).Cells("COLORID").Value.ToString
                            DtTran.Rows(index).Item("CLARITYID") = .GridStuddStone.Rows(i).Cells("CLARITYID").Value.ToString
                            DtTran.Rows(index).Item("SHAPEID") = .GridStuddStone.Rows(i).Cells("SHAPEID").Value.ToString
                            DtTran.Rows(index).Item("SETTYPEID") = .GridStuddStone.Rows(i).Cells("SETTYPEID").Value.ToString
                            DtTran.Rows(index).Item("HEIGHT") = .GridStuddStone.Rows(i).Cells("HEIGHT").Value.ToString
                            DtTran.Rows(index).Item("WIDTH") = .GridStuddStone.Rows(i).Cells("WIDTH").Value.ToString
                            DtTran.Rows(index).Item("TAGNO") = .GridStuddStone.Rows(i).Cells("TAGNO").Value.ToString
                            DtTran.Rows(index).Item("STNGRPID") = .GridStuddStone.Rows(i).Cells("STNGRPID").Value.ToString
                            DtTran.Rows(index).Item("TDSACCODE") = IIf(.txtotdsaccode.Text <> "", .txtotdsaccode.Text, DBNull.Value)
                            DgvTran.AutoResizeRow(index)
                            index = index + 1
                        Next
                    End If
                End If
            ElseIf .rbtOthers.Checked Then
                DtTran.Rows(index).Item("METISSREC") = ObjMaterialDia
                DtTran.Rows(index).Item("TYPE") = "H"
                DtTran.Rows(index).Item("TRANTYPE") = cmbTransactionType.Text
                Desc = .cmbOthCategory.Text
                If .cmbOthItem.Text <> "" Then Desc += vbCrLf + .cmbOthItem.Text
                DtTran.Rows(index).Item("DESCRIPTION") = Desc
                DtTran.Rows(index).Item("METAL") = .cmbOthMetal.Text
                DtTran.Rows(index).Item("CATNAME") = .cmbOthCategory.Text
                DtTran.Rows(index).Item("ACCATNAME") = .cmbOthCategory.Text
                DtTran.Rows(index).Item("ITEM") = .cmbOthItem.Text
                DtTran.Rows(index).Item("SUBITEM") = .cmbOthSubItem.Text
                DtTran.Rows(index).Item("PCS") = IIf(Val(.txtOthPcs_NUM.Text) <> 0, Val(.txtOthPcs_NUM.Text), DBNull.Value)
                DtTran.Rows(index).Item("RATE") = IIf(Val(.txtOthRate_AMT.Text) <> 0, Val(.txtOthRate_AMT.Text), DBNull.Value)
                DtTran.Rows(index).Item("BOARDRATE") = IIf(Val(.oBoardRate) <> 0, Val(.oBoardRate), DBNull.Value)
                DtTran.Rows(index).Item("GROSSAMT") = IIf(Val(.txtOthGrsAmt_AMT.Text) <> 0, Val(.txtOthGrsAmt_AMT.Text), DBNull.Value)
                DtTran.Rows(index).Item("VATPER") = IIf(Val(.txtOthVatPer_PER.Text) <> 0, Val(.txtOthVatPer_PER.Text), DBNull.Value)
                DtTran.Rows(index).Item("VAT") = IIf(Val(.txtOthVat_AMT.Text) <> 0, Val(.txtOthVat_AMT.Text), DBNull.Value)
                DtTran.Rows(index).Item("AMOUNT") = IIf(Val(.txtOthAmount_AMT.Text) <> 0, Val(.txtOthAmount_AMT.Text), DBNull.Value)
                DtTran.Rows(index).Item("SGSTPER") = IIf(Val(.txtOthSgst_AMT.Text) <> 0, Val(.txtOthSgst_AMT.Text), DBNull.Value)
                DtTran.Rows(index).Item("CGSTPER") = IIf(Val(.txtOthCgst_AMT.Text) <> 0, Val(.txtOthCgst_AMT.Text), DBNull.Value)
                DtTran.Rows(index).Item("IGSTPER") = IIf(Val(.txtOthIgst_AMT.Text) <> 0, Val(.txtOthIgst_AMT.Text), DBNull.Value)
                DtTran.Rows(index).Item("SGST") = IIf(Val(.txtOthSG_AMT.Text) <> 0, Val(.txtOthSG_AMT.Text), DBNull.Value)
                DtTran.Rows(index).Item("CGST") = IIf(Val(.txtOthCG_AMT.Text) <> 0, Val(.txtOthCG_AMT.Text), DBNull.Value)
                DtTran.Rows(index).Item("IGST") = IIf(Val(.txtOthIG_AMT.Text) <> 0, Val(.txtOthIG_AMT.Text), DBNull.Value)
                DtTran.Rows(index).Item("GST") = Val(DtTran.Rows(index).Item("SGST").ToString) + Val(DtTran.Rows(index).Item("CGST").ToString) + Val(DtTran.Rows(index).Item("IGST").ToString)
                DtTran.Rows(index).Item("REMARK1") = .txtOthRemark1.Text
                DtTran.Rows(index).Item("REMARK2") = .txtOthRemark2.Text
                DgvTran.Rows(index).DefaultCellStyle.WrapMode = DataGridViewTriState.True
                DgvTran.AutoResizeRow(index)
            End If
            LoadTransactionType()
        End With
    End Sub

    Private Sub cmbTransactionType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbTransactionType.KeyDown
        If e.KeyCode = Keys.Enter Then

            If DtTran.Select("DESCRIPTION<>''").Length = 0 Then
                xomsNoflag = False
            Else
                ObjMaterialDia.TdsomsFlag = True
            End If

            If Val(txtEstNo_NUM.Text) > 0 And (cmbTransactionType.Text = "ISSUE" Or cmbTransactionType.Text = "PURCHASE RETURN") Then
                If MI_POPUP_PROCESSTYPE Then
                    objProcessType.ShowDialog()
                End If
                LoadValues()
                Exit Sub
            End If

            If txttransferno.Text <> "" And (cmbTransactionType.Text = "ISSUE" Or cmbTransactionType.Text = "PURCHASE RETURN") Then
                loadtransferdata()
                Exit Sub
            End If

            If GST Then
                Dim GstDate As Date = GetAdmindbSoftValue("GSTDATE", "")
                If dtpTrandate.Value >= GstDate Then
                    GstFlag = True
                Else
                    GstFlag = False
                End If
            Else
                GstFlag = False
            End If

            If MIMR_VALIDATEBILLNO = True And Val(txtBillNo.Text) > 0 And cmbAcName.Text <> "" Then
                If validatebillno() = True Then
                    MsgBox("Bill No Already Exist for this Account", MsgBoxStyle.Information)
                    txtBillNo.Select()
                    Exit Sub
                End If
            End If

            If GstFlag And (UCase(cmbTransactionType.Text) = "PURCHASE RETURN" Or
                         UCase(cmbTransactionType.Text) = "PURCHASE" Or
                         UCase(cmbTransactionType.Text) = "PURCHASE[APPROVAL]" Or
                         UCase(cmbTransactionType.Text) = "INTERNAL TRANSFER" Or
                         UCase(cmbTransactionType.Text) = "APPROVAL RECEIPT") Then
                Dim StateId As Integer = Val(objGPack.GetSqlValue("SELECT STATEID FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbAcName.Text & "'").ToString)
                If StateId = 0 Then
                    MsgBox("Please Update State for the Account [" & cmbAcName.Text & "]", MsgBoxStyle.Information)
                    Exit Sub
                End If
            End If


            If MRMI_LOCK_BILLNODATE = False And (cmbTransactionType.Text = "PURCHASE" Or cmbTransactionType.Text = "PURCHASE[APPROVAL]" Or cmbTransactionType.Text = "PURCHASE RETURN") And txtBillNo.Text = "" Then
                MsgBox("BillNo Should not Empty", MsgBoxStyle.Information)
                txtBillNo.Select()
                Exit Sub
            End If
            _Accode = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbAcName.Text & "'")
            Dim OrderSnos As String = ""
            If cmbTransactionType.Text = "PURCHASE[APPROVAL]" Then
                Dim dtMat As New DataTable
                dtMat = CType(DgvTran.DataSource, DataTable)
                For Each ro As DataRow In dtMat.Rows
                    If ro.Item("TRANTYPE").ToString = "" Then Exit For
                    If ro.Item("ORSNO").ToString <> "" Then
                        OrderSnos += "'" & ro.Item("ORSNO").ToString & "',"
                    End If
                Next
                If OrderSnos <> "" Then
                    OrderSnos = Mid(OrderSnos, 1, OrderSnos.Length - 1)
                End If
            End If
            'If xomsNoflag = False Then
            '    xOTdsname = Nothing
            '    xMTdsName = Nothing
            '    xSTdsname = Nothing
            'End If
            If xOTdsname <> Nothing Then
                ObjMaterialDia = New MaterialIssRec(OMaterialType, cmbTransactionType.Text, dtpTrandate.Value, _Accode, OrderSnos, CostCenterId, , SRVTPER, GstFlag, xomsNoflag, xOTdsname)
            ElseIf xMTdsName <> Nothing Then
                ObjMaterialDia = New MaterialIssRec(OMaterialType, cmbTransactionType.Text, dtpTrandate.Value, _Accode, OrderSnos, CostCenterId, , SRVTPER, GstFlag, xomsNoflag, xMTdsName)
            ElseIf xSTdsname <> Nothing Then
                ObjMaterialDia = New MaterialIssRec(OMaterialType, cmbTransactionType.Text, dtpTrandate.Value, _Accode, OrderSnos, CostCenterId, , SRVTPER, GstFlag, xomsNoflag, xSTdsname)
            Else
                ObjMaterialDia = New MaterialIssRec(OMaterialType, cmbTransactionType.Text, dtpTrandate.Value, _Accode, OrderSnos, CostCenterId, , SRVTPER, GstFlag, xomsNoflag, Nothing)
            End If

            ObjMaterialDia.Text = "MATERIAL " & cmbTransactionType.Text
            ObjMaterialDia.Remark1 = txtRemark1.Text
            ObjMaterialDia.Remark2 = txtRemark2.Text
            ObjMaterialDia.BillCostId = CostCenterId

            If xOTdsname <> Nothing And xomsNoflag Then ObjMaterialDia.Otdsname = xOTdsname
            If xMTdsName <> Nothing And xomsNoflag Then ObjMaterialDia.Mtdsname = xMTdsName
            If xSTdsname <> Nothing And xomsNoflag Then ObjMaterialDia.stdsname = xSTdsname

            If xomsNoflag = True Then
                If xOordno <> Nothing Then
                    ObjMaterialDia.Oordno = xOordno
                    ObjMaterialDia.Mordno = xOordno
                    ObjMaterialDia.sordno = xOordno
                ElseIf xMordno <> Nothing Then
                    ObjMaterialDia.Oordno = xMordno
                    ObjMaterialDia.Mordno = xMordno
                    ObjMaterialDia.sordno = xMordno
                ElseIf xsordno <> Nothing Then
                    ObjMaterialDia.Oordno = xsordno
                    ObjMaterialDia.Mordno = xsordno
                    ObjMaterialDia.sordno = xsordno
                Else
                    ObjMaterialDia.Oordno = xOordno
                End If

                ObjMaterialDia.TdsomsFlag = True
            Else
                xomsNoflag = True
                ObjMaterialDia.Oordno = ""
                ObjMaterialDia.Mordno = ""
                ObjMaterialDia.sordno = ""
                ObjMaterialDia.TdsomsFlag = False
            End If
            ObjMaterialDia.OProcessname = xOProcessname
            ObjMaterialDia.OMetalname = xOMetalname
            ObjMaterialDia.OCategoryname = xOCategoryname
            ObjMaterialDia.OCategoryname = xOCategoryname
            ObjMaterialDia.OIssCatName = xOIssCatName
            ObjMaterialDia.OAcpostCatname = xOAcpostCatname
            ObjMaterialDia.Opurityper = xOpurityper
            ObjMaterialDia.GstFlag = GstFlag



            Dim Tags() As String = TagNos.Split(",")
            Dim Catname() As String = CatNames.Split(",")
            For i As Integer = 0 To Tags.Length - 1
                ObjMaterialDia.ListTagNos.Add(Tags(i).ToString)
                ObjMaterialDia.arrTagNos(i, 0) = ""
                ObjMaterialDia.arrTagNos(i, 1) = ""
                ObjMaterialDia.arrTagNos(i, 0) = Tags(i).ToString
                ObjMaterialDia.arrTagNos(i, 1) = Catname(i).ToString
            Next

            If MIMR_APPROXVALUE = True And (cmbTransactionType.Text = "ISSUE" Or cmbTransactionType.Text = "RECEIPT") Then
                ObjMaterialDia.txtOAprxAmount_AMT.Visible = True
                ObjMaterialDia.txtOAprxTaxAmt_AMT.Visible = True
                ObjMaterialDia.txtMAprxAmount_AMT.Visible = True
                ObjMaterialDia.txtMAprxTaxAmt_AMT.Visible = True
                ObjMaterialDia.lblOApproxAmt.Visible = True
                ObjMaterialDia.lblOApproxTax.Visible = True
                ObjMaterialDia.lblMApproxAmt.Visible = True
                ObjMaterialDia.lblMApproxTax.Visible = True
            Else
                ObjMaterialDia.txtOAprxAmount_AMT.Visible = False
                ObjMaterialDia.txtOAprxTaxAmt_AMT.Visible = False
                ObjMaterialDia.txtMAprxAmount_AMT.Visible = False
                ObjMaterialDia.txtMAprxTaxAmt_AMT.Visible = False
                ObjMaterialDia.lblOApproxAmt.Visible = False
                ObjMaterialDia.lblOApproxTax.Visible = False
                ObjMaterialDia.lblMApproxAmt.Visible = False
                ObjMaterialDia.lblMApproxTax.Visible = False
            End If

            If ObjMaterialDia.ShowDialog = Windows.Forms.DialogResult.OK Then
                LoadTransaction(ObjMaterialDia)
                If txtRemark1.Text = "" Then txtRemark1.Text = ObjMaterialDia.Remark1
                If txtRemark2.Text = "" Then txtRemark2.Text = ObjMaterialDia.Remark2
                If ObjMaterialDia.rbtOrnament.Checked Then '0414
                    If xOProcessname = Nothing Then xOProcessname = ObjMaterialDia.cmbOProcess.Text
                    If xOordno = Nothing Then xOordno = ObjMaterialDia.txtOOrdNo.Text
                    If xOTdsname = Nothing Then xOTdsname = ObjMaterialDia.Cmboacname.Text
                    If xOCategoryname = Nothing Then xOCategoryname = ObjMaterialDia.cmbOCategory.Text
                    If xOIssCatName = Nothing Then xOIssCatName = ObjMaterialDia.cmbOIssuedCategory.Text
                    If xOAcpostCatname = Nothing Then xOAcpostCatname = ObjMaterialDia.cmbOAcPostCategory.Text
                    If xOMetalname = Nothing Then xOMetalname = ObjMaterialDia.cmbOMetal.Text
                    If xOpurityper = Nothing Then xOpurityper = ObjMaterialDia.CmbOPurity.Text

                ElseIf ObjMaterialDia.rbtMetal.Checked Then
                    If xMordno = Nothing Then xMordno = ObjMaterialDia.txtMOrdNo.Text
                    If xMTdsName = Nothing Then xMTdsName = ObjMaterialDia.Cmbmacname.Text
                    If xMProcessname = Nothing Then xMProcessname = ObjMaterialDia.cmbMProcess.Text
                    If xMcategoryname = Nothing Then xMcategoryname = ObjMaterialDia.cmbMCategory.Text
                    If xMIsscatName = Nothing Then xMIsscatName = ObjMaterialDia.cmbMIssuedCategory.Text
                    If xMAcPostcatname = Nothing Then xMAcPostcatname = ObjMaterialDia.cmbOAcPostCategory.Text
                    If xMmetalName = Nothing Then xMmetalName = ObjMaterialDia.cmbMMetal.Text
                    If xMpurityper = Nothing Then xMpurityper = ObjMaterialDia.CmbMPurity.Text
                ElseIf ObjMaterialDia.rbtStone.Checked Then
                    If xSTdsname = Nothing Then xSTdsname = ObjMaterialDia.Cmbsacname.Text
                    If xsordno = Nothing Then xsordno = ObjMaterialDia.txtSOrdNo.Text
                    If xSProcessname = Nothing Then xSProcessname = ObjMaterialDia.cmbSProcess.Text
                End If
            End If
        ElseIf e.KeyCode = Keys.Escape Then
            If cmbTransactionType.DroppedDown Then Exit Sub
            txtAdjCash_AMT.Select()
        End If

    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        ExitToolStrip = True
        Me.Close()
    End Sub

    Private Sub DgvTran_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles DgvTran.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            DgvTran.CurrentCell = DgvTran.Rows(DgvTran.CurrentRow.Index).Cells(DgvTran.FirstDisplayedCell.ColumnIndex)
            Dim DgvRow As DataGridViewRow = DgvTran.CurrentRow
            If DgvRow.Cells("TRANTYPE").Value.ToString <> "" Then
                Dim obj As MaterialIssRec
                obj = CType(DgvRow.Cells("METISSREC").Value, MaterialIssRec)
                obj.editflag = True
                If EditBatchno <> Nothing Then obj.EditBatchno = EditBatchno.ToString : obj.SrVtTax = SRVTPER
                If DgvRow.Cells("STNGRPID").Value.ToString <> "" Then
                    obj.txtStnGrpName.Text = objGPack.GetSqlValue("SELECT GROUPNAME FROM " & cnAdminDb & "..STONEGROUP WHERE GROUPID = '" & DgvRow.Cells("STNGRPID").Value.ToString & "'")
                End If
                obj.oEditRowIndex = Val(DgvTran.CurrentRow.Index)
                If DgvRow.Cells("TYPE").Value.ToString = "O" And DgvRow.Cells("RATEFIXED").Value.ToString = "Y" Then
                    obj.chkORateFixed.Checked = True
                ElseIf DgvRow.Cells("TYPE").Value.ToString = "M" And DgvRow.Cells("RATEFIXED").Value.ToString = "Y" Then
                    obj.chkMRateFixed.Checked = True
                ElseIf DgvRow.Cells("TYPE").Value.ToString = "T" And DgvRow.Cells("RATEFIXED").Value.ToString = "Y" Then
                    obj.chkSRateFixed.Checked = True
                Else
                    obj.chkORateFixed.Checked = False
                    obj.chkMRateFixed.Checked = False
                    obj.chkSRateFixed.Checked = False
                End If
                If DgvRow.Cells("TRANTYPE").Value.ToString = "ISSUE" Then
                ElseIf DgvRow.Cells("TRANTYPE").Value.ToString = "PURCHASE RETURN" Then
                ElseIf DgvRow.Cells("TRANTYPE").Value.ToString = "APPROVAL ISSUE" Then

                End If

                If DgvRow.Cells("TYPE").Value.ToString = "O" And DgvRow.Cells("HALLMARK").Value.ToString = "Y" Then
                    obj.CmbOHallmark.Text = "YES"
                Else
                    obj.CmbOHallmark.Text = "NO"
                End If



                If MIMR_APPROXVALUE = True And (DgvRow.Cells("TRANTYPE").Value.ToString = "ISSUE" Or DgvRow.Cells("TRANTYPE").Value.ToString = "RECEIPT") Then
                    ObjMaterialDia.txtOAprxAmount_AMT.Visible = True
                    ObjMaterialDia.txtOAprxTaxAmt_AMT.Visible = True
                    ObjMaterialDia.txtMAprxAmount_AMT.Visible = True
                    ObjMaterialDia.txtMAprxTaxAmt_AMT.Visible = True
                    ObjMaterialDia.lblOApproxAmt.Visible = True
                    ObjMaterialDia.lblOApproxTax.Visible = True
                    ObjMaterialDia.lblMApproxAmt.Visible = True
                    ObjMaterialDia.lblMApproxTax.Visible = True
                Else
                    ObjMaterialDia.txtOAprxAmount_AMT.Visible = False
                    ObjMaterialDia.txtOAprxTaxAmt_AMT.Visible = False
                    ObjMaterialDia.txtMAprxAmount_AMT.Visible = False
                    ObjMaterialDia.txtMAprxTaxAmt_AMT.Visible = False
                    ObjMaterialDia.lblOApproxAmt.Visible = False
                    ObjMaterialDia.lblOApproxTax.Visible = False
                    ObjMaterialDia.lblMApproxAmt.Visible = False
                    ObjMaterialDia.lblMApproxTax.Visible = False
                End If

                If obj.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    If dgvOrderDet.Rows.Count > 0 Then
                        Dim Rowindex As Integer = Val(DgvTran.Rows(Val(DgvTran.CurrentRow.Index)).Cells("ROWINDEX").Value.ToString)
                        For i As Integer = 0 To dgvOrderDet.Rows.Count - 1
                            If dgvOrderDet.Rows(i).Cells("ROWINDEX").Value.ToString = Rowindex Then
                                dgvOrderDet.Rows.RemoveAt(i)
                                MdtOdDet.AcceptChanges()
                            End If
                        Next
                    End If
                    LoadTransaction(obj)
                End If
            End If
        End If
    End Sub

    Private Sub DgvTran_UserDeletedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles DgvTran.UserDeletedRow
        DtTran.AcceptChanges()
        For cnt As Integer = 0 To DgvTran.RowCount - 1
            DgvTran.Rows(cnt).DefaultCellStyle.WrapMode = DataGridViewTriState.True
            DgvTran.AutoResizeRow(0)
        Next
        LoadTransactionType()
    End Sub

    Private Sub DgvTran_UserDeletingRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowCancelEventArgs) Handles DgvTran.UserDeletingRow
        Dim CurrentRow As Integer = Val(DgvTran.CurrentRow.Index)
        Dim RowId As Integer = Val(DgvTran.Rows(CurrentRow).Cells("ROWINDEX").Value.ToString)
        If dgvOrderDet.Rows.Count > 0 Then
            For i As Integer = 0 To dgvOrderDet.Rows.Count - 1
                If dgvOrderDet.Rows(i).Cells("ROWINDEX").Value.ToString = RowId Then
                    dgvOrderDet.Rows.RemoveAt(i) 'currentrow-i
                    MdtOdDet.AcceptChanges()
                End If
            Next
            'In future indexout of memory error occur why
        End If
        DtTran.Rows.Add()
        DtTran.AcceptChanges()
    End Sub

    Private Sub funcAdditionalCharges()
        If objAddlCharge.Visible Then Exit Sub
        objAddlCharge.BackColor = Me.BackColor
        objAddlCharge.StartPosition = FormStartPosition.CenterScreen
        objAddlCharge.grpAddlCharge.BackgroundColor = grpHeader.BackgroundColor
        objAddlCharge.cmbChargeName.Select()
        objAddlCharge.ShowDialog()
        Dim AddlAmt As Decimal = Val(objAddlCharge.gridAddChargeTotal.Rows(0).Cells("AMOUNT").Value.ToString)
        txtAdditionalCharges.Text = IIf(AddlAmt <> 0, Format(AddlAmt, "0.00"), Nothing)
        txtAdjCash_AMT.Select()
    End Sub

    Private Sub txtAdjCheque_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAdjCheque_AMT.GotFocus
        If objCheaque.Visible Then Exit Sub
        objCheaque.BackColor = Me.BackColor
        objCheaque.StartPosition = FormStartPosition.CenterScreen
        objCheaque.grpCheque.BackgroundColor = grpHeader.BackgroundColor
        objCheaque.ShowDialog()
        Dim chqAmt As Double = Val(objCheaque.gridChequeTotal.Rows(0).Cells("AMOUNT").Value.ToString)
        txtAdjCheque_AMT.Text = IIf(chqAmt <> 0, Format(chqAmt, "0.00"), Nothing)
        txtAdjCash_AMT.Focus()
    End Sub

    Private Sub txtAdditionalCharges_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAdditionalCharges.TextChanged
        CalcReceiveAmount()
    End Sub

    Private Function GetPersonalInfoSno(ByVal tran1 As OleDbTransaction) As String
GETNSNO:
        Dim tSno As Integer = 0
        Dim strSql As String
        Dim cmd As OleDbCommand
        strSql = " SELECT CTLTEXT AS TAGSNO FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'PERSONALINFOCODE'"
        tSno = Val(objGPack.GetSqlValue(strSql, , , tran1))
        ''UPDATING 
        ''TAGNO INTO SOFTCONTROL
        strSql = " UPDATE " & cnAdminDb & "..SOFTCONTROL SET CTLTEXT = '" & tSno + 1 & "' "
        strSql += " WHERE CTLID = 'PERSONALINFOCODE' AND CONVERT(INT,CTLTEXT) = " & tSno & ""
        cmd = New OleDbCommand(strSql, cn, tran1)
        If cmd.ExecuteNonQuery() = 0 Then
            GoTo GETNSNO
        End If
        strSql = " SELECT SNO FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = '" & GetCostId(cnCostId) & (tSno + 1).ToString & "'"
        If objGPack.GetSqlValue(strSql, , "-1", tran1) <> "-1" Then
            GoTo GETNSNO
        End If
        Return GetCostId(cnCostId) & (tSno + 1).ToString
    End Function

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim billgencatcode As String = ""
        Dim billcontrolid As String = ""
        If EditBatchno = Nothing Then
            If Not BrighttechPack.Methods.GetRights(_DtUserRights, IIf(OMaterialType = MaterialType.Receipt, "REC", "ISS") & Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
        Else
            If Not BrighttechPack.Methods.GetRights(_DtUserRights, IIf(OMaterialType = MaterialType.Receipt, "REC", "ISS") & Me.Name, BrighttechPack.Methods.RightMode.Edit) Then Exit Sub
        End If
        If DemoLogin Then
            MsgBox(E0025, MsgBoxStyle.Exclamation)
            Exit Sub
        End If
        If CheckTrialDate(dtpTrandate.Value) = False Then Exit Sub
        'If CheckEntryDate(dtpTranDate.Value) Then Exit Sub
        If cmbCostCentre.Enabled = True Then
            If cmbCostCentre.Text.Trim = "" Then
                MsgBox("Cost Center Empty", MsgBoxStyle.Information)
                cmbCostCentre.Select()
                Exit Sub
            Else
                If objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre.Text & "'") = "" Then MsgBox("Please select valid costcentre", MsgBoxStyle.Critical) : cmbCostCentre.Focus() : Exit Sub
            End If
        End If
        _Accode = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbAcName.Text & "'")
        If _Accode = "" Then
            MsgBox("Invalid AcName", MsgBoxStyle.Information)
            cmbAcName.Select()
            Exit Sub
        End If
        StrSql = " SELECT ISNULL(ACTIVE,'') ACTIVE FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE='" & _Accode & "'"
        If GetSqlValue(cn, StrSql) = "" Then
            MsgBox("PartyCode not Approve", MsgBoxStyle.Information)
            cmbAcName.Select()
            Exit Sub
        End If

        If CompanyStateId = 0 And GstFlag Then
            MsgBox("State not Updated in Company Master.", MsgBoxStyle.Information)
            Exit Sub
        End If
        If MR_VALIDATESTOCK And CatbalanceDisplay _
                And OMaterialType = MaterialType.Receipt _
                And cmbTransactionType.Text = "RECEIPT" Then
            Dim wt As Decimal = Val(DgvTranTotal.Rows(0).Cells("GRSWT").Value.ToString)
            Dim dtWt As New DataTable
            dtWt = CType(dtCatBalance.DataSource, DataTable)
            Dim Balwt As Decimal = Val(dtWt.Compute("SUM(GRS)", "CATEGORY='TOTAL'").ToString)
            If Balwt > 0 Then
                MsgBox("Receipt weight should not greater then Balance weight...", MsgBoxStyle.Information)
                Exit Sub
            ElseIf Balwt < 0 Then
                If Math.Abs(Balwt) < wt Then
                    MsgBox("Receipt weight should not greater then Balance weight...", MsgBoxStyle.Information)
                    Exit Sub
                End If
            End If
        End If
        Dim Dtchk As New DataTable
        Dtchk = DgvTran.DataSource
        Dim RowCount As Integer = Dtchk.Compute("COUNT(TRANTYPE)", "TRANTYPE<>''")
        If Not RowCount > 0 Then MsgBox("Must Enter one Entry", MsgBoxStyle.Information) : cmbTransactionType.Focus() : Exit Sub

        _Acctype = objGPack.GetSqlValue("SELECT LOCALOUTST FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = '" & _Accode & "'")
        CostCenterId = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre.Text & "'", , , tran)
        Try
            If CatbaseRecIsstranno Then
                Dim dtcatchk As New DataTable
                dtcatchk = DgvTran.DataSource
                Dim dtdisc As New DataTable
                dtdisc = dtcatchk.DefaultView.ToTable(True, "CATNAME")
                Dim catcount As Integer = 0
                For II As Integer = 0 To dtdisc.Rows.Count - 1
                    If dtdisc.Rows(II).Item("CATNAME").ToString.Trim = "" Then GoTo NEXTFOR
                    catcount = catcount + 1
                    billgencatcode = dtdisc.Rows(0).Item("CATNAME")
NEXTFOR:
                Next II
                If catcount > 1 Then
                    MsgBox("Please check entries are conflict by multiple category", MsgBoxStyle.Question)
                    Exit Sub
                End If
                billgencatcode = objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & billgencatcode & "'", , , tran)
            End If
            If CatbaseRecIsstranno = False Then
                If MANBILLNO Then
                    objManualBill = New frmManualBillNoGen
                    objManualBill.Text = IIf(OMaterialType = MaterialType.Receipt, "Receipt Tran No.", "Issue Tran No")
                    If objManualBill.ShowDialog = Windows.Forms.DialogResult.OK Then
                        TranNo = Val(objManualBill.txtBillNo_NUM.Text)
                    Else
                        btnSave.Focus()
                        Exit Sub
                    End If
                End If
            Else
                If MANBILLNO Then
                    If MsgBox("Manual bill no not applicable for Category base maintained" & vbCrLf & " Can you proceed with Auto No generation ?", MsgBoxStyle.YesNo) = MsgBoxResult.No Then btnSave.Focus() : Exit Sub
                End If
                MANBILLNO = False
            End If
            tran = Nothing
            tran = cn.BeginTransaction()
            StrSql = " IF (SELECT COUNT(*) FROM " & cnAdminDb & "..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "BILLNO')> 0"
            StrSql += " DROP TABLE " & cnAdminDb & "..TEMP" & systemId & "BILLNO"
            Cmd = New OleDbCommand(StrSql, cn, tran)
            Cmd.ExecuteNonQuery()
            Dim AccPost As Boolean = True
            If cmbTransactionType.Text = "APPROVAL RECEIPT" Or cmbTransactionType.Text = "APPROVAL ISSUE" Then
                AccPost = MRMIAPPACCPOST
            End If
            If EditBatchno = Nothing Then
                If Not MANBILLNO Then
GenBillNo:
                    If OMaterialType = MaterialType.Receipt Then
                        Select Case cmbTransactionType.Text
                            Case "PURCHASE", "PURCHASE[APPROVAL]"
                                billcontrolid = "GEN-SM-RECPUR"
                                If CatbaseRecIsstranno And billgencatcode <> "" Then billcontrolid = "CAT-" + billgencatcode + "-SM-REC"
                                If _AccAudit Then billcontrolid = billcontrolid + "-APP"
                                StrSql = "SELECT CTLMODE FROM " & cnStockDb & "..BILLCONTROL"
                                StrSql += " WHERE CTLID ='" & billcontrolid & "' AND COMPANYID = '" & strCompanyId & "'"
                                If UCase(objGPack.GetSqlValue(StrSql, , , tran)) = "Y" Then
                                    TranNo = GetBillNoValue(billcontrolid, tran)
                                Else
                                    billcontrolid = "GEN-SM-REC"
                                    TranNo = GetBillNoValue(billcontrolid, tran)
                                End If
                                If cmbTransactionType.Text = "PURCHASE[APPROVAL]" Then
                                    billcontrolid = "GEN-SM-ISS"
                                    TranNoApp = GetBillNoValue(billcontrolid, tran)
                                End If
                            Case "INTERNAL TRANSFER"
                                billcontrolid = "GEN-SM-INTREC"
                                If CatbaseRecIsstranno And billgencatcode <> "" Then billcontrolid = "CAT-" + billgencatcode + "-SM-INTREC"
                                If _AccAudit Then billcontrolid = billcontrolid + "-APP"
                                StrSql = "SELECT CTLMODE FROM " & cnStockDb & "..BILLCONTROL"
                                StrSql += " WHERE CTLID ='" & billcontrolid & "' AND COMPANYID = '" & strCompanyId & "'"
                                If UCase(objGPack.GetSqlValue(StrSql, , , tran)) = "Y" Then
                                    TranNo = GetBillNoValue(billcontrolid, tran)
                                Else
                                    billcontrolid = "GEN-SM-REC"
                                    TranNo = GetBillNoValue(billcontrolid, tran)
                                End If
                            Case "PACKING"
                                If GetBillControlValue("GEN-PMSEPERATENO", tran) = "Y" Then
                                    TranNo = GetBillNoValue("GEN-PMRECEIPTNO", tran)
                                Else
                                    'CAT-00006-ACC'
                                    billcontrolid = "GEN-SM-REC"
                                    If CatbaseRecIsstranno And billgencatcode <> "" Then billcontrolid = "CAT-" + billgencatcode + "-SM-REC"
                                    If _AccAudit Then billcontrolid = billcontrolid + "-APP"
                                    TranNo = GetBillNoValue(billcontrolid, tran)
                                End If
                            Case "APPROVAL RECEIPT"
                                billcontrolid = "GEN-SM-REC"
                                If CatbaseRecIsstranno And billgencatcode <> "" Then billcontrolid = "CAT-" + billgencatcode + "-SM-REC"
                                If GetBillControlValue("GEN-SMRECAPPSEPERATENO", tran) = "Y" Then
                                    billcontrolid = billcontrolid + "-APP"
                                Else
                                    If _AccAudit Then billcontrolid = billcontrolid + "-APP"
                                End If
                                TranNo = GetBillNoValue(billcontrolid, tran)
                            Case Else
                                billcontrolid = "GEN-SM-REC"
                                If CatbaseRecIsstranno And billgencatcode <> "" Then billcontrolid = "CAT-" + billgencatcode + "-SM-REC"
                                If _AccAudit Then billcontrolid = billcontrolid + "-APP"
                                TranNo = GetBillNoValue(billcontrolid, tran)
                        End Select
                    Else
                        Select Case cmbTransactionType.Text
                            Case "PACKING"
                                If GetBillControlValue("GEN-PMSEPERATENO", tran) = "Y" Then
                                    TranNo = GetBillNoValue("GEN-PMISSUENO", tran)
                                Else
                                    billcontrolid = "GEN-SM-ISS"
                                    If CatbaseRecIsstranno And billgencatcode <> "" Then billcontrolid = "CAT-" + billgencatcode + "-SM-ISS"
                                    If _AccAudit Then billcontrolid = billcontrolid + "-APP"
                                    TranNo = GetBillNoValue(billcontrolid, tran)
                                End If
                            Case "INTERNAL TRANSFER"
                                billcontrolid = "GEN-SM-INTISS"
                                If CatbaseRecIsstranno And billgencatcode <> "" Then billcontrolid = "CAT-" + billgencatcode + "-SM-INTISS"
                                If _AccAudit Then billcontrolid = billcontrolid + "-APP"
                                StrSql = "SELECT CTLMODE FROM " & cnStockDb & "..BILLCONTROL"
                                StrSql += " WHERE CTLID ='" & billcontrolid & "' AND COMPANYID = '" & strCompanyId & "'"
                                If UCase(objGPack.GetSqlValue(StrSql, , , tran)) = "Y" Then
                                    TranNo = GetBillNoValue(billcontrolid, tran)
                                Else
                                    billcontrolid = "GEN-SM-ISS"
                                    TranNo = GetBillNoValue(billcontrolid, tran)
                                End If
                                StrSql = "SELECT CTLMODE FROM " & cnStockDb & "..BILLCONTROL"
                                StrSql += " WHERE CTLID ='GEN-TRANSISTNO' AND COMPANYID = '" & strCompanyId & "'"
                                If UCase(objGPack.GetSqlValue(StrSql, , , tran)) = "Y" Then
                                    Transistno = GetBillNoValue("GEN-TRANSISTNO", tran)
                                End If
                            Case "APPROVAL ISSUE"
                                billcontrolid = "GEN-SM-ISS"
                                If CatbaseRecIsstranno And billgencatcode <> "" Then billcontrolid = "CAT-" + billgencatcode + "-SM-ISS"
                                If GetBillControlValue("GEN-SMISSAPPSEPERATENO", tran) = "Y" Then
                                    billcontrolid = billcontrolid + "-APP"
                                Else
                                    If _AccAudit Then billcontrolid = billcontrolid + "-APP"
                                End If
                                TranNo = GetBillNoValue(billcontrolid, tran)
                            Case "PURCHASE RETURN"
                                billcontrolid = "GEN-SM-ISS"
                                If CatbaseRecIsstranno And billgencatcode <> "" Then billcontrolid = "CAT-" + billgencatcode + "-SM-ISS"
                                If GetBillControlValue("GEN-SMISSIPUSEPERATENO", tran) = "Y" Then
                                    billcontrolid = billcontrolid + "-IPU"
                                Else
                                    If _AccAudit Then billcontrolid = billcontrolid + "-IPU"
                                End If
                                TranNo = GetBillNoValue(billcontrolid, tran)
                            Case Else
                                billcontrolid = "GEN-SM-ISS"
                                If CatbaseRecIsstranno And billgencatcode <> "" Then billcontrolid = "CAT-" + billgencatcode + "-SM-ISS"
                                If _AccAudit Then billcontrolid = billcontrolid + "-APP"
                                TranNo = GetBillNoValue(billcontrolid, tran)
                        End Select
                    End If
                End If
                BatchNo = GetNewBatchno(cnCostId, dtpTrandate.Value.ToString("yyyy-MM-dd"), tran)
            Else

                StrSql = " SELECT SNO,TRANNO FROM  " & cnStockDb & "..RECEIPT WHERE BATCHNO = '" & EditBatchno & "' ORDER BY SNO "
                DtEditDet = New DataTable
                Cmd = New OleDbCommand(StrSql, cn, tran)
                Da = New OleDbDataAdapter(Cmd)
                Da.Fill(DtEditDet)

                StrSql = " SELECT RESNO FROM  " & cnStockDb & "..ISSUE WHERE BATCHNO = '" & EditBatchno & "' ORDER BY SNO "
                DtEditIss = New DataTable
                Cmd = New OleDbCommand(StrSql, cn, tran)
                Da = New OleDbDataAdapter(Cmd)
                Da.Fill(DtEditIss)

                StrSql = " DELETE FROM " & cnStockDb & "..ISSUE WHERE BATCHNO = '" & EditBatchno & "'"
                StrSql += vbCrLf + " DELETE FROM " & cnStockDb & "..ISSSTONE WHERE BATCHNO = '" & EditBatchno & "'"
                StrSql += vbCrLf + " DELETE FROM " & cnStockDb & "..ISSMISC WHERE BATCHNO = '" & EditBatchno & "'"
                StrSql += vbCrLf + " DELETE FROM " & cnStockDb & "..RECEIPT WHERE BATCHNO = '" & EditBatchno & "'"
                StrSql += vbCrLf + " DELETE FROM " & cnStockDb & "..RECEIPTSTONE WHERE BATCHNO = '" & EditBatchno & "'"
                StrSql += vbCrLf + " DELETE FROM " & cnStockDb & "..RECEIPTMISC WHERE BATCHNO = '" & EditBatchno & "'"
                StrSql += vbCrLf + " DELETE FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = '" & EditBatchno & "'"
                StrSql += vbCrLf + " DELETE FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO = '" & EditBatchno & "'"
                StrSql += vbCrLf + " DELETE FROM " & cnAdminDb & "..OUTSTANDING WHERE BATCHNO = '" & EditBatchno & "'"
                StrSql += vbCrLf + " DELETE FROM " & cnStockDb & "..ALLOYDETAILS WHERE BATCHNO = '" & EditBatchno & "'"
                StrSql += vbCrLf + " DELETE FROM " & cnStockDb & "..JOBNODETAILS WHERE BATCHNO = '" & EditBatchno & "'"
                ExecQuery(SyncMode.Transaction, StrSql, cn, tran, CostCenterId)
                If CostCenterId <> EditCostId Then
                    Exec(StrSql.Replace("'", "''"), cn, EditCostId, Nothing, tran)
                End If
                TranNo = EditTranNo
                BatchNo = EditBatchno
            End If

            For cnt As Integer = 0 To DgvTran.RowCount - 1

                If DgvTran.Rows(cnt).Cells("TRANTYPE").Value.ToString = "" Then
                    Exit For
                End If
                InsertIssRecDetails(cnt)
            Next
            If AccPost Then
                InsertSASRPUAccountDet()

                ''Cash Trans
                Dim TranMode As String = Nothing
                Dim RecPay As String = Nothing
                TranMode = IIf(Val(txtAdjCash_AMT.Text) > 0, "D", "C")
                If OMaterialType = MaterialType.Receipt Then TranMode = IIf(TranMode = "C", "D", "C")
                InsertIntoAccTran(TranNo, TranMode,
                CASHID, Val(txtAdjCash_AMT.Text), 0, 0, 0, "CA", _Accode, txtBillNo.Text, dtpBillDate_OWN.Value.ToString("yyyy-MM-dd"))
                TranMode = IIf(Val(txtAdjCash_AMT.Text) > 0, "C", "D")
                If OMaterialType = MaterialType.Receipt Then TranMode = IIf(TranMode = "C", "D", "C")
                InsertIntoAccTran(TranNo, TranMode,
                _Accode,
                Val(txtAdjCash_AMT.Text), 0, 0, 0, "CA", CASHID, txtBillNo.Text, dtpBillDate_OWN.Value.ToString("yyyy-MM-dd"))

                ''ROUND Trans
                TranMode = IIf(Val(txtAdjRoundOff_AMT.Text) > 0, "D", "C")
                If OMaterialType = MaterialType.Receipt Then TranMode = IIf(TranMode = "C", "D", "C")
                InsertIntoAccTran(TranNo, TranMode,
                "RNDOFF", Val(txtAdjRoundOff_AMT.Text), 0, 0, 0, "RO", _Accode, txtBillNo.Text, dtpBillDate_OWN.Value.ToString("yyyy-MM-dd"))
                TranMode = IIf(Val(txtAdjRoundOff_AMT.Text) > 0, "C", "D")
                If OMaterialType = MaterialType.Receipt Then TranMode = IIf(TranMode = "C", "D", "C")
                InsertIntoAccTran(TranNo, TranMode,
                _Accode _
                , Val(txtAdjRoundOff_AMT.Text), 0, 0, 0, "RO", "RNDOFF", txtBillNo.Text, dtpBillDate_OWN.Value.ToString("yyyy-MM-dd"))

                TranMode = IIf(Val(txtTCS_AMT.Text) > 0, "D", "C")
                InsertIntoAccTran(TranNo, TranMode,
                "TCS", Val(txtTCS_AMT.Text), 0, 0, 0, "TR", _Accode, txtBillNo.Text, dtpBillDate_OWN.Value.ToString("yyyy-MM-dd"))
                TranMode = IIf(Val(txtTCS_AMT.Text) > 0, "C", "D")
                InsertIntoAccTran(TranNo, TranMode,
                _Accode _
                , Val(txtTCS_AMT.Text), 0, 0, 0, "TR", "TCS", txtBillNo.Text, dtpBillDate_OWN.Value.ToString("yyyy-MM-dd"))


                Dim runNo As String = Nothing
                If Val(txtAdjCredit_AMT.Text) <> 0 Then
GETRUNNO:
                    If EditBatchno = Nothing Then
                        runNo = GetCostId(cnCostId) & GetCompanyId(strCompanyId) & "G" + Mid(Format(cnTranToDate, "dd/MM/yyyy"), 9, 2).ToString + GetTranDbSoftControlValue("RUNNO_ACC", True, tran)
                        StrSql = " SELECT RUNNO FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO = '" & runNo & "'"
                        If objGPack.GetSqlValue(StrSql, , "-1", tran) <> "-1" Then
                            GoTo GETRUNNO
                        End If
                    Else
                        runNo = EditRunno
                    End If
                    RecPay = IIf(Val(txtAdjReceive_AMT.Text) > 0, "P", "R")
                    '' it will (o/s condition)invoked as per Mr.Magesh & Mr.Jagadesh ref mailed "Points to discuss" dt on 12.03.14
                    If objGPack.GetSqlValue("SELECT OUTSTANDING FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = '" & _Accode & "'", , , tran).ToString = "Y" Then
                        If OMaterialType = MaterialType.Receipt Then RecPay = IIf(RecPay = "P", "R", "P")
                        InsertIntoOustanding("D", runNo, Val(txtAdjCredit_AMT.Text), RecPay, "DU", , , , , , , , , , IIf(dtpCreditDays.Visible, dtpCreditDays.Value.ToString("yyyy-MM-dd"), Nothing))

                        Dim psno As String = GetPersonalInfoSno(tran)
                        StrSql = " INSERT INTO " & cnAdminDb & "..PERSONALINFO"
                        StrSql += " ("
                        StrSql += " SNO,ACCODE,TRANDATE,TITLE"
                        StrSql += " ,INITIAL,PNAME,DOORNO,ADDRESS1"
                        StrSql += " ,ADDRESS2,ADDRESS3,AREA,CITY"
                        StrSql += " ,STATE,COUNTRY,PINCODE,PHONERES"
                        StrSql += " ,MOBILE,EMAIL,FAX,APPVER"
                        StrSql += " ,PREVILEGEID,COMPANYID,COSTID,PAN"
                        StrSql += " )"
                        StrSql += " SELECT TOP 1 '" & psno & "' SNO,ACCODE," & dtpTrandate.Value.ToString("yyyy-MM-dd") & " TRANDATE,TITLE"
                        StrSql += " ,INITIAL,ACNAME,DOORNO,ADDRESS1"
                        StrSql += " ,ADDRESS2,ADDRESS3,AREA,CITY"
                        StrSql += " ,STATE,COUNTRY,PINCODE,PHONENO"
                        StrSql += " ,MOBILE,EMAILID,FAX,'' APPVER"
                        StrSql += " ,PREVILEGEID,COMPANYID,COSTID,PAN FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE='" & _Accode & "'"
                        ExecQuery(SyncMode.Master, StrSql, cn, tran, CostCenterId)

                        StrSql = " IF NOT (SELECT COUNT(*) FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = '" & BatchNo & "')>0"
                        StrSql += vbCrLf + " BEGIN"
                        StrSql += " INSERT INTO " & cnAdminDb & "..CUSTOMERINFO"
                        StrSql += " (BATCHNO,PSNO"
                        StrSql += " ,REMARK1"
                        StrSql += " ,COSTID,PAN,DUEDATE,SMSALERT)VALUES"
                        StrSql += " ('" & BatchNo & "','" & psno & "'"
                        StrSql += " ,'" & txtRemark1.Text & "'"
                        StrSql += " ,'" & CostCenterId & "','',NULL,'N')"
                        StrSql += vbCrLf + " END"
                        ExecQuery(SyncMode.Transaction, StrSql, cn, tran, CostCenterId)
                    End If
                End If

                ''Cheque Trans
                For Each ro As DataRow In objCheaque.dtGridCheque.Rows
                    TranMode = IIf(Val(ro!AMOUNT.ToString) > 0, "D", "C")
                    If OMaterialType = MaterialType.Receipt Then TranMode = IIf(TranMode = "C", "D", "C")
                    InsertIntoAccTran(TranNo, TranMode,
                    objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & ro!DEFAULTBANK.ToString & "'", , , tran),
                    Val(ro!AMOUNT.ToString), 0, 0, 0, "CH", _Accode _
                    , txtBillNo.Text, dtpBillDate_OWN.Value.ToString("yyyy-MM-dd"), ro!CHEQUENO.ToString, ro!DATE.ToString, , ro!BANKNAME.ToString)
                    TranMode = IIf(Val(ro!AMOUNT.ToString) > 0, "C", "D")
                    If OMaterialType = MaterialType.Receipt Then TranMode = IIf(TranMode = "C", "D", "C")
                    InsertIntoAccTran(TranNo, TranMode,
                    _Accode,
                    Val(ro!AMOUNT.ToString), 0, 0, 0, "CH", objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & ro!DEFAULTBANK.ToString & "'", , , tran), txtBillNo.Text, dtpBillDate_OWN.Value.ToString("yyyy-MM-dd"), ro!CHEQUENO.ToString, ro!DATE.ToString, , ro!BANKNAME.ToString)
                Next

                ''ADL CHARGES
                For Each ro As DataRow In objAddlCharge.dtGridAddlCharges.Rows
                    Dim Acode As String = objGPack.GetSqlValue("select ACCODE from " & cnAdminDb & "..ADDCHARGE WHERE CHARGENAME = '" & ro.Item("CHARGENAME").ToString & "'", , , tran)
                    Dim cardid As Integer = Val(objGPack.GetSqlValue("select CHARGEID from " & cnAdminDb & "..ADDCHARGE WHERE CHARGENAME = '" & ro.Item("CHARGENAME").ToString & "'", , , tran))
                    InsertIntoAccTran(TranNo, "D",
                     Acode, Val(ro.Item("AMOUNT").ToString), 0, 0, 0, "AC", _Accode, txtBillNo.Text, dtpBillDate_OWN.Value.ToString("yyyy-MM-dd"), , , cardid)
                    InsertIntoAccTran(TranNo, "C",
                    _Accode _
                    , Val(ro.Item("AMOUNT").ToString), 0, 0, 0, "AC", Acode, txtBillNo.Text, dtpBillDate_OWN.Value.ToString("yyyy-MM-dd"), , , cardid)
                Next
                ''UPDATE CONTRA
                StrSql = " UPDATE " & cnStockDb & ".." & IIf(_AccAudit, "TACCTRAN", "ACCTRAN") & " SET CONTRA = "
                StrSql += " (SELECT TOP 1 ACCODE FROM " & cnStockDb & ".." & IIf(_AccAudit, "TACCTRAN", "ACCTRAN") & " WHERE BATCHNO = T.BATCHNO AND TRANMODE = 'C' AND ACCODE <> '' AND ACCODE <> T.ACCODE ORDER BY SNO)"
                StrSql += " FROM " & cnStockDb & ".." & IIf(_AccAudit, "TACCTRAN", "ACCTRAN") & " AS T"
                StrSql += " WHERE TRANDATE = '" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "' AND BATCHNO = '" & BatchNo & "' AND TRANMODE = 'D' AND ISNULL(CONTRA,'') = ''"
                ExecQuery(SyncMode.Transaction, StrSql, cn, tran, CostCenterId)
                StrSql = " UPDATE " & cnStockDb & ".." & IIf(_AccAudit, "TACCTRAN", "ACCTRAN") & " SET CONTRA = "
                StrSql += " (SELECT TOP 1 ACCODE FROM " & cnStockDb & ".." & IIf(_AccAudit, "TACCTRAN", "ACCTRAN") & " WHERE BATCHNO = T.BATCHNO AND TRANMODE = 'D' AND ACCODE <> '' AND ACCODE <> T.ACCODE ORDER BY SNO)"
                StrSql += " FROM " & cnStockDb & ".." & IIf(_AccAudit, "TACCTRAN", "ACCTRAN") & " AS T"
                StrSql += " WHERE TRANDATE = '" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "' AND BATCHNO = '" & BatchNo & "' AND TRANMODE = 'C'  AND ISNULL(CONTRA,'') = ''"
                ExecQuery(SyncMode.Transaction, StrSql, cn, tran, CostCenterId)
                Dim balAmt As Double = Val(objGPack.GetSqlValue("SELECT SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) AS BALANCE FROM " & cnStockDb & ".." & IIf(_AccAudit, "TACCTRAN", "ACCTRAN") & " WHERE BATCHNO = '" & BatchNo & "'", , "0", tran))
                Dim dtBal As New DataTable
                StrSql = "SELECT * FROM " & cnStockDb & ".." & IIf(_AccAudit, "TACCTRAN", "ACCTRAN") & " WHERE BATCHNO = '" & BatchNo & "'"
                Cmd = New OleDbCommand(StrSql, cn, tran)
                Da = New OleDbDataAdapter(Cmd)
                Da.Fill(dtBal)

                If balAmt <> 0 Then
                    If Not tran Is Nothing Then tran.Rollback()
                    tran = Nothing
                    MsgBox("Credit Debit Not Tally : " + balAmt.ToString, MsgBoxStyle.Information)
                    CalcCreditAmount(Me, New EventArgs)
                    txtAdjCash_AMT.Focus()
                    Exit Sub
                End If
            End If
            tran.Commit()
            tran = Nothing
            If EditBatchno = Nothing Then
                Dim msgdesc As String = "Tran No." & TranNo & " Generated.."
                If lotNo <> 0 Then msgdesc = msgdesc & vbCrLf & "Post to Lot No." & lotNo
                If TranNoApp <> 0 Then msgdesc = msgdesc & vbCrLf & "Issue No." & TranNoApp
                MsgBox(msgdesc)
                If SMS_MSG_MRMI <> "" And OMaterialType = MaterialType.Receipt Then
                    Dim TempMsg As String = ""
                    Dim dtSms As New DataTable
                    dtSms = DgvTran.DataSource
                    TempMsg = SMS_MSG_MRMI
                    TempMsg = Replace(SMS_MSG_MRMI, vbCrLf, "")
                    TempMsg = Replace(TempMsg, "<NAME>", cmbAcName.Text)
                    TempMsg = Replace(TempMsg, "<MRMI>", "Received")
                    TempMsg = Replace(TempMsg, "<CATNAME>", dtSms.Rows(0).Item("CATNAME").ToString)
                    TempMsg = Replace(TempMsg, "<GRSWT>", Val(dtSms.Compute("SUM(GRSWT)", Nothing).ToString))
                    TempMsg = Replace(TempMsg, "<AMOUNT>", Val(dtSms.Compute("SUM(AMOUNT)", Nothing).ToString))
                    TempMsg = Replace(TempMsg, "<BILLNO>", TranNo)
                    TempMsg = Replace(TempMsg, "<BILLDATE>", dtpTrandate.Value.ToString("dd-MM-yyyy"))
                    SmsSend(TempMsg, MobileNo)
                End If
                If SMS_MSG_MIMR <> "" And OMaterialType = MaterialType.Issue Then
                    Dim TempMsg As String = ""
                    Dim dtSms As New DataTable
                    dtSms = DgvTran.DataSource
                    TempMsg = SMS_MSG_MIMR
                    TempMsg = Replace(SMS_MSG_MIMR, vbCrLf, "")
                    TempMsg = Replace(TempMsg, "<NAME>", cmbAcName.Text)
                    TempMsg = Replace(TempMsg, "<MRMI>", "Issued")
                    TempMsg = Replace(TempMsg, "<CATNAME>", dtSms.Rows(0).Item("CATNAME").ToString)
                    TempMsg = Replace(TempMsg, "<GRSWT>", Val(dtSms.Compute("SUM(GRSWT)", Nothing).ToString))
                    TempMsg = Replace(TempMsg, "<AMOUNT>", Val(dtSms.Compute("SUM(AMOUNT)", Nothing).ToString))
                    TempMsg = Replace(TempMsg, "<BILLNO>", TranNo)
                    TempMsg = Replace(TempMsg, "<BILLDATE>", dtpTrandate.Value.ToString("dd-MM-yyyy"))
                    SmsSend(TempMsg, MobileNo)
                End If
            Else
                MsgBox(TranNo & " Updated..")
            End If
            lotNo = 0
            TranNoApp = 0
            Dim pBatchno As String = BatchNo
            Dim pBillDate As Date = dtpTrandate.Value.Date.ToString("yyyy-MM-dd")
            btnNew_Click(Me, New EventArgs)
            Dim prnmemsuffix As String = ""
            If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId
            Dim BillPrintExe As Boolean = IIf(GetAdmindbSoftValue("CALLBILLPRINTEXE", "Y") = "Y", True, False)
            Dim BillPrint_Format As String = GetAdmindbSoftValue("BILLPRINT_FORMAT", "")
            If GST And BillPrint_Format = "M1" Then
                Dim obj As New BrighttechREPORT.frmBillPrintDocA4N("POS", pBatchno, pBillDate.ToString("yyyy-MM-dd"), "")
            ElseIf GST And BillPrint_Format = "M2" Then
                Dim obj As New BrighttechREPORT.frmBillPrintDocB5("POS", pBatchno, pBillDate.ToString("yyyy-MM-dd"), "")
            ElseIf GST And BillPrint_Format = "M3" Then
                Dim obj As New BrighttechREPORT.frmBillPrintDocA5("POS", pBatchno, pBillDate.ToString("yyyy-MM-dd"), "")
            ElseIf GST And BillPrint_Format = "M4" Then
                Dim obj As New BrighttechREPORT.frmBillPrintDocB52cpy("POS", pBatchno, pBillDate.ToString("yyyy-MM-dd"), "")
            ElseIf GST And BillPrintExe = False Then
                Dim billDoc As New frmBillPrintDoc("POS", pBatchno, pBillDate.ToString("yyyy-MM-dd"), "")
            Else
                If IO.File.Exists(Application.StartupPath & "\BillPrint.exe") Then
                    Dim write As IO.StreamWriter
                    If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId
                    Dim memfile As String = "\BillPrint" + prnmemsuffix.Trim & ".mem"
                    write = IO.File.CreateText(Application.StartupPath & memfile)
                    If OMaterialType = MaterialType.Receipt Then
                        write.WriteLine(LSet("TYPE", 15) & ":SMR")
                    Else
                        write.WriteLine(LSet("TYPE", 15) & ":SMI")
                    End If
                    write.WriteLine(LSet("BATCHNO", 15) & ":" & pBatchno)
                    write.WriteLine(LSet("TRANDATE", 15) & ":" & pBillDate.ToString("yyyy-MM-dd"))
                    write.WriteLine(LSet("DUPLICATE", 15) & ":N")
                    write.Flush()
                    write.Close()
                    If EXE_WITH_PARAM = False Then
                        System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe")
                    Else
                        System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe",
                        LSet("TYPE", 15) & IIf(OMaterialType = MaterialType.Receipt, ":SMR", ":SMI") & ";" &
                        LSet("BATCHNO", 15) & ":" & pBatchno & ";" &
                        LSet("TRANDATE", 15) & ":" & pBillDate.ToString("yyyy-MM-dd") & ";" &
                        LSet("DUPLICATE", 15) & ":N")
                    End If
                Else
                    MsgBox("Billprint exe not found", MsgBoxStyle.Information)
                End If
            End If
            If EditBatchno <> Nothing Then
                Me.DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()
            End If
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        Finally
            If Not tran Is Nothing Then tran.Dispose()
        End Try
    End Sub

    Private Sub InsertSASRPUAccountDet()
        Dim dtCategory As New DataTable
        dtCategory = DtTran.DefaultView.ToTable(True, "ACCATNAME")
        InsertSASRPUAccountDet(TranNo, dtCategory, DtTran, IIf(OMaterialType = MaterialType.Issue, "TI", "TR"))
    End Sub

    Private Sub InsertSASRPUAccountDet(ByVal tNo As Integer, ByVal dtCategory As DataTable, ByVal matchTable As DataTable, ByVal insertTranType As String)
        For Each roCategory As DataRow In dtCategory.Rows
            Dim TdsRemark As String = Nothing
            Dim pcs As Integer = Nothing
            Dim grsWt As Double = Nothing
            Dim netWT As Double = Nothing
            Dim vatAmt As Double = Nothing
            Dim stnVat As Double = Nothing
            Dim amt As Double = Nothing
            Dim _MCamt As Double = Nothing
            Dim addCharge As Decimal = Nothing
            Dim Remark1 As String = Nothing
            Dim Remark2 As String = Nothing
            Dim TempAmount As Double = Nothing
            Dim TempVat As Double = Nothing
            Dim RowTemp As DataRow = Nothing
            Dim dtStoneAcc As New DataTable
            Dim dtMiscAcc As New DataTable
            Dim dtAlloyAcc As New DataTable
            Dim EDPer As Double = Nothing
            Dim EDAmt As Double = Nothing
            Dim TCSAmt As Double = Nothing
            Dim EDGrsAmt As Double = Nothing
            Dim SGSTAmt As Double = Nothing
            Dim CGSTAmt As Double = Nothing
            Dim IGSTAmt As Double = Nothing
            dtStoneAcc.Columns.Add("CODE", GetType(String))
            dtStoneAcc.Columns.Add("TAXCODE", GetType(String))
            dtStoneAcc.Columns.Add("TAXVALUE", GetType(Decimal))
            dtStoneAcc.Columns.Add("PCS", GetType(Decimal))
            dtStoneAcc.Columns.Add("GRSWT", GetType(Decimal))
            dtStoneAcc.Columns.Add("AMOUNT", GetType(Decimal))
            dtStoneAcc.Columns.Add("SGSTCODE", GetType(String))
            dtStoneAcc.Columns.Add("SGSTVALUE", GetType(Decimal))
            dtStoneAcc.Columns.Add("CGSTCODE", GetType(String))
            dtStoneAcc.Columns.Add("CGSTVALUE", GetType(Decimal))
            dtStoneAcc.Columns.Add("IGSTCODE", GetType(String))
            dtStoneAcc.Columns.Add("IGSTVALUE", GetType(Decimal))
            dtMiscAcc.Columns.Add("CODE", GetType(String))
            dtMiscAcc.Columns.Add("AMOUNT", GetType(Decimal))

            dtAlloyAcc.Columns.Add("ALLOYID", GetType(String))
            dtAlloyAcc.Columns.Add("WEIGHT", GetType(Decimal))
            Dim TdsContra As String = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME='" & cmbAcName.Text & "' ", , "", tran)
            Dim TTdscatid As Integer = Val(objGPack.GetSqlValue("SELECT ISNULL(TDSCATID,0) FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME='" & cmbAcName.Text & "' ", , "", tran))
            Dim TTdsAccode As String = "" 'tdscheck
            If TTdscatid <> 0 Then
                TTdsAccode = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE IN (SELECT TOP 1 ACCODE FROM " & cnAdminDb & "..TDSCATEGORY WHERE TDSCATID='" & TTdscatid & "') ", , "", tran)
            End If
            If _Tdsaccode.ToString <> TTdsAccode.ToString Then
                TTdsAccode = _Tdsaccode.ToString
            End If


            Dim IsOutStation As String = ""
            IsOutStation = objGPack.GetSqlValue("SELECT ISNULL(LOCALOUTST,'') FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME='" & cmbAcName.Text & "' ", , "", tran)

            If roCategory!ACCATNAME.ToString = "" Then Continue For
            For Each roMath As DataRow In matchTable.Rows
                If roMath.RowState = DataRowState.Deleted Then GoTo nnext
                If roMath!TRANTYPE.ToString = "" Then Exit For
                If roCategory!ACCATNAME.ToString <> roMath!ACCATNAME.ToString Then Continue For
                pcs += Val(roMath!PCS.ToString)
                grsWt += Val(roMath!GRSWT.ToString)
                netWT += Val(roMath!NETWT.ToString)
                If IsOutStation = "O" And cmbTransactionType.Text = "PURCHASE" And MRMI_VATSEPPOST = False Then
                    vatAmt += 0
                Else
                    vatAmt += Val(roMath!VAT.ToString)
                End If
                EDPer = Val(roMath!EDPER.ToString)
                EDAmt += Val(roMath!ED.ToString)
                TCSAmt += Val(roMath!TCS.ToString)
                SGSTAmt += Val(roMath!SGST.ToString)
                CGSTAmt += Val(roMath!CGST.ToString)
                IGSTAmt += Val(roMath!IGST.ToString)
                addCharge += Val(roMath!ADDCHARGE.ToString)
                If IsOutStation = "O" And cmbTransactionType.Text = "PURCHASE" And MRMI_VATSEPPOST = False Then
                    amt += Val(roMath!GROSSAMT.ToString) + Val(roMath!VAT.ToString)
                    _MCamt += Val(roMath!MC.ToString)
                    EDGrsAmt += Val(roMath!GROSSAMT.ToString)
                Else
                    amt += Val(roMath!GROSSAMT.ToString) ''- Val(roMath!DISCOUNT.ToString)
                    _MCamt += Val(roMath!MC.ToString)
                    EDGrsAmt += Val(roMath!GROSSAMT.ToString)
                End If
                If cmbTransactionType.Text <> "INTERNAL TRANSFER" Then

                    For Each stRow As DataRow In CType(roMath.Item("METISSREC"), MaterialIssRec).objStone.dtGridStone.Rows
                        Dim diaStn As String = objGPack.GetSqlValue("SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & stRow!ITEM.ToString & "'", , , tran).ToUpper
                        If Not (SepAccPost And SEPACCPOST_ITEM.Contains(diaStn)) Then Continue For
                        RowTemp = dtStoneAcc.NewRow
                        If OMaterialType = MaterialType.Issue Then
                            StrSql = " SELECT CA.SPRETURNID,CA.STAXID,CA.SALESTAX,S_SGSTID,S_CGSTID,S_IGSTID,S_SGSTTAX,S_CGSTTAX,S_IGSTTAX"
                        Else
                            StrSql = " SELECT CA.PURCHASEID,CA.PTAXID,CA.PTAX,P_SGSTID,P_CGSTID,P_IGSTID,P_SGSTTAX,P_CGSTTAX,P_IGSTTAX"
                        End If
                        StrSql += " FROM " & cnAdminDb & "..ITEMMAST AS IM "
                        StrSql += " INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = IM.CATCODE"
                        StrSql += " WHERE IM.ITEMNAME = '" & stRow!ITEM.ToString & "'"
                        Dim dtrow As DataRow = GetSqlRow(StrSql, cn, tran)
                        RowTemp!CODE = dtrow(0).ToString
                        RowTemp!TAXCODE = dtrow(1).ToString
                        RowTemp!PCS = Val(stRow!PCS.ToString)
                        RowTemp!GRSWT = Val(stRow!WEIGHT.ToString)
                        RowTemp!AMOUNT = Val(stRow!AMOUNT.ToString)
                        If vatAmt <> 0 Then RowTemp!TAXVALUE = Math.Round(Val(stRow!AMOUNT.ToString) * (Val(dtrow(2).ToString) / 100), 2)
                        If GstFlag Then
                            If OMaterialType = MaterialType.Issue Then
                                RowTemp!SGSTCODE = dtrow("S_SGSTID").ToString
                                RowTemp!CGSTCODE = dtrow("S_CGSTID").ToString
                                RowTemp!IGSTCODE = dtrow("S_IGSTID").ToString
                                If vatAmt <> 0 Then
                                    If InterStateBill Then
                                        RowTemp!IGSTVALUE = Math.Round(Val(stRow!AMOUNT.ToString) * (Val(dtrow("S_IGSTTAX").ToString) / 100), 2)
                                    Else
                                        RowTemp!CGSTVALUE = Math.Round(Val(stRow!AMOUNT.ToString) * (Val(dtrow("S_CGSTTAX").ToString) / 100), 2)
                                        RowTemp!SGSTVALUE = Math.Round(Val(stRow!AMOUNT.ToString) * (Val(dtrow("S_SGSTTAX").ToString) / 100), 2)
                                    End If
                                End If
                            Else
                                RowTemp!SGSTCODE = dtrow("P_SGSTID").ToString
                                RowTemp!CGSTCODE = dtrow("P_CGSTID").ToString
                                RowTemp!IGSTCODE = dtrow("P_IGSTID").ToString
                                If vatAmt <> 0 Or ((SGSTAmt <> 0 Or CGSTAmt <> 0 Or IGSTAmt <> 0) And cmbTransactionType.Text = "PURCHASE") Then ''''vatAmt <> 0 already this condition only given till 29-June-2021
                                    If InterStateBill Then
                                        RowTemp!IGSTVALUE = Math.Round(Val(stRow!AMOUNT.ToString) * (Val(dtrow("P_IGSTTAX").ToString) / 100), 2)
                                    Else
                                        RowTemp!CGSTVALUE = Math.Round(Val(stRow!AMOUNT.ToString) * (Val(dtrow("P_CGSTTAX").ToString) / 100), 2)
                                        RowTemp!SGSTVALUE = Math.Round(Val(stRow!AMOUNT.ToString) * (Val(dtrow("P_SGSTTAX").ToString) / 100), 2)
                                    End If
                                End If
                            End If
                        End If
                        dtStoneAcc.Rows.Add(RowTemp)
                    Next

                    For Each stRow As DataRow In CType(roMath.Item("METISSREC"), MaterialIssRec).objMisc.dtGridMisc.Rows
                        RowTemp = dtMiscAcc.NewRow
                        StrSql = " SELECT ACCTID FROM " & cnAdminDb & "..MISCCHARGES WHERE MISCNAME = '" & stRow!MISC.ToString & "'"
                        RowTemp!CODE = objGPack.GetSqlValue(StrSql, , , tran)
                        RowTemp!AMOUNT = Val(stRow!AMOUNT.ToString)
                        dtMiscAcc.Rows.Add(RowTemp)
                    Next
                    For Each stRow As DataRow In CType(roMath.Item("METISSREC"), MaterialIssRec).ObjAlloy.dtGridAlloy.Rows
                        RowTemp = dtAlloyAcc.NewRow
                        StrSql = " SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & stRow!ALLOY.ToString & "'"
                        RowTemp!ALLOYID = objGPack.GetSqlValue(StrSql, , , tran)
                        RowTemp!WEIGHT = Val(stRow!WEIGHT.ToString)
                        dtAlloyAcc.Rows.Add(RowTemp)
                    Next
                End If
nnext:
            Next
            Dim temptdsamt As Double = 0
            Dim tranMode As String = Nothing
            Dim accode As String = Nothing
            Dim payMode As String = Nothing
            If cmbTransactionType.Text <> "INTERNAL TRANSFER" Then
                If OMaterialType = MaterialType.Receipt Then
                    tranMode = "D"
                    payMode = "TR"
                Else
                    tranMode = "C"
                    payMode = "TI"
                End If
                ''StuddedPosting
                If vatAmt <> 0 Or ((SGSTAmt <> 0 Or CGSTAmt <> 0 Or IGSTAmt <> 0) And cmbTransactionType.Text = "PURCHASE") Then  ''''vatAmt <> 0 already this condition only given till 29-June-2021
                    For Each RowStuddedTax As DataRow In dtStoneAcc.DefaultView.ToTable(True, "TAXCODE", "SGSTCODE", "CGSTCODE", "IGSTCODE").Rows
                        accode = RowStuddedTax!TAXCODE.ToString
                        If GstFlag Then
                            TempVat = Val(dtStoneAcc.Compute("SUM(SGSTVALUE)", "TAXCODE = '" & accode & "'").ToString)
                            TempVat += Val(dtStoneAcc.Compute("SUM(CGSTVALUE)", "TAXCODE = '" & accode & "'").ToString)
                            TempVat += Val(dtStoneAcc.Compute("SUM(IGSTVALUE)", "TAXCODE = '" & accode & "'").ToString)
                        Else
                            TempVat = Val(dtStoneAcc.Compute("SUM(TAXVALUE)", "TAXCODE = '" & accode & "'").ToString)
                        End If
                        stnVat += TempVat
                        Dim Taxaccode As String = Nothing
                        If Not (_Acctype = "O" And CstPurchsep = False And (cmbTransactionType.Text = "PURCHASE" Or cmbTransactionType.Text = "PURCHASE[APPROVAL]")) And cmbTransactionType.Text <> "RECEIPT" Then
                            If GstFlag Then
                                Dim StTax As Double = 0
                                Taxaccode = RowStuddedTax!SGSTCODE.ToString
                                StTax = Val(dtStoneAcc.Compute("SUM(SGSTVALUE)", "TAXCODE = '" & accode & "'").ToString)
                                InsertIntoAccTran(tNo, tranMode, Taxaccode, StTax _
                                , 0, 0, 0 _
                                , payMode, _Accode, txtBillNo.Text, dtpBillDate_OWN.Value.ToString("yyyy-MM-dd"))
                                SGSTAmt -= StTax
                                If cmbTransactionType.Text <> "PURCHASE" Then vatAmt -= StTax
                                StTax = 0
                                Taxaccode = RowStuddedTax!CGSTCODE.ToString
                                StTax = Val(dtStoneAcc.Compute("SUM(CGSTVALUE)", "TAXCODE = '" & accode & "'").ToString)
                                InsertIntoAccTran(tNo, tranMode, Taxaccode, StTax, 0, 0, 0, payMode, _Accode, txtBillNo.Text, dtpBillDate_OWN.Value.ToString("yyyy-MM-dd"))
                                CGSTAmt -= StTax
                                If cmbTransactionType.Text <> "PURCHASE" Then vatAmt -= StTax
                                StTax = 0
                                Taxaccode = RowStuddedTax!IGSTCODE.ToString
                                StTax = Val(dtStoneAcc.Compute("SUM(IGSTVALUE)", "TAXCODE = '" & accode & "'").ToString)
                                InsertIntoAccTran(tNo, tranMode, Taxaccode, StTax _
                                , 0, 0, 0 _
                                , payMode, _Accode, txtBillNo.Text, dtpBillDate_OWN.Value.ToString("yyyy-MM-dd"))
                                IGSTAmt -= StTax
                                If cmbTransactionType.Text <> "PURCHASE" Then vatAmt -= StTax
                                StTax = 0
                            Else
                                InsertIntoAccTran(tNo, tranMode, accode, TempVat,
                                        Val(dtStoneAcc.Compute("SUM(PCS)", "TAXCODE = '" & accode & "'").ToString),
                                        Val(dtStoneAcc.Compute("SUM(GRSWT)", "TAXCODE = '" & accode & "'").ToString),
                                        Val(dtStoneAcc.Compute("SUM(GRSWT)", "TAXCODE = '" & accode & "'").ToString),
                                        payMode, _Accode, txtBillNo.Text, dtpBillDate_OWN.Value.ToString("yyyy-MM-dd"))
                                vatAmt -= TempVat
                            End If
                        End If
                        TempAmount = Nothing
                    Next
                End If
                For Each RowStuddedAcc As DataRow In dtStoneAcc.DefaultView.ToTable(True, "CODE").Rows
                    accode = RowStuddedAcc!CODE.ToString
                    TempAmount = Val(dtStoneAcc.Compute("SUM(AMOUNT)", "CODE = '" & accode & "'").ToString)
                    If Not (_Acctype = "O" And CstPurchsep = False And (cmbTransactionType.Text = "PURCHASE" Or cmbTransactionType.Text = "PURCHASE[APPROVAL]")) Then
                        InsertIntoAccTran(tNo, tranMode, accode, TempAmount,
                            Val(dtStoneAcc.Compute("SUM(PCS)", "CODE = '" & accode & "'").ToString),
                            Val(dtStoneAcc.Compute("SUM(GRSWT)", "CODE = '" & accode & "'").ToString),
                            Val(dtStoneAcc.Compute("SUM(GRSWT)", "CODE = '" & accode & "'").ToString),
                            payMode, _Accode, txtBillNo.Text, dtpBillDate_OWN.Value.ToString("yyyy-MM-dd"))
                        InsertIntoAccTran(tNo, IIf(tranMode = "D", "C", "D"), _Accode, TempAmount,
                            Val(dtStoneAcc.Compute("SUM(PCS)", "CODE = '" & accode & "'").ToString),
                            Val(dtStoneAcc.Compute("SUM(GRSWT)", "CODE = '" & accode & "'").ToString),
                            Val(dtStoneAcc.Compute("SUM(GRSWT)", "CODE = '" & accode & "'").ToString),
                            payMode, accode, txtBillNo.Text, dtpBillDate_OWN.Value.ToString("yyyy-MM-dd"))
                        amt -= TempAmount
                    End If
                    TempAmount = Nothing
                Next

                ''MiscChargePost
                If OMaterialType = MaterialType.Receipt And (cmbTransactionType.Text <> "PURCHASE" Or cmbTransactionType.Text <> "PURCHASE[APPROVAL]") Then
                    For Each RowMiscAcc As DataRow In dtMiscAcc.DefaultView.ToTable(True, "CODE").Rows
                        accode = RowMiscAcc!CODE.ToString
                        TempAmount = Val(dtMiscAcc.Compute("SUM(AMOUNT)", "CODE = '" & accode & "'").ToString)
                        Dim tempvatamt As Double
                        If Not (_Acctype = "O" And CstPurchsep = False And (cmbTransactionType.Text = "PURCHASE" Or cmbTransactionType.Text <> "PURCHASE[APPROVAL]")) Then
                            InsertIntoAccTran(tNo, tranMode, accode, TempAmount, 0, 0, 0, payMode, _Accode, txtBillNo.Text, dtpBillDate_OWN.Value.ToString("yyyy-MM-dd"))
                            If cmbTransactionType.Text = "RECEIPT" And vatAmt <> 0 And TdsPer <> 0 Then
                                tempvatamt = TempAmount * (TdsPer / 100)
                                temptdsamt += tempvatamt
                                TdsRemark = "TDS Rs " & Format(tempvatamt, "0.00") & " @" & Format(TdsPer, "0.00") & "% for " & Format(TempAmount, "0.00")
                            End If
                            InsertIntoAccTran(tNo, IIf(tranMode = "D", "C", "D"), _Accode, TempAmount, 0, 0, 0, payMode, accode, txtBillNo.Text, dtpBillDate_OWN.Value.ToString("yyyy-MM-dd"), , , , TdsRemark, , , IIf(tempvatamt <> 0, IIf(TTdscatid <> 0, TTdscatid, TdsCatId), 0), TdsPer, tempvatamt)
                            amt -= TempAmount
                        End If
                        TempAmount = Nothing
                    Next
                End If

            End If
            'End If

            ''INSERTING SALES ACC

            If OMaterialType = MaterialType.Receipt Then
                'Dim SepPurAccPost As Boolean = IIf(GetAdmindbSoftValue("SEPPOST_ACC_PURCHASE", "Y", tran) = "Y", True, False)
                ''INSERTING PURCHASE ACC
                If amt <> 0 Then
                    If cmbTransactionType.Text = "INTERNAL TRANSFER" Then
                        tranMode = "D"
                        payMode = "TR"
                        accode = "STKTRAN"
                        InsertIntoAccTran(tNo, tranMode, accode, amt, pcs, grsWt, netWT, payMode, _Accode, dtpBillDate_OWN.Value.ToString("yyyy-MM-dd"))
                        InsertIntoAccTran(tNo, IIf(tranMode = "D", "C", "D"),
                        _Accode,
                        (amt + SGSTAmt + CGSTAmt + IGSTAmt), pcs, grsWt, netWT, payMode, accode, txtBillNo.Text, dtpBillDate_OWN.Value.ToString("yyyy-MM-dd"))
                    ElseIf cmbTransactionType.Text = "PURCHASE" Or cmbTransactionType.Text = "PURCHASE[APPROVAL]" Then
                        stnVat = stnVat - TempVat
                        tranMode = "D"
                        payMode = "TR"
                        Dim mamt As Decimal = 0
                        StrSql = " SELECT PURCHASEID"
                        StrSql += " FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & roCategory!ACCATNAME.ToString & "'"
                        accode = objGPack.GetSqlValue(StrSql, , , tran)
                        'Newly Comment on 29-June-2021
                        'If Not (_Acctype = "O" And CstPurchsep = False) Then mamt = amt Else mamt = amt + vatAmt
                        If Not (_Acctype = "O" And CstPurchsep = False) Then mamt = amt Else mamt = amt + SGSTAmt + CGSTAmt + IGSTAmt
                        InsertIntoAccTran(tNo, tranMode, accode, mamt, pcs, grsWt, netWT, payMode, _Accode, txtBillNo.Text, dtpBillDate_OWN.Value.ToString("yyyy-MM-dd") _
                        , , , , , , , , , , , , , SGSTAmt, CGSTAmt, IGSTAmt)
                        InsertIntoAccTran(tNo, tranMode, "EXDUTY", EDAmt, 0, 0, 0, payMode,
                        _Accode, txtBillNo.Text, dtpBillDate_OWN.Value.ToString("yyyy-MM-dd") _
                        , , , , , , , , , , EDPer, EDGrsAmt, EDAmt)

                        InsertIntoAccTran(tNo, tranMode, "TCS", TCSAmt, 0, 0, 0, payMode,
                        _Accode, txtBillNo.Text, dtpBillDate_OWN.Value.ToString("yyyy-MM-dd") _
                        , , , , , , , , , , EDPer, EDGrsAmt)
                        'Newly Comment on 29-June-2021
                        'InsertIntoAccTran(tNo, IIf(tranMode = "D", "C", "D"),
                        '_Accode,
                        'amt + vatAmt + stnVat + TempVat + EDAmt + TCSAmt, pcs, grsWt, netWT, payMode, accode, txtBillNo.Text, dtpBillDate_OWN.Value.ToString("yyyy-MM-dd"))
                        InsertIntoAccTran(tNo, IIf(tranMode = "D", "C", "D"),
                        _Accode,
                        amt + SGSTAmt + CGSTAmt + IGSTAmt + stnVat + TempVat + EDAmt + TCSAmt, pcs, grsWt, netWT, payMode, accode, txtBillNo.Text, dtpBillDate_OWN.Value.ToString("yyyy-MM-dd"))
                        TdsRemark = ""
                        TdsRemark = "TDS Rs " & Format(vatAmt, "0.00") & " @" & Format(TdsPer, "0.00") & "% for " & Format(amt, "0.00")
                        'tranMode = "D"
                        payMode = "TR"
                        'StrSql = " SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME= '" & roCategory!ACCATNAME.ToString & "'"
                        'accode = objGPack.GetSqlValue(StrSql, , , tran) + "MAKP"
                        InsertIntoAccTran(tNo, "C", IIf(TTdsAccode.ToString <> "", TTdsAccode.ToString, "TDSIN"), vatAmt, pcs, grsWt, netWT, payMode, _Accode, txtBillNo.Text, dtpBillDate_OWN.Value.ToString("yyyy-MM-dd")) ''"TDSIN"
                        InsertIntoAccTran(tNo, "D",
                        _Accode, (vatAmt), pcs, grsWt, netWT, payMode, IIf(TTdsAccode.ToString <> "", TTdsAccode.ToString, "TDSIN"), txtBillNo.Text, dtpBillDate_OWN.Value.ToString("yyyy-MM-dd"), , , , , , , IIf(vatAmt - temptdsamt <> 0, IIf(TTdscatid <> 0, TTdscatid, TdsCatId), 0), IIf(vatAmt - temptdsamt <> 0, TdsPer, 0), IIf(vatAmt - temptdsamt <> 0, vatAmt - temptdsamt, 0)) ''"TDSIN"
                        If vatAmt <> 0 Then
                            StrSql = " INSERT INTO " & cnStockDb & "..TAXTRAN"
                            StrSql += " ("
                            StrSql += " SNO,ACCODE,CONTRA,TRANNO,TRANDATE,TRANTYPE,BATCHNO,TAXID"
                            StrSql += " ,AMOUNT,TAXPER,TAXAMOUNT,TAXTYPE,TSNO,COSTID,COMPANYID"
                            StrSql += " )"
                            StrSql += " VALUES("
                            StrSql += " '" & GetNewSno(IIf(_AccAudit, TranSnoType.TAXTRANCODE, TranSnoType.TAXTRANCODE), tran) & "'" ''SNO
                            StrSql += " ,'" & IIf(TTdsAccode.ToString <> "", TTdsAccode.ToString, "TDSIN") & "'" ''TDSIN
                            StrSql += " ,'" & _Accode & "'"
                            StrSql += " ," & TranNo & "" 'TRANNO
                            StrSql += " ,'" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                            StrSql += " ,'" & payMode & "'"
                            StrSql += " ,'" & BatchNo & "'" 'BATCHNO
                            StrSql += " ,'" & IIf(vatAmt - temptdsamt <> 0, IIf(TTdscatid <> 0, TTdscatid, TdsCatId), 0) & "'" 'TAXID
                            StrSql += " ," & amt & "" 'AMOUNT
                            StrSql += " ," & TdsPer & "" 'TAXPER
                            StrSql += " ," & vatAmt
                            StrSql += " ,'TD'"
                            StrSql += " ,4" 'TSNO
                            StrSql += " ,'" & CostCenterId & "'"
                            StrSql += " ,'" & strCompanyId & "'"
                            StrSql += " )"
                            ExecQuery(SyncMode.Transaction, StrSql, cn, tran, CostCenterId)
                        End If
                    Else
                        TdsRemark = "TDS Rs " & Format(vatAmt, "0.00") & " @" & Format(TdsPer, "0.00") & "% for " & Format(amt, "0.00")
                        tranMode = "D"
                        payMode = "TR"
                        '' CHANGE FOR VBJ MC ACCOUNT 
                        accode = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID='LABOUR_ACC'", , , tran)
                        If accode = "" Then
                            StrSql = " SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME= '" & roCategory!ACCATNAME.ToString & "'"
                            accode = objGPack.GetSqlValue(StrSql, , , tran) + "MAKP"
                        End If

                        InsertIntoAccTran(tNo, tranMode, accode, amt, pcs, grsWt, netWT, payMode, _Accode, txtBillNo.Text, dtpBillDate_OWN.Value.ToString("yyyy-MM-dd"))
                        InsertIntoAccTran(tNo, IIf(tranMode = "D", "C", "D"),
                        _Accode,
                        (amt + SGSTAmt + CGSTAmt + IGSTAmt), pcs, grsWt, netWT, payMode, accode, txtBillNo.Text, dtpBillDate_OWN.Value.ToString("yyyy-MM-dd"), , , , , , , IIf(vatAmt - temptdsamt <> 0, IIf(TTdscatid <> 0, TTdscatid, TdsCatId), 0), IIf(vatAmt - temptdsamt <> 0, TdsPer, 0), IIf(vatAmt - temptdsamt <> 0, vatAmt - temptdsamt, 0))
                        If (vatAmt <> 0) Then
                            If GstFlag And Val(SGSTAmt + CGSTAmt + IGSTAmt) > 0 And Val(SGSTAmt + CGSTAmt + IGSTAmt) = Val(vatAmt) Then
                                If InterStateBill = False Then
                                    StrSql = " INSERT INTO " & cnStockDb & "..TAXTRAN"
                                    StrSql += " ("
                                    StrSql += " SNO,ACCODE,CONTRA,TRANNO,TRANDATE,TRANTYPE,BATCHNO,TAXID"
                                    StrSql += " ,AMOUNT,TAXPER,TAXAMOUNT,TAXTYPE,TSNO,COSTID,COMPANYID"
                                    StrSql += " )"
                                    StrSql += " VALUES("
                                    StrSql += " '" & GetNewSno(TranSnoType.TAXTRANCODE, tran) & "'" ''SNO
                                    StrSql += " ,'" & accode & "'"
                                    StrSql += " ,'" & TdsContra & "'"
                                    StrSql += " ," & TranNo & "" 'TRANNO
                                    StrSql += " ,'" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                                    StrSql += " ,'" & payMode & "'"
                                    StrSql += " ,'" & BatchNo & "'" 'BATCHNO
                                    StrSql += " ,'" & IIf(vatAmt - temptdsamt <> 0, IIf(TTdscatid <> 0, TTdscatid, TdsCatId), 0) & "'" 'TAXID
                                    StrSql += " ," & amt & "" 'AMOUNT
                                    StrSql += " ," & Math.Round((TdsPer / 2), 2) & "" 'TAXPER
                                    StrSql += " ," & Math.Round((vatAmt / 2), 2)
                                    StrSql += " ,'SG'"
                                    StrSql += " ,1" 'TSNO
                                    StrSql += " ,'" & CostCenterId & "'"
                                    StrSql += " ,'" & strCompanyId & "'"
                                    StrSql += " )"
                                    ExecQuery(SyncMode.Transaction, StrSql, cn, tran, CostCenterId)
                                    StrSql = " INSERT INTO " & cnStockDb & "..TAXTRAN"
                                    StrSql += " ("
                                    StrSql += " SNO,ACCODE,CONTRA,TRANNO,TRANDATE,TRANTYPE,BATCHNO,TAXID"
                                    StrSql += " ,AMOUNT,TAXPER,TAXAMOUNT,TAXTYPE,TSNO,COSTID,COMPANYID"
                                    StrSql += " )"
                                    StrSql += " VALUES("
                                    StrSql += " '" & GetNewSno(TranSnoType.TAXTRANCODE, tran) & "'" ''SNO
                                    StrSql += " ,'" & accode & "'"
                                    StrSql += " ,'" & TdsContra & "'"
                                    StrSql += " ," & TranNo & "" 'TRANNO
                                    StrSql += " ,'" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                                    StrSql += " ,'" & payMode & "'"
                                    StrSql += " ,'" & BatchNo & "'" 'BATCHNO
                                    StrSql += " ,'" & IIf(vatAmt - temptdsamt <> 0, IIf(TTdscatid <> 0, TTdscatid, TdsCatId), 0) & "'" 'TAXID
                                    StrSql += " ," & amt & "" 'AMOUNT
                                    StrSql += " ," & Math.Round((TdsPer / 2), 2) & "" 'TAXPER
                                    StrSql += " ," & Math.Round((vatAmt / 2), 2)
                                    StrSql += " ,'CG'"
                                    StrSql += " ,2" 'TSNO
                                    StrSql += " ,'" & CostCenterId & "'"
                                    StrSql += " ,'" & strCompanyId & "'"
                                    StrSql += " )"
                                    ExecQuery(SyncMode.Transaction, StrSql, cn, tran, CostCenterId)
                                Else
                                    StrSql = " INSERT INTO " & cnStockDb & "..TAXTRAN"
                                    StrSql += " ("
                                    StrSql += " SNO,ACCODE,CONTRA,TRANNO,TRANDATE,TRANTYPE,BATCHNO,TAXID"
                                    StrSql += " ,AMOUNT,TAXPER,TAXAMOUNT,TAXTYPE,TSNO,COSTID,COMPANYID"
                                    StrSql += " )"
                                    StrSql += " VALUES("
                                    StrSql += " '" & GetNewSno(TranSnoType.TAXTRANCODE, tran) & "'" ''SNO
                                    StrSql += " ,'" & accode & "'"
                                    StrSql += " ,'" & TdsContra & "'"
                                    StrSql += " ," & TranNo & "" 'TRANNO
                                    StrSql += " ,'" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                                    StrSql += " ,'" & payMode & "'"
                                    StrSql += " ,'" & BatchNo & "'" 'BATCHNO
                                    StrSql += " ,'" & IIf(vatAmt - temptdsamt <> 0, IIf(TTdscatid <> 0, TTdscatid, TdsCatId), 0) & "'" 'TAXID
                                    StrSql += " ," & amt & "" 'AMOUNT
                                    StrSql += " ," & TdsPer & "" 'TAXPER
                                    StrSql += " ," & vatAmt
                                    StrSql += " ,'IG'"
                                    StrSql += " ,3" 'TSNO
                                    StrSql += " ,'" & CostCenterId & "'"
                                    StrSql += " ,'" & strCompanyId & "'"
                                    StrSql += " )"
                                    ExecQuery(SyncMode.Transaction, StrSql, cn, tran, CostCenterId)
                                End If
                            Else
                                StrSql = " INSERT INTO " & cnStockDb & "..TAXTRAN"
                                StrSql += " ("
                                StrSql += " SNO,ACCODE,CONTRA,TRANNO,TRANDATE,TRANTYPE,BATCHNO,TAXID"
                                StrSql += " ,AMOUNT,TAXPER,TAXAMOUNT,TAXTYPE,TSNO,COSTID,COMPANYID"
                                StrSql += " )"
                                StrSql += " VALUES("
                                StrSql += " '" & GetNewSno(TranSnoType.TAXTRANCODE, tran) & "'" ''SNO
                                StrSql += " ,'" & accode & "'"
                                StrSql += " ,'" & TdsContra & "'"
                                StrSql += " ," & TranNo & "" 'TRANNO
                                StrSql += " ,'" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                                StrSql += " ,'" & payMode & "'"
                                StrSql += " ,'" & BatchNo & "'" 'BATCHNO
                                StrSql += " ,'" & IIf(vatAmt - temptdsamt <> 0, IIf(TTdscatid <> 0, TTdscatid, TdsCatId), 0) & "'" 'TAXID
                                StrSql += " ," & amt & "" 'AMOUNT
                                StrSql += " ," & TdsPer & "" 'TAXPER
                                StrSql += " ," & vatAmt
                                StrSql += " ,'TD'"
                                StrSql += " ,4" 'TSNO
                                StrSql += " ,'" & CostCenterId & "'"
                                StrSql += " ,'" & strCompanyId & "'"
                                StrSql += " )"
                                ExecQuery(SyncMode.Transaction, StrSql, cn, tran, CostCenterId)
                            End If
                        End If
                        If SGSTAmt > 0 Then
                            If cmbTransactionType.Text = "RECEIPT" Then
                                Dim splitAccode() As String
                                StrSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID='LABOUR_SGST'"
                                splitAccode = objGPack.GetSqlValue(StrSql, , , tran).Split(":")
                                accode = splitAccode(0)
                                If accode.ToString = "" Then
                                    StrSql = "SELECT MC_SGSTID FROM " & cnAdminDb & "..CATEGORY "
                                    StrSql += " WHERE CATNAME = '" & roCategory!ACCATNAME.ToString & "'"
                                    accode = objGPack.GetSqlValue(StrSql, , , tran)
                                    If accode.ToString = "" Then
                                        StrSql = "SELECT P_SGSTID FROM " & cnAdminDb & "..CATEGORY "
                                        StrSql += " WHERE CATNAME = '" & roCategory!ACCATNAME.ToString & "'"
                                        accode = objGPack.GetSqlValue(StrSql, , , tran)
                                    End If
                                End If
                            Else
                                If amt = _MCamt Then
                                    StrSql = "SELECT MC_SGSTID FROM " & cnAdminDb & "..CATEGORY "
                                    StrSql += " WHERE CATNAME = '" & roCategory!ACCATNAME.ToString & "'"
                                    accode = objGPack.GetSqlValue(StrSql, , , tran)
                                Else
                                    StrSql = "SELECT P_SGSTID FROM " & cnAdminDb & "..CATEGORY "
                                    StrSql += " WHERE CATNAME = '" & roCategory!ACCATNAME.ToString & "'"
                                    accode = objGPack.GetSqlValue(StrSql, , , tran)
                                End If
                                If accode.ToString = "" Then
                                    StrSql = "SELECT P_SGSTID FROM " & cnAdminDb & "..CATEGORY "
                                    StrSql += " WHERE CATNAME = '" & roCategory!ACCATNAME.ToString & "'"
                                    accode = objGPack.GetSqlValue(StrSql, , , tran)
                                End If
                            End If
                            If SrvTaxSGCode <> "" And SrvTaxCGCode <> "" Then
                                accode = SrvTaxSGCode
                            End If
                            InsertIntoAccTran(tNo, tranMode, accode, SGSTAmt, 0, 0, 0, payMode, _Accode, txtBillNo.Text, dtpBillDate_OWN.Value.ToString("yyyy-MM-dd"))
                        End If
                        If CGSTAmt > 0 Then

                            If cmbTransactionType.Text = "RECEIPT" Then
                                Dim splitAccode() As String
                                StrSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID='LABOUR_CGST'"
                                splitAccode = objGPack.GetSqlValue(StrSql, , , tran).Split(":")
                                accode = splitAccode(0)
                                If accode.ToString = "" Then
                                    StrSql = "SELECT MC_CGSTID FROM " & cnAdminDb & "..CATEGORY "
                                    StrSql += " WHERE CATNAME = '" & roCategory!ACCATNAME.ToString & "'"
                                    accode = objGPack.GetSqlValue(StrSql, , , tran)
                                    If accode.ToString = "" Then
                                        StrSql = "SELECT P_CGSTID FROM " & cnAdminDb & "..CATEGORY "
                                        StrSql += " WHERE CATNAME = '" & roCategory!ACCATNAME.ToString & "'"
                                        accode = objGPack.GetSqlValue(StrSql, , , tran)
                                    End If
                                End If
                            Else
                                If amt = _MCamt Then
                                    StrSql = "SELECT MC_CGSTID FROM " & cnAdminDb & "..CATEGORY "
                                    StrSql += " WHERE CATNAME = '" & roCategory!ACCATNAME.ToString & "'"
                                    accode = objGPack.GetSqlValue(StrSql, , , tran)
                                Else
                                    StrSql = "SELECT P_CGSTID FROM " & cnAdminDb & "..CATEGORY "
                                    StrSql += " WHERE CATNAME = '" & roCategory!ACCATNAME.ToString & "'"
                                    accode = objGPack.GetSqlValue(StrSql, , , tran)
                                End If
                                If accode.ToString = "" Then
                                    StrSql = "SELECT P_CGSTID FROM " & cnAdminDb & "..CATEGORY "
                                    StrSql += " WHERE CATNAME = '" & roCategory!ACCATNAME.ToString & "'"
                                    accode = objGPack.GetSqlValue(StrSql, , , tran)
                                End If
                            End If
                            If SrvTaxSGCode <> "" And SrvTaxCGCode <> "" Then
                                accode = SrvTaxCGCode
                            End If
                            InsertIntoAccTran(tNo, tranMode, accode, CGSTAmt, 0, 0, 0, payMode, _Accode, txtBillNo.Text, dtpBillDate_OWN.Value.ToString("yyyy-MM-dd"))
                        End If
                        If IGSTAmt > 0 Then
                            If cmbTransactionType.Text = "RECEIPT" Then
                                Dim splitAccode() As String
                                StrSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID='LABOUR_IGST'"
                                splitAccode = objGPack.GetSqlValue(StrSql, , , tran).Split(":")
                                accode = splitAccode(0)
                                If accode.ToString = "" Then
                                    StrSql = "SELECT MC_IGSTID FROM " & cnAdminDb & "..CATEGORY "
                                    StrSql += " WHERE CATNAME = '" & roCategory!ACCATNAME.ToString & "'"
                                    accode = objGPack.GetSqlValue(StrSql, , , tran)
                                    If accode.ToString = "" Then
                                        StrSql = "SELECT P_IGSTID FROM " & cnAdminDb & "..CATEGORY "
                                        StrSql += " WHERE CATNAME = '" & roCategory!ACCATNAME.ToString & "'"
                                        accode = objGPack.GetSqlValue(StrSql, , , tran)
                                    End If
                                End If
                            Else
                                If amt = _MCamt Then
                                    StrSql = "SELECT MC_IGSTID FROM " & cnAdminDb & "..CATEGORY "
                                    StrSql += " WHERE CATNAME = '" & roCategory!ACCATNAME.ToString & "'"
                                    accode = objGPack.GetSqlValue(StrSql, , , tran)
                                Else
                                    StrSql = "SELECT P_IGSTID FROM " & cnAdminDb & "..CATEGORY "
                                    StrSql += " WHERE CATNAME = '" & roCategory!ACCATNAME.ToString & "'"
                                    accode = objGPack.GetSqlValue(StrSql, , , tran)
                                End If
                                If accode.ToString = "" Then
                                    StrSql = "SELECT P_IGSTID FROM " & cnAdminDb & "..CATEGORY "
                                    StrSql += " WHERE CATNAME = '" & roCategory!ACCATNAME.ToString & "'"
                                    accode = objGPack.GetSqlValue(StrSql, , , tran)
                                End If
                            End If


                            If SrvTaxIGCode <> "" Then
                                accode = SrvTaxIGCode
                            End If
                            InsertIntoAccTran(tNo, tranMode, accode, IGSTAmt, 0, 0, 0, payMode, _Accode, txtBillNo.Text, dtpBillDate_OWN.Value.ToString("yyyy-MM-dd"))
                        End If
                    End If
                End If
                ''INSERTING PURCHASE
                If vatAmt <> 0 Or (SGSTAmt <> 0 Or CGSTAmt <> 0 Or IGSTAmt <> 0) Then  ''''vatAmt <> 0 already this condition only given till 29-June-2021
                    vatAmt = Math.Round(vatAmt, 2)
                    If cmbTransactionType.Text = "PURCHASE" Or cmbTransactionType.Text = "INTERNAL TRANSFER" Or cmbTransactionType.Text = "PURCHASE[APPROVAL]" Then
                        If GstFlag Then
                            tranMode = "D"
                            payMode = "TR"
                            If SGSTAmt > 0 Then
                                StrSql = "SELECT P_SGSTID FROM " & cnAdminDb & "..CATEGORY "
                                StrSql += " WHERE CATNAME = '" & roCategory!ACCATNAME.ToString & "'"
                                accode = objGPack.GetSqlValue(StrSql, , , tran)
                                InsertIntoAccTran(tNo, tranMode, accode, SGSTAmt, 0, 0, 0, payMode, _Accode, txtBillNo.Text, dtpBillDate_OWN.Value.ToString("yyyy-MM-dd"))
                            End If
                            If CGSTAmt > 0 Then
                                StrSql = "SELECT P_CGSTID FROM " & cnAdminDb & "..CATEGORY "
                                StrSql += " WHERE CATNAME = '" & roCategory!ACCATNAME.ToString & "'"
                                accode = objGPack.GetSqlValue(StrSql, , , tran)
                                InsertIntoAccTran(tNo, tranMode, accode, CGSTAmt, 0, 0, 0, payMode, _Accode, txtBillNo.Text, dtpBillDate_OWN.Value.ToString("yyyy-MM-dd"))
                            End If
                            If IGSTAmt > 0 Then
                                StrSql = "SELECT P_IGSTID FROM " & cnAdminDb & "..CATEGORY "
                                StrSql += " WHERE CATNAME = '" & roCategory!ACCATNAME.ToString & "'"
                                accode = objGPack.GetSqlValue(StrSql, , , tran)
                                InsertIntoAccTran(tNo, tranMode, accode, IGSTAmt, 0, 0, 0, payMode, _Accode, txtBillNo.Text, dtpBillDate_OWN.Value.ToString("yyyy-MM-dd"))
                            End If
                        Else
                            If Not (_Acctype = "O" And CstPurchsep = False) Then
                                tranMode = "D"
                                StrSql = "SELECT PTAXID FROM " & cnAdminDb & "..CATEGORY "
                                StrSql += " WHERE CATNAME = '" & roCategory!ACCATNAME.ToString & "'"
                                accode = objGPack.GetSqlValue(StrSql, , , tran)
                                payMode = "TR"
                                InsertIntoAccTran(tNo, tranMode, accode, vatAmt, 0, 0, 0, payMode, _Accode, txtBillNo.Text, dtpBillDate_OWN.Value.ToString("yyyy-MM-dd"))
                            End If
                        End If
                    Else
                        tranMode = "C"
                        If TTdsAccode <> "" Then
                            accode = TTdsAccode
                        Else
                            accode = TdsAc
                        End If
                        payMode = "TR"
                        InsertIntoAccTran(tNo, tranMode, accode, vatAmt, 0, 0, 0, payMode, _Accode, txtBillNo.Text, dtpBillDate_OWN.Value.ToString("yyyy-MM-dd"), , , , Mid(TdsRemark, 1, 50))
                        InsertIntoAccTran(tNo, IIf(tranMode = "D", "C", "D"), _Accode,
                            vatAmt, 0, 0, 0, payMode, accode, txtBillNo.Text, dtpBillDate_OWN.Value.ToString("yyyy-MM-dd"), , , , Mid(TdsRemark, 1, 50))
                    End If
                End If
            Else
                ''ISSUE
                If amt <> 0 Then
                    If cmbTransactionType.Text = "INTERNAL TRANSFER" Then
                        tranMode = "D"
                        payMode = "TI"
                        accode = "STKTRAN"
                        InsertIntoAccTran(tNo, IIf(tranMode = "D", "C", "D"), accode, amt, pcs, grsWt, netWT, payMode, "", txtBillNo.Text, dtpBillDate_OWN.Value.ToString("yyyy-MM-dd"))
                        InsertIntoAccTran(tNo, tranMode,
                       _Accode,
                       (amt + SGSTAmt + CGSTAmt + IGSTAmt), pcs, grsWt, netWT, payMode, accode, txtBillNo.Text, dtpBillDate_OWN.Value.ToString("yyyy-MM-dd"))
                    ElseIf cmbTransactionType.Text = "PURCHASE RETURN" Then
                        tranMode = "C" 'C
                        payMode = "TI"
                        StrSql = " SELECT SPRETURNID"
                        StrSql += " FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & roCategory!ACCATNAME.ToString & "'"
                        accode = objGPack.GetSqlValue(StrSql, , , tran)
                        InsertIntoAccTran(tNo, tranMode, accode, amt, pcs, grsWt, netWT, payMode, "", txtBillNo.Text, dtpBillDate_OWN.Value.ToString("yyyy-MM-dd"))

                        InsertIntoAccTran(tNo, tranMode, "TCS", TCSAmt, 0, 0, 0, payMode, "", txtBillNo.Text, dtpBillDate_OWN.Value.ToString("yyyy-MM-dd"))

                        InsertIntoAccTran(tNo, IIf(tranMode = "D", "C", "D"),
                       _Accode,
                       amt + vatAmt + stnVat + TCSAmt, pcs, grsWt, netWT, payMode, accode, txtBillNo.Text, dtpBillDate_OWN.Value.ToString("yyyy-MM-dd"))
                    Else
                        TdsRemark = "TDS Rs " & Format(vatAmt, "0.00") & " @" & Format(TdsPer, "0.00") & "% for " & Format(amt, "0.00")
                        tranMode = "C"
                        payMode = "TI"
                        '' CHANGE FOR VBJ MC ACCOUNT 
                        accode = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID='LABOUR_ACC'", , , tran)
                        If accode = "" Then
                            StrSql = " SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME= '" & roCategory!ACCATNAME.ToString & "'"
                            accode = objGPack.GetSqlValue(StrSql, , , tran) + "MAKR"
                        End If
                        InsertIntoAccTran(tNo, tranMode, accode, amt, pcs, grsWt, netWT, payMode, "", txtBillNo.Text, dtpBillDate_OWN.Value.ToString("yyyy-MM-dd"))
                        InsertIntoAccTran(tNo, IIf(tranMode = "D", "C", "D"),
                       _Accode,
                       (amt + SGSTAmt + CGSTAmt + IGSTAmt), pcs, grsWt, netWT, payMode, accode, txtBillNo.Text, dtpBillDate_OWN.Value.ToString("yyyy-MM-dd"), , , , , , , IIf(vatAmt <> 0, IIf(TTdscatid <> 0, TTdscatid, TdsCatId), 0), IIf(vatAmt <> 0, TdsPer, 0), IIf(vatAmt <> 0, vatAmt, 0))
                        If vatAmt <> 0 Then
                            If GstFlag Then
                                If InterStateBill Then
                                    StrSql = " INSERT INTO " & cnStockDb & "..TAXTRAN"
                                    StrSql += " ("
                                    StrSql += " SNO,ACCODE,CONTRA,TRANNO,TRANDATE,TRANTYPE,BATCHNO,TAXID"
                                    StrSql += " ,AMOUNT,TAXPER,TAXAMOUNT,TAXTYPE,TSNO,COSTID,COMPANYID"
                                    StrSql += " )"
                                    StrSql += " VALUES("
                                    StrSql += " '" & GetNewSno(TranSnoType.TAXTRANCODE, tran) & "'" ''SNO
                                    StrSql += " ,'" & accode & "'"
                                    StrSql += " ,'" & TdsContra & "'"
                                    StrSql += " ," & TranNo & "" 'TRANNO
                                    StrSql += " ,'" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                                    StrSql += " ,'" & payMode & "'"
                                    StrSql += " ,'" & BatchNo & "'" 'BATCHNO
                                    StrSql += " ,'" & IIf(vatAmt - temptdsamt <> 0, IIf(TTdscatid <> 0, TTdscatid, TdsCatId), 0) & "'" 'TAXID
                                    StrSql += " ," & amt & "" 'AMOUNT
                                    StrSql += " ," & TdsPer & "" 'TAXPER
                                    StrSql += " ," & vatAmt
                                    StrSql += " ,'IG'"
                                    StrSql += " ,3" 'TSNO
                                    StrSql += " ,'" & CostCenterId & "'"
                                    StrSql += " ,'" & strCompanyId & "'"
                                    StrSql += " )"
                                    ExecQuery(SyncMode.Transaction, StrSql, cn, tran, CostCenterId)
                                Else
                                    StrSql = " INSERT INTO " & cnStockDb & "..TAXTRAN"
                                    StrSql += " ("
                                    StrSql += " SNO,ACCODE,CONTRA,TRANNO,TRANDATE,TRANTYPE,BATCHNO,TAXID"
                                    StrSql += " ,AMOUNT,TAXPER,TAXAMOUNT,TAXTYPE,TSNO,COSTID,COMPANYID"
                                    StrSql += " )"
                                    StrSql += " VALUES("
                                    StrSql += " '" & GetNewSno(TranSnoType.TAXTRANCODE, tran) & "'" ''SNO
                                    StrSql += " ,'" & accode & "'"
                                    StrSql += " ,'" & TdsContra & "'"
                                    StrSql += " ," & TranNo & "" 'TRANNO
                                    StrSql += " ,'" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                                    StrSql += " ,'" & payMode & "'"
                                    StrSql += " ,'" & BatchNo & "'" 'BATCHNO
                                    StrSql += " ,'" & IIf(vatAmt - temptdsamt <> 0, IIf(TTdscatid <> 0, TTdscatid, TdsCatId), 0) & "'" 'TAXID
                                    StrSql += " ," & amt & "" 'AMOUNT
                                    StrSql += " ," & Math.Round((TdsPer / 2), 2) & "" 'TAXPER
                                    StrSql += " ," & Math.Round((vatAmt / 2), 2)
                                    StrSql += " ,'SG'"
                                    StrSql += " ,1" 'TSNO
                                    StrSql += " ,'" & CostCenterId & "'"
                                    StrSql += " ,'" & strCompanyId & "'"
                                    StrSql += " )"
                                    ExecQuery(SyncMode.Transaction, StrSql, cn, tran, CostCenterId)
                                    StrSql = " INSERT INTO " & cnStockDb & "..TAXTRAN"
                                    StrSql += " ("
                                    StrSql += " SNO,ACCODE,CONTRA,TRANNO,TRANDATE,TRANTYPE,BATCHNO,TAXID"
                                    StrSql += " ,AMOUNT,TAXPER,TAXAMOUNT,TAXTYPE,TSNO,COSTID,COMPANYID"
                                    StrSql += " )"
                                    StrSql += " VALUES("
                                    StrSql += " '" & GetNewSno(TranSnoType.TAXTRANCODE, tran) & "'" ''SNO
                                    StrSql += " ,'" & accode & "'"
                                    StrSql += " ,'" & TdsContra & "'"
                                    StrSql += " ," & TranNo & "" 'TRANNO
                                    StrSql += " ,'" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                                    StrSql += " ,'" & payMode & "'"
                                    StrSql += " ,'" & BatchNo & "'" 'BATCHNO
                                    StrSql += " ,'" & IIf(vatAmt - temptdsamt <> 0, IIf(TTdscatid <> 0, TTdscatid, TdsCatId), 0) & "'" 'TAXID
                                    StrSql += " ," & amt & "" 'AMOUNT
                                    StrSql += " ," & Math.Round((TdsPer / 2), 2) & "" 'TAXPER
                                    StrSql += " ," & Math.Round((vatAmt / 2), 2)
                                    StrSql += " ,'CG'"
                                    StrSql += " ,2" 'TSNO
                                    StrSql += " ,'" & CostCenterId & "'"
                                    StrSql += " ,'" & strCompanyId & "'"
                                    StrSql += " )"
                                    ExecQuery(SyncMode.Transaction, StrSql, cn, tran, CostCenterId)
                                End If
                            Else
                                StrSql = " INSERT INTO " & cnStockDb & "..TAXTRAN"
                                StrSql += " ("
                                StrSql += " SNO,ACCODE,CONTRA,TRANNO,TRANDATE,TRANTYPE,BATCHNO,TAXID"
                                StrSql += " ,AMOUNT,TAXPER,TAXAMOUNT,TAXTYPE,TSNO,COSTID,COMPANYID"
                                StrSql += " )"
                                StrSql += " VALUES("
                                StrSql += " '" & GetNewSno(TranSnoType.TAXTRANCODE, tran) & "'" ''SNO
                                StrSql += " ,'" & accode & "'"
                                StrSql += " ,'" & TdsContra & "'"
                                StrSql += " ," & TranNo & "" 'TRANNO
                                StrSql += " ,'" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                                StrSql += " ,'" & payMode & "'"
                                StrSql += " ,'" & BatchNo & "'" 'BATCHNO
                                StrSql += " ,'" & IIf(vatAmt - temptdsamt <> 0, IIf(TTdscatid <> 0, TTdscatid, TdsCatId), 0) & "'" 'TAXID
                                StrSql += " ," & amt & "" 'AMOUNT
                                StrSql += " ," & TdsPer & "" 'TAXPER
                                StrSql += " ," & vatAmt
                                StrSql += " ,'TD'"
                                StrSql += " ,1" 'TSNO
                                StrSql += " ,'" & CostCenterId & "'"
                                StrSql += " ,'" & strCompanyId & "'"
                                StrSql += " )"
                                ExecQuery(SyncMode.Transaction, StrSql, cn, tran, CostCenterId)
                            End If
                        End If
                        If SGSTAmt > 0 Then
                            StrSql = "SELECT S_SGSTID FROM " & cnAdminDb & "..CATEGORY "
                            StrSql += " WHERE CATNAME = '" & roCategory!ACCATNAME.ToString & "'"
                            accode = objGPack.GetSqlValue(StrSql, , , tran)
                            If SrvTaxSGCode <> "" And SrvTaxCGCode <> "" Then
                                accode = SrvTaxSGCode
                            End If
                            InsertIntoAccTran(tNo, tranMode, accode, SGSTAmt, 0, 0, 0, payMode, _Accode, txtBillNo.Text, dtpBillDate_OWN.Value.ToString("yyyy-MM-dd"))
                        End If
                        If CGSTAmt > 0 Then
                            StrSql = "SELECT S_CGSTID FROM " & cnAdminDb & "..CATEGORY "
                            StrSql += " WHERE CATNAME = '" & roCategory!ACCATNAME.ToString & "'"
                            accode = objGPack.GetSqlValue(StrSql, , , tran)
                            If SrvTaxSGCode <> "" And SrvTaxCGCode <> "" Then
                                accode = SrvTaxCGCode
                            End If
                            InsertIntoAccTran(tNo, tranMode, accode, CGSTAmt, 0, 0, 0, payMode, _Accode, txtBillNo.Text, dtpBillDate_OWN.Value.ToString("yyyy-MM-dd"))
                        End If
                        If IGSTAmt > 0 Then
                            StrSql = "SELECT S_IGSTID FROM " & cnAdminDb & "..CATEGORY "
                            StrSql += " WHERE CATNAME = '" & roCategory!ACCATNAME.ToString & "'"
                            accode = objGPack.GetSqlValue(StrSql, , , tran)
                            If SrvTaxIGCode <> "" Then
                                accode = SrvTaxIGCode
                            End If
                            InsertIntoAccTran(tNo, tranMode, accode, IGSTAmt, 0, 0, 0, payMode, _Accode, txtBillNo.Text, dtpBillDate_OWN.Value.ToString("yyyy-MM-dd"))
                        End If
                    End If

                End If
                ''INSERTING SALESTAX
                If vatAmt <> 0 Then
                    vatAmt = Math.Round(vatAmt, 2)
                    If cmbTransactionType.Text.ToUpper = "PURCHASE RETURN" Or cmbTransactionType.Text = "INTERNAL TRANSFER" Then 'TDS
                        tranMode = "C"
                        If GstFlag Then
                            payMode = "TI"
                            If SGSTAmt > 0 Then
                                StrSql = " SELECT COUNT(*)CNT FROM " & cnAdminDb & ".dbo.syscolumns WHERE ID =( "
                                StrSql += " SELECT OBJECT_ID FROM " & cnAdminDb & ".sys.tables WHERE NAME='CATEGORY') AND NAME ='PR_SGSTID'"
                                If Val(objGPack.GetSqlValue(StrSql, "CNT", 0, tran).ToString) > 0 And cmbTransactionType.Text.ToUpper = "PURCHASE RETURN" Then
                                    StrSql = "SELECT CASE WHEN ISNULL(PR_SGSTID,'')<>'' THEN PR_SGSTID ELSE P_SGSTID END SGSTID FROM " & cnAdminDb & "..CATEGORY "
                                    StrSql += " WHERE CATNAME = '" & roCategory!ACCATNAME.ToString & "'"
                                Else
                                    StrSql = "SELECT P_SGSTID FROM " & cnAdminDb & "..CATEGORY "
                                    StrSql += " WHERE CATNAME = '" & roCategory!ACCATNAME.ToString & "'"
                                End If
                                accode = objGPack.GetSqlValue(StrSql, , , tran)
                                InsertIntoAccTran(tNo, tranMode, accode, SGSTAmt, 0, 0, 0, payMode, _Accode, txtBillNo.Text, dtpBillDate_OWN.Value.ToString("yyyy-MM-dd"))
                            End If
                            If CGSTAmt > 0 Then
                                StrSql = " SELECT COUNT(*)CNT FROM " & cnAdminDb & ".dbo.syscolumns WHERE ID =( "
                                StrSql += " SELECT OBJECT_ID FROM " & cnAdminDb & ".sys.tables WHERE NAME='CATEGORY') AND NAME ='PR_CGSTID'"
                                If Val(objGPack.GetSqlValue(StrSql, "CNT", 0, tran).ToString) > 0 And cmbTransactionType.Text.ToUpper = "PURCHASE RETURN" Then
                                    StrSql = "SELECT CASE WHEN ISNULL(PR_CGSTID,'')<>'' THEN PR_CGSTID ELSE P_CGSTID END SGSTID FROM " & cnAdminDb & "..CATEGORY "
                                    StrSql += " WHERE CATNAME = '" & roCategory!ACCATNAME.ToString & "'"
                                Else
                                    StrSql = "SELECT P_CGSTID FROM " & cnAdminDb & "..CATEGORY "
                                    StrSql += " WHERE CATNAME = '" & roCategory!ACCATNAME.ToString & "'"
                                End If
                                accode = objGPack.GetSqlValue(StrSql, , , tran)
                                InsertIntoAccTran(tNo, tranMode, accode, CGSTAmt, 0, 0, 0, payMode, _Accode, txtBillNo.Text, dtpBillDate_OWN.Value.ToString("yyyy-MM-dd"))
                            End If
                            If IGSTAmt > 0 Then
                                StrSql = " SELECT COUNT(*)CNT FROM " & cnAdminDb & ".dbo.syscolumns WHERE ID =( "
                                StrSql += " SELECT OBJECT_ID FROM " & cnAdminDb & ".sys.tables WHERE NAME='CATEGORY') AND NAME ='PR_IGSTID'"
                                If Val(objGPack.GetSqlValue(StrSql, "CNT", 0, tran).ToString) > 0 And cmbTransactionType.Text.ToUpper = "PURCHASE RETURN" Then
                                    StrSql = "SELECT CASE WHEN ISNULL(PR_IGSTID,'')<>'' THEN PR_IGSTID ELSE P_IGSTID END SGSTID FROM " & cnAdminDb & "..CATEGORY "
                                    StrSql += " WHERE CATNAME = '" & roCategory!ACCATNAME.ToString & "'"
                                Else
                                    StrSql = "SELECT P_IGSTID FROM " & cnAdminDb & "..CATEGORY "
                                    StrSql += " WHERE CATNAME = '" & roCategory!ACCATNAME.ToString & "'"
                                End If
                                accode = objGPack.GetSqlValue(StrSql, , , tran)
                                InsertIntoAccTran(tNo, tranMode, accode, IGSTAmt, 0, 0, 0, payMode, _Accode, txtBillNo.Text, dtpBillDate_OWN.Value.ToString("yyyy-MM-dd"))
                            End If
                        Else
                            StrSql = "SELECT PTAXID FROM " & cnAdminDb & "..CATEGORY "
                            StrSql += " WHERE CATNAME = '" & roCategory!ACCATNAME.ToString & "'"
                            accode = objGPack.GetSqlValue(StrSql, , , tran)
                            payMode = "TI"
                            InsertIntoAccTran(tNo, tranMode, accode, vatAmt, 0, 0, 0, payMode, _Accode, txtBillNo.Text, dtpBillDate_OWN.Value.ToString("yyyy-MM-dd"))
                        End If
                    Else
                        tranMode = "D"
                        If TTdsAccode <> "" Then
                            accode = TTdsAccode
                        Else
                            accode = TdsAc
                        End If
                        payMode = "TI"
                        InsertIntoAccTran(tNo, tranMode, accode, vatAmt, 0, 0, 0, payMode, _Accode, txtBillNo.Text, dtpBillDate_OWN.Value.ToString("yyyy-MM-dd"), , , , Mid(TdsRemark, 1, 50))
                        InsertIntoAccTran(tNo, IIf(tranMode = "D", "C", "D"),
                        _Accode,
                        vatAmt, 0, 0, 0, payMode, accode, txtBillNo.Text, dtpBillDate_OWN.Value.ToString("yyyy-MM-dd"), , , , Mid(TdsRemark, 1, 50))
                    End If
                End If
            End If
        Next
    End Sub

    Private Sub InsertIntoOustanding _
(
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
Optional ByVal Remark2 As String = Nothing
    )
        If Amount = 0 And GrsWt = 0 Then Exit Sub
        StrSql = " INSERT INTO " & cnAdminDb & ".." & IIf(_AccAudit, "TOUTSTANDING", "OUTSTANDING")
        StrSql += " ("
        StrSql += " SNO,TRANNO,TRANDATE,TRANTYPE,RUNNO"
        StrSql += " ,AMOUNT,GRSWT,NETWT,RECPAY"
        StrSql += " ,REFNO,REFDATE,EMPID"
        StrSql += " ,TRANSTATUS,PURITY,CATCODE,BATCHNO"
        StrSql += " ,USERID,UPDATED,UPTIME,SYSTEMID"
        StrSql += " ,RATE,VALUE,CASHID,REMARK1,REMARK2,ACCODE,CTRANCODE,DUEDATE,APPVER,COMPANYID,COSTID,PAYMODE,FROMFLAG)"
        StrSql += " VALUES"
        StrSql += " ("
        StrSql += " '" & GetNewSno(IIf(_AccAudit, TranSnoType.TOUTSTANDINGCODE, TranSnoType.OUTSTANDINGCODE), tran, "GET_ADMINSNO_TRAN") & "'" ''SNO
        StrSql += " ," & TranNo & "" 'TRANNO
        StrSql += " ,'" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
        StrSql += " ,'" & tType & "'" 'TRANTYPE
        StrSql += " ,'" & RunNo & "'" 'RUNNO
        StrSql += " ," & Math.Abs(Amount) & "" 'AMOUNT
        StrSql += " ," & Math.Abs(GrsWt) & "" 'GRSWT
        StrSql += " ," & Math.Abs(NetWt) & "" 'NETWT
        StrSql += " ,'" & RecPay & "'" 'RECPAY
        StrSql += " ,'" & refNo & "'" 'REFNO
        If refDate <> Nothing Then
            StrSql += " ,'" & refDate & "'" 'REFDATE
        Else
            StrSql += " ,NULL" 'REFDATE
        End If

        StrSql += " ,0" 'EMPID
        StrSql += " ,''" 'TRANSTATUS
        StrSql += " ," & purity & "" 'PURITY
        StrSql += " ,'" & CatCode & "'" 'CATCODE
        StrSql += " ,'" & BatchNo & "'" 'BATCHNO
        StrSql += " ," & userId & "" 'USERID

        StrSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        StrSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
        StrSql += " ,'" & systemId & "'" 'SYSTEMID
        StrSql += " ," & Rate & "" 'RATE
        StrSql += " ," & Value & "" 'VALUE
        StrSql += " ,'" & _CashCtr & "'" 'CASHID
        StrSql += " ,'" & Remark1 & "'" 'REMARK1
        StrSql += " ,'" & Remark2 & "'" 'REMARK1
        StrSql += " ,'" & _Accode & "'" 'ACCODE
        StrSql += " ," & proId & "" 'CTRANCODE
        If dueDate <> Nothing Then
            StrSql += " ,'" & dueDate & "'" 'DUEDATE
        Else
            StrSql += " ,NULL" 'DUEDATE
        End If
        StrSql += " ,'" & VERSION & "'" 'APPVER
        StrSql += " ,'" & strCompanyId & "'" 'COMPANYID
        StrSql += " ,'" & CostCenterId & "'" 'COSTID
        StrSql += " ,'" & Paymode & "'" 'PAYMODE
        StrSql += " ,'S'" 'FROMFLAG
        StrSql += " )"
        ExecQuery(SyncMode.Transaction, StrSql, cn, tran, CostCenterId)
    End Sub

    Private Sub InsertIntoAccTran _
    (ByVal tNo As Integer,
    ByVal tranMode As String,
    ByVal accode As String,
    ByVal amount As Double,
    ByVal pcs As Integer,
    ByVal grsWT As Double,
    ByVal netWT As Double,
    ByVal payMode As String,
    ByVal contra As String,
    Optional ByVal refNo As String = Nothing, Optional ByVal refDate As String = Nothing,
    Optional ByVal chqCardNo As String = Nothing,
    Optional ByVal chqDate As String = Nothing,
    Optional ByVal chqCardId As Integer = Nothing,
    Optional ByVal chqCardRef As String = Nothing,
    Optional ByVal Remark1 As String = Nothing,
    Optional ByVal Remark2 As String = Nothing,
    Optional ByVal TdsCatId As Integer = Nothing,
    Optional ByVal TdsPer As Decimal = Nothing,
    Optional ByVal TdsAmount As Decimal = Nothing,
    Optional ByVal EDPer As Decimal = 0,
    Optional ByVal EDGrsAmt As Decimal = 0,
    Optional ByVal EDAmt As Decimal = 0,
    Optional ByVal SGST As Decimal = 0,
    Optional ByVal CGST As Decimal = 0,
    Optional ByVal IGST As Decimal = 0,
     Optional ByVal _TCSAmt As Decimal = 0
    )
        If amount = 0 Then Exit Sub
        If Remark1 = Nothing Then Remark1 = txtRemark1.Text
        If Remark2 = Nothing Then Remark2 = txtRemark2.Text
        If refNo = Nothing Then refNo = txtBillNo.Text
        If refDate = Nothing Then refDate = dtpBillDate_OWN.Value.ToString("yyyy-MM-dd")

        Dim Sno As String = GetNewSno(IIf(_AccAudit, TranSnoType.TACCTRANCODE, TranSnoType.ACCTRANCODE), tran)
        StrSql = " INSERT INTO " & cnStockDb & ".." & IIf(_AccAudit, "TACCTRAN", "ACCTRAN") & ""
        StrSql += " ("
        StrSql += " SNO,TRANNO,TRANDATE,TRANMODE,ACCODE"
        StrSql += " ,AMOUNT,PCS,GRSWT,NETWT"
        StrSql += " ,REFNO,REFDATE,PAYMODE,CHQCARDNO"
        StrSql += " ,CARDID,CHQCARDREF,CHQDATE,BRSFLAG"
        StrSql += " ,RELIASEDATE,FROMFLAG,REMARK1,REMARK2"
        StrSql += " ,CONTRA,BATCHNO,USERID,UPDATED"
        StrSql += " ,UPTIME,SYSTEMID,CASHID,COSTID,APPVER,COMPANYID"
        StrSql += " ,TDSCATID,TDSPER,TDSAMOUNT"
        StrSql += " )"
        StrSql += " VALUES"
        StrSql += " ("
        StrSql += " '" & Sno & "'" ''SNO
        StrSql += " ," & tNo & "" 'TRANNO 
        StrSql += " ,'" & dtpTrandate.Value.Date.ToString("yyyy-MM-dd") & "'" 'TRANDATE
        StrSql += " ,'" & tranMode & "'" 'TRANMODE
        StrSql += " ,'" & accode & "'" 'ACCODE
        StrSql += " ," & Math.Abs(amount) & "" 'AMOUNT
        StrSql += " ," & Math.Abs(pcs) & "" 'PCS
        StrSql += " ," & Math.Abs(grsWT) & "" 'GRSWT
        StrSql += " ," & Math.Abs(netWT) & "" 'NETWT
        StrSql += " ,'" & refNo & "'" 'REFNO
        If refDate = Nothing Then
            StrSql += " ,NULL" 'REFDATE
        Else
            StrSql += " ,'" & refDate & "'" 'REFDATE
        End If
        StrSql += " ,'" & payMode & "'" 'PAYMODE
        StrSql += " ,'" & chqCardNo & "'" 'CHQCARDNO
        StrSql += " ," & chqCardId & "" 'CARDID
        StrSql += " ,'" & chqCardRef & "'" 'CHQCARDREF
        If chqDate = Nothing Then
            StrSql += " ,NULL" 'CHQDATE
        Else
            StrSql += " ,'" & chqDate & "'" 'CHQDATE
        End If
        StrSql += " ,''" 'BRSFLAG
        StrSql += " ,NULL" 'RELIASEDATE
        StrSql += " ,'S'" 'FROMFLAG
        StrSql += " ,'" & Remark1 & "'" 'REMARK1
        StrSql += " ,'" & Remark2 & "'" 'REMARK2
        StrSql += " ,'" & contra & "'" 'CONTRA
        StrSql += " ,'" & BatchNo & "'" 'BATCHNO
        StrSql += " ,'" & userId & "'" 'USERID
        StrSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        StrSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
        StrSql += " ,'" & systemId & "'" 'SYSTEMID
        StrSql += " ,'" & _CashCtr.ToString & "'" 'CASHID
        StrSql += " ,'" & CostCenterId & "'" 'COSTID
        StrSql += " ,'" & VERSION & "'" 'APPVER
        StrSql += " ,'" & strCompanyId & "'" 'COMPANYID
        StrSql += " ," & TdsCatId & "" 'TDSCATID
        StrSql += " ," & TdsPer & "" 'TDSPER
        StrSql += " ," & TdsAmount & "" 'TDSAMOUNT
        StrSql += " )"
        ExecQuery(SyncMode.Transaction, StrSql, cn, tran, CostCenterId)
        StrSql = ""
        Cmd = Nothing
        If EDAmt <> 0 Then
            StrSql = " INSERT INTO " & cnStockDb & "..TAXTRAN"
            StrSql += " ("
            StrSql += " SNO,ISSSNO,ACCODE,TRANNO,TRANDATE,TRANTYPE,BATCHNO,TAXID"
            StrSql += " ,AMOUNT,TAXPER,TAXAMOUNT,TAXTYPE,TSNO,COSTID,COMPANYID"
            StrSql += " )"
            StrSql += " VALUES("
            StrSql += " '" & GetNewSno(TranSnoType.TAXTRANCODE, tran) & "'" ''SNO
            StrSql += " ,'" & Sno & "'" ''ISSSNO
            StrSql += " ,'" & contra & "'"
            StrSql += " ," & TranNo & "" 'TRANNO
            StrSql += " ,'" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
            StrSql += " ,'TR'"
            StrSql += " ,'" & BatchNo & "'" 'BATCHNO
            StrSql += " ,'ED'" 'TAXID
            StrSql += " ," & EDGrsAmt & "" 'AMOUNT
            StrSql += " ," & EDPer & "" 'TAXPER
            StrSql += " ," & EDAmt
            StrSql += " ,'ED'"
            StrSql += " ,1" 'TSNO
            StrSql += " ,'" & CostCenterId & "'"
            StrSql += " ,'" & strCompanyId & "'"
            StrSql += " )"
            ExecQuery(SyncMode.Transaction, StrSql, cn, tran, CostCenterId)
        End If
        If _TCSAmt <> 0 Then
            StrSql = " INSERT INTO " & cnStockDb & "..TAXTRAN"
            StrSql += " ("
            StrSql += " SNO,ISSSNO,ACCODE,TRANNO,TRANDATE,TRANTYPE,BATCHNO,TAXID"
            StrSql += " ,AMOUNT,TAXPER,TAXAMOUNT,TAXTYPE,TSNO,COSTID,COMPANYID"
            StrSql += " )"
            StrSql += " VALUES("
            StrSql += " '" & GetNewSno(TranSnoType.TAXTRANCODE, tran) & "'" ''SNO
            StrSql += " ,'" & Sno & "'" ''ISSSNO
            StrSql += " ,'" & contra & "'"
            StrSql += " ," & TranNo & "" 'TRANNO
            StrSql += " ,'" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
            StrSql += " ,'TR'"
            StrSql += " ,'" & BatchNo & "'" 'BATCHNO
            StrSql += " ,'TC'" 'TAXID
            StrSql += " ," & EDGrsAmt & "" 'AMOUNT
            StrSql += " ," & EDPer & "" 'TAXPER
            StrSql += " ," & _TCSAmt
            StrSql += " ,'ED'"
            StrSql += " ,1" 'TSNO
            StrSql += " ,'" & CostCenterId & "'"
            StrSql += " ,'" & strCompanyId & "'"
            StrSql += " )"
            ExecQuery(SyncMode.Transaction, StrSql, cn, tran, CostCenterId)
        End If
    End Sub

    Private Sub InsertOrderAdditionalDetails(ByVal tNo As Integer,
          ByVal Orsno As String,
          ByVal Trantype As String,
          ByVal Costid As String,
          ByVal Companyid As String,
          ByVal Batchno As String,
          ByVal typeid As Integer,
          ByVal Valuename As String,
          ByVal tran As OleDbTransaction)

        StrSql = " INSERT INTO " & cnAdminDb & "..ORADTRAN"
        StrSql += " ("
        StrSql += " SNO,TYPEID,VALUENAME,ORSNO,TRANNO,ORNO,TRANDATE"
        StrSql += " ,TRANTYPE,COSTID,COMPANYID,BATCHNO,USERID,UPDATED,UPTIME"
        StrSql += " )"
        StrSql += " VALUES"
        StrSql += " ("
        StrSql += " '" & GetNewSno(TranSnoType.ORADTRAN, tran, "GET_ADMINSNO_TRAN") & "'" ''SNO
        StrSql += " ,'" & typeid & "'" 'typeid
        StrSql += " ,'" & Valuename & "'" 'typeid
        StrSql += " ,'" & Orsno & "'" 'TRANNO 
        StrSql += " ," & tNo & "" 'TRANNO 
        StrSql += " ,''"
        StrSql += " ,'" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
        StrSql += " ,'" & Trantype & "'" 'Trantype
        StrSql += " ,'" & CostCenterId & "'" 'COSTID
        StrSql += " ,'" & strCompanyId & "'" 'COMPANYI
        StrSql += " ,'" & Batchno & "'" 'BATCHNO
        StrSql += " ,'" & userId & "'" 'USERID
        StrSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        StrSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIMe                    
        StrSql += " )"
        ExecQuery(SyncMode.Transaction, StrSql, cn, tran, CostCenterId)
    End Sub

    Public Sub InsertIssRecDetails(ByVal index As Integer)
        'If DgvTran.Rows(index).Cells("JOBNO").Value.ToString = "" Then
        Save_WithOutJobNo(index)
        'Else 
        'Save_JobNo(index)
        'End If
    End Sub
    Private Sub Save_WithOutJobNo(ByVal index As Integer)
        With DgvTran.Rows(index)
            If _Tdsaccode = "" Then
                _Tdsaccode = .Cells("TDSACCODE").Value.ToString
            End If
            Dim Obj As MaterialIssRec
            Obj = CType(.Cells("METISSREC").Value, MaterialIssRec)
            Dim OrdStateId As Integer = 0
            Dim OrdLotid As Integer = 0
            Dim Tax As Decimal = Val(.Cells("IGST").Value.ToString) + Val(.Cells("CGST").Value.ToString) + Val(.Cells("SGST").Value.ToString)
            If Tax = 0 And GstFlag = False Then
                Tax = Val(.Cells("VAT").Value.ToString)
            End If
            Dim Tds As Decimal = Val(.Cells("VAT").Value.ToString)
            Dim Type As String = .Cells("TYPE").Value.ToString ' wheather it is ornament,metal,stone,others
            Dim _StkType As String = ""
            If .Cells("RESNO").Value.ToString <> "" And OMaterialType = MaterialType.Issue Then
                StrSql = " SELECT TOP 1 STKTYPE FROM " & cnStockDb & "..RECEIPT WHERE SNO='" & .Cells("RESNO").Value.ToString & "' "
                _StkType = objGPack.GetSqlValue(StrSql, , "", tran)
            Else
                If IIf(OMaterialType = MaterialType.Issue, "I", "R") & "" & Mid(.Cells("TRANTYPE").Value.ToString, 1, 2) = "RRE" Then
                    _StkType = "M"
                ElseIf IIf(OMaterialType = MaterialType.Issue, "I", "R") & "" & Mid(.Cells("TRANTYPE").Value.ToString, 1, 2) = "RPU" Then
                    _StkType = "T"
                End If
            End If
            If Obj.cmbOStkType.Text <> "" Then
                _StkType = Mid(Obj.cmbOStkType.Text, 1, 1)
            End If
            Select Case Type
                Case "O" 'Ornament
                    OrdStateId = Val(objGPack.GetSqlValue("SELECT ORDSTATE_ID FROM " & cnAdminDb & "..ORDERSTATUS WHERE ORDSTATE_NAME = '" & Obj.cmbOProcess.Text & "'", , , tran))
                    OrdLotid = Val(Obj.txtOOrdNo.Text)
                    If GstFlag = False Then
                        If Obj.lblOVat.Text = "Tds" Then Tax = 0 Else Tds = 0
                    Else
                        If Obj.lblOVat.Text <> "Tds" Then Tds = 0
                    End If
                    If TdsPer = Nothing Then TdsPer = Val(Obj.txtOVatPer_PER.Text)
                Case "M" 'Metal
                    OrdStateId = Val(objGPack.GetSqlValue("SELECT ORDSTATE_ID FROM " & cnAdminDb & "..ORDERSTATUS WHERE ORDSTATE_NAME = '" & Obj.cmbMProcess.Text & "'", , , tran))
                    OrdLotid = Val(Obj.txtOOrdNo.Text)
                    If GstFlag = False Then
                        If Obj.lblMVat.Text = "Tds" Then Tax = 0 Else Tds = 0
                    Else
                        If Obj.lblMVat.Text <> "Tds" Then Tds = 0
                    End If
                    If TdsPer = Nothing Then TdsPer = Val(Obj.txtMVatPer_PER.Text)
                Case "T" 'Stone
                    OrdStateId = Val(objGPack.GetSqlValue("SELECT ORDSTATE_ID FROM " & cnAdminDb & "..ORDERSTATUS WHERE ORDSTATE_NAME = '" & Obj.cmbSProcess.Text & "'", , , tran))
                    OrdLotid = Val(Obj.txtOOrdNo.Text)
                    If GstFlag = False Then
                        If Obj.lblSVat.Text = "Tds" Then Tax = 0 Else Tds = 0
                    Else
                        If Obj.lblSVat.Text <> "Tds" Then Tds = 0
                    End If
                    If TdsPer = Nothing Then TdsPer = Val(Obj.txtSVatPer_PER.Text)
                Case "H" 'Others
                    If GstFlag = False Then
                        If Obj.lblOthVat.Text = "Tds" Then Tax = 0 Else Tds = 0
                    Else
                        If Obj.lblOthVat.Text <> "Tds" Then Tds = 0
                    End If
                    If TdsPer = Nothing Then TdsPer = Val(Obj.txtOthVatPer_PER.Text)
            End Select
            Dim issSno As String = Nothing
            Dim issAppNo As String
            If OMaterialType = MaterialType.Issue Then
                issSno = GetNewSno(IIf(_AccAudit, TranSnoType.TISSUECODE, TranSnoType.ISSUECODE), tran)
            Else
                issSno = GetNewSno(IIf(_AccAudit, TranSnoType.TRECEIPTCODE, TranSnoType.RECEIPTCODE), tran)
            End If
            Dim catCode As String = objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & .Cells("CATNAME").Value.ToString & "'", , , tran)
            Dim OCatcode As String
            If OMaterialType = MaterialType.Issue Then
                OCatcode = catCode
            Else
                OCatcode = objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & .Cells("ISSCATNAME").Value.ToString & "'", , , tran)
            End If
            Dim wast As Decimal = Nothing
            Dim wastPer As Decimal = Nothing
            Dim alloy As Decimal = Nothing
            alloy = Val(.Cells("ALLOY").Value.ToString)
            wast = Val(.Cells("WASTAGE").Value.ToString)
            wastPer = Val(.Cells("WASTPER").Value.ToString)

            'If OMaterialType = MaterialType.Issue Or (Type <> "M" And OMaterialType = MaterialType.Receipt) Then
            '    If objGPack.GetSqlValue("SELECT CATGROUP FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = '" & catCode & "'", , , tran) = "B" Then
            '        alloy = Val(.Cells("WASTAGE").Value.ToString)
            '    Else
            '        wast = Val(.Cells("WASTAGE").Value.ToString)
            '        wastPer = Val(.Cells("WASTPER").Value.ToString)
            '    End If
            'Else
            '    If objGPack.GetSqlValue("SELECT CATGROUP FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = '" & OCatcode & "'", , , tran) = "B" Then
            '        alloy = Val(.Cells("WASTAGE").Value.ToString)
            '    Else
            '        wast = Val(.Cells("WASTAGE").Value.ToString)
            '        wastPer = Val(.Cells("WASTPER").Value.ToString)
            '    End If
            'End If

            Dim itemTypeId As Integer = 0
            Dim Itemid As Integer = Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Cells("ITEM").Value.ToString & "'", , , tran))
            Dim subItemid As Integer = Val(objGPack.GetSqlValue("SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & .Cells("SUBITEM").Value.ToString & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Cells("ITEM").Value.ToString & "')", , , tran))
            If OCatcode Is Nothing Then OCatcode = catCode
            If OCatcode.ToString = "" Then OCatcode = catCode

            Dim Jobisno As String = ""
            If OMaterialType = MaterialType.Receipt And .Cells("JOBNO").Value.ToString <> "" And Mid(.Cells("JOBNO").Value.ToString, 1, 1) = "R" Then
                Dim excessWt As Decimal = 0
                Dim excessPureWt As Decimal = 0
                Dim excessWtCcode As String = objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Cells("ITEM").Value.ToString & "'", , , tran)
                excessWt = Val(.Cells("GRSWT").Value.ToString) - Val(.Cells("OGRSWT").Value.ToString)
                excessPureWt = Math.Round((excessWt / 100) * Val(.Cells("PURITY").Value.ToString), 2)

                StrSql = " INSERT INTO " & cnStockDb & ".." & IIf(_AccAudit, "TRECEIPT", "RECEIPT")
                StrSql += " ("
                StrSql += "  SNO,TRANNO,TRANDATE,TRANTYPE,STKTYPE,PCS,GRSWT,NETWT,LESSWT,PUREWT"
                StrSql += " ,TAGNO,ITEMID,SUBITEMID,WASTPER,WASTAGE,MCGRM,MCHARGE,AMOUNT"
                StrSql += " ,RATE,BOARDRATE,SALEMODE,GRSNET,TRANSTATUS,REFNO,REFDATE,COSTID"
                StrSql += " ,COMPANYID,FLAG,EMPID,TAGGRSWT,TAGNETWT,TAGRATEID,TAGSVALUE,TAGDESIGNER"
                StrSql += " ,ITEMCTRID,ITEMTYPEID,PURITY,TABLECODE,INCENTIVE,WEIGHTUNIT,CATCODE,OCATCODE"
                StrSql += " ,ACCODE,ALLOY,BATCHNO,REMARK1"
                StrSql += " ,REMARK2,USERID,UPDATED,UPTIME,SYSTEMID,DISCOUNT,RUNNO,CASHID,TAX,TDS"
                StrSql += " ,STNAMT,MISCAMT,METALID,STONEUNIT,APPVER,TOUCH,ORDSTATE_ID"
                If _JobNoEnable = True Then
                    StrSql += " ,JOBNO"
                ElseIf OrdLotid <> 0 Then
                    StrSql += " ,JOBNO"
                End If
                StrSql += " ,SEIVE,BAGNO"
                StrSql += " ,STNGRPID,APRXAMT,APRXTAX,RATEFIXED)"
                StrSql += " VALUES("
                StrSql += " '" & issSno & "'" ''SNO
                StrSql += " ," & TranNo & "" 'TRANNO
                StrSql += " ,'" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                ''StrSql += " ,'" & IIf(OMaterialType = MaterialType.Issue, "I", "R") & "" & Mid(.Cells("TRANTYPE").Value.ToString, 1, 2) & "'" 'TRANTYPE [07-AUG-2021]
                StrSql += " ,'" & IIf(OMaterialType = MaterialType.Issue, "I", "R") & "" _
                        & IIf(.Cells("TRANTYPE").Value.ToString = "DELIVERY NOTE" Or .Cells("TRANTYPE").Value.ToString = "RECEIPT NOTE", "DN" _
                        , Mid(.Cells("TRANTYPE").Value.ToString, 1, 2)) & "'" 'TRANTYPE 
                StrSql += " ,'" & _StkType & "'" 'STKTYPE
                StrSql += " ," & Val(.Cells("PCS").Value.ToString) & "" 'PCS
                StrSql += " ," & Val(.Cells("GRSWT").Value.ToString) - (excessWt) & "" 'GRSWT
                StrSql += " ," & Val(.Cells("NETWT").Value.ToString) - (excessWt) & "" 'NETWT
                StrSql += " ," & Val(.Cells("LESSWT").Value.ToString) & "" 'LESSWT
                StrSql += " ," & Val(.Cells("PUREWT").Value.ToString) - (excessPureWt) & "" 'PUREWT
                StrSql += " ,''" 'TAGNO
                StrSql += " ," & Itemid & "" 'ITEMID
                StrSql += " ," & subItemid & "" 'SUBITEMID
                StrSql += " ," & wastPer & "" 'WASTPER
                StrSql += " ," & wast & "" 'WASTAGE
                StrSql += " ," & Val(.Cells("MCGRM").Value.ToString) & "" 'MCGRM
                StrSql += " ," & Val(.Cells("MC").Value.ToString) & "" 'MCHARGE
                StrSql += " ," & Val(.Cells("GROSSAMT").Value.ToString) & "" 'AMOUNT
                StrSql += " ," & Val(.Cells("RATE").Value.ToString) & "" 'RATE
                StrSql += " ," & Val(.Cells("BOARDRATE").Value.ToString) & "" 'BOARDRATE
                StrSql += " ,''" 'SALEMODE
                StrSql += " ,'" & Mid(.Cells("GRSNET").Value.ToString, 1, 1) & "'" 'GRSNET
                StrSql += " ,''" 'TRANSTATUS ''
                StrSql += " ,'" & txtBillNo.Text & "'" 'REFNO ''
                StrSql += " ,'" & dtpBillDate_OWN.Value.ToString("yyyy-MM-dd") & "'" 'REFDATE NULL
                StrSql += " ,'" & CostCenterId & "'" 'COSTID 
                StrSql += " ,'" & strCompanyId & "'" 'COMPANYID
                StrSql += " ,'" & .Cells("TYPE").Value.ToString & "'" 'FLAG
                StrSql += " ,0" 'EMPID
                StrSql += " ,0" 'TAGGRSWT
                StrSql += " ,0" 'TAGNETWT
                StrSql += " ,0" 'TAGRATEID
                StrSql += " ,0" 'TAGSVALUE
                StrSql += " ,''" 'TAGDESIGNER  
                StrSql += " ,0" 'ITEMCTRID
                StrSql += " ," & itemTypeId & "" 'ITEMTYPEID
                StrSql += " ," & Val(.Cells("PURITY").Value.ToString) & "" 'PURITY
                StrSql += " ,''" 'TABLECODE
                StrSql += " ,''" 'INCENTIVE
                StrSql += " ,'" & Mid(.Cells("CALCMODE").Value.ToString, 1, 1) & "'" 'WEIGHTUNIT
                StrSql += " ,'" & catCode & "'" 'CATCODE
                StrSql += " ,'" & OCatcode & "'" 'OCATCODE
                StrSql += " ,'" & _Accode & "'" 'ACCODE
                StrSql += " ," & alloy & "" 'ALLOY
                StrSql += " ,'" & BatchNo & "'" 'BATCHNO
                ''StrSql += " ,'" & .Cells("REMARK1").Value.ToString & "'" 'REMARK1
                ''StrSql += " ,'" & .Cells("REMARK2").Value.ToString & "'" 'REMARK2
                If SPECIFICFORMAT = "1" And .Cells("REMARK1").Value.ToString = "" Then
                    StrSql += " ,'" & txtRemark1.Text & "'" 'REMARK1
                Else
                    StrSql += " ,'" & .Cells("REMARK1").Value.ToString & "'" 'REMARK1
                End If
                If SPECIFICFORMAT = "1" And .Cells("REMARK2").Value.ToString = "" Then
                    StrSql += " ,'" & txtRemark2.Text & "'" 'REMARK2
                Else
                    StrSql += " ,'" & .Cells("REMARK2").Value.ToString & "'" 'REMARK2
                End If
                StrSql += " ,'" & userId & "'" 'USERID
                StrSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                StrSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                StrSql += " ,'" & systemId & "'" 'SYSTEMID
                StrSql += " ,'" & Val(.Cells("DISCOUNT").Value.ToString) & "'" 'DISCOUNT
                StrSql += " ,''" 'RUNNO
                StrSql += " ,'" & _CashCtr & "'" 'CASHID
                StrSql += " ," & Tax & "" 'TAX
                StrSql += " ," & Tds & "" 'TDS
                StrSql += " ,0" 'STNAMT
                StrSql += " ,0" 'MISCAMT
                StrSql += " ,'" & objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & .Cells("METAL").Value.ToString & "'", , , tran) & "'" 'METALID
                StrSql += " ,'" & Mid(.Cells("UNIT").Value.ToString, 1, 1) & "'" 'STONEUNIT
                StrSql += " ,'" & VERSION & "'" 'APPVER
                StrSql += " ,'" & Val(.Cells("TOUCH").Value.ToString) & "'" 'TOUCH
                StrSql += " ," & OrdStateId & "" 'ORDSTATE_ID
                If _JobNoEnable = True Then
                    StrSql += " ,'" & .Cells("JOBNO").Value.ToString & "'"
                ElseIf OrdLotid <> 0 Then
                    StrSql += " ,'" & OrdLotid & "'" 'ORDlotno
                End If
                StrSql += " ,'" & .Cells("SEIVE").Value.ToString & "'" 'SEIVE
                StrSql += ",'" & IIf(Transistno <> 0, Transistno.ToString, "") & "'"
                StrSql += " ,'" & .Cells("STNGRPID").Value.ToString & "'" 'SEIVE
                StrSql += " ,'" & Val(.Cells("APPROXAMT").Value.ToString) & "'" 'APPROXAMT
                StrSql += " ,'" & Val(.Cells("APPROXTAX").Value.ToString) & "'" 'APPROXTAX
                StrSql += " ,'" & IIf(.Cells("RATEFIXED").Value.ToString = "Y", "Y", "") & "'" 'RATEFIXED
                StrSql += " )"
                ExecQuery(SyncMode.Transaction, StrSql, cn, tran, CostCenterId)

                If excessWt <> 0 Then
                    StrSql = " INSERT INTO " & cnStockDb & ".." & IIf(_AccAudit, "TRECEIPT", "RECEIPT")
                    StrSql += " ("
                    StrSql += "  SNO,TRANNO,TRANDATE,TRANTYPE,STKTYPE,PCS,GRSWT,NETWT,LESSWT,PUREWT"
                    StrSql += " ,TAGNO,ITEMID,SUBITEMID,WASTPER,WASTAGE,MCGRM,MCHARGE,AMOUNT"
                    StrSql += " ,RATE,BOARDRATE,SALEMODE,GRSNET,TRANSTATUS,REFNO,REFDATE,COSTID"
                    StrSql += " ,COMPANYID,FLAG,EMPID,TAGGRSWT,TAGNETWT,TAGRATEID,TAGSVALUE,TAGDESIGNER"
                    StrSql += " ,ITEMCTRID,ITEMTYPEID,PURITY,TABLECODE,INCENTIVE,WEIGHTUNIT,CATCODE,OCATCODE"
                    StrSql += " ,ACCODE,ALLOY,BATCHNO,REMARK1"
                    StrSql += " ,REMARK2,USERID,UPDATED,UPTIME,SYSTEMID,DISCOUNT,RUNNO,CASHID,TAX,TDS"
                    StrSql += " ,STNAMT,MISCAMT,METALID,STONEUNIT,APPVER,TOUCH,ORDSTATE_ID"
                    If _JobNoEnable = True Then
                        StrSql += " ,JOBNO"
                    ElseIf OrdLotid <> 0 Then
                        StrSql += " ,JOBNO"
                    End If
                    StrSql += " ,SEIVE,BAGNO"
                    StrSql += " ,STNGRPID,APRXAMT,APRXTAX,RATEFIXED)"
                    StrSql += " VALUES("
                    If OMaterialType = MaterialType.Issue Then
                        StrSql += "'" & GetNewSno(IIf(_AccAudit, TranSnoType.TISSUECODE, TranSnoType.ISSUECODE), tran) & "'" 'SNO
                    Else
                        StrSql += "'" & GetNewSno(IIf(_AccAudit, TranSnoType.TRECEIPTCODE, TranSnoType.RECEIPTCODE), tran) & "'" 'SNO
                    End If
                    StrSql += " ," & TranNo & "" 'TRANNO
                    StrSql += " ,'" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                    StrSql += " ,'" & IIf(OMaterialType = MaterialType.Issue, "I", "R") & "" & Mid(.Cells("TRANTYPE").Value.ToString, 1, 2) & "'" 'TRANTYPE   [07-AUG-2021]
                    StrSql += " ,'" & IIf(OMaterialType = MaterialType.Issue, "I", "R") & "" _
                        & IIf(.Cells("TRANTYPE").Value.ToString = "DELIVERY NOTE" Or .Cells("TRANTYPE").Value.ToString = "RECEIPT NOTE", "DN" _
                        , Mid(.Cells("TRANTYPE").Value.ToString, 1, 2)) & "'" 'TRANTYPE 
                    StrSql += " ,'" & _StkType & "'" 'STKTYPE
                    StrSql += " ," & 0 & "" 'PCS
                    StrSql += " ," & excessWt & "" 'GRSWT
                    StrSql += " ," & excessWt & "" 'NETWT
                    StrSql += " ," & 0 & "" 'LESSWT
                    StrSql += " ," & excessPureWt & "" 'PUREWT
                    StrSql += " ,''" 'TAGNO
                    StrSql += " ," & 0 & "" 'ITEMID
                    StrSql += " ," & 0 & "" 'SUBITEMID
                    StrSql += " ," & 0 & "" 'WASTPER
                    StrSql += " ," & 0 & "" 'WASTAGE
                    StrSql += " ," & 0 & "" 'MCGRM
                    StrSql += " ," & 0 & "" 'MCHARGE
                    StrSql += " ," & 0 & "" 'AMOUNT
                    StrSql += " ," & Val(.Cells("RATE").Value.ToString) & "" 'RATE
                    StrSql += " ," & Val(.Cells("BOARDRATE").Value.ToString) & "" 'BOARDRATE
                    StrSql += " ,''" 'SALEMODE
                    StrSql += " ,'" & Mid(.Cells("GRSNET").Value.ToString, 1, 1) & "'" 'GRSNET
                    StrSql += " ,''" 'TRANSTATUS ''
                    StrSql += " ,'" & txtBillNo.Text & "'" 'REFNO ''
                    StrSql += " ,'" & dtpBillDate_OWN.Value.ToString("yyyy-MM-dd") & "'" 'REFDATE NULL
                    StrSql += " ,'" & CostCenterId & "'" 'COSTID 
                    StrSql += " ,'" & strCompanyId & "'" 'COMPANYID
                    StrSql += " ,'" & .Cells("TYPE").Value.ToString & "'" 'FLAG
                    StrSql += " ,0" 'EMPID
                    StrSql += " ,0" 'TAGGRSWT
                    StrSql += " ,0" 'TAGNETWT
                    StrSql += " ,0" 'TAGRATEID
                    StrSql += " ,0" 'TAGSVALUE
                    StrSql += " ,''" 'TAGDESIGNER  
                    StrSql += " ,0" 'ITEMCTRID
                    StrSql += " ," & itemTypeId & "" 'ITEMTYPEID
                    StrSql += " ," & Val(.Cells("PURITY").Value.ToString) & "" 'PURITY
                    StrSql += " ,''" 'TABLECODE
                    StrSql += " ,''" 'INCENTIVE
                    StrSql += " ,'" & Mid(.Cells("CALCMODE").Value.ToString, 1, 1) & "'" 'WEIGHTUNIT
                    StrSql += " ,'" & excessWtCcode & "'" 'CATCODE
                    StrSql += " ,'" & excessWtCcode & "'" 'OCATCODE
                    StrSql += " ,'" & _Accode & "'" 'ACCODE
                    StrSql += " ," & alloy & "" 'ALLOY
                    StrSql += " ,'" & BatchNo & "'" 'BATCHNO
                    ''StrSql += " ,'" & .Cells("REMARK1").Value.ToString & "'" 'REMARK1
                    ''StrSql += " ,'" & .Cells("REMARK2").Value.ToString & "'" 'REMARK2
                    If SPECIFICFORMAT = "1" And .Cells("REMARK1").Value.ToString = "" Then
                        StrSql += " ,'" & txtRemark1.Text & "'" 'REMARK1
                    Else
                        StrSql += " ,'" & .Cells("REMARK1").Value.ToString & "'" 'REMARK1
                    End If
                    If SPECIFICFORMAT = "1" And .Cells("REMARK2").Value.ToString = "" Then
                        StrSql += " ,'" & txtRemark2.Text & "'" 'REMARK2
                    Else
                        StrSql += " ,'" & .Cells("REMARK2").Value.ToString & "'" 'REMARK2
                    End If
                    StrSql += " ,'" & userId & "'" 'USERID
                    StrSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                    StrSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                    StrSql += " ,'" & systemId & "'" 'SYSTEMID
                    StrSql += " ,0" 'DISCOUNT
                    StrSql += " ,''" 'RUNNO
                    StrSql += " ,'" & _CashCtr & "'" 'CASHID
                    StrSql += " ," & 0 & "" 'TAX
                    StrSql += " ," & 0 & "" 'TDS
                    StrSql += " ,0" 'STNAMT
                    StrSql += " ,0" 'MISCAMT
                    StrSql += " ,'" & objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & .Cells("METAL").Value.ToString & "'", , , tran) & "'" 'METALID
                    StrSql += " ,'" & Mid(.Cells("UNIT").Value.ToString, 1, 1) & "'" 'STONEUNIT
                    StrSql += " ,'" & VERSION & "'" 'APPVER
                    StrSql += " ,'" & 0 & "'" 'APPVER
                    StrSql += " ," & OrdStateId & "" 'ORDSTATE_ID
                    If _JobNoEnable = True Then
                        StrSql += " ,'" & .Cells("JOBNO").Value.ToString & "'"
                    ElseIf OrdLotid <> 0 Then
                        StrSql += " ," & OrdLotid & "" 'ORDlot_no
                    End If
                    StrSql += " ,'" & .Cells("SEIVE").Value.ToString & "'" 'SEIVE
                    StrSql += ",'" & IIf(Transistno <> 0, Transistno.ToString, "") & "'" 'BAGNO
                    StrSql += " ,'" & .Cells("STNGRPID").Value.ToString & "'" 'STNGRPID
                    StrSql += " ,'" & Val(.Cells("APPROXAMT").Value.ToString) & "'" 'APPROXAMT
                    StrSql += " ,'" & Val(.Cells("APPROXTAX").Value.ToString) & "'" 'APPROXTAX
                    StrSql += " ,'" & IIf(.Cells("RATEFIXED").Value.ToString = "Y", "Y", "") & "'" 'RATEFIXED
                    StrSql += " )"
                    ExecQuery(SyncMode.Transaction, StrSql, cn, tran, CostCenterId)
                End If
            ElseIf MIMR_BAGISSUE And OMaterialType = MaterialType.Issue And .Cells("JOBNO").Value.ToString <> "" And .Cells("JOBNO").Value.ToString.Contains("B") Then
                StrSql = " INSERT INTO " & cnStockDb & ".." & IIf(_AccAudit, "TISSUE", "ISSUE")
                StrSql += " ("
                StrSql += "  SNO,TRANNO,TRANDATE,TRANTYPE,PCS"
                StrSql += " ,GRSWT,NETWT,LESSWT,PUREWT"
                StrSql += " ,TAGNO,ITEMID,SUBITEMID,WASTPER"
                StrSql += " ,WASTAGE,MCGRM,MCHARGE,AMOUNT"
                StrSql += " ,RATE,BOARDRATE,SALEMODE,GRSNET"
                StrSql += " ,TRANSTATUS,REFNO,REFDATE,COSTID"
                StrSql += " ,COMPANYID,FLAG,EMPID,TAGGRSWT"
                StrSql += " ,TAGNETWT,TAGRATEID,TAGSVALUE,TAGDESIGNER"
                StrSql += " ,ITEMCTRID,ITEMTYPEID,PURITY,TABLECODE"
                StrSql += " ,INCENTIVE,WEIGHTUNIT,CATCODE,OCATCODE"
                StrSql += " ,ACCODE,ALLOY,BATCHNO,REMARK1"
                StrSql += " ,REMARK2,USERID,UPDATED,UPTIME,SYSTEMID,DISCOUNT,RUNNO,CASHID,TAX,TDS"
                StrSql += " ,STNAMT,MISCAMT,METALID,STONEUNIT,APPVER,TOUCH,ORDSTATE_ID"
                StrSql += " ,RESNO"
                StrSql += " ,SEIVE,BAGNO,RFID,CUTID,COLORID,CLARITYID,SHAPEID,SETTYPEID,HEIGHT,WIDTH"
                StrSql += " ,STNGRPID,APRXAMT,APRXTAX,RATEFIXED)"
                StrSql += " VALUES("
                StrSql += " '" & issSno & "'" ''SNO
                StrSql += " ," & TranNo & "" 'TRANNO
                StrSql += " ,'" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                ''StrSql += " ,'" & IIf(OMaterialType = MaterialType.Issue, "I", "R") & "" & Mid(.Cells("TRANTYPE").Value.ToString, 1, 2) & "'" 'TRANTYPE [07-AUG-2021]
                StrSql += " ,'" & IIf(OMaterialType = MaterialType.Issue, "I", "R") & "" _
                        & IIf(.Cells("TRANTYPE").Value.ToString = "DELIVERY NOTE" Or .Cells("TRANTYPE").Value.ToString = "RECEIPT NOTE", "DN" _
                        , Mid(.Cells("TRANTYPE").Value.ToString, 1, 2)) & "'" 'TRANTYPE 
                StrSql += " ," & Val(.Cells("PCS").Value.ToString) & "" 'PCS
                StrSql += " ," & Val(.Cells("GRSWT").Value.ToString) & "" 'GRSWT
                StrSql += " ," & Val(.Cells("NETWT").Value.ToString) & "" 'NETWT
                StrSql += " ," & Val(.Cells("LESSWT").Value.ToString) & "" 'LESSWT
                StrSql += " ," & Val(.Cells("PUREWT").Value.ToString) & "" 'PUREWT
                ''StrSql += " ,''" 'TAGNO
                If .Cells("TAGNO").Value.ToString <> "" And (cmbTransactionType.Text = "ISSUE" Or cmbTransactionType.Text = "PURCHASE RETURN") Then
                    StrSql += " ,'" & .Cells("TAGNO").Value.ToString & "'" 'TAGNO
                Else
                    StrSql += " ,''" 'TAGNO
                End If
                StrSql += " ," & Itemid & "" 'ITEMID
                StrSql += " ," & subItemid & "" 'SUBITEMID
                StrSql += " ," & wastPer & "" 'WASTPER
                StrSql += " ," & wast & "" 'WASTAGE
                StrSql += " ," & Val(.Cells("MCGRM").Value.ToString) & "" 'MCGRM
                StrSql += " ," & Val(.Cells("MC").Value.ToString) & "" 'MCHARGE
                StrSql += " ," & Val(.Cells("GROSSAMT").Value.ToString) & "" 'AMOUNT
                StrSql += " ," & Val(.Cells("RATE").Value.ToString) & "" 'RATE
                StrSql += " ," & Val(.Cells("BOARDRATE").Value.ToString) & "" 'BOARDRATE
                StrSql += " ,''" 'SALEMODE
                StrSql += " ,'" & Mid(.Cells("GRSNET").Value.ToString, 1, 1) & "'" 'GRSNET
                StrSql += " ,''" 'TRANSTATUS ''
                StrSql += " ,'" & txtBillNo.Text & "'" 'REFNO ''
                StrSql += " ,'" & dtpBillDate_OWN.Value.ToString("yyyy-MM-dd") & "'" 'REFDATE NULL
                StrSql += " ,'" & CostCenterId & "'" 'COSTID 
                StrSql += " ,'" & strCompanyId & "'" 'COMPANYID
                StrSql += " ,'" & .Cells("TYPE").Value.ToString & "'" 'FLAG
                StrSql += " ,0" 'EMPID
                StrSql += " ,0" 'TAGGRSWT
                StrSql += " ,0" 'TAGNETWT
                StrSql += " ,0" 'TAGRATEID
                StrSql += " ,0" 'TAGSVALUE
                StrSql += " ,''" 'TAGDESIGNER  
                StrSql += " ,0" 'ITEMCTRID
                StrSql += " ," & itemTypeId & "" 'ITEMTYPEID
                StrSql += " ," & Val(.Cells("PURITY").Value.ToString) & "" 'PURITY
                StrSql += " ,''" 'TABLECODE
                StrSql += " ,''" 'INCENTIVE
                StrSql += " ,'" & Mid(.Cells("CALCMODE").Value.ToString, 1, 1) & "'" 'WEIGHTUNIT
                StrSql += " ,'" & catCode & "'" 'CATCODE
                StrSql += " ,'" & OCatcode & "'" 'OCATCODE
                StrSql += " ,'" & _Accode & "'" 'ACCODE
                StrSql += " ," & alloy & "" 'ALLOY
                StrSql += " ,'" & BatchNo & "'" 'BATCHNO

                ''StrSql += " ,'" & .Cells("REMARK1").Value.ToString & "'" 'REMARK1
                ''StrSql += " ,'" & .Cells("REMARK2").Value.ToString & "'" 'REMARK2

                If SPECIFICFORMAT = "1" And .Cells("REMARK1").Value.ToString = "" Then
                    StrSql += " ,'" & txtRemark1.Text & "'" 'REMARK1
                Else
                    StrSql += " ,'" & .Cells("REMARK1").Value.ToString & "'" 'REMARK1
                End If
                If SPECIFICFORMAT = "1" And .Cells("REMARK2").Value.ToString = "" Then
                    StrSql += " ,'" & txtRemark2.Text & "'" 'REMARK2
                Else
                    StrSql += " ,'" & .Cells("REMARK2").Value.ToString & "'" 'REMARK2
                End If

                StrSql += " ,'" & userId & "'" 'USERID
                StrSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                StrSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                StrSql += " ,'" & systemId & "'" 'SYSTEMID
                StrSql += " ,'" & Val(.Cells("DISCOUNT").Value.ToString) & "'" 'DISCOUNT
                StrSql += " ,''" 'RUNNO
                StrSql += " ,'" & _CashCtr & "'" 'CASHID
                StrSql += " ," & Tax & "" 'TAX
                StrSql += " ," & Tds & "" 'TDS
                StrSql += " ,0" 'STNAMT
                StrSql += " ,0" 'MISCAMT
                StrSql += " ,'" & objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & .Cells("METAL").Value.ToString & "'", , , tran) & "'" 'METALID
                StrSql += " ,'" & Mid(.Cells("UNIT").Value.ToString, 1, 1) & "'" 'STONEUNIT
                StrSql += " ,'" & VERSION & "'" 'APPVER
                StrSql += " ,'" & Val(.Cells("TOUCH").Value.ToString) & "'" 'APPVER
                StrSql += " ," & OrdStateId & "" 'ORDSTATE_ID
                StrSql += " ,'" & .Cells("RESNO").Value.ToString & "'" 'RESNO
                StrSql += " ,'" & .Cells("SEIVE").Value.ToString & "'" 'SEIVE
                StrSql += ",'" & .Cells("JOBNO").Value.ToString & "'" 'BAGNO
                StrSql += " ,'" & .Cells("RFID").Value.ToString & "'" 'RFID
                StrSql += " ," & Val(.Cells("CUTID").Value.ToString) & " " 'CUTID
                StrSql += " ," & Val(.Cells("COLORID").Value.ToString) & "" 'COLORID
                StrSql += " ," & Val(.Cells("CLARITYID").Value.ToString) & "" 'CLARITYID
                StrSql += " ," & Val(.Cells("SHAPEID").Value.ToString) & "" 'SHAPEID
                StrSql += " ," & Val(.Cells("SETTYPEID").Value.ToString) & "" 'SETTYPEID
                StrSql += " ,'" & Val(.Cells("HEIGHT").Value.ToString) & "'" 'HEIGHT
                StrSql += " ,'" & Val(.Cells("WIDTH").Value.ToString) & "'" 'WIDTH
                StrSql += " ,'" & .Cells("STNGRPID").Value.ToString & "'" 'STNGRPID
                StrSql += " ,'" & Val(.Cells("APPROXAMT").Value.ToString) & "'" 'APPROXAMT
                StrSql += " ,'" & Val(.Cells("APPROXTAX").Value.ToString) & "'" 'APPROXTAX
                StrSql += " ,'" & IIf(.Cells("RATEFIXED").Value.ToString = "Y", "Y", "") & "'" 'RATEFIXED
                StrSql += " )"
                ExecQuery(SyncMode.Transaction, StrSql, cn, tran, CostCenterId)

                StrSql = " INSERT INTO " & cnStockDb & "..MELTINGDETAIL"
                StrSql += " (TRANNO,TRANDATE,RECISS,TRANTYPE,ACCODE,CATCODE,METALID,PCS,GRSWT,NETWT,RATE,AMOUNT"
                StrSql += " ,BAGNO,BATCHNO,APPVER,COMPANYID,USERID,UPDATED"
                StrSql += " ,UPTIME,CANCEL)"
                StrSql += " VALUES"
                StrSql += " ("
                StrSql += " " & TranNo & "" 'TRANNO
                StrSql += " ,'" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "'" 'trandate
                StrSql += " ,'I','MI'" 'RECISS,TRANTYPE
                StrSql += " ,'" & _Accode & "'" 'accode
                StrSql += " ,'" & catCode & "'" 'CATCODE.
                StrSql += " ,'" & objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & .Cells("METAL").Value.ToString & "'", , , tran) & "'" 'METAL.
                StrSql += " ," & Val(.Cells("PCS").Value.ToString) & "" 'PCS    
                StrSql += " ," & Val(.Cells("GRSWT").Value.ToString) & "" 'NETWT
                StrSql += " ," & Val(.Cells("NETWT").Value.ToString) & "" 'NETWT
                StrSql += " ," & Val(.Cells("RATE").Value.ToString) & "" 'RATE
                StrSql += " ," & Val(.Cells("GROSSAMT").Value.ToString) & "" 'AMOUNT
                StrSql += " ,'" & .Cells("JOBNO").Value.ToString & "'" 'BAGNO
                StrSql += " ,'" & BatchNo & "'" 'BATCHNO
                StrSql += " ,'" & VERSION & "'" 'APPVER
                StrSql += " ,'" & strCompanyId & "'" 'COMPANYID
                StrSql += " ,'" & userId & "'" 'USERID
                StrSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                StrSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                StrSql += " ,''" 'CANCEL
                StrSql += " )"
                ExecQuery(SyncMode.Transaction, StrSql, cn, tran, cnCostId)
            Else
                If OMaterialType = MaterialType.Issue Then
                    StrSql = " INSERT INTO " & cnStockDb & ".." & IIf(_AccAudit, "TISSUE", "ISSUE")
                Else
                    StrSql = " INSERT INTO " & cnStockDb & ".." & IIf(_AccAudit, "TRECEIPT", "RECEIPT")
                End If
                StrSql += " ("
                StrSql += "  SNO,TRANNO,TRANDATE,TRANTYPE,STKTYPE,PCS"
                StrSql += " ,GRSWT,NETWT,LESSWT,PUREWT"
                StrSql += " ,TAGNO,ITEMID,SUBITEMID,WASTPER"
                StrSql += " ,WASTAGE,MCGRM,MCHARGE,AMOUNT"
                StrSql += " ,RATE,BOARDRATE,SALEMODE,GRSNET"
                StrSql += " ,TRANSTATUS,REFNO,REFDATE,COSTID"
                StrSql += " ,COMPANYID,FLAG,EMPID,TAGGRSWT"
                StrSql += " ,TAGNETWT,TAGRATEID,TAGSVALUE,TAGDESIGNER"
                StrSql += " ,ITEMCTRID,ITEMTYPEID,PURITY,TABLECODE"
                StrSql += " ,INCENTIVE,WEIGHTUNIT,CATCODE,OCATCODE"
                StrSql += " ,ACCODE,ALLOY,BATCHNO,REMARK1"
                StrSql += " ,REMARK2,USERID,UPDATED,UPTIME,SYSTEMID,DISCOUNT,RUNNO,CASHID,TAX,TDS"
                StrSql += " ,STNAMT,MISCAMT,METALID,STONEUNIT,APPVER,TOUCH,ORDSTATE_ID"
                If _JobNoEnable = True Then
                    StrSql += " ,JOBNO"
                ElseIf OrdLotid <> 0 Then
                    StrSql += " ,JOBNO"
                End If
                If OMaterialType = MaterialType.Receipt Then
                    StrSql += " ,JOBISNO"
                End If
                If OMaterialType = MaterialType.Issue Then StrSql += " ,RESNO"
                If OMaterialType = MaterialType.Receipt Then StrSql += " ,ISSNO"
                StrSql += " ,SEIVE,BAGNO,RFID,CUTID,COLORID,CLARITYID,SHAPEID,SETTYPEID,HEIGHT,WIDTH,CALCON"
                StrSql += " ,STNGRPID,APRXAMT,APRXTAX,RATEFIXED)"
                StrSql += " VALUES("
                StrSql += " '" & issSno & "'" ''SNO
                StrSql += " ," & TranNo & "" 'TRANNO
                StrSql += " ,'" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                ''StrSql += " ,'" & IIf(OMaterialType = MaterialType.Issue, "I", "R") & "" & Mid(.Cells("TRANTYPE").Value.ToString, 1, 2) & "'" 'TRANTYPE [07-AUG-2021]
                StrSql += " ,'" & IIf(OMaterialType = MaterialType.Issue, "I", "R") & "" _
                        & IIf(.Cells("TRANTYPE").Value.ToString = "DELIVERY NOTE" Or .Cells("TRANTYPE").Value.ToString = "RECEIPT NOTE", "DN" _
                        , Mid(.Cells("TRANTYPE").Value.ToString, 1, 2)) & "'" 'TRANTYPE 
                StrSql += " ,'" & _StkType & "'" 'STKTYPE
                StrSql += " ," & Val(.Cells("PCS").Value.ToString) & "" 'PCS
                StrSql += " ," & Val(.Cells("GRSWT").Value.ToString) & "" 'GRSWT
                StrSql += " ," & Val(.Cells("NETWT").Value.ToString) & "" 'NETWT
                StrSql += " ," & Val(.Cells("LESSWT").Value.ToString) & "" 'LESSWT
                StrSql += " ," & Val(.Cells("PUREWT").Value.ToString) & "" 'PUREWT
                ''StrSql += " ,''" 'TAGNO
                If .Cells("TAGNO").Value.ToString <> "" And (cmbTransactionType.Text = "ISSUE" Or cmbTransactionType.Text = "PURCHASE RETURN") Then
                    StrSql += " ,'" & .Cells("TAGNO").Value.ToString & "'" 'TAGNO
                Else
                    StrSql += " ,''" 'TAGNO
                End If
                StrSql += " ," & Itemid & "" 'ITEMID
                StrSql += " ," & subItemid & "" 'SUBITEMID
                StrSql += " ," & wastPer & "" 'WASTPER
                StrSql += " ," & wast & "" 'WASTAGE
                StrSql += " ," & Val(.Cells("MCGRM").Value.ToString) & "" 'MCGRM
                StrSql += " ," & Val(.Cells("MC").Value.ToString) & "" 'MCHARGE
                StrSql += " ," & Val(.Cells("GROSSAMT").Value.ToString) & "" 'AMOUNT
                StrSql += " ," & Val(.Cells("RATE").Value.ToString) & "" 'RATE
                StrSql += " ," & Val(.Cells("BOARDRATE").Value.ToString) & "" 'BOARDRATE
                StrSql += " ,''" 'SALEMODE
                StrSql += " ,'" & Mid(.Cells("GRSNET").Value.ToString, 1, 1) & "'" 'GRSNET
                StrSql += " ,''" 'TRANSTATUS ''
                StrSql += " ,'" & txtBillNo.Text & "'" 'REFNO ''
                StrSql += " ,'" & dtpBillDate_OWN.Value.ToString("yyyy-MM-dd") & "'" 'REFDATE NULL
                StrSql += " ,'" & CostCenterId & "'" 'COSTID 
                StrSql += " ,'" & strCompanyId & "'" 'COMPANYID
                StrSql += " ,'" & .Cells("TYPE").Value.ToString & "'" 'FLAG
                StrSql += " ,0" 'EMPID
                StrSql += " ,0" 'TAGGRSWT
                StrSql += " ,0" 'TAGNETWT
                StrSql += " ,0" 'TAGRATEID
                StrSql += " ,0" 'TAGSVALUE
                StrSql += " ,''" 'TAGDESIGNER  
                StrSql += " ,0" 'ITEMCTRID
                StrSql += " ," & itemTypeId & "" 'ITEMTYPEID
                StrSql += " ," & Val(.Cells("PURITY").Value.ToString) & "" 'PURITY
                StrSql += " ,''" 'TABLECODE
                StrSql += " ,''" 'INCENTIVE
                StrSql += " ,'" & Mid(.Cells("CALCMODE").Value.ToString, 1, 1) & "'" 'WEIGHTUNIT
                StrSql += " ,'" & catCode & "'" 'CATCODE
                StrSql += " ,'" & OCatcode & "'" 'OCATCODE
                StrSql += " ,'" & _Accode & "'" 'ACCODE
                StrSql += " ," & alloy & "" 'ALLOY
                StrSql += " ,'" & BatchNo & "'" 'BATCHNO
                ''StrSql += " ,'" & .Cells("REMARK1").Value.ToString & "'" 'REMARK1
                ''StrSql += " ,'" & .Cells("REMARK2").Value.ToString & "'" 'REMARK2

                If SPECIFICFORMAT = "1" And .Cells("REMARK1").Value.ToString = "" Then
                    StrSql += " ,'" & txtRemark1.Text & "'" 'REMARK1
                Else
                    StrSql += " ,'" & .Cells("REMARK1").Value.ToString & "'" 'REMARK1
                End If
                If SPECIFICFORMAT = "1" And .Cells("REMARK2").Value.ToString = "" Then
                    StrSql += " ,'" & txtRemark2.Text & "'" 'REMARK2
                Else
                    StrSql += " ,'" & .Cells("REMARK2").Value.ToString & "'" 'REMARK2
                End If

                StrSql += " ,'" & userId & "'" 'USERID
                StrSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                StrSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                StrSql += " ,'" & systemId & "'" 'SYSTEMID
                StrSql += " ,'" & Val(.Cells("DISCOUNT").Value.ToString) & "'" 'DISCOUNT
                StrSql += " ,''" 'RUNNO
                StrSql += " ,'" & _CashCtr & "'" 'CASHID
                StrSql += " ," & Tax & "" 'TAX
                StrSql += " ," & Tds & "" 'TDS
                StrSql += " ,0" 'STNAMT
                StrSql += " ,0" 'MISCAMT
                StrSql += " ,'" & objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & .Cells("METAL").Value.ToString & "'", , , tran) & "'" 'METALID
                StrSql += " ,'" & Mid(.Cells("UNIT").Value.ToString, 1, 1) & "'" 'STONEUNIT
                StrSql += " ,'" & VERSION & "'" 'APPVER
                StrSql += " ,'" & Val(.Cells("TOUCH").Value.ToString) & "'" 'APPVER
                StrSql += " ," & OrdStateId & "" 'ORDSTATE_ID
                If _JobNoEnable = True Then
                    StrSql += " ,'" & .Cells("JOBNO").Value.ToString & "'"
                ElseIf OrdLotid <> 0 Then
                    StrSql += " ," & OrdLotid & "" 'ORDlot_no
                End If
                If OMaterialType = MaterialType.Receipt Then
                    StrSql += " ,'" & .Cells("ORSNO").Value.ToString & "'" 'JOBISNO
                    Jobisno = .Cells("ORSNO").Value.ToString
                End If
                If OMaterialType = MaterialType.Issue Then StrSql += " ,'" & .Cells("RESNO").Value.ToString & "'" 'RESNO
                If OMaterialType = MaterialType.Receipt Then StrSql += " ,'" & .Cells("RESNO").Value.ToString & "'" 'RESNO
                StrSql += " ,'" & .Cells("SEIVE").Value.ToString & "'" 'SEIVE
                StrSql += ",'" & IIf(Transistno <> 0, Transistno.ToString, "") & "'" 'BAGNO
                StrSql += " ,'" & .Cells("RFID").Value.ToString & "'" 'RFID
                StrSql += " ," & Val(.Cells("CUTID").Value.ToString) & " " 'CUTID
                StrSql += " ," & Val(.Cells("COLORID").Value.ToString) & "" 'COLORID
                StrSql += " ," & Val(.Cells("CLARITYID").Value.ToString) & "" 'CLARITYID
                StrSql += " ," & Val(.Cells("SHAPEID").Value.ToString) & "" 'SHAPEID
                StrSql += " ," & Val(.Cells("SETTYPEID").Value.ToString) & "" 'SETTYPEID
                StrSql += " ,'" & Val(.Cells("HEIGHT").Value.ToString) & "'" 'HEIGHT
                StrSql += " ,'" & Val(.Cells("WIDTH").Value.ToString) & "'" 'WIDTH
                Dim TEMPCALCON As String
                If .Cells("CALCON").Value.ToString = "GRS WT" Then
                    TEMPCALCON = "G"
                ElseIf .Cells("CALCON").Value.ToString = "NET WT" Then
                    TEMPCALCON = "N"
                ElseIf .Cells("CALCON").Value.ToString = "PURE WT" Then
                    TEMPCALCON = "P"
                End If
                StrSql += " ,'" & TEMPCALCON & "'" 'CALCULATION ON
                StrSql += " ,'" & .Cells("STNGRPID").Value.ToString & "'" 'STNGRPID
                StrSql += " ,'" & Val(.Cells("APPROXAMT").Value.ToString) & "'" 'APPROXAMT
                StrSql += " ,'" & Val(.Cells("APPROXTAX").Value.ToString) & "'" 'APPROXTAX
                StrSql += " ,'" & IIf(.Cells("RATEFIXED").Value.ToString = "Y", "Y", "") & "'" 'RATEFIXED
                StrSql += " )"
                ExecQuery(SyncMode.Transaction, StrSql, cn, tran, CostCenterId)

                If OMaterialType = MaterialType.Issue Then
                    StrSql = $"select TAGTYPE from {cnAdminDb}..ITEMMAST with(nolock) where ITEMID = {Itemid}"

                    If objGPack.GetSqlValue(StrSql, , , tran) = "N" Then
                        Dim tagSno As String = GetNewSno(TranSnoType.ITEMNONTAGCODE, tran, "GET_ADMINSNO_TRAN")
                        StrSql = " INSERT INTO " & cnAdminDb & "..ITEMNONTAG"
                        StrSql += " ("
                        StrSql += " SNO,ITEMID,SUBITEMID,COMPANYID,RECDATE,"
                        StrSql += " PCS,GRSWT,LESSWT,NETWT,"
                        StrSql += " FINRATE,ISSTYPE,RECISS,POSTED,"
                        StrSql += " PACKETNO,DREFNO,ITEMCTRID,"
                        StrSql += " ORDREPNO,ORSNO,NARRATION,"
                        StrSql += " RATE,COSTID,"
                        StrSql += " CTGRM,DESIGNERID,ITEMTYPEID,"
                        StrSql += " CARRYFLAG,REASON,BATCHNO,CANCEL,"
                        StrSql += " USERID,UPDATED,UPTIME,SYSTEMID,APPVER,EXTRAWT,TCOSTID)VALUES("
                        StrSql += " '" & tagSno & "'" 'SNO
                        StrSql += " ," & Itemid & "" 'ITEMID
                        StrSql += " ," & subItemid & "" 'SUBITEMID
                        StrSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                        StrSql += " ,'" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "'" 'RECDATE
                        StrSql += " ," & Val(.Cells("PCS").Value.ToString) & "" 'PCS
                        StrSql += " ," & Val(.Cells("GRSWT").Value.ToString) & "" 'GRSWT
                        StrSql += " ," & Val(.Cells("LESSWT").Value.ToString) & "" 'LESSWT
                        StrSql += " ," & Val(.Cells("NETWT").Value.ToString) & "" 'NETWT
                        StrSql += " ," & Val(.Cells("RATE").Value.ToString) & "" 'FINRATE
                        StrSql += " ,'MI'" 'ISSTYPE
                        StrSql += " ,'I'" 'RECISS
                        StrSql += " ,''" 'POSTED
                        StrSql += " ,''" 'PACKETNO
                        StrSql += " ,0" 'DREFNO
                        StrSql += " ,NULL" 'ITEMCTRID
                        StrSql += " ,''" 'ORDREPNO
                        StrSql += " ,''" 'ORSNO
                        StrSql += " ,'Material ISSUE'" 'NARRATION
                        StrSql += " ," & Val(.Cells("BOARDRATE").Value.ToString) & "" 'RATE
                        StrSql += " ,'" & cnCostId & "'" 'COSTID
                        StrSql += " ,''"
                        StrSql += " ,0" 'DESIGNERID
                        StrSql += " ,0" 'ITEMTYPEID
                        StrSql += " ,''" 'CARRYFLAG
                        StrSql += " ,'0'" 'REASON
                        StrSql += " ,'" + BatchNo + "'" 'BATCHNO
                        StrSql += " ,''" 'CANCEL
                        StrSql += " ," & userId & "" 'USERID
                        StrSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                        StrSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                        StrSql += " ,'" & systemId & "'" 'SYSTEMID
                        StrSql += " ,'" & VERSION & "'" 'APPVER
                        StrSql += " ,'0'" 'EXTRAWT
                        StrSql += " ,'" & cnCostId & "'" 'TCOSTID
                        StrSql += " )"
                        ExecQuery(SyncMode.Transaction, StrSql, cn, tran, CostCenterId)
                    End If
                End If


                If _JobNoEnable = True And _JobNo = False Then
                        _JobNo = True
                        Dim MaxSno As Integer
                        StrSql = "SELECT ISNULL(MAX(SNO),0)+1 AS SNO FROM " & cnStockDb & "..JOBNODETAILS"
                        MaxSno = Val(objGPack.GetSqlValue(StrSql, "SNO", 1, tran).ToString)
                        StrSql = " INSERT INTO " & cnStockDb & "..JOBNODETAILS"
                        StrSql += " ("
                        StrSql += "  SNO,JOBNO,BATCHNO,ACCODE,TYPE)VALUES"
                        StrSql += " ("
                        StrSql += " " & MaxSno
                        StrSql += " ,'" & .Cells("JOBNO").Value.ToString & "'"
                        StrSql += " ,'" & BatchNo & "'" 'BATCHNO
                        StrSql += " ,'" & _Accode & "'" 'ACCODE
                        If OMaterialType = MaterialType.Issue Then
                            StrSql += " ,'I'"  'TYPE
                        Else
                            StrSql += " ,'R'"  'TYPE
                        End If
                        StrSql += " )"
                        ExecQuery(SyncMode.Transaction, StrSql, cn, tran, CostCenterId)
                    End If
                    If .Cells("RFID").Value.ToString.Trim <> "" Then
                        StrSql = " INSERT INTO " & cnStockDb & "..MIMRRFID (SNO,RFID,ISSSNO,TRANTYPE) VALUES( "
                        StrSql += vbCrLf + " '" & GetNewSno(TranSnoType.MIMRRFIDCODE, tran) & "'"
                        StrSql += vbCrLf + " ,'" & .Cells("RFID").Value.ToString.Trim & "'"
                        StrSql += vbCrLf + " ,'" & issSno & "'"
                        StrSql += vbCrLf + " ,'" & IIf(OMaterialType = MaterialType.Issue, "MI", "MR") & "'"
                        StrSql += vbCrLf + " )"
                        ExecQuery(SyncMode.Transaction, StrSql, cn, tran, CostCenterId)

                        StrSql = " SELECT 1 FROM " & cnAdminDb & "..SYSOBJECTS WHERE NAME='RFIDITEMTAGSTONE'"
                        If objGPack.GetSqlValue(StrSql, , 0, tran) = 1 Then
                            Dim RFIDTAG As String = .Cells("RFID").Value.ToString
                            Dim RFIDSno As String = ""
                            Dim RFIDStnSno As String = ""
                            StrSql = "SELECT SNO FROM " & cnAdminDb & "..RFIDITEMTAG WHERE RFIDNO='" & .Cells("RFID").Value.ToString.Trim & "'"
                            RFIDSno = objGPack.GetSqlValue(StrSql, "", "", tran).ToString
                            RFIDStnSno = ""
                            StrSql = "DECLARE @RETSNOVALUE VARCHAR(15) "
                            StrSql += " EXEC " & cnStockDb & "..GET_ADMINSNO_TRAN "
                            StrSql += " @COSTID = '" + CostCenterId + "' ,"
                            StrSql += " @CTLID = 'RFIDITEMTAGSTONESNO' ,"
                            StrSql += " @CHECKTABLENAME = 'RFIDITEMTAGSTONE' ,"
                            StrSql += " @COMPANYID = '" + strCompanyId + "' ,"
                            StrSql += " @RETVALUE = @RETSNOVALUE "
                            StrSql += " OUTPUT SELECT @RETSNOVALUE"
                            RFIDStnSno = objGPack.GetSqlValue(StrSql, "", "", tran).ToString

                            'Insert Rfid Keyno
                            Dim RFIDKEYNO As String = ""
                            Dim RecSno As String
                            Dim Cent As String
                            Dim TagColorId, TagCutId, TagClarityId, TagShapeId, TagSetTypeId As Integer
                            Dim TagHeight, TagWidth As Decimal
                            Dim TagRate, TagAmt As Decimal
                            Dim TagCalMode, TagUnitt As String

                            StrSql = " SELECT CENT FROM " & cnAdminDb & "..RFIDKEYMAST WHERE RFIDNO='" & RFIDTAG & "' "
                            Cent = objGPack.GetSqlValue(StrSql, "CENT", "", tran)

                            Dim dt As New DataTable
                            StrSql = " SELECT RECSNO,COLORID "
                            StrSql += " ,CUTID,CLARITYID,SHAPEID,SETTYPEID,HEIGHT,WIDTH"
                            StrSql += " ,STNRATE,STNAMT,CALCMODE,STONEUNIT"
                            StrSql += " FROM " & cnAdminDb & "..RFIDITEMTAGSTONE WHERE RFIDNO='" & RFIDTAG & "' AND ISSREC='R'"
                            Cmd = New OleDbCommand(StrSql, cn, tran)
                            Da = New OleDbDataAdapter(Cmd)
                            Da.Fill(dt)
                            If dt.Rows.Count > 0 Then
                                With dt.Rows(0)
                                    RecSno = .Item("RECSNO").ToString
                                    TagColorId = Val(.Item("COLORID").ToString)
                                    TagCutId = Val(.Item("CUTID").ToString)
                                    TagClarityId = Val(.Item("CLARITYID").ToString)
                                    TagShapeId = Val(.Item("SHAPEID").ToString)
                                    TagSetTypeId = Val(.Item("SETTYPEID").ToString)
                                    TagHeight = Val(.Item("HEIGHT").ToString)
                                    TagWidth = Val(.Item("WIDTH").ToString)
                                    TagRate = Val(.Item("STNRATE").ToString)
                                    TagAmt = Val(.Item("STNAMT").ToString)
                                    TagCalMode = .Item("CALCMODE").ToString
                                    TagUnitt = .Item("STONEUNIT").ToString
                                End With
                            End If

                            RFIDKEYNO = Convert.ToString(RFIDTAG.ToString + Format(Val(Cent), "0").ToString + TagCutId.ToString + TagColorId.ToString + TagClarityId.ToString + TagShapeId.ToString + TagSetTypeId.ToString).ToString

                            StrSql = "INSERT INTO " & cnAdminDb & "..RFIDITEMTAGSTONE (SNO,RFIDSNO,RECDATE,ITEMID,COMPANYID,STNITEMID,STNSUBITEMID,RFIDNO,"
                            StrSql += " TAGNO,STNPCS,STNWT,STNRATE,STNAMT,CALCMODE,STONEUNIT,ISSDATE,COSTID,SYSTEMID,"
                            StrSql += " APPVER,TRANSFERED,USERID,TOFLAG,DESCRIP,UPDATED,UPTIME,ISSREC"
                            StrSql += " ,CUTID,COLORID,CLARITYID,SETTYPEID,SHAPEID,HEIGHT,WIDTH,KEYNO,RECSNO,BATCHNO)VALUES("
                            StrSql += " '" & RFIDStnSno & "','" & RFIDSno.ToString & "'"
                            StrSql += " ,'" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "'"
                            StrSql += ",0" 'ITEMID
                            StrSql += ",'" & strCompanyId & "'"
                            StrSql += ",'" & Itemid & "'"
                            StrSql += ",'" & subItemid & "'"
                            StrSql += ",'" & RFIDTAG & "'"
                            StrSql += ",'" & "".ToString & "'"
                            StrSql += ",'" & Val(.Cells("PCS").Value.ToString) & "'"
                            StrSql += ",'" & Val(.Cells("GRSWT").Value.ToString) & "'"
                            StrSql += ",'" & TagRate & "'"
                            StrSql += ",'" & TagAmt & "'"
                            StrSql += ",'" & TagCalMode & "'"
                            StrSql += ",'" & TagUnitt & "'"
                            StrSql += ",NULL"
                            StrSql += ",'" & CostCenterId & "'"
                            StrSql += ",'" & userId & "'" 'Systemid
                            StrSql += ",'" & VERSION & "'" 'Appver
                            StrSql += ",'" & "".ToString & "'" 'Transfered
                            StrSql += "," & userId & "" 'USERID
                            StrSql += ",'" & "".ToString & "'" 'Toflag
                            StrSql += ",'" & "".ToString & "'" 'Descrip
                            StrSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                            StrSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & " " & Date.Now.ToLongTimeString & "'" 'UPTIME
                            StrSql += ",'" & "I".ToString & "'"
                            StrSql += ",'" & TagCutId.ToString & "'"
                            StrSql += ",'" & TagColorId.ToString & "'"
                            StrSql += ",'" & TagClarityId.ToString & "'"
                            StrSql += ",'" & TagSetTypeId.ToString & "'"
                            StrSql += ",'" & TagShapeId.ToString & "'"
                            StrSql += ",'" & TagHeight.ToString & "'"
                            StrSql += ",'" & TagWidth.ToString & "'"
                            StrSql += ",'" & RFIDKEYNO.ToString & "'"
                            StrSql += ",'" & RecSno & "','" & BatchNo & "')"
                            ExecQuery(SyncMode.Transaction, StrSql, cn, tran, CostCenterId)

                            StrSql = " SELECT RFIDNO,KEYNO,SUM(CASE WHEN ISSREC='R' THEN STNPCS ELSE -1*STNPCS END)AS STNPCS,"
                            StrSql += vbCrLf + " SUM(CASE WHEN ISSREC='R' THEN STNWT ELSE -1*STNWT END)AS STNWT "
                            StrSql += vbCrLf + " FROM " & cnAdminDb & "..RFIDITEMTAGSTONE"
                            StrSql += vbCrLf + " WHERE RFIDNO='" & RFIDTAG & "' AND KEYNO='" & RFIDKEYNO & "'"
                            StrSql += vbCrLf + " GROUP BY RFIDNO,KEYNO"
                            Dim StnBalPcs As Integer
                            Dim StnBalWt As Integer
                            Cmd = New OleDbCommand(StrSql, cn, tran)
                            Da = New OleDbDataAdapter(Cmd)
                            dt = New DataTable
                            Da.Fill(dt)
                            If dt.Rows.Count > 0 Then
                                StnBalPcs = Val(dt.Rows(0).Item("STNPCS").ToString)
                                StnBalWt = Val(dt.Rows(0).Item("STNWT").ToString)
                                If StnBalPcs = 0 And StnBalWt = 0 Then
                                    StrSql = " UPDATE " & cnAdminDb & "..RFIDKEYMAST SET STATUS='C' WHERE RFIDNO='" & RFIDTAG & "' AND KEYNO='" & RFIDKEYNO & "'"
                                    Cmd = New OleDbCommand(StrSql, cn, tran)
                                    Cmd.ExecuteNonQuery()
                                End If
                            End If
                        End If
                    End If
                    If cmbTransactionType.Text = "PURCHASE[APPROVAL]" Then
                        'For Reverse entry only for Transaction Type PURCHASE[APPROVAL]
                        Dim RecTouch As Decimal = 0
                        Dim RecPurity As Decimal = 0
                        Dim RecPurewt As Decimal = 0
                        StrSql = "SELECT TOUCH,PURITY FROM " & cnStockDb & "..RECEIPT WHERE SNO='" & .Cells("ORSNO").Value.ToString() & "'"
                        Dim drr As DataRow = GetSqlRow(StrSql, cn, tran)
                        If Not drr Is Nothing Then
                            RecTouch = Val(drr("TOUCH").ToString)
                            RecPurity = Val(drr("PURITY").ToString)
                            RecPurewt = funcCalcOPureWt(Mid(.Cells("GRSNET").Value.ToString, 1, 1),
                            Val(.Cells("GRSWT").Value.ToString), Val(.Cells("NETWT").Value.ToString) _
                            , wast, RecTouch, RecPurity, .Cells("METAL").Value.ToString)
                        End If
                        issAppNo = GetNewSno(IIf(_AccAudit, TranSnoType.TISSUECODE, TranSnoType.ISSUECODE), tran)
                        StrSql = " INSERT INTO " & cnStockDb & "..ISSUE"
                        StrSql += " ("
                        StrSql += "  SNO,TRANNO,TRANDATE,TRANTYPE,STKTYPE,PCS"
                        StrSql += " ,GRSWT,NETWT,LESSWT,PUREWT"
                        StrSql += " ,TAGNO,ITEMID,SUBITEMID,WASTPER"
                        StrSql += " ,WASTAGE,MCGRM,MCHARGE,AMOUNT"
                        StrSql += " ,RATE,BOARDRATE,SALEMODE,GRSNET"
                        StrSql += " ,TRANSTATUS,REFNO,REFDATE,COSTID"
                        StrSql += " ,COMPANYID,FLAG,EMPID,TAGGRSWT"
                        StrSql += " ,TAGNETWT,TAGRATEID,TAGSVALUE,TAGDESIGNER"
                        StrSql += " ,ITEMCTRID,ITEMTYPEID,PURITY,TABLECODE"
                        StrSql += " ,INCENTIVE,WEIGHTUNIT,CATCODE,OCATCODE"
                        StrSql += " ,ACCODE,ALLOY,BATCHNO,REMARK1"
                        StrSql += " ,REMARK2,USERID,UPDATED,UPTIME,SYSTEMID,DISCOUNT,RUNNO,CASHID,TAX,TDS"
                        StrSql += " ,STNAMT,MISCAMT,METALID,STONEUNIT,APPVER,TOUCH,ORDSTATE_ID"
                        If _JobNoEnable = True Then
                            StrSql += " ,JOBNO"
                        ElseIf OrdLotid <> 0 Then
                            StrSql += " ,JOBNO"
                        End If
                        StrSql += " ,SEIVE,BAGNO "
                        StrSql += " ,STNGRPID,APRXAMT,APRXTAX,RATEFIXED)"
                        StrSql += " VALUES("
                        StrSql += " '" & issAppNo & "'" ''SNO
                        StrSql += " ," & TranNoApp & "" 'TRANNO
                        StrSql += " ,'" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                        StrSql += " ,'IAP'" 'TRANTYPE
                        StrSql += " ,'" & _StkType & "'" 'STKTYPE
                        StrSql += " ," & Val(.Cells("PCS").Value.ToString) & "" 'PCS
                        StrSql += " ," & Val(.Cells("GRSWT").Value.ToString) & "" 'GRSWT
                        StrSql += " ," & Val(.Cells("NETWT").Value.ToString) & "" 'NETWT
                        StrSql += " ," & Val(.Cells("LESSWT").Value.ToString) & "" 'LESSWT
                        StrSql += " ," & RecPurewt & "" 'PUREWT
                        ''StrSql += " ,''" 'TAGNO
                        If .Cells("TAGNO").Value.ToString <> "" And (cmbTransactionType.Text = "ISSUE" Or cmbTransactionType.Text = "PURCHASE RETURN") Then
                            StrSql += " ,'" & .Cells("TAGNO").Value.ToString & "'" 'TAGNO
                        Else
                            StrSql += " ,''" 'TAGNO
                        End If
                        StrSql += " ," & Itemid & "" 'ITEMID
                        StrSql += " ," & subItemid & "" 'SUBITEMID
                        StrSql += " ," & wastPer & "" 'WASTPER
                        StrSql += " ," & wast & "" 'WASTAGE
                        StrSql += " ,NULL" 'MCGRM
                        StrSql += " ,NULL"  'MCHARGE
                        StrSql += " ,NULL"  'AMOUNT
                        'StrSql += " ," & Val(.Cells("MCGRM").Value.ToString) & "" 'MCGRM
                        'StrSql += " ," & Val(.Cells("MC").Value.ToString) & "" 'MCHARGE
                        'StrSql += " ," & Val(.Cells("GROSSAMT").Value.ToString) & "" 'AMOUNT
                        StrSql += " ," & Val(.Cells("RATE").Value.ToString) & "" 'RATE
                        StrSql += " ," & Val(.Cells("BOARDRATE").Value.ToString) & "" 'BOARDRATE
                        StrSql += " ,''" 'SALEMODE
                        StrSql += " ,'" & Mid(.Cells("GRSNET").Value.ToString, 1, 1) & "'" 'GRSNET
                        StrSql += " ,''" 'TRANSTATUS ''
                        StrSql += " ,'" & .Cells("ORSNO").Value.ToString & "'" 'REFNO ''
                        StrSql += " ,NULL" 'REFDATE NULL
                        StrSql += " ,'" & CostCenterId & "'" 'COSTID 
                        StrSql += " ,'" & strCompanyId & "'" 'COMPANYID
                        StrSql += " ,'" & .Cells("TYPE").Value.ToString & "'" 'FLAG
                        StrSql += " ,0" 'EMPID
                        StrSql += " ,0" 'TAGGRSWT
                        StrSql += " ,0" 'TAGNETWT
                        StrSql += " ,0" 'TAGRATEID
                        StrSql += " ,0" 'TAGSVALUE
                        StrSql += " ,''" 'TAGDESIGNER  
                        StrSql += " ,0" 'ITEMCTRID
                        StrSql += " ," & itemTypeId & "" 'ITEMTYPEID
                        StrSql += " ," & RecPurity & "" 'PURITY
                        StrSql += " ,''" 'TABLECODE
                        StrSql += " ,''" 'INCENTIVE
                        StrSql += " ,'" & Mid(.Cells("CALCMODE").Value.ToString, 1, 1) & "'" 'WEIGHTUNIT
                        StrSql += " ,'" & catCode & "'" 'CATCODE
                        StrSql += " ,'" & OCatcode & "'" 'OCATCODE
                        If .Cells("ACCODE").Value.ToString <> "" Then
                            StrSql += " ,'" & .Cells("ACCODE").Value.ToString & "'" 'REFNO ''
                        Else
                            StrSql += " ,'" & _Accode & "'" 'ACCODE
                        End If
                        StrSql += " ," & alloy & "" 'ALLOY
                        StrSql += " ,'" & BatchNo & "'" 'BATCHNO
                        ''StrSql += " ,'" & .Cells("REMARK1").Value.ToString & "'" 'REMARK1
                        ''StrSql += " ,'REVERSE ENTRY AGAINST PURCHASE[APP]'" 'REMARK2

                        If SPECIFICFORMAT = "1" And .Cells("REMARK1").Value.ToString = "" Then
                            StrSql += " ,'" & txtRemark1.Text & "'" 'REMARK1
                        Else
                            StrSql += " ,'" & .Cells("REMARK1").Value.ToString & "'" 'REMARK1
                        End If
                        If SPECIFICFORMAT = "1" And .Cells("REMARK2").Value.ToString = "" Then
                            StrSql += " ,'" & txtRemark2.Text & "'" 'REMARK2
                        Else
                            StrSql += " ,'REVERSE ENTRY AGAINST PURCHASE[APP]'" 'REMARK2
                        End If

                        StrSql += " ,'" & userId & "'" 'USERID
                        StrSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                        StrSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                        StrSql += " ,'" & systemId & "'" 'SYSTEMID
                        StrSql += " ,0" 'DISCOUNT
                        StrSql += " ,''" 'RUNNO
                        StrSql += " ,'" & _CashCtr & "'" 'CASHID
                        StrSql += " ," & Tax & "" 'TAX
                        StrSql += " ," & Tds & "" 'TDS
                        StrSql += " ,0" 'STNAMT
                        StrSql += " ,0" 'MISCAMT
                        StrSql += " ,'" & objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & .Cells("METAL").Value.ToString & "'", , , tran) & "'" 'METALID
                        StrSql += " ,'" & Mid(.Cells("UNIT").Value.ToString, 1, 1) & "'" 'STONEUNIT
                        StrSql += " ,'" & VERSION & "'" 'APPVER
                        StrSql += " ,'" & RecTouch & "'" 'TOUCH
                        StrSql += " ," & OrdStateId & "" 'ORDSTATE_ID
                        If _JobNoEnable = True Then
                            StrSql += " ,'" & .Cells("JOBNO").Value.ToString & "'"
                        ElseIf OrdLotid <> 0 Then
                            StrSql += " ," & OrdLotid & "" 'ORDlot_no
                        End If
                        StrSql += " ,'" & .Cells("SEIVE").Value.ToString & "'" 'SEIVE
                        StrSql += ",'" & IIf(Transistno <> 0, Transistno.ToString, "") & "'" 'BAGNO
                        StrSql += " ,'" & .Cells("STNGRPID").Value.ToString & "'" 'STNGRPID
                        StrSql += " ,'" & Val(.Cells("APPROXAMT").Value.ToString) & "'" 'APPROXAMT
                        StrSql += " ,'" & Val(.Cells("APPROXTAX").Value.ToString) & "'" 'APPROXTAX
                        StrSql += " ,'" & IIf(.Cells("RATEFIXED").Value.ToString = "Y", "Y", "") & "'" 'RATEFIXED
                        StrSql += " )"
                        ExecQuery(SyncMode.Transaction, StrSql, cn, tran, CostCenterId)
                    End If
                End If


                If _JobNoEnable = True And EditBatchno = Nothing Then
                If (GetCostId(cnCostId) & GetCompanyId(strCompanyId) & "J" & Mid(Format(cnTranToDate, "dd/MM/yyyy"), 9, 2).ToString & (ObjMaterialDia.JobNo + 1)) = .Cells("JOBNO").Value.ToString Then
                    StrSql = " UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT = '" & (ObjMaterialDia.JobNo + 1) & "' "
                    StrSql += " WHERE CTLID = 'GEN-JOBNO' AND COMPANYID = '" & strCompanyId & "'"
                    StrSql += " AND CONVERT(INT,CTLTEXT) = " & ObjMaterialDia.JobNo & ""
                    Cmd = New OleDbCommand(StrSql, cn, tran)
                    Cmd.ExecuteNonQuery()
                End If
            End If

            If EditBatchno <> Nothing And OMaterialType = MaterialType.Receipt And DtEditDet.Rows.Count > 0 Then
                If DtEditDet.Rows.Count > index Then
                    StrSql = " UPDATE " & cnStockDb & "..LOTISSUE SET RECSNO='" & issSno & "' WHERE RECSNO='" & DtEditDet.Rows(index).Item("SNO").ToString & "' AND TRANNO='" & DtEditDet.Rows(index).Item("TRANNO").ToString & "'"
                    ExecQuery(SyncMode.Transaction, StrSql, cn, tran, CostCenterId)
                    StrSql = " UPDATE " & cnStockDb & "..ISSUE SET RESNO='" & issSno & "' WHERE RESNO='" & DtEditDet.Rows(index).Item("SNO").ToString & "' "
                    ExecQuery(SyncMode.Transaction, StrSql, cn, tran, CostCenterId)
                End If
            End If
            If EditBatchno <> Nothing And OMaterialType = MaterialType.Issue And DtEditIss.Rows.Count > 0 Then
                If DtEditIss.Rows.Count > index Then
                    StrSql = " UPDATE " & cnStockDb & "..ISSUE SET RESNO='" & DtEditIss.Rows(index).Item("RESNO").ToString & "' "
                    StrSql += " WHERE  SNO='" & issSno & "'"
                    ExecQuery(SyncMode.Transaction, StrSql, cn, tran, CostCenterId)
                End If
            End If
            ''UPDATE ITEMTAG
            If .Cells("TAGNO").Value.ToString <> "" Then
                ''StrSql = " UPDATE " & cnAdminDb & "..ITEMTAG SET BATCHNO='" & BatchNo & "',ISSDATE='" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "',TOFLAG='MI'  WHERE TAGNO='" & .Cells("TAGNO").Value.ToString & "' "
                StrSql = " EXEC " & cnAdminDb & "..DISABLEENABLE_AUDITTRG 'ITEMTAG','DISABLE','UPD'"
                StrSql += " UPDATE " & cnAdminDb & "..ITEMTAG SET BATCHNO='" & BatchNo & "',ISSDATE='" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "',"
                StrSql += " TOFLAG='MI',ISSWT=" & Val(.Cells("GRSWT").Value.ToString) & "  WHERE ITEMID='" & Itemid.ToString & "' AND TAGNO='" & .Cells("TAGNO").Value.ToString & "' "
                StrSql += vbCrLf + " EXEC " & cnAdminDb & "..DISABLEENABLE_AUDITTRG 'ITEMTAG','ENABLE','UPD'"
                ExecQuery(SyncMode.Transaction, StrSql, cn, tran, CostCenterId)
            End If

            Dim dtOrIrStone As New DataTable
            dtOrIrStone = CType(DgvTran.Rows(index).Cells("METISSREC").Value, MaterialIssRec).objStone.dtGridStone.Copy
            Dim Dpcs, Spcs, Dwt, Swt As Decimal
            ''Stone
            Dpcs = 0 : Spcs = 0 : Dwt = 0 : Swt = 0
            For Each stRow As DataRow In CType(.Cells("METISSREC").Value, MaterialIssRec).objStone.dtGridStone.Rows
                'If Lotautopost = True Then
                '    StrSql = " SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & stRow.Item(2).ToString & "'"
                '    Dim METALID As String = objGPack.GetSqlValue(StrSql, , , tran).ToUpper
                '    Select Case METALID
                '        Case "D"
                '            Dpcs += Val(.Cells("PCS").Value.ToString)
                '            Dwt += .Cells("WEIGHT").Value.ToString
                '        Case "S"
                '            Spcs += Val(.Cells("PCS").Value.ToString)
                '            If .Cells("UNIT").Value = "G" Then Swt += Val(.Cells("WEIGHT").Value) Else Swt += Val(.Cells("WEIGHT").Value) / 5
                '        Case "P"
                '            Spcs += Val(.Cells("PCS").Value.ToString)
                '            If .Cells("UNIT").Value = "G" Then Swt += Val(.Cells("WEIGHT").Value) Else Swt += Val(.Cells("WEIGHT").Value) / 5
                '    End Select
                'End If
                InsertStoneDetails(issSno, TranNo, stRow, Tax)
                If cmbTransactionType.Text = "PURCHASE[APPROVAL]" Then
                    'For Reverse entry only for Transaction Type PURCHASE[APPROVAL]
                    InsertStoneDetailsforReverseEntry(issAppNo, TranNoApp, stRow, Tax)
                End If
            Next
            ''ORADDITIONAL DETAILS
            Dim dtemp As New DataTable
            dtemp = CType(.Cells("METISSREC").Value, MaterialIssRec).objAddtionalDetails.dtOrAdditionalDetails
            Dim Trantype As String
            For Each dr As DataRow In CType(.Cells("METISSREC").Value, MaterialIssRec).objAddtionalDetails.dtOrAdditionalDetails.Rows
                Trantype = IIf(OMaterialType = MaterialType.Issue, "I", "R") & "" & Mid(cmbTransactionType.Text, 1, 2)  'TRANTYPE
                Dim typeid As Integer = GetSqlValue("SELECT TYPEID FROM " & cnAdminDb & "..ORADMAST WHERE TYPENAME = '" & dr.Item("TYPENAME").ToString & "'", cn, tran)
                Dim VALUENAME As String = dr.Item("VALUENAME").ToString
                InsertOrderAdditionalDetails(TranNo, issSno, Trantype, CostCenterId, strCompanyId, BatchNo, typeid, VALUENAME, tran)
            Next
            ''Misc
            For Each miscRow As DataRow In CType(.Cells("METISSREC").Value, MaterialIssRec).objMisc.dtGridMisc.Rows
                InsertMiscDetails(issSno, TranNo, miscRow)
            Next
            ''ALLOY
            If CType(.Cells("METISSREC").Value, MaterialIssRec).ObjAlloy.dtGridAlloy.Rows.Count > 0 Then
                For Each stRow As DataRow In CType(.Cells("METISSREC").Value, MaterialIssRec).ObjAlloy.dtGridAlloy.Rows
                    InsertAlloyDetails(issSno, TranNo, stRow, alloy)
                Next
            Else
                If alloy <> 0 Then
                    InsertAlloyDetails(issSno, TranNo, Nothing, alloy)
                End If
            End If

            'If _JobNoEnable = False Then
            '    ''ORSTATUS
            '    If .Cells("JOBNO").Value.ToString <> "" And (Mid(.Cells("JOBNO").Value.ToString, 1, 1) = "R" Or Mid(.Cells("JOBNO").Value.ToString, 1, 1) = "O") Then
            '        Dim DtOrIrDetail As New DataTable
            '        DtOrIrDetail = BrighttechPack.GetTableStructure(cnAdminDb, "ORIRDETAIL", cn, tran)
            '        Dim R() As DataRow = CType(CType(.Cells("METISSREC").Value, MaterialIssRec).objOrderInfo.DgvOrder.DataSource, DataTable).Select("CHECK = 'TRUE'")
            '        Dim SelectedOrderCount As Integer = R.Length
            '        Dim EntryOrder As Integer = objGPack.GetSqlValue("SELECT ISNULL(MAX(ENTRYORDER),0) FROM " & cnAdminDb & "..ORIRDETAIL", , , tran)
            '        Dim RowOrder As DataRow = Nothing
            '        If OMaterialType = MaterialType.Receipt Then
            '            For Each DgvRow As DataGridViewRow In CType(.Cells("METISSREC").Value, MaterialIssRec).objOrderInfo.DgvOrder.Rows
            '                If CType(DgvRow.Cells("CHECK").Value, Boolean) <> True Then Continue For
            '                RowOrder = DtOrIrDetail.NewRow
            '                EntryOrder += 1
            '                RowOrder("SNO") = GetNewSno(TranSnoType.ORIRDETAILCODE, tran, "GET_ADMINSNO_TRAN")
            '                RowOrder("ORSNO") = DgvRow.Cells("SNO").Value.ToString
            '                RowOrder("TRANNO") = TranNo
            '                RowOrder("TRANDATE") = dtpTrandate.Value.ToString("yyyy-MM-dd")
            '                RowOrder("DESIGNERID") = Val(objGPack.GetSqlValue(" SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE ISNULL(ACCODE,'') = '" & _Accode & "'", , , tran))
            '                RowOrder("PCS") = IIf(SelectedOrderCount = 1, Val(.Cells("PCS").Value.ToString), Val(DgvRow.Cells("PCS").Value.ToString))
            '                RowOrder("GRSWT") = IIf(SelectedOrderCount = 1, Val(.Cells("GRSWT").Value.ToString), Val(DgvRow.Cells("GRSWT").Value.ToString))
            '                RowOrder("NETWT") = IIf(SelectedOrderCount = 1, Val(.Cells("NETWT").Value.ToString), Val(DgvRow.Cells("NETWT").Value.ToString))
            '                RowOrder("WASTAGE") = IIf(SelectedOrderCount = 1, wast, Val(DgvRow.Cells("WASTAGE").Value.ToString))
            '                RowOrder("MC") = IIf(SelectedOrderCount = 1, Val(.Cells("MC").Value.ToString), Val(DgvRow.Cells("MC").Value.ToString))
            '                RowOrder("TAGNO") = DBNull.Value
            '                RowOrder("ORSTATUS") = IIf(OMaterialType = MaterialType.Issue, "I", "R")
            '                RowOrder("CANCEL") = DBNull.Value
            '                RowOrder("COSTID") = CostCenterId
            '                RowOrder("DESCRIPT") = DBNull.Value
            '                RowOrder("ORNO") = GetCostId(CostCenterId) + GetCompanyId(strCompanyId) + DgvRow.Cells("ORNO").Value.ToString
            '                RowOrder("BATCHNO") = BatchNo
            '                RowOrder("USERID") = userId
            '                RowOrder("UPDATED") = Today.Date.ToString("yyyy-MM-dd")
            '                RowOrder("UPTIME") = Date.Now.ToLongTimeString
            '                RowOrder("APPVER") = VERSION
            '                RowOrder("COMPANYID") = strCompanyId
            '                RowOrder("TRANSFERED") = DBNull.Value
            '                RowOrder("PROID") = DBNull.Value
            '                RowOrder("ORDSTATE_ID") = objGPack.GetSqlValue("SELECT ORDSTATE_ID FROM " & cnAdminDb & "..ORDERSTATUS WHERE ORDSTATE_NAME = '" & .Cells("ORDSTATE_NAME").Value.ToString & "'", , , tran)
            '                RowOrder("ENTRYORDER") = EntryOrder
            '                RowOrder("CATCODE") = catCode
            '                DtOrIrDetail.Rows.Add(RowOrder)
            '            Next
            '        Else
            '            For Each DgvRow As DataGridViewRow In dgvOrderDet.Rows
            '                RowOrder = DtOrIrDetail.NewRow
            '                EntryOrder += 1
            '                RowOrder("SNO") = GetNewSno(TranSnoType.ORIRDETAILCODE, tran, "GET_ADMINSNO_TRAN")
            '                RowOrder("ORSNO") = DgvRow.Cells("ORSNO").Value.ToString
            '                RowOrder("TRANNO") = TranNo
            '                RowOrder("TRANDATE") = dtpTrandate.Value.ToString("yyyy-MM-dd")
            '                RowOrder("DESIGNERID") = Val(objGPack.GetSqlValue(" SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE ISNULL(ACCODE,'') = '" & _Accode & "'", , , tran))
            '                'RowOrder("PCS") = IIf(SelectedOrderCount = 1, Val(.Cells("PCS").Value.ToString), Val(DgvRow.Cells("PCS").Value.ToString))
            '                'RowOrder("GRSWT") = IIf(SelectedOrderCount = 1, Val(.Cells("GRSWT").Value.ToString), Val(DgvRow.Cells("GRSWT").Value.ToString))
            '                'RowOrder("NETWT") = IIf(SelectedOrderCount = 1, Val(.Cells("NETWT").Value.ToString), Val(DgvRow.Cells("NETWT").Value.ToString))
            '                'RowOrder("WASTAGE") = IIf(SelectedOrderCount = 1, wast, Val(DgvRow.Cells("WASTAGE").Value.ToString))
            '                'RowOrder("MC") = IIf(SelectedOrderCount = 1, Val(.Cells("MC").Value.ToString), Val(DgvRow.Cells("MC").Value.ToString))

            '                RowOrder("PCS") = Val(DgvRow.Cells("PCS").Value.ToString)
            '                RowOrder("GRSWT") = Val(DgvRow.Cells("GRSWT").Value.ToString)
            '                RowOrder("NETWT") = Val(DgvRow.Cells("NETWT").Value.ToString)
            '                RowOrder("WASTAGE") = Val(DgvRow.Cells("WASTAGE").Value.ToString)
            '                RowOrder("MC") = Val(DgvRow.Cells("MC").Value.ToString)

            '                RowOrder("TAGNO") = DBNull.Value
            '                RowOrder("ORSTATUS") = IIf(OMaterialType = MaterialType.Issue, "I", "R")
            '                RowOrder("CANCEL") = DBNull.Value
            '                RowOrder("COSTID") = CostCenterId
            '                RowOrder("DESCRIPT") = DBNull.Value
            '                RowOrder("ORNO") = GetCostId(CostCenterId) + GetCompanyId(strCompanyId) + DgvRow.Cells("ORNO").Value.ToString
            '                RowOrder("BATCHNO") = BatchNo
            '                RowOrder("USERID") = userId
            '                RowOrder("UPDATED") = Today.Date.ToString("yyyy-MM-dd")
            '                RowOrder("UPTIME") = Date.Now.ToLongTimeString
            '                RowOrder("APPVER") = VERSION
            '                RowOrder("COMPANYID") = strCompanyId
            '                RowOrder("TRANSFERED") = DBNull.Value
            '                RowOrder("PROID") = DBNull.Value
            '                RowOrder("ORDSTATE_ID") = objGPack.GetSqlValue("SELECT ORDSTATE_ID FROM " & cnAdminDb & "..ORDERSTATUS WHERE ORDSTATE_NAME = '" & .Cells("ORDSTATE_NAME").Value.ToString & "'", , , tran)
            '                RowOrder("ENTRYORDER") = EntryOrder
            '                RowOrder("CATCODE") = catCode
            '                DtOrIrDetail.Rows.Add(RowOrder)
            '            Next
            '        End If
            '        InsertData(SyncMode.Transaction, DtOrIrDetail, cn, tran, CostCenterId)
            '    End If
            'End If

            If _JobNoEnable = False Then
                ''ORSTATUS
                'If .Cells("JOBNO").Value.ToString <> "" And (Mid(.Cells("JOBNO").Value.ToString, 1, 1) = "R" Or Mid(.Cells("JOBNO").Value.ToString, 1, 1) = "O") Then
                '    Dim DtOrIrDetail As New DataTable
                '    DtOrIrDetail = BrighttechPack.GetTableStructure(cnAdminDb, "ORIRDETAIL", cn, tran)
                '    Dim R() As DataRow = CType(CType(.Cells("METISSREC").Value, MaterialIssRec).objOrderInfo.DgvOrder.DataSource, DataTable).Select("CHECK = 'TRUE'")
                '    Dim SelectedOrderCount As Integer = R.Length
                '    Dim EntryOrder As Integer = objGPack.GetSqlValue("SELECT ISNULL(MAX(ENTRYORDER),0) FROM " & cnAdminDb & "..ORIRDETAIL", , , tran)
                '    Dim RowOrder As DataRow = Nothing
                '    If OMaterialType = MaterialType.Receipt Then
                '        For Each DgvRow As DataGridViewRow In CType(.Cells("METISSREC").Value, MaterialIssRec).objOrderInfo.DgvOrder.Rows
                '            If CType(DgvRow.Cells("CHECK").Value, Boolean) <> True Then Continue For
                '            RowOrder = DtOrIrDetail.NewRow
                '            EntryOrder += 1
                '            RowOrder("SNO") = GetNewSno(TranSnoType.ORIRDETAILCODE, tran, "GET_ADMINSNO_TRAN")
                '            RowOrder("ORSNO") = DgvRow.Cells("SNO").Value.ToString
                '            RowOrder("TRANNO") = TranNo
                '            RowOrder("TRANDATE") = dtpTrandate.Value.ToString("yyyy-MM-dd")
                '            RowOrder("DESIGNERID") = Val(objGPack.GetSqlValue(" SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE ISNULL(ACCODE,'') = '" & _Accode & "'", , , tran))
                '            RowOrder("PCS") = IIf(SelectedOrderCount = 1, Val(.Cells("PCS").Value.ToString), Val(DgvRow.Cells("PCS").Value.ToString))
                '            RowOrder("GRSWT") = IIf(SelectedOrderCount = 1, Val(.Cells("GRSWT").Value.ToString), Val(DgvRow.Cells("GRSWT").Value.ToString))
                '            RowOrder("NETWT") = IIf(SelectedOrderCount = 1, Val(.Cells("NETWT").Value.ToString), Val(DgvRow.Cells("NETWT").Value.ToString))
                '            RowOrder("WASTAGE") = IIf(SelectedOrderCount = 1, wast, Val(DgvRow.Cells("WASTAGE").Value.ToString))
                '            RowOrder("MC") = IIf(SelectedOrderCount = 1, Val(.Cells("MC").Value.ToString), Val(DgvRow.Cells("MC").Value.ToString))
                '            RowOrder("TAGNO") = DBNull.Value
                '            RowOrder("ORSTATUS") = IIf(OMaterialType = MaterialType.Issue, "I", "R")
                '            RowOrder("CANCEL") = DBNull.Value
                '            RowOrder("COSTID") = CostCenterId
                '            RowOrder("DESCRIPT") = DBNull.Value
                '            RowOrder("ORNO") = GetCostId(CostCenterId) + GetCompanyId(strCompanyId) + DgvRow.Cells("ORNO").Value.ToString
                '            RowOrder("BATCHNO") = BatchNo
                '            RowOrder("USERID") = userId
                '            RowOrder("UPDATED") = Today.Date.ToString("yyyy-MM-dd")
                '            RowOrder("UPTIME") = Date.Now.ToLongTimeString
                '            RowOrder("APPVER") = VERSION
                '            RowOrder("COMPANYID") = strCompanyId
                '            RowOrder("TRANSFERED") = DBNull.Value
                '            RowOrder("PROID") = DBNull.Value
                '            RowOrder("ORDSTATE_ID") = objGPack.GetSqlValue("SELECT ORDSTATE_ID FROM " & cnAdminDb & "..ORDERSTATUS WHERE ORDSTATE_NAME = '" & .Cells("ORDSTATE_NAME").Value.ToString & "'", , , tran)
                '            RowOrder("ENTRYORDER") = EntryOrder
                '            RowOrder("CATCODE") = catCode
                '            DtOrIrDetail.Rows.Add(RowOrder)
                '        Next
                '        InsertData(SyncMode.Transaction, DtOrIrDetail, cn, tran, CostCenterId)
                '    End If
                'End If

                If dgvOrderDet.Rows.Count > 0 Then
                    Dim DtOrIrDetail As New DataTable
                    DtOrIrDetail = BrighttechPack.GetTableStructure(cnAdminDb, "ORIRDETAIL", cn, tran)
                    Dim EntryOrder As Integer = objGPack.GetSqlValue("SELECT ISNULL(MAX(ENTRYORDER),0) FROM " & cnAdminDb & "..ORIRDETAIL", , , tran)
                    Dim RowOrder As DataRow = Nothing
                    For Each DgvRow As DataGridViewRow In dgvOrderDet.Rows
                        If OMaterialType = MaterialType.Issue Then
                            If Val(objGPack.GetSqlValue("SELECT 1 FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORSNO='" & DgvRow.Cells("ORSNO").Value.ToString & "' AND ORSTATUS='I' ", , , tran)) > 0 And ORDER_MULTI_MIMR = False Then
                                StrSql = vbCrLf + "  UPDATE " & cnAdminDb & "..ORIRDETAIL "
                                StrSql += vbCrLf + "  SET PCS='" & DgvRow.Cells("PCS").Value.ToString & "',GRSWT='" & DgvRow.Cells("GRSWT").Value.ToString & "',NETWT='" & DgvRow.Cells("NETWT").Value.ToString & "',BATCHNO='" & BatchNo & "'"
                                StrSql += vbCrLf + "  WHERE ORSNO='" & DgvRow.Cells("ORSNO").Value.ToString & "' AND ORSTATUS='I'"
                                ExecQuery(SyncMode.Transaction, StrSql, cn, tran, CostCenterId)
                            Else
                                RowOrder = DtOrIrDetail.NewRow
                                EntryOrder += 1
                                RowOrder("SNO") = GetNewSno(TranSnoType.ORIRDETAILCODE, tran, "GET_ADMINSNO_TRAN")
                                RowOrder("ORSNO") = DgvRow.Cells("ORSNO").Value.ToString
                                RowOrder("TRANNO") = TranNo
                                RowOrder("TRANDATE") = dtpTrandate.Value.ToString("yyyy-MM-dd")
                                RowOrder("DESIGNERID") = Val(objGPack.GetSqlValue(" SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE ISNULL(ACCODE,'') = '" & _Accode & "'", , , tran))
                                'RowOrder("PCS") = IIf(SelectedOrderCount = 1, Val(.Cells("PCS").Value.ToString), Val(DgvRow.Cells("PCS").Value.ToString))
                                'RowOrder("GRSWT") = IIf(SelectedOrderCount = 1, Val(.Cells("GRSWT").Value.ToString), Val(DgvRow.Cells("GRSWT").Value.ToString))
                                'RowOrder("NETWT") = IIf(SelectedOrderCount = 1, Val(.Cells("NETWT").Value.ToString), Val(DgvRow.Cells("NETWT").Value.ToString))
                                'RowOrder("WASTAGE") = IIf(SelectedOrderCount = 1, wast, Val(DgvRow.Cells("WASTAGE").Value.ToString))
                                'RowOrder("MC") = IIf(SelectedOrderCount = 1, Val(.Cells("MC").Value.ToString), Val(DgvRow.Cells("MC").Value.ToString))

                                RowOrder("PCS") = Val(DgvRow.Cells("PCS").Value.ToString)
                                RowOrder("GRSWT") = Val(DgvRow.Cells("GRSWT").Value.ToString)
                                RowOrder("NETWT") = Val(DgvRow.Cells("NETWT").Value.ToString)
                                RowOrder("WASTAGE") = Val(DgvRow.Cells("WASTAGE").Value.ToString)
                                RowOrder("MC") = Val(DgvRow.Cells("MC").Value.ToString)

                                RowOrder("TAGNO") = DBNull.Value
                                RowOrder("ORSTATUS") = IIf(OMaterialType = MaterialType.Issue, "I", "R")
                                RowOrder("CANCEL") = DBNull.Value
                                RowOrder("COSTID") = CostCenterId
                                RowOrder("DESCRIPT") = DBNull.Value
                                'RowOrder("ORNO") = GetCostId(CostCenterId) + GetCompanyId(strCompanyId) + DgvRow.Cells("ORNO").Value.ToString
                                RowOrder("ORNO") = DgvRow.Cells("ORNO").Value.ToString
                                RowOrder("BATCHNO") = BatchNo
                                RowOrder("USERID") = userId
                                RowOrder("UPDATED") = Today.Date.ToString("yyyy-MM-dd")
                                RowOrder("UPTIME") = Date.Now.ToLongTimeString
                                RowOrder("APPVER") = VERSION
                                RowOrder("COMPANYID") = strCompanyId
                                RowOrder("TRANSFERED") = DBNull.Value
                                RowOrder("PROID") = DBNull.Value
                                'RowOrder("ORDSTATE_ID") = objGPack.GetSqlValue("SELECT ORDSTATE_ID FROM " & cnAdminDb & "..ORDERSTATUS WHERE ORDSTATE_NAME = '" & .Cells("ORDSTATE_NAME").Value.ToString & "'", , , tran)
                                RowOrder("ORDSTATE_ID") = IIf(OMaterialType = MaterialType.Issue, 2, 3)
                                RowOrder("ENTRYORDER") = EntryOrder
                                RowOrder("CATCODE") = catCode
                                RowOrder("ACCODE") = _Accode
                                DtOrIrDetail.Rows.Add(RowOrder)
                            End If
                        Else
                            RowOrder = DtOrIrDetail.NewRow
                            EntryOrder += 1
                            RowOrder("SNO") = GetNewSno(TranSnoType.ORIRDETAILCODE, tran, "GET_ADMINSNO_TRAN")
                            RowOrder("ORSNO") = DgvRow.Cells("ORSNO").Value.ToString
                            RowOrder("TRANNO") = TranNo
                            RowOrder("TRANDATE") = dtpTrandate.Value.ToString("yyyy-MM-dd")
                            RowOrder("DESIGNERID") = Val(objGPack.GetSqlValue(" SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE ISNULL(ACCODE,'') = '" & _Accode & "'", , , tran))
                            'RowOrder("PCS") = IIf(SelectedOrderCount = 1, Val(.Cells("PCS").Value.ToString), Val(DgvRow.Cells("PCS").Value.ToString))
                            'RowOrder("GRSWT") = IIf(SelectedOrderCount = 1, Val(.Cells("GRSWT").Value.ToString), Val(DgvRow.Cells("GRSWT").Value.ToString))
                            'RowOrder("NETWT") = IIf(SelectedOrderCount = 1, Val(.Cells("NETWT").Value.ToString), Val(DgvRow.Cells("NETWT").Value.ToString))
                            'RowOrder("WASTAGE") = IIf(SelectedOrderCount = 1, wast, Val(DgvRow.Cells("WASTAGE").Value.ToString))
                            'RowOrder("MC") = IIf(SelectedOrderCount = 1, Val(.Cells("MC").Value.ToString), Val(DgvRow.Cells("MC").Value.ToString))
                            RowOrder("PCS") = Val(DgvRow.Cells("PCS").Value.ToString)
                            RowOrder("GRSWT") = Val(DgvRow.Cells("GRSWT").Value.ToString)
                            RowOrder("NETWT") = Val(DgvRow.Cells("NETWT").Value.ToString)
                            RowOrder("WASTAGE") = Val(DgvRow.Cells("WASTAGE").Value.ToString)
                            RowOrder("MC") = Val(DgvRow.Cells("MC").Value.ToString)
                            RowOrder("TAGNO") = DBNull.Value
                            RowOrder("ORSTATUS") = IIf(OMaterialType = MaterialType.Issue, "I", "R")
                            RowOrder("CANCEL") = DBNull.Value
                            RowOrder("COSTID") = CostCenterId
                            RowOrder("DESCRIPT") = DBNull.Value
                            'RowOrder("ORNO") = GetCostId(CostCenterId) + GetCompanyId(strCompanyId) + DgvRow.Cells("ORNO").Value.ToString
                            RowOrder("ORNO") = DgvRow.Cells("ORNO").Value.ToString
                            RowOrder("BATCHNO") = BatchNo
                            RowOrder("USERID") = userId
                            RowOrder("UPDATED") = Today.Date.ToString("yyyy-MM-dd")
                            RowOrder("UPTIME") = Date.Now.ToLongTimeString
                            RowOrder("APPVER") = VERSION
                            RowOrder("COMPANYID") = strCompanyId
                            RowOrder("TRANSFERED") = DBNull.Value
                            RowOrder("PROID") = DBNull.Value
                            'RowOrder("ORDSTATE_ID") = objGPack.GetSqlValue("SELECT ORDSTATE_ID FROM " & cnAdminDb & "..ORDERSTATUS WHERE ORDSTATE_NAME = '" & .Cells("ORDSTATE_NAME").Value.ToString & "'", , , tran)
                            RowOrder("ORDSTATE_ID") = IIf(OMaterialType = MaterialType.Issue, 2, 3)
                            RowOrder("ENTRYORDER") = EntryOrder
                            RowOrder("CATCODE") = catCode
                            RowOrder("ACCODE") = _Accode
                            DtOrIrDetail.Rows.Add(RowOrder)
                        End If
                    Next
                    InsertData(SyncMode.Transaction, DtOrIrDetail, cn, tran, CostCenterId)
                    Dim DtOrIrDetailStone As New DataTable
                    DtOrIrDetailStone = BrighttechPack.GetTableStructure(cnAdminDb, "ORIRDETAILSTONE", cn, tran)
                    Dim RowOrderStone As DataRow = Nothing
                    For k As Integer = 0 To dtOrIrStone.Rows.Count - 1
                        Dim sno As String = Nothing
                        Dim stnItemId As Integer = 0
                        Dim stnSubItemid As Integer = 0
                        ''Find itemId
                        StrSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & dtOrIrStone.Rows(k)("ITEM").ToString & "'"
                        stnItemId = Val(objGPack.GetSqlValue(StrSql, , , tran))
                        ''Find subItemId
                        StrSql = " SELECT ISNULL(SUBITEMID,0)AS SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & dtOrIrStone.Rows(k)("SUBITEM").ToString & "' AND ITEMID = " & stnItemId & ""
                        stnSubItemid = Val(objGPack.GetSqlValue(StrSql, , , tran))
                        RowOrder = DtOrIrDetailStone.NewRow
                        RowOrder("SNO") = GetNewSno(TranSnoType.ORIRDETAILSTONECODE, tran, "GET_ADMINSNO_TRAN")
                        RowOrder("ORSNO") = DtOrIrDetail.Rows(0)("SNO").ToString
                        RowOrder("STNITEMID") = stnItemId.ToString
                        RowOrder("STNSUBITEMID") = stnSubItemid.ToString
                        RowOrder("STNPCS") = Val(dtOrIrStone.Rows(k)("PCS").ToString)
                        RowOrder("STNWT") = Val(dtOrIrStone.Rows(k)("WEIGHT").ToString)
                        RowOrder("STNRATE") = Val(dtOrIrStone.Rows(k)("RATE").ToString)
                        RowOrder("STNAMT") = Val(dtOrIrStone.Rows(k)("AMOUNT").ToString)
                        RowOrder("CUTID") = Val(dtOrIrStone.Rows(k).Item("CUTID").ToString)
                        RowOrder("COLORID") = Val(dtOrIrStone.Rows(k).Item("COLORID").ToString)
                        RowOrder("CLARITYID") = Val(dtOrIrStone.Rows(k).Item("CLARITYID").ToString)
                        RowOrder("SETTYPEID") = Val(dtOrIrStone.Rows(k).Item("SETTYPEID").ToString)
                        RowOrder("SHAPEID") = Val(dtOrIrStone.Rows(k).Item("SHAPEID").ToString)
                        RowOrder("HEIGHT") = Val(dtOrIrStone.Rows(k).Item("HEIGHT").ToString)
                        RowOrder("WIDTH") = Val(dtOrIrStone.Rows(k).Item("WIDTH").ToString)
                        RowOrder("STNUNIT") = dtOrIrStone.Rows(k)("UNIT").ToString
                        RowOrder("COSTID") = cnCostId
                        RowOrder("COMPANYID") = cnCompanyId
                        RowOrder("CALCMODE") = dtOrIrStone.Rows(k)("CALC").ToString
                        RowOrder("BATCHNO") = BatchNo
                        RowOrder("USERID") = userId
                        RowOrder("APPVER") = VERSION
                        DtOrIrDetailStone.Rows.Add(RowOrder)
                    Next
                    InsertData(SyncMode.Transaction, DtOrIrDetailStone, cn, tran, CostCenterId)
                End If
            End If
            If EditBatchno <> Nothing Then

            End If

            Dim Isedit As Boolean = False
            If EditBatchno IsNot Nothing Then
                Isedit = True
            End If

            If Lotautopost And OMaterialType = MaterialType.Receipt _
                        And cmbTransactionType.Text <> "PURCHASE[APPROVAL]" _
                        And cmbTransactionType.Text <> "APPROVAL RECEIPT" _
                        And cmbTransactionType.Text <> "RECEIPT NOTE" _
                        And (Type = "O" Or Type = "H" Or (Type = "T" And Jobisno.Trim = "")) Then
                Dim lotDiaWt As Double = 0
                With ObjMaterialDia.objStone.dtGridStone
                    For i As Integer = 0 To .Rows.Count - 1
                        StrSql = "SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME ='" & .Rows(i)("ITEM").ToString & "'"
                        If objGPack.GetSqlValue(StrSql, "METALID", "", tran) = "D" Then lotDiaWt += Val(.Rows(i)("WEIGHT").ToString)
                    Next
                End With

                If ACC_LOTAUTOPOST_TYPE <> "" Then
                    If Type <> "T" And ACC_LOTAUTOPOST_TYPE.Contains("R" & Mid(cmbTransactionType.Text, 1, 2)) Then
                        Call LOTISSUES(TranNo, issSno, _Accode, Itemid, subItemid, index, Val(.Cells("PCS").Value.ToString),
                                           Val(.Cells("Grswt").Value.ToString), Val(.Cells("Netwt").Value.ToString), Val(.Cells("TOUCH").Value.ToString),
                                           Val(.Cells("Wastage").Value.ToString), Val(.Cells("Mc").Value.ToString), Isedit, Val(.Cells("RATE").Value.ToString),
                                           lotDiaWt, _StkType, IIf(.Cells("HALLMARK").Value.ToString = "Y", "Y", "N"), IIf(Lotautopost_Narration, .Cells("REMARK1").Value.ToString, ""))
                    End If
                Else
                    Call LOTISSUES(TranNo, issSno, _Accode, Itemid, subItemid, index, Val(.Cells("PCS").Value.ToString),
                                       Val(.Cells("Grswt").Value.ToString), Val(.Cells("Netwt").Value.ToString), Val(.Cells("TOUCH").Value.ToString),
                                       Val(.Cells("Wastage").Value.ToString), Val(.Cells("Mc").Value.ToString), Isedit, Val(.Cells("RATE").Value.ToString),
                                       lotDiaWt, _StkType, IIf(.Cells("HALLMARK").Value.ToString = "Y", "Y", "N"), IIf(Lotautopost_Narration, .Cells("REMARK1").Value.ToString, ""))
                End If
            End If

            If Val(.Cells("SGST").Value.ToString) <> 0 Then
                StrSql = " INSERT INTO " & cnStockDb & "..TAXTRAN"
                StrSql += " ("
                StrSql += " SNO,ISSSNO,ACCODE,TRANNO,TRANDATE,TRANTYPE,BATCHNO,TAXID"
                StrSql += " ,AMOUNT,TAXPER,TAXAMOUNT,TAXTYPE,TSNO,COSTID,COMPANYID"
                StrSql += " )"
                StrSql += " VALUES("
                StrSql += " '" & GetNewSno(TranSnoType.TAXTRANCODE, tran) & "'" ''SNO
                StrSql += " ,'" & issSno & "'" ''ISSSNO
                StrSql += " ,''"
                StrSql += " ," & TranNo & "" 'TRANNO
                StrSql += " ,'" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                ''StrSql += " ,'" & IIf(OMaterialType = MaterialType.Issue, "I", "R") & "" & Mid(.Cells("TRANTYPE").Value.ToString, 1, 2) & "'" 'TRANTYPE [07-AUG-2021]
                StrSql += " ,'" & IIf(OMaterialType = MaterialType.Issue, "I", "R") & "" _
                        & IIf(.Cells("TRANTYPE").Value.ToString = "DELIVERY NOTE" Or .Cells("TRANTYPE").Value.ToString = "RECEIPT NOTE", "DN" _
                        , Mid(.Cells("TRANTYPE").Value.ToString, 1, 2)) & "'" 'TRANTYPE 
                StrSql += " ,'" & BatchNo & "'" 'BATCHNO
                StrSql += " ,'SG'" 'TAXID
                StrSql += " ," & Val(.Cells("GROSSAMT").Value.ToString) & "" 'AMOUNT
                StrSql += " ," & Val(.Cells("SGSTPER").Value.ToString) & "" 'TAXPER
                StrSql += " ," & Val(.Cells("SGST").Value.ToString)
                StrSql += " ,''"
                StrSql += " ,1" 'TSNO
                StrSql += " ,'" & CostCenterId & "'"
                StrSql += " ,'" & strCompanyId & "'"
                StrSql += " )"
                ExecQuery(SyncMode.Transaction, StrSql, cn, tran, CostCenterId)
            End If
            If Val(.Cells("CGST").Value.ToString) <> 0 Then
                StrSql = " INSERT INTO " & cnStockDb & "..TAXTRAN"
                StrSql += " ("
                StrSql += " SNO,ISSSNO,ACCODE,TRANNO,TRANDATE,TRANTYPE,BATCHNO,TAXID"
                StrSql += " ,AMOUNT,TAXPER,TAXAMOUNT,TAXTYPE,TSNO,COSTID,COMPANYID"
                StrSql += " )"
                StrSql += " VALUES("
                StrSql += " '" & GetNewSno(TranSnoType.TAXTRANCODE, tran) & "'" ''SNO
                StrSql += " ,'" & issSno & "'" ''ISSSNO
                StrSql += " ,''"
                StrSql += " ," & TranNo & "" 'TRANNO
                StrSql += " ,'" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                ''StrSql += " ,'" & IIf(OMaterialType = MaterialType.Issue, "I", "R") & "" & Mid(.Cells("TRANTYPE").Value.ToString, 1, 2) & "'" 'TRANTYPE [07-AUG-2021]
                StrSql += " ,'" & IIf(OMaterialType = MaterialType.Issue, "I", "R") & "" _
                        & IIf(.Cells("TRANTYPE").Value.ToString = "DELIVERY NOTE" Or .Cells("TRANTYPE").Value.ToString = "RECEIPT NOTE", "DN" _
                        , Mid(.Cells("TRANTYPE").Value.ToString, 1, 2)) & "'" 'TRANTYPE 
                StrSql += " ,'" & BatchNo & "'" 'BATCHNO
                StrSql += " ,'CG'" 'TAXID
                StrSql += " ," & Val(.Cells("GROSSAMT").Value.ToString) & "" 'AMOUNT
                StrSql += " ," & Val(.Cells("CGSTPER").Value.ToString) & "" 'TAXPER
                StrSql += " ," & Val(.Cells("CGST").Value.ToString)
                StrSql += " ,''"
                StrSql += " ,2" 'TSNO
                StrSql += " ,'" & CostCenterId & "'"
                StrSql += " ,'" & strCompanyId & "'"
                StrSql += " )"
                ExecQuery(SyncMode.Transaction, StrSql, cn, tran, CostCenterId)
            End If
            If Val(.Cells("IGST").Value.ToString) <> 0 Then
                StrSql = " INSERT INTO " & cnStockDb & "..TAXTRAN"
                StrSql += " ("
                StrSql += " SNO,ISSSNO,ACCODE,TRANNO,TRANDATE,TRANTYPE,BATCHNO,TAXID"
                StrSql += " ,AMOUNT,TAXPER,TAXAMOUNT,TAXTYPE,TSNO,COSTID,COMPANYID"
                StrSql += " )"
                StrSql += " VALUES("
                StrSql += " '" & GetNewSno(TranSnoType.TAXTRANCODE, tran) & "'" ''SNO
                StrSql += " ,'" & issSno & "'" ''ISSSNO
                StrSql += " ,''"
                StrSql += " ," & TranNo & "" 'TRANNO
                StrSql += " ,'" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                ''StrSql += " ,'" & IIf(OMaterialType = MaterialType.Issue, "I", "R") & "" & Mid(.Cells("TRANTYPE").Value.ToString, 1, 2) & "'" 'TRANTYPE [07-AUG-2021]
                StrSql += " ,'" & IIf(OMaterialType = MaterialType.Issue, "I", "R") & "" _
                        & IIf(.Cells("TRANTYPE").Value.ToString = "DELIVERY NOTE" Or .Cells("TRANTYPE").Value.ToString = "RECEIPT NOTE", "DN" _
                        , Mid(.Cells("TRANTYPE").Value.ToString, 1, 2)) & "'" 'TRANTYPE 
                StrSql += " ,'" & BatchNo & "'" 'BATCHNO
                StrSql += " ,'IG'" 'TAXID
                StrSql += " ," & Val(.Cells("GROSSAMT").Value.ToString) & "" 'AMOUNT
                StrSql += " ," & Val(.Cells("IGSTPER").Value.ToString) & "" 'TAXPER
                StrSql += " ," & Val(.Cells("IGST").Value.ToString)
                StrSql += " ,''"
                StrSql += " ,3" 'TSNO
                StrSql += " ,'" & CostCenterId & "'"
                StrSql += " ,'" & strCompanyId & "'"
                StrSql += " )"
                ExecQuery(SyncMode.Transaction, StrSql, cn, tran, CostCenterId)
            End If
            If Val(.Cells("TCS").Value.ToString) <> 0 Then
                StrSql = " INSERT INTO " & cnStockDb & "..TAXTRAN"
                StrSql += " ("
                StrSql += " SNO,ISSSNO,ACCODE,TRANNO,TRANDATE,TRANTYPE,BATCHNO,TAXID"
                StrSql += " ,AMOUNT,TAXPER,TAXAMOUNT,TAXTYPE,TSNO,COSTID,COMPANYID"
                StrSql += " )"
                StrSql += " VALUES("
                StrSql += " '" & GetNewSno(TranSnoType.TAXTRANCODE, tran) & "'" ''SNO
                StrSql += " ,'" & issSno & "'" ''ISSSNO
                StrSql += " ,''"
                StrSql += " ," & TranNo & "" 'TRANNO
                StrSql += " ,'" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                ''StrSql += " ,'" & IIf(OMaterialType = MaterialType.Issue, "I", "R") & "" & Mid(.Cells("TRANTYPE").Value.ToString, 1, 2) & "'" 'TRANTYPE '' [07-AUG-2021]
                StrSql += " ,'" & IIf(OMaterialType = MaterialType.Issue, "I", "R") & "" _
                        & IIf(.Cells("TRANTYPE").Value.ToString = "DELIVERY NOTE" Or .Cells("TRANTYPE").Value.ToString = "RECEIPT NOTE", "DN" _
                        , Mid(.Cells("TRANTYPE").Value.ToString, 1, 2)) & "'" 'TRANTYPE 
                StrSql += " ,'" & BatchNo & "'" 'BATCHNO
                StrSql += " ,'TC'" 'TAXID
                StrSql += " ," & Val(.Cells("GROSSAMT").Value.ToString) & "" 'AMOUNT
                StrSql += " ," & Val("") & "" 'TAXPER
                StrSql += " ," & Val(.Cells("TCS").Value.ToString)
                StrSql += " ,''"
                StrSql += " ,4" 'TSNO
                StrSql += " ,'" & CostCenterId & "'"
                StrSql += " ,'" & strCompanyId & "'"
                StrSql += " )"
                ExecQuery(SyncMode.Transaction, StrSql, cn, tran, CostCenterId)
            End If
        End With
    End Sub
    Function funcCalcOPureWt(ByVal GrsNet As String, ByVal Grswt As Decimal, ByVal Netwt As Decimal, ByVal Wastage As Decimal, ByVal Touch As Decimal, ByVal Purity As Decimal, ByVal Metal As String) As Decimal
        Dim pureWt As Decimal = Nothing
        Dim ROUNDOFF_WT() As String = GetAdmindbSoftValue("ROUNDOFF-MRMI", "0,0,0,0", tran).Split(",")
        Dim PURWTPERACC As String = GetAdmindbSoftValue("PURWTPER-ACC", "100", tran)
        If Val(PURWTPERACC.ToString) = 0 Then PURWTPERACC = 100
        If Val(Touch) <> 0 Then
            pureWt = (IIf(GrsNet = "G", Val(Grswt), Val(Netwt)) + Val(Wastage)) * (Val(Touch) / PURWTPERACC)
        Else
            pureWt = (IIf(GrsNet = "G", Val(Grswt), Val(Netwt)) + Val(Wastage)) * (Val(Purity) / PURWTPERACC)
        End If
        Dim Round_G As Integer = ROUNDOFF_WT(0)
        Dim Round_D As Integer = ROUNDOFF_WT(1)
        Dim Round_S As Integer = ROUNDOFF_WT(2)
        Dim Round_P As Integer = ROUNDOFF_WT(3)
        If Metal = "GOLD" Then
            Return (IIf(pureWt <> 0, Format(Math.Round(pureWt, Round_G), "0.000"), Nothing))
        ElseIf Metal = "DIAMOND" Then
            Return (IIf(pureWt <> 0, Format(Math.Round(pureWt, Round_D), "0.000"), Nothing))
        ElseIf Metal = "SILVER" Then
            Return (IIf(pureWt <> 0, Format(Math.Round(pureWt, Round_S), "0.000"), Nothing))
        ElseIf Metal = "PLATINUM" Then
            Return (IIf(pureWt <> 0, Format(Math.Round(pureWt, Round_P), "0.000"), Nothing))
        Else
            Return (IIf(pureWt <> 0, Format(pureWt, "0.000"), Nothing))
        End If
        Return (IIf(pureWt <> 0, Format(Math.Round(pureWt, Round_G), "0.000"), Nothing))
    End Function
    Private Sub LOTISSUES(ByVal Tranno As Integer, ByVal Recsno As String _
    , ByVal accode As String, ByVal itemid As Integer, ByVal subitemid As Integer _
    , ByVal cnt As Integer, ByVal Pcs As Integer, ByVal Grswt As Decimal _
    , ByVal Netwt As Decimal, ByVal Touch As Decimal, ByVal Wastage As Decimal _
    , ByVal Mcharge As Decimal, ByVal Isediting As Boolean, ByVal FineRate As Decimal, ByVal DiaWt As Decimal, ByVal StkType As String _
    , ByVal HALLMARK As String, ByVal _LotNarration As String)
        Dim DTtemp As New DataTable
        Dim lotSno As String
        Dim oldlotSno As String
        Dim Cgrswt As Decimal
        Dim Cpcs As Integer
        If Isediting = True And MREDIT_WITHLOT = False Then Exit Sub
        If Isediting = True And MREDIT_WITHLOT Then
            lotSno = objGPack.GetSqlValue("SELECT LOTSNO from " & cnStockDb & "..LOTISSUE WHERE RECSNO = '" & Recsno & "'", , , tran)
            oldlotSno = objGPack.GetSqlValue("SELECT LOTSNO from " & cnStockDb & "..LOTISSUE WHERE RECSNO = '" & Recsno & "'", , , tran)
            lotNo = Val(objGPack.GetSqlValue("SELECT LOTNO from " & cnAdminDb & "..ITEMLOT WHERE SNO = '" & lotSno & "'", , , tran))
            Cgrswt = Val(objGPack.GetSqlValue("SELECT SUM(CGRSWT) CGRSWT FROM " & cnAdminDb & "..ITEMLOT WHERE LOTNO = '" & lotNo & "'", , , tran))
            Cpcs = Val(objGPack.GetSqlValue("SELECT SUM(CPCS)CPCS FROM " & cnAdminDb & "..ITEMLOT WHERE LOTNO = '" & lotNo & "'", , , tran))
            If Cgrswt > Grswt Or Cpcs > Pcs Then Exit Sub
            StrSql = " DELETE FROM " & cnAdminDb & "..ITEMLOT "
            StrSql += " WHERE SNO = '" & lotSno & "'"
            ExecQuery(SyncMode.Stock, StrSql, cn, tran, CostCenterId)
            StrSql = " DELETE FROM " & cnStockDb & "..LOTISSUE "
            StrSql += " WHERE LOTSNO = '" & lotSno & "'"
            ExecQuery(SyncMode.Stock, StrSql, cn, tran, CostCenterId)
        End If
        If itemid = 0 Then Exit Sub
        If lotNo = 0 Or MultiLot Then
GENLOTNO:
            StrSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'LOTNO'"
            lotNo = Val(objGPack.GetSqlValue(StrSql, , , tran))
            StrSql = " UPDATE " & cnAdminDb & "..SOFTCONTROL SET CTLTEXT = '" & lotNo + 1 & "' "
            StrSql += " WHERE CTLID = 'LOTNO' AND CTLTEXT = " & lotNo & ""
            Cmd = New OleDbCommand(StrSql, cn, tran)
            If Cmd.ExecuteNonQuery() = 0 Then
                GoTo GENLOTNO
            End If
            lotNo += 1
        End If
        lotSno = GetNewSno(TranSnoType.ITEMLOTCODE, tran, "GET_ADMINSNO_TRAN") '  GetWSno(TranSnoType.ITEMLOTCODE, Tran, CnStockdb)
        StrSql = "select STOCKTYPE,isnull(NOOFPIECE,0) noofpiece,isnull(DEFAULTCOUNTER,0) DEFCOUNTER,VALUEADDEDTYPE from " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & itemid
        Dim dritem As DataRow = GetSqlRow(StrSql, cn, tran)

        Dim stocktype As String = dritem.Item(0).ToString
        Dim noOfTag As Integer = Val(dritem.Item(1).ToString)
        Dim itemCounterId As String = dritem.Item(2).ToString
        Dim VALUEADDEDTYPE As String = dritem.Item(3).ToString
        Dim DesignerId As Integer = 0

        StrSql = "select top 1 designerid from " & cnAdminDb & "..DESIGNER WHERE ACCODE = '" & accode & "'"
        Dim dridesg As DataRow = GetSqlRow(StrSql, cn, tran)
        If Not (dridesg Is Nothing) Then DesignerId = Val(dridesg.Item(0).ToString)
        Dim ordrepno As String = ""
        Dim entryType As String = "R"
        If chkOrder.Checked Then entryType = "OR"
        If _JobNoEnable = False Then
            ''ORSTATUS
            If DgvTran.Rows(cnt).Cells("JOBNO").Value.ToString <> "" And Mid(DgvTran.Rows(cnt).Cells("JOBNO").Value.ToString, 1, 1) = "R" Then entryType = "RE"
            If DgvTran.Rows(cnt).Cells("JOBNO").Value.ToString <> "" And Mid(DgvTran.Rows(cnt).Cells("JOBNO").Value.ToString, 1, 1) = "O" Then entryType = "OR"
            ordrepno = DgvTran.Rows(cnt).Cells("JOBNO").Value.ToString
        End If
        'if 
        Dim mwastper As Decimal = 0
        If Wastage <> 0 Then mwastper = Math.Round(((Wastage * 100) / Grswt), 2)
        Dim mmcgrm As Decimal = 0
        If Mcharge <> 0 Then mmcgrm = Math.Round((Mcharge / Grswt), 2)

        StrSql = " INSERT INTO " & cnAdminDb & "..ITEMLOT "
        StrSql += " ("
        StrSql += " SNO,ENTRYTYPE,LOTDATE,LOTNO,DESIGNERID,TRANINVNO,"
        StrSql += " BILLNO,COSTID,ENTRYORDER,ORDREPNO,ORDENTRYORDER,"
        StrSql += " ITEMID,SUBITEMID,PCS,GRSWT,STNPCS,STNWT,STNUNIT,"
        StrSql += " DIAPCS,DIAWT,NETWT,NOOFTAG,RATE,ITEMCTRID,WMCTYPE,"
        StrSql += " BULKLOT,MULTIPLETAGS,NARRATION,FINERATE,TUCH,"
        StrSql += " WASTPER,MCGRM,OTHCHARGE,STARTTAGNO,ENDTAGNO,CURTAGNO,"
        StrSql += " COMPANYID,CPCS,CGRSWT,COMPLETED,CANCEL,"
        StrSql += " ACCESSING,USERID,UPDATED,"
        StrSql += " UPTIME,SYSTEMID,APPVER,ITEMTYPEID,OPENTIME,STKTYPE,HALLMARK)VALUES("
        StrSql += " '" & lotSno & "'" 'SNO
        StrSql += " ,'" & entryType & "'" 'ENTRYTYPE
        StrSql += " ,'" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "'" 'LOTDATE 'DateTime.Parse(RoIssue(CNT).ITEM("LOTDATE"), ukCultureInfo.DateTimeFormat).ToString("yyyy-MM-dd")
        StrSql += " ," & lotNo & "" 'LOTNO
        StrSql += " ," & DesignerId & "" 'DESIGNERID
        StrSql += " ,''" 'TRANINVNO
        StrSql += " ,''" 'BILLNO
        StrSql += " ,'" & CostCenterId & "'" 'COSTID
        StrSql += " ," & cnt + 1 & "" 'ENTRYORDER
        StrSql += " ,'" & ordrepno & "'" 'ORDREPNO
        StrSql += " ,0" 'ORDENTRYORDER
        StrSql += " ," & Val(itemid) & "" 'ITEMID
        StrSql += " ," & Val(subitemid) & "" 'SUBITEMID
        StrSql += " ," & Pcs & "" 'PCS
        StrSql += " ," & Grswt & "" 'GRSWT
        StrSql += " ,0" 'STNPCS
        StrSql += " ,0" 'STNWT
        StrSql += " ,'G'" 'STNUNIT
        StrSql += " ,0" 'DIAPCS
        StrSql += " ," & DiaWt & "" 'DIAWT
        StrSql += " ," & Netwt & "" 'NETWT
        StrSql += " ," & IIf(noOfTag = 0, 1, noOfTag) & "" 'NOOFTAG
        StrSql += " ,0" 'RATE
        StrSql += " ," & Val(itemCounterId) & "" 'ITEMCTRID
        StrSql += " ,'" & VALUEADDEDTYPE & "'" 'WMCTYPE
        StrSql += " ,'" & IIf(chkBulk.Checked, "Y", "N") & "'" 'BULKLOT
        StrSql += " ,'" & IIf(chkMulti.Checked, "Y", "N") & "'" 'MULTIPLETAGS
        StrSql += " ,'" & _LotNarration.ToString & "'" 'NARRATION 
        StrSql += " ," & FineRate & "" 'FINERATE
        StrSql += " ," & Touch & "" 'TUCH
        StrSql += " ," & mwastper & "" 'WASTPER
        StrSql += " ," & mmcgrm & "" 'MCGRM
        StrSql += " ,0" 'OTHCHARGE
        StrSql += " ,''" 'STARTTAGNO
        StrSql += " ,''" 'ENDTAGNO
        StrSql += " ,''" 'CURTAGNO
        StrSql += " ,'" & GetStockCompId() & "'" 'sCOMPANYID
        If Isediting = True And MREDIT_WITHLOT Then
            StrSql += " ," & Cpcs  'CPIECE
            StrSql += " ," & Cgrswt 'CWEIGHT
        Else
            StrSql += " ,0" 'CPIECE
            StrSql += " ,0" 'CWEIGHT
        End If
        StrSql += " ,''" 'COMPLETED
        StrSql += " ,''" 'CANCEL
        StrSql += " ,''" 'ACCESSING
        StrSql += " ," & userId & "" 'USERID
        StrSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        StrSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
        StrSql += " ,'" & systemId & "'" 'SYSTEMID
        StrSql += " ,'" & VERSION & "'" 'APPVER
        StrSql += " ,0" 'ITEMTYPEID"
        StrSql += " ,GETDATE()" 'OPENTIME
        StrSql += " ,'" & StkType & "'" 'APPVER
        StrSql += " ,'" & HALLMARK.ToString & "'" 'HALLMARK
        StrSql += " )"
        ExecQuery(SyncMode.Stock, StrSql, cn, tran, CostCenterId)

        StrSql = "  INSERT INTO " & cnStockDb & "..LOTISSUE"
        StrSql += "  ("
        StrSql += "  TRANNO"
        StrSql += "  ,TRANDATE"
        StrSql += "  ,GRSWT"
        StrSql += "  ,NETWT"
        StrSql += "  ,CANCEL"
        StrSql += "  ,BATCHNO"
        StrSql += "  ,USERID"
        StrSql += "  ,UPDATED"
        StrSql += "  ,APPVER"
        StrSql += "  ,COMPANYID"
        StrSql += "  ,PCS"
        StrSql += "  ,LOTSNO"
        StrSql += "  ,ITEMID"
        StrSql += "  ,SUBITEMID"
        StrSql += "  ,RECSNO"
        StrSql += "  ,STKTYPE"
        StrSql += "  )"
        StrSql += "  SELECT"
        StrSql += "  " & Tranno & ""
        StrSql += "  ,'" & dtpTrandate.Value & "'"
        StrSql += "  ," & Grswt & "" 'GRSWT
        StrSql += "  ," & Netwt & "" 'NETWT
        StrSql += "  ,''" 'CANCEL
        StrSql += "  ,''" 'BATCHNO
        StrSql += "  ," & userId & "" 'USERID
        StrSql += "  ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        StrSql += "  ,'" & VERSION & "'" 'APPVER
        StrSql += "  ,'" & GetStockCompId() & "'" 'COMPANYID
        StrSql += "  ," & Pcs & "" 'PCS
        StrSql += "  ,'" & lotSno & "'" 'LOTSNO
        StrSql += "  ," & itemid & "" 'ITEMID
        StrSql += "  ," & subitemid & "" 'SUBITEMID
        StrSql += "  ,'" & Recsno & "'" 'SNO
        StrSql += "  ,'" & StkType & "'" 'SNO
        ExecQuery(SyncMode.Stock, StrSql, cn, tran, CostCenterId)
        If Isediting = True And MREDIT_WITHLOT Then
            StrSql = "  UPDATE " & cnAdminDb & "..ITEMTAG  SET LOTSNO = '" & lotSno & "'"
            StrSql += "  WHERE LOTSNO = '" & oldlotSno & "'"
            ExecQuery(SyncMode.Stock, StrSql, cn, tran, CostCenterId)
        End If

    End Sub
    Private Sub LOTISSUES_Old(ByVal Tranno As Integer, ByVal Recsno As String _
    , ByVal accode As String, ByVal itemid As Integer, ByVal subitemid As Integer _
    , ByVal cnt As Integer, ByVal Pcs As Integer, ByVal Grswt As Decimal _
    , ByVal Netwt As Decimal, ByVal Touch As Decimal, ByVal Wastage As Decimal _
    , ByVal Mcharge As Decimal, ByVal Isediting As Boolean, ByVal DiaWt As Decimal, ByVal StkType As String)
        Dim lotSno As String
        If Isediting = False Then
            If itemid = 0 Then Exit Sub
            If lotNo = 0 Or MultiLot Then
GENLOTNO:
                StrSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'LOTNO'"
                lotNo = Val(objGPack.GetSqlValue(StrSql, , , tran))
                StrSql = " UPDATE " & cnAdminDb & "..SOFTCONTROL SET CTLTEXT = '" & lotNo + 1 & "' "
                StrSql += " WHERE CTLID = 'LOTNO' AND CTLTEXT = " & lotNo & ""
                Cmd = New OleDbCommand(StrSql, cn, tran)
                If Cmd.ExecuteNonQuery() = 0 Then
                    GoTo GENLOTNO
                End If
                lotNo += 1
            End If
            lotSno = GetNewSno(TranSnoType.ITEMLOTCODE, tran, "GET_ADMINSNO_TRAN") '  GetWSno(TranSnoType.ITEMLOTCODE, Tran, CnStockdb)
            StrSql = "select STOCKTYPE,isnull(NOOFPIECE,0) noofpiece,isnull(DEFAULTCOUNTER,0) DEFCOUNTER,VALUEADDEDTYPE from " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & itemid
            Dim dritem As DataRow = GetSqlRow(StrSql, cn, tran)

            Dim stocktype As String = dritem.Item(0).ToString
            Dim noOfTag As Integer = Val(dritem.Item(1).ToString)
            Dim itemCounterId As String = dritem.Item(2).ToString
            Dim VALUEADDEDTYPE As String = dritem.Item(3).ToString
            Dim DesignerId As Integer = 0

            StrSql = "select top 1 designerid from " & cnAdminDb & "..DESIGNER WHERE ACCODE = '" & accode & "'"
            Dim dridesg As DataRow = GetSqlRow(StrSql, cn, tran)
            If Not (dridesg Is Nothing) Then DesignerId = Val(dridesg.Item(0).ToString)
            Dim ordrepno As String = ""
            Dim entryType As String = "R"
            If chkOrder.Checked Then entryType = "OR"
            If _JobNoEnable = False Then
                ''ORSTATUS
                If DgvTran.Rows(cnt).Cells("JOBNO").Value.ToString <> "" And Mid(DgvTran.Rows(cnt).Cells("JOBNO").Value.ToString, 1, 1) = "R" Then entryType = "RE"
                If DgvTran.Rows(cnt).Cells("JOBNO").Value.ToString <> "" And Mid(DgvTran.Rows(cnt).Cells("JOBNO").Value.ToString, 1, 1) = "O" Then entryType = "OR"
                ordrepno = DgvTran.Rows(cnt).Cells("JOBNO").Value.ToString
            End If
            'if 
            Dim mwastper As Decimal = 0
            If Wastage <> 0 Then mwastper = Math.Round(((Wastage * 100) / Grswt), 2)
            Dim mmcgrm As Decimal = 0
            If Mcharge <> 0 Then mmcgrm = Math.Round((Mcharge / Grswt), 2)

            StrSql = " INSERT INTO " & cnAdminDb & "..ITEMLOT "
            StrSql += " ("
            StrSql += " SNO,ENTRYTYPE,LOTDATE,LOTNO,DESIGNERID,TRANINVNO,"
            StrSql += " BILLNO,COSTID,ENTRYORDER,ORDREPNO,ORDENTRYORDER,"
            StrSql += " ITEMID,SUBITEMID,PCS,GRSWT,STNPCS,STNWT,STNUNIT,"
            StrSql += " DIAPCS,DIAWT,NETWT,NOOFTAG,RATE,ITEMCTRID,WMCTYPE,"
            StrSql += " BULKLOT,MULTIPLETAGS,NARRATION,FINERATE,TUCH,"
            StrSql += " WASTPER,MCGRM,OTHCHARGE,STARTTAGNO,ENDTAGNO,CURTAGNO,"
            StrSql += " COMPANYID,CPCS,CGRSWT,COMPLETED,CANCEL,"
            StrSql += " ACCESSING,USERID,UPDATED,"
            StrSql += " UPTIME,SYSTEMID,APPVER,ITEMTYPEID,OPENTIME,STKTYPE)VALUES("
            StrSql += " '" & lotSno & "'" 'SNO
            StrSql += " ,'" & entryType & "'" 'ENTRYTYPE
            StrSql += " ,'" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "'" 'LOTDATE 'DateTime.Parse(RoIssue(CNT).ITEM("LOTDATE"), ukCultureInfo.DateTimeFormat).ToString("yyyy-MM-dd")
            StrSql += " ," & lotNo & "" 'LOTNO
            StrSql += " ," & DesignerId & "" 'DESIGNERID
            StrSql += " ,''" 'TRANINVNO
            StrSql += " ,''" 'BILLNO
            StrSql += " ,'" & CostCenterId & "'" 'COSTID
            StrSql += " ," & cnt + 1 & "" 'ENTRYORDER
            StrSql += " ,'" & ordrepno & "'" 'ORDREPNO
            StrSql += " ,0" 'ORDENTRYORDER
            StrSql += " ," & Val(itemid) & "" 'ITEMID
            StrSql += " ," & Val(subitemid) & "" 'SUBITEMID
            StrSql += " ," & Pcs & "" 'PCS
            StrSql += " ," & Grswt & "" 'GRSWT
            StrSql += " ,0" 'STNPCS
            StrSql += " ,0" 'STNWT
            StrSql += " ,'G'" 'STNUNIT
            StrSql += " ,0" 'DIAPCS
            StrSql += " ," & DiaWt & "" 'DIAWT
            StrSql += " ," & Netwt & "" 'NETWT
            StrSql += " ," & IIf(noOfTag = 0, 1, noOfTag) & "" 'NOOFTAG
            StrSql += " ,0" 'RATE
            StrSql += " ," & Val(itemCounterId) & "" 'ITEMCTRID
            StrSql += " ,'" & VALUEADDEDTYPE & "'" 'WMCTYPE
            StrSql += " ,'" & IIf(chkBulk.Checked, "Y", "N") & "'" 'BULKLOT
            StrSql += " ,'" & IIf(chkMulti.Checked, "Y", "N") & "'" 'MULTIPLETAGS
            StrSql += " ,''" 'NARRATION
            StrSql += " ,0" 'FINERATE
            StrSql += " ," & Touch & "" 'TUCH
            StrSql += " ," & mwastper & "" 'WASTPER
            StrSql += " ," & mmcgrm & "" 'MCGRM
            StrSql += " ,0" 'OTHCHARGE
            StrSql += " ,''" 'STARTTAGNO
            StrSql += " ,''" 'ENDTAGNO
            StrSql += " ,''" 'CURTAGNO
            StrSql += " ,'" & GetStockCompId() & "'" 'sCOMPANYID
            StrSql += " ,0" 'CPIECE
            StrSql += " ,0" 'CWEIGHT
            StrSql += " ,''" 'COMPLETED
            StrSql += " ,''" 'CANCEL
            StrSql += " ,''" 'ACCESSING
            StrSql += " ," & userId & "" 'USERID
            StrSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
            StrSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
            StrSql += " ,'" & systemId & "'" 'SYSTEMID
            StrSql += " ,'" & VERSION & "'" 'APPVER
            StrSql += " ,0" 'ITEMTYPEID"
            StrSql += " ,GETDATE()" 'OPENTIME
            StrSql += " ,'" & StkType & "'" 'APPVER
            StrSql += " )"
            ExecQuery(SyncMode.Stock, StrSql, cn, tran, CostCenterId)

            StrSql = "  INSERT INTO " & cnStockDb & "..LOTISSUE"
            StrSql += "  ("
            StrSql += "  TRANNO"
            StrSql += "  ,TRANDATE"
            StrSql += "  ,GRSWT"
            StrSql += "  ,NETWT"
            StrSql += "  ,CANCEL"
            StrSql += "  ,BATCHNO"
            StrSql += "  ,USERID"
            StrSql += "  ,UPDATED"
            StrSql += "  ,APPVER"
            StrSql += "  ,COMPANYID"
            StrSql += "  ,PCS"
            StrSql += "  ,LOTSNO"
            StrSql += "  ,ITEMID"
            StrSql += "  ,SUBITEMID"
            StrSql += "  ,RECSNO"
            StrSql += "  ,STKTYPE"
            StrSql += "  )"
            StrSql += "  SELECT"
            StrSql += "  " & Tranno & ""
            StrSql += "  ,'" & dtpTrandate.Value & "'"
            StrSql += "  ," & Grswt & "" 'GRSWT
            StrSql += "  ," & Netwt & "" 'NETWT
            StrSql += "  ,''" 'CANCEL
            StrSql += "  ,''" 'BATCHNO
            StrSql += "  ," & userId & "" 'USERID
            StrSql += "  ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
            StrSql += "  ,'" & VERSION & "'" 'APPVER
            StrSql += "  ,'" & GetStockCompId() & "'" 'COMPANYID
            StrSql += "  ," & Pcs & "" 'PCS
            StrSql += "  ,'" & lotSno & "'" 'LOTSNO
            StrSql += "  ," & itemid & "" 'ITEMID
            StrSql += "  ," & subitemid & "" 'SUBITEMID
            StrSql += "  ,'" & Recsno & "'" 'SNO
            StrSql += "  ,'" & StkType & "'" 'SNO
            ExecQuery(SyncMode.Stock, StrSql, cn, tran, CostCenterId)
        Else
            lotSno = objGPack.GetSqlValue("select LOTSNO from " & cnStockDb & "..lotissue WHERE RECSNO = '" & Recsno & "'", , , tran)
            StrSql = " update " & cnAdminDb & "..ITEMLOT "
            StrSql += " set PCS=" & Pcs & ",GRSWT=" & Grswt & " WHERE SNO = '" & lotSno & "'"
            ExecQuery(SyncMode.Stock, StrSql, cn, tran, CostCenterId)
            StrSql = "  UPDATE " & cnStockDb & "..LOTISSUE SET PCS = " & Pcs & ",GRSWT = " & Grswt & "NETWT = " & Netwt
            StrSql += "  WHERE LOTSNO = '" & lotSno & "'"
            ExecQuery(SyncMode.Stock, StrSql, cn, tran, CostCenterId)
        End If
    End Sub

    Private Sub Save_JobNo(ByVal index As Integer)
        With DgvTran.Rows(index)
            Dim Obj As MaterialIssRec
            Obj = CType(.Cells("METISSREC").Value, MaterialIssRec)
            Dim OrdStateId As Integer = 0
            Dim Tax As Decimal = Val(.Cells("VAT").Value.ToString)
            Dim Tds As Decimal = Val(.Cells("VAT").Value.ToString)
            Dim Type As String = .Cells("TYPE").Value.ToString ' wheather it is ornament,metal,stone,others
            Dim _StkType As String = ""
            If .Cells("RESNO").Value.ToString <> "" And OMaterialType = MaterialType.Issue Then
                StrSql = " SELECT TOP 1 STKTYPE FROM " & cnStockDb & "..RECEIPT WHERE SNO='" & .Cells("RESNO").Value.ToString & "' "
                _StkType = objGPack.GetSqlValue(StrSql, , "", tran)
            Else
                If IIf(OMaterialType = MaterialType.Issue, "I", "R") & "" & Mid(.Cells("TRANTYPE").Value.ToString, 1, 2) = "RRE" Then
                    _StkType = "M"
                ElseIf IIf(OMaterialType = MaterialType.Issue, "I", "R") & "" & Mid(.Cells("TRANTYPE").Value.ToString, 1, 2) = "RPU" Then
                    _StkType = "T"
                End If
            End If
            Select Case Type
                Case "O" 'Ornament
                    OrdStateId = Val(objGPack.GetSqlValue("SELECT ORDSTATE_ID FROM " & cnAdminDb & "..ORDERSTATUS WHERE ORDSTATE_NAME = '" & Obj.cmbOProcess.Text & "'", , , tran))
                    If Obj.lblOVat.Text = "Tds" Then Tax = 0 Else Tds = 0
                    If TdsPer = Nothing Then TdsPer = Val(Obj.txtOVatPer_PER.Text)
                Case "M" 'Metal
                    OrdStateId = Val(objGPack.GetSqlValue("SELECT ORDSTATE_ID FROM " & cnAdminDb & "..ORDERSTATUS WHERE ORDSTATE_NAME = '" & Obj.cmbMProcess.Text & "'", , , tran))
                    If Obj.lblMVat.Text = "Tds" Then Tax = 0 Else Tds = 0
                    If TdsPer = Nothing Then TdsPer = Val(Obj.txtMVatPer_PER.Text)
                Case "T" 'Stone
                    OrdStateId = Val(objGPack.GetSqlValue("SELECT ORDSTATE_ID FROM " & cnAdminDb & "..ORDERSTATUS WHERE ORDSTATE_NAME = '" & Obj.cmbSProcess.Text & "'", , , tran))
                    If Obj.lblSVat.Text = "Tds" Then Tax = 0 Else Tds = 0
                    If TdsPer = Nothing Then TdsPer = Val(Obj.txtSVatPer_PER.Text)
                Case "H" 'Others
                    If Obj.lblOthVat.Text = "Tds" Then Tax = 0 Else Tds = 0
                    If TdsPer = Nothing Then TdsPer = Val(Obj.txtOthVatPer_PER.Text)
            End Select

            Dim itemTypeId As Integer = 0
            Dim issSno As String = Nothing

            For Each stRow As DataRow In CType(.Cells("METISSREC").Value, MaterialIssRec).MaterialStoneDia.dtGridStone.Rows
                If OMaterialType = MaterialType.Issue Then
                    issSno = GetNewSno(IIf(_AccAudit, TranSnoType.TISSUECODE, TranSnoType.ISSUECODE), tran)
                Else
                    issSno = GetNewSno(IIf(_AccAudit, TranSnoType.TRECEIPTCODE, TranSnoType.RECEIPTCODE), tran)
                End If
                If OMaterialType = MaterialType.Issue Then
                    StrSql = " INSERT INTO " & cnStockDb & ".." & IIf(_AccAudit, "TISSUE", "ISSUE")
                Else
                    StrSql = " INSERT INTO " & cnStockDb & ".." & IIf(_AccAudit, "TRECEIPT", "RECEIPT")
                End If

                StrSql += " ("
                StrSql += "  SNO,TRANNO,TRANDATE,TRANTYPE,STKTYPE,PCS"
                StrSql += " ,GRSWT,NETWT,LESSWT,PUREWT"
                StrSql += " ,TAGNO,ITEMID,SUBITEMID,WASTPER"
                StrSql += " ,WASTAGE,MCGRM,MCHARGE,AMOUNT"
                StrSql += " ,RATE,BOARDRATE,SALEMODE,GRSNET"
                StrSql += " ,TRANSTATUS,REFNO,REFDATE,COSTID"
                StrSql += " ,COMPANYID,FLAG,EMPID,TAGGRSWT"
                StrSql += " ,TAGNETWT,TAGRATEID,TAGSVALUE,TAGDESIGNER"
                StrSql += " ,ITEMCTRID,ITEMTYPEID,PURITY,TABLECODE"
                StrSql += " ,INCENTIVE,WEIGHTUNIT,CATCODE,OCATCODE"
                StrSql += " ,ACCODE,ALLOY,BATCHNO,REMARK1"
                StrSql += " ,REMARK2,USERID,UPDATED,UPTIME,SYSTEMID,DISCOUNT,RUNNO,CASHID,TAX,TDS"
                StrSql += " ,STNAMT,MISCAMT,METALID,STONEUNIT,APPVER,TOUCH,ORDSTATE_ID"
                StrSql += " )"
                StrSql += " VALUES("
                StrSql += " '" & issSno & "'" ''SNO
                StrSql += " ," & TranNo & "" 'TRANNO
                StrSql += " ,'" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                ''StrSql += " ,'" & IIf(OMaterialType = MaterialType.Issue, "I", "R") & "" & Mid(.Cells("TRANTYPE").Value.ToString, 1, 2) & "'" 'TRANTYPE [07-AUG-2021]
                StrSql += " ,'" & IIf(OMaterialType = MaterialType.Issue, "I", "R") & "" _
                    & IIf(.Cells("TRANTYPE").Value.ToString = "DELIVERY NOTE" Or .Cells("TRANTYPE").Value.ToString = "RECEIPT NOTE", "DN" _
                    , Mid(.Cells("TRANTYPE").Value.ToString, 1, 2)) & "'" 'TRANTYPE 
                StrSql += " '" & _StkType & "'" ''STKTYPE
                StrSql += " ," & Val(stRow.Item("PCS").ToString) & "" 'PCS
                StrSql += " ," & Val(stRow.Item("WEIGHT").ToString) & "" 'GRSWT
                StrSql += " ," & Val(stRow.Item("WEIGHT").ToString) & "" 'NETWT
                StrSql += " ," & Val(0) & "" 'LESSWT
                StrSql += " ," & Val(stRow.Item("WEIGHT").ToString) & "" 'LESSWT
                StrSql += " ,''" 'TAGNO
                StrSql += " ," & Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & stRow.Item("ITEM").ToString & "'", , , tran)) & "" 'ITEMID
                StrSql += " ," & Val(objGPack.GetSqlValue("SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & stRow.Item("SUBITEM").ToString & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & stRow.Item("ITEM").ToString & "')", , , tran)) & "" 'SUBITEMID
                StrSql += " ," & Val(0) & "" 'WASTPER
                StrSql += " ," & Val(0) & "" 'WASTAGE
                StrSql += " ," & Val(0) & "" 'MCGRM
                StrSql += " ," & Val(0) & "" 'MCHARGE
                StrSql += " ," & Val(stRow.Item("AMOUNT").ToString) & "" 'AMOUNT
                StrSql += " ," & Val(stRow.Item("RATE").ToString) & "" 'RATE
                StrSql += " ," & Val(stRow.Item("RATE").ToString) & "" 'BOARDRATE
                StrSql += " ,''" 'SALEMODE
                StrSql += " ,''" 'GRSNET
                StrSql += " ,''" 'TRANSTATUS ''
                StrSql += " ,'" & txtBillNo.Text & "'" 'REFNO ''
                StrSql += " ,'" & dtpBillDate_OWN.Value.ToString("yyyy-MM-dd") & "'" 'REFDATE NULL
                StrSql += " ,'" & CostCenterId & "'" 'COSTID 
                StrSql += " ,'" & strCompanyId & "'" 'COMPANYID
                StrSql += " ,''" 'FLAG
                StrSql += " ,0" 'EMPID
                StrSql += " ,0" 'TAGGRSWT
                StrSql += " ,0" 'TAGNETWT
                StrSql += " ,0" 'TAGRATEID
                StrSql += " ,0" 'TAGSVALUE
                StrSql += " ,''" 'TAGDESIGNER  
                StrSql += " ,0" 'ITEMCTRID
                StrSql += " ," & itemTypeId & "" 'ITEMTYPEID
                StrSql += " ,0" 'PURITY
                StrSql += " ,''" 'TABLECODE
                StrSql += " ,''" 'INCENTIVE
                StrSql += " ,'" & Mid(stRow.Item("CALC").ToString, 1, 1) & "'" 'WEIGHTUNIT
                StrSql += " ,'" & objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & stRow.Item("ITEM").ToString & "'", , , tran) & "'" 'CATCODE
                StrSql += " ,'" & objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & stRow.Item("ITEM").ToString & "'", , , tran) & "'" 'OCATCODE
                StrSql += " ,'" & _Accode & "'" 'ACCODE
                StrSql += " ,0" 'ALLOY
                StrSql += " ,'" & BatchNo & "'" 'BATCHNO
                StrSql += " ,'" & .Cells("REMARK1").Value.ToString & "'" 'REMARK1
                StrSql += " ,'" & .Cells("REMARK2").Value.ToString & "'" 'REMARK2
                StrSql += " ,'" & userId & "'" 'USERID
                StrSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                StrSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                StrSql += " ,'" & systemId & "'" 'SYSTEMID
                StrSql += " ,0" 'DISCOUNT
                StrSql += " ,''" 'RUNNO
                StrSql += " ,'" & _CashCtr & "'" 'CASHID
                StrSql += " ,0" 'TAX
                StrSql += " ,0" 'TDS
                StrSql += " ,0" 'STNAMT
                StrSql += " ,0" 'MISCAMT
                StrSql += " ,'" & objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & stRow.Item("ITEM").ToString & "'", , , tran) & "'" 'METALID
                StrSql += " ,'" & Mid(stRow.Item("UNIT").ToString, 1, 1) & "'" 'STONEUNIT
                StrSql += " ,'" & VERSION & "'" 'APPVER
                StrSql += " ,0" 'APPVER
                StrSql += " ," & OrdStateId & "" 'ORDSTATE_ID
                StrSql += " )"
                ExecQuery(SyncMode.Transaction, StrSql, cn, tran, CostCenterId)
            Next

            ''ORSTATUS
            If .Cells("JOBNO").Value.ToString <> "" Then
                Dim DtOrIrDetail As New DataTable
                DtOrIrDetail = BrighttechPack.GetTableStructure(cnStockDb, "ORIRDETAIL", cn, tran)
                Dim R() As DataRow = CType(CType(.Cells("METISSREC").Value, MaterialIssRec).objOrderInfo.DgvOrder.DataSource, DataTable).Select("CHECK = 'TRUE'")
                Dim SelectedOrderCount As Integer = R.Length
                Dim EntryOrder As Integer = objGPack.GetSqlValue("SELECT ISNULL(MAX(ENTRYORDER),0) FROM " & cnAdminDb & "..ORIRDETAIL", , , tran)
                Dim RowOrder As DataRow = Nothing
                For Each DgvRow As DataGridViewRow In CType(.Cells("METISSREC").Value, MaterialIssRec).objOrderInfo.DgvOrder.Rows
                    If CType(DgvRow.Cells("CHECK").Value, Boolean) <> True Then Continue For
                    RowOrder = DtOrIrDetail.NewRow
                    EntryOrder += 1
                    RowOrder("SNO") = GetNewSno(TranSnoType.ORIRDETAILCODE, tran, "GET_ADMINSNO_TRAN")
                    RowOrder("ORSNO") = DgvRow.Cells("SNO").Value.ToString
                    RowOrder("TRANNO") = TranNo
                    RowOrder("TRANDATE") = dtpTrandate.Value.ToString("yyyy-MM-dd")
                    RowOrder("DESIGNERID") = Val(objGPack.GetSqlValue(" SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE ISNULL(ACCODE,'') = '" & _Accode & "'", , , tran))
                    RowOrder("PCS") = IIf(SelectedOrderCount = 1, Val(.Cells("PCS").Value.ToString), Val(DgvRow.Cells("PCS").Value.ToString))
                    RowOrder("GRSWT") = IIf(SelectedOrderCount = 1, Val(.Cells("GRSWT").Value.ToString), Val(DgvRow.Cells("GRSWT").Value.ToString))
                    RowOrder("NETWT") = IIf(SelectedOrderCount = 1, Val(.Cells("NETWT").Value.ToString), Val(DgvRow.Cells("NETWT").Value.ToString))
                    RowOrder("WASTAGE") = IIf(SelectedOrderCount = 1, 0, Val(DgvRow.Cells("WASTAGE").Value.ToString))
                    RowOrder("MC") = IIf(SelectedOrderCount = 1, Val(.Cells("MC").Value.ToString), Val(DgvRow.Cells("MC").Value.ToString))
                    RowOrder("TAGNO") = DBNull.Value
                    RowOrder("ORSTATUS") = IIf(OMaterialType = MaterialType.Issue, "I", "R")
                    RowOrder("CANCEL") = DBNull.Value
                    RowOrder("COSTID") = CostCenterId
                    RowOrder("DESCRIPT") = DBNull.Value
                    RowOrder("ORNO") = DgvRow.Cells("ORNO").Value.ToString
                    RowOrder("BATCHNO") = BatchNo
                    RowOrder("USERID") = userId
                    RowOrder("UPDATED") = Today.Date.ToString("yyyy-MM-dd")
                    RowOrder("UPTIME") = Date.Now.ToLongTimeString
                    RowOrder("APPVER") = VERSION
                    RowOrder("COMPANYID") = strCompanyId
                    RowOrder("TRANSFERED") = DBNull.Value
                    RowOrder("PROID") = DBNull.Value
                    RowOrder("ORDSTATE_ID") = objGPack.GetSqlValue("SELECT ORDSTATE_ID FROM " & cnAdminDb & "..ORDERSTATUS WHERE ORDSTATE_NAME = '" & .Cells("ORDSTATE_NAME").Value.ToString & "'", , , tran)
                    RowOrder("ENTRYORDER") = EntryOrder
                    RowOrder("CATCODE") = ""
                    DtOrIrDetail.Rows.Add(RowOrder)
                Next
                InsertData(SyncMode.Transaction, DtOrIrDetail, cn, tran, CostCenterId)
            End If
        End With
    End Sub
    Private Sub InsertMiscDetails(ByVal IssSno As String _
 , ByVal TNO As Integer, ByVal MiscRow As DataRow)
        Dim miscId As Integer = 0
        Dim sno As String = Nothing
        If OMaterialType = MaterialType.Issue Then
            sno = GetNewSno(IIf(_AccAudit, TranSnoType.TISSMISCCODE, TranSnoType.ISSMISCCODE), tran)
        Else
            sno = GetNewSno(IIf(_AccAudit, TranSnoType.TRECEIPTMISCCODE, TranSnoType.RECEIPTMISCCODE), tran)
        End If
        ''Find MiscId
        StrSql = " SELECT MISCID FROM " & cnAdminDb & "..MISCCHARGES WHERE MISCNAME = '" & MiscRow.Item("MISC").ToString & "'"
        miscId = Val(objGPack.GetSqlValue(StrSql, , , tran))

        If OMaterialType = MaterialType.Issue Then
            StrSql = " INSERT INTO " & cnStockDb & ".." & IIf(_AccAudit, "TISSMISC", "ISSMISC")
        Else
            StrSql = " INSERT INTO " & cnStockDb & ".." & IIf(_AccAudit, "TRECEIPTMISC", "RECEIPTMISC")
        End If
        StrSql += " ("
        StrSql += " SNO,ISSSNO,TRANNO,TRANDATE,TRANTYPE"
        StrSql += " ,MISCID,AMOUNT,TRANSTATUS,COSTID"
        StrSql += " ,COMPANYID,BATCHNO,SYSTEMID,APPVER,DISCOUNT"
        StrSql += " )"
        StrSql += " VALUES"
        StrSql += " ("
        StrSql += " '" & sno & "'" ''SNO
        StrSql += " ,'" & IssSno & "'" 'ISSSNO
        StrSql += " ," & TNO & "" 'TRANNO
        StrSql += " ,'" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
        StrSql += " ,'" & IIf(OMaterialType = MaterialType.Issue, "I", "R") & "" & Mid(cmbTransactionType.Text, 1, 2) & "'" 'TRANTYPE
        StrSql += " ,'" & miscId & "'" 'MISCID
        StrSql += " ," & Val(MiscRow.Item("AMOUNT").ToString) & "" 'AMOUNT
        StrSql += " ,''" 'TRANSTATUS
        StrSql += " ,'" & CostCenterId & "'" 'COSTID
        StrSql += " ,'" & strCompanyId & "'" 'COMPANYID
        StrSql += " ,'" & BatchNo & "'" 'BATCHNO
        StrSql += " ,'" & systemId & "'" 'SYSTEMID
        StrSql += " ,'" & VERSION & "'" 'APPVER
        StrSql += " ," & Val(MiscRow.Item("DISCOUNT").ToString) & "" 'DISCOUNT
        StrSql += " )"
        ExecQuery(SyncMode.Transaction, StrSql, cn, tran, CostCenterId)
    End Sub
    Private Sub InsertStoneDetailsforReverseEntry(ByVal IssSno As String _
 , ByVal TNO As Integer, ByVal stRow As DataRow, Optional ByVal taxx As Decimal = Nothing)
        Dim stnItemId As Integer = 0
        Dim stnSubItemid As Integer = 0
        Dim stnCatCode As String = Nothing
        Dim vat As Double = Nothing
        Dim sno As String = Nothing
        sno = GetNewSno(TranSnoType.ISSSTONECODE, tran)
        ''Find stnCatCode
        StrSql = " Select CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & stRow.Item("ITEM").ToString & "'"
        stnCatCode = objGPack.GetSqlValue(StrSql, , , tran)
        If taxx <> 0 Then
            StrSql = " SELECT "
            If OMaterialType = MaterialType.Issue Then StrSql += " SALESTAX"
            If OMaterialType = MaterialType.Receipt Then StrSql += " PTAX "
            StrSql += " FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = '" & stnCatCode & "'"
            Dim vatPer As Double = Val(objGPack.GetSqlValue(StrSql, , , tran))
            'vatPer = IIf(vatPer = 0, 1, vatPer)
            vat = Val(stRow!AMOUNT.ToString) * (vatPer / 100)
        End If

        ''Find itemId
        StrSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & stRow.Item("ITEM").ToString & "'"
        stnItemId = Val(objGPack.GetSqlValue(StrSql, , , tran))

        ''Find subItemId
        StrSql = " SELECT ISNULL(SUBITEMID,0)AS SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & stRow.Item("SUBITEM").ToString & "' AND ITEMID = " & stnItemId & ""
        stnSubItemid = Val(objGPack.GetSqlValue(StrSql, , , tran))

        StrSql = " INSERT INTO " & cnStockDb & "..ISSSTONE"
        StrSql += " ("
        StrSql += " SNO,ISSSNO,TRANNO,TRANDATE,TRANTYPE"
        StrSql += " ,STNPCS,STNWT,STNRATE,STNAMT"
        StrSql += " ,STNITEMID,STNSUBITEMID,CALCMODE,STONEUNIT"
        StrSql += " ,STONEMODE,TRANSTATUS,COSTID,COMPANYID"
        StrSql += " ,BATCHNO,SYSTEMID,CATCODE,TAX,APPVER,DISCOUNT"
        'If OMaterialType = MaterialType.Receipt Then StrSql += " ,JOBISNO"
        StrSql += " ,JOBISNO"
        StrSql += ",OCATCODE,SEIVE,STUDDEDUCT"
        StrSql += " )"
        StrSql += " VALUES"
        StrSql += " ("
        StrSql += " '" & sno & "'" ''SNO
        StrSql += " ,'" & IssSno & "'" 'ISSSNO
        StrSql += " ," & TNO & "" 'TRANNO
        StrSql += " ,'" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
        StrSql += " ,'IAP'" 'TRANTYPE
        StrSql += " ," & Val(stRow.Item("PCS").ToString) & "" 'STNPCS
        StrSql += " ," & Val(stRow.Item("WEIGHT").ToString) & "" 'STNWT
        StrSql += " ," & Val(stRow.Item("RATE").ToString) & "" 'STNRATE
        StrSql += " ,NULL" 'STNAMT
        'StrSql += " ," & Val(stRow.Item("AMOUNT").ToString) & "" 'STNAMT
        StrSql += " ," & stnItemId & "" 'STNITEMID
        StrSql += " ," & stnSubItemid & "" 'STNSUBITEMID
        StrSql += " ,'" & Mid(stRow.Item("CALC").ToString, 1, 1) & "'" 'CALCMODE
        StrSql += " ,'" & Mid(stRow.Item("UNIT").ToString, 1, 1) & "'" 'STONEUNIT
        StrSql += " ,''" 'STONEMODE 
        StrSql += " ,''" 'TRANSTATUS
        StrSql += " ,'" & CostCenterId & "'" 'COSTID
        StrSql += " ,'" & strCompanyId & "'" 'COMPANYID
        StrSql += " ,'" & BatchNo & "'" 'BATCHNO
        StrSql += " ,'" & systemId & "'" 'SYSTEMID
        StrSql += " ,'" & stnCatCode & "'" 'CATCODE
        StrSql += " ," & vat & "" 'TAX
        StrSql += " ,'" & VERSION & "'" 'APPVER
        StrSql += " ," & Val(stRow.Item("DISCOUNT").ToString) & "" 'DISCOUNT
        'If OMaterialType = MaterialType.Receipt Then StrSql += " ,'" & stRow.Item("ISSSNO").ToString & "'" 'JOBISNO
        StrSql += " ,'" & stRow.Item("ISSSNO").ToString & "'" 'JOBISNO
        StrSql += " ,'" & stRow.Item("OCATCODE").ToString & "'" 'OCATCODE
        StrSql += " ,'" & stRow.Item("SEIVE").ToString & "'" 'SEIVE
        If stRow.Item("STUDDEDUCT").ToString <> "" Then
            StrSql += " ,'" & Mid(stRow.Item("STUDDEDUCT").ToString, 1, 1) & "'" 'STUDDEDUCT
        Else
            StrSql += " ,''" 'STUDDEDUCT
        End If
        StrSql += " )"
        ExecQuery(SyncMode.Transaction, StrSql, cn, tran, CostCenterId)
    End Sub

    Private Sub InsertStoneDetails(ByVal IssSno As String _
 , ByVal TNO As Integer, ByVal stRow As DataRow, Optional ByVal taxx As Decimal = Nothing)
        Dim stnItemId As Integer = 0
        Dim stnSubItemid As Integer = 0
        Dim stnCatCode As String = Nothing
        Dim vat As Double = Nothing
        Dim sno As String = Nothing
        If OMaterialType = MaterialType.Issue Then
            sno = GetNewSno(IIf(_AccAudit, TranSnoType.TISSSTONECODE, TranSnoType.ISSSTONECODE), tran)
        Else
            sno = GetNewSno(IIf(_AccAudit, TranSnoType.TRECEIPTSTONECODE, TranSnoType.RECEIPTSTONECODE), tran)
        End If
        ''Find stnCatCode
        StrSql = " Select CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & stRow.Item("ITEM").ToString & "'"
        stnCatCode = objGPack.GetSqlValue(StrSql, , , tran)
        If taxx <> 0 Then
            StrSql = " SELECT "
            If OMaterialType = MaterialType.Issue Then StrSql += " SALESTAX"
            If OMaterialType = MaterialType.Receipt Then StrSql += " PTAX "
            StrSql += " FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = '" & stnCatCode & "'"
            Dim vatPer As Double = Val(objGPack.GetSqlValue(StrSql, , , tran))
            'vatPer = IIf(vatPer = 0, 1, vatPer)
            vat = Val(stRow!AMOUNT.ToString) * (vatPer / 100)
        End If

        ''Find itemId
        StrSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & stRow.Item("ITEM").ToString & "'"
        stnItemId = Val(objGPack.GetSqlValue(StrSql, , , tran))

        ''Find subItemId
        StrSql = " SELECT ISNULL(SUBITEMID,0)AS SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & stRow.Item("SUBITEM").ToString & "' AND ITEMID = " & stnItemId & ""
        stnSubItemid = Val(objGPack.GetSqlValue(StrSql, , , tran))

        If OMaterialType = MaterialType.Issue Then
            StrSql = " INSERT INTO " & cnStockDb & ".." & IIf(_AccAudit, "TISSSTONE", "ISSSTONE")
        Else
            StrSql = " INSERT INTO " & cnStockDb & ".." & IIf(_AccAudit, "TRECEIPTSTONE", "RECEIPTSTONE")
        End If
        StrSql += " ("
        StrSql += " SNO,ISSSNO,TRANNO,TRANDATE,TRANTYPE"
        StrSql += " ,STNPCS,STNWT,STNRATE,STNAMT"
        StrSql += " ,STNITEMID,STNSUBITEMID,CALCMODE,STONEUNIT"
        StrSql += " ,STONEMODE,TRANSTATUS,COSTID,COMPANYID"
        StrSql += " ,BATCHNO,SYSTEMID,CATCODE,TAX,APPVER,DISCOUNT"
        'If OMaterialType = MaterialType.Receipt Then StrSql += " ,JOBISNO"
        StrSql += " ,JOBISNO"
        If OMaterialType = MaterialType.Issue Then StrSql += " ,RESNO"
        If OMaterialType = MaterialType.Receipt Then StrSql += " ,ISSNO"
        StrSql += ",OCATCODE,SEIVE,CUTID,COLORID,CLARITYID,SHAPEID,SETTYPEID,HEIGHT,WIDTH,STUDDEDUCT"
        StrSql += ", STNGRPID"
        StrSql += " )"
        StrSql += " VALUES"
        StrSql += " ("
        StrSql += " '" & sno & "'" ''SNO
        StrSql += " ,'" & IssSno & "'" 'ISSSNO
        StrSql += " ," & TNO & "" 'TRANNO
        StrSql += " ,'" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
        If OMaterialType = MaterialType.Receipt Then
            StrSql += " ,'R" & Mid(stRow.Item("STNTYPE").ToString, 1, 2) & "'" 'TRANTYPE
        Else
            'StrSql += " ,'" & IIf(OMaterialType = MaterialType.Issue, "I", "R") & "" & Mid(cmbTransactionType.Text, 1, 2) & "'" 'TRANTYPE
            StrSql += " ,'I" & Mid(stRow.Item("STNTYPE").ToString, 1, 2) & "'" 'TRANTYPE
        End If
        StrSql += " ," & Val(stRow.Item("PCS").ToString) & "" 'STNPCS
        StrSql += " ," & Val(stRow.Item("WEIGHT").ToString) & "" 'STNWT
        StrSql += " ," & Val(stRow.Item("RATE").ToString) & "" 'STNRATE
        StrSql += " ," & Val(stRow.Item("AMOUNT").ToString) & "" 'STNAMT
        StrSql += " ," & stnItemId & "" 'STNITEMID
        StrSql += " ," & stnSubItemid & "" 'STNSUBITEMID
        StrSql += " ,'" & Mid(stRow.Item("CALC").ToString, 1, 1) & "'" 'CALCMODE
        StrSql += " ,'" & Mid(stRow.Item("UNIT").ToString, 1, 1) & "'" 'STONEUNIT
        StrSql += " ,''" 'STONEMODE 
        StrSql += " ,''" 'TRANSTATUS
        StrSql += " ,'" & CostCenterId & "'" 'COSTID
        StrSql += " ,'" & strCompanyId & "'" 'COMPANYID
        StrSql += " ,'" & BatchNo & "'" 'BATCHNO
        StrSql += " ,'" & systemId & "'" 'SYSTEMID
        StrSql += " ,'" & stnCatCode & "'" 'CATCODE
        StrSql += " ," & vat & "" 'TAX
        StrSql += " ,'" & VERSION & "'" 'APPVER
        StrSql += " ," & Val(stRow.Item("DISCOUNT").ToString) & "" 'DISCOUNT
        'If OMaterialType = MaterialType.Receipt Then StrSql += " ,'" & stRow.Item("ISSSNO").ToString & "'" 'JOBISNO
        StrSql += " ,'" & stRow.Item("ISSSNO").ToString & "'" 'JOBISNO
        If OMaterialType = MaterialType.Issue Then StrSql += " ,'" & stRow.Item("RESNO").ToString & "'" 'RESNO
        If OMaterialType = MaterialType.Receipt Then StrSql += " ,'" & stRow.Item("RESNO").ToString & "'" 'ISSNO
        StrSql += " ,'" & stRow.Item("OCATCODE").ToString & "'" 'OCATCODE
        StrSql += " ,'" & stRow.Item("SEIVE").ToString & "'" 'SEIVE

        StrSql += " ," & Val(stRow.Item("CUTID").ToString) & " " 'CUTID
        StrSql += " ," & Val(stRow.Item("COLORID").ToString) & "" 'COLORID
        StrSql += " ," & Val(stRow.Item("CLARITYID").ToString) & "" 'CLARITYID
        StrSql += " ," & Val(stRow.Item("SHAPEID").ToString) & "" 'SHAPEID
        StrSql += " ," & Val(stRow.Item("SETTYPEID").ToString) & "" 'SETTYPEID
        StrSql += " ,'" & Val(stRow.Item("HEIGHT").ToString) & "'" 'HEIGHT
        StrSql += " ,'" & Val(stRow.Item("WIDTH").ToString) & "'" 'WIDTH

        If stRow.Item("STUDDEDUCT").ToString <> "" Then
            StrSql += " ,'" & Mid(stRow.Item("STUDDEDUCT").ToString, 1, 1) & "'" 'STUDDEDUCT
        Else
            StrSql += " ,''" 'STUDDEDUCT
        End If
        StrSql += " ,'" & Val(stRow.Item("STNGRPID").ToString) & "'" 'STNGRPID
        StrSql += " )"
        ExecQuery(SyncMode.Transaction, StrSql, cn, tran, CostCenterId)
    End Sub

    Private Sub InsertAlloyDetails(ByVal IssSno As String _
, ByVal TNO As Integer, ByVal stRow As DataRow, ByVal TotAlloy As Decimal)
        Dim InAlloyId As String = ""
        Dim sno As String = Nothing
        sno = GetNewSno(TranSnoType.ALLOYDETAILSCODE, tran)
        If stRow Is Nothing Then
            InAlloyId = "C"
        Else
            ''Find Alloy
            StrSql = " SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & stRow.Item("ALLOY").ToString & "'"
            InAlloyId = objGPack.GetSqlValue(StrSql, , , tran)
        End If

        StrSql = " INSERT INTO " & cnStockDb & "..ALLOYDETAILS"
        StrSql += " ("
        StrSql += vbCrLf & " SNO,ISSSNO,TRANNO,TRANDATE,TRANTYPE,ALLOYID,WEIGHT,COSTID,COMPANYID,BATCHNO,SYSTEMID,APPVER"
        StrSql += " ) VALUES"
        StrSql += " ("
        StrSql += " '" & sno & "'" ''SNO
        StrSql += " ,'" & IssSno & "'" 'ISSSNO
        StrSql += " ," & TNO & "" 'TRANNO
        StrSql += " ,'" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
        StrSql += " ,'" & IIf(OMaterialType = MaterialType.Issue, "I", "R") & "" & Mid(cmbTransactionType.Text, 1, 2) & "'" 'TRANTYPE
        StrSql += " ,'" & InAlloyId & "'" 'ALLOYID
        If stRow Is Nothing Then
            StrSql += " ," & TotAlloy & "" 'WEIGHT
        Else
            StrSql += " ," & Val(stRow.Item("WEIGHT").ToString) & "" 'WEIGHT
        End If
        StrSql += " ,'" & CostCenterId & "'" 'COSTID
        StrSql += " ,'" & strCompanyId & "'" 'COMPANYID
        StrSql += " ,'" & BatchNo & "'" 'BATCHNO
        StrSql += " ,'" & systemId & "'" 'SYSTEMID        
        StrSql += " ,'" & VERSION & "'" 'APPVER
        StrSql += " )"
        ExecQuery(SyncMode.Transaction, StrSql, cn, tran, CostCenterId)
    End Sub

    Private Sub cmbAcName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAcName.GotFocus
        If cmbCostCentre.Enabled = True And cmbCostCentre.Text = "" Then cmbCostCentre.Focus()

    End Sub

    Private Sub cmbAcName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbAcName.KeyPress

    End Sub

    Private Sub cmbAcName_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAcName.Leave
        dtCatBalance.Visible = False
        _Accode = ""
        If cmbAcName.Text.Trim <> "" Then
            _Accode = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbAcName.Text & "'")
            Dim crDays As Integer = Val(objGPack.GetSqlValue("SELECT CREDITDAYS FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbAcName.Text & "'"))
            If crDays > 0 Then txtCreditDays_NUM.Text = crDays Else txtCreditDays_NUM.Text = ""
        Else
            txtCreditDays_NUM.Clear()
            If ExitToolStrip = True Then Exit Sub
            If btnExit.Focused Then Exit Sub
            If btnNew.Focused Then Exit Sub
            If btnView.Focused Then Exit Sub
            If ExitToolStripMenuItem.Selected = True Then Exit Sub
            cmbAcName.Focus()
            cmbAcName.Select()
            Exit Sub
        End If
        If EditBatchno <> Nothing Then
            txtCreditDays_NUM.Text = IIf(EditDueDays <> 0, EditDueDays, "")
        End If

    End Sub


    Private Sub cmbAcName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAcName.LostFocus

    End Sub

    Private Sub txtCreditDays_NUM_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCreditDays_NUM.GotFocus
        If Val(txtAdjCredit_AMT.Text) = 0 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtCreditDays_NUM_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCreditDays_NUM.TextChanged
        dtpCreditDays.Visible = True
        dtpCreditDays.Value = dtpTrandate.Value.Date.AddDays(Val(txtCreditDays_NUM.Text))
    End Sub

    Private Sub dtpCreditDays_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtpCreditDays.Enter
        SendKeys.Send("{tab}")
    End Sub


    Private Sub cmbCostCentre_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        CostCenterId = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre.Text & "'", , , tran)
    End Sub



    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, IIf(OMaterialType = MaterialType.Receipt, "REC", "ISS") & Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        Dim f As New frmSmithRecIssView
        If OMaterialType = MaterialType.Issue Then f.rbtIssue.Checked = True
        If OMaterialType = MaterialType.Receipt Then f.rbtReceipt.Checked = True
        f.rbtsummary.Enabled = False
        f.ShowDialog()
    End Sub


    Private Sub cmbTransactionType_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbTransactionType.Enter
        'If funcOTPaccess(Me.Name.ToString, "ACCOUNTS_MIMR", cnCostId, userId) = True Then Exit Sub
        Dim serverDate As Date = GetActualEntryDate()
        'Dim minAllowDays As Integer = Val(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'DATECONTROL_MIMR'"))
        'If Not CheckBckDaysEntry(userId, "", dtpTrandate.Value, lblTitle.Text.ToString) Then
        '    dtpTrandate.Focus()
        '    Exit Sub
        'Else
        '    GoTo ExT
        'End If
        Dim RestrictDays As String = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'DATECONTROL_MIMR'")
        If RestrictDays.Contains(",") = False Then
            If Not (dtpTrandate.Value >= serverDate.AddDays(-1 * Val(RestrictDays))) Then
                If funcOTPaccess(Me.Name.ToString, "ACCOUNTS_MIMR", cnCostId, userId) = True Then Exit Sub
                'MsgBox("Invalid Date", MsgBoxStyle.Information)
                'dtpTrandate.Focus()

                GoTo ExT

                Exit Sub
            End If
        Else
            Dim dayarray() As String = Split(RestrictDays, ",")
            Dim closeday As Integer = Val(dayarray(0).ToString)
            Dim mondiv As Integer = Val(dayarray(1).ToString)
            If closeday > serverDate.Day Then
                Dim mindate As Date = serverDate.AddMonths(-1 * mondiv)
                mindate = mindate.AddDays(-1 * (mindate.Day - 1))
                If Not (dtpTrandate.Value >= mindate) Then
                    If funcOTPaccess(Me.Name.ToString, "ACCOUNTS_MIMR", cnCostId, userId) = True Then Exit Sub
                    'MsgBox("Invalid Date", MsgBoxStyle.Information)
                    'dtpTrandate.Focus()

                    GoTo ExT
                    Exit Sub
                End If
            Else
                Dim mindate As Date = serverDate.AddMonths(-1 * 0)
                mindate = mindate.AddDays(-1 * (mindate.Day - 1))
                If Not (dtpTrandate.Value >= mindate) Then
                    If funcOTPaccess(Me.Name.ToString, "ACCOUNTS_MIMR", cnCostId, userId) = True Then Exit Sub
                    'MsgBox("Invalid Bill Date", MsgBoxStyle.Information)
                    'dtpTrandate.Focus()

                    GoTo ExT
                    Exit Sub
                End If
            End If
        End If
        GoTo ExTL2
ExT:
        If Not CheckBckDaysEntry(userId, "", dtpTrandate.Value, lblTitle.Text.ToString) Then
            dtpTrandate.Focus()
            Exit Sub
        Else
            GoTo ExTL2
        End If
ExTL2:
        If cmbAcName.Text = "" Then
            MsgBox("Account name is blank", MsgBoxStyle.Information)
            cmbAcName.Focus()
            Exit Sub
        Else
            If objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME ='" & cmbAcName.Text & "'").ToString = "" Then MsgBox("Invalid account selected", MsgBoxStyle.Information)

        End If
        'If Not (dtpTrandate.Value >= serverDate.AddDays(-1 * minAllowDays)) Then
        '    MsgBox("Invalid Transaction date", MsgBoxStyle.Information)
        '    dtpTrandate.Focus()
        '    Exit Sub
        'End If
    End Sub
    Private Sub dtpBillDate_OWN_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub txtBillNo_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBillNo.TextChanged

    End Sub

    Private Sub txtBillNo_LostFocus(sender As Object, e As EventArgs) Handles txtBillNo.LostFocus
        If MIMR_VALIDATEBILLNO = True And Val(txtBillNo.Text) > 0 And cmbAcName.Text <> "" Then
            If validatebillno() = True Then
                MsgBox("Bill No Already Exist for this Account", MsgBoxStyle.Information)
                txtBillNo.Select()
            End If
        End If
    End Sub

    Function validatebillno()
        If Not EditBatchno Is Nothing Then Return False
        StrSql = " SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME ='" & cmbAcName.Text & "'"
        Dim chkaccode As String = GetSqlValue(cn, StrSql)
        If chkaccode.ToString <> "" Then
            StrSql = "SELECT SUM(CNT)CNT FROM ("
            StrSql += " SELECT COUNT(*)CNT FROM " & cnStockDb & "..RECEIPT WHERE ACCODE='" & chkaccode & "' AND REFNO='" & txtBillNo.Text & "' AND ISNULL(CANCEL,'')<>'Y'"
            If cmbCostCentre.Enabled = True Then
                StrSql += " AND COSTID='" & CostCenterId.ToString & "'"
            End If
            StrSql += " )X "
            Da = New OleDbDataAdapter(StrSql, cn)
            Dim dtAdd As New DataTable
            Da.Fill(dtAdd)
            If Val(dtAdd.Rows(0).Item("CNT").ToString) > 0 Then
                Return True
            Else
                Return False
            End If
        End If
        Return False
    End Function
    Private Sub cmbAcName_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAcName.SelectedIndexChanged
        lblAddress.Text = ""
        lblAddress1.Text = ""
        StrSql = " SELECT ACCODE,ACTYPE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME ='" & cmbAcName.Text & "'"
        Dim daAcc As New OleDbDataAdapter(StrSql, cn)
        Dim dtAccode As New DataTable()
        daAcc.Fill(dtAccode)
        If dtAccode.Rows.Count > 0 Then
            acCode = dtAccode.Rows(0).Item("ACCODE").ToString
            Dim actype As String
            actype = dtAccode.Rows(0).Item("ACTYPE").ToString
            If actype = "I" Then
                cmbTransactionType.Text = "INTERNAL TRANSFER"
            End If
        End If

        StrSql = " SELECT DISTINCT ADDRESS1 as ADDRESS FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE='" & acCode & "'"
        StrSql += " UNION ALL"
        StrSql += " SELECT DISTINCT ADDRESS2 AS ADDRESS FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE='" & acCode & "'"
        StrSql += " UNION ALL"
        StrSql += " SELECT DISTINCT MOBILE AS ADDRESS FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE='" & acCode & "'"
        StrSql += " UNION ALL"
        StrSql += " SELECT DISTINCT GSTNO AS ADDRESS FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE='" & acCode & "'"
        Da = New OleDbDataAdapter(StrSql, cn)
        Dim dtAdd As New DataTable
        Da.Fill(dtAdd)
        If dtAdd.Rows.Count > 0 Then
            For i As Integer = 0 To dtAdd.Rows.Count - 1
                If i = 0 Then lblAddress.Text = acCode & IIf(dtAdd.Rows(i).Item("ADDRESS").ToString <> "", " - " & dtAdd.Rows(i).Item("ADDRESS").ToString, "")
                If i = 1 Then lblAddress1.Text = dtAdd.Rows(i).Item("ADDRESS").ToString
                If i = 2 Then MobileNo = dtAdd.Rows(i).Item("ADDRESS").ToString
                If i = 3 Then lblAddress1.Text += IIf(dtAdd.Rows(i).Item("ADDRESS").ToString <> "", " - " & dtAdd.Rows(i).Item("ADDRESS").ToString, "")
            Next
        End If
    End Sub
    Private Sub cmbTransactionType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbTransactionType.GotFocus
        'LoadBalanceWt()
    End Sub
    Private Sub LoadBalanceWt()
        If Not CatbalanceDisplay Then Exit Sub
        lblBalance.Text = ""
        If cmbAcName.Text <> "" Then
            Dim chkwt As String = ""
            chkwt = "GNP"
            Dim DtGrid As New DataSet()
            StrSql = "EXEC " & cnAdminDb & "..SP_RPT_SMITHACCOUNTBALANCE_CAT"
            StrSql += vbCrLf + " @DBNAME='" & cnStockDb & "'"
            StrSql += vbCrLf + ",@TODATE='" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "'"
            StrSql += vbCrLf + ",@ACCODE='" & acCode & "'"
            StrSql += vbCrLf + ",@WEIGHT='" & chkwt & "'"
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.CommandTimeout = 1000
            Da = New OleDbDataAdapter(Cmd)
            Da.Fill(DtGrid)
            dtCatBalance.DataSource = DtGrid.Tables(0)
            If dtCatBalance.Rows.Count > 1 Then
                With dtCatBalance
                    .Visible = True
                    .BringToFront()
                    .Columns(0).Width = 85
                    .Columns(1).Width = 75
                    .Columns(2).Width = 75
                    .Columns(3).Width = 75
                    .Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End With
            End If
        End If
    End Sub

    Private Sub cmbCostCentre_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbCostCentre.KeyDown
        If e.KeyCode = Keys.Enter Then
            If cmbCostCentre.Enabled = False Then Exit Sub
            CostCenterId = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre.Text & "'", , , tran)
            If CostCenterId = "" Then MsgBox("Please Select Valid Costcentre", MsgBoxStyle.Information) : cmbCostCentre.Text = "" : cmbCostCentre.Focus() : Exit Sub
        End If
    End Sub

    Private Sub txtTCS_AMT_TextChanged(sender As Object, e As EventArgs) Handles txtTCS_AMT.TextChanged
        CalcReceiveAmount()
    End Sub

    Private Sub LoadValues()

        If Val(txtEstNo_NUM.Text) = 0 Then Exit Sub
        If (cmbTransactionType.Text <> "ISSUE" And cmbTransactionType.Text <> "PURCHASE RETURN") Then MsgBox("Can Load in this EntryType.", MsgBoxStyle.Information) : Exit Sub
        btnNew.Enabled = False
        btnSave.Enabled = False

        Dim ViewBatchno As String = ""
        Dim CurrentStockDb As String = ""
        StrSql = " SELECT DBNAME FROM " & cnAdminDb & "..DBMASTER WHERE '" & dtpEstdate_OWN.Value.ToString("yyyy-MM-dd") & "' BETWEEN STARTDATE AND ENDDATE"
        CurrentStockDb = objGPack.GetSqlValue(StrSql)
        StrSql = " SELECT 'EXISTS' FROM MASTER..SYSDATABASES WHERE NAME = '" & CurrentStockDb & "'"
        If objGPack.GetSqlValue(StrSql, , "-1") = "-1" Then
            MsgBox(CurrentStockDb & " Database not found", MsgBoxStyle.Information)
            CurrentStockDb = cnStockDb
            dtpBillDate_OWN.Select()
            Exit Sub
        End If

        StrSql = " SELECT TOP 1 ESTBATCHNO FROM " & CurrentStockDb & "..ESTISSUE AS I WHERE TRANDATE='" + dtpEstdate_OWN.Value.Date.ToString("yyyy-MM-dd") + "' "
        StrSql += " AND TRANNO ='" + txtEstNo_NUM.Text.ToString + "' AND TRANTYPE='SA' AND ISNULL(CANCEL,'')<> 'Y' AND ISNULL(BATCHNO,'')='' "

        ViewBatchno = objGPack.GetSqlValue(StrSql)
        If ViewBatchno.ToString = "" Then
            MsgBox("Record not found.")
            txtEstNo_NUM.SelectAll()
            Exit Sub
        Else
            txtEstNo_NUM.Enabled = False
        End If
        ClearTran()
        Try

            Dim index As Integer = Nothing

            For cnt As Integer = 0 To DtTran.Rows.Count - 1
                If DtTran.Rows(cnt).RowState <> DataRowState.Deleted Then
                    If DtTran.Rows(cnt).Item("TRANTYPE").ToString = "" Then
                        index = cnt
                        DtTran.Rows.Add()
                        Exit For
                    End If
                End If
            Next

            StrSql = vbCrLf + "  SELECT "
            StrSql += vbCrLf + "  I.SNO,CASE WHEN ISNULL(CA.DIASTNTYPE,'')<>'' THEN 'T' ELSE 'O' END FLAG"
            StrSql += vbCrLf + "  ,'' AS JOBNO"
            StrSql += vbCrLf + "  ,ME.METALID,ME.METALNAME AS METAL,CA.CATNAME,CA.CATCODE"
            StrSql += vbCrLf + "  ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.OCATCODE)AS ISSCATNAME"
            StrSql += vbCrLf + "  ,I.PURITY"
            StrSql += vbCrLf + "  ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID) AS ITEM"
            StrSql += vbCrLf + "  ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = I.SUBITEMID) AS SUBITEM"
            StrSql += vbCrLf + "  ,I.ITEMID,I.SUBITEMID,I.PCS"
            StrSql += vbCrLf + "  ,I.GRSWT GRSWT"
            StrSql += vbCrLf + "  ,I.LESSWT LESSWT"
            StrSql += vbCrLf + "  ,I.NETWT NETWT"
            StrSql += vbCrLf + "  ,'' AS UNIT,'' AS CALCMODE"
            StrSql += vbCrLf + "  ,I.GRSNET,0 WASTPER,0 AS WASTAGE"
            StrSql += vbCrLf + "  ,0 AS ALLOY,0 TOUCH"
            StrSql += vbCrLf + "  ,I.PUREWT PUREWT"
            StrSql += vbCrLf + "  ,0 MCGRM,0 AS MC"
            If cmbTransactionType.Text = "PURCHASE RETURN" Then
                StrSql += vbCrLf + "  ,I.RATE RATE"
            Else
                StrSql += vbCrLf + "  ,0 RATE"
            End If
            StrSql += vbCrLf + "  ,0 BOARDRATE,0 AS STUDAMT"
            StrSql += vbCrLf + "  ,0 AS GROSSAMT,0 VATPER"
            StrSql += vbCrLf + "  ,(SELECT TOP 1 GSTCATNAME FROM " & cnAdminDb & "..GSTCATEGORY WHERE GSTPER= (SELECT SUM(TAXPER) FROM " & CurrentStockDb & "..ESTTAXTRAN WHERE BATCHNO=I.BATCHNO AND TAXID IN('SG','CG','IG') AND ISNULL(STUDDED,'N')<>'Y'))AS GSTCATNAME"
            StrSql += vbCrLf + "  ,(SELECT TOP 1 TAXPER FROM " & CurrentStockDb & "..ESTTAXTRAN WHERE BATCHNO=I.BATCHNO AND ISSSNO=I.SNO AND TAXID='SG' AND ISNULL(STUDDED,'N')<>'Y')AS SGSTPER"
            StrSql += vbCrLf + "  ,(SELECT TOP 1 TAXPER FROM " & CurrentStockDb & "..ESTTAXTRAN WHERE BATCHNO=I.BATCHNO AND ISSSNO=I.SNO AND TAXID='CG' AND ISNULL(STUDDED,'N')<>'Y')AS CGSTPER"
            StrSql += vbCrLf + "  ,(SELECT TOP 1 TAXPER FROM " & CurrentStockDb & "..ESTTAXTRAN WHERE BATCHNO=I.BATCHNO AND ISSSNO=I.SNO AND TAXID='IG' AND ISNULL(STUDDED,'N')<>'Y')AS IGSTPER"
            StrSql += vbCrLf + "  ,0 AS SGST"
            StrSql += vbCrLf + "  ,0 AS CGST"
            StrSql += vbCrLf + "  ,0 AS IGST"
            StrSql += vbCrLf + "  ,0 AS VAT"
            StrSql += vbCrLf + "  ,'' TDSCATID "
            StrSql += vbCrLf + "  ,'' AS TDSCATNAME "
            StrSql += vbCrLf + "  ,'' AS TDSPER "
            StrSql += vbCrLf + "  ,0 AS TDS,I.REMARK1,I.REMARK2,'' TRANSPORTMODE"
            If MI_POPUP_PROCESSTYPE Then
                StrSql += vbCrLf + "  ,'" & objProcessType.cmbProcessType.Text & "' AS ORDSTATE_NAME"
            Else
                StrSql += vbCrLf + "  ,'' AS ORDSTATE_NAME"
            End If

            StrSql += vbCrLf + "  ,0 AS ADDCHARGE,'' AS SEIVE"
            StrSql += vbCrLf + "  ,'' AS CUT,'' AS COLOR,'' AS CLARITY,'' AS SHAPE,'' AS SETTYPE,0 HEIGHT,0 WIDTH"
            StrSql += vbCrLf + "  ,0 SC,'' STKTYPE,0 TRFVALUE,ISNULL(I.TAGNO,'')TAGNO,0 DISCOUNT,0 ED,0 EDTAX,0 TCS,0 APRXAMT,0 APRXTAX,'N' RATEFIXED"
            StrSql += vbCrLf + "  FROM " & CurrentStockDb & "..ESTISSUE AS I"
            StrSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = I.CATCODE"
            StrSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..METALMAST AS ME ON ME.METALID = CA.METALID"
            StrSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMTAG AS IT ON IT.ITEMID = I.ITEMID AND IT.TAGNO = I.TAGNO AND IT.ISSDATE IS NULL AND ISNULL(IT.TOFLAG,'') ='' "
            StrSql += vbCrLf + "  WHERE I.ESTBATCHNO = '" & ViewBatchno & "' AND I.TRANTYPE ='SA' "

            Dim dtTranInfo As New DataTable
            Da = New OleDbDataAdapter(StrSql, cn)
            Da.Fill(dtTranInfo)
            If Not dtTranInfo.Rows.Count > 0 Then MsgBox("Estimation Not Found", MsgBoxStyle.Information) : btnNew_Click(New Object, New EventArgs) : Exit Sub


            Dim StateId As Integer = Val(objGPack.GetSqlValue("SELECT STATEID FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbAcName.Text & "'").ToString)
            If StateId = 0 Then StateId = CompanyStateId
            If StateId <> CompanyStateId Then
                InterStateBill = True
            End If
            Dim Desc As String = Nothing
            For Each ro As DataRow In dtTranInfo.Rows


                If Val(ro.Item("PCS").ToString) = 0 Then ro.Item("PCS") = DBNull.Value
                If Val(ro.Item("GRSWT").ToString) = 0 Then ro.Item("GRSWT") = DBNull.Value
                If Val(ro.Item("LESSWT").ToString) = 0 Then ro.Item("LESSWT") = DBNull.Value
                If Val(ro.Item("NETWT").ToString) = 0 Then ro.Item("NETWT") = DBNull.Value
                If Val(ro.Item("WASTPER").ToString) = 0 Then ro.Item("WASTPER") = DBNull.Value
                If Val(ro.Item("WASTAGE").ToString) = 0 Then ro.Item("WASTAGE") = DBNull.Value
                If Val(ro.Item("ALLOY").ToString) = 0 Then ro.Item("ALLOY") = DBNull.Value
                If Val(ro.Item("TOUCH").ToString) = 0 Then ro.Item("TOUCH") = DBNull.Value
                If Val(ro.Item("PUREWT").ToString) = 0 Then ro.Item("PUREWT") = DBNull.Value
                If Val(ro.Item("MCGRM").ToString) = 0 Then ro.Item("MCGRM") = DBNull.Value
                If Val(ro.Item("MC").ToString) = 0 Then ro.Item("MC") = DBNull.Value
                If Val(ro.Item("RATE").ToString) = 0 Then ro.Item("RATE") = DBNull.Value
                If Val(ro.Item("GROSSAMT").ToString) = 0 Then ro.Item("GROSSAMT") = DBNull.Value
                If Val(ro.Item("VATPER").ToString) = 0 Then ro.Item("VATPER") = DBNull.Value
                If Val(ro.Item("VAT").ToString) = 0 Then ro.Item("VAT") = DBNull.Value
                If Val(ro.Item("ED").ToString) = 0 Then ro.Item("ED") = DBNull.Value
                If Val(ro.Item("EDTAX").ToString) = 0 Then ro.Item("EDTAX") = DBNull.Value
                If Val(ro.Item("TCS").ToString) = 0 Then ro.Item("TCS") = DBNull.Value
                If Val(ro.Item("DISCOUNT").ToString) = 0 Then ro.Item("DISCOUNT") = DBNull.Value
                If Val(ro.Item("APRXAMT").ToString) = 0 Then ro.Item("APRXAMT") = DBNull.Value
                If Val(ro.Item("APRXTAX").ToString) = 0 Then ro.Item("APRXTAX") = DBNull.Value
                If TdsPer <> 0 Then ro.Item("VATPER") = TdsPer
                ObjMaterialDia = New MaterialIssRec(OMaterialType, cmbTransactionType.Text, dtpTrandate.Value, _Accode)
                ObjMaterialDia.BillCostId = CostCenterId
                ObjMaterialDia.Text = "MATERIAL " & cmbTransactionType.Text
                With ObjMaterialDia

                    StrSql = "  SELECT  "
                    StrSql += vbCrLf + "  I.METALID,I.ITEMNAME AS ITEM,S.SUBITEMNAME AS SUBITEM,T.STNPCS AS PCS,T.STNWT AS WEIGHT"
                    StrSql += vbCrLf + "  ,T.STONEUNIT AS UNIT,T.CALCMODE AS CALC"
                    If cmbTransactionType.Text = "PURCHASE RETURN" Then
                        StrSql += vbCrLf + "  ,T.STNRATE AS RATE ,T.STNAMT AS AMOUNT,'PURCHASE RETURN' STNTYPE"
                    ElseIf cmbTransactionType.Text = "APPROVAL ISSUE" Then
                        StrSql += vbCrLf + "  ,0 AS RATE,0 AS AMOUNT,'APPROVAL' STNTYPE"
                    ElseIf cmbTransactionType.Text = "ISSUE" Then
                        StrSql += vbCrLf + "  ,0 AS RATE,0 AS AMOUNT,'ISSUE' STNTYPE"
                    Else
                        StrSql += vbCrLf + "  ,0 AS RATE,0 AS AMOUNT,'ISSUE' STNTYPE"
                    End If
                    StrSql += vbCrLf + "  FROM " & CurrentStockDb & "..ESTISSSTONE AS T "
                    StrSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS I ON I.ITEMID=T.STNITEMID"
                    StrSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS S ON S.SUBITEMID=T.STNSUBITEMID"
                    StrSql += vbCrLf + "  WHERE T.ISSSNO = '" & ro.Item("SNO").ToString & "'"
                    StrSql += vbCrLf + "  AND T.COMPANYID='" & strCompanyId & "'"
                    StrSql += vbCrLf + "  AND ISNULL(T.COSTID,'')='" & cnCostId & "'"

                    Dim dtStone As New DataTable
                    Da = New OleDbDataAdapter(StrSql, cn)
                    Da.Fill(dtStone)

                    For Each RoStn As DataRow In dtStone.Rows
                        Dim RoStnIm As DataRow = .objStone.dtGridStone.NewRow
                        For Each Col As DataColumn In dtStone.Columns
                            RoStnIm.Item(Col.ColumnName) = RoStn.Item(Col.ColumnName)
                        Next
                        .objStone.dtGridStone.Rows.Add(RoStnIm)
                    Next
                    .objStone.dtGridStone.AcceptChanges()
                    .objStone.CalcStoneWtAmount()


                    DtTran.Rows(index).Item("ROWINDEX") = index

                    DtTran.Rows(index).Item("METISSREC") = ObjMaterialDia
                    .rbtOrnament.Checked = True
                    If _JobNoEnable = False Then
                        .txtOOrdNo.Text = ro.Item("JOBNO").ToString
                    Else
                        .txtOOrdNo.Text = ro.Item("JOBNO").ToString.Replace((GetCostId(cnCostId) & GetCompanyId(strCompanyId)), "")
                    End If

                    .cmbOMetal.Text = ro.Item("METAL").ToString
                    .cmbOMetal.SelectedValue = ro.Item("METALID").ToString
                    .cmbOMetal_SelectedValueChanged(Me, New EventArgs)

                    .cmbOCategory.Text = ro.Item("CATNAME").ToString
                    .cmbOCategory.SelectedValue = ro.Item("CATCODE").ToString
                    .cmbOCategory_SelectedValueChanged(Me, New EventArgs)

                    .cmbOIssuedCategory.Text = "" & ro.Item("ISSCATNAME")
                    .cmbOAcPostCategory.Text = "" & ro.Item("CATNAME")
                    .CmbOPurity.Text = ro.Item("PURITY").ToString
                    .cmbOItem.Text = ro.Item("ITEM").ToString
                    .cmbOItem.SelectedValue = ro.Item("ITEMID")
                    .cmbOItem_SelectedValueChanged(Me, New EventArgs)

                    .cmbOSubItem.Text = ro.Item("SUBITEM").ToString

                    .txtOPcs_NUM.Text = ro.Item("PCS").ToString
                    .txtOGrsWt_WET.Text = ro.Item("GRSWT").ToString
                    .txtONetWt_WET.Text = ro.Item("NETWT").ToString
                    .cmbOGrsNet.Text = IIf(ro.Item("GRSNET").ToString = "G", "GRS WT", "NET WT")
                    .txtOWastPer.Text = ro.Item("WASTPER").ToString
                    .txtOWast_WET.Text = ro.Item("WASTAGE").ToString
                    .txtOalloy_WET.Text = ro.Item("ALLOY").ToString
                    .txtOTouchAMT.Text = ro.Item("TOUCH").ToString
                    If .objStone.dtGridStone.Rows.Count > 0 Then
                        Dim stnWt As Double = Val(.objStone.gridStoneTotal.Rows(0).Cells("WEIGHT").Value.ToString)
                        Dim stnAmt As Double = Val(.objStone.gridStoneTotal.Rows(0).Cells("AMOUNT").Value.ToString)
                        .txtOLessWt_WET.Text = IIf(stnWt <> 0, Format(stnWt, "0.000"), Nothing)
                        .txtOStudAmt_AMT.Text = IIf(stnAmt <> 0, Format(stnAmt, "0.00"), Nothing)
                    Else
                        .txtOLessWt_WET.Text = ro.Item("LESSWT").ToString
                        .txtOStudAmt_AMT.Text = ""
                    End If

                    .txtOPureWt_WET.Text = ro.Item("PUREWT").ToString
                    .txtOMcGrm_AMT.Text = ro.Item("MCGRM").ToString
                    .txtOMc_AMT.Text = ro.Item("MC").ToString
                    .txtORate_OWN.Text = ro.Item("RATE").ToString
                    .oBoardRate = Val(ro.Item("BOARDRATE").ToString)
                    .txtOAddlCharge_AMT.Text = ro.Item("ADDCHARGE").ToString
                    .txtOGrsAmt_AMT.Text = ro.Item("GROSSAMT").ToString
                    .txtOVatPer_PER.Text = ro.Item("VATPER").ToString
                    .txtODisc.Text = ro.Item("DISCOUNT").ToString

                    If Val(ro.Item("TDS").ToString) > 0 Then
                        .txtOVat_AMT.Text = ro.Item("TDS").ToString
                    Else
                        .txtOVat_AMT.Text = ro.Item("VAT").ToString
                        If UCase(cmbTransactionType.Text) <> "PURCHASE RETURN" And UCase(cmbTransactionType.Text) <> "INTERNAL TRANSFER" And UCase(cmbTransactionType.Text) <> "APPROVAL RECEIPT" Then
                            .txtOVat_AMT.Text = 0
                        End If
                    End If
                    If Val(ro.Item("SGST").ToString) > 0 Then
                        .txtOSG_AMT.Text = ro.Item("SGST").ToString
                        .txtOCG_AMT.Text = ro.Item("CGST").ToString
                        .txtOSgst_AMT.Text = ro.Item("SGSTPER").ToString
                        .txtOCgst_AMT.Text = ro.Item("CGSTPER").ToString
                    ElseIf Val(ro.Item("IGST").ToString) > 0 Then
                        .txtOIG_AMT.Text = ro.Item("IGST").ToString
                        .txtOIgst_AMT.Text = ro.Item("IGSTPER").ToString
                    End If

                    .txtOED_AMT.Text = ro.Item("ED").ToString
                    .txtOTCS_AMT.Text = ro.Item("TCS").ToString
                    .txtOedPer_AMT.Text = ro.Item("EDTAX").ToString

                    If cmbTransactionType.Text = "PURCHASE RETURN" Then
                        .CalcOGrossAmt()
                    End If

                    .CalcONetAmt()
                    If ro.Item("TAGNO").ToString <> "" Then
                        .txtTagNo.Text = ro.Item("TAGNO").ToString
                    End If
                    .txtORemark1.Text = ro.Item("REMARK1").ToString
                    .txtORemark2.Text = ro.Item("REMARK2").ToString
                    .cmbOProcess.Text = ro.Item("ORDSTATE_NAME").ToString
                    '.txtOVat_AMT.Text = Ro.Item("VAT").ToString
                    .cmbOcalcon.Text = IIf(ro.Item("GRSNET").ToString = "G", "GRS WT", "NET WT")
                    .txtOedPer_AMT.Text = ro.Item("EDTAX").ToString
                    .txtOED_AMT.Text = ro.Item("ED").ToString
                    .txtOVatPer_PER.Text = ro.Item("VATPER").ToString
                    '.txtOVat_AMT.Text = Ro.Item("VAT").ToString
                    If Val(ro.Item("TDS").ToString) > 0 Then
                        .txtOVat_AMT.Text = ro.Item("TDS").ToString
                    Else
                        .txtOVat_AMT.Text = ro.Item("VAT").ToString
                        If UCase(cmbTransactionType.Text) <> "PURCHASE RETURN" And UCase(cmbTransactionType.Text) <> "INTERNAL TRANSFER" And UCase(cmbTransactionType.Text) <> "APPROVAL RECEIPT" Then
                            .txtOVat_AMT.Text = 0
                        End If
                    End If
                    .txtOAprxAmount_AMT.Text = ro.Item("APRXAMT").ToString
                    .txtOAprxTaxAmt_AMT.Text = ro.Item("APRXTAX").ToString
                    If ro.Item("RATEFIXED").ToString = "Y" Then
                        .chkORateFixed.Checked = True
                    Else
                        .chkORateFixed.Checked = False
                    End If

                    If Val(.txtOalloy_WET.Text) <> 0 Then DgvTran.Columns("ALLOY").Visible = True
                    DgvTran.AutoResizeRow(index)
                    LoadTransaction(ObjMaterialDia)
                    index = index + 1
                End With
            Next
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try
        txtEstNo_NUM.Enabled = True
        btnNew.Enabled = True
        btnSave.Enabled = True
    End Sub

    Private Sub loadtransferdata()
        If txttransferno.Text = "" Then Exit Sub
        If (cmbTransactionType.Text <> "ISSUE") Then MsgBox("Can Load in this EntryType.", MsgBoxStyle.Information) : Exit Sub
        btnNew.Enabled = False
        btnSave.Enabled = False
        Dim cmbcostid As String = ""
        If cmbCostCentre.Text = "" Then
            Exit Sub
        Else
            StrSql = " SELECT COSTID from " & cnAdminDb & ".. COSTCENTRE  WHERE COSTNAME = '" & cmbCostCentre.Text.ToString() & "'"
            cmbcostid = objGPack.GetSqlValue(StrSql)
        End If
        Dim Transferno As String = ""
        Dim CurrentStockDb As String = ""
        StrSql = " SELECT DBNAME FROM " & cnAdminDb & "..DBMASTER WHERE '" & dtpEstdate_OWN.Value.ToString("yyyy-MM-dd") & "' BETWEEN STARTDATE AND ENDDATE"
        CurrentStockDb = objGPack.GetSqlValue(StrSql)
        StrSql = " SELECT 'EXISTS' FROM MASTER..SYSDATABASES WHERE NAME = '" & CurrentStockDb & "'"
        If objGPack.GetSqlValue(StrSql, , "-1") = "-1" Then
            MsgBox(CurrentStockDb & " Database not found", MsgBoxStyle.Information)
            CurrentStockDb = cnStockDb
            dtpBillDate_OWN.Select()
            Exit Sub
        End If


        Transferno = txttransferno.Text.ToString
        If Transferno.ToString = "" Then
            MsgBox("Record not found.")
            txttransferno.SelectAll()
            Exit Sub
        Else
            txttransferno.Enabled = False
        End If
        ClearTran()

        Try

            Dim index As Integer = Nothing

            For cnt As Integer = 0 To DtTran.Rows.Count - 1
                If DtTran.Rows(cnt).RowState <> DataRowState.Deleted Then
                    If DtTran.Rows(cnt).Item("TRANTYPE").ToString = "" Then
                        index = cnt
                        DtTran.Rows.Add()
                        Exit For
                    End If
                End If
            Next

            StrSql = vbCrLf + "  SELECT "
            StrSql += vbCrLf + "  I.SNO,CASE WHEN ISNULL(CA.DIASTNTYPE,'')<>'' THEN 'T' ELSE 'O' END FLAG "
            StrSql += vbCrLf + "  ,'' AS JOBNO"
            StrSql += vbCrLf + "  ,ME.METALID,ME.METALNAME AS METAL,CA.CATNAME,CA.CATCODE"
            StrSql += vbCrLf + "  , CA.CATNAME AS ISSCATNAME"
            StrSql += vbCrLf + "  ,I.PURITY"
            StrSql += vbCrLf + "  ,IM.ITEMNAME AS ITEM"
            StrSql += vbCrLf + "  ,SM.SUBITEMNAME AS SUBITEM"
            StrSql += vbCrLf + "  ,I.ITEMID,I.SUBITEMID,I.PCS"
            StrSql += vbCrLf + "  ,I.GRSWT GRSWT"
            StrSql += vbCrLf + "  ,I.LESSWT LESSWT"
            StrSql += vbCrLf + "  ,I.NETWT NETWT"
            StrSql += vbCrLf + "  ,'' AS UNIT,'' AS CALCMODE"
            StrSql += vbCrLf + "  ,I.GRSNET,0 WASTPER,0 AS WASTAGE"
            StrSql += vbCrLf + "  ,0 AS ALLOY,0 TOUCH"
            StrSql += vbCrLf + "  ,I.NETWT PUREWT"
            StrSql += vbCrLf + "  ,0 MCGRM,0 AS MC"
            If cmbTransactionType.Text = "PURCHASE RETURN" Then
                StrSql += vbCrLf + "  ,I.RATE RATE"
            Else
                StrSql += vbCrLf + "  ,0 RATE"
            End If
            StrSql += vbCrLf + "  ,0 BOARDRATE,0 AS STUDAMT"
            StrSql += vbCrLf + "  ,0 AS GROSSAMT,0 VATPER"
            StrSql += vbCrLf + "  ,'' AS GSTCATNAME"
            StrSql += vbCrLf + "  ,0 AS SGSTPER"
            StrSql += vbCrLf + "  ,0 AS CGSTPER"
            StrSql += vbCrLf + "  ,0 AS IGSTPER"
            StrSql += vbCrLf + "  ,0 AS SGST"
            StrSql += vbCrLf + "  ,0 AS CGST"
            StrSql += vbCrLf + "  ,0 AS IGST"
            StrSql += vbCrLf + "  ,0 AS VAT"
            StrSql += vbCrLf + "  ,'' TDSCATID "
            StrSql += vbCrLf + "  ,'' AS TDSCATNAME "
            StrSql += vbCrLf + "  ,'' AS TDSPER "
            StrSql += vbCrLf + "  ,0 AS TDS,'' REMARK1,'' REMARK2,'' TRANSPORTMODE"
            StrSql += vbCrLf + "  ,'' AS ORDSTATE_NAME,ISNULL(CA.DIASTNTYPE,'')DIASTNTYPE"
            StrSql += vbCrLf + "  ,0 AS ADDCHARGE,'' AS SEIVE,ISNULL((SELECT GROUPNAME FROM " & cnAdminDb & "..STONEGROUP WHERE GROUPID = I.STNGRPID),'')STNGRPNAME"
            StrSql += vbCrLf + "  ,CUTID AS CUT,COLORID AS COLOR,CLARITYID AS CLARITY,SHAPEID AS SHAPE,SETTYPEID AS SETTYPE,HEIGHT,WIDTH"
            StrSql += vbCrLf + "  ,0 SC,'' STKTYPE,0 TRFVALUE,ISNULL(I.TAGNO,'')TAGNO,0 DISCOUNT,0 ED,0 EDTAX,0 TCS,0 APRXAMT,0 APRXTAX,'N' RATEFIXED"
            StrSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMTAG AS I"
            StrSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = I.ITEMID"
            StrSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = IM.CATCODE"
            StrSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..METALMAST AS ME ON ME.METALID = CA.METALID"
            StrSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = I.ITEMID AND SM.SUBITEMID = I.SUBITEMID"
            StrSql += vbCrLf + "  WHERE I.TRANINVNO = '" & Transferno & "' AND I.ISSDATE IS NULL AND ISNULL(I.TOFLAG,'') ='' AND I.COSTID = '" & cmbcostid & "' "

            Dim dtTranInfo As New DataTable
            Da = New OleDbDataAdapter(StrSql, cn)
            Da.Fill(dtTranInfo)
            If Not dtTranInfo.Rows.Count > 0 Then MsgBox("No Record Found", MsgBoxStyle.Information) : btnNew_Click(New Object, New EventArgs) : Exit Sub

            Dim StateId As Integer = Val(objGPack.GetSqlValue("SELECT STATEID FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbAcName.Text & "'").ToString)
            If StateId = 0 Then StateId = CompanyStateId
            If StateId <> CompanyStateId Then
                InterStateBill = True
            End If
            Dim Desc As String = Nothing
            For Each ro As DataRow In dtTranInfo.Rows
                If Val(ro.Item("PCS").ToString) = 0 Then ro.Item("PCS") = DBNull.Value
                If Val(ro.Item("GRSWT").ToString) = 0 Then ro.Item("GRSWT") = DBNull.Value
                If Val(ro.Item("LESSWT").ToString) = 0 Then ro.Item("LESSWT") = DBNull.Value
                If Val(ro.Item("NETWT").ToString) = 0 Then ro.Item("NETWT") = DBNull.Value
                If Val(ro.Item("WASTPER").ToString) = 0 Then ro.Item("WASTPER") = DBNull.Value
                If Val(ro.Item("WASTAGE").ToString) = 0 Then ro.Item("WASTAGE") = DBNull.Value
                If Val(ro.Item("ALLOY").ToString) = 0 Then ro.Item("ALLOY") = DBNull.Value
                If Val(ro.Item("TOUCH").ToString) = 0 Then ro.Item("TOUCH") = DBNull.Value
                If Val(ro.Item("PUREWT").ToString) = 0 Then ro.Item("PUREWT") = DBNull.Value
                If Val(ro.Item("MCGRM").ToString) = 0 Then ro.Item("MCGRM") = DBNull.Value
                If Val(ro.Item("MC").ToString) = 0 Then ro.Item("MC") = DBNull.Value
                If Val(ro.Item("RATE").ToString) = 0 Then ro.Item("RATE") = DBNull.Value
                If Val(ro.Item("GROSSAMT").ToString) = 0 Then ro.Item("GROSSAMT") = DBNull.Value
                If Val(ro.Item("VATPER").ToString) = 0 Then ro.Item("VATPER") = DBNull.Value
                If Val(ro.Item("VAT").ToString) = 0 Then ro.Item("VAT") = DBNull.Value
                If Val(ro.Item("ED").ToString) = 0 Then ro.Item("ED") = DBNull.Value
                If Val(ro.Item("EDTAX").ToString) = 0 Then ro.Item("EDTAX") = DBNull.Value
                If Val(ro.Item("TCS").ToString) = 0 Then ro.Item("TCS") = DBNull.Value
                If Val(ro.Item("DISCOUNT").ToString) = 0 Then ro.Item("DISCOUNT") = DBNull.Value
                If Val(ro.Item("APRXAMT").ToString) = 0 Then ro.Item("APRXAMT") = DBNull.Value
                If Val(ro.Item("CUT").ToString) = 0 Then ro.Item("CUT") = DBNull.Value
                If Val(ro.Item("COLOR").ToString) = 0 Then ro.Item("COLOR") = DBNull.Value
                If Val(ro.Item("CLARITY").ToString) = 0 Then ro.Item("CLARITY") = DBNull.Value
                If Val(ro.Item("SHAPE").ToString) = 0 Then ro.Item("SHAPE") = DBNull.Value
                If Val(ro.Item("SETTYPE").ToString) = 0 Then ro.Item("SETTYPE") = DBNull.Value
                If Val(ro.Item("HEIGHT").ToString) = 0 Then ro.Item("HEIGHT") = DBNull.Value
                If Val(ro.Item("WIDTH").ToString) = 0 Then ro.Item("WIDTH") = DBNull.Value
                If TdsPer <> 0 Then ro.Item("VATPER") = TdsPer
                ObjMaterialDia = New MaterialIssRec(OMaterialType, cmbTransactionType.Text, dtpTrandate.Value, _Accode)
                ObjMaterialDia.BillCostId = CostCenterId
                ObjMaterialDia.Text = "MATERIAL " & cmbTransactionType.Text
                With ObjMaterialDia
                    If ro.Item("FLAG").ToString = "O" Then
                        StrSql = "  SELECT  "
                        StrSql += vbCrLf + "  I.METALID,I.ITEMNAME AS ITEM,S.SUBITEMNAME AS SUBITEM,T.STNPCS AS PCS,T.STNWT AS WEIGHT"
                        StrSql += vbCrLf + "  ,T.STONEUNIT AS UNIT,T.CALCMODE AS CALC"
                        'If cmbTransactionType.Text = "PURCHASE RETURN" Then
                        '    StrSql += vbCrLf + "  ,T.STNRATE AS RATE ,T.STNAMT AS AMOUNT"
                        'Else
                        '    StrSql += vbCrLf + "  ,0 AS RATE,0 AS AMOUNT"
                        'End If
                        If cmbTransactionType.Text = "PURCHASE RETURN" Then
                            StrSql += vbCrLf + "  ,T.STNRATE AS RATE ,T.STNAMT AS AMOUNT,'PURCHASE RETURN' STNTYPE"
                        ElseIf cmbTransactionType.Text = "APPROVAL ISSUE" Then
                            StrSql += vbCrLf + "  ,0 AS RATE,0 AS AMOUNT,'APPROVAL' STNTYPE"
                        ElseIf cmbTransactionType.Text = "ISSUE" Then
                            StrSql += vbCrLf + "  ,0 AS RATE,0 AS AMOUNT,'ISSUE' STNTYPE"
                        Else
                            StrSql += vbCrLf + "  ,0 AS RATE,0 AS AMOUNT,'ISSUE' STNTYPE"
                        End If
                        StrSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMTAGSTONE AS T "
                        StrSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS I ON I.ITEMID=T.STNITEMID"
                        StrSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS S ON S.SUBITEMID=T.STNSUBITEMID"
                        StrSql += vbCrLf + "  WHERE T.TAGSNO = '" & ro.Item("SNO").ToString & "'"
                        StrSql += vbCrLf + "  AND T.COMPANYID='" & strCompanyId & "'"
                        StrSql += vbCrLf + "  AND ISNULL(T.COSTID,'')='" & cnCostId & "'"

                        Dim dtStone As New DataTable
                        Da = New OleDbDataAdapter(StrSql, cn)
                        Da.Fill(dtStone)

                        For Each RoStn As DataRow In dtStone.Rows
                            Dim RoStnIm As DataRow = .objStone.dtGridStone.NewRow
                            For Each Col As DataColumn In dtStone.Columns
                                RoStnIm.Item(Col.ColumnName) = RoStn.Item(Col.ColumnName)
                            Next
                            .objStone.dtGridStone.Rows.Add(RoStnIm)
                        Next
                        .objStone.dtGridStone.AcceptChanges()
                        .objStone.CalcStoneWtAmount()


                        DtTran.Rows(index).Item("ROWINDEX") = index

                        DtTran.Rows(index).Item("METISSREC") = ObjMaterialDia
                        .rbtOrnament.Checked = True
                        If _JobNoEnable = False Then
                            .txtOOrdNo.Text = ro.Item("JOBNO").ToString
                        Else
                            .txtOOrdNo.Text = ro.Item("JOBNO").ToString.Replace((GetCostId(cnCostId) & GetCompanyId(strCompanyId)), "")
                        End If

                        .cmbOMetal.Text = ro.Item("METAL").ToString
                        .cmbOMetal.SelectedValue = ro.Item("METALID").ToString
                        .cmbOMetal_SelectedValueChanged(Me, New EventArgs)

                        .cmbOCategory.Text = ro.Item("CATNAME").ToString
                        .cmbOCategory.SelectedValue = ro.Item("CATCODE").ToString
                        .cmbOCategory_SelectedValueChanged(Me, New EventArgs)

                        .cmbOIssuedCategory.Text = "" & ro.Item("ISSCATNAME")
                        .cmbOAcPostCategory.Text = "" & ro.Item("CATNAME")
                        .CmbOPurity.Text = ro.Item("PURITY").ToString
                        .cmbOItem.Text = ro.Item("ITEM").ToString
                        .cmbOItem.SelectedValue = ro.Item("ITEMID")
                        .cmbOItem_SelectedValueChanged(Me, New EventArgs)

                        .cmbOSubItem.Text = ro.Item("SUBITEM").ToString
                        .txtOPcs_NUM.Text = ro.Item("PCS").ToString
                        .txtOGrsWt_WET.Text = ro.Item("GRSWT").ToString
                        .txtONetWt_WET.Text = ro.Item("NETWT").ToString
                        .cmbOGrsNet.Text = IIf(ro.Item("GRSNET").ToString = "G", "GRS WT", "NET WT")
                        .txtOWastPer.Text = ro.Item("WASTPER").ToString
                        .txtOWast_WET.Text = ro.Item("WASTAGE").ToString
                        .txtOalloy_WET.Text = ro.Item("ALLOY").ToString
                        .txtOTouchAMT.Text = ro.Item("TOUCH").ToString
                        If .objStone.dtGridStone.Rows.Count > 0 Then
                            Dim stnWt As Double = Val(.objStone.gridStoneTotal.Rows(0).Cells("WEIGHT").Value.ToString)
                            Dim stnAmt As Double = Val(.objStone.gridStoneTotal.Rows(0).Cells("AMOUNT").Value.ToString)
                            .txtOLessWt_WET.Text = IIf(stnWt <> 0, Format(stnWt, "0.000"), Nothing)
                            .txtOStudAmt_AMT.Text = IIf(stnAmt <> 0, Format(stnAmt, "0.00"), Nothing)
                        Else
                            .txtOLessWt_WET.Text = ro.Item("LESSWT").ToString
                            .txtOStudAmt_AMT.Text = ""
                        End If

                        .txtOPureWt_WET.Text = ro.Item("PUREWT").ToString
                        .txtOMcGrm_AMT.Text = ro.Item("MCGRM").ToString
                        .txtOMc_AMT.Text = ro.Item("MC").ToString
                        .txtORate_OWN.Text = ro.Item("RATE").ToString
                        .oBoardRate = Val(ro.Item("BOARDRATE").ToString)
                        .txtOAddlCharge_AMT.Text = ro.Item("ADDCHARGE").ToString
                        .txtOGrsAmt_AMT.Text = ro.Item("GROSSAMT").ToString
                        .txtOVatPer_PER.Text = ro.Item("VATPER").ToString
                        .txtODisc.Text = ro.Item("DISCOUNT").ToString

                        If Val(ro.Item("TDS").ToString) > 0 Then
                            .txtOVat_AMT.Text = ro.Item("TDS").ToString
                        Else
                            .txtOVat_AMT.Text = ro.Item("VAT").ToString
                            If UCase(cmbTransactionType.Text) <> "PURCHASE RETURN" And UCase(cmbTransactionType.Text) <> "INTERNAL TRANSFER" And UCase(cmbTransactionType.Text) <> "APPROVAL RECEIPT" Then
                                .txtOVat_AMT.Text = 0
                            End If
                        End If
                        If Val(ro.Item("SGST").ToString) > 0 Then
                            .txtOSG_AMT.Text = ro.Item("SGST").ToString
                            .txtOCG_AMT.Text = ro.Item("CGST").ToString
                            .txtOSgst_AMT.Text = ro.Item("SGSTPER").ToString
                            .txtOCgst_AMT.Text = ro.Item("CGSTPER").ToString
                        ElseIf Val(ro.Item("IGST").ToString) > 0 Then
                            .txtOIG_AMT.Text = ro.Item("IGST").ToString
                            .txtOIgst_AMT.Text = ro.Item("IGSTPER").ToString
                        End If

                        .txtOED_AMT.Text = ro.Item("ED").ToString
                        .txtOTCS_AMT.Text = ro.Item("TCS").ToString
                        .txtOedPer_AMT.Text = ro.Item("EDTAX").ToString

                        If cmbTransactionType.Text = "PURCHASE RETURN" Then
                            .CalcOGrossAmt()
                        End If

                        .CalcONetAmt()
                        If ro.Item("TAGNO").ToString <> "" Then
                            .txtTagNo.Text = ro.Item("TAGNO").ToString
                        End If
                        .txtORemark1.Text = ro.Item("REMARK1").ToString
                        .txtORemark2.Text = ro.Item("REMARK2").ToString

                        '.txtOVat_AMT.Text = Ro.Item("VAT").ToString
                        .cmbOcalcon.Text = IIf(ro.Item("GRSNET").ToString = "G", "GRS WT", "NET WT")
                        .txtOedPer_AMT.Text = ro.Item("EDTAX").ToString
                        .txtOED_AMT.Text = ro.Item("ED").ToString
                        .txtOVatPer_PER.Text = ro.Item("VATPER").ToString
                        '.txtOVat_AMT.Text = Ro.Item("VAT").ToString
                        If Val(ro.Item("TDS").ToString) > 0 Then
                            .txtOVat_AMT.Text = ro.Item("TDS").ToString
                        Else
                            .txtOVat_AMT.Text = ro.Item("VAT").ToString
                            If UCase(cmbTransactionType.Text) <> "PURCHASE RETURN" And UCase(cmbTransactionType.Text) <> "INTERNAL TRANSFER" And UCase(cmbTransactionType.Text) <> "APPROVAL RECEIPT" Then
                                .txtOVat_AMT.Text = 0
                            End If
                        End If
                        .txtOAprxAmount_AMT.Text = ro.Item("APRXAMT").ToString
                        .txtOAprxTaxAmt_AMT.Text = ro.Item("APRXTAX").ToString
                        If ro.Item("RATEFIXED").ToString = "Y" Then
                            .chkORateFixed.Checked = True
                        Else
                            .chkORateFixed.Checked = False
                        End If

                        If Val(.txtOalloy_WET.Text) <> 0 Then DgvTran.Columns("ALLOY").Visible = True

                    Else
                        DtTran.Rows(index).Item("ROWINDEX") = index
                        .oEditRowIndex = index
                        DtTran.Rows(index).Item("METISSREC") = ObjMaterialDia
                        .rbtStone.Checked = True
                        If _JobNoEnable = False Then
                            .txtSOrdNo.Text = ro.Item("JOBNO").ToString
                        Else
                            .txtSOrdNo.Text = ro.Item("JOBNO").ToString.Replace((GetCostId(cnCostId) & GetCompanyId(strCompanyId)), "")
                        End If

                        .cmbSMetal.Text = ro.Item("METAL").ToString
                        .cmbSMetal.SelectedValue = ro.Item("METALID").ToString
                        .cmbSMetal_SelectedValueChanged(Me, New EventArgs)

                        .cmbSCategory.Text = ro.Item("CATNAME").ToString
                        .cmbSCategory.SelectedValue = ro.Item("CATCODE").ToString
                        .cmbSCategory_SelectedValueChanged(Me, New EventArgs)

                        .cmbSIssuedCategory.Text = "" & ro.Item("ISSCATNAME")
                        .cmbSAcPostCategory.Text = "" & ro.Item("CATNAME")
                        .CmbSPurity.Text = ro.Item("PURITY").ToString
                        .cmbSItem.Text = ro.Item("ITEM").ToString
                        .cmbSItem.SelectedValue = ro.Item("ITEMID")
                        .cmbSItem_SelectedValueChanged(Me, New EventArgs)

                        .cmbSSubItem.Text = ro.Item("SUBITEM").ToString
                        .txtSPcs_NUM.Text = ro.Item("PCS").ToString
                        .txtSGrsWt_WET.Text = ro.Item("GRSWT").ToString
                        .txtSGrsWt_WET.Text = ro.Item("NETWT").ToString
                        .cmbSUnit.Text = ro.Item("UNIT").ToString
                        .cmbSCalcMode.Text = ro.Item("CALCMODE").ToString
                        .txtSGrsWt_WET.Text = ro.Item("PUREWT").ToString
                        .txtSRate_OWN.Text = ro.Item("RATE").ToString
                        .oBoardRate = Val(ro.Item("BOARDRATE").ToString)
                        .txtSAddlCharge_AMT.Text = ro.Item("ADDCHARGE").ToString
                        .txtSGrsAmt_AMT.Text = ro.Item("GROSSAMT").ToString
                        .txtSVatPer_PER.Text = ro.Item("VATPER").ToString
                        .txtStnGrpName.Text = ro.Item("STNGRPNAME").ToString
                        '.txtSVat_AMT.Text = Ro.Item("VAT").ToString
                        If Val(ro.Item("TDS").ToString) > 0 Then
                            .txtSVat_AMT.Text = ro.Item("TDS").ToString
                        Else
                            .txtSVat_AMT.Text = ro.Item("VAT").ToString
                            If UCase(cmbTransactionType.Text) <> "PURCHASE RETURN" And UCase(cmbTransactionType.Text) <> "INTERNAL TRANSFER" And UCase(cmbTransactionType.Text) <> "APPROVAL RECEIPT" Then
                                .txtSVat_AMT.Text = 0
                            End If
                        End If
                        If Val(ro.Item("SGST").ToString) > 0 Then
                            .txtSSG_AMT.Text = ro.Item("SGST").ToString
                            .txtSCG_AMT.Text = ro.Item("CGST").ToString
                            .txtSSgst_WET.Text = ro.Item("SGSTPER").ToString
                            .txtSCgst_WET.Text = ro.Item("CGSTPER").ToString
                        ElseIf Val(ro.Item("IGST").ToString) > 0 Then
                            .txtSIG_AMT.Text = ro.Item("IGST").ToString
                            .txtSIgst_WET.Text = ro.Item("IGSTPER").ToString
                        End If

                        .txtSTCS_AMT.Text = ro.Item("TCS").ToString

                        .CalcSNetAmt()
                        If ro.Item("TAGNO").ToString <> "" Then
                            .txtTagNo.Text = ro.Item("TAGNO").ToString
                        End If
                        .txtSRemark1.Text = ro.Item("REMARK1").ToString
                        .txtSRemark2.Text = ro.Item("REMARK2").ToString

                        .cmbSProcess.Text = ro.Item("ORDSTATE_NAME").ToString
                        .cmbSSeive.Text = ro.Item("SEIVE").ToString
                        If ro.Item("RATEFIXED").ToString = "Y" Then
                            .chkSRateFixed.Checked = True
                        Else
                            .chkSRateFixed.Checked = False
                        End If
                    End If
                    DgvTran.AutoResizeRow(index)
                    LoadTransaction(ObjMaterialDia)
                    index = index + 1
                End With
            Next
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try
        txtEstNo_NUM.Enabled = True
        btnNew.Enabled = True
        btnSave.Enabled = True
    End Sub

    Private Sub txttransferno_KeyDown(sender As Object, e As KeyEventArgs) Handles txttransferno.KeyDown
        If e.KeyCode = Keys.Enter Then
            Exit Sub
            If txttransferno.Text <> "" Then
                cmbTransactionType.Text = "ISSUE"
                loadtransferdata()
                Exit Sub
            End If

            If GST Then
                Dim GstDate As Date = GetAdmindbSoftValue("GSTDATE", "")
                If dtpTrandate.Value >= GstDate Then
                    GstFlag = True
                Else
                    GstFlag = False
                End If
            Else
                GstFlag = False
            End If

            If MIMR_VALIDATEBILLNO = True And Val(txtBillNo.Text) > 0 And cmbAcName.Text <> "" Then
                If validatebillno() = True Then
                    MsgBox("Bill No Already Exist for this Account", MsgBoxStyle.Information)
                    txtBillNo.Select()
                    Exit Sub
                End If
            End If

            If GstFlag And (UCase(cmbTransactionType.Text) = "PURCHASE RETURN" Or
                         UCase(cmbTransactionType.Text) = "PURCHASE" Or
                         UCase(cmbTransactionType.Text) = "PURCHASE[APPROVAL]" Or
                         UCase(cmbTransactionType.Text) = "INTERNAL TRANSFER" Or
                         UCase(cmbTransactionType.Text) = "APPROVAL RECEIPT") Then
                Dim StateId As Integer = Val(objGPack.GetSqlValue("SELECT STATEID FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbAcName.Text & "'").ToString)
                If StateId = 0 Then
                    MsgBox("Please Update State for the Account [" & cmbAcName.Text & "]", MsgBoxStyle.Information)
                    Exit Sub
                End If
            End If


            If MRMI_LOCK_BILLNODATE = False And (cmbTransactionType.Text = "PURCHASE" Or cmbTransactionType.Text = "PURCHASE[APPROVAL]" Or cmbTransactionType.Text = "PURCHASE RETURN") And txtBillNo.Text = "" Then
                MsgBox("BillNo Should not Empty", MsgBoxStyle.Information)
                txtBillNo.Select()
                Exit Sub
            End If
            _Accode = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbAcName.Text & "'")
            Dim OrderSnos As String = ""
            If cmbTransactionType.Text = "PURCHASE[APPROVAL]" Then
                Dim dtMat As New DataTable
                dtMat = CType(DgvTran.DataSource, DataTable)
                For Each ro As DataRow In dtMat.Rows
                    If ro.Item("TRANTYPE").ToString = "" Then Exit For
                    If ro.Item("ORSNO").ToString <> "" Then
                        OrderSnos += "'" & ro.Item("ORSNO").ToString & "',"
                    End If
                Next
                If OrderSnos <> "" Then
                    OrderSnos = Mid(OrderSnos, 1, OrderSnos.Length - 1)
                End If
            End If
            ObjMaterialDia = New MaterialIssRec(OMaterialType, cmbTransactionType.Text, dtpTrandate.Value, _Accode, OrderSnos, CostCenterId, , SRVTPER, GstFlag)
            ObjMaterialDia.Text = "MATERIAL " & cmbTransactionType.Text
            ObjMaterialDia.Remark1 = txtRemark1.Text
            ObjMaterialDia.Remark2 = txtRemark2.Text
            ObjMaterialDia.BillCostId = CostCenterId

            ObjMaterialDia.OProcessname = xOProcessname
            ObjMaterialDia.OMetalname = xOMetalname
            ObjMaterialDia.OCategoryname = xOCategoryname
            ObjMaterialDia.OCategoryname = xOCategoryname
            ObjMaterialDia.OIssCatName = xOIssCatName
            ObjMaterialDia.OAcpostCatname = xOAcpostCatname
            ObjMaterialDia.Opurityper = xOpurityper
            ObjMaterialDia.GstFlag = GstFlag
            Dim Tags() As String = TagNos.Split(",")
            Dim Catname() As String = CatNames.Split(",")
            For i As Integer = 0 To Tags.Length - 1
                ObjMaterialDia.ListTagNos.Add(Tags(i).ToString)
                ObjMaterialDia.arrTagNos(i, 0) = ""
                ObjMaterialDia.arrTagNos(i, 1) = ""
                ObjMaterialDia.arrTagNos(i, 0) = Tags(i).ToString
                ObjMaterialDia.arrTagNos(i, 1) = Catname(i).ToString
            Next

            If MIMR_APPROXVALUE = True And (cmbTransactionType.Text = "ISSUE" Or cmbTransactionType.Text = "RECEIPT") Then
                ObjMaterialDia.txtOAprxAmount_AMT.Visible = True
                ObjMaterialDia.txtOAprxTaxAmt_AMT.Visible = True
                ObjMaterialDia.txtMAprxAmount_AMT.Visible = True
                ObjMaterialDia.txtMAprxTaxAmt_AMT.Visible = True
                ObjMaterialDia.lblOApproxAmt.Visible = True
                ObjMaterialDia.lblOApproxTax.Visible = True
                ObjMaterialDia.lblMApproxAmt.Visible = True
                ObjMaterialDia.lblMApproxTax.Visible = True
            Else
                ObjMaterialDia.txtOAprxAmount_AMT.Visible = False
                ObjMaterialDia.txtOAprxTaxAmt_AMT.Visible = False
                ObjMaterialDia.txtMAprxAmount_AMT.Visible = False
                ObjMaterialDia.txtMAprxTaxAmt_AMT.Visible = False
                ObjMaterialDia.lblOApproxAmt.Visible = False
                ObjMaterialDia.lblOApproxTax.Visible = False
                ObjMaterialDia.lblMApproxAmt.Visible = False
                ObjMaterialDia.lblMApproxTax.Visible = False
            End If

            If ObjMaterialDia.ShowDialog = Windows.Forms.DialogResult.OK Then
                LoadTransaction(ObjMaterialDia)
                If txtRemark1.Text = "" Then txtRemark1.Text = ObjMaterialDia.Remark1
                If txtRemark2.Text = "" Then txtRemark2.Text = ObjMaterialDia.Remark2
                If ObjMaterialDia.rbtOrnament.Checked Then
                    If xOProcessname = Nothing Then xOProcessname = ObjMaterialDia.cmbOProcess.Text
                    If xOCategoryname = Nothing Then xOCategoryname = ObjMaterialDia.cmbOCategory.Text
                    If xOIssCatName = Nothing Then xOIssCatName = ObjMaterialDia.cmbOIssuedCategory.Text
                    If xOAcpostCatname = Nothing Then xOAcpostCatname = ObjMaterialDia.cmbOAcPostCategory.Text
                    If xOMetalname = Nothing Then xOMetalname = ObjMaterialDia.cmbOMetal.Text
                    If xOpurityper = Nothing Then xOpurityper = ObjMaterialDia.CmbOPurity.Text
                ElseIf ObjMaterialDia.rbtMetal.Checked Then
                    If xMProcessname = Nothing Then xMProcessname = ObjMaterialDia.cmbMProcess.Text
                    If xMcategoryname = Nothing Then xMcategoryname = ObjMaterialDia.cmbMCategory.Text
                    If xMIsscatName = Nothing Then xMIsscatName = ObjMaterialDia.cmbMIssuedCategory.Text
                    If xMAcPostcatname = Nothing Then xMAcPostcatname = ObjMaterialDia.cmbOAcPostCategory.Text
                    If xMmetalName = Nothing Then xMmetalName = ObjMaterialDia.cmbMMetal.Text
                    If xMpurityper = Nothing Then xMpurityper = ObjMaterialDia.CmbMPurity.Text
                End If
            End If
        ElseIf e.KeyCode = Keys.Escape Then
            If cmbTransactionType.DroppedDown Then Exit Sub
            txtAdjCash_AMT.Select()
        End If
    End Sub

    Private Sub cmbCostCentre_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCostCentre.Leave
        If cmbCostCentre.Enabled = False Then Exit Sub
        CostCenterId = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre.Text & "'", , , tran)
        If Lotautopost_CostId <> "" Then
            Lotautopost = IIf(GetAdmindbSoftValue("ACC_LOTAUTOPOST", "N") = "Y", True, False)
            If Lotautopost_CostId <> CostCenterId Then
                Lotautopost = False
            Else
                Lotautopost = True
            End If
        End If
        If cmbCostCentre.Text <> "System.Data.DataRowView" And cmbCostCentre.Text <> "" And CostCenterId = "" Then MsgBox("Please Select Valid Costcentre", MsgBoxStyle.Information) : cmbCostCentre.Text = "" : cmbCostCentre.Focus() : Exit Sub
    End Sub
    Private Sub chkMulti_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles chkMulti.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Or e.KeyChar = Chr(Keys.Escape) Then gboxBulk.Visible = False : txtAdjCash_AMT.Focus()
    End Sub

    Private Sub chkBulk_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles chkBulk.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then chkMulti.Focus()
        If e.KeyChar = Chr(Keys.Escape) Then gboxBulk.Visible = False : txtAdjCash_AMT.Focus()
    End Sub

    Private Sub dtpTrandate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtpTrandate.KeyDown
        If e.KeyCode = Keys.Enter Then
            LoadBalanceWt()
        End If
    End Sub

    Private Sub chkOrder_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles chkOrder.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then txtRemark1.Focus()
    End Sub

    Private Sub dtpTrandate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtpTrandate.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then LoadBalanceWt()
    End Sub

    Private Sub dgvOrderDet_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgvOrderDet.KeyDown
        If e.KeyCode = Keys.Delete Then
            If dgvOrderDet.Rows.Count > 0 Then
                dgvOrderDet.Rows.RemoveAt(dgvOrderDet.CurrentRow.Index)
                MdtOdDet.AcceptChanges()
            End If
        End If
    End Sub

    Private Sub txtOrdNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtOrdNo.KeyDown
        If e.KeyCode = Keys.Insert Then
            StrSql = vbCrLf + " SELECT * FROM ( "
            StrSql += vbCrLf + " SELECT SUBSTRING(OM.ORNO,6,20)ORNO,OM.STYLENO"
            StrSql += vbCrLf + " ,IM.ITEMNAME AS ITEM,SM.SUBITEMNAME AS SUBITEM"
            StrSql += vbCrLf + " ,OM.PCS-ISNULL((SELECT SUM(isnull(PCS,0)) FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORSNO = OM.SNO AND ORSTATUS = 'R' AND ISNULL(CANCEL,'')='' AND ORDSTATE_ID<>54),0) PCS"
            StrSql += vbCrLf + " ,OM.GRSWT-ISNULL((SELECT SUM(isnull(GRSWT,0)) FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORSNO = OM.SNO AND ORSTATUS = 'R' AND ISNULL(CANCEL,'')='' AND ORDSTATE_ID<>54),0) GRSWT "
            StrSql += vbCrLf + " ,OM.NETWT-ISNULL((SELECT SUM(isnull(NETWT,0)) FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORSNO = OM.SNO AND ORSTATUS = 'R' AND ISNULL(CANCEL,'')='' AND ORDSTATE_ID<>54),0) NETWT"
            StrSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ORSTONE WHERE ORSNO = OM.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAPCS"
            StrSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ORSTONE WHERE ORSNO = OM.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAWT"
            StrSql += vbCrLf + " ,OM.SNO,OM.WAST WASTAGE,OM.MC"
            StrSql += vbCrLf + " ,CASE WHEN ISNULL(OM.ODBATCHNO,'') <> '' THEN 'DELIVERED'"
            StrSql += vbCrLf + " WHEN ISNULL(OM.ORDCANCEL,'') <> '' THEN 'CANCELLED'"
            StrSql += vbCrLf + " ELSE ISNULL((SELECT ORDSTATE_NAME FROM " & cnAdminDb & "..ORDERSTATUS "
            StrSql += vbCrLf + " WHERE ORDSTATE_ID = (SELECT TOP 1 ORDSTATE_ID FROM " & cnAdminDb & "..ORIRDETAIL "
            StrSql += vbCrLf + " WHERE ORSNO = OM.SNO AND ISNULL(CANCEL,'') = '' ORDER BY ENTRYORDER DESC)),'PENDING WITH US') "
            StrSql += vbCrLf + " END AS STATUS"
            StrSql += vbCrLf + " FROM " & cnAdminDb & "..ORMAST AS OM"
            StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = OM.ITEMID"
            StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = OM.ITEMID AND SM.SUBITEMID = OM.SUBITEMID "
            StrSql += vbCrLf + " WHERE 1=1"
            StrSql += vbCrLf + " AND ISNULL(OM.CANCEL,'') = '' AND ISNULL(OM.ORDCANCEL,'') = ''"
            StrSql += vbCrLf + " )X WHERE GRSWT > 0  "
            StrSql += vbCrLf + " AND STATUS NOT IN('RECEIVED FROM SMITH','PENDING WITH US')  "
            Dim dtStk As New DataTable
            If StrSql = "" Then txtOrdNo.SelectAll() : Exit Sub
            Cmd = New OleDbCommand(StrSql, cn)
            Da = New OleDbDataAdapter(Cmd)
            Da.Fill(dtStk)
            Dim StkRow As DataRow
            If dtStk.Rows.Count > 0 Then
                StkRow = BrighttechPack.SearchDialog.Show_R("Find ", StrSql, cn, , 0)
                If Not StkRow Is Nothing Then
                    txtOrdNo.Text = StkRow.Item("ORNO").ToString
                End If
            Else
                MsgBox("Record Not Found", MsgBoxStyle.Information)
            End If
        ElseIf e.KeyCode = Keys.Escape Then
            txtAdjCash_AMT.Focus()
        End If
    End Sub

    Private Sub txtOrdNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtOrdNo.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtOrdNo.Text <> "" Then
                Dim Acc As String = ""
                StrSql = " SELECT ACCODE FROM " & cnStockDb & "..ISSUE WHERE BATCHNO IN(SELECT BATCHNO FROM " & cnAdminDb & "..ORIRDETAIL WHERE SUBSTRING(ORNO,6,20) = '" & txtOrdNo.Text & "')"
                Acc = objGPack.GetSqlValue(StrSql, "ACCODE", "").ToString
                StrSql = "SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE='" & Acc & "'"
                cmbAcName.Text = objGPack.GetSqlValue(StrSql, "ACNAME", "").ToString
                ObjMaterialDia = New MaterialIssRec(OMaterialType, cmbTransactionType.Text, dtpTrandate.Value, Acc, , CostCenterId, txtOrdNo.Text)
                ObjMaterialDia.Text = "MATERIAL " & cmbTransactionType.Text
                ObjMaterialDia.txtOOrdNo.Text = txtOrdNo.Text
                If ObjMaterialDia.ShowDialog = Windows.Forms.DialogResult.OK Then
                    LoadTransaction(ObjMaterialDia)
                    If txtRemark1.Text = "" Then txtRemark1.Text = ObjMaterialDia.Remark1
                    If txtRemark2.Text = "" Then txtRemark2.Text = ObjMaterialDia.Remark2
                    If ObjMaterialDia.rbtOrnament.Checked Then
                        If xOProcessname = Nothing Then xOProcessname = ObjMaterialDia.cmbOProcess.Text
                        If xOCategoryname = Nothing Then xOCategoryname = ObjMaterialDia.cmbOCategory.Text
                        If xOIssCatName = Nothing Then xOIssCatName = ObjMaterialDia.cmbOIssuedCategory.Text
                        If xOAcpostCatname = Nothing Then xOAcpostCatname = ObjMaterialDia.cmbOAcPostCategory.Text
                        If xOMetalname = Nothing Then xOMetalname = ObjMaterialDia.cmbOMetal.Text
                        If xOpurityper = Nothing Then xOpurityper = ObjMaterialDia.CmbOPurity.Text
                    ElseIf ObjMaterialDia.rbtMetal.Checked Then
                        If xMProcessname = Nothing Then xMProcessname = ObjMaterialDia.cmbMProcess.Text
                        If xMcategoryname = Nothing Then xMcategoryname = ObjMaterialDia.cmbMCategory.Text
                        If xMIsscatName = Nothing Then xMIsscatName = ObjMaterialDia.cmbMIssuedCategory.Text
                        If xMAcPostcatname = Nothing Then xMAcPostcatname = ObjMaterialDia.cmbOAcPostCategory.Text
                        If xMmetalName = Nothing Then xMmetalName = ObjMaterialDia.cmbMMetal.Text
                        If xMpurityper = Nothing Then xMpurityper = ObjMaterialDia.CmbMPurity.Text
                    End If
                End If
            End If
        End If
    End Sub

End Class