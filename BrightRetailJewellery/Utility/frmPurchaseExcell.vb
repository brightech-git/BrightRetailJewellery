Imports System.Data.OleDb
Imports System.Math
Public Class frmPurchaseExcell
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



    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
        objGPack.TextClear(Me)
        Initializer()
        ListAllowEmptyValues.Add("TAGNO")
        ListAllowEmptyValues.Add("ITEMNAME")
        ListAllowEmptyValues.Add("SUBNAME")
        ListAllowEmptyValues.Add("WT")
        ListAllowEmptyValues.Add("QTY")
        ListAllowEmptyValues.Add("RETAILPRICE")
        ListAllowEmptyValues.Add("DISCOUNT")
        ListAllowEmptyValues.Add("PURVALUE")
        ListAllowEmptyValues.Add("ITEMCTRNAME")

    End Sub

    Private Sub Initializer()
        CASHID = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'CASH'", , "CASH")
        BANKID = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'BANK'", , "BANK")
        With DtTran
            .Columns.Add("TAGNO", GetType(String))
            .Columns.Add("ITEMNAME", GetType(String))
            .Columns.Add("SUBNAME", GetType(String))
            .Columns.Add("WT", GetType(Decimal))
            .Columns.Add("QTY", GetType(Decimal))
            .Columns.Add("RETAILPRICE", GetType(Decimal))
            .Columns.Add("DISCOUNT", GetType(Decimal))
            .Columns.Add("PURVALUE", GetType(Decimal))
            .Columns.Add("ITEMCTRNAME", GetType(String))
            .Columns.Add("COLHEAD", GetType(String))
            .Columns.Add("KEYNO", GetType(Integer))
            .Columns.Add("REMARKS", GetType(String))
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
        'DtTranTotal.Rows(0).Item("DESCRIPTION") = "TOTAL"
        DgvTranTotal.DataSource = DtTranTotal
        GridStyle(DgvTranTotal)
        DgvTranTotal.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)

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
            .Columns("TAGNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("TAGNO").Width = 100
            .Columns("ITEMNAME").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns("ITEMNAME").Width = 100
            .Columns("SUBNAME").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns("SUBNAME").Width = 100
            .Columns("WT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("WT").Width = 100
            .Columns("QTY").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("QTY").Width = 100
            .Columns("RETAILPRICE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("RETAILPRICE").Width = 100
            .Columns("DISCOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("DISCOUNT").Width = 100
            .Columns("PURVALUE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("PURVALUE").Width = 100
            .Columns("ITEMCTRNAME").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns("ITEMCTRNAME").Width = 100
            .Columns("KEYNO").Visible = False
            .Columns("COLHEAD").Visible = False
            .Columns("REMARKS").Visible = False

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

            .Columns("TAGNO").Visible = True
            .Columns("ITEMNAME").Visible = True
            .Columns("SUBNAME").Visible = True
            .Columns("WT").Visible = True
            .Columns("QTY").Visible = True
            ' .Columns("RETAILVALUE").Visible = True
            .Columns("DISCOUNT").Visible = True
            .Columns("PURVALUE").Visible = True
            .Columns("ITEMCTRNAME").Visible = True
            .Columns("REMARKS").Visible = False
            .Columns("KEYNO").Visible = False
            .Columns("COLHEAD").Visible = False

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
                                .Cells("TAGNO").Style.BackColor = Color.Red
                            End If
                            If Remark(i) = "IM" Then
                                .Cells("ITEMNAME").Style.BackColor = Color.Red
                            End If
                            If Remark(i) = "SM" Then
                                .Cells("SUBNAME").Style.BackColor = Color.Red
                            End If

                            If Remark(i) = "CN" Then
                                .Cells("ITEMCTRNAME").Style.BackColor = Color.Red
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
        'StrSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'CALCMODE'"
        'CalMode = objGPack.GetSqlValue(StrSql, "CTLTEXT", "G", tran)
        'If CalMode = "N" Then
        '    grsnet = "N"
        'Else
        '    grsnet = "G"
        'End If
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
        row = dtt.Select(" COLHEAD='G' AND QTY=0")
        If row.Length > 0 Then
            Return False
        End If
        Return True
    End Function
    Function CheckRateSave()
        Dim dtt As New DataTable
        Dim row() As DataRow = Nothing
        dtt = DgvTran.DataSource
        row = dtt.Select("PURVALUE=0")
        If row.Length > 1 Then
            Return False
        End If
        Return True
    End Function
    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
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

    Private Sub Save(ByVal index As Integer)
        With DgvTran.Rows(index)
            'Dim Obj As MaterialIssRec
            'Obj = CType(.Cells("METISSREC").Value, MaterialIssRec)
            If .Cells("COLHEAD").Value.ToString = "G" Then
                Dim OrdStateId As Integer = 0
                Dim Tax As Decimal = 0
                Dim Tds As Decimal = 0
                Dim Type As String = "O" ' wheather it is ornament,metal,stone,others
                Dim issSno As String = Nothing
                issSno = GetNewSno(TranSnoType.RECEIPTCODE, tran)
                Dim catCode As String = objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Cells("ITEMNAME").Value.ToString & "'", , , tran)
                Dim OCatcode As String
                OCatcode = objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Cells("ITEMNAME").Value.ToString & "'", , , tran)
                Dim alloy As Decimal = Nothing


                Dim itemTypeId As Integer = 0
                'itemTypeId = Val(objGPack.GetSqlValue("SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & .Cells("METALTYPE").Value.ToString & "'", , 0, tran))
                Dim Itemid As Integer = Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Cells("ITEMNAME").Value.ToString & "'", , 0, tran))
                Dim subItemid As Integer = Val(objGPack.GetSqlValue("SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & .Cells("SUBNAME").Value.ToString & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Cells("ITEMNAME").Value.ToString & "')", , 0, tran))
                Dim itemctrid As Integer = Val(objGPack.GetSqlValue("SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & .Cells("ITEMCTRNAME").Value.ToString & "'", , 0, tran))
                If OCatcode.ToString = "" Then OCatcode = catCode
                Dim PurTax As Decimal = 0
                If Val(txtTaxper_PER.Text) <> 0 Then
                    PurTax = (Val(.Cells("RETAILPRICE").Value.ToString) * Val(txtTaxper_PER.Text)) / 100
                End If
                Tax = PurTax
                If ChkPurchase.Checked Then
                    StrSql = " INSERT INTO " & cnStockDb & "..RECEIPT"
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
                    StrSql += " ,REMARK2,USERID,UPDATED,UPTIME,SYSTEMID,DISCOUNT,RUNNO,CASHID,VATEXM,TAX,TDS"
                    StrSql += " ,STNAMT,MISCAMT,METALID,STONEUNIT,APPVER,TOUCH,ORDSTATE_ID"
                    StrSql += " ,SEIVE,BAGNO)"
                    StrSql += " VALUES("
                    StrSql += " '" & issSno & "'" ''SNO
                    StrSql += " ," & TranNo & "" 'TRANNO
                    StrSql += " ,'" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                    StrSql += " ,'RPU'" 'TRANTYPE
                    StrSql += " ," & Val(.Cells("QTY").Value.ToString) & "" 'PCS
                    StrSql += " ," & Val(.Cells("WT").Value.ToString) & "" 'GRSWT
                    StrSql += " ," & Val(.Cells("WT").Value.ToString) & "" 'NETWT
                    StrSql += " ,0" '& Val(.Cells("WT").Value.ToString) - Val(.Cells("NETWT").Value.ToString) & "" 'LESSWT
                    StrSql += " ," & 0 & "" 'LESSWT
                    StrSql += " ,'" & .Cells("TAGNO").Value.ToString & "'" 'TAGNO
                    StrSql += " ," & Itemid & "" 'ITEMID
                    StrSql += " ," & subItemid & "" 'SUBITEMID
                    StrSql += " ," & 0 & "" 'WASTPER
                    StrSql += " ,0" ' & Val(.Cells("WASTAGE").Value.ToString) & "" 'WASTAGE
                    StrSql += " ," & 0 & "" 'MCGRM
                    StrSql += " ,0" '& Val(.Cells("MC").Value.ToString) & "" 'MCHARGE
                    StrSql += " ," & Val(.Cells("RETAILPRICE").Value.ToString) & "" 'AMOUNT
                    StrSql += " ," & 0 & "" 'RATE
                    StrSql += " ," & Val(.Cells("PURVALUE").Value.ToString) & "" 'BOARDRATE
                    StrSql += " ,''" 'SALEMODE
                    StrSql += " ,'" & grsnet & "'" 'GRSNET
                    StrSql += " ,''" 'TRANSTATUS ''
                    StrSql += " ,''" '& txtBillNo.Text & "'" 'REFNO ''
                    StrSql += " ,'" & dtpBillDate_OWN.Value.ToString("yyyy-MM-dd") & "'" 'REFDATE NULL
                    StrSql += " ,'" & CostCenterId & "'" 'COSTID 
                    StrSql += " ,'" & strCompanyId & "'" 'COMPANYID
                    StrSql += " ,0" 'FLAG
                    StrSql += " ,0" 'EMPID
                    StrSql += " ,0" 'TAGGRSWT
                    StrSql += " ,0" 'TAGNETWT
                    StrSql += " ,0" 'TAGRATEID
                    StrSql += " ,0" 'TAGSVALUE
                    StrSql += " ,''" 'TAGDESIGNER  
                    StrSql += " ,0" '& itemCtrId & "" 'itemctrid
                    StrSql += " ," & itemTypeId & "" 'ITEMTYPEID
                    StrSql += " ,0" 'PURITY
                    StrSql += " ,''" 'TABLECODE
                    StrSql += " ,''" 'INCENTIVE
                    StrSql += " ,''" 'WEIGHTUNIT
                    StrSql += " ,'" & catCode & "'" 'CATCODE
                    StrSql += " ,'" & OCatcode & "'" 'OCATCODE
                    StrSql += " ,'" & _Accode & "'" 'ACCODE
                    StrSql += " ,0" 'ALLOY
                    StrSql += " ,'" & BatchNo & "'" 'BATCHNO
                    StrSql += " ,''" 'REMARK1
                    StrSql += " ,''" 'REMARK2
                    StrSql += " ,'" & userId & "'" 'USERID
                    StrSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                    StrSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                    StrSql += " ,'" & systemId & "'" 'SYSTEMID
                    StrSql += " ,'" & Val(.Cells("DISCOUNT").Value.ToString) & "'" 'DISCOUNT
                    StrSql += " ,''" 'RUNNO
                    StrSql += " ,''" 'CASHID
                    StrSql += " ,''" 'VATEXM
                    StrSql += " ,0" & Tax & "" 'TAX
                    StrSql += " ,0" & Tds & "" 'TDS
                    StrSql += " ,0" '& Val(.Cells("STNAMT").Value.ToString) & "" 'STNAMT"
                    StrSql += " ,0" 'MISCAMT
                    StrSql += " ,0" '& objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Cells("ITEM").Value.ToString & "'", , , tran) & "'" 'METALID
                    StrSql += " ,''" 'STONEUNIT
                    StrSql += " ,'" & VERSION & "'" 'APPVER
                    StrSql += " ,0" 'TOUCH
                    StrSql += " ," & OrdStateId & "" 'ORDSTATE_ID
                    StrSql += " ,''" 'SEIVE
                    StrSql += ",'" & IIf(Transistno <> 0, Transistno.ToString, "") & "'"
                    StrSql += " )"
                    ExecQuery(SyncMode.Transaction, StrSql, cn, tran, CostCenterId)
                End If

                Dim calType, mlwmctype As String
                'Dim itemCtrId As Integer
                Dim Lasttagno As Integer
                Dim tagPrefix, tagno, TagSno, tagVal As String
                'Dim SHAPEid, Colorid, Clarityid, Settypeid As Integer

                StrSql = " SELECT ITEMID,METALID,SIZESTOCK,OTHCHARGE,CALTYPE,VALUEADDEDTYPE,DEFAULTCOUNTER"
                StrSql += " ,STUDDED,STOCKTYPE,TABLECODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Cells("ITEMNAME").Value.ToString & "'"
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

                StrSql = " SELECT TOP 1 DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME='" & cmbAcName.Text & "')"
                designerid = Val(objGPack.GetSqlValue(StrSql, , "", tran))

       
                tagno = tagPrefix & Getidwithlen(Lasttagno.ToString, 3)

                tagVal = objtag.GetTagVal(tagno)

                TagSno = GetNewSno(TranSnoType.ITEMTAGCODE, tran, "GET_ADMINSNO_TRAN")

                tagnos = tagnos + "," + tagno

                ''INSERTING ITEMTAG
                StrSql = " INSERT INTO " & cnAdminDb & "..ITEMTAG"
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
                StrSql += " RECSNO,FROMITEMID,SHAPEID,COLORID,CLARITYID,SETTYPEID,HEIGHT,WIDTH) VALUES("

                StrSql += " '" & TagSno & "'" 'SNO
                StrSql += " ,'" & GetEntryDate(dtpTrandate.Value, tran).ToString("yyyy-MM-dd") & "'" 'RECDATE 'dtpRecieptDate.Value.Date.ToString("yyyy-MM-dd")
                StrSql += " ,'" & cnCostId & "'" 'COSTID
                StrSql += " ," & Itemid & "" 'ITEMID
                StrSql += " ,''" 'ORDREPNO
                StrSql += " ,''" 'ORsno
                StrSql += " ,''" 'ORDSALMANCODE
                StrSql += " ," & subItemid & "" 'SUBITEMID
                StrSql += " ,''" 'SIZEID
                StrSql += " ," & itemCtrId & "" 'ITEMCTRID
                'StrSql += " ,'" & .Cells("TABLE").Value.ToString & "'"
                StrSql += " ,''"
                StrSql += " ," & designerid & "" 'DESIGNERID
                StrSql += " ,'" & .Cells("TAGNO").Value.ToString & "'" 'TAGNO
                StrSql += " ," & Val(.Cells("QTY").Value.ToString) & "" 'PCS
                StrSql += " ," & Val(.Cells("WT").Value.ToString) & "" 'GRSWT
                StrSql += " ,0" ' & Val(.Cells("GRSWT").Value.ToString) - Val(.Cells("NETWT").Value.ToString) & "" 'LESSWT
                StrSql += " ," & Val(.Cells("WT").Value.ToString) & "" 'NETWT
                StrSql += " ," & 0 & "" 'RATE
                StrSql += ",0" 'FINERATE
                StrSql += " ,0" '& Val(.Cells("MAXWASTPER").Value.ToString) & "" 'MAXWASTPER
                StrSql += " ,0" '& Val(.Cells("MAXMCGRM").Value.ToString) & "" 'MAXMCGRM
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
                StrSql += " ," & Val(.Cells("RETAILPRICE").Value.ToString) & "" 'SALVALUE
                StrSql += " ,0" 'PURITY
                StrSql += " ,''" 'NARRATION
                If .Cells("SUBNAME").Value.ToString.Trim <> "" Then 'DESCRIP
                    StrSql += " ,'" & .Cells("SUBNAME").Value.ToString & "'"
                Else
                    StrSql += " ,'" & .Cells("ITEMNAME").Value.ToString & "'"
                End If
                StrSql += " ,''" 'REASON
                StrSql += " ,'M'"
                StrSql += " ,'" & grsnet & "'" 'GRSNET
                StrSql += " ,NULL" 'ISSDATE
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
                StrSql += " ,''" '& Val(itemTypeId) & "" 'ITEMTYPEID
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
                StrSql += " ,''" 'STYLENO
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
                StrSql += " ,'" & issSno & "'" 'RECSNO
                StrSql += " ,0" 'FROMITEMID
                StrSql += " ,0" '& SHAPEid & "" 'SHAPEID
                StrSql += " ,0" '& Colorid & "" 'COLORID
                StrSql += " ,0" '& Clarityid & "" 'CLARITYID
                StrSql += " ,0" '& Settypeid & "" 'SETTYPEID
                StrSql += " ,0" '& Val(.Cells("HEIGHT").Value.ToString) & "" 'HEIGHT
                StrSql += " ,0" '& Val(.Cells("WIDTH").Value.ToString) & "" 'WIDTH
                StrSql += " )"
                Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.ExecuteNonQuery()
               
                ''ITEM PUR DETAIL
                StrSql = vbCrLf + " INSERT INTO " & cnAdminDb & "..PURITEMTAG"
                StrSql += vbCrLf + " ("
                StrSql += vbCrLf + " TAGSNO,ITEMID,TAGNO,PURLESSWT,PURNETWT,PURRATE"
                StrSql += vbCrLf + " ,PURGRSNET,PURWASTAGE,PURTOUCH,PURMC"
                StrSql += vbCrLf + " ,PURVALUE,PURTAX,RECDATE,COMPANYID,COSTID"
                StrSql += vbCrLf + " )"
                StrSql += vbCrLf + " VALUES"
                StrSql += vbCrLf + " ("
                StrSql += vbCrLf + " '" & TagSno & "'" 'TAGSNO
                StrSql += vbCrLf + " ," & Itemid & "" 'ITEMID
                StrSql += vbCrLf + " ,'" & .Cells("TAGNO").Value.ToString & "'" 'TAGNO
                StrSql += vbCrLf + " ,0" ' & Val(.Cells("GRSWT").Value.ToString) - Val(.Cells("NETWT").Value.ToString) & "" ' PURLESSWT
                StrSql += vbCrLf + " ," & Val(.Cells("WT").Value.ToString) & "" ' PURNETWT"
                StrSql += vbCrLf + " ," & Val(.Cells("PURVALUE").Value.ToString) & "" ' PURRATE"
                StrSql += vbCrLf + " ,'" & grsnet & "'" ' PURGRSNET"
                StrSql += vbCrLf + " ,0" '& Val(.Cells("WASTAGE").Value.ToString) & "" ' PURWASTAGE"
                StrSql += vbCrLf + " ," & 0 & "" ' PURTOUCH"
                StrSql += vbCrLf + " ,0" '& Val(.Cells("MC").Value.ToString) & "" ' PURMC"
                StrSql += vbCrLf + " ," & Val(.Cells("PURVALUE").Value.ToString) & "" ' PURVALUE"
                StrSql += vbCrLf + " ,0" '& PurTax & ""
                StrSql += vbCrLf + " ,'" & dtpBillDate_OWN.Value.ToString("yyyy-MM-dd") & "'"
                StrSql += vbCrLf + " ,'" & strCompanyId & "'"
                StrSql += vbCrLf + " ,'" & cnCostId & "'"
                StrSql += vbCrLf + " )"
                Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.ExecuteNonQuery()


                Dim rowTag As DataRow = Nothing
                rowTag = dtTAGPrint.NewRow
                rowTag!ITEMID = Itemid
                rowTag!TAGNO = tagno
                dtTAGPrint.Rows.Add(rowTag)

                'StrSql = " UPDATE " & cnAdminDb & "..ITEMMAST SET CURRENTTAGNO='" & ttagno & "' WHERE ITEMID = "
                'StrSql += " (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Cells("ITEM").Value.ToString & "')"
                'Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.ExecuteNonQuery()

                Dim drstone() As DataRow = DtTran.Select("TAGNO='" & .Cells("TAGNO").Value.ToString & "' AND COLHEAD <>'G'", "")
                For Each stRow As DataRow In drstone
                    InsertStoneDetails(issSno, TranNo, stRow, TagSno, tagno, Itemid, Tax)
                Next
            End If
        End With
    End Sub


    Private Sub InsertStoneDetails(ByVal IssSno As String _
 , ByVal TNO As Integer, ByVal stRow As DataRow _
, ByVal Tagsno As String, ByVal Tagno As String, ByVal itemId As Integer _
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
        StrSql += " FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & stRow.Item("ITEM").ToString & "'"
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


        If taxx <> 0 Then
            StrSql = " SELECT PTAX "
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
        If ChkPurchase.Checked Then
            StrSql = " INSERT INTO " & cnStockDb & "..RECEIPTSTONE"
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
            StrSql += " ,'RPU'" 'TRANTYPE
            StrSql += " ," & Val(stRow.Item("PCS").ToString) & "" 'STNPCS
            StrSql += " ," & Val(stRow.Item("GRSWT").ToString) & "" 'STNWT
            StrSql += " ," & Val(stRow.Item("RATE").ToString) & "" 'STNRATE
            StrSql += " ," & Val(stRow.Item("STNAMT").ToString) & "" 'STNAMT
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

        Dim stnSno As String = GetNewSno(TranSnoType.ITEMTAGSTONECODE, tran, "GET_ADMINSNO_TRAN")
        Dim SHAPEid, Colorid, Clarityid, Settypeid As Integer
        SHAPEid = Val(objGPack.GetSqlValue("SELECT SHAPEID FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPENAME='" & stRow.Item("SHAPE").ToString & "' ", , 0, tran).ToString)
        Colorid = Val(objGPack.GetSqlValue("SELECT COLORID FROM " & cnAdminDb & "..STNCOLOR WHERE COLORNAME='" & stRow.Item("COLOR").ToString & "' ", , 0, tran).ToString)
        Clarityid = Val(objGPack.GetSqlValue("SELECT CLARITYID FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYNAME='" & stRow.Item("CLARITY").ToString & "' ", , 0, tran).ToString)
        Settypeid = Val(objGPack.GetSqlValue("SELECT SETTYPEID FROM " & cnAdminDb & "..STNSETTYPE WHERE SETTYPENAME='" & stRow.Item("SETTINGTYPE").ToString & "' ", , 0, tran).ToString)

        'INSERT ITEMTAGSTONE
        StrSql = " INSERT INTO " & cnAdminDb & "..ITEMTAGSTONE("
        StrSql += " SNO,TAGSNO,ITEMID,COMPANYID,STNITEMID,"
        StrSql += " STNSUBITEMID,TAGNO,STNPCS,STNWT,"
        StrSql += " STNRATE,STNAMT,DESCRIP,"
        StrSql += " RECDATE,CALCMODE,"
        StrSql += " MINRATE,SIZECODE,STONEUNIT,ISSDATE,"
        StrSql += " OLDTAGNO,VATEXM,CARRYFLAG,COSTID,SYSTEMID,APPVER,"
        StrSql += " USRATE,INDRS,PACKETNO,SHAPEID,COLORID,CLARITYID,SETTYPEID,HEIGHT,WIDTH"
        StrSql += " )VALUES("
        StrSql += " '" & stnSno & "'" ''SNO
        StrSql += " ,'" & Tagsno & "'" 'TAGSNO
        StrSql += " ,'" & itemId & "'" 'ITEMID
        StrSql += " ,'" & strCompanyId & "'" 'COMPANYID
        StrSql += " ," & stnItemId & "" 'STNITEMID
        StrSql += " ," & stnSubItemid & "" 'STNSUBITEMID
        StrSql += " ,'" & Tagno & "'" 'TAGNO
        StrSql += " ," & Val(stRow.Item("PCS").ToString) & "" 'STNPCS
        StrSql += " ," & Val(stRow.Item("GRSWT").ToString) & "" 'STNWT
        StrSql += " ," & Val(stRow.Item("SRATE").ToString) & "" 'STNRATE
        StrSql += " ," & Val(stRow.Item("SSTNAMT").ToString) & "" 'STNAMT
        If stnSubItemid <> 0 Then 'DESCRIP
            StrSql += " ,'" & stRow.Item("SUBITEM").ToString & "'"
        Else
            StrSql += " ,'" & stRow.Item("ITEM").ToString & "'"
        End If
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
        StrSql += " ," & Colorid & "" 'COLORID
        StrSql += " ," & Clarityid & "" 'CLARITYID
        StrSql += " ," & Settypeid & "" 'SETTYPEID
        StrSql += " ," & Val(stRow.Item("HEIGHT").ToString) & "" 'HEIGHT
        StrSql += " ," & Val(stRow.Item("WIDTH").ToString) & "" 'WIDTH
        StrSql += " )"
        Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.ExecuteNonQuery()

        'INSERT PURCHASE DETAIL
        StrSql = vbCrLf + " INSERT INTO " & cnAdminDb & "..PURITEMTAGSTONE"
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
        StrSql += " ," & stnItemId & "" 'STNITEMID
        StrSql += " ," & stnSubItemid & "" 'STNSUBITEMID
        StrSql += " ," & Val(stRow.Item("PCS").ToString) & "" 'STNPCS
        StrSql += " ," & Val(stRow.Item("GRSWT").ToString) & "" 'STNWT
        StrSql += " ," & Val(stRow.Item("RATE").ToString) & "" 'STNRATE
        StrSql += " ," & Val(stRow.Item("STNAMT").ToString) & "" 'STNAMT
        StrSql += " ,'" & stoneunit & "'" 'STONEUNIT
        StrSql += " ,'" & calType & "'" 'CALCMODE
        StrSql += vbCrLf + " ," & Val(stRow.Item("RATE").ToString) & "" 'PURRATE
        StrSql += vbCrLf + " ," & Val(stRow.Item("STNAMT").ToString) & "" 'PURAMT
        StrSql += vbCrLf + " ,'" & GetStockCompId() & "'"
        StrSql += vbCrLf + " ,'" & cnCostId & "'"
        StrSql += vbCrLf + " ,'" & stnSno & "'"
        StrSql += vbCrLf + " )"
        Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.ExecuteNonQuery()
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
            If Dt.Rows.Count > 0 Then
                Dim Tagno As String = ""
                Dim v As Integer = 1
                For i As Integer = 0 To Dt.Rows.Count - 1
                    If Dt.Rows(i).Item(0).ToString <> "" Then
                        Tagno = Dt.Rows(i).Item(0).ToString

                    End If
                    If Val(Dt.Rows(i).Item(4).ToString) <> 0 Or Val(Dt.Rows(i).Item(5).ToString) <> 0 Or Val(Dt.Rows(i).Item(6).ToString) <> 0 Then
                        StrSql = " SELECT 1 FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO='" & Dt.Rows(i).Item(0).ToString.Trim & "'"
                        If objGPack.GetSqlValue(StrSql, , 0) = 1 Then
                            MsgBox(Dt.Rows(i).Item(0).ToString & " Tagno Already Exists ", MsgBoxStyle.Information)

                        End If
                    End If

                    If Val(Dt.Rows(i).Item(5).ToString) <> 0 Or Val(Dt.Rows(i).Item(6).ToString) <> 0 Or Val(Dt.Rows(i).Item(6).ToString) <> 0 Then
                        StrSql = " SELECT 1 FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & Dt.Rows(i).Item(1).ToString.Trim & "'"
                        If objGPack.GetSqlValue(StrSql, , 0) <> 1 Then
                            MsgBox(Dt.Rows(i).Item(1).ToString & " Not found in ITEMMAST ", MsgBoxStyle.Information)
                        End If
                        If Val(Dt.Rows(i).Item(5).ToString) <> 0 Or Val(Dt.Rows(i).Item(4).ToString) <> 0 Or Val(Dt.Rows(i).Item(6).ToString) <> 0 Then
                            StrSql = " SELECT 1 FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME='" & Dt.Rows(i).Item(2).ToString.Trim & "'"
                            If objGPack.GetSqlValue(StrSql, , 0) <> 1 Then
                                MsgBox(Dt.Rows(i).Item(2).ToString & " Not found in SUBITEMMAST ", MsgBoxStyle.Information)
                            End If
                            If Val(Dt.Rows(i).Item(5).ToString) <> 0 Or Val(Dt.Rows(i).Item(4).ToString) <> 0 Or Val(Dt.Rows(i).Item(6).ToString) <> 0 Then
                                StrSql = " SELECT 1 FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME='" & Dt.Rows(i).Item(8).ToString.Trim & "'"
                                If objGPack.GetSqlValue(StrSql, , 0) <> 1 Then
                                    MsgBox(Dt.Rows(i).Item(8).ToString & " Not found in ItemCounter ", MsgBoxStyle.Information)
                                End If

                                Dim dtrow As DataRow = Nothing
                                dtrow = DtTran.NewRow
                                If CheckExists("ITEMTAG", "TAGNO", Dt.Rows(i).Item(0).ToString.Trim) = True Then
                                    dtrow("REMARKS") = "TN,"
                                End If
                                If CheckExists("ITEMMAST", "ITEMNAME", Dt.Rows(i).Item(1).ToString.Trim) = False Then
                                    dtrow("REMARKS") = dtrow("REMARKS") & "IM,"
                                End If
                                If CheckExists("SUBITEMMAST", "SUBITEMNAME", Dt.Rows(i).Item(2).ToString.Trim) = False Then
                                    dtrow("REMARKS") = dtrow("REMARKS") & "SM,"
                                End If
                                If CheckExists("ITEMCOUNTER", "ITEMCTRNAME", Dt.Rows(i).Item(8).ToString.Trim) = False Then
                                    dtrow("REMARKS") = dtrow("REMARKS") & "CN,"
                                End If
                                dtrow("TAGNO") = Tagno
                                dtrow("TAGNO") = Dt.Rows(i).Item(0).ToString.ToUpper
                                dtrow("ITEMNAME") = Dt.Rows(i).Item(1).ToString.ToUpper
                                dtrow("SUBNAME") = Dt.Rows(i).Item(2).ToString.Trim.ToUpper
                                dtrow("WT") = Dt.Rows(i).Item(3).ToString.Trim.ToUpper
                                dtrow("QTY") = Val(Dt.Rows(i).Item(4).ToString)
                                dtrow("RETAILPRICE") = Format(Val(Dt.Rows(i).Item(5).ToString.Replace(",", "")), "0.00")
                                dtrow("DISCOUNT") = Format(Val(Dt.Rows(i).Item(6).ToString.Replace(",", "")), "0.00")
                                dtrow("PURVALUE") = Format(Val(Dt.Rows(i).Item(7).ToString.Replace(",", "")), "0.00")
                                dtrow("ITEMCTRNAME") = Dt.Rows(i).Item(8).ToString.Trim.ToUpper
                                dtrow("COLHEAD") = "G"
                                DtTran.Rows.Add(dtrow)
                                DtTran.AcceptChanges()
                                v += 1
                            End If
                        End If
                    End If
                Next
                FormatedGridStyle(DgvTran)
            End If

            If Itemname <> "" Then MsgBox("This item Not Found: " & Mid(Itemname, 2, Len(Itemname)) & "", MsgBoxStyle.Information)
        Catch ex As Exception
            DtTran.Rows.Clear()
            DtTran.AcceptChanges()
            MsgBox("Invalid File Format", MsgBoxStyle.Information)
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

        'oSheet.Range("A1").Value = "ITEM"
        'oSheet.Range("B1").Value = "GOLD"
        'oSheet.Range("M1").Value = "DIAMOND"
        'oSheet.Range("U1").Value = "STONE"

        oSheet.Range("A1").Value = "TAGNO"
        oSheet.Range("A1").ColumnWidth = 5
        oSheet.Range("B1").Value = "ITEMNAME"
        oSheet.Range("B1").ColumnWidth = 7
        oSheet.Range("C1").Value = "SUBNAME"
        oSheet.Range("C1").ColumnWidth = 15
        oSheet.Range("D1").Value = "WT"
        oSheet.Range("D1").ColumnWidth = 15
        oSheet.Range("E1").Value = "QTY"
        oSheet.Range("E1").ColumnWidth = 11.57
        oSheet.Range("F1").Value = "RETAILPRICE"
        oSheet.Range("F1").ColumnWidth = 11.57
        oSheet.Range("G1").Value = "DISCOUNT"
        oSheet.Range("G1").ColumnWidth = 3.6
        oSheet.Range("H1").Value = "PURVALUE"
        oSheet.Range("H1").ColumnWidth = 6.6
        oSheet.Range("I1").Value = "ITEMCTRNAME"
        oSheet.Range("I1").ColumnWidth = 6.6

        'oSheet.Range("B2").Value = "ITEMNAME"
        'oSheet.Range("B2").ColumnWidth = 6.6
        'oSheet.Range("C2").Value = "WASTAGE"
        'oSheet.Range("C2").ColumnWidth = 9
        'oSheet.Range("D2").Value = "MC"
        'oSheet.Range("D2").ColumnWidth = 5.71
        'oSheet.Range("E2").Value = "RATE"
        'oSheet.Range("E2").ColumnWidth = 7.86

        'oSheet.Range("F2").Value = "GVALUE"
        'oSheet.Range("F2").ColumnWidth = 7.86
        'oSheet.Range("G2").Value = "SHAPE"
        'oSheet.Range("G2").ColumnWidth = 8.71
        'oSheet.Range("H2").Value = "SETTINGTYPE"
        'oSheet.Range("H2").ColumnWidth = 10.43
        'oSheet.Range("P2").Value = "COLOR"
        'oSheet.Range("P2").ColumnWidth = 5.71
        'oSheet.Range("Q2").Value = "CLARITY"
        'oSheet.Range("Q2").ColumnWidth = 7.29
        'oSheet.Range("R2").Value = "PCS"
        'oSheet.Range("R2").ColumnWidth = 2.91
        'oSheet.Range("S2").Value = "WT"
        'oSheet.Range("S2").ColumnWidth = 5.91
        'oSheet.Range("T2").Value = "RATE"
        'oSheet.Range("T2").ColumnWidth = 6.01
        'oSheet.Range("U2").Value = "VALUE"
        'oSheet.Range("U2").ColumnWidth = 7.86
        'oSheet.Range("V2").Value = "PCS"
        'oSheet.Range("V2").ColumnWidth = 2.91
        'oSheet.Range("W2").Value = "WT"
        'oSheet.Range("W2").ColumnWidth = 5.86
        'oSheet.Range("X2").Value = "RATE"
        'oSheet.Range("X2").ColumnWidth = 6.01
        'oSheet.Range("Y2").Value = "VALUE"
        'oSheet.Range("Y2").ColumnWidth = 7.86
        'oSheet.Range("Z2").Value = "TOTAL"
        'oSheet.Range("Z2").ColumnWidth = 7.86
        'oSheet.Range("AA2").Value = "METALTYPE"
        'oSheet.Range("AA2").ColumnWidth = 14

        oSheet.Range("C1:G1:N1:V1").Font.Bold = True
        oSheet.Range("C1:G1:N1:V1").Font.Name = "VERDANA"
        oSheet.Range("C1:G1:N1:V1").Font.Size = 8
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