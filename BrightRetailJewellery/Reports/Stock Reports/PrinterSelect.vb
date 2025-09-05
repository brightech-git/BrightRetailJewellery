Imports System.Drawing.Printing
Imports System.IO
Public Class frmPrinterSelect
    Dim selectedPrint As String
    Dim flagGlobalPrint As Boolean

    Private Sub frmPrinterSelect_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        cmbPrinterName.Items.Clear()
        rdbDotMatrix.Checked = True
        flagGlobalPrint = False
        Dim PrinterNameList As PrinterSettings.StringCollection
        Dim prtdoc As New PrintDocument
        Dim strDefaultPrinter As String = prtdoc.PrinterSettings.PrinterName
        Dim currentprintername As String
        Dim mysettings As New PrinterSettings
        PrinterNameList = PrinterSettings.InstalledPrinters
        For Each currentprintername In PrinterNameList
            cmbPrinterName.Items.Add(currentprintername)
            If currentprintername = strDefaultPrinter Then
                cmbPrinterName.SelectedIndex = cmbPrinterName.Items.IndexOf(currentprintername)
            End If
        Next
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If rdbDotMatrix.Checked = True Then
            Dim strprint As String
            Dim FileWrite As StreamWriter
            selectedPrint = cmbPrinterName.Text

            FileWrite = File.CreateText(Application.StartupPath + "\Print.bat")

            If InStr(selectedPrint, "\\") > 0 Then
                strprint = "type fileprint.txt > " & selectedPrint
            ElseIf InStr(selectedPrint, "LPT1") > 0 Then
                strprint = "type fileprint.txt >  LPT1"
            ElseIf InStr(selectedPrint, "LPT2") > 0 Then
                strprint = "type fileprint.txt >  LPT2"
            ElseIf InStr(selectedPrint, "LPT3") > 0 Then
                strprint = "type fileprint.txt >  LPT3"
            Else
                strprint = "type fileprint.txt >  PRN"
            End If

            FileWrite.WriteLine(Trim(strprint))
            FileWrite.Close()

            Dim process As New Process
            Dim path As String = Application.StartupPath + "\Print.bat"
            process.StartInfo.FileName = path
            process.Start()
        Else
            flagGlobalPrint = True
        End If
        Me.Dispose(True)
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Dispose(True)
    End Sub
End Class