Imports System.Data.OleDb

Public Class SingleAccountsEntryFilter
    Dim cmd As New OleDbCommand
    Dim da As New OleDbDataAdapter
    Dim Strsql As String
    Public ReturnName As String = ""
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        DGV.DataSource = Nothing
        Strsql = "  SELECT UPPER(ACNAME) AS NAME,CITY,PHONENO,MOBILE"
        Strsql += vbCrLf + " ,O.AMOUNT AMOUNT,RUNNO FROM " & cnAdminDb & "..ACHEAD A"
        Strsql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..OUTSTANDING O ON A.ACCODE =O.ACCODE  "
        Strsql += vbCrLf + " WHERE 1=1 AND ISNULL(MACCODE,'') = '' AND ISNULL(O.CANCEL,'')=''"
        Strsql += vbCrLf + " AND ISNULL(ACTIVE,'Y') NOT IN ('N','H')"
        Strsql += vbCrLf + " AND (ISNULL(A.COMPANYID,'') = '' OR ISNULL(A.COMPANYID,'') LIKE '%" & strCompanyId & "%')"
        If txtname.Text.Trim <> "" Then Strsql += vbCrLf + " AND UPPER(ACNAME) like '%" & txtname.Text.ToUpper & "%'"
        If txtCity.Text.Trim <> "" Then Strsql += vbCrLf + " AND CITY='" & txtCity.Text & "'"
        If txtPhone.Text.Trim <> "" Then Strsql += vbCrLf + " AND PHONENO='" & txtPhone.Text & "'"
        If txtMobile.Text.Trim <> "" Then Strsql += vbCrLf + " AND MOBILE='" & txtMobile.Text & "'"
        If Val(txtAmount_AMT.Text) <> 0 And Val(txtAmountTo_AMT.Text) <> 0 Then Strsql += vbCrLf + " AND O.AMOUNT BETWEEN " & Val(txtAmount_AMT.Text) & " AND " & txtAmountTo_AMT.Text & ""
        Strsql += vbCrLf + " ORDER BY ACNAME"
        Dim DtView As New DataTable
        da = New OleDbDataAdapter(Strsql, cn)
        da.Fill(DtView)
        If DtView.Rows.Count > 0 Then
            DGV.DataSource = DtView
            DGV.Focus()
        Else
            MsgBox("Record not found.", MsgBoxStyle.Information)
        End If
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        FuncNew()
    End Sub
    Private Function FuncNew()
        DGV.DataSource = Nothing
        txtname.Text = ""
        txtCity.Text = ""
        txtPhone.Text = ""
        txtMobile.Text = ""
        txtAmount_AMT.Text = ""
        txtAmountTo_AMT.Text = ""
        txtReference.Text = ""
        txtname.Focus()
    End Function

    Private Sub SingleAccountsEntryFilter_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
            Me.Close()
        End If
    End Sub

    Private Sub SingleAccountsEntryFilter_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub SingleAccountsEntryFilter_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        FuncNew()
    End Sub

    Private Sub DGV_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles DGV.KeyPress
        If DGV.RowCount > 0 Then
            ReturnName = DGV.Rows(DGV.CurrentRow.Index).Cells("NAME").Value.ToString
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

 
    Private Sub btbExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btbExit.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
End Class