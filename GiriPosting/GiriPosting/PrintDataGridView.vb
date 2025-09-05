Imports System.Collections.Generic
Imports System.Drawing.Printing

Public Class PrintDataGridView
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(PrintDataGridView))
    Private StrFormat As StringFormat         ' Holds content of a TextBox Cell to write by DrawString
    Private StrFormatComboBox As StringFormat ' Holds content of a Boolean Cell to write by DrawImage
    Private CellButton As Button          ' Holds the Contents of Button Cell
    Private CellCheckBox As CheckBox      ' Holds the Contents of CheckBox Cell
    Private CellComboBox As ComboBox      ' Holds the Contents of ComboBox Cell

    Private TotalWidth As Integer            ' Summation of Columns widths
    Private RowPos As Integer               ' Position of currently printing row 
    Private NewPage As Boolean            ' Indicates if a new page reached 
    Private PageNo As Integer                ' Number of pages to print 
    Private ColumnLefts As New ArrayList  ' Left Coordinate of Columns
    Private ColumnWidths As New ArrayList ' Width of Columns
    Private ColumnTypes As New ArrayList  ' DataType of Columns
    Private CellHeight As Integer          ' Height of DataGrid Cell
    Private RowsPerPage As Integer           ' Number of Rows per Page 
    Private WithEvents PrintDoc As New System.Drawing.Printing.PrintDocument ' PrintDocumnet Object used for printing

    Private PrintTitle() As String = Nothing               ' Header of pages
    Private dgv As New DataGridView                     ' Holds DataGrid Object to print its contents
    Private SelectedColumns As New List(Of String)  ' The Columns Selected by user to print.
    Private PrintAllRows As Boolean = True          ' True = print all rows,  False = print selected rows    
    Private FitToPageWidth As Boolean = True        ' True = Fits selected columns to page width ,  False = Print columns as showed    
    Private HeaderHeight As Integer = 0
    Private drawFont As Font
    Private grdFont As Font
    Private cellColor As Boolean
    Private cellFont As Boolean
    Private pageLandscape As Boolean
    Private rowHeight As Integer

    Private dgvHeader As DataGridView
    Private headColumnLefts As New ArrayList
    Private headColumnWidths As New ArrayList
    Private MailLablePrintStyle As Boolean
    Private NoOfLables As Integer

    Private WithEvents toolStripBtnPrint As New ToolStripButton


    Public Sub Print_DataGridView(ByVal Title As String _
    , ByVal Dgv1 As DataGridView _
    , ByVal FitToPageWidth As Boolean _
    , ByVal GetSelectedColumns As List(Of String) _
    , ByVal GetFont As Font _
    , ByVal GetCellColor As Boolean _
    , ByVal GetCellFont As Boolean _
    , ByVal Landscape As Boolean _
    , ByVal PrintStyle As Boolean _
    , ByVal NoOfLables As Integer _
    , ByVal GetRowHeight As Integer _
    , Optional ByVal grdHeader As DataGridView = Nothing _
        )
        If Not dgv1.RowCount > 0 Then
            Exit Sub
        End If
        Dim ppvw As PrintPreviewDialog
        Try
            ' Getting DataGridView object to print


            ' Saving some printing attributes
            PrintTitle = Title.Split(Environment.NewLine)
            PrintAllRows = PrintAllRows
            Me.FitToPageWidth = FitToPageWidth
            SelectedColumns = GetSelectedColumns
            grdFont = GetFont
            cellColor = GetCellColor
            cellFont = GetCellFont
            pageLandscape = Landscape
            MailLablePrintStyle = PrintStyle
            NoOfLables = NoOfLables
            rowHeight = GetRowHeight
            If rowHeight = 0 Then rowHeight = dgv1.RowTemplate.Height
            RowsPerPage = 0
            ppvw = New PrintPreviewDialog
            PrintDoc.DefaultPageSettings.Landscape = pageLandscape


            toolStripBtnPrint.Image = My.Resources.Resources.print
            toolStripBtnPrint.ImageTransparentColor = System.Drawing.Color.Magenta
            toolStripBtnPrint.Name = "toolStripBtnPrint"
            toolStripBtnPrint.Size = New System.Drawing.Size(49, 22)
            CType(ppvw.Controls(1), ToolStrip).Items(0).Visible = False
            CType(ppvw.Controls(1), ToolStrip).Items.Insert(0, toolStripBtnPrint)
            If MailLablePrintStyle Then
                Dim objPrint As New PrintLabel(MailLablePrintStyle, dgv1, NoOfLables, SelectedColumns, pageLandscape, rowHeight)
                dgv = objPrint.gridView
                objPrint.ShowDialog()
                Exit Sub
            Else
                dgv = Dgv1
            End If

            dgvHeader = grdHeader

            ppvw.Document = PrintDoc

            ' Showing the Print Preview Page
            If ppvw.ShowDialog() <> DialogResult.OK Then Exit Sub
            ' Printing the Documnet
            'PrintDoc.Print()
        Catch ex As Exception
            MessageBox.Show("MESSAGE    :" + ex.Message + vbCrLf + "STACK TRACE   :" + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub PrintDoc_BeginPrint(ByVal sender As Object, _
                ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDoc.BeginPrint
        Try
            ' Formatting the Content of Text Cells to print
            PrintDoc.DefaultPageSettings.Margins.Top = 50
            PrintDoc.DefaultPageSettings.Margins.Bottom = 50
            PrintDoc.DefaultPageSettings.Margins.Left = 50
            PrintDoc.DefaultPageSettings.Margins.Right = 50


            StrFormat = New StringFormat
            StrFormat.Alignment = StringAlignment.Near
            StrFormat.LineAlignment = StringAlignment.Center
            StrFormat.Trimming = StringTrimming.EllipsisCharacter

            ' Formatting the Content of Combo Cells to print
            StrFormatComboBox = New StringFormat
            StrFormatComboBox.LineAlignment = StringAlignment.Center
            StrFormatComboBox.FormatFlags = StringFormatFlags.NoWrap
            StrFormatComboBox.Trimming = StringTrimming.EllipsisCharacter


            ColumnLefts.Clear()
            ColumnWidths.Clear()
            ColumnTypes.Clear()
            CellHeight = 0
            RowsPerPage = 0

            headColumnLefts.Clear()
            headColumnWidths.Clear()



            ' For various column types
            CellButton = New Button
            CellCheckBox = New CheckBox
            CellComboBox = New ComboBox

            TotalWidth = 0
            For Each GridCol As DataGridViewColumn In dgv.Columns
                If Not GridCol.Visible Then Continue For
                If Not SelectedColumns.Contains(GridCol.Name) Then Continue For
                TotalWidth += GridCol.Width
            Next
            PageNo = 1
            NewPage = True
            RowPos = 0
            'MsgBox("WIDTH   :" + PrintDoc.DefaultPageSettings.PaperSize.Width.ToString + vbCrLf _
            '+ "HEIGHT   :" + PrintDoc.DefaultPageSettings.PaperSize.Height.ToString)

            'PrintDoc.DefaultPageSettings.Landscape = pageLandscape
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally

        End Try
    End Sub

    Private Sub PrintDoc_PrintPage(ByVal sender As Object, _
            ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDoc.PrintPage
        Dim tmpWidth As Integer, i As Integer
        Dim tmpTop As Integer = e.MarginBounds.Top
        Dim tmpLeft As Integer = e.MarginBounds.Left

        Dim strColl As String = Nothing '"GiriTechnologies"
        For Each str As String In PrintTitle
            If str.Length > Math.Ceiling(e.MarginBounds.Width / 8) Then
                str = str.Insert(Math.Ceiling(e.MarginBounds.Width / 8), Environment.NewLine)
            End If
            strColl += str
        Next
        PrintTitle = Nothing
        PrintTitle = strColl.Split(Environment.NewLine)
        Try
            ' Before starting first page, it saves Width & Height of Headers and CoulmnType
            If PageNo = 1 Then
                ''Getting Custom Head ColPos
                If Not (dgvHeader Is Nothing) Then
                    For Each GridCol As DataGridViewColumn In dgvHeader.Columns
                        Dim leng As Integer = 0
                        If GridCol.Name.Contains("~") Then
                            For Each cName As String In GridCol.Name.Split("~")
                                If Not SelectedColumns.Contains(cName) Then Continue For
                                If dgv.Columns.Contains(cName) Then
                                    leng += dgv.Columns(cName).Width
                                End If
                            Next
                        Else
                            If Not SelectedColumns.Contains(GridCol.Name) Then Continue For
                            leng += GridCol.Width
                        End If
                        If Not leng > 0 Then Continue For
                        ' Detemining whether the columns are fitted to page or not.
                        If FitToPageWidth Then
                            tmpWidth = CType(Math.Ceiling(leng / TotalWidth * _
                                       TotalWidth * (e.MarginBounds.Width / TotalWidth)), Int16)
                        Else
                            tmpWidth = leng
                        End If

                        HeaderHeight = e.Graphics.MeasureString(GridCol.HeaderText, _
                                       GridCol.InheritedStyle.Font, tmpWidth).Height + 11

                        headColumnLefts.Add(tmpLeft)
                        headColumnWidths.Add(tmpWidth)
                        tmpLeft += tmpWidth
                    Next
                End If

                tmpLeft = e.MarginBounds.Left
                For Each GridCol As DataGridViewColumn In dgv.Columns
                    If Not GridCol.Visible Then Continue For
                    If Not SelectedColumns.Contains(GridCol.Name) Then
                        Continue For
                    End If

                    ' Detemining whether the columns are fitted to page or not.
                    If FitToPageWidth Then
                        tmpWidth = CType(Math.Ceiling(GridCol.Width / TotalWidth * _
                                   TotalWidth * (e.MarginBounds.Width / TotalWidth)), Int16)
                    Else
                        tmpWidth = GridCol.Width
                    End If

                    HeaderHeight = e.Graphics.MeasureString(GridCol.HeaderText, _
                                   GridCol.InheritedStyle.Font, tmpWidth).Height + 11

                    ColumnLefts.Add(tmpLeft)
                    ColumnWidths.Add(tmpWidth)
                    ColumnTypes.Add(GridCol.GetType)
                    tmpLeft += tmpWidth
                Next
            End If

            ' Printing Current Page, Row by Row
            Do While RowPos <= dgv.Rows.Count - 1
                Dim GridRow As DataGridViewRow = dgv.Rows(RowPos)
                If GridRow.IsNewRow OrElse (Not PrintAllRows AndAlso Not GridRow.Selected) Then
                    RowPos += 1 : Continue Do
                End If

                CellHeight = GridRow.Height
                If cellFont = True Then
                    drawFont = dgv.Font
                Else
                    drawFont = grdFont
                End If
                i = 0
                For Each Cel As DataGridViewCell In GridRow.Cells
                    If Not Cel.OwningColumn.Visible Then Continue For
                    If Not SelectedColumns.Contains(Cel.OwningColumn.Name) Then
                        Continue For
                    End If
                    Dim mesHit As Single = e.Graphics.MeasureString(Cel.Value.ToString, Cel.InheritedStyle.Font, New Size(ColumnWidths(i), rowHeight)).Height
                    Dim tMeas As Integer = mesHit Mod 14
                    If CellHeight < GridRow.Height + IIf(tMeas > 1, GridRow.Height * (tMeas / 4), 0) Then
                        CellHeight = GridRow.Height + IIf(tMeas > 1, GridRow.Height * (tMeas / 4), 0)
                    End If
                    i += 1
                Next
                i = 0
                If tmpTop + CellHeight >= e.MarginBounds.Height + e.MarginBounds.Top Then
                    DrawFooter(e, RowsPerPage)
                    NewPage = True
                    PageNo += 1
                    e.HasMorePages = True
                    Exit Sub
                Else
                    If NewPage Then
                        ' Draw Header
                        tmpTop = e.MarginBounds.Top - 20

                        Dim inc As Integer = PrintTitle.Length - 1
                        For Each title As String In PrintTitle
                            e.Graphics.DrawString(title, New Font(dgv.Font, FontStyle.Bold), _
                                    Brushes.Black, e.MarginBounds.Left + (e.MarginBounds.Width - _
                    e.Graphics.MeasureString(title, New Font(dgv.Font, FontStyle.Bold), _
                    e.MarginBounds.Width).Width) / 2, e.MarginBounds.Top - _
                                    e.Graphics.MeasureString(title, New Font(dgv.Font, _
                                    FontStyle.Bold), e.MarginBounds.Width).Height - 13 * inc)
                            inc -= 1
                        Next

                        ' Draw Columns
                        tmpTop = e.MarginBounds.Top
                        i = 0

                        ''Printing CustHead
                        If Not (dgvHeader Is Nothing) Then
                            For Each GridCol As DataGridViewColumn In dgvHeader.Columns
                                Dim leng As Integer = 0
                                If GridCol.Name.Contains("~") Then
                                    For Each cName As String In GridCol.Name.Split("~")
                                        If Not SelectedColumns.Contains(cName) Then Continue For
                                        If dgv.Columns.Contains(cName) Then
                                            leng += dgv.Columns(cName).Width
                                        End If
                                    Next
                                Else
                                    If Not SelectedColumns.Contains(GridCol.Name) Then Continue For
                                    leng += GridCol.Width
                                End If
                                If Not leng > 0 Then Continue For
                                If cellFont = True Then
                                    drawFont = GridCol.InheritedStyle.Font
                                Else
                                    drawFont = grdFont
                                End If
                                e.Graphics.FillRectangle(New SolidBrush(Drawing.Color.LightGray), _
                                   New Rectangle(headColumnLefts(i), tmpTop, headColumnWidths(i), HeaderHeight))

                                e.Graphics.DrawRectangle(Pens.Black, New Rectangle(headColumnLefts(i), _
                                        tmpTop, headColumnWidths(i), HeaderHeight))

                                e.Graphics.DrawString(" " + GridCol.HeaderText, drawFont, _
                                        New SolidBrush(GridCol.InheritedStyle.ForeColor), _
                                        New RectangleF(headColumnLefts(i), tmpTop, headColumnWidths(i), _
                                        HeaderHeight), StrFormat)
                                i += 1
                            Next
                            tmpTop += HeaderHeight
                        End If

                        i = 0
                        For Each GridCol As DataGridViewColumn In dgv.Columns
                            If Not GridCol.Visible Then Continue For
                            If Not SelectedColumns.Contains(GridCol.Name) Then
                                Continue For
                            End If
                            If cellFont = True Then
                                drawFont = GridCol.InheritedStyle.Font
                            Else
                                drawFont = grdFont
                            End If
                            e.Graphics.FillRectangle(New SolidBrush(Drawing.Color.LightGray), _
                                    New Rectangle(ColumnLefts(i), tmpTop, ColumnWidths(i), HeaderHeight))

                            e.Graphics.DrawRectangle(Pens.Black, New Rectangle(ColumnLefts(i), _
                                    tmpTop, ColumnWidths(i), HeaderHeight))

                            e.Graphics.DrawString(" " + GridCol.HeaderText, drawFont, _
                                    New SolidBrush(GridCol.InheritedStyle.ForeColor), _
                                    New RectangleF(ColumnLefts(i), tmpTop, ColumnWidths(i), _
                                    HeaderHeight), StrFormat)
                            i += 1
                        Next
                        NewPage = False

                        tmpTop += HeaderHeight
                    End If

                    i = 0
                    For Each Cel As DataGridViewCell In GridRow.Cells
                        If Not Cel.OwningColumn.Visible Then Continue For
                        If Not SelectedColumns.Contains(Cel.OwningColumn.Name) Then
                            Continue For
                        End If
                        If cellFont = True Then
                            drawFont = Cel.InheritedStyle.Font
                        Else
                            drawFont = grdFont
                        End If

                        ' For the TextBox Column
                        If ColumnTypes(i) Is GetType(DataGridViewTextBoxColumn) OrElse _
                           ColumnTypes(i) Is GetType(DataGridViewLinkColumn) Then
                            With Cel.OwningColumn.DefaultCellStyle
                                Select Case .Alignment
                                    Case DataGridViewContentAlignment.BottomLeft, DataGridViewContentAlignment.MiddleLeft, DataGridViewContentAlignment.TopLeft
                                        StrFormat.Alignment = StringAlignment.Near
                                    Case DataGridViewContentAlignment.BottomRight, DataGridViewContentAlignment.MiddleRight, DataGridViewContentAlignment.TopRight
                                        StrFormat.Alignment = StringAlignment.Far
                                    Case DataGridViewContentAlignment.BottomCenter, DataGridViewContentAlignment.MiddleCenter, DataGridViewContentAlignment.TopCenter
                                        StrFormat.Alignment = StringAlignment.Center
                                End Select
                            End With
                            If cellColor = True Then
                                e.Graphics.FillRectangle(New SolidBrush(Cel.InheritedStyle.BackColor), _
                                            New Rectangle(ColumnLefts(i), tmpTop, ColumnWidths(i), CellHeight))
                            End If

                            If StrFormat.Alignment = StringAlignment.Near Then
                                e.Graphics.DrawString(Cel.FormattedValue.ToString, drawFont, _
                                       New SolidBrush(Cel.InheritedStyle.ForeColor), _
                                       New RectangleF(ColumnLefts(i), tmpTop, ColumnWidths(i), _
                                       CellHeight), StrFormat)

                                'e.Graphics.DrawString(" " + Cel.FormattedValue.ToString, drawFont, _
                                '                                       New SolidBrush(Cel.InheritedStyle.ForeColor), _
                                '                                       New RectangleF(ColumnLefts(i), tmpTop, ColumnWidths(i), _
                                '                                       CellHeight), StrFormat)
                            Else
                                e.Graphics.DrawString(Cel.FormattedValue.ToString, drawFont, _
                                       New SolidBrush(Cel.InheritedStyle.ForeColor), _
                                       New RectangleF(ColumnLefts(i), tmpTop, ColumnWidths(i), _
                                       CellHeight), StrFormat)
                            End If

                            ' For the Button Column
                        ElseIf ColumnTypes(i) Is GetType(DataGridViewButtonColumn) Then

                            CellButton.Text = Cel.Value.ToString
                            CellButton.Size = New Size(ColumnWidths(i), CellHeight)
                            Dim bmp As New Bitmap(CellButton.Width, CellButton.Height)
                            CellButton.DrawToBitmap(bmp, New Rectangle(0, 0, _
                                    bmp.Width, bmp.Height))
                            e.Graphics.DrawImage(bmp, New Point(ColumnLefts(i), tmpTop))

                            ' For the CheckBox Column
                        ElseIf ColumnTypes(i) Is GetType(DataGridViewCheckBoxColumn) Then

                            CellCheckBox.Size = New Size(14, 14)
                            CellCheckBox.Checked = CType(Cel.Value, Boolean)
                            Dim bmp As New Bitmap(ColumnWidths(i), CellHeight)
                            Dim tmpGraphics As Graphics = Graphics.FromImage(bmp)
                            tmpGraphics.FillRectangle(Brushes.White, New Rectangle(0, 0, _
                                    bmp.Width, bmp.Height))
                            CellCheckBox.DrawToBitmap(bmp, New Rectangle(CType((bmp.Width - _
                                    CellCheckBox.Width) / 2, Int32), CType((bmp.Height - _
                                    CellCheckBox.Height) / 2, Int32), CellCheckBox.Width, _
                                    CellCheckBox.Height))
                            e.Graphics.DrawImage(bmp, New Point(ColumnLefts(i), tmpTop))

                            ' For the ComboBox Column
                        ElseIf ColumnTypes(i) Is GetType(DataGridViewComboBoxColumn) Then

                            CellComboBox.Size = New Size(ColumnWidths(i), CellHeight)
                            Dim bmp As New Bitmap(CellComboBox.Width, CellComboBox.Height)
                            CellComboBox.DrawToBitmap(bmp, New Rectangle(0, 0, _
                                    bmp.Width, bmp.Height))
                            e.Graphics.DrawImage(bmp, New Point(ColumnLefts(i), tmpTop))
                            e.Graphics.DrawString(Cel.Value.ToString, Cel.InheritedStyle.Font, _
                                    New SolidBrush(Cel.InheritedStyle.ForeColor), _
                                    New RectangleF(ColumnLefts(i) + 1, tmpTop, ColumnWidths(i) _
                                    - 16, CellHeight), StrFormatComboBox)

                            ' For the Image Column
                        ElseIf ColumnTypes(i) Is GetType(DataGridViewImageColumn) Then

                            'Dim CelSize As Rectangle = New Rectangle(ColumnLefts(i), _
                            '        tmpTop, ColumnWidths(i), CellHeight)
                            'Dim ImgSize As Size = CType(Cel.FormattedValue, Image).Size
                            'e.Graphics.DrawImage(Cel.FormattedValue, New Rectangle(ColumnLefts(i) _
                            '        + CType(((CelSize.Width - ImgSize.Width) / 2), Int32), _
                            '        tmpTop + CType(((CelSize.Height - ImgSize.Height) / 2), _
                            '        Int32), CType(Cel.FormattedValue, Image).Width, CType(Cel.FormattedValue, _
                            '        Image).Height))


                            Dim cropWid As Integer = 0
                            Dim cropHeight As Integer = 0
                            Dim crobPictureBox As New PictureBox
                            Dim cropBitmap As Bitmap
                            Dim bm_dest As Bitmap
                            Dim bm_source As Bitmap
                            'If imageColWidFlag = False Then
                            cropWid = CInt(Cel.Size.Width)
                            cropHeight = CInt(Cel.Size.Height)
                            'End If

                            crobPictureBox.Size = New Size(cropWid, cropHeight)
                            crobPictureBox.Location = New Point(0, 0)
                            crobPictureBox.SizeMode = PictureBoxSizeMode.StretchImage
                            crobPictureBox.Image = CType(Cel.FormattedValue, Image)
                            Dim rect As Rectangle = New Rectangle(New Point(0, 0), New Size(cropWid, cropHeight))
                            Dim bit As Bitmap = New Bitmap(crobPictureBox.Image, cropWid, cropHeight)
                            cropBitmap = New Bitmap(cropWid, cropHeight)
                            Dim g As Graphics = Graphics.FromImage(cropBitmap)
                            g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
                            g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
                            g.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
                            g.DrawImage(bit, 0, 0, rect, GraphicsUnit.Pixel)

                            Dim img1 As New PictureBox
                            img1.Image = cropBitmap
                            bm_source = New Bitmap(img1.Image)
                            bm_dest = New Bitmap(bm_source.Width, bm_source.Height)
                            Dim gr_dest As Graphics = Graphics.FromImage(bm_dest)
                            gr_dest.DrawImage(bm_source, 0, 0, bm_dest.Width, bm_dest.Height)
                            System.Windows.Forms.Clipboard.SetDataObject(bm_dest, False)

                            Dim CelSize As Rectangle = New Rectangle(ColumnLefts(i), _
                           tmpTop, ColumnWidths(i), CellHeight)
                            Dim ImgSize As Size = bm_dest.Size
                            e.Graphics.DrawImage(bm_dest, New Rectangle(ColumnLefts(i) _
                                    + CType(((CelSize.Width - ImgSize.Width) / 2), Int32), _
                                    tmpTop + CType(((CelSize.Height - ImgSize.Height) / 2), _
                                    Int32), CType(bm_dest, Image).Width, CType(bm_dest, _
                                    Image).Height))

                        End If

                        ' Drawing Cells Borders 
                        e.Graphics.DrawRectangle(Pens.Black, New Rectangle(ColumnLefts(i), _
                                tmpTop, ColumnWidths(i), CellHeight))

                        i += 1

                        ''Set Previous State
                        StrFormat.Alignment = StringAlignment.Near

                    Next
                    tmpTop += CellHeight

                End If

                RowPos += 1
                ' For the first page it calculates Rows per Page
                If PageNo = 1 Then
                    RowsPerPage += 1
                End If
            Loop

            If RowsPerPage = 0 Then Exit Sub

            ' Write Footer (Page Number)
            DrawFooter(e, RowsPerPage)

            e.HasMorePages = False

        Catch ex As Exception
            MessageBox.Show("MESSAGE    :" + ex.Message + vbCrLf + "STACK TRACE   :" + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally

        End Try
    End Sub

    Private Sub DrawFooter(ByVal e As System.Drawing.Printing.PrintPageEventArgs, ByVal RowsPerPage As Int32)
        Dim cnt As Integer

        ' Detemining rows number to print
        If PrintAllRows Then
            If dgv.Rows(dgv.Rows.Count - 1).IsNewRow Then
                ' When the DataGridView doesn't allow adding rows
                cnt = dgv.Rows.Count - 2
            Else
                ' When the DataGridView allows adding rows
                cnt = dgv.Rows.Count - 1
            End If
        Else
            cnt = dgv.SelectedRows.Count
        End If

        ' Writing the Page Number on the Bottom of Page
        Dim PageNum As String = PageNo.ToString + " of " + _
                    Math.Ceiling(cnt / RowsPerPage).ToString
        e.Graphics.DrawString(PageNum, drawFont, Brushes.Black, _
                    e.MarginBounds.Left + (e.MarginBounds.Width - _
                    e.Graphics.MeasureString(PageNum, dgv.Font, _
                    e.MarginBounds.Width).Width) / 2, e.MarginBounds.Top + _
                    e.MarginBounds.Height + 31)

        Dim s As String = Now.ToLongDateString + " " + Now.ToShortTimeString
        e.Graphics.DrawString(s, New Font(dgv.Font, FontStyle.Bold), Brushes.Black, e.MarginBounds.Left, e.MarginBounds.Top + e.MarginBounds.Height + 31)
    End Sub

    Private Sub toolStripBtnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles toolStripBtnPrint.Click
        Dim printDia As New PrintDialog
        printDia.ShowNetwork = True
        printDia.ShowHelp = True
        printDia.AllowCurrentPage = True
        printDia.AllowSelection = True
        printDia.AllowSomePages = True
        printDia.PrinterSettings.DefaultPageSettings.Landscape = pageLandscape
        If printDia.ShowDialog <> DialogResult.OK Then Exit Sub
        PrintDoc.PrinterSettings = printDia.PrinterSettings
        printDia.Document = PrintDoc
        printDia.Document.Print()
    End Sub
End Class
