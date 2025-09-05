Imports System.Data.OleDb
Public Class Denomination
    'Dim cn As New OleDbConnection("PROVIDER = SQLOLEDB.1;PERSIST SECURITY INFO = FALSE;INITIAL CATALOG=;DATA SOURCE = GIRITECH99\GIRITECH99;USER ID=SA;PASSWORD =;")
    Dim da As OleDbDataAdapter
    Dim strSql As String
    'Dim CnAdmindb As String = "" & CNADMINDB & ""
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        'cn.Open()
        ' Add any initialization after the InitializeComponent() call.
        strSql = " SELECT CONVERT(VARCHAR,DEN_VALUE)DEN_VALUE,'X'as sep,CONVERT(NUMERIC(15,2),NULL) AS QTY,CONVERT(NUMERIC(15,2),NULL)AMOUNT,DEN_ID FROM " & cnAdminDb & "..DENOMMAST ORDER BY DEN_ORDER"
        Dim dtGridRec As New DataTable
        Dim dtGridPay As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGridRec)
        da.Fill(dtGridPay)
        ''Receipt
        gridView.DataSource = dtGridRec
        GridStyle(gridView)


        dtGridRec.Rows.Add()
        dtGridRec.Rows(dtGridRec.Rows.Count - 1).Item("DEN_VALUE") = "TOTAL"

        Dim dtGridBalance As DataTable
        dtGridBalance = dtGridRec.Clone
        gridTotal.DataSource = dtGridBalance
        dtGridBalance.Rows.Add()
        dtGridBalance.Rows(0).Item("DEN_VALUE") = "BALANCE"
        GridStyle(gridTotal)

        Dim dtGridRecHeader As New DataTable
        dtGridRecHeader.Columns.Add("HEAD", GetType(String))
        gridViewHeader.DataSource = dtGridRecHeader
        GridHeadStyle(gridViewHeader)
    End Sub

    Private Sub GridHeadStyle(ByVal Dgv As DataGridView)
        With Dgv
            .BorderStyle = BorderStyle.None
            .ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Sunken
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .RowHeadersVisible = False
            .AllowUserToResizeRows = False
            .AllowUserToResizeColumns = False
            .RowTemplate.Height = txt.Height
            .Enabled = False
            .Columns("HEAD").HeaderText = "RECEIPT"
            .Columns("HEAD").Width = gridView.Columns("DEN_VALUE").Width + gridView.Columns("SEP").Width + gridView.Columns("QTY").Width + gridView.Columns("AMOUNT").Width
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        End With
    End Sub

    Private Sub GridStyle(ByVal Dgv As DataGridView)
        With Dgv
            For cnt As Integer = 0 To Dgv.ColumnCount - 1
                .Columns(cnt).SortMode = DataGridViewColumnSortMode.NotSortable
            Next
            .BorderStyle = BorderStyle.None
            .ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Sunken
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .RowHeadersVisible = False
            .AllowUserToResizeRows = False
            .AllowUserToResizeColumns = False
            .RowTemplate.Height = txt.Height
            .Columns("DEN_ID").Visible = False
            .Columns("DEN_VALUE").Width = 120
            .Columns("SEP").Width = 20
            .Columns("QTY").Width = 80
            .Columns("AMOUNT").Width = 120

            .Columns("DEN_VALUE").HeaderText = "DENOMINATION"
            .Columns("SEP").HeaderText = ""
            .Columns("QTY").HeaderText = "No's"
            .Columns("AMOUNT").HeaderText = "AMOUNT"

            .Columns("DEN_VALUE").DefaultCellStyle.Format = "0.00"
            .Columns("AMOUNT").DefaultCellStyle.Format = "0.00"


            .Columns("DEN_VALUE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("QTY").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("SEP").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        End With
    End Sub


    Private Sub gridView_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.CellEnter
        Dim gridViewPt As Point = gridView.Location
        Select Case gridView.Columns(e.ColumnIndex).Name
            Case "QTY"
                txt.Visible = True
                txt.Size = gridView.GetCellDisplayRectangle(gridView.CurrentCell.ColumnIndex, gridView.CurrentCell.RowIndex, True).Size
                txt.Location = gridView.GetCellDisplayRectangle(gridView.CurrentCell.ColumnIndex, gridView.CurrentCell.RowIndex, True).Location + gridViewPt
                txt.Text = gridView.CurrentCell.Value.ToString
                txt.BringToFront()
                txt.Select()
                txt.SelectAll()
        End Select
    End Sub

    Private Sub gridView_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.CellLeave
        If gridView.Columns(e.ColumnIndex).Name = "QTY" Then
            gridView.CurrentCell.Value = IIf(Val(txt.Text) <> 0, Val(txt.Text), DBNull.Value)
            CalcAmount()
        End If
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            SetCurrentRow()
        End If
    End Sub

    Private Sub SetCurrentRow()
        If gridView.CurrentRow Is Nothing Then
            gridView.Select()
            Exit Sub
        End If
        If Not gridView.RowCount > 0 Then Exit Sub
        If gridView.CurrentRow Is Nothing Then Exit Sub
        If gridView.CurrentRow.Index = gridView.RowCount - 1 And gridView.RowCount > 1 Then
            gridView.CurrentCell = gridView.Rows(gridView.CurrentRow.Index - 1).Cells("QTY")
        End If
        If gridView.CurrentCell.ColumnIndex <> gridView.Columns("QTY").Index Then
            gridView.CurrentCell = gridView.Rows(gridView.CurrentRow.Index).Cells("QTY")
        End If
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SetCurrentRow()
        End If
    End Sub

    Private Sub gridView_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.SelectionChanged
        SetCurrentRow()
    End Sub

    Private Sub txt_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt.KeyDown
        If e.KeyCode = Keys.Up Then
            e.Handled = True
            If gridView.CurrentRow.Index <> 0 Then
                gridView.CurrentCell = gridView.Rows(gridView.CurrentRow.Index - 1).Cells("QTY")
            End If
        ElseIf e.KeyCode = Keys.Down Then
            e.Handled = True
            If gridView.CurrentRow.Index + 1 <= gridView.RowCount - 2 Then
                gridView.CurrentCell = gridView.Rows(gridView.CurrentRow.Index + 1).Cells("QTY")
            End If
        End If
    End Sub

    Private Function GetTotalEnteredAmount() As Decimal
        Dim totAmt As Decimal = Nothing
        totAmt = Val(CType(gridView.DataSource, DataTable).Compute("SUM(AMOUNT)", "DEN_VALUE <> 'TOTAL'").ToString)
        Return totAmt
    End Function

    Private Sub CalcAmount()
        Dim amt As Decimal = 0
        amt = IIf(Val(gridView.CurrentRow.Cells("DEN_VALUE").Value.ToString) <> 0, Val(gridView.CurrentRow.Cells("DEN_VALUE").Value.ToString), 1)
        amt *= Val(gridView.CurrentCell.Value.ToString)
        gridView.CurrentRow.Cells("AMOUNT").Value = IIf(amt, Format(amt, "0.00"), DBNull.Value)

        Dim totAmt As Decimal = GetTotalEnteredAmount()
        gridView.Rows(gridView.RowCount - 1).Cells("AMOUNT").Value = IIf(totAmt <> 0, Format(totAmt, "0.00"), DBNull.Value)
        gridTotal.Rows(0).Cells("AMOUNT").Value = IIf(Val(txtBillAmount.Text) - totAmt <> 0, Format((Val(txtBillAmount.Text) - totAmt), "0.00"), DBNull.Value)
    End Sub

    Private Sub txt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            gridView.CurrentCell.Value = IIf(Val(txt.Text) <> 0, Val(txt.Text), DBNull.Value)
            CalcAmount()
ff:
            ''Set Focus
            If gridView.CurrentRow.Index + 1 <= gridView.RowCount - 2 Then
                gridView.CurrentCell = gridView.Rows(gridView.CurrentRow.Index + 1).Cells("QTY")
            End If

            Dim balAmt As Decimal = Val(txtBillAmount.Text) - GetTotalEnteredAmount()
            Dim nos As Decimal = balAmt / IIf(Val(gridView.CurrentRow.Cells("DEN_VALUE").Value.ToString) <> 0, Val(gridView.CurrentRow.Cells("DEN_VALUE").Value.ToString), 1)
            If nos = 0 And gridView.CurrentRow.Index + 1 <= gridView.RowCount - 2 Then
                GoTo ff
            End If
            If Val(gridView.CurrentRow.Cells("DEN_VALUE").Value.ToString) > Math.Abs(balAmt) And gridView.CurrentRow.Index + 1 <= gridView.RowCount - 2 Then
                GoTo ff
            End If
            'If balAmt < 0 Then
            '    nos = balAmt / Val(gridView.CurrentRow.Cells("DEN_VALUE").Value.ToString)
            'End If
            lblSugAmt.Text = IIf(nos < 0, Math.Ceiling(nos), Math.Floor(nos))
            If balAmt <= 0 Then
                If MessageBox.Show("Do you want to save", "Save Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                    Me.DialogResult = Windows.Forms.DialogResult.OK
                    Me.Close()
                End If
            End If
        End If
    End Sub

    Private Sub txt_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt.GotFocus
        txt.BackColor = Color.LightGoldenrodYellow
        txt.SelectAll()
    End Sub

    Private Sub Denomination_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        ElseIf e.KeyCode = Keys.F3 Then
            For Each dgvRow As DataGridViewRow In gridView.Rows
                dgvRow.Cells("QTY").Value = DBNull.Value
                dgvRow.Cells("AMOUNT").Value = DBNull.Value
            Next
            gridView.CurrentCell = gridView.Rows(0).Cells("QTY")
            If Val(txtBillAmount.Text) < 2000 Then
                txt_KeyPress(Me, New KeyPressEventArgs(Chr(Keys.Enter)))
            Else
                Dim balAmt As Decimal = Val(txtBillAmount.Text)
                Dim nos As Decimal = balAmt / IIf(Val(gridView.CurrentRow.Cells("DEN_VALUE").Value.ToString) <> 0, Val(gridView.CurrentRow.Cells("DEN_VALUE").Value.ToString), 1)
                lblSugAmt.Text = IIf(nos < 0, Math.Ceiling(nos), Math.Floor(nos))
            End If
        End If
    End Sub

    Private Sub Denomination_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        gridView.Rows(gridView.RowCount - 1).DefaultCellStyle.BackColor = Color.LightBlue
        gridView.Rows(gridView.RowCount - 1).DefaultCellStyle.SelectionBackColor = Color.LightBlue
        gridTotal.Rows(0).DefaultCellStyle.BackColor = Color.LightBlue
        gridTotal.Rows(0).DefaultCellStyle.SelectionBackColor = Color.LightBlue
        gridTotal.Rows(0).DefaultCellStyle.SelectionForeColor = Color.Black
        If Val(txtBillAmount.Text) < 2000 Then
            txt_KeyPress(Me, New KeyPressEventArgs(Chr(Keys.Enter)))
        Else
            Dim balAmt As Decimal = Val(txtBillAmount.Text)
            Dim nos As Decimal = balAmt / IIf(Val(gridView.CurrentRow.Cells("DEN_VALUE").Value.ToString) <> 0, Val(gridView.CurrentRow.Cells("DEN_VALUE").Value.ToString), 1)
            lblSugAmt.Text = IIf(nos < 0, Math.Ceiling(nos), Math.Floor(nos))
        End If
    End Sub

    Public Sub InsertDenomTran(ByVal batchno As String, ByVal billdate As Date, ByVal billcostid As String, ByVal tran As OleDbTransaction)
        For Each DgvRow As DataGridViewRow In gridView.Rows
            If DgvRow.Cells("DEN_VALUE").Value.ToString = "TOTAL" Then Continue For
            If Val(DgvRow.Cells("AMOUNT").Value.ToString) = 0 Then Continue For
            strSql = vbCrLf + " INSERT INTO " & cnStockDb & "..DENOMTRAN"
            strSql += vbCrLf + " ("
            strSql += vbCrLf + " BATCHNO,TRANDATE,DEN_ID,DEN_QTY,DEN_AMOUNT"
            strSql += vbCrLf + " )"
            strSql += vbCrLf + " VALUES"
            strSql += vbCrLf + " ("
            strSql += vbCrLf + " '" & batchno & "'"
            strSql += vbCrLf + " ,'" & billdate.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ," & Val(DgvRow.Cells("DEN_ID").Value.ToString) & ""
            strSql += vbCrLf + " ," & Val(DgvRow.Cells("QTY").Value.ToString) & ""
            strSql += vbCrLf + " ," & Val(DgvRow.Cells("AMOUNT").Value.ToString) & ""
            strSql += vbCrLf + " )"
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, billcostid)
        Next
    End Sub
End Class