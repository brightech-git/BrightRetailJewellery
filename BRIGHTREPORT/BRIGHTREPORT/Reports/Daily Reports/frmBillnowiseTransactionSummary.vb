Imports System.Data.OleDb
Public Class frmBillnowiseTransactionSummary
    Dim objGridShower As frmGridDispDia
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand

    Dim dtCompany As New DataTable
    Dim dtItemName As New DataTable
    Dim dtDesigner As New DataTable
    Dim dtCostCentre As New DataTable

    Private Sub frmBillnowiseTransactionSummary_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmBillnowiseTransactionSummary_LOG(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GrpContainer.Location = New Point((ScreenWid - GrpContainer.Width) / 2, ((ScreenHit - 128) - GrpContainer.Height) / 2)

        '~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Company ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        strSql = " SELECT 'ALL' COMPANYNAME,'ALL' COMPANYID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COMPANYNAME,CONVERT(VARCHAR,COMPANYID),2 RESULT FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N'"
        strSql += " ORDER BY RESULT,COMPANYNAME"
        dtCompany = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCompany)
        BrighttechPack.GlobalMethods.FillCombo(cmbCompany, dtCompany, "COMPANYNAME", , "ALL")


        'strSql = " SELECT 'ALL' DESIGNERNAME,'ALL' DESIGNERID,1 RESULT"
        'strSql += " UNION ALL"
        'strSql += " SELECT DESIGNERNAME,CONVERT(vARCHAR,DESIGNERID),2 RESULT FROM " & cnAdminDb & "..DESIGNER"
        'strSql += " ORDER BY RESULT,DESIGNERNAME"
        'dtDesigner = New DataTable
        'da = New OleDbDataAdapter(strSql, cn)
        'da.Fill(dtDesigner)
        'BrighttechPack.GlobalMethods.FillCombo(chkCmbDesigner, dtDesigner, "DESIGNERNAME", , "ALL")
        'strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        'strSql += " UNION ALL"
        'strSql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        'strSql += " ORDER BY RESULT,COSTNAME"
        'dtCostCentre = New DataTable
        'da = New OleDbDataAdapter(strSql, cn)
        'da.Fill(dtCostCentre)
        'BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , "ALL")

        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub chkAsOnDate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
      
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        BrighttechPack.GlobalMethods.FillCombo(cmbCompany, dtCompany, "COMPANYNAME", , "ALL")
        Prop_Gets()
    End Sub


    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        Dim chkCompanyId As String = GetQryStringForSp(cmbCompany.Text, cnAdminDb & "..COMPANY", "COMPANYID", "COMPANYNAME")


        strSql = vbCrLf + " EXEC " & cnStockDb & "..SP_DAILYREPORTNEW_SUMMARY"
        strSql += vbCrLf + " @FRMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@COMPANYID = '" & chkCompanyId & "'"
        Dim SYSTEMID As String = Trim(LSet(Mid(Guid.NewGuid.ToString, 1, InStr(Guid.NewGuid.ToString, "-") - 1), 10))
        strSql += vbCrLf + " ,@SYSTEMID = '" & SYSTEMID & "'"
        strSql += vbCrLf + " ,@VATSEP = '" & IIf(chkVatSep.Checked, "Y", "N") & "'"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        'cmd.ExecuteNonQuery()

        Dim dtGrid As New DataTable

        dtGrid.Columns.Add("KEYNO", GetType(Integer))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1

        'strSql = " SELECT A.COL1,A.COL5,A.COL6,ROUND(A.COL11,0,2) AS COL11,ROUND(A.COL12,0,2) AS COL12,A.COLHEAD  "
        'strSql += vbCrLf + " ,''T,B.COL1 AS P_PARTICULAR,B.COL5 AS P_GRSWT,B.COL6 AS P_NETWT"
        'strSql += vbCrLf + " ,B.COL11 AS P_RECEIPT,B.COL12 AS P_PAYMENT,B.COLHEAD AS P_COLHEAD FROM TEMPTABLEDB ..TEMP" & SYSTEMID & "DAYREPORT1 A  "
        'strSql += vbCrLf + " FULL JOIN TEMPTABLEDB ..TEMP" & SYSTEMID & "DAYREPORT2 B ON  A.KEYNO1=B.KEYNO1"
        'da = New OleDbDataAdapter(strSql, cn)
        da = New OleDbDataAdapter(cmd)
        da.Fill(dtGrid)

        'Dim batchnos As String = ""
        'For i As Integer = 0 To dtGrid.Rows.Count - 1
        '    If i <> 0 Then
        '        If dtGrid.Rows(i).Item("COLHEAD").ToString = "" And dtGrid.Rows(i).Item("COL2").ToString <> dtGrid.Rows(i - 1).Item("COL2").ToString And dtGrid.Rows(i).Item("COL2").ToString <> "" Then
        '            If Not batchnos.Contains(dtGrid.Rows(i).Item("COL14").ToString) Then
        '                batchnos = batchnos + "," + dtGrid.Rows(i).Item("COL14").ToString
        '                Dim batchno As String = ""
        '                batchno = dtGrid.Rows(i).Item("COL14").ToString
        '                Dim receipt As String = dtGrid.Compute("SUM(COL11)", "ISNULL(COL14,'')='" & batchno & "'")
        '                dtGrid.Rows(i).Item("COL11") = receipt
        '            ElseIf dtGrid.Rows(i).Item("COL14").ToString <> "" Then
        '                dtGrid.Rows(i).Item("COL11") = DBNull.Value
        '            End If
        '        ElseIf dtGrid.Rows(i).Item("COLHEAD").ToString = "" And dtGrid.Rows(i).Item("COL2").ToString <> "" Then
        '            dtGrid.Rows(i).Item("COL11") = DBNull.Value
        '        End If
        '        If Val(dtGrid.Rows(i).Item("COL5").ToString) = 0 Then
        '            dtGrid.Rows(i).Item("COL5") = DBNull.Value
        '        End If
        '        If Val(dtGrid.Rows(i).Item("COL6").ToString) = 0 Then
        '            dtGrid.Rows(i).Item("COL6") = DBNull.Value
        '        End If
        '        If Val(dtGrid.Rows(i).Item("COL11").ToString) = 0 Then
        '            dtGrid.Rows(i).Item("COL11") = DBNull.Value
        '        End If

        '        If dtGrid.Rows(i).Item("P_PCS").ToString = "0" Or dtGrid.Rows(i).Item("P_PCS").ToString = "0.00" Then
        '            dtGrid.Rows(i).Item("P_PCS") = DBNull.Value
        '        End If
        '        If dtGrid.Rows(i).Item("P_GRSWT").ToString = "0.000" Or dtGrid.Rows(i).Item("P_GRSWT").ToString = "0.00" Then
        '            dtGrid.Rows(i).Item("P_GRSWT") = DBNull.Value
        '        End If
        '        If dtGrid.Rows(i).Item("P_NETWT").ToString = "0.000" Or dtGrid.Rows(i).Item("P_NETWT").ToString = "0.00" Then
        '            dtGrid.Rows(i).Item("P_NETWT") = DBNull.Value
        '        End If
        '        If Val(dtGrid.Rows(i).Item("P_RECEIPT").ToString) = 0 Then
        '            dtGrid.Rows(i).Item("P_RECEIPT") = DBNull.Value
        '        End If
        '        If dtGrid.Rows(i).Item("P_PAYMENT").ToString = "0.00" Then
        '            dtGrid.Rows(i).Item("P_PAYMENT") = DBNull.Value
        '        End If
        '    End If
        'Next

        'dtGrid.Columns("COL9").Data()
        dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If

        Dim dtMergeHeader As New DataTable("~MERGEHEADER")
        With dtMergeHeader
            If rbtGrtwght.Checked = True Then
                .Columns.Add("COL1~COL5~COL11", GetType(String))
                .Columns("COL1~COL5~COL11").Caption = "RECEIPT"
            ElseIf rbtNetWght.Checked = True Then
                .Columns.Add("COL1~COL6~COL11", GetType(String))
                .Columns("COL1~COL6~COL11").Caption = "RECEIPT"
            Else
                .Columns.Add("COL1~COL5~COL6~COL11", GetType(String))
                .Columns("COL1~COL5~COL6~COL11").Caption = "RECEIPT"
            End If
            .Columns.Add("T", GetType(String))
            If rbtGrtwght.Checked = True Then
                .Columns.Add("P_PARTICULAR~P_GRSWT~P_PAYMENT", GetType(String))
            ElseIf rbtNetWght.Checked = True Then
                .Columns.Add("P_PARTICULAR~P_NETWT~P_PAYMENT", GetType(String))
            Else
                .Columns.Add("P_PARTICULAR~P_GRSWT~P_NETWT~P_PAYMENT", GetType(String))
            End If
            .Columns.Add("SCROLL", GetType(String))

            If rbtGrtwght.Checked = True Then
                .Columns("COL1~COL5~COL11").Caption = "RECEIPT"
                .Columns("P_PARTICULAR~P_GRSWT~P_PAYMENT").Caption = "PAYMENT"
            ElseIf rbtNetWght.Checked = True Then
                .Columns("COL1~COL6~COL11").Caption = "RECEIPT"
                .Columns("P_PARTICULAR~P_NETWT~P_PAYMENT").Caption = "PAYMENT"
            Else
                .Columns("COL1~COL5~COL6~COL11").Caption = "RECEIPT"
                .Columns("P_PARTICULAR~P_GRSWT~P_NETWT~P_PAYMENT").Caption = "PAYMENT"
            End If
            .Columns("T").Caption = " "
            .Columns("SCROLL").Caption = ""
        End With

        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        objGridShower.Text = "DAILY REPORT"
        Dim tit As String = "DAILY REPORT REPORT" + vbCrLf
        tit += " DATE FROM " & dtpFrom.Text + " TO " + dtpTo.Text

        objGridShower.lblTitle.Text = tit
        AddHandler objGridShower.gridView.ColumnWidthChanged, AddressOf gridView_ColumnWidthChanged
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(dtGrid)
        objGridShower.dsGrid.Tables.Add(dtMergeHeader)
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        objGridShower.gridViewHeader.DataSource = objGridShower.dsGrid.Tables(1)

        objGridShower.FormReSize = False
        objGridShower.FormReLocation = False
        objGridShower.pnlFooter.Visible = False
        objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        DataGridView_SummaryFormatting(objGridShower.gridView)
        FormatGridColumns(objGridShower.gridView, False, False, , False)

        objGridShower.Show()
        objGridShower.FormReSize = True
        objGridShower.FormReLocation = True
        objGridShower.gridViewHeader.Visible = True
        'GridViewHeaderCreator(objGridShower.gridViewHeader)
        GridFillGridGroupStyle_KeyNoWise(objGridShower.gridView, "COL1")
        GridFillGridGroupStyle_KeyNoWise1(objGridShower.gridView, "P_PARTICULAR")
        'objGridShower.gridView_ColumnWidthChanged(Me, New DataGridViewColumnEventArgs(objGridShower.gridView.Columns("PARTICULAR")))
        GridHead()
        objGridShower.gridView.Rows(0).Frozen = True

        Prop_Sets()
    End Sub
    Private Sub GridHead()
        If objGridShower.gridView.RowCount > 0 Then
            objGridShower.gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
            objGridShower.gridView.Invalidate()
            For Each dgvCol As DataGridViewColumn In objGridShower.gridView.Columns
                dgvCol.Width = dgvCol.Width
            Next
            objGridShower.gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            SetGridHeadColWidth(objGridShower.gridView)
        End If

        With objGridShower.gridView
            objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle = .ColumnHeadersDefaultCellStyle
            objGridShower.gridViewHeader.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised
            .ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Sunken
            Dim TEMPCOLWIDTH As Integer = 0
            Dim TEMPCOLHEAD As String = ""

            If rbtGrtwght.Checked = True Then
                TEMPCOLWIDTH = .Columns("COL1").Width + .Columns("COL5").Width + .Columns("COL11").Width
                TEMPCOLHEAD = "COL1~COL5~COL11"
            ElseIf rbtNetWght.Checked = True Then
                TEMPCOLWIDTH = .Columns("COL1").Width + .Columns("COL6").Width + .Columns("COL11").Width
                TEMPCOLHEAD = "COL1~COL6~COL11"
            Else
                TEMPCOLWIDTH = .Columns("COL1").Width + .Columns("COL5").Width + .Columns("COL6").Width + .Columns("COL11").Width
                TEMPCOLHEAD = "COL1~COL5~COL6~COL11"
            End If
            objGridShower.gridViewHeader.Columns(TEMPCOLHEAD).Width = TEMPCOLWIDTH
            objGridShower.gridViewHeader.Columns(TEMPCOLHEAD).HeaderText = "RECEIPT"

            objGridShower.gridViewHeader.Columns("T").Width = .Columns("T").Width

            If rbtGrtwght.Checked = True Then
                TEMPCOLWIDTH = .Columns("P_PARTICULAR").Width + .Columns("P_GRSWT").Width + .Columns("P_PAYMENT").Width
                TEMPCOLHEAD = "P_PARTICULAR~P_GRSWT~P_PAYMENT"
            ElseIf rbtNetWght.Checked = True Then
                TEMPCOLWIDTH = .Columns("P_PARTICULAR").Width + .Columns("P_NETWT").Width + .Columns("P_PAYMENT").Width
                TEMPCOLHEAD = "P_PARTICULAR~P_NETWT~P_PAYMENT"
            Else
                TEMPCOLWIDTH = .Columns("P_PARTICULAR").Width + .Columns("P_GRSWT").Width + .Columns("P_NETWT").Width + .Columns("P_PAYMENT").Width
                TEMPCOLHEAD = "P_PARTICULAR~P_GRSWT~P_NETWT~P_PAYMENT"
            End If

            objGridShower.gridViewHeader.Columns(TEMPCOLHEAD).Width = TEMPCOLWIDTH
            objGridShower.gridViewHeader.Columns(TEMPCOLHEAD).HeaderText = "PAYMENT"
            objGridShower.gridViewHeader.Columns("SCROLL").Visible = False
        End With

        
    End Sub
    Private Sub gridView_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs)
        SetGridHeadColWidth(CType(sender, DataGridView))
    End Sub
    Private Sub GridFillGridGroupStyle_KeyNoWise(ByVal gridView As DataGridView, Optional ByVal FirstColumnName As String = Nothing)
        If gridView.Columns.Contains("KEYNO") = False Then Exit Sub
        If gridView.Columns.Contains("COLHEAD") = False Then Exit Sub
        Dim dt As New DataTable
        dt = CType(gridView.DataSource, DataTable).Copy
        ''title
        Dim rowTitle() As DataRow = Nothing
        rowTitle = dt.Select("COLHEAD = 'T'")
        For cnt As Integer = 0 To rowTitle.Length - 1
            If gridView.Columns.Contains("PARTICULAR") Then gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).Cells("PARTICULAR").Style = reportHeadStyle
            If FirstColumnName <> Nothing Then
                If gridView.Columns.Contains(FirstColumnName) Then gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).Cells(FirstColumnName).Style = reportHeadStyle
            End If
            'gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        Next
        rowTitle = dt.Select("COLHEAD = 'T1'")
        For cnt As Integer = 0 To rowTitle.Length - 1
            If gridView.Columns.Contains("PARTICULAR") Then gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).Cells("PARTICULAR").Style = reportHeadStyle1
            If FirstColumnName <> Nothing Then
                If gridView.Columns.Contains(FirstColumnName) Then gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).Cells(FirstColumnName).Style = reportHeadStyle1
            End If
            gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        Next
        rowTitle = dt.Select("COLHEAD = 'T2'")
        For cnt As Integer = 0 To rowTitle.Length - 1
            If gridView.Columns.Contains("PARTICULAR") Then gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).Cells("PARTICULAR").Style = reportHeadStyle2
            If FirstColumnName <> Nothing Then
                If gridView.Columns.Contains(FirstColumnName) Then gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).Cells(FirstColumnName).Style = reportHeadStyle2
            End If
            gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        Next
        rowTitle = dt.Select("COLHEAD = 'N'")
        For cnt As Integer = 0 To rowTitle.Length - 1
            If gridView.Columns.Contains("PARTICULAR") Then gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).Cells("PARTICULAR").Style = reportHeadStyle2
            If FirstColumnName <> Nothing Then
                If gridView.Columns.Contains(FirstColumnName) Then gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).Cells(FirstColumnName).Style = reportHeadStyle2
            End If
            gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).DefaultCellStyle.ForeColor = Color.OrangeRed
        Next
        'rowTitle = dt.Select("COLHEAD = 'S'")
        'For cnt As Integer = 0 To rowTitle.Length - 1
        '    If FirstColumnName <> Nothing Then
        '        If gridView.Columns.Contains(FirstColumnName) Then gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).Cells(FirstColumnName).Style = reportSubTotalStyle
        '    End If
        '    'gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).DefaultCellStyle = reportSubTotalStyle
        'Next
        rowTitle = dt.Select("COLHEAD = 'S1'")
        For cnt As Integer = 0 To rowTitle.Length - 1
            If FirstColumnName <> Nothing Then
                If gridView.Columns.Contains(FirstColumnName) Then gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).Cells(FirstColumnName).Style = reportSubTotalStyle1
            End If
            'gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).DefaultCellStyle = reportSubTotalStyle1
        Next
        rowTitle = dt.Select("COLHEAD = 'S2'")
        For cnt As Integer = 0 To rowTitle.Length - 1
            gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).DefaultCellStyle = reportSubTotalStyle2
        Next
        rowTitle = dt.Select("COLHEAD = 'G'")
        For cnt As Integer = 0 To rowTitle.Length - 1
            If FirstColumnName <> Nothing Then
                If gridView.Columns.Contains(FirstColumnName) Then gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).Cells(FirstColumnName).Style = reportTotalStyle
            End If
            'gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).DefaultCellStyle = reportTotalStyle
        Next
        'ROWTITLE = DT.Select("COLHEAD = 'S'"
    End Sub
    Private Sub GridFillGridGroupStyle_KeyNoWise1(ByVal gridView As DataGridView, Optional ByVal FirstColumnName As String = Nothing)
        If gridView.Columns.Contains("KEYNO") = False Then Exit Sub
        If gridView.Columns.Contains("P_COLHEAD") = False Then Exit Sub
        Dim dt As New DataTable
        dt = CType(gridView.DataSource, DataTable).Copy
        ''title
        Dim rowTitle() As DataRow = Nothing
        rowTitle = dt.Select("P_COLHEAD = 'T'")
        For cnt As Integer = 0 To rowTitle.Length - 1
            If gridView.Columns.Contains("P_PARTICULAR") Then gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).Cells("P_PARTICULAR").Style = reportHeadStyle
            If FirstColumnName <> Nothing Then
                If gridView.Columns.Contains(FirstColumnName) Then gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).Cells(FirstColumnName).Style = reportHeadStyle
            End If
            'gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        Next
        rowTitle = dt.Select("P_COLHEAD = 'T1'")
        For cnt As Integer = 0 To rowTitle.Length - 1
            If gridView.Columns.Contains("P_PARTICULAR") Then gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).Cells("P_PARTICULAR").Style = reportHeadStyle1
            If FirstColumnName <> Nothing Then
                If gridView.Columns.Contains(FirstColumnName) Then gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).Cells(FirstColumnName).Style = reportHeadStyle1
            End If
            'gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        Next
        rowTitle = dt.Select("P_COLHEAD = 'T2'")
        For cnt As Integer = 0 To rowTitle.Length - 1
            If gridView.Columns.Contains("P_PARTICULAR") Then gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).Cells("P_PARTICULAR").Style = reportHeadStyle2
            If FirstColumnName <> Nothing Then
                If gridView.Columns.Contains(FirstColumnName) Then gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).Cells(FirstColumnName).Style = reportHeadStyle2
            End If
            'gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        Next
        rowTitle = dt.Select("P_COLHEAD = 'N'")
        For cnt As Integer = 0 To rowTitle.Length - 1
            If gridView.Columns.Contains("P_PARTICULAR") Then gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).Cells("P_PARTICULAR").Style = reportHeadStyle2
            If FirstColumnName <> Nothing Then
                If gridView.Columns.Contains(FirstColumnName) Then gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).Cells(FirstColumnName).Style = reportHeadStyle2
            End If
            gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).DefaultCellStyle.ForeColor = Color.OrangeRed
        Next
        'rowTitle = dt.Select("P_COLHEAD = 'S'")
        'For cnt As Integer = 0 To rowTitle.Length - 1
        '    If FirstColumnName <> Nothing Then
        '        If gridView.Columns.Contains(FirstColumnName) Then gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).Cells(FirstColumnName).Style = reportSubTotalStyle
        '    End If
        '    'gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).DefaultCellStyle = reportSubTotalStyle
        'Next
        rowTitle = dt.Select("P_COLHEAD = 'S1'")
        For cnt As Integer = 0 To rowTitle.Length - 1
            If FirstColumnName <> Nothing Then
                If gridView.Columns.Contains(FirstColumnName) Then gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).Cells(FirstColumnName).Style = reportSubTotalStyle1
            End If
            'gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).DefaultCellStyle = reportSubTotalStyle1
        Next
        rowTitle = dt.Select("P_COLHEAD = 'S2'")
        For cnt As Integer = 0 To rowTitle.Length - 1
            gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).DefaultCellStyle = reportSubTotalStyle2
        Next
        rowTitle = dt.Select("P_COLHEAD = 'G'")
        For cnt As Integer = 0 To rowTitle.Length - 1
            If FirstColumnName <> Nothing Then
                If gridView.Columns.Contains(FirstColumnName) Then gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).Cells(FirstColumnName).Style = reportTotalStyle
            End If
            'gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).DefaultCellStyle = reportTotalStyle
        Next
        'ROWTITLE = DT.Select("P_COLHEAD = 'S'"
    End Sub

 
    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView)
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                dgvCol.Visible = True
            Next
            .Columns("COL5").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("COL6").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("COL11").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("COL12").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            .Columns("COL1").Width = 170
            .Columns("COLHEAD").Visible = False
            .Columns("KEYNO").Visible = False
            .Columns("COL12").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("P_GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("P_NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("P_RECEIPT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("P_PAYMENT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            .Columns("T").Width = 1
            .Columns("T").HeaderText = ""

            .Columns("COL1").Width = 140
            .Columns("P_PARTICULAR").Width = 140
            .Columns("COL5").Width = 70
            .Columns("COL6").Width = 70
            .Columns("COL11").Width = 90

            .Columns("P_GRSWT").Width = 60
            .Columns("P_NETWT").Width = 60
            .Columns("P_PAYMENT").Width = 90


            .Columns("COL1").HeaderText = "PARTICULAR"
            .Columns("COL5").HeaderText = "GRSWT"
            .Columns("COL6").HeaderText = "NETWT"
            .Columns("COL11").HeaderText = "RECEIPT"
            .Columns("COL12").HeaderText = "PAYMENT"
            .Columns("P_PARTICULAR").HeaderText = "PARTICULAR"
            .Columns("P_GRSWT").HeaderText = "GRSWT"
            .Columns("P_NETWT").HeaderText = "NETWT"
            .Columns("P_RECEIPT").HeaderText = "RECEIPT"
            .Columns("P_PAYMENT").HeaderText = "PAYMENT"
            .Columns("P_COLHEAD").HeaderText = ""

            .Columns("P_COLHEAD").Visible = False
            .Columns("COL12").Visible = False
            .Columns("P_RECEIPT").Visible = False
            .Columns("COLHEAD").HeaderText = ""
            .Columns("KEYNO").HeaderText = ""

            If rbtGrtwght.Checked = True Then
                .Columns("COL6").Visible = False
                .Columns("P_NETWT").Visible = False
            ElseIf rbtNetWght.Checked = True Then
                .Columns("COL5").Visible = False
                .Columns("P_GRSWT").Visible = False
            End If

            FormatGridColumns(dgv, False, False, , False)
            FillGridGroupStyle_KeyNoWise(dgv)
        End With
    End Sub
    Private Sub Prop_Sets()
        Dim obj As New frmBillnowiseTransactionSummary_Properties
        obj.p_cmbCompany = cmbCompany.Text
        obj.p_chkVA = chkVatSep.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmBillnowiseTransactionSummary_Properties))
    End Sub
    Private Sub Prop_Gets()
        Dim obj As New frmBillnowiseTransactionSummary_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmBillnowiseTransactionSummary_Properties))
        cmbCompany.Text = obj.p_cmbCompany
        chkVatSep.Checked = obj.p_chkVA
    End Sub
End Class

Public Class frmBillnowiseTransactionSummary_Properties
    Private cmbCompany As String = strCompanyName
    Public Property p_cmbCompany() As String
        Get
            Return cmbCompany
        End Get
        Set(ByVal value As String)
            cmbCompany = value
        End Set
    End Property
    Private chkVA As Boolean = False
    Public Property p_chkVA() As Boolean
        Get
            Return chkVA
        End Get
        Set(ByVal value As Boolean)
            chkVA = value
        End Set
    End Property
End Class
