Imports System.Data.OleDb
Imports System.Threading
Imports System.IO
Public Class frmGSTITC04
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim dsGridView As New DataSet
    Dim dtCostCentre As New DataTable
    Dim ChitDb As String = GetAdmindbSoftValue("CHITDBPREFIX", "")

    Private Sub SalesAbs()
        Try
            Prop_Sets()
            gridView.DataSource = Nothing
            Dim Sysid As String = Trim(LSet(Mid(Guid.NewGuid.ToString, 1, InStr(Guid.NewGuid.ToString, "-") - 1), 10))
            Me.Refresh()
            If ChkJobWorkIssue.Checked = False And ChkJobWorkReceipt.Checked = False Then
                MsgBox("Please Select any one Option...", MsgBoxStyle.Information)
                Exit Sub
            End If
            Dim CostName As String = Nothing
            Dim MetalName As String = Nothing
            Dim FilterString As String = ""

            FilterString += vbCrLf + " AND TRANDATE BETWEEN '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
            FilterString += vbCrLf + " AND I.COMPANYID = '" & strCompanyId & "'"
            If chkCmbCostCentre.Text <> "ALL" Then
                CostName = Replace(chkCmbCostCentre.Text, ",", "','")
                FilterString += vbCrLf + " AND I.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN ('" & CostName & "'))"
            End If
            If cmbMetal.Text <> "ALL" Then
                FilterString += vbCrLf + " AND C.METALID IN (SELECT METALID  FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN ('" & cmbMetal.Text & "'))"
            End If
            FilterString += vbCrLf + " AND ISNULL(CANCEL,'') = '' "


            strSql = vbCrLf + " IF (SELECT COUNT(*) FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & userId & "GSTITC04') > 0  DROP TABLE TEMPTABLEDB..TEMP" & systemId & userId & "GSTITC04"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery() : cmd.CommandTimeout = 1000

            If ChkJobWorkIssue.Checked Then
                strSql = vbCrLf + " SELECT ISNULL(A.GSTNO,'') GSTNO , (SELECT STATECODE + '-'+ STATENAME FROM " & cnAdminDb & "..STATEMAST WHERE STATEID  = A.STATEID) STATE"
                strSql += vbCrLf + " ,CASE WHEN ISNULL(A.GSTNO,'')<>'' THEN 'SEZ' ELSE 'Non SEZ' END TYPE,TRANNO , TRANDATE , 'CAPITAL GOODS' AS TYPEOFGOODS"
                strSql += vbCrLf + " ,C.CATNAME ,'GRAMS' UQC , SUM(I.GRSWT) GRSWT,SUM(ISNULL(I.AMOUNT,0)) AMOUNT"
                strSql += vbCrLf + " ,C.S_IGSTTAX ,C.S_CGSTTAX,C.S_SGSTTAX ,''CESS,'ADD' ACTION,I.BATCHNO,1 RESULT"
                strSql += vbCrLf + "  INTO TEMPTABLEDB..TEMP" & systemId & userId & "GSTITC04"
                strSql += vbCrLf + "  FROM " & cnStockDb & "..ISSUE I"
                strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ACHEAD A ON I.ACCODE = A.ACCODE "
                strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY C ON I.CATCODE =C.CATCODE "
                strSql += vbCrLf + " WHERE 1= 1"
                strSql += vbCrLf + FilterString
                strSql += vbCrLf + "AND I.TRANTYPE IN ('IIS')"
                strSql += vbCrLf + " GROUP BY A.GSTNO , A.STATEID , I.TRANNO , I.TRANDATE,I.BATCHNO "
                strSql += vbCrLf + " ,C.S_IGSTTAX ,C.S_CGSTTAX,C.S_SGSTTAX,C.S_IGSTTAX  ,C.CATNAME "
                strSql += vbCrLf + " ORDER BY I.TRANDATE , I.TRANNO "
            ElseIf ChkJobWorkReceipt.Checked Then
                strSql = vbCrLf + "SELECT ISNULL(A.GSTNO,'') GSTNO , (SELECT STATECODE + '-'+ STATENAME FROM " & cnAdminDb & "..STATEMAST WHERE STATEID  = A.STATEID) STATE"
                strSql += vbCrLf + " ,CASE WHEN ISNULL(A.GSTNO,'')<>'' THEN 'SEZ' ELSE 'Non SEZ' END TYPE,TRANNO , TRANDATE , 'CAPITAL GOODS' AS TYPEOFGOODS"
                strSql += vbCrLf + " ,C.CATNAME ,'GRAMS' UQC , SUM(I.GRSWT) GRSWT,SUM(ISNULL(I.AMOUNT,0)) AMOUNT"
                strSql += vbCrLf + " ,C.S_IGSTTAX ,C.S_CGSTTAX,C.S_SGSTTAX ,''CESS,I.BATCHNO,1 RESULT"
                strSql += vbCrLf + "  INTO TEMPTABLEDB..TEMP" & systemId & userId & "GSTITC04"
                strSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT I"
                strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ACHEAD A ON I.ACCODE = A.ACCODE "
                strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY C ON I.CATCODE =C.CATCODE "
                strSql += vbCrLf + " WHERE 1=1 "
                strSql += vbCrLf + FilterString
                strSql += vbCrLf + " AND I.TRANTYPE IN ('RRE')"
                strSql += vbCrLf + " GROUP BY A.GSTNO , A.STATEID , I.TRANNO , I.TRANDATE ,I.BATCHNO"
                strSql += vbCrLf + " ,C.S_IGSTTAX ,C.S_CGSTTAX,C.S_SGSTTAX,C.S_IGSTTAX  ,C.CATNAME "
                strSql += vbCrLf + " ORDER BY I.TRANDATE , I.TRANNO "
            End If
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery() : cmd.CommandTimeout = 1000

            strSql = "SELECT * FROM TEMPTABLEDB..TEMP" & systemId & userId & "GSTITC04"

            Dim dtGrid As New DataTable
            dtGrid.Columns.Add("KEYNO", GetType(Integer))
            dtGrid.Columns("KEYNO").AutoIncrement = True
            dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
            dtGrid.Columns("KEYNO").AutoIncrementStep = 1
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtGrid)
            If Not dtGrid.Rows.Count > 0 Then
                MsgBox("Record Not Found", MsgBoxStyle.Information)
                dtpFrom.Select()
                Exit Sub
            End If
            dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
            gridView.DataSource = dtGrid

            With gridView
                If .Columns.Contains("BATCHNO") Then .Columns("BATCHNO").Visible = False
                If .Columns.Contains("KEYNO") Then .Columns("KEYNO").Visible = False
                If .Columns.Contains("TRANTYPE") Then .Columns("TRANTYPE").Visible = False
                If .Columns.Contains("RESULT") Then .Columns("RESULT").Visible = False
                If .Columns.Contains("GSTNO") Then .Columns("GSTNO").HeaderText = "GSTIN of Job Worker (JW) "
                If .Columns.Contains("TRANNO") Then .Columns("TRANNO").HeaderText = "Challan Number"
                If .Columns.Contains("TRANDATE") Then .Columns("TRANDATE").HeaderText = "Challan Date"
                If .Columns.Contains("TAXABLE") Then .Columns("AMOUNT").HeaderText = "Taxable Value"
                If .Columns.Contains("TYPEOFGOODS") Then .Columns("TYPEOFGOODS").HeaderText = "Type Of Goods"
                If .Columns.Contains("CATNAME") Then .Columns("CATNAME").HeaderText = "Description of Goods"
                If .Columns.Contains("GRSWT") Then .Columns("GRSWT").HeaderText = "Quantity"
                If .Columns.Contains("STATE") Then .Columns("STATE").HeaderText = "State"
                If .Columns.Contains("TYPE") Then .Columns("TYPE").HeaderText = "Job Worker's Type"
                If .Columns.Contains("AMOUNT") Then .Columns("AMOUNT").HeaderText = "Taxable Value"
                If .Columns.Contains("S_IGSTTAX") Then .Columns("S_IGSTTAX").HeaderText = "Integrated Tax Rate"
                If .Columns.Contains("S_SGSTTAX") Then .Columns("S_SGSTTAX").HeaderText = "Central Tax Rate"
                If .Columns.Contains("S_CGSTTAX") Then .Columns("S_CGSTTAX").HeaderText = "State/UT Tax Rate"
                If .Columns.Contains("TRANDATE") Then .Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"

                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                .Invalidate()
                For Each dgvCol As DataGridViewColumn In .Columns
                    dgvCol.Width = dgvCol.Width
                Next
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                If .Columns.Contains("STATE") Then .Columns("STATE").Width = 150
                If .Columns.Contains("ELIGIBLE") Then .Columns("ELIGIBLE").Width = 150
                If .Columns.Contains("DIFFERENTIAL") Then .Columns("DIFFERENTIAL").Width = 100
                If .Columns.Contains("TYPE") Then .Columns("TYPE").Width = 85
            End With
            gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            gridView.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 9, FontStyle.Bold)
            Dim TITLE As String = ""
            FormatGridColumns(gridView, False)
            FillGridGroupStyle_KeyNoWise(gridView)
            TITLE += " GST REPORT FROM " & dtpFrom.Text & " TO " & dtpTo.Text & ""
            TITLE += "  AT " & chkCmbCostCentre.Text & ""
            lblTitle.Font = New Font("VERDANA", 9, FontStyle.Bold)
            lblTitle.Text = TITLE
            pnlHeading.Visible = True
            btnView_Search.Enabled = True
            gridView.Focus()
        Catch ex As Exception
            MsgBox(ex.Message & vbCrLf & ex.StackTrace, MsgBoxStyle.Information) : Exit Sub
        End Try
    End Sub

    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        pnlHeading.Visible = False
        SalesAbs()
        Exit Sub
    End Sub

    Private Sub frmSalesAbstract_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmSalesAbstract_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        cmbMetal.Items.Clear()
        cmbMetal.Items.Add("ALL")
        strSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST ORDER BY METALNAME"
        objGPack.FillCombo(strSql, cmbMetal, False)
        cmbMetal.Text = "ALL"
        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += vbCrLf + " UNION ALL"
        strSql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE WHERE  ISNULL(COMPANYID,'" & strCompanyId & "')LIKE'%" & strCompanyId & "%' "
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        Me.btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ExtToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExtToolStripMenuItem.Click
        Me.btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        ChkJobWorkIssue.Checked = True
        ChkJobWorkReceipt.Checked = False
        gridView.DataSource = Nothing
        btnView_Search.Enabled = True
        Prop_Gets()
        dtpFrom.Focus()
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub
    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = "X" Or e.KeyChar = "x" Then
            Me.btnExcel_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = "P" Then
            btnPrint_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub dtpFrom_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        dtpTo.MinimumDate = dtpFrom.Value
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New frmGSTR2_Properties
        SetSettingsObj(obj, Me.Name, GetType(frmGSTR2_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmGSTR2_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmGSTR2_Properties))
    End Sub

    Private Sub AutoReziseToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AutoReziseToolStripMenuItem.Click
        If gridView.RowCount > 0 Then
            If AutoReziseToolStripMenuItem.Checked Then
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
        End If
    End Sub

    Private Sub lblTitle_Click(sender As Object, e As EventArgs) Handles lblTitle.Click

    End Sub

    Private Sub ChkJobWorkIssue_CheckedChanged(sender As Object, e As EventArgs) Handles ChkJobWorkIssue.CheckedChanged
        If ChkJobWorkIssue.Checked Then
            ChkJobWorkReceipt.Checked = False
        Else
            ChkJobWorkReceipt.Checked = True
        End If

    End Sub

    Private Sub ChkJobWorkReceipt_CheckedChanged(sender As Object, e As EventArgs) Handles ChkJobWorkReceipt.CheckedChanged
        If ChkJobWorkReceipt.Checked Then
            ChkJobWorkIssue.Checked = False
        Else
            ChkJobWorkIssue.Checked = True
        End If
    End Sub
End Class

Public Class frmGSTITC04_Properties
    Private chkPcs As Boolean = True
    Public Property p_chkPcs() As Boolean
        Get
            Return chkPcs
        End Get
        Set(ByVal value As Boolean)
            chkPcs = value
        End Set
    End Property
    Private chkGrsWt As Boolean = True
    Public Property p_chkGrsWt() As Boolean
        Get
            Return chkGrsWt
        End Get
        Set(ByVal value As Boolean)
            chkGrsWt = value
        End Set
    End Property
    Private chkNetWt As Boolean = True
    Public Property p_chkNetWt() As Boolean
        Get
            Return chkNetWt
        End Get
        Set(ByVal value As Boolean)
            chkNetWt = value
        End Set
    End Property
    Private chkWithSR As Boolean = False
    Public Property p_chkWithSR() As Boolean
        Get
            Return chkWithSR
        End Get
        Set(ByVal value As Boolean)
            chkWithSR = value
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
    Private chkVA As Boolean = False
    Public Property p_chkVA() As Boolean
        Get
            Return chkVA
        End Get
        Set(ByVal value As Boolean)
            chkVA = value
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
    Private cmbCategory As String = "ALL"
    Public Property p_cmbCategory() As String
        Get
            Return cmbCategory
        End Get
        Set(ByVal value As String)
            cmbCategory = value
        End Set
    End Property
    Private rbtSummary As Boolean = True
    Public Property p_rbtSummary() As Boolean
        Get
            Return rbtSummary
        End Get
        Set(ByVal value As Boolean)
            rbtSummary = value
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
    Private rbtDate As Boolean = False
    Public Property p_rbtDate() As Boolean
        Get
            Return rbtDate
        End Get
        Set(ByVal value As Boolean)
            rbtDate = value
        End Set
    End Property
    Private rbtBillNo As Boolean = False
    Public Property p_rbtBillNo() As Boolean
        Get
            Return rbtBillNo
        End Get
        Set(ByVal value As Boolean)
            rbtBillNo = value
        End Set
    End Property
    Private chkBillPrefix As Boolean = False
    Public Property p_chkBillPrefix() As Boolean
        Get
            Return chkBillPrefix
        End Get
        Set(ByVal value As Boolean)
            chkBillPrefix = value
        End Set
    End Property
End Class


