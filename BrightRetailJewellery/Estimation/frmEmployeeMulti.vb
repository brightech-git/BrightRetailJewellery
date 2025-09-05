Imports System.Data.OleDb
Public Class frmEmployeeMulti
#Region "VARIABLE"
    Dim strsql As String
    Dim da As OleDbDataAdapter
    Dim dt As DataTable
    Dim cmd As OleDbCommand
#End Region

#Region "CONSTRUCTOR    "
    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        chkCmbEmployee.Items.Clear()
        strsql = vbCrLf + " SELECT * FROM ( "
        strsql += "SELECT 'ALL' EMPNAME, 0 EMPID"
        strsql += vbCrLf + " UNION ALL"
        strsql += vbCrLf + " SELECT EMPNAME, EMPID FROM " & cnAdminDb & "..EMPMASTER WHERE ACTIVE = 'Y' "
        strsql += vbCrLf + " )X ORDER BY EMPID"
        da = New OleDbDataAdapter(strsql, cn)
        dt = New DataTable
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            For i As Integer = 0 To dt.Rows.Count - 1
                chkCmbEmployee.Items.Add(dt.Rows(i).Item("EMPNAME").ToString)
            Next
        End If
    End Sub

#End Region

#Region "FORM LOAD"
    Private Sub frmEmployeeMulti_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
    Private Sub frmEmployeeMulti_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        ElseIf e.KeyCode = KEYS.Escape Then
            Me.Close()
        End If
    End Sub

    Private Sub chkCmbEmployee_Leave(sender As Object, e As EventArgs) Handles chkCmbEmployee.Leave
        If chkCmbEmployee.Text <> "" And chkCmbEmployee.Text <> "ALL" Then
            Dim EmpName As String = ""
            EmpName = GetChecked_CheckedList(chkCmbEmployee, True)
            strsql = " SELECT EMPID FROM " & cnAdminDb & "..EMPMASTER WHERE EMPNAME IN ( " & EmpName & ")"
            da = New OleDbDataAdapter(strsql, cn)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                Dim EmpId As String = ""
                For i As Integer = 0 To dt.Rows.Count - 1
                    EmpId += "," & dt.Rows(i).Item("EMPID").ToString
                Next
                txtEmpId.Text = EmpId.Trim(",")
                Me.Close()
            End If
        Else
            txtEmpId.Text = ""
        End If
    End Sub
#End Region
End Class