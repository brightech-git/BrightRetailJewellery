Imports System.Data.OleDb
Public Class frmOrderCollectionDetails
    Dim strSql As String
    Dim Cmd As OleDbCommand
    Dim dtGridView As New DataTable
    Dim dtCompany As New DataTable
    Dim dtCostCentre As New DataTable
    Dim dtMetal As New DataTable
    Dim defaultPic As String = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'OR_PICPATH'")

    Function funcNew() As Integer
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        strSql = "SELECT COMPANYNAME FROM " & cnAdminDb & "..COMPANY WHERE ACTIVE = 'Y'"
        dtCompany = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCompany)

        strSql = "SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE "
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)

        strSql = "SELECT METALNAME FROM " & cnAdminDb & "..METALMAST "
        dtMetal = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtMetal)

        BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dtCompany, "COMPANYNAME", , strCompanyName)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, cnCostName, "ALL"))
        If Not dtMetal.Rows.Count < 0 Then
            cmbMetal.Items.Add("ALL")
            For cnt As Integer = 0 To dtMetal.Rows.Count - 1
                cmbMetal.Items.Add(dtMetal.Rows(cnt).Item("METALNAME").ToString)
            Next
        End If
        txtOrderNo.Clear()
        cmbMetal.SelectedIndex = 0
        dtGridView.Rows.Clear()
        rbtAdvance.Select()
        gridView.DataSource = Nothing
    End Function
    Function funcStyleGridView() As Integer
        With gridView
            For I As Integer = 0 To .Columns.Count - 1
                If .Columns(I).ValueType.Name = GetType(String).Name Then
                    .Columns(I).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                    .Columns(I).DefaultCellStyle.WrapMode = DataGridViewTriState.True
                    .Columns(I).Width = 250
                ElseIf .Columns(I).ValueType.Name = GetType(Decimal).Name Then
                    .Columns(I).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns(I).DefaultCellStyle.WrapMode = DataGridViewTriState.True
                    .Columns(I).Width = 120
                ElseIf .Columns(I).ValueType.Name = GetType(Double).Name Then
                    .Columns(I).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns(I).DefaultCellStyle.WrapMode = DataGridViewTriState.True
                    .Columns(I).Width = 120
                ElseIf .Columns(I).ValueType.Name = GetType(Integer).Name Then
                    .Columns(I).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns(I).DefaultCellStyle.WrapMode = DataGridViewTriState.True
                    .Columns(I).Width = 120
                ElseIf .Columns(I).ValueType.Name = GetType(Date).Name Then
                    .Columns(I).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns(I).DefaultCellStyle.WrapMode = DataGridViewTriState.True
                    .Columns(I).DefaultCellStyle.Format = "dd/MM/yyyy"
                    .Columns(I).Width = 120
                End If
            Next

            For Each GV As DataGridViewRow In .Rows
                If GV.Cells("COLHEAD").Value.ToString = "T" Then
                    GV.DefaultCellStyle.BackColor = Color.LightGoldenrodYellow
                    GV.DefaultCellStyle.ForeColor = Color.Green
                    GV.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                End If
                If GV.Cells("COLHEAD").Value.ToString = "C" Then
                    GV.DefaultCellStyle.BackColor = Color.LightGoldenrodYellow
                    GV.DefaultCellStyle.ForeColor = Color.Red
                    GV.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                End If

            Next

            If .Columns.Contains("COLHEAD") Then .Columns("COLHEAD").Visible = False
            If .Columns.Contains("BATCHNO") Then .Columns("BATCHNO").Visible = False
            If .Columns.Contains("TORDATE") Then .Columns("TORDATE").Visible = False
        End With
    End Function

    Private Sub frmOrderCollectionDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    
    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
        End If
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If UCase(e.KeyChar) = "P" Then
            btnPrint_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = "X" Then
            btnExcel_Click(Me, New EventArgs)
        End If
    End Sub
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim OrderNo As String
        dtGridView.Rows.Clear()
        strSql = "EXEC " & cnAdminDb & "..SP_RPT_ORCOLLECTIONDET"
        strSql += vbCrLf + "@DBNAME   = '" & cnStockDb & "'"
        If chkAsOnDate.Checked = True Then
            strSql += vbCrLf + ",@ASONDATE  = 'Y'"
        Else
            strSql += vbCrLf + ",@ASONDATE  = 'N'"
        End If
        strSql += vbCrLf + ",@FROMDATE  = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + ",@TODATE  = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        If rbtOrder.Checked Then
            strSql += vbCrLf + ",@ORTYPE = 'O'"
        ElseIf rbtRepair.Checked Then
            strSql += vbCrLf + ",@ORTYPE = 'R'"
        ElseIf rbtAdvance.Checked Then
            strSql += vbCrLf + ",@ORTYPE = 'A'"
        End If
        strSql += vbCrLf + ",@COMPANY  = '" & chkCmbCompany.Text & "'"
        strSql += vbCrLf + ",@COSTNAME   = '" & chkCmbCostCentre.Text & "'"
        If txtOrderNo.Text <> "" Then
            OrderNo = GetSqlValue(cn, "SELECT ORNO FROM " & cnAdminDb & "..ORMAST WHERE SUBSTRING(ORNO,6,12) = '" & txtOrderNo.Text & "'")
            strSql += vbCrLf + ",@ORDERNO = '" & OrderNo & "'"
        Else
            strSql += vbCrLf + ",@ORDERNO = ''"
        End If
        strSql += vbCrLf + ",@METAL = '" & cmbMetal.Text & "'"

        Cmd = New OleDbCommand(strSql, cn)
        Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
        strSql = "SELECT PARTICULAR,AMOUNT,COLHEAD FROM TEMPTABLEDB..TEMPORCOLLDET"
        Dim dtTemp As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtTemp)

        If Not dtTemp.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            dtpFrom.Select()
            Exit Sub
        End If
        gridView.DataSource = dtTemp
        funcStyleGridView()
        gridView.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        'Fillheadergridcolumn()
        BrighttechPack.GlobalMethods.FormatGridColumns(gridView)
        Prop_Sets()
    End Sub



    Private Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        Dim Title As String = Nothing
        If cmbMetal.Text <> "" And cmbMetal.Text <> "ALL" Then Title += cmbMetal.Text
        Title += IIf(rbtOrder.Checked, " ORDER ", "REPAIR")
        Title += "COLLECTION"
        If chkAsOnDate.Checked Then
            Title += " AS ON " & dtpFrom.Text
        Else
            Title += " FROM " & dtpFrom.Text & " TO " & dtpTo.Text
        End If
        Title += IIf(chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "", " :" & chkCmbCostCentre.Text, "")
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, Title, gridView, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        funcNew()
        Prop_Gets()
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            Dim Title As String = Nothing
            If cmbMetal.Text <> "" And cmbMetal.Text <> "ALL" Then Title += cmbMetal.Text
            Title += IIf(rbtOrder.Checked, " ORDER ", "REPAIR")
            Title += " COLLECTION"
            If chkAsOnDate.Checked Then
                Title += " AS ON " & dtpFrom.Text
            Else
                Title += " FROM " & dtpFrom.Text & " TO " & dtpTo.Text
            End If
            Title += IIf(chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "", " :" & chkCmbCostCentre.Text, "")
            BrightPosting.GExport.Post(Me.Name, strCompanyName, Title, gridView, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        btnExit_Click(Me, New EventArgs)
    End Sub
    Private Sub ResizeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResizeToolStripMenuItem.Click
        If gridView.RowCount > 0 Then
            If ResizeToolStripMenuItem.Checked Then
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                gridView.Invalidate()
                For Each dgvCol As DataGridViewColumn In gridView.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            Else
                For Each dgvCol As DataGridViewColumn In gridView.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End If
            'gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        End If
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New frmOrderCollectionDetails_Properties
        GetChecked_CheckedList(chkCmbCompany, obj.p_chkCmbCompany)
        obj.p_txtOrderNo = txtOrderNo.Text
        obj.p_cmbMetal = cmbMetal.Text
        'GetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre

        obj.p_chkAsOnDate = chkAsOnDate.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmOrderCollectionDetails_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmOrderCollectionDetails_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmOrderCollectionDetails_Properties))
        SetChecked_CheckedList(chkCmbCompany, obj.p_chkCmbCompany, strCompanyName)
        txtOrderNo.Text = obj.p_txtOrderNo
        cmbMetal.Text = obj.p_cmbMetal
        'SetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre, cnCostName)
        chkAsOnDate.Checked = obj.p_chkAsOnDate

    End Sub

    Private Sub rbtOrder_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtOrder.CheckedChanged
        If rbtOrder.Checked Then lblOrder.Text = "Order No"
        If rbtAdvance.Checked Then lblOrder.Text = "RunNO"
    End Sub

    Private Sub rbtRepair_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtRepair.CheckedChanged
        If rbtRepair.Checked Then lblOrder.Text = "Repair No"
        If rbtAdvance.Checked Then lblOrder.Text = "RunNO"
    End Sub

    Private Sub chkAsOnDate_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAsOnDate.CheckedChanged
        If chkAsOnDate.Checked Then
            lblTo.Visible = False
            dtpTo.Visible = False
            chkAsOnDate.Text = "As On"
        Else
            lblTo.Visible = True
            dtpTo.Visible = True
            chkAsOnDate.Text = "Date"
        End If
    End Sub

    Private Sub frmOrderCollectionDetails_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        funcNew()
    End Sub

    Private Sub rbtAdvance_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtAdvance.CheckedChanged
        If rbtOrder.Checked Then lblOrder.Text = "Order No"
        If rbtRepair.Checked Then lblOrder.Text = "Repair No"
    End Sub
End Class

Public Class frmOrderCollectionDetails_Properties
    Private chkCmbCompany As New List(Of String)
    Public Property p_chkCmbCompany() As List(Of String)
        Get
            Return chkCmbCompany
        End Get
        Set(ByVal value As List(Of String))
            chkCmbCompany = value
        End Set
    End Property
    Private txtOrderNo As String = ""
    Public Property p_txtOrderNo() As String
        Get
            Return txtOrderNo
        End Get
        Set(ByVal value As String)
            txtOrderNo = value
        End Set
    End Property
    Private cmbMetal As String = "ALL"
    Public Property p_cmbMetal() As String
        Get
            Return cmbMetal
        End Get
        Set(ByVal value As String)
            cmbMetal = value
        End Set
    End Property
    Private txtSalesMan_NUM As String = ""
    Public Property p_txtSalesMan_NUM() As String
        Get
            Return txtSalesMan_NUM
        End Get
        Set(ByVal value As String)
            txtSalesMan_NUM = value
        End Set
    End Property
    Private chkCmbCostCentre As New List(Of String)
    Public Property p_chkCmbCostCentre() As List(Of String)
        Get
            Return chkCmbCostCentre
        End Get
        Set(ByVal value As List(Of String))
            chkCmbCostCentre = value
        End Set
    End Property
    Private rbtBoth As Boolean = True
    Public Property p_rbtBoth() As Boolean
        Get
            Return rbtBoth
        End Get
        Set(ByVal value As Boolean)
            rbtBoth = value
        End Set
    End Property
    Private rbtCustomer As Boolean = False
    Public Property p_rbtCustomer() As Boolean
        Get
            Return rbtCustomer
        End Get
        Set(ByVal value As Boolean)
            rbtCustomer = value
        End Set
    End Property
    Private rbtCompany As Boolean = False
    Public Property p_rbtCompany() As Boolean
        Get
            Return rbtCompany
        End Get
        Set(ByVal value As Boolean)
            rbtCompany = value
        End Set
    End Property


    Private chkAsOnDate As Boolean = False
    Public Property p_chkAsOnDate() As Boolean
        Get
            Return chkAsOnDate
        End Get
        Set(ByVal value As Boolean)
            chkAsOnDate = value
        End Set
    End Property


    Private rbtReceipt As Boolean = False
    Public Property p_rbtReceipt() As Boolean
        Get
            Return rbtReceipt
        End Get
        Set(ByVal value As Boolean)
            rbtReceipt = value
        End Set
    End Property

    Private rbtDelivery As Boolean = False
    Public Property p_rbtDelivery() As Boolean
        Get
            Return rbtDelivery
        End Get
        Set(ByVal value As Boolean)
            rbtDelivery = value
        End Set
    End Property

    Private rbtPending As Boolean = False
    Public Property p_rbtPending() As Boolean
        Get
            Return rbtPending
        End Get
        Set(ByVal value As Boolean)
            rbtPending = value
        End Set
    End Property
End Class