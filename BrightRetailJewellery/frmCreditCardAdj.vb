Public Class frmCreditCardAdj
    Public dtGridCreditCard As New DataTable
    Dim strSql As String
    Dim POS_CARD_DET As Boolean = IIf(GetAdmindbSoftValue("POS_CARD_DET", "N") = "Y", True, False)
    Dim CardType As String = "R"
    Public BillAmount As Double = 0
    Dim POS_CARD_AMT_CHK As Boolean = IIf(GetAdmindbSoftValue("POS_CARD_AMT_CHK", "N") = "Y", True, False)

    Public Sub New(ByVal Start As Boolean)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        objGPack.Validator_Object(Me)
        objGPack.TextClear(Me)
        gridCreditCard.Font = New Font("VERDANA", 8, FontStyle.Bold)
        gridCreditCardTotal.Font = New Font("VERDANA", 8, FontStyle.Bold)
        ' Add any initialization after the InitializeComponent() call.
        ''CREDIT CARD
        strSql = " SELECT NAME FROM " & cnAdminDb & "..CREDITCARD "
        strSql += " WHERE CARDTYPE IN('R','U') AND ISNULL(ACTIVE,'') <> 'N' ORDER BY DISPORDER "
        objGPack.FillCombo(strSql, cmbCreditCardType)

        With dtGridCreditCard
            .Columns.Add("CARDTYPE", GetType(String))
            .Columns.Add("DATE", GetType(Date))
            .Columns.Add("AMOUNT", GetType(Double))
            .Columns.Add("CARDNO", GetType(String))
            .Columns.Add("APPNO", GetType(String))
            .Columns.Add("COMMISION", GetType(Double))
            .Columns.Add("CRCURAMT", GetType(Double))
            .Columns.Add("CRCONVERT", GetType(Double))
            .Columns.Add("CRAMOUNT", GetType(Double))
            .Columns.Add("CRREMARKS", GetType(String))
        End With
        gridCreditCard.DataSource = dtGridCreditCard
        FormatGridColumns(gridCreditCard)
        StyleGridCreditCard(gridCreditCard)
        Dim dtGridCreditCardTotal As New DataTable
        dtGridCreditCardTotal = dtGridCreditCard.Copy
        dtGridCreditCardTotal.Rows.Clear()
        dtGridCreditCardTotal.Rows.Add()
        dtGridCreditCardTotal.Rows(0).Item("CARDTYPE") = "Total"
        With gridCreditCardTotal
            .DataSource = dtGridCreditCardTotal
            For Each col As DataGridViewColumn In gridCreditCard.Columns
                With gridCreditCard.Columns(col.Name)
                    .Visible = col.Visible
                    .Width = col.Width
                    .DefaultCellStyle = col.DefaultCellStyle
                End With
            Next
        End With
        FormatGridColumns(gridCreditCardTotal)
        StyleGridCreditCard(gridCreditCardTotal)
    End Sub
    Private Sub StyleGridCreditCard(ByVal grid As DataGridView)
        If grid.DataSource Is Nothing Then Exit Sub
        gridCreditCardTotal.DefaultCellStyle.SelectionBackColor = grpCreditCard.BackgroundColor
        With grid
            If CardType = "U" Then
                .Columns("CARDTYPE").Width = cmbCreditCardType.Width + 1
                .Columns("CRCURAMT").Width = txtCr_AMT.Width + 1
                .Columns("CRCONVERT").Width = txtCrCon_AMT.Width + 1
                .Columns("CRAMOUNT").Width = txtCrAmount_AMT.Width + 1
                .Columns("CRREMARKS").Width = txtCrRemark.Width + 1
                .Columns("AMOUNT").Visible = False
                .Columns("DATE").Visible = False
                .Columns("CARDNO").Visible = False
                .Columns("APPNO").Visible = False
                .Columns("CRCURAMT").Visible = True
                .Columns("CRCONVERT").Visible = True
                .Columns("CRAMOUNT").Visible = True
                .Columns("CRREMARKS").Visible = True
                .Columns("COMMISION").Visible = False
            Else
                .Columns("CARDTYPE").Width = cmbCreditCardType.Width + 1
                .Columns("AMOUNT").Width = txtCreditCardAmount_AMT.Width + 1
                .Columns("DATE").Width = dtpCreditCardDate.Width + 1
                .Columns("CARDNO").Width = txtCreditCardNo.Width + 1
                .Columns("APPNO").Width = txtCreditCardAprovalNo.Width + 1
                .Columns("CRCURAMT").Visible = False
                .Columns("CRCONVERT").Visible = False
                .Columns("CRAMOUNT").Visible = False
                .Columns("CRREMARKS").Visible = False
                .Columns("CARDTYPE").Visible = True
                .Columns("AMOUNT").Visible = True
                .Columns("DATE").Visible = True
                .Columns("CARDNO").Visible = True
                .Columns("APPNO").Visible = True
                .Columns("COMMISION").Visible = False
            End If
        End With
    End Sub

    Private Sub frmCreditCardAdj_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub txtAdjCreditCard_AMT_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = Keys.Down Then
            If gridCreditCard.RowCount > 0 Then gridCreditCard.Select()
        End If
    End Sub

    Private Sub txtAdjCreditCard_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If e.KeyChar = Chr(Keys.Enter) Then
            If Val(txtCreditCardAmount_AMT.Text) = 0 Then
                MsgBox(Me.GetNextControl(txtCreditCardAmount_AMT, False).Text + E0001, MsgBoxStyle.Information)
                txtCreditCardAmount_AMT.Focus()
            End If
        End If

    End Sub

    Private Sub txtCreditCardNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtCreditCardNo.KeyDown
        If e.KeyCode = Keys.Down Then
            If gridCreditCard.RowCount > 0 Then gridCreditCard.Select()
        End If

    End Sub

    Private Sub txtCreditCardAprovalNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtCreditCardAprovalNo.KeyDown
        If e.KeyCode = Keys.Down Then
            If gridCreditCard.RowCount > 0 Then gridCreditCard.Select()
        End If
    End Sub

    Private Sub frmCreditCardAdj_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtCreditCardAprovalNo.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub cmbCreditCardType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbCreditCardType.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If cmbCreditCardType.Text = "" Then
                MsgBox(Me.GetNextControl(cmbCreditCardType, False).Text + E0001, MsgBoxStyle.Information)
                cmbCreditCardType.Focus()
            ElseIf cmbCreditCardType.Items.Contains(cmbCreditCardType.Text) = False Then
                MsgBox(E0004 + Me.GetNextControl(cmbCreditCardType, False).Text, MsgBoxStyle.Information)
                cmbCreditCardType.Focus()
            End If
        End If
    End Sub

    Private Sub cmbCreditCardType_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCreditCardType.LostFocus
        If cmbCreditCardType.Text = "" Then Exit Sub
        txtCreditCardComm.Text = Val(objGPack.GetSqlValue("SELECT COMMISSION FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & cmbCreditCardType.Text & "'"))
    End Sub

    Private Sub txtCreditCardAprovalNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCreditCardAprovalNo.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            ''VALIDATION
            If e.KeyChar = Chr(Keys.Enter) Then
                If cmbCreditCardType.Text = "" Then
                    MsgBox(Me.GetNextControl(cmbCreditCardType, False).Text + E0001, MsgBoxStyle.Information)
                    cmbCreditCardType.Focus()
                    Exit Sub
                ElseIf cmbCreditCardType.Items.Contains(cmbCreditCardType.Text) = False Then
                    MsgBox(E0004 + Me.GetNextControl(cmbCreditCardType, False).Text, MsgBoxStyle.Information)
                    cmbCreditCardType.Focus()
                    Exit Sub
                    'ElseIf txtCreditCardNo.Text = "" Then
                    '    MsgBox(Me.GetNextControl(txtCreditCardNo, False).Text + E0001, MsgBoxStyle.Information)
                    '    txtCreditCardNo.Focus()
                    '    Exit Sub
                ElseIf e.KeyChar = Chr(Keys.Enter) Then
                    If Val(txtCreditCardAmount_AMT.Text) = 0 Then
                        MsgBox(Me.GetNextControl(txtCreditCardAmount_AMT, False).Text + E0001, MsgBoxStyle.Information)
                        txtCreditCardAmount_AMT.Focus()
                        Exit Sub
                    End If
                End If
            End If
            If txtCreditCardRowIndex.Text = "" Then
                Dim ro As DataRow = Nothing
                ro = dtGridCreditCard.NewRow
                ro("CARDTYPE") = cmbCreditCardType.Text
                ro("DATE") = dtpCreditCardDate.Value.Date.ToString("yyyy-MM-dd")
                ro("AMOUNT") = Val(txtCreditCardAmount_AMT.Text)
                ro("CARDNO") = txtCreditCardNo.Text
                ro("APPNO") = txtCreditCardAprovalNo.Text
                ro("COMMISION") = Val(txtCreditCardComm.Text)
                dtGridCreditCard.Rows.Add(ro)
            Else
                With gridCreditCard.Rows(Val(txtCreditCardRowIndex.Text))
                    .Cells("CARDTYPE").Value = cmbCreditCardType.Text
                    .Cells("DATE").Value = dtpCreditCardDate.Value.Date.ToString("yyyy-MM-dd")
                    .Cells("AMOUNT").Value = Val(txtCreditCardAmount_AMT.Text)
                    .Cells("CARDNO").Value = txtCreditCardNo.Text
                    .Cells("APPNO").Value = txtCreditCardAprovalNo.Text
                    .Cells("COMMISION").Value = Val(txtCreditCardComm.Text)
                End With
            End If
            dtGridCreditCard.AcceptChanges()
            CalcGridCreditCardTotal()
            gridCreditCard.CurrentCell = gridCreditCard.Rows(gridCreditCard.RowCount - 1).Cells("CARDTYPE")
            ''cLEAR

            objGPack.TextClear(grpCreditCard)
            dtpCreditCardDate.Value = GetEntryDate(GetServerDate)
            cmbCreditCardType.Select()
        End If
    End Sub

    Public Sub CalcGridCreditCardTotal()
        gridCreditCardTotal.DefaultCellStyle.SelectionBackColor = grpCreditCard.BackgroundColor
        dtGridCreditCard.AcceptChanges()
        Dim amt As Double = Nothing
        Dim cramt As Double = Nothing
        For Each ro As DataRow In dtGridCreditCard.Rows
            amt += Val(ro!AMOUNT.ToString)
            cramt += Val(ro!CRAMOUNT.ToString)
        Next
        If CardType = "U" Then
            gridCreditCardTotal.Rows(0).Cells("CRAMOUNT").Value = IIf(cramt <> 0, cramt, DBNull.Value)
        Else
            gridCreditCardTotal.Rows(0).Cells("AMOUNT").Value = IIf(amt <> 0, amt, DBNull.Value)
        End If
    End Sub

    Private Sub gridCreditCard_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridCreditCard.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            If gridCreditCard.RowCount > 0 Then
                gridCreditCard.CurrentCell = gridCreditCard.Rows(gridCreditCard.CurrentRow.Index).Cells("CARDTYPE")
                With dtGridCreditCard.Rows(gridCreditCard.CurrentRow.Index)
                    strSql = "SELECT CARDTYPE FROM " & cnAdminDb & "..CREDITCARD WHERE NAME='" & cmbCreditCardType.Text & "'"
                    CardType = objGPack.GetSqlValue(strSql, "CARDTYPE", "R")
                    cmbCreditCardType.Text = .Item("CARDTYPE").ToString
                    If CardType = "U" Then
                        txtCr_AMT.Text = IIf(Val(.Item("CRCURAMT").ToString) <> 0, Format(Val(.Item("CRCURAMT").ToString), "0.00"), Nothing)
                        txtCrCon_AMT.Text = IIf(Val(.Item("CRCONVERT").ToString) <> 0, Format(Val(.Item("CRCONVERT").ToString), "0.00"), Nothing)
                        txtCrAmount_AMT.Text = IIf(Val(.Item("CRAMOUNT").ToString) <> 0, Format(Val(.Item("CRAMOUNT").ToString), "0.00"), Nothing)
                        txtCrRemark.Text = .Item("CRREMARKS").ToString
                    Else
                        dtpCreditCardDate.Value = .Item("DATE")
                        txtCreditCardAmount_AMT.Text = IIf(Val(.Item("AMOUNT").ToString) <> 0, Format(Val(.Item("AMOUNT").ToString), "0.00"), Nothing)
                        txtCreditCardNo.Text = .Item("CARDNO").ToString
                        txtCreditCardAprovalNo.Text = .Item("APPNO").ToString
                        txtCreditCardComm.Text = Val(.Item("COMMISION").ToString)
                    End If
                    txtCreditCardRowIndex.Text = gridCreditCard.CurrentRow.Index
                    cmbCreditCardType.Select()
                End With
            End If
        End If
    End Sub

    Private Sub gridCreditCard_UserDeletedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles gridCreditCard.UserDeletedRow
        dtGridCreditCard.AcceptChanges()
        CalcGridCreditCardTotal()
        If Not gridCreditCard.RowCount > 0 Then
            cmbCreditCardType.Select()
        End If
    End Sub

    Private Sub frmCreditCardAdj_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        dtpCreditCardDate.MinimumDate = (New DateTimePicker).MinDate
        dtpCreditCardDate.MaximumDate = (New DateTimePicker).MaxDate
        dtpCreditCardDate.Value = GetEntryDate(GetServerDate)
    End Sub

    Private Sub txtCreditCardAmount_AMT_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtCreditCardAmount_AMT.KeyDown
        If e.KeyCode = Keys.Enter And Val(txtCreditCardAmount_AMT.Text) > 0 And POS_CARD_DET Then
            If POS_CARD_AMT_CHK Then
                If Val(txtCreditCardAmount_AMT.Text) > BillAmount Then
                    MsgBox("Card Amount Exceeds Bill Amount.", MsgBoxStyle.Critical)
                    txtCreditCardAmount_AMT.Focus()
                    Exit Sub
                End If
            End If
            Dim strIO As String
            Dim lTrackingID As Long = 4001
            Dim StrCardNo As String = "", CardExpDate As String = "", StrApNo As String = ""
            Dim StrBankName As String = "", StrCardHolderName As String = ""
            Dim StrAmt As String
            Dim lTxno As Integer
            'strIO = "T161" + "," + Format(Val(txtCreditCardAmount_AMT.Text).ToString, "0.00")
            lTxno = lTxno + 1
            StrAmt = Format(Val(txtCreditCardAmount_AMT.Text.ToString), "0.00")
            StrAmt = Replace(StrAmt.ToString, ".", "")
            strIO = "T17" + Val(lTxno).ToString + "," + StrAmt + ",,,,,,,"
            Dim plutObj As New PlutusExchangeLib.ExchangeObj
            plutObj.PL_TriggerTransaction(lTrackingID, strIO)
            If strIO = "" Then
                MsgBox("Card Timeout / Decline", vbOKOnly, "Plutus Response String")
                Exit Sub
            End If
            Dim CsvMsg() As String
            Dim StrApproved As String
            CsvMsg = Split(strIO, ",")
            For i As Integer = 1 To CsvMsg.Length
                If i = 1 Then
                    StrApNo = Replace(CsvMsg(i).ToString, """", "")
                ElseIf i = 2 Then
                    StrApproved = Replace(CsvMsg(i).ToString, """", "")
                    If Trim(StrApproved) <> "" Then
                        MsgBox(Replace(CsvMsg(i).ToString, """", ""), vbOKOnly, "Plutus Response String")
                    End If
                ElseIf i = 3 Then
                    StrCardNo = Replace(CsvMsg(i).ToString, """", "")
                ElseIf i = 4 Then
                    CardExpDate = Replace(CsvMsg(i).ToString, """", "")
                ElseIf i = 5 Then
                    StrCardHolderName = Replace(CsvMsg(i).ToString, """", "")
                ElseIf i = 11 Then
                    StrApproved = Replace(CsvMsg(i).ToString, """", "")
                    If StrApproved <> "PROCESSED" Then
                        MsgBox(Replace(CsvMsg(i).ToString, """", ""), vbOKOnly, "Plutus Response String")
                        Exit Sub
                    End If
                ElseIf i = 12 Then
                    StrBankName = Replace(CsvMsg(i).ToString, """", "")
                End If
            Next
            txtCreditCardNo.Text = StrCardNo
            txtCreditCardAprovalNo.Text = StrApNo
            txtCreditCardAprovalNo.Select()
        End If
        If e.KeyCode = Keys.Enter Then
            If POS_CARD_AMT_CHK Then
                Dim CcAmt As Double = 0
                CcAmt = Val(dtGridCreditCard.Compute("SUM(AMOUNT)", Nothing).ToString)
                If Val(txtCreditCardAmount_AMT.Text) + CcAmt > BillAmount Then
                    MsgBox("Card Amount Exceeds Bill Amount.", MsgBoxStyle.Critical)
                    txtCreditCardAmount_AMT.Focus()
                    Exit Sub
                End If
            End If
        End If

    End Sub

    Private Sub cmbCreditCardType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCreditCardType.SelectedIndexChanged
        If cmbCreditCardType.Text = "" Then Exit Sub
        strSql = "SELECT CARDTYPE FROM " & cnAdminDb & "..CREDITCARD WHERE NAME='" & cmbCreditCardType.Text & "'"
        CardType = objGPack.GetSqlValue(strSql, "CARDTYPE", "R")
        If CardType = "U" Then
            strSql = "SELECT CURENCY FROM " & cnAdminDb & "..CREDITCARD WHERE NAME='" & cmbCreditCardType.Text & "'"
            txtCrCon_AMT.Text = Val(objGPack.GetSqlValue(strSql, "CURENCY", 0))
            pnlCard_OWN.Visible = False
            pnlCurrecny_OWN.Visible = True
            pnlCurrecny_OWN.Location = New Drawing.Point(182, 9)
        Else
            pnlCard_OWN.Visible = True
            pnlCurrecny_OWN.Visible = False
        End If
        StyleGridCreditCard(gridCreditCard)
        StyleGridCreditCard(gridCreditCardTotal)
    End Sub

    Private Sub txtCrRemark_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCrRemark.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            ''VALIDATION
            If cmbCreditCardType.Text = "" Then
                MsgBox(Me.GetNextControl(cmbCreditCardType, False).Text + E0001, MsgBoxStyle.Information)
                cmbCreditCardType.Focus()
                Exit Sub
            ElseIf cmbCreditCardType.Items.Contains(cmbCreditCardType.Text) = False Then
                MsgBox(E0004 + Me.GetNextControl(cmbCreditCardType, False).Text, MsgBoxStyle.Information)
                cmbCreditCardType.Focus()
                Exit Sub
            ElseIf e.KeyChar = Chr(Keys.Enter) Then
                If Val(txtCrAmount_AMT.Text) = 0 Then
                    MsgBox(Me.GetNextControl(txtCrAmount_AMT, False).Text + E0001, MsgBoxStyle.Information)
                    txtCreditCardAmount_AMT.Focus()
                    Exit Sub
                End If
            End If
            If txtCreditCardRowIndex.Text = "" Then
                Dim ro As DataRow = Nothing
                ro = dtGridCreditCard.NewRow
                ro("CARDTYPE") = cmbCreditCardType.Text
                ro("CRCURAMT") = Val(txtCr_AMT.Text)
                ro("CRCONVERT") = Val(txtCrCon_AMT.Text)
                ro("CRAMOUNT") = Val(txtCrAmount_AMT.Text)
                ro("CRREMARKS") = txtCrRemark.Text
                dtGridCreditCard.Rows.Add(ro)
            Else
                With gridCreditCard.Rows(Val(txtCreditCardRowIndex.Text))
                    .Cells("CARDTYPE").Value = cmbCreditCardType.Text
                    .Cells("CRCURAMT").Value = Val(txtCr_AMT.Text)
                    .Cells("CRCONVERT").Value = Val(txtCrCon_AMT.Text)
                    .Cells("CRAMOUNT").Value = Val(txtCrAmount_AMT.Text)
                    .Cells("CRREMARKS").Value = txtCrRemark.Text
                End With
            End If
            dtGridCreditCard.AcceptChanges()
            CalcGridCreditCardTotal()
            gridCreditCard.CurrentCell = gridCreditCard.Rows(gridCreditCard.RowCount - 1).Cells("CARDTYPE")

            objGPack.TextClear(grpCreditCard)
            cmbCreditCardType.Select()
        End If
    End Sub

    Private Sub txtCr_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCr_AMT.TextChanged
        txtCrAmount_AMT.Text = Format(Val(txtCr_AMT.Text) * Val(txtCrCon_AMT.Text), "#0.00")
    End Sub

    Private Sub txtCrCon_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCrCon_AMT.TextChanged
        txtCrAmount_AMT.Text = Format(Val(txtCr_AMT.Text) * Val(txtCrCon_AMT.Text), "#0.00")
    End Sub
End Class