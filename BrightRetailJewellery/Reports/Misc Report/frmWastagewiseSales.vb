Imports System.Data.OleDb
Public Class frmWastagewiseSales
    Dim objGridShower As frmGridDispDia
    Dim dtSalesPerson As New DataTable
    Dim dsSalesPerson As New DataSet
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim dtCostCentre As New DataTable

    Private Sub frmWastagewiseSales_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        cmbMetal.Items.Clear()
        cmbMetal.Items.Add("ALL")
        strSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST ORDER BY DISPLAYORDER,METALNAME"
        objGPack.FillCombo(strSql, cmbMetal, False, False)
        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        GiritechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , "ALL")
        funcTablecode()
        funcNew()
        dtpFrom.Select()
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        Me.btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.btnExit_Click(Me, New EventArgs)
    End Sub
    
    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click


        Try
            btnView_Search.Enabled = False
            dtSalesPerson.Clear()
            dsSalesPerson.Clear()
            lblTitle.Text = "TITLE"
            gridView.DataSource = Nothing
            gridViewHead.DataSource = Nothing
            Me.Refresh()
            strSql = vbCrLf + "  SELECT TABLECODE, RANGE, SUM(PCS) PCS, SUM(GRSWT) GRSWT, SUM(NETWT) NETWT"
            strSql += vbCrLf + "  ,SUM(AMOUNT) AMOUNT,SUM(DISCOUNT) DISCOUNT,SUM(NETAMT) NETAMT,BOARDRATE "
            strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),ROUND(SUM(NETAMT)/SUM(" + IIf(rbtGRSWT.Checked = True, "GRSWT", "NETWT") + "),2)) AVGRATE "
            strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),ROUND(SUM(NETAMT)/SUM(" + IIf(rbtGRSWT.Checked = True, "GRSWT", "NETWT") + "),2)-BOARDRATE) RATEDIFF"
            strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),ROUND((ROUND(SUM(NETAMT)/SUM(" + IIf(rbtGRSWT.Checked = True, "GRSWT", "NETWT") + "),2)-BOARDRATE)* (100 / BOARDRATE),2)) DIFFPERC"
            strSql += vbCrLf + "  FROM"
            strSql += vbCrLf + "  (SELECT W.TABLECODE,CONVERT(VARCHAR,W.FROMWEIGHT)+'-'+CONVERT(VARCHAR,W.TOWEIGHT) RANGE,I.PCS,I.GRSWT "
            strSql += vbCrLf + "  ,I.NETWT,ISNULL(AMOUNT,0)+ISNULL(DISCOUNT,0)+ISNULL(TAX,0)+ISNULL(SC,0)+ISNULL(FIN_DISCOUNT,0)+ISNULL((SELECT SUM(TAXAMOUNT) FROM " & cnStockDb & "..TAXTRAN WHERE SNO=I.SNO),0) AMOUNT "
            strSql += vbCrLf + "  ,ISNULL(DISCOUNT,0)+ISNULL(FIN_DISCOUNT,0) DISCOUNT"
            strSql += vbCrLf + "  ,ISNULL(AMOUNT,0)+ISNULL(TAX,0)+ISNULL(SC,0)+ISNULL((SELECT SUM(TAXAMOUNT) FROM " & cnStockDb & "..TAXTRAN WHERE SNO=I.SNO),0) NETAMT "
            strSql += vbCrLf + "  ,I.BOARDRATE "
            strSql += vbCrLf + "  FROM " & cnAdminDb & "..WMCTABLE W"
            strSql += vbCrLf + "  INNER JOIN " & cnStockDb & "..ISSUE I  ON W.TABLECODE=I.TABLECODE AND I." + IIf(rbtGRSWT.Checked = True, "GRSWT", "NETWT") + " BETWEEN W.FROMWEIGHT AND W.TOWEIGHT "
            strSql += vbCrLf + "  WHERE I.TRANTYPE ='SA' AND ISNULL(W.TABLECODE,'')<>'' AND I.GRSWT<>0 AND I.NETWT <>0"
            strSql += vbCrLf + "  AND I.TRANDATE BETWEEN '" + dtpFrom.Value.ToString("yyyy-MM-dd") + "' AND '" + dtpTo.Value.ToString("yyyy-MM-dd") + "'"
            If cmbMetal.Text <> "ALL" Then strSql += vbCrLf + "  AND I.METALID = (SELECT METALID FROM " + cnAdminDb + "..METALMAST WHERE METALNAME='" + cmbMetal.Text + "')"
            If cmbTable.Text <> "ALL" Then strSql += vbCrLf + "  AND W.TABLECODE = '" + cmbTable.Text + "'"
            If chkCmbCostCentre.Text <> "ALL" Then strSql += vbCrLf + "  AND W.COSTID IN (SELECT COSTID FROM " + cnAdminDb + "..COSTCENTRE WHERE COSTNAME IN (" + GetQryString(chkCmbCostCentre.Text, ",") + "))"
            strSql += vbCrLf + "  ) S"
            strSql += vbCrLf + "  GROUP BY TABLECODE,RANGE,BOARDRATE"
            strSql += vbCrLf + "  ORDER BY TABLECODE,RANGE,BOARDRATE"



            dtSalesPerson = New DataTable
            dtSalesPerson.Columns.Add("KEYNO", GetType(Integer))
            dtSalesPerson.Columns("KEYNO").AutoIncrement = True
            dtSalesPerson.Columns("KEYNO").AutoIncrementSeed = 0
            dtSalesPerson.Columns("KEYNO").AutoIncrementStep = 1
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtSalesPerson)
            If dtSalesPerson.Rows.Count < 1 Then
                btnView_Search.Enabled = True
                MsgBox("Records not found..", MsgBoxStyle.Information, "Message")
                Exit Sub
            End If
            dtSalesPerson.Columns("KEYNO").SetOrdinal(dtSalesPerson.Columns.Count - 1)
            gridView.DataSource = dtSalesPerson
            FillGridGroupStyle_KeyNoWise(gridView, "SALESPERSON")
            funcGridSalesPersonStyle()
            'gridView.Columns("RESULT").Visible = False
            'gridView.Columns("COLHEAD").Visible = False
            'gridView.Columns("SNO").Visible = False
            'gridView.Columns("GROUPCOL").Visible = False
            Dim strTitle As String = Nothing
            strTitle = "WASTAGE WISE SALES REPORT FROM " & dtpFrom.Text & " TO " & dtpTo.Text & ""
            lblTitle.Text = strTitle

            Dim colWid As Integer = 0
            For cnt As Integer = 0 To gridView.ColumnCount - 1
                If gridView.Columns(cnt).Visible Then colWid += gridView.Columns(cnt).Width
            Next
            'With gridViewHead
            '    If colWid >= gridView.Width Then
            '        .Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
            '        .Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
            '    Else
            '        .Columns("SCROLL").Visible = False
            '    End If
            'End With
            '   FillGridGroupStyle_KeyNoWise(objGridShower.gridView, "Sno")
            gridView.Focus()
        Catch ex As Exception
            btnView_Search.Enabled = True
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
        Prop_Sets()
        btnView_Search.Enabled = True
        'End If
    End Sub


    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        Prop_Gets()
        funcNew()
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not GiritechPack.Methods.GetRights(_DtUserRights, Me.Name, GiritechPack.Methods.RightMode.Excel) Then Exit Sub
        'Me.Cursor = Cursors.WaitCursor
        If gridView.Rows.Count > 0 Then
            GiriPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, GiriPosting.GExport.GExportType.Export, gridViewHead)
        End If
        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not GiritechPack.Methods.GetRights(_DtUserRights, Me.Name, GiritechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            GiriPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, GiriPosting.GExport.GExportType.Print, gridViewHead)
        End If
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Function funcGridHeaderNew() As Integer
        Try
            Dim dtMergeHeader As New DataTable("~MERGEHEADER")
            With dtMergeHeader
                .Columns.Add("SALESPERSON", GetType(String))
                .Columns.Add("OPENPCS~OPENWEIGHT~OPENSTNWT~OPENAMOUNT", GetType(String))
                .Columns.Add("SALEPCS~SALEWEIGHT~SALESTNWT~SALEAMOUNT", GetType(String))
                .Columns.Add("RETURNPCS~RETURNWEIGHT~RETURNSTNWT~RETURNAMOUNT", GetType(String))
                .Columns.Add("PCS~WEIGHT~STNWT~AMOUNT", GetType(String))
                .Columns.Add("SCROLL", GetType(String))
                .Columns("SALESPERSON").Caption = ""
                .Columns("OPENPCS~OPENWEIGHT~OPENSTNWT~OPENAMOUNT").Caption = "OPENING"
                .Columns("SALEPCS~SALEWEIGHT~SALESTNWT~SALEAMOUNT").Caption = "SALES"
                .Columns("RETURNPCS~RETURNWEIGHT~RETURNSTNWT~RETURNAMOUNT").Caption = "RETURN"
                .Columns("PCS~WEIGHT~STNWT~AMOUNT").Caption = "DIFFERENCE"
                .Columns("Scroll").Caption = ""

            End With

            ''Dim dtHeader As New DataTable
            ''gridViewHead.DataSource = Nothing
            ''If chkCounterWise.Checked = True Then
            ''    strSql = "select ''SALESPERSON,''SALES,''RETURN1,''DIFFERENCE1,''SCROLL WHERE 1<>1"
            ''Else
            ''    strSql = "select ''SALESPERSON,''SALES,''RETURN1,''DIFFERENCE1,''SCROLL WHERE 1<>1"
            ''End If
            ''da = New OleDbDataAdapter(strSql, cn)
            ''da.Fill(dtHeader)
            ''gridViewHead.DataSource = dtHeader
            gridViewHead.DataSource = dtMergeHeader
            'funcGridHeaderStyle()
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function

    Private Sub frmWastagewiseSales_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If AscW(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    ''Private Sub gridView_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles gridView.CellFormatting
    ''    With gridView
    ''        Select Case .Rows(e.RowIndex).Cells("COLHEAD").Value.ToString
    ''            Case "T"
    ''                .Rows(e.RowIndex).Cells("SALESPERSON").Style.BackColor = reportHeadStyle.BackColor
    ''                .Rows(e.RowIndex).Cells("SALESPERSON").Style.Font = reportHeadStyle.Font
    ''        End Select
    ''    End With
    ''End Sub
    Private Sub gridSalesPersonPerform_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs) Handles gridView.ColumnWidthChanged
        'If gridViewHead.Columns.Count > 0 Then
        '    gridViewHead.Columns("SALESPERSON").Width = gridView.Columns("SALESPERSON").Width
        '    gridViewHead.Columns("OPENPCS~OPENWEIGHT~OPENSTNWT~OPENAMOUNT").Width = gridView.Columns("OPENWEIGHT").Width + gridView.Columns("OPENSTNWT").Width + gridView.Columns("OPENAMOUNT").Width
        '    gridViewHead.Columns("SALEPCS~SALEWEIGHT~SALESTNWT~SALEAMOUNT").Width = gridView.Columns("SALEWEIGHT").Width + gridView.Columns("SALESTNWT").Width + gridView.Columns("SALEAMOUNT").Width
        '    gridViewHead.Columns("RETURNPCS~RETURNWEIGHT~RETURNSTNWT~RETURNAMOUNT").Width = gridView.Columns("RETURNWEIGHT").Width + gridView.Columns("RETURNSTNWT").Width + gridView.Columns("RETURNAMOUNT").Width
        '    gridViewHead.Columns("PCS~WEIGHT~STNWT~AMOUNT").Width = gridView.Columns("WEIGHT").Width + gridView.Columns("STNWT").Width + gridView.Columns("AMOUNT").Width
        '    gridViewHead.Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
        '    gridViewHead.Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
        'End If
    End Sub

    Private Sub gridSalesPersonPerform_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.btnNew.Focus()
        End If
    End Sub

    Function funcGridHeaderStyle() As Integer
        With gridViewHead
            'If chkCounterWise.Checked = True Then
            '    With .Columns("PARTICULAR")
            '        .HeaderText = " "
            '        .SortMode = DataGridViewColumnSortMode.NotSortable
            '        .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            '        .Width = gridView.Columns("PARTICULAR").Width
            '    End With
            'End If
            .Columns("SCROLL").HeaderText = ""
            With .Columns("SALESPERSON")
                'If chkCounterWise.Checked = True Then
                '    .Visible = False
                'Else
                .Visible = True
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .Width = gridView.Columns("SALESPERSON").Width
                .HeaderText = " "
                'End If
            End With

            With .Columns("OPENPCS~OPENWEIGHT~OPENSTNWT~OPENAMOUNT")
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .Width = gridView.Columns("OPENPCS").Width + gridView.Columns("OPENWEIGHT").Width + gridView.Columns("OPENSTNWT").Width + gridView.Columns("OPENAMOUNT").Width
                .HeaderText = "OPENING"
            End With
            With .Columns("SALEPCS~SALEWEIGHT~SALESTNWT~SALEAMOUNT")
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .Width = gridView.Columns("SALEPCS").Width + gridView.Columns("SALEWEIGHT").Width + gridView.Columns("SALESTNWT").Width + gridView.Columns("SALEAMOUNT").Width
                .HeaderText = "SALES"
            End With
            With .Columns("RETURNPCS~RETURNWEIGHT~RETURNSTNWT~RETURNAMOUNT")
                .HeaderText = "RETURN"
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .Width = gridView.Columns("RETURNPCS").Width + gridView.Columns("RETURNWEIGHT").Width + gridView.Columns("RETURNSTNWT").Width + gridView.Columns("RETURNAMOUNT").Width
            End With
            With .Columns("PCS~WEIGHT~STNWT~AMOUNT")
                .HeaderText = "DIFFERENCE"
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .Width = gridView.Columns("PCS").Width + gridView.Columns("WEIGHT").Width + gridView.Columns("STNWT").Width + gridView.Columns("AMOUNT").Width
            End With
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        End With
    End Function

    Function funcGridSalesPersonStyle() As Integer
        With gridView
            'If chkCounterWise.Checked = True Then
            '    With .Columns("PARTICULAR")
            '        .HeaderText = "COUNTERNAME"
            '        .SortMode = DataGridViewColumnSortMode.NotSortable
            '        .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            '        .Width = 200
            '    End With
            'End If
            With .Columns("TABLECODE")
                'If chkCounterWise.Checked = True Then
                '    .Visible = False
                'Else
                .Visible = True
                .HeaderText = "TABLE CODE"
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .Width = 50
                'End If
            End With
            With .Columns("RANGE")
                .HeaderText = "RANGE"
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Width = 100
            End With
            With .Columns("PCS")
                .HeaderText = "PCS"
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Width = 100
            End With
            With .Columns("GRSWT")
                .HeaderText = "GRS WT"
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Width = 100
            End With
            With .Columns("NETWT")
                .HeaderText = "NET WT"
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Width = 100
            End With

            With .Columns("AMOUNT")
                .HeaderText = "AMOUNT"
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Width = 100
            End With
            With .Columns("DISCOUNT")
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Width = 100
            End With
            With .Columns("NETAMT")
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Width = 100
            End With
            With .Columns("BOARDRATE")
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Width = 100
            End With
            With .Columns("AVGRATE")
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Width = 100
            End With
            With .Columns("RATEDIFF")
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Width = 100
            End With
            With .Columns("DIFFPERC")
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Width = 100
            End With
            .Columns("KEYNO").Visible = False
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        End With
        funcGridHeaderNew()
    End Function

    Function funcNew() As Integer
        ' cmbMetal.Text = "ALL"
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        rbtGRSWT.Checked = True
        If cmbTable.Items.Count > 0 Then
            cmbTable.SelectedIndex = 0
        End If
        gridViewHead.DataSource = Nothing
        gridView.DataSource = Nothing
        lblTitle.Text = "TITLE"
        dtpFrom.Select()
    End Function

    Function funcTablecode() As Integer
        Try
            cmbTable.Items.Clear()
            cmbTable.Items.Add("ALL")
            strSql = "select DISTINCT TABLECODE from " & cnAdminDb & "..WMCTABLE ORDER BY TABLECODE"
            objGPack.FillCombo(strSql, cmbTable, False, False)
            cmbTable.Text = "ALL"
            If cmbTable.Items.Count > 0 Then
                cmbTable.SelectedIndex = 0
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function

    Private Sub gridSalesPersonPerform_Scroll(ByVal sender As Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles gridView.Scroll
        'If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
        '    gridViewHead.HorizontalScrollingOffset = e.NewValue
        'End If
        'Try
        '    If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
        '        gridViewHead.HorizontalScrollingOffset = e.NewValue
        '        gridViewHead.Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
        '        gridViewHead.Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
        '    Else
        '        gridViewHead.Columns("SCROLL").Visible = False
        '    End If
        'Catch ex As Exception
        '    MsgBox(ex.Message, MsgBoxStyle.Information)
        'End Try
    End Sub

    Private Sub dtpFrom_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        dtpTo.MinimumDate = dtpFrom.Value
    End Sub



    Private Sub Prop_Sets()
        Dim obj As New frmWastagewiseSales_Properties
        obj.p_cmbMetal = cmbMetal.Text
        obj.p_cmbSalesPerson = cmbTable.Text
        obj.p_rbtGRSWT = rbtGRSWT.Checked
        obj.p_rbtMetal = rbtNetWT.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmWastagewiseSales_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmWastagewiseSales_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmWastagewiseSales_Properties))
        cmbMetal.Text = obj.p_cmbMetal
        cmbTable.Text = obj.p_cmbSalesPerson
        rbtGRSWT.Checked = obj.p_rbtGRSWT
        rbtNetWT.Checked = obj.p_rbtMetal
    End Sub

    Private Sub grpControls_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles grpControls.Enter

    End Sub
End Class


Public Class frmWastagewiseSales_Properties
    Private cmbMetal As String = "ALL"
    Public Property p_cmbMetal() As String
        Get
            Return cmbMetal
        End Get
        Set(ByVal value As String)
            cmbMetal = value
        End Set
    End Property
    Private cmbSalesPerson As String = "ALL"
    Public Property p_cmbSalesPerson() As String
        Get
            Return cmbSalesPerson
        End Get
        Set(ByVal value As String)
            cmbSalesPerson = value
        End Set
    End Property
    Private rbtNone As Boolean = True
    Public Property p_rbtNone() As Boolean
        Get
            Return rbtNone
        End Get
        Set(ByVal value As Boolean)
            rbtNone = value
        End Set
    End Property
    Private rbtMetal As Boolean = True
    Public Property p_rbtMetal() As Boolean
        Get
            Return rbtMetal
        End Get
        Set(ByVal value As Boolean)
            rbtMetal = value
        End Set
    End Property
    Private rbtGRSWT As Boolean = True
    Public Property p_rbtGRSWT() As Boolean
        Get
            Return rbtGRSWT
        End Get
        Set(ByVal value As Boolean)
            rbtGRSWT = value
        End Set
    End Property
End Class

'=======================================================================================================================================
'QUERY FOR WITHOUT STORED PROCEDURE
'==================================
'strsql = "select"
'strsql += " ISNULL(SALESPERSON,'.') SALESPERSON"
'strsql += ",ISNULL(SUM(CASE WHEN SEP='I' THEN GRSWT ELSE 0 END),0) SALEWEIGHT"
'strsql += ",ISNULL(SUM(CASE WHEN SEP='I' THEN STNWT ELSE 0 END),0) SALESTNWT"
'strsql += ",ISNULL(SUM(CASE WHEN SEP='I' THEN AMOUNT ELSE 0 END),0) SALEAMOUNT"

'strsql += ",ISNULL(SUM(CASE WHEN SEP='R' THEN GRSWT ELSE 0 END),0) RETURNWEIGHT"
'strsql += ",ISNULL(SUM(CASE WHEN SEP='R' THEN STNWT ELSE 0 END),0) RETURNSTNWT"
'strsql += ",ISNULL(SUM(CASE WHEN SEP='R' THEN AMOUNT ELSE 0 END),0) RETURNAMOUNT"

'strsql += ",ISNULL(SUM(CASE WHEN SEP='R' THEN -1*GRSWT ELSE GRSWT END),0) WEIGHT"
'strsql += ",ISNULL(SUM(CASE WHEN SEP='R' THEN -1*STNWT ELSE STNWT END),0) STNWT"
'strsql += ",ISNULL(SUM(CASE WHEN SEP='R' THEN -1*AMOUNT ELSE AMOUNT END),0) AMOUNT"

'strsql += ",ISNULL(COUNTERNAME,'') COUNTERNAME"
'strsql += " FROM"
'strsql += " ("
'strsql += " select"
'strsql += " (select EMPNAME from " & cnAdminDb & "..EMPMASTER WHERE EMPID=I.EMPID) SALESPERSON"
'strsql += ",ISNULL(GRSWT,0) GRSWT"
'strsql += ",ISNULL((SELECT SUM(STNWT) FROM " & cnStockDb & "..ISSSTONE "
'strsql += " WHERE ISSSNO=I.SNO "
'strsql += " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID='D') GROUP BY ISSSNO),0) STNWT "
'strsql += ",ISNULL(AMOUNT,0) AMOUNT"
'strsql += ",(select ITEMCTRNAME from " & cnAdminDb & "..ITEMCOUNTER where ITEMCTRID=I.ITEMCTRID) COUNTERNAME"
'strsql += ",'I' SEP"
'strsql += " from " & cnStockDb & "..ISSUE AS I"
'strsql += " where TRANDATE  BETWEEN '" & dtpDateFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpDateTo.Value.Date.ToString("yyyy-MM-dd") & "'"
'If cmbSalesPerson.Text <> "ALL" And cmbSalesPerson.Text <> "" Then
'    strsql += " AND EMPID =(select EMPID from " & cnAdminDb & "..EMPMASTER where EMPNAME='" & Replace(cmbSalesPerson.Text, "'", "''") & "')"
'End If
'strsql += " UNION ALL"
'strsql += " select "
'strsql += " (select EMPNAME from " & cnAdminDb & "..EMPMASTER WHERE EMPID=R.EMPID) SALESPERSON"
'strsql += ",ISNULL(GRSWT,0)GRSWT"
'strsql += ",ISNULL((SELECT SUM(STNWT) FROM " & cnStockDb & "..RECEIPTSTONE"
'strsql += " WHERE ISSSNO=R.SNO "
'strsql += " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID='D') GROUP BY ISSSNO),0) STNWT "
'strsql += ",ISNULL(AMOUNT,0)AMOUNT"
'strsql += ",(select ITEMCTRNAME from " & cnAdminDb & "..ITEMCOUNTER where ITEMCTRID=R.ITEMCTRID) COUNTERNAME"
'strsql += ",'R' SEP"
'strsql += " from " & cnStockDb & "..RECEIPT AS R"
'strsql += " where TRANDATE  BETWEEN '" & dtpDateFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpDateTo.Value.Date.ToString("yyyy-MM-dd") & "'"
'If cmbSalesPerson.Text <> "ALL" And cmbSalesPerson.Text <> "" Then
'    strsql += " AND EMPID =(select EMPID from " & cnAdminDb & "..EMPMASTER where EMPNAME='" & Replace(cmbSalesPerson.Text, "'", "''") & "')"
'End If
'strsql += " )X GROUP BY SALESPERSON,COUNTERNAME"
'=======================================================================================================================================
