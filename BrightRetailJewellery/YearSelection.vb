Public Class YearSelection
    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            gridView.CurrentCell = gridView.Rows(gridView.CurrentRow.Index).Cells(0)
        ElseIf e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            Dim tranYear As String = gridView.Rows(gridView.CurrentCell.RowIndex).Cells("TRANYEAR").Value.ToString
            For Each ro As DataRow In CType(gridView.DataSource, DataTable).Rows
                If ro.Item("TRANYEAR").ToString = tranYear Then
                    cnStockDb = ro.Item("DBNAME").ToString
                    BrighttechREPORT.cnStockDb = ro.Item("DBNAME").ToString
                    BrighttechMaster.cnStockDb = ro.Item("DBNAME").ToString
                    cnTranFromDate = ro.Item("STARTDATE").ToString
                    BrighttechREPORT.cnTranFromDate = ro.Item("STARTDATE").ToString
                    BrighttechMaster.cnTranFromDate = ro.Item("STARTDATE").ToString
                    cnTranToDate = ro.Item("ENDDATE").ToString
                    BrighttechREPORT.cnTranToDate = ro.Item("ENDDATE").ToString
                    BrighttechMaster.cnTranToDate = ro.Item("ENDDATE").ToString
                    Main.TransactionYearTStripLabel.Text = tranYear
                    objGPack = New BrighttechPack.Methods(cn, cnAdminDb, LangId _
                    , focusColor, lostFocusColor, Radio_Check_LostFocusColor _
                    , Button_LostFocusColor, grdBackGroundColor, bakImage _
                    , textCharacterCasing, cnTranFromDate, cnTranToDate, CurrencyDecimal)
                    Me.DialogResult = Windows.Forms.DialogResult.OK
                    Me.Close()
                    Exit For
                End If
            Next
        End If
    End Sub

    Private Sub YearSelection_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim dt As New DataTable
        dt = _DtTransactionYear.Copy
        gridView.DataSource = dt
        gridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        gridView.Columns("DBNAME").Visible = False
        gridView.Columns("STARTDATE").Visible = False
        gridView.Columns("ENDDATE").Visible = False
        gridView.Columns("TRANYEAR").HeaderText = "TRANYEAR DETAIL"
        For CNT As Integer = 0 To gridView.RowCount - 1
            If cnStockDb = gridView.Rows(CNT).Cells("DBNAME").Value.ToString Then
                gridView.CurrentCell = gridView.Rows(CNT).Cells("TRANYEAR")
            End If
        Next
    End Sub
End Class