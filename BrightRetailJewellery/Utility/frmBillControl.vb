Imports System.Data.OleDb
Public Class frmBillControl
    Dim strSql As String
    Dim dtGridView As New DataTable
    Dim cmd As OleDbCommand

    Dim WithEvents lstSearch As New ListBox
    Dim searchSender As Control = Nothing
    Dim listModule As New List(Of String)
    Dim tempText As New TextBox

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        cmbModule.Items.Add("ALL")
        cmbModule.Items.Add("POINT OF SALE")
        cmbModule.Items.Add("ESTIMATION")
        cmbModule.Items.Add("ORDER")
        cmbModule.Items.Add("ACCOUNTS")
        cmbModule.SelectedIndex = 0
    End Sub

    Private Sub frmBillControl_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If lstSearch.Visible And (Not searchSender Is Nothing) Then
                searchSender.Select()
                lstSearch.Visible = False
                Exit Sub
            End If
            If txtGrid.Focused Then
                btnSave.Focus()
            End If
        End If
    End Sub

    Private Sub frmBillControl_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If gridView.Focused Then Exit Sub
            If txtGrid.Focused Then Exit Sub
            If txtName.Focused Then Exit Sub
            If lstSearch.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub cmbModule_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbModule.SelectedIndexChanged
        cmbType.Items.Clear()
        cmbType.Items.Add("ALL")
        cmbType.Items.Add("GENERAL")
        Select Case cmbModule.Text
            Case "ALL"
                cmbType.Items.Add("METAL")
                cmbType.Items.Add("TAX")
                cmbType.Items.Add("CATEGORY")
            Case "POINT OF SALE"
                cmbType.Items.Add("METAL")
                cmbType.Items.Add("TAX")
                cmbType.Items.Add("CATEGORY")
            Case "ESTIMATION"
            Case "ORDER"
            Case "ACCOUNTS"
        End Select
        cmbType.SelectedIndex = 0
        loadGrid()
        cmbModule.Focus()
    End Sub

    Private Sub loadGrid()
        dtGridView.Rows.Clear()
        strSql = " SELECT CASE WHEN CTLMODULE = 'P' THEN 'POINT OF SALE' "
        strSql += vbcrlf + "WHEN CTLMODULE = 'O' THEN 'ORDER'"
        strSql += vbcrlf + "WHEN CTLMODULE = 'E' THEN 'ESTIMATION'"
        strSql += vbcrlf + "WHEN CTLMODULE = 'G' THEN 'GENERAL'"
        strSql += vbcrlf + "WHEN CTLMODULE = 'A' THEN 'ACCOUNTS'"
        strSql += vbcrlf + "ELSE CTLMODULE END AS CTLMODULE"
        strSql += vbcrlf + ",CTLID,CTLNAME"
        strSql += vbcrlf + ",CASE WHEN CTLMODE = 'Y' THEN 'YES' WHEN CTLMODE = 'N' THEN 'NO' ELSE '' END CTLMODE"
        strSql += vbcrlf + ",CASE WHEN CTLTEXT = 'Y' THEN 'YES' WHEN CTLTEXT = 'N' THEN 'NO' ELSE CTLTEXT END AS CTLTEXT"
        strSql += vbcrlf + ",CTLTYPE FROM " & cnStockDb & "..BILLCONTROL A"
        strSql += vbcrlf + "LEFT JOIN " & CNADMINDB & "..COSTCENTRE B ON A.COSTID = B.COSTID  "
        strSql += vbcrlf + "WHERE CTLID LIKE '" & txtId.Text & "%'"
        strSql += vbcrlf + "AND CTLNAME LIKE '" & txtName.Text & "%'"
        strSql += vbcrlf + "AND A.COMPANYID = '" & strCompanyId & "'"

        If cmbModule.Text <> "ALL" Then
            strSql += vbcrlf + "AND CTLMODULE LIKE '" & Mid(cmbModule.Text, 1, 1) & "%'"
        End If
        If cmbType.Text <> "ALL" Then
            strSql += vbcrlf + "AND CTLID LIKE '" & Mid(cmbType.Text, 1, 3) & "-%'"
        End If
        If cmbCostCenter_MAN.Enabled And cmbCostCenter_MAN.Text <> "" Then
            strSql += vbcrlf + "AND ISNULL(B.COSTNAME,'') ='" & cmbCostCenter_MAN.Text & "'"
        End If
        strSql += vbcrlf + "ORDER BY CTLMODULE,CTLID,CTLNAME"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGridView)

        Dim dtModule As New DataTable
        dtModule = dtGridView.DefaultView.ToTable(True, "CTLMODULE")
        listModule.Clear()
        For Each dr As DataRow In dtModule.Rows
            listModule.Add(dr!CTLMODULE.ToString)
        Next

        txtGrid.Visible = False
    End Sub

    Private Sub frmBillControl_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        gridView.SelectionMode = DataGridViewSelectionMode.CellSelect
        gridView.RowTemplate.Height = cmbModule.Height
        loadGrid()
        gridView.DataSource = dtGridView
        With gridView
            .Columns("CTLMODULE").HeaderText = "MODULE"
            .Columns("CTLID").HeaderText = "ID"
            .Columns("CTLNAME").HeaderText = "NAME"
            .Columns("CTLMODE").HeaderText = "MODE"
            .Columns("CTLTEXT").HeaderText = "VALUE"

            .Columns("CTLMODULE").Width = 120
            .Columns("CTLID").Width = 150
            .Columns("CTLNAME").Width = 410
            .Columns("CTLMODE").Width = 70
            .Columns("CTLTEXT").Width = 100

            For int As Integer = 5 To gridView.ColumnCount - 1
                .Columns(5).Visible = False
            Next
        End With
        If gridView.RowCount > 0 Then
            gridView.CurrentCell = gridView.Rows(0).Cells(1)
            gridView.CurrentCell = gridView.Rows(0).Cells(0)
        End If
        If UCase(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COSTCENTRE'")) = "Y" Then
            strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            objGPack.FillCombo(strSql, cmbCostCenter_MAN, False, False)
            strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE where costid ='" & cnCostId & "'"
            cmbCostCenter_MAN.Text = objGPack.GetSqlValue(strSql)
            cmbCostCenter_MAN.Enabled = True
        Else
            cmbCostCenter_MAN.Enabled = False
        End If

        lstSearch.Visible = False
    End Sub

    Private Sub gridView_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.CellEnter
        Dim pt As Point = gridView.Location
        txtGrid.ReadOnly = False
        txtGrid.Clear()
        Select Case gridView.Columns(e.ColumnIndex).Name
            Case "CTLMODULE"
                txtGrid.Size = New Size(gridView.Columns(e.ColumnIndex).Width, txtGrid.Height)
                txtGrid.Location = pt + gridView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Location
                txtGrid.Text = gridView.CurrentCell.FormattedValue
                txtGrid.Visible = True
                txtGrid.Focus()
            Case "CTLID"
                txtGrid.Clear()
                txtGrid.Size = New Size(gridView.Columns(e.ColumnIndex).Width, txtGrid.Height)
                txtGrid.Location = pt + gridView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Location
                txtGrid.Text = gridView.CurrentCell.FormattedValue
                txtGrid.Visible = True
                txtGrid.Focus()
                txtGrid.ReadOnly = True
                txtGrid.BackColor = Color.White
            Case "CTLNAME"
                txtGrid.Clear()
                txtGrid.Size = New Size(gridView.Columns(e.ColumnIndex).Width, txtGrid.Height)
                txtGrid.Location = pt + gridView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Location
                txtGrid.Text = gridView.CurrentCell.FormattedValue
                txtGrid.Visible = True
                txtGrid.Focus()
            Case "CTLMODE"
                If gridView.CurrentRow.Cells("CTLMODE").Value.ToString <> "" Then
                    txtGrid.Size = New Size(gridView.Columns(e.ColumnIndex).Width, txtGrid.Height)
                    txtGrid.Location = pt + gridView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Location
                    txtGrid.Text = gridView.CurrentCell.FormattedValue
                    txtGrid.Visible = True
                    txtGrid.Focus()
                    'cmbGrid.Items.Clear()
                    'cmbGrid.Items.Add("YES")
                    'cmbGrid.Items.Add("NO")
                    'cmbGrid.Size = New Size(gridView.Columns(e.ColumnIndex).Width, cmbGrid.Height)
                    'cmbGrid.Location = pt + gridView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Location
                    'cmbGrid.Visible = True
                    'cmbGrid.Text = gridView.CurrentCell.FormattedValue
                    'cmbGrid.Focus()
                Else
                    txtGrid.Clear()
                    txtGrid.Size = New Size(gridView.Columns(e.ColumnIndex).Width, txtGrid.Height)
                    txtGrid.Location = pt + gridView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Location
                    txtGrid.Text = gridView.CurrentCell.FormattedValue
                    txtGrid.Visible = True
                    txtGrid.Focus()
                    txtGrid.ReadOnly = True
                    txtGrid.BackColor = Color.White
                End If
            Case "CTLTEXT"
                If gridView.Rows(e.RowIndex).Cells("CTLTYPE").Value.ToString = "B" Then
                    txtGrid.Size = New Size(gridView.Columns(e.ColumnIndex).Width, txtGrid.Height)
                    txtGrid.Location = pt + gridView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Location
                    txtGrid.Text = gridView.CurrentCell.FormattedValue
                    txtGrid.Visible = True
                    txtGrid.Focus()
                    'cmbGrid.Items.Clear()
                    'cmbGrid.Items.Add("YES")
                    'cmbGrid.Items.Add("NO")
                    'cmbGrid.Size = New Size(gridView.Columns(e.ColumnIndex).Width, cmbGrid.Height)
                    'cmbGrid.Location = pt + gridView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Location
                    'cmbGrid.Visible = True
                    'cmbGrid.Text = gridView.CurrentCell.FormattedValue
                    'cmbGrid.Focus()
                Else
                    txtGrid.Clear()
                    txtGrid.Size = New Size(gridView.Columns(e.ColumnIndex).Width, txtGrid.Height)
                    txtGrid.Location = pt + gridView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Location
                    txtGrid.Text = gridView.CurrentCell.FormattedValue
                    txtGrid.Visible = True
                    txtGrid.Focus()
                    If gridView.CurrentRow.Cells("CTLMODE").Value.ToString <> "" Then
                        If gridView.CurrentRow.Cells("CTLTYPE").Value.ToString = "N" _
                        And gridView.CurrentRow.Cells("CTLMODE").Value.ToString <> "YES" Then
                            txtGrid.ReadOnly = True
                            txtGrid.BackColor = Color.White
                        End If
                    End If
                End If
        End Select

    End Sub

    Private Sub gridView_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.CellLeave
        Select Case gridView.Columns(e.ColumnIndex).Name
            Case "CTLMODULE"
                'gridView.CurrentCell.Value = cmbGrid.Text
                gridView.CurrentCell.Value = txtGrid.Text
            Case "CTLMODE"
                'gridView.CurrentCell.Value = cmbGrid.Text
                gridView.CurrentCell.Value = txtGrid.Text
            Case "CTLNAME"
                gridView.CurrentCell.Value = txtGrid.Text
            Case "CTLTEXT"
                If gridView.Rows(e.RowIndex).Cells("CTLTYPE").Value.ToString = "B" Then
                    gridView.CurrentCell.Value = txtGrid.Text
                    'gridView.CurrentCell.Value = cmbGrid.Text
                Else
                    gridView.CurrentCell.Value = txtGrid.Text
                End If
        End Select
        txtGrid.Clear()
        txtGrid.Visible = False
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
        ElseIf e.KeyCode = Keys.Escape Then
            btnSave.Focus()
        End If
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            gridView.CurrentCell = gridView.Rows(gridView.CurrentRow.Index).Cells(gridView.CurrentCell.ColumnIndex)
        End If
    End Sub

    Private Sub txtGrid_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtGrid.KeyDown
        'If e.KeyCode = Keys.Insert Then
        '    strSql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST"
        '    strSql += vbcrlf + "WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem_Man.Text & "')"
        '    strSql += vbcrlf + "AND ACTIVE = 'Y' ORDER BY SUBITEMNAME "
        '    showSearch(txtStSubItem, strSql)
        'ElseIf e.KeyCode = Keys.Down Then
        '    If lstSearch.Visible Then
        '        lstSearch.Select()
        '    Else
        '        gridStone.Select()
        '    End If
        'ElseIf e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Tab Then
        '    lstSearch.Visible = False
        'End If
        With gridView

            If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Left Or e.KeyCode = Keys.Right Or e.KeyCode = Keys.Down Or e.KeyCode = Keys.Up Then
                If gridView.Columns(gridView.CurrentCell.ColumnIndex).Name = "CTLMODULE" Then
                    textGridValidator()
                ElseIf gridView.Columns(gridView.CurrentCell.ColumnIndex).Name = "CTLTEXT" And _
                gridView.CurrentRow.Cells("CTLTYPE").Value.ToString = "B" Then
                    textGridValidator()
                ElseIf gridView.Columns(gridView.CurrentCell.ColumnIndex).Name = "CTLMODE" Then
                    textGridValidator()
                End If
            End If

            If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Tab Then
                lstSearch.Visible = False
            ElseIf e.KeyCode = Keys.Up Then
                e.Handled = True
                If .CurrentCell.RowIndex <> 0 Then
                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.CurrentCell.ColumnIndex)
                End If
            ElseIf e.KeyCode = Keys.Down Then
                e.Handled = True
                If lstSearch.Visible Then
                    lstSearch.Select()
                ElseIf .CurrentCell.RowIndex <> .RowCount - 1 Then
                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(.CurrentCell.ColumnIndex)
                End If
            ElseIf e.KeyCode = Keys.Left Then
                e.Handled = True
                If .CurrentCell.ColumnIndex <> 0 Then
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                End If
            ElseIf e.KeyCode = Keys.Right Then
                e.Handled = True
                If gridView.Columns(gridView.CurrentCell.ColumnIndex).Name <> "CTLTEXT" Then
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                End If
            End If
        End With
    End Sub

    Private Sub txtGrid_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtGrid.KeyPress
        'If e.KeyChar = Chr(Keys.Enter) Then
        '    If txtStSubItem.Text = "" Then
        '        MsgBox("SubItem Should Not Empty", MsgBoxStyle.Information)
        '        txtStSubItem.Focus()
        '        Exit Sub
        '    End If
        '    If objGPack.DupCheck("SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem_Man.Text & "') AND SUBITEMNAME = '" & txtStSubItem.Text & "'") = False Then
        '        MsgBox("Invalid SubItem", MsgBoxStyle.Information)
        '        txtStSubItem.Focus()
        '    End If
        'End If

        If e.KeyChar = Chr(Keys.Enter) Then
            With gridView
                If Not .RowCount > 0 Then
                    cmbModule.Focus()
                    Exit Sub
                End If
                Select Case .Columns(.CurrentCell.ColumnIndex).Name
                    Case "CTLMODULE"
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells("CTLNAME")
                    Case "CTLID"
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells("CTLNAME")
                    Case "CTLNAME"
                        If .Rows(.CurrentCell.RowIndex).Cells("CTLMODE").Value.ToString <> "" Then
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells("CTLMODE")
                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells("CTLTEXT")
                        End If
                        Exit Sub
                    Case "CTLMODE"
                        If .CurrentRow.Cells("CTLMODE").Value.ToString <> "" And txtGrid.Text = "YES" Then
                            .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells("CTLTEXT")
                        Else
                            GoTo REACH_LASTROW
                        End If
                    Case "CTLTEXT"
REACH_LASTROW:
                        If .CurrentCell.RowIndex = .RowCount - 1 Then
                            gridView.CurrentCell.Value = txtGrid.Text
                            btnSave.Focus()
                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells("CTLMODULE")
                        End If
                End Select
            End With
        Else
            If gridView.Columns(gridView.CurrentCell.ColumnIndex).Name = "CTLTEXT" _
            And gridView.Rows(gridView.CurrentCell.RowIndex).Cells("CTLTYPE").Value.ToString = "N" Then
                Select Case e.KeyChar
                    Case "0" To "9", Chr(Keys.Back)
                    Case Else
                        e.Handled = True
                End Select
            End If
        End If
    End Sub

    Private Sub txtGrid_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtGrid.GotFocus
        tempText.Text = gridView.CurrentCell.Value.ToString
        txtGrid.SelectAll()
        lstSearch.Visible = False
    End Sub

    Private Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub textGridValidator()
        If lstSearch.Items.Contains(txtGrid.Text) = False Then
            txtGrid.Text = tempText.Text
        End If
    End Sub


    Private Sub txtName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtName.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If gridView.RowCount > 0 Then
                gridView.Focus()
            End If
        End If
    End Sub

    Private Sub txtId_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtId.TextChanged
        loadGrid()
        txtId.Focus()
        txtId.SelectionStart = txtId.Text.Length
    End Sub

    Private Sub txtName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtName.TextChanged
        loadGrid()
        txtName.Focus()
        txtName.SelectionStart = txtName.Text.Length
    End Sub

    Private Sub cmbType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbType.SelectedIndexChanged
        loadGrid()
        cmbType.Focus()
    End Sub

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
        If gridView.RowCount > 0 Then
            gridView.CurrentCell = gridView.Rows(0).Cells(0)
            updateBillControl()
        End If
    End Sub

    Private Sub updateBillControl()
        Try
            Dim selectcostid As String
            If cmbCostCenter_MAN.Text <> "" Then
                strSql = " SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE  ISNULL(COSTNAME,'') ='" & cmbCostCenter_MAN.Text & "'"
                selectcostid = objGPack.GetSqlValue(strSql, , , tran)
            End If
            btnSave.Enabled = False
            tran = cn.BeginTransaction
            For Each dvCol As DataGridViewRow In gridView.Rows
                Dim text As String = IIf(dvCol.Cells("CTLTYPE").Value.ToString = "B", Mid(dvCol.Cells("CTLTEXT").Value.ToString, 1, 1), dvCol.Cells("CTLTEXT").Value.ToString)
                strSql = " UPDATE " & cnStockDb & "..BILLCONTROL"
                strSql += vbcrlf + "SET CTLMODULE = '" & Mid(dvCol.Cells("CTLMODULE").Value.ToString, 1, 1) & "'"
                strSql += vbcrlf + ",CTLNAME = '" & dvCol.Cells("CTLNAME").Value.ToString & "'"
                strSql += vbcrlf + ",CTLTEXT = '" & text & "'"
                strSql += vbcrlf + ",CTLMODE = '" & Mid(dvCol.Cells("CTLMODE").Value.ToString, 1, 1) & "'"
                strSql += vbcrlf + "WHERE CTLID = '" & dvCol.Cells("CTLID").Value.ToString & "'"
                strSql += vbCrLf + "AND COMPANYID = '" & strCompanyId & "'"
                If strBCostid <> "" Then strSql += vbCrLf + "AND ISNULL(COSTID,'') = '" & selectcostid & "'"
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.ExecuteNonQuery()
            Next
            tran.Commit()
            MsgBox("Saved..")
            btnNew_Enter(Me, New EventArgs)
        Catch ex As Exception
            tran.Dispose()
            btnSave.Enabled = True
            MsgBox(ex.Message)
        Finally
            If Not tran Is Nothing Then tran.Dispose()
            btnSave.Enabled = True
        End Try
    End Sub

    Private Sub btnNew_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNew.Enter
        dtGridView.Rows.Clear()
        cmbModule.Text = "ALL"
        cmbType.Text = "ALL"
        txtId.Clear()
        txtName.Clear()
        loadGrid()
        cmbModule.Focus()
    End Sub

    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        btnSave_Click(Me, New EventArgs)
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Enter(Me, New EventArgs)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub txtGrid_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtGrid.TextChanged
        showSearch(txtGrid)
        'If txtStSubItem.Text <> "" Then
        '    strSql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST"
        '    strSql += vbcrlf + "WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem_Man.Text & "')"
        '    strSql += vbcrlf + "AND SUBITEMNAME LIKE '" & txtStSubItem.Text & "%' AND ACTIVE = 'Y' ORDER BY SUBITEMNAME"
        '    showSearch(txtStSubItem, strSql)
        '    'grpOptions.Enabled = False
        'Else
        '    lstSearch.Visible = False
        '    'grpOptions.Enabled = True
        'End If

    End Sub

    Private Sub loadLstSearch()
        lstSearch.Items.Clear()
        With gridView
            Select Case .Columns(.CurrentCell.ColumnIndex).Name
                Case "CTLMODULE"
                    For i As Integer = 0 To listModule.Count - 1
                        lstSearch.Items.Add(listModule.Item(i))
                    Next
                Case "CTLMODE"
                    If gridView.CurrentRow.Cells("CTLMODE").Value.ToString <> "" Then
                        lstSearch.Items.Add("YES")
                        lstSearch.Items.Add("NO")
                    End If
                Case "CTLTEXT"
                    If gridView.CurrentRow.Cells("CTLTYPE").Value.ToString = "B" Then
                        lstSearch.Items.Add("YES")
                        lstSearch.Items.Add("NO")
                    End If
            End Select
        End With
    End Sub


    Private Sub showSearch(ByVal sender As Control)
        loadLstSearch()
        searchSender = sender
        Dim pt As New Point(GetControlLocation(sender, New Point))
        Me.Controls.Add(lstSearch)
        lstSearch.Location = New Point(pt.X, pt.Y + sender.Height)
        If gridView.Columns(gridView.CurrentCell.ColumnIndex).Name = "CTLMODE" Or _
        gridView.Columns(gridView.CurrentCell.ColumnIndex).Name = "CTLTEXT" Then
            lstSearch.Size = New Size(sender.Width, 40)
        Else
            lstSearch.Size = New Size(sender.Width, 120)
        End If

        lstSearch.BringToFront()

        If lstSearch.Items.Count > 0 Then
            lstSearch.Visible = True
        Else
            lstSearch.Visible = False
        End If
    End Sub
    Private Function GetControlLocation(ByVal ctrl As Control, ByRef pt As Point) As Point
        If TypeOf ctrl Is Form Then
            Return pt
        Else
            pt += ctrl.Location
            Return GetControlLocation(ctrl.Parent, pt)
        End If
        Return pt
    End Function

    Public Sub TextScript(ByRef txt As TextBox, ByVal e As System.Windows.Forms.KeyEventArgs)
        Dim sComboText As String = ""
        Dim iLoop As Integer
        Dim sTempString As String
        If e.KeyCode >= 65 And e.KeyCode <= 90 Then
            'only look at letters A-Z
            sTempString = txt.Text
            If Len(sTempString) = 1 Then sComboText = sTempString
            For iLoop = 0 To (lstSearch.Items.Count - 1)
                If UCase((sTempString & Mid$(lstSearch.Items.Item(iLoop), _
                  Len(sTempString) + 1))) = UCase(lstSearch.Items.Item(iLoop)) Then
                    lstSearch.SelectedIndex = iLoop
                    txt.Text = lstSearch.Items.Item(iLoop)
                    txt.SelectionStart = Len(sTempString)
                    txt.SelectionLength = Len(txt.Text) - (Len(sTempString))
                    sComboText = sComboText & Mid$(sTempString, Len(sComboText) + 1)
                    Exit For
                Else
                    If InStr(UCase(sTempString), UCase(sComboText)) Then
                        sComboText = sComboText & Mid$(sTempString, Len(sComboText) _
                        + 1)
                        txt.Text = sComboText
                        txt.SelectionStart = Len(txt.Text)
                    Else
                        sComboText = sTempString
                    End If
                End If
            Next iLoop
        End If
    End Sub

    Private Sub txtGrid_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtGrid.KeyUp
        TextScript(txtGrid, e)
    End Sub

    Private Sub lstSearch_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstSearch.GotFocus
        If lstSearch.Items.Count > 0 Then
            lstSearch.SelectedIndex = 0
        End If
    End Sub

    Private Sub lstSearch_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles lstSearch.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            searchSender.Text = lstSearch.Text
            'gridView.CurrentCell = gridView.Rows(gridView.CurrentRow.Index).Cells(gridView.CurrentCell.ColumnIndex)
            If Not searchSender Is Nothing Then
                searchSender.Select()
            End If
            lstSearch.Visible = False
        End If
    End Sub

    Private Sub lstSearch_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstSearch.VisibleChanged
        If lstSearch.Visible = False Then
            searchSender = Nothing
        End If
    End Sub

    Private Sub cmbCostCenter_MAN_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCostCenter_MAN.SelectedIndexChanged
        loadGrid()
    End Sub
End Class