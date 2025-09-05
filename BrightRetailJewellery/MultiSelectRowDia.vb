Imports System.Data.OleDb
Public Class MultiSelectRowDia
    Public DtGrid As New DataTable
    Public RowSelected() As DataRow = Nothing
    Public DateCheck As Boolean = True
    Dim objSearch As Object
    Dim VALIDATECOLNAME As String
    Public SelectedSno As String = ""
    Public Sub New(ByVal DtGrid As DataTable, ByVal VALIDATECOLNAME As String)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        Me.VALIDATECOLNAME = VALIDATECOLNAME
        ' Add any initialization after the InitializeComponent() call.
        Me.DtGrid = DtGrid
        Dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        Dgv.DataSource = DtGrid
        Dgv.RowHeadersVisible = False
        BrighttechPack.GlobalMethods.FormatGridColumns(Dgv, True, , True)
        For Each dtCol As DataColumn In DtGrid.Columns
            If dtCol.Caption.Contains("VISIBLE_FALSE") Then
                Dgv.Columns(dtCol.ColumnName).Visible = False
            End If
        Next
        If Dgv.Columns.Contains("CHECK") Then Dgv.Columns("CHECK").HeaderText = ""
        If Dgv.Columns.Contains("CHECK") Then Dgv.Columns("CHECK").ReadOnly = False
        Dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        Dgv.Select()
        objSearch = New frmGridSearch(Dgv)

        If DtGrid.Columns.Contains("TAGNO") Then
            lblSearchTagNo.Visible = True
            txtSearchTagNo.Visible = True
        Else
            lblSearchTagNo.Visible = False
            txtSearchTagNo.Visible = False
        End If
    End Sub
    Public Sub New(ByVal DtGrid As DataTable, ByVal VALIDATECOLNAME As String, ByVal Editcolname As String)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        Me.VALIDATECOLNAME = VALIDATECOLNAME
        ' Add any initialization after the InitializeComponent() call.
        Me.DtGrid = DtGrid
        Dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        Dgv.DataSource = DtGrid
        Dgv.RowHeadersVisible = False
        BrighttechPack.GlobalMethods.FormatGridColumns(Dgv, True, , True)
        For Each dtCol As DataColumn In DtGrid.Columns
            If dtCol.Caption.Contains("VISIBLE_FALSE") Then
                Dgv.Columns(dtCol.ColumnName).Visible = False
            End If
        Next
        If Dgv.Columns.Contains("CHECK") Then Dgv.Columns("CHECK").HeaderText = ""
        If Dgv.Columns.Contains("CHECK") Then Dgv.Columns("CHECK").ReadOnly = False
        Dim editcolarray() As String = Editcolname.Split(",")
        If editcolarray.Length > 0 Then
            For ii As Integer = 0 To editcolarray.Length - 1
                If Dgv.Columns.Contains(editcolarray(ii).ToString) Then Dgv.Columns(editcolarray(ii).ToString).ReadOnly = False
            Next
        End If
        Dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        Dgv.Select()
        objSearch = New frmGridSearch(Dgv)

        If DtGrid.Columns.Contains("TAGNO") Then
            lblSearchTagNo.Visible = True
            txtSearchTagNo.Visible = True
        Else
            lblSearchTagNo.Visible = False
            txtSearchTagNo.Visible = False
        End If
    End Sub

    Private Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        DtGrid.AcceptChanges()
        Dim dtDistinct As DataTable
        'dtDistinct = CType(CType(Dgv.DataSource, DataTable).Select("CHECK = TRUE"), DataTable)..ToTable(True, "BILLDATE")
        If CType(Dgv.DataSource, DataTable).Columns.Contains(VALIDATECOLNAME) Then
            dtDistinct = CType(Dgv.DataSource, DataTable).DefaultView.ToTable(True, VALIDATECOLNAME, "CHECK")
            Dim drDistinct As DataRow()
            drDistinct = dtDistinct.Select("CHECK = TRUE")
            If drDistinct.Length > 1 Then
                MsgBox("Please select unique " & VALIDATECOLNAME & " record.", MsgBoxStyle.Information)
                Dgv.Focus()
                Exit Sub
            End If
        End If
        RowSelected = DtGrid.Select("CHECK = TRUE")
        If Not RowSelected.Length > 0 Then
            MsgBox("There is no record selected", MsgBoxStyle.Information)
            Dgv.Focus()
            Exit Sub
        End If
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub Dgv_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Dgv.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
        Else
            If e.Control Then
                If e.KeyCode = Keys.F Then
                    objSearch.Show()
                    'Dim objSearch As New frmGridSearch(gridView_OWN)
                    'objSearch.Show()
                ElseIf e.KeyCode = Keys.S Then
                    If objSearch IsNot Nothing Then
                        CType(objSearch, frmGridSearch).btnFindNext_Click(Me, New EventArgs)
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub Dgv_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Dgv.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            e.Handled = True
            If Not Dgv.RowCount > 0 Then Exit Sub
            If Dgv.CurrentRow Is Nothing Then Exit Sub
            Dgv.CurrentCell = Dgv.Rows(Dgv.CurrentRow.Index).Cells(Dgv.FirstDisplayedCell.ColumnIndex)
            btnOk.Select()
        End If
    End Sub

    Private Sub Dgv_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Dgv.SelectionChanged
        If Dgv.CurrentRow Is Nothing Then Exit Sub
        Dgv.CurrentCell = Dgv.Rows(Dgv.CurrentRow.Index).Cells(Dgv.FirstDisplayedCell.ColumnIndex)
    End Sub

    Private Sub Dgv_CurrentCellDirtyStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Dgv.CurrentCellDirtyStateChanged
        'If DateCheck = True Then
        '    If Dgv.Columns.Contains("BILLDATE") = True Then
        '        Dim dr As DataRow()
        '        dr = CType(Dgv.DataSource, DataTable).Select("BILLDATE ='" & Dgv.CurrentRow.Cells("BILLDATE").Value.ToString() & "' AND CHECK = TRUE", "BILLDATE")
        '        Dim drCheck As DataRow()
        '        drCheck = CType(Dgv.DataSource, DataTable).Select("CHECK = TRUE")
        '        If drCheck.Length > 0 Then
        '            If dr.Length = drCheck.Length Then
        '                Dgv.CommitEdit(DataGridViewDataErrorContexts.Commit)
        '            Else
        '            End If
        '        Else
        '            Dgv.CommitEdit(DataGridViewDataErrorContexts.Commit)
        '        End If
        '    End If
        'Else
        Dgv.CommitEdit(DataGridViewDataErrorContexts.Commit)
        'End If
    End Sub

    Private Sub MultiSelectRowDia_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.Control Then
            If e.KeyCode = Keys.A Then
                chkSelectAll.Checked = Not chkSelectAll.Checked
            End If
        End If
    End Sub

    Private Sub MultiSelectRowDia_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        chkSelectAll.Text = ""
        chkSelectAll.Location = New Point(Dgv.Location.X + 5, Dgv.Location.Y + 5)
        lblTotPcs.Visible = False
        lblTotGrswt.Visible = False
        lblTotNetwt.Visible = False
        Dgv.Select()
    End Sub

    Private Sub chkSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSelectAll.CheckedChanged
        If DateCheck = True Then
            'If Dgv.Columns.Contains("BILLDATE") = True Then
            For Each dgvCol As DataGridViewRow In Dgv.Rows
                dgvCol.Cells("CHECK").Value = chkSelectAll.Checked
            Next
            'Dim dtDistinct As DataTable
            'dtDistinct = CType(Dgv.DataSource, DataTable).DefaultView.ToTable(True, "BILLDATE")
            'If dtDistinct.Rows.Count = 1 Then
            '    For Each dgvCol As DataGridViewRow In Dgv.Rows
            '        dgvCol.Cells("CHECK").Value = chkSelectAll.Checked
            '    Next
            'Else
            '    For Each dgvCol As DataGridViewRow In Dgv.Rows
            '        dgvCol.Cells("CHECK").Value = False
            '    Next
            'End If
            'End If
        Else
            For Each dgvCol As DataGridViewRow In Dgv.Rows
                dgvCol.Cells("CHECK").Value = chkSelectAll.Checked
            Next
        End If
    End Sub
    Private Sub frmBankReconciliation_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If objSearch IsNot Nothing Then
            objSearch = Nothing
        End If
    End Sub

    Private Sub txtSearchTagNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtSearchTagNo.KeyDown
        If e.KeyCode = Keys.Enter Then
            If txtSearchTagNo.Text = "" Then Exit Sub
            For cnt As Integer = 0 To DtGrid.Rows.Count - 1
                If txtSearchItem.Text <> "" Then
                    If DtGrid.Rows(cnt).Item("ITEMID").ToString <> txtSearchItem.Text Then Continue For
                End If
                If DtGrid.Rows(cnt).Item("TAGNO").ToString = txtSearchTagNo.Text Then
                    DtGrid.Rows(cnt).Item("CHECK") = True
                    Dgv.CurrentCell = Dgv.Rows(cnt).Cells("CHECK")
                    txtSearchTagNo.Clear()
                    txtSearchItem.Clear()
                    Exit Sub
                End If
            Next
            MsgBox("TagNo not found", MsgBoxStyle.Information)
            txtSearchTagNo.Select()
            txtSearchTagNo.SelectAll()
            txtSearchItem.Select()
            txtSearchItem.SelectAll()
        End If
    End Sub

    Private Sub txtSearchTagNo_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSearchTagNo.TextChanged

    End Sub

    Private Sub txtSearchItem_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtSearchItem.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            'Dim barcode2d() As String = txtSAItemId.Text.Split(objSoftKeys.BARCODE2DSEP)
            'If barcode2d.Length > 2 Then
            '    If Not Barcode2dfound(txtSAItemId.Text) Then Call Barcode2ddetails(txtSAItemId.Text) : Exit Sub
            '    GoTo LoadItemInfo
            'End If
            'If barcode2d.Length > 2 Then Call Barcode2ddetails(txtSAItemId.Text) : Exit Sub
            Dim TAGSEP As Char = GetAdmindbSoftValue("PRODTAGSEP", "")
            Dim sp() As String = txtSearchItem.Text.Split(TAGSEP)
            Dim ScanStr As String = txtSearchItem.Text
            If TAGSEP <> "" And txtSearchItem.Text <> "" Then
                sp = txtSearchItem.Text.Split(TAGSEP)
                txtSearchItem.Text = Trim(sp(0))
            End If
            Dim STRSQL As String
            Dim dtItemDet As New DataTable
            If txtSearchItem.Text.StartsWith("#") Then txtSearchItem.Text = txtSearchItem.Text.Remove(0, 1)
CheckItem:

            If IsNumeric(ScanStr) = False And ScanStr.Contains(TAGSEP) = False Then
                STRSQL = " SELECT ITEMID,TAGNO FROM " & cnAdminDb & "..ITEMTAG WHERE TAGKEY = '" & Trim(ScanStr) & "'"
                dtItemDet = New DataTable
                da = New OleDbDataAdapter(STRSQL, cn)
                da.Fill(dtItemDet)
                If dtItemDet.Rows.Count > 0 Then
                    txtSearchItem.Text = dtItemDet.Rows(0).Item("ITEMID").ToString
                    txtSearchTagNo.Text = dtItemDet.Rows(0).Item("TAGNO").ToString
                    GoTo LoadItemInfo
                End If

            ElseIf IsNumeric(ScanStr) = True And ScanStr.Contains(TAGSEP) = False And objGPack.DupCheck("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE CONVERT(VARCHAR,ITEMID) = '" & Val(ScanStr) & "'" & GetItemQryFilteration()) = True Then
                txtSearchTagNo.Select()
                txtSearchTagNo.Focus()
                Exit Sub
            ElseIf TAGSEP <> "" Then
                STRSQL = " SELECT ITEMID,TAGNO FROM " & cnAdminDb & "..ITEMTAG WHERE TAGKEY = '" & Trim(ScanStr).Replace(TAGSEP, "") & "'"
                dtItemDet = New DataTable
                da = New OleDbDataAdapter(STRSQL, cn)
                da.Fill(dtItemDet)
                If dtItemDet.Rows.Count > 0 Then
                    txtSearchItem.Text = dtItemDet.Rows(0).Item("ITEMID").ToString
                    txtSearchTagNo.Text = dtItemDet.Rows(0).Item("TAGNO").ToString
                    GoTo LoadItemInfo
                End If
            ElseIf txtSearchItem.Text <> "" And objGPack.DupCheck("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE CONVERT(VARCHAR,ITEMID) = '" & Trim(txtSearchItem.Text) & "'" & GetItemQryFilteration()) = False Then
                STRSQL = " SELECT ITEMID,TAGNO FROM " & cnAdminDb & "..ITEMTAG WHERE TAGKEY = '" & Trim(txtSearchItem.Text) & "'"
                dtItemDet = New DataTable
                da = New OleDbDataAdapter(STRSQL, cn)
                da.Fill(dtItemDet)
                If dtItemDet.Rows.Count > 0 Then
                    txtSearchItem.Text = dtItemDet.Rows(0).Item("ITEMID").ToString
                    txtSearchTagNo.Text = dtItemDet.Rows(0).Item("TAGNO").ToString
                    GoTo LoadItemInfo
                Else
                    'LoadSalesItemName()
                    txtSearchTagNo.Select()
                    txtSearchTagNo.Focus()
                    Exit Sub
                End If
            Else
LoadItemInfo:
                '  LoadSalesItemNameDetail()
            End If
            If sp.Length > 1 And TAGSEP <> "" And txtSearchItem.Text <> "" Then
                txtSearchTagNo.Text = Trim(sp(1))
            End If
            If txtSearchTagNo.Text <> "" Then
                txtSearchItem.Focus()
                txtSearchTagNo_KeyDown(Me, New KeyEventArgs(Keys.Enter))
            End If
        End If

    End Sub

    Private Sub Dgv_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles Dgv.CellValueChanged
        lblTotPcs.Visible = False
        lblTotGrswt.Visible = False
        lblTotNetwt.Visible = False
        If Dgv.Rows.Count > 0 Then
            Dim _tPcs As Integer = 0
            Dim _tGwt As Decimal = 0
            Dim _tNwt As Decimal = 0

            For cnt As Integer = 0 To DtGrid.Rows.Count - 1
                With DtGrid.Rows(cnt)
                    If .Item("CHECK").ToString.ToUpper = "TRUE" Then
                        If Dgv.Columns.Contains("PCS") Then
                            _tPcs += Val(.Item("Pcs").ToString())
                        End If
                        If Dgv.Columns.Contains("GRSWT") Then
                            _tGwt += Val(.Item("GrsWt").ToString())
                        End If
                        If Dgv.Columns.Contains("NETWT") Then
                            _tNwt += Val(.Item("NetWt").ToString())
                        End If
                    End If
                End With
            Next

            If Val(_tPcs) > 0 Then
                lblTotPcs.Visible = True
                lblTotPcs.Text = "TotPcs : " & _tPcs
            End If
            If Val(_tGwt) > 0 Then
                lblTotGrswt.Visible = True
                lblTotGrswt.Text = "TotGwt : " & Format(_tGwt, "0.000")
            End If
            If Val(_tNwt) > 0 Then
                lblTotNetwt.Visible = True
                lblTotNetwt.Text = "TotNwt : " & Format(_tNwt, "0.000")
            End If

        End If
    End Sub
End Class