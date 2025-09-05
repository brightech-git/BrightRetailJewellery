Public Class frmwGridDispDia
    Public dsGrid As New DataSet
    Public dtFilteration As New DataTable ''This table has 2 columns. one is filter caption and another one is filter text
    Public FormReLocation As Boolean = True
    Public FormReSize As Boolean = True
    Public WithEvents TxtBox As New TextBox 'this control helps the user to raise events like if user press d or ...
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
        tStripExcel.Image = My.Resources.Excel_icon_22
        tStripPrint.Image = My.Resources.Printer_22
    End Sub

    Public Sub gridView_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs) Handles gridView.ColumnWidthChanged
        If FormReSize Then
            Dim wid As Double = Nothing
            For cnt As Integer = 0 To gridView.ColumnCount - 1
                If gridView.Columns(cnt).Visible Then
                    wid += gridView.Columns(cnt).Width
                End If
            Next
            wid += 10
            If CType(gridView.Controls(1), VScrollBar).Visible Then
                wid += 20
            End If
            wid += 20
            If wid > ScreenWid Then
                wid = ScreenWid
            End If
            Me.Size = New Size(wid, Me.Height)
        End If
        If FormReLocation Then SetCenterLocation(Me)
    End Sub

    Private Sub gridView_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles gridView.DataError

    End Sub

    Private Sub tStripExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripExcel.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export, IIf(gridViewHeader.Visible, gridViewHeader, Nothing))
        End If
    End Sub

    Private Sub tStripPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print, IIf(gridViewHeader.Visible, gridViewHeader, Nothing))
        End If
    End Sub

    Private Sub frmwGridDispDia_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'SetCenterLocation(Me)
        gridView.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
        gridViewHeader.Height = gridViewHeader.ColumnHeadersHeight
        gridViewSubHeader.Height = gridViewSubHeader.ColumnHeadersHeight
    End Sub

    Private Sub tStripExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
        End If
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If UCase(e.KeyChar) = "X" Then
            tStripExcel_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = "P" Then
            tStripPrint_Click(Me, New EventArgs)
        ElseIf e.KeyChar = Chr(Keys.Enter) Then
            e.Handled = True
            If gridView.RowCount > 0 Then
                Dim ind As Integer = gridView.FirstDisplayedCell.ColumnIndex
                If gridView.CurrentRow Is Nothing Then Exit Sub
                gridView.CurrentCell = gridView.Rows(gridView.CurrentRow.Index).Cells(gridView.CurrentCell.ColumnIndex)
            End If
        End If
    End Sub
    Public Sub SetFilterStr(ByVal filterColumnName As String, ByVal setValue As Object)
        If Not dtFilteration.Columns.Contains(filterColumnName) Then Exit Sub
        If Not dtFilteration.Rows.Count > 0 Then Exit Sub
        dtFilteration.Rows(0).Item(filterColumnName) = setValue
    End Sub

    Public Function GetFilterStr(ByVal filterColumnName As String) As String
        Dim ftrStr As String = Nothing
        If Not dtFilteration.Columns.Contains(filterColumnName) Then Return ftrStr
        If Not dtFilteration.Rows.Count > 0 Then Return ftrStr
        Return dtFilteration.Rows(0).Item(filterColumnName).ToString
        Return ftrStr
    End Function

    Private Sub gridView_Scroll(ByVal sender As Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles gridView.Scroll
        Try
            If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
                gridViewHeader.HorizontalScrollingOffset = e.NewValue
                If gridViewHeader.Columns.Count > 0 Then
                    If gridViewHeader.Columns.Contains("SCROLL") Then
                        gridViewHeader.Columns("SCROLL").Visible = CType(gridView.Controls(1), VScrollBar).Visible
                        gridViewHeader.Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
                    End If
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try
    End Sub
End Class