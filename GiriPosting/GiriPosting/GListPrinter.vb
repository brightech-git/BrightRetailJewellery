Imports System.Data.OleDb
Public Class GListPrinter
    Private RowPos As Integer               ' Position of currently printing row 
    Private RowPosObject As Integer         ' Position of currently printing row of Object
    Private PrintComplete As Boolean        ' Indicates if a object print reached end line  
    Private NewPage As Boolean              ' Indicates if a new page reached 
    Private PageNo As Integer               ' Number of pages to print 
    Private StrFormat As StringFormat
    Private WithEvents PrintDoc As New System.Drawing.Printing.PrintDocument ' PrintDocumnet Object used for printing
    Private ObjPrintDatatable As PrintDatatable
    Private lstSource As New List(Of Object)
    Private Title As String
    Private TitleFont As New Font("TIMES NEW ROMAN", 20, FontStyle.Bold)
    Public Property pTitleFont() As Font
        Get
            Return TitleFont
        End Get
        Set(ByVal value As Font)
            TitleFont = value
        End Set
    End Property
    Public Property pTitle() As String
        Get
            Return Title
        End Get
        Set(ByVal value As String)
            Title = value
        End Set
    End Property
    Public Sub New(ByVal lstSource As List(Of Object))
        Me.lstSource = lstSource
    End Sub
    Public Sub Print()
        If lstSource Is Nothing Then
            MsgBox("Data Source is Empty", MsgBoxStyle.Information)
            Exit Sub
        End If
        If lstSource.Count = 0 Then
            MsgBox("Data Source is Empty", MsgBoxStyle.Information)
            Exit Sub
        End If
        Using dlg As CoolPrintPreviewDialog = New CoolPrintPreviewDialog
            dlg.Document = Me.PrintDoc
            dlg.ShowDialog()
        End Using
        'Dim ppvw As New PrintPreviewDialog
        'ppvw.Document = PrintDoc
        'ppvw.ShowDialog()
    End Sub


    Private Sub PrintDoc_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDoc.BeginPrint
        PrintDoc.DefaultPageSettings.Margins.Top = 50
        PrintDoc.DefaultPageSettings.Margins.Bottom = 50
        PrintDoc.DefaultPageSettings.Margins.Left = 50
        PrintDoc.DefaultPageSettings.Margins.Right = 50

        StrFormat = New StringFormat
        StrFormat.Alignment = StringAlignment.Near
        StrFormat.LineAlignment = StringAlignment.Center
        StrFormat.Trimming = StringTrimming.EllipsisCharacter

        PageNo = 1
        NewPage = True
        RowPos = 0
        RowPosObject = 0
        PrintComplete = True
    End Sub

    Private Sub PrintTitle(ByVal title As String, ByVal e As System.Drawing.Printing.PrintPageEventArgs, ByRef Y As String)
        Dim xAx As Single = e.MarginBounds.Left + (e.MarginBounds.Width - e.Graphics.MeasureString(title, New Font("Courier New", 20, FontStyle.Bold), e.MarginBounds.Width).Width) / 2
        Dim yAx As Single = Y - e.Graphics.MeasureString(title, New Font("Courier New", 20, FontStyle.Bold), e.MarginBounds.Width).Height - 13 * 1
        e.Graphics.DrawString(title _
        , New Font("Courier New", 20, FontStyle.Bold) _
        , Brushes.Black _
        , xAx _
        , yAx)
        Y = yAx + e.Graphics.MeasureString(title, New Font("Courier New", 20, FontStyle.Bold), e.MarginBounds.Width).Height
    End Sub

    Private Sub PrintDoc_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDoc.PrintPage
        Dim tmpTop As Integer = e.MarginBounds.Top
        Dim tmpLeft As Integer = e.MarginBounds.Left
        ' Before starting first page, it saves Width & Height of Headers and CoulmnType
        If PageNo = 1 Then
            'tmpTop += 20
            'PrintTitle("CATALOG PRINT", e, tmpTop)
        End If

        Do While RowPos <= lstSource.Count - 1
            If NewPage Then
                ''Report Title Here
                tmpTop += 20
                PrintTitle(pTitle, e, tmpTop)
                NewPage = False
            End If
            If TypeOf lstSource.Item(RowPos) Is PictureBox Then
                Dim picBox As New PictureBox
                picBox.Size = CType(lstSource.Item(RowPos), PictureBox).Size
                picBox.Image = CType(lstSource.Item(RowPos), PictureBox).Image
                If picBox.Size.Height > e.MarginBounds.Height Then
                    picBox.Size = New Size(picBox.Size.Width, e.MarginBounds.Height)
                End If
                tmpLeft = e.MarginBounds.Left + (e.MarginBounds.Width - picBox.Width) / 2
                Dim BM_DEST As Bitmap
                Dim BM_SOURCE As Bitmap
                Dim CELSIZE As Rectangle = New Rectangle(tmpLeft, tmpTop, picBox.Width, picBox.Height)
                BM_SOURCE = New Bitmap(CType(lstSource.Item(RowPos), PictureBox).Image)
                BM_DEST = New Bitmap(BM_SOURCE.Width, BM_SOURCE.Height)
                Dim GR_DEST As Graphics = Graphics.FromImage(BM_DEST)
                GR_DEST.DrawImage(BM_SOURCE, 0, 0, BM_DEST.Width, BM_DEST.Height)
                System.Windows.Forms.Clipboard.SetDataObject(BM_DEST, False)
                Dim IMGSIZE As Size = BM_DEST.Size
                If tmpTop + CType(BM_DEST, Image).Height >= e.MarginBounds.Height + e.MarginBounds.Top Then
                    DrawFooter(e)
                    PageNo += 1
                    e.HasMorePages = True
                    NewPage = True
                    Exit Sub
                End If
                e.Graphics.FillRectangle(Brushes.Gray _
                , New Rectangle(tmpLeft + 5 + CType(((CELSIZE.Width - IMGSIZE.Width) / 2), Int32), tmpTop + 5, CType(BM_DEST, Image).Width, CType(BM_DEST, Image).Height) _
                )
                e.Graphics.DrawImage(BM_DEST _
                , New Rectangle(tmpLeft + CType(((CELSIZE.Width - IMGSIZE.Width) / 2), Int32), tmpTop, CType(BM_DEST, Image).Width, CType(BM_DEST, Image).Height) _
                )

                'e.Graphics.FillRectangle(Brushes.Gray _
                ', New Rectangle(tmpLeft + CType(((CELSIZE.Width - IMGSIZE.Width) / 2), Int32), tmpTop + 6 + CType(((CELSIZE.Height - IMGSIZE.Height) / 2), Int32) _
                ', CType(BM_DEST, Image).Width + 6 _
                ', CType(BM_DEST, Image).Height) _
                ')
                'e.Graphics.DrawImage(BM_DEST _
                ', New Rectangle(tmpLeft + CType(((CELSIZE.Width - IMGSIZE.Width) / 2), Int32), tmpTop + CType(((CELSIZE.Height - IMGSIZE.Height) / 2), Int32) _
                ', CType(BM_DEST, Image).Width _
                ', CType(BM_DEST, Image).Height) _
                ')
                tmpTop += CType(BM_DEST, Image).Height + 10 ''Increasing tmpTop Value
            ElseIf TypeOf lstSource.Item(RowPos) Is GDatatable Then
                If PrintComplete Then
                    ObjPrintDatatable = New PrintDatatable()
                    ObjPrintDatatable.pDtSource = lstSource.Item(RowPos)
                End If
                ObjPrintDatatable.Print(tmpTop, RowPosObject, e)
                PrintComplete = ObjPrintDatatable.pPrintComplete
                If Not PrintComplete Then
                    DrawFooter(e)
                    PageNo += 1
                    NewPage = True
                    e.HasMorePages = True
                    Exit Sub
                End If
            ElseIf TypeOf lstSource.Item(RowPos) Is Char Then
                If CType(lstSource.Item(RowPos), Char) = Chr(13) Then
                    DrawFooter(e)
                    PageNo += 1
                    NewPage = True
                    e.HasMorePages = True
                    RowPos += 1
                    Exit Sub
                End If
            End If
            RowPos += 1
        Loop
        DrawFooter(e)
    End Sub

    Private Sub DrawFooter(ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        e.Graphics.DrawString(PageNo, New Font("TIMES NEW ROMAN", 8, FontStyle.Regular), Brushes.Black, _
                    e.MarginBounds.Left + (e.MarginBounds.Width - _
                    e.Graphics.MeasureString(PageNo, New Font("TIMES NEW ROMAN", 8, FontStyle.Regular), _
                    e.MarginBounds.Width).Width) / 2, e.MarginBounds.Top + _
                    e.MarginBounds.Height + 31)

        Dim s As String = Now.ToLongDateString + " " + Now.ToShortTimeString
        e.Graphics.DrawString(s, New Font("TIMES NEW ROMAN", 8, FontStyle.Regular), Brushes.Black, e.MarginBounds.Left, e.MarginBounds.Top + e.MarginBounds.Height + 31)
    End Sub

    Private Sub PrintDoc_EndPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDoc.EndPrint

    End Sub

End Class
