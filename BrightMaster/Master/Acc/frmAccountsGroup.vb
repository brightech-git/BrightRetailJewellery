Imports System.Data.OleDb
Public Class frmAccountsGroup
    Dim strSql As String
    Dim dt As New DataTable
    Dim da As OleDbDataAdapter
    Dim flagSave As Boolean
    Dim dumpDt As New DataTable
    Dim cmd As OleDbCommand
    Dim upAcGrpCode As Integer = Nothing
    Dim dtAltered As New DataTable
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        funcGridStyle(gridView)
        cmbGroupLedger.Items.Add("BALANCE SHEET")
        cmbGroupLedger.Items.Add("PROFIT & LOSS")
        cmbGroupLedger.Items.Add("TRADING")
        funcNew()
    End Sub
    Function funcGetDetails(ByVal AcGrpCode As Integer) As Integer
        flagSave = True
        strSql = " SELECT "
        strSql += " AcGrpCode, ACGRPNAME"
        strSql += " ,(SELECT TOP 1 ACGRPNAME FROM " & CNADMINDB & "..ACGROUP WHERE ACGRPCODE = A.ACMAINCODE) AS ACMAINGROUP"
        strSql += " ,CASE WHEN GRPLEDGER = 'B' THEN 'BALANCE SHEET' "
        strSql += "       WHEN GRPLEDGER = 'P' THEN 'PROFIT & LOSS' ELSE 'TRADING' END AS GRPLEDGER"
        strSql += " ,CASE WHEN GRPTYPE = 'E' THEN 'EXPENDITURE' "
        strSql += "       WHEN GRPTYPE = 'I' THEN 'INCOME'"
        strSql += "       WHEN GRPTYPE = 'A' THEN 'ASSETS' ELSE 'LIABLITIES' END GRPTYPE"
        strSql += " ,GRPTYPE,SCHEDULECODE,DISPORDER,ACMAINSUB"
        strSql += " FROM " & CNADMINDB & "..ACGROUP AS A WHERE ACGRPCODE = " & AcGrpCode & ""
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Exit Function
        End If
        upAcGrpCode = AcGrpCode
        With dt.Rows(0)
            txtAcGroupName__Man.Text = .Item("AcGrpName").ToString
            cmbAcGroup_Man.Text = .Item("ACMAINGROUP").ToString
            cmbAcGroup_Man_SelectedIndexChanged(Me, New EventArgs)
            txtDispOrder_NUM.Text = .Item("DISPORDER").ToString
            txtDispCaption.Text = .Item("SCHEDULECODE").ToString
            If .Item("ACMAINSUB").ToString = "1" Then
                cmbGroupLedger.Enabled = True
                cmbGroupType.Enabled = True
            Else
                cmbGroupLedger.Enabled = False
                cmbGroupType.Enabled = False
            End If
        End With
        txtAcGroupName__Man.Focus()
    End Function
    Function funcFillDataTable(ByVal temp As DataTable) As DataTable
        Dim dt2 As New DataTable
        dumpDt = temp.Copy
        dumpDt.Rows.Clear()
        Dim row As DataRow = Nothing
        Dim AcMainSub As Integer = 2
        ''Rajkumar for NSC 29/10/2014
        funcAcSubMain(temp, AcMainSub)
        'For cnt As Integer = 0 To temp.Rows.Count - 1
        '    dumpDt.ImportRow(temp.Rows(cnt))
        '    strSql = " SELECT "
        '    strSql += " ACGRPCODE,ACMAINCODE"
        '    strSql += " ,CASE WHEN ACMAINSUB = '1' THEN ACGRPNAME WHEN ACMAINSUB = '2' THEN '      '+ACGRPNAME ELSE '          '+ACGRPNAME END AS ACGRPNAME"
        '    strSql += " ,CASE WHEN GRPLEDGER = 'B' THEN 'BALANCE SHEET' "
        '    strSql += "       WHEN GRPLEDGER = 'P' THEN 'PROFIT & LOSS' ELSE 'TRADING' END AS GRPLEDGER"
        '    strSql += " ,CASE WHEN GRPTYPE = 'E' THEN 'EXPENDITURE' "
        '    strSql += "       WHEN GRPTYPE = 'I' THEN 'INCOME'"
        '    strSql += "       WHEN GRPTYPE = 'A' THEN 'ASSETS' ELSE 'LIABLITIES' END GRPTYPE"
        '    strSql += " ,DISPORDER,SCHEDULECODE AS DISPCAPTION"
        '    strSql += " FROM " & cnAdminDb & "..ACGROUP WHERE ACMAINCODE = '" & temp.Rows(cnt).Item("ACGRPCODE").ToString & "' AND ACMAINSUB = '2'"
        '    strSql += " ORDER BY DISPORDER"
        '    da = New OleDbDataAdapter(strSql, cn)
        '    dt.Clear()
        '    da.Fill(dt)
        '    ''Rajkumar for NSC 29/10/2014
        '    If dt.Rows.Count > 0 Then
        '        funcAcSubMain(dt, dumpDt, AcMainSub)
        '    End If
        '    'For i As Integer = 0 To dt.Rows.Count - 1
        '    '    dumpDt.ImportRow(dt.Rows(i))
        '    '    strSql = " SELECT "
        '    '    strSql += " ACGRPCODE,ACMAINCODE"
        '    '    strSql += " ,CASE WHEN ACMAINSUB = '1' THEN ACGRPNAME WHEN ACMAINSUB = '2' THEN '      '+ACGRPNAME ELSE '          '+ACGRPNAME END AS ACGRPNAME"
        '    '    strSql += " ,CASE WHEN GRPLEDGER = 'B' THEN 'BALANCE SHEET' "
        '    '    strSql += "       WHEN GRPLEDGER = 'P' THEN 'PROFIT & LOSS' ELSE 'TRADING' END AS GRPLEDGER"
        '    '    strSql += " ,CASE WHEN GRPTYPE = 'E' THEN 'EXPENDITURE' "
        '    '    strSql += "       WHEN GRPTYPE = 'I' THEN 'INCOME'"
        '    '    strSql += "       WHEN GRPTYPE = 'A' THEN 'ASSETS' ELSE 'LIABLITIES' END GRPTYPE"
        '    '    strSql += " ,DISPORDER,SCHEDULECODE AS DISPCAPTION"
        '    '    strSql += " FROM " & cnAdminDb & "..ACGROUP WHERE ACMAINCODE = '" & dt.Rows(i).Item("ACGRPCODE").ToString & "' AND ACMAINSUB = '3'"
        '    '    strSql += " ORDER BY DISPORDER"
        '    '    da = New OleDbDataAdapter(strSql, cn)
        '    '    dt2.Clear()
        '    '    da.Fill(dt2)
        '    '    For j As Integer = 0 To dt2.Rows.Count - 1
        '    '        dumpDt.ImportRow(dt2.Rows(j))
        '    '    Next
        '    'Next
        'Next
        Return dumpDt
    End Function
    Function funcAcSubMain(ByVal dt As DataTable, ByVal AcMainSub As Integer) As DataTable
        For i As Integer = 0 To dt.Rows.Count - 1
            dumpDt.ImportRow(dt.Rows(i))
            strSql = " SELECT "
            strSql += " ACGRPCODE,ACMAINCODE"
            strSql += " ,CASE WHEN ACMAINSUB = '1' THEN ACGRPNAME "
            strSql += " ELSE SPACE(" & AcMainSub & "*5)+ACGRPNAME END AS ACGRPNAME"
            strSql += " ,CASE WHEN GRPLEDGER = 'B' THEN 'BALANCE SHEET' "
            strSql += "       WHEN GRPLEDGER = 'P' THEN 'PROFIT & LOSS' ELSE 'TRADING' END AS GRPLEDGER"
            strSql += " ,CASE WHEN GRPTYPE = 'E' THEN 'EXPENDITURE' "
            strSql += "       WHEN GRPTYPE = 'I' THEN 'INCOME'"
            strSql += "       WHEN GRPTYPE = 'A' THEN 'ASSETS' ELSE 'LIABLITIES' END GRPTYPE"
            strSql += " ,DISPORDER,SCHEDULECODE AS DISPCAPTION"
            strSql += " FROM " & cnAdminDb & "..ACGROUP WHERE ACMAINCODE = '" & dt.Rows(i).Item("ACGRPCODE").ToString & "' AND ACMAINSUB = " & AcMainSub
            strSql += " ORDER BY DISPORDER"
            da = New OleDbDataAdapter(strSql, cn)
            Dim dt2 As New DataTable
            da.Fill(dt2)
            If dt2.Rows.Count > 0 Then
                funcAcSubMain(dt2, AcMainSub + 1)
            End If
        Next
    End Function
    Function funcCallGrid() As Integer
        strSql = " SELECT"
        strSql += " ACGRPCODE,ACMAINCODE"
        strSql += " ,ACGRPNAME"
        strSql += " ,CASE WHEN GRPLEDGER = 'B' THEN 'BALANCE SHEET' "
        strSql += "       WHEN GRPLEDGER = 'P' THEN 'PROFIT & LOSS' ELSE 'TRADING' END AS GRPLEDGER"
        strSql += " ,CASE WHEN GRPTYPE = 'E' THEN 'EXPENDITURE' "
        strSql += "       WHEN GRPTYPE = 'I' THEN 'INCOME'"
        strSql += "       WHEN GRPTYPE = 'A' THEN 'ASSETS' ELSE 'LIABLITIES' END GRPTYPE"
        strSql += " ,DISPORDER,SCHEDULECODE AS DISPCAPTION"
        strSql += " FROM " & cnAdminDb & "..ACGROUP"
        strSql += " WHERE ACMAINSUB = '1' ORDER BY DISPORDER "
        da = New OleDbDataAdapter(strSql, cn)
        Dim dtGrid As New DataTable
        dtGrid.Clear()
        da.Fill(dtGrid)
        gridView.DataSource = funcFillDataTable(dtGrid)
        gridView.Columns("ACGRPCODE").Visible = False
        gridView.Columns("ACMAINCODE").Visible = False
        gridView.Columns("DISPORDER").MinimumWidth = 100
        'gridView.Columns("DISPORDER").SortMode = DataGridViewColumnSortMode.NotSortable
        gridView.Columns("DISPCAPTION").MinimumWidth = 100
        'gridView.Columns("DISPCAPTION").SortMode = DataGridViewColumnSortMode.NotSortable
        gridView.Columns("ACGRPNAME").MinimumWidth = 250
        'gridView.Columns("ACGRPNAME").SortMode = DataGridViewColumnSortMode.NotSortable
        gridView.Columns("GRPLEDGER").MinimumWidth = 150
        'gridView.Columns("GRPLEDGER").SortMode = DataGridViewColumnSortMode.NotSortable
        gridView.Columns("GRPTYPE").MinimumWidth = 100
        'gridView.Columns("GRPTYPE").SortMode = DataGridViewColumnSortMode.NotSortable
    End Function
    Function funcAdd() As Integer
        Dim grpCode As Integer = Nothing
        Dim mainCode As Integer = Nothing
        Dim mainSub As String = Nothing
        ''FIND GRPCODE
        strSql = " SELECT ISNULL(MAX(ACGRPCODE),0)+1 ACGRPCODE FROM " & CNADMINDB & "..ACGROUP "
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            grpCode = Val(dt.Rows(0).Item("ACGRPCODE").ToString)
        End If
        ''FIND MAINCODE
        strSql = " SELECT ACGRPCODE,ACMAINSUB FROM " & CNADMINDB & "..ACGROUP WHERE ACGRPNAME = '" & cmbAcGroup_Man.Text & "'"
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            mainCode = Val(dt.Rows(0).Item("ACGRPCODE").ToString)
            ''Rajkumar for NSC 29/10/2014
            'If StrComp(dt.Rows(0).Item("ACMAINSUB").ToString, "1", CompareMethod.Text) = 0 Then
            '    mainSub = "2"
            'Else
            '    mainSub = "3"
            'End If
            mainSub = Val(dt.Rows(0).Item("ACMAINSUB").ToString) + 1
        Else
            mainCode = grpCode
            mainSub = "1"
        End If
        strSql = " INSERT INTO " & cnAdminDb & "..ACGROUP("
        strSql += " ACGrpCode,"
        strSql += " AcGrpName,"
        strSql += " ACMainCode,"
        strSql += " GrpLedger,"
        strSql += " GrpType,"
        strSql += " ScheduleCode,"
        strSql += " Disporder,"
        strSql += " UserId,"
        strSql += " Crdate,"
        strSql += " CrTime,ACMAINSUB"
        strSql += " ) VALUES ("
        strSql += " " & grpCode & ","
        strSql += " '" & txtAcGroupName__Man.Text & "',"
        strSql += " '" & mainCode & "',"
        strSql += " '" & Mid(cmbGroupLedger.Text, 1, 1) & "',"
        strSql += " '" & Mid(cmbGroupType.Text, 1, 1) & "',"
        strSql += " '" & txtDispCaption.Text & "'," 'SCHEDULE CODE OR DISPLAY CAP
        strSql += " " & Val(txtDispOrder_NUM.Text) & ","
        strSql += " " & userId & ","
        strSql += " '" & Today.Date.ToString("yyyy-MM-dd") & "',"
        strSql += " '" & Date.Now.ToLongTimeString & "'"
        strSql += " ,'" & mainSub & "'"
        strSql += ")"
        ExecQuery(SyncMode.Master, strSql, cn, tran)
        'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
    End Function
    Function funcUpdateSubMains(ByVal acGrpCode As Integer, ByVal acMainCode As Integer, ByVal parentState As String) As Integer
        dt.Clear()
        strSql = " SELECT ACGRPCODE,ACMAINSUB FROM " & cnAdminDb & "..ACGROUP WHERE ACMAINCODE = " & acGrpCode & ""
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        For cnt As Integer = 0 To dt.Rows.Count - 1
            strSql = " UPDATE " & cnAdminDb & "..ACGROUP"
            strSql += " SET"
            'strSql += " ACMAINCODE = " & acMainCode & ""
            strSql += " ACMAINCODE = " & acGrpCode & ""
            strSql += " ,GRPLEDGER = '" & Mid(cmbGroupLedger.Text, 1, 1) & "'"
            strSql += " ,GRPTYPE = '" & Mid(cmbGroupType.Text, 1, 1) & "'"
            strSql += " ,SCHEDULECODE = '" & txtDispCaption.Text & "'"
            strSql += " ,DISPORDER = " & Val(txtDispOrder_NUM.Text) & ""
            strSql += " ,USERID = " & userId & ""
            strSql += " ,CRDATE = '" & Today.Date.ToString("yyyy-MM-dd") & "'"
            strSql += " ,CRTIME = '" & Date.Now.ToLongTimeString & "'"
            strSql += " ,ACMAINSUB = '" & parentState + 1 & "'"
            strSql += " WHERE ACGRPCODE = " & Val(dt.Rows(cnt).Item("ACGRPCODE").ToString) & ""
            ExecQuery(SyncMode.Master, strSql, cn, tran)
            'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
            Dim dtSub As New DataTable
            dtSub.Clear()
            strSql = " SELECT ACGRPCODE,ACMAINSUB FROM " & cnAdminDb & "..ACGROUP WHERE ACMAINCODE = " & Val(dt.Rows(cnt).Item("ACGRPCODE").ToString) & ""
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtSub)
            For i As Integer = 0 To dtSub.Rows.Count - 1
                strSql = " UPDATE " & cnAdminDb & "..ACGROUP"
                strSql += " SET"
                strSql += " ACMAINCODE = " & acMainCode & ""
                strSql += " ,GRPLEDGER = '" & Mid(cmbGroupLedger.Text, 1, 1) & "'"
                strSql += " ,GRPTYPE = '" & Mid(cmbGroupType.Text, 1, 1) & "'"
                strSql += " ,SCHEDULECODE = '" & txtDispCaption.Text & "'"
                strSql += " ,DISPORDER = " & Val(txtDispOrder_NUM.Text) & ""
                strSql += " ,USERID = " & userId & ""
                strSql += " ,CRDATE = '" & Today.Date.ToString("yyyy-MM-dd") & "'"
                strSql += " ,CRTIME = '" & Date.Now.ToLongTimeString & "'"
                strSql += " ,ACMAINSUB = '" & parentState & "'"
                strSql += " WHERE ACGRPCODE = " & Val(dtSub.Rows(i).Item("ACGRPCODE").ToString) & ""
                '   cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
                ExecQuery(SyncMode.Master, strSql, cn, tran)
            Next
        Next
    End Function
    Function funcUpdate() As Integer
        Dim mainCode As Integer = Nothing
        Dim mainSub As String = Nothing
        mainCode = Val(objGPack.GetSqlValue("SELECT acgrpcode FROM " & cnAdminDb & "..ACGROUP WHERE acgrpname = '" & cmbAcGroup_Man.Text & "'").ToString)
        If mainCode = Nothing Or mainCode = 0 Then
            mainCode = upAcGrpCode
        End If
        If mainCode = upAcGrpCode Then
            mainSub = "1"
        Else
            strSql = " SELECT ACMAINSUB FROM " & cnAdminDb & "..ACGROUP"
            strSql += " WHERE ACGRPCODE = '" & mainCode & "'"
            mainSub = Val(objGPack.GetSqlValue(strSql)) + 1
            'Select Case Val(objGPack.GetSqlValue(strSql))
            '    Case 1
            '        mainSub = "2"
            '    Case 2
            '        mainSub = "3"
            '    Case Else
            '        mainSub = "0"
            'End Select
        End If
        Dim updAcGrpName As String = objGPack.GetSqlValue("SELECT ACGRPNAME FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPCODE = '" & upAcGrpCode & "'")
        strSql = " UPDATE " & cnAdminDb & "..ACGROUP"
        strSql += " SET"
        strSql += " ACGRPCODE = " & upAcGrpCode & ""
        strSql += " ,ACGRPNAME = '" & txtAcGroupName__Man.Text & "'"
        strSql += " ,ACMAINCODE = " & mainCode & ""
        strSql += " ,GRPLEDGER = '" & Mid(cmbGroupLedger.Text, 1, 1) & "'"
        strSql += " ,GRPTYPE = '" & Mid(cmbGroupType.Text, 1, 1) & "'"
        strSql += " ,SCHEDULECODE = '" & txtDispCaption.Text & "'"
        strSql += " ,DISPORDER = " & Val(txtDispOrder_NUM.Text) & ""
        strSql += " ,USERID = " & userId & ""
        strSql += " ,CRDATE = '" & Today.Date.ToString("yyyy-MM-dd") & "'"
        strSql += " ,CRTIME = '" & Date.Now.ToLongTimeString & "'"
        strSql += " ,ACMAINSUB = '" & mainSub & "'"
        strSql += " WHERE ACGRPCODE = " & upAcGrpCode & ""
        ExecQuery(SyncMode.Master, strSql, cn, tran)
        'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
        dt.Clear()
        funcUpdateSubMains(upAcGrpCode, mainCode, mainSub)
    End Function
    Function funcSave() As Integer
        If objGPack.Validator_Check(Me) Then Exit Function
        If objGPack.DupChecker(txtAcGroupName__Man, "SELECT 1 FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPNAME = '" & txtAcGroupName__Man.Text & "' AND ACGRPCODE <> '" & upAcGrpCode & "'") Then
            Exit Function
        End If
        If flagSave = False Then
            funcAdd()
        ElseIf flagSave = True Then
            funcUpdate()
        End If
        funcNew()
    End Function
    Function funcNew() As Integer
        btnSave.Enabled = False
        objGPack.TextClear(Me)
        flagSave = False
        cmbAcGroup_Man.Text = ""

        'strSql = " SELECT ACGRPNAME FROM " & cnAdminDb & "..ACGROUP WHERE ACMAINSUB IN ('1','2') ORDER BY ACGRPNAME"
        strSql = " SELECT ACGRPNAME FROM " & cnAdminDb & "..ACGROUP ORDER BY ACGRPNAME"
        objGPack.FillCombo(strSql, cmbAcGroup_Man, , False)
        cmbGroupLedger.Enabled = True
        cmbGroupType.Enabled = True
        funcCallGrid()
        txtAcGroupName__Man.Select()
    End Function

    Private Sub frmAccountsGroup_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtAcGroupName__Man.Focused Then
                Exit Sub
            End If
            SendKeys.Send("{tab}")
        End If
    End Sub
    Private Sub cmbGroupLedger_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbGroupLedger.SelectedIndexChanged
        cmbGroupType.Items.Clear()
        If cmbGroupLedger.Text = "BALANCE SHEET" Then
            cmbGroupType.Items.Add("ASSETS")
            cmbGroupType.Items.Add("LIABLITIES")
            cmbGroupType.Text = "ASSETS"
        Else
            cmbGroupType.Items.Add("INCOME")
            cmbGroupType.Items.Add("EXPENDITURE")
            cmbGroupType.Text = "INCOME"
        End If
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        Me.btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        Me.btnSave_Click(Me, New EventArgs)
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        funcSave()
    End Sub

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click
        gridView.Focus()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        funcNew()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub OpenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click
        Me.btnOpen_Click(Me, New EventArgs)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub cmbAcGroup_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        If flagSave = True Then
            Exit Sub
        End If
        If txtAcGroupName__Man.Text <> "" Then
            For CNT As Integer = 0 To cmbAcGroup_Man.Items.Count - 1
                If StrComp(txtAcGroupName__Man.Text, cmbAcGroup_Man.Items(CNT).ToString) = 0 Then
                    Exit Sub
                End If
            Next
            cmbAcGroup_Man.Items.Insert(0, txtAcGroupName__Man.Text)
            cmbAcGroup_Man.Text = txtAcGroupName__Man.Text
        End If
    End Sub

    Private Sub gridView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.GotFocus
        lblStatus.Visible = True
    End Sub


    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Escape Then txtAcGroupName__Man.Focus()
        If e.KeyCode = Keys.Enter Then
            If gridView.RowCount > 0 Then
                e.Handled = True
                funcGetDetails(gridView.Item("AcGrpCode", gridView.CurrentRow.Index).Value.ToString)
            End If
        End If
    End Sub

    Private Sub gridView_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.LostFocus

        lblStatus.Visible = False
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
        Dim chkQry As String = Nothing
        Dim delKey As String = gridView.Rows(gridView.CurrentRow.Index).Cells("ACGRPCODE").Value.ToString
        'chkQry = " SELECT 'CHECK' FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPCODE = '" & delKey & "' AND ISNULL(AUTOGENERATOR,'') = 'A'"
        'chkQry += " UNION ALL"
        chkQry = " SELECT 'CHECK' FROM " & cnAdminDb & "..ACHEAD WHERE ACGRPCODE = '" & delKey & "'"
        chkQry += " UNION ALL"
        chkQry += " SELECT 'CHECK' FROM " & cnAdminDb & "..ACGROUP "
        chkQry += " WHERE ACGRPCODE IN (SELECT ACGRPCODE FROM " & cnAdminDb & "..ACGROUP WHERE ACMAINCODE = '" & delKey & "' AND ACGRPCODE <> '" & delKey & "')"
        DeleteItem(SyncMode.Master, chkQry, "DELETE FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPCODE = '" & delKey & "' ")
        'DeleteItem(SyncMode.Master, chkQry, "DELETE FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPCODE = '" & delKey & "' AND ISNULL(AUTOGENERATOR,'') = ''")
        funcCallGrid()
        gridView.Select()
    End Sub

    Private Sub txtAcGroupName__Man_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtAcGroupName__Man.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If objGPack.DupChecker(txtAcGroupName__Man, "SELECT 1 FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPNAME = '" & txtAcGroupName__Man.Text & "' AND ACGRPCODE <> '" & upAcGrpCode & "'") Then
                Exit Sub
            Else
                SendKeys.Send("{TAB}")
            End If
        End If

    End Sub

    Private Sub cmbAcGroup_Man_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAcGroup_Man.GotFocus
        If txtAcGroupName__Man.Text <> "" Then
            If Not cmbAcGroup_Man.Contains(txtAcGroupName__Man) Then cmbAcGroup_Man.Items.Add(txtAcGroupName__Man.Text)
        End If
    End Sub

    Private Sub cmbAcGroup_Man_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAcGroup_Man.SelectedIndexChanged
        If cmbAcGroup_Man.Text <> "" Then
            strSql = " SELECT "
            strSql += " AcGrpCode, ACGRPNAME"
            strSql += " ,(SELECT TOP 1 ACGRPNAME FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPCODE = A.ACMAINCODE) AS ACMAINGROUP"
            strSql += " ,CASE WHEN GRPLEDGER = 'B' THEN 'BALANCE SHEET' "
            strSql += "       WHEN GRPLEDGER = 'P' THEN 'PROFIT & LOSS' ELSE 'TRADING' END AS GRPLEDGER"
            strSql += " ,CASE WHEN GRPTYPE = 'E' THEN 'EXPENDITURE' "
            strSql += "       WHEN GRPTYPE = 'I' THEN 'INCOME'"
            strSql += "       WHEN GRPTYPE = 'A' THEN 'ASSETS' ELSE 'LIABLITIES' END GRPTYPE"
            strSql += " ,GRPTYPE,SCHEDULECODE,DISPORDER"
            strSql += " FROM " & cnAdminDb & "..ACGROUP AS A WHERE ACGRPNAME = '" & cmbAcGroup_Man.Text & "'"
            Dim dtTemp As New DataTable
            dtTemp.Clear()
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtTemp)
            If dtTemp.Rows.Count > 0 Then
                cmbGroupLedger.Text = dtTemp.Rows(0).Item("GRPLEDGER").ToString
                cmbGroupType.Text = dtTemp.Rows(0).Item("GRPTYPE").ToString
            End If
            If StrComp(txtAcGroupName__Man.Text, cmbAcGroup_Man.Text, CompareMethod.Text) = 0 Then
                cmbGroupLedger.Enabled = True
                cmbGroupType.Enabled = True
            Else
                cmbGroupLedger.Enabled = False
                cmbGroupType.Enabled = False
            End If
        End If
    End Sub

    Private Sub txtAcGroupName__Man_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAcGroupName__Man.TextChanged
        If txtAcGroupName__Man.Text <> "" Then
            btnSave.Enabled = True
        Else
            btnSave.Enabled = False
        End If
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "HEAD OF ACCOUNTGROUP", gridView, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub
End Class