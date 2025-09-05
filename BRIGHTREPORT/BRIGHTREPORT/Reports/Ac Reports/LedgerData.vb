Partial Class LedgerData
    Partial Class dtLedgerDataTable

        Private Sub dtLedgerDataTable_ColumnChanging(ByVal sender As System.Object, ByVal e As System.Data.DataColumnChangeEventArgs) Handles Me.ColumnChanging
            If (e.Column.ColumnName = Me.columnCONTRA.ColumnName) Then
                'Add user code here
            End If

        End Sub

    End Class

End Class
