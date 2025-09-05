Imports System.Data.OleDb
Public Class COMPANY
    Private Cmd As OleDbCommand
    Private Tran As OleDbTransaction
    Private Da As OleDbDataAdapter
    Private RCompanyId As String = ""
    Private lCn As New OleDbConnection
    Private StrSql As String
    Private CnStockDb As String
    Private CnUserId As Integer
    Private CnAdmindb As String
    Public Sub New(ByVal RCompanyId As String, ByVal CnStockdb As String, ByVal CnAdmindb As String, ByVal ConStr As String, ByVal UserId As Integer)
        Me.RCompanyId = RCompanyId
        Me.CnStockDb = CnStockdb
        Me.CnAdmindb = CnAdmindb
        Me.lCn = New OleDbConnection(ConStr)
        Me.CnUserId = UserId
    End Sub
    Public Sub RemoveInfo()
        If RCompanyId = "" Then Exit Sub
        StrSql = " SELECT "
        StrSql += " (SELECT NAME FROM " & CnStockDb & "..SYSOBJECTS WHERE ID = C.ID)AS TBLNAME"
        StrSql += " FROM " & CnStockDb & "..SYSCOLUMNS C WHERE NAME = 'COMPANYID'"
        StrSql += " ORDER BY TBLNAME"
        Dim dtTranTableName As New DataTable
        Da = New OleDbDataAdapter(StrSql, lCn)
        Da.Fill(dtTranTableName)

        Try
            Me.lCn.Open()
            Tran = Nothing
            Tran = lCn.BeginTransaction
            For Each Ro As DataRow In dtTranTableName.Rows
                Select Case Ro.Item("TBLNAME").ToString.ToUpper
                    Case "BILLCONTROL", "TBILLCONTROL", "DBINFO", "SNOCREATOR", "TSNOCREATOR"
                        Continue For
                End Select
                StrSql = " DELETE FROM " & CnStockDb & ".." & Ro.Item("TBLNAME").ToString
                StrSql += " WHERE ISNULL(COMPANYID,'') = '" & RCompanyId & "'"
                Cmd = New OleDbCommand(StrSql, lCn, Tran) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
            Next
            StrSql = " INSERT INTO " & CnAdmindb & "..RINFO(USERID,UPDATED,UPTIME)"
            StrSql += " SELECT " & CnUserId & ",'" & Today.Date.ToString("yyyy-MM-dd") & "','" & Date.Now.ToShortTimeString & "'"
            Cmd = New OleDbCommand(StrSql, lCn, Tran) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
            Tran.Commit()
            Tran = Nothing
        Catch ex As Exception
            If Not Tran Is Nothing Then Tran.Rollback()
            Exit Sub
        End Try
    End Sub
End Class
