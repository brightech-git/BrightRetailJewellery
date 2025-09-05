Public Class GExport
    Public Shared UserId As Integer = 0
    Public Shared UserName As String = ""
    Public Enum GExportType
        Print = 0
        Export = 1
    End Enum
    Public Shared Sub Post(ByVal frmName As String, ByVal CompanyName As String, ByVal Title As String, ByVal Dgv As DataGridView, ByVal ExportType As GExportType, Optional ByVal OptionalDgvHeader As DataGridView = Nothing, Optional ByVal dsFooter As DataSet = Nothing, Optional ByVal Fuser As Integer = Nothing, Optional ByVal ToMailId As String = "" _
    , Optional ByVal DateFormat As String = "dd/MM/yyyy", Optional ByVal DateAsStr As Boolean = False)
        If Not Dgv.RowCount > 0 Then Exit Sub
        Dim exportTo As New List(Of String)
        If ExportType = GExportType.Print Then
            exportTo.Add("Inkjet Print")
            exportTo.Add("Dotmatrix Print")
        Else
            exportTo.Add("Ms Office Excel")
            exportTo.Add("EMail")
            exportTo.Add("Adobe PDF")
            exportTo.Add("Open Office Spread Sheet")
            exportTo.Add("Chart")
        End If
        If IO.Directory.Exists(Application.StartupPath & "\AppData") = False Then
            IO.Directory.CreateDirectory(Application.StartupPath & "\AppData")
        End If
        If Fuser = Nothing Then
            Fuser = UserId
        End If
        Dim objExp As New ExportProperties(frmName, CompanyName, exportTo, Title, Dgv, OptionalDgvHeader, dsFooter, Fuser, ToMailId, DateFormat, DateAsStr)
        objExp.ShowDialog()
    End Sub
    Public Shared Sub Post(ByVal frmName As String, ByVal CompanyName As String, ByVal Dgv As DataGridView, ByVal Title As String, ByVal ExportType As GExportType, Optional ByVal Fuser As Integer = Nothing)
        If Not Dgv.RowCount > 0 Then Exit Sub
        Dim exportTo As New List(Of String)
        If ExportType = GExportType.Print Then
            exportTo.Add("Inkjet Print")
            exportTo.Add("Dotmatrix Print")
        Else
            exportTo.Add("Ms Office Excel")
            exportTo.Add("EMail")
            exportTo.Add("Adobe PDF")
            exportTo.Add("Open Office Spread Sheet")
            exportTo.Add("Chart")
        End If
        If IO.Directory.Exists(Application.StartupPath & "\AppData") = False Then
            IO.Directory.CreateDirectory(Application.StartupPath & "\AppData")
        End If
        Dim objExp As New ExportProperties(frmName, CompanyName, exportTo, Title, Dgv, Nothing, Nothing, Fuser)
        objExp.ShowDialog()
    End Sub

End Class
