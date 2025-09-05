Public Class CatalogPrint
    Private WithEvents PrintDoc As New System.Drawing.Printing.PrintDocument ' PrintDocumnet Object used for printing
    Public Sub New()
        Dim ppvw As PrintPreviewDialog
        ppvw.ShowDialog()
    End Sub
End Class
