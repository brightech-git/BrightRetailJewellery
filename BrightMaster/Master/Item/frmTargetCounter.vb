Imports System.Data.OleDb
Public Class frmTargetCounter
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim flagSave As Boolean = False
    Dim PctFile As String
    Dim OpenFileDia As New OpenFileDialog
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Dim dt1 As New DataTable
        dt1.Clear()
        cmbCounter.Items.Clear()
        cmbItemName_Man.Items.Clear()
        cmbItemName_Man.Items.Add("ALL")
        cmbItemName_Man.Text = "ALL"
        strSql = " select ItemName from " & cnAdminDb & "..ItemMast Order by ItemName"
        objGPack.FillCombo(strSql, cmbItemName_Man, False, False)
        funcGridStyle(gridView)
        ''Loading SubItem
        Dim dt As New DataTable
        cmbSubItemName_Own.Items.Clear()
        'cmbSubItemName.Items.Add("ALL")
        'cmbSubItemName.Text = "ALL"
        strSql = " Select SUBITEMNAME from " & cnAdminDb & "..SUBITEMMAST where itemid=(Select ItemId from " & cnAdminDb & "..ItemMast where ItemName='" & cmbItemName_Man.Text & "') ORDER BY SUBITEMNAME"
        objGPack.FillCombo(strSql, cmbSubItemName_Own, False, False)
        cmbSubItemName_Own.Text = "[NONE]"

        strSql = " Select distinct ItemCtrName from " & cnAdminDb & "..ItemCounter"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt1)
        If dt1.Rows.Count > 0 Then
            objGPack.FillCombo(strSql, cmbCounter, False, False)
            cmbCounter.Text = "[NONE]"
        End If
        funcNew()
    End Sub
    Function funcCalGrid() As Integer
        Dim dt As New DataTable
        dt.Clear()
        strSql = " Select SNO,T.ItemId,"
        strSql += " IM.ItemName,SM.SubItemName,T.PCS,T.GRSWT,IC.ITEMCTRNAME as COUNTERNAME"
        strSql += " from " & cnAdminDb & "..TargetCounter T"
        strSql += " left outer join " & cnAdminDb & "..ItemMast IM on IM.itemID=T.ItemId"
        strSql += " left outer join " & cnAdminDb & "..SubItemMast SM on SM.Subitemid=T.SubItemId"
        strSql += " left outer join " & cnAdminDb & "..ItemCounter IC on IC.ItemCtrId=T.ItemCtrID"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        gridView.DataSource = dt
        gridView.Columns("SNO").Visible = False
        gridView.Columns("ItemId").Visible = False
        gridView.Columns("ITEMNAME").MinimumWidth = 200
        gridView.Columns("SUBITEMNAME").MinimumWidth = 200
        gridView.Columns("COUNTERNAME").MinimumWidth = 150
        gridView.Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridView.Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        For cnt As Integer = 0 To gridView.ColumnCount - 1
            gridView.Columns(cnt).HeaderText = UCase(gridView.Columns(cnt).HeaderText)
        Next
    End Function
    Function funcNew() As Integer
        flagSave = False
        Dim dt As New DataTable
        dt.Clear()
        funcCalGrid()
        cmbItemName_Man.Focus()
        cmbItemName_Man.Text = "ALL"
        cmbCounter.Text = "ALL"
        funcClear()
    End Function
    Function funcClear() As Integer
        txtSNo.Text = ""
        txtPiece_NUM.Text = ""
        txtWeight_WET.Text = ""
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
    Function funcAdd() As Integer
        Dim itemId As Integer = Nothing
        Dim subItemId As Integer = Nothing
        Dim itemCtrId As Integer = Nothing
        Dim itemFlag, Iscounter As Boolean
        Dim wtUnit As String = Nothing
        Dim sNo As Integer
        Dim dt As New DataTable
        Dim dt1, dt2 As New DataTable
        dt1.Clear()
        dt.Clear()
        dt2.Clear()
        strSql = " Select ItemId from " & cnAdminDb & "..ItemMast where ItemName = '" & cmbItemName_Man.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            itemId = dt.Rows(0).Item("ItemId")
        End If
        strSql = " Select SubItemId from " & cnAdminDb & "..SubItemMast where SubItemName = '" & cmbSubItemName_Own.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt1)
        If dt1.Rows.Count > 0 Then
            subItemId = dt1.Rows(0).Item("SubItemId")
        End If
        strSql = "Select ItemCtrId from " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME ='" & cmbCounter.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt2)
        If dt2.Rows.Count > 0 Then
            itemCtrId = dt2.Rows(0).Item("ItemCtrId")
            itemFlag = True
        Else
            itemFlag = False
        End If
        strSql = "Select ItemCtrId from " & cnAdminDb & "..TargetCounter where ItemId=" & itemId & " and SubItemId=" & subItemId & " and ItemCtrID =" & itemCtrId
        da = New OleDbDataAdapter(strSql, cn)
        Dim dt3 As New DataTable
        da.Fill(dt3)
        If dt3.Rows.Count > 0 Then
            Iscounter = True
        End If

        strSql = "Select isnull(max(SNO),0)+1 as SNO from " & cnAdminDb & "..TargetCounter "
        da = New OleDbDataAdapter(strSql, cn)
        Dim dt4 As New DataTable
        da.Fill(dt4)
        If dt4.Rows.Count > 0 Then
            sNo = dt4.Rows(0).Item("SNO")
        End If

        If Iscounter = False Then
            If itemFlag = True Then
                strSql = " insert into " & cnAdminDb & "..TargetCounter"
                strSql += " ("
                strSql += " SNO,ItemId,SubItemId,"
                strSql += " UserId,Updated,Uptime"
                strSql += " ,PCS,GRSWT,ItemCtrId,WeightUnit"
                strSql += " )values ("
                strSql += " " & sNo & ""
                strSql += " ," & itemId & "" 'ItemId
                strSql += " ," & subItemId & "" 'SubItemId
                strSql += " ,'" & userId & "'" 'UserId
                strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'Updated
                strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'Uptime
                strSql += " ," & Val(txtPiece_NUM.Text) & "" 'pcs
                strSql += " ," & Val(txtWeight_WET.Text) & "" 'weight
                strSql += " ," & itemCtrId & ""    'ItemCounterID
                If rdbGram.Checked Then
                    wtUnit = "G"
                    strSql += " ,'" & wtUnit & "' )"
                ElseIf rdbDiaWt.Checked Then
                    wtUnit = "C"
                    strSql += " ,'" & wtUnit & "' )"
                End If


                Try
                    ExecQuery(SyncMode.Master, strSql, cn)

                    funcNew()
                Catch ex As Exception
                    MsgBox(ex.Message)
                    MsgBox(ex.StackTrace)
                End Try
            Else
                MsgBox("Invalid Counter...")
            End If
        Else
            MsgBox("Counter Already Exist...")
        End If
    End Function

    Function funcUpdate() As Integer
        Dim itemId As Integer = Nothing
        Dim subItemId As Integer = Nothing
        Dim itemCtrId As Integer = Nothing
        Dim itemFlag As Boolean
        Dim dt, dt1, dt2 As New DataTable
        Dim wtUnit As String = Nothing
        dt1.Clear()
        dt.Clear()
        dt2.Clear()
        strSql = " Select ItemId from " & cnAdminDb & "..ItemMast where ItemName = '" & cmbItemName_Man.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            itemId = dt.Rows(0).Item("ItemId")
        End If
        strSql = " Select SubItemId from " & cnAdminDb & "..SubItemMast where SubItemName = '" & cmbSubItemName_Own.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt1)
        If dt1.Rows.Count > 0 Then
            subItemId = dt1.Rows(0).Item("SubItemId")
        End If
        strSql = "Select ItemCtrId from " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME ='" & cmbCounter.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt2)
        If dt2.Rows.Count > 0 Then
            itemCtrId = dt2.Rows(0).Item("ItemCtrId")
            itemFlag = True
        End If
        If itemFlag = True Then
            strSql = " Update " & cnAdminDb & "..TargetCounter Set "
            strSql += " ItemId=" & itemId & ""
            strSql += " ,SubItemId=" & subItemId & ""
            strSql += " ,UserId='" & userId & "'"
            strSql += " ,Updated='" & Today.Date.ToString("yyyy-MM-dd") & "'"
            strSql += " ,Uptime='" & Date.Now.ToLongTimeString & "'"
            strSql += " ,PCS = " & Val(txtPiece_NUM.Text) & ""
            strSql += " ,GRSWT = " & Val(txtWeight_WET.Text) & ""
            strSql += " ,ITEMCTRID= " & itemCtrId & ""
            If rdbGram.Checked Then
                wtUnit = "G"
                strSql += " ,WeightUnit='" & wtUnit & "'"
            ElseIf rdbDiaWt.Checked Then
                wtUnit = "C"
                strSql += " ,WeightUnit='" & wtUnit & "'"
            End If
            strSql += " Where SNO = " & txtSNo.Text & ""
            Try
                ExecQuery(SyncMode.Master, strSql, cn)
                funcNew()
            Catch ex As Exception
                MsgBox(ex.Message)
                MsgBox(ex.StackTrace)
            End Try
        Else
            MsgBox("Invalid Counter...")
        End If
    End Function
    Function funcExit() As Integer
        Me.Close()
    End Function
    Function funcGetDetails(ByVal sNo As Integer) As Integer
        Dim dt As New DataTable
        dt.Clear()
        strSql = " Select SNO,"
        strSql += " T.ItemId,"
        strSql += " (Select ItemName from " & cnAdminDb & "..ItemMast I where I.ItemId=T.ItemId)as ItemName,"
        strSql += " SubItemId,"
        strSql += " (Select SubItemName from " & cnAdminDb & "..SubItemMast S where S.SubItemId=T.SubItemId)as SubItemName,"
        strSql += " PCS,GRSWT,ItemCtrId,(Select ItemCtrName from " & cnAdminDb & "..ItemCounter IC where IC.ItemCtrId=T.ItemCtrId)as CounterName"
        strSql += " from " & cnAdminDb & "..TargetCounter T"
        strSql += " Where SNO = " & sNo & ""
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Return 0
        End If
        With dt.Rows(0)
            txtSNo.Text = .Item("SNO")
            If .Item("itemname").ToString <> String.Empty Then cmbItemName_Man.Text = .Item("ItemName")
            If .Item("SubItemName").ToString <> String.Empty Then cmbSubItemName_Own.Text = .Item("SubItemName")
            txtPiece_NUM.Text = .Item("PCS").ToString
            txtWeight_WET.Text = .Item("GRSWT").ToString
            If .Item("CounterName").ToString <> String.Empty Then cmbCounter.Text = .Item("CounterName")
        End With
        flagSave = True
    End Function

    Private Sub frmTargetCounter_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmTargetCounter_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        funcGridStyle(gridView)
        funcNew()
    End Sub

    Private Sub gridView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.GotFocus
        lblStatus.Visible = True
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            If gridView.RowCount > 0 Then
                gridView.CurrentCell = gridView.CurrentCell
                If gridView.Item(0, gridView.CurrentRow.Index).Value.ToString <> "" Then
                    funcGetDetails(gridView.Item(0, gridView.CurrentRow.Index).Value.ToString)
                End If
                cmbItemName_Man.Focus()
                cmbSubItemName_Own.Focus()
            End If
        End If
        If e.KeyCode = Keys.Delete Then
            If gridView.Rows.Count > 0 Then
                Dim result = MessageBox.Show("Do you want to Delete...", "Message", MessageBoxButtons.YesNo)
                If result = Windows.Forms.DialogResult.Yes Then
                    strSql = " Delete from " & cnAdminDb & "..TARGETCOUNTER where Sno=" & gridView("SNo", gridView.CurrentRow.Index).Value & ""
                    cmd = New OleDbCommand(strSql, cn)
                    If Not tran Is Nothing Then cmd.Transaction = tran
                    cmd.ExecuteNonQuery()
                    funcCalGrid()
                    funcClear()
                    cmbItemName_Man.Focus()
                    cmbItemName_Man.Text = "ALL"
                    cmbCounter.Text = "ALL"
                    cmbSubItemName_Own.Text = "ALL"
                    gridView.Refresh()
                ElseIf result = Windows.Forms.DialogResult.No Then
                    Exit Sub
                End If
            End If
        End If
    End Sub

    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        funcSave()
    End Sub

    Private Sub OpenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        funcCalGrid()
        gridView.Focus()
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        funcNew()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        funcExit()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        funcNew()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        funcExit()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        funcSave()
    End Sub

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        funcCalGrid()
        gridView.Focus()
    End Sub
    Private Sub gridView_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.LostFocus
        lblStatus.Visible = False
    End Sub

    Private Sub cmbSubItemName_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSubItemName_Own.SelectedIndexChanged
        Dim itemId As Integer = Nothing
        Dim dt As New DataTable
        dt.Clear()

        strSql = " Select ItemId from " & cnAdminDb & "..ItemMast where ItemName = '" & cmbItemName_Man.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            itemId = dt.Rows(0).Item("ItemId")
        End If
    End Sub

    Private Sub cmbItemName_Man_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbItemName_Man.SelectedIndexChanged
        Dim dt As New DataTable
        cmbSubItemName_Own.Items.Clear()
        cmbSubItemName_Own.Items.Add("ALL")
        'cmbSubItemName.Text = "[NONE]"
        strSql = " Select SUBITEMNAME from " & cnAdminDb & "..SUBITEMMAST where itemid=(Select ItemId from " & cnAdminDb & "..ItemMast where ItemName='" & cmbItemName_Man.Text & "') ORDER BY SUBITEMNAME"
        objGPack.FillCombo(strSql, cmbSubItemName_Own, False, False)
        cmbSubItemName_Own.Text = "[NONE]"
    End Sub
    Private Sub gridView_RowEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.RowEnter
        With gridView.Rows(e.RowIndex)
            txtSNo.Text = .Cells("SNO").Value.ToString
            cmbItemName_Man.Text = .Cells("ITEMNAME").Value.ToString
            cmbSubItemName_Own.Text = .Cells("SUBITEMNAME").Value.ToString
            txtPiece_NUM.Text = .Cells("PCS").Value.ToString
            txtWeight_WET.Text = .Cells("GRSWT").Value.ToString
            cmbCounter.Text = .Cells("CounterName").Value.ToString
        End With
    End Sub

    Private Sub frmTargetCounter_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = (Keys.F1) Then funcSave()
        If e.KeyCode = (Keys.F2) Then
            If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
            funcCalGrid()
            gridView.Focus()
        End If
        If e.KeyCode = (Keys.F3) Then funcNew()
        If e.KeyCode = (Keys.F12) Then funcExit()
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If Not gridView.RowCount > 0 Then
            Exit Sub
        End If
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
        Dim chkQry As String = Nothing
        Dim delKey As String = gridView.Rows(gridView.CurrentRow.Index).Cells("SNO").Value.ToString
        If gridView.Rows.Count > 0 Then
            Dim result = MessageBox.Show("Do you want to Delete...", "Message", MessageBoxButtons.YesNo)
            If result = Windows.Forms.DialogResult.Yes Then
                strSql = " Delete from " & cnAdminDb & "..TARGETCOUNTER where Sno=" & gridView("SNo", gridView.CurrentRow.Index).Value & ""
                cmd = New OleDbCommand(strSql, cn)
                If Not tran Is Nothing Then cmd.Transaction = tran
                cmd.ExecuteNonQuery()
                funcCalGrid()
                funcClear()
                cmbItemName_Man.Focus()
                cmbItemName_Man.Text = "ALL"
                cmbCounter.Text = "ALL"
                cmbSubItemName_Own.Text = "ALL"
                gridView.Refresh()
            ElseIf result = Windows.Forms.DialogResult.No Then
                Exit Sub
            End If
        End If
        'strSql = " SELECT DBNAME FROM " & cnAdminDb & "..DBMASTER"
        'Dim dtDb As New DataTable
        'da = New OleDbDataAdapter(strSql, cn)
        'da.Fill(dtDb)
        'chkQry = " SELECT TOP 1 SNO FROM " & cnAdminDb & "..TARGETCOUNTER WHERE SNO = '" & delKey & "'"
        'DeleteItem(SyncMode.Master, chkQry, "DELETE FROM " & cnAdminDb & "..TARGETCOUNTER WHERE SNO = '" & delKey & "'")
        'funcCalGrid()
    End Sub

    Private Sub rdbGram_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdbGram.CheckedChanged
        If rdbGram.Checked Then
            lblWt.Text = "Weight"
        End If
    End Sub

    Private Sub rdbDiaWt_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdbDiaWt.CheckedChanged
        If rdbDiaWt.Checked Then
            lblWt.Text = "Dia Weight"
        End If
    End Sub
End Class