Imports System.Data.OleDb
Public Class frmAccountMIMRPrintDesign
#Region " Variable"
    Dim fontRegular As New Font("Palatino Linotype", 9, FontStyle.Regular)
    Dim fontMargin As New Font("Palatino Linotype", 7, FontStyle.Regular)
    Dim fontBold As New Font("Palatino Linotype", 9, FontStyle.Bold)
    Dim fontBold8 As New Font("Palatino Linotype", 8, FontStyle.Bold)
    Dim fontBold7 As New Font("Palatino Linotype", 7, FontStyle.Bold)
    Dim fontUnderLine As New Font("Palatino Linotype", 9, FontStyle.Underline)
    Dim fontBoldUnderLine As New Font("Palatino Linotype", 9, FontStyle.Bold And FontStyle.Underline)
    Dim fontBoldTitle As New Font("Palatino Linotype", 11, FontStyle.Bold)
    Dim LeftFormat As New StringFormat(StringAlignment.Near)
    Dim RightFormat As New StringFormat(StringAlignment.Far)
    Dim CentreFormat As New StringFormat(StringAlignment.Center)
    Dim c0 As Integer = 0  ' Sno
    Dim c1 As Integer = 50  ' TRANTYPENAME
    Dim c2 As Integer = 125  ' TRANNO
    Dim c3 As Integer = 200  ' TRANDATE
    Dim c4 As Integer = 320 ' PCS 
    Dim c5 As Integer = 380 ' GRSWT 
    Dim c6 As Integer = 450 ' STNWT
    Dim c7 As Integer = 520 ' NETWT
    Dim c8 As Integer = 595 ' PUREWT
    Dim c9 As Integer = 670 ' MCHARGE
    Dim c10 As Integer = 770 ' AMOUNT
    Dim rAlign As New StringFormat
    Dim dtAcnamePrint As New DataTable
    Dim Topy As Integer = 0
    Dim dtAddressAccode As DataTable
    Dim strsql As String = ""
    Dim loopCount As Integer = 0
#End Region

#Region " Constructor"
    Public Sub New(ByVal DtAcname As DataTable)
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        strsql = " SELECT COMPANYID,COMPANYNAME,ADDRESS1,ADDRESS2,ADDRESS3,ADDRESS4,PHONE,GSTNO "
        strsql += vbCrLf + " ,LTRIM(CONVERT(VARCHAR(15),GETDATE(),103)) +' ' + LTRIM(CONVERT(VARCHAR(15),GETDATE(),108))  SDATE "
        strsql += vbCrLf + " FROM " & cnAdminDb & "..COMPANY WHERE COMPANYID='" & strCompanyId & "' "
        dtAddressAccode = GetSqlTable(strsql, cn)
        dtAcnamePrint = DtAcname
        If dtAcnamePrint.Rows.Count > 0 Then
            PrintDialog1.Document = PrintDocument1
            PrintDocument1.PrinterSettings.Copies = 1
            PrintDocument1.PrintController = New System.Drawing.Printing.StandardPrintController
            PrintDocument1.Print()
        End If
    End Sub
    Private Sub printCompanyTile(ByVal g As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        If dtAddressAccode.Rows.Count > 0 Then
            With dtAddressAccode.Rows(0)
                Dim rect1 As New Rectangle(10, Topy, 800, 50)
                Dim sf As New StringFormat
                sf.LineAlignment = StringAlignment.Center
                sf.Alignment = StringAlignment.Center
                g.DrawString(strCompanyName, fontBold, Brushes.Black, rect1, sf)
                Topy = Topy + 20
                Dim rect2 As New Rectangle(10, Topy, 800, 50)
                g.DrawString(dtAddressAccode.Rows(0).Item("GSTNO").ToString, fontRegular, Brushes.Black, rect2, sf)
                Topy = Topy + 20
                Dim rect3 As New Rectangle(10, Topy, 800, 50)
                g.DrawString("MATERIAL RECEIPT / ISSUE ", fontRegular, Brushes.Black, rect3, sf)
                Topy = Topy + 20
                If dtAddressAccode.Rows.Count > 0 Then
                    If dtAddressAccode.Rows(0).Item("ADDRESS1").ToString <> "" Then
                        Dim rect4 As New Rectangle(10, Topy, 800, 50)
                        g.DrawString(dtAddressAccode.Rows(0).Item("ADDRESS1").ToString & " " & dtAddressAccode.Rows(0).Item("ADDRESS2").ToString(), fontRegular, Brushes.Black, rect4, sf)
                        Topy = Topy + 18
                    End If
                    If dtAddressAccode.Rows(0).Item("ADDRESS3").ToString <> "" Then
                        Dim rect4 As New Rectangle(10, Topy, 800, 50)
                        g.DrawString(dtAddressAccode.Rows(0).Item("ADDRESS3").ToString & " " & dtAddressAccode.Rows(0).Item("ADDRESS4").ToString, fontRegular, Brushes.Black, rect4, sf)
                        Topy = Topy + 18
                    End If
                End If
                Topy = Topy + 18
            End With
        End If
    End Sub

    Private Sub DrawLineGrand(ByVal g As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs, ByVal LineColorBlack As Boolean)
        If LineColorBlack = True Then
            g.DrawLine(Pens.Black, c4, Topy, c10, Topy)
        Else
            g.DrawLine(Pens.Silver, c4, Topy, c10, Topy)
        End If
    End Sub

    Private Sub DrawLine(ByVal g As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs, ByVal LineColorBlack As Boolean)
        If LineColorBlack = True Then
            g.DrawLine(Pens.Black, c1, Topy, c10, Topy)
        Else
            g.DrawLine(Pens.Silver, c1, Topy, c10, Topy)
        End If
    End Sub

    Private Sub PrintDocument1_PrintPage(sender As Object, e As Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        rAlign.Alignment = StringAlignment.Far
        Using g As Graphics = e.Graphics
            Dim TempFont As Object = Nothing
            printCompanyTile(e.Graphics, e)
            Topy = Topy + 5
            DrawLine(e.Graphics, e, True)
            Topy = Topy + 5
            'g.DrawString("#", fontBold8, Brushes.Black, c0, Topy, LeftFormat)
            g.DrawString("NAME ", fontBold8, Brushes.Black, c1, Topy, LeftFormat)
            g.DrawString("TRANNO ", fontBold8, Brushes.Black, c2, Topy, LeftFormat)
            g.DrawString("TRANDATE ", fontBold8, Brushes.Black, c3, Topy, LeftFormat)
            g.DrawString("PCS ", fontBold8, Brushes.Black, c4, Topy, rAlign)
            g.DrawString("GRSWT ", fontBold8, Brushes.Black, c5, Topy, rAlign)
            g.DrawString("STNWT ", fontBold8, Brushes.Black, c6, Topy, rAlign)
            g.DrawString("NETWT ", fontBold8, Brushes.Black, c7, Topy, rAlign)
            g.DrawString("PUREWT ", fontBold8, Brushes.Black, c8, Topy, rAlign)
            g.DrawString("MCHARGE ", fontBold8, Brushes.Black, c9, Topy, rAlign)
            g.DrawString("AMOUNT ", fontBold8, Brushes.Black, c10, Topy, rAlign)
            Topy = Topy + 30
            DrawLine(g, e, True)
            Topy = Topy + 18
            If dtAcnamePrint.Rows.Count > 0 Then
                For loopCount = loopCount To dtAcnamePrint.Rows.Count - 1
                    With dtAcnamePrint.Rows(loopCount)
                        'g.DrawString(loopCount + 1, fontRegular, Brushes.Black, c0, Topy, LeftFormat)
                        If .Item("COLHEAD").ToString = "G" Then
                            DrawLineGrand(e.Graphics, e, True)
                        End If
                        If .Item("COLHEAD").ToString = "T" Or .Item("COLHEAD").ToString = "S" Or .Item("COLHEAD").ToString = "G" Then
                            TempFont = fontBold
                        Else
                            TempFont = fontRegular
                        End If
                        g.DrawString(.Item("TRANTYPENAME").ToString, TempFont, Brushes.Black, c1, Topy, LeftFormat)
                        If .Item("COLHEAD").ToString = "T" Then
                            Topy = Topy + 25
                            Continue For
                        End If
                        g.DrawString(IIf(IsDBNull(.Item("TRANNO")), "", .Item("TRANNO")), TempFont, Brushes.Black, c2, Topy, LeftFormat)
                        If .Item("TRANDATE").ToString <> "" Then
                            g.DrawString(Format(.Item("TRANDATE"), "dd/MM/yyyy"), TempFont, Brushes.Black, c3, Topy, LeftFormat)
                        End If
                        g.DrawString(IIf(Val(.Item("PCS").ToString) = 0, "", .Item("PCS")), TempFont, Brushes.Black, c4, Topy, rAlign)
                        g.DrawString(IIf(Val(.Item("GRSWT").ToString) = 0, "", .Item("GRSWT")), TempFont, Brushes.Black, c5, Topy, rAlign)
                        g.DrawString(IIf(Val(.Item("STNWT").ToString) = 0, "", .Item("STNWT")), TempFont, Brushes.Black, c6, Topy, rAlign)
                        g.DrawString(IIf(Val(.Item("NETWT").ToString) = 0, "", .Item("NETWT")), TempFont, Brushes.Black, c7, Topy, rAlign)
                        g.DrawString(IIf(Val(.Item("PUREWT").ToString) = 0, "", .Item("PUREWT")), TempFont, Brushes.Black, c8, Topy, rAlign)
                        g.DrawString(IIf(Val(.Item("MCHARGE").ToString) = 0, "", .Item("MCHARGE")), TempFont, Brushes.Black, c9, Topy, rAlign)
                        g.DrawString(IIf(Val(.Item("AMOUNT").ToString) = 0, "", .Item("AMOUNT")), TempFont, Brushes.Black, c10, Topy, rAlign)
                        Topy = Topy + 25
                        If Topy > 1025 Then
                            loopCount = loopCount + 1
                            Topy = Topy + 5
                            g.DrawString(" Printed Out Taken By " & cnUserName & " " & dtAddressAccode.Rows(0).Item("SDATE").ToString, fontRegular, Brushes.Black, c1, Topy + 10, LeftFormat)
                            DrawLine(e.Graphics, e, True)
                            Topy = 0
                            e.HasMorePages = True
                            Return
                        End If
                    End With
                Next
                Topy = 1025
                g.DrawString(" Printed Out Taken By " & cnUserName & " " & dtAddressAccode.Rows(0).Item("SDATE").ToString, fontRegular, Brushes.Black, c1, Topy + 10, LeftFormat)
                DrawLine(e.Graphics, e, True)
            End If
        End Using
    End Sub

    Private Sub frmAccountMIMRPrintDesign_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
#End Region

End Class