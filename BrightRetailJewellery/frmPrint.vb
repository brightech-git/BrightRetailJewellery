Imports System.IO
Public Class frmPrint
    Dim fName As String
    Public Sub New(ByVal fileName As String)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        fName = fileName
        Try
            Dim printerNames As System.Drawing.Printing.PrinterSettings.StringCollection = Nothing
            Dim printDoc As New System.Drawing.Printing.PrintDocument
            For Each name As String In System.Drawing.Printing.PrinterSettings.InstalledPrinters
                cmbPrinterNames.Items.Add(name)
            Next
            If cmbPrinterNames.Items.Count > 0 Then
                cmbPrinterNames.Text = printDoc.PrinterSettings.PrinterName
            End If
        Catch ex As Exception
            cmbPrinterNames.Items.Clear()
        End Try
    End Sub
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not cmbPrinterNames.Items.Count > 0 Then
            MsgBox("Printer Name Should Not Empty", MsgBoxStyle.Information)
            Me.Close()
            Exit Sub
        End If
        If cmbPrinterNames.Text.Contains(" ") Then
            Dim msg As String = Nothing
            msg = "Invalid Printer Name" + vbCrLf
            msg += "1.Printer Name Should not contain white spaces" + vbCrLf
            msg += "2.Printer Name length should not exceed 8 charachters"
            MsgBox(msg, MsgBoxStyle.Critical, "Error!!!")
            Me.Close()
            Exit Sub
        End If
        Dim batWriter As StreamWriter
        batWriter = File.CreateText("C:\REPORTS\" & fName & ".bat")
        If Trim(cmbPrinterNames.Text).StartsWith("\\") = False Then
            batWriter.WriteLine("TYPE " + "C:\REPORTS\" & fName & ".txt > " + "PRN")
        Else
            batWriter.WriteLine("TYPE " + "C:\REPORTS\" & fName & ".txt > " + cmbPrinterNames.Text)
        End If
        batWriter.Close()
        System.Diagnostics.Process.Start("C:\REPORTS\" & fName & ".bat")
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub frmPrint_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub

    Private Sub frmPrint_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub btnFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFile.Click
        Dim fDia As New SaveFileDialog
        fDia.DefaultExt = ".txt"
        fDia.Filter = "TEXT(*.txt)|*.txt"
        fDia.FileName = ".txt"
        If fDia.ShowDialog = Windows.Forms.DialogResult.OK Then
            IO.File.Copy("C:\REPORTS\" & fName & ".txt", fDia.FileName)
            System.Diagnostics.Process.Start(fDia.FileName)
            Me.Close()
        End If
    End Sub
End Class