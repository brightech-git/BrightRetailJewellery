Imports System.Data.OleDb
Public Class frmBagNoGeneration
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Dim dtGrid As DataTable
    Private WithEvents chkCheckAll As New CheckBox
    Dim BulkCheck As Boolean = False
    Dim dtCompany As New DataTable
    Dim prnmemsuffix As String = ""
    Dim objManualBill As frmManualBillNoGen
    Dim MANBILLNO As Boolean = IIf(GetAdmindbSoftValue("MANBAGNO", "Y") = "Y", True, False)
    Dim BAGNO_INCDIRPUR As Boolean = IIf(GetAdmindbSoftValue("BAGNO_INCDIRPUR", "N") = "Y", True, False)
    Private Sub frmBagNoGeneration_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If tabMain.SelectedTab.Name = tabView.Name Then btnBack_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub frmBagNoGeneration_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmBagNoGeneration_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGeneral.Left, Me.tabGeneral.Top, Me.tabGeneral.Width, Me.tabGeneral.Height))

        chkCheckAll.Name = "chkCheckAll"
        chkCheckAll.Text = ""
        chkCheckAll.AutoSize = True
        AddHandler chkCheckAll.CheckedChanged, AddressOf check_Changed
        gridView.Controls.Add(chkCheckAll)
        grpContainer.Location = New Point((ScreenWid - grpContainer.Width) / 2, ((ScreenHit - 128) - grpContainer.Height) / 2)


        cmbMetal_MAN.Items.Add("ALL")
        strSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE ACTIVE = 'Y' ORDER BY METALNAME"
        objGPack.FillCombo(strSql, cmbMetal_MAN, False, False)

        cmbItemType.Items.Add("ALL")
        strSql = " SELECT NAME FROM " & cnAdminDb & "..ITEMTYPE ORDER BY NAME"
        objGPack.FillCombo(strSql, cmbItemType, False, False)

        cmbCostcentre.Items.Add("ALL")
        strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
        objGPack.FillCombo(strSql, cmbCostcentre, False, False)

        strSql = " SELECT 'ALL' COMPANYNAME,'ALL' COMPANYID,1 RESULT"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT COMPANYNAME,COMPANYID,2 RESULT FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N'"
        strSql += vbCrLf + " ORDER BY RESULT,COMPANYNAME"
        dtCompany = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCompany)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dtCompany, "COMPANYNAME", , "ALL")
        cmbItemType.Text = "ALL"        
        cmbMetal_MAN.Text = "ALL"
        cmbCostcentre.Text = "ALL"
        cmbCategory_MAN.Items.Add("ALL")
        strSql = "SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY"
        strSql += vbCrLf + "  WHERE METALID IN  (SELECT METALID FROM " & cnAdminDb & "..METALMAST)"
        strSql += vbCrLf + "  ORDER BY CATNAME"
        objGPack.FillCombo(strSql, cmbCategory_MAN, False, False)
        cmbCategory_MAN.Text = "ALL"
       
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub check_Changed(ByVal sender As Object, ByVal e As EventArgs)
        BulkCheck = True
        For Each dgvRow As DataGridViewRow In gridView.Rows
            dgvRow.Cells("CHECK").Value = chkCheckAll.Checked
        Next
        BulkCheck = False
        CalcSelectedValues()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        tabMain.SelectedTab = tabGeneral
        Dim lastBagNo As Integer = Val(GetTranDbSoftControlValue("BAGNO", False)) - 1
        If Val(lastBagNo) > 0 Then
            lblLastBagNo.Text = "Last BagNo : " & cnCostId & lastBagNo.ToString
            lblLastBagNo1.Text = "Last BagNo : " & cnCostId & lastBagNo.ToString
        Else
            lblLastBagNo.Text = ""
            lblLastBagNo1.Text = ""
        End If
        dtpFrom.Value = GetEntryDate(GetServerDate)
        dtpTo.Value = GetEntryDate(GetServerDate)     
        dtpFrom.Select()
    End Sub


    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGeneral
        dtpFrom.Select()
    End Sub

    Private Sub CalcSelectedValues()
        Dim dt As New DataTable
        dt = CType(gridView.DataSource, DataTable)
        dt.AcceptChanges()
        Dim pcs As Integer = Val(dt.Compute("SUM(PCS)", "CHECK = TRUE").ToString)
        Dim grsWt As Double = Val(dt.Compute("SUM(GRSWT)", "CHECK = TRUE").ToString)
        Dim netWt As Double = Val(dt.Compute("SUM(NETWT)", "CHECK = TRUE").ToString)
        Dim amount As Double = Val(dt.Compute("SUM(AMOUNT)", "CHECK = TRUE").ToString)
        lblPcs.Text = IIf(pcs <> 0, pcs, "")
        lblGrsWt.Text = IIf(grsWt <> 0, Format(grsWt, "0.000"), "")
        lblNetWt.Text = IIf(netWt <> 0, Format(netWt, "0.000"), "")
        lblAmount.Text = IIf(amount <> 0, Format(amount, "0.00"), "")
    End Sub

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim CompanyId As String = GetQryStringForSp(chkCmbCompany.Text, cnAdminDb & "..COMPANY", "COMPANYID", "COMPANYNAME", False)
        If objGPack.Validator_Check(Me) Then Exit Sub
        gridView.DataSource = Nothing
        strSql = " SELECT TRANNO,TRANDATE"
        strSql += vbCrLf + "  ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(AMOUNT)AMOUNT"
        strSql += vbCrLf + "  ,(SELECT NAME FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = R.ITEMTYPEID)AS ITEMTYPE"
        strSql += vbCrLf + "  ,BATCHNO,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE=R.CATCODE)CATNAME"
        strSql += vbCrLf + "  ,'REC' AS TYPE"
        strSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT AS R"
        strSql += vbCrLf + "  WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + "  AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        If cmbCategory_MAN.Text <> "ALL" Then
            strSql += vbCrLf + "  AND CATCODE = (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & cmbCategory_MAN.Text & "')"
        End If
        If BAGNO_INCDIRPUR Then
            strSql += vbCrLf + "  AND TRANTYPE IN('RPU','PU')"
        Else
            strSql += vbCrLf + "  AND TRANTYPE = 'PU'"
        End If
        strSql += vbCrLf + "  AND ISNULL(CANCEL,'') = ''"
        If chkCmbCompany.Text <> "ALL" And chkCmbCompany.Text <> "" Then strSql += vbCrLf + " AND COMPANYID IN('" & CompanyId.Replace(",", "','") & "')"        
        If cmbCostcentre.Text <> "ALL" Then
            strSql += vbCrLf + "  AND COSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostcentre.Text & "')"
        End If
        strSql += vbCrLf + "  AND ISNULL(BAGNO,'') = ''"
        If GetAdmindbSoftValue("PENDTRF2NONTAG", "Y") = "B" Then
            strSql += vbCrLf + "  AND ISNULL(TRFNO,'') <> ''"
        End If
        If cmbMetal_MAN.Text <> "ALL" Then
            strSql += vbCrLf + "  AND METALID=(SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME='" & cmbMetal_MAN.Text & "')"
        End If
        If cmbItemType.Text <> "ALL" And cmbItemType.Text <> "" Then
            strSql += vbCrLf + "  AND ISNULL(ITEMTYPEID,0) = ISNULL((SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & cmbItemType.Text & "'),0)"
        End If
        strSql += vbCrLf + "  GROUP BY TRANNO,TRANDATE,BATCHNO,ITEMTYPEID,CATCODE "
        If BAGNO_INCDIRPUR Then
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT NULL TRANNO,'" & cnTranFromDate & "' TRANDATE"
            strSql += vbCrLf + "  ,SUM(CASE WHEN TRANTYPE='R' THEN PCS ELSE -1*PCS END)PCS"
            strSql += vbCrLf + "  ,SUM(CASE WHEN TRANTYPE='R' THEN GRSWT ELSE -1*GRSWT END)GRSWT"
            strSql += vbCrLf + "  ,SUM(CASE WHEN TRANTYPE='R' THEN NETWT ELSE -1*NETWT END)NETWT"
            strSql += vbCrLf + "  ,SUM(CASE WHEN TRANTYPE='R' THEN AMOUNT ELSE -1*AMOUNT END)AMOUNT"
            strSql += vbCrLf + "  ,NULL AS ITEMTYPE"
            strSql += vbCrLf + "  ,SNO AS BATCHNO ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE=R.CATCODE)CATNAME"
            strSql += vbCrLf + "  ,'OPE' AS TYPE"
            strSql += vbCrLf + "  FROM " & cnStockDb & "..OPENWEIGHT AS R"
            strSql += vbCrLf + "  WHERE 1=1"
            If cmbCategory_MAN.Text <> "ALL" Then
                strSql += vbCrLf + "  AND CATCODE = (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & cmbCategory_MAN.Text & "')"
            End If
            strSql += vbCrLf + "  AND STOCKTYPE = 'C'"
            If chkCmbCompany.Text <> "ALL" And chkCmbCompany.Text <> "" Then strSql += vbCrLf + " AND COMPANYID IN('" & CompanyId.Replace(",", "','") & "')"
            If cmbCostcentre.Text <> "ALL" Then
                strSql += vbCrLf + "  AND COSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostcentre.Text & "')"
            End If
            strSql += vbCrLf + "  AND ISNULL(BAGNO,'') = ''"
            strSql += vbCrLf + "  GROUP BY SNO,CATCODE "
        End If
        dtGrid = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        Dim dtCol As New DataColumn("CHECK", GetType(Boolean))
        dtCol.DefaultValue = True
        dtGrid.Columns.Add(dtCol)
        gridView.DataSource = dtGrid
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        Dim title As String = cmbCategory_MAN.Text + vbCrLf
        title += "BAG NO GENERATION FROM " + dtpFrom.Text + " TO " + dtpTo.Text
        lblTitle.Text = title
        tabView.Show()
        'gridView.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
        With gridView
            .Columns("CHECK").HeaderText = ""
            .Columns("CHECK").Width = 30
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
            .Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("BATCHNO").Visible = False
            .Columns("CATNAME").Visible = False
            .Columns("TYPE").Visible = False
            .Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
            For CNT As Integer = 0 To .ColumnCount - 1
                .Columns(CNT).ReadOnly = True
            Next
            .Columns("CHECK").ReadOnly = False
            .Columns("CHECK").MinimumWidth = 50
        End With
        Dim rec As Rectangle
        rec = gridView.GetCellDisplayRectangle(8, -1, True)
        chkCheckAll.Location = rec.Location + New Point(565, 4)
        chkCheckAll.Checked = True
        CalcSelectedValues()
        tabMain.SelectedTab = tabView
        gridView.Focus()
    End Sub

    Private Sub gridView_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.CellValueChanged
        If BulkCheck Then Exit Sub
        CalcSelectedValues()
    End Sub

    Private Sub gridView_CurrentCellDirtyStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.CurrentCellDirtyStateChanged
        If BulkCheck Then Exit Sub
        gridView.CommitEdit(DataGridViewDataErrorContexts.Commit)
    End Sub

    Private Sub gridView_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.SelectionChanged
        If gridView.CurrentRow IsNot Nothing Then
            gridView.CurrentCell = gridView.Rows(gridView.CurrentRow.Index).Cells("CHECK")
        End If
    End Sub

    Private Sub btnGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGenerate.Click
        Try
            Dim dt As New DataTable
            Dim CatName As String
            dt = CType(gridView.DataSource, DataTable).Copy
            Dim ros() As DataRow = Nothing
            ros = dt.Select("CHECK = TRUE")
            If Not ros.Length > 0 Then
                MsgBox("There is no checked record", MsgBoxStyle.Information)
                Exit Sub
            End If
            tran = Nothing
            tran = cn.BeginTransaction
            Dim BagNo As String = Nothing
            If MANBILLNO Then
                objManualBill = New frmManualBillNoGen
                frmManualBillNoGen.Text = "Manual BagNo"
ReEnterBillNO:
                If objManualBill.ShowDialog = Windows.Forms.DialogResult.OK Then
                    If Val(objManualBill.txtBillNo_NUM.Text) = 0 Then MsgBox("BagNo not Valid...", MsgBoxStyle.Information) : GoTo ReEnterBillNO
                    BagNo = cnCostId & "B" & Mid(Format(cnTranToDate, "dd/MM/yyyy"), 9, 2).ToString & Val(objManualBill.txtBillNo_NUM.Text)
                    strSql = " SELECT 'CHECK' FROM " & cnStockDb & "..RECEIPT"
                    strSql += vbCrLf + "  WHERE BAGNO = '" & BagNo & "'"
                    If objGPack.GetSqlValue(strSql, , , tran).Length > 0 Then
                        MsgBox("BagNo Already Exist", MsgBoxStyle.Information)
                        GoTo ReEnterBillNO
                    End If
                End If
            Else
GENBAGNO:
                'BagNo = cnCostId & GetTranDbSoftControlValue("BAGNO", True, tran)
                BagNo = cnCostId & "B" & Mid(Format(cnTranToDate, "dd/MM/yyyy"), 9, 2).ToString & GetTranDbSoftControlValue("BAGNO", True, tran)
                ''check
                strSql = " SELECT 'CHECK' FROM " & cnStockDb & "..RECEIPT"
                strSql += vbCrLf + "  WHERE BAGNO = '" & BagNo & "'"
                If objGPack.GetSqlValue(strSql, , , tran).Length > 0 Then
                    GoTo GENBAGNO
                End If
            End If
            Dim batchno As String = ""
            Dim Sno As String = ""
            For Each row As DataRow In ros
                If row.Item("TYPE").ToString = "OPE" Then
                    Sno += "'" & row.Item("BATCHNO").ToString & "',"
                Else
                    batchno += "'" & row.Item("BATCHNO").ToString & "',"
                End If
                CatName += "'" & row.Item("CATNAME").ToString & "',"
            Next
            If batchno.Length > 0 Then
                batchno = Mid(batchno, 1, batchno.Length - 1)
                CatName = Mid(CatName, 1, CatName.Length - 1)
            End If
            If Sno.Length > 0 Then
                Sno = Mid(Sno, 1, Sno.Length - 1)
            End If
            If Sno <> "" Then
                strSql = " UPDATE " & cnStockDb & "..OPENWEIGHT"
                strSql += vbCrLf + "  SET BAGNO = '" & BagNo & "'"
                strSql += vbCrLf + "  WHERE SNO IN (" & Sno & ")"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
            End If
            If batchno <> "" Then
                strSql = " UPDATE " & cnStockDb & "..RECEIPT"
                strSql += vbCrLf + "  SET MELT_RETAG='M',BAGNO = '" & BagNo & "'"
                strSql += vbCrLf + "  WHERE BATCHNO IN (" & batchno & ")"
                strSql += vbCrLf + "  AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN(" & CatName & "))"
                If cmbItemType.Text <> "ALL" And cmbItemType.Text <> "" Then
                    strSql += vbCrLf + "  AND ISNULL(ITEMTYPEID,0) = ISNULL((SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & cmbItemType.Text & "'),0)"
                End If
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
            End If
            tran.Commit()
            tran = Nothing
            MsgBox("BagNo   : " & BagNo & " Generated..")
            Try
                If IO.File.Exists(Application.StartupPath & "\BillPrint.exe") Then
                    Dim write As IO.StreamWriter
                    Dim memfile As String = "\BillPrint" & prnmemsuffix.Trim & ".mem"
                    write = IO.File.CreateText(Application.StartupPath & memfile)
                    write.WriteLine(LSet("TYPE", 15) & ":BAG")
                    write.WriteLine(LSet("BATCHNO", 15) & ":" & BagNo)
                    write.WriteLine(LSet("TRANDATE", 15) & ":")
                    write.WriteLine(LSet("DUPLICATE", 15) & ":N")
                    write.Flush()
                    write.Close()
                    If EXE_WITH_PARAM = False Then
                        System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe")
                    Else
                        System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe", _
                        LSet("TYPE", 15) & ":BAG" & ";" & _
                        LSet("BATCHNO", 15) & ":" & BagNo & ";" & _
                        LSet("TRANDATE", 15) & ":" & _
                        LSet("DUPLICATE", 15) & ":N")
                    End If

                Else
                    MsgBox("Billprint exe not found", MsgBoxStyle.Information)
                End If
            Catch ex As Exception
            End Try
            btnNew_Click(Me, New EventArgs)
        Catch ex As Exception
            If tran IsNot Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub

    Private Sub cmbMetal_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbMetal_MAN.SelectedIndexChanged
        cmbCategory_MAN.Items.Clear()
        cmbCategory_MAN.Items.Add("ALL")
        If cmbMetal_MAN.Text = "ALL" Then
            strSql = "SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY"
            strSql += vbCrLf + "  WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST)"
            strSql += vbCrLf + "  ORDER BY CATNAME"
            objGPack.FillCombo(strSql, cmbCategory_MAN, False, False)
        Else
            strSql = "SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY"
            strSql += vbCrLf + "  WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetal_MAN.Text & "')"
            strSql += vbCrLf + "  ORDER BY CATNAME"
            objGPack.FillCombo(strSql, cmbCategory_MAN, False, False)
        End If
        cmbCategory_MAN.Text = "ALL"
    End Sub


End Class