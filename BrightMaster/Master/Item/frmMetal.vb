Imports System.Data.OleDb
Public Class frmMetal
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim flagSave As Boolean = False
    Dim metalId As String = Nothing
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
    End Sub
    Function funcCallGrid() As Integer
        Dim dt As New DataTable
        dt.Clear()

        strSql = " SELECT "
        strSql += " METALID AS METALID,METALNAME METALNAME"
        strSql += " ,CASE WHEN TTYPE = 'M' THEN 'METAL' WHEN TTYPE = 'A' THEN 'ALLOY' ELSE 'STONE' END AS TYPE"
        strSql += " ,DISPLAYORDER,CASE WHEN ACTIVE = 'Y' THEN 'YES' ELSE 'NO' END AS ACTIVE"
        strSql += " FROM " & cnAdminDb & "..METALMAST ORDER BY DISPLAYORDER,METALNAME"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        gridView.DataSource = dt
        gridView.Columns("METALID").HeaderText = "ID"
        gridView.Columns("METALID").Width = 40
        gridView.Columns("METALNAME").MinimumWidth = 150
        gridView.Columns("TYPE").Width = 70
        gridView.Columns("DISPLAYORDER").HeaderText = "ORDER"
        gridView.Columns("DISPLAYORDER").Width = 50
        gridView.Focus()
        gridView.Select()
    End Function
    Function funcNew() As Integer
        objGPack.TextClear(Me)
        metalId = Nothing
        txtMetalId__Man.Enabled = True
        flagSave = False
        cmbType.Text = "METAL"
        cmbActive.Items.Clear()
        cmbActive.Items.Add("YES")
        cmbActive.Items.Add("NO")
        cmbActive.Text = "YES"
        funcCallGrid()
        txtMetalId__Man.Select()
    End Function
    Function funcExit() As Integer
        Me.Close()
    End Function
    Function funcSave() As Integer
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Function
        If objGPack.Validator_Check(Me) Then Exit Function
        If objGPack.DupChecker(txtMetalName__Man, "SELECT 1 FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & txtMetalName__Man.Text & "' AND METALID <> '" & txtMetalId__Man.Text & "'") Then Exit Function
        If flagSave = False Then
            funcAdd()
        ElseIf flagSave = True Then
            funcUpdate()
        End If
    End Function
    Function funcAdd() As Integer
        Try
            strSql = " INSERT INTO " & cnAdminDb & "..METALMAST"
            strSql += " ("
            strSql += " METALID"
            strSql += " ,METALNAME"
            strSql += " ,USERID"
            strSql += " ,UPDATED"
            strSql += " ,UPTIME"
            strSql += " ,TTYPE"
            strSql += " ,DISPLAYORDER,ACTIVE"
            strSql += " )VALUES ("
            strSql += " '" & txtMetalId__Man.Text & "'"
            strSql += " ,'" & txtMetalName__Man.Text & "'"
            strSql += " ,'" & userId & "'"
            strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'"
            strSql += " ,'" & Date.Now.ToLongTimeString & "'"
            strSql += " ,'" & Mid(cmbType.Text, 1, 1) & "'"
            strSql += " ," & Val(txtDispOrder_Num.Text) & ""
            strSql += " ,'" & Mid(cmbActive.Text, 1, 1) & "'"
            strSql += " )"
            ExecQuery(SyncMode.Master, strSql, cn)
            InsertIntoBillControl("MET-" + txtMetalId__Man.Text + "-SAL", txtMetalName__Man.Text + " SALES BILLNO", "N", "N", "", "P", tran)
            InsertIntoBillControl("MET-" + txtMetalId__Man.Text + "-PUR", txtMetalName__Man.Text + " PURCHASE BILLNO", "N", "N", "", "P", tran)
            funcNew()
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function
    Function funcUpdate()
        Dim dt As New DataTable
        dt.Clear()
        strSql = " Update " & cnAdminDb & "..MetalMast Set"
        strSql += " MetalID= '" & txtMetalId__Man.Text & "'"
        strSql += " ,Metalname= '" & txtMetalName__Man.Text & "'"
        strSql += " ,UserId= '" & userId & "'"
        strSql += " ,Updated=  '" & Today.Date.ToString("yyyy-MM-dd") & "'"
        strSql += " ,Uptime= '" & Date.Now.ToLongTimeString & "'"
        strSql += " ,tTYPE = '" & Mid(cmbType.Text, 1, 1) & "'"
        strSql += " ,DISPLAYORDER = " & Val(txtDispOrder_Num.Text) & ""
        strSql += " ,ACTIVE = '" & Mid(cmbActive.Text, 1, 1) & "'"
        strSql += " where MetalId = '" & metalId & "'"
        Try
            ExecQuery(SyncMode.Master, strSql, cn)

            funcNew()
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
        Return 0
    End Function
    Function funcOpen() As Integer
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Function
        funcCallGrid()
        gridView.Focus()
    End Function
    Function funcGetDetails(ByVal tempMetalId As String)
        Dim dt As New DataTable
        dt.Clear()
        strSql = " SELECT "
        strSql += " METALID,METALNAME,"
        strSql += " USERID,UPDATED,UPTIME"
        strSql += " ,CASE WHEN TTYPE = 'M' THEN 'METAL' WHEN TTYPE = 'A' THEN 'ALLOY' ELSE 'STONE' END AS TTYPE"
        strSql += " ,DISPLAYORDER,CASE WHEN ACTIVE = 'Y' THEN 'YES' ELSE 'NO' END AS ACTIVE"
        strSql += " FROM " & cnAdminDb & "..METALMAST WHERE METALID = '" & tempMetalId & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Return 0
        End If
        With dt.Rows(0)
            txtMetalId__Man.Text = .Item("METALID")
            txtMetalName__Man.Text = .Item("METALNAME")
            cmbType.Text = .Item("TTYPE").ToString
            txtDispOrder_Num.Text = .Item("DISPLAYORDER").ToString
            cmbActive.Text = .Item("ACTIVE").ToString
        End With
        flagSave = True
        txtMetalId__Man.Enabled = False
        metalId = tempMetalId

        Return 0
    End Function

    Private Sub frmMetal_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtMetalName__Man.Focused Then
                Exit Sub
            End If
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub frmMetal_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        funcGridStyle(gridView)
        cmbType.Items.Add("METAL")
        cmbType.Items.Add("STONE")
        cmbType.Items.Add("ALLOY")
        funcNew()
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

    Private Sub gridView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.GotFocus
        lblStatus.Visible = True
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            If gridView.RowCount > 0 Then
                gridView.CurrentCell = gridView.CurrentCell
                funcGetDetails(gridView.Item(0, gridView.CurrentRow.Index).Value.ToString)
                Me.SelectNextControl(Me, True, True, True, True)
            End If
        ElseIf e.KeyCode = Keys.Escape Then
            Me.SelectNextControl(Me, True, True, True, True)
        End If
    End Sub

    Private Sub txtMetalName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtMetalName__Man.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If objGPack.DupChecker(txtMetalName__Man, "SELECT 1 FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & txtMetalName__Man.Text & "' AND METALID <> '" & txtMetalId__Man.Text & "'") Then
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
        Dim delKey As String = gridView.Rows(gridView.CurrentRow.Index).Cells("METALID").Value.ToString
        Dim chkQry As String = Nothing
        chkQry = " SELECT TOP 1 METALID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID = '" & delKey & "'"
        chkQry += " UNION"
        chkQry += " SELECT TOP 1 METALID FROM " & cnAdminDb & "..PURITYMAST WHERE METALID = '" & delKey & "'"
        chkQry += " UNION"
        chkQry += " SELECT TOP 1 METALID FROM " & cnAdminDb & "..CATEGORY WHERE METALID = '" & delKey & "'"
        chkQry += " UNION"
        chkQry += " SELECT TOP 1 ALLOYID FROM " & cnStockDb & "..ALLOYDETAILS WHERE ALLOYID = '" & delKey & "'"
        DeleteItem(SyncMode.Master, chkQry, "DELETE FROM " & cnAdminDb & "..METALMAST WHERE METALID = '" & delKey & "' AND ISNULL(AUTOGENERATOR,'') = ''")
        funcCallGrid()
    End Sub

    Private Sub txtMetalId__Man_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtMetalId__Man.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If objGPack.DupChecker(txtMetalId__Man, "SELECT 1 FROM " & cnAdminDb & "..METALMAST WHERE METALID = '" & txtMetalId__Man.Text & "'") Then Exit Sub
        End If
    End Sub
End Class