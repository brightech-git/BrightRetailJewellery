Imports System.Data.OleDb
Public Class frmSoftControl

    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim flagSave As Boolean = False
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        funcGridStyle(gridView)
        cmbOpenModule.Items.Add("ALL")
        strSql = " select ModuleName from " & cnAdminDb & "..ModuleDetails order by ModuleName"
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        objGPack.FillCombo(strSql, cmbModule)
        cmbModule.Items.Add("")
        objGPack.FillCombo(strSql, cmbOpenModule, False)
        cmbOpenModule.Items.Add("")
        If UCase(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COSTCENTRE'")) = "Y" Then
            strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            objGPack.FillCombo(strSql, cmbCostCenter_MAN, False, False)
            strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE where costid ='" & cnCostId & "'"
            cmbCostCenter_MAN.Text = objGPack.GetSqlValue(strSql)
            cmbCostCenter_MAN.Enabled = True
        Else
            cmbCostCenter_MAN.Enabled = False
        End If
    End Sub
    Function funcNew() As Integer
        objGPack.TextClear(Me)
        flagSave = False
        txtControlValue_OWN.PasswordChar = Nothing
        cmbControlType.Enabled = True
        cmbControlType.Text = "TEXT"
        txtControlId__Man.Enabled = True
        cmbOpenModule.Text = "ALL"
        cmbModule.Text = "ESTIMATION"
        dtpDate.Value = GetServerDate(tran)
        funcOpen()
        txtControlId__Man.Select()
    End Function
    Function funcOpen() As Integer
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Function
        txtOpenControlId.Focus()
    End Function
    Function funcExit() As Integer
        Me.Close()
    End Function
    Function funcSave() As Integer
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Function
        If objGPack.Validator_Check(Me) Then Exit Function
        Dim mbcostid As String = objGPack.GetSqlValue("select costid from " & cnAdminDb & "..costcentre where costname = '" & cmbCostCenter_MAN.Text & "'")
        If flagSave = False Then

            If objGPack.DupChecker(txtControlName__Man, "SELECT 1 FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLNAME = '" & txtControlName__Man.Text & "' AND CTLID <> '" & txtControlId__Man.Text & "'") Then Exit Function
            If objGPack.DupChecker(txtControlId__Man, "SELECT 1 FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = '" & txtControlId__Man.Text & "'") Then
                Exit Function
            End If
            funcAdd(mbcostid)
        Else
            funcUpdate(mbcostid)
        End If
        SetGlobalVariables()
    End Function
    Function funcGetDefaultModule(ByVal combo As ComboBox) As String
        Dim str As String = ""
        Select Case combo.Text
            Case "ADMIN"
                Return "A"
            Case "STOCK"
                Return "S"
            Case "ESTIMATION"
                Return "E"
            Case "POINT OF SALES"
                Return "P"
            Case "ADDRESS BOOK"
                Return "B"
            Case "ORDER REPAIR"
                Return "R"
            Case "FINANCE"
                Return "F"
        End Select
        Return Trim(str)
    End Function
    Function funcAdd(Optional ByVal Ccostid As String = Nothing) As Integer
        If Ccostid Is Nothing Then Ccostid = cnCostId
        strSql = " Insert into " & cnAdminDb & "..Softcontrol"
        strSql += " ("
        strSql += " ctlId,"
        strSql += " ctlName,"
        strSql += " ctlType,"
        strSql += " ctlText,"
        strSql += " ctlModule,"
        strSql += " ctlUpd,"
        strSql += " Userid,Updated,Uptime,costid"
        strSql += " )Values("
        strSql += " '" & txtControlId__Man.Text & "'" 'ctlId
        strSql += " ,'" & txtControlName__Man.Text & "'" 'ctlName
        strSql += " ,'" & funcGetControlType() & "'" 'ctlType
        If cmbControlType.Text = "DATE" Then
            strSql += " ,'" & dtpDate.Text & "'" 'ctlText
        ElseIf cmbControlType.Text = "PASS" Then
            strSql += ",'" & BrighttechPack.Methods.Encrypt(txtControlValue_OWN.Text) & "'" 'CTLTEXT
        Else
            strSql += " ,'" & txtControlValue_OWN.Text & "'" 'ctlText
        End If
        strSql += " ,'" & funcGetDefaultModule(cmbModule) & "'" 'ctlModule
        'strSql += " ,'" & cnCompanyId & "'" 'ctlUpd
        strSql += " ,''" 'ctlUpd
        strSql += " ," & userId & "" 'Userid
        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'Updated
        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'Uptime
        If Ccostid Is Nothing Then strSql += ",NULL" Else strSql += " ,'" & Ccostid & "'"
        strSql += " )"
        Try
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            If txtControlId__Man.Text.ToUpper = "COSTID" Then
                cnCostId = txtControlValue_OWN.Text
            End If
            funcNew()
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function
    Function funcGetControlType() As String
        Select Case cmbControlType.Text
            Case "TEXT"
                Return "T"
            Case "DECIMAL"
                Return "D"
            Case "DATE"
                Return "E"
            Case "NUMBER"
                Return "N"
            Case "PASS"
                Return "P"
        End Select
        Return "T"
    End Function
    Function funcUpdate(Optional ByVal mbcostid As String = Nothing) As Integer

        strSql = "select count(*) as cnt from " & cnAdminDb & "..SoftControl where ctlId = '" & txtControlId__Man.Text & "'"
        If mbcostid <> "" Then strSql += " and costId = '" & mbcostid & "'"
        Dim costidchk As Boolean = True
        If Val(objGPack.GetSqlValue(strSql).ToString) = 0 Then costidchk = False
        strSql = " Update " & cnAdminDb & "..SoftControl Set"
        strSql += " ctlName='" & txtControlName__Man.Text & "'"
        strSql += " ,ctlType='" & funcGetControlType() & "'"
        If cmbControlType.Text = "DATE" Then
            strSql += " ,ctlText='" & dtpDate.Text & "'"
        ElseIf cmbControlType.Text = "PASS" Then
            strSql += ",ctltext = '" & BrighttechPack.Methods.Encrypt(txtControlValue_OWN.Text) & "'" 'CTLTEXT
        Else
            strSql += " ,ctltext = '" & txtControlValue_OWN.Text & "'" 'ctlText
        End If
        strSql += " ,ctlModule='" & funcGetDefaultModule(cmbModule) & "'"
        strSql += " ,ctlUpd='" & cnCompanyId & "'"
        strSql += " ,costid='" & mbcostid & "'"
        strSql += " ,Userid='" & userId & "'"
        strSql += " ,Updated='" & Today.Date.ToString("yyyy-MM-dd") & "'"
        strSql += " ,Uptime='" & Date.Now.ToLongTimeString & "'"
        strSql += " where ctlId = '" & txtControlId__Man.Text & "'"
        If mbcostid <> "" And costidchk Then strSql += " and costId = '" & mbcostid & "'"
        Try
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            If txtControlId__Man.Text.ToUpper = "COSTID" Then
                cnCostId = txtControlValue_OWN.Text
            End If
            btnSearch_Click(Me, New EventArgs)
            funcNew()
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function
    Function funcGetDetails(ByVal temp As String) As Integer
        strSql = " select b.costname,ctlId,ctlName,"
        strSql += " case when ctlType = 'T' then 'TEXT'"
        strSql += " when ctlType = 'N' then 'NUMBER'"
        strSql += " when ctlType = 'D' then 'DECIMAL' when ctltype = 'P' then 'PASS' else 'DATE' end as CtlType,"
        strSql += " ctlText,"
        strSql += " case when ctlModule = 'A' then 'ADMIN'"
        strSql += " when ctlModule = 'S' then 'STOCK'"
        strSql += " when ctlModule = 'E' then 'ESTIMATION'"
        strSql += " when ctlModule = 'P' then 'POINT OF SALES'"
        strSql += " when ctlModule = 'A' then 'ADDRESSBOOK'"
        strSql += " when ctlModule = 'O' then 'ORDER REPAIR'"
        strSql += " when ctlModule = 'F' then 'FINANCE' else '' end as ctlModule,"
        strSql += " ctlUpd,colId"
        strSql += " from " & cnAdminDb & "..SoftControl a"
        strSql += " Left Join " & cnAdminDb & "..costcentre b on a.costid = b.costid"
        strSql += " where Ctlid = '" & temp & "'"
        strSql += " AND CTLMODULE <> 'X'"
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Return 0
        End If
        Dim dat As New DateTime
        Try
            With dt.Rows(0)
                If .Item("ctlId").ToString = "TRANDB_ADMINDB" Or .Item("ctlId").ToString = "TRANDB_ADMINDB_ORD" Then MsgBox("Not able to edit this control", MsgBoxStyle.Information) : Exit Function
                txtControlId__Man.Text = .Item("ctlId").ToString
                txtControlName__Man.Text = .Item("ctlName").ToString
                cmbControlType.Text = .Item("ctlType").ToString
                If .Item("ctltype") = "DATE" Then
                    txtControlValue_OWN.Clear()
                    dtpDate.Value = .Item("ctlText")
                ElseIf .Item("CTLTYPE") = "PASS" Then
                    txtControlValue_OWN.Text = BrighttechPack.Methods.Decrypt(.Item("ctlText").ToString)
                    cmbControlType.Enabled = False
                Else
                    txtControlValue_OWN.Text = .Item("ctlText").ToString
                End If
                cmbModule.Text = .Item("ctlModule").ToString
                cmbCostCenter_MAN.Text = .Item("costname").ToString
            End With
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            flagSave = True
            txtControlId__Man.Enabled = False
        End Try
    End Function

    Private Sub frmSoftControl_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtControlId__Man.Focused Then
                Exit Sub
            End If
            If txtControlName__Man.Focused Then
                Exit Sub
            End If
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        funcSave()
    End Sub

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click
        funcOpen()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        funcNew()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        funcExit()
    End Sub

    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        funcSave()
    End Sub

    Private Sub OpenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click
        funcOpen()
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        funcNew()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        funcExit()
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        strSql = " SELECT UPPER(CTLID) ID,UPPER(CTLNAME) NAME,"
        strSql += " CASE WHEN CTLTYPE = 'T' THEN 'TEXT'"
        strSql += " WHEN CTLTYPE = 'N' THEN 'NUMBER'"
        strSql += " WHEN CTLTYPE = 'D' THEN 'DECIMAL' WHEN CTLTYPE = 'P' THEN 'PASS' ELSE 'DATE' END AS TYPE,"
        strSql += " CTLTEXT TEXT,"
        strSql += " CASE WHEN CTLMODULE = 'A' THEN 'ADMIN'"
        strSql += " WHEN CTLMODULE = 'S' THEN 'STOCK'"
        strSql += " WHEN CTLMODULE = 'E' THEN 'ESTIMATION'"
        strSql += " WHEN CTLMODULE = 'P' THEN 'POINT OF SALES'"
        strSql += " WHEN CTLMODULE = 'A' THEN 'ADDRESSBOOK'"
        strSql += " WHEN CTLMODULE = 'O' THEN 'ORDER REPAIR'"
        strSql += " WHEN CTLMODULE = 'F' THEN 'FINANCE' ELSE '' END AS MODULE,"
        strSql += " CTLUPD COMPID,COLID"
        strSql += " ,B.COSTNAME"
        strSql += " FROM " & cnAdminDb & "..SOFTCONTROL a"
        'If strBCostid <> Nothing Then
        'End If
        strSql += " left join " & cnAdminDb & "..COSTCENTRE b on a.costid = b.costid  "
        strSql += " WHERE 1=1"
        strSql += " AND CTLID NOT LIKE '%VBC%'"
        If cmbOpenModule.Text <> "ALL" Then
            strSql += " AND CTLMODULE = '" & funcGetDefaultModule(cmbOpenModule) & "'"
        End If
        If txtOpenControlId.Text <> "" Then
            strSql += " AND CTLID LIKE '%" & txtOpenControlId.Text & "%'"
        End If
        If ChkAll.Checked = False Then
            strSql += " AND CTLUPD LIKE '" & cnCompanyId & "%'"
        End If
        If cmbCostCenter_MAN.Enabled And cmbCostCenter_MAN.Text <> "" Then
            strSql += " AND ISNULL(B.COSTNAME,'') ='" & cmbCostCenter_MAN.Text & "'"
        End If
        strSql += " AND CTLMODULE <> 'X' ORDER BY B.COSTNAME"
        funcOpenGrid(strSql, gridView)
        With gridView
            .Columns("ID").Width = 120
            .Columns("NAME").Width = 400
            .Columns("TYPE").Width = 80
            .Columns("TEXT").Width = 120
            .Columns("MODULE").Width = 70
            .Columns("COMPID").Visible = False
            .Columns("COLID").Visible = False
        End With
        gridView.Select()
    End Sub

    Private Sub gridView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.GotFocus
        lblStatus.Visible = True
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            If gridView.RowCount > 0 Then
                funcGetDetails(gridView.Item(0, gridView.CurrentRow.Index).Value)
                txtControlName__Man.Focus()
            End If
        ElseIf e.KeyCode = Keys.Escape Then
            If txtControlId__Man.Enabled = True Then
                txtControlId__Man.Focus()
            Else
                txtControlName__Man.Focus()
            End If
        End If
    End Sub

    Private Sub txtControlName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtControlName__Man.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If flagSave = False Then
                If objGPack.DupChecker(txtControlName__Man, "SELECT 1 FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLNAME = '" & txtControlName__Man.Text & "' AND CTLID <> '" & txtControlId__Man.Text & "'") Then
                    Exit Sub
                Else
                    SendKeys.Send("{TAB}")
                End If
            Else
                SendKeys.Send("{TAB}")
            End If

        End If
    End Sub

    Private Sub txtControlId_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtControlId__Man.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If objGPack.DupChecker(txtControlId__Man, "SELECT 1 FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = '" & txtControlId__Man.Text & "'") Then
                txtControlId__Man.Focus()
                Exit Sub
            Else
                SendKeys.Send("{TAB}")
            End If
        End If
    End Sub

    Private Sub cmbControlType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbControlType.SelectedIndexChanged
        txtControlValue_OWN.PasswordChar = ""
        If cmbControlType.Text = "DATE" Then
            dtpDate.Enabled = True
            txtControlValue_OWN.Enabled = False
        ElseIf cmbControlType.Text = "PASS" Then
            dtpDate.Enabled = False
            txtControlValue_OWN.PasswordChar = "*"
            txtControlValue_OWN.Enabled = True
        Else
            dtpDate.Enabled = False
            txtControlValue_OWN.Enabled = True
        End If
    End Sub

    Private Sub gridView_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.LostFocus

        lblStatus.Visible = False
    End Sub

    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
    End Sub

    Private Sub frmSoftControl_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        funcNew()
    End Sub

    Private Sub txtControlValue_OWN_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtControlValue_OWN.Leave
        'If txtControlValue_OWN.Text.Length = 1 Then txtControlValue_OWN.Text = txtControlValue_OWN.Text.ToUpper
        If cmbControlType.Text = "TEXT" _
                And Not txtControlValue_OWN.Text.Contains("@") _
                And Not txtControlId__Man.Text.Contains("SMS") _
                And Not txtControlId__Man.Text.Contains("BILLNAME_LANG") _
                And Not txtControlValue_OWN.Text.Contains("http") _
                And Not txtControlValue_OWN.Text.Contains("//") Then
            txtControlValue_OWN.Text = txtControlValue_OWN.Text.ToUpper
        End If
    End Sub

    Private Sub txtControlValue_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtControlValue_OWN.TextChanged

    End Sub
End Class