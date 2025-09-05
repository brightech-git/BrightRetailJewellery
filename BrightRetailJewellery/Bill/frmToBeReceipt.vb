Imports System.Data.OleDb
Public Class frmToBeReceipt
    Public dtToBeReceipt As New DataTable
    Public Sub New(ByVal Start As Boolean)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

    End Sub
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)

        Dim col As New DataColumn
        col.ColumnName = "CHECK"
        col.DataType = GetType(Boolean)
        col.DefaultValue = False
        With dtToBeReceipt
            .Columns.Add(col)
            .Columns.Add("ITEM", GetType(String))
            .Columns.Add("PCS", GetType(Integer))
            .Columns.Add("GRSWT", GetType(Decimal))
            .Columns.Add("NETWT", GetType(Decimal))
            .Columns.Add("VALUE", GetType(Double))
            .Columns.Add("REMARK", GetType(String))
            .Columns.Add("RUNNO", GetType(String))
            .Columns.Add("ISSSNO", GetType(String))
        End With
        gridView.DataSource = dtToBeReceipt
        StyleGridTobeReceipt()
        For CNT As Integer = 0 To 4
            dtToBeReceipt.Rows.Add()
        Next
        dtToBeReceipt.AcceptChanges()
    End Sub

    Private Sub frmToBeReceipt_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            dtToBeReceipt.AcceptChanges()
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Public Sub StyleGridTobeReceipt()
        With gridView
            .Columns("CHECK").Width = 25
            .Columns("CHECK").HeaderText = ""
            .Columns("CHECK").DefaultCellStyle.DataSourceNullValue = True
            .Columns("CHECK").ReadOnly = False
            .Columns("ITEM").Width = 147
            .Columns("PCS").Width = 62
            .Columns("GRSWT").Width = 75
            .Columns("NETWT").Width = 75
            .Columns("VALUE").Width = 97
            .Columns("REMARK").Width = 146
            .Columns("RUNNO").Visible = False
            .Columns("ISSSNO").Visible = False
            For cnt As Integer = 1 To gridView.Columns.Count - 1
                gridView.Columns(cnt).ReadOnly = True
            Next
            .Columns("REMARK").ReadOnly = False
        End With
    End Sub

    Private Sub frmToBeReceipt_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        gridView.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
    End Sub

    Private Sub gridView_CurrentCellDirtyStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.CurrentCellDirtyStateChanged
        gridView.CommitEdit(DataGridViewDataErrorContexts.Commit)
        gridView.EndEdit()
    End Sub
End Class