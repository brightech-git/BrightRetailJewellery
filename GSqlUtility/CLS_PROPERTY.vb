Public Class CLS_PROPERTY
    Private DbName As String
    Private RestoreDtFileNames As New DataTable("RESTOREFILENAMES")
    Public Sub New()
        RestoreDtFileNames.Columns.Add("FILENAME", GetType(String))
        RestoreDtFileNames.Columns.Add("FILEPATH", GetType(String))
        RestoreDtFileNames.Columns.Add("DBNAME", GetType(String))
    End Sub
#Region "Restore Properties"
    Public Property pDbName() As String
        Get
            Return DbName
        End Get
        Set(ByVal value As String)
            DbName = value
        End Set
    End Property
    Public Property pRestoreDtFileNames() As DataTable
        Get
            Return RestoreDtFileNames
        End Get
        Set(ByVal value As DataTable)
            RestoreDtFileNames = value
        End Set
    End Property
#End Region
End Class
