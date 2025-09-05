Imports System.Drawing.Printing
Public Class frmBillPrinterSelect
    Dim selectedPrint As String
    'Dim DupFlag As Boolean
    Dim DupPdc As String
    Dim flagGlobalPrint As Boolean
    Dim Nodeid As String = Environment.MachineName.ToString.Replace("-", "")
    Private Sub frmPrinterSelect_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmPrinterSelect_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        cmbrecprinter.Items.Clear()
        'cmbcardprinter.Items.Clear()
        'rdbDotMatrix.Checked = True
        flagGlobalPrint = False
        Dim PrinterNameList As PrinterSettings.StringCollection
        Dim prtdoc As New PrintDocument
        Dim strDefaultPrinter As String = prtdoc.PrinterSettings.PrinterName
        Dim currentprintername As String
        Dim mysettings As New PrinterSettings
        PrinterNameList = PrinterSettings.InstalledPrinters
        For Each currentprintername In PrinterNameList
            cmbrecprinter.Items.Add(currentprintername)
            If currentprintername = strDefaultPrinter Then
                cmbrecprinter.SelectedIndex = cmbrecprinter.Items.IndexOf(currentprintername)
            End If
        Next
    End Sub
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim printername As String
        selectedPrint = cmbrecprinter.Text
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Dispose(True)
    End Sub

    Private Sub txtNoofCopies_TextChanged(sender As Object, e As EventArgs) Handles txtNoofCopies.TextChanged
        If Val(txtNoofCopies.Text) > 2 Then
            txtNoofCopies.Text = 2
        End If
    End Sub
End Class