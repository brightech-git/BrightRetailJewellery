Imports System.Data.OleDb

Public Class frmStateItemMast
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim dtGrid As New DataTable
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        strSql = " SELECT STATENAME FROM " & cnAdminDb & "..STATEMAST ORDER BY STATENAME"
        objGPack.FillCombo(strSql, cmbState_MAN)
        cmbOpenState.Items.Add("ALL")
        objGPack.FillCombo(strSql, cmbOpenState, False, False)
        strSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST ORDER BY ITEMNAME"
        objGPack.FillCombo(strSql, cmbItem_MAN)
        strSql = " SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY ORDER BY CATNAME "
        objGPack.FillCombo(strSql, cmbCategory_MAN)
    End Sub
    Private Sub btnNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        objGPack.TextClear(Me)
        tabMain.SelectedTab = tabGeneral
        cmbState_MAN.Focus()
    End Sub
    Private Sub frmStateItemMast_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If tabMain.SelectedTab.Name = tabView.Name Then
                tabMain.SelectedTab = tabGeneral
                cmbState_MAN.Focus()
            End If
        End If
    End Sub
    Private Sub frmStateItemMast_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub frmStateItemMast_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGeneral.Left, Me.tabGeneral.Top, Me.tabGeneral.Width, Me.tabGeneral.Height))
        btnNew_Click(Me, New EventArgs)
    End Sub
    Private Sub Save()
        Dim stateId As Integer = Val(objGPack.GetSqlValue("SELECT STATEID FROM " & cnAdminDb & "..STATEMAST WHERE STATENAME = '" & cmbState_MAN.Text & "'"))
        Dim itemId As Integer = Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'"))
        Dim catCode As String = objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & cmbCategory_MAN.Text & "'")
        tran = Nothing
        tran = cn.BeginTransaction
        Try
            strSql = " SELECT 'CHECK' FROM " & cnAdminDb & "..STATEITEMMAST "
            strSql += " WHERE STATEID = " & stateId & ""
            strSql += " AND ITEMID = " & itemId & ""
            If objGPack.GetSqlValue(strSql, , , tran).Length > 0 Then
                strSql = " UPDATE " & cnAdminDb & "..STATEITEMMAST SET"
                strSql += " ITEMID = " & itemId & "" 'ITEMID
                strSql += " ,CATCODE = '" & catCode & "'" 'CATCODE
                strSql += " WHERE STATEID = " & stateId & ""
                ExecQuery(SyncMode.Master, strSql, cn, tran, , stateId)
                tran.Commit()
                btnNew_Click(Me, New EventArgs)
                MsgBox("Updated..")
            Else
                strSql = " INSERT INTO " & cnAdminDb & "..STATEITEMMAST"
                strSql += " ("
                strSql += " STATEID,ITEMID,CATCODE"
                strSql += " )"
                strSql += " VALUES"
                strSql += " ("
                strSql += " " & stateId & "" 'STATEID
                strSql += " ," & itemId & "" 'ITEMID
                strSql += " ,'" & catCode & "'" 'CATCODE
                strSql += " )"
                ExecQuery(SyncMode.Master, strSql, cn, tran, , stateId)
                tran.Commit()
                btnNew_Click(Me, New EventArgs)
                MsgBox("Inserted..")
            End If
            tran = Nothing
        Catch ex As Exception
            If tran IsNot Nothing Then
                tran.Rollback()
                tran = Nothing
            End If
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click, SaveToolStripMenuItem.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
        If objGPack.Validator_Check(Me) Then
            Exit Sub
        End If
        Try
            Save()
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub
    Private Sub CallGrid()
        strSql = " SELECT "
        strSql += " (SELECT STATENAME FROM " & cnAdminDb & "..STATEMAST WHERE STATEID = S.STATEID)aS STATE"
        strSql += " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = S.CATCODE)AS CATEGORY"
        strSql += " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.ITEMID)AS ITEM"
        strSql += " ,STATEID,CATCODE,ITEMID"
        strSql += " FROM " & cnAdminDb & "..STATEITEMMAST AS S"
        If cmbOpenState.Text <> "ALL" Then
            strSql += " WHERE STATEID = (SELECT STATEID FROM " & cnAdminDb & "..STATEMAST WHERE STATENAME = '" & cmbOpenState.Text & "')"
        End If
        dtGrid = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        gridView.DataSource = dtGrid
        gridView.Columns("STATEID").Visible = False
        gridView.Columns("CATCODE").Visible = False
        gridView.Columns("ITEMID").Visible = False
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    End Sub
    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click, OpenToolStripMenuItem.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        cmbOpenState.Text = "ALL"
        CallGrid()
        tabMain.SelectedTab = tabView
        cmbOpenState.Focus()
    End Sub
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub cmbOpenState_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbOpenState.SelectedIndexChanged
        CallGrid()
    End Sub

    Private Sub cmbItem_MAN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbItem_MAN.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            strSql = " SELECT (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = S.CATCODE)AS CATEGORY FROM " & cnAdminDb & "..STATEITEMMAST S "
            strSql += " WHERE STATEID = (SELECT STATEID FROM " & cnAdminDb & "..STATEMAST WHERE STATENAME = '" & cmbState_MAN.Text & "')"
            strSql += " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "')"
            Dim dt As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                cmbCategory_MAN.Text = dt.Rows(0).Item("CATEGORY").ToString
            Else
                cmbCategory_MAN.Text = ""
            End If
        End If
    End Sub
    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
        If Not gridView.RowCount > 0 Then Exit Sub
        If MessageBox.Show("Do you want to Delete the current row?", "Del Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = MessageBoxDefaultButton.Button2 Then
            Exit Sub
        End If
        Try
            tran = Nothing
            tran = cn.BeginTransaction
            strSql = " DELETE FROM " & cnAdminDb & "..STATEITEMMAST"
            strSql += " WHERE STATEID = " & gridView.CurrentRow.Cells("STATEID").Value & ""
            strSql += " AND CATCODE = '" & gridView.CurrentRow.Cells("CATCODE").Value.ToString & "'"
            strSql += " AND ITEMID = " & gridView.CurrentRow.Cells("ITEMID").Value & ""
            ExecQuery(SyncMode.Master, strSql, cn, tran, , gridView.CurrentRow.Cells("STATEID").Value)
            tran.Commit()
            MsgBox("Deleted Successfully", MsgBoxStyle.Information)
            cmbOpenState_SelectedIndexChanged(Me, New EventArgs)
        Catch ex As Exception
            If tran.Connection IsNot Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub
End Class