Imports System.Data.OleDb
Public Class frmCardComm
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
        strSql = "  SELECT GV.CARDCODE,"
        strSql += vbCrLf + "  (SELECT TOP 1 NAME FROM " & cnAdminDb & "..CREDITCARD WHERE CARDCODE = GV.CARDCODE AND CARDTYPE='R')AS NAME,"
        strSql += vbCrLf + "  GV.FROMAMOUNT,GV.TOAMOUNT,COMMISSION,TAX"
        strSql += vbCrLf + "  FROM " & cnAdminDb & "..CREDITCARDSLAB GV"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        gridView.DataSource = dt
        'gridView.Columns("NAME").Width = 200
        gridView.Columns("NAME").Visible = True
        'gridView.Columns("FROMAMOUNT").Width = 60
        'gridView.Columns("TOAMOUNT").Width = 60
        gridView.Columns("FROMAMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridView.Columns("TOAMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridView.Columns("CARDCODE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridView.Columns("COMMISSION").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridView.Columns("TAX").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridView.Columns("TAX").HeaderText = "GST"
    End Function
    Function funcNew()
        cmbGV.Items.Clear()
        strSql = "SELECT NAME FROM " & cnAdminDb & "..CREDITCARD WHERE CARDTYPE='R' ORDER BY NAME"
        objGPack.FillCombo(strSql, cmbGV, True, False)
        objGPack.TextClear(Me)
        designerId = Nothing
        txtFromAmount_AMT_MAN.Text = ""
        txtComm_AMT_MAN.Text = ""
        txtToAmount_AMT_MAN.Text = ""
        txtFromAmount_AMT_MAN.Enabled = True
        flagSave = False
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
            strSql = " SELECT 1 FROM "
            strSql += " " & cnAdminDb & "..CREDITCARDSLAB WHERE FROMAMOUNT<='" & txtFromAmount_AMT_MAN.Text.ToString & "' AND TOAMOUNT>='" & txtToAmount_AMT_MAN.Text.ToString & "'"
            strSql += vbCrLf + " AND CARDCODE IN (SELECT TOP 1 CARDCODE FROM " & cnAdminDb & "..CREDITCARD WHERE CARDTYPE='R' AND NAME='" & cmbGV.Text.ToString & "')"
            If objGPack.GetSqlValue(strSql) <> "" Then
                MsgBox("Slab Already exists...", MsgBoxStyle.Information)
                txtFromAmount_AMT_MAN.Focus()
                Exit Function
            End If

            strSql = " SELECT 1 FROM "
            strSql += " " & cnAdminDb & "..CREDITCARDSLAB WHERE '" & txtFromAmount_AMT_MAN.Text.ToString & "' BETWEEN FROMAMOUNT AND TOAMOUNT"
            strSql += vbCrLf + " AND CARDCODE IN (SELECT TOP 1 CARDCODE FROM " & cnAdminDb & "..CREDITCARD WHERE CARDTYPE='R' AND NAME='" & cmbGV.Text.ToString & "')"
            If objGPack.GetSqlValue(strSql) <> "" Then
                MsgBox("From Amount Already exists...", MsgBoxStyle.Information)
                txtFromAmount_AMT_MAN.Focus()
                Exit Function
            End If

            strSql = " SELECT 1 FROM "
            strSql += " " & cnAdminDb & "..CREDITCARDSLAB WHERE '" & txtToAmount_AMT_MAN.Text.ToString & "' BETWEEN FROMAMOUNT AND TOAMOUNT"
            strSql += vbCrLf + " AND CARDCODE IN (SELECT TOP 1 CARDCODE FROM " & cnAdminDb & "..CREDITCARD WHERE CARDTYPE='R' AND NAME='" & cmbGV.Text.ToString & "')"
            If objGPack.GetSqlValue(strSql) <> "" Then
                MsgBox("To Amount Already exists...", MsgBoxStyle.Information)
                txtToAmount_AMT_MAN.Focus()
                Exit Function
            End If

            'strSql = " SELECT AMOUNT FROM "
            'strSql += " " & cnAdminDb & "..GVDENOMMAST WHERE AMOUNT='" & txtFromAmount_AMT_MAN.Text.ToString & "' "
            'strSql += vbCrLf + " AND GVID IN (SELECT TOP 1 CARDCODE FROM " & cnAdminDb & "..CREDITCARD WHERE CARDTYPE='G' AND NAME='" & cmbGV.Text.ToString & "')"
            'If objGPack.GetSqlValue(strSql) <> "" Then
            '    MsgBox("Denomination/Amount Already exists...", MsgBoxStyle.Information)
            '    txtFromAmount_AMT_MAN.Focus()
            '    Exit Function
            'End If

            funcAdd()
        ElseIf flagSave = True Then
            funcUpdate()
        End If
    End Function
    Function funcAdd()
        Dim CardCode As Integer = Nothing
        strSql = " SELECT CARDCODE FROM "
        strSql += " " & cnAdminDb & "..CREDITCARD WHERE CARDTYPE='R' AND NAME='" & cmbGV.Text.ToString & "'"
        CardCode = Val(objGPack.GetSqlValue(strSql).ToString)
        If CardCode = 0 Then
            MsgBox("Card not found...", MsgBoxStyle.Information)
            cmbGV.Focus()
            Exit Function
        End If
        Dim dt As New DataTable
        dt.Clear()
        Dim tran As OleDbTransaction = Nothing
        Try
            tran = cn.BeginTransaction()
            strSql = " INSERT INTO " & cnAdminDb & "..CREDITCARDSLAB"
            strSql += " ("
            strSql += " CARDCODE,"
            strSql += " FROMAMOUNT,"
            strSql += " TOAMOUNT,"
            strSql += " COMMISSION,"
            strSql += " TAX,"
            strSql += " USERID,"
            strSql += " UPDATED,"
            strSql += " UPTIME"
            strSql += " )VALUES ("
            strSql += " " & CardCode & ""
            strSql += " ,'" & Val(txtFromAmount_AMT_MAN.Text) & "'"
            strSql += " ,'" & Val(txtToAmount_AMT_MAN.Text) & "'"
            strSql += " ,'" & Val(txtComm_AMT_MAN.Text.ToString) & "'"
            strSql += " ,'" & Val(txtGST_AMT_MAN.Text.ToString) & "'"
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
        strSql += " " & cnAdminDb & "..CREDITCARD WHERE CARDTYPE='R' AND NAME='" & cmbGV.Text.ToString & "'"
        CardCode = Val(objGPack.GetSqlValue(strSql).ToString)
        If CardCode = 0 Then
            MsgBox("Gift Voucher not found...", MsgBoxStyle.Information)
            cmbGV.Focus()
            Exit Function
        End If

        strSql = " SELECT PREFIX FROM "
        strSql += " " & cnAdminDb & "..GVDENOMMAST WHERE PREFIX = '" & txtComm_AMT_MAN.Text & "' AND (GVID != " & CardCode & " OR AMOUNT!='" & txtFromAmount_AMT_MAN.Text.ToString & "')"
        If objGPack.GetSqlValue(strSql) <> "" Then
            MsgBox("Prefix already exists...", MsgBoxStyle.Information)
            txtComm_AMT_MAN.Focus()
            Exit Function
        End If

        Try
            tran = cn.BeginTransaction()
            strSql = "UPDATE " & cnAdminDb & "..CREDITCARDSLAB SET"
            strSql += " FROMAMOUNT = '" & txtFromAmount_AMT_MAN.Text & "'"
            strSql += " ,TOAMOUNT = '" & Val(txtToAmount_AMT_MAN.Text.ToString) & "'"
            strSql += " ,COMMISSION = '" & Val(txtComm_AMT_MAN.Text.ToString) & "'"
            strSql += " ,TAX = '" & Val(txtGST_AMT_MAN.Text.ToString) & "'"
            strSql += " ,USERID = " & userId & ""
            strSql += " ,UPDATED = '" & Today.Date.ToString("yyyy-MM-dd") & "'"
            strSql += " ,UPTIME = '" & Date.Now & "'"
            strSql += " WHERE CARDCODE = " & CardCode & " "
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
                    cmbGV.Text = .CurrentRow.Cells("NAME").Value.ToString
                    txtComm_AMT_MAN.Text = .CurrentRow.Cells("COMMISSION").Value.ToString
                    txtFromAmount_AMT_MAN.Text = .CurrentRow.Cells("FROMAMOUNT").Value.ToString
                    txtToAmount_AMT_MAN.Text = .CurrentRow.Cells("TOAMOUNT").Value.ToString
                    txtComm_AMT_MAN.Text = .CurrentRow.Cells("COMMISSION").Value.ToString
                    txtGST_AMT_MAN.Text = .CurrentRow.Cells("TAX").Value.ToString
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
            strSql = "DELETE FROM " & cnAdminDb & "..CREDITCARDSLAB WHERE CARDCODE = " & .CurrentRow.Cells("CARDCODE").Value.ToString & " AND FROMAMOUNT='" & .CurrentRow.Cells("FROMAMOUNT").Value.ToString & "' AND TOAMOUNT='" & .CurrentRow.Cells("TOAMOUNT").Value.ToString & "'"
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