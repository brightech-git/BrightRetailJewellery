Imports System.ComponentModel
Imports System.Data.OleDb
Public Class frmMIMR

#Region " Enum"
    'Public Enum Material
    '    Issue = 0
    '    Receipt = 1
    'End Enum
#End Region

#Region " SoftControl Vaiable"
    Dim InclCusttype As String = GetAdmindbSoftValue("INCL_CUSTOMER_ISSREC", "N")
    Private ROUNDOFF_ACC As String = GetAdmindbSoftValue("ROUNDOFF-ACC", "N").ToUpper
    Private ROUNDOFF_ACC_TDS As String = GetAdmindbSoftValue("ROUNDOFF-ACC-TDS", "N").ToUpper
    Private PURWTPERACC As String = GetAdmindbSoftValue("PURWTPER-ACC", "100")
    Private ROUNDOFF_WT() As String = GetAdmindbSoftValue("ROUNDOFF-MRMI", "0,0,0,0").Split(",")
    Private ROUNDOFF_PWT As String = GetAdmindbSoftValue("ROUNDOFF-PUREWT", "")
    Private GRSNETCAL As String = GetAdmindbSoftValue("ACC_GROSSNET", "B")
    Private D_VA_DATAENABLE As Boolean = IIf(GetAdmindbSoftValue("D_VA_DATAENABLE", "Y").ToUpper = "Y", True, False)
    Private ACC_STONEISSCAT As Boolean = IIf(GetAdmindbSoftValue("ACC_STONEISSCAT", "N").ToUpper = "Y", True, False)
    Private ACC_STUDDEDUCT_OPTIONAL As Boolean = IIf(GetAdmindbSoftValue("ACC_STUDDEDUCT_OPTIONAL", "N").ToUpper = "Y", True, False)
    Dim MRMIAPPACCPOST As Boolean = IIf(GetAdmindbSoftValue("MRMIAPPACCPOST", "Y") = "Y", True, False)
    Dim CatbaseRecIsstranno As Boolean = IIf(GetAdmindbSoftValue("ACC_RECISS_NO", "N") = "Y", True, False)
    Dim _AccAudit As Boolean = IIf(GetAdmindbSoftValue("ACC_AUDIT", "N") = "Y", True, False)
    Dim _JobNoEnable As Boolean = IIf(GetAdmindbSoftValue("MRMIJOBNO", "N") = "Y", True, False)
    Dim CstPurchsep As Boolean = IIf(GetAdmindbSoftValue("ACC_CSTPURTAXSEP", "Y") = "Y", True, False)
    Dim SepAccPost As Boolean = IIf(GetAdmindbSoftValue("SEPACCPOST_MRMI", "Y") = "Y", True, False)
    Dim TdsCatId As Integer = Val(GetAdmindbSoftValue("TDS_CAT_MC", 1))
    Dim SEPACCPOST_ITEM As String = GetAdmindbSoftValue("SEPACCPOST_ITEM_MRMI", "DSP")
    Dim TdsAc As String = GetAdmindbSoftValue("TDS_AC", "TDSIN")
    Dim MRMI_VATSEPPOST As Boolean = IIf(GetAdmindbSoftValue("MRMI_VATSEPPOST", "Y") = "Y", True, False)
    Dim strLockCtrl() As String
    Private WASTPERPC As String = ""
    Private MCPERGMPC As String = ""
    Dim cmbOMetal As String = "GOLD"
    Dim txtOalloy_WET As String = "0"
    Dim txtOED_AMT As String = "0"
    Dim txtOOrdNo As String = ""
    Dim txtOalloyper As Decimal = 0
    Dim lblOVat As String = "TDS"
    Dim dealerwmcSno As String = ""
    'Private oMaterial As Material
    Private STONEISSCAT_EDIT As Boolean = False
    Private ACC_STUDDEDUCT_OPTIONAL_EDIT As Boolean = False
    Dim TdsPer As Decimal = Nothing
    Dim SMS_OTP_RATECHANGE_MIMR_MOBILENO As String = GetAdmindbSoftValue("SMS_OTP_RATECHANGE_MIMR_MOBILENO", "").ToString
    Dim MIMRSTOCKCHECKING As String = GetAdmindbSoftValue("MIMRSTOCKCHECKING", "").ToString
    Dim objMIMRCd As New frmMIMRDebitNoteCreditNote
    Dim objMIMRfrmEst As New frmMIMREstDetail()
    Public ESTTAGNO As String = ""
    Public ESTSNO As String = ""
    Public ESTBATCHNO As String = ""
    Public ITEMTAGPCS As Integer = 0
    Public ITEMTAGGRSWT As Double = 0
    Public ITEMTAGNETWT As Double = 0
#End Region

#Region " Variable"
    Public _CashCtr As String = ""
    Dim strsql As String = ""
    Dim EditBatchno As String = ""
    Dim BatchNo As String = Nothing
    Dim _StkType As String = ""
    Dim CostCenterId As String = Nothing
    Dim TranNo As Integer = Nothing
    Dim Transistno As Long = 0
    Dim _Accode As String = Nothing
    Dim TranNoApp As Integer = Nothing
    Dim _Acctype As String = Nothing
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim Tran As OleDbTransaction = Nothing
    Dim dt As New DataTable
    Dim dtAcname As New DataTable
    Dim dtItemName As New DataTable
    Dim dtSubItemName As New DataTable
    Dim dtAcAddress As New DataTable
    Dim DtTran As New DataTable
    Dim dtTranGrandTotal As New DataTable
    Dim objStone As New frmStoneDiaAc
    Dim dtStone As New DataTable
    Dim objGSTTDS As frmGSTGSTTDS
    Dim dtGSTTDS As New DataTable
    Dim objMIMRRate As New MaterialIssRecRateChangeBox
    Dim ManuallyRatechange As Boolean = False
    Dim dtWastageGSt As New DataTable
    Dim objOtherUpdate As New frmMIMRUpdate(GetServerDate(), "")
    Dim PURCHASE_TDS_YNFIRSTTIME As Boolean = True
    Dim objChangeAcname As New frmMIMRAcNameChange(InclCusttype)
    Dim Lotautopost As Boolean = IIf(GetAdmindbSoftValue("ACC_LOTAUTOPOST", "N") = "Y", True, False)
#End Region

#Region " Constructor"
    Public Sub New()
        InitializeComponent()
        GetDatatable()
        objWriter.Close()
    End Sub
#End Region

#Region " Calculation Function"

    Private Sub stonepropint()
        objStone.cmbIssRecCat.Visible = ACC_STONEISSCAT
        objStone.Label1.Visible = ACC_STONEISSCAT
        objStone.lblStud.Visible = ACC_STUDDEDUCT_OPTIONAL
        objStone.CmbStudDeduct.Visible = ACC_STUDDEDUCT_OPTIONAL
        If ACC_STONEISSCAT = False And STONEISSCAT_EDIT = False Then
            objStone.Label26.Left -= objStone.cmbIssRecCat.Width
            objStone.Label57.Left -= objStone.cmbIssRecCat.Width
            objStone.Label58.Left -= objStone.cmbIssRecCat.Width
            objStone.Label59.Left -= objStone.cmbIssRecCat.Width
            objStone.Label60.Left -= objStone.cmbIssRecCat.Width
            objStone.Label61.Left -= objStone.cmbIssRecCat.Width
            objStone.Label51.Left -= objStone.cmbIssRecCat.Width
            If ACC_STUDDEDUCT_OPTIONAL = False Then
                objStone.Label26.Left -= objStone.CmbStudDeduct.Width
                objStone.Label57.Left -= objStone.CmbStudDeduct.Width
                objStone.Label58.Left -= objStone.CmbStudDeduct.Width
                objStone.Label59.Left -= objStone.CmbStudDeduct.Width
                objStone.Label60.Left -= objStone.CmbStudDeduct.Width
                objStone.Label61.Left -= objStone.CmbStudDeduct.Width
                objStone.Label51.Left -= objStone.CmbStudDeduct.Width
                objStone.txtStPcs_NUM.Left -= objStone.CmbStudDeduct.Width
                objStone.txtStWeight.Left -= objStone.CmbStudDeduct.Width
                objStone.txtStRate_AMT.Left -= objStone.CmbStudDeduct.Width
                objStone.txtStAmount_AMT.Left -= objStone.CmbStudDeduct.Width
                objStone.cmbStUnit.Left -= objStone.CmbStudDeduct.Width
                objStone.cmbStCalc.Left -= objStone.CmbStudDeduct.Width
                objStone.CmbSeive.Left -= objStone.CmbStudDeduct.Width
            Else
                objStone.CmbStudDeduct.Left -= objStone.cmbIssRecCat.Width
                objStone.lblStud.Left -= objStone.cmbIssRecCat.Width
            End If
            objStone.txtStPcs_NUM.Left -= objStone.cmbIssRecCat.Width
            objStone.txtStWeight.Left -= objStone.cmbIssRecCat.Width
            objStone.txtStRate_AMT.Left -= objStone.cmbIssRecCat.Width
            objStone.txtStAmount_AMT.Left -= objStone.cmbIssRecCat.Width
            objStone.cmbStUnit.Left -= objStone.cmbIssRecCat.Width
            objStone.cmbStCalc.Left -= objStone.cmbIssRecCat.Width
            objStone.CmbSeive.Left -= objStone.cmbIssRecCat.Width
            objStone.Size = New Size(objStone.Size.Width - objStone.cmbIssRecCat.Width - (IIf(ACC_STUDDEDUCT_OPTIONAL, 0, objStone.CmbStudDeduct.Width)), objStone.Size.Height)
            objStone.gridStone.Size = New Size(objStone.gridStone.Size.Width - objStone.cmbIssRecCat.Width - (IIf(ACC_STUDDEDUCT_OPTIONAL, 0, objStone.CmbStudDeduct.Width)), objStone.gridStone.Size.Height)
            objStone.gridStoneTotal.Size = New Size(objStone.gridStoneTotal.Size.Width - objStone.cmbIssRecCat.Width, objStone.gridStoneTotal.Size.Height)
            STONEISSCAT_EDIT = True
        Else
            If STONEISSCAT_EDIT = False Then
                If ACC_STUDDEDUCT_OPTIONAL_EDIT = False And ACC_STUDDEDUCT_OPTIONAL = False Then
                    objStone.Label26.Left -= objStone.CmbStudDeduct.Width
                    objStone.Label57.Left -= objStone.CmbStudDeduct.Width
                    objStone.Label58.Left -= objStone.CmbStudDeduct.Width
                    objStone.Label59.Left -= objStone.CmbStudDeduct.Width
                    objStone.Label60.Left -= objStone.CmbStudDeduct.Width
                    objStone.Label61.Left -= objStone.CmbStudDeduct.Width
                    objStone.Label51.Left -= objStone.CmbStudDeduct.Width
                    objStone.txtStPcs_NUM.Left -= objStone.CmbStudDeduct.Width
                    objStone.txtStWeight.Left -= objStone.CmbStudDeduct.Width
                    objStone.txtStRate_AMT.Left -= objStone.CmbStudDeduct.Width
                    objStone.txtStAmount_AMT.Left -= objStone.CmbStudDeduct.Width
                    objStone.cmbStUnit.Left -= objStone.CmbStudDeduct.Width
                    objStone.cmbStCalc.Left -= objStone.CmbStudDeduct.Width
                    objStone.CmbSeive.Left -= objStone.CmbStudDeduct.Width
                    ACC_STUDDEDUCT_OPTIONAL_EDIT = True
                    objStone.Size = New Size(objStone.Size.Width - objStone.CmbStudDeduct.Width, objStone.Size.Height)
                    objStone.gridStone.Size = New Size(objStone.gridStone.Size.Width - objStone.CmbStudDeduct.Width, objStone.gridStone.Size.Height)
                    objStone.gridStoneTotal.Size = New Size(objStone.gridStoneTotal.Size.Width - objStone.CmbStudDeduct.Width, objStone.gridStoneTotal.Size.Height)
                Else
                    objStone.Size = New Size(objStone.Size.Width, objStone.Size.Height)
                    objStone.gridStone.Size = New Size(objStone.gridStone.Size.Width, objStone.gridStone.Size.Height)
                    objStone.gridStoneTotal.Size = New Size(objStone.gridStoneTotal.Size.Width, objStone.gridStoneTotal.Size.Height)
                End If
            Else
                objStone.Size = New Size(objStone.Size.Width, objStone.Size.Height)
                objStone.gridStone.Size = New Size(objStone.gridStone.Size.Width, objStone.gridStone.Size.Height)
                objStone.gridStoneTotal.Size = New Size(objStone.gridStoneTotal.Size.Width, objStone.gridStoneTotal.Size.Height)
            End If
        End If
    End Sub

    Private Function MaterialValue() As Integer
        If cmbTrantype.SelectedValue.ToString = "RECEIPT" Then
            Return 1
        ElseIf cmbTrantype.SelectedValue.ToString = "ISSUE" Then
            Return 0
        End If
    End Function

    Private Function funcdealerStudedinfoEst(ByVal EstNo As Integer, ByVal estdate As Date, ByVal tagno As String) As String
        Dim Qry As String = ""
        Qry = " "
        Qry += vbCrLf + " SELECT "
        Qry += vbCrLf + " '' KEYNO"
        Qry += vbCrLf + " ,'' TRANTYPE"
        Qry += vbCrLf + " ,'' STNTYPE"
        Qry += vbCrLf + " ,(SELECT TOP 1 ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = A.STNITEMID)ITEM "
        Qry += vbCrLf + " ,(Select TOP 1 SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = A.STNITEMID And SUBITEMID =  A.STNSUBITEMID)SUBITEM"
        Qry += vbCrLf + " ,'' OCATCODE "
        Qry += vbCrLf + " ,'N' STUDDEDUCT "
        Qry += vbCrLf + " , STNPCS PCS "
        Qry += vbCrLf + " , STNWT WEIGHT"
        Qry += vbCrLf + " ,A.STONEUNIT UNIT"
        Qry += vbCrLf + " ,A.CALCMODE CALC"
        Qry += vbCrLf + " ,A.PSTNRATE RATE"
        Qry += vbCrLf + " ,CASE WHEN A.CALCMODE ='W' THEN STNWT*PSTNRATE ELSE STNPCS*PSTNRATE END AMOUNT"
        Qry += vbCrLf + " ,'' METALID"
        Qry += vbCrLf + " ,0.00 DISCOUNT"
        Qry += vbCrLf + " ,0 TAGSTNPCS"
        Qry += vbCrLf + " ,0 TAGSTNWT"
        Qry += vbCrLf + " ,0 TAGSNO"
        Qry += vbCrLf + " ,0 R_VAT"
        Qry += vbCrLf + " ,'' ISSSNO"
        Qry += vbCrLf + " ,'' RESNO"
        Qry += vbCrLf + " ,'' SEIVE"
        Qry += vbCrLf + " ,'' CUTID"
        Qry += vbCrLf + " ,'' COLORID"
        Qry += vbCrLf + " ,'' CLARITYID"
        Qry += vbCrLf + " ,'' SETTYPEID"
        Qry += vbCrLf + " ,'' SHAPEID"
        Qry += vbCrLf + " ,'' HEIGHT"
        Qry += vbCrLf + " ,'' WIDTH"
        Qry += vbCrLf + " FROM " & cnAdminDb & "..DEALER_STUDDED AS A"
        Qry += vbCrLf + " ," & cnStockDb & "..ESTISSUE AS B "
        Qry += vbCrLf + " ," & cnStockDb & "..ESTISSSTONE AS C"
        Qry += vbCrLf + " WHERE A.ITEMID = B.ITEMID And A.SUBITEMID = B.SUBITEMID "
        Qry += vbCrLf + " AND B.SNO = C.ISSSNO AND A.STNITEMID=C.STNITEMID AND A.STNSUBITEMID = C.STNSUBITEMID "
        Qry += vbCrLf + " And A.ACCODE='" & cmbAcname_OWN.SelectedValue.ToString & "' "
        Qry += vbCrLf + " AND A.COSTID = '" & cnCostId & "' "
        Qry += vbCrLf + " AND A.ITEMID = " & Val(txtItemId_NUM.Text) & " "
        Qry += vbCrLf + " AND A.SUBITEMID = " & cmbSubItem_OWN.SelectedValue.ToString & ""
        Qry += vbCrLf + " AND B.TAGNO = '" & tagno & "'"
        Qry += vbCrLf + " AND B.TRANNO = '" & EstNo & "'"
        Qry += vbCrLf + " AND B.TRANDATE = '" & Format(estdate, "yyyy-MM-dd") & "'"
        Qry += vbCrLf + " AND ISNULL(B.CANCEL,'') = '' "
        Qry += vbCrLf + " AND ISNULL(B.BATCHNO,'') = '' "
        Return Qry
    End Function

    Private Sub ShowStoneDia(ByVal EstNo As Integer, ByVal estdate As Date, ByVal tagno As String)
        If objStone.Visible Then Exit Sub
        objStone.grsWt = Val(txtGrsWt_WET.Text)
        objStone._Authorize = True
        objStone.BackColor = Me.BackColor
        objStone.StartPosition = FormStartPosition.CenterScreen
        objStone.MaximizeBox = False
        objStone.StyleGridStone(objStone.gridStone)
        objStone.FromFlag = "A"
        objStone._Accode = cmbAcname_OWN.SelectedValue.ToString
        ' objStone.oMaterial = oMaterial
        If MaterialValue() = 0 Then
            objStone.oMaterial = frmStoneDiaAc.Material.Issue
        Else
            objStone.oMaterial = frmStoneDiaAc.Material.Receipt
        End If
        objStone.txtTranno_OWN.Select()
        objStone.txtTranno_OWN.Visible = True
        objStone.IssRecCat = ACC_STONEISSCAT
        objStone.IssRecStudWtDedut = ACC_STUDDEDUCT_OPTIONAL
        objStone.MIMRSTONETYPE = cmbTrantype.Text
        strsql = ""
        strsql = ""
        'stonepropint()
        'objStone.GETDEALER_STUDDEDRATE = True
        'objStone.GETDEALER_STUDDEDACCODE = cmbAcname_OWN.SelectedValue.ToString
        Dim dtNewStone As DataTable
        strsql = funcdealerStudedinfoEst(EstNo, estdate, tagno)
        da = New OleDbDataAdapter(strsql, cn)
        dtNewStone = New DataTable
        da.Fill(dtNewStone)
        If dtNewStone.Rows.Count > 0 Then
            objStone.dtGridStone = dtNewStone
            objStone.gridStone.DataSource = objStone.dtGridStone
            objStone.dtGridStone.AcceptChanges()
            objStone.frmStoneDiaAc_Load(Me, New System.EventArgs)
        Else
            If ESTSNO.Trim <> "" Then
            Else
                objStone.ShowDialog()
            End If
        End If
        Dim stnWt As Double = 0
        If objStone.CmbStudDeduct.Text = "YES" Then
            stnWt = Val(objStone.gridStoneTotal.Rows(0).Cells("WEIGHT").Value.ToString)
        Else
            stnWt = 0
        End If
        Dim stnAmt As Double = Val(objStone.gridStoneTotal.Rows(0).Cells("AMOUNT").Value.ToString)
        txtLessWt_WET.Text = IIf(stnWt <> 0, Format(stnWt, "0.000"), Nothing)
        txtStoneValue_AMT.Text = IIf(stnAmt <> 0, Format(stnAmt, "0.00"), Nothing)
        Me.SelectNextControl(txtGrsWt_WET, True, True, True, True)
    End Sub

    Private Sub AutoResize()
        If Gridview_OWN.RowCount > 0 Then
            If True Then
                Gridview_OWN.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                Gridview_OWN.Invalidate()
                For Each dgvCol As DataGridViewColumn In Gridview_OWN.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                Gridview_OWN.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            Else
                For Each dgvCol As DataGridViewColumn In Gridview_OWN.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                Gridview_OWN.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End If
        End If
    End Sub

    Private Sub AutoResize2(ByVal grid1 As DataGridView)
        If grid1.RowCount > 0 Then
            If True Then
                grid1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                grid1.Invalidate()
                For Each dgvCol As DataGridViewColumn In grid1.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                grid1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            Else
                For Each dgvCol As DataGridViewColumn In grid1.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                grid1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End If
        End If
    End Sub

    Private Sub AutoResizeColumnWidth(ByVal grid1 As DataGridView, ByVal columnName As String)
        If grid1.RowCount > 0 Then
            If True Then
                grid1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                grid1.Invalidate()
                For Each dgvCol As DataGridViewColumn In grid1.Columns
                    If dgvCol.HeaderText.Contains("TRANTYPENAME") Then Continue For
                    dgvCol.Width = dgvCol.Width
                Next
                grid1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
                For cc As Integer = 0 To grid1.Rows.Count - 1
                    If grid1.Rows(cc).Cells("COLHEAD").Value.ToString = "T" Then
                        If grid1.Rows(cc).Cells("TRANTYPENAME").Value.ToString.Length > 10 Then
                            grid1.Rows(cc).DefaultCellStyle.WrapMode = DataGridViewTriState.True
                            grid1.Rows(cc).Height = 70
                        End If
                    End If
                Next
                'For cc As Integer = 0 To grid1.Rows.Count - 1
                '    If grid1.Rows(cc).Cells("COLHEAD").Value.ToString = "T" Then
                '        grid1.Rows(cc).Cells("TRANTYPE").GetContentBounds(cc)
                '        'grid1.Rows(cc).DefaultCellStyle.WrapMode = DataGridViewTriState.True
                '        'grid1.Rows(cc).Height = 70
                '    End If
                'Next

                For Each dgvrow As DataGridViewRow In grid1.Rows

                Next
            Else
                For Each dgvCol As DataGridViewColumn In grid1.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                grid1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End If
        End If
    End Sub

    Private Sub AutoResizeTotal()
        If gridViewTotal_Own.RowCount > 0 Then
            For i As Integer = 0 To Gridview_OWN.Columns.Count - 1
                gridViewTotal_Own.Columns(i).Width = Gridview_OWN.Columns(i).Width
            Next
        End If
    End Sub

    Private Function RoundOffPisa(ByVal value As Decimal, Optional ByVal Istds As Boolean = False) As Decimal
        Dim mRnd As String
        If Istds Then mRnd = ROUNDOFF_ACC_TDS Else mRnd = ROUNDOFF_ACC
        Select Case mRnd
            Case "L"
                Return Math.Floor(value)
            Case "F"
                If Math.Abs(value - Math.Floor(value)) >= 0.5 Then
                    Return Math.Ceiling(value)
                Else
                    Return Math.Floor(value)
                End If
            Case "H"
                Return Math.Ceiling(value)
            Case Else
                Return value
        End Select
        Return value
    End Function

    Private Sub Getdealerwmcwtrange()
        Dim avgwt As Double = 0
        If Val(txtPcs_NUM.Text) <> 0 Then avgwt = IIf(cmbGrsNet.Text = "GRS WT", Val(txtGrsWt_WET.Text), Val(txtNetWt_WET.Text)) / Val(txtPcs_NUM.Text)
        If avgwt <> 0 Then Getdealerwmc(avgwt)
    End Sub

    Private Sub Getdealerwmc(Optional ByVal wt As Double = 0)
        If lblEditKeyNo.Text <> "" Then Exit Sub
        Dim dttab As New DataTable
        MCPERGMPC = ""
        WASTPERPC = ""
        If Not strLockCtrl Is Nothing Then
            If strLockCtrl.Length > 0 Then
                If Not (Array.IndexOf(strLockCtrl, "WP")) >= 0 Then
                    Me.txtwastage_AMT.Enabled = True : Me.txtwastageper_AMT.Enabled = True : Me.txtTouchper_AMT.Enabled = True
                End If
            Else
                Me.txtwastage_AMT.Enabled = True : Me.txtwastageper_AMT.Enabled = True : Me.txtTouchper_AMT.Enabled = True
            End If
        End If
        Dim msubitemid As Integer
        Dim mitemid As Integer
        If cmbSubItem_OWN.SelectedValue Is Nothing Then msubitemid = 0 Else msubitemid = Val(cmbSubItem_OWN.SelectedValue.ToString & "")
        If cmbItem_OWN.SelectedValue Is Nothing Then mitemid = 0 Else mitemid = Val(cmbItem_OWN.SelectedValue.ToString & "")

        txtwastageper_AMT.Text = "" : txtwastageper_AMT.Text = ""
        If txtOOrdNo = "" Then txtTouchper_AMT.Text = ""
        txtMC_AMT.Text = "" : txtMCPerGram_RATE.Text = ""
nextt:
        strsql = " SELECT * FROM " & cnAdminDb & "..DEALER_WMCTABLE "
        strsql = strsql & " WHERE ACCODE = '" & cmbAcname_OWN.SelectedValue.ToString & "'"
        strsql = strsql & " AND  ITEMID = " & mitemid
        strsql = strsql & " AND  SUBITEMID = " & msubitemid
        If True Then
            'strsql += " and " & wt.ToString & " between from_wt and to_wt"
        End If
        If dealerwmcSno <> "" Then
            strsql += " and sno = '" & dealerwmcSno & "'"
        End If
        dttab = Nothing
        dttab = GetSqlTable(strsql, cn, Tran)
        If dttab.Rows.Count > 0 Then
            If dttab.Rows.Count > 1 Then
                dealerwmcSno = BrighttechPack.SearchDialog.Show("DEALER_WMCTABLE", strsql, cn, 0, 0)
                GoTo nextt
            End If
            With dttab.Rows(0)
                'If .Item("calcmode").ToString = "T" Then Me.txtwastage_AMT.Enabled = False : Me.txtwastageper_AMT.Enabled = False : Me.txtTouchper_AMT.Enabled = True
                'If .Item("calcmode").ToString = "W" Then Me.txtwastage_AMT.Enabled = True : Me.txtwastageper_AMT.Enabled = True : Me.txtTouchper_AMT.Enabled = False
                'If .Item("MCCALC").ToString = "W" Then Me.txtMCPerGram_AMT.Enabled = True : Me.txtMC_AMT.Enabled = D_VA_DATAENABLE
                'If .Item("MCCALC").ToString = "P" Then Me.txtMCPerGram_AMT.Enabled = D_VA_DATAENABLE : Me.txtMC_AMT.Enabled = True
                GRSNETCAL = .Item("grsnet").ToString & ""
                If GRSNETCAL = "N" Then
                    cmbGrsNet.Text = "NET WT"
                Else
                    cmbGrsNet.Text = "GRS WT"
                End If
                MCPERGMPC = .Item("MCCALC").ToString
                'calculationget()
                If cmbTrantype.Text = "PURCHASE RETURN" Or cmbTrantype.Text = "ISSUE" Then
                    txtPurityper_RATE.Text = Format(Val(.Item("TOUCH").ToString), "0.0000")
                    txtTouchper_AMT.Text = Format(Val(.Item("TOUCH").ToString), "0.00")
                Else
                    txtPurityper_RATE.Text = Format(Val(.Item("PUREWT").ToString), "0.0000")
                    txtTouchper_AMT.Text = Format(Val(.Item("TOUCH").ToString), "0.00")
                    txtwastageper_AMT.Text = Val(.Item("wastper").ToString)
                    txtwastage_AMT.Text = Val(.Item("wast").ToString)
                    If Val(.Item("WASTPIE").ToString) <> 0 Then txtwastageper_AMT.Text = Val(.Item("WASTPIE").ToString) : WASTPERPC = "Y" : CalcOWastage()
                    If (Val(.Item("MCPER").ToString)) > 0 Then
                        txtMCPPer_RATE.Text = Format((Val(.Item("MCPER").ToString)), "0.0000")
                    End If
                    If (Val(.Item("MCGRM").ToString)) > 0 Then
                        txtMCPerGram_RATE.Text = Format((Val(.Item("MCGRM").ToString)), "0.00")
                    End If
                    If (Val(.Item("MCPIE").ToString)) > 0 Then
                        txtMCPieces_AMT.Text = Format((Val(.Item("MCPIE").ToString)), "0.00")
                    End If

                    If (Val(.Item("EMCPERG").ToString)) > 0 Then
                        txtEMCPERG_AMT.Text = Format((Val(.Item("EMCPERG").ToString)), "0.00")
                    End If

                    txtOalloyper = Val(.Item("ALLOY").ToString)
                    If MCPERGMPC = "P" Then CalcOMcPercentage()
                End If

                Exit Sub
            End With
        Else
            If wt <> 0 Then wt = 0 : GoTo nextt
            If msubitemid <> 0 Then msubitemid = 0 : GoTo nextt
            If mitemid <> 0 Then
                mitemid = 0
                GoTo nextt
            Else
                txtwastageper_AMT.Text = ""
                txtwastageper_AMT.Text = ""
                If txtOOrdNo = "" Then txtTouchper_AMT.Text = ""
                txtMC_AMT.Text = ""
                txtMCPerGram_RATE.Text = ""
                Exit Sub
            End If

        End If
    End Sub

    Private Sub CalcOEMCPERG()
        If Val(txtEMCPERG_AMT.Text) > 0 And Val(txtRate_RATE.Text) > 0 Then
            CalcONetWt()
            Dim eMcPer As Double = 0
            Dim eMcperRound As Double = 0
            Dim eMcFinal As Double = 0
            eMcPer = Val(txtEMCPERG_AMT.Text) * Val(txtNetWt_WET.Text)
            eMcperRound = eMcPer / Val(txtRate_RATE.Text)
            eMcFinal = Math.Round(eMcperRound / Val(txtNetWt_WET.Text) * 100, 4)
            txtMCPPer_RATE.Text = Math.Round(eMcFinal, 4)
            txtPurityper_RATE.Text = Format(Math.Round(Val(txtTouchper_AMT.Text) - Val(txtMCPPer_RATE.Text), 4), "0.0000")
        End If
    End Sub

    Private Sub CalcONetWt()
        Dim netWt As Decimal = Val(txtGrsWt_WET.Text) - Val(txtLessWt_WET.Text)
        txtNetWt_WET.Text = IIf(netWt <> 0, Format(netWt, "0.000"), "")
    End Sub

    Private Sub CalcOWastage()
        'If Val(txtwastageper_AMT.Text) = 0 Then Exit Sub
        Dim was As Decimal = Nothing
        was = IIf(cmbGrsNet.Text = "GRS WT", Val(txtGrsWt_WET.Text), Val(txtNetWt_WET.Text)) * (Val(txtwastageper_AMT.Text) / 100)
        If WASTPERPC = "Y" Then was = Val(txtPcs_NUM.Text) * Val(txtwastageper_AMT.Text)
        txtwastage_AMT.Text = IIf(was <> 0, Format(was, "0.000"), Nothing)
    End Sub

    Private Function CalcOsumofTouch_purityWastagemc() As Boolean
        If Val(txtTouchper_AMT.Text) > 0 And Val(txtPurityper_RATE.Text) > 0 And Val(txtwastageper_AMT.Text) > 0 And Val(txtMCPPer_RATE.Text) > 0 Then
            If Val(txtTouchper_AMT.Text) <> Math.Round(Val(txtPurityper_RATE.Text) + Val(txtwastageper_AMT.Text) + Val(txtMCPPer_RATE.Text), 2) Then
                MsgBox("Mismatch Touch percentage", MsgBoxStyle.Information)
                txtTouchper_AMT.Focus()
                txtTouchper_AMT.SelectAll()
                Return False
            Else
                Return True
            End If
        End If
        Return True
    End Function

    Private Sub CalcOMcPercentage()
        Dim mc As Decimal = 0
        If Val(txtMCPPer_RATE.Text) = 0 And Val(txtMCPerGram_RATE.Text) = 0 And Val(txtMCPieces_AMT.Text) = 0 And txtMC_AMT.ReadOnly = False Then
            mc = Val(txtMC_AMT.Text)
            GoTo cMc
        End If
        If Val(txtMCPPer_RATE.Text) >= 0 Then
            mc = CalcOMcPerGram()
        End If
        If Val(txtMCPieces_AMT.Text) > 0 Then
            mc += CalcOMcPieces()
        End If
        If cmbTrantype.Text = "RECEIPT" Or cmbTrantype.Text = "ISSUE" Then
            If IIf(cmbGrsNet.Text = "GRS WT", Val(txtGrsWt_WET.Text), Val(txtNetWt_WET.Text)) > 0 Then
                mc += IIf(cmbGrsNet.Text = "GRS WT", Val(txtGrsWt_WET.Text), Val(txtNetWt_WET.Text)) * (Val(txtMCPPer_RATE.Text) / 100) * Val(txtRate_RATE.Text)
            Else
                mc += Val(txtPcs_NUM.Text) * Val(txtMCPPer_RATE.Text)
            End If
        Else
            If IIf(cmbGrsNet.Text = "GRS WT", Val(txtPurityWt_WET.Text), Val(txtPurityWt_WET.Text)) > 0 Then
                mc += IIf(cmbGrsNet.Text = "GRS WT", Val(txtPurityWt_WET.Text), Val(txtPurityWt_WET.Text)) * (Val(txtMCPPer_RATE.Text) / 100) * Val(txtRate_RATE.Text)
            Else
                mc += Val(txtPcs_NUM.Text) * Val(txtMCPPer_RATE.Text)
            End If
        End If
        If MCPERGMPC = "P" Then
            mc += Val(txtPcs_NUM.Text) * Val(txtMCPPer_RATE.Text)
        End If
cMc:
        mc = RoundOffPisa(mc)
        txtMC_AMT.Text = IIf(mc <> 0, Format(mc, "0.00"), Nothing)
        txtMC_AMT.Text = IIf(mc <> 0, Format(Math.Round(mc, 0), "0.00"), Nothing)
        CalcOGrossAmt()
    End Sub

    Private Function CalcOMcPerGram() As Double
        Dim mc As Decimal = Nothing
        If Val(txtMCPerGram_RATE.Text) = 0 Then Return False
        If True Then
            If IIf(cmbGrsNet.Text = "GRS WT", Val(txtGrsWt_WET.Text), Val(txtNetWt_WET.Text)) > 0 Then
                mc = IIf(cmbGrsNet.Text = "GRS WT", Val(txtGrsWt_WET.Text), Val(txtNetWt_WET.Text)) * (Val(txtMCPerGram_RATE.Text))
            Else
                mc = Val(txtPcs_NUM.Text) * Val(txtMCPerGram_RATE.Text)
            End If
            'Else
            'If IIf(cmbGrsNet.Text = "GRS WT", Val(txtPurityWt_WET.Text), Val(txtPurityWt_WET.Text)) > 0 Then
            '    mc = IIf(cmbGrsNet.Text = "GRS WT", Val(txtPurityWt_WET.Text), Val(txtPurityWt_WET.Text)) * (Val(txtMCPerGram_AMT.Text))
            'Else
            '    mc = Val(txtPcs_NUM.Text) * Val(txtMCPerGram_AMT.Text)
            'End If
        End If
        mc = RoundOffPisa(mc)
        'txtMC_AMT.Text = IIf(mc <> 0, Format(mc, "0.00"), Nothing)
        'CalcOGrossAmt()
        Return mc
    End Function
    Private Function CalcOMcPieces() As Double
        Dim mc As Decimal = Nothing
        mc = Val(txtPcs_NUM.Text) * Val(txtMCPieces_AMT.Text)
        mc = RoundOffPisa(mc)
        Return mc
    End Function

    Private Sub CalcOPureWt()
        Dim pureWt As Decimal = Nothing
        If Val(PURWTPERACC.ToString) = 0 Then PURWTPERACC = 100
        If cmbTrantype.Text = "RECEIPT" Or cmbTrantype.Text = "ISSUE" Then
            pureWt = (IIf(cmbGrsNet.Text = "GRS WT", Val(txtGrsWt_WET.Text), Val(txtNetWt_WET.Text))) * (Val(txtPurityper_RATE.Text) / PURWTPERACC)
        Else
            If Val(txtTouchper_AMT.Text) > 0 Then
                pureWt = (IIf(cmbGrsNet.Text = "GRS WT", Val(txtGrsWt_WET.Text), Val(txtNetWt_WET.Text))) * (Val(txtTouchper_AMT.Text) / PURWTPERACC)
            Else
                pureWt = (IIf(cmbGrsNet.Text = "GRS WT", Val(txtGrsWt_WET.Text), Val(txtNetWt_WET.Text))) * (Val(txtPurityper_RATE.Text) / PURWTPERACC)
            End If
        End If
        If cmbOMetal = "GOLD" Then
            txtPurityWt_WET.Text = IIf(pureWt <> 0, Format(Math.Round(pureWt, 2), "0.000"), Nothing)
        ElseIf cmbOMetal = "DIAMOND" Then
            txtPurityWt_WET.Text = IIf(pureWt <> 0, Format(Math.Round(pureWt, 2), "0.000"), Nothing)
        ElseIf cmbOMetal = "SILVER" Then
            txtPurityWt_WET.Text = IIf(pureWt <> 0, Format(Math.Round(pureWt, 0), "0.000"), Nothing)
        ElseIf cmbOMetal = "PLATINUM" Then
            txtPurityWt_WET.Text = IIf(pureWt <> 0, Format(Math.Round(pureWt, 2), "0.000"), Nothing)
        Else
            txtPurityWt_WET.Text = IIf(pureWt <> 0, Format(pureWt, "0.000"), Nothing)
        End If
    End Sub
    Private Sub CalcOGrossAmt()
        Dim GrsAmt As Decimal = Nothing
        If Val(txtGrsWt_WET.Text) <> 0 Then
            If cmbTrantype.Text = "RECEIPT" Or cmbTrantype.Text = "ISSUE" Then 'cmbInvoiceType.Text = "RECEIPT"
                GrsAmt = 0
                GrsAmt = (IIf(cmbGrsNet.Text = "GRS WT", Val(txtGrsWt_WET.Text), Val(txtNetWt_WET.Text)) _
        + Val(txtwastage_AMT.Text) + (-1 * Val(txtOalloy_WET))) _
        * Val(txtRate_RATE.Text)
                GrsAmt = Val(txtMC_AMT.Text) + Val(txtStoneValue_AMT.Text) + Val(txtAddition_AMT.Text)
            ElseIf cmbTrantype.Text = "PURCHASE" Or cmbTrantype.Text = "PURCHASE RETURN" Then 'cmbInvoiceType.Text = "RECEIPT" And
                GrsAmt = (Val(txtPurityWt_WET.Text) + Val(txtwastage_AMT.Text) + (-1 * Val(txtOalloy_WET))) _
        * Val(txtRate_RATE.Text)
                GrsAmt = GrsAmt + Val(txtMC_AMT.Text) + Val(txtStoneValue_AMT.Text) + Val(txtAddition_AMT.Text) '+ Val(txtOED_AMT.Text)
            Else
                GrsAmt = (IIf(cmbGrsNet.Text = "GRS WT", Val(txtGrsWt_WET.Text), Val(txtNetWt_WET.Text)) _
        + Val(txtwastage_AMT.Text) + (-1 * Val(txtOalloy_WET))) _
        * Val(txtRate_RATE.Text)
                GrsAmt = GrsAmt + Val(txtMC_AMT.Text) + Val(txtStoneValue_AMT.Text) + Val(txtAddition_AMT.Text) '+ Val(txtOED_AMT.Text)
            End If
        Else
            GrsAmt = Val(txtPcs_NUM.Text) * Val(txtRate_RATE.Text)
        End If
        GrsAmt = RoundOffPisa(GrsAmt)
        If cmbTrantype.Text = "" Or cmbTrantype.Text = "" Then
            GrsAmt = 0
        End If
        txtGrossAmt_AMT.Text = IIf(GrsAmt <> 0, Format(GrsAmt, "0.00"), Nothing)
        CalcExclusiveGST()
        CalcOVatTds()
        CalcONetAmt()
    End Sub

    Private Sub CalcOVatTds()
        Dim vatTds As Decimal = Nothing
        vatTds = (Val(txtGrossAmt_AMT.Text) + Val(txtOED_AMT)) * (Val(objGSTTDS.txtTdsPer_PER.Text) / 100)
        'vatTds = IIf(lblOVat = "TDS", RoundOffPisa(vatTds, True), RoundOffPisa(vatTds))
        vatTds = Math.Ceiling(vatTds)
        If cmbTrantype.Text = "PURCHASE" Or cmbTrantype.Text = "PURCHASE RETURN" Then
            vatTds = 0
        End If
        CalcONetAmt()
    End Sub
    Private Function funcSalesGst() As DataRow
        If Val(cmbItem_OWN.SelectedValue.ToString) = 0 Then
            Exit Function
        End If
        Dim Catcode As String = ""
        Dim drCat As DataRow = Nothing
        strsql = " SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & cmbItem_OWN.SelectedValue.ToString & " "
        drCat = GetSqlRow(strsql, cn)
        If drCat Is Nothing Then
            Return Nothing
        End If
        Catcode = drCat.Item("CATCODE").ToString
        Dim drGstper As DataRow = Nothing
        If False Then
            strsql = " SELECT 0 S_SGSTTAX, 0 S_CGSTTAX,0 S_IGSTTAX FROM " & cnAdminDb & "..CATEGORY "
            strsql += vbCrLf + " WHERE CATCODE = '" & Catcode & "' "
        Else
            strsql = " SELECT S_SGSTTAX,S_CGSTTAX,S_IGSTTAX FROM " & cnAdminDb & "..CATEGORY "
            strsql += vbCrLf + " WHERE CATCODE = '" & Catcode & "' "
        End If
        drGstper = GetSqlRow(strsql, cn)
        Return drGstper
    End Function
    Private Function funcAcNameStatecode() As String
        Dim AcStatecode As String = ""
        strsql = " SELECT ISNULL(STATECODE,'') STATECODE FROM " & cnAdminDb & "..STATEMAST WHERE  "
        strsql += " STATEID IN(SELECT STATEID FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = '" & cmbAcname_OWN.SelectedValue.ToString & "') "
        AcStatecode = objGPack.GetSqlValue(strsql).ToString
        Return AcStatecode
    End Function

    Private Function funcCompanyStatecode() As String
        Dim cmpStatecode As String = ""
        strsql = " SELECT ISNULL(STATECODE,'') STATECODE FROM " & cnAdminDb & "..STATEMAST WHERE  "
        strsql += " STATEID IN(SELECT STATEID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYID = '" & strCompanyId & "') "
        cmpStatecode = objGPack.GetSqlValue(strsql).ToString
        Return cmpStatecode
    End Function


    Private Function funcSupplierGstNo(ByVal accode As String) As String
        Dim suppGst As String = ""
        strsql = " SELECT ISNULL(GSTNO,'') GSTNO FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = '" & accode & "'  "
        suppGst = objGPack.GetSqlValue(strsql).ToString
        Return suppGst
    End Function

    Private Sub CalcOGSTPER()
        If cmbItem_OWN.SelectedValue Is Nothing Then
            Exit Sub
        End If
        If cmbItem_OWN.SelectedValue.ToString = "0" Then
            Exit Sub
        End If
        If cmbAcname_OWN.SelectedValue.ToString = "" Then
            cmbAcname_OWN.Focus()
            cmbAcname_OWN.SelectAll()
            Exit Sub
        End If
        If cmbAcname_OWN.Text = "" Then
            cmbAcname_OWN.Focus()
            cmbAcname_OWN.SelectAll()
            Exit Sub
        End If
        Dim drGstper As DataRow = Nothing
        drGstper = funcSalesGst()
        If drGstper Is Nothing Then
            MsgBox("Invalid Gstper ", MsgBoxStyle.Information)
            Exit Sub
        End If

        Dim AcStatecode As String = ""
        Dim cmpStatecode As String = ""
        Dim supplierGst As String = ""
        AcStatecode = funcAcNameStatecode()
        cmpStatecode = funcCompanyStatecode()
        supplierGst = funcSupplierGstNo(cmbAcname_OWN.SelectedValue.ToString)
        If AcStatecode = "" Then
            AcStatecode = cmpStatecode
        End If
        If cmbTrantype.Text = "RECEIPT" Or cmbTrantype.Text = "ISSUE" Then
            Dim splitAccode() As String = Nothing
            If AcStatecode = cmpStatecode Then
                strsql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID='LABOUR_SGST' "
                splitAccode = objGPack.GetSqlValue(strsql).Split(":")
                lblSGSTPer.Text = splitAccode(1)
                strsql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID='LABOUR_CGST'"
                splitAccode = objGPack.GetSqlValue(strsql).Split(":")
                lblCGSTPer.Text = splitAccode(1)
                lblIGSTPer.Text = ""
            Else
                lblSGSTPer.Text = ""
                lblCGSTPer.Text = ""
                strsql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID='LABOUR_IGST'"
                splitAccode = objGPack.GetSqlValue(strsql).Split(":")
                lblIGSTPer.Text = splitAccode(1)
            End If
            If supplierGst = "" Or supplierGst = "UNGREGISTER" Or supplierGst = "UNREGISTERED" Then
                lblSGSTPer.Text = ""
                lblCGSTPer.Text = ""
                lblIGSTPer.Text = ""
            End If
        Else
            If AcStatecode = cmpStatecode Then
                lblSGSTPer.Text = Format(drGstper.Item("S_SGSTTAX"), "#.###")
                lblCGSTPer.Text = Format(drGstper.Item("S_CGSTTAX"), "#.###")
                lblIGSTPer.Text = ""
            Else
                lblSGSTPer.Text = ""
                lblCGSTPer.Text = ""
                lblIGSTPer.Text = Format(drGstper.Item("S_IGSTTAX"), "#.###")
            End If
            If supplierGst = "" Or supplierGst = "UNGREGISTER" Or supplierGst = "UNREGISTERED" Then
                lblSGSTPer.Text = ""
                lblCGSTPer.Text = ""
                lblIGSTPer.Text = ""
            End If
        End If
    End Sub
    Dim MIMR_WASTAGE_GSTREVERSE As Boolean = False
    Private Function CalcGstReversalonWastage() As DataTable
        Dim dr As DataRow = Nothing
        dtWastageGSt = GetDatatableWastage()
        If cmbTrantype.Text = "RECEIPT" Or cmbTrantype.Text = "ISSUE" Or MIMR_WASTAGE_GSTREVERSE = False Then
            dr = Nothing
            Dim AcStatecode As String = ""
            Dim cmpStatecode As String = ""
            If Val(txtwastage_AMT.Text) >= 0 Then
                Dim drGstper As DataRow = Nothing
                drGstper = funcSalesGst()
                If drGstper Is Nothing Then
                    If Val(cmbItem_OWN.SelectedValue.ToString) = 0 Then
                        Return Nothing
                    End If
                    MsgBox("Invalid Gstper ", MsgBoxStyle.Information)
                    Return Nothing
                End If
                AcStatecode = funcAcNameStatecode()
                cmpStatecode = funcCompanyStatecode()
                If AcStatecode = cmpStatecode Then
                    dr = dtWastageGSt.NewRow
                    dr!WSGSTPER = drGstper.Item("S_SGSTTAX")
                    dr!WCGSTPER = drGstper.Item("S_CGSTTAX")
                    dr!WIGSTPER = 0
                    dr!GSTPER = drGstper.Item("S_SGSTTAX") + drGstper.Item("S_CGSTTAX")
                    dr!WSGST = ((Val(txtwastage_AMT.Text) * (drGstper.Item("S_SGSTTAX") / 100)) * Val(txtRate_RATE.Text)) * ((drGstper.Item("S_SGSTTAX") + drGstper.Item("S_CGSTTAX")) / 100)
                    dr!WCGST = ((Val(txtwastage_AMT.Text) * (drGstper.Item("S_CGSTTAX") / 100)) * Val(txtRate_RATE.Text)) * ((drGstper.Item("S_SGSTTAX") + drGstper.Item("S_CGSTTAX")) / 100)
                    dr!WIGST = 0
                    '((Val(txtwastage_AMT.Text) * ((drGstper.Item("S_SGSTTAX") + drGstper.Item("S_CGSTTAX")) / 100)) * Val(txtRate_AMT.Text)) * ((drGstper.Item("S_SGSTTAX") + drGstper.Item("S_CGSTTAX")) / 100) '
                    dr!GST = Val(dr!WSGST) + Val(dr!WCGST)
                Else
                    dr = dtWastageGSt.NewRow
                    dr!WSGSTPER = 0
                    dr!WCGSTPER = 0
                    dr!WIGSTPER = drGstper.Item("S_IGSTTAX")
                    dr!GSTPER = drGstper.Item("S_IGSTTAX")
                    dr!WSGST = 0
                    dr!WCGST = 0
                    dr!WIGST = ((Val(txtwastage_AMT.Text) * (drGstper.Item("S_IGSTTAX") / 100)) * Val(txtRate_RATE.Text)) * (drGstper.Item("S_IGSTTAX") / 100)
                    dr!GST = ((Val(txtwastage_AMT.Text) * (drGstper.Item("S_IGSTTAX") / 100)) * Val(txtRate_RATE.Text)) * (drGstper.Item("S_IGSTTAX") / 100)
                End If
                dtWastageGSt.Rows.Add(dr)
                'txtWastGST_AMT.Text = Format(Val(dtWastageGSt.Compute("SUM(GST)", "").ToString), "0.00")
            End If
        End If
        dtWastageGSt = GetDatatableWastage()
        Return dtWastageGSt
    End Function
    Private Sub CalcExclusiveGST()
        CalcOGSTPER()
        Dim Gst As Decimal = Nothing
        Gst = (Val(txtGrossAmt_AMT.Text) + Val(txtOED_AMT)) * (Val(lblSGSTPer.Text) / 100)
        Gst = RoundOffPisa(Gst)
        txtSGSTVAL_AMT.Text = IIf(Gst <> 0, Format(Gst, "0.00"), Nothing)

        Gst = (Val(txtGrossAmt_AMT.Text) + Val(txtOED_AMT)) * (Val(lblCGSTPer.Text) / 100)
        Gst = RoundOffPisa(Gst)
        txtCGSTVAL_AMT.Text = IIf(Gst <> 0, Format(Gst, "0.00"), Nothing)

        Gst = (Val(txtGrossAmt_AMT.Text) + Val(txtOED_AMT)) * (Val(lblIGSTPer.Text) / 100)
        Gst = RoundOffPisa(Gst)
        txtIGSTVAL_AMT.Text = IIf(Gst <> 0, Format(Gst, "0.00"), Nothing)
        If lblOVat.ToUpper <> "TDS" Then
            Gst = Val(txtSGSTVAL_AMT.Text) + Val(txtCGSTVAL_AMT.Text) + Val(txtIGSTVAL_AMT.Text)
        End If
        CalcGstReversalonWastage()
    End Sub
    Private Sub CalcInclusiveGST()
        CalcOGSTPER()
        Dim Gst As Decimal = Nothing
        Gst = (Val(txtGrossAmt_AMT.Text) * Val(lblSGSTPer.Text)) / (100 + (Val(lblSGSTPer.Text) + Val(lblCGSTPer.Text) + 0))
        Gst = RoundOffPisa(Gst)
        txtSGSTVAL_AMT.Text = IIf(Gst <> 0, Format(Gst, "0.00"), Nothing)

        Gst = (Val(txtGrossAmt_AMT.Text) * Val(lblCGSTPer.Text)) / (100 + (Val(lblSGSTPer.Text) + Val(lblCGSTPer.Text) + 0))
        Gst = RoundOffPisa(Gst)
        txtCGSTVAL_AMT.Text = IIf(Gst <> 0, Format(Gst, "0.00"), Nothing)

        Gst = (Val(txtGrossAmt_AMT.Text) * Val(lblIGSTPer.Text)) / (100 + (Val(lblIGSTPer.Text) + 0))
        Gst = RoundOffPisa(Gst)
        txtIGSTVAL_AMT.Text = IIf(Gst <> 0, Format(Gst, "0.00"), Nothing)

        CalcGstReversalonWastage()
        If lblOVat.ToUpper <> "TDS" Then
            Gst = Val(txtSGSTVAL_AMT.Text) + Val(txtCGSTVAL_AMT.Text) + Val(txtIGSTVAL_AMT.Text)
        End If
    End Sub

    Public Sub CalcONetAmt()
        Dim netAmt As Decimal = Nothing
        Dim TaxdeductedAmt As Decimal = Val(objGSTTDS.txtTdsAmt_AMT.Text)
        Dim TaxCollectedAmt As Decimal = Val(objGSTTDS.txtTCSAmt_AMT.Text)
        netAmt = Val(txtGrossAmt_AMT.Text) + Val(txtOED_AMT) + IIf(lblOVat.ToUpper = "TDS", -1 * TaxdeductedAmt, TaxdeductedAmt)
        netAmt += Val(txtCGSTVAL_AMT.Text) + Val(txtSGSTVAL_AMT.Text) + Val(txtIGSTVAL_AMT.Text) - 0 + Val(TaxCollectedAmt)
        netAmt = RoundOffPisa(netAmt)
        txtNetVal_AMT.Text = IIf(netAmt <> 0, Format(netAmt, "0.00"), Nothing)
    End Sub

#End Region

#Region " User Define Function"
    Private Sub LoadBalanceWt(ByVal accode As String, ByVal TranType As String)
        If cmbAcname_OWN.Text <> "" Then
            gridviewTotalValue_Own.DataSource = Nothing
            Dim DtGrid As New DataSet()
            Dim smithAbstract As Boolean = False
            Dim mimrSmithAbstract As Boolean = True
            If smithAbstract = True Then
                strsql = vbCrLf + "EXEC " & cnAdminDb & "..SP_RPT_SMITHABSTRACT"
                strsql += vbCrLf + " @DBNAME='" & cnStockDb & "'"
                strsql += vbCrLf + ",@TEMPTAB= 'TEMP" & systemId & "ABSTRACT'"
                strsql += vbCrLf + ",@FROMDATE='" & Format(cnTranFromDate, "yyyy-MM-dd") & "'"
                strsql += vbCrLf + ",@TODATE='" & Format(cnTranToDate, "yyyy-MM-dd") & "'"
                strsql += vbCrLf + ",@WEIGHT='P'"
                strsql += vbCrLf + ",@METALID='G,S,P,D,T,O'"
                strsql += vbCrLf + ",@COMPANYID='" & strCompanyId & "'"
                strsql += vbCrLf + ",@COSTID='" & cnCostId & "'"
                strsql += vbCrLf + ",@ACFILTER=''"
                strsql += vbCrLf + ",@TRANFILTER=''"
                strsql += vbCrLf + ",@CATFILTER=''"
                strsql += vbCrLf + ",@NILBALANCE='Y'"
                strsql += vbCrLf + ",@LOCALOUT=''"
                strsql += vbCrLf + ",@APPTRANFILTER=''"
                strsql += vbCrLf + ",@PUREWTPER=0"
                strsql += vbCrLf + ",@ACCODE='" & accode & "'"
                strsql += vbCrLf + ",@WITHWAST='Y'"
                cmd = New OleDbCommand(strsql, cn) : cmd.CommandTimeout = 1000
                cmd.CommandTimeout = 1000
                da = New OleDbDataAdapter(cmd)
                da.Fill(DtGrid)
            ElseIf mimrSmithAbstract = True Then
                strsql = ""
                strsql += vbCrLf + "EXEC " & cnAdminDb & "..[SP_RPT_MIMR_SMITHSUMMARY] "
                strsql += vbCrLf + "@ADMINDB ='" & cnAdminDb & "'"
                strsql += vbCrLf + ",@DBNAME ='" & cnStockDb & "'"
                strsql += vbCrLf + ",@COMPANYID = '" & strCompanyId & "' "
                strsql += vbCrLf + ",@ASONDATE = '" & GetServerDate(Nothing) & "'"
                strsql += vbCrLf + ",@ACCODE ='" & accode & "'"
                strsql += vbCrLf + ",@TRANTYPE='" & TranType & "'"
                cmd = New OleDbCommand(strsql, cn) : cmd.CommandTimeout = 1000
                cmd.CommandTimeout = 1000
                da = New OleDbDataAdapter(cmd)
                da.Fill(DtGrid)
            Else
                Dim chkwt As String = ""
                chkwt = "GNP"

                strsql = "EXEC " & cnAdminDb & "..SP_RPT_SMITHACCOUNTBALANCE_CAT"
                strsql += vbCrLf + " @DBNAME='" & cnStockDb & "'"
                strsql += vbCrLf + ",@TODATE='" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "'"
                strsql += vbCrLf + ",@ACCODE='" & accode & "'"
                strsql += vbCrLf + ",@WEIGHT='" & chkwt & "'"
                cmd = New OleDbCommand(strsql, cn) : cmd.CommandTimeout = 1000
                cmd.CommandTimeout = 1000
                da = New OleDbDataAdapter(cmd)
                da.Fill(DtGrid)
            End If
            gridviewTotalValue_Own.DataSource = Nothing
            gridviewTotalValue_Own.DataSource = DtGrid.Tables(0)
            If gridviewTotalValue_Own.Rows.Count > 1 Then
                With gridviewTotalValue_Own
                    If smithAbstract = True Then
                        For i As Integer = 0 To .Columns.Count - 1
                            .Columns(i).Visible = False
                        Next
                        .Columns("CLOSING_GRSWT").Visible = True
                        .Columns("CLOSING_PUREWT").Visible = True
                        .Columns("CLOSING_GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                        .Columns("CLOSING_PUREWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                        .Visible = True
                        .BringToFront()
                        If gridviewTotalValue_Own.RowCount > 0 Then
                            gridviewTotalValue_Own.Rows.RemoveAt(gridviewTotalValue_Own.RowCount - 1)
                        End If
                    ElseIf mimrSmithAbstract = True Then
                        .Columns("CLOSING_GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                        .Visible = True
                        .BringToFront()
                        If gridviewTotalValue_Own.RowCount > 0 Then
                            gridviewTotalValue_Own.Rows.RemoveAt(gridviewTotalValue_Own.RowCount - 1)
                        End If
                    Else
                        .Visible = True
                        .BringToFront()
                        .Columns(0).Width = 85
                        .Columns(1).Width = 75
                        .Columns(2).Width = 75
                        .Columns(3).Width = 75
                        .Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                        .Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                        .Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    End If
                End With
            End If
        End If
    End Sub
    Private Sub funcMcAmtReadonly()
        If cmbTrantype.Text = "PURCHASE" Then
            txtMC_AMT.ReadOnly = False
        Else
            txtMC_AMT.ReadOnly = True
        End If
    End Sub

    Private Sub funcLastTransaction(ByVal typeName As String, ByVal accode As String)
        If accode = "" Then Exit Sub
        Dim TranType As String = "RRE"
        If typeName = "RECEIPT" Then
            TranType = "RRE"
        ElseIf typeName = "PURCHASE" Then
            TranType = "RPU"
        ElseIf typeName = "ISSUE" Then
            TranType = "ISS"
        End If
    End Sub

    Public Sub funcTdsCalReceipt()
        txtConTDS_AMT.Text = Format(Math.Ceiling(((Val(txtConAmount_AMT.Text) + 0) * Val(txtConTDSPER_AMT.Text)) / 100), "0.00")
    End Sub

    Public Sub funcTcsCalReceipt()
        'txtConTCS_AMT.Text = Format(Math.Round(((Val(txtConAmount_AMT.Text) + Val(txtConGST_AMT.Text)) * Val(txtConTCSPER_WET.Text)) / 100, 2), "0.00")
        txtConTCS_AMT.Text = Format(Math.Ceiling(((Val(txtConAmount_AMT.Text) + Val(txtConGST_AMT.Text)) * Val(txtConTCSPER_WET.Text)) / 100), "0.00")
    End Sub

    Private Sub funcalcTds_TcsRounded(ByVal dtchk As DataTable)
        Dim Qry As String = strAchead(cmbAcname_OWN.SelectedValue.ToString)
        Dim drAchead As DataRow = Nothing
        drAchead = GetSqlRow(Qry, cn)
        If Not drAchead Is Nothing Then
            txtConTDSPER_AMT.Text = "0.00"
            txtConTCSPER_WET.Text = "0.000"
            Dim getGridTT As String = getGridTrantype()
            If cmbTrantype.Text = "PURCHASE" Then
                'txtConTCSPER_WET.Text = Format(Val(drAchead.Item("TCSPER").ToString), "0.000")
                Dim NetAmtTDS As Double = 0
                If dtchk.Rows.Count > 0 Then
                    NetAmtTDS = Val(dtchk.Compute("SUM(NETAMT)", "PAYMODE='PURCHASE'"))
                Else
                    NetAmtTDS = 0
                End If
                funTdsNew_calculationmethod(cmbAcname_OWN.SelectedValue.ToString, Format(Val(drAchead.Item("TDSPER").ToString), "0.00"), Val(NetAmtTDS), PURCHASE_TDS_YNFIRSTTIME)
            ElseIf cmbTrantype.Text = "RECEIPT" Then
                txtConTDSPER_AMT.Text = Format(Val(drAchead.Item("TDSPER").ToString), "0.00")
            ElseIf getGridTT = "PURCHASE" Then
                'txtConTCSPER_WET.Text = Format(Val(drAchead.Item("TCSPER").ToString), "0.000")
                Dim NetAmtTDS As Double = 0
                If dtchk.Rows.Count > 0 Then
                    NetAmtTDS = Val(dtchk.Compute("SUM(NETAMT)", "PAYMODE='PURCHASE'"))
                Else
                    NetAmtTDS = 0
                End If
                funTdsNew_calculationmethod(cmbAcname_OWN.SelectedValue.ToString, Format(Val(drAchead.Item("TDSPER").ToString), "0.00"), Val(NetAmtTDS), PURCHASE_TDS_YNFIRSTTIME)
            ElseIf getGridTT = "RECEIPT" Then
                txtConTDSPER_AMT.Text = Format(Val(drAchead.Item("TDSPER").ToString), "0.00")
            End If
        End If
        Dim dtOthReceipt As New DataTable
        Dim dtOthIssue As New DataTable

        Dim dataViewReceipt As New DataView(dtchk)
        dataViewReceipt.RowFilter = "TRANTYPE='RECEIPT'"
        dtOthReceipt = dataViewReceipt.ToTable

        Dim dataViewIssue As New DataView(dtchk)
        dataViewIssue.RowFilter = "TRANTYPE='ISSUE'"
        dtOthIssue = dataViewIssue.ToTable
        If dtOthReceipt.Rows.Count > 0 Or dtOthIssue.Rows.Count > 0 Then
            Dim Trantype As String = ""
            Dim GrsRec As Double = 0
            Dim GstRec As Double = 0
            Dim GrsIss As Double = 0
            Dim GstIss As Double = 0
            If dtOthReceipt.Rows.Count > 0 Then
                Trantype = "RECEIPT"
                GrsRec = dtOthReceipt.Compute("SUM(GROSSAMT)", "TRANTYPE='" & Trantype & "'")
                GstRec = Val(dtOthReceipt.Compute("SUM(SGST)", "TRANTYPE='" & Trantype & "'")) + Val(dtOthReceipt.Compute("SUM(CGST)", "TRANTYPE='" & Trantype & "'")) + Val(dtOthReceipt.Compute("SUM(IGST)", "TRANTYPE='" & Trantype & "'"))
            End If
            If dtOthIssue.Rows.Count > 0 Then
                Trantype = "ISSUE"
                GrsIss = dtOthIssue.Compute("SUM(GROSSAMT)", "TRANTYPE='" & Trantype & "'")
                GstIss = Val(dtOthIssue.Compute("SUM(SGST)", "TRANTYPE='" & Trantype & "'")) + Val(dtOthIssue.Compute("SUM(CGST)", "TRANTYPE='" & Trantype & "'")) + Val(dtOthIssue.Compute("SUM(IGST)", "TRANTYPE='" & Trantype & "'"))
            End If
            txtConAmount_AMT.Text = Format(GrsRec, "0.00")
            txtConGST_AMT.Text = Format(GstRec, "0.00")
            funcTdsCalReceipt()
            funcTcsCalReceipt()
            Dim taxCharge As Double = Val(txtConTCS_AMT.Text) - Val(txtConTDS_AMT.Text)
            txtSumReceipt.Text = Format(GrsRec + GstRec + taxCharge, "0.00")
            txtSumIssue.Text = Format(GrsIss + GstIss, "0.00")
            If Val(txtSumReceipt.Text) > 0 And Val(txtSumIssue.Text) > 0 Then
                txtSumNet.Text = Format(Val(txtSumReceipt.Text) - Val(txtSumIssue.Text), "0.00")
            ElseIf Val(txtSumReceipt.Text) > 0 Then
                txtSumNet.Text = Format(Val(txtSumReceipt.Text), "0.00")
            ElseIf Val(txtSumIssue.Text) > 0 Then
                txtSumNet.Text = Format(Val(txtSumIssue.Text), "0.00")
            Else
                txtSumNet.Text = Format(Val(txtSumReceipt.Text) - Val(txtSumIssue.Text), "0.00")
            End If
            Dim Rnd As Double = CalcRoundoffAmt(Val(txtSumNet.Text), "F")
            txtRoundoff_AMT.Text = Format(Rnd - Val(txtSumNet.Text), "0.00")
        End If
    End Sub

    Private Sub funGrandTotal()
        Dim dtGrandT As New DataTable
        dtGrandT = Gridview_OWN.DataSource
        If dtGrandT.Rows.Count > 0 Then
            dtTranGrandTotal.Rows.Clear()
            Dim dtTrantypeDistinct As New DataTable
            dtTrantypeDistinct = dtGrandT.DefaultView.ToTable(True, "PAYMODE")
            If dtTrantypeDistinct.Rows.Count > 0 Then
                For i As Integer = 0 To dtTrantypeDistinct.Rows.Count - 1
                    With dtTrantypeDistinct.Rows(i)
                        Dim TPCS As Integer = Val(dtGrandT.Compute("SUM(PCS)", "PAYMODE='" & .Item("PAYMODE").ToString & "'").ToString)
                        Dim TGRSWT As Decimal = Val(dtGrandT.Compute("SUM(GRSWT)", "PAYMODE='" & .Item("PAYMODE").ToString & "'").ToString)
                        Dim TLESSWT As Decimal = Val(dtGrandT.Compute("SUM(LESSWT)", "PAYMODE='" & .Item("PAYMODE").ToString & "'").ToString)
                        Dim TNETWT As Decimal = Val(dtGrandT.Compute("SUM(NETWT)", "PAYMODE='" & .Item("PAYMODE").ToString & "'").ToString)
                        Dim TPUREWT As Decimal = Val(dtGrandT.Compute("SUM(PUREWT)", "PAYMODE='" & .Item("PAYMODE").ToString & "'").ToString)
                        Dim TWASTAGE As Decimal = Val(dtGrandT.Compute("SUM(WASTAGE)", "PAYMODE='" & .Item("PAYMODE").ToString & "'").ToString)
                        Dim TMC As Decimal = Val(dtGrandT.Compute("SUM(MC)", "PAYMODE='" & .Item("PAYMODE").ToString & "'").ToString)
                        Dim TSTUDAMT As Decimal = Val(dtGrandT.Compute("SUM(STUDAMT)", "PAYMODE='" & .Item("PAYMODE").ToString & "'").ToString)
                        Dim TGROSSAMT As Decimal = Val(dtGrandT.Compute("SUM(GROSSAMT)", "PAYMODE='" & .Item("PAYMODE").ToString & "'").ToString)
                        Dim TGST As Decimal = Val(dtGrandT.Compute("SUM(GST)", "PAYMODE='" & .Item("PAYMODE").ToString & "'").ToString)
                        Dim TTDSVAL As Decimal = Val(dtGrandT.Compute("SUM(TDSVAL)", "PAYMODE='" & .Item("PAYMODE").ToString & "'").ToString)
                        Dim TTCSVAL As Decimal = Val(dtGrandT.Compute("SUM(TCSVAL)", "PAYMODE='" & .Item("PAYMODE").ToString & "'").ToString)
                        Dim TNETAMT As Decimal = Val(dtGrandT.Compute("SUM(NETAMT)", "PAYMODE='" & .Item("PAYMODE").ToString & "'").ToString)
                        Dim drNewrow As DataRow = Nothing
                        drNewrow = dtTranGrandTotal.NewRow()
                        drNewrow!PAYMODE = .Item("PAYMODE").ToString
                        drNewrow!PCS = TPCS
                        drNewrow!GRSWT = Format(TGRSWT, "0.000")
                        drNewrow!LESSWT = Format(TLESSWT, "0.000")
                        drNewrow!NETWT = Format(TNETWT, "0.000")
                        drNewrow!PUREWT = Format(TPUREWT, "0.000")
                        drNewrow!WASTAGE = Format(TWASTAGE, "0.00")
                        drNewrow!MC = Format(TMC, "0.00")
                        drNewrow!STUDAMT = Format(TSTUDAMT, "0.00")
                        drNewrow!GROSSAMT = Format(TGROSSAMT, "0.00")
                        drNewrow!GST = Format(TGST, "0.00")
                        drNewrow!TDSVAL = Format(TTDSVAL, "0.00")
                        drNewrow!TCSVAL = Format(TTCSVAL, "0.00")
                        drNewrow!NETAMT = Format(TNETAMT, "0.00")
                        Dim Drselect() As DataRow = dtGrandT.Select("PAYMODE='" & .Item("PAYMODE").ToString & "'")
                        If Drselect.Length > 0 Then
                            drNewrow!TRANTYPE = Drselect(0)("TRANTYPE").ToString
                        End If
                        dtTranGrandTotal.Rows.Add(drNewrow)
                    End With
                Next
            End If
            With gridViewTotal_Own
                .DataSource = Nothing
                .DataSource = dtTranGrandTotal
                lblSumPcs.Text = Val(dtTranGrandTotal.Compute("SUM(PCS)", "TRANTYPE='RECEIPT'").ToString) - Val(dtTranGrandTotal.Compute("SUM(PCS)", "TRANTYPE='ISSUE'").ToString)
                lblSumGrswt.Text = Format(Val(dtTranGrandTotal.Compute("SUM(GRSWT)", "TRANTYPE='RECEIPT'").ToString) - Val(dtTranGrandTotal.Compute("SUM(GRSWT)", "TRANTYPE='ISSUE'").ToString), "0.000")
                lblSumStone.Text = Format(Val(dtTranGrandTotal.Compute("SUM(LESSWT)", "TRANTYPE='RECEIPT'").ToString) - Val(dtTranGrandTotal.Compute("SUM(LESSWT)", "TRANTYPE='ISSUE'").ToString), "0.000")
                lblSumNetwt.Text = Format(Val(dtTranGrandTotal.Compute("SUM(NETWT)", "TRANTYPE='RECEIPT'").ToString) - Val(dtTranGrandTotal.Compute("SUM(NETWT)", "TRANTYPE='ISSUE'").ToString), "0.000")
                lblSumPureWt.Text = Format(Val(dtTranGrandTotal.Compute("SUM(PUREWT)", "TRANTYPE='RECEIPT'").ToString) - Val(dtTranGrandTotal.Compute("SUM(PUREWT)", "TRANTYPE='ISSUE'").ToString), "0.000")

                lblSumYield.Text = Format(Val(dtTranGrandTotal.Compute("SUM(WASTAGE)", "TRANTYPE='RECEIPT'").ToString) - Val(dtTranGrandTotal.Compute("SUM(WASTAGE)", "TRANTYPE='ISSUE'").ToString), "0.00")
                lblSumMc.Text = Format(Val(dtTranGrandTotal.Compute("SUM(MC)", "TRANTYPE='RECEIPT'").ToString) - Val(dtTranGrandTotal.Compute("SUM(MC)", "TRANTYPE='ISSUE'").ToString), "0.00")
                lblSumAmount.Text = Format(Val(dtTranGrandTotal.Compute("SUM(NETAMT)", "TRANTYPE='RECEIPT'").ToString) - Val(dtTranGrandTotal.Compute("SUM(NETAMT)", "TRANTYPE='ISSUE'").ToString), "0.00")
                styleGrid(gridViewTotal_Own)
                'For i As Integer = 0 To .Rows.Count - 1
                '    .Rows(i).DefaultCellStyle.ForeColor = Color.Orange
                'Next
            End With
        End If
    End Sub
    Private Sub funLoadGRSNET()
        cmbGrsNet.Items.Clear()
        cmbGrsNet.Items.Add("GRS WT")
        cmbGrsNet.Items.Add("NET WT")
        cmbGrsNet.Text = "NET WT"
        'cmbGrsNet.Text = "GRS WT"
    End Sub
    Private Sub Textboxonlyclear()
        ClearTextBox(Me, True)
    End Sub

    Private Sub txtRemarkreadonlytrueorfalse(ByVal bool1 As Boolean, ByVal bool2 As Boolean)
        txtRemark1.ReadOnly = bool1
        txtRemark2.ReadOnly = bool2
    End Sub

    Private Sub funMultiClear()
        Textboxonlyclear()
        objGSTTDS = New frmGSTGSTTDS(False, 0, "", "", 0)
        objStone = New frmStoneDiaAc
        dtWastageGSt = GetDatatableWastage()
        'stonepropint()
        lblEditKeyNo.Text = ""
        ClearGSTLabel()
        gridviewTotalValue_Own.DataSource = Nothing
    End Sub

    Private Function funcRateOTP() As Boolean
        If cmbAcname_OWN.Text = "" Then Return False
        If cmbTrantype.Text = "" Then Return False
        objMIMRRate.cmbTrantype = cmbTrantype.Text
        objMIMRRate.cmbAcname = cmbAcname_OWN.Text
        objMIMRRate.ItemId = cmbItem_OWN.SelectedValue.ToString
        objMIMRRate.Subitemid = cmbSubItem_OWN.SelectedValue.ToString
        If cmbTrantype.Text = "PURCHASE" Then
            objMIMRRate.txtNewRate_RATE.ReadOnly = False
            objMIMRRate.funradiobuttonFixed(True)
        Else
            objMIMRRate.txtNewRate_RATE.ReadOnly = True
            objMIMRRate.funradiobuttonFixed(False)
        End If
        objMIMRRate.txtNewRateInclusive_RATE.Focus()
        objMIMRRate.txtNewRateInclusive_RATE.SelectAll()
        If objMIMRRate.ShowDialog = Windows.Forms.DialogResult.OK Then
            Return True
        End If
    End Function

    Private Function funcBoardRate(ByVal ItemId As Integer) As Double
        If ItemId = 0 Then Exit Function
        If ManuallyRatechange = False Then
            If funcRateOTP() = True Then
                ManuallyRatechange = True
            End If
        End If
        If ManuallyRatechange = True Then
            Return Val(objMIMRRate.txtNewRate_RATE.Text)
        End If
        Dim Metalid As String = ""
        strsql = " SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = '" & ItemId & "'"
        Metalid = GetSqlValue(cn, strsql)
        Dim gRate As Double = Nothing
        strsql = " SELECT TOP 1 SRATE FROM " & cnAdminDb & "..RATEMAST R"
        strsql += vbCrLf + " WHERE RDATE = '" & GetServerDate() & "'"
        strsql += vbCrLf + " AND RATEGROUP = (SELECT MAX(RATEGROUP) FROM " & cnAdminDb & "..RATEMAST WHERE RDATE = R.RDATE)"
        strsql += vbCrLf + " AND PURITY BETWEEN 91.6 AND 92 AND METALID = '" & Metalid & "'"
        strsql += vbCrLf + " AND SRATE <> 0"
        strsql += vbCrLf + " ORDER BY METALID,PURITY"
        gRate = Val(BrighttechPack.GlobalMethods.GetSqlValue(cn, strsql))
        Return gRate
    End Function

    Private Sub styleGrid(ByVal GridStyle As DataGridView)
        With GridStyle
            .Columns("KEYNO").Visible = False
            .Columns("METALNAME").Visible = False
            .Columns("METALID").Visible = False
            .Columns("CATCODE").Visible = False
            .Columns("CATNAME").Visible = False
            .Columns("ITEMID").Visible = False
            .Columns("SUBITEMID").Visible = False
            .Columns("GRSNET").Visible = False
            .Columns("BOARDRATE").Visible = False
            .Columns("SGSTPER").Visible = False
            .Columns("CGSTPER").Visible = False
            .Columns("IGSTPER").Visible = False
            .Columns("CGST").Visible = False
            .Columns("SGST").Visible = False
            .Columns("IGST").Visible = False
            .Columns("TRANTYPE").Visible = False
            .Columns("ALLOY").Visible = False
            .Columns("ACCODE").Visible = False
            .Columns("CALTYPE").Visible = False

            .Columns("ESTTAGNO").Visible = False
            .Columns("ESTBATCHNO").Visible = False
            .Columns("ESTSNO").Visible = False

            .Columns("ITEMTAGPCS").Visible = False
            .Columns("ITEMTAGGRSWT").Visible = False
            .Columns("ITEMTAGNETWT").Visible = False

            .Columns("TRANSTATUS").Visible = False
            .Columns("RATEFIXACCODE").Visible = False

            .Columns("ITEMNAME").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("SUBITEMNAME").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("KEYNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("PURITY").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("LESSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("Rate").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("TOUCH").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("PUREWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("WASTAGEPER").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("WASTAGE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("MCPER").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("MC").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("MCPIE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("STUDAMT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("GROSSAMT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("GST").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("TDSVAL").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("NETAMT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            'WASTAGE GSTPER
            .Columns("WSGSTPER").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("WCGSTPER").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("WIGSTPER").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("WSGST").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("WCGST").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("WIGST").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            FormatGridColumns(GridStyle, False, False, True, False)
            AutoResize()
            AutoResizeTotal()
            .Columns("KEYNO").DefaultCellStyle.Format = "#"
            .Columns("PURITY").DefaultCellStyle.Format = "0.00"
            .Columns("PCS").DefaultCellStyle.Format = "0.00"
            .Columns("GRSWT").DefaultCellStyle.Format = "0.00"
            .Columns("LESSWT").DefaultCellStyle.Format = "0.00"
            .Columns("NETWT").DefaultCellStyle.Format = "0.00"
            .Columns("Rate").DefaultCellStyle.Format = "0.00"
            .Columns("TOUCH").DefaultCellStyle.Format = "0.00"
            .Columns("PUREWT").DefaultCellStyle.Format = "0.00"
            .Columns("WASTAGEPER").DefaultCellStyle.Format = "0.00"
            .Columns("WASTAGE").DefaultCellStyle.Format = "0.00"
            .Columns("MCPER").DefaultCellStyle.Format = "0.00"
            .Columns("MC").DefaultCellStyle.Format = "0.00"
            .Columns("MCPIE").DefaultCellStyle.Format = "0.00"
            .Columns("STUDAMT").DefaultCellStyle.Format = "0.00"
            .Columns("GROSSAMT").DefaultCellStyle.Format = "0.00"
            .Columns("GST").DefaultCellStyle.Format = "0.00"
            .Columns("TDSVAL").DefaultCellStyle.Format = "0.00"
            .Columns("NETAMT").DefaultCellStyle.Format = "0.00"
            'WASTAGE GSTPER
            .Columns("WCGSTPER").DefaultCellStyle.Format = "0.00"
            .Columns("WSGSTPER").DefaultCellStyle.Format = "0.00"
            .Columns("WIGSTPER").DefaultCellStyle.Format = "0.00"
            .Columns("WSGST").DefaultCellStyle.Format = "0.00"
            .Columns("WCGST").DefaultCellStyle.Format = "0.00"
            .Columns("WIGST").DefaultCellStyle.Format = "0.00"
        End With
    End Sub

    Function GetDatatableWastage() As DataTable
        ' Create new DataTable instance.
        Dim table As New DataTable
        ' Create four typed columns in the DataTable.
        table.Columns.Add("WSGSTPER", GetType(Decimal))
        table.Columns.Add("WCGSTPER", GetType(Decimal))
        table.Columns.Add("WIGSTPER", GetType(Decimal))
        table.Columns.Add("GSTPER", GetType(Decimal))
        table.Columns.Add("WSGST", GetType(Decimal))
        table.Columns.Add("WCGST", GetType(Decimal))
        table.Columns.Add("WIGST", GetType(Decimal))
        table.Columns.Add("GST", GetType(Decimal))
        table.Columns.Add("WCALTYPE", GetType(String))
        table.Columns.Add("WPRATE", GetType(Decimal))
        Return table
    End Function

    Private Sub GetDatatable()
        With DtTran
            .Columns.Add("KEYNO", GetType(Integer))
            .Columns.Add("METALID", GetType(String))
            .Columns.Add("METALNAME", GetType(String))
            .Columns.Add("CATCODE", GetType(String))
            .Columns.Add("CATNAME", GetType(String))
            .Columns.Add("TRANTYPE", GetType(String)) 'ONLY FIND RECEIPT/ISSUE
            .Columns.Add("PAYMODE", GetType(String)) 'PAYMODE RRE,RPU ARE ETC
            .Columns.Add("ITEMID", GetType(Integer))
            .Columns.Add("ITEMNAME", GetType(String))
            .Columns.Add("SUBITEMID", GetType(String))
            .Columns.Add("SUBITEMNAME", GetType(String))
            .Columns.Add("PURITY", GetType(Decimal))
            .Columns.Add("PCS", GetType(Integer))
            .Columns.Add("GRSNET", GetType(String))
            .Columns.Add("GRSWT", GetType(Decimal))
            .Columns.Add("LESSWT", GetType(Decimal))
            .Columns.Add("NETWT", GetType(Decimal))
            .Columns.Add("RATE", GetType(Decimal))
            .Columns.Add("BOARDRATE", GetType(Decimal))
            .Columns.Add("TOUCH", GetType(Decimal))
            .Columns.Add("PUREWT", GetType(Decimal))
            .Columns.Add("WASTAGEPER", GetType(Decimal))
            .Columns.Add("WASTAGE", GetType(Decimal))
            .Columns.Add("MCPER", GetType(Decimal))
            .Columns.Add("MCGRAM", GetType(Decimal))
            .Columns.Add("MC", GetType(Decimal))
            .Columns.Add("MCPIE", GetType(Decimal))
            .Columns.Add("EMCPERG", GetType(Decimal))
            .Columns.Add("SGSTPER", GetType(Decimal))
            .Columns.Add("CGSTPER", GetType(Decimal))
            .Columns.Add("IGSTPER", GetType(Decimal))
            .Columns.Add("STUDAMT", GetType(Decimal))
            .Columns.Add("GROSSAMT", GetType(Decimal))
            .Columns.Add("SGST", GetType(Decimal))
            .Columns.Add("CGST", GetType(Decimal))
            .Columns.Add("IGST", GetType(Decimal))
            .Columns.Add("GST", GetType(Decimal))
            .Columns.Add("TDSPER", GetType(Decimal))
            .Columns.Add("TDSVAL", GetType(Decimal))
            .Columns.Add("TCSPER", GetType(Decimal))
            .Columns.Add("TCSVAL", GetType(Decimal))
            .Columns.Add("NETAMT", GetType(Decimal))
            .Columns.Add("ALLOY", GetType(Decimal))
            .Columns.Add("REMARK1", GetType(String))
            .Columns.Add("REMARK2", GetType(String))
            .Columns.Add("ACCODE", GetType(String))
            .Columns.Add("CALTYPE", GetType(String))

            .Columns.Add("WSGSTPER", GetType(Decimal))
            .Columns.Add("WCGSTPER", GetType(Decimal))
            .Columns.Add("WIGSTPER", GetType(Decimal))
            .Columns.Add("WSGST", GetType(Decimal))
            .Columns.Add("WCGST", GetType(Decimal))
            .Columns.Add("WIGST", GetType(Decimal))
            .Columns.Add("TRANSTATUS", GetType(String))
            .Columns.Add("RATEFIXACCODE", GetType(String))

            'EST
            .Columns.Add("ESTTAGNO", GetType(String))
            .Columns.Add("ESTBATCHNO", GetType(String))
            .Columns.Add("ESTSNO", GetType(String))

            'ITEMTAG 
            .Columns.Add("ITEMTAGPCS", GetType(Integer))
            .Columns.Add("ITEMTAGGRSWT", GetType(Double))
            .Columns.Add("ITEMTAGNETWT", GetType(Double))

            .Columns("KEYNO").AutoIncrement = True
            .Columns("KEYNO").AutoIncrementSeed = 1
            .Columns("KEYNO").AutoIncrementStep = 1
        End With
        With Gridview_OWN
            .Columns.Clear()
            .DataSource = Nothing
            .DataSource = DtTran
            styleGrid(Gridview_OWN)
        End With
        dtTranGrandTotal = New DataTable
        dtTranGrandTotal = DtTran.Copy
        With gridViewTotal_Own
            .DataSource = Nothing
            .DataSource = dtTranGrandTotal
        End With
    End Sub

    Private Sub LoadPurity(ByVal Cmb As ComboBox)
        strsql = "SELECT DISTINCT CONVERT(DECIMAL(12,2),PURITY) FROM " & cnAdminDb & "..PURITYMAST  "
        objGPack.FillCombo(strsql, Cmb, True, False)
        Cmb.Text = "91.60"
    End Sub
    Public Sub dtClear()
        dtAcname = New DataTable
        dtItemName = New DataTable
        dtSubItemName = New DataTable
        dtAcAddress = New DataTable
        DtTran = New DataTable
        dtStone = New DataTable
        dtGSTTDS = New DataTable
        GetDatatable()
    End Sub
    Public Function LoadTratypeStrsql(ByVal all As Boolean) As String
        Dim Qry As String = ""
        Qry = " "
        If all = True Then
            Qry += vbCrLf + "SELECT '' AS INVOICETYPE,'ALL' TRANTYPE"
            Qry += vbCrLf + "UNION ALL"
        End If
        Qry += vbCrLf + "SELECT 'RECEIPT' AS INVOICETYPE,'RECEIPT' TRANTYPE"
        Qry += vbCrLf + "UNION ALL"
        Qry += vbCrLf + "SELECT 'RECEIPT' AS INVOICETYPE,'PURCHASE' TRANTYPE"
        Qry += vbCrLf + "UNION ALL"
        Qry += vbCrLf + "SELECT 'RECEIPT' AS INVOICETYPE,'PURCHASE[APPROVAL]' TRANTYPE"
        Qry += vbCrLf + "UNION ALL"
        'Qry += vbCrLf + "SELECT 'RECEIPT' AS INVOICETYPE,'INTERNAL TRANSFER' TRANTYPE"
        'Qry += vbCrLf + "UNION ALL"
        'Qry += vbCrLf + "SELECT 'RECEIPT' AS INVOICETYPE,'APPROVAL RECEIPT' TRANTYPE"
        'Qry += vbCrLf + "UNION ALL"
        'Qry += vbCrLf + "SELECT 'RECEIPT' AS INVOICETYPE,'OTHER RECEIPT' TRANTYPE"
        'Qry += vbCrLf + "UNION ALL"
        'Qry += vbCrLf + "SELECT 'RECEIPT' AS INVOICETYPE,'PACKING' TRANTYPE"
        'Qry += vbCrLf + "UNION ALL"
        Qry += vbCrLf + "SELECT 'ISSUE' AS INVOICETYPE,'ISSUE' TRANTYPE"
        Qry += vbCrLf + "UNION ALL"
        Qry += vbCrLf + "SELECT 'ISSUE' AS INVOICETYPE,'PURCHASE RETURN' TRANTYPE"
        'Qry += vbCrLf + "UNION ALL"
        'Qry += vbCrLf + "SELECT 'ISSUE' AS INVOICETYPE,'INTERNAL TRANSFER' TRANTYPE"
        'Qry += vbCrLf + "UNION ALL"
        'Qry += vbCrLf + "SELECT 'ISSUE' AS INVOICETYPE,'APPROVAL ISSUE' TRANTYPE"
        'Qry += vbCrLf + "UNION ALL"
        'Qry += vbCrLf + "SELECT 'ISSUE' AS INVOICETYPE,'OTHER ISSUE' TRANTYPE"
        'Qry += vbCrLf + "UNION ALL"
        'Qry += vbCrLf + "SELECT 'ISSUE' AS INVOICETYPE,'PACKING' TRANTYPE"
        Return Qry
    End Function

    Public Function LoadTratypePrefixStrsqlChecked(ByVal all As Boolean, ByVal receipt As Boolean, ByVal issue As Boolean) As String
        Dim Qry As String = ""
        Qry = " "
        If all = True Then
            Qry += vbCrLf + "SELECT '' AS INVOICETYPE,'ALL' TRANTYPE"
            Qry += vbCrLf + "UNION ALL"
        End If
        If receipt = True Then
            Qry += vbCrLf + "SELECT 'RRE' AS INVOICETYPE,'RECEIPT-RRE' TRANTYPE"
            Qry += vbCrLf + "UNION ALL"
            Qry += vbCrLf + "SELECT 'RPU' AS INVOICETYPE,'PURCHASE-RPU' TRANTYPE"
            Qry += vbCrLf + "UNION ALL"
            Qry += vbCrLf + "SELECT 'RPA' AS INVOICETYPE,'PURCHASE[APPROVAL]-RPA' TRANTYPE"
        End If
        If issue = True Then
            If receipt = True Then
                Qry += vbCrLf + "UNION ALL"
            End If
            Qry += vbCrLf + "SELECT 'IIS' AS INVOICETYPE,'ISSUE-IIS' TRANTYPE"
            Qry += vbCrLf + "UNION ALL"
            Qry += vbCrLf + "SELECT 'IPU' AS INVOICETYPE,'PURCHASE RETURN-IPU' TRANTYPE"
        End If
        Return Qry
    End Function

    Public Function LoadTratypePrefixStrsql(ByVal all As Boolean, ByVal receipt As Boolean, ByVal issue As Boolean) As String
        Dim Qry As String = ""
        Qry = " "
        If all = True Then
            Qry += vbCrLf + "SELECT '' AS INVOICETYPE,'ALL' TRANTYPE"
            Qry += vbCrLf + "UNION ALL"
        End If
        If receipt = True Then
            Qry += vbCrLf + "SELECT 'RRE' AS INVOICETYPE,'RECEIPT' TRANTYPE"
            Qry += vbCrLf + "UNION ALL"
            Qry += vbCrLf + "SELECT 'RPU' AS INVOICETYPE,'PURCHASE' TRANTYPE"
            Qry += vbCrLf + "UNION ALL"
            Qry += vbCrLf + "SELECT 'RPA' AS INVOICETYPE,'PURCHASE[APPROVAL]' TRANTYPE"
        End If
        If issue = True Then
            If receipt = True Then
                Qry += vbCrLf + "UNION ALL"
            End If
            Qry += vbCrLf + "SELECT 'IIS' AS INVOICETYPE,'ISSUE' TRANTYPE"
            Qry += vbCrLf + "UNION ALL"
            Qry += vbCrLf + "SELECT 'IPU' AS INVOICETYPE,'PURCHASE RETURN' TRANTYPE"
        End If
        Return Qry
    End Function
    Public Sub LoadTrantype()
        'cmbTrantype.Items.Clear()
        'If cmbInvoiceType.Text = "RECEIPT" Then
        '    cmbTrantype.Items.Add("RECEIPT")
        '    cmbTrantype.Items.Add("PURCHASE")
        '    cmbTrantype.Items.Add("PURCHASE[APPROVAL]")
        '    cmbTrantype.Items.Add("INTERNAL TRANSFER")
        '    cmbTrantype.Items.Add("APPROVAL RECEIPT")
        '    cmbTrantype.Items.Add("OTHER RECEIPT")
        '    cmbTrantype.Items.Add("PACKING")
        '    cmbTrantype.Text = "RECEIPT"
        '    oMaterial = Material.Receipt
        'ElseIf cmbInvoiceType.Text = "ISSUE" Then
        '    cmbTrantype.Items.Add("ISSUE")
        '    cmbTrantype.Items.Add("PURCHASE RETURN")
        '    cmbTrantype.Items.Add("INTERNAL TRANSFER")
        '    cmbTrantype.Items.Add("APPROVAL ISSUE")
        '    cmbTrantype.Items.Add("OTHER ISSUE")
        '    cmbTrantype.Items.Add("PACKING")
        '    cmbTrantype.Text = "ISSUE"
        '    oMaterial = Material.Issue
        'End If
        strsql = LoadTratypeStrsql(False)
        Dim dtTrantype As New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dtTrantype)
        If dtTrantype.Rows.Count > 0 Then
            With dtTrantype
                cmbTrantype.DataSource = Nothing
                cmbTrantype.DataSource = dtTrantype
                cmbTrantype.ValueMember = "INVOICETYPE"
                cmbTrantype.DisplayMember = "TRANTYPE"
            End With
        End If
        ' lblTitle.Text = "MATERIAL " & cmbInvoiceType.Text
        lblTitle.Text = "MATERIAL " & cmbTrantype.Text
    End Sub
    Public Sub LoadTrantypeReport(ByVal all As Boolean, ByVal receipt As Boolean, ByVal issue As Boolean)
        strsql = LoadTratypePrefixStrsql(all, receipt, issue)
        Dim dtTrantype As New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dtTrantype)
        If dtTrantype.Rows.Count > 0 Then
            With dtTrantype
                cmbTrantypeRPT.DataSource = Nothing
                cmbTrantypeRPT.DataSource = dtTrantype
                cmbTrantypeRPT.ValueMember = "INVOICETYPE"
                cmbTrantypeRPT.DisplayMember = "TRANTYPE"
            End With
        End If
    End Sub

    'Public Sub LoadTrantype()
    '    cmbTrantype.Items.Clear()
    '    If cmbInvoiceType.Text = "RECEIPT" Then
    '        cmbTrantype.Items.Add("RECEIPT")
    '        cmbTrantype.Items.Add("PURCHASE")
    '        cmbTrantype.Items.Add("PURCHASE[APPROVAL]")
    '        cmbTrantype.Items.Add("INTERNAL TRANSFER")
    '        cmbTrantype.Items.Add("APPROVAL RECEIPT")
    '        cmbTrantype.Items.Add("OTHER RECEIPT")
    '        cmbTrantype.Items.Add("PACKING")
    '        cmbTrantype.Text = "RECEIPT"
    '        oMaterial = Material.Receipt
    '    ElseIf cmbInvoiceType.Text = "ISSUE" Then
    '        cmbTrantype.Items.Add("ISSUE")
    '        cmbTrantype.Items.Add("PURCHASE RETURN")
    '        cmbTrantype.Items.Add("INTERNAL TRANSFER")
    '        cmbTrantype.Items.Add("APPROVAL ISSUE")
    '        cmbTrantype.Items.Add("OTHER ISSUE")
    '        cmbTrantype.Items.Add("PACKING")
    '        cmbTrantype.Text = "ISSUE"
    '        oMaterial = Material.Issue
    '    End If
    '    lblTitle.Text = "MATERIAL " & cmbInvoiceType.Text
    'End Sub
    'Public Sub LoadInvoiceType()
    '    cmbInvoiceType.Items.Clear()
    '    cmbInvoiceType.Items.Add("RECEIPT")
    '    cmbInvoiceType.Items.Add("ISSUE")
    '    cmbInvoiceType.Text = "RECEIPT"
    'End Sub

    Public Sub ClearTextBox(ByVal root As Control, ByVal particularTextboxnotclear As Boolean)
        For Each ctrl As Control In root.Controls
            If particularTextboxnotclear = True Then
                If ctrl.Name.ToString = "dtpInvoiceDate" Then
                    Continue For
                End If
                If ctrl.Name.ToString = "txtInvoiceNo" Then
                    Continue For
                End If
                If ctrl.Name.ToString = "cmbInvoiceType" Then
                    Continue For
                End If
                If ctrl.Name.ToString = "cmbTrantype" Then
                    Continue For
                End If
                If ctrl.Name.ToString = "dtpTrandate" Then
                    Continue For
                End If
                If ctrl.Name.ToString = "dtpInvoiceDate_OWN" Then
                    Continue For
                End If
            End If
            ClearTextBox(ctrl, particularTextboxnotclear)
            If TypeOf ctrl Is TextBox Then
                CType(ctrl, TextBox).Text = String.Empty
            End If
        Next ctrl
    End Sub
    Private Sub ClearGSTLabel()
        lblCGSTPer.Text = "..."
        lblSGSTPer.Text = "..."
        lblIGSTPer.Text = "..."
    End Sub

    Public Sub ClearLabel()
        lblAddress.Text = "Address"
        lblAddress1.Text = "Address"
        lblGSTNo.Text = "Address"
        ClearGSTLabel()
    End Sub

    Public Function strLoadAcname() As String
        Dim Qry As String = ""
        Qry = " "
        Qry += vbCrLf + " SELECT ACCODE,ACNAME FROM ("
        Qry += vbCrLf + " SELECT '' ACCODE, '' ACNAME,0 RESULT"
        Qry += vbCrLf + " UNION ALL"
        Qry += vbCrLf + " SELECT ACCODE,ACNAME,1 RESULT FROM " & cnAdminDb & "..ACHEAD "
        Qry += vbCrLf + " WHERE ISNULL(ACTIVE,'') = 'Y' "
        If InclCusttype = "Y" Then
            Qry += " AND ACTYPE IN (SELECT DISTINCT TYPEID FROM " & cnAdminDb & "..ACCTYPE "
            Qry += " WHERE ACTYPE IN ('G','D','I','C'))"
        Else
            Qry += " AND ACTYPE IN (SELECT DISTINCT TYPEID FROM " & cnAdminDb & "..ACCTYPE "
            Qry += " WHERE ACTYPE IN ('G','D',''))" 'I'
        End If
        Qry += vbCrLf + " )X"
        Qry += vbCrLf + " ORDER BY RESULT,ACNAME"
        Return Qry
    End Function
    Public Sub LoadAcname()
        strsql = strLoadAcname()
        dtAcname = New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dtAcname)
        If dtAcname.Rows.Count > 0 Then
            cmbAcname_OWN.DataSource = Nothing
            cmbAcname_OWN.DataSource = dtAcname
            cmbAcname_OWN.ValueMember = "ACCODE"
            cmbAcname_OWN.DisplayMember = "ACNAME"
            cmbAcname_OWN.AutoCompleteMode = AutoCompleteMode.SuggestAppend
            cmbAcname_OWN.AutoCompleteSource = AutoCompleteSource.ListItems
        End If
    End Sub
    Public Sub LoadAcnameSupplier()
        Dim dtAcname As DataTable
        strsql = strLoadAcname()
        dtAcname = New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dtAcname)
        If dtAcname.Rows.Count > 0 Then
            cmbAcnameAcname_Own.DataSource = Nothing
            cmbAcnameAcname_Own.DataSource = dtAcname
            cmbAcnameAcname_Own.ValueMember = "ACCODE"
            cmbAcnameAcname_Own.DisplayMember = "ACNAME"
            cmbAcnameAcname_Own.AutoCompleteMode = AutoCompleteMode.SuggestAppend
            cmbAcnameAcname_Own.AutoCompleteSource = AutoCompleteSource.ListItems
        End If
    End Sub

    Public Sub LoadItemName()
        strsql = ""
        strsql += vbCrLf + " SELECT ITEMID,ITEMNAME FROM ("
        strsql += vbCrLf + " SELECT 0 ITEMID,'' ITEMNAME,0 RESULT"
        strsql += vbCrLf + " UNION ALL"
        strsql += vbCrLf + " SELECT ITEMID,ITEMNAME,1 RESULT "
        strsql += vbCrLf + " FROM " & cnAdminDb & "..ITEMMAST "
        strsql += vbCrLf + " WHERE ACTIVE = 'Y'"
        strsql += vbCrLf + " )X ORDER BY RESULT,ITEMNAME"
        dtItemName = New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dtItemName)
        If dtItemName.Rows.Count > 0 Then
            cmbItem_OWN.DataSource = Nothing
            cmbItem_OWN.DataSource = dtItemName
            cmbItem_OWN.ValueMember = "ITEMID"
            cmbItem_OWN.DisplayMember = "ITEMNAME"
            cmbItem_OWN.AutoCompleteMode = AutoCompleteMode.SuggestAppend
            cmbItem_OWN.AutoCompleteSource = AutoCompleteSource.ListItems
        End If
    End Sub

    Public Sub LoadSubItemName(ByVal Itemid As Integer)
        strsql = ""
        strsql += vbCrLf + " SELECT SUBITEMID,SUBITEMNAME FROM ("
        strsql += vbCrLf + " SELECT 0 SUBITEMID,'' SUBITEMNAME,0 RESULT"
        strsql += vbCrLf + " UNION ALL"
        strsql += vbCrLf + " SELECT SUBITEMID,SUBITEMNAME,1 RESULT "
        strsql += vbCrLf + " FROM " & cnAdminDb & "..SUBITEMMAST "
        strsql += vbCrLf + " WHERE ACTIVE = 'Y'"
        strsql += vbCrLf + " AND ITEMID = '" & Itemid & "' "
        strsql += vbCrLf + " )X ORDER BY RESULT,SUBITEMNAME"
        dtSubItemName = New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dtSubItemName)
        If dtSubItemName.Rows.Count > 0 Then
            cmbSubItem_OWN.DataSource = Nothing
            cmbSubItem_OWN.DataSource = dtSubItemName
            cmbSubItem_OWN.ValueMember = "SUBITEMID"
            cmbSubItem_OWN.DisplayMember = "SUBITEMNAME"
        End If
    End Sub

    Public Sub GetItemName(ByVal ItemId As Integer)
        If ItemId > 0 Then
            strsql = ""
            strsql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = '" & ItemId & "' AND ISNULL(ACTIVE,'') = 'Y'"
            cmbItem_OWN.Text = GetSqlValue(cn, strsql).ToString
        End If
    End Sub

    Private Function strAchead(ByVal accode As String) As String
        Dim Qry As String = ""
        Qry = " SELECT * FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = '" & accode & "' AND ISNULL(ACTIVE,'') = 'Y'"
        Return Qry
    End Function

    Private Function getGridTrantype() As String
        Dim GridTrantype As String = ""
        Dim dtDistinctRece As New DataTable
        If Gridview_OWN.Rows.Count > 0 Then
            dtDistinctRece = Gridview_OWN.DataSource
            dtDistinctRece = dtDistinctRece.DefaultView.ToTable(True, "PAYMODE")
        End If
        If dtDistinctRece.Rows.Count > 0 Then
            GridTrantype = dtDistinctRece.Rows(0).Item("PAYMODE").ToString
        End If
        Return GridTrantype
    End Function

    Public Sub funTdsNew_calculationmethod(ByVal accode As String, ByVal tdsper As Double, ByVal currentAmtTDS As Double, ByVal TdsYesorNoFirst As Boolean)
        Dim dsTdsCal As New DataSet
        Dim dtTdsCal As New DataTable
        strsql = ""
        strsql += vbCrLf + " EXEC " & cnAdminDb & "..[SP_PURCHASETDS_DEDUCTION_CALC] "
        strsql += vbCrLf + " @SALADMINDB='" & cnAdminDb & "'"
        strsql += vbCrLf + " ,@SYSTEMNAME = '" & systemId & "'"
        strsql += vbCrLf + " ,@TODAYFROMDATE ='" & Format(dtpTrandate.Value.Date, "yyyy-MM-dd") & "'"
        strsql += vbCrLf + " ,@ACCODE = '" & accode & "'"
        strsql += vbCrLf + " ,@AMOUNT ='5000000'"
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dsTdsCal)
        dtTdsCal = dsTdsCal.Tables(0)
        If dtTdsCal.Rows.Count > 0 Then
            If tdsper > 0 And Val(dtTdsCal.Rows(0).Item("AMOUNT").ToString) > 0 Then
                txtConTDSPER_AMT.Text = Format(Val(tdsper), "0.00")
            End If
        End If
        If tdsper > 0 And currentAmtTDS > 5000000 And Val(txtConTDSPER_AMT.Text) = 0 Then
            If Val(txtConTDS_AMT.Text) = 0 Then
                txtConTDSPER_AMT.Text = Format(Val(tdsper), "0.00")
            End If
        End If
        If Val(txtConTDSPER_AMT.Text) = 0 Then
            If TdsYesorNoFirst = True Then
                'If MsgBox("TDS Deduct Yes Or No...!", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton1) = MsgBoxResult.Yes Then
                '    txtConTDSPER_AMT.Text = 0.1 'Format(Val(tdsper), "0.00")
                '    txtItemId_NUM.Focus()
                '    txtItemId_NUM.SelectAll()
                '    PURCHASE_TDS_YNFIRSTTIME = False
                'End If
            Else
                'txtConTDSPER_AMT.Text = 0.1 'Format(Val(tdsper), "0.00")
                'txtItemId_NUM.Focus()
                'txtItemId_NUM.SelectAll()
            End If
        End If
    End Sub

    Public Sub GetSupplierName(ByVal accode As String)
        strsql = strAchead(accode)
        dtAcAddress = GetSqlTable(strsql, cn, Nothing)
        If dtAcAddress.Rows.Count > 0 Then
            With dtAcAddress.Rows(0)
                lblAddress.Text = .Item("ADDRESS1").ToString & " " & .Item("ADDRESS2").ToString & " " & .Item("ADDRESS3").ToString & " " & .Item("AREA").ToString
                lblAddress1.Text = .Item("CITY").ToString & " " & .Item("STATE").ToString & " " & .Item("COUNTRY").ToString & " " & .Item("PINCODE").ToString & " " & .Item("MOBILE").ToString
                lblGSTNo.Text = "GSTIN : " & .Item("GSTNO").ToString
                If .Item("PAN").ToString.Trim = "" Then
                    ''MsgBox("PAN is empty, Please update PAN ", MsgBoxStyle.Information)
                    ''cmbAcname_OWN.SelectAll()
                    ''cmbAcname_OWN.Focus()
                End If
                If .Item("TDSFLAG").ToString = "Y" Or .Item("TDSFLAG").ToString = "N" Or .Item("TDSFLAG").ToString = "" Then
                    If Val(.Item("TDSPER").ToString) = 0 Then
                        ''If userId = 999 Then
                        ''    If MsgBox("TDS% is empty, Please update TDS ", MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                        ''        cmbAcname_OWN.SelectAll()
                        ''        cmbAcname_OWN.Focus()
                        ''    End If
                        ''Else
                        ''    MsgBox("TDS% is empty, Please update TDS ", MsgBoxStyle.Information)
                        ''    cmbAcname_OWN.SelectAll()
                        ''    cmbAcname_OWN.Focus()
                        ''End If
                    End If
                End If
                strsql = ""
                Dim getGridTT As String = getGridTrantype()
                txtConTDSPER_AMT.Text = "0.00"
                txtConTCSPER_WET.Text = "0.000"
                If cmbTrantype.Text = "PURCHASE" Then
                    If txtInvoiceNo.Text.Trim = "" Then
                        MsgBox("Enter the Invoice No..", MsgBoxStyle.Information)
                        txtInvoiceNo.SelectAll()
                        txtInvoiceNo.Focus()
                        Exit Sub
                    End If
                    If Val(.Item("TCSPER").ToString) > 0 Then
                        'asper rules 06-July-2021
                        'txtConTCSPER_WET.Text = Format(Val(.Item("TCSPER").ToString), "0.000")
                    End If
                    funTdsNew_calculationmethod(accode, Val(.Item("TDSPER").ToString), Val(txtNetVal_AMT.Text), PURCHASE_TDS_YNFIRSTTIME)
                ElseIf cmbTrantype.Text = "PURCHASE RETURN" Then
                    funTdsNew_calculationmethod(accode, Val(.Item("TDSPER").ToString), Val(txtNetVal_AMT.Text), PURCHASE_TDS_YNFIRSTTIME)
                ElseIf cmbTrantype.Text = "RECEIPT" Then
                    If Val(.Item("TDSPER").ToString) > 0 Then
                        txtConTDSPER_AMT.Text = Format(Val(.Item("TDSPER").ToString), "0.00")
                    End If
                ElseIf getGridTT = "PURCHASE" Then
                    If Val(.Item("TCSPER").ToString) > 0 Then
                        txtConTCSPER_WET.Text = Format(Val(.Item("TCSPER").ToString), "0.000")
                    End If
                ElseIf getGridTT = "RECEIPT" Then
                    If Val(.Item("TDSPER").ToString) > 0 Then
                        txtConTDSPER_AMT.Text = Format(Val(.Item("TDSPER").ToString), "0.00")
                    End If
                Else
                    'At present No Need
                    'If Val(.Item("TCSPER").ToString) > 0 Then
                    '    txtConTCSPER_WET.Text = Format(Val(.Item("TCSPER").ToString), "0.000")
                    'End If
                    'If Val(.Item("TDSPER").ToString) > 0 Then
                    '    txtConTDSPER_AMT.Text = Format(Val(.Item("TDSPER").ToString), "0.00")
                    'End If
                End If
                Dim Dealer_wmcTax As String = ""
                strsql = " SELECT  TOP 1 ISNULL(TAXINCLUCIVE,'')TAXINCLUCIVE "
                strsql += vbCrLf + " FROM " & cnAdminDb & "..DEALER_WMCTABLE  "
                strsql += vbCrLf + " WHERE ACCODE='" & accode & "' "
                Dealer_wmcTax = objGPack.GetSqlValue(strsql).ToString
                If Dealer_wmcTax = "Y" Then
                ElseIf Dealer_wmcTax = "N" Then
                ElseIf Dealer_wmcTax = "E" Then
                Else
                End If
                If .Item("GSTNO").ToString = "" Or .Item("GSTNO").ToString = "UNREGISTERED" Then
                End If
                strsql = ""
            End With
            strsql = ""
        End If
    End Sub
    Private Sub LoadProcess(ByVal Cmb As ComboBox)
        strsql = vbCrLf + " SELECT ORDSTATE_NAME,ORDSTATE_ID FROM " & cnAdminDb & "..ORDERSTATUS ORDER BY ORDSTATE_NAME"
        Dim dtMetal As New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dtMetal)
        Cmb.DataSource = dtMetal
        Cmb.DisplayMember = "ORDSTATE_NAME"
        Cmb.ValueMember = "ORDSTATE_ID"
        'Cmb.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        'Cmb.AutoCompleteSource = AutoCompleteSource.ListItems
    End Sub
#End Region

#Region " Form Load"
    Private Sub frmMIMR_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        cmbAcname_OWN.Focus()
        cmbAcname_OWN.SelectAll()
    End Sub
    Private Sub frmMIMR_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{tab}")
        ElseIf e.KeyCode = Keys.Escape Then
            tabGeneral.SelectedTab = tabTran
        End If
    End Sub
    Private Sub frmMIMR_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        tabGeneral.ItemSize = New System.Drawing.Size(1, 1)
        'Me.tabGeneral.Region = New Region(New RectangleF(Me.tabGeneral.Left, Me.tabGeneral.Top, Me.tabGeneral.Width, Me.tabGeneral.Height))
        btnNew_Click(Me, New System.EventArgs)
        btnNewRpt_Click(Me, New System.EventArgs)
        btnNewRTGS_Click(Me, New System.EventArgs)
        btnNewDailyRpt_Click(Me, New System.EventArgs)
        btnNewAcname_Click(Me, New System.EventArgs)
    End Sub
    Private Sub frmMIMR_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        objWriter.Close()
    End Sub

    Private Sub frmMIMR_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        objWriter.Close()
    End Sub
#End Region

#Region "Button Events"
    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            btnOk.Enabled = False
            funcAddGrid()
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        Finally
            btnOk.Enabled = True
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

    End Sub
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        If MsgBox("Do you want to close ", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2) = MsgBoxResult.Yes Then
            Me.Close()
        End If
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        ClearTextBox(Me, False)
        funMultiClear()
        LoadAcname()
        funLoadGRSNET()
        LoadProcess(cmbProcess_OWN)
        LoadItemName()
        dtpInvoiceDate_OWN.Value = GetServerDate()
        dtpTrandate.Value = GetServerDate()
        'LoadInvoiceType()
        LoadTrantype()
        ClearLabel()
        dtClear()
        'LoadPurity(cmbPurityper_MAN)
        objStone = New frmStoneDiaAc
        'stonepropint()
        cmbAcname_OWN.Focus()
        cmbAcname_OWN.SelectAll()
        EditBatchno = ""
        BatchNo = Nothing
        _Accode = ""
        _Acctype = ""
        TranNo = Nothing
        Transistno = 0
        TranNoApp = Nothing
        _StkType = ""
        _CashCtr = ""
        objMIMRRate = New MaterialIssRecRateChangeBox
        ManuallyRatechange = False
        'RECEIPT TRANNO
        RPU_TRANNO = 0
        RIN_TRANNO = 0
        RPA_TRANNO = 0
        RAP_TRANNO = 0
        RRE_TRANNO = 0
        'ISSUE TRANNO
        IPA_TRANNO = 0
        IIN_TRANNO = 0
        IAP_TRANNO = 0
        IPU_TRANNO = 0
        IIS_TRANNO = 0
        lblSumPcs.Text = "..."
        lblSumGrswt.Text = "..."
        lblSumStone.Text = "..."
        lblSumNetwt.Text = "..."
        lblSumPureWt.Text = "..."
        lblSumYield.Text = "..."
        lblSumMc.Text = "..."
        lblSumAmount.Text = "..."
        objMIMRCd = New frmMIMRDebitNoteCreditNote
        funcdroptemptabledbMimrdebitcreditnote()
        PURCHASE_TDS_YNFIRSTTIME = True
        objMIMRfrmEst = New frmMIMREstDetail()
        funclearESTVariable()
        txtRemarkreadonlytrueorfalse(False, False)
    End Sub

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click, OpenToolStripMenuItem.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        tabGeneral.SelectedTab = tabAcnameDetail
        btnNewAcname_Click(Me, New System.EventArgs)
    End Sub

    Dim RPU_TRANNO As Integer = 0
    Dim RIN_TRANNO As Integer = 0
    Dim RPA_TRANNO As Integer = 0
    Dim RAP_TRANNO As Integer = 0
    Dim RRE_TRANNO As Integer = 0

    Dim IPA_TRANNO As Integer = 0
    Dim IIN_TRANNO As Integer = 0
    Dim IAP_TRANNO As Integer = 0
    Dim IPU_TRANNO As Integer = 0
    Dim IIS_TRANNO As Integer = 0

    Private Function funcPaymode(ByVal Paymode As String, ByVal Trantype As String) As Integer
        Dim billgencatcode As String = ""
        Dim billcontrolid As String = ""
        If EditBatchno = Nothing Then
            If True Then
GenBillNo:
                If Trantype = "RECEIPT" Then 'oMaterial = Material.Receipt
                    Select Case Paymode 'cmbTrantype.Text
                        Case "PURCHASE", "PURCHASE[APPROVAL]"
                            If RPU_TRANNO > 0 Then Return RPU_TRANNO
                            billcontrolid = "GEN-SM-RECPUR"
                            If CatbaseRecIsstranno And billgencatcode <> "" Then billcontrolid = "CAT-" + billgencatcode + "-SM-REC"
                            If _AccAudit Then billcontrolid = billcontrolid + "-APP"
                            strsql = "SELECT CTLMODE FROM " & cnStockDb & "..BILLCONTROL"
                            strsql += " WHERE CTLID ='" & billcontrolid & "' AND COMPANYID = '" & strCompanyId & "'"
                            If UCase(objGPack.GetSqlValue(strsql, , , Tran)) = "Y" Then
                                TranNo = GetBillNoValue(billcontrolid, Tran)
                            Else
                                billcontrolid = "GEN-SM-REC"
                                TranNo = GetBillNoValue(billcontrolid, Tran)
                            End If
                            If Paymode = "PURCHASE[APPROVAL]" Then 'cmbTrantype.Text
                                billcontrolid = "GEN-SM-ISS"
                                TranNoApp = GetBillNoValue(billcontrolid, Tran)
                            End If
                            RPU_TRANNO = TranNo
                            Return RPU_TRANNO
                        Case "INTERNAL TRANSFER"
                            If RIN_TRANNO > 0 Then Return RIN_TRANNO
                            billcontrolid = "GEN-SM-INTREC"
                            If CatbaseRecIsstranno And billgencatcode <> "" Then billcontrolid = "CAT-" + billgencatcode + "-SM-INTREC"
                            If _AccAudit Then billcontrolid = billcontrolid + "-APP"
                            strsql = "SELECT CTLMODE FROM " & cnStockDb & "..BILLCONTROL"
                            strsql += " WHERE CTLID ='" & billcontrolid & "' AND COMPANYID = '" & strCompanyId & "'"
                            If UCase(objGPack.GetSqlValue(strsql, , , Tran)) = "Y" Then
                                TranNo = GetBillNoValue(billcontrolid, Tran)
                            Else
                                billcontrolid = "GEN-SM-REC"
                                TranNo = GetBillNoValue(billcontrolid, Tran)
                            End If
                            RIN_TRANNO = TranNo
                            Return RIN_TRANNO
                            '                                TranNo = GetBillNoValue(billcontrolid, tran)
                        Case "PACKING"
                            If RPA_TRANNO > 0 Then Return RPA_TRANNO
                            If GetBillControlValue("GEN-PMSEPERATENO", Tran) = "Y" Then
                                TranNo = GetBillNoValue("GEN-PMRECEIPTNO", Tran)
                            Else
                                'CAT-00006-ACC'
                                billcontrolid = "GEN-SM-REC"
                                If CatbaseRecIsstranno And billgencatcode <> "" Then billcontrolid = "CAT-" + billgencatcode + "-SM-REC"
                                If _AccAudit Then billcontrolid = billcontrolid + "-APP"
                                TranNo = GetBillNoValue(billcontrolid, Tran)
                            End If
                            RPA_TRANNO = TranNo
                            Return RPA_TRANNO
                        Case "APPROVAL RECEIPT"
                            If RAP_TRANNO > 0 Then Return RAP_TRANNO
                            billcontrolid = "GEN-SM-REC"
                            If CatbaseRecIsstranno And billgencatcode <> "" Then billcontrolid = "CAT-" + billgencatcode + "-SM-REC"
                            If GetBillControlValue("GEN-SMRECAPPSEPERATENO", Tran) = "Y" Then
                                billcontrolid = billcontrolid + "-APP"
                            Else
                                If _AccAudit Then billcontrolid = billcontrolid + "-APP"
                            End If
                            TranNo = GetBillNoValue(billcontrolid, Tran)
                            RAP_TRANNO = TranNo
                            Return RAP_TRANNO
                        Case Else
                            If RRE_TRANNO > 0 Then Return RRE_TRANNO
                            billcontrolid = "GEN-SM-REC"
                            If CatbaseRecIsstranno And billgencatcode <> "" Then billcontrolid = "CAT-" + billgencatcode + "-SM-REC"
                            If _AccAudit Then billcontrolid = billcontrolid + "-APP"
                            TranNo = GetBillNoValue(billcontrolid, Tran)
                            RRE_TRANNO = TranNo
                            Return RRE_TRANNO
                            'TranNo = GetBillNoValue("GEN-SM-REC" & IIf(_AccAudit, "-APP", ""), tran)
                    End Select
                Else
                    Select Case Paymode 'cmbTrantype.Text
                        Case "PACKING"
                            If IPA_TRANNO > 0 Then Return IPA_TRANNO
                            If GetBillControlValue("GEN-PMSEPERATENO", Tran) = "Y" Then
                                TranNo = GetBillNoValue("GEN-PMISSUENO", Tran)
                            Else
                                billcontrolid = "GEN-SM-ISS"
                                If CatbaseRecIsstranno And billgencatcode <> "" Then billcontrolid = "CAT-" + billgencatcode + "-SM-ISS"
                                If _AccAudit Then billcontrolid = billcontrolid + "-APP"
                                TranNo = GetBillNoValue(billcontrolid, Tran)
                            End If
                            IPA_TRANNO = TranNo
                            Return IPA_TRANNO
                        Case "INTERNAL TRANSFER"
                            If IIN_TRANNO > 0 Then Return IIN_TRANNO
                            billcontrolid = "GEN-SM-INTISS"
                            If CatbaseRecIsstranno And billgencatcode <> "" Then billcontrolid = "CAT-" + billgencatcode + "-SM-INTISS"
                            If _AccAudit Then billcontrolid = billcontrolid + "-APP"
                            strsql = "SELECT CTLMODE FROM " & cnStockDb & "..BILLCONTROL"
                            strsql += " WHERE CTLID ='" & billcontrolid & "' AND COMPANYID = '" & strCompanyId & "'"
                            If UCase(objGPack.GetSqlValue(strsql, , , Tran)) = "Y" Then
                                TranNo = GetBillNoValue(billcontrolid, Tran)
                            Else
                                billcontrolid = "GEN-SM-ISS"
                                TranNo = GetBillNoValue(billcontrolid, Tran)
                            End If
                            strsql = "SELECT CTLMODE FROM " & cnStockDb & "..BILLCONTROL"
                            strsql += " WHERE CTLID ='GEN-TRANSISTNO' AND COMPANYID = '" & strCompanyId & "'"
                            If UCase(objGPack.GetSqlValue(strsql, , , Tran)) = "Y" Then Transistno = GetBillNoValue("GEN-TRANSISTNO", Tran)
                            IIN_TRANNO = TranNo
                            Return IIN_TRANNO
                        Case "APPROVAL ISSUE"
                            If IAP_TRANNO > 0 Then Return IAP_TRANNO
                            billcontrolid = "GEN-SM-ISS"
                            If CatbaseRecIsstranno And billgencatcode <> "" Then billcontrolid = "CAT-" + billgencatcode + "-SM-ISS"
                            If GetBillControlValue("GEN-SMISSAPPSEPERATENO", Tran) = "Y" Then
                                billcontrolid = billcontrolid + "-APP"
                            Else
                                If _AccAudit Then billcontrolid = billcontrolid + "-APP"
                            End If
                            TranNo = GetBillNoValue(billcontrolid, Tran)
                            IAP_TRANNO = TranNo
                            Return IAP_TRANNO
                        Case "PURCHASE RETURN"
                            If IPU_TRANNO > 0 Then Return IPU_TRANNO
                            billcontrolid = "GEN-SM-IPU"
                            If CatbaseRecIsstranno And billgencatcode <> "" Then billcontrolid = "CAT-" + billgencatcode + "-SM-IPU"
                            If _AccAudit Then billcontrolid = billcontrolid + "-APP"
                            TranNo = GetBillNoValue(billcontrolid, Tran)
                            IPU_TRANNO = TranNo
                            Return IPU_TRANNO
                        Case Else
                            If IIS_TRANNO > 0 Then Return IIS_TRANNO
                            billcontrolid = "GEN-SM-ISS"
                            If CatbaseRecIsstranno And billgencatcode <> "" Then billcontrolid = "CAT-" + billgencatcode + "-SM-ISS"
                            If _AccAudit Then billcontrolid = billcontrolid + "-APP"
                            TranNo = GetBillNoValue(billcontrolid, Tran)
                            'TranNo = GetBillNoValue("GEN-SM-ISS" & IIf(_AccAudit, "-APP", ""), tran)
                            IIS_TRANNO = TranNo
                            Return IIS_TRANNO
                    End Select
                End If
            End If
        Else
            'strsql = " SELECT SNO,TRANNO FROM  " & cnStockDb & "..RECEIPT WHERE BATCHNO = '" & EditBatchno & "' ORDER BY SNO "
            'DtEditDet = New DataTable
            'cmd = New OleDbCommand(strsql, cn, Tran)
            'da = New OleDbDataAdapter(cmd)
            'da.Fill(DtEditDet)

            'strsql = " SELECT RESNO FROM  " & cnStockDb & "..ISSUE WHERE BATCHNO = '" & EditBatchno & "' ORDER BY SNO "
            'DtEditIss = New DataTable
            'cmd = New OleDbCommand(strsql, cn, Tran)
            'da = New OleDbDataAdapter(cmd)
            'da.Fill(DtEditIss)

            'strsql = " DELETE FROM " & cnStockDb & "..ISSUE WHERE BATCHNO = '" & EditBatchno & "'"
            'strsql += vbCrLf + " DELETE FROM " & cnStockDb & "..ISSSTONE WHERE BATCHNO = '" & EditBatchno & "'"
            'strsql += vbCrLf + " DELETE FROM " & cnStockDb & "..ISSMISC WHERE BATCHNO = '" & EditBatchno & "'"
            'strsql += vbCrLf + " DELETE FROM " & cnStockDb & "..RECEIPT WHERE BATCHNO = '" & EditBatchno & "'"
            'strsql += vbCrLf + " DELETE FROM " & cnStockDb & "..RECEIPTSTONE WHERE BATCHNO = '" & EditBatchno & "'"
            'strsql += vbCrLf + " DELETE FROM " & cnStockDb & "..RECEIPTMISC WHERE BATCHNO = '" & EditBatchno & "'"
            'strsql += vbCrLf + " DELETE FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = '" & EditBatchno & "'"
            'strsql += vbCrLf + " DELETE FROM " & cnAdminDb & "..OUTSTANDING WHERE BATCHNO = '" & EditBatchno & "'"
            'strsql += vbCrLf + " DELETE FROM " & cnStockDb & "..ALLOYDETAILS WHERE BATCHNO = '" & EditBatchno & "'"
            'strsql += vbCrLf + " DELETE FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO = '" & EditBatchno & "'"

            'ExecQuery(SyncMode.Transaction, strsql, cn, Tran, CostCenterId)
            'If CostCenterId <> EditCostId Then
            '    Exec(strsql.Replace("'", "''"), cn, EditCostId, Nothing, Tran)
            'End If
            'TranNo = EditTranNo
            'BatchNo = EditBatchno
        End If
    End Function

    Private Function checkMIMR() As Boolean
        Dim serverDate As Date = GetActualEntryDate()
        Dim RestrictDays As String = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'DATECONTROL_MIMR'")
        If RestrictDays.Contains(",") = False Then
            If Not (dtpTrandate.Value >= serverDate.AddDays(-1 * Val(RestrictDays))) Then
                If funcOTPaccess(Me.Name.ToString, "ACCOUNTS_MIMR", cnCostId, userId) = True Then Return False
                MsgBox("Invalid Date", MsgBoxStyle.Information)
                dtpTrandate.Focus()
                Return False
            End If
        Else
            Dim dayarray() As String = Split(RestrictDays, ",")
            Dim closeday As Integer = Val(dayarray(0).ToString)
            Dim mondiv As Integer = Val(dayarray(1).ToString)
            If closeday > serverDate.Day Then
                Dim mindate As Date = serverDate.AddMonths(-1 * mondiv)
                mindate = mindate.AddDays(-1 * (mindate.Day - 1))
                If Not (dtpTrandate.Value >= mindate) Then
                    If funcOTPaccess(Me.Name.ToString, "ACCOUNTS_MIMR", cnCostId, userId) = True Then Return False
                    MsgBox("Invalid Date", MsgBoxStyle.Information)
                    dtpTrandate.Focus()
                    Return False
                End If
            Else
                Dim mindate As Date = serverDate.AddMonths(-1 * 0)
                mindate = mindate.AddDays(-1 * (mindate.Day - 1))
                If Not (dtpTrandate.Value >= mindate) Then
                    If funcOTPaccess(Me.Name.ToString, "ACCOUNTS_MIMR", cnCostId, userId) = True Then Return False
                    MsgBox("Invalid Bill Date", MsgBoxStyle.Information)
                    dtpTrandate.Focus()
                    Return False
                End If
            End If
        End If
        Return True
    End Function

    Private Function ReceiptStockChecking(ByVal dtCheckAllLoopFunc As DataTable, ByVal txtNetwt As Decimal) As Boolean
        If gridviewTotalValue_Own.Rows.Count > 0 And dtCheckAllLoopFunc.Rows.Count > 0 Then
            Dim ClosingCheck As Boolean = True
            For abc As Integer = 0 To dtCheckAllLoopFunc.Rows.Count - 1
                If dtCheckAllLoopFunc.Rows(abc).Item("PAYMODE").ToString = "PURCHASE" _
                    Or dtCheckAllLoopFunc.Rows(abc).Item("PAYMODE").ToString = "PURCHASE RETURN" Then
                    ClosingCheck = False
                    Exit For
                End If
            Next
            If cmbTrantype.Text = "RECEIPT" Then
                LoadBalanceWt(cmbAcname_OWN.SelectedValue.ToString, "RECEIPT")
                Dim SmithGrswt As Double = Val(gridviewTotalValue_Own.Rows(0).Cells("CLOSING_GRSWT").Value.ToString)
                Dim MimrGrswt As Double = Val(dtCheckAllLoopFunc.Compute("SUM(GRSWT)", "TRANTYPE='RECEIPT'").ToString)
                If ClosingCheck = True And MimrGrswt <> 0 Then
                    If Math.Round((MimrGrswt + Val(txtNetwt)), 2) > SmithGrswt Then
                        MsgBox("Stock not available (R)", MsgBoxStyle.Information)
                        Return False
                    End If
                End If
            End If
        Else
            strsql = ""
            If cmbTrantype.Text = "RECEIPT" Then
                LoadBalanceWt(cmbAcname_OWN.SelectedValue.ToString, "RECEIPT")
                Dim AccodeFind As String = cmbAcname_OWN.SelectedValue.ToString
                For ii As Integer = 0 To gridviewTotalValue_Own.Rows.Count - 1
                    If AccodeFind <> "" Then
                        If gridviewTotalValue_Own.Rows(ii).Cells("ACCODE").Value.ToString = AccodeFind Then
                            Dim SmithGrswt As Double = Val(gridviewTotalValue_Own.Rows(ii).Cells("CLOSING_GRSWT").Value.ToString)
                            Dim MimrGrswt As Double = Val(txtNetWt_WET.Text)
                            If MimrGrswt <> 0 Then
                                If MimrGrswt > SmithGrswt Then
                                    MsgBox("Stock not available (R)", MsgBoxStyle.Information)
                                    Return False
                                End If
                            End If
                        End If
                    End If
                Next
            End If
        End If
        Return True
    End Function

    Private Function IssueStockChecking(ByVal dtCheckAllLoopFunc As DataTable, ByVal txtNetwt As Decimal) As Boolean
        If gridviewTotalValue_Own.Rows.Count > 0 And dtCheckAllLoopFunc.Rows.Count > 0 Then
            Dim ClosingCheck As Boolean = True
            For abc As Integer = 0 To dtCheckAllLoopFunc.Rows.Count - 1
                If dtCheckAllLoopFunc.Rows(abc).Item("PAYMODE").ToString = "PURCHASE" _
                    Or dtCheckAllLoopFunc.Rows(abc).Item("PAYMODE").ToString = "PURCHASE RETURN" Then
                    ClosingCheck = False
                    Exit For
                End If
            Next
            LoadBalanceWt(cmbAcname_OWN.SelectedValue.ToString, "ISSUE")
            For ii As Integer = 0 To gridviewTotalValue_Own.Rows.Count - 1
                Dim SmithGrswt As Double = Val(gridviewTotalValue_Own.Rows(ii).Cells("CLOSING_GRSWT").Value.ToString)
                Dim MimrGrswt As Double = Val(dtCheckAllLoopFunc.Compute("SUM(GRSWT)", "TRANTYPE='ISSUE' AND CATCODE = '" & gridviewTotalValue_Own.Rows(ii).Cells("CATCODE").Value.ToString & "'").ToString)
                If ClosingCheck = True And MimrGrswt <> 0 Then
                    If Math.Round((MimrGrswt + Val(txtNetwt)), 2) > SmithGrswt Then
                        MsgBox("Stock not available (I)", MsgBoxStyle.Information)
                        Return False
                    End If
                End If
            Next
        Else
            strsql = ""
            If cmbTrantype.Text = "ISSUE" Then
                LoadBalanceWt(cmbAcname_OWN.SelectedValue.ToString, "ISSUE")
                Dim catcodeFind As String = ""
                strsql = " SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = '" & cmbItem_OWN.SelectedValue.ToString & "'"
                catcodeFind = objGPack.GetSqlValue(strsql).ToString
                For ii As Integer = 0 To gridviewTotalValue_Own.Rows.Count - 1
                    If catcodeFind <> "" Then
                        If gridviewTotalValue_Own.Rows(ii).Cells("CATCODE").Value.ToString = catcodeFind Then
                            Dim SmithGrswt As Double = Val(gridviewTotalValue_Own.Rows(ii).Cells("CLOSING_GRSWT").Value.ToString)
                            Dim MimrGrswt As Double = Val(txtNetWt_WET.Text)
                            If MimrGrswt <> 0 Then
                                If MimrGrswt > SmithGrswt Then
                                    MsgBox("Stock not available (I)", MsgBoxStyle.Information)
                                    Return False
                                End If
                            End If
                        End If
                    End If
                Next
            End If
            strsql = ""
        End If
        Return True
    End Function

    Private Sub LOTISSUES(ByVal Tranno As Integer, ByVal Recsno As String _
    , ByVal accode As String, ByVal itemid As Integer, ByVal subitemid As Integer _
    , ByVal cnt As Integer, ByVal Pcs As Integer, ByVal Grswt As Decimal _
    , ByVal Netwt As Decimal, ByVal Touch As Decimal, ByVal Wastage As Decimal _
    , ByVal Mcharge As Decimal, ByVal Isediting As Boolean, ByVal FineRate As Decimal, ByVal DiaWt As Decimal, ByVal StkType As String _
    , ByVal HALLMARK As String, ByVal _LotNarration As String)
        Dim DTtemp As New DataTable
        Dim lotSno As String
        Dim lotNo As Integer = 0
        If itemid = 0 Then Exit Sub
        If lotNo = 0 Then
GENLOTNO:
            strsql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'LOTNO'"
            lotNo = Val(objGPack.GetSqlValue(strsql, , , Tran))
            strsql = " UPDATE " & cnAdminDb & "..SOFTCONTROL SET CTLTEXT = '" & lotNo + 1 & "' "
            strsql += " WHERE CTLID = 'LOTNO' AND CTLTEXT = " & lotNo & ""
            cmd = New OleDbCommand(strsql, cn, Tran)
            If cmd.ExecuteNonQuery() = 0 Then
                GoTo GENLOTNO
            End If
            lotNo += 1
        End If
        lotSno = GetNewSno(TranSnoType.ITEMLOTCODE, Tran, "GET_ADMINSNO_TRAN") '  GetWSno(TranSnoType.ITEMLOTCODE, Tran, CnStockdb)
        strsql = "select STOCKTYPE,isnull(NOOFPIECE,0) noofpiece,isnull(DEFAULTCOUNTER,0) DEFCOUNTER,VALUEADDEDTYPE from " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & itemid
        Dim dritem As DataRow = GetSqlRow(strsql, cn, Tran)
        Dim stocktype As String = dritem.Item(0).ToString
        Dim noOfTag As Integer = Val(dritem.Item(1).ToString)
        Dim itemCounterId As String = dritem.Item(2).ToString
        Dim VALUEADDEDTYPE As String = dritem.Item(3).ToString
        Dim DesignerId As Integer = 0
        strsql = "select top 1 designerid from " & cnAdminDb & "..DESIGNER WHERE ACCODE = '" & accode & "'"
        Dim dridesg As DataRow = GetSqlRow(strsql, cn, Tran)
        If Not (dridesg Is Nothing) Then DesignerId = Val(dridesg.Item(0).ToString)
        Dim ordrepno As String = ""
        Dim entryType As String = "R"
        Dim mwastper As Decimal = 0
        If Wastage <> 0 Then mwastper = Math.Round(((Wastage * 100) / Grswt), 2)
        Dim mmcgrm As Decimal = 0
        If Mcharge <> 0 Then mmcgrm = Math.Round((Mcharge / Grswt), 2)

        strsql = " INSERT INTO " & cnAdminDb & "..ITEMLOT "
        strsql += " ("
        strsql += " SNO,ENTRYTYPE,LOTDATE,LOTNO,DESIGNERID,TRANINVNO,"
        strsql += " BILLNO,COSTID,ENTRYORDER,ORDREPNO,ORDENTRYORDER,"
        strsql += " ITEMID,SUBITEMID,PCS,GRSWT,STNPCS,STNWT,STNUNIT,"
        strsql += " DIAPCS,DIAWT,NETWT,NOOFTAG,RATE,ITEMCTRID,WMCTYPE,"
        strsql += " BULKLOT,MULTIPLETAGS,NARRATION,FINERATE,TUCH,"
        strsql += " WASTPER,MCGRM,OTHCHARGE,STARTTAGNO,ENDTAGNO,CURTAGNO,"
        strsql += " COMPANYID,CPCS,CGRSWT,COMPLETED,CANCEL,"
        strsql += " ACCESSING,USERID,UPDATED,"
        strsql += " UPTIME,SYSTEMID,APPVER,ITEMTYPEID,OPENTIME,STKTYPE,HALLMARK)VALUES("
        strsql += " '" & lotSno & "'" 'SNO
        strsql += " ,'" & entryType & "'" 'ENTRYTYPE
        strsql += " ,'" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "'" 'LOTDATE 'DateTime.Parse(RoIssue(CNT).ITEM("LOTDATE"), ukCultureInfo.DateTimeFormat).ToString("yyyy-MM-dd")
        strsql += " ," & lotNo & "" 'LOTNO
        strsql += " ," & DesignerId & "" 'DESIGNERID
        strsql += " ,''" 'TRANINVNO
        strsql += " ,''" 'BILLNO
        strsql += " ,'" & CostCenterId & "'" 'COSTID
        strsql += " ," & cnt + 1 & "" 'ENTRYORDER
        strsql += " ,'" & ordrepno & "'" 'ORDREPNO
        strsql += " ,0" 'ORDENTRYORDER
        strsql += " ," & Val(itemid) & "" 'ITEMID
        strsql += " ," & Val(subitemid) & "" 'SUBITEMID
        strsql += " ," & Pcs & "" 'PCS
        strsql += " ," & Grswt & "" 'GRSWT
        strsql += " ,0" 'STNPCS
        strsql += " ,0" 'STNWT
        strsql += " ,'G'" 'STNUNIT
        strsql += " ,0" 'DIAPCS
        strsql += " ," & DiaWt & "" 'DIAWT
        strsql += " ," & Netwt & "" 'NETWT
        strsql += " ," & IIf(noOfTag = 0, 1, noOfTag) & "" 'NOOFTAG
        strsql += " ,0" 'RATE
        strsql += " ," & Val(itemCounterId) & "" 'ITEMCTRID
        strsql += " ,'" & VALUEADDEDTYPE & "'" 'WMCTYPE
        strsql += " ,'N'" 'BULKLOT
        strsql += " ,'N'" 'MULTIPLETAGS
        strsql += " ,'" & _LotNarration.ToString & "'" 'NARRATION 
        strsql += " ," & FineRate & "" 'FINERATE
        strsql += " ," & Touch & "" 'TUCH
        strsql += " ," & mwastper & "" 'WASTPER
        strsql += " ," & mmcgrm & "" 'MCGRM
        strsql += " ,0" 'OTHCHARGE
        strsql += " ,''" 'STARTTAGNO
        strsql += " ,''" 'ENDTAGNO
        strsql += " ,''" 'CURTAGNO
        strsql += " ,'" & GetStockCompId() & "'" 'sCOMPANYID
        strsql += " ,0" 'CPIECE
        strsql += " ,0" 'CWEIGHT
        strsql += " ,''" 'COMPLETED
        strsql += " ,''" 'CANCEL
        strsql += " ,''" 'ACCESSING
        strsql += " ," & userId & "" 'USERID
        strsql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        strsql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
        strsql += " ,'" & systemId & "'" 'SYSTEMID
        strsql += " ,'" & VERSION & "'" 'APPVER
        strsql += " ,0" 'ITEMTYPEID"
        strsql += " ,GETDATE()" 'OPENTIME
        strsql += " ,'" & StkType & "'" 'APPVER
        strsql += " ,'" & HALLMARK.ToString & "'" 'HALLMARK
        strsql += " )"
        ExecQuery(SyncMode.Stock, strsql, cn, Tran, CostCenterId)

        strsql = "  INSERT INTO " & cnStockDb & "..LOTISSUE"
        strsql += "  ("
        strsql += "  TRANNO"
        strsql += "  ,TRANDATE"
        strsql += "  ,GRSWT"
        strsql += "  ,NETWT"
        strsql += "  ,CANCEL"
        strsql += "  ,BATCHNO"
        strsql += "  ,USERID"
        strsql += "  ,UPDATED"
        strsql += "  ,APPVER"
        strsql += "  ,COMPANYID"
        strsql += "  ,PCS"
        strsql += "  ,LOTSNO"
        strsql += "  ,ITEMID"
        strsql += "  ,SUBITEMID"
        strsql += "  ,RECSNO"
        strsql += "  ,STKTYPE"
        strsql += "  )"
        strsql += "  SELECT"
        strsql += "  " & Tranno & ""
        strsql += "  ,'" & dtpTrandate.Value & "'"
        strsql += "  ," & Grswt & "" 'GRSWT
        strsql += "  ," & Netwt & "" 'NETWT
        strsql += "  ,''" 'CANCEL
        strsql += "  ,''" 'BATCHNO
        strsql += "  ," & userId & "" 'USERID
        strsql += "  ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        strsql += "  ,'" & VERSION & "'" 'APPVER
        strsql += "  ,'" & GetStockCompId() & "'" 'COMPANYID
        strsql += "  ," & Pcs & "" 'PCS
        strsql += "  ,'" & lotSno & "'" 'LOTSNO
        strsql += "  ," & itemid & "" 'ITEMID
        strsql += "  ," & subitemid & "" 'SUBITEMID
        strsql += "  ,'" & Recsno & "'" 'SNO
        strsql += "  ,'" & StkType & "'" 'SNO
        ExecQuery(SyncMode.Stock, strsql, cn, Tran, CostCenterId)
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click, SaveToolStripMenuItem.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
        If Gridview_OWN.Rows.Count > 0 Then
            If checkMIMR() = False Then
                Exit Sub
            End If

            If Val(txtSumNet.Text) < 0 Then
                MsgBox("Negative amount should not allowed", MsgBoxStyle.Information)
                txtSumNet.Focus()
                txtSumNet.SelectAll()
                Exit Sub
            End If

            strsql = ""

            Dim dtCheckAllLoopFunc As New DataTable
            dtCheckAllLoopFunc = Gridview_OWN.DataSource
            If dtCheckAllLoopFunc.Rows.Count > 0 Then
                For u As Integer = 0 To dtCheckAllLoopFunc.Rows.Count - 1
                    With dtCheckAllLoopFunc.Rows(u)
                        If .Item("PAYMODE").ToString = "RECEIPT" Or .Item("PAYMODE").ToString = "PURCHASE" Then
                            If Val(txtConTDS_AMT.Text) > 0 And Val(txtConTCS_AMT.Text) > 0 Then
                                'MsgBox("Tds & Tcs Receipt, Purchase not allowed", MsgBoxStyle.Information)
                                'Exit Sub
                            End If
                        End If
                        If .Item("PAYMODE").ToString = "RECEIPT" Then
                            If Val(txtSumNet.Text) <> 0 Then
                                If Val(txtConTDS_AMT.Text) = 0 Then
                                    'MsgBox("Receipt TDS Mandatory ", MsgBoxStyle.Information)
                                    'Exit Sub
                                End If
                            End If
                        End If
                        If .Item("PAYMODE").ToString = "PURCHASE" Then
                            Dim flagTcsPurchase As Boolean = True
                            Dim flagTdsPurchase As Boolean = True

                            Dim MTcsPer As Double = 0
                            strsql = " SELECT ISNULL(TCSPER,0)TCSPER FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE= '" & cmbAcname_OWN.SelectedValue.ToString & "'"
                            MTcsPer = Val(objGPack.GetSqlValue(strsql).ToString)
                            If MTcsPer > 0 Then
                                If Val(txtConTCS_AMT.Text) = 0 Then
                                    'MsgBox("Receipt TCS Mandatory ", MsgBoxStyle.Information)
                                    'Exit Sub
                                    'OLD Concept
                                    flagTcsPurchase = False
                                End If
                            End If

                            Dim MTdsPer As Double = 0
                            strsql = " SELECT ISNULL(TDSPER,0)TDSPER FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE= '" & cmbAcname_OWN.SelectedValue.ToString & "'"
                            MTdsPer = Val(objGPack.GetSqlValue(strsql).ToString)
                            If MTdsPer > 0 Then
                                If Val(txtConTDSPER_AMT.Text) = 0 Then
                                    'MsgBox("Receipt TCS Mandatory ", MsgBoxStyle.Information)
                                    'Exit Sub
                                    flagTdsPurchase = False
                                End If
                            End If
                            If flagTcsPurchase = False And flagTdsPurchase = False Then
                                'MsgBox("Purchase Entry Either one TDS or TCS Mandatory ", MsgBoxStyle.Information)
                                'Exit Sub
                            End If
                        End If
                    End With
                Next
            End If

            strsql = ""
            If MIMRSTOCKCHECKING = "Y" Then
                If ReceiptStockChecking(dtCheckAllLoopFunc, 0) = False Then
                    Exit Sub
                End If
                If IssueStockChecking(dtCheckAllLoopFunc, 0) = False Then
                    Exit Sub
                End If
            End If

            strsql = ""
            Dim dtDistinctReceiptIssue As New DataTable
            dtDistinctReceiptIssue = Gridview_OWN.DataSource
            dtDistinctReceiptIssue = dtDistinctReceiptIssue.DefaultView.ToTable(True, "PAYMODE", "TRANTYPE")
            If dtDistinctReceiptIssue.Rows.Count > 0 Then
                Dim DistinctReceipt As Integer = Val(dtDistinctReceiptIssue.Compute("COUNT(TRANTYPE)", "TRANTYPE= 'RECEIPT'").ToString)
                Dim DistinctIssue As Integer = Val(dtDistinctReceiptIssue.Compute("COUNT(TRANTYPE)", "TRANTYPE= 'ISSUE'").ToString)
                If DistinctReceipt > 1 Or DistinctIssue > 1 Then
                    MsgBox("Multiple Paymode & Trantype should not allowed", MsgBoxStyle.Information)
                    Exit Sub
                End If
            End If

            If txtInvoiceNo.Text.Trim = "" Then
                ' MsgBox("Invoice No should not empty", MsgBoxStyle.Information)
                ' txtInvoiceNo.Focus()
                ' txtInvoiceNo.SelectAll()
                ' Exit Sub
            End If
            If cmbTrantype.Text = "" Then
                MsgBox("Trantype should not empty", MsgBoxStyle.Information)
                cmbTrantype.Focus()
                cmbTrantype.SelectAll()
                Exit Sub
            End If
            If EditBatchno = Nothing Then
                'If Not BrighttechPack.Methods.GetRights(_DtUserRights, IIf(oMaterial = Material.Receipt, "REC", "ISS") & Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
            Else
                'If Not BrighttechPack.Methods.GetRights(_DtUserRights, IIf(oMaterial = Material.Receipt, "REC", "ISS") & Me.Name, BrighttechPack.Methods.RightMode.Edit) Then Exit Sub
            End If
            If CheckTrialDate(dtpTrandate.Value) = False Then Exit Sub
            'If cmbCostCentre.Enabled = True Then
            '    If cmbCostCentre.Text.Trim = "" Then
            '        MsgBox("Cost Center Empty", MsgBoxStyle.Information)
            '        cmbCostCentre.Select()
            '        Exit Sub
            '    Else
            '        If objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre.Text & "'") = "" Then MsgBox("Please select valid costcentre", MsgBoxStyle.Critical) : cmbCostCentre.Focus() : Exit Sub
            '    End If
            'End If
            _Accode = cmbAcname_OWN.SelectedValue.ToString
            If _Accode = "" Then
                MsgBox("Invalid AcName", MsgBoxStyle.Information)
                cmbAcname_OWN.Select()
                Exit Sub
            End If

            Dim Dtchk As New DataTable
            Dtchk = Gridview_OWN.DataSource
            Dim RowCount As Integer = Dtchk.Compute("COUNT(TRANTYPE)", "TRANTYPE<>''")
            If Not RowCount > 0 Then MsgBox("Must Enter one Entry", MsgBoxStyle.Information) : cmbTrantype.Focus() : Exit Sub

            _Acctype = objGPack.GetSqlValue("SELECT LOCALOUTST FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = '" & _Accode & "'")
            CostCenterId = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cnCostName & "'", , , Nothing)
            '

            Dim dtDistinctKeyNo As New DataTable
            dtDistinctKeyNo = Dtchk.DefaultView.ToTable(True, "KEYNO")
            If Dtchk.Rows.Count <> dtDistinctKeyNo.Rows.Count Then
                MsgBox("KeyNo Duplicate ", MsgBoxStyle.Information)
                Exit Sub
            End If

            strsql = " IF (SELECT COUNT(*) FROM " & cnAdminDb & "..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "BILLNO')> 0"
            strsql += " DROP TABLE " & cnAdminDb & "..TEMP" & systemId & "BILLNO"
            cmd = New OleDbCommand(strsql, cn, Tran)
            cmd.ExecuteNonQuery()

            Try
                Tran = Nothing
                Tran = cn.BeginTransaction()
                Me.Cursor = Cursors.WaitCursor
                btnSave.Enabled = False
                SaveToolStripMenuItem.ShortcutKeys = Keys.None
                If True Then
                    Dim dtDealerWmc As New DataTable
                    dtDealerWmc = Gridview_OWN.DataSource
                    If dtDealerWmc.Rows.Count > 0 Then
                        For d As Integer = 0 To dtDealerWmc.Rows.Count - 1
                            With dtDealerWmc.Rows(d)
                                If .Item("PAYMODE").ToString = "PURCHASE RETURN" Then Continue For
                                funcAddDEALER_WMCTABLE(Val(.Item("ITEMID").ToString) _
                                   , Val(.Item("SUBITEMID").ToString) _
                                   , .Item("ACCODE").ToString _
                                   , "W", Val(.Item("WASTAGEPER").ToString) _
                                   , Val(.Item("WASTAGE").ToString), 0 _
                                   , Val(.Item("MCGRAM").ToString) _
                                   , 0, Val(.Item("TOUCH").ToString), Val(.Item("MCPER").ToString), Val(.Item("PURITY").ToString) _
                                   , Tran _
                                   , .Item("CALTYPE").ToString _
                                   , Val(.Item("RATE").ToString) _
                                   , Val(.Item("MCPIE").ToString) _
                                   , Val(.Item("EMCPERG").ToString))
                            End With

                            If Not dtStone Is Nothing Then
                                If dtStone.Rows.Count > 0 Then
                                    '.Cells("KEYNO").Value.ToString
                                    Dim stRow() As DataRow = Nothing
                                    Dim drstnRow As DataRow = Nothing
                                    stRow = dtStone.Select("KEYNO = '" & dtDealerWmc.Rows(d).Item("KEYNO").ToString & "'", "")
                                    If Not stRow Is Nothing Then
                                        For stn As Integer = 0 To stRow.Length - 1
                                            Dim cntDealerstn As Double = 0

                                            Dim stnItemid As Integer = 0
                                            Dim stnSubitemId As Integer = 0

                                            ''Find itemId
                                            strsql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & stRow(stn)("ITEM").ToString & "'"
                                            stnItemid = Val(objGPack.GetSqlValue(strsql, , , Tran))

                                            ''Find subItemId
                                            strsql = " SELECT ISNULL(SUBITEMID,0)AS SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & stRow(stn)("SUBITEM").ToString & "' AND ITEMID = " & stnItemid & ""
                                            stnSubitemId = Val(objGPack.GetSqlValue(strsql, , , Tran))

                                            strsql = " SELECT COUNT(*)CNT FROM " & cnAdminDb & "..DEALER_STUDDED "
                                            strsql += vbCrLf + " WHERE ITEMID=" & Val(dtDealerWmc.Rows(d).Item("ITEMID").ToString) & " "
                                            strsql += vbCrLf + " And SUBITEMID=" & Val(dtDealerWmc.Rows(d).Item("SUBITEMID").ToString) & " "
                                            strsql += vbCrLf + " And STNITEMID=" & stnItemid & " "
                                            strsql += vbCrLf + " And STNSUBITEMID=" & stnSubitemId & " "
                                            strsql += vbCrLf + " And ACCODE='" & dtDealerWmc.Rows(d).Item("ACCODE").ToString & "' "
                                            strsql += vbCrLf + " And PSTNRATE=" & Val(stRow(stn)("RATE").ToString) & ""
                                            cntDealerstn = Val(objGPack.GetSqlValue(strsql,,, Tran))
                                            If cntDealerstn = 0 Then

                                                strsql = " "
                                                strsql += vbCrLf + " INSERT INTO " & cnAdminDb & "..DEALER_STUDDED(ROWID,ITEMID"
                                                strsql += vbCrLf + " ,SUBITEMID,STNITEMID,STNSUBITEMID,ACCODE,PSTNRATE"
                                                strsql += vbCrLf + " ,CALCMODE,STONEUNIT,USERID,UPDATED,UPTIME,COSTID)"
                                                strsql += vbCrLf + " SELECT ISNULL(MAX(ROWID),0)+1 ROWID "
                                                strsql += vbCrLf + " , " & Val(dtDealerWmc.Rows(d).Item("ITEMID").ToString) & " ITEMID"
                                                strsql += vbCrLf + " , " & Val(dtDealerWmc.Rows(d).Item("SUBITEMID").ToString) & " SUBITEMID"
                                                strsql += vbCrLf + " , " & stnItemid & " STNITEMID"
                                                strsql += vbCrLf + " , " & stnSubitemId & " STNSUBITEMID"
                                                strsql += vbCrLf + " , '" & dtDealerWmc.Rows(d).Item("ACCODE").ToString & "' ACCODE"
                                                strsql += vbCrLf + " , " & Val(stRow(stn)("RATE").ToString) & " PSTNRATE"
                                                strsql += vbCrLf + " , '" & (stRow(stn)("CALC").ToString) & "' CALCMODE"
                                                strsql += vbCrLf + " , '" & (stRow(stn)("UNIT").ToString) & "' STONEUNIT"
                                                strsql += vbCrLf + " , " & userId & " USERID"
                                                strsql += vbCrLf + " , REPLACE(CONVERT(VARCHAR(15),GETDATE(),102),'.','-')  UPDATED"
                                                strsql += vbCrLf + " , CONVERT(VARCHAR(15),GETDATE(),108) UPTIME"
                                                strsql += vbCrLf + " , '" & cnCostId & "' COSTID"
                                                strsql += vbCrLf + " FROM " & cnAdminDb & "..DEALER_STUDDED"
                                                cmd = New OleDbCommand(strsql, cn, Tran)
                                                cmd.ExecuteNonQuery()
                                            End If
                                        Next
                                    End If
                                End If
                            End If
                        Next
                    End If
                End If

                strsql = ""

                strsql = " If (Select COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "BILLNO')> 0"
                strsql += " DROP TABLE TEMP" & systemId & "BILLNO"
                cmd = New OleDbCommand(strsql, cn, Tran)
                cmd.ExecuteNonQuery()
                Dim AccPost As Boolean = True

                If EditBatchno = Nothing Or EditBatchno = "" Then
                    BatchNo = GetNewBatchno(cnCostId, dtpTrandate.Value.ToString("yyyy-MM-dd"), Tran)
                End If
                For cnt As Integer = 0 To Gridview_OWN.RowCount - 1
                    'If cmbTrantype.Text = "APPROVAL RECEIPT" Or cmbTrantype.Text = "APPROVAL ISSUE" Then
                    '    AccPost = MRMIAPPACCPOST
                    'End If
                    If Gridview_OWN.Rows(cnt).Cells("PAYMODE").Value.ToString = "APPROVAL RECEIPT" _
                    Or Gridview_OWN.Rows(cnt).Cells("PAYMODE").Value.ToString = "APPROVAL ISSUE" Then
                        AccPost = MRMIAPPACCPOST
                    End If
                    If Gridview_OWN.Rows(cnt).Cells("TRANTYPE").Value.ToString = "" Then
                        Exit For
                    End If
                    With Gridview_OWN.Rows(cnt)
                        TranNo = funcPaymode(Gridview_OWN.Rows(cnt).Cells("PAYMODE").Value.ToString, Gridview_OWN.Rows(cnt).Cells("TRANTYPE").Value.ToString)
                        Dim Tds As Decimal = Val(.Cells("TDSVAL").Value.ToString) 'VAT
                        Dim Tax As Decimal = Val(.Cells("SGST").Value.ToString) + Val(.Cells("CGST").Value.ToString) + Val(.Cells("IGST").Value.ToString)
                        CostCenterId = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cnCostName & "'", , , Tran)
                        Dim OrdStateId As Integer = 0
                        OrdStateId = Val(objGPack.GetSqlValue("SELECT ORDSTATE_ID FROM " & cnAdminDb & "..ORDERSTATUS WHERE ORDSTATE_NAME = '" & cmbProcess_OWN.Text & "'", , , Tran)) '" & Obj.cmbOProcess.Text & "
                        Dim issSno As String = Nothing
                        Dim issAppNo As String = ""
                        If Gridview_OWN.Rows(cnt).Cells("TRANTYPE").Value.ToString = "ISSUE" Then 'oMaterial = Material.Issue
                            issSno = GetNewSno(IIf(_AccAudit, TranSnoType.TISSUECODE, TranSnoType.ISSUECODE), Tran)
                        Else
                            issSno = GetNewSno(IIf(_AccAudit, TranSnoType.TRECEIPTCODE, TranSnoType.RECEIPTCODE), Tran)
                        End If
                        Dim catCode As String = objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & .Cells("CATNAME").Value.ToString & "'", , , Tran)
                        Dim OCatcode As String
                        If Gridview_OWN.Rows(cnt).Cells("PAYMODE").Value.ToString = "ISSUE" Then 'oMaterial = Material.Issue
                            OCatcode = catCode
                        Else
                            OCatcode = objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & .Cells("CATNAME").Value.ToString & "'", , , Tran) 'ISSCATNAME 
                        End If

                        Dim wast As Decimal = Nothing
                        Dim wastPer As Decimal = Nothing
                        Dim alloy As Decimal = Nothing
                        alloy = Val(.Cells("ALLOY").Value.ToString)
                        wast = Val(.Cells("WASTAGE").Value.ToString)
                        wastPer = Val(.Cells("WASTAGEPER").Value.ToString)

                        Dim itemTypeId As Integer = 0
                        Dim Itemid As Integer = Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Cells("ITEMNAME").Value.ToString & "'", , , Tran))
                        Dim subItemid As Integer = Val(objGPack.GetSqlValue("SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & .Cells("SUBITEMNAME").Value.ToString & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Cells("ITEMNAME").Value.ToString & "')", , , Tran))
                        If OCatcode Is Nothing Then OCatcode = catCode
                        If OCatcode.ToString = "" Then OCatcode = catCode
                        Dim Jobisno As String = ""
                        If Gridview_OWN.Rows(cnt).Cells("TRANTYPE").Value.ToString = "RECEIPT" Then 'oMaterial = Material.Receipt
                            strsql = " INSERT INTO " & cnStockDb & ".." & IIf(_AccAudit, "TRECEIPT", "RECEIPT")
                            strsql += " ("
                            strsql += "  SNO,TRANNO,TRANDATE,TRANTYPE,STKTYPE,PCS,GRSWT,NETWT,LESSWT,PUREWT"
                            strsql += " ,TAGNO,ITEMID,SUBITEMID,WASTPER,WASTAGE,MCGRM,MCHARGE,AMOUNT"
                            strsql += " ,RATE,BOARDRATE,SALEMODE,GRSNET,TRANSTATUS,REFNO,REFDATE,COSTID"
                            strsql += " ,COMPANYID,FLAG,EMPID,TAGGRSWT,TAGNETWT,TAGRATEID,TAGSVALUE,TAGDESIGNER"
                            strsql += " ,ITEMCTRID,ITEMTYPEID,PURITY,TABLECODE,INCENTIVE,WEIGHTUNIT,CATCODE,OCATCODE"
                            strsql += " ,ACCODE,ALLOY,BATCHNO,REMARK1"
                            strsql += " ,REMARK2,USERID,UPDATED,UPTIME,SYSTEMID,DISCOUNT,RUNNO,CASHID,TAX,TDS"
                            strsql += " ,STNAMT,MISCAMT,METALID,STONEUNIT,APPVER,TOUCH,ORDSTATE_ID"
                            If _JobNoEnable = True Then
                                strsql += " ,JOBNO"
                            End If
                            strsql += " ,SEIVE,BAGNO,MCPIE,BILLPREFIX)"
                            strsql += " VALUES("
                            strsql += " '" & issSno & "'" ''SNO
                            strsql += " ," & TranNo & "" 'TRANNO
                            strsql += " ,'" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                            'strsql += " ,'" & IIf(oMaterial = Material.Issue, "I", "R") & "" & Mid(.Cells("TRANTYPE").Value.ToString, 1, 2) & "'" 'TRANTYPE
                            strsql += " ,'" & "R" & Mid(.Cells("PAYMODE").Value.ToString, 1, 2) & "'" 'TRANTYPE
                            strsql += " ,'" & _StkType & "'" 'STKTYPE
                            strsql += " ," & Val(.Cells("PCS").Value.ToString) & "" 'PCS
                            strsql += " ," & Val(.Cells("GRSWT").Value.ToString) - (0) & "" 'GRSWT
                            strsql += " ," & Val(.Cells("NETWT").Value.ToString) - (0) & "" 'NETWT
                            strsql += " ," & Val(.Cells("LESSWT").Value.ToString) & "" 'LESSWT
                            strsql += " ," & Val(.Cells("PUREWT").Value.ToString) - (0) & "" 'PUREWT
                            strsql += " ,'" & .Cells("ESTTAGNO").Value.ToString & "'" 'TAGNO
                            strsql += " ," & Itemid & "" 'ITEMID
                            strsql += " ," & subItemid & "" 'SUBITEMID
                            strsql += " ," & wastPer & "" 'WASTPER
                            strsql += " ," & wast & "" 'WASTAGE
                            If Val(.Cells("MCPER").Value.ToString) > 0 Then
                                strsql += " ," & Format(Val(.Cells("MCPER").Value.ToString), "0.0000") & "" 'MCGRM
                            Else
                                strsql += " ," & Val(.Cells("MCGRAM").Value.ToString) & "" 'MCGRM
                            End If
                            strsql += " ," & Val(.Cells("MC").Value.ToString) & "" 'MCHARGE
                            strsql += " ," & Val(.Cells("GROSSAMT").Value.ToString) & "" 'AMOUNT
                            strsql += " ," & Format(Val(.Cells("RATE").Value.ToString), "0.0000") & "" 'RATE
                            strsql += " ," & Val(.Cells("BOARDRATE").Value.ToString) & "" 'BOARDRATE
                            strsql += " ,'1' " 'SALEMODE
                            strsql += " ,'" & Mid(.Cells("GRSNET").Value.ToString, 1, 1) & "'" 'GRSNET
                            strsql += " ,'" & .Cells("TRANSTATUS").Value.ToString & "'" 'TRANSTATUS ''
                            strsql += " ,'" & txtInvoiceNo.Text & "'" 'REFNO ''
                            strsql += " ,'" & dtpInvoiceDate_OWN.Value.ToString("yyyy-MM-dd") & "'" 'REFDATE NULL
                            strsql += " ,'" & CostCenterId & "'" 'COSTID 
                            strsql += " ,'" & strCompanyId & "'" 'COMPANYID
                            strsql += " ,'O'" 'FLAG'" & .Cells("TYPE").Value.ToString & "
                            strsql += " ,0" 'EMPID
                            strsql += " ,0" 'TAGGRSWT
                            strsql += " ,0" 'TAGNETWT
                            strsql += " ,0" 'TAGRATEID
                            strsql += " ,0" 'TAGSVALUE
                            strsql += " ,''" 'TAGDESIGNER  
                            strsql += " ,0" 'ITEMCTRID
                            strsql += " ," & itemTypeId & "" 'ITEMTYPEID
                            strsql += " ," & Format(Val(.Cells("PURITY").Value.ToString), "0.0000") & "" 'PURITY
                            strsql += " ,''" 'TABLECODE
                            strsql += " ,''" 'INCENTIVE
                            strsql += " ,NULL" 'WEIGHTUNIT'" & Mid(.Cells("CALCMODE").Value.ToString, 1, 1) & "
                            strsql += " ,'" & catCode & "'" 'CATCODE
                            strsql += " ,'" & OCatcode & "'" 'OCATCODE
                            strsql += " ,'" & _Accode & "'" 'ACCODE
                            strsql += " ," & alloy & "" 'ALLOY
                            strsql += " ,'" & BatchNo & "'" 'BATCHNO
                            strsql += " ,'" & .Cells("REMARK1").Value.ToString & "'" 'REMARK1
                            strsql += " ,'" & .Cells("REMARK2").Value.ToString & "'" 'REMARK2
                            strsql += " ,'" & userId & "'" 'USERID
                            strsql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                            strsql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                            strsql += " ,'" & systemId & "'" 'SYSTEMID
                            strsql += " ,0" 'DISCOUNT
                            strsql += " ,''" 'RUNNO
                            strsql += " ,'" & _CashCtr & "'" 'CASHID
                            strsql += " ," & Tax & "" 'TAX
                            strsql += " ," & Tds & "" 'TDS
                            strsql += " ,0" 'STNAMT
                            strsql += " ,0" 'MISCAMT
                            strsql += " ,'" & objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & .Cells("METALNAME").Value.ToString & "'", , , Tran) & "'" 'METALID
                            strsql += " ,NULL" 'STONEUNIT''" & Mid(.Cells("UNIT").Value.ToString, 1, 1) & "'
                            strsql += " ,'" & VERSION & "'" 'APPVER
                            strsql += " ,'" & Val(.Cells("TOUCH").Value.ToString) & "'" 'APPVER
                            strsql += " ," & OrdStateId & "" 'ORDSTATE_ID
                            If _JobNoEnable = True Then
                                strsql += " ,NULL" ''" & .Cells("JOBNO").Value.ToString & "'
                            End If
                            strsql += " ,NULL" 'SEIVE'" & .Cells("SEIVE").Value.ToString & "'
                            strsql += ",'" & IIf(Transistno <> 0, Transistno.ToString, "") & "'"
                            strsql += ", " & Val(.Cells("MCPIE").Value.ToString) & "" 'MCPIE
                            strsql += ", '" & cnCostId & Mid(cnStockDb, 5, 4) & Mid(.Cells("PAYMODE").Value.ToString, 1, 2) & "'" 'BILLPREFIX
                            strsql += " )"
                            ExecQuery(SyncMode.Transaction, strsql, cn, Tran, CostCenterId)
                            If Lotautopost = True Then
                                If "R" & Mid(.Cells("PAYMODE").Value.ToString, 1, 2) = "RRE" Then
                                    LOTISSUES(TranNo, issSno, _Accode, Itemid, subItemid _
                                              , cnt, Val(.Cells("PCS").Value.ToString) _
                                              , Val(.Cells("GRSWT").Value.ToString), Val(.Cells("NETWT").Value.ToString) _
                                              , Val(.Cells("TOUCH").Value.ToString), wast, Val(.Cells("MC").Value.ToString), False, 0, 0, "M", "N", "")
                                End If
                            End If
                        ElseIf Gridview_OWN.Rows(cnt).Cells("TRANTYPE").Value.ToString = "ISSUE" Then 'oMaterial = Material.Issue
                            strsql = " INSERT INTO " & cnStockDb & ".." & IIf(_AccAudit, "TISSUE", "ISSUE")
                            strsql += " ("
                            strsql += "  SNO,TRANNO,TRANDATE,TRANTYPE,PCS"
                            strsql += " ,GRSWT,NETWT,LESSWT,PUREWT"
                            strsql += " ,TAGNO,ITEMID,SUBITEMID,WASTPER"
                            strsql += " ,WASTAGE,MCGRM,MCHARGE,AMOUNT"
                            strsql += " ,RATE,BOARDRATE,SALEMODE,GRSNET"
                            strsql += " ,TRANSTATUS,REFNO,REFDATE,COSTID"
                            strsql += " ,COMPANYID,FLAG,EMPID,TAGGRSWT"
                            strsql += " ,TAGNETWT,TAGRATEID,TAGSVALUE,TAGDESIGNER"
                            strsql += " ,ITEMCTRID,ITEMTYPEID,PURITY,TABLECODE"
                            strsql += " ,INCENTIVE,WEIGHTUNIT,CATCODE,OCATCODE"
                            strsql += " ,ACCODE,ALLOY,BATCHNO,REMARK1"
                            strsql += " ,REMARK2,USERID,UPDATED,UPTIME,SYSTEMID,DISCOUNT"
                            strsql += " ,RUNNO,CASHID,TAX,TDS"
                            strsql += " ,STNAMT,MISCAMT,METALID,STONEUNIT,APPVER,TOUCH,ORDSTATE_ID"
                            strsql += " ,RESNO"
                            strsql += " ,SEIVE,BAGNO,RFID,CUTID,COLORID,CLARITYID"
                            strsql += " ,SHAPEID,SETTYPEID,HEIGHT,WIDTH,MCPIE,BILLPREFIX"
                            strsql += " ,TAGPCS"
                            strsql += " )"
                            strsql += " VALUES("
                            strsql += " '" & issSno & "'" ''SNO
                            strsql += " ," & TranNo & "" 'TRANNO
                            strsql += " ,'" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                            'strsql += " ,'" & IIf(oMaterial = Material.Issue, "I", "R") & "" & Mid(.Cells("TRANTYPE").Value.ToString, 1, 2) & "'" 'TRANTYPE
                            strsql += " ,'I" & Mid(.Cells("PAYMODE").Value.ToString, 1, 2) & "'" 'TRANTYPE
                            strsql += " ," & Val(.Cells("PCS").Value.ToString) & "" 'PCS
                            strsql += " ," & Val(.Cells("GRSWT").Value.ToString) & "" 'GRSWT
                            strsql += " ," & Val(.Cells("NETWT").Value.ToString) & "" 'NETWT
                            strsql += " ," & Val(.Cells("LESSWT").Value.ToString) & "" 'LESSWT
                            strsql += " ," & Val(.Cells("PUREWT").Value.ToString) & "" 'PUREWT
                            strsql += " ,'" & .Cells("ESTTAGNO").Value.ToString & "'" 'TAGNO
                            strsql += " ," & Itemid & "" 'ITEMID
                            strsql += " ," & subItemid & "" 'SUBITEMID
                            strsql += " ," & wastPer & "" 'WASTPER
                            strsql += " ," & wast & "" 'WASTAGE
                            strsql += " ," & Format(Val(.Cells("MCPER").Value.ToString), "0.0000") & "" 'MCGRM
                            strsql += " ," & Val(.Cells("MC").Value.ToString) & "" 'MCHARGE
                            strsql += " ," & Val(.Cells("GROSSAMT").Value.ToString) & "" 'AMOUNT
                            strsql += " ," & Format(Val(.Cells("RATE").Value.ToString), "0.0000") & "" 'RATE
                            strsql += " ," & Val(.Cells("BOARDRATE").Value.ToString) & "" 'BOARDRATE
                            strsql += " ,'1'" 'SALEMODE
                            strsql += " ,'" & Mid(.Cells("GRSNET").Value.ToString, 1, 1) & "'" 'GRSNET
                            strsql += " ,'" & .Cells("TRANSTATUS").Value.ToString & "'" 'TRANSTATUS ''
                            strsql += " ,'" & txtInvoiceNo.Text & "'" 'REFNO ''
                            strsql += " ,'" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "'" 'REFDATE NULL
                            strsql += " ,'" & CostCenterId & "'" 'COSTID 
                            strsql += " ,'" & strCompanyId & "'" 'COMPANYID
                            strsql += " ,'O'" 'FLAG'" & .Cells("TYPE").Value.ToString & "
                            strsql += " ,0" 'EMPID
                            If .Cells("ESTTAGNO").Value.ToString.Trim <> "" Then
                                strsql += ", " & Val(.Cells("ITEMTAGGRSWT").Value.ToString) & "" 'TAGGRSWT
                                strsql += ", " & Val(.Cells("ITEMTAGNETWT").Value.ToString) & "" 'TAGNETWT
                            Else
                                strsql += " ,0" 'TAGGRSWT
                                strsql += " ,0" 'TAGNETWT
                            End If
                            strsql += " ,0" 'TAGRATEID
                            strsql += " ,0" 'TAGSVALUE
                            strsql += " ,''" 'TAGDESIGNER  
                            strsql += " ,0" 'ITEMCTRID
                            strsql += " ," & itemTypeId & "" 'ITEMTYPEID
                            strsql += " ," & Format(Val(.Cells("PURITY").Value.ToString), "0.0000") & "" 'PURITY
                            strsql += " ,''" 'TABLECODE
                            strsql += " ,''" 'INCENTIVE
                            strsql += " ,NULL" 'WEIGHTUNIT '" & Mid(.Cells("CALCMODE").Value.ToString, 1, 1) & "'
                            strsql += " ,'" & catCode & "'" 'CATCODE
                            strsql += " ,'" & OCatcode & "'" 'OCATCODE
                            strsql += " ,'" & _Accode & "'" 'ACCODE
                            strsql += " ," & alloy & "" 'ALLOY
                            strsql += " ,'" & BatchNo & "'" 'BATCHNO
                            strsql += " ,'" & .Cells("REMARK1").Value.ToString & "'" 'REMARK1'.Cells("REMARK1").Value.ToString
                            strsql += " ,'" & .Cells("REMARK1").Value.ToString & "'" 'REMARK2'.Cells("REMARK2").Value.ToString
                            strsql += " ,'" & userId & "'" 'USERID
                            strsql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                            strsql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                            strsql += " ,'" & systemId & "'" 'SYSTEMID
                            strsql += " ,0" 'DISCOUNT
                            strsql += " ,''" 'RUNNO
                            strsql += " ,'" & _CashCtr & "'" 'CASHID
                            strsql += " ," & Tax & "" 'TAX
                            strsql += " ," & Tds & "" 'TDS
                            strsql += " ,0" 'STNAMT
                            strsql += " ,0" 'MISCAMT
                            strsql += " ,'" & objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & .Cells("METALNAME").Value.ToString & "'", , , Tran) & "'" 'METALID
                            strsql += " ,NULL" 'STONEUNIT'" & Mid(.Cells("UNIT").Value.ToString, 1, 1) & "'
                            strsql += " ,'" & VERSION & "'" 'APPVER
                            strsql += " ,'" & Val(.Cells("TOUCH").Value.ToString) & "'" 'APPVER
                            strsql += " ," & OrdStateId & "" 'ORDSTATE_ID
                            strsql += " ,NULL" 'RESNO''" & .Cells("RESNO").Value.ToString & "'
                            strsql += " ,NULL" 'SEIVE''" & .Cells("SEIVE").Value.ToString & "'
                            strsql += ",NULL" 'BAGNO''" & .Cells("JOBNO").Value.ToString & "'
                            strsql += " ,NULL" 'RFID''" & .Cells("RFID").Value.ToString & "'
                            strsql += " ,NULL " 'CUTID
                            strsql += " ,NULL" 'COLORID
                            strsql += " ,NULL" 'CLARITYID
                            strsql += " ,NULL" 'SHAPEID
                            strsql += " ,NULL" 'SETTYPEID
                            strsql += " ,0" 'HEIGHT
                            strsql += " ,0" 'WIDTH
                            strsql += ", " & Val(.Cells("MCPIE").Value.ToString) & "" 'MCPIE
                            strsql += ", '" & cnCostId & Mid(cnStockDb, 5, 4) & Mid(.Cells("PAYMODE").Value.ToString, 1, 2) & "'" ' BILLPREFIX
                            If .Cells("ESTTAGNO").Value.ToString.Trim <> "" Then
                                strsql += ", " & Val(.Cells("ITEMTAGPCS").Value.ToString) & "" 'TAGPCS
                            Else
                                strsql += ",0" 'TAGPCS
                            End If
                            strsql += " )"
                            ExecQuery(SyncMode.Transaction, strsql, cn, Tran, CostCenterId)
                        End If
                        If Not dtStone Is Nothing Then
                            If dtStone.Rows.Count > 0 Then
                                Dim stRow() As DataRow = Nothing
                                stRow = dtStone.Select("KEYNO = '" & .Cells("KEYNO").Value.ToString & "'", "")
                                If Not stRow Is Nothing Then
                                    For stn As Integer = 0 To stRow.Length - 1
                                        '*Hints Trantype-> Receipt/Issue i.e Material Receipt / Material Issue
                                        '*Hints Paymode-> Receipt,Purchase Return, are etc.,
                                        Dim drstnRow As DataRow = stRow(stn)
                                        InsertStoneDetails(issSno, TranNo, drstnRow, Tax, .Cells("TRANTYPE").Value.ToString, .Cells("PAYMODE").Value.ToString)
                                    Next
                                End If
                            End If
                        End If
                        If Val(.Cells("SGST").Value.ToString) <> 0 Then
                            strsql = " INSERT INTO " & cnStockDb & "..TAXTRAN"
                            strsql += " ("
                            strsql += " SNO,ISSSNO,ACCODE,TRANNO,TRANDATE,TRANTYPE,BATCHNO,TAXID"
                            strsql += " ,AMOUNT,TAXPER,TAXAMOUNT,TAXTYPE,TSNO,COSTID,COMPANYID"
                            strsql += " )"
                            strsql += " VALUES("
                            strsql += " '" & GetNewSno(TranSnoType.TAXTRANCODE, Tran) & "'" ''SNO
                            strsql += " ,'" & issSno & "'" ''ISSSNO
                            strsql += " ,''"
                            strsql += " ," & TranNo & "" 'TRANNO
                            strsql += " ,'" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                            'strsql += " ,'" & IIf(oMaterial = Material.Issue, "I", "R") & "" & Mid(.Cells("TRANTYPE").Value.ToString, 1, 2) & "'" 'TRANTYPE
                            If .Cells("TRANTYPE").Value.ToString = "ISSUE" Then
                                strsql += " ,'I" & Mid(.Cells("PAYMODE").Value.ToString, 1, 2) & "'" 'TRANTYPE
                            Else
                                strsql += " ,'R" & Mid(.Cells("PAYMODE").Value.ToString, 1, 2) & "'" 'TRANTYPE
                            End If
                            strsql += " ,'" & BatchNo & "'" 'BATCHNO
                            strsql += " ,'SG'" 'TAXID
                            strsql += " ," & Val(.Cells("GROSSAMT").Value.ToString) & "" 'AMOUNT
                            strsql += " ," & Val(.Cells("SGSTPER").Value.ToString) & "" 'TAXPER
                            strsql += " ," & Val(.Cells("SGST").Value.ToString)
                            strsql += " ,'TX'" 'TAXTYPE
                            strsql += " ,1" 'TSNO
                            strsql += " ,'" & CostCenterId & "'" 'COSTID
                            strsql += " ,'" & strCompanyId & "'" 'COMPANYID
                            strsql += " )"
                            ExecQuery(SyncMode.Transaction, strsql, cn, Tran, CostCenterId)
                        End If
                        If Val(.Cells("CGST").Value.ToString) <> 0 Then
                            strsql = " INSERT INTO " & cnStockDb & "..TAXTRAN"
                            strsql += " ("
                            strsql += " SNO,ISSSNO,ACCODE,TRANNO,TRANDATE,TRANTYPE,BATCHNO,TAXID"
                            strsql += " ,AMOUNT,TAXPER,TAXAMOUNT,TAXTYPE,TSNO,COSTID,COMPANYID"
                            strsql += " )"
                            strsql += " VALUES("
                            strsql += " '" & GetNewSno(TranSnoType.TAXTRANCODE, Tran) & "'" ''SNO
                            strsql += " ,'" & issSno & "'" ''ISSSNO
                            strsql += " ,''"
                            strsql += " ," & TranNo & "" 'TRANNO
                            strsql += " ,'" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                            'strsql += " ,'" & IIf(oMaterial = Material.Issue, "I", "R") & "" & Mid(.Cells("TRANTYPE").Value.ToString, 1, 2) & "'" 'TRANTYPE
                            If .Cells("TRANTYPE").Value.ToString = "ISSUE" Then
                                strsql += " ,'I" & Mid(.Cells("PAYMODE").Value.ToString, 1, 2) & "'" 'TRANTYPE
                            Else
                                strsql += " ,'R" & Mid(.Cells("PAYMODE").Value.ToString, 1, 2) & "'" 'TRANTYPE
                            End If
                            strsql += " ,'" & BatchNo & "'" 'BATCHNO
                            strsql += " ,'CG'" 'TAXID
                            strsql += " ," & Val(.Cells("GROSSAMT").Value.ToString) & "" 'AMOUNT
                            strsql += " ," & Val(.Cells("CGSTPER").Value.ToString) & "" 'TAXPER
                            strsql += " ," & Val(.Cells("CGST").Value.ToString)
                            strsql += " ,'TX'" 'TAXTYPE
                            strsql += " ,2" 'TSNO
                            strsql += " ,'" & CostCenterId & "'" 'COSTID
                            strsql += " ,'" & strCompanyId & "'" 'COMPANYID
                            strsql += " )"
                            ExecQuery(SyncMode.Transaction, strsql, cn, Tran, CostCenterId)
                        End If
                        If Val(.Cells("IGST").Value.ToString) <> 0 Then
                            strsql = " INSERT INTO " & cnStockDb & "..TAXTRAN"
                            strsql += " ("
                            strsql += " SNO,ISSSNO,ACCODE,TRANNO,TRANDATE,TRANTYPE,BATCHNO,TAXID"
                            strsql += " ,AMOUNT,TAXPER,TAXAMOUNT,TAXTYPE,TSNO,COSTID,COMPANYID"
                            strsql += " )"
                            strsql += " VALUES("
                            strsql += " '" & GetNewSno(TranSnoType.TAXTRANCODE, Tran) & "'" ''SNO
                            strsql += " ,'" & issSno & "'" ''ISSSNO
                            strsql += " ,''"
                            strsql += " ," & TranNo & "" 'TRANNO
                            strsql += " ,'" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                            'strsql += " ,'" & IIf(oMaterial = Material.Issue, "I", "R") & "" & Mid(.Cells("TRANTYPE").Value.ToString, 1, 2) & "'" 'TRANTYPE
                            If .Cells("TRANTYPE").Value.ToString = "ISSUE" Then
                                strsql += " ,'I" & Mid(.Cells("PAYMODE").Value.ToString, 1, 2) & "'" 'TRANTYPE
                            Else
                                strsql += " ,'R" & Mid(.Cells("PAYMODE").Value.ToString, 1, 2) & "'" 'TRANTYPE
                            End If
                            strsql += " ,'" & BatchNo & "'" 'BATCHNO
                            strsql += " ,'IG'" 'TAXID
                            strsql += " ," & Val(.Cells("GROSSAMT").Value.ToString) & "" 'AMOUNT
                            strsql += " ," & Val(.Cells("IGSTPER").Value.ToString) & "" 'TAXPER
                            strsql += " ," & Val(.Cells("IGST").Value.ToString)
                            strsql += " ,'TX'" 'TAXTYPE
                            strsql += " ,3" 'TSNO
                            strsql += " ,'" & CostCenterId & "'" 'COSTID
                            strsql += " ,'" & strCompanyId & "'" 'COMPANYID
                            strsql += " )"
                            ExecQuery(SyncMode.Transaction, strsql, cn, Tran, CostCenterId)
                        End If
                        If Val(.Cells("WSGST").Value.ToString) <> 0 Then
                            strsql = " INSERT INTO " & cnStockDb & "..TAXTRAN"
                            strsql += " ("
                            strsql += " SNO,ISSSNO,ACCODE,TRANNO,TRANDATE,TRANTYPE,BATCHNO,TAXID"
                            strsql += " ,AMOUNT,TAXPER,TAXAMOUNT,TAXTYPE,TSNO,COSTID,COMPANYID"
                            strsql += " )"
                            strsql += " VALUES("
                            strsql += " '" & GetNewSno(TranSnoType.TAXTRANCODE, Tran) & "'" ''SNO
                            strsql += " ,'" & issSno & "'" ''ISSSNO
                            strsql += " ,''"
                            strsql += " ," & TranNo & "" 'TRANNO
                            strsql += " ,'" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                            If .Cells("TRANTYPE").Value.ToString = "ISSUE" Then
                                strsql += " ,'I" & Mid(.Cells("PAYMODE").Value.ToString, 1, 2) & "'" 'TRANTYPE
                            Else
                                strsql += " ,'R" & Mid(.Cells("PAYMODE").Value.ToString, 1, 2) & "'" 'TRANTYPE
                            End If
                            strsql += " ,'" & BatchNo & "'" 'BATCHNO
                            strsql += " ,'SG'" 'TAXID
                            strsql += " ," & Val(.Cells("GROSSAMT").Value.ToString) & "" 'AMOUNT
                            strsql += " ," & Val(.Cells("WSGSTPER").Value.ToString) & "" 'TAXPER
                            strsql += " ," & Val(.Cells("WSGST").Value.ToString)
                            strsql += " ,'RG'" 'TAXTYPE
                            strsql += " ,7" 'TSNO
                            strsql += " ,'" & CostCenterId & "'" 'COSTID
                            strsql += " ,'" & strCompanyId & "'" 'COMPANYID
                            strsql += " )"
                            ExecQuery(SyncMode.Transaction, strsql, cn, Tran, CostCenterId)
                        End If
                        If Val(.Cells("WCGST").Value.ToString) <> 0 Then
                            strsql = " INSERT INTO " & cnStockDb & "..TAXTRAN"
                            strsql += " ("
                            strsql += " SNO,ISSSNO,ACCODE,TRANNO,TRANDATE,TRANTYPE,BATCHNO,TAXID"
                            strsql += " ,AMOUNT,TAXPER,TAXAMOUNT,TAXTYPE,TSNO,COSTID,COMPANYID"
                            strsql += " )"
                            strsql += " VALUES("
                            strsql += " '" & GetNewSno(TranSnoType.TAXTRANCODE, Tran) & "'" ''SNO
                            strsql += " ,'" & issSno & "'" ''ISSSNO
                            strsql += " ,''"
                            strsql += " ," & TranNo & "" 'TRANNO
                            strsql += " ,'" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                            If .Cells("TRANTYPE").Value.ToString = "ISSUE" Then
                                strsql += " ,'I" & Mid(.Cells("PAYMODE").Value.ToString, 1, 2) & "'" 'TRANTYPE
                            Else
                                strsql += " ,'R" & Mid(.Cells("PAYMODE").Value.ToString, 1, 2) & "'" 'TRANTYPE
                            End If
                            strsql += " ,'" & BatchNo & "'" 'BATCHNO
                            strsql += " ,'CG'" 'TAXID
                            strsql += " ," & Val(.Cells("GROSSAMT").Value.ToString) & "" 'AMOUNT
                            strsql += " ," & Val(.Cells("WCGSTPER").Value.ToString) & "" 'TAXPER
                            strsql += " ," & Val(.Cells("WCGST").Value.ToString)
                            strsql += " ,'RG'" 'TAXTYPE
                            strsql += " ,8" 'TSNO
                            strsql += " ,'" & CostCenterId & "'" 'COSTID
                            strsql += " ,'" & strCompanyId & "'" 'COMPANYID
                            strsql += " )"
                            ExecQuery(SyncMode.Transaction, strsql, cn, Tran, CostCenterId)
                        End If
                        If Val(.Cells("WIGST").Value.ToString) <> 0 Then
                            strsql = " INSERT INTO " & cnStockDb & "..TAXTRAN"
                            strsql += " ("
                            strsql += " SNO,ISSSNO,ACCODE,TRANNO,TRANDATE,TRANTYPE,BATCHNO,TAXID"
                            strsql += " ,AMOUNT,TAXPER,TAXAMOUNT,TAXTYPE,TSNO,COSTID,COMPANYID"
                            strsql += " )"
                            strsql += " VALUES("
                            strsql += " '" & GetNewSno(TranSnoType.TAXTRANCODE, Tran) & "'" ''SNO
                            strsql += " ,'" & issSno & "'" ''ISSSNO
                            strsql += " ,''"
                            strsql += " ," & TranNo & "" 'TRANNO
                            strsql += " ,'" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                            If .Cells("TRANTYPE").Value.ToString = "ISSUE" Then
                                strsql += " ,'I" & Mid(.Cells("PAYMODE").Value.ToString, 1, 2) & "'" 'TRANTYPE
                            Else
                                strsql += " ,'R" & Mid(.Cells("PAYMODE").Value.ToString, 1, 2) & "'" 'TRANTYPE
                            End If
                            strsql += " ,'" & BatchNo & "'" 'BATCHNO
                            strsql += " ,'IG'" 'TAXID
                            strsql += " ," & Val(.Cells("GROSSAMT").Value.ToString) & "" 'AMOUNT
                            strsql += " ," & Val(.Cells("WIGSTPER").Value.ToString) & "" 'TAXPER
                            strsql += " ," & Val(.Cells("WIGST").Value.ToString)
                            strsql += " ,'RG'" 'TAXTYPE
                            strsql += " ,9" 'TSNO
                            strsql += " ,'" & CostCenterId & "'" 'COSTID
                            strsql += " ,'" & strCompanyId & "'" 'COMPANYID
                            strsql += " )"
                            ExecQuery(SyncMode.Transaction, strsql, cn, Tran, CostCenterId)
                        End If
                        'UPDATE ITEMTAG
                        If .Cells("ESTTAGNO").Value.ToString.Trim <> "" Then
                            strsql = ""
                            strsql = ""
                            strsql += vbCrLf + " UPDATE " & cnAdminDb & "..ITEMTAG  "
                            strsql += vbCrLf + " SET ISSDATE = '" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "' "
                            strsql += vbCrLf + " ,ISSREFNO = '" & TranNo & "' "
                            strsql += vbCrLf + " ,ISSPCS = " & Val(.Cells("PCS").Value.ToString) & " "
                            strsql += vbCrLf + " ,ISSWT = " & .Cells("GRSWT").Value.ToString & " "
                            strsql += vbCrLf + " ,TOFLAG = 'MI' "
                            strsql += vbCrLf + " ,BATCHNO = '" & BatchNo & "' "
                            strsql += vbCrLf + " ,APPROVAL = '' "
                            strsql += vbCrLf + " ,COMPANYID = '" & strCompanyId & "' "
                            strsql += vbCrLf + " WHERE ITEMID = '" & Val(.Cells("ITEMID").Value.ToString) & "' "
                            strsql += vbCrLf + " And TAGNO = '" & .Cells("ESTTAGNO").Value.ToString & "'"
                            ExecQuery(SyncMode.Transaction, strsql, cn, Tran, CostCenterId)
                            strsql = ""
                            strsql = " UPDATE " & cnAdminDb & "..ITEMTAGSTONE "
                            strsql += " SET COMPANYID = '" & strCompanyId & "'"
                            strsql += " WHERE ITEMID = '" & Val(.Cells("ITEMID").Value.ToString) & "' AND TAGNO = '" & .Cells("ESTTAGNO").Value.ToString & "'"
                            ExecQuery(SyncMode.Transaction, strsql, cn, Tran, CostCenterId)
                            strsql = " UPDATE " & cnStockDb & "..ESTISSUE SET BATCHNO='" & BatchNo & "' WHERE SNO='" & .Cells("ESTSNO").Value.ToString & "'"
                            ExecQuery(SyncMode.Transaction, strsql, cn, Tran, CostCenterId)
                            strsql = ""
                        End If
                    End With
                Next
                If AccPost Then
                    InsertSASRPUAccountDet()
                    ''UPDATE CONTRA
                    strsql = " UPDATE " & cnStockDb & ".." & IIf(_AccAudit, "TACCTRAN", "ACCTRAN") & " SET CONTRA = "
                    strsql += " (SELECT TOP 1 ACCODE FROM " & cnStockDb & ".." & IIf(_AccAudit, "TACCTRAN", "ACCTRAN") & " WHERE BATCHNO = T.BATCHNO AND TRANMODE = 'C' AND ACCODE <> '' AND ACCODE <> T.ACCODE ORDER BY SNO)"
                    strsql += " FROM " & cnStockDb & ".." & IIf(_AccAudit, "TACCTRAN", "ACCTRAN") & " AS T"
                    strsql += " WHERE TRANDATE = '" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "' AND BATCHNO = '" & BatchNo & "' AND TRANMODE = 'D' AND ISNULL(CONTRA,'') = ''"
                    ExecQuery(SyncMode.Transaction, strsql, cn, Tran, CostCenterId)
                    strsql = " UPDATE " & cnStockDb & ".." & IIf(_AccAudit, "TACCTRAN", "ACCTRAN") & " SET CONTRA = "
                    strsql += " (SELECT TOP 1 ACCODE FROM " & cnStockDb & ".." & IIf(_AccAudit, "TACCTRAN", "ACCTRAN") & " WHERE BATCHNO = T.BATCHNO AND TRANMODE = 'D' AND ACCODE <> '' AND ACCODE <> T.ACCODE ORDER BY SNO)"
                    strsql += " FROM " & cnStockDb & ".." & IIf(_AccAudit, "TACCTRAN", "ACCTRAN") & " AS T"
                    strsql += " WHERE TRANDATE = '" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "' AND BATCHNO = '" & BatchNo & "' AND TRANMODE = 'C'  AND ISNULL(CONTRA,'') = ''"
                    ExecQuery(SyncMode.Transaction, strsql, cn, Tran, CostCenterId)
                    Dim balAmt As Double = Val(objGPack.GetSqlValue("SELECT SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) AS BALANCE FROM " & cnStockDb & ".." & IIf(_AccAudit, "TACCTRAN", "ACCTRAN") & " WHERE BATCHNO = '" & BatchNo & "'", , "0", Tran))
                    If balAmt <> 0 Then
                        If Not Tran Is Nothing Then Tran.Rollback()
                        Tran = Nothing
                        MsgBox("Credit Debit Not Tally : " + balAmt.ToString, MsgBoxStyle.Information)
                        Exit Sub
                    End If
                End If
                Tran.Commit()
                Tran = Nothing
                Dim PrintTran As String = "Tranno : " & TranNo & vbCrLf & " Batchno : " & BatchNo
                Dim pBatchno As String = BatchNo
                Dim pBillDate As String = dtpTrandate.Value.ToString("yyyy-MM-dd")
                Dim PAcname As String = cmbAcname_OWN.Text
                Dim PAccode As String = cmbAcname_OWN.SelectedValue.ToString
                Dim dtBoardRate As New DataTable
                dtBoardRate = Gridview_OWN.DataSource
                Dim dvRateCutSms As New DataView(dtBoardRate)
                dvRateCutSms.RowFilter = "TRANTYPE='RECEIPT' AND (MCPER> 0 OR GRSWT > 0)"
                dtBoardRate = dvRateCutSms.ToTable
                If dtBoardRate.Rows.Count > 0 Then
                    SmsForMIMR(TranNo, dtpTrandate.Value.Date, PAcname, dtBoardRate.Rows(0).Item("BOARDRATE"), PAccode)
                End If
                btnNew_Click(Me, New System.EventArgs)
                MsgBox("Saved Sucessfully " & vbCrLf & PrintTran, MsgBoxStyle.Information)
                If GetAdmindbSoftValue("MIMR_PRINT", "N") = "Y" Then 'oMaterial = Material.Receipt
                    'final 10June22
                    Dim objBill As New frmBillPrintDoc_RSR("SMR", pBatchno, pBillDate, "N", "NEW")
                Else
                    Dim prnmemsuffix As String = ""
                    If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId
                    If IO.File.Exists(Application.StartupPath & "\BillPrint.exe") Then
                        Dim write As IO.StreamWriter
                        If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId
                        Dim memfile As String = "\BillPrint" + prnmemsuffix.Trim & ".mem"
                        write = IO.File.CreateText(Application.StartupPath & memfile)
                        write.WriteLine(LSet("TYPE", 15) & ":SMIMR")
                        write.WriteLine(LSet("BATCHNO", 15) & ":" & pBatchno)
                        write.WriteLine(LSet("TRANDATE", 15) & ":" & pBillDate.ToString)
                        write.WriteLine(LSet("DUPLICATE", 15) & ":N")
                        write.Flush()
                        write.Close()
                        If EXE_WITH_PARAM = False Then
                            System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe")
                        Else
                            System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe",
                        LSet("TYPE", 15) & ":SMIMR" & ";" &
                        LSet("BATCHNO", 15) & ":" & pBatchno & ";" &
                        LSet("TRANDATE", 15) & ":" & pBillDate.ToString & ";" &
                        LSet("DUPLICATE", 15) & ":N")
                        End If
                    Else
                        MsgBox("Billprint exe not found", MsgBoxStyle.Information)
                    End If
                End If
            Catch ex As Exception
                If Not Tran Is Nothing Then
                    Tran.Rollback()
                    Tran = Nothing
                    MessageBox.Show(ex.ToString)
                Else
                    MessageBox.Show(ex.ToString)
                    btnNew_Click(Me, New System.EventArgs)
                End If
            Finally
                Me.Cursor = Cursors.Default
                btnSave.Enabled = True
                SaveToolStripMenuItem.ShortcutKeys = Keys.F1
            End Try
        End If
    End Sub
    Private Sub SmsForMIMR(ByVal tranno As Integer, ByVal trandate As Date, ByVal PAcname As String, ByVal Boardrate As Double, ByVal paccode As String)
        If Boardrate = 0 Then Exit Sub
        Dim getMobile() As String = SMS_OTP_RATECHANGE_MIMR_MOBILENO.Split(",")
        Dim msgcontent As String = ""
        strsql = ""
        strsql = " SELECT TEMPLATE_DESC FROM " & cnAdminDb & "..SMSTEMPLATE "
        strsql += vbCrLf + " WHERE TEMPLATE_NAME = 'SMS_MIMR_RATECHANGE' and ACTIVE = 'Y'"
        msgcontent = GetSqlValue(cn, strsql)
        If msgcontent = "" Then
            Exit Sub
        End If
        msgcontent = msgcontent.Replace("<BOARDRATE>", Format(Val(Boardrate), "0.00"))
        msgcontent = msgcontent.Replace("<ACNAME>", PAcname)
        msgcontent = msgcontent.Replace("<TRANNO>", tranno)
        msgcontent = msgcontent.Replace("<TRANDATE>", trandate)
        msgcontent = msgcontent.Replace("<DATE>", Format(Now.Date, "dd/MM/yyyy"))
        msgcontent = msgcontent.Replace("<TIME>", Date.Now.ToShortTimeString)
        For i As Integer = 0 To getMobile.Length - 1
            SmsSend(msgcontent, getMobile(i))
        Next
        Dim AcMobile As String = ""
        strsql = " SELECT ISNULL(MOBILE,'') MOBILE FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE='" & paccode & "'"
        AcMobile = objGPack.GetSqlValue(strsql)
        If AcMobile <> "" Then
            SmsSend(msgcontent, AcMobile)
        End If
    End Sub

    Private Sub InsertSASRPUAccountDet()
        Dim dtCategory As New DataTable
        'dtCategory = DtTran.DefaultView.ToTable(True, "CATNAME") 'ACCATNAME
        dtCategory = DtTran.DefaultView.ToTable(True, "CATNAME", "PAYMODE", "TRANTYPE") 'ACCATNAME
        InsertSASRPUAccountDet(TranNo, dtCategory, DtTran) 'IIf(oMaterial = Material.Issue, "TI", "TR")
    End Sub

    Private Sub INSERTTCS(ByVal TCSAmt As Double _
                          , ByVal TrantypeNew As String _
                          , ByVal amt As Double _
                          , ByVal TCSPerNew As Double _
                          , ByVal _Accode As String _
                          , ByVal tNo As Integer)
        If TCSAmt > 0 Then
            strsql = " INSERT INTO " & cnStockDb & "..TAXTRAN"
            strsql += " ("
            strsql += " SNO,ACCODE,CONTRA,TRANNO,TRANDATE,TRANTYPE,BATCHNO,TAXID"
            strsql += " ,AMOUNT,TAXPER,TAXAMOUNT,TAXTYPE,TSNO,COSTID,COMPANYID"
            strsql += " )"
            strsql += " VALUES("
            strsql += " '" & GetNewSno(TranSnoType.TAXTRANCODE, Tran) & "'" ''SNO
            strsql += " ,'TCS'"
            strsql += " ,'" & _Accode & "'"
            strsql += " ," & TranNo & "" 'TRANNO
            strsql += " ,'" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
            strsql += " ,'" & TrantypeNew & "'"
            strsql += " ,'" & BatchNo & "'" 'BATCHNO
            strsql += " ,'TC'" 'TAXID
            strsql += " ," & amt & "" 'AMOUNT
            strsql += " ," & TCSPerNew & "" 'TAXPER
            strsql += " ," & TCSAmt
            strsql += " ,'TC'"
            strsql += " ,1" 'TSNO
            strsql += " ,'" & CostCenterId & "'"
            strsql += " ,'" & strCompanyId & "'"
            strsql += " )"
            ExecQuery(SyncMode.Transaction, strsql, cn, Tran, CostCenterId)
            Dim payMode As String = "TR"
            Dim AccodeTCS As String = "TCS"
            If TrantypeNew = "TR" Then
                AccodeTCS = "TCS_PU"
            Else
                AccodeTCS = "TCS"
            End If
            InsertIntoAccTran(tNo, "D", AccodeTCS, TCSAmt, 0, 0, 0, payMode, _Accode, txtInvoiceNo.Text, dtpTrandate.Value.ToString("yyyy-MM-dd"), , , , "")
            'InsertIntoAccTran(tNo, "D", _Accode, TCSAmt, 0, 0, 0, payMode, "TCS", txtInvoiceNo.Text, dtpTrandate.Value.ToString("yyyy-MM-dd"), , , , "")
        End If
    End Sub
    Private Sub INSERTTDS(ByVal TDSAmt As Double, ByVal accode As String _
                          , ByVal TdsContra As String, ByVal payMode As String _
                          , ByVal amt As Double _
                          , ByVal TDSPerNew As String, ByVal TTdsAccode As String _
                          , ByVal tNo As Integer, ByVal TdsRemark As String)
        If TDSAmt > 0 Then
            strsql = " INSERT INTO " & cnStockDb & "..TAXTRAN"
            strsql += " ("
            strsql += " SNO,ACCODE,CONTRA,TRANNO,TRANDATE,TRANTYPE,BATCHNO,TAXID"
            strsql += " ,AMOUNT,TAXPER,TAXAMOUNT,TAXTYPE,TSNO,COSTID,COMPANYID"
            strsql += " )"
            strsql += " VALUES("
            strsql += " '" & GetNewSno(TranSnoType.TAXTRANCODE, Tran) & "'" ''SNO
            strsql += " ,'" & accode & "'"
            strsql += " ,'" & TdsContra & "'"
            strsql += " ," & TranNo & "" 'TRANNO
            strsql += " ,'" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
            strsql += " ,'" & payMode & "'"
            strsql += " ,'" & BatchNo & "'" 'BATCHNO
            strsql += " ,'TD'" 'TAXID
            strsql += " ," & amt & "" 'AMOUNT
            strsql += " ," & TDSPerNew & "" 'TAXPER 'Replace TdsPer to TDSPerNew
            strsql += " ," & TDSAmt
            strsql += " ,'TD'"
            strsql += " ,1" 'TSNO
            strsql += " ,'" & CostCenterId & "'"
            strsql += " ,'" & strCompanyId & "'"
            strsql += " )"
            ExecQuery(SyncMode.Transaction, strsql, cn, Tran, CostCenterId)
            If TTdsAccode <> "" Then
                accode = TTdsAccode
            Else
                accode = TdsAc
            End If
            payMode = "TR"
            InsertIntoAccTran(tNo, "C", accode, TDSAmt, 0, 0, 0, payMode, _Accode, txtInvoiceNo.Text, dtpTrandate.Value.ToString("yyyy-MM-dd"), , , , Mid(TdsRemark, 1, 50))
            'InsertIntoAccTran(tNo, "D", _Accode, TDSAmt, 0, 0, 0, payMode, accode, txtInvoiceNo.Text, dtpTrandate.Value.ToString("yyyy-MM-dd"), , , , Mid(TdsRemark, 1, 50))
        End If
    End Sub
    Private Sub InsertSASRPUAccountDet(ByVal tNo As Integer, ByVal dtCategory As DataTable, ByVal matchTable As DataTable) 'ByVal insertTranType As String
        Dim TTdsAccode As String = ""
        For Each roCategory As DataRow In dtCategory.Rows
            Dim TdsRemark As String = Nothing
            Dim pcs As Integer = Nothing
            Dim grsWt As Double = Nothing
            Dim netWT As Double = Nothing
            Dim vatAmt As Double = Nothing
            Dim stnVat As Double = Nothing
            Dim amt As Double = Nothing
            Dim addCharge As Decimal = Nothing
            Dim Remark1 As String = Nothing
            Dim Remark2 As String = Nothing
            Dim TempAmount As Double = Nothing
            Dim TempVat As Double = Nothing
            Dim RowTemp As DataRow = Nothing
            Dim dtStoneAcc As New DataTable
            'Dim dtMiscAcc As New DataTable
            'Dim dtAlloyAcc As New DataTable
            Dim EDPer As Double = Nothing
            Dim EDAmt As Double = Nothing
            Dim EDGrsAmt As Double = Nothing
            Dim SGSTAmt As Double = Nothing
            Dim CGSTAmt As Double = Nothing
            Dim IGSTAmt As Double = Nothing
            Dim TDSAmt As Double = Nothing
            Dim TCSAmt As Double = Nothing
            Dim TDSPerNew As Double = Nothing
            Dim TCSPerNew As Double = Nothing
            Dim TrantypeNew As String = ""
            tNo = funcPaymode(roCategory!PAYMODE.ToString, roCategory!TRANTYPE.ToString)
            'STONE ACC
            dtStoneAcc.Columns.Add("CODE", GetType(String))
            dtStoneAcc.Columns.Add("TAXCODE", GetType(String))
            dtStoneAcc.Columns.Add("TAXVALUE", GetType(Decimal))
            dtStoneAcc.Columns.Add("PCS", GetType(Decimal))
            dtStoneAcc.Columns.Add("GRSWT", GetType(Decimal))
            dtStoneAcc.Columns.Add("AMOUNT", GetType(Decimal))

            ''MISC ACC
            'dtMiscAcc.Columns.Add("CODE", GetType(String))
            'dtMiscAcc.Columns.Add("AMOUNT", GetType(Decimal))
            ''ALLOY ACC
            'dtAlloyAcc.Columns.Add("ALLOYID", GetType(String))
            'dtAlloyAcc.Columns.Add("WEIGHT", GetType(Decimal))


            Dim TdsContra As String = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME='" & cmbAcname_OWN.Text & "' ", , "", Tran)
            Dim TTdscatid As Integer = Val(objGPack.GetSqlValue("SELECT ISNULL(TDSCATID,0) FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME='" & cmbAcname_OWN.Text & "' ", , "", Tran))

            If TTdscatid <> 0 Then
                TTdsAccode = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE IN (SELECT TOP 1 ACCODE FROM " & cnAdminDb & "..TDSCATEGORY WHERE TDSCATID='" & TTdscatid & "') ", , "", Tran)
            End If
            Dim IsOutStation As String = ""
            IsOutStation = objGPack.GetSqlValue("SELECT ISNULL(LOCALOUTST,'') FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME='" & cmbAcname_OWN.Text & "' ", , "", Tran)
            If roCategory!CATNAME.ToString = "" Then Continue For
            'MatchTable For
            For Each roMath As DataRow In matchTable.Rows
                If roMath.RowState = DataRowState.Deleted Then GoTo nnext
                If roMath!TRANTYPE.ToString = "" Then Exit For
                'If roCategory!CATNAME.ToString <> roMath!CATNAME.ToString Then Continue For
                If roCategory!CATNAME.ToString = roMath!CATNAME.ToString _
                And roCategory!TRANTYPE.ToString = roMath!TRANTYPE.ToString _
                And roCategory!PAYMODE.ToString = roMath!PAYMODE.ToString Then
                    pcs += Val(roMath!PCS.ToString)
                    grsWt += Val(roMath!GRSWT.ToString)
                    netWT += Val(roMath!NETWT.ToString)
                    If IsOutStation = "O" And roCategory!PAYMODE.ToString = "PURCHASE" And MRMI_VATSEPPOST = False Then 'cmbTrantype.Text = "PURCHASE"
                        vatAmt += 0
                    Else
                        'vatAmt += Val(roMath!VAT.ToString) 'vatAmt += 0
                        vatAmt += Val(roMath!GST.ToString)
                    End If
                    'EDPer = Val(roMath!EDPER.ToString)
                    'EDAmt += Val(roMath!ED.ToString)
                    SGSTAmt += Val(roMath!SGST.ToString)
                    CGSTAmt += Val(roMath!CGST.ToString)
                    IGSTAmt += Val(roMath!IGST.ToString)
                    TDSAmt += Val(roMath!TDSVAL.ToString)
                    TCSAmt += Val(roMath!TCSVAL.ToString)
                    TDSPerNew = Val(roMath!TDSPER.ToString)
                    TCSPerNew = Val(roMath!TCSPER.ToString)
                    If roMath!TRANTYPE.ToString = "ISSUE" Then
                        TrantypeNew = "I" & Mid(roMath!PAYMODE, 1, 2) & "" 'TRANTYPE
                    Else
                        TrantypeNew = "R" & Mid(roMath!PAYMODE, 1, 2) & "" 'TRANTYPE
                    End If
                    'addCharge += Val(roMath!ADDCHARGE.ToString)
                    If IsOutStation = "O" And roCategory!PAYMODE.ToString = "PURCHASE" And MRMI_VATSEPPOST = False Then 'cmbTrantype.Text = "PURCHASE"
                        amt += Val(roMath!GROSSAMT.ToString) + Val(roMath!VAT.ToString)
                        EDGrsAmt += Val(roMath!GROSSAMT.ToString)
                    Else
                        amt += Val(roMath!GROSSAMT.ToString) ''- Val(roMath!DISCOUNT.ToString)
                        EDGrsAmt += Val(roMath!GROSSAMT.ToString)
                    End If
                    If roCategory!PAYMODE.ToString <> "INTERNAL TRANSFER" Then 'cmbTrantype.Text <> "INTERNAL TRANSFER"
                        For Each stRow As DataRow In dtStone.Rows 'CType(roMath.Item("METISSREC"), MaterialIssRec).objStone.dtGridStone.Rows
                            'Dim diaStn As String = objGPack.GetSqlValue("SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & stRow!ITEM.ToString & "'", , , Tran).ToUpper
                            'If Not (SepAccPost And SEPACCPOST_ITEM.Contains(diaStn)) Then Continue For
                            'RowTemp = dtStoneAcc.NewRow
                            'If oMaterial = Material.Issue Then
                            '    strsql = " SELECT CA.SPRETURNID,CA.STAXID,CA.SALESTAX"
                            'Else
                            '    strsql = " SELECT CA.PURCHASEID,CA.PTAXID,CA.PTAX"
                            'End If
                            'strsql += " FROM " & cnAdminDb & "..ITEMMAST AS IM "
                            'strsql += " INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = IM.CATCODE"
                            'strsql += " WHERE IM.ITEMNAME = '" & stRow!ITEM.ToString & "'"
                            'Dim dtrow As DataRow = GetSqlRow(strsql, cn, Tran)
                            'RowTemp!CODE = dtrow(0).ToString
                            'RowTemp!TAXCODE = dtrow(1).ToString
                            'RowTemp!PCS = Val(stRow!PCS.ToString)
                            'RowTemp!GRSWT = Val(stRow!WEIGHT.ToString)
                            'RowTemp!AMOUNT = Val(stRow!AMOUNT.ToString)
                            'If vatAmt <> 0 Then RowTemp!TAXVALUE = Math.Round(Val(stRow!AMOUNT.ToString) * (Val(dtrow(2).ToString) / 100), 2)
                            'dtStoneAcc.Rows.Add(RowTemp)
                        Next
                        'For Each stRow As DataRow In CType(roMath.Item("METISSREC"), MaterialIssRec).objMisc.dtGridMisc.Rows
                        '    RowTemp = dtMiscAcc.NewRow
                        '    strsql = " SELECT ACCTID FROM " & cnAdminDb & "..MISCCHARGES WHERE MISCNAME = '" & stRow!MISC.ToString & "'"
                        '    RowTemp!CODE = objGPack.GetSqlValue(strsql, , , Tran)
                        '    RowTemp!AMOUNT = Val(stRow!AMOUNT.ToString)
                        '    dtMiscAcc.Rows.Add(RowTemp)
                        'Next
                        'For Each stRow As DataRow In CType(roMath.Item("METISSREC"), MaterialIssRec).ObjAlloy.dtGridAlloy.Rows
                        '    RowTemp = dtAlloyAcc.NewRow
                        '    strsql = " SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & stRow!ALLOY.ToString & "'"
                        '    RowTemp!ALLOYID = objGPack.GetSqlValue(strsql, , , Tran)
                        '    RowTemp!WEIGHT = Val(stRow!WEIGHT.ToString)
                        '    dtAlloyAcc.Rows.Add(RowTemp)
                        'Next
                    End If
                Else
                    Continue For
                    'Exit For
                End If
nnext:
            Next
            Dim temptdsamt As Double = 0
            Dim tranMode As String = Nothing
            Dim accode As String = Nothing
            Dim payMode As String = Nothing
            'STONE STARING ACCTRAN POSTING
            If roCategory!PAYMODE.ToString <> "INTERNAL TRANSFER" Then 'cmbTrantype.Text <> "INTERNAL TRANSFER"
                If roCategory!TRANTYPE.ToString = "RECEIPT" Then 'oMaterial = Material.Receipt
                    tranMode = "D"
                    payMode = "TR"
                Else
                    tranMode = "C"
                    payMode = "TI"
                End If
                ''StuddedPosting
                If vatAmt <> 0 Then
                    For Each RowStuddedTax As DataRow In dtStoneAcc.DefaultView.ToTable(True, "TAXCODE").Rows
                        accode = RowStuddedTax!TAXCODE.ToString
                        TempVat = Val(dtStoneAcc.Compute("SUM(TAXVALUE)", "TAXCODE = '" & accode & "'").ToString)
                        stnVat += TempVat
                        If Not (_Acctype = "O" And CstPurchsep = False _
                        And (roCategory!PAYMODE.ToString = "PURCHASE" Or roCategory!PAYMODE.ToString = "PURCHASE[APPROVAL]")) _
                        And roCategory!PAYMODE.ToString <> "RECEIPT" Then
                            'And (cmbTrantype.Text = "PURCHASE" Or cmbTrantype.Text = "PURCHASE[APPROVAL]")) _
                            'And cmbTrantype.Text <> "RECEIPT" Then
                            InsertIntoAccTran(tNo, tranMode, accode, TempVat,
                            Val(dtStoneAcc.Compute("SUM(PCS)", "TAXCODE = '" & accode & "'").ToString),
                            Val(dtStoneAcc.Compute("SUM(GRSWT)", "TAXCODE = '" & accode & "'").ToString),
                            Val(dtStoneAcc.Compute("SUM(GRSWT)", "TAXCODE = '" & accode & "'").ToString),
                            payMode, _Accode, txtInvoiceNo.Text, dtpTrandate.Value.ToString("yyyy-MM-dd"))
                            'InsertIntoAccTran(tNo, IIf(tranMode = "D", "C", "D"), _Accode, TempVat, _
                            'Val(dtStoneAcc.Compute("SUM(PCS)", "TAXCODE = '" & accode & "'").ToString), _
                            'Val(dtStoneAcc.Compute("SUM(GRSWT)", "TAXCODE = '" & accode & "'").ToString), _
                            'Val(dtStoneAcc.Compute("SUM(GRSWT)", "TAXCODE = '" & accode & "'").ToString), _
                            'payMode, accode, txtInvoiceNo.Text, dtpTrandate.Value.ToString("yyyy-MM-dd"))
                            vatAmt -= TempVat
                        End If
                        TempAmount = Nothing
                    Next
                End If
                'STONE POSTING
                For Each RowStuddedAcc As DataRow In dtStoneAcc.DefaultView.ToTable(True, "CODE").Rows
                    accode = RowStuddedAcc!CODE.ToString
                    TempAmount = Val(dtStoneAcc.Compute("SUM(AMOUNT)", "CODE = '" & accode & "'").ToString)
                    If Not (_Acctype = "O" And CstPurchsep = False And
                    (roCategory!PAYMODE.ToString = "PURCHASE" Or roCategory!PAYMODE.ToString = "PURCHASE[APPROVAL]")) Then
                        '(cmbTrantype.Text = "PURCHASE" Or cmbTrantype.Text = "PURCHASE[APPROVAL]")) Then
                        InsertIntoAccTran(tNo, tranMode, accode, TempAmount,
                        Val(dtStoneAcc.Compute("SUM(PCS)", "CODE = '" & accode & "'").ToString),
                        Val(dtStoneAcc.Compute("SUM(GRSWT)", "CODE = '" & accode & "'").ToString),
                        Val(dtStoneAcc.Compute("SUM(GRSWT)", "CODE = '" & accode & "'").ToString),
                        payMode, _Accode, txtInvoiceNo.Text, dtpTrandate.Value.ToString("yyyy-MM-dd"))
                        'SU
                        'InsertIntoAccTran(tNo, IIf(tranMode = "D", "C", "D"), _Accode, TempAmount,
                        'Val(dtStoneAcc.Compute("SUM(PCS)", "CODE = '" & accode & "'").ToString),
                        'Val(dtStoneAcc.Compute("SUM(GRSWT)", "CODE = '" & accode & "'").ToString),
                        'Val(dtStoneAcc.Compute("SUM(GRSWT)", "CODE = '" & accode & "'").ToString),
                        'payMode, accode, txtInvoiceNo.Text, dtpTrandate.Value.ToString("yyyy-MM-dd"))
                        amt -= TempAmount
                    End If
                    TempAmount = Nothing
                Next
                ''MiscChargePost
                'If roCategory!TRANTYPE.ToString = "RECEIPT" And (roCategory!PAYMODE.ToString <> "PURCHASE" Or roCategory!PAYMODE.ToString <> "PURCHASE[APPROVAL]") Then
                '    'oMaterial = Material.Receipt
                '    '(cmbTrantype.Text <> "PURCHASE" Or cmbTrantype.Text <> "PURCHASE[APPROVAL]")
                '    For Each RowMiscAcc As DataRow In dtMiscAcc.DefaultView.ToTable(True, "CODE").Rows
                '        accode = RowMiscAcc!CODE.ToString
                '        TempAmount = Val(dtMiscAcc.Compute("SUM(AMOUNT)", "CODE = '" & accode & "'").ToString)
                '        Dim tempvatamt As Double
                '        If Not (_Acctype = "O" And CstPurchsep = False _
                '        And (roCategory!PAYMODE.ToString = "PURCHASE" Or roCategory!PAYMODE.ToString <> "PURCHASE[APPROVAL]")) Then
                '            'And (cmbTrantype.Text = "PURCHASE" Or cmbTrantype.Text <> "PURCHASE[APPROVAL]")) Then
                '            InsertIntoAccTran(tNo, tranMode, accode, TempAmount, 0, 0, 0, payMode, _Accode, txtInvoiceNo.Text, dtpTrandate.Value.ToString("yyyy-MM-dd"))
                '            If roCategory!PAYMODE.ToString = "RECEIPT" And vatAmt <> 0 And TdsPer <> 0 Then 'cmbTrantype.Text = "RECEIPT"
                '                tempvatamt = TempAmount * (TdsPer / 100)
                '                temptdsamt += tempvatamt
                '                TdsRemark = "TDS Rs " & Format(tempvatamt, "0.00") & " @" & Format(TdsPer, "0.00") & "% for " & Format(TempAmount, "0.00")
                '            End If
                '            InsertIntoAccTran(tNo, IIf(tranMode = "D", "C", "D"), _Accode, TempAmount, 0, 0, 0, payMode, accode, txtInvoiceNo.Text, dtpTrandate.Value.ToString("yyyy-MM-dd"), , , , TdsRemark, , , IIf(tempvatamt <> 0, IIf(TTdscatid <> 0, TTdscatid, TdsCatId), 0), TdsPer, tempvatamt)
                '            amt -= TempAmount
                '        End If
                '        TempAmount = Nothing
                '    Next
                'End If
            End If
            'STONE ENDING ACCTRAN POSTING
            'End If
            ''INSERTING SALES ACC
            If roCategory!TRANTYPE.ToString = "RECEIPT" Then 'oMaterial = Material.Receipt
                'Dim SepPurAccPost As Boolean = IIf(GetAdmindbSoftValue("SEPPOST_ACC_PURCHASE", "Y", tran) = "Y", True, False)
                ''INSERTING PURCHASE ACC
                If amt <> 0 Then
                    If roCategory!PAYMODE.ToString = "INTERNAL TRANSFER" Then 'cmbTrantype.Text = "INTERNAL TRANSFER"
                        'Not Used MIMR Project 11-Jan-2021
                        'tranMode = "D"
                        'payMode = "TR"
                        'accode = "STKTRAN"
                        'InsertIntoAccTran(tNo, tranMode, accode, amt, pcs, grsWt, netWT, payMode, _Accode, dtpTrandate.Value.ToString("yyyy-MM-dd"))
                        'InsertIntoAccTran(tNo, IIf(tranMode = "D", "C", "D"), _Accode,
                        'amt, pcs, grsWt, netWT, payMode, accode, txtInvoiceNo.Text, dtpTrandate.Value.ToString("yyyy-MM-dd"))
                    ElseIf roCategory!PAYMODE.ToString = "PURCHASE" Or roCategory!PAYMODE.ToString = "PURCHASE[APPROVAL]" Then 'cmbTrantype.Text = "PURCHASE" Or cmbTrantype.Text = "PURCHASE[APPROVAL]"
                        stnVat = stnVat - TempVat
                        tranMode = "D"
                        payMode = "TR"
                        Dim mamt As Decimal = 0
                        strsql = " SELECT PURCHASEID"
                        strsql += " FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & roCategory!CATNAME.ToString & "'"
                        accode = objGPack.GetSqlValue(strsql, , , Tran)
                        If Not (_Acctype = "O" And CstPurchsep = False) Then mamt = amt Else mamt = amt + vatAmt
                        InsertIntoAccTran(tNo, tranMode, accode, mamt, pcs, grsWt, netWT, payMode, _Accode, txtInvoiceNo.Text, dtpTrandate.Value.ToString("yyyy-MM-dd") _
                        , , , , , , , , , , , , , SGSTAmt, CGSTAmt, IGSTAmt)
                        'SU
                        'InsertIntoAccTran(tNo, tranMode, "EXDUTY", EDAmt, 0, 0, 0, payMode,
                        '_Accode, txtInvoiceNo.Text, dtpTrandate.Value.ToString("yyyy-MM-dd") _
                        ', , , , , , , , , , EDPer, EDGrsAmt, EDAmt)
                        'InsertIntoAccTran(tNo, IIf(tranMode = "D", "C", "D"),
                        '_Accode,
                        'amt + vatAmt + stnVat + TempVat + EDAmt, pcs, grsWt, netWT, payMode, accode, txtInvoiceNo.Text, dtpTrandate.Value.ToString("yyyy-MM-dd"))
                        'INSERTTDS(TDSAmt, accode, TdsContra, payMode, vatAmt, temptdsamt, TTdscatid, amt, TDSPerNew, TTdsAccode, tNo, TdsRemark)
                        'INSERTTCS(TCSAmt, TrantypeNew, amt, TCSPerNew, accode, tNo)
                        'SU
                    Else ' RECEIPT
                        TdsRemark = "TDS Rs " & Format(vatAmt, "0.00") & " @" & Format(TdsPer, "0.00") & "% for " & Format(amt, "0.00")
                        tranMode = "D"
                        payMode = "TR"
                        strsql = " SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME= '" & roCategory!CATNAME.ToString & "'"
                        accode = objGPack.GetSqlValue(strsql, , , Tran) + "MAKP"
                        InsertIntoAccTran(tNo, tranMode, accode, amt, pcs, grsWt _
                                          , netWT, payMode, _Accode, txtInvoiceNo.Text _
                                          , dtpTrandate.Value.ToString("yyyy-MM-dd"))
                        'SU
                        'InsertIntoAccTran(tNo, IIf(tranMode = "D", "C", "D"),
                        '_Accode,
                        '(amt + SGSTAmt + CGSTAmt + IGSTAmt), pcs, grsWt, netWT, payMode, accode, txtInvoiceNo.Text, dtpTrandate.Value.ToString("yyyy-MM-dd"), , , , , , , IIf(vatAmt - temptdsamt <> 0, IIf(TTdscatid <> 0, TTdscatid, TdsCatId), 0), IIf(vatAmt - temptdsamt <> 0, TdsPer, 0), IIf(vatAmt - temptdsamt <> 0, vatAmt - temptdsamt, 0))
                        'INSERTTDS(TDSAmt, accode, TdsContra, payMode, vatAmt, temptdsamt, TTdscatid, amt, TDSPerNew, TTdsAccode, tNo, TdsRemark)
                        'INSERTTCS(TCSAmt, TrantypeNew, amt, TCSPerNew, accode, tNo)
                        'SU
                        If SGSTAmt > 0 Then
                            If roCategory!PAYMODE.ToString = "RECEIPT" Then
                                Dim splitAccode() As String
                                strsql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID='LABOUR_SGST'"
                                splitAccode = objGPack.GetSqlValue(strsql, , , Tran).Split(":")
                                accode = splitAccode(0)
                            Else
                                strsql = "SELECT P_SGSTID FROM " & cnAdminDb & "..CATEGORY "
                                strsql += " WHERE CATNAME = '" & roCategory!CATNAME.ToString & "'"
                                accode = objGPack.GetSqlValue(strsql, , , Tran)
                            End If
                            InsertIntoAccTran(tNo, tranMode, accode, SGSTAmt, 0, 0, 0, payMode, _Accode, txtInvoiceNo.Text, dtpTrandate.Value.ToString("yyyy-MM-dd"))
                        End If
                        If CGSTAmt > 0 Then
                            If roCategory!PAYMODE.ToString = "RECEIPT" Then
                                Dim splitAccode() As String
                                strsql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID='LABOUR_CGST'"
                                splitAccode = objGPack.GetSqlValue(strsql, , , Tran).Split(":")
                                accode = splitAccode(0)
                            Else
                                strsql = "SELECT P_CGSTID FROM " & cnAdminDb & "..CATEGORY "
                                strsql += " WHERE CATNAME = '" & roCategory!CATNAME.ToString & "'"
                                accode = objGPack.GetSqlValue(strsql, , , Tran)
                            End If
                            InsertIntoAccTran(tNo, tranMode, accode, CGSTAmt, 0, 0, 0, payMode, _Accode, txtInvoiceNo.Text, dtpTrandate.Value.ToString("yyyy-MM-dd"))
                        End If
                        If IGSTAmt > 0 Then
                            If roCategory!PAYMODE.ToString = "RECEIPT" Then
                                Dim splitAccode() As String
                                strsql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID='LABOUR_IGST'"
                                splitAccode = objGPack.GetSqlValue(strsql, , , Tran).Split(":")
                                accode = splitAccode(0)
                            Else
                                strsql = "SELECT P_IGSTID FROM " & cnAdminDb & "..CATEGORY "
                                strsql += " WHERE CATNAME = '" & roCategory!CATNAME.ToString & "'"
                                accode = objGPack.GetSqlValue(strsql, , , Tran)
                            End If
                            InsertIntoAccTran(tNo, tranMode, accode, IGSTAmt, 0, 0, 0, payMode, _Accode, txtInvoiceNo.Text, dtpTrandate.Value.ToString("yyyy-MM-dd"))
                        End If
                    End If
                End If
                ''INSERTING PURCHASE
                If vatAmt <> 0 Then
                    vatAmt = Math.Round(vatAmt, 2)
                    If roCategory!PAYMODE.ToString = "PURCHASE" Or roCategory!PAYMODE.ToString = "INTERNAL TRANSFER" Or roCategory!PAYMODE.ToString = "PURCHASE[APPROVAL]" Then 'cmbTrantype.Text = "PURCHASE" Or cmbTrantype.Text = "INTERNAL TRANSFER" Or cmbTrantype.Text = "PURCHASE[APPROVAL]"
                        If GST Then
                            tranMode = "D"
                            payMode = "TR"
                            If SGSTAmt > 0 Then
                                strsql = "SELECT P_SGSTID FROM " & cnAdminDb & "..CATEGORY "
                                strsql += " WHERE CATNAME = '" & roCategory!CATNAME.ToString & "'"
                                accode = objGPack.GetSqlValue(strsql, , , Tran)
                                InsertIntoAccTran(tNo, tranMode, accode, SGSTAmt, 0, 0, 0, payMode, _Accode, txtInvoiceNo.Text, dtpTrandate.Value.ToString("yyyy-MM-dd"))
                            End If
                            If CGSTAmt > 0 Then
                                strsql = "SELECT P_CGSTID FROM " & cnAdminDb & "..CATEGORY "
                                strsql += " WHERE CATNAME = '" & roCategory!CATNAME.ToString & "'"
                                accode = objGPack.GetSqlValue(strsql, , , Tran)
                                InsertIntoAccTran(tNo, tranMode, accode, CGSTAmt, 0, 0, 0, payMode, _Accode, txtInvoiceNo.Text, dtpTrandate.Value.ToString("yyyy-MM-dd"))
                            End If
                            If IGSTAmt > 0 Then
                                strsql = "SELECT P_IGSTID FROM " & cnAdminDb & "..CATEGORY "
                                strsql += " WHERE CATNAME = '" & roCategory!CATNAME.ToString & "'"
                                accode = objGPack.GetSqlValue(strsql, , , Tran)
                                InsertIntoAccTran(tNo, tranMode, accode, IGSTAmt, 0, 0, 0, payMode, _Accode, txtInvoiceNo.Text, dtpTrandate.Value.ToString("yyyy-MM-dd"))
                            End If
                        Else
                            If Not (_Acctype = "O" And CstPurchsep = False) Then
                                tranMode = "D"
                                strsql = "SELECT PTAXID FROM " & cnAdminDb & "..CATEGORY "
                                strsql += " WHERE CATNAME = '" & roCategory!CATNAME.ToString & "'"
                                accode = objGPack.GetSqlValue(strsql, , , Tran)
                                payMode = "TR"
                                InsertIntoAccTran(tNo, tranMode, accode, vatAmt, 0, 0, 0, payMode, _Accode, txtInvoiceNo.Text, dtpTrandate.Value.ToString("yyyy-MM-dd"))
                            End If
                        End If
                    Else
                        'tranMode = "C"
                        'If TTdsAccode <> "" Then
                        '    accode = TTdsAccode
                        'Else
                        '    accode = TdsAc
                        'End If
                        'payMode = "TR"
                        'InsertIntoAccTran(tNo, tranMode, accode, vatAmt, 0, 0, 0, payMode, _Accode, txtInvoiceNo.Text, dtpTrandate.Value.ToString("yyyy-MM-dd"), , , , Mid(TdsRemark, 1, 50))
                        'InsertIntoAccTran(tNo, IIf(tranMode = "D", "C", "D"),
                        '_Accode,
                        'vatAmt, 0, 0, 0, payMode, accode, txtInvoiceNo.Text, dtpTrandate.Value.ToString("yyyy-MM-dd"), , , , Mid(TdsRemark, 1, 50))
                    End If
                End If
            Else
                ''ISSUE
                If amt <> 0 Then
                    If roCategory!PAYMODE.ToString = "INTERNAL TRANSFER" Then 'cmbTrantype.Text = "INTERNAL TRANSFER"
                        'Not Used comment on 11-Jan-2021
                        ' tranMode = "D"
                        ' payMode = "TI"
                        ' accode = "STKTRAN"
                        ' InsertIntoAccTran(tNo, IIf(tranMode = "D", "C", "D"), accode, amt, pcs, grsWt, netWT, payMode, "", txtInvoiceNo.Text, dtpTrandate.Value.ToString("yyyy-MM-dd"))
                        ' InsertIntoAccTran(tNo, tranMode,
                        '_Accode,
                        'amt, pcs, grsWt, netWT, payMode, accode, txtInvoiceNo.Text, dtpTrandate.Value.ToString("yyyy-MM-dd"))
                    ElseIf roCategory!PAYMODE.ToString = "PURCHASE RETURN" Then 'cmbTrantype.Text = "PURCHASE RETURN"
                        tranMode = "C" 'C
                        payMode = "TI"
                        strsql = " SELECT SPRETURNID"
                        strsql += " FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & roCategory!CATNAME.ToString & "'"
                        accode = objGPack.GetSqlValue(strsql, , , Tran)
                        InsertIntoAccTran(tNo, tranMode, accode, amt, pcs, grsWt, netWT, payMode, "", txtInvoiceNo.Text, dtpTrandate.Value.ToString("yyyy-MM-dd"))
                        'SU
                        ' InsertIntoAccTran(tNo, IIf(tranMode = "D", "C", "D"),
                        '_Accode,
                        'amt + vatAmt + stnVat, pcs, grsWt, netWT, payMode, accode, txtInvoiceNo.Text, dtpTrandate.Value.ToString("yyyy-MM-dd"))
                    Else
                        TdsRemark = "TDS Rs " & Format(vatAmt, "0.00") & " @" & Format(TdsPer, "0.00") & "% for " & Format(amt, "0.00")
                        tranMode = "C"
                        payMode = "TI"
                        strsql = " SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME= '" & roCategory!CATNAME.ToString & "'"
                        accode = objGPack.GetSqlValue(strsql, , , Tran) + "MAKR"
                        InsertIntoAccTran(tNo, tranMode, accode, amt, pcs, grsWt, netWT, payMode, "", txtInvoiceNo.Text, dtpTrandate.Value.ToString("yyyy-MM-dd"))
                        'SU
                        ' InsertIntoAccTran(tNo, IIf(tranMode = "D", "C", "D"),
                        '_Accode,
                        '(amt + SGSTAmt + CGSTAmt + IGSTAmt), pcs, grsWt, netWT, payMode, accode, txtInvoiceNo.Text, dtpTrandate.Value.ToString("yyyy-MM-dd"), , , , , , , IIf(vatAmt <> 0, IIf(TTdscatid <> 0, TTdscatid, TdsCatId), 0), IIf(vatAmt <> 0, TdsPer, 0), IIf(vatAmt <> 0, vatAmt, 0))
                        ' If vatAmt <> 0 Then
                        '     strsql = " INSERT INTO " & cnStockDb & "..TAXTRAN"
                        '     strsql += " ("
                        '     strsql += " SNO,ACCODE,CONTRA,TRANNO,TRANDATE,TRANTYPE,BATCHNO,TAXID"
                        '     strsql += " ,AMOUNT,TAXPER,TAXAMOUNT,TAXTYPE,TSNO,COSTID,COMPANYID"
                        '     strsql += " )"
                        '     strsql += " VALUES("
                        '     strsql += " '" & 0 & "'" ''SNO
                        '     strsql += " ,'" & accode & "'"
                        '     strsql += " ,'" & TdsContra & "'"
                        '     strsql += " ," & TranNo & "" 'TRANNO
                        '     strsql += " ,'" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                        '     strsql += " ,'" & payMode & "'"
                        '     strsql += " ,'" & BatchNo & "'" 'BATCHNO
                        '     strsql += " ,'" & IIf(vatAmt - temptdsamt <> 0, IIf(TTdscatid <> 0, TTdscatid, TdsCatId), 0) & "'" 'TAXID
                        '     strsql += " ," & amt & "" 'AMOUNT
                        '     strsql += " ," & TdsPer & "" 'TAXPER
                        '     strsql += " ," & vatAmt
                        '     strsql += " ,'TD'"
                        '     strsql += " ,1" 'TSNO
                        '     strsql += " ,'" & CostCenterId & "'"
                        '     strsql += " ,'" & strCompanyId & "'"
                        '     strsql += " )"
                        '     ExecQuery(SyncMode.Transaction, strsql, cn, Tran, CostCenterId)
                        ' End If
                        'SU
                        If SGSTAmt > 0 Then
                            If True Then
                                Dim splitAccode() As String
                                strsql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID='LABOUR_SGST'"
                                splitAccode = objGPack.GetSqlValue(strsql, , , Tran).Split(":")
                                accode = splitAccode(0)
                            Else
                                strsql = "SELECT S_SGSTID FROM " & cnAdminDb & "..CATEGORY "
                                strsql += " WHERE CATNAME = '" & roCategory!CATNAME.ToString & "'"
                                accode = objGPack.GetSqlValue(strsql, , , Tran)
                            End If
                            InsertIntoAccTran(tNo, tranMode, accode, SGSTAmt, 0, 0, 0, payMode, _Accode, txtInvoiceNo.Text, dtpTrandate.Value.ToString("yyyy-MM-dd"))
                        End If
                        If CGSTAmt > 0 Then
                            If True Then
                                Dim splitAccode() As String
                                strsql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID='LABOUR_CGST'"
                                splitAccode = objGPack.GetSqlValue(strsql, , , Tran).Split(":")
                                accode = splitAccode(0)
                            Else
                                strsql = "SELECT S_CGSTID FROM " & cnAdminDb & "..CATEGORY "
                                strsql += " WHERE CATNAME = '" & roCategory!CATNAME.ToString & "'"
                                accode = objGPack.GetSqlValue(strsql, , , Tran)
                            End If
                            InsertIntoAccTran(tNo, tranMode, accode, CGSTAmt, 0, 0, 0, payMode, _Accode, txtInvoiceNo.Text, dtpTrandate.Value.ToString("yyyy-MM-dd"))
                        End If
                        If IGSTAmt > 0 Then
                            If True Then
                                Dim splitAccode() As String
                                strsql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID='LABOUR_IGST'"
                                splitAccode = objGPack.GetSqlValue(strsql, , , Tran).Split(":")
                                accode = splitAccode(0)
                            Else
                                strsql = "SELECT S_IGSTID FROM " & cnAdminDb & "..CATEGORY "
                                strsql += " WHERE CATNAME = '" & roCategory!CATNAME.ToString & "'"
                                accode = objGPack.GetSqlValue(strsql, , , Tran)
                            End If
                            InsertIntoAccTran(tNo, tranMode, accode, IGSTAmt, 0, 0, 0, payMode, _Accode, txtInvoiceNo.Text, dtpTrandate.Value.ToString("yyyy-MM-dd"))
                        End If
                    End If

                End If
                ''INSERTING SALESTAX
                If vatAmt <> 0 Then
                    vatAmt = Math.Round(vatAmt, 2)
                    If roCategory!PAYMODE.ToString = "PURCHASE RETURN" Or roCategory!PAYMODE.ToString = "INTERNAL TRANSFER" Then 'cmbTrantype.Text.ToUpper = "PURCHASE RETURN" Or cmbTrantype.Text = "INTERNAL TRANSFER" 'TDS
                        tranMode = "C"
                        If GST Then
                            payMode = "TI"
                            If SGSTAmt > 0 Then
                                strsql = "SELECT P_SGSTID FROM " & cnAdminDb & "..CATEGORY "
                                strsql += " WHERE CATNAME = '" & roCategory!CATNAME.ToString & "'"
                                accode = objGPack.GetSqlValue(strsql, , , Tran)
                                InsertIntoAccTran(tNo, tranMode, accode, SGSTAmt, 0, 0, 0, payMode, _Accode, txtInvoiceNo.Text, dtpTrandate.Value.ToString("yyyy-MM-dd"))
                            End If
                            If CGSTAmt > 0 Then
                                strsql = "SELECT P_CGSTID FROM " & cnAdminDb & "..CATEGORY "
                                strsql += " WHERE CATNAME = '" & roCategory!CATNAME.ToString & "'"
                                accode = objGPack.GetSqlValue(strsql, , , Tran)
                                InsertIntoAccTran(tNo, tranMode, accode, CGSTAmt, 0, 0, 0, payMode, _Accode, txtInvoiceNo.Text, dtpTrandate.Value.ToString("yyyy-MM-dd"))
                            End If
                            If IGSTAmt > 0 Then
                                strsql = "SELECT P_IGSTID FROM " & cnAdminDb & "..CATEGORY "
                                strsql += " WHERE CATNAME = '" & roCategory!CATNAME.ToString & "'"
                                accode = objGPack.GetSqlValue(strsql, , , Tran)
                                InsertIntoAccTran(tNo, tranMode, accode, IGSTAmt, 0, 0, 0, payMode, _Accode, txtInvoiceNo.Text, dtpTrandate.Value.ToString("yyyy-MM-dd"))
                            End If
                        Else
                            strsql = "SELECT PTAXID FROM " & cnAdminDb & "..CATEGORY "
                            strsql += " WHERE CATNAME = '" & roCategory!CATNAME.ToString & "'"
                            accode = objGPack.GetSqlValue(strsql, , , Tran)
                            payMode = "TI"
                            InsertIntoAccTran(tNo, tranMode, accode, vatAmt, 0, 0, 0, payMode, _Accode, txtInvoiceNo.Text, dtpTrandate.Value.ToString("yyyy-MM-dd"))
                        End If
                        'InsertIntoAccTran(tNo, IIf(tranMode = "D", "C", "D"), _
                        '_Accode, _
                        'vatAmt, 0, 0, 0, payMode, accode)
                    Else
                        'tranMode = "D"
                        'If TTdsAccode <> "" Then
                        '    accode = TTdsAccode
                        'Else
                        '    accode = TdsAc
                        'End If
                        'payMode = "TI"
                        'InsertIntoAccTran(tNo, tranMode, accode, vatAmt, 0, 0, 0, payMode, _Accode, txtInvoiceNo.Text, dtpTrandate.Value.ToString("yyyy-MM-dd"), , , , Mid(TdsRemark, 1, 50))
                        'InsertIntoAccTran(tNo, IIf(tranMode = "D", "C", "D"),
                        '_Accode,
                        'vatAmt, 0, 0, 0, payMode, accode, txtInvoiceNo.Text, dtpTrandate.Value.ToString("yyyy-MM-dd"), , , , Mid(TdsRemark, 1, 50))
                    End If
                End If
            End If
        Next
        Dim PaymodeSup As String = "TR"
        If Val(txtConTDS_AMT.Text) > 0 Then
            If Val(txtSumReceipt.Text) > 0 Then
                PaymodeSup = "TR"
            ElseIf Val(txtSumIssue.Text) > 0 Then
                PaymodeSup = "TI"
            Else
                PaymodeSup = "TR"
            End If
            INSERTTDS(Val(txtConTDS_AMT.Text), TTdsAccode, _Accode, PaymodeSup, Val(txtConAmount_AMT.Text), Val(txtConTDSPER_AMT.Text), TTdsAccode, tNo, "")
        End If
        If Val(txtConTCS_AMT.Text) > 0 Then
            If Val(txtSumReceipt.Text) > 0 Then
                PaymodeSup = "TR"
            ElseIf Val(txtSumIssue.Text) > 0 Then
                PaymodeSup = "TI"
            Else
                PaymodeSup = "TR"
            End If
            INSERTTCS(Val(txtConTCS_AMT.Text), PaymodeSup, Val(txtConAmount_AMT.Text) + Val(txtConGST_AMT.Text), Val(txtConTCSPER_WET.Text), _Accode, tNo)
        End If
        If Val(txtSumNet.Text) <> 0 Then
            PaymodeSup = "TR"
            Dim Tranmode As String = "C"
            Dim _AcFinal As Double = Val(Val(txtSumNet.Text) + Val(txtRoundoff_AMT.Text))
            If Val(txtSumReceipt.Text) > 0 Then
                PaymodeSup = "TR"
                Tranmode = "C"
            ElseIf Val(txtSumIssue.Text) > 0 Then
                PaymodeSup = "TI"
                Tranmode = "D"
            Else
                PaymodeSup = "TR"
                Tranmode = "C"
            End If
            InsertIntoAccTran(tNo, Tranmode, _Accode, _AcFinal, 0, 0, 0, PaymodeSup, "", txtInvoiceNo.Text, dtpTrandate.Value.ToString("yyyy-MM-dd"))
        End If
        If Val(txtRoundoff_AMT.Text) <> 0 Then
            If Val(txtSumReceipt.Text) > 0 Then
                If Val(txtRoundoff_AMT.Text) < 0 Then
                    InsertIntoAccTran(tNo, "C", "RNDOFF", -1 * Val(txtRoundoff_AMT.Text), 0, 0, 0, "RO", "", txtInvoiceNo.Text, dtpTrandate.Value.ToString("yyyy-MM-dd"))
                Else
                    InsertIntoAccTran(tNo, "D", "RNDOFF", Val(txtRoundoff_AMT.Text), 0, 0, 0, "RO", "", txtInvoiceNo.Text, dtpTrandate.Value.ToString("yyyy-MM-dd"))
                End If
            ElseIf Val(txtSumIssue.Text) > 0 Then
                If Val(txtRoundoff_AMT.Text) < 0 Then
                    InsertIntoAccTran(tNo, "D", "RNDOFF", -1 * Val(txtRoundoff_AMT.Text), 0, 0, 0, "RO", "", txtInvoiceNo.Text, dtpTrandate.Value.ToString("yyyy-MM-dd"))
                Else
                    InsertIntoAccTran(tNo, "C", "RNDOFF", Val(txtRoundoff_AMT.Text), 0, 0, 0, "RO", "", txtInvoiceNo.Text, dtpTrandate.Value.ToString("yyyy-MM-dd"))
                End If
            Else
                If Val(txtRoundoff_AMT.Text) < 0 Then
                    InsertIntoAccTran(tNo, "C", "RNDOFF", -1 * Val(txtRoundoff_AMT.Text), 0, 0, 0, "RO", "", txtInvoiceNo.Text, dtpTrandate.Value.ToString("yyyy-MM-dd"))
                Else
                    InsertIntoAccTran(tNo, "D", "RNDOFF", Val(txtRoundoff_AMT.Text), 0, 0, 0, "RO", "", txtInvoiceNo.Text, dtpTrandate.Value.ToString("yyyy-MM-dd"))
                End If
            End If
        End If
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
    Optional ByVal IGST As Decimal = 0
    )
        If amount = 0 Then Exit Sub
        If Remark1 = Nothing Then Remark1 = txtRemark1.Text
        If Remark2 = Nothing Then Remark2 = txtRemark2.Text
        If refNo = Nothing Then refNo = txtInvoiceNo.Text
        If refDate = Nothing Then refDate = dtpTrandate.Value.ToString("yyyy-MM-dd")

        Dim Sno As String = GetNewSno(IIf(_AccAudit, TranSnoType.TACCTRANCODE, TranSnoType.ACCTRANCODE), Tran)
        strsql = " INSERT INTO " & cnStockDb & ".." & IIf(_AccAudit, "TACCTRAN", "ACCTRAN") & ""
        strsql += " ("
        strsql += " SNO,TRANNO,TRANDATE,TRANMODE,ACCODE"
        strsql += " ,AMOUNT,PCS,GRSWT,NETWT"
        strsql += " ,REFNO,REFDATE,PAYMODE,CHQCARDNO"
        strsql += " ,CARDID,CHQCARDREF,CHQDATE,BRSFLAG"
        strsql += " ,RELIASEDATE,FROMFLAG,REMARK1,REMARK2"
        strsql += " ,CONTRA,BATCHNO,USERID,UPDATED"
        strsql += " ,UPTIME,SYSTEMID,CASHID,COSTID,APPVER,COMPANYID"
        strsql += " ,TDSCATID,TDSPER,TDSAMOUNT"
        strsql += " )"
        strsql += " VALUES"
        strsql += " ("
        strsql += " '" & Sno & "'" ''SNO
        strsql += " ," & tNo & "" 'TRANNO 
        strsql += " ,'" & dtpTrandate.Value.Date.ToString("yyyy-MM-dd") & "'" 'TRANDATE
        strsql += " ,'" & tranMode & "'" 'TRANMODE
        strsql += " ,'" & accode & "'" 'ACCODE
        strsql += " ," & Math.Abs(amount) & "" 'AMOUNT
        strsql += " ," & Math.Abs(pcs) & "" 'PCS
        strsql += " ," & Math.Abs(grsWT) & "" 'GRSWT
        strsql += " ," & Math.Abs(netWT) & "" 'NETWT
        strsql += " ,'" & refNo & "'" 'REFNO
        If refDate = Nothing Then
            strsql += " ,NULL" 'REFDATE
        Else
            strsql += " ,'" & refDate & "'" 'REFDATE
        End If
        strsql += " ,'" & payMode & "'" 'PAYMODE
        strsql += " ,'" & chqCardNo & "'" 'CHQCARDNO
        strsql += " ," & chqCardId & "" 'CARDID
        strsql += " ,'" & chqCardRef & "'" 'CHQCARDREF
        If chqDate = Nothing Then
            strsql += " ,NULL" 'CHQDATE
        Else
            strsql += " ,'" & chqDate & "'" 'CHQDATE
        End If
        strsql += " ,''" 'BRSFLAG
        strsql += " ,NULL" 'RELIASEDATE
        strsql += " ,'S'" 'FROMFLAG
        strsql += " ,'" & Remark1 & "'" 'REMARK1
        strsql += " ,'" & Remark2 & "'" 'REMARK2
        strsql += " ,'" & contra & "'" 'CONTRA
        strsql += " ,'" & BatchNo & "'" 'BATCHNO
        strsql += " ,'" & userId & "'" 'USERID
        strsql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        strsql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
        strsql += " ,'" & systemId & "'" 'SYSTEMID
        strsql += " ,'" & _CashCtr.ToString & "'" 'CASHID
        strsql += " ,'" & CostCenterId & "'" 'COSTID
        strsql += " ,'" & VERSION & "'" 'APPVER
        strsql += " ,'" & strCompanyId & "'" 'COMPANYID
        strsql += " ," & TdsCatId & "" 'TDSCATID
        strsql += " ," & TdsPer & "" 'TDSPER
        strsql += " ," & TdsAmount & "" 'TDSAMOUNT
        strsql += " )"
        ExecQuery(SyncMode.Transaction, strsql, cn, Tran, CostCenterId)
        strsql = ""
        cmd = Nothing
        If EDAmt <> 0 Then
            strsql = " INSERT INTO " & cnStockDb & "..TAXTRAN"
            strsql += " ("
            strsql += " SNO,ACCODE,TRANNO,TRANDATE,TRANTYPE,BATCHNO,TAXID"
            strsql += " ,AMOUNT,TAXPER,TAXAMOUNT,TAXTYPE,TSNO,COSTID,COMPANYID"
            strsql += " )"
            strsql += " VALUES("
            strsql += " '" & Sno & "'" ''SNO
            strsql += " ,'" & contra & "'"
            strsql += " ," & TranNo & "" 'TRANNO
            strsql += " ,'" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
            strsql += " ,'TR'"
            strsql += " ,'" & BatchNo & "'" 'BATCHNO
            strsql += " ,'ED'" 'TAXID
            strsql += " ," & EDGrsAmt & "" 'AMOUNT
            strsql += " ," & EDPer & "" 'TAXPER
            strsql += " ," & EDAmt
            strsql += " ,'ED'"
            strsql += " ,1" 'TSNO
            strsql += " ,'" & CostCenterId & "'"
            strsql += " ,'" & strCompanyId & "'"
            strsql += " )"
            ExecQuery(SyncMode.Transaction, strsql, cn, Tran, CostCenterId)
        End If
    End Sub

    Private Sub InsertStoneDetails(ByVal IssSno As String _
 , ByVal TNO As Integer, ByVal stRow As DataRow, ByVal taxx As Decimal _
 , ByVal Trantype As String, ByVal Paymode As String)
        Dim stnItemId As Integer = 0
        Dim stnSubItemid As Integer = 0
        Dim stnCatCode As String = Nothing
        Dim vat As Double = Nothing
        Dim sno As String = Nothing
        If Trantype = "ISSUE" Then 'oMaterial = Material.Issue
            sno = GetNewSno(IIf(_AccAudit, TranSnoType.TISSSTONECODE, TranSnoType.ISSSTONECODE), Tran)
        Else
            sno = GetNewSno(IIf(_AccAudit, TranSnoType.TRECEIPTSTONECODE, TranSnoType.RECEIPTSTONECODE), Tran)
        End If
        ''Find stnCatCode
        strsql = " Select CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & stRow.Item("ITEM").ToString & "'"
        stnCatCode = objGPack.GetSqlValue(strsql, , , Tran)
        If taxx <> 0 Then
            strsql = " SELECT "
            If Trantype = "ISSUE" Then strsql += " SALESTAX" 'oMaterial = Material.Issue
            If Trantype = "RECEIPT" Then strsql += " PTAX " 'oMaterial = Material.Receipt
            strsql += " FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = '" & stnCatCode & "'"
            Dim vatPer As Double = Val(objGPack.GetSqlValue(strsql, , , Tran))
            'vatPer = IIf(vatPer = 0, 1, vatPer)
            vat = Val(stRow!AMOUNT.ToString) * (vatPer / 100)
        End If

        ''Find itemId
        strsql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & stRow.Item("ITEM").ToString & "'"
        stnItemId = Val(objGPack.GetSqlValue(strsql, , , Tran))

        ''Find subItemId
        strsql = " SELECT ISNULL(SUBITEMID,0)AS SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & stRow.Item("SUBITEM").ToString & "' AND ITEMID = " & stnItemId & ""
        stnSubItemid = Val(objGPack.GetSqlValue(strsql, , , Tran))

        If Trantype = "ISSUE" Then 'oMaterial = Material.Issue
            strsql = " INSERT INTO " & cnStockDb & ".." & IIf(_AccAudit, "TISSSTONE", "ISSSTONE")
        Else
            strsql = " INSERT INTO " & cnStockDb & ".." & IIf(_AccAudit, "TRECEIPTSTONE", "RECEIPTSTONE")
        End If
        strsql += " ("
        strsql += " SNO,ISSSNO,TRANNO,TRANDATE,TRANTYPE"
        strsql += " ,STNPCS,STNWT,STNRATE,STNAMT"
        strsql += " ,STNITEMID,STNSUBITEMID,CALCMODE,STONEUNIT"
        strsql += " ,STONEMODE,TRANSTATUS,COSTID,COMPANYID"
        strsql += " ,BATCHNO,SYSTEMID,CATCODE,TAX,APPVER,DISCOUNT"
        'If oMaterial = Material.Receipt Then StrSql += " ,JOBISNO"
        strsql += " ,JOBISNO"
        'If oMaterial = Material.Issue Then strsql += " ,RESNO"
        If Trantype = "ISSUE" Then strsql += " ,RESNO"
        strsql += ",OCATCODE,SEIVE,CUTID,COLORID,CLARITYID,SHAPEID,SETTYPEID,HEIGHT,WIDTH,STUDDEDUCT"
        strsql += " )"
        strsql += " VALUES"
        strsql += " ("
        strsql += " '" & sno & "'" ''SNO
        strsql += " ,'" & IssSno & "'" 'ISSSNO
        strsql += " ," & TNO & "" 'TRANNO
        strsql += " ,'" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
        If Trantype = "RECEIPT" Then 'oMaterial = Material.Receipt
            ' strsql += " ,'R" & Mid(stRow.Item("STNTYPE").ToString, 1, 2) & "'" 'TRANTYPE Newly Add
            strsql += " ,'" & IIf(Trantype = "ISSUE", "I", "R") & "" & Mid(Paymode, 1, 2) & "'" 'TRANTYPE
        Else
            'strsql += " ,'" & IIf(oMaterial = Material.Issue, "I", "R") & "" & Mid(cmbTrantype.Text, 1, 2) & "'" 'TRANTYPE
            strsql += " ,'" & IIf(Trantype = "ISSUE", "I", "R") & "" & Mid(Paymode, 1, 2) & "'" 'TRANTYPE
        End If
        strsql += " ," & Val(stRow.Item("PCS").ToString) & "" 'STNPCS
        strsql += " ," & Val(stRow.Item("WEIGHT").ToString) & "" 'STNWT
        strsql += " ," & Val(stRow.Item("RATE").ToString) & "" 'STNRATE
        strsql += " ," & Val(stRow.Item("AMOUNT").ToString) & "" 'STNAMT
        strsql += " ," & stnItemId & "" 'STNITEMID
        strsql += " ," & stnSubItemid & "" 'STNSUBITEMID
        strsql += " ,'" & Mid(stRow.Item("CALC").ToString, 1, 1) & "'" 'CALCMODE
        strsql += " ,'" & Mid(stRow.Item("UNIT").ToString, 1, 1) & "'" 'STONEUNIT
        strsql += " ,''" 'STONEMODE 
        strsql += " ,''" 'TRANSTATUS
        strsql += " ,'" & CostCenterId & "'" 'COSTID
        strsql += " ,'" & strCompanyId & "'" 'COMPANYID
        strsql += " ,'" & BatchNo & "'" 'BATCHNO
        strsql += " ,'" & systemId & "'" 'SYSTEMID
        strsql += " ,'" & stnCatCode & "'" 'CATCODE
        strsql += " ," & vat & "" 'TAX
        strsql += " ,'" & VERSION & "'" 'APPVER
        strsql += " ," & Val(stRow.Item("DISCOUNT").ToString) & "" 'DISCOUNT
        'If oMaterial = Material.Receipt Then StrSql += " ,'" & stRow.Item("ISSSNO").ToString & "'" 'JOBISNO
        strsql += " ,'" & stRow.Item("ISSSNO").ToString & "'" 'JOBISNO
        'If oMaterial = Material.Issue Then strsql += " ,'" & stRow.Item("RESNO").ToString & "'" 'RESNO
        If Trantype = "ISSUE" Then
            strsql += " ,'" & stRow.Item("RESNO").ToString & "'" 'RESNO
        End If
        strsql += " ,'" & stRow.Item("OCATCODE").ToString & "'" 'OCATCODE
        strsql += " ,'" & stRow.Item("SEIVE").ToString & "'" 'SEIVE

        strsql += " ," & Val(stRow.Item("CUTID").ToString) & " " 'CUTID
        strsql += " ," & Val(stRow.Item("COLORID").ToString) & "" 'COLORID
        strsql += " ," & Val(stRow.Item("CLARITYID").ToString) & "" 'CLARITYID
        strsql += " ," & Val(stRow.Item("SHAPEID").ToString) & "" 'SHAPEID
        strsql += " ," & Val(stRow.Item("SETTYPEID").ToString) & "" 'SETTYPEID
        strsql += " ,'" & Val(stRow.Item("HEIGHT").ToString) & "'" 'HEIGHT
        strsql += " ,'" & Val(stRow.Item("WIDTH").ToString) & "'" 'WIDTH
        If stRow.Item("STUDDEDUCT").ToString <> "" Then
            strsql += " ,'" & Mid(stRow.Item("STUDDEDUCT").ToString, 1, 1) & "'" 'STUDDEDUCT
        Else
            strsql += " ,''" 'STUDDEDUCT
        End If
        strsql += " )"
        ExecQuery(SyncMode.Transaction, strsql, cn, Tran, CostCenterId)
    End Sub
#End Region

#Region " Combox Events"

    Private Sub txtPurityper_AMT_TextChanged(sender As Object, e As EventArgs) Handles txtPurityper_RATE.TextChanged
        CalcONetWt()
        CalcOWastage()
        CalcOMcPercentage()
        CalcOPureWt()
        CalcOGrossAmt()
    End Sub
    Private Sub funcReadonlyPurchasereturn()
        If cmbTrantype.Text = "" Or cmbTrantype.Text = "" Then
            txtwastageper_AMT.Text = ""
            txtMCPPer_RATE.Text = ""
            txtMCPerGram_RATE.Text = ""
            txtMC_AMT.Text = ""
            txtStoneValue_AMT.Text = ""
            txtAddition_AMT.Text = ""
            txtGrossAmt_AMT.Text = ""
            txtCGSTVAL_AMT.Text = ""
            txtSGSTVAL_AMT.Text = ""
            txtIGSTVAL_AMT.Text = ""
            txtNetVal_AMT.Text = ""
            lblCGSTPer.Text = "..."
            lblSGSTPer.Text = "..."
            lblIGSTPer.Text = "..."
        End If
    End Sub
    Private Sub txtPurityper_AMT_KeyDown(sender As Object, e As KeyEventArgs) Handles txtPurityper_RATE.KeyDown
        If e.KeyCode = Keys.Enter Then
            If cmbTrantype.Text = "PURCHASE RETURN" Or cmbTrantype.Text = "ISSUE" Then
            Else
                'Dim MValue As Double = funcgetDealervalue("PUREWT")
                'If Val(txtPurityper_AMT.Text) > MValue Then
                '    If MValue > 0 Then
                '        txtPurityper_AMT.Text = MValue
                '        MsgBox("Exceed not allowed")
                '    End If
                'End If
            End If
            CalcONetWt()
            CalcOWastage()
            CalcOMcPercentage()
            CalcOPureWt()
            CalcOGrossAmt()
            CalcOsumofTouch_purityWastagemc()
            funcReadonlyPurchasereturn()
        End If
    End Sub
    Private Sub cmbAcname_OWN_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAcname_OWN.GotFocus
        cmbAcname_OWN.BackColor = Color.LightGreen
    End Sub
    Private Sub cmbAcname_OWN_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAcname_OWN.Leave
        If cmbAcname_OWN.SelectedValue Is Nothing Then
            '''MsgBox("Select Acname", MsgBoxStyle.Information)
            'cmbAcname_OWN.Focus()
            'cmbAcname_OWN.SelectAll()
            Exit Sub
        End If
        If cmbAcname_OWN.Text = "" Then
            ''MsgBox("Select Acname", MsgBoxStyle.Information)
            'cmbAcname_OWN.Focus()
            'cmbAcname_OWN.SelectAll()
            Exit Sub
        End If
        GetSupplierName(cmbAcname_OWN.SelectedValue.ToString)
        LoadBalanceWt(cmbAcname_OWN.SelectedValue.ToString, "RECEIPT")
        funcOnAccountvalid()
    End Sub

    Private Sub cmbAcname_OWN_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAcname_OWN.LostFocus
        cmbAcname_OWN.BackColor = Color.White
    End Sub
    Private Sub cmbAcname_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAcname_OWN.SelectedIndexChanged
        If cmbAcname_OWN.SelectedValue Is Nothing Then
            Exit Sub
        End If
        If cmbAcname_OWN.SelectedValue.ToString = "" Or cmbAcname_OWN.SelectedValue.ToString = "System.Data.DataRowView" Then
            Exit Sub
        End If
        GetSupplierName(cmbAcname_OWN.SelectedValue.ToString)
        LoadBalanceWt(cmbAcname_OWN.SelectedValue.ToString, "RECEIPT")
        funcOnAccountvalid()
    End Sub

    'Private Sub cmbInvoiceType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbInvoiceType.GotFocus
    '    LoadTrantype()
    'End Sub
    'Private Sub cmbInvoiceType_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbInvoiceType.KeyDown
    '    If e.KeyCode = Keys.Enter Then
    '        LoadTrantype()
    '    End If
    'End Sub
    'Private Sub cmbInvoiceType_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbInvoiceType.Leave
    '    LoadTrantype()
    'End Sub
    'Private Sub cmbInvoiceType_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbInvoiceType.LostFocus
    '    LoadTrantype()
    'End Sub
    'Private Sub cmbInvoiceType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbInvoiceType.SelectedIndexChanged
    '    If cmbInvoiceType.SelectedValue Is Nothing Then
    '        Exit Sub
    '    End If
    '    LoadTrantype()
    'End Sub

    Private Function funEstButtonVisible() As Boolean
        If cmbTrantype.Text = "PURCHASE RETURN" Or cmbTrantype.Text = "ISSUE" Then
            btnReturnEst.Visible = True
            Return True
        Else
            btnReturnEst.Visible = False
            Return False
        End If
    End Function

    Private Sub uploadExcelPurchaseVisible()
        If cmbTrantype.Text = "PURCHASE" Or cmbTrantype.Text = "PURCHASE RETURN" Then
            btnUpload.Visible = True
        Else
            btnUpload.Visible = False
        End If
        If cmbTrantype.Text = "PURCHASE" Or cmbTrantype.Text = "" Then
            btnCD.Visible = True
        Else
            btnCD.Visible = False
        End If
    End Sub

    Private Sub cmbTrantype_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbTrantype.SelectedIndexChanged
        If cmbTrantype.Text = "System.Data.DataRowView" Then Exit Sub
        If cmbTrantype.Text = "" Then Exit Sub
        If cmbAcname_OWN.SelectedValue.ToString = "" Then Exit Sub
        If cmbTrantype.Text = "RECEIPT" Then
            LoadBalanceWt(cmbAcname_OWN.SelectedValue.ToString, "RECEIPT")
        ElseIf cmbTrantype.Text = "ISSUE" Then
            LoadBalanceWt(cmbAcname_OWN.SelectedValue.ToString, "ISSUE")
        Else
        End If
        funEstButtonVisible()
    End Sub

    Private Sub cmbTrantype_GotFocus(sender As Object, e As EventArgs) Handles cmbTrantype.GotFocus
        If cmbAcnameAcname_Own.SelectedValue Is Nothing Then
            Exit Sub
        End If
        If cmbAcnameAcname_Own.SelectedValue.ToString = "" Then
            Exit Sub
        End If
        If cmbTrantype.Text = "RECEIPT" Then
            LoadBalanceWt(cmbAcname_OWN.SelectedValue.ToString, "RECEIPT")
        ElseIf cmbTrantype.Text = "ISSUE" Then
            LoadBalanceWt(cmbAcname_OWN.SelectedValue.ToString, "ISSUE")
        Else
        End If
    End Sub

    Private Sub cmbTrantype_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbTrantype.Leave
        If cmbTrantype.Text = "" Then
            cmbTrantype.Focus()
            cmbTrantype.SelectAll()
        End If
        funcLastTransaction(cmbTrantype.Text, cmbAcname_OWN.SelectedValue.ToString)
        funcMcAmtReadonly()
        uploadExcelPurchaseVisible()
        GetSupplierName(cmbAcname_OWN.SelectedValue.ToString)
        If cmbTrantype.Text = "RECEIPT" Then
            LoadBalanceWt(cmbAcname_OWN.SelectedValue.ToString, "RECEIPT")
        ElseIf cmbTrantype.Text = "ISSUE" Then
            LoadBalanceWt(cmbAcname_OWN.SelectedValue.ToString, "ISSUE")
        Else
        End If
        funEstButtonVisible()
    End Sub

    Private Sub cmbTrantype_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbTrantype.KeyDown
        If e.KeyCode = Keys.Enter Then
            If cmbTrantype.Text = "" Then
                cmbTrantype.Focus()
                cmbTrantype.SelectAll()
            End If
            funcLastTransaction(cmbTrantype.Text, cmbAcname_OWN.SelectedValue.ToString)
            funcMcAmtReadonly()
            uploadExcelPurchaseVisible()
            GetSupplierName(cmbAcname_OWN.SelectedValue.ToString)
            If cmbTrantype.Text = "RECEIPT" Then
                LoadBalanceWt(cmbAcname_OWN.SelectedValue.ToString, "RECEIPT")
            ElseIf cmbTrantype.Text = "ISSUE" Then
                LoadBalanceWt(cmbAcname_OWN.SelectedValue.ToString, "ISSUE")
            Else
            End If
            funEstButtonVisible()
        ElseIf e.KeyCode = Keys.Escape Then
            btnSave.Focus()
        End If
    End Sub

    Private Sub cmbItem_OWN_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbItem_OWN.GotFocus
        cmbItem_OWN.BackColor = Color.LightGreen
    End Sub

    Private Sub cmbItem_OWN_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbItem_OWN.LostFocus
        cmbItem_OWN.BackColor = Color.White
    End Sub

    Private Sub cmbItem_OWN_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbItem_OWN.Leave
        If cmbItem_OWN.SelectedValue Is Nothing Then
            Exit Sub
        End If
        txtItemId_NUM.Text = Val(cmbItem_OWN.SelectedValue.ToString)
        If Val(txtItemId_NUM.Text) = 0 Then
            txtItemId_NUM.Text = ""
        End If
        LoadSubItemName(Val(cmbItem_OWN.SelectedValue.ToString))
        txtRate_RATE.Text = Format(funcBoardRate(Val(cmbItem_OWN.SelectedValue.ToString)), "0.0000")
        If cmbTrantype.SelectedValue Is Nothing Then
            objGSTTDS = New frmGSTGSTTDS(False, Val(cmbItem_OWN.SelectedValue.ToString), cmbAcname_OWN.SelectedValue.ToString, "", 0)
        Else
            objGSTTDS = New frmGSTGSTTDS(False, Val(cmbItem_OWN.SelectedValue.ToString), cmbAcname_OWN.SelectedValue.ToString, cmbTrantype.Text, 0)
        End If
        CalcOGSTPER()
    End Sub

    Private Sub cmbItem_OWN_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbItem_OWN.SelectedIndexChanged
        If cmbItem_OWN.SelectedValue Is Nothing Then
            Exit Sub
        End If
        If cmbItem_OWN.SelectedValue.ToString = "" Or cmbItem_OWN.SelectedValue.ToString = "System.Data.DataRowView" Then
            Exit Sub
        End If
        txtItemId_NUM.Text = Val(cmbItem_OWN.SelectedValue.ToString)
        If Val(txtItemId_NUM.Text) = 0 Then
            txtItemId_NUM.Text = ""
        End If
        LoadSubItemName(Val(cmbItem_OWN.SelectedValue.ToString))
        txtRate_RATE.Text = Format(funcBoardRate(Val(cmbItem_OWN.SelectedValue.ToString)), "0.0000")
        If cmbTrantype.SelectedValue Is Nothing Then
            objGSTTDS = New frmGSTGSTTDS(False, Val(cmbItem_OWN.SelectedValue.ToString), cmbAcname_OWN.SelectedValue.ToString, "", 0)
        Else
            objGSTTDS = New frmGSTGSTTDS(False, Val(cmbItem_OWN.SelectedValue.ToString), cmbAcname_OWN.SelectedValue.ToString, cmbTrantype.Text, 0)
        End If
        CalcOGSTPER()
    End Sub

    Private Sub cmbSubItem_OWN_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSubItem_OWN.GotFocus
        cmbSubItem_OWN.BackColor = Color.LightGreen
    End Sub

    Private Sub cmbSubItem_OWN_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSubItem_OWN.LostFocus
        cmbSubItem_OWN.BackColor = Color.White
    End Sub
    Private Function funcGetRatefromdealermc(ByVal itemid As Integer, ByVal subitemid As Integer, ByVal acname As String) As Double
        strsql = " SELECT PRATE FROM " & cnAdminDb & "..DEALER_WMCTABLE "
        strsql += vbCrLf + " WHERE ISNULL(PRATE,0) > 0 "
        strsql += vbCrLf + " And ITEMID = " & itemid & "  "
        strsql += vbCrLf + " AND ACCODE = '" & acname & "'"
        If subitemid > 0 Then
            strsql += vbCrLf + " And SUBITEMID = " & subitemid & ""
        End If
        Return Val(objGPack.GetSqlValue(strsql).ToString)
    End Function
    Private Sub cmbSubItem_OWN_KeyDown(sender As Object, e As KeyEventArgs) Handles cmbSubItem_OWN.KeyDown
        If e.KeyCode = Keys.Enter Then
            If cmbSubItem_OWN.SelectedValue Is Nothing Then
                Exit Sub
            End If
            funcSubitemmastValidate()
            If funcGetRatefromdealermc(cmbItem_OWN.SelectedValue.ToString, cmbSubItem_OWN.SelectedValue.ToString, cmbAcname_OWN.SelectedValue.ToString) > 0 Then
                'Temporary Stop 17-Feb-2021
                'txtRate_RATE.Text = Format(funcGetRatefromdealermc(cmbItem_OWN.SelectedValue.ToString, cmbSubItem_OWN.SelectedValue.ToString, cmbAcname_OWN.SelectedValue.ToString), "0.00")
            End If
        End If
    End Sub
    Private Sub cmbSubItem_OWN_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSubItem_OWN.SelectedIndexChanged
        If cmbSubItem_OWN.SelectedValue Is Nothing Then
            Exit Sub
        End If
        ' funcSubitemmastValidate()
    End Sub
#End Region

#Region " Textbox Event"

#Region " TDS Event"
    Private Sub funcNetValueTcs()
        txtNetVal_AMT.Text = Val(txtGrossAmt_AMT.Text) + Val(txtCGSTVAL_AMT.Text) + Val(txtSGSTVAL_AMT.Text) + Val(txtIGSTVAL_AMT.Text)
    End Sub
    Private Sub txtTdsVal_AMT_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs)
        CalcONetWt()
        CalcOWastage()
        CalcOMcPercentage()
        CalcOPureWt()
        CalcOGrossAmt()
    End Sub
#End Region

#Region "Making Charge Event"
    Private Sub funcclearMcTextbox()
        txtMCPPer_RATE.Text = 0
        txtMCPerGram_RATE.Text = 0
        txtMCPieces_AMT.Text = 0
    End Sub
    Private Sub txtMC_AMT_TextChanged(sender As Object, e As EventArgs) Handles txtMC_AMT.TextChanged
        If txtMC_AMT.ReadOnly = False Then
            If Val(txtMC_AMT.Text) > 0 Then
                'funcclearMcTextbox()
            End If
        End If
    End Sub
    Private Sub txtMC_AMT_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtMC_AMT.KeyDown
        If e.KeyCode = Keys.Enter Then
            CalcONetWt()
            CalcOWastage()
            CalcOMcPercentage()
            CalcOPureWt()
            CalcOGrossAmt()
            funcReadonlyPurchasereturn()
        End If
    End Sub

    Private Sub txtMC_AMT_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMC_AMT.Leave
        CalcONetWt()
        CalcOWastage()
        CalcOMcPercentage()
        CalcOPureWt()
        CalcOGrossAmt()
        funcReadonlyPurchasereturn()
    End Sub

    Private Sub txtMCPPer_AMT_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMCPPer_RATE.Leave
        CalcOMcPercentage()
    End Sub

    Private Sub txtMCPPer_AMT_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMCPPer_RATE.TextChanged
        If Val(txtMCPPer_RATE.Text) > 0 Then
            txtMCPerGram_RATE.Text = 0
        End If
    End Sub

    Private Sub txtMCPPer_AMT_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtMCPPer_RATE.KeyDown
        If e.KeyCode = Keys.Enter Then
            'Dim MValue As Double = funcgetDealervalue("MCPER")
            'If Val(txtMCPPer_AMT.Text) > MValue Then
            '    If MValue > 0 Then
            '        txtMCPPer_AMT.Text = MValue
            '        MsgBox("Exceed not allowed")
            '    End If
            'End If
            CalcOMcPercentage()
            CalcOsumofTouch_purityWastagemc()
            funcReadonlyPurchasereturn()
        End If
    End Sub

    Private Sub txtMCPerGram_AMT_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtMCPerGram_RATE.KeyDown
        If e.KeyCode = Keys.Enter Then
            CalcOMcPercentage()
            funcReadonlyPurchasereturn()
        End If
    End Sub

    Private Sub txtMCPerGram_AMT_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMCPerGram_RATE.Leave
        CalcOMcPercentage()
        funcReadonlyPurchasereturn()
    End Sub

    Private Sub txtMCPerGram_AMT_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMCPerGram_RATE.TextChanged
        If Val(txtMCPerGram_RATE.Text) > 0 Then
            'txtMCPPer_AMT.Text = 0
        End If
    End Sub

    Private Sub txtMCPieces_AMT_KeyDown(sender As Object, e As KeyEventArgs) Handles txtMCPieces_AMT.KeyDown
        If e.KeyCode = Keys.Enter Then
            CalcOMcPercentage()
        End If
    End Sub

    Private Sub txtMCPieces_AMT_Leave(sender As Object, e As EventArgs) Handles txtMCPieces_AMT.Leave
        CalcOMcPercentage()
    End Sub

#End Region

#Region " Purity Event"
    Private Sub txtPurityWt_WET_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPurityWt_WET.Leave
        CalcOPureWt()
    End Sub
    Private Sub txtPurityWt_WET_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPurityWt_WET.TextChanged
        CalcOPureWt()
    End Sub
#End Region

#Region " Rate Event"

    Private Function ItemCaltype(ByVal itemid As Integer, ByVal subitemId As Integer) As String
        Dim calType As String = ""
        strsql = " SELECT CALTYPE FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID= '" & itemid & "' AND SUBITEMID='" & subitemId & "'"
        calType = objGPack.GetSqlValue(strsql).ToString
        If calType = "" Then
            strsql = " SELECT CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID= '" & itemid & "' "
            calType = objGPack.GetSqlValue(strsql).ToString
            Return calType
        End If
        Return calType
    End Function

    Private Sub txtRate_AMT_GotFocus(sender As Object, e As EventArgs) Handles txtRate_RATE.GotFocus
        txtRate_RATE.ReadOnly = True
        If ItemCaltype(cmbItem_OWN.SelectedValue.ToString, cmbSubItem_OWN.SelectedValue.ToString) = "R" Or ItemCaltype(cmbItem_OWN.SelectedValue.ToString, cmbSubItem_OWN.SelectedValue.ToString) = "M" Or ItemCaltype(cmbItem_OWN.SelectedValue.ToString, cmbSubItem_OWN.SelectedValue.ToString) = "P" Then
            txtRate_RATE.ReadOnly = False
        Else
            txtRate_RATE.ReadOnly = True
        End If
    End Sub
    Private Sub txtRate_AMT_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtRate_RATE.KeyDown
        If e.KeyCode = Keys.Enter Then
            CalcONetWt()
        End If
    End Sub

    Private Sub txtRate_AMT_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRate_RATE.Leave
        CalcONetWt()
    End Sub

    Private Sub txtRate_AMT_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtRate_RATE.TextChanged
        CalcONetWt()
    End Sub
#End Region

#Region " Touch Event"
    Private Sub txtTouchper_AMT_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtTouchper_AMT.KeyDown
        If e.KeyCode = Keys.Enter Then
            Dim MTouch As Double = funcgetDealervalue("TOUCH")
            If Val(txtTouchper_AMT.Text) > MTouch Then
                If MTouch > 0 Then
                    txtTouchper_AMT.Text = MTouch
                    MsgBox("Exceed not allowed")
                End If
            End If
            CalcOEMCPERG()
            CalcOPureWt()
            funcReadonlyPurchasereturn()
        End If
    End Sub

    Private Sub txtTouchper_AMT_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTouchper_AMT.TextChanged
        CalcOEMCPERG()
        CalcOPureWt()
    End Sub
#End Region

#Region " WastagePercentage Event"
    Private Sub txtwastageper_AMT_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtwastageper_AMT.KeyDown
        If e.KeyCode = Keys.Enter Then
            'Dim MValue As Double = funcgetDealervalue("WASTPER")
            'If Val(txtwastageper_AMT.Text) > MValue Then
            '    If MValue > 0 Then
            '        txtwastageper_AMT.Text = MValue
            '        MsgBox("Exceed not allowed")
            '    End If
            'End If
            CalcOWastage()
            CalcOsumofTouch_purityWastagemc()
        End If
    End Sub

    Private Sub txtwastageper_AMT_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtwastageper_AMT.Leave
        CalcOWastage()
    End Sub

    Private Sub txtwastageper_AMT_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtwastageper_AMT.TextChanged
        CalcOWastage()
    End Sub
#End Region

#Region " Wastage Event Amount"
    Private Sub txtwastage_AMT_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtwastage_AMT.Leave
        CalcOWastage()
        CalcOPureWt()
        CalcOGrossAmt()
    End Sub

    Private Sub txtwastage_AMT_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtwastage_AMT.TextChanged
        CalcOWastage()
        CalcOPureWt()
        CalcOGrossAmt()
    End Sub
#End Region

#Region " Pcs Event"
    Private Sub txtPcs_NUM_KeyDown(sender As Object, e As KeyEventArgs) Handles txtPcs_NUM.KeyDown
        If e.KeyCode = Keys.Enter Then
            If Val(txtPcs_NUM.Text) = 0 Then
                MsgBox("Pcs should not empty", MsgBoxStyle.Information)
                txtPcs_NUM.Focus()
                txtPcs_NUM.SelectAll()
            End If
        End If
    End Sub
    Private Sub txtPcs_NUM_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPcs_NUM.GotFocus
        Getdealerwmc()
    End Sub

    Private Sub txtPcs_NUM_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPcs_NUM.Leave
        Getdealerwmc()
    End Sub
#End Region

#Region " Gross Event"

    Private Sub txtGrsWt_WET_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtGrsWt_WET.LostFocus
        Getdealerwmcwtrange()
    End Sub

    Private Sub txtGrsWt_WET_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtGrsWt_WET.KeyPress

    End Sub

    Private Sub txtGrsWt_WET_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtGrsWt_WET.Leave
        If Val(txtGrsWt_WET.Text) = 0 And Val(txtPcs_NUM.Text) = 0 Then
            txtGrsWt_WET.Focus()
            Exit Sub
        End If
        Getdealerwmcwtrange()
        funcReadonlyPurchasereturn()
    End Sub

    Private Sub CalcONetWt(ByVal sender As Object, ByVal e As System.EventArgs) Handles _
   txtLessWt_WET.TextChanged _
   , txtGrsWt_WET.TextChanged
        CalcONetWt()
        CalcOWastage()
        CalcOMcPercentage()
        CalcOPureWt()
        CalcOGrossAmt()
    End Sub
#End Region

#Region " Textbox Event"
    Private Sub txtInvoiceNo_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtInvoiceNo.KeyDown
        If e.KeyCode = Keys.Enter Then
            If txtInvoiceNo.Text.Trim = "" Then
                ' MsgBox("Enter the Invoice No", MsgBoxStyle.Information)
            End If
        End If
    End Sub
#End Region

#Region " Item Event"
    Private Sub txtItemId_NUM_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtItemId_NUM.Leave
        GetItemName(Val(txtItemId_NUM.Text))
    End Sub

    Private Sub txtItemId_NUM_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtItemId_NUM.KeyDown
        If e.KeyCode = Keys.Enter Then
            GetItemName(Val(txtItemId_NUM.Text))
        End If
    End Sub
#End Region

#Region " Stone Events"
    Private Sub txtStoneValue_AMT_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtStoneValue_AMT.TextChanged
        CalcOGrossAmt()
    End Sub

    Private Sub txtStoneValue_AMT_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtStoneValue_AMT.KeyDown
        If e.KeyCode = Keys.Enter Then
            CalcOGrossAmt()
        End If
    End Sub

    Private Sub txtStoneValue_AMT_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtStoneValue_AMT.Leave
        CalcOGrossAmt()
        funcReadonlyPurchasereturn()
    End Sub
#End Region

#Region " CGST Events"

    Private Sub txtCGSTVAL_AMT_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtCGSTVAL_AMT.KeyDown
        If e.KeyCode = Keys.Enter Then
            CalcONetWt()
            CalcOWastage()
            CalcOMcPercentage()
            CalcOPureWt()
            CalcOGrossAmt()
        End If
    End Sub

    Private Sub txtCGSTVAL_AMT_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCGSTVAL_AMT.Leave
        CalcONetWt()
        CalcOWastage()
        CalcOMcPercentage()
        CalcOPureWt()
        CalcOGrossAmt()
    End Sub

    Private Sub txtCGSTVAL_AMT_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCGSTVAL_AMT.TextChanged
        CalcONetWt()
        CalcOWastage()
        CalcOMcPercentage()
        CalcOPureWt()
        CalcOGrossAmt()
    End Sub
#End Region

#Region " SGST Event"

    Private Sub txtWastGST_AMT_KeyDown(sender As Object, e As KeyEventArgs)
        If e.KeyCode = Keys.Enter Then
            CalcONetWt()
            CalcOWastage()
            CalcOMcPercentage()
            CalcOPureWt()
            CalcOGrossAmt()
        End If
    End Sub

    Private Sub txtWastGST_AMT_Leave(sender As Object, e As EventArgs)
        CalcONetWt()
        CalcOWastage()
        CalcOMcPercentage()
        CalcOPureWt()
        CalcOGrossAmt()
    End Sub

    Private Sub txtWastGST_AMT_TextChanged(sender As Object, e As EventArgs)
        CalcONetWt()
        CalcOWastage()
        CalcOMcPercentage()
        CalcOPureWt()
        CalcOGrossAmt()
    End Sub
    Private Sub txtSGSTVAL_AMT_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtSGSTVAL_AMT.KeyDown
        If e.KeyCode = Keys.Enter Then
            CalcONetWt()
            CalcOWastage()
            CalcOMcPercentage()
            CalcOPureWt()
            CalcOGrossAmt()
        End If
    End Sub

    Private Sub txtSGSTVAL_AMT_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSGSTVAL_AMT.Leave
        CalcONetWt()
        CalcOWastage()
        CalcOMcPercentage()
        CalcOPureWt()
        CalcOGrossAmt()
    End Sub

    Private Sub txtSGSTVAL_AMT_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSGSTVAL_AMT.TextChanged
        CalcONetWt()
        CalcOWastage()
        CalcOMcPercentage()
        CalcOPureWt()
        CalcOGrossAmt()
    End Sub
#End Region

#Region " IGST Event"
    Private Sub txtIGSTVAL_AMT_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtIGSTVAL_AMT.KeyDown
        If e.KeyCode = Keys.Enter Then
            CalcONetWt()
            CalcOWastage()
            CalcOMcPercentage()
            CalcOPureWt()
            CalcOGrossAmt()
        End If
    End Sub

    Private Sub txtIGSTVAL_AMT_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtIGSTVAL_AMT.Leave
        CalcONetWt()
        CalcOWastage()
        CalcOMcPercentage()
        CalcOPureWt()
        CalcOGrossAmt()
    End Sub

    Private Sub txtIGSTVAL_AMT_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtIGSTVAL_AMT.TextChanged
        CalcONetWt()
        CalcOWastage()
        CalcOMcPercentage()
        CalcOPureWt()
        CalcOGrossAmt()
    End Sub
#End Region

#Region " Gross Event"
    Private Sub txtGrossAmt_AMT_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtGrossAmt_AMT.KeyDown
        If e.KeyCode = Keys.Enter Then
            CalcONetWt()
            CalcOWastage()
            CalcOMcPercentage()
            CalcOPureWt()
            CalcOGrossAmt()
        End If
    End Sub

    Private Sub txtGrossAmt_AMT_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtGrossAmt_AMT.Leave
        CalcONetWt()
        CalcOWastage()
        CalcOMcPercentage()
        CalcOPureWt()
        CalcOGrossAmt()
    End Sub

    Private Sub txtGrossAmt_AMT_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtGrossAmt_AMT.TextChanged
        CalcONetWt()
        CalcOWastage()
        CalcOMcPercentage()
        CalcOPureWt()
        CalcOGrossAmt()
    End Sub
#End Region

#Region " Datatable Stone Event"
    Private Sub funcDtstone()
        If dtStone.Rows.Count > 0 Then
            If lblEditKeyNo.Text = "" Then
            Else
                Dim dvUpdStn As New DataView(dtStone)
                Dim Dtup As New DataTable
                dvUpdStn.RowFilter = "KEYNO <> '" & Val(lblEditKeyNo.Text) + 1 & "'"
                Dtup = New DataTable
                Dtup = dvUpdStn.ToTable()
                dtStone = New DataTable
                dtStone = Dtup
            End If
        End If
        If Not objStone Is Nothing Then
            If objStone.dtGridStone.Rows.Count > 0 Then
                If dtStone Is Nothing Then
                    dtStone = objStone.dtGridStone.Clone
                ElseIf dtStone.Rows.Count = 0 Then
                    dtStone = objStone.dtGridStone.Clone
                End If
                For Rupd As Integer = 0 To objStone.dtGridStone.Rows.Count - 1
                    If objStone.dtGridStone.Rows(Rupd).Item("KEYNO").ToString <> "" Then Continue For
                    If lblEditKeyNo.Text.ToString = "" Then
                        objStone.dtGridStone.Rows(Rupd)("KEYNO") = Val(DtTran.Rows(DtTran.Rows.Count - 1).Item("KEYNO").ToString)
                    Else
                        objStone.dtGridStone.Rows(Rupd)("KEYNO") = Val(lblEditKeyNo.Text.ToString) + 1
                    End If
                Next
                Dim row1 As DataRow = Nothing
                For Each row1 In objStone.dtGridStone.Rows
                    dtStone.ImportRow(row1)
                Next
            End If
        End If
    End Sub
#End Region

#Region " Datatable TdsEvent"
    Private Sub funcDtGstTds(ByVal ItemId As Integer, ByVal Netamt As Double)
        If lblEditKeyNo.Text = "" Then
        Else
            objGSTTDS = New frmGSTGSTTDS(False, ItemId, cmbAcname_OWN.SelectedValue.ToString, cmbTrantype.Text, 0)
            objGSTTDS.txtNetAmount_AMT.Text = Netamt
        End If
    End Sub
#End Region

#Region " Function Add Event"
    Private Function funcgetDealervalue(ByVal columnName As String) As Double
        Dim value As Decimal = Nothing
        value = func_getDealer_wmc(Val(cmbItem_OWN.SelectedValue.ToString), Val(cmbSubItem_OWN.SelectedValue.ToString), cmbAcname_OWN.SelectedValue.ToString, columnName)
        Return value
    End Function

    Private Function func_getDealer_wmc(ByVal Itemid As Integer, ByVal subitemId As Integer, ByVal accode As String, ByVal columnName As String) As Decimal
        Dim dr As DataRow = Nothing
        Dim Qry As String = funcAlreadyExit_DA(Itemid, subitemId, accode)
        dr = GetSqlRow(Qry, cn)
        If Not dr Is Nothing Then
            Return dr.Item(columnName)
        Else
            Return 0
        End If
    End Function

    Private Function funcAlreadyExit_DA(ByVal Itemid As Integer, ByVal subitemId As Integer, ByVal accode As String) As String
        Dim Qry As String = ""
        Qry = " "
        Qry = vbCrLf + " SELECT COUNT(*) CNT,WASTPER,WAST,MCPER,MCGRM,TOUCH,PUREWT "
        Qry += vbCrLf + " FROM " & cnAdminDb & "..DEALER_WMCTABLE"
        Qry += vbCrLf + " WHERE ITEMID = " & Itemid & "  "
        Qry += vbCrLf + " And SUBITEMID = " & subitemId & ""
        Qry += vbCrLf + " AND ACCODE = '" & accode & "'"
        'Qry += vbCrLf + " And PUREWT = " & purewt & ""
        'Qry += vbCrLf + " And TOUCH = " & Touch & ""
        'Qry += vbCrLf + " And WASTPER = " & wastper & ""
        'Qry += vbCrLf + " And WAST = " & wast & ""
        'Qry += vbCrLf + " And MCPER = " & Mcper & ""
        'Qry += vbCrLf + " And MCGRM = " & MCGRM & ""
        Qry += vbCrLf + " GROUP BY WASTPER,WAST,MCPER,MCGRM,TOUCH,PUREWT"
        Return Qry
    End Function

    Private Sub funcAddDEALER_WMCTABLE(ByVal ItemId As Integer _
                                       , ByVal subItemId As Integer _
                                       , ByVal accode As String _
                                       , ByVal mccalc As String _
                                       , ByVal wastper As Double _
                                       , ByVal wast As Double _
                                       , ByVal wastpie As Double _
                                       , ByVal MCGRM As Double _
                                       , ByVal MC As Double _
                                       , ByVal Touch As Double _
                                       , ByVal Mcper As Double _
                                       , ByVal purewt As Double, ByVal tran As OleDbTransaction _
                                       , ByVal calType As String _
                                       , ByVal Prate As Double, ByVal mcpie As Double, ByVal emcperg As Double)

        Dim alreadyExit As Integer = 0
        strsql = ""
        strsql = funcAlreadyExit_DA(ItemId, subItemId, accode)
        alreadyExit = Val(objGPack.GetSqlValue(strsql, "CNT",, tran).ToString)

        If True Then 'alreadyExit = 0

            strsql = " DELETE " & cnAdminDb & "..DEALER_WMCTABLE where "
            strsql += vbCrLf + " ACCODE='" & accode & "' "
            strsql += vbCrLf + " And ITEMID = " & ItemId & " "
            strsql += vbCrLf + " And SUBITEMID= " & subItemId & ""
            cmd = New OleDbCommand(strsql, cn, tran)
            cmd.ExecuteNonQuery()


            strsql = ""

            strsql = ""
            strsql += vbCrLf + " INSERT INTO " & cnAdminDb & "..DEALER_WMCTABLE"
            strsql += vbCrLf + " ("
            strsql += vbCrLf + " SNO"
            strsql += vbCrLf + " ,ITEMID"
            strsql += vbCrLf + " ,SUBITEMID"
            strsql += vbCrLf + " ,ACCODE"
            strsql += vbCrLf + " ,CALCMODE"
            strsql += vbCrLf + " ,GRSNET"
            strsql += vbCrLf + " ,MCCALC"
            strsql += vbCrLf + " ,WASTPER,WAST,WASTPIE"
            strsql += vbCrLf + " ,MCGRM,MC"
            strsql += vbCrLf + " ,FROM_WT"
            strsql += vbCrLf + " ,TO_WT"
            strsql += vbCrLf + " ,TOUCH"
            strsql += vbCrLf + " ,USERID"
            strsql += vbCrLf + " ,UPDATED"
            strsql += vbCrLf + " ,UPTIME"
            strsql += vbCrLf + " ,COSTID"
            strsql += vbCrLf + " ,MCPER"
            strsql += vbCrLf + " ,PUREWT "
            strsql += vbCrLf + " ,TAXINCLUCIVE "
            strsql += vbCrLf + "  ,PRATE,MCPIE,EMCPERG)"
            strsql += vbCrLf + " SELECT ISNULL(MAX(SNO),0)+1  SNO"
            strsql += vbCrLf + " ," & ItemId & " AS ITEMID"
            strsql += vbCrLf + " ," & subItemId & " AS SUBITEMID"
            strsql += vbCrLf + " ,'" & accode & "' AS ACCODE"
            strsql += vbCrLf + " ,'T' CALCMODE"
            strsql += vbCrLf + " ,'N' GRSNET"
            strsql += vbCrLf + " ,'" & mccalc & "' MCCALC"
            strsql += vbCrLf + " ," & wastper & " WASTPER"
            strsql += vbCrLf + " ," & wast & " WAST"
            strsql += vbCrLf + " ," & wastpie & " WASTPIE"
            strsql += vbCrLf + " ," & MCGRM & " MCGRM"
            strsql += vbCrLf + " ," & MC & " MC"
            strsql += vbCrLf + " ,0 FROM_WT"
            strsql += vbCrLf + " ,0 TO_WT"
            strsql += vbCrLf + " ," & Touch & " TOUCH"
            strsql += vbCrLf + " ," & userId & " USERID"
            strsql += vbCrLf + " ,'" & Format(Now.Date, "yyyy-MM-dd") & "' UPDATED"
            strsql += vbCrLf + " ,'" & Date.Now.ToLongTimeString & "' UPTIME"
            strsql += vbCrLf + " ,'" & cnCostId & "' COSTID"
            strsql += vbCrLf + " ," & Mcper & " MCPER"
            strsql += vbCrLf + " ," & purewt & " PUREWT"
            strsql += vbCrLf + " ,'' TAXINCLUCIVE"
            If calType = "R" Or calType = "M" Or calType = "P" Then
                strsql += vbCrLf + "," & Prate & " PRATE"
            Else
                strsql += vbCrLf + ",0 PRATE"
            End If
            strsql += vbCrLf + " ," & mcpie & " MCPIE"
            strsql += vbCrLf + " ," & emcperg & " EMCPERG"
            strsql += vbCrLf + " FROM " & cnAdminDb & "..DEALER_WMCTABLE"
            cmd = New OleDbCommand(strsql, cn, tran)
            cmd.ExecuteNonQuery()
        End If
    End Sub

    Private Function funcSubitemmastValidate() As Boolean
        Dim drItemMast As DataRow = Nothing
        strsql = " SELECT ISNULL(SUBITEM,'N')SUBITEM "
        strsql += vbCrLf + " ,COUNT(*) CNT "
        strsql += vbCrLf + " FROM " & cnAdminDb & "..ITEMMAST  "
        strsql += vbCrLf + " WHERE ITEMID = " & cmbItem_OWN.SelectedValue.ToString & " "
        strsql += vbCrLf + " GROUP BY ISNULL(SUBITEM,'N') "
        drItemMast = GetSqlRow(strsql, cn)
        If Not drItemMast Is Nothing Then
            If Val(drItemMast.Item("CNT").ToString) > 0 Then
                If drItemMast.Item("SUBITEM").ToString = "Y" Then
                    If cmbSubItem_OWN.Text = "" Then
                        MsgBox("Subitemmast should not empty", MsgBoxStyle.Information)
                        Return False
                    End If
                End If
            End If
        End If
        Return True
    End Function

    Private Function mimrDate() As Boolean
        Dim serverDate As Date = GetActualEntryDate()
        Dim RestrictDays As String = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'DATECONTROL_MIMR'")
        If RestrictDays.Contains(",") = False Then
            If Not (dtpTrandate.Value >= serverDate.AddDays(-1 * Val(RestrictDays))) Then
                If funcOTPaccess(Me.Name.ToString, "ACCOUNTS_MIMR", cnCostId, userId) = True Then Return False
                MsgBox("Invalid Date", MsgBoxStyle.Information)
                dtpTrandate.Focus()
                Return False
            End If
        Else
            Dim dayarray() As String = Split(RestrictDays, ",")
            Dim closeday As Integer = Val(dayarray(0).ToString)
            Dim mondiv As Integer = Val(dayarray(1).ToString)
            If closeday > serverDate.Day Then
                Dim mindate As Date = serverDate.AddMonths(-1 * mondiv)
                mindate = mindate.AddDays(-1 * (mindate.Day - 1))
                If Not (dtpTrandate.Value >= mindate) Then
                    If funcOTPaccess(Me.Name.ToString, "ACCOUNTS_MIMR", cnCostId, userId) = True Then Return False
                    MsgBox("Invalid Date", MsgBoxStyle.Information)
                    dtpTrandate.Focus()
                    Return False
                End If
            Else
                Dim mindate As Date = serverDate.AddMonths(-1 * 0)
                mindate = mindate.AddDays(-1 * (mindate.Day - 1))
                If Not (dtpTrandate.Value >= mindate) Then
                    If funcOTPaccess(Me.Name.ToString, "ACCOUNTS_MIMR", cnCostId, userId) = True Then Return False
                    MsgBox("Invalid Bill Date", MsgBoxStyle.Information)
                    dtpTrandate.Focus()
                    Return False
                End If
            End If
        End If
        Return True
    End Function
    Private Function funcOnAccountvalid() As Boolean
        'strsql = ""
        'strsql = " SELECT COUNT(*)cnt FROM " & cnStockDb & "..PAYMENTDET WHERE TRANTYPE='ON' "
        'strsql += vbCrLf + " AND ISNULL(CHQBATCHNO,'')  ='' "
        'strsql += vbCrLf + " AND ACCODE='" & cmbAcname_OWN.SelectedValue.ToString & "' "
        'strsql += vbCrLf + " AND ISNULL(CANCEL,'') = '' "
        'Dim onAccountCheck As Integer = 0
        'onAccountCheck = Val(objGPack.GetSqlValue(strsql).ToString)
        'If onAccountCheck > 0 Then
        '    MsgBox("OnAccount Entry Not Closed", MsgBoxStyle.Information)
        '    strsql = ""
        '    Return False
        'End If
        Return True
        strsql = ""
    End Function


    Private Sub funcAddGrid()
        If checkMIMR() = False Then
            Exit Sub
        End If
        If mimrDate() = False Then
            Exit Sub
        End If
        Dim AcStatecode As String = funcAcNameStatecode()
        Dim supplierGst As String = Mid(funcSupplierGstNo(cmbAcname_OWN.SelectedValue.ToString), 1, 2)
        If AcStatecode = "" Then
            AcStatecode = supplierGst
        End If
        If supplierGst <> "" Then
            If AcStatecode <> supplierGst Then
                MsgBox("State Code " & AcStatecode & " and Gstcode " & supplierGst & " mismatch in master", MsgBoxStyle.Information)
                Exit Sub
            End If
        End If
        If cmbTrantype.Text = "PURCHASE" Or cmbTrantype.Text = "RECEIPT" Then
            If Val(txtConTDSPER_AMT.Text) > 0 And Val(txtConTCSPER_WET.Text) > 0 Then
                MsgBox("Entry not allowed TDS & TCS both ", MsgBoxStyle.Information)
                Exit Sub
            End If
        End If
        If Val(txtNetVal_AMT.Text) <= 0 Then
            ' MsgBox("Amount Should not empty", MsgBoxStyle.Information)
            ' Exit Sub
        End If
        If Val(txtMC_AMT.Text) > 0 Then
            If Val(txtGrossAmt_AMT.Text) = 0 Then
                MsgBox("Making Charge greater than Amount Should not empty", MsgBoxStyle.Information)
                Exit Sub
            End If
        End If
        If funcSubitemmastValidate() = False Then
            Exit Sub
        End If
        Dim ItemId As Integer = 0
        strsql = " SELECT COUNT(*) CNT FROM " & cnAdminDb & "..ITEMMAST "
        strsql += vbCrLf + " WHERE ITEMID = '" & cmbItem_OWN.SelectedValue.ToString & "'"
        ItemId = Val(objGPack.GetSqlValue(strsql).ToString)
        If ItemId = 0 Then
            MsgBox("ItemId should not empty", MsgBoxStyle.Information)
            cmbItem_OWN.Focus()
            cmbItem_OWN.SelectAll()
            Exit Sub
        End If
        strsql = ""
        Dim SubItemId As Integer = 0
        Dim drSubItemmast As DataRow = Nothing
        If cmbSubItem_OWN.Text <> "" Then
            strsql = " SELECT * FROM " & cnAdminDb & "..SUBITEMMAST "
            strsql += vbCrLf + " WHERE ITEMID = '" & cmbItem_OWN.SelectedValue.ToString & "' "
            strsql += vbCrLf + " AND SUBITEMNAME='" & cmbSubItem_OWN.Text & "' "
            SubItemId = Val(objGPack.GetSqlValue(strsql).ToString)
            If SubItemId = 0 Then
                MsgBox("SubitemName should not empty", MsgBoxStyle.Information)
                cmbSubItem_OWN.Focus()
                cmbSubItem_OWN.SelectAll()
                Exit Sub
            End If
            strsql = " SELECT * FROM " & cnAdminDb & "..SUBITEMMAST "
            strsql += vbCrLf + " WHERE ITEMID = '" & cmbItem_OWN.SelectedValue.ToString & "' "
            strsql += vbCrLf + " AND SUBITEMNAME='" & cmbSubItem_OWN.Text & "' "
            drSubItemmast = GetSqlRow(strsql, cn)
            If Not drSubItemmast Is Nothing Then
                ''If drSubItemmast.Item("HSN").ToString.Length <= 5 Then
                ''    MsgBox("Please update HSNCODE 6 digit in master", MsgBoxStyle.Information)
                ''    Exit Sub
                ''End If
            End If
        End If
        strsql = ""
        If Val(txtPcs_NUM.Text) = 0 Then
            MsgBox("Pcs should not empty", MsgBoxStyle.Information)
            txtPcs_NUM.Focus()
            txtPcs_NUM.SelectAll()
            Exit Sub
        End If

        strsql = ""

        If Val(txtPcs_NUM.Text) = 0 And Val(txtGrsWt_WET.Text) = 0 Then
            MsgBox("Pcs & Weight should Not empty", MsgBoxStyle.Information)
            txtPcs_NUM.Focus()
            txtPcs_NUM.SelectAll()
            Exit Sub
        End If

        If funcOnAccountvalid() = False Then
            Exit Sub
        End If

        strsql = ""
        Dim dtGridTotalCheck As New DataTable
        dtGridTotalCheck = Gridview_OWN.DataSource
        If MIMRSTOCKCHECKING = "Y" Then
            If ReceiptStockChecking(dtGridTotalCheck, Val(txtNetWt_WET.Text)) = False Then
                Exit Sub
            End If
            If IssueStockChecking(dtGridTotalCheck, Val(txtNetWt_WET.Text)) = False Then
                Exit Sub
            End If
        End If
        For i As Integer = 0 To dtGridTotalCheck.Rows.Count - 1
        Next
        Dim ReceiptAmt As Double = 0
        Dim IssueAmt As Double = 0
        Dim FinalRecIssAmt As Double = 0
        ReceiptAmt = Val(dtGridTotalCheck.Compute("SUM(NETAMT)", "TRANTYPE='RECEIPT'").ToString)
        If cmbTrantype.SelectedValue = "RECEIPT" Then
            ReceiptAmt += Val(txtNetVal_AMT.Text)
        End If
        IssueAmt = Val(dtGridTotalCheck.Compute("SUM(NETAMT)", "TRANTYPE='ISSUE'").ToString)
        If cmbTrantype.SelectedValue = "ISSUE" Then
            IssueAmt += Val(txtNetVal_AMT.Text)
        End If
        FinalRecIssAmt = ReceiptAmt - IssueAmt
        If FinalRecIssAmt < 0 Then
            If ReceiptAmt > 0 Then
                MsgBox("Amount should not exceed", MsgBoxStyle.Information)
                Exit Sub
            End If
        End If
        If cmbTrantype.Text = "PURCHASE" Then
        Else
            If Val(txtTouchper_AMT.Text) <> Math.Round(Val(txtPurityper_RATE.Text) + Val(txtwastageper_AMT.Text) + Val(txtMCPPer_RATE.Text), 2) Then
                MsgBox("Mismatch Touch percentage", MsgBoxStyle.Information)
                txtTouchper_AMT.Focus()
                txtTouchper_AMT.SelectAll()
                Exit Sub
            End If
        End If
        Dim metalId As String = ""
        Dim metalName As String = ""
        Dim catcode As String = ""
        Dim catName As String = ""

        Dim ItemHsn As String = ""
        Dim SubItemHsn As String = ""
        Dim CatNameHsn As String = ""

        Dim drItemmast As DataRow = Nothing
        Dim drCategory As DataRow = Nothing

        strsql = " SELECT METALID,CATCODE,ISNULL(HSN,'') HSN FROM " & cnAdminDb & "..ITEMMAST "
        strsql += vbCrLf + " WHERE ITEMID = '" & cmbItem_OWN.SelectedValue.ToString & "'"
        drItemmast = GetSqlRow(strsql, cn)
        If Not drItemmast Is Nothing Then
            metalId = drItemmast.Item("METALID").ToString
            catcode = drItemmast.Item("CATCODE").ToString
            ItemHsn = drItemmast.Item("HSN").ToString
            ''If ItemHsn.Length <= 5 Then
            ''    MsgBox("Update HSNCODE 6 digit's in Itemmast", MsgBoxStyle.Information)
            ''    Exit Sub
            ''End If
        End If
        strsql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID ='" & metalId & "' "
        metalName = GetSqlValue(cn, strsql)

        strsql = " SELECT * FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE ='" & catcode & "'"
        drCategory = GetSqlRow(strsql, cn)
        If drCategory Is Nothing Then
            MsgBox("Invalid Category")
            Exit Sub
        End If
        ''If Not drCategory Is Nothing Then
        ''    CatNameHsn = drCategory.Item("HSN").ToString
        ''    If CatNameHsn.Length <= 5 Then
        ''        MsgBox("Update HSNCODE 6 digits in Category", MsgBoxStyle.Information)
        ''        Exit Sub
        ''    End If
        ''End If
        Dim dtAddReseverCharge As New DataTable
        dtAddReseverCharge = CalcGstReversalonWastage()
        If dtAddReseverCharge.Rows.Count > 0 Then
            If Math.Round(Val(dtAddReseverCharge.Rows(0).Item("WSGST").ToString) _
                + Val(dtAddReseverCharge.Rows(0).Item("WCGST").ToString) _
                + Val(dtAddReseverCharge.Rows(0).Item("WIGST").ToString), 2) <> Math.Round(Val(dtAddReseverCharge.Rows(0).Item("GST").ToString), 2) Then
                MsgBox("Wastage CGST & SGST & IGST mismatch", MsgBoxStyle.Information)
                Exit Sub
            End If
        End If
        Dim strratefixaccode As String = ""
        Dim drRfix As DataRow = Nothing
        strsql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID='RATEFIXACCODE'"
        drRfix = GetSqlRow(strsql, cn)
        If Not drRfix Is Nothing Then
            If drRfix(0).ToString <> "" Then
                Dim s1() As String = drRfix(0).ToString.Split(",")
                For i As Integer = 0 To s1.Length - 1
                    Dim s2() As String = s1(i).ToString.Split(":")
                    If s2.Length > 0 Then
                        If s2(0).ToString = catcode Then
                            strratefixaccode = s2(1).ToString
                            GoTo findRateFixed
                        End If
                    End If
                Next
            End If
        End If
findRateFixed:
        If lblEditKeyNo.Text.ToString = "" Then
            Dim dr As DataRow
            dr = DtTran.NewRow
            With dr
                .Item("METALID") = metalId
                .Item("METALNAME") = metalName
                .Item("CATCODE") = catcode
                .Item("CATNAME") = drCategory.Item("CATNAME")
                .Item("ITEMID") = cmbItem_OWN.SelectedValue.ToString
                .Item("ITEMNAME") = cmbItem_OWN.Text
                If cmbSubItem_OWN.SelectedValue Is Nothing Then
                    .Item("SUBITEMID") = 0
                Else
                    .Item("SUBITEMID") = cmbSubItem_OWN.SelectedValue.ToString
                End If
                .Item("SUBITEMNAME") = cmbSubItem_OWN.Text
                .Item("PURITY") = Format(Val(txtPurityper_RATE.Text), "0.0000")
                .Item("PCS") = Val(txtPcs_NUM.Text)
                .Item("GRSNET") = Mid(cmbGrsNet.Text, 1, 1)
                .Item("GRSWT") = Val(txtGrsWt_WET.Text)
                .Item("LESSWT") = Val(txtLessWt_WET.Text)
                .Item("NETWT") = Val(txtNetWt_WET.Text)
                .Item("RATE") = Format(Val(txtRate_RATE.Text), "0.0000")
                .Item("BOARDRATE") = Format(Val(objMIMRRate.txtNewRateInclusive_RATE.Text), "0.0000") ' funcBoardRate(cmbItem_OWN.SelectedValue.ToString)
                .Item("TOUCH") = Val(txtTouchper_AMT.Text)
                .Item("PUREWT") = Val(txtPurityWt_WET.Text)
                .Item("WASTAGEPER") = Val(txtwastageper_AMT.Text)
                .Item("WASTAGE") = Val(txtwastage_AMT.Text)

                .Item("MCPER") = Format(Val(txtMCPPer_RATE.Text), "0.0000")
                .Item("MCGRAM") = Format(Val(txtMCPerGram_RATE.Text), "0.0000")
                .Item("MC") = Val(txtMC_AMT.Text)
                .Item("MCPIE") = Val(txtMCPieces_AMT.Text)
                .Item("EMCPERG") = Val(txtEMCPERG_AMT.Text)

                .Item("SGSTPER") = Val(lblSGSTPer.Text) 'drCategory.Item("S_SGSTTAX")
                .Item("CGSTPER") = Val(lblCGSTPer.Text) 'drCategory.Item("S_CGSTTAX")
                .Item("IGSTPER") = Val(lblIGSTPer.Text) 'drCategory.Item("S_IGSTTAX")

                .Item("STUDAMT") = Val(txtStoneValue_AMT.Text)
                .Item("GROSSAMT") = Val(txtGrossAmt_AMT.Text)
                .Item("SGST") = Val(txtSGSTVAL_AMT.Text)
                .Item("CGST") = Val(txtCGSTVAL_AMT.Text)
                .Item("IGST") = Val(txtIGSTVAL_AMT.Text)
                .Item("GST") = Val(txtSGSTVAL_AMT.Text) + Val(txtCGSTVAL_AMT.Text) + Val(txtIGSTVAL_AMT.Text)
                .Item("TDSPER") = 0
                .Item("TDSVAL") = 0

                .Item("TCSPER") = 0
                .Item("TCSVAL") = 0

                .Item("NETAMT") = Val(txtNetVal_AMT.Text)
                '.Item("TRANTYPE") = cmbInvoiceType.Text
                .Item("TRANTYPE") = cmbTrantype.SelectedValue.ToString()
                .Item("REMARK1") = txtRemark1.Text.Trim
                .Item("REMARK2") = txtRemark2.Text.Trim
                .Item("PAYMODE") = cmbTrantype.Text
                .Item("ACCODE") = cmbAcname_OWN.SelectedValue.ToString
                .Item("CALTYPE") = ItemCaltype(cmbItem_OWN.SelectedValue.ToString, cmbSubItem_OWN.SelectedValue.ToString)
                If dtAddReseverCharge.Rows.Count > 0 Then
                    .Item("WSGSTPER") = dtAddReseverCharge.Rows(0).Item("WSGSTPER").ToString
                    .Item("WCGSTPER") = dtAddReseverCharge.Rows(0).Item("WCGSTPER").ToString
                    .Item("WIGSTPER") = dtAddReseverCharge.Rows(0).Item("WIGSTPER").ToString
                    .Item("WSGST") = dtAddReseverCharge.Rows(0).Item("WSGST").ToString
                    .Item("WCGST") = dtAddReseverCharge.Rows(0).Item("WCGST").ToString
                    .Item("WIGST") = dtAddReseverCharge.Rows(0).Item("WIGST").ToString
                End If
                If cmbTrantype.Text = "PURCHASE" Then
                    If objMIMRRate.rbtFixed.Checked = True Then
                        .Item("TRANSTATUS") = "F"
                    ElseIf objMIMRRate.rbtUnFixed.Checked = True Then
                        .Item("TRANSTATUS") = "U"
                    End If
                Else
                    .Item("TRANSTATUS") = ""
                End If
                .Item("RATEFIXACCODE") = strratefixaccode
                If txtRemark1.Text.Trim <> "" Then
                    .Item("ESTTAGNO") = ESTTAGNO
                    .Item("ESTSNO") = ESTSNO
                    .Item("ESTBATCHNO") = ESTBATCHNO
                    .Item("ITEMTAGPCS") = ITEMTAGPCS
                    .Item("ITEMTAGGRSWT") = ITEMTAGGRSWT
                    .Item("ITEMTAGNETWT") = ITEMTAGNETWT
                End If
            End With
            DtTran.Rows.Add(dr)
        Else
            With DtTran.Rows(Val(lblEditKeyNo.Text.ToString))
                .Item("METALID") = metalId
                .Item("METALNAME") = metalName
                .Item("CATCODE") = catcode
                .Item("CATNAME") = drCategory.Item("CATNAME")
                .Item("ITEMID") = cmbItem_OWN.SelectedValue.ToString
                .Item("ITEMNAME") = cmbItem_OWN.Text
                If cmbSubItem_OWN.SelectedValue Is Nothing Then
                    .Item("SUBITEMID") = 0
                Else
                    .Item("SUBITEMID") = cmbSubItem_OWN.SelectedValue.ToString
                End If
                .Item("SUBITEMNAME") = cmbSubItem_OWN.Text
                .Item("PURITY") = Format(Val(txtPurityper_RATE.Text), "0.0000")
                .Item("PCS") = Val(txtPcs_NUM.Text)
                .Item("GRSNET") = Mid(cmbGrsNet.Text, 1, 1)
                .Item("GRSWT") = Val(txtGrsWt_WET.Text)
                .Item("LESSWT") = Val(txtLessWt_WET.Text)
                .Item("NETWT") = Val(txtNetWt_WET.Text)
                .Item("RATE") = Format(Val(txtRate_RATE.Text), "0.0000")
                .Item("BOARDRATE") = Format(Val(objMIMRRate.txtNewRateInclusive_RATE.Text), "0.0000") 'funcBoardRate(cmbItem_OWN.SelectedValue.ToString)
                .Item("TOUCH") = Val(txtTouchper_AMT.Text)
                .Item("PUREWT") = Val(txtPurityWt_WET.Text)

                .Item("WASTAGEPER") = Val(txtwastageper_AMT.Text)
                .Item("WASTAGE") = Val(txtwastage_AMT.Text)

                .Item("MCPER") = Format(Val(txtMCPPer_RATE.Text), "0.0000")
                .Item("MCGRAM") = Format(Val(txtMCPerGram_RATE.Text), "0.0000")
                .Item("MC") = Val(txtMC_AMT.Text)
                .Item("MCPIE") = Val(txtMCPieces_AMT.Text)
                .Item("EMCPERG") = Val(txtEMCPERG_AMT.Text)

                .Item("SGSTPER") = Val(lblSGSTPer.Text) 'drCategory.Item("S_SGSTTAX")
                .Item("CGSTPER") = Val(lblCGSTPer.Text) 'drCategory.Item("S_CGSTTAX")
                .Item("IGSTPER") = Val(lblIGSTPer.Text) 'drCategory.Item("S_IGSTTAX")
                .Item("STUDAMT") = Val(txtStoneValue_AMT.Text)
                .Item("GROSSAMT") = Val(txtGrossAmt_AMT.Text)
                .Item("SGST") = Val(txtSGSTVAL_AMT.Text)
                .Item("CGST") = Val(txtCGSTVAL_AMT.Text)
                .Item("IGST") = Val(txtIGSTVAL_AMT.Text)
                .Item("GST") = Val(txtSGSTVAL_AMT.Text) + Val(txtCGSTVAL_AMT.Text) + Val(txtIGSTVAL_AMT.Text)
                .Item("TDSPER") = 0
                .Item("TDSVAL") = 0
                .Item("TCSPER") = 0
                .Item("TCSVAL") = 0
                .Item("NETAMT") = Val(txtNetVal_AMT.Text)
                '.Item("TRANTYPE") = cmbInvoiceType.Text
                .Item("TRANTYPE") = cmbTrantype.SelectedValue.ToString
                .Item("REMARK1") = txtRemark1.Text.Trim
                .Item("REMARK2") = txtRemark2.Text.Trim
                .Item("PAYMODE") = cmbTrantype.Text
                .Item("ACCODE") = cmbAcname_OWN.SelectedValue.ToString
                .Item("CALTYPE") = ItemCaltype(cmbItem_OWN.SelectedValue.ToString, cmbSubItem_OWN.SelectedValue.ToString)
                If dtAddReseverCharge.Rows.Count > 0 Then
                    .Item("WSGSTPER") = dtAddReseverCharge.Rows(0).Item("WSGSTPER").ToString
                    .Item("WCGSTPER") = dtAddReseverCharge.Rows(0).Item("WCGSTPER").ToString
                    .Item("WIGSTPER") = dtAddReseverCharge.Rows(0).Item("WIGSTPER").ToString
                    .Item("WSGST") = dtAddReseverCharge.Rows(0).Item("WSGST").ToString
                    .Item("WCGST") = dtAddReseverCharge.Rows(0).Item("WCGST").ToString
                    .Item("WIGST") = dtAddReseverCharge.Rows(0).Item("WIGST").ToString
                End If
                If cmbTrantype.SelectedValue.ToString() = "PU" Then
                    If objMIMRRate.rbtFixed.Checked = True Then
                        .Item("TRANSTATUS") = "F"
                    ElseIf objMIMRRate.rbtUnFixed.Checked = True Then
                        .Item("TRANSTATUS") = "U"
                    End If
                Else
                    .Item("TRANSTATUS") = ""
                End If
                .Item("RATEFIXACCODE") = strratefixaccode
                If txtRemark1.Text.Trim <> "" Then
                    .Item("ESTTAGNO") = ESTTAGNO
                    .Item("ESTSNO") = ESTSNO
                    .Item("ESTBATCHNO") = ESTBATCHNO
                    .Item("ITEMTAGPCS") = ITEMTAGPCS
                    .Item("ITEMTAGGRSWT") = ITEMTAGGRSWT
                    .Item("ITEMTAGNETWT") = ITEMTAGNETWT
                End If
                DtTran.AcceptChanges()
            End With
        End If
        funcDtstone()
        funMultiClear()
        styleGrid(Gridview_OWN)
        funGrandTotal()
        If Val(txtNetVal_AMT.Text) < 0 Then
            txtNetVal_AMT.Text = ""
        End If
        Textboxonlyclear()
        txtRemarkreadonlytrueorfalse(False, False)
        Dim dtGrandTdTc As New DataTable
        dtGrandTdTc = Gridview_OWN.DataSource
        If dtGrandTdTc.Rows.Count > 0 Then
            funcalcTds_TcsRounded(dtGrandTdTc)
        End If
        LoadBalanceWt(cmbAcname_OWN.SelectedValue.ToString, "RECEIPT")
        cmbTrantype.Focus()
        cmbTrantype.SelectAll()
    End Sub
#End Region
#End Region

#Region " GridView Events"
    Private Sub gridviewcellformatColor(ByVal grid As DataGridView, ByVal e As DataGridViewCellFormattingEventArgs)
        With grid
            If .Rows.Count > 0 Then
                If .Columns.Contains("PAYMODE") Then
                    If .Rows(e.RowIndex).Cells("PAYMODE").Value.ToString = "RECEIPT" Then
                        .Rows(e.RowIndex).DefaultCellStyle.BackColor = Color.Lavender
                    ElseIf .Rows(e.RowIndex).Cells("PAYMODE").Value.ToString = "PURCHASE" Then
                        .Rows(e.RowIndex).DefaultCellStyle.BackColor = Color.LightPink
                    ElseIf .Rows(e.RowIndex).Cells("PAYMODE").Value.ToString = "PURCHASE[APPROVAL]" Then
                        .Rows(e.RowIndex).DefaultCellStyle.BackColor = Color.LightBlue
                    ElseIf .Rows(e.RowIndex).Cells("PAYMODE").Value.ToString = "ISSUE" Then
                        .Rows(e.RowIndex).DefaultCellStyle.BackColor = Color.LightGray
                    ElseIf .Rows(e.RowIndex).Cells("PAYMODE").Value.ToString = "PURCHASE RETURN" Then
                        .Rows(e.RowIndex).DefaultCellStyle.BackColor = Color.LightGreen
                    End If
                End If
            End If
        End With
    End Sub
    Private Sub gridViewTotal_Own_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles gridViewTotal_Own.CellFormatting
        gridviewcellformatColor(gridViewTotal_Own, e)
    End Sub
    Private Sub Gridview_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles Gridview_OWN.CellFormatting
        gridviewcellformatColor(Gridview_OWN, e)
    End Sub
    Private Sub Gridview_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Gridview_OWN.KeyDown
        If e.KeyCode = Keys.Enter Then
            If Gridview_OWN.Rows.Count > 0 Then
                lblEditKeyNo.Text = (Val(Gridview_OWN.CurrentRow.Cells("KEYNO").Value) - 1)
                With Gridview_OWN.Rows(Val(lblEditKeyNo.Text))
                    txtItemId_NUM.Text = .Cells("ITEMID").Value.ToString
                    cmbItem_OWN.Text = .Cells("ITEMNAME").Value.ToString
                    cmbSubItem_OWN.Text = .Cells("SUBITEMNAME").Value.ToString
                    txtPurityper_RATE.Text = .Cells("PURITY").Value.ToString
                    txtPcs_NUM.Text = .Cells("PCS").Value.ToString
                    txtEMCPERG_AMT.Text = .Cells("EMCPERG").Value.ToString
                    txtGrsWt_WET.Text = .Cells("GRSWT").Value.ToString
                    txtLessWt_WET.Text = .Cells("LESSWT").Value.ToString
                    txtNetWt_WET.Text = .Cells("NETWT").Value.ToString
                    txtRate_RATE.Text = .Cells("RATE").Value.ToString
                    txtTouchper_AMT.Text = .Cells("TOUCH").Value.ToString
                    txtPurityWt_WET.Text = .Cells("PUREWT").Value.ToString

                    txtwastageper_AMT.Text = .Cells("WASTAGEPER").Value.ToString
                    txtwastage_AMT.Text = .Cells("WASTAGE").Value.ToString

                    txtMCPPer_RATE.Text = .Cells("MCPER").Value.ToString
                    txtMCPerGram_RATE.Text = .Cells("MCGRAM").Value.ToString

                    txtMC_AMT.Text = .Cells("MC").Value.ToString
                    txtMCPieces_AMT.Text = .Cells("MCPIE").Value.ToString

                    txtStoneValue_AMT.Text = .Cells("STUDAMT").Value.ToString
                    txtGrossAmt_AMT.Text = .Cells("GROSSAMT").Value.ToString
                    funcDtGstTds(.Cells("ITEMID").Value.ToString, Val(txtGrossAmt_AMT.Text))
                    'GSTVAL CAL
                    'txtGstVal_AMT.Text = .Cells("GST").Value.ToString
                    txtCGSTVAL_AMT.Text = .Cells("CGST").Value.ToString()
                    lblCGSTPer.Text = .Cells("CGSTPER").Value.ToString()
                    txtSGSTVAL_AMT.Text = .Cells("SGST").Value.ToString()
                    lblSGSTPer.Text = .Cells("SGSTPER").Value.ToString()
                    txtIGSTVAL_AMT.Text = .Cells("IGST").Value.ToString()
                    lblIGSTPer.Text = .Cells("IGSTPER").Value.ToString()
                    'TDSVAL CAL
                    txtNetVal_AMT.Text = .Cells("NETAMT").Value.ToString

                    If dtStone.Rows.Count > 0 Then
                        Dim dtTmp As New DataTable
                        Dim dv As New DataView(dtStone)
                        dv.RowFilter = "KEYNO='" & Val(Gridview_OWN.CurrentRow.Cells("KEYNO").Value.ToString) & "'"
                        dtTmp = dv.ToTable
                        If dtTmp.Rows.Count > 0 Then
                            objStone = New frmStoneDiaAc
                            objStone.dtGridStone = dtTmp
                            objStone.gridStone.DataSource = dtTmp
                        End If
                    End If
                    txtRemark1.Text = .Cells("REMARK1").Value.ToString
                    txtRemark2.Text = .Cells("REMARK2").Value.ToString
                    If .Cells("ESTTAGNO").Value.ToString <> "" Then
                        txtRemarkreadonlytrueorfalse(True, True)
                        ESTTAGNO = .Cells("ESTTAGNO").Value.ToString
                        ESTSNO = .Cells("ESTSNO").Value.ToString
                        ESTBATCHNO = .Cells("ESTBATCHNO").Value.ToString
                        ITEMTAGPCS = Val(.Cells("ITEMTAGPCS").Value.ToString)
                        ITEMTAGGRSWT = Val(.Cells("ITEMTAGGRSWT").Value.ToString)
                        ITEMTAGNETWT = Val(.Cells("ITEMTAGNETWT").Value.ToString)
                    Else
                        txtRemarkreadonlytrueorfalse(False, False)
                        funclearESTVariable()
                    End If
                    cmbTrantype.Text = .Cells("PAYMODE").Value.ToString
                    txtPurityper_RATE.Focus()
                    txtPurityper_RATE.SelectAll()
                End With
            End If
        ElseIf e.KeyCode = Keys.Delete Then
            If Gridview_OWN.Rows.Count > 0 Then

            End If
        End If
    End Sub
#End Region

#Region " Panel KeyCode"
    Private Sub pnlText_PreviewKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles pnlText.PreviewKeyDown
        If e.KeyCode = Keys.Escape Then
            btnOk.Focus()
        End If
    End Sub
    Private Sub txtLessWt_WET_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtLessWt_WET.KeyPress
        If e.KeyChar = Chr(19) Then 'Chr(Keys.S)
            Dim stud As String = ""
            strsql = " SELECT ISNULL(STUDDEDSTONE,'')STUDDEDSTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & cmbItem_OWN.SelectedValue.ToString & ""
            stud = GetSqlValue(cn, strsql)
            If True Then 'stud = "Y"
                If False Then
                    If objStone.dtGridStone.Rows.Count > 0 Then
                        ShowStoneDia(0, Now.Date, "")
                    Else
                        SendKeys.Send("{TAB}")
                    End If
                Else
                    ShowStoneDia(0, Now.Date, "")
                End If
            Else
                SendKeys.Send("{TAB}")
            End If
        End If
    End Sub
    Private Sub txtLessWt_WET_KeyDown(sender As Object, e As KeyEventArgs) Handles txtLessWt_WET.KeyDown
        If e.KeyCode = Keys.Enter Then
            If Val(txtLessWt_WET.Text) > 0 Then
                cmbGrsNet.Text = "NET WT"
            End If
        End If
    End Sub
#End Region

#Region " Tab 2"
    Dim SMS_OTP_MIMR_DUPLICATEPRINT As String = GetAdmindbSoftValue("SMS_OTP_MIMR_DUPLICATEPRINT", "").ToString
    Dim SMS_OTP_MIMR_CANCEL As String = GetAdmindbSoftValue("SMS_OTP_MIMR_CANCEL", "").ToString
    Private Sub gridViewRpt_KeyDown(sender As Object, e As KeyEventArgs) Handles gridViewRpt.KeyDown
    End Sub
    Private Sub btnSearchRpt_Click(sender As Object, e As EventArgs) Handles btnSearchRpt.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        Try
            btnSearchRpt.Enabled = False
            Me.Cursor = Cursors.WaitCursor
            gridViewRpt.DataSource = Nothing
            strsql = ""
            strsql += vbCrLf + " EXEC " & cnAdminDb & "..[SP_RPT_MIMR_NEW] "
            strsql += vbCrLf + " @DBNAME ='" & cnStockDb & "'"
            strsql += vbCrLf + " ,@ADMINDB='" & cnAdminDb & "'"
            strsql += vbCrLf + " ,@FROMDATE='" & Format(dtpFrom.Value.Date, "yyyy-MM-dd") & "'"
            strsql += vbCrLf + " ,@TODATE='" & Format(dtpFrom.Value.Date, "yyyy-MM-dd") & "'"
            strsql += vbCrLf + " ,@COMPANYID=''"
            strsql += vbCrLf + " ,@COSTID =''"
            strsql += vbCrLf + " ,@VIEWTRANSFERD='Y'"
            strsql += vbCrLf + " ,@SYSTEMID='" & Environment.MachineName & "'"
            strsql += vbCrLf + " ,@GROUPBY='T'"
            If cmbMetalRPT_OWN.Text = "" Or cmbMetalRPT_OWN.Text = "ALL" Then
                strsql += vbCrLf + " ,@METALID=''"
            Else
                strsql += vbCrLf + " ,@METALID='" & cmbMetalRPT_OWN.SelectedValue.ToString & "'"
            End If
            If cmbTrantypeRPT.Text = "" Or cmbTrantypeRPT.Text = "ALL" Then
                strsql += vbCrLf + " ,@TRANTYPE=''"
            Else
                strsql += vbCrLf + " ,@TRANTYPE='" & cmbTrantypeRPT.SelectedValue & "'"
            End If
            If cmbTrantypeRPT.Text = "" Or cmbTrantypeRPT.Text = "ALL" Then
                strsql += vbCrLf + " ,@TABLENAME=''"
            Else
                strsql += vbCrLf + " ,@TABLENAME='" & cmbTrantypeRPT.SelectedValue.ToString & "'"
            End If
            If rbtReceiptRPT.Checked = True Then
                strsql += vbCrLf + " ,@PAYMODE='R'"
            ElseIf rbtIssueRPT.Checked = True Then
                strsql += vbCrLf + " ,@PAYMODE='I'"
            ElseIf rbtAllRPT.Checked = True Then
                strsql += vbCrLf + " ,@PAYMODE=''"
            End If
            Dim dsRpt As New DataSet
            cmd = New OleDbCommand(strsql, cn)
            da = New OleDbDataAdapter(cmd)
            da.Fill(dsRpt)
            dt = New DataTable
            dt = dsRpt.Tables(0)
            If dt.Rows.Count > 0 Then
                With gridViewRpt
                    .DataSource = Nothing
                    .DataSource = dt
                    FormatGridColumns(gridViewRpt, False, True, True, False)
                    AutoResize2(gridViewRpt)
                    If rbtAmountRpt.Checked = True Then
                        .Columns("GRSWT").Visible = False
                        .Columns("STONEWT").Visible = False
                        .Columns("NETWT").Visible = False
                        .Columns("PUREWT").Visible = False
                    ElseIf rbtWeightRpt.Checked = True Then
                        .Columns("GRSWT").Visible = True
                        .Columns("STONEWT").Visible = True
                        .Columns("NETWT").Visible = True
                        .Columns("PUREWT").Visible = True
                        .Columns("AMOUNT").Visible = False
                        .Columns("TAX").Visible = False
                        .Columns("TDS").Visible = False
                    End If
                    .Columns("BATCHNO").Visible = False
                    .Columns("ACCODE").Visible = False
                    .Columns("RESULT").Visible = False
                    .Columns("COLHEAD").Visible = False
                    .Columns("MCHARGE").Visible = False
                    .Columns("BANKACNO").Visible = False
                    .Columns("BANKOTHERDET").Visible = False
                    .Columns("TRANSFERED").Visible = False
                    .Columns("CHK").Visible = False
                    .Columns("TYPE").Visible = False
                End With
            Else
                MsgBox("No Record found", MsgBoxStyle.Information)
                Exit Sub
            End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        Finally
            btnSearchRpt.Enabled = True
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Function funstrMetalName() As String
        Dim Qry As String = ""
        Qry = " SELECT METALID,METALNAME FROM ( "
        Qry += vbCrLf + " SELECT '0' METALID, 'ALL' METALNAME, 0 RESULT,0 DISPLAYORDER"
        Qry += vbCrLf + " UNION ALL"
        Qry += vbCrLf + " select METALID,METALNAME,1 RESULT,DISPLAYORDER from " & cnAdminDb & "..METALMAST"
        Qry += vbCrLf + " )X ORDER BY RESULT,DISPLAYORDER"
        Return Qry
    End Function


    Private Sub btnNewRpt_Click(sender As Object, e As EventArgs) Handles btnNewRpt.Click
        gridViewRpt.DataSource = Nothing
        dtpFrom.Value = GetServerDate()
        cmbMetalRPT_OWN.DataSource = Nothing
        dt = New DataTable
        strsql = funstrMetalName()
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            cmbMetalRPT_OWN.DataSource = Nothing
            cmbMetalRPT_OWN.DataSource = dt
            cmbMetalRPT_OWN.ValueMember = "METALID"
            cmbMetalRPT_OWN.DisplayMember = "METALNAME"
        End If
        LoadTrantypeReport(True, True, True)
        strsql = ""
        dt = New DataTable
    End Sub

    Private Sub btnExcelRpt_Click(sender As Object, e As EventArgs) Handles btnExcelRpt.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridViewRpt.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "MIMR " & Format(dtpFrom.Value.Date, "dd/MM/yyyy"), gridViewRpt, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub btnPrintRpt_Click(sender As Object, e As EventArgs) Handles btnPrintRpt.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridViewRpt.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "MIMR " & Format(dtpFrom.Value.Date, "dd/MM/yyyy"), gridViewRpt, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub btnExitRpt_Click(sender As Object, e As EventArgs) Handles btnExitRpt.Click
        Me.Close()
    End Sub

    Private Sub btnRTGSPaymentView_Click(sender As Object, e As EventArgs) Handles btnRTGSPaymentView.Click, RTGSToolStripMenuItem.Click
        btnNewRTGS_Click(Me, New System.EventArgs)
        tabGeneral.SelectedTab = tabRTGSPayment
    End Sub

#End Region

#Region " Tab 3"
    Private Sub btnExitRTGS_Click(sender As Object, e As EventArgs) Handles btnExitRTGS.Click
        Me.Close()
    End Sub

    Dim AppStartPath As String = Application.StartupPath
    Dim MIMRRTGSfileName As String = "MRRTGS"
    Dim objWriter As New System.IO.StreamWriter(AppStartPath & "\" & MIMRRTGSfileName & ".txt")

    Private Sub btnGenerateRTGS_Click(sender As Object, e As EventArgs) Handles btnGenerateRTGS.Click
        Try
            If MsgBox(" Are you sure Generate text File ", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2) = MsgBoxResult.No Then
                Exit Sub
            End If
            Dim dtSave As New DataTable
            dtSave = gridViewRTGS.DataSource
            If dtSave.Rows.Count > 0 Then
                Tran = Nothing
                Tran = cn.BeginTransaction
                For i As Integer = 0 To dtSave.Rows.Count - 1
                    With dtSave.Rows(i)
                        If .Item("chk").ToString = "" Then Continue For
                        If .Item("chk").ToString = "False" Then Continue For
                        If .Item("BATCHNO").ToString = "" Then Continue For
                        strsql = " UPDATE " & cnStockDb & "..RECEIPT SET TRANSFERED='Y'  "
                        strsql += vbCrLf + " WHERE TRANDATE='" & Format(.Item("TRANDATE"), "yyyy-MM-dd") & "'  "
                        strsql += vbCrLf + " AND BATCHNO = '" & .Item("BATCHNO") & "'"
                        cmd = New OleDbCommand(strsql, cn, Tran)
                        cmd.ExecuteNonQuery()
                    End With
                Next
                Tran.Commit()
                Tran = Nothing
                If System.IO.File.Exists(AppStartPath & "\" & MIMRRTGSfileName & ".txt") = True Then
                    System.IO.File.Create(AppStartPath & "\" & MIMRRTGSfileName & ".txt").Dispose()
                Else
                    System.IO.File.Create(AppStartPath & "\" & MIMRRTGSfileName & ".txt").Close()
                End If
                If True Then
                    If gridViewRTGS.Rows.Count > 0 Then
                        objWriter = New System.IO.StreamWriter(AppStartPath & "\" & MIMRRTGSfileName & ".txt")
                        Dim txt As String = String.Empty
                        For i As Integer = 0 To gridViewRTGS.Rows.Count - 1
                            With gridViewRTGS.Rows(i)
                                If .Cells("chk").Value.ToString = "" Then Continue For
                                If .Cells("chk").Value.ToString = "False" Then Continue For
                                If .Cells("BATCHNO").Value.ToString = "" Then Continue For
                                If Val(.Cells("TRANNO").Value.ToString) = 0 Then Continue For
                                txt += "SAR" & "_MR_" & .Cells("TRANNO").Value.ToString & "," _
                                                               & .Cells("ACNAME").Value.ToString & "," _
                                                               & .Cells("BANKACNO").Value.ToString & "," _
                                                               & .Cells("BANKOTHERDET").Value.ToString & "," _
                                                               & "02," _
                                                               & Val(.Cells("NETAMOUNT").Value.ToString) & "," _
                                                               & "" & Format(Now.Date, "yyyyMMdd") & ""
                                txt += vbCrLf
                            End With
                        Next
                        objWriter.WriteLine(txt)
                        objWriter.Close()
                        MsgBox("Generated MR Text File in serverPath " & vbCrLf & AppStartPath & "\" & MIMRRTGSfileName & ".txt", MsgBoxStyle.Information)
                    End If
                End If
                MsgBox("Generate ")
                btnNewRTGS_Click(Me, New System.EventArgs)
            End If
        Catch ex As Exception
            If Not Tran Is Nothing Then
                Tran.Rollback()
                Tran = Nothing
                MessageBox.Show(ex.ToString)
            Else
                MessageBox.Show(ex.ToString)
            End If
        End Try
    End Sub

    Private Sub btnNewRTGS_Click(sender As Object, e As EventArgs) Handles btnNewRTGS.Click
        dtpFromRTGS.Value = GetServerDate()
        gridViewRTGS.DataSource = Nothing
    End Sub

    Private Sub btnSearchRTGS_Click(sender As Object, e As EventArgs) Handles btnSearchRTGS.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        Try
            btnSearchRTGS.Enabled = False
            Me.Cursor = Cursors.WaitCursor
            gridViewRTGS.DataSource = Nothing
            strsql = ""
            strsql += vbCrLf + " EXEC " & cnAdminDb & "..[SP_RPT_MIMR_NEW] "
            strsql += vbCrLf + " @DBNAME ='" & cnStockDb & "'"
            strsql += vbCrLf + " ,@ADMINDB='" & cnAdminDb & "'"
            strsql += vbCrLf + " ,@FROMDATE='" & Format(dtpFromRTGS.Value.Date, "yyyy-MM-dd") & "'"
            strsql += vbCrLf + " ,@TODATE='" & Format(dtpFromRTGS.Value.Date, "yyyy-MM-dd") & "'"
            strsql += vbCrLf + " ,@COMPANYID=''"
            strsql += vbCrLf + " ,@COSTID =''"
            strsql += vbCrLf + " ,@VIEWTRANSFERD=''"
            strsql += vbCrLf + " ,@SYSTEMID='" & Environment.MachineName & "'"
            strsql += vbCrLf + " ,@GROUPBY='T'"
            strsql += vbCrLf + " ,@METALID=''"
            strsql += vbCrLf + " ,@TRANTYPE='RRE'"
            strsql += vbCrLf + " ,@TABLENAME=''"
            strsql += vbCrLf + " ,@PAYMODE=''"
            Dim dsRpt As New DataSet
            cmd = New OleDbCommand(strsql, cn)
            da = New OleDbDataAdapter(cmd)
            da.Fill(dsRpt)
            dt = New DataTable
            dt = dsRpt.Tables(0)
            If dt.Rows.Count > 0 Then
                With gridViewRTGS
                    .DataSource = Nothing
                    .DataSource = dt
                    FormatGridColumns(gridViewRTGS, False, True, True, False)
                    AutoResize2(gridViewRTGS)
                    .Columns("BATCHNO").Visible = False
                    .Columns("ACCODE").Visible = False
                    .Columns("RESULT").Visible = False
                    .Columns("COLHEAD").Visible = False
                    .Columns("MCHARGE").Visible = False
                    .Columns("CHK").ReadOnly = False
                    .Columns("TRANSFERED").Visible = False
                End With
            Else
                MsgBox("No Record found", MsgBoxStyle.Information)
                Exit Sub
            End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        Finally
            btnSearchRTGS.Enabled = True
            Me.Cursor = Cursors.Default
        End Try
    End Sub
#End Region

#Region "tab 4"
    Private Sub btnSearchDailyRpt_Click(sender As Object, e As EventArgs) Handles btnSearchDailyRpt.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        Try
            btnSearchDailyRpt.Enabled = False
            Me.Cursor = Cursors.WaitCursor
            gridviewDailyReport.DataSource = Nothing
            strsql = ""
            strsql += vbCrLf + " EXEC " & cnAdminDb & "..[SP_RPT_MIMR_NEW] "
            strsql += vbCrLf + " @DBNAME ='" & cnStockDb & "'"
            strsql += vbCrLf + " ,@ADMINDB='" & cnAdminDb & "'"
            strsql += vbCrLf + " ,@FROMDATE='" & Format(dtpFromDailyRpt.Value.Date, "yyyy-MM-dd") & "'"
            strsql += vbCrLf + " ,@TODATE='" & Format(dtpFromDailyRpt.Value.Date, "yyyy-MM-dd") & "'"
            strsql += vbCrLf + " ,@COMPANYID=''"
            strsql += vbCrLf + " ,@COSTID =''"
            strsql += vbCrLf + " ,@VIEWTRANSFERD=''"
            strsql += vbCrLf + " ,@SYSTEMID='" & Environment.MachineName & "'"
            strsql += vbCrLf + " ,@GROUPBY='C'"
            strsql += vbCrLf + " ,@METALID=''"
            strsql += vbCrLf + " ,@TRANTYPE=''"
            strsql += vbCrLf + " ,@TABLENAME=''"
            strsql += vbCrLf + " ,@PAYMODE=''"
            Dim dsRpt As New DataSet
            cmd = New OleDbCommand(strsql, cn)
            da = New OleDbDataAdapter(cmd)
            da.Fill(dsRpt)
            dt = New DataTable
            dt = dsRpt.Tables(0)
            If dt.Rows.Count > 0 Then
                With gridviewDailyReport
                    .DataSource = Nothing
                    .DataSource = dt
                    FormatGridColumns(gridviewDailyReport, False, True, True, False)
                    AutoResize2(gridviewDailyReport)
                    .Columns("BATCHNO").Visible = False
                    .Columns("ACCODE").Visible = False
                    .Columns("RESULT").Visible = False
                    .Columns("COLHEAD").Visible = False
                    .Columns("CATCODE").Visible = False
                    .Columns("CATNAME").Visible = False
                End With
            Else
                MsgBox("No Record found", MsgBoxStyle.Information)
                Exit Sub
            End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        Finally
            btnSearchDailyRpt.Enabled = True
            Me.Cursor = Cursors.Default
        End Try
    End Sub
    Private Sub btnNewDailyRpt_Click(sender As Object, e As EventArgs) Handles btnNewDailyRpt.Click
        dtpFromDailyRpt.Value = GetServerDate()
        gridviewDailyReport.DataSource = Nothing
    End Sub

    Private Sub btnExcelDailyRpt_Click(sender As Object, e As EventArgs) Handles btnExcelDailyRpt.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridviewDailyReport.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "MATERIAL RECEIPT/ISSUE " & Format(dtpFromDailyRpt.Value.Date, "dd/MM/yyyy"), gridviewDailyReport, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub
    Private Sub btnPrintDailyRpt_Click(sender As Object, e As EventArgs) Handles btnPrintDailyRpt.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridviewDailyReport.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "MATERIAL RECEIPT/ISSUE " & Format(dtpFromDailyRpt.Value.Date, "dd/MM/yyyy"), gridviewDailyReport, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub
    Private Sub btnExitDailyRpt_Click(sender As Object, e As EventArgs) Handles btnExitDailyRpt.Click
        Me.Close()
    End Sub

    Private Sub btnDailyRTGS_Click(sender As Object, e As EventArgs) Handles btnDailyRTGS.Click, ReportToolStripMenuItem.Click
        tabGeneral.SelectedTab = tabDailyReport
        btnNewDailyRpt_Click(Me, New System.EventArgs)
    End Sub

    Private Sub txtAddition_AMT_KeyDown(sender As Object, e As KeyEventArgs) Handles txtAddition_AMT.KeyDown
        If e.KeyCode = Keys.Enter Then
            funcReadonlyPurchasereturn()
        End If
    End Sub

    Private Sub rbtAmountRpt_CheckedChanged(sender As Object, e As EventArgs) Handles rbtAmountRpt.CheckedChanged
        gridViewRpt.DataSource = Nothing
    End Sub

    Private Sub rbtWeightRpt_CheckedChanged(sender As Object, e As EventArgs) Handles rbtWeightRpt.CheckedChanged
        gridViewRpt.DataSource = Nothing
    End Sub

    Private Sub rbtIssueRPT_CheckedChanged(sender As Object, e As EventArgs) Handles rbtIssueRPT.CheckedChanged
        gridViewRpt.DataSource = Nothing
        LoadTrantypeReport(True, False, True)
    End Sub

    Private Sub rbtReceiptRPT_CheckedChanged(sender As Object, e As EventArgs) Handles rbtReceiptRPT.CheckedChanged
        gridViewRpt.DataSource = Nothing
        LoadTrantypeReport(True, True, False)
    End Sub

    Private Sub rbtAllRPT_CheckedChanged(sender As Object, e As EventArgs) Handles rbtAllRPT.CheckedChanged
        gridViewRpt.DataSource = Nothing
        LoadTrantypeReport(True, True, True)
    End Sub
    Dim dtLoadExcelFile As DataTable
    Private Sub LoadDataFromExcel()
        If dtLoadExcelFile.Columns.Count > 0 Then
            Exit Sub
        End If
        dtLoadExcelFile.Columns.Add("ROWID", GetType(Integer))
        dtLoadExcelFile.Columns.Add("ITEMID", GetType(Integer))
        dtLoadExcelFile.Columns.Add("SUBITEMID", GetType(Integer))
        dtLoadExcelFile.Columns.Add("PCS", GetType(Integer))
        dtLoadExcelFile.Columns.Add("WEIGHT", GetType(Double))
        dtLoadExcelFile.Columns.Add("TCS", GetType(Double))
        dtLoadExcelFile.Columns("ROWID").AutoIncrement = True
        dtLoadExcelFile.Columns("ROWID").AutoIncrementSeed = 1
        dtLoadExcelFile.Columns("ROWID").AutoIncrementStep = 1
    End Sub
    Private Sub btnUpload_Click(sender As Object, e As EventArgs) Handles btnUpload.Click
        If cmbTrantype.Text = "PURCHASE" Or cmbTrantype.Text = "PURCHASE RETURN" Then
            If Gridview_OWN.Rows.Count > 0 Then
                'MsgBox("Already Loaded...", MsgBoxStyle.Information)
                'Exit Sub
            End If
            If cmbAcname_OWN.Text = "" Then
                MsgBox("Acname should not empty", MsgBoxStyle.Information)
                cmbAcname_OWN.Focus()
                cmbAcname_OWN.SelectAll()
                Exit Sub
            End If
            If Val(txtRate_RATE.Text) > 0 Then
                Dim OpenDialog As New OpenFileDialog
                Dim Str As String
                Str = "(*.xls) | *.xls"
                Str += "|(*.xlsx) | *.xlsx"
                Str += "|(*.csv) | *.csv"
                If OpenDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
                    Dim path As String = OpenDialog.FileName
                    dtLoadExcelFile = New DataTable
                    LoadDataFromExcel()
                    If path <> "" Then
                        LoadFromExel(path)
                    End If
                End If
            Else
                MsgBox("Enter the Rate ", MsgBoxStyle.Information)
            End If
        End If
    End Sub
    Private Sub LoadFromExel(ByVal Path As String)
        Try
            '.csv format
            Me.Cursor = Cursors.WaitCursor
            dtLoadExcelFile = New DataTable
            LoadDataFromExcel()
            Dim TextLine As String = ""
            Dim SplitLine() As String
            If System.IO.File.Exists(Path) = True Then
                Dim objReader As New System.IO.StreamReader(Path, System.Text.Encoding.UTF8)
                Do While objReader.Peek() <> -1
                    SplitLine = Nothing
                    TextLine = objReader.ReadLine()
                    SplitLine = Split(TextLine, ",")
                    If SplitLine.Length > 0 Then
                        If SplitLine.Length = 1 Then Continue Do
                        If SplitLine(0).ToString.Trim = "" Then Continue Do
                        If SplitLine(0).ToString.Trim = "SNO" Then Continue Do
                        If Val(SplitLine(1).ToString.Trim) = 0 Then Continue Do
                        Dim drItemmast As DataRow = Nothing
                        strsql = ""
                        strsql += vbCrLf + "        SELECT "
                        strsql += vbCrLf + "        ITEMID"
                        strsql += vbCrLf + "        ,SUBITEMID"
                        strsql += vbCrLf + "        ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = A.ITEMID) ITEMNAME"
                        strsql += vbCrLf + "        ,SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST AS A "
                        strsql += vbCrLf + "        WHERE SUBITEMID = " & SplitLine(2).ToString.Trim & " "
                        strsql += vbCrLf + "        AND ITEMID = " & SplitLine(1).ToString.Trim & ""
                        drItemmast = GetSqlRow(strsql, cn)
                        If Not drItemmast Is Nothing Then
                            cmbItem_OWN.Text = drItemmast.Item("ITEMNAME").ToString
                            cmbSubItem_OWN.Text = drItemmast.Item("SUBITEMNAME").ToString
                        Else
                            Continue Do
                        End If
                        'cmbItem_OWN.Text = SplitLine(3).ToString.Trim
                        'cmbSubItem_OWN.Text = SplitLine(4).ToString.Trim
                        txtRate_RATE.Text = Val(objMIMRRate.txtNewRate_RATE.Text)
                        txtPcs_NUM.Text = Val(SplitLine(6).ToString.Trim)
                        txtGrsWt_WET.Text = Val(SplitLine(7).ToString.Trim)
                        txtTouchper_AMT.Text = "100.00"
                        txtPurityper_RATE.Text = "100.00"
                        txtMCPerGram_RATE.Text = Val(SplitLine(5).ToString.Trim)
                        CalcOMcPercentage()
                        funcReadonlyPurchasereturn()
                        'TCS
                        objGSTTDS = New frmGSTGSTTDS(False, 0, "", "", 0)
                        objGSTTDS.txtNetAmount_AMT.Text = Val(txtGrossAmt_AMT.Text)
                        objGSTTDS.txtGstAmount.Text = Val(txtCGSTVAL_AMT.Text) + Val(txtSGSTVAL_AMT.Text) + Val(txtIGSTVAL_AMT.Text)
                        objGSTTDS.txtTCSPer_PER.Text = Val(SplitLine(9).ToString.Trim) ' objGSTTDS.Show()
                        objGSTTDS.tdsval(Val(SplitLine(9).ToString.Trim))
                        'TCS
                        funcNetValueTcs()
                        CalcOMcPercentage()
                        funcReadonlyPurchasereturn()
                        funcAddGrid()

                        'objGSTTDS.btnOk_Click(Me, New System.EventArgs)
                        'objGSTTDS.btnCancel_Click(Me, New System.EventArgs)
                        'objGSTTDS.Hide()
                        'objGSTTDS.Dispose()
                        'objGSTTDS.BeginInvoke(New MethodInvoker(AddressOf objGSTTDS.Close)) : Exit Sub

                        'Dim dr As DataRow = Nothing
                        'dr = dtLoadExcelFile.NewRow
                        'dr!ITEMID = 0
                        'dr!SUBITEMID = 0
                        'dr!PCS = 0
                        'dr!WEIGHT = 0
                        'dtLoadExcelFile.Rows.Add(dr)
                    End If
                Loop
            Else
                MsgBox("File Does Not Exist")
                Exit Sub
            End If
            If dtLoadExcelFile.Rows.Count > 0 Then

            End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub
    Private Sub btnAcnameShortCut_Click(sender As Object, e As EventArgs) Handles btnAcnameShortCut.Click, SupplierReportToolStripMenuItem.Click
        btnNewRpt_Click(Me, New System.EventArgs)
        tabGeneral.SelectedTab = tabOpen
    End Sub

#End Region

#Region " Tab Page 5"

    Private Function GetSelectedStoredProducer(ByVal chkLst As BrighttechPack.CheckedComboBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Items.Count > 0 Then
            For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
                If WithQuotes Then retStr += "''"
                Dim val() As String = chkLst.CheckedItems.Item(cnt).ToString.Split("-")
                If val.Length = 1 Then
                    retStr += val(0)
                Else
                    retStr += val(1)
                End If
                If WithQuotes Then retStr += "''"
                If cnt <> chkLst.CheckedItems.Count - 1 Then
                    retStr += ","
                End If
            Next
        Else
            retStr = "''"
        End If
        Return retStr
    End Function

    Private Function GetSelectedMetalId(ByVal chkLst As BrighttechPack.CheckedComboBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Items.Count > 0 Then
            For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
                If WithQuotes Then retStr += "''"
                retStr += objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME= '" & chkLst.CheckedItems.Item(cnt).ToString & "'")
                If WithQuotes Then retStr += "''"
                If cnt <> chkLst.CheckedItems.Count - 1 Then
                    retStr += ","
                End If
            Next
        Else
            retStr = "''"
        End If
        Return retStr
    End Function

    Private Sub btnSearchAcname_Click(sender As Object, e As EventArgs) Handles btnSearchAcname.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        Try
            Dim TrantypeShorName As String = GetSelectedStoredProducer(chkCmbTrantype, True)
            Dim strMetalName As String = GetSelectedMetalId(chkCmbMetalName, True)

            btnSearchAcname.Enabled = False
            Me.Cursor = Cursors.WaitCursor
            strsql = ""
            strsql += vbCrLf + "        EXEC " & cnAdminDb & "..[SP_RPT_MIMR_NEW_ACNAME]"
            strsql += vbCrLf + "        @DBNAME ='" & cnStockDb & "'"
            strsql += vbCrLf + "        ,@ADMINDB ='" & cnAdminDb & "'"
            strsql += vbCrLf + "        ,@FROMDATE = '" & Format(dtpFromAcName.Value.Date, "yyyy-MM-dd") & "'"
            strsql += vbCrLf + "        ,@TODATE = '" & Format(dtpToAcname.Value.Date, "yyyy-MM-dd") & "'"
            strsql += vbCrLf + "        ,@SYSTEMID ='" & systemId & "'"
            If strMetalName = "ALL" Then
                strsql += vbCrLf + "    ,@METALID =''"
            ElseIf strMetalName = "" Or strMetalName = "''" Or strMetalName = "''''" Then
                strsql += vbCrLf + "    ,@METALID =''"
            Else
                strsql += vbCrLf + "    ,@METALID ='" & strMetalName & "'"
            End If
            If cmbAcnameAcname_Own.Text = "" Or cmbAcnameAcname_Own.Text = "ALL" Then
                strsql += vbCrLf + "        ,@ACCODE =''"
            Else
                strsql += vbCrLf + "        ,@ACCODE ='" & cmbAcnameAcname_Own.SelectedValue.ToString & "'"
            End If
            If TrantypeShorName = "ALL" Or TrantypeShorName = "" Or TrantypeShorName = "''" Or TrantypeShorName = "''ALL''" Then
                strsql += vbCrLf + "        ,@TRANTYPE =''"
            Else
                strsql += vbCrLf + "        , @TRANTYPE = '" & TrantypeShorName & "' "
            End If
            strsql += vbCrLf + "        , @BATCHNO = '' "
            If chkWithCancel.Checked = True Then
                strsql += vbCrLf + "        ,@WITHCANCEL = 'Y'"
            Else
                strsql += vbCrLf + "        ,@WITHCANCEL = ''"
            End If
            Dim ds As New DataSet
            Dim dt As New DataTable
            cmd = New OleDbCommand(strsql, cn)
            da = New OleDbDataAdapter(cmd)
            da.Fill(ds)
            dt = ds.Tables(0)
            If dt.Rows.Count > 0 Then
                With gridViewAcname_OWN
                    .DataSource = Nothing
                    .DataSource = dt
                    FormatGridColumns(gridViewAcname_OWN, False, True, True, False)
                    AutoResizeColumnWidth(gridViewAcname_OWN, "TRANTYPENAME")
                    .Columns("ACCODE").Visible = False
                    .Columns("BATCHNO").Visible = False
                    .Columns("TRANTYPE").Visible = False
                    .Columns("ACNAME").Visible = False
                    .Columns("RECISS").Visible = False
                    .Columns("RESULT").Visible = False
                    .Columns("COLHEAD").Visible = False
                    .Columns("RECISS").Visible = False
                    .Columns("RECISSH").Visible = False
                    .Columns("CANCEL").Visible = False
                    For i As Integer = 0 To .Rows.Count - 1
                        If .Rows(i).Cells("COLHEAD").Value.ToString = "T" Then
                            .Rows(i).Cells("TRANTYPENAME").Style.BackColor = Color.LightSkyBlue
                            '.Rows(i).DefaultCellStyle.BackColor = Color.LightPink
                        ElseIf .Rows(i).Cells("COLHEAD").Value.ToString = "S" Then
                            .Rows(i).DefaultCellStyle.BackColor = Color.LightPink
                        ElseIf .Rows(i).Cells("COLHEAD").Value.ToString = "G" Then
                            .Rows(i).DefaultCellStyle.BackColor = Color.LightBlue
                        End If
                    Next
                End With
            Else
                MsgBox("No Record found", MsgBoxStyle.Information)
                gridViewAcname_OWN.DataSource = Nothing
            End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        Finally
            btnSearchAcname.Enabled = True
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub btnNewAcname_Click(sender As Object, e As EventArgs) Handles btnNewAcname.Click
        dtpFromAcName.Value = GetServerDate()
        dtpToAcname.Value = GetServerDate()
        gridViewAcname_OWN.DataSource = Nothing
        dt = New DataTable
        strsql = funstrMetalName()
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dt)
        chkCmbMetalName.Items.Clear()
        If dt.Rows.Count > 0 Then
            BrighttechPack.GlobalMethods.FillCombo(chkCmbMetalName, dt, "METALNAME", , "ALL")
        End If
        dt = New DataTable

        cmbAcnameAcname_Own.DataSource = Nothing
        LoadAcnameSupplier()

        strsql = LoadTratypePrefixStrsqlChecked(True, True, True)
        Dim dtTrantypeCheck As New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dtTrantypeCheck)
        If dtTrantypeCheck.Rows.Count > 0 Then
            BrighttechPack.GlobalMethods.FillCombo(chkCmbTrantype, dtTrantypeCheck, "TRANTYPE", , "ALL")
        End If
        objOtherUpdate = New frmMIMRUpdate(GetServerDate(), "")
        objChangeAcname = New frmMIMRAcNameChange(InclCusttype)
    End Sub

    Private Sub btnExcelAcname_Click(sender As Object, e As EventArgs) Handles btnExcelAcname.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridViewAcname_OWN.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "MATERIAL RECEIPT/ISSUE ACNAME " & Format(dtpFromAcName.Value.Date, "dd/MM/yyyy"), gridViewAcname_OWN, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub btnPrintAcname_Click(sender As Object, e As EventArgs) Handles btnPrintAcname.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridViewAcname_OWN.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "MATERIAL RECEIPT/ISSUE ACNAME " & Format(dtpFromAcName.Value.Date, "dd/MM/yyyy"), gridViewAcname_OWN, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub btnExitAcname_Click(sender As Object, e As EventArgs) Handles btnExitAcname.Click
        Me.Close()
    End Sub

    Private Sub funAcnameChangepresss(ByVal Trandate As String, ByVal batchno As String, ByVal oldAccode As String, ByVal newaccode As String)
        Try
            Tran = Nothing
            Tran = cn.BeginTransaction

            strsql = ""

            strsql = " UPDATE " & cnStockDb & "..RECEIPT SET ACCODE = '" & newaccode & "' "
            strsql += vbCrLf + " WHERE TRANDATE='" & Trandate & "' "
            strsql += vbCrLf + " AND BATCHNO='" & batchno & "'"
            strsql += vbCrLf + " AND ISNULL(CANCEL,'')=''"
            cmd = New OleDbCommand(strsql, cn, Tran)
            cmd.ExecuteNonQuery()

            strsql = " UPDATE " & cnStockDb & "..ISSUE SET ACCODE = '" & newaccode & "' "
            strsql += vbCrLf + " WHERE TRANDATE='" & Trandate & "' "
            strsql += vbCrLf + " AND BATCHNO='" & batchno & "'"
            strsql += vbCrLf + " AND ISNULL(CANCEL,'')=''"
            cmd = New OleDbCommand(strsql, cn, Tran)
            cmd.ExecuteNonQuery()

            strsql = " UPDATE " & cnStockDb & "..ACCTRAN SET ACCODE = '" & newaccode & "' "
            strsql += vbCrLf + " WHERE TRANDATE='" & Trandate & "' "
            strsql += vbCrLf + " AND BATCHNO='" & batchno & "' "
            strsql += vbCrLf + " AND ACCODE='" & oldAccode & "'"
            strsql += vbCrLf + " AND ISNULL(CANCEL,'')=''"
            cmd = New OleDbCommand(strsql, cn, Tran)
            cmd.ExecuteNonQuery()

            strsql = " UPDATE " & cnStockDb & "..ACCTRAN SET CONTRA='" & newaccode & "' "
            strsql += vbCrLf + " WHERE TRANDATE='" & Trandate & "' "
            strsql += vbCrLf + " AND BATCHNO='" & batchno & "' "
            strsql += vbCrLf + " AND CONTRA='" & oldAccode & "'"
            cmd = New OleDbCommand(strsql, cn, Tran)
            cmd.ExecuteNonQuery()

            strsql = ""

            Tran.Commit()
            Tran = Nothing
            MsgBox("Changed Successfully", MsgBoxStyle.Information)
            btnNewAcname_Click(Me, New System.EventArgs)
        Catch ex As Exception
            If Not Tran Is Nothing Then
                Tran.Rollback()
                Tran = Nothing
                MessageBox.Show(ex.ToString())
            Else
                MessageBox.Show(ex.ToString())
            End If
        End Try
    End Sub

    Private Function funOTPNAMECHANGE(ByVal batchno As String, ByVal oldAcname As String, ByVal newAcname As String) As Boolean
        Dim drSmsTemplate As DataRow = Nothing
        strsql = "select * from " & cnAdminDb & "..SMSTEMPLATE where TEMPLATE_NAME ='TAG_REPLACE_OLD_NEW'"
        drSmsTemplate = GetSqlRow(strsql, cn)
        If drSmsTemplate.Item("ACTIVE").ToString = "Y" Then
            'Dim Optionid As Integer = 0
            'Dim pwdid As Integer = 0 : Dim pwdpass As Boolean = False
            'Dim Sqlqry As String = "SELECT OPTIONID FROM " & cnAdminDb & "..PRJPWDOPTION WHERE OPTIONNAME ='TAG_REPLACE_OLD_NEW' AND ACTIVE = 'Y'"
            'Optionid = Val("" & objGPack.GetSqlValue(Sqlqry, , , Tran))
            'If Optionid = 0 Then pwdpass = False
            'If userId <> 0 Then '999
            '    If Optionid <> 0 Then
            '        Dim chkOTp As Integer = 0
            '        chkOTp = objGPack.GetSqlValue("SELECT PWDID FROM " & cnAdminDb & "..PWDMASTER WHERE PWDOPTIONID = " & Optionid & " AND  PWDUSERID = " & userId & " AND ISNULL(PWDSTATUS,'') <> 'C'", , Nothing)
            '        If chkOTp = 0 Then
            '            MsgBox("Access Denied", MsgBoxStyle.Information)
            '            If MsgBox("Do you want to Send OTP to Mobile?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            '                If SMS_OTP_MIMR_DUPLICATEPRINT = "" Then MsgBox("OTP No. Should not empty", MsgBoxStyle.Information) : Return False
            '                Dim getMobile() As String = SMS_OTP_MIMR_DUPLICATEPRINT.Split(",")
            '                Dim otpnumber As String = setUserPwd(Optionid, 1, 4, batchno, getMobile(0).ToString, "MIMR")
            '                pwdid = GetuserPwd(Optionid, cnCostId, userId)
            '                Dim msgcontent As String = drSmsTemplate.Item("TEMPLATE_DESC")
            '                msgcontent = msgcontent.Replace("<OTPNO>", otpnumber)
            '                msgcontent = msgcontent.Replace("<OLDTAGNO>", oldAcname)
            '                msgcontent = msgcontent.Replace("<NEWTAGNO>", newAcname)
            '                msgcontent = msgcontent.Replace("<username>", cnUserName)
            '                msgcontent = msgcontent.Replace("<systemname>", Environment.MachineName)
            '                msgcontent = msgcontent.Replace("<date>", Format(Now, "dd/MM/yyyy"))
            '                msgcontent = msgcontent.Replace("<time>", Date.Now.ToLongTimeString)
            '                For i As Integer = 0 To getMobile.Length - 1
            '                    SmsSend(msgcontent, getMobile(i))
            '                Next
            '                Return False
            '            End If
            '        Else
            '            Dim objUpwd As New frmUserPassword(chkOTp)
            '            objUpwd.PasswordGenerate()
            '            objUpwd.lblUsrPwd.Visible = False
            '            If objUpwd.ShowDialog <> Windows.Forms.DialogResult.OK Then
            '                Return False
            '            End If
            '            Return True
            '        End If
            '    End If
            'End If
        Else
            Return True
        End If
    End Function


    Private Sub gridViewAcname_OWN_KeyDown(sender As Object, e As KeyEventArgs) Handles gridViewAcname_OWN.KeyDown
        If gridViewAcname_OWN.Rows.Count = 0 Then Exit Sub
        Try
            If e.KeyCode = Keys.D Then
                Me.Cursor = Cursors.WaitCursor
                If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
                With gridViewAcname_OWN.CurrentRow
                    If .Cells("BATCHNO").Value.ToString = "" Or .Cells("TRANDATE").Value.ToString = "" Then
                        MsgBox("Invalid Type", MsgBoxStyle.Information)
                        Exit Sub
                    End If
                    'Dim drSmsTemplate As DataRow = Nothing
                    'strsql = "select * from " & cnAdminDb & "..SMSTEMPLATE where TEMPLATE_NAME ='SMS_MIMR_DUPLICATE'"
                    'drSmsTemplate = GetSqlRow(strsql, cn)
                    'If Not drSmsTemplate Is Nothing Then
                    '    If drSmsTemplate.Item("ACTIVE").ToString = "Y" Then
                    '        'Dim Optionid As Integer = 0
                    '        'Dim pwdid As Integer = 0 : Dim pwdpass As Boolean = False
                    '        'Dim Sqlqry As String = "SELECT OPTIONID FROM " & cnAdminDb & "..PRJPWDOPTION WHERE OPTIONNAME ='MIMR_DUPPRINT' AND ACTIVE = 'Y'"
                    '        'Optionid = Val("" & objGPack.GetSqlValue(Sqlqry, , , Tran))
                    '        'If Optionid = 0 Then pwdpass = False
                    '        'If userId <> 0 Then '999
                    '        '    If Optionid <> 0 Then
                    '        '        Dim chkOTp As Integer = 0
                    '        '        chkOTp = objGPack.GetSqlValue("SELECT PWDID FROM " & cnAdminDb & "..PWDMASTER WHERE PWDOPTIONID = " & Optionid & " AND  PWDUSERID = " & userId & " AND ISNULL(PWDSTATUS,'') <> 'C'", , Nothing)
                    '        '        If chkOTp = 0 Then
                    '        '            MsgBox("Access Denied", MsgBoxStyle.Information)
                    '        '            If MsgBox("Do you want to Send OTP to Mobile?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                    '        '                If SMS_OTP_MIMR_DUPLICATEPRINT = "" Then MsgBox("OTP No. Should not empty", MsgBoxStyle.Information) : Exit Sub
                    '        '                Dim getMobile() As String = SMS_OTP_MIMR_DUPLICATEPRINT.Split(",")
                    '        '                Dim otpnumber As String = setUserPwd(Optionid, 1, 4, .Cells("BATCHNO").Value.ToString, getMobile(0).ToString, "MIMR")
                    '        '                pwdid = GetuserPwd(Optionid, cnCostId, userId)
                    '        '                Dim msgcontent As String = drSmsTemplate.Item("TEMPLATE_DESC")
                    '        '                msgcontent = msgcontent.Replace("<OTPNO>", otpnumber)
                    '        '                msgcontent = msgcontent.Replace("<TRANDATE>", Format(.Cells("TRANDATE").Value, "dd/MM/yyyy"))
                    '        '                msgcontent = msgcontent.Replace("<TRANNO>", .Cells("TRANNO").Value)
                    '        '                msgcontent = msgcontent.Replace("<ACNAME>", .Cells("ACNAME").Value)
                    '        '                msgcontent = msgcontent.Replace("<USERNAME>", cnUserName)
                    '        '                msgcontent = msgcontent.Replace("<SYSTEMNAME>", Environment.MachineName)
                    '        '                msgcontent = msgcontent.Replace("<DATE>", Format(Now, "dd/MM/yyyy"))
                    '        '                msgcontent = msgcontent.Replace("<TIME>", Date.Now.ToLongTimeString)
                    '        '                msgcontent = msgcontent.Replace("<COSTNAME>", cnCostName)
                    '        '                For i As Integer = 0 To getMobile.Length - 1
                    '        '                    SmsSend(msgcontent, getMobile(i))
                    '        '                Next
                    '        '                Exit Sub
                    '        '            End If
                    '        '        Else
                    '        '            Dim objUpwd As New frmUserPassword(chkOTp)
                    '        '            objUpwd.PasswordGenerate()
                    '        '            objUpwd.lblUsrPwd.Visible = False
                    '        '            If objUpwd.ShowDialog <> Windows.Forms.DialogResult.OK Then
                    '        '                Exit Sub
                    '        '            End If
                    '        '            'IIf(.Cells("TYPE").Value.ToString = "I", "SMI", "SMR")
                    '        '            'Dim objBill As New frmBillPrintDoc("SMI", .Cells("BATCHNO").Value.ToString, Format(.Cells("TRANDATE").Value, "yyyy-MM-dd"), "Y", "NEW")
                    '        '        End If
                    '        '    End If
                    '        'End If
                    '    Else
                    '        'IIf(.Cells("TYPE").Value.ToString = "I", "SMI", "SMR")
                    '        'Dim objBill As New frmBillPrintDoc("SMI", .Cells("BATCHNO").Value.ToString, Format(.Cells("TRANDATE").Value, "yyyy-MM-dd"), "Y", "NEW")
                    '    End If
                    'Else
                    '    'IIf(.Cells("TYPE").Value.ToString = "I", "SMI", "SMR")
                    '    'Dim objBill As New frmBillPrintDoc("SMI", .Cells("BATCHNO").Value.ToString, Format(.Cells("TRANDATE").Value, "yyyy-MM-dd"), "Y", "NEW")
                    'End If
                    Dim pBatchno As String = .Cells("BATCHNO").Value.ToString
                    Dim pBillDate As String = Format(.Cells("TRANDATE").Value, "yyyy-MM-dd").ToString
                    If GetAdmindbSoftValue("MIMR_PRINT", "N") = "Y" Then 'oMaterial = Material.Receipt
                        'final 10June22
                        Dim objBill As New frmBillPrintDoc_RSR("SMR", pBatchno, pBillDate, "N", "NEW")
                    Else
                        Dim prnmemsuffix As String = ""
                        If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId
                        If IO.File.Exists(Application.StartupPath & "\BillPrint.exe") Then
                            Dim write As IO.StreamWriter
                            If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId
                            Dim memfile As String = "\BillPrint" + prnmemsuffix.Trim & ".mem"
                            write = IO.File.CreateText(Application.StartupPath & memfile)
                            write.WriteLine(LSet("TYPE", 15) & ":SMIMR")
                            write.WriteLine(LSet("BATCHNO", 15) & ":" & pBatchno)
                            write.WriteLine(LSet("TRANDATE", 15) & ":" & pBillDate.ToString)
                            write.WriteLine(LSet("DUPLICATE", 15) & ":N")
                            write.Flush()
                            write.Close()
                            If EXE_WITH_PARAM = False Then
                                System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe")
                            Else
                                System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe",
                            LSet("TYPE", 15) & ":SMIMR" & ";" &
                            LSet("BATCHNO", 15) & ":" & pBatchno & ";" &
                            LSet("TRANDATE", 15) & ":" & pBillDate.ToString & ";" &
                            LSet("DUPLICATE", 15) & ":N")
                            End If
                        Else
                            MsgBox("Billprint exe not found", MsgBoxStyle.Information)
                        End If
                    End If
                End With
            End If
            '** PRESS U Update Invoice No
            If e.KeyCode = Keys.U Then
                Me.Cursor = Cursors.WaitCursor
                If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Edit) Then Exit Sub
                Dim InvDate As Date
                Dim InvNo As String = ""
                With gridViewAcname_OWN.CurrentRow
                    If .Cells("BATCHNO").Value.ToString = "" Or .Cells("TRANDATE").Value.ToString = "" Then
                        MsgBox("Invalid Type", MsgBoxStyle.Information)
                        Exit Sub
                    End If
                    InvDate = .Cells("REFDATE").Value
                    InvNo = .Cells("REFNO").Value.ToString
                    'If PaymentDetExist(.Cells("BATCHNO").Value.ToString, "Yes") = True Then
                    '    Exit Sub
                    'End If
                End With
                objOtherUpdate = New frmMIMRUpdate(InvDate, InvNo)
                If objOtherUpdate.ShowDialog = DialogResult.OK Then
                    InvDate = objOtherUpdate.dtpFrom.Value.Date
                    InvNo = objOtherUpdate.txtInvoiceNo.Text.Trim
                    Try
                        Me.Cursor = Cursors.WaitCursor
                        Tran = Nothing
                        Tran = cn.BeginTransaction
                        With gridViewAcname_OWN.CurrentRow
                            strsql = " UPDATE " & cnStockDb & "..RECEIPT SET "
                            strsql += vbCrLf + "  REFNO = '" & InvNo & "', REFDATE='" & Format(InvDate, "yyyy-MM-dd") & "' WHERE  "
                            strsql += vbCrLf + " TRANDATE='" & Format(.Cells("TRANDATE").Value, "yyyy-MM-dd") & "'  "
                            strsql += vbCrLf + " AND BATCHNO='" & .Cells("BATCHNO").Value.ToString & "' "
                            strsql += vbCrLf + " AND COMPANYID='" & strCompanyId & "' "
                            cmd = New OleDbCommand(strsql, cn, Tran)
                            cmd.ExecuteNonQuery()

                            strsql = " UPDATE " & cnStockDb & "..ISSUE SET   "
                            strsql += vbCrLf + " REFNO = '" & InvNo & "', REFDATE='" & Format(InvDate, "yyyy-MM-dd") & "' WHERE  "
                            strsql += vbCrLf + " TRANDATE='" & Format(.Cells("TRANDATE").Value, "yyyy-MM-dd") & "'  "
                            strsql += vbCrLf + " AND BATCHNO='" & .Cells("BATCHNO").Value.ToString & "' "
                            strsql += vbCrLf + " AND COMPANYID='" & strCompanyId & "' "
                            cmd = New OleDbCommand(strsql, cn, Tran)
                            cmd.ExecuteNonQuery()

                            strsql = " UPDATE " & cnStockDb & "..ACCTRAN SET "
                            strsql += vbCrLf + " REFNO = '" & InvNo & "', REFDATE='" & Format(InvDate, "yyyy-MM-dd") & "' WHERE  "
                            strsql += vbCrLf + " TRANDATE='" & Format(.Cells("TRANDATE").Value, "yyyy-MM-dd") & "'  "
                            strsql += vbCrLf + " AND BATCHNO='" & .Cells("BATCHNO").Value.ToString & "' "
                            strsql += vbCrLf + " AND COMPANYID='" & strCompanyId & "' "
                            cmd = New OleDbCommand(strsql, cn, Tran)
                            cmd.ExecuteNonQuery()
                        End With
                        Tran.Commit()
                        Tran = Nothing
                        btnNewAcname_Click(Me, New System.EventArgs)
                        MsgBox("Update Successfully", MsgBoxStyle.Information)
                    Catch ex As Exception
                        If Not Tran Is Nothing Then
                            Tran.Rollback()
                            Tran = Nothing
                            MessageBox.Show(ex.ToString)
                        Else
                            MessageBox.Show(ex.ToString)
                        End If
                    Finally
                        Me.Cursor = Cursors.Default
                    End Try
                End If
            End If
            '***''
            If e.KeyCode = Keys.C Then
                Me.Cursor = Cursors.WaitCursor
                If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Cancel) Then Exit Sub
                Dim objSecret As New frmAdminPassword()
                If objSecret.ShowDialog <> Windows.Forms.DialogResult.OK Then
                    Exit Sub
                End If
                With gridViewAcname_OWN.CurrentRow
                    If .Cells("BATCHNO").Value.ToString = "" _
                        Or .Cells("TRANDATE").Value.ToString = "" Then
                        MsgBox("Invalid Type", MsgBoxStyle.Information)
                        Exit Sub
                    End If

                    'If PaymentDetExist(.Cells("BATCHNO").Value.ToString) = True Then
                    '    Exit Sub
                    'End If

                    Dim drSmsTemplate As DataRow = Nothing
                    strsql = "select * from " & cnAdminDb & "..SMSTEMPLATE where TEMPLATE_NAME ='SMS_MIMR_CANCEL'"
                    drSmsTemplate = GetSqlRow(strsql, cn)
                    If Not drSmsTemplate Is Nothing Then
                        If drSmsTemplate.Item("ACTIVE").ToString = "Y" Then
                            Dim Optionid As Integer = 0
                            Dim pwdid As Integer = 0 : Dim pwdpass As Boolean = False
                            Dim Sqlqry As String = "SELECT OPTIONID FROM " & cnAdminDb & "..PRJPWDOPTION WHERE OPTIONNAME ='MIMR_CANCEL' AND ACTIVE = 'Y'"
                            Optionid = Val("" & objGPack.GetSqlValue(Sqlqry, , , Tran))
                            If Optionid = 0 Then pwdpass = False
                            If userId <> 0 Then '999
                                If Optionid <> 0 Then
                                    Dim chkOTp As Integer = 0
                                    'chkOTp = objGPack.GetSqlValue("SELECT PWDID FROM " & cnAdminDb & "..PWDMASTER WHERE PWDOPTIONID = " & Optionid & " AND  PWDUSERID = " & userId & " AND ISNULL(PWDSTATUS,'') <> 'C'", , Nothing)
                                    If chkOTp = 0 Then
                                        'MsgBox("Access Denied", MsgBoxStyle.Information)
                                        'If MsgBox("Do you want to Send OTP to Mobile?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                                        '    If SMS_OTP_MIMR_CANCEL = "" Then
                                        '        SMS_OTP_MIMR_CANCEL = SMS_OTP_MIMR_DUPLICATEPRINT
                                        '    End If
                                        '    If SMS_OTP_MIMR_CANCEL = "" Then MsgBox("OTP No. Should not empty", MsgBoxStyle.Information) : Exit Sub
                                        '    Dim getMobile() As String = SMS_OTP_MIMR_CANCEL.Split(",")
                                        '    Dim otpnumber As String = setUserPwd(Optionid, 1, 4, .Cells("BATCHNO").Value.ToString, getMobile(0).ToString, "MIMR")
                                        '    pwdid = GetuserPwd(Optionid, cnCostId, userId)
                                        '    Dim msgcontent As String = drSmsTemplate.Item("TEMPLATE_DESC")
                                        '    msgcontent = msgcontent.Replace("<OTPNO>", otpnumber)
                                        '    msgcontent = msgcontent.Replace("<TRANDATE>", Format(.Cells("TRANDATE").Value, "dd/MM/yyyy"))
                                        '    msgcontent = msgcontent.Replace("<TRANNO>", .Cells("TRANNO").Value)
                                        '    msgcontent = msgcontent.Replace("<ACNAME>", .Cells("ACNAME").Value)
                                        '    msgcontent = msgcontent.Replace("<USERNAME>", cnUserName)
                                        '    msgcontent = msgcontent.Replace("<SYSTEMNAME>", Environment.MachineName)
                                        '    msgcontent = msgcontent.Replace("<DATE>", Format(Now, "dd/MM/yyyy"))
                                        '    msgcontent = msgcontent.Replace("<TIME>", Date.Now.ToLongTimeString)
                                        '    msgcontent = msgcontent.Replace("<COSTNAME>", cnCostName)
                                        '    For i As Integer = 0 To getMobile.Length - 1
                                        '        SmsSend(msgcontent, getMobile(i))
                                        '    Next
                                        '    Exit Sub
                                        'End If
                                    Else
                                        'Dim objUpwd As New frmUserPassword(chkOTp)
                                        'objUpwd.PasswordGenerate()
                                        'objUpwd.lblUsrPwd.Visible = False
                                        'If objUpwd.ShowDialog <> Windows.Forms.DialogResult.OK Then
                                        '    Exit Sub
                                        'End If
                                        Try
                                            If gridViewAcname_OWN.CurrentRow.Cells("BATCHNO").Value.ToString = "" _
                                                Or gridViewAcname_OWN.CurrentRow.Cells("TRANDATE").Value.ToString = "" Then
                                                MsgBox("Invalid Type", MsgBoxStyle.Information)
                                                Exit Sub
                                            End If
                                            Me.Cursor = Cursors.WaitCursor
                                            Tran = Nothing
                                            Tran = cn.BeginTransaction
                                            With gridViewAcname_OWN.CurrentRow

                                                strsql = " UPDATE " & cnStockDb & "..RECEIPT SET CANCEL ='Y' WHERE  "
                                                strsql += vbCrLf + " TRANDATE='" & Format(.Cells("TRANDATE").Value, "yyyy-MM-dd") & "'  "
                                                strsql += vbCrLf + " AND BATCHNO='" & .Cells("BATCHNO").Value.ToString & "' "
                                                strsql += vbCrLf + " AND COMPANYID='" & strCompanyId & "' "
                                                cmd = New OleDbCommand(strsql, cn, Tran)
                                                cmd.ExecuteNonQuery()


                                                strsql = " UPDATE " & cnStockDb & "..ISSUE SET CANCEL ='Y' WHERE  "
                                                strsql += vbCrLf + " TRANDATE='" & Format(.Cells("TRANDATE").Value, "yyyy-MM-dd") & "'  "
                                                strsql += vbCrLf + " AND BATCHNO='" & .Cells("BATCHNO").Value.ToString & "' "
                                                strsql += vbCrLf + " AND COMPANYID='" & strCompanyId & "' "
                                                cmd = New OleDbCommand(strsql, cn, Tran)
                                                cmd.ExecuteNonQuery()

                                                strsql = " UPDATE " & cnStockDb & "..ACCTRAN SET CANCEL ='Y' WHERE  "
                                                strsql += vbCrLf + " TRANDATE='" & Format(.Cells("TRANDATE").Value, "yyyy-MM-dd") & "'  "
                                                strsql += vbCrLf + " AND BATCHNO='" & .Cells("BATCHNO").Value.ToString & "' "
                                                strsql += vbCrLf + " AND COMPANYID='" & strCompanyId & "' "
                                                cmd = New OleDbCommand(strsql, cn, Tran)
                                                cmd.ExecuteNonQuery()

                                                strsql = ""
                                                strsql = ""
                                                strsql += vbCrLf + " UPDATE " & cnAdminDb & "..ITEMTAG  "
                                                strsql += vbCrLf + " SET ISSDATE = NULL "
                                                strsql += vbCrLf + " ,ISSREFNO = 0 "
                                                strsql += vbCrLf + " ,ISSPCS = 0 "
                                                strsql += vbCrLf + " ,ISSWT = 0 "
                                                strsql += vbCrLf + " ,TOFLAG = '' "
                                                strsql += vbCrLf + " ,BATCHNO = '' "
                                                strsql += vbCrLf + " WHERE BATCHNO = '" & (.Cells("BATCHNO").Value.ToString.Trim) & "' "
                                                cmd = New OleDbCommand(strsql, cn, Tran)
                                                cmd.ExecuteNonQuery()

                                                strsql = ""
                                                strsql = " UPDATE " & cnStockDb & "..ESTISSUE SET BATCHNO='' "
                                                strsql += vbCrLf + " WHERE BATCHNO='" & .Cells("BATCHNO").Value.ToString.Trim & "'"
                                                cmd = New OleDbCommand(strsql, cn, Tran)
                                                cmd.ExecuteNonQuery()

                                                strsql = ""
                                                Tran.Commit()
                                                Tran = Nothing
                                            End With
                                            btnNewAcname_Click(Me, New System.EventArgs)
                                            MsgBox("Cancelled Successfully", MsgBoxStyle.Information)
                                        Catch ex As Exception
                                            If Not Tran Is Nothing Then
                                                Tran.Rollback()
                                                Tran = Nothing
                                                MessageBox.Show(ex.ToString)
                                            Else
                                                MessageBox.Show(ex.ToString)
                                            End If
                                        Finally
                                            Me.Cursor = Cursors.Default
                                        End Try
                                    End If
                                End If
                            End If
                        Else
                            MsgBox("Access Denied")
                        End If
                    End If
                End With
            End If
            '***''
            If e.KeyCode = Keys.I Then
                Me.Cursor = Cursors.WaitCursor
                If MsgBox("Search Details ", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                    With gridViewAcname_OWN.CurrentRow
                        If .Cells("BATCHNO").Value.ToString = "" Or .Cells("TRANDATE").Value.ToString = "" Then
                            MsgBox("Invalid Type", MsgBoxStyle.Information)
                            Exit Sub
                        End If
                        If userId = 64 Then
                            'If PaymentDetExist(.Cells("BATCHNO").Value.ToString, "Yes") = True Then
                            '    Exit Sub
                            'End If
                        Else
                            'If PaymentDetExist(.Cells("BATCHNO").Value.ToString, "No") = True Then
                            '    Exit Sub
                            'End If
                        End If
                        Dim obj As New frmMIMRAcnameWiseDetailReport(.Cells("BATCHNO").Value.ToString, .Cells("ACNAME").Value.ToString)
                        obj.ShowDialog()
                    End With
                End If
            End If
            '**Supplier Name Change***' 
            If e.KeyCode = Keys.S Then
                Dim objSecret As New frmAdminPassword()
                If objSecret.ShowDialog <> Windows.Forms.DialogResult.OK Then
                    Exit Sub
                End If
                If gridViewAcname_OWN.CurrentRow.Cells("BATCHNO").Value.ToString = "" Then
                    MsgBox("Invalid Batchno ", MsgBoxStyle.Information)
                    Exit Sub
                End If
                If gridViewAcname_OWN.Columns.Contains("BATCHNO") = False Then
                    MsgBox("Batchno not found", MsgBoxStyle.Information)
                    Exit Sub
                End If
                objChangeAcname.cmbOldAcName.Text = gridViewAcname_OWN.CurrentRow.Cells("ACNAME").Value.ToString
                If objChangeAcname.ShowDialog = Windows.Forms.DialogResult.OK Then
                    With gridViewAcname_OWN.CurrentRow
                        'If PaymentDetExist(.Cells("BATCHNO").Value.ToString, "YES") = True Then 'NO VERY IMPORTANT REMOVE ON 02Feb2022
                        '    Exit Sub
                        'End If
                        'If funOTPNAMECHANGE(.Cells("BATCHNO").Value.ToString, objChangeAcname.cmbOldAcName.Text, objChangeAcname.cmbNewAcName.Text) = True Then
                        '    Dim Tdate As String = Format(.Cells("TRANDATE").Value, "yyyy-MM-dd")
                        '    Dim TBathno As String = .Cells("BATCHNO").Value.ToString
                        '    funAcnameChangepresss(Tdate, TBathno, objChangeAcname.cmbOldAcName.SelectedValue.ToString, objChangeAcname.cmbNewAcName.SelectedValue.ToString)
                        'End If
                    End With
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub
    Private Sub btnPrintAcnameS2_Click(sender As Object, e As EventArgs) Handles btnPrintAcnameS2.Click
        If gridViewAcname_OWN.Rows.Count > 0 Then
            Dim dtPrint As New DataTable
            dtPrint = gridViewAcname_OWN.DataSource
            Dim objPrint As New frmAccountMIMRPrintDesign(dtPrint)
        End If
    End Sub

    Private Sub txtLessWt_WET_Leave(sender As Object, e As EventArgs) Handles txtLessWt_WET.Leave
        If Val(txtGrsWt_WET.Text) <> 0 Then
            'If Val(txtLessWt_WET.Text) = 0 Then
            '    Dim dtNewStone As DataTable
            '    strsql = funcdealerStudedinfo()
            '    da = New OleDbDataAdapter(strsql, cn)
            '    dtNewStone = New DataTable
            '    da.Fill(dtNewStone)
            '    If dtNewStone.Rows.Count > 0 Then
            '        If objStone.gridStone.Rows.Count > 0 Then
            '            Dim dtCheckStone As New DataTable
            '            dtCheckStone = objStone.gridStone.DataSource
            '            Dim StnAmt As Double = Val(dtCheckStone.Compute("SUM(AMOUNT)", "").ToString)
            '            If StnAmt = 0 Then
            '                MsgBox("Studded detail to be entered", MsgBoxStyle.Information)
            '                txtLessWt_WET.Focus()
            '                txtLessWt_WET.SelectAll()
            '            End If
            '        End If
            '    End If
            'End If
            If Val(txtLessWt_WET.Text) = 0 Then
                If Val(txtItemId_NUM.Text) > 0 Then
                    If cmbAcname_OWN.Text.Trim <> "" Then
                        Dim dtGrid As New DataTable("SUMMARY")
                        strsql = " "
                        strsql += vbCrLf + " SELECT ITEMID"
                        strsql += vbCrLf + " ,SUBITEMID"
                        strsql += vbCrLf + " ,STNITEMID"
                        strsql += vbCrLf + " ,STNSUBITEMID"
                        strsql += vbCrLf + " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = A.ITEMID) ITEMNAME"
                        strsql += vbCrLf + " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = A.ITEMID AND SUBITEMID =A.SUBITEMID) SUBITEMNAME"
                        strsql += vbCrLf + " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = A.STNITEMID) STNITEMNAME"
                        strsql += vbCrLf + " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = A.STNITEMID AND SUBITEMID =A.STNSUBITEMID) STNSUBITEMNAME"
                        strsql += vbCrLf + " ,PSTNRATE"
                        strsql += vbCrLf + " ,CALCMODE"
                        strsql += vbCrLf + " ,STONEUNIT "
                        strsql += vbCrLf + " FROM " & cnAdminDb & "..DEALER_STUDDED  AS A"
                        strsql += vbCrLf + " WHERE ACCODE='" & cmbAcname_OWN.SelectedValue.ToString & "' "
                        strsql += vbCrLf + " And ITEMID = " & Val(txtItemId_NUM.Text) & ""
                        strsql += vbCrLf + " AND SUBITEMID=" & cmbSubItem_OWN.SelectedValue.ToString & ""
                        da = New OleDbDataAdapter(strsql, cn)
                        da.Fill(dtGrid)
                        If dtGrid.Rows.Count > 0 Then
                            Dim objGridShower As frmGridDispDia
                            objGridShower = New frmGridDispDia
                            objGridShower.Name = Me.Name
                            objGridShower.gridView.RowTemplate.Height = 21
                            objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
                            objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                            objGridShower.Size = New Size(778, 550)
                            Dim tit As String = "DEALER_STUDDED DETAIL"
                            objGridShower.Text = tit
                            objGridShower.lblTitle.Text = tit
                            objGridShower.StartPosition = FormStartPosition.CenterScreen
                            objGridShower.dsGrid.DataSetName = objGridShower.Name
                            objGridShower.dsGrid.Tables.Add(dtGrid)
                            objGridShower.gridView.DataSource = Nothing
                            objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
                            objGridShower.gridView.Columns("ITEMID").Visible = False
                            objGridShower.gridView.Columns("SUBITEMID").Visible = False
                            objGridShower.gridView.Columns("STNITEMID").Visible = False
                            objGridShower.gridView.Columns("STNSUBITEMID").Visible = False
                            objGridShower.lblTitle.Text = ""
                            objGridShower.FormReLocation = False
                            objGridShower.pnlFooter.Visible = True
                            objGridShower.pnlFooter.BorderStyle = BorderStyle.Fixed3D
                            objGridShower.pnlFooter.Size = New Size(objGridShower.pnlFooter.Width, 20)
                            objGridShower.formuser = userId
                            objGridShower.Show()
                        End If
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub gridViewAcname_OWN_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles gridViewAcname_OWN.CellFormatting
        With gridViewAcname_OWN
            If .Rows.Count > 0 Then
                If .Columns.Contains("CANCEL") Then
                    If .Rows(e.RowIndex).Cells("CANCEL").Value.ToString = "Y" Then
                        .Rows(e.RowIndex).DefaultCellStyle.BackColor = Color.Red
                    End If
                End If
            End If
        End With
    End Sub

    Private Sub funcdroptemptabledbMimrdebitcreditnote()
        strsql = " IF OBJECT_ID('TEMPTABLEDB..[TEMPMIMRDEBITCREDITNOTE" & systemId & "]','U') "
        strsql += vbCrLf + " IS NOT NULL DROP TABLE TEMPTABLEDB..[TEMPMIMRDEBITCREDITNOTE" & systemId & "]"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()
    End Sub

    Private Sub btnCD_Click(sender As Object, e As EventArgs) Handles btnCD.Click
        If cmbTrantype.Text = "PURCHASE" Then
            If Gridview_OWN.Rows.Count = 0 Then
                If objMIMRCd.ShowDialog = Windows.Forms.DialogResult.OK Then
                    If Val(objMIMRCd.txtNewRateInclusive_RATE.Text) = 0 Then
                        MsgBox("Enter the Rate", MsgBoxStyle.Information)
                        objMIMRCd.txtNewRateInclusive_RATE.Focus()
                        objMIMRCd.txtNewRateInclusive_RATE.SelectAll()
                        Exit Sub
                    End If
                    Dim drTdsOld As DataRow = Nothing
                    Dim dtViewcd As New DataTable
                    Dim dtViewcdGrp As New DataTable

                    funcdroptemptabledbMimrdebitcreditnote()
                    strsql = " "
                    strsql += vbCrLf + " SELECT "
                    strsql += vbCrLf + " A.TRANSTATUS"
                    strsql += vbCrLf + " ,A.PCS"
                    strsql += vbCrLf + " ,A.GRSWT "
                    strsql += vbCrLf + " ,A.NETWT "
                    strsql += vbCrLf + " ,A.LESSWT "
                    strsql += vbCrLf + " ,A.GRSNET "
                    strsql += vbCrLf + " ,A.ITEMID"
                    strsql += vbCrLf + " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=  A.ITEMID) ITEMNAME"
                    strsql += vbCrLf + " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID=  A.ITEMID And SUBITEMID = A.SUBITEMID) SUBITEMNAME"
                    strsql += vbCrLf + " ,A.SUBITEMID"
                    strsql += vbCrLf + " ,A.TOUCH "
                    strsql += vbCrLf + " ,A.PURITY "
                    strsql += vbCrLf + " ,A.WASTPER "
                    strsql += vbCrLf + " ,A.MCGRM "
                    'CONFUSION VARIABLE
                    strsql += vbCrLf + " ,A.MCHARGE"
                    strsql += vbCrLf + " ,A.MCPIE"
                    strsql += vbCrLf + " ,A.RATE "
                    strsql += vbCrLf + " ,ISNULL((SELECT SUM(STNAMT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE BATCHNO=A.BATCHNO And ISSSNO = A.SNO),0) STNAMT"
                    strsql += vbCrLf + " ,A.AMOUNT "

                    strsql += vbCrLf + " ,ISNULL((SELECT SUM(TAXAMOUNT) FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO=A.BATCHNO And ISSSNO = A.SNO And TAXID='CG'),0) CGSTVAL"
                    strsql += vbCrLf + " ,ISNULL((SELECT (TAXPER) FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO=A.BATCHNO AND ISSSNO = A.SNO AND TAXID='CG'),0) CGSTPER"

                    strsql += vbCrLf + " ,ISNULL((SELECT SUM(TAXAMOUNT) FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO=A.BATCHNO AND ISSSNO = A.SNO AND TAXID='SG'),0) SGSTVAL"
                    strsql += vbCrLf + " ,ISNULL((SELECT (TAXPER) FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO=A.BATCHNO AND ISSSNO = A.SNO AND TAXID='SG'),0) SGSTPER"

                    strsql += vbCrLf + " ,ISNULL((SELECT SUM(TAXAMOUNT) FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO=A.BATCHNO AND ISSSNO = A.SNO AND TAXID='IG'),0) IGSTVAL"
                    strsql += vbCrLf + " ,ISNULL((SELECT (TAXPER) FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO=A.BATCHNO AND ISSSNO = A.SNO AND TAXID='IG'),0) IGSTPER"

                    strsql += vbCrLf + " ,A.TAX "
                    strsql += vbCrLf + " ,A.BATCHNO "
                    strsql += vbCrLf + " ,A.SNO "
                    strsql += vbCrLf + " ,A.CATCODE "
                    strsql += vbCrLf + " ,C.PURCHASEID "
                    strsql += vbCrLf + " ,C.P_CGSTID "
                    strsql += vbCrLf + " ,C.P_SGSTID "
                    strsql += vbCrLf + " ,C.P_IGSTID "
                    strsql += vbCrLf + " INTO TEMPTABLEDB..[TEMPMIMRDEBITCREDITNOTE" & systemId & "] "
                    strsql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT AS A "
                    strsql += vbCrLf + " , " & cnAdminDb & "..ACHEAD AS B"
                    strsql += vbCrLf + " , " & cnAdminDb & "..CATEGORY AS C"
                    strsql += vbCrLf + " WHERE A.ACCODE = B.ACCODE "
                    strsql += vbCrLf + " AND A.CATCODE = C.CATCODE"
                    strsql += vbCrLf + " And A.TRANDATE='" & Format(objMIMRCd.dtpTrandate.Value.Date, "yyyy-MM-dd") & "' "
                    strsql += vbCrLf + " AND A.TRANNO = " & objMIMRCd.txtInvoiceNo.Text.Trim & ""
                    strsql += vbCrLf + " And ISNULL(CANCEL,'')= ''"
                    strsql += vbCrLf + " AND A.TRANTYPE = 'RPU'"
                    strsql += vbCrLf + " And A.ACCODE='" & cmbAcname_OWN.SelectedValue.ToString & "'"
                    strsql += vbCrLf + " And ISNULL(A.SALEMODE,0) = 1 "
                    strsql += vbCrLf + " AND A.COMPANYID = '" & strCompanyId & "'"
                    cmd = New OleDbCommand(strsql, cn)
                    cmd.ExecuteNonQuery()
                    strsql = " SELECT * FROM TEMPTABLEDB..[TEMPMIMRDEBITCREDITNOTE" & systemId & "]"
                    da = New OleDbDataAdapter(strsql, cn)
                    dtViewcd = New DataTable
                    da.Fill(dtViewcd)
                    Dim tdsper As Double = 0
                    If dtViewcd.Rows.Count > 0 Then
                        For i As Integer = 0 To dtViewcd.Rows.Count - 1
                            With dtViewcd.Rows(i)
                                If .Item("TRANSTATUS").ToString = "" Then
                                    MsgBox("Not an UNFIXED Item's")
                                    Continue For
                                End If
                                If .Item("TRANSTATUS").ToString = "F" Then
                                    MsgBox("Not an UNFIXED Item's")
                                    Continue For
                                End If
                                If .Item("TRANSTATUS").ToString = "U" Then
                                    cmbTrantype.Text = "PURCHASE"
                                    txtItemId_NUM.Text = .Item("ITEMID").ToString
                                    cmbItem_OWN.Text = .Item("ITEMNAME").ToString
                                    cmbSubItem_OWN.Text = .Item("SUBITEMNAME").ToString
                                    txtRate_RATE.Text = Val(objMIMRCd.txtNewRateInclusive_RATE.Text)

                                    txtPcs_NUM.Text = Val(.Item("PCS").ToString)

                                    txtGrsWt_WET.Text = Val(.Item("GRSWT").ToString)
                                    txtLessWt_WET.Text = Val(.Item("LESSWT").ToString)
                                    txtNetWt_WET.Text = Val(.Item("NETWT").ToString)

                                    If .Item("GRSNET").ToString = "G" Then
                                        cmbGrsNet.Text = "GRS WT"
                                    Else
                                        cmbGrsNet.Text = "NET WT"
                                    End If
                                    txtTouchper_AMT.Text = Val(.Item("TOUCH").ToString)
                                    txtPurityper_RATE.Text = Val(.Item("PURITY").ToString)
                                    txtwastageper_AMT.Text = Val(.Item("WASTPER").ToString)
                                    txtMCPPer_RATE.Text = Val(.Item("MCGRM").ToString)
                                    txtMCPerGram_RATE.Text = "0"
                                    'txtMCPieces_AMT.Text = Val(.Item("MCHARGE").ToString)
                                    'txtMC_AMT.Text = Val(.Item("MCPIE").ToString)
                                    txtStoneValue_AMT.Text = Val(.Item("STNAMT").ToString)
                                    CalcOWastage()
                                    CalcOMcPercentage()
                                    CalcOPureWt()
                                    funcReadonlyPurchasereturn()
                                    'TCS
                                    objGSTTDS = New frmGSTGSTTDS(False, 0, "", "", 0)
                                    objGSTTDS.txtNetAmount_AMT.Text = Val(txtGrossAmt_AMT.Text)
                                    objGSTTDS.txtGstAmount.Text = Val(txtCGSTVAL_AMT.Text) + Val(txtSGSTVAL_AMT.Text) + Val(txtIGSTVAL_AMT.Text)
                                    strsql = " select TAXPER,TAXAMOUNT from " & cnStockDb & "..TAXTRAN where BATCHNO ='" & .Item("BATCHNO").ToString & "' and TAXID = 'TD'"
                                    drTdsOld = GetSqlRow(strsql, cn)
                                    tdsper = 0
                                    If Not drTdsOld Is Nothing Then
                                        tdsper = Val(drTdsOld.Item("TAXPER").ToString)
                                    End If
                                    objGSTTDS.txtTCSPer_PER.Text = Val(tdsper)
                                    objGSTTDS.tdsval(Val(tdsper))
                                    CalcOWastage()
                                    CalcOMcPercentage()
                                    CalcOPureWt()
                                    CalcOGrossAmt()
                                    funcReadonlyPurchasereturn()
                                    funcAddGrid()
                                End If
                            End With
                        Next
                        With objMIMRCd
                            'INVOICE VALUE
                            .lblCGSTPer.Text = Format(Val(dtViewcd.Compute("SUM(CGSTPER)", "").ToString), "0.00")
                            .lblSGSTPer.Text = Format(Val(dtViewcd.Compute("SUM(SGSTPER)", "").ToString), "0.00")
                            .lblIGSTPer.Text = Format(Val(dtViewcd.Compute("SUM(IGSTPER)", "").ToString), "0.00")
                            .lblTDSPer.Text = tdsper
                            .txtInValue.Text = Format(Val(dtViewcd.Compute("SUM(AMOUNT)", "").ToString), "0.00")
                            .txtInCGST.Text = Format(Val(dtViewcd.Compute("SUM(CGSTVAL)", "").ToString), "0.00")
                            .txtInSGST.Text = Format(Val(dtViewcd.Compute("SUM(SGSTVAL)", "").ToString), "0.00")
                            .txtInIGST.Text = Format(Val(dtViewcd.Compute("SUM(IGSTVAL)", "").ToString), "0.00")
                            If Not drTdsOld Is Nothing Then
                                .txtInTDS.Text = Format(Val(drTdsOld.Item("TAXAMOUNT").ToString), "0.00")
                            Else
                                .txtInTDS.Text = "0.00"
                            End If
                            .txtInNet.Text = Format(Val(.txtInValue.Text) + Val(.txtInCGST.Text) + Val(.txtInSGST.Text) + Val(.txtInIGST.Text) - Val(.txtInTDS.Text), "0.00")
                            'CURRENT VALUE
                            Dim dtGridViewcd As New DataTable
                            dtGridViewcd = Gridview_OWN.DataSource
                            .txtCuValue.Text = Format(Val(dtGridViewcd.Compute("SUM(GROSSAMT)", "").ToString), "0.00")
                            .txtCuCGST.Text = Format(Val(dtGridViewcd.Compute("SUM(CGST)", "").ToString), "0.00")
                            .txtCuSGST.Text = Format(Val(dtGridViewcd.Compute("SUM(SGST)", "").ToString), "0.00")
                            .txtCuIGST.Text = Format(Val(dtGridViewcd.Compute("SUM(IGST)", "").ToString), "0.00")
                            .txtCuTDS.Text = Format(Val(txtConTDS_AMT.Text), "0.00")
                            .txtCuNet.Text = Format(Val(.txtCuValue.Text) + Val(.txtCuCGST.Text) + Val(.txtCuSGST.Text) + Val(.txtCuIGST.Text) - Val(.txtCuTDS.Text), "0.00")
                            'CD VALUE
                            .txtCDValue.Text = Format(Val(.txtInValue.Text) - Val(.txtCuValue.Text), "0.00")
                            .txtCDCGst.Text = Format((Val(.txtInCGST.Text)) - (Val(.txtCuCGST.Text)), "0.00")
                            .txtCDSGst.Text = Format((Val(.txtInSGST.Text)) - (Val(.txtCuSGST.Text)), "0.00")
                            .txtCDIGst.Text = Format((Val(.txtInIGST.Text)) - (Val(.txtCuIGST.Text)), "0.00")
                            If Val(.txtCDNet.Text) <= 0 Then
                                .lblCNoteOrDNote.Text = "CREDIT NOTE" 'TDS MUST
                                .lblTDSPer.Text = tdsper
                                .txtcdTDS.Text = Format(Math.Ceiling(Val(.txtCDValue.Text) * tdsper / 100), "0.00")
                            Else
                                .lblCNoteOrDNote.Text = "DEBIT NOTE"
                                .txtcdTDS.Text = "0.00"
                            End If
                            .txtCDNet.Text = Format((Val(.txtCDValue.Text) + Val(.txtCDCGst.Text) + Val(.txtCDSGst.Text) + Val(.txtCDIGst.Text)) - Val(.txtcdTDS.Text), "0.00")
                            'COMPLETED

                            Dim dtDistinctRatecode As New DataTable
                            dtDistinctRatecode = Gridview_OWN.DataSource
                            dtDistinctRatecode = dtDistinctRatecode.DefaultView.ToTable(True, "RATEFIXACCODE")
                            If dtDistinctRatecode.Rows.Count > 0 Then
                                strsql = ""
                                If dtDistinctRatecode.Rows.Count > 1 Then
                                    MsgBox("Multi Accode should not allowed", MsgBoxStyle.Information)
                                    btnNew_Click(Me, New System.EventArgs)
                                    Exit Sub
                                Else
                                    .lblRateFixedAccode.Text = dtDistinctRatecode.Rows(0).Item("RATEFIXACCODE").ToString
                                    strsql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = '" & dtDistinctRatecode.Rows(0).Item("RATEFIXACCODE").ToString & "'"
                                    .lblRateFixedAcName.Text = objGPack.GetSqlValue(strsql).ToString
                                    strsql = ""
                                End If
                            End If
                        End With
                        strsql = ""
                    Else
                        MsgBox("No Record found", MsgBoxStyle.Information)
                    End If
                    If objMIMRCd.ShowDialog = Windows.Forms.DialogResult.OK Then
                        btnNew_Click(Me, New System.EventArgs)
                    Else
                        btnNew_Click(Me, New System.EventArgs)
                    End If
                Else
                    btnNew_Click(Me, New System.EventArgs)
                End If
                strsql = ""
            Else
                btnNew_Click(Me, New System.EventArgs)
            End If
        End If
    End Sub
    Private Sub funclearESTVariable()
        ESTTAGNO = ""
        ESTSNO = ""
        ESTBATCHNO = ""
        ITEMTAGPCS = 0
        ITEMTAGGRSWT = 0
        ITEMTAGNETWT = 0
    End Sub

    Private Sub btnReturnEst_Click(sender As Object, e As EventArgs) Handles btnReturnEst.Click
        If funEstButtonVisible() = True Then
            If cmbAcname_OWN.Text = "" Then
                MsgBox("Acname should not empty", MsgBoxStyle.Information)
                cmbAcname_OWN.Focus()
                cmbAcname_OWN.SelectAll()
                Exit Sub
            End If
            objMIMRfrmEst.MIMRAccode = cmbAcname_OWN.SelectedValue.ToString
            If Val(txtRate_RATE.Text) > 0 Then
                If objMIMRfrmEst.ShowDialog = Windows.Forms.DialogResult.OK Then
                    funclearESTVariable()
                    For i As Integer = 0 To objMIMRfrmEst.gridView.Rows.Count - 1
                        With objMIMRfrmEst.gridView.Rows(i)
                            If Val(.Cells("ITEMID").Value.ToString) = 0 Then Continue For
                            txtItemId_NUM.Text = .Cells("ITEMID").Value.ToString
                            cmbItem_OWN.Text = .Cells("ITEMNAME").Value.ToString
                            cmbSubItem_OWN.Text = .Cells("SUBITEMNAME").Value.ToString
                            txtPcs_NUM.Text = .Cells("PCS").Value.ToString
                            txtGrsWt_WET.Text = .Cells("GRSWT").Value.ToString
                            ShowStoneDia(.Cells("TRANNO").Value.ToString, Format(.Cells("TRANDATE").Value, "yyyy-MM-dd"), .Cells("TAGNO").Value.ToString)
                            txtNetWt_WET.Text = .Cells("NETWT").Value.ToString
                            cmbGrsNet.Text = .Cells("GRSNET").Value.ToString
                            txtRate_RATE.Text = Val(objMIMRRate.txtNewRate_RATE.Text)
                            txtTouchper_AMT.Text = .Cells("TOUCH").Value.ToString
                            txtPurityper_RATE.Text = .Cells("PUREWT").Value.ToString
                            txtwastageper_AMT.Text = .Cells("WASTPER").Value.ToString
                            If Val(.Cells("MCPER").Value.ToString) > 0 Then
                                txtMCPPer_RATE.Text = .Cells("MCPER").Value.ToString
                            End If
                            If Val(.Cells("MCGRM").Value.ToString) > 0 Then
                                txtMCPerGram_RATE.Text = .Cells("MCGRM").Value.ToString
                            End If
                            If Val(.Cells("MCPIE").Value.ToString) > 0 Then
                                txtMCPieces_AMT.Text = .Cells("MCPIE").Value.ToString
                            End If
                            txtRemark1.Text = .Cells("TAGNO").Value.ToString
                            txtRemark2.Text = "" & .Cells("TRANNO").Value.ToString & "," & Format(.Cells("TRANDATE").Value, "yyyy-MM-dd")
                            ESTTAGNO = .Cells("TAGNO").Value.ToString
                            ESTSNO = .Cells("SNO").Value.ToString
                            ESTBATCHNO = .Cells("ESTBATCHNO").Value.ToString
                            ITEMTAGPCS = Val(.Cells("TAGPCS").Value.ToString)
                            ITEMTAGGRSWT = Val(.Cells("TAGGRSWT").Value.ToString)
                            ITEMTAGNETWT = Val(.Cells("TAGNETWT").Value.ToString)
                            CalcOMcPercentage()
                            funcReadonlyPurchasereturn()
                            'TCS
                            objGSTTDS = New frmGSTGSTTDS(False, 0, "", "", 0)
                            objGSTTDS.txtNetAmount_AMT.Text = Val(txtGrossAmt_AMT.Text)
                            objGSTTDS.txtGstAmount.Text = Val(txtCGSTVAL_AMT.Text) + Val(txtSGSTVAL_AMT.Text) + Val(txtIGSTVAL_AMT.Text)
                            'objGSTTDS.txtTCSPer_PER.Text = Val(SplitLine(9).ToString.Trim) ' objGSTTDS.Show()
                            'objGSTTDS.tdsval(Val(SplitLine(9).ToString.Trim))
                            'TCS
                            funcNetValueTcs()
                            CalcOMcPercentage()
                            funcReadonlyPurchasereturn()
                            funcAddGrid()
                        End With
                    Next
                End If
            Else
                MsgBox("Enter the Rate ", MsgBoxStyle.Information)
                Exit Sub
            End If
        End If
    End Sub

    Private Sub txtInvoiceNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtInvoiceNo.KeyPress
        If textInvoiceCharacterNotallowed(sender, e) = True Then
            MsgBox("Not Allowed Special Character", MsgBoxStyle.Information)
            e.Handled = True
            Exit Sub
        End If
    End Sub
#End Region
End Class