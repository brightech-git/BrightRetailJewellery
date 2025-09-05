Imports System.IO
Imports System.Data.OleDb

Public Class frmEstSalesPurch
#Region "Varaible Declarations"
    Dim strSql As String = ""
    Dim dtOtherDetails As New DataTable
    Dim cmd As New OleDbCommand
#End Region
    Private Sub frmEstSalesPurch_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Return Then
            SendKeys.Send("{TAB}")
            e.Handled = True
        End If
    End Sub

    Private Sub frmEstSalesPurch_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        strSql = " select MetalName from " & cnAdminDb & "..metalMast order by MetalName"
        cmbMetal.Items.Clear()
        cmbMetal.Items.Add("ALL")
        objGPack.FillCombo(strSql, cmbMetal, False)
        cmbMetal.Text = "ALL"
        funcClearOtherDetails()
        lblTitle.Text = ""
    End Sub
    Function funcClearOtherDetails() As Integer
        'strSql = " Select 'ITEM'Col1,''Col2,'PUR.WASTAGE'Col3,''Col4"
        'strsql += vbcrlf + "  UNION ALL"
        'strSql += vbCrLf + "  Select 'SUB ITEM'Col1,''Col2,'PUR.TAX'Col3,''Col4"
        'strsql += vbcrlf + "  UNION ALL"
        'strSql += vbCrLf + "  Select 'TAG WEIGHT'Col1,''Col2,'PUR.SC'Col3,''Col4"
        'strsql += vbcrlf + "  UNION ALL"
        'strSql += vbCrLf + "  Select 'DIA WEIGHT'Col1,''Col2,'PUR.ASC'Col3,''Col4"
        'strsql += vbcrlf + "  UNION ALL"
        'strSql += vbCrLf + "  Select 'MK CHARGE'Col1,''Col2,'TOT.PUR.TAX'Col3,''Col4"
        'strsql += vbcrlf + "  UNION ALL"
        'strSql += vbCrLf + "  Select 'WASTAGE'Col1,''Col2,'TOT.PUR.AMT'Col3,''Col4"
        'strsql += vbcrlf + "  UNION ALL"
        'strSql += vbCrLf + "  Select 'RATE'Col1,''Col2,'SALES-PUR'Col3,''Col4"
        'strsql += vbcrlf + "  UNION ALL"
        'strSql += vbCrLf + "  Select 'RATE ID'Col1,''Col2,'SALE AMT'Col3,''Col4"
        'strsql += vbcrlf + "  UNION ALL"
        'strSql += vbCrLf + "  Select 'STONE AMT'Col1,''Col2,'TOT TAX'Col3,''Col4"
        'strsql += vbcrlf + "  UNION ALL"
        'strSql += vbCrLf + "  Select 'TAX'Col1,''Col2,'ASC'Col3,''Col4"
        'strsql += vbcrlf + "  UNION ALL"
        'strSql += vbCrLf + "  Select 'SC'Col1,''Col2,''Col3,''Col4"

        strSql = vbCrLf + "   Select 'ITEM'Col1,''Col2,'ASC'Col3,''Col4"
        strSql += vbCrLf + "   UNION ALL"
        STRSQL += VBCRLF + "   Select 'SUB ITEM'Col1,''Col2,'CATEGORY'Col3,''Col4"
        STRSQL += VBCRLF + "   UNION ALL"
        STRSQL += VBCRLF + "   Select 'TAG WEIGHT'Col1,''Col2,'PUR.WASTAGE'Col3,''Col4"
        STRSQL += VBCRLF + "   UNION ALL"
        STRSQL += VBCRLF + "   Select 'DIA WEIGHT'Col1,''Col2,'PUR.SC'Col3,''Col4"
        STRSQL += VBCRLF + "   UNION ALL"
        STRSQL += VBCRLF + "   Select 'MK CHARGE'Col1,''Col2,'PUR.ASC'Col3,''Col4 "
        STRSQL += VBCRLF + "   UNION ALL"
        STRSQL += VBCRLF + "   Select 'WASTAGE'Col1,''Col2,'TOT.PUR.TAX'Col3,''Col4"
        STRSQL += VBCRLF + "   UNION ALL"
        STRSQL += VBCRLF + "   Select 'RATE'Col1,''Col2,'TOT.PUR.AMT'Col3,''Col4 "
        STRSQL += VBCRLF + "   UNION ALL"
        STRSQL += VBCRLF + "   Select 'RATE ID'Col1,''Col2,'SALES-PUR'Col3,''Col4 "
        STRSQL += VBCRLF + "   UNION ALL"
        STRSQL += VBCRLF + "   Select 'STONE AMT'Col1,''Col2,'SALE AMT'Col3,''Col4"
        STRSQL += VBCRLF + "   UNION ALL"
        STRSQL += VBCRLF + "   Select 'TAX'Col1,''Col2,'TOT TAX'Col3,''Col4"
        strSql += vbCrLf + "   UNION ALL"
        strSql += vbCrLf + "   Select 'SC'Col1,''Col2,''Col3,''Col4"
        dtOtherDetails.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtOtherDetails)
        gridDetail.DataSource = dtOtherDetails
        funcSetOtherDetailsStyle()
    End Function
    Function funcSetOtherDetailsStyle() As Integer
        With gridDetail
            With .Columns("Col1")
                .Width = 100
                .DefaultCellStyle.BackColor = System.Drawing.SystemColors.Control
                .Resizable = DataGridViewTriState.False
            End With
            With .Columns("Col2")
                .Width = 125
                .Resizable = DataGridViewTriState.False
            End With
            With .Columns("Col3")
                .Width = 100
                .Resizable = DataGridViewTriState.False
                .DefaultCellStyle.BackColor = System.Drawing.SystemColors.Control
            End With
            With .Columns("Col4")
                .Width = 125
                .Resizable = DataGridViewTriState.False
            End With
            'With .Columns("Col5")
            '    .Width = 100
            '    .DefaultCellStyle.BackColor = System.Drawing.SystemColors.Control
            '    .Resizable = DataGridViewTriState.False
            'End With
            'With .Columns("Col6")
            '    .Width = 100
            '    .Resizable = DataGridViewTriState.False
            'End With
        End With
    End Function

    Private Sub btn_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Search.Click
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        'strSql = " IF(SELECT 1 FROM MASTER..SYSOBJECTS WHERE NAME ='TEMP_ESTVIEW' AND TRANTYPE='U')"
        'strSql += " SELECT I.TRANNO AS IBILLNO,I.ITEMID AS ID,I.TAGNO AS TAGNO,IT.ITEMNAME AS ITEM,SI.SUBITEMNAME AS SUBITEM,		"
        'strSql += " I.PCS AS IPCS,I.GRSWT AS IGRSWT,I.NETWT AS INETWT,I.AMOUNT AS IAMOUNT,I.TAGGRSWT,'' AS DIAWT,I.MCHARGE,I.WASTAGE,I.RATE,I.STNAMT,I.TAX AS ITAX,"
        'strSql += " 0 AS RBILLNO,0 AS RGRSWT,0 AS RNETWT,0 AS RAMOUNT,0 AS PURWASTAGE,0 AS RTAX INTO MASTER..TEMP_ESTVIEW FROM " & cnStockDb & "..ESTISSUE AS I"
        'strSql += " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IT"
        'strSql += "ON I.ITEMID =IT.ITEMID LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SI ON I.SUBITEMID=SI.SUBITEMID"
        'strSql = " WHERE I.TRANDATE='" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND I.ITEMID=" & txtItemId.Text & ""

        'cmd = New OleDbCommand(strSql, cn)
        'cmd.ExecuteNonQuery()
        'strSql = " SELECT 0 AS IBILLNO,R.ITEMID AS ID,R.TAGNO AS TAGNO,IT.ITEMNAME AS ITEM,ISNULL(SI.SUBITEMNAME,'') AS SUBITEM,		"
        'strSql += " R.PCS AS IPCS, 0 AS IGRSWT,0 AS INETWT,0 AS IAMOUNT,R.TAGGRSWT,'' AS DIAWT,R.MCHARGE,0 AS WASTAGE,R.RATE,R.STNAMT,0 AS ITAX,"
        'strSql += " R.TRANNO AS RBILLNO,R.GRSWT AS RGRSWT,R.NETWT AS RNETWT,R.AMOUNT AS RAMOUNT,R.WASTAGE AS PURWASTAGE,R.TAX AS RTAX INTO  MASTER..TEMP_ESTVIEW"
        'strSql += " FROM " & cnStockDb & "..ESTRECEIPT AS R INNER JOIN " & cnAdminDb & "..ITEMMAST AS IT"
        'strSql += " ON R.ITEMID =IT.ITEMID LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SI ON R.SUBITEMID=SI.SUBITEMID"
        'strSql = " WHERE R.TRANDATE='" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND R.ITEMID=" & txtItemId.Text & ""
        'cmd = New OleDbCommand(strSql, cn)
        'cmd.ExecuteNonQuery()

        'strSql = " UPDATE MASTER..TEMP_ESTVIEW SET IGRSWT=CASE WHEN IGRSWT=0 THEN NULL ELSE IGRSWT"
        'strSql += ",INETWT=CASE WHEN INETWT=0 THEN NULL ELSE INETWT"
        'strSql += ",IBILLNO=CASE WHEN IBILLNO=0 THEN NULL ELSE IBILLNO"
        'strSql += ",INETWT=CASE WHEN INETWT=0 THEN NULL ELSE INETWT"

        Dim dt As New DataTable
        strSql = "SELECT "
        strSql += vbCrLf + " CASE WHEN IBILLNO=0 THEN NULL ELSE IBILLNO END AS IBILLNO,"
        strSql += vbCrLf + " ID,TAGNO,ITEM,SUBITEM,IPCS,CATEGORY,"
        strSql += vbCrLf + " CASE WHEN IGRSWT=0 THEN NULL ELSE IGRSWT END AS IGRSWT,"
        strSql += vbCrLf + " CASE WHEN INETWT=0 THEN NULL ELSE INETWT END INETWT,"
        strSql += vbCrLf + " CASE WHEN IAMOUNT=0 THEN NULL ELSE IAMOUNT END IAMOUNT,"
        strSql += vbCrLf + " CASE WHEN TAGGRSWT=0 THEN NULL ELSE TAGGRSWT END TAGGRSWT,"
        strSql += vbCrLf + " DIAWT,"
        strSql += vbCrLf + " CASE WHEN MCHARGE=0 THEN NULL ELSE MCHARGE END	MCHARGE,"
        strSql += vbCrLf + " WASTAGE,RATE,STNAMT,"
        strSql += vbCrLf + " CASE WHEN ITAX=0 THEN NULL ELSE ITAX END ITAX,  "
        strSql += vbCrLf + " CASE WHEN SC =0 THEN NULL ELSE SC END SC,"
        strSql += vbCrLf + " CASE WHEN ADSC=0 THEN NULL ELSE ADSC END ADSC,"
        strSql += vbCrLf + " CASE WHEN RBILLNO=0 THEN NULL ELSE RBILLNO END RBILLNO,"
        strSql += vbCrLf + " CASE WHEN RGRSWT=0 THEN NULL ELSE RGRSWT END RGRSWT,"
        strSql += vbCrLf + " CASE WHEN RNETWT=0 THEN NULL ELSE RNETWT END RNETWT,"
        strSql += vbCrLf + " CASE WHEN RAMOUNT=0 THEN NULL ELSE RAMOUNT END RAMOUNT,	"
        strSql += vbCrLf + " CASE WHEN PURWASTAGE=0 THEN NULL ELSE PURWASTAGE END PURWASTAGE,"
        strSql += vbCrLf + " CASE WHEN RTAX=0 THEN NULL ELSE RTAX END RTAX ,"
        strSql += vbCrLf + " CASE WHEN PURASC=0 THEN NULL ELSE PURASC END PURASC,"
        strSql += vbCrLf + " CASE WHEN PURSC=0 THEN NULL ELSE PURSC END PURSC FROM ("
        strSql += vbCrLf + "       SELECT I.TRANNO AS IBILLNO,I.ITEMID AS ID,I.TAGNO AS TAGNO,IT.ITEMNAME AS ITEM,SI.SUBITEMNAME AS SUBITEM,    "
        strSql += vbCrLf + " I.PCS AS IPCS,(SELECT TOP 1 CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE=I.CATCODE) AS CATEGORY,I.GRSWT AS IGRSWT,I.NETWT AS INETWT,I.AMOUNT AS IAMOUNT,I.TAGGRSWT,'' AS DIAWT,I.MCHARGE,  "
        strSql += vbCrLf + " I.WASTAGE,I.RATE,I.STNAMT,I.TAX AS ITAX,I.SC AS SC,I.ADSC AS ADSC, 0 AS RBILLNO,0 AS RGRSWT,0 AS RNETWT,0 AS RAMOUNT,0 AS PURWASTAGE,  "
        strSql += vbCrLf + " 0 AS RTAX,0 AS PURASC,0 AS PURSC FROM " & cnStockDb & "..ESTISSUE AS I INNER JOIN " & cnAdminDb & "..ITEMMAST AS IT  "
        strSql += vbCrLf + " ON I.ITEMID =IT.ITEMID LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SI ON I.SUBITEMID=SI.SUBITEMID  "
        strSql += vbCrLf + " WHERE I.TRANDATE=' " & Format(dtpFrom.Value, "yyyy-MM-dd") & " ' AND ISNULL(I.CANCEL,'') =''"
        If txtItemId.Text <> "" Then strSql += " AND I.ITEMID=" & txtItemId.Text & ""
        If txtBillNo.Text <> "" Then strSql += " AND I.TRANNO=" & txtBillNo.Text & ""
        If cmbMetal.Text <> "" And cmbMetal.Text <> "ALL" Then strSql += " AND IT.METALID=(SELECT TOP 1 METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME='" & cmbMetal.Text & "')"
        strSql += vbCrLf + "         UNION ALL "

        strSql += vbCrLf + " SELECT 0 AS IBILLNO,R.ITEMID AS ID,R.TAGNO AS TAGNO,IT.ITEMNAME AS ITEM,ISNULL(SI.SUBITEMNAME,'') AS SUBITEM,    "
        strSql += vbCrLf + " R.PCS AS IPCS,(SELECT TOP 1 CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE=R.CATCODE) AS CATEGORY, 0 AS IGRSWT,0 AS INETWT,0 AS IAMOUNT,R.TAGGRSWT,'' AS DIAWT,R.MCHARGE,0 AS WASTAGE,R.RATE,R.STNAMT,0 AS ITAX,  "
        strSql += vbCrLf + " 0 AS SC ,0 AS ADSC,"
        strSql += vbCrLf + " R.TRANNO AS RBILLNO,R.GRSWT AS RGRSWT,R.NETWT AS RNETWT,R.AMOUNT AS RAMOUNT,R.WASTAGE AS PURWASTAGE,R.TAX AS RTAX,R.ADSC AS PURASC,R.SC AS PURSC  "
        strSql += vbCrLf + " FROM " & cnStockDb & "..ESTRECEIPT AS R INNER JOIN " & cnAdminDb & "..ITEMMAST AS IT  "
        strSql += vbCrLf + " ON R.ITEMID =IT.ITEMID LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SI ON R.SUBITEMID=SI.SUBITEMID  "

        strSql += vbCrLf + " WHERE R.TRANDATE=' " & Format(dtpFrom.Value, "yyyy-MM-dd") & " ' AND ISNULL(R.CANCEL,'')=''"
        If txtItemId.Text <> "" Then strSql += " AND R.ITEMID=" & txtItemId.Text & ""
        If txtBillNo.Text <> "" Then strSql += " AND R.TRANNO=" & txtBillNo.Text & ""
        If cmbMetal.Text <> "" And cmbMetal.Text <> "ALL" Then strSql += " AND IT.METALID=(SELECT TOP 1 METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME='" & cmbMetal.Text & "')"
        strSql += vbCrLf + " ) AS X  "

        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count = 0 Then MsgBox("No records found", MsgBoxStyle.Information) : Exit Sub
        lblTitle.Text = "SALES AND PURCHASE ESTIMATION VIEW ON " + dtpFrom.Value
        gridView.DataSource = dt
        funcGridStyle()
        strSql = " SELECT 'TOTAL'iDES"
        strSql += vbCrLf + "  ,'" & Val(IIf(IsDBNull(dt.Compute("SUM(IPCS)", "")), 0, dt.Compute("SUM(IPCS)", ""))) & "' IPCS"
        strSql += vbCrLf + "  ,'" & Format(Val(IIf(IsDBNull(dt.Compute("SUM(IGRSWT)", "")), 0, dt.Compute("SUM(IGRSWT)", ""))), "0.00") & "' IGRSWT"
        strSql += vbCrLf + "  ,'" & Format(Val(IIf(IsDBNull(dt.Compute("SUM(INETWT)", "")), 0, dt.Compute("SUM(INETWT)", ""))), "0.00") & "'INETWT"
        strSql += vbCrLf + "  ,'" & Format(Val(IIf(IsDBNull(dt.Compute("SUM(IAMOUNT)", "")), 0, dt.Compute("SUM(IAMOUNT)", ""))), "0.00") & "'IAMOUNT"
        strSql += vbCrLf + "  ,''rDES"
        strSql += vbCrLf + "  ,'" & Format(Val(IIf(IsDBNull(dt.Compute("SUM(RGRSWT)", "")), 0, dt.Compute("SUM(RGRSWT)", ""))), "0.00") & "'RGRSWT"
        strSql += vbCrLf + "  ,'" & Format(Val(IIf(IsDBNull(dt.Compute("SUM(RNETWT)", "")), 0, dt.Compute("SUM(RNETWT)", ""))), "0.00") & "'RNETWT"
        strSql += vbCrLf + "  ,'" & Format(Val(IIf(IsDBNull(dt.Compute("SUM(RAMOUNT)", "")), 0, dt.Compute("SUM(RAMOUNT)", ""))), "0.00") & "'RAMOUNT "
        Dim dtTot As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtTot)
        'pnlTotGrid.Visible = True
        gridTot.DataSource = dtTot
        StyleGridTot()
    End Sub
    Function funcGridStyle()
        If gridView.Rows.Count > 0 Then
            For i As Integer = 0 To gridView.Columns.Count - 1
                With gridView.Columns(i)
                    .Visible = False
                End With
            Next
        End If

        With gridView
            With .Columns("IBILLNO")
                .Visible = True
                .Width = 65
                .DefaultCellStyle.BackColor = Color.Beige
                .HeaderText = "BillNo"
            End With
            With .Columns("ID")
                .Visible = True
                .DefaultCellStyle.BackColor = Color.Beige
                .HeaderText = "ID"
            End With
            With .Columns("TAGNO")
                .Visible = True
                .DefaultCellStyle.BackColor = Color.Beige
                .HeaderText = "TagNo"
            End With
            With .Columns("IPCS")
                .Visible = True
                .Width = 50
                .DefaultCellStyle.BackColor = Color.Beige
                .HeaderText = "Pcs"
            End With
            With .Columns("IGRSWT")
                .Width = 80
                .Visible = True
                .DefaultCellStyle.BackColor = Color.Beige
                .HeaderText = "GrsWt"
                .DefaultCellStyle.Format = "#,##0.000"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("INETWT")
                .Width = 80
                .Visible = True
                .DefaultCellStyle.BackColor = Color.Beige
                .HeaderText = "NetWt"
                .DefaultCellStyle.Format = "#,##0.000"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("IAMOUNT")
                .Width = 90
                .Visible = True
                .DefaultCellStyle.BackColor = Color.Beige
                .HeaderText = "Amount"
                .DefaultCellStyle.Format = "#,##0.000"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("RBILLNO")
                .Visible = True
                .Width = 65
                .DefaultCellStyle.BackColor = Color.Ivory
                .HeaderText = "Billno"
            End With
            With .Columns("RGRSWT")
                .Visible = True
                .Width = 80
                .DefaultCellStyle.BackColor = Color.Ivory
                .HeaderText = "GrsWt"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.Format = "#,##0.000"
            End With
            With .Columns("RNETWT")
                .Visible = True
                .Width = 80
                .DefaultCellStyle.BackColor = Color.Ivory
                .HeaderText = "RNETWT"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.Format = "#,##0.000"
            End With
            With .Columns("RAMOUNT")
                .Visible = True
                .Width = 90
                .DefaultCellStyle.BackColor = Color.Ivory
                .HeaderText = "AMOUNT"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.Format = "#,##0.000"
            End With
            .ClearSelection()

        End With
        funcHeadGridStyle()
    End Function
    Function funcHeadGridStyle()
        Dim dtHead As New DataTable
        dtHead.Clear()
        gridHead.DataSource = Nothing
        strSql = " SELECT ''ISSUE,''RECEIPT WHERE 1<>1"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtHead)
        gridHead.DataSource = dtHead
        With gridHead
            With .Columns("ISSUE")
                .HeaderText = "SALES/RECEIPTS"
                .Width = gridView.Columns("IBILLNO").Width + gridView.Columns("ID").Width + gridView.Columns("TAGNO").Width + gridView.Columns("IPCS").Width + gridView.Columns("IGRSWT").Width + gridView.Columns("INETWT").Width + gridView.Columns("IAMOUNT").Width
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("RECEIPT")
                .HeaderText = "PURCHASE/PAYMENTS"
                .Width = gridView.Columns("RBILLNO").Width + gridView.Columns("RGRSWT").Width + gridView.Columns("RNETWT").Width + gridView.Columns("RAMOUNT").Width
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8.25!, FontStyle.Bold)
        End With
    End Function
    Function StyleGridTot() As Integer
        With gridTot
            With .Columns("iDes")
                .Width = 185
                .Visible = True
            End With
            With .Columns("rDes")
                .Width = 65
                .Visible = True
            End With
            With .Columns("iPcs")
                .HeaderText = "PCS"
                .Width = 55
                .Visible = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.Format = "#,##0.00"
            End With
            With .Columns("iGrsWt")
                .HeaderText = "GRSWT"
                .Width = 80
                .Visible = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.Format = "#,##0.00"
            End With
            With .Columns("iNETwt")
                .HeaderText = "NETWT"
                .Width = 80
                .Visible = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.Format = "#,##0.00"
            End With
            With .Columns("iAmount")
                .HeaderText = "AMOUNT"
                .Width = 90
                .Visible = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.Format = "#,##0.00"
            End With
            '''''''''''''''
            With .Columns("rGrsWt")
                .HeaderText = "GRSWT"
                .Width = 80
                .Visible = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.Format = "#,##0.00"
            End With
            With .Columns("rNETwt")
                .HeaderText = "NETWT"
                .Width = 80
                .Visible = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.Format = "#,##0.00"
            End With
            With .Columns("rAmount")
                .HeaderText = "AMOUNT"
                .Width = 90
                .Visible = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.Format = "#,##0.00"
            End With
        End With
    End Function
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub gridView_ColumnWidthChanged(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs)
        If gridHead.Rows.Count > 0 Then
            With gridHead
                .Columns("ISSUE").Width = gridView.Columns("IBILLNO").Width + gridView.Columns("ID").Width + gridView.Columns("TAGNO").Width + gridView.Columns("IPCS").Width + gridView.Columns("IGRSWT").Width + gridView.Columns("INETWT").Width + gridView.Columns("IAMOUNT").Width
                .Columns("RECEIPT").Width = gridView.Columns("RBILLNO").Width + gridView.Columns("RGRSWT").Width + gridView.Columns("RNETWT").Width + gridView.Columns("RAMOUNT").Width

            End With
        End If
    End Sub

    Private Sub gridView_Scroll(ByVal sender As Object, ByVal e As System.Windows.Forms.ScrollEventArgs)
        If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
            gridHead.HorizontalScrollingOffset = e.NewValue
            'gridTot.HorizontalScrollingOffset = e.NewValue
        End If
    End Sub

    Private Sub gridView_RowEnter(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.RowEnter
        Dim rw As Integer = e.RowIndex

        With gridView
            Dim saAmt As Double = Val(.Item("IAMOUNT", rw).Value.ToString)
            Dim saVat As Double = Val(.Item("ITAX", rw).Value.ToString)
            Dim purAmt As Double = Val(.Item("RAMOUNT", rw).Value.ToString)
            Dim purVat As Double = Val(.Item("RTAX", rw).Value.ToString)
            Dim purWithVat As Double = purAmt - purVat
            With .Rows(rw)
                strSql = vbCrLf + "   Select 'ITEM'Col1,' " & .Cells("ITEM").Value & " 'Col2,'ASC'Col3,' " & .Cells("ADSC").Value & " 'Col4"
                strSql += vbCrLf + "   UNION ALL"
                strSql += vbCrLf + "   Select 'SUB ITEM'Col1,'" & .Cells("SUBITEM").Value & "'Col2,'CATEGORY'Col3,'" & .Cells("CATEGORY").Value & "'Col4"
                strSql += vbCrLf + "   UNION ALL"
                strSql += vbCrLf + "   Select 'TAG WEIGHT'Col1,'" & .Cells("TAGGRSWT").Value & "'Col2,'PUR.WASTAGE'Col3,'" & .Cells("PURWASTAGE").Value & "'Col4"
                strSql += vbCrLf + "   UNION ALL"
                strSql += vbCrLf + "   Select 'DIA WEIGHT'Col1,'" & .Cells("DIAWT").Value & "'Col2,'PUR.SC'Col3,'" & .Cells("PURSC").Value & "'Col4"
                strSql += vbCrLf + "   UNION ALL"
                strSql += vbCrLf + "   Select 'MK CHARGE'Col1,'" & .Cells("MCHARGE").Value & "'Col2,'PUR.ASC'Col3,'" & .Cells("PURASC").Value & "'Col4 "
                strSql += vbCrLf + "   UNION ALL"
                strSql += vbCrLf + "   Select 'WASTAGE'Col1,'" & .Cells("WASTAGE").Value & "'Col2,'TOT.PUR.TAX'Col3,'" & .Cells("RTAX").Value & "'Col4"
                strSql += vbCrLf + "   UNION ALL"
                strSql += vbCrLf + "   Select 'RATE'Col1,'" & .Cells("RATE").Value & "'Col2,'TOT.PUR.AMT'Col3, ' " & purWithVat & "'Col4 "
                strSql += vbCrLf + "   UNION ALL"
                strSql += vbCrLf + "   Select 'RATE ID'Col1,''Col2,'SALES-PUR'Col3,''Col4 "
                strSql += vbCrLf + "   UNION ALL"
                strSql += vbCrLf + "   Select 'STONE AMT'Col1,''Col2,'SALE AMT'Col3,'" & saAmt & "'Col4"
                strSql += vbCrLf + "   UNION ALL"
                strSql += vbCrLf + "   Select 'TAX'Col1,'" & .Cells("ITAX").Value & "'Col2,'TOT TAX'Col3,''Col4"
                strSql += vbCrLf + "   UNION ALL"
                strSql += vbCrLf + "   Select 'SC'Col1,'" & .Cells("SC").Value & "'Col2,''Col3,''Col4"
                dtOtherDetails.Clear()
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtOtherDetails)
                gridDetail.DataSource = dtOtherDetails
                funcSetOtherDetailsStyle()
            End With
        End With
    End Sub

    Private Sub btn_Print_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Print.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print)

    End Sub

    Private Sub btn_Export_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Export.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export)
    End Sub
End Class