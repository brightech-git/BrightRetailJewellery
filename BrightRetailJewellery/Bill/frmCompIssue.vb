Imports System.Data.OleDb
Public Class frmCompIssue
    Dim MBillno As Integer
    Dim MBatchno As String = ""
    Dim Strsql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim _tran As OleDbTransaction = Nothing
    Public DtPmtran As New DataTable
    Dim FromBill As Boolean = False
    Dim COMPL_ISS_MODE As String = GetAdmindbSoftValue("COMPL_ISS_MODE", "N")
    Public dtitems As New DataTable

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
    Public Sub New(ByVal DtTable As DataTable)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        GridView_OWN.DataSource = Nothing
        DtPmtran = DtTable
        If DtPmtran.Rows.Count > 0 Then
            GridView_OWN.DataSource = DtPmtran
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
        If e.KeyChar = Chr(Keys.Enter) Then
            If btnAdd.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub PMGenerate_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If MBillno = 0 Then Me.Close()
        txtBillNo.Focus()

        Strsql = " SELECT CONVERT(VARCHAR(150),NULL) AS ITEM,CONVERT(VARCHAR(150),NULL) AS SUBITEM,CONVERT(INT,NULL) AS PCS FROM " & cnAdminDb & "..ITEMMAST WHERE 1<>1"
        'Dim dtitems As New DataTable
        cmd = New OleDbCommand(Strsql, cn, _tran)
        da = New OleDbDataAdapter(cmd)
        da.Fill(dtitems)

        'gridItems.DataSource = Nothing
        gridItems_OWN.DataSource = dtitems
        gridItems_OWN.Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        'If FromBill = False Then
        '    txtBillNo.Focus()
        'Else
        '    GridView.Focus()
        '    GridView.Rows(0).Cells(0).Selected = True
        'End If
        btnClear_Click(Me, e)
    End Sub
    Private Function LoadBillDetail()
        GridView_OWN.DataSource = Nothing
        Strsql = "SELECT CONVERT(VARCHAR(12),I.TRANDATE,105)TRANDATE,I.TRANNO,IM.ITEMNAME,I.PCS,I.AMOUNT,I.BATCHNO FROM " & cnStockDb & "..ISSUE I"
        Strsql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON I.ITEMID=IM.ITEMID"
        Strsql += vbCrLf + " LEFT JOIN " & cnStockDb & "..PMTRAN P ON P.TRANNO=I.TRANNO AND P.TRANDATE=I.TRANDATE AND P.AMOUNT=I.AMOUNT"
        Strsql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST PM ON P.PMID=PM.ITEMID "
        Strsql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST PS ON P.PMSUBID=PS.SUBITEMID  AND P.PMID=PS.ITEMID"
        Strsql += vbCrLf + " WHERE ISNULL(I.CANCEL,'') = '' AND I.TRANNO=" & MBillno & " AND I.TRANTYPE='SA'"
        Dim Dtgrid As New DataTable
        cmd = New OleDbCommand(Strsql, cn, _tran)
        da = New OleDbDataAdapter(cmd)
        da.Fill(Dtgrid)
        MBatchno = ""
        If Dtgrid.Rows.Count > 0 Then
            GridView_OWN.DataSource = Dtgrid
            'gridStyle()
            'GridView.Focus()
            'GridView.Rows(0).Cells(0).Selected = True
            'GridView.Rows(0).Cells(0).Selected = True
            MBatchno = GridView_OWN.Rows(0).Cells("BATCHNO").Value.ToString
            GridView_OWN.Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            GridView_OWN.Columns("TRANNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            GridView_OWN.Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            GridView_OWN.Columns("BATCHNO").Visible = False
            GridView_OWN.Columns("ITEMNAME").Width = 180
            GridView_OWN.Columns("PCS").Width = 35
            gridItems_OWN.Columns("PCS").Width = 35
            For Each col As DataGridViewColumn In GridView_OWN.Columns
                col.SortMode = DataGridViewColumnSortMode.NotSortable
            Next
            For Each col As DataGridViewColumn In gridItems_OWN.Columns
                col.SortMode = DataGridViewColumnSortMode.NotSortable
            Next
        End If
    End Function
    Function gridStyle()
        If GridView_OWN.Columns.Contains("SUBITEMNAME") Then GridView_OWN.Columns("SUBITEMNAME").Visible = True

        'If GridView.Columns.Contains("PCS") Then GridView.Columns("PCS").Width = 50
        'If GridView.Columns.Contains("ITEMNAME") Then GridView.Columns("ITEMNAME").Width = 150
        'If GridView.Columns.Contains("QTY") Then GridView.Columns("QTY").Width = 50
        'If GridView.Columns.Contains("AMOUNT") Then GridView.Columns("AMOUNT").Width = 80 : GridView.Columns("AMOUNT").Visible = False
        If COMPL_ISS_MODE <> "A" Then
            If GridView_OWN.Columns.Contains("ITEMNAME") Then GridView_OWN.Columns("ITEMNAME").Width = 150 : GridView_OWN.Columns("ITEMNAME").ReadOnly = True
            If GridView_OWN.Columns.Contains("TRANDATE") Then GridView_OWN.Columns("TRANDATE").Visible = False
            If GridView_OWN.Columns.Contains("TRANNO") Then GridView_OWN.Columns("TRANNO").Visible = False
            If GridView_OWN.Columns.Contains("BATCHNO") Then GridView_OWN.Columns("BATCHNO").Visible = False
            If GridView_OWN.Columns.Contains("TRANNO") Then GridView_OWN.Columns("TRANNO").Visible = False
            If GridView_OWN.Columns.Contains("GRSWT") Then GridView_OWN.Columns("GRSWT").Visible = False
        End If
        GridView_OWN.SelectionMode = DataGridViewSelectionMode.CellSelect
        GridView_OWN.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    End Function
    Private Function LoadCombo()
        cmbPMType.Items.Clear()
        Strsql = "SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ACTIVE,'')<>'N' AND ISNULL(COMPLIMENTS,'')='Y'"
        Strsql += " ORDER BY ITEMNAME"
        objGPack.FillCombo(Strsql, cmbPMType, True, False)
    End Function

    Private Sub GridView_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles GridView_OWN.CellEnter
        'Select Case GridView.Columns(e.ColumnIndex).Name
        '    Case "ITEMNAME"
        '        GridView.CurrentCell = GridView.Rows(GridView.CurrentRow.Index).Cells("ITEMNAME")
        '        Dim pt As Point = GridView.Location
        '        cmbPMType.Visible = True
        '        pt = pt + GridView.GetCellDisplayRectangle(GridView.Columns("ITEMNAME").Index, GridView.CurrentRow.Index, False).Location
        '        cmbPMType.Text = GridView.Rows(GridView.CurrentRow.Index).Cells("ITEMNAME").Value.ToString
        '        cmbPMType.Location = pt
        '        cmbPMType.Width = GridView.Columns("ITEMNAME").Width
        '        'cmbPMType.SelectedIndex = 0
        '        cmbPMType.Focus()
        '    Case "SUBITEMNAME"
        '        GridView.CurrentCell = GridView.Rows(GridView.CurrentRow.Index).Cells("SUBITEMNAME")
        '        Dim pst As Point = GridView.Location
        '        cmbPMSubType.Visible = True
        '        pst = pst + GridView.GetCellDisplayRectangle(GridView.Columns("SUBITEMNAME").Index, GridView.CurrentRow.Index, False).Location
        '        cmbPMSubType.Text = GridView.Rows(GridView.CurrentRow.Index).Cells("SUBITEMNAME").Value.ToString
        '        cmbPMSubType.Location = pst
        '        cmbPMSubType.Width = GridView.Columns("SUBITEMNAME").Width
        '        'cmbPMType.SelectedIndex = 0
        '        cmbPMSubType.Focus()
        '    Case "QTY"
        '        GridView.CurrentCell = GridView.Rows(GridView.CurrentRow.Index).Cells("QTY")
        '        Dim pt As Point = GridView.Location
        '        txtGrid.Visible = True
        '        pt = pt + GridView.GetCellDisplayRectangle(GridView.Columns("QTY").Index, GridView.CurrentRow.Index, False).Location
        '        txtGrid.Text = GridView.Rows(GridView.CurrentRow.Index).Cells("QTY").Value.ToString
        '        txtGrid.Location = pt
        '        txtGrid.Width = GridView.Columns("QTY").Width
        '        txtGrid.Focus()
        'End Select
    End Sub


    Private Sub GridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles GridView_OWN.KeyDown
        'If e.KeyCode = Keys.Enter Then
        '    e.Handled = True
        'ElseIf e.KeyCode = Keys.Escape Then
        '    btnOk.Focus()
        'End If
    End Sub

    Private Sub cmbPMType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbPMType.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            'If GridView.Columns(GridView.CurrentCell.ColumnIndex).Name = "ITEMNAME" Then
            '    If cmbPMType.Text <> "" And cmbPMType.Text <> "ALL" Then
            '        Strsql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID IN (SELECT ITEMID FROM  " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbPMType.Text.ToString & "') "
            '        Strsql += " ORDER BY SUBITEMNAME"
            '        objGPack.FillCombo(Strsql, cmbPMSubType, True, False)
            '    Else
            '        cmbPMSubType.Items.Clear()
            '        cmbPMSubType.Items.Add("All")
            '    End If
            '    Dim pt As Point = GridView.Location
            '    GridView.CurrentRow.Cells("ITEMNAME").Value = cmbPMType.Text
            '    'dtGridView.AcceptChanges()
            '    cmbPMType.Visible = True
            '    GridView.CurrentRow.Cells("SUBITEMNAME").Selected = True
            '    'GridView.Focus()
            'End If
            Strsql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID IN (SELECT ITEMID FROM  " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbPMType.Text.ToString & "') "
            Strsql += " ORDER BY SUBITEMNAME"
            objGPack.FillCombo(Strsql, cmbPMSubType, True, False)
            Exit Sub
        ElseIf e.KeyChar = Chr(Keys.Escape) Then
            cmbPMType.Visible = True : btnOk.Focus()
        End If
    End Sub

    Private Sub cmbPMType_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbPMType.Leave
        cmbPMType.Visible = True
        'cmbPMType_KeyPress(Me, New KeyPressEventArgs(Chr(Keys.Enter)))
    End Sub

    Private Sub txtGrid_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtGrid.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If GridView_OWN.Columns(GridView_OWN.CurrentCell.ColumnIndex).Name = "QTY" Then
                Dim pt As Point = GridView_OWN.Location
                GridView_OWN.CurrentRow.Cells("QTY").Value = Val(txtGrid.Text)
                GridView_OWN.CurrentRow.Selected = True
                txtGrid.Visible = False
                If GridView_OWN.CurrentRow.Index = GridView_OWN.RowCount - 1 Then
                    btnOk.Focus()
                Else
                    GridView_OWN.Rows(GridView_OWN.CurrentRow.Index + 1).Cells("ITEMNAME").Selected = True
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
        If GridView_OWN.RowCount = 0 Then Me.DialogResult = Windows.Forms.DialogResult.Cancel : Exit Sub
        If FromBill = True Then Me.DialogResult = Windows.Forms.DialogResult.OK : Exit Sub

        If gridItems_OWN.Rows.Count = 0 Then
            MsgBox("No records to save.")
            Exit Sub
        End If

        If Val(objGPack.GetSqlValue("SELECT COUNT(*) FROM " & cnStockDb & "..PMTRAN WHERE BATCHNO='" & MBatchno & "' ", , "", _tran)) > 0 Then
            MsgBox("Already Issued.")
            Exit Sub
        End If

        'If MessageBox.Show("Do you want to save following packing material.", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
        '    InsertintoPmtran()
        'End If

        Try
            _tran = cn.BeginTransaction
            InsertintoPmtran()
            _tran.Commit()
            _tran.Dispose()
            MsgBox("Saved.")
            btnClear_Click(Me, e)
        Catch ex As Exception
            If Not _tran Is Nothing Then
                _tran.Rollback()
                _tran.Dispose()
            End If
            MsgBox(ex.Message)
        End Try
        Exit Sub
        btnClear_Click(Me, e)
        'Me.Close()
    End Sub
    Public Function InsertintoPmtran()
        Dim Pmid As Integer = 0
        Dim Pmsubid As Integer = 0

        Dim TranNo As String = "0"
        Dim tranDate As String = ""

        If MBatchno = "" Then MBatchno = GridView_OWN.Rows(0).Cells("BATCHNO").Value.ToString
        TranNo = GridView_OWN.Rows(0).Cells("TRANNO").Value.ToString
        tranDate = Format(GridView_OWN.Rows(0).Cells("TRANDATE").Value, "yyyy-MM-dd")



        For m As Integer = 0 To gridItems_OWN.RowCount - 1
            Pmid = Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME ='" & gridItems_OWN.Rows(m).Cells("ITEM").Value.ToString & "'", , "", _tran))
            Pmsubid = Val(objGPack.GetSqlValue("SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME ='" & gridItems_OWN.Rows(m).Cells("SUBITEM").Value.ToString & "' AND ITEMID=" & Pmid & "", , "", _tran))
            Strsql = " INSERT INTO " & cnStockDb & "..PMTRAN "
            Strsql += vbCrLf + " (SNO,PMID,PMSUBID,TRANDATE,TRANNO,TRANTYPE,PCS,AMOUNT,BATCHNO,COSTID,REMARK1,REMARK2"
            Strsql += vbCrLf + " ,CANCEL,FROMFLAG,USERID,UPDATED,UPTIME) VALUES("
            Strsql += vbCrLf + " '" & GetNewSno(TranSnoType.PMTRANCODE, _tran) & "'" 'SNO
            Strsql += vbCrLf + " ," & Pmid & "" 'PMID
            Strsql += vbCrLf + " ," & Pmsubid & "" 'PMSUBID
            Strsql += vbCrLf + " ,'" & GetEntryDate(DateTime.Now.Date, _tran).ToString("yyyy-MM-dd") & "'" 'TRANDATE
            Strsql += vbCrLf + " ,'" & Val(TranNo) & "'" 'TRANNO
            Strsql += vbCrLf + " ,''" 'TRANTYPE
            Strsql += vbCrLf + " ," & Val(1) & "" 'PCS
            Strsql += vbCrLf + " ," & Val(0) & "" 'AMOUNT
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


            Dim tagSno As String = GetNewSno(TranSnoType.ITEMNONTAGCODE, _tran, "GET_ADMINSNO_TRAN")
            Strsql = " INSERT INTO " & cnAdminDb & "..ITEMNONTAG"
            Strsql += " ("
            Strsql += " SNO,ITEMID,SUBITEMID,COMPANYID,RECDATE,"
            Strsql += " PCS,GRSWT,LESSWT,NETWT,"
            Strsql += " FINRATE,ISSTYPE,RECISS,POSTED,"
            Strsql += " PACKETNO,DREFNO,ITEMCTRID,"
            Strsql += " ORDREPNO,ORSNO,NARRATION,"
            Strsql += " RATE,COSTID,"
            Strsql += " CTGRM,DESIGNERID,ITEMTYPEID,"
            Strsql += " CARRYFLAG,REASON,BATCHNO,CANCEL,"
            Strsql += " USERID,UPDATED,UPTIME,SYSTEMID,APPVER,EXTRAWT,TCOSTID)VALUES("
            Strsql += " '" & tagSno & "'" 'SNO
            Strsql += " ," & Pmid & "" 'ITEMID
            Strsql += " ," & Pmsubid & "" 'SUBITEMID
            Strsql += " ,'" & GetStockCompId() & "'" 'COMPANYID
            Strsql += " ,'" & GetEntryDate(DateTime.Now.Date, _tran).ToString("yyyy-MM-dd") & "'" 'RECDATE
            Strsql += " ," & Val("1").ToString & "" 'PCS
            Strsql += " ," & Val("0").ToString & "" 'GRSWT
            Strsql += " ," & Val("0").ToString & "" 'LESSWT
            Strsql += " ," & Val("0").ToString & "" 'NETWT
            Strsql += " ," & Val("0").ToString & "" 'FINRATE
            Strsql += " ,''" 'ISSTYPE
            Strsql += " ,'I'" 'RECISS
            Strsql += " ,''" 'POSTED
            Strsql += " ,''" 'PACKETNO
            Strsql += " ,0" 'DREFNO
            Strsql += " ,NULL" 'ITEMCTRID
            Strsql += " ,''" 'ORDREPNO
            Strsql += " ,''" 'ORSNO
            Strsql += " ,'COMPLIMENT ISSUE'" 'NARRATION
            Strsql += " ," & Val("0") & "" 'RATE
            Strsql += " ,'" & cnCostId & "'" 'COSTID
            Strsql += " ,''"
            Strsql += " ,0" 'DESIGNERID
            Strsql += " ,0" 'ITEMTYPEID
            Strsql += " ,''" 'CARRYFLAG
            Strsql += " ,'0'" 'REASON
            Strsql += " ,'" + MBatchno + "'" 'BATCHNO
            Strsql += " ,''" 'CANCEL
            Strsql += " ," & userId & "" 'USERID
            Strsql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
            Strsql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
            Strsql += " ,'" & systemId & "'" 'SYSTEMID
            Strsql += " ,'" & VERSION & "'" 'APPVER
            Strsql += " ,'0'" 'EXTRAWT
            Strsql += " ,'" & cnCostId & "'" 'TCOSTID
            Strsql += " )"
            ExecQuery(SyncMode.Stock, Strsql, cn, _tran, cnCostId)
        Next
    End Function

    Private Sub txtBillNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtBillNo.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            MBillno = Val(txtBillNo.Text)
            LoadCombo()
            LoadBillDetail()
            dtitems.Rows.Clear()
        End If
    End Sub

    Private Sub cmbPMSubType_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbPMSubType.KeyPress
        'If e.KeyChar = Chr(Keys.Enter) Then
        '    GridView_OWN.CurrentRow.Cells("QTY").Selected = True
        'End If
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        txtBillNo.Text = ""
        dtitems.Rows.Clear()
        GridView_OWN.DataSource = Nothing
        txtBillNo.Select()
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        If cmbPMType.Text.ToString.Trim = "" Then
            MsgBox("Item should not be empty.")
            Exit Sub
        End If
        If txtBillNo.Text.ToString.Trim = "" Then
            MsgBox("Bill no should not be empty.")
            Exit Sub
        End If
        If GridView_OWN.Rows.Count = 0 Then
            MsgBox("Bill details is empty.")
            Exit Sub
        End If
        Dim dr As DataRow
        dr = dtitems.NewRow
        dr("ITEM") = cmbPMType.Text.ToString
        dr("SUBITEM") = cmbPMSubType.Text.ToString
        dr("PCS") = "1"
        dtitems.Rows.Add(dr)
        dtitems.AcceptChanges()
        cmbPMType.Select()
    End Sub

    Private Sub SaveToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveToolStripMenuItem.Click
        btnOk_Click(Me, e)
    End Sub

    Private Sub NewToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NewToolStripMenuItem.Click
        btnClear_Click(Me, e)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        btnExit_Click(Me, e)
    End Sub
End Class