Imports System.Data.OleDb

Public Class frmTagStuddedStoneDetails
    Dim sno As String
    Dim lblTit As String
    Dim QRY As String
    Dim QRY1 As String
    Dim _counter As String
    Dim MODE As String
    Dim StrSql As String = Nothing
    Dim dt As New DataTable
    Dim da As New OleDbDataAdapter
    Dim dtStuddedDetails As New DataTable("GridStuddedDetails")
    Dim objitemtagstockview As New frmTagedItemsStockView

    Dim rowSubTotal(5) As Integer
    Dim indexSubTotal As Integer = 0
    Dim rowStnType(5) As Integer
    Dim indexStnStype As Integer

    Private WithEvents btnExcelStudDetails As New Button
    Private WithEvents btnPrintStudDetails As New Button
    Private WithEvents btnExit As New Button

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Public Sub New(ByVal sno As String, ByVal QRY As String, ByVal lbltit As String, Optional ByVal QRY1 As String = Nothing, Optional ByVal MODE As String = Nothing, Optional ByVal counter As String = Nothing)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        Me.sno = sno
        Me.lblTit = lbltit
        Me.QRY = QRY
        Me.QRY1 = QRY1
        Me.MODE = MODE
        Me._counter = counter
        If MODE = "M" Then
            lblTitle.Text = lbltit + " (METAL DETAILS)"
        Else
            lblTitle.Text = lbltit + " (STUDDED DETAILS)"
        End If

        If MODE = "M" Then
            ''Creating dtStuddedDetails TAble
            dtStuddedDetails.Columns.Add("Particular", GetType(String))
            dtStuddedDetails.Columns.Add("Itemname", GetType(String))
            dtStuddedDetails.Columns.Add("Grswt", GetType(String))
            dtStuddedDetails.Columns.Add("Netwt", GetType(String))
            dtStuddedDetails.Columns.Add("Salvalue", GetType(String))
            dtStuddedDetails.Columns.Add("COLHEAD", GetType(String))
        Else
            ''Creating dtStuddedDetails TAble
            dtStuddedDetails.Columns.Add("Particular", GetType(String))
            dtStuddedDetails.Columns.Add("cPcs", GetType(String))
            dtStuddedDetails.Columns.Add("cWeight", GetType(String))
            dtStuddedDetails.Columns.Add("cAmount", GetType(String))
            dtStuddedDetails.Columns.Add("gPcs", GetType(String))
            dtStuddedDetails.Columns.Add("gWeight", GetType(String))
            dtStuddedDetails.Columns.Add("gAmount", GetType(String))
            dtStuddedDetails.Columns.Add("tPcs", GetType(String))
            dtStuddedDetails.Columns.Add("tWeight", GetType(String))
            dtStuddedDetails.Columns.Add("tAmount", GetType(String))
            dtStuddedDetails.Columns.Add("COLHEAD", GetType(String))
        End If


        funcShowStuddedDetails()
        If gridStudDetails.RowCount = 0 Then
            MsgBox("There is no Record", MsgBoxStyle.Exclamation)
        Else
            If MODE <> "M" Then
                funcgridHead()
            Else
                grpStudDetails.Text = "METAL DETAILS"
            End If
            grpStudDetails.Visible = True
            btnExport.Focus()
        End If
    End Sub

    Function funcShowStuddedDetails() As Integer
        Dim cmd As OleDbCommand

        If MODE = "M" Then
            StrSql = " IF (SELECT TOP  1 1 FROM SYSOBJECTS WHERE NAME = 'TEMPMETALDETTOTAL') > 0 DROP TABLE TEMPMETALDETTOTAL"
            cmd = New OleDbCommand(StrSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            StrSql = " IF (SELECT TOP  1 1 FROM SYSOBJECTS WHERE NAME = 'TEMPMETALDET') > 0 DROP TABLE TEMPMETALDET"
            StrSql += " SELECT CATNAME AS  PARTICULAR,ITEMNAME,"
            StrSql += " CASE WHEN SUM(GRSWT) = 0 THEN NULL ELSE SUM(GRSWT) END GRSWT, "
            StrSql += " CASE WHEN SUM(NETWT) = 0 THEN NULL ELSE SUM(NETWT) END NETWT, "
            StrSql += " CASE WHEN SUM(SALVALUE) = 0 THEN NULL ELSE SUM(SALVALUE) END SALVALUE, "
            StrSql += " 1 RESULT, CONVERT(VARCHAR(1),'')COLHEAD ,CATNAME AS CATNAME1"
            StrSql += " INTO TEMPMETALDET"
            StrSql += " FROM ("
            StrSql += " SELECT"
            StrSql += " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = T.CATCODE)CATNAME,"
            StrSql += " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)ITEMNAME,"
            StrSql += " GRSWT,NETWT, "
            StrSql += " AMOUNT SALVALUE,"
            StrSql += " '1'AS RESULT,CONVERT(VARCHAR(1),'')COLHEAD"
            StrSql += " FROM " & cnAdminDb & "..ITEMTAGMETAL AS T "
            StrSql += QRY + vbCrLf
            StrSql += " )S" + vbCrLf
            StrSql += " GROUP BY CATNAME,ITEMNAME" + vbCrLf
        Else
            StrSql = " IF (SELECT TOP  1 1 FROM SYSOBJECTS WHERE NAME = 'TEMPSTUDDET') > 0 DROP TABLE TEMPSTUDDET"
            StrSql += " SELECT " + vbCrLf
            StrSql += " CASE WHEN STNSUBITEMID <> 0 THEN (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = S.STNSUBITEMID)"
            StrSql += " ELSE (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID) END AS PARTICULAR," + vbCrLf
            StrSql += " CONVERT(INT,ISNULL(SUM((CASE WHEN STONEUNIT = 'C' THEN STNPCS END)),0)) AS CPCS," + vbCrLf
            StrSql += " CONVERT(NUMERIC(15,3),ISNULL(SUM((CASE WHEN STONEUNIT = 'C' THEN STNWT END)),0)) AS CWEIGHT," + vbCrLf
            StrSql += " CONVERT(NUMERIC(15,2),ISNULL(SUM((CASE WHEN STONEUNIT = 'C' THEN STNAMT END)),0)) AS CAMOUNT," + vbCrLf
            StrSql += " CONVERT(INT,ISNULL(SUM((CASE WHEN STONEUNIT = 'G' THEN STNPCS END)),0)) AS GPCS," + vbCrLf
            StrSql += " CONVERT(NUMERIC(15,3),ISNULL(SUM((CASE WHEN STONEUNIT = 'G' THEN STNWT END)),0)) AS GWEIGHT," + vbCrLf
            StrSql += " CONVERT(NUMERIC(15,2),ISNULL(SUM((CASE WHEN STONEUNIT = 'G' THEN STNAMT END)),0)) AS GAMOUNT," + vbCrLf
            StrSql += " CONVERT(INT,ISNULL(SUM(STNPCS),0)) AS TPCS," + vbCrLf
            StrSql += " CONVERT(NUMERIC(15,3),ISNULL(SUM(STNWT),0)) AS TWEIGHT," + vbCrLf
            StrSql += " CONVERT(NUMERIC(15,2),ISNULL(SUM(STNAMT),0)) AS TAMOUNT,CONVERT(VARCHAR(1),'')COLHEAD," + vbCrLf
            StrSql += " CASE WHEN STNTYPE = 'D' THEN 'DIAMOND'" + vbCrLf
            StrSql += " WHEN STNTYPE = 'S' THEN 'STONE'" + vbCrLf
            StrSql += " WHEN STNTYPE = 'P' THEN 'PRECIOUS'" + vbCrLf
            StrSql += " ELSE STNTYPE END AS STNTYPE," + vbCrLf
            StrSql += " 1 AS RESULT" + vbCrLf
            StrSql += " INTO TEMPSTUDDET"
            StrSql += " FROM" + vbCrLf
            StrSql += " (" + vbCrLf
            StrSql += " SELECT (SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.STNITEMID)STNTYPE,STNITEMID,STNSUBITEMID,STNPCS,STNWT,STNAMT,STONEUNIT" + vbCrLf
            StrSql += " FROM " & cnAdminDb & "..ITEMTAGSTONE AS T" + vbCrLf
            StrSql += QRY + vbCrLf
            StrSql += " )S" + vbCrLf
            StrSql += " GROUP BY STNTYPE,STNITEMID,STNSUBITEMID,STONEUNIT" + vbCrLf
        End If
        cmd = New OleDbCommand(StrSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        'adding approval
        If QRY1 <> "" Then
            StrSql = " INSERT INTO TEMPSTUDDET "
            StrSql += " SELECT " + vbCrLf
            StrSql += " CASE WHEN STNSUBITEMID <> 0 THEN (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = S.STNSUBITEMID)"
            StrSql += " ELSE (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID) END AS PARTICULAR," + vbCrLf
            StrSql += " CONVERT(INT,ISNULL(SUM((CASE WHEN STONEUNIT = 'C' THEN STNPCS END)),0)) AS CPCS," + vbCrLf
            StrSql += " CONVERT(NUMERIC(15,3),ISNULL(SUM((CASE WHEN STONEUNIT = 'C' THEN STNWT END)),0)) AS CWEIGHT," + vbCrLf
            StrSql += " CONVERT(NUMERIC(15,2),ISNULL(SUM((CASE WHEN STONEUNIT = 'C' THEN STNAMT END)),0)) AS CAMOUNT," + vbCrLf
            StrSql += " CONVERT(INT,ISNULL(SUM((CASE WHEN STONEUNIT = 'G' THEN STNPCS END)),0)) AS GPCS," + vbCrLf
            StrSql += " CONVERT(NUMERIC(15,3),ISNULL(SUM((CASE WHEN STONEUNIT = 'G' THEN STNWT END)),0)) AS GWEIGHT," + vbCrLf
            StrSql += " CONVERT(NUMERIC(15,2),ISNULL(SUM((CASE WHEN STONEUNIT = 'G' THEN STNAMT END)),0)) AS GAMOUNT," + vbCrLf
            StrSql += " CONVERT(INT,ISNULL(SUM(STNPCS),0)) AS TPCS," + vbCrLf
            StrSql += " CONVERT(NUMERIC(15,3),ISNULL(SUM(STNWT),0)) AS TWEIGHT," + vbCrLf
            StrSql += " CONVERT(NUMERIC(15,2),ISNULL(SUM(STNAMT),0)) AS TAMOUNT,CONVERT(VARCHAR(1),'')COLHEAD," + vbCrLf
            StrSql += " CASE WHEN STNTYPE = 'D' THEN 'DIAMOND'" + vbCrLf
            StrSql += " WHEN STNTYPE = 'S' THEN 'STONE'" + vbCrLf
            StrSql += " WHEN STNTYPE = 'P' THEN 'PRECIOUS'" + vbCrLf
            StrSql += " ELSE STNTYPE END AS STNTYPE," + vbCrLf
            StrSql += " 1 AS RESULT" + vbCrLf
            StrSql += " FROM" + vbCrLf
            StrSql += " (" + vbCrLf
            StrSql += " SELECT (SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.STNITEMID)STNTYPE,STNITEMID,STNSUBITEMID,STNPCS,STNWT,STNAMT,STONEUNIT" + vbCrLf
            StrSql += " FROM " & cnAdminDb & "..ITEMTAGSTONE AS T" + vbCrLf
            StrSql += QRY1 + vbCrLf
            StrSql += " S" + vbCrLf
            StrSql += " GROUP BY STNTYPE,STNITEMID,STNSUBITEMID,STONEUNIT" + vbCrLf
            cmd = New OleDbCommand(StrSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End If
        If MODE = "M" Then
            StrSql = " IF (sELECT COUNT(*) FROM TEMPMETALDET)>0"
            StrSql += " BEGIN"
            StrSql += " INSERT INTO TEMPMETALDET(PARTICULAR,ITEMNAME,RESULT,COLHEAD,CATNAME1)"
            StrSql += " SELECT DISTINCT CATNAME1,'' ,0 RESULT,'T' COLHEAD ,CATNAME1 FROM TEMPMETALDET"
            StrSql += " INSERT INTO TEMPMETALDET(PARTICULAR,ITEMNAME,RESULT,COLHEAD,GRSWT,NETWT,SALVALUE,CATNAME1)"
            StrSql += " SELECT CATNAME1 + ' TOTAL',''CATNAME1,2 RESULT,'S' COLHEAD "
            StrSql += " ,SUM(GRSWT),SUM(NETWT),SUM(SALVALUE) ,CATNAME1"
            StrSql += " FROM TEMPMETALDET WHERE RESULT = 1 GROUP BY CATNAME1"
            StrSql += " INSERT INTO TEMPMETALDET(PARTICULAR,ITEMNAME,RESULT,COLHEAD,GRSWT,NETWT,SALVALUE,CATNAME1)"
            StrSql += " SELECT 'GRAND TOTAL','ZZZZZ'ITEMNAME,3 RESULT,'G' COLHEAD "
            StrSql += " ,SUM(GRSWT),SUM(NETWT),SUM(SALVALUE),'zzzzzzzzz'CATNAME1 "
            StrSql += " FROM TEMPMETALDET WHERE RESULT = 2 "
            StrSql += " END"
        Else
            StrSql = " IF (sELECT COUNT(*) FROM TEMPSTUDDET)>0"
            StrSql += " BEGIN"
            StrSql += " INSERT INTO TEMPSTUDDET(PARTICULAR,STNTYPE,RESULT,COLHEAD)"
            StrSql += " SELECT DISTINCT STNTYPE,STNTYPE,0 RESULT,'T' COLHEAD FROM TEMPSTUDDET"
            StrSql += " INSERT INTO TEMPSTUDDET(PARTICULAR,STNTYPE,RESULT,COLHEAD,CPCS,CWEIGHT,CAMOUNT,GPCS,GWEIGHT,GAMOUNT,TPCS,TWEIGHT,TAMOUNT)"
            StrSql += " SELECT STNTYPE + ' TOTAL',STNTYPE,2 RESULT,'S' COLHEAD "
            StrSql += " ,SUM(CPCS),SUM(CWEIGHT),SUM(CAMOUNT)"
            StrSql += " ,SUM(GPCS),SUM(GWEIGHT),SUM(GAMOUNT)"
            StrSql += " ,SUM(TPCS),SUM(TWEIGHT),SUM(TAMOUNT)"
            StrSql += " FROM TEMPSTUDDET WHERE RESULT = 1 GROUP BY STNTYPE"
            StrSql += " INSERT INTO TEMPSTUDDET(PARTICULAR,STNTYPE,RESULT,COLHEAD,CPCS,CWEIGHT,CAMOUNT,GPCS,GWEIGHT,GAMOUNT,TPCS,TWEIGHT,TAMOUNT)"
            StrSql += " SELECT 'GRAND TOTAL','ZZZZZ'STNTYPE,3 RESULT,'G' COLHEAD "
            StrSql += " ,SUM(CPCS),SUM(CWEIGHT),SUM(CAMOUNT)"
            StrSql += " ,SUM(GPCS),SUM(GWEIGHT),SUM(GAMOUNT)"
            StrSql += " ,SUM(TPCS),SUM(TWEIGHT),SUM(TAMOUNT)"
            StrSql += " FROM TEMPSTUDDET WHERE RESULT = 2"
            StrSql += " END"
        End If
        cmd = New OleDbCommand(StrSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()


        'StrSql += " UNION ALL" + vbCrLf

        'StrSql += " SELECT " + vbCrLf
        'StrSql += " CASE WHEN STNTYPE = 'D' THEN 'DIAMOND'" + vbCrLf
        'StrSql += " WHEN STNTYPE = 'S' THEN 'STONE'" + vbCrLf
        'StrSql += " WHEN STNTYPE = 'P' THEN 'PRECIOUS'" + vbCrLf
        'StrSql += " ELSE 'GRAND' END AS STNTYPE," + vbCrLf
        'StrSql += " '2' AS RESULT," + vbCrLf
        'StrSql += " 'TOTAL' AS ITEMNAME,'TOTAL'AS SUBITEMNAME," + vbCrLf
        'StrSql += " ISNULL(SUM((CASE WHEN STONEUNIT = 'C' THEN STNPCS END)),0) AS CPCS," + vbCrLf
        'StrSql += " ISNULL(SUM((CASE WHEN STONEUNIT = 'C' THEN STNWT END)),0) AS CWEIGHTT," + vbCrLf
        'StrSql += " ISNULL(SUM((CASE WHEN STONEUNIT = 'C' THEN STNAMT END)),0) AS CAMOUNT," + vbCrLf
        'StrSql += " ISNULL(SUM((CASE WHEN STONEUNIT = 'G' THEN STNPCS END)),0) AS GPCS," + vbCrLf
        'StrSql += " ISNULL(SUM((CASE WHEN STONEUNIT = 'G' THEN STNWT END)),0) AS GWEIGHT," + vbCrLf
        'StrSql += " ISNULL(SUM((CASE WHEN STONEUNIT = 'G' THEN STNAMT END)),0) AS GAMOUNT," + vbCrLf
        'StrSql += " ISNULL(SUM(STNPCS),0) AS TPCS," + vbCrLf
        'StrSql += " ISNULL(SUM(STNWT),0) AS TWEIGHT," + vbCrLf
        'StrSql += " ISNULL(SUM(STNAMT),0) AS TAMOUNT,'S'COLHEAD" + vbCrLf
        'StrSql += " FROM" + vbCrLf
        'StrSql += " (" + vbCrLf
        'StrSql += " SELECT (SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.STNITEMID)STNTYPE,STNITEMID,STNSUBITEMID,STNPCS,STNWT,STNAMT,STONEUNIT" + vbCrLf
        'StrSql += " FROM " & cnAdminDb & "..ITEMTAGSTONE AS T" + vbCrLf
        'StrSql += QRY + vbCrLf
        'StrSql += " )S" + vbCrLf
        'StrSql += " GROUP BY STNTYPE" + vbCrLf
        'StrSql += " UNION ALL" + vbCrLf

        'StrSql += " SELECT " + vbCrLf
        'StrSql += " 'X' AS STNTYPE,'3'AS RESULT," + vbCrLf
        'StrSql += " 'GRANTOTAL' AS ITEMNAME,'GRANDTOTAL'AS SUBITEMNAME," + vbCrLf
        'StrSql += " ISNULL(SUM((CASE WHEN STONEUNIT = 'C' THEN STNPCS END)),0) AS CPCS," + vbCrLf
        'StrSql += " ISNULL(SUM((CASE WHEN STONEUNIT = 'C' THEN STNWT END)),0) AS CWEIGHT," + vbCrLf
        'StrSql += " ISNULL(SUM((CASE WHEN STONEUNIT = 'C' THEN STNAMT END)),0) AS CAMOUNT," + vbCrLf
        'StrSql += " ISNULL(SUM((CASE WHEN STONEUNIT = 'G' THEN STNPCS END)),0) AS GPCS," + vbCrLf
        'StrSql += " ISNULL(SUM((CASE WHEN STONEUNIT = 'G' THEN STNWT END)),0) AS GWEIGHT," + vbCrLf
        'StrSql += " ISNULL(SUM((CASE WHEN STONEUNIT = 'G' THEN STNAMT END)),0) AS GAMOUNT," + vbCrLf
        'StrSql += " ISNULL(SUM(STNPCS),0) AS TPCS," + vbCrLf
        'StrSql += " ISNULL(SUM(STNWT),0) AS TWEIGHT," + vbCrLf
        'StrSql += " ISNULL(SUM(STNAMT),0) AS TAMOUNT,'G'COLHEAD" + vbCrLf
        'StrSql += " FROM" + vbCrLf
        'StrSql += " (" + vbCrLf
        'StrSql += " SELECT (SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.STNITEMID)STNTYPE,STNITEMID,STNSUBITEMID,STNPCS,STNWT,STNAMT,STONEUNIT" + vbCrLf
        'StrSql += " FROM " & cnAdminDb & "..ITEMTAGSTONE AS T" + vbCrLf
        'StrSql += QRY + vbCrLf
        'StrSql += " )S" + vbCrLf
        'StrSql += " )Y" + vbCrLf
        'StrSql += " ORDER BY STNTYPE,RESULT" + vbCrLf
        If MODE = "M" Then
            StrSql = " UPDATE TEMPMETALDET SET NETWT = NULL WHERE NETWT = 0"
            StrSql += " UPDATE TEMPMETALDET SET GRSWT = NULL WHERE GRSWT = 0"
            StrSql += " UPDATE TEMPMETALDET SET SALVALUE = NULL WHERE SALVALUE = 0"
        Else
            StrSql = " UPDATE TEMPSTUDDET SET CPCS = NULL WHERE CPCS = 0"
            StrSql += " UPDATE TEMPSTUDDET SET CWEIGHT = NULL WHERE CWEIGHT = 0"
            StrSql += " UPDATE TEMPSTUDDET SET CAMOUNT = NULL WHERE CAMOUNT = 0"
            StrSql += " UPDATE TEMPSTUDDET SET GPCS = NULL WHERE GPCS = 0"
            StrSql += " UPDATE TEMPSTUDDET SET GWEIGHT = NULL WHERE GWEIGHT = 0"
            StrSql += " UPDATE TEMPSTUDDET SET GAMOUNT = NULL WHERE GAMOUNT = 0"
            StrSql += " UPDATE TEMPSTUDDET SET TPCS = NULL WHERE TPCS = 0"
            StrSql += " UPDATE TEMPSTUDDET SET TWEIGHT = NULL WHERE TWEIGHT = 0"
            StrSql += " UPDATE TEMPSTUDDET SET TAMOUNT = NULL WHERE TAMOUNT = 0"
        End If

        cmd = New OleDbCommand(StrSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        Dim dt As New DataTable
        dt.Clear()
        If MODE = "M" Then
            StrSql = " SELECT * FROM TEMPMETALDET ORDER BY CATNAME1,RESULT"
        Else
            StrSql = " SELECT * FROM TEMPSTUDDET ORDER BY STNTYPE,RESULT"
        End If

        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Exit Function
        End If
        gridStudDetails.DataSource = dt ' dtStuddedDetails
        With gridStudDetails
            .Columns("COLHEAD").Visible = False

            If MODE = "M" Then
                With .Columns("Particular")
                    .HeaderText = "Particular"
                    .Width = 250
                    .Resizable = DataGridViewTriState.False
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
                With .Columns("ITEMNAME")
                    .HeaderText = "ItemName"
                    .Width = 170
                    .Resizable = DataGridViewTriState.False
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
                With .Columns("GRSWT")
                    .HeaderText = "Grs.Wt"
                    .Width = 70
                    .Resizable = DataGridViewTriState.False
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
                With .Columns("NETWT")
                    .HeaderText = "Net.Wt"
                    .Width = 70
                    .Resizable = DataGridViewTriState.False
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
                With .Columns("SALVALUE")
                    .HeaderText = "Sal.Value"
                    .Width = 100
                    .Resizable = DataGridViewTriState.False
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
                .Columns("CATNAME1").Visible = False
            Else
                With .Columns("Particular")
                    .HeaderText = ""
                    .Width = 150
                    .Resizable = DataGridViewTriState.False
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
                With .Columns("cPcs")
                    .HeaderText = "C.PCS"
                    .Width = 50
                    .Resizable = DataGridViewTriState.False
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
                With .Columns("cWeight")
                    .HeaderText = "C.WEIGHT"
                    .Width = 80
                    .Resizable = DataGridViewTriState.False
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
                With .Columns("cAmount")
                    .HeaderText = "C.AMOUNT"
                    .Width = 80
                    .Resizable = DataGridViewTriState.False
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With

                With .Columns("gPcs")
                    .HeaderText = "G.PCS"
                    .Width = 50
                    .Resizable = DataGridViewTriState.False
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
                With .Columns("gWeight")
                    .HeaderText = "G.WEIGHT"
                    .Width = 80
                    .Resizable = DataGridViewTriState.False
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
                With .Columns("gAmount")
                    .HeaderText = "G.AMOUNT"
                    .Width = 80
                    .Resizable = DataGridViewTriState.False
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
                With .Columns("tPcs")
                    .HeaderText = "T.PCS"
                    .Width = 50
                    .Resizable = DataGridViewTriState.False
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
                With .Columns("tWeight")
                    .HeaderText = "T.WEIGHT"
                    .Width = 80
                    .Resizable = DataGridViewTriState.False
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
                With .Columns("tAmount")
                    .HeaderText = "T.AMOUNT"
                    .Width = 80
                    .Resizable = DataGridViewTriState.False
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
            End If

            .Columns("RESULT").Visible = False
            .Columns("COLHEAD").Visible = False
        End With
    End Function

    Function funcgridHead() As Integer
        Dim dtMergeHeader As New DataTable("~MERGEHEADER")
        With dtMergeHeader
            .Columns.Add("PARTICULAR", GetType(String))
            .Columns.Add("CPCS~CWEIGHT~CAMOUNT", GetType(String))
            .Columns.Add("GPCS~GWEIGHT~GAMOUNT", GetType(String))
            .Columns.Add("TPCS~TWEIGHT~TAMOUNT", GetType(String))
            .Columns.Add("STNTYPE", GetType(String))
            .Columns.Add("SCROLL", GetType(String))
        End With
        gridStudDetailsHead.DataSource = dtMergeHeader
        With gridStudDetailsHead
            .Height = .ColumnHeadersHeight
            .Columns("PARTICULAR").Width = gridStudDetails.Columns("PARTICULAR").Width
            .Columns("CPCS~CWEIGHT~CAMOUNT").Width = gridStudDetails.Columns("CPCS").Width + gridStudDetails.Columns("CWEIGHT").Width + gridStudDetails.Columns("CAMOUNT").Width
            .Columns("GPCS~GWEIGHT~GAMOUNT").Width = gridStudDetails.Columns("GPCS").Width + gridStudDetails.Columns("GWEIGHT").Width + gridStudDetails.Columns("GAMOUNT").Width
            .Columns("TPCS~TWEIGHT~TAMOUNT").Width = gridStudDetails.Columns("TPCS").Width + gridStudDetails.Columns("TWEIGHT").Width + gridStudDetails.Columns("TAMOUNT").Width
            .Columns("STNTYPE").Width = gridStudDetails.Columns("STNTYPE").Width
            .Columns("PARTICULAR").HeaderText = "PARTICULAR"
            .Columns("CPCS~CWEIGHT~CAMOUNT").HeaderText = "CARAT"
            .Columns("GPCS~GWEIGHT~GAMOUNT").HeaderText = "GRAM"
            .Columns("TPCS~TWEIGHT~TAMOUNT").HeaderText = "TOTAL"
            .Columns("STNTYPE").HeaderText = "STNTYPE"
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            For CNT As Integer = 0 To .Columns.Count - 1
                .Columns(CNT).SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns(CNT).Resizable = DataGridViewTriState.False
            Next
            .RowHeadersVisible = False
            .Columns("SCROLL").Visible = CType(gridStudDetailsHead.Controls(0), HScrollBar).Visible
            .Columns("SCROLL").Width = CType(gridStudDetailsHead.Controls(1), VScrollBar).Width
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
            .Height = 18
            With .Columns("SCROLL")
                .Width = 20
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .HeaderText = ""
            End With
        End With
    End Function

    Function funcAddStuddedDetails(ByVal Particular As String, ByVal cPcs As String, ByVal cWeight As String, ByVal cAmount As String, ByVal gPcs As String, ByVal gWeight As String, ByVal gAmount As String, ByVal tpcs As String, ByVal tWeight As String, ByVal tAmount As String, ByVal COLHEAD As String) As Integer
        Dim ro As DataRow = Nothing
        ro = dtStuddedDetails.NewRow
        ro("Particular") = Particular
        ro("cPcs") = IIf(Val(cPcs) = 0, DBNull.Value, cPcs)
        ro("cWeight") = IIf(Val(cWeight) = 0, DBNull.Value, cWeight)
        ro("cAmount") = IIf(Val(cAmount) = 0, DBNull.Value, cAmount)
        ro("gPcs") = IIf(Val(gPcs) = 0, DBNull.Value, gPcs)
        ro("gWeight") = IIf(Val(gWeight) = 0, DBNull.Value, gWeight)
        ro("gAmount") = IIf(Val(gAmount) = 0, DBNull.Value, gAmount)
        ro("tPcs") = IIf(Val(tpcs) = 0, DBNull.Value, tpcs)
        ro("tWeight") = IIf(Val(tWeight) = 0, DBNull.Value, tWeight)
        ro("tAmount") = IIf(Val(tAmount) = 0, DBNull.Value, tAmount)
        ro("COLHEAD") = COLHEAD
        dtStuddedDetails.Rows.Add(ro)
    End Function

    ''Private Sub gridStudDetails_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles gridStudDetails.CellFormatting
    ''    With gridStudDetails
    ''        Select Case .Rows(e.RowIndex).Cells("COLHEAD").Value.ToString
    ''            Case "G"
    ''                .Rows(e.RowIndex).DefaultCellStyle.BackColor = reportTotalStyle.BackColor
    ''                .Rows(e.RowIndex).DefaultCellStyle.Font = reportTotalStyle.Font
    ''            Case "S"
    ''                .Rows(e.RowIndex).DefaultCellStyle.Font = reportSubTotalStyle.Font
    ''            Case "T"
    ''                .Rows(e.RowIndex).Cells("PARTICULAR").Style.Font = reportHeadStyle.Font
    ''                .Rows(e.RowIndex).Cells("PARTICULAR").Style.BackColor = reportHeadStyle.BackColor
    ''        End Select
    ''    End With
    ''End Sub

    Function GridViewFormat() As Integer
        For Each dgvRow As DataGridViewRow In gridStudDetails.Rows
            With dgvRow
                Select Case .Cells("COLHEAD").Value.ToString
                    Case "G"
                        .DefaultCellStyle.BackColor = reportTotalStyle.BackColor
                        .DefaultCellStyle.Font = reportTotalStyle.Font
                    Case "S"
                        .DefaultCellStyle.Font = reportSubTotalStyle.Font
                    Case "T"
                        .Cells("PARTICULAR").Style.Font = reportHeadStyle.Font
                        .Cells("PARTICULAR").Style.BackColor = reportHeadStyle.BackColor
                End Select
            End With
        Next
    End Function
    Private Sub btnExcelStudDetails_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExcelStudDetails.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridStudDetails.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridStudDetails, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub gridStudDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridStudDetails.KeyPress
        If e.KeyChar = Chr(Keys.Escape) Then
            btnExit_Click(Me, New EventArgs)
        ElseIf e.KeyChar = Chr(Keys.X) Or AscW(e.KeyChar) = 120 Then
            Me.btnExcelStudDetails_Click(Me, New EventArgs)
        ElseIf e.KeyChar = Chr(Keys.P) Or UCase(e.KeyChar) = "P" Then
            Me.btnPrintStudDetails_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub btnPrintStudDetails_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintStudDetails.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridStudDetails.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridStudDetails, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub frmTagStuddedStoneDetails_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GridViewFormat()
        gridStudDetails.Select()
    End Sub

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        'If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridStudDetails.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridStudDetails, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        ' If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridStudDetails.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridStudDetails, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub ContextMenuStrip1_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip1.Opening

    End Sub

    Private Sub AutoResizeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AutoResizeToolStripMenuItem.Click
        If gridStudDetails.RowCount > 0 Then
            If AutoResizeToolStripMenuItem.Checked Then
                gridStudDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                gridStudDetails.Invalidate()
                For Each dgvCol As DataGridViewColumn In gridStudDetails.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridStudDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            Else
                For Each dgvCol As DataGridViewColumn In gridStudDetails.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridStudDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End If
        End If
    End Sub
End Class
