Imports System.Data.OleDb
Public Class frmSmsTemplate
    Dim strSql As String = ""
    Dim cmd As OleDbCommand
    Dim dt As DataTable
    Dim da As New OleDbDataAdapter
    Private Sub BtnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click, btnExit.Click
        Me.Dispose()
    End Sub
    Private Sub btnupdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click, btnSave.Click
        funcUpdate()
    End Sub
    Private Sub funcClear()
        txtTemplateDescription_OWN.Clear()
    End Sub
    Private Sub funcUpdate()
        strSql = "UPDATE " & cnAdminDb & "..SMSTEMPLATE SET "
        strSql += " TEMPLATE_DESC = N'" & txtTemplateDescription_OWN.Text & "'"
        strSql += " ,USERID = '" & userId & "'"
        strSql += " ,UPDATED = '" & Format(Date.Parse(Date.Now), "yyyy-MM-dd HH:mm:ss") & "'"
        strSql += " ,ACTIVE='" & Mid(cmbActive.Text, 1, 1) & "'"
        strSql += " ,SMSTYPE='" & Mid(CmbType.Text, 1, 1) & "'"
        strSql += " WHERE "
        strSql += " TEMPLATE_CAPTION = '" & CmbTemplateName.Text & "'"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()
        MsgBox("Successfully Updated", MsgBoxStyle.Information)
        funcClear()
        CmbTemplateName.Focus()
    End Sub

    Private Sub SmsTemplate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub SmsTemplate_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        cmbActive.Text = "Yes"
        CmbType.Text = "Customer"
        funcLoadTemplateName()
        funcLoadTempalteDesc()
        chkTamilFont.Visible = True
    End Sub
    Private Sub funcLoadTemplateName()
        CmbTemplateName.Items.Clear()
        strSql = " SELECT TEMPLATE_CAPTION FROM " & cnAdminDb & "..SMSTEMPLATE"
        da = New OleDbDataAdapter(strSql, cn)
        dt = New DataTable
        da.Fill(dt)
        BrighttechPack.FillCombo(CmbTemplateName, dt, "TEMPLATE_CAPTION")
    End Sub
    Private Sub funcLoadTempalteDesc()
        strSql = "SELECT * FROM " & cnAdminDb & "..SMSTEMPLATE WHERE TEMPLATE_CAPTION =  '" & CmbTemplateName.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        dt = New DataTable
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            txtTemplateDescription_OWN.Clear()
            With dt.Rows(0)
                txtTemplateDescription_OWN.Text = .Item("TEMPLATE_DESC").ToString
                If .Item("ACTIVE").ToString = "N" Then
                    cmbActive.Text = "No"
                Else
                    cmbActive.Text = "Yes"
                End If
                If .Item("SMSTYPE").ToString = "O" Then
                    CmbType.Text = "Owner"
                Else
                    CmbType.Text = "Customer"
                End If
            End With
        End If
    End Sub

    Private Sub CmbTemplateName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles CmbTemplateName.KeyDown
        If e.KeyCode = Keys.Enter And CmbTemplateName.Text <> "" Then
            funcLoadTempalteDesc()
        End If
    End Sub

    Private Sub CmbTemplateName_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmbTemplateName.SelectedIndexChanged
        funcLoadTempalteDesc()
    End Sub

    Private Sub txtTemplateDescription_OWN_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTemplateDescription_OWN.GotFocus
        txtTemplateDescription_OWN.SelectionStart = txtTemplateDescription_OWN.TextLength
    End Sub

    Private Sub chkTamilFont_CheckedChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkTamilFont.CheckedChanged
        If chkTamilFont.Checked Then
            txtTemplateDescription_OWN.Font = New Font("baamini", 12, FontStyle.Regular)
        Else
            txtTemplateDescription_OWN.Font = New Font("VERDANA", 8, FontStyle.Bold)
        End If
    End Sub
End Class