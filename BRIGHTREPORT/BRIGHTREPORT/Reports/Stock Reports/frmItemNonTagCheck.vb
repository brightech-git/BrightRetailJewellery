Imports System.Data.OleDb
Public Class frmItemNonTagCheck
#Region "Variable Declaration"
    Dim strsql As String
    Dim dt As New DataTable
    Dim cmd As New OleDbCommand
    Dim da As New OleDbDataAdapter
    Dim Flag As Boolean = False
    Dim CheckId As Integer = 0
#End Region
    Private Sub frmItemNonTagCheck_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            If gridview.Focus = False Then
                SendKeys.Send("{TAB}")
            End If
        End If
    End Sub
    Private Sub frmItemNonTagCheck_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        strsql = "SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ACTIVE ='Y'"
        'cmd = New OleDbCommand(strsql, cn) 
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dt)
        BrighttechPack.GlobalMethods.FillCombo(cmbItemcount, dt, "ITEMCTRNAME", True)
        strsql = "SELECT * FROM " & cnAdminDb & "..ITEMMAST"
        da = New OleDbDataAdapter(strsql, cn)
        dt = New DataTable
        da.Fill(dt)
        BrighttechPack.GlobalMethods.FillCombo(cmbItem, dt, "ITEMNAME", True)
        btn_New_Click(Me, New EventArgs)
        HideTab()
    End Sub
    Function HideTab()
        tabmain.Appearance = TabAppearance.FlatButtons
        tabmain.ItemSize = New Size(0, 1)
        tabmain.SizeMode = TabSizeMode.Fixed
    End Function
    Private Sub btn_New_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_New.Click
        txtGWt.Text = ""
        txtNwt.Text = ""
        txtPcs.Text = ""
        cmbItem.SelectedIndex = -1
        cmbSubItem.SelectedIndex = -1
        cmbItemcount.SelectedIndex = -1
        btn_Save.Text = "Save [F1]"
        Flag = False
        CheckId = 0
    End Sub

    Private Sub cmbItem_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbItem.SelectionChangeCommitted
        strsql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST "
        strsql += " WHERE ITEMID =(SELECT TOP 1 ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME ='" & cmbItem.SelectedText & "')"
        da = New OleDbDataAdapter(strsql, cn)
        dt = New DataTable
        da.Fill(dt)
        BrighttechPack.GlobalMethods.FillCombo(cmbSubItem, dt, "SUBITEMNAME", True)
    End Sub

    Private Sub btn_Exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Exit.Click
        Me.Close()
    End Sub

    Private Sub btn_Open_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Open.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        funcCallGrid()
        tabmain.SelectedTab = tabView
        gridview.Focus()
    End Sub
    Function funccallgrid()
        strsql = " SELECT T.ID,IC.ITEMCTRNAME AS ITEMCOUNTER,IM.ITEMNAME,SUB.SUBITEMNAME,T.PCS AS PCS,T.GRSWT,T.NETWT "
        strsql += " FROM " & cnAdminDb & "..ITEMNONTAGSTKCHK AS T INNER JOIN " & cnAdminDb & "..ITEMCOUNTER AS IC"
        strsql += " ON T.ITEMCTRID=IC.ITEMCTRID INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID=T.ITEMID"
        strsql += " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SUB ON SUB.SUBITEMID=T.SUBITEMID"
        da = New OleDbDataAdapter(strsql, cn)
        dt = New DataTable
        da.Fill(dt)
        gridview.DataSource = Nothing
        If dt.Rows.Count > 0 Then
            gridview.DataSource = dt
            GridStyle()
        End If

    End Function
    Function GridStyle()
        With gridview
            .Columns("ID").Visible = False
            .Columns("ITEMNAME").Width = 150
            .Columns("SUBITEMNAME").Width = 150
            .Columns("ITEMCOUNTER").Width = 150
            .Columns("ITEMNAME").HeaderText = "Item Name"
            .Columns("SUBITEMNAME").HeaderText = "Sub Item Name"
            .Columns("ITEMCOUNTER").HeaderText = "Item Counter"
            .Columns("PCS").HeaderText = "Pcs"
            .Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("GRSWT").HeaderText = "GrsWt"
            .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("NETwT").HeaderText = "Net Wt"
            .Columns("NETwT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            '.ColumnHeadersDefaultCellStyle.Font = New Font("Verdana", 8.5, FontStyle.Bold)
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        End With
    End Function

    Private Sub btn_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Save.Click
        If (cmbItemcount.Text = "") Then
            MsgBox("Please select Item Counter", MsgBoxStyle.Information)
            Exit Sub
        End If
        If (cmbItem.Text = "") Then
            MsgBox("Please select Item name", MsgBoxStyle.Information)
            Exit Sub
        End If
        If (Val(txtPcs.Text) = 0) Then
            MsgBox("Pieces is Zero", MsgBoxStyle.Information)
            Exit Sub
        End If
        If (Val(txtGWt.Text) = 0) Then
            MsgBox("Gross weight is Zero", MsgBoxStyle.Information)
            Exit Sub
        End If
        If (Val(txtNwt.Text) = 0) Then
            MsgBox("Net weight is Zero", MsgBoxStyle.Information)
            Exit Sub
        End If
        If Not Flag Then
            strsql = "INSERT INTO " & cnAdminDb & "..ITEMNONTAGSTKCHK ("
            strsql += " ID,ITEMID,SUBITEMID,ITEMCTRID,PCS,GRSWT,NETWT,TRANDATE) VALUES("
            strsql += "(SELECT ISNULL(MAX(ID),0)+1 FROM " & cnAdminDb & "..ITEMNONTAGSTKCHK)"
            strsql += ",(SELECT TOP 1 ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME ='" & cmbItem.Text & "')"
            If cmbSubItem.Text = "" Then
                strsql += ",0"
            Else

                strsql += ",(SELECT TOP 1 SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME ='" & cmbSubItem.Text & "')"
            End If
            strsql += ",(SELECT TOP 1 ITEMCTRID  FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME='" & cmbItemcount.Text & "')"
            strsql += "," & Val(txtPcs.Text) & "," & Val(txtGWt.Text) & "," & Val(txtNwt.Text) & ""
            strsql += ",'" & Format(Now, "yyyy-MM-dd") & "')"
            cmd = New OleDbCommand(strsql, cn)
            cmd.ExecuteNonQuery()
            MsgBox("Saved Successfully", MsgBoxStyle.Information)
        Else
            strsql = " UPDATE " & cnAdminDb & "..ITEMNONTAGSTKCHK SET"
            strsql += " ITEMID="
            strsql += "(SELECT TOP 1 ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME ='" & cmbItem.Text & "')"
            strsql += ",SUBITEMID="
            If cmbSubItem.Text = "" Then
                strsql += "0"
            Else

                strsql += "(SELECT TOP 1 SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME ='" & cmbSubItem.Text & "')"
            End If
            strsql += ",PCS=" & Val(txtPcs.Text) & ""
            strsql += ",GRSWT=" & Val(txtGWt.Text) & ""
            strsql += ",NETWT=" & Val(txtNwt.Text) & ""
            strsql += "WHERE ID=" & CheckId & ""
            cmd = New OleDbCommand(strsql, cn)
            cmd.ExecuteNonQuery()
            MsgBox("Updated Successfully", MsgBoxStyle.Information)
        End If
        funccallgrid()
        btn_New_Click(Me, New EventArgs)
    End Sub

    Private Sub gridview_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridview.KeyDown
        If e.KeyCode = Keys.Enter Then
            If gridview.CurrentRow.Cells("ID").Value <> Nothing Then
                With gridview.CurrentRow
                    CheckId = Val(.Cells("ID").Value)
                    cmbItem.Text = .Cells("ITEMNAME").Value.ToString()
                    cmbSubItem.Text = .Cells("SUBITEMNAME").Value.ToString()
                    cmbItemcount.Text = .Cells("ITEMCOUNTER").Value.ToString()
                    txtPcs.Text = Val(.Cells("PCS").Value)
                    txtGWt.Text = Val(.Cells("GRSWT").Value)
                    txtNwt.Text = Val(.Cells("NETWT").Value)
                    Flag = True
                    tabmain.SelectedTab = tabEntry
                    btn_Save.Text = "Update [F1]"
                    cmbItemcount.Focus()
                End With
            End If
        ElseIf e.KeyCode = Keys.Escape Then
            tabmain.SelectedTab = tabEntry
            cmbItemcount.Focus()
        End If
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btn_New_Click(Me, New EventArgs)
    End Sub

    Private Sub SaveToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        btn_Save_Click(Me, New EventArgs)
    End Sub

    Private Sub ViewToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ViewToolStripMenuItem.Click
        btn_Open_Click(Me, New EventArgs)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        btn_Exit_Click(Me, New EventArgs)
    End Sub
End Class