Imports System.Data.OleDb
Public Class frmCostCentre
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim flagUpdate As Boolean
    Dim strSql As String
    Dim dt As New DataTable
    Dim dtCompany As DataTable
    Dim da1 As OleDbDataAdapter
    Dim SelectedCompanyId As String
    Dim CENTR_DB_BR As Boolean = IIf(GetAdmindbSoftValue("CENTR_DB_ALLCOSTID", "N") = "Y", True, False)
    Dim REQ_FRANCHISEE As Boolean = IIf(GetAdmindbSoftValue("REQ_FRANCHISEE", "N") = "Y", True, False)
    Dim GSTNOVALID As String = GetAdmindbSoftValue("GSTNOVALID", "W")

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub
    Function funcCallGrid() As Integer
        Dim dt As New DataTable
        dt.Clear()
        strSql = " Select COSTID,COSTNAME,DISPORDER,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = C.ACCODE)aS ACNAME"
        strSql += " ,CASE WHEN ISNULL(ACTIVE,'')<>'N' THEN 'YES' ELSE 'NO' END ACTIVE "
        strSql += "  from " & cnAdminDb & "..CostCentre AS C order by dispOrder,COSTNAME"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        gridView.DataSource = dt
        gridView.AutoResizeColumns()
        gridView.Columns("COSTNAME").MinimumWidth = 150
        Return 0
    End Function
    Function funcClear()
        txtCostID1_Man.Clear()
        txtCostName1_Man.Clear()
        txtDispOrd.Clear()
        txtAdd1.Clear()
        txtAdd2.Clear()
        txtAdd3.Clear()
        txtAdd4.Clear()
        txtAreaCode.Clear()
        txtPhone.Clear()
        txtEmail.Clear()
        txtTaxNo.Clear()
        txtCstNo.Clear()
        txtTinNo.Clear()
        txtPanNo.Clear()
        txtTDSNo.Clear()
        txtTanno.Clear()
        txtRatePer_PER.Clear()
        chkCompanyId.Items.Clear()
        Return 0
    End Function
    Function funcNew()
        tabMain.SelectedTab = tabGen
        txtCostID1_Man.Enabled = True
        txtDispOrd.Text = Val(objGPack.GetSqlValue("SELECT MAX(DISPLAYORDER)  FROM  " & cnAdminDb & "..COMPANY").ToString) + 1
        txtCostID1_Man.Select()
        flagUpdate = False
        funcClear()
        cmbActive.SelectedIndex = 0
        cmbAccName.Text = ""
        txtShortName.Text = ""
        cmbCrAccName.Text = ""
        cmbAdvAccName.Text = ""
        cmbAlterAcname.Text = ""
        txtCostID1_Man.Enabled = True
        txtCostID1_Man.Focus()
        funcLoadCostCentre()
        If RATE_BRANCHWISE Then
            lblRatePer.Visible = True
            txtRatePer_PER.Visible = True
            lblDratePer.Visible = True
            detRatePer.Visible = True
        End If
        If REQ_FRANCHISEE Then
            Label33.Visible = True
            CmbBranch.Visible = True
            CmbBranch.SelectedIndex = 0
        End If
        'If CENTR_DB_BR = False Then chkCompanyId.Enabled = False
        Return 0
    End Function
    Function funcLoadCostCentre()
        strSql = " SELECT 'ALL' COMPANYNAME,1 RESULT UNION ALL SELECT COMPANYNAME,2 RESULT FROM " & cnAdminDb & "..COMPANY"
        strSql += " ORDER BY RESULT,COMPANYNAME"
        dtCompany = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCompany)
        BrighttechPack.GlobalMethods.FillCombo(chkCompanyId, dtCompany, "COMPANYNAME", , "ALL")
    End Function
    Function funcExit() As Integer
        Me.Close()
    End Function
    Function funcSave() As Integer
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Function
        If objGPack.Validator_Check(Me) Then Exit Function
        If Not flagUpdate And objGPack.GetSqlValue("SELECT 'CHECK' FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = '" & txtCostId1_MAN.Text & "'").Length > 0 Then
            MsgBox("CostId Already Exist", MsgBoxStyle.Information)
            txtCostID1_Man.Focus()
            Exit Function
        End If
        If objGPack.DupChecker(txtCostName1_Man, "SELECT 1 FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & txtCostName1_Man.Text & "' and COSTID <> '" & txtCostID1_Man.Text & "'") Then
            Exit Function
        End If
        If txtGSTNo.Text = "" Then
            If GSTNOVALID = "R" Then
                MsgBox("GSTIN should not Empty...", MsgBoxStyle.Information)
                txtGSTNo.Focus()
                Exit Function
            ElseIf GSTNOVALID = "W" Then
                If MessageBox.Show("GSTIN should not empty" + vbCrLf + "Do you wish to Continue?", "GstNo Info Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
                    txtGSTNo.Focus()
                    Exit Function
                End If
            End If
        End If
        If Not flagUpdate Then
            funcAdd()
        Else
            funcUpdate()
        End If
    End Function
    Function funcAdd()
        SelectedCompanyId = GetSelectedCompanyId(chkCompanyId, False)
        Dim strField As String = Nothing
        Dim strValue As String = Nothing
        strField = "COSTID"
        strValue = "'" & Trim(txtCostID1_Man.Text) & "'"
        strField += ",COSTNAME"
        strValue += ",'" & Trim(txtCostName1_Man.Text) & "'"
        strField += ",COSTSHORTNAME"
        strValue += ",'" & Trim(txtShortName.Text) & "'"
        If chkCompanyId.Text <> "ALL" Then
            strField += ",COMPANYID"
            strValue += ",'" & SelectedCompanyId & "'"
        Else
            strField += ",COMPANYID"
            strValue += ",NULL"
        End If
        strField += ",ADDRESS1"
        strValue += ",'" & Trim(txtAdd1.Text) & "'"
        strField += ",ADDRESS2"
        strValue += ",'" & Trim(txtAdd2.Text) & "'"
        strField += ",ADDRESS3"
        strValue += ",'" & Trim(txtAdd3.Text) & "'"
        strField += ",ADDRESS4"
        strValue += ",'" & Trim(txtAdd4.Text) & "'"
        strField += ",AREACODE"
        strValue += ",'" & Trim(txtAreaCode.Text) & "'"
        strField += ",PHONE"
        strValue += ",'" & Trim(txtPhone.Text) & "'"
        strField += ",EMAIL"
        strValue += ",'" & Trim(txtEmail.Text) & "'"
        strField += ",LOCALTAXNO"
        strValue += ",'" & Trim(txtTaxNo.Text) & "'"
        strField += ",CSTNO"
        strValue += ",'" & Trim(txtCstNo.Text) & "'"
        strField += ",TINNO"
        strValue += ",'" & Trim(txtTinNo.Text) & "'"
        strField += ",PANNO"
        strValue += ",'" & Trim(txtPanNo.Text) & "'"
        strField += ",TDSNO"
        strValue += ",'" & Trim(txtTDSNo.Text) & "'"
        strField += ",DISPORDER"
        strValue += "," & IIf(Trim(txtDispOrd.Text) = "", 0, Val(Trim(txtDispOrd.Text)))
        strField += ",USERID"
        strValue += "," & userId
        strField += ",UPDATED"
        strValue += ",'" & Today.Date.ToString("yyyy-MM-dd") & "'"
        strField += ",UPTIME"
        strValue += ",'" & Date.Now.ToLongTimeString & "'"
        strField += ",ACCODE"
        strValue += ",'" & objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbAccName.Text & "'") & "'"
        strField += ",AACCODE"
        strValue += ",'" & objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbAlterAcname.Text & "'") & "'"
        strField += ",ADVACCODE"
        strValue += ",'" & objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbAdvAccName.Text & "'") & "'"
        strField += ",CRACCODE"
        strValue += ",'" & objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbCrAccName.Text & "'") & "'"
        strField += ",ACTIVE "
        strValue += ",'" & Mid(cmbActive.Text, 1, 1) & "'"
        strField += ",TANNO"
        strValue += ",'" & Trim(txtTanno.Text) & "'"
        If RATE_BRANCHWISE And CENTR_DB_BR Then
            strField += ",RATEPER"
            strValue += ",'" & Val(txtRatePer_PER.Text) & "'"
        End If
        If REQ_FRANCHISEE Then
            strField += ",COSTTYPE"
            strValue += ",'" & Mid(CmbBranch.Text, 1, 1) & "'"
        End If
        strField += ",GSTNO"
        strValue += ",'" & Trim(txtGSTNo.Text) & "'"
        strSql = " Insert into " & cnAdminDb & "..CostCentre"
        strSql += " (" & strField
        strSql += " ) Values (" & strValue & ")"

        Try
            ExecQuery(SyncMode.Master, strSql, cn)
            funcNew()
        Catch ex As OleDbException
            If cn.State = ConnectionState.Open Then

            End If
            If ex.ErrorCode = 2627 Then
                MsgBox("CostName Already Exist", MsgBoxStyle.Information)
                txtCostName1_Man.Focus()
            Else
                MsgBox(ex.Message)
                MsgBox(ex.StackTrace)
            End If
        Catch ex As Exception
            If cn.State = ConnectionState.Open Then

            End If
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        Finally
            If cn.State = ConnectionState.Open Then

            End If
        End Try
        Return 0
    End Function
    Function funcUpdate()
        Dim dt As New DataTable
        dt.Clear()
        SelectedCompanyId = GetSelectedCompanyId(chkCompanyId, False)

        strSql = " UPDATE " & cnAdminDb & "..COSTCENTRE SET "
        strSql += " COSTID = '" & txtCostID1_Man.Text & "'"
        strSql += " ,COSTNAME = '" & Trim(txtCostName1_Man.Text) & "'"
        strSql += " ,COSTSHORTNAME = '" & Trim(txtShortName.Text) & "'"
        If chkCompanyId.Text <> "ALL" Then
            strSql += " ,COMPANYID = '" & SelectedCompanyId & "'"
        Else
            strSql += " ,COMPANYID = NULL"
        End If
        strSql += " ,ADDRESS1 = '" & Trim(txtAdd1.Text) & "'"
        strSql += " ,ADDRESS2 = '" & Trim(txtAdd2.Text) & "'"
        strSql += " ,ADDRESS3 = '" & Trim(txtAdd3.Text) & "'"
        strSql += " ,ADDRESS4 = '" & Trim(txtAdd4.Text) & "'"
        strSql += " ,AREACODE = '" & Trim(txtAreaCode.Text) & "'"
        strSql += " ,PHONE = '" & Trim(txtPhone.Text) & "'"
        strSql += " ,EMAIL = '" & Trim(txtEmail.Text) & "'"
        strSql += " ,LOCALTAXNO = '" & Trim(txtTaxNo.Text) & "'"
        strSql += " ,CSTNO = '" & Trim(txtCstNo.Text) & "'"
        strSql += " ,TINNO = '" & Trim(txtTinNo.Text) & "'"
        strSql += " ,PANNO = '" & Trim(txtPanNo.Text) & "'"
        strSql += " ,TDSNO = '" & Trim(txtTDSNo.Text) & "'"
        strSql += " ,DISPORDER = '" & Trim(txtDispOrd.Text) & "'"
        strSql += " ,ACCODE = '" & objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbAccName.Text & "'") & "'"
        strSql += " ,AACCODE = '" & objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbAlterAcname.Text & "'") & "'"
        strSql += " ,ADVACCODE = '" & objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbAdvAccName.Text & "'") & "'"
        strSql += " ,CRACCODE = '" & objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbCrAccName.Text & "'") & "'"
        strSql += " ,ACTIVE = '" & Mid(cmbActive.Text, 1, 1) & "'"
        strSql += " ,TANNO = '" & Trim(txtTanno.Text) & "'"
        If RATE_BRANCHWISE And CENTR_DB_BR Then
            strSql += " ,RATEPER = '" & Val(txtRatePer_PER.Text) & "'"
        End If
        If REQ_FRANCHISEE Then
            strSql += " ,COSTTYPE = '" & Mid(CmbBranch.Text, 1, 1) & "'"
        End If
        strSql += " ,GSTNO = '" & Trim(txtGSTNo.Text) & "'"
        strSql += " WHERE COSTID = '" & txtCostID1_Man.Text & "'"
        'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        'cmd.ExecuteNonQuery()
        ExecQuery(SyncMode.Master, strSql, cn)
        funcNew()
        Return 0
    End Function

    Private Sub frmCostCentre_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{tab}")
        ElseIf e.KeyChar = Chr(Keys.Escape) Then
            btnBack_Click(Me, New EventArgs)
        End If
    End Sub
    Private Sub frmCostCentre_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        txtEmail.CharacterCasing = CharacterCasing.Normal
        Me.tabMain.ItemSize = New Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGen.Left, Me.tabGen.Top, Me.tabGen.Width, Me.tabGen.Height))
        Me.tabMain.SelectedTab = tabGen
        'cmbAccName.Items.Clear()
        strSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACTYPE IN ('G','D','I','O') ORDER BY ACNAME"
        objGPack.FillCombo(strSql, cmbAccName, True, False)
        strSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD /*WHERE ACTYPE IN ('G','D','I','O') ORDER BY ACNAME*/"
        objGPack.FillCombo(strSql, cmbAlterAcname, True, False)
        strSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD /*WHERE ACTYPE IN ('G','D','I','O') ORDER BY ACNAME*/"
        cmbAdvAccName.Items.Clear()
        cmbAdvAccName.Items.Add("ALL")
        objGPack.FillCombo(strSql, cmbAdvAccName, True, False)
        strSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACTYPE IN ('G','D','I') ORDER BY ACNAME"
        cmbCrAccName.Items.Clear()
        cmbCrAccName.Items.Add("ALL")
        objGPack.FillCombo(strSql, cmbCrAccName, True, False)
        cmbActive.Items.Clear()
        cmbActive.Items.Add("YES")
        cmbActive.Items.Add("NO")
        funcNew()
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        funcSave()
    End Sub

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click, Button5.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        'funcCallGrid()
        funcView()
        gridView.Focus()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        funcNew()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        funcExit()
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

    Private Sub txtDispOrd_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        Dim keyChar As String
        keyChar = e.KeyChar
        If (Not Char.IsDigit(keyChar)) And AscW(keyChar) <> 8 And AscW(keyChar) <> 13 Then
            e.Handled = True
            MsgBox("Digits only Allowed 1 to 9")
            txtDispOrd.Focus()
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

    Private Sub txtCostName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If e.KeyChar = Chr(Keys.Enter) Then
            If objGPack.DupChecker(txtCostName1_Man, "SELECT 1 FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & txtCostName1_Man.Text & "' AND COSTID <> '" & txtCostID1_Man.Text & "'") Then
                Exit Sub
            Else
                SendKeys.Send("{TAB}")
            End If
        End If
    End Sub

    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
        If Not gridView.RowCount > 0 Then Exit Sub
        Dim list As New List(Of String)
        list.Add("COSTID")
        Dim costId As String = gridView.CurrentRow.Cells("COSTID").Value.ToString
        If DeleteItem(SyncMode.Master, list, "DELETE FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = '" & costId & "'", costId, "COSTCENTRE") Then
            funcCallGrid()
            gridView.Focus()
        End If
    End Sub

    Private Sub txtCostId_MAN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If e.KeyChar = Chr(Keys.Enter) Then
            If Not flagUpdate And objGPack.GetSqlValue("SELECT 'CHECK' FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = '" & txtCostID1_Man.Text & "'").Length > 0 Then
                MsgBox("CostId Already Exist", MsgBoxStyle.Information)
                txtCostID1_Man.Focus()
                Exit Sub
            End If
        End If
    End Sub

    Function funcGetDetails(ByVal tempCostID As String)
        Dim dt As New DataTable
        dt.Clear()
        Dim dt1 As New DataTable
        dt1.Clear()
        Try
            strSql = " select * from " & cnAdminDb & "..COSTCENTRE where COSTID = '" & tempCostID & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If Not dt.Rows.Count > 0 Then
                Return 0
            End If
            With dt.Rows(0)
                txtCostID1_Man.Text = .Item("COSTID").ToString
                txtCostName1_Man.Text = .Item("COSTNAME").ToString
                txtShortName.Text = .Item("COSTSHORTNAME").ToString
                Dim sql As String = "SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE  ACCODE= '" & .Item("ACCODE").ToString & "'"
                da1 = New OleDbDataAdapter(sql, cn)
                da1.Fill(dt1)
                If dt1.Rows.Count > 0 Then
                    cmbAccName.Text = dt1.Rows(0).Item("ACNAME").ToString
                End If
                cmbAlterAcname.Text = objGPack.GetSqlValue("SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE= '" & .Item("AACCODE").ToString & "'", "", "", )
                cmbAdvAccName.Text = objGPack.GetSqlValue("SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE= '" & .Item("ADVACCODE").ToString & "'", "", "", )
                cmbCrAccName.Text = objGPack.GetSqlValue("SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE= '" & .Item("CRACCODE").ToString & "'", "", "", )
                txtAdd1.Text = .Item("ADDRESS1").ToString
                txtAdd2.Text = .Item("ADDRESS2").ToString
                txtAdd3.Text = .Item("ADDRESS3").ToString
                txtAdd4.Text = .Item("ADDRESS4").ToString
                txtPhone.Text = .Item("PHONE").ToString
                txtEmail.Text = .Item("EMAIL").ToString
                txtAreaCode.Text = .Item("AREACODE").ToString
                txtTaxNo.Text = .Item("LOCALTAXNO").ToString
                txtCstNo.Text = .Item("CSTNO").ToString
                txtTinNo.Text = .Item("TINNO").ToString
                txtPanNo.Text = .Item("PANNO").ToString
                txtTDSNo.Text = .Item("TDSNO").ToString
                txtTanno.Text = .Item("TANNO").ToString
                txtGSTNo.Text = .Item("GSTNO").ToString
                txtRatePer_PER.Text = Val(.Item("RATEPER").ToString)
                txtDispOrd.Text = .Item("DISPORDER").ToString
                cmbActive.Text = IIf(.Item("ACTIVE").ToString = "N", "NO", "YES")
                If REQ_FRANCHISEE Then
                    If .Item("COSTTYPE").ToString = "F" Then
                        CmbBranch.Text = "Franchisee"
                    Else
                        CmbBranch.Text = "Branch"
                    End If
                End If
                txtCostID1_Man.Enabled = False
            End With
            tabMain.SelectedTab = tabGen
            txtCostID1_Man.Focus()
            flagUpdate = True
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        Return 0
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
            .Columns("DISPORDER").Visible = False
            .Columns("ADDRESS1").Visible = False
            .Columns("ADDRESS2").Visible = False
            .Columns("ADDRESS3").Visible = False
            .Columns("ADDRESS4").Visible = False
            .Columns("RATEPER").Visible = False
            With .Columns("COSTID")
                .HeaderText = "COSTID"
                .Width = 150
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("COSTNAME")
                .HeaderText = "NAME"
                .Width = 350
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("COMPANYID")
                .Visible = True
                .HeaderText = "COMPANYNAME"
                .Width = 350
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        End With
    End Function

    Private Sub gridView_RowEnter(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.RowEnter
        objGPack.TextClear(pnlViewDet)
        For i As Integer = 0 To chkCompanyId.Items.Count - 1
            chkCompanyId.SetItemChecked(i, CheckState.Unchecked)
        Next
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
            detRatePer.Text = .Cells("RATEPER").Value.ToString
            cmbActive.Text = .Cells("ACTIVE").Value.ToString
            If .Cells("COMPANYID").Value.ToString <> "" Then
                Dim dtusercompanyid As New DataTable
                strSql = " SELECT COMPANYNAME FROM " & cnAdminDb & "..COMPANY WHERE COMPANYID IN ('" & Replace(.Cells("COMPANYID").Value.ToString, ",", "','") & "')"
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtusercompanyid)
                If dtusercompanyid.Rows.Count > 0 Then
                    For iij As Integer = 0 To dtusercompanyid.Rows.Count - 1
                        If chkCompanyId.Items.Contains(dtusercompanyid.Rows(iij).Item("COMPANYNAME").ToString) Then
                            Dim INDX As Integer = chkCompanyId.Items.IndexOf(dtusercompanyid.Rows(iij).Item("COMPANYNAME").ToString)
                            chkCompanyId.SetItemCheckState(INDX, CheckState.Checked)
                        End If
                    Next
                End If
            Else
                chkCompanyId.SetItemCheckState(0, CheckState.Checked)
            End If
        End With
    End Sub
    Function funcView() As Integer
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Function
        Try
            Me.btnOpen.Enabled = False
            'Me.Cursor = Cursors.WaitCursor

            strSql = " SELECT COSTID,COSTNAME,COSTSHORTNAME,COMPANYID,ADDRESS1,ADDRESS2,ADDRESS3,ADDRESS4,"
            strSql += " AREACODE,PHONE,EMAIL,LOCALTAXNO,CSTNO,TINNO,PANNO,TDSNO,TANNO,DISPORDER"
            strSql += " ,CASE WHEN ISNULL(ACTIVE,'')<>'N' THEN 'YES' ELSE 'NO' END ACTIVE,RATEPER "
            strSql += " ,GSTNO"
            strSql += " FROM " & cnAdminDb & "..COSTCENTRE ORDER BY DISPORDER"

            dt = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
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

    Private Sub txtPhone_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If (Not Char.IsDigit(e.KeyChar)) And e.KeyChar <> Chr(Keys.Back) And e.KeyChar <> Chr(Keys.Enter) Then
            e.Handled = True
            MsgBox("Digits Only Allowed 0 to 9", MsgBoxStyle.Information)
            txtPhone.Focus()
        End If
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        For i As Integer = 0 To chkCompanyId.Items.Count - 1
            chkCompanyId.SetItemChecked(i, CheckState.Unchecked)
        Next
        tabMain.SelectedTab = tabGen
        txtCostID1_Man.Focus()
    End Sub

    Private Sub btnDelete_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
        If Not gridView.RowCount > 0 Then Exit Sub
        Dim list As New List(Of String)
        list.Add("COSTID")
        Dim costId As String = gridView.CurrentRow.Cells("COSTID").Value.ToString
        If DeleteItem(SyncMode.Master, list, "DELETE FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = '" & costId & "'", costId, "COSTCENTRE") Then
            funcCallGrid()
            gridView.Focus()
        End If
    End Sub
End Class