Imports System.Data.OleDb

Public Class frmTagStuddedStoneDetails
    Dim sno As String
    Dim lblTit As String
    Dim QRY As String
    Dim StrSql As String = Nothing
    Dim dt As New DataTable
    Dim da As New OleDbDataAdapter
    Dim dtStuddedDetails As New DataTable("GridStuddedDetails")

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

    Public Sub New(ByVal sno As String, ByVal QRY As String, ByVal lbltit As String)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        Me.sno = sno
        Me.lblTit = lbltit
        Me.QRY = QRY
        lblTitle.Text = lbltit + " (STUDDED DETAILS)"

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

        funcShowStuddedDetails()
        funcgridHead()
        grpStudDetails.Visible = True
        'grpStudDetails.BringToFront()
        gridStudDetails.Focus()
    End Sub

    Function funcShowStuddedDetails() As Integer
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
        Dim cmd As OleDbCommand
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

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
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
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
        StrSql = " UPDATE TEMPSTUDDET SET CPCS = NULL WHERE CPCS = 0"
        StrSql += " UPDATE TEMPSTUDDET SET CWEIGHT = NULL WHERE CWEIGHT = 0"
        StrSql += " UPDATE TEMPSTUDDET SET CAMOUNT = NULL WHERE CAMOUNT = 0"
        StrSql += " UPDATE TEMPSTUDDET SET GPCS = NULL WHERE GPCS = 0"
        StrSql += " UPDATE TEMPSTUDDET SET GWEIGHT = NULL WHERE GWEIGHT = 0"
        StrSql += " UPDATE TEMPSTUDDET SET GAMOUNT = NULL WHERE GAMOUNT = 0"
        StrSql += " UPDATE TEMPSTUDDET SET TPCS = NULL WHERE TPCS = 0"
        StrSql += " UPDATE TEMPSTUDDET SET TWEIGHT = NULL WHERE TWEIGHT = 0"
        StrSql += " UPDATE TEMPSTUDDET SET TAMOUNT = NULL WHERE TAMOUNT = 0"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        Dim dt As New DataTable
        dt.Clear()
        StrSql = " SELECT * FROM TEMPSTUDDET ORDER BY STNTYPE,RESULT"
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Exit Function
        End If
        'Dim ro As DataRow = Nothing
        'dtStuddedDetails.Clear()
        'Dim stnType As String = Nothing
        'Dim stnItem As String = Nothing
        'Dim stnSubItem As String = Nothing
        'Dim cPcs As Integer = 0
        'Dim cWeight As Double = 0
        'Dim cAmount As Double = 0
        'Dim gPcs As Integer = 0
        'Dim gWeight As Double = 0
        'Dim gAmount As Double = 0
        'Dim tPcs As Integer = 0
        'Dim tWeight As Double = 0
        'Dim tAmount As Double = 0
        'Dim COLHEAD As String = Nothing
        'indexStnStype = 0
        'indexSubTotal = 0
        'For cnt As Integer = 0 To dt.Rows.Count - 1
        '    cPcs = 0
        '    cWeight = 0
        '    cAmount = 0
        '    gPcs = 0
        '    gWeight = 0
        '    gAmount = 0
        '    tPcs = 0
        '    tWeight = 0
        '    tAmount = 0
        '    COLHEAD = ""
        '    With dt.Rows(cnt)
        '        If .Item("Result") <> "3" Then ''Not Grand Total
        '            If stnType <> .Item("stnType").ToString Then
        '                funcAddStuddedDetails(.Item("stnType").ToString, "", "", "", "", "", "", "", "", "", "T")
        '                rowStnType(indexStnStype) = dtStuddedDetails.Rows.Count - 1
        '                indexStnStype += 1
        '                stnType = .Item("stnType").ToString
        '            End If
        '        End If
        '        cPcs = Val(.Item("CPcs").ToString)
        '        cWeight = Format(Val(.Item("cWeight").ToString), "0.000")
        '        cAmount = Format(Val(.Item("cAmount").ToString), "0.00")
        '        gPcs = Val(.Item("gPcs").ToString)
        '        gWeight = Format(Val(.Item("gWeight").ToString), "0.000")
        '        gAmount = Format(Val(.Item("gAmount").ToString), "0.00")
        '        tPcs = Val(.Item("tPcs").ToString)
        '        tWeight = Format(Val(.Item("tWeight").ToString), "0.000")
        '        tAmount = Format(.Item("tAmount").ToString, "0.00")
        '        If .Item("Result") = "2" Then  ''SubTotal
        '            funcAddStuddedDetails(stnType + " TOTAL", cPcs, cWeight, cAmount, gPcs, gWeight, gAmount, tPcs, tWeight, tAmount, "S")
        '            rowSubTotal(indexSubTotal) = dtStuddedDetails.Rows.Count - 1
        '            indexSubTotal += 1

        '        ElseIf .Item("Result") = "3" Then ''Grand Total
        '            funcAddStuddedDetails("GRAND TOTAL", cPcs, cWeight, cAmount, gPcs, gWeight, gAmount, tPcs, tWeight, tAmount, "G")
        '        Else
        '            If stnItem <> .Item("ItemName").ToString Then
        '                If .Item("subItemName").ToString = "" Then
        '                    funcAddStuddedDetails(.Item("itemName").ToString, cPcs, cWeight, cAmount, gPcs, gWeight, gAmount, tPcs, tWeight, tAmount, "")
        '                Else
        '                    funcAddStuddedDetails(.Item("ItemName").ToString, "", "", "", "", "", "", "", "", "", "T")
        '                    funcAddStuddedDetails(.Item("subItemName").ToString, cPcs, cWeight, cAmount, gPcs, gWeight, gAmount, tPcs, tWeight, tAmount, "")
        '                End If
        '                stnItem = .Item("ItemName").ToString
        '            Else
        '                funcAddStuddedDetails(.Item("subItemName").ToString, cPcs, cWeight, cAmount, gPcs, gWeight, gAmount, tPcs, tWeight, tAmount, "")
        '            End If
        '        End If
        '    End With
        'Next
        gridStudDetails.DataSource = dt ' dtStuddedDetails
        With gridStudDetails
            .Columns("COLHEAD").Visible = False
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
            .Columns("RESULT").Visible = False
            .Columns("COLHEAD").Visible = False
        End With
    End Function

    Function funcgridHead() As Integer
        StrSql = "SELECT ''PARTICULAR,'CARAT'CARAT,'GRAM'GRAM,'TOTAL'TOTAL"
        dt = New DataTable
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(dt)
        With gridStudDetailsHead
            .DataSource = dt
            .Height = .ColumnHeadersHeight
            .Columns("PARTICULAR").Width = gridStudDetails.Columns("PARTICULAR").Width
            .Columns("CARAT").Width = gridStudDetails.Columns("CPCS").Width + gridStudDetails.Columns("CWEIGHT").Width + gridStudDetails.Columns("CAMOUNT").Width
            .Columns("GRAM").Width = gridStudDetails.Columns("GPCS").Width + gridStudDetails.Columns("GWEIGHT").Width + gridStudDetails.Columns("GAMOUNT").Width
            .Columns("TOTAL").Width = gridStudDetails.Columns("TPCS").Width + gridStudDetails.Columns("TWEIGHT").Width + gridStudDetails.Columns("TAMOUNT").Width
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            For CNT As Integer = 0 To .Columns.Count - 1
                .Columns(CNT).SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns(CNT).Resizable = DataGridViewTriState.False
            Next
            .RowHeadersVisible = False
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
            '.Height = .ColumnHeadersHeight
            .Height = 18
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
End Class
