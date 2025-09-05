Imports System.Data.OleDb
Public Class frmGiftDenom
    Dim da As OleDbDataAdapter
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim designerId As Integer = Nothing ''Update Purpose
    Dim flagSave As Boolean = False
    Dim HideAccLink As Boolean = IIf(GetAdmindbSoftValue("HIDE_ACHARI_ACCLINK", "Y") = "Y", True, False)

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.        
    End Sub
    Function funcCallGrid() As Integer
        Dim dt As New DataTable
        dt.Clear()
        strSql = "  SELECT GV.GVID,"
        strSql += vbCrLf + "  (SELECT TOP 1 NAME FROM " & cnAdminDb & "..CREDITCARD WHERE CARDCODE = GV.GVID AND CARDTYPE='G')AS GIFTNAME,"
        strSql += vbCrLf + "  GV.PREFIX,GV.AMOUNT,REGNO,"
        strSql += vbCrLf + "  CASE WHEN GV.ACTIVE = 'Y' THEN 'YES' ELSE 'NO' END AS ACTIVE"
        strSql += vbCrLf + "  FROM " & cnAdminDb & "..GVDENOMMAST GV"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        gridView.DataSource = dt
        gridView.Columns("GIFTNAME").Width = 200
        gridView.Columns("GIFTNAME").Visible = True
        gridView.Columns("AMOUNT").Width = 60
        gridView.Columns("PREFIX").Width = 60
        gridView.Columns("ACTIVE").Width = 60
        gridView.Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridView.Columns("GVID").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridView.Columns("REGNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridView.Columns("REGNO").HeaderText = "STARTING_NO"
    End Function
    Function funcNew()
        cmbGV.Items.Clear()
        strSql = "SELECT NAME FROM " & cnAdminDb & "..CREDITCARD WHERE CARDTYPE='G' ORDER BY NAME"
        objGPack.FillCombo(strSql, cmbGV, True, False)
        objGPack.TextClear(Me)
        designerId = Nothing
        txtDenom_NUM.Text = ""
        txtPrefix.Text = ""
        txtStartNo_NUM.Text = ""
        txtDenom_NUM.Enabled = True
        flagSave = False
        cmbActive.Text = "YES"
        funcCallGrid()
        cmbGV.Select()
        Return 0
    End Function
    Function funcExit() As Integer
        Me.Close()
    End Function
    Function funcSave() As Integer
        If objGPack.Validator_Check(Me) Then Exit Function
        If flagSave = False Then
            strSql = " SELECT PREFIX FROM "
            strSql += " " & cnAdminDb & "..GVDENOMMAST WHERE PREFIX='" & txtPrefix.Text.ToString & "'"
            strSql += vbCrLf + " AND GVID IN (SELECT TOP 1 CARDCODE FROM " & cnAdminDb & "..CREDITCARD WHERE CARDTYPE='G' AND NAME='" & cmbGV.Text.ToString & "')"
            If objGPack.GetSqlValue(strSql) <> "" Then
                MsgBox("Prefix Already exists...", MsgBoxStyle.Information)
                txtPrefix.Focus()
                Exit Function
            End If

            strSql = " SELECT AMOUNT FROM "
            strSql += " " & cnAdminDb & "..GVDENOMMAST WHERE AMOUNT='" & txtDenom_NUM.Text.ToString & "' "
            strSql += vbCrLf + " AND GVID IN (SELECT TOP 1 CARDCODE FROM " & cnAdminDb & "..CREDITCARD WHERE CARDTYPE='G' AND NAME='" & cmbGV.Text.ToString & "')"
            If objGPack.GetSqlValue(strSql) <> "" Then
                MsgBox("Denomination/Amount Already exists...", MsgBoxStyle.Information)
                txtDenom_NUM.Focus()
                Exit Function
            End If

            funcAdd()
        ElseIf flagSave = True Then
            funcUpdate()
        End If
    End Function
    Function funcAdd()
        Dim CardCode As Integer = Nothing
        strSql = " SELECT CARDCODE FROM "
        strSql += " " & cnAdminDb & "..CREDITCARD WHERE CARDTYPE='G' AND NAME='" & cmbGV.Text.ToString & "'"
        CardCode = Val(objGPack.GetSqlValue(strSql).ToString)
        If CardCode = 0 Then
            MsgBox("Gift Voucher not found...", MsgBoxStyle.Information)
            cmbGV.Focus()
            Exit Function
        End If
        Dim dt As New DataTable
        dt.Clear()
        Dim tran As OleDbTransaction = Nothing
        Try
            tran = cn.BeginTransaction()
            strSql = " INSERT INTO " & cnAdminDb & "..GVDENOMMAST"
            strSql += " ("
            strSql += " GVID,"
            strSql += " PREFIX,AMOUNT,"
            strSql += " REGNO,"
            strSql += " ACTIVE,"
            strSql += " USERID,"
            strSql += " UPDATED,"
            strSql += " UPTIME"
            strSql += " )VALUES ("
            strSql += " " & CardCode & ""
            strSql += " ,'" & txtPrefix.Text & "'"
            strSql += " ,'" & txtDenom_NUM.Text & "'"
            strSql += " ,'" & Val(txtStartNo_NUM.Text.ToString) & "'"
            strSql += " ,'" & Mid(cmbActive.Text, 1, 1) & "'"
            strSql += " ," & userId & ""
            strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'"
            strSql += " ,'" & Date.Now.ToLongTimeString & "'"
            strSql += " )"
            ExecQuery(SyncMode.Master, strSql, cn, tran)
            tran.Commit()
            funcNew()
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
        Return 0
    End Function
    Function funcUpdate() As Integer
        Dim tran As OleDbTransaction = Nothing
        Dim CardCode As Integer = Nothing
        strSql = " SELECT TOP 1 CARDCODE FROM "
        strSql += " " & cnAdminDb & "..CREDITCARD WHERE CARDTYPE='G' AND NAME='" & cmbGV.Text.ToString & "'"
        CardCode = Val(objGPack.GetSqlValue(strSql).ToString)
        If CardCode = 0 Then
            MsgBox("Gift Voucher not found...", MsgBoxStyle.Information)
            cmbGV.Focus()
            Exit Function
        End If

        strSql = " SELECT PREFIX FROM "
        strSql += " " & cnAdminDb & "..GVDENOMMAST WHERE PREFIX = '" & txtPrefix.Text & "' AND (GVID != " & CardCode & " OR AMOUNT!='" & txtDenom_NUM.Text.ToString & "')"
        If objGPack.GetSqlValue(strSql) <> "" Then
            MsgBox("Prefix already exists...", MsgBoxStyle.Information)
            txtPrefix.Focus()
            Exit Function
        End If

        Try
            tran = cn.BeginTransaction()
            strSql = "UPDATE " & cnAdminDb & "..GVDENOMMAST SET"
            strSql += " PREFIX = '" & txtPrefix.Text & "'"
            strSql += " ,REGNO = '" & Val(txtStartNo_NUM.Text.ToString) & "'"
            strSql += " ,ACTIVE = '" & Mid(cmbActive.Text, 1, 1) & "'"
            strSql += " ,USERID = " & userId & ""
            strSql += " ,UPDATED = '" & Today.Date.ToString("yyyy-MM-dd") & "'"
            strSql += " ,UPTIME = '" & Date.Now & "'"
            strSql += " WHERE GVID = " & CardCode & " AND AMOUNT='" & txtDenom_NUM.Text.ToString & "'"
            ExecQuery(SyncMode.Master, strSql, cn, tran)
            tran.Commit()
            funcNew()
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function

    Private Sub frmDisigner_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            'If txtDesignerName__Man.Focused Then
            '    Exit Sub
            'End If
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmDisigner_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        funcGridStyle(gridView)
        cmbActive.Items.Add("YES")
        cmbActive.Items.Add("NO")
        cmbActive.Text = "YES"
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
        funcSave()
    End Sub

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click, OpenToolStripMenuItem.Click
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
            cmbGV.Focus()
        ElseIf e.KeyCode = Keys.Enter Then
            e.Handled = True
            gridView.CurrentCell = gridView.CurrentCell
            If gridView.RowCount > 0 Then
                With gridView
                    cmbGV.Text = .CurrentRow.Cells("GIFTNAME").Value.ToString
                    txtPrefix.Text = .CurrentRow.Cells("PREFIX").Value.ToString
                    txtStartNo_NUM.Text = .CurrentRow.Cells("REGNO").Value.ToString
                    txtDenom_NUM.Text = .CurrentRow.Cells("AMOUNT").Value.ToString
                    txtDenom_NUM.Enabled = False
                    cmbActive.Text = .CurrentRow.Cells("ACTIVE").Value.ToString
                    flagSave = True
                    cmbGV.Focus()
                End With
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
        'If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
        Dim CardCode As Integer = Nothing
        With gridView
            strSql = "DELETE FROM " & cnAdminDb & "..GVDENOMMAST WHERE GVID = " & .CurrentRow.Cells("GVID").Value.ToString & " AND AMOUNT='" & .CurrentRow.Cells("AMOUNT").Value.ToString & "'"
            ExecQuery(SyncMode.Master, strSql, cn, tran)
        End With
        funcCallGrid()
    End Sub

    Private Sub txtDesignerName__Man_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If e.KeyChar = Chr(Keys.Enter) Then
            
        End If
    End Sub

    Private Sub txtSeal__Man_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        'If txtSeal__Man.Text = "" Then txtSeal__Man.Text = Mid(txtDesignerName__Man.Text, 1, 5)
    End Sub

End Class