Imports System.Data.OleDb
Imports System.Xml
Public Class frmSalesDifferenceReport
    Dim strSql As String = Nothing
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim tagCondStr As String = Nothing
    Dim itemCondStr As String = Nothing
    Dim emptyCondStr As String = Nothing
    Dim emptyCondStr_NONTAG As String = Nothing
    Dim dsResult As New DataSet("MainResult")
    Dim RW As Integer = Nothing
    Dim SelectedCompany As String

    Dim dtMetal As New DataTable
    Dim dtALLOY As New DataTable
    Dim dtCounter As New DataTable
    Dim dtItemType As New DataTable
    Dim dtCompany As New DataTable
    Dim dtCostCentre As New DataTable
    Dim dtItem As New DataTable
    Dim dtDesigner As New DataTable
    Dim HideSummary As Boolean = IIf(GetAdmindbSoftValue("HIDE-STOCKSUMMARY", "N") = "Y", True, False)


    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        'Me.WindowState = FormWindowState.Maximized
        tabMain.SelectedTab = tabGen
    End Sub

    Function funcExit() As Integer
        Me.Close()
    End Function

    'Function funcLoadItemName() As Integer
    '    strSql = " SELECT 'ALL' ITEMNAME,'ALL' ITEMID,1 RESULT"
    '    strSql += " UNION ALL"
    '    strSql += " SELECT ITEMNAME,CONVERT(vARCHAR,ITEMID),2 RESULT FROM " & cnAdminDb & "..ITEMMAST"
    '    strSql += " WHERE ACTIVE = 'Y'"
    '    If chkCmbMetal.Text <> "ALL" And chkCmbMetal.Text <> "" Then
    '        strSql += " AND METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & GetQryString(chkCmbMetal.Text) & "))"
    '    End If
    '    strSql += " ORDER BY RESULT,ITEMNAME"
    '    dtItem = New DataTable
    '    da = New OleDbDataAdapter(strSql, cn)
    '    da.Fill(dtItem)
    '    GiritechPack.GlobalMethods.FillCombo(chkCmbItem, dtItem, "ITEMNAME", , "ALL")
    'End Function

    'Function funcLoadMetal() As Integer
    '    cmbMetal.Items.Clear()
    '    cmbMetal.Items.Add("ALL")
    '    strSql = " select MetalName from " & cnAdminDb & "..metalMast order by MetalName"
    '    objGPack.FillCombo(strSql, cmbMetal, False, False)
    '    cmbMetal.Text = "ALL"
    'End Function

    Function funcGridViewStyle() As Integer
        With gridView
            .Columns("OPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("OGRSWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("ONETWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("IPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("IGRSWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("INETWT").DefaultCellStyle.BackColor = Color.AliceBlue

            .Columns("COLHEAD").Visible = False
            .Columns("SNO").Visible = False
            .Columns("itemid").Visible = False
            .Columns("subitemid").Visible = False
            With .Columns("itemName")
                .HeaderText = "ITEM"
                .Width = 200
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("subItemName")
                .Visible = False
                .HeaderText = "SUBITEM"
                .Width = 150
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("designerName")
                .HeaderText = "DesignerName"
                .Visible = False
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("CostName")
                .Visible = False
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("Counter")
                .Visible = False
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
        End With
    End Function

    'Function funcEmptyItemFiltration() As String
    '    Dim str As String = Nothing
    '    str = " "
    '    If chkAll.Checked = False Then
    '        str = " Having not (sum(isnull(t.Pcs,0)) = 0 and sum(isnull(t.GrsWt,0)) = 0 and sum(isnull(t.NetWt,0))=0)"
    '    End If
    '    Return str
    'End Function

    Private Sub GridStyle()
        ''
        ' FillGridGroupStyle_KeyNoWise(gridView)

        'For Each dgvRow As DataGridViewRow In gridView.Rows
        '    If dgvRow.Cells("COLHEAD").Value.ToString = "T" Then
        '        dgvRow.Cells("PARTICULAR").Style.BackColor = Color.LightBlue
        '        dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        '    ElseIf dgvRow.Cells("COLHEAD").Value.ToString = "S" Then
        '        dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        '    End If
        'Next
        FormatGridColumns(gridView, False, , , False)
        gridView.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
        gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        gridView.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        With gridView
            .Columns("BILLNO").Width = 75
            .Columns("BILLDATE").Width = 75
            .Columns("PRODUCTNAME").Width = 200
            .Columns("TAGNO").Width = 100
            .Columns("TAGVALUE").Width = 100
            .Columns("SALVALUE").Width = 100
            .Columns("DIFFERENCE").Width = 100
            .Columns("DIFFERENCEP").Width = 100
            ''HEADER TEXT

            .Columns("BILLNO").HeaderText = "BILL NO"
            .Columns("BILLDATE").HeaderText = "BILL DATE"
            .Columns("PRODUCTNAME").HeaderText = "PRODUCT NAME"
            .Columns("TAGNO").HeaderText = "TAG NO"
            .Columns("TAGVALUE").HeaderText = "TAG VALUE"
            .Columns("SALVALUE").HeaderText = "SALE VALUE"
            .Columns("DIFFERENCE").HeaderText = "DIFFERENCE"
            .Columns("DIFFERENCEP").HeaderText = "DIFF %"

            'FillGridGroupStyle_KeyNoWise(gridView, "METAL")
            .Columns("TAGVALUE").DefaultCellStyle.Format = "0.00"
            .Columns("TAGVALUE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("SALVALUE").DefaultCellStyle.Format = "0.00"
            .Columns("SALVALUE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("DIFFERENCE").DefaultCellStyle.Format = "0.00"
            .Columns("DIFFERENCE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("DIFFERENCEP").DefaultCellStyle.Format = "0.00"
            .Columns("DIFFERENCEP").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            ''VISIBLE
            'gridView.Columns("WEIGHT").Visible = chkwithweightdetails.Checked
            gridView.Columns("KEYNO").Visible = False
            gridView.Columns("COLHEAD").Visible = False
            gridView.Columns("RESULT").Visible = False
        End With
    End Sub

    'Private Sub Report()
    '    Dim RecDate As String = Nothing
    '    gridViewHead.DataSource = Nothing
    '    gridView.DataSource = Nothing
    '    If chkAsOnDate.Checked Then
    '        dtpTo.Value = dtpFrom.Value
    '    End If
    '    RecDate = "RECDATE"
    '    strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ITEMSTOCK1')>0 DROP TABLE TEMP" & systemId & "ITEMSTOCK1"
    '    strSql += vbcrlf + " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ITEMSTOCK')>0 DROP TABLE TEMP" & systemId & "ITEMSTOCK"
    '    strSql += vbcrlf + " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & SYSTEMID & "ITEMSTK')>0 DROP TABLE TEMP" & SYSTEMID & "ITEMSTK"
    '    strSql += vbcrlf + " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ITEMNONTAGSTOCK')>0 DROP TABLE TEMP" & systemId & "ITEMNONTAGSTOCK"
    '    strSql += vbcrlf + " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMPCTRANSFER1')>0 DROP TABLE TEMPCTRANSFER1"
    '    strSql += vbcrlf + " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMPCTRANSFER')>0 DROP TABLE TEMPCTRANSFER"
    '    cmd = New OleDbCommand(strSql, cn)
    '    cmd.CommandTimeout = 100

    '    cmd.ExecuteNonQuery()
    '    If rbtTag.Checked Or rbtBoth.Checked Then
    '        strSql = " DECLARE @ASONDATE SMALLDATETIME"
    '        strSql += " DECLARE @TODATE SMALLDATETIME"
    '        strSql += vbCrLf + " SELECT @ASONDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
    '        strSql += vbCrLf + " SELECT @TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
    '        strSql += vbCrLf + " /* GETTING OPENING,ISSUE,RECEIPT FROM ITEMTAG */"
    '        strSql += vbCrLf + " SELECT 'OPE'SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT,SALVALUE AS VALUE,CONVERT(VARCHAR(15),SNO)SNO,STYLENO,CASE WHEN RATE <> 0 THEN RATE ELSE BOARDRATE END AS RATE"
    '        If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
    '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnStockDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
    '        Else
    '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
    '        End If
    '        If chkStone.Checked And rbtDiaStnByColumn.Checked Then
    '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnStockDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
    '        Else
    '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
    '        End If
    '        strSql += vbCrLf + " INTO TEMP" & systemId & "ITEMSTOCK1"
    '        strSql += vbCrLf + " FROM " & cnStockDb & "..ITEMTAG AS T"
    '        strSql += vbCrLf + " WHERE " & RecDate & " < @ASONDATE"
    '        strSql += vbCrLf + " AND (ISSDATE >= @ASONDATE OR ISSDATE IS NULL)"
    '        strSql += itemCondStr
    '        strSql += tagCondStr
    '        strSql += funcApproval()
    '        strSql += vbCrLf + " UNION ALL"
    '        strSql += vbCrLf + " SELECT 'ISS'SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT,SALVALUE AS VALUE,SNO,STYLENO,CASE WHEN RATE <> 0 THEN RATE ELSE BOARDRATE END AS RATE "
    '        If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
    '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnStockDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
    '        Else
    '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
    '        End If
    '        If chkStone.Checked And rbtDiaStnByColumn.Checked Then
    '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnStockDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
    '        Else
    '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
    '        End If
    '        strSql += vbCrLf + " FROM " & cnStockDb & "..ITEMTAG T"
    '        strSql += vbCrLf + " WHERE ISSDATE BETWEEN @ASONDATE AND @TODATE"
    '        strSql += itemCondStr
    '        strSql += tagCondStr
    '        strSql += funcApproval()
    '        strSql += vbCrLf + " UNION ALL"
    '        strSql += vbCrLf + " SELECT 'REC'SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT,SALVALUE AS VALUE,SNO,STYLENO,CASE WHEN RATE <> 0 THEN RATE ELSE BOARDRATE END AS RATE "
    '        If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
    '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnStockDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
    '        Else
    '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
    '        End If
    '        If chkStone.Checked And rbtDiaStnByColumn.Checked Then
    '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnStockDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
    '        Else
    '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
    '        End If
    '        strSql += vbCrLf + " FROM " & cnStockDb & "..ITEMTAG T "
    '        strSql += vbCrLf + " WHERE " & RecDate & " BETWEEN @ASONDATE AND @TODATE"
    '        strSql += itemCondStr
    '        strSql += tagCondStr
    '        strSql += funcApproval()
    '        If chkWithCumulative.Checked Then
    '            strSql += vbCrLf + " UNION ALL"
    '            strSql += vbCrLf + " SELECT 'OPE'SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT,SALVALUE AS VALUE,CONVERT(VARCHAR(15),SNO)SNO,STYLENO,CASE WHEN RATE <> 0 THEN RATE ELSE BOARDRATE END AS RATE"
    '            If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
    '                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnStockDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
    '            Else
    '                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
    '            End If
    '            If chkStone.Checked And rbtDiaStnByColumn.Checked Then
    '                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnStockDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
    '            Else
    '                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
    '            End If
    '            strSql += vbCrLf + " FROM " & cnStockDb & "..CITEMTAG AS T"
    '            strSql += vbCrLf + " WHERE " & RecDate & " < @ASONDATE"
    '            strSql += vbCrLf + " AND (ISSDATE >= @ASONDATE)"
    '            strSql += itemCondStr
    '            strSql += tagCondStr
    '            strSql += funcApproval()
    '            strSql += vbCrLf + " UNION ALL"
    '            strSql += vbCrLf + " SELECT 'ISS'SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT,SALVALUE AS VALUE,SNO,STYLENO,CASE WHEN RATE <> 0 THEN RATE ELSE BOARDRATE END AS RATE"
    '            If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
    '                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnStockDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
    '            Else
    '                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
    '            End If
    '            If chkStone.Checked And rbtDiaStnByColumn.Checked Then
    '                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnStockDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
    '            Else
    '                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
    '            End If
    '            strSql += vbCrLf + " FROM " & cnStockDb & "..CITEMTAG T"
    '            strSql += vbCrLf + " WHERE ISSDATE BETWEEN @ASONDATE AND @TODATE"
    '            strSql += itemCondStr
    '            strSql += tagCondStr
    '            strSql += funcApproval()
    '            strSql += vbCrLf + " UNION ALL"
    '            strSql += vbCrLf + " SELECT 'REC'SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT,SALVALUE AS VALUE,SNO,STYLENO,CASE WHEN RATE <> 0 THEN RATE ELSE BOARDRATE END AS RATE"
    '            If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
    '                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnStockDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
    '            Else
    '                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
    '            End If
    '            If chkStone.Checked And rbtDiaStnByColumn.Checked Then
    '                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnStockDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
    '            Else
    '                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
    '            End If
    '            strSql += vbCrLf + " FROM " & cnStockDb & "..CITEMTAG T "
    '            strSql += vbCrLf + " WHERE " & RecDate & " BETWEEN @ASONDATE AND @TODATE"
    '            strSql += itemCondStr
    '            strSql += tagCondStr
    '            strSql += funcApproval()
    '        End If
    '        If chkSeperateColumnApproval.Checked Then
    '            strSql += vbCrLf + " UNION ALL"
    '            strSql += vbCrLf + " SELECT 'APPISS'SEP,T.COSTID,T.DESIGNERID,T.ITEMCTRID,T.ITEMID,T.SUBITEMID,T.PCS,T.GRSWT,T.NETWT,T.SALVALUE AS VALUE"
    '            strSql += vbCrLf + " ,T.SNO,T.STYLENO,CASE WHEN T.RATE <> 0 THEN T.RATE ELSE T.BOARDRATE END AS RATE"
    '            If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
    '                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnStockDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
    '            Else
    '                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
    '            End If
    '            If chkStone.Checked And rbtDiaStnByColumn.Checked Then
    '                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnStockDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
    '            Else
    '                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
    '            End If
    '            strSql += vbCrLf + " FROM " & cnStockDb & "..ITEMTAG T "
    '            strSql += vbCrLf + " INNER JOIN " & cnStockDb & "..APPISSUE AS I ON I.TRANDATE BETWEEN @ASONDATE AND @TODATE AND ISNULL(I.CANCEL,'') = '' "
    '            strSql += vbCrLf + " AND I.ITEMID = T.ITEMID AND I.TAGNO = T.TAGNO"
    '            strSql += vbCrLf + " AND I.TRANTYPE = 'AI'"
    '            strSql += vbCrLf + " WHERE 1=1"
    '            strSql += itemCondStr
    '            strSql += tagCondStr
    '            strSql += vbCrLf + " UNION ALL"
    '            strSql += vbCrLf + " SELECT 'APPISS'SEP,T.COSTID,T.DESIGNERID,T.ITEMCTRID,T.ITEMID,T.SUBITEMID,T.PCS,T.GRSWT,T.NETWT,T.SALVALUE AS VALUE"
    '            strSql += vbCrLf + " ,T.SNO,T.STYLENO,CASE WHEN T.RATE <> 0 THEN T.RATE ELSE T.BOARDRATE END AS RATE"
    '            If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
    '                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnStockDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
    '            Else
    '                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
    '            End If
    '            If chkStone.Checked And rbtDiaStnByColumn.Checked Then
    '                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnStockDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
    '            Else
    '                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
    '            End If
    '            strSql += vbCrLf + " FROM " & cnStockDb & "..ITEMTAG T "
    '            strSql += vbCrLf + " INNER JOIN " & cnStockDb & "..ISSUE AS I ON I.TRANDATE BETWEEN @ASONDATE AND @TODATE AND ISNULL(I.CANCEL,'') = '' "
    '            strSql += vbCrLf + " AND I.ITEMID = T.ITEMID AND I.TAGNO = T.TAGNO"
    '            strSql += vbCrLf + " AND I.TRANTYPE = 'AI'"
    '            strSql += vbCrLf + " WHERE 1=1"
    '            strSql += itemCondStr
    '            strSql += tagCondStr
    '            strSql += vbCrLf + " UNION ALL"
    '            strSql += vbCrLf + " SELECT 'APPREC'SEP,T.COSTID,T.DESIGNERID,T.ITEMCTRID,T.ITEMID,T.SUBITEMID,T.PCS,T.GRSWT,T.NETWT,T.SALVALUE AS VALUE"
    '            strSql += vbCrLf + " ,T.SNO,T.STYLENO,CASE WHEN T.RATE <> 0 THEN T.RATE ELSE T.BOARDRATE END AS RATE"
    '            If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
    '                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnStockDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
    '            Else
    '                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
    '            End If
    '            If chkStone.Checked And rbtDiaStnByColumn.Checked Then
    '                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnStockDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
    '            Else
    '                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
    '            End If
    '            strSql += vbCrLf + " FROM " & cnStockDb & "..ITEMTAG T "
    '            strSql += vbCrLf + " INNER JOIN " & cnStockDb & "..RECEIPT AS I ON I.TRANDATE BETWEEN @ASONDATE AND @TODATE AND ISNULL(I.CANCEL,'') = '' "
    '            strSql += vbCrLf + " AND I.ITEMID = T.ITEMID AND I.TAGNO = T.TAGNO"
    '            strSql += vbCrLf + " AND I.TRANTYPE = 'AR'"
    '            strSql += vbCrLf + " WHERE 1=1"
    '            strSql += itemCondStr
    '            strSql += tagCondStr
    '        End If
    '        cmd = New OleDbCommand(strSql, cn)
    '        cmd.CommandTimeout = 100
    '        cmd.ExecuteNonQuery()
    '    End If
    '    If rbtNonTag.Checked Or rbtBoth.Checked Then
    '        strSql = " DECLARE @ASONDATE SMALLDATETIME"
    '        strSql += " DECLARE @TODATE SMALLDATETIME"
    '        strSql += vbCrLf + " SELECT @ASONDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
    '        strSql += vbCrLf + " SELECT @TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
    '        strSql += vbCrLf + "   /* GETTING OPENING,ISSUE,RECEIPT FROM ITEMNONTAG */"
    '        strSql += vbCrLf + "  SELECT 'OPE'SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMID TITEMID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT,CONVERT(NUMERIC(15,2),NULL)VALUE,SNO"
    '        strSql += " ,RECISS"
    '        strSql += " ,0 STONE,CONVERT(VARCHAR(20),PACKETNO)STYLENO,RATE"
    '        If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
    '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnStockDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
    '        Else
    '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
    '        End If
    '        If chkStone.Checked And rbtDiaStnByColumn.Checked Then
    '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnStockDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
    '        Else
    '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
    '        End If
    '        strSql += vbCrLf + "  INTO TEMP" & systemId & "ITEMNONTAGSTOCK"
    '        strSql += vbCrLf + "  FROM " & cnStockDb & "..ITEMNONTAG T WHERE RECDATE < @ASONDATE"
    '        strSql += itemCondStr
    '        strSql += tagCondStr
    '        strSql += funcApproval()
    '        If _HideBackOffice Then strSql += vbCrLf + " AND ISNULL(T.FLAG,'') <> 'B'"
    '        strSql += vbCrLf + "  AND ISNULL(CANCEL,'') = ''"
    '        strSql += vbCrLf + "  UNION ALL"
    '        If chkSeperateColumnApproval.Checked Then
    '            strSql += vbCrLf + "  SELECT CASE WHEN ISNULL(APPROVAL,'') = 'A' THEN 'APPISS' ELSE 'ISS' END AS SEP"
    '        Else
    '            strSql += vbCrLf + "  SELECT 'ISS'SEP"
    '        End If
    '        strSql += vbCrLf + " ,COSTID,DESIGNERID,ITEMCTRID,ITEMID TITEMID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT,CONVERT(NUMERIC(15,2),NULL)VALUE,SNO,'R'RECISS,0 STONE,CONVERT(VARCHAR(20),PACKETNO)STYLENO,RATE"
    '        If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
    '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnStockDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
    '        Else
    '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
    '        End If
    '        If chkStone.Checked And rbtDiaStnByColumn.Checked Then
    '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnStockDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
    '        Else
    '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
    '        End If
    '        strSql += vbCrLf + "  FROM " & cnStockDb & "..ITEMNONTAG T WHERE RECDATE BETWEEN @ASONDATE AND @TODATE AND RECISS = 'I'"
    '        strSql += itemCondStr
    '        strSql += tagCondStr
    '        If chkWithApproval.Checked Then
    '            strSql += " AND ISNULL(T.APPROVAL,'') <> 'A'"
    '        ElseIf chkOnlyApproval.Checked Then
    '            strSql += " AND ISNULL(T.APPROVAL,'') = 'A'"
    '        End If

    '        If _HideBackOffice Then strSql += vbCrLf + " AND ISNULL(T.FLAG,'') <> 'B'"
    '        strSql += vbCrLf + "  AND ISNULL(CANCEL,'') = ''"
    '        strSql += vbCrLf + "  UNION ALL"
    '        If chkSeperateColumnApproval.Checked Then
    '            strSql += vbCrLf + "  SELECT CASE WHEN ISNULL(APPROVAL,'') = 'A' THEN 'APPREC' ELSE 'REC' END AS SEP"
    '        Else
    '            strSql += vbCrLf + "  SELECT 'REC'SEP"
    '        End If
    '        strSql += vbCrLf + " ,COSTID,DESIGNERID,ITEMCTRID,ITEMID TITEMID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT,CONVERT(NUMERIC(15,2),NULL)VALUE,SNO,RECISS,0 STONE,CONVERT(VARCHAR(20),PACKETNO)STYLENO,RATE"
    '        If chkDiamond.Checked And rbtDiaStnByColumn.Checked Then
    '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnStockDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
    '        Else
    '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
    '        End If
    '        If chkStone.Checked And rbtDiaStnByColumn.Checked Then
    '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),(SELECT SUM(STNWT) FROM " & cnStockDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
    '        Else
    '            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),NULL)STNWT"
    '        End If
    '        strSql += vbCrLf + "  FROM " & cnStockDb & "..ITEMNONTAG T WHERE RECDATE BETWEEN @ASONDATE AND @TODATE AND RECISS = 'R'"
    '        strSql += itemCondStr
    '        strSql += tagCondStr
    '        If chkWithApproval.Checked Then
    '            strSql += " AND ISNULL(T.APPROVAL,'') <> 'A'"
    '        ElseIf chkOnlyApproval.Checked Then
    '            strSql += " AND ISNULL(T.APPROVAL,'') = 'A'"
    '        End If
    '        If _HideBackOffice Then strSql += vbCrLf + " AND ISNULL(T.FLAG,'') <> 'B'"
    '        strSql += vbCrLf + "  AND ISNULL(CANCEL,'') = ''"
    '        If (chkDiamond.Checked Or chkStone.Checked) And rbtDiaStnByRow.Checked Then
    '            Dim diaStone As String = ""
    '            If chkDiamond.Checked Then diaStone += "'D',"
    '            If chkStone.Checked Then diaStone += "'S',"
    '            diaStone = Mid(diaStone, 1, diaStone.Length - 1)
    '            strSql += vbCrLf + "  INSERT INTO TEMP" & systemId & "ITEMNONTAGSTOCK"
    '            strSql += vbCrLf + "  SELECT S.SEP,S.COSTID,S.DESIGNERID,S.ITEMCTRID,S.ITEMID TITEMID,T.STNITEMID ITEMID,T.STNSUBITEMID SUBITEMID"
    '            strSql += vbCrLf + "  ,STNPCS PCS,T.STNWT GRSWT,T.STNWT NETWT,T.STNAMT VALUE,TAGSNO,S.RECISS,1 STONE,S.STYLENO,S.RATE,NULL DIAWT,NULL STNWT"
    '            strSql += vbCrLf + "  FROM " & cnStockDb & "..ITEMNONTAGSTONE AS T INNER JOIN TEMP" & systemId & "ITEMNONTAGSTOCK AS S ON T.TAGSNO = S.SNO"
    '            strSql += vbCrLf + "  WHERE T.STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE IN (" & diaStone & "))"
    '        End If
    '        cmd = New OleDbCommand(strSql, cn)
    '        cmd.CommandTimeout = 100
    '        cmd.ExecuteNonQuery()
    '    End If

    '    If chkOnlyTag.Checked Then
    '        strSql = " SELECT SEP,COSTID,DESIGNERID,ITEMCTRID"
    '        strSql += vbCrLf + " ,ITEMID"
    '        strSql += vbCrLf + " ,SUBITEMID"
    '        strSql += vbCrLf + " ,COUNT(PCS)PCS,0 GRSWT,0 NETWT,CONVERT(NUMERIC(15,2),NULL) VALUE,NULL SNO,NULL DIAWT,NULL STNWT,NULL STYLENO,CONVERT(NUMERIC(15,2),NULL)RATE"
    '        strSql += vbCrLf + "  INTO TEMP" & systemId & "ITEMSTOCK"
    '        strSql += vbCrLf + "  FROM TEMP" & systemId & "ITEMSTOCK1"
    '        strSql += vbCrLf + " GROUP BY  SEP,ITEMID,SUBITEMID"
    '        strSql += vbCrLf + " ,ITEMCTRID"
    '        strSql += vbCrLf + " ,DESIGNERID"
    '        strSql += vbCrLf + " ,COSTID"
    '        cmd = New OleDbCommand(strSql, cn)
    '        cmd.CommandTimeout = 100
    '        cmd.ExecuteNonQuery()
    '    Else
    '        If rbtTag.Checked Or rbtBoth.Checked Then
    '            strSql = " SELECT *"
    '            strSql += vbCrLf + "  INTO TEMP" & systemId & "ITEMSTOCK"
    '            strSql += vbCrLf + "  FROM TEMP" & systemId & "ITEMSTOCK1"
    '            cmd = New OleDbCommand(strSql, cn)
    '            cmd.CommandTimeout = 100
    '            cmd.ExecuteNonQuery()
    '        Else 'nontag
    '            strSql = " SELECT *"
    '            strSql += vbCrLf + "  INTO TEMP" & systemId & "ITEMSTOCK"
    '            strSql += vbCrLf + "  FROM TEMP" & systemId & "ITEMNONTAGSTOCK"
    '            cmd = New OleDbCommand(strSql, cn)
    '            cmd.CommandTimeout = 100
    '            cmd.ExecuteNonQuery()
    '        End If
    '    End If

    '    strSql = " IF OBJECT_ID('MASTER..TEMP_ITEMSTKVIEW') IS NOT NULL DROP TABLE MASTER..TEMP_ITEMSTKVIEW"
    '    strSql += " SELECT "
    '    If chkOrderbyItemId.Checked Then
    '        strSql += vbCrLf + "     (SELECT '['+REPLICATE(' ',5-LEN(X.ITEMID)) + CONVERT(VARCHAR,X.ITEMID) + '] ' + ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = X.ITEMID)AS ITEM"
    '    Else
    '        strSql += vbCrLf + " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = X.ITEMID)AS ITEM"
    '    End If
    '    If chkOrderbyItemId.Checked Then
    '        strSql += vbCrLf + "     ,(SELECT '['+REPLICATE(' ',5-LEN(X.TITEMID)) + CONVERT(VARCHAR,X.TITEMID) + '] ' + ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = X.TITEMID)AS TITEM"
    '    Else
    '        strSql += vbCrLf + " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = X.TITEMID)AS TITEM"
    '    End If
    '    If chkOrderbyItemId.Checked Then
    '        strSql += vbCrLf + "     ,(SELECT '['+REPLICATE(' ',5-LEN(X.SUBITEMID)) + CONVERT(VARCHAR,X.SUBITEMID) + ']' + SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = X.SUBITEMID)AS SUBITEM"
    '    Else
    '        strSql += vbCrLf + " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = X.SUBITEMID)AS SUBITEM"
    '    End If
    '    strSql += " ,* INTO TEMP_ITEMSTKVIEW FROM"
    '    strSql += " ("
    '    If rbtTag.Checked Or rbtBoth.Checked Then
    '        strSql += vbCrLf + " SELECT SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMID TITEMID,ITEMID,SUBITEMID,PCS,GRSWT,NETWT,VALUE,0 STONE,DIAWT,STNWT,STYLENO,RATE FROM TEMP" & systemId & "ITEMSTOCK"
    '        If (chkDiamond.Checked Or chkStone.Checked) And rbtDiaStnByRow.Checked Then
    '            Dim diaStone As String = ""
    '            If chkDiamond.Checked Then diaStone += "'D',"
    '            If chkStone.Checked Then diaStone += "'S',"
    '            diaStone = Mid(diaStone, 1, diaStone.Length - 1)
    '            strSql += vbCrLf + " UNION ALL"
    '            strSql += vbCrLf + " SELECT S.SEP,S.COSTID,S.DESIGNERID,S.ITEMCTRID,S.ITEMID TITEMID,T.STNITEMID ITEMID,T.STNSUBITEMID SUBITEMID"
    '            strSql += vbCrLf + " ,T.STNPCS PCS,T.STNWT GRSWT,T.STNWT NETWT,S.VALUE,1 STONE,NULL DIAWT,NULL STNWT,S.STYLENO,S.RATE"
    '            strSql += vbCrLf + " FROM " & cnStockDb & "..ITEMTAGSTONE AS T "
    '            strSql += vbCrLf + " INNER JOIN TEMP" & systemId & "ITEMSTOCK S ON T.TAGSNO = CONVERT(VARCHAR,S.SNO)"
    '            strSql += vbCrLf + " WHERE T.STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE IN (" & diaStone & "))"
    '            'strSql += vbCrLf + " UNION ALL"
    '            'strSql += vbCrLf + " SELECT S.SEP,S.COSTID,S.DESIGNERID,S.ITEMCTRID,S.ITEMID TITEMID,T.STNITEMID ITEMID,T.STNSUBITEMID SUBITEMID"
    '            'strSql += vbCrLf + " ,T.STNPCS PCS,T.STNWT GRSWT,T.STNWT NETWT,S.VALUE,1 STONE,NULL DIAWT,NULL STNWT,S.STYLENO,S.RATE"
    '            'strSql += vbCrLf + " FROM " & cnStockDb & "..CITEMTAGSTONE AS T "
    '            'strSql += vbCrLf + " INNER JOIN TEMP" & systemId & "ITEMSTOCK S ON T.TAGSNO = CONVERT(VARCHAR,S.SNO)"
    '            'strSql += vbCrLf + " WHERE T.STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE IN (" & diaStone & "))"
    '        End If
    '        If rbtBoth.Checked Then strSql += vbCrLf + " UNION ALL"
    '    End If
    '    If rbtNonTag.Checked Or rbtBoth.Checked Then
    '        strSql += vbCrLf + "  SELECT "
    '        strSql += vbCrLf + "  SEP,COSTID,DESIGNERID,ITEMCTRID,ITEMID TITEMID,ITEMID,SUBITEMID"
    '        strSql += vbCrLf + "  ,SUM(CASE WHEN RECISS = 'R' THEN PCS ELSE -1*PCS END) AS PCS"
    '        strSql += vbCrLf + "  ,SUM(CASE WHEN RECISS = 'R' THEN GRSWT ELSE -1*GRSWT END) AS GRSWT"
    '        strSql += vbCrLf + "  ,SUM(CASE WHEN RECISS = 'R' THEN NETWT ELSE -1*NETWT END) AS NETWT,VALUE,STONE"
    '        strSql += vbCrLf + "  ,SUM(CASE WHEN RECISS = 'R' THEN DIAWT ELSE -1*DIAWT END) AS DIAWT"
    '        strSql += vbCrLf + "  ,SUM(CASE WHEN RECISS = 'R' THEN STNWT ELSE -1*STNWT END) AS STNWT,STYLENO,RATE"
    '        strSql += vbCrLf + "  FROM TEMP" & systemId & "ITEMNONTAGSTOCK AS X"
    '        strSql += vbCrLf + "  GROUP BY SEP,ITEMID,TITEMID,STONE,ITEMCTRID,DESIGNERID,COSTID,SUBITEMID,STYLENO,RATE,VALUE"
    '    End If
    '    strSql += " )X"
    '    cmd = New OleDbCommand(strSql, cn)
    '    cmd.CommandTimeout = 100
    '    cmd.ExecuteNonQuery()


    '    strSql = ""
    '    If Not chkWithRate.Checked Then strSql += vbCrLf + " UPDATE MASTER..TEMP_ITEMSTKVIEW SET RATE = NULL"
    '    If Not chkWithValue.Checked Then strSql += vbCrLf + " UPDATE MASTER..TEMP_ITEMSTKVIEW SET VALUE = NULL"
    '    If Not ChkLstGroupBy.CheckedItems.Contains("COSTCENTRE") Then strSql += vbCrLf + " UPDATE MASTER..TEMP_ITEMSTKVIEW SET COSTID = NULL"
    '    If Not ChkLstGroupBy.CheckedItems.Contains("DESIGNER") Then strSql += vbCrLf + " UPDATE MASTER..TEMP_ITEMSTKVIEW SET DESIGNERID = NULL"
    '    If Not ChkLstGroupBy.CheckedItems.Contains("COUNTER") Then strSql += vbCrLf + " UPDATE MASTER..TEMP_ITEMSTKVIEW SET ITEMCTRID = NULL"
    '    If Not chkStyleNo.Checked Then strSql += vbCrLf + " UPDATE MASTER..TEMP_ITEMSTKVIEW SET STYLENO = NULL"
    '    'If Not ChkLstGroupBy.CheckedItems.Contains("ITEM") Then strSql += vbCrLf + " UPDATE MASTER..TEMP_ITEMSTKVIEW SET ITEM = NULL"
    '    If Not chkWithSubItem.Checked Then strSql += vbCrLf + " UPDATE MASTER..TEMP_ITEMSTKVIEW SET SUBITEM = NULL"
    '    If strSql <> "" Then
    '        cmd = New OleDbCommand(strSql, cn)
    '        cmd.CommandTimeout = 100
    '        cmd.ExecuteNonQuery()
    '    End If


    '    strSql = " DECLARE @ASONDATE SMALLDATETIME"
    '    strSql += " DECLARE @TODATE SMALLDATETIME"
    '    strSql += vbCrLf + " SELECT @ASONDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
    '    strSql += vbCrLf + " SELECT @TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
    '    strSql += vbCrLf + " SELECT ITEM,SUBITEM,TITEM"
    '    strSql += vbCrLf + " ,(SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = X.ITEMCTRID)AS COUNTER"
    '    strSql += vbCrLf + " ,(SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = X.DESIGNERID)AS DESIGNER"
    '    strSql += vbCrLf + " ,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = X.COSTID)AS COSTCENTRE"
    '    strSql += vbCrLf + " ,STYLENO"
    '    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN PCS ELSE 0 END) AS OPCS"
    '    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN GRSWT ELSE 0 END) AS OGRSWT"
    '    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN NETWT ELSE 0 END) AS ONETWT"
    '    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN DIAWT ELSE 0 END) AS ODIAWT"
    '    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN STNWT ELSE 0 END) AS OSTNWT"
    '    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN VALUE ELSE 0 END) AS OVALUE"
    '    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN PCS ELSE 0 END) AS RPCS"
    '    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN GRSWT ELSE 0 END) AS RGRSWT"
    '    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN NETWT ELSE 0 END) AS RNETWT"
    '    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN DIAWT ELSE 0 END) AS RDIAWT"
    '    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN STNWT ELSE 0 END) AS RSTNWT"
    '    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN VALUE ELSE 0 END) AS RVALUE"
    '    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN PCS ELSE 0 END) AS IPCS"
    '    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN GRSWT ELSE 0 END) AS IGRSWT"
    '    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN NETWT ELSE 0 END) AS INETWT"
    '    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN DIAWT ELSE 0 END) AS IDIAWT"
    '    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN STNWT ELSE 0 END) AS ISTNWT"
    '    strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN VALUE ELSE 0 END) AS IVALUE"
    '    If chkSeperateColumnApproval.Checked Then
    '        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN PCS ELSE 0 END) AS ARPCS"
    '        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN GRSWT ELSE 0 END) AS ARGRSWT"
    '        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN NETWT ELSE 0 END) AS ARNETWT"
    '        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN DIAWT ELSE 0 END) AS ARDIAWT"
    '        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN STNWT ELSE 0 END) AS ARSTNWT"
    '        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPREC' THEN VALUE ELSE 0 END) AS ARVALUE"
    '        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN PCS ELSE 0 END) AS AIPCS"
    '        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN GRSWT ELSE 0 END) AS AIGRSWT"
    '        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN NETWT ELSE 0 END) AS AINETWT"
    '        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN DIAWT ELSE 0 END) AS AIDIAWT"
    '        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN STNWT ELSE 0 END) AS AISTNWT"
    '        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'APPISS' THEN VALUE ELSE 0 END) AS AIVALUE"
    '    End If
    '    strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*PCS ELSE PCS END) AS CPCS"
    '    strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*GRSWT ELSE GRSWT END) AS CGRSWT"
    '    strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*NETWT ELSE NETWT END) AS CNETWT"
    '    strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*DIAWT ELSE DIAWT END) AS CDIAWT"
    '    strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*STNWT ELSE DIAWT END) AS CSTNWT"
    '    strSql += vbCrLf + " ,SUM(CASE WHEN SEP IN ('ISS','APPISS') THEN -1*VALUE ELSE VALUE END) AS CVALUE"
    '    strSql += vbCrLf + " ,RATE"
    '    strSql += vbCrLf + " ,1 RESULT,' 'COLHEAD,STONE "
    '    If chkWithSubItem.Checked Then
    '        strSql += vbCrLf + " ,CASE WHEN ISNULL(SUBITEM,'') <> '' THEN SUBITEM ELSE ITEM END AS PARTICULAR"
    '    Else
    '        strSql += vbCrLf + " ,ITEM AS PARTICULAR"
    '    End If
    '    strSql += vbCrLf + " INTO TEMP" & systemId & "ITEMSTK"
    '    strSql += vbCrLf + " FROM MASTER..TEMP_ITEMSTKVIEW AS X"
    '    strSql += vbCrLf + " GROUP BY  TITEM,ITEM,SUBITEM,STONE,RATE"
    '    strSql += vbCrLf + " ,ITEMCTRID"
    '    strSql += vbCrLf + " ,DESIGNERID"
    '    strSql += vbCrLf + " ,COSTID"
    '    strSql += vbCrLf + " ,SUBITEM,STYLENO"
    '    cmd = New OleDbCommand(strSql, cn)
    '    cmd.CommandTimeout = 100
    '    cmd.ExecuteNonQuery()

    '    'Dim GroupColumn As String = Nothing
    '    'If cmbGroupBy.Text <> "ITEM WISE" Or chkWithSubItem.Checked Then
    '    '    Select Case cmbGroupBy.Text
    '    '        Case "COUNTER WISE"
    '    '            GroupColumn = "COUNTER"
    '    '        Case "DESIGNER WISE"
    '    '            GroupColumn = "DESIGNER"
    '    '        Case "COSTCENTRE WISE"
    '    '            GroupColumn = "COSTNAME"
    '    '        Case "ITEM WISE"
    '    '            GroupColumn = "TEMPITEM"
    '    '    End Select
    '    '    strSql = " /*INSERTING GROUP TITLE*/"
    '    '    strSql += vbcrlf + " INSERT INTO TEMP" & systemId & "ITEMSTK(" & GroupColumn & ",ITEM,RESULT,COLHEAD,STONE)"
    '    '    strSql += vbcrlf + " SELECT DISTINCT " & GroupColumn & "," & GroupColumn & ",0 RESULT,'T'COLHEAD,3 STONE FROM TEMP" & systemId & "ITEMSTK WHERE RESULT = 1"

    '    '    strSql += vbcrlf + " /*INSERTIN GROUP SUBTOTAL*/"
    '    '    strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "ITEMSTK(" & GroupColumn & ",ITEM,OPCS,OGRSWT,ONETWT,RPCS,RGRSWT,RNETWT,IPCS,IGRSWT,INETWT,CPCS,CGRSWT,CNETWT,RESULT,COLHEAD,STONE,ODIAWT,OSTNWT,IDIAWT,ISTNWT,RDIAWT,RSTNWT,CDIAWT,CSTNWT)"
    '    '    strSql += vbCrLf + " SELECT " & GroupColumn & "," & IIf(rbtSummary.Checked, GroupColumn, "'SUBTOTAL'") & " AS ITEM,SUM(OPCS)OPCS,SUM(OGRSWT)OGRSWT,SUM(ONETWT)ONETWT,SUM(RPCS)RPCS,SUM(RGRSWT)RGRSWT,SUM(RNETWT)RNETWT,SUM(IPCS)IPCS,SUM(IGRSWT)IGRSWT,SUM(INETWT)INETWT,SUM(CPCS)CPCS,SUM(CGRSWT)CGRSWT,SUM(CNETWT)CNETWT,2 RESULT,'S'COLHEAD,3 STONE,SUM(ODIAWT),SUM(OSTNWT),SUM(IDIAWT),SUM(ISTNWT),SUM(RDIAWT),SUM(RSTNWT),SUM(CDIAWT),SUM(CSTNWT) FROM TEMP" & systemId & "ITEMSTK WHERE RESULT = 1 AND ISNULL(STONE,0) = 0"
    '    '    strSql += vbcrlf + " GROUP BY " & GroupColumn & ""
    '    '    cmd = New OleDbCommand(strSql, cn)
    '    '    cmd.CommandTimeout = 100
    '    '    cmd.ExecuteNonQuery()

    '    '    strSql = " /*INSERTIN Grand */"
    '    '    strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "ITEMSTK(" & GroupColumn & ",ITEM,OPCS,OGRSWT,ONETWT,RPCS,RGRSWT,RNETWT,IPCS,IGRSWT,INETWT,CPCS,CGRSWT,CNETWT,RESULT,COLHEAD,STONE,ODIAWT,OSTNWT,IDIAWT,ISTNWT,RDIAWT,RSTNWT,CDIAWT,CSTNWT)"
    '    '    strSql += vbCrLf + " SELECT 'ZZZZZZ','GRAND TOTAL'ITEM,SUM(OPCS)OPCS,SUM(OGRSWT)OGRSWT,SUM(ONETWT)ONETWT,SUM(RPCS)RPCS,SUM(RGRSWT)RGRSWT,SUM(RNETWT)RNETWT,SUM(IPCS)IPCS,SUM(IGRSWT)IGRSWT,SUM(INETWT)INETWT,SUM(CPCS)CPCS,SUM(CGRSWT)CGRSWT,SUM(CNETWT)CNETWT,2 RESULT,'G'COLHEAD,3 STONE,SUM(ODIAWT),SUM(OSTNWT),SUM(IDIAWT),SUM(ISTNWT),SUM(RDIAWT),SUM(RSTNWT),SUM(CDIAWT),SUM(CSTNWT) FROM TEMP" & systemId & "ITEMSTK WHERE RESULT = 1 AND ISNULL(STONE,0) = 0"
    '    '    cmd = New OleDbCommand(strSql, cn)
    '    '    cmd.CommandTimeout = 100
    '    '    cmd.ExecuteNonQuery()
    '    'Else
    '    '    GroupColumn = "TEMPITEM"
    '    '    strSql = " /*INSERTIN Grand */"
    '    '    strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "ITEMSTK(TEMPITEM,ITEM,OPCS,OGRSWT,ONETWT,RPCS,RGRSWT,RNETWT,IPCS,IGRSWT,INETWT,CPCS,CGRSWT,CNETWT,RESULT,COLHEAD,STONE,ODIAWT,OSTNWT,IDIAWT,ISTNWT,RDIAWT,RSTNWT,CDIAWT,CSTNWT)"
    '    '    strSql += vbCrLf + " SELECT 'ZZZZZZ','GRAND TOTAL'ITEM,SUM(OPCS)OPCS,SUM(OGRSWT)OGRSWT,SUM(ONETWT)ONETWT,SUM(RPCS)RPCS,SUM(RGRSWT)RGRSWT,SUM(RNETWT)RNETWT,SUM(IPCS)IPCS,SUM(IGRSWT)IGRSWT,SUM(INETWT)INETWT,SUM(CPCS)CPCS,SUM(CGRSWT)CGRSWT,SUM(CNETWT)CNETWT,2 RESULT,'G'COLHEAD,3 STONE,SUM(ODIAWT),SUM(OSTNWT),SUM(IDIAWT),SUM(ISTNWT),SUM(RDIAWT),SUM(RSTNWT),SUM(CDIAWT),SUM(CSTNWT) FROM TEMP" & systemId & "ITEMSTK WHERE RESULT = 1 AND ISNULL(STONE,0) = 0"
    '    '    cmd = New OleDbCommand(strSql, cn)
    '    '    cmd.CommandTimeout = 100
    '    '    cmd.ExecuteNonQuery()
    '    'End If




    '    strSql = " UPDATE TEMP" & systemId & "ITEMSTK SET OPCS = NULL WHERE OPCS = 0"
    '    strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET OGRSWT = NULL WHERE OGRSWT = 0"
    '    strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ONETWT = NULL WHERE ONETWT = 0"
    '    strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ODIAWT = NULL WHERE ODIAWT = 0"
    '    strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET OSTNWT = NULL WHERE OSTNWT = 0"
    '    strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET OVALUE = NULL WHERE OVALUE = 0"

    '    strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RPCS = NULL WHERE RPCS = 0"
    '    strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RGRSWT = NULL WHERE RGRSWT = 0"
    '    strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RNETWT = NULL WHERE RNETWT = 0"
    '    strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RDIAWT = NULL WHERE RDIAWT = 0"
    '    strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RSTNWT = NULL WHERE RSTNWT = 0"
    '    strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RVALUE = NULL WHERE RVALUE = 0"
    '    strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET IPCS = NULL WHERE IPCS = 0"
    '    strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET IGRSWT = NULL WHERE IGRSWT = 0"
    '    strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET INETWT = NULL WHERE INETWT = 0"
    '    strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET IDIAWT = NULL WHERE IDIAWT = 0"
    '    strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ISTNWT = NULL WHERE ISTNWT = 0"
    '    strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET IVALUE = NULL WHERE IVALUE = 0"
    '    If chkSeperateColumnApproval.Checked Then
    '        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARPCS = NULL WHERE ARPCS = 0"
    '        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARGRSWT = NULL WHERE ARGRSWT = 0"
    '        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARNETWT = NULL WHERE ARNETWT = 0"
    '        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARDIAWT = NULL WHERE ARDIAWT = 0"
    '        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARSTNWT = NULL WHERE ARSTNWT = 0"
    '        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ARVALUE = NULL WHERE ARVALUE = 0"
    '        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AIPCS = NULL WHERE AIPCS = 0"
    '        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AIGRSWT = NULL WHERE AIGRSWT = 0"
    '        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AINETWT = NULL WHERE AINETWT = 0"
    '        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AIDIAWT = NULL WHERE AIDIAWT = 0"
    '        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AISTNWT = NULL WHERE AISTNWT = 0"
    '        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET AIVALUE = NULL WHERE AIVALUE = 0"
    '    End If
    '    strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CPCS = NULL WHERE CPCS = 0"
    '    strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CGRSWT = NULL WHERE CGRSWT = 0"
    '    strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CNETWT = NULL WHERE CNETWT = 0"
    '    strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CDIAWT = NULL WHERE CDIAWT = 0"
    '    strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CSTNWT = NULL WHERE CSTNWT = 0"
    '    strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET CVALUE = NULL WHERE CVALUE = 0"
    '    strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET RATE = NULL WHERE RATE = 0"
    '    'strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET ITEM = '    '+ITEM WHERE STONE = 1"
    '    strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET PARTICULAR = '    '+PARTICULAR WHERE STONE = 1"
    '    If chkWithSubItem.Checked Then
    '        strSql += vbCrLf + " UPDATE TEMP" & systemId & "ITEMSTK SET SUBITEM = '    '+SUBITEM WHERE STONE = 1"
    '    End If
    '    cmd = New OleDbCommand(strSql, cn)
    '    cmd.CommandTimeout = 100
    '    cmd.ExecuteNonQuery()


    '    strSql = " SELECT PARTICULAR"
    '    'If chkWithSubItem.Checked Then
    '    '    strSql += vbCrLf + " CASE WHEN ISNULL(SUBITEM,'') <> '' THEN SUBITEM ELSE ITEM END AS PARTICULAR"
    '    'Else
    '    '    strSql += vbCrLf + " ITEM AS PARTICULAR"
    '    'End If
    '    strSql += vbCrLf + " ,OPCS,OGRSWT,ONETWT,ODIAWT,OSTNWT,OVALUE"
    '    strSql += vbCrLf + " ,RPCS,RGRSWT,RNETWT,RDIAWT,RSTNWT,RVALUE"
    '    strSql += vbCrLf + " ,IPCS,IGRSWT,INETWT,IDIAWT,ISTNWT,IVALUE"
    '    If chkSeperateColumnApproval.Checked Then
    '        strSql += vbCrLf + " ,ARPCS,ARGRSWT,ARNETWT,ARDIAWT,ARSTNWT,ARVALUE"
    '        strSql += vbCrLf + " ,AIPCS,AIGRSWT,AINETWT,AIDIAWT,AISTNWT,AIVALUE"
    '    End If
    '    strSql += vbCrLf + " ,CPCS,CGRSWT,CNETWT,CDIAWT,CSTNWT,CVALUE"
    '    strSql += vbCrLf + " ,COLHEAD,COUNTER,DESIGNER,ITEM,SUBITEM,COSTCENTRE,STYLENO,RATE,STONE,TITEM"
    '    strSql += vbCrLf + "  FROM TEMP" & systemId & "ITEMSTK"
    '    'strSql += vbcrlf + "  WHERE NOT(" & GroupColumn & " <> 'ZZZZZZ' AND RESULT <> 0 AND ISNULL(OPCS,0) = 0 AND ISNULL(RPCS,0) = 0 AND ISNULL(IPCS,0) = 0 AND ISNULL(CPCS,0) = 0 "
    '    'If chkGrsWt.Checked Then
    '    '    strSql += vbcrlf + "  AND ISNULL(OGRSWT,0) = 0 AND ISNULL(RGRSWT,0) = 0 AND ISNULL(IGRSWT,0) = 0 AND ISNULL(CGRSWT,0) = 0 "
    '    'End If
    '    'If chkNetWt.Checked Then
    '    '    strSql += vbcrlf + "  AND ISNULL(ONETWT,0) = 0 AND ISNULL(RNETWT,0) = 0 AND ISNULL(INETWT,0) = 0 AND ISNULL(CNETWT,0) = 0 "
    '    'End If
    '    'strSql += vbcrlf + " )"
    '    'If rbtSummary.Checked Then
    '    '    strSql += vbcrlf + " AND RESULT NOT IN (0,1)"
    '    'End If
    '    'If cmbGroupBy.Text = "COUNTER WISE" Then
    '    '    strSql += vbcrlf + " ORDER BY COUNTER,RESULT,TEMPITEM,STONE,PARTICULAR"
    '    'ElseIf cmbGroupBy.Text = "DESIGNER WISE" Then
    '    '    strSql += vbcrlf + " ORDER BY DESIGNER,RESULT,TEMPITEM,STONE,PARTICULAR"
    '    'ElseIf cmbGroupBy.Text = "COSTCENTRE WISE" Then
    '    '    strSql += vbcrlf + " ORDER BY COSTNAME,RESULT,TEMPITEM,STONE,PARTICULAR"
    '    'ElseIf cmbGroupBy.Text = "ITEM WISE" And chkWithSubItem.Checked = True Then
    '    '    strSql += vbCrLf + " ORDER BY TEMPITEM,RESULT,STONE,PARTICULAR"
    '    'Else
    '    '    strSql += vbCrLf + " ORDER BY RESULT,TEMPITEM,STONE,PARTICULAR"
    '    'End If

    '    'strSql = " SELECT * FROM TEMP" & systemId & "TAGSTOCKVIEW"
    '    tabView.Show()
    '    Dim dtSource As New DataTable
    '    dtSource.Columns.Add("KEYNO", GetType(Integer))
    '    dtSource.Columns("KEYNO").AutoIncrement = True
    '    dtSource.Columns("KEYNO").AutoIncrementSeed = 0
    '    dtSource.Columns("KEYNO").AutoIncrementStep = 1
    '    da = New OleDbDataAdapter(strSql, cn)
    '    da.Fill(dtSource)
    '    Dim ObjGrouper As New GiritechPack.DataGridViewGrouper(gridView, dtSource)
    '    For cnt As Integer = 0 To ChkLstGroupBy.CheckedItems.Count - 1
    '        ObjGrouper.pColumns_Group.Add(ChkLstGroupBy.CheckedItems.Item(cnt).ToString)
    '    Next
    '    If chkWithSubItem.Checked Then ObjGrouper.pColumns_Group.Add("ITEM")
    '    ObjGrouper.pColumns_Sum.Add("OPCS")
    '    ObjGrouper.pColumns_Sum.Add("OGRSWT")
    '    ObjGrouper.pColumns_Sum.Add("ONETWT")
    '    ObjGrouper.pColumns_Sum.Add("ODIAWT")
    '    ObjGrouper.pColumns_Sum.Add("OSTNWT")
    '    ObjGrouper.pColumns_Sum.Add("OVALUE")

    '    ObjGrouper.pColumns_Sum.Add("RPCS")
    '    ObjGrouper.pColumns_Sum.Add("RGRSWT")
    '    ObjGrouper.pColumns_Sum.Add("RNETWT")
    '    ObjGrouper.pColumns_Sum.Add("RDIAWT")
    '    ObjGrouper.pColumns_Sum.Add("RSTNWT")
    '    ObjGrouper.pColumns_Sum.Add("RVALUE")
    '    ObjGrouper.pColumns_Sum.Add("IPCS")
    '    ObjGrouper.pColumns_Sum.Add("IGRSWT")
    '    ObjGrouper.pColumns_Sum.Add("INETWT")
    '    ObjGrouper.pColumns_Sum.Add("IDIAWT")
    '    ObjGrouper.pColumns_Sum.Add("ISTNWT")
    '    ObjGrouper.pColumns_Sum.Add("IVALUE")

    '    If chkSeperateColumnApproval.Checked Then
    '        ObjGrouper.pColumns_Sum.Add("ARPCS")
    '        ObjGrouper.pColumns_Sum.Add("ARGRSWT")
    '        ObjGrouper.pColumns_Sum.Add("ARNETWT")
    '        ObjGrouper.pColumns_Sum.Add("ARDIAWT")
    '        ObjGrouper.pColumns_Sum.Add("ARSTNWT")
    '        ObjGrouper.pColumns_Sum.Add("ARVALUE")
    '        ObjGrouper.pColumns_Sum.Add("AIPCS")
    '        ObjGrouper.pColumns_Sum.Add("AIGRSWT")
    '        ObjGrouper.pColumns_Sum.Add("AINETWT")
    '        ObjGrouper.pColumns_Sum.Add("AIDIAWT")
    '        ObjGrouper.pColumns_Sum.Add("AISTNWT")
    '        ObjGrouper.pColumns_Sum.Add("AIVALUE")
    '    End If

    '    ObjGrouper.pColumns_Sum.Add("CPCS")
    '    ObjGrouper.pColumns_Sum.Add("CGRSWT")
    '    ObjGrouper.pColumns_Sum.Add("CNETWT")
    '    ObjGrouper.pColumns_Sum.Add("CDIAWT")
    '    ObjGrouper.pColumns_Sum.Add("CSTNWT")
    '    ObjGrouper.pColumns_Sum.Add("CVALUE")
    '    ObjGrouper.pColName_Particular = "PARTICULAR"
    '    ObjGrouper.pColName_ReplaceWithParticular = "PARTICULAR"
    '    ObjGrouper.pColumns_Sort = "TITEM,STONE"
    '    ObjGrouper.pColumns_Sum_FilterString = "STONE <> '1'"
    '    ObjGrouper.GroupDgv()
    '    If HideSummary = False Then
    '        Dim ind As Integer = gridView.RowCount - 1
    '        CType(gridView.DataSource, DataTable).Rows.Add()
    '        CType(gridView.DataSource, DataTable).Rows.Add()
    '        gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "OPENING"
    '        gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("OPCS").Value
    '        gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("OGRSWT").Value
    '        gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("ONETWT").Value
    '        gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("ODIAWT").Value
    '        gridView.Rows(gridView.RowCount - 1).Cells("OSTNWT").Value = gridView.Rows(ind).Cells("ONETWT").Value
    '        gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("OVALUE").Value
    '        gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
    '        gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = GlobalVariables.reportTotalStyle
    '        ''RECEIPT
    '        CType(gridView.DataSource, DataTable).Rows.Add()
    '        gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "RECEIPT"
    '        gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("RPCS").Value
    '        gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("RGRSWT").Value
    '        gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("RNETWT").Value
    '        gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("RDIAWT").Value
    '        gridView.Rows(gridView.RowCount - 1).Cells("OSTNWT").Value = gridView.Rows(ind).Cells("RNETWT").Value
    '        gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("RVALUE").Value

    '        gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
    '        gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = GlobalVariables.reportTotalStyle
    '        ''ISSUE
    '        CType(gridView.DataSource, DataTable).Rows.Add()
    '        gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "ISSUE"
    '        gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("IPCS").Value
    '        gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("IGRSWT").Value
    '        gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("INETWT").Value
    '        gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("IDIAWT").Value
    '        gridView.Rows(gridView.RowCount - 1).Cells("OSTNWT").Value = gridView.Rows(ind).Cells("INETWT").Value
    '        gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("IVALUE").Value
    '        gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
    '        gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = GlobalVariables.reportTotalStyle
    '        If chkSeperateColumnApproval.Checked Then
    '            ''APP RECEIPT
    '            CType(gridView.DataSource, DataTable).Rows.Add()
    '            gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "APP RECEIPT"
    '            gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("ARPCS").Value
    '            gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("ARGRSWT").Value
    '            gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("ARNETWT").Value
    '            gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("ARDIAWT").Value
    '            gridView.Rows(gridView.RowCount - 1).Cells("OSTNWT").Value = gridView.Rows(ind).Cells("ARNETWT").Value
    '            gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("ARVALUE").Value

    '            gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
    '            gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = GlobalVariables.reportTotalStyle
    '            ''APP ISSUE
    '            CType(gridView.DataSource, DataTable).Rows.Add()
    '            gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "APP ISSUE"
    '            gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("AIPCS").Value
    '            gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("AIGRSWT").Value
    '            gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("AINETWT").Value
    '            gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("AIDIAWT").Value
    '            gridView.Rows(gridView.RowCount - 1).Cells("OSTNWT").Value = gridView.Rows(ind).Cells("AINETWT").Value
    '            gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("AIVALUE").Value
    '            gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
    '            gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = GlobalVariables.reportTotalStyle

    '        End If
    '        ''CLOSING
    '        CType(gridView.DataSource, DataTable).Rows.Add()
    '        gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "CLOSING"
    '        gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = gridView.Rows(ind).Cells("CPCS").Value
    '        gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = gridView.Rows(ind).Cells("CGRSWT").Value
    '        gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = gridView.Rows(ind).Cells("CNETWT").Value
    '        gridView.Rows(gridView.RowCount - 1).Cells("ODIAWT").Value = gridView.Rows(ind).Cells("CDIAWT").Value
    '        gridView.Rows(gridView.RowCount - 1).Cells("OSTNWT").Value = gridView.Rows(ind).Cells("CNETWT").Value
    '        gridView.Rows(gridView.RowCount - 1).Cells("OVALUE").Value = gridView.Rows(ind).Cells("CVALUE").Value
    '        gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
    '        gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = GlobalVariables.reportTotalStyle
    '    End If
    '    'If HideSummary = False Then
    '    '    strSql = vbCrLf + " INSERT INTO TEMP" & systemId & "ITEMSTK(" & GroupColumn & ",ITEM,RESULT,COLHEAD,STONE)"
    '    '    strSql += vbCrLf + " SELECT 'ZZZZZZ',''ITEM,3 RESULT,' ',3 STONE "
    '    '    strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "ITEMSTK(" & GroupColumn & ",ITEM,OPCS,OGRSWT,ONETWT,RESULT,COLHEAD,STONE)"
    '    '    strSql += vbCrLf + " SELECT 'ZZZZZZ','OPENING'ITEM,SUM(OPCS)OPCS,SUM(OGRSWT)OGRSWT,SUM(ONETWT)ONETWT,4 RESULT,'G',4 STONE FROM TEMP" & systemId & "ITEMSTK WHERE RESULT = 1 AND ISNULL(STONE,0) = 0"
    '    '    strSql += vbCrLf + " UNION ALL"
    '    '    strSql += vbCrLf + " SELECT 'ZZZZZZ','RECEIPT'ITEM,SUM(RPCS)RPCS,SUM(RGRSWT)RGRSWT,SUM(RNETWT)RNETWT,5 RESULT,'G',5 STONE FROM TEMP" & systemId & "ITEMSTK WHERE RESULT = 1 AND ISNULL(STONE,0) = 0"
    '    '    strSql += vbCrLf + " UNION ALL"
    '    '    strSql += vbCrLf + " SELECT 'ZZZZZZ','ISSUE'ITEM,SUM(IPCS)IPCS,SUM(IGRSWT)IGRSWT,SUM(INETWT)INETWT,6 RESULT,'G',6 STONE FROM TEMP" & systemId & "ITEMSTK WHERE RESULT = 1 AND ISNULL(STONE,0) = 0"
    '    '    strSql += vbCrLf + " UNION ALL"
    '    '    strSql += vbCrLf + " SELECT 'ZZZZZZ','CLOSING'ITEM,SUM(CPCS)CPCS,SUM(CGRSWT)CGRSWT,SUM(CNETWT)CNETWT,7 RESULT,'G',7 STONE FROM TEMP" & systemId & "ITEMSTK WHERE RESULT = 1 AND ISNULL(STONE,0) = 0"
    '    '    cmd = New OleDbCommand(strSql, cn)
    '    '    cmd.CommandTimeout = 100
    '    '    cmd.ExecuteNonQuery()
    '    'End If

    '    'Dim dt As New DataTable
    '    'dt.Columns.Add("KEYNO", GetType(Integer))
    '    'dt.Columns("KEYNO").AutoIncrement = True
    '    'dt.Columns("KEYNO").AutoIncrementSeed = 0
    '    'dt.Columns("KEYNO").AutoIncrementStep = 1
    '    'da = New OleDbDataAdapter(strSql, cn)
    '    'da.Fill(dt)
    '    'If Not dt.Rows.Count > 0 Then
    '    '    MsgBox("Record not found", MsgBoxStyle.Information)
    '    '    dtpAsOnDate.Focus()
    '    '    Exit Sub
    '    'End If
    '    'dt.Columns("KEYNO").SetOrdinal(dt.Columns.Count - 1)
    '    'tabView.Show()
    '    'gridView.DataSource = dt
    '    lblTitle.Text = ""
    '    If rbtTag.Checked Then lblTitle.Text += " TAGGED"
    '    If rbtNonTag.Checked Then lblTitle.Text += " NON TAGGED"
    '    lblTitle.Text += " ITEM WISE STOCK REPORT"
    '    If chkAsOnDate.Checked Then
    '        lblTitle.Text += " AS ON " & dtpFrom.Value.ToString("dd-MM-yyyy")
    '    Else
    '        lblTitle.Text += " FROM " & dtpFrom.Text & " TO " & dtpTo.Text
    '    End If
    '    If chkCmbMetal.Text <> "ALL" Then lblTitle.Text += " FOR " & chkCmbMetal.Text
    '    If rbtAll.Checked = False Then lblTitle.Text += " [" & IIf(rbtRegular.Checked, "REGULAR", "ORDER") & " STOCK]"
    '    lblTitle.Font = New Font("VERDANA", 8, FontStyle.Bold)
    '    GridStyle()
    '    gridView.Columns("COLHEAD").Visible = False
    '    GridViewHeaderStyle()
    '    tabMain.SelectedTab = tabView
    'End Sub
    Private Sub Report()


        gridView.DataSource = Nothing

        strSql = " Exec " & cnStockDb & "..SP_RPT_SAL_DIFFERENCE "
        strSql += vbCrLf + " @FROMDATE ='" & dtpFrom.Value.ToString("yyyy-MM-dd") & "',"
        strSql += vbCrLf + " @TODATE ='" & dtpTo.Value.ToString("yyyy-MM-dd") & "',"
        strSql += vbCrLf + " @METAL ='" & chkCmbMetal.Text & "', "
        strSql += vbCrLf + " @COSTCENTRE ='" & chkCmbCostCentre.Text & "'"
        'strSql += vbCrLf + " @AMETAL ='" & chkCmbALLOY.Text & "', "
        'strSql += vbCrLf + " @TRANTYPE ='" & IIf(rbtReceipt.Checked = True, "R", "I") & "'"
        '        tabView.Show()
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

        Dim dtSource As New DataTable
        dtSource.Columns.Add("KEYNO", GetType(Integer))
        dtSource.Columns("KEYNO").AutoIncrement = True
        dtSource.Columns("KEYNO").AutoIncrementSeed = 0
        dtSource.Columns("KEYNO").AutoIncrementStep = 1
        strSql = "SELECT * FROM " & cnStockDb & "..TEMPSALDIFF"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtSource)
        If dtSource.Rows.Count < 1 Then
            MessageBox.Show("Record Not Found")
            Exit Sub
        End If
        tabMain.SelectedTab = tabView
        'gridView.DataSource = dtSource
        Dim ObjGrouper As New GiritechPack.DataGridViewGrouper(gridView, dtSource)
        'If rbtCategorywise.Checked = True Then ObjGrouper.pColumns_Group.Add("CATNAME")
        ObjGrouper.pColumns_Sum.Add("DIFFERENCE")
        'If rbtCategorywise.Checked = True Then
        ObjGrouper.pColName_Particular = "PRODUCTNAME"
        ObjGrouper.pColName_ReplaceWithParticular = "PRODUCTNAME"
        ObjGrouper.pColumns_Sort = "BILLDATE,BILLNO"
        'End If
        ObjGrouper.GroupDgv()
        GridStyle()
        Dim title As String = Nothing
        title += " SALES DIFFERENCE"
        title += " FROM " + dtpFrom.Text + " TO " + dtpTo.Text
        lblTitle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        lblTitle.Text = title
        gridView.Columns("DIFFERENCE").Visible = True
        Prop_Sets()
    End Sub
    Private Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        Report()
    End Sub

    Private Sub frmItemWiseStock_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Escape) And tabMain.SelectedTab.Name = tabView.Name Then
            btnBack_Click(Me, New EventArgs)
        ElseIf e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    ''Private Sub gridView_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles gridView.CellFormatting
    ''    With gridView
    ''        Select Case .Rows(e.RowIndex).Cells("COLHEAD").Value.ToString
    ''            Case "T"
    ''                .Rows(e.RowIndex).Cells("ITEMNAME").Style.BackColor = reportHeadStyle.BackColor
    ''                .Rows(e.RowIndex).Cells("ITEMNAME").Style.Font = reportHeadStyle.Font
    ''            Case "S"
    ''                .Rows(e.RowIndex).DefaultCellStyle.Font = reportSubTotalStyle.Font
    ''            Case "G"
    ''                .Rows(e.RowIndex).DefaultCellStyle.Font = reportTotalStyle.Font
    ''                .Rows(e.RowIndex).DefaultCellStyle.BackColor = reportTotalStyle.BackColor
    ''        End Select
    ''    End With
    ''End Sub

    Function GridViewFormat() As Integer
        For Each dgvRow As DataGridViewRow In gridView.Rows
            With dgvRow
                Select Case .Cells("COLHEAD").Value.ToString
                    Case "T"
                        .Cells("ITEMNAME").Style.BackColor = reportHeadStyle.BackColor
                        .Cells("ITEMNAME").Style.Font = reportHeadStyle.Font
                    Case "S"
                        .DefaultCellStyle.Font = reportSubTotalStyle.Font
                    Case "G"
                        .DefaultCellStyle.Font = reportTotalStyle.Font
                        .DefaultCellStyle.BackColor = reportTotalStyle.BackColor
                End Select
            End With
        Next
    End Function
    Private Sub gridView_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs) Handles gridView.ColumnWidthChanged
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = "X" Or e.KeyChar = "x" Then
            Me.btnExcel_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = "P" Then
            Me.btnPrint_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        funcExit()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        funcExit()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        'funcLoadMetal()

        dtpFrom.Value = GetServerDate()
        gridView.DataSource = Nothing
        ' chkDiamond.Checked = False
        ' chkStone.Checked = False
        GiritechPack.GlobalMethods.FillCombo(chkCmbMetal, dtMetal, "METALNAME", , "ALL")
        GiritechPack.GlobalMethods.FillCombo(chkCmbALLOY, dtALLOY, "METALNAME", , "ALL")
        GiritechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , "ALL")
        dtpFrom.Focus()
        dtpFrom.Select()
        Prop_Gets()
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not GiritechPack.Methods.GetRights(_DtUserRights, Me.Name, GiritechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
            GiriPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, GiriPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not GiritechPack.Methods.GetRights(_DtUserRights, Me.Name, GiritechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
            GiriPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, GiriPosting.GExport.GExportType.Print, )
        End If
    End Sub

    Private Sub frmItemWiseStock_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGen.Left, Me.tabGen.Top, Me.tabGen.Width, Me.tabGen.Height))
        strSql = " SELECT 'ALL' METALNAME,'ALL' METALID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT METALNAME,METALID,2 RESULT FROM " & cnAdminDb & "..METALMAST "
        strSql += " WHERE TTYPE='M'"
        strSql += " ORDER BY RESULT,METALNAME"
        dtMetal = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtMetal)
        GiritechPack.GlobalMethods.FillCombo(chkCmbMetal, dtMetal, "METALNAME", , "ALL")
        'strSql = " SELECT 'ALL' METALNAME,'ALL' METALID,1 RESULT"
        'strSql += " UNION ALL"
        'strSql += " SELECT METALNAME,METALID,2 RESULT FROM " & cnAdminDb & "..METALMAST "
        'strSql += " WHERE TTYPE='A'"
        'strSql += " ORDER BY RESULT,METALNAME"
        'dtALLOY = New DataTable
        'da = New OleDbDataAdapter(strSql, cn)
        'da.Fill(dtALLOY)
        'GiritechPack.GlobalMethods.FillCombo(chkCmbALLOY, dtALLOY, "METALNAME", , "ALL")
        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        GiritechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , "ALL")
        pnlGroupFilter.Location = New Point((ScreenWid - pnlGroupFilter.Width) / 2, ((ScreenHit - 128) - pnlGroupFilter.Height) / 2)
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGen
        chkCmbMetal.Focus()
    End Sub

    Private Sub ResizeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResizeToolStripMenuItem.Click
        If gridView.RowCount > 0 Then
            If ResizeToolStripMenuItem.Checked Then
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
            'gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        End If
    End Sub

    Private Sub chkCmbMetal_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCmbMetal.TextChanged
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New frmALLOYREPORT_Properties
        GetChecked_CheckedList(chkCmbMetal, obj.p_chkCmbMetal)
        GetChecked_CheckedList(chkCmbALLOY, obj.p_chkCmbALLOY)
        GetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre)
        obj.p_rbtTranDate = rbtTranDate.Checked
        obj.p_rbtCategorywise = rbtCategorywise.Checked
        obj.p_rbtReceipt = rbtReceipt.Checked
        obj.p_rbtIssue = rbtIssue.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmALLOYREPORT_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmALLOYREPORT_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmALLOYREPORT_Properties))
        SetChecked_CheckedList(chkCmbMetal, obj.p_chkCmbMetal, "ALL")
        SetChecked_CheckedList(chkCmbALLOY, obj.p_chkCmbALLOY, "ALL")
        SetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre, "ALL")
        rbtTranDate.Checked = obj.p_rbtTranDate
        rbtCategorywise.Checked = obj.p_rbtCategorywise
        rbtReceipt.Checked = obj.p_rbtReceipt
        rbtIssue.Checked = obj.p_rbtIssue
    End Sub

    Private Sub rbtReceipt_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtTranDate.CheckedChanged
        'If rbtReceipt.Checked = True Then
        '    chkDirectpurchase.Text = "Direct Purchase"
        'Else
        '    chkDirectpurchase.Text = "Direct Sales"
        'End If
    End Sub

    Private Sub rbtIssue_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtCategorywise.CheckedChanged
        'If rbtIssue.Checked = False Then
        '    chkDirectpurchase.Text = "Direct Purchase"
        'Else
        '    chkDirectpurchase.Text = "Direct Sales"
        'End If
    End Sub

    Private Sub tabGen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tabGen.Click

    End Sub
End Class

Public Class frmSalesDifferenceReport_Properties
    Private chkCmbMetal As New List(Of String)
    Public Property p_chkCmbMetal() As List(Of String)
        Get
            Return chkCmbMetal
        End Get
        Set(ByVal value As List(Of String))
            chkCmbMetal = value
        End Set
    End Property

    Private chkCmbALLOY As New List(Of String)
    Public Property p_chkCmbALLOY() As List(Of String)
        Get
            Return chkCmbALLOY
        End Get
        Set(ByVal value As List(Of String))
            chkCmbALLOY = value
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

    Private rbtTranDate As Boolean = True
    Public Property p_rbtTranDate() As Boolean
        Get
            Return rbtTranDate
        End Get
        Set(ByVal value As Boolean)
            rbtTranDate = value
        End Set
    End Property

    Private rbtCategorywise As Boolean = True
    Public Property p_rbtCategorywise() As Boolean
        Get
            Return rbtCategorywise
        End Get
        Set(ByVal value As Boolean)
            rbtCategorywise = value
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

    Private rbtIssue As Boolean = True
    Public Property p_rbtIssue() As Boolean
        Get
            Return rbtIssue
        End Get
        Set(ByVal value As Boolean)
            rbtIssue = value
        End Set
    End Property
End Class