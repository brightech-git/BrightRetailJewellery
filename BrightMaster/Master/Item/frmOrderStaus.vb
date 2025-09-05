Imports System.Data.OleDb
Public Class frmOrderStaus

    Dim strSql As String
    Dim da As New OleDbDataAdapter
    Dim cmd As New OleDbCommand
    Dim tran As OleDbTransaction
    Dim OrStateId As String
    Dim AutoGen As Boolean
    Dim OrdStateName As String

    Private Function funcFilt() As Boolean
        If txtOrderStatusName__Man.Text = "" Then
            MsgBox("Order Status Name ShouldN't Empty...")
            txtOrderStatusName__Man.Focus()
            Return False
        End If
        If txtOrderSubject_MAN.Text = "" Then
            MsgBox("Subject ShouldN't Empty...")
            txtOrderSubject_MAN.Focus()
            Return False
        End If
        Return True
    End Function
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            If funcFilt() = False Then Exit Sub

            strSql = "SELECT COUNT(*) COUNTROW FROM " & cnAdmindb & "..ORDERSTATUS WHERE ORDSTATE_NAME ='" + txtOrderStatusName__Man.Text + "'"
            If OrStateId <> "" Then
                strSql += " AND ORDSTATE_NAME <> '" + OrdStateName + "'"
            End If
            If Val(BrighttechPack.GetSqlValue(cn, strSql, "COUNTROW", "0")) <> 0 Then
                MsgBox("Order Status Name Already Exists...")
                txtOrderStatusName__Man.Focus()
                Exit Sub
            End If
            If OrStateId = "" Then
                'If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
                tran = cn.BeginTransaction
                Dim DtOrderMast As New DataTable
                DtOrderMast = BrighttechPack.GlobalMethods.GetTableStructure(cnAdminDb, "ORDERSTATUS", cn, tran)
                Dim RowOrder As DataRow = DtOrderMast.NewRow
                strSql = "SELECT ISNULL(MAX(ORDSTATE_ID),0) + 1 ORDSTATE_ID FROM " & cnAdmindb & "..ORDERSTATUS"
                RowOrder.Item("ORDSTATE_ID") = Val(BrighttechPack.GetSqlValue(cn, strSql, "ORDSTATE_ID", "1", tran))
                RowOrder.Item("ORDSTATE_NAME") = txtOrderStatusName__Man.Text
                RowOrder.Item("SUBJECT") = txtOrderSubject_MAN.Text
                RowOrder.Item("SALESMAN") = IIf(chkSalesMan.Checked = True, "Y", "N")
                RowOrder.Item("CUSTOMER") = IIf(chkCustomer.Checked = True, "Y", "N")
                RowOrder.Item("SMITH") = IIf(chkSmith.Checked = True, "Y", "N")
                RowOrder.Item("INCHARGE") = IIf(chkIncharge.Checked = True, "Y", "N")
                RowOrder.Item("AUTOGEN") = ""
                RowOrder.Item("DISPORDER") = txtDispOrder_NUM.Text.ToString
                DtOrderMast.Rows.Add(RowOrder)
                InsertData(SyncMode.Master, DtOrderMast, cn, tran, "")
            Else
                'If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Edit) Then Exit Sub
                tran = cn.BeginTransaction

                strSql = " UPDATE " & cnAdmindb & "..ORDERSTATUS SET "
                strSql += "  ORDSTATE_NAME = '" + txtOrderStatusName__Man.Text + "'"
                strSql += " ,SUBJECT = '" + txtOrderSubject_MAN.Text + "'"
                strSql += " ,SALESMAN = '" + IIf(chkSalesMan.Checked = True, "Y", "N") + "'"
                strSql += " ,CUSTOMER = '" + IIf(chkCustomer.Checked = True, "Y", "N") + "'"
                strSql += " ,SMITH = '" + IIf(chkSmith.Checked = True, "Y", "N") + "'"
                strSql += " ,INCHARGE = '" + IIf(chkIncharge.Checked = True, "Y", "N") + "'"
                strSql += " ,AUTOGEN = '" + IIf(AutoGen, "Y", "") + "'"
                strSql += " ,DISPORDER = '" + txtDispOrder_NUM.Text.ToString + "'"
                strSql += " WHERE ORDSTATE_ID = '" + OrStateId + "'"
                ExecQuery(SyncMode.Master, strSql, cn, tran, "")
            End If
            tran.Commit()
            tran = Nothing
            btnOpen_Click(Me, New EventArgs)
            btnNew_Click(Me, New EventArgs)
        Catch ex As Exception
            If tran IsNot Nothing Then
                tran.Dispose()
                tran = Nothing
            End If
            MsgBox(ex.Message & vbCrLf & ex.StackTrace)
        End Try
    End Sub

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click
        strSql = vbCrLf + " SELECT "
        strSql += vbcrlf + " 	ORDSTATE_ID,ORDSTATE_NAME,SUBJECT"
        strSql += vbcrlf + " 	,CASE WHEN SALESMAN = 'Y' THEN 'YES' ELSE 'NO' END SALESMAN"
        strSql += vbCrLf + " 	,CASE WHEN CUSTOMER = 'Y' THEN 'YES' ELSE 'NO' END CUSTOMER"
        strSql += vbCrLf + " 	,CASE WHEN SMITH = 'Y' THEN 'YES' ELSE 'NO' END SMITH"
        strSql += vbCrLf + " 	,CASE WHEN INCHARGE = 'Y' THEN 'YES' ELSE 'NO' END INCHARGE,AUTOGEN,DISPORDER"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ORDERSTATUS"
        da = New OleDbDataAdapter(strSql, cn)
        Dim dt As New DataTable
        da.Fill(dt)
        gridView.DataSource = dt
        If gridView.ColumnCount > 0 Then
            With gridView
                .Columns("ORDSTATE_ID").Visible = False
                .Columns("ORDSTATE_NAME").HeaderText = "NAME"
                .Columns("ORDSTATE_NAME").Width = 175
                .Columns("ORDSTATE_NAME").SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns("SUBJECT").Width = 195
                .Columns("SUBJECT").SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns("SALESMAN").Width = 80
                .Columns("SALESMAN").SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns("CUSTOMER").Width = 75
                .Columns("CUSTOMER").SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns("SMITH").Width = 60
                .Columns("SMITH").SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns("INCHARGE").Width = 80
                .Columns("INCHARGE").SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns("AUTOGEN").Visible = False
                .Columns("DISPORDER").HeaderText = "DISPLAY ORDER"
                .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                .Focus()
            End With
        End If
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        objGPack.TextClear(Me)
        chkCustomer.Checked = False
        chkIncharge.Checked = False
        chkSalesMan.Checked = False
        chkSmith.Checked = False
        txtDispOrder_NUM.Text = ""
        OrStateId = ""
        AutoGen = False
        txtOrderStatusName__Man.Focus()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Try
            If gridView.CurrentRow Is Nothing Then Exit Sub
            If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
            OrStateId = gridView.CurrentRow.Cells("ORDSTATE_ID").Value.ToString
            If OrStateId = "" Then
                MsgBox("Invalid Order Status...")
                txtOrderStatusName__Man.Focus()
                Exit Sub
            End If
            tran = cn.BeginTransaction
            strSql = " DELETE FROM " & cnAdminDb & "..ORDERSTATUS WHERE ORDSTATE_ID ='" + OrStateId + "'"
            ExecQuery(SyncMode.Master, strSql, cn, tran)
            tran.Commit()
            tran = Nothing
            btnOpen_Click(Me, New EventArgs)
            btnNew_Click(Me, New EventArgs)
        Catch Ex As Exception
            If tran IsNot Nothing Then
                tran.Dispose()
                tran = Nothing
            End If
            MsgBox(Ex.Message & vbCrLf & Ex.StackTrace)
        End Try
    End Sub

    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        btnSave_Click(Me, New EventArgs)
    End Sub

    Private Sub OpenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click
        btnOpen_Click(Me, New EventArgs)
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = Convert.ToChar(Keys.Enter) Then
            With gridView.CurrentRow
                If gridView.CurrentRow Is Nothing Then Exit Sub
                If .Cells("ORDSTATE_ID").Value.ToString = "Y" Then
                    MsgBox("Auto Generated Item Cannot Delete", MsgBoxStyle.Information)
                    Exit Sub
                End If
                OrStateId = .Cells("ORDSTATE_ID").Value.ToString
                txtOrderStatusName__Man.Text = .Cells("ORDSTATE_NAME").Value.ToString
                OrdStateName = txtOrderStatusName__Man.Text
                txtOrderSubject_MAN.Text = .Cells("SUBJECT").Value.ToString
                txtDispOrder_NUM.Text = .Cells("DISPORDER").Value.ToString
                chkSalesMan.Checked = IIf(.Cells("SALESMAN").Value.ToString = "YES", True, False)
                chkCustomer.Checked = IIf(.Cells("CUSTOMER").Value.ToString = "YES", True, False)
                chkSmith.Checked = IIf(.Cells("SMITH").Value.ToString = "YES", True, False)
                chkIncharge.Checked = IIf(.Cells("INCHARGE").Value.ToString = "YES", True, False)
                AutoGen = IIf(.Cells("AUTOGEN").Value.ToString = "Y", True, False)
                txtOrderStatusName__Man.Focus()
            End With
        End If
    End Sub

    Private Sub txtOrderStatusName__Man_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtOrderStatusName__Man.Leave
        If txtOrderSubject_MAN.Text = "" Then
            txtOrderSubject_MAN.Text = txtOrderStatusName__Man.Text
        End If
    End Sub

    Private Sub frmOrderStaus_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        If e.KeyChar = Convert.ToChar(Keys.Enter) Then
            If gridView.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Escape Then
            txtOrderStatusName__Man.Focus()
        ElseIf e.KeyCode = Keys.Enter Then
            e.Handled = True
        End If
    End Sub

    Private Sub frmOrderStaus_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        btnOpen_Click(Me, New EventArgs)
        btnNew_Click(Me, New EventArgs())
    End Sub
End Class