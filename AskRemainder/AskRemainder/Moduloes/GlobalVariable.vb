Imports System.Data.OleDb
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient

Module GlobalVariable
    Public cn As New OleDb.OleDbConnection
    Public cmd As OleDb.OleDbCommand
    Public dbSourceName As String = ""
    Public dbName As String = ""
    Public compName As String = ""


    Public Function GetSqlValue(ByVal qry As String, Optional ByVal field As String = Nothing, Optional ByVal defValue As String = "", Optional ByRef tran As OleDbTransaction = Nothing) As String
        Dim G_DTable As New DataTable
        Dim G_DAdapter As New OleDbDataAdapter
        If tran Is Nothing Then
            G_DAdapter = New OleDbDataAdapter(qry, cn)
            G_DAdapter.Fill(G_DTable)
        Else
            Dim G_Cmd As New OleDbCommand(qry, cn)
            G_DAdapter = New OleDbDataAdapter(G_Cmd)
            G_DAdapter.Fill(G_DTable)
        End If
        If field <> "" Then
            If G_DTable.Rows.Count > 0 Then Return G_DTable.Rows(0).Item(field).ToString
        Else
            If G_DTable.Rows.Count > 0 Then Return G_DTable.Rows(0).Item(0).ToString
        End If
        Return defValue
    End Function


End Module
