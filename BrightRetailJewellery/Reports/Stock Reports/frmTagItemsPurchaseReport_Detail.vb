Imports System.Data.OleDb
Imports System.Math
Imports System.Collections
Public Class frmTagItemsPurchaseReport_Detail
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim objGridShower As frmGridDispDia
    Dim defaultPic As String = GetAdmindbSoftValue("PICPATH")
    Dim ItemList As New ArrayList()
    Dim tagNo As New ArrayList()
    Dim metalId As New ArrayList()


    Private Sub frmTagItemsPurchaseReport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmTagItemsPurchaseReport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not defaultPic.EndsWith("\") Then defaultPic += "\"
        grpContainer.Location = New Point((ScreenWid - grpContainer.Width) / 2, ((ScreenHit - 128) - grpContainer.Height) / 2)
        strSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE ACTIVE = 'Y' ORDER BY DISPLAYORDER"
        FillCheckedListBox(strSql, chkLstMetal)
        If GetAdmindbSoftValue("COSTCENTRE") = "Y" Then
            chkLstCostCentre.Enabled = True
            chkLstCostCentre.Items.Clear()
            strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            'FillCheckedListBox(strSql, chkLstCostCentre)
            Dim dt As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            For i As Integer = 0 To dt.Rows.Count - 1
                If cnCostName = dt.Rows(i).Item("COSTNAME").ToString And cnDefaultCostId = False Then
                    chkLstCostCentre.Items.Add(dt.Rows(i).Item("COSTNAME").ToString, True)
                Else
                    chkLstCostCentre.Items.Add(dt.Rows(i).Item("COSTNAME").ToString, False)
                End If
            Next
            If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkLstCostCentre.Enabled = False
        Else
            chkLstCostCentre.Enabled = False
            chkLstCostCentre.Items.Clear()
        End If
        strSql = " SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE ACTIVE = 'Y' ORDER BY DESIGNERNAME"
        FillCheckedListBox(strSql, chkLstDesigner)
        strSql = " SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ACTIVE = 'Y' ORDER BY ITEMCTRNAME"
        FillCheckedListBox(strSql, chkLstItemCounter)
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        objGPack.TextClear(Me)
        ' chkStockOnly.Checked = True
        'chkWithStudded.Checked = True
        dtpAsOnDate.Value = GetServerDate()
        dtpAsOnDate.Select()
        Prop_Gets()
        rbtStockOnly.Checked = True
    End Sub
    Private Sub chkMetalSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkMetalSelectAll.CheckedChanged
        ' GetMetalId()
        SetChecked_CheckedList(chkLstMetal, chkMetalSelectAll.Checked)
    End Sub

    Private Sub chkCostCentreSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCostCentreSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCostCentre, chkCostCentreSelectAll.Checked)
    End Sub

    Private Sub chkDesignerSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkDesignerSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstDesigner, chkDesignerSelectAll.Checked)
    End Sub

    Private Sub chkItemCounterSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkItemCounterSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstItemCounter, chkItemCounterSelectAll.Checked)
    End Sub

    Private Sub chkItemAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkItemSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstItem, chkItemSelectAll.Checked)
    End Sub

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        'If rbtDetailed.Checked = True Then
        DataGridLoad()

        'Dim count As Integer
        'Dim chkCostName As String = GetChecked_CheckedList(chkLstCostCentre)
        'Dim chkItemCounter As String = GetChecked_CheckedList(chkLstItemCounter)
        'Dim chkMetalName As String = GetChecked_CheckedList(chkLstMetal)
        'Dim chkItemName As String = GetChecked_CheckedList(chkLstItem)
        'Dim chkDesigner As String = GetChecked_CheckedList(chkLstDesigner)
        'strSql = vbCrLf + " IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'TEMPTAGVIEW')>0 DROP TABLE TEMPTAGVIEW"
        'strSql += vbCrLf + " SELECT "
        'strSql += vbCrLf + " (SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = T.DESIGNERID)aS DESIGNER"
        'strSql += vbCrLf + " ,T.TAGNO,T.STYLENO,T.PCS,p.purNETWT NETWT"
        ''strSql += vbCrLf + " ,ISNULL((SELECT PTAX FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = (SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)),0)PURTAX"
        'strSql += vbCrLf + " ,T.TRANINVNO,T.SUPBILLNO"
        'strSql += vbCrLf + " ,CASE WHEN T.ISSDATE IS NOT NULL THEN 'DATE : ' + CONVERT(VARCHAR,T.ISSDATE,103) + ';PCS : ' + CONVERT(VARCHAR,T.ISSPCS) + ';WEIGHT : ' + CONVERT(VARCHAR,T.ISSWT) ELSE '' END AS SALESDET"
        'strSql += vbCrLf + " ,(SELECT ITEMNAME FROM  " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS ITEM"
        'strSql += vbCrLf + " ,(SELECT SUBITEMNAME FROM  " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = T.ITEMID AND SUBITEMID = T.SUBITEMID)AS SUBITEM"
        'strSql += vbCrLf + " ,T.TAGVAL"
        'strSql += vbCrLf + " ,P.PURVALUE,P.PURMC,P.PURTAX"
        'strSql += vbCrLf + " ,T.SNO,T.ITEMID,T.PCTFILE AS PCTFILE"
        'strSql += vbCrLf + " INTO TEMPTAGVIEW"
        'strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T"
        'strSql += vbCrLf + " LEFT OUTER JOIN " & cnAdminDb & "..PURITEMTAG AS P ON P.TAGSNO = T.SNO"
        'strSql += vbCrLf + " WHERE T.RECDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
        'If chkCostName <> "" Then strSql += vbCrLf + " AND T.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        'If chkItemCounter <> "" Then strSql += vbCrLf + " AND T.ITEMCTRID IN (SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME IN (" & chkItemCounter & "))"
        'If chkItemName <> "" Then strSql += vbCrLf + " AND T.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & chkItemName & "))"
        'If chkMetalName <> "" Then strSql += vbCrLf + " AND T.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
        'If chkDesigner <> "" Then strSql += vbCrLf + " AND T.DESIGNERID IN (SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME IN (" & chkDesigner & "))"
        'If txtTranInvNo.Text <> "" Then strSql += vbCrLf + " AND T.TRANINVNO = '" & txtTranInvNo.Text & "'"
        'If chkStockOnly.Checked Then strSql += vbCrLf + " AND T.ISSDATE IS NULL"
        'strSql += vbCrLf + " AND T.COMPANYID = '" & GetStockCompId() & "'"

        'strSql += vbCrLf + " IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'TEMPTAGSTONEVIEW')>0 DROP TABLE TEMPTAGSTONEVIEW"
        'strSql += vbCrLf + " SELECT "
        'strSql += vbCrLf + " SUM(CASE WHEN IM.DIASTONE = 'D' THEN PST.STNWT ELSE 0 END) AS DIAWT"
        'strSql += vbCrLf + " ,SUM(CASE WHEN IM.DIASTONE = 'D' THEN PST.PURAMT ELSE 0 END) AS DIAAMT"
        'strSql += vbCrLf + " ,SUM(CASE WHEN IM.DIASTONE = 'S' THEN PST.STNWT ELSE 0 END) AS STNWT"
        'strSql += vbCrLf + " ,SUM(CASE WHEN IM.DIASTONE = 'S' THEN PST.PURAMT ELSE 0 END) AS STNAMT"
        'strSql += vbCrLf + " ,PST.TAGSNO"
        'strSql += vbCrLf + "  INTO TEMPTAGSTONEVIEW"
        'strSql += vbCrLf + " FROM " & cnAdminDb & "..PURITEMTAGSTONE AS PST"
        'strSql += vbCrLf + " INNER JOIN TEMPTAGVIEW AS T ON T.SNO = PST.TAGSNO"
        'strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = PST.STNITEMID"
        'strSql += vbCrLf + "  GROUP BY PST.TAGSNO"
        'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        'cmd.CommandTimeout = 1000
        'cmd.ExecuteNonQuery()

        'strSql = " IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "TAGPURDET')>0 DROP TABLE TEMP" & systemId & "TAGPURDET"
        ''If rbtSummary.Checked = True Then
        ''    strSql += vbCrLf + " SELECT CONVERT(VARCHAR(60),item) PARTICULAR,NULL TAGNO,NULL STYLENO"
        ''    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),SUM(PURVALUE) + SUM(CONVERT(NUMERIC(15,2),((PURVALUE * PURTAX)/100)))) TOTAL"
        ''    strSql += vbCrLf + " ,SUM(PCS) PCS,SUM(NETWT)NETWT,SUM(ISNULL(PURVALUE,0)-ISNULL(STNAMT,0)-ISNULL(DIAAMT,0)-ISNULL(PURMC,0)) AS NETAMT,SUM(CONVERT(NUMERIC(15,4),DIAWT))DIAWT,SUM(CONVERT(NUMERIC(15,2),DIAAMT))DIAAMT,SUM(CONVERT(NUMERIC(15,3),STNWT))STNWT"
        ''    strSql += vbCrLf + " ,SUM(CONVERT(NUMERIC(15,2),STNAMT))STNAMT,SUM(CONVERT(NUMERIC(15,2),PURMC))PURMC,SUM(CONVERT(NUMERIC(15,2),PURVALUE)) GRANDTOTAL,SUM(CONVERT(NUMERIC(15,2),((PURVALUE * PURTAX)/100))) PURTAX,NULL TRANINVNO,NULL SUPBILLNO,CONVERT(VARCHAR,NULL) AS ITEM,1 RESULT,' 'COLHEAD,NULL TAGVAL,NULL PCTFILE,ITEMID"
        ''Else
        ''    strSql += vbCrLf + " SELECT CONVERT(VARCHAR(60),PARTICULAR)PARTICULAR,TAGNO,STYLENO"
        ''    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),PURVALUE + ((PURVALUE * PURTAX)/100)) TOTAL"
        ''    strSql += vbCrLf + " ,PCS,NETWT,ISNULL(PURVALUE,0)-ISNULL(STNAMT,0)-ISNULL(DIAAMT,0)-ISNULL(PURMC,0) AS NETAMT,CONVERT(NUMERIC(15,4),DIAWT)DIAWT,CONVERT(NUMERIC(15,2),DIAAMT)DIAAMT,CONVERT(NUMERIC(15,3),STNWT)STNWT"
        ''    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),STNAMT)STNAMT,CONVERT(NUMERIC(15,2),PURMC)PURMC,CONVERT(NUMERIC(15,2),PURVALUE) GRANDTOTAL,CONVERT(NUMERIC(15,2),((PURVALUE * PURTAX)/100)) PURTAX,TRANINVNO,SUPBILLNO,ITEM,1 RESULT,' 'COLHEAD,CONVERT(INT,TAGVAL)TAGVAL,PCTFILE,ITEMID"
        ''End If
        'If rbtSummary.Checked = True Then
        '    strSql += vbCrLf + " SELECT CONVERT(VARCHAR(60),item) PARTICULAR,NULL DESIGNER,NULL TAGNO,NULL STYLENO"
        '    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),SUM(PURVALUE)) TOTAL"
        '    strSql += vbCrLf + " ,SUM(PCS) PCS,SUM(NETWT)NETWT,SUM(ISNULL(PURVALUE,0)-ISNULL(STNAMT,0)-ISNULL(DIAAMT,0)-ISNULL(PURMC,0)) AS NETAMT,SUM(CONVERT(NUMERIC(15,4),DIAWT))DIAWT,SUM(CONVERT(NUMERIC(15,2),DIAAMT))DIAAMT,SUM(CONVERT(NUMERIC(15,3),STNWT))STNWT"
        '    strSql += vbCrLf + " ,SUM(CONVERT(NUMERIC(15,2),STNAMT))STNAMT,SUM(CONVERT(NUMERIC(15,2),PURMC))PURMC,SUM(CONVERT(NUMERIC(15,2),PURVALUE-ISNULL(PURTAX,0))) GRANDTOTAL,SUM(CONVERT(NUMERIC(15,2),PURTAX)) PURTAX,NULL TRANINVNO,NULL SUPBILLNO,CONVERT(VARCHAR,NULL) AS ITEM,1 RESULT,' 'COLHEAD,NULL TAGVAL,NULL PCTFILE,ITEMID"
        'Else
        '    strSql += vbCrLf + " SELECT CONVERT(VARCHAR(60),PARTICULAR)PARTICULAR,DESIGNER,TAGNO,STYLENO"
        '    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),PURVALUE) TOTAL"
        '    strSql += vbCrLf + " ,PCS,NETWT,ISNULL(PURVALUE,0)-ISNULL(STNAMT,0)-ISNULL(DIAAMT,0)-ISNULL(PURMC,0) AS NETAMT,CONVERT(NUMERIC(15,4),DIAWT)DIAWT,CONVERT(NUMERIC(15,2),DIAAMT)DIAAMT,CONVERT(NUMERIC(15,3),STNWT)STNWT"
        '    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),STNAMT)STNAMT,CONVERT(NUMERIC(15,2),PURMC)PURMC,CONVERT(NUMERIC(15,2),PURVALUE-ISNULL(PURTAX,0)) GRANDTOTAL,CONVERT(NUMERIC(15,2),PURTAX) PURTAX,TRANINVNO,SUPBILLNO,ITEM,1 RESULT,' 'COLHEAD,CONVERT(INT,TAGVAL)TAGVAL,PCTFILE,ITEMID"
        'End If
        'strSql += vbCrLf + " INTO TEMP" & systemId & "TAGPURDET"
        'strSql += vbCrLf + " FROM"
        'strSql += vbCrLf + " ("
        'strSql += vbCrLf + " SELECT CASE WHEN ISNULL(T.SUBITEM,'') <> '' THEN T.SUBITEM ELSE T.ITEM END AS PARTICULAR"
        'strSql += vbCrLf + " ,T.TAGNO,T.STYLENO,T.PCS,T.NETWT"
        'strSql += vbCrLf + " ,T.PURMC,T.PURVALUE"
        ''strSql += vbCrLf + " ,ISNULL((SELECT PTAX FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = (SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)),0)PURTAX"
        'strSql += vbCrLf + " ,T.TRANINVNO,T.SUPBILLNO"
        'strSql += vbCrLf + " ,T.SALESDET"
        'strSql += vbCrLf + " ,T.ITEM"
        'strSql += vbCrLf + " ,T.TAGVAL,T.PURTAX"
        'strSql += vbCrLf + " ,ST.DIAWT,ST.DIAAMT,ST.STNWT,ST.STNAMT,T.PCTFILE,T.ITEMID,T.DESIGNER"
        'strSql += vbCrLf + " FROM TEMPTAGVIEW AS T"
        'strSql += vbCrLf + " LEFT OUTER JOIN TEMPTAGSTONEVIEW AS ST ON ST.TAGSNO = T.SNO"
        'strSql += vbCrLf + " )X"
        'If rbtSummary.Checked = True Then
        '    strSql += vbCrLf + " GROUP BY item,ITEMID "
        'End If
        'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        'cmd.ExecuteNonQuery()
        'strSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "TAGPURDET)>0"
        'strSql += vbCrLf + " BEGIN"
        'If rbtDetailed.Checked = True Then
        '    strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "TAGPURDET(ITEM,PARTICULAR,RESULT,COLHEAD)"
        '    strSql += vbCrLf + " SELECT DISTINCT ITEM,ITEM,0 RESULT,'T'COLHEAD FROM TEMP" & systemId & "TAGPURDET"

        '    strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "TAGPURDET(ITEM,PARTICULAR,RESULT,COLHEAD,PCS,NETWT,NETAMT,DIAWT,DIAAMT,STNWT,STNAMT,PURMC,GRANDTOTAL,PURTAX)"
        '    strSql += vbCrLf + " SELECT DISTINCT ITEM,'SUB TOTAL',2 RESULT,'S'COLHEAD"
        '    strSql += vbCrLf + " ,SUM(PCS)PCS,SUM(NETWT),SUM(NETAMT),SUM(DIAWT),SUM(DIAAMT),SUM(STNWT),SUM(STNAMT),SUM(PURMC),SUM(GRANDTOTAL),SUM(PURTAX)"
        '    strSql += vbCrLf + " FROM TEMP" & systemId & "TAGPURDET GROUP BY ITEM"
        'End If
        'strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "TAGPURDET(ITEM,PARTICULAR,RESULT,COLHEAD,TOTAL,PCS,NETWT,NETAMT,DIAWT,DIAAMT,STNWT,STNAMT,PURMC,GRANDTOTAL ,PURTAX)"
        'strSql += vbCrLf + " SELECT DISTINCT 'ZZZZZ','GRAND TOTAL',3 RESULT,'G'COLHEAD,"
        'strSql += vbCrLf + " SUM(TOTAL)TOTAL,SUM(PCS)PCS,SUM(NETWT),SUM(NETAMT),SUM(DIAWT),SUM(DIAAMT),SUM(STNWT),SUM(STNAMT),SUM(PURMC),SUM(GRANDTOTAL),SUM(PURTAX)"
        'strSql += vbCrLf + " FROM TEMP" & systemId & "TAGPURDET WHERE RESULT = 1"
        'strSql += vbCrLf + " END"
        'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        'cmd.ExecuteNonQuery()
        'strSql = " SELECT * FROM TEMP" & systemId & "TAGPURDET ORDER BY ITEM,RESULT,TAGVAL"
        'Dim dtGrid As New DataTable
        'dtGrid.Columns.Add("KEYNO", GetType(Integer))
        'dtGrid.Columns("KEYNO").AutoIncrement = True
        'dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        'dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        'dtGrid.Columns.Add("TAGIMAGE", GetType(Image))
        'da = New OleDbDataAdapter(strSql, cn)
        'da.Fill(dtGrid)
        ''dtGrid.Columns.Add(dtGrid.Rows.Item(0).Item("METALID").ToString)
        'dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        'If Not dtGrid.Rows.Count > 0 Then
        '    MsgBox("Record not found", MsgBoxStyle.Information)
        '    Exit Sub
        'End If              
    End Sub
    Private Sub DataGridLoad()

        Dim int As Integer
        Dim dtItem As New DataTable
        Dim dts As New DataTable
        Dim chkCostName As String = GetChecked_CheckedList(chkLstCostCentre)
        Dim chkItemCounter As String = GetChecked_CheckedList(chkLstItemCounter)
        Dim chkMetalName As String = GetChecked_CheckedList(chkLstMetal)
        Dim chkItemName As String = GetChecked_CheckedList(chkLstItem)
        Dim chkDesigner As String = GetChecked_CheckedList(chkLstDesigner)
        If Not CheckBckDays(userId, Me.Name, dtpAsOnDate.Value) Then dtpAsOnDate.Focus() : Exit Sub
        strSql = " IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP_DETAILS')>0 DROP TABLE TEMPTABLEDB..TEMP_DETAILS"
        strSql += vbCrLf + " SELECT ITEMID,TAGNO,SUM(GRSWT) AS GRSWT,'B' TTYPE,(SELECT METALID FROM " & cnAdminDb & "..ITEMMAST  AS I WHERE I.ITEMID = T.ITEMID)AS METALID,"
        strSql += vbCrLf + " SUM(GRSWT) AS NETWT,'G' AS STONEUNIT, (SELECT SUM(PURVALUE) FROM " & cnAdminDb & "..PURITEMTAG AS P WHERE P.ITEMID = T.ITEMID) AS PURVALUE "
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP_DETAILS"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T"
        strSql += vbCrLf + " GROUP BY ITEMID,TAGNO"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT ITEMID,TAGNO,0 AS GRSWT,'' TTYPE,(SELECT METALID FROM " & cnAdminDb & "..CATEGORY  AS I WHERE I.CATCODE = T.CATCODE)AS METALID,"
        strSql += vbCrLf + " SUM(GRSWT) AS NETWT,'G' AS STONEUNIT,0 AS PURVALUE FROM " & cnAdminDb & "..ITEMTAGMETAL AS T"
        strSql += vbCrLf + " GROUP BY ITEMID,TAGNO,CATCODE"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT ITEMID,TAGNO,0 AS GRSWT,'' TTYPE,(SELECT METALID FROM " & cnAdminDb & "..ITEMMAST  AS I WHERE I.ITEMID  = T.STNITEMID)AS METALID,"
        strSql += vbCrLf + " SUM((CASE WHEN STONEUNIT = 'G' THEN STNWT  ELSE STNWT /5  END)) AS NETWT,STONEUNIT,0 AS PURVALUE FROM " & cnAdminDb & "..ITEMTAGSTONE AS T"
        strSql += vbCrLf + " GROUP BY ITEMID,TAGNO,STNITEMID ,STONEUNIT "
        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMPTAGPURDET')>0 DROP TABLE TEMPTABLEDB..TEMPTAGPURDET"
        strSql += vbCrLf + "  SELECT ITEM,TAGNO,GRSWT AS NETWT,"

        Dim SQL As String
        If chkMetalName = "" Then
            SQL = vbCrLf + " SELECT METALNAME,METALID FROM " & cnAdminDb & "..METALMAST ORDER BY DISPLAYORDER"
        Else
            SQL = vbCrLf + " SELECT METALNAME,METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ") ORDER BY DISPLAYORDER"
        End If
        da = New OleDbDataAdapter(SQL, cn)
        Dim DT As New DataTable
        da.Fill(DT)
        If DT.Rows.Count > 0 Then
            For int = 0 To DT.Rows.Count - 1
                If DT.Rows(int).Item("METALID").ToString = "G" Then
                    strSql += vbCrLf + " CONVERT(NUMERIC(15,3)," & DT.Rows(int).Item("METALNAME") & ")" & DT.Rows(int).Item("METALNAME") & ","
                End If
            Next
            For int = 0 To DT.Rows.Count - 1
                If DT.Rows(int).Item("METALID").ToString <> "G" Then
                    strSql += vbCrLf + " CONVERT(NUMERIC(15,3)," & DT.Rows(int).Item("METALNAME") & ")" & DT.Rows(int).Item("METALNAME") & ","
                End If
            Next
        End If

        strSql += vbCrLf + " 1 RESULT,CONVERT(VARCHAR(2),'') COLHEAD,"
        strSql += vbCrLf + " CONVERT(VARCHAR(60),ITEMID)ITEMID "
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMPTAGPURDET"
        strSql += vbCrLf + " FROM ("
        strSql += vbCrLf + " SELECT (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST AS A WHERE A.ITEMID=T.ITEMID) AS ITEM,TAGNO,SUM(GRSWT) AS GRSWT,"

        If DT.Rows.Count > 0 Then
            For int = 0 To DT.Rows.Count - 1
                'IF INT = 0 THEN
                If DT.Rows(int).Item("METALID").ToString = "G" Then
                    strSql = strSql + "(SUM(CASE WHEN METALID = '" & DT.Rows(int).Item("METALID") & "' THEN NETWT ELSE (CASE WHEN TTYPE = '' THEN NETWT *-1 ELSE 0 END) END)) AS " & DT.Rows(int).Item("METALNAME") & ","
                    'STRSQL = STRSQL + ","
                End If
                'END IF
            Next

            For int = 0 To DT.Rows.Count - 1
                If DT.Rows(int).Item("METALID").ToString <> "G" Then
                    strSql = strSql + "(SUM(CASE WHEN METALID = '" & DT.Rows(int).Item("METALID") & "' THEN NETWT ELSE 0 END)) AS " & DT.Rows(int).Item("METALNAME") & ""
                    strSql = strSql + ","
                End If
            Next

        End If

        strSql = strSql + " ITEMID "
        strSql = strSql + " FROM TEMPTABLEDB..TEMP_DETAILS AS T GROUP BY TAGNO,ITEMID)X"

        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMPFINALREPORT')>0 DROP TABLE TEMPTABLEDB..TEMPFINALREPORT "
        strSql += vbCrLf + " SELECT CONVERT(VARCHAR(60),PARTICULAR)PARTICULAR,DESIGNER,TAGNO,STYLENO,CONVERT(NUMERIC(15,2),TOTAL)TOTAL,"
        strSql += vbCrLf + " PCS,NETWT,ISNULL(TOTAL,0)-ISNULL(STNAMT,0)-ISNULL(PURMC,0) AS NETAMT,"

        If DT.Rows.Count > 0 Then
            For int = 0 To DT.Rows.Count - 1
                If DT.Rows(int).Item("METALID").ToString = "G" Then
                    strSql += vbCrLf + " CONVERT(NUMERIC(15,3)," & DT.Rows(int).Item("METALNAME") & ")" & DT.Rows(int).Item("METALNAME") & ","
                End If
            Next

            For int = 0 To DT.Rows.Count - 1
                If DT.Rows(int).Item("METALID").ToString <> "G" Then
                    strSql += vbCrLf + " CONVERT(NUMERIC(15,3)," & DT.Rows(int).Item("METALNAME") & ")" & DT.Rows(int).Item("METALNAME") & ","
                End If
            Next
        End If
        strSql += vbCrLf + " CONVERT(NUMERIC(15,3),DIAWT)DIAWT ,CONVERT(NUMERIC(15,2),DIARATE)DIARATE,CONVERT(NUMERIC(15,2),DIAVALUE)DIAVALUE, "
        strSql += vbCrLf + " CONVERT(NUMERIC(15,3),STNWT)STNWT ,CONVERT(NUMERIC(15,2),STNAMT)STNAMT, "
        strSql += vbCrLf + " CONVERT(NUMERIC(15,2),PURMC)PURMC,CONVERT(NUMERIC(15,2),TOTAL-ISNULL(PURTAX,0)) GRANDTOTAL, CONVERT(NUMERIC(15,2),PURTAX) PURTAX,"
        strSql += vbCrLf + " TRANINVNO,SUPBILLNO,ITEM,1 RESULT,1 ITEMORDER,CONVERT(VARCHAR(2),'') COLHEAD, ITEMID,MTAGNO"
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMPFINALREPORT"
        strSql += vbCrLf + " FROM"
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " SELECT DISTINCT (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST AS I WHERE I.ITEMID=A.ITEMID) AS PARTICULAR,(SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER  WHERE DESIGNERID=A.DESIGNERID ) AS DESIGNER,"
        strSql += vbCrLf + " A.TAGNO, A.STYLENO ,PURVALUE AS TOTAL,PCS,E.NETWT,"
        If DT.Rows.Count > 0 Then
            For int = 0 To DT.Rows.Count - 1
                If DT.Rows(int).Item("METALID").ToString = "G" Then
                    strSql += "E." & DT.Rows(int).Item("METALNAME") & ","
                End If
            Next
            For int = 0 To DT.Rows.Count - 1
                If DT.Rows(int).Item("METALID").ToString <> "G" Then
                    strSql += "E." & DT.Rows(int).Item("METALNAME") & ","
                End If
            Next
        End If
        strSql += vbCrLf + " NULL AS DIAWT,NULL AS DIARATE,NULL AS DIAVALUE"
        strSql += vbCrLf + " ,NULL AS STNWT,NULL AS STNAMT"
        strSql += vbCrLf + " ,PURMC,PURVALUE AS GRANDTOTAL,PURTAX,TRANINVNO,SUPBILLNO,"
        'STRSQL += " CASE WHEN A.ISSDATE IS NOT NULL THEN 'DATE : ' + CONVERT(VARCHAR,A.ISSDATE,103),"
        strSql += vbCrLf + " A.ITEMID,ITEM,RESULT,COLHEAD,A.TAGNO AS MTAGNO FROM " & cnAdminDb & "..ITEMTAG A"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMTAGMETAL B ON A.TAGNO=B.TAGNO"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMTAGSTONE C ON A.TAGNO=C.TAGNO "
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..PURITEMTAG D ON A.TAGNO=D.TAGNO"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..PURITEMTAGSTONE PS ON A.TAGNO=PS.TAGNO"
        strSql += vbCrLf + " LEFT JOIN TEMPTABLEDB..TEMPTAGPURDET E ON A.TAGNO=E.TAGNO"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID = C.STNITEMID "
        strSql += vbCrLf + " WHERE  A.RECDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "' and RESULT=1"
        If rbtStockOnly.Checked Then strSql += vbCrLf + " AND a.RECDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "' AND (a.ISSDATE IS NULL OR a.ISSDATE > '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "')"
        If rbtReceipt.Checked Then strSql += vbCrLf + " AND a.RECDATE BETWEEN '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        If rbtIssue.Checked Then strSql += vbCrLf + " AND a.ISSDATE  BETWEEN '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        If chkCostName <> "" Then strSql += vbCrLf + " AND A.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        If chkItemCounter <> "" Then strSql += vbCrLf + " AND A.ITEMCTRID IN (SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME IN (" & chkItemCounter & "))"
        If chkItemName <> "" Then strSql += vbCrLf + " AND A.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & chkItemName & "))"
        If chkMetalName <> "" Then strSql += vbCrLf + " AND A.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
        If chkDesigner <> "" Then strSql += vbCrLf + " AND DESIGNERID IN (SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME IN (" & chkDesigner & "))"
        If txtTranInvNo.Text <> "" Then strSql += vbCrLf + " AND TRANINVNO = '" & txtTranInvNo.Text & "'"
        strSql += vbCrLf + " GROUP BY A.TAGNO,A.STYLENO,PURVALUE ,PCS,E.NETWT,"
        If DT.Rows.Count > 0 Then
            For int = 0 To DT.Rows.Count - 1
                If DT.Rows(int).Item("METALID").ToString = "G" Then
                    strSql += "E." & DT.Rows(int).Item("METALNAME") & ","
                End If
            Next

            For int = 0 To DT.Rows.Count - 1
                If DT.Rows(int).Item("METALID").ToString <> "G" Then
                    strSql += "E." & DT.Rows(int).Item("METALNAME") & ","
                End If
            Next
        End If

        strSql += vbCrLf + " PURMC,PURVALUE,PURTAX,TRANINVNO,SUPBILLNO,a.ITEMID,RESULT,COLHEAD,ITEM,DESIGNERID "
        strSql += vbCrLf + " )X"

        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMPFINALREPORT)>0"
        strSql += vbCrLf + " BEGIN"
        strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPFINALREPORT (PARTICULAR,MTAGNO,DIAWT,DIARATE,DIAVALUE,STNWT,STNAMT,"
        strSql += vbCrLf + " RESULT,ITEMORDER,COLHEAD,ITEMID,ITEM)"
        strSql += vbCrLf + " SELECT CASE WHEN ISNULL(SI.SUBITEMNAME,'')<>'' THEN SI.SUBITEMNAME ELSE IM.ITEMNAME END AS PARTICULAR"
        strSql += vbCrLf + " ,S.TAGNO,NULL AS DIAWT,NULL DIARATE,NULL AS DIAVALUE"
        strSql += vbCrLf + " ,S.STNWT AS STNWT,S.STNAMT AS STNAMT"
        strSql += vbCrLf + " ,1 RESULT,2 ITEMORDER,''COLHEAD,P.ITEMID,P.ITEM"
        strSql += vbCrLf + " FROM  " & cnAdminDb & "..PURITEMTAGSTONE S"
        strSql += vbCrLf + " LEFT JOIN TEMPTABLEDB..TEMPTAGPURDET P ON P.TAGNO = S.TAGNO"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMTAG A ON A.SNO = S.TAGSNO"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID = S.STNITEMID AND IM.DIASTONE = 'S'"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.SUBITEMID = S.STNSUBITEMID "
        If rbtStockOnly.Checked Then strSql += vbCrLf + " AND A.RECDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "' AND (A.ISSDATE IS NULL OR A.ISSDATE > '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "')"
        If rbtReceipt.Checked Then strSql += vbCrLf + " AND A.RECDATE BETWEEN '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        If rbtIssue.Checked Then strSql += vbCrLf + " AND A.ISSDATE  BETWEEN '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        If chkCostName <> "" Then strSql += vbCrLf + " AND A.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        If chkItemCounter <> "" Then strSql += vbCrLf + " AND A.ITEMCTRID IN (SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME IN (" & chkItemCounter & "))"
        If chkItemName <> "" Then strSql += vbCrLf + " AND A.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & chkItemName & "))"
        If chkMetalName <> "" Then strSql += vbCrLf + " AND A.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
        If chkDesigner <> "" Then strSql += vbCrLf + " AND A.DESIGNERID IN (SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME IN (" & chkDesigner & "))"
        If txtTranInvNo.Text <> "" Then strSql += vbCrLf + " AND A.TRANINVNO = '" & txtTranInvNo.Text & "'"
        strSql += vbCrLf + " End"
        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        If ChkStudded.Checked Then
            strSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMPFINALREPORT)>0"
            strSql += vbCrLf + " BEGIN"
            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPFINALREPORT (PARTICULAR,MTAGNO,DIAWT,DIARATE,DIAVALUE,"
            strSql += vbCrLf + " RESULT,ITEMORDER,COLHEAD,ITEMID,ITEM)"
            strSql += vbCrLf + " SELECT CASE WHEN ISNULL(SI.SUBITEMNAME,'')<>'' THEN SI.SUBITEMNAME ELSE IM.ITEMNAME END AS PARTICULAR"
            strSql += vbCrLf + " ,S.TAGNO,S.STNWT AS DIAWT,S.PURRATE DIARATE,S.PURAMT AS DIAVALUE"
            strSql += vbCrLf + " ,1 RESULT,3 ITEMORDER,''COLHEAD,P.ITEMID,P.ITEM"
            strSql += vbCrLf + " FROM  " & cnAdminDb & "..PURITEMTAGSTONE S"
            strSql += vbCrLf + " LEFT JOIN TEMPTABLEDB..TEMPTAGPURDET P ON P.TAGNO = S.TAGNO"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMTAG A ON A.SNO = S.TAGSNO"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID = S.STNITEMID AND IM.DIASTONE = 'D'"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.SUBITEMID = S.STNSUBITEMID "
            If rbtStockOnly.Checked Then strSql += vbCrLf + " AND A.RECDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "' AND (A.ISSDATE IS NULL OR A.ISSDATE > '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "')"
            If rbtReceipt.Checked Then strSql += vbCrLf + " AND A.RECDATE BETWEEN '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            If rbtIssue.Checked Then strSql += vbCrLf + " AND A.ISSDATE  BETWEEN '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            If chkCostName <> "" Then strSql += vbCrLf + " AND A.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
            If chkItemCounter <> "" Then strSql += vbCrLf + " AND A.ITEMCTRID IN (SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME IN (" & chkItemCounter & "))"
            If chkItemName <> "" Then strSql += vbCrLf + " AND A.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & chkItemName & "))"
            If chkMetalName <> "" Then strSql += vbCrLf + " AND A.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
            If chkDesigner <> "" Then strSql += vbCrLf + " AND A.DESIGNERID IN (SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME IN (" & chkDesigner & "))"
            If txtTranInvNo.Text <> "" Then strSql += vbCrLf + " AND A.TRANINVNO = '" & txtTranInvNo.Text & "'"
            strSql += vbCrLf + " End"
            cmd = New OleDbCommand(strSql, cn)
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End If

        strSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMPFINALREPORT)>0"
        strSql += vbCrLf + " BEGIN"
        strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPFINALREPORT(ITEM,PARTICULAR,RESULT,ITEMORDER,COLHEAD)"
        strSql += vbCrLf + " SELECT DISTINCT ITEM,PARTICULAR,0 RESULT,0 ITEMORDER,CONVERT(VARCHAR(2),'T') COLHEAD FROM TEMPTABLEDB..TEMPFINALREPORT WHERE ITEMORDER NOT IN(2,3)"
        strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPFINALREPORT(ITEM,PARTICULAR,RESULT,ITEMORDER,COLHEAD,PCS,NETWT,NETAMT,"

        If DT.Rows.Count > 0 Then
            For int = 0 To DT.Rows.Count - 1
                If DT.Rows(int).Item("METALID").ToString = "G" Then
                    strSql += DT.Rows(int).Item("METALNAME") & ","
                End If
            Next

            For int = 0 To DT.Rows.Count - 1
                If DT.Rows(int).Item("METALID").ToString <> "G" Then
                    strSql += DT.Rows(int).Item("METALNAME") & ","
                End If
            Next
        End If

        strSql += vbCrLf + " DIAWT,DIAVALUE,STNWT,STNAMT,PURMC,GRANDTOTAL,PURTAX)"
        strSql += vbCrLf + " Select distinct ITEM,'SUB TOTAL',2 RESULT,3 ITEMORDER,'S' COLHEAD,SUM(PCS),SUM(NETWT),SUM(NETAMT),"


        If DT.Rows.Count > 0 Then
            For int = 0 To DT.Rows.Count - 1
                If DT.Rows(int).Item("METALID").ToString = "G" Then
                    strSql += " SUM(" & DT.Rows(int).Item("METALNAME") & "),"
                End If
            Next

            For int = 0 To DT.Rows.Count - 1
                If DT.Rows(int).Item("METALID").ToString <> "G" Then
                    strSql += " SUM(" & DT.Rows(int).Item("METALNAME") & "),"
                End If
            Next
        End If

        strSql += vbCrLf + " SUM(DIAWT),SUM(DIAVALUE),SUM(STNWT),SUM(STNAMT),SUM(PURMC),SUM(GRANDTOTAL),SUM(PURTAX)"
        strSql += vbCrLf + " from TEMPTABLEDB..TEMPFINALREPORT Group by ITEM"

        strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPFINALREPORT(ITEM,PARTICULAR,RESULT,ITEMORDER,COLHEAD,TOTAL,PCS,NETWT,NETAMT,"

        If DT.Rows.Count > 0 Then
            For int = 0 To DT.Rows.Count - 1
                If DT.Rows(int).Item("METALID").ToString = "G" Then
                    strSql += DT.Rows(int).Item("METALNAME") & ","
                End If
            Next

            For int = 0 To DT.Rows.Count - 1
                If DT.Rows(int).Item("METALID").ToString <> "G" Then
                    strSql += DT.Rows(int).Item("METALNAME") & ","
                End If
            Next
        End If


        strSql += vbCrLf + " DIAWT,DIAVALUE,STNWT,STNAMT,PURMC,GRANDTOTAL ,PURTAX)"
        strSql += vbCrLf + " Select DISTINCT 'ZZZZ','GRAND TOTAL',3 RESULT,4 ITEMORDER,convert(varchar(2),'G') COLHEAD,"
        strSql += vbCrLf + " SUM(TOTAL),SUM(PCS),SUM(NETWT),SUM(NETAMT),"

        If DT.Rows.Count > 0 Then
            For int = 0 To DT.Rows.Count - 1
                If DT.Rows(int).Item("METALID").ToString = "G" Then
                    strSql += " SUM(" & DT.Rows(int).Item("METALNAME") & "),"
                End If
            Next

            For int = 0 To DT.Rows.Count - 1
                If DT.Rows(int).Item("METALID").ToString <> "G" Then
                    strSql += " SUM(" & DT.Rows(int).Item("METALNAME") & "),"
                End If
            Next
        End If


        strSql += vbCrLf + " SUM(DIAWT),SUM(DIAVALUE),SUM(STNWT),SUM(STNAMT),SUM(PURMC),SUM(GRANDTOTAL),SUM(PURTAX)"
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMPFINALREPORT where Result=1"
        strSql += vbCrLf + " End"

        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " SELECT * FROM TEMPTABLEDB..TEMPFINALREPORT ORDER BY ITEM,RESULT,MTAGNO,ITEMORDER"
        dtItem.Columns.Add("KEYNO", GetType(Integer))
        dtItem.Columns("KEYNO").AutoIncrement = True
        dtItem.Columns("KEYNO").AutoIncrementSeed = 0
        dtItem.Columns("KEYNO").AutoIncrementStep = 1
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtItem)
        If Not dtItem.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        objGridShower.Size = New Size(778, 550)
        objGridShower.Text = "TAGED ITEMS DETAILED PURCHASE REPORT"
        Dim tit As String = "TAGED ITEMS DETAILED PURCHASE REPORT" + IIf(chkMetalName <> "", chkMetalName.Replace("'", ""), " ALL METAL") + vbCrLf
        tit += "AS ON DATE  " + dtpAsOnDate.Text
        Dim Cname As String = ""
        If chkLstCostCentre.Items.Count > 0 And chkLstCostCentre.CheckedItems.Count > 0 Then
            For CNT As Integer = 0 To chkLstCostCentre.Items.Count - 1
                If chkLstCostCentre.GetItemChecked(CNT) = True Then
                    Cname += "" & chkLstCostCentre.Items(CNT) + ","
                End If
            Next
            If Cname <> "" Then Cname = " :" + Mid(Cname, 1, Len(Cname) - 1)
        End If
        objGridShower.lblTitle.Text = tit & Cname
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(dtItem)
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        'objGridShower.gridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        AddHandler objGridShower.gridView.KeyPress, AddressOf GridView_KeyPress

        'If objGridShower.gridView("COLHEAD", 0).Value = "T" Then
        '    objGridShower.gridView.BackgroundColor = Color.Gold
        'End If
        'If objGridShower.gridView.Columns("COLHEAD") = "S" Then
        '    objGridShower.gridView.ForeColor = Color.CadetBlue
        'End If
        'If objGridShower.gridView.Columns("COLHEAD") = "G" Then
        '    objGridShower.gridView.BackgroundColor = Color.LightYellow
        'End If

        objGridShower.pnlFooter.Visible = False
        objGridShower.FormReLocation = False
        objGridShower.FormReSize = False
        objGridShower.Show()
        FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
        DataGridView_Detailed(objGridShower.gridView)
        objGridShower.FormReSize = True
        objGridShower.gridViewHeader.Visible = False
        objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle = objGridShower.gridView.ColumnHeadersDefaultCellStyle
        Prop_Sets()
    End Sub
    Private Sub DataGridView_Detailed(ByVal dgv As DataGridView)
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                dgvCol.Visible = True
            Next
            .Columns("PARTICULAR").Width = 200
            .Columns("TAGNO").Width = 75
            .Columns("STYLENO").Width = 60
            .Columns("TOTAL").Width = 100
            .Columns("PCS").Width = 70
            .Columns("NETWT").Width = 80
            .Columns("NETAMT").Width = 90
            '.Columns("DIAWT").Width = 80
            ' .Columns("DIAAMT").Width = 100
            .Columns("STNWT").Width = 80
            .Columns("STNAMT").Width = 100
            .Columns("PURMC").Width = 80
            .Columns("GRANDTOTAL").Width = 100
            .Columns("PURTAX").Width = 70
            .Columns("TRANINVNO").Width = 100
            .Columns("SUPBILLNO").Width = 100
            '.Columns("TAGIMAGE").Width = 100

            '    .Columns("DIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
            '    If chkWithImage.Checked And chkWithImage.Visible Then
            '        .Columns("TAGIMAGE").Visible = True
            '    Else
            '        .Columns("TAGIMAGE").Visible = False
            '    End If
            .Columns("DESIGNER").Visible = rbtDetailed.Checked
            .Columns("STYLENO").Visible = rbtDetailed.Checked
            .Columns("TAGNO").Visible = Not rbtSummary.Checked
            .Columns("STYLENO").Visible = Not rbtSummary.Checked
            .Columns("TRANINVNO").Visible = Not rbtSummary.Checked
            .Columns("SUPBILLNO").Visible = Not rbtSummary.Checked
            .Columns("ITEMORDER").Visible = False
            .Columns("MTAGNO").Visible = False
            If ChkStudded.Checked = False Then
                .Columns("DIAWT").Visible = False
                .Columns("DIARATE").Visible = False
                .Columns("DIAVALUE").Visible = False
            End If
            '.Columns("PCTFILE").Visible = False
            .Columns("RESULT").Visible = False
            .Columns("COLHEAD").Visible = False
            .Columns("ITEM").Visible = False
            .Columns("KEYNO").Visible = False
            '.Columns("TAGVAL").Visible = False
            .Columns("ITEMID").Visible = False
            FormatGridColumns(dgv, False, False, , False)
        End With
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub
    Private Sub chkLstItem_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLstItem.GotFocus
        Dim chkMetalName As String = GetChecked_CheckedList(chkLstMetal)
        strSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST "
        strSql += " WHERE ACTIVE = 'Y'"
        If chkMetalName <> "" Then strSql += " AND METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")) "
        strSql += " ORDER BY ITEMNAME"
        FillCheckedListBox(strSql, chkLstItem)
        SetChecked_CheckedList(chkLstItem, chkItemSelectAll.Checked)
    End Sub
    'Private Sub GetItemId()
    '    Dim chkItemName As String = GetChecked_CheckedList(chkLstItem)
    '    ItemList.Clear()
    '    strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & chkItemName & ")"
    '    Dim dt As New DataTable
    '    da = New OleDbDataAdapter(strSql, cn)
    '    da.Fill(dt)
    '    For Each ro As DataRow In dt.Rows
    '        ItemList.Add(ro(0).ToString)
    '    Next
    'End Sub
    'Private Sub GetMetalId()
    '    Dim chkMetalName As String = GetChecked_CheckedList(chkLstMetal)
    '    metalId.Clear()
    '    strSql = " SELECT MetalId FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ") order by DISPLAYORDER"
    '    Dim dt As New DataTable
    '    da = New OleDbDataAdapter(strSql, cn)
    '    da.Fill(dt)
    '    For Each ro As DataRow In dt.Rows
    '        metalId.Add(ro(0).ToString)
    '    Next
    'End Sub
    Private Sub LoadTagNo()
        Dim i As Integer
        Dim itemID As String = ItemCheck(ItemList)
        tagNo.Clear()
        strSql = " SELECT TAGNO FROM " & cnAdminDb & "..ITEMTAG WHERE ITEMID IN (" & itemID & ")"
        Dim dt As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        For Each ro As DataRow In dt.Rows
            tagNo.Add(ro(0).ToString)
        Next
    End Sub
    Public Function ItemCheck(ByVal list As ArrayList, Optional ByVal withSingleQt As Boolean = True) As String
        Dim retStr As String = ""
        For cnt As Integer = 0 To list.Count - 1
            If withSingleQt Then retStr += "'"
            retStr += list.Item(cnt).ToString
            If withSingleQt Then retStr += "'"
            retStr += ","
        Next
        If retStr <> "" Then retStr = Mid(retStr, 1, retStr.Length - 1)
        Return retStr
    End Function
    Private Sub rbtDetailed_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtDetailed.CheckedChanged
        chkWithImage.Visible = rbtDetailed.Checked
    End Sub

    Private Sub GridView_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs)
        If e.KeyChar = Chr(Keys.Enter) Then
            With objGridShower
                If Not .gridView.RowCount > 0 Then Exit Sub
                If .gridView.CurrentRow Is Nothing Then Exit Sub
                If .gridView.CurrentRow.Cells("ITEMID").Value.ToString = "" Then Exit Sub
                If .gridView.CurrentRow.Cells("TAGNO").Value.ToString = "" Then Exit Sub
                Dim objTagViewer As New frmTagImageViewer( _
                 .gridView.CurrentRow.Cells("TAGNO").Value.ToString, _
                 Val(.gridView.CurrentRow.Cells("ITEMID").Value.ToString), _
                 BrighttechPack.Methods.GetRights(_DtUserRights, frmTagCheck.Name, BrighttechPack.Methods.RightMode.Authorize, False))
                objTagViewer.ShowDialog()
            End With
        End If
    End Sub


    Private Sub Prop_Sets()
        Dim obj As New frmTagItemsPurchaseReport_Detail_Properties
        obj.p_txtTranInvNo = txtTranInvNo.Text
        obj.p_chkDesignerSelectAll = chkDesignerSelectAll.Checked
        GetChecked_CheckedList(chkLstDesigner, obj.p_chkLstDesigner)
        'obj.p_chkCostCentreSelectAll = chkCostCentreSelectAll.Checked
        'GetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre)
        obj.p_chkMetalSelectAll = chkMetalSelectAll.Checked
        GetChecked_CheckedList(chkLstMetal, obj.p_chkLstMetal)
        obj.p_chkItemSelectAll = chkItemSelectAll.Checked
        GetChecked_CheckedList(chkLstItem, obj.p_chkLstItem)
        obj.p_chkItemCounterSelectAll = chkItemCounterSelectAll.Checked
        GetChecked_CheckedList(chkLstItemCounter, obj.p_chkLstItemCounter)
        obj.p_rbtStockOnly = rbtStockOnly.Checked
        obj.p_rbtDetailed = rbtDetailed.Checked
        obj.p_rbtSummary = rbtSummary.Checked
        obj.p_chkWithImage = chkWithImage.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmTagItemsPurchaseReport_Detail_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmTagItemsPurchaseReport_Detail_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmTagItemsPurchaseReport_Detail_Properties))
        txtTranInvNo.Text = obj.p_txtTranInvNo
        chkDesignerSelectAll.Checked = obj.p_chkDesignerSelectAll
        SetChecked_CheckedList(chkLstDesigner, obj.p_chkLstDesigner, Nothing)
        'chkCostCentreSelectAll.Checked = obj.p_chkCostCentreSelectAll
        'SetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre, cnCostName)
        chkMetalSelectAll.Checked = obj.p_chkMetalSelectAll
        SetChecked_CheckedList(chkLstMetal, obj.p_chkLstMetal, Nothing)
        chkItemSelectAll.Checked = obj.p_chkItemSelectAll
        SetChecked_CheckedList(chkLstItem, obj.p_chkLstItem, Nothing)
        chkItemCounterSelectAll.Checked = obj.p_chkItemCounterSelectAll
        SetChecked_CheckedList(chkLstItemCounter, obj.p_chkLstItemCounter, Nothing)
        rbtStockOnly.Checked = obj.p_rbtStockOnly
        rbtIssue.Checked = obj.p_rbtissue
        rbtReceipt.Checked = obj.p_rbtReceipt
        rbtDetailed.Checked = obj.p_rbtDetailed
        rbtSummary.Checked = obj.p_rbtSummary
        chkWithImage.Checked = obj.p_chkWithImage
    End Sub

    Private Sub chkLstItem_ItemCheck(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs) Handles chkLstItem.ItemCheck
        'GetItemId()
    End Sub

    Private Sub chkLstItem_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkLstItem.SelectedIndexChanged
        ' GetItemId()
    End Sub

    Private Sub chkLstItem_SelectedValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkLstItem.SelectedValueChanged
        'GetItemId()
    End Sub

    Private Sub chkLstItem_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkLstItem.Leave
        ' GetItemId()
    End Sub

    Private Sub chkLstMetal_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkLstMetal.Leave
        'GetMetalId()
    End Sub

    Private Sub chkLstMetal_SelectedValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkLstMetal.SelectedValueChanged
        ' GetMetalId()
    End Sub

    Private Sub chkLstMetal_ItemCheck(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs) Handles chkLstMetal.ItemCheck
        'GetMetalId()
    End Sub

    Private Sub chkLstMetal_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkLstMetal.SelectedIndexChanged
        ' GetMetalId()
    End Sub

    Private Sub rbtStockOnly_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtStockOnly.CheckedChanged
        If rbtStockOnly.Checked = True Then
            PnlTodate.Visible = False
            lblAsOnDate.Text = "As On Date"
        End If
    End Sub

    Private Sub rbtReceipt_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtReceipt.CheckedChanged
        If rbtReceipt.Checked = True Then
            PnlTodate.Visible = True
            lblAsOnDate.Text = "From Date"
        End If
    End Sub

    Private Sub rbtIssue_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtIssue.CheckedChanged
        If rbtIssue.Checked = True Then
            PnlTodate.Visible = True
            lblAsOnDate.Text = "From Date"
        End If
    End Sub
End Class


Public Class frmTagItemsPurchaseReport_Detail_Properties
    Private txtTranInvNo As String = ""
    Public Property p_txtTranInvNo() As String
        Get
            Return txtTranInvNo
        End Get
        Set(ByVal value As String)
            txtTranInvNo = value
        End Set
    End Property
    Private chkDesignerSelectAll As Boolean = False
    Public Property p_chkDesignerSelectAll() As Boolean
        Get
            Return chkDesignerSelectAll
        End Get
        Set(ByVal value As Boolean)
            chkDesignerSelectAll = value
        End Set
    End Property
    Private chkLstDesigner As New List(Of String)
    Public Property p_chkLstDesigner() As List(Of String)
        Get
            Return chkLstDesigner
        End Get
        Set(ByVal value As List(Of String))
            chkLstDesigner = value
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
    Private chkLstCostCentre As New List(Of String)
    Public Property p_chkLstCostCentre() As List(Of String)
        Get
            Return chkLstCostCentre
        End Get
        Set(ByVal value As List(Of String))
            chkLstCostCentre = value
        End Set
    End Property
    Private chkMetalSelectAll As Boolean = False
    Public Property p_chkMetalSelectAll() As Boolean
        Get
            Return chkMetalSelectAll
        End Get
        Set(ByVal value As Boolean)
            chkMetalSelectAll = value
        End Set
    End Property
    Private chkLstMetal As New List(Of String)
    Public Property p_chkLstMetal() As List(Of String)
        Get
            Return chkLstMetal
        End Get
        Set(ByVal value As List(Of String))
            chkLstMetal = value
        End Set
    End Property

    Private chkItemSelectAll As Boolean = False
    Public Property p_chkItemSelectAll() As Boolean
        Get
            Return chkItemSelectAll
        End Get
        Set(ByVal value As Boolean)
            chkItemSelectAll = value
        End Set
    End Property
    Private chkLstItem As New List(Of String)
    Public Property p_chkLstItem() As List(Of String)
        Get
            Return chkLstItem
        End Get
        Set(ByVal value As List(Of String))
            chkLstItem = value
        End Set
    End Property
    Private chkItemCounterSelectAll As Boolean = False
    Public Property p_chkItemCounterSelectAll() As Boolean
        Get
            Return chkItemCounterSelectAll
        End Get
        Set(ByVal value As Boolean)
            chkItemCounterSelectAll = value
        End Set
    End Property
    Private chkLstItemCounter As New List(Of String)
    Public Property p_chkLstItemCounter() As List(Of String)
        Get
            Return chkLstItemCounter
        End Get
        Set(ByVal value As List(Of String))
            chkLstItemCounter = value
        End Set
    End Property
    Private rbtStockOnly As Boolean = False
    Public Property p_rbtStockOnly() As Boolean
        Get
            Return rbtStockOnly
        End Get
        Set(ByVal value As Boolean)
            rbtStockOnly = value
        End Set
    End Property
    Private rbtReceipt As Boolean = True
    Public Property p_rbtReceipt() As Boolean
        Get
            Return rbtReceipt
        End Get
        Set(ByVal value As Boolean)
            rbtReceipt = value
        End Set
    End Property
    Private rbtissue As Boolean = True
    Public Property p_rbtissue() As Boolean
        Get
            Return rbtissue
        End Get
        Set(ByVal value As Boolean)
            rbtissue = value
        End Set
    End Property
    Private rbtDetailed As Boolean = True
    Public Property p_rbtDetailed() As Boolean
        Get
            Return rbtDetailed
        End Get
        Set(ByVal value As Boolean)
            rbtDetailed = value
        End Set
    End Property
    Private rbtSummary As Boolean = False
    Public Property p_rbtSummary() As Boolean
        Get
            Return rbtSummary
        End Get
        Set(ByVal value As Boolean)
            rbtSummary = value
        End Set
    End Property
    Private chkWithImage As Boolean = False
    Public Property p_chkWithImage() As Boolean
        Get
            Return chkWithImage
        End Get
        Set(ByVal value As Boolean)
            chkWithImage = value
        End Set
    End Property
End Class

