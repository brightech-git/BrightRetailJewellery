Imports System.Data.OleDb
Public Class frmSingleAccountsEnttry
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
    Dim DrCap As String = "Dr"
    Dim CrCap As String = "Cr"
    Dim Approval As Boolean = False
    Dim VouchFilteration As String
    Dim chkval As Double
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
    Dim objSearch As New SingleAccountsEntryFilter


    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        Initialize()
        Dim dtCol As New DataColumn("KEYNO", GetType(Integer))
        dtCol.AutoIncrement = True
        dtCol.AutoIncrementSeed = 1
        dtCol.AutoIncrementStep = 1
        If Not dtGridView.Columns.Contains("KEYNO") Then dtGridView.Columns.Add(dtCol)
        ' Add any initialization after the InitializeComponent() call.
        Me.vouchType = VoucherType.Journal
        lblTitle.Text = "SINGLE ENTRY (JOURNAL)"
        Me.ctrlId = "GEN-JE"
        SetDrCrCaption()
    End Sub

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
        SetDrCrCaption()
    End Sub

    Private Sub SetDrCrCaption()
        DrCap = "Payment" 'objGPack.GetSqlValue("SELECT DRCAPTION FROM " & cnAdminDb & "..ACCENTRYMASTER WHERE TYPE = '" & typ & "' AND ISNULL(DRCAPTION,'') <> ''", , DrCap)
        CrCap = "Receipt" 'objGPack.GetSqlValue("SELECT CRCAPTION FROM " & cnAdminDb & "..ACCENTRYMASTER WHERE TYPE = '" & typ & "' AND ISNULL(CRCAPTION,'') <> ''", , CrCap)
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

        'strSql = " SELECT TYPE,DISPLAYTEXT,SNO  FROM " & cnAdminDb & "..ACCENTRYMASTER WHERE PAYMODE = '" & payMode & "'"
        'Dim dritem As DataRow = GetSqlRow(strSql, cn)
        Dim type As String = "J" ' dritem.Item(0)
        Dim dispText As String = "JOURNAL ENTRY" 'dritem.Item(1)
        'mnuId = dritem.Item(2)

        ctrlId = "GEN-" & payMode
        strSql = " SELECT 'CHECK' FROM " & cnStockDb & "..BILLCONTROL"
        strSql += " WHERE CTLID = '" & ctrlId & "'"
        strSql += " AND COMPANYID = '" & strCompanyId & "'"
        If Not objGPack.GetSqlValue(strSql).Length > 0 Then
            MsgBox(dispText & " Billno Generation Controlid Not Found", MsgBoxStyle.Information)
        End If
        lblTitle.Text = dispText
        Me.vouchType = VoucherType.Journal
        SetDrCrCaption()
        editFlag = True
        btnNew_Click(Me, New EventArgs)
        btnNew.Enabled = False
        editDt = dtView
    End Sub

    Private Sub LoadAcc()
        Dim ftr As String = Nothing
        Select Case vouchType
            Case VoucherType.Receipt
                If UCase(gridView_OWN.CurrentRow.Cells("type").Value.ToString) = CrCap.ToUpper Then
                Else
                    ftr = "actype in ('B','H')"
                End If
            Case VoucherType.Payment
                If UCase(gridView_OWN.CurrentRow.Cells("type").Value.ToString) = DrCap.ToUpper Then
                Else
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

    Private Sub Initialize()
        strSql = " SELECT ''TYPE,''DESCRIPTION,''BALANCE,''NARRATION1,''NARRATION2,''SCOSTCENTRE,''CHQNO"
        strSql += " ,CONVERT(SMALLDATETIME,NULL)CHQDATE,''CHQDETAIL"
        strSql += " ,CONVERT(NUMERIC(15,2),NULL)TDSPER,CONVERT(NUMERIC(15,2),NULL)TDSAMT,CONVERT(INT,NULL)TDSCATID"
        strSql += " ,CONVERT(NUMERIC(15,2),NULL)SRVTPER,CONVERT(NUMERIC(15,2),NULL)SRVTAMT"
        strSql += " ,CONVERT(NUMERIC(15,2),NULL)VATPER,CONVERT(NUMERIC(15,2),NULL)VATAMT,CONVERT(VARCHAR(10),NULL)VATCATID"
        strSql += " ,CONVERT(NUMERIC(15,2),NULL)DEBIT,CONVERT(NUMERIC(15,2),NULL)CREDIT"
        strSql += " ,CONVERT(VARCHAR,NULL)GENBY WHERE 1<>1"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGridView)

        strSql = " SELECT " & cnAdminDb & ".DBO.WORDCAPS(ACNAME)ACNAME,ACGRPCODE,ACTYPE FROM " & cnAdminDb & "..ACHEAD WHERE 1=1"
        strSql += " AND ISNULL(MACCODE,'') = ''"
        strSql += GetAcNameQryFilteration()
        strSql += " ORDER BY ACNAME"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtAccNames)
        DgvSearch.DataSource = dtAccNames
        DgvSearch.Columns("ACGRPCODE").Visible = False
        DgvSearch.Columns("ACTYPE").Visible = False
        DgvSearch.ColumnHeadersVisible = False
        DgvSearch.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DgvSearch.Visible = False

        If UCase(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COSTCENTRE'")) = "Y" Then
            strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            objGPack.FillCombo(strSql, cmbCostCenter_MAN, False, False)
            cmbCostCenter_MAN.Enabled = True
        Else
            cmbCostCenter_MAN.Enabled = False
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

    Private Sub frmSingleAccountsEnttry_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            gridView_OWN.Focus()
        End If
    End Sub

    Private Sub frmSingleAccountsEnttry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtGrid_OWN.Focused Then Exit Sub
            If DgvSearch.Focused Then Exit Sub
            If gridView_OWN.Focused Then Exit Sub
            If txtNarration1.Focused Then Exit Sub
            If txtNarration2.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    '    Private Sub ShowWeightDetail()
    'Loadwt:
    '        If objGPack.GetSqlValue("SELECT INVENTORY FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & txtGrid_OWN.Text & "'") <> "Y" Then
    '            Exit Sub
    '        End If
    '        Dim updFlag As Boolean = False
    '        Dim ro() As DataRow
    '        ro = dtWeightDetail.Select("KEYNO = " & Val(gridView_OWN.CurrentRow.Cells("KEYNO").Value.ToString) & "")
    '        Dim f As New AccEntryWeightDetail(gridView_OWN.CurrentRow.Cells("TYPE").Value.ToString)
    '        If ro.Length > 0 Then
    '            f.cmbWCategory_MAN.Text = ro(0).Item("CATNAME").ToString
    '            f.txtWPcs_NUM.Text = IIf(Val(ro(0).Item("PCS").ToString) <> 0, Val(ro(0).Item("PCS").ToString), "")
    '            f.txtWGrsWt_WET.Text = IIf(Val(ro(0).Item("GRSWT").ToString) <> 0, Format(Val(ro(0).Item("GRSWT").ToString), "0.000"), "")
    '            f.txtWNetWt_WET.Text = IIf(Val(ro(0).Item("NETWT").ToString) <> 0, Format(Val(ro(0).Item("NETWT").ToString), "0.000"), "")
    '            f.txtPureWt_WET.Text = IIf(Val(ro(0).Item("PUREWT").ToString) <> 0, Format(Val(ro(0).Item("PUREWT").ToString), "0.000"), "")
    '            f.cmbWCalcMode.Text = ro(0).Item("CALCMODE").ToString
    '            f.txtWRate_AMT.Text = IIf(Val(ro(0).Item("RATE").ToString) <> 0, Format(Val(ro(0).Item("RATE").ToString), "0.00"), "")
    '            f.cmbWType.Text = ro(0).Item("TYPE").ToString
    '            f.dtpRefDate.Value = ro(0).Item("REFDATE").ToString
    '            f.txtRefno.Text = ro(0).Item("REFNO").ToString
    '            f.txtWamt_AMT.Text = ro(0).Item("AMOUNT").ToString
    '            f.txtVat_AMT.Text = ro(0).Item("VAT").ToString
    '            updFlag = True
    '        End If
    '        If f.ShowDialog() = Windows.Forms.DialogResult.OK Then
    '            If updFlag Then
    '                For cnt As Integer = 0 To dtWeightDetail.Rows.Count - 1
    '                    With dtWeightDetail.Rows(cnt)
    '                        If dtWeightDetail.Rows(cnt).Item("KEYNO").ToString <> gridView_OWN.CurrentRow.Cells("KEYNO").Value.ToString Then Continue For
    '                        .Item("CATNAME") = f.cmbWCategory_MAN.Text
    '                        .Item("PCS") = Val(f.txtWPcs_NUM.Text)
    '                        .Item("GRSWT") = Val(f.txtWGrsWt_WET.Text)
    '                        .Item("NETWT") = Val(f.txtWNetWt_WET.Text)
    '                        .Item("PUREWT") = Val(f.txtPureWt_WET.Text)
    '                        .Item("CALCMODE") = f.cmbWCalcMode.Text
    '                        .Item("RATE") = Val(f.txtWRate_AMT.Text)
    '                        .Item("TYPE") = f.cmbWType.Text
    '                        .Item("REFDATE") = f.dtpRefDate.Value
    '                        .Item("REFNO") = f.txtRefno.Text
    '                        .Item("AMOUNT") = f.txtWamt_AMT.Text
    '                        .Item("VAT") = f.txtVat_AMT.Text
    '                        Exit For
    '                    End With
    '                Next
    '            Else
    '                Dim row As DataRow = Nothing
    '                row = dtWeightDetail.NewRow
    '                row("KEYNO") = Val(gridView_OWN.CurrentRow.Cells("KEYNO").Value.ToString)
    '                row("CATNAME") = f.cmbWCategory_MAN.Text
    '                row("PCS") = Val(f.txtWPcs_NUM.Text)
    '                row("GRSWT") = Val(f.txtWGrsWt_WET.Text)
    '                row("NETWT") = Val(f.txtWNetWt_WET.Text)
    '                row("PUREWT") = Val(f.txtPureWt_WET.Text)
    '                row("CALCMODE") = f.cmbWCalcMode.Text
    '                row("RATE") = Val(f.txtWRate_AMT.Text)
    '                row("TYPE") = f.cmbWType.Text
    '                row("REFDATE") = f.dtpRefDate.Value
    '                row("REFNO") = f.txtRefno.Text
    '                row("VAT") = Val(f.txtVat_AMT.Text & "")
    '                row("AMOUNT") = Val(f.txtWamt_AMT.Text)
    '                dtWeightDetail.Rows.Add(row)
    '            End If
    '            dtWeightDetail.AcceptChanges()

    '            'wt details loading

    '            gridView_OWN.CurrentRow.Cells("VATPER").Value = Val(f.txtVat_per.Text)
    '            gridView_OWN.CurrentRow.Cells("VATAMT").Value = Val(f.txtVat_AMT.Text)

    '            strSql = "SELECT " & IIf(f.cmbWType.Text = "SALES", "STAXID", "PTAXID") & " FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & f.cmbWCategory_MAN.Text & "'"
    '            gridView_OWN.CurrentRow.Cells("VATCATID").Value = objGPack.GetSqlValue(strSql)

    '            If vouchType = VoucherType.Journal Then
    '                If gridView_OWN.CurrentRow.Cells("TYPE").Value.ToString.ToUpper = DrCap.ToUpper Then '"DR"

    '                    gridView_OWN.CurrentRow.Cells("DEBIT").Value = Format(Val(f.txtWamt_AMT.Text), "0.00")

    '                ElseIf gridView_OWN.CurrentRow.Cells("TYPE").Value.ToString.ToUpper = CrCap.ToUpper Then '"CR"

    '                    gridView_OWN.CurrentRow.Cells("CREDIT").Value = Format(Val(f.txtWamt_AMT.Text), "0.00")


    '                End If
    '            Else
    '                If gridView_OWN.CurrentRow.Cells("TYPE").Value.ToString.ToUpper = DrCap.ToUpper Then '"DR"
    '                    gridView_OWN.CurrentRow.Cells("DEBIT").Value = Format(Val(f.txtWamt_AMT.Text), "0.00")
    '                ElseIf gridView_OWN.CurrentRow.Cells("TYPE").Value.ToString.ToUpper = CrCap.ToUpper Then '"CR"
    '                    gridView_OWN.CurrentRow.Cells("CREDIT").Value = Format(Val(f.txtWamt_AMT.Text), "0.00")
    '                End If
    '            End If

    '        End If
    '    End Sub

    Private Sub frmSingleAccountsEnttry_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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
        If Not editFlag Then dtpDate.Value = GetEntryDate(GetServerDate)
        If Not editFlag Then btnNew_Click(Me, New EventArgs)
        If editFlag Then
            dtGridView = editDt
            dtGridView.Columns("KEYNO").AutoIncrement = True
            dtGridView.AcceptChanges()
            gridView_OWN.DataSource = dtGridView
        End If
        strSql = "SELECT NARRATION FROM " & cnAdminDb & "..NARRATION WHERE MODULEID='F'"
        'objGPack.FillCombo(strSql, cmbNarration, , False)
        narration = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(narration)
        'BrighttechPack.GlobalMethods.FillCombo(chklstAcname, dtAccNames, "ACNAME", , "ALL")
        If narration.Rows.Count > 0 Then
            Dgv1Search.DataSource = narration
            Dgv1Search.ColumnHeadersVisible = False
            Dgv1Search.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        End If
        Dgv1Search.Visible = False
        pnlContainer_OWN.BorderStyle = BorderStyle.Fixed3D
        If dtpDate.Text = "" Then
            dtpDate.Value = GetEntryDate(GetServerDate)
        End If
        DgvSearch.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DgvSearch.BackgroundColor = Color.Lavender
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
                lblbud.Text = " Insert Key to help"
                lblbud.Visible = True
                LoadAcc()
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
                lblbud.Visible = False
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
                    If gridView_OWN.Columns(gridView_OWN.CurrentCell.ColumnIndex).Name = "DESCRIPTION" Then
                        objSearch = New SingleAccountsEntryFilter
                        objSearch.StartPosition = FormStartPosition.CenterScreen
                        If objSearch.ShowDialog() = Windows.Forms.DialogResult.OK Then
                            txtGrid_OWN.Text = objSearch.ReturnName
                        Else
                            txtGrid_OWN.Text = ""
                        End If

                        'strSql = " SELECT ACNAME AS NAME,CITY,PHONENO,MOBILE FROM " & cnAdminDb & "..ACHEAD ORDER BY ACNAME"
                        'strSql = " SELECT " & cnAdminDb & ".DBO.WORDCAPS(ACNAME) AS NAME,CITY,PHONENO,MOBILE FROM " & cnAdminDb & "..ACHEAD WHERE 1=1"
                        'strSql += " AND ISNULL(MACCODE,'') = ''"
                        'strSql += GetAcNameQryFilteration()
                        'strSql += " ORDER BY ACNAME"

                        'strSql = "  SELECT UPPER(ACNAME) AS NAME,CITY,PHONENO,MOBILE"
                        'strSql += vbCrLf + " ,CONVERT(VARCHAR(10),SUM(CASE WHEN RECPAY ='R' THEN O.AMOUNT ELSE (-1)*O.AMOUNT END)) AMOUNT,RUNNO FROM " & cnAdminDb & "..ACHEAD A"
                        'strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..OUTSTANDING O ON A.ACCODE =O.ACCODE  "
                        'strSql += vbCrLf + " WHERE 1=1 AND ISNULL(MACCODE,'') = '' AND ISNULL(O.CANCEL,'')=''"
                        'strSql += vbCrLf + " AND ISNULL(ACTIVE,'Y') NOT IN ('N','H')"
                        'strSql += vbCrLf + " AND (ISNULL(A.COMPANYID,'') = '' OR ISNULL(A.COMPANYID,'') LIKE '%" & strCompanyId & "%')"
                        'strSql += vbCrLf + " GROUP BY ACNAME,CITY,PHONENO,MOBILE,RUNNO"
                        'strSql += vbCrLf + " ORDER BY ACNAME"
                        'txtGrid_OWN.Text = BrighttechPack.SearchDialog.Show("Select Name", strSql, cn, 0, 0, , , , , , , , )

                    End If
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
                    If gridView_OWN.Columns(gridView_OWN.CurrentCell.ColumnIndex).Name = "DESCRIPTION" And txtGrid_OWN.Text.Trim = "" Then
                        objSearch = New SingleAccountsEntryFilter
                        objSearch.StartPosition = FormStartPosition.CenterScreen
                        If objSearch.ShowDialog() = Windows.Forms.DialogResult.OK Then
                            txtGrid_OWN.Text = objSearch.ReturnName
                        Else
                            txtGrid_OWN.Text = ""
                        End If
                    End If
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
                    If txtGrid_OWN.Text = "" Then
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
                    Dim bcostid As String
                    If cmbCostCenter_MAN.Text <> "" Then
                        bcostid = objGPack.GetSqlValue("SELECT  COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCenter_MAN.Text & "'")
                    Else
                        bcostid = ""
                    End If
                    Dim mPartycode As String = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME='" & txtGrid_OWN.Text.ToString() & "'")
                    If IsdispBalance Then
                        .CurrentRow.Cells("BALANCE").Value = GetPartyBalanceNew(mPartycode, txtGrid_OWN.Text.ToString())
                    Else
                        .CurrentRow.Cells("BALANCE").Value = ""
                    End If
skip:
                    If gridView_OWN.CurrentRow.Cells("NARRATION1").Value.ToString <> "" Then
                        txtNarration1.Text = gridView_OWN.CurrentRow.Cells("NARRATION1").Value.ToString
                    End If
                    If gridView_OWN.CurrentRow.Cells("NARRATION2").Value.ToString <> "" Then
                        txtNarration2.Text = gridView_OWN.CurrentRow.Cells("NARRATION2").Value.ToString
                    End If
                    'If ShowTds() Then
                    '    NextAtDescription()
                    '    Exit Sub
                    'End If

                    If objGPack.GetSqlValue("SELECT ACTYPE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & txtGrid_OWN.Text & "'") = "B" Then
                        objBankDetail.DefDate = dtpDate.Value
                        If Me.vouchType = VoucherType.Payment Then
                            objBankDetail.MANDATORY = True
                        End If
                        objBankDetail.ClearBankDetails()
                        If .CurrentRow.Cells("TYPE").Value.ToString.ToUpper = CrCap.ToUpper Then
                            Dim chqnumber As Integer = Nothing
                            Dim accode As String = ""
                            accode = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & txtGrid_OWN.Text.ToString & "'", "ACCODE", Nothing)
                            chqnumber = Val(objGPack.GetSqlValue("SELECT TOP 1 CHQNUMBER FROM " & cnStockDb & "..CHEQUEBOOK WHERE " & IIf(bcostid <> "", "COSTID='" & bcostid & "' AND", "") & " BANKCODE='" & accode & "' AND CHQISSUEDATE IS NULL order by chqnumber", "CHQNUMBER", "0"))
                            objBankDetail.txtChqNo.Text = chqnumber
                        Else
                            objBankDetail.txtChqNo.Text = gridView_OWN.CurrentRow.Cells("CHQNO").Value.ToString
                        End If

                        objBankDetail.CmbChqDetail_OWN.Text = gridView_OWN.CurrentRow.Cells("CHQDETAIL").Value.ToString
                        If gridView_OWN.CurrentRow.Cells("CHQDATE").Value.ToString <> "" Then
                            objBankDetail.dtpBankDate.Value = gridView_OWN.CurrentRow.Cells("CHQDATE").Value
                        End If
                        If Edchqno <> Nothing Then objBankDetail.txtChqNo.Text = Edchqno
                        If Edchqdate <> Nothing Then objBankDetail.dtpBankDate.Text = Format(Edchqdate, "dd/MM/yyyy")
                        If Edchqdetail <> Nothing Then objBankDetail.CmbChqDetail_OWN.Text = Edchqdetail
                        e.Handled = True
                        objBankDetail.txtChqNo.Select()
                        objBankDetail.ShowDialog()
                        gridView_OWN.CurrentRow.Cells("CHQNO").Value = objBankDetail.txtChqNo.Text
                        gridView_OWN.CurrentRow.Cells("CHQDATE").Value = objBankDetail.dtpBankDate.Value.Date
                        gridView_OWN.CurrentRow.Cells("CHQDETAIL").Value = objBankDetail.CmbChqDetail_OWN.Text
                    End If
                    NextAtDescription()
                Case "DEBIT"
                    If CheckDebit() Then Exit Sub
                    ShowOutStDt()
                    If ChkChqTranLimit(objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & gridView_OWN.CurrentRow.Cells("DESCRIPTION").Value.ToString & "' AND ACTYPE = 'B'"), Val(txtGrid_OWN.Text)) Then Exit Sub
                    If .CurrentRow.Cells("TYPE").Value.ToString.ToUpper = DrCap.ToUpper Then '"DR"
                        txtNarration1.Select()
                    ElseIf .CurrentRow.Cells("TYPE").Value.ToString.ToUpper = CrCap.ToUpper Then '"CR"
                        Dim balAmt As Double = Math.Abs(GetGridViewTotal)
                        .CurrentRow.Cells("CREDIT").Value = IIf(balAmt <> 0, Format(balAmt, "0.00"), DBNull.Value)
                        .CurrentCell = .CurrentRow.Cells("CREDIT")
                    End If
                Case "CREDIT"
                    If CheckCredit() Then Exit Sub
                    ShowOutStDt()
                    If ChkChqTranLimit(objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & gridView_OWN.CurrentRow.Cells("DESCRIPTION").Value.ToString & "' AND ACTYPE = 'B'"), Val(txtGrid_OWN.Text)) Then Exit Sub
                    txtNarration1.Select()

            End Select
        End With
    End Sub
    Private Sub ShowInvno()
        objInvNo.Text = ""
        objInvNo.ShowDialog()
    End Sub

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

        'Dim balAmt As Double = GetGridViewTotal()
        'If balAmt = 0 Then
        gridView_CellEnter(Me, New DataGridViewCellEventArgs(0, gridView_OWN.CurrentRow.Index))
        txtGrid_OWN.SelectAll()
        Exit Sub
        'Else
        '    If Not balAmt > 0 Then
        '        dtGridView.Rows(lastInd).Item("TYPE") = DrCap '"Dr"
        '    Else
        '        dtGridView.Rows(lastInd).Item("TYPE") = CrCap ' "Cr"
        '    End If
        'End If
        searchDia = False
        gridView_CellEnter(Me, New DataGridViewCellEventArgs(0, lastInd))
        txtGrid_OWN.SelectAll()
        searchDia = False
    End Sub

    Private Sub NextAtDescription()
        With gridView_OWN
            If .CurrentRow.Cells("TYPE").Value.ToString.ToUpper = DrCap.ToUpper Then
                Dim balAmt As Double = Math.Abs(GetGridViewTotal)
                'If Val(.CurrentRow.Cells("DEBIT").Value.ToString) = 0 Then
                '    .CurrentRow.Cells("DEBIT").Value = IIf(balAmt <> 0, Format(balAmt, "0.00"), DBNull.Value)
                'End If
                .CurrentCell = .CurrentRow.Cells("DEBIT")
            ElseIf .CurrentRow.Cells("TYPE").Value.ToString.ToUpper = CrCap.ToUpper Then '"CR"
                Dim balAmt As Double = Math.Abs(GetGridViewTotal)
                'If Val(.CurrentRow.Cells("CREDIT").Value.ToString) = 0 Then
                '    .CurrentRow.Cells("CREDIT").Value = IIf(balAmt <> 0, Format(balAmt, "0.00"), DBNull.Value)
                'End If
                .CurrentCell = .CurrentRow.Cells("CREDIT")
            End If
            gridView_OWN.Focus()
        End With
    End Sub

    Private Function GetPartyBalanceNew(ByVal Partycode As String, ByVal Partyname As String) As String
        Dim retStr As String = Nothing
        Dim costid As String
        If cmbCostCenter_MAN.Text <> "" Then
            costid = objGPack.GetSqlValue("SELECT  COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCenter_MAN.Text & "'")
        Else
            costid = ""
        End If
        strSql = " SELECT "
        strSql += " ISNULL(SUM(DEBIT),0) - ISNULL(SUM(CREDIT),0) BALANCE"
        strSql += " FROM"
        strSql += " ("
        strSql += " SELECT DEBIT,CREDIT FROM  " & cnStockDb & "..OPENTRAILBALANCE"
        strSql += " WHERE ACCODE = '" & Partycode & "'"
        strSql += " AND COMPANYID = '" & strCompanyId & "'"
        If OS_ADJ_COSTCENTRE Then strSql += " AND COSTID = '" & costid & "'"
        strSql += " UNION ALL"
        strSql += " SELECT "
        strSql += " ISNULL(SUM(CASE WHEN TRANMODE = 'D' THEN ISNULL(AMOUNT,0) ELSE 0 END),0) AS DEBIT"
        strSql += " ,ISNULL(SUM(CASE WHEN TRANMODE = 'C' THEN ISNULL(AMOUNT,0) ELSE 0 END),0) AS CREDIT"
        strSql += " FROM  " & cnStockDb & "..ACCTRAN"
        strSql += " WHERE ACCODE = '" & Partycode & "' AND TRANDATE <= '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' "
        strSql += " AND COMPANYID = '" & strCompanyId & "'  AND ISNULL(CANCEL,'') = ''"
        If OS_ADJ_COSTCENTRE Then strSql += " AND isnull(COSTID,'') = '" & costid & "'"
        strSql += " UNION ALL"
        strSql += " SELECT "
        strSql += " ISNULL(SUM(CASE WHEN TRANMODE = 'D' THEN ISNULL(AMOUNT,0) ELSE 0 END),0) AS DEBIT"
        strSql += " ,ISNULL(SUM(CASE WHEN TRANMODE = 'C' THEN ISNULL(AMOUNT,0) ELSE 0 END),0) AS CREDIT"
        strSql += " FROM  " & cnStockDb & "..TACCTRAN"
        strSql += " WHERE ACCODE = '" & Partycode & "' AND TRANDATE <= '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' "
        strSql += " AND COMPANYID = '" & strCompanyId & "'  AND ISNULL(CANCEL,'') = ''"
        If OS_ADJ_COSTCENTRE Then strSql += " AND COSTID = '" & costid & "'"
        strSql += " )X"
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
        strSql += "     ISNULL(SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1 * AMOUNT END),0) BALANCE"
        strSql += " FROM  " & cnStockDb & "..TACCTRAN"
        strSql += " WHERE ACCODE = '" & Partycode & "' AND TRANDATE <= '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' "
        strSql += " AND COMPANYID = '" & strCompanyId & "'  AND ISNULL(CANCEL,'') = ''"
        If OS_ADJ_COSTCENTRE Then strSql += " AND COSTID = '" & costid & "'"
        strSql += " HAVING ISNULL(SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1 * AMOUNT END),0) <> 0"
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

    '    Private Function ShowTds() As Boolean
    '        If vouchType <> VoucherType.Journal And vouchType <> VoucherType.Payment Then Exit Function
    'LoadTds:
    '        If objGPack.GetSqlValue("SELECT ISNULL(TDSFLAG,'') FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & txtGrid_OWN.Text & "'").ToUpper = "Y" Then
    '            objTds.Accode = objGPack.GetSqlValue("SELECT ISNULL(ACCODE,'') FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & txtGrid_OWN.Text & "'")
    '            objTds.txtActualAmount_AMT.Text = IIf(Val(gridView_OWN.CurrentRow.Cells("DEBIT").Value.ToString) <> 0, Val(gridView_OWN.CurrentRow.Cells("DEBIT").Value.ToString), Val(gridView_OWN.CurrentRow.Cells("CREDIT").Value.ToString))
    '            objTds.Accode = objGPack.GetSqlValue("SELECT ISNULL(ACCODE,'') FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & txtGrid_OWN.Text & "'")
    '            Dim tdsAmt As Double = Val(gridView_OWN.CurrentRow.Cells("TDSAMT").Value.ToString)
    '            Dim srvtAmt As Double = Val(gridView_OWN.CurrentRow.Cells("SRVTAMT").Value.ToString)
    '            If vouchType = VoucherType.Journal Then
    '                objTds.txtActualAmount_AMT.Text = objTds.Amount + srvtAmt + tdsAmt
    '            End If
    '            Dim tdsPer As Double = Val(gridView_OWN.CurrentRow.Cells("TDSPER").Value.ToString)
    '            objTds.txtTdsPer_PER.Text = IIf(tdsPer <> 0, Format(tdsPer, "0.00"), Nothing)
    '            If tdsAmt <> 0 Then
    '                objTds.EditFlag = True
    '            Else
    '                objTds.EditFlag = False
    '            End If
    '            If SRVTAX = True Then
    '                objTds.txtServTax_PER.Text = IIf(SRVTPER <> 0, Format(SRVTPER, "0.00"), Nothing)
    '            End If
    '            objTds.SRVTCODE = SrvTaxCode
    '            objTds.txtTdsAmt_AMT.Text = IIf(tdsAmt <> 0, Format(tdsAmt, "0.00"), Nothing)
    '            Dim tdsCategory As Integer = Val(gridView_OWN.CurrentRow.Cells("TDSCATID").Value.ToString)
    '            If tdsCategory <> 0 Then
    '                objTds.cmbTdsCategory_OWN.SelectedValue = tdsCategory.ToString
    '            Else
    '                'Dim TtdsCategory As String = objGPack.GetSqlValue("SELECT TDSCATNAME FROM " & cnAdminDb & "..TDSCATEGORY WHERE TDSCATID IN(SELECT ISNULL(TDSCATID,0) FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & txtGrid_OWN.Text & "')", , , )
    '                Dim TtdsCategory As Integer = objGPack.GetSqlValue("SELECT ISNULL(TDSCATID,0) FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & txtGrid_OWN.Text & "'", , , )
    '                If TtdsCategory <> 0 Then
    '                    objTds.cmbTdsCategory_OWN.SelectedValue = TtdsCategory.ToString
    '                Else
    '                    objTds.cmbTdsCategory_OWN.SelectedText = ""
    '                End If
    '            End If
    '            objTds.txtActualAmount_AMT.Select()
    '            objTds.ShowDialog()
    '            If SRVTAX Then
    '                gridView_OWN.CurrentRow.Cells("SRVTPER").Value = Val(objTds.txtServTax_PER.Text)
    '                gridView_OWN.CurrentRow.Cells("SRVTAMT").Value = Val(objTds.txtServTax_Amt.Text)
    '            End If

    '            gridView_OWN.CurrentRow.Cells("TDSPER").Value = Val(objTds.txtTdsPer_PER.Text)
    '            gridView_OWN.CurrentRow.Cells("TDSAMT").Value = Val(objTds.txtTdsAmt_AMT.Text)
    '            gridView_OWN.CurrentRow.Cells("TDSCATID").Value = Val(objTds.cmbTdsCategory_OWN.SelectedValue.ToString)
    '            If vouchType = VoucherType.Journal Then
    '                If gridView_OWN.CurrentRow.Cells("TYPE").Value.ToString.ToUpper = DrCap.ToUpper Then '"DR"
    '                    gridView_OWN.CurrentRow.Cells("DEBIT").Value = Format(Val(objTds.txtNetAmount_AMT.Text), "0.00")
    '                ElseIf gridView_OWN.CurrentRow.Cells("TYPE").Value.ToString.ToUpper = CrCap.ToUpper Then '"CR"
    '                    gridView_OWN.CurrentRow.Cells("CREDIT").Value = Format(Val(objTds.txtNetAmount_AMT.Text), "0.00")
    '                End If
    '            Else
    '                If gridView_OWN.CurrentRow.Cells("TYPE").Value.ToString.ToUpper = DrCap.ToUpper Then '"DR"
    '                    gridView_OWN.CurrentRow.Cells("DEBIT").Value = Format(Val(objTds.txtActualAmount_AMT.Text), "0.00")
    '                ElseIf gridView_OWN.CurrentRow.Cells("TYPE").Value.ToString.ToUpper = CrCap.ToUpper Then '"CR"
    '                    gridView_OWN.CurrentRow.Cells("CREDIT").Value = Format(Val(objTds.txtActualAmount_AMT.Text), "0.00")
    '                End If
    '            End If
    '            txtNarration1.Focus()
    '            Return True
    '        End If
    '    End Function

    Private Sub ShowOutStDt()
        'If objGPack.GetSqlValue("SELECT OUTSTANDING FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & gridView_OWN.CurrentRow.Cells("DESCRIPTION").Value.ToString & "'") = "Y" Then
        objOutStDt.adjAmt = Val(txtGrid_OWN.Text)
        If Not dsOutStDtCol.Tables.Contains(gridView_OWN.CurrentRow.Cells("DESCRIPTION").Value.ToString) Then
            Select Case UCase(gridView_OWN.CurrentRow.Cells("TYPE").Value.ToString)
                Case CrCap.ToUpper  '"CR"
                    'Calno 180114
                    'strSql = " SELECT CASE WHEN SUBSTRING(RUNNO,1,1) = 'G' THEN RUNNO ELSE SUBSTRING(RUNNO,6,20) END RUNNO"
                    strSql = " SELECT CASE WHEN SUBSTRING(RUNNO,1,1) = 'G' THEN '" & GetCostId(cnCostId) & GetCompanyId(strCompanyId) & "'+RUNNO ELSE RUNNO END RUNNO"
                    strSql += " ,(SELECT TOP 1 TRANDATE FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO = O.RUNNO ORDER BY TRANDATE)AS TRANDATE"
                    strSql += " ,TRANTYPE"
                    strSql += " ,SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE 0 END) AS PAYMENT"
                    strSql += " ,SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE 0 END) AS RECEIPT"
                    strSql += " ,SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE -1*AMOUNT END) AS BALANCE"
                    strSql += " ,CONVERT(NUMERIC(15,2),NULL) AS ADJUST"
                    strSql += " FROM ("
                    strSql += " SELECT RUNNO,AMOUNT,TRANTYPE,RECPAY"
                    strSql += " FROM " & cnAdminDb & "..OUTSTANDING AS O"
                    strSql += " WHERE ACCODE = (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & gridView_OWN.CurrentRow.Cells("DESCRIPTION").Value.ToString & "')"
                    strSql += " AND COMPANYID = '" & strCompanyId & "'"
                    strSql += " AND BATCHNO <> '" & BatchNo & "'"
                    strSql += " AND ISNULL(CANCEL,'') = ''"
                    If OS_ADJ_COSTCENTRE Then
                        strSql += " AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCenter_MAN.Text & "'),'')"
                    End If
                    If Approval Then
                        strSql += " UNION ALL"
                        'Calno 180114
                        'strSql += " SELECT CASE WHEN SUBSTRING(RUNNO,1,1) = 'G' THEN RUNNO ELSE SUBSTRING(RUNNO,6,20) END RUNNO,AMOUNT,TRANTYPE,RECPAY"
                        strSql += " SELECT CASE WHEN SUBSTRING(RUNNO,1,1) = 'G' THEN  '" & GetCostId(cnCostId) & GetCompanyId(strCompanyId) & "'+ RUNNO ELSE RUNNO END RUNNO,AMOUNT,TRANTYPE,RECPAY"
                        strSql += " FROM " & cnStockDb & "..TOUTSTANDING AS O"
                        strSql += " WHERE ACCODE = (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & gridView_OWN.CurrentRow.Cells("DESCRIPTION").Value.ToString & "')"
                        strSql += " AND COMPANYID = '" & strCompanyId & "'"
                        strSql += " AND BATCHNO <> '" & BatchNo & "'"
                        strSql += " AND ISNULL(CANCEL,'') = ''"
                        If OS_ADJ_COSTCENTRE Then
                            strSql += " AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCenter_MAN.Text & "'),'')"
                        End If

                    End If
                    strSql += " )O"
                    strSql += " GROUP BY RUNNO,TRANTYPE"
                    strSql += " HAVING SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE -1*AMOUNT END) > 0"
                    strSql += " ORDER BY TRANDATE"
                Case DrCap.ToUpper
                    'Calno 180114
                    'strSql = " SELECT CASE WHEN SUBSTRING(RUNNO,1,1) = 'G' THEN RUNNO ELSE SUBSTRING(RUNNO,6,20) END RUNNO"
                    strSql = " SELECT CASE WHEN SUBSTRING(RUNNO,1,1) = 'G' THEN '" & GetCostId(cnCostId) & GetCompanyId(strCompanyId) & "'+RUNNO ELSE RUNNO END RUNNO"
                    strSql += " ,(SELECT TOP 1 TRANDATE FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO = O.RUNNO ORDER BY TRANDATE)AS TRANDATE"
                    strSql += " ,TRANTYPE"
                    strSql += " ,SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE 0 END) AS RECEIPT"
                    strSql += " ,SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE 0 END) AS PAYMENT"
                    strSql += " ,SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE -1*AMOUNT END) AS BALANCE"
                    strSql += " ,CONVERT(NUMERIC(15,2),NULL) AS ADJUST"
                    strSql += " FROM ("
                    strSql += " SELECT RUNNO,TRANTYPE,AMOUNT,RECPAY"
                    strSql += " FROM " & cnAdminDb & "..OUTSTANDING AS O"
                    strSql += " WHERE ACCODE = (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & gridView_OWN.CurrentRow.Cells("DESCRIPTION").Value.ToString & "')"
                    strSql += " AND COMPANYID = '" & strCompanyId & "'"
                    strSql += " AND BATCHNO <> '" & BatchNo & "'"
                    strSql += " AND ISNULL(CANCEL,'') = ''"
                    If OS_ADJ_COSTCENTRE Then
                        strSql += " AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCenter_MAN.Text & "'),'')"
                    End If
                    If Approval Then
                        strSql += " UNION ALL"
                        'Calno 180114
                        'strSql += " SELECT CASE WHEN SUBSTRING(RUNNO,1,1) = 'G' THEN RUNNO ELSE SUBSTRING(RUNNO,6,20) END RUNNO,TRANTYPE,AMOUNT,RECPAY"
                        strSql += " SELECT CASE WHEN SUBSTRING(RUNNO,1,1) = 'G' THEN '" & GetCostId(cnCostId) & GetCompanyId(strCompanyId) & "'+RUNNO ELSE RUNNO END RUNNO,TRANTYPE,AMOUNT,RECPAY"
                        strSql += " FROM " & cnStockDb & "..TOUTSTANDING AS O"
                        strSql += " WHERE ACCODE = (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & gridView_OWN.CurrentRow.Cells("DESCRIPTION").Value.ToString & "')"
                        strSql += " AND COMPANYID = '" & strCompanyId & "'"
                        strSql += " AND BATCHNO <> '" & BatchNo & "'"
                        strSql += " AND ISNULL(CANCEL,'') = ''"
                        If OS_ADJ_COSTCENTRE Then
                            strSql += " AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCenter_MAN.Text & "'),'')"
                        End If
                    End If
                    strSql += " )O"
                    strSql += " GROUP BY RUNNO,TRANTYPE"
                    strSql += " HAVING SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE -1*AMOUNT END) > 0"
                    strSql += " ORDER BY TRANDATE"
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
                'ShowInvno()
            End If
        Else
            objOutStDt.LoadGridOutStDt(dsOutStDtCol.Tables(gridView_OWN.CurrentRow.Cells("DESCRIPTION").Value.ToString))
            objOutStDt.AutoAdjust()
            objOutStDt.ShowDialog()
        End If
        'End If
    End Sub

    Private Sub InsertIntoOustanding _
  (ByVal tranNo As Integer, ByVal batchno As String, _
  ByVal tType As String, ByVal RunNo As String, ByVal Amount As Double, _
  ByVal RecPay As String, _
  ByVal Paymode As String, _
  Optional ByVal GrsWt As Double = 0, _
  Optional ByVal NetWt As Double = 0, _
  Optional ByVal CatCode As String = Nothing, _
  Optional ByVal Rate As Double = Nothing, _
  Optional ByVal Value As Double = Nothing, _
  Optional ByVal refNo As String = Nothing, _
  Optional ByVal refDate As String = Nothing, _
  Optional ByVal purity As Double = Nothing, _
  Optional ByVal proId As Integer = Nothing, _
  Optional ByVal dueDate As String = Nothing, _
  Optional ByVal Remark1 As String = Nothing, _
  Optional ByVal Remark2 As String = Nothing, _
  Optional ByVal accode As String = Nothing _
      )
        If Amount = 0 And GrsWt = 0 Then Exit Sub
        strSql = " INSERT INTO " & cnAdminDb & ".." & IIf(Approval, "TOUTSTANDING", "OUTSTANDING")
        strSql += " ("
        strSql += " SNO,TRANNO,TRANDATE,TRANTYPE,RUNNO"
        strSql += " ,AMOUNT,GRSWT,NETWT,RECPAY"
        strSql += " ,REFNO,REFDATE,EMPID"
        strSql += " ,TRANSTATUS,PURITY,CATCODE,BATCHNO"
        strSql += " ,USERID,UPDATED,UPTIME,SYSTEMID"
        strSql += " ,RATE,VALUE,CASHID,REMARK1,REMARK2,ACCODE,CTRANCODE,DUEDATE,APPVER,COSTID,COMPANYID,PAYMODE,FROMFLAG)"
        strSql += " VALUES"
        strSql += " ("
        strSql += " '" & GetNewSno(IIf(Approval, TranSnoType.TOUTSTANDINGCODE, TranSnoType.OUTSTANDINGCODE), tran, "GET_ADMINSNO_TRAN") & "'" ''SNO
        strSql += " ," & tranNo & "" 'TRANNO
        strSql += " ,'" & dtpDate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
        strSql += " ,'" & tType & "'" 'TRANTYPE
        strSql += " ,'" & RunNo & "'" 'RUNNO
        strSql += " ," & Math.Abs(Amount) & "" 'AMOUNT
        strSql += " ," & Math.Abs(GrsWt) & "" 'GRSWT
        strSql += " ," & Math.Abs(NetWt) & "" 'NETWT
        strSql += " ,'" & RecPay & "'" 'RECPAY
        strSql += " ,'" & refNo & "'" 'REFNO
        If refDate <> Nothing Then
            strSql += " ,'" & refDate & "'" 'REFDATE
        Else
            strSql += " ,NULL" 'REFDATE
        End If

        strSql += " ,0" 'EMPID
        strSql += " ,''" 'TRANSTATUS
        strSql += " ," & purity & "" 'PURITY
        strSql += " ,'" & CatCode & "'" 'CATCODE
        strSql += " ,'" & batchno & "'" 'BATCHNO
        strSql += " ," & userId & "" 'USERID

        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
        strSql += " ,'" & systemId & "'" 'SYSTEMID
        strSql += " ," & Rate & "" 'RATE
        strSql += " ," & Value & "" 'VALUE
        strSql += " ,''" 'CASHID
        strSql += " ,'" & Remark1 & "'" 'REMARK1
        strSql += " ,'" & Remark2 & "'" 'REMARK1
        strSql += " ,'" & accode & "'" 'ACCODE
        strSql += " ," & proId & "" 'CTRANCODE
        If dueDate <> Nothing Then
            strSql += " ,'" & dueDate & "'" 'DUEDATE
        Else
            strSql += " ,NULL" 'DUEDATE
        End If
        strSql += " ,'" & VERSION & "'" 'APPVER
        strSql += " ,'" & CostId & "'" 'COSTID
        strSql += " ,'" & strCompanyId & "'" 'COMPANYID
        strSql += " ,'" & Paymode & "'" 'PAYMODE
        strSql += " ,'A'" 'FROMFLAG
        strSql += " )"
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
            Dim sw As String = txtGrid_OWN.Text
            Dim RowFilterStr As String = VouchFilteration
            If RowFilterStr <> Nothing Then RowFilterStr += " AND "
            RowFilterStr += "ACNAME LIKE '%" & sw & "%'"
            dtAccNames.DefaultView.RowFilter = RowFilterStr
        Else
            lstSearch.Visible = False
        End If
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
            'cmbNarration.Text = Dgv1Search.CurrentRow.Cells("NARRATION").Value.ToString
            txtNarration1.Text = Dgv1Search.CurrentRow.Cells("NARRATION").Value.ToString
            Dgv1Search.Visible = False
            txtNarration1.Select()
        End If
    End Sub
    Private Sub txtNarration1_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNarration1.GotFocus
        'txtNarration1.Text = cmbNarration.Text
        'Dgv1Search.Location = New Point(cmbNarration.Location.X, cmbNarration.Location.Y + cmbNarration.Height)
        'Dgv1Search.Size = New Size(cmbNarration.Size.Width, 150)
        'Dgv1Search.Columns(0).Width = cmbNarration.Size.Width
        txtNarration1.Visible = True
        txtNarration1.BringToFront()
        txtNarration1.Select()
        Dgv1Search.BringToFront()
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
        gridView_OWN.DataSource = dtGridView
        StyleGridView(gridView_OWN)
        dsSl = New DataSet
        gridView_OWN.CurrentCell = gridView_OWN.Rows(0).Cells(0)
        lblPendAmt.Visible = False
        LoadAcc()
        lblbud.Visible = False
        dtpDate.Select()
    End Sub

    Private Sub txtNarration2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtNarration2.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then

            Dim curIndexx As Integer = gridView_OWN.CurrentRow.Index
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
            NextNewRow()
            objTds = New frmAccTds
        End If
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If editFlag Then
            If Not BrighttechPack.Methods.GetRights(_DtUserRights, mnuId & Me.Name, BrighttechPack.Methods.RightMode.Edit) Then Exit Sub
        Else
            If Not BrighttechPack.Methods.GetRights(_DtUserRights, mnuId & Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
        End If
        If checkBdate() = False Then Exit Sub
        If CheckTrialDate(dtpDate.Value) = False Then Exit Sub
        Dim debit As Double = Val(dtGridView.Compute("SUM(DEBIT)", "").ToString)
        Dim Credit As Double = Val(dtGridView.Compute("SUM(CREDIT)", "").ToString)
        If Val(debit.ToString) = 0 And Val(Credit.ToString) = 0 Then
            MsgBox("There is no Record", MsgBoxStyle.Information)
            Exit Sub
        End If
        'If GetGridViewTotal() <> 0 Then
        '    MsgBox("Debit Credit Not Tally", MsgBoxStyle.Information)
        '    gridView_OWN.CurrentCell = gridView_OWN.CurrentRow.Cells(0)
        '    gridView_OWN.Select()
        '    Exit Sub
        'End If
        If cmbCostCenter_MAN.Enabled = True Then
            If cmbCostCenter_MAN.Text = "" Then
                MsgBox("Cost center Empty", MsgBoxStyle.Information)
                cmbCostCenter_MAN.Select()
                Exit Sub
            Else
                If objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCenter_MAN.Text & "'") = "" Then MsgBox("Please select valid costcentre", MsgBoxStyle.Critical) : cmbCostCenter_MAN.Focus() : Exit Sub
            End If
        End If
        Try
            tran = Nothing
            tran = cn.BeginTransaction()
            strSql = " IF (SELECT COUNT(*) FROM " & cnStockDb & "..SYSOBJECTS WHERE NAME = '" & mtempacctran & "')> 0"
            strSql += " DROP TABLE " & cnStockDb & ".." & mtempacctran
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()

            strSql = " SELECT  SNO, TRANNO, TRANDATE, TRANMODE, ACCODE, SACCODE, AMOUNT, BALANCE, PCS, GRSWT, NETWT, PUREWT, REFNO, REFDATE, PAYMODE, CHQCARDNO, CARDID, CHQCARDREF, CHQDATE, BRSFLAG, RELIASEDATE, FROMFLAG, REMARK1, REMARK2, CONTRA, BatchNo, userId, UPDATED, UPTIME, systemId, CANCEL, CASHID, COSTID, SCOSTID, APPVER, COMPANYID, TRANSFERED, WT_ENTORDER, Rate, TRANFLAG, PCODE, Disc_EmpId, CASHPOINTID, TDSCATID, TDSPER, TDSAMOUNT, FLAG, ENTREFNO, ESTBATCHNO"
            strSql += " INTO " & cnStockDb & ".." & mtempacctran & " FROM " & cnStockDb & "..ACCTRAN WHERE 1=2"
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()

            'If editFlag Then
            '    ''delete
            '    strSql = " DELETE FROM " & cnStockDb & ".." & IIf(Approval, "TACCTRAN", "ACCTRAN") & ""
            '    strSql += " WHERE BATCHNO = '" & BatchNo & "'"
            '    ExecQuery(SyncMode.Transaction, strSql, cn, tran, CostId)
            '    strSql = " DELETE FROM " & cnAdminDb & ".." & IIf(Approval, "TOUTSTANDING", "OUTSTANDING") & ""
            '    strSql += " WHERE BATCHNO = '" & BatchNo & "'"
            '    ExecQuery(SyncMode.Transaction, strSql, cn, tran, CostId)
            '    Edchqno = Nothing
            '    Edchqdate = Nothing
            '    Edchqdetail = Nothing
            '    GoTo InsertEntry
            'End If
            payMode = "JE" 'objGPack.GetSqlValue("SELECT PAYMODE FROM " & cnAdminDb & "..ACCENTRYMASTER WHERE SNO = '" & mnuId & "'", , , tran)
            Dim Isfirst As Boolean = True
GenBillNo:
            TranNo = Val(GetBillControlValue(ctrlId, tran, Not Isfirst))
            strSql = " UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT = '" & TranNo + 1 & "'"
            strSql += " WHERE CTLID = '" & ctrlId & "' AND COMPANYID = '" & strCompanyId & "'"
            If Isfirst And strBCostid <> Nothing Then strSql += " AND COSTID ='" & strBCostid & "'"
            strSql += " AND CONVERT(INT,CTLTEXT) = '" & TranNo & "'"
            cmd = New OleDbCommand(strSql, cn, tran)
            If Not cmd.ExecuteNonQuery() > 0 Then
                If strBCostid <> Nothing Then MsgBox("Tran No. empty. Please check Bill control") : tran.Rollback() : tran.Dispose() : tran = Nothing : Exit Sub
                Isfirst = False
                GoTo GenBillNo
            End If
            TranNo += 1
            BatchNo = GetNewBatchno(cnCostId, dtpDate.Value.ToString("yyyy-MM-dd"), tran)
InsertEntry:
            CostId = objGPack.GetSqlValue("SELECT ISNULL(COSTID,'') FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCenter_MAN.Text & "'", , , tran)
            Dim SingleMulty As String = Nothing
            Dim drRows() As DataRow = Nothing
            Dim crRows() As DataRow = Nothing
            drRows = dtGridView.Select("TYPE = '" & DrCap & "' AND (GENBY = '' OR GENBY IS NULL)")
            crRows = dtGridView.Select("TYPE = '" & CrCap & "' AND (GENBY = '' OR GENBY IS NULL)")

            'If drRows.Length <> crRows.Length Then
            '    If drRows.Length = 1 Then
            '        SingleMulty = DrCap '"DR"
            '    ElseIf crRows.Length = 1 Then
            '        SingleMulty = CrCap '"CR"
            '    End If
            'End If
            Dim chqNo As String = Nothing
            Dim chqDate As String = Nothing
            Dim chqDetail As String = Nothing

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
                Dim amt As Double = IIf(Val(ro!DEBIT.ToString) > 0, Val(ro!DEBIT.ToString), Val(ro!CREDIT.ToString))
                Dim tranMode As Char = IIf(Val(ro!DEBIT.ToString) > 0, "D", "C")
                Dim recPay As Char = IIf(Val(ro!DEBIT.ToString) > 0, "P", "R")
                Dim Contra As String = Nothing
                Dim keyNo As Integer = Val(ro.Item("KEYNO").ToString)
                Dim accode As String = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & ro!DESCRIPTION & "'", , , tran)
                If gsaccode = "" Then gsaccode = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & ro!DESCRIPTION & "' AND ACTYPE IN('G','S','D')", , , tran)
                Dim adjAmt As Double = amt
                If tranMode = "D" Then totdramt += amt Else totcramt += amt
                Contra = "DRS"
                ''BALANCE OUTST ADJUST AMT
                Dim runNo As String = Nothing
                runNo = GetCostId(cnCostId) & GetCompanyId(strCompanyId) & "G" + Mid(Format(cnTranToDate, "dd/MM/yyyy"), 9, 2).ToString + GetTranDbSoftControlValue("RUNNO_ACC", True, tran)
                Dim mInvno As String = "99999"
                If objInvNo.txtInvno.Text <> "" Then mInvno = objInvNo.txtInvno.Text


                Dim RefInvno As String = IIf(objInvNo.txtInvno.Text <> "", objInvNo.txtInvno.Text, "")
                Dim Refinvdate As Date = IIf(objInvNo.txtInvno.Text <> "", Format(objInvNo.dtpInvDate.Value, "yyyy/MM/dd"), Nothing)


                If dsOutStDtCol.Tables.Contains(ro!DESCRIPTION.ToString) Then
                    For Each roOutSt As DataRow In dsOutStDtCol.Tables(ro!DESCRIPTION.ToString).Rows
                        InsertIntoOustanding(TranNo, BatchNo, roOutSt!TRANTYPE.ToString, roOutSt!RUNNO.ToString, Val(roOutSt!ADJUST.ToString), recPay _
                        , IIf(recPay = "R", "DR", "DP"), , , , , , , , , , , , , accode)
                        ''Acctran Entry
                        InsertIntoAccTran(keyNo, TranNo, tranMode _
                          , accode, Val(roOutSt!ADJUST.ToString), 0, 0, 0, payMode, Contra, 0, 0, 0, RefInvno, Refinvdate, chqNo, chqDate, , chqDetail, ro!NARRATION1.ToString, ro!NARRATION2.ToString, ro!GENBY.ToString, , "")
                        InsertIntoAccTran(keyNo, TranNo, IIf(tranMode = "D", "C", "D") _
                        , Contra, Val(roOutSt!ADJUST.ToString), 0, 0, 0, payMode, accode, 0, 0, 0, RefInvno, Refinvdate, chqNo, chqDate, , chqDetail, ro!NARRATION1.ToString, ro!NARRATION2.ToString, ro!GENBY.ToString, , "")
                        adjAmt -= Val(roOutSt!ADJUST.ToString)
                    Next
                Else
                    InsertIntoOustanding(TranNo, BatchNo, "T", runNo, adjAmt, recPay _
                                    , IIf(recPay = "R", "DR", "DP"), , , , , , , , , , , , , accode)
                    ''Acctran Entry
                    InsertIntoAccTran(keyNo, TranNo, tranMode _
                      , accode, amt, 0, 0, 0, payMode, Contra, 0, 0, 0, RefInvno, Refinvdate, chqNo, chqDate, , chqDetail, ro!NARRATION1.ToString, ro!NARRATION2.ToString, ro!GENBY.ToString, , "")
                    InsertIntoAccTran(keyNo, TranNo, IIf(tranMode = "D", "C", "D") _
                    , Contra, amt, 0, 0, 0, payMode, accode, 0, 0, 0, RefInvno, Refinvdate, chqNo, chqDate, , chqDetail, ro!NARRATION1.ToString, ro!NARRATION2.ToString, ro!GENBY.ToString, , "")

                End If

                

                If SingleMulty = Nothing Then
                    If editFlag Then
                        strSql = " UPDATE " & cnStockDb & "..ACCTRAN SET CONTRA  = (SELECT TOP 1 ACCODE FROM " & cnStockDb & "..ACCTRAN WHERE TRANDATE = T.TRANDATE AND AMOUNT = T.AMOUNT AND BATCHNO = T.BATCHNO AND TRANMODE = 'D' AND ACCODE <> T.ACCODE AND COMPANYID = '" & strCompanyId & "' ORDER BY SNO)"
                        strSql += " FROM " & cnStockDb & "..ACCTRAN AS T WHERE TRANDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' AND BATCHNO = '" & BatchNo & "' AND  TRANMODE = 'C' AND COMPANYID = '" & strCompanyId & "' AND ISNULL(CONTRA,'') = ''"
                        ExecQuery(SyncMode.Transaction, strSql, cn, tran, CostId)
                        strSql = " UPDATE " & cnStockDb & "..ACCTRAN SET CONTRA  = (SELECT TOP 1 ACCODE FROM " & cnStockDb & "..ACCTRAN WHERE TRANDATE = T.TRANDATE AND AMOUNT =T.AMOUNT AND BATCHNO = T.BATCHNO AND TRANMODE = 'C' AND ACCODE <> T.ACCODE AND COMPANYID = '" & strCompanyId & "' ORDER BY SNO)"
                        strSql += " FROM " & cnStockDb & "..ACCTRAN AS T WHERE TRANDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' AND BATCHNO = '" & BatchNo & "' AND TRANMODE = 'D' AND COMPANYID = '" & strCompanyId & "' AND ISNULL(CONTRA,'') = ''"
                        ExecQuery(SyncMode.Transaction, strSql, cn, tran, CostId)
                    Else
                        strSql = " UPDATE " & cnStockDb & ".." & mtempacctran & " SET CONTRA  = (SELECT TOP 1 ACCODE FROM " & cnStockDb & ".." & mtempacctran & " WHERE TRANDATE = T.TRANDATE AND AMOUNT = T.AMOUNT AND BATCHNO = T.BATCHNO AND TRANMODE = 'D' AND ACCODE <> T.ACCODE AND COMPANYID = '" & strCompanyId & "' ORDER BY SNO)"
                        strSql += " FROM " & cnStockDb & ".." & mtempacctran & " AS T WHERE TRANDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' AND BATCHNO = '" & BatchNo & "' AND  TRANMODE = 'C' AND COMPANYID = '" & strCompanyId & "' AND ISNULL(CONTRA,'') = ''"
                        cmd = New OleDbCommand(strSql, cn, tran)
                        cmd.ExecuteNonQuery()
                        strSql = " UPDATE " & cnStockDb & ".." & mtempacctran & " SET CONTRA  = (SELECT TOP 1 ACCODE FROM " & cnStockDb & ".." & mtempacctran & " WHERE TRANDATE = T.TRANDATE AND AMOUNT =T.AMOUNT AND BATCHNO = T.BATCHNO AND TRANMODE = 'C' AND ACCODE <> T.ACCODE AND COMPANYID = '" & strCompanyId & "' ORDER BY SNO)"
                        strSql += " FROM " & cnStockDb & ".." & mtempacctran & " AS T WHERE TRANDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' AND BATCHNO = '" & BatchNo & "' AND TRANMODE = 'D' AND COMPANYID = '" & strCompanyId & "' AND ISNULL(CONTRA,'') = ''"
                        cmd = New OleDbCommand(strSql, cn, tran)
                        cmd.ExecuteNonQuery()
                    End If

                End If
            Next

            If SingleMulty = Nothing Then
                If editFlag Then
                    strSql = " UPDATE " & cnStockDb & "..ACCTRAN SET CONTRA  = (SELECT TOP 1 ACCODE FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = T.BATCHNO AND TRANDATE = T.TRANDATE AND TRANMODE = 'D' AND ACCODE <> T.ACCODE AND COMPANYID = '" & strCompanyId & "' ORDER BY SNO)"
                    strSql += " FROM " & cnStockDb & "..ACCTRAN AS T WHERE BATCHNO = '" & BatchNo & "' AND TRANDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' AND TRANMODE = 'C' AND COMPANYID = '" & strCompanyId & "' AND ISNULL(CONTRA,'') = ''"
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, CostId)

                    strSql = " UPDATE " & cnStockDb & "..ACCTRAN SET CONTRA  = (SELECT TOP 1 ACCODE FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = T.BATCHNO AND TRANDATE = T.TRANDATE AND TRANMODE = 'C' AND ACCODE <> T.ACCODE AND COMPANYID = '" & strCompanyId & "' ORDER BY SNO)"
                    strSql += " FROM " & cnStockDb & "..ACCTRAN AS T WHERE BATCHNO = '" & BatchNo & "' AND TRANDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' AND TRANMODE = 'D' AND COMPANYID = '" & strCompanyId & "' AND ISNULL(CONTRA,'') = ''"
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, CostId)
                Else
                    strSql = " UPDATE " & cnStockDb & ".." & mtempacctran & " SET CONTRA  = (SELECT TOP 1 ACCODE FROM " & cnStockDb & ".." & mtempacctran & " WHERE BATCHNO = T.BATCHNO AND TRANDATE = T.TRANDATE AND TRANMODE = 'D' AND ACCODE <> T.ACCODE AND COMPANYID = '" & strCompanyId & "' ORDER BY SNO)"
                    strSql += " FROM " & cnStockDb & ".." & mtempacctran & " AS T WHERE BATCHNO = '" & BatchNo & "' AND TRANDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' AND TRANMODE = 'C' AND COMPANYID = '" & strCompanyId & "' AND ISNULL(CONTRA,'') = ''"
                    cmd = New OleDbCommand(strSql, cn, tran)
                    cmd.ExecuteNonQuery()

                    strSql = " UPDATE " & cnStockDb & ".." & mtempacctran & " SET CONTRA  = (SELECT TOP 1 ACCODE FROM " & cnStockDb & ".." & mtempacctran & " WHERE BATCHNO = T.BATCHNO AND TRANDATE = T.TRANDATE AND TRANMODE = 'C' AND ACCODE <> T.ACCODE AND COMPANYID = '" & strCompanyId & "' ORDER BY SNO)"
                    strSql += " FROM " & cnStockDb & ".." & mtempacctran & " AS T WHERE BATCHNO = '" & BatchNo & "' AND TRANDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' AND TRANMODE = 'D' AND COMPANYID = '" & strCompanyId & "' AND ISNULL(CONTRA,'') = ''"
                    cmd = New OleDbCommand(strSql, cn, tran)
                    cmd.ExecuteNonQuery()
                End If
            End If

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
            strSql += " DROP TABLE " & cnStockDb & ".." & mtempacctran
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()
            tran.Commit()
            tran = Nothing


            If editFlag Then
                MsgBox(TranNo & " Updated..")
                Me.DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()
                Exit Sub
            Else
                MsgBox(TranNo & " Generated..")
            End If
            Dim pBatchno As String = BatchNo
            Dim pBillDate As Date = dtpDate.Value
            Dim pParamStr As String = ""
            btnNew_Click(Me, New EventArgs)
            If IO.File.Exists(Application.StartupPath & "\BillPrint.exe") Then
                Dim write As IO.StreamWriter
                Dim prnmemsuffix As String = ""
                If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId
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
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        Finally
            If Not tran Is Nothing Then tran.Dispose()
        End Try
    End Sub

    Private Sub InsertIntoAccTran _
  (ByVal EntryOrder As Integer, ByVal tNo As Integer, _
  ByVal tranMode As String, _
  ByVal accode As String, _
  ByVal amount As Double, _
  ByVal pcs As Integer, _
  ByVal grsWT As Double, _
  ByVal netWT As Double, _
  ByVal payMode As String, _
  ByVal contra As String, _
  ByVal TDSCATID As Integer, _
  ByVal TDSPER As Decimal, _
  ByVal TDSAMOUNT As Decimal, _
  Optional ByVal refNo As String = Nothing, Optional ByVal refDate As String = Nothing, _
  Optional ByVal chqCardNo As String = Nothing, _
  Optional ByVal chqDate As String = Nothing, _
  Optional ByVal chqCardId As Integer = Nothing, _
  Optional ByVal chqCardRef As String = Nothing, _
  Optional ByVal Remark1 As String = Nothing, _
  Optional ByVal Remark2 As String = Nothing, _
  Optional ByVal fLAG As String = Nothing, _
  Optional ByVal SAccode As String = "", _
  Optional ByVal SCostid As String = "" _
  )
        If amount = 0 Then Exit Sub

        If editFlag Then
            strSql = " INSERT INTO " & cnStockDb & ".." & IIf(Approval, "TACCTRAN", "ACCTRAN") & ""
        Else
            strSql = " INSERT INTO " & cnStockDb & ".." & mtempacctran & ""
        End If
        strSql += " ("
        strSql += " SNO,TRANNO,TRANDATE,TRANMODE,ACCODE,SACCODE"
        strSql += " ,AMOUNT,PCS,GRSWT,NETWT,REFNO,REFDATE,PAYMODE,CHQCARDNO"
        strSql += " ,CARDID,CHQCARDREF,CHQDATE,BRSFLAG,RELIASEDATE,FROMFLAG,REMARK1,REMARK2"
        strSql += " ,CONTRA,BATCHNO,USERID,UPDATED,UPTIME,SYSTEMID,CASHID,COSTID,SCOSTID"
        strSql += " ,APPVER,COMPANYID,WT_ENTORDER,TDSCATID,TDSPER,TDSAMOUNT,FLAG"
        strSql += " )"
        strSql += " VALUES"
        strSql += " ("
        strSql += " '" & GetNewSno(IIf(Approval, TranSnoType.TACCTRANCODE, TranSnoType.ACCTRANCODE), tran) & "'" ''SNO
        strSql += " ," & tNo & "" 'TRANNO 
        strSql += " ,'" & dtpDate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
        strSql += " ,'" & tranMode & "'" 'TRANMODE
        strSql += " ,'" & accode & "'" 'ACCODE
        strSql += " ,'" & SAccode & "'" 'ACCODE
        strSql += " ," & Math.Abs(amount) & "" 'AMOUNT
        strSql += " ," & Math.Abs(pcs) & "" 'PCS
        strSql += " ," & Math.Abs(grsWT) & "" 'GRSWT
        strSql += " ," & Math.Abs(netWT) & "" 'NETWT
        strSql += " ,'" & refNo & "'" 'REFNO
        If refDate = Nothing Then
            strSql += " ,NULL" 'REFDATE
        Else
            strSql += " ,'" & refDate & "'" 'REFDATE
        End If
        strSql += " ,'" & payMode & "'" 'PAYMODE
        strSql += " ,'" & chqCardNo & "'" 'CHQCARDNO
        strSql += " ," & chqCardId & "" 'CARDID
        strSql += " ,'" & chqCardRef & "'" 'CHQCARDREF
        If chqDate = Nothing Then
            strSql += " ,NULL" 'CHQDATE
        Else
            strSql += " ,'" & chqDate & "'" 'CHQDATE
        End If
        strSql += " ,''" 'BRSFLAG
        strSql += " ,NULL" 'RELIASEDATE
        strSql += " ,'A'" 'FROMFLAG
        strSql += " ,'" & Remark1 & "'" 'REMARK1
        strSql += " ,'" & Remark2 & "'" 'REMARK2
        strSql += " ,'" & contra & "'" 'CONTRA
        strSql += " ,'" & BatchNo & "'" 'BATCHNO
        strSql += " ,'" & userId & "'" 'USERID
        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
        strSql += " ,'" & systemId & "'" 'SYSTEMID
        strSql += " ,''" 'CASHID
        strSql += " ,'" & CostId & "'" 'COSTID
        strSql += " ,'" & SCostid & "'" 'FLAG
        strSql += " ,'" & VERSION & "'" 'APPVER
        strSql += " ,'" & strCompanyId & "'" 'COMPANYID
        strSql += " ," & EntryOrder & "" 'WT_ENTORDER
        strSql += " ," & TDSCATID & "" 'TDSCATID
        strSql += " ," & TDSPER & "" 'TDSPER
        strSql += " ," & TDSAMOUNT & "" 'TDSAMOUNT
        strSql += " ,'" & fLAG & "'" 'FLAG
        strSql += " )"
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

    Private Sub dgvData_CellPainting(ByVal sender As Object, _
    ByVal e As DataGridViewCellPaintingEventArgs) _
    Handles gridView_OWN.CellPainting
        ' Only the Header Row (which Index is -1) is to be affected.
        If e.RowIndex = -1 Then
            GridDrawCustomHeaderColumns(gridView_OWN, e, _
             My.Resources.Button_Gray_Stripe_01_050, _
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
    Private Sub GridDrawCustomHeaderColumns(ByVal dgv As DataGridView, _
     ByVal e As DataGridViewCellPaintingEventArgs, ByVal img As Image, _
     ByVal Style As DGVHeaderImageAlignments)
        ' All of the graphical Processing is done here.
        Dim gr As Graphics = e.Graphics
        ' Fill the BackGround with the BackGroud Color of Headers.
        ' This step is necessary, for transparent images, or what's behind
        ' would be painted instead.
        gr.FillRectangle( _
         New SolidBrush(dgv.ColumnHeadersDefaultCellStyle.BackColor), _
         e.CellBounds)
        If img IsNot Nothing Then
            Select Case Style
                Case DGVHeaderImageAlignments.FillCell
                    gr.DrawImage( _
                     img, e.CellBounds.X, e.CellBounds.Y, _
                     e.CellBounds.Width, e.CellBounds.Height)
                Case DGVHeaderImageAlignments.SingleCentered
                    gr.DrawImage(img, _
                     ((e.CellBounds.Width - img.Width) \ 2) + e.CellBounds.X, _
                     ((e.CellBounds.Height - img.Height) \ 2) + e.CellBounds.Y, _
                     img.Width, img.Height)
                Case DGVHeaderImageAlignments.SingleLeft
                    gr.DrawImage(img, e.CellBounds.X, _
                     ((e.CellBounds.Height - img.Height) \ 2) + e.CellBounds.Y, _
                     img.Width, img.Height)
                Case DGVHeaderImageAlignments.SingleRight
                    gr.DrawImage(img, _
                     (e.CellBounds.Width - img.Width) + e.CellBounds.X, _
                     ((e.CellBounds.Height - img.Height) \ 2) + e.CellBounds.Y, _
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
                    gr.DrawImage( _
                     img, e.CellBounds.X, e.CellBounds.Y, _
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
                gr.DrawString(e.Value.ToString, .Font, _
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
        strSql += " AND ISNULL(MACCODE,'') = ''"
        strSql += GetAcNameQryFilteration()
        strSql += " ORDER BY ACNAME"
        dtAccNames = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtAccNames)
        LoadAcc()
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

    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView.Click
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

        Dim serverDate As Date = GetActualEntryDate()
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


End Class