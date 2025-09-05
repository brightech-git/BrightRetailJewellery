Imports System.Data.OleDb
Public Class frmItemSalesPerformance
    Dim objGridShower As frmGridDispDia
    Dim dtITEMNAME As New DataTable
    Dim dsITEMNAME As New DataSet
    Dim DtItem As New DataTable
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim dtCostCentre As New DataTable
    Dim flagORMAST As Boolean = IIf(GetAdmindbSoftValue("RPT_P_ORMAST", "Y") = "Y", True, False)

    Private Sub frmItemSalesPerformance_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        cmbMetal.Items.Clear()
        cmbMetal.Items.Add("ALL")
        CmbCashCounter.Items.Clear()
        CmbCashCounter.Items.Add("ALL")
        'LOAD COMPANY
        strSql = vbCrLf + " SELECT 'ALL' COMPANYNAME,'ALL' COMPANYID,1 RESULT"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT COMPANYNAME,CONVERT(VARCHAR,COMPANYID),2 RESULT FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N'"
        strSql += vbCrLf + " ORDER BY RESULT,COMPANYNAME"
        da = New OleDbDataAdapter(strSql, cn)
        Dim dtCompany As New DataTable()
        da.Fill(dtCompany)
        cmbCompany.Items.Clear()
        GiritechPack.GlobalMethods.FillCombo(cmbCompany, dtCompany, "COMPANYNAME", , "ALL")
        strSql = " SELECT CASHNAME FROM " & cnAdminDb & "..CASHCOUNTER ORDER BY CASHNAME"
        objGPack.FillCombo(strSql, CmbCashCounter, False, False)
        strSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST ORDER BY DISPLAYORDER,METALNAME"
        objGPack.FillCombo(strSql, cmbMetal, False, False)
        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        GiritechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        cmbGroupBy.Items.Clear()
        cmbGroupBy.Items.Add("NONE")
        cmbGroupBy.Items.Add("COST CENTRE")
        cmbGroupBy.Items.Add("COUNTER")
        cmbGroupBy.Items.Add("COUNTER GROUP")
        cmbGroupBy.Items.Add("METAL")
        cmbGroupBy.Text = "None"
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False
        funcAddCounter()
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
            Dim selCompany As String = Nothing
            btnView_Search.Enabled = False
            dtITEMNAME.Clear()
            dsITEMNAME.Clear()
            lblTitle.Text = "TITLE"
            gridView.DataSource = Nothing
            gridViewHead.DataSource = Nothing
            Me.Refresh()
            Dim day As Integer = dtpFrom.Value.Day
            Dim date1 As DateTime
            If WithDate.Checked = True Then
                If (Val(Txtwithdate.Text.ToString) >= day) Then
                    If dtpFrom.Value.Date.AddDays(-1 * dtpFrom.Value.Date.Day).Day < Val(Txtwithdate.Text.ToString) Then
                        date1 = dtpFrom.Value.Date.AddDays(-1 * dtpFrom.Value.Date.Day)
                    Else
                        date1 = New Date(dtpFrom.Value.Date.AddMonths(-1).Year, dtpFrom.Value.AddMonths(-1).Date.Month, Val(Txtwithdate.Text.ToString))
                    End If
                Else
                    date1 = New Date(dtpFrom.Value.Date.Year, dtpFrom.Value.Date.Month, Val(Txtwithdate.Text.ToString))
                End If
            End If

            If cmbCompany.Text = "ALL" Then
                'selCompany = "ALL"
                'Dim sql As String = "SELECT COMPANYNAME FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N'"
                Dim sql As String = "SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N'"
                Dim dtCompany As New DataTable()
                da = New OleDbDataAdapter(sql, cn)
                da.Fill(dtCompany)
                If dtCompany.Rows.Count > 0 Then
                    'selItemId = "'"
                    For i As Integer = 0 To dtCompany.Rows.Count - 1
                        selCompany += "''" + dtCompany.Rows(i).Item("COMPANYID").ToString + "'',"
                    Next
                    If selCompany <> "" Then
                        selCompany = Mid(selCompany, 1, selCompany.Length - 1)
                    End If
                    'selItemId += "'"
                End If
            ElseIf cmbCompany.Text <> "" Then
                'Dim sql As String = "SELECT COMPANYNAME FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(cmbCompany.Text) & ")"
                Dim sql As String = "SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(cmbCompany.Text) & ")"
                Dim dtCompany As New DataTable()
                da = New OleDbDataAdapter(sql, cn)
                da.Fill(dtCompany)
                If dtCompany.Rows.Count > 0 Then
                    'selItemId = "'"
                    For i As Integer = 0 To dtCompany.Rows.Count - 1
                        selCompany += "''" + dtCompany.Rows(i).Item("COMPANYID").ToString + "''" + ","
                    Next
                    If selCompany <> "" Then
                        selCompany = Mid(selCompany, 1, selCompany.Length - 1)
                    End If
                    'selItemId += "'"
                End If
            End If

            strSql = " EXEC " & cnStockDb & "..SP_RPT_ITEMSALESPERFORMANCE"
            strSql += " @DATEFROM ='" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "'"
            strSql += ",@DATETO= '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
            If cmbGroupBy.Text = "COUNTER" Then
                strSql += ",@GROUPBY = 'C'"
            ElseIf cmbGroupBy.Text = "COUNTER GROUP" Then
                strSql += ",@GROUPBY = 'G'"
            ElseIf cmbGroupBy.Text = "METAL" Then
                strSql += ",@GROUPBY = 'M'"
            ElseIf cmbGroupBy.Text = "COST CENTRE" Then
                strSql += ",@GROUPBY = 'T'"
            Else
                strSql += ",@GROUPBY = ''"
            End If
            strSql += ",@METALNAME = '" & cmbMetal.Text & "'"
            strSql += ",@ITEMNAME='" & Replace(chkcmbItem.Text, "'", "''''") & "'"
            strSql += ",@CNCOMPANYID='" & selCompany & "'"
            strSql += ",@COSTNAME = '" & IIf(chkCmbCostCentre.Text.Trim <> "", GetQryString(chkCmbCostCentre.Text).Replace("'", ""), chkCmbCostCentre.Text) & "'"
            If WithDate.Checked = True Then
                strSql += ",@WITHOPENING = 'Y'"
                strSql += ",@OPENINGFDATE = '" & date1.ToString("yyyy-MM-dd") & "'"
                strSql += ",@OPENINGTDATE = '" & dtpFrom.Value.AddDays(-1).Date.ToString("yyyy-MM-dd") & "'"
            Else
                strSql += ",@WITHOPENING = 'N'"
                strSql += ",@OPENINGFDATE = ''"
                strSql += ",@OPENINGTDATE = ''"
            End If
            strSql += ",@GRSNET = '" + IIf(rbtGRSWT.Checked = True, "G", "N") + "'"
            strSql += ",@ORDERONLY = '" + IIf(ChkOrderonly.Checked = True, "Y", "N") + "'"
            strSql += ",@ORMAST = '" + IIf(flagORMAST, "Y", "N") + "'"
            'strSql += ",@CNADMINDB='" & cnAdminDb & "'"
            'strSql += ",@CNSTOCKDB='" & cnStockDb & "'"
            strSql += vbCrLf + ",@CASHCOUNTER = '" & CmbCashCounter.Text & "'"
            dtITEMNAME = New DataTable
            dtITEMNAME.Columns.Add("KEYNO", GetType(Integer))
            dtITEMNAME.Columns("KEYNO").AutoIncrement = True
            dtITEMNAME.Columns("KEYNO").AutoIncrementSeed = 0
            dtITEMNAME.Columns("KEYNO").AutoIncrementStep = 1
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtITEMNAME)
            If dtITEMNAME.Rows.Count < 1 Then
                btnView_Search.Enabled = True
                MsgBox("Records not found..", MsgBoxStyle.Information, "Message")
                Exit Sub
            End If
            dtITEMNAME.Columns("KEYNO").SetOrdinal(dtITEMNAME.Columns.Count - 1)
            gridView.DataSource = dtITEMNAME
            FillGridGroupStyle_KeyNoWise(gridView, "ITEMNAME")
            funcGridITEMNAMEStyle()
            gridView.Columns("RESULT").Visible = False
            gridView.Columns("COLHEAD").Visible = False
            gridView.Columns("SNO").Visible = False
            gridView.Columns("GROUPCOL").Visible = False
            If WithDate.Checked = True Then
                gridViewHead.Columns("OPENPCS~OPENWEIGHT~OPENSTNWT~OPENAMOUNT").Visible = True
                gridView.Columns("OPENPCS").Visible = True
                gridView.Columns("OPENWEIGHT").Visible = True
                gridView.Columns("OPENSTNWT").Visible = True
                gridView.Columns("OPENAMOUNT").Visible = True
            Else
                gridViewHead.Columns("OPENPCS~OPENWEIGHT~OPENSTNWT~OPENAMOUNT").Visible = False
                gridView.Columns("OPENPCS").Visible = False
                gridView.Columns("OPENWEIGHT").Visible = False
                gridView.Columns("OPENSTNWT").Visible = False
                gridView.Columns("OPENAMOUNT").Visible = False
            End If

            Dim strTitle As String = Nothing
            strTitle = "ITEM SALES PERFORMANCE REPORT FROM " & dtpFrom.Text & " TO " & dtpTo.Text & ""
            If chkcmbItem.Text <> "ALL" And chkcmbItem.Text <> "" Then
                strTitle += " FOR " & chkcmbItem.Text & ""
            End If
            strTitle += IIf(chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "", " :" & chkCmbCostCentre.Text, "")
            lblTitle.Text = strTitle

            Dim colWid As Integer = 0
            For cnt As Integer = 0 To gridView.ColumnCount - 1
                If gridView.Columns(cnt).Visible Then colWid += gridView.Columns(cnt).Width
            Next
            With gridViewHead
                If colWid >= gridView.Width Then
                    .Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
                    .Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
                Else
                    .Columns("SCROLL").Visible = False
                End If
            End With
            '   FillGridGroupStyle_KeyNoWise(objGridShower.gridView, "Sno")
            gridView.Focus()
        Catch ex As Exception
            btnView_Search.Enabled = True
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
        Prop_Sets()
        btnView_Search.Enabled = True
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
                .Columns.Add("ITEMNAME", GetType(String))
                .Columns.Add("OPENPCS~OPENWEIGHT~OPENSTNWT~OPENAMOUNT", GetType(String))
                .Columns.Add("SALEPCS~SALEWEIGHT~SALESTNWT~SALEAMOUNT", GetType(String))
                .Columns.Add("RETURNPCS~RETURNWEIGHT~RETURNSTNWT~RETURNAMOUNT", GetType(String))
                .Columns.Add("PCS~WEIGHT~STNWT~AMOUNT", GetType(String))
                .Columns.Add("SCROLL", GetType(String))
                .Columns("ITEMNAME").Caption = ""
                .Columns("OPENPCS~OPENWEIGHT~OPENSTNWT~OPENAMOUNT").Caption = "OPENING"
                .Columns("SALEPCS~SALEWEIGHT~SALESTNWT~SALEAMOUNT").Caption = IIf(ChkOrderonly.Checked = True, "ORDER", "SALES")
                .Columns("RETURNPCS~RETURNWEIGHT~RETURNSTNWT~RETURNAMOUNT").Caption = "RETURN"
                .Columns("PCS~WEIGHT~STNWT~AMOUNT").Caption = "DIFFERENCE"
                .Columns("Scroll").Caption = ""
            End With

            ''Dim dtHeader As New DataTable
            ''gridViewHead.DataSource = Nothing
            ''If chkCounterWise.Checked = True Then
            ''    strSql = "select ''ITEMNAME,''SALES,''RETURN1,''DIFFERENCE1,''SCROLL WHERE 1<>1"
            ''Else
            ''    strSql = "select ''ITEMNAME,''SALES,''RETURN1,''DIFFERENCE1,''SCROLL WHERE 1<>1"
            ''End If
            ''da = New OleDbDataAdapter(strSql, cn)
            ''da.Fill(dtHeader)
            ''gridViewHead.DataSource = dtHeader
            gridViewHead.DataSource = dtMergeHeader
            funcGridHeaderStyle()
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function

    Private Sub frmItemSalesPerformance_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If AscW(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    ''Private Sub gridView_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles gridView.CellFormatting
    ''    With gridView
    ''        Select Case .Rows(e.RowIndex).Cells("COLHEAD").Value.ToString
    ''            Case "T"
    ''                .Rows(e.RowIndex).Cells("ITEMNAME").Style.BackColor = reportHeadStyle.BackColor
    ''                .Rows(e.RowIndex).Cells("ITEMNAME").Style.Font = reportHeadStyle.Font
    ''        End Select
    ''    End With
    ''End Sub
    Private Sub gridITEMNAMEPerform_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs) Handles gridView.ColumnWidthChanged
        If gridViewHead.Columns.Count > 0 Then
            gridViewHead.Columns("ITEMNAME").Width = gridView.Columns("ITEMNAME").Width
            gridViewHead.Columns("OPENPCS~OPENWEIGHT~OPENSTNWT~OPENAMOUNT").Width = gridView.Columns("OPENWEIGHT").Width + gridView.Columns("OPENSTNWT").Width + gridView.Columns("OPENAMOUNT").Width
            gridViewHead.Columns("SALEPCS~SALEWEIGHT~SALESTNWT~SALEAMOUNT").Width = gridView.Columns("SALEWEIGHT").Width + gridView.Columns("SALESTNWT").Width + gridView.Columns("SALEAMOUNT").Width
            gridViewHead.Columns("RETURNPCS~RETURNWEIGHT~RETURNSTNWT~RETURNAMOUNT").Width = gridView.Columns("RETURNWEIGHT").Width + gridView.Columns("RETURNSTNWT").Width + gridView.Columns("RETURNAMOUNT").Width
            gridViewHead.Columns("PCS~WEIGHT~STNWT~AMOUNT").Width = gridView.Columns("WEIGHT").Width + gridView.Columns("STNWT").Width + gridView.Columns("AMOUNT").Width
            gridViewHead.Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
            gridViewHead.Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
        End If
    End Sub

    Private Sub gridITEMNAMEPerform_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
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
            With .Columns("ITEMNAME")
                'If chkCounterWise.Checked = True Then
                '    .Visible = False
                'Else
                .Visible = True
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .Width = gridView.Columns("ITEMNAME").Width
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

    Function funcGridITEMNAMEStyle() As Integer
        With gridView
            With .Columns("ITEMNAME")
                .Visible = True
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .Width = 200
                'End If
            End With
            With .Columns("PCS")
                .HeaderText = "PCS"
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Width = 100
            End With
            With .Columns("RETURNPCS")
                .HeaderText = "PCS"
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Width = 100
            End With
            With .Columns("SALEPCS")
                .HeaderText = "PCS"
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Width = 100
            End With

            With .Columns("SALEWEIGHT")
                .HeaderText = "WEIGHT"
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Width = 100
            End With
            With .Columns("SALESTNWT")
                .HeaderText = "DIAWT"
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Width = 100
            End With
            With .Columns("SALEAMOUNT")
                .HeaderText = "AMOUNT"
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Width = 100
            End With
            With .Columns("RETURNWEIGHT")
                .HeaderText = "WEIGHT"
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Width = 100
            End With
            With .Columns("RETURNSTNWT")
                .HeaderText = "DIAWT"
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Width = 100
            End With
            With .Columns("RETURNAMOUNT")
                .HeaderText = "AMOUNT"
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Width = 100
            End With
            With .Columns("WEIGHT")
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Width = 100
            End With
            With .Columns("STNWT")
                .HeaderText = "DIAWT"
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Width = 100
            End With
            With .Columns("AMOUNT")
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Width = 100
            End With
            With .Columns("GROUPCOL")
                .Visible = False
            End With
            .Columns("KEYNO").Visible = False
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
            .Invalidate()
            For Each dgvCol As DataGridViewColumn In .Columns
                dgvCol.Width = dgvCol.Width
            Next
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        End With
        funcGridHeaderNew()
    End Function

    Function funcNew() As Integer
        ' cmbMetal.Text = "ALL"
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        rbtNone.Checked = True
        chkcmbItem.Text = "ALL"
        gridViewHead.DataSource = Nothing
        gridView.DataSource = Nothing
        lblTitle.Text = "TITLE"
        dtpFrom.Select()
    End Function

    Function funcAddCounter() As Integer
        Try

            strSql = " SELECT 'ALL' ITEMNAME,'ALL' ITEMID,1 RESULT"
            strSql += " UNION ALL"
            strSql += " SELECT ITEMNAME,CONVERT(vARCHAR,ITEMID),2 RESULT FROM " & cnAdminDb & "..ITEMMAST "
            strSql += " ORDER BY RESULT,ITEMNAME"
            DtItem = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(DtItem)
            GiritechPack.GlobalMethods.FillCombo(chkcmbItem, DtItem, "ITEMNAME", , "ALL")

        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function

    Private Sub gridITEMNAMEPerform_Scroll(ByVal sender As Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles gridView.Scroll
        If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
            gridViewHead.HorizontalScrollingOffset = e.NewValue
        End If
        Try
            If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
                gridViewHead.HorizontalScrollingOffset = e.NewValue
                gridViewHead.Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
                gridViewHead.Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
            Else
                gridViewHead.Columns("SCROLL").Visible = False
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try
    End Sub

    Private Sub dtpFrom_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        dtpTo.MinimumDate = dtpFrom.Value
    End Sub



    Private Sub Prop_Sets()
        Dim obj As New frmItemSalesPerformance_Properties
        obj.p_cmbMetal = cmbMetal.Text
        obj.p_cmbCashCounter = CmbCashCounter.Text
        'obj.p_cmbITEMNAME = cmbSalesCounter.Text
        obj.p_rbtNone = rbtNone.Checked
        obj.p_rbtMetal = rbtMetal.Checked
        obj.p_rbtCounter = rbtCounter.Checked
        obj.p_rbtGRSWT = rbtGRSWT.Checked
        obj.p_rbtNETWT = rbtNetWT.Checked
        GetChecked_CheckedList(chkcmbItem, obj.p_chkcmbItem)
        SetSettingsObj(obj, Me.Name, GetType(frmItemSalesPerformance_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmItemSalesPerformance_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmItemSalesPerformance_Properties))
        cmbMetal.Text = obj.p_cmbMetal
        CmbCashCounter.Text = obj.p_CmbCashCounter
        'cmbSalesCounter.Text = obj.p_cmbITEMNAME
        SetChecked_CheckedList(chkcmbItem, obj.p_chkcmbItem, "ALL")
        rbtNone.Checked = obj.p_rbtNone
        rbtMetal.Checked = obj.p_rbtMetal
        rbtCounter.Checked = obj.p_rbtCounter
        rbtGRSWT.Checked = obj.p_rbtGRSWT
        rbtNetWT.Checked = obj.p_rbtNETWT
    End Sub

    Private Sub chkCmbSalesCounter_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub chkCmbSalesCounter_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub chkCmbSalesCounter_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs)
    End Sub

    Private Sub chkCmbSalesCounter_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub chkcmbItem_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkcmbItem.GotFocus

    End Sub

    Private Sub chkcmbItem_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkcmbItem.SelectedIndexChanged

    End Sub

    Private Sub WithDate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles WithDate.CheckedChanged
        If WithDate.Checked = True Then
            Txtwithdate.Visible = True
        Else
            Txtwithdate.Visible = False
        End If
    End Sub
End Class


Public Class frmItemSalesPerformance_Properties
    Private cmbMetal As String = "ALL"
    Public Property p_cmbMetal() As String
        Get
            Return cmbMetal
        End Get
        Set(ByVal value As String)
            cmbMetal = value
        End Set
    End Property
    Private cmbCashCounter As String = "ALL"
    Public Property p_cmbCashCounter() As String
        Get
            Return cmbCashCounter
        End Get
        Set(ByVal value As String)
            cmbCashCounter = value
        End Set
    End Property
    Private chkcmbItem As New List(Of String)
    Public Property p_chkcmbItem() As List(Of String)
        Get
            Return chkcmbItem
        End Get
        Set(ByVal value As List(Of String))
            chkcmbItem = value
        End Set
    End Property


    Private chkCmbSalesCounter As New List(Of String)
    Public Property p_chkCmbSalesCounter() As List(Of String)
        Get
            Return chkCmbSalesCounter
        End Get
        Set(ByVal value As List(Of String))
            chkCmbSalesCounter = value
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
    Private rbtCounter As Boolean = True
    Public Property p_rbtCounter() As Boolean
        Get
            Return rbtCounter
        End Get
        Set(ByVal value As Boolean)
            rbtCounter = value
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
    Private rbtNETWT As Boolean = True
    Public Property p_rbtNETWT() As Boolean
        Get
            Return rbtNETWT
        End Get
        Set(ByVal value As Boolean)
            rbtNETWT = value
        End Set
    End Property
End Class

'=======================================================================================================================================
'QUERY FOR WITHOUT STORED PROCEDURE
'==================================
'strsql = "select"
'strsql += " ISNULL(ITEMNAME,'.') ITEMNAME"
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
'strsql += " (select EMPNAME from " & cnAdminDb & "..EMPMASTER WHERE EMPID=I.EMPID) ITEMNAME"
'strsql += ",ISNULL(GRSWT,0) GRSWT"
'strsql += ",ISNULL((SELECT SUM(STNWT) FROM " & cnStockDb & "..ISSSTONE "
'strsql += " WHERE ISSSNO=I.SNO "
'strsql += " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID='D') GROUP BY ISSSNO),0) STNWT "
'strsql += ",ISNULL(AMOUNT,0) AMOUNT"
'strsql += ",(select ITEMCTRNAME from " & cnAdminDb & "..ITEMCOUNTER where ITEMCTRID=I.ITEMCTRID) COUNTERNAME"
'strsql += ",'I' SEP"
'strsql += " from " & cnStockDb & "..ISSUE AS I"
'strsql += " where TRANDATE  BETWEEN '" & dtpDateFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpDateTo.Value.Date.ToString("yyyy-MM-dd") & "'"
'If cmbITEMNAME.Text <> "ALL" And cmbITEMNAME.Text <> "" Then
'    strsql += " AND EMPID =(select EMPID from " & cnAdminDb & "..EMPMASTER where EMPNAME='" & Replace(cmbITEMNAME.Text, "'", "''") & "')"
'End If
'strsql += " UNION ALL"
'strsql += " select "
'strsql += " (select EMPNAME from " & cnAdminDb & "..EMPMASTER WHERE EMPID=R.EMPID) ITEMNAME"
'strsql += ",ISNULL(GRSWT,0)GRSWT"
'strsql += ",ISNULL((SELECT SUM(STNWT) FROM " & cnStockDb & "..RECEIPTSTONE"
'strsql += " WHERE ISSSNO=R.SNO "
'strsql += " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID='D') GROUP BY ISSSNO),0) STNWT "
'strsql += ",ISNULL(AMOUNT,0)AMOUNT"
'strsql += ",(select ITEMCTRNAME from " & cnAdminDb & "..ITEMCOUNTER where ITEMCTRID=R.ITEMCTRID) COUNTERNAME"
'strsql += ",'R' SEP"
'strsql += " from " & cnStockDb & "..RECEIPT AS R"
'strsql += " where TRANDATE  BETWEEN '" & dtpDateFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpDateTo.Value.Date.ToString("yyyy-MM-dd") & "'"
'If cmbITEMNAME.Text <> "ALL" And cmbITEMNAME.Text <> "" Then
'    strsql += " AND EMPID =(select EMPID from " & cnAdminDb & "..EMPMASTER where EMPNAME='" & Replace(cmbITEMNAME.Text, "'", "''") & "')"
'End If
'strsql += " )X GROUP BY ITEMNAME,COUNTERNAME"
'=======================================================================================================================================
