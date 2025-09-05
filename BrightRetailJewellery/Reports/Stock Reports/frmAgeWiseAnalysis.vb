Imports System.Data.OleDb

Public Class frmAgeWiseAnalysis
    'CALID-596: CLIENT-BSM:CORRECTION-ACTUAL RECDATE OPTION REQUIRED: ALTER BY SATHYA
    'CALID-714: CLIENT-PRINCE:CORRECTION-GROUP BY TABLE AND DESIGNER OPTION REQUIRED: ALTER BY VASANTH
    Dim strSql As String
    Dim dt As DataTable
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim dsGridView As New DataSet
    Dim max As Integer = 0
    Dim cnthead As Integer = 0
    Dim STRQRYSTRINGHEAD As String = Nothing
    Dim dtrange As New DataTable
    Dim dtTablecode As New DataTable
    Dim agestkview As Boolean = IIf(GetAdmindbSoftValue("AGESTKITEMROW", "N") = "Y", True, False)
    Dim dtCompany As New DataTable

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.

        'CATEGORY
        cmbCategory.Items.Add("ALL")
        strSql = " SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY ORDER BY CATNAME"
        objGPack.FillCombo(strSql, cmbCategory, False)
        cmbCategory.Text = "ALL"

        'METAL
        cmbmetal.Items.Add("ALL")
        strSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST ORDER BY METALNAME"
        objGPack.FillCombo(strSql, cmbmetal, False)
        cmbmetal.Text = "ALL"

        'ITEM
        cmbItem.Items.Add("ALL")
        strSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST ORDER BY ITEMNAME"
        objGPack.FillCombo(strSql, cmbItem, False)
        cmbItem.Text = "ALL"

        'DESIGNER
        cmbDesigner.Items.Add("ALL")
        strSql = " SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER ORDER BY DESIGNERNAME"
        objGPack.FillCombo(strSql, cmbDesigner, False)
        cmbDesigner.Text = "ALL"

        'ITEM COUNTER
        cmbCounter.Items.Add("ALL")
        strSql = " SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER ORDER BY ITEMCTRNAME"
        objGPack.FillCombo(strSql, cmbCounter, False)
        cmbCounter.Text = "ALL"
        


        'COST CENTRE
        strSql = "SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COSTCENTRE'"
        If objGPack.GetSqlValue(strSql, , "N") = "Y" Then
            cmbCostCentre.Enabled = True
            cmbCostCentre.Items.Add("ALL")
            strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTID"
            objGPack.FillCombo(strSql, cmbCostCentre, False)
            cmbCostCentre.Text = IIf(cnDefaultCostId, "ALL", cnCostName)
            If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then cmbCostCentre.Enabled = False
        Else
            cmbCostCentre.Enabled = False
        End If

        'CALID-714
        strSql = " SELECT 'ALL' TABLECODE,0 RESULT "
        strSql += " UNION ALL"
        strSql += " SELECT DISTINCT TABLECODE,1 RESULT FROM " & cnAdminDb & "..ITEMTAG WHERE ISNULL(TABLECODE,'')<>''  "
        strSql += " ORDER BY RESULT,TABLECODE"
        dtTablecode = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtTablecode)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbtablecode, dtTablecode, "TABLECODE", , "ALL")
        'objGPack.FillCombo(strSql, CmbTableCode, False)
        'CmbTableCode.Text = "ALL"

    End Sub

    Private Sub frmAgeWiseAnalysis_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Me.WindowState = FormWindowState.Maximized
        Me.tabMain.ItemSize = New Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGen.Left, Me.tabGen.Top, Me.tabGen.Width, Me.tabGen.Height))
        Me.tabMain.SelectedTab = tabGen
        strSql = " SELECT COMPANYNAME,COMPANYID,1 RESULT FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'Y')<>'N'"
        strSql += " ORDER BY RESULT,COMPANYNAME"
        dtCompany = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCompany)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dtCompany, "COMPANYNAME", , strCompanyName)
        btnNew_Click(Me, e)
    End Sub

    Function funcGridStyle() As Integer
        With gridView_OWN
            With .Rows(0)
                .DefaultCellStyle.BackColor = System.Drawing.SystemColors.ControlDark
                .DefaultCellStyle.SelectionBackColor = System.Drawing.SystemColors.ControlDark
                .DefaultCellStyle.SelectionForeColor = Color.Black
                .DefaultCellStyle.Font = New Font("Verdana", "8.25", FontStyle.Bold, GraphicsUnit.Point)
            End With
            With .Rows(1)
                .DefaultCellStyle.BackColor = System.Drawing.SystemColors.Control
                .DefaultCellStyle.SelectionBackColor = System.Drawing.SystemColors.Control
                .DefaultCellStyle.SelectionForeColor = Color.Black
                .DefaultCellStyle.Font = New Font("Verdana", "8.25", FontStyle.Bold, GraphicsUnit.Point)
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            End With
            With .Columns("Particular")
                .Width = 300
            End With
            With .Columns("result")
                .Visible = False
            End With
            With .Columns("itemId")
                .Visible = False
            End With
            With .Columns("itemName")
                .Width = 150
                .Visible = False
            End With
            If .Columns.Contains("subitemName") Then
                With .Columns("subitemName")
                    .Visible = False
                End With
            End If
            For cnt As Integer = 3 To .Columns.Count - 1 Step 2
                With .Columns(cnt)
                    .Width = 60
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .Resizable = DataGridViewTriState.False
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End With
            Next
            For cnt As Integer = 4 To .Columns.Count - 1 Step 2
                With .Columns(cnt)
                    .Width = 80
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .Resizable = DataGridViewTriState.False
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End With
            Next
        End With
    End Function

    Function funcHeader1(ByVal ro As DataRow) As DataRow
        If txtFromDay1_NUM.Text <> "" And txtToDay1_NUM.Text <> "" Then
            ro("Pcs1") = txtFromDay1_NUM.Text + "-"
            ro("GrsWt1") = txtToDay1_NUM.Text + " DAYS"
        End If
        If txtFromDay2_NUM.Text <> "" And txtToDay2_NUM.Text <> "" Then
            ro("Pcs2") = txtFromDay2_NUM.Text + "-"
            ro("GrsWt2") = txtToDay2_NUM.Text + " DAYS"
        End If
        If txtFromDay3_NUM.Text <> "" And txtToDay3_NUM.Text <> "" Then
            ro("Pcs3") = txtFromDay3_NUM.Text + "-"
            ro("GrsWt3") = txtToDay3_NUM.Text + " DAYS"
        End If
        If txtFromDay4_NUM.Text <> "" And txtToDay4_NUM.Text <> "" Then
            ro("Pcs4") = txtFromDay4_NUM.Text + "-"
            ro("GrsWt4") = txtToDay4_NUM.Text + " DAYS"
        End If
        ro("MaxPcs") = ">" + max.ToString
        ro("MaxGrsWt") = "DAYS"
        Return ro
    End Function

    Function funcHeader2(ByVal ro As DataRow) As DataRow
        ro("itemName") = "ITEM"
        ro("subItemName") = "SUB ITEM"
        If txtFromDay1_NUM.Text <> "" And txtToDay1_NUM.Text <> "" Then
            ro("Pcs1") = "PCS"
            ro("GrsWt1") = "WEIGHT"
        End If
        If txtFromDay2_NUM.Text <> "" And txtToDay2_NUM.Text <> "" Then
            ro("Pcs2") = "PCS"
            ro("GrsWt2") = "WEIGHT"
        End If
        If txtFromDay3_NUM.Text <> "" And txtToDay3_NUM.Text <> "" Then
            ro("Pcs3") = "PCS"
            ro("GrsWt3") = "WEIGHT"
        End If
        If txtFromDay4_NUM.Text <> "" And txtToDay4_NUM.Text <> "" Then
            ro("Pcs4") = "PCS"
            ro("GrsWt4") = "WEIGHT"
        End If
        ro("MaxPcs") = "PCS"
        ro("MaxGrsWt") = "WEIGHT"
        Return ro
    End Function

    Function funcWithStone() As Integer
        strSql = " if (select count(*) from TEMPTABLEDB..sysobjects where name = 'temp" & systemId & "AgeStone')>0"
        strsql += VBCRLF + " 	drop table TEMPTABLEDB..temp" & systemId & "AgeStone"
        strsql += VBCRLF + " select  '2'result,t.itemId,T.tagno,"
        strsql += VBCRLF + " (select itemName from " & cnAdminDb & "..itemMast where itemid = t.StnItemid)as itemName, "
        strsql += VBCRLF + " isnull((select subItemName from " & cnAdminDb & "..subItemMast where subItemId = t.StnSubItemid),0)as subItemName, "
        If chkCounterWise.Checked Then strSql += vbCrLf + " isnull((select ITEMCTRNAME from " & cnAdminDb & "..ITEMCOUNTER where ITEMCTRID = (select top 1 ITEMCTRID from " & cnAdminDb & "..ITEMTAG where TAGNO=t.TAGNO)),'')as ITEMCTRNAME, "
        If chkitemgrp.Checked Then strSql += vbCrLf + " isnull((select GROUPNAME from " & cnAdminDb & "..ITEMGROUPMAST where GROUPID = (select top 1 ITEMGROUP from " & cnAdminDb & "..itemmast where itemid=t.itemid)),'')as ITEMGROUPNAME, "
        If Chktablegrpwise.Checked Then strSql += vbCrLf + "ISNULL(TT.TABLECODE,'') TABLECODE, "
        If ChkDesignerGrp.Checked Then strSql += vbCrLf + " isnull((select DESIGNERNAME from " & cnAdminDb & "..DESIGNER where DESIGNERID = (select top 1 DESIGNERID from " & cnAdminDb & "..DESIGNER where DESIGNERID=TT.DESIGNERID)),'')as DESIGNER, "
        '596
        strsql += VBCRLF + " datediff(d," & IIf(chkActualDate.Checked, "TT.ACTUALRECDATE", "T.RecDate") & ",'" & IIf(chkason.Checked, dtpAsOnDate.Value.Date.ToString("yyyy-MM-dd"), dtpTo.Value.ToString("yyyy-MM-dd")) & "')as Days, "
        '596
        strsql += VBCRLF + " t.StnPcs,t.StnWt,t.StoneUnit,'2'Stone"
        strsql += VBCRLF + " into TEMPTABLEDB..temp" & systemId & "AgeStone "
        strsql += VBCRLF + " from " & cnAdminDb & "..ITEMTAGSTONE as t "
        strsql += VBCRLF + " INNER JOIN " & cnAdminDb & "..ITEMTAG TT ON T.TAGSNO=TT.SNO"
        strSql += vbCrLf + " where t.tagNO in (select tagNo from TEMPTABLEDB..temp" & systemId & "Age)"
        strSql += vbCrLf + " and t.itemId in(select itemId from TEMPTABLEDB..temp" & systemId & "Age)"
        If Not chkason.Checked Then strSql += vbCrLf + " AND " & IIf(chkActualDate.Checked, "TT.ACTUALRECDATE", "TT.RECDATE") & " BETWEEN '" & dtpAsOnDate.Value.Date.ToString("yyyy-MM-dd") & "'AND'" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Function

    Function funcFiltration() As String
        Dim chkCompId As String = GetQryStringForSp(chkCmbCompany.Text, cnAdminDb & "..COMPANY", "COMPANYID", "COMPANYNAME", True)

        If chkason.Checked = True Then
            chkason.Text = "&As OnDate"
            strSql = " WHERE (ISSDATE IS NULL OR ISSDATE >'" & dtpAsOnDate.Value.Date.ToString("yyyy-MM-dd") & "') "
        Else
            chkason.Text = "&Date From"
            strSql = " WHERE (ISSDATE IS NULL OR ISSDATE >'" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "') "
        End If

        If Not cnCentStock Then strSql += " AND COMPANYID = '" & GetStockCompId() & "'"
        If cmbCategory.Text <> "ALL" And cmbCategory.Text <> "" Then
            strSql += " and t.itemid in (select itemId from " & cnAdminDb & "..itemMast where CatCode = (select CatCode from " & cnAdminDb & "..Category where CatName = '" & cmbCategory.Text & "'))"
        End If
        If cmbmetal.Text <> "ALL" And cmbmetal.Text <> "" Then
            strSql += " and t.itemId in (select itemId from " & cnAdminDb & "..itemMast where metalid = (select metalid from " & cnAdminDb & "..metalmast where metalname='" & cmbmetal.Text & "'))"
        End If
        If cmbItem.Text <> "ALL" And cmbItem.Text <> "" Then
            strSql += " and t.itemId in (select itemId from " & cnAdminDb & "..itemMast where itemName = '" & cmbItem.Text & "')"
        End If
        If cmbDesigner.Text <> "ALL" And cmbDesigner.Text <> "" Then
            strSql += " and t.DesignerId in (select DesignerId from " & cnAdminDb & "..Designer where DesignerName = '" & cmbDesigner.Text & "')"
        End If
        If cmbCounter.Text <> "ALL" And cmbCounter.Text <> "" Then
            strSql += " and t.itemCtrId in (select itemCtrId from " & cnAdminDb & "..itemCounter where itemCtrName = '" & cmbCounter.Text & "')"
        End If
        If cmbCostCentre.Enabled = True Then
            If cmbCostCentre.Text <> "ALL" And cmbCostCentre.Text <> "" Then
                strSql += " and t.COSTID in (Select CostId from " & cnAdminDb & "..CostCentre where CostName = '" & cmbCostCentre.Text & "')"
            End If
        End If
        'CALID-714
        If chkCmbtablecode.Text <> "ALL" And chkCmbtablecode.Text <> "" Then
            strSql += " AND T.TABLECODE IN ('" & chkCmbtablecode.Text.ToString.Replace(",", "','") & "')"
        End If
        If chkCompId <> "" And chkCompId <> "ALL" Then strSql += vbCrLf + " AND ISNULL(COMPANYID,'') IN (" & chkCompId & ")"
        Return strSql
    End Function
    Function funcFiltration2() As String
        Dim chkCompId As String = GetQryStringForSp(chkCmbCompany.Text, cnAdminDb & "..COMPANY", "COMPANYID", "COMPANYNAME", True)
        If chkason.Checked = True Then
            chkason.Text = "&As OnDate"
            strSql = " WHERE (ISSDATE IS NULL OR ISSDATE >'" & dtpAsOnDate.Value.Date.ToString("yyyy-MM-dd") & "') "
        Else
            chkason.Text = "&Date From"
            strSql = " WHERE (ISSDATE IS NULL OR ISSDATE >'" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "') "
        End If
        If cmbCategory.Text <> "ALL" And cmbCategory.Text <> "" Then
            strSql += " and t.itemid in (select itemId from " & cnAdminDb & "..itemMast where CatCode = (select CatCode from " & cnAdminDb & "..Category where CatName = '" & cmbCategory.Text & "'))"
        End If
        If cmbmetal.Text <> "ALL" And cmbmetal.Text <> "" Then
            strSql += " and t.itemId in (select itemId from " & cnAdminDb & "..itemMast where metalid = (select metalid from " & cnAdminDb & "..metalmast where metalname='" & cmbmetal.Text & "'))"
        End If
        If cmbItem.Text <> "ALL" And cmbItem.Text <> "" Then
            strSql += " and t.itemId in (select itemId from " & cnAdminDb & "..itemMast where itemName = '" & cmbItem.Text & "')"
        End If
        If cmbDesigner.Text <> "ALL" And cmbDesigner.Text <> "" Then
            strSql += " and t.DesignerId in (select DesignerId from " & cnAdminDb & "..Designer where DesignerName = '" & cmbDesigner.Text & "')"
        End If
        If cmbCounter.Text <> "ALL" And cmbCounter.Text <> "" Then
            strSql += " and t.itemCtrId in (select itemCtrId from " & cnAdminDb & "..itemCounter where itemCtrName = '" & cmbCounter.Text & "')"
        End If
        If cmbCostCentre.Enabled = True Then
            If cmbCostCentre.Text <> "ALL" And cmbCostCentre.Text <> "" Then
                strSql += " and t.COSTID in (Select CostId from " & cnAdminDb & "..CostCentre where CostName = '" & cmbCostCentre.Text & "')"
            End If
        End If
        If chkCompId <> "" And chkCompId <> "ALL" Then strSql += vbCrLf + " AND ISNULL(T.COMPANYID,'') IN (" & chkCompId & ")"
        Return strSql
    End Function

    Function funcSearch() As Integer
        Dim STRQRYSTRING As String = Nothing
        Dim PCSQRY As String = Nothing
        Dim WTQRY As String = Nothing
        Dim CRTPCSQRY As String = Nothing
        Dim CRTWTQRY As String = Nothing
        Dim GRMPCSQRY As String = Nothing
        Dim GRMWTQRY As String = Nothing
        Dim ftrStr As String = funcFiltration()

        strSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "AGEWISE')>0"
        strSql += vbCrLf + " 	DROP TABLE TEMPTABLEDB..TEMP" & systemId & "AGEWISE"
        strSql += vbCrLf + " CREATE TABLE TEMPTABLEDB..TEMP" & systemId & "AGEWISE("
        strSql += vbCrLf + " PARTICULAR VARCHAR(50)"
        strSql += vbCrLf + " ,ITEMNAME VARCHAR(50)"
        If chkSubItem.Checked Then strSql += vbCrLf + " ,SUBITEMNAME VARCHAR(50)"
        If chkCounterWise.Checked Then strSql += vbCrLf + " ,ITEMCTRNAME VARCHAR(50)"
        If chkitemgrp.Checked Then strSql += vbCrLf + " ,ITEMGROUPNAME VARCHAR(50)"
        'CALID-714
        If Chktablegrpwise.Checked Then strSql += vbCrLf + " ,TABLECODE VARCHAR(50)"
        If ChkDesignerGrp.Checked Then strSql += vbCrLf + " ,DESIGNER VARCHAR(50)"

        If Not chkrangefrmmaster.Checked Then
            strSql += vbCrLf + " ,PCS1 INT"
            strSql += vbCrLf + " ,GRSWT1 NUMERIC(15,3)"
            strSql += vbCrLf + " ,PCS2 INT"
            strSql += vbCrLf + " ,GRSWT2 NUMERIC(15,3)"
            strSql += vbCrLf + " ,PCS3 INT"
            strSql += vbCrLf + " ,GRSWT3 NUMERIC(15,3)"
            strSql += vbCrLf + " ,PCS4 INT"
            strSql += vbCrLf + " ,GRSWT4 NUMERIC(15,3)"
        Else
            For i As Integer = 0 To dtrange.Rows.Count - 1
                strSql += vbCrLf + ",PCS" & i & " INT"
                strSql += vbCrLf + ",GRSWT" & i & " NUMERIC(15,3)"
            Next
        End If
        strSql += vbCrLf + " ,TOTPCS INT ,TOTGRSWT NUMERIC(15,3) ,MAXPCS INT"
        strSql += vbCrLf + " ,MAXGRSWT NUMERIC(15,3)"
        If Not chkrangefrmmaster.Checked Then
            strSql += vbCrLf + " ,CARATPCS1 INT"
            strSql += vbCrLf + " ,CARATWT1 NUMERIC(15,3)"
            strSql += vbCrLf + " ,GRMPCS1 INT"
            strSql += vbCrLf + " ,GRMWT1 NUMERIC(15,3)"
            strSql += vbCrLf + " ,CARATPCS2 INT"
            strSql += vbCrLf + " ,CARATWT2 NUMERIC(15,3)"
            strSql += vbCrLf + " ,GRMPCS2 INT"
            strSql += vbCrLf + " ,GRMWT2 NUMERIC(15,3)"
            strSql += vbCrLf + " ,CARATPCS3 INT"
            strSql += vbCrLf + " ,CARATWT3 NUMERIC(15,3)"
            strSql += vbCrLf + " ,GRMPCS3 INT"
            strSql += vbCrLf + " ,GRMWT3 NUMERIC(15,3)"
            strSql += vbCrLf + " ,CARATPCS4 INT"
            strSql += vbCrLf + " ,CARATWT4 NUMERIC(15,3)"
            strSql += vbCrLf + " ,GRMPCS4 INT"
            strSql += vbCrLf + " ,GRMWT4 NUMERIC(15,3)"
        Else
            For i As Integer = 0 To dtrange.Rows.Count - 1
                strSql += vbCrLf + ",CARATPCS" & i & " INT"
                strSql += vbCrLf + ",CARATWT" & i & " NUMERIC(15,3)"
                strSql += vbCrLf + ",GRMPCS" & i & " INT"
                strSql += vbCrLf + ",GRMWT" & i & " NUMERIC(15,3)"
            Next
        End If

        strSql += vbCrLf + " ,TOTCARATPCS INT"
        strSql += vbCrLf + " ,TOTCARATWT NUMERIC(15,3)"
        strSql += vbCrLf + " ,TOTGRMPCS INT"
        strSql += vbCrLf + " ,TOTGRMWT NUMERIC(15,3)"

        strSql += vbCrLf + " ,MAXCARATPCS INT"
        strSql += vbCrLf + " ,MAXCARATWT NUMERIC(15,3)"
        strSql += vbCrLf + " ,MAXGRMPCS INT"
        strSql += vbCrLf + " ,MAXGRMWT NUMERIC(15,3)"

        strSql += vbCrLf + " ,ITEMID INT"
        strSql += vbCrLf + " ,RESULT NUMERIC(15,2)"
        strSql += vbCrLf + " ,STONE VARCHAR(1)"
        strSql += vbCrLf + " ,COLHEAD VARCHAR(1)"
        strSql += vbCrLf + " ,TAGNO VARCHAR(100)"
        strSql += vbCrLf + " ,SNO INT IDENTITY)"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        max = 0
        strSql = " if (select count(*) from TEMPTABLEDB..sysobjects where name = 'temp" & systemId & "Age')>0"
        strSql += vbCrLf + " 	drop table TEMPTABLEDB..temp" & systemId & "Age"
        strSql += vbCrLf + " select  '1'result,t.itemId,t.tagNo,"
        strSql += vbCrLf + " convert(nvarchar(50),(select itemName from " & cnAdminDb & "..itemMast where itemid = t.itemid))as itemName, "
        If chkSubItem.Checked Then strSql += vbCrLf + " isnull((select subItemName from " & cnAdminDb & "..subItemMast where subItemId = t.subItemid),'')as subItemName, "
        If chkCounterWise.Checked Then strSql += vbCrLf + " isnull((select ITEMCTRNAME from " & cnAdminDb & "..ITEMCOUNTER where ITEMCTRID = t.ITEMCTRID),'')as ITEMCTRNAME, "
        If chkitemgrp.Checked Then strSql += vbCrLf + " isnull((select GROUPNAME from " & cnAdminDb & "..ITEMGROUPMAST where GROUPID =(SELECT ITEMGROUP FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=t.ITEMID)),'')as ITEMGROUPNAME, "
        If Chktablegrpwise.Checked Then strSql += vbCrLf + "ISNULL(TABLECODE,'') TABLECODE, "
        If ChkDesignerGrp.Checked Then strSql += vbCrLf + " ISNULL((SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID =T.DESIGNERID),'')AS DESIGNER, "
        '596
        strSql += vbCrLf + " datediff(d," & IIf(chkActualDate.Checked, "ACTUALRECDATE", "RecDate") & ",'" & IIf(chkason.Checked, dtpAsOnDate.Value.Date.ToString("yyyy-MM-dd"), dtpTo.Value.ToString("yyyy-MM-dd")) & "')+1 as Days, "
        '596
        strSql += vbCrLf + " Pcs,GrsWt,'1'Stone,CONVERT(VARCHAR(1),'')COLHEAD "
        strSql += vbCrLf + " into TEMPTABLEDB..temp" & systemId & "Age"
        strSql += vbCrLf + " from " & cnAdminDb & "..ITEMTAG as t " ''where IssDate is null
        strSql += ftrStr
        If chkason.Checked Then
            strSql += " AND " & IIf(chkActualDate.Checked, "T.ACTUALRECDATE", "T.RECDATE") & " <= '" & dtpAsOnDate.Value.Date.ToString("yyyy-MM-dd") & "'"
        Else
            strSql += " AND " & IIf(chkActualDate.Checked, "T.ACTUALRECDATE", "T.RECDATE") & " BETWEEN '" & dtpAsOnDate.Value.Date.ToString("yyyy-MM-dd") & "'AND'" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
        End If

        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        If chkSubItem.Checked Then
            ''-----------------------------Sub Total--------------------------
            strSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "AGE)>0"
            strSql += vbCrLf + " BEGIN "
            strSql += vbCrLf + " Insert into  TEMPTABLEDB..temp" & systemId & "Age(result,itemid,itemName," + IIf(chkSubItem.Checked, "subitemname,", "") + IIf(chkCounterWise.Checked, "ITEMCTRNAME,", "") + IIf(chkitemgrp.Checked, "ITEMGROUPNAME,", "") + IIf(Chktablegrpwise.Checked, "TABLECODE,", "") + IIf(ChkDesignerGrp.Checked, "DESIGNER,", "") + "days,pcs,grsWt,Stone,COLHEAD)"
            strSql += vbCrLf + " select  '3'result,t.itemId,"
            If chkCounterWise.Checked Then
                strSql += vbCrLf + " (select itemName from " & cnAdminDb & "..itemMast where itemid = t.itemid)+' TOTAL' as itemName, "
            ElseIf chkitemgrp.Checked Then
                strSql += vbCrLf + " (select itemName from " & cnAdminDb & "..itemMast where itemid = t.itemid)+' TOTAL' as itemName, "
            Else
                strSql += vbCrLf + " 'SUB TOTAL' as itemName, "
            End If

            If chkSubItem.Checked Then strSql += vbCrLf + " ''as subItemName,"
            If chkCounterWise.Checked Then strSql += vbCrLf + "  isnull((select ITEMCTRNAME from " & cnAdminDb & "..ITEMCOUNTER where ITEMCTRID = t.ITEMCTRID),'') as ITEMCTRNAME, "
            If chkitemgrp.Checked Then strSql += vbCrLf + " isnull((select GROUPNAME from " & cnAdminDb & "..ITEMGROUPMAST where GROUPID =(SELECT ITEMGROUP FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=t.ITEMID)),'')as ITEMGROUPNAME, "
            If Chktablegrpwise.Checked Then strSql += vbCrLf + " ISNULL(TABLECODE,'') TABLECODE,"
            If ChkDesignerGrp.Checked Then strSql += vbCrLf + "  ISNULL((SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = T.DESIGNERID),'') AS DESIGNER, "
            '596
            strSql += vbCrLf + " datediff(d," & IIf(chkActualDate.Checked, "ACTUALRECDATE", "RecDate") & ",'" & IIf(chkason.Checked, dtpAsOnDate.Value.Date.ToString("yyyy-MM-dd"), dtpTo.Value.Date.ToString("yyyy-MM-dd")) & "')as Days, "
            '596
            strSql += vbCrLf + " Pcs,GrsWt,'1'Stone,'S' COLHEAD"
            strSql += vbCrLf + " from " & cnAdminDb & "..ITEMTAG as t " 'where IssDate is null 
            strSql += ftrStr
            If chkason.Checked Then
                strSql += " AND " & IIf(chkActualDate.Checked, "T.ACTUALRECDATE", "T.RECDATE") & " <='" & dtpAsOnDate.Value.Date.ToString("yyyy-MM-dd") & "'"
            Else
                strSql += " AND " & IIf(chkActualDate.Checked, "T.ACTUALRECDATE", "T.RECDATE") & " BETWEEN '" & dtpAsOnDate.Value.Date.ToString("yyyy-MM-dd") & "'AND'" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
            End If

            strSql += vbCrLf + " END "
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End If
        If chkWithStone.Checked = True Then
            funcWithStone()
        End If


        STRQRYSTRING = "ITEMNAME"
        If chkSubItem.Checked Then STRQRYSTRING += ", SUBITEMNAME"
        If chkCounterWise.Checked Then STRQRYSTRING += " ,ITEMCTRNAME"
        If chkitemgrp.Checked Then STRQRYSTRING += " , ITEMGROUPNAME"
        If Chktablegrpwise.Checked Then STRQRYSTRING += " , TABLECODE"
        If ChkDesignerGrp.Checked Then STRQRYSTRING += " , DESIGNER"
        STRQRYSTRINGHEAD = "' ' PARTICULAR"
        cnthead = 1
        PCSQRY = "("
        WTQRY = "("
        If chkWithStone.Checked Then CRTPCSQRY = "(" : CRTWTQRY = "(" : GRMPCSQRY = "(" : GRMWTQRY = "("
        If Not chkrangefrmmaster.Checked Then
            If txtFromDay1_NUM.Text <> "" And txtToDay1_NUM.Text <> "" Then
                STRQRYSTRING += " , PCS1, GRSWT1 "
                PCSQRY += "CONVERT(INT,ISNULL(PCS1,0))"
                WTQRY += "CONVERT(DECIMAL(12,3),ISNULL(GRSWT1,0))"
                ''STRQRYSTRINGHEAD += " , '' """ & txtFromDay1_NUM.Text & " - " & txtToDay1_NUM.Text & """"
                max = Val(txtToDay1_NUM.Text)
                cnthead += 1
                If chkWithStone.Checked Then
                    STRQRYSTRING += " , CARATPCS1, CARATWT1, GRMPCS1, GRMWT1 "
                    CRTPCSQRY += "  CONVERT(INT,ISNULL(CARATPCS1,0))"
                    CRTWTQRY += " CONVERT(DECIMAL(12,3),ISNULL(CARATWT1,0))"
                    GRMPCSQRY += " CONVERT(INT,ISNULL(GRMPCS1,0))"
                    GRMWTQRY += " CONVERT(DECIMAL(12,3),ISNULL(GRMWT1,0))"
                    STRQRYSTRINGHEAD += " , '' 'PCS1~GRSWT1~CARATPCS1~CARATWT1~GRMPCS1~GRMWT1'"
                Else
                    STRQRYSTRINGHEAD += " , '' 'PCS1~GRSWT1'"
                End If

            End If
            If txtFromDay2_NUM.Text <> "" And txtToDay2_NUM.Text <> "" Then
                STRQRYSTRING += " , PCS2, GRSWT2 "
                PCSQRY += "+CONVERT(INT,ISNULL(PCS2,0))"
                WTQRY += "+CONVERT(DECIMAL(12,3),ISNULL(GRSWT2,0))"
                ''STRQRYSTRINGHEAD += " , '' """ & txtFromDay2_NUM.Text & " - " & txtToDay2_NUM.Text & """"
                max = Val(txtToDay2_NUM.Text)
                cnthead += 1
                If chkWithStone.Checked Then
                    STRQRYSTRING += " , CARATPCS2, CARATWT2, GRMPCS2, GRMWT2 "
                    CRTPCSQRY += "  +CONVERT(INT,ISNULL(CARATPCS2,0))"
                    CRTWTQRY += " +CONVERT(DECIMAL(12,3),ISNULL(CARATWT2,0))"
                    GRMPCSQRY += " +CONVERT(INT,ISNULL(GRMPCS2,0))"
                    GRMWTQRY += " +CONVERT(DECIMAL(12,3),ISNULL(GRMWT2,0))"
                    STRQRYSTRINGHEAD += " , '' 'PCS2~GRSWT2~CARATPCS2~CARATWT2~GRMPCS2~GRMWT2'"
                Else
                    STRQRYSTRINGHEAD += " , '' 'PCS2~GRSWT2'"
                End If

            End If
            If txtFromDay3_NUM.Text <> "" And txtToDay3_NUM.Text <> "" Then
                STRQRYSTRING += " , PCS3, GRSWT3 "
                PCSQRY += "+CONVERT(INT,ISNULL(PCS3,0))"
                WTQRY += "+CONVERT(DECIMAL(12,3),ISNULL(GRSWT3,0))"
                ''STRQRYSTRINGHEAD += " , '' """ & txtFromDay3_NUM.Text & " - " & txtToDay3_NUM.Text & """"
                max = Val(txtToDay3_NUM.Text)
                cnthead += 1
                If chkWithStone.Checked Then
                    STRQRYSTRING += " , CARATPCS3, CARATWT3, GRMPCS3, GRMWT3 "
                    CRTPCSQRY += "  +CONVERT(INT,ISNULL(CARATPCS3,0))"
                    CRTWTQRY += " +CONVERT(DECIMAL(12,3),ISNULL(CARATWT3,0))"
                    GRMPCSQRY += " +CONVERT(INT,ISNULL(GRMPCS3,0))"
                    GRMWTQRY += " +CONVERT(DECIMAL(12,3),ISNULL(GRMWT3,0))"
                    STRQRYSTRINGHEAD += " , '' 'PCS3~GRSWT3~CARATPCS3~CARATWT3~GRMPCS3~GRMWT3'"
                Else
                    STRQRYSTRINGHEAD += " , '' 'PCS3~GRSWT3'"
                End If

            End If
            If txtFromDay4_NUM.Text <> "" And txtToDay4_NUM.Text <> "" Then
                STRQRYSTRING += " , PCS4, GRSWT4 "
                PCSQRY += "+CONVERT(INT,ISNULL(PCS4,0))"
                WTQRY += "+CONVERT(DECIMAL(12,3),ISNULL(GRSWT4,0))"
                ''STRQRYSTRINGHEAD += " , '' """ & txtFromDay4_NUM.Text & " - " & txtToDay4_NUM.Text & """"
                max = Val(txtToDay4_NUM.Text)
                cnthead += 1
                If chkWithStone.Checked Then
                    STRQRYSTRING += " , CARATPCS4, CARATWT4, GRMPCS4, GRMWT4 "
                    CRTPCSQRY += " +CONVERT(INT,ISNULL(CARATPCS4,0))"
                    CRTWTQRY += " +CONVERT(DECIMAL(12,3),ISNULL(CARATWT4,0))"
                    GRMPCSQRY += " +CONVERT(INT,ISNULL(GRMPCS4,0))"
                    GRMWTQRY += "  +CONVERT(DECIMAL(12,3),ISNULL(GRMWT4,0))"
                    STRQRYSTRINGHEAD += " , '' 'PCS4~GRSWT4~CARATPCS4~CARATWT4~GRMPCS4~GRMWT4'"
                Else
                    STRQRYSTRINGHEAD += " , '' 'PCS4~GRSWT4'"
                End If
            End If
        Else
            For i As Integer = 0 To dtrange.Rows.Count - 1
                STRQRYSTRING += " , PCS" & i & ", GRSWT" & i & " "
                PCSQRY += "+CONVERT(INT,ISNULL(PCS" & i & ",0))"
                WTQRY += "+CONVERT(DECIMAL(12,3),ISNULL(GRSWT" & i & ",0))"
                If i <> dtrange.Rows.Count - 1 Then PCSQRY += "+" : WTQRY += "+"
                ''STRQRYSTRINGHEAD += " , '' """ & txtFromDay1_NUM.Text & " - " & txtToDay1_NUM.Text & """"
                max = Val(dtrange.Rows(i).Item("TODAY").ToString)
                cnthead += 1
                If chkWithStone.Checked Then
                    STRQRYSTRING += " , CARATPCS" & i & ", CARATWT" & i & ", GRMPCS" & i & ", GRMWT" & i & ""
                    CRTPCSQRY += " +CONVERT(INT,ISNULL(CARATPCS" & i & ",0))"
                    CRTWTQRY += " +CONVERT(DECIMAL(12,3),ISNULL(CARATWT" & i & ",0))"
                    GRMPCSQRY += " +CONVERT(INT,ISNULL(GRMPCS" & i & ",0))"
                    GRMWTQRY += "  +CONVERT(DECIMAL(12,3),ISNULL(GRMWT" & i & ",0))"
                    If i <> dtrange.Rows.Count - 1 Then CRTPCSQRY += "+" : CRTWTQRY += "+" : GRMPCSQRY += "+" : GRMWTQRY += "+"
                    STRQRYSTRINGHEAD += " , '' 'PCS" & i & "~GRSWT" & i & "~CARATPCS" & i & "~CARATWT" & i & "~GRMPCS" & i & "~GRMWT" & i & "'"
                Else
                    STRQRYSTRINGHEAD += " , '' 'PCS" & i & "~GRSWT" & i & "'"
                End If
            Next
        End If
        PCSQRY += "+CONVERT(INT,ISNULL(MAXPCS,0))) TOTPCS"
        WTQRY += "+CONVERT(DECIMAL(12,3),ISNULL(MAXGRSWT,0))) TOTGRSWT"
        If chkWithStone.Checked Then CRTPCSQRY += ") TOTCARATPCS" : CRTWTQRY += ") TOTCARATWT" : GRMPCSQRY += ") TOTGRMPCS" : GRMWTQRY += ") TOTGRMWT"
        STRQRYSTRING += " , MAXPCS, MAXGRSWT"
        ''STRQRYSTRINGHEAD += " , '' "" > " & max & """"
        cnthead += 1
        If chkWithStone.Checked Then
            STRQRYSTRING += " , MAXCARATPCS, MAXCARATWT, MAXGRMPCS, MAXGRMWT "
            STRQRYSTRINGHEAD += " , '' 'MAXPCS~MAXGRSWT~MAXCARATPCS~MAXCARATWT~MAXGRMPCS~MAXGRMWT'"
            STRQRYSTRINGHEAD += " , '' 'TOTPCS~TOTGRSWT~TOTCARATPCS~TOTCARATWT~TOTGRMPCS~TOTGRMWT'"
        Else
            STRQRYSTRINGHEAD += " , '' 'MAXPCS~MAXGRSWT'"
            STRQRYSTRINGHEAD += " , '' 'TOTPCS~TOTGRSWT'"
        End If

        STRQRYSTRING += " , ITEMID, RESULT, STONE, COLHEAD"

        strSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "AGE)>0"
        strSql += vbCrLf + " BEGIN "

        ''strsql += vbcrlf + " INSERT INTO TEMP" & systemId & "AGEWISE(" & STRQRYSTRING & ") "
        strSql += vbCrLf + " IF (SELECT COUNT(*) FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "AGE1')>0"
        strSql += vbCrLf + " 	DROP TABLE TEMPTABLEDB..TEMP" & systemId & "AGE1"
        strSql += vbCrLf + "  SELECT " & STRQRYSTRING & ",CONVERT(VARCHAR(100),NULL) TAGNO INTO TEMPTABLEDB..TEMP" & systemId & "AGE1 from ("
        strSql += vbCrLf + " Select itemName," + IIf(chkSubItem.Checked = True, "subItemName,", "")
        If chkCounterWise.Checked Then strSql += vbCrLf + " ITEMCTRNAME,"
        If chkitemgrp.Checked Then strSql += vbCrLf + " ITEMGROUPNAME,"
        If Chktablegrpwise.Checked Then strSql += vbCrLf + " TABLECODE,"
        If ChkDesignerGrp.Checked Then strSql += vbCrLf + " DESIGNER,"
        If Not chkrangefrmmaster.Checked Then
            If txtFromDay1_NUM.Text <> "" And txtToDay1_NUM.Text <> "" Then
                strSql += vbCrLf + " convert(varchar,sum(case when Days between " & Val(txtFromDay1_NUM.Text) & " and " & Val(txtToDay1_NUM.Text) & " then pcs else 0 end ))Pcs1 ,"
                strSql += vbCrLf + " convert(varchar,sum(case when Days between " & Val(txtFromDay1_NUM.Text) & " and " & Val(txtToDay1_NUM.Text) & " then GrsWt else 0 end ))GrsWt1 ,"
                If chkWithStone.Checked = True Then
                    strSql += vbCrLf + " convert(varchar,sum(case when Days between " & Val(txtFromDay1_NUM.Text) & " and " & Val(txtToDay1_NUM.Text) & " then CaratPcs else 0 end ))CaratPcs1,"
                    strSql += vbCrLf + " convert(varchar,sum(case when Days between " & Val(txtFromDay1_NUM.Text) & " and " & Val(txtToDay1_NUM.Text) & " then CaratWt else 0 end ))CaratWt1 ,"
                    strSql += vbCrLf + " convert(varchar,sum(case when Days between " & Val(txtFromDay1_NUM.Text) & " and " & Val(txtToDay1_NUM.Text) & " then GrmPcs else 0 end ))GrmPcs1 ,"
                    strSql += vbCrLf + " convert(varchar,sum(case when Days between " & Val(txtFromDay1_NUM.Text) & " and " & Val(txtToDay1_NUM.Text) & " then GrmWt else 0 end ))GrmWt1 ,"
                End If
                max = Val(txtToDay1_NUM.Text)
            End If
            If txtFromDay2_NUM.Text <> "" And txtToDay2_NUM.Text <> "" Then
                strSql += vbCrLf + " convert(varchar,sum(case when Days between " & Val(txtFromDay2_NUM.Text) & " and " & Val(txtToDay2_NUM.Text) & " then pcs else 0 end ))Pcs2 ,"
                strSql += vbCrLf + " convert(varchar,sum(case when Days between " & Val(txtFromDay2_NUM.Text) & " and " & Val(txtToDay2_NUM.Text) & " then GrsWt else 0 end ))GrsWt2 ,"
                If chkWithStone.Checked = True Then
                    strSql += vbCrLf + " convert(varchar,sum(case when Days between " & Val(txtFromDay2_NUM.Text) & " and " & Val(txtToDay2_NUM.Text) & " then CaratPcs else 0 end ))CaratPcs2,"
                    strSql += vbCrLf + " convert(varchar,sum(case when Days between " & Val(txtFromDay2_NUM.Text) & " and " & Val(txtToDay2_NUM.Text) & " then CaratWt else 0 end ))CaratWt2 ,"
                    strSql += vbCrLf + " convert(varchar,sum(case when Days between " & Val(txtFromDay2_NUM.Text) & " and " & Val(txtToDay2_NUM.Text) & " then GrmPcs else 0 end ))GrmPcs2 ,"
                    strSql += vbCrLf + " convert(varchar,sum(case when Days between " & Val(txtFromDay2_NUM.Text) & " and " & Val(txtToDay2_NUM.Text) & " then GrmWt else 0 end ))GrmWt2 ,"
                End If
                max = Val(txtToDay2_NUM.Text)
            End If
            If txtFromDay3_NUM.Text <> "" And txtToDay3_NUM.Text <> "" Then
                strSql += vbCrLf + " convert(varchar,sum(case when Days between " & Val(txtFromDay3_NUM.Text) & " and " & Val(txtToDay3_NUM.Text) & " then pcs else 0 end ))Pcs3 ,"
                strSql += vbCrLf + " convert(varchar,sum(case when Days between " & Val(txtFromDay3_NUM.Text) & " and " & Val(txtToDay3_NUM.Text) & " then GrsWt else 0 end ))GrsWt3 ,"
                If chkWithStone.Checked = True Then
                    strSql += vbCrLf + " convert(varchar,sum(case when Days between " & Val(txtFromDay3_NUM.Text) & " and " & Val(txtToDay3_NUM.Text) & " then CaratPcs else 0 end ))CaratPcs3,"
                    strSql += vbCrLf + " convert(varchar,sum(case when Days between " & Val(txtFromDay3_NUM.Text) & " and " & Val(txtToDay3_NUM.Text) & " then CaratWt else 0 end ))CaratWt3 ,"
                    strSql += vbCrLf + " convert(varchar,sum(case when Days between " & Val(txtFromDay3_NUM.Text) & " and " & Val(txtToDay3_NUM.Text) & " then GrmPcs else 0 end ))GrmPcs3 ,"
                    strSql += vbCrLf + " convert(varchar,sum(case when Days between " & Val(txtFromDay3_NUM.Text) & " and " & Val(txtToDay3_NUM.Text) & " then GrmWt else 0 end ))GrmWt3 ,"
                End If
                max = Val(txtToDay3_NUM.Text)
            End If
            If txtFromDay4_NUM.Text <> "" And txtToDay4_NUM.Text <> "" Then
                strSql += vbCrLf + " convert(varchar,sum(case when Days between " & Val(txtFromDay4_NUM.Text) & " and " & Val(txtToDay4_NUM.Text) & " then pcs else 0 end ))Pcs4 ,"
                strSql += vbCrLf + " convert(varchar,sum(case when Days between " & Val(txtFromDay4_NUM.Text) & " and " & Val(txtToDay4_NUM.Text) & " then GrsWt else 0 end ))GrsWt4 ,"
                If chkWithStone.Checked = True Then
                    strSql += vbCrLf + " convert(varchar,sum(case when Days between " & Val(txtFromDay4_NUM.Text) & " and " & Val(txtToDay4_NUM.Text) & " then CaratPcs else 0 end ))CaratPcs4,"
                    strSql += vbCrLf + " convert(varchar,sum(case when Days between " & Val(txtFromDay4_NUM.Text) & " and " & Val(txtToDay4_NUM.Text) & " then CaratWt else 0 end ))CaratWt4 ,"
                    strSql += vbCrLf + " convert(varchar,sum(case when Days between " & Val(txtFromDay4_NUM.Text) & " and " & Val(txtToDay4_NUM.Text) & " then GrmPcs else 0 end ))GrmPcs4 ,"
                    strSql += vbCrLf + " convert(varchar,sum(case when Days between " & Val(txtFromDay4_NUM.Text) & " and " & Val(txtToDay4_NUM.Text) & " then GrmWt else 0 end ))GrmWt4 ,"
                End If
                max = Val(txtToDay4_NUM.Text)
            End If
        Else
            For i As Integer = 0 To dtrange.Rows.Count - 1
                strSql += vbCrLf + " convert(varchar,sum(case when Days between " & Val(dtrange.Rows(i).Item("FROMDAY").ToString) & " and " & Val(dtrange.Rows(i).Item("TODAY").ToString) & " then pcs else 0 end ))Pcs" & i & " ,"
                strSql += vbCrLf + " convert(varchar,sum(case when Days between " & Val(dtrange.Rows(i).Item("FROMDAY").ToString) & " and " & Val(dtrange.Rows(i).Item("TODAY").ToString) & " then GrsWt else 0 end ))GrsWt" & i & "  ,"
                If chkWithStone.Checked = True Then
                    strSql += vbCrLf + " convert(varchar,sum(case when Days between " & Val(dtrange.Rows(i).Item("FROMDAY").ToString) & " and " & Val(dtrange.Rows(i).Item("TODAY").ToString) & " then CaratPcs else 0 end ))CaratPcs" & i & " ,"
                    strSql += vbCrLf + " convert(varchar,sum(case when Days between " & Val(dtrange.Rows(i).Item("FROMDAY").ToString) & " and " & Val(dtrange.Rows(i).Item("TODAY").ToString) & " then CaratWt else 0 end ))CaratWt" & i & "  ,"
                    strSql += vbCrLf + " convert(varchar,sum(case when Days between " & Val(dtrange.Rows(i).Item("FROMDAY").ToString) & " and " & Val(dtrange.Rows(i).Item("TODAY").ToString) & " then GrmPcs else 0 end ))GrmPcs" & i & "  ,"
                    strSql += vbCrLf + " convert(varchar,sum(case when Days between " & Val(dtrange.Rows(i).Item("FROMDAY").ToString) & " and " & Val(dtrange.Rows(i).Item("TODAY").ToString) & " then GrmWt else 0 end ))GrmWt" & i & "  ,"
                End If
                max = Val(dtrange.Rows(i).Item("TODAY").ToString)
            Next
        End If

        strSql += vbCrLf + " convert(varchar,sum(case when Days > " & max & " then pcs else 0 end ))MaxPcs ,"
        strSql += vbCrLf + " convert(varchar,sum(case when Days > " & max & " then GrsWt else 0 end ))MaxGrsWt,"
        If chkWithStone.Checked = True Then
            strSql += vbCrLf + " convert(varchar,sum(case when Days > " & max & " then Caratpcs else 0 end ))MaxCaratPcs ,"
            strSql += vbCrLf + " convert(varchar,sum(case when Days > " & max & " then CaratWt else 0 end ))MaxCaratWt,"
            strSql += vbCrLf + " convert(varchar,sum(case when Days > " & max & " then Grmpcs else 0 end ))MaxGrmPcs ,"
            strSql += vbCrLf + " convert(varchar,sum(case when Days > " & max & " then GrmWt else 0 end ))MaxGrmWt,"
        End If
        strSql += vbCrLf + " itemid,RESULT,Stone,COLHEAD"
        strSql += vbCrLf + " from"
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " select "
        strSql += vbCrLf + " itemName," + IIf(chkSubItem.Checked = True, "subItemName,", "")
        If chkCounterWise.Checked Then strSql += vbCrLf + " ITEMCTRNAME,"
        If chkitemgrp.Checked Then strSql += vbCrLf + " ITEMGROUPNAME,"
        If Chktablegrpwise.Checked Then strSql += vbCrLf + " TABLECODE,"
        If ChkDesignerGrp.Checked Then strSql += vbCrLf + " DESIGNER,"
        strSql += vbCrLf + " sum(convert(float,Pcs))as Pcs,"
        strSql += vbCrLf + " sum(convert(float,GrsWt))as GrsWt"
        strSql += vbCrLf + " ,'0' CaratPcs,'0' CaratWt,'0' GrmPcs,'0' GrmWt,1 result,itemId,days,Stone,COLHEAD"
        strSql += vbCrLf + " from TEMPTABLEDB..temp" & systemId & "Age "
        If chkCounterWise.Checked Or chkitemgrp.Checked Or Chktablegrpwise.Checked Or ChkDesignerGrp.Checked Then strSql += vbCrLf + " where colhead<>'G'"
        If txtFromDay1_NUM.Text <> "" And txtToDay1_NUM.Text <> "" Then
            strSql += vbCrLf + " AND DAYS >='" & txtFromDay1_NUM.Text & "'"
        End If
        strSql += vbCrLf + " group by itemid,result,itemName," + IIf(chkSubItem.Checked = True, "subItemName,", "") + "days,Stone,COLHEAD"
        If chkCounterWise.Checked Then strSql += vbCrLf + ",ITEMCTRNAME"
        If chkitemgrp.Checked Then strSql += vbCrLf + ",ITEMGROUPNAME"
        If Chktablegrpwise.Checked Then strSql += vbCrLf + ",TABLECODE"
        If ChkDesignerGrp.Checked Then strSql += vbCrLf + ",DESIGNER"
        'If chkCounterWise.Checked Then
        '    strSql += vbCrLf + " union all"
        '    strSql += vbCrLf + "  select "
        '    strSql += vbCrLf + "   itemName,'' subitemName,"
        '    strSql += vbCrLf + "   ITEMCTRNAME,"
        '    strSql += vbCrLf + "   sum(convert(float,Pcs))as Pcs,"
        '    strSql += vbCrLf + "   sum(convert(float,GrsWt))as GrsWt"
        '    strSql += vbCrLf + "   ,'0' CaratPcs,'0' CaratWt,'0' GrmPcs,'0' GrmWt,result,max(ITEMID)+1 ITEMID,days,Stone,COLHEAD"
        '    strSql += vbCrLf + "   from temp" & systemId & "Age where colhead='G'"
        '    strSql += vbCrLf + "   group by ITEMCTRNAME,itemName,result,days,Stone,COLHEAD"

        'End If
        If chkWithStone.Checked = True Then
            strSql += vbCrLf + " union all"
            strSql += vbCrLf + " select"
            strSql += vbCrLf + " '       '+itemName as itemName" + IIf(chkSubItem.Checked = True, ",case when subitemName = '' then '      'else'       '+subitemName end as subitemName", "") + IIf(chkCounterWise.Checked, ",ITEMCTRNAME", "") + IIf(chkitemgrp.Checked, ",ITEMGROUPNAME", "") + IIf(Chktablegrpwise.Checked, ",TABLECODE", "") + IIf(ChkDesignerGrp.Checked, ",DESIGNER", "") + ",'0' Pcs,'0' GrsWt"
            strSql += vbCrLf + " ,sum(case when StoneUnit = 'C' then StnPcs else 0 end) as CaratPcs"
            strSql += vbCrLf + " ,sum(case when StoneUnit = 'C' then StnWt else 0 end) as CaratWt"
            strSql += vbCrLf + " ,sum(case when StoneUnit = 'G' then StnPcs else 0 end) as GrmPcs"
            strSql += vbCrLf + " ,sum(case when StoneUnit = 'G' then StnWt else 0 end) as GrmWt,result,itemId,days,Stone,''COLHEAD"
            strSql += vbCrLf + " from TEMPTABLEDB..temp" & systemId & "AgeStone"
            strSql += vbCrLf + " group by itemid,result,itemName," + IIf(chkSubItem.Checked = True, "subItemName,", "") + "days,Stone"
            If chkCounterWise.Checked Then strSql += vbCrLf + ",ITEMCTRNAME"
            If chkitemgrp.Checked Then strSql += vbCrLf + ",ITEMGROUPNAME"
            If Chktablegrpwise.Checked Then strSql += vbCrLf + ",TABLECODE"
            If ChkDesignerGrp.Checked Then strSql += vbCrLf + ",DESIGNER"
        End If
        strSql += vbCrLf + " )age"
        strSql += vbCrLf + " group by itemid,result,ITEMNAME," + IIf(chkSubItem.Checked = True, "subItemName,", "") + "Stone,COLHEAD"
        If chkCounterWise.Checked Then strSql += vbCrLf + ",ITEMCTRNAME"
        If chkitemgrp.Checked Then strSql += vbCrLf + ",ITEMGROUPNAME"
        If Chktablegrpwise.Checked Then strSql += vbCrLf + ",TABLECODE"
        If ChkDesignerGrp.Checked Then strSql += vbCrLf + ",DESIGNER"
        strSql += vbCrLf + " )Z"
        strSql += vbCrLf + " Where 1=1"
        If Not chkrangefrmmaster.Checked Then
            If txtFromDay1_NUM.Text <> "" And txtToDay1_NUM.Text <> "" Then
                strSql += vbCrLf + " or (Pcs1 <> '0' or GrsWt1 <> '0'"
                If chkWithStone.Checked = True Then
                    strSql += vbCrLf + " and CaratPcs1 <> '0' and CaratWt1 <> '0'"
                    strSql += vbCrLf + " and GrmPcs1 <> '0' and GrmWt1 <> '0'"
                End If
                strSql += vbCrLf + " )"
            End If
            If txtFromDay2_NUM.Text <> "" And txtToDay2_NUM.Text <> "" Then
                strSql += vbCrLf + " or (Pcs2 <> '0' or GrsWt2 <> '0'"
                If chkWithStone.Checked = True Then
                    strSql += vbCrLf + " and CaratPcs2 <> '0' and CaratWt2 <> '0'"
                    strSql += vbCrLf + " and GrmPcs2 <> '0' and GrmWt2 <> '0'"
                End If
                strSql += vbCrLf + " )"
            End If
            If txtFromDay3_NUM.Text <> "" And txtToDay3_NUM.Text <> "" Then
                strSql += vbCrLf + " or (Pcs3 <> '0' or GrsWt3 <> '0'"
                If chkWithStone.Checked = True Then
                    strSql += vbCrLf + " and CaratPcs3 <> '0' and CaratWt3 <> '0'"
                    strSql += vbCrLf + " and GrmPcs3 <> '0' and GrmWt3 <> '0'"
                End If
                strSql += vbCrLf + " )"
            End If
            If txtFromDay4_NUM.Text <> "" And txtToDay4_NUM.Text <> "" Then
                strSql += vbCrLf + " or (Pcs4 <> '0' or GrsWt4 <> '0'"
                If chkWithStone.Checked = True Then
                    strSql += vbCrLf + " and CaratPcs4 <> '0' and CaratWt4 <> '0'"
                    strSql += vbCrLf + " and GrmPcs4 <> '0' and GrmWt4 <> '0'"
                End If
                strSql += vbCrLf + " )"
            End If
        Else
            For i As Integer = 0 To dtrange.Rows.Count - 1
                strSql += vbCrLf + " or (Pcs" & i & " <> '0' or GrsWt" & i & " <> '0'"
                If chkWithStone.Checked = True Then
                    strSql += vbCrLf + " and CaratPcs" & i & " <> '0' and CaratWt" & i & " <> '0'"
                    strSql += vbCrLf + " and GrmPcs" & i & " <> '0' and GrmWt" & i & " <> '0'"
                End If
                strSql += vbCrLf + " )"
            Next
        End If

        strSql += vbCrLf + " or (MaxPcs <> '0' or MaxGrsWt <> '0'"
        If chkWithStone.Checked = True Then
            strSql += vbCrLf + " and MaxCaratPcs <> '0' and MaxCaratWt <> '0'"
            strSql += vbCrLf + " and MaxGrmPcs <> '0' and MaxGrmWt <> '0'"
        End If
        strSql += vbCrLf + " )"
        strSql += vbCrLf + " order by " + IIf(chkCounterWise.Checked, "ITEMCTRNAME,", "") + IIf(chkitemgrp.Checked, "ITEMGROUPNAME,", "") + IIf(Chktablegrpwise.Checked, "TABLECODE,", "") + IIf(ChkDesignerGrp.Checked, "DESIGNER,", "") + "itemId," + IIf((chkCounterWise.Checked Or chkitemgrp.Checked Or Chktablegrpwise.Checked Or ChkDesignerGrp.Checked), "ITEMNAME,", "") + "result"
        strSql += vbCrLf + " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()












        Dim STRQRYSTRING1 As String = STRQRYSTRING
        STRQRYSTRING1 = STRQRYSTRING1.Replace("RESULT", "0.5 AS RESULT")
        STRQRYSTRING1 = STRQRYSTRING1 + ", TAGNO"

        'STRQRYSTRING1 = STRQRYSTRING

        If chkDetails.Checked Then
            strSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "AGE1)>0"
            strSql += vbCrLf + " BEGIN "
            strSql += vbCrLf + " 	INSERT INTO TEMPTABLEDB..TEMP" & systemId & "AGE1"
            strSql += vbCrLf + "  SELECT " & STRQRYSTRING1 & " from ("
            strSql += vbCrLf + " Select itemName," + IIf(chkSubItem.Checked = True, "subItemName,", "")
            If chkCounterWise.Checked Then strSql += vbCrLf + " ITEMCTRNAME,"
            If chkitemgrp.Checked Then strSql += vbCrLf + " ITEMGROUPNAME,"
            If Chktablegrpwise.Checked Then strSql += vbCrLf + " TABLECODE,"
            If ChkDesignerGrp.Checked Then strSql += vbCrLf + " DESIGNER,"
            If Not chkrangefrmmaster.Checked Then
                If txtFromDay1_NUM.Text <> "" And txtToDay1_NUM.Text <> "" Then
                    strSql += vbCrLf + " convert(varchar,sum(case when Days between " & Val(txtFromDay1_NUM.Text) & " and " & Val(txtToDay1_NUM.Text) & " then pcs else 0 end ))Pcs1 ,"
                    strSql += vbCrLf + " convert(varchar,sum(case when Days between " & Val(txtFromDay1_NUM.Text) & " and " & Val(txtToDay1_NUM.Text) & " then GrsWt else 0 end ))GrsWt1 ,"
                    If chkWithStone.Checked = True Then
                        strSql += vbCrLf + " convert(varchar,sum(case when Days between " & Val(txtFromDay1_NUM.Text) & " and " & Val(txtToDay1_NUM.Text) & " then CaratPcs else 0 end ))CaratPcs1,"
                        strSql += vbCrLf + " convert(varchar,sum(case when Days between " & Val(txtFromDay1_NUM.Text) & " and " & Val(txtToDay1_NUM.Text) & " then CaratWt else 0 end ))CaratWt1 ,"
                        strSql += vbCrLf + " convert(varchar,sum(case when Days between " & Val(txtFromDay1_NUM.Text) & " and " & Val(txtToDay1_NUM.Text) & " then GrmPcs else 0 end ))GrmPcs1 ,"
                        strSql += vbCrLf + " convert(varchar,sum(case when Days between " & Val(txtFromDay1_NUM.Text) & " and " & Val(txtToDay1_NUM.Text) & " then GrmWt else 0 end ))GrmWt1 ,"
                    End If
                    max = Val(txtToDay1_NUM.Text)
                End If
                If txtFromDay2_NUM.Text <> "" And txtToDay2_NUM.Text <> "" Then
                    strSql += vbCrLf + " convert(varchar,sum(case when Days between " & Val(txtFromDay2_NUM.Text) & " and " & Val(txtToDay2_NUM.Text) & " then pcs else 0 end ))Pcs2 ,"
                    strSql += vbCrLf + " convert(varchar,sum(case when Days between " & Val(txtFromDay2_NUM.Text) & " and " & Val(txtToDay2_NUM.Text) & " then GrsWt else 0 end ))GrsWt2 ,"
                    If chkWithStone.Checked = True Then
                        strSql += vbCrLf + " convert(varchar,sum(case when Days between " & Val(txtFromDay2_NUM.Text) & " and " & Val(txtToDay2_NUM.Text) & " then CaratPcs else 0 end ))CaratPcs2,"
                        strSql += vbCrLf + " convert(varchar,sum(case when Days between " & Val(txtFromDay2_NUM.Text) & " and " & Val(txtToDay2_NUM.Text) & " then CaratWt else 0 end ))CaratWt2 ,"
                        strSql += vbCrLf + " convert(varchar,sum(case when Days between " & Val(txtFromDay2_NUM.Text) & " and " & Val(txtToDay2_NUM.Text) & " then GrmPcs else 0 end ))GrmPcs2 ,"
                        strSql += vbCrLf + " convert(varchar,sum(case when Days between " & Val(txtFromDay2_NUM.Text) & " and " & Val(txtToDay2_NUM.Text) & " then GrmWt else 0 end ))GrmWt2 ,"
                    End If
                    max = Val(txtToDay2_NUM.Text)
                End If
                If txtFromDay3_NUM.Text <> "" And txtToDay3_NUM.Text <> "" Then
                    strSql += vbCrLf + " convert(varchar,sum(case when Days between " & Val(txtFromDay3_NUM.Text) & " and " & Val(txtToDay3_NUM.Text) & " then pcs else 0 end ))Pcs3 ,"
                    strSql += vbCrLf + " convert(varchar,sum(case when Days between " & Val(txtFromDay3_NUM.Text) & " and " & Val(txtToDay3_NUM.Text) & " then GrsWt else 0 end ))GrsWt3 ,"
                    If chkWithStone.Checked = True Then
                        strSql += vbCrLf + " convert(varchar,sum(case when Days between " & Val(txtFromDay3_NUM.Text) & " and " & Val(txtToDay3_NUM.Text) & " then CaratPcs else 0 end ))CaratPcs3,"
                        strSql += vbCrLf + " convert(varchar,sum(case when Days between " & Val(txtFromDay3_NUM.Text) & " and " & Val(txtToDay3_NUM.Text) & " then CaratWt else 0 end ))CaratWt3 ,"
                        strSql += vbCrLf + " convert(varchar,sum(case when Days between " & Val(txtFromDay3_NUM.Text) & " and " & Val(txtToDay3_NUM.Text) & " then GrmPcs else 0 end ))GrmPcs3 ,"
                        strSql += vbCrLf + " convert(varchar,sum(case when Days between " & Val(txtFromDay3_NUM.Text) & " and " & Val(txtToDay3_NUM.Text) & " then GrmWt else 0 end ))GrmWt3 ,"
                    End If
                    max = Val(txtToDay3_NUM.Text)
                End If
                If txtFromDay4_NUM.Text <> "" And txtToDay4_NUM.Text <> "" Then
                    strSql += vbCrLf + " convert(varchar,sum(case when Days between " & Val(txtFromDay4_NUM.Text) & " and " & Val(txtToDay4_NUM.Text) & " then pcs else 0 end ))Pcs4 ,"
                    strSql += vbCrLf + " convert(varchar,sum(case when Days between " & Val(txtFromDay4_NUM.Text) & " and " & Val(txtToDay4_NUM.Text) & " then GrsWt else 0 end ))GrsWt4 ,"
                    If chkWithStone.Checked = True Then
                        strSql += vbCrLf + " convert(varchar,sum(case when Days between " & Val(txtFromDay4_NUM.Text) & " and " & Val(txtToDay4_NUM.Text) & " then CaratPcs else 0 end ))CaratPcs4,"
                        strSql += vbCrLf + " convert(varchar,sum(case when Days between " & Val(txtFromDay4_NUM.Text) & " and " & Val(txtToDay4_NUM.Text) & " then CaratWt else 0 end ))CaratWt4 ,"
                        strSql += vbCrLf + " convert(varchar,sum(case when Days between " & Val(txtFromDay4_NUM.Text) & " and " & Val(txtToDay4_NUM.Text) & " then GrmPcs else 0 end ))GrmPcs4 ,"
                        strSql += vbCrLf + " convert(varchar,sum(case when Days between " & Val(txtFromDay4_NUM.Text) & " and " & Val(txtToDay4_NUM.Text) & " then GrmWt else 0 end ))GrmWt4 ,"
                    End If
                    max = Val(txtToDay4_NUM.Text)
                End If
            Else
                For i As Integer = 0 To dtrange.Rows.Count - 1
                    strSql += vbCrLf + " convert(varchar,sum(case when Days between " & Val(dtrange.Rows(i).Item("FROMDAY").ToString) & " and " & Val(dtrange.Rows(i).Item("TODAY").ToString) & " then pcs else 0 end ))Pcs" & i & " ,"
                    strSql += vbCrLf + " convert(varchar,sum(case when Days between " & Val(dtrange.Rows(i).Item("FROMDAY").ToString) & " and " & Val(dtrange.Rows(i).Item("TODAY").ToString) & " then GrsWt else 0 end ))GrsWt" & i & "  ,"
                    If chkWithStone.Checked = True Then
                        strSql += vbCrLf + " convert(varchar,sum(case when Days between " & Val(dtrange.Rows(i).Item("FROMDAY").ToString) & " and " & Val(dtrange.Rows(i).Item("TODAY").ToString) & " then CaratPcs else 0 end ))CaratPcs" & i & " ,"
                        strSql += vbCrLf + " convert(varchar,sum(case when Days between " & Val(dtrange.Rows(i).Item("FROMDAY").ToString) & " and " & Val(dtrange.Rows(i).Item("TODAY").ToString) & " then CaratWt else 0 end ))CaratWt" & i & "  ,"
                        strSql += vbCrLf + " convert(varchar,sum(case when Days between " & Val(dtrange.Rows(i).Item("FROMDAY").ToString) & " and " & Val(dtrange.Rows(i).Item("TODAY").ToString) & " then GrmPcs else 0 end ))GrmPcs" & i & "  ,"
                        strSql += vbCrLf + " convert(varchar,sum(case when Days between " & Val(dtrange.Rows(i).Item("FROMDAY").ToString) & " and " & Val(dtrange.Rows(i).Item("TODAY").ToString) & " then GrmWt else 0 end ))GrmWt" & i & "  ,"
                    End If
                    max = Val(dtrange.Rows(i).Item("TODAY").ToString)
                Next
            End If

            strSql += vbCrLf + " convert(varchar,sum(case when Days > " & max & " then pcs else 0 end ))MaxPcs ,"
            strSql += vbCrLf + " convert(varchar,sum(case when Days > " & max & " then GrsWt else 0 end ))MaxGrsWt,"
            If chkWithStone.Checked = True Then
                strSql += vbCrLf + " convert(varchar,sum(case when Days > " & max & " then Caratpcs else 0 end ))MaxCaratPcs ,"
                strSql += vbCrLf + " convert(varchar,sum(case when Days > " & max & " then CaratWt else 0 end ))MaxCaratWt,"
                strSql += vbCrLf + " convert(varchar,sum(case when Days > " & max & " then Grmpcs else 0 end ))MaxGrmPcs ,"
                strSql += vbCrLf + " convert(varchar,sum(case when Days > " & max & " then GrmWt else 0 end ))MaxGrmWt,"
            End If
            strSql += vbCrLf + " itemid,RESULT,Stone,COLHEAD,TAGNO"
            strSql += vbCrLf + " from"
            strSql += vbCrLf + " ("
            strSql += vbCrLf + " select "
            strSql += vbCrLf + " itemName," + IIf(chkSubItem.Checked = True, "subItemName,", "")
            If chkCounterWise.Checked Then strSql += vbCrLf + " ITEMCTRNAME,"
            If chkitemgrp.Checked Then strSql += vbCrLf + " ITEMGROUPNAME,"
            If Chktablegrpwise.Checked Then strSql += vbCrLf + " TABLECODE,"
            If ChkDesignerGrp.Checked Then strSql += vbCrLf + " DESIGNER,"
            strSql += vbCrLf + " sum(convert(float,Pcs))as Pcs,"
            strSql += vbCrLf + " sum(convert(float,GrsWt))as GrsWt"
            strSql += vbCrLf + " ,'0' CaratPcs,'0' CaratWt,'0' GrmPcs,'0' GrmWt,result,itemId,days,Stone,COLHEAD,TAGNO"
            strSql += vbCrLf + " from TEMPTABLEDB..temp" & systemId & "Age "
            If chkCounterWise.Checked Or chkitemgrp.Checked Or Chktablegrpwise.Checked Or ChkDesignerGrp.Checked Then strSql += vbCrLf + " where colhead<>'G'"
            strSql += vbCrLf + " group by itemid,result,itemName," + IIf(chkSubItem.Checked = True, "subItemName,", "") + "days,Stone,COLHEAD,TAGNO"
            If chkCounterWise.Checked Then strSql += vbCrLf + ",ITEMCTRNAME"
            If chkitemgrp.Checked Then strSql += vbCrLf + ",ITEMGROUPNAME"
            If Chktablegrpwise.Checked Then strSql += vbCrLf + ",TABLECODE"
            If ChkDesignerGrp.Checked Then strSql += vbCrLf + ",DESIGNER"
            If chkWithStone.Checked = True Then
                strSql += vbCrLf + " union all"
                strSql += vbCrLf + " select"
                strSql += vbCrLf + " '       '+itemName as itemName" + IIf(chkSubItem.Checked = True, ",case when subitemName = '' then '      'else'       '+subitemName end as subitemName", "") + IIf(chkCounterWise.Checked, ",ITEMCTRNAME", "") + IIf(chkitemgrp.Checked, ",ITEMGROUPNAME", "") + IIf(Chktablegrpwise.Checked, ",TABLECODE", "") + IIf(ChkDesignerGrp.Checked, ",DESIGNER", "") + ",'0' Pcs,'0' GrsWt"
                strSql += vbCrLf + " ,sum(case when StoneUnit = 'C' then StnPcs else 0 end) as CaratPcs"
                strSql += vbCrLf + " ,sum(case when StoneUnit = 'C' then StnWt else 0 end) as CaratWt"
                strSql += vbCrLf + " ,sum(case when StoneUnit = 'G' then StnPcs else 0 end) as GrmPcs"
                strSql += vbCrLf + " ,sum(case when StoneUnit = 'G' then StnWt else 0 end) as GrmWt,result,itemId,days,Stone,''COLHEAD,TAGNO"
                strSql += vbCrLf + " from TEMPTABLEDB..temp" & systemId & "AgeStone"
                strSql += vbCrLf + " group by itemid,result,itemName," + IIf(chkSubItem.Checked = True, "subItemName,", "") + "days,Stone,TAGNO"
                If chkCounterWise.Checked Then strSql += vbCrLf + ",ITEMCTRNAME"
                If chkitemgrp.Checked Then strSql += vbCrLf + ",ITEMGROUPNAME"
                If Chktablegrpwise.Checked Then strSql += vbCrLf + ",TABLECODE"
                If ChkDesignerGrp.Checked Then strSql += vbCrLf + ",DESIGNER"
            End If
            strSql += vbCrLf + " )age"
            strSql += vbCrLf + " group by itemid,result,ITEMNAME," + IIf(chkSubItem.Checked = True, "subItemName,", "") + "Stone,COLHEAD,TAGNO"
            If chkCounterWise.Checked Then strSql += vbCrLf + ",ITEMCTRNAME"
            If chkitemgrp.Checked Then strSql += vbCrLf + ",ITEMGROUPNAME"
            If Chktablegrpwise.Checked Then strSql += vbCrLf + ",TABLECODE"
            If ChkDesignerGrp.Checked Then strSql += vbCrLf + ",DESIGNER"
            strSql += vbCrLf + " )Z"
            strSql += vbCrLf + " Where 1=1"
            If Not chkrangefrmmaster.Checked Then
                If txtFromDay1_NUM.Text <> "" And txtToDay1_NUM.Text <> "" Then
                    strSql += vbCrLf + " or (Pcs1 <> '0' or GrsWt1 <> '0'"
                    If chkWithStone.Checked = True Then
                        strSql += vbCrLf + " and CaratPcs1 <> '0' and CaratWt1 <> '0'"
                        strSql += vbCrLf + " and GrmPcs1 <> '0' and GrmWt1 <> '0'"
                    End If
                    strSql += vbCrLf + " )"
                End If
                If txtFromDay2_NUM.Text <> "" And txtToDay2_NUM.Text <> "" Then
                    strSql += vbCrLf + " or (Pcs2 <> '0' or GrsWt2 <> '0'"
                    If chkWithStone.Checked = True Then
                        strSql += vbCrLf + " and CaratPcs2 <> '0' and CaratWt2 <> '0'"
                        strSql += vbCrLf + " and GrmPcs2 <> '0' and GrmWt2 <> '0'"
                    End If
                    strSql += vbCrLf + " )"
                End If
                If txtFromDay3_NUM.Text <> "" And txtToDay3_NUM.Text <> "" Then
                    strSql += vbCrLf + " or (Pcs3 <> '0' or GrsWt3 <> '0'"
                    If chkWithStone.Checked = True Then
                        strSql += vbCrLf + " and CaratPcs3 <> '0' and CaratWt3 <> '0'"
                        strSql += vbCrLf + " and GrmPcs3 <> '0' and GrmWt3 <> '0'"
                    End If
                    strSql += vbCrLf + " )"
                End If
                If txtFromDay4_NUM.Text <> "" And txtToDay4_NUM.Text <> "" Then
                    strSql += vbCrLf + " or (Pcs4 <> '0' or GrsWt4 <> '0'"
                    If chkWithStone.Checked = True Then
                        strSql += vbCrLf + " and CaratPcs4 <> '0' and CaratWt4 <> '0'"
                        strSql += vbCrLf + " and GrmPcs4 <> '0' and GrmWt4 <> '0'"
                    End If
                    strSql += vbCrLf + " )"
                End If
            Else
                For i As Integer = 0 To dtrange.Rows.Count - 1
                    strSql += vbCrLf + " or (Pcs" & i & " <> '0' or GrsWt" & i & " <> '0'"
                    If chkWithStone.Checked = True Then
                        strSql += vbCrLf + " and CaratPcs" & i & " <> '0' and CaratWt" & i & " <> '0'"
                        strSql += vbCrLf + " and GrmPcs" & i & " <> '0' and GrmWt" & i & " <> '0'"
                    End If
                    strSql += vbCrLf + " )"
                Next
            End If

            strSql += vbCrLf + " or (MaxPcs <> '0' or MaxGrsWt <> '0'"
            If chkWithStone.Checked = True Then
                strSql += vbCrLf + " and MaxCaratPcs <> '0' and MaxCaratWt <> '0'"
                strSql += vbCrLf + " and MaxGrmPcs <> '0' and MaxGrmWt <> '0'"
            End If
            strSql += vbCrLf + " )"
            strSql += vbCrLf + " order by " + IIf(chkCounterWise.Checked, "ITEMCTRNAME,", "") + IIf(chkitemgrp.Checked, "ITEMGROUPNAME,", "") + IIf(Chktablegrpwise.Checked, "TABLECODE,", "") + IIf(ChkDesignerGrp.Checked, "DESIGNER,", "") + "itemId," + IIf((chkCounterWise.Checked Or chkitemgrp.Checked Or Chktablegrpwise.Checked Or ChkDesignerGrp.Checked), "ITEMNAME,", "") + "result"
            strSql += vbCrLf + " END "
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()


        End If

        STRQRYSTRING1 = STRQRYSTRING1.Replace("0.5 AS RESULT", "RESULT")



















        If chkCounterWise.Checked Or chkitemgrp.Checked Or Chktablegrpwise.Checked Or ChkDesignerGrp.Checked Then
            ''-----------------------------GRAND TOTAL--------------------------
            strSql = "IF (SELECT COUNT(*) FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "AGE1')>0"
            strSql += " BEGIN "
            strSql += vbCrLf + " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "AGE1)>0"
            strSql += vbCrLf + " BEGIN "
            strSql += vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & "AGE1(ITEMNAME," + IIf(chkSubItem.Checked = True, "subItemName,", "") + IIf(chkCounterWise.Checked, "ITEMCTRNAME,", IIf(chkitemgrp.Checked, "ITEMGROUPNAME,", IIf(Chktablegrpwise.Checked, "TABLECODE,", IIf(ChkDesignerGrp.Checked, "DESIGNER,", ""))))
            If Not chkrangefrmmaster.Checked Then
                If txtFromDay1_NUM.Text <> "" And txtToDay1_NUM.Text <> "" Then strSql += vbCrLf + "PCS1, GRSWT1, "
                If txtFromDay2_NUM.Text <> "" And txtToDay2_NUM.Text <> "" Then strSql += vbCrLf + "PCS2, GRSWT2, "
                If txtFromDay3_NUM.Text <> "" And txtToDay3_NUM.Text <> "" Then strSql += vbCrLf + " PCS3,GRSWT3,"
                If txtFromDay4_NUM.Text <> "" And txtToDay4_NUM.Text <> "" Then strSql += vbCrLf + " PCS4, GRSWT4,"
            Else
                For i As Integer = 0 To dtrange.Rows.Count - 1
                    strSql += vbCrLf + "PCS" & i & ", GRSWT" & i & ", "
                Next
            End If

            strSql += vbCrLf + " MAXPCS, MAXGRSWT,ITEMID,RESULT,STONE,COLHEAD)"
            strSql += vbCrLf + "  SELECT " & IIf(chkCounterWise.Checked, "ITEMCTRNAME", IIf(chkitemgrp.Checked, "ITEMGROUPNAME", IIf(Chktablegrpwise.Checked, "TABLECODE", IIf(ChkDesignerGrp.Checked, "DESIGNER", "")))) & "+' TOTAL'," & IIf(chkSubItem.Checked = True, "'',", "") + "" & IIf(chkCounterWise.Checked, "ITEMCTRNAME,", IIf(chkitemgrp.Checked, "ITEMGROUPNAME,", IIf(Chktablegrpwise.Checked, "TABLECODE,", IIf(ChkDesignerGrp.Checked, "DESIGNER,", "")))) & ""
            If Not chkrangefrmmaster.Checked Then
                If txtFromDay1_NUM.Text <> "" And txtToDay1_NUM.Text <> "" Then
                    strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(INT,ISNULL(PCS1,0)))),"
                    strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(NUMERIC(18,2),ISNULL(GRSWT1,0)))),"
                End If
                If txtFromDay2_NUM.Text <> "" And txtToDay2_NUM.Text <> "" Then
                    strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(INT,ISNULL(PCS2,0)))),"
                    strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(NUMERIC(18,2),ISNULL(GRSWT2,0)))),"
                End If
                If txtFromDay3_NUM.Text <> "" And txtToDay3_NUM.Text <> "" Then
                    strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(INT,ISNULL(PCS3,0)))),"
                    strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(NUMERIC(18,2),ISNULL(GRSWT3,0)))),"
                End If
                If txtFromDay4_NUM.Text <> "" And txtToDay4_NUM.Text <> "" Then
                    strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(INT,ISNULL(PCS4,0)))),"
                    strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(NUMERIC(18,2),ISNULL(GRSWT4,0)))),"
                End If
            Else
                For i As Integer = 0 To dtrange.Rows.Count - 1
                    strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(INT,ISNULL(PCS" & i & ",0)))),"
                    strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(NUMERIC(18,2),ISNULL(GRSWT" & i & ",0)))),"
                Next
            End If

            strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(INT,ISNULL(MAXPCS,0)))),"
            strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(NUMERIC(18,2),ISNULL(MAXGRSWT,0)))),"
            strSql += vbCrLf + "  MAX(ITEMID)+1,'4'RESULT,'1'STONE,'G' COLHEAD"
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & "AGE1 where colhead <> 'S' AND RESULT=1"
            strSql += vbCrLf + "  GROUP BY " & IIf(chkCounterWise.Checked, "ITEMCTRNAME", IIf(chkitemgrp.Checked, "ITEMGROUPNAME", IIf(Chktablegrpwise.Checked, "TABLECODE", IIf(ChkDesignerGrp.Checked, "DESIGNER", "")))) & ""
            'strSql += vbCrLf + "  ORDER BY " & IIf(chkCounterWise.Checked, "ITEMCTRNAME", IIf(chkitemgrp.Checked, "ITEMGROUPNAME", IIf(Chktablegrpwise.Checked, "TABLECODE", IIf(ChkDesignerGrp.Checked, "DESIGNER", "")))) & ""


            strSql += vbCrLf + "  UNION ALL"


            strSql += vbCrLf + "  SELECT " & IIf(chkCounterWise.Checked, "ITEMCTRNAME", IIf(chkitemgrp.Checked, "ITEMGROUPNAME", IIf(Chktablegrpwise.Checked, "TABLECODE", IIf(ChkDesignerGrp.Checked, "DESIGNER", "")))) & "+' AVERAGE'," & IIf(chkSubItem.Checked = True, "'',", "") + "" & IIf(chkCounterWise.Checked, "ITEMCTRNAME,", IIf(chkitemgrp.Checked, "ITEMGROUPNAME,", IIf(Chktablegrpwise.Checked, "TABLECODE,", IIf(ChkDesignerGrp.Checked, "DESIGNER,", "")))) & ""
            If Not chkrangefrmmaster.Checked Then
                If txtFromDay1_NUM.Text <> "" And txtToDay1_NUM.Text <> "" Then
                    strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(INT,ISNULL(NULL,0)))),"
                    strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(NUMERIC(18,2),ISNULL(GRSWT1,0)))),"
                End If
                If txtFromDay2_NUM.Text <> "" And txtToDay2_NUM.Text <> "" Then
                    strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(INT,ISNULL(NULL,0)))),"
                    strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(NUMERIC(18,2),ISNULL(GRSWT2,0)))),"
                End If
                If txtFromDay3_NUM.Text <> "" And txtToDay3_NUM.Text <> "" Then
                    strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(INT,ISNULL(NULL,0)))),"
                    strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(NUMERIC(18,2),ISNULL(GRSWT3,0)))),"
                End If
                If txtFromDay4_NUM.Text <> "" And txtToDay4_NUM.Text <> "" Then
                    strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(INT,ISNULL(NULL,0)))),"
                    strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(NUMERIC(18,2),ISNULL(GRSWT4,0)))),"
                End If
            Else
                For i As Integer = 0 To dtrange.Rows.Count - 1
                    strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(INT,ISNULL(NULL" & i & ",0)))),"
                    strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(NUMERIC(18,2),ISNULL(GRSWT" & i & ",0)))),"
                Next
            End If

            strSql += vbCrLf + "  CONVERT(VARCHAR,NULL),"
            strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(NUMERIC(18,2),ISNULL(MAXGRSWT,0)))),"
            strSql += vbCrLf + "  MAX(ITEMID)+1,'5'RESULT,'1'STONE,'G' COLHEAD"
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & "AGE1 where colhead <> 'S' AND RESULT=1"
            strSql += vbCrLf + "  GROUP BY " & IIf(chkCounterWise.Checked, "ITEMCTRNAME", IIf(chkitemgrp.Checked, "ITEMGROUPNAME", IIf(Chktablegrpwise.Checked, "TABLECODE", IIf(ChkDesignerGrp.Checked, "DESIGNER", "")))) & ""

            strSql += vbCrLf + "  ORDER BY " & IIf(chkCounterWise.Checked, "ITEMCTRNAME", IIf(chkitemgrp.Checked, "ITEMGROUPNAME", IIf(Chktablegrpwise.Checked, "TABLECODE", IIf(ChkDesignerGrp.Checked, "DESIGNER", "")))) & ""

            strSql += vbCrLf + " END "
            strSql += vbCrLf + "END "
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End If

        ''-----------------------------FINAL TOTAL--------------------------
        strSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "AGE1')>0"
        strSql += " BEGIN "
        strSql += vbCrLf + " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "AGE1)>0"
        strSql += vbCrLf + " BEGIN "
        strSql += vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & "AGE1(ITEMNAME" + IIf(chkSubItem.Checked = True, ",subItemName", "") + IIf(chkCounterWise.Checked, ",ITEMCTRNAME", "") + "" + IIf(chkitemgrp.Checked, ",ITEMGROUPNAME", "") + IIf(Chktablegrpwise.Checked, ",TABLECODE", "") + IIf(ChkDesignerGrp.Checked, ",DESIGNER", "") + ","
        If Not chkrangefrmmaster.Checked Then
            If txtFromDay1_NUM.Text <> "" And txtToDay1_NUM.Text <> "" Then strSql += vbCrLf + "  PCS1, GRSWT1,"
            If txtFromDay2_NUM.Text <> "" And txtToDay2_NUM.Text <> "" Then strSql += vbCrLf + "  PCS2, GRSWT2, "
            If txtFromDay3_NUM.Text <> "" And txtToDay3_NUM.Text <> "" Then strSql += vbCrLf + "  PCS3,GRSWT3,"
            If txtFromDay4_NUM.Text <> "" And txtToDay4_NUM.Text <> "" Then strSql += vbCrLf + "  PCS4, GRSWT4,"
        Else
            For i As Integer = 0 To dtrange.Rows.Count - 1
                strSql += vbCrLf + "  PCS" & i & ", GRSWT" & i & ","
            Next
        End If

        strSql += vbCrLf + "  MAXPCS, MAXGRSWT,ITEMID,RESULT,STONE,COLHEAD)"
        strSql += vbCrLf + "  SELECT '  GRAND TOTAL'" + IIf(chkSubItem.Checked, ",' GRAND TOTAL'", "") + IIf(chkCounterWise.Checked, ",'ZZZZZZZZZZZ'", "") + "" + IIf(chkitemgrp.Checked, ",'ZZZZZZZZZZZZ'", "") + IIf(Chktablegrpwise.Checked, ",'ZZZZ'", "") + IIf(ChkDesignerGrp.Checked, ",'ZZZZ'", "") + ","
        If Not chkrangefrmmaster.Checked Then
            If txtFromDay1_NUM.Text <> "" And txtToDay1_NUM.Text <> "" Then
                strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(INT,ISNULL(PCS1,0)))),"
                strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(NUMERIC(18,2),ISNULL(GRSWT1,0)))),"
            End If
            If txtFromDay2_NUM.Text <> "" And txtToDay2_NUM.Text <> "" Then
                strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(INT,ISNULL(PCS2,0)))),"
                strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(NUMERIC(18,2),ISNULL(GRSWT2,0)))),"
            End If
            If txtFromDay3_NUM.Text <> "" And txtToDay3_NUM.Text <> "" Then
                strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(INT,ISNULL(PCS3,0)))),"
                strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(NUMERIC(18,2),ISNULL(GRSWT3,0)))),"
            End If
            If txtFromDay4_NUM.Text <> "" And txtToDay4_NUM.Text <> "" Then
                strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(INT,ISNULL(PCS4,0)))),"
                strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(NUMERIC(18,2),ISNULL(GRSWT4,0)))),"
            End If
        Else
            For i As Integer = 0 To dtrange.Rows.Count - 1
                strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(INT,ISNULL(PCS" & i & ",0)))),"
                strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(NUMERIC(18,2),ISNULL(GRSWT" & i & ",0)))),"
            Next
        End If

        strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(INT,ISNULL(MAXPCS,0)))),"
        strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(NUMERIC(18,2),ISNULL(MAXGRSWT,0)))),"
        strSql += vbCrLf + "  MAX(ITEMID)+5,'6'RESULT,'1'STONE,'Z' COLHEAD"
        strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & "AGE1 where colhead <> 'S' and colhead <> 'G' AND RESULT=1"

        strSql += vbCrLf + "  UNION ALL"
        strSql += vbCrLf + "  SELECT '  AVERAGE'" + IIf(chkSubItem.Checked, ",' AVERAGE'", "") + IIf(chkCounterWise.Checked, ",'ZZZZZZZZZZZ'", "") + "" + IIf(chkitemgrp.Checked, ",'ZZZZZZZZZZZZ'", "") + IIf(Chktablegrpwise.Checked, ",'ZZZZ'", "") + IIf(ChkDesignerGrp.Checked, ",'ZZZZ'", "") + ","
        If Not chkrangefrmmaster.Checked Then
            If txtFromDay1_NUM.Text <> "" And txtToDay1_NUM.Text <> "" Then
                strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(INT,ISNULL(NULL,0)))),"
                strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(NUMERIC(18,2),ISNULL(GRSWT1,0)))),"
            End If
            If txtFromDay2_NUM.Text <> "" And txtToDay2_NUM.Text <> "" Then
                strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(INT,ISNULL(NULL,0)))),"
                strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(NUMERIC(18,2),ISNULL(GRSWT2,0)))),"
            End If
            If txtFromDay3_NUM.Text <> "" And txtToDay3_NUM.Text <> "" Then
                strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(INT,ISNULL(NULL,0)))),"
                strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(NUMERIC(18,2),ISNULL(GRSWT3,0)))),"
            End If
            If txtFromDay4_NUM.Text <> "" And txtToDay4_NUM.Text <> "" Then
                strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(INT,ISNULL(NULL,0)))),"
                strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(NUMERIC(18,2),ISNULL(GRSWT4,0)))),"
            End If
        Else
            For i As Integer = 0 To dtrange.Rows.Count - 1
                strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(INT,ISNULL(NULL" & i & ",0)))),"
                strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(NUMERIC(18,2),ISNULL(GRSWT" & i & ",0)))),"
            Next
        End If

        strSql += vbCrLf + "  CONVERT(VARCHAR,NULL),"
        strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(NUMERIC(18,2),ISNULL(MAXGRSWT,0)))),"
        strSql += vbCrLf + "  MAX(ITEMID)+5,'7'RESULT,'1'STONE,'Z' COLHEAD"
        strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & "AGE1 where colhead <> 'S' and colhead <> 'G' AND RESULT=1"
        strSql += vbCrLf + " END "
        strSql += vbCrLf + " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()


        If chkDetails.Checked Then
            strSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "AGE)>0"
            strSql += vbCrLf + " BEGIN "
            strSql += vbCrLf + " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "AGE1)>0"
            strSql += vbCrLf + " BEGIN "
            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "AGE1(ITEMNAME,ITEMID,RESULT," + IIf(chkSubItem.Checked = True, "subItemName,", "") + "STONE,COLHEAD" + IIf(chkCounterWise.Checked, ",ITEMCTRNAME", "") + IIf(chkitemgrp.Checked, ",ITEMGROUPNAME", "") + IIf(Chktablegrpwise.Checked, ",TABLECODE", "") + IIf(ChkDesignerGrp.Checked, ",DESIGNER", "") + ")"
            strSql += vbCrLf + " SELECT DISTINCT ITEMNAME,ITEMID,-1 RESULT" + IIf(chkSubItem.Checked, ",''", "") + ",STONE,'T'" + IIf(chkCounterWise.Checked, ",ITEMCTRNAME", "") + IIf(chkitemgrp.Checked, ",ITEMGROUPNAME", "") + IIf(Chktablegrpwise.Checked, ",TABLECODE", "") + IIf(ChkDesignerGrp.Checked, ",DESIGNER", "") + " FROM TEMPTABLEDB..TEMP" & systemId & "AGE1 WHERE RESULT = 1"
            strSql += vbCrLf + " END "
            strSql += vbCrLf + " END "
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End If

        If chkCounterWise.Checked Or chkitemgrp.Checked Or Chktablegrpwise.Checked Or ChkDesignerGrp.Checked Then
            strSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "AGE)>0"
            strSql += vbCrLf + " BEGIN "
            strSql += vbCrLf + " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "AGE1)>0"
            strSql += vbCrLf + " BEGIN "
            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "AGE1(ITEMNAME,ITEMID,RESULT," + IIf(chkSubItem.Checked = True, "subItemName,", "") + "STONE,COLHEAD," & IIf(chkCounterWise.Checked, "ITEMCTRNAME", IIf(chkitemgrp.Checked, "ITEMGROUPNAME", IIf(Chktablegrpwise.Checked, "TABLECODE", IIf(ChkDesignerGrp.Checked, "DESIGNER", "")))) & ")"
            strSql += vbCrLf + " SELECT DISTINCT " & IIf(chkCounterWise.Checked, "ITEMCTRNAME,", IIf(chkitemgrp.Checked, "ITEMGROUPNAME,", IIf(Chktablegrpwise.Checked, "TABLECODE,", IIf(ChkDesignerGrp.Checked, "DESIGNER,", "")))) & "0,0 RESULT" + IIf(chkSubItem.Checked, ",''", "") + ",STONE,'B'," & IIf(chkCounterWise.Checked, "ITEMCTRNAME", IIf(chkitemgrp.Checked, "ITEMGROUPNAME", IIf(Chktablegrpwise.Checked, "TABLECODE", IIf(ChkDesignerGrp.Checked, "DESIGNER", "")))) & " FROM TEMPTABLEDB..TEMP" & systemId & "AGE1 WHERE RESULT = 4"
            strSql += vbCrLf + " END "
            strSql += vbCrLf + " END "
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End If

        strSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "AGE)>0"
        strSql += vbCrLf + " BEGIN "
        strSql += vbCrLf + " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "AGE1)>0"
        strSql += vbCrLf + " BEGIN "
        strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "AGEWISE(" & STRQRYSTRING1 & ",TOTPCS,TOTGRSWT" & IIf(chkWithStone.Checked, ",TOTCARATPCS,TOTCARATWT,TOTGRMPCS,TOTGRMWT", "") & ")"

        strSql += vbCrLf + " SELECT " & STRQRYSTRING1 & "," & PCSQRY & "," & WTQRY
        If chkWithStone.Checked Then strSql += vbCrLf + "," & CRTPCSQRY & "," & CRTWTQRY & "," & GRMPCSQRY & "," & GRMWTQRY & " "
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "AGE1 ORDER BY " + IIf(chkCounterWise.Checked, "ITEMCTRNAME,", "") + IIf(chkitemgrp.Checked, "ITEMGROUPNAME,", "") + IIf(Chktablegrpwise.Checked, "TABLECODE,", "") + IIf(ChkDesignerGrp.Checked, "DESIGNER,", "") + "ITEMID,RESULT" + IIf((chkCounterWise.Checked Or chkitemgrp.Checked Or Chktablegrpwise.Checked Or ChkDesignerGrp.Checked), ",ITEMNAME", "")
        strSql += vbCrLf + " END "
        strSql += vbCrLf + " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "AGE)>0"
        strSql += vbCrLf + " BEGIN "
        strSql += vbCrLf + " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "AGEWISE)>0"
        strSql += vbCrLf + " BEGIN "
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET PARTICULAR = ITEMNAME WHERE RESULT IN (0,1,2,5)"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET PARTICULAR = ITEMNAME WHERE RESULT  = 3 "
        If chkCounterWise.Checked Then strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET PARTICULAR = ITEMCTRNAME+' TOTAL' WHERE RESULT  = 4 "
        If chkitemgrp.Checked Then strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET PARTICULAR = ITEMGROUPNAME+' TOTAL' WHERE RESULT  = 4 "
        If Chktablegrpwise.Checked Then strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET PARTICULAR = TABLECODE+' TOTAL' WHERE RESULT  = 4 "
        If ChkDesignerGrp.Checked Then strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET PARTICULAR = DESIGNER+' TOTAL' WHERE RESULT  = 4 "
        If chkSubItem.Checked Then
            strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET PARTICULAR = SUBITEMNAME WHERE ISNULL(PARTICULAR,'') <> ISNULL(SUBITEMNAME,'') AND RESULT IN (1) AND ISNULL(SUBITEMNAME,'') <> ''"
        Else
            ''''''' strSql += vbCrLf + " delete from TEMPTABLEDB..TEMP" & systemId & "AGEWISE WHERE RESULT =3 or (result=0 and COLHEAD='T')"
        End If
        strSql += vbCrLf + " END "
        strSql += vbCrLf + " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()


        strSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "AGE)>0"
        strSql += vbCrLf + " BEGIN "
        strSql += vbCrLf + " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "AGEWISE)>0"
        strSql += vbCrLf + " BEGIN "
        If Not chkrangefrmmaster.Checked Then
            If txtFromDay1_NUM.Text <> "" And txtToDay1_NUM.Text <> "" Then
                strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET PCS1 = NULL WHERE PCS1 = 0 "
                strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET GRSWT1 = NULL WHERE GRSWT1 = 0 "
                strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET GRMPCS1 = NULL WHERE GRMPCS1 = 0 "
                strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET GRMWT1 = NULL WHERE GRMWT1 = 0 "
                strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET CARATPCS1 = NULL WHERE CARATPCS1 = 0 "
                strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET CARATWT1 = NULL WHERE CARATWT1 = 0 "

                strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET GRSWT1 =(CASE WHEN ISNULL(TOTGRSWT,0)>0 THEN CONVERT(NUMERIC(15,2),ISNULL(GRSWT1,0)*100/ISNULL(TOTGRSWT,0)) END) WHERE RESULT=5"
                strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET GRSWT1 =(CASE WHEN ISNULL(TOTGRSWT,0)>0 THEN CONVERT(NUMERIC(15,2),ISNULL(GRSWT1,0)*100/ISNULL(TOTGRSWT,0)) END) WHERE RESULT=7"
            End If
            If txtFromDay2_NUM.Text <> "" And txtToDay2_NUM.Text <> "" Then
                strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET PCS2 = NULL WHERE PCS2 = 0 "
                strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET GRSWT2 = NULL WHERE GRSWT2 = 0 "
                strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET CARATPCS2 = NULL WHERE CARATPCS2 = 0 "
                strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET CARATWT2 = NULL WHERE CARATWT2 = 0 "
                strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET GRMPCS2 = NULL WHERE GRMPCS2 = 0 "
                strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET GRMWT2 = NULL WHERE GRMWT2 = 0 "

                strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET GRSWT2 =(CASE WHEN ISNULL(TOTGRSWT,0)>0 THEN CONVERT(NUMERIC(15,2),ISNULL(GRSWT2,0)*100/ISNULL(TOTGRSWT,0)) END) WHERE RESULT=5"
                strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET GRSWT2 =(CASE WHEN ISNULL(TOTGRSWT,0)>0 THEN CONVERT(NUMERIC(15,2),ISNULL(GRSWT2,0)*100/ISNULL(TOTGRSWT,0)) END) WHERE RESULT=7"
            End If
            If txtFromDay3_NUM.Text <> "" And txtToDay3_NUM.Text <> "" Then
                strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET PCS3 = NULL WHERE PCS3 = 0 "
                strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET GRSWT3 = NULL WHERE GRSWT3 = 0 "
                strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET CARATPCS3 = NULL WHERE CARATPCS3 = 0 "
                strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET CARATWT3 = NULL WHERE CARATWT3 = 0 "
                strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET GRMPCS3 = NULL WHERE GRMPCS3 = 0 "
                strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET GRMWT3 = NULL WHERE GRMWT3 = 0 "

                strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET GRSWT3 =(CASE WHEN ISNULL(TOTGRSWT,0)>0 THEN CONVERT(NUMERIC(15,2),ISNULL(GRSWT3,0)*100/ISNULL(TOTGRSWT,0)) END) WHERE RESULT=5"
                strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET GRSWT3 =(CASE WHEN ISNULL(TOTGRSWT,0)>0 THEN CONVERT(NUMERIC(15,2),ISNULL(GRSWT3,0)*100/ISNULL(TOTGRSWT,0)) END) WHERE RESULT=7"
            End If
            If txtFromDay4_NUM.Text <> "" And txtToDay4_NUM.Text <> "" Then
                strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET PCS4 = NULL WHERE PCS4 = 0 "
                strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET GRSWT4 = NULL WHERE GRSWT4 = 0 "
                strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET CARATPCS4 = NULL WHERE CARATPCS4 = 0 "
                strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET CARATWT4 = NULL WHERE CARATWT4 = 0 "
                strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET GRMPCS4 = NULL WHERE GRMPCS4 = 0 "
                strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET GRMWT4 = NULL WHERE GRMWT4 = 0 "

                strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET GRSWT4 =(CASE WHEN ISNULL(TOTGRSWT,0)>0 THEN CONVERT(NUMERIC(15,2),ISNULL(GRSWT4,0)*100/ISNULL(TOTGRSWT,0)) END) WHERE RESULT=5"
                strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET GRSWT4 =(CASE WHEN ISNULL(TOTGRSWT,0)>0 THEN CONVERT(NUMERIC(15,2),ISNULL(GRSWT4,0)*100/ISNULL(TOTGRSWT,0)) END) WHERE RESULT=7"
            End If
        Else
            For i As Integer = 0 To dtrange.Rows.Count - 1
                strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET PCS" & i & " = NULL WHERE PCS" & i & " = 0 "
                strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET GRSWT" & i & " = NULL WHERE GRSWT" & i & " = 0 "
                strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET CARATPCS" & i & " = NULL WHERE CARATPCS" & i & " = 0 "
                strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET CARATWT" & i & " = NULL WHERE CARATWT" & i & " = 0 "
                strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET GRMPCS" & i & " = NULL WHERE GRMPCS" & i & " = 0 "
                strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET GRMWT" & i & " = NULL WHERE GRMWT" & i & " = 0 "

                strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET GRSWT" & i & " =(CASE WHEN ISNULL(TOTGRSWT,0)>0 THEN CONVERT(NUMERIC(15,2),ISNULL(GRSWT" & i & ",0)*100/ISNULL(TOTGRSWT,0)) END) WHERE RESULT=5"
                strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET GRSWT" & i & " =(CASE WHEN ISNULL(TOTGRSWT,0)>0 THEN CONVERT(NUMERIC(15,2),ISNULL(GRSWT" & i & ",0)*100/ISNULL(TOTGRSWT,0)) END) WHERE RESULT=7"
            Next
        End If
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET TOTPCS = NULL WHERE TOTPCS = 0 "
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET TOTGRSWT = NULL WHERE TOTGRSWT = 0 "
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET MAXPCS = NULL WHERE MAXPCS = 0 "
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET MAXGRSWT = NULL WHERE MAXGRSWT = 0 "
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET MAXCARATPCS = NULL WHERE MAXCARATPCS = 0 "
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET MAXCARATWT = NULL WHERE MAXCARATWT = 0 "
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET MAXGRMPCS = NULL WHERE MAXGRMPCS = 0 "
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET MAXGRMWT = NULL WHERE MAXGRMWT = 0 "
        If chkWithStone.Checked Then
            strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET TOTCARATPCS = NULL WHERE TOTCARATPCS = 0 "
            strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET TOTCARATWT = NULL WHERE TOTCARATWT = 0 "
            strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET TOTGRMPCS = NULL WHERE TOTGRMPCS = 0 "
            strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET TOTGRMWT = NULL WHERE TOTGRMWT = 0 "
        End If

        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET MAXPCS=NULL,MAXGRSWT =(CASE WHEN ISNULL(TOTGRSWT,0)>0 THEN CONVERT(NUMERIC(15,2),ISNULL(MAXGRSWT,0)*100/ISNULL(TOTGRSWT,0)) END) WHERE RESULT=5"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET MAXPCS=NULL,MAXGRSWT =(CASE WHEN ISNULL(TOTGRSWT,0)>0 THEN CONVERT(NUMERIC(15,2),ISNULL(MAXGRSWT,0)*100/ISNULL(TOTGRSWT,0)) END) WHERE RESULT=7"

        strSql += vbCrLf + " END "
        strSql += vbCrLf + " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        If chkDetails.Checked Then

            strSql = " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET PARTICULAR = TAGNO WHERE RESULT = 0 "
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET RESULT = 3 WHERE RESULT = 1 "
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()


            If chkCounterWise.Checked Then
                strSql = " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET PARTICULAR = ITEMNAME WHERE RESULT = -1 "
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                strSql = " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET PARTICULAR = ITEMNAME,COLHEAD = 'Z' WHERE RESULT = 3 "
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                strSql = " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET PARTICULAR = ITEMCTRNAME WHERE COLHEAD = 'B' "
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
            End If

            If chkitemgrp.Checked Then
                'strSql = " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET PARTICULAR = ITEMNAME WHERE RESULT = -1 "
                'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                'cmd.ExecuteNonQuery()

                'strSql = " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET COLHEAD = 'Z' WHERE RESULT = 3 "
                'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                'cmd.ExecuteNonQuery()

                strSql = " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET PARTICULAR = ITEMNAME WHERE RESULT = -1 "
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                strSql = " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET PARTICULAR = ITEMNAME,COLHEAD = 'Z' WHERE RESULT = 3 "
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                strSql = " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET PARTICULAR = ITEMGROUPNAME WHERE COLHEAD = 'B' "
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
            End If

            If Chktablegrpwise.Checked Then
                'strSql = " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET PARTICULAR = ITEMNAME WHERE RESULT = -1 "
                'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                'cmd.ExecuteNonQuery()

                'strSql = " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET COLHEAD = 'Z' WHERE RESULT = 3 "
                'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                'cmd.ExecuteNonQuery()

                strSql = " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET PARTICULAR = ITEMNAME WHERE RESULT = -1 "
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                strSql = " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET PARTICULAR = ITEMNAME,COLHEAD = 'Z' WHERE RESULT = 3 "
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                strSql = " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET PARTICULAR = TABLECODE WHERE COLHEAD = 'B' "
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
            End If

            If ChkDesignerGrp.Checked Then
                'strSql = " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET PARTICULAR = ITEMNAME WHERE RESULT = -1 "
                'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                'cmd.ExecuteNonQuery()

                'strSql = " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET COLHEAD = 'Z' WHERE RESULT = 3 "
                'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                'cmd.ExecuteNonQuery()

                strSql = " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET PARTICULAR = ITEMNAME WHERE RESULT = -1 "
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                strSql = " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET PARTICULAR = ITEMNAME,COLHEAD = 'Z' WHERE RESULT = 3 "
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                strSql = " UPDATE TEMPTABLEDB..TEMP" & systemId & "AGEWISE SET PARTICULAR = DESIGNER WHERE COLHEAD = 'B' "
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
            End If

        End If

        strSql = "SELECT PARTICULAR, " & STRQRYSTRING & IIf(chkWithStone.Checked, ",'' SAM1,'' SAM2", "") & ",TOTPCS,TOTGRSWT " & IIf(chkWithStone.Checked, ",TOTCARATPCS,TOTCARATWT,TOTGRMPCS,TOTGRMWT", "") & " "
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "AGEWISE ORDER BY SNO "
        dt = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        gridView_OWN.DataSource = Nothing
        tabView.Show()
        gridView_OWN.DataSource = dt
        GridViewFormat()
        If Not dt.Rows.Count > 0 Then
            MsgBox("Record Not Found", MsgBoxStyle.Exclamation)
            dtpAsOnDate.Select()
            Exit Function
        End If
        If chkason.Checked Then
            lblTitle.Text = "AGEWISE ANALYSIS AS ON " + dtpAsOnDate.Value.ToString("dd-MM-yyyy") & IIf(cmbCostCentre.Text <> "" And cmbCostCentre.Text <> "ALL", " :" & cmbCostCentre.Text, "")
        Else
            lblTitle.Text = "AGEWISE ANALYSIS BETWEEN " & dtpAsOnDate.Value.ToString("dd-MM-yyyy") & "AND " & dtpTo.Value.ToString("dd-MM-yyyy") & "" & IIf(cmbCostCentre.Text <> "" And cmbCostCentre.Text <> "ALL", " :" & cmbCostCentre.Text, "")
        End If

        tabMain.SelectedTab = tabView
        gridView_OWN.Focus()
        With gridView_OWN
            .Columns("PARTICULAR").Width = 300
            .Columns("RESULT").Visible = False
            .Columns("ITEMID").Visible = False
            .Columns("ITEMNAME").Visible = False
            If chkWithStone.Checked Then
                .Columns("SAM1").Visible = False
                .Columns("SAM2").Visible = False
            End If
            If chkSubItem.Checked Then .Columns("SUBITEMNAME").Visible = False
            If chkCounterWise.Checked Then .Columns("ITEMCTRNAME").Visible = False
            If chkitemgrp.Checked Then .Columns("ITEMGROUPNAME").Visible = False
            If Chktablegrpwise.Checked Then .Columns("TABLECODE").Visible = False
            If ChkDesignerGrp.Checked Then .Columns("DESIGNER").Visible = False
            .Columns("COLHEAD").Visible = False
            .Columns("STONE").Visible = False

            Dim i As Int16 = IIf((chkCounterWise.Checked Or chkitemgrp.Checked Or Chktablegrpwise.Checked Or ChkDesignerGrp.Checked), 3, 2)
            For cnt As Integer = IIf((chkCounterWise.Checked Or chkitemgrp.Checked Or Chktablegrpwise.Checked Or ChkDesignerGrp.Checked), 3, 2) To .Columns.Count - 1 Step 2
                With .Columns(cnt)
                    .Width = 60
                    If chkWithStone.Checked And cnt <> i Then ' And cnt <> 15 And cnt <> 21 And cnt <> 27 Then
                        .HeaderText = "STN PCS"
                    Else
                        .HeaderText = "PCS"
                        i = i + 6
                    End If
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End With
            Next
            Dim CAR As Boolean = True
            i = IIf((chkCounterWise.Checked Or chkitemgrp.Checked Or Chktablegrpwise.Checked Or ChkDesignerGrp.Checked), 4, 3)
            For cnt As Integer = IIf((chkCounterWise.Checked Or chkitemgrp.Checked Or Chktablegrpwise.Checked Or ChkDesignerGrp.Checked), 4, 3) To .Columns.Count - 1 Step 2
                With .Columns(cnt)
                    .Width = 80
                    If Not chkWithStone.Checked Then
                        .HeaderText = "WEIGHT"
                    ElseIf CAR = True And cnt <> i Then ' And cnt <> 10 And cnt <> 16 And cnt <> 22 And cnt <> 28 Then
                        .HeaderText = "STN CARAT"
                        'If curflag Then i = i + 6 : curflag = False Else curflag = True
                        i = i + 6
                        CAR = False
                    ElseIf CAR = False And cnt <> i Then '4 And cnt <> 10 And cnt <> 16 And cnt <> 22 And cnt <> 28 Then
                        .HeaderText = "STN GRM"
                        'If curflag Then i = i + 6 : curflag = False Else curflag = True
                        CAR = True
                    Else
                        .HeaderText = "WEIGHT"
                    End If
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End With
            Next


            ''CELL BACKCOLORING********************START************************
            If cnthead > 2 Then
                'For CNT As Integer = IIf(chkrangefrmmaster.Checked, 2, 3) To cnthead Step 2
                '    Dim XX As String = Nothing
                '    XX = "PCS" + Trim(Str(CNT - 2))
                '    .Columns(XX).DefaultCellStyle.BackColor = Color.AliceBlue
                '    XX = "GRSWT" + Trim(Str(CNT - 2))
                '    .Columns(XX).DefaultCellStyle.BackColor = Color.AliceBlue
                '    If chkWithStone.Checked Then
                '        XX = "CARATPCS" + Trim(Str(CNT - 2))
                '        .Columns(XX).DefaultCellStyle.BackColor = Color.AliceBlue
                '        XX = "CARATWT" + Trim(Str(CNT - 2))
                '        .Columns(XX).DefaultCellStyle.BackColor = Color.AliceBlue
                '        XX = "GRMPCS" + Trim(Str(CNT - 2))
                '        .Columns(XX).DefaultCellStyle.BackColor = Color.AliceBlue
                '        XX = "GRMWT" + Trim(Str(CNT - 2))
                '        .Columns(XX).DefaultCellStyle.BackColor = Color.AliceBlue
                '    End If
                'Next
                .Columns("MAXPCS").DefaultCellStyle.BackColor = Color.AliceBlue
                .Columns("MAXGRSWT").DefaultCellStyle.BackColor = Color.AliceBlue
                If chkWithStone.Checked Then
                    .Columns("MAXCARATPCS").DefaultCellStyle.BackColor = Color.AliceBlue
                    .Columns("MAXCARATWT").DefaultCellStyle.BackColor = Color.AliceBlue
                    .Columns("MAXGRMPCS").DefaultCellStyle.BackColor = Color.AliceBlue
                    .Columns("MAXGRMWT").DefaultCellStyle.BackColor = Color.AliceBlue
                End If
            End If
            ''CELL BACKCOLORING*******************END*************************
            .ScrollBars = ScrollBars.Both
            For CNT As Integer = 0 To .Columns.Count - 1
                .Columns(CNT).SortMode = DataGridViewColumnSortMode.NotSortable
            Next
        End With
        gridViewHead.DataSource = Nothing
        funcGridHeadWidth()
        pnlGridView.BringToFront()
        pnlgrid.BringToFront()
        gridView_OWN.BringToFront()
    End Function
    Function funcGridHeadWidth() As Integer
        If STRQRYSTRINGHEAD <> "" Then
            strSql = " SELECT " & STRQRYSTRINGHEAD & ",'' SCROLL WHERE 1=2"
            dt = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            With gridViewHead
                .DataSource = dt
                .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                For CNT As Integer = 0 To .Columns.Count - 1
                    .Columns(CNT).SortMode = DataGridViewColumnSortMode.NotSortable
                    .Columns(CNT).Resizable = DataGridViewTriState.False
                Next
                .Columns("PARTICULAR").Width = gridView_OWN.Columns("PARTICULAR").Width
                If Not chkrangefrmmaster.Checked Then
                    If cnthead > 5 Then
                        .Columns(5).Width = gridView_OWN.Columns("MAXPCS").Width + gridView_OWN.Columns("MAXGRSWT").Width
                        If chkWithStone.Checked Then .Columns(5).Width += gridView_OWN.Columns("MAXCARATPCS").Width + gridView_OWN.Columns("MAXCARATWT").Width + gridView_OWN.Columns("MAXGRMPCS").Width + gridView_OWN.Columns("MAXGRMWT").Width
                        .Columns(5).HeaderText = " > " & max

                        .Columns(6).Width = gridView_OWN.Columns("TOTPCS").Width + gridView_OWN.Columns("TOTGRSWT").Width
                        If chkWithStone.Checked Then .Columns(6).Width += gridView_OWN.Columns("TOTCARATPCS").Width + gridView_OWN.Columns("TOTCARATWT").Width + gridView_OWN.Columns("TOTGRMPCS").Width + gridView_OWN.Columns("TOTGRMWT").Width
                        .Columns(6).HeaderText = " TOTAL"


                        .Columns(4).Width = gridView_OWN.Columns("PCS4").Width + gridView_OWN.Columns("GRSWT4").Width
                        If chkWithStone.Checked Then .Columns(4).Width += gridView_OWN.Columns("CARATPCS4").Width + gridView_OWN.Columns("CARATWT4").Width + gridView_OWN.Columns("GRMPCS4").Width + gridView_OWN.Columns("GRMWT4").Width
                        .Columns(4).HeaderText = txtFromDay4_NUM.Text & " - " & txtToDay4_NUM.Text
                        .Columns(3).Width = gridView_OWN.Columns("PCS3").Width + gridView_OWN.Columns("GRSWT3").Width
                        If chkWithStone.Checked Then .Columns(3).Width += gridView_OWN.Columns("CARATPCS3").Width + gridView_OWN.Columns("CARATWT3").Width + gridView_OWN.Columns("GRMPCS3").Width + gridView_OWN.Columns("GRMWT3").Width
                        .Columns(3).HeaderText = txtFromDay3_NUM.Text & " - " & txtToDay3_NUM.Text
                        .Columns(2).Width = gridView_OWN.Columns("PCS2").Width + gridView_OWN.Columns("GRSWT2").Width
                        If chkWithStone.Checked Then .Columns(2).Width += gridView_OWN.Columns("CARATPCS2").Width + gridView_OWN.Columns("CARATWT2").Width + gridView_OWN.Columns("GRMPCS2").Width + gridView_OWN.Columns("GRMWT2").Width
                        .Columns(2).HeaderText = txtFromDay2_NUM.Text & " - " & txtToDay2_NUM.Text
                        .Columns(1).Width = gridView_OWN.Columns("PCS1").Width + gridView_OWN.Columns("GRSWT1").Width
                        If chkWithStone.Checked Then .Columns(1).Width += gridView_OWN.Columns("CARATPCS1").Width + gridView_OWN.Columns("CARATWT1").Width + gridView_OWN.Columns("GRMPCS1").Width + gridView_OWN.Columns("GRMWT1").Width
                        .Columns(1).HeaderText = txtFromDay1_NUM.Text & " - " & txtToDay1_NUM.Text
                    ElseIf cnthead > 4 Then
                        .Columns(4).Width = gridView_OWN.Columns("MAXPCS").Width + gridView_OWN.Columns("MAXGRSWT").Width
                        If chkWithStone.Checked Then .Columns(4).Width += gridView_OWN.Columns("MAXCARATPCS").Width + gridView_OWN.Columns("MAXCARATWT").Width + gridView_OWN.Columns("MAXGRMPCS").Width + gridView_OWN.Columns("MAXGRMWT").Width
                        .Columns(4).HeaderText = " > " & max

                        .Columns(5).Width = gridView_OWN.Columns("TOTPCS").Width + gridView_OWN.Columns("TOTGRSWT").Width
                        If chkWithStone.Checked Then .Columns(5).Width += gridView_OWN.Columns("TOTCARATPCS").Width + gridView_OWN.Columns("TOTCARATWT").Width + gridView_OWN.Columns("TOTGRMPCS").Width + gridView_OWN.Columns("TOTGRMWT").Width
                        .Columns(5).HeaderText = " TOTAL"


                        .Columns(3).Width = gridView_OWN.Columns("PCS3").Width + gridView_OWN.Columns("GRSWT3").Width
                        If chkWithStone.Checked Then .Columns(3).Width += gridView_OWN.Columns("CARATPCS3").Width + gridView_OWN.Columns("CARATWT3").Width + gridView_OWN.Columns("GRMPCS3").Width + gridView_OWN.Columns("GRMWT3").Width
                        .Columns(3).HeaderText = txtFromDay3_NUM.Text & " - " & txtToDay3_NUM.Text
                        .Columns(2).Width = gridView_OWN.Columns("PCS2").Width + gridView_OWN.Columns("GRSWT2").Width
                        If chkWithStone.Checked Then .Columns(2).Width += gridView_OWN.Columns("CARATPCS2").Width + gridView_OWN.Columns("CARATWT2").Width + gridView_OWN.Columns("GRMPCS2").Width + gridView_OWN.Columns("GRMWT2").Width
                        .Columns(2).HeaderText = txtFromDay2_NUM.Text & " - " & txtToDay2_NUM.Text
                        .Columns(1).Width = gridView_OWN.Columns("PCS1").Width + gridView_OWN.Columns("GRSWT1").Width
                        If chkWithStone.Checked Then .Columns(1).Width += gridView_OWN.Columns("CARATPCS1").Width + gridView_OWN.Columns("CARATWT1").Width + gridView_OWN.Columns("GRMPCS1").Width + gridView_OWN.Columns("GRMWT1").Width
                        .Columns(1).HeaderText = txtFromDay1_NUM.Text & " - " & txtToDay1_NUM.Text
                    ElseIf cnthead > 3 Then
                        .Columns(3).Width = gridView_OWN.Columns("MAXPCS").Width + gridView_OWN.Columns("MAXGRSWT").Width
                        If chkWithStone.Checked Then .Columns(3).Width += gridView_OWN.Columns("MAXCARATPCS").Width + gridView_OWN.Columns("MAXCARATWT").Width + gridView_OWN.Columns("MAXGRMPCS").Width + gridView_OWN.Columns("MAXGRMWT").Width
                        .Columns(3).HeaderText = " > " & max

                        .Columns(4).Width = gridView_OWN.Columns("TOTPCS").Width + gridView_OWN.Columns("TOTGRSWT").Width
                        If chkWithStone.Checked Then .Columns(4).Width += gridView_OWN.Columns("TOTCARATPCS").Width + gridView_OWN.Columns("TOTCARATWT").Width + gridView_OWN.Columns("TOTGRMPCS").Width + gridView_OWN.Columns("TOTGRMWT").Width
                        .Columns(4).HeaderText = " TOTAL"

                        .Columns(2).Width = gridView_OWN.Columns("PCS2").Width + gridView_OWN.Columns("GRSWT2").Width
                        If chkWithStone.Checked Then .Columns(2).Width += gridView_OWN.Columns("CARATPCS2").Width + gridView_OWN.Columns("CARATWT2").Width + gridView_OWN.Columns("GRMPCS2").Width + gridView_OWN.Columns("GRMWT2").Width
                        .Columns(2).HeaderText = txtFromDay2_NUM.Text & " - " & txtToDay2_NUM.Text
                        .Columns(1).Width = gridView_OWN.Columns("PCS1").Width + gridView_OWN.Columns("GRSWT1").Width
                        If chkWithStone.Checked Then .Columns(1).Width += gridView_OWN.Columns("CARATPCS1").Width + gridView_OWN.Columns("CARATWT1").Width + gridView_OWN.Columns("GRMPCS1").Width + gridView_OWN.Columns("GRMWT1").Width
                        .Columns(1).HeaderText = txtFromDay1_NUM.Text & " - " & txtToDay1_NUM.Text
                    ElseIf cnthead > 2 Then
                        .Columns(2).Width = gridView_OWN.Columns("MAXPCS").Width + gridView_OWN.Columns("MAXGRSWT").Width
                        If chkWithStone.Checked Then .Columns(2).Width += gridView_OWN.Columns("MAXCARATPCS").Width + gridView_OWN.Columns("MAXCARATWT").Width + gridView_OWN.Columns("MAXGRMPCS").Width + gridView_OWN.Columns("MAXGRMWT").Width
                        .Columns(2).HeaderText = " > " & max

                        .Columns(3).Width = gridView_OWN.Columns("TOTPCS").Width + gridView_OWN.Columns("TOTGRSWT").Width
                        If chkWithStone.Checked Then .Columns(3).Width += gridView_OWN.Columns("TOTCARATPCS").Width + gridView_OWN.Columns("TOTCARATWT").Width + gridView_OWN.Columns("TOTGRMPCS").Width + gridView_OWN.Columns("TOTGRMWT").Width
                        .Columns(3).HeaderText = " TOTAL"


                        .Columns(1).Width = gridView_OWN.Columns("PCS1").Width + gridView_OWN.Columns("GRSWT1").Width
                        If chkWithStone.Checked Then .Columns(1).Width += gridView_OWN.Columns("CARATPCS1").Width + gridView_OWN.Columns("CARATWT1").Width + gridView_OWN.Columns("GRMPCS1").Width + gridView_OWN.Columns("GRMWT1").Width
                        .Columns(1).HeaderText = txtFromDay1_NUM.Text & " - " & txtToDay1_NUM.Text
                    Else
                        .Columns(1).Width = gridView_OWN.Columns("MAXPCS").Width + gridView_OWN.Columns("MAXGRSWT").Width
                        If chkWithStone.Checked Then .Columns(1).Width += gridView_OWN.Columns("MAXCARATPCS").Width + gridView_OWN.Columns("MAXCARATWT").Width + gridView_OWN.Columns("MAXGRMPCS").Width + gridView_OWN.Columns("MAXGRMWT").Width
                        .Columns(1).HeaderText = " > " & max

                        .Columns(2).Width = gridView_OWN.Columns("TOTPCS").Width + gridView_OWN.Columns("TOTGRSWT").Width
                        If chkWithStone.Checked Then .Columns(2).Width += gridView_OWN.Columns("TOTCARATPCS").Width + gridView_OWN.Columns("TOTCARATWT").Width + gridView_OWN.Columns("TOTGRMPCS").Width + gridView_OWN.Columns("TOTGRMWT").Width
                        .Columns(2).HeaderText = " TOTAL"


                    End If
                Else
                    For i As Integer = 1 To .Columns.Count - 4
                        For j As Integer = 0 To dtrange.Rows.Count - 1
                            .Columns(i).Width = gridView_OWN.Columns("PCS" & j & "").Width + gridView_OWN.Columns("GRSWT" & j & "").Width
                            If chkWithStone.Checked Then .Columns(i).Width += gridView_OWN.Columns("CARATPCS" & j & "").Width + gridView_OWN.Columns("CARATWT" & j & "").Width + gridView_OWN.Columns("GRMPCS" & j & "").Width + gridView_OWN.Columns("GRMWT" & j & "").Width
                        Next
                        .Columns(i).HeaderText = dtrange.Rows(i - 1).Item("FROMDAY").ToString & " - " & dtrange.Rows(i - 1).Item("TODAY").ToString
                    Next
                    .Columns(.Columns.Count - 3).Width = gridView_OWN.Columns("MAXPCS").Width + gridView_OWN.Columns("MAXGRSWT").Width
                    If chkWithStone.Checked Then .Columns(.Columns.Count - 3).Width += gridView_OWN.Columns("MAXCARATPCS").Width + gridView_OWN.Columns("MAXCARATWT").Width + gridView_OWN.Columns("MAXGRMPCS").Width + gridView_OWN.Columns("MAXGRMWT").Width
                    .Columns(.Columns.Count - 3).HeaderText = " > " & max


                    .Columns(.Columns.Count - 2).Width = gridView_OWN.Columns("TOTPCS").Width + gridView_OWN.Columns("TOTGRSWT").Width
                    If chkWithStone.Checked Then .Columns(.Columns.Count - 2).Width += gridView_OWN.Columns("TOTCARATPCS").Width + gridView_OWN.Columns("TOTCARATWT").Width + gridView_OWN.Columns("TOTGRMPCS").Width + gridView_OWN.Columns("TOTGRMWT").Width
                    .Columns(.Columns.Count - 2).HeaderText = " TOTAL"


                End If

                .RowHeadersVisible = False
                .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                .ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
                .Height = .ColumnHeadersHeight
                .Columns("PARTICULAR").HeaderText = ""
                .Columns("SCROLL").Width = 20
                .Columns("SCROLL").HeaderText = ""
                Dim colWid As Integer = 0
                For cnt As Integer = 0 To gridView_OWN.ColumnCount - 1
                    If gridView_OWN.Columns(cnt).Visible Then colWid += gridView_OWN.Columns(cnt).Width
                Next
                If colWid >= gridView_OWN.Width Then
                    gridViewHead.Columns("SCROLL").Visible = CType(gridView_OWN.Controls(1), VScrollBar).Visible
                    gridViewHead.Columns("SCROLL").Width = CType(gridView_OWN.Controls(1), VScrollBar).Width
                Else
                    gridViewHead.Columns("SCROLL").Visible = False
                End If
            End With
        End If
    End Function
    Private Sub cmbItem_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbItem.SelectedIndexChanged
        ''Load subItemName
        strSql = " Select subItemName from " & cnAdminDb & "..subItemMast "
        strSql += " where itemId = (select itemId from " & cnAdminDb & "..itemMast where itemName = '" & cmbItem.Text & "')"
        strSql += " order by subItemName"
        cmbSubItem.Items.Clear()
        cmbSubItem.Items.Add("ALL")
        objGPack.FillCombo(strSql, cmbSubItem, False)
        cmbSubItem.Text = "ALL"
        grprangedetail.Visible = False
        chkrangefrmmaster.Checked = False
    End Sub

    Private Sub txtFromDay1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtFromDay1_NUM.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If Not Val(txtFromDay1_NUM.Text) > 0 Then
                MsgBox("Range Should be > 0", MsgBoxStyle.Exclamation)
                txtFromDay1_NUM.Focus()
                Exit Sub
            End If
            If txtToDay1_NUM.Text <> "" Then
                If Not Val(txtFromDay1_NUM.Text) < Val(txtToDay1_NUM.Text) Then
                    MsgBox("Range Should be < " + Val(txtToDay1_NUM.Text).ToString, MsgBoxStyle.Exclamation)
                    txtFromDay1_NUM.Focus()
                    Exit Sub
                End If
            End If
        End If
    End Sub

    Private Sub txtToDay1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtToDay1_NUM.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If Not Val(txtToDay1_NUM.Text) > Val(txtFromDay1_NUM.Text) Then
                MsgBox("Range Should be > " + Val(txtFromDay1_NUM.Text).ToString, MsgBoxStyle.Exclamation)
                txtToDay1_NUM.Focus()
                Exit Sub
            End If
        End If
    End Sub

    Private Sub txtFromDay2_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFromDay2_NUM.GotFocus
        If Trim(txtToDay1_NUM.Text) <> "" Then txtFromDay2_NUM.Text = Val(txtToDay1_NUM.Text) + 1
    End Sub

    Private Sub txtFromDay2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtFromDay2_NUM.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtFromDay2_NUM.Text = "" Then
                Exit Sub
            End If
            If Not Val(txtFromDay2_NUM.Text) > Val(txtToDay1_NUM.Text) Then
                MsgBox("Range Should be > " + Val(txtToDay1_NUM.Text).ToString, MsgBoxStyle.Exclamation)
                txtFromDay2_NUM.Focus()
                Exit Sub
            End If
        End If
    End Sub

    Private Sub txtToDay2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtToDay2_NUM.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtFromDay2_NUM.Text = "" Then
                txtToDay2_NUM.Text = ""
                Exit Sub
            End If
            If Not Val(txtToDay2_NUM.Text) > Val(txtFromDay2_NUM.Text) Then
                MsgBox("Range Should be > " + Val(txtFromDay2_NUM.Text).ToString, MsgBoxStyle.Exclamation)
                txtToDay2_NUM.Focus()
                Exit Sub
            End If
        End If
    End Sub


    Private Sub txtFromDay3_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFromDay3_NUM.GotFocus
        If Trim(txtToDay2_NUM.Text) <> "" Then txtFromDay3_NUM.Text = Val(txtToDay2_NUM.Text) + 1
    End Sub

    Private Sub txtFromDay3_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtFromDay3_NUM.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtFromDay2_NUM.Text = "" Then
                Exit Sub
            End If
            If Not Val(txtFromDay3_NUM.Text) > Val(txtToDay2_NUM.Text) Then
                MsgBox("Range Should be > " + Val(txtToDay2_NUM.Text).ToString, MsgBoxStyle.Exclamation)
                txtFromDay3_NUM.Focus()
                Exit Sub
            End If
        End If
    End Sub

    Private Sub txtToDay3_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtToDay3_NUM.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtFromDay3_NUM.Text = "" Then
                txtToDay3_NUM.Text = ""
                Exit Sub
            End If
            If Not Val(txtToDay3_NUM.Text) > Val(txtFromDay3_NUM.Text) Then
                MsgBox("Range Should be > " + Val(txtFromDay3_NUM.Text).ToString, MsgBoxStyle.Exclamation)
                txtToDay3_NUM.Focus()
                Exit Sub
            End If
        End If
    End Sub

    Private Sub txtFromDay4_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFromDay4_NUM.GotFocus
        If Trim(txtToDay3_NUM.Text) <> "" Then txtFromDay4_NUM.Text = Val(txtToDay3_NUM.Text) + 1
    End Sub

    Private Sub txtFromDay4_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtFromDay4_NUM.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtFromDay2_NUM.Text = "" Then
                Exit Sub
            End If
            If Not Val(txtFromDay4_NUM.Text) > Val(txtToDay3_NUM.Text) Then
                MsgBox("Range Should be > " + Val(txtToDay3_NUM.Text).ToString, MsgBoxStyle.Exclamation)
                txtFromDay4_NUM.Focus()
                Exit Sub
            End If
        End If
    End Sub

    Private Sub txtToDay4_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtToDay4_NUM.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtFromDay4_NUM.Text = "" Then
                txtToDay4_NUM.Text = ""
                Exit Sub
            End If
            If Not Val(txtToDay4_NUM.Text) > Val(txtFromDay4_NUM.Text) Then
                MsgBox("Range Should be > " + Val(txtFromDay4_NUM.Text).ToString, MsgBoxStyle.Exclamation)
                txtToDay4_NUM.Focus()
                Exit Sub
            End If
        End If
    End Sub

    Private Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        If Not CheckBckDays(userId, Me.Name, dtpAsOnDate.Value) Then dtpAsOnDate.Focus() : Exit Sub
        ResizeToolStripMenuItem1.Checked = False
        If (chkCounterWise.Checked = False And chkitemgrp.Checked = False And Chktablegrpwise.Checked = False And ChkDesignerGrp.Checked = False) Then
            MsgBox("Select any one group ", MsgBoxStyle.Information)
            chkCounterWise.Focus()
            Exit Sub
        End If

        If chkCmbCompany.Text = "" Then MsgBox("Company Should not Empty...", MsgBoxStyle.Information) : chkCmbCompany.Focus() : Exit Sub
        strSql = " IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "AGEDIFFDAYS') > 0 DROP TABLE TEMPTABLEDB..TEMP" & systemId & "AGEDIFFDAYS"
        strSql += " CREATE TABLE TEMPTABLEDB..TEMP" & systemId & "AGEDIFFDAYS"
        strSql += " ("
        strSql += " FROMDAY INT"
        strSql += " ,TODAY INT"
        strSql += " )"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strSql = ""
        If Not chkrangefrmmaster.Checked Then
            If Val(txtFromDay1_NUM.Text) < Val(txtToDay1_NUM.Text) Then
                strSql += " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "AGEDIFFDAYS(FROMDAY,TODAY)VALUES(" & Val(txtFromDay1_NUM.Text) & "," & Val(txtToDay1_NUM.Text) & ")"
            End If
            If Val(txtFromDay2_NUM.Text) < Val(txtToDay2_NUM.Text) Then
                strSql += " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "AGEDIFFDAYS(FROMDAY,TODAY)VALUES(" & Val(txtFromDay2_NUM.Text) & "," & Val(txtToDay2_NUM.Text) & ")"
            End If
            If Val(txtFromDay3_NUM.Text) < Val(txtToDay3_NUM.Text) Then
                strSql += " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "AGEDIFFDAYS(FROMDAY,TODAY)VALUES(" & Val(txtFromDay3_NUM.Text) & "," & Val(txtToDay3_NUM.Text) & ")"
            End If
            If Val(txtFromDay4_NUM.Text) < Val(txtToDay4_NUM.Text) Then
                strSql += " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "AGEDIFFDAYS(FROMDAY,TODAY)VALUES(" & Val(txtFromDay4_NUM.Text) & "," & Val(txtToDay4_NUM.Text) & ")"
            End If
        Else
            If dtrange.Rows.Count > 0 Then
                For i As Integer = 0 To dtrange.Rows.Count - 1
                    If Val(dtrange.Rows(i).Item("FROMDAY").ToString) < Val(dtrange.Rows(i).Item("TODAY").ToString) Then
                        strSql += " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "AGEDIFFDAYS(FROMDAY,TODAY)VALUES(" & Val(dtrange.Rows(i).Item("FROMDAY").ToString) & "," & Val(dtrange.Rows(i).Item("TODAY").ToString) & ")"
                    End If
                Next
            End If
        End If
        If strSql <> "" Then
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End If

        funcSearch()
        Prop_Sets()
    End Sub

    ''Private Sub gridView_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles gridView.CellFormatting
    ''    With gridView
    ''        Select Case .Rows(e.RowIndex).Cells("COLHEAD").Value.ToString
    ''            Case "T"
    ''                .Rows(e.RowIndex).Cells("PARTICULAR").Style.BackColor = reportHeadStyle.BackColor
    ''                .Rows(e.RowIndex).Cells("PARTICULAR").Style.Font = reportHeadStyle.Font
    ''            Case "S"
    ''                .Rows(e.RowIndex).DefaultCellStyle.Font = reportSubTotalStyle.Font
    ''        End Select
    ''    End With
    ''End Sub

    Function GridViewFormat() As Integer
        For Each dgvRow As DataGridViewRow In gridView_OWN.Rows
            With dgvRow
                Select Case .Cells("COLHEAD").Value.ToString
                    Case "S"
                        .DefaultCellStyle.Font = reportSubTotalStyle.Font
                        .DefaultCellStyle.ForeColor = reportSubTotalStyle.ForeColor
                    Case "T"
                        .Cells("PARTICULAR").Style.BackColor = reportHeadStyle.BackColor
                        .Cells("PARTICULAR").Style.Font = reportHeadStyle.Font
                    Case "B"
                        .Cells("PARTICULAR").Style.BackColor = Color.Orange
                        .Cells("PARTICULAR").Style.Font = reportHeadStyle.Font
                    Case "G"
                        .DefaultCellStyle.Font = reportTotalStyle.Font
                        .DefaultCellStyle.BackColor = reportTotalStyle.BackColor
                    Case "Z"
                        .DefaultCellStyle.Font = reportTotalStyle.Font
                        .DefaultCellStyle.BackColor = Color.Yellow
                End Select
            End With
        Next
    End Function
    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub gridView_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs) Handles gridView_OWN.ColumnWidthChanged
        If gridView_OWN.Rows.Count > 0 Then
            'funcGridHeadWidth()
        End If
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView_OWN.KeyPress
        If e.KeyChar = "X" Or e.KeyChar = "x" Then
            Me.btnExcel_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = "P" Then
            btnPrint_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = "D" Then
            ' MsgBox("Do You want Selected details ", MsgBoxStyle.YesNo, )
            DetailedView()
        End If
    End Sub
    Private Sub DataGridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = Keys.Escape Then
            DetailedView()
        ElseIf e.KeyCode = Keys.Enter Then
            Dim dgv As DataGridView = CType(sender, DataGridView)
            If dgv.Rows(dgv.CurrentRow.Index).Cells("TAGNO").Value.ToString <> "" Then
                If dgv.Columns.Contains("ITEMNAME") Then
                    strSql = "SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & dgv.Rows(dgv.CurrentRow.Index).Cells("ITEMNAME").Value.ToString & "'"
                Else
                    strSql = "SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & dgv.Rows(dgv.CurrentRow.Index).Cells("PARTICULAR").Value.ToString & "'"
                End If
                Dim ItemId As Integer = Val(objGPack.GetSqlValue(strSql, "ITEMID").ToString)
                Dim objTagViewer As New frmTagImageViewer(dgv.Rows(dgv.CurrentRow.Index).Cells("TAGNO").Value.ToString, ItemId)
                objTagViewer.ShowDialog()
            End If
        End If
    End Sub
    Private Sub DetailedView()
        Dim itemName As String = gridView_OWN.CurrentRow.Cells("ITEMNAME").Value.ToString

        Dim subItemName As String
        Dim Tablecode As String = ""
        Dim Designer As String = ""
        Dim Counter As String = ""

        Dim dayFilter As String = ""

        If gridView_OWN.Columns(gridView_OWN.CurrentCell.ColumnIndex).Name.ToString = "PCS1" Or gridView_OWN.Columns(gridView_OWN.CurrentCell.ColumnIndex).Name.ToString = "GRSWT1" Then
            dayFilter += vbCrLf + " AND DATEDIFF(D," & IIf(chkActualDate.Checked, "ACTUALRECDATE", "RECDATE") & ",'" & IIf(chkason.Checked, dtpAsOnDate.Value.ToString("yyyy-MM-dd"), dtpTo.Value.ToString("yyyy-MM-dd")) & "')+1 BETWEEN " & Val(txtFromDay1_NUM.Text.ToString) & " AND " & Val(txtToDay1_NUM.Text.ToString) & " "
        ElseIf gridView_OWN.Columns(gridView_OWN.CurrentCell.ColumnIndex).Name.ToString = "PCS2" Or gridView_OWN.Columns(gridView_OWN.CurrentCell.ColumnIndex).Name.ToString = "GRSWT2" Then
            dayFilter += vbCrLf + " AND DATEDIFF(D," & IIf(chkActualDate.Checked, "ACTUALRECDATE", "RECDATE") & ",'" & IIf(chkason.Checked, dtpAsOnDate.Value.ToString("yyyy-MM-dd"), dtpTo.Value.ToString("yyyy-MM-dd")) & "')+1 BETWEEN " & Val(txtFromDay2_NUM.Text.ToString) & " AND " & Val(txtToDay2_NUM.Text.ToString) & " "
        ElseIf gridView_OWN.Columns(gridView_OWN.CurrentCell.ColumnIndex).Name.ToString = "PCS3" Or gridView_OWN.Columns(gridView_OWN.CurrentCell.ColumnIndex).Name.ToString = "GRSWT3" Then
            dayFilter += vbCrLf + " AND DATEDIFF(D," & IIf(chkActualDate.Checked, "ACTUALRECDATE", "RECDATE") & ",'" & IIf(chkason.Checked, dtpAsOnDate.Value.ToString("yyyy-MM-dd"), dtpTo.Value.ToString("yyyy-MM-dd")) & "')+1 BETWEEN " & Val(txtFromDay3_NUM.Text.ToString) & " AND " & Val(txtToDay3_NUM.Text.ToString) & " "
        ElseIf gridView_OWN.Columns(gridView_OWN.CurrentCell.ColumnIndex).Name.ToString = "PCS4" Or gridView_OWN.Columns(gridView_OWN.CurrentCell.ColumnIndex).Name.ToString = "GRSWT4" Then
            dayFilter += vbCrLf + " AND DATEDIFF(D," & IIf(chkActualDate.Checked, "ACTUALRECDATE", "RECDATE") & ",'" & IIf(chkason.Checked, dtpAsOnDate.Value.ToString("yyyy-MM-dd"), dtpTo.Value.ToString("yyyy-MM-dd")) & "')+1 BETWEEN " & Val(txtFromDay4_NUM.Text.ToString) & " AND " & Val(txtToDay4_NUM.Text.ToString) & " "
        End If


        If gridView_OWN.Columns.Contains("SUBITEMNAME") = True Then subItemName = gridView_OWN.CurrentRow.Cells("SUBITEMNAME").Value.ToString Else subItemName = ""
        If gridView_OWN.Columns.Contains("DESIGNER") = True Then Designer = gridView_OWN.CurrentRow.Cells("DESIGNER").Value.ToString Else Designer = ""
        If gridView_OWN.Columns.Contains("ITEMCTRNAME") = True Then Counter = gridView_OWN.CurrentRow.Cells("ITEMCTRNAME").Value.ToString Else Counter = ""
        If gridView_OWN.Columns.Contains("TABLECODE") = True Then Tablecode = gridView_OWN.CurrentRow.Cells("TABLECODE").Value.ToString Else Tablecode = ""
        If itemName = "" And subItemName = "" Then Exit Sub

        Dim filtration As String = funcFiltration()
        Dim filtration2 As String = funcFiltration2()
        If subItemName <> "" Then
            If MessageBox.Show("Do you want to view item detail?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) <> Windows.Forms.DialogResult.Yes Then
                'subitem detail    
                filtration += " AND SUBITEMID = (SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & subItemName & "' and itemid = (select itemid from " & cnAdminDb & "..itemmast where itemname = '" & itemName & "'))"
            End If
        End If
        'item detail

        If MsgBox("Do you want Row Detail?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            filtration += " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & itemName & "')"
            dayFilter = ""
        Else
            filtration += dayFilter
        End If

        If Designer <> "" Then
            filtration += " AND DESIGNERID = (SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & Designer & "')"
        End If
        If Tablecode <> "" Then
            filtration += " AND TABLECODE = '" & Tablecode & "'"
        End If
        If Counter <> "" Then
            filtration += " AND ITEMCTRID =(SELECT TOP 1 ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME='" & Counter & "')"
        End If
        If chkCounterWise.Checked = False Then
            strSql = " IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "DETAGEANALYSIS') > 0 DROP TABLE TEMPTABLEDB..TEMP" & systemId & "DETAGEANALYSIS"
            strSql += vbCrLf + " CREATE TABLE TEMPTABLEDB..TEMP" & systemId & "DETAGEANALYSIS"
            strSql += vbCrLf + " ("
            strSql += vbCrLf + " PARTICULAR VARCHAR(200)"
            strSql += vbCrLf + " ,TAGNO VARCHAR(12)"
            strSql += vbCrLf + " ,PCS INT,GRSWT NUMERIC(15,3),NETWT NUMERIC(15,3),RECDATE VARCHAR(12),DAYS INT"
            strSql += vbCrLf + " ,COLHEAD VARCHAR(3)"
            strSql += vbCrLf + " ,SNO INT IDENTITY(1,1)"
            strSql += vbCrLf + " ,RESULT INT"
            strSql += vbCrLf + " )"
            strSql += vbCrLf + " "
            strSql += vbCrLf + " IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "DETAGE') > 0 DROP TABLE TEMPTABLEDB..TEMP" & systemId & "DETAGE"
            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " CASE WHEN SUBITEMID = 0 THEN (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)"
            strSql += vbCrLf + " ELSE (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = T.SUBITEMID) END AS PARTICULAR"
            strSql += vbCrLf + " ,TAGNO,PCS,GRSWT,NETWT,CONVERT(vARCHAR(10)," & IIf(chkActualDate.Checked, "ACTUALRECDATE", "RECDATE") & ",103)RECDATE,DATEDIFF(D," & IIf(chkActualDate.Checked, "ACTUALRECDATE", "RECDATE") & ",'" & IIf(chkason.Checked, dtpAsOnDate.Value.ToString("yyyy-MM-dd"), dtpTo.Value.ToString("yyyy-MM-dd")) & "')+1 AS DAYS,TAGVAL"
            strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "DETAGE"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T"
            strSql += filtration
            strSql += vbCrLf + " ORDER BY DAYS DESC,PARTICULAR"
            strSql += vbCrLf + " DECLARE @FROMDAY AS INT"
            strSql += vbCrLf + " DECLARE @TODAY AS INT"
            strSql += vbCrLf + " SELECT @FROMDAY = 0,@TODAY = 0"
            strSql += vbCrLf + " DECLARE CUR CURSOR"
            strSql += vbCrLf + " FOR SELECT FROMDAY,TODAY FROM TEMPTABLEDB..TEMP" & systemId & "AGEDIFFDAYS "
            strSql += vbCrLf + " OPEN CUR"
            strSql += vbCrLf + " WHILE 1=1"
            strSql += vbCrLf + " BEGIN"
            strSql += vbCrLf + " 	FETCH NEXT FROM CUR INTO @FROMDAY,@TODAY"
            strSql += vbCrLf + " 	IF @@FETCH_STATUS = -1 BREAK"
            strSql += vbCrLf + " 	/** Inserting Days Title **/"
            strSql += vbCrLf + " 	IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "DETAGE WHERE DAYS BETWEEN @FROMDAY AND @TODAY)>0"
            strSql += vbCrLf + " 	BEGIN"
            strSql += vbCrLf + " 	INSERT INTO TEMPTABLEDB..TEMP" & systemId & "DETAGEANALYSIS(PARTICULAR,COLHEAD,RESULT)"
            strSql += vbCrLf + " 	SELECT CONVERT(VARCHAR,@FROMDAY) + ' TO ' + CONVERT(VARCHAR,@TODAY),'T'COLHEAD,0 RESULT"
            strSql += vbCrLf + " 	END"
            strSql += vbCrLf + " 	/** Inserting Days Values **/"
            strSql += vbCrLf + " 	INSERT INTO TEMPTABLEDB..TEMP" & systemId & "DETAGEANALYSIS(PARTICULAR,TAGNO,PCS,GRSWT,NETWT,RECDATE,DAYS,RESULT)"
            strSql += vbCrLf + " 	SELECT PARTICULAR,TAGNO,PCS,GRSWT,NETWT,RECDATE,DAYS,1 RESULT"
            strSql += vbCrLf + " 	FROM TEMPTABLEDB..TEMP" & systemId & "DETAGE "
            strSql += vbCrLf + " 	WHERE DAYS BETWEEN @FROMDAY AND @TODAY"
            'strSql += vbCrLf + " 	ORDER BY TAGVAL"
            strSql += vbCrLf + " 	ORDER BY DAYS DESC,PARTICULAR"
            strSql += vbCrLf + " 	/** Inserting Sub Total **/"
            strSql += vbCrLf + " 	IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "DETAGE WHERE DAYS BETWEEN @FROMDAY AND @TODAY)>0"
            strSql += vbCrLf + " 	BEGIN"
            strSql += vbCrLf + " 		INSERT INTO TEMPTABLEDB..TEMP" & systemId & "DETAGEANALYSIS(PARTICULAR,PCS,GRSWT,NETWT,RESULT,COLHEAD)"
            strSql += vbCrLf + " 		SELECT CONVERT(VARCHAR,@FROMDAY) + ' TO ' + CONVERT(VARCHAR,@TODAY) + ' => '"
            strSql += vbCrLf + " 		+ CONVERT(VARCHAR,COUNT(*)) + ' Tags' AS PARTICULAR,SUM(PCS),SUM(GRSWT),SUM(NETWT),2 RESULT,'S'COLHEAD"
            strSql += vbCrLf + " 		FROM TEMPTABLEDB..TEMP" & systemId & "DETAGE "
            strSql += vbCrLf + " 		WHERE DAYS BETWEEN @FROMDAY AND @TODAY"
            strSql += vbCrLf + " 	END"
            strSql += vbCrLf + " END"
            strSql += vbCrLf + " CLOSE CUR"
            strSql += vbCrLf + " DEALLOCATE CUR"
            strSql += vbCrLf + " /** Above Given Days **/"
            strSql += vbCrLf + " /** Inserting Days Title **/"
            strSql += vbCrLf + " DECLARE @MAXDAY INT"
            strSql += vbCrLf + " SELECT @MAXDAY = ISNULL((SELECT MAX(TODAY) FROM TEMPTABLEDB..TEMP" & systemId & "AGEDIFFDAYS),0)"
            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "DETAGEANALYSIS(PARTICULAR,COLHEAD,RESULT)"
            strSql += vbCrLf + " SELECT '> ' + CONVERT(VARCHAR,@MAXDAY),'T'COLHEAD,0 RESULT"
            strSql += vbCrLf + " /** Inserting Days Values **/"
            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "DETAGEANALYSIS(PARTICULAR,TAGNO,PCS,GRSWT,NETWT,RECDATE,DAYS,RESULT)"
            strSql += vbCrLf + " SELECT PARTICULAR,TAGNO,PCS,GRSWT,NETWT,RECDATE,DAYS,1 RESULT"
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "DETAGE "
            strSql += vbCrLf + " WHERE DAYS > @MAXDAY"
            'strSql += vbCrLf + " ORDER BY TAGVAL"
            strSql += vbCrLf + " 	ORDER BY DAYS DESC,PARTICULAR"
            strSql += vbCrLf + " /** Inserting Sub Total **/"
            strSql += vbCrLf + " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "DETAGE WHERE DAYS > @MAXDAY)>0"
            strSql += vbCrLf + " BEGIN"
            strSql += vbCrLf + " 	INSERT INTO TEMPTABLEDB..TEMP" & systemId & "DETAGEANALYSIS(PARTICULAR,PCS,GRSWT,NETWT,RESULT,COLHEAD)"
            strSql += vbCrLf + " 	SELECT '> '+CONVERT(VARCHAR,@MAXDAY) + ' => '"
            strSql += vbCrLf + " 	+ CONVERT(VARCHAR,COUNT(*)) + ' Tags' AS PARTICULAR,SUM(PCS),SUM(GRSWT),SUM(NETWT),2 RESULT,'S'COLHEAD"
            strSql += vbCrLf + " 	FROM TEMPTABLEDB..TEMP" & systemId & "DETAGE "
            strSql += vbCrLf + " 	WHERE DAYS > @MAXDAY"
            strSql += vbCrLf + " END"
            strSql += vbCrLf + " /** Inserting Grand Total **/"
            strSql += vbCrLf + " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "DETAGEANALYSIS WHERE RESULT = 1)>0"
            strSql += vbCrLf + " BEGIN"
            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "DETAGEANALYSIS(PARTICULAR,PCS,GRSWT,NETWT,RESULT,COLHEAD)"
            strSql += vbCrLf + " SELECT 'GRAND TOTAL => ' + CONVERT(VARCHAR,COUNT(*)) + ' Tags' AS PARTICULAR"
            strSql += vbCrLf + " ,SUM(PCS),SUM(GRSWT),SUM(NETWT),3 RESULT,'G'COLHEAD FROM TEMPTABLEDB..TEMP" & systemId & "DETAGEANALYSIS WHERE RESULT = 1"
            strSql += vbCrLf + " END"

            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            If agestkview = True Then
                strSql = " ALTER TABLE TEMPTABLEDB..TEMP" & systemId & "DETAGEANALYSIS ADD SUBITEM VARCHAR(200)"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
                strSql = " update TEMPTABLEDB..TEMP" & systemId & "DETAGEANALYSIS SET SUBITEM=PARTICULAR "
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
                strSql = " SELECT * FROM TEMPTABLEDB..TEMP" & systemId & "DETAGEANALYSIS  WHERE RESULT='1'"
                Dim DTT As New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                DTT = New DataTable
                da.Fill(DTT)
                If DTT.Rows.Count > 0 Then
                    For I As Integer = 0 To DTT.Rows.Count - 1
                        strSql = "  update TEMPTABLEDB..TEMP" & systemId & "DETAGEANALYSIS SET PARTICULAR ="
                        strSql += vbCrLf + " (SELECT ITEMNAME FROM " & cnAdminDb & " ..ITEMMAST  WHERE ITEMID IN( "
                        strSql += vbCrLf + " SELECT ITEMID  FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME='" & DTT.Rows(I).Item("PARTICULAR").ToString & "'))"
                        strSql += vbCrLf + "  WHERE PARTICULAR='" & DTT.Rows(I).Item("PARTICULAR").ToString & "'"
                        strSql += vbCrLf + "  AND TAGNO='" & DTT.Rows(I).Item("TAGNO").ToString & "'"
                        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                        cmd.ExecuteNonQuery()
                    Next
                End If
                strSql = " update TEMPTABLEDB..TEMP" & systemId & "DETAGEANALYSIS SET SUBITEM=NULL WHERE RESULT <>'1' "
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
                strSql = " SELECT PARTICULAR,SUBITEM,TAGNO,PCS,GRSWT,NETWT ,RECDATE ,DAYS ,COLHEAD ,SNO,RESULT FROM TEMPTABLEDB..TEMP" & systemId & "DETAGEANALYSIS "
            Else
                strSql = " SELECT * FROM TEMPTABLEDB..TEMP" & systemId & "DETAGEANALYSIS "
            End If


            Dim dtGrid As New DataTable
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

            Dim objGridShower As New frmGridDispDia
            objGridShower.Name = Me.Name
            objGridShower.gridView.RowTemplate.Height = 21
            objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
            objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            AddHandler objGridShower.gridView.KeyDown, AddressOf DataGridView_KeyDown
            'objGridShower.Size = New Size(778, 550)
            objGridShower.Text = "DETAILED AGING ANALYSIS"
            Dim tit As String = Nothing
            If subItemName <> "" Then
                tit += subItemName.ToUpper
            Else
                tit += itemName.ToUpper
            End If
            If Counter <> "" Then tit += " (" & Counter.ToUpper & ") "
            If chkason.Checked Then
                tit += " DETAILED AGING ANALYSIS REPORT AS ON " & dtpAsOnDate.Text & "" + vbCrLf
            Else
                tit += " DETAILED AGING ANALYSIS REPORT BETWEEN " & dtpAsOnDate.Text & " AND " + dtpTo.Text + " " + vbCrLf
            End If


            objGridShower.lblTitle.Text = tit
            AddHandler objGridShower.gridView.ColumnWidthChanged, AddressOf gridView_ColumnWidthChanged
            objGridShower.StartPosition = FormStartPosition.CenterScreen
            objGridShower.dsGrid.DataSetName = objGridShower.Name
            objGridShower.dsGrid.Tables.Add(dtGrid)
            objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
            objGridShower.FormReSize = True
            objGridShower.FormReLocation = False
            objGridShower.pnlFooter.Visible = False
            objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            DataGridView_SummaryFormatting(objGridShower.gridView)
            FormatGridColumns(objGridShower.gridView, False, False, , False)

            objGridShower.Show()
            objGridShower.FormReSize = True
            FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
        Else
            If MsgBox("Do You want Selected details ", MsgBoxStyle.YesNo, ) = MsgBoxResult.Yes Then
                'strSql = Nothing
                strSql = "SELECT COUNT(*)CNT FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & gridView_OWN.CurrentRow.Cells("ITEMNAME").Value.ToString & "'"
                If objGPack.GetSqlValue(strSql, "CNT", 0) = 0 Then MsgBox("No Record Found") : Exit Sub
                strSql = " IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "DETAGEANALYSIS') > 0 DROP TABLE TEMPTABLEDB..TEMP" & systemId & "DETAGEANALYSIS"
                strSql += vbCrLf + " CREATE TABLE TEMPTABLEDB..TEMP" & systemId & "DETAGEANALYSIS"
                strSql += vbCrLf + " ("
                strSql += vbCrLf + " PARTICULAR VARCHAR(200)"
                strSql += vbCrLf + " ,TAGNO VARCHAR(12)"
                strSql += vbCrLf + " ,PCS INT,GRSWT NUMERIC(15,3),NETWT NUMERIC(15,3),RECDATE VARCHAR(12),DAYS INT"
                strSql += vbCrLf + " ,COLHEAD VARCHAR(3)"
                strSql += vbCrLf + " ,SNO INT IDENTITY(1,1)"
                strSql += vbCrLf + " ,RESULT INT"
                strSql += vbCrLf + " )"
                strSql += vbCrLf + " "
                strSql += vbCrLf + " IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "DETAGE') > 0 DROP TABLE TEMPTABLEDB..TEMP" & systemId & "DETAGE"
                strSql += vbCrLf + " SELECT "
                strSql += vbCrLf + " CASE WHEN SUBITEMID = 0 THEN (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)"
                strSql += vbCrLf + " ELSE (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = T.SUBITEMID) END AS PARTICULAR"
                strSql += vbCrLf + " ,TAGNO,PCS,GRSWT,NETWT,CONVERT(vARCHAR(10)," & IIf(chkActualDate.Checked, "ACTUALRECDATE", "RECDATE") & ",103)RECDATE,DATEDIFF(D," & IIf(chkActualDate.Checked, "ACTUALRECDATE", "RECDATE") & ",'" & IIf(chkason.Checked, dtpAsOnDate.Value.ToString("yyyy-MM-dd"), dtpTo.Value.ToString("yyyy-MM-dd")) & "')+1 AS DAYS,TAGVAL"
                strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "DETAGE"
                strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T"
                strSql += filtration
                strSql += vbCrLf + " ORDER BY DAYS DESC,PARTICULAR"
                strSql += vbCrLf + " DECLARE @FROMDAY AS INT"
                strSql += vbCrLf + " DECLARE @TODAY AS INT"
                strSql += vbCrLf + " SELECT @FROMDAY = 0,@TODAY = 0"
                strSql += vbCrLf + " DECLARE CUR CURSOR"
                strSql += vbCrLf + " FOR SELECT FROMDAY,TODAY FROM TEMPTABLEDB..TEMP" & systemId & "AGEDIFFDAYS"
                strSql += vbCrLf + " OPEN CUR"
                strSql += vbCrLf + " WHILE 1=1"
                strSql += vbCrLf + " BEGIN"
                strSql += vbCrLf + " 	FETCH NEXT FROM CUR INTO @FROMDAY,@TODAY"
                strSql += vbCrLf + " 	IF @@FETCH_STATUS = -1 BREAK"
                strSql += vbCrLf + " 	/** Inserting Days Title **/"
                strSql += vbCrLf + " 	IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "DETAGE WHERE DAYS BETWEEN @FROMDAY AND @TODAY)>0"
                strSql += vbCrLf + " 	BEGIN"
                strSql += vbCrLf + " 	INSERT INTO TEMPTABLEDB..TEMP" & systemId & "DETAGEANALYSIS(PARTICULAR,COLHEAD,RESULT)"
                strSql += vbCrLf + " 	SELECT CONVERT(VARCHAR,@FROMDAY) + ' TO ' + CONVERT(VARCHAR,@TODAY),'T'COLHEAD,0 RESULT"
                strSql += vbCrLf + " 	END"
                strSql += vbCrLf + " 	/** Inserting Days Values **/"
                strSql += vbCrLf + " 	INSERT INTO TEMPTABLEDB..TEMP" & systemId & "DETAGEANALYSIS(PARTICULAR,TAGNO,PCS,GRSWT,NETWT,RECDATE,DAYS,RESULT)"
                strSql += vbCrLf + " 	SELECT PARTICULAR,TAGNO,PCS,GRSWT,NETWT,RECDATE,DAYS,1 RESULT"
                strSql += vbCrLf + " 	FROM TEMPTABLEDB..TEMP" & systemId & "DETAGE "
                strSql += vbCrLf + " 	WHERE DAYS BETWEEN @FROMDAY AND @TODAY"
                'strSql += vbCrLf + " 	ORDER BY TAGVAL"
                strSql += vbCrLf + " ORDER BY DAYS DESC,PARTICULAR"
                strSql += vbCrLf + " 	/** Inserting Sub Total **/"
                strSql += vbCrLf + " 	IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "DETAGE WHERE DAYS BETWEEN @FROMDAY AND @TODAY)>0"
                strSql += vbCrLf + " 	BEGIN"
                strSql += vbCrLf + " 		INSERT INTO TEMPTABLEDB..TEMP" & systemId & "DETAGEANALYSIS(PARTICULAR,PCS,GRSWT,NETWT,RESULT,COLHEAD)"
                strSql += vbCrLf + " 		SELECT CONVERT(VARCHAR,@FROMDAY) + ' TO ' + CONVERT(VARCHAR,@TODAY) + ' => '"
                strSql += vbCrLf + " 		+ CONVERT(VARCHAR,COUNT(*)) + ' Tags' AS PARTICULAR,SUM(PCS),SUM(GRSWT),SUM(NETWT),2 RESULT,'S'COLHEAD"
                strSql += vbCrLf + " 		FROM TEMPTABLEDB..TEMP" & systemId & "DETAGE "
                strSql += vbCrLf + " 		WHERE DAYS BETWEEN @FROMDAY AND @TODAY"
                strSql += vbCrLf + " 	END"
                strSql += vbCrLf + " END"
                strSql += vbCrLf + " CLOSE CUR"
                strSql += vbCrLf + " DEALLOCATE CUR"
                strSql += vbCrLf + " /** Above Given Days **/"
                strSql += vbCrLf + " /** Inserting Days Title **/"
                strSql += vbCrLf + " DECLARE @MAXDAY INT"
                strSql += vbCrLf + " SELECT @MAXDAY = ISNULL((SELECT MAX(TODAY) FROM TEMPTABLEDB..TEMP" & systemId & "AGEDIFFDAYS),0)"
                strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "DETAGEANALYSIS(PARTICULAR,COLHEAD,RESULT)"
                strSql += vbCrLf + " SELECT '> ' + CONVERT(VARCHAR,@MAXDAY),'T'COLHEAD,0 RESULT"
                strSql += vbCrLf + " /** Inserting Days Values **/"
                strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "DETAGEANALYSIS(PARTICULAR,TAGNO,PCS,GRSWT,NETWT,RECDATE,DAYS,RESULT)"
                strSql += vbCrLf + " SELECT PARTICULAR,TAGNO,PCS,GRSWT,NETWT,RECDATE,DAYS,1 RESULT"
                strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "DETAGE "
                strSql += vbCrLf + " WHERE DAYS > @MAXDAY"
                'strSql += vbCrLf + " ORDER BY TAGVAL"
                strSql += vbCrLf + " ORDER BY DAYS DESC,PARTICULAR"
                strSql += vbCrLf + " /** Inserting Sub Total **/"
                strSql += vbCrLf + " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "DETAGE WHERE DAYS > @MAXDAY)>0"
                strSql += vbCrLf + " BEGIN"
                strSql += vbCrLf + " 	INSERT INTO TEMPTABLEDB..TEMP" & systemId & "DETAGEANALYSIS(PARTICULAR,PCS,GRSWT,NETWT,RESULT,COLHEAD)"
                strSql += vbCrLf + " 	SELECT '> '+CONVERT(VARCHAR,@MAXDAY) + ' => '"
                strSql += vbCrLf + " 	+ CONVERT(VARCHAR,COUNT(*)) + ' Tags' AS PARTICULAR,SUM(PCS),SUM(GRSWT),SUM(NETWT),2 RESULT,'S'COLHEAD"
                strSql += vbCrLf + " 	FROM TEMPTABLEDB..TEMP" & systemId & "DETAGE "
                strSql += vbCrLf + " 	WHERE DAYS > @MAXDAY"
                strSql += vbCrLf + " END"
                strSql += vbCrLf + " /** Inserting Grand Total **/"
                strSql += vbCrLf + " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "DETAGEANALYSIS WHERE RESULT = 1)>0"
                strSql += vbCrLf + " BEGIN"
                strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "DETAGEANALYSIS(PARTICULAR,PCS,GRSWT,NETWT,RESULT,COLHEAD)"
                strSql += vbCrLf + " SELECT 'GRAND TOTAL => ' + CONVERT(VARCHAR,COUNT(*)) + ' Tags' AS PARTICULAR"
                strSql += vbCrLf + " ,SUM(PCS),SUM(GRSWT),SUM(NETWT),3 RESULT,'G'COLHEAD FROM TEMPTABLEDB..TEMP" & systemId & "DETAGEANALYSIS AS A"
                'strSql += vbCrLf + " INNER JOIN TEMPTABLEDB..TEMP" & systemId & "AGEDIFFDAYS AS B ON 1=1 "
                strSql += vbCrLf + " WHERE RESULT=1 "
                'strSql += vbCrLf + " AND A.DAYS BETWEEN B.FROMDAY AND B.TODAY "
                strSql += vbCrLf + " END"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
                If agestkview = True Then
                    strSql = " ALTER TABLE TEMPTABLEDB..TEMP" & systemId & "DETAGEANALYSIS ADD SUBITEM VARCHAR(200)"
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                    cmd.ExecuteNonQuery()
                    strSql = " update TEMPTABLEDB..TEMP" & systemId & "DETAGEANALYSIS SET SUBITEM=PARTICULAR "
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                    cmd.ExecuteNonQuery()
                    strSql = " SELECT * FROM TEMPTABLEDB..TEMP" & systemId & "DETAGEANALYSIS  WHERE RESULT='1'"
                    Dim DTT As New DataTable
                    da = New OleDbDataAdapter(strSql, cn)
                    DTT = New DataTable
                    da.Fill(DTT)
                    If DTT.Rows.Count > 0 Then
                        For I As Integer = 0 To DTT.Rows.Count - 1
                            strSql = "  update TEMPTABLEDB..TEMP" & systemId & "DETAGEANALYSIS SET PARTICULAR ="
                            strSql += vbCrLf + " (SELECT ITEMNAME FROM " & cnAdminDb & " ..ITEMMAST  WHERE ITEMID IN( "
                            strSql += vbCrLf + " SELECT ITEMID  FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME='" & DTT.Rows(I).Item("PARTICULAR").ToString & "'))"
                            strSql += vbCrLf + "  WHERE PARTICULAR='" & DTT.Rows(I).Item("PARTICULAR").ToString & "'"
                            strSql += vbCrLf + "  AND TAGNO='" & DTT.Rows(I).Item("TAGNO").ToString & "'"
                            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                            cmd.ExecuteNonQuery()
                        Next
                    End If
                    strSql = " update TEMPTABLEDB..TEMP" & systemId & "DETAGEANALYSIS SET SUBITEM=NULL WHERE RESULT <>'1' "
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                    cmd.ExecuteNonQuery()
                    strSql = " SELECT PARTICULAR,SUBITEM,TAGNO,PCS,GRSWT,NETWT ,RECDATE ,DAYS ,COLHEAD ,SNO,RESULT FROM TEMPTABLEDB..TEMP" & systemId & "DETAGEANALYSIS "
                Else
                    strSql = " SELECT * FROM TEMPTABLEDB..TEMP" & systemId & "DETAGEANALYSIS "
                End If
                Dim dtGrid As New DataTable
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

                Dim objGridShower As New frmGridDispDia
                objGridShower.Name = Me.Name
                objGridShower.gridView.RowTemplate.Height = 21
                objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
                objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                AddHandler objGridShower.gridView.KeyDown, AddressOf DataGridView_KeyDown
                'objGridShower.Size = New Size(778, 550)
                objGridShower.Text = "DETAILED AGING ANALYSIS"
                Dim tit As String = Nothing
                If subItemName <> "" Then
                    tit += subItemName.ToUpper
                Else
                    tit += itemName.ToUpper
                End If
                If Counter <> "" Then tit += " (" & Counter.ToUpper & ") "
                If chkason.Checked Then
                    tit += " DETAILED AGING ANALYSIS REPORT AS ON " & dtpAsOnDate.Text & "" + vbCrLf
                Else
                    tit += " DETAILED AGING ANALYSIS REPORT BETWEEN " & dtpAsOnDate.Text & " AND " + dtpTo.Text + " " + vbCrLf
                End If


                objGridShower.lblTitle.Text = tit
                AddHandler objGridShower.gridView.ColumnWidthChanged, AddressOf gridView_ColumnWidthChanged
                objGridShower.StartPosition = FormStartPosition.CenterScreen
                objGridShower.dsGrid.DataSetName = objGridShower.Name
                objGridShower.dsGrid.Tables.Add(dtGrid)
                objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
                objGridShower.FormReSize = True
                objGridShower.FormReLocation = False
                objGridShower.pnlFooter.Visible = False
                objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                DataGridView_SummaryFormatting(objGridShower.gridView)
                FormatGridColumns(objGridShower.gridView, False, False, , False)

                objGridShower.Show()
                objGridShower.FormReSize = True
                FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
                Exit Sub
            Else
                gridView_OWN.ClearSelection()
                strSql = String.Empty
                strSql = " IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "DETAGEANALYSIS2') > 0 DROP TABLE TEMPTABLEDB..TEMP" & systemId & "DETAGEANALYSIS2"
                strSql += vbCrLf + " CREATE TABLE TEMPTABLEDB..TEMP" & systemId & "DETAGEANALYSIS2"
                strSql += vbCrLf + " ("
                strSql += vbCrLf + " PARTICULAR VARCHAR(200),ITEMCOUNTER VARCHAR(200),ITEMNAME VARCHAR(100),SUBITEMNAME varchar(100)"
                strSql += vbCrLf + " ,TAGNO VARCHAR(12)"
                strSql += vbCrLf + " ,PCS INT,GRSWT NUMERIC(15,3),NETWT NUMERIC(15,3),RECDATE VARCHAR(12),DAYS INT"
                strSql += vbCrLf + " ,COLHEAD VARCHAR(3)"
                strSql += vbCrLf + " ,SNO INT IDENTITY(1,1)"
                strSql += vbCrLf + " ,RESULT INT"
                strSql += vbCrLf + " )"
                strSql += vbCrLf + " "
                strSql += vbCrLf + " IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "DETAGE2') > 0 DROP TABLE TEMPTABLEDB..TEMP" & systemId & "DETAGE2"
                strSql += vbCrLf + " SELECT "
                strSql += vbCrLf + " CASE WHEN SUBITEMID = 0 THEN (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)"
                strSql += vbCrLf + " ELSE (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = T.SUBITEMID) END AS PARTICULAR"
                strSql += vbCrLf + " ,(SELECT SUBITEMNAME FROM  " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = T.SUBITEMID) as SUBITEMNAME"
                strSql += vbCrLf + " ,(SELECT ITEMCTRNAME FROM  " & cnAdminDb & "..ITEMCOUNTER WHERE itemCtrID = T.itemCtrID) AS ITEMCOUNTER,"
                strSql += vbCrLf + " (SELECT ITEMNAME FROM  " & cnAdminDb & "..ITEMMAST WHERE  ITEMID=T.ITEMID ) AS ITEMNAME"
                strSql += vbCrLf + " ,TAGNO,PCS,GRSWT,NETWT,CONVERT(vARCHAR(10)," & IIf(chkActualDate.Checked, "ACTUALRECDATE", "RECDATE") & ",103)RECDATE,DATEDIFF(D," & IIf(chkActualDate.Checked, "ACTUALRECDATE", "RECDATE") & ",'" & IIf(chkason.Checked, dtpAsOnDate.Value.ToString("yyyy-MM-dd"), dtpTo.Value.ToString("yyyy-MM-dd")) & "')+1 AS DAYS,TAGVAL "
                strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "DETAGE2"
                strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T"
                strSql += filtration2
                strSql += vbCrLf + " DECLARE @FROMDAY AS INT"
                strSql += vbCrLf + " DECLARE @TODAY AS INT"
                strSql += vbCrLf + " SELECT @FROMDAY = 0,@TODAY = 0"
                strSql += vbCrLf + " DECLARE CUR CURSOR"
                strSql += vbCrLf + " FOR SELECT FROMDAY,TODAY FROM TEMPTABLEDB..TEMP" & systemId & "AGEDIFFDAYS"
                strSql += vbCrLf + " OPEN CUR"
                strSql += vbCrLf + " WHILE 1=1"
                strSql += vbCrLf + " BEGIN"
                strSql += vbCrLf + " 	FETCH NEXT FROM CUR INTO @FROMDAY,@TODAY"
                strSql += vbCrLf + " 	IF @@FETCH_STATUS = -1 BREAK"
                strSql += vbCrLf + " 	/** Inserting Days Title **/"
                strSql += vbCrLf + "    /** Inserting Days Title **/"
                strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "DETAGEANALYSIS2(ITEMNAME,ITEMCOUNTER,PARTICULAR,COLHEAD,RESULT)"
                strSql += vbCrLf + " SELECT  DISTINCT ITEMNAME,ITEMCOUNTER,CONVERT(VARCHAR,@FROMDAY) + ' TO ' + CONVERT(VARCHAR,@TODAY),'T'COLHEAD,0 RESULT FROM TEMPTABLEDB..TEMP" & systemId & "DETAGE2 GROUP BY ITEMNAME,ITEMCOUNTER"
                strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "DETAGEANALYSIS2 (PARTICULAR,ITEMCOUNTER,ITEMNAME,COLHEAD,RESULT)"
                strSql += vbCrLf + " SELECT DISTINCT ITEMCOUNTER AS PARTICULAR,ITEMCOUNTER,ITEMNAME,'T1',1 RESULT FROM TEMPTABLEDB..TEMP" & systemId & "DETAGE2 "
                strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "DETAGEANALYSIS2 (PARTICULAR,ITEMNAME,ITEMCOUNTER,COLHEAD,RESULT)"
                strSql += vbCrLf + " SELECT DISTINCT ITEMNAME AS PARTICULAR,ITEMNAME ,ITEMCOUNTER,'T2',2 RESULT FROM TEMPTABLEDB..TEMP" & systemId & "DETAGE2 "
                strSql += vbCrLf + " /** Inserting Days Values **/"
                strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "DETAGEANALYSIS2(PARTICULAR,ITEMCOUNTER,ITEMNAME,SUBITEMNAME,TAGNO,PCS,GRSWT,NETWT,RECDATE,DAYS,RESULT)"
                strSql += vbCrLf + " SELECT PARTICULAR,ITEMCOUNTER,ITEMNAME,SUBITEMNAME,TAGNO,PCS,GRSWT,NETWT,RECDATE,DAYS,3 RESULT "
                strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "DETAGE2"
                strSql += vbCrLf + " WHERE DAYS BETWEEN @FROMDAY AND @TODAY  "
                strSql += vbCrLf + " ORDER BY TAGVAL"
                strSql += vbCrLf + " /** Inserting Sub Total **/"
                strSql += vbCrLf + " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "DETAGE2 WHERE DAYS BETWEEN @FROMDAY AND @TODAY)>0"
                strSql += vbCrLf + " BEGIN"
                strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "DETAGEANALYSIS2(PARTICULAR,ITEMNAME,ITEMCOUNTER,PCS,GRSWT,NETWT,RESULT,COLHEAD)"
                strSql += vbCrLf + " SELECT CONVERT(VARCHAR,@FROMDAY) + ' TO ' + CONVERT(VARCHAR,@TODAY) + ' => '"
                strSql += vbCrLf + " + CONVERT(VARCHAR,COUNT(*)) + ' Tags' AS PARTICULAR,ITEMNAME,ITEMCOUNTER,SUM(PCS),SUM(GRSWT),SUM(NETWT),4 RESULT,'S'COLHEAD"
                strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "DETAGE2"
                strSql += vbCrLf + " WHERE DAYS BETWEEN @FROMDAY AND @TODAY 	GROUP BY ITEMNAME,ITEMCOUNTER"
                strSql += vbCrLf + " End"
                strSql += vbCrLf + "  End"
                strSql += vbCrLf + " Close CUR"
                strSql += vbCrLf + " DEALLOCATE CUR"
                strSql += vbCrLf + " /** Above Given Days **/"
                strSql += vbCrLf + " /** Inserting Days Title **/"
                strSql += vbCrLf + " DECLARE @MAXDAY INT"
                strSql += vbCrLf + " SELECT @MAXDAY = ISNULL((SELECT MAX(TODAY) FROM TEMPTABLEDB..TEMP" & systemId & "AGEDIFFDAYS),0)"
                strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "DETAGEANALYSIS2(ITEMNAME,ITEMCOUNTER,PARTICULAR,COLHEAD,RESULT)"
                strSql += vbCrLf + " SELECT DISTINCT ITEMNAME,ITEMCOUNTER,'> ' + CONVERT(VARCHAR,@MAXDAY), 'T'COLHEAD,0 RESULT FROM TEMPTABLEDB..TEMP" & systemId & "DETAGE2  GROUP BY ITEMNAME,ITEMCOUNTER"
                strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "DETAGEANALYSIS2 (PARTICULAR,ITEMCOUNTER,ITEMNAME,COLHEAD,RESULT)"
                strSql += vbCrLf + " SELECT DISTINCT ITEMCOUNTER AS PARTICULAR ,ITEMCOUNTER,ITEMNAME,'T1',1 RESULT FROM TEMPTABLEDB..TEMP" & systemId & "DETAGE2"
                strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "DETAGEANALYSIS2 (PARTICULAR,ITEMNAME,ITEMCOUNTER,COLHEAD,RESULT)"
                strSql += vbCrLf + " SELECT DISTINCT ITEMNAME AS PARTICULAR ,ITEMNAME,ITEMCOUNTER,'T2',2 RESULT FROM TEMPTABLEDB..TEMP" & systemId & "DETAGE2 "
                strSql += vbCrLf + " /** Inserting Days Values **/"
                strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "DETAGEANALYSIS2(PARTICULAR,ITEMCOUNTER,ITEMNAME,SUBITEMNAME,TAGNO,PCS,GRSWT,NETWT,RECDATE,DAYS,RESULT)"
                strSql += vbCrLf + " SELECT PARTICULAR,ITEMCOUNTER,ITEMNAME,SUBITEMNAME,TAGNO,PCS,GRSWT,NETWT,RECDATE,DAYS,3 RESULT "
                strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "DETAGE2"
                strSql += vbCrLf + " WHERE DAYS > @MAXDAY "
                strSql += vbCrLf + " ORDER BY TAGVAL"
                strSql += vbCrLf + " /** Inserting Sub Total **/"
                strSql += vbCrLf + " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "DETAGE2 WHERE DAYS > @MAXDAY)>0"
                strSql += vbCrLf + " BEGIN"
                strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "DETAGEANALYSIS2(PARTICULAR,ITEMNAME,ITEMCOUNTER,PCS,GRSWT,NETWT,RESULT,COLHEAD)"
                strSql += vbCrLf + " SELECT '> '+CONVERT(VARCHAR,@MAXDAY) + ' => '"
                strSql += vbCrLf + " + CONVERT(VARCHAR,COUNT(*)) + ' Tags' AS PARTICULAR,ITEMNAME,ITEMCOUNTER,SUM(PCS),SUM(GRSWT),SUM(NETWT),4 RESULT,'S'COLHEAD"
                strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "DETAGE2"
                strSql += vbCrLf + " WHERE DAYS > @MAXDAY GROUP BY ITEMNAME,ITEMCOUNTER"
                strSql += vbCrLf + " End"
                strSql += vbCrLf + " /** Inserting Grand Total **/"
                strSql += vbCrLf + " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "DETAGEANALYSIS2 WHERE RESULT = 3)>0"
                strSql += vbCrLf + "  BEGIN"
                strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "DETAGEANALYSIS2(PARTICULAR,ITEMNAME,ITEMCOUNTER,PCS,GRSWT,NETWT,RESULT,COLHEAD)"
                strSql += vbCrLf + " SELECT 'GRAND TOTAL => ' + CONVERT(VARCHAR,COUNT(*)) + ' Tags' AS PARTICULAR,'ZZZZ','ZZZZ'"
                strSql += vbCrLf + " ,SUM(PCS),SUM(GRSWT),SUM(NETWT),5 RESULT,'G'COLHEAD FROM TEMPTABLEDB..TEMP" & systemId & "DETAGE2"
                strSql += vbCrLf + " End"


                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                strSql = " SELECT * FROM TEMPTABLEDB..TEMP" & systemId & "DETAGEANALYSIS2 ORDER BY ITEMCOUNTER,ITEMNAME,SNO,SUBITEMNAME"
                Dim dtGrid As New DataTable
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
                Dim objGridShower As New frmGridDispDia
                objGridShower.Name = Me.Name
                objGridShower.gridView.RowTemplate.Height = 21
                objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
                objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                'objGridShower.Size = New Size(778, 550)
                AddHandler objGridShower.gridView.KeyDown, AddressOf DataGridView_KeyDown
                objGridShower.Text = "DETAILED AGING ANALYSIS"
                Dim tit As String = Nothing
                'If subItemName <> "" Then
                '    tit += subItemName.ToUpper
                'Else
                '    tit += itemName.ToUpper
                'End If
                'If Counter <> "" Then tit += " (" & Counter.ToUpper & ") "
                If chkason.Checked Then
                    tit += " DETAILED AGING ANALYSIS REPORT AS ON " & dtpAsOnDate.Text & "" + vbCrLf
                Else
                    tit += " DETAILED AGING ANALYSIS REPORT BETWEEN " & dtpAsOnDate.Text & " AND " + dtpTo.Text + " " + vbCrLf
                End If


                objGridShower.lblTitle.Text = tit
                AddHandler objGridShower.gridView.ColumnWidthChanged, AddressOf gridView_ColumnWidthChanged
                objGridShower.StartPosition = FormStartPosition.CenterScreen
                objGridShower.dsGrid.DataSetName = objGridShower.Name
                objGridShower.dsGrid.Tables.Add(dtGrid)
                objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
                objGridShower.FormReSize = True
                objGridShower.FormReLocation = False
                objGridShower.pnlFooter.Visible = False
                objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)

                DataGridView_SummaryFormatting1(objGridShower.gridView)
                FormatGridColumns(objGridShower.gridView, False, False, , False)

                objGridShower.Show()
                objGridShower.FormReSize = True
                FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
            End If
        End If
    End Sub

    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView)
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                dgvCol.Visible = True
            Next
            For cnt As Integer = 7 To dgv.ColumnCount - 1
                .Columns(cnt).Visible = False
            Next
            .Columns("PARTICULAR").Width = 350
            .Columns("TAGNO").Width = 120
            .Columns("PCS").Width = 80
            .Columns("GRSWT").Width = 120
            .Columns("NETWT").Width = 120
            .Columns("RECDATE").Width = 100
            .Columns("DAYS").Width = 80
            FormatGridColumns(dgv, False, False, , False)
            FillGridGroupStyle_KeyNoWise(dgv)
        End With

    End Sub
    Private Sub DataGridView_SummaryFormatting1(ByVal dgv As DataGridView)
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                dgvCol.Visible = True
            Next
            For cnt As Integer = 7 To dgv.ColumnCount - 1
                .Columns(cnt).Visible = False
            Next
            .Columns("PARTICULAR").Width = 250
            .Columns("TAGNO").Width = 100
            .Columns("PCS").Width = 80
            .Columns("GRSWT").Width = 100
            .Columns("NETWT").Width = 100
            .Columns("RECDATE").Width = 80
            .Columns("DAYS").Width = 80
            .Columns("DAYS").Visible = True
            .Columns("ITEMNAME").Visible = False
            .Columns("SUBITEMNAME").Visible = False
            .Columns("ITEMCOUNTER").Visible = False
            FormatGridColumns(dgv, False, False, , False)
            FillGridGroupStyle_KeyNoWise(dgv)
        End With
    End Sub


    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView_OWN.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView_OWN, BrightPosting.GExport.GExportType.Print, gridViewHead)
        End If
    End Sub

    Private Sub frmAgeWiseAnalysis_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        ElseIf e.KeyChar = Chr(Keys.Escape) And tabMain.SelectedTab.Name = tabView.Name Then
            btnBack_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        dtpAsOnDate.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        'objGPack.TextClear(Me)
        chkason.Checked = True
        'chkWithStone.Checked = False
        grprangedetail.Visible = False
        chkrangefrmmaster.Checked = False
        chkason.Focus()
        chkason.Select()
        'cmbCostCentre.Text = IIf(cnDefaultCostId, "ALL", cnCostName)
        Prop_Gets()
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGen
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView_OWN.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView_OWN, BrightPosting.GExport.GExportType.Export, gridViewHead)
        End If
    End Sub

    Private Sub gridView_Scroll(ByVal sender As Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles gridView_OWN.Scroll
        Try
            If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
                gridViewHead.HorizontalScrollingOffset = e.NewValue
                gridViewHead.Columns("SCROLL").Visible = CType(gridView_OWN.Controls(1), VScrollBar).Visible
                gridViewHead.Columns("SCROLL").Width = CType(gridView_OWN.Controls(1), VScrollBar).Width
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New frmAgeWiseAnalysis_Properties
        obj.p_cmbCategory = cmbCategory.Text
        obj.p_cmbItem = cmbItem.Text
        obj.p_cmbmetal = cmbmetal.Text
        obj.p_cmbSubItem = cmbSubItem.Text
        obj.p_cmbDesigner = cmbDesigner.Text
        obj.p_cmbCounter = cmbCounter.Text
        'obj.p_cmbCostCentre = cmbCostCentre.Text
        obj.p_txtFromDay1_NUM = txtFromDay1_NUM.Text
        obj.p_txtToDay1_NUM = txtToDay1_NUM.Text
        obj.p_txtFromDay2_NUM = txtFromDay2_NUM.Text
        obj.p_txtToDay2_NUM = txtToDay2_NUM.Text
        obj.p_txtFromDay3_NUM = txtFromDay3_NUM.Text
        obj.p_txtToDay3_NUM = txtToDay3_NUM.Text
        obj.p_txtFromDay4_NUM = txtFromDay4_NUM.Text
        obj.p_txtToDay4_NUM = txtToDay4_NUM.Text
        obj.p_chkWithStone = chkWithStone.Checked
        obj.p_chkCounterWise = chkCounterWise.Checked
        obj.p_chkitemgrp = chkitemgrp.Checked
        obj.p_chktablegrp = Chktablegrpwise.Checked
        obj.p_chkDesignergrp = ChkDesignerGrp.Checked
        obj.p_chkrangefrmmaster = chkrangefrmmaster.Checked
        obj.p_chkSubItem = chkSubItem.Checked
        obj.p_chkActualDate = chkActualDate.Checked
        GetChecked_CheckedList(chkCmbtablecode, obj.p_chkCmbtablecode)
        SetSettingsObj(obj, Me.Name, GetType(frmAgeWiseAnalysis_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmAgeWiseAnalysis_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmAgeWiseAnalysis_Properties))
        cmbCategory.Text = obj.p_cmbCategory
        cmbItem.Text = obj.p_cmbItem
        cmbmetal.Text = obj.p_cmbmetal
        cmbSubItem.Text = obj.p_cmbSubItem
        cmbDesigner.Text = obj.p_cmbDesigner
        cmbCounter.Text = obj.p_cmbCounter
        'cmbCostCentre.Text = obj.p_cmbCostCentre
        txtFromDay1_NUM.Text = obj.p_txtFromDay1_NUM
        txtToDay1_NUM.Text = obj.p_txtToDay1_NUM
        txtFromDay2_NUM.Text = obj.p_txtFromDay2_NUM
        txtToDay2_NUM.Text = obj.p_txtToDay2_NUM
        txtFromDay3_NUM.Text = obj.p_txtFromDay3_NUM
        txtToDay3_NUM.Text = obj.p_txtToDay3_NUM
        txtFromDay4_NUM.Text = obj.p_txtFromDay4_NUM
        txtToDay4_NUM.Text = obj.p_txtToDay4_NUM
        chkWithStone.Checked = obj.p_chkWithStone
        chkCounterWise.Checked = obj.p_chkCounterWise
        chkitemgrp.Checked = obj.p_chkitemgrp
        Chktablegrpwise.Checked = obj.p_chktablegrp
        ChkDesignerGrp.Checked = obj.p_chkDesignergrp
        chkrangefrmmaster.Checked = obj.p_chkrangefrmmaster
        chkSubItem.Checked = obj.p_chkSubItem
        chkActualDate.Checked = obj.p_chkActualDate
        SetChecked_CheckedList(chkCmbtablecode, obj.p_chkCmbtablecode, "ALL")
        If chkSubItem.Checked = False Then
            cmbSubItem.Text = "ALL"
            cmbSubItem.Enabled = False
        End If
    End Sub

    Private Sub chkSubItem_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSubItem.CheckedChanged
        If chkSubItem.Checked = False Then
            cmbSubItem.Text = "ALL"
            cmbSubItem.Enabled = False
        Else
            cmbSubItem.Enabled = True
        End If
    End Sub

    Private Sub chkason_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkason.CheckedChanged
        If chkason.Checked = True Then
            chkason.Text = "&As OnDate"
            lblto.Enabled = False
            dtpTo.Enabled = False
        Else
            chkason.Text = "&Date From"
            lblto.Enabled = True
            dtpTo.Enabled = True
        End If
    End Sub

    Private Sub chkrangefrmmaster_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkrangefrmmaster.CheckedChanged
        If chkrangefrmmaster.Checked Then
            If GetRange() Then
                rangepanel.Enabled = False
                grprangedetail.Visible = True
            Else
                MsgBox("No Ranges are available belong to this item.", MsgBoxStyle.Information, "Brighttech Message.")
                grprangedetail.Visible = False
                chkrangefrmmaster.Checked = False
            End If
        Else
            grprangedetail.Visible = False
            gridrange.DataSource = Nothing
            rangepanel.Enabled = True
        End If
    End Sub
    Private Function GetRange() As Boolean
        Dim itemid As String = ""
        If cmbItem.Text <> "ALL" Then
            itemid = GetSqlValue(cn, " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbItem.Text.ToString & "'")
        Else
            itemid = 0
        End If
        strSql = "SELECT FROMDAY,TODAY FROM " & cnAdminDb & "..STKREORDER WHERE FROMFLAG='D' AND ITEMID='" & itemid & "' ORDER BY FROMDAY"
        dtrange = New DataTable()
        dtrange.Columns.Add("Check", Type.GetType("System.Boolean"))
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtrange)
        If dtrange.Rows.Count > 0 Then
            gridrange.DataSource = Nothing
            gridrange.DataSource = dtrange
            For i As Integer = 0 To gridrange.Rows.Count - 1
                gridrange.Rows(i).Cells("Check").Value = True
            Next
            gridrange.Columns(0).Width = 20
            gridrange.Columns(1).Width = 86
            gridrange.Columns(2).Width = 86
            gridrange.Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomRight
            gridrange.Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomRight
            gridrange.Columns(1).DefaultCellStyle.BackColor = Color.LightGreen
            gridrange.Columns(1).DefaultCellStyle.ForeColor = Color.Red
            gridrange.Columns(2).DefaultCellStyle.BackColor = Color.Khaki
            gridrange.Columns(2).DefaultCellStyle.ForeColor = Color.Black
            gridrange.Columns(1).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            gridrange.Columns(2).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            gridrange.Columns(0).HeaderText = ""
            gridrange.Columns(1).ReadOnly = True
            gridrange.Columns(2).ReadOnly = True
            gridrange.Focus()
            chkrangefrmmaster.Focus()
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub cmbmetal_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub cmbmetal_SelectedIndexChanged1(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbmetal.SelectedIndexChanged
        If cmbmetal.Text <> "ALL" Then
            cmbItem.Items.Clear()
            cmbItem.Items.Add("ALL")
            strSql = "SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME='" & Trim(cmbmetal.Text.ToString) & "'"
            Dim metalid = GetSqlValue(cn, strSql)
            strSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE METALID='" & metalid & "'"
            strSql = strSql + " ORDER BY ITEMNAME"
            objGPack.FillCombo(strSql, cmbItem, False)
            cmbItem.Text = "ALL"
        Else
            cmbItem.Items.Clear()
            cmbItem.Items.Add("ALL")
            strSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST ORDER BY ITEMNAME"
            objGPack.FillCombo(strSql, cmbItem, False)
            cmbItem.Text = "ALL"
        End If

    End Sub

    Private Sub chkitemgrp_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkitemgrp.CheckedChanged
        If chkitemgrp.Checked = True Then
            chkCounterWise.Checked = False
            Chktablegrpwise.Checked = False
            ChkDesignerGrp.Checked = False
        End If
    End Sub

    Private Sub chkCounterWise_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCounterWise.CheckedChanged
        If chkCounterWise.Checked = True Then
            chkitemgrp.Checked = False
            Chktablegrpwise.Checked = False
            ChkDesignerGrp.Checked = False
        End If
    End Sub


    Private Sub Chktablegrpwise_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Chktablegrpwise.CheckedChanged
        If Chktablegrpwise.Checked = True Then
            chkitemgrp.Checked = False
            chkCounterWise.Checked = False
            ChkDesignerGrp.Checked = False
        End If
    End Sub

    Private Sub ChkDesignerGrp_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkDesignerGrp.CheckedChanged
        If ChkDesignerGrp.Checked = True Then
            chkitemgrp.Checked = False
            chkCounterWise.Checked = False
            Chktablegrpwise.Checked = False
        End If
    End Sub

    Private Sub ResizeToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResizeToolStripMenuItem1.Click
        If gridView_OWN.RowCount > 0 Then
            GridViewFormat()
            funcGridStyle()
            If ResizeToolStripMenuItem1.Checked Then
                gridView_OWN.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                gridView_OWN.Invalidate()
                For Each dgvCol As DataGridViewColumn In gridView_OWN.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridView_OWN.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            Else
                For Each dgvCol As DataGridViewColumn In gridView_OWN.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridView_OWN.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End If
            funcGridHeadWidth()
        End If
    End Sub
End Class

Public Class frmAgeWiseAnalysis_Properties
    Private cmbCategory As String = "ALL"
    Public Property p_cmbCategory() As String
        Get
            Return cmbCategory
        End Get
        Set(ByVal value As String)
            cmbCategory = value
        End Set
    End Property
    Private cmbmetal As String = "ALL"
    Public Property p_cmbmetal() As String
        Get
            Return cmbmetal
        End Get
        Set(ByVal value As String)
            cmbmetal = value
        End Set
    End Property
    Private cmbItem As String = "ALL"
    Public Property p_cmbItem() As String
        Get
            Return cmbItem
        End Get
        Set(ByVal value As String)
            cmbItem = value
        End Set
    End Property
    Private cmbSubItem As String = "ALL"
    Public Property p_cmbSubItem() As String
        Get
            Return cmbSubItem
        End Get
        Set(ByVal value As String)
            cmbSubItem = value
        End Set
    End Property
    Private cmbDesigner As String = "ALL"
    Public Property p_cmbDesigner() As String
        Get
            Return cmbDesigner
        End Get
        Set(ByVal value As String)
            cmbDesigner = value
        End Set
    End Property
    Private cmbCounter As String = "ALL"
    Public Property p_cmbCounter() As String
        Get
            Return cmbCounter
        End Get
        Set(ByVal value As String)
            cmbCounter = value
        End Set
    End Property
    Private cmbCostCentre As String = "ALL"
    Public Property p_cmbCostCentre() As String
        Get
            Return cmbCostCentre
        End Get
        Set(ByVal value As String)
            cmbCostCentre = value
        End Set
    End Property
    Private txtFromDay1_NUM As String = ""
    Public Property p_txtFromDay1_NUM() As String
        Get
            Return txtFromDay1_NUM
        End Get
        Set(ByVal value As String)
            txtFromDay1_NUM = value
        End Set
    End Property
    Private txtToDay1_NUM As String = ""
    Public Property p_txtToDay1_NUM() As String
        Get
            Return txtToDay1_NUM
        End Get
        Set(ByVal value As String)
            txtToDay1_NUM = value
        End Set
    End Property
    Private txtFromDay2_NUM As String = ""
    Public Property p_txtFromDay2_NUM() As String
        Get
            Return txtFromDay2_NUM
        End Get
        Set(ByVal value As String)
            txtFromDay2_NUM = value
        End Set
    End Property
    Private txtToDay2_NUM As String = ""
    Public Property p_txtToDay2_NUM() As String
        Get
            Return txtToDay2_NUM
        End Get
        Set(ByVal value As String)
            txtToDay2_NUM = value
        End Set
    End Property
    Private txtFromDay3_NUM As String = ""
    Public Property p_txtFromDay3_NUM() As String
        Get
            Return txtFromDay3_NUM
        End Get
        Set(ByVal value As String)
            txtFromDay3_NUM = value
        End Set
    End Property
    Private txtToDay3_NUM As String = ""
    Public Property p_txtToDay3_NUM() As String
        Get
            Return txtToDay3_NUM
        End Get
        Set(ByVal value As String)
            txtToDay3_NUM = value
        End Set
    End Property
    Private txtFromDay4_NUM As String = ""
    Public Property p_txtFromDay4_NUM() As String
        Get
            Return txtFromDay4_NUM
        End Get
        Set(ByVal value As String)
            txtFromDay4_NUM = value
        End Set
    End Property
    Private txtToDay4_NUM As String = ""
    Public Property p_txtToDay4_NUM() As String
        Get
            Return txtToDay4_NUM
        End Get
        Set(ByVal value As String)
            txtToDay4_NUM = value
        End Set
    End Property
    Private chkWithStone As Boolean = False
    Public Property p_chkWithStone() As Boolean
        Get
            Return chkWithStone
        End Get
        Set(ByVal value As Boolean)
            chkWithStone = value
        End Set
    End Property
    Private chkCounterWise As Boolean = False
    Public Property p_chkCounterWise() As Boolean
        Get
            Return chkCounterWise
        End Get
        Set(ByVal value As Boolean)
            chkCounterWise = value
        End Set
    End Property
    Private chkitemgrp As Boolean = False
    Public Property p_chkitemgrp() As Boolean
        Get
            Return chkitemgrp
        End Get
        Set(ByVal value As Boolean)
            chkitemgrp = value
        End Set
    End Property
    Private chktablegrp As Boolean = False
    Public Property p_chktablegrp() As Boolean
        Get
            Return chktablegrp
        End Get
        Set(ByVal value As Boolean)
            chktablegrp = value
        End Set
    End Property
    Private chkDesignergrp As Boolean = False
    Public Property p_chkDesignergrp() As Boolean
        Get
            Return chkDesignergrp
        End Get
        Set(ByVal value As Boolean)
            chkDesignergrp = value
        End Set
    End Property

    Private chkrangefrmmaster As Boolean = False
    Public Property p_chkrangefrmmaster() As Boolean
        Get
            Return chkrangefrmmaster
        End Get
        Set(ByVal value As Boolean)
            chkrangefrmmaster = value
        End Set
    End Property



    Private chkSubItem As Boolean = False
    Public Property p_chkSubItem() As Boolean
        Get
            Return chkSubItem
        End Get
        Set(ByVal value As Boolean)
            chkSubItem = value
        End Set
    End Property

    Private chkActualDate As Boolean = False
    Public Property p_chkActualDate() As Boolean
        Get
            Return chkActualDate
        End Get
        Set(ByVal value As Boolean)
            chkActualDate = value
        End Set
    End Property

    Private chkCmbtablecode As New List(Of String)
    Public Property p_chkCmbtablecode() As List(Of String)
        Get
            Return chkCmbtablecode
        End Get
        Set(ByVal value As List(Of String))
            chkCmbtablecode = value
        End Set
    End Property
End Class