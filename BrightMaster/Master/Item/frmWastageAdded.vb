Imports System.Data.OleDb
Public Class frmWastageAdded
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim dtGridView As New DataTable
    Dim flagSave As Boolean
    Dim cmd As OleDbCommand


    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        Try
            ' Add any initialization after the InitializeComponent() call.
            txtSno.Visible = False
            tabGeneral.BackgroundImage = bakImage
            tabGeneral.BackgroundImageLayout = ImageLayout.Stretch
            tabView.BackgroundImage = bakImage
            tabView.BackgroundImageLayout = ImageLayout.Stretch
            tabMain.ItemSize = New System.Drawing.Size(1, 1)
            'AddHandler txtTouch.KeyPress, AddressOf percentage_Keypress

            rbtCustomer.Checked = True
            rbtCustomer.Focus()

            cmbOpenParty.Items.Add("ALL")
            cmbOpenParty.Text = "ALL"
            objGPack.FillCombo(strSql, cmbOpenParty, , False)

            strSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST ORDER BY ITEMNAME"
            objGPack.FillCombo(strSql, cmbItem)

            cmbOpenItem.Items.Add("ALL")
            cmbOpenItem.Text = "ALL"
            objGPack.FillCombo(strSql, cmbOpenItem, , False)

            gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
            gridView.BorderStyle = BorderStyle.None
            With dtGridView
                .Columns.Add("SNO", GetType(Integer))
                .Columns.Add("ACNAME", GetType(String))
                .Columns.Add("ITEM", GetType(String))
                .Columns.Add("SUBITEM", GetType(String))
                .Columns.Add("TOUCH", GetType(Double))
                .Columns.Add("MAXMC", GetType(Double))
            End With
            gridView.DataSource = dtGridView
            gridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
            With gridView
                .Columns("SNO").Visible = False
                .Columns("TOUCH").Width = 50
                .Columns("TOUCH").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("MAXMC").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("MAXMC").HeaderText = "MC"
            End With
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub
    Function funcAdd(ByVal Accode As String, ByVal ItemId As Integer, ByVal subItemId As Integer) As Integer
        strSql = " INSERT INTO " & cnAdminDb & "..WMCTABLE"
        strSql += " ("
        strSql += " ACCODE,ITEMID,SUBITEMID"
        strSql += " ,TOUCH,MAXMC"
        strSql += " )"
        strSql += " VALUES"
        strSql += " ("
        strSql += " '" & Accode & "'" 'ACCODE
        strSql += " ," & ItemId & "" 'ITEMID
        strSql += " ," & subItemId & "" 'SUBITEMID
        strSql += " ," & Val(txtTouch_Amt.Text) & "" 'TOUCH
        strSql += " ," & Val(txtMc_Amt.Text) & "" 'MCGRM
        strSql += " )"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()
        MsgBox("Saved Successfully", MsgBoxStyle.Information)
    End Function
    Function funcUpdate(ByVal Accode As String, ByVal ItemId As Integer, ByVal subItemId As Integer) As Integer
        strSql = " UPDATE " & cnAdminDb & "..WMCTABLE SET"
        strSql += " ACCODE = '" & Accode & "'"
        strSql += " ,ITEMID = " & ItemId & ""
        strSql += " ,SUBITEMID = " & subItemId & ""
        strSql += " ,TOUCH = " & Val(txtTouch_Amt.Text) & ""
        strSql += " ,MAXMC = " & Val(txtMc_Amt.Text) & ""
        strSql += " WHERE SLNO = " & Val(txtSno.Text) & ""
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()
        MsgBox("Updated  Successfully", MsgBoxStyle.Information)
    End Function

    Private Sub frmValueAdded_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Try
            If e.KeyChar = Chr(Keys.Enter) Then
                If cmbPartyName.Focused Then
                    Exit Sub
                End If
                If cmbItem.Focused Then
                    Exit Sub
                End If
                If cmbSubItem.Focused Then
                    Exit Sub
                End If
                If txtTouch_Amt.Focused Then
                    Exit Sub
                End If
                SendKeys.Send("{TAB}")
            ElseIf e.KeyChar = Chr(Keys.Escape) Then
                If tabMain.SelectedTab.Name = tabView.Name Then
                    tabMain.SelectedTab = tabGeneral
                    rbtCustomer.Focus()
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            If funcComboChecker("Party Name", cmbPartyName) Then Exit Sub
            If funcComboChecker("Item", cmbItem) Then Exit Sub
            If funcComboChecker("SubItem", cmbSubItem, False) Then Exit Sub
            If Val(txtTouch_Amt.Text) > 112 Then
                MsgBox("Touch Range Should be in 0 to 112", MsgBoxStyle.Information, "Message")
                txtTouch_Amt.SelectAll()
                Exit Sub
            End If
            strSql = " SELECT 1 FROM " & cnAdminDb & "..WMCTABLE"
            strSql += " WHERE ACCODE = (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbPartyName.Text & "')"
            strSql += " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem.Text & "')"
            strSql += " AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem.Text & "'),0)"
            If flagSave Then
                strSql += " AND SLNO <> " & Val(txtSno.Text) & ""
            End If
            Dim dtTemp As New DataTable
            dtTemp.Clear()
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtTemp)
            If dtTemp.Rows.Count > 0 Then
                MsgBox("Touch/Mc/Wastage Already Exist in this Party", MsgBoxStyle.Information)
                If Not cmbSubItem.Items.Count > 0 Then
                    cmbSubItem.Focus()
                Else
                    cmbItem.Focus()
                End If
                Exit Sub
            End If
            'Try
            Dim accode As String = Nothing
            Dim itemId As Integer = Nothing
            Dim subItemId As Integer = Nothing
            strSql = " SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbPartyName.Text & "'"
            dtTemp.Clear()
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtTemp)
            If dtTemp.Rows.Count > 0 Then
                accode = dtTemp.Rows(0).Item("ACCODE").ToString
            End If

            strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem.Text & "'"
            dtTemp.Clear()
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtTemp)
            If dtTemp.Rows.Count > 0 Then
                itemId = Val(dtTemp.Rows(0).Item("ITEMID").ToString)
            End If

            strSql = " SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem.Text & "'"
            dtTemp.Clear()
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtTemp)
            If dtTemp.Rows.Count > 0 Then
                subItemId = Val(dtTemp.Rows(0).Item("SUBITEMID").ToString)
            End If

            If flagSave = False Then
                funcAdd(accode, itemId, subItemId)
            Else
                funcUpdate(accode, itemId, subItemId)
            End If
            objGPack.TextClear(Me)
            flagSave = False
            txtSno.Text = ""
            If rbtCustomer.Checked Then
                rbtCustomer.Focus()
            Else
                rbtSmith.Focus()
            End If
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub

    Private Sub frmValueAdded_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Me.WindowState = FormWindowState.Maximized
            btnNew_Click(Me, New EventArgs)
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click
        Try
            tabMain.SelectedTab = tabView
            If rbtCustomer.Checked = True Then
                rbtOpenCustomer.Checked = True
                rbtOpenCustomer.Focus()
            ElseIf rbtSmith.Checked = True Then
                rbtOpenSmith.Checked = True
                rbtOpenSmith.Focus()
            End If
            btnOpenView_Click(Me, New EventArgs)

        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub

    Private Sub OpenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click
        btnOpen_Click(Me, New EventArgs)
    End Sub

    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        btnSave_Click(Me, New EventArgs)
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub cmbPartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbPartyName.KeyPress
        e.KeyChar = UCase(e.KeyChar)
        If e.KeyChar = Chr(Keys.Enter) Then
            If funcComboChecker("Party Name", cmbPartyName) Then
                Exit Sub
            End If
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub cmbItem_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbItem.KeyPress
        Try
            e.KeyChar = UCase(e.KeyChar)
            If e.KeyChar = Chr(Keys.Enter) Then
                If funcComboChecker("Item", cmbItem) Then
                    Exit Sub
                End If
                strSql = " SELECT 1 FROM " & cnAdminDb & "..WMCTABLE"
                strSql += " WHERE ACCODE = (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbPartyName.Text & "')"
                strSql += " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem.Text & "')"
                strSql += " AND SUBITEMID = 0"
                If flagSave Then
                    strSql += " AND SLNO <> " & Val(txtSno.Text) & ""
                End If
                Dim dtTemp As New DataTable
                dtTemp.Clear()
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtTemp)
                If dtTemp.Rows.Count > 0 Then
                    MsgBox("Touch/Mc/Wastage Already Exist in this Party", MsgBoxStyle.Information)
                    cmbItem.Focus()
                    Exit Sub
                End If
                SendKeys.Send("{TAB}")
            End If
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub

    Private Sub cmbItem_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbItem.LostFocus
        Try
            strSql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE"
            strSql += " ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME"
            strSql += " = '" & cmbItem.Text & "') ORDER BY SUBITEMNAME"
            objGPack.FillCombo(strSql, cmbSubItem, , False)
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub

    Private Sub cmbSubItem_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSubItem.GotFocus
        If Not cmbSubItem.Items.Count > 0 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub cmbSubItem_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbSubItem.KeyPress
        Try
            e.KeyChar = UCase(e.KeyChar)
            If e.KeyChar = Chr(Keys.Enter) Then
                If funcComboChecker("SubItem", cmbSubItem) Then
                    Exit Sub
                End If
                strSql = " SELECT 1 FROM " & cnAdminDb & "..WMCTABLE"
                strSql += " WHERE ACCODE = (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbPartyName.Text & "')"
                strSql += " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem.Text & "')"
                strSql += " AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem.Text & "'),0)"
                If flagSave Then
                    strSql += " AND SLNO <> " & Val(txtSno.Text) & ""
                End If
                Dim dtTemp As New DataTable
                dtTemp.Clear()
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtTemp)
                If dtTemp.Rows.Count > 0 Then
                    MsgBox("Touch/Mc/Wastage Already Exist in this Party", MsgBoxStyle.Information)
                    cmbSubItem.Focus()
                    Exit Sub
                End If
                SendKeys.Send("{TAB}")
            End If
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        Try
            objGPack.TextClear(Me)
            flagSave = False
            cmbPartyName.Text = ""
            cmbItem.Text = ""
            cmbSubItem.Text = ""
            rbtCustomer.Focus()
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub

    Private Sub cmbOpenItem_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbOpenItem.LostFocus
        Try
            strSql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE"
            strSql += " ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME"
            strSql += " = '" & cmbOpenItem.Text & "') ORDER BY SUBITEMNAME"
            cmbOpenSubItem.Items.Clear()
            cmbOpenSubItem.Items.Add("ALL")
            cmbOpenSubItem.Text = "ALL"
            objGPack.FillCombo(strSql, cmbOpenSubItem, , False)
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub

    Private Sub cmbOpenSubItem_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbOpenSubItem.GotFocus
        If Not cmbOpenSubItem.Items.Count > 0 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub btnOpenView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpenView.Click
        Try
            strSql = " SELECT "
            strSql += " SLNO,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = W.ACCODE)AS ACNAME"
            strSql += " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = W.ITEMID)AS ITEM"
            strSql += " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = W.SUBITEMID)AS SUBITEM"
            strSql += " ,TOUCH,MAXMC"
            strSql += " FROM " & cnAdminDb & "..WMCTABLE AS W "
            If rbtOpenCustomer.Checked Then '
                strSql += " WHERE ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACTYPE='D') "
            Else
                strSql += " WHERE ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACTYPE IN ('G')) "
            End If
            If UCase(cmbOpenParty.Text) <> "ALL" And cmbOpenParty.Text <> "" Then
                strSql += " AND ACCODE = (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbOpenParty.Text & "')"
            End If
            If UCase(cmbOpenItem.Text) <> "ALL" And cmbOpenItem.Text <> "" Then
                strSql += " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbOpenItem.Text & "')"
            End If
            If UCase(cmbOpenSubItem.Text) <> "ALL" And cmbOpenSubItem.Text <> "" Then
                strSql += " AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbOpenSubItem.Text & "'),0)"
            End If

            dtGridView.Rows.Clear()
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtGridView)
            If dtGridView.Rows.Count > 0 Then
                gridView.Focus()
            Else
                MsgBox("Record Not Found", MsgBoxStyle.Information)
                rbtOpenCustomer.Focus()
            End If
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        Try
            If e.KeyCode = Keys.Enter Then
                e.Handled = True
                If gridView.RowCount > 0 Then
                    strSql = " SELECT "
                    strSql += " SLNO,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = W.ACCODE)AS ACNAME"
                    strSql += " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = W.ITEMID)AS ITEM"
                    strSql += " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = W.SUBITEMID)AS SUBITEM"
                    strSql += " ,TOUCH,MAXMC"
                    strSql += " FROM " & cnAdminDb & "..WMCTABLE AS W"
                    strSql += " WHERE SLNO = " & Val(gridView.Rows(gridView.CurrentRow.Index).Cells("SLNO").Value.ToString) & ""
                    Dim dtTemp As New DataTable
                    da = New OleDbDataAdapter(strSql, cn)
                    da.Fill(dtTemp)
                    If dtTemp.Rows.Count > 0 Then
                        With dtTemp.Rows(0)
                            cmbPartyName.Text = .Item("ACNAME").ToString
                            cmbItem.Text = .Item("ITEM").ToString
                            cmbSubItem.Text = .Item("SUBITEM").ToString
                            txtTouch_Amt.Text = .Item("TOUCH").ToString
                            txtMc_Amt.Text = .Item("MAXMC").ToString
                            txtSno.Text = .Item("SLNO").ToString
                            flagSave = True
                            tabMain.SelectedTab = tabGeneral
                            If Val(objGPack.GetSqlValue("SELECT 1 CUS FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME='" & .Item("ACNAME").ToString & "' AND ACTYPE='D'")) > 0 Then
                                rbtCustomer.Focus()
                            Else
                                rbtSmith.Focus()
                            End If
                        End With
                    End If
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub

    Private Sub rbtCustomer_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtCustomer.CheckedChanged
        Try
            If rbtCustomer.Checked Then
                If flagSave = False Then
                    cmbPartyName.Text = ""
                End If
                strSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACGRPCODE IN (5,6) AND ACTYPE = 'D'"
                objGPack.FillCombo(strSql, cmbPartyName, , False)
                rbtCustomer.Focus()
            End If
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub

    Private Sub rbtSmith_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtSmith.CheckedChanged
        Try
            If rbtSmith.Checked Then
                If flagSave = False Then
                    cmbPartyName.Text = ""
                End If
                strSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACGRPCODE IN (5,6) AND ACTYPE in ('G')"
                objGPack.FillCombo(strSql, cmbPartyName, , False)
                rbtSmith.Focus()
            End If
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub

    Private Sub cmbItem_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbItem.SelectedIndexChanged
        cmbSubItem.Text = ""
    End Sub
    Private Sub rbtOpenCustomer_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtOpenCustomer.CheckedChanged
        Try
            If rbtOpenCustomer.Checked Then
                cmbOpenParty.Text = ""
                strSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACGRPCODE IN (5,6) AND ACTYPE ='C'"
                objGPack.FillCombo(strSql, cmbOpenParty, , False)
            End If
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub

    Private Sub rbtOpenSmith_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtOpenSmith.CheckedChanged
        Try
            If rbtOpenSmith.Checked Then
                cmbOpenParty.Text = ""
                strSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACGRPCODE IN (5,6) AND ACTYPE in ('G')"
                objGPack.FillCombo(strSql, cmbOpenParty, , False)
            End If
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub

    Private Sub txtTouch_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtTouch_Amt.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
End Class