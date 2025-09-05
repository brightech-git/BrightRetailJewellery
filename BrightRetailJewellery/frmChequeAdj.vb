Imports System.Data.OleDb
Public Class frmChequeAdj
    Dim strSql As String
    Public dtGridCheque As New DataTable
    Public PayBank As Boolean = False
    Public Getchqno As Boolean = False
    Public ChqCostid As String = Nothing

    Public Sub New(ByVal Start As Boolean)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
    End Sub
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        objGPack.Validator_Object(Me)
        objGPack.TextClear(Me)
        gridCheque.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        gridChequeTotal.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)

        Dim IsCardbank As String = GetAdmindbSoftValue("POS_INCL_CCBANK", "N")
        ' Add any initialization after the InitializeComponent() call.
        ''CHEQUE

        'strSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACGRPCODE = 2"
        'strSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACGRPCODE in"
        'strSql += " (SELECT ACGRPCODE FROM " & cnAdminDb & "..ACGROUP WHERE ACMAINCODE = 2)"
        strSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ISNULL(ACTYPE,'O') ='B'"
        If IsCardbank = "N" Then strSql += " AND ACCODE NOT IN (SELECT ACCODE FROM " & cnAdminDb & "..CREDITCARD)"
        strSql += " AND ISNULL(ACTIVE,'') <> 'N' "
        strSql += " ORDER BY ACNAME "
        objGPack.FillCombo(strSql, cmbChequeDefaultName)
        strSql = vbCrLf + " SELECT "
        strSql += vbCrLf + " (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = S.CTLTEXT)DEF"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..SOFTCONTROL S WHERE "
        If PayBank Then strSql += vbCrLf + "  CTLID = 'PAYBANK'" Else strSql += vbCrLf + "  CTLID = 'BANK'"
        cmbChequeDefaultName.Text = objGPack.GetSqlValue(strSql, , "BANK")

        strSql = " SELECT MODENAME FROM " & cnAdminDb & "..PAYMENTMODE "
        strSql += " ORDER BY MODENAME "
        objGPack.FillCombo(strSql, cmbMode)

        With dtGridCheque
            .Columns.Add("BANKNAME", GetType(String))
            .Columns.Add("DATE", GetType(Date))
            .Columns.Add("CHEQUENO", GetType(String))
            .Columns.Add("AMOUNT", GetType(Double))
            .Columns.Add("DEFAULTBANK", GetType(String))
            .Columns.Add("MODE", GetType(String))
        End With
        gridCheque.DataSource = dtGridCheque
        FormatGridColumns(gridCheque)
        StyleGridCheque(gridCheque)
        Dim dtGridChequeTotal As New DataTable
        dtGridChequeTotal = dtGridCheque.Copy
        gridChequeTotal.DataSource = dtGridChequeTotal
        dtGridChequeTotal.Rows.Clear()
        dtGridChequeTotal.Rows.Add()
        dtGridChequeTotal.Rows(0).Item("BANKNAME") = "Total"
        With gridChequeTotal
            .DataSource = dtGridChequeTotal
            For Each col As DataGridViewColumn In gridCheque.Columns
                With gridChequeTotal.Columns(col.Name)
                    .Visible = col.Visible
                    .Width = col.Width
                    .DefaultCellStyle = col.DefaultCellStyle
                End With
            Next
        End With
        FormatGridColumns(gridChequeTotal)
        StyleGridCheque(gridChequeTotal)
    End Sub



    Private Sub StyleGridCheque(ByVal grid As DataGridView)
        gridChequeTotal.DefaultCellStyle.SelectionBackColor = grpCheque.BackgroundColor
        With grid
            .Columns("BANKNAME").Width = txtChequeBankName.Width + 1
            .Columns("DATE").Width = dtpChequeDate.Width + 1
            .Columns("CHEQUENO").Width = txtChequeNo.Width + 1
            .Columns("AMOUNT").Width = txtChequeAmount_AMT.Width + 1
            .Columns("DEFAULTBANK").Visible = False
            .Columns("MODE").Visible = False
        End With
    End Sub


    Private Sub frmChequeAdj_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            dtGridCheque.AcceptChanges()
            txtChequeBankName.Select()
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub frmChequeAdj_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtChequeAmount_AMT.Focused Then Exit Sub
            If cmbChequeDefaultName.Focused And Getchqno Then Call FindGetLastChqno()

            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub FindGetLastChqno()
        Dim bankcode As String = objGPack.GetSqlValue("select Accode from " & cnAdminDb & "..Achead where Acname = '" & cmbChequeDefaultName.Text & "'")
        strSql = "SELECT TOP 1 CHQNUMBER FROM " & cnStockDb & "..CHEQUEBOOK WHERE BANKCODE='" & bankcode & "' AND CHQISSUEDATE IS NULL"
            If ChqCostid <> Nothing Then strSql += "  AND ISNULL(COSTID,'') = '" & ChqCostid & "'"
        strSql += "  order by chqnumber"
        txtChequeNo.Text = objGPack.GetSqlValue(strSql).ToString
        End Sub
    Private Sub txtChequeAmount_AMT_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtChequeAmount_AMT.KeyDown
        If e.KeyCode = Keys.Down Then
            If gridCheque.RowCount > 0 Then gridCheque.Select()
        End If
    End Sub
    Private Sub txtChequeAmount_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtChequeAmount_AMT.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If cmbChequeDefaultName.Text = "" Then
                MsgBox(Me.GetNextControl(cmbChequeDefaultName, False).Text + E0001, MsgBoxStyle.Information)
                cmbChequeDefaultName.Focus()
                Exit Sub
            ElseIf Val(txtChequeAmount_AMT.Text) = 0 Then
                MsgBox(Me.GetNextControl(txtChequeAmount_AMT, False).Text + E0001, MsgBoxStyle.Information)
                txtChequeAmount_AMT.Focus()
                Exit Sub
            End If
            If txtChequeRowIndex.Text = "" Then
                Dim ro As DataRow = Nothing
                ro = dtGridCheque.NewRow
                ro("BANKNAME") = txtChequeBankName.Text
                ro("DATE") = dtpChequeDate.Value.Date.ToString("yyyy-MM-dd")
                ro("CHEQUENO") = txtChequeNo.Text
                ro("AMOUNT") = IIf(Val(txtChequeAmount_AMT.Text) <> 0, Val(txtChequeAmount_AMT.Text), DBNull.Value)
                ro("DEFAULTBANK") = cmbChequeDefaultName.Text
                ro("MODE") = cmbMode.Text.ToString()
                dtGridCheque.Rows.Add(ro)
            Else
                With gridCheque.Rows(Val(txtChequeRowIndex.Text))
                    .Cells("BANKNAME").Value = txtChequeBankName.Text
                    .Cells("DATE").Value = dtpChequeDate.Value.Date.ToString("yyyy-MM-dd")
                    .Cells("CHEQUENO").Value = txtChequeNo.Text
                    .Cells("AMOUNT").Value = IIf(Val(txtChequeAmount_AMT.Text) <> 0, Val(txtChequeAmount_AMT.Text), DBNull.Value)
                    .Cells("DEFAULTBANK").Value = cmbChequeDefaultName.Text
                    .Cells("MODE").Value = cmbMode.Text.ToString()
                End With
            End If
            CalcGridChequeTotal()
            Dim defaultBank As String = cmbChequeDefaultName.Text
            objGPack.TextClear(grpCheque)
            dtpChequeDate.Value = GetServerDate()
            cmbChequeDefaultName.Text = defaultBank
            txtChequeBankName.Select()
        End If
    End Sub

    Private Sub txtChequeBankName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtChequeBankName.GotFocus
        txtChequeBankName.Text = "" 'cmbChequeDefaultName.Text
        txtChequeBankName.SelectAll()
    End Sub
    Private Sub txtChequeBankName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtChequeBankName.KeyDown
        If e.KeyCode = Keys.Down Then
            If gridCheque.RowCount > 0 Then gridCheque.Select()
        End If
    End Sub
    Private Sub txtChequeNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtChequeNo.KeyDown
        If e.KeyCode = Keys.Down Then
            If gridCheque.RowCount > 0 Then gridCheque.Select()
        End If
    End Sub
    Public Sub CalcGridChequeTotal()
        dtGridCheque.AcceptChanges()
        Dim amt As Double = Nothing
        For Each ro As DataRow In dtGridCheque.Rows
            amt += Val(ro!AMOUNT.ToString)
        Next
        gridChequeTotal.Rows(0).Cells("AMOUNT").Value = IIf(amt <> 0, amt, DBNull.Value)
    End Sub

    Private Sub gridCheque_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridCheque.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            If gridCheque.RowCount > 0 Then
                gridCheque.CurrentCell = gridCheque.Rows(gridCheque.CurrentRow.Index).Cells("BANKNAME")
                With dtGridCheque.Rows(gridCheque.CurrentRow.Index)
                    txtChequeBankName.Text = .Item("BANKNAME").ToString
                    dtpChequeDate.Value = .Item("DATE")
                    txtChequeNo.Text = .Item("CHEQUENO").ToString
                    txtChequeAmount_AMT.Text = IIf(Val(.Item("AMOUNT").ToString) <> 0, Format(Val(.Item("AMOUNT").ToString), "0.00"), Nothing)
                    cmbChequeDefaultName.Text = .Item("DEFAULTBANK").ToString
                    cmbMode.Text = .Item("MODE").ToString
                    txtChequeRowIndex.Text = gridCheque.CurrentRow.Index
                    txtChequeBankName.Select()
                End With
            End If
        End If
    End Sub

    Private Sub gridCheque_UserDeletedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles gridCheque.UserDeletedRow
        dtGridCheque.AcceptChanges()
        CalcGridChequeTotal()
        If Not gridCheque.RowCount > 0 Then
            txtChequeBankName.Select()
        End If
    End Sub

    Private Sub frmChequeAdj_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        dtpChequeDate.MinimumDate = (New DateTimePicker).MinDate
        dtpChequeDate.MaximumDate = (New DateTimePicker).MaxDate
        dtpChequeDate.Value = GetEntryDate(GetServerDate)
        cmbMode.SelectedIndex = 0
    End Sub

    Private Sub cmbChequeDefaultName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbChequeDefaultName.GotFocus
        strSql = vbCrLf + " SELECT "
        strSql += vbCrLf + " (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = S.CTLTEXT)DEF"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..SOFTCONTROL S WHERE "
        If PayBank Then strSql += vbCrLf + "  CTLID = 'PAYBANK'" Else strSql += vbCrLf + "  CTLID = 'BANK'"
        'CTLID = 'BANK'"
        cmbChequeDefaultName.Text = objGPack.GetSqlValue(strSql, , "BANK")
    End Sub
End Class