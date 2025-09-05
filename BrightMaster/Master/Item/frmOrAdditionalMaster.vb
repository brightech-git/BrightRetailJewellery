Imports System.Data.OleDb
Public Class frmOrAdditionalMaster
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
        strSql += " TYPEID,TYPENAME,"
        strSql += " CASE WHEN ACTIVE = 'Y' THEN 'YES' ELSE 'NO' END ACTIVE,DISPLAYORDER"
        strSql += " from " & cnAdminDb & "..ORADMAST AS I " 'WHERE ISNULL(ACTIVE,'Y') <> 'N'
        funcOpenGrid(strSql, gridView)
        For cnt As Integer = 0 To gridView.ColumnCount - 1
            gridView.Columns(cnt).HeaderText = UCase(gridView.Columns(cnt).HeaderText)
        Next
    End Function

    Function funcNew() As Integer
        objGPack.TextClear(Me)
        txtTypeName__Man.Select()
        strSql = "SELECT TYPENAME FROM " & cnAdminDb & "..ORADMAST WHERE ISNULL(ACTIVE,'Y') <> 'N' ORDER BY DISPLAYORDER, TYPENAME"
        objGPack.FillCombo(strSql, cmbTypename, True, True)
        flagSave = False
        funcCallGrid()
        txtTypeId_Num_Man.Text = objGPack.GetMax("TYPEID", "ORADMAST", cnAdminDb)
        cmbActive.Items.Clear()
        cmbActive.Items.Add("YES")
        cmbActive.Items.Add("NO")
        cmbActive.Text = "YES"
    End Function
    Function funcSave() As Integer
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Function
        If objGPack.Validator_Check(Me) Then Exit Function
        Dim TypeId As Integer = Val(txtTypeId_Num_Man.Text)
        Dim dt As New DataTable
        strSql = " SELECT 1 FROM " & cnAdminDb & "..ORADMAST WHERE "
        strSql += "  TYPENAME = '" & txtTypeName__Man.Text & "'"
        strSql += " AND TYPEID <> '" & txtTypeId_Num_Man.Text & "'"
        If objGPack.DupChecker(txtTypeName__Man, strSql) Then Exit Function
        If flagSave = False Then
            funcAdd(TypeId)
        ElseIf flagSave = True Then
            funcUpdate(TypeId)
        End If
        funcNew()
    End Function
    Function funcAdd(ByVal Typeid As Integer) As Integer

        strSql = " insert into " & cnAdminDb & "..ORADMAST"
        strSql += " ("
        strSql += " TYPEID,TYPENAME,ACTIVE,DISPLAYORDER"
        strSql += " )Values("
        strSql += " " & Typeid & "" 'TYPEID        
        strSql += " ,'" & txtTypeName__Man.Text & "'" 'SizeName                
        strSql += " , '" & IIf(cmbActive.Text = "YES", "Y", "N") & "'"
        If txtDisplayOrder_Num.Text = "" Then
            strSql += " , NULL"
        Else
            strSql += " , " & txtDisplayOrder_Num.Text & ""
        End If

        strSql += ")"
        Try
            ExecQuery(SyncMode.Master, strSql, cn)
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function
    Function funcUpdate(ByVal Typeid As Integer) As Integer

        strSql = " Update " & cnAdminDb & "..ORADMAST Set"
        strSql += " TYPEID=" & Typeid & ""
        strSql += " ,TYPENAME='" & txtTypeName__Man.Text & "'"
        strSql += " , ACTIVE = '" & IIf(cmbActive.Text = "YES", "Y", "N") & "'"
        If txtDisplayOrder_Num.Text = "" Then
            strSql += ", DISPLAYORDER  = NULL"
        Else
            strSql += " , DISPLAYORDER  = " & txtDisplayOrder_Num.Text & ""
        End If
        strSql += " WHERE TYPEID = " & txtTypeId_Num_Man.Text & ""
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
        strSql = " SELECT TYPENAME "
        strSql += " ,TYPEID,CASE WHEN ACTIVE = 'Y' THEN 'YES' ELSE 'NO' END ACTIVE,DISPLAYORDER"
        strSql += " FROM " & cnAdminDb & "..ORADMAST AS I"
        strSql += " WHERE TYPEID = '" & TempValueId & "' ORDER BY DISPLAYORDER,TYPENAME"
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Return 0
        End If
        With dt.Rows(0)
            txtTypeId_Num_Man.Text = .Item("TYPEID")
            txtTypeName__Man.Text = .Item("TYPENAME")
            If .Item("DISPLAYORDER").ToString = "" Then
                txtDisplayOrder_Num.Text = ""
            Else
                txtDisplayOrder_Num.Text = .Item("DISPLAYORDER")
            End If

            cmbActive.Text = .Item("ACTIVE")
        End With
        flagSave = True
    End Function

    Private Sub frmOrAdditionalMaster_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub frmOrAdditionalMaster_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        funcGridStyle(gridView)
        txtTypeId_Num_Man.Enabled = False
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
                funcGetDetails(gridView.Item(0, gridView.CurrentRow.Index).Value)
                txtTypeName__Man.Focus()
            End If
        ElseIf e.KeyCode = Keys.Escape Then
            txtTypeName__Man.Focus()
        End If
    End Sub

    Private Sub gridView_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.LostFocus

        lblStatus.Visible = False
    End Sub

    Private Sub txtValueName__Man_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtTypeName__Man.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            strSql = " SELECT 1 FROM " & cnAdminDb & "..ORADMAST I WHERE "
            strSql += "  TYPENAME = '" & txtTypeName__Man.Text & "'"
            strSql += " AND TYPEID <> '" & txtTypeId_Num_Man.Text & "'"
            If objGPack.DupChecker(txtTypeName__Man, strSql) Then Exit Sub
        End If
    End Sub

    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If Not gridView.RowCount > 0 Then
            Exit Sub
        End If
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
        Dim chkQry As String = Nothing
        Dim delKey As String = gridView.Rows(gridView.CurrentRow.Index).Cells("TYPEID").Value.ToString

        chkQry = " SELECT TOP 1 TYPEID FROM " & cnAdminDb & "..ORADTRAN WHERE TYPEID = '" & delKey & "' "
        DeleteItem(SyncMode.Master, chkQry, "DELETE FROM " & cnAdminDb & "..ORADMAST WHERE TYPEID = '" & delKey & "'")
        funcCallGrid()
    End Sub



    Private Sub Label2_Click(sender As Object, e As EventArgs) Handles Label2.Click

    End Sub
End Class