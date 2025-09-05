Imports System.Data.OleDb
Public Class frmPurchaseVATReportPer
    Dim objGridShower As frmGridDispDia
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim dtCompany As New DataTable
    Dim dtTranType As New DataTable
    Dim dtCostCentre As New DataTable
    Private Sub frmPurchaseVATReportNewNew_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmPurchaseVATReportNewNew_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GrpContainer.Location = New Point((ScreenWid - GrpContainer.Width) / 2, ((ScreenHit - 128) - GrpContainer.Height) / 2)
        cmbMetal.Items.Clear()
        cmbMetal.Items.Add("ALL")
        strSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST ORDER BY METALNAME"
        objGPack.FillCombo(strSql, cmbMetal, False, False)

        strSql = " SELECT 'ALL' COMPANYNAME,'ALL' COMPANYID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COMPANYNAME,COMPANYID,2 RESULT FROM " & cnAdminDb & "..COMPANY where ISNULL(ACTIVE,'')<>'N'"
        strSql += " ORDER BY RESULT,COMPANYNAME"
        dtCompany = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCompany)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dtCompany, "COMPANYNAME", , strCompanyName)

        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False
        chkcmbTranType.Visible = False
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        Prop_Gets()
        dtpFrom.Select()
    End Sub

    Private Function GetReportType() As String
        Dim retStr As String
        If rbtCategoryWise.Checked Then
            retStr = "C"
        ElseIf rbtMonth.Checked Then
            retStr = "M"
        ElseIf rbtTranDate.Checked Then
            retStr = "D"
        Else
            retStr = "T"
        End If
        Return retStr
    End Function

    Private Function ReportTitle(ByVal ttype As String) As String
        Dim rptTitle As String = ""
        Select Case ttype
            Case "C" 'category wise
                rptTitle += " CATEGORY WISE "
            Case "M"
                rptTitle += " MONTH WISE "
            Case "D"
                rptTitle += " DATE WISE "
            Case "T"
                rptTitle += " BILL NO WISE "
        End Select
        rptTitle += " PURCHASE VAT REPORT FROM " & dtpFrom.Text & " TO " & dtpTo.Text
        rptTitle += IIf(chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "", " :" & chkCmbCostCentre.Text, "")
        Return rptTitle
    End Function

    Private Function GetTable(ByVal rptType As String, ByVal fromDate As Date, ByVal ToDate As Date, Optional ByVal CatNames As String = "ALL", Optional ByVal MonthName As String = "ALL") As DataTable
        Dim AcType As String = "ALL"
        Dim LocOut As String = ""
        If chkLocal.Checked Then LocOut += "L"
        If chkOutstaion.Checked Then LocOut += "O"
        If LocOut = "" Then LocOut = "LO"
        Dim InclVat As String = ""
        If chkInclVat.Checked Then InclVat = "I"

        strSql = "IF OBJECT_ID('TEMPTABLEDB..TEMPPURVATRPT','U') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPPURVATRPT "
        strSql += "IF OBJECT_ID('TEMPTABLEDB..TEMPPURVATRESULT','U') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPPURVATRESULT "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " EXEC " & cnStockDb & "..SP_RPT_PURCHASEVATPER"
        strSql += "  @FROMDATE = '" & fromDate.ToString("yyyy-MM-dd") & "'"
        strSql += " ,@TODATE = '" & ToDate.ToString("yyyy-MM-dd") & "'"
        strSql += " ,@RPTTYPE = '" & rptType & "'"
        strSql += " ,@COSTNAME = '" & IIf(chkCmbCostCentre.Text <> "", chkCmbCostCentre.Text, "ALL") & "'"
        strSql += " ,@COMPANYNAME = '" & chkCmbCompany.Text & "'"
        strSql += " ,@METALNAME = '" & cmbMetal.Text & "'"
        strSql += " ,@CATNAME = '" & CatNames & "'"
        strSql += " ,@MONTHNAME = '" & MonthName & "'"
        strSql += " ,@ACTYPE = '" & AcType & "'"
        strSql += " ,@LOCOUT = '" & LocOut & "'"
        strSql += " ,@TRANTYPE = 'PURCHASE'"
        strSql += " ,@invat='" & InclVat & "'"
        strSql += " ,@GRADNTOT='" & IIf(ChkGrndtot.Checked, "Y", "N") & "'"
        strSql += " ,@DBNAME='" & cnAdminDb & "'"

        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        da = New OleDbDataAdapter(cmd)
        Dim DtGrid As New DataTable
        DtGrid.TableName = rptType
        DtGrid.Columns.Add("KEYNO", GetType(Integer))
        DtGrid.Columns("KEYNO").AutoIncrement = True
        DtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        DtGrid.Columns("KEYNO").AutoIncrementStep = 1
        da.Fill(DtGrid)
        strSql = "IF OBJECT_ID('TEMPTABLEDB..TEMPPURVATRPT','U') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPPURVATRPT "
        strSql += "IF OBJECT_ID('TEMPTABLEDB..TEMPPURVATRESULT','U') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPPURVATRESULT "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        Return DtGrid

    End Function

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        Dim DtGrid As New DataTable
        DtGrid = GetTable(GetReportType, dtpFrom.Value.ToString, dtpTo.Value.ToString)
        If Not DtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        DtGrid.Columns("KEYNO").SetOrdinal(DtGrid.Columns.Count - 1)
        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        objGridShower.Size = New Size(778, 550)
        objGridShower.Text = "PURCHASE VAT REPORT"
        objGridShower.lblTitle.Text = ReportTitle(GetReportType)
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        AddHandler objGridShower.gridView.KeyDown, AddressOf DataGridView_KeyDown
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(DtGrid)
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        objGridShower.FormReLocation = False
        objGridShower.pnlFooter.Visible = True
        objGridShower.pnlFooter.BorderStyle = BorderStyle.Fixed3D
        objGridShower.pnlFooter.Size = New Size(objGridShower.pnlFooter.Width, 20)
        objGridShower.Show()
        objGridShower.FormReLocation = True
        objGridShower.FormReSize = True
        GridFormatting(GetReportType, objGridShower.gridView)
        Prop_Sets()
    End Sub
    Private Sub GridFormatting(ByVal ttype As String, ByVal Dgv As DataGridView)
        Select Case ttype
            Case "C", "M", "D"
                DataGridView_Formatting_Category(Dgv, ttype)
            Case Else
                DataGridView_Formatting_Tranno(Dgv)
        End Select
        Dim f As frmGridDispDia
        f = objGPack.GetParentControl(Dgv)
        Select Case ttype
            Case "C", "M", "D"
                f.lblStatus.Text = " <Press [D]Detail View for Selected /[Ctrl+D]Detail View for All"
        End Select
        If f.dsGrid.Tables.Count > 1 Then
            f.lblStatus.Text += " /[Escape]To Back"
        End If
        f.gridView_ColumnWidthChanged(Me, New DataGridViewColumnEventArgs(Dgv.Columns(Dgv.FirstDisplayedCell.ColumnIndex)))
    End Sub
    Private Sub DataGridView_Formatting_Tranno(ByVal Dgv As DataGridView)
        With Dgv
            For Each dgvCol As DataGridViewColumn In Dgv.Columns
                dgvCol.SortMode = DataGridViewColumnSortMode.NotSortable
                dgvCol.Visible = False
            Next
            .Columns("PARTICULAR").Visible = True
            .Columns("PARTICULAR").Frozen = True
            .Columns("DESCRIPT").Visible = False
            .Columns("Refno").Visible = True
            .Columns("Refdate").Visible = True
            .Columns("TRANNO").Visible = True
            .Columns("TRANDATE").Visible = True
            .Columns("CENTRALTAXNO").Visible = True
            .Columns("PCS").Visible = True
            .Columns("GRSWT").Visible = True
            .Columns("NETWT").Visible = True
            .Columns("AMOUNT").Visible = True
            'If chkED.Checked Then
            '    .Columns("ED").Visible = True
            '    .Columns("ECESS").Visible = True
            '    .Columns("HECESS").Visible = True
            'End If
            .Columns("TAX").Visible = True
            .Columns("TAX_PERCENTAGE").Visible = True
            '.Columns("CATNAME").Visible = True
            .Columns("CTAX").Visible = True
            .Columns("TOTAL").Visible = True
            .Columns("COSTID").Visible = True
            .Columns("PARTICULAR").Width = 250
            .Columns("TRANNO").Width = 60
            .Columns("TRANDATE").Width = 100
            .Columns("PCS").Width = 70
            .Columns("GRSWT").Width = 100
            .Columns("NETWT").Width = 100
            .Columns("AMOUNT").Width = 100
            If chkED.Checked Then
                .Columns("ED").Width = 100
                .Columns("ECESS").Width = 100
                .Columns("HECESS").Width = 100
            End If
            .Columns("TAX").Width = 100
            .Columns("TAX_PERCENTAGE").Width = 100
            .Columns("CATNAME").Width = 200
            .Columns("CTAX").Width = 100
            .Columns("TOTAL").Width = 100
            .Columns("COSTID").Width = 60
            .Columns("TRANNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            If chkED.Checked Then
                .Columns("ED").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("ECESS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("HECESS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End If
            .Columns("TAX").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("TAX_PERCENTAGE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("CATNAME").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns("CTAX").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("TOTAL").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns("CENTRALTAXNO").HeaderText = "CTAXNO"
            .Columns("REFNO").HeaderText = "INV.NO"
            .Columns("REFDATE").HeaderText = "INV.DATE"
            .Columns("TAX_PERCENTAGE").HeaderText = "TAX %"
            .Columns("CATNAME").HeaderText = "PARTICULAR NAME"
            FillGridGroupStyle_KeyNoWise(Dgv)
        End With
    End Sub
    Private Sub DataGridView_Formatting_Category(ByVal Dgv As DataGridView, ByVal groupby As String)
        With Dgv
            For Each dgvCol As DataGridViewColumn In Dgv.Columns
                dgvCol.SortMode = DataGridViewColumnSortMode.NotSortable
                dgvCol.Visible = False
            Next
            .Columns("PARTICULAR").Visible = True
            .Columns("PARTICULAR").Frozen = True
            .Columns("PCS").Visible = True
            .Columns("GRSWT").Visible = True
            .Columns("NETWT").Visible = True
            .Columns("AMOUNT").Visible = True
            If chkED.Checked Then
                .Columns("ED").Visible = True
                .Columns("ECESS").Visible = True
                .Columns("HECESS").Visible = True
            End If
            .Columns("TAX").Visible = True
            .Columns("TAX_PERCENTAGE").Visible = True
            .Columns("CATNAME").Visible = True
            .Columns("CTAX").Visible = True
            .Columns("TOTAL").Visible = True
            .Columns("CENTRALTAXNO").Visible = IIf(groupby = "C", True, False)
            .Columns("PARTICULAR").Width = 250
            .Columns("PCS").Width = 70
            .Columns("GRSWT").Width = 100
            .Columns("NETWT").Width = 100
            .Columns("AMOUNT").Width = 100
            If chkED.Checked Then
                .Columns("ED").Width = 100
                .Columns("ECESS").Width = 100
                .Columns("HECESS").Width = 100
            End If
            .Columns("TAX").Width = 100
            .Columns("TAX_PERCENTAGE").Width = 100
            .Columns("CATNAME").Width = 200
            .Columns("CTAX").Width = 100
            .Columns("TOTAL").Width = 100
            .Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            If chkED.Checked Then
                .Columns("ED").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("ECESS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("HECESS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End If
            .Columns("TAX").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("TAX_PERCENTAGE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("CATNAME").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns("CTAX").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("TOTAL").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("CENTRALTAXNO").HeaderText = "CTAXNO"
            .Columns("TAX_PERCENTAGE").HeaderText = "TAX %"
            .Columns("CATNAME").HeaderText = "PARTICULAR NAME"
            FillGridGroupStyle_KeyNoWise(Dgv)
        End With
    End Sub
    Private Sub DataGridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = Keys.Escape Then
            Dim dgv As DataGridView = CType(sender, DataGridView)
            Dim DtGrid As New DataTable
            Dim f As frmGridDispDia
            Dim NextType As String = ""
            f = objGPack.GetParentControl(dgv)
            Select Case f.dsGrid.Tables(f.dsGrid.Tables.Count - 1).TableName
                Case "C"
                    Exit Sub
                Case "M"
                    If f.dsGrid.Tables.Contains("C") = False Then Exit Sub
                    NextType = "C"
                Case "D"
                    If f.dsGrid.Tables.Contains("M") = False Then Exit Sub
                    NextType = "M"
                Case "T"
                    If f.dsGrid.Tables.Contains("D") = False Then Exit Sub
                    NextType = "D"
            End Select
            f.FormReSize = False
            f.gridView.DataSource = Nothing
            DtGrid = f.dsGrid.Tables(NextType)
            DtGrid.TableName = NextType
            f.gridView.DataSource = f.dsGrid.Tables(NextType)
            f.dsGrid.Tables.Remove(f.dsGrid.Tables(f.dsGrid.Tables.Count - 1))
            f.lblTitle.Text = ReportTitle(NextType)
            GridFormatting(NextType, f.gridView)
            f.FormReSize = True
            f.gridView.Select()
        ElseIf e.KeyCode = Keys.D Then
            Dim isCtrl As Boolean = e.Control
            Dim dgv As DataGridView = CType(sender, DataGridView)
            Dim f As frmGridDispDia
            Dim DtGrid As New DataTable
            Dim NextType As String
            f = objGPack.GetParentControl(dgv)
            Select Case f.dsGrid.Tables(f.dsGrid.Tables.Count - 1).TableName
                Case "C"
                    If dgv.CurrentRow Is Nothing Then
                        dgv.Select()
                        Exit Sub
                    End If
                    If dgv.CurrentRow.Cells("CATNAME").Value.ToString = "" Then Exit Sub
                    NextType = "M"
                    DtGrid = GetTable(NextType, dtpFrom.Value, dtpTo.Value, IIf(isCtrl, "ALL", dgv.CurrentRow.Cells("CATNAME").Value.ToString))
                    DtGrid.TableName = NextType
                Case "M"
                    If dgv.CurrentRow Is Nothing Then
                        dgv.Select()
                        Exit Sub
                    End If
                    If dgv.CurrentRow.Cells("CATNAME").Value.ToString = "" Then Exit Sub
                    If dgv.CurrentRow.Cells("MMONTHNAME").Value.ToString = "" Then Exit Sub
                    NextType = "D"
                    DtGrid = GetTable(NextType, dtpFrom.Value, dtpTo.Value, IIf(isCtrl, "ALL", dgv.CurrentRow.Cells("CATNAME").Value.ToString), IIf(isCtrl, "ALL", dgv.CurrentRow.Cells("MMONTHNAME").Value.ToString))
                    DtGrid.TableName = NextType
                Case "D"
                    If dgv.CurrentRow Is Nothing Then
                        dgv.Select()
                        Exit Sub
                    End If
                    If dgv.CurrentRow.Cells("CATNAME").Value.ToString = "" Then Exit Sub
                    If dgv.CurrentRow.Cells("MMONTHNAME").Value.ToString = "" Then Exit Sub
                    If IsDBNull(dgv.CurrentRow.Cells("TRANDATE").Value) Then Exit Sub
                    NextType = "T"
                    DtGrid = GetTable(NextType, IIf(isCtrl, dtpFrom.Value, dgv.CurrentRow.Cells("TRANDATE").Value), IIf(isCtrl, dtpTo.Value, dgv.CurrentRow.Cells("TRANDATE").Value), IIf(isCtrl, "ALL", dgv.CurrentRow.Cells("CATNAME").Value.ToString), IIf(isCtrl, "ALL", dgv.CurrentRow.Cells("MMONTHNAME").Value.ToString))
                    DtGrid.TableName = NextType
            End Select
            If Not DtGrid.Rows.Count > 0 Then
                MsgBox("Record not found", MsgBoxStyle.Information)
                Exit Sub
            End If
            objGridShower.dsGrid.Tables.Add(DtGrid)
            f.FormReSize = False
            f.gridView.DataSource = Nothing
            f.gridView.DataSource = f.dsGrid.Tables(DtGrid.TableName)
            f.lblTitle.Text = ReportTitle(NextType)
            GridFormatting(NextType, f.gridView)
            f.FormReSize = True
            f.gridView.Select()
        End If
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New PurchaseVatReportNew_Properties
        GetSettingsObj(obj, Me.Name, GetType(PurchaseVatReportNew_Properties))
        SetChecked_CheckedList(chkCmbCompany, obj.p_chkCmbCompany, strCompanyName)
        SetChecked_CheckedList(chkcmbTranType, obj.p_chkCmbTrantype, "ALL")
        cmbMetal.Text = obj.p_cmbMetal
        chkLocal.Checked = obj.p_chkLocal
        chkOutstaion.Checked = obj.p_chkOutstaion
        rbtTranNo.Checked = obj.p_rbtTranNo
        rbtCategoryWise.Checked = obj.p_rbtCategoryWise
        rbtMonth.Checked = obj.p_rbtMonth
        rbtTranDate.Checked = obj.p_rbtTranDate
        chkInclVat.Checked = obj.p_chkincludeVat
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New PurchaseVatReportNew_Properties
        GetChecked_CheckedList(chkCmbCompany, obj.p_chkCmbCompany)
        GetChecked_CheckedList(chkcmbTranType, obj.p_chkCmbTrantype)
        GetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre)
        obj.p_cmbMetal = cmbMetal.Text
        obj.p_chkLocal = chkLocal.Checked
        obj.p_chkOutstaion = chkOutstaion.Checked
        obj.p_rbtTranNo = rbtTranNo.Checked
        obj.p_rbtCategoryWise = rbtCategoryWise.Checked
        obj.p_rbtMonth = rbtMonth.Checked
        obj.p_rbtTranDate = rbtTranDate.Checked
        SetSettingsObj(obj, Me.Name, GetType(PurchaseVatReportNew_Properties))
    End Sub
End Class
Public Class PurchaseVatReportNewNew_Properties
    Private cmbMetal As String = "ALL"
    Public Property p_cmbMetal() As String
        Get
            Return cmbMetal
        End Get
        Set(ByVal value As String)
            cmbMetal = value
        End Set
    End Property
    Private chkCmbCompany As New List(Of String)
    Public Property p_chkCmbCompany() As List(Of String)
        Get
            Return chkCmbCompany
        End Get
        Set(ByVal value As List(Of String))
            chkCmbCompany = value
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

    Private chkCmbTrantype As New List(Of String)
    Public Property p_chkCmbTrantype() As List(Of String)
        Get
            Return chkCmbTrantype
        End Get
        Set(ByVal value As List(Of String))
            chkCmbTrantype = value
        End Set
    End Property
    Private chkLocal As Boolean = True
    Public Property p_chkLocal() As Boolean
        Get
            Return chkLocal
        End Get
        Set(ByVal value As Boolean)
            chkLocal = value
        End Set
    End Property
    Private chkOutstaion As Boolean = True
    Public Property p_chkOutstaion() As Boolean
        Get
            Return chkOutstaion
        End Get
        Set(ByVal value As Boolean)
            chkOutstaion = value
        End Set
    End Property
    Private rbtTranNo As Boolean = True
    Public Property p_rbtTranNo() As Boolean
        Get
            Return rbtTranNo
        End Get
        Set(ByVal value As Boolean)
            rbtTranNo = value
        End Set
    End Property
    Private rbtCategoryWise As Boolean = False
    Public Property p_rbtCategoryWise() As Boolean
        Get
            Return rbtCategoryWise
        End Get
        Set(ByVal value As Boolean)
            rbtCategoryWise = value
        End Set
    End Property
    Private rbtMonth As Boolean = False
    Public Property p_rbtMonth() As Boolean
        Get
            Return rbtMonth
        End Get
        Set(ByVal value As Boolean)
            rbtMonth = value
        End Set
    End Property
    Private rbtTranDate As Boolean = False
    Public Property p_rbtTranDate() As Boolean
        Get
            Return rbtTranDate
        End Get
        Set(ByVal value As Boolean)
            rbtTranDate = value
        End Set
    End Property
    Private chkInclVat As Boolean = False
    Public Property p_chkincludeVat() As Boolean
        Get
            Return chkInclVat
        End Get
        Set(ByVal value As Boolean)
            chkInclVat = value
        End Set
    End Property
End Class