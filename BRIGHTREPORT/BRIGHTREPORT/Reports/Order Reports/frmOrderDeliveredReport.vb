Imports System.Data.OleDb
Public Class frmOrderDeliveredReport
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Dim objGridShower As frmGridDispDia
    Dim dtMetal As New DataTable
    Dim dtCounter As New DataTable()
    Dim dtCostCentre As New DataTable
    Dim dtItem As New DataTable
    Dim dtDesigner As New DataTable
    Dim dtOrderStatus As New DataTable
    Dim dtSoftKeys As DataTable = GetAdmindbSoftValueAll()
    Dim READYITEMBOOKING As Boolean = IIf(GetAdmindbSoftValuefromDt(dtSoftKeys, "READYITEMBOOKING", "N") = "Y", True, False)


    Private Sub frmOrderDeliveredReport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmOrderDeliveredReport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        dtpFrom.MinimumDate = (New DateTimePicker).MinDate
        dtpFrom.MaximumDate = (New DateTimePicker).MaxDate
        dtpTo.MinimumDate = (New DateTimePicker).MinDate
        dtpTo.MaximumDate = (New DateTimePicker).MaxDate

        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.TabGeneral.Left, Me.TabGeneral.Top, Me.TabGeneral.Width, Me.TabGeneral.Height))
        grpContainer.Location = New Point((ScreenWid - grpContainer.Width) / 2, ((ScreenHit - 128) - grpContainer.Height) / 2)
        strSql = " SELECT 'ALL' METALNAME,'ALL' METALID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT METALNAME,METALID,2 RESULT FROM " & cnAdminDb & "..METALMAST "
        strSql += " ORDER BY RESULT,METALNAME"
        dtMetal = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtMetal)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbMetal, dtMetal, "METALNAME", , "ALL")

        strSql = " SELECT 'ALL' ITEMCTRNAME,'ALL' ITEMCTRID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT ITEMCTRNAME,CONVERT(vARCHAR,ITEMCTRID),2 RESULT FROM " & cnAdminDb & "..ITEMCOUNTER "
        strSql += " ORDER BY RESULT,ITEMCTRNAME"

        dtCounter = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCounter)
        BrighttechPack.GlobalMethods.FillCombo(chkcountercmb, dtCounter, "ITEMCTRNAME", , "ALL")

        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False
        strSql = " SELECT 'ALL' DESIGNERNAME,'ALL' DESIGNERID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT DESIGNERNAME,CONVERT(vARCHAR,DESIGNERID),2 RESULT FROM " & cnAdminDb & "..DESIGNER"
        strSql += " ORDER BY RESULT,DESIGNERNAME"
        dtDesigner = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtDesigner)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbDesigner, dtDesigner, "DESIGNERNAME", , "ALL")
        Dim dthead As DataTable
        strSql = " SELECT 'ALL' SMITHNAME,'ALL' ACCODE,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT ACNAME SMITHNAME,ACCODE,2 RESULT FROM " & cnAdminDb & "..ACHEAD WHERE ACTYPE IN ('G','D')"
        strSql += " ORDER BY RESULT,SMITHNAME"
        dthead = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dthead)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbDealer, dthead, "SMITHNAME", , "ALL")

        cmbEmpName.Items.Clear()
        cmbEmpName.Items.Add("ALL")
        strSql = " SELECT EMPNAME FROM " & cnAdminDb & "..EMPMASTER WHERE ACTIVE = 'Y' ORDER BY EMPNAME"
        objGPack.FillCombo(strSql, cmbEmpName, False, False)
        getorderby()
        btnNew_Click(Me, New EventArgs)
        If READYITEMBOOKING Then rbtbooked.Visible = True Else rbtbooked.Visible = False
    End Sub
    Private Sub getorderby()
        cmbOrderBy.Items.Clear()
        If rbtOrder.Checked Then
            cmbOrderBy.Items.Add("ORDER DATE")
        ElseIf rbtbooked.Checked Then
            cmbOrderBy.Items.Add("BOOKED DATE")
        Else
            cmbOrderBy.Items.Add("REPAIR DATE")
        End If
        cmbOrderBy.Items.Add("CUSTOMER WISE")
        cmbOrderBy.Items.Add("DESIGNER WISE")
        cmbOrderBy.Items.Add("SMITH/DEALER WISE")
        cmbOrderBy.Items.Add("ORDER NO")
        cmbOrderBy.SelectedIndex = 0
    End Sub

    Function funcLoadItemName() As Integer
        strSql = " SELECT 'ALL' ITEMNAME,'ALL' ITEMID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT ITEMNAME,CONVERT(vARCHAR,ITEMID),2 RESULT FROM " & cnAdminDb & "..ITEMMAST"
        strSql += " WHERE ACTIVE = 'Y'"
        If chkCmbMetal.Text <> "ALL" And chkCmbMetal.Text <> "" Then
            strSql += " AND METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & GetQryString(chkCmbMetal.Text) & "))"
        End If
        strSql += " ORDER BY RESULT,ITEMNAME"
        dtItem = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtItem)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbItem, dtItem, "ITEMNAME", , "ALL")
    End Function
    Private Sub chkCmbMetal_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCmbMetal.TextChanged
        funcLoadItemName()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        tabMain.SelectedTab = TabGeneral
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()

        chkAsOnDate.Select()
        Prop_Gets()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        If rbtformat2.Checked Then
            funcformat2()
        Else
            OrderStatus()
        End If
        Prop_Sets()
    End Sub

    Private Sub funcformat2()
        strSql = " IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMPORSTATUS' AND XTYPE = 'U') > 0 DROP TABLE TEMPTABLEDB..TEMPORSTATUS"
        strSql += " IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'TEMPORSTATUS' AND XTYPE = 'V') > 0 DROP VIEW TEMPORSTATUS"
        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + " SELECT  TRANNO"
        strSql += vbCrLf + " ,(SELECT TOP 1 SUBSTRING(ORNO,6,20) FROM " & cnAdminDb & "..ORMAST WHERE ODBATCHNO=I.BATCHNO)ORNO"
        strSql += vbCrLf + " ,(SELECT SUM(GRSWT) FROM " & cnAdminDb & "..ORMAST WHERE ODBATCHNO=I.BATCHNO)ORDWT"
        strSql += vbCrLf + " ,(SELECT SUM(GRSWT) FROM " & cnAdminDb & "..OUTSTANDING "
        strSql += vbCrLf + " WHERE RUNNO IN(SELECT ORNO FROM " & cnAdminDb & "..ORMAST WHERE ODBATCHNO=I.BATCHNO) )ADVWT"
        strSql += vbCrLf + " ,SUM(GRSWT)SALESWT "
        strSql += vbCrLf + " ,(SELECT TOP 1 PNAME FROM " & cnAdminDb & "..PERSONALINFO "
        strSql += vbCrLf + " WHERE SNO=(SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO=I.BATCHNO))NAME"
        strSql += vbCrLf + " ,1 AS RESULT,' ' AS COLHEAD"
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMPORSTATUS"
        strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I"
        If chkAsOnDate.Checked Then
            strSql += vbCrLf + " WHERE TRANDATE <= '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        Else
            strSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        End If
        strSql += vbCrLf + " AND TRANTYPE IN('SA','OD')"
        strSql += vbCrLf + " AND ISNULL(CANCEL,'')=''"
        strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
        If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
            strSql += vbCrLf + "  AND   COSTID IN (SELECT  COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & GetQryString(chkCmbCostCentre.Text) & "))"
        End If
        strSql += vbCrLf + " AND BATCHNO IN(SELECT ODBATCHNO FROM " & cnAdminDb & "..ORMAST WHERE 1=1"
        If rbtOrder.Checked Then strSql += vbCrLf + " AND  ORTYPE='O'"
        If rbtRepair.Checked Then strSql += vbCrLf + " AND  ORTYPE='R'"
        If rbtbooked.Checked Then strSql += vbCrLf + " AND  ORTYPE='B'"
        strSql += vbCrLf + " )"
        strSql += vbCrLf + " GROUP BY TRANNO,BATCHNO"
        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMPORSTATUS)>0"
        strSql += vbCrLf + " BEGIN"
        strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPORSTATUS(ORDWT,ADVWT,SALESWT,ORNO,RESULT,COLHEAD)"
        strSql += vbCrLf + " SELECT SUM(ORDWT),SUM(ADVWT),SUM(SALESWT) ,'TOTAL',2,'G' FROM TEMPTABLEDB..TEMPORSTATUS"
        strSql += vbCrLf + " END"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()


        strSql = " SELECT * FROM TEMPTABLEDB..TEMPORSTATUS"
        strSql += " ORDER BY RESULT,TRANNO"

        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("KEYNO", GetType(Integer))
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            dtpFrom.Select()
            Exit Sub
        End If

        dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        Dim tit As String = ""
        If READYITEMBOOKING Then
            objGridShower.Text = "ORDER/REPAIR DELIVERED REPORT"
            tit += "ORDER/REPAIR DELIVERED  REPORT "
        Else
            objGridShower.Text = "ORDER/REPAIR/BOOKED DELIVERED REPORT"
            tit += "ORDER/REPAIR/BOOKED DELIVERED  REPORT "
        End If

        If chkAsOnDate.Checked Then
            tit += " AS ONDATE " & dtpFrom.Text
        Else
            tit += "FROM " + dtpFrom.Text + " TO " + dtpTo.Text
        End If
        tit += IIf(chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "", " :" & chkCmbCostCentre.Text, "")
        objGridShower.lblTitle.Text = tit
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(dtGrid)
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)

        objGridShower.FormReSize = True
        objGridShower.FormReLocation = False
        objGridShower.pnlFooter.Visible = False
        objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        FormatGridColumns(objGridShower.gridView, False, False, , False)
        With objGridShower.gridView
            .Columns("ORDWT").DefaultCellStyle.Format = FormatNumberStyle(3)
            .Columns("ADVWT").DefaultCellStyle.Format = FormatNumberStyle(3)
            .Columns("SALESWT").DefaultCellStyle.Format = FormatNumberStyle(3)
            If .Columns.Contains("KEYNO") Then .Columns("KEYNO").Visible = False
            If .Columns.Contains("RESULT") Then .Columns("RESULT").Visible = False
            If .Columns.Contains("COLHEAD") Then .Columns("COLHEAD").Visible = False
        End With
        objGridShower.Show()
        objGridShower.FormReSize = True
        objGridShower.gridViewHeader.Visible = False
        objGridShower.pnlFooter.Visible = False
        FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
    End Sub
    Private Sub OrderStatus()
        Dim chkCostName As String = ""
        Dim chkMetalName As String = ""
        Dim chkItemName As String = ""
        Dim chkDesigner As String = ""
        Dim chkStatus As String = ""

        strSql = " IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMPORSTATUS' AND XTYPE = 'U') > 0 DROP TABLE TEMPTABLEDB..TEMPORSTATUS"
        strSql += " IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'TEMPORSTATUS' AND XTYPE = 'V') > 0 DROP VIEW TEMPORSTATUS"
        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " SELECT DISTINCT CONVERT(VARCHAR(500),NULL)PARTICULAR,O.STYLENO,SUBSTRING(O.ORNO,6,20)ORNO,CONVERT(VARCHAR(15),ORDATE,103)ORDATE,REMDATE,O.DUEDATE"
        strSql += vbCrLf + " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST IM WHERE IM.ITEMID=O.ITEMID) + '-'+ O.DESCRIPT AS DESCRIPT"
        strSql += vbCrLf + " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST IM WHERE IM.SUBITEMID=O.SUBITEMID) AS SUBITEM"
        strSql += vbCrLf + " ,I.PCS,I.GRSWT,I.NETWT,I.MCGRM,I.MCHARGE AS MC,I.WASTPER,I.WASTAGE AS WAST"
        strSql += vbCrLf + " ,I.AMOUNT,I.TAX "
        strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ORSTONE OS WHERE OS.ORSNO=O.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNPCS"
        strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ORSTONE OS WHERE OS.ORSNO=O.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNWT"
        strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ORSTONE WHERE ORSNO = O.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))AS DIAPCS"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & DiaRnd & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ORSTONE WHERE ORSNO = O.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
        strSql += vbCrLf + " ,O.SIZENO"
        strSql += vbCrLf + " ,CONVERT(VARCHAR(100),CASE WHEN ISNULL(O.ODBATCHNO,'') <> '' AND ISNULL(ISS.SNO,'')='' "
        'strSql += vbCrLf + " And Isnull((Select top 1 trandate from " & cnStockDb & "..issue where isnull(CANCEL,'')<>'C' and batchno=o.odbatchno),'')<='" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " THEN (SELECT TOP 1 ORDSTATE_NAME FROM " & cnAdminDb & "..ORDERSTATUS WHERE ORDSTATE_ID = 5)"
        strSql += vbCrLf + " WHEN ISNULL(O.ODBATCHNO,'') <> '' AND ISNULL(ISS.SNO,'')<>'' THEN 'APPROVAL ISSUE'"
        strSql += vbCrLf + "   WHEN ISNULL(O.ORDCANCEL,'') <> '' THEN (SELECT ORDSTATE_NAME FROM " & cnAdminDb & "..ORDERSTATUS WHERE ORDSTATE_ID = 6)"
        strSql += vbCrLf + "   ELSE ISNULL((SELECT ORDSTATE_NAME FROM " & cnAdminDb & "..ORDERSTATUS WHERE ORDSTATE_ID = (SELECT TOP 1 ORDSTATE_ID FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORSNO = O.SNO AND ISNULL(CANCEL,'') = '' ORDER BY ENTRYORDER DESC)),'PENDING WITH US') "
        strSql += vbCrLf + "   END) AS STATUS"
        strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),O.RATE)AS ORRATE"
        If chkAsOnDate.Checked = True Then
            strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),(SELECT SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE -1*AMOUNT END) FROM " & cnAdminDb & "..OUTSTANDING WHERE TRANDATE <= '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND RUNNO = O.ORNO AND ISNULL(CANCEL,'') = ''))AS ADV_AMT"
            strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,3),(SELECT SUM(CASE WHEN RECPAY = 'P' THEN NETWT ELSE -1*NETWT END) FROM " & cnAdminDb & "..OUTSTANDING WHERE TRANDATE <= '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'AND RUNNO = O.ORNO  AND ISNULL(CANCEL,'') = ''))AS ADV_WT"
        Else
            strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),(SELECT SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE -1*AMOUNT END) FROM " & cnAdminDb & "..OUTSTANDING WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND RUNNO = O.ORNO AND ISNULL(CANCEL,'') = ''))AS ADV_AMT"
            strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,3),(SELECT SUM(CASE WHEN RECPAY = 'P' THEN NETWT ELSE -1*NETWT END) FROM " & cnAdminDb & "..OUTSTANDING WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND RUNNO = O.ORNO  AND ISNULL(CANCEL,'') = ''))AS ADV_WT"
        End If
        strSql += vbCrLf + " ,O.GRSWT AS ORWT ,(SELECT SUM(GRSWT) FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO=O.ORNO AND RECPAY='R') ADWT "
        strSql += vbCrLf + " ,I.GRSWT AS SALEWT"
        strSql += vbCrLf + " ,CONVERT(VARCHAR,P.PNAME + CASE WHEN ISNULL(INITIAL,'') <> '' THEN INITIAL ELSE '' END"
        strSql += vbCrLf + "  + CASE WHEN ISNULL(P.PHONERES,'') <> '' THEN CASE WHEN ISNULL(P.MOBILE,'') <> '' THEN ' ('+P.PHONERES+','+P.MOBILE+')' ELSE ' ('+P.PHONERES+')' END   ELSE CASE WHEN ISNULL(P.MOBILE,'') <> '' THEN ' ('+P.MOBILE+')' ELSE '' END END) AS PNAME "
        strSql += vbCrLf + "  ,CONVERT(VARCHAR(20),CASE WHEN ISNULL(O.ODBATCHNO,'') <> '' THEN (SELECT TOP 1 TRANNO  FROM " & cnStockDb & "..ISSUE WHERE BATCHNO  = O.ODBATCHNO GROUP BY TRANNO) ELSE NULL END) DELIVERYNO"
        strSql += vbCrLf + "  ,CONVERT(VARCHAR(12),CASE WHEN ISNULL(O.ODBATCHNO,'') <> '' THEN (SELECT TOP 1 TRANDATE FROM " & cnStockDb & "..ISSUE WHERE BATCHNO  = O.ODBATCHNO GROUP BY TRANDATE) ELSE NULL END,103) DELIVERYDATE"
        strSql += vbCrLf + " ,(SELECT  TOP 1 DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = (SELECT TOP 1 DESIGNERID FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORSNO = O.SNO AND ORSTATUS = 'I' AND ISNULL(CANCEL,'') = ''))DESIGNER"
        strSql += vbCrLf + "  ,(SELECT  TOP 1 COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = O.COSTID)AS COSTNAME"
        strSql += vbCrLf + "  ,(SELECT  TOP 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = O.PROPSMITH)AS SMITHNAME"
        strSql += vbCrLf + "  ,O.BATCHNO"
        strSql += vbCrLf + "  ,(SELECT  TOP 1 EMPNAME FROM " & cnAdminDb & "..EMPMASTER WHERE EMPID = O.EMPID)AS EMPNAME"

        strSql += vbCrLf + "  ,CONVERT(vARCHAR(3),NULL)COLHEAD"
        strSql += vbCrLf + "  ,CONVERT(INT,1)RESULT"
        strSql += vbCrLf + "  ,CONVERT(INT,NULL)GRESULT,CONVERT(VARCHAR(15),O.SNO)SNO"
        strSql += vbCrLf + "  ,IDENTITY(INT,1,1)ROWID"
        strSql += vbCrLf + "  ,(SELECT TOP 1 TRANNO FROM " & cnAdminDb & "..ORIRDETAIL WHERE ISNULL(ORSNO,'') = ISNULL(O.SNO,'')) TRANNO"
        strSql += vbCrLf + "  ,(SELECT  TOP 1 ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = T.ITEMCTRID) COUNTER"
        strSql += vbCrLf + " ,O.ORNO as ORSNO"
        strSql += vbCrLf + " ,(SELECT TOP 1 TRANNO FROM " & cnStockDb & "..ISSUE WHERE BATCHNO=O.ODBATCHNO)SALTRANNO"
        strSql += vbCrLf + "  INTO TEMPTABLEDB..TEMPORSTATUS"
        strSql += vbCrLf + "  FROM " & cnAdminDb & "..ORMAST O"
        strSql += vbCrLf + "  INNER JOIN " & cnStockDb & "..ISSUE I ON I.BATCHNO = O.ODBATCHNO AND I.SNO=O.ODSNO"
        If txtCustomerName.Text <> "" Then
            strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..CUSTOMERINFO CU ON CU.BATCHNO = O.BATCHNO"
            strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..PERSONALINFO AS P ON P.SNO = CU.PSNO"
            strSql += vbCrLf + " AND P.PNAME LIKE '" & txtCustomerName.Text & "%'"
        Else
            strSql += vbCrLf + "  LEFT OUTER JOIN " & cnAdminDb & "..CUSTOMERINFO CU ON CU.BATCHNO = O.BATCHNO"
            strSql += vbCrLf + "  LEFT OUTER JOIN " & cnAdminDb & "..PERSONALINFO AS P ON P.SNO = CU.PSNO"
        End If
        strSql += vbCrLf + "  LEFT OUTER JOIN " & cnAdminDb & "..ITEMTAG AS T ON ISNULL(T.ORSNO,'') = O.SNO AND ISNULL(T.ORDREPNO,'') = O.ORNO"
        strSql += vbCrLf + "  LEFT OUTER JOIN " & cnStockDb & "..ISSUE ISS ON ISS.SNO =O.ODSNO AND ISNULL(ISS.TRANTYPE,'')='AI'"
        'strSql += vbCrLf + " LEFT OUTER JOIN " & cnadmindb & "..ORIRDETAIL AS I ON ISNULL(I.ORSNO,'') = ISNULL(O.SNO,'')"
        If rbtformat2.Checked Then
            strSql += vbCrLf + "  WHERE I.TRANDATE "
        Else
            strSql += vbCrLf + "  WHERE " & IIf(rbtOrderDate.Checked, "O.ORDATE", "O.DUEDATE") & " "
        End If
        If chkAsOnDate.Checked Then
            strSql += vbCrLf + "  <= '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        Else
            strSql += vbCrLf + "  BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        End If
        If rbtOrder.Checked Then strSql += vbCrLf + " AND  O.ORTYPE='O'"
        If rbtRepair.Checked Then strSql += vbCrLf + " AND  O.ORTYPE='R'"
        If rbtbooked.Checked Then strSql += vbCrLf + " AND  O.ORTYPE='B'"

        strSql += vbCrLf + "  AND ISNULL(O.ORDCANCEL,'') = ''"
        strSql += vbCrLf + " AND O.COMPANYID = '" & strCompanyId & "'"
        strSql += vbCrLf + " AND ISNULL(O.CANCEL,'') = ''"

        'If rbtOrder.Checked Then
        '    strSql += vbCrLf + " AND O.ORTYPE = 'O'"
        'ElseIf rbtRepair.Checked Then
        '    strSql += vbCrLf + " AND O.ORTYPE = 'R'"
        'End If
        If Not rbtTypeAll.Checked Then
            If rbtTypeCustomer.Checked Then strSql += vbCrLf + "  AND ORMODE = 'C'" Else strSql += vbCrLf + "  AND ORMODE = 'O'"
        End If
        If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
            strSql += vbCrLf + "  AND   O.COSTID IN (SELECT  COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & GetQryString(chkCmbCostCentre.Text) & "))"
        End If
        If chkCmbMetal.Text <> "ALL" And chkCmbMetal.Text <> "" Then
            strSql += vbCrLf + "  AND O.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & GetQryString(chkCmbMetal.Text) & ")))"
        End If
        If chkCmbItem.Text <> "ALL" And chkCmbItem.Text <> "" Then
            strSql += vbCrLf + "  AND O.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & GetQryString(chkCmbItem.Text) & "))"
        End If
        If chkcountercmb.Text <> "ALL" And chkcountercmb.Text <> "" Then
            strSql += vbCrLf + "  AND T.ITEMCTRID IN (SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME IN (" & GetQryString(chkcountercmb.Text) & "))"
        End If
        If cmbEmpName.Text <> "ALL" And cmbEmpName.Text <> "" Then strSql += vbCrLf + " AND O.EMPID = (SELECT EMPID FROM " & cnAdminDb & "..EMPMASTER WHERE EMPNAME = '" & cmbEmpName.Text & "')"
        If rbtCurrentRate.Checked = True Then
            strSql += vbCrLf + " AND O.ORRATE = 'C'"
        ElseIf rbtDeliveryRate.Checked = True Then
            strSql += vbCrLf + " AND O.ORRATE = 'D'"
        End If
        'strSql += vbCrLf + "  AND ISNULL(T.ORDREPNO,'') != '' AND ISNULL(T.ORSNO,'') != ''"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

        If chkCmbDesigner.Text <> "ALL" And chkCmbDesigner.Text <> "" Then
            strSql = " DELETE FROM TEMPTABLEDB..TEMPORSTATUS WHERE"
            strSql += vbCrLf + " isnull(DESIGNER,'') NOT IN (" & GetQryString(chkCmbDesigner.Text) & ")"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
        End If
        'If ChkCmbStatus.Text <> "ALL" And ChkCmbStatus.Text <> "" Then
        'strSql = " DELETE FROM TEMPTABLEDB..TEMPORSTATUS WHERE"
        'strSql += vbCrLf + "  STATUS NOT IN ('DELIVERED')" 'strSql += vbCrLf + "  STATUS NOT IN (" & GetQryString(ChkCmbStatus.Text) & ")"
        'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
        'End If
        If chkCmbDealer.Text <> "ALL" And chkCmbDealer.Text <> "" Then
            strSql = " DELETE FROM TEMPTABLEDB..TEMPORSTATUS WHERE"
            strSql += vbCrLf + "  ACCODE NOT IN (" & GetQryString(chkCmbDealer.Text) & ")"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
        End If


        strSql = " UPDATE TEMPTABLEDB..TEMPORSTATUS SET ADV_WT = NULL,ADV_AMT = NULL"
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMPORSTATUS AS T"
        strSql += vbCrLf + " WHERE ROWID NOT IN (SELECT MIN(ROWID) FROM TEMPTABLEDB..TEMPORSTATUS GROUP BY ORNO)"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

        strSql = " IF (sELECT COUNT(*) FROM TEMPTABLEDB..TEMPORSTATUS)>0"
        strSql += vbCrLf + " BEGIN"
        If rbtEmpName.Checked Then
            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPORSTATUS"
            strSql += vbCrLf + " ("
            strSql += vbCrLf + " EMPNAME,COLHEAD,RESULT"
            strSql += vbCrLf + " )"
            strSql += vbCrLf + " SELECT DISTINCT EMPNAME,'T',0 RESULT FROM TEMPTABLEDB..TEMPORSTATUS WHERE RESULT = 1"
            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPORSTATUS"
            strSql += vbCrLf + " ("
            strSql += vbCrLf + " PARTICULAR,EMPNAME,COLHEAD,RESULT,PCS,GRSWT,NETWT,MCGRM,MC,WASTPER,WAST,STNPCS,STNWT,DIAPCS,DIAWT,ADV_AMT,ADV_WT,ORWT,ADWT,SALEWT,AMOUNT,TAX"
            strSql += vbCrLf + " )"
            strSql += vbCrLf + " SELECT 'SUB TOT',EMPNAME,'S',2 RESULT,SUM(ISNULL(PCS,0)),SUM(ISNULL(GRSWT,0)),SUM(ISNULL(NETWT,0)),SUM(MCGRM),SUM(MC),SUM(WASTPER),SUM(WAST),SUM(STNPCS),SUM(STNWT),SUM(DIAPCS),SUM(DIAWT),SUM(ISNULL(ADV_AMT,0)),SUM(ISNULL(ADV_WT,0)),SUM(ISNULL(ORWT,0)),SUM(ISNULL(ADWT,0)),SUM(ISNULL(SALEWT,0)),SUM(ISNULL(AMOUNT,0)),SUM(ISNULL(TAX,0)) FROM TEMPTABLEDB..TEMPORSTATUS WHERE RESULT = 1 GROUP BY EMPNAME"
        ElseIf rbtCounter.Checked Then
            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPORSTATUS"
            strSql += vbCrLf + " ("
            strSql += vbCrLf + " COUNTER,COLHEAD,RESULT"
            strSql += vbCrLf + " )"
            strSql += vbCrLf + " SELECT DISTINCT COUNTER,'T',0 RESULT FROM TEMPTABLEDB..TEMPORSTATUS WHERE RESULT = 1"
            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPORSTATUS"
            strSql += vbCrLf + " ("
            strSql += vbCrLf + " PARTICULAR,COUNTER,COLHEAD,RESULT,PCS,GRSWT,NETWT,MCGRM,MC,WASTPER,WAST,STNPCS,STNWT,DIAPCS,DIAWT,ADV_AMT,ADV_WT,ORWT,ADWT,SALEWT,AMOUNT,TAX"
            strSql += vbCrLf + " )"
            strSql += vbCrLf + " SELECT 'SUB TOT',COUNTER,'S',2 RESULT,SUM(ISNULL(PCS,0)),SUM(ISNULL(GRSWT,0)),SUM(ISNULL(NETWT,0)),SUM(MCGRM),SUM(MC),SUM(WASTPER),SUM(WAST),SUM(STNPCS),SUM(STNWT),SUM(DIAPCS),SUM(DIAWT),SUM(ISNULL(ADV_AMT,0)),SUM(ISNULL(ADV_WT,0)),SUM(ISNULL(ORWT,0)),SUM(ISNULL(ADWT,0)) ,SUM(ISNULL(SALEWT,0)),SUM(ISNULL(AMOUNT,0)),SUM(ISNULL(TAX,0)) FROM TEMPTABLEDB..TEMPORSTATUS WHERE RESULT = 1 GROUP BY COUNTER"
        ElseIf rbtSmith.Checked Then
            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPORSTATUS"
            strSql += vbCrLf + " ("
            strSql += vbCrLf + " SMITHNAME,COLHEAD,RESULT"
            strSql += vbCrLf + " )"
            strSql += vbCrLf + " SELECT DISTINCT SMITHNAME,'T',0 RESULT FROM TEMPTABLEDB..TEMPORSTATUS WHERE RESULT = 1"
            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPORSTATUS"
            strSql += vbCrLf + " ("
            strSql += vbCrLf + " PARTICULAR,SMITHNAME,COLHEAD,RESULT,PCS,GRSWT,NETWT,MCGRM,MC,WASTPER,WAST,STNPCS,STNWT,DIAPCS,DIAWT,ADV_AMT,ADV_WT,ORWT,ADWT,SALEWT,AMOUNT,TAX"
            strSql += vbCrLf + " )"
            strSql += vbCrLf + " SELECT 'SUB TOT',SMITHNAME,'S',2 RESULT,SUM(ISNULL(PCS,0)),SUM(ISNULL(GRSWT,0)),SUM(ISNULL(NETWT,0)),SUM(MCGRM),SUM(MC),SUM(WASTPER),SUM(WAST),SUM(STNPCS),SUM(STNWT),SUM(DIAPCS),SUM(DIAWT),SUM(ISNULL(ADV_AMT,0)),SUM(ISNULL(ADV_WT,0)),SUM(ISNULL(ORWT,0)),SUM(ISNULL(ADWT,0)),SUM(ISNULL(SALEWT,0)),SUM(ISNULL(AMOUNT,0)),SUM(ISNULL(TAX,0)) FROM TEMPTABLEDB..TEMPORSTATUS WHERE RESULT = 1 GROUP BY SMITHNAME"
        ElseIf rbtWithOutGrpBy.Checked Then
            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPORSTATUS"
            strSql += vbCrLf + " ("
            strSql += vbCrLf + " PARTICULAR,COLHEAD,RESULT,PCS,GRSWT,NETWT,MCGRM,MC,WASTPER,WAST,STNPCS,STNWT,DIAPCS,DIAWT,ADV_AMT,ADV_WT,ORWT,ADWT,SALEWT,AMOUNT,TAX"
            strSql += vbCrLf + " )"
            strSql += vbCrLf + " SELECT 'SUB TOT','S',2 RESULT,SUM(ISNULL(PCS,0)),SUM(ISNULL(GRSWT,0)),SUM(ISNULL(NETWT,0)),SUM(MCGRM),SUM(MC),SUM(WASTPER),SUM(WAST),SUM(STNPCS),SUM(STNWT),SUM(DIAPCS),SUM(DIAWT),SUM(ISNULL(ADV_AMT,0)),SUM(ISNULL(ADV_WT,0)),SUM(ISNULL(ORWT,0)),SUM(ISNULL(ADWT,0)),SUM(ISNULL(SALEWT,0)),SUM(ISNULL(AMOUNT,0)),SUM(ISNULL(TAX,0)) FROM TEMPTABLEDB..TEMPORSTATUS WHERE RESULT = 1 "
        End If

        If rbtCounter.Checked Then
            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPORSTATUS"
            strSql += vbCrLf + " ("
            strSql += vbCrLf + " COUNTER,COLHEAD,GRESULT,PCS,GRSWT,NETWT,MCGRM,MC,WASTPER,WAST,STNPCS,STNWT,DIAPCS,DIAWT,ADV_AMT,ADV_WT,ORWT,ADWT,SALEWT,AMOUNT,TAX"
            strSql += vbCrLf + " )"
            strSql += vbCrLf + " SELECT 'GRAND TOT','G',1 GRESULT,SUM(ISNULL(PCS,0)),SUM(ISNULL(GRSWT,0)),SUM(ISNULL(NETWT,0)),SUM(MCGRM),SUM(MC),SUM(WASTPER),SUM(WAST),SUM(STNPCS),SUM(STNWT),SUM(DIAPCS),SUM(DIAWT),SUM(ISNULL(ADV_AMT,0)),SUM(ISNULL(ADV_WT,0)),SUM(ISNULL(ORWT,0)),SUM(ISNULL(ADWT,0)),SUM(ISNULL(SALEWT,0)),SUM(ISNULL(AMOUNT,0)),SUM(ISNULL(TAX,0)) FROM TEMPTABLEDB..TEMPORSTATUS WHERE RESULT = 1"
            strSql += vbCrLf + " END"
        ElseIf rbtEmpName.Checked Then
            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPORSTATUS"
            strSql += vbCrLf + " ("
            strSql += vbCrLf + " EMPNAME,COLHEAD,GRESULT,PCS,GRSWT,NETWT,MCGRM,MC,WASTPER,WAST,STNPCS,STNWT,DIAPCS,DIAWT,ADV_AMT,ADV_WT,ORWT,ADWT,SALEWT,AMOUNT,TAX"
            strSql += vbCrLf + " )"
            strSql += vbCrLf + " SELECT 'GRAND TOT','G',1 GRESULT,SUM(ISNULL(PCS,0)),SUM(ISNULL(GRSWT,0)),SUM(ISNULL(NETWT,0)),SUM(MCGRM),SUM(MC),SUM(WASTPER),SUM(WAST),SUM(STNPCS),SUM(STNWT),SUM(DIAPCS),SUM(DIAWT),SUM(ISNULL(ADV_AMT,0)),SUM(ISNULL(ADV_WT,0)),SUM(ISNULL(ORWT,0)),SUM(ISNULL(ADWT,0)),SUM(ISNULL(SALEWT,0)),SUM(ISNULL(AMOUNT,0)),SUM(ISNULL(TAX,0)) FROM TEMPTABLEDB..TEMPORSTATUS WHERE RESULT = 1"
            strSql += vbCrLf + " END"
        ElseIf rbtSmith.Checked Then
            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPORSTATUS"
            strSql += vbCrLf + " ("
            strSql += vbCrLf + " SMITHNAME,COLHEAD,GRESULT,PCS,GRSWT,NETWT,MCGRM,MC,WASTPER,WAST,STNPCS,STNWT,DIAPCS,DIAWT,ADV_AMT,ADV_WT,ORWT,ADWT,SALEWT,AMOUNT,TAX"
            strSql += vbCrLf + " )"
            strSql += vbCrLf + " SELECT 'GRAND TOT','G',1 GRESULT,SUM(ISNULL(PCS,0)),SUM(ISNULL(GRSWT,0)),SUM(ISNULL(NETWT,0)),SUM(MCGRM),SUM(MC),SUM(WASTPER),SUM(WAST),SUM(STNPCS),SUM(STNWT),SUM(DIAPCS),SUM(DIAWT),SUM(ISNULL(ADV_AMT,0)),SUM(ISNULL(ADV_WT,0)),SUM(ISNULL(ORWT,0)),SUM(ISNULL(ADWT,0)),SUM(ISNULL(SALEWT,0)),SUM(ISNULL(AMOUNT,0)),SUM(ISNULL(TAX,0)) FROM TEMPTABLEDB..TEMPORSTATUS WHERE RESULT = 1"
            strSql += vbCrLf + " END"
        ElseIf rbtWithOutGrpBy.Checked Then
            strSql += vbCrLf + " END"
        End If

        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        If rbtformat.Checked = True Then
            If rbtCounter.Checked Then
                strSql = " UPDATE TEMPTABLEDB..TEMPORSTATUS SET PARTICULAR = CASE WHEN RESULT = 1 THEN ORNO ELSE COUNTER END WHERE ISNULL(PARTICULAR,'') = ''"
            ElseIf rbtEmpName.Checked Then
                strSql = " UPDATE TEMPTABLEDB..TEMPORSTATUS SET PARTICULAR = CASE WHEN RESULT = 1 THEN ORNO ELSE EMPNAME END WHERE ISNULL(PARTICULAR,'') = ''"
            Else
                strSql = " UPDATE TEMPTABLEDB..TEMPORSTATUS SET PARTICULAR = CASE WHEN RESULT = 1 THEN ORNO ELSE SMITHNAME END WHERE ISNULL(PARTICULAR,'') = ''"
            End If
        Else

            If rbtCounter.Checked Then
                strSql = " UPDATE TEMPTABLEDB..TEMPORSTATUS SET PARTICULAR = CASE WHEN RESULT = 1 THEN CONVERT(VARCHAR(15),SALTRANNO)   ELSE COUNTER END WHERE ISNULL(PARTICULAR,'') = '' /* AND ISNULL(RESULT,'') <> '' */"
            ElseIf rbtEmpName.Checked Then
                strSql = " UPDATE TEMPTABLEDB..TEMPORSTATUS SET PARTICULAR = CASE WHEN RESULT = 1 THEN CONVERT(VARCHAR(15),SALTRANNO)   ELSE EMPNAME END WHERE ISNULL(PARTICULAR,'') = '' /* AND ISNULL(RESULT,'') <> '' */"
            Else
                strSql = " UPDATE TEMPTABLEDB..TEMPORSTATUS SET PARTICULAR = CASE WHEN RESULT = 1 THEN CONVERT(VARCHAR(15),SALTRANNO)   ELSE SMITHNAME END WHERE ISNULL(PARTICULAR,'') = '' /* AND ISNULL(RESULT,'') <> '' */"
            End If
        End If
        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()


        strSql = " SELECT * FROM TEMPTABLEDB..TEMPORSTATUS"
        strSql += vbCrLf + "  ORDER BY GRESULT "
        If rbtEmpName.Checked Then
            strSql += " ,EMPNAME,RESULT"
        ElseIf rbtCounter.Checked Then
            strSql += " ,COUNTER,RESULT"
        ElseIf rbtSmith.Checked Then
            strSql += " ,SMITHNAME,RESULT"
        ElseIf rbtWithOutGrpBy.Checked Then
            strSql += " ,RESULT"
        End If
        If cmbOrderBy.Text = "CUSTOMER WISE" Then
            strSql += " ,PNAME"
        ElseIf cmbOrderBy.Text = "DESIGNER WISE" Then
            strSql += " ,DESIGNER"
        ElseIf cmbOrderBy.Text = "ORDER NO" Then
            If rbtOrder.Checked Then
                strSql += " ,CONVERT(INT,REPLACE(ORNO,'O',''))"
            ElseIf rbtRepair.Checked Then
                strSql += " ,CONVERT(INT,REPLACE(ORNO,'R',''))"
            Else
                strSql += " ,CONVERT(INT,REPLACE(ORNO,'B',''))"
            End If
        End If
        strSql += " ,ORDATE,ORNO"

        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("KEYNO", GetType(Integer))
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            dtpFrom.Select()
            Exit Sub
        End If

        dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        'objGridShower.Size = New Size(778, 550)
        Dim tit As String = ""
        If READYITEMBOOKING Then
            objGridShower.Text = "ORDER/REPAIR DELIVERED REPORT"
            tit += "ORDER/REPAIR DELIVERED  REPORT "
        Else
            objGridShower.Text = "ORDER/REPAIR/BOOKED DELIVERED REPORT"
            tit += "ORDER/REPAIR/BOOKED DELIVERED  REPORT "
        End If
        'If rbtBoth.Checked Then
        '    tit += "ORDER & REPAIR STATUS REPORT" + vbCrLf
        'ElseIf rbtOrder.Checked Then
        '    tit += "ORDER STATUS REPORT" + vbCrLf
        'Else
        '    tit += "REPAIR STATUS REPORT" + vbCrLf
        'End If

        If chkAsOnDate.Checked Then
            tit += " AS ONDATE " & dtpFrom.Text
        Else
            tit += "FROM " + dtpFrom.Text + " TO " + dtpTo.Text
        End If
        tit += IIf(chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "", " :" & chkCmbCostCentre.Text, "")
        objGridShower.lblTitle.Text = tit
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(dtGrid)
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)

        objGridShower.FormReSize = True
        objGridShower.FormReLocation = False
        objGridShower.pnlFooter.Visible = False
        objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        Ddv_OrderStatusFormatting(objGridShower.gridView)
        FormatGridColumns(objGridShower.gridView, False, False, , False)
        objGridShower.Show()
        objGridShower.FormReSize = True
        objGridShower.gridViewHeader.Visible = False
        objGridShower.pnlFooter.Visible = False
        AddHandler objGridShower.gridView.KeyPress, AddressOf GridView_KeyPress
        FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
        'GridViewFormat()
        objGridShower.gridView.Columns("DIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
    End Sub
    Private Sub Ddv_OrderStatusFormatting(ByVal dgv As DataGridView)
        With dgv
            For cnt As Integer = 25 To dgv.ColumnCount - 1
                .Columns(cnt).Visible = False
            Next
            .Columns("ORWT").Visible = False
            .Columns("SALEWT").Visible = False
            .Columns("ADWT").Visible = False
            .Columns("DESIGNER").Width = 100
            .Columns("DESIGNER").Visible = True
            '.Columns("SMITHNAME").Width = 100
            '.Columns("SMITHNAME").Visible = IIf(rbtSmith.Checked, False, True)
            .Columns("SMITHNAME").Visible = False
            '.Columns("SMITH ISS NO").Visible = False
            .Columns("PNAME").Visible = True
            .Columns("PNAME").Width = 100
            .Columns("PNAME").HeaderText = "PARTY NAME"
            .Columns("MCGRM").HeaderText = "MC/Gm"
            .Columns("WASTPER").HeaderText = "W%"
            .Columns("WAST").HeaderText = "W/Gm"
            .Columns("PARTICULAR").Width = 200
            .Columns("ORNO").Visible = False
            .Columns("ORSNO").Visible = False
            '.Columns("ORDATE").Width = 80
            '.Columns("REMDATE").Width = 80
            '.Columns("DUEDATE").Width = 80
            .Columns("ORDATE").Visible = True
            .Columns("REMDATE").Visible = False
            .Columns("DUEDATE").Visible = False
            '.Columns("STYLENO").Width = 80
            .Columns("STYLENO").Visible = False
            .Columns("DESCRIPT").Width = 230
            .Columns("DESCRIPT").HeaderText = "ITEM NAME"
            .Columns("PCS").Width = 60
            .Columns("GRSWT").Width = 80
            .Columns("NETWT").Width = 80
            .Columns("MCGRM").Width = 80
            .Columns("WASTPER").Width = 80
            .Columns("WAST").Width = 80
            .Columns("STNPCS").Width = 60
            .Columns("STNWT").Width = 80
            .Columns("DIAPCS").Width = 60
            .Columns("DIAWT").Width = 80
            .Columns("SIZENO").Width = 60
            .Columns("SIZENO").Visible = False
            '.Columns("STATUS").Width = 150
            .Columns("STATUS").Visible = False
            .Columns("PNAME").Width = 200
            .Columns("COSTNAME").Width = 150
            '.Columns("ORRATE").Width = 70
            .Columns("ORRATE").Visible = False
            '.Columns("ADV_WT").Width = 70
            '.Columns("ADV_AMT").Width = 70
            .Columns("TRANNO").Width = 70
            .Columns("DELIVERYNO").Width = 90
            .Columns("DELIVERYNO").Visible = True
            .Columns("DELIVERYDATE").Width = 120
            .Columns("DELIVERYDATE").Visible = True
            '.Columns("TRANNO").HeaderText = "SMITH" + vbCrLf + "ISS NO"
            .Columns("TRANNO").Visible = False
            .Columns("COUNTER").Visible = IIf(rbtCounter.Checked, False, True)
            .Columns("COUNTER").Width = 100
            .Columns("DIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
            '.Columns("ORDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
            '.Columns("REMDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
            '.Columns("DUEDATE").DefaultCellStyle.Format = "dd/MM/yyyy"

            'If rbtOrder.Checked Then
            '    .Columns("ORDATE").HeaderText = "ORDATE"
            'ElseIf rbtRepair.Checked Then
            '    .Columns("ORDATE").HeaderText = "REPDATE"
            'Else
            '    .Columns("ORDATE").HeaderText = "OR_REP DATE"
            'End If
            For cnt As Integer = 0 To dgv.ColumnCount - 1
                .Columns(cnt).SortMode = DataGridViewColumnSortMode.Automatic
            Next
            .Columns("ADV_AMT").Visible = False
            .Columns("ADV_WT").Visible = False
            '.Columns("ADV_AMT").HeaderText = "ADV AMT"
            '.Columns("ADV_WT").HeaderText = "ADV WT"
        End With
        If rbtformat2.Checked = True Then
            With dgv
                For cnt As Integer = 0 To dgv.ColumnCount - 1
                    .Columns(cnt).Visible = False
                Next
                .Columns("PARTICULAR").Visible = True
                .Columns("ORWT").Visible = True
                .Columns("ORWT").HeaderText = "ORDER WT"
                .Columns("SALEWT").Visible = True
                .Columns("SALEWT").HeaderText = "SALES WT"
                .Columns("ADWT").Visible = True
                .Columns("ADWT").HeaderText = "ADVANCE WT"
                .Columns("PNAME").Visible = True
                .Columns("ORNO").Visible = True
            End With
        End If
    End Sub
    Function GridViewFormat() As Integer
        For Each dgvRow As DataGridViewRow In objGridShower.gridView.Rows
            With dgvRow
                Select Case .Cells("STATUS").Value.ToString
                    Case "BOOKED"
                        .DefaultCellStyle.BackColor = Color.AliceBlue
                        .DefaultCellStyle.Font = reportTotalStyle.Font
                    Case "ISSUE TO SMITH"
                        .DefaultCellStyle.BackColor = Color.LightPink
                        .DefaultCellStyle.Font = reportTotalStyle.Font
                    Case "RECEIVE FROM SMITH"
                        .DefaultCellStyle.BackColor = Color.RosyBrown
                        .DefaultCellStyle.Font = reportTotalStyle.Font
                    Case "READY FOR DELIVERY"
                        .DefaultCellStyle.BackColor = Color.Orange
                        .DefaultCellStyle.Font = reportTotalStyle.Font
                    Case "DELIVERED"
                        .DefaultCellStyle.BackColor = Color.LightGreen
                        .DefaultCellStyle.Font = reportTotalStyle.Font
                    Case "CANCELLED"
                        .DefaultCellStyle.BackColor = Color.Red
                        .DefaultCellStyle.Font = reportTotalStyle.Font
                    Case "PENDING WITH US"
                        .DefaultCellStyle.BackColor = Color.Wheat
                        .DefaultCellStyle.Font = reportTotalStyle.Font
                    Case "APPROVAL ISSUE"
                        .DefaultCellStyle.BackColor = Color.LightSkyBlue
                        .DefaultCellStyle.Font = reportTotalStyle.Font
                End Select
            End With
        Next
    End Function
    Private Sub chkAsOnDate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAsOnDate.CheckedChanged
        If chkAsOnDate.Checked Then
            chkAsOnDate.Text = "As OnDate"
            Label2.Visible = False
            dtpTo.Visible = False
        Else
            chkAsOnDate.Text = "Date From"
            Label2.Visible = True
            dtpTo.Visible = True
        End If
    End Sub


    Private Sub Prop_Sets()
        Dim obj As New frmOrderDeliveredReport_Properties
        obj.p_chkAsOnDate = chkAsOnDate.Checked
        obj.p_rbtOrderDate = rbtOrderDate.Checked
        obj.p_rbtDueDate = rbtDueDate.Checked
        obj.p_rbtTypeAll = rbtTypeAll.Checked
        obj.p_rbtTypeCompany = rbtTypeCompany.Checked
        obj.p_rbtTypeCustomer = rbtTypeCustomer.Checked
        'GetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre)
        GetChecked_CheckedList(chkCmbMetal, obj.p_chkCmbMetal)
        GetChecked_CheckedList(chkCmbItem, obj.p_chkCmbItem)
        GetChecked_CheckedList(chkCmbDesigner, obj.p_chkCmbDesigner)
        obj.p_cmbEmpName = cmbEmpName.Text
        obj.p_txtCustomerName = txtCustomerName.Text
        obj.p_cmbOrderBy = cmbOrderBy.Text
        'GetChecked_CheckedList(ChkCmbStatus, obj.p_ChkCmbStatus)
        obj.p_rbtEmpName = rbtEmpName.Checked
        obj.p_rbtCounter = rbtCounter.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmOrderDeliveredReport_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmOrderDeliveredReport_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmOrderDeliveredReport_Properties))
        chkAsOnDate.Checked = obj.p_chkAsOnDate
        rbtOrderDate.Checked = obj.p_rbtOrderDate
        rbtDueDate.Checked = obj.p_rbtDueDate
        rbtTypeAll.Checked = obj.p_rbtTypeAll
        rbtTypeCompany.Checked = obj.p_rbtTypeCompany
        rbtTypeCustomer.Checked = obj.p_rbtTypeCustomer
        'SetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre, "ALL")
        SetChecked_CheckedList(chkCmbMetal, obj.p_chkCmbMetal, "ALL")
        SetChecked_CheckedList(chkCmbItem, obj.p_chkCmbItem, "ALL")
        SetChecked_CheckedList(chkCmbDesigner, obj.p_chkCmbDesigner, "ALL")
        cmbEmpName.Text = obj.p_cmbEmpName
        txtCustomerName.Text = obj.p_txtCustomerName
        cmbOrderBy.Text = obj.p_cmbOrderBy
        rbtEmpName.Checked = obj.p_rbtEmpName
        rbtCounter.Checked = obj.p_rbtCounter
    End Sub

    Private Sub GridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If UCase(e.KeyChar) = "D" Then
            If GetAdmindbSoftValue("PRN_ORIR", "N") = "Y" Then
                Dim prnmemsuffix As String = ""
                Dim pbatchno As String
                Dim pbilldate As Date
                Dim dgv As DataGridView = CType(sender, DataGridView)
                If dgv.CurrentRow Is Nothing Then Exit Sub
                If dgv.CurrentRow.Cells("BATCHNO").Value.ToString = "" Then Exit Sub
                pbatchno = dgv.CurrentRow.Cells("BATCHNO").Value.ToString
                pbilldate = dgv.CurrentRow.Cells("ORDATE").Value.ToString
                If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId

                If IO.File.Exists(Application.StartupPath & "\BillPrint.exe") Then
                    Dim write As IO.StreamWriter
                    Dim memfile As String = "\BillPrint" & prnmemsuffix.Trim & ".mem"
                    write = IO.File.CreateText(Application.StartupPath & memfile)
                    write.WriteLine(LSet("TYPE", 15) & ":OIR")
                    write.WriteLine(LSet("BATCHNO", 15) & ":" & pbatchno)
                    write.WriteLine(LSet("TRANDATE", 15) & ":" & pbilldate.ToString("yyyy-MM-dd"))
                    write.WriteLine(LSet("DUPLICATE", 15) & ":N")
                    write.Flush()
                    write.Close()
                    If EXE_WITH_PARAM = False Then
                        System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe")
                    Else
                        System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe",
                        LSet("TYPE", 15) & ":OIR" & ";" &
                        LSet("BATCHNO", 15) & ":" & pbatchno & ";" &
                        LSet("TRANDATE", 15) & ":" & pbilldate.ToString("yyyy-MM-dd") & ";" &
                        LSet("DUPLICATE", 15) & ":N")
                    End If
                Else
                    MsgBox("Billprint exe not found", MsgBoxStyle.Information)
                End If
            End If
        End If
    End Sub

    Private Sub rbtRepair_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtRepair.CheckedChanged
        If rbtRepair.Checked Then rbtOrderDate.Text = "Repair Date"
        getorderby()
    End Sub

    Private Sub rbtOrder_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtOrder.CheckedChanged
        If rbtOrder.Checked Then rbtOrderDate.Text = "Order Date"
        getorderby()
    End Sub

    Private Sub rbtbooked_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtbooked.CheckedChanged
        If rbtbooked.Checked Then rbtOrderDate.Text = "Booked Date"
        getorderby()
    End Sub
End Class

Public Class frmOrderDeliveredReport_Properties

    Private rbtEmpName As Boolean = True
    Public Property p_rbtEmpName() As Boolean
        Get
            Return rbtEmpName
        End Get
        Set(ByVal value As Boolean)
            rbtEmpName = value
        End Set
    End Property

    Private rbtCounter As Boolean
    Public Property p_rbtCounter() As Boolean
        Get
            Return rbtCounter
        End Get
        Set(ByVal value As Boolean)
            rbtCounter = value
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
    Private rbtOrder As Boolean = False
    Public Property p_rbtOrder() As Boolean
        Get
            Return rbtOrder
        End Get
        Set(ByVal value As Boolean)
            rbtOrder = value
        End Set
    End Property
    Private rbtRepair As Boolean = False
    Public Property p_rbtRepair() As Boolean
        Get
            Return rbtRepair
        End Get
        Set(ByVal value As Boolean)
            rbtRepair = value
        End Set
    End Property
    Private rbtBooked As Boolean = False
    Public Property p_rbtBooked() As Boolean
        Get
            Return rbtBooked
        End Get
        Set(ByVal value As Boolean)
            rbtBooked = value
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
    Private rbtOrderDate As Boolean = True
    Public Property p_rbtOrderDate() As Boolean
        Get
            Return rbtOrderDate
        End Get
        Set(ByVal value As Boolean)
            rbtOrderDate = value
        End Set
    End Property
    Private rbtDueDate As Boolean = False
    Public Property p_rbtDueDate() As Boolean
        Get
            Return rbtDueDate
        End Get
        Set(ByVal value As Boolean)
            rbtDueDate = value
        End Set
    End Property
    Private rbtTypeAll As Boolean = True
    Public Property p_rbtTypeAll() As Boolean
        Get
            Return rbtTypeAll
        End Get
        Set(ByVal value As Boolean)
            rbtTypeAll = value
        End Set
    End Property
    Private rbtTypeCompany As Boolean = False
    Public Property p_rbtTypeCompany() As Boolean
        Get
            Return rbtTypeCompany
        End Get
        Set(ByVal value As Boolean)
            rbtTypeCompany = value
        End Set
    End Property
    Private rbtTypeCustomer As Boolean = False
    Public Property p_rbtTypeCustomer() As Boolean
        Get
            Return rbtTypeCustomer
        End Get
        Set(ByVal value As Boolean)
            rbtTypeCustomer = value
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
    Private chkCmbMetal As New List(Of String)
    Public Property p_chkCmbMetal() As List(Of String)
        Get
            Return chkCmbMetal
        End Get
        Set(ByVal value As List(Of String))
            chkCmbMetal = value
        End Set
    End Property
    Private chkCmbItem As New List(Of String)
    Public Property p_chkCmbItem() As List(Of String)
        Get
            Return chkCmbItem
        End Get
        Set(ByVal value As List(Of String))
            chkCmbItem = value
        End Set
    End Property
    Private chkCmbDesigner As New List(Of String)
    Public Property p_chkCmbDesigner() As List(Of String)
        Get
            Return chkCmbDesigner
        End Get
        Set(ByVal value As List(Of String))
            chkCmbDesigner = value
        End Set
    End Property
    Private cmbEmpName As String = "ALL"
    Public Property p_cmbEmpName() As String
        Get
            Return cmbEmpName
        End Get
        Set(ByVal value As String)
            cmbEmpName = value
        End Set
    End Property
    Private cmbOrderBy As String = "ORDER DATE"
    Public Property p_cmbOrderBy() As String
        Get
            Return cmbOrderBy
        End Get
        Set(ByVal value As String)
            cmbOrderBy = value
        End Set
    End Property
    Private txtCustomerName As String = ""
    Public Property p_txtCustomerName() As String
        Get
            Return txtCustomerName
        End Get
        Set(ByVal value As String)
            txtCustomerName = value
        End Set
    End Property
    Private ChkCmbStatus As New List(Of String)
    Public Property p_ChkCmbStatus() As List(Of String)
        Get
            Return ChkCmbStatus
        End Get
        Set(ByVal value As List(Of String))
            ChkCmbStatus = value
        End Set
    End Property

    Private chkGroupByEmpName As Boolean = True
    Public Property p_chkGroupByEmpName() As Boolean
        Get
            Return chkGroupByEmpName
        End Get
        Set(ByVal value As Boolean)
            chkGroupByEmpName = value
        End Set
    End Property
End Class