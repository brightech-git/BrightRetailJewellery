Imports System.Data.OleDb
Public Class W_PendingBillDiscount
    Dim StrSql As String
    Dim Da As OleDbDataAdapter
    Dim Cmd As OleDbCommand
    Dim DtGrid As DataTable
    Dim strDiscTable As String
    Private Sub W_PendingBillDiscount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Try
            If e.KeyChar = Chr(Keys.Enter) Then
                'If gridView.Focused Then
                '    Exit Sub
                'End If
                SendKeys.Send("{TAB}")
            ElseIf e.KeyChar = Chr(Keys.Escape) Then
                Me.DialogResult = Windows.Forms.DialogResult.Cancel
                Me.Close()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub W_PendingBillDiscount_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            'If Not gridView.RowCount > 0 Then Me.Close()
           
            cmbDiscountType.Items.Add("TAKE AMOUNT")
            cmbDiscountType.Items.Add("TAKE WEIGHT")

            cmbDiscountType.SelectedIndex = 0

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Public Sub New(ByVal BillNo As Integer, ByVal BillDate As Date, ByVal strTabName As String)
        Try
            ' This call is required by the Windows Form Designer.
            InitializeComponent()
            ' Add any initialization after the InitializeComponent() call.
            strDiscTable = strTabName
            StrSql = " Select * from " & cnStockDb & ".." & strTabName & " where TranNo=" & BillNo & " and TranDate='" & Format(BillDate, "yyyy-MM-dd") & "'"
            DtGrid = New DataTable
            Da = New OleDbDataAdapter(StrSql, cn)
            Da.Fill(DtGrid)
            gridView.DataSource = DtGrid
            With gridView
                .Columns("TRANDATE").Visible = False
                .Columns("TRANNO").Visible = False
                .Columns("ITEMID").Visible = False
                .Columns("PCS").Visible = False
                .Columns("MODE").Visible = False
                .Columns("DISVAL").Visible = False

                .Columns("ITEMNAME").Width = 100
                .Columns("ITEMNAME").SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns("ITEMNAME").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .Columns("ITEMNAME").ReadOnly = True

                .Columns("TAGNO").Width = 60
                .Columns("TAGNO").SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns("TAGNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("TAGNO").ReadOnly = True


                .Columns("GRSWT").Width = 60
                .Columns("GRSWT").SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("GRSWT").ReadOnly = True

                .Columns("NETWT").Width = 60
                .Columns("NETWT").SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("NETWT").ReadOnly = True

                .Columns("PUREWT").Width = 60
                .Columns("PUREWT").SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns("PUREWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("PUREWT").ReadOnly = True

                .Columns("TOUCH").Width = 60
                .Columns("TOUCH").SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns("TOUCH").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("TOUCH").ReadOnly = True

                .Columns("RATE").Width = 60
                .Columns("RATE").SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns("RATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("RATE").ReadOnly = True

                .Columns("AMOUNT").Width = 60
                .Columns("AMOUNT").SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("AMOUNT").ReadOnly = True

                .Columns("DISPER").Width = 60
                .Columns("DISPER").SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns("DISPER").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("DISPER").ReadOnly = False

                .Columns("DISPURE").Width = 60
                .Columns("DISPURE").SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns("DISPURE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("DISPURE").ReadOnly = True

                .Columns("NETPURE").Width = 60
                .Columns("NETPURE").SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns("NETPURE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("NETPURE").ReadOnly = True

            End With
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub
    
    Private Sub btnOk_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Try
            For Each row As DataGridViewRow In gridView.Rows
                StrSql = "Update  " & cnStockDb & ".." & strDiscTable & " set DISPURE=" & Val(row.Cells("DISPURE").ToString)
                StrSql += vbCrLf + " ,DISPER=" & Val(row.Cells("DISPURE").ToString)
                StrSql += vbCrLf + " ,NETPURE=" & Val(row.Cells("NETPURE").ToString)
                If cmbDiscountType.Text = "TAKE AMOUNT" Then
                    StrSql += vbCrLf + ",DISVAL=" & Val(txtDisAmount.Text)
                    StrSql += vbCrLf + ",MODE='A'"
                Else
                    StrSql += vbCrLf + ",DISVAL=" & Val(txtDisPure.Text)
                    StrSql += vbCrLf + ",MODE='W'"
                End If
                StrSql += vbCrLf + " where Tranno=" & row.Cells("Tranno").Value
                StrSql += vbCrLf + " and TagNo='" & row.Cells("TagNo").Value.ToString & "'"
                StrSql += vbCrLf + " and TranDate='" & Format(row.Cells("TranDate").Value, "yyyy-MM-dd") & "'"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                Cmd.ExecuteNonQuery()
            Next
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub btnCancel_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Try
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
            Me.Close()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub gridView_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.CellEndEdit
        Try
            Dim RowIdx As Integer
            Dim dblNetWt As Double
            Dim dblDisPer As Double
            Dim dblPure As Double
            Dim dblDisPure As Double
            Dim dblNetPure As Double
            Dim dblRate As Double

            RowIdx = gridView.CurrentRow.Index
            With gridView
                If .Rows.Count > 0 Then

                    dblNetWt = Val(.Rows(RowIdx).Cells("NETWT").Value)
                    dblDisPer = Val(.Rows(RowIdx).Cells("DISPER").Value)
                    dblPure = Val(.Rows(RowIdx).Cells("PUREWT").Value)
                    dblRate = Val(.Rows(RowIdx).Cells("RATE").Value)

                    dblDisPure = Format(dblNetWt * dblDisPer / 100, "0.000")
                    dblNetPure = Format(dblPure - dblDisPure, "0.000")

                    .Rows(RowIdx).Cells("DISPURE").Value = dblDisPure
                    .Rows(RowIdx).Cells("NETPURE").Value = dblNetPure
                    
                    txtDisPure.Text = DtGrid.Compute("Sum(DISPURE)", "").ToString
                    txtDisAmount.Text = Format(Val(DtGrid.Rows(0).Item("Rate").ToString) * Val(txtDisPure.Text), "0.00")
                End If
            End With
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub gridView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.GotFocus
        Try
            If gridView.RowCount > 0 Then
                gridView.CurrentCell = gridView.Rows(0).Cells("DisPer")
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub gridView_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.CellValueChanged

    End Sub
End Class