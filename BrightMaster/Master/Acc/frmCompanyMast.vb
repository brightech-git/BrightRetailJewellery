Imports System.Data.OleDb
Public Class frmCompanyMast
    Dim dt As New DataTable
    Dim da As New OleDbDataAdapter
    Dim cmd As New OleDbCommand
    Dim StrSql As String = Nothing
    Dim updCompId As String = Nothing 'for update
    Dim dtCostCentre As DataTable
    Dim StrCostid As String
    Dim CENTR_DB_BR As Boolean = IIf(GetAdmindbSoftValue("CENTR_DB_ALLCOSTID", "N") = "Y", True, False)
    Dim GSTNOVALID As String = GetAdmindbSoftValue("GSTNOVALID", "W")
    Dim GSTNOFORMAT As String = GetAdmindbSoftValue("GSTNOFORMAT", "")

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        btnNew_Click(Me, New EventArgs)
    End Sub

        Private Sub frmCompanyMast_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{tab}")
        ElseIf e.KeyChar = Chr(Keys.Escape) Then
            btnBack_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub frmCompanyMast_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.tabMain.ItemSize = New Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGen.Left, Me.tabGen.Top, Me.tabGen.Width, Me.tabGen.Height))
        Me.tabMain.SelectedTab = tabGen
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        tabMain.SelectedTab = tabGen
        objGPack.TextClear(pnlInputControls)
        cmbShortKey.Text = ""
        txtCompanyId.Enabled = True
        updCompId = Nothing
        txtDisplayOrder.Text = Val(objGPack.GetSqlValue("SELECT MAX(DISPLAYORDER)  FROM  " & cnAdminDb & "..COMPANY").ToString) + 1
        cmbActive.Text = "Yes"
        txtCompanyId.Select()
        LoadCostName()
        StrSql = " SELECT STATENAME FROM " & cnAdminDb & "..STATEMAST ORDER BY STATENAME"
        objGPack.FillCombo(StrSql, CmbState, , False)
        CmbState.Text = ""
        'If CENTR_DB_BR = False Then chkcmbcostcentre.Enabled = False
    End Sub
    Private Sub LoadCostName()
        StrSql = " SELECT 'ALL'COSTNAME, 'ALL' COSTID,1 RESULT UNION ALL "
        StrSql += vbCrLf + " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        StrSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkcmbcostcentre, dtCostCentre, "COSTNAME", , "ALL")
    End Sub

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        funcSave()
    End Sub

    Function funcSave() As Integer
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Function
        If objGPack.Validator_Check(Me) Then Exit Function

        If Trim(txtCompanyId.Text) = "" Then
            MsgBox("Company Id Should not be empty.", MsgBoxStyle.Information)
            txtCompanyId.Focus()
            Exit Function
        End If
        If Trim(CmbState.Text) = "" Then
            MsgBox("State Should not be empty.", MsgBoxStyle.Information)
            CmbState.Focus()
            Exit Function
        End If
        If Trim(txtCompanyName.Text) = "" Then
            MsgBox("Company Name Should not be empty.", MsgBoxStyle.Information)
            txtCompanyName.Focus()
            Exit Function
        End If
        StrSql = "SELECT STATEID FROM " & cnAdminDb & "..STATEMAST WHERE STATENAME='" & CmbState.Text & "'"
        Dim StateId As Integer = Val(objGPack.GetSqlValue(StrSql, "STATEID", 24).ToString)
        If StateId = 0 Then
            MsgBox("State not found in State Master.", MsgBoxStyle.Information)
            CmbState.Focus()
            Exit Function
        End If
        If txtGstNo.Text = "" Then
            If GSTNOVALID = "R" Then
                MsgBox("GSTIN should not Empty...", MsgBoxStyle.Information)
                txtGstNo.Focus()
                Exit Function
            ElseIf GSTNOVALID = "W" Then
                If MessageBox.Show("GSTIN Should not empty" + vbCrLf + "Do you wish to Continue?", "GSTIN Info Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
                    txtGstNo.Focus()
                    Exit Function
                End If
            End If
        End If

        If updCompId = Nothing Then
            If objGPack.DupChecker(txtCompanyId, "SELECT 1 FROM " & cnAdminDb & "..COMPANY WHERE COMPANYID = '" & txtCompanyId.Text & "'") Then Exit Function
            If objGPack.DupChecker(txtCompanyName, "SELECT 1 FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & txtCompanyName.Text & "'") Then Exit Function
            If Trim(txtDisplayOrder.Text) <> "" Then
                If objGPack.DupChecker(txtDisplayOrder, "SELECT 1 FROM " & cnAdminDb & "..COMPANY WHERE DISPLAYORDER = " & txtDisplayOrder.Text) Then Exit Function
            End If
            If cmbShortKey.Text <> "" Then
                If (objGPack.GetSqlValue("SELECT 'EXISTS' FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(SHORTKEY,'') = '" & cmbShortKey.Text & "'", , "-1")) <> "-1" Then
                    MsgBox("Shortkey already assigned", MsgBoxStyle.Information)
                    cmbShortKey.Focus()
                    Exit Function
                End If
            End If
            funcAdd(StateId)
        Else
            If cmbShortKey.Text <> "" Then
                If (objGPack.GetSqlValue("SELECT 'EXISTS' FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(SHORTKEY,'') = '" & cmbShortKey.Text & "' AND ISNULL(SHORTKEY,'') <> '' AND COMPANYID <> '" & updCompId & "'", , "-1")) <> "-1" Then
                    MsgBox("Shortkey already assigned", MsgBoxStyle.Information)
                    cmbShortKey.Focus()
                    Exit Function
                End If
            End If
            funcUpdate(StateId)
        End If
    End Function

    Function funcAdd(ByVal StateId As Integer) As Integer
        StrCostid = Nothing
        StrCostid = GetSelectedCostId_NEW(chkcmbcostcentre, False)
        Try
            tran = cn.BeginTransaction()
            Dim strField As String = Nothing
            Dim strValue As String = Nothing
            strField = "COMPANYID"
            strValue = "'" & Trim(txtCompanyId.Text) & "'"
            strField += ",COMPANYNAME"
            strValue += ",'" & Trim(txtCompanyName.Text) & "'"
            If chkcmbcostcentre.Text <> "ALL" Then
                strField += ",COSTID"
                strValue += "," & StrCostid & ""
            Else
                strField += ",COSTID"
                strValue += ",NULL"
            End If
            strField += ",ADDRESS1"
            strValue += ",'" & Trim(txtAddress1.Text) & "'"
            strField += ",ADDRESS2"
            strValue += ",'" & Trim(txtAddress2.Text) & "'"
            strField += ",ADDRESS3"
            strValue += ",'" & Trim(txtAddress3.Text) & "'"
            strField += ",ADDRESS4"
            strValue += ",'" & Trim(txtAddress4.Text) & "'"
            strField += ",AREACODE"
            strValue += ",'" & Trim(txtAreaCode.Text) & "'"
            strField += ",PHONE"
            strValue += ",'" & Trim(txtPhone.Text) & "'"
            strField += ",EMAIL"
            strValue += ",'" & Trim(txtEmail.Text) & "'"
            strField += ",LOCALTAXNO"
            strValue += ",'" & Trim(txtLocalTaxNo.Text) & "'"
            strField += ",CSTNO"
            strValue += ",'" & Trim(txtCstNo.Text) & "'"
            strField += ",TINNO"
            strValue += ",'" & Trim(txtTinNo.Text) & "'"
            strField += ",PANNO"
            strValue += ",'" & Trim(txtPanNo.Text) & "'"
            strField += ",TDSNO"
            strValue += ",'" & Trim(txtTdsNo.Text) & "'"
            strField += ",DISPLAYORDER"
            strValue += "," & IIf(Trim(txtDisplayOrder.Text) = "", 0, Val(Trim(txtDisplayOrder.Text)))
            strField += ",USERID"
            strValue += "," & userId
            strField += ",UPDATED"
            strValue += ",'" & Today.Date.ToString("yyyy-MM-dd") & "'"
            strField += ",UPTIME"
            strValue += ",'" & Date.Now.ToLongTimeString & "'"
            strField += ",ACTIVE"
            strValue += ",'" & Mid(cmbActive.Text, 1, 1) & "'"
            If cmbShortKey.Text <> "" Then
                strField += ",SHORTKEY"
                strValue += ",'" & cmbShortKey.Text & "'"
            End If
            strField += ",TANNO"
            strValue += ",'" & Trim(txtTanno.Text) & "'"
            strField += ",STATEID"
            strValue += ",'" & StateId & "'"

            StrSql = " Insert into " & cnAdminDb & "..COMPANY"
            StrSql += " (" & strField
            StrSql += " ) Values (" & strValue & ")"
            ExecQuery(SyncMode.Master, StrSql, cn, tran)

            StrSql = " INSERT INTO " & cnStockDb & "..BILLCONTROL(CTLID,CTLNAME,CTLTYPE,CTLTEXT,CTLMODE,CTLMODULE,UPDATED,UPTIME,COMPANYID)"
            StrSql += " SELECT CTLID,CTLNAME,CTLTYPE,CTLTEXT,CTLMODE,CTLMODULE"
            StrSql += " ,CONVERT(SMALLDATETIME,CONVERT(VARCHAR(10),GETDATE(),101))AS UPDATED"
            StrSql += " ,CONVERT(SMALLDATETIME,CONVERT(VARCHAR(10),GETDATE(),108))AS UPTIME"
            StrSql += " ,'" & Trim(txtCompanyId.Text) & "' COMPANYID FROM " & cnStockDb & "..TBILLCONTROL T"
            ExecQuery(SyncMode.Master, StrSql, cn, tran)

            tran.Commit()
            MsgBox(Trim(txtCompanyName.Text) & " Company Created Successfully", MsgBoxStyle.Information)
            btnNew_Click(Me, New EventArgs)
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function

    Function funcUpdate(ByVal StateId As Integer) As Integer
        Try
            StrCostid = Nothing
            StrCostid = GetSelectedCostId_NEW(chkcmbcostcentre, False)
            tran = cn.BeginTransaction()
            StrSql = " UPDATE " & cnAdminDb & "..COMPANY SET "
            StrSql += " COMPANYID = '" & txtCompanyId.Text & "'"
            StrSql += " ,COMPANYNAME = '" & Trim(txtCompanyName.Text) & "'"
            If chkcmbcostcentre.Text <> "ALL" Then
                StrSql += ",COSTID = " & StrCostid & ""
            Else
                StrSql += ",COSTID = NULL"
            End If
            StrSql += " ,ADDRESS1 = '" & Trim(txtAddress1.Text) & "'"
            StrSql += " ,ADDRESS2 = '" & Trim(txtAddress2.Text) & "'"
            StrSql += " ,ADDRESS3 = '" & Trim(txtAddress3.Text) & "'"
            StrSql += " ,ADDRESS4 = '" & Trim(txtAddress4.Text) & "'"
            StrSql += " ,AREACODE = '" & Trim(txtAreaCode.Text) & "'"
            StrSql += " ,PHONE = '" & Trim(txtPhone.Text) & "'"
            StrSql += " ,EMAIL = '" & Trim(txtEmail.Text) & "'"
            StrSql += " ,LOCALTAXNO = '" & Trim(txtLocalTaxNo.Text) & "'"
            StrSql += " ,CSTNO = '" & Trim(txtCstNo.Text) & "'"
            StrSql += " ,TINNO = '" & Trim(txtTinNo.Text) & "'"
            StrSql += " ,PANNO = '" & Trim(txtPanNo.Text) & "'"
            StrSql += " ,TDSNO = '" & Trim(txtTdsNo.Text) & "'"
            StrSql += " ,DISPLAYORDER = '" & Trim(txtDisplayOrder.Text) & "'"
            StrSql += " ,ACTIVE = '" & Mid(cmbActive.Text, 1, 1) & "'"
            If cmbShortKey.Text <> "" Then
                StrSql += " ,SHORTKEY = '" & cmbShortKey.Text & "'"
            End If
            StrSql += " ,TANNO = '" & Trim(txtTanno.Text) & "'"
            StrSql += " ,STATEID = '" & StateId & "'"
            StrSql += " ,GSTNO = '" & Trim(txtGstNo.Text) & "'"
            StrSql += " WHERE COMPANYID = '" & updCompId & "'"
            ExecQuery(SyncMode.Master, StrSql, cn, tran)

            StrSql = " UPDATE " & cnStockDb & "..BILLCONTROL SET"
            StrSql += " COMPANYID = '" & txtCompanyId.Text & "'"
            StrSql += " WHERE COMPANYID = '" & updCompId & "'"
            ExecQuery(SyncMode.Master, StrSql, cn, tran)

            tran.Commit()
            MsgBox("Company Details Updated Successfully", MsgBoxStyle.Information)
            btnNew_Click(Me, New EventArgs)
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message & vbCrLf & ex.StackTrace)
        End Try
    End Function

    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        If tabMain.SelectedTab.Name = tabGen.Name Then btnSave_Click(Me, New EventArgs)
    End Sub

    Private Sub OpenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click
        If tabMain.SelectedTab.Name = tabGen.Name Then btnOpen_Click(Me, New EventArgs)
    End Sub

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click
        funcView()
    End Sub

    Function funcView() As Integer
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Function
        Try
            Me.btnOpen.Enabled = False
            'Me.Cursor = Cursors.WaitCursor
            StrSql = " SELECT COMPANYID,COMPANYNAME,COSTID,ADDRESS1,ADDRESS2,ADDRESS3,ADDRESS4,"
            StrSql += " AREACODE,PHONE,EMAIL,LOCALTAXNO,CSTNO,TINNO,PANNO,TDSNO,TANNO,DISPLAYORDER,CASE WHEN ISNULL(ACTIVE,'Y') = 'N' THEN 'NO' ELSE 'YES' END AS ACTIVE"
            StrSql += " ,(SELECT STATENAME FROM " & cnAdminDb & "..STATEMAST WHERE STATEID=C.STATEID)STATENAME"
            StrSql += " ,GSTNO"
            StrSql += " FROM " & cnAdminDb & "..COMPANY C ORDER BY DISPLAYORDER"
            dt = New DataTable
            da = New OleDbDataAdapter(StrSql, cn)
            da.Fill(dt)
            If dt.Rows.Count <= 0 Then
                MsgBox("No Records to View", MsgBoxStyle.Information)
                Exit Function
            End If
            gridView.DataSource = dt
            funcGridStyle()
            If gridView.Rows.Count > 0 Then
                tabMain.SelectedTab = tabView
                gridView.Focus()
            End If
        Catch ex As Exception
            MsgBox("Error : " & ex.Message & " Position : " & MsgBox(ex.StackTrace), MsgBoxStyle.Critical)
        Finally
            Me.btnOpen.Enabled = True
            Me.Cursor = Cursors.Arrow
        End Try
    End Function

    Function funcGridStyle() As Integer
        With gridView
            .RowHeadersVisible = False
            .Columns("PHONE").Visible = False
            .Columns("EMAIL").Visible = False
            .Columns("AREACODE").Visible = False
            .Columns("LOCALTAXNO").Visible = False
            .Columns("CSTNO").Visible = False
            .Columns("TINNO").Visible = False
            .Columns("PANNO").Visible = False
            .Columns("TANNO").Visible = False
            .Columns("TDSNO").Visible = False
            .Columns("DISPLAYORDER").Visible = False
            .Columns("ADDRESS1").Visible = False
            .Columns("ADDRESS2").Visible = False
            .Columns("ADDRESS3").Visible = False
            .Columns("ADDRESS4").Visible = False
            .Columns("ACTIVE").Visible = False
            .Columns("COSTID").Visible = True
            .Columns("STATENAME").Visible = True
            .Columns("GSTNO").Visible = True
            With .Columns("COMPANYID")
                .HeaderText = "COMPANYID"
                .Width = 150
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("COMPANYNAME")
                .HeaderText = "NAME"
                .Width = 350
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        End With
    End Function

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        If tabMain.SelectedTab.Name = tabGen.Name Then btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ExToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExToolStripMenuItem.Click
        If tabMain.SelectedTab.Name = tabGen.Name Then btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub txtDisplayOrder_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtDisplayOrder.KeyPress
        If (Not Char.IsDigit(e.KeyChar)) And AscW(e.KeyChar) <> 13 And AscW(e.KeyChar) <> 8 Then
            e.Handled = True
            MsgBox("Digits Only Allowed 0 to 9", MsgBoxStyle.Information)
            txtDisplayOrder.Focus()
        End If
    End Sub

    Private Sub txtPhone_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPhone.KeyPress
        If (Not Char.IsDigit(e.KeyChar)) And e.KeyChar <> Chr(Keys.Back) And e.KeyChar <> Chr(Keys.Enter) Then
            e.Handled = True
            MsgBox("Digits Only Allowed 0 to 9", MsgBoxStyle.Information)
            txtPhone.Focus()
        End If
    End Sub

    Private Sub txtAreaCode_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtAreaCode.KeyPress
        'If (Not Char.IsDigit(e.KeyChar)) And AscW(e.KeyChar) <> 13 And AscW(e.KeyChar) <> 8 Then
        '    e.Handled = True
        '    MsgBox("Digits Only Allowed 0 to 9", MsgBoxStyle.Information)
        '    txtAreaCode.Focus()
        'End If
    End Sub

    Private Sub gridView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.GotFocus
        If gridView.Rows.Count > 0 Then
            lblStatus.Visible = True
            btnDelete.Enabled = True
        End If
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            If gridView.RowCount > 0 Then
                funcGetDetails(gridView.Item(0, gridView.CurrentRow.Index).Value.ToString)
            End If
        End If
    End Sub
    Function funcGetDetails(ByVal tempCompID As String)
        Dim dt As New DataTable
        dt.Clear()
        For i As Integer = 0 To chkcmbcostcentre.Items.Count - 1
            chkcmbcostcentre.SetItemChecked(i, CheckState.Unchecked)
        Next
        Try
            StrSql = " select * from " & cnAdminDb & "..company where companyid = '" & tempCompID & "'"
            da = New OleDbDataAdapter(StrSql, cn)
            da.Fill(dt)
            If Not dt.Rows.Count > 0 Then
                Return 0
            End If
            With dt.Rows(0)
                txtCompanyId.Text = .Item("COMPANYID").ToString
                txtCompanyName.Text = .Item("COMPANYNAME").ToString

                If .Item("COSTID").ToString <> "" Then
                    Dim dtusercompanyid As New DataTable
                    StrSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID IN ('" & Replace(.Item("COSTID").ToString, ",", "','") & "')"
                    da = New OleDbDataAdapter(StrSql, cn)
                    da.Fill(dtusercompanyid)
                    If dtusercompanyid.Rows.Count > 0 Then
                        For iij As Integer = 0 To dtusercompanyid.Rows.Count - 1
                            If chkcmbcostcentre.Items.Contains(dtusercompanyid.Rows(iij).Item("COSTNAME").ToString) Then
                                Dim INDX As Integer = chkcmbcostcentre.Items.IndexOf(dtusercompanyid.Rows(iij).Item("COSTNAME").ToString)
                                chkcmbcostcentre.SetItemCheckState(INDX, CheckState.Checked)
                            End If
                        Next
                    End If
                Else
                    chkcmbcostcentre.SetItemCheckState(0, CheckState.Checked)
                End If

                txtAddress1.Text = .Item("ADDRESS1").ToString
                txtAddress2.Text = .Item("ADDRESS2").ToString
                txtAddress3.Text = .Item("ADDRESS3").ToString
                txtAddress4.Text = .Item("ADDRESS4").ToString
                txtPhone.Text = .Item("PHONE").ToString
                txtEmail.Text = .Item("EMAIL").ToString
                txtAreaCode.Text = .Item("AREACODE").ToString
                txtLocalTaxNo.Text = .Item("LOCALTAXNO").ToString
                txtCstNo.Text = .Item("CSTNO").ToString
                txtTinNo.Text = .Item("TINNO").ToString
                txtPanNo.Text = .Item("PANNO").ToString
                txtTdsNo.Text = .Item("TDSNO").ToString
                txtDisplayOrder.Text = .Item("DISPLAYORDER").ToString
                cmbActive.Text = IIf(.Item("ACTIVE").ToString = "N", "NO", "YES")
                If .Item("SHORTKEY").ToString <> "" Then
                    cmbShortKey.Text = .Item("SHORTKEY").ToString
                End If
                txtTanno.Text = .Item("TANNO").ToString
                updCompId = .Item("COMPANYID").ToString
                StrSql = "SELECT STATENAME FROM " & cnAdminDb & "..STATEMAST WHERE STATEID=" & Val(.Item("STATEID").ToString)
                CmbState.Text = objGPack.GetSqlValue(StrSql, "STATENAME", "").ToString
                txtGstNo.Text = .Item("GSTNO").ToString
            End With
            tabMain.SelectedTab = tabGen
            txtCompanyId.Focus()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        Return 0
    End Function

    Private Sub gridView_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.LostFocus
        lblStatus.Visible = False
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        For i As Integer = 0 To chkcmbcostcentre.Items.Count - 1
            chkcmbcostcentre.SetItemChecked(i, CheckState.Unchecked)
        Next
        tabMain.SelectedTab = tabGen
        txtCompanyId.Focus()
    End Sub

    Private Sub gridView_RowEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.RowEnter
        objGPack.TextClear(pnlViewDet)
        With gridView.Rows(e.RowIndex)
            detAddress1.Text = .Cells("ADDRESS1").Value.ToString
            detAddress2.Text = .Cells("ADDRESS2").Value.ToString
            detAddress3.Text = .Cells("ADDRESS3").Value.ToString
            detAddress4.Text = .Cells("ADDRESS4").Value.ToString
            detPhone.Text = .Cells("PHONE").Value.ToString
            detEmail.Text = .Cells("EMAIL").Value.ToString
            detAreaCode.Text = .Cells("AREACODE").Value.ToString
            detLocalTaxNo.Text = .Cells("LOCALTAXNO").Value.ToString
            detCSTNo.Text = .Cells("CSTNO").Value.ToString
            detTINNo.Text = .Cells("TINNO").Value.ToString
            detPANNo.Text = .Cells("PANNO").Value.ToString
            detTDSNo.Text = .Cells("TDSNO").Value.ToString
            detTANNo.Text = .Cells("TANNO").Value.ToString
            detGSTNO.Text = .Cells("GSTNO").Value.ToString
            detState.Text = .Cells("STATENAME").Value.ToString
        End With
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If gridView.Rows.Count > 0 Then
            If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
            Dim chkQry As String = Nothing
            Dim strdelete As String = gridView.Rows(gridView.CurrentRow.Index).Cells("COMPANYID").Value.ToString

            If strdelete <> " THEN" Then
                StrSql = " SELECT DBNAME FROM " & cnAdminDb & "..DBMASTER"
                Dim dtDb As New DataTable
                da = New OleDbDataAdapter(StrSql, cn)
                da.Fill(dtDb)
                If dtDb.Rows.Count > 0 Then
                    For cnt As Integer = 0 To dtDb.Rows.Count - 1
                        With dtDb.Rows(cnt)
                            chkQry += " SELECT TOP 1 COMPANYID FROM " & .Item("DBNAME").ToString & "..ACCTRAN WHERE COMPANYID = '" & strdelete & "'"
                            chkQry += " UNION ALL SELECT TOP 1 COMPANYID FROM " & .Item("DBNAME").ToString & "..ISSUE WHERE COMPANYID = '" & strdelete & "'"
                            chkQry += " UNION ALL SELECT TOP 1 COMPANYID FROM " & .Item("DBNAME").ToString & "..RECEIPT WHERE COMPANYID = '" & strdelete & "'"
                            If cnt <> dtDb.Rows.Count - 1 Then
                                chkQry += " UNION "
                            End If
                        End With
                    Next
                    StrSql = " DELETE " & cnAdminDb & "..COMPANY WHERE COMPANYID = '" & strdelete & "'  AND ISNULL(AUTOGENERATOR,'') = '' "
                    DeleteItem(SyncMode.Master, chkQry, StrSql)
                    btnOpen_Click(Me, New EventArgs)
                End If
            End If
        End If
    End Sub

    Private Sub txtCompanyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCompanyName.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            objGPack.DupChecker(txtCompanyName, "SELECT 1 FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & txtCompanyName.Text & "' AND COMPANYID <> '" & updCompId & "'")
        End If
    End Sub

    Private Sub txtCompanyId_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCompanyId.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            objGPack.DupChecker(txtCompanyId, "SELECT 1 FROM " & cnAdminDb & "..COMPANY WHERE COMPANYID = '" & txtCompanyId.Text & "' AND COMPANYID <> '" & updCompId & "'")
        End If
    End Sub
#Region "USER DEFINE FUNCTION"
    Public Function GetSelectedCostId_NEW(ByVal chkLst As BrighttechPack.CheckedComboBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        retStr += "'"
        If chkLst.Items.Count > 0 Then
            For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
                retStr += objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME= '" & chkLst.CheckedItems.Item(cnt).ToString & "'")
                If cnt <> chkLst.CheckedItems.Count - 1 Then
                    retStr += ","
                End If
            Next
            retStr += "'"
        Else
            retStr = "''"
        End If
        Return retStr
    End Function
#End Region

    Private Sub txtGstNo_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtGstNo.Leave
        If GSTNOFORMAT <> "" And txtGstNo.Text <> "" Then
            If Not formatchkGST(GSTNOFORMAT, txtGstNo.Text.Trim) Then
                MsgBox("GST No format(" & GSTNOFORMAT & ")should not match", MsgBoxStyle.Information)
                txtGstNo.Focus()
                Exit Sub
            End If
        End If
    End Sub
End Class