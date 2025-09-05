Imports System.Data.OleDb
Public Class frmBranchVA
    Dim da As OleDbDataAdapter
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim Sno As Integer = Nothing ''Update Purpose
    Dim flagSave As Boolean = False
    Function funcLoadItemName(ByVal combo As ComboBox, Optional ByVal Allitem As Boolean = False) As Integer
        combo.Items.Clear()
        If Allitem = True Then combo.Items.Add("ALL")
        strSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST ORDER BY ITEMNAME"
        objGPack.FillCombo(strSql, combo, False, False)
    End Function
    Function funcLoadSubItemName(ByVal combo As ComboBox) As Integer
        combo.Text = ""
        combo.Items.Clear()
        combo.Items.Add("ALL")
        strSql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST "
        If cmbItemName_Man.Text <> "" And cmbItemName_Man.Text <> "ALL" Then
            strSql += " WHERE ITEMID = "
            strSql += "(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "')"
        End If
        strSql += " ORDER BY SUBITEMNAME"
        objGPack.FillCombo(strSql, combo, False, False)
        If Not combo.Items.Count > 0 Then combo.Enabled = False Else combo.Enabled = True
    End Function
    Function funcCallGrid() As Integer
        Dim dt As New DataTable
        dt.Clear()

        strSql = " SELECT SNO"
        strSql += vbCrLf + " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=D.ITEMID)ITEM"
        strSql += vbCrLf + " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID=D.SUBITEMID)SUBITEM"
        strSql += vbCrLf + " ,FROMWEIGHT,TOWEIGHT"
        strSql += vbCrLf + " ,MAXWASTPER AS WASTPER,MAXMCGRM AS MCGRM"
        strSql += vbCrLf + " ,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID=D.COSTID)COSTNAME "
        strSql += vbCrLf + " FROM " & cnAdminDb & "..BRANCHVA D ORDER BY SNO"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        With gridView
            .DataSource = dt
            .Columns("ITEM").Width = 100
            .Columns("SUBITEM").Width = 100
            .Columns("FROMWEIGHT").Width = 90
            .Columns("TOWEIGHT").Width = 80
            .Columns("WASTPER").Width = 65
            .Columns("MCGRM").Width = 65
            .Columns("COSTNAME").Width = 100
            .Columns("SNO").Visible = False
            .Columns("FROMWEIGHT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("TOWEIGHT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("WASTPER").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("MCGRM").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            If .Columns.Contains("USERID") Then .Columns("USERID").Visible = False
        End With
    End Function

    Function funcNew()
        objGPack.TextClear(Me)
        strSql = vbCrLf + " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTTYPE='F' AND ISNULL(ACTIVE,'Y')<>'N' ORDER BY COSTNAME"
        objGPack.FillCombo(strSql, cmbCostCentre, , False)
        If cmbCostCentre.Items.Count > 0 Then cmbCostCentre.SelectedIndex = 0
        funcLoadItemName(cmbItemName_Man)
        funcLoadSubItemName(cmbSubItemName_Man)
        Sno = Nothing
        flagSave = False
        funcCallGrid()
        cmbItemName_Man.Select()
        Return 0
    End Function
    Function funcExit() As Integer
        Me.Close()
    End Function
    Function funcSave() As Integer
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Function
        If objGPack.Validator_Check(Me) Then Exit Function
        If flagSave = False Then
            funcAdd()
        ElseIf flagSave = True Then
            funcUpdate()
        End If
    End Function
    Function funcAdd()
        Dim dt As New DataTable
        dt.Clear()
        Dim Id As Integer = Nothing
        Dim tran As OleDbTransaction = Nothing
        Try
            strSql = " DECLARE @WTFROM FLOAT,@WTTO FLOAT,@PCSFROM INT,@PCSTO INT"
            strSql += vbCrLf + " SET @WTFROM = " & Val(txtWtFrom_Wet.Text) & ""
            strSql += vbCrLf + " SET @WTTO = " & Val(txtWtTo_Wet.Text) & ""
            strSql += vbCrLf + " SELECT 1 FROM " & cnAdminDb & "..BRANCHVA WHERE "
            strSql += vbCrLf + " ((@WTFROM BETWEEN FROMWEIGHT AND TOWEIGHT)"
            strSql += vbCrLf + " OR"
            strSql += vbCrLf + " (@WTTO BETWEEN FROMWEIGHT AND TOWEIGHT))"
            strSql += vbCrLf + " AND ITEMID = ISNULL((SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "'),0)"
            strSql += vbCrLf + " AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItemName_Man.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "')),0)"
            strSql += vbCrLf + " AND COSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME ='" & cmbCostCentre.Text & "')"
            If objGPack.DupCheck(strSql) Then
                MsgBox(E0002, MsgBoxStyle.Information)
                cmbCostCentre.Focus()
                Exit Function
            End If

            strSql = "SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME='" & cmbCostCentre.Text & "'"
            Dim CostId As String = objGPack.GetSqlValue(strSql, "COSTID", "", )

            strSql = "SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbItemName_Man.Text & "'"
            Dim Itemid As Integer = Val(objGPack.GetSqlValue(strSql, "ITEMID", "", ).ToString)

            strSql = "SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME='" & cmbSubItemName_Man.Text & "' AND ITEMID=" & Itemid
            Dim SubItemid As Integer = Val(objGPack.GetSqlValue(strSql, "SUBITEMID", "", ).ToString)

            tran = cn.BeginTransaction()
            strSql = " SELECT ISNULL(MAX(SNO),0)+1 AS SNO FROM "
            strSql += " " & cnAdminDb & "..BRANCHVA"
            Id = Val(objGPack.GetSqlValue(strSql, , , tran))
            strSql = " INSERT INTO " & cnAdminDb & "..BRANCHVA"
            strSql += " ("
            strSql += " SNO,"
            strSql += " ITEMID,SUBITEMID,"
            strSql += " FROMWEIGHT,"
            strSql += " TOWEIGHT,"
            strSql += " MAXWASTPER,"
            strSql += " MAXMCGRM,"
            strSql += " COSTID"
            strSql += " )VALUES ("
            strSql += " " & Id & ""
            strSql += " ," & Itemid
            strSql += " ," & SubItemid
            strSql += " ,'" & Val(txtWtFrom_Wet.Text) & "'"
            strSql += " ,'" & Val(txtWtTo_Wet.Text) & "'"
            strSql += " ,'" & Val(txtMaxWastage_Per.Text) & "'"
            strSql += " ,'" & Val(txtMaxMcGrm_Amt.Text) & "'"
            strSql += " ,'" & CostId & "'"
            strSql += " )"
            ExecQuery(SyncMode.Master, strSql, cn, tran)
            tran.Commit()
            tran = Nothing
            funcNew()
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback() : tran = Nothing
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
        Return 0
    End Function
    Function funcUpdate() As Integer
        Dim tran As OleDbTransaction = Nothing
        Try
            strSql = "SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME='" & cmbCostCentre.Text & "'"
            Dim CostId As String = objGPack.GetSqlValue(strSql, "COSTID", "", )

            strSql = "SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbItemName_Man.Text & "'"
            Dim Itemid As Integer = Val(objGPack.GetSqlValue(strSql, "ITEMID", "", ).ToString)

            strSql = "SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME='" & cmbSubItemName_Man.Text & "' AND ITEMID=" & Itemid
            Dim SubItemid As Integer = Val(objGPack.GetSqlValue(strSql, "SUBITEMID", "", ).ToString)

            tran = cn.BeginTransaction()
            strSql = "UPDATE " & cnAdminDb & "..BRANCHVA SET"
            strSql += " ITEMID = " & Itemid
            strSql += " ,SUBITEMID = " & SubItemid
            strSql += " ,FROMWEIGHT = '" & Val(txtWtFrom_Wet.Text) & "'"
            strSql += " ,TOWEIGHT = '" & Val(txtWtTo_Wet.Text) & "'"
            strSql += " ,MAXWASTPER = '" & Val(txtMaxMcGrm_Amt.Text) & "'"
            strSql += " ,MAXMCGRM = '" & Val(txtMaxMcGrm_Amt.Text) & "'"
            strSql += " ,COSTID = '" & CostId & "'"
            strSql += " WHERE SNO = " & Sno & ""
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
        strSql = " SELECT SNO "
        strSql += vbCrLf + " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=D.ITEMID)ITEM"
        strSql += vbCrLf + " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID=D.SUBITEMID)SUBITEM"
        strSql += vbCrLf + " ,FROMWEIGHT,TOWEIGHT"
        strSql += vbCrLf + " ,MAXWASTPER AS WASTPER,MAXMCGRM AS MCGRM"
        strSql += vbCrLf + " ,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID=D.COSTID)COSTNAME "
        strSql += vbCrLf + " FROM " & cnAdminDb & "..BRANCHVA D "
        strSql += vbCrLf + " WHERE SNO = " & tempId
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Return 0
        End If
        With dt.Rows(0)
            cmbItemName_Man.Text = .Item("ITEM").ToString
            cmbSubItemName_Man.Text = .Item("SUBITEM").ToString
            txtWtFrom_Wet.Text = Val(.Item("FROMWEIGHT").ToString)
            txtWtTo_Wet.Text = Val(.Item("TOWEIGHT").ToString)
            txtMaxWastage_Per.Text = Val(.Item("WASTPER").ToString)
            txtMaxMcGrm_Amt.Text = Val(.Item("MCGRM").ToString)
            cmbCostCentre.Text = .Item("COSTNAME").ToString
        End With
        Sno = tempId
        Return 0
    End Function

    Private Sub frmDisigner_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub frmDisigner_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        funcGridStyle(gridView)
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
        If cnCostId <> cnHOCostId Then MsgBox("Master Entry not allowed in Location..", MsgBoxStyle.Information) : Exit Sub
        funcSave()
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
            cmbCostCentre.Focus()
        ElseIf e.KeyCode = Keys.Enter Then
            e.Handled = True
            gridView.CurrentCell = gridView.CurrentCell
            If gridView.RowCount > 0 Then
                funcGetDetails(gridView.Item(0, gridView.CurrentRow.Index).Value.ToString)
                flagSave = True
                cmbItemName_Man.Focus()
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
        Dim delKey As String = gridView.Rows(gridView.CurrentRow.Index).Cells("SNO").Value.ToString
        strSql = "DELETE FROM " & cnAdminDb & "..BRANCHVA WHERE SNO = '" & delKey & "'"
        ExecQuery(SyncMode.Master, strSql, cn, tran)
        funcCallGrid()
    End Sub
    Private Sub cmbItemName_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbItemName_Man.SelectedIndexChanged
        If cmbItemName_Man.Text = "" Then
            cmbSubItemName_Man.Items.Clear()
        End If
        If flagSave = True Then
            Exit Sub
        End If
        funcLoadSubItemName(cmbSubItemName_Man)
    End Sub
    Private Sub txtWtFrom_Wet_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtWtFrom_Wet.GotFocus
        If flagSave = True Then Exit Sub
        strSql = " SELECT MAX(TOWEIGHT) FROM " & cnAdminDb & "..BRANCHVA"
        strSql += vbCrLf + " WHERE "
        strSql += vbCrLf + " ITEMID = ISNULL((SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "'),0)"
        strSql += vbCrLf + " AND ISNULL(SUBITEMID,0) = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItemName_Man.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "')),0)"
        strSql += vbCrLf + " AND COSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre.Text & "')"
        Dim wt As Double = Val(objGPack.GetSqlValue(strSql).ToString)
        txtWtFrom_Wet.Text = Format(wt + 0.001, "0.000")
    End Sub
End Class