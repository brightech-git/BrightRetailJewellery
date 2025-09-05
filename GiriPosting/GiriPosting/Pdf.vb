Imports iTextSharp.text
Imports iTextSharp.text.pdf

Public Class Pdf
    Private _PdfTitle As String
    Private _SelectedColumnus As New List(Of String)
    Private _PageLeft As Single
    Private _PageRight As Single
    Private _PageBottom As Single
    Private _PageTop As Single
    Private _SavePath As String
    Private _FittoPageWidth As Boolean
    Private _Dgv As New DataGridView
    Private _DgvHeader As New DataGridView
    Private _Document As Document

    Private _FontNormal As Font = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 7, Font.NORMAL)
    Private _FontBold As Font = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12, Font.BOLD)
    Private _Width() As Single
    Private _CellColor As Boolean
    Private _Landscape As Boolean
    Private _HeaderForAllPages As Boolean
    Private _WithBorder As Boolean


    Public Sub Export(ByVal Dgv As DataGridView, ByVal Title As String _
    , ByVal Pdf_GetSelectedColumns As List(Of String) _
    , ByVal Pdf_DgvHeader As DataGridView _
    , ByVal Pdf_Title As String _
    , ByVal Pdf_PageLeft As Single _
    , ByVal Pdf_PageRight As Single _
    , ByVal Pdf_PageBottom As Single _
    , ByVal Pdf_PageTop As Single _
    , ByVal Pdf_SavePath As String _
    , ByVal Pdf_FitToPageWidth As Boolean _
    , ByVal Pdf_CellColor As Boolean _
    , ByVal Pdf_Landscape As Boolean _
    , ByVal Pdf_HeaderForAllPages As Boolean _
    , ByVal Pdf_WithBorder As Boolean _
    , ByVal Pdf_ResultMsg As Boolean
    )

        If Not Dgv.RowCount > 0 Then Exit Sub

        ' Saving some exporting attributes
        _Dgv = Dgv
        _DgvHeader = Pdf_DgvHeader
        _PdfTitle = Pdf_Title
        _SelectedColumnus = Pdf_GetSelectedColumns
        _PageLeft = Pdf_PageLeft
        _PageRight = Pdf_PageRight
        _PageBottom = Pdf_PageBottom
        _PageTop = Pdf_PageTop
        _SavePath = Pdf_SavePath
        _FittoPageWidth = Pdf_FitToPageWidth
        _CellColor = Pdf_CellColor
        _Landscape = Pdf_Landscape
        _HeaderForAllPages = Pdf_HeaderForAllPages
        _WithBorder = Pdf_WithBorder

        ReDim _Width(_SelectedColumnus.Count - 1)
        Dim i As Integer = 0
        For j As Integer = 0 To _Dgv.Columns.Count - 1
            If Not _Dgv.Columns(j).Visible Then Continue For
            If Not _SelectedColumnus.Contains(_Dgv.Columns(j).Name) Then Continue For
            _Width(i) = _Dgv.Columns(j).Width
            i += 1
        Next
        Export()
        If Pdf_ResultMsg Then MsgBox("Exported..")
    End Sub


    Private Sub Export()
        Dim wid As Integer
        Dim i As Integer = 0
        For j As Integer = 0 To _Dgv.Columns.Count - 1
            If Not _Dgv.Columns(j).Visible Then Continue For
            If Not _SelectedColumnus.Contains(_Dgv.Columns(j).Name) Then Continue For
            wid += _Dgv.Columns(j).Width
            i += 1
        Next
        wid += _PageLeft + _PageRight

        Dim pgRect As Rectangle = Nothing
        If _FittoPageWidth Then
            pgRect = PageSize.A4
        Else
            pgRect = New Rectangle(0, 0, wid, PageSize.A4.Height)
        End If
        If _Landscape Then pgRect.Rotate()
        _Document = New Document(pgRect, _PageLeft, _PageRight, _PageTop, _PageBottom)
        Dim Writer As PdfWriter = PdfWriter.GetInstance(_Document, New IO.FileStream(_SavePath, IO.FileMode.Create))

        _Document.Open()

        Dim foot As New HeaderFooter(New Phrase(Date.Now.ToString("dd/MM/yyyy") & "   Page : "), True)
        foot.Alignment = Element.ALIGN_LEFT
        foot.Border = Rectangle.NO_BORDER
        _Document.Footer = foot

        Dim _Table As Table = New Table(_SelectedColumnus.Count)
        _Table.Padding = 4
        _Table.Spacing = 0
        _Table.DefaultRowspan = 1
        _Table.DefaultCellBorderWidth = 1
        _Table.Widths = _Width
        Dim cel As New Cell(New Phrase(_PdfTitle, FontFactory.GetFont(FontFactory.HELVETICA, 8, FontStyle.Bold)))
        cel.Border = 0
        cel.HorizontalAlignment = Element.ALIGN_CENTER
        cel.Leading = 30
        cel.Colspan = _SelectedColumnus.Count
        cel.BackgroundColor = New Color(SystemColors.Control.ToArgb)
        _Table.AddCell(cel)

        If Not _WithBorder Then _Table.DefaultCellBorder = 0

        If _DgvHeader IsNot Nothing Then
            If _DgvHeader.ColumnCount > 0 Then
                For cnt As Integer = 0 To _DgvHeader.Columns.Count - 1
                    Dim GridCol As DataGridViewColumn = _DgvHeader.Columns(cnt)
                    If GridCol.Name.Contains("~") Then
                        Dim spCnt As Integer = 0
                        For Each colName As String In GridCol.Name.Split("~")
                            If Not _SelectedColumnus.Contains(colName) Then Continue For
                            spCnt += 1
                        Next
                        If spCnt > 0 Then
                            cel = New Cell(New Phrase(GridCol.HeaderText, FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD)))
                            cel.Header = True
                            cel.Colspan = spCnt
                            cel.HorizontalAlignment = Element.ALIGN_CENTER
                            cel.BackgroundColor = New Color(SystemColors.Control.ToArgb)
                            _Table.AddCell(cel)
                        End If
                    ElseIf _SelectedColumnus.Contains(GridCol.Name) Then
                        cel = New Cell(New Phrase(GridCol.HeaderText, FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD)))
                        cel.Header = True
                        cel.HorizontalAlignment = Element.ALIGN_CENTER
                        cel.BackgroundColor = New Color(SystemColors.Control.ToArgb)
                        _Table.AddCell(cel)
                    End If
                Next
            End If
        End If

        For cnt As Integer = 0 To _Dgv.Columns.Count - 1
            Dim GridCol As DataGridViewColumn = _Dgv.Columns(cnt)
            If Not GridCol.Visible Then Continue For
            If Not _SelectedColumnus.Contains(GridCol.Name) Then Continue For
            cel = New Cell(New Phrase(GridCol.HeaderText, FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD)))
            cel.Header = True
            cel.HorizontalAlignment = Element.ALIGN_CENTER
            cel.BackgroundColor = New Color(SystemColors.Control.ToArgb)
            'If Not _WithBorder Then cel.Border = 0
            _Table.AddCell(cel)
        Next
        If _HeaderForAllPages Then _Table.EndHeaders()

        For cnt As Integer = 0 To _Dgv.RowCount - 1
            Dim GridRow As DataGridViewRow = _Dgv.Rows(cnt)
            For Each GridCell As DataGridViewCell In GridRow.Cells
                If Not _SelectedColumnus.Contains(GridCell.OwningColumn.Name) Then Continue For
                cel = New Cell(New Phrase(GridCell.FormattedValue.ToString, FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.NORMAL)))
                cel.HorizontalAlignment = DgvColumnAlignment(GridCell.OwningColumn.DefaultCellStyle.Alignment)
                If _CellColor Then cel.BackgroundColor = New Color(GridCell.InheritedStyle.BackColor.ToArgb)
                _Table.AddCell(cel)
            Next
        Next
        _Document.Add(_Table)
        _Document.Close()
    End Sub

    Private Function DgvColumnAlignment(ByVal DgvContentAlign As DataGridViewContentAlignment) As Integer
        Select Case DgvContentAlign
            Case DataGridViewContentAlignment.BottomLeft, DataGridViewContentAlignment.MiddleLeft, DataGridViewContentAlignment.TopLeft
                Return Element.ALIGN_LEFT
            Case DataGridViewContentAlignment.BottomCenter, DataGridViewContentAlignment.MiddleCenter, DataGridViewContentAlignment.TopCenter
                Return Element.ALIGN_MIDDLE
            Case DataGridViewContentAlignment.BottomRight, DataGridViewContentAlignment.MiddleRight, DataGridViewContentAlignment.TopRight
                Return Element.ALIGN_RIGHT
            Case Else
                Return Element.ALIGN_MIDDLE
        End Select
    End Function
End Class

Public Class MyPageEvents
    Inherits PdfPageEventHelper

    Dim _Bf As BaseFont = Nothing
    Dim _Cb As PdfContentByte
    Dim _Act As String = ""
    Dim _Template As PdfTemplate

    Public Overrides Sub OnOpenDocument(ByVal writer As iTextSharp.text.pdf.PdfWriter, ByVal document As iTextSharp.text.Document)
        _Bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED)
        _Cb = writer.DirectContent
        _Template = _Cb.CreateTemplate(10, 10)
    End Sub

    Public Overrides Sub OnEndPage(ByVal writer As iTextSharp.text.pdf.PdfWriter, ByVal document As iTextSharp.text.Document)
        Dim pageN As Integer = writer.PageNumber
        Dim text As String = Date.Now.ToString("dd/MM/yyyy") & "               Page " & pageN
        Dim len As Single = _Bf.GetWidthPoint(text, 8)
        _Cb.BeginText()
        _Cb.SetFontAndSize(_Bf, 8)
        _Cb.SetTextMatrix(280, 30)
        _Cb.ShowText(text)
        _Cb.EndText()
        _Cb.AddTemplate(_Template, 280 + len, 30)
        _Cb.BeginText()
        writer.NewPage()
    End Sub
    Public Overrides Sub OnStartPage(ByVal writer As iTextSharp.text.pdf.PdfWriter, ByVal document As iTextSharp.text.Document)
        'Pdf.GridHeader()
        'DgvHeader(document)
        'MsgBox("st : " & document.PageNumber.ToString)
        '_Cb.BeginText()
        '_Cb.SetFontAndSize(_Bf, 8)
        '_Cb.SetTextMatrix(280, 30)
        '_Cb.ShowText("")
        '_Cb.EndText()
        '_NewPage = True
    End Sub
End Class
