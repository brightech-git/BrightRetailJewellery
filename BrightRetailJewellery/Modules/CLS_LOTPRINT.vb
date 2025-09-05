Imports System.IO
Imports System.Data.OleDb
Public Class CLS_LOTPRINT
    Public Enum PrinterType
        Col40 = 1
        Col80 = 2
    End Enum
    Private DtLotInfo As New DataTable
    Private Da As OleDbDataAdapter
    Private Cmd As OleDbCommand
    Private StrSql As String
    Private LotNo As Integer
    Private LotDate As Date
    Private TotalColumn As Integer
    Private FileName As String = "LTPRN" & systemId
    Dim oWrite As System.IO.StreamWriter
    Public Sub New(ByVal LotNo As Integer, ByVal LotDate As Date)
        If IO.Directory.Exists("C:\REPORTS") = False Then IO.Directory.CreateDirectory("C:\REPORTS")
        Me.LotNo = LotNo
        Me.LotDate = LotDate
        TotalColumn = 40
    End Sub
    Public Sub Print()
        StrSql = vbCrLf + " SELECT L.LOTNO,CONVERT(VARCHAR,L.LOTDATE,103)LOTDATE,L.ITEMID,IM.ITEMNAME,L.DESIGNERID,DE.SEAL"
        StrSql += vbCrLf + " ,L.PCS,L.GRSWT,L.NETWT,L.STNPCS,L.STNWT,L.DIAPCS,L.DIAWT"
        StrSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMLOT AS L"
        StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = L.ITEMID"
        StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..DESIGNER AS DE ON DE.DESIGNERID = L.DESIGNERID"
        StrSql += vbCrLf + " WHERE L.LOTNO = " & LotNo
        StrSql += vbCrLf + " AND L.LOTDATE = '" & LotDate.ToString("yyyy-MM-dd") & "'"
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(DtLotInfo)
        If Not DtLotInfo.Rows.Count > 0 Then
            Exit Sub
        End If
        Dim Str As String = ""
        oWrite = System.IO.File.CreateText("C:\REPORTS\" & FileName & ".txt")
        PrintHead()
        Str = LSet("LOTNO", 6) & ":" & LSet(DtLotInfo.Rows(0).Item("LOTNO").ToString, 6)
        Str += LSet("RECDATE", 7) & ":" & LSet(DtLotInfo.Rows(0).Item("LOTDATE").ToString, 8)
        PrintLine(Str)
        For Each Ro As DataRow In DtLotInfo.Rows
            Str = LSet("ITEM", 6) & ":" & LSet(Ro.Item("ITEMID") & "/" & Ro.Item("ITEMNAME").ToString, 33)
            PrintLine(Str)
            Str = LSet("DES", 6) & ":" & LSet(Ro.Item("DESIGNERID") & "/" & Ro.Item("SEAL").ToString, 33)
            PrintLine(Str)
            Str = RSet("P", 6) & ":" & RSet(IIf(Val(Ro.Item("PCS").ToString) <> 0, Ro.Item("PCS").ToString, ""), 5)
            Str += " " & RSet("GWT:", 4) & RSet(IIf(Val(Ro.Item("GRSWT").ToString) <> 0, Format(Val(Ro.Item("GRSWT").ToString), "0.000"), ""), 9)
            Str += " " & RSet("NWT:", 4) & RSet(IIf(Val(Ro.Item("NETWT").ToString) <> 0, Format(Val(Ro.Item("NETWT").ToString), "0.000"), ""), 9)
            PrintLine(Str)
            If Val(Ro.Item("DIAPCS").ToString) <> 0 Or Val(Ro.Item("DIAWT").ToString) <> 0 Then
                Str = RSet("DIA P", 6) & ":" & RSet(IIf(Val(Ro.Item("DIAPCS").ToString) <> 0, Ro.Item("DIAPCS").ToString, ""), 5)
                Str += " " & RSet("WT:", 4) & RSet(IIf(Val(Ro.Item("DIAWT").ToString) <> 0, Format(Val(Ro.Item("DIAWT").ToString), FormatNumberStyle(Rnd)), ""), 9)
                PrintLine(Str)
            End If
            If Val(Ro.Item("STNPCS").ToString) <> 0 Or Val(Ro.Item("STNWT").ToString) <> 0 Then
                Str = RSet("STN P", 6) & ":" & RSet(IIf(Val(Ro.Item("STNPCS").ToString) <> 0, Ro.Item("STNPCS").ToString, ""), 5)
                Str += " " & RSet("WT:", 4) & RSet(IIf(Val(Ro.Item("STNWT").ToString) <> 0, Format(Val(Ro.Item("STNWT").ToString), "0.000"), ""), 9)
                PrintLine(Str)
            End If
            PrintLine("")
        Next
        oWrite.Close()
        oWrite.Dispose()
        Dim objPrintDia As New frmPrint(FileName)
        objPrintDia.ShowDialog()
    End Sub

    Private Sub PrintHead()
        PrintLine(CSet(cnCompanyName, TotalColumn))
        PrintLine(CSet("ACKNOWLEDGE SLIP FOR LOT ENTRY", TotalColumn))
        PrintLine(PrintChar("-", TotalColumn))
    End Sub


    Private Function CSet(ByVal Source As String, ByVal ColLength As String)
        Return Space((ColLength - Source.Length) / 2) & Source & Space((ColLength - Source.Length) / 2)
    End Function

    Private Function PrintChar(ByVal ch As Char, ByVal Leng As Integer) As String
        Dim retStr As String = ""
        For cnt As Integer = 1 To Leng
            retStr += ch
        Next
        Return retStr
    End Function

    Private Sub PrintLine(ByVal PrintStr As String)
        oWrite.WriteLine(PrintStr)
    End Sub

End Class
