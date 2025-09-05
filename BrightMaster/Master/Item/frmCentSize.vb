Imports System.Data.OleDb
Public Class frmCentSize
    Dim StrSql As String
    Dim Cmd As OleDbCommand
    Dim Da As OleDbDataAdapter
    Dim flagUpdate As Boolean
    Dim updSno As Integer

    Private Sub CallGrid()
        StrSql = " SELECT SLNO,"
        StrSql += " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST AS I WHERE I.ITEMID = CR.ITEMID)AS ITEMNAME,"
        StrSql += " ISNULL((SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST AS S WHERE S.SUBITEMID = CR.SUBITEMID),'')AS SUBITEMNAME,"
        StrSql += " FROMCENT,TOCENT,SIZEDESC,GROUPID"
        StrSql += " FROM " & cnAdminDb & "..CENTSIZE AS CR"
        funcOpenGrid(StrSql, gridView)
        With gridView
            .Columns("SLNO").Visible = False
            .Columns("ITEMNAME").Width = 150
            .Columns("SUBITEMNAME").Width = 150
            .Columns("FROMCENT").HeaderText = "FROMCENT"
            .Columns("FROMCENT").Width = 70
            .Columns("FROMCENT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("TOCENT").HeaderText = "TOCENT"
            .Columns("TOCENT").Width = 70
            .Columns("TOCENT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("SIZEDESC").Width = 150
            .Columns("GROUPID").Width = 60
        End With
    End Sub

    Private Sub frmCentSize_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If gridView.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmCentSize_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        StrSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST"
        StrSql += " WHERE METALID = 'D' OR METALID = 'T'"
        StrSql += " ORDER BY ITEMNAME"
        objGPack.FillCombo(StrSql, cmbItemName_Man)
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        objGPack.TextClear(Me)
        cmbItemName_Man.Enabled = True
        cmbSubItemName_Man.Enabled = True
        CallGrid()
        flagUpdate = False
        updSno = Nothing
        cmbItemName_Man.Focus()
    End Sub

    Private Sub cmbItemName_Man_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbItemName_Man.LostFocus
        cmbSubItemName_Man.Text = ""
        StrSql = GetSubItemQry(New String() {"SUBITEMNAME"}, Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "'")))
        'StrSql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST "
        'StrSql += " WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "' AND SUBITEM = 'Y')"
        'StrSql += " ORDER BY SUBITEMNAME"
        objGPack.FillCombo(StrSql, cmbSubItemName_Man, True)
        If cmbSubItemName_Man.Items.Count > 0 Then cmbSubItemName_Man.Enabled = True Else cmbSubItemName_Man.Enabled = False
    End Sub
    Private Sub txtCentFrom_Amt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCentFrom.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
        Else
            If objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "'") = "D" And diaRnd = 4 Then
                WeightValidation(txtCentFrom, e, diaRnd)
            Else
                WeightValidation(txtCentFrom, e)
            End If
        End If
    End Sub

    Private Sub txtCentTo_Amt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCentTo.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
        Else
            If objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "'") = "D" And diaRnd = 4 Then
                WeightValidation(txtCentTo, e, diaRnd)
            Else
                WeightValidation(txtCentTo, e)
            End If
        End If
    End Sub


    Private Sub txtCentFrom_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCentFrom.LostFocus
        If objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "'") = "D" Then
            txtCentFrom.Text = IIf(Val(txtCentFrom.Text) <> 0, Format(Val(txtCentFrom.Text), FormatNumberStyle(DiaRnd)), txtCentFrom.Text)
        Else
            txtCentFrom.Text = IIf(Val(txtCentFrom.Text) <> 0, Format(Val(txtCentFrom.Text), "0.000"), txtCentFrom.Text)
        End If
    End Sub

    Private Sub txtCentTo_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCentTo.LostFocus
        If objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "'") = "D" Then
            txtCentTo.Text = IIf(Val(txtCentTo.Text) <> 0, Format(Val(txtCentTo.Text), FormatNumberStyle(DiaRnd)), txtCentTo.Text)
        Else
            txtCentTo.Text = IIf(Val(txtCentTo.Text) <> 0, Format(Val(txtCentTo.Text), "0.000"), txtCentTo.Text)
        End If
    End Sub
    Private Sub txtCentFrom_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCentFrom.GotFocus
        If flagUpdate = True Then Exit Sub
        StrSql = " SELECT MAX(TOCENT) FROM " & cnAdminDb & "..CENTSIZE"
        strSql += vbCrLf + " WHERE "
        strSql += vbCrLf + " ITEMID = ISNULL((SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "'),0)"
        strSql += vbCrLf + " AND ISNULL(SUBITEMID,0) = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItemName_Man.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "')),0)"
        Dim wt As Double = Val(objGPack.GetSqlValue(StrSql).ToString)
        If objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "'") = "D" Then
            txtCentFrom.Text = Format(wt + 0.0001, FormatNumberStyle(DiaRnd))
        Else
            txtCentFrom.Text = Format(wt + 0.001, "0.000")
        End If
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click, SaveToolStripMenuItem.Click
        ''Validation
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
        If objGPack.Validator_Check(Me) Then Exit Sub
        If txtCentFrom.Text = "" Then
            MsgBox(E0005, MsgBoxStyle.Information)
            txtCentFrom.Focus()
            Exit Sub
        End If
        If txtCentTo.Text = "" Then
            MsgBox(E0005, MsgBoxStyle.Information)
            txtCentTo.Focus()
            Exit Sub
        End If
        If Not Val(txtCentFrom.Text) <= Val(txtCentTo.Text) Then
            MsgBox(E0005 + vbCrLf + E0006 + txtCentTo.Text, MsgBoxStyle.Information)
            txtCentFrom.Focus()
            Exit Sub
        End If
        If CheckDuplicate(txtCentFrom.Text, txtCentTo.Text) = True Then
            MsgBox("Already Exist", MsgBoxStyle.Information)
            txtCentFrom.Focus()
            Exit Sub
        End If
        StrSql = " SELECT 'EXISTS' FROM " & cnAdminDb & "..CENTSIZE "
        StrSql += " WHERE SIZEDESC = '" & txtSize.Text & "'"
        StrSql += " AND "
        StrSql += " ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "')"
        StrSql += " AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItemName_Man.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "')),0)"
        If flagUpdate Then
            StrSql += " AND SLNO <> " & updSno & ""
        End If
        If objGPack.GetSqlValue(StrSql, , "-1") <> "-1" Then
            MsgBox("Already exists this size", MsgBoxStyle.Information)
            Exit Sub
        End If
        If flagUpdate Then
            UpdateEntry()
        Else
            SaveEntry()
        End If
    End Sub
    Private Sub UpdateEntry()
        Dim ItemId As Integer = Nothing
        Dim SubItemId As Integer = Nothing

        StrSql = " Select ItemId from " & cnAdminDb & "..ItemMast where"
        StrSql += " ItemName = '" & cmbItemName_Man.Text & "'"
        ItemId = Val(objGPack.GetSqlValue(StrSql))

        StrSql = " Select SubItemId from " & cnAdminDb & "..SubItemMast where"
        StrSql += " SubItemName = '" & cmbSubItemName_Man.Text & "'"
        StrSql += " AND ITEMID = " & ItemId & ""
        SubItemId = Val(objGPack.GetSqlValue(StrSql))

        StrSql = " UPDATE " & cnAdminDb & "..CENTSIZE SET"
        StrSql += " ITEMID=" & ItemId & ""
        StrSql += " ,SUBITEMID=" & SubItemId & ""
        StrSql += " ,FROMCENT=" & Val(txtCentFrom.Text) & ""
        StrSql += " ,TOCENT=" & Val(txtCentTo.Text) & ""
        StrSql += " ,SIZEDESC = '" & txtSize.Text & "'"
        StrSql += " ,USERID='" & userId & "'"
        StrSql += " ,UPDATED='" & Today.Date.ToString("yyyy-MM-dd") & "'"
        StrSql += " ,UPTIME='" & Date.Now.ToLongTimeString & "'"
        StrSql += " ,GROUPID = " & Val(txtGroupId_NUM.Text) & ""
        StrSql += " WHERE SLNO = " & updSno & ""
        Try
            ExecQuery(SyncMode.Master, StrSql, cn)
            btnNew_Click(Me, New EventArgs)
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Sub
    Private Sub SaveEntry()
        Dim ItemId As Integer = Nothing
        Dim SubItemId As Integer = Nothing

        StrSql = " Select ItemId from " & cnAdminDb & "..ItemMast where"
        StrSql += " ItemName = '" & cmbItemName_Man.Text & "'"
        ItemId = Val(objGPack.GetSqlValue(StrSql, , , tran))

        StrSql = " Select SubItemId from " & cnAdminDb & "..SubItemMast where"
        StrSql += " SubItemName = '" & cmbSubItemName_Man.Text & "'"
        StrSql += " AND ITEMID = " & ItemId & ""
        SubItemId = Val(objGPack.GetSqlValue(StrSql, , , tran))

        StrSql = " INSERT INTO " & cnAdminDb & "..CENTSIZE"
        StrSql += " ("
        StrSql += " SLNO,ITEMID,SUBITEMID,FROMCENT,TOCENT,"
        StrSql += " SIZEDESC,"
        StrSql += " USERID,UPDATED,UPTIME,GROUPID"
        StrSql += " )VALUES("
        StrSql += " " & Val(objGPack.GetSqlValue("SELECT ISNULL(MAX(SLNO),0)+1 FROM " & cnAdminDb & "..CENTSIZE")) & "" 'SLNO
        StrSql += " ," & ItemId & "" 'ItemId
        StrSql += " ," & SubItemId & "" 'SubItemId
        StrSql += " ," & Val(txtCentFrom.Text) & "" 'FromCent
        StrSql += " ," & Val(txtCentTo.Text) & "" 'ToCent
        StrSql += " ,'" & txtSize.Text & "'" 'SIZE
        StrSql += " ,'" & userId & "'" 'UserId
        StrSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'Updated
        StrSql += " ,'" & Date.Now.ToLongTimeString & "'" 'Uptime
        StrSql += " ," & Val(txtGroupId_NUM.Text) & "" 'GROUPID
        StrSql += " )"
        Try
            ExecQuery(SyncMode.Master, StrSql, cn)
            CallGrid()
            txtCentFrom.Clear()
            txtCentTo.Clear()
            txtGroupId_NUM.Clear()
            txtSize.Clear()
            txtCentFrom.Focus()
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Sub
    Function CheckDuplicate(ByVal FRMCENT As String, ByVal TOCENT As String) As Boolean
        Dim STR As String = Nothing
        Dim DT As New DataTable
        DT.Clear()
        STR = " DECLARE @FROMCENT AS FLOAT,@TOCENT AS FLOAT"
        STR += " SET @FROMCENT = " & Val(FRMCENT) & ""
        STR += " SET @TOCENT = " & Val(TOCENT) & ""
        STR += " SELECT 1 FROM " & cnAdminDb & "..CENTSIZE"
        STR += " WHERE"
        STR += " ((FROMCENT BETWEEN @FROMCENT AND @TOCENT)OR (TOCENT BETWEEN @FROMCENT AND @TOCENT))"
        STR += " AND "
        STR += " ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "')"
        STR += " AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItemName_Man.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "')),0)"
        If flagUpdate Then
            STR += " AND SLNO <> " & updSno & ""
        End If
        Da = New OleDbDataAdapter(STR, cn)
        Da.Fill(DT)
        If DT.Rows.Count > 0 Then
            Return True ''ALREADY EXIST
        End If
        Return False
    End Function

    Private Sub txtSize_MAN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtSize.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtSize.Text = "" Then
                Exit Sub
            End If
            StrSql = " SELECT 'EXISTS' FROM " & cnAdminDb & "..CENTSIZE "
            StrSql += " WHERE SIZEDESC = '" & txtSize.Text & "'"
            StrSql += " AND "
            StrSql += " ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "')"
            StrSql += " AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItemName_Man.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "')),0)"
            If flagUpdate Then
                StrSql += " AND SLNO <> " & updSno & ""
            End If
            If objGPack.GetSqlValue(StrSql, , "-1") <> "-1" Then
                MsgBox("Already exists this size", MsgBoxStyle.Information)
                Exit Sub
            End If
        End If
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
        End If
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            e.Handled = True
            If gridView.RowCount > 0 Then
                gridView.CurrentCell = gridView.CurrentCell
                If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Edit) Then Exit Sub
                GetDetails(Val(gridView.CurrentRow.Cells("SLNO").Value.ToString))
                txtCentFrom.Focus()
            End If
        End If
    End Sub
    Function GetDetails(ByVal temp As Integer) As Integer
        StrSql = " SELECT SLNO,"
        StrSql += " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST AS I WHERE I.ITEMID = CR.ITEMID)AS ITEMNAME,"
        StrSql += " ISNULL((SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST AS S WHERE S.SUBITEMID = CR.SUBITEMID),'')AS SUBITEMNAME,"
        StrSql += " FROMCENT,TOCENT,SIZEDESC,GROUPID"
        StrSql += " FROM " & cnAdminDb & "..CENTSIZE AS CR"
        StrSql += " WHERE SLNO = " & temp & ""
        Dim dt As New DataTable
        dt.Clear()
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Return 0
        End If
        With dt.Rows(0)
            cmbItemName_Man.Text = .Item("ItemName").ToString
            cmbSubItemName_Man.Text = .Item("SubItemName").ToString
            txtCentFrom.Text = .Item("FromCent").ToString
            txtCentTo.Text = .Item("ToCent").ToString
            txtSize.Text = .Item("SIZEDESC").ToString
            txtGroupId_NUM.Text = IIf(Val(.Item("GROUPID").ToString) <> 0, .Item("GROUPID").ToString, "")
        End With
        flagUpdate = True
        updSno = temp
        cmbItemName_Man.Enabled = False
        cmbSubItemName_Man.Enabled = False
    End Function

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnOpen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOpen.Click, OpenToolStripMenuItem.Click
        gridView.Focus()
    End Sub
End Class