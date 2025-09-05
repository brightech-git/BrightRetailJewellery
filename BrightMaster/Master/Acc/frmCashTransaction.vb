Imports System.Data.OleDb
Public Class frmCashTransaction
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim dtCashTran As New DataTable
    Dim da As OleDbDataAdapter
    Dim tran As OleDbTransaction
    Dim editFlag As Boolean
    Dim editRowPos As Integer = Nothing
    Dim tranCode As Integer = Nothing
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Function funcFillGridCashTran() As Integer
        dtCashTran.Rows.Clear()
        strSql = " SELECT TRANCODE,TRANNAME,CASE TRANTYPE WHEN 'R' THEN 'RECEIPT' WHEN 'P' THEN 'PAYMENT' WHEN 'B' THEN 'BOTH' ELSE '' END TRANTYPE"
        strSql += " FROM " & cnADMINDB & "..CASHTRAN"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCashTran)
    End Function

    Function funcNew() As Integer
        funcFillGridCashTran()

        txtTranName__Man.Clear()
        cmbTranType.Text = "BOTH"

        editFlag = False
        txtEditTranCode.Clear()
        txtTranName__Man.Focus()
    End Function

    Function funcAdd() As Integer
        Dim dt As New DataTable
        strSql = " SELECT ISNULL(MAX(TRANCODE),0)+1 TRANCODE FROM " & cnCompanyId & "ADMINDB..CASHTRAN"
        cmd = New OleDbCommand(strSql, cn, tran)
        da = New OleDbDataAdapter(cmd)
        da.Fill(dt)
        strSql = " INSERT INTO " & cnADMINDB & "..CASHTRAN"
        strSql += " ("
        strSql += " TRANCODE,TRANNAME,TRANTYPE,USERID"
        strSql += " ,UPDATED,UPTIME"
        strSql += " )"
        strSql += " VALUES"
        strSql += " ("
        strSql += " " & Val(dt.Rows(0).Item("TRANCODE").ToString) 'TRANCODE
        strSql += " ,'" & txtTranName__Man.Text & "'" 'TRANNAME
        strSql += " ,'" & Mid(cmbTranType.Text, 1, 1) & "'" 'TRANTYPE
        strSql += " ," & userId & "" 'USERID
        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
        strSql += " )"
        ExecQuery(SyncMode.Master, strSql, cn, tran)
    End Function

    Function funcUpdate() As Integer
        strSql = " UPDATE " & cnCompanyId & "ADMINDB..CASHTRAN SET"
        strSql += " TRANNAME = '" & txtTranName__Man.Text & "'"
        strSql += " ,TRANTYPE = '" & Mid(cmbTranType.Text, 1, 1) & "'"
        strSql += " WHERE TRANCODE = " & Val(txtEditTranCode.Text) & ""
        ExecQuery(SyncMode.Master, strSql, cn, tran)
    End Function

    Function funcGetDetails(ByVal tranCode As Integer) As Integer
        Dim dt As New DataTable
        dt.Clear()
        strSql = " SELECT TRANCODE,TRANNAME,CASE TRANTYPE WHEN 'R' THEN 'RECEIPT' WHEN 'P' THEN 'PAYMENT' WHEN 'B' THEN 'BOTH' ELSE '' END TRANTYPE"
        strSql += " FROM " & cnAdminDb & "..CASHTRAN"
        strSql += " WHERE TRANCODE = " & tranCode & ""
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            With dt.Rows(0)
                txtEditTranCode.Text = Val(.Item("TRANCODE").ToString)
                txtTranName__Man.Text = .Item("TRANNAME").ToString
                cmbTranType.Text = .Item("TRANTYPE").ToString
                editFlag = True
            End With
        End If
    End Function

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub frmCashTransaction_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtTranName__Man.Focused Then
                Exit Sub
            End If
            SendKeys.Send("{TAB}")
        End If
    End Sub


    Private Sub frmCashTransaction_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        funcGridStyle(gridCashTran)
        cmbTranType.Items.Clear()
        cmbTranType.Items.Add("BOTH")
        cmbTranType.Items.Add("RECEIPT")
        cmbTranType.Items.Add("PAYMENT")
        strSql = " SELECT  TRANCODE,TRANNAME,TRANTYPE"
        strSql += " FROM " & cnAdminDb & "..CASHTRAN"
        strSql += " WHERE 1<>1"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCashTran)
        gridCashTran.DataSource = dtCashTran
        With gridCashTran
            With .Columns("TRANCODE")
                .Visible = False
            End With
            With .Columns("TRANNAME")
                .MinimumWidth = 330
            End With
            With .Columns("TRANTYPE")
                .MinimumWidth = 100
            End With
        End With
        funcNew()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        funcNew()
    End Sub

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        funcFillGridCashTran()
        gridCashTran.Select()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
        Try
            If objGPack.Validator_Check(Me) Then Exit Sub
            If objGPack.DupChecker(txtTranName__Man, "SELECT 1 FROM " & cnCompanyId & "ADMINDB..CASHTRAN WHERE TRANNAME = '" & txtTranName__Man.Text & "' AND TRANCODE <> '" & txtEditTranCode.Text & "'") Then
                Exit Sub
            End If
            tran = cn.BeginTransaction
            If editFlag = False Then
                funcAdd()
            Else
                funcUpdate()
            End If
            tran.Commit()
            funcNew()
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
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

    Private Sub gridCashTran_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridCashTran.GotFocus
        lblStatus.Visible = True
    End Sub

    Private Sub gridCashTran_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridCashTran.KeyDown
        If e.KeyCode = Keys.Enter Then
            gridCashTran.CurrentCell = gridCashTran.CurrentCell
            If gridCashTran.Rows.Count > 0 Then
                funcGetDetails(Val(gridCashTran.Rows(gridCashTran.CurrentRow.Index).Cells("TRANCODE").Value.ToString))
            End If
            txtTranName__Man.Focus()
            e.Handled = True
        ElseIf e.KeyCode = Keys.Escape Then
            txtTranName__Man.Focus()
        End If
    End Sub


    Private Sub txtTranName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtTranName__Man.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If objGPack.DupChecker(txtTranName__Man, "SELECT 1 FROM " & cnCompanyId & "ADMINDB..CASHTRAN WHERE TRANNAME = '" & txtTranName__Man.Text & "' AND TRANCODE <> '" & txtEditTranCode.Text & "'") Then
                Exit Sub
            Else
                SendKeys.Send("{TAB}")
            End If
        End If
    End Sub

    Private Sub gridCashTran_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridCashTran.LostFocus
        lblStatus.Visible = False

    End Sub

    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
    End Sub
End Class