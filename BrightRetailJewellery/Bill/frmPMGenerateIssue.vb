Imports System.Data.OleDb
Public Class frmPMGenerateIssue
    Dim MBillno As Integer
    Dim MBatchno As String = ""
    Dim Strsql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim _tran As OleDbTransaction = Nothing
    Public DtPmtran As New DataTable
    Dim FromBill As Boolean = False
    Dim COMPL_ISS_MODE As String = GetAdmindbSoftValue("COMPL_ISS_MODE", "N")
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        
    End Sub
    Public Sub New(ByVal DtTable As DataTable)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        GridView.DataSource = Nothing
        DtPmtran = DtTable
        If DtPmtran.Rows.Count > 0 Then
            GridView.DataSource = DtPmtran
            LoadCombo()
            FromBill = True
            gridStyle()
        End If
    End Sub


    Private Sub frmPMGenerateIssue_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        'If e.KeyChar = Chr(Keys.Enter) Then
        '    If GridView.Focused Then Exit Sub
        '    If cmbPMType.Focused Then Exit Sub
        '    If txtGrid.Focused Then Exit Sub
        '    SendKeys.Send("{TAB}")
        'End If
    End Sub

    Private Sub PMGenerate_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If MBillno = 0 Then Me.Close()
        If FromBill = False Then
            txtBillNo.Focus()
        Else
            GridView.Focus()
            GridView.Rows(0).Cells(0).Selected = True
        End If
    End Sub
    Private Function LoadBillDetail()
        GridView.DataSource = Nothing
        Strsql = "SELECT IM.ITEMNAME,I.PCS,PM.PMNAME,PS.PMSUBNAME,ISNULL(P.PCS,1) QTY,I.AMOUNT,I.TRANDATE,I.TRANNO,I.BATCHNO FROM " & cnStockDb & "..ISSUE I"
        Strsql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON I.ITEMID=IM.ITEMID"
        Strsql += vbCrLf + " LEFT JOIN " & cnStockDb & "..PMTRAN P ON P.TRANNO=I.TRANNO AND P.TRANDATE=I.TRANDATE AND P.AMOUNT=I.AMOUNT"
        Strsql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..PMMAST PM ON P.PMID=PM.PMID "
        Strsql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..PMSUBMAST PS ON P.PMSUBID=PS.PMSUBID  AND P.PMID=PS.PMID"
        Strsql += vbCrLf + " WHERE ISNULL(I.CANCEL,'') = '' AND I.TRANNO=" & MBillno & " AND I.TRANTYPE='SA'"
        Dim Dtgrid As New DataTable
        cmd = New OleDbCommand(Strsql, cn, _tran)
        da = New OleDbDataAdapter(cmd)
        da.Fill(Dtgrid)
        If Dtgrid.Rows.Count > 0 Then
            GridView.DataSource = Dtgrid
            gridStyle()
            GridView.Focus()
            GridView.Rows(0).Cells(0).Selected = True
        End If
    End Function
    Function gridStyle()
        If GridView.Columns.Contains("PMSUBNAME") Then GridView.Columns("PMSUBNAME").Visible = True

        'If GridView.Columns.Contains("PCS") Then GridView.Columns("PCS").Width = 50
        'If GridView.Columns.Contains("PMNAME") Then GridView.Columns("PMNAME").Width = 150
        'If GridView.Columns.Contains("QTY") Then GridView.Columns("QTY").Width = 50
        'If GridView.Columns.Contains("AMOUNT") Then GridView.Columns("AMOUNT").Width = 80 : GridView.Columns("AMOUNT").Visible = False
        If COMPL_ISS_MODE <> "A" Then
            If GridView.Columns.Contains("ITEMNAME") Then GridView.Columns("ITEMNAME").Width = 150 : GridView.Columns("ITEMNAME").ReadOnly = True
            If GridView.Columns.Contains("TRANDATE") Then GridView.Columns("TRANDATE").Visible = False
            If GridView.Columns.Contains("TRANNO") Then GridView.Columns("TRANNO").Visible = False
            If GridView.Columns.Contains("BATCHNO") Then GridView.Columns("BATCHNO").Visible = False
            If GridView.Columns.Contains("TRANNO") Then GridView.Columns("TRANNO").Visible = False
            If GridView.Columns.Contains("GRSWT") Then GridView.Columns("GRSWT").Visible = False
        End If
        GridView.SelectionMode = DataGridViewSelectionMode.CellSelect
        GridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    End Function
    Private Function LoadCombo()
        cmbPMType.Items.Clear()
        Strsql = "SELECT PMNAME FROM " & cnAdminDb & "..PMMAST WHERE ISNULL(ACTIVE,'')<>'N'"
        Strsql += " ORDER BY PMNAME"
        objGPack.FillCombo(Strsql, cmbPMType, True, False)
    End Function

    Private Sub GridView_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles GridView.CellEnter
        Select Case GridView.Columns(e.ColumnIndex).Name
            Case "PMNAME"
                GridView.CurrentCell = GridView.Rows(GridView.CurrentRow.Index).Cells("PMNAME")
                Dim pt As Point = GridView.Location
                cmbPMType.Visible = True
                pt = pt + GridView.GetCellDisplayRectangle(GridView.Columns("PMNAME").Index, GridView.CurrentRow.Index, False).Location
                cmbPMType.Text = GridView.Rows(GridView.CurrentRow.Index).Cells("PMNAME").Value.ToString
                cmbPMType.Location = pt
                cmbPMType.Width = GridView.Columns("PMNAME").Width
                'cmbPMType.SelectedIndex = 0
                cmbPMType.Focus()
            Case "PMSUBNAME"
                GridView.CurrentCell = GridView.Rows(GridView.CurrentRow.Index).Cells("PMSUBNAME")
                Dim pst As Point = GridView.Location
                cmbPMSubType.Visible = True
                pst = pst + GridView.GetCellDisplayRectangle(GridView.Columns("PMSUBNAME").Index, GridView.CurrentRow.Index, False).Location
                cmbPMSubType.Text = GridView.Rows(GridView.CurrentRow.Index).Cells("PMSUBNAME").Value.ToString
                cmbPMSubType.Location = pst
                cmbPMSubType.Width = GridView.Columns("PMSUBNAME").Width
                'cmbPMType.SelectedIndex = 0
                cmbPMSubType.Focus()
            Case "QTY"
                GridView.CurrentCell = GridView.Rows(GridView.CurrentRow.Index).Cells("QTY")
                Dim pt As Point = GridView.Location
                txtGrid.Visible = True
                pt = pt + GridView.GetCellDisplayRectangle(GridView.Columns("QTY").Index, GridView.CurrentRow.Index, False).Location
                txtGrid.Text = GridView.Rows(GridView.CurrentRow.Index).Cells("QTY").Value.ToString
                txtGrid.Location = pt
                txtGrid.Width = GridView.Columns("QTY").Width
                txtGrid.Focus()
        End Select
    End Sub


    Private Sub GridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles GridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
        ElseIf e.KeyCode = Keys.Escape Then
            btnOk.Focus()
        End If
    End Sub

    Private Sub cmbPMType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbPMType.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If GridView.Columns(GridView.CurrentCell.ColumnIndex).Name = "PMNAME" Then
                If cmbPMType.Text <> "" And cmbPMType.Text <> "ALL" Then
                    Strsql = " SELECT PMSUBNAME FROM " & cnAdminDb & "..PMSUBMAST WHERE PMID IN (SELECT PMID FROM  " & cnAdminDb & "..PMMAST WHERE PMNAME='" & cmbPMType.Text.ToString & "') "
                    Strsql += " ORDER BY PMSUBNAME"
                    objGPack.FillCombo(Strsql, cmbPMSubType, True, False)
                Else
                    cmbPMSubType.Items.Clear()
                    cmbPMSubType.Items.Add("All")
                End If
                Dim pt As Point = GridView.Location
                GridView.CurrentRow.Cells("PMNAME").Value = cmbPMType.Text
                'dtGridView.AcceptChanges()
                cmbPMType.Visible = False
                GridView.CurrentRow.Cells("PMSUBNAME").Selected = True
                'GridView.Focus()
            End If
                Exit Sub
            ElseIf e.KeyChar = Chr(Keys.Escape) Then
                cmbPMType.Visible = False : btnOk.Focus()
            End If
    End Sub

    Private Sub cmbPMType_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbPMType.Leave
        cmbPMType.Visible = False
        'cmbPMType_KeyPress(Me, New KeyPressEventArgs(Chr(Keys.Enter)))
    End Sub

    Private Sub txtGrid_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtGrid.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If GridView.Columns(GridView.CurrentCell.ColumnIndex).Name = "QTY" Then
                Dim pt As Point = GridView.Location
                GridView.CurrentRow.Cells("QTY").Value = Val(txtGrid.Text)
                GridView.CurrentRow.Selected = True
                txtGrid.Visible = False
                If GridView.CurrentRow.Index = GridView.RowCount - 1 Then
                    btnOk.Focus()
                Else
                    GridView.Rows(GridView.CurrentRow.Index + 1).Cells("PMNAME").Selected = True
                    'GridView.Focus()
                End If
                'GridView.Focus()
                Exit Sub
            End If
        ElseIf e.KeyChar = Chr(Keys.Escape) Then
            txtGrid.Visible = False : btnOk.Focus()
        End If
    End Sub

    Private Sub txtGrid_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtGrid.Leave
        txtGrid.Visible = False
    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        If GridView.RowCount = 0 Then Me.DialogResult = Windows.Forms.DialogResult.Cancel : Exit Sub
        If FromBill = True Then Me.DialogResult = Windows.Forms.DialogResult.OK : Exit Sub
        If MessageBox.Show("Do you want to save following packing material.", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
            InsertintoPmtran()
        End If
        Me.Close()
    End Sub
    Public Function InsertintoPmtran()
        Dim Pmid As Integer = 0
        Dim Pmsubid As Integer = 0
        If MBatchno = "" Then MBatchno = GridView.Rows(0).Cells("BATCHNO").Value.ToString
        If Val(objGPack.GetSqlValue("SELECT COUNT(*) FROM " & cnStockDb & "..PMTRAN WHERE BATCHNO='" & MBatchno & "' AND TRANNO='" & Val(GridView.Rows(0).Cells("TRANNO").Value.ToString) & "'", , "", _tran)) > 0 Then
            Strsql = "DELETE FROM " & cnStockDb & "..PMTRAN WHERE BATCHNO='" & MBatchno & "' AND TRANNO='" & Val(GridView.Rows(0).Cells("TRANNO").Value.ToString) & "'"
            ExecQuery(SyncMode.Transaction, Strsql, cn, _tran, cnCostId)
        End If

        For m As Integer = 0 To GridView.RowCount - 1
            Pmid = Val(objGPack.GetSqlValue("SELECT PMID FROM " & cnAdminDb & "..PMMAST WHERE PMNAME ='" & GridView.Rows(m).Cells("PMNAME").Value.ToString & "'", , "", _tran))
            Pmsubid = Val(objGPack.GetSqlValue("SELECT PMSUBID FROM " & cnAdminDb & "..PMSUBMAST WHERE PMSUBNAME ='" & GridView.Rows(m).Cells("PMSUBNAME").Value.ToString & "' AND PMID=" & Pmid & "", , "", _tran))
            Strsql = " INSERT INTO " & cnStockDb & "..PMTRAN "
            Strsql += vbCrLf + " (SNO,PMID,PMSUBID,TRANDATE,TRANNO,TRANTYPE,PCS,AMOUNT,BATCHNO,COSTID,REMARK1,REMARK2"
            Strsql += vbCrLf + " ,CANCEL,FROMFLAG,USERID,UPDATED,UPTIME) VALUES("
            Strsql += vbCrLf + " '" & GetNewSno(TranSnoType.PMTRANCODE, tran) & "'" 'SNO
            Strsql += vbCrLf + " ," & Pmid & "" 'PMID
            Strsql += vbCrLf + " ," & Pmsubid & "" 'PMSUBID
            Strsql += vbCrLf + " ,'" & Format(GridView.Rows(m).Cells("TRANDATE").Value, "yyyy-MM-dd") & "'" 'TRANDATE
            Strsql += vbCrLf + " ,'" & Val(GridView.Rows(m).Cells("TRANNO").Value.ToString) & "'" 'TRANNO
            Strsql += vbCrLf + " ,''" 'TRANTYPE
            Strsql += vbCrLf + " ," & Val(GridView.Rows(m).Cells("QTY").Value.ToString) & "" 'PCS
            Strsql += vbCrLf + " ," & Val(GridView.Rows(m).Cells("AMOUNT").Value.ToString) & "" 'AMOUNT
            Strsql += vbCrLf + " ,'" & MBatchno & "'" 'BATCHNO
            Strsql += vbCrLf + " ,'" & cnCostId & "'" 'COSTID
            Strsql += vbCrLf + " ,''" 'REMARK1
            Strsql += vbCrLf + " ,''" 'REMARK2
            Strsql += vbCrLf + " ,NULL" 'CANCEL"
            Strsql += vbCrLf + " ,'P'" 'FROMFLAG
            Strsql += vbCrLf + " ,'" & userId & "'" 'USERID
            Strsql += vbCrLf + " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
            Strsql += vbCrLf + " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
            Strsql += vbCrLf + " )"
            ExecQuery(SyncMode.Transaction, Strsql, cn, _tran, cnCostId)
        Next
    End Function

    Private Sub txtBillNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtBillNo.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            MBillno = Val(txtBillNo.Text)
            LoadCombo()
            LoadBillDetail()
        End If
    End Sub

    Private Sub cmbPMSubType_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbPMSubType.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            GridView.CurrentRow.Cells("QTY").Selected = True
        End If
    End Sub
End Class