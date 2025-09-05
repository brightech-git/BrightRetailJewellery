Imports System.Data.OleDb
Imports System.IO

Public Class frmDrsMarking
    'CALID-593: CLIENT-ALL: CORRECTION-EMAIL : ALTER BY SATHYA
    Inherits System.Windows.Forms.Form
    Dim strSql As String
    Dim dt As DataTable
    Dim dtTemp As New DataTable
    Dim cmd As OleDbCommand
    Dim dtViewTotal As New DataTable
    Dim dtSummary As New DataTable
    Dim dtBillView As New DataTable
    Dim dtGridView As New DataTable
    Dim dtTotPcsWt As New DataTable
    Dim Ismaxdrcr As Boolean = False
    Dim AccEntry As New List(Of String)
    Public flag As Boolean = False
    Dim objMoreOption As New frmAccLedgerMore
    Dim dtContraSummary As New DataTable
    Dim objContraSummary As frmGridDispDia
    Dim dtCostCentre As New DataTable
    Dim dtCompany As New DataTable
    Dim dtAcName As New DataTable
    Dim dtAcGroup As New DataTable
    Public ExCalling As Boolean = False
    Dim objSearch As Object = Nothing
    Dim _ledgerName As String
    Dim _pageNo As String
    Dim _sNo As String
    Dim _indexPage As String

    Dim strprint As String
    Dim FileWrite As StreamWriter
    Dim PgNo As Integer
    Dim line As Integer
    Dim dtAccNames As New DataTable
    'Drs Marking
    Dim _dtMark As New DataTable
   
    Function funcAccountsEntry(ByVal rwIndex As String) As Integer
        strSql = " SELECT "
        strSql += vbCrLf + " CASE WHEN TRANMODE = 'C' THEN 'Cr' else 'Dr' end as TYPE"
        strSql += vbCrLf + " ," & cnAdminDb & ".DBO.WORDCAPS((SELECT TOP 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.ACCODE))DESCRIPTION"
        strSql += vbCrLf + " ,REMARK1 NARRATION1"
        strSql += vbCrLf + " ,REMARK2 NARRATION2"
        strSql += vbCrLf + " ,CHQCARDNO CHQNO"
        strSql += vbCrLf + " ,CHQDATE,CHQCARDREF CHQDETAIL"
        strSql += vbCrLf + " ,(SELECT TOP 1 TDSPER FROM " & cnStockDb & "..ACCTRAN WHERE TRANDATE = T.TRANDATE AND BATCHNO = T.BATCHNO AND ACCODE = T.ACCODE AND TDSPER <> 0)AS TDSPER"
        strSql += vbCrLf + " ,(SELECT TOP 1 TDSAMOUNT TDSAMT FROM " & cnStockDb & "..ACCTRAN WHERE TRANDATE = T.TRANDATE AND BATCHNO = T.BATCHNO AND ACCODE = T.ACCODE AND TDSAMOUNT <> 0)AS TDSAMT"
        strSql += vbCrLf + " ,(SELECT TOP 1 TDSCATID FROM " & cnStockDb & "..ACCTRAN WHERE TRANDATE = T.TRANDATE AND BATCHNO = T.BATCHNO AND ACCODE = T.ACCODE AND TDSCATID <> 0)AS TDSCATID"
        strSql += vbCrLf + " ,NULL VATPER,NULL VATAMT,CONVERT(VARCHAR(10),NULL) VATCATID"
        strSql += " ,CONVERT(VARCHAR(100),NULL) BALANCE"
        strSql += vbCrLf + " ,SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT - CASE WHEN PAYMODE = 'JE' THEN TDSAMOUNT ELSE 0 END ELSE NULL END) AS DEBIT"
        strSql += vbCrLf + " ,SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT - CASE WHEN PAYMODE = 'JE' THEN TDSAMOUNT ELSE 0 END ELSE NULL END) AS CREDIT"
        strSql += vbCrLf + " ,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = T.SCOSTID)SCOSTCENTRE"
        'strSql += vbCrLf + " ,SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT-TDSAMOUNT ELSE NULL END) AS DEBIT"
        'strSql += vbCrLf + " ,SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT-TDSAMOUNT ELSE NULL END) AS CREDIT"
        strSql += vbCrLf + " ,CONVERT(VARCHAR,FLAG)GENBY,WT_ENTORDER AS KEYNO"
        strSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN T"
        strSql += vbCrLf + " WHERE BATCHNO = '" & gridView_OWN.Rows(rwIndex).Cells("BATCHNO").Value.ToString & "'"
        strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'  "
        strSql += vbCrLf + " AND ISNULL(FLAG,'') <> 'T'"
        'strSql += " AND ISNULL(WT_ENTORDER,0) <> 0"
        strSql += vbCrLf + " GROUP BY TRANMODE,ACCODE,REMARK1,REMARK2,CHQCARDNO,CHQDATE,CHQCARDREF,WT_ENTORDER,FLAG,TRANDATE,BATCHNO,SCOSTID"
        strSql += vbCrLf + " ORDER BY WT_ENTORDER"
        Dim dtAcc As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtAcc)
        If Not dtAcc.Rows.Count > 0 Then
Acc_NotFound:
            MsgBox("Record Not Found", MsgBoxStyle.Information)
            Exit Function
        Else
            strSql = "SELECT TOP 1 TRANDATE,TRANNO,PAYMODE,COSTID,BATCHNO,CHQCARDNO,CHQDATE,CHQCARDREF "
            strSql += vbCrLf + " ,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = T.COSTID)COSTNAME"
            strSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN T WHERE BATCHNO = '" & gridView_OWN.Rows(rwIndex).Cells("BATCHNO").Value.ToString & "'"
            strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
            strSql += vbCrLf + " AND PAYMODE IN (SELECT PAYMODE FROM " & cnAdminDb & "..ACCENTRYMASTER WHERE ACTIVE = 'Y')"
            Dim dtAccDet As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtAccDet)
            If Not dtAccDet.Rows.Count > 0 Then
                GoTo Acc_NotFound
            End If
            Dim DrCaption As String = "Dr"
            Dim CrCAption As String = "Cr"
            For Each Row As DataRow In dtAcc.Rows
                DrCaption = objGPack.GetSqlValue("SELECT DRCAPTION FROM " & cnAdminDb & "..ACCENTRYMASTER WHERE PAYMODE = '" & dtAccDet.Rows(0).Item("PAYMODE").ToString & "' AND ISNULL(DRCAPTION,'') <> ''", , DrCaption)
                CrCAption = objGPack.GetSqlValue("SELECT CRCAPTION FROM " & cnAdminDb & "..ACCENTRYMASTER WHERE PAYMODE = '" & dtAccDet.Rows(0).Item("PAYMODE").ToString & "' AND ISNULL(CRCAPTION,'') <> ''", , CrCAption)
                Row!TYPE = IIf(Row!TYPE.ToString.ToUpper = "DR", DrCaption, CrCAption)
            Next
            dtAcc.AcceptChanges()

            Dim objAcc As New frmAccountsEnt(dtAccDet.Rows(0).Item("PAYMODE").ToString, dtAcc)
            objGPack.Validator_Object(objAcc)
            objAcc.WindowState = FormWindowState.Normal
            objAcc.StartPosition = FormStartPosition.Manual
            objAcc.Location = New Point((ScreenWid - objAcc.Width) / 2, (ScreenHit + 22 - objAcc.Height) / 2)
            'objAcc.Location = New Point((1024 - objAcc.Width) / 2, (790 - objAcc.Height) / 2)
            objAcc.TranNo = Val(dtAccDet.Rows(0).Item("TRANNO").ToString)
            objAcc.BatchNo = dtAccDet.Rows(0).Item("BATCHNO").ToString
            objAcc.CostId = dtAccDet.Rows(0).Item("COSTID").ToString
            objAcc.payMode = dtAccDet.Rows(0).Item("PAYMODE").ToString
            objAcc.dtpDate.Value = dtAccDet.Rows(0).Item("TRANDATE")
            objAcc.cmbCostCenter_MAN.Text = dtAccDet.Rows(0).Item("COSTNAME").ToString
            'objAcc.cmbCostCenter_MAN.Enabled = False
            objAcc.Edchqno = dtAccDet.Rows(0).Item("CHQCARDNO").ToString
            objAcc.Edchqdate = IIf(IsDBNull(dtAccDet.Rows(0).Item("CHQDATE")), Nothing, dtAccDet.Rows(0).Item("CHQDATE"))
            objAcc.Edchqdetail = dtAccDet.Rows(0).Item("CHQCARDREF").ToString
            objAcc.ShowDialog()
        End If
    End Function

    Function funcGridViewStyle() As Integer
        With gridView_OWN
            With .Columns("TRANNO")
                .Width = 68
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("TRANDATE")
                .HeaderText = "TRANDATE"
                .Width = 80
                .DefaultCellStyle.Format = "dd/MM/yyyy"
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("PAYMODE")
                .HeaderText = "TYPE"
                .Width = 50
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("CONTRA")
                .HeaderText = "PARTICULARS"
                .Width = 440 - IIf(chkWithRunningBalance.Checked, 112, 0)
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("DEBIT")
                .Width = 170
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Format = "#,##0.00"
            End With
            With .Columns("CREDIT")
                .Width = 170
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Format = "#,##0.00"
            End With
            If .Columns.Contains("COMPANYNAME") Then
                With .Columns("COMPANYNAME")
                    .HeaderText = "COMPANY"
                    .Width = 150
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
            End If
            With .Columns("RUNTOTAL")
                .HeaderText = "RUNBAL"
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Width = 112
            End With

            For CNT As Integer = 6 To gridView_OWN.ColumnCount - 1
                .Columns(CNT).Visible = False
            Next
            .Columns("ENTREFNO").Visible = True
            .Columns("ENTREFNO").HeaderText = "MARK REFNO"
            .Columns("ENTREFNO").Width = 120
            .Columns("MARK").Visible = True
            If chkCmbCompany.CheckedItems.Count > 1 Then
                .Columns("COMPANYNAME").Visible = True
            End If

            If objMoreOption.rdbWithParticular.Checked = False Then
                With objMoreOption
                    gridView_OWN.Columns("CHQCARDNO").Visible = .chkChequeNo.Checked
                    If .chkChequeNo.Checked Then
                        gridView_OWN.Columns("CHQCARDNO").HeaderText = "CHQNO"
                        gridView_OWN.Columns("CHQCARDNO").SortMode = DataGridViewColumnSortMode.NotSortable
                        gridView_OWN.Columns("CHQCARDNO").Width = 100
                    End If
                    gridView_OWN.Columns("CHQDATE").Visible = .chkChequeDate.Checked
                    If .chkChequeDate.Checked Then
                        gridView_OWN.Columns("CHQDATE").SortMode = DataGridViewColumnSortMode.NotSortable
                        gridView_OWN.Columns("CHQDATE").Width = 100
                    End If
                    gridView_OWN.Columns("CHQCARDREF").Visible = .chkBankName.Checked
                    If .chkBankName.Checked Then
                        gridView_OWN.Columns("CHQCARDREF").HeaderText = "BANKNAME"
                        gridView_OWN.Columns("CHQCARDREF").SortMode = DataGridViewColumnSortMode.NotSortable
                        gridView_OWN.Columns("CHQCARDREF").Width = 120
                    End If
                    gridView_OWN.Columns("REMARK1").Visible = .chkRemark1.Checked
                    If .chkRemark1.Checked Then
                        gridView_OWN.Columns("REMARK1").SortMode = DataGridViewColumnSortMode.NotSortable
                        gridView_OWN.Columns("REMARK1").Width = 120
                    End If
                    gridView_OWN.Columns("REMARK2").Visible = .chkRemark2.Checked
                    If .chkRemark2.Checked Then
                        gridView_OWN.Columns("REMARK2").SortMode = DataGridViewColumnSortMode.NotSortable
                        gridView_OWN.Columns("REMARK2").Width = 120
                    End If
                    gridView_OWN.Columns("COSTNAME").Visible = .chkCostName.Checked
                    If .chkCostName.Checked Then
                        gridView_OWN.Columns("COSTNAME").SortMode = DataGridViewColumnSortMode.NotSortable
                        gridView_OWN.Columns("COSTNAME").Width = 100
                    End If
                    gridView_OWN.Columns("REFNO").Visible = .chkRefNo.Checked
                    If .chkRefNo.Checked Then
                        gridView_OWN.Columns("REFNO").SortMode = DataGridViewColumnSortMode.NotSortable
                        gridView_OWN.Columns("REFNO").Width = 100
                    End If
                    gridView_OWN.Columns("REFDATE").Visible = .chkRefDate.Checked
                    If .chkRefDate.Checked Then
                        gridView_OWN.Columns("REFDATE").SortMode = DataGridViewColumnSortMode.NotSortable
                        gridView_OWN.Columns("REFDATE").Width = 100
                    End If
                    gridView_OWN.Columns("USERNAME").Visible = .chkUserName.Checked
                    If .chkUserName.Checked Then
                        gridView_OWN.Columns("USERNAME").SortMode = DataGridViewColumnSortMode.NotSortable
                        gridView_OWN.Columns("USERNAME").Width = 120
                    End If
                End With
            End If
            .Columns("RUNTOTAL").Visible = chkWithRunningBalance.Checked
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 9, FontStyle.Bold)
            .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Font = New Font("VERDANA", 8, FontStyle.Regular)
        End With
    End Function

    Function funcTrailBalFiltration() As String
        Dim qry As String = Nothing
        qry += " WHERE 1 = 1"
        qry += " AND ACCODE ='DRS'"
        If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
            qry += " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & GetQryString(chkCmbCostCentre.Text) & "))"
        End If
        If chkCmbCompany.Text <> "ALL" And chkCmbCompany.Text <> "" Then
            qry += " AND COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(chkCmbCompany.Text) & "))"
        Else
            qry += " AND COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N')"
        End If
        Return qry
    End Function

    Function funcTranFiltration() As String
        Dim qry As String = Nothing
        qry += " AND ACCODE = 'DRS'"
        If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
            qry += " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & GetQryString(chkCmbCostCentre.Text) & "))"
        End If
        If objMoreOption.cmbFindAmount.Text <> "" Then
            qry += " AND AMOUNT " & objMoreOption.cmbFindAmount.Text & " " & Val(objMoreOption.txtFindAmount_AMT.Text) & ""
        End If
        If GetCheckedItems(objMoreOption.lstContra) <> Nothing Then
            qry += " AND CONTRA IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME IN (" & GetCheckedItems(objMoreOption.lstContra) & "))"
        End If
        If objMoreOption.txtFindNarration.Text <> "" Then
            qry += " AND (REMARK1 LIKE '%" & objMoreOption.txtFindNarration.Text & "%' OR REMARK1 LIKE '%" & objMoreOption.txtFindNarration.Text & "%')"
        End If
        If objMoreOption.cmbVoucherType.Text <> "" Then
            qry += " AND BATCHNO IN (SELECT DISTINCT BATCHNO FROM " & cnStockDb & "..ACCTRAN WHERE TRANDATE BETWEEN '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "' AND PAYMODE = (SELECT PAYMODE FROM " & cnAdminDb & "..ACCENTRYMASTER WHERE CAPTION = '" & objMoreOption.cmbVoucherType.Text & "'))"
        End If
        If chkCmbCompany.Text <> "ALL" And chkCmbCompany.Text <> "" Then
            qry += " AND COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(chkCmbCompany.Text) & "))"
        Else
            qry += " AND COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N')"
        End If
        If cmbLedger.Enabled = True And cmbLedger.Text <> "" Then
            qry += " AND SACCODE = (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbLedger.Text & "')"
        End If
        qry += " AND ISNULL(CANCEL,'') = ''"
        Return qry
    End Function

    Function funcBillViewDetails(ByVal rwIndex As Integer) As Integer

        strSql = " SELECT T.TRANNO,CONVERT(VARCHAR,T.TRANDATE,103)AS TRANDATE,HE.ACNAME"
        strSql += " ,CASE WHEN T.TRANMODE = 'D' AND T.AMOUNT <> 0 THEN T.AMOUNT ELSE NULL END AS DEBIT"
        strSql += " ,CASE WHEN T.TRANMODE = 'C' AND T.AMOUNT <> 0 THEN T.AMOUNT ELSE NULL END AS CREDIT,T.COMPANYID"
        strSql += " ,REMARK1 + CASE WHEN ISNULL(REMARK2,'')<>'' THEN  '/' + REMARK2 ELSE REMARK2 END REMARK FROM " & cnStockDb & "..ACCTRAN AS T"
        strSql += " INNER JOIN " & cnAdminDb & "..ACHEAD AS HE ON HE.ACCODE = T.ACCODE"
        strSql += " WHERE "
        strSql += " T.BATCHNO = '" & gridView_OWN.Rows(rwIndex).Cells("BATCHNO").Value.ToString & "'"
        strSql += " AND  T.COMPANYID = '" & strCompanyId & "'"
        'strSql += " "
        'strSql = " SELECT TRANNO,TRANDATE,ACNAME,CASE WHEN DEBIT - CREDIT > 0  THEN DEBIT - CREDIT ELSE 0 END AS DEBIT"
        'strSql += " ,CASE WHEN CREDIT - DEBIT > 0  THEN CREDIT - DEBIT ELSE 0 END AS CREDIT"
        'strSql += " FROM ("
        'strSql += " SELECT TRANNO,CONVERT(VARCHAR,TRANDATE,103)TRANDATE"
        'strSql += " ,(SELECT TOP 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE =T.ACCODE )AS ACNAME"
        'strSql += " ,SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE 0 END) AS DEBIT"
        'strSql += " ,SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE 0 END) AS CREDIT"
        'strSql += " FROM " & cnStockDb & "..ACCTRAN AS T"
        'strSql += " WHERE " 'TRANNO = " & Val(gridView_OWN.Rows(rwIndex).Cells("TRANNO").Value.ToString) & " "
        'strSql += " BATCHNO = '" & gridView_OWN.Rows(rwIndex).Cells("BATCHNO").Value.ToString & "'"
        'strSql += " AND  T.COMPANYID = '" & strCompanyId & "'"
        'strSql += " GROUP BY ACCODE,TRANNO,TRANDATE"
        'strSql += " )X"
        dtBillView.Rows.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtBillView)
        If dtBillView.Rows.Count > 0 Then
            grpBillView.Visible = True
            gridBillView.Focus()
        End If
        funcGridBIllViewFormating()
    End Function

    Function funcBillViewGridStyle() As Integer
        With gridBillView
            With .Columns("TRANNO")
                .Width = 70
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("TRANDATE")
                .Width = 80
            End With
            With .Columns("ACNAME")
                .Width = 300
            End With
            With .Columns("REMARK")
                .Width = 300
            End With
            With .Columns("DEBIT")
                .Width = 112
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.Format = "#,##0.00"
            End With
            With .Columns("CREDIT")
                .Width = 112
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.Format = "#,##0.00"
            End With
        End With
    End Function

    Function funcSummaryGridStyle() As Integer
        With gridSummary
            '58,60,500
            With .Columns("COL1")
                .Width = 86
                .DefaultCellStyle.BackColor = SystemColors.Control
                .DefaultCellStyle.SelectionBackColor = SystemColors.Control
            End With
            With .Columns("COL2")
                .Width = 300 - IIf(chkWithRunningBalance.Checked, 112, 0)
            End With
            With .Columns("COL3") '310+140 = 450    
                .Width = 70
                .DefaultCellStyle.BackColor = SystemColors.Control
                .DefaultCellStyle.SelectionBackColor = SystemColors.Control
            End With '146+390+110
            With .Columns("COL4")
                .Width = 71
            End With
            With .Columns("COL5")
                .Width = 112
                .DefaultCellStyle.BackColor = SystemColors.Control
                .DefaultCellStyle.SelectionBackColor = SystemColors.Control
            End With
            With .Columns("COL6")
                .Width = 170
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("COL7")
                .Width = 170
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            .Columns("COL8").Width = 112
            .Columns("COL8").Visible = chkWithRunningBalance.Checked
        End With
    End Function

    Private Sub ClearSummaryDetails()
        For Each ro As DataRow In dtSummary.Rows
            ro("COL2") = ""
            ro("COL4") = ""
            ro("COL6") = ""
            ro("COL7") = ""
            ro("COL8") = ""
        Next
    End Sub

    Private Sub SetSummaryDetails(ByVal rwIndex As Integer, ByVal col1 As String, ByVal col2 As String, _
    ByVal col3 As String, ByVal col4 As String, ByVal col5 As String, _
    ByVal col6 As String, ByVal col7 As String)
        If Not rwIndex <= dtSummary.Rows.Count - 1 Then Exit Sub
        With dtSummary.Rows(rwIndex)
            .Item("COL1") = col1
            .Item("COL2") = col2
            .Item("COL3") = col3
            .Item("COL4") = col4
            .Item("COL5") = col5
            .Item("COL6") = col6
            .Item("COL7") = col7
        End With
    End Sub

    Function funcSummaryDetails(ByVal rwIndex As Integer) As Integer
        ClearSummaryDetails()
        Dim totDebit As Double = Nothing
        Dim totCredit As Double = Nothing

        For Each totRow As DataRow In _dtMark.Select("1=1")
            totDebit += totRow.Item("DEBIT").ToString
            totCredit += totRow.Item("CREDIT").ToString
        Next
        
            SetSummaryDetails(0, "", "", "", "", "MARKED VALUE", totDebit, totCredit)
            SetSummaryDetails(1, "", "", "", "", "DIFF AMOUNT", IIf((totDebit - totCredit) >= 0, (totDebit - totCredit), ""), IIf((totDebit - totCredit) < 0, (totCredit - totDebit), ""))

            '        SetSummaryDetails(1, issue + "DATE", .Cells("CHQDATE").Value.ToString, _
            '        "GRSWT", IIf(Val(.Cells("GRSWT").Value.ToString) <> 0, .Cells("GRSWT").Value.ToString, ""), "TOTAL", totDebit, totCredit)
            '        SetSummaryDetails(2, "BANKNAME", .Cells("CHQCARDREF").Value.ToString, _
            '        "NETWT", IIf(Val(.Cells("NETWT").Value.ToString) <> 0, .Cells("NETWT").Value.ToString, ""), "BALANCE", balDebit, balCredit)
            '        SetSummaryDetails(3, "ACNAME", .Cells("ACNAME").Value.ToString, _
            '        "REFNO", IIf(.Cells("REFNO").Value.ToString <> "0", .Cells("REFNO").Value.ToString, ""), _
            '        "TOTPCS", totPcs, "")
            '        If .Cells("REFDATE").Value.ToString <> "" Then
            '            SetSummaryDetails(4, "REMARK1", .Cells("REMARK1").Value.ToString, _
            '            "REFDATE", Format(.Cells("REFDATE").Value, "dd/MM/yyyy"), "TOTGRSWT", totGrswt, "")
            '        Else
            '            SetSummaryDetails(4, "REMARK1", .Cells("REMARK1").Value.ToString, _
            '"REFDATE", "", "TOTGRSWT", totGrswt, "")
            '        End If
            '        SetSummaryDetails(5, "REMARK2", .Cells("REMARK2").Value.ToString, _
            '        "", "", "TOTNETWT", totNetWt, "")
            '        SetSummaryDetails(6, "UPDATED", .Cells("UPDATED").Value.ToString, _
            '        "COSTNAME", .Cells("COSTNAME").Value.ToString, "USERNAME", .Cells("USERNAME").Value.ToString, "")
        'End With
        gridSummary.Refresh()
    End Function

    

    Function funcAccLedger() As Integer
        Dim ftrTrailbal As String = Nothing
        Dim ftrTran As String = Nothing
        ftrTrailbal = funcTrailBalFiltration()
        ftrTran = funcTranFiltration()

        ProgressBarStep("Retrieving Data", 10)
        strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ACC')>0 DROP TABLE TEMP" & systemId & "ACC"
        strSql += vbCrLf + " DECLARE @FRMDATE SMALLDATETIME"
        strSql += vbCrLf + " DECLARE @TODATE SMALLDATETIME"
        strSql += vbCrLf + " SELECT @FRMDATE = '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " SELECT @TODATE = '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " SELECT *"
        strSql += vbCrLf + " INTO TEMP" & systemId & "ACC"
        strSql += vbCrLf + " FROM"
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " SELECT CONVERT(VARCHAR,'')SNO,CONVERT(INT,NULL)TRANNO,CONVERT(SMALLDATETIME,NULL) TRANDATE,CONVERT(VARCHAR(40),NULL) PAYMODE,'OPENING...' as CONTRA,''SUBLEDGER"
        strSql += vbCrLf + " ,CASE WHEN SUM(DEBIT) - SUM(CREDIT) > 0 THEN SUM(DEBIT) - SUM(CREDIT) ELSE 0 END AS DEBIT"
        strSql += vbCrLf + " ,CASE WHEN SUM(CREDIT)- SUM(DEBIT) > 0 THEN SUM(CREDIT)- SUM(DEBIT) ELSE 0 END AS CREDIT"
        strSql += vbCrLf + " ,ACCODE"
        strSql += vbCrLf + " ,CONVERT(VARCHAR(11),NULL)BATCHNO,CONVERT(VARCHAR(40),NULL)REFNO,CONVERT(SMALLDATETIME,NULL) REFDATE,CONVERT(VARCHAR(55),NULL)REMARK1,CONVERT(VARCHAR(55),NULL)REMARK2"
        strSql += vbCrLf + " ,1 RESULT"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),NULL)RUNTOT,CONVERT(VARCHAR(1),NULL)CUR "
        strSql += vbCrLf + " ,NULL CHQCARDNO,NULL CARDID,NULL CHQCARDREF,CONVERT(VARCHAR(10),NULL) CHQDATE"
        strSql += vbCrLf + " ,NULL PCS"
        strSql += vbCrLf + " ,NULL GRSWT,NULL NETWT,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = X.ACCODE)AS ACNAME,CONVERT(VARCHAR(2),NULL)FROMFLAG,CONVERT(VARCHAR(3),NULL)COSTID,COMPANYID"
        strSql += vbCrLf + " ,CONVERT(VARCHAR(25),NULL)UPDATED,CONVERT(VARCHAR(50),NULL)USERNAME,CONVERT(VARCHAR(50),NULL)ENTREFNO"
        strSql += vbCrLf + " FROM "
        strSql += vbCrLf + " 	("
        strSql += vbCrLf + " 	SELECT 'TRI_OPEN'SEP,' 'TRANNO,@FRMDATE TRANDATE"
        strSql += vbCrLf + " 	,(SELECT TOP 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.ACCODE)AS ACNAME"
        strSql += vbCrLf + "    ,'' SUBLEDGER"
        strSql += vbCrLf + " 	,SUM(DEBIT)DEBIT"
        strSql += vbCrLf + " 	,SUM(CREDIT)CREDIT"
        strSql += vbCrLf + " 	,ACCODE,NULL PCS,NULL GRSWT,NULL NETWT,COSTID,COMPANYID,NULL ENTREFNO"
        strSql += vbCrLf + " 	FROM " & cnStockDb & "..OPENTRAILBALANCE T"
        strSql += ftrTrailbal
        strSql += vbCrLf + " 	GROUP BY ACCODE,COSTID,COMPANYID"
        strSql += vbCrLf + " 	UNION ALL"
        strSql += vbCrLf + " 	SELECT 'ACC_OPEN'SEP,CONVERT(VARCHAR,TRANNO)TRANNO,TRANDATE"
        strSql += vbCrLf + " 	,(SELECT TOP 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.ACCODE) AS ACNAME"
        strSql += vbCrLf + "   ,'' SUBLEDGER"
        strSql += vbCrLf + " 	,ISNULL(SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT END),0) AS DEBIT"
        strSql += vbCrLf + " 	,ISNULL(SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT END),0) AS CREDIT"
        strSql += vbCrLf + " 	,ACCODE,PCS"
        strSql += vbCrLf + " 	,GRSWT,NETWT"
        strSql += vbCrLf + " 	,COSTID,COMPANYID,NULL ENTREFNO"
        strSql += vbCrLf + " 	FROM " & cnStockDb & "..ACCTRAN AS T"
        strSql += vbCrLf + " 	WHERE TRANDATE < @FRMDATE"
        strSql += ftrTran
        strSql += vbCrLf + " 	GROUP BY TRANNO,TRANDATE,ACCODE,SACCODE,PCS,GRSWT,NETWT,COSTID,COMPANYID"
        strSql += vbCrLf + " 	)X "
        strSql += vbCrLf + " GROUP BY ACCODE,COMPANYID"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT SNO,CONVERT(VARCHAR,TRANNO)TRANNO,TRANDATE,PAYMODE"
        strSql += vbCrLf + " ,(SELECT TOP 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.CONTRA)CONTRA"
        strSql += vbCrLf + " ,(SELECT TOP 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.SACCODE)SUBLEDGER"
        strSql += vbCrLf + " ,ISNULL(CASE WHEN TRANMODE = 'D' THEN AMOUNT END,0) AS DEBIT"
        strSql += vbCrLf + " ,ISNULL(CASE WHEN TRANMODE = 'C' THEN AMOUNT END,0) AS CREDIT"
        strSql += vbCrLf + " ,ACCODE"
        strSql += vbCrLf + " ,BATCHNO,REFNO,REFDATE,REMARK1,REMARK2"
        strSql += vbCrLf + " ,2 RESULT"
        strSql += vbCrLf + " ,0 RUNTOT,' 'CUR "
        strSql += vbCrLf + " ,CHQCARDNO,CARDID,CHQCARDREF,CONVERT(VARCHAR,CHQDATE,103)CHQDATE"
        strSql += vbCrLf + " ,PCS,GRSWT,NETWT,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.ACCODE)AS ACNAME,FROMFLAG,COSTID,COMPANYID"
        strSql += vbCrLf + " ,CONVERT(VARCHAR(25),CONVERT(VARCHAR,UPDATED,103)+ ' ' + CONVERT(VARCHAR,UPTIME,108))UPDATED"
        strSql += vbCrLf + " ,'['+CONVERT(VARCHAR,USERID)+']'+ (SELECT USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERID = T.USERID)AS USERNAME"
        strSql += vbCrLf + " ,ENTREFNO FROM " & cnStockDb & "..ACCTRAN AS T"
        strSql += vbCrLf + " WHERE TRANDATE BETWEEN @FRMDATE AND @TODATE"
        strSql += ftrTran
        strSql += vbCrLf + " )X"
        'strSql += vbCrLf + " ORDER BY ACCODE,TRANDATE,TRANNO"

        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        ProgressBarStep("Calculating Total", 10)
        strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ACCTOTAL')>0 DROP TABLE TEMP" & systemId & "ACCTOTAL"
        strSql += vbCrLf + "  SELECT 3 RESULT,ACCODE,'EXCLUDING OPENING'CONTRA,SUM(DEBIT)DEBIT,SUM(CREDIT)CREDIT "
        strSql += vbCrLf + " INTO TEMP" & systemId & "ACCTOTAL"
        strSql += vbCrLf + " FROM TEMP" & systemId & "ACC"
        strSql += vbCrLf + "  WHERE RESULT = 2"
        strSql += vbCrLf + "  GROUP BY ACCODE"
        strSql += vbCrLf + "  UNION ALL"
        strSql += vbCrLf + "  SELECT 4 RESULT,ACCODE,'TOTAL'CONTRA,SUM(DEBIT)DEBIT,SUM(CREDIT)CREDIT FROM TEMP" & systemId & "ACC"
        strSql += vbCrLf + "  WHERE RESULT IN(1,2)"
        strSql += vbCrLf + "  GROUP BY ACCODE"
        strSql += vbCrLf + "  UNION ALL"
        strSql += vbCrLf + "  SELECT 5 RESULT,ACCODE,'BALANCE'CONTRA"
        strSql += vbCrLf + "  ,CASE WHEN AMOUNT > 0 THEN AMOUNT ELSE null END AS DEBIT"
        strSql += vbCrLf + "  ,CASE WHEN AMOUNT < 0 THEN -1*AMOUNT ELSE null END AS CREDIT"
        strSql += vbCrLf + "  	FROM"
        strSql += vbCrLf + "  	("
        strSql += vbCrLf + "  	SELECT ACCODE,SUM(DEBIT)-SUM(CREDIT) AS AMOUNT FROM TEMP" & systemId & "ACC"
        strSql += vbCrLf + "  	WHERE RESULT IN(1,2)"
        strSql += vbCrLf + "  	GROUP BY ACCODE"
        strSql += vbCrLf + "  )X"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        ProgressBarStep("", 10)
        strSql = " SELECT * FROM TEMP" & systemId & "ACCTOTAL"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtViewTotal)
        ProgressBarStep("", 10)
        strSql = " INSERT INTO TEMP" & systemId & "ACC(RESULT,ACCODE,CONTRA,DEBIT,CREDIT,ACNAME)"
        If chkMultiSelect.Checked Then ' If chkCmbAcName.Text = "ALL" Then
            strSql += vbCrLf + " SELECT DISTINCT 0 RESULT,ACCODE"
            strSql += vbCrLf + " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.ACCODE)"
            strSql += vbCrLf + " ,0,0"
            strSql += vbCrLf + " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.ACCODE)"
            strSql += vbCrLf + " FROM TEMP" & systemId & "ACC T"
            strSql += vbCrLf + "  UNION ALL"
        End If
        strSql += vbCrLf + " SELECT * "
        strSql += vbCrLf + " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.ACCODE)"
        strSql += vbCrLf + " FROM TEMP" & systemId & "ACCTOTAL T"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        ProgressBarStep("Generate Ledger Table", 10)
        strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ACCLEDGER')>0 DROP TABLE TEMP" & systemId & "ACCLEDGER"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strSql = " CREATE TABLE TEMP" & systemId & "ACCLEDGER"
        strSql += vbCrLf + " (TRANNO INT,TRANDATE SMALLDATETIME,PAYMODE VARCHAR (2),CONTRA VARCHAR (1000)"
        strSql += vbCrLf + " ,DEBIT NUMERIC (20,2),CREDIT NUMERIC (20,2),ACCODE VARCHAR (7)"
        strSql += vbCrLf + " ,BATCHNO VARCHAR (15),REFNO VARCHAR (40),REFDATE SMALLDATETIME,REMARK1 VARCHAR (200)"
        strSql += vbCrLf + " ,REMARK2 VARCHAR (200),RESULT INT,RUNTOTAL VARCHAR(20),RUNTOT NUMERIC (20,2),CUR VARCHAR (1)"
        strSql += vbCrLf + " ,CHQCARDNO VARCHAR(30),CARDID INT,CHQCARDREF VARCHAR(100),CHQDATE VARCHAR(10)"
        strSql += vbCrLf + " ,PCS INT,GRSWT NUMERIC(15,3),NETWT NUMERIC(15,3),ACNAME VARCHAR(55),SUBLEDGER VARCHAR(55)"
        strSql += vbCrLf + " ,SNO1 INT IDENTITY(1,1),FROMFLAG VARCHAR(2),COSTID VARCHAR(3),UPDATED VARCHAR(25),COSTNAME VARCHAR(50),USERNAME VARCHAR(50),COMPANYID VARCHAR(50),COMPANYNAME VARCHAR(500),ENTREFNO VARCHAR(15),SNO VARCHAR(20))"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strSql = " INSERT INTO TEMP" & systemId & "ACCLEDGER"
        strSql += vbCrLf + " (TRANNO,TRANDATE,PAYMODE,CONTRA,DEBIT,CREDIT,ACCODE,BATCHNO,REFNO"
        strSql += vbCrLf + " ,REFDATE,REMARK1,REMARK2,RESULT,RUNTOT,CUR,CHQCARDNO,CARDID,CHQCARDREF"
        strSql += vbCrLf + " ,CHQDATE,PCS,GRSWT,NETWT,ACNAME,FROMFLAG,COSTID,UPDATED,COSTNAME,USERNAME,COMPANYID"
        strSql += vbCrLf + " ,COMPANYNAME,SUBLEDGER,ENTREFNO,SNO)"
        strSql += vbCrLf + " SELECT"
        strSql += vbCrLf + " TRANNO,TRANDATE,PAYMODE,CONTRA,DEBIT,CREDIT,ACCODE,BATCHNO,REFNO"
        strSql += vbCrLf + " ,REFDATE,REMARK1,REMARK2,RESULT,RUNTOT,CUR,CHQCARDNO,CARDID,CHQCARDREF"
        strSql += vbCrLf + " ,CHQDATE,PCS,GRSWT,NETWT,ACNAME,FROMFLAG,T.COSTID,T.UPDATED,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = T.COSTID)AS COSTNAME,USERNAME,T.COMPANYID,C.COMPANYNAME,SUBLEDGER,ENTREFNO,SNO "
        strSql += vbCrLf + " FROM TEMP" & systemId & "ACC T"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..COMPANY C ON T.COMPANYID=C.COMPANYID"
        strSql += vbCrLf + " WHERE ISNULL(C.ACTIVE,'')<>'N'"
        If chkOrderbyTranNo.Checked Then
            strSql += vbCrLf + " ORDER BY ACNAME,ACCODE,RESULT,TRANDATE,TRANNO,DEBIT DESC,ENTREFNO "
        Else
            strSql += vbCrLf + " ORDER BY ACNAME,ACCODE,RESULT,TRANDATE,DEBIT DESC,TRANNO,ENTREFNO "
        End If
        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        With objMoreOption
            If .rdbWithParticular.Checked = True Then
                If .chkSubledger.Checked Or .chkChequeNo.Checked Or .chkChequeDate.Checked Or .chkBankName.Checked Or .chkRemark1.Checked Or .chkRemark2.Checked Or .chkRefNo.Checked Or .chkRefDate.Checked Or .chkCostName.Checked Or .chkUserName.Checked Then
                    strSql = " UPDATE TEMP" & systemId & "ACCLEDGER"
                    strSql += vbCrLf + " SET CONTRA = CONTRA "
                    If .chkSubledger.Checked Then
                        strSql += vbCrLf + " + CASE WHEN ISNULL(SUBLEDGER,'') <> '' THEN CHAR(13)+'  ' + SUBLEDGER ELSE '' END"
                    End If

                    If .chkChequeNo.Checked Then
                        strSql += vbCrLf + " + CASE WHEN ISNULL(CHQCARDNO,'') <> '' THEN CHAR(13)+'   CHQ NO : ' + CHQCARDNO ELSE '' END"
                    End If
                    If .chkChequeDate.Checked Then
                        strSql += vbCrLf + " + CASE WHEN ISNULL(CHQDATE,'') <> '' THEN CHAR(13)+'   CHQ DATE : ' + CHQDATE ELSE '' END"
                    End If
                    If .chkBankName.Checked Then
                        strSql += vbCrLf + " + CASE WHEN ISNULL(CHQCARDREF,'') <> '' THEN CHAR(13)+'   ' + CHQCARDREF ELSE '' END"
                    End If
                    If .chkRefNo.Checked Or .chkRefDate.Checked Then
                        'strSql += vbCrLf + " + CHAR(13) + '  '"
                        strSql += vbCrLf + " + CASE WHEN ISNULL(REFNO,'') <> '' OR ISNULL(REFDATE,'') <> '' THEN CHAR(13) ELSE '' END"
                        strSql += vbCrLf + " + CASE WHEN ISNULL(REFNO,'') <> '' THEN ' REFNO : ' + REFNO ELSE '' END"
                        strSql += vbCrLf + " + CASE WHEN ISNULL(REFDATE,'') <> '' THEN ' REFDATE : ' + CONVERT(VARCHAR,REFDATE,103) ELSE '' END"
                    End If
                    If .chkCostName.Checked Then
                        strSql += vbCrLf + " + CASE WHEN ISNULL(COSTNAME,'') <> '' THEN CHAR(13) + ' COST : ' + COSTNAME ELSE '' END"
                    End If
                    If .chkUserName.Checked Then
                        strSql += vbCrLf + " + CASE WHEN ISNULL(USERNAME,'') <> '' THEN CHAR(13) + ' USERID : ' + USERNAME ELSE '' END"
                    End If
                    If .chkRemark1.Checked Then
                        strSql += vbCrLf + " + CASE WHEN ISNULL(REMARK1,'') <> '' THEN CHAR(13)+'  ' + REMARK1 ELSE '' END"
                    End If
                    If .chkRemark2.Checked Then
                        strSql += vbCrLf + " + CASE WHEN ISNULL(REMARK2,'') <> '' THEN  CHAR(13)+'  ' +REMARK2 ELSE '' END"
                    End If
                    cmd = New OleDbCommand(strSql, cn)
                    cmd.CommandTimeout = 1000
                    cmd.ExecuteNonQuery()
                End If
            End If
        End With

        'ProgressBarStep("Inserting Row no", 10)
        ' ''ADDING SNO INDENTITY COLUMN
        'strSql = " ALTER TABLE TEMP" & systemId & "ACCLEDGER ADD SNO INT IDENTITY"
        'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        'cmd.CommandTimeout = 1000
        'cmd.ExecuteNonQuery()
        ProgressBarStep("Calculating Running Total", 10)
        ''MANIPULATING RUNTOT
        strSql = "  DECLARE @ACCODE VARCHAR(10)"
        strSql += vbCrLf + "  DECLARE @PREACCODE VARCHAR(10)"
        strSql += vbCrLf + "  DECLARE @DEBIT NUMERIC(20,2)"
        strSql += vbCrLf + "  DECLARE @CREDIT NUMERIC(20,2)"
        strSql += vbCrLf + "  DECLARE @RUNTOT NUMERIC(20,2)"
        strSql += vbCrLf + "  DECLARE RUNCUR CURSOR"
        strSql += vbCrLf + "  FOR SELECT DEBIT,CREDIT,ACCODE FROM TEMP" & systemId & "ACCLEDGER WHERE RESULT IN (1,2)"
        strSql += vbCrLf + "  OPEN RUNCUR"
        strSql += vbCrLf + "  WHILE 1=1"
        strSql += vbCrLf + "  BEGIN"
        strSql += vbCrLf + "  	FETCH NEXT FROM RUNCUR INTO @DEBIT,@CREDIT,@ACCODE"
        strSql += vbCrLf + "  	IF @@FETCH_STATUS = -1 BREAK"
        strSql += vbCrLf + "  	IF ISNULL(@ACCODE,'') <> ISNULL(@PREACCODE ,'')"
        strSql += vbCrLf + "  	BEGIN"
        strSql += vbCrLf + "  	SELECT @PREACCODE = @ACCODE"
        strSql += vbCrLf + "  	SELECT @RUNTOT = 0"
        strSql += vbCrLf + "  	END"
        strSql += vbCrLf + "  	SELECT @RUNTOT = ISNULL(@RUNTOT,0) + ISNULL(@DEBIT,0) - ISNULL(@CREDIT,0)"
        strSql += vbCrLf + "  	PRINT @RUNTOT"
        strSql += vbCrLf + "  	UPDATE TEMP" & systemId & "ACCLEDGER SET "
        strSql += vbCrLf + "  			     RUNTOT = CASE WHEN @RUNTOT > 0 THEN @RUNTOT ELSE -1*@RUNTOT END,"
        strSql += vbCrLf + "  			     CUR    = CASE WHEN @RUNTOT > 0 THEN 'D' ELSE 'C' END"
        'If chkGrpTranNo.Checked Then
        'strSql += vbCrLf + "  ,RUNTOTAL = CASE WHEN @RUNTOT > 0 THEN @RUNTOT ELSE -1*@RUNTOT END"
        ' Else
        strSql += vbCrLf + "                 ,RUNTOTAL = CONVERT(VARCHAR,@RUNTOT) "
        ' End If
        If chkGrpTranNo.Checked = False Then strSql += vbCrLf + "  + CASE WHEN @RUNTOT > 0 THEN ' Dr' ELSE ' Cr' END"
        strSql += vbCrLf + "  			     WHERE CURRENT OF RUNCUR"
        strSql += vbCrLf + "  END"
        strSql += vbCrLf + "  CLOSE RUNCUR"
        strSql += vbCrLf + "  DEALLOCATE RUNCUR"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        ProgressBarStep("", 10)
    End Function


    Private Sub frmAccountsLedger_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If objSearch IsNot Nothing Then
            objSearch = Nothing
        End If
    End Sub

    Private Sub frmAccountsLedger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Try
            If e.KeyChar = Chr(Keys.Escape) Then
                If grpBillView.Visible = True Then
                    grpBillView.Visible = False
                    gridView_OWN.Focus()
                    Me.Refresh()
                    Exit Sub
                End If
                If tabMain.SelectedTab.Name = tabView.Name Then
                    If grpSumOfSelectedRows.Visible = True Then
                        grpSumOfSelectedRows.Visible = False
                        gridView_OWN.Focus()
                        Me.Refresh()
                        Exit Sub
                    End If
                    If grpReportOptions.Visible = True Then
                        grpReportOptions.Visible = False
                        gridView_OWN.Focus()
                        Me.Refresh()
                        Exit Sub
                    End If
                    If flag = True Then
                        Me.Refresh()
                        Me.Close()
                    End If
                    tabMain.SelectedTab = tabGeneral
                    Me.SelectNextControl(tabGeneral, True, True, True, True)
                End If
            ElseIf e.KeyChar = Chr(Keys.Enter) Then
                'If dtpTo.Focused Then Exit Sub
                'If lstAccGroup.Focused Then Exit Sub
                'If lstAccName.Focused Then Exit Sub
                'If lstAccCostCentre.Focused Then Exit Sub
                If txtAcname.Focused Then Exit Sub
                SendKeys.Send("{TAB}")
            End If



            If UCase(e.KeyChar) = Chr(Keys.L) Then
                If gridView_OWN.RowCount > 0 Then
                    If gridView_OWN.Visible = True Then
                        Dim result = MessageBox.Show("Do you want to Print Dotmatrix(Yes) or Crystal Report(No)...", "Message", MessageBoxButtons.YesNoCancel)
                        If result = Windows.Forms.DialogResult.Cancel Then
                        ElseIf result = Windows.Forms.DialogResult.No Then
                            'CrystalPrint()
                        ElseIf result = Windows.Forms.DialogResult.Yes Then
                            'LedgerDotmatrix()
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            MsgBox("ERROR: " + ex.Message + vbCrLf + vbCrLf + "POSITION: " + ex.StackTrace, MsgBoxStyle.Critical, "ERROR MESSAGE")
        End Try
    End Sub

    Private Sub frmAccountsLedger_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Me.WindowState = FormWindowState.Maximized
        Me.tabMain.ItemSize = New Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGeneral.Left, Me.tabGeneral.Top, Me.tabGeneral.Width, Me.tabGeneral.Height))
        gridView_OWN.MultiSelect = True
        gridView_OWN.BorderStyle = BorderStyle.None
        cmbAcGroup.Location = chkCmbAcGroup.Location
        cmbAcName.Location = chkCmbAcName.Location
        strSql = " SELECT PAYMODE FROM " & cnAdminDb & "..ACCENTRYMASTER WHERE ACTIVE = 'Y'"
        Dim dtAccEnt As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtAccEnt)
        For Each ro As DataRow In dtAccEnt.Rows
            AccEntry.Add(ro!PAYMODE.ToString)
        Next
        btnNew_Click(Me, New EventArgs)
    End Sub

    Public Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        Ismaxdrcr = False
        Dim previous As Integer
        Dim page As Integer = 1
        Dim current As Integer
        Dim zeroPage As Integer

        funcSummaryGridStyle()
        gridView_OWN.RowTemplate.Height = 18
        gridSummary.BackgroundColor = Me.BackColor
        Try
            If ExCalling = False Then Prop_Sets()
            pnlHeading.Visible = False
            btnView_Search.Enabled = False
            ProgressBarShow()
            ProgressBarStep("", 10)
            funcAccLedger()
            ProgressBarStep("Fill into Datatable", 10)
            If chkGrpTranNo.Checked Then
                strSql = " IF (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "LEDGER')>0 DROP TABLE TEMPTABLEDB..TEMP" & systemId & "LEDGER"
                strSql += " SELECT TRANNO,TRANDATE,PAYMODE,CONTRA,DEBIT,CREDIT,ACCODE,'' BATCHNO,REFNO,ISNULL(REFDATE,'')REFDATE,CONVERT(VARCHAR(50),RUNTOTAL)RUNTOTAL"
                strSql += " ,RUNTOT,PCS,GRSWT,NETWT,CUR,CHQCARDNO,CARDID,ISNULL(CHQDATE,'')CHQDATE,CHQCARDREF,REMARK1,REMARK2,ACNAME,FROMFLAG,COSTID,'' UPDATED,COSTNAME,'' USERNAME"
                strSql += " ,COMPANYID,RESULT,ISNULL(ENTREFNO,'')ENTREFNO  "
                strSql += " INTO TEMPTABLEDB..TEMP" & systemId & "LEDGER"
                strSql += " FROM ("
                strSql += " SELECT TRANNO,TRANDATE,PAYMODE,ACNAME CONTRA,SUM(DEBIT)DEBIT,SUM(CREDIT)CREDIT,ACCODE,'' BATCHNO,REFNO,REFDATE,SUM(CONVERT(NUMERIC(18,2),RUNTOTAL)) AS  RUNTOTAL"
                strSql += " ,SUM(RUNTOT)RUNTOT,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,CUR,CHQCARDNO,CARDID,CHQDATE,CHQCARDREF,REMARK1,REMARK2,ACNAME,FROMFLAG,COSTID,'' UPDATED,COSTNAME,'' USERNAME"
                strSql += " ,COMPANYID,RESULT,ENTREFNO  FROM TEMP" & systemId & "ACCLEDGER"
                strSql += " GROUP BY TRANNO,TRANDATE,PAYMODE,ACCODE,REFNO,REFDATE,CUR,CHQCARDNO,CARDID,CHQDATE,CHQCARDREF,REMARK1,REMARK2,ACNAME,FROMFLAG,COSTID,COSTNAME"
                strSql += " ,COMPANYID,RESULT,ENTREFNO "
                strSql += " )X"
                strSql += " ORDER BY TRANDATE"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                strSql = " UPDATE TEMPTABLEDB..TEMP" & systemId & "LEDGER SET CONTRA='EXCLUDING OPENING' WHERE RESULT=3"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
                strSql = " UPDATE TEMPTABLEDB..TEMP" & systemId & "LEDGER SET CONTRA='TOTAL' WHERE RESULT=4"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
                strSql = " UPDATE TEMPTABLEDB..TEMP" & systemId & "LEDGER SET CONTRA='BALANCE' WHERE RESULT=5"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                strSql = " SELECT "
                strSql += " TRANNO, TRANDATE, PAYMODE, CONTRA, DEBIT, CREDIT, ACCODE, BATCHNO, REFNO, REFDATE"
                strSql += " ,(CASE WHEN CUR='C' THEN  RUNTOTAL + ' Cr'  ELSE RUNTOTAL +' Dr' END) AS RUNTOTAL    "
                strSql += " ,RUNTOT,PCS,GRSWT,NETWT,CUR,CHQCARDNO,CARDID,CHQDATE,CHQCARDREF,REMARK1,REMARK2,ACNAME,FROMFLAG"
                strSql += " ,COSTID,UPDATED,COSTNAME,USERNAME,COMPANYID,RESULT,ENTREFNO,CONVERT(VARCHAR(1),'')MARK  "
                strSql += " FROM TEMPTABLEDB..TEMP" & systemId & "LEDGER ORDER BY RESULT"
            Else
                strSql = " UPDATE TEMP" & systemId & "ACCLEDGER SET RUNTOTAL=REPLACE(RUNTOTAL,'-','') "
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                strSql = " SELECT *,CONVERT(VARCHAR(1),'')MARK" ',CONVERT(VARCHAR(10),'')CHECKNO"
                strSql += " FROM TEMP" & systemId & "ACCLEDGER"
                strSql += " ORDER BY SNO1"
            End If
            dtGridView = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtGridView)
            If Not dtGridView.Rows.Count > 0 Then
                MsgBox("RECORD NOT FOUND")
                Exit Sub
            End If

            ProgressBarStep("Fill into Grid", 10)
            dtContraSummary = New DataTable
            objContraSummary = New frmGridDispDia
            objContraSummary.Name = Me.Name
            objGPack.Validator_Object(objContraSummary)
            tabView.Show()
            gridView_OWN.DataSource = dtGridView
            GridViewFormat()
            ProgressBarStep("", 10)
            funcGridViewStyle()
            If chkGrpTranNo.Checked Then

            End If
            strSql = " SELECT ACCODE,SUM(ISNULL(PCS,0))TOTPCS,SUM(ISNULL(GRSWT,0))TOTGRSWT,SUM(ISNULL(NETWT,0))TOTNETWT FROM TEMP" & systemId & "ACCLEDGER"
            strSql += " GROUP BY ACCODE"
            da = New OleDbDataAdapter(strSql, cn)
            dtTotPcsWt.Clear()
            da.Fill(dtTotPcsWt)
            ProgressBarStep("", 10)
            Dim title As String = " LEDGER FROM " & dtpFrom.Value.ToString("dd-MM-yyyy") & "  TO " & dtpTo.Value.ToString("dd-MM-yyyy") + vbCrLf
            'title = GetSelectedItems(lstAccName, "") & " ACCOUNTS LEDGER " + vbCrLf

            If chkCmbAcGroup.Visible Then
                title += chkCmbAcName.Text
            Else
                title += cmbAcName.Text
                If cmbLedger.Enabled = True And cmbLedger.Text <> "" Then title += "(" & cmbLedger.Text + ")"
                title += " LEDGER"
                Dim tin As String = objGPack.GetSqlValue("SELECT TIN FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbAcName.Text & "'")
                Dim PAN As String = objGPack.GetSqlValue("SELECT PAN FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbAcName.Text & "'")
                If tin <> "" Then title += " TIN : " & tin
                If PAN <> "" Then title += " PAN : " & PAN
                title += vbCrLf
            End If
            title += " "
            Dim fltrTit As String = Nothing
            'fltrTit = GetSelectedItems(lstAccCostCentre, "")
            fltrTit = chkCmbCostCentre.Text
            fltrTit += GetCheckedItems(objMoreOption.lstContra, "")
            If objMoreOption.cmbFindAmount.Text <> "" Then
                fltrTit += " AMOUNT " & objMoreOption.cmbFindAmount.Text & "" & objMoreOption.txtFindAmount_AMT.Text & ""
            End If
            If fltrTit <> Nothing Then fltrTit += ","
            If objMoreOption.txtFindNarration.Text <> "" Then
                fltrTit += " NARRATION LIKE " & objMoreOption.txtFindNarration.Text & ""
            End If
            If fltrTit.EndsWith(",") Then fltrTit = Mid(fltrTit, 1, fltrTit.Length - 1)
            If fltrTit <> Nothing Then
                title += "(" + fltrTit + ")"
            End If

            lblTitle.Text = title
            lblTitle.Font = New Font("VERDANA", 9, FontStyle.Bold)
            pnlHeading.Visible = True
            txtSumCredit.Clear()
            txtSumDebit.Clear()
            tabMain.SelectedTab = tabView
            objSearch = New frmGridSearch(gridView_OWN)
            gridView_OWN.Focus()
            tabView.Show()
            InitializedMarkdatatable()
        Catch ex As Exception
            MsgBox("ERROR: " + ex.Message + vbCrLf + vbCrLf + "POSITION: " + ex.StackTrace, MsgBoxStyle.Critical, "ERROR MESSAGE")
        Finally
            ProgressBarHide()
            btnView_Search.Enabled = True
            Me.Cursor = Cursors.Default
        End Try
    End Sub
    Function MAXDRCR()
        strSql = " SELECT max(runtot) CR FROM TEMP" & systemId & "ACCLEDGER WHERE cur = 'C' AND RESULT = 2"
        Dim maxcr As Decimal = Val(GetSqlValue(cn, strSql).ToString)
        Dim maxcrdate As String
        If maxcr <> 0 Then maxcrdate = GetSqlValue(cn, "SELECT top 1 CONVERT(VARCHAR(13),trandate,103) date FROM TEMP" & systemId & "ACCLEDGER WHERE cur = 'C' AND RESULT = 2 AND RUNTOT = " & maxcr).ToString
        strSql = " SELECT max(RUNTOT) dr FROM TEMP" & systemId & "ACCLEDGER WHERE CUR = 'D' AND RESULT = 2"
        Dim maxdr As Decimal = Val(GetSqlValue(cn, strSql).ToString)
        Dim maxdrdate As String
        If maxdr <> 0 Then maxdrdate = GetSqlValue(cn, "SELECT top 1 CONVERT(VARCHAR(13),trandate,103) date FROM TEMP" & systemId & "ACCLEDGER WHERE cur = 'D' AND RESULT = 2 AND RUNTOT = " & maxdr).ToString


        Dim GridDt As New DataTable
        GridDt = CType(gridView_OWN.DataSource, DataTable)
        Dim dmaxr As DataRow
        dmaxr = GridDt.NewRow
        dmaxr!contra = "Max. Cr Value On " & maxcrdate
        If chkWithRunningBalance.Checked Then dmaxr!runtotal = maxcr & " Cr" Else dmaxr!credit = maxcr
        GridDt.Rows.Add(dmaxr)
        dmaxr = GridDt.NewRow
        dmaxr!contra = "Max. Dr Value On " & maxdrdate
        If chkWithRunningBalance.Checked Then dmaxr!runtotal = maxdr & " Dr" Else dmaxr!debit = maxdr
        GridDt.Rows.Add(dmaxr)
        gridView_OWN.DataSource = GridDt
        GridViewFormat()
        ProgressBarStep("", 10)
        funcGridViewStyle()
        Ismaxdrcr = True
    End Function

    Private Sub gridView_OWN_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView_OWN.KeyDown
        If Not gridView_OWN.RowCount > 0 Then Exit Sub
        If gridView_OWN.CurrentRow Is Nothing Then Exit Sub
        If gridView_OWN.CurrentRow.Cells("SNO").Value.ToString = "" Then Exit Sub
        If e.KeyCode = Keys.Space Then
            Dim ro As DataRow
            With gridView_OWN.CurrentRow
                If .Cells("MARK").Value = "Marked" And .Cells("ENTREFNO").Value = "" Then
                    .Cells("MARK").Value = ""
                    .DefaultCellStyle.BackColor = Color.White
                    Dim dv As New DataView
                    dv = _dtMark.DefaultView
                    dv.RowFilter = "(SNO<>'" & .Cells("SNO").Value.ToString & "')"
                    _dtMark = dv.ToTable
                ElseIf .Cells("MARK").Value <> "Marked" And .Cells("ENTREFNO").Value = "" Then
                    .Cells("MARK").Value = "Marked"
                    .DefaultCellStyle.BackColor = Color.Pink
                    ro = _dtMark.NewRow
                    ro("CREDIT") = Val(.Cells("CREDIT").Value.ToString)
                    ro("DEBIT") = Val(.Cells("DEBIT").Value.ToString)
                    ro("SNO") = .Cells("SNO").Value.ToString
                    _dtMark.Rows.Add(ro)
                End If
                _dtMark.AcceptChanges()
                funcSummaryDetails(.Index)
                gridView_OWN.Refresh()
            End With
        ElseIf e.KeyCode = Keys.Escape Then
            btnUpdate.Focus()
        End If
        If e.Control Then
            If e.KeyCode = Keys.F Then
                FindSearchToolStripMenuItem_Click(Me, New EventArgs)
            ElseIf e.KeyCode = Keys.S Then
                If objSearch IsNot Nothing Then
                    CType(objSearch, frmGridSearch).btnFindNext_Click(Me, New EventArgs)
                End If
            End If
        End If
    End Sub

    '========================================================================
    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView_OWN.KeyPress
        Try
            If Not gridView_OWN.RowCount > 0 Then
                Exit Sub
            End If
            If e.KeyChar = Chr(Keys.Enter) Then
                Dim rwIndex As Integer
                If gridView_OWN.RowCount = 1 Then
                    rwIndex = gridView_OWN.CurrentRow.Index
                Else
                    rwIndex = gridView_OWN.CurrentRow.Index - 1
                End If
                gridView_OWN.CurrentCell = gridView_OWN.Rows(rwIndex).Cells(0)
            End If
            If e.KeyChar = "R" Or e.KeyChar = "r" Then
                grpReportOptions.Visible = True
                grpReportOptions.BackColor = System.Drawing.SystemColors.InactiveCaptionText
                grpReportOptions.Size = New System.Drawing.Size(353, 56)
                grpReportOptions.Location = New System.Drawing.Point(288, 183)
                grpReportOptions.Focus()
            ElseIf UCase(e.KeyChar) = "E" Then
                '  btnEdit_Click(Me, New EventArgs)
            End If
            If e.KeyChar = "X" Or e.KeyChar = "x" Then
                'Me.btnExcel_Click(Me, New EventArgs)
            ElseIf UCase(e.KeyChar) = "P" Then
                'Me.btnPrint_Click(Me, New EventArgs)
            ElseIf UCase(e.KeyChar) = "C" Then
                If btnCancel.Enabled Then Me.btnCancel_Click(Me, New EventArgs)
            End If
        Catch ex As Exception
            MsgBox("ERROR: " + ex.Message + vbCrLf + vbCrLf + "POSITION: " + ex.StackTrace, MsgBoxStyle.Critical, "ERROR MESSAGE")
        End Try
    End Sub


    '========================================================================
    '16.09.08 modified

    Function funcGridSummaryFormating() As Integer
        If gridSummary.Rows.Count > 0 Then
            If gridSummary.Columns.Count > 0 Then
                For rwIndex As Integer = 0 To gridSummary.Rows.Count - 1
                    gridSummary.Rows(rwIndex).Cells(0).Style.BackColor = System.Drawing.SystemColors.ControlLight
                    gridSummary.Rows(rwIndex).Cells(2).Style.BackColor = System.Drawing.SystemColors.ControlLight
                    gridSummary.Rows(rwIndex).Cells(4).Style.BackColor = System.Drawing.SystemColors.ControlLight
                    If rwIndex < 3 Then
                        If Val(gridSummary.Rows(rwIndex).Cells(3).Value) = 0 Then
                            gridSummary.Rows(rwIndex).Cells(3).Value = ""
                        End If
                        If Val(gridSummary.Rows(rwIndex).Cells(3).Value) <> 0 Then
                            gridSummary.Rows(rwIndex).Cells(3).Style.BackColor = Color.Lavender
                        End If
                        If Val(gridSummary.Rows(rwIndex).Cells(5).Value) = 0 Then
                            gridSummary.Rows(rwIndex).Cells(5).Value = ""
                        End If
                        If Val(gridSummary.Rows(rwIndex).Cells(5).Value) <> 0 Then
                            gridSummary.Rows(rwIndex).Cells(5).Style.BackColor = Color.Lavender
                        End If
                        If Val(gridSummary.Rows(rwIndex).Cells(6).Value) = 0 Then
                            gridSummary.Rows(rwIndex).Cells(6).Value = ""
                        End If
                        If Val(gridSummary.Rows(rwIndex).Cells(6).Value) <> 0 Then
                            gridSummary.Rows(rwIndex).Cells(6).Style.BackColor = Color.Lavender
                        End If
                    End If
                    'For colIndex As Integer = 0 To gridSummary.ColumnCount - 1
                    '    gridSummary.Rows(rwIndex).Cells(colIndex).Style.SelectionBackColor = gridSummary.Rows(rwIndex).Cells(colIndex).Style.BackColor
                    '    gridSummary.Rows(rwIndex).Cells(colIndex).Style.SelectionForeColor = gridSummary.Rows(rwIndex).Cells(colIndex).Style.ForeColor
                    'Next colIndex
                Next rwIndex
            End If
        End If
    End Function

    Function funcGridBIllViewFormating() As Integer
        If gridBillView.RowCount > 0 Then
            If gridBillView.ColumnCount > 0 Then
                For rwIndex As Integer = 0 To gridBillView.Rows.Count - 1
                    If Val(gridBillView.Rows(rwIndex).Cells(3).Value.ToString) = 0 Then
                        gridBillView.Rows(rwIndex).Cells(3).Value = ""
                    End If
                    If Val(gridBillView.Rows(rwIndex).Cells(3).Value.ToString) <> 0 Then
                        gridBillView.Rows(rwIndex).Cells(3).Style.BackColor = Color.Lavender
                    End If
                    If Val(gridBillView.Rows(rwIndex).Cells(4).Value.ToString) = 0 Then
                        gridBillView.Rows(rwIndex).Cells(4).Value = ""
                    End If
                    If Val(gridBillView.Rows(rwIndex).Cells(4).Value.ToString) <> 0 Then
                        gridBillView.Rows(rwIndex).Cells(4).Style.BackColor = Color.LavenderBlush
                    End If
                Next rwIndex
            End If
        End If
    End Function

    '========================================================================
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub cmbAcGroup_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        e.KeyChar = UCase(e.KeyChar)
    End Sub


    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        Try
            objGPack.TextClear(Me)
            dtpFrom.Value = cnTranFromDate.Date.ToString("yyyy-MM-dd")
            dtpTo.Value = cnTranToDate.Date.ToString("yyyy-MM-dd")
            'chkMultiSelection.Checked = False
            objMoreOption = New frmAccLedgerMore
            BrighttechPack.GlobalMethods.FillCombo(chkCmbAcGroup, dtAcGroup, "ACGRPNAME", , "ALL")
            BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
            If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False
            BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dtCompany, "COMPANYNAME", , strCompanyName)
            chkMultiSelect.Checked = False
            objMoreOption.rdbWithParticular.Checked = True
            If ExCalling = False Then Prop_Gets()
            'cmbAccGroup.Focus()
            InitializedMarkdatatable()
            chkCmbAcGroup.Focus()
        Catch ex As Exception
            MsgBox("ERROR: " + ex.Message + vbCrLf + vbCrLf + "POSITION: " + ex.StackTrace, MsgBoxStyle.Critical, "ERROR MESSAGE")
        End Try
    End Sub
    Private Sub InitializedMarkdatatable()
        _dtMark = New DataTable
        _dtMark.Columns.Add("SNO", Type.GetType("System.String"))
        _dtMark.Columns.Add("CREDIT", Type.GetType("System.Double"))
        _dtMark.Columns.Add("DEBIT", Type.GetType("System.Double"))
        _dtMark.AcceptChanges()
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub cmbVoucherType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        e.KeyChar = UCase(e.KeyChar)
    End Sub
    '=================================================================================================
    '17.09.08
    'Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
    '    'Me.Cursor = Cursors.WaitCursor
    '    If gridView_OWN.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
    '        '593
    '        lblTitle.Text = Replace(lblTitle.Text, vbCrLf, " ")
    '        BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView_OWN, BrightPosting.GExport.GExportType.Export)
    '        '593
    '    End If
    '    Me.Cursor = Cursors.Arrow
    'End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        frmAccountsLedger_KeyPress(Me, New KeyPressEventArgs(ChrW(Keys.Escape)))
    End Sub

    Private Sub FindSearchToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FindSearchToolStripMenuItem.Click
        If Not gridView_OWN.RowCount > 0 Then
            Exit Sub
        End If
        If Not gridView_OWN.RowCount > 0 Then Exit Sub
        objSearch.Show()
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Try
            funcGridStyle(gridSummary)
            tabMain.ItemSize = New System.Drawing.Size(1, 1)
            If GetAdmindbSoftValue("COSTCENTRE", "Y").ToUpper = "Y" Then
                ''COSTCENTRE
                strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
                strSql += " UNION ALL"
                strSql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
                strSql += " ORDER BY RESULT,COSTNAME"
                dtCostCentre = New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtCostCentre)
                BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , "ALL")
                chkCmbCostCentre.Enabled = True
            Else
                chkCmbCostCentre.Enabled = False
            End If
            strSql = " SELECT 'ALL' COMPANYNAME,'ALL' COMPANYID,1 RESULT"
            strSql += " UNION ALL"
            strSql += " SELECT COMPANYNAME,CONVERT(vARCHAR,COMPANYID),2 RESULT FROM " & cnAdminDb & "..COMPANY"
            strSql += " WHERE ISNULL(ACTIVE,'')<>'N'"
            strSql += " ORDER BY RESULT,COMPANYNAME"
            dtCompany = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtCompany)
            BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dtCompany, "COMPANYNAME", , "ALL")


            ''ACGROUP
            strSql = " SELECT 'ALL' ACGRPNAME,'ALL' ACGRPCODE,'1' RESULT"
            strSql += " UNION ALL"
            strSql += " SELECT ACGRPNAME,CONVERT(vARCHAR,ACGRPCODE),2 RESULT FROM " & cnAdminDb & "..ACGROUP"
            strSql += " ORDER BY RESULT,ACGRPNAME"
            dtAcGroup = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtAcGroup)
            BrighttechPack.GlobalMethods.FillCombo(chkCmbAcGroup, dtAcGroup, "ACGRPNAME", , "ALL")
            strSql = " SELECT 'ALL' ACGRPNAME,'ALL' ACGRPCODE,'1' RESULT"
            strSql += " UNION ALL"
            strSql += " SELECT ACGRPNAME,CONVERT(vARCHAR,ACGRPCODE),2 RESULT FROM " & cnAdminDb & "..ACGROUP"
            strSql += " ORDER BY RESULT,ACGRPNAME"
            objGPack.FillCombo(strSql, cmbAcGroup)

            strSql = " SELECT ' 'COL1,' 'COL2,' 'COL3,' 'COL4,' 'COL5,' 'COL6,' 'COL7 ,' 'COL8 where  1<>1"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtSummary)
            dtSummary.Rows.Add()
            dtSummary.Rows.Add()
            dtSummary.Rows.Add()
            dtSummary.Rows.Add()
            dtSummary.Rows.Add()
            dtSummary.Rows.Add()
            dtSummary.Rows.Add()
            gridSummary.DataSource = dtSummary
            funcSummaryGridStyle()

            strSql = "SELECT ' 'TRANNO,' 'TRANDATE,' 'ACNAME,' 'DEBIT,' 'CREDIT,' 'REMARK WHERE 1<>1"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtBillView)
            dtBillView.Rows.Clear()
            gridBillView.DataSource = dtBillView
            funcBillViewGridStyle()

            tabView.Controls.Add(grpBillView)
            grpBillView.Location = New System.Drawing.Point(54, 140)
            grpBillView.Size = New System.Drawing.Size(911, 264)
            grpBillView.BringToFront()
            grpBillView.Visible = False

            tabView.Controls.Add(grpSumOfSelectedRows)
            grpSumOfSelectedRows.BringToFront()
            grpSumOfSelectedRows.Visible = False

            tabView.Controls.Add(grpReportOptions)
            grpReportOptions.BringToFront()
            grpReportOptions.Visible = False
        Catch ex As Exception
            MsgBox("Message     :" + ex.Message + vbCrLf + "StackTrace    :" + ex.StackTrace)
        End Try
    End Sub


    Private Sub dtpFrom_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        dtpTo.MinimumDate = dtpFrom.Value
    End Sub

    Private Function GetCheckedItems(ByVal lst As CheckedListBox, Optional ByVal concatStr As String = "'") As String
        Dim ret As String = ""
        If Not lst.Items.Count > 0 Then Return Nothing
        For cnt As Integer = 0 To lst.CheckedItems.Count - 1
            ret += "'" & lst.CheckedItems.Item(cnt).ToString & "'"
            If cnt <> lst.CheckedItems.Count - 1 Then ret += ","
        Next
        If concatStr <> "'" Then
            ret = ret.ToString.Replace("'", concatStr)
        End If
        Return ret
    End Function

    Private Sub dtpTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If e.KeyChar = Chr(Keys.Enter) Then
            btnView_Search.Focus()
        End If
    End Sub

    Private Sub btnMore_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMore.Click
        If objMoreOption.Visible Then Exit Sub
        objMoreOption.BackColor = Me.BackColor
        objMoreOption.StartPosition = FormStartPosition.CenterScreen
        objMoreOption.MaximizeBox = False
        objMoreOption.ShowDialog()
        btnView_Search.Focus()
    End Sub

    Private Sub gridView_OWN_RowEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView_OWN.RowEnter
        If gridView_OWN.RowCount > 3 Then
            funcSummaryDetails(e.RowIndex)
        End If
    End Sub

    Private Sub GridViewFormat()
        For Each dgvRow As DataGridViewRow In gridView_OWN.Rows
            If dgvRow.Cells("CONTRA").Value.ToString.Contains(Chr(13)) Then
                dgvRow.DefaultCellStyle.WrapMode = DataGridViewTriState.True
                gridView_OWN.AutoResizeRow(dgvRow.Index, DataGridViewAutoSizeRowMode.AllCells)
            End If
            If Val(dgvRow.Cells("DEBIT").Value.ToString) = 0 Then
                dgvRow.Cells("DEBIT").Value = DBNull.Value
                dgvRow.Cells("DEBIT").Style.BackColor = dgvRow.Cells("CONTRA").Style.BackColor
            Else
                dgvRow.Cells("DEBIT").Style.BackColor = Color.Lavender
            End If
            If Val(dgvRow.Cells("CREDIT").Value.ToString) = 0 Then
                dgvRow.Cells("CREDIT").Value = DBNull.Value
                dgvRow.Cells("CREDIT").Style.BackColor = dgvRow.Cells("CONTRA").Style.BackColor
            Else
                dgvRow.Cells("CREDIT").Style.BackColor = Color.LavenderBlush
            End If
            If dgvRow.Cells("RUNTOTAL").Value.ToString.ToString.Contains("Dr") Then
                dgvRow.Cells("RUNTOTAL").Style.BackColor = Color.Lavender
            Else
                dgvRow.Cells("RUNTOTAL").Style.BackColor = Color.LavenderBlush
            End If
            Select Case dgvRow.Cells("RESULT").Value.ToString
                Case "0" 'TITLE
                    dgvRow.DefaultCellStyle.BackColor = Color.LightBlue
                    dgvRow.DefaultCellStyle.ForeColor = Color.Red
                    dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                Case "1" 'OPENING
                    dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                Case "2" 'TRANS
                Case "3" 'EXCLUD OPEN
                    dgvRow.DefaultCellStyle.BackColor = Color.LightGoldenrodYellow
                    dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                Case "4" 'TOTAL
                    dgvRow.DefaultCellStyle.BackColor = Color.LightGoldenrodYellow
                    dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                Case "5" 'BALANCE
                    dgvRow.DefaultCellStyle.BackColor = Color.LightGoldenrodYellow
                    dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            End Select
        Next
    End Sub

    Private Sub gridSummary_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles gridSummary.CellFormatting
        Try
            Select Case e.ColumnIndex
                Case 0, 2, 4
                    e.CellStyle.BackColor = System.Drawing.SystemColors.ControlLight
                Case 5
                    If e.RowIndex < 3 Then
                        If Val(e.Value.ToString) <> 0 Then
                            e.CellStyle.BackColor = Color.Lavender
                        End If
                    End If
                Case 6
                    If e.RowIndex < 3 Then
                        If Val(e.Value.ToString) <> 0 Then

                            e.CellStyle.BackColor = Color.LavenderBlush
                        End If
                    End If
            End Select
            e.CellStyle.SelectionBackColor = e.CellStyle.BackColor
            e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor
        Catch ex As Exception
            MsgBox("ERROR: " + ex.Message + vbCrLf + vbCrLf + "POSITION: " + ex.StackTrace, MsgBoxStyle.Critical, "ERROR MESSAGE")
        End Try
    End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        If gridView_OWN.CurrentRow Is Nothing Then Exit Sub
        Try
            If gridView_OWN.CurrentRow.Cells("ENTREFNO").Value.ToString = "" Or gridView_OWN.CurrentRow.Cells("SNO").Value.ToString = "" Then Exit Sub
            MsgBox("Are you sure to Cancel.", MsgBoxStyle.Information)
            strSql = " UPDATE " & cnStockDb & "..ACCTRAN SET ENTREFNO='' WHERE ENTREFNO='" & gridView_OWN.CurrentRow.Cells("ENTREFNO").Value.ToString & "'"
            cmd = New OleDbCommand(strSql, cn) : cmd.ExecuteNonQuery()
            btnView_Search_Click(Me, New EventArgs())
            MsgBox("Successfully Cancelled", MsgBoxStyle.Information)
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
        gridView_OWN.Focus()
    End Sub

    Private Sub chkCmbAcName_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCmbAcName.Enter
        ''ACNAME
        strSql = " SELECT ACNAME,CONVERT(vARCHAR,ACCODE),2 RESULT FROM " & cnAdminDb & "..ACHEAD"
        If chkCmbAcGroup.Text <> "ALL" Then
            strSql += " WHERE ISNULL(ACGRPCODE,0) IN (SELECT ACGRPCODE FROM " & cnAdminDb & "..ACGROUP WHERE ISNULL(ACGRPNAME,0) IN (" & GetQryString(chkCmbAcGroup.Text) & "))"
        End If
        strSql += " AND ACCODE='DRS'"
        strSql += " ORDER BY RESULT,ACNAME"
        dtAcName = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtAcName)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbAcName, dtAcName, "ACNAME", , "SUNDRY DEBTORS")
    End Sub


    Private Sub cmbAcGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAcGroup.SelectedIndexChanged
        strSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD "
        If cmbAcGroup.Text <> "ALL" And cmbAcGroup.Text <> "" Then
            strSql += " WHERE ACGRPCODE = (SELECT ACGRPCODE FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPNAME = '" & cmbAcGroup.Text & "')"
        End If
        strSql += " ORDER BY ACNAME"
        objGPack.FillCombo(strSql, cmbAcName, , False)
        dtAccNames = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtAccNames)
        DgvSearch.DataSource = dtAccNames
        DgvSearch.ColumnHeadersVisible = False
        DgvSearch.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DgvSearch.Visible = False
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New frmAccountsLedger_Properties
        obj.p_chkMultiSelect = chkMultiSelect.Checked
        GetChecked_CheckedList(chkCmbAcGroup, obj.p_chkCmbAcGroup)
        obj.p_cmbAcGroup = cmbAcGroup.Text
        GetChecked_CheckedList(chkCmbAcName, obj.p_chkCmbAcName)
        obj.p_cmbAcName = cmbAcName.Text
        obj.p_chkWithRunningBalance = chkWithRunningBalance.Checked
        obj.p_chkOrderbyTranNo = chkOrderbyTranNo.Checked
        GetChecked_CheckedList(chkCmbCompany, obj.p_chkCmbCompany)
        GetChecked_CheckedList(objMoreOption.lstContra, obj.p_lstContra)
        obj.p_cmbVoucherType = objMoreOption.cmbVoucherType.Text
        obj.p_cmbFindAmount = objMoreOption.cmbFindAmount.Text
        obj.p_txtFindAmount_AMT = objMoreOption.txtFindAmount_AMT.Text
        obj.p_txtFindNarration = objMoreOption.txtFindNarration.Text
        obj.p_chkChequeNo = objMoreOption.chkChequeNo.Checked
        obj.p_chkChequeDate = objMoreOption.chkChequeDate.Checked
        obj.p_chkChequeDate = objMoreOption.chkBankName.Checked
        obj.p_chkRemark1 = objMoreOption.chkRemark1.Checked
        obj.p_chkRemark2 = objMoreOption.chkRemark2.Checked
        obj.p_chkRefNo = objMoreOption.chkRefNo.Checked
        obj.p_chkRefDate = objMoreOption.chkRefDate.Checked
        obj.p_chkUserName = objMoreOption.chkUserName.Checked
        obj.p_chkCostName = objMoreOption.chkCostName.Checked
        obj.p_rdbWithParticular = objMoreOption.rdbWithParticular.Checked
        obj.p_rdbSepColumns = objMoreOption.rdbSepColumns.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmAccountsLedger_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmAccountsLedger_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmAccountsLedger_Properties))
        chkMultiSelect.Checked = obj.p_chkMultiSelect
        SetChecked_CheckedList(chkCmbAcGroup, obj.p_chkCmbAcGroup, "ALL")
        cmbAcGroup.Text = obj.p_cmbAcGroup
        SetChecked_CheckedList(chkCmbAcName, obj.p_chkCmbAcName, Nothing)
        cmbAcName.Text = obj.p_cmbAcName
        chkWithRunningBalance.Checked = obj.p_chkWithRunningBalance
        chkOrderbyTranNo.Checked = obj.p_chkOrderbyTranNo
        SetChecked_CheckedList(chkCmbCompany, obj.p_chkCmbCompany, cnCompanyName)
        SetChecked_CheckedList(objMoreOption.lstContra, obj.p_lstContra, Nothing)
        objMoreOption.cmbVoucherType.Text = obj.p_cmbVoucherType
        objMoreOption.cmbFindAmount.Text = obj.p_cmbFindAmount
        objMoreOption.txtFindAmount_AMT.Text = obj.p_txtFindAmount_AMT
        objMoreOption.txtFindNarration.Text = obj.p_txtFindNarration
        objMoreOption.chkChequeNo.Checked = obj.p_chkChequeNo
        objMoreOption.chkChequeDate.Checked = obj.p_chkChequeDate
        objMoreOption.chkBankName.Checked = obj.p_chkChequeDate
        objMoreOption.chkRemark1.Checked = obj.p_chkRemark1
        objMoreOption.chkRemark2.Checked = obj.p_chkRemark2
        objMoreOption.chkRefNo.Checked = obj.p_chkRefNo
        objMoreOption.chkRefDate.Checked = obj.p_chkRefDate
        objMoreOption.chkUserName.Checked = obj.p_chkUserName
        objMoreOption.chkCostName.Checked = obj.p_chkCostName
        objMoreOption.rdbWithParticular.Checked = obj.p_rdbWithParticular
        objMoreOption.rdbSepColumns.Checked = obj.p_rdbSepColumns
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.Close()
    End Sub

    Private Sub cmbAcName_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAcName.SelectedIndexChanged
        strSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD "
        strSql += " WHERE MACCODE = (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbAcName.Text & "')"
        strSql += " ORDER BY ACNAME"
        objGPack.FillCombo(strSql, cmbLedger, , False)
        If cmbLedger.Items.Count = 0 Then cmbLedger.Enabled = False Else cmbLedger.Enabled = True
    End Sub

    Private Sub btn_MaxRb_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If Ismaxdrcr Then Exit Sub
        MAXDRCR()
    End Sub

    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        If gridView_OWN.Rows.Count <= 0 Then Exit Sub
        If _dtMark.Rows.Count <= 0 Then Exit Sub
        Try
            Dim totDebit As Double = 0
            Dim totCredit As Double = 0
            For Each totRow As DataRow In _dtMark.Select("1=1")
                totDebit += totRow.Item("DEBIT").ToString
                totCredit += totRow.Item("CREDIT").ToString
            Next
            If totDebit <> totCredit Then
                MsgBox("Credit Debit Not Tally.") : Exit Sub
            End If
            tran = cn.BeginTransaction
            Dim ENTREFNO As String = ""
            ENTREFNO = Val(GetBillControlValue("DRS-MARKINGNO", tran, False)) + 1
            strSql = "UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT=" & ENTREFNO & " WHERE CTLID='DRS-MARKINGNO'"
            cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
            ENTREFNO = cnCostId & Mid(Format(System.DateTime.Now, "dd/MM/yyyy"), 9, 2).ToString & ENTREFNO
            For i As Integer = 0 To _dtMark.Rows.Count - 1
                strSql = "UPDATE " & cnStockDb & "..ACCTRAN SET ENTREFNO='" & ENTREFNO & "' WHERE SNO='" & _dtMark.Rows(i).Item("SNO").ToString & "'"
                cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
            Next
            tran.Commit()
            btnView_Search_Click(Me, New EventArgs())
            MsgBox("Update successfully.")
        Catch ex As Exception
            tran.Rollback()
        End Try
    End Sub
End Class

Public Class frmDrsMarking_Properties
    Private chkMultiSelect As Boolean = False
    Public Property p_chkMultiSelect() As Boolean
        Get
            Return chkMultiSelect
        End Get
        Set(ByVal value As Boolean)
            chkMultiSelect = value
        End Set
    End Property

    Private chkCmbAcGroup As New List(Of String)
    Public Property p_chkCmbAcGroup() As List(Of String)
        Get
            Return chkCmbAcGroup
        End Get
        Set(ByVal value As List(Of String))
            chkCmbAcGroup = value
        End Set
    End Property

    Private cmbAcGroup As String = Nothing
    Public Property p_cmbAcGroup() As String
        Get
            Return cmbAcGroup
        End Get
        Set(ByVal value As String)
            cmbAcGroup = value
        End Set
    End Property

    Private chkCmbAcName As New List(Of String)
    Public Property p_chkCmbAcName() As List(Of String)
        Get
            Return chkCmbAcName
        End Get
        Set(ByVal value As List(Of String))
            chkCmbAcName = value
        End Set
    End Property

    Private cmbAcName As String = Nothing
    Public Property p_cmbAcName() As String
        Get
            Return cmbAcName
        End Get
        Set(ByVal value As String)
            cmbAcName = value
        End Set
    End Property
    Private chkWithRunningBalance As Boolean = False
    Public Property p_chkWithRunningBalance() As Boolean
        Get
            Return chkWithRunningBalance
        End Get
        Set(ByVal value As Boolean)
            chkWithRunningBalance = value
        End Set
    End Property
    Private chkOrderbyTranNo As Boolean = False
    Public Property p_chkOrderbyTranNo() As Boolean
        Get
            Return chkOrderbyTranNo
        End Get
        Set(ByVal value As Boolean)
            chkOrderbyTranNo = value
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

    Private chkCmbCompany As New List(Of String)
    Public Property p_chkCmbCompany() As List(Of String)
        Get
            Return chkCmbCompany
        End Get
        Set(ByVal value As List(Of String))
            chkCmbCompany = value
        End Set
    End Property


    Private lstContra As New List(Of String)
    Public Property p_lstContra() As List(Of String)
        Get
            Return lstContra
        End Get
        Set(ByVal value As List(Of String))
            lstContra = value
        End Set
    End Property

    Private cmbVoucherType As String = Nothing
    Public Property p_cmbVoucherType() As String
        Get
            Return cmbVoucherType
        End Get
        Set(ByVal value As String)
            cmbVoucherType = value
        End Set
    End Property

    Private cmbFindAmount As String = Nothing
    Public Property p_cmbFindAmount() As String
        Get
            Return cmbFindAmount
        End Get
        Set(ByVal value As String)
            cmbFindAmount = value
        End Set
    End Property

    Private txtFindAmount_AMT As String = Nothing
    Public Property p_txtFindAmount_AMT() As String
        Get
            Return txtFindAmount_AMT
        End Get
        Set(ByVal value As String)
            txtFindAmount_AMT = value
        End Set
    End Property

    Private txtFindNarration As String = Nothing
    Public Property p_txtFindNarration() As String
        Get
            Return txtFindNarration
        End Get
        Set(ByVal value As String)
            txtFindNarration = value
        End Set
    End Property
    Private rdbWithParticular As Boolean = False
    Public Property p_rdbWithParticular() As Boolean
        Get
            Return rdbWithParticular
        End Get
        Set(ByVal value As Boolean)
            rdbWithParticular = value
        End Set
    End Property
    Private rdbSepColumns As Boolean = False
    Public Property p_rdbSepColumns()
        Get
            Return rdbSepColumns
        End Get
        Set(ByVal value)
            rdbSepColumns = value
        End Set
    End Property
    Private chkChequeNo As Boolean = False
    Public Property p_chkChequeNo() As Boolean
        Get
            Return chkChequeNo
        End Get
        Set(ByVal value As Boolean)
            chkChequeNo = value
        End Set
    End Property
    Private chkChequeDate As Boolean = False
    Public Property p_chkChequeDate() As Boolean
        Get
            Return chkChequeDate
        End Get
        Set(ByVal value As Boolean)
            chkChequeDate = value
        End Set
    End Property
    Private chkBankName As Boolean = False
    Public Property p_chkBankName() As Boolean
        Get
            Return chkBankName
        End Get
        Set(ByVal value As Boolean)
            chkBankName = value
        End Set
    End Property
    Private chkRemark1 As Boolean = False
    Public Property p_chkRemark1() As Boolean
        Get
            Return chkRemark1
        End Get
        Set(ByVal value As Boolean)
            chkRemark1 = value
        End Set
    End Property
    Private chkRemark2 As Boolean = False
    Public Property p_chkRemark2() As Boolean
        Get
            Return chkRemark2
        End Get
        Set(ByVal value As Boolean)
            chkRemark2 = value
        End Set
    End Property
    Private chkRefNo As Boolean = False
    Public Property p_chkRefNo() As Boolean
        Get
            Return chkRefNo
        End Get
        Set(ByVal value As Boolean)
            chkRefNo = value
        End Set
    End Property
    Private chkRefDate As Boolean = False
    Public Property p_chkRefDate() As Boolean
        Get
            Return chkRefDate
        End Get
        Set(ByVal value As Boolean)
            chkRefDate = value
        End Set
    End Property
    Private chkCostName As Boolean = False
    Public Property p_chkCostName() As Boolean
        Get
            Return chkCostName
        End Get
        Set(ByVal value As Boolean)
            chkCostName = value
        End Set
    End Property
    Private chkUserName As Boolean = False
    Public Property p_chkUserName() As Boolean
        Get
            Return chkUserName
        End Get
        Set(ByVal value As Boolean)
            chkUserName = value
        End Set
    End Property
End Class