Imports System.Data.OleDb
Public Class RPT_BILLNOWISETRANSACTION
    Dim objGridShower As frmGridDispDia
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand

    Dim dtCompany As New DataTable
    Dim dtItemName As New DataTable
    Dim dtDesigner As New DataTable
    Dim dtCostCentre As New DataTable

    Private Sub RPT_BILLNOWISETRANSACTION_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub RPT_BILLNOWISETRANSACTION_LOG(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GrpContainer.Location = New Point((ScreenWid - GrpContainer.Width) / 2, ((ScreenHit - 128) - GrpContainer.Height) / 2)
        'strSql = " SELECT 'ALL' ITEMNAME,'ALL' ITEMID,1 RESULT"
        'strSql += " UNION ALL"
        'strSql += " SELECT ITEMNAME,CONVERT(vARCHAR,ITEMID),2 RESULT FROM " & cnAdminDb & "..ITEMMAST"
        'strSql += " WHERE ACTIVE = 'Y' AND STOCKTYPE = 'P'"
        'strSql += " ORDER BY RESULT,ITEMNAME"
        'dtItemName = New DataTable
        'da = New OleDbDataAdapter(strSql, cn)
        'da.Fill(dtItemName)
        'BrighttechPack.GlobalMethods.FillCombo(cmbCompany, dtItemName, "ITEMNAME", , "ALL")

        '~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Company ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        strSql = " SELECT 'ALL' COMPANYNAME,'ALL' COMPANYID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COMPANYNAME,CONVERT(VARCHAR,COMPANYID),2 RESULT FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N'"
        strSql += " ORDER BY RESULT,COMPANYNAME"
        dtCompany = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCompany)
        BrighttechPack.GlobalMethods.FillCombo(cmbCompany, dtCompany, "COMPANYNAME", , "ALL")


        strSql = " SELECT 'ALL' DESIGNERNAME,'ALL' DESIGNERID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT DESIGNERNAME,CONVERT(vARCHAR,DESIGNERID),2 RESULT FROM " & cnAdminDb & "..DESIGNER"
        strSql += " ORDER BY RESULT,DESIGNERNAME"
        dtDesigner = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtDesigner)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbDesigner, dtDesigner, "DESIGNERNAME", , "ALL")
        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False

        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub chkAsOnDate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        'If chkAsOnDate.Checked Then
        '    Label1.Visible = False
        '    chkAsOnDate.Text = "As OnDate"
        '    dtpTo.Visible = False
        'Else
        '    chkAsOnDate.Text = "Date From"
        '    Label1.Visible = True
        '    dtpTo.Visible = True
        'End If
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        'chkGroupByDesigner.Checked = False
        'chkWithSubitem.Checked = False
        'chkWithValue.Checked = False
        'chkGroupbyItem.Checked = False
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        BrighttechPack.GlobalMethods.FillCombo(chkCmbDesigner, dtDesigner, "DESIGNERNAME", , "ALL")
        'BrighttechPack.GlobalMethods.FillCombo(cmbCompany, dtItemName, "ITEMNAME", , "ALL")
        BrighttechPack.GlobalMethods.FillCombo(cmbCompany, dtCompany, "COMPANYNAME", , "ALL")
        Prop_Gets()
    End Sub


    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        Dim chkCostId As String = GetQryStringForSp(chkCmbCostCentre.Text, cnAdminDb & "..COSTCENTRE", "COSTID", "COSTNAME")
        'Dim chkItemId As String = GetQryStringForSp(cmbCompany.Text, cnAdminDb & "..ITEMMAST", "ITEMID", "ITEMNAME")
        Dim chkDesignerId As String = GetQryStringForSp(chkCmbDesigner.Text, cnAdminDb & "..DESIGNER", "DESIGNERID", "DESIGNERNAME")
        Dim chkCompanyId As String = GetQryStringForSp(cmbCompany.Text, cnAdminDb & "..COMPANY", "COMPANYID", "COMPANYNAME")


        strSql = vbCrLf + " EXEC " & cnStockDb & "..SP_DAILYREPORTNEW"
        strSql += vbCrLf + " @FRMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@COMPANYID = '" & chkCompanyId & "'"
        If rbtFormat2.Checked = True Then
            strSql += vbCrLf + " ,@FORMAT = 'N'"
        ElseIf rbtFormat1.Checked = True Then
            strSql += vbCrLf + " ,@FORMAT = 'O'"
        ElseIf rbtFormat3.Checked = True Then
            strSql += vbCrLf + " ,@FORMAT = 'T'"
        End If
        Dim SYSTEMID As String = Trim(LSet(Mid(Guid.NewGuid.ToString, 1, InStr(Guid.NewGuid.ToString, "-") - 1), 10))
        strSql += vbCrLf + " ,@SYSTEMID = '" & SYSTEMID & "'"

        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        Dim dtGrid As New DataTable

        dtGrid.Columns.Add("KEYNO", GetType(Integer))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        
        If rbtFormat2.Checked = True Then
            'strSql = "SELECT COL1,COL2,COL3,COL13,COL4,COL5,COL6,COL7,COL8,COL9,COL10,COL11,COL12,COLHEAD,KEYNO FROM MASTER..TEMPDAYREPORT1"

            strSql = " SELECT A.COL1,A.COL2,A.COL3,A.COL13,A.COL4,A.COL5,A.COL6,A.COL7,A.COL8,A.COL9,A.COL9A,A.COL9B,A.COL9C,A.COL10,ROUND(A.COL11,0,2) AS COL11,ROUND(A.COL12,0,2) AS COL12,A.COL14 ,A.COLHEAD  "
            strSql += vbCrLf + " ,''T,B.COL1 AS P_PARTICULAR,B.COL2 AS P_BILLNO,B.COL3 AS P_ITEM,B.COL13 AS P_TAGNO,B.COL4 AS P_PCS,B.COL5 AS P_GRSWT,B.COL6 AS P_NETWT"
            strSql += vbCrLf + " ,B.COL7 AS P_MCHARGE,B.COL8 AS P_RATE,B.COL9 AS P_AMOUNT,B.COL9A AS P_SGST,B.COL9B AS P_CGST,B.COL9C AS P_IGST,B.COL10 AS P_TAX,B.COL11 AS P_RECEIPT,ROUND(B.COL12,0,2) AS P_PAYMENT,B.COL14 AS p_BATCHNO,B.COLHEAD AS P_COLHEAD FROM "
            If Val(objGPack.GetSqlValue("SELECT COUNT(*) FROM TEMPTABLEDB ..TEMP" & SYSTEMID & "DAYREPORT1", , "", )) > Val(objGPack.GetSqlValue("SELECT COUNT(*) FROM TEMPTABLEDB ..TEMP" & SYSTEMID & "DAYREPORT2", , "", )) Then
                strSql += vbCrLf + " TEMPTABLEDB ..TEMP" & SYSTEMID & "DAYREPORT1 A  FULL JOIN TEMPTABLEDB ..TEMP" & SYSTEMID & "DAYREPORT2 B ON  A.KEYNO1=B.KEYNO1 "
            Else
                strSql += vbCrLf + " TEMPTABLEDB ..TEMP" & SYSTEMID & "DAYREPORT2 B  FULL JOIN TEMPTABLEDB ..TEMP" & SYSTEMID & "DAYREPORT1 A ON  A.KEYNO1=B.KEYNO1 "
            End If

            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtGrid)
        Else
            strSql = "SELECT * FROM TEMPTABLEDB..TEMP" & SYSTEMID & "DAYREPORT1"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtGrid)
        End If

        Dim batchnos As String = ""
        If rbtFormat2.Checked = True Then
            For i As Integer = 0 To dtGrid.Rows.Count - 1
                If i <> 0 Then
                    If dtGrid.Rows(i).Item("COLHEAD").ToString = "" And dtGrid.Rows(i).Item("COL2").ToString <> dtGrid.Rows(i - 1).Item("COL2").ToString And dtGrid.Rows(i).Item("COL2").ToString <> "" Then
                        If Not batchnos.Contains(dtGrid.Rows(i).Item("COL14").ToString) Then
                            batchnos = batchnos + "," + dtGrid.Rows(i).Item("COL14").ToString
                            Dim batchno As String = ""
                            batchno = dtGrid.Rows(i).Item("COL14").ToString
                            Dim receipt As String = dtGrid.Compute("SUM(COL11)", "ISNULL(COL14,'')='" & batchno & "'")
                            dtGrid.Rows(i).Item("COL11") = receipt
                        ElseIf dtGrid.Rows(i).Item("COL14").ToString <> "" Then
                            dtGrid.Rows(i).Item("COL11") = DBNull.Value
                        End If
                    ElseIf dtGrid.Rows(i).Item("COLHEAD").ToString = "" And dtGrid.Rows(i).Item("COL2").ToString <> "" Then
                        dtGrid.Rows(i).Item("COL11") = DBNull.Value
                    End If
                    If Val(dtGrid.Rows(i).Item("COL4").ToString) = 0 Then
                        dtGrid.Rows(i).Item("COL4") = DBNull.Value
                    End If
                    If Val(dtGrid.Rows(i).Item("COL5").ToString) = 0 Then
                        dtGrid.Rows(i).Item("COL5") = DBNull.Value
                    End If
                    If Val(dtGrid.Rows(i).Item("COL6").ToString) = 0 Then
                        dtGrid.Rows(i).Item("COL6") = DBNull.Value
                    End If
                    If Val(dtGrid.Rows(i).Item("COL11").ToString) = 0 Then
                        dtGrid.Rows(i).Item("COL11") = DBNull.Value
                    End If
                    If dtGrid.Columns.Contains("COL9A") Then
                        If Val(dtGrid.Rows(i).Item("COL9A").ToString) = 0 Then
                            dtGrid.Rows(i).Item("COL9A") = DBNull.Value
                        End If
                    End If
                    If dtGrid.Columns.Contains("COL9B") Then
                        If Val(dtGrid.Rows(i).Item("COL9B").ToString) = 0 Then
                            dtGrid.Rows(i).Item("COL9B") = DBNull.Value
                        End If
                    End If
                    If dtGrid.Columns.Contains("COL9C") Then
                        If Val(dtGrid.Rows(i).Item("COL9C").ToString) = 0 Then
                            dtGrid.Rows(i).Item("COL9C") = DBNull.Value
                        End If
                    End If
                    If dtGrid.Rows(i).Item("P_PCS").ToString = "0" Or dtGrid.Rows(i).Item("P_PCS").ToString = "0.00" Then
                        dtGrid.Rows(i).Item("P_PCS") = DBNull.Value
                    End If
                    If dtGrid.Rows(i).Item("P_GRSWT").ToString = "0.000" Or dtGrid.Rows(i).Item("P_GRSWT").ToString = "0.00" Then
                        dtGrid.Rows(i).Item("P_GRSWT") = DBNull.Value
                    End If
                    If dtGrid.Rows(i).Item("P_NETWT").ToString = "0.000" Or dtGrid.Rows(i).Item("P_NETWT").ToString = "0.00" Then
                        dtGrid.Rows(i).Item("P_NETWT") = DBNull.Value
                    End If
                    If Val(dtGrid.Rows(i).Item("P_RECEIPT").ToString) = 0 Then
                        dtGrid.Rows(i).Item("P_RECEIPT") = DBNull.Value
                    End If
                    If dtGrid.Rows(i).Item("P_PAYMENT").ToString = "0.00" Then
                        dtGrid.Rows(i).Item("P_PAYMENT") = DBNull.Value
                    End If
                End If
            Next
        End If

        'dtGrid.Columns("COL9").Data()
        dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If

        Dim dtMergeHeader As New DataTable("~MERGEHEADER")
        With dtMergeHeader
            .Columns.Add("COL1~COL2~COL13~COL4~COL5~COL6~COL9A~COL9B~COL9C~COL10~COL11", GetType(String))
            .Columns.Add("P_PARTICULAR~P_BILLNO~P_ITEM~P_PCS~P_GRSWT~P_NETWT~P_PAYMENT", GetType(String))
            .Columns.Add("SCROLL", GetType(String))
            .Columns("COL1~COL2~COL13~COL4~COL5~COL6~COL9A~COL9B~COL9C~COL10~COL11").Caption = "RECEIPT"
            .Columns("P_PARTICULAR~P_BILLNO~P_ITEM~P_PCS~P_GRSWT~P_NETWT~P_PAYMENT").Caption = "PAYMENT"
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
        If rbtFormat2.Checked = True Then
            objGridShower.dsGrid.Tables.Add(dtMergeHeader)
        End If
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        If rbtFormat2.Checked = True Then
            objGridShower.gridViewHeader.DataSource = objGridShower.dsGrid.Tables(1)
        End If
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
        If rbtFormat2.Checked = False Then
            FillGridGroupStyle_KeyNoWise(objGridShower.gridView, "COL1")
        End If
        If rbtFormat2.Checked = True Then
            GridFillGridGroupStyle_KeyNoWise(objGridShower.gridView, "COL1")
            GridFillGridGroupStyle_KeyNoWise1(objGridShower.gridView, "P_PARTICULAR")
        End If
        'objGridShower.gridView_ColumnWidthChanged(Me, New DataGridViewColumnEventArgs(objGridShower.gridView.Columns("PARTICULAR")))
        If rbtFormat2.Checked = True Then
            GridHead()
        End If
        If rbtFormat2.Checked = False Then
            objGridShower.gridView.Rows(0).Frozen = True
        End If
        Prop_Sets()
    End Sub
    Private Sub GridHead()
        With objGridShower.gridView
            objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle = .ColumnHeadersDefaultCellStyle
            objGridShower.gridViewHeader.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised
            .ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Sunken
            Dim TEMPCOLWIDTH As Integer = 0

            If rbtGrtwght.Checked = True Then
                TEMPCOLWIDTH += .Columns("COL1").Width + .Columns("COL2").Width + .Columns("COL4").Width + .Columns("COL5").Width + .Columns("COL11").Width + .Columns("COL13").Width + 2
            ElseIf rbtNetWght.Checked = True Then
                TEMPCOLWIDTH += .Columns("COL1").Width + .Columns("COL2").Width + .Columns("COL4").Width + .Columns("COL6").Width + .Columns("COL11").Width + .Columns("COL13").Width + 2
            Else
                TEMPCOLWIDTH += .Columns("COL1").Width + .Columns("COL2").Width + .Columns("COL4").Width + .Columns("COL5").Width + .Columns("COL6").Width + .Columns("COL11").Width + .Columns("COL13").Width + 2
            End If
            objGridShower.gridViewHeader.Columns("COL1~COL2~COL13~COL4~COL5~COL6~COL9A~COL9B~COL9C~COL10~COL11").Width = TEMPCOLWIDTH
            objGridShower.gridViewHeader.Columns("COL1~COL2~COL13~COL4~COL5~COL6~COL9A~COL9B~COL9C~COL10~COL11").HeaderText = "RECEIPT"
            TEMPCOLWIDTH = 0

            If rbtGrtwght.Checked = True Then
                TEMPCOLWIDTH += .Columns("P_PARTICULAR").Width + .Columns("P_BILLNO").Width + .Columns("P_ITEM").Width + .Columns("P_GRSWT").Width + .Columns("P_PAYMENT").Width + 15
            ElseIf rbtNetWght.Checked = True Then
                TEMPCOLWIDTH += .Columns("P_PARTICULAR").Width + .Columns("P_BILLNO").Width + .Columns("P_ITEM").Width + .Columns("P_NETWT").Width + .Columns("P_PAYMENT").Width + 15
            Else
                TEMPCOLWIDTH += .Columns("P_PARTICULAR").Width + .Columns("P_BILLNO").Width + .Columns("P_ITEM").Width + .Columns("P_GRSWT").Width + .Columns("P_NETWT").Width + .Columns("P_PAYMENT").Width + 15
            End If

            objGridShower.gridViewHeader.Columns("P_PARTICULAR~P_BILLNO~P_ITEM~P_PCS~P_GRSWT~P_NETWT~P_PAYMENT").Width = TEMPCOLWIDTH
            objGridShower.gridViewHeader.Columns("P_PARTICULAR~P_BILLNO~P_ITEM~P_PCS~P_GRSWT~P_NETWT~P_PAYMENT").HeaderText = "PAYMENT"
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
        rowTitle = dt.Select("COLHEAD = 'S'")
        For cnt As Integer = 0 To rowTitle.Length - 1
            If FirstColumnName <> Nothing Then
                If gridView.Columns.Contains(FirstColumnName) Then gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).Cells(FirstColumnName).Style = reportSubTotalStyle
            End If
            'gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).DefaultCellStyle = reportSubTotalStyle
        Next
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
        rowTitle = dt.Select("P_COLHEAD = 'S'")
        For cnt As Integer = 0 To rowTitle.Length - 1
            If FirstColumnName <> Nothing Then
                If gridView.Columns.Contains(FirstColumnName) Then gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).Cells(FirstColumnName).Style = reportSubTotalStyle
            End If
            'gridView.Rows(Val(rowTitle(cnt).Item("KEYNO").ToString)).DefaultCellStyle = reportSubTotalStyle
        Next
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

    Private Sub GridViewHeaderCreator(ByVal gridviewHead As DataGridView)
        Dim dtHead As New DataTable
        strSql = "SELECT ''[COL2]"
        strSql += " ,''[COL3]"
        strSql += " ,''[COL4]"
        strSql += " ,''[COL5]"
        strSql += " ,''[COL6]"
        strSql += " ,''[COL7],''[COL8],''[COL9],''[COL10],''[COL11],''[COL12],''SCROLL"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtHead)
        Dim colName As String = "Apr-Val"
        Dim s() As String = colName.Split("-")

        gridviewHead.DataSource = dtHead
        'gridviewHead.Columns("PARTICULAR~SUBITEMNAME").HeaderText = ""
        'gridviewHead.Columns("OPCS~OGRSWT~ODIAPCS~ODIAWT").HeaderText = "OPEING"
        'gridviewHead.Columns("RPCS~RGRSWT~RDIAPCS~RDIAWT").HeaderText = "RECEIPT"
        'gridviewHead.Columns("IPCS~IGRSWT~IDIAPCS~IDIAWT").HeaderText = "ISSUE"
        'gridviewHead.Columns("CPCS~CGRSWT~CDIAPCS~CDIAWT").HeaderText = "CLOSING"
        'gridviewHead.Columns("RATE").HeaderText = ""
        'gridviewHead.Columns("PURRATE").HeaderText = ""
        gridviewHead.Columns("SCROLL").HeaderText = ""
        gridviewHead.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        SetGridHeadColWidth(gridviewHead)
    End Sub

    'Private Sub SetGridHeadColWid(ByVal gridViewHeader As DataGridView)
    '    Dim f As frmGridDispDia
    '    f = objGPack.GetParentControl(gridViewHeader)
    '    If Not f.gridViewHeader.Visible Then Exit Sub
    '    If f.gridViewHeader Is Nothing Then Exit Sub
    '    If Not f.gridView.ColumnCount > 0 Then Exit Sub
    '    If Not f.gridViewHeader.ColumnCount > 0 Then Exit Sub
    '    With f.gridViewHeader
    '        .Columns("PARTICULAR~SUBITEMNAME").Width = f.gridView.Columns("PARTICULAR").Width + _
    '        IIf(f.gridView.Columns("SUBITEMNAME").Visible, f.gridView.Columns("SUBITEMNAME").Width, 0)
    '        .Columns("OPCS~OGRSWT").Width = f.gridView.Columns("OPCS").Width + f.gridView.Columns("OGRSWT").Width
    '        .Columns("RPCS~RGRSWT").Width = f.gridView.Columns("RPCS").Width + f.gridView.Columns("RGRSWT").Width
    '        .Columns("IPCS~IGRSWT").Width = f.gridView.Columns("IPCS").Width + f.gridView.Columns("IGRSWT").Width
    '        .Columns("CPCS~CGRSWT").Width = f.gridView.Columns("CPCS").Width + f.gridView.Columns("CGRSWT").Width
    '        .Columns("RATE").Width = f.gridView.Columns("RATE").Width
    '        .Columns("RATE").Visible = f.gridView.Columns("RATE").Visible
    '        .Columns("PURRATE").Width = f.gridView.Columns("PURRATE").Width
    '        .Columns("PURRATE").Visible = f.gridView.Columns("PURRATE").Visible
    '        Dim colWid As Integer = 0
    '        For cnt As Integer = 0 To f.gridView.ColumnCount - 1
    '            If f.gridView.Columns(cnt).Visible Then colWid += f.gridView.Columns(cnt).Width
    '        Next
    '        If colWid >= f.gridView.Width Then
    '            f.gridViewHeader.Columns("SCROLL").Visible = CType(f.gridView.Controls(1), VScrollBar).Visible
    '            f.gridViewHeader.Columns("SCROLL").Width = CType(f.gridView.Controls(1), VScrollBar).Width
    '            f.gridViewHeader.Columns("SCROLL").HeaderText = ""
    '        Else
    '            f.gridViewHeader.Columns("SCROLL").Visible = False
    '        End If
    '    End With
    'End Sub

    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView)
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                dgvCol.Visible = True
            Next
            .Columns("COL4").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("COL5").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("COL6").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("COL7").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("COL8").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("COL9").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            If .Columns.Contains("COL9A") Then
                .Columns("COL9A").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("COL9A").HeaderText = "SGST"
            End If
            If .Columns.Contains("COL9B") Then
                .Columns("COL9B").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("COL9B").HeaderText = "CGST"
            End If
            If .Columns.Contains("COL9C") Then
                .Columns("COL9C").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("COL9C").HeaderText = "IGST"
            End If
            .Columns("COL10").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("COL11").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("COL12").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            '.Columns("PARTICULAR").Width = IIf(chkGroupbyItem.Checked, 200, 100)
            '.Columns("SUBITEMNAME").Width = 150
            '.Columns("OPCS").Width = 50
            .Columns("COL1").Width = 170
            .Columns("COL2").Width = 60
            .Columns("COL3").Width = 150
            If rbtFormat2.Checked = True Then
                .Columns("COL13").Width = 60
            End If
            If rbtFormat1.Checked = True Then
                .Columns("COL5").Visible = False
            End If
            .Columns("COLHEAD").Visible = False
            .Columns("KEYNO").Visible = False
          
            If rbtFormat2.Checked = False Then
                .Columns("COL1").HeaderText = ""
                .Columns("COL2").HeaderText = ""
                .Columns("COL3").HeaderText = ""
                .Columns("COL4").HeaderText = ""
                .Columns("COL5").HeaderText = ""
                .Columns("COL6").HeaderText = ""
                .Columns("COL7").HeaderText = ""
                .Columns("COL8").HeaderText = ""
                .Columns("COL9").HeaderText = ""
                .Columns("COL10").HeaderText = ""
                .Columns("COL11").HeaderText = ""
                .Columns("COL12").HeaderText = ""
            End If
          
            If rbtFormat2.Checked = True Then

                .Columns("COL12").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("P_PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("P_GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("P_NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("P_MCHARGE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("P_RATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("P_AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("P_TAX").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("P_RECEIPT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("P_PAYMENT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("P_SGST").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("P_CGST").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("P_IGST").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

                .Columns("T").Width = 1
                .Columns("T").HeaderText = ""

                .Columns("COL1").Width = 140
                .Columns("COL4").Width = 40
                .Columns("P_PARTICULAR").Width = 140
                .Columns("P_BILLNO").Width = 50
                .Columns("P_ITEM").Width = 130
                .Columns("P_PCS").Width = 40
                .Columns("P_TAGNO").Width = 60
                .Columns("COL5").Width = 70
                .Columns("COL6").Width = 70
                .Columns("COL11").Width = 90

                .Columns("P_GRSWT").Width = 60
                .Columns("P_NETWT").Width = 60
                .Columns("P_PAYMENT").Width = 90


                .Columns("COL1").HeaderText = "PARTICULAR"
                .Columns("COL2").HeaderText = "BILLNO"
                .Columns("COL3").HeaderText = "ITEM NAME"
                .Columns("COL4").HeaderText = "PCS"
                .Columns("COL5").HeaderText = "GRSWT"
                .Columns("COL6").HeaderText = "NETWT"
                .Columns("COL7").HeaderText = "MCHARGE"
                .Columns("COL8").HeaderText = "RATE"
                .Columns("COL9").HeaderText = "AMOUNT"
                .Columns("COL10").HeaderText = "TAX"
                .Columns("COL11").HeaderText = "RECEIPT"
                .Columns("COL12").HeaderText = "PAYMENT"
                .Columns("COL13").HeaderText = "TAGNO"
                .Columns("P_PARTICULAR").HeaderText = "PARTICULAR"
                .Columns("P_BILLNO").HeaderText = "BILLNO"
                .Columns("P_ITEM").HeaderText = "BILL DETAIL"
                .Columns("P_PCS").HeaderText = "PCS"
                .Columns("P_GRSWT").HeaderText = "GRSWT"
                .Columns("P_NETWT").HeaderText = "NETWT"
                .Columns("P_MCHARGE").HeaderText = "MCHARGE"
                .Columns("P_RATE").HeaderText = "RATE"
                .Columns("P_AMOUNT").HeaderText = "AMOUNT"
                .Columns("P_TAX").HeaderText = "TAX"
                .Columns("P_RECEIPT").HeaderText = "RECEIPT"
                .Columns("P_PAYMENT").HeaderText = "PAYMENT"
                .Columns("P_COLHEAD").HeaderText = ""
                .Columns("P_TAGNO").HeaderText = "TAGNO"
                .Columns("P_SGST").HeaderText = "SGST"
                .Columns("P_CGST").HeaderText = "CGST"
                .Columns("P_IGST").HeaderText = "IGST"


                .Columns("T").Visible = False
                .Columns("COL7").Visible = False
                .Columns("COL8").Visible = False
                .Columns("COL9").Visible = False
                '.Columns("COL10").Visible = False
                .Columns("P_MCHARGE").Visible = False
                .Columns("P_RATE").Visible = False
                .Columns("P_AMOUNT").Visible = False
                '.Columns("P_TAX").Visible = False
                .Columns("P_COLHEAD").Visible = False

                .Columns("P_TAGNO").Visible = False
                .Columns("P_RECEIPT").Visible = False
                .Columns("P_ITEM").Visible = True
                .Columns("COL3").Visible = False
                .Columns("COL12").Visible = False
                .Columns("COL14").Visible = False
                .Columns("P_BATCHNO").Visible = False

            End If
            .Columns("COLHEAD").HeaderText = ""
            .Columns("KEYNO").HeaderText = ""
            If rbtFormat2.Checked = False Then
                .ColumnHeadersVisible = False
            End If

            If rbtFormat1.Checked = True Then
                If rbtGrtwght.Checked = True Then
                    .Columns("COL8").Visible = False
                ElseIf rbtNetWght.Checked = True Then
                    .Columns("COL7").Visible = False
                End If
            ElseIf rbtFormat2.Checked = True Then
                If rbtGrtwght.Checked = True Then
                    .Columns("COL6").Visible = False
                    .Columns("P_NETWT").Visible = False
                ElseIf rbtNetWght.Checked = True Then
                    .Columns("COL5").Visible = False
                    .Columns("P_GRSWT").Visible = False
                End If
            End If


            FormatGridColumns(dgv, False, False, , False)
            FillGridGroupStyle_KeyNoWise(dgv)
        End With
    End Sub
    Private Sub Prop_Sets()
        Dim obj As New RPT_BILLNOWISETRANSACTION_Properties
        obj.p_cmbCompany = cmbCompany.Text
        GetChecked_CheckedList(chkCmbDesigner, obj.p_chkCmbDesigner)
        GetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre)
        SetSettingsObj(obj, Me.Name, GetType(RPT_BILLNOWISETRANSACTION_Properties))
    End Sub
    Private Sub Prop_Gets()
        Dim obj As New RPT_BILLNOWISETRANSACTION_Properties
        GetSettingsObj(obj, Me.Name, GetType(RPT_BILLNOWISETRANSACTION_Properties))
        cmbCompany.Text = obj.p_cmbCompany
        SetChecked_CheckedList(chkCmbDesigner, obj.p_chkCmbDesigner, Nothing)
        SetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre, cnCostName)
    End Sub
End Class

Public Class RPT_BILLNOWISETRANSACTION_Properties
    Private cmbCompany As String = strCompanyName
    Public Property p_cmbCompany() As String
        Get
            Return cmbCompany
        End Get
        Set(ByVal value As String)
            cmbCompany = value
        End Set
    End Property
    Private chkCmbDesigner As New List(Of String)
    Public Property p_chkCmbDesigner() As List(Of String)
        Get
            Return chkCmbDesigner
        End Get
        Set(ByVal value As List(Of String))
            chkCmbDesigner = value
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
End Class
