Imports System.Data.OleDb
Public Class frmPurityMaster
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim strSql As String = Nothing
    Dim PurityId As String = Nothing
    Dim flagSave As Boolean = False
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
    End Sub
    Function funcCallGrid() As Integer
        Dim dt As New DataTable
        dt.Clear()
        strSql = " select "
        strSql += " PURITYID,PURITYNAME,SHORTNAME,"
        strSql += " PURITY,"
        strSql += " (Select MetalName from " & cnAdminDb & "..MetalMast where MetalId = P.MetalId)as METALNAME,"
        strSql += " case when metaltype = 'M' then 'METAL' else 'ORNAMENT' end as METALTYPE"
        strSql += " from " & cnAdminDb & "..PurityMast as P"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        gridView.DataSource = dt
        gridView.Columns("PURITYID").Visible = False
        gridView.Columns("PURITYNAME").MinimumWidth = 250
        gridView.Focus()
        gridView.Select()
    End Function
    Function funcLoadMetalName()
        Dim dt As New DataTable
        dt.Clear()
        cmbMetalName_Man.Items.Clear()
        funcClear()
        strSql = " Select MetalName from " & cnAdminDb & "..MetalMast Order by displayorder,MetalName"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            Dim cnt As Integer
            For cnt = 0 To dt.Rows.Count - 1
                cmbMetalName_Man.Items.Add(dt.Rows(cnt).Item("MetalName"))
            Next
            cmbMetalName_Man.Text = dt.Rows(0).Item("MetalName")
        End If
        Return 0
    End Function
    Function funcNew() As Integer
        funcClear()
        Dim dt As New DataTable
        dt.Clear()
        strSql = " Select isnull(Max(PurityId),0) +1 PurityId from " & cnAdminDb & "..PurityMast WHERE ISNUMERIC(PurityId) = 1 "
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            PurityId = funcSetNumberStyle(dt.Rows(0).Item("PurityId").ToString, 2)
        End If
        cmbMetalType.Text = "METAL"
        flagSave = False
        funcCallGrid()
        cmbMetalName_Man.Select()
    End Function
    Function funcClear() As Integer
        PurityId = Nothing
        txtPurityName__Man.Clear()
        txtShortName.Clear()
        txtPurity_Per_Man.Clear()
        txtRatePurity_Amt.Clear()
        Return 0
    End Function
    Function funcExit() As Integer
        Me.Close()
    End Function
    Function funcSave() As Integer
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Function
        If objGPack.Validator_Check(Me) Then Exit Function
        If objGPack.DupChecker(txtPurityName__Man, "SELECT 1 FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYNAME = '" & txtPurityName__Man.Text & "' AND PURITYID <> '" & PurityId & "'") Then
            Exit Function
        End If
        If flagSave = False Then
            funcAdd()
        ElseIf flagSave = True Then
            funcUpdate()
        End If
    End Function
    Function funcAdd() As Integer
        Dim dt As New DataTable
        dt.Clear()
        Dim Id As String = Nothing
        strSql = " Select MetalId from " & cnAdminDb & "..MetalMast where MetalName = '" & cmbMetalName_Man.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            Id = dt.Rows(0).Item("MetalId")
        End If

        strSql = " insert into " & cnAdminDb & "..PurityMast"
        strSql += " ("
        strSql += " PurityID,"
        strSql += " PurityName,"
        strSql += " Shortname,"
        strSql += " Purity,"
        strSql += " RatePurity,"
        strSql += " metalID,"
        strSql += " metaltype,"
        strSql += " UserId,"
        strSql += " Updated,"
        strSql += " Uptime"
        strSql += " )Values("
        strSql += " '" & PurityId & "'"
        strSql += " ,'" & txtPurityName__Man.Text & "'"
        strSql += " ,'" & txtShortName.Text & "'"
        strSql += " ,'" & txtPurity_Per_Man.Text & "'"
        strSql += " ,'" & Val(txtRatePurity_Amt.Text) & "'"
        strSql += " ,'" & Id & "'"
        strSql += " ,'" & Mid(cmbMetalType.Text, 1, 1) & "'"
        strSql += " ,'" & userId & "'"
        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'"
        strSql += " ,'" & Date.Now.ToLongTimeString & "'"
        strSql += " )"
        Try
            ExecQuery(SyncMode.Master, strSql, cn)
            funcNew()
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function
    Function funcUpdate() As Integer
        Dim ds As New Data.DataSet
        ds.Clear()
        Dim Id As String = Nothing
        strSql = " Select MetalId from " & cnAdminDb & "..MetalMast where MetalName = '" & cmbMetalName_Man.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(ds, "MetalId")
        If ds.Tables("MetalId").Rows.Count > 0 Then
            Id = ds.Tables("Metalid").Rows(0).Item("MetalId")
        End If
        strSql = " Update " & cnAdminDb & "..PurityMast Set"
        strSql += " PurityID = '" & PurityId & "'"
        strSql += " ,PurityName = '" & txtPurityName__Man.Text & "'"
        strSql += " ,Shortname = '" & txtShortName.Text & "'"
        strSql += " ,Purity = '" & txtPurity_Per_Man.Text & "'"
        strSql += " ,RatePurity = '" & Val(txtRatePurity_Amt.Text) & "'"
        strSql += " ,metalID = '" & Id & "'"
        strSql += " ,metaltype ='" & Mid(cmbMetalType.Text, 1, 1) & "'"
        strSql += " ,UserId = '" & userId & "'"
        strSql += " ,Updated = '" & Today.Date.ToString("yyyy-MM-dd") & "'"
        strSql += " ,Uptime = '" & Date.Now.ToLongTimeString & "'"
        strSql += " Where PurityId = '" & PurityId & "'"
        Try
            ExecQuery(SyncMode.Master, strSql, cn)
            funcNew()
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function
    Function funcOpen() As Integer
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Function
        funcCallGrid()
    End Function
    Function funcGetDetails(ByVal tempPurityId As String)
        Dim dt As New DataTable
        dt.Clear()
        strSql = " select "
        strSql += " PurityID,PurityName,Shortname,"
        strSql += " Purity,RatePurity,"
        strSql += " Case When MetalType = 'M' then 'METAL' else 'ORNAMENT' end as MetalType,"
        strSql += " (select MetalName from " & cnAdminDb & "..MetalMast where MetalId = p.Metalid)as MetalName"
        strSql += " from " & cnAdminDb & "..PurityMast as P where Purityid = '" & tempPurityId & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Return 0
        End If
        With dt.Rows(0)
            PurityId = .Item("PurityID").ToString
            txtPurityName__Man.Text = .Item("PurityName").ToString
            txtShortName.Text = .Item("Shortname").ToString
            txtPurity_Per_Man.Text = .Item("Purity").ToString
            txtRatePurity_Amt.Text = .Item("RatePurity").ToString
            cmbMetalName_Man.Text = .Item("metalName").ToString
            cmbMetalType.Text = .Item("metaltype").ToString
        End With
        flagSave = True
        PurityId = tempPurityId
        Return 0
    End Function

    Private Sub frmPurityMaster_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtPurityName__Man.Focused Then
                Exit Sub
            End If
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmPurityMaster_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        funcGridStyle(gridView)
        funcLoadMetalName()

        cmbMetalType.Items.Add("METAL")
        cmbMetalType.Items.Add("ORNAMENT")
        cmbMetalType.Text = "METAL"

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
                flagSave = True
                Me.SelectNextControl(Me, True, True, True, True)
            End If
        ElseIf e.KeyCode = Keys.Escape Then
            Me.SelectNextControl(Me, True, True, True, True)
        End If
    End Sub

    Private Sub txtPurityName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPurityName__Man.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If objGPack.DupChecker(txtPurityName__Man, "SELECT 1 FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYNAME = '" & txtPurityName__Man.Text & "' AND PURITYID <> '" & PurityId & "'") Then
                Exit Sub
            Else
                SendKeys.Send("{TAB}")
            End If
        End If
    End Sub

    Private Sub gridView_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.LostFocus
        lblStatus.Visible = False

    End Sub

    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
        If Not gridView.RowCount > 0 Then
            Exit Sub
        End If
        Dim delKey As String = gridView.Rows(gridView.CurrentRow.Index).Cells("PURITYID").Value.ToString
        Dim chkQry As String = " SELECT TOP 1 PURITYID FROM " & cnAdminDb & "..CATEGORY WHERE PURITYID = '" & delKey & "'"
        DeleteItem(SyncMode.Master, chkQry, "DELETE FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = '" & delKey & "' AND ISNULL(AUTOGENERATOR,'') = '' ")
        funcCallGrid()
    End Sub
End Class