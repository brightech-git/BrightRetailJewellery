Imports System.Data.OleDb
Public Class frmOGRpt

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
        Try
            funcview()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

#End Region

#Region "Functions"
    Function FuncNew() As Integer
        strsql = " SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY ORDER BY CATNAME"
        objGPack.FillCombo(strsql, cmbCategory, , False)

        strsql = " SELECT 'ALL' COSTNAME"
        strsql += vbCrLf + " UNION ALL"
        strsql += vbCrLf + " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
        objGPack.FillCombo(strsql, cmbCostCentre, , False)
        cmbCostCentre.Text = "ALL"
        dgvview.DataSource = Nothing
    End Function
    Function funcview() As Integer
        Dim strCatCode As String = ""
        If cmbCategory.Text = "" Then MsgBox("Category is Empty") : FuncNew() : Exit Function
        strsql = " SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY"
        strsql += vbCrLf + "  WHERE CATNAME = '" & cmbCategory.Text.ToString() & "'"
        strCatCode = objGPack.GetSqlValue(strsql, "CATCODE", "")
        If strCatCode = "" Then MsgBox("Invalid Category") : FuncNew() : Exit Function
        Dim strCostID As String = ""
        strsql = " SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE"
        strsql += vbCrLf + "  WHERE COSTNAME = '" & cmbCostCentre.Text.ToString() & "'"
        strCostID = objGPack.GetSqlValue(strsql, "COSTID", "")
        Dim SysId As String = systemId

        strsql = vbCrLf + " EXEC " & cnAdminDb & "..SP_RPT_OGREPORT"
        strsql += vbCrLf + " @FROMDATE ='" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "',"
        strsql += vbCrLf + " @TODATE ='" & dtpToDate.Value.Date.ToString("yyyy-MM-dd") & "',"
        strsql += vbCrLf + " @DBNAME ='" & cnStockDb & "',"
        strsql += vbCrLf + " @CATID ='" & strCatCode & "',"
        strsql += vbCrLf + " @SYSID ='" & SysId & "',"
        strsql += vbCrLf + " @COSTID ='" & strCostID & "'"
        _cmd = New OleDbCommand(strsql, cn)
        _cmd.ExecuteNonQuery()

        strsql = vbCrLf + "   SELECT PARTICULAR,GRSWT,NETWT,SEP,RESULT FROM TEMPTABLEDB..TEMP" & SysId & "MELTING "
        strsql += vbCrLf + "  ORDER BY RESULT "
        Dim dt As New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        Dim DS As New DataSet
        da.Fill(DS)
        If DS.Tables(0).Rows.Count > 0 Then
            dgvview.DataSource = Nothing
            dgvview.DataSource = DS.Tables(0)
            dgvview.Columns("RESULT").Visible = False
            dgvview.Columns("SEP").Visible = False
            dgvview.Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dgvview.Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dgvview.Columns("PARTICULAR").Width = 300
            dgvview.Columns("GRSWT").Width = 200
            dgvview.Columns("NETWT").Width = 200

            For Each dgv As DataGridViewRow In dgvview.Rows
                With dgv
                    Select Case .Cells("SEP").Value.ToString
                        Case "TO"
                            .Cells("PARTICULAR").Style.BackColor = reportHeadStyle.BackColor
                            .DefaultCellStyle.ForeColor = reportHeadStyle.ForeColor
                            .DefaultCellStyle.Font = reportHeadStyle.Font
                        Case "S1"
                            .Cells("PARTICULAR").Style.BackColor = reportHeadStyle1.BackColor
                            .DefaultCellStyle.ForeColor = reportHeadStyle1.ForeColor
                            .DefaultCellStyle.Font = reportHeadStyle1.Font
                        Case "S2"
                            .Cells("PARTICULAR").Style.BackColor = reportHeadStyle1.BackColor
                            .DefaultCellStyle.ForeColor = reportHeadStyle1.ForeColor
                            .DefaultCellStyle.Font = reportHeadStyle1.Font
                        Case "S3"
                            .Cells("PARTICULAR").Style.BackColor = reportHeadStyle1.BackColor
                            .DefaultCellStyle.ForeColor = reportHeadStyle1.ForeColor
                            .DefaultCellStyle.Font = reportHeadStyle1.Font
                        Case "S4"
                            .Cells("PARTICULAR").Style.BackColor = reportHeadStyle.BackColor
                            .DefaultCellStyle.ForeColor = reportHeadStyle.ForeColor
                            .DefaultCellStyle.Font = reportHeadStyle.Font
                    End Select
                End With
            Next
            For k As Integer = 0 To dgvview.Columns.Count - 1
                dgvview.Columns(k).SortMode = DataGridViewColumnSortMode.NotSortable
            Next
        Else
            MsgBox("Record not found...")
            dgvview.DataSource = Nothing
        End If

    End Function
    Function funcview_OLD() As Integer
        Dim strCatCode As String = ""
        If cmbCategory.Text = "" Then MsgBox("Category is Empty") : FuncNew() : Exit Function
        strsql = " SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY"
        strsql += vbCrLf + "  WHERE CATNAME = '" & cmbCategory.Text.ToString() & "'"
        strCatCode = objGPack.GetSqlValue(strsql, "CATCODE", "")
        If strCatCode = "" Then MsgBox("Invalid Category") : FuncNew() : Exit Function
        Dim strCostID As String = ""
        strsql = " SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE"
        strsql += vbCrLf + "  WHERE COSTNAME = '" & cmbCostCentre.Text.ToString() & "'"
        strCostID = objGPack.GetSqlValue(strsql, "COSTID", "")


        strsql = vbCrLf + "   IF OBJECT_ID('TEMPTABLEDB..TEMPMELTING') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPMELTING "
        strsql += vbCrLf + "   SELECT SEP AS PARTICULAR,SUM(RGRSWT)RGRSWT,SUM(RNETWT)RNETWT,SUM(IGRSWT)IGRSWT,SUM(INETWT)INETWT,RESULT "
        strsql += vbCrLf + "   INTO TEMPTABLEDB..TEMPMELTING"
        strsql += vbCrLf + "   FROM ("
        strsql += vbCrLf + "   SELECT "
        strsql += vbCrLf + "   'TOTAL OLD' AS SEP,TRANNO,TRANDATE,SUM(GRSWT)RGRSWT,SUM(NETWT)RNETWT,0 AS IGRSWT,0 AS INETWT,0 AS RESULT"
        strsql += vbCrLf + "   FROM " & cnStockDb & "..RECEIPT AS R"
        strsql += vbCrLf + "   WHERE ISNULL(R.CANCEL,'')=''"
        strsql += vbCrLf + "   AND R.TRANDATE BETWEEN '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpToDate.Value.Date.ToString("yyyy-MM-dd") & "' "
        strsql += vbCrLf + "   AND R.TRANTYPE='PU' "
        If strCostID <> "" Then strsql += vbCrLf + "   AND R.COSTID='" & strCostID & "' "
        strsql += vbCrLf + "   AND R.CATCODE='" & strCatCode & "' /*AND R.COMPANYID='" & strCompanyId & "'*/ AND REMARK1 <> 'TRANSFERED ENTRY'"
        strsql += vbCrLf + "   GROUP BY TRANNO,TRANDATE"
        strsql += vbCrLf + "   UNION ALL"
        strsql += vbCrLf + "   SELECT "
        strsql += vbCrLf + "   'OLD TO NEW' AS SEP,TRANNO,TRANDATE,0 AS RGRSWT,0 AS RNETWT,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,1 AS RESULT"
        strsql += vbCrLf + "   FROM " & cnStockDb & "..RECEIPT AS R"
        strsql += vbCrLf + "   WHERE ISNULL(R.CANCEL,'')=''"
        strsql += vbCrLf + "   AND R.TRANDATE BETWEEN '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpToDate.Value.Date.ToString("yyyy-MM-dd") & "' "
        strsql += vbCrLf + "   AND R.TRANTYPE='PU' AND ISNULL(MELT_RETAG,'')='L'"
        strsql += vbCrLf + "   AND R.CATCODE='" & strCatCode & "' /*AND R.COMPANYID='" & strCompanyId & "'*/"
        If strCostID <> "" Then strsql += vbCrLf + "   AND R.COSTID='" & strCostID & "' "
        strsql += vbCrLf + "   GROUP BY TRANNO,TRANDATE"
        strsql += vbCrLf + "   UNION ALL"
        strsql += vbCrLf + "   SELECT "
        strsql += vbCrLf + "   'MELTING ISSUE' AS SEP,TRANNO,TRANDATE,0 AS RGRSWT,0 AS RNETWT,SUM(GRSWT)IGRSWT,SUM(NETWT)INETWT,2 AS RESULT"
        strsql += vbCrLf + "   FROM " & cnStockDb & "..RECEIPT AS R"
        strsql += vbCrLf + "   WHERE ISNULL(R.CANCEL,'')=''"
        strsql += vbCrLf + "   AND R.TRANDATE BETWEEN '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpToDate.Value.Date.ToString("yyyy-MM-dd") & "' "
        strsql += vbCrLf + "   AND R.TRANTYPE='PU' AND ISNULL(MELT_RETAG,'')='M'"
        strsql += vbCrLf + "   AND R.CATCODE='" & strCatCode & "' "
        strsql += vbCrLf + "   /*AND R.COMPANYID='" & strCompanyId & "'*/"
        If strCostID <> "" Then strsql += vbCrLf + "   AND R.COSTID='" & strCostID & "' "
        strsql += vbCrLf + "   GROUP BY TRANNO,TRANDATE"
        strsql += vbCrLf + "   UNION ALL"
        strsql += vbCrLf + "   SELECT "
        strsql += vbCrLf + "   'STONE LOSS' AS SEP,TRANNO,TRANDATE,0 AS RGRSWT,0 AS RNETWT,SUM(GRSWT-NETWT)IGRSWT,SUM(GRSWT-NETWT)INETWT,3 AS RESULT"
        strsql += vbCrLf + "   FROM " & cnStockDb & "..MELTINGDETAIL AS R"
        strsql += vbCrLf + "   WHERE ISNULL(R.CANCEL,'')=''"
        strsql += vbCrLf + "   AND R.CATCODE='" & strCatCode & "' AND R.RECISS='I' "
        strsql += vbCrLf + "   AND R.BAGNO IN (SELECT DISTINCT BAGNO FROM " & cnStockDb & "..RECEIPT WHERE ISNULL(CANCEL,'')='' AND R.TRANDATE BETWEEN '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpToDate.Value.Date.ToString("yyyy-MM-dd") & "')"
        If strCostID <> "" Then strsql += vbCrLf + "   AND R.COSTID='" & strCostID & "' "
        strsql += vbCrLf + "   GROUP BY TRANNO,TRANDATE"
        strsql += vbCrLf + "   UNION ALL"
        strsql += vbCrLf + "   SELECT "
        strsql += vbCrLf + "   'MELTING RECEIPT' AS SEP,TRANNO,TRANDATE,SUM(GRSWT) AS RGRSWT,SUM(NETWT) AS RNETWT,0 AS IGRSWT,0 AS INETWT,4 AS RESULT"
        strsql += vbCrLf + "   FROM " & cnStockDb & "..RECEIPT AS R"
        strsql += vbCrLf + "   WHERE ISNULL(R.CANCEL,'')=''"
        strsql += vbCrLf + "   AND R.TRANDATE BETWEEN '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpToDate.Value.Date.ToString("yyyy-MM-dd") & "' "
        strsql += vbCrLf + "   AND R.TRANTYPE='RRE' "
        strsql += vbCrLf + "   AND R.CATCODE='" & strCatCode & "' "
        strsql += vbCrLf + "   /*AND R.COMPANYID='" & strCompanyId & "'*/"
        strsql += vbCrLf + "   AND R.BAGNO IN (SELECT DISTINCT BAGNO FROM " & cnStockDb & "..MELTINGDETAIL WHERE ISNULL(CANCEL,'')='')"
        If strCostID <> "" Then strsql += vbCrLf + "   AND R.COSTID='" & strCostID & "' "
        strsql += vbCrLf + "   GROUP BY TRANNO,TRANDATE"
        strsql += vbCrLf + "   UNION ALL"
        strsql += vbCrLf + "   SELECT "
        strsql += vbCrLf + "   'MELTING LOSS' AS SEP,TRANNO,TRANDATE,0 AS RGRSWT,0 AS RNETWT,SUM(WASTAGE)IGRSWT,SUM(WASTAGE) IGRSWT,5 AS RESULT"
        strsql += vbCrLf + "   FROM " & cnStockDb & "..RECEIPT AS R"
        strsql += vbCrLf + "   WHERE ISNULL(R.CANCEL,'')=''"
        strsql += vbCrLf + "   AND R.TRANDATE BETWEEN '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpToDate.Value.Date.ToString("yyyy-MM-dd") & "' "
        strsql += vbCrLf + "   AND R.TRANTYPE='RRE' "
        strsql += vbCrLf + "   AND R.CATCODE='" & strCatCode & "' "
        strsql += vbCrLf + "   /*AND R.COMPANYID='" & strCompanyId & "'*/"
        strsql += vbCrLf + "   AND R.BAGNO IN (SELECT DISTINCT BAGNO FROM " & cnStockDb & "..MELTINGDETAIL WHERE ISNULL(CANCEL,'')='' AND RECISS='R')"
        If strCostID <> "" Then strsql += vbCrLf + "   AND R.COSTID='" & strCostID & "' "
        strsql += vbCrLf + "   GROUP BY TRANNO,TRANDATE"
        strsql += vbCrLf + "   UNION ALL"
        strsql += vbCrLf + "   SELECT "
        strsql += vbCrLf + "   'SALES' AS SEP,TRANNO,TRANDATE,0 AS RGRSWT,0 AS RNETWT,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,6 AS RESULT"
        strsql += vbCrLf + "   FROM " & cnStockDb & "..ISSUE AS R"
        strsql += vbCrLf + "   WHERE ISNULL(R.CANCEL,'')=''"
        strsql += vbCrLf + "   AND R.TRANDATE BETWEEN '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpToDate.Value.Date.ToString("yyyy-MM-dd") & "' "
        strsql += vbCrLf + "   AND R.TRANTYPE='SA' "
        strsql += vbCrLf + "   AND R.CATCODE='" & strCatCode & "' /*AND R.COMPANYID='" & strCompanyId & "'*/"
        If strCostID <> "" Then strsql += vbCrLf + "   AND R.COSTID='" & strCostID & "' "
        strsql += vbCrLf + "   GROUP BY TRANNO,TRANDATE"
        strsql += vbCrLf + "   )X GROUP BY SEP,RESULT "
        _cmd = New OleDbCommand(strsql, cn)
        _cmd.ExecuteNonQuery()

        strsql = vbCrLf + "   ALTER TABLE TEMPTABLEDB..TEMPMELTING ADD RUNGRSWT NUMERIC(15,3),RUNNETWT NUMERIC(15,3)"
        _cmd = New OleDbCommand(strsql, cn)
        _cmd.ExecuteNonQuery()


        strsql = vbCrLf + "  DECLARE @RUNGRSWT NUMERIC(15,3)"
        strsql += vbCrLf + "  DECLARE @RUNNETWT NUMERIC(15,3)"
        strsql += vbCrLf + "  DECLARE @RGRSWT NUMERIC(15,3)"
        strsql += vbCrLf + "  DECLARE @RNETWT NUMERIC(15,3)"
        strsql += vbCrLf + "  DECLARE @IGRSWT NUMERIC(15,3)"
        strsql += vbCrLf + "  DECLARE @INETWT NUMERIC(15,3)"
        strsql += vbCrLf + "  DECLARE @RESULT INTEGER"
        strsql += vbCrLf + "  DECLARE CUR CURSOR FOR SELECT RGRSWT,RNETWT,IGRSWT,INETWT,RESULT FROM TEMPTABLEDB..TEMPMELTING ORDER BY RESULT"
        strsql += vbCrLf + "  OPEN CUR"
        strsql += vbCrLf + "  SET @RUNGRSWT = 0"
        strsql += vbCrLf + "  SET @RUNNETWT = 0"
        strsql += vbCrLf + "  FETCH NEXT FROM CUR INTO @RGRSWT,@RNETWT,@IGRSWT,@INETWT,@RESULT"
        strsql += vbCrLf + "  WHILE @@FETCH_STATUS = 0"
        strsql += vbCrLf + "  BEGIN"
        strsql += vbCrLf + "  PRINT @RESULT"
        strsql += vbCrLf + "  PRINT @RGRSWT"
        strsql += vbCrLf + "  PRINT @IGRSWT"
        strsql += vbCrLf + "  SET @RUNGRSWT = @RUNGRSWT + @RGRSWT -@IGRSWT"
        strsql += vbCrLf + "  PRINT @RUNGRSWT"
        strsql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMPMELTING SET RUNGRSWT=@RUNGRSWT WHERE RESULT=@RESULT "
        strsql += vbCrLf + "  PRINT @RNETWT"
        strsql += vbCrLf + "  PRINT @INETWT"
        strsql += vbCrLf + "  SET @RUNNETWT = @RUNNETWT + @RNETWT -@INETWT"
        strsql += vbCrLf + "  PRINT @RUNNETWT"
        strsql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMPMELTING SET RUNNETWT=@RUNNETWT WHERE RESULT=@RESULT "
        strsql += vbCrLf + "  FETCH NEXT FROM CUR INTO @RGRSWT,@RNETWT,@IGRSWT,@INETWT,@RESULT"
        strsql += vbCrLf + "  END"
        strsql += vbCrLf + "  CLOSE CUR"
        strsql += vbCrLf + "  DEALLOCATE CUR"
        _cmd = New OleDbCommand(strsql, cn)
        _cmd.ExecuteNonQuery()


        'strsql = vbCrLf + "   SELECT PARTICULAR,SUM(RGRSWT)RGRSWT,SUM(IGRSWT)IGRSWT,RESULT  "
        'strsql += vbCrLf + "   FROM TEMPTABLEDB..TEMPMELTING AS T GROUP BY PARTICULAR,RESULT"
        'strsql += vbCrLf + "   UNION ALL"
        'strsql += vbCrLf + "   SELECT 'TOTAL' PARTICULAR,"
        'strsql += vbCrLf + "  SUM(CASE WHEN RESULT IN (4,5) THEN -1*RGRSWT ELSE RGRSWT END)RGRSWT,"
        'strsql += vbCrLf + "  SUM(CASE WHEN RESULT IN (4,5) THEN -1*IGRSWT ELSE IGRSWT END)IGRSWT,7 RESULT  "
        'strsql += vbCrLf + "   FROM TEMPTABLEDB..TEMPMELTING AS T "
        'strsql += vbCrLf + "   WHERE RESULT NOT IN (3)"
        'strsql += vbCrLf + "   /*WHERE RESULT IN (0,1,3,4,5,6)*/"

        strsql = vbCrLf + "   SELECT PARTICULAR,RGRSWT,IGRSWT,RUNGRSWT,RESULT FROM TEMPTABLEDB..TEMPMELTING "
        strsql += vbCrLf + "  ORDER BY RESULT "



        Dim dt As New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        Dim DS As New DataSet
        da.Fill(DS)
        If DS.Tables(0).Rows.Count > 0 Then
            dgvview.DataSource = Nothing
            dgvview.DataSource = DS.Tables(0)
            dgvview.Columns("RESULT").Visible = False
            dgvview.Columns("RGRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dgvview.Columns("IGRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dgvview.Columns("RUNGRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            dgvview.Columns("RGRSWT").HeaderText = "RECEIPT"
            dgvview.Columns("IGRSWT").HeaderText = "ISSUE"
            dgvview.Columns("RUNGRSWT").HeaderText = "BALANCE"
            dgvview.Columns("PARTICULAR").Width = 300
            dgvview.Columns("RGRSWT").Width = 200
            dgvview.Columns("IGRSWT").Width = 200
            dgvview.Columns("RUNGRSWT").Width = 200

            For Each dgv As DataGridViewRow In dgvview.Rows
                With dgv
                    Select Case .Cells("RESULT").Value.ToString
                        Case "0"
                            .DefaultCellStyle.BackColor = Color.LightGray
                            .DefaultCellStyle.Font = New Font("VERDHANA", 8, FontStyle.Bold)
                        Case "4"
                            .DefaultCellStyle.BackColor = Color.MistyRose
                        Case "5"
                            .DefaultCellStyle.BackColor = Color.MistyRose
                    End Select
                End With
            Next
            For k As Integer = 0 To dgvview.Columns.Count - 1
                dgvview.Columns(k).SortMode = DataGridViewColumnSortMode.NotSortable
            Next
        Else
            MsgBox("Record not found...")
            dgvview.DataSource = Nothing
        End If

    End Function

#End Region

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        BrightPosting.GExport.Post(Me.Name, strCompanyName, "Old Gold Report", dgvview, BrightPosting.GExport.GExportType.Export)
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        BrightPosting.GExport.Post(Me.Name, strCompanyName, "Old Gold Report", dgvview, BrightPosting.GExport.GExportType.Print)
    End Sub

End Class