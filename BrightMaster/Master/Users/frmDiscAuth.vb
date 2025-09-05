Imports System.Data.OleDb
Public Class frmDiscAuth
    Dim da As OleDbDataAdapter
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim Sno As Integer = Nothing ''Update Purpose
    Dim flagSave As Boolean = False

    Function funcCallGrid() As Integer
        Dim dt As New DataTable
        dt.Clear()

        strSql = " SELECT SNO,AMOUNTFROM,AMOUNTTO,NAME,MOBILENO"
        strSql += vbCrLf + " ,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID=D.COSTID)COSTNAME "
        strSql += vbCrLf + " FROM " & cnAdminDb & "..DISCAUTH D ORDER BY SNO"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        With gridView
            .DataSource = dt
            .Columns("NAME").Width = 200
            .Columns("AMOUNTFROM").Width = 100
            .Columns("AMOUNTTO").Width = 100
            .Columns("NAME").Width = 100
            .Columns("MOBILENO").Width = 200
            .Columns("SNO").Visible = False
            .Columns("AMOUNTFROM").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("AMOUNTTO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("AMOUNTFROM").HeaderText = "FROM"
            .Columns("AMOUNTTO").HeaderText = "TO"
            If .Columns.Contains("USERID") Then .Columns("USERID").Visible = False
        End With
    End Function

    Function funcNew()
        objGPack.TextClear(Me)
        strSql = vbCrLf + " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE ISNULL(ACTIVE,'Y')<>'N' ORDER BY COSTNAME"
        objGPack.FillCombo(strSql, cmbCostCentre, , False)
        Sno = Nothing
        flagSave = False
        funcCallGrid()
        cmbCostCentre.Select()
        Return 0
    End Function
    Function funcExit() As Integer
        Me.Close()
    End Function
    Function funcSave() As Integer
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Function
        If objGPack.Validator_Check(Me) Then Exit Function
        If flagSave = False Then
            funcAdd()
        ElseIf flagSave = True Then
            funcUpdate()
        End If
    End Function
    Function funcAdd()
        Dim dt As New DataTable
        dt.Clear()
        Dim Id As Integer = Nothing
        Dim tran As OleDbTransaction = Nothing
        Try
            strSql = "SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME='" & cmbCostCentre.Text & "'"
            Dim CostId As String = objGPack.GetSqlValue(strSql, "COSTID", "", )

            tran = cn.BeginTransaction()
            strSql = " SELECT ISNULL(MAX(SNO),0)+1 AS SNO FROM "
            strSql += " " & cnAdminDb & "..DISCAUTH"
            Id = Val(objGPack.GetSqlValue(strSql, , , tran))
            strSql = " INSERT INTO " & cnAdminDb & "..DISCAUTH"
            strSql += " ("
            strSql += " SNO,"
            strSql += " NAME,MOBILENO,"
            strSql += " AMOUNTFROM,"
            strSql += " AMOUNTTO,"
            strSql += " COSTID"
            strSql += " )VALUES ("
            strSql += " " & Id & ""
            strSql += " ,'" & txtName__MAN.Text & "'"
            strSql += " ,'" & txtMobileNo_MAN.Text & "'"
            strSql += " ,'" & Val(txtFromAmt_PER.Text) & "'"
            strSql += " ,'" & Val(txtToAmt_PER.Text) & "'"
            strSql += " ,'" & CostId & "'"
            strSql += " )"
            ExecQuery(SyncMode.Master, strSql, cn, tran)
            tran.Commit()
            tran = Nothing
            funcNew()
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback() : tran = Nothing
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
        Return 0
    End Function
    Function funcUpdate() As Integer
        Dim tran As OleDbTransaction = Nothing
        Try
            strSql = "SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME='" & cmbCostCentre.Text & "'"
            Dim CostId As String = objGPack.GetSqlValue(strSql, "COSTID", "", )

            tran = cn.BeginTransaction()
            strSql = "UPDATE " & cnAdminDb & "..DISCAUTH SET"
            strSql += " NAME = '" & txtName__MAN.Text & "'"
            strSql += " ,MOBILENO = '" & txtMobileNo_MAN.Text & "'"
            strSql += " ,AMOUNTFROM = '" & Val(txtFromAmt_PER.Text) & "'"
            strSql += " ,AMOUNTTO = '" & Val(txtToAmt_PER.Text) & "'"
            strSql += " ,COSTID = '" & CostId & "'"
            strSql += " WHERE SNO = " & Sno & ""
            ExecQuery(SyncMode.Master, strSql, cn, tran)
            tran.Commit()
            funcNew()
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function
    Function funcGetDetails(ByVal tempId As Integer)
        Dim dt As New DataTable
        dt.Clear()
        strSql = " SELECT SNO,AMOUNTFROM,AMOUNTTO,NAME,MOBILENO"
        strSql += vbCrLf + " ,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID=D.COSTID)COSTNAME "
        strSql += vbCrLf + " FROM " & cnAdminDb & "..DISCAUTH D "
        strSql += vbCrLf + " WHERE SNO = " & tempId
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Return 0
        End If
        With dt.Rows(0)
            txtName__MAN.Text = .Item("NAME").ToString
            txtMobileNo_MAN.Text = .Item("MOBILENO").ToString
            txtFromAmt_PER.Text = Val(.Item("AMOUNTFROM").ToString)
            txtToAmt_PER.Text = Val(.Item("AMOUNTTO").ToString)
            cmbCostCentre.Text = .Item("COSTNAME").ToString
        End With
        Sno = tempId
        Return 0
    End Function

    Private Sub frmDisigner_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtName__MAN.Focused Then
                Exit Sub
            End If
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub frmDisigner_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        funcGridStyle(gridView)
        btnNew_Click(Me, New EventArgs)
    End Sub
    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        funcSave()
    End Sub
    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        funcNew()
    End Sub
    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        funcExit()
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If cnCostId <> cnHOCostId Then MsgBox("Master Entry not allowed in Location..", MsgBoxStyle.Information) : Exit Sub
        funcSave()
    End Sub
    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click, OpenToolStripMenuItem.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        funcCallGrid()
        gridView.Focus()
    End Sub
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        funcNew()
    End Sub
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        funcExit()
    End Sub

    Private Sub gridView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.GotFocus
        lblStatus.Visible = True
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Escape Then
            txtName__MAN.Focus()
        ElseIf e.KeyCode = Keys.Enter Then
            e.Handled = True
            gridView.CurrentCell = gridView.CurrentCell
            If gridView.RowCount > 0 Then
                funcGetDetails(gridView.Item(0, gridView.CurrentRow.Index).Value.ToString)
                flagSave = True
                cmbCostCentre.Focus()
            End If
        End If
    End Sub

    Private Sub gridView_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.LostFocus
        lblStatus.Visible = False
    End Sub

    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If Not gridView.RowCount > 0 Then
            Exit Sub
        End If
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
        Dim delKey As String = gridView.Rows(gridView.CurrentRow.Index).Cells("SNO").Value.ToString
        strSql = "DELETE FROM " & cnAdminDb & "..DISCAUTH WHERE SNO = '" & delKey & "'"
        ExecQuery(SyncMode.Master, strSql, cn, tran)
        funcCallGrid()
    End Sub

    Private Sub txtDesignerName__Man_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtName__MAN.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If objGPack.DupChecker(txtName__MAN, "SELECT 1 FROM " & cnAdminDb & "..DISCAUTH WHERE NAME = '" & txtName__MAN.Text & "' AND SNO <> '" & Sno & "'") Then
                Exit Sub
            Else
                SendKeys.Send("{TAB}")
            End If
        End If
    End Sub
End Class