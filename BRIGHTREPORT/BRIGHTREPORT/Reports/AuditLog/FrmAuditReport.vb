Imports System.Data.OleDb
Imports System.Threading
Imports System.IO

Public Class FrmAuditReport
    Dim strSql As String
    Dim dtschema As New DataTable
    Dim dtview As New DataTable
    Dim datatype As String


    Private Sub FrmReport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Panel2.Visible = False
        CmbMaster.Focus()
        strSql = "SELECT SUBSTRING(TABLE_NAME,7,30) FROM " & cnCompanyId & "AUDITLOG.INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' order by TABLE_NAME"
        objGPack.FillCombo(strSql, CmbMaster, False)
        CmbMaster.SelectedIndex = -1
        CmbTransaction.Text = ""
    End Sub



    Private Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        If CmbMaster.Text <> "" Then
            strSql = " SELECT * FROM " & cnCompanyId & "AUDITLOG..AUDIT_" & CmbMaster.Text & " WHERE 1=1"
            If CmbTransaction.Text.Trim <> "" Then
                If TxtString.Visible = True And TxtString.Text.Trim <> "" Then
                    strSql += " AND " & CmbTransaction.Text & " like '%" & TxtString.Text & "%'"
                ElseIf dtpFrom.Visible = True Then
                    strSql += "AND " & CmbTransaction.Text & " BETWEEN '" & dtpFrom.Value & "' AND '" & dtpTo.Value & "'"
                ElseIf TxtFrom.Visible = True And TxtFrom.Text <> "" Then
                    strSql += "AND " & CmbTransaction.Text & " = " & Val(TxtFrom.Text) & " OR " & CmbTransaction.Text & " BETWEEN " & Val(TxtFrom.Text) & " AND " & Val(TxtTo.Text) & ""
                End If
            End If

            dtview = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtview)
            If dtview.Rows.Count > 0 Then
                gridView.DataSource = dtview
            Else
                gridView.DataSource = Nothing
                MsgBox("Record Not Found")
            End If
        End If
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        CmbMaster.SelectedIndex = -1
        CmbTransaction.SelectedIndex = -1
        TxtFrom.Text = ""
        TxtTo.Text = ""
        'Label1.Visible = False
        Label2.Visible = False
        Label5.Visible = False
        'Label6.Visible = False
        Label7.Visible = False
        dtpFrom.Visible = False
        dtpTo.Visible = False
        TxtFrom.Visible = False
        TxtTo.Visible = False
        TxtString.Visible = False
        Label7.Text = ""
        CmbMaster.Focus()
    End Sub

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "", gridView, BrightPosting.GExport.GExportType.Export)
        End If
        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "", gridView, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub


    Private Sub CmbTransaction_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmbTransaction.SelectedIndexChanged
        TxtFrom.Text = ""
        TxtTo.Text = ""
        TxtString.Text = ""
        datatype = objGPack.GetSqlValue("SELECT DATA_TYPE FROM " & cnCompanyId & "AUDITLOG.INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'AUDIT_" & CmbMaster.Text & "' AND COLUMN_NAME = '" & CmbTransaction.Text & "'")
        If datatype = "" Then
            'Label1.Visible = False
            Label2.Visible = False
            Label5.Visible = False
            'Label6.Visible = False
            Label7.Visible = False
            dtpFrom.Visible = False
            dtpTo.Visible = False
            TxtFrom.Visible = False
            TxtTo.Visible = False
            TxtString.Visible = False
            Label7.Text = ""
        ElseIf datatype = "varchar" Then
            'Label1.Visible = False
            Label2.Visible = False
            Label5.Visible = False
            'Label6.Visible = False
            Label7.Visible = True
            dtpFrom.Visible = False
            dtpTo.Visible = False
            TxtFrom.Visible = False
            TxtTo.Visible = False
            TxtString.Visible = True
            Label7.Text = CmbTransaction.Text
        ElseIf datatype = "smalldatetime" Then
            'Label1.Visible = True
            Label2.Visible = True
            Label5.Visible = True
            'Label6.Visible = False
            Label7.Visible = True
            dtpFrom.Visible = True
            dtpTo.Visible = True
            TxtFrom.Visible = False
            TxtTo.Visible = False
            TxtString.Visible = False
            Label7.Text = CmbTransaction.Text
            dtpFrom.Value = GetServerDate()
            dtpTo.Value = GetServerDate()
        ElseIf datatype = "int" Then
            'Label1.Visible = False
            Label2.Visible = True
            Label5.Visible = True
            'Label6.Visible = True
            Label7.Visible = True
            dtpFrom.Visible = False
            dtpTo.Visible = False
            TxtFrom.Visible = True
            TxtTo.Visible = True
            TxtString.Visible = False
            Label7.Text = CmbTransaction.Text
        ElseIf datatype = "numeric" Then
            'Label1.Visible = False
            Label2.Visible = True
            Label5.Visible = True
            'Label6.Visible = True
            Label7.Visible = True
            dtpFrom.Visible = False
            dtpTo.Visible = False
            TxtFrom.Visible = True
            TxtTo.Visible = True
            TxtString.Visible = False
            Label7.Text = CmbTransaction.Text
        End If
    End Sub


    Private Sub CmbTransaction_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmbTransaction.Enter
        gridView.DataSource = Nothing
        CmbTransaction.Items.Clear()
        strSql = "SELECT COLUMN_NAME FROM " & cnCompanyId & "AUDITLOG.INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'AUDIT_" & CmbMaster.Text & "' ORDER BY COLUMN_NAME "
        objGPack.FillCombo(strSql, CmbTransaction, False)
        CmbTransaction.SelectedIndex = -1
        TxtString.Text = ""
        Panel2.Visible = True
    End Sub

    Private Sub CmbTransaction_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmbTransaction.TextChanged
        CmbTransaction_SelectedIndexChanged(Me, New EventArgs)
    End Sub

    Private Sub FrmReport_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.F12 Then
            btnExit_Click(Me, New EventArgs)
        ElseIf e.KeyCode = Keys.F3 Then
            btnNew_Click(Me, New EventArgs)
        End If
    End Sub


    Private Sub FrmReport_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
End Class