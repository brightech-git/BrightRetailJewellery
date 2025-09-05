Imports System.Data.OleDb
Imports System.Math
Public Class frmPurchaseExcel
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
    Dim TAGXLUPLOAD_MAN_AUTO As String = GetAdmindbSoftValue("TAGXLUPLOAD_MAN_AUTO", "A")
    Dim STNRATE_DBLPURRATE As Boolean = IIf(GetAdmindbSoftValue("STNRATE_DBLPURRATE", "N") = "Y", True, False)
    Dim CER_CHARGESID As Integer = Val(GetAdmindbSoftValue("CER_CHARGESID", "0"))
    Dim TAGXLUPLOAD_WASTMCCALC As String = GetAdmindbSoftValue("TAGXLUPLOAD_WASTMCCALC", "D")
    Dim SKUTAGNOS As String = ""
    Dim PURCHUPD_TAGASTAGKEY As Boolean = IIf(GetAdmindbSoftValue("PURCHUPD_TAGASTAGKEY", "N") = "Y", True, False)
    Dim PURCHUPD_SEPTAGNO As Boolean = IIf(GetAdmindbSoftValue("PURCHUPD_SEPTAGNO", "N") = "Y", True, False)
    Dim PURCHUPD_STYLENOASTAGKEY As Boolean = IIf(GetAdmindbSoftValue("PURCHUPD_STYLENOASTAGKEY", "N") = "Y", True, False)


    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
        objGPack.TextClear(Me)
        Initializer()
        ListAllowEmptyValues.Add("SUBITEMMAST")
        ListAllowEmptyValues.Add("STNSETTYPE")
        ListAllowEmptyValues.Add("STNSHAPE")
        ListAllowEmptyValues.Add("STNCOLOR")
        ListAllowEmptyValues.Add("STNCLARITY")
        ListAllowEmptyValues.Add("ITEMTYPE")
    End Sub

    Private Sub Initializer()
        CASHID = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'CASH'", , "CASH")
        BANKID = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'BANK'", , "BANK")
        With DtTran
            .Columns.Add("MARK", GetType(Boolean))
            .Columns.Add("SNO", GetType(String))
            .Columns.Add("STYLENO", GetType(String))
            .Columns.Add("ITEM", GetType(String))
            .Columns.Add("SUBITEM", GetType(String))
            .Columns.Add("HEIGHT", GetType(Decimal))
            .Columns.Add("WIDTH", GetType(Decimal))
            .Columns.Add("PCS", GetType(Integer))
            .Columns.Add("GRSWT", GetType(Decimal))
            .Columns.Add("NETWT", GetType(Decimal))
            .Columns.Add("RATE", GetType(Decimal))
            .Columns.Add("WASTAGE", GetType(Decimal))
            .Columns.Add("MC", GetType(Decimal))
            .Columns.Add("SHAPE", GetType(String))
            .Columns.Add("SETTINGTYPE", GetType(String))
            .Columns.Add("COLOR", GetType(String))
            .Columns.Add("CLARITY", GetType(String))
            .Columns.Add("AMOUNT", GetType(Decimal))
            .Columns.Add("STNAMT", GetType(Decimal))
            .Columns.Add("TOTAMT", GetType(Decimal))
            .Columns.Add("COLHEAD", GetType(String))
            .Columns.Add("MAXWASTPER", GetType(Decimal))
            .Columns.Add("MAXWASTAGE", GetType(Decimal))
            .Columns.Add("MAXMCGRM", GetType(Decimal))
            .Columns.Add("MAXMC", GetType(Decimal))
            .Columns.Add("SRATE", GetType(Decimal))
            .Columns.Add("GRSAMT", GetType(Decimal))
            .Columns.Add("SSTNAMT", GetType(Decimal))
            .Columns.Add("CERCHARGE", GetType(Decimal))
            .Columns.Add("SALVALUE", GetType(Decimal))
            .Columns.Add("METALTYPE", GetType(String))
            .Columns.Add("NARRATION", GetType(String))
            .Columns.Add("CATEGORY", GetType(String))
            .Columns.Add("MGRSWT", GetType(String))
            .Columns.Add("MWASTAGE", GetType(String))
            .Columns.Add("MMC", GetType(String))
            .Columns.Add("MRATE", GetType(String))
            .Columns.Add("MAMOUNT", GetType(String))
            .Columns.Add("TABLE", GetType(String))
            .Columns.Add("KEYNO", GetType(Integer))
            .Columns.Add("REMARKS", GetType(String))
            .Columns.Add("TOUCH", GetType(Double))
            .Columns.Add("TAGTYPE", GetType(String))
            .Columns.Add("TAGNO", GetType(String))
            .Columns.Add("SIZE", GetType(String))
            .Columns.Add("HM_WT", GetType(Decimal))
            .Columns.Add("HM_BILLNO", GetType(String))
            .Columns("KEYNO").AutoIncrement = True
        End With
        DgvTran.Columns.Clear()
        DgvTran.DataSource = DtTran
        ClearTran()
        'GridStyle(DgvTran)
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
            .Columns("HEIGHT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("WIDTH").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("SIZE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("RATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("WASTAGE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("MC").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("STNAMT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("TOTAMT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("MAXWASTPER").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("MAXWASTAGE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("MAXMC").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("MAXMCGRM").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("GRSAMT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("SSTNAMT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("SALVALUE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("SRATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("CERCHARGE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            .Columns("GRSWT").DefaultCellStyle.Format = "0.000"
            .Columns("NETWT").DefaultCellStyle.Format = "0.000"
            .Columns("RATE").DefaultCellStyle.Format = "0.00"
            .Columns("WASTAGE").DefaultCellStyle.Format = "0.000"
            .Columns("MC").DefaultCellStyle.Format = "0.00"
            .Columns("AMOUNT").DefaultCellStyle.Format = "0.00"
            .Columns("TAGNO").DefaultCellStyle.Format = ""
            .Columns("SIZE").DefaultCellStyle.Format = ""

            .Columns("WASTAGE").HeaderText = "WAST"
            .Columns("MAXWASTAGE").HeaderText = "WAST"
            .Columns("MAXWASTPER").HeaderText = "WAST%"
            .Columns("MAXMC").HeaderText = "MC"
            .Columns("MAXMCGRM").HeaderText = "MC/GRM"
            .Columns("SRATE").HeaderText = "RATE"
            .Columns("GRSAMT").HeaderText = "AMOUNT"
            .Columns("SSTNAMT").HeaderText = "STNAMT"

            .Columns("HEIGHT").Width = 60
            .Columns("WIDTH").Width = 60
            .Columns("SIZE").Width = 60

            .Columns("MARK").Width = 50
            .Columns("SNO").Width = 52
            .Columns("PCS").Width = 40
            .Columns("GRSWT").Width = 70
            .Columns("NETWT").Width = 70
            .Columns("RATE").Width = 80
            .Columns("WASTAGE").Width = 60
            .Columns("MAXWASTAGE").Width = 60
            .Columns("MAXWASTPER").Width = 60
            .Columns("MC").Width = 60
            .Columns("MAXMC").Width = 60
            .Columns("MAXMCGRM").Width = 70
            .Columns("SRATE").Width = 80
            .Columns("GRSAMT").Width = 80
            .Columns("AMOUNT").Width = 80
            .Columns("STNAMT").Width = 80
            .Columns("SSTNAMT").Width = 80
            .Columns("TOTAMT").Width = 80
            .Columns("SALVALUE").Width = 80
            .Columns("CERCHARGE").Width = 90

            .Columns("SHAPE").Width = 70
            .Columns("COLOR").Width = 60
            .Columns("CLARITY").Width = 70
            .Columns("STYLENO").Width = 80
            .Columns("TAGNO").Width = 80
            'For cnt As Integer = 12 To .ColumnCount - 1
            '    .Columns(cnt).Visible = False
            'Next

            .Columns("KEYNO").Visible = False
            .Columns("SNO").Visible = False
            .Columns("COLHEAD").Visible = False
            .Columns("TABLE").Visible = False
            .Columns("ITEM").ReadOnly = True
            .Columns("SUBITEM").ReadOnly = True
            .Columns("STYLENO").ReadOnly = True
            .Columns("REMARKS").Visible = False
            .Columns("MARK").ReadOnly = True
            If TAGXLUPLOAD_MAN_AUTO <> "J" Then
                .Columns("CATEGORY").Visible = False
                .Columns("MGRSWT").Visible = False
                .Columns("MWASTAGE").Visible = False
                .Columns("MMC").Visible = False
                .Columns("MRATE").Visible = False
                .Columns("MAMOUNT").Visible = False
            Else
                .Columns("MGRSWT").DefaultCellStyle.Format = "0.000"
                .Columns("MWASTAGE").DefaultCellStyle.Format = "0.000"
                .Columns("MMC").DefaultCellStyle.Format = "0.00"
                .Columns("MRATE").DefaultCellStyle.Format = "0.00"
                .Columns("MAMOUNT").DefaultCellStyle.Format = "0.00"
                .Columns("MGRSWT").Width = 70
                .Columns("MWASTAGE").Width = 80
                .Columns("MMC").Width = 80
                .Columns("MRATE").Width = 80
                .Columns("MAMOUNT").Width = 80
                .Columns("CATEGORY").Width = 100
                .Columns("MGRSWT").HeaderText = "GRSWT"
                .Columns("MMC").HeaderText = "MC"
                .Columns("MRATE").HeaderText = "RATE"
                .Columns("MWASTAGE").HeaderText = "WAST"
                .Columns("MAMOUNT").HeaderText = "AMOUNT"
                .Columns("MGRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("MWASTAGE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("MMC").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("MRATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("MAMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End If
        End With
    End Sub
    Private Sub GridStyleVisible(ByVal Dgv As DataGridView)
        With Dgv
            For cnt As Integer = 0 To .ColumnCount - 1
                .Columns(cnt).Visible = True
                .Columns(cnt).SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns(cnt).Resizable = DataGridViewTriState.False
            Next
            .Columns("NARRATION").Visible = False
            .Columns("WASTAGE").Visible = True
            .Columns("RATE").Visible = True
            .Columns("WASTAGE").Visible = True
            .Columns("MC").Visible = True
            .Columns("AMOUNT").Visible = True
            .Columns("STNAMT").Visible = True
            .Columns("TOTAMT").Visible = True


            If ChkSale.Checked Then
                .Columns("MAXWASTPER").Visible = True
                .Columns("MAXWASTAGE").Visible = True
                .Columns("MAXMCGRM").Visible = True
                .Columns("MAXMC").Visible = True
                .Columns("SRATE").Visible = True
                .Columns("GRSAMT").Visible = True
                .Columns("SSTNAMT").Visible = True
                .Columns("SALVALUE").Visible = True
                .Columns("CERCHARGE").Visible = True
            Else
                .Columns("MAXWASTPER").Visible = False
                .Columns("MAXWASTAGE").Visible = False
                .Columns("MAXMCGRM").Visible = False
                .Columns("MAXMC").Visible = False
                .Columns("SRATE").Visible = False
                .Columns("GRSAMT").Visible = False
                .Columns("SSTNAMT").Visible = False
                .Columns("SALVALUE").Visible = False
                .Columns("CERCHARGE").Visible = False
            End If

            .Columns("KEYNO").Visible = False
            .Columns("SNO").Visible = False
            .Columns("COLHEAD").Visible = False
            .Columns("TABLE").Visible = False
            .Columns("ITEM").ReadOnly = True
            .Columns("SUBITEM").ReadOnly = True
            .Columns("HEIGHT").ReadOnly = True
            .Columns("WIDTH").ReadOnly = True
            .Columns("STYLENO").ReadOnly = True
            .Columns("REMARKS").Visible = False
        End With
    End Sub
    Private Sub FormatedGridStyle(ByVal Dgv As DataGridView)
        Dim Remark() As String
        With Dgv
            .Columns("REMARKS").Visible = False
            For cnt As Integer = 0 To .RowCount - 1
                .Rows(cnt).Cells("MARK").ReadOnly = False
                If .Rows(cnt).Cells("COLHEAD").Value.ToString <> "G" Then
                    .Rows(cnt).DefaultCellStyle.BackColor = Color.LightYellow
                Else
                    Dim Itemid As Integer = Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Rows(cnt).Cells("ITEM").Value.ToString & "'", , 0, tran))
                    If Val(objGPack.GetSqlValue("SELECT COUNT(*) FROM " & cnAdminDb & "..WITEMTAG WHERE TAGNO='" & .Rows(cnt).Cells("STYLENO").Value.ToString & "' AND ITEMID='" & Itemid & "'", , "", tran)) Then
                        .Rows(cnt).Cells("MARK").ReadOnly = False
                        .Rows(cnt).DefaultCellStyle.BackColor = Color.LightGreen
                    End If
                End If
                Remark = Split(.Rows(cnt).Cells("REMARKS").Value.ToString, ",")
                If Remark.Length > 1 Then
                    For i As Integer = 0 To Remark.Length - 1
                        With .Rows(cnt)
                            If Remark(i) = "IM" Then
                                .Cells("ITEM").Style.BackColor = Color.Red
                            End If
                            If Remark(i) = "SI" Then
                                .Cells("SUBITEM").Style.BackColor = Color.Red
                            End If
                            If Remark(i) = "SH" Then
                                .Cells("SHAPE").Style.BackColor = Color.Red
                            End If
                            If Remark(i) = "SC" Then
                                .Cells("COLOR").Style.BackColor = Color.Red
                            End If
                            If Remark(i) = "SR" Then
                                .Cells("CLARITY").Style.BackColor = Color.Red
                            End If
                            If Remark(i) = "IT" Then
                                .Cells("METALTYPE").Style.BackColor = Color.Red
                            End If
                            If Remark(i) = "ST" Then
                                .Cells("SETTINGTYPE").Style.BackColor = Color.Red
                            End If
                            If Remark(i) = "HUID" Then
                                .DefaultCellStyle.BackColor = Color.Red
                                .Cells("MARK").ReadOnly = True
                                .Cells("MARK").Value = False
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
        If TAGXLUPLOAD_WASTMCCALC = "D" Then
            CmbValueAdded.Text = "DESIGNER"
        ElseIf TAGXLUPLOAD_WASTMCCALC = "I" Then
            CmbValueAdded.Text = "ITEM"
        ElseIf TAGXLUPLOAD_WASTMCCALC = "T" Then
            CmbValueAdded.Text = "TABLE"
        ElseIf TAGXLUPLOAD_WASTMCCALC = "P" Then
            CmbValueAdded.Text = "TAG"
        ElseIf TAGXLUPLOAD_WASTMCCALC = "E" Then
            CmbValueAdded.Text = "EXCEL"
        End If
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
            'Return False
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
        Dim billgencatcode As String = ""
        Dim billcontrolid As String = ""
        'If Not CheckRateSave() Then MsgBox("Rate Should Not Empty", MsgBoxStyle.Information) : Exit Sub
        If Not CheckMaster() Then MsgBox("Master Not Found", MsgBoxStyle.Information) : Exit Sub
        If Not CheckPiece() Then MsgBox("Piece Should Not be Empty", MsgBoxStyle.Information) : Exit Sub
        If CheckTrialDate(dtpTrandate.Value) = False Then Exit Sub
        _Accode = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbAcName.Text & "'")
        If _Accode = "" Then
            MsgBox("Invalid AcName", MsgBoxStyle.Information)
            cmbAcName.Select()
            Exit Sub
        End If
        For cnt As Integer = 0 To DgvTran.RowCount - 1
            If DgvTran.Rows(cnt).Cells(1).Value.ToString = "" Then
                MsgBox("Must Enter StyleNo(TagNo) In All Rows")
                Exit Sub
            End If
        Next
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
            If SKUTAGNOS <> "" Then GenerateSkuFile(cn, tran, "", SKUTAGNOS)
            SKUTAGNOS = ""
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
                Dim issSno As String = ""
                Dim catCode As String = objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Cells("ITEM").Value.ToString & "'", , , tran)
                Dim OCatcode As String
                OCatcode = objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Cells("ITEM").Value.ToString & "'", , , tran)
                Dim alloy As Decimal = Nothing

                Dim itemTypeId As Integer = 0
                itemTypeId = Val(objGPack.GetSqlValue("SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & .Cells("TAGTYPE").Value.ToString & "'", , 0, tran))
                Dim Itemid As Integer = Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Cells("ITEM").Value.ToString & "'", , 0, tran))
                Dim subItemid As Integer = Val(objGPack.GetSqlValue("SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & .Cells("SUBITEM").Value.ToString & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Cells("ITEM").Value.ToString & "')", , 0, tran))
                Dim SizeId As Integer = 0
                If .Cells("SIZE").Value.ToString <> "" Then
                    SizeId = Val(objGPack.GetSqlValue("SELECT SIZEID FROM " & cnAdminDb & "..ITEMSIZE WHERE SIZENAME = '" & .Cells("SIZE").Value.ToString & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Cells("ITEM").Value.ToString & "')", , 0, tran))
                End If

                If OCatcode Is Nothing Then OCatcode = catCode
                If OCatcode.ToString = "" Then OCatcode = catCode

                If ChkPurchase.Checked Then
                    issSno = GetNewSno(TranSnoType.RECEIPTCODE, tran)
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
                    StrSql += " ," & Val(.Cells("PCS").Value.ToString) & "" 'PCS
                    StrSql += " ," & Val(.Cells("GRSWT").Value.ToString) & "" 'GRSWT
                    StrSql += " ," & Val(.Cells("NETWT").Value.ToString) & "" 'NETWT
                    StrSql += " ," & Val(.Cells("GRSWT").Value.ToString) - Val(.Cells("NETWT").Value.ToString) & "" 'LESSWT
                    StrSql += " ," & 0 & "" 'LESSWT
                    StrSql += " ,''" 'TAGNO
                    StrSql += " ," & Itemid & "" 'ITEMID
                    StrSql += " ," & subItemid & "" 'SUBITEMID
                    StrSql += " ," & 0 & "" 'WASTPER
                    StrSql += " ," & Val(.Cells("WASTAGE").Value.ToString) & "" 'WASTAGE
                    StrSql += " ," & 0 & "" 'MCGRM
                    StrSql += " ," & Val(.Cells("MC").Value.ToString) & "" 'MCHARGE
                    StrSql += " ," & Val(.Cells("AMOUNT").Value.ToString) & "" 'AMOUNT
                    StrSql += " ," & 0 & "" 'RATE
                    StrSql += " ," & Val(.Cells("RATE").Value.ToString) & "" 'BOARDRATE
                    StrSql += " ,''" 'SALEMODE
                    StrSql += " ,'" & grsnet & "'" 'GRSNET
                    StrSql += " ,''" 'TRANSTATUS ''
                    StrSql += " ,'" & txtBillNo.Text & "'" 'REFNO ''
                    StrSql += " ,'" & dtpBillDate_OWN.Value.ToString("yyyy-MM-dd") & "'" 'REFDATE NULL
                    StrSql += " ,'" & CostCenterId & "'" 'COSTID 
                    StrSql += " ,'" & strCompanyId & "'" 'COMPANYID
                    StrSql += " ,'O'" 'FLAG
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
                    StrSql += " ,0" 'DISCOUNT
                    StrSql += " ,''" 'RUNNO
                    StrSql += " ,''" 'CASHID
                    StrSql += " ,''" 'VATEXM
                    StrSql += " ," & Tax & "" 'TAX
                    StrSql += " ," & Tds & "" 'TDS
                    StrSql += " ," & Val(.Cells("STNAMT").Value.ToString) & "" 'STNAMT"
                    StrSql += " ,0" 'MISCAMT
                    StrSql += " ,'" & objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Cells("ITEM").Value.ToString & "'", , , tran) & "'" 'METALID
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
                Dim itemCtrId As Integer
                Dim Lasttagno As Integer
                Dim tagPrefix, tagno, TagSno As String
                Dim SHAPEid, Colorid, Clarityid, Settypeid As Integer
                Dim tagVal As String

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
                        itemCtrId = Val(.Item("DEFAULTCOUNTER").ToString)
                    End With
                End If

                StrSql = " SELECT TOP 1 DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME='" & cmbAcName.Text & "')"
                designerid = Val(objGPack.GetSqlValue(StrSql, , "", tran))

                Dim SEAL As String = ""
                Dim ItemSname As String = ""
                Dim Styleno As String = ""
                Dim uniqueid As String = ""
                SEAL = objGPack.GetSqlValue("SELECT SUBSTRING(SEAL,1,2) SEAL FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID=" & designerid & "", , "00", tran)
                ItemSname = objGPack.GetSqlValue("SELECT SUBSTRING(SHORTNAME,1,2) SNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=" & Itemid & "", , "00", tran)
                Styleno = .Cells("STYLENO").Value.ToString

                Dim stnSHAPEid, stnColorid, stnClarityid As Integer
                stnSHAPEid = 0 : stnColorid = 0 : stnClarityid = 0
                Dim drsstone() As DataRow = DtTran.Select("SNO='" & .Cells("SNO").Value.ToString & "' AND COLHEAD <>'G'", "")
                If drsstone.Length > 0 Then
                    stnSHAPEid = Val(objGPack.GetSqlValue("SELECT SHAPEID FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPENAME='" & drsstone(0).Item("SHAPE").ToString & "' ", , 0, tran).ToString)
                    stnColorid = Val(objGPack.GetSqlValue("SELECT COLORID FROM " & cnAdminDb & "..STNCOLOR WHERE COLORNAME='" & drsstone(0).Item("COLOR").ToString & "' ", , 0, tran).ToString)
                    stnClarityid = Val(objGPack.GetSqlValue("SELECT CLARITYID FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYNAME='" & drsstone(0).Item("CLARITY").ToString & "' ", , 0, tran).ToString)
                End If
                StrSql = "SELECT TOP 1 UNIQUEID FROM " & cnAdminDb & "..DIASTYLE "
                StrSql += " WHERE SHAPEID = " & stnSHAPEid & ""
                StrSql += " AND COLORID = " & stnColorid & ""
                StrSql += " AND CLARITYID = " & stnClarityid & ""
                uniqueid = objGPack.GetSqlValue(StrSql, , "0", tran)

                'tagPrefix = SEAL & Getidwithlen(Styleno, 6) & uniqueid
                If TAGXLUPLOAD_MAN_AUTO = "A" Then
                    tagPrefix = Getidwithlen(Styleno, 6) & uniqueid
                Else
                    tagPrefix = ""
                End If
                StrSql = " SELECT LASTNO FROM " & cnAdminDb & "..DIAUNIQUETAG WHERE UNQDESC='" & tagPrefix & "'"
                Lasttagno = objGPack.GetSqlValue(StrSql, , "0", tran)
                If Lasttagno = 0 Then
                    StrSql += " INSERT INTO " & cnAdminDb & "..DIAUNIQUETAG(UNQDESC,LASTNO) "
                    StrSql += " VALUES('" & tagPrefix & "',1)"
                    ExecQuery(SyncMode.Transaction, StrSql, cn, tran, CostCenterId)
                    Lasttagno = Lasttagno + 1
                Else
                    Lasttagno = Lasttagno + 1
                    StrSql = " UPDATE " & cnAdminDb & "..DIAUNIQUETAG SET LASTNO =" & Lasttagno & " WHERE UNQDESC='" & tagPrefix & "'"
                    ExecQuery(SyncMode.Transaction, StrSql, cn, tran, CostCenterId)
                End If
                If TAGXLUPLOAD_MAN_AUTO = "A" Then
                    If Trim(Styleno) = "" Then
                        tagno = objtag.GetTagNo(dtpTrandate.Value.ToString("yyyy-MM-dd"), .Cells("ITEM").Value.ToString, "", tran)
                    Else
                        tagno = tagPrefix & Getidwithlen(Lasttagno.ToString, 4)
                    End If
                ElseIf TAGXLUPLOAD_MAN_AUTO = "P" Then
                    If PURCHUPD_SEPTAGNO Then
                        tagno = objtag.GetTagNo(dtpTrandate.Value.ToString("yyyy-MM-dd"), .Cells("ITEM").Value.ToString, "", tran)
                    Else
                        tagno = Styleno
                    End If

                Else
                    tagno = objtag.GetTagNo(dtpTrandate.Value.ToString("yyyy-MM-dd"), .Cells("ITEM").Value.ToString, "", tran)
                End If

                tagVal = objtag.GetTagVal(tagno)
                If tagVal = 0 Then
                    StrSql = "SELECT ISNULL(MAX(TAGVAL),0)+1 FROM " & cnAdminDb & "..ITEMTAG "
                    tagVal = Val(objGPack.GetSqlValue(StrSql, , 0, tran).ToString)
                End If

                TagSno = GetNewSno(TranSnoType.ITEMTAGCODE, tran, "GET_ADMINSNO_TRAN")
                SHAPEid = Val(objGPack.GetSqlValue("SELECT SHAPEID FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPENAME='" & .Cells("SHAPE").Value.ToString & "' ", , 0, tran).ToString)
                Colorid = Val(objGPack.GetSqlValue("SELECT COLORID FROM " & cnAdminDb & "..STNCOLOR WHERE COLORNAME='" & .Cells("COLOR").Value.ToString & "' ", , 0, tran).ToString)
                Clarityid = Val(objGPack.GetSqlValue("SELECT CLARITYID FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYNAME='" & .Cells("CLARITY").Value.ToString & "' ", , 0, tran).ToString)
                Settypeid = Val(objGPack.GetSqlValue("SELECT SETTYPEID FROM " & cnAdminDb & "..STNSETTYPE WHERE SETTYPENAME='" & .Cells("SETTINGTYPE").Value.ToString & "' ", , 0, tran).ToString)

                tagnos = tagnos + "," + tagno

                Dim WupdIssSno As String = ""
                WupdIssSno = objGPack.GetSqlValue("SELECT SNO FROM " & cnAdminDb & "..WITEMTAG WHERE ITEMID='" & Itemid & "' AND TAGNO='" & .Cells("STYLENO").Value.ToString & "'", , , tran)
                Dim webtag As Boolean = False
                If .Cells("MARK").Value = True And WupdIssSno <> "" Then
                    StrSql = " UPDATE " & cnAdminDb & "..WITEMTAG SET "
                    StrSql += " ITEMCTRID = " & Val(itemCtrId) & "" 'ITEMCTRID
                    StrSql += " ,TABLECODE = '" & .Cells("TABLE").Value.ToString & "'"
                    StrSql += " ,DESIGNERID = " & designerid & "" 'DESIGNERID
                    StrSql += " ,PCS = " & Val(.Cells("PCS").Value.ToString) & "" 'PCS
                    StrSql += " ,GRSWT = " & Val(.Cells("GRSWT").Value.ToString) & "" 'GRSWT
                    StrSql += " ,LESSWT = " & Math.Round(Val(.Cells("GRSWT").Value.ToString) - Val(.Cells("NETWT").Value.ToString), 3) & "" 'LESSWT
                    StrSql += " ,NETWT = " & Val(.Cells("NETWT").Value.ToString) & "" 'NETWT
                    StrSql += " ,MAXWASTPER = " & Val(.Cells("MAXWASTPER").Value.ToString) & "" 'MAXWASTPER
                    StrSql += " ,MAXMCGRM = " & Val(.Cells("MAXMCGRM").Value.ToString) & "" 'MAXMCGRM
                    StrSql += " ,MAXWAST = " & Val(.Cells("MAXWASTAGE").Value.ToString) & "" 'MAXWAST
                    StrSql += " ,MAXMC = " & Val(.Cells("MAXMC").Value.ToString) & "" 'MAXMC
                    StrSql += " ,COMPANYID =  '" & strCompanyId & "'" 'COMPANYID
                    StrSql += " ,SALVALUE = " & Val(.Cells("SALVALUE").Value.ToString) & "" 'SALVALUE
                    StrSql += " ,ITEMTYPEID = " & Val(itemTypeId) & ""
                    StrSql += " ,USERID = " & userId & ""
                    StrSql += " ,UPDATED = '" & Today.Date.ToString("yyyy-MM-dd") & "'"
                    StrSql += " ,UPTIME = '" & Date.Now.ToLongTimeString & "'"
                    StrSql += " ,SYSTEMID = '" & systemId & "'"
                    'StrSql += " ,BOARDRATE = " & Val(.Cells("SRATE").Value.ToString) & ""
                    StrSql += " ,COLORID=" & Colorid
                    StrSql += " ,CLARITYID=" & Clarityid
                    StrSql += " ,SHAPEID=" & SHAPEid
                    StrSql += " ,SETTYPEID=" & Settypeid
                    StrSql += " WHERE SNO = '" & WupdIssSno & "'"
                    Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.ExecuteNonQuery()
                    webtag = True
                    StrSql = " DELETE FROM " & cnAdminDb & "..WITEMTAGSTONE WHERE TAGSNO='" & WupdIssSno & "' AND TAGNO='" & .Cells("STYLENO").Value.ToString & "'"
                    Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.ExecuteNonQuery()
                End If
                ''INSERTING ITEMTAG
                Dim lessWt As Decimal = Math.Round((Val(.Cells("GRSWT").Value.ToString) + Val(.Cells("MGRSWT").Value.ToString)) - (Val(.Cells("NETWT").Value.ToString) + Val(.Cells("MGRSWT").Value.ToString)), 3)
                StrSql = " INSERT INTO " & cnAdminDb & "..ITEMTAG"
                StrSql += " ("
                StrSql += " SNO,RECDATE,COSTID,ITEMID,ORDREPNO,ORSNO,ORDSALMANCODE,SUBITEMID,SIZEID"
                StrSql += " ,ITEMCTRID,TABLECODE,DESIGNERID,TAGNO,PCS,GRSWT,LESSWT,NETWT,RATE,FINERATE"
                StrSql += " ,MAXWASTPER,MAXMCGRM,MAXWAST,MAXMC,MINWASTPER,MINMCGRM,MINWAST,MINMC,TAGKEY"
                StrSql += " ,TAGVAL,LOTSNO,COMPANYID,SALVALUE,PURITY,NARRATION,DESCRIP,REASON,ENTRYMODE,GRSNET"
                StrSql += " ,ISSDATE,ISSREFNO,ISSPCS,ISSWT,FROMFLAG,TOFLAG,APPROVAL,SALEMODE,BATCHNO,MARK"
                StrSql += " ,VATEXM,PCTFILE,OLDTAGNO,ITEMTYPEID,ACTUALRECDATE,WEIGHTUNIT,TRANSFERWT,CHKDATE"
                StrSql += " ,CHKTRAY,CARRYFLAG,BRANDID,PRNFLAG,MCDISCPER,WASTDISCPER,RESDATE,TRANINVNO"
                StrSql += " ,SUPBILLNO,WORKDAYS,USERID,UPDATED,UPTIME,SYSTEMID,STYLENO,APPVER,TRANSFERDATE"
                StrSql += " ,BOARDRATE,RFID,TOUCH,HM_BILLNO,HM_CENTER,ADD_VA_PER,REFVALUE,VALUEADDEDTYPE"
                StrSql += " ,TCOSTID,EXTRAWT,USRATE,INDRS,RECSNO,FROMITEMID,SHAPEID"
                StrSql += " ,COLORID,CLARITYID,SETTYPEID,HEIGHT,WIDTH,RECTYPE) VALUES("
                StrSql += " '" & TagSno & "'" 'SNO
                StrSql += " ,'" & GetEntryDate(dtpTrandate.Value, tran).ToString("yyyy-MM-dd") & "'" 'RECDATE 'dtpRecieptDate.Value.Date.ToString("yyyy-MM-dd")
                StrSql += " ,'" & cnCostId & "'" 'COSTID
                StrSql += " ," & Itemid & "" 'ITEMID
                StrSql += " ,''" 'ORDREPNO
                StrSql += " ,''" 'ORsno
                StrSql += " ,''" 'ORDSALMANCODE
                StrSql += " ," & subItemid & "" 'SUBITEMID
                StrSql += " ,'" & IIf(Val(SizeId) <> 0, SizeId, "") & "'" 'SIZEID
                StrSql += " ," & itemCtrId & "" 'ITEMCTRID
                StrSql += " ,'" & .Cells("TABLE").Value.ToString & "'" 'TABLECODE
                StrSql += " ," & designerid & "" 'DESIGNERID
                StrSql += " ,'" & tagno & "'" 'TAGNO
                StrSql += " ," & Val(.Cells("PCS").Value.ToString) & "" 'PCS
                StrSql += " ," & Val(.Cells("GRSWT").Value.ToString + Val(.Cells("MGRSWT").Value.ToString)) & "" 'GRSWT
                StrSql += " ," & lessWt & "" 'LESSWT
                StrSql += " ," & Val(.Cells("NETWT").Value.ToString + Val(.Cells("MGRSWT").Value.ToString)) & "" 'NETWT
                If TAGXLUPLOAD_MAN_AUTO = "P" Then
                    StrSql += " ," & Val(.Cells("RATE").Value.ToString) & "" 'RATE
                Else
                    StrSql += " ," & 0 & "" 'RATE
                End If
                StrSql += ",0" 'FINERATE
                StrSql += " ," & Val(.Cells("MAXWASTPER").Value.ToString) & "" 'MAXWASTPER
                StrSql += " ," & Val(.Cells("MAXMCGRM").Value.ToString) & "" 'MAXMCGRM
                StrSql += " ," & Val(.Cells("MAXWASTAGE").Value.ToString) & "" 'MAXWAST
                StrSql += " ," & Val(.Cells("MAXMC").Value.ToString) & "" 'MAXMC
                StrSql += " ,0" 'MINWASTPER
                StrSql += " ,0" 'MINMCGRM
                StrSql += " ,0" 'MINWAST
                StrSql += " ,0" 'MINMC
                If PURCHUPD_TAGASTAGKEY Then
                    StrSql += " ,'" & tagno & "'" 'TAGKEY
                ElseIf PURCHUPD_STYLENOASTAGKEY Then
                    StrSql += " ,'" & Styleno & "'" 'TAGKEY
                Else
                    StrSql += " ,'" & Itemid.ToString & "" & tagno & "'" 'TAGKEY
                End If
                StrSql += " ,'" & tagVal & "'" 'TAGVAL
                StrSql += " ,''" 'LOTSNO
                StrSql += " ,'" & strCompanyId & "'" 'COMPANYID
                StrSql += " ," & Val(.Cells("SALVALUE").Value.ToString) & "" 'SALVALUE
                StrSql += " ,0" 'PURITY
                StrSql += " ,'" & .Cells("NARRATION").Value.ToString() & "'" 'NARRATION
                If .Cells("SUBITEM").Value.ToString.Trim <> "" Then 'DESCRIP
                    StrSql += " ,'" & .Cells("SUBITEM").Value.ToString & "'"
                Else
                    StrSql += " ,'" & .Cells("ITEM").Value.ToString & "'"
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
                StrSql += " ," & Val(itemTypeId) & "" 'ITEMTYPEID
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
                If TAGXLUPLOAD_MAN_AUTO = "A" Then
                    StrSql += " ,''" 'STYLENO
                Else
                    StrSql += " ,'" & Styleno & "'" 'STYLENO
                End If
                StrSql += " ,'" & VERSION & "'" 'APPVER
                StrSql += " ,'" & GetEntryDate(dtpTrandate.Value, tran).ToString("yyyy-MM-dd") & "'" 'TRANSFERDATE
                StrSql += " ," & Val(.Cells("SRATE").Value.ToString) & "" 'BOARDRATE
                StrSql += " ,''"
                StrSql += " ," & 0 & "" ' TOUCH
                StrSql += " ,''" 'HM_BILLNO
                StrSql += " ,''" 'HM_CENTER
                StrSql += " ,0" 'ADD_VA_PER
                StrSql += " ,0" 'REFVALUE
                StrSql += " ,'" & mlwmctype & "'"
                StrSql += " ,'" & cnCostId & "'" 'TCOSTID
                StrSql += " ,0" 'EXTRAWT
                StrSql += " ,0"
                StrSql += " ,0"
                StrSql += " ,'" & IIf(issSno <> "", issSno, WupdIssSno) & "'" 'RECSNO
                StrSql += " ,0" 'FROMITEMID
                StrSql += " ," & SHAPEid & "" 'SHAPEID
                StrSql += " ," & Colorid & "" 'COLORID
                StrSql += " ," & Clarityid & "" 'CLARITYID
                StrSql += " ," & Settypeid & "" 'SETTYPEID
                StrSql += " ," & Val(.Cells("HEIGHT").Value.ToString) & "" 'HEIGHT
                StrSql += " ," & Val(.Cells("WIDTH").Value.ToString) & "" 'WIDTH
                If TAGXLUPLOAD_MAN_AUTO = "A" And Trim(Styleno) = "" Then
                    StrSql += " ,'EX' " 'RECTYPE
                Else
                    StrSql += " ,'' " 'RECTYPE
                End If
                StrSql += " )"
                Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.ExecuteNonQuery()
                SKUTAGNOS += "'" & tagno & "',"
                Dim PurTax As Decimal = 0
                If Val(txtTaxper_PER.Text) <> 0 Then
                    PurTax = (Val(.Cells("TOTAMT").Value.ToString) * Val(txtTaxper_PER.Text)) / 100
                End If
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
                StrSql += vbCrLf + " ,'" & tagno & "'" 'TAGNO
                StrSql += vbCrLf + " ," & (Val(.Cells("GRSWT").Value.ToString) + Val(.Cells("MGRSWT").Value.ToString)) - (Val(.Cells("NETWT").Value.ToString) + Val(.Cells("MGRSWT").Value.ToString)) & "" ' PURLESSWT
                StrSql += vbCrLf + " ," & Val(.Cells("NETWT").Value.ToString + Val(.Cells("MGRSWT").Value.ToString)) & "" ' PURNETWT"
                StrSql += vbCrLf + " ," & Val(.Cells("RATE").Value.ToString) & "" ' PURRATE"
                StrSql += vbCrLf + " ,'" & grsnet & "'" ' PURGRSNET"
                StrSql += vbCrLf + " ," & Val(.Cells("WASTAGE").Value.ToString) & "" ' PURWASTAGE"
                StrSql += vbCrLf + " ," & Val(.Cells("TOUCH").Value.ToString) & "" ' PURTOUCH"
                StrSql += vbCrLf + " ," & Val(.Cells("MC").Value.ToString) & "" ' PURMC"
                StrSql += vbCrLf + " ," & (Val(.Cells("TOTAMT").Value.ToString) + PurTax) & "" ' PURVALUE"
                StrSql += vbCrLf + " ," & PurTax & ""
                StrSql += vbCrLf + " ,'" & dtpBillDate_OWN.Value.ToString("yyyy-MM-dd") & "'"
                StrSql += vbCrLf + " ,'" & strCompanyId & "'"
                StrSql += vbCrLf + " ,'" & cnCostId & "'"
                StrSql += vbCrLf + " )"
                Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.ExecuteNonQuery()

                ''ITEMTAGMETAL
                If Val(.Cells("MGRSWT").Value.ToString) > 0 Then
                    Dim MetalId As String = ""
                    Dim mCatCode As String = ""
                    StrSql = "SELECT METALID,CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=" & Itemid
                    Dim DrRow As DataRow
                    DrRow = GetSqlRow(StrSql, cn, tran)
                    If Not DrRow Is Nothing Then
                        MetalId = DrRow("METALID").ToString
                        mCatCode = DrRow("CATCODE").ToString
                    End If
                    StrSql = vbCrLf + " INSERT INTO " & cnAdminDb & "..ITEMTAGMETAL"
                    StrSql += vbCrLf + " ("
                    StrSql += vbCrLf + " SNO,METALID,COMPANYID,ITEMID,RECDATE,CATCODE,TAGNO,GRSWT,RATE,AMOUNT"
                    StrSql += vbCrLf + " ,TAGSNO,COSTID,SYSTEMID,WAST,MCGRM,MC,APPVER"
                    StrSql += vbCrLf + " )"
                    StrSql += vbCrLf + " VALUES"
                    StrSql += vbCrLf + " ("
                    StrSql += vbCrLf + " '" & GetNewSno(TranSnoType.ITEMTAGMETALCODE, tran, "GET_ADMINSNO_TRAN") & "'" 'SNO
                    StrSql += vbCrLf + " ,'" & MetalId & "'" 'METALID
                    StrSql += vbCrLf + " ,'" & strCompanyId & "'" 'COMPANYID
                    StrSql += vbCrLf + " ," & Itemid & "" 'ITEMID
                    StrSql += vbCrLf + " ,'" & dtpBillDate_OWN.Value.ToString("yyyy-MM-dd") & "'" 'RECDATE
                    StrSql += vbCrLf + " ,'" & mCatCode & "'" 'CATCODE
                    StrSql += vbCrLf + " ,'" & tagno & "'" 'TAGNO
                    StrSql += vbCrLf + " ," & Val(.Cells("GRSWT").Value.ToString) & "" ' GRSWT
                    StrSql += vbCrLf + " ," & Val(.Cells("RATE").Value.ToString) & "" ' RATE"
                    StrSql += vbCrLf + " ," & Val(.Cells("AMOUNT").Value.ToString) & "" ' MAMOUNT"
                    StrSql += vbCrLf + " ,'" & TagSno & "'" ' TAGSNO"
                    StrSql += vbCrLf + " ,'" & cnCostId & "'" 'COSTID
                    StrSql += vbCrLf + " ,'" & systemId & "'" 'SYSTEMID
                    StrSql += vbCrLf + " ,'" & Val(.Cells("WASTAGE").Value.ToString) & "'" 'WAST
                    StrSql += vbCrLf + " ," & Val(.Cells("MC").Value.ToString) & "" 'MCGRM
                    StrSql += vbCrLf + " ," & (Val(.Cells("MC").Value.ToString) * Val(.Cells("GRSWT").Value.ToString)) & "" 'MC
                    StrSql += vbCrLf + " ,'" & VERSION & "'" 'VERSION
                    StrSql += vbCrLf + " )"
                    Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.ExecuteNonQuery()

                    StrSql = "SELECT METALID,CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME='" & .Cells("CATEGORY").Value.ToString & "'"
                    Dim DrRw As DataRow
                    DrRw = GetSqlRow(StrSql, cn, tran)
                    MetalId = ""
                    mCatCode = ""
                    If Not DrRw Is Nothing Then
                        MetalId = DrRw("METALID").ToString
                        mCatCode = DrRw("CATCODE").ToString
                    End If
                    StrSql = vbCrLf + " INSERT INTO " & cnAdminDb & "..ITEMTAGMETAL"
                    StrSql += vbCrLf + " ("
                    StrSql += vbCrLf + " SNO,METALID,COMPANYID,ITEMID,RECDATE,CATCODE,TAGNO,GRSWT,RATE,AMOUNT"
                    StrSql += vbCrLf + " ,TAGSNO,COSTID,SYSTEMID,WAST,MCGRM,MC,APPVER"
                    StrSql += vbCrLf + " )"
                    StrSql += vbCrLf + " VALUES"
                    StrSql += vbCrLf + " ("
                    StrSql += vbCrLf + " '" & GetNewSno(TranSnoType.ITEMTAGMETALCODE, tran, "GET_ADMINSNO_TRAN") & "'" 'SNO
                    StrSql += vbCrLf + " ,'" & MetalId & "'" 'METALID
                    StrSql += vbCrLf + " ,'" & strCompanyId & "'" 'COMPANYID
                    StrSql += vbCrLf + " ," & Itemid & "" 'ITEMID
                    StrSql += vbCrLf + " ,'" & dtpBillDate_OWN.Value.ToString("yyyy-MM-dd") & "'" 'RECDATE
                    StrSql += vbCrLf + " ,'" & mCatCode & "'" 'CATCODE
                    StrSql += vbCrLf + " ,'" & tagno & "'" 'TAGNO
                    StrSql += vbCrLf + " ," & Val(.Cells("MGRSWT").Value.ToString) & "" ' GRSWT
                    StrSql += vbCrLf + " ," & Val(.Cells("MRATE").Value.ToString) & "" ' RATE"
                    StrSql += vbCrLf + " ," & Val(.Cells("MAMOUNT").Value.ToString) & "" ' MAMOUNT"
                    StrSql += vbCrLf + " ,'" & TagSno & "'" ' TAGSNO"
                    StrSql += vbCrLf + " ,'" & cnCostId & "'" 'COSTID
                    StrSql += vbCrLf + " ,'" & systemId & "'" 'SYSTEMID
                    StrSql += vbCrLf + " ,'" & Val(.Cells("MWASTAGE").Value.ToString) & "'" 'WAST
                    StrSql += vbCrLf + " ," & Val(.Cells("MMC").Value.ToString) & "" 'MCGRM
                    StrSql += vbCrLf + " ," & (Val(.Cells("MMC").Value.ToString) * Val(.Cells("MGRSWT").Value.ToString)) & "" 'MC
                    StrSql += vbCrLf + " ,'" & VERSION & "'" 'VERSION
                    StrSql += vbCrLf + " )"
                    Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.ExecuteNonQuery()
                End If

                Dim rowTag As DataRow = Nothing
                rowTag = dtTAGPrint.NewRow
                rowTag!ITEMID = Itemid
                rowTag!TAGNO = tagno
                dtTAGPrint.Rows.Add(rowTag)

                'StrSql = " UPDATE " & cnAdminDb & "..ITEMMAST SET CURRENTTAGNO='" & ttagno & "' WHERE ITEMID = "
                'StrSql += " (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Cells("ITEM").Value.ToString & "')"
                'Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.ExecuteNonQuery()

                Dim drstone() As DataRow = DtTran.Select("SNO='" & .Cells("SNO").Value.ToString & "' AND COLHEAD <>'G' AND COLHEAD <>'H'", "")
                For Each stRow As DataRow In drstone
                    InsertStoneDetails(issSno, TranNo, stRow, TagSno, tagno, Itemid, Tax, webtag, WupdIssSno, .Cells("STYLENO").Value.ToString)
                Next
                Dim DRHMDETAILS() As DataRow = DtTran.Select("SNO='" & .Cells("SNO").Value.ToString & "' AND COLHEAD <>'G' AND COLHEAD ='H' AND MARK=TRUE", "")
                For Each HMRow As DataRow In DRHMDETAILS
                    insertHallmarkDetails(HMRow, TagSno)
                Next
            End If
        End With
    End Sub


    Private Sub InsertStoneDetails(ByVal IssSno As String _
 , ByVal TNO As Integer, ByVal stRow As DataRow _
, ByVal Tagsno As String, ByVal Tagno As String, ByVal itemId As Integer _
, Optional ByVal taxx As Decimal = Nothing, Optional ByVal Webtag As Boolean = False, Optional ByVal Webtagsno As String = "", Optional ByVal Webtagno As String = "")
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
        Dim _StnRate As Decimal = Val(stRow.Item("SRATE").ToString)
        Dim _StnAmt As Decimal = Val(stRow.Item("SSTNAMT").ToString)
        If _StnRate = 0 Then _StnRate = Val(stRow.Item("RATE").ToString)
        If _StnAmt = 0 Then _StnAmt = Val(stRow.Item("STNAMT").ToString)
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
        StrSql += " ," & _StnRate & "" 'STNRATE
        StrSql += " ," & _StnAmt & "" 'STNAMT
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

        If Webtag = True And Webtagsno <> "" Then
            'INSERT WITEMTAGSTONE
            Dim wstnSno As String = GetNewSno(TranSnoType.WITEMTAGSTONECODE, tran, "GET_ADMINSNO_TRAN")
            StrSql = " INSERT INTO " & cnAdminDb & "..WITEMTAGSTONE("
            StrSql += " SNO,TAGSNO,ITEMID,COMPANYID,STNITEMID,"
            StrSql += " STNSUBITEMID,TAGNO,STNPCS,STNWT,"
            StrSql += " STNRATE,STNAMT,DESCRIP,"
            StrSql += " RECDATE,CALCMODE,"
            StrSql += " MINRATE,SIZECODE,STONEUNIT,ISSDATE,"
            StrSql += " OLDTAGNO,VATEXM,CARRYFLAG,COSTID,SYSTEMID,APPVER,"
            StrSql += " USRATE,INDRS,PACKETNO,SHAPEID,COLORID,CLARITYID,SETTYPEID,HEIGHT,WIDTH"
            StrSql += " )VALUES("
            StrSql += " '" & wstnSno & "'" ''SNO
            StrSql += " ,'" & Webtagsno & "'" 'TAGSNO
            StrSql += " ,'" & itemId & "'" 'ITEMID
            StrSql += " ,'" & strCompanyId & "'" 'COMPANYID
            StrSql += " ," & stnItemId & "" 'STNITEMID
            StrSql += " ," & stnSubItemid & "" 'STNSUBITEMID
            StrSql += " ,'" & Webtagno & "'" 'TAGNO
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

        End If
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


        If Val(stRow.Item("CERCHARGE").ToString) > 0 Then
            Dim miscSno As String = GetNewSno(TranSnoType.ITEMTAGMISCCHARCODE, tran, "GET_ADMINSNO_TRAN")
            StrSql = " INSERT INTO " & cnAdminDb & "..ITEMTAGMISCCHAR"
            StrSql += " ("
            StrSql += " SNO,ITEMID,TAGNO,MISCID,AMOUNT,"
            StrSql += " TAGSNO,COSTID,SYSTEMID,APPVER,COMPANYID)VALUES("
            StrSql += " '" & miscSno & "'" 'SNO
            StrSql += " ," & itemId & "" 'ITEMID
            StrSql += " ,'" & Tagno & "'" 'TAGNO
            StrSql += " ," & CER_CHARGESID & ""  'MISCID
            StrSql += " ," & Val(stRow.Item("CERCHARGE").ToString) & "" 'AMOUNT
            StrSql += " ,'" & Tagsno & "'" 'TAGSNO
            StrSql += " ,'" & cnCostId & "'" 'COSTID
            StrSql += " ,'" & systemId & "'" 'SYSTEMID
            StrSql += " ,'" & VERSION & "'" 'APPVER
            StrSql += " ,'" & GetStockCompId() & "'" ' COMPANYID
            StrSql += " )"
            Cmd = New OleDbCommand(StrSql, cn, tran) : Cmd.ExecuteNonQuery()
        End If
    End Sub

    Private Sub insertHallmarkDetails(ByVal stRow As DataRow, ByVal Tagsno As String)
        Dim HALLMARKSno As String = ""
        HALLMARKSno = GetNewSno(TranSnoType.ITEMTAGHALLMARKCODE, tran, "GET_ADMINSNO_TRAN")
        StrSql = " INSERT INTO " & cnAdminDb & "..ITEMTAGHALLMARK"
        StrSql += " ("
        StrSql += " SNO,TAGSNO,GRSWT,HM_BILLNO,USERID,SYSTEMID,APPVER,UPDATED,UPTIME,COSTID,COMPANYID"
        StrSql += " )VALUES("
        StrSql += " '" & HALLMARKSno & "'" 'SNO
        StrSql += " ,'" & Tagsno & "'" 'TAGSNO
        StrSql += " ,'" & Val(stRow.Item("HM_WT").ToString) & "'" 'GRSWT
        StrSql += " ,'" & stRow.Item("HM_BILLNO").ToString & "'" 'HM_BILLNO
        StrSql += " ," & userId & "" 'USERID
        StrSql += " ,'" & systemId & "'" 'SYSTEMID
        StrSql += " ,'" & VERSION & "'" 'APPVER
        StrSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        StrSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
        StrSql += " ,'" & cnCostId & "'" 'COSTID
        StrSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
        StrSql += " )"
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

    'Private Sub cmbAcName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAcName.GotFocus
    '    If cmbCostCentre.Enabled = True And cmbCostCentre.Text = "" Then cmbCostCentre.Focus()
    'End Sub

    Private Sub dtpCreditDays_Enter(ByVal sender As Object, ByVal e As System.EventArgs)
        SendKeys.Send("{tab}")
    End Sub


    Private Sub cmbCostCentre_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        CostCenterId = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre.Text & "'", , , tran)
    End Sub

    'Private Sub cmbAcName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbAcName.KeyPress
    '    If e.KeyChar = Chr(Keys.Enter) Then
    '        StrSql = " SELECT TOP 1 DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME='" & cmbAcName.Text & "')"
    '        designerid = objGPack.GetSqlValue(StrSql, , 0, )
    '        If designerid = 0 Then MsgBox("Designer id not available for this acname,Check designer master", MsgBoxStyle.Information) : Me.SelectNextControl(cmbAcName, False, True, True, False) : Exit Sub
    '        Maxwastper = 0
    '        Maxwastage = 0
    '        MaxmcGrm = 0
    '        Maxmc = 0
    '    End If
    'End Sub

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
            If designerid = 0 Then MsgBox("Designer id not available for this acname,Check designer master", MsgBoxStyle.Information) : cmbAcName.Focus() : Exit Sub 'Me.SelectNextControl(cmbAcName, False, True, True, False) : Exit Sub

            'StrSql = " SELECT MAXWASTPER,MAXWAST,MAXMCGRM,MAXMC FROM " & cnAdminDb & "..WMCTABLE WHERE DESIGNERID = '" & designerid & "'"
            'Dim dtItemDetail As New DataTable
            'Cmd = New OleDbCommand(StrSql, cn, tran)
            'Da = New OleDbDataAdapter(Cmd)
            'Da.Fill(dtItemDetail)
            'If dtItemDetail.Rows.Count > 0 Then
            '    With dtItemDetail.Rows(0)
            '        Maxwastper = Val(.Item("MAXWASTPER").ToString)
            '        Maxwastage = Val(.Item("MAXWAST").ToString)
            '        MaxmcGrm = Val(.Item("MAXMCGRM").ToString)
            '        Maxmc = Val(.Item("MAXMC").ToString)
            '    End With
            'End If
        End If
    End Sub
    Private Sub cmbCostCentre_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCostCentre.TextChanged
        CostCenterId = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre.Text & "'", , , tran)
    End Sub


    Private Sub BtnExcelDownload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnExcelDownload.Click
        Dim OpenDialog As New OpenFileDialog
        Dim Str As String
        Str = "(*.xlsx) | *.xlsx"
        Str += "|(*.xls) | *.xls"
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
            MyConnection = New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" & Path & "';Extended Properties=""Excel 12.0 Xml;HDR=YES;IMEX=1;""")
            StrSql = "SELECT * FROM [SHEET1$]"
            Da = New OleDbDataAdapter(StrSql, MyConnection)
            Dim Dt As New DataTable
            Da.Fill(Dt)
            MyConnection.Close()
            Dim Itemname As String = ""
            Dim SubItemname As String = ""
            Dim itemTypeId As Integer = 0
            If Dt.Rows.Count > 1 Then
                Dim sno As String = ""
                Dim v As Integer = 1
                If CmbValueAdded.Text = "ITEM" Then
                    TAGXLUPLOAD_WASTMCCALC = "I"
                ElseIf CmbValueAdded.Text = "DESIGNER" Then
                    TAGXLUPLOAD_WASTMCCALC = "D"
                ElseIf CmbValueAdded.Text = "TABLE" Then
                    TAGXLUPLOAD_WASTMCCALC = "T"
                ElseIf TAGXLUPLOAD_WASTMCCALC = "E" Then
                    CmbValueAdded.Text = "EXCEL"
                Else
                    TAGXLUPLOAD_WASTMCCALC = "P"
                End If
                Dim DesignerVA As Boolean = False
                StrSql = "SELECT TOP 1 * FROM " & cnAdminDb & "..DESIGNERVA "
                StrSql += " WHERE DESIGNERID=" & designerid & " AND ISNULL(ACTIVE,'Y')<>'N'"
                StrSql += " AND (WASTPER<>0 OR WASTAGE<>0 OR MCGRM<>0 OR MC<>0)"
                Dim drRowVA As DataRow
                drRowVA = GetSqlRow(StrSql, cn, tran)
                If Not drRowVA Is Nothing Then
                    Maxwastper = Val(drRowVA.Item("WASTPER").ToString)
                    Maxwastage = Val(drRowVA.Item("WASTAGE").ToString)
                    MaxmcGrm = Val(drRowVA.Item("MCGRM").ToString)
                    Maxmc = Val(drRowVA.Item("MC").ToString)
                    DesignerVA = True
                End If
                Dim calType As String = ""
                For i As Integer = 1 To Dt.Rows.Count - 1
                    If Dt.Rows(i).Item(0).ToString <> "" Then
                        sno = Dt.Rows(i).Item(0).ToString
                    End If

                    If Dt.Rows(i).Item(32).ToString <> "" Then
                        If Dt.Rows(i).Item(32).ToString = "G" Then grsnet = "G"
                        If Dt.Rows(i).Item(32).ToString = "N" Then grsnet = "N"
                    End If

                    Dim ChkCount As Integer = 0
                    If (Val(Dt.Rows(i).Item(6).ToString) <> 0 Or Val(Dt.Rows(i).Item(7).ToString) <> 0 Or Val(Dt.Rows(i).Item(8).ToString) <> 0) And Dt.Rows(i).Item("ITEM").ToString <> "HUID" Then
                        StrSql = " SELECT 1 FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & Dt.Rows(i).Item(2).ToString.Trim & "'"
                        If objGPack.GetSqlValue(StrSql, , 0) <> 1 Then
                            MsgBox(Dt.Rows(i).Item(2).ToString & " Not found in itemmaster ", MsgBoxStyle.Information)
                        End If
                        StrSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & Dt.Rows(i).Item(2).ToString.Trim & "'"
                        Dim Itemid As Integer = Val(objGPack.GetSqlValue(StrSql).ToString)

                        StrSql = " SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME='" & Dt.Rows(i).Item(31).ToString.Trim & "'"
                        Dim SubItemid As Integer = Val(objGPack.GetSqlValue(StrSql).ToString)

                        StrSql = "SELECT 1 FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & Dt.Rows(i).Item(31).ToString.Trim & "'"
                        If objGPack.GetSqlValue(StrSql, , 0) <> 1 Then
                            MsgBox(Dt.Rows(i).Item(31).ToString & " Not found in ITEMTYPE ", MsgBoxStyle.Information)
                        End If
                        StrSql = "SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & Dt.Rows(i).Item(31).ToString.Trim & "'"
                        itemTypeId = Val(objGPack.GetSqlValue(StrSql).ToString)
                        Dim SizeName As String = ""
                        If Dt.Rows(i).Item(34).ToString.Trim <> "" Then
                            StrSql = "SELECT 1 FROM " & cnAdminDb & "..ITEMSIZE WHERE SIZENAME = '" & Dt.Rows(i).Item(34).ToString.Trim & "' AND ITEMID='" & Val(Itemid) & "'"
                            If objGPack.GetSqlValue(StrSql, , 0) <> 1 Then
                                MsgBox(Dt.Rows(i).Item(31).ToString & " Not found in ItemSize ", MsgBoxStyle.Information)
                            Else
                                SizeName = Dt.Rows(i).Item(34).ToString.Trim
                            End If
                        End If


                        Dim dtrow As DataRow = Nothing
                        Dim dtVa As New DataTable
                        Dim TblCode As String
                        If TAGXLUPLOAD_WASTMCCALC = "D" And DesignerVA = False Then
                            StrSql = " SELECT MAXWASTPER,MAXWAST,MAXMCGRM,MAXMC FROM " & cnAdminDb & "..WMCTABLE "
                            StrSql += " WHERE DESIGNERID = '" & designerid & "'"
                            StrSql += " AND '" & IIf(grsnet = "N", Val(Dt.Rows(i).Item(8).ToString), Val(Dt.Rows(i).Item(7).ToString)) & "' BETWEEN FROMWEIGHT AND TOWEIGHT"
                            If CostCenterId <> "" Then StrSql += " AND COSTID = '" & CostCenterId & "'"
                            Cmd = New OleDbCommand(StrSql, cn, tran)
                            Da = New OleDbDataAdapter(Cmd)
                            dtVa = New DataTable
                            Da.Fill(dtVa)
                            If dtVa.Rows.Count > 0 Then
                                With dtVa.Rows(0)
                                    Maxwastper = Val(.Item("MAXWASTPER").ToString)
                                    Maxwastage = Val(.Item("MAXWAST").ToString)
                                    MaxmcGrm = Val(.Item("MAXMCGRM").ToString)
                                    Maxmc = Val(.Item("MAXMC").ToString)
                                End With
                            End If
                        ElseIf TAGXLUPLOAD_WASTMCCALC = "I" And DesignerVA = False Then
ItemRecheck:
                            StrSql = " SELECT MAXWASTPER,MAXWAST,MAXMCGRM,MAXMC FROM " & cnAdminDb & "..WMCTABLE "
                            StrSql += " WHERE DESIGNERID = '0'"
                            StrSql += " AND ITEMID = " & Itemid & " "
                            If SubItemid <> 0 Then
                                StrSql += "  AND SUBITEMID =" & SubItemid
                                'Else
                                '    StrSql += "  AND SUBITEMID =0"
                            End If
                            StrSql += " AND '" & IIf(grsnet = "N", Val(Dt.Rows(i).Item(8).ToString), Val(Dt.Rows(i).Item(7).ToString)) & "' BETWEEN FROMWEIGHT AND TOWEIGHT"
                            If Dt.Rows(i).Item(26).ToString.ToUpper <> "" And ChkCount = 0 Then
                                StrSql += " AND ITEMTYPE = (SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & Dt.Rows(i).Item(26).ToString.ToUpper & "')"
                            End If
                            If CostCenterId <> "" Then StrSql += " AND COSTID = '" & CostCenterId & "'"
                            dtVa = New DataTable
                            Cmd = New OleDbCommand(StrSql, cn, tran)
                            Da = New OleDbDataAdapter(Cmd)
                            Da.Fill(dtVa)
                            If dtVa.Rows.Count > 0 Then
                                With dtVa.Rows(0)
                                    Maxwastper = Val(.Item("MAXWASTPER").ToString)
                                    Maxwastage = Val(.Item("MAXWAST").ToString)
                                    MaxmcGrm = Val(.Item("MAXMCGRM").ToString)
                                    Maxmc = Val(.Item("MAXMC").ToString)
                                End With
                            Else
                                If ChkCount = 0 Then ChkCount += 1 : GoTo ItemRecheck
                            End If
                        ElseIf TAGXLUPLOAD_WASTMCCALC = "T" And DesignerVA = False Then
                            StrSql = " SELECT TABLECODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=" & Itemid
                            TblCode = objGPack.GetSqlValue(StrSql).ToString
                            StrSql = " SELECT MAXWASTPER,MAXWAST,MAXMCGRM,MAXMC FROM " & cnAdminDb & "..WMCTABLE "
                            StrSql += " WHERE TABLECODE= '" & TblCode & "'"
                            StrSql += " AND '" & IIf(grsnet = "N", Val(Dt.Rows(i).Item(8).ToString), Val(Dt.Rows(i).Item(7).ToString)) & "' BETWEEN FROMWEIGHT AND TOWEIGHT"
                            If CostCenterId <> "" Then StrSql += " AND COSTID = '" & CostCenterId & "'"
                            dtVa = New DataTable
                            Cmd = New OleDbCommand(StrSql, cn, tran)
                            Da = New OleDbDataAdapter(Cmd)
                            Da.Fill(dtVa)
                            If dtVa.Rows.Count > 0 And TblCode <> "" Then
                                With dtVa.Rows(0)
                                    Maxwastper = Val(.Item("MAXWASTPER").ToString)
                                    Maxwastage = Val(.Item("MAXWAST").ToString)
                                    MaxmcGrm = Val(.Item("MAXMCGRM").ToString)
                                    Maxmc = Val(.Item("MAXMC").ToString)
                                End With
                            End If
                        ElseIf TAGXLUPLOAD_WASTMCCALC = "P" And DesignerVA = False Then
                            StrSql = " SELECT MAXWASTPER,MAXWAST,MAXMCGRM,MAXMC FROM " & cnAdminDb & "..WMCTABLE "
                            StrSql += " WHERE ITEMTYPE = (SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & Dt.Rows(i).Item(26).ToString.ToUpper & "')"
                            StrSql += " AND '" & IIf(grsnet = "N", Val(Dt.Rows(i).Item(8).ToString), Val(Dt.Rows(i).Item(7).ToString)) & "' BETWEEN FROMWEIGHT AND TOWEIGHT"
                            If CostCenterId <> "" Then StrSql += " AND COSTID = '" & CostCenterId & "'"
                            dtVa = New DataTable
                            Cmd = New OleDbCommand(StrSql, cn, tran)
                            Da = New OleDbDataAdapter(Cmd)
                            Da.Fill(dtVa)
                            If dtVa.Rows.Count > 0 Then
                                With dtVa.Rows(0)
                                    Maxwastper = Val(.Item("MAXWASTPER").ToString)
                                    Maxwastage = Val(.Item("MAXWAST").ToString)
                                    MaxmcGrm = Val(.Item("MAXMCGRM").ToString)
                                    Maxmc = Val(.Item("MAXMC").ToString)
                                End With
                            End If
                        ElseIf TAGXLUPLOAD_WASTMCCALC = "E" And DesignerVA = False Then
                            Maxwastper = Val(Dt.Rows(i).Item(27).ToString)
                            Maxwastage = Val(Dt.Rows(i).Item(28).ToString)
                            MaxmcGrm = Val(Dt.Rows(i).Item(29).ToString)
                            Maxmc = Val(Dt.Rows(i).Item(30).ToString)
                        End If

                        Dim PURRATE As Decimal = 0
                        itemTypeId = Val(objGPack.GetSqlValue("SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & Dt.Rows(i).Item(26).ToString.Trim & "'", , 0, tran))
                        If itemTypeId <> 0 Then
                            Dim purityid As String = objGPack.GetSqlValue("SELECT PURITYID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & Dt.Rows(i).Item(26).ToString.Trim & "'", , 0, tran)
                            If purityid <> "" Then PURRATE = Val(GetRate_Purity(dtpBillDate_OWN.Value, purityid))
                        End If
                        Dim RATE As Decimal = Val(GetRate(dtpBillDate_OWN.Value, objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & Dt.Rows(i).Item(2).ToString & "'")))
                        If PURRATE = 0 Then PURRATE = RATE

                        dtrow = DtTran.NewRow

                        If CheckExists("ITEMMAST", "ITEMNAME", Dt.Rows(i).Item(2).ToString.Trim) = False Then
                            dtrow("REMARKS") = "IM,"
                        End If
                        If CheckExists("SUBITEMMAST", "SUBITEMNAME", Dt.Rows(i).Item(3).ToString.Trim) = False Then
                            dtrow("REMARKS") = dtrow("REMARKS") & "SI,"
                        End If
                        If CheckExists("ITEMTYPE", "NAME", Dt.Rows(i).Item(26).ToString.Trim) = False Then
                            dtrow("REMARKS") = dtrow("REMARKS") & "IT,"
                        End If
                        If CheckExists("ITEMSIZE", "ITEMID=" & Val(Itemid) & " AND SIZENAME", Dt.Rows(i).Item(34).ToString.Trim) = False Then
                            dtrow("REMARKS") = dtrow("REMARKS") & "SZ,"
                        End If
                        dtrow("MARK") = False
                        dtrow("SNO") = sno
                        dtrow("STYLENO") = Dt.Rows(i).Item(1).ToString.ToUpper
                        dtrow("ITEM") = Dt.Rows(i).Item(2).ToString.Trim.ToUpper
                        calType = objGPack.GetSqlValue("SELECT CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & dtrow("ITEM").ToString & "'")
                        dtrow("SUBITEM") = Dt.Rows(i).Item(3).ToString.Trim.ToUpper
                        dtrow("HEIGHT") = Val(Dt.Rows(i).Item(4).ToString)
                        dtrow("WIDTH") = Val(Dt.Rows(i).Item(5).ToString)
                        dtrow("PCS") = Val(Dt.Rows(i).Item(6).ToString)
                        dtrow("GRSWT") = Val(Dt.Rows(i).Item(7).ToString)
                        dtrow("NETWT") = Val(Dt.Rows(i).Item(8).ToString)
                        dtrow("RATE") = Format(Val(Dt.Rows(i).Item(11).ToString.Replace(",", "")), "0.00")
                        dtrow("WASTAGE") = Val(Dt.Rows(i).Item(9).ToString)
                        dtrow("MC") = Val(Dt.Rows(i).Item(10).ToString)
                        dtrow("AMOUNT") = Format(Val(Dt.Rows(i).Item(12).ToString.Replace(",", "")), "0.00")
                        dtrow("SALVALUE") = Format(Val(Dt.Rows(i).Item(12).ToString.Replace(",", "")), "0.00")
                        dtrow("MAXWASTPER") = Maxwastper
                        dtrow("MAXWASTAGE") = Maxwastage
                        dtrow("MAXMCGRM") = MaxmcGrm
                        dtrow("MAXMC") = Maxmc
                        dtrow("METALTYPE") = Dt.Rows(i).Item(26).ToString.ToUpper
                        If TAGXLUPLOAD_MAN_AUTO = "A" Then
                            If Dt.Columns.Count > 27 Then
                                dtrow("NARRATION") = Dt.Rows(i).Item(27).ToString.ToUpper
                            End If
                        ElseIf TAGXLUPLOAD_MAN_AUTO = "P" Then
                            If Dt.Columns.Count > 27 Then
                                dtrow("NARRATION") = Dt.Rows(i).Item(27).ToString.ToUpper
                            End If
                        End If
                        dtrow("COLHEAD") = "G"
                        dtrow("TABLE") = TblCode
                        dtrow("SRATE") = IIf(PURRATE <> 0, Format(PURRATE, "0.00"), "0.00")
                        If TAGXLUPLOAD_MAN_AUTO = "J" And calType = "F" Then
                            dtrow("SALVALUE") = Format(Val(Dt.Rows(i).Item(27).ToString.Replace(",", "")), "0.00")
                        End If
                        If TAGXLUPLOAD_MAN_AUTO = "J" And Dt.Columns.Count >= 33 Then
                            dtrow("CATEGORY") = Dt.Rows(i).Item(27).ToString.Replace(",", "")
                            dtrow("MGRSWT") = Format(Val(Dt.Rows(i).Item(28).ToString.Replace(",", "")), "0.000")
                            dtrow("MWASTAGE") = Maxwastage 'Format(Val(Dt.Rows(i).Item(29).ToString.Replace(",", "")), "0.000")
                            dtrow("MMC") = MaxmcGrm 'Format(Val(Dt.Rows(i).Item(30).ToString.Replace(",", "")), "0.00")
                            dtrow("MRATE") = Format(Val(Dt.Rows(i).Item(31).ToString.Replace(",", "")), "0.00")
                            dtrow("MAMOUNT") = Format(Val(Dt.Rows(i).Item(32).ToString.Replace(",", "")), "0.00")
                        End If
                        If TAGXLUPLOAD_MAN_AUTO = "P" Then
                            'dtrow("MAXWASTPER") = IIf(IsDBNull(Dt.Rows(i).Item(28).ToString), "0.00", Dt.Rows(i).Item(28).ToString)
                            'dtrow("MAXMCGRM") = IIf(IsDBNull(Dt.Rows(i).Item(29).ToString), "0.00", Dt.Rows(i).Item(29).ToString)
                            'dtrow("TOUCH") = IIf(IsDBNull(Dt.Rows(i).Item(30).ToString), "0.00", Dt.Rows(i).Item(30).ToString)
                            'dtrow("TAGTYPE") = IIf(IsDBNull(Dt.Rows(i).Item(31).ToString), "0.00", Dt.Rows(i).Item(31).ToString)
                            dtrow("MAXWASTPER") = IIf(Val(Dt.Rows(i).Item(28).ToString) = 0, "0.00", Dt.Rows(i).Item(28).ToString)
                            dtrow("MAXMCGRM") = IIf(Val(Dt.Rows(i).Item(29).ToString) = 0, "0.00", Dt.Rows(i).Item(29).ToString)
                            dtrow("TOUCH") = IIf(Dt.Rows(i).Item(30).ToString = "", "0.00", Dt.Rows(i).Item(30).ToString)
                            dtrow("TAGTYPE") = IIf(Dt.Rows(i).Item(31).ToString = "", "0.00", Dt.Rows(i).Item(31).ToString)
                            dtrow("TAGNO") = IIf(Dt.Rows(i).Item(33).ToString = "", "", Dt.Rows(i).Item(33).ToString)
                        End If
                        dtrow("SIZE") = SizeName.ToString
                        DtTran.Rows.Add(dtrow)
                        DtTran.AcceptChanges()
                        v += 1
                    ElseIf (Val(Dt.Rows(i).Item(17).ToString) <> 0 Or Val(Dt.Rows(i).Item(18).ToString) <> 0) And Dt.Rows(i).Item("ITEM").ToString <> "HUID" Then
                        Dim dtStrow As DataRow = Nothing
                        dtStrow = DtTran.NewRow
                        If CheckExists("ITEMMAST", "ITEMNAME", Dt.Rows(i).Item(2).ToString.Trim) = False Then
                            dtStrow("REMARKS") = "IM,"
                        End If
                        If CheckExists("SUBITEMMAST", "SUBITEMNAME", Dt.Rows(i).Item(3).ToString.Trim) = False Then
                            dtStrow("REMARKS") = dtStrow("REMARKS") & "SI,"
                        End If
                        If CheckExists("STNSHAPE", "SHAPENAME", Dt.Rows(i).Item(13).ToString.Trim) = False Then
                            dtStrow("REMARKS") = dtStrow("REMARKS") & "SH,"
                        End If
                        If CheckExists("STNSETTYPE", "SETTYPENAME", Dt.Rows(i).Item(14).ToString.Trim) = False Then
                            dtStrow("REMARKS") = dtStrow("REMARKS") & "ST,"
                        End If
                        If CheckExists("STNCOLOR", "COLORNAME", Dt.Rows(i).Item(15).ToString.Trim) = False Then
                            dtStrow("REMARKS") = dtStrow("REMARKS") & "SC,"
                        End If
                        If CheckExists("STNCLARITY", "CLARITYNAME", Dt.Rows(i).Item(16).ToString.Trim) = False Then
                            dtStrow("REMARKS") = dtStrow("REMARKS") & "SR,"
                        End If
                        dtStrow("MARK") = False
                        dtStrow("SNO") = sno
                        dtStrow("STYLENO") = Dt.Rows(i).Item(1).ToString.ToUpper
                        dtStrow("ITEM") = Dt.Rows(i).Item(2).ToString.Trim.ToUpper
                        dtStrow("SUBITEM") = Dt.Rows(i).Item(3).ToString.Trim.ToUpper
                        dtStrow("PCS") = Val(Dt.Rows(i).Item(17).ToString)
                        dtStrow("GRSWT") = Val(Dt.Rows(i).Item(18).ToString)
                        dtStrow("RATE") = Format(Val(Dt.Rows(i).Item(19).ToString.Replace(",", "")), "0.00")
                        dtStrow("STNAMT") = Format(Val(Dt.Rows(i).Item(20).ToString.Replace(",", "")), "0.00")
                        If TAGXLUPLOAD_WASTMCCALC = "E" Then
                            dtStrow("SRATE") = Format(Val(Dt.Rows(i).Item(31).ToString.Replace(",", "")), "0.00")
                            dtStrow("SSTNAMT") = Format(Val(Dt.Rows(i).Item(32).ToString.Replace(",", "")), "0.00")
                        End If
                        dtStrow("SHAPE") = Dt.Rows(i).Item(13).ToString.ToUpper
                        dtStrow("SETTINGTYPE") = Dt.Rows(i).Item(14).ToString.ToUpper
                        dtStrow("COLOR") = Dt.Rows(i).Item(15).ToString.ToUpper
                        dtStrow("CLARITY") = Dt.Rows(i).Item(16).ToString.ToUpper
                        dtStrow("COLHEAD") = "D"
                        DtTran.Rows.Add(dtStrow)
                        DtTran.AcceptChanges()
                        If TAGXLUPLOAD_WASTMCCALC <> "E" And calType <> "F" Then calcStnrate(v - 1)
                        v += 1
                    ElseIf (Val(Dt.Rows(i).Item(21).ToString) <> 0 Or Val(Dt.Rows(i).Item(22).ToString) <> 0) And Dt.Rows(i).Item("ITEM").ToString <> "HUID" Then
                        'StrSql = " SELECT 1 FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & Dt.Rows(i).Item(2).ToString.Trim & "' "
                        'If objGPack.GetSqlValue(StrSql, , 0) <> 1 Then
                        '    If Itemname.Contains(Dt.Rows(i).Item(2).ToString) = False Then Itemname = Itemname + "," + Dt.Rows(i).Item(2).ToString
                        'End If
                        Dim dtStrow As DataRow = Nothing
                        dtStrow = DtTran.NewRow
                        If CheckExists("ITEMMAST", "ITEMNAME", Dt.Rows(i).Item(2).ToString.Trim) = False Then
                            dtStrow("REMARKS") = "IM,"
                        End If
                        If CheckExists("SUBITEMMAST", "SUBITEMNAME", Dt.Rows(i).Item(3).ToString.Trim) = False Then
                            dtStrow("REMARKS") = dtStrow("REMARKS") & "SI,"
                        End If
                        If CheckExists("STNSHAPE", "SHAPENAME", Dt.Rows(i).Item(13).ToString.Trim) = False Then
                            dtStrow("REMARKS") = dtStrow("REMARKS") & "SH,"
                        End If
                        If CheckExists("STNSETTYPE", "SETTYPENAME", Dt.Rows(i).Item(14).ToString.Trim) = False Then
                            dtStrow("REMARKS") = dtStrow("REMARKS") & "ST,"
                        End If
                        If CheckExists("STNCOLOR", "COLORNAME", Dt.Rows(i).Item(15).ToString.Trim) = False Then
                            dtStrow("REMARKS") = dtStrow("REMARKS") & "SC,"
                        End If
                        If CheckExists("STNCLARITY", "CLARITYNAME", Dt.Rows(i).Item(16).ToString.Trim) = False Then
                            dtStrow("REMARKS") = dtStrow("REMARKS") & "SR,"
                        End If
                        dtStrow("MARK") = False
                        dtStrow("SNO") = sno
                        dtStrow("STYLENO") = Dt.Rows(i).Item(1).ToString.ToUpper
                        dtStrow("ITEM") = Dt.Rows(i).Item(2).ToString.Trim.ToUpper
                        dtStrow("SUBITEM") = Dt.Rows(i).Item(3).ToString.Trim.ToUpper
                        dtStrow("PCS") = Val(Dt.Rows(i).Item(21).ToString)
                        dtStrow("GRSWT") = Val(Dt.Rows(i).Item(22).ToString)
                        dtStrow("RATE") = Format(Val(Dt.Rows(i).Item(23).ToString.Replace(",", "")), "0.00")
                        dtStrow("STNAMT") = Format(Val(Dt.Rows(i).Item(24).ToString.Replace(",", "")), "0.00")
                        If TAGXLUPLOAD_WASTMCCALC = "E" Then
                            dtStrow("SRATE") = Format(Val(Dt.Rows(i).Item(31).ToString.Replace(",", "")), "0.00")
                            dtStrow("SSTNAMT") = Format(Val(Dt.Rows(i).Item(32).ToString.Replace(",", "")), "0.00")
                        End If
                        dtStrow("SHAPE") = Dt.Rows(i).Item(13).ToString.ToUpper
                        dtStrow("SETTINGTYPE") = Dt.Rows(i).Item(14).ToString.ToUpper
                        dtStrow("COLOR") = Dt.Rows(i).Item(15).ToString.ToUpper
                        dtStrow("CLARITY") = Dt.Rows(i).Item(16).ToString.ToUpper
                        dtStrow("COLHEAD") = "S"
                        DtTran.Rows.Add(dtStrow)
                        DtTran.AcceptChanges()
                        If TAGXLUPLOAD_WASTMCCALC <> "E" And calType <> "F" Then calcStnrate(v - 1, Val(Dt.Rows(i).Item(23).ToString.Replace(",", "")))
                        v += 1
                    ElseIf Dt.Rows(i).Item("ITEM").ToString = "HUID" Then
                        Dim Htagsno As String = ""
                        Dim chkhmno As String = ""
                        Dim chkhmWT As Double = 0
                        Dim dtStrow As DataRow = Nothing
                        dtStrow = DtTran.NewRow
                        dtStrow("MARK") = False
                        dtStrow("SNO") = sno
                        dtStrow("ITEM") = "HUID"
                        chkhmno = Dt.Rows(i).Item(36).ToString
                        chkhmWT = Val(Dt.Rows(i).Item(35).ToString)
                        If chkhmno.ToString = "" Or chkhmno.ToString = "0" Then
                            dtStrow("SUBITEM") = "Hallmark No is Empty"
                            dtStrow("REMARKS") = dtStrow("REMARKS") & "HUID,"
                        End If
                        StrSql = "SELECT DISTINCT TAGSNO FROM (SELECT ISNULL(SNO,'')TAGSNO FROM " & cnAdminDb & "..ITEMTAG WHERE  HM_BILLNO = '" & chkhmno.ToString & "'"
                        StrSql += " AND SNO NOT IN (SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE LTRIM(ITEMID)+'-'+TAGNO IN "
                        StrSql += " (SELECT LTRIM(ITEMID)+'-'+TAGNO FROM " & cnStockDb & "..ISSUE WHERE SNO IN "
                        StrSql += " (SELECT ISSSNO FROM " & cnStockDb & "..ISSHALLMARK WHERE HM_BILLNO ='" & chkhmno.ToString & "') AND ISNULL(CANCEL,'')<>'Y' AND TRANTYPE='SA')) "
                        StrSql += " UNION ALL "
                        StrSql += " SELECT ISNULL(TAGSNO,'')TAGSNO FROM " & cnAdminDb & "..ITEMTAGHALLMARK WHERE  HM_BILLNO = '" & chkhmno.ToString & "' "
                        StrSql += " AND TAGSNO NOT IN (SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE LTRIM(ITEMID)+'-'+TAGNO IN "
                        StrSql += " (SELECT LTRIM(ITEMID)+'-'+TAGNO FROM " & cnStockDb & "..ISSUE WHERE SNO IN "
                        StrSql += " (SELECT ISSSNO FROM " & cnStockDb & "..ISSHALLMARK WHERE HM_BILLNO ='" & chkhmno.ToString & "') AND ISNULL(CANCEL,'')<>'Y' AND TRANTYPE='SA')) "
                        StrSql += " ) X"
                        Htagsno = GetSqlValue(cn, StrSql)
                        If Not Htagsno Is Nothing Then
                            Dim Htagrow As DataRow
                            Htagrow = Nothing
                            StrSql = " SELECT ISNULL((SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID=T.COSTID),'') COSTNAME "
                            StrSql += " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=T.ITEMID)ITEMNAME "
                            StrSql += " ,ITEMID,TAGNO,CONVERT(VARCHAR(12),RECDATE,103)RECDATE "
                            StrSql += " FROM " & cnAdminDb & "..ITEMTAG T WHERE "
                            StrSql += " SNO='" & Htagsno.ToString & "'"
                            Htagrow = GetSqlRow(StrSql, cn, Nothing)
                            If Not Htagrow Is Nothing Then
                                Dim tempstr As String = ""
                                tempstr = "HallmarkNo Already Exist" _
                        & IIf(Htagrow!Costname.ToString <> "", " Branch : " & Htagrow!Costname.ToString & ",", "") _
                        & " Itemname : " & Htagrow!ITEMNAME.ToString & ", Recdate : " & Htagrow!RECDATE.ToString _
                        & ", Itemid : " & Htagrow!ITEMID.ToString & ", Tagno : " & Htagrow!TAGNO.ToString
                                dtStrow("SUBITEM") = tempstr
                                dtStrow("REMARKS") = dtStrow("REMARKS") & "HUID,"
                            End If
                        End If
                        dtStrow("HM_WT") = Val(chkhmWT.ToString)
                        dtStrow("HM_BILLNO") = chkhmno.ToString
                        dtStrow("COLHEAD") = "H"
                        DtTran.Rows.Add(dtStrow)
                        DtTran.AcceptChanges()
                        v += 1
                    End If
                Next
                For k As Integer = 0 To DtTran.Rows.Count - 1
                    'DgvTran.Rows(k).Cells("MARK").ReadOnly = True
                    If DtTran.Rows(k).Item("COLHEAD").ToString = "G" Then
                        'Dim Itemid As Integer = Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & DtTran.Rows(k).Item("ITEM").ToString & "'", , 0, tran))
                        'If Val(objGPack.GetSqlValue("SELECT COUNT(*) FROM " & cnAdminDb & "..WITEMTAG WHERE TAGNO='" & DtTran.Rows(k).Item("STYLENO").ToString & "' AND ITEMID='" & Itemid & "'", , "", tran)) Then
                        '    DgvTran.Rows(k).Cells("MARK").ReadOnly = False
                        'End If
                        calcmaxval(k)
                        CalcToTStnamt(k)
                        CalcSaleValue(k)
                    End If
                Next
                validateHmWtdetails(DtTran)
                FormatedGridStyle(DgvTran)
                'For k As Integer = 0 To DgvTran.Rows.Count - 1
                '    DgvTran.Rows(k).Cells("MARK").ReadOnly = True
                '    If DgvTran.Rows(k).Cells("COLHEAD").Value.ToString = "G" Then
                '        Dim Itemid As Integer = Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & DgvTran.Rows(k).Cells("ITEM").Value.ToString & "'", , 0, tran))
                '        If Val(objGPack.GetSqlValue("SELECT COUNT(*) FROM " & cnAdminDb & "..WITEMTAG WHERE TAGNO='" & DgvTran.Rows(k).Cells("STYLENO").Value.ToString & "' AND ITEMID='" & Itemid & "'", , "", tran)) Then
                '            DgvTran.Rows(k).Cells("MARK").ReadOnly = False
                '            DgvTran.Rows(k).DefaultCellStyle.BackColor = Color.LightGreen
                '        End If
                '    End If
                'Next
            End If
            If Itemname <> "" Then MsgBox("This item Not Found: " & Mid(Itemname, 2, Len(Itemname)) & "", MsgBoxStyle.Information)
        Catch ex As Exception
            DtTran.Rows.Clear()
            DtTran.AcceptChanges()
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try

    End Sub
    Function validateHmWtdetails(ByVal dthm As DataTable) As Integer
        For Each drchkHm As DataRow In dthm.Select("COLHEAD='G'", Nothing)
            If Val(drchkHm("GRSWT").ToString) <> Val(dthm.Compute("SUM(HM_WT)", "SNO = '" & drchkHm("SNO") & "' AND COLHEAD='H' AND ISNULL(REMARKS,'') NOT LIKE '%HUID%'").ToString) Then
                For Each drchghm As DataRow In DtTran.Select("SNO = '" & drchkHm("SNO") & "' AND COLHEAD='H' AND ISNULL(REMARKS,'') NOT LIKE '%HUID%'", Nothing)
                    drchghm("SUBITEM") = "Weight Mismatch"
                    drchghm("REMARKS") = drchghm("REMARKS") & "HUID,"
                Next
            End If
        Next
    End Function
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
        Dim drstone() As DataRow = DtTran.Select("SNO='" & DtTran.Rows(index).Item("SNO").ToString.Trim & "' AND COLHEAD <>'G' and COLHEAD IS NOT NULL")
        Dim stnamt As Decimal = 0
        Dim sstnamt As Decimal = 0

        For k As Integer = 0 To drstone.Length - 1
            sstnamt += Val(drstone(k).Item("SSTNAMT").ToString)
            stnamt += Val(drstone(k).Item("STNAMT").ToString)
        Next
        If sstnamt = 0 Then sstnamt = stnamt
        DtTran.Rows(index).Item("SSTNAMT") = Format(IIf(sstnamt <> 0, Round(sstnamt), 0), "0.00")
        DtTran.Rows(index).Item("STNAMT") = Format(IIf(stnamt <> 0, Round(stnamt), 0), "0.00")

        DtTran.AcceptChanges()
    End Sub
    Private Sub CalcSaleValue(ByVal index As Integer)
        Dim amt As Double = Nothing
        Dim calType As String = objGPack.GetSqlValue(" SELECT CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & DtTran.Rows(index).Item("ITEM").ToString & "'")

        If calType = "R" Then
            amt = Val(DtTran.Rows(index).Item("PCS").ToString) * Val(DtTran.Rows(index).Item("RATE").ToString)
        ElseIf calType = "F" Then
            amt = Val(DtTran.Rows(index).Item("AMOUNT").ToString) + Val(DtTran.Rows(index).Item("STNAMT").ToString)
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
        If calType <> "F" Then
            amt += Val(DtTran.Rows(index).Item("SSTNAMT").ToString)
            DtTran.Rows(index).Item("SALVALUE") = Format(IIf(amt <> 0, SALEVALUE_ROUND(amt), 0), "0.00")
        End If
        If TAGXLUPLOAD_MAN_AUTO = "A" Then
            DtTran.Rows(index).Item("TOTAMT") = Format(SALEVALUE_ROUND(Val(DtTran.Rows(index).Item("AMOUNT").ToString) + Val(DtTran.Rows(index).Item("MC").ToString) + Val(DtTran.Rows(index).Item("STNAMT").ToString)), "0.00")
        ElseIf TAGXLUPLOAD_MAN_AUTO = "J" And calType = "F" Then
            DtTran.Rows(index).Item("TOTAMT") = Format(amt, "0.00")
        ElseIf TAGXLUPLOAD_MAN_AUTO = "J" Then
            DtTran.Rows(index).Item("TOTAMT") = Format(SALEVALUE_ROUND(Val(DtTran.Rows(index).Item("AMOUNT").ToString) + Val(DtTran.Rows(index).Item("MC").ToString) + Val(DtTran.Rows(index).Item("STNAMT").ToString) + Val(DtTran.Rows(index).Item("MAMOUNT").ToString)), "0.00")
        Else
            DtTran.Rows(index).Item("TOTAMT") = Format(SALEVALUE_ROUND(Val(DtTran.Rows(index).Item("AMOUNT").ToString) + Val(DtTran.Rows(index).Item("STNAMT").ToString)), "0.00")
        End If

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

    Private Sub calcStnrate(ByVal index As Integer, Optional ByVal StnRate As Decimal = 0)
        Dim rate As Double
        Dim Designer As Boolean = False
        StrSql = "SELECT STNITEMID,STNRATEPER,CERCHARGES FROM " & cnAdminDb & "..DESIGNERVA "
        StrSql += " WHERE DESIGNERID=" & designerid & " AND ISNULL(ACTIVE,'Y')<>'N'"
        Dim drRow As DataRow
        drRow = GetSqlRow(StrSql, cn, tran)
        If Not drRow Is Nothing Then
            Designer = True
            Dim drRowI As DataRow
            Dim drRowII() As DataRow
            Dim Sno As Integer
            Dim Item, stnSubItem, stnItem As String
            Sno = Val(DtTran.Rows(index).Item("SNO").ToString)
            drRowII = DtTran.Select("SNO='" & Sno & "' AND COLHEAD='G'", Nothing)
            If Not drRowII Is Nothing Then
                If drRowII.Length > 0 Then
                    Item = drRowII(0).Item("ITEM").ToString
                End If
            End If
            stnItem = DtTran.Rows(index).Item("ITEM").ToString
            stnSubItem = DtTran.Rows(index).Item("SUBITEM").ToString
            StrSql = "SELECT STNITEMID,STNRATEPER,CERCHARGES FROM " & cnAdminDb & "..DESIGNERVA "
            StrSql += " WHERE DESIGNERID = " & designerid
            StrSql += " AND ITEMID=(SELECT TOP 1 ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & Item & "')"
            StrSql += " AND STNITEMID=(SELECT TOP 1 ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & stnItem & "')"
            If stnSubItem <> "" Then
                StrSql += " AND STNSUBITEMID=(SELECT TOP 1 SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME='" & stnSubItem & "')"
            End If
            drRowI = GetSqlRow(StrSql, cn, tran)
            If Not drRowI Is Nothing Then
                Dim SRate As Decimal
                Dim Charges As String = "Y"
                Dim RatePer As Decimal
                Charges = drRowI("CERCHARGES").ToString
                RatePer = Val(drRowI("STNRATEPER").ToString)
                SRate = Val(DtTran.Rows(index).Item("RATE").ToString)
                If RatePer > 0 And SRate > 0 Then
                    rate = SRate + ((SRate * RatePer) / 100)
                    DtTran.Rows(index).Item("SRATE") = IIf(rate <> 0, Format(rate, "0.00"), "")
                    calcStnamt(index, rate)
                End If
                If Charges = "Y" Then CalcCerCharges(index)
                Exit Sub
            End If
        End If
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
        rate = Val(objGPack.GetSqlValue(StrSql, "MAXRATE", "", tran))
        If rate <> 0 Then
            DtTran.Rows(index).Item("SRATE") = IIf(rate <> 0, Format(rate, "0.00"), "")
        End If
        If rate > 0 Then calcStnamt(index, rate)
        If DtTran.Rows(index).Item("COLHEAD") = "S" And rate = 0 And TAGXLUPLOAD_MAN_AUTO <> "A" And STNRATE_DBLPURRATE Then
            rate = (StnRate * 2)
            If rate <> 0 Then
                DtTran.Rows(index).Item("SRATE") = IIf(rate <> 0, Format(rate, "0.00"), "")
            End If
            calcStnamt(index, rate)
        End If
        If Designer = False Then CalcCerCharges(index)
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

    Private Sub CalcCerCharges(ByVal index As Integer)
        Dim cent As Double = 0
        Dim CCharges As Double = 0
        If objGPack.GetSqlValue("SELECT ISNULL(OTHCHARGE,'') FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & DtTran.Rows(index).Item("ITEM").ToString & "'", , "", tran) <> "Y" Then Exit Sub
        Dim diaCaratWt As Decimal = 0
        Dim diaPcs As Integer = 0
        Dim dtCCharges As New DataTable
        Dim Platinum As Boolean = False
        Dim TMetalid As String = objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & DtTran.Rows(index).Item("ITEM").ToString & "'", , "", tran)
        If TMetalid = "P" Then Platinum = True
        StrSql = " DECLARE @WT FLOAT"
        StrSql += " SET @WT = " & Val(DtTran.Rows(index).Item("GRSWT").ToString) & ""
        StrSql += " SELECT PERCARATAMT,FLATAMT FROM " & cnAdminDb & "..CERCHARGES "
        StrSql += " WHERE METALID='" & TMetalid & "' AND @WT BETWEEN FROMWT AND TOWT "
        dtCCharges = New DataTable
        Cmd = New OleDbCommand(StrSql, cn, tran)
        Da = New OleDbDataAdapter(Cmd)
        Da.Fill(dtCCharges)
        If dtCCharges.Rows.Count > 0 Then
            If Val(dtCCharges.Rows(0).Item("FLATAMT").ToString) > 0 Then
                CCharges = CCharges + Val(dtCCharges.Rows(0).Item("FLATAMT").ToString)
            Else
                CCharges = Format((Val(dtCCharges.Rows(0).Item("PERCARATAMT").ToString) * Val(DtTran.Rows(index).Item("GRSWT").ToString)) + CCharges, "#0.00")
            End If
        End If
        If Platinum = True Then GoTo Cercharger
        If DtTran.Rows(index).Item("COLHEAD").ToString = "D" Then
            diaPcs += Val(DtTran.Rows(index).Item("PCS").ToString)
            diaCaratWt += Val(DtTran.Rows(index).Item("GRSWT").ToString)
            If diaPcs = 0 Then diaPcs = 1
            cent = diaCaratWt '(diaCaratWt / diaPcs)
            cent *= 100
            StrSql = " DECLARE @CENT FLOAT"
            StrSql += " SET @CENT = " & cent & ""
            StrSql += " SELECT PERCARATAMT,FLATAMT FROM " & cnAdminDb & "..CERCHARGES "
            StrSql += " WHERE METALID='" & TMetalid & "' AND @CENT BETWEEN FROMCENT AND TOCENT "
            dtCCharges = New DataTable
            Cmd = New OleDbCommand(StrSql, cn, tran)
            Da = New OleDbDataAdapter(Cmd)
            Da.Fill(dtCCharges)
            If dtCCharges.Rows.Count > 0 Then
                If Val(dtCCharges.Rows(0).Item("FLATAMT").ToString) > 0 Then
                    CCharges = CCharges + Val(dtCCharges.Rows(0).Item("FLATAMT").ToString)
                Else
                    CCharges = Format((Val(dtCCharges.Rows(0).Item("PERCARATAMT").ToString) * diaCaratWt) + CCharges, "#0.00")
                End If
            End If
        End If
Cercharger:
        DtTran.Rows(index).Item("CERCHARGE") = Format(IIf(CCharges <> 0, CCharges, 0), "0.00")
    End Sub

    Private Sub ChkPurchase_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkPurchase.CheckedChanged
        GridStyleVisible(DgvTran)
    End Sub

    Private Sub ChkSale_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkSale.CheckedChanged
        GridStyleVisible(DgvTran)
    End Sub

    Private Sub btntemplate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btntemplate.Click
        exceltemplate()
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

        oSheet.Range("C1").Value = "ITEM"
        oSheet.Range("E1").Value = "GOLD"
        oSheet.Range("N1").Value = "DIAMOND"
        oSheet.Range("V1").Value = "STONE"
        oSheet.Range("AJ1").Value = "HALLMARK"

        oSheet.Range("A2").Value = "SLNO"
        oSheet.Range("A2").ColumnWidth = 5
        oSheet.Range("B2").Value = "STYLENO"
        oSheet.Range("B2").ColumnWidth = 7
        oSheet.Range("C2").Value = "ITEM"
        oSheet.Range("C2").ColumnWidth = 15
        oSheet.Range("D2").Value = "SUBITEM"
        oSheet.Range("D2").ColumnWidth = 15
        oSheet.Range("E2").Value = "HEIGHT(MM)"
        oSheet.Range("E2").ColumnWidth = 11.57
        oSheet.Range("F2").Value = "WIDTH(MM)"
        oSheet.Range("F2").ColumnWidth = 11.57
        oSheet.Range("G2").Value = "PCS"
        oSheet.Range("G2").ColumnWidth = 3.6
        oSheet.Range("H2").Value = "GRSWT"
        oSheet.Range("H2").ColumnWidth = 6.6
        oSheet.Range("I2").Value = "NETWT"
        oSheet.Range("I2").ColumnWidth = 6.6
        oSheet.Range("J2").Value = "WASTAGE"
        oSheet.Range("J2").ColumnWidth = 9
        oSheet.Range("K2").Value = "MC"
        oSheet.Range("K2").ColumnWidth = 5.71
        oSheet.Range("L2").Value = "RATE"
        oSheet.Range("L2").ColumnWidth = 7.86

        oSheet.Range("M2").Value = "GVALUE"
        oSheet.Range("M2").ColumnWidth = 7.86
        oSheet.Range("N2").Value = "SHAPE"
        oSheet.Range("n2").ColumnWidth = 8.71
        oSheet.Range("O2").Value = "SETTINGTYPE"
        oSheet.Range("O2").ColumnWidth = 10.43
        oSheet.Range("P2").Value = "COLOR"
        oSheet.Range("P2").ColumnWidth = 5.71
        oSheet.Range("Q2").Value = "CLARITY"
        oSheet.Range("Q2").ColumnWidth = 7.29
        oSheet.Range("R2").Value = "PCS"
        oSheet.Range("R2").ColumnWidth = 2.91
        oSheet.Range("S2").Value = "WT"
        oSheet.Range("S2").ColumnWidth = 5.91
        oSheet.Range("T2").Value = "RATE"
        oSheet.Range("T2").ColumnWidth = 6.01
        oSheet.Range("U2").Value = "VALUE"
        oSheet.Range("U2").ColumnWidth = 7.86
        oSheet.Range("V2").Value = "PCS"
        oSheet.Range("V2").ColumnWidth = 2.91
        oSheet.Range("W2").Value = "WT"
        oSheet.Range("W2").ColumnWidth = 5.86
        oSheet.Range("X2").Value = "RATE"
        oSheet.Range("X2").ColumnWidth = 6.01
        oSheet.Range("Y2").Value = "VALUE"
        oSheet.Range("Y2").ColumnWidth = 7.86
        oSheet.Range("Z2").Value = "TOTAL"
        oSheet.Range("Z2").ColumnWidth = 7.86
        oSheet.Range("AA2").Value = "METALTYPE"
        oSheet.Range("AA2").ColumnWidth = 14
        oSheet.Range("AB2").Value = "NARRATION"
        oSheet.Range("AB2").ColumnWidth = 14
        oSheet.Range("AC2").Value = "MAXWASTPER"
        oSheet.Range("AC2").ColumnWidth = 13
        oSheet.Range("AD2").Value = "MAXMCGRM"
        oSheet.Range("AD2").ColumnWidth = 11
        oSheet.Range("AE2").Value = "TOUCH"
        oSheet.Range("AE2").ColumnWidth = 6.5
        oSheet.Range("AF2").Value = "TAGTYPE"
        oSheet.Range("AF2").ColumnWidth = 8
        oSheet.Range("AG2").Value = "GRSNET"
        oSheet.Range("AG2").ColumnWidth = 8
        oSheet.Range("AH2").Value = "TAGNO"
        oSheet.Range("AH2").ColumnWidth = 8
        oSheet.Range("AI2").Value = "SIZE"
        oSheet.Range("AI2").ColumnWidth = 8
        oSheet.Range("AJ2").Value = "HM_WT"
        oSheet.Range("AJ2").ColumnWidth = 8
        oSheet.Range("AK2").Value = "HM_BILLNO"
        oSheet.Range("AK2").ColumnWidth = 8

        oSheet.Range("C1:G1:N1:V1").Font.Bold = True
        oSheet.Range("C1:G1:N1:V1").Font.Name = "VERDANA"
        oSheet.Range("C1:G1:N1:V1").Font.Size = 8
        oSheet.Range("C1:D1").Merge()
        oSheet.Range("C1:D1").HorizontalAlignment = Excel.Constants.xlCenter
        oSheet.Range("E1:F1:G1:H1:I1:J1:K1:L1:M1").Merge()
        oSheet.Range("E1:F1:G1:H1:I1:J1:K1:L1:M1").HorizontalAlignment = Excel.Constants.xlCenter
        oSheet.Range("N1:O1:P1:Q1:R1:S1:T1:U1").Merge()
        oSheet.Range("N1:O1:P1:Q1:R1:S1:T1:U1").HorizontalAlignment = Excel.Constants.xlCenter
        oSheet.Range("V1:W1:X1:Y1").Merge()
        oSheet.Range("V1:W1:X1:Y1").HorizontalAlignment = Excel.Constants.xlCenter
        oSheet.Range("AJ1:AK1").Merge()
        oSheet.Range("AJ1:AK1").HorizontalAlignment = Excel.Constants.xlCenter
        oSheet.Range("A2:B2:C2:D2:E2:F2:G2:H2:I2:J2:K2:L2:M2:N2:O2:P2:Q2:R2:S2:T2:U2:V2:W2:X2:Y2:Z2:AA2:AB2:AC2:AD2:AE2:AF2:AG2:AH2:AI2:AJ2:AK2").Font.Bold = False
        oSheet.Range("A2:B2:C2:D2:E2:F2:G2:H2:I2:J2:K2:L2:M2:N2:O2:P2:Q2:R2:S2:T2:U2:V2:W2:X2:Y2:Z2:AA2:AB2:AC2:AD2:AE2:AF2:AG2:AH2:AI2:AJ2:AK2").Font.Name = "VERDANA"
        oSheet.Range("A2:B2:C2:D2:E2:F2:G2:H2:I2:J2:K2:L2:M2:N2:O2:P2:Q2:R2:S2:T2:U2:V2:W2:X2:Y2:Z2:AA2:AB2:AC2:AD2:AE2:AF2:AG2:AH2:AI2:AJ2:AK2").Font.Size = 8

    End Function
End Class
