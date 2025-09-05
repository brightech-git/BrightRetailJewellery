Imports System.Data.OleDb
Imports System.Math
Public Class frmTagExcelDown
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

    Private Cmd As OleDbCommand
    Private Da As OleDbDataAdapter
    Private DtTran As New DataTable
    'Private dtGridStone As New DataTable
    Private StrSql As String
    Public objStone As New frmStoneDiaAc
    Dim objCheaque As New frmChequeAdj
    Dim objAddlCharge As New AddlChargesDia
    Dim objtag As New TagGeneration
    Dim Transistno As Long = 0
    Dim TranNo As Integer = Nothing

    Dim tagnos As String = ""
    Dim BatchNo As String = Nothing
    Dim CostCenterId As String = Nothing
    Dim _AccAudit As Boolean = IIf(GetAdmindbSoftValue("ACC_AUDIT", "N") = "Y", True, False)
    Dim InclCusttype As String = GetAdmindbSoftValue("INCL_CUSTOMER_ISSREC", "N")
    Dim SALVALUEROUND As Decimal = Val(GetAdmindbSoftValue("STKSALVALUEROUND", "0"))

    Dim TdsPer As Decimal = Nothing
    Dim _Accode As String = Nothing
    Dim _Acctype As String = Nothing
    Dim CASHID As String = Nothing
    Dim BANKID As String = Nothing
    Dim Remark1 As String = Nothing
    Dim Remark2 As String = Nothing
    Dim lotNo As Integer = 0
    Dim Maxwastper As Decimal = 0
    Dim Maxwastage As Decimal = 0
    Dim MaxmcGrm As Decimal = 0
    Dim Maxmc As Decimal = 0
    Dim CalMode As String
    Dim grsnet As String = ""
    Dim designerid As Integer = 0
    Dim dtTAGPrint As New DataTable
    Dim ListAllowEmptyValues As New List(Of String)
    Dim Trantype As String = "RPU"
    Dim POS_TAGXLDOWN_TAGNO As Boolean = IIf(GetAdmindbSoftValue("POS_TAGXLDOWN", "T") = "T", True, False)



    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
        objGPack.TextClear(Me)
        Initializer()
        ListAllowEmptyValues.Add("TAGNO")
        ListAllowEmptyValues.Add("ITEM")
        'ListAllowEmptyValues.Add("SUBITEM")
        'ListAllowEmptyValues.Add("WT")
        'ListAllowEmptyValues.Add("QTY")
        'ListAllowEmptyValues.Add("RETAILPRICE")
        'ListAllowEmptyValues.Add("DISCOUNT")
        'ListAllowEmptyValues.Add("PURVALUE")
        'ListAllowEmptyValues.Add("ITEMCTRNAME")

    End Sub

    Private Sub Initializer()
        CASHID = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'CASH'", , "CASH")
        BANKID = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'BANK'", , "BANK")
        With DtTran
            .Columns.Add("SNO", GetType(Integer))
            .Columns.Add("STYLENO", GetType(String))
            .Columns.Add("CERTNO", GetType(String))
            .Columns.Add("ITEM", GetType(String))
            .Columns.Add("SUBITEM", GetType(String))
            .Columns.Add("PURITY", GetType(Decimal))
            .Columns.Add("PCS", GetType(Integer))
            .Columns.Add("GRSWT", GetType(Decimal))
            .Columns.Add("NETWT", GetType(Decimal))
            .Columns.Add("WASTPER", GetType(Decimal))
            .Columns.Add("RATE", GetType(Decimal))
            .Columns.Add("MCGRM", GetType(Decimal))
            .Columns.Add("AMOUNT", GetType(Decimal))
            .Columns.Add("SALEAMOUNT", GetType(Decimal))
            .Columns.Add("SIZE", GetType(String))
            .Columns.Add("CUT", GetType(String))
            .Columns.Add("COLOR", GetType(String))
            .Columns.Add("CLARITY", GetType(String))
            .Columns.Add("SHAPE", GetType(String))
            .Columns.Add("ITEMTYPE", GetType(String))
            .Columns.Add("DESIGNERNAME", GetType(String))
            .Columns.Add("COLHEAD", GetType(String))
            .Columns.Add("KEYNO", GetType(Integer))
            .Columns.Add("NO", GetType(Integer))
            .Columns.Add("REMARKS", GetType(String))
            .Columns("KEYNO").AutoIncrement = True
        End With

        DgvTran.Columns.Clear()
        DgvTran.DataSource = DtTran
        ClearTran()
        GridStyle(DgvTran)
        DgvTran.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 9, FontStyle.Bold)
        'DgvTran.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)

        Dim DtTranTotal As New DataTable
        DtTranTotal = DtTran.Clone
        DtTranTotal.Rows.Add()
        'DtTranTotal.Rows(0).Item("DESCRIPTION") = "TOTAL"
        DgvTranTotal.DataSource = DtTranTotal
        GridStyle(DgvTranTotal)
        'DgvTranTotal.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)

        dtTAGPrint = New DataTable
        dtTAGPrint.Columns.Add("ITEMID", GetType(Integer))
        dtTAGPrint.Columns.Add("TAGNO", GetType(String))

        ''COSTCENTRE
        If GetAdmindbSoftValue("COSTCENTRE", "N") = "Y" Then
            StrSql = "SELECT COSTNAME,COSTID FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            Dim dtCostcentre As New DataTable
            Da = New OleDbDataAdapter(StrSql, cn)
            Da.Fill(dtCostcentre)
            cmbCostCentre.DataSource = dtCostcentre
            cmbCostCentre.AutoCompleteMode = AutoCompleteMode.SuggestAppend
            cmbCostCentre.AutoCompleteSource = AutoCompleteSource.ListItems
            cmbCostCentre.DisplayMember = "COSTNAME"
            cmbCostCentre.ValueMember = "COSTID"
            cmbCostCentre.Enabled = True

        Else
            cmbCostCentre.Enabled = False
        End If
        ''ACCNAME
        StrSql = " SELECT ACNAME,ACCODE FROM " & cnAdminDb & "..ACHEAD "
        If InclCusttype = "Y" Then
            StrSql += " WHERE ACTYPE IN ('G','D','I','C')"
        Else
            StrSql += " WHERE ACTYPE IN ('G','D','I')"
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
        tagnos = ""
        ''TransactionType
    End Sub

    Private Sub ClearTran()
        DtTran.Rows.Clear()

        'For cnt As Integer = 0 To 20
        '    DtTran.Rows.Add()
        'Next
        DtTran.AcceptChanges()
        'dtGridStone.Rows.Clear()
        'dtGridStone.AcceptChanges()
    End Sub

    Private Sub GridStyle(ByVal Dgv As DataGridView)
        With Dgv
            For cnt As Integer = 0 To .ColumnCount - 1
                .Columns(cnt).Visible = True
                .Columns(cnt).SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns(cnt).Resizable = DataGridViewTriState.False
            Next
            .Columns("SNO").Width = 35
            .Columns("STYLENO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("STYLENO").Width = 85
            .Columns("ITEM").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns("ITEM").Width = 150
            .Columns("SUBITEM").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns("SUBITEM").Width = 100
            .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("GRSWT").Width = 75
            .Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("PCS").Width = 50
            .Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("AMOUNT").Width = 100
            .Columns("SALEAMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("SALEAMOUNT").Width = 100
            .Columns("MCGRM").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("MCGRM").Width = 50
            .Columns("WASTPER").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("WASTPER").Width = 75
            .Columns("PURITY").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("PURITY").Width = 75
            .Columns("RATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("RATE").Width = 75
            .Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("NETWT").Width = 75
            .Columns("DESIGNERNAME").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns("DESIGNERNAME").Width = 150
            .Columns("ITEMTYPE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns("ITEMTYPE").Width = 90
            .Columns("SIZE").Width = 50
            .Columns("CUT").Width = 50
            .Columns("COLOR").Width = 75
            .Columns("CLARITY").Width = 75
            .Columns("SHAPE").Width = 75
            .Columns("KEYNO").Visible = False
            .Columns("COLHEAD").Visible = False
            .Columns("REMARKS").Visible = False
            .Columns("NO").Visible = False
            DgvTran.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            DgvTran.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

        End With
    End Sub
    Private Sub GridStyleVisible(ByVal Dgv As DataGridView)
        With Dgv
            For cnt As Integer = 0 To .ColumnCount - 1
                .Columns(cnt).Visible = True
                .Columns(cnt).SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns(cnt).Resizable = DataGridViewTriState.False
            Next
            .Columns("STYLENO").Visible = True
            .Columns("ITEM").Visible = True
            .Columns("SUBITEM").Visible = True
            .Columns("PCS").Visible = True
            .Columns("GRSWT").Visible = True
            .Columns("NETWT").Visible = True
            .Columns("WASTPER").Visible = True
            .Columns("MCGRM").Visible = True
            .Columns("AMOUNT").Visible = True
            .Columns("SALEAMOUNT").Visible = True
            .Columns("ITEMTYPE").Visible = True
            .Columns("REMARKS").Visible = False
            .Columns("KEYNO").Visible = False
            .Columns("COLHEAD").Visible = False
            .Columns("NO").Visible = False
        End With
    End Sub
    Private Sub FormatedGridStyle(ByVal Dgv As DataGridView)
        Dim Remark() As String
        With Dgv
            .Columns("REMARKS").Visible = False
            For cnt As Integer = 0 To .RowCount - 1
                If .Rows(cnt).Cells("COLHEAD").Value.ToString <> "G" Then
                    .Rows(cnt).DefaultCellStyle.BackColor = Color.LightYellow
                End If
                Remark = Split(.Rows(cnt).Cells("REMARKS").Value.ToString, ",")
                If Remark.Length > 1 Then
                    For i As Integer = 0 To Remark.Length - 1
                        With .Rows(cnt)
                            If Remark(i) = "TN" Then
                                .Cells("STYLENO").Style.BackColor = Color.Red
                            End If
                            If Remark(i) = "IM" Then
                                .Cells("ITEM").Style.BackColor = Color.Red
                            End If
                            If Remark(i) = "SM" Then
                                .Cells("SUBITEM").Style.BackColor = Color.Red
                            End If
                            If Remark(i) = "DE" Then
                                .Cells("DESIGNERNAME").Style.BackColor = Color.Red
                            End If
                            If Remark(i) = "SI" Then
                                .Cells("SUBITEM").Style.BackColor = Color.Red
                            End If
                            If Remark(i) = "CU" Then
                                .Cells("CUT").Style.BackColor = Color.Red
                            End If
                            If Remark(i) = "CO" Then
                                .Cells("COLOR").Style.BackColor = Color.Red
                            End If
                            If Remark(i) = "CL" Then
                                .Cells("CLARITY").Style.BackColor = Color.Red
                            End If
                            If Remark(i) = "SH" Then
                                .Cells("SHAPE").Style.BackColor = Color.Red
                            End If
                        End With
                    Next
                End If
            Next
        End With
    End Sub

    Private Sub frmMaterialIssRecExcel_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            If DgvTran.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub


    Private Sub frmMaterialIssRecExcel_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.BackColor = SystemColors.InactiveCaption
        objGPack.Validator_Object(Me)
        DgvTran.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Sunken
        DgvTranTotal.Rows(0).DefaultCellStyle.BackColor = Color.LightGoldenrodYellow
        DgvTranTotal.Rows(0).DefaultCellStyle.SelectionBackColor = Color.LightGoldenrodYellow
        DgvTranTotal.Rows(0).DefaultCellStyle.ForeColor = Color.Red
        DgvTranTotal.Rows(0).DefaultCellStyle.SelectionForeColor = Color.Red
        dtpTrandate.Value = GetEntryDate(GetServerDate)
        dtpBillDate_OWN.Value = GetEntryDate(GetServerDate)
        GridStyle(DgvTran)
        ChkSale.Checked = True
        ChkPurchase.Checked = False
        grsnet = "N"
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNew.Click
        objGPack.TextClear(Me)
        ClearTran()
        tagnos = ""
        objCheaque = New frmChequeAdj
        objAddlCharge = New AddlChargesDia
        TdsPer = Nothing
        TranNo = Nothing
        BatchNo = Nothing
        Remark1 = Nothing
        Remark2 = Nothing
        lotNo = 0
        CmbTrantype.Enabled = True
        If cmbCostCentre.Enabled Then cmbCostCentre.Select() Else Me.SelectNextControl(cmbCostCentre, True, True, True, True)
    End Sub

    Private Sub Combo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = Keys.Enter Then
            If CType(sender, ComboBox).FindStringExact(CType(sender, ComboBox).SelectedText) <> -1 Then
                SendKeys.Send("{TAB}")
            End If
        End If
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub DgvTran_UserDeletedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs)
        DtTran.AcceptChanges()
        For cnt As Integer = 0 To DgvTran.RowCount - 1
            DgvTran.Rows(cnt).DefaultCellStyle.WrapMode = DataGridViewTriState.True
            DgvTran.AutoResizeRow(0)
        Next
    End Sub

    Private Sub DgvTran_UserDeletingRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowCancelEventArgs)
        DtTran.Rows.Add()
        DtTran.AcceptChanges()
    End Sub
    Function CheckMaster()
        Dim dtt As New DataTable
        Dim row() As DataRow = Nothing
        dtt = DgvTran.DataSource
        row = dtt.Select("REMARKS<>''")
        If row.Length > 0 Then
            Return False
        End If
        Return True
    End Function
    Function CheckPiece()
        Dim dtt As New DataTable
        Dim row() As DataRow = Nothing
        dtt = DgvTran.DataSource
        row = dtt.Select(" COLHEAD='G' AND PCS=0")
        If row.Length > 0 Then
            Return False
        End If
        Return True
    End Function
    Function CheckRateSave()
        Dim dtt As New DataTable
        Dim row() As DataRow = Nothing
        dtt = DgvTran.DataSource
        row = dtt.Select("RATE=0")
        If row.Length > 1 Then
            Return False
        End If
        Return True
    End Function
    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        ChkPurchase.Checked = True
        Dim billgencatcode As String = ""
        Dim billcontrolid As String = ""
        If Not CheckRateSave() Then MsgBox("Rate Should Not Empty", MsgBoxStyle.Information) : Exit Sub
        If Not CheckMaster() Then MsgBox("Master Not Found", MsgBoxStyle.Information) : Exit Sub
        If Not CheckPiece() Then MsgBox("Piece Should Not be Empty", MsgBoxStyle.Information) : Exit Sub
        If CheckTrialDate(dtpTrandate.Value) = False Then Exit Sub
        _Accode = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbAcName.Text & "'")
        If _Accode = "" Then
            MsgBox("Invalid AcName", MsgBoxStyle.Information)
            cmbAcName.Select()
            Exit Sub
        End If
        _Acctype = objGPack.GetSqlValue("SELECT LOCALOUTST FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = '" & _Accode & "'")
        Try
            tran = Nothing
            tran = cn.BeginTransaction()
            StrSql = " IF (SELECT COUNT(*) FROM " & cnAdminDb & "..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "BILLNO')> 0"
            StrSql += " DROP TABLE " & cnAdminDb & "..TEMP" & systemId & "BILLNO"
            Cmd = New OleDbCommand(StrSql, cn, tran)
            Cmd.ExecuteNonQuery()

            CostCenterId = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre.Text & "'", , , tran)

GenBillNo:
            billcontrolid = "GEN-SM-RECPUR"
            If _AccAudit Then billcontrolid = billcontrolid + "-APP"
            StrSql = "SELECT CTLMODE FROM " & cnStockDb & "..BILLCONTROL"
            StrSql += " WHERE CTLID ='" & billcontrolid & "' AND COMPANYID = '" & strCompanyId & "'"
            If UCase(objGPack.GetSqlValue(StrSql, , , tran)) = "Y" Then
                TranNo = GetBillNoValue(billcontrolid, tran)
            Else
                billcontrolid = "GEN-SM-REC"
                TranNo = GetBillNoValue(billcontrolid, tran)
            End If
            BatchNo = GetNewBatchno(cnCostId, dtpTrandate.Value.ToString("yyyy-MM-dd"), tran)

            For cnt As Integer = 0 To DgvTran.RowCount - 1
                Save(cnt)
            Next

            tran.Commit()
            tran = Nothing
            MsgBox("Saved Successfully")
            Dim msgdesc As String = "Tran No." & TranNo & " "
            'If tagnos <> "" Then msgdesc += vbCrLf + " And Tagno. " & Mid(tagnos, 2, Len(tagnos)) & " Generated.."
            MsgBox(msgdesc)
            Dim prnmemsuffix As String = ""
            If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId
            Dim oldItem As Integer = Nothing
            Dim memfile As String = "\Barcodeprint" & prnmemsuffix.Trim & ".mem"
            Dim write As IO.StreamWriter
            write = IO.File.CreateText(Application.StartupPath & memfile)
            For Each ro As DataRow In dtTAGPrint.Rows
                If oldItem <> Val(ro!itemid.ToString) Then
                    write.WriteLine(LSet("PROC", 7) & ":" & ro!ITEMID.ToString)
                    oldItem = Val(ro!itemid.ToString)
                End If
                write.WriteLine(LSet("TAGNO", 7) & ":" & ro!TAGNO.ToString)
            Next
            write.Flush()
            write.Close()
            If IO.File.Exists(Application.StartupPath & "\PrjBarcodePrint.exe") Then
                System.Diagnostics.Process.Start(Application.StartupPath & "\PrjBarcodePrint.exe")
            Else
                MsgBox("Barcode print exe not found", MsgBoxStyle.Information)
            End If

            lotNo = 0
            Dim pBatchno As String = BatchNo
            Dim pBillDate As Date = dtpTrandate.Value.Date.ToString("yyyy-MM-dd")
            btnNew_Click(Me, New EventArgs)
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
            DgvTran.DataSource = Nothing
            DgvTran.Refresh()
        Finally
            If Not tran Is Nothing Then tran.Dispose()
        End Try
    End Sub
    Function InsertIssueReceipt(ByVal index As Integer, ByVal Tagno As String, ByVal issSno As String _
                                , ByVal Itemid As Integer, ByVal subItemid As Integer _
                                , ByVal itemTypeId As Integer, ByVal catcode As String _
                                , ByVal OCatcode As String)
        If ChkPurchase.Checked = False Then Exit Function
        With DgvTran.Rows(index)
            StrSql = " INSERT INTO " & cnStockDb & ".."
            If Trantype = "IAP" Then
                StrSql += " ISSUE"
            Else
                StrSql += " RECEIPT"
            End If
            StrSql += " ("
            StrSql += "  SNO,TRANNO,TRANDATE,TRANTYPE,PCS"
            StrSql += " ,GRSWT,NETWT,LESSWT"
            StrSql += " ,TAGNO,ITEMID,SUBITEMID,WASTPER"
            StrSql += " ,WASTAGE,MCGRM,MCHARGE,AMOUNT"
            StrSql += " ,RATE,BOARDRATE,GRSNET"
            StrSql += " ,REFDATE,COSTID"
            StrSql += " ,COMPANYID"
            StrSql += " ,ITEMTYPEID"
            StrSql += " ,CATCODE,OCATCODE"
            StrSql += " ,ACCODE,BATCHNO,REMARK1"
            StrSql += " ,REMARK2,USERID,UPDATED,UPTIME,SYSTEMID"
            StrSql += " ,APPVER"
            StrSql += " )VALUES("
            StrSql += " '" & issSno & "'" ''SNO
            StrSql += " ," & TranNo & "" 'TRANNO
            StrSql += " ,'" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
            StrSql += " ,'" & Trantype & "'" 'TRANTYPE
            StrSql += " ," & Val(.Cells("PCS").Value.ToString) & "" 'PCS
            StrSql += " ," & Val(.Cells("GRSWT").Value.ToString) & "" 'GRSWT
            StrSql += " ," & Val(.Cells("NETWT").Value.ToString) & "" 'NETWT
            StrSql += " ," & 0 & "" 'LESSWT
            StrSql += " ,'" & Tagno & "'" 'TAGNO
            StrSql += " ," & Itemid & "" 'ITEMID
            StrSql += " ," & subItemid & "" 'SUBITEMID
            StrSql += " ," & 0 & "" 'WASTPER
            StrSql += " ,0" ' & Val(.Cells("WASTAGE").Value.ToString) & "" 'WASTAGE
            StrSql += " ," & 0 & "" 'MCGRM
            StrSql += " ,0" '& Val(.Cells("MC").Value.ToString) & "" 'MCHARGE
            StrSql += " ," & Val(.Cells("AMOUNT").Value.ToString) & "" 'AMOUNT
            StrSql += " ," & 0 & "" 'RATE
            StrSql += " ," & Val(.Cells("RATE").Value.ToString) & "" 'BOARDRATE
            StrSql += " ,'" & grsnet & "'" 'GRSNET
            StrSql += " ,'" & dtpBillDate_OWN.Value.ToString("yyyy-MM-dd") & "'" 'REFDATE NULL
            StrSql += " ,'" & CostCenterId & "'" 'COSTID 
            StrSql += " ,'" & strCompanyId & "'" 'COMPANYID
            StrSql += " ," & itemTypeId & "" 'ITEMTYPEID
            StrSql += " ,'" & catcode & "'" 'CATCODE
            StrSql += " ,'" & OCatcode & "'" 'OCATCODE
            StrSql += " ,'" & _Accode & "'" 'ACCODE
            StrSql += " ,'" & BatchNo & "'" 'BATCHNO
            StrSql += " ,''" 'REMARK1
            StrSql += " ,''" 'REMARK2
            StrSql += " ,'" & userId & "'" 'USERID
            StrSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
            StrSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
            StrSql += " ,'" & systemId & "'" 'SYSTEMID
            StrSql += " ,'" & VERSION & "'" 'APPVER
            StrSql += " )"
            ExecQuery(SyncMode.Transaction, StrSql, cn, tran, CostCenterId)
        End With
    End Function
    Function InsertAcctran(ByVal index As Integer, ByVal Tagno As String, ByVal issSno As String _
                               , ByVal Itemid As Integer, ByVal subItemid As Integer _
                               , ByVal itemTypeId As Integer, ByVal catcode As String _
                               , ByVal OCatcode As String)
        If ChkPurchase.Checked = False Then Exit Function
        With DgvTran.Rows(index)
            Dim vat As String = Nothing
            Dim CONT As String = Nothing
            Dim TAXACC As String = Nothing
            issSno = GetNewSno(TranSnoType.ACCTRANCODE, tran)
            vat = Val(.Cells("AMOUNT").Value.ToString) * Val(txtTaxper_PER.Text.ToString) / 100
            CONT = objGPack.GetSqlValue("SELECT PURCHASEID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = '" & catcode & "'", , , tran)
            TAXACC = objGPack.GetSqlValue("SELECT PTAXID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = '" & catcode & "'", , , tran)
            Dim amt As Decimal = Val(.Cells("AMOUNT").Value.ToString)
            StrSql = " INSERT INTO " & cnStockDb & "..ACCTRAN"
            StrSql += " ("
            StrSql += "  SNO,TRANNO,TRANDATE,TRANMODE"
            StrSql += "  ,acCode, SACCODE, AMOUNT, BALANCE, PCS,"
            StrSql += " GRSWT, NETWT, PUREWT, REFNO, REFDATE, PAYMODE,"
            StrSql += " CARDID,FROMFLAG,"
            StrSql += " Remark1, Remark2, CONTRA, BatchNo, userId, UPDATED, "
            StrSql += " UPTIME, systemId, CANCEL, CASHID, "
            StrSql += " COSTID, SCOSTID, VATEXM, APPVER,COMPANYID "
            StrSql += " )VALUES("
            StrSql += " '" & issSno & "'" ''SNO
            StrSql += " ," & TranNo & "" 'TRANNO
            StrSql += " ,'" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
            StrSql += " ,'C'" 'TRANMODE
            StrSql += " ,'" & _Accode & "'" 'ACCODE
            StrSql += " ,''" 'SACCODE
            amt = amt + vat
            StrSql += " ," & amt & "" 'AMOUNT
            StrSql += " ,'0.0'" 'BALANCE
            StrSql += " ," & Val(.Cells("PCS").Value.ToString) & "" 'PCS
            StrSql += " ," & Val(.Cells("GRSWT").Value.ToString) & "" 'GRSWT
            StrSql += " ," & Val(.Cells("NETWT").Value.ToString) & "" 'NETWT
            StrSql += " ,'0.0'" 'PUREWT
            StrSql += " ,'" & txtBillNo.Text & "'" 'REFNO
            StrSql += " ,'" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "'" 'REFDATE
            StrSql += " ,'TR'" 'PAYMODE
            StrSql += " ,'0'" 'CARDID
            StrSql += " ,'S'" 'FROMFLAG
            StrSql += " ,''" 'REMARK1
            StrSql += " ,''" 'REMARK2
            StrSql += " ,'" & CONT & "'" 'CONTRA
            StrSql += " ,'" & BatchNo & "'" 'BATCHNO
            StrSql += " ,'" & userId & "'" 'USERID
            StrSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
            StrSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
            StrSql += " ,'" & systemId & "'" 'SYSTEMID
            StrSql += " ,''" 'CANCEL
            StrSql += " ,''" 'CASHID
            StrSql += " ,'" & CostCenterId & "'" 'COSTID 
            StrSql += " ,''" 'SCOSTID
            StrSql += " ,''" 'VATEXM
            StrSql += " ,'" & VERSION & "'" 'APPVER
            StrSql += " ,'" & strCompanyId & "'" 'COMPANYID
            StrSql += " )"
            ExecQuery(SyncMode.Transaction, StrSql, cn, tran, CostCenterId)

            If Val(txtTaxper_PER.Text) = 0 Then
                issSno = GetNewSno(TranSnoType.ACCTRANCODE, tran)
                StrSql = " INSERT INTO " & cnStockDb & "..ACCTRAN"
                StrSql += " ("
                StrSql += "  SNO,TRANNO,TRANDATE,TRANMODE"
                StrSql += "  ,acCode, SACCODE, AMOUNT, BALANCE, PCS,"
                StrSql += " GRSWT, NETWT, PUREWT, REFNO, REFDATE, PAYMODE,"
                StrSql += " CARDID,FROMFLAG,"
                StrSql += " Remark1, Remark2, CONTRA, BatchNo, userId, UPDATED, "
                StrSql += " UPTIME, systemId, CANCEL, CASHID, "
                StrSql += " COSTID, SCOSTID, VATEXM, APPVER,COMPANYID "
                StrSql += " )VALUES("
                StrSql += " '" & issSno & "'" ''SNO
                StrSql += " ," & TranNo & "" 'TRANNO
                StrSql += " ,'" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                StrSql += " ,'D'" 'TRANMODE
                StrSql += " ,'" & CONT & "'" 'ACCODE
                StrSql += " ,''" 'SACCODE
                amt = amt + vat
                StrSql += " ," & amt & "" 'AMOUNT
                StrSql += " ,'0.0'" 'BALANCE
                StrSql += " ," & Val(.Cells("PCS").Value.ToString) & "" 'PCS
                StrSql += " ," & Val(.Cells("GRSWT").Value.ToString) & "" 'GRSWT
                StrSql += " ," & Val(.Cells("NETWT").Value.ToString) & "" 'NETWT
                StrSql += " ,'0.0'" 'PUREWT
                StrSql += " ,'" & txtBillNo.Text & "'" 'REFNO
                StrSql += " ,'" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "'" 'REFDATE
                StrSql += " ,'TR'" 'PAYMODE
                StrSql += " ,'0'" 'CARDID
                StrSql += " ,'S'" 'FROMFLAG
                StrSql += " ,''" 'REMARK1
                StrSql += " ,''" 'REMARK2
                StrSql += " ,'" & _Accode & "'" 'CONTRA
                StrSql += " ,'" & BatchNo & "'" 'BATCHNO
                StrSql += " ,'" & userId & "'" 'USERID
                StrSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                StrSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                StrSql += " ,'" & systemId & "'" 'SYSTEMID
                StrSql += " ,''" 'CANCEL
                StrSql += " ,''" 'CASHID
                StrSql += " ,'" & CostCenterId & "'" 'COSTID 
                StrSql += " ,''" 'SCOSTID
                StrSql += " ,''" 'VATEXM
                StrSql += " ,'" & VERSION & "'" 'APPVER
                StrSql += " ,'" & strCompanyId & "'" 'COMPANYID
                StrSql += " )"
                ExecQuery(SyncMode.Transaction, StrSql, cn, tran, CostCenterId)
            Else
                issSno = GetNewSno(TranSnoType.ACCTRANCODE, tran)
                StrSql = " INSERT INTO " & cnStockDb & "..ACCTRAN"
                StrSql += " ("
                StrSql += "  SNO,TRANNO,TRANDATE,TRANMODE"
                StrSql += "  ,acCode, SACCODE, AMOUNT, BALANCE, PCS,"
                StrSql += " GRSWT, NETWT, PUREWT, REFNO, REFDATE, PAYMODE,"
                StrSql += " CARDID,FROMFLAG,"
                StrSql += " Remark1, Remark2, CONTRA, BatchNo, userId, UPDATED, "
                StrSql += " UPTIME, systemId, CANCEL, CASHID, "
                StrSql += " COSTID, SCOSTID, VATEXM, APPVER,COMPANYID "
                StrSql += " )VALUES("
                StrSql += " '" & issSno & "'" ''SNO
                StrSql += " ," & TranNo & "" 'TRANNO
                StrSql += " ,'" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                StrSql += " ,'D'" 'TRANMODE
                StrSql += " ,'" & CONT & "'" 'ACCODE
                StrSql += " ,''" 'SACCODE
                StrSql += " ," & Val(.Cells("AMOUNT").Value.ToString) & "" 'AMOUNT
                StrSql += " ,'0.0'" 'BALANCE
                StrSql += " ," & Val(.Cells("PCS").Value.ToString) & "" 'PCS
                StrSql += " ," & Val(.Cells("GRSWT").Value.ToString) & "" 'GRSWT
                StrSql += " ," & Val(.Cells("NETWT").Value.ToString) & "" 'NETWT
                StrSql += " ,'0.0'" 'PUREWT
                StrSql += " ,'" & txtBillNo.Text & "'" 'REFNO
                StrSql += " ,'" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "'" 'REFDATE
                StrSql += " ,'TR'" 'PAYMODE
                StrSql += " ,'0'" 'CARDID
                StrSql += " ,'S'" 'FROMFLAG
                StrSql += " ,''" 'REMARK1
                StrSql += " ,''" 'REMARK2
                StrSql += " ,'" & _Accode & "'" 'CONTRA
                StrSql += " ,'" & BatchNo & "'" 'BATCHNO
                StrSql += " ,'" & userId & "'" 'USERID
                StrSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                StrSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                StrSql += " ,'" & systemId & "'" 'SYSTEMID
                StrSql += " ,''" 'CANCEL
                StrSql += " ,''" 'CASHID
                StrSql += " ,'" & CostCenterId & "'" 'COSTID 
                StrSql += " ,''" 'SCOSTID
                StrSql += " ,''" 'VATEXM
                StrSql += " ,'" & VERSION & "'" 'APPVER
                StrSql += " ,'" & strCompanyId & "'" 'COMPANYID
                StrSql += " )"
                ExecQuery(SyncMode.Transaction, StrSql, cn, tran, CostCenterId)

                ''TAX
                issSno = GetNewSno(TranSnoType.ACCTRANCODE, tran)
                StrSql = " INSERT INTO " & cnStockDb & ".."
                StrSql += " ACCTRAN"
                StrSql += " ("
                StrSql += "  SNO,TRANNO,TRANDATE,TRANMODE"
                StrSql += "  ,acCode, SACCODE, AMOUNT, BALANCE, PCS,"
                StrSql += " GRSWT, NETWT, PUREWT, REFNO, REFDATE, PAYMODE,"
                StrSql += " CARDID,FROMFLAG,"
                StrSql += " Remark1, Remark2, CONTRA, BatchNo, userId, UPDATED, "
                StrSql += " UPTIME, systemId, CANCEL, CASHID, "
                StrSql += " COSTID, SCOSTID, VATEXM, APPVER,COMPANYID "
                StrSql += " )VALUES("
                StrSql += " '" & issSno & "'" ''SNO
                StrSql += " ," & TranNo & "" 'TRANNO
                StrSql += " ,'" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                StrSql += " ,'D'" 'TRANMODE
                StrSql += " ,'" & TAXACC & "'" 'ACCODE
                StrSql += " ,''" 'SACCODE
                StrSql += " ," & vat & "" 'AMOUNT
                StrSql += " ,'0.0'" 'BALANCE
                StrSql += " ," & 0 & "" 'PCS
                StrSql += " ," & 0 & "" 'GRSWT
                StrSql += " ," & 0 & "" 'NETWT
                StrSql += " ,'0.0'" 'PUREWT
                StrSql += " ,'" & txtBillNo.Text & "'" 'REFNO
                StrSql += " ,'" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "'" 'REFDATE
                StrSql += " ,'TR'" 'PAYMODE
                StrSql += " ,'0'" 'CARDID
                StrSql += " ,'S'" 'FROMFLAG
                StrSql += " ,''" 'REMARK1
                StrSql += " ,''" 'REMARK2
                StrSql += " ,'" & _Accode & "'" 'CONTRA
                StrSql += " ,'" & BatchNo & "'" 'BATCHNO
                StrSql += " ,'" & userId & "'" 'USERID
                StrSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                StrSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                StrSql += " ,'" & systemId & "'" 'SYSTEMID
                StrSql += " ,''" 'CANCEL
                StrSql += " ,''" 'CASHID
                StrSql += " ,'" & CostCenterId & "'" 'COSTID 
                StrSql += " ,''" 'SCOSTID
                StrSql += " ,''" 'VATEXM
                StrSql += " ,'" & VERSION & "'" 'APPVER
                StrSql += " ,'" & strCompanyId & "'" 'COMPANYID
                StrSql += " )"
                ExecQuery(SyncMode.Transaction, StrSql, cn, tran, CostCenterId)
            End If
        End With
    End Function
    Private Sub Save(ByVal index As Integer)
        With DgvTran.Rows(index)
            If .Cells("COLHEAD").Value.ToString = "G" Then
                Dim OrdStateId As Integer = 0
                Dim Tax As Decimal = 0
                Dim Tds As Decimal = 0
                Dim Type As String = "O" ' wheather it is ornament,metal,stone,others
                Dim catCode As String = objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Cells("ITEM").Value.ToString & "'", , , tran)
                Dim OCatcode As String
                OCatcode = objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Cells("ITEM").Value.ToString & "'", , , tran)
                Dim alloy As Decimal = Nothing
                Dim itemTypeId As Integer = 0
                itemTypeId = Val(objGPack.GetSqlValue("SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & .Cells("ITEMTYPE").Value.ToString & "'", , 0, tran))
                Dim Itemid As Integer = Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Cells("ITEM").Value.ToString & "'", , 0, tran))
                Dim subItemid As Integer = Val(objGPack.GetSqlValue("SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & .Cells("SUBITEM").Value.ToString & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Cells("ITEM").Value.ToString & "')", , 0, tran))
                Dim itemctrid As Integer '= Val(objGPack.GetSqlValue("SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & .Cells("ITEMCTRNAME").Value.ToString & "'", , 0, tran))
                If OCatcode.ToString = "" Then OCatcode = catCode

                If CmbTrantype.Text = "PURCHASE APPROVAL RECEIPT" Then
                    Trantype = "RAP"
                ElseIf CmbTrantype.Text = "PURCHASE APPROVAL RETURN" Then
                    Trantype = "IAP"
                Else
                    Trantype = "RPU"
                End If

                Dim calType, mlwmctype As String
                Dim Lasttagno As Integer
                Dim tagPrefix, tagno, TagSno, tagVal, StyleNo As String
                'Dim SHAPEid, Colorid, Clarityid, Settypeid As Integer

                StrSql = " SELECT ITEMID,METALID,SIZESTOCK,OTHCHARGE,CALTYPE,VALUEADDEDTYPE,DEFAULTCOUNTER"
                StrSql += " ,STUDDED,STOCKTYPE,TABLECODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Cells("ITEM").Value.ToString & "'"
                Dim dtItemDetail As New DataTable
                Cmd = New OleDbCommand(StrSql, cn, tran)
                Da = New OleDbDataAdapter(Cmd)
                Da.Fill(dtItemDetail)
                If dtItemDetail.Rows.Count > 0 Then
                    With dtItemDetail.Rows(0)
                        calType = .Item("CALTYPE").ToString
                        mlwmctype = .Item("VALUEADDEDTYPE").ToString
                        'itemCtrId = Val(.Item("DEFAULTCOUNTER").ToString)
                    End With
                End If

                StrSql = " SELECT TOP 1 DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME='" & .Cells("DESIGNERNAME").Value.ToString & "'"
                designerid = Val(objGPack.GetSqlValue(StrSql, , "", tran))

                StrSql = " SELECT TOP 1 SIZEID FROM " & cnAdminDb & "..ITEMSIZE WHERE SIZENAME='" & .Cells("SIZE").Value.ToString & "' AND ITEMID=" & Itemid
                Dim SizeId As Integer = Val(objGPack.GetSqlValue(StrSql, , "", tran))

                StyleNo = .Cells("STYLENO").Value.ToString()

                Dim issSno As String = Nothing
                If Trantype = "IAP" Then
                    issSno = GetNewSno(TranSnoType.ISSUECODE, tran)
                Else
                    issSno = GetNewSno(TranSnoType.RECEIPTCODE, tran)
                End If

                InsertIssueReceipt(index, tagno, issSno, Itemid, subItemid, itemTypeId, catCode, OCatcode)
                InsertAcctran(index, tagno, issSno, Itemid, subItemid, itemTypeId, catCode, OCatcode)
                Dim Issdate As Date
                Dim CTag As Boolean = False
                If Trantype = "RPU" And POS_TAGXLDOWN_TAGNO = False Then
                    StrSql = "SELECT 1 FROM " & cnAdminDb & "..ITEMTAG WHERE STYLENO='" & StyleNo & "' AND ISNULL(BATCHNO,'')<>''  AND RECTYPE='IAP'"
                    If objGPack.GetSqlValue(StrSql, , 0, tran) = 1 Then
                        CTag = True
                        StrSql = "SELECT TRANDATE FROM " & cnStockDb & "..ISSUE WHERE BATCHNO=(SELECT TOP 1 BATCHNO FROM " & cnAdminDb & "..ITEMTAG WHERE STYLENO='" & StyleNo & "' AND ISNULL(BATCHNO,'')<>'' AND RECTYPE='IAP')"
                        Issdate = objGPack.GetSqlValue(StrSql, "TRANDATE", "", tran)
                    End If
                End If

                ''INSERTING ITEMTAG
                If Trantype <> "IAP" Then
                    tagno = objtag.GetTagNo(dtpTrandate.Value.ToString("yyyy-MM-dd"), .Cells("ITEM").Value.ToString, "", tran)
                    TagSno = GetNewSno(TranSnoType.ITEMTAGCODE, tran, "GET_ADMINSNO_TRAN")
                    tagVal = objtag.GetTagVal(tagno)

                    StrSql = " INSERT INTO " & cnAdminDb & ".."
                    If CTag Then
                        StrSql += "CITEMTAG"
                    Else
                        StrSql += "ITEMTAG"
                    End If
                    StrSql += " ("
                    StrSql += " SNO,RECDATE,COSTID,ITEMID,ORDREPNO,ORSNO,"
                    StrSql += " ORDSALMANCODE,SUBITEMID,SIZEID,ITEMCTRID,TABLECODE,DESIGNERID,"
                    StrSql += " TAGNO, PCS,GRSWT,"
                    StrSql += " LESSWT,NETWT,RATE,FINERATE,MAXWASTPER,MAXMCGRM,"
                    StrSql += " MAXWAST,MAXMC,MINWASTPER,MINMCGRM,MINWAST,MINMC,"
                    StrSql += " TAGKEY,TAGVAL,LOTSNO,COMPANYID,SALVALUE,PURITY,NARRATION,DESCRIP,"
                    StrSql += " REASON,ENTRYMODE,GRSNET,"
                    StrSql += " ISSDATE,ISSREFNO,ISSPCS,ISSWT,FROMFLAG,TOFLAG,APPROVAL,SALEMODE,"
                    StrSql += " BATCHNO,MARK,"
                    StrSql += " VATEXM,PCTFILE,OLDTAGNO,ITEMTYPEID,ACTUALRECDATE,WEIGHTUNIT,"
                    StrSql += " TRANSFERWT,CHKDATE,CHKTRAY,CARRYFLAG,BRANDID,PRNFLAG,"
                    StrSql += " MCDISCPER,WASTDISCPER,RESDATE,TRANINVNO,SUPBILLNO,WORKDAYS,"
                    StrSql += " USERID,UPDATED,UPTIME,SYSTEMID,STYLENO,APPVER,TRANSFERDATE,"
                    StrSql += " BOARDRATE,RFID,TOUCH,HM_BILLNO,HM_CENTER,ADD_VA_PER,REFVALUE,VALUEADDEDTYPE,TCOSTID,EXTRAWT,"
                    StrSql += " USRATE,INDRS,"
                    StrSql += " CERTIFICATENO,RECSNO,FROMITEMID,SHAPEID"
                    StrSql += " ,COLORID,CLARITYID,SETTYPEID,HEIGHT,WIDTH,RECTYPE"
                    StrSql += " ) "
                    StrSql += " VALUES("
                    StrSql += " '" & TagSno & "'" 'SNO
                    StrSql += " ,'" & GetEntryDate(dtpTrandate.Value, tran).ToString("yyyy-MM-dd") & "'" 'RECDATE 
                    StrSql += " ,'" & cnCostId & "'" 'COSTID
                    StrSql += " ," & Itemid & "" 'ITEMID
                    StrSql += " ,''" 'ORDREPNO
                    StrSql += " ,''" 'ORsno
                    StrSql += " ,''" 'ORDSALMANCODE
                    StrSql += " ," & subItemid & "" 'SUBITEMID
                    StrSql += " ," & SizeId 'SIZEID
                    StrSql += " ," & itemctrid & "" 'ITEMCTRID
                    StrSql += " ,''"
                    StrSql += " ," & designerid & "" 'DESIGNERID
                    StrSql += " ,'" & tagno & "'" 'TAGNO
                    StrSql += " ," & Val(.Cells("PCS").Value.ToString) & "" 'PCS
                    StrSql += " ," & Val(.Cells("GRSWT").Value.ToString) & "" 'GRSWT
                    StrSql += " ,0" ' & Val(.Cells("GRSWT").Value.ToString) - Val(.Cells("NETWT").Value.ToString) & "" 'LESSWT
                    StrSql += " ," & Val(.Cells("NETWT").Value.ToString) & "" 'NETWT
                    StrSql += " ," & Val(.Cells("RATE").Value.ToString) & "" 'RATE
                    StrSql += ",0" 'FINERATE
                    StrSql += " ," & Val(.Cells("WASTPER").Value.ToString) & "" 'MAXWASTPER
                    StrSql += " ," & Val(.Cells("MCGRM").Value.ToString) & "" 'MAXMCGRM
                    StrSql += " ,0" '& Val(.Cells("MAXWASTAGE").Value.ToString) & "" 'MAXWAST
                    StrSql += " ,0" '& Val(.Cells("MAXMC").Value.ToString) & "" 'MAXMC
                    StrSql += " ,0" 'MINWASTPER
                    StrSql += " ,0" 'MINMCGRM
                    StrSql += " ,0" 'MINWAST
                    StrSql += " ,0" 'MINMC
                    StrSql += " ,'" & Itemid.ToString & "" & tagno & "'" 'TAGKEY
                    StrSql += " ,'" & tagVal & "'" 'TAGVAL
                    StrSql += " ,''" 'LOTSNO
                    StrSql += " ,'" & strCompanyId & "'" 'COMPANYID
                    StrSql += " ," & Val(.Cells("SALEAMOUNT").Value.ToString) & "" 'SALVALUE
                    StrSql += " ," & Val(.Cells("PURITY").Value.ToString) & "" 'PURITY"
                    StrSql += " ,''" 'NARRATION
                    If .Cells("SUBITEM").Value.ToString.Trim <> "" Then 'DESCRIP
                        StrSql += " ,'" & .Cells("SUBITEM").Value.ToString & "'"
                    Else
                        StrSql += " ,'" & .Cells("ITEM").Value.ToString & "'"
                    End If
                    StrSql += " ,''" 'REASON
                    StrSql += " ,'M'"
                    StrSql += " ,'" & grsnet & "'" 'GRSNET
                    If CTag Then
                        StrSql += " ,'" & Issdate.ToString("yyyy-MM-dd") & "'" 'ISSDATE
                    Else
                        StrSql += " ,NULL" 'ISSDATE
                    End If
                    StrSql += " ,0" 'ISSREFNO
                    StrSql += " ,0" 'ISSPCS
                    StrSql += " ,0" 'ISSWT
                    StrSql += " ,''" 'FROMFLAG
                    StrSql += " ,''" 'TOFLAG
                    StrSql += " ,''" 'APPROVAL
                    StrSql += " ,'" & calType & "'" 'SALEMODE
                    StrSql += " ,''" 'BATCHNO
                    StrSql += " ,0" 'MARK
                    StrSql += " ,''" 'VATEXM
                    StrSql += " ,''" ' pctfile
                    StrSql += " ,''" 'OLDTAGNO
                    StrSql += " ," & Val(itemTypeId)  'ITEMTYPEID
                    StrSql += " ,'" & GetEntryDate(dtpTrandate.Value, tran).ToString("yyyy-MM-dd") & "'" 'ACTUALRECDATE
                    StrSql += " ,''" 'WEIGHTUNIT
                    StrSql += " ,0" 'TRANSFERWT
                    StrSql += " ,NULL" 'CHKDATE
                    StrSql += " ,''" 'CHKTRAY
                    StrSql += " ,''" 'CARRYFLAG
                    StrSql += " ,''" 'BRANDID
                    StrSql += " ,''" 'PRNFLAG
                    StrSql += " ,0" 'MCDISCPER
                    StrSql += " ,0" 'WASTDISCPER
                    StrSql += " ,NULL" 'RESDATE
                    StrSql += " ,''" 'TRANINVNO
                    StrSql += " ,''" 'SUPBILLNO
                    StrSql += " ,''" 'WORKDAYS
                    StrSql += " ," & userId & "" 'USERID
                    StrSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                    StrSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                    StrSql += " ,'" & systemId & "'" 'SYSTEMID
                    StrSql += " ,'" & StyleNo & "'" 'STYLENO
                    StrSql += " ,'" & VERSION & "'" 'APPVER
                    StrSql += " ,'" & GetEntryDate(dtpTrandate.Value, tran).ToString("yyyy-MM-dd") & "'" 'TRANSFERDATE
                    StrSql += " ,0" '& Val(.Cells("RETAILPRICE").Value.ToString) & "" 'BOARDRATE
                    StrSql += " ,''"
                    StrSql += " ,0"
                    StrSql += " ,''" 'HM_BILLNO
                    StrSql += " ,''" 'HM_CENTER
                    StrSql += " ,0" 'ADD_VA_PER
                    StrSql += " ,0" 'REFVALUE
                    StrSql += " ,'" & mlwmctype & "'"
                    StrSql += " ,'" & cnCostId & "'" 'TCOSTID
                    StrSql += " ,0" 'EXTRAWT
                    StrSql += " ,0"
                    StrSql += " ,0"
                    StrSql += " ,'" & .Cells("CERTNO").Value.ToString & "'" 'CERTIFICATENO
                    StrSql += " ,'" & issSno & "'" 'RECSNO
                    StrSql += " ,0" 'FROMITEMID
                    StrSql += " ,0" '& SHAPEid & "" 'SHAPEID
                    StrSql += " ,0" '& Colorid & "" 'COLORID
                    StrSql += " ,0" '& Clarityid & "" 'CLARITYID
                    StrSql += " ,0" '& Settypeid & "" 'SETTYPEID
                    StrSql += " ,0" '& Val(.Cells("HEIGHT").Value.ToString) & "" 'HEIGHT
                    StrSql += " ,0" '& Val(.Cells("WIDTH").Value.ToString) & "" 'WIDTH
                    If CTag Then
                        StrSql += " ,'IAP'"  'TRANTYPE
                    Else
                        StrSql += " ,'" & Trantype & "'"  'TRANTYPE
                    End If
                    StrSql += " )"
                    Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.ExecuteNonQuery()
                    If CTag Then
                        StrSql = "UPDATE " & cnAdminDb & "..ITEMTAG SET RECTYPE='RPU' WHERE STYLENO='" & StyleNo & "' AND BATCHNO<>'' "
                        Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.ExecuteNonQuery()
                    End If

                    Dim PurTax As Decimal = 0
                    If Val(txtTaxper_PER.Text) <> 0 Then
                        PurTax = (Val(.Cells("AMOUNT").Value.ToString) * Val(txtTaxper_PER.Text)) / 100
                    End If
                    ''ITEM PUR DETAIL
                    If CTag Then
                        StrSql = vbCrLf + " INSERT INTO " & cnAdminDb & "..CPURITEMTAG"
                    Else
                        StrSql = vbCrLf + " INSERT INTO " & cnAdminDb & "..PURITEMTAG"
                    End If
                    StrSql += vbCrLf + " ("
                    StrSql += vbCrLf + " TAGSNO,ITEMID,TAGNO,PURLESSWT,PURNETWT,PURRATE"
                    StrSql += vbCrLf + " ,PURGRSNET,PURWASTAGE,PURTOUCH,PURMC"
                    StrSql += vbCrLf + " ,PURVALUE,PURTAX,RECDATE,COMPANYID,COSTID"
                    StrSql += vbCrLf + " )"
                    StrSql += vbCrLf + " VALUES"
                    StrSql += vbCrLf + " ("
                    StrSql += vbCrLf + " '" & TagSno & "'" 'TAGSNO
                    StrSql += vbCrLf + " ," & Itemid & "" 'ITEMID
                    StrSql += vbCrLf + " ,'" & tagno & "'" 'TAGNO
                    StrSql += vbCrLf + " ,0"
                    StrSql += vbCrLf + " ," & Val(.Cells("NETWT").Value.ToString) & "" ' PURNETWT"
                    StrSql += vbCrLf + " ," & Val(.Cells("AMOUNT").Value.ToString) & "" ' PURRATE"
                    StrSql += vbCrLf + " ,'" & grsnet & "'" ' PURGRSNET"
                    StrSql += vbCrLf + " ,0" '& Val(.Cells("WASTAGE").Value.ToString) & "" ' PURWASTAGE"
                    StrSql += vbCrLf + " ," & 0 & "" ' PURTOUCH"
                    StrSql += vbCrLf + " ,0" '& Val(.Cells("MC").Value.ToString) & "" ' PURMC"
                    StrSql += vbCrLf + " ," & Val(.Cells("AMOUNT").Value.ToString) & "" ' PURVALUE"
                    StrSql += vbCrLf + " ," & PurTax & ""
                    StrSql += vbCrLf + " ,'" & dtpBillDate_OWN.Value.ToString("yyyy-MM-dd") & "'"
                    StrSql += vbCrLf + " ,'" & strCompanyId & "'"
                    StrSql += vbCrLf + " ,'" & cnCostId & "'"
                    StrSql += vbCrLf + " )"
                    Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.ExecuteNonQuery()
                End If

                Dim rowTag As DataRow = Nothing
                rowTag = dtTAGPrint.NewRow
                rowTag!ITEMID = Itemid
                rowTag!TAGNO = tagno
                dtTAGPrint.Rows.Add(rowTag)

                Dim drstone() As DataRow = DtTran.Select("NO='" & .Cells("NO").Value.ToString & "' AND COLHEAD <>'G'", "")
                For Each stRow As DataRow In drstone
                    InsertStoneDetails(CTag, issSno, StyleNo, TranNo _
                        , stRow.Item("ITEM").ToString, stRow.Item("SUBITEM").ToString _
                        , stRow.Item("CUT").ToString, stRow.Item("COLOR").ToString _
                        , stRow.Item("CLARITY").ToString, stRow.Item("SHAPE").ToString _
                        , Val(stRow.Item("PCS").ToString) _
                        , Val(stRow.Item("GRSWT").ToString), Val(stRow.Item("AMOUNT").ToString) _
                        , TagSno, tagno, Itemid, Tax)
                Next


                If Trantype = "IAP" Then
                    Dim ColList As String

                    StrSql = vbCrLf + " UPDATE " & cnAdminDb & "..ITEMTAG SET "
                    StrSql += vbCrLf + " ISSDATE = '" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "' WHERE ISSDATE IS NULL "
                    StrSql += vbCrLf + " AND STYLENO = '" & StyleNo & "' AND RECTYPE='RAP' AND ISNULL(BATCHNO,'')=''"
                    Cmd = New OleDbCommand(StrSql, cn, tran)
                    If Cmd.ExecuteNonQuery() = 0 Then
                        StrSql = vbCrLf + " UPDATE " & cnAdminDb & "..ITEMTAG SET RECTYPE='IAP'"
                        StrSql += vbCrLf + " WHERE STYLENO = '" & StyleNo & "' "
                        StrSql += vbCrLf + " AND RECTYPE='RAP' AND ISNULL(BATCHNO,'')<>''"
                        Cmd = New OleDbCommand(StrSql, cn, tran)
                        Cmd.ExecuteNonQuery()
                    End If

                    ColList = GetColumnNames(cnAdminDb, "CITEMTAG", tran)
                    StrSql = vbCrLf + " INSERT INTO " & cnAdminDb & "..CITEMTAG(" & ColList & ")"
                    StrSql += vbCrLf + " SELECT  " & ColList & " FROM " & cnAdminDb & "..ITEMTAG WHERE ISSDATE = '" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "' "
                    StrSql += vbCrLf + " AND STYLENO = '" & StyleNo & "' AND RECTYPE='RAP' AND ISNULL(BATCHNO,'')=''"
                    Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.ExecuteNonQuery()

                    ColList = GetColumnNames(cnAdminDb, "CPURITEMTAG", tran)
                    StrSql = vbCrLf + " INSERT INTO " & cnAdminDb & "..CPURITEMTAG(" & ColList & ")"
                    StrSql += vbCrLf + " SELECT " & ColList & " FROM " & cnAdminDb & "..PURITEMTAG "
                    StrSql += vbCrLf + " WHERE TAGSNO IN"
                    StrSql += vbCrLf + " (SELECT SNO FROM  " & cnAdminDb & "..ITEMTAG WHERE ISSDATE = '" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "'"
                    StrSql += vbCrLf + " AND STYLENO = '" & StyleNo & "' AND RECTYPE='RAP' AND ISNULL(BATCHNO,'')='')"
                    Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.ExecuteNonQuery()

                    StrSql = " DELETE FROM " & cnAdminDb & "..PURITEMTAG  "
                    StrSql += " WHERE TAGSNO IN("
                    StrSql += " SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE STYLENO='" & StyleNo & "' "
                    StrSql += " AND ISSDATE = '" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(TOFLAG,'')='' AND ISNULL(BATCHNO,'')='' AND RECTYPE='RAP')"
                    Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.ExecuteNonQuery()

                    StrSql = "DELETE FROM " & cnAdminDb & "..ITEMTAGSTONE "
                    StrSql += " WHERE TAGSNO IN("
                    StrSql += " SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE STYLENO='" & StyleNo & "' "
                    StrSql += " AND ISSDATE = '" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(TOFLAG,'')='' AND ISNULL(BATCHNO,'')='' AND RECTYPE='RAP')"
                    Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.ExecuteNonQuery()

                    StrSql = "DELETE FROM " & cnAdminDb & "..PURITEMTAGSTONE  "
                    StrSql += " WHERE TAGSNO IN("
                    StrSql += " SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE STYLENO='" & StyleNo & "' "
                    StrSql += " AND ISSDATE = '" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(TOFLAG,'')='' AND ISNULL(BATCHNO,'')='' AND RECTYPE='RAP')"
                    Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.ExecuteNonQuery()

                    StrSql = "DELETE FROM " & cnAdminDb & "..ITEMTAG WHERE STYLENO='" & StyleNo & "' "
                    StrSql += " AND ISSDATE = '" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(TOFLAG,'')='' AND RECTYPE='RAP'"
                    StrSql += " AND ISNULL(BATCHNO,'')='' "
                    Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.ExecuteNonQuery()
                End If
            End If
        End With
    End Sub


    Private Sub InsertStoneDetails(ByVal CTag As Boolean, ByVal IssSno As String, ByVal StyleNo As String _
                , ByVal TNO As Integer, ByVal StnItem As String, ByVal StnSubItem As String _
                , ByVal Cut As String, ByVal Color As String _
                , ByVal Clarity As String, ByVal Shape As String _
                , ByVal StnPcs As Integer, ByVal StnWt As Decimal _
                , ByVal StnAmt As Decimal, ByVal Tagsno As String _
                , ByVal Tagno As String, ByVal itemId As Integer _
                , Optional ByVal taxx As Decimal = Nothing)
        Dim stnItemId As Integer = 0
        Dim stnSubItemid As Integer = 0
        Dim stnCatCode As String = Nothing
        Dim vat As Double = Nothing
        Dim sno As String = Nothing
        Dim calType As String = ""
        Dim stoneunit As String = ""

        sno = GetNewSno(TranSnoType.RECEIPTSTONECODE, tran)
        ''Find stnCatCode
        StrSql = " SELECT CATCODE,CALTYPE,STONEUNIT"
        StrSql += " FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & StnItem & "'"
        Dim dtItemDetail As New DataTable
        Cmd = New OleDbCommand(StrSql, cn, tran)
        Da = New OleDbDataAdapter(Cmd)
        Da.Fill(dtItemDetail)
        If dtItemDetail.Rows.Count > 0 Then
            With dtItemDetail.Rows(0)
                calType = .Item("CALTYPE").ToString
                stnCatCode = .Item("CATCODE").ToString
                stoneunit = .Item("STONEUNIT").ToString
            End With
        End If

        ''Find itemId
        StrSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & StnItem & "'"
        stnItemId = Val(objGPack.GetSqlValue(StrSql, , , tran))
        StrSql = " SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & StnSubItem & "'"
        stnSubItemid = Val(objGPack.GetSqlValue(StrSql, , , tran))

        If ChkPurchase.Checked Then
            StrSql = " INSERT INTO " & cnStockDb & ".."
            If Trantype = "IAP" Then
                StrSql += "ISSSTONE"
            Else
                StrSql += "RECEIPTSTONE"
            End If
            StrSql += " ("
            StrSql += " SNO,ISSSNO,TRANNO,TRANDATE,TRANTYPE"
            StrSql += " ,STNPCS,STNWT,STNRATE,STNAMT"
            StrSql += " ,STNITEMID,STNSUBITEMID,CALCMODE,STONEUNIT"
            StrSql += " ,STONEMODE,TRANSTATUS,COSTID,COMPANYID"
            StrSql += " ,BATCHNO,SYSTEMID,VATEXM,CATCODE,TAX,APPVER,DISCOUNT"
            StrSql += ",OCATCODE,SEIVE"
            StrSql += " )"
            StrSql += " VALUES"
            StrSql += " ("
            StrSql += " '" & sno & "'" ''SNO
            StrSql += " ,'" & IssSno & "'" 'ISSSNO
            StrSql += " ," & TNO & "" 'TRANNO
            StrSql += " ,'" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
            StrSql += " ,'" & Trantype & "'" 'TRANTYPE
            StrSql += " ," & StnPcs & "" 'STNPCS
            StrSql += " ," & StnWt & "" 'STNWT
            StrSql += " ,0"  'STNRATE
            StrSql += " ," & StnAmt & "" 'STNAMT
            StrSql += " ," & stnItemId & "" 'STNITEMID
            StrSql += " ," & stnSubItemid & "" 'STNSUBITEMID
            StrSql += " ,'" & calType & "'" '" 'CALCMODE
            StrSql += " ,'" & stoneunit & "'" 'STONEUNIT
            StrSql += " ,''" 'STONEMODE 
            StrSql += " ,''" 'TRANSTATUS
            StrSql += " ,'" & CostCenterId & "'" 'COSTID
            StrSql += " ,'" & strCompanyId & "'" 'COMPANYID
            StrSql += " ,'" & BatchNo & "'" 'BATCHNO
            StrSql += " ,'" & systemId & "'" 'SYSTEMID
            StrSql += " ,''" 'VATEXM    
            StrSql += " ,'" & stnCatCode & "'" 'CATCODE
            StrSql += " ," & vat & "" 'TAX
            StrSql += " ,'" & VERSION & "'" 'APPVER
            StrSql += " ,0" 'DISCOUNT
            StrSql += " ,''" 'OCATCODE
            StrSql += " ,''" 'SEIVE
            StrSql += " )"
            ExecQuery(SyncMode.Transaction, StrSql, cn, tran, CostCenterId)
        End If

        'INSERT ITEMTAGSTONE
        If Trantype <> "IAP" Then
            Dim stnSno As String = GetNewSno(TranSnoType.ITEMTAGSTONECODE, tran, "GET_ADMINSNO_TRAN")
            Dim SHAPEid, Colorid, Clarityid, Settypeid, CutId As Integer
            CutId = Val(objGPack.GetSqlValue("SELECT CUTID FROM " & cnAdminDb & "..STNCUT WHERE CUTNAME='" & Cut & "' ", , 0, tran).ToString)
            Colorid = Val(objGPack.GetSqlValue("SELECT COLORID FROM " & cnAdminDb & "..STNCOLOR WHERE COLORNAME='" & Color & "' ", , 0, tran).ToString)
            Clarityid = Val(objGPack.GetSqlValue("SELECT CLARITYID FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYNAME='" & Clarity & "' ", , 0, tran).ToString)
            SHAPEid = Val(objGPack.GetSqlValue("SELECT SHAPEID FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPENAME='" & Shape & "' ", , 0, tran).ToString)
            'Settypeid = Val(objGPack.GetSqlValue("SELECT SETTYPEID FROM " & cnAdminDb & "..STNSETTYPE WHERE SETTYPENAME='" & stRow.Item("SETTINGTYPE").ToString & "' ", , 0, tran).ToString)
            StrSql = " INSERT INTO " & cnAdminDb & ".."
            If CTag Then
                StrSql += " CITEMTAGSTONE("
            Else
                StrSql += " ITEMTAGSTONE("
            End If
            StrSql += " SNO,TAGSNO,ITEMID,COMPANYID,STNITEMID,"
            StrSql += " STNSUBITEMID,TAGNO,STNPCS,STNWT,"
            StrSql += " STNRATE,STNAMT,DESCRIP,"
            StrSql += " RECDATE,CALCMODE,"
            StrSql += " MINRATE,SIZECODE,STONEUNIT,ISSDATE,"
            StrSql += " OLDTAGNO,VATEXM,CARRYFLAG,COSTID,SYSTEMID,APPVER,"
            StrSql += " USRATE,INDRS,PACKETNO,SHAPEID,CUTID,COLORID,CLARITYID,SETTYPEID"
            StrSql += " )VALUES("
            StrSql += " '" & stnSno & "'" ''SNO
            StrSql += " ,'" & Tagsno & "'" 'TAGSNO
            StrSql += " ,'" & itemId & "'" 'ITEMID
            StrSql += " ,'" & strCompanyId & "'" 'COMPANYID
            StrSql += " ," & stnItemId & "" 'STNITEMID
            StrSql += " ," & stnSubItemid & "" 'STNSUBITEMID
            StrSql += " ,'" & Tagno & "'" 'TAGNO
            StrSql += " ," & StnPcs & "" 'STNPCS
            StrSql += " ," & StnWt & "" 'STNWT
            StrSql += " ,0" 'STNRATE
            StrSql += " ," & StnAmt & "" 'STNAMT
            StrSql += " ,'" & StnItem & "'"
            StrSql += " ,'" & GetEntryDate(dtpTrandate.Value, tran).ToString("yyyy-MM-dd") & "'" 'RECDATE
            StrSql += " ,'" & calType & "'" 'CALCMODE
            StrSql += " ,0" 'MINRATE
            StrSql += " ,0" 'SIZECODE
            StrSql += " ,'" & stoneunit & "'" 'STONEUNIT
            StrSql += " ,NULL" 'ISSDATE
            StrSql += " ,''" 'OLDTAGNO
            StrSql += " ,''" 'VATEXM
            StrSql += " ,''" 'CARRYFLAG
            StrSql += " ,'" & cnCostId & "'" 'COSTID
            StrSql += " ,'" & systemId & "'" 'SYSTEMID
            StrSql += " ,'" & VERSION & "'" 'APPVER
            StrSql += " ,0" 'USRATE
            StrSql += " ,0" 'INDRS
            StrSql += " ,''" 'PACKETNO
            StrSql += " ," & SHAPEid & "" 'SHAPEID
            StrSql += " ," & CutId & "" 'CUTID
            StrSql += " ," & Colorid & "" 'COLORID
            StrSql += " ," & Clarityid & "" 'CLARITYID
            StrSql += " ," & Settypeid & "" 'SETTYPEID
            StrSql += " )"
            Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.ExecuteNonQuery()

            'INSERT PURCHASE DETAIL
            StrSql = vbCrLf + " INSERT INTO " & cnAdminDb & ".."
            If CTag Then
                StrSql += "CPURITEMTAGSTONE"
            Else
                StrSql += "PURITEMTAGSTONE"
            End If
            StrSql += vbCrLf + " ("
            StrSql += vbCrLf + " TAGSNO,ITEMID,TAGNO"
            StrSql += vbCrLf + " ,STNITEMID,STNSUBITEMID,STNPCS,STNWT,STNRATE,STNAMT,STONEUNIT,CALCMODE"
            StrSql += vbCrLf + " ,PURRATE,PURAMT,COMPANYID,COSTID,STNSNO"
            StrSql += vbCrLf + " )"
            StrSql += vbCrLf + " VALUES"
            StrSql += vbCrLf + " ("
            StrSql += vbCrLf + " '" & Tagsno & "'" 'TAGSNO
            StrSql += vbCrLf + " ," & itemId & "" 'ITEMID
            StrSql += vbCrLf + " ,'" & Tagno & "'" 'TAGNO
            StrSql += vbCrLf + " ," & stnItemId & "" 'STNITEMID
            StrSql += vbCrLf + " ," & stnSubItemid & "" 'STNSUBITEMID
            StrSql += vbCrLf + " ," & StnPcs & "" 'STNPCS
            StrSql += vbCrLf + " ," & StnWt & "" 'STNWT
            StrSql += vbCrLf + " ,0"   'STNRATE
            StrSql += vbCrLf + " ," & StnAmt & "" 'STNAMT
            StrSql += vbCrLf + " ,'" & stoneunit & "'" 'STONEUNIT
            StrSql += vbCrLf + " ,'" & calType & "'" 'CALCMODE
            StrSql += vbCrLf + " ,0" 'PURRATE
            StrSql += vbCrLf + " ," & StnAmt & "" 'PURAMT
            StrSql += vbCrLf + " ,'" & GetStockCompId() & "'"
            StrSql += vbCrLf + " ,'" & cnCostId & "'"
            StrSql += vbCrLf + " ,'" & stnSno & "'"
            StrSql += vbCrLf + " )"
            Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.ExecuteNonQuery()
        Else
            Dim ColList As String
            ColList = GetColumnNames(cnAdminDb, "CITEMTAGSTONE", tran)
            StrSql = vbCrLf + " INSERT INTO " & cnAdminDb & "..CITEMTAGSTONE(" & ColList & ")"
            StrSql += vbCrLf + " SELECT  " & ColList & " FROM " & cnAdminDb & "..ITEMTAGSTONE "
            StrSql += vbCrLf + " WHERE TAGSNO IN("
            StrSql += vbCrLf + " SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE STYLENO='" & StyleNo & "' "
            StrSql += vbCrLf + " AND ISSDATE IS NULL AND ISNULL(TOFLAG,'')='' AND ISNULL(BATCHNO,'')='' AND RECTYPE='RAP')"
            Cmd = New OleDbCommand(StrSql, cn, tran)
            Cmd.ExecuteNonQuery()

            ColList = GetColumnNames(cnAdminDb, "CPURITEMTAGSTONE", tran)
            StrSql = vbCrLf + " INSERT INTO " & cnAdminDb & "..CPURITEMTAGSTONE(" & ColList & ")"
            StrSql += vbCrLf + " SELECT " & ColList & " FROM " & cnAdminDb & "..PURITEMTAGSTONE "
            StrSql += vbCrLf + " WHERE TAGSNO IN("
            StrSql += vbCrLf + " SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE STYLENO='" & StyleNo & "' "
            StrSql += vbCrLf + " AND ISSDATE IS NULL AND ISNULL(TOFLAG,'')='' AND ISNULL(BATCHNO,'')='' AND RECTYPE='RAP')"
            Cmd = New OleDbCommand(StrSql, cn, tran)
            Cmd.ExecuteNonQuery()
        End If
    End Sub

    Private Function Getidwithlen(ByVal CItemid As String, ByVal len As Integer) As String
        If CItemid = Nothing Then CItemid = ""
        Dim RetStr As String = ""
        If len = 0 Then len = 1
        For cnt As Integer = 1 To len - CItemid.ToString.Length
            RetStr += "0"
        Next
        RetStr += CItemid
        Return RetStr
    End Function

    Private Function GenTagNo(ByRef TagNo As String, Optional ByVal tran As OleDbTransaction = Nothing, Optional ByVal increament As Boolean = True) As String
        Dim str As String = Nothing
        If IsNumeric(TagNo) Then
            If increament Then TagNo = Val(TagNo) + 1 Else TagNo = Val(TagNo) - 1
        Else
            Dim fPart As String = Nothing
            Dim sPart As String = Nothing
            For Each c As Char In TagNo
                If IsNumeric(c) Then
                    sPart += c
                Else
                    fPart += c
                End If
            Next
            If increament Then TagNo = fPart + (Val(sPart) + 1).ToString Else TagNo = fPart + (IIf(Val(sPart) - 1 > 0, Val(sPart) - 1, 0)).ToString
        End If
        Dim RetStr As String = ""
        For cnt As Integer = 1 To 4 - TagNo.ToString.Length
            RetStr += "0"
        Next
        RetStr += TagNo
        Return RetStr
    End Function

    Private Sub cmbAcName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAcName.GotFocus
        If cmbCostCentre.Enabled = True And cmbCostCentre.Text = "" Then cmbCostCentre.Focus()
    End Sub

    Private Sub dtpCreditDays_Enter(ByVal sender As Object, ByVal e As System.EventArgs)
        SendKeys.Send("{tab}")
    End Sub


    Private Sub cmbCostCentre_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        CostCenterId = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre.Text & "'", , , tran)
    End Sub

    Private Sub cmbAcName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbAcName.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            StrSql = " SELECT TOP 1 DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME='" & cmbAcName.Text & "')"
            designerid = objGPack.GetSqlValue(StrSql, , 0, )
            If designerid = 0 Then MsgBox("Designer id not available for this acname,Check designer master", MsgBoxStyle.Information) : Me.SelectNextControl(cmbAcName, False, True, True, False) : Exit Sub
            Maxwastper = 0
            Maxwastage = 0
            MaxmcGrm = 0
            Maxmc = 0
        End If
    End Sub

    Private Sub cmbAcName_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAcName.SelectedIndexChanged
        lblAddress.Text = ""
        StrSql = " SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME ='" & cmbAcName.Text & "'"
        Dim daAcc As New OleDbDataAdapter(StrSql, cn)
        Dim dtAccode As New DataTable()
        daAcc.Fill(dtAccode)
        If dtAccode.Rows.Count > 0 Then
            acCode = dtAccode.Rows(0).Item("ACCODE").ToString
        End If

        StrSql = " SELECT DISTINCT ADDRESS1 as ADDRESS FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE='" & acCode & "'"
        StrSql += " UNION ALL"
        StrSql += " SELECT DISTINCT ADDRESS2 AS ADDRESS FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE='" & acCode & "'"
        Da = New OleDbDataAdapter(StrSql, cn)
        Dim dtAdd As New DataTable
        Da.Fill(dtAdd)
        If dtAdd.Rows.Count > 0 Then
            For i As Integer = 0 To dtAdd.Rows.Count - 1
                lblAddress.Text = lblAddress.Text + dtAdd.Rows(i).Item("ADDRESS").ToString
            Next
        End If
        If cmbAcName.Text.Trim <> "" And cmbAcName.Text.Trim <> "System.Data.DataRowView" Then
            StrSql = " SELECT TOP 1 DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME='" & cmbAcName.Text & "')"
            designerid = objGPack.GetSqlValue(StrSql, , 0, )
            If designerid = 0 Then MsgBox("Designer id not available for this acname,Check designer master", MsgBoxStyle.Information) : Me.SelectNextControl(cmbAcName, False, True, True, False) : Exit Sub

            StrSql = " SELECT MAXWASTPER,MAXWAST,MAXMCGRM,MAXMC FROM " & cnAdminDb & "..WMCTABLE WHERE DESIGNERID = '" & designerid & "'"
            Dim dtItemDetail As New DataTable
            Cmd = New OleDbCommand(StrSql, cn, tran)
            Da = New OleDbDataAdapter(Cmd)
            Da.Fill(dtItemDetail)
            If dtItemDetail.Rows.Count > 0 Then
                With dtItemDetail.Rows(0)
                    Maxwastper = Val(.Item("MAXWASTPER").ToString)
                    Maxwastage = Val(.Item("MAXWAST").ToString)
                    MaxmcGrm = Val(.Item("MAXMCGRM").ToString)
                    Maxmc = Val(.Item("MAXMC").ToString)
                End With
            End If
        End If
    End Sub
    Private Sub cmbCostCentre_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCostCentre.TextChanged
        CostCenterId = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre.Text & "'", , , tran)
    End Sub


    Private Sub BtnExcelDownload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnExcelDownload.Click
        If CmbTrantype.Text = "" Then MsgBox("Trantype Should not Empty", MsgBoxStyle.Information) : Exit Sub
        Dim OpenDialog As New OpenFileDialog
        Dim Str As String
        Str = "(*.xls) | *.xls"
        Str += "|(*.xlsx) | *.xlsx"
        OpenDialog.Filter = Str
        If OpenDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            Dim path As String = OpenDialog.FileName
            If path <> "" Then
                LoadFromExel(path)
            End If
        End If
    End Sub
    Private Sub LoadFromExel(ByVal Path As String)
        Try

            DtTran.Rows.Clear()
            DtTran.AcceptChanges()
            Dim MyConnection As System.Data.OleDb.OleDbConnection
            'MyConnection = New OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;Data Source='" & Path & "';Extended Properties=Excel 8.0;")
            MyConnection = New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" & Path & "';Extended Properties=""Excel 12.0 Xml;HDR=YES;""")
            StrSql = "SELECT * FROM [SHEET1$]"
            Da = New OleDbDataAdapter(StrSql, MyConnection)
            Dim Dt As New DataTable
            Da.Fill(Dt)
            MyConnection.Close()
            Dim Itemname As String = ""
            Dim SubItemname As String = ""
            Dim Itemctrname As String = ""
            Dim Designer As String = ""
            Dim StnItem As String = "" : Dim StnSubItem As String = ""
            Dim StnItem1 As String = "" : Dim StnItem2 As String = "" : Dim StnItem3 As String = ""
            Dim StnItem4 As String = "" : Dim StnItem5 As String = "" : Dim StnItem6 As String = ""
            Dim StnSubItem1 As String = "" : Dim StnSubItem2 As String = "" : Dim StnSubItem3 As String = ""
            Dim StnSubItem4 As String = "" : Dim StnSubItem5 As String = "" : Dim StnSubItem6 As String = ""
            If Dt.Rows.Count > 0 Then
                Dim StyleNo As String = ""
                Dim v As Integer = 1
                If CmbTrantype.Text = "PURCHASE APPROVAL RECEIPT" Then
                    Trantype = "RAP"
                ElseIf CmbTrantype.Text = "PURCHASE APPROVAL RETURN" Then
                    Trantype = "IAP"
                Else
                    Trantype = "RPU"
                End If
                For i As Integer = 0 To Dt.Rows.Count - 1
                    If Dt.Rows(i).Item("STYLENO").ToString <> "" Then
                        StyleNo = Dt.Rows(i).Item("STYLENO").ToString
                    End If
                    If Dt.Rows(i).Item("ITEM").ToString <> "" Then
                        Itemname = Dt.Rows(i).Item("ITEM").ToString
                    End If
                    If Dt.Rows(i).Item("SUBITEM").ToString <> "" Then
                        SubItemname = Dt.Rows(i).Item("SUBITEM").ToString
                    End If
                    If Dt.Columns.Contains("STNITEM1") Then
                        If Dt.Rows(i).Item("STNITEM1").ToString <> "" Then
                            StnItem1 = Dt.Rows(i).Item("STNITEM1").ToString
                        End If
                        If Dt.Rows(i).Item("STNSUBITEM1").ToString <> "" Then
                            StnSubItem1 = Dt.Rows(i).Item("STNSUBITEM1").ToString
                        End If
                    End If
                    If Dt.Columns.Contains("STNITEM2") Then
                        If Dt.Rows(i).Item("STNITEM2").ToString <> "" Then
                            StnItem2 = Dt.Rows(i).Item("STNITEM2").ToString
                        End If
                        If Dt.Rows(i).Item("STNSUBITEM2").ToString <> "" Then
                            StnSubItem2 = Dt.Rows(i).Item("STNSUBITEM2").ToString
                        End If
                    End If
                    If Dt.Columns.Contains("STNITEM3") Then
                        If Dt.Rows(i).Item("STNITEM3").ToString <> "" Then
                            StnItem3 = Dt.Rows(i).Item("STNITEM3").ToString
                        End If
                        If Dt.Rows(i).Item("STNSUBITEM3").ToString <> "" Then
                            StnSubItem3 = Dt.Rows(i).Item("STNSUBITEM3").ToString
                        End If
                    End If
                    If Dt.Columns.Contains("STNITEM4") Then
                        If Dt.Rows(i).Item("STNITEM4").ToString <> "" Then
                            StnItem4 = Dt.Rows(i).Item("STNITEM4").ToString
                        End If
                        If Dt.Rows(i).Item("STNSUBITEM4").ToString <> "" Then
                            StnSubItem4 = Dt.Rows(i).Item("STNSUBITEM4").ToString
                        End If
                    End If
                    If Dt.Columns.Contains("STNITEM5") Then
                        If Dt.Rows(i).Item("STNITEM5").ToString <> "" Then
                            StnItem5 = Dt.Rows(i).Item("STNITEM5").ToString
                        End If
                        If Dt.Rows(i).Item("STNSUBITEM5").ToString <> "" Then
                            StnSubItem5 = Dt.Rows(i).Item("STNSUBITEM5").ToString
                        End If
                    End If
                    If Dt.Columns.Contains("STNITEM6") Then
                        If Dt.Rows(i).Item("STNITEM6").ToString <> "" Then
                            StnItem6 = Dt.Rows(i).Item("STNITEM6").ToString
                        End If
                        If Dt.Rows(i).Item("STNSUBITEM6").ToString <> "" Then
                            StnSubItem6 = Dt.Rows(i).Item("STNSUBITEM6").ToString
                        End If
                    End If
                    If Dt.Rows(i).Item("DESIGNERNAME").ToString <> "" Then
                        Designer = Dt.Rows(i).Item("DESIGNERNAME").ToString
                    End If
                    If Val(Dt.Rows(i).Item("GRSWT").ToString) <> 0 And Val(Dt.Rows(i).Item("NETWT").ToString) <> 0 And Itemname <> "" Then
                        StrSql = " SELECT 1 FROM " & cnAdminDb & "..ITEMTAG WHERE STYLENO='" & StyleNo & "'"
                        If objGPack.GetSqlValue(StrSql, , 0) = 1 Then
                            If Trantype <> "IAP" Then
                                Dim TagExists As Boolean = True
                                If Trantype = "RPU" Then
                                    StrSql = " SELECT 1 FROM " & cnAdminDb & "..ITEMTAG WHERE STYLENO='" & StyleNo & "' AND ISNULL(BATCHNO,'')<>'' AND RECTYPE='IAP'"
                                    If objGPack.GetSqlValue(StrSql, , 0) = 1 Then
                                        TagExists = False
                                    End If
                                End If
                                If TagExists And POS_TAGXLDOWN_TAGNO = False Then MsgBox(StyleNo & " StyleNo Already Exists ", MsgBoxStyle.Information)
                            End If
                        End If
                        StrSql = " SELECT 1 FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & Itemname & "'"
                        If objGPack.GetSqlValue(StrSql, , 0) <> 1 Then
                            MsgBox(Itemname & " Not found in ITEMMAST ", MsgBoxStyle.Information)
                        End If
                        If SubItemname <> "" Then
                            StrSql = " SELECT 1 FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME='" & SubItemname & "'"
                            If objGPack.GetSqlValue(StrSql, , 0) <> 1 Then
                                MsgBox(SubItemname & " Not found in SUBITEMMAST ", MsgBoxStyle.Information)
                            End If
                        End If
                        If Designer <> "" Then
                            StrSql = " SELECT 1 FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME='" & Designer & "'"
                            If objGPack.GetSqlValue(StrSql, , 0) <> 1 Then
                                MsgBox(Designer & " Not found in DesignerId ", MsgBoxStyle.Information)
                            End If
                        End If
                        Dim dtrow As DataRow = Nothing
                        dtrow = DtTran.NewRow
                        If CheckExists("ITEMTAG", "STYLENO", StyleNo) = True Then
                            If Trantype <> "IAP" Then
                                Dim TagExists As Boolean = True
                                If Trantype = "RPU" Then
                                    StrSql = " SELECT 1 FROM " & cnAdminDb & "..ITEMTAG WHERE STYLENO='" & StyleNo & "' AND ISNULL(BATCHNO,'')<>'' AND RECTYPE='IAP'"
                                    If objGPack.GetSqlValue(StrSql, , 0) = 1 Then
                                        TagExists = False
                                    End If
                                End If
                                If TagExists And POS_TAGXLDOWN_TAGNO = False Then dtrow("REMARKS") = "TN,"
                            End If
                        End If
                        If CheckExists("ITEMMAST", "ITEMNAME", Itemname) = False Then
                            dtrow("REMARKS") = dtrow("REMARKS") & "IM,"
                        End If
                        If SubItemname <> "" Then
                            If CheckExists("SUBITEMMAST", "SUBITEMNAME", SubItemname) = False Then
                                dtrow("REMARKS") = dtrow("REMARKS") & "SM,"
                            End If
                        End If
                        If Designer <> "" Then
                            If CheckExists("DESIGNER", "DESIGNERNAME", Designer) = False Then
                                dtrow("REMARKS") = dtrow("REMARKS") & "DE,"
                            End If
                        End If

                        dtrow("NO") = v
                        dtrow("SNO") = v
                        dtrow("STYLENO") = StyleNo
                        dtrow("CERTNO") = Dt.Rows(i).Item("CERTNO").ToString
                        dtrow("ITEM") = Itemname
                        dtrow("SUBITEM") = SubItemname
                        dtrow("PURITY") = Val(Dt.Rows(i).Item("PURITY").ToString)
                        dtrow("PCS") = Val(Dt.Rows(i).Item("PCS").ToString)
                        dtrow("GRSWT") = Format(Val(Dt.Rows(i).Item("GRSWT").ToString.Replace(",", "")), "0.000")
                        dtrow("SIZE") = Dt.Rows(i).Item("SIZE").ToString
                        dtrow("NETWT") = Format(Val(Dt.Rows(i).Item("NETWT").ToString.Replace(",", "")), "0.000")
                        dtrow("WASTPER") = Format(Val(Dt.Rows(i).Item("WASTPER").ToString.Replace(",", "")), "0.00")
                        dtrow("WASTPER") = Format(Val(Dt.Rows(i).Item("WASTPER").ToString.Replace(",", "")), "0.00")
                        dtrow("RATE") = Format(Val(Dt.Rows(i).Item("RATE").ToString.Replace(",", "")), "0.00")
                        dtrow("MCGRM") = Format(Val(Dt.Rows(i).Item("MCGRM").ToString.Replace(",", "")), "0.00")
                        dtrow("AMOUNT") = Format(Val(Dt.Rows(i).Item("AMOUNT").ToString.Replace(",", "")), "0.00")
                        dtrow("SALEAMOUNT") = Format(Val(Dt.Rows(i).Item("SALEAMOUNT").ToString.Replace(",", "")), "0.00")
                        dtrow("DESIGNERNAME") = Dt.Rows(i).Item("DESIGNERNAME").ToString
                        If Dt.Columns.Contains("ITEMTYPE") Then
                            dtrow("ITEMTYPE") = Dt.Rows(i).Item("ITEMTYPE").ToString
                        End If
                        dtrow("COLHEAD") = "G"
                        DtTran.Rows.Add(dtrow)
                        DtTran.AcceptChanges()

                        StnItem = StnItem1
                        StnSubItem = StnSubItem1
                        Dim StnInd As Integer = 1
NextStone:
                        If StnItem <> "" Then
                            Dim Cut As Boolean = False
                            Dim Color As Boolean = False
                            Dim Clarity As Boolean = False
                            Dim Shape As Boolean = False
                            Dim dtstrow As DataRow = Nothing
                            dtstrow = DtTran.NewRow
                            If StnItem <> "" Then
                                If CheckExists("ITEMMAST", "ITEMNAME", StnItem) = False Then
                                    dtstrow("REMARKS") = dtstrow("REMARKS") & "IM,"
                                End If
                            End If
                            If StnSubItem <> "" Then
                                If CheckExists("SUBITEMMAST", "SUBITEMNAME", StnSubItem) = False Then
                                    dtstrow("REMARKS") = dtstrow("REMARKS") & "SI,"
                                End If
                            End If
                            If Dt.Columns.Contains("CUT" & StnInd) Then
                                Cut = True
                                If Dt.Rows(i).Item("CUT" & StnInd).ToString <> "" Then
                                    If CheckExists("STNCUT", "CUTNAME", Dt.Rows(i).Item("CUT" & StnInd).ToString) = False Then
                                        dtstrow("REMARKS") = dtstrow("REMARKS") & "CU,"
                                    End If
                                End If
                            End If
                            If Dt.Columns.Contains("COLOR" & StnInd) Then
                                Color = True
                                If Dt.Rows(i).Item("COLOR" & StnInd).ToString <> "" Then
                                    If CheckExists("STNCOLOR", "COLORNAME", Dt.Rows(i).Item("COLOR" & StnInd).ToString) = False Then
                                        dtstrow("REMARKS") = dtstrow("REMARKS") & "CO,"
                                    End If
                                End If
                            End If
                            If Dt.Columns.Contains("CLARITY" & StnInd) Then
                                Clarity = True
                                If Dt.Rows(i).Item("CLARITY" & StnInd).ToString <> "" Then
                                    If CheckExists("STNCLARITY", "CLARITYNAME", Dt.Rows(i).Item("CLARITY" & StnInd).ToString) = False Then
                                        dtstrow("REMARKS") = dtstrow("REMARKS") & "CL,"
                                    End If
                                End If
                            End If
                            If Dt.Columns.Contains("SHAPE" & StnInd) Then
                                Shape = True
                                If Dt.Rows(i).Item("SHAPE" & StnInd).ToString <> "" Then
                                    If CheckExists("STNSHAPE", "SHAPENAME", Dt.Rows(i).Item("SHAPE" & StnInd).ToString) = False Then
                                        dtstrow("REMARKS") = dtstrow("REMARKS") & "SH,"
                                    End If
                                End If
                            End If
                            dtstrow("NO") = v
                            dtstrow("ITEM") = StnItem
                            dtstrow("SUBITEM") = StnSubItem
                            dtstrow("PCS") = Val(Dt.Rows(i).Item("STNPCS" & StnInd).ToString)
                            dtstrow("GRSWT") = Format(Val(Dt.Rows(i).Item("STNWT" & StnInd).ToString), "0.000")
                            dtstrow("AMOUNT") = Format(Val(Dt.Rows(i).Item("STNAMT" & StnInd).ToString.Replace(",", "")), "0.00")
                            If Cut Then dtstrow("CUT") = Dt.Rows(i).Item("CUT" & StnInd).ToString Else dtstrow("CUT") = ""
                            If Color Then dtstrow("COLOR") = Dt.Rows(i).Item("COLOR" & StnInd).ToString Else dtstrow("COLOR") = ""
                            If Clarity Then dtstrow("CLARITY") = Dt.Rows(i).Item("CLARITY" & StnInd).ToString Else dtstrow("CLARITY") = ""
                            If Shape Then dtstrow("SHAPE") = Dt.Rows(i).Item("SHAPE" & StnInd).ToString Else dtstrow("SHAPE") = ""
                            dtstrow("COLHEAD") = "S"
                            DtTran.Rows.Add(dtstrow)
                            DtTran.AcceptChanges()
                            StnInd += 1
                            If StnInd = 2 Then
                                StnItem = StnItem2
                                StnSubItem = StnSubItem2
                            ElseIf StnInd = 3 Then
                                StnItem = StnItem3
                                StnSubItem = StnSubItem3
                            ElseIf StnInd = 4 Then
                                StnItem = StnItem4
                                StnSubItem = StnSubItem4
                            ElseIf StnInd = 5 Then
                                StnItem = StnItem5
                                StnSubItem = StnSubItem5
                            ElseIf StnInd = 6 Then
                                StnItem = StnItem6
                                StnSubItem = StnSubItem6
                            End If
                            Cut = False : Color = False : Clarity = False : Shape = False
                            GoTo NextStone
                        End If
                        v += 1
                    End If
                    StnItem1 = "" : StyleNo = "" : Designer = "" : SubItemname = "" : Itemname = ""
                    StnItem2 = "" : StnItem3 = "" : StnItem4 = "" : StnItem5 = "" : StnItem6 = ""
                    StnSubItem1 = "" : StnSubItem2 = "" : StnSubItem3 = "" : StnSubItem4 = "" : StnSubItem5 = ""
                    StnSubItem6 = ""
                Next
                CmbTrantype.Enabled = False
                FormatedGridStyle(DgvTran)
            End If
        Catch ex As Exception
            DtTran.Rows.Clear()
            DtTran.AcceptChanges()
            MsgBox("Invalid File Format", MsgBoxStyle.Information)
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try
    End Sub

    Function CheckExists(ByVal TableName As String, ByVal GetName As String, ByVal Value As String)
        If Value = "" And ListAllowEmptyValues.Contains(TableName) Then Return True
        StrSql = " SELECT 1 FROM " & cnAdminDb & ".." & TableName & " WHERE " & GetName & "='" & Value & "' "
        If objGPack.GetSqlValue(StrSql, , 0) <> 1 Then
            Return False
        End If
        Return True
    End Function

    Private Sub DgvTran_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles DgvTran.CellFormatting
        If DgvTran.Rows(e.RowIndex).Cells("COLHEAD").Value.ToString = "G" Then
            DgvTran.Rows(e.RowIndex).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        ElseIf DgvTran.Rows(e.RowIndex).Cells("COLHEAD").Value.ToString = "S" Then
            DgvTran.Rows(e.RowIndex).DefaultCellStyle.Font = New Font("VERDANA", 7.25!, FontStyle.Bold)
        End If
    End Sub

    Private Sub DgvTran_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DgvTran.CellValueChanged
        If DgvTran.Rows.Count > 0 Then
            If DgvTran.Rows(DgvTran.CurrentRow.Index).Cells("COLHEAD").Value.ToString = "G" Then
                calcmaxval(DgvTran.CurrentRow.Index)
                CalcSaleValue(DgvTran.CurrentRow.Index)
                FormatedGridStyle(DgvTran)
            ElseIf DgvTran.Rows(DgvTran.CurrentRow.Index).Cells("COLHEAD").Value.ToString <> "G" Then
                calcStnrate(DgvTran.CurrentRow.Index)
                calcStnamt(DgvTran.CurrentRow.Index, Val(DgvTran.Rows(DgvTran.CurrentRow.Index).Cells("SRATE").Value.ToString))
                For k As Integer = 0 To DtTran.Rows.Count - 1
                    If DtTran.Rows(k).Item("COLHEAD").ToString = "G" Then
                        CalcToTStnamt(k)
                        CalcSaleValue(k)
                        FormatedGridStyle(DgvTran)
                    End If
                Next
            End If
        End If
    End Sub

    Private Sub calcmaxval(ByVal index As Integer)
        Dim wmcWastPer As Double = Val(DtTran.Rows(index).Item("MAXWASTPER").ToString)
        Dim wmcWast As Double = Val(DtTran.Rows(index).Item("MAXWASTAGE").ToString)
        Dim wmcMcGrm As Double = Val(DtTran.Rows(index).Item("MAXMCGRM").ToString)
        Dim wmcMc As Double = Val(DtTran.Rows(index).Item("MAXMC").ToString)
        Dim wt As Double = 0
        wt = IIf(grsnet = "N", Val(DtTran.Rows(index).Item("NETWT").ToString), Val(DtTran.Rows(index).Item("GRSWT").ToString))

        If wmcWastPer <> 0 Then
            DtTran.Rows(index).Item("MAXWASTAGE") = Format((wt / 100) * Val(DtTran.Rows(index).Item("MAXWASTPER").ToString), "0.000")
        End If
        If wmcMcGrm <> 0 Then
            DtTran.Rows(index).Item("MAXMC") = Format(SALEVALUE_ROUND(Val(DtTran.Rows(index).Item("MAXMCGRM").ToString) * wt), "0.00")
        End If
        DtTran.AcceptChanges()
    End Sub

    Private Sub CalcToTStnamt(ByVal index As Integer)
        Dim drstone() As DataRow = DtTran.Select("TAGNO='" & DtTran.Rows(index).Item("TAGNO").ToString.Trim & "' AND COLHEAD <>'G' and COLHEAD IS NOT NULL")
        Dim stnamt As Decimal = 0
        Dim sstnamt As Decimal = 0

        For k As Integer = 0 To drstone.Length - 1
            sstnamt += Val(drstone(k).Item("SSTNAMT").ToString)
            stnamt += Val(drstone(k).Item("STNAMT").ToString)
        Next
        DtTran.Rows(index).Item("SSTNAMT") = Format(IIf(sstnamt <> 0, Round(sstnamt), 0), "0.00")
        DtTran.Rows(index).Item("STNAMT") = Format(IIf(stnamt <> 0, Round(stnamt), 0), "0.00")

        DtTran.AcceptChanges()
    End Sub
    Private Sub CalcSaleValue(ByVal index As Integer)
        Dim amt As Double = Nothing
        Dim calType As String = objGPack.GetSqlValue(" SELECT CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & DtTran.Rows(index).Item("ITEM").ToString & "'")

        If calType = "R" Then
            amt = Val(DtTran.Rows(index).Item("PCS").ToString) * Val(DtTran.Rows(index).Item("RATE").ToString)
        Else
            Dim wt As Double = 0
            Dim rate As Double = Val(DtTran.Rows(index).Item("SRATE").ToString)

            wt = IIf(grsnet = "N", Val(DtTran.Rows(index).Item("NETWT").ToString), Val(DtTran.Rows(index).Item("GRSWT").ToString))

WegithCalc:
            amt = ((wt + Val(DtTran.Rows(index).Item("MAXWASTAGE").ToString)) * rate) _
            + Val(DtTran.Rows(index).Item("MAXMC").ToString) _
            '+ Val(DtTran.Rows(index).Item("STNAMT").ToString)
            amt += IIf(calType = "B", Val(DtTran.Rows(index).Item("RATE").ToString), 0)
            amt += IIf(calType = "F", Val(DtTran.Rows(index).Item("RATE").ToString), 0)
        End If
        amt = Math.Round(amt)
        DtTran.Rows(index).Item("GRSAMT") = Format(IIf(amt <> 0, SALEVALUE_ROUND(amt), 0), "0.00")
        amt += Val(DtTran.Rows(index).Item("SSTNAMT").ToString)
        DtTran.Rows(index).Item("SALVALUE") = Format(IIf(amt <> 0, SALEVALUE_ROUND(amt), 0), "0.00")

        DtTran.Rows(index).Item("TOTAMT") = Format(SALEVALUE_ROUND(Val(DtTran.Rows(index).Item("AMOUNT").ToString) + Val(DtTran.Rows(index).Item("MC").ToString) + Val(DtTran.Rows(index).Item("STNAMT").ToString)), "0.00")

        DtTran.AcceptChanges()
    End Sub
    Private Function SALEVALUE_ROUND(ByVal svalue As Decimal) As Decimal
        If SALVALUEROUND <> 0 Then
            If svalue <> 0 Then
                Dim wholepart As Decimal = Val(svalue) / SALVALUEROUND
                Dim intpart As Decimal = Int(wholepart)
                Dim decpart As Decimal = Round(wholepart - intpart)
                svalue = (intpart + decpart) * SALVALUEROUND
            End If
        End If
        Return svalue
    End Function

    Private Sub calcStnrate(ByVal index As Integer)
        Dim cent As Double = 0
        cent = Val(DtTran.Rows(index).Item("GRSWT").ToString) / IIf(Val(DtTran.Rows(index).Item("PCS").ToString) = 0, 1, Val(DtTran.Rows(index).Item("PCS").ToString))
        cent *= 100
        StrSql = " DECLARE @CENT FLOAT"
        StrSql += vbCrLf + " SET @CENT = " & cent & ""
        StrSql += vbCrLf + " SELECT MAXRATE FROM " & cnAdminDb & "..CENTRATE WHERE"
        StrSql += vbCrLf + " @CENT BETWEEN FROMCENT AND TOCENT "
        StrSql += vbCrLf + " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & DtTran.Rows(index).Item("ITEM").ToString & "')"
        StrSql += vbCrLf + " AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & DtTran.Rows(index).Item("SUBITEM").ToString & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & DtTran.Rows(index).Item("ITEM").ToString & "')),0)"
        'strSql += vbCrLf + " AND ISNULL(ACCODE,'') = ISNULL((SELECT ACCODE FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & cmbDesigner_MAN.Text & "'),'')"
        Dim ColorId As Integer = 0
        Dim SHAPEId As Integer = 0
        Dim ClarityId As Integer = 0
        ColorId = objGPack.GetSqlValue("SELECT COLORID FROM " & cnAdminDb & "..STNCOLOR WHERE COLORNAME = '" & DtTran.Rows(index).Item("COLOR").ToString & "'", "COLORID", 0)
        SHAPEId = objGPack.GetSqlValue("SELECT SHAPEID FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPENAME = '" & DtTran.Rows(index).Item("SHAPE").ToString & "'", "SHAPEID", 0)
        ClarityId = objGPack.GetSqlValue("SELECT CLARITYID FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYNAME = '" & DtTran.Rows(index).Item("CLARITY").ToString & "'", "CLARITYID", 0)
        If ColorId <> 0 Then StrSql += vbCrLf + " AND COLORID=" & ColorId
        If SHAPEId <> 0 Then StrSql += vbCrLf + " AND SHAPEID=" & SHAPEId
        If ClarityId <> 0 Then StrSql += vbCrLf + " AND CLARITYID=" & ClarityId
        Dim rate As Double = Val(objGPack.GetSqlValue(StrSql, "MAXRATE", "", tran))
        If rate <> 0 Then
            DtTran.Rows(index).Item("SRATE") = IIf(rate <> 0, Format(rate, "0.00"), "")
        End If
        If rate > 0 Then calcStnamt(index, rate)
    End Sub
    Private Sub calcStnamt(ByVal index As Integer, ByVal rate As Double)
        Dim wt As Double = 0
        Dim amt As Double = 0
        wt = Val(DtTran.Rows(index).Item("GRSWT").ToString)
        amt = (wt * rate)
        If rate <> 0 Then
            DtTran.Rows(index).Item("SSTNAMT") = Format(IIf(amt <> 0, amt, 0), "0.00")
        Else
            DtTran.Rows(index).Item("SSTNAMT") = Format(0, "0.00")
        End If
    End Sub


    Private Sub ChkPurchase_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkPurchase.CheckedChanged
        GridStyleVisible(DgvTran)
    End Sub

    Private Sub ChkSale_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkSale.CheckedChanged
        GridStyleVisible(DgvTran)
    End Sub
    Function exceltemplate()
        Dim rwStart As Integer = 0
        Dim oXL As Excel.Application
        Dim oWB As Excel.Workbook
        Dim oSheet As Excel.Worksheet
        Dim oRng As Excel.Range

        oXL = CreateObject("Excel.Application")
        oXL.Visible = True
        oWB = oXL.Workbooks.Add
        oSheet = oWB.ActiveSheet

        oSheet.Range("A1").Value = "SNO"
        oSheet.Range("A1").ColumnWidth = 5
        oSheet.Range("B1").Value = "STYLENO"
        oSheet.Range("B1").ColumnWidth = 7
        oSheet.Range("C1").Value = "TAGNO"
        oSheet.Range("C1").ColumnWidth = 15
        oSheet.Range("D1").Value = "CERTNO"
        oSheet.Range("D1").ColumnWidth = 15
        oSheet.Range("E1").Value = "ITEM"
        oSheet.Range("E1").ColumnWidth = 11.57
        oSheet.Range("F1").Value = "SUBITEM"
        oSheet.Range("F1").ColumnWidth = 11.57
        oSheet.Range("G1").Value = "PURITY"
        oSheet.Range("G1").ColumnWidth = 11.57
        oSheet.Range("H1").Value = "PCS"
        oSheet.Range("H1").ColumnWidth = 6.6
        oSheet.Range("I1").Value = "GRSWT"
        oSheet.Range("I1").ColumnWidth = 6.6
        oSheet.Range("J1").Value = "SIZE"
        oSheet.Range("J1").ColumnWidth = 6.6
        oSheet.Range("K1").Value = "NETWT"
        oSheet.Range("K1").ColumnWidth = 6.6
        oSheet.Range("L1").Value = "WASTPER"
        oSheet.Range("L1").ColumnWidth = 11.57
        oSheet.Range("M1").Value = "RATE"
        oSheet.Range("M1").ColumnWidth = 6.6
        oSheet.Range("N1").Value = "MCGRM"
        oSheet.Range("N1").ColumnWidth = 6.6
        oSheet.Range("O1").Value = "AMOUNT"
        oSheet.Range("O1").ColumnWidth = 11.57
        oSheet.Range("P1").Value = "STNITEM"
        oSheet.Range("P1").ColumnWidth = 11.57
        oSheet.Range("Q1").Value = "STNPCS"
        oSheet.Range("Q1").ColumnWidth = 6.6
        oSheet.Range("R1").Value = "STNWT"
        oSheet.Range("R1").ColumnWidth = 6.6
        oSheet.Range("S1").Value = "STNAMT"
        oSheet.Range("S1").ColumnWidth = 6.6
        oSheet.Range("T1").Value = "SALEAMOUNT"
        oSheet.Range("T1").ColumnWidth = 11.57
        oSheet.Range("U1").Value = "DESIGNERNAME"
        oSheet.Range("U1").ColumnWidth = 11.57

        oSheet.Range("A1:G1:N1:V1").Font.Bold = True
        oSheet.Range("A1:G1:N1:V1").Font.Name = "VERDANA"
        oSheet.Range("A1:G1:N1:V1").Font.Size = 8
        ' oSheet.Range("C1:D1").Merge()
        'oSheet.Range("C1:D1").HorizontalAlignment = Excel.Constants.xlCenter
        'oSheet.Range("G1:H1:I1:J1:K1:L1:M1").Merge()
        'oSheet.Range("G1:H1:I1:J1:K1:L1:M1").HorizontalAlignment = Excel.Constants.xlCenter
        'oSheet.Range("N1:O1:P1:Q1:R1:S1:T1:U1").Merge()
        'oSheet.Range("N1:O1:P1:Q1:R1:S1:T1:U1").HorizontalAlignment = Excel.Constants.xlCenter
        'oSheet.Range("V1:W1:X1:Y1").Merge()
        'oSheet.Range("V1:W1:X1:Y1").HorizontalAlignment = Excel.Constants.xlCenter
        oSheet.Range("A2:B2:C2:D2:E2:F2:G2:H2:I2:J2:K2:L2:M2:N2:O2:P2:Q2:R2:S2:T2:U2:V2:W2:X2:Y2:Z2:AA2").Font.Bold = False
        oSheet.Range("A2:B2:C2:D2:E2:F2:G2:H2:I2:J2:K2:L2:M2:N2:O2:P2:Q2:R2:S2:T2:U2:V2:W2:X2:Y2:Z2:AA2").Font.Name = "VERDANA"
        oSheet.Range("A2:B2:C2:D2:E2:F2:G2:H2:I2:J2:K2:L2:M2:N2:O2:P2:Q2:R2:S2:T2:U2:V2:W2:X2:Y2:Z2:AA2").Font.Size = 8

    End Function

    Private Sub btntemplate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btntemplate.Click
        exceltemplate()
    End Sub

End Class