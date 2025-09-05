Imports System.Data.OleDb
Public Class NonTagIssRecSyncRpt
    Dim objGridShower As frmGridDispDia
    Dim objGridShower1 As frmGridDispDia
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim f_costname As String
    Dim t_costname As String

    Private Sub NonTagIssRecSyncRpt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub NonTagIssRecSyncRpt_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GrpContainer.Location = New Point((ScreenWid - GrpContainer.Width) / 2, ((ScreenHit - 128) - GrpContainer.Height) / 2)
        cmbItemName.Items.Clear()
        cmbItemName.Items.Add("ALL")
        strSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST"
        strSql += " WHERE ACTIVE = 'Y'"
        strSql += " AND STOCKTYPE = 'T'"
        objGPack.FillCombo(strSql, cmbItemName, False, False)

        CmbItemCounter.Items.Clear()
        CmbItemCounter.Items.Add("ALL")
        strSql = " SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER"
        strSql += " WHERE ACTIVE = 'Y'"
        objGPack.FillCombo(strSql, CmbItemCounter, False, False)
        CmbItemCounter.Text = "ALL"


        ''Designer
        cmbDesigner.Items.Clear()
        cmbDesigner.Items.Add("ALL")
        strSql = " SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER ORDER BY DESIGNERNAME"
        objGPack.FillCombo(strSql, cmbDesigner, False, False)
        cmbDesigner.Text = "ALL"

        cmbMetal.Items.Add("ALL")
        strSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST ORDER BY METALNAME"
        objGPack.FillCombo(strSql, cmbMetal, False)
        cmbMetal.Text = "ALL"

        cmbCategory.Items.Clear()
        cmbCategory.Items.Add("ALL")
        strSql = " SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY"
        strSql += " WHERE ACTIVE = 'Y'"
        objGPack.FillCombo(strSql, cmbCategory, False, False)
        cmbCategory.Text = "ALL"

        cmbFromCostCenter.Items.Clear()
        strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += " WHERE ACTIVE = 'Y'"
        objGPack.FillCombo(strSql, cmbFromCostCenter, False, False)
        cmbFromCostCenter.Text = cnCostName

        cmbtoCostCenter.Items.Clear()
        strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += " WHERE ACTIVE = 'Y'"
        objGPack.FillCombo(strSql, cmbtoCostCenter, False, False)
        cmbtoCostCenter.Text = ""

        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub chkAsOnDate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAsOnDate.CheckedChanged
        If chkAsOnDate.Checked Then
            Label1.Visible = False
            chkAsOnDate.Text = "As OnDate"
            dtpTo.Visible = False
        Else
            chkAsOnDate.Text = "Date From"
            Label1.Visible = True
            dtpTo.Visible = True
        End If
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        cmbOrderBy.Items.Clear()
        cmbOrderBy.Items.Add("TAGNO")
        cmbOrderBy.Items.Add("COSTCENTRE")
        chkAsOnDate.Select()
        Prop_Gets()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim chkTCostId As String = GetSqlValue(cn, "SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME ='" & cmbtoCostCenter.Text & "'")
        Dim chkFCostId As String = GetSqlValue(cn, "SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME ='" & cmbFromCostCenter.Text & "'")
        Dim dtGrid As New DataTable
        strSql = vbCrLf + " DECLARE @FROMDATE SMALLDATETIME"
        strSql += vbCrLf + " DECLARE @TODATE SMALLDATETIME"
        strSql += vbCrLf + " DECLARE @TRANINVNO VARCHAR(25)"
        strSql += vbCrLf + " SET @FROMDATE = '" & IIf(chkAsOnDate.Checked, _AsOnFromDate.ToString("yyyy-MM-dd"), dtpFrom.Value.ToString("yyyy-MM-dd")) & "'"
        strSql += vbCrLf + " SET @TODATE = '" & IIf(chkAsOnDate.Checked, dtpFrom.Value.ToString("yyyy-MM-dd"), dtpTo.Value.ToString("yyyy-MM-dd")) & "'"
        strSql += vbCrLf + " SET @TRANINVNO = '" & txtrefNo.Text & "'"
        strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_SYNCISSREC') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_SYNCISSREC"
        strSql += vbCrLf + " SELECT "
        strSql += vbCrLf + " CONVERT(VARCHAR(200),NULL)PARTICULAR"
        strSql += vbCrLf + " ,IFC.COSTNAME [ISSUED FROM]"
        strSql += vbCrLf + " ,ITC.COSTNAME [ISSUED TO]"
        strSql += vbCrLf + " ,IM.ITEMNAME,SM.SUBITEMNAME,NULL TAGNO,T.PCS,T.GRSWT,T.NETWT"
        strSql += vbCrLf + " ,NULL AS DIAPCS"
        strSql += vbCrLf + " ,NULL AS DIAWT"
        strSql += vbCrLf + " ,NULL AS STNPCS"
        strSql += vbCrLf + " ,NULL AS STNWT"
        strSql += vbCrLf + " ,CASE WHEN T.WASTPER=0 THEN NULL ELSE T.WASTPER END AS WASTPER  "
        strSql += vbCrLf + " ,CASE WHEN T.WASTAGE=0 THEN NULL ELSE T.WASTAGE END AS WASTAGE"
        strSql += vbCrLf + " ,0  MCGRM "
        strSql += vbCrLf + " ,CASE WHEN T.MC=0 THEN NULL ELSE T.MC END AS MC"
        strSql += vbCrLf + " ,T.RATE,NULL SALVALUE"
        strSql += vbCrLf + " ,NULL TABLECODE"
        strSql += vbCrLf + " ,(SELECT TOP 1 LOTNO FROM " & cnAdminDb & "..ITEMLOT L WHERE L.SNO=T.LOTSNO)LOTNO"
        strSql += vbCrLf + " ,(SELECT NAME FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID=T.ITEMTYPEID)ITEMTYPE,"
        strSql += vbCrLf + " NULL [CLARITY]"
        strSql += vbCrLf + " ,CASE WHEN STKTYPE='M' THEN 'MANUFACTURING' WHEN STKTYPE='E' THEN 'EXEMPTED' ELSE 'TRADING' END AS STOCKTYPE"
        strSql += vbCrLf + " ,(SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = T.DESIGNERID) DESIGNERNAME"
        strSql += vbCrLf + " , NULL TAGVAL/*,TAG.TRANINVNO REFNO*/,CASE WHEN ISNULL(T.REFNO,'') <>'' THEN T.REFNO ELSE NULL END REFNO "
        strSql += vbCrLf + " ,CONVERT(VARCHAR(200),NULL)GROUP1"
        strSql += vbCrLf + " ,CONVERT(VARCHAR(200),NULL)GROUP2"
        strSql += vbCrLf + " ,CONVERT(VARCHAR(3),NULL)COLHEAD"
        strSql += vbCrLf + " ,CONVERT(INT,1)RESULT"
        strSql += vbCrLf + " ,T.RECDATE AS [ISSUED DATE]"
        strSql += vbCrLf + " ,'TRANSFER' FLAG"
        strSql += vbCrLf + " , CNT.ITEMCTRNAME AS COUNTERNAME "
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP_SYNCISSREC"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMNONTAG T"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
        If cmbCategory.Text <> "ALL" And cmbCategory.Text <> "" Then
            strSql += " AND IM.CATCODE = (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & cmbCategory.Text & "')"
        End If
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..COSTCENTRE AS IFC ON IFC.COSTID = '" & chkFCostId & "' "
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..COSTCENTRE AS ITC ON ITC.COSTID = '" & chkTCostId & "' "
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.SUBITEMID = T.SUBITEMID AND SM.ITEMID=T.ITEMID"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMCOUNTER AS CNT ON CNT.ITEMCTRID = T.ITEMCTRID "
        strSql += vbCrLf + " WHERE 1=1 "
        strSql += vbCrLf + " AND T.RECDATE BETWEEN @FROMDATE AND @TODATE "
        If txtrefNo.Text <> "" Then
            strSql += vbCrLf + " AND REFNO=@TRANINVNO "
        End If
        If cmbItemName.Text <> "ALL" And cmbItemName.Text <> "" Then
            strSql += " AND T.ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName.Text & "')"
        End If
        strSql += vbCrLf + " AND T.RECISS = '" & IIf(rbtIssue.Checked, "I", "R") & "'  "
        strSql += vbCrLf + " AND T.ISSTYPE ='TR'"
        strSql += vbCrLf + " AND T.COSTID IN('" & chkFCostId & "')"
        strSql += vbCrLf + " AND T.TCOSTID IN('" & chkTCostId & "')"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_SYNCISSREC_COUNTER_NON') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_SYNCISSREC_COUNTER_NON"
        strSql += vbCrLf + " SELECT PARTICULAR,ITEMNAME,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT, [ISSUED FROM],[ISSUED TO]"
        strSql += vbCrLf + " ,SUM(DIAPCS)DIAPCS,SUM(DIAWT)DIAWT,SUM(STNPCS)STNPCS,SUM(STNWT)STNWT,GROUP1,GROUP2,COLHEAD,RESULT"
        strSql += vbCrLf + " ,COUNTERNAME,NULL WASTAGE,NULL MC,NULL STOCKTYPE"
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP_SYNCISSREC_COUNTER_NON "
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP_SYNCISSREC "
        strSql += vbCrLf + " GROUP BY ITEMNAME ,COUNTERNAME,PARTICULAR,GROUP1,GROUP2,COLHEAD,RESULT,[ISSUED FROM],[ISSUED TO]"
        strSql += vbCrLf + " ORDER BY COUNTERNAME"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()



        strSql = vbCrLf + ""
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP_SYNCISSREC_COUNTER_NON SET PARTICULAR = ITEMNAME,GROUP1 = [ISSUED FROM] ,GROUP2 = ' '+COUNTERNAME"
        strSql += vbCrLf + " "
        strSql += vbCrLf + " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP_SYNCISSREC_COUNTER_NON)>0"
        strSql += vbCrLf + " BEGIN"
        strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP_SYNCISSREC_COUNTER_NON(PARTICULAR,GROUP1,COLHEAD,RESULT,STOCKTYPE)"
        strSql += vbCrLf + " SELECT DISTINCT GROUP1,GROUP1,'T',0 RESULT,'' FROM TEMPTABLEDB..TEMP_SYNCISSREC_COUNTER_NON WHERE RESULT = 1"
        strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP_SYNCISSREC_COUNTER_NON(PARTICULAR,GROUP1,GROUP2,COLHEAD,RESULT,STOCKTYPE)"
        strSql += vbCrLf + " SELECT DISTINCT GROUP2,GROUP1,GROUP2,'T1',0,'' RESULT FROM TEMPTABLEDB..TEMP_SYNCISSREC_COUNTER_NON WHERE RESULT = 1"
        strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP_SYNCISSREC_COUNTER_NON(PARTICULAR,GROUP1,GROUP2,COLHEAD,RESULT,PCS,GRSWT,NETWT,DIAPCS,DIAWT,STNPCS,STNWT)"
        strSql += vbCrLf + " SELECT GROUP2 + ' TOTAL',GROUP1,GROUP2,'S1',2 RESULT,SUM(PCS),SUM(GRSWT),SUM(NETWT),SUM(DIAPCS),SUM(DIAWT),SUM(STNPCS),SUM(STNWT) FROM TEMPTABLEDB..TEMP_SYNCISSREC_COUNTER_NON WHERE RESULT = 1 GROUP BY GROUP2,GROUP1"
        strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP_SYNCISSREC_COUNTER_NON(PARTICULAR,GROUP1,GROUP2,COLHEAD,RESULT,PCS,GRSWT,NETWT,DIAPCS,DIAWT,STNPCS,STNWT)"
        strSql += vbCrLf + " SELECT GROUP1 + ' TOTAL',GROUP1,'ZZZ'GROUP2,'S',2 RESULT,SUM(PCS),SUM(GRSWT),SUM(NETWT),SUM(DIAPCS),SUM(DIAWT),SUM(STNPCS),SUM(STNWT) FROM TEMPTABLEDB..TEMP_SYNCISSREC_COUNTER_NON WHERE RESULT = 1 GROUP BY GROUP1"
        strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP_SYNCISSREC_COUNTER_NON(PARTICULAR,GROUP1,GROUP2,COLHEAD,RESULT,PCS,GRSWT,NETWT,DIAPCS,DIAWT,STNPCS,STNWT)"
        strSql += vbCrLf + " SELECT 'GRAND TOTAL','ZZZZ'GROUP1,'ZZZ'GROUP2,'G',3 RESULT,SUM(PCS),SUM(GRSWT),SUM(NETWT),SUM(DIAPCS),SUM(DIAWT),SUM(STNPCS),SUM(STNWT) FROM TEMPTABLEDB..TEMP_SYNCISSREC_COUNTER_NON WHERE RESULT = 1"
        strSql += vbCrLf + " END"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + " SELECT * FROM TEMPTABLEDB..TEMP_SYNCISSREC_COUNTER_NON"
        strSql += vbCrLf + " ORDER BY GROUP1,GROUP2,RESULT"
            If cmbOrderBy.Text = "COSTCENTRE" Then strSql += vbCrLf + ",[ISSUED TO]"

        dtGrid.Columns.Add("KEYNO", GetType(Integer))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        objGridShower.Text = "NON TAG ITEM WISE TRANSFER ISSUE/RECEIPT"


        Dim tit As String = ""
        If cmbItemName.Text <> "ALL" And cmbItemName.Text <> "" Then
            tit += cmbItemName.Text + " - "
        End If
        tit += "NON TAG ITEM WISE TRANSFER " + IIf(rbtIssue.Checked, "ISSUE", "RECEIPT")


        If chkAsOnDate.Checked Then
            tit += " AS ON " & dtpFrom.Text & ""
        Else
            tit += " DATE FROM " & dtpFrom.Text + " TO " + dtpTo.Text
        End If
        'tit += vbCrLf + " FOR " & cmbFromCostCenter.Text

        If rbtIssue.Checked Then
            tit += vbCrLf + "Issued From " & cmbFromCostCenter.Text
            tit += " To " & cmbtoCostCenter.Text
        Else
            tit += vbCrLf + "Received From " & cmbtoCostCenter.Text
            tit += " To " & cmbFromCostCenter.Text
        End If

        If CmbItemCounter.Text <> "ALL" Then
            tit += " [ " + CmbItemCounter.Text + " ] "
        End If

        objGridShower.lblTitle.Text = tit
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(dtGrid)
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        objGridShower.FormReSize = False
        objGridShower.FormReLocation = False
        'objGridShower.pnlFooter.Visible = False
        'objGridShower.pnlFooter.Visible = True
        'objGridShower.lblStatus.Text = "<Press [D] for Stone / Diamond Detail View>"
        AddHandler objGridShower.gridView.KeyDown, AddressOf DataGridView_KeyDown
        objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        DataGridView_SummaryFormatting(objGridShower.gridView)
        FormatGridColumns(objGridShower.gridView, False, False, , False)
        objGridShower.gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        objGridShower.Show()
        objGridShower.gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        objGridShower.FormReSize = True
        objGridShower.FormReLocation = True
        FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
        objGridShower.gridView_ColumnWidthChanged(Me, New DataGridViewColumnEventArgs(objGridShower.gridView.Columns("PARTICULAR")))
        Prop_Sets()
    End Sub

    Private Sub DataGridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = Keys.D Then
            If objGridShower.gridView.Rows.Count > 0 Then
                Dim dtGrid1 As New DataTable
                Dim tagno As String = objGridShower.gridView.CurrentRow.Cells("TAGNO").Value.ToString
                Dim ItemName As String = objGridShower.gridView.CurrentRow.Cells("ITEMNAME").Value.ToString
                If tagno.ToString = "" Then Exit Sub
                strSql = vbCrLf + " SELECT (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = STNITEMID) STNITEMNAME"
                strSql += vbCrLf + " ,STNPCS"
                strSql += vbCrLf + " ,STNWT"
                strSql += vbCrLf + " ,STNRATE"
                strSql += vbCrLf + " ,STNAMT"
                strSql += vbCrLf + " ,DESCRIP"
                strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO = '" & tagno & "'"
                strSql += vbCrLf + " AND ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & ItemName & "'))"
                dtGrid1.Columns.Add("KEYNO", GetType(Integer))
                dtGrid1.Columns("KEYNO").AutoIncrement = True
                dtGrid1.Columns("KEYNO").AutoIncrementSeed = 0
                dtGrid1.Columns("KEYNO").AutoIncrementStep = 1
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtGrid1)
                dtGrid1.Columns("KEYNO").SetOrdinal(dtGrid1.Columns.Count - 1)
                If Not dtGrid1.Rows.Count > 0 Then
                    MsgBox("Record not found", MsgBoxStyle.Information)
                    Exit Sub
                End If
                objGridShower1 = New frmGridDispDia
                objGridShower1.Name = Me.Name
                objGridShower1.gridView.RowTemplate.Height = 21
                objGridShower1.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
                objGridShower1.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                objGridShower1.Text = "STONE / DIAMOND DETAILS"
                Dim tit As String = ""
                tit = " STONE / DIAMOND DETAILS : " & ItemName & " / TAGNO : " & tagno
                objGridShower1.lblTitle.Text = tit
                objGridShower1.StartPosition = FormStartPosition.CenterScreen
                objGridShower1.dsGrid.DataSetName = objGridShower1.Name
                objGridShower1.dsGrid.Tables.Add(dtGrid1)
                objGridShower1.gridView.DataSource = objGridShower1.dsGrid.Tables(0)
                objGridShower1.FormReSize = False
                objGridShower1.FormReLocation = False
                objGridShower1.pnlFooter.Visible = False
                objGridShower1.gridViewHeader.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                'DataGridView_SummaryFormatting(objGridShower1.gridView)
                FormatGridColumns(objGridShower1.gridView, False, False, , False)
                objGridShower1.gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
                objGridShower1.Show()
                objGridShower1.gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
                objGridShower1.FormReSize = True
                objGridShower1.FormReLocation = True
            End If
        End If
    End Sub


    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView)
        With dgv
            For CNT As Integer = 0 To .ColumnCount - 1
                .Columns(CNT).Visible = True
            Next
            For CNT As Integer = 20 To .ColumnCount - 1
                .Columns(CNT).Visible = False
            Next
            .Columns("ITEMNAME").Visible = False
            .Columns("COUNTERNAME").Visible = False
            .Columns("WASTAGE").Visible = False
            .Columns("STOCKTYPE").Visible = False
            .Columns("DIAPCS").Visible = False
            .Columns("DIAWT").Visible = False
            .Columns("STNPCS").Visible = False
            .Columns("STNWT").Visible = False
            .Columns("GROUP1").Visible = False
            .Columns("GROUP2").Visible = False
            .Columns("COLHEAD").Visible = False
            .Columns("RESULT").Visible = False
            .Columns("MC").Visible = False
            .Columns("KEYNO").Visible = False
            .Columns("ISSUED FROM").HeaderText = IIf(rbtIssue.Checked, "ISSUED FROM", "RECEIVED TO")
            .Columns("ISSUED TO").HeaderText = IIf(rbtIssue.Checked, "ISSUED TO", "RECEIVED FROM")
            FormatGridColumns(dgv, False, False, , False)
            FillGridGroupStyle_KeyNoWise(dgv)
        End With
    End Sub
    Private Sub chkCostCentreSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCostCentreSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCostCentre, chkCostCentreSelectAll.Checked)
    End Sub
    Private Sub chkFCostCentreSelectAll_CheckedChanged(sender As Object, e As EventArgs) Handles chkFCostCentreSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstFCostCentre, chkFCostCentreSelectAll.Checked)
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New NonTagIssRecSyncRpt_Properties
        obj.p_chkAsOnDate = chkAsOnDate.Checked
        obj.p_cmbItemName = cmbItemName.Text
        obj.p_cmbcategory = cmbCategory.Text
        obj.p_chkCostCentreSelectAll = chkCostCentreSelectAll.Checked
        GetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre)
        obj.p_chkFCostCentreSelectAll = chkFCostCentreSelectAll.Checked
        GetChecked_CheckedList(chkLstFCostCentre, obj.p_chkLstFCostCentre)
        obj.p_rbtIssue = rbtIssue.Checked
        obj.p_rbtReceipt = rbtReceipt.Checked
        obj.p_chkItemWiseGroup = chkItemWiseGroup.Checked
        obj.p_cmbOrderBy = cmbOrderBy.Text
        obj.p_chkInclVA = ChkVaDetail.Checked
        SetSettingsObj(obj, Me.Name, GetType(NonTagIssRecSyncRpt_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New NonTagIssRecSyncRpt_Properties
        GetSettingsObj(obj, Me.Name, GetType(NonTagIssRecSyncRpt_Properties))
        chkAsOnDate.Checked = obj.p_chkAsOnDate
        cmbItemName.Text = obj.p_cmbItemName
        cmbCategory.Text = obj.p_cmbcategory
        chkCostCentreSelectAll.Checked = obj.p_chkCostCentreSelectAll
        SetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre, cnCostName)
        chkFCostCentreSelectAll.Checked = obj.p_chkFCostCentreSelectAll
        SetChecked_CheckedList(chkLstFCostCentre, obj.p_chkLstFCostCentre, cnCostName)
        rbtIssue.Checked = obj.p_rbtIssue
        rbtReceipt.Checked = obj.p_rbtReceipt
        chkItemWiseGroup.Checked = obj.p_chkItemWiseGroup
        ChkVaDetail.Checked = obj.p_chkInclVA
        cmbOrderBy.Text = obj.p_cmbOrderBy
    End Sub

    Private Sub rbtIssue_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtIssue.CheckedChanged
        If rbtIssue.Checked Then
            chkFCostCentreSelectAll.Text = "To Centre"
        Else
            chkFCostCentreSelectAll.Text = "From Centre"
        End If
    End Sub

    Private Sub rbtReceipt_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtReceipt.CheckedChanged
        If rbtReceipt.Checked Then
            chkFCostCentreSelectAll.Text = "From Centre"
        Else
            chkFCostCentreSelectAll.Text = "To Centre"
        End If
    End Sub

    Private Sub cmbItemName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbItemName.GotFocus
        cmbItemName.Items.Clear()
        cmbItemName.Items.Add("ALL")
        strSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST"
        strSql += " WHERE ACTIVE = 'Y'"
        strSql += " AND STOCKTYPE = 'T'"
        If cmbMetal.Text <> "ALL" And cmbMetal.Text <> "" Then
            strSql += " AND METALID=(SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME='" & cmbMetal.Text & "')"
        End If
        objGPack.FillCombo(strSql, cmbItemName, False, False)
        cmbItemName.Text = "ALL"
    End Sub

    Private Sub ChkSummary_CheckedChanged(sender As Object, e As EventArgs) Handles ChkSummary.CheckedChanged
        chkItemWiseGroup.Enabled = Not ChkSummary.Checked : chkItemWiseGroup.Checked = False
        ChkActDate.Enabled = Not ChkSummary.Checked : ChkActDate.Checked = False
        ChkVaDetail.Enabled = Not ChkSummary.Checked : ChkVaDetail.Checked = False
    End Sub

    Private Sub cmbCategory_OWN_GotFocus(sender As Object, e As EventArgs) Handles cmbCategory.GotFocus
        cmbCategory.Items.Clear()
        cmbCategory.Items.Add("ALL")
        strSql = " SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY"
        strSql += " WHERE ACTIVE = 'Y'"
        If cmbMetal.Text <> "ALL" And cmbMetal.Text <> "" Then
            strSql += " AND METALID=(SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME='" & cmbMetal.Text & "')"
        End If
        objGPack.FillCombo(strSql, cmbCategory, False, False)
        cmbCategory.Text = "ALL"
    End Sub

    Private Sub chkCounterWiseGroup_CheckedChanged(sender As Object, e As EventArgs) Handles chkCounterWiseGroup.CheckedChanged
        If chkCounterWiseGroup.Checked Then
            ChkActDate.Enabled = False
            chkItemWiseGroup.Enabled = False
            ChkSummary.Enabled = False
            ChkVaDetail.Enabled = False
            txtrefNo.Enabled = False
            chkItemWiseGroup.Checked = False
            ChkSummary.Checked = False
        Else
            txtrefNo.Enabled = True
            ChkActDate.Enabled = True
            chkItemWiseGroup.Enabled = True
            ChkSummary.Enabled = True
            ChkVaDetail.Enabled = True
        End If
    End Sub
    Private Sub cmbtoCostCenter_Leave(sender As Object, e As EventArgs) Handles cmbtoCostCenter.Leave
        If cmbFromCostCenter.Text = cmbtoCostCenter.Text Then
            MsgBox("SAME COST CENTER NOT ALLOWED", MsgBoxStyle.Information)
            cmbtoCostCenter.Text = ""
            cmbtoCostCenter.Focus()
        End If
    End Sub
End Class

Public Class NonTagIssRecSyncRpt_Properties
    Private chkAsOnDate As Boolean = True
    Public Property p_chkAsOnDate() As Boolean
        Get
            Return chkAsOnDate
        End Get
        Set(ByVal value As Boolean)
            chkAsOnDate = value
        End Set
    End Property

    Private cmbItemName As String = "ALL"
    Public Property p_cmbItemName() As String
        Get
            Return cmbItemName
        End Get
        Set(ByVal value As String)
            cmbItemName = value
        End Set
    End Property
    Private cmbFromCostCenter As String = ""
    Public Property p_cmbFromCostCenter() As String
        Get
            Return cmbFromCostCenter
        End Get
        Set(ByVal value As String)
            cmbFromCostCenter = value
        End Set
    End Property
    Private cmbtoCostCenter As String = ""
    Public Property p_cmbtoCostCenter() As String
        Get
            Return cmbtoCostCenter
        End Get
        Set(ByVal value As String)
            cmbtoCostCenter = value
        End Set
    End Property
    Private cmbcategory As String = "ALL"
    Public Property p_cmbcategory() As String
        Get
            Return cmbcategory
        End Get
        Set(ByVal value As String)
            cmbcategory = value
        End Set
    End Property

    Private chkCostCentreSelectAll As Boolean = False
    Public Property p_chkCostCentreSelectAll() As Boolean
        Get
            Return chkCostCentreSelectAll
        End Get
        Set(ByVal value As Boolean)
            chkCostCentreSelectAll = value
        End Set
    End Property
    Private chkFCostCentreSelectAll As Boolean = False
    Public Property p_chkFCostCentreSelectAll() As Boolean
        Get
            Return chkFCostCentreSelectAll
        End Get
        Set(ByVal value As Boolean)
            chkFCostCentreSelectAll = value
        End Set
    End Property

    Private chkLstCostCentre As New List(Of String)
    Public Property p_chkLstCostCentre() As List(Of String)
        Get
            Return chkLstCostCentre
        End Get
        Set(ByVal value As List(Of String))
            chkLstCostCentre = value
        End Set
    End Property
    Private chkLstFCostCentre As New List(Of String)
    Public Property p_chkLstFCostCentre() As List(Of String)
        Get
            Return chkLstFCostCentre
        End Get
        Set(ByVal value As List(Of String))
            chkLstFCostCentre = value
        End Set
    End Property

    Private rbtIssue As Boolean = True
    Public Property p_rbtIssue() As Boolean
        Get
            Return rbtIssue
        End Get
        Set(ByVal value As Boolean)
            rbtIssue = value
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
    Private chkItemWiseGroup As Boolean = False
    Public Property p_chkItemWiseGroup() As Boolean
        Get
            Return chkItemWiseGroup
        End Get
        Set(ByVal value As Boolean)
            chkItemWiseGroup = value
        End Set
    End Property
    Private chkCounterWiseGroup As Boolean = False
    Public Property p_chkCounterWiseGroup() As Boolean
        Get
            Return chkCounterWiseGroup
        End Get
        Set(ByVal value As Boolean)
            chkCounterWiseGroup = value
        End Set
    End Property
    Private chkInclVA As Boolean = False
    Public Property p_chkInclVA() As Boolean
        Get
            Return chkInclVA
        End Get
        Set(ByVal value As Boolean)
            chkInclVA = value
        End Set
    End Property

    Private cmbOrderBy As String = "TAGNO"
    Public Property p_cmbOrderBy() As String
        Get
            Return cmbOrderBy
        End Get
        Set(ByVal value As String)
            cmbOrderBy = value
        End Set
    End Property
End Class






