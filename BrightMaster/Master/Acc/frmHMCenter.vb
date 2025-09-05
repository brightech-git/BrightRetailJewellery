Imports System.Data.OleDb
Public Class frmHMCenter
    Dim da As OleDbDataAdapter
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim hallMarkId As Integer = Nothing ''Update Purpose
    Dim flagSave As Boolean = False
    Dim HideAccLink As Boolean = IIf(GetAdmindbSoftValue("HIDE_ACHARI_ACCLINK", "Y") = "Y", True, False)

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        If HideAccLink Then
            lblAcName.Visible = False
            cmbAcName.Visible = False
        Else
            lblAcName.Visible = True
            cmbAcName.Visible = True
            strSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACTYPE IN ('G','D') order by acname"
            cmbAcName.Items.Clear()
            objGPack.FillCombo(strSql, cmbAcName)
        End If
    End Sub
    Function funcCallGrid() As Integer
        Dim dt As New DataTable
        dt.Clear()
        strSql = " SELECT DE.HALLMARKID,"
        strSql += " DE.HALLMARKNAME ""HALLMARK NAME"",DE.SEAL,"
        strSql += " CASE WHEN DE.ACTIVE = 'Y' THEN 'YES' ELSE 'NO' END AS ACTIVE"
        strSql += " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = DE.ACCODE)AS ACNAME"
        strSql += " FROM " & cnAdminDb & "..HALLMARK DE"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        gridView.DataSource = dt
        gridView.Columns("ACNAME").Width = 200
        gridView.Columns("ACNAME").Visible = Not HideAccLink
        gridView.Columns("HALLMARKID").Visible = False
        gridView.Columns("HALLMARK NAME").Width = 300
        gridView.Columns("SEAL").Width = 60
        gridView.Columns("ACTIVE").Width = 60
    End Function
    Function funcNew()
        objGPack.TextClear(Me)
        hallMarkId = Nothing
        flagSave = False
        cmbActive.Text = "YES"
        funcCallGrid()
        txtHMCName.Select()
        Return 0
    End Function
    Function funcExit() As Integer
        Me.Close()
    End Function
    Function funcSave() As Integer
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Function
        If objGPack.Validator_Check(Me) Then Exit Function
        If CheckAcNameAvail() Then Exit Function
        If flagSave = False Then
            funcAdd()
        ElseIf flagSave = True Then
            funcUpdate()
        End If
    End Function
    Function funcAdd()
        Dim dt As New DataTable
        dt.Clear()
        Dim designerId As Integer = Nothing
        Dim tran As OleDbTransaction = Nothing
        Try
            tran = cn.BeginTransaction()
            strSql = " SELECT ISNULL(MAX(HALLMARKID),0)+1 AS HALLMARKID FROM "
            strSql += " " & cnAdminDb & "..HALLMARK"
            designerId = Val(objGPack.GetSqlValue(strSql, , , tran))

            strSql = " INSERT INTO " & cnAdminDb & "..HALLMARK"
            strSql += " ("
            strSql += " HALLMARKID,"
            strSql += " HALLMARKNAME,SEAL,"
            strSql += " ACTIVE,"
            strSql += " USERID,"
            strSql += " UPDATED,"
            strSql += " UPTIME,ACCODE"
            strSql += " )VALUES ("
            strSql += " " & designerId & ""
            strSql += " ,'" & txtHMCName.Text & "'"
            strSql += " ,'" & txtHMSeal.Text & "'"
            strSql += " ,'" & Mid(cmbActive.Text, 1, 1) & "'"
            strSql += " ," & userId & ""
            strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'"
            strSql += " ,'" & Date.Now.ToLongTimeString & "'"
            If cmbAcName.Visible Then 'ACCODE
                strSql += " ,'" & objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbAcName.Text & "'", , , tran) & "'"
            Else
                strSql += " ,''"
            End If
            strSql += " )"
            ExecQuery(SyncMode.Master, strSql, cn, tran)
            tran.Commit()
            funcNew()
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
        Return 0
    End Function
    Function funcUpdate() As Integer
        Dim tran As OleDbTransaction = Nothing
        Try
            tran = cn.BeginTransaction()
            strSql = "UPDATE " & cnAdminDb & "..HALLMARK SET"
            strSql += " HALLMARKNAME = '" & txtHMCName.Text & "'"
            strSql += " ,SEAL = '" & txtHMSeal.Text & "'"
            strSql += " ,ACTIVE = '" & Mid(cmbActive.Text, 1, 1) & "'"
            strSql += " ,USERID = " & userId & ""
            strSql += " ,UPDATED = '" & Today.Date.ToString("yyyy-MM-dd") & "'"
            strSql += " ,UPTIME = '" & Date.Now & "'"
            strSql += " ,ACCODE = '" & objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbAcName.Text & "'", , , tran) & "'"
            strSql += " WHERE HALLMARKID = " & hallMarkId & ""
            ExecQuery(SyncMode.Master, strSql, cn, tran)
            tran.Commit()
            funcNew()
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function
    Function funcGetDetails(ByVal tempId As Integer)
        Dim dt As New DataTable
        dt.Clear()
        strSql = " select HALLMARKName,SEAL,"
        strSql += " case when Active = 'Y' then 'YES' else 'NO' end as Active"
        strSql += " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = DE.ACCODE)AS ACNAME"
        strSql += " from " & cnAdminDb & "..HALLMARK de where HALLMARKId = " & tempId & ""
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Return 0
        End If
        With dt.Rows(0)
            txtHMCName.Text = .Item("HALLMARKName").ToString
            txtHMSeal.Text = .Item("SEAL").ToString
            cmbActive.Text = .Item("Active").ToString
            If cmbAcName.Visible Then cmbAcName.Text = .Item("ACNAME").ToString
        End With
        hallMarkId = tempId
        Return 0
    End Function

    Private Sub frmDisigner_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtHMCName.Focused Then
                Exit Sub
            End If
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub frmDisigner_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        funcGridStyle(gridView)
        cmbActive.Items.Add("YES")
        cmbActive.Items.Add("NO")
        cmbActive.Text = "YES"
        btnNew_Click(Me, New EventArgs)
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
        funcCallGrid()
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
        If e.KeyCode = Keys.Escape Then
            txtHMCName.Focus()
        ElseIf e.KeyCode = Keys.Enter Then
            e.Handled = True
            gridView.CurrentCell = gridView.CurrentCell
            If gridView.RowCount > 0 Then
                funcGetDetails(gridView.Item(0, gridView.CurrentRow.Index).Value.ToString)
                flagSave = True
                txtHMCName.Focus()
            End If
        End If
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
        Dim delKey As String = gridView.Rows(gridView.CurrentRow.Index).Cells("HALLMARKID").Value.ToString
        chkQry += " SELECT TOP 1 HALLMARKID  FROM " & cnAdminDb & "..WMCTABLE"
        chkQry += " WHERE HALLMARKID = '" & delKey & "'"
        chkQry += " UNION"
        chkQry += " SELECT TOP 1 HALLMARKID  FROM " & cnAdminDb & "..ITEMLOT"
        chkQry += " WHERE HALLMARKID = '" & delKey & "'"
        chkQry += " UNION"
        chkQry += " SELECT TOP 1 HALLMARKID  FROM " & cnAdminDb & "..ITEMTAG"
        chkQry += " WHERE HALLMARKID = '" & delKey & "'"
        chkQry += " UNION"
        chkQry += " SELECT TOP 1 HALLMARKID  FROM " & cnAdminDb & "..ITEMNONTAG"
        chkQry += " WHERE HALLMARKID = '" & delKey & "'"
        chkQry += " UNION"
        chkQry += " SELECT TOP 1 HALLMARKID  FROM " & cnAdminDb & "..ORIRDETAIL"
        chkQry += " WHERE HALLMARKID = '" & delKey & "'"
        DeleteItem(SyncMode.Master, chkQry, "DELETE FROM " & cnAdminDb & "..HALLMARK WHERE HALLMARKID = '" & delKey & "'")
        funcCallGrid()
    End Sub

    Private Sub txtDesignerName__Man_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtHMCName.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If objGPack.DupChecker(txtHMCName, "SELECT 1 FROM " & cnAdminDb & "..HALLMARK WHERE HALLMARKNAME = '" & txtHMCName.Text & "' AND HALLMARKID <> '" & hallMarkId & "'") Then
                Exit Sub
            Else
                SendKeys.Send("{TAB}")
            End If
        End If
    End Sub

    Private Sub txtSeal__Man_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtHMSeal.GotFocus
        If txtHMSeal.Text = "" Then txtHMSeal.Text = Mid(txtHMCName.Text, 1, 5)
    End Sub

    Private Function CheckAcNameAvail() As Boolean
        If cmbAcName.Visible = False Then Return False
        Dim Accode As String = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbAcName.Text & "'")
        If Accode = "" Then
            MsgBox("Invalid AcName", MsgBoxStyle.Information)
            Return True
        End If
        strSql = " SELECT 'Y' FROM " & cnAdminDb & "..HALLMARK AS DE WHERE ACCODE = '" & Accode & "'"
        strSql += " AND HALLMARKID <> " & hallMarkId & ""
        If objGPack.GetSqlValue(strSql) = "Y" Then
            MsgBox("Already Exists", MsgBoxStyle.Information)
            cmbAcName.Focus()
            Return True
        End If
        Return False
    End Function

    Private Sub cmbAcName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbAcName.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            CheckAcNameAvail()
        End If
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "HEAD OF HALLMARK", gridView, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub
End Class