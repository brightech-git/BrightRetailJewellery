Imports System.Data.OleDb
Public Class frmSendReceiveView
    Dim strSql As String


    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        Dim Syncdb As String = cnAdminDb
        Dim uprefix As String = Mid(cnAdminDb, 1, 3)
        If GetAdmindbSoftValue("SYNC-SEPDATADB", "N", tran) = "Y" Then
            Dim usuffix As String = "UTILDB"
            If DbChecker(uprefix + usuffix) <> 0 Then syncdb = uprefix + usuffix
        End If

        strSql = " SELECT "
        strSql += " FROMID"
        strSql += " ,TOID"
        strSql += " ,SQLTEXT"
        strSql += " ,SUBSTRING(SQLTEXT,1,30)STEXT"
        strSql += " ,STATUS,UID"
        If rbtSend.Checked Then
            strSql += " FROM " & Syncdb & "..SENDSYNC as S"
        Else
            strSql += " FROM " & Syncdb & "..RECEIVESYNC as S"
        End If
        Dim ftr As String = Nothing
        If cmbCostCentre.Text <> "ALL" Then
            ftr += " WHERE TOID = '" & cmbCostCentre.Text & "'"
        End If
        If cmbStatus.Text <> "ALL" Then
            If ftr = Nothing Then ftr += " WHERE " Else ftr += " AND "
            ftr += " STATUS = '" & cmbStatus.Text & "'"
        End If
        strSql += ftr
        Dim dtGrid As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        gridView.DataSource = dtGrid
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        gridView.Columns("SQLTEXT").Visible = False
    End Sub

    Private Sub frmSendReceiveView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmSendReceiveView_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        gridView.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
        gridView.MultiSelect = True
        strSql = " SELECT COSTID "
        strSql += " FROM " & cnAdminDb & "..SYNCCOSTCENTRE S"
        cmbCostCentre.Items.Clear()
        cmbCostCentre.Items.Add("ALL")
        objGPack.FillCombo(strSql, cmbCostCentre, False, False)
        cmbCostCentre.Text = "ALL"

        cmbStatus.Text = "ALL"
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        btnExit_Click(Me, New EventArgs)
    End Sub
End Class