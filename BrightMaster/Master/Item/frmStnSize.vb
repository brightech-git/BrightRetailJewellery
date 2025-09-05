Imports System.Data.OleDb
Public Class frmStnSize
    Dim StrSql As String
    Dim Cmd As OleDbCommand
    Dim Da As OleDbDataAdapter
    Dim flagUpdate As Boolean
    Dim updSno As Integer

    Private Sub CallGrid()
        StrSql = " SELECT STNSIZEID,SIZENAME,"
        StrSql += " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST AS I WHERE I.ITEMID = SS.STNITEMID)AS ITEMNAME,"
        StrSql += " ISNULL((SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST AS S WHERE S.SUBITEMID = SS.STNSITEMID),'')AS SUBITEMNAME,"
        StrSql += " STNWT,STNUNIT,CALCTYPE,DISPORDER,ACTIVE"
        StrSql += " FROM " & cnAdminDb & "..STNSIZE AS SS ORDER BY SS.DISPORDER"
        funcOpenGrid(StrSql, gridView)
        With gridView
            .Columns("STNSIZEID").Visible = False
            .Columns("SIZENAME").Width = 150
            .Columns("ITEMNAME").Width = 150
            .Columns("SUBITEMNAME").Width = 150
            .Columns("STNWT").HeaderText = "STNWT"
            .Columns("STNWT").Width = 70
            .Columns("STNWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("STNUNIT").HeaderText = "STNUNIT"
            .Columns("STNUNIT").Width = 70
            .Columns("CALCTYPE").Width = 70
            .Columns("STNUNIT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        End With
    End Sub

    Private Sub frmStnSize_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If gridView.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmStnSize_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        StrSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST"
        StrSql += " WHERE METALID = 'D' OR METALID = 'T'"
        StrSql += " ORDER BY ITEMNAME"
        objGPack.FillCombo(StrSql, cmbItemName_Man)
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        objGPack.TextClear(Me)
        cmbItemName_Man.Enabled = True
        cmbSubItemName_Man.Enabled = True
        CallGrid()
        flagUpdate = False
        updSno = Nothing        
        cmbActive.Items.Add("YES")
        cmbActive.Items.Add("NO")
        cmbActive.Text = "YES"
        cmbStnUnit.Items.Add("Carat")
        cmbStnUnit.Items.Add("Grams")
        cmbStnUnit.Text = "Carat"
        cmbStnCalc.Items.Add("Pcs")
        cmbStnCalc.Items.Add("Weight")
        cmbStnCalc.Text = "Pcs"
        cmbItemName_Man.Focus()
    End Sub

    Private Sub cmbItemName_Man_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbItemName_Man.LostFocus
        cmbSubItemName_Man.Text = ""
        StrSql = GetSubItemQry(New String() {"SUBITEMNAME"}, Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "'")))
        'StrSql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST "
        'StrSql += " WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "' AND SUBITEM = 'Y')"
        'StrSql += " ORDER BY SUBITEMNAME"
        objGPack.FillCombo(StrSql, cmbSubItemName_Man, True)
        If cmbSubItemName_Man.Items.Count > 0 Then cmbSubItemName_Man.Enabled = True Else cmbSubItemName_Man.Enabled = False
    End Sub


    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click, SaveToolStripMenuItem.Click
        ''Validation
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
        If objGPack.Validator_Check(Me) Then Exit Sub
        If txtSizeName.Text = "" Then
            MsgBox(E0005, MsgBoxStyle.Information)
            txtSizeName.Focus()
            Exit Sub
        End If
        If txtStnWt.Text = "" Then
            MsgBox(E0005, MsgBoxStyle.Information)
            txtStnWt.Focus()
            Exit Sub
        End If
        StrSql = " SELECT 'EXISTS' FROM " & cnAdminDb & "..STNSIZE "
        StrSql += " WHERE SIZENAME = '" & txtSizeName.Text & "'"
        StrSql += " AND "
        StrSql += " STNITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "')"
        StrSql += " AND STNSITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItemName_Man.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "')),0)"
        If flagUpdate Then
            StrSql += " AND STNSIZEID <> " & updSno & ""
        End If
        If objGPack.GetSqlValue(StrSql, , "-1") <> "-1" Then
            MsgBox("Already exists this size", MsgBoxStyle.Information)
            Exit Sub
        End If
        If flagUpdate Then
            UpdateEntry()
        Else
            SaveEntry()
        End If
    End Sub
    Private Sub UpdateEntry()
        Dim ItemId As Integer = Nothing
        Dim SubItemId As Integer = Nothing

        StrSql = " Select ItemId from " & cnAdminDb & "..ItemMast where"
        StrSql += " ItemName = '" & cmbItemName_Man.Text & "'"
        ItemId = Val(objGPack.GetSqlValue(StrSql))

        StrSql = " Select SubItemId from " & cnAdminDb & "..SubItemMast where"
        StrSql += " SubItemName = '" & cmbSubItemName_Man.Text & "'"
        StrSql += " AND ITEMID = " & ItemId & ""
        SubItemId = Val(objGPack.GetSqlValue(StrSql))

        StrSql = " UPDATE " & cnAdminDb & "..STNSIZE SET"
        StrSql += " SIZENAME='" & txtSizeName.Text & "'"
        StrSql += " ,STNITEMID=" & ItemId & ""
        StrSql += " ,STNSITEMID=" & SubItemId & ""
        StrSql += " ,STNWT=" & Val(txtStnWt.Text) & ""
        StrSql += " ,STNUNIT='" & Mid(cmbStnUnit.Text, 1, 1) & "'"
        StrSql += " ,CALCTYPE='" & Mid(cmbStnCalc.Text, 1, 1) & "'"
        StrSql += " ,DISPORDER=" & Val(txtDispOrder.Text) & ""
        StrSql += " ,ACTIVE='" & Mid(cmbActive.Text, 1, 1) & "'"
        StrSql += " ,USERID='" & userId & "'"
        StrSql += " WHERE STNSIZEID = " & updSno & ""
        Try
            ExecQuery(SyncMode.Master, StrSql, cn)
            btnNew_Click(Me, New EventArgs)
        Catch ex As Exception
            MsgBox(ex.Message & vbCrLf & ex.StackTrace)
        End Try
    End Sub
    Private Sub SaveEntry()
        Dim ItemId As Integer = Nothing
        Dim SubItemId As Integer = Nothing

        StrSql = " Select ItemId from " & cnAdminDb & "..ItemMast where"
        StrSql += " ItemName = '" & cmbItemName_Man.Text & "'"
        ItemId = Val(objGPack.GetSqlValue(StrSql, , , tran))

        StrSql = " Select SubItemId from " & cnAdminDb & "..SubItemMast where"
        StrSql += " SubItemName = '" & cmbSubItemName_Man.Text & "'"
        StrSql += " AND ITEMID = " & ItemId & ""
        SubItemId = Val(objGPack.GetSqlValue(StrSql, , , tran))

        StrSql = " INSERT INTO " & cnAdminDb & "..STNSIZE"
        StrSql += " ("
        StrSql += " STNSIZEID,SIZENAME,STNITEMID,STNSITEMID,"
        StrSql += " STNWT,STNUNIT,CALCTYPE,DISPORDER,ACTIVE,"
        StrSql += " USERID"
        StrSql += " )VALUES("
        StrSql += " " & Val(objGPack.GetSqlValue("SELECT ISNULL(MAX(STNSIZEID),0)+1 FROM " & cnAdminDb & "..STNSIZE")) & "" 'STNSIZEID
        StrSql += " ,'" & txtSizeName.Text & "'" 'SIZENAME
        StrSql += " ," & ItemId & "" 'ItemId
        StrSql += " ," & SubItemId & "" 'SubItemId
        StrSql += " ," & Val(txtStnWt.Text) & "" 'STNWT
        StrSql += " ,'" & Mid(cmbStnUnit.Text, 1, 1) & "'" 'STNUNIT
        StrSql += " ,'" & Mid(cmbStnCalc.Text, 1, 1) & "'" 'CALCTYPE
        StrSql += " ,'" & Val(txtDispOrder.Text) & "'" 'DISPORDER
        StrSql += " ,'" & Mid(cmbActive.Text, 1, 1) & "'" 'ACTIVE
        StrSql += " ,'" & userId & "'" 'USERID        
        StrSql += " )"
        Try
            ExecQuery(SyncMode.Master, StrSql, cn)
            CallGrid()
            txtSizeName.Clear()
            txtStnWt.Clear()
            txtDispOrder.Focus()
            btnNew_Click(Me, New EventArgs)
        Catch ex As Exception
           MsgBox(ex.Message & vbCrLf & ex.StackTrace)
        End Try
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
        End If
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            e.Handled = True
            If gridView.RowCount > 0 Then
                gridView.CurrentCell = gridView.CurrentCell
                If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Edit) Then Exit Sub
                GetDetails(Val(gridView.CurrentRow.Cells("STNSIZEID").Value.ToString))
                txtSizeName.Focus()
            End If
        End If
    End Sub
    Function GetDetails(ByVal temp As Integer) As Integer
        StrSql = " SELECT STNSIZEID,SIZENAME,"
        StrSql += " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST AS I WHERE I.ITEMID = SS.STNITEMID)AS ITEMNAME,"
        StrSql += " ISNULL((SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST AS S WHERE S.SUBITEMID = SS.STNSITEMID),'')AS SUBITEMNAME,"
        StrSql += " STNWT,CASE WHEN STNUNIT='C' THEN 'Carat' ELSE 'Grams' END STNUNIT"
        StrSql += " ,CASE WHEN CALCTYPE='P' THEN 'Pcs' ELSE 'Weight' END CALCTYPE"
        StrSql += " ,DISPORDER,CASE WHEN ACTIVE='Y' THEN 'Yes' ELSE 'No' END ACTIVE"
        StrSql += " FROM " & cnAdminDb & "..STNSIZE AS SS"
        StrSql += " WHERE STNSIZEID = " & temp & ""
        Dim dt As New DataTable
        dt.Clear()
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Return 0
        End If
        With dt.Rows(0)
            cmbItemName_Man.Text = .Item("ItemName").ToString
            cmbSubItemName_Man.Text = .Item("SubItemName").ToString
            txtSizeName.Text = .Item("SIZENAME").ToString
            txtStnWt.Text = .Item("STNWT").ToString
            cmbStnUnit.Text = .Item("STNUNIT").ToString
            cmbStnCalc.Text = .Item("CALCTYPE").ToString
            txtDispOrder.Text = .Item("DISPORDER").ToString
            cmbActive.Text = .Item("ACTIVE").ToString
        End With
        flagUpdate = True
        updSno = temp
        cmbItemName_Man.Enabled = False
        cmbSubItemName_Man.Enabled = False
    End Function

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnOpen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOpen.Click, OpenToolStripMenuItem.Click
        gridView.Focus()
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If gridView.RowCount > 0 Then
            gridView.CurrentCell = gridView.CurrentCell
            If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Edit) Then Exit Sub
            StrSql = "DELETE FROM " & cnAdminDb & "..STNSIZE WHERE STNSIZEID = '" & Val(gridView.CurrentRow.Cells("STNSIZEID").Value.ToString) & "'"
            Try
                ExecQuery(SyncMode.Master, StrSql, cn)
                gridView.Focus()
            Catch ex As Exception
               MsgBox(ex.Message & vbCrLf & ex.StackTrace)
            End Try
            CallGrid()
        End If
    End Sub
End Class