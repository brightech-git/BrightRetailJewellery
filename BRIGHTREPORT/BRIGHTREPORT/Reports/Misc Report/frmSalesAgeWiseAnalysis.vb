Imports System.Data.OleDb

Public Class frmSalesAgeWiseAnalysis
    Dim strSql As String
    Dim dt As DataTable
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim dsGridView As New DataSet
    Dim max As Integer = 0
    Dim cnthead As Integer = 0
    Dim STRQRYSTRINGHEAD As String = Nothing
    Dim dtrange As New DataTable
    Dim dtTablecode, dtItem, dtSubitem, dtItemgroup As New DataTable

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
        strSql = " SELECT 'ALL' ITEMNAME,1 RESULT UNION ALL "
        strSql += " SELECT ITEMNAME,2 RESULT FROM " & cnAdminDb & "..ITEMMAST ORDER BY RESULT,ITEMNAME"
        dtItem = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtItem)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbItem, dtItem, "ITEMNAME", True, "ALL")

        'ITEMGROUP
        strSql = "SELECT 'ALL' GROUPNAME,1 RESULT UNION ALL "
        strSql += " SELECT GROUPNAME,2 RESULT FROM " & cnAdminDb & "..ITEMGROUPMAST ORDER BY RESULT,GROUPNAME"
        dtItem = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtItem)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbItemgroup, dtItem, "GROUPNAME", True, "ALL")

        'DESIGNER
        strSql = " SELECT 'ALL' DESIGNERNAME,1 RESULT UNION ALL "
        strSql += " SELECT DESIGNERNAME,2 RESULT FROM " & cnAdminDb & "..DESIGNER ORDER BY RESULT,DESIGNERNAME"
        dtItem = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtItem)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbDesigner, dtItem, "DESIGNERNAME", True, "ALL")

        'ITEM COUNTER
        cmbCounter.Items.Add("ALL")
        strSql = " SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER ORDER BY ITEMCTRNAME"
        objGPack.FillCombo(strSql, cmbCounter, False)
        cmbCounter.Text = "ALL"

        'COST CENTRE
        strSql = "SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COSTCENTRE'"
        If objGPack.GetSqlValue(strSql, , "N") = "Y" Then
            chkCmbCostcentre.Enabled = True
            strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
            strSql += " UNION ALL"
            strSql += " SELECT COSTNAME,CONVERT(VARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
            strSql += " ORDER BY RESULT,COSTNAME"
            dtItem = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtItem)
            BrighttechPack.GlobalMethods.FillCombo(chkCmbCostcentre, dtItem, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
            If strUserCentrailsed <> "Y" Then chkCmbCostcentre.Enabled = False
        Else
            chkCmbCostcentre.Enabled = False
        End If

        'CALID-714
        strSql = " SELECT 'ALL' TABLECODE,0 RESULT "
        strSql += " UNION ALL"
        strSql += " SELECT DISTINCT TABLECODE,1 RESULT FROM " & cnAdminDb & "..WMCTABLE WHERE ISNULL(TABLECODE,'')<>''  "
        strSql += " ORDER BY RESULT,TABLECODE"
        dtTablecode = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtTablecode)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbtablecode, dtTablecode, "TABLECODE", , "ALL")
    End Sub

    Private Sub frmAgeWiseAnalysis_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Me.WindowState = FormWindowState.Maximized
        Me.tabMain.ItemSize = New Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGen.Left, Me.tabGen.Top, Me.tabGen.Width, Me.tabGen.Height))
        Me.tabMain.SelectedTab = tabGen
        grpControls.Location = New Point((ScreenWid - grpControls.Width) / 2, ((ScreenHit - 128) - grpControls.Height) / 2)
        btnNew_Click(Me, e)
    End Sub

    Function funcGridStyle() As Integer
        With gridView
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
            With .Columns("subitemName")
                .Visible = False
            End With
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
        strSql = " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "AGESTONE')>0"
        strSql += " 	DROP TABLE TEMP" & systemId & "AGESTONE"
        strSql += " SELECT  '1'RESULT,TT.ITEMID,T.TAGNO,"
        strSql += " '[ ' +CAST(TT.ITEMID AS VARCHAR)+ ' ] '+ (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = TT.ITEMID)AS ITEMNAME, "
        strSql += " ISNULL((SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = TT.SUBITEMID),0)AS SUBITEMNAME, "
        If chkCounterWisegrp.Checked Then strSql += vbCrLf + " isnull((select ITEMCTRNAME from " & cnAdminDb & "..ITEMCOUNTER where ITEMCTRID = (select top 1 ITEMCTRID from " & cnAdminDb & "..ITEMTAG where TAGNO=t.TAGNO)),'')as ITEMCTRNAME, "
        If chkitemgrp.Checked Then strSql += vbCrLf + " isnull((select GROUPNAME from " & cnAdminDb & "..ITEMGROUPMAST where GROUPID = (select top 1 ITEMGROUP from " & cnAdminDb & "..itemmast where itemid=t.itemid)),'')as ITEMGROUPNAME, "
        If ChkDesignerGrp.Checked Then strSql += vbCrLf + " isnull((select DESIGNERNAME from " & cnAdminDb & "..DESIGNER where DESIGNERID = (select top 1 DESIGNERID from " & cnAdminDb & "..DESIGNER where DESIGNERID=TT.DESIGNERID)),'')as DESIGNER, "
        strSql += " datediff(d," & IIf(chkActualDate.Checked, "TT.ACTUALRECDATE", "TT.RecDate") & ",TT.ISSDATE)+1 as Days "
        strSql += " ,CASE WHEN IM.DIASTONE ='D' THEN T.STNPCS END AS  DIAPCS"
        strSql += " ,CASE WHEN IM.DIASTONE ='D' THEN T.STNWT END  AS  DIAWT"
        strSql += " ,CASE WHEN IM.DIASTONE ='P' THEN T.STNPCS END AS  PREPCS"
        strSql += " ,CASE WHEN IM.DIASTONE ='P' THEN T.STNWT END  AS  PREWT"
        strSql += " ,CASE WHEN IM.DIASTONE ='S' THEN T.STNPCS END AS  STNPCS"
        strSql += " ,CASE WHEN IM.DIASTONE ='S' THEN T.STNWT END  AS  STNWT"
        strSql += " ,T.STONEUNIT,'1'STONE"
        strSql += " INTO TEMP" & systemId & "AGESTONE "
        strSql += " from " & cnAdminDb & "..ITEMTAGSTONE AS T "
        strSql += " INNER JOIN " & cnAdminDb & "..ITEMTAG TT ON T.TAGSNO=TT.SNO"
        strSql += " INNER JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID=T.STNITEMID"
        strSql += " WHERE T.TAGNO IN (SELECT TAGNO FROM TEMP" & systemId & "AGE)"
        strSql += " AND T.ITEMID IN(SELECT ITEMID FROM TEMP" & systemId & "AGE)"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Function

    Function funcFiltration(ByVal Tag As Boolean) As String
        strSql = ""
        If Tag Then
            If chkason.Checked = True Then
                chkason.Text = "&As OnDate"
            Else
                chkason.Text = "&Date From"
            End If
        End If
        If Not cnCentStock Then strSql += " AND COMPANYID = '" & GetStockCompId() & "'"
        If cmbCategory.Text <> "ALL" And cmbCategory.Text <> "" Then
            strSql += " AND T.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE CATCODE = (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & cmbCategory.Text & "'))"
        End If
        If cmbmetal.Text <> "ALL" And cmbmetal.Text <> "" Then
            strSql += " AND T.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME='" & cmbmetal.Text & "'))"
        End If
        If chkCmbItem.Text <> "ALL" And chkCmbItem.Text <> "" Then
            strSql += " AND T.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN('" & chkCmbItem.Text.Replace(",", "','") & "'))"
        End If

        If chkCmbItemgroup.Text <> "ALL" And chkCmbItemgroup.Text <> "" Then
            strSql += " AND T.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMGROUP IN(SELECT GROUPID FROM " & cnAdminDb & "..ITEMGROUPMAST WHERE GROUPNAME IN ('" & chkCmbItemgroup.Text.Replace(",", "','") & "')))"
        End If

        If chkCmbSubitem.Text <> "ALL" And chkCmbSubitem.Text <> "" Then
            strSql += " AND T.SUBITEMID IN (SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID IN(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN('" & chkCmbItem.Text.Replace(",", "','") & "'))"
            strSql += "  AND SUBITEMNAME IN('" & chkCmbSubitem.Text.Replace(",", "','") & "'))"
        End If

        If chkCmbDesigner.Text <> "ALL" And chkCmbDesigner.Text <> "" Then
            strSql += " AND T.DESIGNERID IN (SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME IN( " & GetQryString(chkCmbDesigner.Text) & "))"
        End If
        If cmbCounter.Text <> "ALL" And cmbCounter.Text <> "" Then
            strSql += " AND T.ITEMCTRID IN (SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & cmbCounter.Text & "')"
        End If
        If chkCmbCostcentre.Enabled = True Then
            If chkCmbCostcentre.Text <> "ALL" And chkCmbCostcentre.Text <> "" Then
                strSql += " AND T.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN( " & GetQryString(chkCmbCostcentre.Text) & "))"
            End If
        End If
        'CALID-714
        If chkCmbtablecode.Text <> "ALL" And chkCmbtablecode.Text <> "" Then
            strSql += " AND T.TABLECODE IN ('" & chkCmbtablecode.Text.ToString.Replace(",", "','") & "')"
        End If
        Return strSql
    End Function

    Function funcSearch() As Integer
        Dim STRQRYSTRING As String = Nothing
        Dim PCSQRY As String = Nothing
        Dim WTQRY As String = Nothing
        Dim DIAPCSQRY As String = Nothing
        Dim STNPCSQRY As String = Nothing
        Dim PREPCSQRY As String = Nothing
        Dim DIAWTQRY As String = Nothing
        Dim STNWTQRY As String = Nothing
        Dim PREWTQRY As String = Nothing
        Dim ftrStr As String = funcFiltration(True)
        Dim ftrStrN As String = funcFiltration(False)

        strSql = " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "AGEWISE')>0"
        strSql += vbCrLf + " 	DROP TABLE TEMP" & systemId & "AGEWISE"
        strSql += vbCrLf + " CREATE TABLE TEMP" & systemId & "AGEWISE("
        strSql += vbCrLf + " PARTICULAR VARCHAR(50)"
        strSql += vbCrLf + " ,ITEMNAME VARCHAR(50)"
        If chkSubItem.Checked Then strSql += vbCrLf + " ,SUBITEMNAME VARCHAR(50)NULL"
        If chkCounterWisegrp.Checked Then strSql += vbCrLf + " ,ITEMCTRNAME VARCHAR(50)NULL"
        If chkitemgrp.Checked Then strSql += vbCrLf + " ,ITEMGROUPNAME VARCHAR(50)NULL"
        If ChkDesignerGrp.Checked Then strSql += vbCrLf + " ,DESIGNER VARCHAR(50)NULL"
        strSql += vbCrLf + " ,PCS1 INT"
        strSql += vbCrLf + " ,GRSWT1 NUMERIC(20,3)"
        strSql += vbCrLf + " ,PCS2 INT"
        strSql += vbCrLf + " ,GRSWT2 NUMERIC(20,3)"
        strSql += vbCrLf + " ,PCS3 INT"
        strSql += vbCrLf + " ,GRSWT3 NUMERIC(20,3)"
        strSql += vbCrLf + " ,PCS4 INT"
        strSql += vbCrLf + " ,GRSWT4 NUMERIC(20,3)"

        strSql += vbCrLf + " ,TOTPCS INT ,TOTGRSWT NUMERIC(15,3) ,MAXPCS INT"
        strSql += vbCrLf + " ,MAXGRSWT NUMERIC(15,3)"
        strSql += vbCrLf + " ,DIAPCS1 INT"
        strSql += vbCrLf + " ,DIAWT1 NUMERIC(20,3)"
        strSql += vbCrLf + " ,PREPCS1 INT"
        strSql += vbCrLf + " ,PREWT1 NUMERIC(20,3)"
        strSql += vbCrLf + " ,STNPCS1 INT"
        strSql += vbCrLf + " ,STNWT1 NUMERIC(20,3)"
        strSql += vbCrLf + " ,DIAPCS2 INT"
        strSql += vbCrLf + " ,DIAWT2 NUMERIC(20,3)"
        strSql += vbCrLf + " ,PREPCS2 INT"
        strSql += vbCrLf + " ,PREWT2 NUMERIC(20,3)"
        strSql += vbCrLf + " ,STNPCS2 INT"
        strSql += vbCrLf + " ,STNWT2 NUMERIC(20,3)"
        strSql += vbCrLf + " ,DIAPCS3 INT"
        strSql += vbCrLf + " ,DIAWT3 NUMERIC(20,3)"
        strSql += vbCrLf + " ,PREPCS3 INT"
        strSql += vbCrLf + " ,PREWT3 NUMERIC(20,3)"
        strSql += vbCrLf + " ,STNPCS3 INT"
        strSql += vbCrLf + " ,STNWT3 NUMERIC(20,3)"
        strSql += vbCrLf + " ,DIAPCS4 INT"
        strSql += vbCrLf + " ,DIAWT4 NUMERIC(20,3)"
        strSql += vbCrLf + " ,PREPCS4 INT"
        strSql += vbCrLf + " ,PREWT4 NUMERIC(20,3)"
        strSql += vbCrLf + " ,STNPCS4 INT"
        strSql += vbCrLf + " ,STNWT4 NUMERIC(20,3)"

        strSql += vbCrLf + " ,TOTDIAPCS INT"
        strSql += vbCrLf + " ,TOTDIAWT NUMERIC(20,3)"
        strSql += vbCrLf + " ,TOTPREPCS INT"
        strSql += vbCrLf + " ,TOTPREWT NUMERIC(20,3)"
        strSql += vbCrLf + " ,TOTSTNPCS INT"
        strSql += vbCrLf + " ,TOTSTNWT NUMERIC(20,3)"

        strSql += vbCrLf + " ,MAXDIAPCS INT"
        strSql += vbCrLf + " ,MAXDIAWT NUMERIC(20,3)"
        strSql += vbCrLf + " ,MAXPREPCS INT"
        strSql += vbCrLf + " ,MAXPREWT NUMERIC(20,3)"
        strSql += vbCrLf + " ,MAXSTNPCS INT"
        strSql += vbCrLf + " ,MAXSTNWT NUMERIC(20,3)"

        strSql += vbCrLf + " ,ITEMID INT"
        strSql += vbCrLf + " ,RESULT INT"
        strSql += vbCrLf + " ,STONE VARCHAR(1)"
        strSql += vbCrLf + " ,COLHEAD VARCHAR(1)"
        strSql += vbCrLf + " ,SNO INT IDENTITY)"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        max = 0

        strSql = " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "AGE')>0"
        strSql += vbCrLf + " 	DROP TABLE TEMP" & systemId & "AGE"
        strSql += vbCrLf + " SELECT RESULT,ITEMID,TAGNO,'[ ' +CAST(ITEMID AS VARCHAR)+ ' ] ' + ITEMNAME AS ITEMNAME"
        If chkSubItem.Checked Then strSql += vbCrLf + " ,SUBITEMNAME"
        If chkCounterWisegrp.Checked Then strSql += vbCrLf + " ,ITEMCTRNAME"
        If chkitemgrp.Checked Then strSql += vbCrLf + " ,ITEMGROUPNAME"
        If ChkDesignerGrp.Checked Then strSql += vbCrLf + " ,DESIGNER"
        strSql += vbCrLf + " ,DAYS,SUM(PCS)PCS,SUM(GRSWT)GRSWT"
        strSql += vbCrLf + " ,'1'STONE,CONVERT(VARCHAR(1),'')COLHEAD "
        strSql += vbCrLf + " INTO TEMP" & systemId & "AGE"
        strSql += vbCrLf + " FROM ("
        strSql += vbCrLf + " SELECT  '1'RESULT,T.ITEMID,T.TAGNO,"
        strSql += vbCrLf + " CONVERT(NVARCHAR(50),(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID))AS ITEMNAME, "
        If chkSubItem.Checked Then strSql += vbCrLf + " ISNULL((SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = T.SUBITEMID),'')AS SUBITEMNAME, "
        If chkCounterWisegrp.Checked Then strSql += vbCrLf + " ISNULL((SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = T.ITEMCTRID),'')AS ITEMCTRNAME, "
        If chkitemgrp.Checked Then strSql += vbCrLf + " ISNULL((SELECT GROUPNAME FROM " & cnAdminDb & "..ITEMGROUPMAST WHERE GROUPID =(SELECT ITEMGROUP FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=T.ITEMID)),'')AS ITEMGROUPNAME, "
        If ChkDesignerGrp.Checked Then strSql += vbCrLf + " ISNULL((SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID =T.DESIGNERID),'')AS DESIGNER, "
        strSql += vbCrLf + " DATEDIFF(D," & IIf(chkActualDate.Checked, "ACTUALRECDATE", "RECDATE") & ",ISSDATE)+1 AS DAYS, "
        strSql += vbCrLf + " PCS,GRSWT,'1'STONE,CONVERT(VARCHAR(1),'')COLHEAD "
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T "
        If chkason.Checked Then
            strSql += vbCrLf + " WHERE ISSDATE <= '" & dtpAsOnDate.Value.Date.ToString("yyyy-MM-dd") & "'"
        Else
            strSql += vbCrLf + " WHERE ISSDATE BETWEEN '" & dtpAsOnDate.Value.Date.ToString("yyyy-MM-dd") & "'AND'" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
        End If
        strSql += ftrStr
        strSql += vbCrLf + " AND TOFLAG='SA'"
        strSql += vbCrLf + " )X GROUP BY ITEMID,RESULT,TAGNO,ITEMNAME,DAYS"
        If chkSubItem.Checked Then strSql += vbCrLf + " ,SUBITEMNAME"
        If chkCounterWisegrp.Checked Then strSql += vbCrLf + " ,ITEMCTRNAME"
        If chkitemgrp.Checked Then strSql += vbCrLf + " ,ITEMGROUPNAME"
        If ChkDesignerGrp.Checked Then strSql += vbCrLf + " ,DESIGNER"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        If chkWithStone.Checked Or chkWithDia.Checked Or chkWithPre.Checked Then
            funcWithStone()
        End If
        STRQRYSTRING = "ITEMNAME"
        If chkSubItem.Checked Then STRQRYSTRING += ", SUBITEMNAME"
        If chkCounterWisegrp.Checked Then STRQRYSTRING += " ,ITEMCTRNAME"
        If chkitemgrp.Checked Then STRQRYSTRING += " , ITEMGROUPNAME"
        If ChkDesignerGrp.Checked Then STRQRYSTRING += " , DESIGNER"
        STRQRYSTRINGHEAD = "' ' PARTICULAR"
        cnthead = 1
        PCSQRY = "(" : WTQRY = "("
        If chkWithStone.Checked Or chkWithDia.Checked Or chkWithPre.Checked Then
            DIAPCSQRY = "("
            DIAWTQRY = "("
            PREPCSQRY = "("
            PREWTQRY = "("
            STNPCSQRY = "("
            STNWTQRY = "("
        End If
        If txtFromDay1_NUM.Text <> "" And txtToDay1_NUM.Text <> "" Then
            STRQRYSTRING += " , PCS1, GRSWT1 "
            PCSQRY += "CONVERT(INT,ISNULL(PCS1,0))"
            WTQRY += "CONVERT(DECIMAL(12,3),ISNULL(GRSWT1,0))"
            max = Val(txtToDay1_NUM.Text)
            cnthead += 1
            If chkWithStone.Checked Or chkWithDia.Checked Or chkWithPre.Checked Then
                STRQRYSTRING += " , DIAPCS1, DIAWT1, PREPCS1, PREWT1, STNPCS1, STNWT1 "
                DIAPCSQRY += "  CONVERT(INT,ISNULL(DIAPCS1,0))"
                DIAWTQRY += "  CONVERT(NUMERIC(15,3),ISNULL(DIAWT1,0))"
                PREPCSQRY += "  CONVERT(INT,ISNULL(PREPCS1,0))"
                PREWTQRY += "  CONVERT(NUMERIC(15,3),ISNULL(PREWT1,0))"
                STNPCSQRY += "  CONVERT(INT,ISNULL(STNPCS1,0))"
                STNWTQRY += "  CONVERT(NUMERIC(15,3),ISNULL(STNWT1,0))"
                STRQRYSTRINGHEAD += " , '' 'PCS1~GRSWT1~DIAPCS1~DIAWT1~PREPCS1~PREWT1~STNPCS1~STNWT1'"
            Else
                STRQRYSTRINGHEAD += " , '' 'PCS1~GRSWT1'"
            End If

        End If
        If txtFromDay2_NUM.Text <> "" And txtToDay2_NUM.Text <> "" Then
            STRQRYSTRING += " , PCS2, GRSWT2 "
            PCSQRY += "+CONVERT(INT,ISNULL(PCS2,0))"
            WTQRY += "+CONVERT(DECIMAL(12,3),ISNULL(GRSWT2,0))"
            max = Val(txtToDay2_NUM.Text)
            cnthead += 1
            If chkWithStone.Checked Or chkWithDia.Checked Or chkWithPre.Checked Then
                STRQRYSTRING += " , DIAPCS2, DIAWT2, PREPCS2, PREWT2, STNPCS2, STNWT2 "
                DIAPCSQRY += "  +CONVERT(INT,ISNULL(DIAPCS2,0))"
                DIAWTQRY += "  +CONVERT(NUMERIC(15,3),ISNULL(DIAWT2,0))"
                PREPCSQRY += "  +CONVERT(INT,ISNULL(PREPCS2,0))"
                PREWTQRY += "  +CONVERT(NUMERIC(15,3),ISNULL(PREWT2,0))"
                STNPCSQRY += "  +CONVERT(INT,ISNULL(STNPCS2,0))"
                STNWTQRY += "  +CONVERT(NUMERIC(15,3),ISNULL(STNWT2,0))"
                STRQRYSTRINGHEAD += " , '' 'PCS2~GRSWT2~DIAPCS2~DIAWT2~PREPCS2~PREWT2~STNPCS2~STNWT2'"
            Else
                STRQRYSTRINGHEAD += " , '' 'PCS2~GRSWT2'"
            End If

        End If
        If txtFromDay3_NUM.Text <> "" And txtToDay3_NUM.Text <> "" Then
            STRQRYSTRING += " , PCS3, GRSWT3 "
            PCSQRY += "+CONVERT(INT,ISNULL(PCS3,0))"
            WTQRY += "+CONVERT(DECIMAL(12,3),ISNULL(GRSWT3,0))"
            max = Val(txtToDay3_NUM.Text)
            cnthead += 1
            If chkWithStone.Checked Or chkWithDia.Checked Or chkWithPre.Checked Then
                STRQRYSTRING += " , DIAPCS3, DIAWT3, PREPCS3, PREWT3, STNPCS3, STNWT3 "
                DIAPCSQRY += "  +CONVERT(INT,ISNULL(DIAPCS3,0))"
                DIAWTQRY += "  +CONVERT(NUMERIC(15,3),ISNULL(DIAWT3,0))"
                PREPCSQRY += "  +CONVERT(INT,ISNULL(PREPCS3,0))"
                PREWTQRY += "  +CONVERT(NUMERIC(15,3),ISNULL(PREWT3,0))"
                STNPCSQRY += "  +CONVERT(INT,ISNULL(STNPCS3,0))"
                STNWTQRY += "  +CONVERT(NUMERIC(15,3),ISNULL(STNWT3,0))"
                STRQRYSTRINGHEAD += " , '' 'PCS3~GRSWT3~DIAPCS3~DIAWT3~PREPCS3~PREWT3~STNPCS3~STNWT3'"
            Else
                STRQRYSTRINGHEAD += " , '' 'PCS3~GRSWT3'"
            End If

        End If
        If txtFromDay4_NUM.Text <> "" And txtToDay4_NUM.Text <> "" Then
            STRQRYSTRING += " , PCS4, GRSWT4 "
            PCSQRY += "+CONVERT(INT,ISNULL(PCS4,0))"
            WTQRY += "+CONVERT(DECIMAL(12,3),ISNULL(GRSWT4,0))"
            max = Val(txtToDay4_NUM.Text)
            cnthead += 1
            If chkWithStone.Checked Or chkWithDia.Checked Or chkWithPre.Checked Then
                STRQRYSTRING += " , DIAPCS4, DIAWT4, PREPCS4, PREWT4, STNPCS4, STNWT4 "
                DIAPCSQRY += "  +CONVERT(INT,ISNULL(DIAPCS4,0))"
                DIAWTQRY += "  +CONVERT(NUMERIC(15,3),ISNULL(DIAWT4,0))"
                PREPCSQRY += "  +CONVERT(INT,ISNULL(PREPCS4,0))"
                PREWTQRY += "  +CONVERT(NUMERIC(15,3),ISNULL(PREWT4,0))"
                STNPCSQRY += "  +CONVERT(INT,ISNULL(STNPCS4,0))"
                STNWTQRY += "  +CONVERT(NUMERIC(15,3),ISNULL(STNWT4,0))"
                STRQRYSTRINGHEAD += " , '' 'PCS4~GRSWT4~DIAPCS4~DIAWT4~PREPCS4~PREWT4~STNPCS4~STNWT4'"
            Else
                STRQRYSTRINGHEAD += " , '' 'PCS4~GRSWT4'"
            End If
        End If
        PCSQRY += "+CONVERT(INT,ISNULL(MAXPCS,0))) TOTPCS"
        WTQRY += "+CONVERT(DECIMAL(12,3),ISNULL(MAXGRSWT,0))) TOTGRSWT"
        If chkWithStone.Checked Or chkWithDia.Checked Or chkWithPre.Checked Then
            DIAPCSQRY += "+CONVERT(INT,ISNULL(MAXDIAPCS,0)))TOTDIAPCS"
            DIAWTQRY += "+CONVERT(DECIMAL(12,3),ISNULL(MAXDIAWT,0)))TOTDIAWT"
            PREPCSQRY += "+CONVERT(INT,ISNULL(MAXPREPCS,0)))TOTPREPCS"
            PREWTQRY += "+CONVERT(DECIMAL(12,3),ISNULL(MAXPREWT,0)))TOTPREWT"
            STNPCSQRY += "+CONVERT(INT,ISNULL(MAXSTNPCS,0)))TOTSTNPCS"
            STNWTQRY += "+CONVERT(DECIMAL(12,3),ISNULL(MAXSTNWT,0)))TOTSTNWT"
        End If
        STRQRYSTRING += " , MAXPCS, MAXGRSWT"
        cnthead += 1
        If chkWithStone.Checked Or chkWithDia.Checked Or chkWithPre.Checked Then
            STRQRYSTRING += " , MAXDIAPCS, MAXDIAWT, MAXPREPCS, MAXPREWT, MAXSTNPCS, MAXSTNWT"
            STRQRYSTRINGHEAD += " , '' 'MAXPCS~MAXGRSWT~MAXDIAPCS~MAXDIAWT~MAXPREPCS~MAXPREWT~MAXSTNPCS~MAXSTNWT'"
            STRQRYSTRINGHEAD += " , '' 'TOTPCS~TOTGRSWT~TOTDIAPCS~TOTDIAWT~TOTPREPCS~TOTPREWT~TOTSTNPCS~TOTSTNWT'"
        Else
            STRQRYSTRINGHEAD += " , '' 'MAXPCS~MAXGRSWT'"
            STRQRYSTRINGHEAD += " , '' 'TOTPCS~TOTGRSWT'"
        End If

        STRQRYSTRING += " , ITEMID, RESULT, STONE, COLHEAD"

        strSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "AGE)>0"
        strSql += vbCrLf + " BEGIN "
        strSql += vbCrLf + " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "AGE1')>0"
        strSql += vbCrLf + " 	DROP TABLE TEMP" & systemId & "AGE1"
        strSql += vbCrLf + "  SELECT " & STRQRYSTRING & " INTO TEMP" & systemId & "AGE1 from ("
        strSql += vbCrLf + " Select itemName," + IIf(chkSubItem.Checked = True, "subItemName,", "")
        If chkCounterWisegrp.Checked Then strSql += vbCrLf + " ITEMCTRNAME,"
        If chkitemgrp.Checked Then strSql += vbCrLf + " ITEMGROUPNAME,"
        If ChkDesignerGrp.Checked Then strSql += vbCrLf + " DESIGNER,"
        If txtFromDay1_NUM.Text <> "" And txtToDay1_NUM.Text <> "" Then
            strSql += vbCrLf + " convert(INT,sum(case when Days between " & Val(txtFromDay1_NUM.Text) & " and " & Val(txtToDay1_NUM.Text) & " then pcs else 0 end ))Pcs1 ,"
            strSql += vbCrLf + " convert(numeric(20,3),sum(case when Days between " & Val(txtFromDay1_NUM.Text) & " and " & Val(txtToDay1_NUM.Text) & " then GrsWt else 0 end ))GrsWt1 ,"
            If chkWithStone.Checked Or chkWithDia.Checked Or chkWithPre.Checked Then
                strSql += vbCrLf + " convert(INT,sum(case when Days between " & Val(txtFromDay1_NUM.Text) & " and " & Val(txtToDay1_NUM.Text) & " then DIAPCS else 0 end ))DIAPCS1,"
                strSql += vbCrLf + " convert(numeric(20,3),sum(case when Days between " & Val(txtFromDay1_NUM.Text) & " and " & Val(txtToDay1_NUM.Text) & " then DIAWT else 0 end ))DIAWT1 ,"
                strSql += vbCrLf + " convert(INT,sum(case when Days between " & Val(txtFromDay1_NUM.Text) & " and " & Val(txtToDay1_NUM.Text) & " then PREPCS else 0 end ))PREPCS1 ,"
                strSql += vbCrLf + " convert(numeric(20,3),sum(case when Days between " & Val(txtFromDay1_NUM.Text) & " and " & Val(txtToDay1_NUM.Text) & " then PREWT else 0 end ))PREWT1 ,"
                strSql += vbCrLf + " convert(INT,sum(case when Days between " & Val(txtFromDay1_NUM.Text) & " and " & Val(txtToDay1_NUM.Text) & " then STNPCS else 0 end ))STNPCS1 ,"
                strSql += vbCrLf + " convert(numeric(20,3),sum(case when Days between " & Val(txtFromDay1_NUM.Text) & " and " & Val(txtToDay1_NUM.Text) & " then STNWT else 0 end ))STNWT1 ,"
            End If
            max = Val(txtToDay1_NUM.Text)
        End If
        If txtFromDay2_NUM.Text <> "" And txtToDay2_NUM.Text <> "" Then
            strSql += vbCrLf + " convert(INT,sum(case when Days between " & Val(txtFromDay2_NUM.Text) & " and " & Val(txtToDay2_NUM.Text) & " then pcs else 0 end ))Pcs2 ,"
            strSql += vbCrLf + " convert(numeric(20,3),sum(case when Days between " & Val(txtFromDay2_NUM.Text) & " and " & Val(txtToDay2_NUM.Text) & " then GrsWt else 0 end ))GrsWt2 ,"
            If chkWithStone.Checked Or chkWithDia.Checked Or chkWithPre.Checked Then
                strSql += vbCrLf + " convert(INT,sum(case when Days between " & Val(txtFromDay2_NUM.Text) & " and " & Val(txtToDay2_NUM.Text) & " then DIAPCS else 0 end ))DIAPCS2,"
                strSql += vbCrLf + " convert(numeric(20,3),sum(case when Days between " & Val(txtFromDay2_NUM.Text) & " and " & Val(txtToDay2_NUM.Text) & " then DIAWT else 0 end ))DIAWT2 ,"
                strSql += vbCrLf + " convert(INT,sum(case when Days between " & Val(txtFromDay2_NUM.Text) & " and " & Val(txtToDay2_NUM.Text) & " then PREPCS else 0 end ))PREPCS2 ,"
                strSql += vbCrLf + " convert(numeric(20,3),sum(case when Days between " & Val(txtFromDay2_NUM.Text) & " and " & Val(txtToDay2_NUM.Text) & " then PREWT else 0 end ))PREWT2 ,"
                strSql += vbCrLf + " convert(INT,sum(case when Days between " & Val(txtFromDay2_NUM.Text) & " and " & Val(txtToDay2_NUM.Text) & " then STNPCS else 0 end ))STNPCS2 ,"
                strSql += vbCrLf + " convert(numeric(20,3),sum(case when Days between " & Val(txtFromDay2_NUM.Text) & " and " & Val(txtToDay2_NUM.Text) & " then STNWT else 0 end ))STNWT2 ,"
            End If
            max = Val(txtToDay2_NUM.Text)
        End If
        If txtFromDay3_NUM.Text <> "" And txtToDay3_NUM.Text <> "" Then
            strSql += vbCrLf + " convert(INT,sum(case when Days between " & Val(txtFromDay3_NUM.Text) & " and " & Val(txtToDay3_NUM.Text) & " then pcs else 0 end ))Pcs3 ,"
            strSql += vbCrLf + " convert(numeric(20,3),sum(case when Days between " & Val(txtFromDay3_NUM.Text) & " and " & Val(txtToDay3_NUM.Text) & " then GrsWt else 0 end ))GrsWt3 ,"
            If chkWithStone.Checked Or chkWithDia.Checked Or chkWithPre.Checked Then
                strSql += vbCrLf + " convert(INT,sum(case when Days between " & Val(txtFromDay3_NUM.Text) & " and " & Val(txtToDay3_NUM.Text) & " then DIAPCS else 0 end ))DIAPCS3,"
                strSql += vbCrLf + " convert(numeric(20,3),sum(case when Days between " & Val(txtFromDay3_NUM.Text) & " and " & Val(txtToDay3_NUM.Text) & " then DIAWT else 0 end ))DIAWT3 ,"
                strSql += vbCrLf + " convert(INT,sum(case when Days between " & Val(txtFromDay3_NUM.Text) & " and " & Val(txtToDay3_NUM.Text) & " then PREPCS else 0 end ))PREPCS3 ,"
                strSql += vbCrLf + " convert(numeric(20,3),sum(case when Days between " & Val(txtFromDay3_NUM.Text) & " and " & Val(txtToDay3_NUM.Text) & " then PREWT else 0 end ))PREWT3 ,"
                strSql += vbCrLf + " convert(INT,sum(case when Days between " & Val(txtFromDay3_NUM.Text) & " and " & Val(txtToDay3_NUM.Text) & " then STNPCS else 0 end ))STNPCS3 ,"
                strSql += vbCrLf + " convert(numeric(20,3),sum(case when Days between " & Val(txtFromDay3_NUM.Text) & " and " & Val(txtToDay3_NUM.Text) & " then STNWT else 0 end ))STNWT3 ,"
            End If
            max = Val(txtToDay3_NUM.Text)
        End If
        If txtFromDay4_NUM.Text <> "" And txtToDay4_NUM.Text <> "" Then
            strSql += vbCrLf + " convert(INT,sum(case when Days between " & Val(txtFromDay4_NUM.Text) & " and " & Val(txtToDay4_NUM.Text) & " then pcs else 0 end ))Pcs4 ,"
            strSql += vbCrLf + " convert(numeric(20,3),sum(case when Days between " & Val(txtFromDay4_NUM.Text) & " and " & Val(txtToDay4_NUM.Text) & " then GrsWt else 0 end ))GrsWt4 ,"
            If chkWithStone.Checked Or chkWithDia.Checked Or chkWithPre.Checked Then
                strSql += vbCrLf + " convert(INT,sum(case when Days between " & Val(txtFromDay4_NUM.Text) & " and " & Val(txtToDay4_NUM.Text) & " then DIAPCS else 0 end ))DIAPCS4,"
                strSql += vbCrLf + " convert(numeric(20,3),sum(case when Days between " & Val(txtFromDay4_NUM.Text) & " and " & Val(txtToDay4_NUM.Text) & " then DIAWT else 0 end ))DIAWT4 ,"
                strSql += vbCrLf + " convert(INT,sum(case when Days between " & Val(txtFromDay4_NUM.Text) & " and " & Val(txtToDay4_NUM.Text) & " then PREPCS else 0 end ))PREPCS4 ,"
                strSql += vbCrLf + " convert(numeric(20,3),sum(case when Days between " & Val(txtFromDay4_NUM.Text) & " and " & Val(txtToDay4_NUM.Text) & " then PREWT else 0 end ))PREWT4 ,"
                strSql += vbCrLf + " convert(INT,sum(case when Days between " & Val(txtFromDay4_NUM.Text) & " and " & Val(txtToDay4_NUM.Text) & " then STNPCS else 0 end ))STNPCS4 ,"
                strSql += vbCrLf + " convert(numeric(20,3),sum(case when Days between " & Val(txtFromDay4_NUM.Text) & " and " & Val(txtToDay4_NUM.Text) & " then STNWT else 0 end ))STNWT4 ,"
            End If
            max = Val(txtToDay4_NUM.Text)
        End If

        strSql += vbCrLf + " convert(INT,sum(case when Days > " & max & " then pcs else 0 end ))MaxPcs ,"
        strSql += vbCrLf + " convert(numeric(20,3),sum(case when Days > " & max & " then GrsWt else 0 end ))MaxGrsWt,"
        If chkWithStone.Checked Or chkWithDia.Checked Or chkWithPre.Checked Then
            strSql += vbCrLf + " convert(INT,sum(case when Days > " & max & " then DIAPCS else 0 end ))MAXDIAPCS ,"
            strSql += vbCrLf + " convert(numeric(20,3),sum(case when Days > " & max & " then DIAWT else 0 end ))MAXDIAWT,"
            strSql += vbCrLf + " convert(INT,sum(case when Days > " & max & " then PREPCS else 0 end ))MAXPREPCS ,"
            strSql += vbCrLf + " convert(numeric(20,3),sum(case when Days > " & max & " then PREWT else 0 end ))MAXPREWT,"
            strSql += vbCrLf + " convert(INT,sum(case when Days > " & max & " then STNPCS else 0 end ))MAXSTNPCS ,"
            strSql += vbCrLf + " convert(numeric(20,3),sum(case when Days > " & max & " then STNWT else 0 end ))MAXSTNWT,"
        End If
        strSql += vbCrLf + " itemid,RESULT,Stone,COLHEAD"
        strSql += vbCrLf + " from"
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " select "
        strSql += vbCrLf + " itemName," + IIf(chkSubItem.Checked = True, "subItemName,", "")
        If chkCounterWisegrp.Checked Then strSql += vbCrLf + " ITEMCTRNAME,"
        If chkitemgrp.Checked Then strSql += vbCrLf + " ITEMGROUPNAME,"
        If ChkDesignerGrp.Checked Then strSql += vbCrLf + " DESIGNER,"
        strSql += vbCrLf + " sum(convert(float,Pcs))as Pcs,"
        strSql += vbCrLf + " sum(convert(float,GrsWt))as GrsWt"
        strSql += vbCrLf + " ,'0' DIAPCS,'0' DIAWT"
        strSql += vbCrLf + " ,'0' PREPCS,'0' PREWT"
        strSql += vbCrLf + " ,'0' STNPCS,'0' STNWT"
        strSql += vbCrLf + " ,result,itemId,days,Stone,COLHEAD"
        strSql += vbCrLf + " from temp" & systemId & "Age AS TEM "
        If chkCounterWisegrp.Checked Or chkitemgrp.Checked Or ChkDesignerGrp.Checked Then strSql += vbCrLf + " where colhead<>'G'"
        strSql += vbCrLf + " group by itemid,result,itemName," + IIf(chkSubItem.Checked = True, "subItemName,", "") + "days,Stone,COLHEAD"
        If chkCounterWisegrp.Checked Then strSql += vbCrLf + ",ITEMCTRNAME"
        If chkitemgrp.Checked Then strSql += vbCrLf + ",ITEMGROUPNAME"
        If ChkDesignerGrp.Checked Then strSql += vbCrLf + ",DESIGNER"
        If chkWithStone.Checked Or chkWithDia.Checked Or chkWithPre.Checked Then
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT"
            strSql += vbCrLf + " ITEMNAME AS ITEMNAME" + IIf(chkSubItem.Checked = True, ",CASE WHEN SUBITEMNAME = '' THEN ''ELSE SUBITEMNAME END AS SUBITEMNAME", "") + IIf(chkCounterWisegrp.Checked, ",ITEMCTRNAME", "") + IIf(chkitemgrp.Checked, ",ITEMGROUPNAME", "") + IIf(ChkDesignerGrp.Checked, ",DESIGNER", "") + ""
            strSql += vbCrLf + " ,'0' PCS,'0' GRSWT"
            strSql += vbCrLf + " ,SUM(DIAPCS) AS DIAPCS"
            strSql += vbCrLf + " ,SUM(DIAWT) AS DIAWT"
            strSql += vbCrLf + " ,SUM(STNPCS) AS STNPCS"
            strSql += vbCrLf + " ,SUM(STNWT) AS STNWT"
            strSql += vbCrLf + " ,SUM(PREPCS) AS PREPCS"
            strSql += vbCrLf + " ,SUM(PREWT) AS PREWT"
            strSql += vbCrLf + " ,RESULT,ITEMID,DAYS,STONE,''COLHEAD"
            strSql += vbCrLf + " FROM TEMP" & systemId & "AGESTONE AS TEM"
            strSql += vbCrLf + " GROUP BY ITEMID,RESULT,ITEMNAME," + IIf(chkSubItem.Checked = True, "SUBITEMNAME,", "") + "DAYS,STONE"
            If chkCounterWisegrp.Checked Then strSql += vbCrLf + ",ITEMCTRNAME"
            If chkitemgrp.Checked Then strSql += vbCrLf + ",ITEMGROUPNAME"
            If ChkDesignerGrp.Checked Then strSql += vbCrLf + ",DESIGNER"
        End If
        strSql += vbCrLf + " )AGE"
        strSql += vbCrLf + " GROUP BY ITEMID,RESULT,ITEMNAME," + IIf(chkSubItem.Checked = True, "SUBITEMNAME,", "") + "STONE,COLHEAD"
        If chkCounterWisegrp.Checked Then strSql += vbCrLf + ",ITEMCTRNAME"
        If chkitemgrp.Checked Then strSql += vbCrLf + ",ITEMGROUPNAME"
        If ChkDesignerGrp.Checked Then strSql += vbCrLf + ",DESIGNER"
        strSql += vbCrLf + " )Z"
        strSql += vbCrLf + " Where 1=1"
        If txtFromDay1_NUM.Text <> "" And txtToDay1_NUM.Text <> "" Then
            strSql += vbCrLf + " OR (PCS1 <> '0' OR GRSWT1 <> '0'"
            If chkWithDia.Checked Then
                strSql += vbCrLf + " AND DIAPCS1 <> '0' AND DIAWT1 <> '0'"
            End If
            If chkWithPre.Checked Then
                strSql += vbCrLf + " AND PREPCS1 <> '0' AND PREWT1 <> '0'"
            End If
            If chkWithStone.Checked Then
                strSql += vbCrLf + " AND STNPCS1 <> '0' AND STNWT1 <> '0'"
            End If
            strSql += vbCrLf + " )"
        End If
        If txtFromDay2_NUM.Text <> "" And txtToDay2_NUM.Text <> "" Then
            strSql += vbCrLf + " or (Pcs2 <> '0' or GrsWt2 <> '0'"
            If chkWithDia.Checked Then
                strSql += vbCrLf + " AND DIAPCS2 <> '0' AND DIAWT2 <> '0'"
            End If
            If chkWithPre.Checked Then
                strSql += vbCrLf + " AND PREPCS2 <> '0' AND PREWT2 <> '0'"
            End If
            If chkWithStone.Checked Then
                strSql += vbCrLf + " AND STNPCS2 <> '0' AND STNWT2 <> '0'"
            End If
            strSql += vbCrLf + " )"
        End If
        If txtFromDay3_NUM.Text <> "" And txtToDay3_NUM.Text <> "" Then
            strSql += vbCrLf + " or (Pcs3 <> '0' or GrsWt3 <> '0'"
            If chkWithDia.Checked Then
                strSql += vbCrLf + " AND DIAPCS3 <> '0' AND DIAWT3 <> '0'"
            End If
            If chkWithPre.Checked Then
                strSql += vbCrLf + " AND PREPCS3 <> '0' AND PREWT3 <> '0'"
            End If
            If chkWithStone.Checked Then
                strSql += vbCrLf + " AND STNPCS3 <> '0' AND STNWT3 <> '0'"
            End If
            strSql += vbCrLf + " )"
        End If
        If txtFromDay4_NUM.Text <> "" And txtToDay4_NUM.Text <> "" Then
            strSql += vbCrLf + " or (Pcs4 <> '0' or GrsWt4 <> '0'"
            If chkWithDia.Checked Then
                strSql += vbCrLf + " AND DIAPCS4 <> '0' AND DIAWT4 <> '0'"
            End If
            If chkWithPre.Checked Then
                strSql += vbCrLf + " AND PREPCS4 <> '0' AND PREWT4 <> '0'"
            End If
            If chkWithStone.Checked Then
                strSql += vbCrLf + " AND STNPCS4 <> '0' AND STNWT4 <> '0'"
            End If
            strSql += vbCrLf + " )"
        End If


        strSql += vbCrLf + " or (MaxPcs <> '0' or MaxGrsWt <> '0'"
        If chkWithDia.Checked = True Then
            strSql += vbCrLf + " and MaxDIAPcs <> '0' and MaxDIAWt <> '0'"
        End If
        If chkWithPre.Checked = True Then
            strSql += vbCrLf + " and MaxPREPcs <> '0' and MaxPREWt <> '0'"
        End If
        If chkWithStone.Checked = True Then
            strSql += vbCrLf + " and MaxSTNPcs <> '0' and MaxSTNWt <> '0'"
        End If
        strSql += vbCrLf + " )"
        strSql += vbCrLf + " ORDER BY " + IIf(chkCounterWisegrp.Checked, "ITEMCTRNAME,", "") + IIf(chkitemgrp.Checked, "ITEMGROUPNAME,", "") + IIf(ChkDesignerGrp.Checked, "DESIGNER,", "") + "itemId," + IIf((chkCounterWisegrp.Checked Or chkitemgrp.Checked Or ChkDesignerGrp.Checked), "ITEMNAME,", "") + "result"
        strSql += vbCrLf + " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        If chkSubItem.Checked = True Then
            If chkCounterWisegrp.Checked Or chkitemgrp.Checked Or ChkDesignerGrp.Checked Then
                ''-----------------------------ITEM TOTAL--------------------------
                strSql = "IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "AGE1')>0"
                strSql += " BEGIN "
                strSql += vbCrLf + " IF (SELECT COUNT(*) FROM TEMP" & systemId & "AGE1)>0"
                strSql += vbCrLf + " BEGIN "
                strSql += vbCrLf + "  INSERT INTO TEMP" & systemId & "AGE1(ITEMNAME," + IIf(chkSubItem.Checked = True, "subItemName,", "") + IIf(chkCounterWisegrp.Checked, "ITEMCTRNAME,", IIf(chkitemgrp.Checked, "ITEMGROUPNAME,", IIf(ChkDesignerGrp.Checked, "DESIGNER,", "")))
                If txtFromDay1_NUM.Text <> "" And txtToDay1_NUM.Text <> "" Then strSql += vbCrLf + "PCS1, GRSWT1, "
                If txtFromDay2_NUM.Text <> "" And txtToDay2_NUM.Text <> "" Then strSql += vbCrLf + "PCS2, GRSWT2, "
                If txtFromDay3_NUM.Text <> "" And txtToDay3_NUM.Text <> "" Then strSql += vbCrLf + " PCS3,GRSWT3,"
                If txtFromDay4_NUM.Text <> "" And txtToDay4_NUM.Text <> "" Then strSql += vbCrLf + " PCS4, GRSWT4,"
                strSql += vbCrLf + " MAXPCS, MAXGRSWT,ITEMID,RESULT,STONE,COLHEAD)"
                strSql += vbCrLf + "  SELECT ITEMNAME+' TOTAL'"
                If chkSubItem.Checked = True Then
                    strSql += vbCrLf + ",'' AS SUBITEMNAME"
                End If
                If chkCounterWisegrp.Checked Then strSql += vbCrLf + " ,ITEMCTRNAME"
                If chkitemgrp.Checked Then strSql += vbCrLf + " ,ITEMGROUPNAME"
                If ChkDesignerGrp.Checked Then strSql += vbCrLf + " ,DESIGNER"
                If txtFromDay1_NUM.Text <> "" And txtToDay1_NUM.Text <> "" Then
                    strSql += vbCrLf + "  ,CONVERT(VARCHAR,SUM(CONVERT(INT,ISNULL(PCS1,0)))),"
                    strSql += vbCrLf + "  CONVERT(NUMERIC(20,3),SUM(CONVERT(NUMERIC(18,2),ISNULL(GRSWT1,0)))),"
                End If
                If txtFromDay2_NUM.Text <> "" And txtToDay2_NUM.Text <> "" Then
                    strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(INT,ISNULL(PCS2,0)))),"
                    strSql += vbCrLf + "  CONVERT(NUMERIC(20,3),SUM(CONVERT(NUMERIC(18,2),ISNULL(GRSWT2,0)))),"
                End If
                If txtFromDay3_NUM.Text <> "" And txtToDay3_NUM.Text <> "" Then
                    strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(INT,ISNULL(PCS3,0)))),"
                    strSql += vbCrLf + "  CONVERT(NUMERIC(20,3),SUM(CONVERT(NUMERIC(18,2),ISNULL(GRSWT3,0)))),"
                End If
                If txtFromDay4_NUM.Text <> "" And txtToDay4_NUM.Text <> "" Then
                    strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(INT,ISNULL(PCS4,0)))),"
                    strSql += vbCrLf + "  CONVERT(NUMERIC(20,3),SUM(CONVERT(NUMERIC(18,2),ISNULL(GRSWT4,0)))),"
                End If
                strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(INT,ISNULL(MAXPCS,0)))),"
                strSql += vbCrLf + "  CONVERT(NUMERIC(20,3),SUM(CONVERT(NUMERIC(18,2),ISNULL(MAXGRSWT,0)))),"
                strSql += vbCrLf + "  MAX(ITEMID)+1,'2'RESULT,'1'STONE,'O'COLHEAD"
                strSql += vbCrLf + "  FROM TEMP" & systemId & "AGE1 where RESULT = 1 "
                strSql += vbCrLf + "  GROUP BY ITEMNAME"
                If chkCounterWisegrp.Checked Then strSql += vbCrLf + " ,ITEMCTRNAME"
                If chkitemgrp.Checked Then strSql += vbCrLf + " ,ITEMGROUPNAME"
                If ChkDesignerGrp.Checked Then strSql += vbCrLf + " ,DESIGNER"
                strSql += vbCrLf + " END "
                strSql += vbCrLf + "END "
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
            End If
        End If
        If chkCounterWisegrp.Checked Or chkitemgrp.Checked Or ChkDesignerGrp.Checked Then
            ''-----------------------------GROUP BY TOTAL--------------------------
            strSql = "IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "AGE1')>0"
            strSql += " BEGIN "
            strSql += vbCrLf + " IF (SELECT COUNT(*) FROM TEMP" & systemId & "AGE1)>0"
            strSql += vbCrLf + " BEGIN "
            strSql += vbCrLf + "  INSERT INTO TEMP" & systemId & "AGE1(ITEMNAME," + IIf(chkSubItem.Checked = True, "subItemName,", "") + IIf(chkCounterWisegrp.Checked, "ITEMCTRNAME,", IIf(chkitemgrp.Checked, "ITEMGROUPNAME,", IIf(ChkDesignerGrp.Checked, "DESIGNER,", "")))
            If txtFromDay1_NUM.Text <> "" And txtToDay1_NUM.Text <> "" Then strSql += vbCrLf + "PCS1, GRSWT1, "
            If txtFromDay2_NUM.Text <> "" And txtToDay2_NUM.Text <> "" Then strSql += vbCrLf + "PCS2, GRSWT2, "
            If txtFromDay3_NUM.Text <> "" And txtToDay3_NUM.Text <> "" Then strSql += vbCrLf + " PCS3,GRSWT3,"
            If txtFromDay4_NUM.Text <> "" And txtToDay4_NUM.Text <> "" Then strSql += vbCrLf + " PCS4, GRSWT4,"

            strSql += vbCrLf + " MAXPCS, MAXGRSWT,ITEMID,RESULT,STONE,COLHEAD)"
            strSql += vbCrLf + "  SELECT " & IIf(chkCounterWisegrp.Checked, "ITEMCTRNAME", IIf(chkitemgrp.Checked, "ITEMGROUPNAME", IIf(ChkDesignerGrp.Checked, "DESIGNER", ""))) & "+' TOTAL'," & IIf(chkSubItem.Checked = True, "'',", "") + "" & IIf(chkCounterWisegrp.Checked, "ITEMCTRNAME,", IIf(chkitemgrp.Checked, "ITEMGROUPNAME,", IIf(ChkDesignerGrp.Checked, "DESIGNER,", ""))) & ""
            If txtFromDay1_NUM.Text <> "" And txtToDay1_NUM.Text <> "" Then
                strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(INT,ISNULL(PCS1,0)))),"
                strSql += vbCrLf + "  CONVERT(NUMERIC(20,3),SUM(CONVERT(NUMERIC(18,2),ISNULL(GRSWT1,0)))),"
            End If
            If txtFromDay2_NUM.Text <> "" And txtToDay2_NUM.Text <> "" Then
                strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(INT,ISNULL(PCS2,0)))),"
                strSql += vbCrLf + "  CONVERT(NUMERIC(20,3),SUM(CONVERT(NUMERIC(18,2),ISNULL(GRSWT2,0)))),"
            End If
            If txtFromDay3_NUM.Text <> "" And txtToDay3_NUM.Text <> "" Then
                strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(INT,ISNULL(PCS3,0)))),"
                strSql += vbCrLf + "  CONVERT(NUMERIC(20,3),SUM(CONVERT(NUMERIC(18,2),ISNULL(GRSWT3,0)))),"
            End If
            If txtFromDay4_NUM.Text <> "" And txtToDay4_NUM.Text <> "" Then
                strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(INT,ISNULL(PCS4,0)))),"
                strSql += vbCrLf + "  CONVERT(NUMERIC(20,3),SUM(CONVERT(NUMERIC(18,2),ISNULL(GRSWT4,0)))),"
            End If


            strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(INT,ISNULL(MAXPCS,0)))),"
            strSql += vbCrLf + "  CONVERT(NUMERIC(20,3),SUM(CONVERT(NUMERIC(18,2),ISNULL(MAXGRSWT,0)))),"
            strSql += vbCrLf + "  MAX(ITEMID)+1,'4'RESULT,'1'STONE,'G' COLHEAD"
            strSql += vbCrLf + "  FROM TEMP" & systemId & "AGE1 where RESULT = 1 "
            strSql += vbCrLf + "  GROUP BY " & IIf(chkCounterWisegrp.Checked, "ITEMCTRNAME", IIf(chkitemgrp.Checked, "ITEMGROUPNAME", IIf(ChkDesignerGrp.Checked, "DESIGNER", ""))) & ""
            strSql += vbCrLf + "  ORDER BY " & IIf(chkCounterWisegrp.Checked, "ITEMCTRNAME", IIf(chkitemgrp.Checked, "ITEMGROUPNAME", IIf(ChkDesignerGrp.Checked, "DESIGNER", ""))) & ""
            strSql += vbCrLf + " END "
            strSql += vbCrLf + "END "
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End If
        ''-----------------------------GRAND TOTAL--------------------------
        strSql = " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "AGE1')>0"
        strSql += " BEGIN "
        strSql += vbCrLf + " IF (SELECT COUNT(*) FROM TEMP" & systemId & "AGE1)>0"
        strSql += vbCrLf + " BEGIN "
        strSql += vbCrLf + "  INSERT INTO TEMP" & systemId & "AGE1(ITEMNAME" + IIf(chkSubItem.Checked = True, ",subItemName", "") + IIf(chkCounterWisegrp.Checked, ",ITEMCTRNAME", "") + "" + IIf(chkitemgrp.Checked, ",ITEMGROUPNAME", "") + IIf(ChkDesignerGrp.Checked, ",DESIGNER", "") + ","
        If txtFromDay1_NUM.Text <> "" And txtToDay1_NUM.Text <> "" Then
            strSql += vbCrLf + "  PCS1, GRSWT1,"
            If chkWithDia.Checked Or chkWithPre.Checked Or chkWithStone.Checked Then
                strSql += vbCrLf + "  DIAPCS1, DIAWT1,PREPCS1,PREWT1,STNPCS1,STNWT1,"
            End If
        End If
        If txtFromDay2_NUM.Text <> "" And txtToDay2_NUM.Text <> "" Then
            strSql += vbCrLf + "  PCS2, GRSWT2, "
            If chkWithDia.Checked Or chkWithPre.Checked Or chkWithStone.Checked Then
                strSql += vbCrLf + "  DIAPCS2, DIAWT2,PREPCS2,PREWT2,STNPCS2,STNWT2,"
            End If
        End If
        If txtFromDay3_NUM.Text <> "" And txtToDay3_NUM.Text <> "" Then
            strSql += vbCrLf + "  PCS3,GRSWT3,"
            If chkWithDia.Checked Or chkWithPre.Checked Or chkWithStone.Checked Then
                strSql += vbCrLf + "  DIAPCS3, DIAWT3,PREPCS3,PREWT3,STNPCS3,STNWT3,"
            End If
        End If
        If txtFromDay4_NUM.Text <> "" And txtToDay4_NUM.Text <> "" Then
            strSql += vbCrLf + "  PCS4, GRSWT4,"
            If chkWithDia.Checked Or chkWithPre.Checked Or chkWithStone.Checked Then
                strSql += vbCrLf + "  DIAPCS4, DIAWT4,PREPCS4,PREWT4,STNPCS4,STNWT4,"
            End If
        End If


        strSql += vbCrLf + "  MAXPCS, MAXGRSWT,"
        If chkWithDia.Checked Or chkWithPre.Checked Or chkWithStone.Checked Then
            strSql += vbCrLf + "  MAXDIAPCS, MAXDIAWT,"
            strSql += vbCrLf + "  MAXPREPCS, MAXPREWT,"
            strSql += vbCrLf + "  MAXSTNPCS, MAXSTNWT,"
        End If
        strSql += vbCrLf + "  ITEMID,RESULT,STONE,COLHEAD)"
        strSql += vbCrLf + "  SELECT 'GRAND TOTAL'" + IIf(chkSubItem.Checked, ",''", "") + IIf(chkCounterWisegrp.Checked, ",'ZZZZZZZZZZZ'", "") + "" + IIf(chkitemgrp.Checked, ",'ZZZZZZZZZZZZ'", "") + IIf(ChkDesignerGrp.Checked, ",'ZZZZ'", "") + ","
        If txtFromDay1_NUM.Text <> "" And txtToDay1_NUM.Text <> "" Then
            strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(INT,ISNULL(PCS1,0)))),"
            strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(NUMERIC(18,2),ISNULL(GRSWT1,0)))),"
            If chkWithDia.Checked Or chkWithPre.Checked Or chkWithStone.Checked Then
                strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(INT,ISNULL(DIAPCS1,0)))),"
                strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(NUMERIC(18,3),ISNULL(DIAWT1,0)))),"
                strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(INT,ISNULL(PREPCS1,0)))),"
                strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(NUMERIC(18,3),ISNULL(PREWT1,0)))),"
                strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(INT,ISNULL(STNPCS1,0)))),"
                strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(NUMERIC(18,3),ISNULL(STNWT1,0)))),"
            End If
        End If
        If txtFromDay2_NUM.Text <> "" And txtToDay2_NUM.Text <> "" Then
            strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(INT,ISNULL(PCS2,0)))),"
            strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(NUMERIC(18,2),ISNULL(GRSWT2,0)))),"
            If chkWithDia.Checked Or chkWithPre.Checked Or chkWithStone.Checked Then
                strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(INT,ISNULL(DIAPCS2,0)))),"
                strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(NUMERIC(18,3),ISNULL(DIAWT2,0)))),"
                strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(INT,ISNULL(PREPCS2,0)))),"
                strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(NUMERIC(18,3),ISNULL(PREWT2,0)))),"
                strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(INT,ISNULL(STNPCS2,0)))),"
                strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(NUMERIC(18,3),ISNULL(STNWT2,0)))),"
            End If
        End If
        If txtFromDay3_NUM.Text <> "" And txtToDay3_NUM.Text <> "" Then
            strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(INT,ISNULL(PCS3,0)))),"
            strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(NUMERIC(18,2),ISNULL(GRSWT3,0)))),"
            If chkWithDia.Checked Or chkWithPre.Checked Or chkWithStone.Checked Then
                strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(INT,ISNULL(DIAPCS3,0)))),"
                strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(NUMERIC(18,3),ISNULL(DIAWT3,0)))),"
                strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(INT,ISNULL(PREPCS3,0)))),"
                strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(NUMERIC(18,3),ISNULL(PREWT3,0)))),"
                strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(INT,ISNULL(STNPCS3,0)))),"
                strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(NUMERIC(18,3),ISNULL(STNWT3,0)))),"
            End If
        End If
        If txtFromDay4_NUM.Text <> "" And txtToDay4_NUM.Text <> "" Then
            strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(INT,ISNULL(PCS4,0)))),"
            strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(NUMERIC(18,2),ISNULL(GRSWT4,0)))),"
            If chkWithDia.Checked Or chkWithPre.Checked Or chkWithStone.Checked Then
                strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(INT,ISNULL(DIAPCS4,0)))),"
                strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(NUMERIC(18,3),ISNULL(DIAWT4,0)))),"
                strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(INT,ISNULL(PREPCS4,0)))),"
                strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(NUMERIC(18,3),ISNULL(PREWT4,0)))),"
                strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(INT,ISNULL(STNPCS4,0)))),"
                strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(NUMERIC(18,3),ISNULL(STNWT4,0)))),"
            End If
        End If

        strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(INT,ISNULL(MAXPCS,0)))),"
        strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(NUMERIC(18,2),ISNULL(MAXGRSWT,0)))),"
        If chkWithDia.Checked Or chkWithPre.Checked Or chkWithStone.Checked Then
            strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(INT,ISNULL(MAXDIAPCS,0)))),"
            strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(NUMERIC(18,3),ISNULL(MAXDIAWT,0)))),"
            strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(INT,ISNULL(MAXPREPCS,0)))),"
            strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(NUMERIC(18,3),ISNULL(MAXPREWT,0)))),"
            strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(INT,ISNULL(MAXSTNPCS,0)))),"
            strSql += vbCrLf + "  CONVERT(VARCHAR,SUM(CONVERT(NUMERIC(18,3),ISNULL(MAXSTNWT,0)))),"
        End If
        strSql += vbCrLf + "  MAX(ITEMID)+5,'5'RESULT,'1'STONE,'Z' COLHEAD"
        strSql += vbCrLf + "  FROM TEMP" & systemId & "AGE1 where RESULT = 1 "
        'colhead <> 'S' and colhead <> 'G'
        strSql += vbCrLf + " END "
        strSql += vbCrLf + " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        If chkCounterWisegrp.Checked Or chkitemgrp.Checked Or ChkDesignerGrp.Checked Then
            ''''''''''''''''''''''''''''''''''''''''''''ITEM TITLE'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            strSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "AGE)>0"
            strSql += vbCrLf + " BEGIN "
            strSql += vbCrLf + " IF (SELECT COUNT(*) FROM TEMP" & systemId & "AGE1)>0"
            strSql += vbCrLf + " BEGIN "
            strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "AGE1(ITEMNAME,ITEMID,RESULT," + IIf(chkSubItem.Checked = True, "subItemName,", "") + "STONE,COLHEAD" + IIf(chkCounterWisegrp.Checked, ",ITEMCTRNAME", "") + IIf(chkitemgrp.Checked, ",ITEMGROUPNAME", "") + IIf(ChkDesignerGrp.Checked, ",DESIGNER", "") + ")"
            strSql += vbCrLf + " SELECT DISTINCT ITEMNAME,ITEMID,"
            strSql += vbCrLf + " 0 RESULT" + IIf(chkSubItem.Checked, ",''", "") + ",STONE,'T'" + IIf(chkCounterWisegrp.Checked, ",ITEMCTRNAME", "") + IIf(chkitemgrp.Checked, ",ITEMGROUPNAME", "") + IIf(ChkDesignerGrp.Checked, ",DESIGNER", "") + " FROM TEMP" & systemId & "AGE1 WHERE RESULT = 1"
            strSql += vbCrLf + " END "
            strSql += vbCrLf + " END "
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End If

        If chkCounterWisegrp.Checked Or chkitemgrp.Checked Or ChkDesignerGrp.Checked Then
            ''''''''''''''''''''''''''''''''''''''''GROUP BY TITLE'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            strSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "AGE)>0"
            strSql += vbCrLf + " BEGIN "
            strSql += vbCrLf + " IF (SELECT COUNT(*) FROM TEMP" & systemId & "AGE1)>0"
            strSql += vbCrLf + " BEGIN "
            strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "AGE1(ITEMNAME,ITEMID,RESULT," + IIf(chkSubItem.Checked = True, "subItemName,", "") + "STONE,COLHEAD," & IIf(chkCounterWisegrp.Checked, "ITEMCTRNAME", IIf(chkitemgrp.Checked, "ITEMGROUPNAME", IIf(ChkDesignerGrp.Checked, "DESIGNER", ""))) & ")"
            strSql += vbCrLf + " SELECT DISTINCT " & IIf(chkCounterWisegrp.Checked, "ITEMCTRNAME,", IIf(chkitemgrp.Checked, "ITEMGROUPNAME,", IIf(ChkDesignerGrp.Checked, "DESIGNER,", ""))) & "0,0 RESULT" + IIf(chkSubItem.Checked, ",''", "") + ",STONE,'B'," & IIf(chkCounterWisegrp.Checked, "ITEMCTRNAME", IIf(chkitemgrp.Checked, "ITEMGROUPNAME", IIf(ChkDesignerGrp.Checked, "DESIGNER", ""))) & " FROM TEMP" & systemId & "AGE1 WHERE RESULT = 4"
            strSql += vbCrLf + " END "
            strSql += vbCrLf + " END "
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End If

        strSql = vbCrLf + " ALTER TABLE TEMP" & systemId & "AGE1 ADD TEMPGROUPPCOLUMN VARCHAR(250)"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        If chkCounterWisegrp.Checked Or chkitemgrp.Checked Or ChkDesignerGrp.Checked Then
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''GROUP BY TITLE ORDER''''''''''''''''
            strSql = vbCrLf + " UPDATE TEMP" & systemId & "AGE1 SET TEMPGROUPPCOLUMN = 'ZZZZZZZZZZZZZZ' "
            strSql += vbCrLf + " WHERE COLHEAD = 'Z'"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            strSql = vbCrLf + " UPDATE TEMP" & systemId & "AGE1 SET TEMPGROUPPCOLUMN = 'ZZZZZZZZZZZZZZ' "
            strSql += vbCrLf + " WHERE COLHEAD = 'G'"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            strSql = vbCrLf + " UPDATE TEMP" & systemId & "AGE1 SET TEMPGROUPPCOLUMN= " & IIf(chkCounterWisegrp.Checked, "ITEMCTRNAME", IIf(chkitemgrp.Checked, "ITEMGROUPNAME", IIf(ChkDesignerGrp.Checked, "DESIGNER", "")))
            strSql += vbCrLf + " WHERE COLHEAD <> 'B' AND COLHEAD <> 'G'"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End If
        ''''''''''''FINAL OUTPUT'''''''''''''''''''''''''''''''
        strSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "AGE)>0"
        strSql += vbCrLf + " BEGIN "
        strSql += vbCrLf + " IF (SELECT COUNT(*) FROM TEMP" & systemId & "AGE1)>0"
        strSql += vbCrLf + " BEGIN "
        strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "AGEWISE(" & STRQRYSTRING & ",TOTPCS,TOTGRSWT" & IIf(chkWithDia.Checked Or chkWithPre.Checked Or chkWithStone.Checked, ",TOTDIAPCS,TOTDIAWT,TOTPREPCS,TOTPREWT,TOTSTNPCS,TOTSTNWT", "") & ")"

        strSql += vbCrLf + " SELECT " & STRQRYSTRING & "," & PCSQRY & "," & WTQRY
        If chkWithDia.Checked Or chkWithPre.Checked Or chkWithStone.Checked Then
            strSql += vbCrLf + "," & DIAPCSQRY & "," & DIAWTQRY & ""
            strSql += vbCrLf + "," & PREPCSQRY & "," & PREWTQRY & ""
            strSql += vbCrLf + "," & STNPCSQRY & "," & STNWTQRY & ""
        End If
        '''''''''''''''''ORDER BY FINAL ''''''''''''''''''''''''''''''''''''''
        strSql += vbCrLf + " FROM TEMP" & systemId & "AGE1 ORDER BY " + IIf(chkCounterWisegrp.Checked, "ITEMCTRNAME,", "") + IIf(chkitemgrp.Checked, "ITEMGROUPNAME,", "") + IIf(ChkDesignerGrp.Checked, "DESIGNER,", "") + " TEMPGROUPPCOLUMN "
        If chkCounterWisegrp.Checked Or chkitemgrp.Checked Then
            strSql += vbCrLf + " ,ITEMID,RESULT "
        ElseIf ChkDesignerGrp.Checked = True Then
            strSql += vbCrLf + " ,ITEMID,RESULT "
        Else
            ''''''''''''''''''''''''''''''''''''''''''''GROUP BY ALL UNCHECKED 
            strSql += vbCrLf + " ,RESULT "
        End If
        strSql += vbCrLf + " END "
        strSql += vbCrLf + " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "AGE)>0"
        strSql += vbCrLf + " BEGIN "
        strSql += vbCrLf + " IF (SELECT COUNT(*) FROM TEMP" & systemId & "AGEWISE)>0"
        strSql += vbCrLf + " BEGIN "
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "AGEWISE SET PARTICULAR = ITEMNAME WHERE RESULT IN (0,1,2,5)"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "AGEWISE SET PARTICULAR = ITEMNAME WHERE RESULT  = 3 "
        If chkCounterWisegrp.Checked Then strSql += vbCrLf + " UPDATE TEMP" & systemId & "AGEWISE SET PARTICULAR = ITEMCTRNAME+' TOTAL' WHERE RESULT  = 4 "
        If chkitemgrp.Checked Then strSql += vbCrLf + " UPDATE TEMP" & systemId & "AGEWISE SET PARTICULAR = ITEMGROUPNAME+' TOTAL' WHERE RESULT  = 4 "
        If ChkDesignerGrp.Checked Then strSql += vbCrLf + " UPDATE TEMP" & systemId & "AGEWISE SET PARTICULAR = DESIGNER+' TOTAL' WHERE RESULT  = 4 "
        If chkSubItem.Checked Then
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "AGEWISE SET PARTICULAR = SUBITEMNAME WHERE ISNULL(PARTICULAR,'') <> ISNULL(SUBITEMNAME,'') AND RESULT IN (1) AND ISNULL(SUBITEMNAME,'') <> ''"
        Else
            strSql += vbCrLf + " delete from TEMP" & systemId & "AGEWISE WHERE RESULT =3 or (result=0 and COLHEAD='T')"
        End If
        strSql += vbCrLf + " END "
        strSql += vbCrLf + " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "AGE)>0"
        strSql += vbCrLf + " BEGIN "
        strSql += vbCrLf + " IF (SELECT COUNT(*) FROM TEMP" & systemId & "AGEWISE)>0"
        strSql += vbCrLf + " BEGIN "
        If txtFromDay1_NUM.Text <> "" And txtToDay1_NUM.Text <> "" Then
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "AGEWISE SET PCS1 = NULL WHERE PCS1 = 0 "
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "AGEWISE SET GRSWT1 = NULL WHERE GRSWT1 = 0 "
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "AGEWISE SET DIAPCS1 = NULL WHERE DIAPCS1 = 0 "
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "AGEWISE SET DIAWT1 = NULL WHERE DIAWT1 = 0 "
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "AGEWISE SET PREPCS1 = NULL WHERE PREPCS1 = 0 "
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "AGEWISE SET PREWT1 = NULL WHERE PREWT1 = 0 "
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "AGEWISE SET STNPCS1 = NULL WHERE STNPCS1 = 0 "
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "AGEWISE SET STNWT1 = NULL WHERE STNWT1 = 0 "
        End If
        If txtFromDay2_NUM.Text <> "" And txtToDay2_NUM.Text <> "" Then
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "AGEWISE SET PCS2 = NULL WHERE PCS2 = 0 "
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "AGEWISE SET GRSWT2 = NULL WHERE GRSWT2 = 0 "
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "AGEWISE SET DIAPCS2 = NULL WHERE DIAPCS2 = 0 "
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "AGEWISE SET DIAWT2 = NULL WHERE DIAWT2 = 0 "
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "AGEWISE SET PREPCS2 = NULL WHERE PREPCS2 = 0 "
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "AGEWISE SET PREWT2 = NULL WHERE PREWT2 = 0 "
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "AGEWISE SET STNPCS2 = NULL WHERE STNPCS2 = 0 "
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "AGEWISE SET STNWT2 = NULL WHERE STNWT2 = 0 "
        End If
        If txtFromDay3_NUM.Text <> "" And txtToDay3_NUM.Text <> "" Then
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "AGEWISE SET PCS3 = NULL WHERE PCS3 = 0 "
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "AGEWISE SET GRSWT3 = NULL WHERE GRSWT3 = 0 "
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "AGEWISE SET DIAPCS3 = NULL WHERE DIAPCS3 = 0 "
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "AGEWISE SET DIAWT3 = NULL WHERE DIAWT3 = 0 "
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "AGEWISE SET PREPCS3 = NULL WHERE PREPCS3 = 0 "
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "AGEWISE SET PREWT3 = NULL WHERE PREWT3 = 0 "
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "AGEWISE SET STNPCS3 = NULL WHERE STNPCS3 = 0 "
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "AGEWISE SET STNWT3 = NULL WHERE STNWT3 = 0 "
        End If
        If txtFromDay4_NUM.Text <> "" And txtToDay4_NUM.Text <> "" Then
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "AGEWISE SET PCS4 = NULL WHERE PCS4 = 0 "
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "AGEWISE SET GRSWT4 = NULL WHERE GRSWT4 = 0 "
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "AGEWISE SET DIAPCS4 = NULL WHERE DIAPCS4 = 0 "
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "AGEWISE SET DIAWT4 = NULL WHERE DIAWT4 = 0 "
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "AGEWISE SET PREPCS4 = NULL WHERE PREPCS4 = 0 "
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "AGEWISE SET PREWT4 = NULL WHERE PREWT4 = 0 "
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "AGEWISE SET STNPCS4 = NULL WHERE STNPCS4 = 0 "
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "AGEWISE SET STNWT4 = NULL WHERE STNWT4 = 0 "
        End If
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "AGEWISE SET TOTPCS = NULL WHERE TOTPCS = 0 "
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "AGEWISE SET TOTGRSWT = NULL WHERE TOTGRSWT = 0 "
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "AGEWISE SET MAXPCS = NULL WHERE MAXPCS = 0 "
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "AGEWISE SET MAXGRSWT = NULL WHERE MAXGRSWT = 0 "
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "AGEWISE SET MAXDIAPCS = NULL WHERE MAXDIAPCS = 0 "
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "AGEWISE SET MAXDIAWT = NULL WHERE MAXDIAWT = 0 "
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "AGEWISE SET MAXPREPCS = NULL WHERE MAXPREPCS = 0 "
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "AGEWISE SET MAXPREWT = NULL WHERE MAXPREWT = 0 "
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "AGEWISE SET MAXSTNPCS = NULL WHERE MAXSTNPCS = 0 "
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "AGEWISE SET MAXSTNWT = NULL WHERE MAXSTNWT = 0 "
        If chkWithDia.Checked Then
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "AGEWISE SET TOTDIAPCS = NULL WHERE TOTDIAPCS = 0 "
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "AGEWISE SET TOTDIAWT = NULL WHERE TOTDIAWT = 0 "
        End If
        If chkWithPre.Checked Then
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "AGEWISE SET TOTPREPCS = NULL WHERE TOTPREPCS = 0 "
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "AGEWISE SET TOTPREWT = NULL WHERE TOTPREWT = 0 "
        End If
        If chkWithStone.Checked Then
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "AGEWISE SET TOTSTNPCS = NULL WHERE TOTSTNPCS = 0 "
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "AGEWISE SET TOTSTNWT = NULL WHERE TOTSTNWT = 0 "
        End If
        strSql += vbCrLf + " END "
        strSql += vbCrLf + " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "SELECT PARTICULAR, " & STRQRYSTRING & IIf(chkWithStone.Checked, ",'' SAM1,'' SAM2", "") & ""
        strSql += vbCrLf + " ,TOTPCS,TOTGRSWT "
        strSql += vbCrLf + " " & IIf(chkWithDia.Checked, ",TOTDIAPCS,TOTDIAWT", "") & " "
        strSql += vbCrLf + " " & IIf(chkWithPre.Checked, ",TOTPREPCS,TOTPREWT", "") & " "
        strSql += vbCrLf + " " & IIf(chkWithStone.Checked, ",TOTSTNPCS,TOTSTNWT", "") & " "
        strSql += vbCrLf + " FROM TEMP" & systemId & "AGEWISE ORDER BY SNO "
        dt = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        gridView.DataSource = Nothing
        tabView.Show()
        gridView.DataSource = dt
        GridViewFormat()
        If Not dt.Rows.Count > 0 Then
            MsgBox("Record Not Found", MsgBoxStyle.Exclamation)
            dtpAsOnDate.Select()
            Exit Function
        End If
        If chkason.Checked Then
            lblTitle.Text = "SALES AGEWISE ANALYSIS AS ON " + dtpAsOnDate.Value.ToString("dd-MM-yyyy") & IIf(chkCmbCostcentre.Text <> "" And chkCmbCostcentre.Text <> "ALL", " :" & chkCmbCostcentre.Text, "")
        Else
            lblTitle.Text = "SALES AGEWISE ANALYSIS BETWEEN " & dtpAsOnDate.Value.ToString("dd-MM-yyyy") & "AND " & dtpTo.Value.ToString("dd-MM-yyyy") & "" & IIf(chkCmbCostcentre.Text <> "" And chkCmbCostcentre.Text <> "ALL", " :" & chkCmbCostcentre.Text, "")
        End If
        tabMain.SelectedTab = tabView
        gridView.Focus()
        With gridView
            .Columns("PARTICULAR").Width = 300
            .Columns("RESULT").Visible = False
            .Columns("ITEMID").Visible = False
            .Columns("ITEMNAME").Visible = False
            If chkWithStone.Checked Then
                .Columns("SAM1").Visible = False
                .Columns("SAM2").Visible = False
            End If
            If chkSubItem.Checked Then .Columns("SUBITEMNAME").Visible = False
            If chkCounterWisegrp.Checked Then .Columns("ITEMCTRNAME").Visible = False
            If chkitemgrp.Checked Then .Columns("ITEMGROUPNAME").Visible = False
            If ChkDesignerGrp.Checked Then .Columns("DESIGNER").Visible = False
            .Columns("COLHEAD").Visible = False
            .Columns("STONE").Visible = False

            Dim i As Int16 = IIf((chkCounterWisegrp.Checked Or chkitemgrp.Checked Or ChkDesignerGrp.Checked), 3, 2)
            For CNT As Integer = 0 To .ColumnCount - 1
                With .Columns(CNT)
                    If .Name.Contains("GRSWT") Then .HeaderText = "GRSWT" : .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    If .Name.Contains("PCS") Then .HeaderText = "PCS" : .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    If .Name.Contains("DIAPCS") Then
                        .HeaderText = "DIAPCS"
                        .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                        If chkWithDia.Checked = False Then
                            .Visible = False
                        End If
                    End If
                    If .Name.Contains("DIAWT") Then
                        .HeaderText = "DIAWT"
                        .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                        If chkWithDia.Checked = False Then
                            .Visible = False
                        End If
                    End If
                    If .Name.Contains("STNPCS") Then
                        .HeaderText = "STNPCS"
                        .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                        If chkWithStone.Checked = False Then
                            .Visible = False
                        End If
                    End If
                    If .Name.Contains("STNWT") Then
                        .HeaderText = "STNWT"
                        .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                        If chkWithStone.Checked = False Then
                            .Visible = False
                        End If
                    End If
                    If .Name.Contains("PREPCS") Then
                        .HeaderText = "PREPCS"
                        .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                        If chkWithPre.Checked = False Then
                            .Visible = False
                        End If
                    End If
                    If .Name.Contains("PREWT") Then
                        .HeaderText = "PREWT"
                        .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                        If chkWithPre.Checked = False Then
                            .Visible = False
                        End If
                    End If
                End With
            Next
            Dim CAR As Boolean = True
            i = IIf((chkCounterWisegrp.Checked Or chkitemgrp.Checked Or ChkDesignerGrp.Checked), 4, 3)

            If cnthead > 2 Then
                .Columns("MAXPCS").DefaultCellStyle.BackColor = Color.AliceBlue
                .Columns("MAXGRSWT").DefaultCellStyle.BackColor = Color.AliceBlue
                If chkWithStone.Checked Then
                    .Columns("MAXDIAPCS").DefaultCellStyle.BackColor = Color.AliceBlue
                    .Columns("MAXDIAWT").DefaultCellStyle.BackColor = Color.AliceBlue
                    .Columns("MAXPREPCS").DefaultCellStyle.BackColor = Color.AliceBlue
                    .Columns("MAXPREWT").DefaultCellStyle.BackColor = Color.AliceBlue
                    .Columns("MAXSTNPCS").DefaultCellStyle.BackColor = Color.AliceBlue
                    .Columns("MAXSTNWT").DefaultCellStyle.BackColor = Color.AliceBlue
                End If
            End If
            .ScrollBars = ScrollBars.Both
            For CNT As Integer = 0 To .Columns.Count - 1
                .Columns(CNT).SortMode = DataGridViewColumnSortMode.NotSortable
            Next
        End With
        gridViewHead.DataSource = Nothing
        funcGridHeadWidth()
        AutoReziseToolStripMenuItem.Checked = True
        AutoReziseToolStripMenuItem_Click(Me, New System.EventArgs)
        pnlGridView.BringToFront()
        pnlgrid.BringToFront()
        gridView.BringToFront()
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
                .Columns("PARTICULAR").Width = gridView.Columns("PARTICULAR").Width
                If cnthead > 5 Then
                    .Columns(5).Width = gridView.Columns("MAXPCS").Width + gridView.Columns("MAXGRSWT").Width
                    If chkWithDia.Checked Then .Columns(5).Width += gridView.Columns("MAXDIAPCS").Width + gridView.Columns("MAXDIAWT").Width
                    If chkWithPre.Checked Then .Columns(5).Width += gridView.Columns("MAXPREPCS").Width + gridView.Columns("MAXPREWT").Width
                    If chkWithStone.Checked Then .Columns(5).Width += gridView.Columns("MAXSTNPCS").Width + gridView.Columns("MAXSTNWT").Width
                    .Columns(5).HeaderText = " > " & max

                    .Columns(6).Width = gridView.Columns("TOTPCS").Width + gridView.Columns("TOTGRSWT").Width
                    If chkWithDia.Checked Then .Columns(6).Width += gridView.Columns("TOTDIAPCS").Width + gridView.Columns("TOTDIAWT").Width
                    If chkWithPre.Checked Then .Columns(6).Width += gridView.Columns("TOTPREPCS").Width + gridView.Columns("TOTPREWT").Width
                    If chkWithStone.Checked Then .Columns(6).Width += gridView.Columns("TOTSTNPCS").Width + gridView.Columns("TOTSTNWT").Width
                    .Columns(6).HeaderText = " TOTAL"


                    .Columns(4).Width = gridView.Columns("PCS4").Width + gridView.Columns("GRSWT4").Width
                    If chkWithDia.Checked Then .Columns(4).Width += gridView.Columns("DIAPCS4").Width + gridView.Columns("DIAWT4").Width
                    If chkWithPre.Checked Then .Columns(4).Width += gridView.Columns("PREPCS4").Width + gridView.Columns("PREWT4").Width
                    If chkWithStone.Checked Then .Columns(4).Width += gridView.Columns("STNPCS4").Width + gridView.Columns("STNWT4").Width
                    .Columns(4).HeaderText = txtFromDay4_NUM.Text & " - " & txtToDay4_NUM.Text
                    .Columns(3).Width = gridView.Columns("PCS3").Width + gridView.Columns("GRSWT3").Width
                    If chkWithDia.Checked Then .Columns(3).Width += gridView.Columns("DIAPCS3").Width + gridView.Columns("DIAWT3").Width
                    If chkWithPre.Checked Then .Columns(3).Width += gridView.Columns("PREPCS3").Width + gridView.Columns("PREWT3").Width
                    If chkWithStone.Checked Then .Columns(3).Width += gridView.Columns("STNPCS3").Width + gridView.Columns("STNWT3").Width
                    .Columns(3).HeaderText = txtFromDay3_NUM.Text & " - " & txtToDay3_NUM.Text
                    .Columns(2).Width = gridView.Columns("PCS2").Width + gridView.Columns("GRSWT2").Width
                    If chkWithDia.Checked Then .Columns(2).Width += gridView.Columns("DIAPCS2").Width + gridView.Columns("DIAWT2").Width
                    If chkWithPre.Checked Then .Columns(2).Width += gridView.Columns("PREPCS2").Width + gridView.Columns("PREWT2").Width
                    If chkWithStone.Checked Then .Columns(2).Width += gridView.Columns("STNPCS2").Width + gridView.Columns("STNWT2").Width
                    .Columns(2).HeaderText = txtFromDay2_NUM.Text & " - " & txtToDay2_NUM.Text
                    .Columns(1).Width = gridView.Columns("PCS1").Width + gridView.Columns("GRSWT1").Width
                    If chkWithDia.Checked Then .Columns(1).Width += gridView.Columns("DIAPCS1").Width + gridView.Columns("DIAWT1").Width
                    If chkWithPre.Checked Then .Columns(1).Width += gridView.Columns("PREPCS1").Width + gridView.Columns("PREWT1").Width
                    If chkWithStone.Checked Then .Columns(1).Width += gridView.Columns("STNPCS1").Width + gridView.Columns("STNWT1").Width
                    .Columns(1).HeaderText = txtFromDay1_NUM.Text & " - " & txtToDay1_NUM.Text
                ElseIf cnthead > 4 Then
                    .Columns(4).Width = gridView.Columns("MAXPCS").Width + gridView.Columns("MAXGRSWT").Width
                    If chkWithDia.Checked Then .Columns(4).Width += gridView.Columns("MAXDIAPCS").Width + gridView.Columns("MAXDIAWT").Width
                    If chkWithPre.Checked Then .Columns(4).Width += gridView.Columns("MAXPREPCS").Width + gridView.Columns("MAXPREWT").Width
                    If chkWithStone.Checked Then .Columns(4).Width += gridView.Columns("MAXSTNPCS").Width + gridView.Columns("MAXSTNWT").Width
                    .Columns(4).HeaderText = " > " & max

                    .Columns(5).Width = gridView.Columns("TOTPCS").Width + gridView.Columns("TOTGRSWT").Width
                    If chkWithDia.Checked Then .Columns(5).Width += gridView.Columns("TOTDIAPCS").Width + gridView.Columns("TOTDIAWT").Width
                    If chkWithPre.Checked Then .Columns(5).Width += gridView.Columns("TOTPREPCS").Width + gridView.Columns("TOTPREWT").Width
                    If chkWithStone.Checked Then .Columns(5).Width += gridView.Columns("TOTSTNPCS").Width + gridView.Columns("TOTSTNWT").Width
                    .Columns(5).HeaderText = " TOTAL"


                    .Columns(3).Width = gridView.Columns("PCS3").Width + gridView.Columns("GRSWT3").Width
                    If chkWithDia.Checked Then .Columns(3).Width += gridView.Columns("DIAPCS3").Width + gridView.Columns("DIAWT3").Width
                    If chkWithPre.Checked Then .Columns(3).Width += gridView.Columns("PREPCS3").Width + gridView.Columns("PREWT3").Width
                    If chkWithStone.Checked Then .Columns(3).Width += gridView.Columns("STNPCS3").Width + gridView.Columns("STNWT3").Width
                    .Columns(3).HeaderText = txtFromDay3_NUM.Text & " - " & txtToDay3_NUM.Text
                    .Columns(2).Width = gridView.Columns("PCS2").Width + gridView.Columns("GRSWT2").Width
                    If chkWithDia.Checked Then .Columns(2).Width += gridView.Columns("DIAPCS2").Width + gridView.Columns("DIAWT2").Width
                    If chkWithPre.Checked Then .Columns(2).Width += gridView.Columns("PREPCS2").Width + gridView.Columns("PREWT2").Width
                    If chkWithStone.Checked Then .Columns(2).Width += gridView.Columns("STNPCS2").Width + gridView.Columns("STNWT2").Width
                    .Columns(2).HeaderText = txtFromDay2_NUM.Text & " - " & txtToDay2_NUM.Text
                    .Columns(1).Width = gridView.Columns("PCS1").Width + gridView.Columns("GRSWT1").Width
                    If chkWithDia.Checked Then .Columns(1).Width += gridView.Columns("DIAPCS1").Width + gridView.Columns("DIAWT1").Width
                    If chkWithPre.Checked Then .Columns(1).Width += gridView.Columns("PREPCS1").Width + gridView.Columns("PREWT1").Width
                    If chkWithStone.Checked Then .Columns(1).Width += gridView.Columns("STNPCS1").Width + gridView.Columns("STNWT1").Width
                    .Columns(1).HeaderText = txtFromDay1_NUM.Text & " - " & txtToDay1_NUM.Text
                ElseIf cnthead > 3 Then
                    .Columns(3).Width = gridView.Columns("MAXPCS").Width + gridView.Columns("MAXGRSWT").Width
                    If chkWithDia.Checked Then .Columns(3).Width += gridView.Columns("MAXDIAPCS").Width + gridView.Columns("MAXDIAWT").Width
                    If chkWithPre.Checked Then .Columns(3).Width += gridView.Columns("MAXPREPCS").Width + gridView.Columns("MAXPREWT").Width
                    If chkWithStone.Checked Then .Columns(3).Width += gridView.Columns("MAXSTNPCS").Width + gridView.Columns("MAXSTNWT").Width
                    .Columns(3).HeaderText = " > " & max

                    .Columns(4).Width = gridView.Columns("TOTPCS").Width + gridView.Columns("TOTGRSWT").Width
                    If chkWithDia.Checked Then .Columns(4).Width += gridView.Columns("TOTDIAPCS").Width + gridView.Columns("TOTDIAWT").Width
                    If chkWithPre.Checked Then .Columns(4).Width += gridView.Columns("TOTPREPCS").Width + gridView.Columns("TOTPREWT").Width
                    If chkWithStone.Checked Then .Columns(4).Width += gridView.Columns("TOTSTNPCS").Width + gridView.Columns("TOTSTNWT").Width
                    .Columns(4).HeaderText = " TOTAL"

                    .Columns(2).Width = gridView.Columns("PCS2").Width + gridView.Columns("GRSWT2").Width
                    If chkWithDia.Checked Then .Columns(2).Width += gridView.Columns("DIAPCS2").Width + gridView.Columns("DIAWT2").Width
                    If chkWithPre.Checked Then .Columns(2).Width += gridView.Columns("PREPCS2").Width + gridView.Columns("PREWT2").Width
                    If chkWithStone.Checked Then .Columns(2).Width += gridView.Columns("STNPCS2").Width + gridView.Columns("STNWT2").Width
                    .Columns(2).HeaderText = txtFromDay2_NUM.Text & " - " & txtToDay2_NUM.Text
                    .Columns(1).Width = gridView.Columns("PCS1").Width + gridView.Columns("GRSWT1").Width

                    If chkWithDia.Checked Then .Columns(1).Width += gridView.Columns("DIAPCS1").Width + gridView.Columns("DIAWT1").Width
                    If chkWithPre.Checked Then .Columns(1).Width += gridView.Columns("PREPCS1").Width + gridView.Columns("PREWT1").Width
                    If chkWithStone.Checked Then .Columns(1).Width += gridView.Columns("STNPCS1").Width + gridView.Columns("STNWT1").Width
                    .Columns(1).HeaderText = txtFromDay1_NUM.Text & " - " & txtToDay1_NUM.Text
                ElseIf cnthead > 2 Then
                    .Columns(2).Width = gridView.Columns("MAXPCS").Width + gridView.Columns("MAXGRSWT").Width
                    If chkWithDia.Checked Then .Columns(2).Width += gridView.Columns("MAXDIAPCS").Width + gridView.Columns("MAXDIAWT").Width
                    If chkWithPre.Checked Then .Columns(2).Width += gridView.Columns("MAXPREPCS").Width + gridView.Columns("MAXPREWT").Width
                    If chkWithStone.Checked Then .Columns(2).Width += gridView.Columns("MAXSTNPCS").Width + gridView.Columns("MAXSTNWT").Width
                    .Columns(2).HeaderText = " > " & max

                    .Columns(3).Width = gridView.Columns("TOTPCS").Width + gridView.Columns("TOTGRSWT").Width
                    If chkWithDia.Checked Then .Columns(3).Width += gridView.Columns("TOTDIAPCS").Width + gridView.Columns("TOTDIAWT").Width
                    If chkWithPre.Checked Then .Columns(3).Width += gridView.Columns("TOTPREPCS").Width + gridView.Columns("TOTPREWT").Width
                    If chkWithStone.Checked Then .Columns(3).Width += gridView.Columns("TOTSTNPCS").Width + gridView.Columns("TOTSTNWT").Width
                    .Columns(3).HeaderText = " TOTAL"


                    .Columns(1).Width = gridView.Columns("PCS1").Width + gridView.Columns("GRSWT1").Width
                    If chkWithDia.Checked Then .Columns(1).Width += gridView.Columns("DIAPCS1").Width + gridView.Columns("DIAWT1").Width
                    If chkWithPre.Checked Then .Columns(1).Width += gridView.Columns("PREPCS1").Width + gridView.Columns("PREWT1").Width
                    If chkWithStone.Checked Then .Columns(1).Width += gridView.Columns("STNPCS1").Width + gridView.Columns("STNWT1").Width
                    .Columns(1).HeaderText = txtFromDay1_NUM.Text & " - " & txtToDay1_NUM.Text
                Else
                    .Columns(1).Width = gridView.Columns("MAXPCS").Width + gridView.Columns("MAXGRSWT").Width
                    If chkWithDia.Checked Then .Columns(1).Width += gridView.Columns("MAXDIAPCS").Width + gridView.Columns("MAXDIAWT").Width
                    If chkWithPre.Checked Then .Columns(1).Width += gridView.Columns("MAXPREPCS").Width + gridView.Columns("MAXPREWT").Width
                    If chkWithStone.Checked Then .Columns(1).Width += gridView.Columns("MAXSTNPCS").Width + gridView.Columns("MAXSTNWT").Width
                    .Columns(1).HeaderText = " > " & max

                    .Columns(2).Width = gridView.Columns("TOTPCS").Width + gridView.Columns("TOTGRSWT").Width
                    If chkWithDia.Checked Then .Columns(2).Width += gridView.Columns("TOTDIAPCS").Width + gridView.Columns("TOTDIAWT").Width
                    If chkWithPre.Checked Then .Columns(2).Width += gridView.Columns("TOTPREPCS").Width + gridView.Columns("TOTPREWT").Width
                    If chkWithStone.Checked Then .Columns(2).Width += gridView.Columns("TOTSTNPCS").Width + gridView.Columns("TOTSTNWT").Width
                    .Columns(2).HeaderText = " TOTAL"
                End If


                .RowHeadersVisible = False
                .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                .ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
                .Height = .ColumnHeadersHeight
                .Columns("PARTICULAR").HeaderText = ""
                .Columns("SCROLL").Width = 20
                .Columns("SCROLL").HeaderText = ""
                Dim colWid As Integer = 0
                For cnt As Integer = 0 To gridView.ColumnCount - 1
                    If gridView.Columns(cnt).Visible Then colWid += gridView.Columns(cnt).Width
                Next
                If colWid >= gridView.Width Then
                    gridViewHead.Columns("SCROLL").Visible = CType(gridView.Controls(1), VScrollBar).Visible
                    gridViewHead.Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
                Else
                    gridViewHead.Columns("SCROLL").Visible = False
                End If
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                gridView.Invalidate()
                For Each dgvCol As DataGridViewColumn In gridView.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End With
        End If
    End Function
    Private Sub chkCmbItem_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chkCmbItem.KeyDown
        If e.KeyCode = Keys.Enter Then
            Dim Catcode As String = Nothing
            Dim MetalId As String = Nothing
            If cmbCategory.Text <> "ALL" Then
                strSql = "SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & cmbCategory.Text.ToString & "'"
                Catcode = GetSqlValue(cn, strSql)
            End If
            If cmbmetal.Text <> "ALL" Then
                strSql = "SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbmetal.Text.ToString & "'"
                MetalId = GetSqlValue(cn, strSql)
            End If

            strSql = "SELECT 'ALL' ITEMGROUPNAME,1 RESULT "
            strSql += " UNION ALL"
            strSql += " SELECT DISTINCT"
            strSql += " (SELECT GROUPNAME FROM " & cnAdminDb & "..ITEMGROUPMAST AS IG  WHERE IG.GROUPID = I.ITEMGROUP)  AS ITEMGROUPNAME, "
            strSql += " 2 AS  RESULT FROM " & cnAdminDb & "..ITEMMAST AS I WHERE 1 = 1 "
            strSql += " AND I.ITEMGROUP <> 0"
            If cmbCategory.Text <> "ALL" Then
                strSql += " AND I.CATCODE = '" & Catcode & "' "
            End If
            If cmbmetal.Text <> "ALL" Then
                strSql += " AND I.METALID = '" & MetalId & "' "
            End If
            If chkCmbItem.Text <> "ALL" Then
                strSql += " AND I.ITEMNAME IN ('" & chkCmbItem.Text.Replace(",", "','") & "')"
            End If
            strSql += " ORDER BY RESULT,ITEMGROUPNAME"
            dtItemgroup = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtItemgroup)
            BrighttechPack.GlobalMethods.FillCombo(chkCmbItemgroup, dtItemgroup, "ITEMGROUPNAME", True, "ALL")
        End If
    End Sub
    Private Sub chkCmbItem_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCmbItem.TextChanged
        Loadsubitem()
    End Sub
    Private Function Loadsubitem()
        'Load subItemName
        strSql = " SELECT 'ALL' SUBITEMNAME,1 RESULT UNION ALL"
        strSql += " SELECT SUBITEMNAME,2 RESULT FROM " & cnAdminDb & "..SUBITEMMAST "
        If chkCmbItem.Text <> "" And chkCmbItem.Text <> "ALL" Then strSql += " WHERE ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN ('" & chkCmbItem.Text.Replace(",", "','") & "'))"
        strSql += " ORDER BY RESULT,SUBITEMNAME"
        dtSubitem = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtSubitem)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbSubitem, dtSubitem, "SUBITEMNAME", True, "ALL")
    End Function

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
        AutoReziseToolStripMenuItem.Checked = False
        strSql = " IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "AGEDIFFDAYS') > 0 DROP TABLE TEMP" & systemId & "AGEDIFFDAYS"
        strSql += " CREATE TABLE TEMP" & systemId & "AGEDIFFDAYS"
        strSql += " ("
        strSql += " FROMDAY INT"
        strSql += " ,TODAY INT"
        strSql += " )"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strSql = ""
        If Val(txtFromDay1_NUM.Text) < Val(txtToDay1_NUM.Text) Then
            strSql += " INSERT INTO TEMP" & systemId & "AGEDIFFDAYS(FROMDAY,TODAY)VALUES(" & Val(txtFromDay1_NUM.Text) & "," & Val(txtToDay1_NUM.Text) & ")"
        End If
        If Val(txtFromDay2_NUM.Text) < Val(txtToDay2_NUM.Text) Then
            strSql += " INSERT INTO TEMP" & systemId & "AGEDIFFDAYS(FROMDAY,TODAY)VALUES(" & Val(txtFromDay2_NUM.Text) & "," & Val(txtToDay2_NUM.Text) & ")"
        End If
        If Val(txtFromDay3_NUM.Text) < Val(txtToDay3_NUM.Text) Then
            strSql += " INSERT INTO TEMP" & systemId & "AGEDIFFDAYS(FROMDAY,TODAY)VALUES(" & Val(txtFromDay3_NUM.Text) & "," & Val(txtToDay3_NUM.Text) & ")"
        End If
        If Val(txtFromDay4_NUM.Text) < Val(txtToDay4_NUM.Text) Then
            strSql += " INSERT INTO TEMP" & systemId & "AGEDIFFDAYS(FROMDAY,TODAY)VALUES(" & Val(txtFromDay4_NUM.Text) & "," & Val(txtToDay4_NUM.Text) & ")"
        End If
        If strSql <> "" Then
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End If
        Prop_Sets()
        funcSearch()
    End Sub

    Function GridViewFormat() As Integer
        For Each dgvRow As DataGridViewRow In gridView.Rows
            With dgvRow
                Select Case .Cells("COLHEAD").Value.ToString
                    Case "S"
                        .DefaultCellStyle.Font = reportSubTotalStyle.Font
                        .DefaultCellStyle.ForeColor = reportSubTotalStyle.ForeColor
                    Case "T"
                        .Cells("PARTICULAR").Style.BackColor = reportHeadStyle.BackColor
                        .Cells("PARTICULAR").Style.Font = reportHeadStyle.Font
                    Case "B"
                        .Cells("PARTICULAR").Style.BackColor = reportHeadStyle1.BackColor
                        .Cells("PARTICULAR").Style.Font = reportHeadStyle2.Font
                        .DefaultCellStyle.ForeColor = reportHeadStyle2.ForeColor
                    Case "G"
                        .DefaultCellStyle.Font = reportHeadStyle1.Font
                        .DefaultCellStyle.ForeColor = reportHeadStyle1.ForeColor
                        .Cells("PARTICULAR").Style.BackColor = reportHeadStyle1.BackColor
                    Case "Z"
                        .DefaultCellStyle.Font = reportHeadStyle.Font
                        .DefaultCellStyle.ForeColor = reportHeadStyle.ForeColor
                        .Cells("PARTICULAR").Style.BackColor = reportHeadStyle.BackColor
                    Case "O"
                        .DefaultCellStyle.Font = reportHeadStyle.Font
                        .DefaultCellStyle.ForeColor = reportHeadStyle.ForeColor
                        .Cells("PARTICULAR").Style.BackColor = Color.Lavender
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

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = "X" Or e.KeyChar = "x" Then
            Me.btnExcel_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = "P" Then
            btnPrint_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = "D" Then
            'DetailedView()
        End If
    End Sub

    Private Sub DetailedView()
        Dim itemName As String = gridView.CurrentRow.Cells("ITEMNAME").Value.ToString

        Dim subItemName As String
        Dim Tablecode As String = ""
        Dim Designer As String = ""

        If gridView.Columns.Contains("SUBITEMNAME") = True Then subItemName = gridView.CurrentRow.Cells("SUBITEMNAME").Value.ToString Else subItemName = ""
        If gridView.Columns.Contains("DESIGNER") = True Then Designer = gridView.CurrentRow.Cells("DESIGNER").Value.ToString Else Designer = ""
        If gridView.Columns.Contains("TABLECODE") = True Then Tablecode = gridView.CurrentRow.Cells("TABLECODE").Value.ToString Else Tablecode = ""
        If itemName = "" And subItemName = "" Then Exit Sub
        Dim filtration As String = funcFiltration(True)
        If subItemName <> "" Then
            If MessageBox.Show("Do you want to view item detail?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) <> Windows.Forms.DialogResult.Yes Then
                'subitem detail
                filtration += " AND SUBITEMID = (SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & subItemName & "' and itemid = (select itemid from " & cnAdminDb & "..itemmast where itemname = '" & itemName & "'))"
            End If
        End If
        'item detail
        filtration += " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & itemName & "')"

        If Designer <> "" Then
            filtration += " AND DESIGNERID = (SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & Designer & "')"
        End If
        If Tablecode <> "" Then
            filtration += " AND TABLECODE = '" & Tablecode & "'"
        End If
        strSql = " IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "DETAGEANALYSIS') > 0 DROP TABLE TEMP" & systemId & "DETAGEANALYSIS"
        strSql += " CREATE TABLE TEMP" & systemId & "DETAGEANALYSIS"
        strSql += " ("
        strSql += " PARTICULAR VARCHAR(200)"
        strSql += " ,TAGNO VARCHAR(12)"
        strSql += " ,PCS INT,GRSWT NUMERIC(15,3),NETWT NUMERIC(15,3),RECDATE VARCHAR(12),DAYS INT"
        strSql += " ,COLHEAD VARCHAR(3)"
        strSql += " ,SNO INT IDENTITY(1,1)"
        strSql += " ,RESULT INT"
        strSql += " )"
        strSql += " "
        strSql += " IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "DETAGE') > 0 DROP TABLE TEMP" & systemId & "DETAGE"
        strSql += " SELECT "
        strSql += " CASE WHEN SUBITEMID = 0 THEN (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)"
        strSql += " ELSE (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = T.SUBITEMID) END AS PARTICULAR"
        strSql += " ,TAGNO,PCS,GRSWT,NETWT,CONVERT(vARCHAR(10),RECDATE,103)RECDATE,DATEDIFF(D," & IIf(chkActualDate.Checked, "ACTUALRECDATE", "RECDATE") & ",'" & IIf(chkason.Checked, dtpAsOnDate.Value.ToString("yyyy-MM-dd"), dtpTo.Value.ToString("yyyy-MM-dd")) & "')+1 AS DAYS,TAGVAL"
        strSql += " INTO TEMP" & systemId & "DETAGE"
        strSql += " FROM " & cnAdminDb & "..ITEMTAG AS T"
        strSql += filtration
        strSql += " "

        strSql += " DECLARE @FROMDAY AS INT"
        strSql += " DECLARE @TODAY AS INT"
        strSql += " SELECT @FROMDAY = 0,@TODAY = 0"
        strSql += " DECLARE CUR CURSOR"
        strSql += " FOR SELECT FROMDAY,TODAY FROM TEMP" & systemId & "AGEDIFFDAYS"
        strSql += " OPEN CUR"
        strSql += " WHILE 1=1"
        strSql += " BEGIN"
        strSql += " 	FETCH NEXT FROM CUR INTO @FROMDAY,@TODAY"
        strSql += " 	IF @@FETCH_STATUS = -1 BREAK"
        strSql += " 	/** Inserting Days Title **/"
        strSql += " 	INSERT INTO TEMP" & systemId & "DETAGEANALYSIS(PARTICULAR,COLHEAD,RESULT)"
        strSql += " 	SELECT CONVERT(VARCHAR,@FROMDAY) + ' TO ' + CONVERT(VARCHAR,@TODAY),'T'COLHEAD,0 RESULT"
        strSql += " "
        strSql += " 	/** Inserting Days Values **/"
        strSql += " 	INSERT INTO TEMP" & systemId & "DETAGEANALYSIS(PARTICULAR,TAGNO,PCS,GRSWT,NETWT,RECDATE,DAYS,RESULT)"
        strSql += " 	SELECT PARTICULAR,TAGNO,PCS,GRSWT,NETWT,RECDATE,DAYS,1 RESULT"
        strSql += " 	FROM TEMP" & systemId & "DETAGE "
        strSql += " 	WHERE DAYS BETWEEN @FROMDAY AND @TODAY"
        strSql += " 	ORDER BY TAGVAL"
        strSql += " 	"
        strSql += " 	/** Inserting Sub Total **/"
        strSql += " 	IF (SELECT COUNT(*) FROM TEMP" & systemId & "DETAGE WHERE DAYS BETWEEN @FROMDAY AND @TODAY)>0"
        strSql += " 	BEGIN"
        strSql += " 		INSERT INTO TEMP" & systemId & "DETAGEANALYSIS(PARTICULAR,PCS,GRSWT,NETWT,RESULT,COLHEAD)"
        strSql += " 		SELECT CONVERT(VARCHAR,@FROMDAY) + ' TO ' + CONVERT(VARCHAR,@TODAY) + ' => '"
        strSql += " 		+ CONVERT(VARCHAR,COUNT(*)) + ' Tags' AS PARTICULAR,SUM(PCS),SUM(GRSWT),SUM(NETWT),2 RESULT,'S'COLHEAD"
        strSql += " 		FROM TEMP" & systemId & "DETAGE "
        strSql += " 		WHERE DAYS BETWEEN @FROMDAY AND @TODAY"
        strSql += " 	END"
        strSql += " "
        strSql += " 	"
        strSql += " END"
        strSql += " CLOSE CUR"
        strSql += " DEALLOCATE CUR"
        strSql += " "
        strSql += " /** Above Given Days **/"
        strSql += " /** Inserting Days Title **/"
        strSql += " DECLARE @MAXDAY INT"
        strSql += " SELECT @MAXDAY = ISNULL((SELECT MAX(TODAY) FROM TEMP" & systemId & "AGEDIFFDAYS),0)"
        strSql += " INSERT INTO TEMP" & systemId & "DETAGEANALYSIS(PARTICULAR,COLHEAD,RESULT)"
        strSql += " SELECT '> ' + CONVERT(VARCHAR,@MAXDAY),'T'COLHEAD,0 RESULT"
        strSql += " "
        strSql += " /** Inserting Days Values **/"
        strSql += " INSERT INTO TEMP" & systemId & "DETAGEANALYSIS(PARTICULAR,TAGNO,PCS,GRSWT,NETWT,RECDATE,DAYS,RESULT)"
        strSql += " SELECT PARTICULAR,TAGNO,PCS,GRSWT,NETWT,RECDATE,DAYS,1 RESULT"
        strSql += " FROM TEMP" & systemId & "DETAGE "
        strSql += " WHERE DAYS > @MAXDAY"
        strSql += " ORDER BY TAGVAL"
        strSql += " "
        strSql += " /** Inserting Sub Total **/"
        strSql += " IF (SELECT COUNT(*) FROM TEMP" & systemId & "DETAGE WHERE DAYS > @MAXDAY)>0"
        strSql += " BEGIN"
        strSql += " 	INSERT INTO TEMP" & systemId & "DETAGEANALYSIS(PARTICULAR,PCS,GRSWT,NETWT,RESULT,COLHEAD)"
        strSql += " 	SELECT '> '+CONVERT(VARCHAR,@MAXDAY) + ' => '"
        strSql += " 	+ CONVERT(VARCHAR,COUNT(*)) + ' Tags' AS PARTICULAR,SUM(PCS),SUM(GRSWT),SUM(NETWT),2 RESULT,'S'COLHEAD"
        strSql += " 	FROM TEMP" & systemId & "DETAGE "
        strSql += " 	WHERE DAYS > @MAXDAY"
        strSql += " END"
        strSql += " "
        strSql += " /** Inserting Grand Total **/"
        strSql += " IF (SELECT COUNT(*) FROM TEMP" & systemId & "DETAGEANALYSIS WHERE RESULT = 1)>0"
        strSql += " BEGIN"
        strSql += " INSERT INTO TEMP" & systemId & "DETAGEANALYSIS(PARTICULAR,PCS,GRSWT,NETWT,RESULT,COLHEAD)"
        strSql += " SELECT 'GRAND TOTAL => ' + CONVERT(VARCHAR,COUNT(*)) + ' Tags' AS PARTICULAR"
        strSql += " ,SUM(PCS),SUM(GRSWT),SUM(NETWT),3 RESULT,'G'COLHEAD FROM TEMP" & systemId & "DETAGE"
        strSql += " END"
        strSql += " "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " SELECT * FROM TEMP" & systemId & "DETAGEANALYSIS"
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
        objGridShower.Text = "DETAILED AGING ANALYSIS"
        Dim tit As String = Nothing
        If subItemName <> "" Then
            tit += subItemName.ToUpper
        Else
            tit += itemName.ToUpper
        End If
        If chkason.Checked Then
            tit += " DETAILED AGING ANALYSIS REPORT AS ON " & dtpAsOnDate.Text & "" + vbCrLf
        Else
            tit += " DETAILED AGING ANALYSIS REPORT BETWEEN " & dtpAsOnDate.Text & " AND " + dtpTo.Text + " " + vbCrLf
        End If


        objGridShower.lblTitle.Text = tit
        'AddHandler objGridShower.gridView.ColumnWidthChanged, AddressOf gridView_ColumnWidthChanged
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

    End Sub

    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView)
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

            FormatGridColumns(dgv, False, False, , False)
            FillGridGroupStyle_KeyNoWise(dgv)
        End With
    End Sub


    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print, gridViewHead)
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
        chkason.Checked = True
        chkason.Focus()
        chkason.Select()
        Prop_Gets()
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGen
        chkason.Focus()
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export, gridViewHead)
        End If
    End Sub

    Private Sub gridView_Scroll(ByVal sender As Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles gridView.Scroll
        Try
            If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
                gridViewHead.HorizontalScrollingOffset = e.NewValue
                gridViewHead.Columns("SCROLL").Visible = CType(gridView.Controls(1), VScrollBar).Visible
                gridViewHead.Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New frmSalesAgeWiseAnalysis_Properties
        obj.p_cmbCategory = cmbCategory.Text
        GetChecked_CheckedList(chkCmbItem, obj.p_chkCmbItem)
        obj.p_cmbmetal = cmbmetal.Text
        GetChecked_CheckedList(chkCmbSubitem, obj.p_chkCmbSubItem)
        obj.p_cmbCounter = cmbCounter.Text
        obj.p_txtFromDay1_NUM = txtFromDay1_NUM.Text
        obj.p_txtToDay1_NUM = txtToDay1_NUM.Text
        obj.p_txtFromDay2_NUM = txtFromDay2_NUM.Text
        obj.p_txtToDay2_NUM = txtToDay2_NUM.Text
        obj.p_txtFromDay3_NUM = txtFromDay3_NUM.Text
        obj.p_txtToDay3_NUM = txtToDay3_NUM.Text
        obj.p_txtFromDay4_NUM = txtFromDay4_NUM.Text
        obj.p_txtToDay4_NUM = txtToDay4_NUM.Text
        obj.p_chkWithStone = chkWithStone.Checked
        obj.p_chkWithDia = chkWithDia.Checked
        obj.p_chkWithPre = chkWithPre.Checked
        obj.p_chkCounterWise = chkCounterWisegrp.Checked
        obj.p_chkitemgrp = chkitemgrp.Checked
        obj.p_chkDesignergrp = ChkDesignerGrp.Checked
        obj.p_chkSubItem = chkSubItem.Checked
        obj.p_chkActualDate = chkActualDate.Checked
        GetChecked_CheckedList(chkCmbtablecode, obj.p_chkCmbtablecode)
        GetChecked_CheckedList(chkCmbDesigner, obj.p_chkCmbDesigner)
        SetSettingsObj(obj, Me.Name, GetType(frmSalesAgeWiseAnalysis_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmSalesAgeWiseAnalysis_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmSalesAgeWiseAnalysis_Properties))
        cmbCategory.Text = obj.p_cmbCategory
        SetChecked_CheckedList(chkCmbItem, obj.p_chkCmbItem, "ALL")
        SetChecked_CheckedList(chkCmbDesigner, obj.p_chkCmbDesigner, "ALL")
        cmbmetal.Text = obj.p_cmbmetal
        SetChecked_CheckedList(chkCmbSubitem, obj.p_chkCmbSubItem, "ALL")
        cmbCounter.Text = obj.p_cmbCounter
        txtFromDay1_NUM.Text = obj.p_txtFromDay1_NUM
        txtToDay1_NUM.Text = obj.p_txtToDay1_NUM
        txtFromDay2_NUM.Text = obj.p_txtFromDay2_NUM
        txtToDay2_NUM.Text = obj.p_txtToDay2_NUM
        txtFromDay3_NUM.Text = obj.p_txtFromDay3_NUM
        txtToDay3_NUM.Text = obj.p_txtToDay3_NUM
        txtFromDay4_NUM.Text = obj.p_txtFromDay4_NUM
        txtToDay4_NUM.Text = obj.p_txtToDay4_NUM
        chkWithStone.Checked = obj.p_chkWithStone
        chkWithDia.Checked = obj.p_chkWithDia
        chkWithPre.Checked = obj.p_chkWithPre
        chkCounterWisegrp.Checked = obj.p_chkCounterWise
        chkitemgrp.Checked = obj.p_chkitemgrp
        ChkDesignerGrp.Checked = obj.p_chkDesignergrp
        chkSubItem.Checked = obj.p_chkSubItem
        chkActualDate.Checked = obj.p_chkActualDate
        SetChecked_CheckedList(chkCmbtablecode, obj.p_chkCmbtablecode, "ALL")
        If chkSubItem.Checked = False Then
            chkCmbSubitem.Enabled = False
        End If
    End Sub

    Private Sub chkSubItem_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSubItem.CheckedChanged
        If chkSubItem.Checked = False Then
            chkCmbSubitem.Enabled = False
        Else
            chkCmbSubitem.Enabled = True
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
    Private Sub cmbmetal_SelectedIndexChanged1(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbmetal.SelectedIndexChanged
        If cmbmetal.Text <> "ALL" Then
            strSql = "SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME='" & Trim(cmbmetal.Text.ToString) & "'"
            Dim metalid = GetSqlValue(cn, strSql)
            strSql = " SELECT 'ALL'ITEMNAME,1 RESULT UNION ALL"
            strSql += vbCrLf + " SELECT ITEMNAME,2 RESULT FROM " & cnAdminDb & "..ITEMMAST WHERE METALID='" & metalid & "'"
            strSql += vbCrLf + " ORDER BY RESULT,ITEMNAME"
            dtItem = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtItem)
            BrighttechPack.GlobalMethods.FillCombo(chkCmbItem, dtItem, "ITEMNAME", True, "ALL")
        Else
            strSql = " SELECT 'ALL'ITEMNAME,1 RESULT UNION ALL"
            strSql += " SELECT ITEMNAME,2 RESULT FROM " & cnAdminDb & "..ITEMMAST ORDER BY RESULT,ITEMNAME"
            dtItem = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtItem)
            BrighttechPack.GlobalMethods.FillCombo(chkCmbItem, dtItem, "ITEMNAME", True, "ALL")
        End If

    End Sub

    Private Sub chkitemgrp_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkitemgrp.CheckedChanged
        If chkitemgrp.Checked = True Then
            chkCounterWisegrp.Checked = False
            ChkDesignerGrp.Checked = False
        End If
    End Sub

    Private Sub chkCounterWise_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCounterWisegrp.CheckedChanged
        If chkCounterWisegrp.Checked = True Then
            chkitemgrp.Checked = False
            ChkDesignerGrp.Checked = False
        End If
    End Sub

    Private Sub ChkDesignerGrp_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkDesignerGrp.CheckedChanged
        If ChkDesignerGrp.Checked = True Then
            chkitemgrp.Checked = False
            chkCounterWisegrp.Checked = False
        End If
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
            funcGridHeadWidth()
        End If
    End Sub
End Class

Public Class frmSalesAgeWiseAnalysis_Properties
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
    Private chkCmbItem As New List(Of String)
    Public Property p_chkCmbItem() As List(Of String)
        Get
            Return chkCmbItem
        End Get
        Set(ByVal value As List(Of String))
            chkCmbItem = value
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
    Private chkCmbSubItem As New List(Of String)
    Public Property p_chkCmbSubItem() As List(Of String)
        Get
            Return chkCmbSubItem
        End Get
        Set(ByVal value As List(Of String))
            chkCmbSubItem = value
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
    Private chkWithDia As Boolean = False
    Public Property p_chkWithDia() As Boolean
        Get
            Return chkWithDia
        End Get
        Set(ByVal value As Boolean)
            chkWithDia = value
        End Set
    End Property
    Private chkWithPre As Boolean = False
    Public Property p_chkWithPre() As Boolean
        Get
            Return chkWithPre
        End Get
        Set(ByVal value As Boolean)
            chkWithPre = value
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
    Private chkCatgrp As Boolean = False
    Public Property p_chkCatgrp() As Boolean
        Get
            Return chkCatgrp
        End Get
        Set(ByVal value As Boolean)
            chkCatgrp = value
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
    Private chkCmbCostCentre As New List(Of String)
    Public Property p_chkCmbCostCentre() As List(Of String)
        Get
            Return chkCmbCostCentre
        End Get
        Set(ByVal value As List(Of String))
            chkCmbCostCentre = value
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
End Class