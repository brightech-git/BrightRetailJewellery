Imports System.Data.OleDb
Public Class frmBalanceSheet_Format1
    Dim strSql As String = ""
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Dim dt As DataTable
    Dim dtCostCentre As DataTable
    Dim dtCompany As DataTable
    Dim MultiCostcentre As Boolean = IIf(GetAdmindbSoftValue("CCWISE_FINRPT", "N", ) = "Y", True, False)
    Dim Dtcostid As DataTable
    Dim Rempty As Boolean = False
    Dim Closin_stk As String = GetAdmindbSoftValue("CLSSTK_POST_PL", "L", )
    Dim BLSHT_CLSTK_CONTRA As String = IIf(GetAdmindbSoftValue("BLSHT_CLSTK_CONTRA", "Y", ) = "Y", True, False)
    Dim TRPLFROM_CATETRPL As Boolean = IIf(GetAdmindbSoftValue("RPT_BSPL_CATEPL", "N", ) = "Y", True, False)
    Dim BALSHEET_EXPENSEINCOME As Boolean = IIf(GetAdmindbSoftValue("BALSHEET_EXPENSEINCOME", "N", ) = "Y", True, False)

    Private Sub frmBalanceSheet_Format1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmBalanceSheet_Format1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.WindowState = FormWindowState.Maximized
        dtpDate.Value = DateTime.Today.Date
        ''COSTCENTRE
        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False

        strSql = " SELECT COMPANYNAME,COMPANYID,2 RESULT FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N' "
        strSql += " ORDER BY RESULT,COMPANYNAME"
        dtCompany = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCompany)
        BrighttechPack.GlobalMethods.FillCombo(cmbCompany, dtCompany, "COMPANYNAME", True)
        btnView.Focus()

        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub funcView()
        strSql = " SELECT COMPANYID  FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & cmbCompany.Text & "'"
        Dim strCompId As String = BrighttechPack.GetSqlValue(cn, strSql, "COMPANYID", "")
        Dim strCostId As String = ""
        If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
            strCostId += " AND COSTID IN"
            strCostId += "(SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & GetQryString(chkCmbCostCentre.Text) & "))"
        End If
        Dim COSTNAME As String
        If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
            COSTNAME = GetQryString(chkCmbCostCentre.Text)
        End If



        If Closin_stk = "N" Then
            strSql = "  IF EXISTS (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "BALSHEET10')"
            strSql += vbCrLf + "   DROP TABLE TEMPTABLEDB..TEMP" & systemId & "BALSHEET10"
            strSql += vbCrLf + "  SELECT 1 AS DISPORDER,"
            strSql += vbCrLf + "  CONVERT(VARCHAR(50),'CLOSING STOCK') PARTICULARS"
            strSql += vbCrLf + "  ,SUM(VALUE) AS AMOUNT"
            strSql += vbCrLf + "  INTO TEMPTABLEDB..TEMP" & systemId & "BALSHEET10"
            strSql += vbCrLf + "  FROM " & cnStockDb & "..CLOSINGVAL WHERE COMPANYID = '" & strCompId & "'" & strCostId & " "
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        Else
            Dim SYSID As String = systemId
            strSql = "  IF EXISTS (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "BALSHEET10')"
            strSql += vbCrLf + "   DROP TABLE TEMPTABLEDB..TEMP" & systemId & "BALSHEET10"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = "  IF EXISTS (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMPCSTKVALUE4" & SYSID & "')"
            strSql += vbCrLf + "   DROP TABLE TEMPTABLEDB..TEMPCSTKVALUE4" & SYSID & ""
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            Dim Qry As String = ""
            'ProgressBarShow()
            If Closin_stk = "W" Then
                'ProgressBarStep("Closing Stock arrival Weighted avg method", 10)
                Qry = " EXEC " & cnAdminDb & "..SP_CLOSING_STOCK"
            Else
                'ProgressBarStep("Closing Stock arrival LIFO method", 10)
                Qry = " EXEC " & cnAdminDb & "..SP_CLOSINGSTOCK_LIFO"
            End If
            Qry += vbCrLf + " @DBNAME = '" & cnStockDb & "'"
            Qry += vbCrLf + " ,@ADMINDB = '" & cnAdminDb & "'"
            Qry += vbCrLf + " ,@FRMDATE = '" & cnTranFromDate.Date.ToString("yyyy-MM-dd") & "'"
            Qry += vbCrLf + " ,@TODATE = '" & Format(dtpDate.Value, "yyyy-MM-dd") & "'"
            Qry += vbCrLf + " ,@METALNAME = 'ALL'"
            Qry += vbCrLf + " ,@CATCODE = 'ALL'"
            Qry += vbCrLf + " ,@CATNAME = 'ALL'"
            Qry += vbCrLf + " ,@COSTNAME = '" & GetQryString(chkCmbCostCentre.Text).Replace("'", "") & "'"
            Qry += vbCrLf + " ,@COMPANYID = '" & strCompId & "'"
            Qry += vbCrLf + " ,@RPTTYPE = 'S'"
            Qry += vbCrLf + " ,@SYSTEMID = '" & SYSID & "'"
            cmd = New OleDbCommand(Qry, cn) : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()

            strSql = "  IF EXISTS (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "BALSHEET10')"
            strSql += vbCrLf + "   DROP TABLE TEMPTABLEDB..TEMP" & systemId & "BALSHEET10"
            strSql += vbCrLf + "  SELECT 1 AS DISPORDER,"
            strSql += vbCrLf + "  CONVERT(VARCHAR(50),'CLOSING STOCK') PARTICULARS"
            strSql += vbCrLf + "  ,SUM(CAMOUNT) AS AMOUNT"
            strSql += vbCrLf + "  INTO TEMPTABLEDB..TEMP" & systemId & "BALSHEET10"
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMPCSTKVALUE4" & SYSID & " A "
            strSql += " WHERE A.RESULT=3 AND COLHEAD='G'"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()
        End If

        strSql = vbCrLf + " EXEC " & cnAdminDb & "..PROC_BALANCESHEET_F1"
        strSql += vbCrLf + " @DBNAME = '" & cnStockDb & "'"
        strSql += vbCrLf + " ,@COMPANYID = '" & strCompId & "'"
        strSql += vbCrLf + " ,@FROMDATE = '" & cnTranFromDate.Date.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@TODATE = '" & Format(dtpDate.Value, "yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@SYSTEMID='" & systemId & "'"
        If chkCmbCostCentre.Text = "ALL" Or chkCmbCostCentre.Text = "" Then
            strSql += vbCrLf + ",@COSTNAME = 'ALL'"
        Else
            strSql += vbCrLf + ",@COSTNAME = """ & COSTNAME & """"
        End If
        strSql += vbCrLf + ",@TYPE = '" & IIf(rbtSummary.Checked, "S", "D") & "'"
        strSql += vbCrLf + ",@FORMAT = '" & IIf(rbtAccount.Checked, "A", "R") & "'"
        strSql += vbCrLf + ",@EXPENSE = '" & IIf(BALSHEET_EXPENSEINCOME, "Y", "N") & "'"
        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + "SELECT * FROM TEMPTABLEDB..TEMPBALANCESHEET_FINAL "
        Dim DT As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(DT)
        gridView.DataSource = DT


        If gridView.RowCount > 0 Then
            lblTitle.Text = cmbCompany.Text & "BALANCE SHEET FOR THE PERIOD OF - 01/04/" & (IIf(dtpDate.Value.Date.Month > 3, dtpDate.Value.Date.Year, dtpDate.Value.Date.Year - 1)).ToString() & " - " & dtpDate.Text.Replace("-", "/")
            If chkCmbCostCentre.Text <> "" Then lblTitle.Text = lblTitle.Text & " COST CENTRE :" & chkCmbCostCentre.Text
            gridView.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Regular)
            gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            gridView.Font = New Font("VERDANA", 8, FontStyle.Regular)
            If gridView.Columns.Contains("LIAACGRPNAME1") Then gridView.Columns("LIAACGRPNAME1").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft

            If gridView.Columns.Contains("LIAACGRPNAME1") Then gridView.Columns("LIAACGRPNAME1").HeaderText = "LIABILITIES"
            If rbtReport.Checked Then
                If gridView.Columns.Contains("LIAACGRPNAME1") Then gridView.Columns("LIAACGRPNAME1").HeaderText = "PARTICULARS"
            End If

            If gridView.Columns.Contains("LIAACGRPNAME1") Then gridView.Columns("LIAACGRPNAME1").DefaultCellStyle.Font = New Font("VERDANA", 7, FontStyle.Regular)
            If gridView.Columns.Contains("LIAACGRPNAME1") Then gridView.Columns("LIAACGRPNAME1").Width = 220
            If gridView.Columns.Contains("LIABILITY") Then gridView.Columns("LIABILITY").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            If gridView.Columns.Contains("LIABILITY") Then gridView.Columns("LIABILITY").HeaderText = "" 'CREDIT
            If gridView.Columns.Contains("LIABILITY") Then gridView.Columns("LIABILITY").Width = 125
            If gridView.Columns.Contains("LIABILITY1") Then gridView.Columns("LIABILITY1").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            If gridView.Columns.Contains("LIABILITY1") Then gridView.Columns("LIABILITY1").HeaderText = "" 'DEBIT
            If gridView.Columns.Contains("LIABILITY1") Then gridView.Columns("LIABILITY1").Width = 125
            If gridView.Columns.Contains("ASSACGRPNAME1") Then gridView.Columns("ASSACGRPNAME1").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            If gridView.Columns.Contains("ASSACGRPNAME1") Then gridView.Columns("ASSACGRPNAME1").HeaderText = "ASSETS"
            If gridView.Columns.Contains("ASSACGRPNAME1") Then gridView.Columns("ASSACGRPNAME1").Width = 220
            If gridView.Columns.Contains("ASSACGRPNAME1") Then gridView.Columns("ASSACGRPNAME1").DefaultCellStyle.Font = New Font("VERDANA", 7, FontStyle.Regular)
            If gridView.Columns.Contains("ASSET") Then gridView.Columns("ASSET").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            If gridView.Columns.Contains("ASSET") Then gridView.Columns("ASSET").Width = 125
            If gridView.Columns.Contains("ASSET") Then gridView.Columns("ASSET").HeaderText = "" 'CREDIT
            If gridView.Columns.Contains("ASSET1") Then gridView.Columns("ASSET1").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            If gridView.Columns.Contains("ASSET1") Then gridView.Columns("ASSET1").Width = 125
            If gridView.Columns.Contains("ASSET1") Then gridView.Columns("ASSET1").HeaderText = "" 'DEBIT
            If gridView.Columns.Contains("COLHEAD3") Then gridView.Columns("COLHEAD3").Visible = False
            If gridView.Columns.Contains("COLHEAD4") Then gridView.Columns("COLHEAD4").Visible = False
            If gridView.Columns.Contains("REPORT") Then gridView.Columns("REPORT").Visible = False

            For i As Integer = 0 To gridView.RowCount - 1
                If gridView.Rows(i).Cells("COLHEAD3").Value.ToString() = "T" OrElse gridView.Rows(i).Cells("COLHEAD3").Value.ToString() = "T2" Then
                    gridView.Rows(i).Cells("LIAACGRPNAME1").Style.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    gridView.Rows(i).Cells("LIABILITY").Style.Font = New Font("VERDANA", 8, FontStyle.Bold)
                ElseIf gridView.Rows(i).Cells("COLHEAD3").Value.ToString() = "T1" Then
                    gridView.Rows(i).Cells("LIAACGRPNAME1").Style.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    gridView.Rows(i).Cells("LIABILITY").Style.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    gridView.Rows(i).Cells("LIAACGRPNAME1").Style.ForeColor = Color.Blue
                    gridView.Rows(i).Cells("LIABILITY").Style.ForeColor = Color.Blue


                    If gridView.Columns.Contains("ASSACGRPNAME1") Then
                        gridView.Rows(i).Cells("ASSACGRPNAME1").Style.ForeColor = Color.Blue
                        gridView.Rows(i).Cells("ASSACGRPNAME1").Style.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    End If

                    If gridView.Columns.Contains("ASSET") Then
                        gridView.Rows(i).Cells("ASSET").Style.ForeColor = Color.Blue
                        gridView.Rows(i).Cells("ASSET").Style.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    End If
                    If gridView.Columns.Contains("ASSET") Then
                        gridView.Rows(i).Cells("ASSET1").Style.ForeColor = Color.Blue
                        gridView.Rows(i).Cells("ASSET1").Style.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    End If


                ElseIf gridView.Rows(i).Cells("COLHEAD3").Value.ToString() = "S" OrElse gridView.Rows(i).Cells("COLHEAD3").Value.ToString() = "S1" OrElse gridView.Rows(i).Cells("COLHEAD3").Value.ToString() = "S2" Then
                    gridView.Rows(i).Cells("LIABILITY").Style.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    gridView.Rows(i).Cells("LIABILITY1").Style.Font = New Font("VERDANA", 8, FontStyle.Bold)
                ElseIf gridView.Rows(i).Cells("COLHEAD3").Value.ToString() = "G" Then
                    If Convert.ToDouble(IIf(gridView.Rows(i).Cells("LIABILITY").Value.ToString() <> "", gridView.Rows(i).Cells("LIABILITY").Value.ToString(), "0")) <> 0 Then
                        gridView.Rows(i).Cells("LIAACGRPNAME1").Style.Font = New Font("VERDANA", 8, FontStyle.Bold)
                        gridView.Rows(i).Cells("LIAACGRPNAME1").Style.ForeColor = Color.Red
                        gridView.Rows(i).Cells("LIABILITY").Style.ForeColor = Color.Red
                        gridView.Rows(i).Cells("LIABILITY").Style.Font = New Font("VERDANA", 8, FontStyle.Bold)
                        gridView.Rows(i).Cells("LIABILITY1").Style.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    End If
                ElseIf gridView.Rows(i).Cells("COLHEAD3").Value.ToString() = "G1" OrElse gridView.Rows(i).Cells("COLHEAD3").Value.ToString() = "G2" Then
                    gridView.Rows(i).Cells("LIABILITY").Style.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    gridView.Rows(i).Cells("LIABILITY1").Style.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    gridView.Rows(i).Cells("LIAACGRPNAME1").Style.Font = New Font("VERDANA", 8, FontStyle.Bold)
                End If
                If gridView.Rows(i).Cells("COLHEAD4").Value.ToString() = "T" OrElse gridView.Rows(i).Cells("COLHEAD4").Value.ToString() = "T1" OrElse gridView.Rows(i).Cells("COLHEAD4").Value.ToString() = "T2" Then
                    If gridView.Columns.Contains("ASSACGRPNAME1") Then gridView.Rows(i).Cells("ASSACGRPNAME1").Style.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    If gridView.Columns.Contains("ASSET") Then gridView.Rows(i).Cells("ASSET").Style.Font = New Font("VERDANA", 8, FontStyle.Bold)
                ElseIf gridView.Rows(i).Cells("COLHEAD4").Value.ToString() = "S" OrElse gridView.Rows(i).Cells("COLHEAD4").Value.ToString() = "S1" OrElse gridView.Rows(i).Cells("COLHEAD4").Value.ToString() = "S2" Then
                    If gridView.Columns.Contains("ASSET") Then gridView.Rows(i).Cells("ASSET").Style.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    If gridView.Columns.Contains("ASSET1") Then gridView.Rows(i).Cells("ASSET1").Style.Font = New Font("VERDANA", 8, FontStyle.Bold)
                ElseIf gridView.Rows(i).Cells("COLHEAD4").Value.ToString() = "G" Then
                    If gridView.Columns.Contains("ASSET") Then
                        If Convert.ToDouble(IIf(gridView.Rows(i).Cells("ASSET").Value.ToString() <> "", gridView.Rows(i).Cells("ASSET").Value.ToString(), "0")) <> 0 Then
                            gridView.Rows(i).Cells("ASSACGRPNAME1").Style.Font = New Font("VERDANA", 8, FontStyle.Bold)
                            gridView.Rows(i).Cells("ASSACGRPNAME1").Style.ForeColor = Color.Red
                            gridView.Rows(i).Cells("ASSET").Style.ForeColor = Color.Red
                            gridView.Rows(i).Cells("ASSET").Style.Font = New Font("VERDANA", 8, FontStyle.Bold)
                            gridView.Rows(i).Cells("ASSET1").Style.Font = New Font("VERDANA", 8, FontStyle.Bold)
                        End If
                    End If
                ElseIf gridView.Rows(i).Cells("COLHEAD4").Value.ToString() = "G1" OrElse gridView.Rows(i).Cells("COLHEAD4").Value.ToString() = "G2" Then
                    If gridView.Columns.Contains("ASSET") Then gridView.Rows(i).Cells("ASSET").Style.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    If gridView.Columns.Contains("ASSET1") Then gridView.Rows(i).Cells("ASSET1").Style.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    If gridView.Columns.Contains("ASSACGRPNAME1") Then gridView.Rows(i).Cells("ASSACGRPNAME1").Style.Font = New Font("VERDANA", 8, FontStyle.Bold)
                End If
            Next

            If gridView.RowCount > 0 Then
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                gridView.Invalidate()
                For Each dgvCol As DataGridViewColumn In gridView.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End If
            gridView.Focus()
        Else
            MessageBox.Show("Message", "Records not found...", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            dtpDate.Focus()
        End If
    End Sub

    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView.Click
        gridView.DataSource = Nothing
        gridViewHead.DataSource = Nothing
        funcView()
        gridViewHead.Visible = False
        Prop_Sets()
    End Sub

    Public Function funcHeaderNew() As Integer
        Dim dtheader As New DataTable
        dtheader.Clear()
        Try
            Dim dtMergeHeader As New DataTable("~MERGEHEADER")
            With dtMergeHeader
                For i As Integer = 0 To Dtcostid.Rows.Count - 1
                    .Columns.Add("LIAACGRPNAME1" & Dtcostid.Rows(i).Item(0).ToString.Trim & "~LIABILITY" & Dtcostid.Rows(i).Item(0).ToString.Trim & "~LIABILITY1" & Dtcostid.Rows(i).Item(0).ToString.Trim & "~ASSACGROUPNAME1" & Dtcostid.Rows(i).Item(0).ToString.Trim & "~ASSET" & Dtcostid.Rows(i).Item(0).ToString.Trim & "~ASSET1" & Dtcostid.Rows(i).Item(0).ToString.Trim & "", GetType(String))
                    .Columns.Add("COLHEAD3" & Dtcostid.Rows(i).Item(0).ToString.Trim, GetType(String))
                    .Columns.Add("COLHEAD4" & Dtcostid.Rows(i).Item(0).ToString.Trim, GetType(String))
                Next
                .Columns.Add("SCROLL", GetType(String))
                For i As Integer = 0 To Dtcostid.Rows.Count - 1
                    .Columns("LIAACGRPNAME1" & Dtcostid.Rows(i).Item(0).ToString.Trim & "~LIABILITY" & Dtcostid.Rows(i).Item(0).ToString.Trim & "~LIABILITY1" & Dtcostid.Rows(i).Item(0).ToString.Trim & "~ASSACGROUPNAME1" & Dtcostid.Rows(i).Item(0).ToString.Trim & "~ASSET" & Dtcostid.Rows(i).Item(0).ToString.Trim & "~ASSET1" & Dtcostid.Rows(i).Item(0).ToString.Trim & "").Caption = Dtcostid.Rows(i).Item(1).ToString
                    .Columns("COLHEAD3" & Dtcostid.Rows(i).Item(0).ToString.Trim).Caption = ""
                    .Columns("COLHEAD4" & Dtcostid.Rows(i).Item(0).ToString.Trim).Caption = ""
                Next
                .Columns("SCROLL").Caption = ""
            End With
            gridViewHead.DataSource = dtMergeHeader
            funcGridHeaderStyle()
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function
    Public Function funcGridHeaderStyle() As Integer
        With gridViewHead
            For i As Integer = 0 To Dtcostid.Rows.Count - 1
                With .Columns("LIAACGRPNAME1" & Dtcostid.Rows(i).Item(0).ToString.Trim & "~LIABILITY" & Dtcostid.Rows(i).Item(0).ToString.Trim & "~LIABILITY1" & Dtcostid.Rows(i).Item(0).ToString.Trim & "~ASSACGROUPNAME1" & Dtcostid.Rows(i).Item(0).ToString.Trim & "~ASSET" & Dtcostid.Rows(i).Item(0).ToString.Trim & "~ASSET1" & Dtcostid.Rows(i).Item(0).ToString.Trim)
                    .Width = gridView.Columns("LIAACGRPNAME1" & Dtcostid.Rows(i).Item(0).ToString.Trim).Width + gridView.Columns("LIABILITY" & Dtcostid.Rows(i).Item(0).ToString.Trim).Width + gridView.Columns("LIABILITY1" & Dtcostid.Rows(i).Item(0).ToString.Trim).Width + gridView.Columns("ASSACGROUPNAME1" & Dtcostid.Rows(i).Item(0).ToString.Trim).Width + gridView.Columns("ASSET" & Dtcostid.Rows(i).Item(0).ToString.Trim).Width + gridView.Columns("ASSET1" & Dtcostid.Rows(i).Item(0).ToString.Trim).Width
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .HeaderText = Dtcostid.Rows(i).Item(1).ToString
                End With
                With .Columns("COLHEAD3" & Dtcostid.Rows(i).Item(0).ToString.Trim)
                    .Width = gridView.Columns("COLHEAD3" & Dtcostid.Rows(i).Item(0).ToString.Trim).Width
                    .Visible = gridView.Columns("COLHEAD3" & Dtcostid.Rows(i).Item(0).ToString.Trim).Visible
                    .HeaderText = ""
                End With
                With .Columns("COLHEAD4" & Dtcostid.Rows(i).Item(0).ToString.Trim)
                    .Width = gridView.Columns("COLHEAD4" & Dtcostid.Rows(i).Item(0).ToString.Trim).Width
                    .Visible = gridView.Columns("COLHEAD4" & Dtcostid.Rows(i).Item(0).ToString.Trim).Visible
                    .HeaderText = ""
                End With
            Next
            With .Columns("SCROLL")
                .Width = 20
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .HeaderText = ""
            End With
            gridViewHead.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 9, FontStyle.Bold)
            gridViewHead.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        End With
    End Function

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        If cmbCompany.Items.Count > 0 Then cmbCompany.SelectedIndex = 0
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        dtpDate.Value = DateTime.Today.Date
        gridView.DataSource = Nothing
        gridViewHead.DataSource = Nothing
        lblTitle.Text = ""
        Prop_Gets()
        cmbCompany.Focus()
    End Sub

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        'If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.RowCount > 0 Then
            BrightPosting.GExport.Post(Me.Name, "", lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export, gridViewHead)
        End If
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        'If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.RowCount > 0 Then
            BrightPosting.GExport.Post(Me.Name, "", lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print, gridViewHead)
        End If
    End Sub

    Private Sub gridView_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs) Handles gridView.ColumnWidthChanged
        With gridViewHead
            If .ColumnCount > 0 Then
                For i As Integer = 0 To Dtcostid.Rows.Count - 1
                    .Columns("LIAACGRPNAME1" & Dtcostid.Rows(i).Item(0).ToString.Trim & "~LIABILITY" & Dtcostid.Rows(i).Item(0).ToString.Trim & "~LIABILITY1" & Dtcostid.Rows(i).Item(0).ToString.Trim & "~ASSACGROUPNAME1" & Dtcostid.Rows(i).Item(0).ToString.Trim & "~ASSET" & Dtcostid.Rows(i).Item(0).ToString.Trim & "~ASSET1" & Dtcostid.Rows(i).Item(0).ToString.Trim).Width = gridView.Columns("LIAACGRPNAME1" & Dtcostid.Rows(i).Item(0).ToString.Trim).Width + gridView.Columns("LIABILITY" & Dtcostid.Rows(i).Item(0).ToString.Trim).Width + gridView.Columns("LIABILITY1" & Dtcostid.Rows(i).Item(0).ToString.Trim).Width + gridView.Columns("ASSACGROUPNAME1" & Dtcostid.Rows(i).Item(0).ToString.Trim).Width + gridView.Columns("ASSET" & Dtcostid.Rows(i).Item(0).ToString.Trim).Width + gridView.Columns("ASSET1" & Dtcostid.Rows(i).Item(0).ToString.Trim).Width
                    .Columns("COLHEAD3" & Dtcostid.Rows(i).Item(0).ToString.Trim).Width = gridView.Columns("COLHEAD3" & Dtcostid.Rows(i).Item(0).ToString.Trim).Width
                    .Columns("COLHEAD4" & Dtcostid.Rows(i).Item(0).ToString.Trim).Width = gridView.Columns("COLHEAD4" & Dtcostid.Rows(i).Item(0).ToString.Trim).Width
                Next
                .Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
                .Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
            End If
        End With
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar.ToString().ToUpper = "P" Then
            btnPrint_Click(Me, New EventArgs)
        ElseIf e.KeyChar.ToString().ToUpper = "X" Then
            btnExport_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub viewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles viewToolStripMenuItem.Click
        btnView_Click(Me, New EventArgs)
    End Sub

    Private Sub newToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles newToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub exitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles exitToolStripMenuItem.Click
        btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New frmBalanceSheet_Format1_Properties
        obj.p_cmbCompany = cmbCompany.Text
        'GetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre)
        SetSettingsObj(obj, Me.Name, GetType(frmBalanceSheet_Format1_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmBalanceSheet_Format1_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmBalanceSheet_Format1_Properties))
        cmbCompany.Text = obj.p_cmbCompany
        'SetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre, cnCostName)
    End Sub

    Private Sub gridView_Scroll(ByVal sender As Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles gridView.Scroll
        If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
            gridViewHead.HorizontalScrollingOffset = e.NewValue
        End If
        If gridViewHead.DataSource Is Nothing Then Exit Sub
        Try
            If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
                gridViewHead.HorizontalScrollingOffset = e.NewValue
                gridViewHead.Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
                gridViewHead.Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try
    End Sub

    Private Sub btnView_DragLeave(sender As Object, e As EventArgs) Handles btnView.DragLeave

    End Sub
End Class

Public Class frmBalanceSheet_Format1_Properties
    Private cmbCompany As String = "ALL"
    Public Property p_cmbCompany() As String
        Get
            Return cmbCompany
        End Get
        Set(ByVal value As String)
            cmbCompany = value
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

End Class