Imports System.Data.OleDb
Imports System.Threading
Imports System.IO
Public Class frmCategoryWiseTransaction
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim dsGridView As New DataSet
    Dim dtCostCentre As New DataTable
    Function funcGetValues(ByVal field As String, ByVal def As String) As String
        strSql = " Select ctlText from " & cnAdminDb & "..SoftControl"
        strSql += vbCrLf + " where ctlId = '" & field & "'"
        Dim dt As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            Return dt.Rows(0).Item("ctlText").ToString
        End If
        Return def
    End Function

    Private Sub SalesAbs()
        Try
            gridView.DataSource = Nothing
            Dim selectcostid As String = IIf(chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "", GetSelectedCostId(chkCmbCostCentre, False), "ALL")
            Dim selectcompid As String = IIf(chkcmbCompany.Text <> "ALL" And chkcmbCompany.Text <> "", GetSelectedCompanyId(chkcmbCompany, False), "ALL")
            Dim selectcatcode As String = IIf(chkCmbCategory.Text <> "ALL" And chkCmbCategory.Text <> "", GetSelectedCatcode(chkCmbCategory, False), "ALL")
            gridView.DataSource = Nothing
            Me.Refresh()

            Dim _sysid As String = BrighttechPack.GetSqlValue(cn, "SELECT CONVERT(VARCHAR(8),LTRIM(NEWID())) NEWID ", "NEWID", "")

            strSql = " IF OBJECT_ID('TEMPTABLEDB..TEMP" & _sysid.ToString & "REPORTNEW') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP" & _sysid.ToString & "REPORTNEW "
            strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP" & _sysid.ToString & "REPORTNEWSA') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP" & _sysid.ToString & "REPORTNEWSA "
            strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP" & _sysid.ToString & "REPORTNEWSR') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP" & _sysid.ToString & "REPORTNEWSR "
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()

            strSql = " SELECT * INTO TEMPTABLEDB..TEMP" & _sysid.ToString & "REPORTNEWSA FROM " & cnStockDb & "..ISSUE WHERE"
            strSql += vbCrLf + " TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " AND TRANTYPE='SA' AND ISNULL(CANCEL,'')='' "
            If selectcostid <> "ALL" Then
                strSql += vbCrLf + " AND COSTID IN ('" & selectcostid.ToString & "')"
            End If
            If selectcompid <> "ALL" Then
                strSql += vbCrLf + " AND COMPANYID IN ('" & selectcompid.ToString & "')"
            End If
            If selectcatcode <> "ALL" Then
                strSql += vbCrLf + " AND CATCODE IN ('" & selectcatcode.ToString & "')"
            End If
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()

            strSql = " SELECT * INTO TEMPTABLEDB..TEMP" & _sysid.ToString & "REPORTNEWSR FROM " & cnStockDb & "..RECEIPT WHERE"
            strSql += vbCrLf + " TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " AND TRANTYPE='SR' AND ISNULL(CANCEL,'')='' "
            If selectcostid <> "ALL" Then
                strSql += vbCrLf + " AND COSTID IN ('" & selectcostid.ToString & "')"
            End If
            If selectcompid <> "ALL" Then
                strSql += vbCrLf + " AND COMPANYID IN ('" & selectcompid.ToString & "')"
            End If
            If selectcatcode <> "ALL" Then
                strSql += vbCrLf + " AND CATCODE IN ('" & selectcatcode.ToString & "')"
            End If
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + " SELECT * INTO TEMPTABLEDB..TEMP" & _sysid.ToString & "REPORTNEW FROM ("
            strSql += vbCrLf + " SELECT 'SALES' TRANTYPE,TRANNO,TRANDATE,GRSWT,NETWT,LESSWT,TAX,AMOUNT,"
            strSql += vbCrLf + " 0 STNITEMID,0 STNSUBITEMID,CONVERT(VARCHAR(50),'') STNNAME,CONVERT(VARCHAR(50),'') STNSUBNAME, 0 STNWT,0 STNRATE,0 STNAMT,''STONEUNIT,SNO,1 ORDBY"
            strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE AS I  WHERE SNO IN (SELECT SNO FROM TEMPTABLEDB..TEMP" & _sysid.ToString & "REPORTNEWSA)"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT 'SALES' TRANTYPE,TRANNO,TRANDATE,0 GRSWT,0 NETWT,0 LESSWT,0 TAX,0 AMOUNT,"
            strSql += vbCrLf + " STNITEMID,STNSUBITEMID,"
            strSql += vbCrLf + " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=I.STNITEMID)STNNAME,"
            strSql += vbCrLf + " (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID=I.STNITEMID AND SUBITEMID=I.STNSUBITEMID)STNSUBNAME,"
            strSql += vbCrLf + " STNWT,STNRATE,STNAMT,STONEUNIT,ISSSNO,2  ORDBY"
            strSql += vbCrLf + " FROM " & cnStockDb & "..ISSSTONE AS I WHERE ISSSNO IN (SELECT SNO FROM TEMPTABLEDB..TEMP" & _sysid.ToString & "REPORTNEWSA)"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " /**/"
            strSql += vbCrLf + " SELECT 'RETURN' TRANTYPE,TRANNO,TRANDATE,GRSWT * (-1),NETWT * (-1),LESSWT *(-1),TAX *(-1),AMOUNT  *(-1),"
            strSql += vbCrLf + " 0 STNITEMID,0 STNSUBITEMID,CONVERT(VARCHAR(50),'') STNNAME,CONVERT(VARCHAR(50),'') STNSUBNAME, 0 STNWT,0 STNRATE,0 STNAMT,''STONEUNIT,SNO,1 ORDBY"
            strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT AS I  WHERE TRANTYPE='SR' AND SNO IN (SELECT SNO FROM TEMPTABLEDB..TEMP" & _sysid.ToString & "REPORTNEWSR)"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT 'RETURN' TRANTYPE,TRANNO,TRANDATE,0 GRSWT,0 NETWT,0 LESSWT,0 TAX,0 AMOUNT,"
            strSql += vbCrLf + " STNITEMID,STNSUBITEMID,"
            strSql += vbCrLf + " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=I.STNITEMID)STNNAME,"
            strSql += vbCrLf + " (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID=I.STNITEMID AND SUBITEMID=I.STNSUBITEMID)STNSUBNAME,"
            strSql += vbCrLf + " STNWT  *(-1) STNWT,STNRATE  *(-1) STNRATE,STNAMT  *(-1) STNAMT,STONEUNIT,ISSSNO,2  ORDBY"
            strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPTSTONE AS I WHERE  TRANTYPE='SR' AND ISSSNO IN (SELECT SNO FROM TEMPTABLEDB..TEMP" & _sysid.ToString & "REPORTNEWSR)"
            strSql += vbCrLf + " ) AS K ORDER BY TRANDATE,TRANNO,SNO,ORDBY "
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()


            strSql = vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & _sysid.ToString & "REPORTNEW SET STNITEMID=NULL WHERE STNITEMID=0"
            strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & _sysid.ToString & "REPORTNEW SET STNSUBITEMID=NULL WHERE STNSUBITEMID=0"
            strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & _sysid.ToString & "REPORTNEW SET STNWT=NULL WHERE STNWT=0"
            strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & _sysid.ToString & "REPORTNEW SET STNRATE=NULL WHERE STNRATE=0"
            strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & _sysid.ToString & "REPORTNEW SET STNAMT=NULL WHERE STNAMT=0"
            strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & _sysid.ToString & "REPORTNEW SET LESSWT=NULL WHERE LESSWT=0"
            strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & _sysid.ToString & "REPORTNEW SET GRSWT=NULL WHERE GRSWT=0"
            strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & _sysid.ToString & "REPORTNEW SET NETWT=NULL WHERE NETWT=0"
            strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & _sysid.ToString & "REPORTNEW SET AMOUNT=NULL WHERE AMOUNT=0"
            strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & _sysid.ToString & "REPORTNEW SET TAX=NULL WHERE TAX=0"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + " SELECT * FROM TEMPTABLEDB..TEMP" & _sysid.ToString & "REPORTNEW ORDER BY TRANDATE,TRANNO,SNO,ORDBY"
            cmd = New OleDbCommand(strSql, cn)
            cmd.CommandTimeout = 1000
            da = New OleDbDataAdapter(cmd)
            dsGridView = New DataSet()
            da.Fill(dsGridView)
            Dim dtGrid As New DataTable
            If Not dsGridView.Tables.Count > 0 Then
                MsgBox("Record Not Found", MsgBoxStyle.Information)
                Exit Sub
            End If

            dtGrid = dsGridView.Tables(0).Copy()
            gridView.DataSource = dtGrid

            strSql = " IF OBJECT_ID('TEMPTABLEDB..TEMP" & _sysid.ToString & "REPORTNEW') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP" & _sysid.ToString & "REPORTNEW "
            strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP" & _sysid.ToString & "REPORTNEWSA') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP" & _sysid.ToString & "REPORTNEWSA "
            strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP" & _sysid.ToString & "REPORTNEWSR') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP" & _sysid.ToString & "REPORTNEWSR "
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()

            For i As Integer = 0 To gridView.Columns.Count - 1
                gridView.Columns(i).SortMode = DataGridViewColumnSortMode.NotSortable
            Next

            With gridView
                .Columns("TRANDATE").DefaultCellStyle.Format = "dd-MM-yyyy"
                .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("LESSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("TAX").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("STNWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("STNRATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("STNAMT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("SNO").Visible = False
                .Columns("ORDBY").Visible = False
            End With
            If Not ResizeToolStripMenuItem.Checked Then ResizeToolStripMenuItem_Click(Me, New EventArgs)
            lblTitle.Text = "CategoryWise Transaction Report FROM " & dtpFrom.Value.ToString("dd/MM/yyyy") & " TO " & dtpTo.Value.ToString("dd/MM/yyyy") & ""
            If chkCmbCostCentre.Text <> "" Then lblTitle.Text = lblTitle.Text & vbCrLf & " COST CENTRE :" & chkCmbCostCentre.Text
            lblTitle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            ResizeToolStripMenuItem.Checked = False
            btnView_Search.Enabled = True
            'FOR RESIZE COLUMN
            gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
            gridView.Invalidate()
            For Each dgvCol As DataGridViewColumn In gridView.Columns
                dgvCol.Width = dgvCol.Width
            Next
            gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None

            gridView.Focus()
        Catch ex As Exception
            MsgBox("Predefined conditions/dbs are un matched") : Exit Sub
        End Try
    End Sub

    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        SalesAbs()
    End Sub
    Private Sub frmSalesAbstract_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmSalesAbstract_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGen.Left, Me.tabGen.Top, Me.tabGen.Width, Me.tabGen.Height))
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
        strSql = " SELECT 'ALL' COMPANYNAME,'ALL' COMPANYID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COMPANYNAME,CONVERT(VARCHAR,COMPANYID),2 RESULT FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N' "
        strSql += " ORDER BY RESULT,COMPANYNAME"
        Dim dtcompany = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtcompany)
        BrighttechPack.GlobalMethods.FillCombo(chkcmbCompany, dtcompany, "COMPANYNAME", , "ALL")
        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COSTNAME,CONVERT(VARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))

        strSql = " SELECT 'ALL' CATNAME,'ALL' CATCODE,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT CATNAME,CONVERT(VARCHAR,CATCODE),2 RESULT FROM " & cnAdminDb & "..CATEGORY"
        strSql += " ORDER BY RESULT,CATNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCategory, dtCostCentre, "CATNAME", , "ALL")
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        gridView.DataSource = Nothing
        btnView_Search.Enabled = True
        dtpFrom.Focus()

    End Sub
    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If e.KeyChar = "X" Or e.KeyChar = "x" Then
            Me.btnExport_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = "P" Then
            btnPrint_Click(Me, New EventArgs)
        End If
    End Sub
    Private Sub dtpFrom_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        dtpTo.MinimumDate = dtpFrom.Value
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        tabMain.SelectedTab = tabGen
        dtpFrom.Focus()
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
        End If
    End Sub

    Private Sub frmSalesRegister_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Escape Then
            tabMain.SelectedTab = tabGen
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text.ToString, gridView, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export)
        End If
        Me.Cursor = Cursors.Arrow
    End Sub

End Class
