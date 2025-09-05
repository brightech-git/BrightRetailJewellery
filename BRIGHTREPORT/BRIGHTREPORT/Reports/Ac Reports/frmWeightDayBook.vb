Imports System.Data.OleDb
Public Class frmWeightDayBook
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Dim objGridShower As frmGridDispDia
    Dim dtCompany As New DataTable
    Dim dtCostCentre As New DataTable

    Private Sub DayBookfrm_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub DayBookfrm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        grpContainer.Location = New Point((ScreenWid - grpContainer.Width) / 2, ((ScreenHit - 128) - grpContainer.Height) / 2)


        strSql = " SELECT 'ALL' COMPANYNAME,'ALL' COMPANYID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COMPANYNAME,COMPANYID,2 RESULT FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N'"
        strSql += " ORDER BY RESULT,COMPANYNAME"
        dtCompany = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCompany)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dtCompany, "COMPANYNAME", , strCompanyName)

        If GetAdmindbSoftValue("COSTCENTRE") = "Y" Then
            strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
            strSql += " UNION ALL"
            strSql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
            strSql += " ORDER BY RESULT,COSTNAME"
            dtCostCentre = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtCostCentre)
            BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        Else
            chkCmbCostCentre.Enabled = False
            chkCmbCostCentre.Items.Clear()
        End If

        btnNew_Click(Me, New EventArgs)
    End Sub
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dtCompany, "COMPANYNAME", , "ALL")
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False
        Prop_Gets()
        chkCmbCompany.Select()
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click


        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("KEYNO", GetType(Integer))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        'If rbtAcName.Checked Then
        '    strSql = " SELECT * FROM MASTER..TEMPDAYBOOK ORDER BY TRANDATE,TDATE DESC,RESULT,PARTICULAR"
        'Else
        '    strSql = " SELECT * FROM MASTER..TEMPDAYBOOK ORDER BY TRANDATE,TDATE DESC,RESULT,TRANNO,PARTICULAR"
        'End If

        Dim CompanyFilt As String = funcCompanyFilt()

        funcView(CompanyFilt)

        'strSql = "  SELECT "
        'strSql += "  	 DISTINCT CONVERT(VARCHAR,TRANDATE,103) PARTICULARS,NULL RGRSWT,NULL RTOUCH,NULL RNETWT,NULL RAMOUNT,NULL IGRSWT"
        'strSql += "  	,NULL ITOUCH,NULL INETWT,NULL IAMOUNT,NULL HEAD,'-1' ORDBY,'-1' COLSEP,'-1' TOTSEP,NULL CATNAME,NULL ACNAME,TRANDATE,'T' COLHEAD,NULL PAGEBREAK"
        'strSql += "  FROM TEMPDAYBOOKSUMMARY"
        'strSql += "  UNION ALL"
        'strSql += "  SELECT "
        'strSql += "  	 PARTICULARS,RGRSWT,RTOUCH,RNETWT,RAMOUNT,IGRSWT,ITOUCH,INETWT,IAMOUNT"
        'strSql += "  	,HEAD,ORDBY,COLSEP,TOTSEP,CATNAME,ACNAME,TRANDATE,COLHEAD,NULL PAGEBREAK "
        'strSql += "  FROM TEMPDAYBOOKSUMMARY"
        'strSql += "  UNION ALL"
        'strSql += "  SELECT "
        'strSql += "  	 DISTINCT '' PARTICULARS,NULL RGRSWT,NULL RTOUCH,NULL RNETWT,NULL RAMOUNT,NULL IGRSWT"
        'strSql += "  	,NULL ITOUCH,NULL INETWT,NULL IAMOUNT,NULL HEAD,'9' ORDBY,'9' COLSEP,'9' TOTSEP,NULL CATNAME,NULL ACNAME,TRANDATE,'' COLHEAD,'CHR(12)' PAGEBREAK"
        'strSql += "  FROM TEMPDAYBOOKSUMMARY"
        'strSql += "  UNION ALL"
        'strSql += "  SELECT"
        'strSql += "  	'GRAND TOTAL' PARTICULARS"
        'strSql += "  	,SUM(RGRSWT) RGRSWT,NULL RTOUCH,SUM(RNETWT) RNETWT,SUM(RAMOUNT) RAMOUNT"
        'strSql += "  	,SUM(IGRSWT) IGRSWT,NULL ITOUCH,SUM(INETWT) INETWT,SUM(IAMOUNT) IAMOUNT,HEAD,ORDBY,'5' COLSEP,'2' TOTSEP,NULL CATNAME,NULL ACNAME"
        'strSql += "  	,TRANDATE,'G1' COLHEAD,NULL PAGEBREAK"
        'strSql += "  FROM TEMPDAYBOOKSUMMARY WHERE COLSEP != '4'"
        'strSql += "  GROUP BY HEAD,ORDBY,TRANDATE"
        'strSql += "  UNION ALL"
        'strSql += "  SELECT"
        'strSql += "  	'BALANCE' PARTICULARS"
        'strSql += "  	,CASE WHEN ISNULL(SUM(RGRSWT),0) > ISNULL(SUM(IGRSWT),0) THEN ISNULL(SUM(RGRSWT),0) - ISNULL(SUM(IGRSWT),0) END RGRSWT,NULL RTOUCH,CASE WHEN ISNULL(SUM(RNETWT),0) > ISNULL(SUM(INETWT),0) THEN ISNULL(SUM(RNETWT),0) - ISNULL(SUM(INETWT),0) END RNETWT,CASE WHEN ISNULL(SUM(RAMOUNT),0) > ISNULL(SUM(IAMOUNT),0) THEN ISNULL(SUM(RAMOUNT),0) - ISNULL(SUM(IAMOUNT),0) END RAMOUNT"
        'strSql += "  	,CASE WHEN ISNULL(SUM(IGRSWT),0) > ISNULL(SUM(RGRSWT),0) THEN ISNULL(SUM(IGRSWT),0) - ISNULL(SUM(RGRSWT),0) END IGRSWT,NULL ITOUCH,CASE WHEN ISNULL(SUM(INETWT),0) > ISNULL(SUM(RNETWT),0) THEN ISNULL(SUM(INETWT),0) - ISNULL(SUM(RNETWT),0) END INETWT,CASE WHEN ISNULL(SUM(IAMOUNT),0) > ISNULL(SUM(RAMOUNT),0) THEN ISNULL(SUM(IAMOUNT),0) - ISNULL(SUM(RAMOUNT),0) END IAMOUNT,HEAD,ORDBY,'6' COLSEP,'3' TOTSEP,NULL CATNAME,NULL ACNAME"
        'strSql += "  	,TRANDATE,'G' COLHEAD,NULL PAGEBREAK"
        'strSql += "  FROM TEMPDAYBOOKSUMMARY WHERE COLSEP != '4'"
        'strSql += "  GROUP BY HEAD,ORDBY,TRANDATE"
        'strSql += "  ORDER BY TRANDATE,ORDBY,HEAD,TOTSEP,CATNAME,COLSEP,ACNAME"

        strSql = " IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "DAYSTOCKOPENING1')"
        strSql += "  	DROP TABLE TEMP" & systemId & "DAYSTOCKOPENING1"
        strSql += "  CREATE TABLE TEMP" & systemId & "DAYSTOCKOPENING1"
        strSql += "  (CONTRANAME VARCHAR(250),OPENING NUMERIC(15,2),RECEIPT NUMERIC(15,2),ISSUE NUMERIC(15,2),CLOSING NUMERIC(15,2),TRANDATE SMALLDATETIME,CATCODE VARCHAR(25))"
        strSql += " "
        strSql += "  INSERT INTO TEMP" & systemId & "DAYSTOCKOPENING1"
        strSql += "  (CATCODE,TRANDATE)"
        strSql += "  SELECT X.CATCODE,TRANDATE FROM"
        strSql += "  ("
        strSql += "  	SELECT DISTINCT CATCODE FROM  " & cnStockDb & "..OPENWEIGHT "
        strSql += "     WHERE STOCKTYPE='C'"
        strSql += CompanyFilt
        strSql += "  	UNION "
        strSql += "  	SELECT DISTINCT CATCODE FROM  " & cnStockDb & "..ISSUE"
        strSql += "     WHERE TRANDATE <=  '" + dtpTo.Value.Date.ToString("yyyy-MM-dd") + "'"
        strSql += CompanyFilt
        strSql += "  	UNION "
        strSql += "  	SELECT DISTINCT CATCODE FROM  " & cnStockDb & "..RECEIPT"
        strSql += "     WHERE TRANDATE <=  '" + dtpTo.Value.Date.ToString("yyyy-MM-dd") + "'"
        strSql += CompanyFilt
        strSql += "  )X,TEMP" & systemId & "DAYOPENING"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "  UPDATE TEMP" & systemId & "DAYSTOCKOPENING1 SET"
        strSql += "  	 CONTRANAME = (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = T1.CATCODE)"
        strSql += "  	,OPENING = (ISNULL((SELECT SUM(CASE WHEN TRANTYPE = 'R' THEN GRSWT ELSE -1 * GRSWT END) FROM  " & cnStockDb & "..OPENWEIGHT WHERE STOCKTYPE = 'C' AND CATCODE = T1.CATCODE " & CompanyFilt & "),0)"
        strSql += "  			+ "
        strSql += "  		   ISNULL((SELECT -1 * SUM(GRSWT) FROM " & cnStockDb & "..ISSUE WHERE CATCODE = T1.CATCODE " & CompanyFilt & " AND TRANDATE < T1.TRANDATE AND ISNULL(CANCEL,'') != 'Y'),0)"
        strSql += " 		    	+ "
        strSql += "  		   ISNULL((SELECT SUM(GRSWT) FROM " & cnStockDb & "..RECEIPT WHERE CATCODE = T1.CATCODE " & CompanyFilt & " AND TRANDATE < T1.TRANDATE AND ISNULL(CANCEL,'') != 'Y'),0))"
        strSql += "     ,RECEIPT = ISNULL((SELECT SUM(GRSWT) FROM " & cnStockDb & "..RECEIPT WHERE CATCODE = T1.CATCODE " & CompanyFilt & " AND TRANDATE = T1.TRANDATE AND ISNULL(CANCEL,'') != 'Y'),0)"
        strSql += "     ,ISSUE = ISNULL((SELECT SUM(GRSWT) FROM " & cnStockDb & "..ISSUE WHERE CATCODE = T1.CATCODE " & CompanyFilt & " AND TRANDATE = T1.TRANDATE AND ISNULL(CANCEL,'') != 'Y'),0)"
        strSql += "  	,CLOSING = (ISNULL((SELECT SUM(CASE WHEN TRANTYPE = 'R' THEN GRSWT ELSE -1 * GRSWT END) FROM  " & cnStockDb & "..OPENWEIGHT WHERE STOCKTYPE = 'C' AND CATCODE = T1.CATCODE " & CompanyFilt & "),0)"
        strSql += "  			+ "
        strSql += "  		   ISNULL((SELECT -1 * SUM(GRSWT) FROM " & cnStockDb & "..ISSUE WHERE CATCODE = T1.CATCODE " & CompanyFilt & " AND TRANDATE <= T1.TRANDATE AND ISNULL(CANCEL,'') != 'Y'),0)"
        strSql += " 		    	+ "
        strSql += "  		   ISNULL((SELECT SUM(GRSWT) FROM " & cnStockDb & "..RECEIPT WHERE CATCODE = T1.CATCODE " & CompanyFilt & " AND TRANDATE <= T1.TRANDATE AND ISNULL(CANCEL,'') != 'Y'),0))"
        strSql += "  FROM TEMP" & systemId & "DAYSTOCKOPENING1 AS T1"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "  SELECT   	 "
        strSql += "  	 DISTINCT CONVERT(VARCHAR,TRANDATE,103) PARTICULARS,NULL RGRSWT,NULL RTOUCH,NULL RNETWT,NULL RAMOUNT,NULL IGRSWT  	"
        strSql += "  	,NULL ITOUCH,NULL INETWT,NULL IAMOUNT,NULL HEAD,'-1' ORDBY,'-1' COLSEP,'-1' TOTSEP,NULL CATNAME,NULL ACNAME,TRANDATE"
        strSql += "  	,'T' COLHEAD,NULL PAGEBREAK  "
        strSql += "  FROM TEMP" & systemId & "DAYBOOKSUMMARY "
        strSql += "  UNION ALL  "
        strSql += "  SELECT"
        strSql += "     	 PARTICULARS,CONVERT(VARCHAR,RGRSWT) RGRSWT,CONVERT(VARCHAR,RTOUCH) RTOUCH,CONVERT(VARCHAR,RNETWT) RNETWT,RAMOUNT,CONVERT(VARCHAR,IGRSWT) IGRSWT,ITOUCH,INETWT,IAMOUNT"
        strSql += "    	,HEAD,ORDBY,COLSEP,TOTSEP,CATNAME,ACNAME,TRANDATE,COLHEAD,NULL PAGEBREAK"
        strSql += "  FROM TEMP" & systemId & "DAYBOOKSUMMARY  "
        strSql += "  UNION ALL  "
        strSql += "  SELECT"
        strSql += "     	 DISTINCT '' PARTICULARS,NULL RGRSWT,NULL RTOUCH,NULL RNETWT,NULL RAMOUNT,NULL IGRSWT  	"
        strSql += "  	,NULL ITOUCH,NULL INETWT,NULL IAMOUNT,NULL HEAD,'9' ORDBY,'9' COLSEP,'9' TOTSEP,NULL CATNAME,NULL ACNAME"
        strSql += "  	,TRANDATE,'' COLHEAD,'CHR(12)' PAGEBREAK  "
        strSql += "  FROM TEMP" & systemId & "DAYBOOKSUMMARY  "
        strSql += "  UNION ALL  "
        strSql += "  SELECT  	"
        strSql += "  	'GRAND TOTAL' PARTICULARS  	,CONVERT(VARCHAR,SUM(RGRSWT)) RGRSWT,NULL RTOUCH,CONVERT(VARCHAR,SUM(RNETWT)) RNETWT,SUM(RAMOUNT) RAMOUNT  	"
        strSql += "  	,CONVERT(VARCHAR,SUM(IGRSWT)) IGRSWT,NULL ITOUCH,SUM(INETWT) INETWT,SUM(IAMOUNT) IAMOUNT"
        strSql += "  	,HEAD,ORDBY,'5' COLSEP,'2' TOTSEP"
        strSql += "  	,NULL CATNAME,NULL ACNAME  	,TRANDATE,'G1' COLHEAD,NULL PAGEBREAK  "
        strSql += "  FROM TEMP" & systemId & "DAYBOOKSUMMARY WHERE COLSEP != '4'  "
        strSql += "  GROUP BY HEAD,ORDBY,TRANDATE  "
        strSql += "  UNION ALL  "
        strSql += "  SELECT  	"
        strSql += "  	'BALANCE' PARTICULARS,CONVERT(VARCHAR,CASE WHEN ISNULL(SUM(RGRSWT),0) > ISNULL(SUM(IGRSWT),0) THEN ISNULL(SUM(RGRSWT),0) - ISNULL(SUM(IGRSWT),0) END) RGRSWT,NULL RTOUCH,CONVERT(VARCHAR,CASE WHEN ISNULL(SUM(RNETWT),0) > ISNULL(SUM(INETWT),0) THEN ISNULL(SUM(RNETWT),0) - ISNULL(SUM(INETWT),0) END) RNETWT"
        strSql += "  	,CASE WHEN ISNULL(SUM(RAMOUNT),0) > ISNULL(SUM(IAMOUNT),0) THEN ISNULL(SUM(RAMOUNT),0) - ISNULL(SUM(IAMOUNT),0) END RAMOUNT  	"
        strSql += "  	,CONVERT(VARCHAR,CASE WHEN ISNULL(SUM(IGRSWT),0) > ISNULL(SUM(RGRSWT),0) THEN ISNULL(SUM(IGRSWT),0) - ISNULL(SUM(RGRSWT),0) END) IGRSWT,NULL ITOUCH"
        strSql += "  	,CASE WHEN ISNULL(SUM(INETWT),0) > ISNULL(SUM(RNETWT),0) THEN ISNULL(SUM(INETWT),0) - ISNULL(SUM(RNETWT),0) END INETWT"
        strSql += "  	,CASE WHEN ISNULL(SUM(IAMOUNT),0) > ISNULL(SUM(RAMOUNT),0) THEN ISNULL(SUM(IAMOUNT),0) - ISNULL(SUM(RAMOUNT),0) END IAMOUNT"
        strSql += "  	,HEAD,ORDBY,'6' COLSEP,'3' TOTSEP,NULL CATNAME,NULL ACNAME,TRANDATE,'G' COLHEAD,NULL PAGEBREAK  "
        strSql += "  FROM TEMP" & systemId & "DAYBOOKSUMMARY "
        strSql += "  WHERE COLSEP != '4'  "
        strSql += "  GROUP BY HEAD,ORDBY,TRANDATE  "
        strSql += "  UNION ALL"
        strSql += "  SELECT "
        strSql += "  	DISTINCT 'STOCK NAME' PARTICULARS,'OPENING' RGRSWT,'RECEIPT' RTOUCH,'ISSUE' RNETWT,NULL RAMOUNT"
        strSql += "  	,'CLOSING' IGRSWT,NULL ITOUCH,NULL INETWT,NULL IAMOUNT"
        strSql += "  	,'STOCK BALANCE' HEAD,3 ORDBY,'7' COLSEP,'4' TOTSEP,NULL CATNAME,NULL ACNAME,TRANDATE,'T' COLHEAD"
        strSql += "  	,NULL PAGEBREAK"
        strSql += "  FROM TEMP" & systemId & "DAYSTOCKOPENING1"
        strSql += "  UNION ALL"
        strSql += "  SELECT"
        strSql += "  	 CONTRANAME,CONVERT(VARCHAR,CASE WHEN ISNULL(OPENING,0) = 0 THEN NULL ELSE OPENING END) RGRSWT,CONVERT(VARCHAR,CASE WHEN ISNULL(RECEIPT,0) = 0 THEN NULL ELSE RECEIPT END) RTOUCH,CONVERT(VARCHAR,CASE WHEN ISNULL(ISSUE,0) = 0 THEN NULL ELSE ISSUE END) RNETWT,NULL RAMOUNT"
        strSql += "  	,CONVERT(VARCHAR,CASE WHEN ISNULL(CLOSING,0) = 0 THEN NULL ELSE CLOSING END) IGRSWT,NULL ITOUCH,NULL INETWT,NULL IAMOUNT"
        strSql += "  	,'STOCK BALANCE' HEAD,4 ORDBY,'8' COLSEP,'5' TOTSEP,CONTRANAME CATNAME,NULL ACNAME,TRANDATE,'' COLHEAD,NULL PAGEBREAK"
        strSql += "  FROM TEMP" & systemId & "DAYSTOCKOPENING1 WHERE (OPENING <> 0 OR RECEIPT <> 0 OR ISSUE <> 0 OR CLOSING <> 0)"
        strSql += "  ORDER BY TRANDATE,ORDBY,HEAD,TOTSEP,CATNAME,COLSEP,ACNAME"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        
        dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        GridViewHeaderCreator(objGridShower.gridViewHeader)
        objGridShower.gridViewHeader.Font = New Font("VERDANA", 8, FontStyle.Bold)
        objGridShower.gridView.RowTemplate.Height = 21
        AddHandler objGridShower.gridView.ColumnWidthChanged, AddressOf gridView_ColumnWidthChanged
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        objGridShower.Text = "WEIGHT DAYBOOK"
        Dim tit As String = "WEIGHT DAYBOOK FROM " & dtpFrom.Text & " TO " & dtpTo.Text
        If (chkCmbCompany.Text <> "ALL") And (chkCmbCompany.Text <> "") Then
            tit += vbCrLf & "For " & chkCmbCompany.Text
        End If
        If (chkCmbCostCentre.Text <> "ALL") And (chkCmbCostCentre.Text <> "") Then
            tit += vbCrLf & chkCmbCostCentre.Text
        End If
        objGridShower.lblTitle.Text = tit
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(dtGrid)
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        objGridShower.FormReSize = True
        objGridShower.pnlFooter.Visible = False
        objGridShower.gridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        objGridShower.gridView.DefaultCellStyle.WrapMode = DataGridViewTriState.True
        DataGridView_SummaryFormatting(objGridShower.gridView)
        objGridShower.Show()
        objGridShower.gridViewHeader.Visible = True
        objGridShower.pnlHeader.Size = New Size(objGridShower.pnlHeader.Size.Width, objGridShower.pnlHeader.Size.Height + 10)
        objGridShower.FormReLocation = True
        FillGridGroupStyle_KeyNoWise(objGridShower.gridView, "PARTICULARS")
        objGridShower.gridView_ColumnWidthChanged(Me, New DataGridViewColumnEventArgs(objGridShower.gridView.Columns("PARTICULARS")))
        SetGridHeadColWidth(objGridShower.gridViewHeader)
        Prop_Sets()
    End Sub

    Private Sub gridView_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs)
        SetGridHeadColWidth(CType(sender, DataGridView))
    End Sub
    Private Sub GridViewHeaderCreator(ByVal gridviewHead As DataGridView)
        Dim dtHead As New DataTable
        strSql = "SELECT ''[PARTICULARS]"
        strSql += " ,''[RGRSWT~RTOUCH~RNETWT~RAMOUNT]"
        strSql += " ,''[IGRSWT~ITOUCH~INETWT~IAMOUNT]"
        strSql += " ,''SCROLL"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtHead)
        gridviewHead.DataSource = dtHead
        gridviewHead.Columns("RGRSWT~RTOUCH~RNETWT~RAMOUNT").HeaderText = "RECEIVED"
        gridviewHead.Columns("IGRSWT~ITOUCH~INETWT~IAMOUNT").HeaderText = "ISSUED"
        gridviewHead.Columns("SCROLL").HeaderText = ""
        gridviewHead.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        SetGridHeadColWidth(gridviewHead)
    End Sub
    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView)
        With dgv
            BrighttechPack.GlobalMethods.FormatGridColumns(dgv)

            .Columns("PARTICULARS").Width = 350
            .Columns("RGRSWT").Width = 150
            .Columns("RGRSWT").HeaderText = "GRSWT"
            .Columns("RTOUCH").Width = 150
            .Columns("RTOUCH").HeaderText = "TOUCH"
            .Columns("RNETWT").Width = 150
            .Columns("RNETWT").HeaderText = "NETWT"
            .Columns("RAMOUNT").Width = 150
            .Columns("RAMOUNT").HeaderText = "AMOUNT"
            .Columns("IGRSWT").Width = 150
            .Columns("IGRSWT").HeaderText = "GRSWT"
            .Columns("ITOUCH").Width = 150
            .Columns("ITOUCH").HeaderText = "TOUCH"
            .Columns("INETWT").Width = 150
            .Columns("INETWT").HeaderText = "NETWT"
            .Columns("IAMOUNT").Width = 150
            .Columns("IAMOUNT").HeaderText = "AMOUNT"

            .Columns("RGRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("RTOUCH").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("RNETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("RAMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            .Columns("IGRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("ITOUCH").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("INETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("IAMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            For CNT As Integer = 9 To .ColumnCount - 1
                .Columns(CNT).Visible = False
            Next
        End With
    End Sub

    Private Function funcCompanyFilt() As String
        Dim Str As String = ""
        If chkCmbCompany.Text <> "ALL" And chkCmbCompany.Text <> "" Then
            Str += " AND COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(chkCmbCompany.Text) & ") AND ISNULL(ACTIVE,'')<>'N')"
        Else
            Str += " AND COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N')"
        End If
        If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
            Str += " AND COSTID IN"
            Str += "(SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & GetQryString(chkCmbCostCentre.Text) & "))"
        End If
        Return Str
    End Function
    Private Sub funcView(ByVal CompanyFilt As String)
        strSql = " IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "DAYBOOKSUMMARY')"
        strSql += " 	DROP TABLE TEMP" & systemId & "DAYBOOKSUMMARY"
        strSql += " CREATE TABLE TEMP" & systemId & "DAYBOOKSUMMARY"
        strSql += " (PARTICULARS VARCHAR(250),CATNAME VARCHAR(250),ACNAME VARCHAR(250)"
        strSql += " ,RGRSWT NUMERIC(15,3),RTOUCH NUMERIC(15,3),RNETWT NUMERIC(15,3),RAMOUNT NUMERIC(15,2)"
        strSql += " ,IGRSWT NUMERIC(15,3),ITOUCH NUMERIC(15,3),INETWT NUMERIC(15,3),IAMOUNT NUMERIC(15,2)"
        strSql += " ,HEAD VARCHAR(250),ORDBY VARCHAR(1),COLSEP VARCHAR(1),TOTSEP VARCHAR(1),TRANDATE SMALLDATETIME,COLHEAD VARCHAR(2))"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strSql = " IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "DAYOPENING')"
        strSql += " 	DROP TABLE TEMP" & systemId & "DAYOPENING"
        strSql += " CREATE TABLE TEMP" & systemId & "DAYOPENING"
        strSql += " (TRANDATE SMALLDATETIME)"
        strSql += " DECLARE @RUNDATE SMALLDATETIME"
        strSql += " SELECT @RUNDATE = '" + dtpFrom.Value.Date.ToString("yyyy-MM-dd") + "'"
        strSql += " WHILE @RUNDATE <= '" + dtpTo.Value.Date.ToString("yyyy-MM-dd") + "'"
        strSql += " BEGIN"
        strSql += "  	INSERT INTO TEMP" & systemId & "DAYOPENING"
        strSql += " 	(TRANDATE)VALUES(@RUNDATE)"
        strSql += " 	SELECT @RUNDATE = DATEADD(DAY,1,@RUNDATE)"
        strSql += " END"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strSql = " IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "DAYOPENING1')"
        strSql += " 	DROP TABLE TEMP" & systemId & "DAYOPENING1"
        strSql += " CREATE TABLE TEMP" & systemId & "DAYOPENING1"
        strSql += " (CONTRANAME VARCHAR(250),DEBIT NUMERIC(15,2),CREDIT NUMERIC(15,2),ACNAME VARCHAR(250),MODE VARCHAR(10)"
        strSql += " ,TRANDATE SMALLDATETIME,ACCODE VARCHAR(25))"
        strSql += " INSERT INTO TEMP" & systemId & "DAYOPENING1"
        strSql += " (ACCODE,TRANDATE)"
        strSql += " SELECT X.ACCODE,TRANDATE FROM"
        strSql += " ("
        strSql += " 	SELECT DISTINCT ACCODE FROM " & cnStockDb & "..OPENTRAILBALANCE "
        strSql += "     WHERE 1 = 1"
        strSql += CompanyFilt
        strSql += " 	UNION "
        strSql += " 	SELECT DISTINCT ACCODE FROM " & cnStockDb & "..ACCTRAN"
        strSql += "     WHERE TRANDATE <=  '" + dtpTo.Value.Date.ToString("yyyy-MM-dd") + "'"
        strSql += CompanyFilt
        strSql += " )X,TEMP" & systemId & "DAYOPENING"
        strSql += " WHERE ISNULL(X.ACCODE,'') IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACGRPCODE IN ('1','2'))"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strSql = " UPDATE TEMP" & systemId & "DAYOPENING1 SET"
        strSql += " 	 CONTRANAME = ' OPENING BALANCE'"
        strSql += " 	,DEBIT = (ISNULL((SELECT SUM(DEBIT) FROM " & cnStockDb & "..OPENTRAILBALANCE WHERE ACCODE = T1.ACCODE " & CompanyFilt & "),0)"
        strSql += " 			+ "
        strSql += " 		   ISNULL((SELECT SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT END) FROM " & cnStockDb & "..ACCTRAN WHERE ACCODE = T1.ACCODE " & CompanyFilt & " AND TRANDATE < T1.TRANDATE AND ISNULL(CANCEL,'') != 'Y'),0))"
        strSql += " 	,CREDIT = (ISNULL((SELECT SUM(CREDIT) FROM " & cnStockDb & "..OPENTRAILBALANCE WHERE ACCODE = T1.ACCODE " & CompanyFilt & "),0)"
        strSql += " 			+ "
        strSql += " 		   ISNULL((SELECT SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT END) FROM " & cnStockDb & "..ACCTRAN WHERE ACCODE = T1.ACCODE " & CompanyFilt & " AND TRANDATE < T1.TRANDATE AND ISNULL(CANCEL,'') != 'Y'),0))"
        strSql += " 	,ACNAME = (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T1.ACCODE)"
        strSql += " FROM TEMP" & systemId & "DAYOPENING1 AS T1"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strSql = " UPDATE TEMP" & systemId & "DAYOPENING1 SET "
        strSql += " 	 DEBIT  = CASE WHEN ISNULL(DEBIT,0) > ISNULL(CREDIT,0) THEN ISNULL(DEBIT,0) - ISNULL(CREDIT,0) END"
        strSql += " 	,CREDIT = CASE WHEN ISNULL(CREDIT,0) > ISNULL(DEBIT,0) THEN ISNULL(CREDIT,0) - ISNULL(DEBIT,0) END"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strSql = " IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "PURSALES')"
        strSql += " 	DROP TABLE TEMP" & systemId & "PURSALES"
        strSql += " CREATE TABLE TEMP" & systemId & "PURSALES "
        strSql += " (ACCODE VARCHAR(250),ITEMID VARCHAR(50),GRSWT NUMERIC(15,3),TOUCH NUMERIC(15,3)"
        strSql += " ,NETWT NUMERIC(15,3),AMOUNT NUMERIC(15,2),MODE VARCHAR(3),TRANDATE SMALLDATETIME,COSTID VARCHAR(10)"
        strSql += " ,COMPANYID VARCHAR(10),TRANNO VARCHAR(25),SNO VARCHAR(25),BATCHNO VARCHAR(25))"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strSql = " INSERT INTO TEMP" & systemId & "PURSALES"
        strSql += " (ACCODE,ITEMID,GRSWT,TOUCH,NETWT,AMOUNT,MODE,TRANDATE,COSTID,COMPANYID,TRANNO,SNO,BATCHNO)"
        strSql += " SELECT "
        strSql += " 	 ACCODE"
        strSql += " 	,ITEMID"
        strSql += " 	,GRSWT,TOUCH,NETWT,AMOUNT"
        strSql += " 	,'P' MODE,TRANDATE,COSTID,COMPANYID,TRANNO,SNO,BATCHNO"
        strSql += " FROM " & cnStockDb & "..RECEIPT AS R"
        strSql += " WHERE TRANDATE BETWEEN '" + dtpFrom.Value.Date.ToString("yyyy-MM-dd") + "' AND '" + dtpTo.Value.Date.ToString("yyyy-MM-dd") + "' " & CompanyFilt & " AND ISNULL(CANCEL,'') != 'Y'"
        strSql += " UNION ALL"
        strSql += " SELECT "
        strSql += " 	 ACCODE"
        strSql += " 	,ITEMID"
        strSql += " 	,GRSWT,TOUCH,NETWT,AMOUNT"
        strSql += " 	,'S' MODE,TRANDATE,COSTID,COMPANYID,TRANNO,SNO,BATCHNO"
        strSql += " FROM " & cnStockDb & "..ISSUE AS I"
        strSql += " WHERE TRANDATE BETWEEN '" + dtpFrom.Value.Date.ToString("yyyy-MM-dd") + "' AND '" + dtpTo.Value.Date.ToString("yyyy-MM-dd") + "' " & CompanyFilt & " AND ISNULL(CANCEL,'') != 'Y'"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strSql = " IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "PURSALES1')"
        strSql += " 	DROP TABLE TEMP" & systemId & "PURSALES1"
        strSql += " SELECT"
        strSql += "  	 CASE MODE WHEN 'P' THEN 'PURCHASE' ELSE 'SALES' END HEAD"
        strSql += " 	,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE"
        strSql += " 		IN (SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)) CATNAME"
        strSql += " 	,CASE WHEN ISNULL((SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.ACCODE),'') != '' THEN  ISNULL((SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.ACCODE),'') ELSE (SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO IN (SELECT PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = T.BATCHNO)) END ACNAME"
        strSql += " 	,CASE MODE WHEN 'P' THEN GRSWT END RGRSWT"
        strSql += " 	,CASE MODE WHEN 'P' THEN TOUCH END RTOUCH"
        strSql += " 	,CASE MODE WHEN 'P' THEN NETWT END RNETWT"
        strSql += " 	,CASE MODE WHEN 'P' THEN AMOUNT END RAMOUNT"
        strSql += " 	,CASE MODE WHEN 'S' THEN GRSWT END IGRSWT"
        strSql += " 	,CASE MODE WHEN 'S' THEN TOUCH END ITOUCH"
        strSql += " 	,CASE MODE WHEN 'S' THEN NETWT END INETWT"
        strSql += " 	,CASE MODE WHEN 'S' THEN AMOUNT END IAMOUNT"
        strSql += " 	,TRANDATE,TRANNO,SNO"
        strSql += " INTO TEMP" & systemId & "PURSALES1"
        strSql += " FROM TEMP" & systemId & "PURSALES AS T"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strSql = " IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ACCVALUES')"
        strSql += " 	DROP TABLE TEMP" & systemId & "ACCVALUES"
        strSql += " SELECT "
        strSql += " 	 (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = A.CONTRA) CONTRANAME"
        strSql += " 	,CASE WHEN TRANMODE = 'D' THEN AMOUNT END RAMOUNT"
        strSql += " 	,CASE WHEN TRANMODE = 'C' THEN AMOUNT END IAMOUNT"
        strSql += " 	,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = A.ACCODE) ACNAME ,'A' MODE"
        strSql += " 	,TRANDATE,ACCODE,TRANNO,SNO"
        strSql += " INTO TEMP" & systemId & "ACCVALUES"
        strSql += " FROM " & cnStockDb & "..ACCTRAN AS A"
        strSql += " WHERE ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACGRPCODE IN ('1','2'))"
        strSql += " AND TRANDATE BETWEEN '" + dtpFrom.Value.Date.ToString("yyyy-MM-dd") + "' AND '" + dtpTo.Value.Date.ToString("yyyy-MM-dd") + "' " & CompanyFilt & " AND ISNULL(CANCEL,'') != 'Y'"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strSql = " INSERT INTO TEMP" & systemId & "DAYBOOKSUMMARY" & vbCrLf
        strSql += " (" & vbCrLf
        strSql += " 	 PARTICULARS,CATNAME,ACNAME,RGRSWT,RTOUCH,RNETWT,RAMOUNT,IGRSWT,ITOUCH,INETWT,IAMOUNT,HEAD,ORDBY,COLSEP,TOTSEP" & vbCrLf
        strSql += " 	,TRANDATE,COLHEAD" & vbCrLf
        strSql += " )" & vbCrLf
        strSql += " SELECT" & vbCrLf
        strSql += "  	 DISTINCT ACNAME PARTICULARS" & vbCrLf
        strSql += "  	,NULL CATNAME,ACNAME,NULL RGRSWT,NULL RTOUCH,NULL RNETWT,NULL RAMOUNT" & vbCrLf
        strSql += "  	,NULL IGRSWT,NULL ITOUCH,NULL INETWT,NULL IAMOUNT,ACNAME HEAD" & vbCrLf
        strSql += "  	,'0' ORDBY,'1' COLSEP,1 TOTSEP,TRANDATE,'T' COLHEAD" & vbCrLf
        strSql += "  FROM (" & vbCrLf
        strSql += "  SELECT" & vbCrLf
        strSql += "  	 DISTINCT ACNAME PARTICULARS" & vbCrLf
        strSql += "  	,NULL CATNAME,ACNAME,NULL RGRSWT,NULL RTOUCH,NULL RNETWT,NULL RAMOUNT" & vbCrLf
        strSql += "  	,NULL IGRSWT,NULL ITOUCH,NULL INETWT,NULL IAMOUNT,ACNAME HEAD" & vbCrLf
        strSql += "  	,'0' ORDBY,'1' COLSEP,1 TOTSEP,TRANDATE,'T' COLHEAD" & vbCrLf
        strSql += "  FROM TEMP" & systemId & "ACCVALUES" & vbCrLf
        strSql += "  UNION" & vbCrLf
        strSql += "  SELECT" & vbCrLf
        strSql += "  	 DISTINCT ACNAME PARTICULARS" & vbCrLf
        strSql += "  	,NULL CATNAME,ACNAME,NULL RGRSWT,NULL RTOUCH,NULL RNETWT,NULL RAMOUNT" & vbCrLf
        strSql += "  	,NULL IGRSWT,NULL ITOUCH,NULL INETWT,NULL IAMOUNT,ACNAME HEAD" & vbCrLf
        strSql += "  	,'0' ORDBY,'1' COLSEP,1 TOTSEP,TRANDATE,'T' COLHEAD" & vbCrLf
        strSql += "  FROM TEMP" & systemId & "DAYOPENING1 ) X" & vbCrLf
        strSql += "  UNION ALL" & vbCrLf
        strSql += "  SELECT " & vbCrLf
        strSql += "  	 CONTRANAME PARTICULARS,NULL CATNAME,ACNAME,NULL RGRSWT,NULL RTOUCH,NULL RNETWT" & vbCrLf
        strSql += "  	,SUM(DEBIT) RAMOUNT,NULL IGRSWT,NULL ITOUCH,NULL INETWT,SUM(CREDIT) IAMOUNT" & vbCrLf
        strSql += "  	,ACNAME HEAD,'0' ORDBY,'2' COLSEP,1 TOTSEP,TRANDATE,'' COLHEAD" & vbCrLf
        strSql += "  FROM TEMP" & systemId & "DAYOPENING1" & vbCrLf
        strSql += "  GROUP BY CONTRANAME,ACNAME,TRANDATE" & vbCrLf
        strSql += "  UNION ALL" & vbCrLf
        strSql += "  SELECT " & vbCrLf
        strSql += "  	' ' + CONTRANAME PARTICULARS" & vbCrLf
        strSql += "  	,NULL CATNAME,ACNAME,NULL RGRSWT,NULL RTOUCH,NULL RNETWT" & vbCrLf
        strSql += "  	,SUM(RAMOUNT) RAMOUNT,NULL IGRSWT,NULL ITOUCH,NULL INETWT,SUM(IAMOUNT) IAMOUNT" & vbCrLf
        strSql += "  	,ACNAME HEAD,'0' ORDBY,'2' COLSEP,1 TOTSEP,TRANDATE,'' COLHEAD" & vbCrLf
        strSql += "  FROM TEMP" & systemId & "ACCVALUES WHERE TRANDATE BETWEEN '" + dtpFrom.Value.Date.ToString("yyyy-MM-dd") + "' AND '" + dtpTo.Value.Date.ToString("yyyy-MM-dd") + "'"
        strSql += " GROUP BY ACNAME,CONTRANAME,TRANDATE,TRANNO,SNO" & vbCrLf
        strSql += "  UNION ALL" & vbCrLf
        strSql += "  SELECT " & vbCrLf
        strSql += "  	 DISTINCT HEAD PARTICULARS" & vbCrLf
        strSql += "  	,NULL CATNAME,NULL ACNAME,NULL RGRSWT,NULL RTOUCH,NULL RNETWT,NULL RAMOUNT" & vbCrLf
        strSql += "  	,NULL IGRSWT,NULL ITOUCH,NULL INETWT,NULL IAMOUNT,HEAD" & vbCrLf
        strSql += "  	,CASE HEAD WHEN 'PURCHASE' THEN '1' WHEN 'SALES' THEN '2' END ORDBY	" & vbCrLf
        strSql += "  	,'1' COLSEP,1 TOTSEP,TRANDATE,'T' COLHEAD" & vbCrLf
        strSql += "  FROM TEMP" & systemId & "PURSALES1" & vbCrLf
        strSql += "  UNION ALL" & vbCrLf
        strSql += "  SELECT" & vbCrLf
        strSql += "  	 DISTINCT ' ' + ISNULL(CATNAME,'') PARTICULARS" & vbCrLf
        strSql += "  	,CATNAME,NULL ACNAME,NULL RGRSWT,NULL RTOUCH,NULL RNETWT,NULL RAMOUNT" & vbCrLf
        strSql += "  	,NULL IGRSWT,NULL ITOUCH,NULL INETWT,NULL IAMOUNT,HEAD" & vbCrLf
        strSql += "  	,CASE HEAD WHEN 'PURCHASE' THEN '1' WHEN 'SALES' THEN '2' END ORDBY	" & vbCrLf
        strSql += "  	,'2' COLSEP,1 TOTSEP,TRANDATE,'T1' COLHEAD" & vbCrLf
        strSql += "  FROM TEMP" & systemId & "PURSALES1" & vbCrLf
        strSql += "  UNION ALL" & vbCrLf
        strSql += "  SELECT" & vbCrLf
        strSql += "  	 '  ' + ISNULL(ACNAME,'') PARTICULARS" & vbCrLf
        strSql += "  	,CATNAME,ACNAME,RGRSWT,RTOUCH,RNETWT,RAMOUNT" & vbCrLf
        strSql += "  	,IGRSWT,ITOUCH,INETWT,IAMOUNT,HEAD" & vbCrLf
        strSql += "  	,CASE HEAD WHEN 'PURCHASE' THEN '1' WHEN 'SALES' THEN '2' END ORDBY	" & vbCrLf
        strSql += "  	,'3' COLSEP,1 TOTSEP,TRANDATE,'' COLHEAD" & vbCrLf
        strSql += "  FROM TEMP" & systemId & "PURSALES1" & vbCrLf
        strSql += "  UNION ALL" & vbCrLf
        strSql += "  SELECT" & vbCrLf
        strSql += "  	 ' Total' PARTICULARS" & vbCrLf
        strSql += "  	,CATNAME,'' ACNAME,SUM(RGRSWT) RGRSWT,NULL RTOUCH,SUM(RNETWT) RNETWT,SUM(RAMOUNT) RAMOUNT" & vbCrLf
        strSql += "  	,SUM(IGRSWT) IGRSWT,NULL ITOUCH,SUM(INETWT) INETWT,SUM(IAMOUNT) IAMOUNT,HEAD" & vbCrLf
        strSql += "  	,CASE HEAD WHEN 'PURCHASE' THEN '1' WHEN 'SALES' THEN '2' END ORDBY	" & vbCrLf
        strSql += "  	,'4' COLSEP,1 TOTSEP,TRANDATE,'S1' COLHEAD" & vbCrLf
        strSql += "  FROM TEMP" & systemId & "PURSALES1" & vbCrLf
        strSql += "  GROUP BY CATNAME,HEAD,TRANDATE" & vbCrLf
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

    End Sub
    Private Sub Prop_Sets()
        Dim obj As New frmWeightDayBook_Properties
        GetChecked_CheckedList(chkCmbCompany, obj.p_chkCmbCompany)
        GetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre)
        SetSettingsObj(obj, Me.Name, GetType(frmWeightDayBook_Properties))
    End Sub
    Private Sub Prop_Gets()
        Dim obj As New frmWeightDayBook_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmWeightDayBook_Properties))
        SetChecked_CheckedList(chkCmbCompany, obj.p_chkCmbCompany, strCompanyName)
        'SetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre, cnCostName)
    End Sub
End Class

Public Class frmWeightDayBook_Properties
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
End Class