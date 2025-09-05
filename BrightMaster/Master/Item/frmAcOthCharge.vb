Imports System.Data.OleDb
Public Class frmAcOthCharge
    Dim da As OleDbDataAdapter
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim flagSave As Boolean = False
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
    Function funcCallGrid() As Integer
        strSql = " SELECT CH.ACCMISCID,CH.MISCID,CH.ACCODE,"
        strSql += " (SELECT MISCNAME FROM " & cnAdminDb & "..MISCCHARGES AS M WHERE M.MISCID = CH.MISCID)AS MISCNAME,"
        strSql += " (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD AS A WHERE A.ACCODE = CH.ACCODE)AS ACGRPNAME,"
        strSql += " FROMRANGE ,TORANGE , AMOUNT,"
        strSql += " CASE WHEN ACTIVE = 'Y' THEN 'YES' ELSE 'NO' END AS ACTIVE"
        strSql += " FROM " & cnAdminDb & "..ACC_MISCCHARGE AS CH"
        funcOpenGrid(strSql, gridView)
        With gridView
            .Columns("MISCID").Visible = False
            .Columns("ACCMISCID").Visible = False
            .Columns("MISCNAME").Width = 200
            .Columns("ACCODE").Visible = False
            ' .Columns("CATNAME").Width = 200
            .Columns("ACGRPNAME").HeaderText = "ACCNAME"
            .Columns("ACGRPNAME").Width = 150
            .Columns("AMOUNT").HeaderText = "VALUE"
            .Columns("AMOUNT").Width = 80
            .Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("FROMRANGE").HeaderText = " FROM "
            .Columns("FROMRANGE").Width = 80
            .Columns("FROMRANGE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("TORANGE").HeaderText = "TO"
            .Columns("TORANGE").Width = 80
            .Columns("TORANGE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            .Columns("ACTIVE").Width = 60
        End With
    End Function
    Function funcNew() As Integer
        Dim dt As New DataTable
        dt.Clear()
        objGPack.TextClear(Me)
        flagSave = False
        txtId.Text = objGPack.GetMax("ACCMiscId", "ACC_MiscCharge", cnAdminDb)
        funcLoadMiscName()
        funcLoadAcctName()
        cmbActive.Text = "YES"
        cmbMiscName_Man.Enabled = True
        cmbAcctName_Own.Enabled = True
        funcCallGrid()
        cmbMiscName_Man.Select()
    End Function
    Function funcExit() As Integer
        Me.Close()
    End Function
    Function funcSave() As Integer
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Function
        If objGPack.Validator_Check(Me) Then Exit Function
        Dim ds As New Data.DataSet
        ds.Clear()
        Dim AcctId As String = Nothing
        Dim Miscid As Integer = Nothing
        strSql = " select Miscid from " & cnAdminDb & "..MISCCHARGES where MISCNAME = '" & cmbMiscName_Man.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(ds, "Miscid")
        If ds.Tables("Miscid").Rows.Count > 0 Then
            Miscid = ds.Tables("Miscid").Rows(0).Item("Miscid")
        Else
            MsgBox("Misc. Charges Invalid ")
            Exit Function
        End If
        If cmbAcctName_Own.Text <> "" Then
            strSql = " select AcCode from " & cnAdminDb & "..AcHead where AcName = '" & cmbAcctName_Own.Text & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(ds, "AcCode")
            If ds.Tables("AcCode").Rows.Count > 0 Then
                AcctId = ds.Tables("AcCode").Rows(0).Item("AcCode")
            Else
                MsgBox("Account Name Invalid ")
                Exit Function
            End If
        Else
            If Not _IsWholeSaleType Then MsgBox("Account Name is blank") : Exit Function
        End If
        If flagSave = False Then
            If Val(objGPack.GetSqlValue("SELECT 1 FROM " & cnAdminDb & "..ACC_MISCCHARGE WHERE MISCID = " & Miscid & " AND ACCODE = '" & AcctId & "'").ToString) = 1 Then
                Exit Function
            End If

            funcAdd(Miscid, AcctId)
        ElseIf flagSave = True Then
            funcUpdate()
        End If
    End Function
    Function funcAdd(ByVal miscid As Integer, ByVal acctid As String) As Integer

        strSql = " Insert into " & cnAdminDb & "..ACC_MISCCHARGE"
        strSql += " ("
        strSql += " Accmiscid,MiscId, Accode,Amount,fromrange,torange,active,"
        strSql += " UserId,Updated,Uptime"
        strSql += " )Values("
        strSql += " " & Val(txtId.Text) & "" 'MiscId
        strSql += " ," & miscid & "" 'MiscId
        strSql += " ,'" & acctid & "'" 'AcctId
        strSql += " ," & Val(txtDefaultValue_Amt.Text) & "" 'DefaultValue
        strSql += " ," & Val(txtRangeFrom_WET.Text) & "" 'DefaultValue
        strSql += " ," & Val(txtRangeTo_WET.Text) & "" 'DefaultValue
        strSql += " ,'" & Mid(cmbActive.Text, 1, 1) & "'" 'active
        strSql += " ," & userId & "" 'UserId
        strSql += " ,'" & GetServerDate() & "'" 'Updated
        strSql += " ,'" & GetServerTime() & "'" 'Uptime
        strSql += " )"

        Try
            tran = cn.BeginTransaction
            ExecQuery(SyncMode.Master, strSql, cn, tran)
            tran.Commit()
            funcNew()
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try

    End Function
    Function funcUpdate() As Integer
        Dim ds As New Data.DataSet
        ds.Clear()

        strSql = " Update " & cnAdminDb & "..ACC_MISCCHARGE Set"
        strSql += " Amount=" & Val(txtDefaultValue_Amt.Text) & ""
        strSql += " ,FROMRANGE=" & Val(txtRangeFrom_WET.Text) & ""
        strSql += " ,TORANGE=" & Val(txtRangeTo_WET.Text) & ""
        strSql += " ,active='" & Mid(cmbActive.Text, 1, 1) & "'"
        strSql += " ,UserId=" & userId & ""
        strSql += " ,Updated='" & Today.Date.ToString("yyyy-MM-dd") & "'"
        strSql += " ,Uptime='" & Date.Now.ToLongTimeString & "'"
        strSql += " where AccMiscid=" & Val(txtId.Text)
        Try

            ExecQuery(SyncMode.Master, strSql, cn)

            funcNew()
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function
    Function funcGetDetails(ByVal tempstr As String) As Integer
        strSql = " Select ch.accmiscid,ch.miscid,ch.accode,"
        strSql += " (select MiscName from " & cnAdminDb & "..MiscCharges as m where m.Miscid= ch.Miscid)as Miscname,"
        strSql += " (select AcName from " & cnAdminDb & "..AcHead as a where a.AcCode = ch.Accode)as AcGrpName,"
        strSql += " Amount,FROMRANGE,TORANGE,"
        strSql += " case when active = 'Y' then 'YES' else 'NO' end as Active"
        strSql += " From " & cnAdminDb & "..ACC_MISCCHARGE as Ch"
        strSql += " where " & tempstr
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Return 0
        End If
        With dt.Rows(0)
            txtId.Text = .Item("AccMiscId").ToString
            cmbMiscName_Man.Text = .Item("MiscName").ToString
            cmbAcctName_Own.Text = .Item("Acgrpname").ToString
            cmbAcctName_Own.Enabled = False
            cmbMiscName_Man.Enabled = False
            txtDefaultValue_Amt.Text = .Item("AMOUNT").ToString
            txtRangeFrom_WET.Text = .Item("FROMRANGE").ToString
            txtRangeTo_WET.Text = .Item("TORANGE").ToString
            cmbActive.Text = .Item("active").ToString
        End With
        flagSave = True

    End Function

    Function funcLoadMiscName() As Integer
        strSql = " select MiscName from " & cnAdminDb & "..MISCCHARGES where isnull(active,'Y') <> 'N' "
        objGPack.FillCombo(strSql, cmbMiscName_Man)
    End Function
    Function funcLoadAcctName() As Integer
        cmbAcctName_Own.Items.Clear()

        strSql = " select AcName from " & cnAdminDb & "..AcHead where active = 'Y' and Actype in ('C','S','D','I')"
        objGPack.FillCombo(strSql, cmbAcctName_Own)
        'cmbAcctName_Man.Text = 0
    End Function

    Private Sub frmOtherCharges_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            'If txtMiscName__Man.Focused Then
            '    Exit Sub
            'End If
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmOtherCharges_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        funcGridStyle(gridView)
        cmbActive.Items.Add("YES")
        cmbActive.Items.Add("NO")
        cmbActive.Text = "YES"
        txtId.Visible = False
        funcNew()
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        funcSave()
    End Sub
    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click, OpenToolStripMenuItem.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        gridView.Focus()
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

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        funcNew()
    End Sub
    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        funcExit()
    End Sub
    Private Sub cmbAcctName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAcctName_Own.GotFocus
        If flagSave = True Then
            Exit Sub
        End If
    End Sub

    Private Sub gridView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.GotFocus
        lblStatus.Visible = True
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            gridView.CurrentCell = gridView.CurrentCell
            If gridView.RowCount > 0 Then
                Dim Strg As String = "ACCmiscid =" & Val(gridView.Item(0, gridView.CurrentRow.Index).Value.ToString)

                funcGetDetails(Strg)
            End If
        ElseIf e.KeyCode = Keys.Escape Then
            Me.SelectNextControl(grpField, True, True, True, True)
        End If
    End Sub


    Private Sub cmbAcctName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbAcctName_Own.KeyPress
        e.KeyChar = UCase(e.KeyChar)
    End Sub

    Private Sub gridView_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.LostFocus

        lblStatus.Visible = False
    End Sub

    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If Not gridView.RowCount > 0 Then
            Exit Sub
        End If
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
        Dim chkQry As String = Nothing
        Dim delKey As String
        delKey = gridView.Rows(gridView.CurrentRow.Index).Cells("ACCMISCID").Value.ToString
        chkQry = "DELETE FROM " & cnAdminDb & "..ACC_MISCCHARGE WHERE ACCMISCID = " & delKey
        DeleteItem(SyncMode.Master, chkQry, "DELETE FROM " & cnAdminDb & "..ACC_MISCCHARGE WHERE ACCMISCID = '" & delKey & "'")
        funcCallGrid()
    End Sub

    Private Sub cmbAcctName_Man_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAcctName_Own.SelectedIndexChanged

    End Sub
End Class