Imports System.Data.OleDb

Public Class SearchDialog
    Private Shared da As OleDbDataAdapter
    Public Shared Function Show( _
    ByVal Title As String _
    , ByVal Qry As String _
    , ByVal cn As OleDbConnection _
    , Optional ByVal defaultSearchColIndex As Integer = 0 _
    , Optional ByVal defaultReturnColIndex As Integer = 0 _
    , Optional ByVal style As frmSearch.GridStyle = frmSearch.GridStyle.DefaultStyle _
    , Optional ByVal defaultSearchValue As String = Nothing _
    , Optional ByVal searchKeyPnlVisible As Boolean = True _
    , Optional ByVal errMsg As Boolean = True _
    , Optional ByVal returnMust As Boolean = False _
    , Optional ByVal AutoFitColumns As Boolean = True _
    , Optional ByVal AutoFitRows As Boolean = False _
    , Optional ByVal Sort As Boolean = True _
    ) As String
        Dim dtGridData As New DataTable
        Dim cmd As OleDbCommand
        cmd = New OleDbCommand(Qry, cn)
        cmd.CommandTimeout = 1000
        da = New OleDbDataAdapter(cmd)
        da.Fill(dtGridData)
        If Not dtGridData.Rows.Count > 0 Then
            If errMsg = True Then MsgBox("Record Not Found", MsgBoxStyle.Information)
            Return ""
        End If

        Dim f As New frmSearch(Title, dtGridData _
        , defaultSearchColIndex, defaultReturnColIndex, frmSearch.GridStyle.Style2 _
        , defaultSearchValue, searchKeyPnlVisible, returnMust, AutoFitColumns, AutoFitRows, , Sort)
        If f.ShowDialog = DialogResult.OK Then
            Return f.ReturnValue
        End If
        Return ""
    End Function
    Public Shared Function Show_R( _
   ByVal Title As String _
   , ByVal Qry As String _
   , ByVal cn As OleDbConnection _
   , Optional ByVal defaultSearchColIndex As Integer = 0 _
   , Optional ByVal defaultReturnColIndex As Integer = 0 _
   , Optional ByVal style As frmSearch.GridStyle = frmSearch.GridStyle.DefaultStyle _
   , Optional ByVal defaultSearchValue As String = Nothing _
   , Optional ByVal searchKeyPnlVisible As Boolean = True _
   , Optional ByVal errMsg As Boolean = True _
   , Optional ByVal returnMust As Boolean = False _
   , Optional ByVal AutoFitColumns As Boolean = True _
   , Optional ByVal AutoFitRows As Boolean = False _
   , Optional ByVal Excel As Boolean = False _
   ) As DataRow
        Dim dtGridData As New DataTable
        da = New OleDbDataAdapter(Qry, cn)
        da.Fill(dtGridData)
        If Not dtGridData.Rows.Count > 0 Then
            If errMsg = True Then MsgBox("Record Not Found", MsgBoxStyle.Information)
            Return Nothing
        End If
        Dim f As New frmSearch(Title, dtGridData _
        , defaultSearchColIndex, defaultReturnColIndex, frmSearch.GridStyle.Style2 _
        , defaultSearchValue, searchKeyPnlVisible, returnMust, AutoFitColumns, AutoFitRows, Excel _
        )
        If f.ShowDialog = DialogResult.OK Then
            Return f.ReturnRow
        End If
        Return Nothing
    End Function
    Public Shared Function Show( _
ByVal Title As String _
, ByVal Dt As Data.DataTable _
, ByVal cn As OleDbConnection _
, Optional ByVal defaultSearchColIndex As Integer = 0 _
, Optional ByVal defaultReturnColIndex As Integer = 0 _
, Optional ByVal style As frmSearch.GridStyle = frmSearch.GridStyle.DefaultStyle _
, Optional ByVal defaultSearchValue As String = Nothing _
, Optional ByVal searchKeyPnlVisible As Boolean = True _
, Optional ByVal errMsg As Boolean = True _
, Optional ByVal returnMust As Boolean = False _
, Optional ByVal AutoFitColumns As Boolean = True _
, Optional ByVal AutoFitRows As Boolean = False _
) As String
        If Not Dt.Rows.Count > 0 Then
            If errMsg = True Then MsgBox("Record Not Found", MsgBoxStyle.Information)
            Return ""
        End If

        Dim f As New frmSearch(Title, Dt _
        , defaultSearchColIndex, defaultReturnColIndex, frmSearch.GridStyle.Style2 _
        , defaultSearchValue, searchKeyPnlVisible, returnMust _
        )
        If f.ShowDialog = DialogResult.OK Then
            Return f.ReturnValue
        End If
        Return ""
    End Function

End Class
