Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
'Imports CrystalDecisions.ReportSource

Public Class GiritechReport
    Dim myreport As New ReportDocument()
    Dim logoninfo As New TableLogOnInfo()
    Dim i As Integer
    Public Function ShowReport(ByVal objReport As Object, ByVal serverName As String, Optional ByVal dbName As String = "MASTER", Optional ByVal uId As String = "SA", Optional ByVal pwd As String = "") As ReportDocument
        myreport = objReport
        'If Not myreport.Database.Tables.Count > 0 Then
        '    MessageBox.Show("Record Not Found")
        '    Exit Function
        'End If
        Dim password As String
        ConInfo = New BrighttechPack.Coninfo(Application.StartupPath + "\ConInfo.ini")
        password = BrighttechPack.Methods.Decrypt(ConInfo.lDbPwd)
        If ConInfo.lDbLoginType.ToString <> "S" Then
            uId = ConInfo.lDbLoginType
        End If
        If password <> "" Then
            pwd = password
        End If
        If pwd Is Nothing Then pwd = ""

        For i = 0 To myreport.Database.Tables.Count - 1
            ' Set the connection information for current table.
            logoninfo.ConnectionInfo.ServerName = serverName
            logoninfo.ConnectionInfo.DatabaseName = dbName
            logoninfo.ConnectionInfo.UserID = uId
            logoninfo.ConnectionInfo.Password = pwd
            myreport.Database.Tables.Item(i).ApplyLogOnInfo(logoninfo)
        Next i
        Return myreport
    End Function

End Class
