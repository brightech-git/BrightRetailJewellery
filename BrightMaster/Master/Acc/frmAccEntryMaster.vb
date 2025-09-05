Imports System.Data.OleDb
Public Class frmAccEntryMaster
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter

    Private Sub frmAccEntryMaster_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If tabMain.SelectedTab.Name = tabView.Name Then
                btnBack_Click(Me, New EventArgs)
            End If
        End If
    End Sub
    Private Sub frmAccEntryMaster_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If gridView.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmAccEntryMaster_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        cmbType.Items.Add("RECEIPT")
        cmbType.Items.Add("PAYMENT")
        cmbType.Items.Add("JOURNAL")
        cmbType.Items.Add("DEBIT NOTE")
        cmbType.Items.Add("CREDIT NOTE")
        cmbType.SelectedIndex = 0
        'cmbType.Text = "RECEIPT"

        cmbActive.Items.Add("YES")
        cmbActive.Items.Add("NO")
        cmbActive.Text = "YES"
        txtName_MAN.CharacterCasing = CharacterCasing.Normal
        txtDisplayText.CharacterCasing = CharacterCasing.Normal
        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGeneral.Left, Me.tabGeneral.Top, Me.tabGeneral.Width, Me.tabGeneral.Height))
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        objGPack.TextClear(Me)
        EnableAutoGenerator(True)
        Dim payMode As Integer = Val(objGPack.GetSqlValue("SELECT ISNULL(COUNT(*),0) FROM " & cnAdminDb & "..ACCENTRYMASTER")) + 1
        txtPaymode_MAN.Text = Format(payMode, "00")
        cmbType.SelectedIndex = 0
        cmbType_SelectedIndexChanged(Me, New EventArgs)
        cmbActive.Text = "YES"
        chkApprovalCheck.Checked = False
        txtName_MAN.Focus()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click, SaveToolStripMenuItem.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
        If objGPack.Validator_Check(Me) Then Exit Sub
        If objGPack.GetSqlValue("SELECT 'CHECK' FROM " & cnAdminDb & "..ACCENTRYMASTER WHERE SNO <> '" & txtSno.Text & "' AND CAPTION = '" & txtName_MAN.Text & "'").Length > 0 Then
            MsgBox("Name Already Exist")
            txtName_MAN.Focus()
            Exit Sub
        End If
        If objGPack.GetSqlValue("SELECT 'CHECK' FROM " & cnAdminDb & "..ACCENTRYMASTER WHERE SNO <> '" & txtSno.Text & "' AND PAYMODE = '" & txtPaymode_MAN.Text & "'").Length > 0 Then
            MsgBox("Paymode Already Exist")
            txtPaymode_MAN.Focus()
            Exit Sub
        End If
        Dim sno As String = Nothing
        If txtSno.Text = "" Then
            sno = cnCostId + (Val(objGPack.GetSqlValue("SELECT COUNT(*) FROM " & cnAdminDb & "..ACCENTRYMASTER")) + 1).ToString
        End If
        Try
            tran = Nothing
            tran = cn.BeginTransaction
            If txtSno.Text = "" Then
                strSql = "  INSERT INTO " & cnAdminDb & "..ACCENTRYMASTER"
                strSql += "  (SNO,CAPTION,TYPE,PAYMODE,ACTIVE,DISPLAYORDER,DISPLAYTEXT "
                strSql += " ,DRCAPTION,CRCAPTION,APPROVAL"
                strSql += " )"
                strSql += "  VALUES"
                strSql += "  ("
                strSql += " '" & sno & "'" 'SNO
                strSql += " ,'" & txtName_MAN.Text & "'" 'CAPTION
                strSql += " ,'" & Mid(cmbType.Text, 1, 1) & "'" 'TYPE
                strSql += " ,'" & txtPaymode_MAN.Text & "'" 'PAYMODE
                strSql += " ,'" & Mid(cmbActive.Text, 1, 1) & "'" 'ACTIVE
                strSql += " ," & Val(txtDisplayOrder.Text) & "" 'DISPLAYORDER
                strSql += " ,'" & txtDisplayText.Text & "'" 'DISPLAYTEXT
                strSql += " ,'" & cmbDrCaption.Text & "'" 'DrCaption
                strSql += " ,'" & cmbCrCaption.Text & "'" 'CrCaption
                strSql += " ,'" & IIf(chkApprovalCheck.Checked, "Y", "N") & "'"
                strSql += ")"
                ExecQuery(SyncMode.Master, strSql, cn, tran)
                InsertIntoBillControl("GEN-" + txtPaymode_MAN.Text, txtName_MAN.Text + " ACCOUNTS ENTRY", "N", "N", "", "A", tran)
            Else
                strSql = " UPDATE " & cnAdminDb & "..ACCENTRYMASTER"
                strSql += " SET CAPTION = '" & txtName_MAN.Text & "'"
                strSql += " ,TYPE = '" & Mid(cmbType.Text, 1, 1) & "'"
                strSql += " ,PAYMODE = '" & txtPaymode_MAN.Text & "'"
                strSql += " ,ACTIVE = '" & Mid(cmbActive.Text, 1, 1) & "'"
                strSql += " ,DISPLAYORDER = " & Val(txtDisplayOrder.Text) & ""
                strSql += " ,DISPLAYTEXT = '" & txtDisplayText.Text & "'"
                strSql += " ,DRCAPTION = '" & cmbDrCaption.Text & "'"
                strSql += " ,CRCAPTION = '" & cmbCrCaption.Text & "'"
                strSql += " ,APPROVAL = '" & IIf(chkApprovalCheck.Checked, "Y", "N") & "'"
                strSql += " WHERE SNO = '" & txtSno.Text & "'"
                ExecQuery(SyncMode.Master, strSql, cn, tran)
            End If
            tran.Commit()
            tran = Nothing
            Dim str As String = Nothing
            If txtSno.Text = "" Then str = "Saved Successfully.." Else str = "Updated Successfully.."
            str += vbCrLf + "You must restart your application for some of the change"
            str += vbCrLf + "made by menu creation to take effect."
            str += vbCrLf + "Do you want to restart your application now?"
            If MessageBox.Show(str, "Restart Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                Application.Restart()
            End If
            btnNew_Click(Me, New EventArgs)
        Catch ex As Exception
            If tran IsNot Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub

    Private Sub CallGrid()
        strSql = " SELECT CAPTION,DISPLAYTEXT"
        strSql += " ,CASE TYPE WHEN 'R' THEN 'RECEIPT'"
        strSql += "            WHEN 'P' THEN 'PAYMENT' "
        strSql += "            WHEN 'J' THEN 'JOURNAL'"
        strSql += "            WHEN 'D' THEN 'DEBIT NOTE'"
        strSql += "            WHEN 'C' THEN 'CREDIT NOTE'"
        strSql += "            END AS TYPE"
        strSql += " ,PAYMODE,DRCAPTION,CRCAPTION,CASE WHEN ACTIVE = 'Y' THEN 'YES' ELSE 'NO' END AS ACTIVE"
        strSql += " ,CASE WHEN APPROVAL = 'Y' THEN 'YES' ELSE 'NO' END AS APPROVAL,AUTOGENERATOR,SNO,DISPLAYORDER "
        strSql += " FROM " & cnAdminDb & "..ACCENTRYMASTER"
        Dim dtGrid As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        gridView.DataSource = dtGrid
        gridView.Columns("AUTOGENERATOR").Visible = False
        gridView.Columns("SNO").Visible = False
        gridView.Columns("DISPLAYORDER").HeaderText = "ORDER"
        gridView.Columns("DRCAPTION").HeaderText = "DR CAPTION"
        gridView.Columns("CRCAPTION").HeaderText = "CR CAPTION"
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
    End Sub

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click, OpenToolStripMenuItem.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        tabView.Show()
        CallGrid()
        If Not gridView.RowCount > 0 Then
            MsgBox("Record not Found", MsgBoxStyle.Information)
            Exit Sub
        End If
        tabMain.SelectedTab = tabView
        gridView.Focus()
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGeneral
        txtName_MAN.Select()
    End Sub

    Private Sub txtName_MAN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtName_MAN.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If objGPack.GetSqlValue("SELECT 'CHECK' FROM " & cnAdminDb & "..ACCENTRYMASTER WHERE SNO <> '" & txtSno.Text & "' AND CAPTION = '" & txtName_MAN.Text & "'").Length > 0 Then
                MsgBox("Name Already Exist")
                txtName_MAN.Focus()
                Exit Sub
            End If
        End If
    End Sub

    Private Sub txtPaymode_MAN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPaymode_MAN.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If objGPack.GetSqlValue("SELECT 'CHECK' FROM " & cnAdminDb & "..ACCENTRYMASTER WHERE SNO <> '" & txtSno.Text & "' AND PAYMODE = '" & txtPaymode_MAN.Text & "'").Length > 0 Then
                MsgBox("Paymode Already Exist")
                txtPaymode_MAN.Focus()
                Exit Sub
            End If
        End If
    End Sub

    Private Sub gridView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.GotFocus
        lblStatus.Visible = True
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
        End If
    End Sub

    Private Sub EnableAutoGenerator(ByVal status As Boolean)
        cmbType.Enabled = status
        cmbActive.Enabled = status
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            e.Handled = True
            If Not gridView.RowCount > 0 Then Exit Sub
            gridView.CurrentCell = gridView.CurrentRow.Cells("CAPTION")
            With gridView.CurrentRow
                If .Cells("AUTOGENERATOR").Value.ToString = "A" Then
                    EnableAutoGenerator(False)
                Else
                    EnableAutoGenerator(True)
                End If
                txtSno.Text = .Cells("SNO").Value.ToString
                txtName_MAN.Text = .Cells("CAPTION").Value.ToString
                txtPaymode_MAN.Text = .Cells("PAYMODE").Value.ToString
                cmbType.Text = .Cells("TYPE").Value.ToString
                cmbActive.Text = .Cells("ACTIVE").Value.ToString
                txtDisplayOrder.Text = .Cells("DISPLAYORDER").Value.ToString
                txtDisplayText.Text = .Cells("DISPLAYTEXT").Value.ToString
                If .Cells("DRCAPTION").Value.ToString <> "" Then cmbDrCaption.Text = .Cells("DRCAPTION").Value.ToString
                If .Cells("CRCAPTION").Value.ToString <> "" Then cmbCrCaption.Text = .Cells("CRCAPTION").Value.ToString
                chkApprovalCheck.Checked = IIf(.Cells("APPROVAL").Value.ToString = "YES", True, False)
                btnBack_Click(Me, New EventArgs)
            End With
        End If
    End Sub

    Private Sub gridView_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.LostFocus
        lblStatus.Visible = False
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
        If Not gridView.RowCount > 0 Then
            Exit Sub
        End If
        If gridView.CurrentRow.Cells("AUTOGENERATOR").Value.ToString = "A" Then
            MsgBox("Auto generator item cannot delete", MsgBoxStyle.Information)
            Exit Sub
        End If
        Dim chkQry As String = Nothing
        strSql = " SELECT DBNAME FROM " & cnAdminDb & "..DBMASTER"
        Dim dtDb As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtDb)
        If dtDb.Rows.Count > 0 Then
            For cnt As Integer = 0 To dtDb.Rows.Count - 1
                With dtDb.Rows(cnt)
                    chkQry = " SELECT 'CHECK' FROM " & .Item("DBNAME").ToString & "..BILLCONTROL WHERE CTLID = 'GEN-" & gridView.CurrentRow.Cells("PAYMODE").Value.ToString & "' AND ISNULL(CONVERT(INT,CTLTEXT),0) > 0 "
                    If cnt <> dtDb.Rows.Count - 1 Then
                        chkQry += " UNION "
                    End If
                End With
            Next
            Dim delQry As String = Nothing
            delQry = " DELETE FROM " & cnAdminDb & "..ACCENTRYMASTER WHERE SNO = '" & gridView.CurrentRow.Cells("SNO").Value.ToString & "'"
            delQry += " DELETE FROM " & cnStockDb & "..TBILLCONTROL WHERE CTLID = 'GEN-" & gridView.CurrentRow.Cells("PAYMODE").Value.ToString & "'"
            delQry += " DELETE FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = 'GEN-" & gridView.CurrentRow.Cells("PAYMODE").Value.ToString & "'"
            If DeleteItem(SyncMode.Master, chkQry, delQry) Then
                CallGrid()
                Application.Restart()
            End If
        End If
    End Sub

    Private Sub cmbType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbType.SelectedIndexChanged
        cmbDrCaption.Items.Clear()
        cmbCrCaption.Items.Clear()
        cmbDrCaption.Text = ""
        cmbCrCaption.Text = ""
        Select Case cmbType.Text.ToUpper
            Case "RECEIPT"
                cmbDrCaption.Items.Add("Received To")
                cmbDrCaption.Items.Add("Dr")
                cmbDrCaption.Text = "Received To"
                cmbCrCaption.Items.Add("Received From")
                cmbCrCaption.Items.Add("Cr")
                cmbCrCaption.Text = "Received From"
            Case "PAYMENT"
                cmbDrCaption.Items.Add("Paid To")
                cmbDrCaption.Items.Add("Dr")
                cmbDrCaption.Text = "Paid To"
                cmbCrCaption.Items.Add("Paid From")
                cmbCrCaption.Items.Add("Cr")
                cmbCrCaption.Text = "Paid From"
            Case "JOURNAL", "DEBIT NOTE", "CREDIT NOTE"
                cmbDrCaption.Items.Add("Dr")
                cmbDrCaption.Text = "Dr"
                cmbCrCaption.Items.Add("Cr")
                cmbCrCaption.Text = "Cr"
        End Select
    End Sub
End Class