Imports System.Data.OleDb
Public Class frmDesignation
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim flagSave As Boolean = False
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
    Function funcNew() As Integer
        txtDesigName__Man.Clear()
        txtDesigId.Enabled = False
        flagSave = False
        txtDesigName__Man.Focus()
        funcOpen()
        txtDispOrder_NUM.Text = ""
        txtDesigId.Text = objGPack.GetMax("DesigId", "Designation", cnAdminDb)
        txtDesigName__Man.Select()
    End Function
    Function funcOpen() As Integer
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Function
        strSql = "Select "
        strSql += "DesigId,DesigName ""DESIGNATION NAME"",DISPORDER DISPLAYORDER "
        strSql += "from " & cnAdminDb & "..Designation "
        funcOpenGrid(strSql, gridview)
        gridview.Columns("DESIGID").Visible = False
        gridview.Columns("DESIGNATION NAME").MinimumWidth = 415
        gridview.Focus()
        gridview.Select()
    End Function
    Function funcExit() As Integer
        Me.Close()
    End Function
    Function funcSave() As Integer
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Function
        If objGPack.Validator_Check(Me) Then Exit Function
        If objGPack.DupChecker(txtDesigName__Man, "SELECT 1 FROM " & cnAdminDb & "..DESIGNATION WHERE DESIGNAME = '" & txtDesigName__Man.Text & "' AND DESIGID <> '" & txtDesigId.Text & "'") Then
            Exit Function
        End If
        If flagSave = False Then
            funcAdd()
        Else
            funcUpdate()
        End If
    End Function
    Function funcAdd() As Integer
        Dim ds As New Data.DataSet
        ds.Clear()
        strSql = "Insert into " & cnAdminDb & "..Designation "
        strSql += "("
        strSql += "DesigId,DesigName,DISPORDER,UserId,Updated,Uptime"
        strSql += ")values("
        strSql += " '" & txtDesigId.Text & " '" 'DesigId
        strSql += ",'" & txtDesigName__Man.Text & "'" 'DesigName
        strSql += ",'" & Val(txtDispOrder_NUM.Text.ToString) & "','" & userId & "'" 'UserId
        strSql += ",'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'Updated
        strSql += ",'" & Date.Now.ToLongTimeString & "'" 'Uptime
        strSql += ")"
        Try
            ExecQuery(SyncMode.Master, strSql, cn)

            funcNew()
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function
    Function funcUpdate() As Integer
        strSql = " UPDATE " & cnAdminDb & "..DESIGNATION SET"
        strSql += " DESIGNAME= '" & txtDesigName__Man.Text & "'"
        strSql += ",USERID='" & userId & "'"
        strSql += ",DISPORDER=" & Val(txtDispOrder_NUM.Text.ToString) & ""
        strSql += ",UPDATED='" & Today.Date.ToString("yyyy-MM-dd") & " '"
        strSql += ",UPTIME='" & Date.Now.ToLongTimeString & " '"
        strSql += " WHERE DESIGID='" & txtDesigId.Text & "'"
        Try
            ExecQuery(SyncMode.Master, strSql, cn)

            funcNew()
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function
    Function funcGetDetails(ByVal temp As Integer) As Integer
        Dim dt As New DataTable
        dt.Clear()
        strSql = " Select "
        strSql += " DesigId,DesigName,DISPORDER "
        strSql += " from " & cnAdminDb & "..Designation"
        strSql += " where DesigId = '" & temp & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Return 0
        End If
        With dt.Rows(0)
            txtDesigId.Text = .Item("DesigId").ToString
            txtDesigName__Man.Text = .Item("DesigName").ToString    
            txtDispOrder_NUM.Text = .Item("DISPORDER").ToString
        End With
        txtDesigId.Enabled = False
        flagSave = True
    End Function

    Private Sub frmDesignation_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If gridview.Focused = True Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmDesignation_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        funcGridStyle(gridview)
        funcNew()
    End Sub

    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        funcSave()
    End Sub

    Private Sub OpenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click
        funcOpen()
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        funcNew()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        funcExit()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        funcSave()
    End Sub

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click
        funcOpen()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        funcNew()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        funcExit()
    End Sub

    Private Sub txtDesigName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtDesigName__Man.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If objGPack.DupChecker(txtDesigName__Man, "SELECT 1 FROM " & cnAdminDb & "..DESIGNATION WHERE DESIGNAME = '" & txtDesigName__Man.Text & "' AND DESIGID <> '" & txtDesigId.Text & "'") Then
                Exit Sub
            Else
                'SendKeys.Send("{TAB}")
            End If
        End If
    End Sub

    Private Sub gridview_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridview.GotFocus
        lblStatus.Visible = True
    End Sub

    Private Sub gridview_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridview.KeyDown
        If e.KeyCode = Keys.Enter Then
            If gridview.RowCount > 0 Then
                e.Handled = True
                gridview.CurrentCell = gridview.CurrentCell
                funcGetDetails(gridview.Item(0, gridview.CurrentRow.Index).Value)
                Label2.Focus()  
            End If
        ElseIf e.KeyCode = Keys.Escape Then
            txtDesigName__Man.Focus()
        End If
    End Sub

    Private Sub gridview_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridview.LostFocus

        lblStatus.Visible = False
    End Sub

    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
    End Sub
End Class