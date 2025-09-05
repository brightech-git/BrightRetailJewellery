Imports System.Data.OleDb
Public Class frmItemCounter
    Dim da As OleDbDataAdapter
    Dim strSql As String
    Dim flagSave As Boolean = False
    Dim cmd As OleDbCommand
    Dim CTRITEMTRF As String = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'ISCTRITEMTRF'", , , tran)
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
    End Sub
    Function funcCalGrid() As Integer
        Dim dt As New DataTable
        dt.Clear()
        strSql = " SELECT "
        strSql += " ITEMCTRID,ITEMCTRNAME,ITEMCTRSHNAME,"
        strSql += " CASE WHEN I.ACTIVE = 'Y' THEN 'YES' ELSE 'NO' END ACTIVE,"
        strSql += " IG.GROUPNAME AS CTRGROUP,TARGET,DISPLAYORDER DISPORDER,PCS,WEIGHT,TAGWT,COVERWT,POS_ITEMID"
        strSql += " ,(SELECT TOP 1 ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID=I.TRFITEMCTRID)TRFITEMCTRNAME"
        strSql += " FROM " & cnAdminDb & "..ITEMCOUNTER I"
        strSql += " LEFT JOIN " & cnAdminDb & "..ITEMGROUPMAST IG ON I.CTRGROUP=IG.GROUPID "
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        gridView.DataSource = dt
        gridView.Columns("ITEMCTRID").Visible = False
        gridView.Columns("ITEMCTRNAME").MinimumWidth = 230
        gridView.Columns("ITEMCTRSHNAME").MinimumWidth = 150
        For cnt As Integer = 0 To gridView.ColumnCount - 1
            gridView.Columns(cnt).HeaderText = UCase(gridView.Columns(cnt).HeaderText)
        Next
    End Function
    Function funcNew() As Integer
        funcClear()
        flagSave = False
        cmbActive.Text = "YES"
        Dim dt As New DataTable
        dt.Clear()
        strSql = " select isnull(max(ItemCtrId),0)+1 as ItemCtrId from " & cnAdminDb & "..ItemCounter"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            txtCounterId_Num_Man.Text = dt.Rows(0).Item("ItemCtrId")
        End If
        strSql = " SELECT GROUPNAME FROM " & cnAdminDb & "..ITEMGROUPMAST WHERE GROUPTYPE='C' ORDER BY GROUPNAME"
        objGPack.FillCombo(strSql, CmbCounterGrp, True, False)

        strSql = " SELECT '' ITEMCTRNAME UNION ALL SELECT DISTINCT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER ORDER BY ITEMCTRNAME"
        objGPack.FillCombo(strSql, cmbTrfCounter, True, False)

        funcCalGrid()
        txtCounterName__Man.Select()
    End Function
    Function funcClear() As Integer
        txtCounterId_Num_Man.Clear()
        txtTarget_Amt.Clear()
        txtCounterGroup.Clear()
        txtCounterName__Man.Clear()
        txtCounterSHName.Clear()
        txtDisplayOrder_NUM.Clear()
        txtPiece_NUM.Clear()
        txtWeight_WET.Clear()
        txtTagWt_WET.Clear()
        txtCoverWt_WET.Clear()
        txt_POS_ITEMID.Clear()
    End Function
    Function funcSave() As Integer
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Function
        If objGPack.Validator_Check(Me) Then Exit Function
        If objGPack.DupChecker(txtCounterName__Man, "SELECT 1 FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & txtCounterName__Man.Text & "' AND ITEMCTRID <> '" & txtCounterId_Num_Man.Text & "'") Then
            Exit Function
        End If
        If VAL(txtDisplayOrder_NUM.Text) <> 0 Then
            If objGPack.DupChecker(txtDisplayOrder_NUM, "SELECT 1 FROM " & cnAdminDb & "..ITEMCOUNTER WHERE DISPLAYORDER = " & Val(txtDisplayOrder_NUM.Text) & " AND ITEMCTRID <> '" & txtCounterId_Num_Man.Text & "'") Then
                Exit Function
            End If
        End If
        If flagSave = False Then
            funcAdd()
        ElseIf flagSave = True Then
            funcUpdate()
        End If
    End Function
    Function funcAdd() As Integer
        Dim ctrgroup As String = ""
        ctrgroup = GetSqlValue(cn, "SELECT GROUPID FROM " & cnAdminDb & "..ITEMGROUPMAST WHERE GROUPNAME='" & CmbCounterGrp.Text & "' AND GROUPTYPE='C'")
        Dim trfItemCtrId As String = ""
        trfItemCtrId = GetSqlValue(cn, "SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME='" & cmbTrfCounter.Text.ToString() & "'")
        strSql = " INSERT INTO " & cnAdminDb & "..ITEMCOUNTER"
        strSql += " ("
        strSql += " ITEMCTRID,ITEMCTRNAME,ITEMCTRSHNAME,ACTIVE,CTRGROUP,"
        strSql += " TARGET,USERID,UPDATED,UPTIME"
        strSql += " ,DISPLAYORDER,PCS,WEIGHT,TAGWT,COVERWT,POS_ITEMID,TRFITEMCTRID"
        strSql += " )VALUES ("
        strSql += " '" & txtCounterId_Num_Man.Text & "'" 'ItemCtrId
        strSql += " ,'" & txtCounterName__Man.Text & "'" 'ItemCtrName
        strSql += " ,'" & txtCounterSHName.Text & "'" 'ITEMCTRSHNAME
        strSql += " ,'" & Mid(cmbActive.Text, 1, 1) & "'" 'Active
        strSql += " ,'" & ctrgroup & "'" 'ctrgroup
        strSql += " ,'" & Val(txtTarget_Amt.Text) & "'" 'Target
        strSql += " ,'" & userId & "'" 'UserId
        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'Updated
        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'Uptime
        strSql += " ," & Val(txtDisplayOrder_NUM.Text) & "" 'DISPLAYORDER
        strSql += " ," & Val(txtPiece_NUM.Text) & "" 'pcs
        strSql += " ," & Val(txtWeight_WET.Text) & "" 'weight
        strSql += " ," & Val(txtTagWt_WET.Text) & "" 'TAGWT
        strSql += " ," & Val(txtCoverWt_WET.Text) & "" 'COVERWT
        strSql += " ,'" & txt_POS_ITEMID.Text & "'" 'weight
        strSql += " ,'" & trfItemCtrId & "')" 'weight
        Try
            ExecQuery(SyncMode.Master, strSql, cn)

            funcNew()
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function
    Function funcUpdate() As Integer
        Dim ctrgroup As String = ""
        ctrgroup = GetSqlValue(cn, "SELECT GROUPID FROM " & cnAdminDb & "..ITEMGROUPMAST WHERE GROUPNAME='" & CmbCounterGrp.Text & "' AND GROUPTYPE='C'")
        Dim trfItemCtrId As String = ""
        trfItemCtrId = GetSqlValue(cn, "SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME='" & cmbTrfCounter.Text.ToString() & "'")
        strSql = " Update " & cnAdminDb & "..ItemCounter Set "
        strSql += " ItemCtrName='" & txtCounterName__Man.Text & "'"
        strSql += " ,ITEMCTRSHNAME='" & txtCounterSHName.Text & "'"
        strSql += " ,Active='" & Mid(cmbActive.Text, 1, 1) & "'"
        strSql += " ,ctrgroup='" & ctrgroup & "'"
        strSql += " ,Target='" & Val(txtTarget_Amt.Text) & "'"
        strSql += " ,UserId='" & userId & "'"
        strSql += " ,Updated='" & Today.Date.ToString("yyyy-MM-dd") & "'"
        strSql += " ,Uptime='" & Date.Now.ToLongTimeString & "'"
        strSql += " ,DISPLAYORDER = " & Val(txtDisplayOrder_NUM.Text) & ""
        strSql += " ,PCS = " & Val(txtPiece_NUM.Text) & ""
        strSql += " ,WEIGHT = " & Val(txtWeight_WET.Text) & ""
        strSql += " ,TAGWT = " & Val(txtTagWt_WET.Text) & ""
        strSql += " ,COVERWT = " & Val(txtCoverWt_WET.Text) & ""
        strSql += " ,POS_ITEMID = '" & txt_POS_ITEMID.Text & "'"
        strSql += " ,TRFITEMCTRID = '" & trfItemCtrId & "'"
        strSql += " Where ItemCtrId = '" & txtCounterId_Num_Man.Text & "'"
        Try
            ExecQuery(SyncMode.Master, strSql, cn)
            funcNew()
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function
    Function funcExit() As Integer
        Me.Close()
    End Function
    Function funcGetDetails(ByVal tempCtrId As Integer) As Integer
        Dim dt As New DataTable
        dt.Clear()

        strSql = " SELECT "
        strSql += " ITEMCTRID,ITEMCTRNAME,ISNULL(ITEMCTRSHNAME,'')ITEMCTRSHNAME,"
        strSql += " CASE WHEN I.ACTIVE = 'Y' THEN 'YES' ELSE 'NO' END ACTIVE,"
        strSql += " IG.GROUPNAME AS CTRGROUP,TARGET,DISPLAYORDER,PCS,WEIGHT,TAGWT,COVERWT,POS_ITEMID"
        strSql += " ,(SELECT TOP 1 ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID=I.TRFITEMCTRID)TRFITEMCTRNAME"
        strSql += " FROM " & cnAdminDb & "..ITEMCOUNTER I"
        strSql += " LEFT JOIN " & cnAdminDb & "..ITEMGROUPMAST IG ON I.CTRGROUP=IG.GROUPID "
        strSql += " WHERE I.ITEMCTRID = '" & tempCtrId & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Return 0
        End If
        With dt.Rows(0)
            txtCounterId_Num_Man.Text = .Item("ItemCtrId")
            txtCounterName__Man.Text = .Item("ItemCtrName").ToString
            txtCounterSHName.Text = .Item("ITEMCTRSHNAME").ToString
            cmbActive.Text = .Item("Active")
            CmbCounterGrp.Text = IIf(IsDBNull(.Item("ctrgroup")), "", .Item("ctrgroup"))
            txtTarget_Amt.Text = .Item("Target")
            txtDisplayOrder_NUM.Text = IIf(Val(.Item("DISPLAYORDER").ToString) <> 0, .Item("DISPLAYORDER").ToString, "")
            txtPiece_NUM.Text = .Item("PCS").ToString
            txtWeight_WET.Text = .Item("WEIGHT").ToString
            txtTagWt_WET.Text = .Item("TAGWT").ToString
            txtCoverWt_WET.Text = .Item("COVERWT").ToString
            txt_POS_ITEMID.Text = .Item("POS_ITEMID").ToString
            cmbTrfCounter.Text = .Item("TRFITEMCTRNAME").ToString
        End With
        flagSave = True
    End Function

    Private Sub frmItemCounter_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtCounterName__Man.Focused Then
                Exit Sub
            End If
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub frmItemCounter_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        funcGridStyle(gridView)
        txtCounterId_Num_Man.Enabled = False
        cmbActive.Items.Add("YES")
        cmbActive.Items.Add("NO")
        cmbActive.Text = "YES"
        If CTRITEMTRF = "Y" Then
            txt_POS_ITEMID.Visible = True
            lblctr.Visible = True
            lblctr1.Visible = True
        End If
        strSql = "SELECT COUNT(*)CNT FROM " & cnAdminDb & "..ITEMMAST WHERE (TAGWT='Y' OR COVERWT='Y')"
        If Val(objGPack.GetSqlValue(strSql, "CNT", 0).ToString) > 0 Then
            pnlExtra.Visible = True
        Else
            pnlExtra.Visible = False
        End If
        funcNew()
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


    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        funcSave()
    End Sub

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click, OpenToolStripMenuItem.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        funcCalGrid()
        gridView.Focus()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        funcNew()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        funcExit()
    End Sub

    Private Sub gridView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.GotFocus
        lblStatus.Visible = True
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            gridView.CurrentCell = gridView.CurrentCell
            If gridView.RowCount > 0 Then
                funcGetDetails(gridView.Item(0, gridView.CurrentRow.Index).Value.ToString)
                txtCounterName__Man.Focus()
            End If
        ElseIf e.KeyCode = Keys.Escape Then
            txtCounterName__Man.Focus()
        End If
    End Sub

    Private Sub txtCounterName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCounterName__Man.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If objGPack.DupChecker(txtCounterName__Man, "SELECT 1 FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & txtCounterName__Man.Text & "' AND ITEMCTRID <> '" & txtCounterId_Num_Man.Text & "'") Then
                Exit Sub
            Else
                SendKeys.Send("{TAB}")
            End If
        End If
    End Sub

    Private Sub gridView_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.LostFocus
        lblStatus.Visible = False

    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If Not gridView.RowCount > 0 Then
            Exit Sub
        End If
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
        Dim chkQry As String = Nothing
        Dim delKey As String = gridView.Rows(gridView.CurrentRow.Index).Cells("ITEMCTRID").Value.ToString

        strSql = " SELECT DBNAME FROM " & cnAdminDb & "..DBMASTER  WHERE DBNAME IN(SELECT NAME FROM MASTER..SYSDATABASES)"
        Dim dtDb As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtDb)
        chkQry = " SELECT TOP 1 DEFAULTCOUNTER FROM " & cnAdminDb & "..ITEMMAST WHERE DEFAULTCOUNTER = '" & delKey & "'"
        chkQry += " UNION "
        chkQry += " SELECT TOP 1 ITEMCTRID FROM " & cnAdminDb & "..ITEMLOT WHERE ITEMCTRID = '" & delKey & "'"
        chkQry += " UNION "
        chkQry += " SELECT TOP 1 ITEMCTRID FROM " & cnAdminDb & "..ITEMTAG WHERE ITEMCTRID = '" & delKey & "'"
        chkQry += " UNION "
        chkQry += " SELECT TOP 1 ITEMCTRID FROM " & cnAdminDb & "..ITEMNONTAG WHERE ITEMCTRID = '" & delKey & "'"
        For cnt As Integer = 0 To dtDb.Rows.Count - 1
            With dtDb.Rows(cnt)
                chkQry += " UNION "
                chkQry += " SELECT TOP 1 ITEMCTRID FROM " & .Item("DBNAME").ToString & "..ISSUE WHERE ITEMCTRID = '" & delKey & "'"
                chkQry += " UNION "
                chkQry += " SELECT TOP 1 ITEMCTRID FROM " & .Item("DBNAME").ToString & "..RECEIPT WHERE ITEMCTRID = '" & delKey & "'"
                chkQry += " UNION "
                chkQry += " SELECT TOP 1 ITEMCTRID FROM " & .Item("DBNAME").ToString & "..OPENITEM WHERE ITEMCTRID = '" & delKey & "'"
                chkQry += " UNION "
                chkQry += " SELECT TOP 1 ITEMCTRID FROM " & .Item("DBNAME").ToString & "..ESTISSUE WHERE ITEMCTRID = '" & delKey & "'"
                chkQry += " UNION "
                chkQry += " SELECT TOP 1 ITEMCTRID FROM " & .Item("DBNAME").ToString & "..ESTRECEIPT WHERE ITEMCTRID = '" & delKey & "'"
            End With
        Next
        DeleteItem(SyncMode.Master, chkQry, "DELETE FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = '" & delKey & "'")
        funcCalGrid()
    End Sub
End Class