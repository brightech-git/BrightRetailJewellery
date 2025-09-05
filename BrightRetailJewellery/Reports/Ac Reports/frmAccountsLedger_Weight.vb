Imports System.Data.OleDb
Imports System.IO

Public Class frmAccountsLedger_Weight
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
    Dim _WholeSaleFormat As Boolean = IIf(GetAdmindbSoftValue("LEDGERVIEW_WSALETYPE", "N") = "Y", True, False)



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
        strSql += " ,CONVERT(VARCHAR(100),NULL) BALANCE"
        strSql += vbCrLf + " ,SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT - CASE WHEN PAYMODE = 'JE' THEN TDSAMOUNT ELSE 0 END ELSE NULL END) AS DEBIT"
        strSql += vbCrLf + " ,SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT - CASE WHEN PAYMODE = 'JE' THEN TDSAMOUNT ELSE 0 END ELSE NULL END) AS CREDIT"
        'strSql += vbCrLf + " ,SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT-TDSAMOUNT ELSE NULL END) AS DEBIT"
        'strSql += vbCrLf + " ,SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT-TDSAMOUNT ELSE NULL END) AS CREDIT"
        strSql += vbCrLf + " ,CONVERT(VARCHAR,FLAG)GENBY,WT_ENTORDER AS KEYNO"
        strSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN T"
        strSql += vbCrLf + " WHERE BATCHNO = '" & gridView_OWN.Rows(rwIndex).Cells("BATCHNO").Value.ToString & "'"
        strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'  "
        strSql += vbCrLf + " AND ISNULL(FLAG,'') <> 'T'"
        'strSql += " AND ISNULL(WT_ENTORDER,0) <> 0"
        strSql += vbCrLf + " GROUP BY TRANMODE,ACCODE,REMARK1,REMARK2,CHQCARDNO,CHQDATE,CHQCARDREF,WT_ENTORDER,FLAG,TRANDATE,BATCHNO"
        strSql += vbCrLf + " ORDER BY WT_ENTORDER"
        Dim dtAcc As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtAcc)
        If Not dtAcc.Rows.Count > 0 Then
Acc_NotFound:
            MsgBox("Record Not Found", MsgBoxStyle.Information)
            Exit Function
        Else
            strSql = "SELECT TOP 1 TRANDATE,TRANNO,PAYMODE,COSTID,BATCHNO "
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
            If gridView_OWN.Columns.Contains("COMPANYNAME") = True Then
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

            With .Columns("RECEIPT")
                '.HeaderText = "ISSUE"
                .Width = 170
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Format = "#,##0.000"
            End With
            With .Columns("ISSUE")
                '.HeaderText = "RECEIPT"
                .Width = 170
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Format = "#,##0.000"
            End With

            If gridView_OWN.Columns.Contains("RUNTOTALWT") = True Then
                With .Columns("RUNTOTALWT")
                    .HeaderText = "RUNBALWT"
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Width = 112
                End With
            End If

            For CNT As Integer = 6 To gridView_OWN.ColumnCount - 1
                .Columns(CNT).Visible = False
            Next

            .Columns("RECEIPT").Visible = True
            .Columns("ISSUE").Visible = True
            If gridView_OWN.Columns.Contains("RUNTOTALWT") = True Then
                .Columns("RUNTOTALWT").Visible = chkWithRunningBalance.Checked
            End If

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
    Function funcGridViewStyle1() As Integer
        With DataGridView2
            With .Columns("CONTRA")
                .HeaderText = "PARTICULARS"
                .Width = 440
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



            With .Columns("RECEIPT")
                '.HeaderText = "ISSUE"
                .Width = 170
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Format = "#,##0.000"
            End With
            With .Columns("ISSUE")
                '.HeaderText = "RECEIPT"
                .Width = 170
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Format = "#,##0.000"
            End With



            'For CNT As Integer = 6 To DataGridView2.ColumnCount - 1
            '    .Columns(CNT).Visible = False
            'Next

            .Columns("RECEIPT").Visible = True
            .Columns("ISSUE").Visible = True
            .Columns("RESULT").Visible = False
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 9, FontStyle.Bold)
            .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Font = New Font("VERDANA", 8, FontStyle.Regular)
        End With
    End Function

    Function funcTrailBalFiltration() As String
        Dim qry As String = Nothing
        qry += " WHERE 1 = 1"
        'If GetSelectedItems(lstAccName) <> Nothing Then
        If chkMultiSelect.Checked Then
            If chkCmbAcName.Text <> "ALL" And chkCmbAcName.Text <> "" Then
                qry += " AND ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME IN  (" & GetQryString(chkCmbAcName.Text) & "))"
            End If
            'If GetSelectedItems(lstAccGroup) <> Nothing Then
            If chkCmbAcGroup.Text <> "ALL" And chkCmbAcGroup.Text <> "" Then
                qry += " AND ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACGRPCODE IN (SELECT ACGRPCODE FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPNAME IN (" & GetQryString(chkCmbAcGroup.Text) & ")))"
            End If
        Else
            qry += " AND ACCODE = (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbAcName.Text & "')"
        End If
        If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
            qry += " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & GetQryString(chkCmbCostCentre.Text) & "))"
        End If
        If chkCmbCompany.Text <> "ALL" And chkCmbCompany.Text <> "" Then
            qry += " AND COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(chkCmbCompany.Text) & ")  AND ISNULL(ACTIVE,'')<>'N')"
        Else
            qry += " AND COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N')"
        End If
        'qry += " AND COMPANYID = '" & strCompanyId & "' "
        Return qry
    End Function

    Function funcTranFiltration() As String
        Dim qry As String = Nothing
        If chkMultiSelect.Checked Then
            If chkCmbAcName.Text <> "ALL" And chkCmbAcName.Text <> "" Then
                qry += " AND ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME IN  (" & GetQryString(chkCmbAcName.Text) & "))"
            End If
            If chkCmbAcGroup.Text <> "ALL" And chkCmbAcGroup.Text <> "" Then
                qry += " AND ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACGRPCODE IN (SELECT ACGRPCODE FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPNAME IN (" & GetQryString(chkCmbAcGroup.Text) & ")))"
            End If
        Else
            qry += " AND ACCODE = (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbAcName.Text & "')"
        End If
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

        'qry += " AND COMPANYID = '" & strCompanyId & "' "

        qry += " AND ISNULL(CANCEL,'') = ''"
        Return qry
    End Function

    Function funcBillViewDetails(ByVal rwIndex As Integer) As Integer

        strSql = " SELECT T.TRANNO,CONVERT(VARCHAR,T.TRANDATE,103)AS TRANDATE,HE.ACNAME"
        strSql += " ,CASE WHEN T.TRANMODE = 'D' AND T.AMOUNT <> 0 THEN T.AMOUNT ELSE NULL END AS DEBIT"
        strSql += " ,CASE WHEN T.TRANMODE = 'C' AND T.AMOUNT <> 0 THEN T.AMOUNT ELSE NULL END AS CREDIT,T.COMPANYID"
        strSql += " ,REMARK1 + CASE WHEN ISNULL(REMARK2,'')<>'' THEN  '/' + REMARK2 ELSE REMARK2 END REMARK FROM " & cnStockDb & "..ACCTRAN AS T"
        strSql += " INNER JOIN " & cnAdminDb & "..ACHEAD AS HE ON HE.ACCODE = T.ACCODE"
        strSql += " WHERE " 'TRANNO = " & Val(gridView_OWN.Rows(rwIndex).Cells("TRANNO").Value.ToString) & " "
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
        Dim totPcs As String = Nothing
        Dim totGrswt As String = Nothing
        Dim totNetWt As String = Nothing
        Dim totDebit As String = Nothing
        Dim totCredit As String = Nothing
        Dim balDebit As String = Nothing
        Dim balCredit As String = Nothing
        Dim accode As String = gridView_OWN.CurrentRow.Cells("ACCODE").Value.ToString
        For Each totRow As DataRow In dtTotPcsWt.Select("ACCODE = '" & accode & "'", "ACCODE")
            totPcs = IIf(Val(totRow.Item("TOTPCS").ToString) > 0, totRow.Item("TOTPCS").ToString, "")
            totGrswt = IIf(Val(totRow.Item("TOTGRSWT").ToString) > 0, Format(Val(totRow.Item("TOTGRSWT").ToString), "0.000"), "")
            totNetWt = IIf(Val(totRow.Item("TOTNETWT").ToString) > 0, Format(Val(totRow.Item("TOTNETWT").ToString), "0.000"), "")
        Next
        For Each totRow As DataRow In dtViewTotal.Select("ACCODE = '" & accode & "' AND RESULT = 4")
            totDebit = totRow.Item("DEBIT").ToString
            totCredit = totRow.Item("CREDIT").ToString
        Next
        For Each totRow As DataRow In dtViewTotal.Select("ACCODE = '" & accode & "' AND RESULT = 5")
            balDebit = totRow.Item("DEBIT").ToString
            balCredit = totRow.Item("CREDIT").ToString
        Next
        With gridView_OWN.Rows(rwIndex)
            Dim issue As String = Nothing
            If gridView_OWN.Rows(rwIndex).Cells("PAYMODE").Value.ToString = "CH" Then
                issue = "CHQ "
            Else
                issue = "CARD "
            End If
            Dim curDebit As String
            Dim curCredit As String
            If .Cells("CUR").Value.ToString = "D" Then
                curDebit = .Cells("RUNTOT").Value.ToString
                curCredit = ""
            Else
                curDebit = ""
                curCredit = .Cells("RUNTOT").Value.ToString
            End If
            SetSummaryDetails(0, issue + "NO", .Cells("CHQCARDNO").Value.ToString, _
            "PCS", IIf(Val(.Cells("PCS").Value.ToString) <> 0, .Cells("PCS").Value.ToString, ""), "CURSOR BAL", curDebit, curCredit)
            SetSummaryDetails(1, issue + "DATE", .Cells("CHQDATE").Value.ToString, _
            "GRSWT", IIf(Val(.Cells("GRSWT").Value.ToString) <> 0, .Cells("GRSWT").Value.ToString, ""), "TOTAL", totDebit, totCredit)
            SetSummaryDetails(2, "BANKNAME", .Cells("CHQCARDREF").Value.ToString, _
            "NETWT", IIf(Val(.Cells("NETWT").Value.ToString) <> 0, .Cells("NETWT").Value.ToString, ""), "BALANCE", balDebit, balCredit)
            SetSummaryDetails(3, "ACNAME", .Cells("ACNAME").Value.ToString, _
            "REFNO", IIf(.Cells("REFNO").Value.ToString <> "0", .Cells("REFNO").Value.ToString, ""), _
            "TOTPCS", totPcs, "")
            If .Cells("REFDATE").Value.ToString <> "" Then
                SetSummaryDetails(4, "REMARK1", .Cells("REMARK1").Value.ToString, _
                "REFDATE", Format(.Cells("REFDATE").Value, "dd/MM/yyyy"), "TOTGRSWT", totGrswt, "")
            Else
                SetSummaryDetails(4, "REMARK1", .Cells("REMARK1").Value.ToString, _
    "REFDATE", "", "TOTGRSWT", totGrswt, "")
            End If
            SetSummaryDetails(5, "REMARK2", .Cells("REMARK2").Value.ToString, _
            "", "", "TOTNETWT", totNetWt, "")
            SetSummaryDetails(6, "UPDATED", .Cells("UPDATED").Value.ToString, _
            "COSTNAME", .Cells("COSTNAME").Value.ToString, "USERNAME", .Cells("USERNAME").Value.ToString, "")
        End With
        gridSummary.Refresh()
    End Function

    Function funcContraWiseSummury() As Integer
        dtContraSummary = New DataTable
        strSql = " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ACCLEDGER_CONTSUMMARY')>0"
        strSql += " DROP TABLE TEMP" & systemId & "ACCLEDGER_CONTSUMMARY"
        strSql += " SELECT ACNAME,CONTRA"
        strSql += " ,CASE WHEN SUM(ISNULL(DEBIT,0))-SUM(ISNULL(CREDIT,0))>0 THEN SUM(ISNULL(DEBIT,0))-SUM(ISNULL(CREDIT,0)) ELSE 0 END AS DEBIT"
        strSql += " ,CASE WHEN SUM(ISNULL(CREDIT,0))-SUM(ISNULL(DEBIT,0))>0 THEN SUM(ISNULL(CREDIT,0))-SUM(ISNULL(DEBIT,0)) ELSE 0 END AS CREDIT"
        strSql += " ,2 RESULT"
        strSql += " INTO TEMP" & systemId & "ACCLEDGER_CONTSUMMARY"
        strSql += " FROM TEMP" & systemId & "ACCLEDGER"
        strSql += " WHERE RESULT = 2"
        strSql += " GROUP BY ACNAME,CONTRA"
        strSql += " UNION ALL"
        strSql += " SELECT ACNAME,CONTRA,SUM(ISNULL(DEBIT,0)) AS DEBIT,SUM(ISNULL(CREDIT,0))CREDIT,1 RESULT"
        strSql += " FROM TEMP" & systemId & "ACCLEDGER"
        strSql += " WHERE RESULT = 1"
        strSql += " GROUP BY ACNAME,CONTRA"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        If chkCmbAcName.Text = "ALL" Then
            strSql = " INSERT INTO TEMP" & systemId & "ACCLEDGER_CONTSUMMARY(ACNAME,CONTRA,RESULT)"
            strSql += " SELECT DISTINCT ACNAME,ACNAME,0 RESULT FROM TEMP" & systemId & "ACCLEDGER_CONTSUMMARY"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End If
        strSql = " "
        strSql += " INSERT INTO TEMP" & systemId & "ACCLEDGER_CONTSUMMARY"
        strSql += " SELECT ACNAME,'TOTAL' CONTRA,SUM(DEBIT)DEBIT,SUM(CREDIT)CREDIT,3 RESULT"
        strSql += " FROM TEMP" & systemId & "ACCLEDGER_CONTSUMMARY WHERE RESULT IN(1,2)"
        strSql += " GROUP BY ACNAME"
        strSql += " "
        strSql += " INSERT INTO TEMP" & systemId & "ACCLEDGER_CONTSUMMARY"
        strSql += " SELECT ACNAME,'BALANCE' CONTRA"
        strSql += " ,CASE WHEN SUM(ISNULL(DEBIT,0))-SUM(ISNULL(CREDIT,0))>0 THEN SUM(ISNULL(DEBIT,0))-SUM(ISNULL(CREDIT,0)) ELSE 0 END AS DEBIT"
        strSql += " ,CASE WHEN SUM(ISNULL(CREDIT,0))-SUM(ISNULL(DEBIT,0))>0 THEN SUM(ISNULL(CREDIT,0))-SUM(ISNULL(DEBIT,0)) ELSE 0 END AS CREDIT"
        strSql += " ,4 RESULT"
        strSql += " FROM TEMP" & systemId & "ACCLEDGER_CONTSUMMARY WHERE RESULT = 3"
        strSql += " GROUP BY ACNAME"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strSql = " SELECT * FROM TEMP" & systemId & "ACCLEDGER_CONTSUMMARY ORDER BY ACNAME,RESULT"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtContraSummary)
        If Not dtContraSummary.Rows.Count > 0 Then Exit Function
        objContraSummary = New frmGridDispDia
        objContraSummary.BaseName = Me.Name & "_ContraSummary"
        objContraSummary.Name = Me.Name
        objContraSummary.WindowState = FormWindowState.Normal
        objContraSummary.gridView.RowTemplate.Height = 21
        objContraSummary.gridView.DataSource = dtContraSummary
        objContraSummary.gridView.Columns("RESULT").Visible = False
        objContraSummary.gridView.Columns("ACNAME").Visible = False
        With objContraSummary.gridView.Columns("CONTRA")
            .HeaderText = "PARTICULARS"
            .Width = 410
            .SortMode = DataGridViewColumnSortMode.NotSortable
        End With
        With objContraSummary.gridView.Columns("DEBIT")
            .Width = 170
            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .SortMode = DataGridViewColumnSortMode.NotSortable
            .DefaultCellStyle.Format = "#,##0.00"
        End With
        With objContraSummary.gridView.Columns("CREDIT")
            .Width = 170
            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .SortMode = DataGridViewColumnSortMode.NotSortable
            .DefaultCellStyle.Format = "#,##0.00"
        End With
        objContraSummary.Size = New Size(778, 550)
        'objContraSummary.gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        objContraSummary.gridView.BackgroundColor = objContraSummary.BackColor
        objContraSummary.gridView.ColumnHeadersDefaultCellStyle = gridView_OWN.ColumnHeadersDefaultCellStyle
        objContraSummary.gridView.ColumnHeadersHeightSizeMode = gridView_OWN.ColumnHeadersHeightSizeMode
        objContraSummary.gridView.ColumnHeadersHeight = gridView_OWN.ColumnHeadersHeight
        objContraSummary.Text = gridView_OWN.CurrentRow.Cells("ACNAME").Value.ToString() & " [Contra Summary View]"
        objContraSummary.lblTitle.Text = lblTitle.Text + "[Contra Summary View]"
        objContraSummary.StartPosition = FormStartPosition.CenterScreen
        objContraSummary.Show()
        ContraSummaryGridViewFormat(objContraSummary.gridView)
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
        strSql += vbCrLf + " SELECT CONVERT(INT,NULL)TRANNO,CONVERT(SMALLDATETIME,NULL) TRANDATE,CONVERT(VARCHAR(40),NULL) PAYMODE,'OPENING...' as CONTRA,''SUBLEDGER"
        strSql += vbCrLf + " ,CASE WHEN SUM(DEBIT) - SUM(CREDIT) > 0 THEN SUM(DEBIT) - SUM(CREDIT) ELSE 0 END AS DEBIT"
        strSql += vbCrLf + " ,CASE WHEN SUM(CREDIT)- SUM(DEBIT) > 0 THEN SUM(CREDIT)- SUM(DEBIT) ELSE 0 END AS CREDIT"
        strSql += vbCrLf + " ,ISNULL(ACCODE,'') ACCODE"
        strSql += vbCrLf + " ,CONVERT(VARCHAR(11),NULL)BATCHNO,CONVERT(VARCHAR(40),NULL)REFNO,CONVERT(SMALLDATETIME,NULL) REFDATE,CONVERT(VARCHAR(55),NULL)REMARK1,CONVERT(VARCHAR(55),NULL)REMARK2"
        strSql += vbCrLf + " ,1 RESULT"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),NULL)RUNTOT,CONVERT(VARCHAR(1),NULL)CUR "
        strSql += vbCrLf + " ,NULL CHQCARDNO,NULL CARDID,NULL CHQCARDREF,CONVERT(VARCHAR(10),NULL) CHQDATE"
        strSql += vbCrLf + " ,NULL PCS,NULL GRSWT,NULL NETWT"
        strSql += vbCrLf + " ,CASE WHEN (SUM(ISNULL(RECEIPT,0))-SUM(ISNULL(ISSUE,0)))>0 THEN (SUM(ISNULL(RECEIPT,0))-SUM(ISNULL(ISSUE,0))) ELSE 0 END  RECEIPT"
        strSql += vbCrLf + " ,CASE WHEN (SUM(ISNULL(ISSUE,0))-SUM(ISNULL(RECEIPT,0)))>0 THEN  SUM(ISNULL(ISSUE,0))-SUM(ISNULL(RECEIPT,0)) ELSE 0 END ISSUE"
        strSql += vbCrLf + " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = ISNULL(X.ACCODE,''))AS ACNAME,CONVERT(VARCHAR(2),NULL)FROMFLAG,CONVERT(VARCHAR(3),NULL)COSTID,COMPANYID"
        strSql += vbCrLf + " ,CONVERT(VARCHAR(25),NULL)UPDATED,CONVERT(VARCHAR(50),NULL)USERNAME"
        strSql += vbCrLf + " FROM "
        strSql += vbCrLf + " 	("
        strSql += vbCrLf + " 	SELECT 'TRI_OPEN'SEP,' 'TRANNO,@FRMDATE TRANDATE"
        strSql += vbCrLf + " 	,(SELECT TOP 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = ISNULL(T.ACCODE,''))AS ACNAME"
        strSql += vbCrLf + "    ,'' SUBLEDGER,SUM(DEBIT)DEBIT,SUM(CREDIT)CREDIT"
        strSql += vbCrLf + " 	,ISNULL(ACCODE,'') ACCODE,NULL PCS,NULL GRSWT,NULL NETWT,NULL RECEIPT,NULL ISSUE,COSTID,COMPANYID"
        strSql += vbCrLf + " 	FROM " & cnStockDb & "..OPENTRAILBALANCE T"
        strSql += ftrTrailbal
        strSql += vbCrLf + " 	GROUP BY ISNULL(ACCODE,''),COSTID,COMPANYID"
        strSql += vbCrLf + " 	UNION ALL"
        strSql += vbCrLf + " 	SELECT 'ACC_OPEN'SEP,CONVERT(VARCHAR,TRANNO)TRANNO,TRANDATE"
        strSql += vbCrLf + " 	,(SELECT TOP 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = ISNULL(T.ACCODE,''))AS ACNAME"
        'strSql += vbCrLf + "  ,(SELECT TOP 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.SACCODE)SUBLEDGER"
        strSql += vbCrLf + "   ,'' SUBLEDGER"
        strSql += vbCrLf + " 	,ISNULL(SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT END),0) AS DEBIT"
        strSql += vbCrLf + " 	,ISNULL(SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT END),0) AS CREDIT"
        strSql += vbCrLf + " 	,ISNULL(ACCODE,'') ACCODE,PCS,GRSWT,NETWT,ISNULL(SUM(CASE WHEN TRANMODE = 'D' THEN GRSWT END),0) RECEIPT,ISNULL(SUM(CASE WHEN TRANMODE = 'C' THEN GRSWT END),0) ISSUE,COSTID,COMPANYID"
        strSql += vbCrLf + " 	FROM " & cnStockDb & "..ACCTRAN AS T"
        strSql += vbCrLf + " 	WHERE TRANDATE < @FRMDATE"
        strSql += ftrTran
        strSql += vbCrLf + " 	GROUP BY TRANNO,TRANDATE,ISNULL(ACCODE,''),PCS,GRSWT,NETWT,COSTID,COMPANYID"
        strSql += vbCrLf + " 	UNION ALL"
        strSql += vbCrLf + " 	SELECT 'TRI_OPEN'SEP,' 'TRANNO,@FRMDATE TRANDATE"
        strSql += vbCrLf + " 	,(SELECT TOP 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = ISNULL(T.ACCODE,''))AS ACNAME"
        strSql += vbCrLf + "    ,'' SUBLEDGER,0 DEBIT,0 CREDIT,ISNULL(ACCODE,'') ACCODE,NULL PCS,NULL GRSWT,NULL NETWT"
        strSql += vbCrLf + " 	,ISNULL(SUM(CASE WHEN TRANTYPE = 'R' THEN PUREWT END),0) RECEIPT"
        strSql += vbCrLf + " 	,ISNULL(SUM(CASE WHEN TRANTYPE = 'I' THEN PUREWT END),0) ISSUE"
        strSql += vbCrLf + " 	,COSTID,COMPANYID"
        strSql += vbCrLf + " 	FROM " & cnStockDb & "..OPENWEIGHT T"
        strSql += ftrTrailbal
        strSql += vbCrLf + " 	GROUP BY ISNULL(ACCODE,''),COSTID,COMPANYID"
        strSql += vbCrLf + " 	UNION ALL"
        strSql += vbCrLf + " 	SELECT 'REC_OPEN'SEP,CONVERT(VARCHAR,TRANNO)TRANNO,TRANDATE"
        strSql += vbCrLf + " 	,(SELECT TOP 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = ISNULL(T.ACCODE,''))AS ACNAME"
        strSql += vbCrLf + "   ,'' SUBLEDGER,0 AS DEBIT,0 AS CREDIT"
        strSql += vbCrLf + " 	,ISNULL(ACCODE,'') ACCODE,SUM(PCS) PCS,SUM(GRSWT) GRSWT,SUM(NETWT) NETWT,SUM(PUREWT) RECEIPT,NULL ISSUE,COSTID,COMPANYID"
        strSql += vbCrLf + " 	FROM " & cnStockDb & "..RECEIPT AS T"
        strSql += vbCrLf + " 	WHERE TRANDATE < @FRMDATE "
        If _WholeSaleFormat = False Then
            strSql += vbCrLf + " 	AND LEN(TRANTYPE) >2 "
        End If
        strSql += ftrTran
        If _WholeSaleFormat = False Then
            strSql += vbCrLf + " AND BATCHNO NOT IN (SELECT BATCHNO FROM " & cnStockDb & "..ACCTRAN WHERE TRANDATE BETWEEN @FRMDATE AND @TODATE"
            strSql += ftrTran
            strSql += vbCrLf + " )"
        End If
        strSql += vbCrLf + " 	GROUP BY TRANNO,TRANDATE,ISNULL(ACCODE,''),COSTID,COMPANYID"
        strSql += vbCrLf + " 	UNION ALL"
        strSql += vbCrLf + " 	SELECT 'ISS_OPEN'SEP,CONVERT(VARCHAR,TRANNO)TRANNO,TRANDATE"
        strSql += vbCrLf + " 	,(SELECT TOP 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = ISNULL(T.ACCODE,''))AS ACNAME"
        strSql += vbCrLf + "   ,'' SUBLEDGER,0 AS DEBIT,0 AS CREDIT"
        strSql += vbCrLf + " 	,ISNULL(ACCODE,'') ACCODE,SUM(PCS) PCS,SUM(GRSWT) GRSWT,SUM(NETWT) NETWT,NULL RECEIPT,SUM(PUREWT) ISSUE,COSTID,COMPANYID"
        strSql += vbCrLf + " 	FROM " & cnStockDb & "..ISSUE AS T"
        strSql += vbCrLf + " 	WHERE TRANDATE < @FRMDATE "
        If _WholeSaleFormat = False Then
            strSql += vbCrLf + " 	AND LEN(TRANTYPE) >2 "
        End If
        strSql += ftrTran
        If _WholeSaleFormat = False Then
            strSql += vbCrLf + " AND BATCHNO NOT IN (SELECT BATCHNO FROM " & cnStockDb & "..ACCTRAN WHERE TRANDATE BETWEEN @FRMDATE AND @TODATE"
            strSql += ftrTran
            strSql += vbCrLf + " )"
        End If
        strSql += vbCrLf + " 	GROUP BY TRANNO,TRANDATE,ISNULL(ACCODE,''),COSTID,COMPANYID"
        strSql += vbCrLf + " 	)X "
        strSql += vbCrLf + " GROUP BY ISNULL(ACCODE,''),COMPANYID"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT CONVERT(VARCHAR,TRANNO)TRANNO,TRANDATE,PAYMODE"
        strSql += vbCrLf + " ,(SELECT TOP 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.CONTRA)CONTRA"
        strSql += vbCrLf + " ,(SELECT TOP 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.SACCODE)SUBLEDGER"
        strSql += vbCrLf + " ,ISNULL(CASE WHEN TRANMODE = 'D' THEN AMOUNT END,0) AS DEBIT"
        strSql += vbCrLf + " ,ISNULL(CASE WHEN TRANMODE = 'C' THEN AMOUNT END,0) AS CREDIT"
        strSql += vbCrLf + " ,ACCODE,BATCHNO,REFNO,REFDATE,REMARK1,REMARK2,2 RESULT,0 RUNTOT,' 'CUR "
        strSql += vbCrLf + " ,CHQCARDNO,CARDID,CHQCARDREF,CONVERT(VARCHAR,CHQDATE,103)CHQDATE"
        strSql += vbCrLf + " ,PCS,GRSWT,NETWT,ISNULL(CASE WHEN TRANMODE = 'D' THEN GRSWT END,0) RECEIPT,ISNULL(CASE WHEN TRANMODE = 'C' THEN GRSWT END,0) ISSUE,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.ACCODE)AS ACNAME,FROMFLAG,COSTID,COMPANYID"
        strSql += vbCrLf + " ,CONVERT(VARCHAR(25),CONVERT(VARCHAR,UPDATED,103)+ ' ' + CONVERT(VARCHAR,UPTIME,108))UPDATED"
        strSql += vbCrLf + " ,'['+CONVERT(VARCHAR,USERID)+']'+ (SELECT USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERID = T.USERID)AS USERNAME"
        strSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN AS T"
        strSql += vbCrLf + " WHERE TRANDATE BETWEEN @FRMDATE AND @TODATE"
        strSql += ftrTran
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT CONVERT(VARCHAR,TRANNO)TRANNO,TRANDATE,TRANTYPE AS PAYMODE"
        strSql += vbCrLf + " ,'RECEIPT' CONTRA,'' SUBLEDGER"
        strSql += vbCrLf + " ,0 AS DEBIT,0 AS CREDIT,ACCODE,BATCHNO,REFNO,REFDATE,REMARK1,REMARK2,2 RESULT,0 RUNTOT,' 'CUR "
        strSql += vbCrLf + " ,NULL CHQCARDNO,NULL CARDID,NULL CHQCARDREF,CONVERT(VARCHAR,NULL,103)CHQDATE"
        strSql += vbCrLf + " ,PCS,GRSWT,NETWT,PUREWT RECEIPT,NULL ISSUE,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.ACCODE)AS ACNAME,''FROMFLAG,COSTID,COMPANYID"
        strSql += vbCrLf + " ,CONVERT(VARCHAR(25),CONVERT(VARCHAR,UPDATED,103)+ ' ' + CONVERT(VARCHAR,UPTIME,108))UPDATED"
        strSql += vbCrLf + " ,'['+CONVERT(VARCHAR,USERID)+']'+ (SELECT USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERID = T.USERID)AS USERNAME"
        strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT AS T"
        strSql += vbCrLf + " WHERE TRANDATE BETWEEN @FRMDATE AND @TODATE "
        If _WholeSaleFormat = False Then
            strSql += vbCrLf + " AND LEN(TRANTYPE) >2 "
        End If
        strSql += ftrTran
        If _WholeSaleFormat = False Then
            strSql += vbCrLf + " AND BATCHNO NOT IN (SELECT BATCHNO FROM " & cnStockDb & "..ACCTRAN WHERE TRANDATE BETWEEN @FRMDATE AND @TODATE"
            strSql += ftrTran
            strSql += vbCrLf + " )"
        End If
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT CONVERT(VARCHAR,TRANNO)TRANNO,TRANDATE,TRANTYPE AS PAYMODE"
        strSql += vbCrLf + " ,'ISSUE' CONTRA,'' SUBLEDGER"
        strSql += vbCrLf + " ,0 AS DEBIT,0 AS CREDIT,ACCODE,BATCHNO,REFNO,REFDATE,REMARK1,REMARK2,2 RESULT"
        strSql += vbCrLf + " ,0 RUNTOT,' 'CUR "
        strSql += vbCrLf + " ,NULL CHQCARDNO,NULL CARDID,NULL CHQCARDREF,CONVERT(VARCHAR,NULL,103)CHQDATE"
        strSql += vbCrLf + " ,PCS,GRSWT,NETWT,NULL RECEIPT,PUREWT ISSUE,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.ACCODE)AS ACNAME,''FROMFLAG,COSTID,COMPANYID"
        strSql += vbCrLf + " ,CONVERT(VARCHAR(25),CONVERT(VARCHAR,UPDATED,103)+ ' ' + CONVERT(VARCHAR,UPTIME,108))UPDATED"
        strSql += vbCrLf + " ,'['+CONVERT(VARCHAR,USERID)+']'+ (SELECT USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERID = T.USERID)AS USERNAME"
        strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE AS T"
        strSql += vbCrLf + " WHERE TRANDATE BETWEEN @FRMDATE AND @TODATE "
        If _WholeSaleFormat = False Then
            strSql += vbCrLf + " AND LEN(TRANTYPE) >2 "
        End If
        strSql += ftrTran
        If _WholeSaleFormat = False Then
            strSql += vbCrLf + " AND BATCHNO NOT IN (SELECT BATCHNO FROM " & cnStockDb & "..ACCTRAN WHERE TRANDATE BETWEEN @FRMDATE AND @TODATE"
            strSql += ftrTran
            strSql += vbCrLf + " )"
        End If
        strSql += vbCrLf + " )X"
        'strSql += vbCrLf + " ORDER BY ACCODE,TRANDATE,TRANNO"

        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = ""
        strSql += vbCrLf + " UPDATE T SET CONTRA = CONTRA "
        strSql += vbCrLf + " + CASE WHEN Z.GOLWT >0 THEN ' [GOL:' + CONVERT(VARCHAR(20),Z.GOLWT) +']' ELSE '' END "
        strSql += vbCrLf + " + CASE WHEN Z.SILWT >0 THEN ' [SIL:' + CONVERT(VARCHAR(20),Z.SILWT) +']' ELSE '' END "
        strSql += vbCrLf + " + CASE WHEN Z.PLAWT >0 THEN ' [PLA:' + CONVERT(VARCHAR(20),Z.PLAWT) +']'ELSE '' END "
        strSql += vbCrLf + " + CASE WHEN Z.OTHWT >0 THEN ' [OTH:' + CONVERT(VARCHAR(20),Z.OTHWT) +']'ELSE '' END "
        strSql += vbCrLf + " FROM ( "
        strSql += vbCrLf + " SELECT DISTINCT BATCHNO "
        strSql += vbCrLf + " ,SUM(GOLWT)GOLWT ,SUM(SILWT)SILWT ,SUM(PLAWT)PLAWT ,SUM(OTHWT)OTHWT FROM "
        strSql += vbCrLf + " ( "
        strSql += vbCrLf + " SELECT BATCHNO "
        strSql += vbCrLf + " ,CASE WHEN I.METALID='G' THEN GRSWT ELSE 0 END GOLWT "
        strSql += vbCrLf + " ,CASE WHEN I.METALID='S' THEN GRSWT ELSE 0 END SILWT "
        strSql += vbCrLf + " ,CASE WHEN I.METALID='P' THEN GRSWT ELSE 0 END PLAWT "
        strSql += vbCrLf + " ,CASE WHEN I.METALID NOT IN('G','S','P') THEN GRSWT ELSE 0 END OTHWT "
        strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I WHERE BATCHNO IN (SELECT BATCHNO FROM TEMP" & systemId & "ACC WHERE PAYMODE ='DU') "
        strSql += vbCrLf + " )Y GROUP BY BATCHNO ) Z , TEMP" & systemId & "ACC T WHERE Z.BATCHNO =T.BATCHNO AND  PAYMODE ='DU' "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        ProgressBarStep("Calculating Total", 10)
        strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ACCTOTAL')>0 DROP TABLE TEMP" & systemId & "ACCTOTAL"
        strSql += vbCrLf + "  SELECT 3 RESULT,ISNULL(ACCODE,'') ACCODE,'EXCLUDING OPENING'CONTRA,SUM(DEBIT)DEBIT,SUM(CREDIT)CREDIT,SUM(RECEIPT) RECEIPT,SUM(ISSUE) ISSUE"
        strSql += vbCrLf + " INTO TEMP" & systemId & "ACCTOTAL"
        strSql += vbCrLf + " FROM TEMP" & systemId & "ACC"
        strSql += vbCrLf + "  WHERE RESULT = 2"
        strSql += vbCrLf + "  GROUP BY ISNULL(ACCODE,'')"
        strSql += vbCrLf + "  UNION ALL"
        strSql += vbCrLf + "  SELECT 4 RESULT,ISNULL(ACCODE,'') ACCODE,'TOTAL'CONTRA,SUM(DEBIT)DEBIT,SUM(CREDIT)CREDIT,SUM(RECEIPT) RECEIPT,SUM(ISSUE) ISSUE FROM TEMP" & systemId & "ACC"
        strSql += vbCrLf + "  WHERE RESULT IN(1,2)"
        strSql += vbCrLf + "  GROUP BY ISNULL(ACCODE,'')"
        strSql += vbCrLf + "  UNION ALL"
        strSql += vbCrLf + "  SELECT 5 RESULT,ACCODE,'BALANCE'CONTRA"
        strSql += vbCrLf + "  ,CASE WHEN AMOUNT > 0 THEN AMOUNT ELSE NULL END AS DEBIT"
        strSql += vbCrLf + "  ,CASE WHEN AMOUNT < 0 THEN -1*AMOUNT ELSE NULL END AS CREDIT"
        strSql += vbCrLf + "  ,CASE WHEN WEIGHT > 0 THEN WEIGHT ELSE NULL END AS RECEIPT"
        strSql += vbCrLf + "  ,CASE WHEN WEIGHT < 0 THEN -1*WEIGHT ELSE NULL END AS ISSUE"
        strSql += vbCrLf + "  	FROM"
        strSql += vbCrLf + "  	("
        strSql += vbCrLf + "  	SELECT ISNULL(ACCODE,'') ACCODE,SUM(DEBIT)-SUM(CREDIT) AS AMOUNT,SUM(RECEIPT)-SUM(ISSUE) AS WEIGHT FROM TEMP" & systemId & "ACC"
        strSql += vbCrLf + "  	WHERE RESULT IN(1,2)"
        strSql += vbCrLf + "  	GROUP BY ISNULL(ACCODE,'')"
        strSql += vbCrLf + "  )X"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        ProgressBarStep("", 10)
        strSql = " SELECT * FROM TEMP" & systemId & "ACCTOTAL"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtViewTotal)
        ProgressBarStep("", 10)
        strSql = " INSERT INTO TEMP" & systemId & "ACC(RESULT,ACCODE,CONTRA,DEBIT,CREDIT,RECEIPT,ISSUE,ACNAME)"
        If chkMultiSelect.Checked Then ' If chkCmbAcName.Text = "ALL" Then
            strSql += vbCrLf + " SELECT DISTINCT 0 RESULT,ACCODE"
            strSql += vbCrLf + " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.ACCODE)"
            strSql += vbCrLf + " ,0,0,0,0"
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
        strSql += vbCrLf + " (TRANNO INT,TRANDATE SMALLDATETIME,PAYMODE VARCHAR (3),CONTRA VARCHAR (1000)"
        strSql += vbCrLf + " ,DEBIT NUMERIC (20,2),CREDIT NUMERIC (20,2),ACCODE VARCHAR (7)"
        strSql += vbCrLf + " ,BATCHNO VARCHAR (15),REFNO VARCHAR (40),REFDATE SMALLDATETIME,REMARK1 VARCHAR (200)"
        strSql += vbCrLf + " ,REMARK2 VARCHAR (200),RESULT INT,RUNTOTAL VARCHAR(20),RUNTOT NUMERIC (20,2),CUR VARCHAR (1)"
        strSql += vbCrLf + " ,CHQCARDNO VARCHAR(30),CARDID INT,CHQCARDREF VARCHAR(100),CHQDATE VARCHAR(10)"
        strSql += vbCrLf + " ,PCS INT,GRSWT NUMERIC(15,3),NETWT NUMERIC(15,3),RECEIPT NUMERIC(15,3),ISSUE NUMERIC(15,3)"
        strSql += vbCrLf + " ,RUNTOTALWT VARCHAR(20),RUNTOTWT NUMERIC (20,2),ACNAME VARCHAR(55),SUBLEDGER VARCHAR(55)"
        strSql += vbCrLf + " ,SNO INT IDENTITY(1,1),FROMFLAG VARCHAR(2),COSTID VARCHAR(3),UPDATED VARCHAR(25),COSTNAME VARCHAR(50),USERNAME VARCHAR(50),COMPANYID VARCHAR(50),COMPANYNAME VARCHAR(500))"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strSql = " INSERT INTO TEMP" & systemId & "ACCLEDGER"
        strSql += vbCrLf + " (TRANNO,TRANDATE,PAYMODE,CONTRA,DEBIT,CREDIT,ACCODE,BATCHNO,REFNO"
        strSql += vbCrLf + " ,REFDATE,REMARK1,REMARK2,RESULT,RUNTOT,CUR,CHQCARDNO,CARDID,CHQCARDREF"
        strSql += vbCrLf + " ,CHQDATE,PCS,GRSWT,NETWT,RECEIPT,ISSUE,ACNAME,FROMFLAG,COSTID,UPDATED,COSTNAME,USERNAME,COMPANYID"
        strSql += vbCrLf + " ,COMPANYNAME,SUBLEDGER)"
        strSql += vbCrLf + " SELECT"
        strSql += vbCrLf + " TRANNO,TRANDATE,PAYMODE,CONTRA,DEBIT,CREDIT,ACCODE,BATCHNO,REFNO"
        strSql += vbCrLf + " ,REFDATE,REMARK1,REMARK2,RESULT,RUNTOT,CUR,CHQCARDNO,CARDID,CHQCARDREF"
        strSql += vbCrLf + " ,CHQDATE,PCS,GRSWT,NETWT,RECEIPT,ISSUE,ACNAME,FROMFLAG,T.COSTID,T.UPDATED,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = T.COSTID)AS COSTNAME,USERNAME,T.COMPANYID,C.COMPANYNAME,SUBLEDGER"
        strSql += vbCrLf + " FROM TEMP" & systemId & "ACC T"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..COMPANY C ON T.COMPANYID=C.COMPANYID"
        strSql += vbCrLf + " WHERE ISNULL(C.ACTIVE,'')<>'N' "
        If chkOrderbyTranNo.Checked Then
            strSql += vbCrLf + " ORDER BY ACNAME,ACCODE,RESULT,TRANDATE,TRANNO,DEBIT DESC"
        Else
            strSql += vbCrLf + " ORDER BY ACNAME,ACCODE,RESULT,TRANDATE,DEBIT DESC,TRANNO"
        End If

        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
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
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
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
        strSql += vbCrLf + "  DECLARE @RECEIPT NUMERIC(20,2)"
        strSql += vbCrLf + "  DECLARE @ISSUE NUMERIC(20,2)"
        strSql += vbCrLf + "  DECLARE @RUNTOTWT NUMERIC(20,2)"
        strSql += vbCrLf + "  DECLARE RUNCUR CURSOR"
        strSql += vbCrLf + "  FOR SELECT DEBIT,CREDIT,RECEIPT,ISSUE,ACCODE FROM TEMP" & systemId & "ACCLEDGER WHERE RESULT IN (1,2)"
        strSql += vbCrLf + "  OPEN RUNCUR"
        strSql += vbCrLf + "  WHILE 1=1"
        strSql += vbCrLf + "  BEGIN"
        strSql += vbCrLf + "  	FETCH NEXT FROM RUNCUR INTO @DEBIT,@CREDIT,@RECEIPT,@ISSUE,@ACCODE"
        strSql += vbCrLf + "  	IF @@FETCH_STATUS = -1 BREAK"
        strSql += vbCrLf + "  	IF ISNULL(@ACCODE,'') <> ISNULL(@PREACCODE ,'')"
        strSql += vbCrLf + "  	BEGIN"
        strSql += vbCrLf + "  	SELECT @PREACCODE = @ACCODE"
        strSql += vbCrLf + "  	SELECT @RUNTOT = 0"
        strSql += vbCrLf + "  	SELECT @RUNTOTWT = 0"
        strSql += vbCrLf + "  	END"
        strSql += vbCrLf + "  	SELECT @RUNTOT = ISNULL(@RUNTOT,0) + ISNULL(@DEBIT,0) - ISNULL(@CREDIT,0)"
        strSql += vbCrLf + "  	SELECT @RUNTOTWT = ISNULL(@RUNTOTWT,0) + ISNULL(@RECEIPT,0) - ISNULL(@ISSUE,0)"
        strSql += vbCrLf + "  	PRINT @RUNTOT"
        strSql += vbCrLf + "  	UPDATE TEMP" & systemId & "ACCLEDGER SET "
        strSql += vbCrLf + "  			     RUNTOT = CASE WHEN @RUNTOT > 0 THEN @RUNTOT ELSE -1*@RUNTOT END,"
        strSql += vbCrLf + "  			     CUR    = CASE WHEN @RUNTOT > 0 THEN 'D' ELSE 'C' END"
        strSql += vbCrLf + "                 ,RUNTOTAL = CONVERT(VARCHAR,@RUNTOT) "
        If chkGrpTranNo.Checked = False Then strSql += vbCrLf + "  + CASE WHEN @RUNTOT > 0 THEN ' Dr' ELSE ' Cr' END"
        strSql += vbCrLf + "  			     ,RUNTOTWT = CASE WHEN @RUNTOTWT > 0 THEN @RUNTOTWT ELSE -1*@RUNTOTWT END"
        strSql += vbCrLf + "                 ,RUNTOTALWT = CONVERT(VARCHAR,@RUNTOTWT) "
        strSql += vbCrLf + "  			     WHERE CURRENT OF RUNCUR"
        strSql += vbCrLf + "  END"
        strSql += vbCrLf + "  CLOSE RUNCUR"
        strSql += vbCrLf + "  DEALLOCATE RUNCUR"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        ProgressBarStep("", 10)
    End Function


    Private Sub frmAccountsLedger_Weight_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If objSearch IsNot Nothing Then
            objSearch = Nothing
        End If
    End Sub

    Private Sub frmAccountsLedger_Weight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
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
                SendKeys.Send("{TAB}")
            End If



            If UCase(e.KeyChar) = Chr(Keys.L) Then
                If gridView_OWN.RowCount > 0 Then
                    If gridView_OWN.Visible = True Then
                        Dim result = MessageBox.Show("Do you want to Print Dotmatrix(Yes) or Crystal Report(No)...", "Message", MessageBoxButtons.YesNoCancel)
                        If result = Windows.Forms.DialogResult.Cancel Then
                        ElseIf result = Windows.Forms.DialogResult.No Then
                            CrystalPrint()
                        ElseIf result = Windows.Forms.DialogResult.Yes Then
                            LedgerDotmatrix()
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            MsgBox("ERROR: " + ex.Message + vbCrLf + vbCrLf + "POSITION: " + ex.StackTrace, MsgBoxStyle.Critical, "ERROR MESSAGE")
        End Try
    End Sub

    Private Sub frmAccountsLedger_Weight_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Me.WindowState = FormWindowState.Maximized
        grpFields.Location = New Point((ScreenWid - grpFields.Width) / 2, ((ScreenHit - 128) - grpFields.Height) / 2)
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

        Dim previous As Integer
        Dim page As Integer = 1
        Dim current As Integer
        Dim zeroPage As Integer

        funcSummaryGridStyle()
        gridView_OWN.RowTemplate.Height = 18
        gridSummary.BackgroundColor = Me.BackColor
        Try
            If ExCalling = False Then Prop_Sets()
            'If chkMultiSelection.Checked Then
            '    If Not lstAccName.Items.Count > 0 Then
            '        MsgBox("Select AccName", MsgBoxStyle.Information)
            '        cmbAccName.Focus()
            '        Exit Sub
            '    End If
            'Else
            '    If cmbAccName.Text = "ALL" Or cmbAccName.Text = "" Then
            '        MsgBox("Select AccName", MsgBoxStyle.Information)
            '        cmbAccName.Focus()
            '        Exit Sub
            '    End If
            'End If
            pnlHeading.Visible = False
            btnView_Search.Enabled = False
            'Me.Cursor = Cursors.WaitCursor
            ProgressBarShow()
            ProgressBarStep("", 10)
            funcAccLedger()
            ProgressBarStep("Fill into Datatable", 10)
            If chkGrpTranNo.Checked Then
                'strSql = " SELECT *"
                'strSql += " FROM TEMP" & systemId & "ACCLEDGER"
                'strSql += " ORDER BY SNO"

                'Dim dtView As New DataTable
                'da = New OleDbDataAdapter(strSql, cn)
                'da.Fill(dtView)
                'If dtView.Rows.Count > 0 Then
                '    dtView.Columns.Contains("RUNTOTAL").ToString()

                'End If
                strSql = " IF (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "LEDGER')>0 DROP TABLE TEMPTABLEDB..TEMP" & systemId & "LEDGER"
                strSql += " SELECT TRANNO,TRANDATE,PAYMODE,CONTRA,DEBIT,CREDIT,ACCODE,'' BATCHNO,REFNO,ISNULL(REFDATE,'')REFDATE,CONVERT(VARCHAR(50),RUNTOTAL)RUNTOTAL"
                strSql += " ,RUNTOT,PCS,GRSWT,NETWT,RECEIPT,ISSUE,CUR,CHQCARDNO,CARDID,ISNULL(CHQDATE,'')CHQDATE,CHQCARDREF,REMARK1,REMARK2,ACNAME,FROMFLAG,COSTID,'' UPDATED,COSTNAME,'' USERNAME"
                strSql += " ,COMPANYID,RESULT"
                strSql += " INTO TEMPTABLEDB..TEMP" & systemId & "LEDGER"
                strSql += " FROM ("
                strSql += " SELECT TRANNO,TRANDATE,ACNAME CONTRA,SUM(DEBIT)DEBIT,SUM(CREDIT)CREDIT,ACCODE,'' BATCHNO,REFNO,REFDATE,SUM(CONVERT(NUMERIC(18,2),RUNTOTAL)) AS  RUNTOTAL"
                If _WholeSaleFormat = False Then
                    strSql += ",PAYMODE,CHQCARDNO,CARDID,CHQCARDREF,FROMFLAG "
                Else
                    strSql += ",''PAYMODE,''CHQCARDNO,''CARDID,''CHQCARDREF,''FROMFLAG "
                End If
                strSql += " ,SUM(RUNTOT)RUNTOT,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(RECEIPT)RECEIPT,SUM(ISSUE)ISSUE,CUR,CHQDATE,REMARK1,REMARK2,ACNAME,COSTID,'' UPDATED,COSTNAME,'' USERNAME"
                strSql += " ,COMPANYID,RESULT FROM TEMP" & systemId & "ACCLEDGER"
                strSql += " GROUP BY TRANNO,TRANDATE,ACCODE,REFNO,REFDATE,CUR,CHQDATE,REMARK1,REMARK2,ACNAME,COSTID,COSTNAME"
                If _WholeSaleFormat = False Then
                    strSql += ",PAYMODE,CHQCARDNO,CARDID,CHQCARDREF,FROMFLAG "
                End If
                strSql += " ,COMPANYID,RESULT"
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
                If chkGrpTranNo.Checked = False Then
                    strSql += " ,RUNTOT,PCS,GRSWT,NETWT,RECEIPT,ISSUE,RUNTOTALWT,RUNTOTWT,CUR,CHQCARDNO,CARDID,CHQDATE,CHQCARDREF,REMARK1,REMARK2,ACNAME,FROMFLAG"
                Else
                    strSql += " ,RUNTOT,PCS,GRSWT,NETWT,RECEIPT,ISSUE,CUR,CHQCARDNO,CARDID,CHQDATE,CHQCARDREF,REMARK1,REMARK2,ACNAME,FROMFLAG"
                End If
                strSql += " ,COSTID,UPDATED,COSTNAME,USERNAME,COMPANYID,RESULT "
                strSql += " FROM TEMPTABLEDB..TEMP" & systemId & "LEDGER ORDER BY RESULT"
            Else
                strSql = " SELECT *"
                strSql += " FROM TEMP" & systemId & "ACCLEDGER"
                strSql += " ORDER BY SNO"
            End If
            dtGridView = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtGridView)
            If Not dtGridView.Rows.Count > 0 Then
                MsgBox("RECORD NOT FOUND")
                Exit Sub
            End If
            DataGridView2.Visible = False
            gridView_OWN.Visible = True
            gridSummary.Visible = True
            btnChqPrint.Enabled = True
            Button3.Enabled = True
            Button2.Enabled = True
            btnSumOfSelectedRows.Enabled = True
            btnFindSearch.Enabled = True
            btnAccountInfo.Enabled = True
            btnContraSummary.Enabled = True
            If ChkSummaryBalance.Checked = True Then
                btnChqPrint.Enabled = False                
                Button3.Enabled = False               
                Button2.Enabled = False
                btnSumOfSelectedRows.Enabled = False
                btnFindSearch.Enabled = False
                btnAccountInfo.Enabled = False
                btnContraSummary.Enabled = False
                strSql = " SELECT 'LEDGER SUMMARY'CONTRA,NULL DEBIT,NULL CREDIT,NULL RECEIPT,NULL ISSUE,0 RESULT"
                strSql += " UNION ALL"
                strSql += " SELECT ACNAME CONTRA"
                strSql += " ,DEBIT,CREDIT,RECEIPT,ISSUE,2 RESULT FROM TEMP" & systemId & "ACCLEDGER T"
                strSql += " WHERE CONTRA='BALANCE'"
                strSql += "    UNION ALL "
                strSql += " SELECT 'TOTAL' CONTRA"
                strSql += " ,SUM(DEBIT)DEBIT,SUM(CREDIT)CREDIT,SUM(RECEIPT)RECEIPT,SUM(ISSUE)ISSUE,4 RESULT FROM TEMP" & systemId & "ACCLEDGER T"
                strSql += " WHERE CONTRA='BALANCE'  ORDER BY RESULT,CONTRA   "
                ProgressBarStep("Fill into Grid", 10)
                DataGridView2.Visible = True
                gridView_OWN.Visible = False
                gridSummary.Visible = False
                dt = New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dt)
                If Not dt.Rows.Count > 0 Then
                    MsgBox("RECORD NOT FOUND")
                    Exit Sub
                End If
                DataGridView2.DataSource = dt               
                GridViewFormat1()
                funcGridViewStyle1()
                DataGridView2.Size = gridView_OWN.Size
                DataGridView2.Location = gridView_OWN.Location
                DataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                DataGridView2.Invalidate()
                For Each dgvCol As DataGridViewColumn In DataGridView2.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                DataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
                DataGridView2.Focus()
                DataGridView2.Select()
                tabView.Show()
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
                title += cmbAcName.Text & " LEDGER"
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
            gridView_OWN.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
            gridView_OWN.Invalidate()
            For Each dgvCol As DataGridViewColumn In gridView_OWN.Columns
                dgvCol.Width = dgvCol.Width
            Next
            gridView_OWN.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            gridView_OWN.Focus()
            tabView.Show()
        Catch ex As Exception
            MsgBox("ERROR: " + ex.Message + vbCrLf + vbCrLf + "POSITION: " + ex.StackTrace, MsgBoxStyle.Critical, "ERROR MESSAGE")
        Finally
            ProgressBarHide()
            btnView_Search.Enabled = True
            Me.Cursor = Cursors.Default
        End Try
    End Sub
    Function ledgerprint() As Integer
        Dim i As Integer
        Dim dt2 As New DataTable
        Dim dtTemp As New DataTable
        Dim da As New OleDbDataAdapter
        Dim cmd As New OleDbCommand
        strSql = " SELECT *"
        strSql += " FROM TEMP" & systemId & "ACCLEDGER"
        strSql += " ORDER BY SNO"
        'strsql += " where SNO"
        dtGridView = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt2)
        'dt = ugridView.DataSource
        'dtTemp = dt2.Copy
        If dt2.Rows.Count > 0 Then
            For i = 0 To dt2.Rows.Count - 1



                'dtTemp.Columns.Remove("ACCOUNTID")
                'If MsgBox("Do You Want to Print Narration", MsgBoxStyle.YesNo) = MsgBoxResult.No Then dtTemp.Columns.Remove("Narration")
                'dtTemp.Columns.Remove("NARRATION")
                'dtTemp.Columns("ACCNAME").MaxLength= 27
                dt2.Columns("TRANNO").Caption = "Tranno"
                dt2.Columns("TRANDATE").Caption = "Trandate"
                dt2.Columns("PAYMODE").Caption = "Paymode"
                dt2.Columns("CONTRA").Caption = "Contra"
                dt2.Columns("DEBIT").Caption = "Debit"
                dt2.Columns("CREDIT").Caption = "Credit"
                DataGridView1.DataSource = dt2
                lblTitle.Font = New Font("VERDANA", 9, FontStyle.Bold)
                lblTitle.Text = "Ledger Report"
                DataGridView1.Visible = True

                'dtTemp.Columns("DOCDATE").Caption = "Vou. Date"
                'dtTemp.Columns("DOCTYPE").Caption = "Vou. Type"
                'ADMPL.PrintDataGridView.Print_View(dtTemp, Nothing, title)
            Next

        End If



        'ADMPL.PrintDataGridView.Print_View(dtTemp, Nothing, title)
        'BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, dtTemp, BrightPosting.GExport.GExportType.Print)
    End Function

    '========================================================================
    '16.09.08 modified

    Function ContraSummaryGridViewFormat(ByVal grid As DataGridView) As Integer
        For Each dgvRow As DataGridViewRow In grid.Rows
            If dgvRow.Cells("CONTRA").Value.ToString.Contains(Chr(13)) Then
                dgvRow.DefaultCellStyle.WrapMode = DataGridViewTriState.True
                grid.AutoResizeRow(dgvRow.Index, DataGridViewAutoSizeRowMode.AllCells)
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
            Select Case dgvRow.Cells("RESULT").Value.ToString
                Case "0" 'TITLE
                    dgvRow.DefaultCellStyle.BackColor = Color.LightBlue
                    dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    dgvRow.DefaultCellStyle.ForeColor = Color.Red
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
    End Function

    Private Sub gridView_OWN_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView_OWN.DataSourceChanged

    End Sub


    Private Sub gridView_OWN_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView_OWN.KeyDown
        If Not gridView_OWN.RowCount > 0 Then Exit Sub
        If gridView_OWN.CurrentRow Is Nothing Then Exit Sub
        If e.KeyCode = Keys.D Then
            If e.Control = False Then
                funcBillViewDetails(gridView_OWN.CurrentRow.Index)
            Else
                If btnDuplicatePrint.Enabled = False Then Exit Sub
                btnDuplicatePrint_Click(Me, New EventArgs)
            End If

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
                btnEdit_Click(Me, New EventArgs)
            End If
            If e.KeyChar = "X" Or e.KeyChar = "x" Then
                Me.btnExcel_Click(Me, New EventArgs)
            ElseIf UCase(e.KeyChar) = "P" Then
                Me.btnPrint_Click(Me, New EventArgs)
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

    '========================================================================

    'Private Sub gridBillView_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles gridBillView.CellFormatting
    '    Try
    '        Select Case e.ColumnIndex
    '            Case 3
    '                If Val(e.Value) = 0 Then
    '                    e.Value = ""
    '                End If
    '                If Val(e.Value) <> 0 Then
    '                    e.CellStyle.BackColor = Color.Lavender
    '                End If
    '            Case 4
    '                If Val(e.Value) = 0 Then
    '                    e.Value = ""
    '                End If
    '                If Val(e.Value) <> 0 Then
    '                    e.CellStyle.BackColor = Color.LavenderBlush
    '                End If
    '        End Select
    '    Catch ex As Exception
    '        MsgBox("ERROR: " + ex.Message + vbCrLf + vbCrLf + "POSITION: " + ex.StackTrace, MsgBoxStyle.Critical, "ERROR MESSAGE")
    '    End Try
    'End Sub
    '========================================================================
    '16.09.08 modified

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
            chkCmbAcGroup.Focus()
        Catch ex As Exception
            MsgBox("ERROR: " + ex.Message + vbCrLf + vbCrLf + "POSITION: " + ex.StackTrace, MsgBoxStyle.Critical, "ERROR MESSAGE")
        End Try
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub



    Private Sub cmbVoucherType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        e.KeyChar = UCase(e.KeyChar)
    End Sub
    '=================================================================================================
    '17.09.08
    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        'Me.Cursor = Cursors.WaitCursor
        If ChkSummaryBalance.Checked = True Then
            If DataGridView2.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
                '593
                lblTitle.Text = Replace(lblTitle.Text, vbCrLf, " ")
                BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, DataGridView2, BrightPosting.GExport.GExportType.Export)
                '593
            End If
        Else
            If gridView_OWN.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
                '593
                lblTitle.Text = Replace(lblTitle.Text, vbCrLf, " ")
                BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView_OWN, BrightPosting.GExport.GExportType.Export)
                '593
            End If
        End If

        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub

        If ChkSummaryBalance.Checked = True Then
            If DataGridView2.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
                BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, DataGridView2, BrightPosting.GExport.GExportType.Print)
            End If
        Else
            If gridView_OWN.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
                BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView_OWN, BrightPosting.GExport.GExportType.Print)
            End If
        End If
    End Sub
    '=================================================================================================

    Private Sub btnFindSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindSearch.Click
        FindSearchToolStripMenuItem_Click(Me, New EventArgs)
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        frmAccountsLedger_Weight_KeyPress(Me, New KeyPressEventArgs(ChrW(Keys.Escape)))
    End Sub

    Private Sub btnSumOfSelectedRows_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSumOfSelectedRows.Click
        Dim STR As String = Nothing
        Dim debit As Double = Nothing
        Dim credit As Double = Nothing
        If Not gridView_OWN.SelectedRows.Count > 0 Then
            Exit Sub
        End If
        For CNT As Integer = 0 To gridView_OWN.SelectedRows.Count - 1
            With gridView_OWN.SelectedRows(CNT)
                debit += Val(.Cells("DEBIT").FormattedValue.ToString.Replace(",", ""))
                credit += Val(.Cells("CREDIT").FormattedValue.ToString.Replace(",", ""))
            End With
        Next
        If debit - credit > 0 Then
            debit = debit - credit
            credit = 0
        Else
            credit = credit - debit
            debit = 0
        End If
        txtSumDebit.Text = Format(debit, "0.00")
        txtSumCredit.Text = Format(credit, "0.00")
        grpSumOfSelectedRows.BackColor = System.Drawing.SystemColors.InactiveCaptionText
        txtSumDebit.BackColor = System.Drawing.SystemColors.Window
        txtSumCredit.BackColor = System.Drawing.SystemColors.Window
        txtSumDebit.ForeColor = System.Drawing.SystemColors.WindowText
        txtSumCredit.ForeColor = System.Drawing.SystemColors.WindowText
        grpSumOfSelectedRows.Visible = True
        grpSumOfSelectedRows.Size = New System.Drawing.Size(353, 50)
        grpSumOfSelectedRows.Location = New System.Drawing.Point(288, 127)
    End Sub

    Private Sub FindSearchToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FindSearchToolStripMenuItem.Click
        If Not gridView_OWN.RowCount > 0 Then
            Exit Sub
        End If
        If Not gridView_OWN.RowCount > 0 Then Exit Sub
        objSearch.Show()
        'Dim objSearch As New frmGridSearch(gridView_OWN)
        'objSearch.ShowDialog()
    End Sub

    Private Sub SumOfSelectedRowsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SumOfSelectedRowsToolStripMenuItem.Click
        btnSumOfSelectedRows_Click(Me, New EventArgs)
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Try
            'cmbAccGroup.Items.Add("ALL")
            'strSql = " SELECT ACGRPNAME FROM " & cnAdminDb & "..ACGROUP ORDER BY ACGRPNAME"
            'objGPack.FillCombo(strSql, cmbAccGroup, False, False)

            funcGridStyle(gridSummary)
            tabMain.ItemSize = New System.Drawing.Size(1, 1)
            If _IsCostCentre Then
                'cmbAccCostCentre.Items.Clear()
                'cmbAccCostCentre.Items.Add("ALL")
                'strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
                'objGPack.FillCombo(strSql, cmbAccCostCentre, False, False)
                'cmbAccCostCentre.Enabled = True
                'lstAccCostCentre.Enabled = True
                ''COSTCENTRE
                strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
                strSql += " UNION ALL"
                strSql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
                strSql += " WHERE  ISNULL(COMPANYID,'" & strCompanyId & "')LIKE'%" & strCompanyId & "%' ORDER BY RESULT,COSTNAME"
                dtCostCentre = New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtCostCentre)
                BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , "ALL")



                chkCmbCostCentre.Enabled = True
            Else
                'cmbAccCostCentre.Enabled = False
                'lstAccCostCentre.Enabled = False
                chkCmbCostCentre.Enabled = False
            End If
            strSql = " SELECT 'ALL' COMPANYNAME,'ALL' COMPANYID,1 RESULT"
            strSql += " UNION ALL"
            strSql += " SELECT COMPANYNAME,CONVERT(vARCHAR,COMPANYID),2 RESULT FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N'"
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

    'Private Sub gridView_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles gridView.CellFormatting
    '    'Select Case Val(gridView.Rows(e.RowIndex).Cells("DEBIT").Value.ToString)
    '    '    Case Is <> 0
    '    '        gridView.Rows(e.RowIndex).Cells("DEBIT").Style.BackColor = Color.Lavender
    '    '    Case Is = 0
    '    '        gridView.Rows(e.RowIndex).Cells("DEBIT").Value = DBNull.Value
    '    'End Select

    '    Select Case Val(gridView.Rows(e.RowIndex).Cells("CREDIT").Value.ToString)
    '        Case Is <> 0
    '            gridView.Rows(e.RowIndex).Cells("CREDIT").Style.BackColor = Color.LavenderBlush
    '        Case Is = 0
    '            gridView.Rows(e.RowIndex).Cells("CREDIT").Value = DBNull.Value
    '    End Select

    '    'Select Case gridView.Rows(e.RowIndex).Cells("RESULT").Value.ToString
    '    '    Case Is > 2
    '    '        gridView.Rows(e.RowIndex).DefaultCellStyle.Font = reportSubTotalStyle.Font
    '    'End Select
    'End Sub

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

    'Private Function GetSelectedItems(ByVal lst As ListBox, Optional ByVal concatStr As String = "'") As String
    '    Dim ret As String = ""
    '    If Not chkMultiSelection.Checked Then
    '        Select Case lst.Name
    '            Case lstAccGroup.Name
    '                If cmbAccGroup.Text = "ALL" Or cmbAccGroup.Text = "" Then Return Nothing
    '                ret = "'" & cmbAccGroup.Text & "'"
    '            Case lstAccName.Name
    '                If cmbAccName.Text = "ALL" Or cmbAccName.Text = "" Then Return Nothing
    '                ret = "'" & cmbAccName.Text & "'"
    '            Case lstAccCostCentre.Name
    '                If Not cmbAccCostCentre.Enabled Then Return Nothing
    '                If cmbAccCostCentre.Text = "ALL" Or cmbAccCostCentre.Text = "" Then Return Nothing
    '                ret = "'" & cmbAccCostCentre.Text & "'"
    '        End Select
    '    Else
    '        For cnt As Integer = 0 To lst.Items.Count - 1
    '            If lst.Items.Item(cnt).ToString = "ALL" Then Return Nothing
    '            ret += "'" & lst.Items.Item(cnt).ToString & "'"
    '            If cnt <> lst.Items.Count - 1 Then ret += ","
    '        Next
    '    End If
    '    If concatStr <> "'" Then
    '        ret = ret.ToString.Replace("'", concatStr)
    '    End If
    '    Return ret
    'End Function

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

    'Private Sub chkMultiSelection_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    lstAccGroup.Visible = chkMultiSelection.Checked
    '    lstAccName.Visible = chkMultiSelection.Checked
    '    lstAccCostCentre.Visible = chkMultiSelection.Checked
    'End Sub

    'Private Sub cmbAccGroup_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
    '    cmbAccGroup.Size = New Size(cmbAccGroup.Width, 100)
    '    cmbAccGroup.BringToFront()
    'End Sub

    'Private Sub cmbAccGroup_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
    '    If e.KeyCode = Keys.Insert And chkMultiSelection.Checked Then
    '        If Not lstAccGroup.Items.Contains(cmbAccGroup.Text) And cmbAccGroup.Items.Contains(cmbAccGroup.Text) Then
    '            lstAccGroup.Items.Add(cmbAccGroup.Text)
    '        End If
    '    End If
    'End Sub

    'Private Sub cmbAccGroup_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
    '    cmbAccGroup_SelectedIndexChanged(Me, New EventArgs)
    '    cmbAccGroup.Size = New Size(cmbAccGroup.Width, 21)
    '    If chkMultiSelection.Checked Then cmbAccGroup.Text = ""
    'End Sub

    'Private Sub cmbAccGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    'cmbAccName.Text = ""
    '    'cmbAccName.Items.Clear()
    '    'lstAccName.Items.Clear()
    '    'If chkMultiSelection.Checked Then cmbAccName.Items.Add("ALL")
    '    'strSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD "
    '    'If GetSelectedItems(lstAccGroup) <> Nothing Then
    '    '    strSql += " WHERE ACGRPCODE IN (SELECT ACGRPCODE FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPNAME IN (" & GetSelectedItems(lstAccGroup) & "))"
    '    'End If
    '    'strSql += " ORDER BY ACNAME"
    '    'objGPack.FillCombo(strSql, cmbAccName, False, False)
    'End Sub

    'Private Sub cmbAccName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
    '    'cmbAccName.Size = New Size(cmbAccName.Width, 100)
    '    'cmbAccName.BringToFront()
    'End Sub

    'Private Sub cmbAccName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
    '    'If e.KeyCode = Keys.Insert And chkMultiSelection.Checked Then
    '    '    If Not lstAccName.Items.Contains(cmbAccName.Text) And cmbAccName.Items.Contains(cmbAccName.Text) Then
    '    '        lstAccName.Items.Add(cmbAccName.Text)
    '    '    End If
    '    'End If
    'End Sub

    'Private Sub cmbAccName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
    '    'cmbAccName.Size = New Size(cmbAccName.Width, 21)
    '    'If chkMultiSelection.Checked Then cmbAccName.Text = ""
    'End Sub

    'Private Sub cmbAccCostCentre_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
    '    'cmbAccCostCentre.Size = New Size(cmbAccCostCentre.Width, 100)
    '    'cmbAccCostCentre.BringToFront()
    'End Sub

    'Private Sub cmbAccCostCentre_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
    '    cmbAccCostCentre.Size = New Size(cmbAccCostCentre.Width, 21)
    '    If chkMultiSelection.Checked Then cmbAccCostCentre.Text = ""
    'End Sub

    'Private Sub lstAccName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
    '    If e.KeyChar = Chr(Keys.Enter) And Not lstAccName.Items.Count > 0 Then
    '        cmbAccName.Focus()
    '    Else
    '        SendKeys.Send("{TAB}")
    '    End If
    'End Sub

    'Private Sub lstAcc_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)
    '    If e.KeyCode = Keys.Delete Then
    '        Dim lst As ListBox
    '        lst = CType(sender, ListBox)
    '        If lst.SelectedIndex > -1 Then
    '            lst.Items.RemoveAt(lst.SelectedIndex)
    '        End If
    '        If Not lst.Items.Count > 0 Then Me.SelectNextControl(lst, False, True, True, True)
    '    End If
    'End Sub

    'Private Sub cmbTip_Gotfocus(ByVal sender As Object, ByVal e As EventArgs) Handles _
    'cmbAccGroup.GotFocus, cmbAccName.GotFocus, cmbAccCostCentre.GotFocus
    '    If Not chkMultiSelection.Checked Then Exit Sub
    '    lblTip.Visible = True
    '    lblTip.Location = New Point(lblTip.Location.X, CType(sender, Control).Location.Y)
    '    lblTip.Text = "Add -> Insert Key"
    'End Sub

    'Private Sub lstTip_Gotfocus(ByVal sender As Object, ByVal e As EventArgs)
    '    If Not chkMultiSelection.Checked Then Exit Sub
    '    lblTip.Visible = True
    '    lblTip.Location = New Point(lblTip.Location.X, CType(sender, Control).Location.Y)
    '    lblTip.Text = "Remove -> Del Key"
    'End Sub

    'Private Sub Tip_Lostfocus(ByVal sender As Object, ByVal e As EventArgs) Handles _
    'cmbAccGroup.LostFocus, cmbAccName.LostFocus, cmbAccCostCentre.LostFocus
    '    lblTip.Text = ""
    '    lblTip.Visible = False
    'End Sub

    'Private Sub cmbAccCostCentre_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
    '    If e.KeyCode = Keys.Insert And chkMultiSelection.Checked Then
    '        If Not lstAccCostCentre.Items.Contains(cmbAccCostCentre.Text) And cmbAccCostCentre.Items.Contains(cmbAccCostCentre.Text) Then
    '            lstAccCostCentre.Items.Add(cmbAccCostCentre.Text)
    '        End If
    '    End If
    'End Sub

    Private Sub gridView_OWN_RowEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView_OWN.RowEnter
        Try
            If gridView_OWN.Rows(e.RowIndex).Cells("FROMFLAG").Value.ToString = "A" Then
                btnCancel.Enabled = True
            Else
                btnCancel.Enabled = False
            End If
            btnEdit.Enabled = False
            btnDuplicatePrint.Enabled = False
            If gridView_OWN.RowCount > 3 Then
                funcSummaryDetails(e.RowIndex)
                If AccEntry.Contains(gridView_OWN.Rows(e.RowIndex).Cells("PAYMODE").Value.ToString) Then
                    btnEdit.Enabled = True
                    btnDuplicatePrint.Enabled = True
                End If
            End If
            'funcGridSummaryFormating()
        Catch ex As Exception
            MsgBox("ERROR: " + ex.Message + vbCrLf + vbCrLf + "POSITION: " + ex.StackTrace, MsgBoxStyle.Critical, "ERROR MESSAGE")
        End Try
    End Sub

    'Private Sub cmbAccName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
    '    If e.KeyChar = Chr(Keys.Enter) And Not chkMultiSelection.Checked Then
    '        If cmbAccName.Text = "" Then
    '            MsgBox("AccName should not empty", MsgBoxStyle.Information)
    '            cmbAccName.Focus()
    '        End If
    '    End If
    'End Sub

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
            If Val(dgvRow.Cells("RECEIPT").Value.ToString) = 0 Then
                dgvRow.Cells("RECEIPT").Value = DBNull.Value
                dgvRow.Cells("RECEIPT").Style.BackColor = dgvRow.Cells("CONTRA").Style.BackColor
            Else
                dgvRow.Cells("RECEIPT").Style.BackColor = Color.Lavender
            End If
            If Val(dgvRow.Cells("ISSUE").Value.ToString) = 0 Then
                dgvRow.Cells("ISSUE").Value = DBNull.Value
                dgvRow.Cells("ISSUE").Style.BackColor = dgvRow.Cells("CONTRA").Style.BackColor
            Else
                dgvRow.Cells("ISSUE").Style.BackColor = Color.LavenderBlush
            End If
            'If dgvRow.Cells("COMPANYID").Value.ToString = "" Then
            '    'dgvRow.Cells("COMPANYID").Value = DBNull.Value
            '    dgvRow.Cells("COMPANYID").Style.BackColor = dgvRow.Cells("CONTRA").Style.BackColor
            'Else
            '    dgvRow.Cells("COMPANYID").Style.BackColor = Color.Lavender
            'End If


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
    Private Sub GridViewFormat1()
        For Each dgvRow1 As DataGridViewRow In DataGridView2.Rows
            If dgvRow1.Cells("CONTRA").Value.ToString.Contains(Chr(13)) Then
                dgvRow1.DefaultCellStyle.WrapMode = DataGridViewTriState.True
                DataGridView2.AutoResizeRow(dgvRow1.Index, DataGridViewAutoSizeRowMode.AllCells)
            End If
            If Val(dgvRow1.Cells("DEBIT").Value.ToString) = 0 Then
                dgvRow1.Cells("DEBIT").Value = DBNull.Value
                dgvRow1.Cells("DEBIT").Style.BackColor = dgvRow1.Cells("CONTRA").Style.BackColor
            Else
                dgvRow1.Cells("DEBIT").Style.BackColor = Color.Lavender
            End If
            If Val(dgvRow1.Cells("CREDIT").Value.ToString) = 0 Then
                dgvRow1.Cells("CREDIT").Value = DBNull.Value
                dgvRow1.Cells("CREDIT").Style.BackColor = dgvRow1.Cells("CONTRA").Style.BackColor
            Else
                dgvRow1.Cells("CREDIT").Style.BackColor = Color.LavenderBlush
            End If

            If Val(dgvRow1.Cells("RECEIPT").Value.ToString) = 0 Then
                dgvRow1.Cells("RECEIPT").Value = DBNull.Value
                dgvRow1.Cells("RECEIPT").Style.BackColor = dgvRow1.Cells("CONTRA").Style.BackColor
            Else
                dgvRow1.Cells("RECEIPT").Style.BackColor = Color.Lavender
            End If
            If Val(dgvRow1.Cells("ISSUE").Value.ToString) = 0 Then
                dgvRow1.Cells("ISSUE").Value = DBNull.Value
                dgvRow1.Cells("ISSUE").Style.BackColor = dgvRow1.Cells("CONTRA").Style.BackColor
            Else
                dgvRow1.Cells("ISSUE").Style.BackColor = Color.LavenderBlush
            End If



            Select Case dgvRow1.Cells("RESULT").Value.ToString
                Case "0" 'TITLE
                    dgvRow1.DefaultCellStyle.BackColor = Color.LightBlue
                    dgvRow1.DefaultCellStyle.ForeColor = Color.Red
                    dgvRow1.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                Case "1" 'OPENING
                    dgvRow1.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                Case "2" 'TRANS
                Case "3" 'EXCLUD OPEN
                    dgvRow1.DefaultCellStyle.BackColor = Color.LightGoldenrodYellow
                    dgvRow1.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                Case "4" 'TOTAL
                    dgvRow1.DefaultCellStyle.BackColor = Color.LightGoldenrodYellow
                    dgvRow1.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                Case "5" 'BALANCE
                    dgvRow1.DefaultCellStyle.BackColor = Color.LightGoldenrodYellow
                    dgvRow1.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
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
                        If Val(e.Value.ToString) = 0 Then
                            'gridSummary.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = ""
                        End If
                        If Val(e.Value.ToString) <> 0 Then
                            'gridSummary.Rows(e.RowIndex).Cells(e.ColumnIndex).Style.BackColor = Color.Lavender
                            e.CellStyle.BackColor = Color.Lavender
                        End If
                    End If
                Case 6
                    If e.RowIndex < 3 Then
                        If Val(e.Value.ToString) = 0 Then
                            'gridSummary.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = ""
                        End If
                        If Val(e.Value.ToString) <> 0 Then
                            'gridSummary.Rows(e.RowIndex).Cells(e.ColumnIndex).Style.BackColor = Color.Lavender
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

    Private Sub btnEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEdit.Click
        If Not gridView_OWN.RowCount > 0 Then Exit Sub
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Edit) Then Exit Sub
        strSql = vbCrLf + " SELECT COUNT(*) AS CNT FROM " & cnStockDb & "..ACCTRAN AS T"
        strSql += vbCrLf + " INNER JOIN " & cnStockDb & "..BRSINFO AS B"
        strSql += vbCrLf + " ON T.TRANDATE = B.TRANDATE AND T.BATCHNO = B.BATCHNO"
        strSql += vbCrLf + " AND T.ACCODE = B.ACCODE"
        strSql += vbCrLf + " AND T.CHQCARDNO = B.CHQCARDNO"
        strSql += vbCrLf + " AND T.CARDID = B.CARDID"
        strSql += vbCrLf + " AND T.CHQCARDREF = B.CHQCARDREF"
        strSql += vbCrLf + " AND ISNULL(T.CHQDATE,'') = ISNULL(B.CHQDATE,'')"
        strSql += vbCrLf + " AND T.COSTID = B.COSTID"
        strSql += vbCrLf + " AND T.COMPANYID = B.COMPANYID"
        strSql += vbCrLf + " AND T.AMOUNT = B.AMOUNT"
        strSql += vbCrLf + " WHERE T.BATCHNO = '" & gridView_OWN.CurrentRow.Cells("BATCHNO").Value.ToString & "'"
        strSql += vbCrLf + " AND T.TRANDATE = '" & Format(gridView_OWN.CurrentRow.Cells("TRANDATE").Value, "yyyy-MM-dd") & "'"
        Dim dtChequeCheck As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtChequeCheck)
        If dtChequeCheck.Rows.Count > 0 Then
            If Val(dtChequeCheck.Rows(0).Item(0).ToString) > 0 Then
                MsgBox("It cannot be edit.This Entry Cheque Reconciled", MsgBoxStyle.Information)
                Exit Sub
            End If
        End If
        funcAccountsEntry(gridView_OWN.CurrentRow.Index)
        gridView_OWN.Select()
    End Sub

    Private Sub btnContraSummary_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnContraSummary.Click
        funcContraWiseSummury()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Cancel) Then Exit Sub
        If gridView_OWN.CurrentRow Is Nothing Then Exit Sub
        If gridView_OWN.CurrentRow.DefaultCellStyle.BackColor = Color.Red Then
            MsgBox("Already Cancelled", MsgBoxStyle.Information)
            Exit Sub
        End If
        strSql = vbCrLf + " SELECT COUNT(*) AS CNT FROM " & cnStockDb & "..ACCTRAN AS T"
        strSql += vbCrLf + " INNER JOIN " & cnStockDb & "..BRSINFO AS B"
        strSql += vbCrLf + " ON T.TRANDATE = B.TRANDATE AND T.BATCHNO = B.BATCHNO"
        strSql += vbCrLf + " AND T.ACCODE = B.ACCODE"
        strSql += vbCrLf + " AND T.CHQCARDNO = B.CHQCARDNO"
        strSql += vbCrLf + " AND T.CARDID = B.CARDID"
        strSql += vbCrLf + " AND T.CHQCARDREF = B.CHQCARDREF"
        strSql += vbCrLf + " AND ISNULL(T.CHQDATE,'') = ISNULL(B.CHQDATE,'')"
        strSql += vbCrLf + " AND T.COSTID = B.COSTID"
        strSql += vbCrLf + " AND T.COMPANYID = B.COMPANYID"
        strSql += vbCrLf + " AND T.AMOUNT = B.AMOUNT"
        strSql += vbCrLf + " WHERE T.BATCHNO = '" & gridView_OWN.CurrentRow.Cells("BATCHNO").Value.ToString & "'"
        strSql += vbCrLf + " AND T.TRANDATE = '" & Format(gridView_OWN.CurrentRow.Cells("TRANDATE").Value, "yyyy-MM-dd") & "'"
        Dim dtChequeCheck As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtChequeCheck)
        If dtChequeCheck.Rows.Count > 0 Then
            If Val(dtChequeCheck.Rows(0).Item(0).ToString) > 0 Then
                MsgBox("It cannot be cancel.This Entry Cheque Reconciled", MsgBoxStyle.Information)
                Exit Sub
            End If
        End If

        Dim objSecret As New frmAdminPassword()
        If objSecret.ShowDialog <> Windows.Forms.DialogResult.OK Then
            Exit Sub
        End If
        Dim objRemark As New frmBillRemark
        objRemark.Text = "Cancel Remark"
        If objRemark.ShowDialog() <> Windows.Forms.DialogResult.OK Then
            Exit Sub
        End If
        Dim cCostId As String = gridView_OWN.CurrentRow.Cells("COSTID").Value.ToString
        Try
            tran = Nothing
            tran = cn.BeginTransaction
            strSql = " INSERT INTO " & cnStockDb & "..CANCELLEDTRAN"
            strSql += vbCrLf + "  (TRANDATE,BATCHNO,UPDATED,UPTIME,FLAG,REMARK1,REMARK2,USERID)"
            strSql += vbCrLf + "  VALUES"
            strSql += vbCrLf + "  ("
            strSql += vbCrLf + "  '" & Format(gridView_OWN.CurrentRow.Cells("TRANDATE").Value, "yyyy-MM-dd") & "'"
            strSql += vbCrLf + "  ,'" & gridView_OWN.CurrentRow.Cells("BATCHNO").Value.ToString & "'"
            strSql += vbCrLf + "  ,'" & Today.Date.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + "  ,'" & Date.Now.ToLongTimeString & "'"
            strSql += vbCrLf + "  ,'C'" 'FLAG CANCEL OR DELETE
            strSql += vbCrLf + "  ,'" & objRemark.cmbRemark1_OWN.Text & "'" 'REMARK1
            strSql += vbCrLf + "  ,'" & objRemark.cmbRemark2_OWN.Text & "'" 'REMARK2
            strSql += vbCrLf + "  ," & userId & "" 'USERID
            strSql += vbCrLf + "  )"
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, cCostId)
            'strSql = " UPDATE " & cnStockDb & "..ACCTRAN SET CANCEL = 'Y' WHERE BATCHNO = '" & cBatchno & "' AND COSTID = '" & cCostId & "'"
            'strSql += " UPDATE " & cnAdminDb & "..OUTSTANDING SET CANCEL = 'Y' WHERE BATCHNO = '" & cBatchno & "' AND COSTID = '" & cCostId & "'"
            'ExecQuery(SyncMode.Transaction, strSql, cn, tran, cCostId)
            tran.Commit()
            tran = Nothing
            gridView_OWN.CurrentRow.DefaultCellStyle.BackColor = Color.Red
            MsgBox("Successfully Cancelled", MsgBoxStyle.Information)
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
        gridView_OWN.Focus()
    End Sub

    Private Sub chkCmbAcName_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCmbAcName.Enter
        ''ACNAME
        strSql = " SELECT 'ALL' ACNAME,'ALL' ACCODE,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT ACNAME,CONVERT(vARCHAR,ACCODE),2 RESULT FROM " & cnAdminDb & "..ACHEAD"
        If chkCmbAcGroup.Text <> "ALL" Then
            strSql += " WHERE ISNULL(ACGRPCODE,0) IN (SELECT ACGRPCODE FROM " & cnAdminDb & "..ACGROUP WHERE ISNULL(ACGRPNAME,0) IN (" & GetQryString(chkCmbAcGroup.Text) & "))"
        End If
        strSql += " ORDER BY RESULT,ACNAME"
        dtAcName = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtAcName)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbAcName, dtAcName, "ACNAME", , "ALL")
    End Sub

    Private Sub chkMultiSelect_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkMultiSelect.CheckedChanged
        chkCmbAcGroup.Visible = chkMultiSelect.Checked
        chkCmbAcName.Visible = chkMultiSelect.Checked
        cmbAcGroup.Visible = Not chkMultiSelect.Checked
        cmbAcName.Visible = Not chkMultiSelect.Checked
    End Sub

    Private Sub cmbAcGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAcGroup.SelectedIndexChanged
        strSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD "
        If cmbAcGroup.Text <> "ALL" And cmbAcGroup.Text <> "" Then
            strSql += " WHERE ACGRPCODE = (SELECT ACGRPCODE FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPNAME = '" & cmbAcGroup.Text & "')"
        End If
        strSql += " ORDER BY ACNAME"
        objGPack.FillCombo(strSql, cmbAcName, , False)
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New frmAccountsLedger_Weight_Properties
        obj.p_chkMultiSelect = chkMultiSelect.Checked
        GetChecked_CheckedList(chkCmbAcGroup, obj.p_chkCmbAcGroup)
        obj.p_cmbAcGroup = cmbAcGroup.Text
        GetChecked_CheckedList(chkCmbAcName, obj.p_chkCmbAcName)
        obj.p_cmbAcName = cmbAcName.Text
        obj.p_chkWithRunningBalance = chkWithRunningBalance.Checked
        obj.p_chkOrderbyTranNo = chkOrderbyTranNo.Checked
        'GetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre)
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
        SetSettingsObj(obj, Me.Name, GetType(frmAccountsLedger_Weight_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmAccountsLedger_Weight_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmAccountsLedger_Weight_Properties))
        chkMultiSelect.Checked = obj.p_chkMultiSelect
        SetChecked_CheckedList(chkCmbAcGroup, obj.p_chkCmbAcGroup, "ALL")
        cmbAcGroup.Text = obj.p_cmbAcGroup
        SetChecked_CheckedList(chkCmbAcName, obj.p_chkCmbAcName, "ALL")
        chkCmbAcName.Text = obj.p_cmbAcName
        SetChecked_CheckedList(chkCmbAcName, obj.p_chkCmbAcName, Nothing)
        cmbAcName.Text = obj.p_cmbAcName
        chkWithRunningBalance.Checked = obj.p_chkWithRunningBalance
        chkOrderbyTranNo.Checked = obj.p_chkOrderbyTranNo
        'SetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre, cnCostName)
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

    Private Sub btnDuplicatePrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDuplicatePrint.Click
        ''Duplicate Print
        If IO.File.Exists(Application.StartupPath & "\BillPrint.exe") Then
            If gridView_OWN.CurrentRow.Cells("TRANDATE").Value.ToString = "" Then Exit Sub
            Dim pBatchno As String = gridView_OWN.CurrentRow.Cells("BATCHNO").Value.ToString
            Dim pBillDate As Date = gridView_OWN.CurrentRow.Cells("TRANDATE").Value
            Dim pParamStr As String = ""
            Dim prnmemsuffix As String = ""
            If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId
            Dim memfile As String = "\BillPrint" + prnmemsuffix.Trim & ".mem"

            Dim write As IO.StreamWriter
            write = IO.File.CreateText(Application.StartupPath & memfile)
            write.WriteLine(LSet("TYPE", 15) & ":ACC")
            pParamStr += LSet("TYPE", 15) & ":ACC" & ";"
            write.WriteLine(LSet("BATCHNO", 15) & ":" & pBatchno)
            pParamStr += LSet("BATCHNO", 15) & ":" & pBatchno & ";"
            write.WriteLine(LSet("TRANDATE", 15) & ":" & pBillDate.ToString("yyyy-MM-dd"))
            pParamStr += LSet("TRANDATE", 15) & ":" & pBillDate.ToString("yyyy-MM-dd") & ";"
            write.WriteLine(LSet("DUPLICATE", 15) & ":Y")
            pParamStr += LSet("DUPLICATE", 15) & ":Y"
            write.Flush()
            write.Close()

            If EXE_WITH_PARAM = False Then
                System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe")
            Else
                System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe", pParamStr)
            End If
        End If
    End Sub

    Private Sub btnChqPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChqPrint.Click
        ''Duplicate Print
        If IO.File.Exists(Application.StartupPath & "\ChequePrint.exe") Then
            If gridView_OWN.CurrentRow.Cells("TRANDATE").Value.ToString = "" Then Exit Sub
            Dim pBatchno As String = gridView_OWN.CurrentRow.Cells("BATCHNO").Value.ToString
            Dim pchqDate As String = gridView_OWN.CurrentRow.Cells("CHQDATE").Value.ToString
            Dim pPayee As String = gridView_OWN.CurrentRow.Cells(18).Value
            Dim pChqamt As String = gridView_OWN.CurrentRow.Cells("Credit").Value

            strSql = " SELECT CASE WHEN SUM(ISNULL(DEBIT,0))-SUM(ISNULL(CREDIT,0))>0 THEN SUM(ISNULL(DEBIT,0))-SUM(ISNULL(CREDIT,0)) ELSE 0 END AS DEBIT"
            strSql += " ,CASE WHEN SUM(ISNULL(CREDIT,0))-SUM(ISNULL(DEBIT,0))>0 THEN SUM(ISNULL(CREDIT,0))-SUM(ISNULL(DEBIT,0)) ELSE 0 END AS CREDIT"
            strSql += " FROM TEMP" & systemId & "ACCLEDGER"
            strSql += " WHERE BATCHNO='" & pBatchno & "'"
            Dim dtchqamt As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtchqamt)
            If dtchqamt.Rows.Count > 0 Then pChqamt = dtchqamt.Rows(0).Item("Credit").ToString

            Dim pChqformat As String = getchqbookfomat(gridView_OWN.CurrentRow.Cells(6).Value, gridView_OWN.CurrentRow.Cells(16).Value)
            If Val(pChqamt) = 0 Then MsgBox("Cheque Amount was zero", MsgBoxStyle.Critical) : Exit Sub
            If pChqformat = "" Then MsgBox("Cheque print format Not Defined", MsgBoxStyle.Critical) : Exit Sub
            If pPayee = "" Then MsgBox("Cheque payee is empty ", MsgBoxStyle.Critical) : Exit Sub
            Dim write As IO.StreamWriter
            write = IO.File.CreateText(Application.StartupPath & "\Cheque.Ini")
            write.WriteLine(LSet("Cheque Date", 16) & ":" & pchqDate)
            write.WriteLine(LSet("Payee", 16) & ":" & pPayee)
            write.WriteLine(LSet("Cheque Amount", 16) & ":" & pChqamt)
            write.WriteLine(LSet("Cheque Format ", 16) & ":" & pChqformat)
            write.Flush()
            write.Close()
            System.Diagnostics.Process.Start(Application.StartupPath & "\ChequePrint.exe")

        Else
            MsgBox("Printing Application<ChequePrint.exe> " & vbCrLf & " Not found", MsgBoxStyle.Critical)
            Exit Sub
        End If
    End Sub

    '    Cheque Date    :29/07/2011
    'Payee          :KOTAK SECURITIES LTD.
    'Cheque Amount  :10000
    'Cheque Format  :1

    Function getchqbookfomat(ByVal bankid As String, ByVal chqno As Long)
        Dim strsql As String = "select CHQFORMAT from " & cnStockDb & "..CHEQUEBOOK where bankcode = '" & bankid & "'" ' AND CHQNUMBER = BETWEEN
        Return objGPack.GetSqlValue(strsql, "CHQFORMAT")
    End Function

    Private Sub btnAccountInfo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccountInfo.Click
        Dim objAcHead As New frmAccountHead(cmbAcName.Text)
        objAcHead.ShowDialog()
    End Sub

    Private Sub tabGeneral_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tabGeneral.Click

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.Close()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim result = MessageBox.Show("Do you want to Print Dotmatrix(Yes) or Crystal Report(No)...", "Message", MessageBoxButtons.YesNoCancel)
        If result = Windows.Forms.DialogResult.Cancel Then
        ElseIf result = Windows.Forms.DialogResult.No Then
            CrystalPrint()
        ElseIf result = Windows.Forms.DialogResult.Yes Then
            LedgerDotmatrix()
        End If
    End Sub
    Private Sub CrystalPrint()
        Dim ds As New LedgerData
        Dim row As LedgerData.dtLedgerRow
        Dim objInvoiceReport As New LedgerData
        Dim i As Integer
        Dim dt As New DataTable
        Dim previousPage As Integer
        Dim nextPage As Integer

        Dim mtemptable As String = "TEMP" & systemId & "ACCINDEX"
        strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ACCINDEX')>0 DROP TABLE TEMP" & systemId & "ACCINDEX"
        strSql += " select acname AS LEDGER,count(*) AS LINES,0 as pageno into master.." & mtemptable & " from master..TEMP" & systemId & "ACCLEDGER group by acname"
        strSql += " union "
        strSql += " select  acname AS LEDGER,count(*) AS LINES,0 as pageno from master..TEMP" & systemId & "ACCLEDGER where remark1 <> '' group by acname"
        strSql += " union  "
        strSql += " select  acname AS LEDGER,count(*) AS LINES,0 as pageno  from master..TEMP" & systemId & "ACCLEDGER where remark2 <> '' group by acname"
        strSql += " order by acname"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        Dim mtemptable1 As String = "TEMP" & systemId & "ACCINDEX1"
        strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ACCINDEX1')>0 DROP TABLE TEMP" & systemId & "ACCINDEX1"
        strSql += " select LEDGER,SUM(LINES) LINES,0 as pageno into master.." & mtemptable1 & " from master.." & mtemptable & " group by LEDGER"
        strSql += " order by LEDGER"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " update a set a.pageno = CEILING(lines/63) from master.." & mtemptable1 & " a where isnull(Ledger,'')<>'' "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " SELECT * FROM master.." & mtemptable1 & " ORDER BY LEDGER"
        Dim dt1 As New DataTable()
        dt1.Clear()
        _ledgerName = ""
        _pageNo = ""
        _sNo = ""
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt1)

        dt1.Columns.Add("Pgno", Type.GetType("System.Int32"))
        dt1.Columns.Add("Sno", Type.GetType("System.Int32"))
        If dt1.Rows.Count > 0 Then
            For k As Integer = 0 To dt1.Rows.Count - 1
                If k = 0 Then
                    dt1.Rows(k).Item("Pgno") = 1
                    dt1.Rows(k).Item("Sno") = k + 1
                    _sNo += dt1.Rows(k).Item("Sno").ToString + vbCrLf
                    _pageNo += dt1.Rows(k).Item("Pgno").ToString + vbCrLf
                ElseIf dt1.Rows(k - 1).Item("Pageno").ToString = 0 Then
                    dt1.Rows(k).Item("pgno") = Val(dt1.Rows(k - 1).Item("pgno")) + 1
                    dt1.Rows(k).Item("Sno") = k + 1
                    _sNo += dt1.Rows(k).Item("Sno").ToString + vbCrLf
                    _pageNo += dt1.Rows(k).Item("Pgno").ToString + vbCrLf
                Else
                    dt1.Rows(k).Item("pgno") = Val(dt1.Rows(k - 1).Item("pgno")) + Val(dt1.Rows(k - 1).Item("Pageno").ToString)
                    dt1.Rows(k).Item("Sno") = k + 1
                    _sNo += dt1.Rows(k).Item("Sno").ToString + vbCrLf
                    _pageNo += dt1.Rows(k).Item("Pgno").ToString + vbCrLf
                End If
                _ledgerName += dt1.Rows(k).Item("LEDGER").ToString + vbCrLf
            Next
        End If

        dt.Clear()
        dt.TableName = "Crystal Report"
        strSql = " SELECT c.CompanyName,Address1,Address2,Address3,Phone,Convert(varchar(50),TranNo) as TranNo,CONVERT(VARCHAR(10), TranDate, 103)as TranDate,Contra,Convert(varchar(50),Debit)as Debit,Convert(varchar(50),Credit)as Credit,ACNAME"
        strSql += " ,(SELECT COUNT(*) FROM TEMP" & systemId & "ACCLEDGER AC WHERE AC.contra= Ac.acname) as SNO"
        strSql += " FROM TEMP" & systemId & "ACCLEDGER A"
        strSql += " Left join " & cnAdminDb & "..Company as C on C.CompanyID=A.CompanyID"
        strSql += " WHERE ISNULL(c.ACTIVE,'')<>'N'"
        strSql += " ORDER BY A.SNO"

        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then

            ''ds.Tables(0).Merge(dt)
            Dim rptInvoice As New cryLedger
            'Dim objReportViewerFrm As New frmReportViewer
            'rptInvoice.SetDataSource(dt)
            'objReportViewerFrm.CrystalReportViewer1.ReportSource = rptInvoice
            'objReportViewerFrm.Show()

            For j As Integer = 0 To dt.Rows.Count - 1
                If dt.Rows(j).Item("Contra").ToString = dt.Rows(j).Item("AcName").ToString Then
                    If dt.Rows(j).Item("TRANNO").ToString = "" And dt.Rows(j).Item("TRANDATE").ToString = "" Then
                        dt.Rows(j).Item("TRANNO") = "".ToString
                        dt.Rows(j).Item("TRANDATE") = ""
                        dt.Rows(j).Item("Contra") = ""
                        dt.Rows(j).Item("DEBIT") = ""
                        dt.Rows(j).Item("CREDIT") = ""
                        ' _ledgerName += dt.Rows(j).Item("AcName").ToString + vbCrLf
                        '_sNo += +1.ToString
                        '_sNo = _sNo + vbCrLf
                    End If
                End If
            Next
            For i = 0 To dt.Rows.Count - 1
                If dt.Rows(i).Item("Contra").ToString = dt.Rows(i).Item("AcName").ToString Then
                    If dt.Rows(i).Item("TRANNO").ToString = "" And dt.Rows(i).Item("TRANDATE").ToString = "" Then
                        dt.Rows(i).Item("TRANNO") = "".ToString
                        dt.Rows(i).Item("TRANDATE") = ""
                        dt.Rows(i).Item("Contra") = ""
                        dt.Rows(i).Item("DEBIT") = ""
                        dt.Rows(i).Item("CREDIT") = ""
                    End If
                End If
                row = objInvoiceReport.dtLedger.NewRow
                row.COMPANYNAME = dt.Rows(1).Item("COMPANYNAME").ToString
                row.ADDRESS1 = dt.Rows(1).Item("ADDRESS1").ToString
                row.ADDRESS2 = dt.Rows(1).Item("ADDRESS2").ToString
                row.ADDRESS3 = dt.Rows(1).Item("ADDRESS3").ToString
                row.PHONE = dt.Rows(1).Item("Phone").ToString
                row.TRANNO = dt.Rows(i).Item("TRANNO").ToString
                row.TRANDATE = dt.Rows(i).Item("TRANDATE").ToString
                'row.Type = dt2.Rows(i).Item("PAYMODE").ToString
                row.CONTRA = dt.Rows(i).Item("CONTRA").ToString
                If dt.Rows(i).Item("DEBIT").ToString <> "0.00" Then
                    row.DEBIT = dt.Rows(i).Item("DEBIT").ToString
                Else
                    row.DEBIT = ""
                End If
                If dt.Rows(i).Item("CREDIT").ToString <> "0.00" Then
                    row.CREDIT = dt.Rows(i).Item("CREDIT").ToString
                Else
                    row.CREDIT = ""
                End If
                row.ACNAME = dt.Rows(i).Item("ACNAME").ToString
                row.FROMDATE = dtpFrom.Value.Date.ToString("dd-MMM-yyyy")
                row.TODATE = dtpTo.Value.Date.ToString("dd-MMM-yyyy")
                row.LEDGERNAME = _ledgerName
                row.SNO = _sNo
                row.INDEXPAGE = _pageNo
                objInvoiceReport.dtLedger.AdddtLedgerRow(row)
            Next

            Dim objReportViewerFrm As New frmReportViewer
            rptInvoice.SetDataSource(objInvoiceReport)

            objReportViewerFrm.CrystalReportViewer1.ReportSource = rptInvoice
            objReportViewerFrm.Show()
        End If
    End Sub
    Function LedgerDotmatrix()
        Dim CompanyName, Address1, Address2, Address3, Phone As String
        Dim dtledger As New DataTable
        Dim dtindex As New DataTable
        Dim i As Integer
        Dim dt As New DataTable
        dtledger.Clear()
        dtledger = gridView_OWN.DataSource

        Dim mtemptable As String = "TEMP" & systemId & "ACCINDEX"
        strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ACCINDEX')>0 DROP TABLE TEMP" & systemId & "ACCINDEX"
        strSql += " select acname AS LEDGER,count(*) AS LINES,0 as pageno into master.." & mtemptable & " from master..TEMP" & systemId & "ACCLEDGER group by acname"
        strSql += " union "
        strSql += " select  acname AS LEDGER,count(*) AS LINES,0 as pageno from master..TEMP" & systemId & "ACCLEDGER where remark1 <> '' group by acname"
        strSql += " union  "
        strSql += " select  acname AS LEDGER,count(*) AS LINES,0 as pageno  from master..TEMP" & systemId & "ACCLEDGER where remark2 <> '' group by acname"
        strSql += " order by acname"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        Dim mtemptable1 As String = "TEMP" & systemId & "ACCINDEX1"
        strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ACCINDEX1')>0 DROP TABLE TEMP" & systemId & "ACCINDEX1"
        strSql += " select LEDGER,SUM(LINES) LINES,0 as pageno into master.." & mtemptable1 & " from master.." & mtemptable & " group by LEDGER"
        strSql += " order by LEDGER"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " update a set a.pageno = CEILING(lines/63) from master.." & mtemptable1 & " a where isnull(Ledger,'')<>'' "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " SELECT * FROM master.." & mtemptable1 & " ORDER BY LEDGER"
        Dim dt1 As New DataTable()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt1)
        dt1.Columns.Add("Pgno", Type.GetType("System.Int32"))
        dt1.Columns.Add("Sno", Type.GetType("System.Int32"))
        If dt1.Rows.Count > 0 Then
            For i = 0 To dt1.Rows.Count - 1
                If i = 0 Then
                    dt1.Rows(i).Item("Pgno") = 1
                    dt1.Rows(i).Item("Sno") = i + 1
                ElseIf dt1.Rows(i - 1).Item("Pageno").ToString = 0 Then
                    dt1.Rows(i).Item("pgno") = Val(dt1.Rows(i - 1).Item("pgno")) + 1
                    dt1.Rows(i).Item("Sno") = i + 1
                Else
                    dt1.Rows(i).Item("pgno") = Val(dt1.Rows(i - 1).Item("pgno")) + Val(dt1.Rows(i - 1).Item("Pageno").ToString)
                    dt1.Rows(i).Item("Sno") = i + 1
                End If
            Next
        End If

        strSql = " SELECT C.CompanyName,Address1,Address2,Address3,Phone"
        strSql += " FROM TEMP" & systemId & "ACCLEDGER A"
        strSql += " Left join " & cnAdminDb & "..Company as C on C.CompanyID=A.CompanyID"
        strSql += " WHERE ISNULL(C.ACTIVE,'')<>'N'"
        strSql += " ORDER BY SNO"

        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        CompanyName = dt.Rows(1).Item("COMPANYNAME").ToString
        Address1 = dt.Rows(1).Item("ADDRESS1").ToString
        Address2 = dt.Rows(1).Item("ADDRESS2").ToString
        Address3 = dt.Rows(1).Item("ADDRESS3").ToString
        Phone = dt.Rows(1).Item("Phone").ToString
        FileWrite = File.CreateText(Application.StartupPath + "\FilePrint.txt")
        PgNo = 0
        line = 0
        strprint = Chr(27) + Chr(67) + Chr(72)
        FileWrite.WriteLine(strprint)
        printindexheader()
        Dim str1 As String = Space(8) : Dim str2 As String = Space(11) : Dim str3 As String = Space(45)
        Dim str4 As String = Space(12) : Dim str5 As String = Space(12)
        Dim stri1 As String = Space(6) : Dim stri2 As String = Space(41) : Dim stri3 As String = Space(8)
        Dim headname As String
        If dt1.Rows.Count > 0 Then
            For i = 0 To dt1.Rows.Count - 1
                stri1 = LSet(dt1.Rows(i).Item("Sno").ToString, 6)
                stri2 = LSet(dt1.Rows(i).Item("LEDGER").ToString, 60)
                stri3 = LSet(dt1.Rows(i).Item("Pgno").ToString, 8)
                strprint = stri1 + stri2 + stri3
                FileWrite.WriteLine(strprint)
                line += 1
                If line > 66 Then
                    strprint = Chr(12)
                    FileWrite.WriteLine(strprint)
                    printindexheader()
                End If
            Next
        End If

        Dim mremark As String

        If dt1.Rows.Count > 0 Then
            For ij As Integer = 0 To dt1.Rows.Count - 1
                headname = dt1.Rows(ij).Item("LEDGER").ToString
                '  If headname <> "" Then
                strprint = Chr(12)
                FileWrite.WriteLine(strprint)
                Printheader(headname, CompanyName, Address1, Address2, Address3, Phone)

                Dim dtrows() As DataRow = dtledger.Select("AcName ='" & headname & "'")
                If dtrows.Length > 0 Then
                    For i = 0 To dtrows.Length - 1
                        'If headname <> dtrows(i).Item("CONTRA").ToString Then
                        mremark = ""
                        str1 = LSet(dtrows(i).Item("TRANNO").ToString, 8)
                        str2 = LSet(dtrows(i).Item("TRANDATE").ToString, 11)
                        str3 = dtrows(i).Item("CONTRA").ToString
                        If str3.Contains(vbCr) Then mremark = Mid(str3, InStr(str3, vbCr) + 1, Len(str3))
                        If Trim(mremark) <> "" Then str3 = LSet(Mid(str3, 1, InStr(str3, vbCr) - 1), 45)
                        If dtrows(i).Item("DEBIT").ToString <> "0.00" Then
                            str4 = RSet(dtrows(i).Item("DEBIT").ToString, 12)
                        Else
                            str4 = ""
                        End If
                        If dtrows(i).Item("CREDIT").ToString <> "0.00" Then
                            str5 = RSet(dtrows(i).Item("CREDIT").ToString, 12)
                        Else
                            str5 = ""
                        End If
                        str3 = LSet(str3, 45)
                        strprint = str1 + str2 + str3 + str4 + str5
                        FileWrite.WriteLine(strprint)
                        line += 1
remprint:
                        If Trim(mremark) <> "" Then
                            str3 = Trim(mremark) : mremark = ""
                            If str3.Contains(vbCr) Then mremark = Trim(Mid(str3, InStr(str3, vbCr) + 1, Len(str3)))
                            If Trim(mremark) <> "" Then str3 = LSet(Mid(str3, 1, InStr(str3, vbCr) - 1), 45)
                            strprint = Space(19) + IIf(Len(Trim(str3)) > 45, Chr(27) + "M" + str3 + Chr(27) + Chr(18), str3)
                            FileWrite.WriteLine(strprint)
                            line += 1
                        End If

                        If mremark <> "" Then GoTo remprint
                        If line >= 61 Then
                            strprint = "------------------------------------------------------------------------------------------"
                            FileWrite.WriteLine(strprint)
                            strprint = Chr(12)
                            FileWrite.WriteLine(strprint)
                            Printheader(headname, CompanyName, Address1, Address2, Address3, Phone)
                        End If
                        'End If
                    Next
                End If

                ' End If
            Next
        End If
        FileWrite.Close()
        line += 1
        frmPrinterSelect.Show()
    End Function
    Function printindexheader()
        line = 0
        strprint = Space(35) + Chr(14) + "INDEX" + Chr(27) + Chr(18)
        FileWrite.WriteLine(strprint)
        line = line + 1
        strprint = ("SNO   PARTICULARS                                                 PAGENO ")
        FileWrite.WriteLine(strprint) : line += 1
        strprint = "------------------------------------------------------------------------------------------"
        FileWrite.WriteLine(Trim(strprint)) : line += 1
    End Function
    Function Printheader(ByVal acname As String, ByVal CompanyName As String, Optional ByVal Address1 As String = Nothing, Optional ByVal Address2 As String = Nothing, Optional ByVal Address3 As String = Nothing, Optional ByVal Phone As String = Nothing) As Integer
        PgNo += 1
        line = 0
        strprint = Space((80 - Len(CompanyName)) / 2) + CompanyName
        FileWrite.WriteLine(strprint) : line += 1
        strprint = Space((80 - Len(Trim(Address1))) / 2) + Address1
        FileWrite.WriteLine(strprint) : line += 1
        strprint = Space((80 - Len(Trim(Address2))) / 2) + Address2
        FileWrite.WriteLine(strprint) : line += 1
        strprint = Space((80 - Len(Trim(Address3))) / 2) + Address3
        FileWrite.WriteLine(strprint) : line += 1
        strprint = Space((80 - Len(Trim(Phone))) / 2) + Phone
        FileWrite.WriteLine(strprint) : line += 1
        'line += 2
        strprint = ("Statement of accounts for the period  from " & dtpFrom.Value.Date.ToString("dd/MM/yyyy") & " to " & dtpTo.Value.Date.ToString("dd/MM/yyyy") & "             Pg #" & PgNo)
        FileWrite.WriteLine(strprint) : line += 1
        strprint = Space((80 - Len(Trim(acname))) / 2) + acname
        FileWrite.WriteLine(strprint) : line += 1
        strprint = ("")
        FileWrite.WriteLine(strprint) : line = line + 1
        strprint = "------------------------------------------------------------------------------------------"
        FileWrite.WriteLine(Trim(strprint)) : line += 1

        Dim str1 As String = Space(8) : Dim str2 As String = Space(11) : Dim str3 As String = Space(45)
        Dim str4 As String = Space(12) : Dim str5 As String = Space(12)
        str1 = LSet("VOU NO", 8)
        str2 = LSet("DATE", 11)
        str3 = LSet("PARTICULARS ", 45)
        str4 = RSet("DEBIT ", 12)
        str5 = RSet("CREDIT ", 12)
        strprint = str1 + str2 + str3 + Space(2) + str4 + str5
        FileWrite.WriteLine(strprint) : line += 1
        strprint = "------------------------------------------------------------------------------------------"
        FileWrite.WriteLine(Trim(strprint)) : line += 1
    End Function
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        ledgerprint()
    End Sub

    Private Sub dtpFrom_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtpFrom.KeyDown

    End Sub

    Private Sub dtpFrom_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtpFrom.Leave

    End Sub

    Private Sub CheckedComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCmbCompany.SelectedIndexChanged

    End Sub


    Private Sub ChkSummaryBalance_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkSummaryBalance.CheckedChanged

    End Sub
End Class

Public Class frmAccountsLedger_Weight_Properties
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