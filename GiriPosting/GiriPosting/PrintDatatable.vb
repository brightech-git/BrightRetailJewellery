Public Class PrintDatatable
    Private DtSource As New GDatatable
    Private ColumnLeft As New List(Of String)
    Private ColumnWidth As New List(Of String)
    Private TotalWidth As Integer
    Private StrFormat As StringFormat
    Private PrintComplete As Boolean
    Public Property pPrintComplete() As Boolean
        Get
            Return PrintComplete
        End Get
        Set(ByVal value As Boolean)
            PrintComplete = value
        End Set
    End Property
    Public Property pDtSource() As GDatatable
        Get
            Return DtSource
        End Get
        Set(ByVal value As GDatatable)
            DtSource = value
        End Set
    End Property

    Public Sub New()
        StrFormat = New StringFormat
        StrFormat.Alignment = StringAlignment.Near
        StrFormat.LineAlignment = StringAlignment.Center
        StrFormat.Trimming = StringTrimming.EllipsisCharacter
    End Sub

    Public Sub Print(ByRef TmpTop As Integer, ByRef RowPos As Integer, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        Dim TmpLeft As Integer = e.MarginBounds.Left
        Dim TmpWidth As Integer
        Dim TmpRowHeight As Integer = DtSource.pRowHeight
        If DtSource.Rows.Count > 0 Then
            If TypeOf (DtSource.Rows(RowPos)) Is BrightPosting.GDataRow Then
                TmpRowHeight = CType(DtSource.Rows(RowPos), BrightPosting.GDataRow).pRowHeight
            End If
        End If
        If TmpRowHeight = 0 Then TmpRowHeight = DtSource.pRowHeight
        Dim i As Integer = 0
        If RowPos = 0 Then
            TotalWidth = 0
            For Each DtCol As DataColumn In DtSource.Columns ''Get Column Left Width
                TotalWidth += Val(DtCol.Caption)
            Next
            If TotalWidth = 0 Then
                TotalWidth = e.MarginBounds.Width
                For Each dtcol As DataColumn In DtSource.Columns
                    dtcol.Caption = CType(Math.Ceiling(Val(TotalWidth / DtSource.Columns.Count) / TotalWidth * TotalWidth * (e.MarginBounds.Width / TotalWidth)), Int16)
                Next
            End If

            Dim TotWid As Integer
            For Each DtCol As DataColumn In DtSource.Columns ''Get Column Left Width
                TmpWidth = CType(Math.Ceiling(Val(DtCol.Caption) / TotalWidth * TotalWidth * (e.MarginBounds.Width / TotalWidth)), Int16)
                ColumnLeft.Add(TmpLeft)
                ColumnWidth.Add(TmpWidth)
                TmpLeft += TmpWidth
                TotWid += TmpWidth
            Next
            If TotWid <> e.MarginBounds.Width Then
                If ColumnWidth.Count > 0 Then
                    ColumnWidth.Item(ColumnWidth.Count - 1) = Val(ColumnWidth.Item(ColumnWidth.Count - 1)) + (e.MarginBounds.Width - TotWid)
                End If
            End If


        End If
        'If DtSource.Rows.Count > 0 And DtSource.pColHeaderVisible Then
        If DtSource.pColHeaderVisible Then
            If TmpTop + (TmpRowHeight * 2) >= e.MarginBounds.Height + e.MarginBounds.Top Then
                Exit Sub
            End If
            For Each DtCol As DataColumn In DtSource.Columns ''Print Column Heading
                e.Graphics.FillRectangle(New SolidBrush(Drawing.Color.LightGray), _
                   New Rectangle(ColumnLeft(i), TmpTop, ColumnWidth(i), TmpRowHeight))
                e.Graphics.DrawRectangle(Pens.Gray, New Rectangle(ColumnLeft(i), _
                        TmpTop, ColumnWidth(i), TmpRowHeight))
                e.Graphics.DrawString(" " + DtCol.ColumnName, DtSource.pColHeaderFont, _
                        New SolidBrush(Color.Black), _
                        New RectangleF(ColumnLeft(i), TmpTop, ColumnWidth(i), _
                        TmpRowHeight), StrFormat)
                i += 1
            Next
            TmpTop += TmpRowHeight ''Increasing TmpTop Value
        End If
        Dim DtRow As DataRow = Nothing
        Do While RowPos <= DtSource.Rows.Count - 1 ''Print Row by Row
            If TypeOf (DtSource.Rows(RowPos)) Is BrightPosting.GDataRow Then
                TmpRowHeight = CType(DtSource.Rows(RowPos), BrightPosting.GDataRow).pRowHeight
            End If
            If TmpRowHeight = 0 Then TmpRowHeight = DtSource.pRowHeight
            i = 0
            DtRow = DtSource.Rows(RowPos)
            If TmpTop + TmpRowHeight >= e.MarginBounds.Height + e.MarginBounds.Top Then
                Exit Sub
            End If
            For Each dtcol As DataColumn In DtSource.Columns

                Select Case dtcol.DataType.Name
                    Case GetType(Integer).Name, GetType(Int16).Name, GetType(Int32).Name, GetType(Int32).Name, GetType(Int64).Name, GetType(Decimal).Name
                        StrFormat = New StringFormat
                        If DtSource.pTableContentAlignment = Nothing Then
                            StrFormat.Alignment = StringAlignment.Far
                        Else
                            StrFormat.Alignment = DtSource.pTableContentAlignment
                        End If
                        StrFormat.LineAlignment = StringAlignment.Center
                        StrFormat.Trimming = StringTrimming.EllipsisCharacter
                        If DtSource.pTableBackColor <> Nothing Then
                            e.Graphics.FillRectangle(New SolidBrush(DtSource.pTableBackColor) _
                            , New Rectangle(ColumnLeft(i), TmpTop, ColumnWidth(i), TmpRowHeight))
                        End If
                        e.Graphics.DrawString(DtRow.Item(dtcol).ToString, DtSource.pContentFont _
                                , New SolidBrush(Color.Black) _
                                , New RectangleF(ColumnLeft(i), TmpTop, ColumnWidth(i), TmpRowHeight) _
                                , StrFormat)
                    Case GetType(PictureBox).Name
                        Dim picBox As New PictureBox
                        If IsDBNull(DtRow.Item(dtcol)) Then
                            Dim tPic As New PictureBox
                            tPic.Size = New Size(ColumnWidth(i), TmpRowHeight)
                            tPic.Image = My.Resources.emptyImage
                            DtRow.Item(dtcol) = tPic
                        End If
                        picBox = CType(DtRow.Item(dtcol), PictureBox)
                        If picBox.Size.Height > e.MarginBounds.Height Then
                            picBox.Size = New Size(picBox.Size.Width, e.MarginBounds.Height)
                        End If
                        TmpLeft = e.MarginBounds.Left + (e.MarginBounds.Width - picBox.Width) / 2
                        Dim BM_DEST As Bitmap
                        Dim BM_SOURCE As Bitmap
                        Dim CELSIZE As Rectangle = New Rectangle(TmpLeft, TmpTop, picBox.Width, picBox.Height)
                        BM_SOURCE = New Bitmap(picBox.Image)
                        BM_DEST = New Bitmap(BM_SOURCE.Width, BM_SOURCE.Height)
                        Dim GR_DEST As Graphics = Graphics.FromImage(BM_DEST)
                        GR_DEST.DrawImage(BM_SOURCE, 0, 0, BM_DEST.Width, BM_DEST.Height)
                        System.Windows.Forms.Clipboard.SetDataObject(BM_DEST, False)
                        Dim IMGSIZE As Size = BM_DEST.Size

                        e.Graphics.DrawImage(BM_DEST _
                        , New Rectangle(ColumnLeft(i) + CType(((CELSIZE.Width - IMGSIZE.Width) / 2), Int32), TmpTop, CType(BM_DEST, Image).Width, CType(BM_DEST, Image).Height) _
                        )



                    Case Else
                        StrFormat = New StringFormat
                        If DtSource.pTableContentAlignment = Nothing Then
                            StrFormat.Alignment = StringAlignment.Near
                        Else
                            StrFormat.Alignment = DtSource.pTableContentAlignment
                        End If
                        StrFormat.LineAlignment = StringAlignment.Center
                        StrFormat.Trimming = StringTrimming.EllipsisCharacter
                        If DtSource.pTableBackColor <> Nothing Then
                            e.Graphics.FillRectangle(New SolidBrush(DtSource.pTableBackColor) _
                            , New Rectangle(ColumnLeft(i), TmpTop, ColumnWidth(i), TmpRowHeight))
                        End If
                        e.Graphics.DrawString(DtRow.Item(dtcol).ToString, DtSource.pContentFont _
                                , New SolidBrush(Color.Black) _
                                , New RectangleF(ColumnLeft(i), TmpTop, ColumnWidth(i), TmpRowHeight) _
                                , StrFormat)
                End Select
                If DtSource.pCellBorder Then
                    e.Graphics.DrawRectangle(DtSource.pCellBorderColor _
                    , New Rectangle(ColumnLeft(i), TmpTop, ColumnWidth(i), TmpRowHeight))
                End If
                i += 1
            Next
            TmpTop += TmpRowHeight ''Increasing TmpTop Value
            RowPos += 1
        Loop
        RowPos = 0
        TmpTop += DtSource.pTablePadding
        pPrintComplete = True
    End Sub

End Class
