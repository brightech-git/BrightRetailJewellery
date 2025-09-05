Imports System.Data.OleDb
Public Class VAMARGIN
    Dim StrSql As String
    Dim Cmd As OleDbCommand
    Dim Da As OleDbDataAdapter
    Dim MarginId As Integer
    Dim UpdateFlag As Boolean

    Private Sub VAMARGIN_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If tabMain.SelectedTab.Name = tabView.Name Then
                tabMain.SelectedTab = tabGeneral
            End If
        End If
    End Sub

    Private Sub VAMARGIN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtMarginName.Focused Then Exit Sub
            If txtDisplayOrder_NUM.Focused Then Exit Sub
            If gridView.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Function funcAddCostCentre() As Integer
        strSql = "select DISTINCT CostName from " & cnAdminDb & "..CostCentre order by CostName"
        cmbCostCentre.Items.Clear()
        cmbCostCentre.Items.Add("ALL")
        objGPack.FillCombo(strSql, cmbCostCentre, False, False)
        cmbCostCentre.Text = "ALL"
    End Function
    Private Sub VAMARGIN_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGeneral.Left, Me.tabGeneral.Top, Me.tabGeneral.Width, Me.tabGeneral.Height))
        grpContainer.Location = New Point((ScreenWid - grpContainer.Width) / 2, ((ScreenHit - 128) - grpContainer.Height) / 2)
        grpContainer2.Location = New Point((ScreenWid - grpContainer2.Width) / 2, ((ScreenHit - 128) - grpContainer2.Height) / 2)
        cmbMetalName.Items.Clear()
        cmbMetalName.Items.Add("ALL")
        StrSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE ACTIVE = 'Y' ORDER BY DISPLAYORDER,METALNAME "
        objGPack.FillCombo(StrSql, cmbMetalName, False)
        cmbGetPwd.Items.Clear()
        cmbGetPwd.Items.Add("YES")
        cmbGetPwd.Items.Add("NO")
        cmbDisplay.Items.Clear()
        cmbDisplay.Items.Add("YES")
        cmbDisplay.Items.Add("NO")
        funcAddCostCentre()
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        objGPack.TextClear(Me)
        cmbGetPwd.Text = "NO"
        cmbDisplay.Text = "YES"
        UpdateFlag = False
        cmbMetalName.Select()
    End Sub

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click, OpenToolStripMenuItem.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        gridView.DataSource = Nothing
        StrSql = vbCrLf + "  SELECT MARGINID,MARGINNAME"
        StrSql += vbCrLf + "  ,isnull(METALNAME,'') AS METAL"
        StrSql += vbCrLf + "  ,isnull(COSTNAME,'') AS COSTNAME"
        StrSql += vbCrLf + "  ,WASTPER,WASTAGE,MCGRM,MCHARGE,FIXEDITEM,DIARATE,STNRATE,PRERATE,DIARATEPER,STNRATEPER,PRERATEPER"
        StrSql += vbCrLf + "  ,CASE WHEN GETPWD = 'Y' THEN 'YES' ELSE 'NO' END AS GETPWD"
        StrSql += vbCrLf + "  ,V.DISPLAYORDER"
        StrSql += vbCrLf + "  ,CASE WHEN DISPLAY = 'Y' THEN 'YES' ELSE 'NO' END AS DISPLAY,MCPER"
        StrSql += vbCrLf + "  FROM " & cnAdminDb & "..VAMARGIN AS V"
        StrSql += vbCrLf + "   left join " & cnAdminDb & "..METALMAST m on m.METALID = V.METALID"
        StrSql += vbCrLf + "   left join " & cnAdminDb & "..COSTCENTRE C on C.COSTID= V.COSTID"
        Dim dtGrid As New DataTable
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dtGrid)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        tabView.Show()
        gridView.DataSource = dtGrid
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        tabMain.SelectedTab = tabView
        gridView.Select()
    End Sub

    Private Sub funcAdd()
        Try
            Dim dtVaMargin As New DataTable
            dtVaMargin = BrighttechPack.GlobalMethods.GetTableStructure(cnAdminDb, "VAMARGIN", cn)
            Dim RoVaMargin As DataRow = dtVaMargin.NewRow
            tran = Nothing
            tran = cn.BeginTransaction

            RoVaMargin.Item("MARGINID") = Val(objGPack.GetSqlValue("SELECT ISNULL(MAX(MARGINID),0)+1 FROM " & cnAdminDb & "..VAMARGIN", , , tran))
            RoVaMargin.Item("MARGINNAME") = txtMarginName.Text
            RoVaMargin.Item("COSTID") = IIf(cmbCostCentre.Text <> "ALL", objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre.Text & "'", , , tran), "")
            RoVaMargin.Item("METALID") = IIf(cmbMetalName.Text <> "ALL", objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetalName.Text & "'", , , tran), "")
            RoVaMargin.Item("WASTPER") = Val(txtWastPer_PER.Text)
            RoVaMargin.Item("WASTAGE") = Val(txtWastage_WET.Text)
            RoVaMargin.Item("MCGRM") = Val(txtMcGrm_AMT.Text)
            RoVaMargin.Item("MCHARGE") = Val(txtMc_AMT.Text)
            RoVaMargin.Item("MCPER") = Val(txtMc_PER.Text)
            RoVaMargin.Item("FIXEDITEM") = Val(txtFixedValue_AMT.Text)
            RoVaMargin.Item("DIARATE") = Val(txtDiaRate_AMT.Text)
            RoVaMargin.Item("STNRATE") = Val(txtStnRate_AMT.Text)
            RoVaMargin.Item("PRERATE") = Val(txtPreRate_AMT.Text)
            RoVaMargin.Item("DIARATEPER") = Val(txtDiaRate_PER.Text)
            RoVaMargin.Item("STNRATEPER") = Val(txtStnRate_PER.Text)
            RoVaMargin.Item("PRERATEPER") = Val(txtPreRate_PER.Text)

            RoVaMargin.Item("GETPWD") = Mid(cmbGetPwd.Text, 1, 1)
            RoVaMargin.Item("DISPLAYORDER") = Val(txtDisplayOrder_NUM.Text)
            RoVaMargin.Item("DISPLAY") = Mid(cmbDisplay.Text, 1, 1)
            dtVaMargin.Rows.Add(RoVaMargin)
            InsertData(SyncMode.Master, dtVaMargin, cn, tran)
            tran.Commit()
            tran = Nothing
            MsgBox("Successfully Added", MsgBoxStyle.Information)
            btnNew_Click(Me, New EventArgs)
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub funcUpdate()
        Try
            tran = Nothing
            tran = cn.BeginTransaction
            StrSql = " UPDATE " & cnAdminDb & "..VAMARGIN SET"
            StrSql += vbCrLf + " MARGINNAME = '" & txtMarginName.Text & "'"
            If cmbMetalName.Text <> "ALL" Then
                StrSql += vbCrLf + " ,METALID = (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetalName.Text & "')"
            Else
                StrSql += vbCrLf + " ,METALID ='' "
            End If
            If cmbCostCentre.Text <> "ALL" Then
                StrSql += vbCrLf + " ,COSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre.Text & "')"
            Else
                StrSql += vbCrLf + " ,COSTID ='' "
            End If
            StrSql += vbCrLf + " ,WASTPER = " & Val(txtWastPer_PER.Text)
            StrSql += vbCrLf + " ,WASTAGE = " & Val(txtWastage_WET.Text)
            StrSql += vbCrLf + " ,MCGRM = " & Val(txtMcGrm_AMT.Text)
            StrSql += vbCrLf + " ,MCHARGE = " & Val(txtMc_AMT.Text)
            StrSql += vbCrLf + " ,MCPER = " & Val(txtMc_PER.Text)
            StrSql += vbCrLf + " ,FIXEDITEM = " & Val(txtFixedValue_AMT.Text)
            StrSql += vbCrLf + " ,DIARATE = " & Val(txtDiaRate_AMT.Text)
            StrSql += vbCrLf + " ,STNRATE = " & Val(txtStnRate_AMT.Text)
            StrSql += vbCrLf + " ,PRERATE = " & Val(txtPreRate_AMT.Text)
            StrSql += vbCrLf + " ,DIARATEPER = " & Val(txtDiaRate_PER.Text)
            StrSql += vbCrLf + " ,STNRATEPER = " & Val(txtStnRate_PER.Text)
            StrSql += vbCrLf + " ,PRERATEPER = " & Val(txtPreRate_PER.Text)
            StrSql += vbCrLf + " ,GETPWD = '" & Mid(cmbGetPwd.Text, 1, 1) & "'"
            StrSql += vbCrLf + " ,DISPLAYORDER = " & Val(txtDisplayOrder_NUM.Text)
            StrSql += vbCrLf + " ,DISPLAY = '" & Mid(cmbDisplay.Text, 1, 1) & "'"
            StrSql += vbCrLf + " WHERE MARGINID = " & MarginId & ""
            ExecQuery(SyncMode.Master, StrSql, cn, tran)
            tran.Commit()
            tran = Nothing
            MsgBox("Successfully Updated", MsgBoxStyle.Information)
            btnNew_Click(Me, New EventArgs)
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click, SaveToolStripMenuItem.Click
        If UpdateFlag Then
            If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Edit) Then Exit Sub
        Else
            If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
        End If
        If Not ValidMargin() Then Exit Sub
        If Not ValidDisplayOrder() Then Exit Sub
        If UpdateFlag Then
            funcUpdate()
        Else
            funcAdd()
        End If
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGeneral
        cmbMetalName.Select()
    End Sub

    Private Function ValidDisplayOrder() As Boolean
        If Val(txtDisplayOrder_NUM.Text) > 0 Then
            StrSql = " SELECT 'EXISTS' FROM " & cnAdminDb & "..VAMARGIN WHERE"
            StrSql += " DISPLAYORDER = " & Val(txtDisplayOrder_NUM.Text) & ""
            StrSql += " AND MARGINID <> " & MarginId & ""
            If objGPack.GetSqlValue(StrSql, , "-1") <> "-1" Then
                MsgBox("Displayorder Already Exist", MsgBoxStyle.Information)
                txtDisplayOrder_NUM.Select()
                Exit Function
            End If
        End If
        Return True
    End Function

    Private Function ValidMargin() As Boolean
        If txtMarginName.Text = "" Then
            MsgBox("Margin Name should not empty", MsgBoxStyle.Information)
            txtMarginName.Select()
            Exit Function
        End If
        StrSql = " SELECT 'EXISTS' FROM " & cnAdminDb & "..VAMARGIN WHERE"
        StrSql += " MARGINNAME = '" & txtMarginName.Text & "'"
        If cmbMetalName.Text <> "ALL" Then StrSql += " AND METALID = (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetalName.Text & "')"
        If cmbCostCentre.Text <> "ALL" Then StrSql += " AND COSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre.Text & "')"
        StrSql += " AND MARGINID <> " & MarginId & ""
        If objGPack.GetSqlValue(StrSql, , "-1") <> "-1" Then
            MsgBox("MarginName Already Exist ", MsgBoxStyle.Information)
            txtMarginName.Select()
            Exit Function
        End If
        Return True
    End Function

    Private Sub txtMarginName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtMarginName.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If ValidMargin() Then
                SendKeys.Send("{TAB}")
            End If
        End If
    End Sub

    Private Sub txtDisplayOrder_NUM_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtDisplayOrder_NUM.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If ValidDisplayOrder() Then
                SendKeys.Send("{TAB}")
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
                gridView.CurrentCell = gridView.Rows(gridView.CurrentRow.Index).Cells(gridView.FirstDisplayedCell.ColumnIndex)
                With gridView.CurrentRow
                    cmbMetalName.Text = .Cells("METAL").Value.ToString
                    cmbCostCentre.Text = .Cells("COSTNAME").Value.ToString
                    txtMarginName.Text = .Cells("MARGINNAME").Value.ToString
                    txtWastPer_PER.Text = .Cells("WASTPER").Value.ToString
                    txtWastage_WET.Text = .Cells("WASTAGE").Value.ToString
                    txtMcGrm_AMT.Text = .Cells("MCGRM").Value.ToString
                    txtMc_AMT.Text = .Cells("MCHARGE").Value.ToString
                    txtMc_PER.Text = .Cells("MCPER").Value.ToString
                    txtFixedValue_AMT.Text = .Cells("FIXEDITEM").Value.ToString
                    txtDiaRate_AMT.Text = .Cells("DIARATE").Value.ToString
                    txtStnRate_AMT.Text = .Cells("STNRATE").Value.ToString
                    txtPreRate_AMT.Text = .Cells("PRERATE").Value.ToString
                    txtDiaRate_PER.Text = .Cells("DIARATEPER").Value.ToString
                    txtStnRate_PER.Text = .Cells("STNRATEPER").Value.ToString
                    txtPreRate_PER.Text = .Cells("PRERATEPER").Value.ToString
                    cmbGetPwd.Text = .Cells("GETPWD").Value.ToString
                    txtDisplayOrder_NUM.Text = .Cells("DISPLAYORDER").Value.ToString
                    cmbDisplay.Text = .Cells("DISPLAY").Value.ToString
                    UpdateFlag = True
                    MarginId = Val(.Cells("MARGINID").Value.ToString)
                    tabMain.SelectedTab = tabGeneral
                    cmbMetalName.Select()
                End With
            End If
        End If
    End Sub
End Class