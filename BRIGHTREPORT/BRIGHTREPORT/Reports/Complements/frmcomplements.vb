Imports System.Data.OleDb
Public Class frmcomplements

#Region "Declaration"
    Dim strsql As String
    Dim _cmd As OleDbCommand
    Dim _da As OleDbDataAdapter
    Dim dtcostcentre As DataTable
#End Region

#Region "Events"
    Private Sub frmcomplements_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.F12 Then
            FuncClose()
        ElseIf e.KeyCode = Keys.F3 Then
            FuncNew()
        ElseIf e.KeyCode = Keys.F2 Then
            funcview()
        End If
    End Sub

    Private Sub ResizeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResizeToolStripMenuItem.Click
        If dgvview.RowCount > 0 Then
            If ResizeToolStripMenuItem.Checked Then
                dgvview.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                dgvview.Invalidate()
                For Each dgvCol As DataGridViewColumn In dgvview.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                dgvview.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            Else
                For Each dgvCol As DataGridViewColumn In dgvview.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                dgvview.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End If
            GridViewHeaderStyle()
        End If
    End Sub

    Private Sub frmcomplements_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmcomplements_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        FuncNew()
    End Sub

    Private Sub BtnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnExit.Click, ExitToolStripMenuItem.Click
        FuncClose()
    End Sub
    Function FuncClose() As Integer
        Me.Close()
    End Function

    Private Sub btnNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        FuncNew()
    End Sub

    Private Sub btnView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnView.Click, ViewToolStripMenuItem.Click
        If GetAdmindbSoftValue("COMPLIMENT_NONTAG", "Y") = "Y" Then
            funcviewnontag()
        Else
            funcview()
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If dgvview.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, dgvview, BrightPosting.GExport.GExportType.Print, gridViewHead)
        End If
    End Sub

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If dgvview.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, dgvview, BrightPosting.GExport.GExportType.Export, gridViewHead)
        End If
    End Sub

#End Region

#Region "Functions"
    Function FuncNew() As Integer
        If GetAdmindbSoftValue("COMPLIMENT_NONTAG", "Y") = "Y" Then
            cmbPm.Items.Add("ALL")
            strsql = " SELECT 'ALL' PMNAME"
            strsql += vbCrLf + " UNION ALL"
            strsql += vbCrLf + " SELECT ITEMNAME PMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE  ISNULL(COMPLIMENTS,'')='Y' "
            strsql += vbCrLf + " ORDER BY PMNAME"
            objGPack.FillCombo(strsql, cmbPm, , False)
            cmbPm.Text = "ALL"
            cmbPmSub.Items.Add("ALL")
            strsql = " SELECT 'ALL' PMSUBNAME"
            strsql += vbCrLf + " UNION ALL"
            strsql += vbCrLf + " SELECT SUBITEMNAME PMSUBNAME FROM " & cnAdminDb & "..SUBITEMMAST "
            strsql += vbCrLf + " WHERE ITEMID IN (SELECT ITEMID  FROM " & cnAdminDb & "..ITEMMAST WHERE  ISNULL(COMPLIMENTS,'')='Y') ORDER BY PMSUBNAME"
            objGPack.FillCombo(strsql, cmbPmSub, , False)
            cmbPmSub.Text = "ALL"
        Else
            cmbPm.Items.Add("ALL")
            strsql = " SELECT 'ALL' PMNAME"
            strsql += vbCrLf + " UNION ALL"
            strsql += vbCrLf + " SELECT PMNAME FROM " & cnAdminDb & "..PMMAST WHERE ACTIVE = 'Y'"
            strsql += vbCrLf + " ORDER BY PMNAME"
            objGPack.FillCombo(strsql, cmbPm, , False)
            cmbPm.Text = "ALL"
            cmbPmSub.Items.Add("ALL")
            strsql = " SELECT 'ALL' PMSUBNAME"
            strsql += vbCrLf + " UNION ALL"
            strsql += vbCrLf + " SELECT PMSUBNAME FROM " & cnAdminDb & "..PMSUBMAST WHERE ACTIVE = 'Y' ORDER BY PMSUBNAME"
            objGPack.FillCombo(strsql, cmbPmSub, , False)
            cmbPmSub.Text = "ALL"

        End If
        cmbCostCentre.Items.Add("ALL")
        strsql = " SELECT 'ALL' COSTNAME"
        strsql += vbCrLf + " UNION ALL"
        strsql += vbCrLf + " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
        objGPack.FillCombo(strsql, cmbCostCentre, , False)
        cmbCostCentre.Text = "ALL"
        txtTranno.Text = ""
        dgvview.DataSource = Nothing
        cmbGroupby.Items.Clear()
        cmbGroupby.Items.Add("ITEM")
        cmbGroupby.Items.Add("SUBITEM")
        cmbGroupby.Items.Add("TRANDATE")
        lblDiff.BackColor = Color.MistyRose
        lblMulti.BackColor = Color.LightGreen
        If GetAdmindbSoftValue("COMPLIMENT_NONTAG", "Y") = "Y" Then
            lblDiff.Visible = False
            lblMulti.Visible = False
            Label6.Visible = False
            txtTranno.Visible = False
            'Label4.Visible = False
            'cmbPm.Visible = False
            'Label5.Visible = False
            'cmbPmSub.Visible = False
            'Label7.Visible = False
            'cmbGroupby.Visible = False
        End If
        dtpFrom.Focus()
    End Function

    Function funcviewnontag() As Integer

        strsql = "IF OBJECT_ID('TEMPTABLEDB..TEMPPMISSUE') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPPMISSUE"
        _cmd = New OleDbCommand(strsql, cn)
        _cmd.ExecuteNonQuery()

        strsql = "SELECT "
        strsql += vbCrLf + "T.TRANDATE,T.TRANNO,T.BATCHNO,I.ITEMNAME,S.SUBITEMNAME,CONVERT(INT,T.PCS) AS IPCS , CONVERT(VARCHAR,T.TRANDATE,103) AS BILLDATE , T.TRANNO AS BILLNO"
        'strsql += vbCrLf + ",CONVERT(NUMERIC(15,0),NULL)PCS,CONVERT(NUMERIC(15,3),NULL)GRSWT,CONVERT(NUMERIC(15,3),NULL)NETWT,CONVERT(NUMERIC(15,2),NULL)AMOUNT"
        ''strsql += vbCrLf + ",SUM(ISS.PCS)PCS,SUM(ISS.GRSWT)GRSWT,SUM(ISS.NETWT)NETWT,SUM(ISS.AMOUNT)AMOUNT"
        strsql += vbCrLf + " ,(SELECT SUM(PCS) FROM " & cnStockDb & "..ISSUE WHERE BATCHNO = T.BATCHNO  AND ISNULL(CANCEL,'')='' ) PCS "
        strsql += vbCrLf + " ,(SELECT SUM(GRSWT) FROM " & cnStockDb & "..ISSUE WHERE BATCHNO = T.BATCHNO AND ISNULL(CANCEL,'')='' ) GRSWT "
        strsql += vbCrLf + " ,(SELECT SUM(NETWT) FROM " & cnStockDb & "..ISSUE WHERE BATCHNO = T.BATCHNO AND ISNULL(CANCEL,'')='') NETWT "
        strsql += vbCrLf + " ,(SELECT SUM(AMOUNT) FROM " & cnStockDb & "..ISSUE WHERE BATCHNO = T.BATCHNO AND ISNULL(CANCEL,'')='') AMOUNT "
        strsql += vbCrLf + ",2 AS RESULT"
        strsql += vbCrLf + "INTO TEMPTABLEDB..TEMPPMISSUE"
        strsql += vbCrLf + "FROM " & cnStockDb & "..PMTRAN AS T"
        strsql += vbCrLf + "INNER JOIN " & cnAdminDb & "..ITEMMAST AS I ON I.ITEMID=T.PMID"
        strsql += vbCrLf + "LEFT OUTER JOIN " & cnAdminDb & "..SUBITEMMAST AS S ON S.SUBITEMID=T.PMSUBID"
        strsql += vbCrLf + "WHERE T.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpToDate.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(T.CANCEL,'')=''"
        If cmbCostCentre.Text <> "ALL" Then
            strsql += vbCrLf + " AND T.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN ('" & cmbCostCentre.Text.ToString & "'))"
        End If
        If cmbPm.Text <> "ALL" Then
            strsql += vbCrLf + " AND T.PMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN ('" & cmbPm.Text.ToString & "'))"
        End If
        If cmbPmSub.Text <> "ALL" Then
            strsql += vbCrLf + " AND T.PMSUBID IN (SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME IN ('" & cmbPmSub.Text.ToString & "'))"
        End If
        strsql += vbCrLf + "ORDER BY T.TRANDATE,T.TRANNO"
        _cmd = New OleDbCommand(strsql, cn)
        _cmd.ExecuteNonQuery()

        '' CHANGE FOR MALLIGA JEWELLARY REF CHANRA MOHAN 29-04-2022


        'strsql = "INSERT INTO TEMPTABLEDB..TEMPPMISSUE"
        'strsql += vbCrLf + "(TRANDATE,TRANNO,BATCHNO,ITEMNAME,SUBITEMNAME,IPCS,BILLDATE,BILLNO,PCS,GRSWT,NETWT,AMOUNT,RESULT)"
        'strsql += vbCrLf + "SELECT "
        'strsql += vbCrLf + "T.TRANDATE,T.TRANNO,T.BATCHNO,CONVERT(VARCHAR(12),T.TRANDATE,105) ITEMNAME,CONVERT(VARCHAR(12),T.TRANNO) SUBITEMNAME,NULL AS IPCS"
        ''strsql += vbCrLf + ",SUM(ISS.PCS)PCS,SUM(ISS.GRSWT)GRSWT,SUM(ISS.NETWT)NETWT,SUM(ISS.AMOUNT)AMOUNT"
        'strsql += vbCrLf + " ,(SELECT SUM(PCS) FROM " & cnStockDb & "..ISSUE WHERE BATCHNO = T.BATCHNO  AND ISNULL(CANCEL,'')='' ) PCS "
        'strsql += vbCrLf + " ,(SELECT SUM(GRSWT) FROM " & cnStockDb & "..ISSUE WHERE BATCHNO = T.BATCHNO AND ISNULL(CANCEL,'')='' ) GRSWT "
        'strsql += vbCrLf + " ,(SELECT SUM(NETWT) FROM " & cnStockDb & "..ISSUE WHERE BATCHNO = T.BATCHNO AND ISNULL(CANCEL,'')='') NETWT "
        'strsql += vbCrLf + " ,(SELECT SUM(AMOUNT) FROM " & cnStockDb & "..ISSUE WHERE BATCHNO = T.BATCHNO AND ISNULL(CANCEL,'')='') AMOUNT "
        'strsql += vbCrLf + ",0 AS RESULT"
        'strsql += vbCrLf + "FROM " & cnStockDb & "..PMTRAN AS T"
        ''strsql += vbCrLf + "INNER JOIN " & cnStockDb & "..ISSUE AS ISS ON ISS.BATCHNO=T.BATCHNO"
        'strsql += vbCrLf + "WHERE T.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpToDate.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(T.CANCEL,'')=''"
        'If cmbPm.Text <> "ALL" Then
        '    strsql += vbCrLf + " AND T.PMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN ('" & cmbPm.Text.ToString & "'))"
        'End If
        'If cmbPmSub.Text <> "ALL" Then
        '    strsql += vbCrLf + " AND T.PMSUBID IN (SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME IN ('" & cmbPmSub.Text.ToString & "'))"
        'End If
        'strsql += vbCrLf + "GROUP BY T.TRANDATE,T.TRANNO,T.BATCHNO"
        'strsql += vbCrLf + "ORDER BY T.TRANDATE,T.TRANNO"
        '_cmd = New OleDbCommand(strsql, cn)
        '_cmd.ExecuteNonQuery()

        strsql = "INSERT INTO TEMPTABLEDB..TEMPPMISSUE"
        strsql += vbCrLf + "(TRANDATE,TRANNO,BATCHNO,ITEMNAME,SUBITEMNAME,IPCS,PCS,GRSWT,NETWT,AMOUNT,RESULT)"
        strsql += vbCrLf + " SELECT TRANDATE,TRANNO,BATCHNO,ITEMNAME,SUBITEMNAME,SUM(IPCS) AS IPCS  "
        strsql += vbCrLf + " ,SUM(PCS) AS PCS "
        strsql += vbCrLf + " ,SUM(GRSWT) AS GRSWT "
        strsql += vbCrLf + " ,SUM(NETWT) AS NETWT "
        strsql += vbCrLf + " ,SUM(AMOUNT) AS AMOUNT,RESULT "
        strsql += vbCrLf + " FROM ( "
        strsql += vbCrLf + "SELECT "
        strsql += vbCrLf + "'2050-01-01' TRANDATE,999999 TRANNO,'ZZZZZZZZZZZZ' BATCHNO,'GRAND TOTAL' ITEMNAME,'' SUBITEMNAME,SUM(T.PCS) AS IPCS"
        'strsql += vbCrLf + ",SUM(ISS.PCS)PCS,SUM(ISS.GRSWT)GRSWT,SUM(ISS.NETWT)NETWT,SUM(ISS.AMOUNT)AMOUNT"
        strsql += vbCrLf + " ,(SELECT SUM(PCS) FROM " & cnStockDb & "..ISSUE WHERE BATCHNO = T.BATCHNO AND ISNULL(CANCEL,'')='') PCS "
        strsql += vbCrLf + " ,(SELECT SUM(GRSWT) FROM " & cnStockDb & "..ISSUE WHERE BATCHNO = T.BATCHNO AND ISNULL(CANCEL,'')='' ) GRSWT "
        strsql += vbCrLf + " ,(SELECT SUM(NETWT) FROM " & cnStockDb & "..ISSUE WHERE BATCHNO = T.BATCHNO AND ISNULL(CANCEL,'')='' ) NETWT "
        strsql += vbCrLf + " ,(SELECT SUM(AMOUNT) FROM " & cnStockDb & "..ISSUE WHERE BATCHNO = T.BATCHNO AND ISNULL(CANCEL,'')='' ) AMOUNT "
        strsql += vbCrLf + ",5 AS RESULT"
        strsql += vbCrLf + "FROM " & cnStockDb & "..PMTRAN AS T"
        'strsql += vbCrLf + "INNER JOIN " & cnStockDb & "..ISSUE AS ISS ON ISS.BATCHNO=T.BATCHNO"
        strsql += vbCrLf + "WHERE T.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpToDate.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(T.CANCEL,'')=''"
        If cmbCostCentre.Text <> "ALL" Then
            strsql += vbCrLf + " AND T.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN ('" & cmbCostCentre.Text.ToString & "'))"
        End If
        If cmbPm.Text <> "ALL" Then
            strsql += vbCrLf + " AND T.PMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN ('" & cmbPm.Text.ToString & "'))"
        End If
        If cmbPmSub.Text <> "ALL" Then
            strsql += vbCrLf + " AND T.PMSUBID IN (SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME IN ('" & cmbPmSub.Text.ToString & "'))"
        End If
        strsql += vbCrLf + "  GROUP BY T.BATCHNO )X GROUP BY TRANDATE,TRANNO,BATCHNO,ITEMNAME,SUBITEMNAME,RESULT "
        _cmd = New OleDbCommand(strsql, cn)
        _cmd.ExecuteNonQuery()

        strsql = "SELECT * FROM TEMPTABLEDB..TEMPPMISSUE ORDER BY TRANDATE,TRANNO,RESULT,ITEMNAME,SUBITEMNAME"
        _cmd = New OleDbCommand(strsql, cn)
        _cmd.ExecuteNonQuery()


        Dim dt As New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        Dim DS As New DataSet

        da.Fill(DS)
        If DS.Tables(0).Rows.Count > 0 Then
            dgvview.DataSource = Nothing
            dgvview.DataSource = DS.Tables(0)
            dgvview.Columns("RESULT").Visible = False
            dgvview.Columns("TRANDATE").Visible = False
            dgvview.Columns("TRANNO").Visible = False
            dgvview.Columns("BATCHNO").Visible = False

            If dgvview.Columns.Contains("BILLDATE") Then dgvview.Columns("BILLDATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            If dgvview.Columns.Contains("BILLNO") Then dgvview.Columns("BILLNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            If dgvview.Columns.Contains("IPCS") Then dgvview.Columns("IPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            If dgvview.Columns.Contains("PCS") Then dgvview.Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            If dgvview.Columns.Contains("GRSWT") Then dgvview.Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            If dgvview.Columns.Contains("NETWT") Then dgvview.Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            If dgvview.Columns.Contains("AMOUNT") Then dgvview.Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dgvview.Columns("ITEMNAME").HeaderText = "ITEM"
            dgvview.Columns("SUBITEMNAME").HeaderText = "SUBITEM"
            dgvview.Columns("IPCS").HeaderText = "COMP_PCS"

            Dim strTranno As String = ""
            For Each dgv As DataGridViewRow In dgvview.Rows
                With dgv
                    Select Case .Cells("RESULT").Value.ToString
                        'Case "1"
                        '    .Cells("ITEMNAME").Style.Font = reportHeadStyle.Font
                        '    .Cells("ITEMNAME").Style.BackColor = reportHeadStyle.BackColor
                        '    .Cells("ITEMNAME").Style.ForeColor = reportHeadStyle.ForeColor
                        Case "0"
                            '.DefaultCellStyle.Font = reportHeadStyle.Font
                            '.DefaultCellStyle.ForeColor = reportHeadStyle.ForeColor
                            '.DefaultCellStyle.BackColor = reportHeadStyle.BackColor

                            .Cells("ITEMNAME").Style.Font = reportHeadStyle.Font
                            .Cells("ITEMNAME").Style.BackColor = System.Drawing.ColorTranslator.FromHtml("#f1e3dd")
                            .Cells("ITEMNAME").Style.ForeColor = Color.DarkViolet

                            .Cells("SUBITEMNAME").Style.Font = reportHeadStyle.Font
                            .Cells("SUBITEMNAME").Style.BackColor = System.Drawing.ColorTranslator.FromHtml("#f1e3dd")
                            .Cells("SUBITEMNAME").Style.ForeColor = Color.DarkViolet

                        Case "5"
                            .DefaultCellStyle.Font = reportTotalStyle.Font
                            .DefaultCellStyle.ForeColor = Color.LavenderBlush
                            .DefaultCellStyle.BackColor = Color.DarkViolet
                        Case "4"
                            .DefaultCellStyle.BackColor = reportTotalStyle.BackColor
                            .DefaultCellStyle.Font = reportTotalStyle.Font
                            .DefaultCellStyle.ForeColor = Color.Red
                            .Cells("SUBITEMNAME").Value = ""
                            .Cells("TRANDATE").Value = ""
                    End Select
                End With
            Next
            For k As Integer = 0 To dgvview.Columns.Count - 1
                dgvview.Columns(k).SortMode = DataGridViewColumnSortMode.NotSortable
            Next
            gridViewHead.Visible = False
            'GridViewHeaderStyle()
        Else
            dgvview.DataSource = Nothing
            MsgBox("Record not found...")
        End If
        Dim strTitle As String = ""
        strTitle = "COMPLEMENTS REPORT FROM " & dtpFrom.Value.ToString("dd-MM-yyyy") & " TO " & dtpToDate.Value.ToString("dd-MM-yyyy")
        If cmbPm.Text <> "" And cmbPm.Text <> "ALL" Then
            strTitle += " ITEM - " & cmbPm.Text.ToString.Trim
        End If
        If cmbPmSub.Text <> "" And cmbPmSub.Text <> "ALL" Then
            strTitle += " SUBITEM - " & cmbPmSub.Text.ToString.Trim
        End If
        If txtTranno.Text <> "" And txtTranno.Text <> "ALL" Then
            strTitle += " TRANNO - " & txtTranno.Text.ToString.Trim
        End If
        lblTitle.Text = strTitle
    End Function


    Function funcview() As Integer

        strsql = vbCrLf + "EXEC " & cnStockDb & "..SP_PMTRAN "
        strsql += vbCrLf + "@FROMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strsql += vbCrLf + ",@TODATE = '" & dtpToDate.Value.ToString("yyyy-MM-dd") & "'"
        strsql += vbCrLf + ",@TRANNO = '" & txtTranno.Text & "'"
        strsql += vbCrLf + ",@COSTCENTRE = '" & cmbCostCentre.Text & "'"
        strsql += vbCrLf + ",@PMNAME = '" & cmbPm.Text & "'"
        strsql += vbCrLf + ",@PMSUBNAME = '" & cmbPmSub.Text & "'"
        strsql += vbCrLf + ",@GROUPBY = ''"
        Dim dt As New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        Dim DS As New DataSet

        da.Fill(DS)
        If DS.Tables(0).Rows.Count > 0 Then
            dgvview.DataSource = Nothing
            dgvview.DataSource = DS.Tables(0)
            dgvview.Columns("RESULT").Visible = False
            If dgvview.Columns.Contains("COSTID") Then dgvview.Columns("COSTID").Visible = False
            If dgvview.Columns.Contains("AMOUNT") Then dgvview.Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            If dgvview.Columns.Contains("ORGITEMNAME") Then
                dgvview.Columns("ORGITEMNAME").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                dgvview.Columns("ORGITEMNAME").HeaderText = "ITEM"
            End If
            If dgvview.Columns.Contains("ORGSUBITEMNAME") Then
                dgvview.Columns("ORGSUBITEMNAME").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                dgvview.Columns("ORGSUBITEMNAME").HeaderText = "SUBITEM"
            End If
            dgvview.Columns("ITEMNAME").HeaderText = "ITEM"
            dgvview.Columns("SUBITEMNAME").HeaderText = "SUBITEM"
            If dgvview.Columns.Contains("AMOUNT") Then
                dgvview.Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End If
            dgvview.Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dgvview.Columns("ORGPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dgvview.Columns("ORGPCS").HeaderText = "PCS"
            dgvview.Columns("TRANDATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            Dim strTranno As String = ""
            For Each dgv As DataGridViewRow In dgvview.Rows
                With dgv
                    Select Case .Cells("RESULT").Value.ToString
                        Case "1"
                            .Cells("ITEMNAME").Style.Font = reportHeadStyle.Font
                            .Cells("ITEMNAME").Style.BackColor = reportHeadStyle.BackColor
                            .Cells("ITEMNAME").Style.ForeColor = reportHeadStyle.ForeColor
                        Case "3"
                            .DefaultCellStyle.Font = reportSubTotalStyle.Font
                            .DefaultCellStyle.ForeColor = reportSubTotalStyle.ForeColor
                        Case "4"
                            .DefaultCellStyle.BackColor = reportTotalStyle.BackColor
                            .DefaultCellStyle.Font = reportTotalStyle.Font
                            .DefaultCellStyle.ForeColor = Color.Red
                            .Cells("SUBITEMNAME").Value = ""
                            .Cells("TRANDATE").Value = ""
                    End Select
                    If .Cells("ITEMNAME").Value.ToString <> .Cells("ORGITEMNAME").Value.ToString And .Cells("RESULT").Value.ToString = "2" Then
                        .DefaultCellStyle.BackColor = Color.MistyRose
                    End If
                    If .Cells("PCS").Value.ToString <> .Cells("ORGPCS").Value.ToString And .Cells("RESULT").Value.ToString = "2" Then
                        .DefaultCellStyle.BackColor = Color.MistyRose
                    End If
                    If strTranno = dgv.Cells("TRANNO").Value.ToString And .Cells("RESULT").Value.ToString = "2" Then
                        .DefaultCellStyle.BackColor = Color.LightGreen
                    End If
                    strTranno = dgv.Cells("TRANNO").Value.ToString
                End With
            Next
            For k As Integer = 0 To dgvview.Columns.Count - 1
                dgvview.Columns(k).SortMode = DataGridViewColumnSortMode.NotSortable
            Next
            GridViewHeaderStyle()
        Else
            dgvview.DataSource = Nothing
            MsgBox("Record not found...")
        End If
        Dim strTitle As String = ""
        strTitle = "COMPLEMENTS REPORT FROM " & dtpFrom.Value.ToString("dd-MM-yyyy") & " TO " & dtpToDate.Value.ToString("dd-MM-yyyy")
        If cmbPm.Text <> "" And cmbPm.Text <> "ALL" Then
            strTitle += " ITEM - " & cmbPm.Text.ToString.Trim
        End If
        If cmbPmSub.Text <> "" And cmbPmSub.Text <> "ALL" Then
            strTitle += " SUBITEM - " & cmbPmSub.Text.ToString.Trim
        End If
        If txtTranno.Text <> "" And txtTranno.Text <> "ALL" Then
            strTitle += " TRANNO - " & txtTranno.Text.ToString.Trim
        End If
        lblTitle.Text = strTitle
    End Function

    Private Function GridViewHeaderStyle()
        Dim dtMergeHeader As New DataTable("~MERGEHEADER")
        With dtMergeHeader
            .Columns.Add("ITEMNAME~SUBITEMNAME~PCS", GetType(String))
            .Columns.Add("ORGITEMNAME~ORGSUBITEMNAME~ORGPCS", GetType(String))
            .Columns.Add("TRANNO~TRANDATE~AMOUNT", GetType(String))
            .Columns.Add("SCROLL", GetType(String))
            .Columns("ITEMNAME~SUBITEMNAME~PCS").Caption = "ISSUE"
            .Columns("ORGITEMNAME~ORGSUBITEMNAME~ORGPCS").Caption = "ACTUAL"
            .Columns("TRANNO~TRANDATE~AMOUNT").Caption = ""
            .Columns("SCROLL").Caption = ""
        End With
        With gridViewHead
            .DataSource = dtMergeHeader
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            funcColWidth()
            dgvview.Focus()
            Dim colWid As Integer = 0
            For cnt As Integer = 0 To dgvview.ColumnCount - 1
                If dgvview.Columns(cnt).Visible Then colWid += dgvview.Columns(cnt).Width
            Next
            If colWid >= dgvview.Width Then
                .Columns("SCROLL").Visible = CType(dgvview.Controls(0), HScrollBar).Visible
                .Columns("SCROLL").Width = CType(dgvview.Controls(1), VScrollBar).Width
            Else
                .Columns("SCROLL").Visible = False
            End If
        End With
    End Function

    Function funcColWidth() As Integer
        With gridViewHead
            Dim strapproval As String = ""

            .Columns("ITEMNAME~SUBITEMNAME~PCS").Width = dgvview.Columns("ITEMNAME").Width + dgvview.Columns("SUBITEMNAME").Width + dgvview.Columns("PCS").Width
            .Columns("ITEMNAME~SUBITEMNAME~PCS").HeaderText = "ISSUE"

            .Columns("ORGITEMNAME~ORGSUBITEMNAME~ORGPCS").Width = dgvview.Columns("ORGITEMNAME").Width + dgvview.Columns("ORGSUBITEMNAME").Width + dgvview.Columns("ORGPCS").Width
            .Columns("ORGITEMNAME~ORGSUBITEMNAME~ORGPCS").HeaderText = "ACTUAL"

            .Columns("TRANNO~TRANDATE~AMOUNT").Width = dgvview.Columns("TRANNO").Width + dgvview.Columns("TRANDATE").Width _
             + dgvview.Columns("AMOUNT").Width

            .Columns("TRANNO~TRANDATE~AMOUNT").HeaderText = ""
            .Columns("SCROLL").Visible = CType(dgvview.Controls(0), HScrollBar).Visible
            .Columns("SCROLL").Width = CType(dgvview.Controls(1), VScrollBar).Width
        End With
    End Function

#End Region

    
    
End Class