Imports System.Data.OleDb
Public Class FRM_ORDERREPAIRSTATUS
    'CALID=592 : CLITENT - ALL: CORRECTION - COLOR INDICATION IS NOT COME PROPERLY : ALTER BY SATHYA
    Dim objGridShower As frmGridDispDia
    Private StrSql As String
    Private Cmd As OleDbCommand
    Private Da As OleDbDataAdapter
    Dim CostId As String = Nothing
    Dim defaultPic As String = GetAdmindbSoftValue("TAGCATALOGPATH")
    Dim dtSoftKeys As DataTable = GetAdmindbSoftValueAll()
    Dim READYITEMBOOKING As Boolean = IIf(GetAdmindbSoftValuefromDt(dtSoftKeys, "READYITEMBOOKING", "N") = "Y", True, False)
    Dim PicPath As String = GetAdmindbSoftValue("OR_PICPATH")
    Dim ORDER_MULTI_MIMR As Boolean = IIf(GetAdmindbSoftValue("ORDER_MULTI_MIMR", "N") = "Y", True, False)
    Dim RPT_ORSTATUS_TOT_WASTMC As Boolean = IIf(GetAdmindbSoftValue("RPT_ORSTATUS_TOT_WASTMC", "N") = "Y", True, False)

    Private Sub FRM_ORDERREPAIRSTATUS_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            ' If txtOrdRepNo.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        ElseIf e.KeyCode = Keys.F12 Then
            btnExit_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub FRM_ORDERREPAIRSTATUS_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        cmbCostcentre.Items.Clear()
        'cmbCostcentre.Text = ""
        If GetAdmindbSoftValue("COSTCENTRE", "N") = "Y" Then
            StrSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            objGPack.FillCombo(StrSql, cmbCostcentre)
            cmbCostcentre.Enabled = True
            cmbCostcentre.Text = IIf(cnDefaultCostId, "", cnCostName)
            If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then cmbCostcentre.Enabled = False
        Else
            cmbCostcentre.Enabled = False
        End If
        If READYITEMBOOKING Then rbtnbooked.Visible = True Else rbtnbooked.Visible = False
        Prop_Gets()
        If PicPath = "" Then PicPath = GetAdmindbSoftValue("PICPATH")
    End Sub

    Private Sub ReqDetail()
        StrSql = vbCrLf + " IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMPORMAST') > 0 DROP TABLE TEMPTABLEDB..TEMPORMAST"
        ''StrSql += vbCrLf + " IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'V' AND NAME = 'TEMPORMAST') > 0 DROP VIEW TEMPTABLEDB..TEMPORMAST"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
        StrSql = vbCrLf + " SELECT "
        StrSql += vbCrLf + " CONVERT(VARCHAR(50),(SELECT NAME FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = O.ITEMTYPEID))AS PARTICULAR"
        StrSql += vbCrLf + " ,CONVERT(VARCHAR(15),SUM(O.GRSWT))DIASIZE"
        StrSql += vbCrLf + " ,CONVERT(VARCHAR(20),NULL)DIAPCS,CONVERT(VARCHAR(20),NULL)DIAWT"
        StrSql += vbCrLf + " ,CONVERT(INT,1)RESULT"
        StrSql += vbCrLf + " ,CONVERT(VARCHAR(3),NULL)COLHEAD"
        StrSql += vbCrLf + " INTO TEMPTABLEDB..TEMPORMAST"
        StrSql += vbCrLf + "  FROM " & cnAdminDb & "..ORMAST AS O"
        StrSql += vbCrLf + " WHERE ORNO = '" & GetCostId(CostId) & GetCompanyId(strCompanyId) & txtOrdRepNo.Text & "'"
        StrSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
        StrSql += vbCrLf + " AND COSTID = '" & CostId & "'"
        StrSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
        StrSql += vbCrLf + " AND ISNULL(ORDCANCEL,'') = ''"
        If rbtorder.Checked Then StrSql += vbCrLf + "AND  O.ORTYPE='O'"
        If rbtRepair.Checked Then StrSql += vbCrLf + "AND  O.ORTYPE='R'"
        If rbtnbooked.Checked Then StrSql += vbCrLf + "AND  O.ORTYPE='B'"
        StrSql += vbCrLf + " GROUP BY ITEMTYPEID"
        StrSql += vbCrLf + " "
        StrSql += vbCrLf + " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMPORMAST)>0"
        StrSql += vbCrLf + " BEGIN"
        StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPORMAST(PARTICULAR,DIASIZE,COLHEAD,RESULT)"
        StrSql += vbCrLf + " SELECT 'ITEM TYPE','GRS WEIGHT','T',0 RESULT"
        StrSql += vbCrLf + " END"
        StrSql += vbCrLf + " "

        StrSql += vbCrLf + "      IF  (SELECT COUNT(*) FROM TEMPTABLEDB..TEMPORMAST)>0"
        StrSql += vbCrLf + "     BEGIN"
        StrSql += vbCrLf + " IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMPORSTONE')>0 DROP TABLE TEMPTABLEDB..TEMPORSTONE"

        StrSql += vbCrLf + " SELECT SIZEDESC,CONVERT(VARCHAR,CONVERT(NUMERIC(15,4),SUM(STNWT)/CASE WHEN SUM(STNPCS) > 0 THEN SUM(STNPCS) ELSE 1 END))DIASIZE,CONVERT(VARCHAR,SUM(STNPCS))DIAPCS,CONVERT(VARCHAR,SUM(STNWT))DIAWT,CONVERT(INT,3)RESULT,CONVERT(VARCHAR,NULL)COLHEAD"
        StrSql += vbCrLf + " INTO TEMPTABLEDB..TEMPORSTONE"
        StrSql += vbCrLf + " FROM"
        StrSql += vbCrLf + " ("
        StrSql += vbCrLf + " 	SELECT X.*,SZ.SIZEDESC FROM "
        StrSql += vbCrLf + " 	("
        StrSql += vbCrLf + " 	SELECT STNITEMID,STNSUBITEMID,STNPCS,STNWT,CONVERT(NUMERIC(15,4),CASE WHEN STNPCS > 0 THEN (STNWT/STNPCS)*100 ELSE STNWT*100 END) AS WTRANGE"
        StrSql += vbCrLf + " 	,CONVERT(NUMERIC(15,3),CASE WHEN STNPCS > 0 THEN (STNWT/STNPCS) ELSE STNWT END) AS DIASIZE"
        StrSql += vbCrLf + " 	FROM " & cnAdminDb & "..ORSTONE"
        StrSql += vbCrLf + " 	WHERE ORSNO IN (SELECT SNO FROM " & cnAdminDb & "..ORMAST WHERE ORNO= '" & GetCostId(CostId) & GetCompanyId(strCompanyId) & txtOrdRepNo.Text & "')"
        StrSql += vbCrLf + " 	)X"
        StrSql += vbCrLf + " 	LEFT OUTER JOIN " & cnAdminDb & "..CENTSIZE AS SZ ON SZ.ITEMID = STNITEMID AND SZ.SUBITEMID = STNSUBITEMID AND WTRANGE BETWEEN FROMCENT AND TOCENT"
        StrSql += vbCrLf + " )Y GROUP BY SIZEDESC"
        StrSql += vbCrLf + " "
        StrSql += vbCrLf + " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMPORSTONE)>0"
        StrSql += vbCrLf + " BEGIN"
        StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPORSTONE(DIASIZE,DIAPCS,DIAWT,RESULT,COLHEAD)"
        StrSql += vbCrLf + " SELECT 'DIA.SIZE','DIA.PCS','DIA.WT',2,'T'"
        StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPORSTONE(SIZEDESC,DIAPCS,DIAWT,RESULT,COLHEAD)"
        StrSql += vbCrLf + " SELECT 'TOTAL',SUM(CONVERT(INT,DIAPCS)),SUM(CONVERT(NUMERIC(15,4),DIAWT)),5 RESULT,'S'COLHEAD FROM TEMPTABLEDB..TEMPORSTONE WHERE RESULT = 3"
        StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPORMAST(PARTICULAR,DIASIZE,DIAPCS,DIAWT,RESULT,COLHEAD)"
        StrSql += vbCrLf + " SELECT SIZEDESC,DIASIZE,DIAPCS,DIAWT,RESULT,COLHEAD FROM TEMPTABLEDB..TEMPORSTONE "
        StrSql += vbCrLf + " END"
        StrSql += vbCrLf + " END"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
        StrSql = vbCrLf + " SELECT * FROM TEMPTABLEDB..TEMPORMAST"
        StrSql += vbCrLf + " ORDER BY RESULT"
        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("KEYNO", GetType(Integer))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dtGrid)
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
        objGridShower.Text = "ORDER REQUIREMENT"
        Dim tit As String = "ORDER REQUIREMENT FOR " + txtOrdRepNo.Text
        objGridShower.lblTitle.Text = tit
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(dtGrid)
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        objGridShower.FormReSize = False
        objGridShower.FormReLocation = False
        objGridShower.pnlFooter.Visible = False
        objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        'DataGridView_SummaryFormatting(objGridShower.gridView)
        FormatGridColumns(objGridShower.gridView, False, False, , False)
        With objGridShower.gridView
            .Columns("PARTICULAR").Width = 150
            .Columns("DIASIZE").Width = 100
            .Columns("DIAPCS").Width = 100
            .Columns("DIAWT").Width = 100
            .Columns("DIASIZE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("DIAPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("DIAWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("KEYNO").Visible = False
            .Columns("RESULT").Visible = False
            .Columns("COLHEAD").Visible = False
            .ColumnHeadersVisible = False
        End With
        objGridShower.Show()
        objGridShower.FormReSize = True
        objGridShower.FormReLocation = True
        objGridShower.gridViewHeader.Visible = False
        FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
        objGridShower.gridView_ColumnWidthChanged(Me, New DataGridViewColumnEventArgs(objGridShower.gridView.Columns("PARTICULAR")))
    End Sub
    Private Sub ReqDetail_09_AUG_2021()
        StrSql = vbCrLf + " IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMPORMAST') > 0 DROP TABLE TEMPORMAST"
        StrSql += vbCrLf + " IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE XTYPE = 'V' AND NAME = 'TEMPORMAST') > 0 DROP VIEW TEMPORMAST"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
        StrSql = vbCrLf + " SELECT "
        StrSql += vbCrLf + " CONVERT(VARCHAR(50),(SELECT NAME FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = O.ITEMTYPEID))AS PARTICULAR"
        StrSql += vbCrLf + " ,CONVERT(VARCHAR(15),SUM(O.GRSWT))DIASIZE"
        StrSql += vbCrLf + " ,CONVERT(VARCHAR(20),NULL)DIAPCS,CONVERT(VARCHAR(20),NULL)DIAWT"
        StrSql += vbCrLf + " ,CONVERT(INT,1)RESULT"
        StrSql += vbCrLf + " ,CONVERT(VARCHAR(3),NULL)COLHEAD"
        StrSql += vbCrLf + " INTO TEMPORMAST"
        StrSql += vbCrLf + "  FROM " & cnAdminDb & "..ORMAST AS O"
        StrSql += vbCrLf + " WHERE ORNO = '" & GetCostId(CostId) & GetCompanyId(strCompanyId) & txtOrdRepNo.Text & "'"
        StrSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
        StrSql += vbCrLf + " AND COSTID = '" & CostId & "'"
        StrSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
        StrSql += vbCrLf + " AND ISNULL(ORDCANCEL,'') = ''"
        If rbtorder.Checked Then StrSql += vbCrLf + "AND  O.ORTYPE='O'"
        If rbtRepair.Checked Then StrSql += vbCrLf + "AND  O.ORTYPE='R'"
        If rbtnbooked.Checked Then StrSql += vbCrLf + "AND  O.ORTYPE='B'"
        StrSql += vbCrLf + " GROUP BY ITEMTYPEID"
        StrSql += vbCrLf + " "
        StrSql += vbCrLf + " IF (SELECT COUNT(*) FROM TEMPORMAST)>0"
        StrSql += vbCrLf + " BEGIN"
        StrSql += vbCrLf + " INSERT INTO TEMPORMAST(PARTICULAR,DIASIZE,COLHEAD,RESULT)"
        StrSql += vbCrLf + " SELECT 'ITEM TYPE','GRS WEIGHT','T',0 RESULT"
        StrSql += vbCrLf + " END"
        StrSql += vbCrLf + " "

        StrSql += vbCrLf + "      IF  (SELECT COUNT(*) FROM TEMPTABLEDB..TEMPORSTATUS)>0"
        StrSql += vbCrLf + "     BEGIN"
        StrSql += vbCrLf + " IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'TEMPORSTONE')>0 DROP TABLE TEMPORSTONE"

        StrSql += vbCrLf + " SELECT SIZEDESC,CONVERT(VARCHAR,CONVERT(NUMERIC(15,4),SUM(STNWT)/CASE WHEN SUM(STNPCS) > 0 THEN SUM(STNPCS) ELSE 1 END))DIASIZE,CONVERT(VARCHAR,SUM(STNPCS))DIAPCS,CONVERT(VARCHAR,SUM(STNWT))DIAWT,CONVERT(INT,3)RESULT,CONVERT(VARCHAR,NULL)COLHEAD"
        StrSql += vbCrLf + " INTO TEMPORSTONE"
        StrSql += vbCrLf + " FROM"
        StrSql += vbCrLf + " ("
        StrSql += vbCrLf + " 	SELECT X.*,SZ.SIZEDESC FROM "
        StrSql += vbCrLf + " 	("
        StrSql += vbCrLf + " 	SELECT STNITEMID,STNSUBITEMID,STNPCS,STNWT,CONVERT(NUMERIC(15,4),CASE WHEN STNPCS > 0 THEN (STNWT/STNPCS)*100 ELSE STNWT*100 END) AS WTRANGE"
        StrSql += vbCrLf + " 	,CONVERT(NUMERIC(15,3),CASE WHEN STNPCS > 0 THEN (STNWT/STNPCS) ELSE STNWT END) AS DIASIZE"
        StrSql += vbCrLf + " 	FROM " & cnAdminDb & "..ORSTONE"
        StrSql += vbCrLf + " 	WHERE ORSNO IN (SELECT SNO FROM " & cnAdminDb & "..ORMAST WHERE ORNO= '" & GetCostId(CostId) & GetCompanyId(strCompanyId) & txtOrdRepNo.Text & "')"
        StrSql += vbCrLf + " 	)X"
        StrSql += vbCrLf + " 	LEFT OUTER JOIN " & cnAdminDb & "..CENTSIZE AS SZ ON SZ.ITEMID = STNITEMID AND SZ.SUBITEMID = STNSUBITEMID AND WTRANGE BETWEEN FROMCENT AND TOCENT"
        StrSql += vbCrLf + " )Y GROUP BY SIZEDESC"
        StrSql += vbCrLf + " "
        StrSql += vbCrLf + " IF (SELECT COUNT(*) FROM TEMPORSTONE)>0"
        StrSql += vbCrLf + " BEGIN"
        StrSql += vbCrLf + " INSERT INTO TEMPORSTONE(DIASIZE,DIAPCS,DIAWT,RESULT,COLHEAD)"
        StrSql += vbCrLf + " SELECT 'DIA.SIZE','DIA.PCS','DIA.WT',2,'T'"
        StrSql += vbCrLf + " INSERT INTO TEMPORSTONE(SIZEDESC,DIAPCS,DIAWT,RESULT,COLHEAD)"
        StrSql += vbCrLf + " SELECT 'TOTAL',SUM(CONVERT(INT,DIAPCS)),SUM(CONVERT(NUMERIC(15,4),DIAWT)),5 RESULT,'S'COLHEAD FROM TEMPORSTONE WHERE RESULT = 3"
        StrSql += vbCrLf + " INSERT INTO TEMPORMAST(PARTICULAR,DIASIZE,DIAPCS,DIAWT,RESULT,COLHEAD)"
        StrSql += vbCrLf + " SELECT SIZEDESC,DIASIZE,DIAPCS,DIAWT,RESULT,COLHEAD FROM TEMPORSTONE "
        StrSql += vbCrLf + " END"
        StrSql += vbCrLf + " END"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
        StrSql = vbCrLf + " SELECT * FROM TEMPORMAST"
        StrSql += vbCrLf + " ORDER BY RESULT"
        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("KEYNO", GetType(Integer))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dtGrid)
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
        objGridShower.Text = "ORDER REQUIREMENT"
        Dim tit As String = "ORDER REQUIREMENT FOR " + txtOrdRepNo.Text
        objGridShower.lblTitle.Text = tit
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(dtGrid)
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        objGridShower.FormReSize = False
        objGridShower.FormReLocation = False
        objGridShower.pnlFooter.Visible = False
        objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        'DataGridView_SummaryFormatting(objGridShower.gridView)
        FormatGridColumns(objGridShower.gridView, False, False, , False)
        With objGridShower.gridView
            .Columns("PARTICULAR").Width = 150
            .Columns("DIASIZE").Width = 100
            .Columns("DIAPCS").Width = 100
            .Columns("DIAWT").Width = 100
            .Columns("DIASIZE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("DIAPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("DIAWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("KEYNO").Visible = False
            .Columns("RESULT").Visible = False
            .Columns("COLHEAD").Visible = False
            .ColumnHeadersVisible = False
        End With
        objGridShower.Show()
        objGridShower.FormReSize = True
        objGridShower.FormReLocation = True
        objGridShower.gridViewHeader.Visible = False
        FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
        objGridShower.gridView_ColumnWidthChanged(Me, New DataGridViewColumnEventArgs(objGridShower.gridView.Columns("PARTICULAR")))
    End Sub
    Private Function NewSearch() As Integer
        If Not defaultPic.EndsWith("\") Then defaultPic += "\"
        If Not PicPath.EndsWith("\") Then PicPath += "\"
        StrSql = vbCrLf + "     CREATE TABLE TEMPTABLEDB..TEMPORSTATUS"
        StrSql += vbCrLf + "     ("
        StrSql += vbCrLf + "      COL1 VARCHAR(1500)"
        StrSql += vbCrLf + "     ,COL2 VARCHAR(1000)"
        StrSql += vbCrLf + "     ,COL3 VARCHAR(1000)"
        StrSql += vbCrLf + "     ,COL4 VARCHAR(1000)"
        StrSql += vbCrLf + "     ,COL5 VARCHAR(1000)"
        StrSql += vbCrLf + "     ,COL6 VARCHAR(1000)"
        StrSql += vbCrLf + "     ,COL7 VARCHAR(1000)"
        StrSql += vbCrLf + "     ,COL8 VARCHAR(1000)"
        StrSql += vbCrLf + "     ,COL9 VARCHAR(1000)"
        StrSql += vbCrLf + "     ,COL10 VARCHAR(1000)"
        StrSql += vbCrLf + "     ,COL11 VARCHAR(1000)"
        StrSql += vbCrLf + "     ,COL12 VARCHAR(1000)"
        StrSql += vbCrLf + "     ,COL13 VARCHAR(1000)"
        StrSql += vbCrLf + "     ,COL14 VARCHAR(1000)"
        StrSql += vbCrLf + "     ,COL15 VARCHAR(1000)"
        StrSql += vbCrLf + "     ,COL16 VARCHAR(1000)"
        StrSql += vbCrLf + "     ,COL17 VARCHAR(1000)"
        StrSql += vbCrLf + "     ,COL18 VARCHAR(1000)"
        StrSql += vbCrLf + "     ,COLIMAGE IMAGE"
        StrSql += vbCrLf + "     ,COLHEAD VARCHAR(2)"
        StrSql += vbCrLf + "     ,KEYNO INT IDENTITY(0,1)"
        StrSql += vbCrLf + "     )"
        StrSql += vbCrLf + " DECLARE @ORNO VARCHAR(15)"
        StrSql += vbCrLf + " DECLARE @COSTID VARCHAR(2)"
        StrSql += vbCrLf + " DECLARE @COMPANYID VARCHAR(3)"
        StrSql += vbCrLf + " DECLARE @CATALOGPATH VARCHAR(250)"
        StrSql += vbCrLf + " DECLARE @PICPATH VARCHAR(250)"
        StrSql += vbCrLf + " SET @ORNO = '" & GetCostId(CostId) & GetCompanyId(strCompanyId) & txtOrdRepNo.Text & "'"
        StrSql += vbCrLf + " SET @COSTID = '" & CostId & "'"
        StrSql += vbCrLf + " SET @COMPANYID = '" & strCompanyId & "'"
        StrSql += vbCrLf + " SET @CATALOGPATH = '" & defaultPic & "'"
        StrSql += vbCrLf + " SET @PICPATH = '" & PicPath & "'"

        If ORDER_MULTI_MIMR Then
            StrSql += vbCrLf + "     SELECT "
            StrSql += vbCrLf + "     IM.ITEMNAME AS ITEM,SM.SUBITEMNAME AS SUBITEM,ORIR.PCS,ORIR.GRSWT,ORIR.NETWT"
            'StrSql += vbCrLf + "     ,OR.MCGRM,OR.MC,OR.WASTPER,OR.WAST"
            If RPT_ORSTATUS_TOT_WASTMC Then
                StrSql += vbCrLf + "     ,0 MCGRM"
                StrSql += vbCrLf + "     ,(SELECT SUM(CASE WHEN ISNULL(ORSTATUS,'') = 'I' THEN MC*-1 ELSE MC END   ) FROM " & cnAdminDb & "..ORIRDETAIL WHERE  ORNO = ORIR.ORNO) MC"
                StrSql += vbCrLf + "     ,0 WASTPER"
                StrSql += vbCrLf + "     ,(SELECT SUM(CASE WHEN ISNULL(ORSTATUS,'') = 'I' THEN WASTAGE*-1 ELSE WASTAGE END   ) FROM " & cnAdminDb & "..ORIRDETAIL WHERE  ORNO = ORIR.ORNO) WAST"
            Else
                StrSql += vbCrLf + "     ,0 MCGRM,ORIR.MC,0 WASTPER,ORIR.WASTAGE WAST"
            End If
            StrSql += vbCrLf + "     ,CONVERT(VARCHAR,OM.RATE) + '('+ORRATE +')' AS RATE"
            'StrSql += vbCrLf + "     ,CASE WHEN ISNULL(OM.ODBATCHNO,'') <> '' AND ISNULL(OM.REASON,'')='' AND ISNULL(ISS.BATCHNO,'')='' THEN 'DELIVERED'"
            StrSql += vbCrLf + "     ,CASE WHEN ISNULL(OM.ODBATCHNO,'') <> ''  AND ISNULL(ISS.BATCHNO,'')='' THEN 'DELIVERED'"
            StrSql += vbCrLf + "       WHEN ISNULL(OM.ODBATCHNO,'') <> '' AND ISNULL(OM.REASON,'')='' AND ISNULL(ISS.BATCHNO,'')<>'' THEN 'APPROVAL ISSUE'"
            StrSql += vbCrLf + "       WHEN ISNULL(OM.ORDCANCEL,'') <> '' THEN 'CANCELLED'"
            StrSql += vbCrLf + "       WHEN ISNULL(OM.ORDCANCEL,'') = '' and ISNULL(OM.ODBATCHNO,'') = '' AND (select SUM(AMOUNT) from " & cnAdminDb & "..outstanding where recpay = 'P' AND PAYMODE = 'AP' AND OM.ORNO = RUNNO) >0 THEN 'ADVANCE REPAY'"
            StrSql += vbCrLf + "       ELSE ISNULL((SELECT ORDSTATE_NAME FROM " & cnAdminDb & "..ORDERSTATUS WHERE ORDSTATE_ID = (SELECT TOP 1 ORDSTATE_ID FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORSNO = OM.SNO ORDER BY ENTRYORDER DESC)),'PENDING WITH US') "
            StrSql += vbCrLf + "       END AS STATUS,OM.DISCOUNT,PER.PNAME,CASE WHEN PER.DOORNO <> '' THEN PER.DOORNO + ',' ELSE '' END + PER.ADDRESS1 AS ADDRESS1,PER.ADDRESS2,PER.AREA,CASE WHEN PER.PHONERES <> '' THEN PER.PHONERES + ',' ELSE '' END + PER.MOBILE AS MOBILE"
            StrSql += vbCrLf + "     ,OM.ORDATE,OM.REMDATE,OM.DUEDATE,(SELECT EMPNAME FROM " & cnAdminDb & "..EMPMASTER WHERE EMPID = OM.EMPID)aS EMPNAME "
            StrSql += vbCrLf + "     ,OM.STYLENO,OM.REASON,OM.PICTFILE"
            StrSql += vbCrLf + "     ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ORIRDETAILSTONE WHERE ORSNO=ORIR.SNO) AS STNPCS"
            StrSql += vbCrLf + "      ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ORIRDETAILSTONE WHERE ORSNO=ORIR.SNO) AS STNWT"
            StrSql += vbCrLf + "     ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ORIRDETAILSTONE WHERE ORSNO = ORIR.SNO) DIAPCS"
            StrSql += vbCrLf + "     ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ORIRDETAILSTONE WHERE ORSNO = ORIR.SNO) DIAWT"
            StrSql += vbCrLf + "     INTO TEMPTABLEDB..TEMPORBOOK"
            StrSql += vbCrLf + "     FROM " & cnAdminDb & "..ORIRDETAIL ORIR"
            StrSql += vbCrLf + "     INNER JOIN " & cnAdminDb & "..ORMAST AS OM ON ORIR.ORSNO = OM.SNO "
            StrSql += vbCrLf + "     INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = OM.ITEMID"
            StrSql += vbCrLf + "     LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = OM.ITEMID And SM.SUBITEMID = OM.SUBITEMID"
            StrSql += vbCrLf + "     INNER JOIN " & cnAdminDb & "..CUSTOMERINFO AS CUS ON CUS.BATCHNO = OM.BATCHNO"
            StrSql += vbCrLf + "     INNER JOIN " & cnAdminDb & "..PERSONALINFO AS PER ON PER.SNO = CUS.PSNO"
            StrSql += vbCrLf + "     LEFT JOIN " & cnStockDb & "..ISSUE ISS ON ISS.SNO=OM.ODSNO And ISS.TRANTYPE='AI'"
            StrSql += vbCrLf + "     WHERE ORIR.SNO IN((SELECT TOP 1 SNO  FROM " & cnAdminDb & "..ORIRDETAIL  WHERE ORNO = ORIR.ORNO AND ORSNO = ORIR.ORSNO ORDER BY ENTRYORDER DESC ))"
            StrSql += vbCrLf + "     AND OM.ORNO = @ORNO AND OM.COSTID = @COSTID AND OM.COMPANYID = @COMPANYID"
            If rbtorder.Checked Then StrSql += vbCrLf + "AND  OM.ORTYPE='O'"
            If rbtRepair.Checked Then StrSql += vbCrLf + "AND  OM.ORTYPE='R'"
            If rbtnbooked.Checked Then StrSql += vbCrLf + "AND  OM.ORTYPE='B'"
        Else
            StrSql += vbCrLf + "     SELECT "
            StrSql += vbCrLf + "     IM.ITEMNAME AS ITEM,SM.SUBITEMNAME AS SUBITEM,OM.PCS,OM.GRSWT,OM.NETWT"
            StrSql += vbCrLf + "     ,OM.MCGRM,OM.MC,OM.WASTPER,OM.WAST"
            StrSql += vbCrLf + "     ,CONVERT(VARCHAR,OM.RATE) + '('+ORRATE +')' AS RATE"
            StrSql += vbCrLf + "     ,CASE WHEN ISNULL(OM.ODBATCHNO,'') <> '' AND ISNULL(OM.REASON,'')='' AND ISNULL(ISS.BATCHNO,'')='' THEN 'DELIVERED'"
            StrSql += vbCrLf + "       WHEN ISNULL(OM.ODBATCHNO,'') <> '' AND ISNULL(OM.REASON,'')='' AND ISNULL(ISS.BATCHNO,'')<>'' THEN 'APPROVAL ISSUE'"
            StrSql += vbCrLf + "       WHEN ISNULL(OM.ORDCANCEL,'') <> '' THEN 'CANCELLED'"
            StrSql += vbCrLf + "       WHEN ISNULL(OM.ORDCANCEL,'') = '' and ISNULL(OM.ODBATCHNO,'') = '' AND (select SUM(AMOUNT) from " & cnAdminDb & "..outstanding where recpay = 'P' AND PAYMODE = 'AP' AND OM.ORNO = RUNNO) >0 THEN 'ADVANCE REPAY'"
            StrSql += vbCrLf + "       ELSE ISNULL((SELECT ORDSTATE_NAME FROM " & cnAdminDb & "..ORDERSTATUS WHERE ORDSTATE_ID = (SELECT TOP 1 ORDSTATE_ID FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORSNO = OM.SNO ORDER BY ENTRYORDER DESC)),'PENDING WITH US') "
            StrSql += vbCrLf + "       END AS STATUS,OM.DISCOUNT,PER.PNAME,CASE WHEN PER.DOORNO <> '' THEN PER.DOORNO + ',' ELSE '' END + PER.ADDRESS1 AS ADDRESS1,PER.ADDRESS2,PER.AREA,CASE WHEN PER.PHONERES <> '' THEN PER.PHONERES + ',' ELSE '' END + PER.MOBILE AS MOBILE"
            StrSql += vbCrLf + "     ,OM.ORDATE,OM.REMDATE,OM.DUEDATE,(SELECT EMPNAME FROM " & cnAdminDb & "..EMPMASTER WHERE EMPID = OM.EMPID)aS EMPNAME "
            StrSql += vbCrLf + "     ,OM.STYLENO,OM.REASON,OM.PICTFILE"
            'StrSql += vbCrLf + "     ,(SELECT CTLTEXT FROM "& cnadmindb &"..SOFTCONTROL WHERE ISNULL(CTLID,'') = 'PICPATH') PICPATH"
            'StrSql += vbCrLf + "     ,(SELECT CTLTEXT FROM "& cnadmindb &"..SOFTCONTROL WHERE ISNULL(CTLID,'') = 'TAGCATALOGPATH') CATALOGPATH"
            StrSql += vbCrLf + "     ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ORSTONE WHERE ORSNO=OM.SNO) AS STNPCS"
            StrSql += vbCrLf + "      ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ORSTONE WHERE ORSNO=OM.SNO) AS STNWT"
            StrSql += vbCrLf + "     ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ORSTONE WHERE ORSNO = OM.SNO) DIAPCS"
            StrSql += vbCrLf + "     ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ORSTONE WHERE ORSNO = OM.SNO) DIAWT"
            StrSql += vbCrLf + "     INTO TEMPTABLEDB..TEMPORBOOK"
            StrSql += vbCrLf + "     FROM " & cnAdminDb & "..ORMAST OM"
            StrSql += vbCrLf + "     INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = OM.ITEMID"
            StrSql += vbCrLf + "     LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = OM.ITEMID And SM.SUBITEMID = OM.SUBITEMID"
            StrSql += vbCrLf + "     INNER JOIN " & cnAdminDb & "..CUSTOMERINFO AS CUS ON CUS.BATCHNO = OM.BATCHNO"
            StrSql += vbCrLf + "     INNER JOIN " & cnAdminDb & "..PERSONALINFO AS PER ON PER.SNO = CUS.PSNO"
            StrSql += vbCrLf + "     LEFT JOIN " & cnStockDb & "..ISSUE ISS ON ISS.SNO=OM.ODSNO And ISS.TRANTYPE='AI'"
            StrSql += vbCrLf + "     WHERE OM.ORNO = @ORNO AND OM.COSTID = @COSTID AND OM.COMPANYID = @COMPANYID"
            If rbtorder.Checked Then StrSql += vbCrLf + "AND  OM.ORTYPE='O'"
            If rbtRepair.Checked Then StrSql += vbCrLf + "AND  OM.ORTYPE='R'"
            If rbtnbooked.Checked Then StrSql += vbCrLf + "AND  OM.ORTYPE='B'"
        End If



        StrSql += vbCrLf + "     IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMPORBOOK)>0"
        StrSql += vbCrLf + "     BEGIN"
        StrSql += vbCrLf + "     INSERT INTO TEMPTABLEDB..TEMPORSTATUS(COL1,COLHEAD)SELECT 'BOOKED DETAIL','T'"
        StrSql += vbCrLf + "     INSERT INTO TEMPTABLEDB..TEMPORSTATUS(COL1,COL2,COL7,COL8)SELECT TOP 1 'BOOKED DATE',CONVERT(vARCHAR,ORDATE,103),'NAME :',PNAME FROM TEMPTABLEDB..TEMPORBOOK"
        StrSql += vbCrLf + "     INSERT INTO TEMPTABLEDB..TEMPORSTATUS(COL1,COL2,COL7,COL8)SELECT TOP 1 'REMINDER DATE',CONVERT(vARCHAR,REMDATE,103),'ADDRESS :',ADDRESS1 FROM TEMPTABLEDB..TEMPORBOOK"
        StrSql += vbCrLf + "     INSERT INTO TEMPTABLEDB..TEMPORSTATUS(COL1,COL2,COL7,COL8)SELECT TOP 1 'DUE DATE',CONVERT(vARCHAR,DUEDATE,103),'',ADDRESS2 FROM TEMPTABLEDB..TEMPORBOOK"
        StrSql += vbCrLf + "     INSERT INTO TEMPTABLEDB..TEMPORSTATUS(COL1,COL2,COL7,COL8)SELECT TOP 1 'EMPLOYEE',EMPNAME,'AREA',AREA FROM TEMPTABLEDB..TEMPORBOOK"
        StrSql += vbCrLf + "     INSERT INTO TEMPTABLEDB..TEMPORSTATUS(COL1,COL2,COL7,COL8)SELECT TOP 1 'DISCOUNT',CASE WHEN DISCOUNT <> 0 THEN CONVERT(VARCHAR,DISCOUNT) ELSE NULL END,'PHONE',MOBILE FROM TEMPTABLEDB..TEMPORBOOK"
        StrSql += vbCrLf + "     INSERT INTO TEMPTABLEDB..TEMPORSTATUS(COL1,COL2,COL3,COL4,COL5,COL6,COL7,COL8,COL9,COL10,COL11,COL12,COL13,COL14,COL15,COL16,COLHEAD)"
        StrSql += vbCrLf + "     SELECT 'ITEM','SUBITEM','','PCS','GRSWT','NETWT','MCGRM','MC','WASTPER','WAST','STNWT','DIAPCS','DIAWT','RATE','STATUS','NATURE','T1'"
        StrSql += vbCrLf + "     INSERT INTO TEMPTABLEDB..TEMPORSTATUS(COL1,COL2,COL3,COL4,COL5,COL6,COL7,COL8,COL9,COL10,COL11,COL12,COL13,COL14,COL15,COL16,COL18,COLHEAD)" 'COL17 TO COL18
        StrSql += vbCrLf + "     SELECT ITEM,SUBITEM,'',PCS,GRSWT,NETWT,MCGRM,MC,WASTPER,WAST,STNWT,DIAPCS,DIAWT,RATE,STATUS"
        StrSql += vbCrLf + "     ,REASON,CASE WHEN ISNULL(PICTFILE,'') != '' THEN CASE WHEN ISNULL(STYLENO,'') != '' THEN @CATALOGPATH ELSE @PICPATH END END + PICTFILE PIC,'T5'"
        StrSql += vbCrLf + "     FROM TEMPTABLEDB..TEMPORBOOK"
        StrSql += vbCrLf + "     INSERT INTO TEMPTABLEDB..TEMPORSTATUS(COL1)SELECT ''"

        StrSql += vbCrLf + "     /** GETTTING SAMPLE INFO **/"
        StrSql += vbCrLf + "     SELECT IR.*,OM.SUBITEMID"
        StrSql += vbCrLf + "     ,OM.STYLENO,OM.REASON,OM.PICTFILE"
        StrSql += vbCrLf + "     ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ORSTONE WHERE ORSNO=OM.SNO) AS STNPCS,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ORSTONE WHERE ORSNO=OM.SNO) AS STNWT"
        StrSql += vbCrLf + "     ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ORSTONE WHERE ORSNO = OM.SNO) DIAPCS"
        StrSql += vbCrLf + "     ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ORSTONE WHERE ORSNO = OM.SNO) DIAWT"
        StrSql += vbCrLf + "     INTO TEMPTABLEDB..TEMPORSAMPLE"
        StrSql += vbCrLf + "     FROM " & cnAdminDb & "..ORSAMPLE AS IR "
        StrSql += vbCrLf + "     INNER JOIN " & cnAdminDb & "..ORMAST AS OM ON "
        StrSql += vbCrLf + "     	OM.SNO = IR.ORSNO AND OM.SNO = IR.ORSNO"
        StrSql += vbCrLf + "     	AND OM.ORNO = @ORNO AND OM.COSTID = @COSTID AND OM.COMPANYID = @COMPANYID"
        StrSql += vbCrLf + "     WHERE  ISNULL(IR.CANCEL,'') = ''"
        If rbtorder.Checked Then StrSql += vbCrLf + "AND  OM.ORTYPE='O'"
        If rbtRepair.Checked Then StrSql += vbCrLf + "AND  OM.ORTYPE='R'"
        If rbtnbooked.Checked Then StrSql += vbCrLf + "AND  OM.ORTYPE='B'"

        StrSql += vbCrLf + "     INSERT INTO TEMPTABLEDB..TEMPORSTATUS(COL1,COLHEAD)SELECT 'SAMPLE DETAIL','T'"
        StrSql += vbCrLf + "     IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMPORSAMPLE)>0"
        StrSql += vbCrLf + "     BEGIN"
        StrSql += vbCrLf + "     INSERT INTO TEMPTABLEDB..TEMPORSTATUS(COL1,COL2,Col3,COL4,COL5,COL6,COL7,COL8,COL9,COL10,COL11,COL12,COL13,COL14,COL15,COL16,COL17,COLHEAD)"
        StrSql += vbCrLf + "     SELECT 'ITEM','SUBITEM','','PCS','GRSWT','NETWT','MCGRM','MC','WASTPER','WAST','STNWT','DIAPCS','DIAWT','DESIGNER','STATUS','TRANNO','TRANDATE','T1'"
        StrSql += vbCrLf + "     INSERT INTO TEMPTABLEDB..TEMPORSTATUS(COL1,COL2,COL3,COL4,COL5,COL6,COL7,COL8,COL9,COL10,COL11,COL12,COL13,COL14,COL15,COL16,COL17,COL18)"
        StrSql += vbCrLf + "     SELECT IM.ITEMNAME AS ITEM,SM.SUBITEMNAME AS SUBITEM,'' tagno,'' AS PCS,IR.GRSWT,IR.NETWT,0,'' AS MC,0,'' AS WASTAGE,IR.STNWT,IR.DIAPCS,IR.DIAWT,'' AS DESIGNERNAME,'SAMPLE' AS ORDSTATE_NAME,'' AS TRANNO,'' AS TRANDATE"
        StrSql += vbCrLf + "     ,CASE WHEN ISNULL(PICTFILE,'') != '' THEN CASE WHEN ISNULL(IR.STYLENO,'') != '' THEN @CATALOGPATH ELSE @PICPATH END END + PICTFILE PIC"
        StrSql += vbCrLf + "     FROM TEMPTABLEDB..TEMPORSAMPLE AS IR"
        StrSql += vbCrLf + "     INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = IR.ITEMID"
        StrSql += vbCrLf + "     LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = IR.ITEMID AND SM.SUBITEMID = IR.SUBITEMID"
        StrSql += vbCrLf + "     INSERT INTO TEMPTABLEDB..TEMPORSTATUS(COL1)SELECT ''"
        StrSql += vbCrLf + "     END"
        If ORDER_MULTI_MIMR = False Then
            StrSql += vbCrLf + "     /** GETTTING SMITH ISSUE INFO **/"
            StrSql += vbCrLf + "     SELECT DE.DESIGNERNAME,IR.*,OM.ITEMID,OM.SUBITEMID"
            StrSql += vbCrLf + "     ,OM.STYLENO,OM.REASON,OM.PICTFILE"
            'StrSql += vbCrLf + "     ,(SELECT CTLTEXT FROM "& cnadmindb &"..SOFTCONTROL WHERE ISNULL(CTLID,'') = 'PICPATH') PICPATH"
            'StrSql += vbCrLf + "     ,(SELECT CTLTEXT FROM "& cnadmindb &"..SOFTCONTROL WHERE ISNULL(CTLID,'') = 'TAGCATALOGPATH') CATALOGPATH"
            StrSql += vbCrLf + "     ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ORSTONE WHERE ORSNO=OM.SNO) AS STNPCS"
            StrSql += vbCrLf + "     ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ORSTONE WHERE ORSNO=OM.SNO) AS STNWT"
            StrSql += vbCrLf + "     ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ORSTONE WHERE ORSNO = OM.SNO) DIAPCS"
            StrSql += vbCrLf + "     ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ORSTONE WHERE ORSNO = OM.SNO) DIAWT"
            StrSql += vbCrLf + "     INTO TEMPTABLEDB..TEMPORISSUE"
            StrSql += vbCrLf + "     FROM " & cnAdminDb & "..ORIRDETAIL AS IR "
            StrSql += vbCrLf + "     INNER JOIN " & cnAdminDb & "..ORMAST AS OM ON "
            StrSql += vbCrLf + "     	OM.SNO = IR.ORSNO AND OM.ORNO = IR.ORNO"
            StrSql += vbCrLf + "     	AND OM.ORNO = @ORNO AND OM.COSTID = @COSTID AND OM.COMPANYID = @COMPANYID"
            StrSql += vbCrLf + "     LEFT JOIN " & cnAdminDb & "..DESIGNER AS DE ON DE.DESIGNERID = IR.DESIGNERID"
            StrSql += vbCrLf + "     WHERE IR.ORSTATUS = 'I' AND ISNULL(IR.CANCEL,'') = ''"
            If rbtorder.Checked Then StrSql += vbCrLf + "AND  OM.ORTYPE='O'"
            If rbtRepair.Checked Then StrSql += vbCrLf + "AND  OM.ORTYPE='R'"
            If rbtnbooked.Checked Then StrSql += vbCrLf + "AND  OM.ORTYPE='B'"

            StrSql += vbCrLf + "     /** GETTTING ITEMTAG INFO **/"
            StrSql += vbCrLf + "     INSERT INTO TEMPTABLEDB..TEMPORSTATUS(COL1,COLHEAD)SELECT 'ISSUED DETAIL','T'"
            StrSql += vbCrLf + "     IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMPORISSUE)>0"
            StrSql += vbCrLf + "     BEGIN"
            StrSql += vbCrLf + "     INSERT INTO TEMPTABLEDB..TEMPORSTATUS(COL1,COL2,Col3,COL4,COL5,COL6,COL7,COL8,COL9,COL10,COL11,COL12,COL13,COL14,COL15,COL16,COL17,COLHEAD)"
            StrSql += vbCrLf + "     SELECT 'ITEM','SUBITEM','','PCS','GRSWT','NETWT','MCGRM','MC','WASTPER','WAST','STNWT','DIAPCS','DIAWT','DESIGNER','STATUS','TRANNO','TRANDATE','T1'"
            StrSql += vbCrLf + "     INSERT INTO TEMPTABLEDB..TEMPORSTATUS(COL1,COL2,COL3,COL4,COL5,COL6,COL7,COL8,COL9,COL10,COL11,COL12,COL13,COL14,COL15,COL16,COL17,COL18)"
            StrSql += vbCrLf + "     SELECT IM.ITEMNAME AS ITEM,SM.SUBITEMNAME AS SUBITEM,'' tagno,IR.PCS,IR.GRSWT,IR.NETWT,0,IR.MC,0,IR.WASTAGE,IR.STNWT,IR.DIAPCS,IR.DIAWT,IR.DESIGNERNAME,OS.ORDSTATE_NAME,IR.TRANNO,CONVERT(VARCHAR,IR.TRANDATE,103)"
            StrSql += vbCrLf + "     ,CASE WHEN ISNULL(PICTFILE,'') != '' THEN CASE WHEN ISNULL(IR.STYLENO,'') != '' THEN @CATALOGPATH ELSE @PICPATH END END + PICTFILE PIC"
            StrSql += vbCrLf + "     FROM TEMPTABLEDB..TEMPORISSUE AS IR"
            StrSql += vbCrLf + "     INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = IR.ITEMID"
            StrSql += vbCrLf + "     LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = IR.ITEMID AND SM.SUBITEMID = IR.SUBITEMID"
            StrSql += vbCrLf + "     LEFT JOIN " & cnAdminDb & "..ORDERSTATUS AS OS ON OS.ORDSTATE_ID = IR.ORDSTATE_ID"
            StrSql += vbCrLf + "     ORDER BY IR.ENTRYORDER"
            StrSql += vbCrLf + "     INSERT INTO TEMPTABLEDB..TEMPORSTATUS(COL1)SELECT ''"
            StrSql += vbCrLf + "     END"

            StrSql += vbCrLf + "     /** GETTTING SMITH RECEIPT INFO **/"
            StrSql += vbCrLf + "     SELECT DE.DESIGNERNAME"
            StrSql += vbCrLf + "     ,IR.SNO,IR.ORSNO,IR.TRANNO,IR.TRANDATE,IR.DESIGNERID"
            StrSql += vbCrLf + "     ,IR.PCS,IR.GRSWT,IR.NETWT"
            StrSql += vbCrLf + "     ,IT.MAXWASTPER AS WASTPER,IT.MAXWAST AS WASTAGE,IT.MAXMCGRM AS MCGRM,IT.MAXMC AS MC"
            StrSql += vbCrLf + "     ,IR.TAGNO,IR.ORSTATUS"
            StrSql += vbCrLf + "     ,IR.CANCEL,IR.COSTID,IR.DESCRIPT,IR.ORNO,IR.BATCHNO,IR.USERID,IR.UPDATED"
            StrSql += vbCrLf + "     ,IR.UPTIME,IR.APPVER,IR.COMPANYID,IR.TRANSFERED,IR.PROID"
            StrSql += vbCrLf + "     ,IR.ORDSTATE_ID,IR.ENTRYORDER,IR.CATCODE"
            StrSql += vbCrLf + "     ,OM.ITEMID,OM.SUBITEMID"
            StrSql += vbCrLf + "       ,OM.STYLENO,OM.REASON,OM.PICTFILE"
            'StrSql += vbCrLf + "     ,(SELECT CTLTEXT FROM "& cnadmindb &"..SOFTCONTROL WHERE ISNULL(CTLID,'') = 'PICPATH') PICPATH"
            'StrSql += vbCrLf + "     ,(SELECT CTLTEXT FROM "& cnadmindb &"..SOFTCONTROL WHERE ISNULL(CTLID,'') = 'TAGCATALOGPATH') CATALOGPATH"
            StrSql += vbCrLf + "     ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ORSTONE WHERE ORSNO=OM.SNO) AS STNPCS "
            StrSql += vbCrLf + "       ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ORSTONE WHERE ORSNO=OM.SNO) AS STNWT"
            StrSql += vbCrLf + "     ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ORSTONE WHERE ORSNO = OM.SNO) DIAPCS"
            StrSql += vbCrLf + "     ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ORSTONE WHERE ORSNO = OM.SNO) DIAWT"
            StrSql += vbCrLf + "     INTO TEMPTABLEDB..TEMPORRECEIPT"
            StrSql += vbCrLf + "     FROM " & cnAdminDb & "..ORIRDETAIL AS IR "
            StrSql += vbCrLf + "     INNER JOIN " & cnAdminDb & "..ORMAST AS OM ON "
            StrSql += vbCrLf + "     	OM.SNO = IR.ORSNO AND OM.ORNO = IR.ORNO"
            StrSql += vbCrLf + "     	AND OM.ORNO = @ORNO AND OM.COSTID = @COSTID AND OM.COMPANYID = @COMPANYID"
            StrSql += vbCrLf + "     LEFT  JOIN " & cnAdminDb & "..ITEMTAG AS IT ON IR.ORSNO = IT.ORSNO "
            StrSql += vbCrLf + "     LEFT JOIN " & cnAdminDb & "..DESIGNER AS DE ON DE.DESIGNERID = IR.DESIGNERID"
            StrSql += vbCrLf + "     WHERE IR.ORSTATUS = 'R' AND ISNULL(IR.CANCEL,'') = '' "
            If rbtorder.Checked Then StrSql += vbCrLf + "AND  OM.ORTYPE='O'"
            If rbtRepair.Checked Then StrSql += vbCrLf + "AND  OM.ORTYPE='R'"
            If rbtnbooked.Checked Then StrSql += vbCrLf + "AND  OM.ORTYPE='B'"

            StrSql += vbCrLf + "     INSERT INTO TEMPTABLEDB..TEMPORSTATUS(COL1,COLHEAD)SELECT 'RECEIVED DETAIL','T'"
            StrSql += vbCrLf + "     IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMPORRECEIPT)>0"
            StrSql += vbCrLf + "     	BEGIN"
            StrSql += vbCrLf + "     	INSERT INTO TEMPTABLEDB..TEMPORSTATUS(COL1,COL2,COL3,COL4,COL5,COL6,COL7,COL8,COL9,COL10,COL11,COL12,COL13,COL14,COL15,COL16,COL17,COLHEAD)"
            StrSql += vbCrLf + "     	SELECT 'ITEM','SUBITEM','TAGNO','PCS','GRSWT','NETWT','MCGRM','MC','WASTPER','WAST','STNWT','DAIPCS','DIAWT','DESIGNER','STATUS','TRANNO','TRANDATE','T1'"
            StrSql += vbCrLf + "     	INSERT INTO TEMPTABLEDB..TEMPORSTATUS(COL1,COL2,COL3,COL4,COL5,COL6,COL7,COL8,COL9,COL10,COL11,COL12,COL13,COL14,COL15,COL16,COL17,COL18) 	"
            StrSql += vbCrLf + "     	SELECT IM.ITEMNAME AS ITEM,SM.SUBITEMNAME AS SUBITEM,IR.TAGNO,IR.PCS,IR.GRSWT,IR.NETWT,IR.MCGRM,IR.MC,WASTPER,IR.WASTAGE,IR.STNWT,IR.DIAPCS,IR.DIAWT,IR.DESIGNERNAME,OS.ORDSTATE_NAME,IR.TRANNO,CONVERT(VARCHAR,IR.TRANDATE,103)"
            StrSql += vbCrLf + "     	,CASE WHEN ISNULL(PICTFILE,'') != '' THEN CASE WHEN ISNULL(IR.STYLENO,'') != '' THEN @CATALOGPATH ELSE @PICPATH END END + PICTFILE PIC"
            StrSql += vbCrLf + "     	FROM TEMPTABLEDB..TEMPORRECEIPT AS IR"
            StrSql += vbCrLf + "     	INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = IR.ITEMID"
            StrSql += vbCrLf + "     	LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = IR.ITEMID AND SM.SUBITEMID = IR.SUBITEMID"
            StrSql += vbCrLf + "     	LEFT JOIN " & cnAdminDb & "..ORDERSTATUS AS OS ON OS.ORDSTATE_ID = IR.ORDSTATE_ID"
            StrSql += vbCrLf + "     	ORDER BY IR.ENTRYORDER"
            StrSql += vbCrLf + "     	INSERT INTO TEMPTABLEDB..TEMPORSTATUS(COL1)SELECT ''"
            StrSql += vbCrLf + "     	END"
        Else
            StrSql += vbCrLf + "     /** GETTTING SMITH ISSUE/RECEIPT INFO **/"
            StrSql += vbCrLf + "     SELECT AC.ACNAME DESIGNERNAME,IR.*,OM.ITEMID,OM.SUBITEMID"
            StrSql += vbCrLf + "     ,OM.STYLENO,OM.REASON,OM.PICTFILE"
            'StrSql += vbCrLf + "    ,(SELECT CTLTEXT FROM "& cnadmindb &"..SOFTCONTROL WHERE ISNULL(CTLID,'') = 'PICPATH') PICPATH"
            'StrSql += vbCrLf + "    ,(SELECT CTLTEXT FROM "& cnadmindb &"..SOFTCONTROL WHERE ISNULL(CTLID,'') = 'TAGCATALOGPATH') CATALOGPATH"
            'StrSql += vbCrLf + "     ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ORSTONE WHERE ORSNO=OM.SNO) AS STNPCS "
            'StrSql += vbCrLf + "     ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ORSTONE WHERE ORSNO=OM.SNO) AS STNWT "
            'StrSql += vbCrLf + "     ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ORSTONE WHERE ORSNO = OM.SNO) DIAPCS"
            'StrSql += vbCrLf + "     ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ORSTONE WHERE ORSNO = OM.SNO) DIAWT"
            StrSql += vbCrLf + "    ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ORIRDETAILSTONE WHERE ORSNO=IR.SNO) AS STNPCS"
            StrSql += vbCrLf + "    ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ORIRDETAILSTONE WHERE ORSNO=IR.SNO) AS STNWT"
            StrSql += vbCrLf + "    ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ORIRDETAILSTONE WHERE ORSNO = IR.SNO) DIAPCS"
            StrSql += vbCrLf + "    ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ORIRDETAILSTONE WHERE ORSNO = IR.SNO) DIAWT"
            StrSql += vbCrLf + "     ,CT.CATNAME "
            StrSql += vbCrLf + "     INTO TEMPTABLEDB..TEMPORISSUE"
            StrSql += vbCrLf + "     FROM " & cnAdminDb & "..ORIRDETAIL AS IR "
            StrSql += vbCrLf + "     INNER JOIN " & cnAdminDb & "..ACHEAD AS AC ON AC.ACCODE = IR.ACCODE"
            StrSql += vbCrLf + "     INNER JOIN " & cnAdminDb & "..ORMAST AS OM ON "
            StrSql += vbCrLf + "     	OM.SNO = IR.ORSNO AND OM.ORNO = IR.ORNO"
            StrSql += vbCrLf + "     	AND OM.ORNO = @ORNO AND OM.COSTID = @COSTID AND OM.COMPANYID = @COMPANYID"
            StrSql += vbCrLf + "     LEFT JOIN " & cnAdminDb & "..DESIGNER AS DE ON DE.DESIGNERID = IR.DESIGNERID"
            StrSql += vbCrLf + "     LEFT JOIN " & cnAdminDb & "..CATEGORY AS CT ON CT.CATCODE = IR.CATCODE"
            StrSql += vbCrLf + "     WHERE ISNULL(IR.CANCEL,'') = ''"
            If rbtorder.Checked Then StrSql += vbCrLf + "AND  OM.ORTYPE='O'"
            If rbtRepair.Checked Then StrSql += vbCrLf + "AND  OM.ORTYPE='R'"
            If rbtnbooked.Checked Then StrSql += vbCrLf + "AND  OM.ORTYPE='B'"

            StrSql += vbCrLf + "     INSERT INTO TEMPTABLEDB..TEMPORSTATUS(COL1,COLHEAD)SELECT 'ISSUE / RECEIPT DETAIL','T'"
            StrSql += vbCrLf + "     IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMPORISSUE)>0"
            StrSql += vbCrLf + "     BEGIN"
            StrSql += vbCrLf + "     INSERT INTO TEMPTABLEDB..TEMPORSTATUS(COL1,COL2,Col3,COL4,COL5,COL6,COL7,COL8,COL9,COL10,COL11,COL12,COL13,COL14,COL15,COL16,COL17,COL18,COLHEAD)"
            StrSql += vbCrLf + "     SELECT 'ITEM','SUBITEM','','PCS','GRSWT','NETWT','MCGRM','MC','WASTPER','WAST','STNWT','DIAPCS','DIAWT','DESIGNER','STATUS','TRANNO','TRANDATE','CATEGORY','T1'"
            StrSql += vbCrLf + "     INSERT INTO TEMPTABLEDB..TEMPORSTATUS(COL1,COL2,COL3,COL4,COL5,COL6,COL7,COL8,COL9,COL10,COL11,COL12,COL13,COL14,COL15,COL16,COL17,COL18)"
            StrSql += vbCrLf + "     SELECT IM.ITEMNAME AS ITEM,SM.SUBITEMNAME AS SUBITEM,'' tagno,IR.PCS,IR.GRSWT,IR.NETWT,0,IR.MC,0,IR.WASTAGE,IR.STNWT,IR.DIAPCS,IR.DIAWT,IR.DESIGNERNAME,OS.ORDSTATE_NAME,IR.TRANNO,CONVERT(VARCHAR,IR.TRANDATE,103)"
            StrSql += vbCrLf + "     ,CT.CATNAME"
            StrSql += vbCrLf + "     FROM TEMPTABLEDB..TEMPORISSUE AS IR"
            StrSql += vbCrLf + "     INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = IR.ITEMID"
            StrSql += vbCrLf + "     LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = IR.ITEMID AND SM.SUBITEMID = IR.SUBITEMID"
            StrSql += vbCrLf + "     LEFT JOIN " & cnAdminDb & "..ORDERSTATUS AS OS ON OS.ORDSTATE_ID = IR.ORDSTATE_ID"
            StrSql += vbCrLf + "     LEFT JOIN " & cnAdminDb & "..CATEGORY AS CT ON CT.CATCODE= IR.CATCODE"
            StrSql += vbCrLf + "     ORDER BY IR.ENTRYORDER"
            StrSql += vbCrLf + "     INSERT INTO TEMPTABLEDB..TEMPORSTATUS(COL1)SELECT ''"
            StrSql += vbCrLf + "     END"
            If ORDER_MULTI_MIMR = False Then
                StrSql += vbCrLf + "     /** GETTTING ITEMTAG INFO **/"
                StrSql += vbCrLf + "     INSERT INTO TEMPTABLEDB..TEMPORSTATUS(COL1,COLHEAD)SELECT 'ISSUED DETAIL','T'"
                StrSql += vbCrLf + "     IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMPORISSUE)>0"
                StrSql += vbCrLf + "     BEGIN"
                StrSql += vbCrLf + "     INSERT INTO TEMPTABLEDB..TEMPORSTATUS(COL1,COL2,Col3,COL4,COL5,COL6,COL7,COL8,COL9,COL10,COL11,COL12,COL13,COL14,COL15,COL16,COL17,COLHEAD)"
                StrSql += vbCrLf + "     SELECT 'ITEM','SUBITEM','','PCS','GRSWT','NETWT','MCGRM','MC','WASTPER','WAST','STNWT','DIAPCS','DIAWT','DESIGNER','STATUS','TRANNO','TRANDATE','T1'"
                StrSql += vbCrLf + "     INSERT INTO TEMPTABLEDB..TEMPORSTATUS(COL1,COL2,COL3,COL4,COL5,COL6,COL7,COL8,COL9,COL10,COL11,COL12,COL13,COL14,COL15,COL16,COL17,COL18)"
                StrSql += vbCrLf + "     SELECT IM.ITEMNAME AS ITEM,SM.SUBITEMNAME AS SUBITEM,'' tagno,IR.PCS,IR.GRSWT,IR.NETWT,0,IR.MC,0,IR.WASTAGE,IR.STNWT,IR.DIAPCS,IR.DIAWT,IR.DESIGNERNAME,OS.ORDSTATE_NAME,IR.TRANNO,CONVERT(VARCHAR,IR.TRANDATE,103)"
                StrSql += vbCrLf + "     ,CASE WHEN ISNULL(PICTFILE,'') != '' THEN CASE WHEN ISNULL(IR.STYLENO,'') != '' THEN @CATALOGPATH ELSE @PICPATH END END + PICTFILE PIC"
                StrSql += vbCrLf + "     FROM TEMPTABLEDB..TEMPORISSUE AS IR"
                StrSql += vbCrLf + "     INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = IR.ITEMID"
                StrSql += vbCrLf + "     LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = IR.ITEMID AND SM.SUBITEMID = IR.SUBITEMID"
                StrSql += vbCrLf + "     LEFT JOIN " & cnAdminDb & "..ORDERSTATUS AS OS ON OS.ORDSTATE_ID = IR.ORDSTATE_ID"
                StrSql += vbCrLf + "     ORDER BY IR.ENTRYORDER"
                StrSql += vbCrLf + "     INSERT INTO TEMPTABLEDB..TEMPORSTATUS(COL1)SELECT ''"
                StrSql += vbCrLf + "     END"
            End If
        End If
        

        StrSql += vbCrLf + "     /** DELIVERED INFO **/"
        StrSql += vbCrLf + "     SELECT "
        StrSql += vbCrLf + "     IM.ITEMNAME AS ITEM,SM.SUBITEMNAME AS SUBITEM"
        StrSql += vbCrLf + "     ,ISS.TAGNO,ISS.PCS,ISS.GRSWT,ISS.NETWT,ISS.TRANNO,CONVERT(VARCHAR,ISS.TRANDATE,103)TRANDATE,'DELIVERED' AS STATUS"
        StrSql += vbCrLf + "      ,OM.STYLENO,OM.PICTFILE"
        'StrSql += vbCrLf + "     ,(SELECT CTLTEXT FROM "& cnadmindb &"..SOFTCONTROL WHERE ISNULL(CTLID,'') = 'PICPATH') PICPATH"
        'StrSql += vbCrLf + "     ,(SELECT CTLTEXT FROM "& cnadmindb &"..SOFTCONTROL WHERE ISNULL(CTLID,'') = 'TAGCATALOGPATH') CATALOGPATH"
        StrSql += vbCrLf + "     ,(SELECT SUM(STNPCS) FROM " & cnadmindb & "..ORSTONE WHERE ORSNO=OM.SNO) AS STNPCS,(SELECT SUM(STNWT) FROM " & cnadmindb & "..ORSTONE WHERE ORSNO=OM.SNO) AS STNWT"
        StrSql += vbCrLf + "     ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ORSTONE WHERE ORSNO = OM.SNO) DIAPCS"
        StrSql += vbCrLf + "     ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ORSTONE WHERE ORSNO = OM.SNO) DIAWT"
        StrSql += vbCrLf + "     INTO TEMPTABLEDB..TEMPORDELIVER"
        StrSql += vbCrLf + "     FROM " & cnadmindb & "..ORMAST AS OM"
        StrSql += vbCrLf + "     INNER JOIN " & cnStockDb & "..ISSUE AS ISS ON ISS.SNO = OM.ODSNO"
        StrSql += vbCrLf + "     INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = ISS.ITEMID"
        StrSql += vbCrLf + "     LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = ISS.ITEMID AND SM.SUBITEMID = ISS.SUBITEMID"
        StrSql += vbCrLf + "     WHERE OM.ORNO = @ORNO AND OM.COSTID = @COSTID AND OM.COMPANYID = @COMPANYID"
        StrSql += vbCrLf + "     AND ISNULL(OM.ODBATCHNO,'') <> '' AND ISNULL(OM.ODSNO,'') <> ''  AND ISNULL(TRANTYPE,'')<>'AI' "
        If rbtorder.Checked Then StrSql += vbCrLf + "AND  OM.ORTYPE='O'"
        If rbtRepair.Checked Then StrSql += vbCrLf + "AND  OM.ORTYPE='R'"
        If rbtnbooked.Checked Then StrSql += vbCrLf + "AND  OM.ORTYPE='B'"

        StrSql += vbCrLf + "     INSERT INTO TEMPTABLEDB..TEMPORSTATUS(COL1,COLHEAD)SELECT 'DELIVERED DETAIL','T'"

        StrSql += vbCrLf + "     IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMPORDELIVER)>0"
        StrSql += vbCrLf + "     	BEGIN"
        StrSql += vbCrLf + "     	INSERT INTO TEMPTABLEDB..TEMPORSTATUS(COL1,COL2,COL3,COL4,COL5,COL6,COL7,COL8,COL9,COL10,COL11,COL12,COL13,COL14,COL15,COL16,COL17,COLHEAD)"
        StrSql += vbCrLf + "     	SELECT 'ITEM','SUBITEM','TAGNO','PCS','GRSWT','NETWT','MCGRM','MC','WASTPER','WAST','STNWT','DIAPCS','DIAWT','','STATUS','TRANNO','TRANDATE','T1'"
        StrSql += vbCrLf + "     	INSERT INTO TEMPTABLEDB..TEMPORSTATUS(COL1,COL2,COL3,COL4,COL5,COL6,COL7,COL8,COL9,COL10,COL11,COL12,COL13,COL14,COL15,COL16,COL17,COL18)"
        StrSql += vbCrLf + "     	SELECT ITEM,SUBITEM,TAGNO,PCS,GRSWT,NETWT,0,0,0,0,0,DIAPCS,DIAWT,'',STATUS,TRANNO,TRANDATE"
        StrSql += vbCrLf + "     	,CASE WHEN ISNULL(PICTFILE,'') != '' THEN CASE WHEN ISNULL(STYLENO,'') != '' THEN @CATALOGPATH ELSE @PICPATH END END + PICTFILE PIC"
        StrSql += vbCrLf + "     	FROM TEMPTABLEDB..TEMPORDELIVER"
        StrSql += vbCrLf + "     	INSERT INTO TEMPTABLEDB..TEMPORSTATUS(COL1)SELECT ''"
        StrSql += vbCrLf + "     	END"
        StrSql += vbCrLf + "     END"

        StrSql += vbCrLf + "      IF  (SELECT COUNT(*) FROM TEMPTABLEDB..TEMPORSTATUS)>0"
        StrSql += vbCrLf + "     BEGIN"
        StrSql += vbCrLf + "     /** COLLECTION DETAIL **/"
        StrSql += vbCrLf + "      IF (SELECT COUNT(*) FROM " & cnStockDb & "..ACCTRAN T WHERE BATCHNO IN (SELECT DISTINCT BATCHNO FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO = @ORNO AND COSTID = @COSTID AND RECPAY='R')"
        StrSql += vbCrLf + "         			AND COSTID = @COSTID AND COMPANYID = @COMPANYID AND ISNULL(CANCEL,'') = '' "
        StrSql += vbCrLf + "      ) > 0"
        StrSql += vbCrLf + "      BEGIN"
        StrSql += vbCrLf + "         SELECT * "
        StrSql += vbCrLf + "     	INTO TEMPTABLEDB..TEMPORCOLLECTION"
        StrSql += vbCrLf + "         FROM"
        StrSql += vbCrLf + "         	("
        StrSql += vbCrLf + "         	SELECT TRANNO,TRANDATE,PAYMODE,CASE WHEN RECEIPTAMT <> 0 THEN RECEIPTAMT ELSE NULL END AS RECEIPTAMT"
        StrSql += vbCrLf + "         	,CASE WHEN PAYMENTAMT <> 0 THEN PAYMENTAMT ELSE NULL END PAYMENTAMT,NULL RECEIPTWT,NULL PAYMENTWT"
        StrSql += vbCrLf + "         	FROM"
        StrSql += vbCrLf + "         	("
        StrSql += vbCrLf + "         		SELECT "
        StrSql += vbCrLf + "         		TRANNO,CONVERT(VARCHAR,TRANDATE,103)TRANDATE,'AVD-ADJ' AS PAYMODE" 'ADJ&WT-CONV
        StrSql += vbCrLf + "         		,CASE WHEN SUM(AMOUNT) >= 0 THEN SUM(AMOUNT) ELSE 0 END AS RECEIPTAMT"
        StrSql += vbCrLf + "         		,CASE WHEN SUM(AMOUNT) < 0 THEN SUM(AMOUNT) ELSE 0 END AS PAYMENTAMT"
        StrSql += vbCrLf + "         		FROM"
        StrSql += vbCrLf + "         			("
        StrSql += vbCrLf + "         			SELECT "
        StrSql += vbCrLf + "         			TRANNO,TRANDATE,PAYMODE"
        StrSql += vbCrLf + "         			,SUM(CASE WHEN TRANMODE = 'C' AND GRSWT = 0 THEN AMOUNT WHEN TRANMODE = 'D' AND GRSWT = 0  THEN -1*AMOUNT ELSE 0 END)AS AMOUNT"
        StrSql += vbCrLf + "       		        ,' 'TRANS"
        StrSql += vbCrLf + "         			FROM " & cnStockDb & "..ACCTRAN T WHERE BATCHNO IN (SELECT DISTINCT BATCHNO FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO = @ORNO)"
        StrSql += vbCrLf + "         			AND PAYMODE IN ('CA','CH','CC','CT','OR','SV')"
        StrSql += vbCrLf + "         			AND COMPANYID = @COMPANYID AND ISNULL(CANCEL,'') = '' AND COSTID = @COSTID"
        StrSql += vbCrLf + "         			AND FROMFLAG IN ('D','R')"
        StrSql += vbCrLf + "         			GROUP BY PAYMODE,TRANNO,TRANDATE,BATCHNO"
        StrSql += vbCrLf + "         			)X"
        StrSql += vbCrLf + "         			GROUP BY TRANNO,TRANDATE"
        StrSql += vbCrLf + "         		UNION ALL"
        StrSql += vbCrLf + "         		SELECT "
        StrSql += vbCrLf + "         		TRANNO,CONVERT(VARCHAR,TRANDATE,103)TRANDATE,CASE PAYMODE WHEN 'CA' THEN 'CASH' WHEN 'CH' THEN 'CHEQUE' WHEN 'CC' THEN 'CREDIT CARD' END PAYMODE"
        StrSql += vbCrLf + "         		,SUM(CASE WHEN TRANMODE = 'D' AND GRSWT = 0 THEN AMOUNT ELSE 0 END)AS RECIPTAMT"
        StrSql += vbCrLf + "         		,SUM(CASE WHEN TRANMODE = 'C' AND GRSWT = 0 THEN AMOUNT ELSE 0 END)AS PAYMENTAMT"
        StrSql += vbCrLf + "         		FROM " & cnStockDb & "..ACCTRAN AS T WHERE BATCHNO IN (SELECT DISTINCT BATCHNO FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO = @ORNO)"
        StrSql += vbCrLf + "         		AND PAYMODE IN ('CA','CH','CC','CT')"
        StrSql += vbCrLf + "         		AND COMPANYID = @COMPANYID  AND ISNULL(CANCEL,'') = ''  AND COSTID = @COSTID"
        StrSql += vbCrLf + "       		AND FROMFLAG IN ('D','R')"
        StrSql += vbCrLf + "         		GROUP BY PAYMODE,TRANNO,TRANDATE,BATCHNO"
        StrSql += vbCrLf + "         		UNION ALL"
        StrSql += vbCrLf + "       	    SELECT TRANNO,CONVERT(VARCHAR,TRANDATE,103)TRANDATE,'TRANSFER' PAYMODE"
        StrSql += vbCrLf + "       	    ,SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE 0 END) AS RECEIPTAMT"
        StrSql += vbCrLf + "       	    ,SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE 0 END) AS PAYMENTAMT"
        StrSql += vbCrLf + "       	    FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO = @ORNO "
        StrSql += vbCrLf + "       	    AND COMPANYID = @COMPANYID AND GRSWT = 0 AND NETWT = 0 "
        StrSql += vbCrLf + "       	    AND FROMFLAG IN ('T')  "
        StrSql += vbCrLf + "       	    AND ISNULL(CANCEL,'') = ''  AND COSTID = @COSTID"
        StrSql += vbCrLf + "       	    GROUP BY TRANNO,TRANDATE"
        StrSql += vbCrLf + "         		UNION ALL"
        StrSql += vbCrLf + "       	    SELECT TRANNO,CONVERT(VARCHAR,TRANDATE,103)TRANDATE,'ADVANCE' PAYMODE"
        StrSql += vbCrLf + "       	    ,SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE 0 END) AS RECEIPTAMT"
        StrSql += vbCrLf + "       	    ,SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE 0 END) AS PAYMENTAMT"
        StrSql += vbCrLf + "       	    FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO = @ORNO "
        StrSql += vbCrLf + "       	    AND COMPANYID = @COMPANYID AND GRSWT = 0 AND NETWT = 0 "
        StrSql += vbCrLf + "       	    AND FROMFLAG IN ('F','P')  "
        StrSql += vbCrLf + "       	    AND ISNULL(CANCEL,'') = ''  AND COSTID = @COSTID"
        StrSql += vbCrLf + "       	    GROUP BY TRANNO,TRANDATE"
        StrSql += vbCrLf + "         	)Y"
        StrSql += vbCrLf + "         	UNION ALL"
        StrSql += vbCrLf + "         	SELECT TRANNO,CONVERT(VARCHAR,TRANDATE,103)TRANDATE,NULL PAYMODE,NULL RECEIPTAMT,NULL PAYMENTAMT,SUM(CASE WHEN RECPAY = 'R' AND GRSWT <> 0 THEN GRSWT ELSE 0 END) AS RECEIPTWT"
        StrSql += vbCrLf + "         	,SUM(CASE WHEN RECPAY = 'P' AND GRSWT <> 0 THEN GRSWT ELSE 0 END) AS PAYMENTWT"
        StrSql += vbCrLf + "         	FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO = @ORNO AND COMPANYID = @COMPANYID AND GRSWT <> 0  AND ISNULL(CANCEL,'') = ''  AND COSTID = @COSTID"
        StrSql += vbCrLf + "         	GROUP BY TRANNO,TRANDATE"
        StrSql += vbCrLf + "         	)Z"
        StrSql += vbCrLf + "         	WHERE NOT(ISNULL(RECEIPTAMT,0) = 0 AND ISNULL(PAYMENTAMT,0) = 0 AND ISNULL(RECEIPTWT,0) = 0 AND ISNULL(PAYMENTWT,0) = 0)"
        StrSql += vbCrLf + "      END"
        StrSql += vbCrLf + "      ELSE"
        StrSql += vbCrLf + "      BEGIN"
        StrSql += vbCrLf + "         	SELECT TRANNO,TRANDATE,PAYMODE,CASE WHEN RECEIPTAMT <> 0 THEN RECEIPTAMT ELSE NULL END AS RECEIPTAMT"
        StrSql += vbCrLf + "         	,CASE WHEN PAYMENTAMT <> 0 THEN PAYMENTAMT ELSE NULL END PAYMENTAMT,NULL RECEIPTWT,NULL PAYMENTWT"
        StrSql += vbCrLf + "             INTO TEMPTABLEDB..TEMPORCOLLECTION"
        StrSql += vbCrLf + "             FROM("
        StrSql += vbCrLf + "       	    SELECT TRANNO,CONVERT(VARCHAR,TRANDATE,103)TRANDATE,'ADVANCE' PAYMODE"
        StrSql += vbCrLf + "       	    ,SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE 0 END) AS RECEIPTAMT"
        StrSql += vbCrLf + "       	    ,SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE 0 END) AS PAYMENTAMT"
        StrSql += vbCrLf + "       	    FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO = @ORNO AND COMPANYID = @COMPANYID AND GRSWT = 0 AND NETWT = 0 AND ISNULL(CANCEL,'') = ''  AND COSTID = @COSTID"
        StrSql += vbCrLf + "       	    GROUP BY TRANNO,TRANDATE"
        StrSql += vbCrLf + "             )Y"
        StrSql += vbCrLf + "         	UNION ALL"
        StrSql += vbCrLf + "         	SELECT TRANNO,CONVERT(VARCHAR,TRANDATE,103)TRANDATE,NULL PAYMODE,NULL RECEIPTAMT,NULL PAYMENTAMT,SUM(CASE WHEN RECPAY = 'R' AND GRSWT <> 0 THEN GRSWT ELSE 0 END) AS RECEIPTWT"
        StrSql += vbCrLf + "         	,SUM(CASE WHEN RECPAY = 'P' AND GRSWT <> 0 THEN GRSWT ELSE 0 END) AS PAYMENTWT"
        StrSql += vbCrLf + "         	FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO = @ORNO AND COMPANYID = @COMPANYID AND GRSWT <> 0  AND ISNULL(CANCEL,'') = ''  AND COSTID = @COSTID"
        StrSql += vbCrLf + "         	GROUP BY TRANNO,TRANDATE"
        StrSql += vbCrLf + "     END"

        StrSql += vbCrLf + "     IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMPORCOLLECTION)>0"
        StrSql += vbCrLf + "     BEGIN"
        StrSql += vbCrLf + "     INSERT INTO TEMPTABLEDB..TEMPORSTATUS(COL1,COLHEAD)SELECT 'COLLECTION DETAIL','T'"
        StrSql += vbCrLf + "     INSERT INTO TEMPTABLEDB..TEMPORSTATUS(COL1,COL2,COL3,COL4,COL5,COL6,COL7,COLHEAD)"
        StrSql += vbCrLf + "     SELECT 'TRANNO','TRANDATE','REC-AMT','PAY-AMT','REC-WT','PAY-WT','PAYMODE','T1'"
        StrSql += vbCrLf + "     INSERT INTO TEMPTABLEDB..TEMPORSTATUS(COL1,COL2,COL3,COL4,COL5,COL6,COL7)"
        StrSql += vbCrLf + "     SELECT TRANNO,CONVERT(VARCHAR,TRANDATE,103),RECEIPTAMT,PAYMENTAMT,RECEIPTWT,PAYMENTWT,PAYMODE"
        StrSql += vbCrLf + "     FROM TEMPTABLEDB..TEMPORCOLLECTION"
        StrSql += vbCrLf + "     INSERT INTO TEMPTABLEDB..TEMPORSTATUS(COL1,COL2,COL3,COL4,COL5,COL6,COLHEAD)"
        StrSql += vbCrLf + "     SELECT 'TOTAL' AS TRANNO,NULL,SUM(RECEIPTAMT),SUM(PAYMENTAMT),SUM(RECEIPTWT),SUM(PAYMENTWT),'S1'"
        StrSql += vbCrLf + "     FROM TEMPTABLEDB..TEMPORCOLLECTION"
        'StrSql += vbCrLf + "     INSERT INTO TEMPTABLEDB..TEMPORSTATUS(COL1,COL2,COL3,COLHEAD)"
        'StrSql += vbCrLf + "     SELECT 'REMARKS',CONVERT(VARCHAR(12),TRANDATE,103),QUERY,'S2' FROM " & cnAdminDb & "..CUSTOMERQUERY WHERE BATCHNO IN (SELECT  TOP 1 BATCHNO FROM  " & cnAdminDb & "..ORMAST WHERE ORNO=@ORNO AND ISNULL(CANCEL,'')='') "
        StrSql += vbCrLf + "     END"
        StrSql += vbCrLf + "     END"
        StrSql += vbCrLf + "     /** COLLECTION DETAIL **/"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = vbCrLf + " SELECT * FROM TEMPTABLEDB..TEMPORSTATUS"
        'If rbtorder.Checked Then StrSql += vbCrLf + " where ORTYPE='O' "
        'If rbtRepair.Checked Then StrSql += vbCrLf + " where ORTYPE='R' "
        StrSql += vbCrLf + " ORDER BY KEYNO"
        Dim dtGrid As New DataTable
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dtGrid)
        dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Function
        End If

        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.AllowUserToResizeColumns = True
        objGridShower.gridView.AllowUserToResizeRows = True
        objGridShower.gridView.AutoResizeRows()
        'objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        AddHandler objGridShower.gridView.KeyDown, AddressOf Handler_Keydown

        objGridShower.Text = "ORDER REPAIR STATUS"
        Dim tit As String = "ORDER REPAIR STATUS FOR " & txtOrdRepNo.Text & "" + vbCrLf & IIf(cmbCostcentre.Text <> "" And cmbCostcentre.Text <> "ALL", " :" & cmbCostcentre.Text, "")
        objGridShower.lblTitle.Text = tit
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(dtGrid)
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        objGridShower.FormReSize = False
        objGridShower.FormReLocation = False
        objGridShower.pnlFooter.Visible = False
        objGridShower.WindowState = FormWindowState.Maximized
        objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        DataGridView_SummaryFormatting(objGridShower.gridView)
        FormatGridColumns(objGridShower.gridView, False, False, , False)
        objGridShower.gridView.Columns("COL13").Visible = True

        For Each col As DataGridViewColumn In objGridShower.gridView.Columns
            col.SortMode = DataGridViewColumnSortMode.NotSortable
        Next


        objGridShower.Show()
        If chkImage.Checked = True Then
            objGridShower.gridView.Columns("COLIMAGE").Visible = True
            Dim fileDestPath As String = Nothing
            Dim picBox As New PictureBox
            picBox.Size = New Size(100, 100)
            Dim cel As DataGridViewImageCell
            For Each ro As DataGridViewRow In objGridShower.gridView.Rows
                cel = New DataGridViewImageCell
                cel = ro.Cells("COLIMAGE")
                If ro.Cells("COL18").Value.ToString = "" Then 'COL13->COL17 OR COL18
                    ro.Cells("COLIMAGE").Value = My.Resources.EmptyImage
                    Continue For
                End If
                fileDestPath = ro.Cells("COL18").Value.ToString 'COL13->COL17 OR COL18
                If IO.File.Exists(fileDestPath) Then
                    'objGridShower.gridView.Rows(ro.Index).Height = 250
                    AutoImageSizer(fileDestPath, picBox, PictureBoxSizeMode.CenterImage)
                    ro.Cells("COLIMAGE").Value = picBox.Image
                    ro.Height = 100
                Else
                    ro.Cells("COLIMAGE").Value = My.Resources.EmptyImage
                End If

            Next
        Else
            objGridShower.gridView.Columns("COLIMAGE").Visible = False
        End If
        objGridShower.FormReSize = True
        objGridShower.FormReLocation = True
        objGridShower.gridViewHeader.Visible = False
        objGridShower.WindowState = FormWindowState.Maximized
        FillGridGroupStyle_KeyNoWise(objGridShower.gridView, "COL1")
        '592
        For i As Integer = 0 To objGridShower.gridView.Rows.Count - 1
            With objGridShower.gridView.Rows(i)
                Select Case .Cells("COL15").Value.ToString
                    Case "DELIVERED"
                        .Cells("COL15").Style.BackColor = Color.LightGreen
                        .Cells("COL15").Style.ForeColor = Color.Red
                        .Cells("COL15").Style.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    Case "PENDING WITH US"
                        .Cells("COL15").Style.BackColor = Color.SandyBrown
                        .Cells("COL15").Style.ForeColor = Color.Black
                        .Cells("COL15").Style.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    Case "CANCELLED"
                        .Cells("COL15").Style.BackColor = Color.Red
                        .Cells("COL15").Style.ForeColor = Color.White
                        .Cells("COL15").Style.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    Case "TRANSFERED"
                        .Cells("COL15").Style.BackColor = Color.LightGreen
                        .Cells("COL15").Style.ForeColor = Color.Red
                        .Cells("COL15").Style.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    Case "ADVANCE REPAY"
                        .Cells("COL15").Style.BackColor = Color.Aquamarine
                        .Cells("COL15").Style.ForeColor = Color.Brown
                        .Cells("COL15").Style.Font = New Font("VERDANA", 8, FontStyle.Bold)
                End Select
                If .Cells("COLHEAD").Value.ToString = "T5" Then
                    .Cells("COL15").Style.BackColor = Color.LightGreen
                    .Cells("COL15").Style.ForeColor = Color.Red
                    .Cells("COL15").Style.Font = New Font("VERDANA", 8, FontStyle.Bold)
                End If
            End With

        Next
        '592
        objGridShower.gridView_ColumnWidthChanged(Me, New DataGridViewColumnEventArgs(objGridShower.gridView.Columns("COL1")))
        Prop_Sets()
    End Function
    Private Sub Handler_Keydown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = Keys.F Then
            If txtOrdRepNo.Text <> "" Then
                StrSql = " SELECT CONVERT(VARCHAR(12),TRANDATE,103) REPORT_DATE,(SELECT EMPNAME FROM NAJADMINDB..EMPMASTER WHERE EMPID=C.EMPID) EMPNAME,QUERY FROM " & cnAdminDb & "..CUSTOMERQUERY AS C WHERE BATCHNO IN (SELECT  TOP 1 BATCHNO FROM  " & cnAdminDb & "..ORMAST WHERE ORNO='" & GetCostId(CostId) & GetCompanyId(strCompanyId) & txtOrdRepNo.Text & "' AND ISNULL(CANCEL,'')='') "
                BrighttechPack.SearchDialog.Show_R("Find Customer Query ", StrSql, cn, , , , , False, , , False)
            End If
        End If
    End Sub
    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        'If Not CheckBckDays(userId, Me.Name, dtp.Value) Then dtpFrom.Focus() : Exit Sub
        'Dim checkstr As String
        'checkstr = txtOrdRepNo.Text.(substring, 0, 1)


        If cmbCostcentre.Text <> "" Then
            CostId = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostcentre.Text & "'")
        End If
        If rbtReqdetail.Checked Then
            ReqDetail()
            Exit Sub
        End If

        StrSql = vbCrLf + "IF exists(SELECT * FROM  TEMPTABLEDB..SYSOBJECTS WHERE xtype = 'U' AND NAME ='TEMPORSTATUS') DROP TABLE  TEMPTABLEDB..TEMPORSTATUS                         "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
        StrSql = vbCrLf + "IF exists(SELECT * FROM  TEMPTABLEDB..SYSOBJECTS WHERE xtype = 'U' AND NAME ='TEMPORBOOK') DROP TABLE  TEMPTABLEDB..TEMPORBOOK                         "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
        StrSql = vbCrLf + "IF exists(SELECT * FROM  TEMPTABLEDB..SYSOBJECTS WHERE xtype = 'U' AND NAME ='TEMPORISSUE') DROP TABLE  TEMPTABLEDB..TEMPORISSUE                         "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
        StrSql = vbCrLf + "IF exists(SELECT * FROM  TEMPTABLEDB..SYSOBJECTS WHERE xtype = 'U' AND NAME ='TEMPORSAMPLE') DROP TABLE  TEMPTABLEDB..TEMPORSAMPLE                         "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
        StrSql = vbCrLf + "IF exists(SELECT * FROM  TEMPTABLEDB..SYSOBJECTS WHERE xtype = 'U' AND NAME ='TEMPORRECEIPT') DROP TABLE  TEMPTABLEDB..TEMPORRECEIPT                         "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
        StrSql = vbCrLf + "IF exists(SELECT * FROM  TEMPTABLEDB..SYSOBJECTS WHERE xtype = 'U' AND NAME ='TEMPORDELIVER') DROP TABLE  TEMPTABLEDB..TEMPORDELIVER                         "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
        StrSql = vbCrLf + "IF exists(SELECT * FROM  TEMPTABLEDB..SYSOBJECTS WHERE xtype = 'U' AND NAME ='TEMPORCOLLECTION') DROP TABLE  TEMPTABLEDB..TEMPORCOLLECTION                         "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        NewSearch()
        Exit Sub


    End Sub
    Private Sub DataGridView_SummaryFormatting(ByVal Dgv As DataGridView)
        Dgv.Columns("COLHEAD").Visible = False
        Dgv.Columns("KEYNO").Visible = False
        For cnt As Integer = 0 To Dgv.Columns.Count - 1
            Dgv.Columns(cnt).HeaderText = ""
        Next
        '     Dgv.Columns("MCGRM").HeaderText = "MC/GM"
        '    Dgv.Columns("WASTPER").HeaderText = "W%"
        '   Dgv.Columns("WAST").HeaderText = "W/GM"

        Dgv.ColumnHeadersVisible = False
        Dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub


    Private Sub txtOrdRepNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOrdRepNo.GotFocus
        If txtOrdRepNo.Text = "" Then
            txtOrdRepNo.Text = "O" & Mid((Today.Year + 1).ToString, 3, 2)
        End If
    End Sub

    Private Sub txtOrdRepNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtOrdRepNo.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            StrSql = " SELECT DISTINCT COSTID FROM " & cnAdminDb & "..ORMAST WHERE SUBSTRING(ORNO,6,20) = '" & txtOrdRepNo.Text & "'"
            StrSql += " AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostcentre.Text & "'),'')"
            If rbtorder.Checked Then StrSql += vbCrLf + "AND  ORTYPE='O'"
            If rbtRepair.Checked Then StrSql += vbCrLf + "AND  ORTYPE='R'"
            If rbtnbooked.Checked Then StrSql += vbCrLf + "AND  ORTYPE='B'"
            Dim dt As New DataTable
            Da = New OleDbDataAdapter(StrSql, cn)
            Da.Fill(dt)
            If Not dt.Rows.Count > 0 Then
                MsgBox("Invalid OrdRep No", MsgBoxStyle.Information)
                Exit Sub
            End If
            'btnSearch.Focus()
        End If
    End Sub

    Private Sub txtOrdRepNo_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtOrdRepNo.KeyDown
        If e.KeyCode = Keys.Insert Then
            Dim TEMPHELPORD As String = "TEMPHELPORD" + systemId
            CostId = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostcentre.Text & "'")
            StrSql = "  IF OBJECT_ID('TEMPTABLEDB.." & TEMPHELPORD & "')IS NOT NULL DROP TABLE TEMPTABLEDB.." & TEMPHELPORD & ""
            StrSql += vbCrLf + "  SELECT"
            StrSql += vbCrLf + "  	 DISTINCT SUBSTRING(ORNO,6,20)ORNO,O.COMPANYID COMPANYID_HIDE,O.COSTID COSTID_HIDE"
            StrSql += vbCrLf + "  	,PNAME"
            StrSql += vbCrLf + "  	,CASE WHEN ISNULL(DOORNO,'') = '' THEN ADDRESS1 ELSE ISNULL(DOORNO,'') + ISNULL(ADDRESS1,'') END ADDRESS1"
            StrSql += vbCrLf + "  	,ADDRESS2,MOBILE"
            StrSql += vbCrLf + "  	,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = O.COSTID) COSTCENTRE"
            StrSql += " INTO TEMPTABLEDB.DBO." & TEMPHELPORD
            StrSql += vbCrLf + "  FROM " & cnAdminDb & "..ORMAST AS O LEFT OUTER JOIN " & cnAdminDb & "..CUSTOMERINFO AS C"
            StrSql += vbCrLf + "  ON O.BATCHNO = C.BATCHNO"
            StrSql += vbCrLf + "  LEFT OUTER JOIN " & cnAdminDb & "..PERSONALINFO AS P"
            StrSql += vbCrLf + "  ON C.PSNO = P.SNO WHERE ISNULL(O.CANCEL,'') != 'Y'"
            StrSql += vbCrLf + "  AND ISNULL(O.COSTID,'') = '" & CostId & "'"
            If rbtorder.Checked Then StrSql += vbCrLf + "AND  ORTYPE='O'"
            If rbtRepair.Checked Then StrSql += vbCrLf + "AND  ORTYPE='R'"
            If rbtnbooked.Checked Then StrSql += vbCrLf + "AND  ORTYPE='B'"
            Cmd = New OleDbCommand(StrSql, cn)
            Cmd.ExecuteNonQuery()
            StrSql = "SELECT * FROM  TEMPTABLEDB.DBO." & TEMPHELPORD & " ORDER BY CONVERT(INT,SUBSTRING(ORNO,2,14))"
            Dim dr As DataRow
            dr = Nothing
            dr = BrighttechPack.SearchDialog.Show_R("Find Order No", StrSql, cn, 3, , , , , , , False)
            If dr IsNot Nothing Then
                txtOrdRepNo.Text = dr.Item("ORNO").ToString
            End If
            txtOrdRepNo.SelectAll()
        End If
    End Sub
    Private Sub Prop_Sets()
        Dim obj As New FRM_ORDERREPAIRSTATUS_Properties
        'obj.p_cmbCostcentre = cmbCostcentre.Text
        obj.p_txtOrdRepNo = txtOrdRepNo.Text
        obj.p_chkImage = chkImage.Checked
        SetSettingsObj(obj, Me.Name, GetType(FRM_ORDERREPAIRSTATUS_Properties))
    End Sub
    Private Sub Prop_Gets()
        Dim obj As New FRM_ORDERREPAIRSTATUS_Properties
        GetSettingsObj(obj, Me.Name, GetType(FRM_ORDERREPAIRSTATUS_Properties))
        'cmbCostcentre.Text = obj.p_cmbCostcentre
        txtOrdRepNo.Text = obj.p_txtOrdRepNo
        chkImage.Checked = obj.p_chkImage
    End Sub

    
    Private Sub rbtorder_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtorder.CheckedChanged
        Label2.Text = "Order No"
    End Sub

    Private Sub rbtRepair_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtRepair.CheckedChanged
        Label2.Text = "Repair No"
    End Sub
   
End Class

Public Class FRM_ORDERREPAIRSTATUS_Properties
    Private cmbCostcentre As String = cnCostName
    Public Property p_cmbCostcentre() As String
        Get
            Return cmbCostcentre
        End Get
        Set(ByVal value As String)
            cmbCostcentre = value
        End Set
    End Property
    Private txtOrdRepNo As String = ""
    Public Property p_txtOrdRepNo() As String
        Get
            Return txtOrdRepNo
        End Get
        Set(ByVal value As String)
            txtOrdRepNo = value
        End Set
    End Property
    Private chkImage As Boolean = False
    Public Property p_chkImage() As Boolean
        Get
            Return chkImage
        End Get
        Set(ByVal value As Boolean)
            chkImage = value
        End Set
    End Property
End Class