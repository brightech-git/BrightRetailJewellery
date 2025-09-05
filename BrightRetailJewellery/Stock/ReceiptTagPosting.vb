Imports System.Data.OleDb
Imports System.Math
Imports System.IO

Public Class ReceiptTagPosting

    Dim SALVALUEROUND As Decimal = Val(GetAdmindbSoftValue("STKSALVALUEROUND", "0"))
    Public BillDate As Date = GetServerDate(tran)
    Dim cmd As OleDbCommand
    Dim objTag As New TagGeneration
    Dim objStone As New frmStoneDia
    Dim saStone As Boolean = False

    Dim dtGridReceipt As New DataTable
    Dim dtGridReceiptStone As New DataTable
    Dim dtGridRecTotal As New DataTable
    Dim rate As Double = 0
    Dim strSql As String = ""
    Dim OldGrsWt As Decimal = 0
    Dim Maxwastper As Decimal = 0
    Dim Maxwastage As Decimal = 0
    Dim MaxMcharge As Decimal = 0
    Dim MaxMcGrm As Decimal = 0
    Dim SalValue As Double = 0
    Dim OldSalValue As Double = 0
    Dim isssno As String = Nothing
    Dim TagPrefix As String = GetAdmindbSoftValue("TAGPREFIX", , tran)


    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        With dtGridReceipt
            .Columns.Add("TRANNO", GetType(Integer))
            .Columns.Add("SNO", GetType(String))
            .Columns.Add("ITEMID", GetType(Integer))
            .Columns.Add("ITEMNAME", GetType(String))
            .Columns.Add("SUBITEMID", GetType(Integer))
            .Columns.Add("SUBITEMNAME", GetType(String))
            .Columns.Add("DESIGNERID", GetType(Integer))
            .Columns.Add("DESIGNERNAME", GetType(String))
            .Columns.Add("PCS", GetType(Integer))
            .Columns.Add("GRSWT", GetType(Decimal))
            .Columns.Add("NETWT", GetType(Decimal))
            .Columns.Add("LESSWT", GetType(Decimal))
            .Columns.Add("TABLE", GetType(String))
            .Columns.Add("MAXWASTPER", GetType(Decimal))
            .Columns.Add("MAXWASTAGE", GetType(Decimal))
            .Columns.Add("MAXMCGRM", GetType(Decimal))
            .Columns.Add("MAXMCHARGE", GetType(Decimal))
            .Columns.Add("STNWT", GetType(Decimal))
            .Columns.Add("STNAMT", GetType(Decimal))
            .Columns.Add("PURITY", GetType(Decimal))
            .Columns.Add("RATE", GetType(Decimal))
            .Columns.Add("SALVALUE", GetType(Decimal))
            .Columns.Add("COSTID", GetType(String))
            .Columns.Add("ITEMTYPEID", GetType(Decimal))
            .Columns.Add("ITEMCTRID", GetType(Decimal))
            .Columns.Add("GRSNET", GetType(String))
        End With
        dtGridReceipt.AcceptChanges()
        gridview.DataSource = dtGridReceipt
        gridview.ColumnHeadersVisible = False
        FormatGridColumns(gridview)
        StyleGridTag(gridview)

        Dim dtGridRecTotal As New DataTable
        dtGridRecTotal = dtGridReceipt.Copy
        dtGridRecTotal.Rows.Clear()
        dtGridRecTotal.Rows.Add()
        gridViewTotal.ColumnHeadersVisible = False
        gridViewTotal.DataSource = dtGridRecTotal
        For Each col As DataGridViewColumn In gridview.Columns
            With gridViewTotal.Columns(col.Name)
                .Visible = col.Visible
                .Width = col.Width
                .DefaultCellStyle = col.DefaultCellStyle
            End With
        Next
        CalcGridTotal()
        StyleGridTag(gridViewTotal)
        txtTranno.Focus()
    End Sub

    Private Sub ReceiptTagPosting_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtTranno.Focused Then Exit Sub
            If txtItemId.Focused Then Exit Sub
            'If txtDesigner.Focused Then Exit Sub
            If txtPcs.Focused Then Exit Sub
            If txtGrsWt_WET.Focused Then Exit Sub
            If txtNetwt_Wet.Focused Then Exit Sub
            If txtLessWt_Wet.Focused Then Exit Sub
            If txtWastPer.Focused Then Exit Sub
            If txtWastage.Focused Then Exit Sub
            If txtMcGrm.Focused Then Exit Sub
            If txtMcharge.Focused Then Exit Sub
            If txtstnWt_Wet.Focused Then Exit Sub
            If txtStnAmt_amt.Focused Then Exit Sub
            If txtSalvalue_Amt.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub Cleardet()
        txtTranno.Clear()
        txtItemId.Clear()
        'txtDesigner.Clear()
        txtPcs.Clear()
        txtGrsWt_WET.Clear()
        txtNetwt_Wet.Clear()
        txtLessWt_Wet.Clear()
        txtWastPer.Clear()
        txtWastage.Clear()
        txtMcGrm.Clear()
        txtMcharge.Clear()
        txtstnWt_Wet.Clear()
        txtStnAmt_amt.Clear()
        txtSalvalue_Amt.Clear()
        isssno = Nothing
    End Sub

    Private Sub StyleGridTag(ByVal gridTagView As DataGridView)
        With gridTagView
            .Columns("TRANNO").Width = txtTranno.Width
            .Columns("ITEMID").Width = txtItemId.Width
            .Columns("PCS").Width = txtPcs.Width
            .Columns("GRSWT").Width = txtGrsWt_WET.Width
            .Columns("NETWT").Width = txtNetwt_Wet.Width
            .Columns("LESSWT").Width = txtLessWt_Wet.Width
            .Columns("MAXWASTPER").Width = txtWastPer.Width
            .Columns("MAXWASTAGE").Width = txtWastage.Width
            .Columns("MAXMCGRM").Width = txtMcGrm.Width
            .Columns("MAXMCHARGE").Width = txtMcharge.Width
            .Columns("SALVALUE").Width = txtSalvalue_Amt.Width
            .Columns("STNAMT").Width = txtStnAmt_amt.Width
            .Columns("STNWT").Width = txtstnWt_Wet.Width
            'For i As Integer = 15 To gridTagView.Columns.Count - 1
            '    .Columns(i).Visible = False
            'Next
            .Columns("SNO").Visible = False
            .Columns("ITEMNAME").Visible = False
            .Columns("SUBITEMID").Visible = False
            .Columns("SUBITEMNAME").Visible = False

            .Columns("DESIGNERNAME").Visible = False
            .Columns("DESIGNERID").Visible = False
            .Columns("TABLE").Visible = False
            .Columns("PURITY").Visible = False
            .Columns("RATE").Visible = False

            .Columns("COSTID").Visible = False
            .Columns("ITEMTYPEID").Visible = False
            .Columns("ITEMCTRID").Visible = False
            .Columns("GRSNET").Visible = False


        End With
    End Sub

    Private Sub CalcGridTotal()

        Dim Pcs As Integer = Nothing
        Dim GrsWt As Decimal = Nothing
        Dim NetWt As Decimal = Nothing
        Dim LessWt As Decimal = Nothing
        Dim wastage As Decimal = Nothing
        Dim mcharge As Decimal = Nothing
        Dim salevalue As Decimal = Nothing
        Dim stnwt As Decimal = Nothing
        Dim stnamt As Decimal = Nothing

        For i As Integer = 0 To gridview.RowCount - 1
            With gridview.Rows(i)
                .DefaultCellStyle.BackColor = SystemColors.HighlightText
                Pcs += Val(.Cells("PCS").Value.ToString)
                GrsWt += Val(.Cells("GRSWT").Value.ToString)
                NetWt += Val(.Cells("NETWT").Value.ToString)
                LessWt += Val(.Cells("LESSWT").Value.ToString)
                wastage += Val(.Cells("MAXWASTAGE").Value.ToString)
                mcharge += Val(.Cells("MAXMCHARGE").Value.ToString)
                stnwt += Val(.Cells("STNWT").Value.ToString)
                stnamt += Val(.Cells("STNAMT").Value.ToString)
                salevalue += Val(.Cells("SALVALUE").Value.ToString)

                .DefaultCellStyle.ForeColor = Color.Blue
                .DefaultCellStyle.SelectionForeColor = Color.Blue
            End With
        Next
        grpRecTagDetail.BackColor = SystemColors.InactiveCaption
        With gridViewTotal.Rows(0)
            .Cells("ITEMNAME").Value = "TOTAL"
            .Cells("PCS").Value = IIf(Pcs <> 0, Pcs, DBNull.Value)
            .Cells("GRSWT").Value = IIf(GrsWt <> 0, GrsWt, DBNull.Value)
            .Cells("NETWT").Value = IIf(NetWt <> 0, NetWt, DBNull.Value)
            .Cells("LESSWT").Value = IIf(LessWt <> 0, LessWt, DBNull.Value)
            .Cells("MAXWASTAGE").Value = IIf(wastage <> 0, wastage, DBNull.Value)
            .Cells("MAXMCHARGE").Value = IIf(mcharge <> 0, mcharge, DBNull.Value)
            .Cells("STNWt").Value = IIf(mcharge <> 0, mcharge, DBNull.Value)
            .Cells("STNAMT").Value = IIf(mcharge <> 0, mcharge, DBNull.Value)
            .Cells("SALVALUE").Value = IIf(salevalue <> 0, salevalue, DBNull.Value)

            .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            .DefaultCellStyle.ForeColor = Color.White
            .DefaultCellStyle.SelectionForeColor = Color.Blue
        End With

    End Sub
    Private Sub ReceiptTagPosting_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Cleardet()
        txtTranno.Focus()
    End Sub

    Private Sub txtTranno_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtTranno.KeyDown
        If e.KeyCode = Keys.Insert Then
            If e.KeyCode = Keys.Insert Then
                'If Val(txtTranno.Text) = 0 Then Exit Sub

                strSql = vbCrLf + " SELECT TRANNO,TRANDATE,ISNULL(A.ACNAME,'') ACNAME FROM  " & cnStockDb & "..RECEIPT R"
                strSql += vbCrLf + " LEFT JOIN  " & cnAdminDb & "..ACHEAD A ON A.ACCODE=R.ACCODE "
                strSql += vbCrLf + " WHERE R.TRANTYPE IN('RPU','RRE')"
                strSql += vbCrLf + " AND R.SNO NOT IN (SELECT DISTINCT  ISNULL(RECSNO,'') FROM " & cnAdminDb & "..ITEMTAG)"
                strSql += vbCrLf + " AND ISNULL(R.CANCEL,'')<>'Y'"
                txtTranno.Text = BrighttechPack.SearchDialog.Show("Find Tranno", strSql, cn, , , , , , , , False)
                txtTranno.SelectAll()

            End If
        ElseIf e.KeyCode = Keys.Down Then
            If gridview.Rows.Count = 0 Then txtTranno.Focus() : Exit Sub
            gridview.Focus()
        ElseIf e.KeyCode = Keys.Escape Then
            If gridview.Rows.Count = 0 Then txtTranno.Focus() : Exit Sub
            btnSave.Focus()
        End If
    End Sub

    Private Sub txtTranno_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtTranno.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtTranno.Text <> String.Empty Then
                For Each ro As DataRow In dtGridReceipt.Rows
                    If ro.Item("TRANNO").ToString = txtTranno.Text Then
                        MsgBox("This Bill Already Loaded", MsgBoxStyle.Information)
                        txtTranno.Clear()
                        Exit Sub
                    End If
                Next
                Dim dtTran As New DataTable
                strSql = vbCrLf + " SELECT TRANNO,SNO,R.ITEMID,IT.ITEMNAME,R.SUBITEMID,SI.SUBITEMNAME,TAGDESIGNER AS DESIGNER,D.DESIGNERNAME,PCS,R.GRSWT,NETWT"
                strSql += vbCrLf + " ,LESSWT,R.TABLECODE,WASTPER,WASTAGE,MCGRM,MCHARGE,PURITY,AMOUNT,RATE"
                strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO =R.SNO)STNWT"
                strSql += vbCrLf + " ,(SELECT SUM(STNAMT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO =R.SNO)STNAMT"
                strSql += vbCrLf + " ,MISCAMT,GRSNET,COSTID,ITEMTYPEID,ITEMCTRID  FROM " & cnStockDb & "..RECEIPT R"
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IT ON IT.ITEMID=R.ITEMID "
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.SUBITEMID = R.SUBITEMID "
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..DESIGNER D ON D.DESIGNERID =R.TAGDESIGNER "
                strSql += vbCrLf + " WHERE R.TRANNO='" & Val(txtTranno.Text) & "' AND TRANTYPE IN('RPU','RRE')"
                strSql += vbCrLf + " AND SNO NOT IN (SELECT DISTINCT  ISNULL(RECSNO,'') FROM " & cnAdminDb & "..ITEMTAG)"
                strSql += vbCrLf + " AND ISNULL(CANCEL,'')<>'Y'"
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtTran)
                Dim dtTranStone As New DataTable
                strSql = vbCrLf + " SELECT R.SNO,ISSSNO,R.TRANNO,RE.ITEMID,STNITEMID,IT.ITEMNAME AS ITEM,STNSUBITEMID,SI.SUBITEMNAME AS SUBITEM"
                strSql += vbCrLf + " ,STNPCS AS PCS,STNWT AS WEIGHT,STNRATE AS RATE,R.STNAMT AS AMOUNT,R.STONEUNIT AS UNIT,CALCMODE AS CALC,R.COSTID FROM " & cnStockDb & "..RECEIPTSTONE R"
                strSql += vbCrLf + " LEFT JOIN " & cnStockDb & "..RECEIPT RE ON RE.SNO=R.ISSSNO "
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IT ON IT.ITEMID=R.STNITEMID "
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.SUBITEMID = R.STNSUBITEMID  "
                strSql += vbCrLf + " WHERE R.TRANNO='" & Val(txtTranno.Text) & "' AND RE.TRANTYPE IN('RPU','RRE')"
                strSql += vbCrLf + " AND RE.SNO NOT IN (SELECT DISTINCT  ISNULL(RECSNO,'') FROM " & cnAdminDb & "..ITEMTAG)"
                strSql += vbCrLf + " AND ISNULL(RE.CANCEL,'')<>'Y'"
                'da = New OleDbDataAdapter(strSql, cn)
                'da.Fill(dtTranStone)
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(objStone.dtGridStone)
                objStone.dtGridStone.AcceptChanges()
                objStone.CalcStoneWtAmount()
                ' DisplayStoneWtTotal()
                If objStone.dtGridStone.Rows.Count > 0 Then
                    saStone = True
                Else
                    saStone = False
                End If

                If Not dtTran.Rows.Count > 0 Then
                    MsgBox("Record Not Found", , "Brighttech Gold")
                    txtTranno.Clear()
                    Exit Sub
                Else
                    LoadDetails(dtTran)
                    If dtTranStone.Rows.Count > 0 Then
                        dtGridReceiptStone.Merge(dtTranStone)
                    End If
                End If
                txtTranno.Clear()
            End If
        End If
    End Sub
    Private Sub LoadDetails(ByVal dtl As DataTable)
        'Dim dtl As DataTable = CalcMaxMinValues(dtRec)

        For ii As Integer = 0 To dtl.Rows.Count - 1
            Dim drl As DataRow
            drl = dtGridReceipt.NewRow
            drl("TRANNO") = dtl.Rows(ii).Item("TRANNO").ToString
            drl("SNO") = dtl.Rows(ii).Item("SNO").ToString
            drl("ITEMID") = dtl.Rows(ii).Item("ITEMID").ToString
            drl("ITEMNAME") = dtl.Rows(ii).Item("ITEMNAME").ToString
            drl("SUBITEMID") = dtl.Rows(ii).Item("SUBITEMID").ToString
            drl("SUBITEMNAME") = dtl.Rows(ii).Item("SUBITEMNAME").ToString
            drl("DESIGNERID") = Val(dtl.Rows(ii).Item("DESIGNER").ToString)
            drl("DESIGNERNAME") = dtl.Rows(ii).Item("DESIGNERNAME").ToString
            drl("PCS") = dtl.Rows(ii).Item("PCS").ToString
            drl("GRSWT") = dtl.Rows(ii).Item("GRSWT").ToString
            drl("NETWT") = dtl.Rows(ii).Item("NETWT").ToString
            drl("LESSWT") = dtl.Rows(ii).Item("LESSWT").ToString
            drl("TABLE") = dtl.Rows(ii).Item("TABLECODE").ToString
            CalcMaxMinValues(dtl, ii)
            drl("MAXWASTPER") = Maxwastper
            drl("MAXWASTAGE") = Maxwastage
            drl("MAXMCGRM") = MaxMcGrm
            drl("MAXMCHARGE") = MaxMcharge
            drl("STNWT") = Val(dtl.Rows(ii).Item("STNWT").ToString)
            drl("STNAMT") = Val(dtl.Rows(ii).Item("STNAMT").ToString)
            drl("PURITY") = dtl.Rows(ii).Item("PURITY").ToString
            drl("RATE") = rate
            drl("SALVALUE") = SalValue

            drl("COSTID") = dtl.Rows(ii).Item("COSTID").ToString
            drl("ITEMTYPEID") = Val(dtl.Rows(ii).Item("ITEMTYPEID").ToString)
            drl("ITEMCTRID") = Val(dtl.Rows(ii).Item("ITEMCTRID").ToString)
            drl("GRSNET") = dtl.Rows(ii).Item("GRSNET").ToString

            dtGridReceipt.Rows.Add(drl)
            Maxwastper = 0
            Maxwastage = 0
            MaxMcharge = 0
            MaxMcGrm = 0
            SalValue = 0
            rate = 0
        Next
        gridview.DataSource = dtGridReceipt
        CalcGridTotal()
        'gridview.Rows(0).DefaultCellStyle.SelectionBackColor = Color.Wheat
    End Sub

    Function CalcMaxMinValues(ByVal dtl As DataTable, ByVal ii As Integer)
        strSql = Nothing
        Dim type As String
        Dim tablecode As String
        Dim calcmode As String = dtl.Rows(ii).Item("GRSNET").ToString
        type = objGPack.GetSqlValue(" SELECT VALUEADDEDTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = '" & dtl.Rows(ii).Item("ITEMID").ToString & "'")
        Select Case type
            Case "T"
                If dtl.Rows(ii).Item("TABLECODE").ToString <> "" Then
                    tablecode = dtl.Rows(ii).Item("TABLECODE").ToString
                Else
                    strSql = " SELECT DISTINCT TABLECODE FROM " & cnAdminDb & "..WMCTABLE WHERE TABLECODE <> '' "
                    strSql += " AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = '" & dtl.Rows(ii).Item("COSTID").ToString & "'),'')"
                    strSql += " ORDER BY TABLECODE"
                    tablecode = objGPack.GetSqlValue(strSql)
                End If
                strSql = " DECLARE @WT FLOAT"
                If calcmode = "N" Then ''NET WT
                    strSql += vbCrLf + " SET @WT = " & Val(dtl.Rows(ii).Item("NETWT").ToString) & ""
                Else ''GRS WT
                    strSql += vbCrLf + " SET @WT = " & Val(dtl.Rows(ii).Item("GRSWT").ToString) & ""
                End If
                strSql += vbCrLf + " SELECT TOUCH,MAXWASTPER,MAXMCGRM,MAXWAST,MAXMC,TOUCH_PUR,MAXWASTPER_PUR,MAXMCGRM_PUR,MAXWAST_PUR,MAXMC_PUR"
                strSql += vbCrLf + " ,MINWASTPER,MINMCGRM,MINWAST,MINMC FROM " & cnAdminDb & "..WMCTABLE "
                strSql += vbCrLf + " WHERE TABLECODE = '" & tablecode & "'"
                strSql += vbCrLf + " AND @WT BETWEEN FROMWEIGHT AND TOWEIGHT"
                strSql += vbCrLf + " AND ISNULL(ACCODE,'') = ''"
                strSql += vbCrLf + " AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = '" & dtl.Rows(ii).Item("COSTID").ToString & "'),'')"
            Case "I"
                strSql = " DECLARE @WT FLOAT"
                If calcmode = "N" Then ''NET WT
                    strSql += vbCrLf + " SET @WT = " & Val(dtl.Rows(ii).Item("NETWT").ToString) & ""
                Else ''GRS WT
                    strSql += vbCrLf + " SET @WT = " & Val(dtl.Rows(ii).Item("GRSWT").ToString) & ""
                End If
                strSql += vbCrLf + " SELECT TOUCH,MAXWASTPER,MAXMCGRM,MAXWAST,MAXMC,TOUCH_PUR,MAXWASTPER_PUR,MAXMCGRM_PUR,MAXWAST_PUR,MAXMC_PUR"
                strSql += vbCrLf + " ,MINWASTPER,MINMCGRM,MINWAST,MINMC FROM " & cnAdminDb & "..WMCTABLE "
                strSql += vbCrLf + " WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = '" & dtl.Rows(ii).Item("ITEMID").ToString & "')"
                strSql += vbCrLf + " AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = '" & dtl.Rows(ii).Item("SUBITEMID").ToString & "' AND ITEMID = ( SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = '" & dtl.Rows(ii).Item("ITEMID").ToString & "')),0)"
                strSql += vbCrLf + " AND @WT BETWEEN FROMWEIGHT AND TOWEIGHT"
                strSql += vbCrLf + " AND ISNULL(ACCODE,'') = ''"
                strSql += vbCrLf + " AND ISNULL(TABLECODE,'') = ''"
                strSql += vbCrLf + " AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = '" & dtl.Rows(ii).Item("COSTID").ToString & "'),'')"
            Case "D"
                strSql = " DECLARE @WT FLOAT"
                If calcmode = "N" Then ''NET WT
                    strSql += vbCrLf + " SET @WT = " & Val(dtl.Rows(ii).Item("NETWT").ToString) & ""
                Else ''GRS WT
                    strSql += vbCrLf + " SET @WT = " & Val(dtl.Rows(ii).Item("GRSWT").ToString) & ""
                End If
                strSql += vbCrLf + " SELECT TOUCH,MAXWASTPER,MAXMCGRM,MAXWAST,MAXMC,TOUCH_PUR,MAXWASTPER_PUR,MAXMCGRM_PUR,MAXWAST_PUR,MAXMC_PUR"
                strSql += vbCrLf + " ,MINWASTPER,MINMCGRM,MINWAST,MINMC FROM " & cnAdminDb & "..WMCTABLE "
                strSql += vbCrLf + " WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = '" & dtl.Rows(ii).Item("ITEMID").ToString & "')"
                strSql += vbCrLf + " AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = '" & dtl.Rows(ii).Item("SUBITEMID").ToString & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = '" & dtl.Rows(ii).Item("ITEMID").ToString & "')),0)"
                strSql += vbCrLf + " AND DESIGNERID = ISNULL((SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = '" & dtl.Rows(ii).Item("DESIGNER").ToString & "'),0)"
                strSql += vbCrLf + " AND @WT BETWEEN FROMWEIGHT AND TOWEIGHT"
                strSql += vbCrLf + " AND ISNULL(ACCODE,'') = ''"
                strSql += vbCrLf + " AND ISNULL(TABLECODE,'') = ''"
                strSql += " AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = '" & dtl.Rows(ii).Item("COSTID").ToString & "'),'')"
            Case "P"
                strSql = " DECLARE @WT FLOAT"
                If calcmode = "N" Then ''NET WT
                    strSql += vbCrLf + " SET @WT = " & Val(dtl.Rows(ii).Item("NETWT").ToString) & ""
                Else ''GRS WT
                    strSql += vbCrLf + " SET @WT = " & Val(dtl.Rows(ii).Item("GRSWT").ToString) & ""
                End If
                strSql += vbCrLf + " SELECT TOUCH,MAXWASTPER,MAXMCGRM,MAXWAST,MAXMC,TOUCH_PUR,MAXWASTPER_PUR,MAXMCGRM_PUR,MAXWAST_PUR,MAXMC_PUR"
                strSql += vbCrLf + " ,MINWASTPER,MINMCGRM,MINWAST,MINMC FROM " & cnAdminDb & "..WMCTABLE "
                strSql += vbCrLf + " WHERE ITEMTYPE = (SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = '" & dtl.Rows(ii).Item("ITEMTYPEID").ToString & "')"
                strSql += vbCrLf + " AND @WT BETWEEN FROMWEIGHT AND TOWEIGHT"
                strSql += vbCrLf + " AND ISNULL(ACCODE,'') = ''"
                strSql += vbCrLf + " AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = '" & dtl.Rows(ii).Item("COSTID").ToString & "'),'')"
        End Select
        If type = Nothing Then
            GoTo CALCSALVALTE
        End If
        Dim dt As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            GoTo CALCSALVALTE
        Else
            With dt.Rows(0)
                Dim wmcWastPer As Double = Val(.Item("MAXWASTPER").ToString)
                Dim wmcWast As Double = Val(.Item("MAXWAST").ToString)
                Dim wmcMcGrm As Double = Val(.Item("MAXMCGRM").ToString)
                Dim wmcMc As Double = Val(.Item("MAXMC").ToString)

                If wmcWastPer = 0 Then
                    Maxwastage = IIf(Val(.Item("MAXWAST").ToString) <> 0, Format(Val(.Item("MAXWAST").ToString), "0.000"), 0)

                Else
                    Maxwastper = IIf(Val(.Item("MAXWASTPER").ToString) <> 0, Format(Val(.Item("MAXWASTPER").ToString), "0.00"), 0)
                    If calcmode = "N" Then ''NET WT
                        Maxwastage = (Val(dtl.Rows(ii).Item("NETWT").ToString) / 100) * Maxwastper
                    Else ''GRS WT
                        Maxwastage = (Val(dtl.Rows(ii).Item("GRSWT").ToString) / 100) * Maxwastper
                    End If
                End If
                If wmcMcGrm = 0 Then
                    MaxMcharge = IIf(Val(.Item("MAXMC").ToString) <> 0, Format(Val(.Item("MAXMC").ToString), "0.00"), 0)
                Else
                    MaxMcGrm = IIf(Val(.Item("MAXMCGRM").ToString) <> 0, Format(Val(.Item("MAXMCGRM").ToString), "0.00"), 0)
                    If calcmode = "N" Then ''NET WT
                        MaxMcharge = (Val(dtl.Rows(ii).Item("NETWT").ToString) * MaxMcGrm)
                    Else ''GRS WT
                        MaxMcharge = (Val(dtl.Rows(ii).Item("GRSWT").ToString) * MaxMcGrm)
                    End If
                End If
            End With
        End If
CALCSALVALTE:
        'calculate salvalue
        Dim calType As String = Nothing
        If dtl.Rows(ii).Item("SUBITEMNAME").ToString <> "" Then
            strSql = " SELECT CALTYPE FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & dtl.Rows(ii).Item("SUBITEMNAME").ToString & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & dtl.Rows(ii).Item("ITEMNAME").ToString & "')"
        Else
            strSql = " SELECT CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & dtl.Rows(ii).Item("ITEMNAME").ToString & "'"
        End If
        calType = objGPack.GetSqlValue(strSql)

        Dim amt As Double = Nothing

        If Val(dtl.Rows(ii).Item("RATE").ToString) > 0 Then
            rate = Format(Val(dtl.Rows(ii).Item("RATE").ToString), "0.00")
        Else
            rate = Format(GetMetalRate(Val(dtl.Rows(ii).Item("ITEMTYPEID").ToString), dtl.Rows(ii).Item("ITEMNAME").ToString), "0.00")
        End If
        If calType = "R" Then
            amt = Val(dtl.Rows(ii).Item("PCS").ToString) * Val(rate)
        Else
            Dim wt As Double = 0
            If calcmode = "N" Then ''NET WT
                wt = Val(dtl.Rows(ii).Item("NETWT").ToString)
            Else ''GRS WT
                wt = Val(dtl.Rows(ii).Item("GRSWT").ToString)
            End If

            amt = ((wt + Val(Maxwastage)) * rate) _
            + Val(MaxMcharge) _
                    + Val(dtl.Rows(ii).Item("STNAMT").ToString) _
            + Val(dtl.Rows(ii).Item("MISCAMT").ToString)
            amt += IIf(calType = "B", Val(rate), 0)
            amt += IIf(calType = "F", Val(rate), 0)
        End If
        amt = Math.Round(amt)
        SalValue = IIf(amt <> 0, Format(SALEVALUE_ROUND(amt), "0.00"), 0)

    End Function
    Private Function SALEVALUE_ROUND(ByVal svalue As Decimal) As Decimal
        If SALVALUEROUND <> 0 Then
            If svalue <> 0 Then
                Dim wholepart As Decimal = Val(svalue) / SALVALUEROUND
                Dim intpart As Decimal = Int(wholepart)
                Dim decpart As Decimal = Round(wholepart - intpart)
                svalue = (intpart + decpart) * SALVALUEROUND
            End If
        End If
        Return svalue
    End Function
    '    Private Sub CalcSaleValue()
    '        Dim amt As Double = Nothing
    '        'If calType = "F" Then Exit Sub
    '        If calType = "R" Then
    '            amt = Val(txtPieces_Num_Man.Text) * Val(txtRate_Amt.Text)
    '        Else
    '            Dim wt As Double = 0
    '            Dim rate As Double = IIf(calType = "M", Val(txtRate_Amt.Text), Val(txtMetalRate_Amt.Text))
    '            If cmbCalcMode.SelectedIndex = 0 Then ''GRS WT
    '                wt = Val(txtGrossWt_Wet.Text)
    '            Else ''NET WT
    '                wt = Val(txtNetWt_Wet.Text)
    '            End If
    '            If Val(txtTouch_AMT.Text) > 0 And Val(txtMaxWastage_Wet.Text) = 0 Then
    '                wt = (wt * Val(txtTouch_AMT.Text)) / 100
    '            End If
    '            If TabControl1.Contains(tabMultiMetal) And multiMetalCalc Then
    '                amt = 0
    '                If GetSoftValue("MULTIMETALCALC") = "N" Then
    '                    GoTo WegithCalc
    '                End If
    '                For Each ro As DataRow In dtMultiMetalDetails.Rows
    '                    If Not Val(ro!AMOUNT.ToString) > 0 Then
    '                        amt += (Val(ro!WEIGHT.ToString) + Val(ro!WASTAGE.ToString)) * Val(ro!RATE.ToString)
    '                        amt += Val(ro!MC.ToString)
    '                    End If
    '                    amt += Val(ro!AMOUNT.ToString)
    '                Next
    '                amt += Val(lblDiaAmount.Text) + Val(lblStnAmount.Text) + Val(lblPreAmount.Text) _
    '                + Val(txtMiscAmt.Text)
    '            Else
    'WegithCalc:
    '                amt = ((wt + Val(txtMaxWastage_Wet.Text)) * rate) _
    '                + Val(txtMaxMkCharge_Amt.Text) _
    '                + Val(txtMultiAmt.Text) _
    '                + Val(lblDiaAmount.Text) + Val(lblStnAmount.Text) + Val(lblPreAmount.Text) _
    '                + Val(txtMiscAmt.Text)
    '                amt += IIf(calType = "B", Val(txtRate_Amt.Text), 0)
    '                amt += IIf(calType = "F", Val(txtRate_Amt.Text), 0)
    '            End If
    '        End If
    '        amt = Math.Round(amt)
    '        txtSalValue_Amt_Man.Text = IIf(amt <> 0, Format(SALEVALUE_ROUND(amt), "0.00"), "")
    '        If SALEVALUEPLUS <> 0 Then txtSalValue_Amt_Man.Text = Val(txtSalValue_Amt_Man.Text) * SALEVALUEPLUS
    '        ObjPurDetail.CalcPurchaseValue()
    '        txtPurchaseValue_Amt.Text = ObjPurDetail.txtPurPurchaseVal_Amt.Text
    '    End Sub
    Private Function GetMetalRate(ByVal itemtypeid As Integer, ByVal itemname As String) As Double
        Dim purityId As String = Nothing
        ''objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = " & saItemTypeId & " AND RATEGET = 'Y'", , )
        If itemtypeid <> 0 Then
            purityId = objGPack.GetSqlValue("SELECT PURITYID FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = '" & itemtypeid & "' AND RATEGET = 'Y' AND SOFTMODULE = 'S'", , )
        End If
        If Not Trim(purityId).Length > 0 Then
            purityId = objGPack.GetSqlValue("SELECT PURITYID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = (SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & itemname.ToString & "')")
        End If
        If purityId = "" Then Return 0
        Dim metalId As String = objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = " & purityId & "")

        Dim rate As Double = Nothing
        strSql = " SELECT TOP 1 SRATE FROM " & cnAdminDb & "..RATEMAST AS M"
        strSql += " WHERE RDATE = '" & BillDate & "'"
        strSql += " AND RATEGROUP = (SELECT MAX(RATEGROUP) FROM " & cnAdminDb & "..RATEMAST WHERE RDATE = M.RDATE)"
        strSql += " AND METALID = '" & metalId & "'"
        strSql += " AND PURITY = (SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = '" & purityId & "')"
        strSql += " ORDER BY SNO DESC"
        rate = Val(objGPack.GetSqlValue(strSql, , , tran))
        If IsDate(BillDate) Then
            Return rate
        Else
            Return 0
        End If
    End Function

    Private Function GetSoftValue(ByVal id As String) As String
        Return UCase(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = '" & id & "'", , "", tran))
    End Function

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        fnew()
    End Sub
    Function fnew()
        dtGridReceipt.Clear()
        dtGridReceiptStone.Clear()
        dtGridRecTotal.Clear()
        objStone.dtGridStone.Clear()
        ClearDet()
        CalcGridTotal()
        txtTAGRowIndex.Text = ""
        txtTranno.Focus()
    End Function


    Private Sub gridview_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridview.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            gridview.CurrentCell = gridview.Rows(gridview.CurrentRow.Index).Cells("TRANNO")
            gridview.CurrentRow.DefaultCellStyle.SelectionBackColor = Color.White
            gridview.CurrentRow.DefaultCellStyle.SelectionForeColor = Color.Black
            Dim rwIndex As Integer = gridview.CurrentRow.Index
            With gridview.Rows(rwIndex)
                txtTranno.Text = .Cells("TRANNO").FormattedValue
                txtItemId.Text = .Cells("ITEMID").FormattedValue
                'txtItemname.Text = .Cells("ITEMNAME").FormattedValue
                'txtSubItemId.Text = .Cells("SUBITEMID").FormattedValue
                'txtSubItemName.Text = .Cells("SUBITEMNAME").FormattedValue
                'txtDesigner.Text = .Cells("DESIGNERNAME").FormattedValue
                txtPcs.Text = .Cells("PCS").FormattedValue
                txtGrsWt_WET.Text = .Cells("GRSWT").FormattedValue
                OldGrsWt = Val(.Cells("GRSWT").FormattedValue.ToString)
                txtNetwt_Wet.Text = .Cells("NETWT").FormattedValue
                txtLessWt_Wet.Text = .Cells("LESSWT").FormattedValue
                txtWastPer.Text = .Cells("MAXWASTPER").FormattedValue
                txtWastage.Text = .Cells("MAXWASTAGE").FormattedValue
                txtMcGrm.Text = .Cells("MAXMCGRM").FormattedValue
                txtMcharge.Text = .Cells("MAXMCHARGE").FormattedValue
                txtstnWt_Wet.Text = .Cells("STNWT").FormattedValue
                txtStnAmt_amt.Text = .Cells("STNAMT").FormattedValue
                txtSalvalue_Amt.Text = .Cells("SALVALUE").FormattedValue
                OldSalValue = Val(.Cells("SALVALUE").FormattedValue)
                isssno = .Cells("SNO").FormattedValue
                CalcGridTotal()
                txtTAGRowIndex.Text = rwIndex
                txtPcs.Focus()
                'Sno = .Cells("SNO").FormattedValue

                GrpDetail.Visible = True
                txt_Itemname.Text = .Cells("ITEMNAME").FormattedValue
                txt_SubItemname.Text = .Cells("SUBITEMNAME").FormattedValue
                txt_DesignerName.Text = .Cells("DESIGNERNAME").FormattedValue
                txt_SubItemname.Text = .Cells("TABLE").FormattedValue
                txt_Purity.Text = .Cells("PURITY").FormattedValue
            End With
        ElseIf e.KeyCode = Keys.Escape Then
            btnSave.Focus()
        ElseIf e.KeyCode = Keys.Up Then
            If gridview.CurrentRow.Index = 0 Then txtTranno.Focus()
        End If
    End Sub

    Private Sub txtSalvalue_Amt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtSalvalue_Amt.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            dtGridReceipt.AcceptChanges()
            With dtGridReceipt.Rows(Val(txtTAGRowIndex.Text))
                .Item("TRANNO") = Val(txtTranno.Text)
                .Item("ITEMID") = Val(txtItemId.Text)
                '.Item("DESIGNERNAME") = txtDesigner.Text
                .Item("PCS") = IIf(Val(txtPcs.Text) > 0, txtPcs.Text, DBNull.Value)
                .Item("GRSWT") = IIf(Val(txtGrsWt_WET.Text) > 0, Val(txtGrsWt_WET.Text), DBNull.Value)
                .Item("NETWT") = IIf(Val(txtNetwt_Wet.Text) > 0, Val(txtNetwt_Wet.Text), DBNull.Value)
                .Item("LESSWT") = IIf(Val(txtLessWt_Wet.Text) > 0, Val(txtLessWt_Wet.Text), DBNull.Value)
                .Item("MAXWASTPER") = IIf(Val(txtWastPer.Text) > 0, Val(txtWastPer.Text), DBNull.Value)
                .Item("MAXWASTAGE") = IIf(Val(txtWastage.Text) > 0, Val(txtWastage.Text), DBNull.Value)
                .Item("MAXMCGRM") = IIf(Val(txtMcGrm.Text) > 0, Val(txtMcGrm.Text), DBNull.Value)
                .Item("MAXMCHARGE") = IIf(Val(txtMcharge.Text) > 0, Val(txtMcharge.Text), DBNull.Value)
                .Item("STNWT") = IIf(Val(txtstnWt_Wet.Text) > 0, Val(txtstnWt_Wet.Text), DBNull.Value)
                .Item("STNAMT") = IIf(Val(txtStnAmt_amt.Text) > 0, Val(txtStnAmt_amt.Text), DBNull.Value)
                .Item("SALVALUE") = IIf(Val(txtSalvalue_Amt.Text) > 0, Val(txtSalvalue_Amt.Text), DBNull.Value)
            End With
            GrpDetail.Visible = False
            CalcGridTotal()
            Cleardet()
            gridview.Focus()
        End If
    End Sub


    Function funcAdd() As Integer

        Dim TagSno As String
        Dim TagNo As String
        Dim tagVal As String
        Dim LotSno As String
        Dim calType As String = Nothing
        Dim mlwmctype As String

        Dim Ptagnos As String = Nothing

        Dim roselected As New DataTable
        With roselected
            .Columns.Add("ITEMID", GetType(Integer))
            .Columns.Add("TAGNO", GetType(String))
        End With

        Try

            tran = Nothing
            tran = cn.BeginTransaction()
            ''Find TagSno

TagDupGen:

            For ij As Integer = 0 To dtGridReceipt.Rows.Count - 1

                If dtGridReceipt.Rows(ij).Item("SUBITEMNAME").ToString <> "" Then
                    strSql = " SELECT CALTYPE FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & dtGridReceipt.Rows(ij).Item("SUBITEMNAME").ToString & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & dtGridReceipt.Rows(ij).Item("ITEMNAME").ToString & "')"
                Else
                    strSql = " SELECT CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & dtGridReceipt.Rows(ij).Item("ITEMNAME").ToString & "'"
                End If
                calType = objGPack.GetSqlValue(strSql, , , tran)

                LotSno = objGPack.GetSqlValue(" SELECT LOTSNO FROM " & cnStockDb & "..LOTISSUE WHERE TRANNO='" & dtGridReceipt.Rows(ij).Item("TRANNO").ToString & "' AND RECSNO='" & dtGridReceipt.Rows(ij).Item("SNO").ToString & "'", , , tran)
                TagSno = GetNewSno(TranSnoType.ITEMTAGCODE, tran, "GET_ADMINSNO_TRAN")
                TagNo = objTag.GetTagNo(BillDate.ToString("yyyy-MM-dd"), dtGridReceipt.Rows(ij).Item("ITEMNAME").ToString, LotSno, tran)
                'tagVal = (Val(dtGridReceipt.Rows(ij).Item("ITEMID").ToString) & TagNo).ToString
                tagVal = TagNo.ToString
                If tagVal.ToUpper.Contains(TagPrefix.ToUpper) Then
                    tagVal = Mid(tagVal, TagPrefix.Length + 1, tagVal.Length)
                End If

                mlwmctype = objGPack.GetSqlValue(" SELECT VALUEADDEDTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = '" & dtGridReceipt.Rows(ij).Item("ITEMID").ToString & "'", , , tran)
                ''INSERTING ITEMTAG
                strSql = " INSERT INTO " & cnAdminDb & "..ITEMTAG"
                strSql += " ("
                strSql += " SNO,RECDATE,COSTID,ITEMID,ORDREPNO,ORSNO,"
                strSql += " ORDSALMANCODE,SUBITEMID,SIZEID,ITEMCTRID,TABLECODE,DESIGNERID,"
                strSql += " TAGNO,PCS,GRSWT,"
                strSql += " LESSWT,NETWT,RATE,FINERATE,MAXWASTPER,MAXMCGRM,"
                strSql += " MAXWAST,MAXMC,MINWASTPER,MINMCGRM,MINWAST,MINMC,"
                strSql += " TAGKEY,TAGVAL,LOTSNO,COMPANYID,SALVALUE,PURITY,NARRATION,DESCRIP,"
                strSql += " REASON,ENTRYMODE,GRSNET,"
                strSql += " ISSDATE,ISSREFNO,ISSPCS,ISSWT,FROMFLAG,TOFLAG,APPROVAL,SALEMODE,"
                strSql += " BATCHNO,MARK,"
                strSql += " VATEXM,PCTFILE,OLDTAGNO,ITEMTYPEID,ACTUALRECDATE,WEIGHTUNIT,"
                strSql += " TRANSFERWT,CHKDATE,CHKTRAY,CARRYFLAG,BRANDID,PRNFLAG,"
                strSql += " MCDISCPER,WASTDISCPER,RESDATE,TRANINVNO,SUPBILLNO,WORKDAYS,"
                strSql += " USERID,UPDATED,UPTIME,SYSTEMID,STYLENO,APPVER,TRANSFERDATE,"
                strSql += " BOARDRATE,RFID,TOUCH,HM_BILLNO,HM_CENTER,ADD_VA_PER,REFVALUE,VALUEADDEDTYPE,TCOSTID,EXTRAWT,"
                strSql += " USRATE,INDRS,"
                strSql += " RECSNO) VALUES("

                strSql += " '" & TagSno & "'" 'SNO
                strSql += " ,'" & GetEntryDate(BillDate, tran).ToString("yyyy-MM-dd") & "'" 'RECDATE 'dtpRecieptDate.Value.Date.ToString("yyyy-MM-dd")
                strSql += " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, dtGridReceipt.Rows(ij).Item("COSTID").ToString) & "'" 'COSTID
                strSql += " ," & Val(dtGridReceipt.Rows(ij).Item("ITEMID").ToString) & "" 'ITEMID
                strSql += " ,''" 'ORDREPNO
                strSql += " ,''" 'ORsno
                strSql += " ,''" 'ORDSALMANCODE
                strSql += " ," & Val(dtGridReceipt.Rows(ij).Item("SUBITEMID").ToString) & "" 'SUBITEMID
                strSql += " ,0" 'SIZEID
                strSql += " ," & dtGridReceipt.Rows(ij).Item("ITEMCTRID").ToString & "" 'ITEMCTRID
                strSql += " ,'" & dtGridReceipt.Rows(ij).Item("TABLE").ToString & "'"
                'If cmbTableCode.Enabled = True Then 'TABLECODE

                'Else
                '    strSql += " ,''"
                'End If
                strSql += " ," & Val(dtGridReceipt.Rows(ij).Item("DESIGNERID").ToString) & "" 'DESIGNERID
                strSql += " ,'" & TagNo & "'" 'TAGNO
                strSql += " ," & Val(dtGridReceipt.Rows(ij).Item("PCS").ToString) & "" 'PCS
                strSql += " ," & Val(dtGridReceipt.Rows(ij).Item("GRSWT").ToString) & "" 'GRSWT
                strSql += " ," & Val(dtGridReceipt.Rows(ij).Item("LESSWT").ToString) & "" 'LESSWT
                strSql += " ," & Val(dtGridReceipt.Rows(ij).Item("NETWT").ToString) & "" 'NETWT
                strSql += " ," & Val(dtGridReceipt.Rows(ij).Item("RATE").ToString) & "" 'RATE
                strSql += ",0" 'FINERATE
                strSql += " ," & Val(dtGridReceipt.Rows(ij).Item("MAXWASTPER").ToString) & "" 'MAXWASTPER
                strSql += " ," & Val(dtGridReceipt.Rows(ij).Item("MAXMCGRM").ToString) & "" 'MAXMCGRM
                strSql += " ," & Val(dtGridReceipt.Rows(ij).Item("MAXWASTAGE").ToString) & "" 'MAXWAST
                strSql += " ," & Val(dtGridReceipt.Rows(ij).Item("MAXMCHARGE").ToString) & "" 'MAXMC
                strSql += " ," & 0 & "" 'MINWASTPER
                strSql += " ," & 0 & "" 'MINMCGRM
                strSql += " ," & 0 & "" 'MINWAST
                strSql += " ," & 0 & "" 'MINMC
                strSql += " ,'" & dtGridReceipt.Rows(ij).Item("ITEMID").ToString & "" & TagNo & "'" 'TAGKEY
                strSql += " ," & tagVal & "" 'TAGVAL
                strSql += " ,'" & LotSno & "'" 'LOTSNO
                strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                strSql += " ," & Val(dtGridReceipt.Rows(ij).Item("SALVALUE").ToString) & "" 'SALVALUE
                strSql += " ," & Val(dtGridReceipt.Rows(ij).Item("PURITY").ToString) & "" 'PURITY
                strSql += " ,''" 'NARRATION
                If dtGridReceipt.Rows(ij).Item("SUBITEMNAME").ToString <> "" Then 'DESCRIP
                    strSql += " ,'" & dtGridReceipt.Rows(ij).Item("SUBITEMNAME").ToString & "'"
                Else
                    strSql += " ,'" & dtGridReceipt.Rows(ij).Item("ITEMNAME").ToString & "'"
                End If
                strSql += " ,''" 'REASON
                strSql += " ,'M'"
                strSql += " ,'" & dtGridReceipt.Rows(ij).Item("GRSNET").ToString & "'" 'GRSNET
                strSql += " ,NULL" 'ISSDATE
                strSql += " ,0" 'ISSREFNO
                strSql += " ,0" 'ISSPCS
                strSql += " ,0" 'ISSWT
                strSql += " ,''" 'FROMFLAG
                strSql += " ,''" 'TOFLAG
                strSql += " ,''" 'APPROVAL
                strSql += " ,'" & calType & "'" 'SALEMODE
                strSql += " ,''" 'BATCHNO
                strSql += " ,0" 'MARK
                strSql += " ,''" 'VATEXM
                strSql += " ,''" ' pctfile
                'strSql += " ,'" & IIf(picPath <> Nothing, "L" + txtLotNo_Num_Man.Text + "I" + itemId.ToString + "T" + TagNo.Replace(":", "") + "." + picExtension.ToString, picPath) & "'" 'PCTFILE
                strSql += " ,''" 'OLDTAGNO
                strSql += " ," & Val(dtGridReceipt.Rows(ij).Item("ITEMTYPEID").ToString) & "" 'ITEMTYPEID
                strSql += " ,'" & GetEntryDate(BillDate, tran).ToString("yyyy-MM-dd") & "'" 'ACTUALRECDATE
                strSql += " ,''" 'WEIGHTUNIT
                strSql += " ,0" 'TRANSFERWT
                strSql += " ,NULL" 'CHKDATE
                strSql += " ,''" 'CHKTRAY
                strSql += " ,''" 'CARRYFLAG
                strSql += " ,''" 'BRANDID
                strSql += " ,''" 'PRNFLAG
                strSql += " ,0" 'MCDISCPER
                strSql += " ,0" 'WASTDISCPER
                strSql += " ,NULL" 'RESDATE
                strSql += " ,''" 'TRANINVNO
                strSql += " ,''" 'SUPBILLNO
                strSql += " ,''" 'WORKDAYS
                strSql += " ," & userId & "" 'USERID
                strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                strSql += " ,'" & systemId & "'" 'SYSTEMID
                strSql += " ,''" 'STYLENO
                strSql += " ,'" & VERSION & "'" 'APPVER
                strSql += " ,'" & GetEntryDate(BillDate, tran).ToString("yyyy-MM-dd") & "'" 'TRANSFERDATE
                strSql += " ," & Val(dtGridReceipt.Rows(ij).Item("RATE").ToString) & ""
                strSql += " ,''"
                strSql += " ,0"
                strSql += " ,''" 'HM_BILLNO
                strSql += " ,''" 'HM_CENTER
                strSql += " ,0" 'ADD_VA_PER
                strSql += " ,0" 'REFVALUE
                strSql += " ,'" & mlwmctype & "'"
                strSql += " ,'" & dtGridReceipt.Rows(ij).Item("COSTID").ToString & "'" 'TCOSTID
                strSql += " ,0" 'EXTRAWT
                strSql += " ,0"
                strSql += " ,0"
                strSql += " ,'" & dtGridReceipt.Rows(ij).Item("SNO").ToString & "'" 'RECSNO
                strSql += " )"
                cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()

                strSql = " UPDATE " & cnAdminDb & "..ITEMLOT SET CPCS = CPCS + " & Val(dtGridReceipt.Rows(ij).Item("PCS").ToString) & ""
                strSql += " ,CGRSWT = CGRSWT + " & Val(dtGridReceipt.Rows(ij).Item("GRSWT").ToString) & ""
                strSql += " ,CNETWT = ISNULL(CNETWT,0) + " & Val(dtGridReceipt.Rows(ij).Item("NETWT").ToString) & ""
                strSql += " WHERE SNO = '" & LotSno & "'"
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.ExecuteNonQuery()
                Dim tagprnrow As DataRow
                tagprnrow = roselected.NewRow
                tagprnrow("ITEMID") = dtGridReceipt.Rows(ij).Item("ITEMID").ToString
                tagprnrow("TAGNO") = TagNo
                roselected.Rows.Add(tagprnrow)

                If Ptagnos = Nothing Then
                    Ptagnos = TagNo
                Else
                    Ptagnos = Ptagnos & "," & TagNo
                End If
            Next


            ''Inserting StoneDetail
            For cnt As Integer = 0 To objStone.dtGridStone.Rows.Count - 1
                With objStone.dtGridStone.Rows(cnt)
                    Dim stnItemId As Integer = 0
                    Dim stnSubItemId As Integer = 0
                    strSql = " SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE RECSNO = '" & .Item("ISSSNO").ToString & "'"
                    Dim stnTagSno As String = objGPack.GetSqlValue(strSql, "SNO", , tran)
                    strSql = " SELECT TAGNO FROM " & cnAdminDb & "..ITEMTAG WHERE RECSNO = '" & .Item("ISSSNO").ToString & "'"
                    Dim stnTagno As String = objGPack.GetSqlValue(strSql, "TAGNO", , tran)
                    Dim stnSno As String = GetNewSno(TranSnoType.ITEMTAGSTONECODE, tran, "GET_ADMINSNO_TRAN")
                    'Dim caType As String = Nothing
                    strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Item("ITEM").ToString & "'"
                    stnItemId = Val(objGPack.GetSqlValue(strSql, "ITEMID", , tran))
                    strSql = " SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & .Item("SUBITEM").ToString & "' AND ITEMID = " & stnItemId & ""
                    stnSubItemId = Val(objGPack.GetSqlValue(strSql, , , tran))
                    '.Item("STNSNO") = stnSno
                    ''Inserting itemTagStone
                    strSql = " INSERT INTO " & cnAdminDb & "..ITEMTAGSTONE("
                    strSql += " SNO,TAGSNO,ITEMID,COMPANYID,STNITEMID,"
                    strSql += " STNSUBITEMID,TAGNO,STNPCS,STNWT,"
                    strSql += " STNRATE,STNAMT,DESCRIP,"
                    strSql += " RECDATE,CALCMODE,"
                    strSql += " MINRATE,SIZECODE,STONEUNIT,ISSDATE,"
                    strSql += " OLDTAGNO,VATEXM,CARRYFLAG,COSTID,SYSTEMID,APPVER,"
                    strSql += " USRATE,INDRS"
                    strSql += " )VALUES("
                    strSql += " '" & stnSno & "'" ''SNO
                    strSql += " ,'" & stnTagSno & "'" 'TAGSNO
                    strSql += " ,'" & Val(.Item("ITEMID").ToString) & "'" 'ITEMID
                    strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                    strSql += " ," & stnItemId & "" 'STNITEMID
                    strSql += " ," & stnSubItemId & "" 'STNSUBITEMID
                    strSql += " ,'" & stnTagno & "'" 'TAGNO
                    strSql += " ," & Val(.Item("PCS").ToString) & "" 'STNPCS
                    strSql += " ," & Val(.Item("WEIGHT").ToString) & "" 'STNWT
                    strSql += " ," & Val(.Item("RATE").ToString) & "" 'STNRATE
                    strSql += " ," & Val(.Item("AMOUNT").ToString) & "" 'STNAMT
                    If stnSubItemId <> 0 Then 'DESCRIP
                        strSql += " ,'" & .Item("SUBITEM").ToString & "'"
                    Else
                        strSql += " ,'" & .Item("ITEM").ToString & "'"
                    End If
                    strSql += " ,'" & GetEntryDate(BillDate, tran).ToString("yyyy-MM-dd") & "'" 'RECDATE
                    strSql += " ,'" & .Item("CALC").ToString & "'" 'CALCMODE
                    strSql += " ,0" 'MINRATE
                    strSql += " ,0" 'SIZECODE
                    strSql += " ,'" & .Item("UNIT").ToString & "'" 'STONEUNIT
                    strSql += " ,NULL" 'ISSDATE
                    strSql += " ,''" 'OLDTAGNO
                    strSql += " ,''" 'VATEXM
                    strSql += " ,''" 'CARRYFLAG
                    strSql += " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, .Item("COSTID").ToString) & "'" 'COSTID
                    strSql += " ,'" & systemId & "'" 'SYSTEMID
                    strSql += " ,'" & VERSION & "'" 'APPVER
                    strSql += " ,0" 'USRATE
                    strSql += " ,0" 'INDRS
                    strSql += " )"
                    cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()

                End With
            Next


            Dim oldItem As Integer = Nothing
            Dim paramStr As String = ""
            Dim write As StreamWriter
            Dim roSelection() As DataRow = roselected.Select("")

            write = IO.File.CreateText(Application.StartupPath & "\Barcodeprint.mem")
            For Each ro As DataRow In roSelection
                If oldItem <> Val(ro!itemid.ToString) Then
                    write.WriteLine(LSet("PROC", 7) & ":" & ro!ITEMID.ToString)
                    paramStr += LSet("PROC", 7) & ":" & ro!ITEMID.ToString & ";"
                    oldItem = Val(ro!itemid.ToString)
                End If
                write.WriteLine(LSet("TAGNO", 7) & ":" & ro!TAGNO.ToString)
                paramStr += LSet("TAGNO", 7) & ":" & ro!TAGNO.ToString & ";"
            Next
            If paramStr.EndsWith(";") Then
                paramStr = Mid(paramStr, 1, paramStr.Length - 1)
            End If
            write.Flush()
            write.Close()
            If EXE_WITH_PARAM = False Then
                If IO.File.Exists(Application.StartupPath & "\PrjBarcodePrint.exe") Then
                    System.Diagnostics.Process.Start(Application.StartupPath & "\PrjBarcodePrint.exe")
                Else
                    MsgBox("Barcode print exe not found", MsgBoxStyle.Information)
                End If
            Else
                If IO.File.Exists(Application.StartupPath & "\PrjBarcodePrint.exe") Then
                    System.Diagnostics.Process.Start(Application.StartupPath & "\PrjBarcodePrint.exe", paramStr)
                Else
                    MsgBox("Barcode print exe not found", MsgBoxStyle.Information)
                End If
            End If

            'Dim tagPrefix As String = GetSoftValue("TAGPREFIX")
            'Dim updTagNo As String = Nothing
            'If tagPrefix.Length > 0 Then
            '    updTagNo = TagNo.ToString.Replace(tagPrefix, "")
            'Else
            '    updTagNo = TagNo
            'End If

            'If GetSoftValue("TAGNOFROM") = "I" Then ''FROM ITEMMAST OR UNIQUE
            '    If GetSoftValue("TAGNOGEN") = "I" Then ''FROM ITEM
            '        strSql = " UPDATE " & cnAdminDb & "..ITEMMAST SET CURRENTTAGNO = '" & updTagNo & "' WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'"
            '        cmd = New OleDbCommand(strSql, cn, tran)
            '        cmd.ExecuteNonQuery()
            '        'ExecQuery(strSql, cn, tran, COSTID)
            '    Else ''FROM LOT
            '        strSql = " UPDATE " & cnAdminDb & "..ITEMLOT SET CURTAGNO = '" & updTagNo & "'"
            '        cmd = New OleDbCommand(strSql, cn, tran)
            '        cmd.ExecuteNonQuery()
            '        'ExecQuery(strSql, cn, tran, COSTID)
            '    End If
            'ElseIf GetSoftValue("TAGNOFROM") = "U" Then  'UNIQUE
            '    strSql = " UPDATE " & cnAdminDb & "..SOFTCONTROL SET CTLTEXT  = '" & updTagNo & "' WHERE CTLID = 'LASTTAGNO'"
            '    cmd = New OleDbCommand(strSql, cn, tran)
            '    cmd.ExecuteNonQuery()
            '    'ExecQuery(strSql, cn, tran, COSTID)
            'End If




            tran.Commit()
            tran = Nothing

            fnew()
            MsgBox(Ptagnos + E0012, MsgBoxStyle.Exclamation)

        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        Finally
            If Not tran Is Nothing Then tran.Dispose()
        End Try
    End Function


    Private Sub gridview_UserDeletedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles gridview.UserDeletedRow
        dtGridReceipt.AcceptChanges()
        CalcGridTotal()
        If gridview.Rows.Count = 0 Then txtTranno.Focus()
    End Sub

    Private Sub gridview_UserDeletingRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowCancelEventArgs) Handles gridview.UserDeletingRow
        If gridview.Rows(e.Row.Index).Cells("SNO").Value.ToString <> "" Then
            For Each ro As DataRow In dtGridReceiptStone.Rows
                If ro("ISSSNO") = e.Row.Cells("SNO").Value Then
                    ro.Delete()
                End If
            Next
            dtGridReceiptStone.AcceptChanges()
        End If
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        funcAdd()
    End Sub

    Private Sub txtPcs_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPcs.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            txtGrsWt_WET.Focus()
        End If
    End Sub
    Private Sub txtGrsWt_WET_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtGrsWt_WET.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            txtNetwt_Wet.Focus()
        End If
    End Sub
    Private Sub txtWastPer_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtWastPer.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            CalcWast()
            CalcMc()
            CalcSaleValue()
            txtWastage.Focus()
        End If
    End Sub
    Private Sub txtNetwt_Wet_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtNetwt_Wet.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            'CalcWast()
            'CalcMc()

            If saStone = True Then
                ShowStoneDia()
            End If
            CalcSaleValue()
            txtWastPer.Focus()
        End If
    End Sub
    Private Sub txtWastage_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtWastage.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            txtMcGrm.Focus()
        End If
    End Sub

    Private Sub txtMcGrm_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtMcGrm.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            CalcMc()
            CalcSaleValue()
            txtMcharge.Focus()
        End If
    End Sub

    Private Sub txtMcharge_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtMcharge.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            CalcSaleValue()
            txtSalvalue_Amt.Focus()
        End If
    End Sub
    Private Sub CalcWast()
        Dim wast As Decimal
        Dim sval As Decimal

        wast = (Val(txtGrsWt_WET.Text) / 100) * Val(txtWastPer.Text)
        txtWastage.Text = Format(wast, "#0.00").ToString
        sval = ((OldSalValue / OldGrsWt) * (Val(txtGrsWt_WET.Text) + Val(txtWastage.Text))) + Val(txtMcharge.Text) + Val(txtStnAmt_amt.Text)
        txtSalvalue_Amt.Text = Format(sval, "#0.00").ToString
    End Sub
    Private Sub CalcMc()
        Dim mc As Decimal
        Dim sval As Decimal

        mc = Val(txtGrsWt_WET.Text) * Val(txtMcGrm.Text)
        txtMcharge.Text = Format(mc, "#0.00").ToString

        sval = ((OldSalValue / OldGrsWt) * (Val(txtGrsWt_WET.Text) + Val(txtWastage.Text))) + Val(txtMcharge.Text) + Val(txtStnAmt_amt.Text)
        txtSalvalue_Amt.Text = Format(sval, "#0.00").ToString
    End Sub
    Private Sub CalcSaleValue()
        Dim sval As Decimal

        sval = ((OldSalValue / OldGrsWt) * (Val(txtGrsWt_WET.Text) + Val(txtWastage.Text))) + Val(txtMcharge.Text) + Val(txtStnAmt_amt.Text)
        txtSalvalue_Amt.Text = Format(sval, "#0.00").ToString
    End Sub

    Private Sub StoneDetailsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StoneDetailsToolStripMenuItem.Click
        'ShowStoneDia()
    End Sub

    

    Private Sub ShowStoneDia()
        If objStone.Visible Then Exit Sub
        If Not saStone Then Exit Sub
        objStone._EditLock = False
        objStone._DelLock = False
        If txtGrsWt_WET.Text <> "" Then
            objStone.grsWt = Val(txtGrsWt_WET.Text)
        Else
            objStone.grsWt = Val(gridViewTotal.Rows(0).Cells("GRSWT").Value.ToString)
        End If


        objStone.dtGridStone.DefaultView.RowFilter = "ISSSNO='" & isssno & "'"
        objStone.dtGridStone.AcceptChanges()
        objStone.CalcStoneWtAmount()

        'objStone._Authorize = Authorize
        objStone.BackColor = Me.BackColor
        objStone.StartPosition = FormStartPosition.CenterScreen
        objStone.MaximizeBox = False
        objStone.grpStone.BackgroundColor = grpRecTagDetail.BackgroundColor
        objStone.StyleGridStone(objStone.gridStone)
        objStone.txtStItem.Select()
        objStone.ShowDialog()
        If txtGrsWt_WET.Text <> "" Then
            Dim stnamt As Decimal
            Dim stnwt As Decimal
            stnamt = IIf(IsDBNull(objStone.dtGridStone.Compute("SUM(AMOUNT)", "ISSSNO='" & isssno & "'")), 0, objStone.dtGridStone.Compute("SUM(AMOUNT)", "ISSSNO='" & isssno & "'"))
            stnwt = IIf(IsDBNull(objStone.dtGridStone.Compute("SUM(WEIGHT)", "ISSSNO='" & isssno & "'")), 0, objStone.dtGridStone.Compute("SUM(WEIGHT)", "ISSSNO='" & isssno & "'"))
            txtStnAmt_amt.Text = Format(stnamt, "#0.00").ToString
            txtstnWt_Wet.Text = Format(stnwt, "#0.00").ToString
            txtWastPer.Focus()
        End If
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        fnew()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        funcAdd()
    End Sub
End Class

