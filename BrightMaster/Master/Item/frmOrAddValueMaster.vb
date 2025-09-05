Imports System.Data.OleDb
Public Class frmOrAddValueMaster
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim flagSave As Boolean = False
    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub
    Function funcCallGrid() As Integer
        strSql = " SELECT "
        strSql += " (SELECT TYPENAME FROM " & cnAdminDb & "..ORADMAST WHERE TYPEID = I.TYPEID) TYPENAME"
        strSql += " ,VALUEID,VALUENAME,"
        strSql += " CASE WHEN ACTIVE = 'Y' THEN 'YES' ELSE 'NO' END ACTIVE,DISPLAYORDER"
        strSql += " from " & cnAdminDb & "..ORADVALUEMAST AS I " 'WHERE ISNULL(ACTIVE,'Y') <> 'N'
        funcOpenGrid(strSql, gridView)
        For cnt As Integer = 0 To gridView.ColumnCount - 1
            gridView.Columns(cnt).HeaderText = UCase(gridView.Columns(cnt).HeaderText)
        Next
    End Function

    Function funcNew() As Integer
        objGPack.TextClear(Me)
        txtValueName__Man.Select()
        strSql = "SELECT TYPENAME FROM " & cnAdminDb & "..ORADMAST WHERE ISNULL(ACTIVE,'Y') <> 'N' ORDER BY DISPLAYORDER, TYPENAME"
        objGPack.FillCombo(strSql, cmbTypename, True, True)
        flagSave = False
        funcCallGrid()
        txtValueId_Num_Man.Text = objGPack.GetMax("VALUEID", "ORADVALUEMAST", cnAdminDb)
        cmbActive.Items.Clear()
        cmbActive.Items.Add("YES")
        cmbActive.Items.Add("NO")
        cmbActive.Text = "YES"
    End Function
    Function funcSave() As Integer
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Function
        If objGPack.Validator_Check(Me) Then Exit Function
        Dim Typeid As Integer = GetSqlValue(cn, "SELECT TYPEID FROM " & cnAdminDb & "..ORADMAST WHERE TYPENAME = '" & (cmbTypename.Text) & "'")
        Dim Valueid As Integer = Val(txtValueId_Num_Man.Text)
        Dim dt As New DataTable
        If flagSave = False Then
            strSql = " SELECT 1 FROM " & cnAdminDb & "..ORADVALUEMAST I WHERE "
            strSql += "  VALUENAME = '" & txtValueName__Man.Text & "'"
            strSql += " AND TYPEID IN (select TYPEID from " & cnAdminDb & "..ORADMAST WHERE TYPENAME IN ('" & cmbTypename.Text & "' ))"
            If objGPack.DupChecker(txtValueName__Man, strSql) Then Exit Function
            funcAdd(Typeid, Valueid)
        ElseIf flagSave = True Then
            funcUpdate(Typeid, vaLUeid)
        End If
        funcNew()
    End Function
    Function funcAdd(ByVal Typeid As Integer, ByVal ValueId As Integer) As Integer

        strSql = " INSERT INTO " & cnAdminDb & "..ORADVALUEMAST"
        strSql += " ("
        strSql += " TYPEID,VALUEID,VALUENAME,ACTIVE,DISPLAYORDER"
        strSql += " )Values("
        strSql += " " & Typeid & "" 'TYPEID        
        strSql += " , " & ValueId & "" 'VALUEID    
        strSql += " , '" & txtValueName__Man.Text & "'" 'SizeName                
        strSql += " , '" & IIf(cmbActive.Text = "YES", "Y", "N") & "'"
        If txtDisplayOrder_Num.Text = "" Then
            strSql += ", NULL"
        Else
            strSql += "," & txtDisplayOrder_Num.Text & ""
        End If
        strSql += ")"
        Try
            ExecQuery(SyncMode.Master, strSql, cn)
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function
    Function funcUpdate(ByVal Typeid As Integer, ByVal ValueId As Integer) As Integer

        strSql = " UPDATE " & cnAdminDb & "..ORADVALUEMAST SET"
        strSql += " TYPEID=" & Typeid & ""
        strSql += " ,VALUEID='" & txtValueId_Num_Man.Text & "'"
        strSql += " ,VALUENAME='" & txtValueName__Man.Text & "'"
        strSql += " , ACTIVE = '" & IIf(cmbActive.Text = "YES", "Y", "N") & "'"
        If txtDisplayOrder_Num.Text = "" Then
            strSql += ", DISPLAYORDER  = NULL"
        Else
            strSql += " , DISPLAYORDER  = " & txtDisplayOrder_Num.Text & ""
        End If
        strSql += " WHERE VALUEID = " & ValueId & " AND "
        strSql += " TYPEID = " & Typeid & " "
        Try
            ExecQuery(SyncMode.Master, strSql, cn)
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function
    Function funcExit() As Integer
        Me.Close()
    End Function
    Function funcGetDetails(ByVal TempValueId As Integer) As Integer
        strSql = " SELECT (SELECT TYPENAME FROM " & cnAdminDb & "..ORADMAST WHERE TYPEID= I.TYPEID) TYPENAME "
        strSql += " ,VALUEID,VALUENAME"
        strSql += " ,CASE WHEN ACTIVE = 'Y' THEN 'YES' ELSE 'NO' END ACTIVE"
        strSql += " ,DISPLAYORDER"
        strSql += " FROM " & cnAdminDb & "..ORADVALUEMAST AS I "
        strSql += " WHERE VALUEID = '" & TempValueId & "' "
        strSql += " ORDER BY DISPLAYORDER,VALUENAME"
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Return 0
        End If
        With dt.Rows(0)
            cmbTypename.Text = .Item("TYPENAME").ToString
            txtValueId_Num_Man.Text = .Item("VALUEID")
            txtValueName__Man.Text = .Item("VALUENAME")
            If .Item("DISPLAYORDER").ToString = "" Then
                txtDisplayOrder_Num.Text = ""
            Else
                txtDisplayOrder_Num.Text = .Item("DISPLAYORDER")
            End If
            cmbActive.Text = .Item("ACTIVE")
        End With
        flagSave = True
    End Function

    Private Sub frmOrAddValueMaster_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub frmOrAddValueMaster_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        funcGridStyle(gridView)
        txtValueId_Num_Man.Enabled = False
        funcNew()
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

    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        funcSave()
    End Sub


    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        funcNew()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
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
                funcGetDetails(gridView.Item(1, gridView.CurrentRow.Index).Value)
                txtValueName__Man.Focus()
            End If
        ElseIf e.KeyCode = Keys.Escape Then
            txtValueName__Man.Focus()
        End If
    End Sub

    Private Sub gridView_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.LostFocus

        lblStatus.Visible = False
    End Sub

    Private Sub txtValueName__Man_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtValueName__Man.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            strSql = " SELECT 1 FROM " & cnAdminDb & "..ORADVALUEMAST I WHERE "
            strSql += "  VALUENAME = '" & txtValueName__Man.Text & "'"
            strSql += " AND TYPEID IN (select TYPEID from " & cnAdminDb & "..ORADMAST WHERE TYPENAME IN ('" & cmbTypename.Text & "' ))"
            If objGPack.DupChecker(txtValueName__Man, strSql) Then Exit Sub
        End If
    End Sub

    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If Not gridView.RowCount > 0 Then
            Exit Sub
        End If
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
        Dim chkQry As String = Nothing
        Dim delKey As String = gridView.Rows(gridView.CurrentRow.Index).Cells("VALUEID").Value.ToString

        chkQry = " SELECT TOP 1 TYPEID FROM " & cnAdminDb & "..ORADTRAN WHERE VALUEID = '" & delKey & "' "
        DeleteItem(SyncMode.Master, chkQry, "DELETE FROM " & cnAdminDb & "..ORADVALUEMAST WHERE VALUEID = '" & delKey & "'")
        funcCallGrid()
    End Sub

End Class