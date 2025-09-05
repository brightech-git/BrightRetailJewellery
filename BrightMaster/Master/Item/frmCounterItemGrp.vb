Imports System.Data.OleDb
Public Class frmCounterItemGrp
    Dim strSql As String
    Dim flagSave As Boolean
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim id, TEMP As Integer
    Dim cefval, act As String
    Dim dt As New DataTable
    Dim designerid, itemid, ssubitem, sitem As String
    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub frmSalesCommSlab_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
 
    Private Sub frmDesignerVA_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        funcNew()
        btnOpen1_Click(Me, New EventArgs)
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Close()
    End Sub

    Private Sub btnExit1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit1.Click
        Me.Close()
    End Sub

    Function funcNew() As Integer
        objGPack.TextClear(Me)
        flagSave = False
        gridViewTarget.DataSource = Nothing

        strSql = " SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER"
        strSql += " ORDER BY ITEMCTRNAME"
        Dim dtCounter As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCounter)
        cmbItemCounter.Items.Clear()
        For k As Integer = 0 To dtCounter.Rows.Count - 1
            cmbItemCounter.Items.Add(dtCounter.Rows(k)("ITEMCTRNAME").ToString)
        Next


        strSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST"
        strSql += " ORDER BY ITEMNAME"
        Dim dtItem As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtItem)
        cmbItemName.Items.Clear()
        For k As Integer = 0 To dtItem.Rows.Count - 1
            cmbItemName.Items.Add(dtItem.Rows(k)("ITEMNAME").ToString)
        Next


        btnOpen1_Click(Me, New EventArgs)
        cmbItemCounter.Focus()
    End Function

    Private Sub btnOpen1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen1.Click
        strSql = " SELECT "
        strSql += vbCrLf + " A.SNO,B.ITEMCTRNAME,C.ITEMNAME"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMCOUNTERGROUP AS A"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMCOUNTER AS B ON A.ITEMCTRID=B.ITEMCTRID"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS C ON A.ITEMID=C.ITEMID"
        cmd = New OleDbCommand(strSql, cn)
        da = New OleDbDataAdapter(cmd)
        dt = New DataTable
        da.Fill(dt)
        gridViewTarget.DataSource = Nothing
        If dt.Rows.Count > 0 Then
            gridViewTarget.DataSource = dt
            gridViewTarget.Columns("ITEMCTRNAME").HeaderText = "ITEM COUNTER"
            gridViewTarget.Columns("SNO").Visible = False
            gridViewTarget.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        Else
            MsgBox("Record not found...", MsgBoxStyle.Information)
        End If
    End Sub

    Private Sub btnNew1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew1.Click
        objGPack.TextClear(Me)
        cmbItemCounter.Focus()
        flagSave = True
        funcNew()
        btnOpen1_Click(Me, e)
    End Sub

    Private Sub btnDelete1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete1.Click
        If gridViewTarget.Rows.Count > 0 Then
            Dim delKey As String = gridViewTarget.Rows(gridViewTarget.CurrentRow.Index).Cells("SNO").Value.ToString
            strSql = "DELETE FROM " & cnAdminDb & "..ITEMCOUNTERGROUP WHERE SNO= '" & delKey & "'"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
            btnOpen1_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub gridViewTarget_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridViewTarget.KeyDown
        If e.KeyCode = Keys.Escape Then
            cmbItemCounter.Focus()
        ElseIf e.KeyCode = Keys.Enter Then
            e.Handled = True
            If gridViewTarget.RowCount > 0 Then
                gridViewTarget.CurrentCell = gridViewTarget.CurrentCell
                cmbItemCounter.Text = gridViewTarget.CurrentRow.Cells("ITEMCTRNAME").Value.ToString
                cmbItemName.Text = gridViewTarget.CurrentRow.Cells("ITEMNAME").Value.ToString
                TEMP = Val(gridViewTarget.CurrentRow.Cells("SNO").Value.ToString)
                flagSave = True
                cmbItemCounter.Select()
                cmbItemCounter.Focus()
            End If
        End If
    End Sub

    Private Sub btnSave1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave1.Click
        strSql = "SELECT TOP 1 ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME='" & cmbItemCounter.Text.ToString & "'"
        Dim ItemctrId As String = objGPack.GetSqlValue(strSql, , "")
        If ItemctrId = "" Then MsgBox("Item Counter not found...", MsgBoxStyle.Information) : cmbItemCounter.Focus() : Exit Sub
        strSql = "SELECT TOP 1 ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbItemName.Text.ToString & "'"
        Dim ItemId As String = objGPack.GetSqlValue(strSql, , "")


        If ItemId = "" Then MsgBox("Item not found...", MsgBoxStyle.Information) : cmbItemName.Focus() : Exit Sub

        strSql = "SELECT 1 FROM " & cnAdminDb & "..ITEMCOUNTERGROUP WHERE ItemId='" & ItemId & "' AND ITEMCTRID='" & ItemctrId & "'"
        If flagSave = False And Val(objGPack.GetSqlValue(strSql, , "").ToString) > 0 Then MsgBox("Group already found for this selection...", MsgBoxStyle.Information) : cmbItemCounter.Focus() : Exit Sub

        Dim sno As Integer = 0
        strSql = "SELECT MAX(SNO)SNO FROM " & cnAdminDb & "..ITEMCOUNTERGROUP "
        sno = Val(objGPack.GetSqlValue(strSql, , "").ToString) + 1
        If flagSave = False Then
            strSql = " INSERT INTO " & cnAdminDb & "..ITEMCOUNTERGROUP "
            strSql += " (SNO,ITEMCTRID,ITEMID,USERID) VALUES "
            strSql += " ("
            strSql += " " & sno & ""
            strSql += " ," & ItemctrId & ""
            strSql += " ," & ItemId & ""
            strSql += " ," & userId & " )"
            ExecQuery(SyncMode.Master, strSql, cn)
            MsgBox("Save Successfully..", MsgBoxStyle.Information, "Message")
        Else
            strSql = " UPDATE " & cnAdminDb & "..ITEMCOUNTERGROUP SET "
            strSql += " ITEMCTRID = " & ItemctrId & " "
            strSql += " ,ITEMID = " & ItemId & " "
            strSql += " ,USERID = " & userId & ""
            strSql += " WHERE SNO = " & TEMP & ""
            ExecQuery(SyncMode.Master, strSql, cn)
            MsgBox("Update Successfully..", MsgBoxStyle.Information, "Message")
        End If
        btnNew1_Click(Me, e)
    End Sub

    Private Sub tabMain_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tabMain.SelectedIndexChanged
        If tabMain.SelectedTab.Name = "TABSLAB" Then
            btnNew1_Click(Me, e)
        End If
    End Sub

    Private Sub SaveToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem1.Click
        btnSave1_Click(Me, e)
    End Sub

    Private Sub NewToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem1.Click
        btnNew1_Click(Me, e)
    End Sub

    Private Sub ExitToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem1.Click
        btnExit1_Click(Me, e)
    End Sub

    Private Sub OpenToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem1.Click
        btnOpen1_Click(Me, e)
    End Sub
End Class