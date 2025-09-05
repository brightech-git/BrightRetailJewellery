Imports System.Data.OleDb
Public Class frmAcheadApproval
#Region "VARIABLE"
    Dim strsql As String
    Dim cmd As OleDbCommand
    Dim tran As OleDbTransaction
    Dim da As OleDbDataAdapter
    Dim dtView As New DataTable
#End Region

#Region "BUTTON EVENTS"
    Private Sub BtnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        gridViewAppAchead_Own.DataSource = Nothing
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        strsql = vbCrLf + " SELECT CONVERT(BIT,0)CHK,* "
        strsql += vbCrLf + " FROM " & cnAdminDb & "..ACHEAD WHERE ISNULL(ACTIVE,'') = '' "
        If cmbActype.Text <> "ALL" Then
            strsql += vbCrLf + " AND ACTYPE IN (SELECT TOP 1 ACTYPE FROM " & cnAdminDb & "..ACCTYPE WHERE TYPENAME = '" & cmbActype.Text & "')"
        End If
        strsql += vbCrLf + " ORDER BY ACNAME "
        dtView = New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dtView)
        With gridViewAppAchead_Own
            .DataSource = Nothing
            .DataSource = dtView
            FormatGridColumns(gridViewAppAchead_Own, False, False, True, False)
            gridViewAppAchead_Own.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
            gridViewAppAchead_Own.Invalidate()
            For Each dgvCol As DataGridViewColumn In gridViewAppAchead_Own.Columns
                dgvCol.Width = dgvCol.Width
            Next
            gridViewAppAchead_Own.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            .Columns("CHK").ReadOnly = False
        End With
    End Sub

    Private Sub BtnApprove_Click(sender As Object, e As EventArgs) Handles btnApprove.Click, SaveToolStripMenuItem.Click
        If gridViewAppAchead_Own.Rows.Count > 0 Then

            Dim AdminAccess As String = ""
            strsql = vbCrLf + " SELECT TOP 1 ISNULL(ADMINACCESS,'') ADMINACCESS FROM " & cnAdminDb & "..ROLEMASTER "
            strsql += vbCrLf + " WHERE ROLEID IN (SELECT ROLEID FROM " & cnAdminDb & "..USERROLE WHERE USERID = '" & userId & "')"
            AdminAccess = GetSqlValue(cn, strsql)
            If userId <> 999 Then
                If AdminAccess <> "Y" Then
                    MsgBox("Access Denied", MsgBoxStyle.Information)
                    Exit Sub
                End If
            End If
            With gridViewAppAchead_Own
                For i As Integer = 0 To .Rows.Count - 1
                    If .Rows(i).Cells("CHK").Value.ToString = "False" Then Continue For
                    Try
                        Me.Cursor = Cursors.WaitCursor
                        tran = Nothing
                        tran = cn.BeginTransaction
                        strsql = " UPDATE " & cnAdminDb & "..ACHEAD SET ACTIVE = 'Y' WHERE ACCODE = '" & .Rows(i).Cells("ACCODE").Value.ToString & "'"
                        cmd = New OleDbCommand(strsql, cn, tran)
                        cmd.ExecuteNonQuery()
                        tran.Commit()
                        tran = Nothing
                    Catch ex As Exception
                        If Not tran Is Nothing Then
                            tran.Rollback()
                            tran = Nothing
                            MessageBox.Show(ex.ToString)
                            Exit Sub
                        Else
                            MessageBox.Show(ex.ToString)
                            Exit Sub
                        End If
                    Finally
                        Me.Cursor = Cursors.Default
                    End Try
                Next
                FuncNew()
            End With
        End If
    End Sub

    Private Sub BtnExcel_Click(sender As Object, e As EventArgs) Handles btnExcel.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridViewAppAchead_Own.Rows.Count Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "ACCOUNT HEAD APPROVAL DETAILS", gridViewAppAchead_Own, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub BtnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridViewAppAchead_Own.Rows.Count Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "ACCOUNT HEAD APPROVAL DETAILS", gridViewAppAchead_Own, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub BtnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub
#End Region

#Region "User define function "
    Private Sub FuncNew()
        gridViewAppAchead_Own.DataSource = Nothing
        cmbActype.Items.Clear()
        strsql = " SELECT * FROM ( "
        strsql += vbCrLf + " SELECT 'ALL' TYPENAME, 0 DISPLAYORDER"
        strsql += vbCrLf + " UNION ALL"
        strsql += vbCrLf + " SELECT TYPENAME, 1 DISPLAYORDER FROM " & cnAdminDb & "..ACCTYPE   "
        strsql += vbCrLf + " ) X ORDER BY DISPLAYORDER"
        objGPack.FillCombo(strsql, cmbActype)
        cmbActype.Text = "SMITH"
    End Sub

    Private Sub ChkSelectAll_CheckedChanged(sender As Object, e As EventArgs) Handles chkSelectAll.CheckedChanged
        If gridViewAppAchead_Own.Rows.Count > 0 Then
            With gridViewAppAchead_Own
                For i As Integer = 0 To .Rows.Count - 1
                    .Rows(i).Cells("CHK").Value = chkSelectAll.Checked
                Next
            End With
        End If
    End Sub

#End Region

#Region "FORM LOAD"
    Private Sub frmAcheadApproval_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        FuncNew()
    End Sub

    Private Sub frmAcheadApproval_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
#End Region
End Class