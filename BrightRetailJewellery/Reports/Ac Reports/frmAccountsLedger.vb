Imports System.Data.OleDb
Imports System.IO
Imports System.Globalization

Public Class frmAccountsLedger
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
    'Dim objMoreOption As New frmAccLedgerMore
    Dim objMoreOption As frmAccLedgerMore
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
    Dim sysid As String
    Dim _Chqformat As Integer = 1
    Dim flagAge As Boolean
    Dim _Accode As String = ""
    Dim Rpt_LView As Boolean = IIf(GetAdmindbSoftValue("RPT_LEDGERVIEW", "N") = "Y", True, False)
    Dim Rpt_LView_Age As Boolean = IIf(GetAdmindbSoftValue("RPT_LEDGERVIEW_AGE", "N") = "Y", True, False)
    Dim Rpt_LView_Sledger As Boolean = IIf(GetAdmindbSoftValue("RPT_LEDGERVIEW_SLEDGER", "N") = "Y", True, False)
    Dim Rpt_LView_ZEROAC As Boolean = IIf(GetAdmindbSoftValue("RPT_LEDGERVIEW_ZEROAC", "N") = "Y", True, False)
    Dim Rpt_LView_AccodeSearch As Boolean = IIf(GetAdmindbSoftValue("RPT_LEDGERVIEW_ACCODESEARCH", "N") = "Y", True, False)

#Region "Variable Cheque Print Only"

    Public chqDate As String = ""
    Public chqActPayee As String = ""
    Public chqPayee1 As String = ""
    Public chqPayee2 As String = ""
    Public chqAmtName1 As String = ""
    Public chqAmtName2 As String = ""
    Public chqTxtLine1 As String = ""
    Public chqTxtLine2 As String = ""
    Public chqTxtLine3 As String = ""
    Public chqNotAbove As String = ""
    Public chqAmtValue As String = ""
    Public chqBearer As String = ""
    Public chqBankName As String = ""
#End Region

    Function funcAccountsEntry(ByVal rwIndex As String) As Integer
        'strSql = " SELECT "
        'strSql += vbCrLf + " CASE WHEN TRANMODE = 'C' THEN 'Cr' else 'Dr' end as TYPE"
        'strSql += vbCrLf + " ," & cnAdminDb & ".DBO.WORDCAPS((SELECT TOP 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.ACCODE))DESCRIPTION"
        'strSql += vbCrLf + " ,REMARK1 NARRATION1"
        'strSql += vbCrLf + " ,REMARK2 NARRATION2"
        'strSql += vbCrLf + " ,CHQCARDNO CHQNO"
        'strSql += vbCrLf + " ,CHQDATE,CHQCARDREF CHQDETAIL"
        'strSql += vbCrLf + " ,(SELECT TOP 1 TDSPER FROM " & cnStockDb & "..ACCTRAN WHERE TRANDATE = T.TRANDATE AND BATCHNO = T.BATCHNO AND ACCODE = T.ACCODE AND TDSPER <> 0)AS TDSPER"
        'strSql += vbCrLf + " ,(SELECT TOP 1 TDSAMOUNT TDSAMT FROM " & cnStockDb & "..ACCTRAN WHERE TRANDATE = T.TRANDATE AND BATCHNO = T.BATCHNO AND ACCODE = T.ACCODE AND TDSAMOUNT <> 0)AS TDSAMT"
        'strSql += vbCrLf + " ,(SELECT TOP 1 TDSCATID FROM " & cnStockDb & "..ACCTRAN WHERE TRANDATE = T.TRANDATE AND BATCHNO = T.BATCHNO AND ACCODE = T.ACCODE AND TDSCATID <> 0)AS TDSCATID"
        'strSql += vbCrLf + " ,NULL VATPER,NULL VATAMT,CONVERT(VARCHAR(10),NULL) VATCATID"
        'strSql += vbCrLf + " ,NULL SRVTAMT,NULL SRVTPER"
        'strSql += " ,CONVERT(VARCHAR(100),NULL) BALANCE"
        'strSql += vbCrLf + " ,SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT - CASE WHEN PAYMODE = 'JE' THEN TDSAMOUNT ELSE 0 END ELSE NULL END) AS DEBIT"
        'strSql += vbCrLf + " ,SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT - CASE WHEN PAYMODE = 'JE' THEN TDSAMOUNT ELSE 0 END ELSE NULL END) AS CREDIT"
        'strSql += vbCrLf + " ,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = T.SCOSTID)SCOSTCENTRE"
        ''strSql += vbCrLf + " ,SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT-TDSAMOUNT ELSE NULL END) AS DEBIT"
        ''strSql += vbCrLf + " ,SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT-TDSAMOUNT ELSE NULL END) AS CREDIT"
        'strSql += vbCrLf + " ,CONVERT(VARCHAR,FLAG)GENBY,WT_ENTORDER AS KEYNO"
        'strSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN T"
        'strSql += vbCrLf + " WHERE BATCHNO = '" & gridView_OWN.Rows(rwIndex).Cells("BATCHNO").Value.ToString & "'"
        'strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'  "
        'strSql += vbCrLf + " AND ISNULL(FLAG,'') <> 'T'"
        ''strSql += " AND ISNULL(WT_ENTORDER,0) <> 0"
        'strSql += vbCrLf + " GROUP BY TRANMODE,ACCODE,REMARK1,REMARK2,CHQCARDNO,CHQDATE,CHQCARDREF,WT_ENTORDER,FLAG,TRANDATE,BATCHNO,SCOSTID"
        'strSql += vbCrLf + " ORDER BY WT_ENTORDER"

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
        strSql += vbCrLf + " ,NULL SRVTAMT,NULL SRVTPER"
        strSql += " ,CONVERT(VARCHAR(100),NULL) BALANCE"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),NULL)SGSTPER,CONVERT(NUMERIC(15,2),NULL)SGST,CONVERT(VARCHAR(10),NULL)SGSTID"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),NULL)CGSTPER,CONVERT(NUMERIC(15,2),NULL)CGST,CONVERT(VARCHAR(10),NULL)CGSTID"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),NULL)IGSTPER,CONVERT(NUMERIC(15,2),NULL)IGST,CONVERT(VARCHAR(10),NULL)IGSTID"
        strSql += vbCrLf + " ,SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT - CASE WHEN PAYMODE = 'JE' THEN TDSAMOUNT ELSE 0 END ELSE NULL END) AS DEBIT"
        strSql += vbCrLf + " ,SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT - CASE WHEN PAYMODE = 'JE' THEN TDSAMOUNT ELSE 0 END ELSE NULL END) AS CREDIT"
        strSql += vbCrLf + " ,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = T.SCOSTID)SCOSTCENTRE"
        strSql += vbCrLf + " ,CONVERT(VARCHAR,FLAG)GENBY,WT_ENTORDER AS KEYNO,PCODE"
        strSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN T"
        strSql += vbCrLf + " WHERE BATCHNO = '" & gridView_OWN.Rows(rwIndex).Cells("BATCHNO").Value.ToString & "'"
        strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'  "
        strSql += vbCrLf + " AND ISNULL(FLAG,'') <> 'T'"
        strSql += " AND ISNULL(TDSAMOUNT,0)<>0 "
        strSql += vbCrLf + " GROUP BY TRANMODE,ACCODE,REMARK1,REMARK2,CHQCARDNO,CHQDATE,CHQCARDREF,WT_ENTORDER,FLAG,TRANDATE,BATCHNO,SCOSTID,PCODE"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT "
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
        strSql += vbCrLf + " ,NULL SRVTAMT,NULL SRVTPER"
        strSql += vbCrLf + " ,CONVERT(VARCHAR(100),NULL) BALANCE"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),NULL)SGSTPER,CONVERT(NUMERIC(15,2),NULL)SGST,CONVERT(VARCHAR(10),NULL)SGSTID"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),NULL)CGSTPER,CONVERT(NUMERIC(15,2),NULL)CGST,CONVERT(VARCHAR(10),NULL)CGSTID"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),NULL)IGSTPER,CONVERT(NUMERIC(15,2),NULL)IGST,CONVERT(VARCHAR(10),NULL)IGSTID"
        strSql += vbCrLf + " ,SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT - CASE WHEN PAYMODE = 'JE' THEN TDSAMOUNT ELSE 0 END ELSE NULL END) AS DEBIT"
        strSql += vbCrLf + " ,SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT - CASE WHEN PAYMODE = 'JE' THEN TDSAMOUNT ELSE 0 END ELSE NULL END) AS CREDIT"
        strSql += vbCrLf + " ,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = T.SCOSTID)SCOSTCENTRE"
        'strSql += vbCrLf + " ,SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT-TDSAMOUNT ELSE NULL END) AS DEBIT"
        'strSql += vbCrLf + " ,SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT-TDSAMOUNT ELSE NULL END) AS CREDIT"
        strSql += vbCrLf + " ,CONVERT(VARCHAR,FLAG)GENBY,WT_ENTORDER AS KEYNO,PCODE"
        strSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN T"
        strSql += vbCrLf + " WHERE BATCHNO = '" & gridView_OWN.Rows(rwIndex).Cells("BATCHNO").Value.ToString & "'"
        strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'  "
        strSql += vbCrLf + " AND ISNULL(FLAG,'') <> 'T'"
        strSql += " AND ISNULL(TDSAMOUNT,0)=0 "

        strSql += " AND WT_ENTORDER NOT IN "

        strSql += vbCrLf + " (SELECT ISNULL(WT_ENTORDER,0) FROM " & cnStockDb & "..ACCTRAN T"
        strSql += vbCrLf + " WHERE BATCHNO = '" & gridView_OWN.Rows(rwIndex).Cells("BATCHNO").Value.ToString & "'"
        strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'  "
        strSql += vbCrLf + " AND ISNULL(FLAG,'') <> 'T'"
        strSql += " AND ISNULL(TDSAMOUNT,0)<>0) "

        strSql += vbCrLf + " GROUP BY TRANMODE,ACCODE,REMARK1,REMARK2,CHQCARDNO,CHQDATE,CHQCARDREF,WT_ENTORDER,FLAG,TRANDATE,BATCHNO,SCOSTID,PCODE"
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
            strSql += vbCrLf + " ,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = T.COSTID)COSTNAME,PCODE"
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
            If dtAccDet.Rows(0).Item("PCODE").ToString = "SA" Then
                objAcc.cmbAgainst_Own.Text = "SALES"
            ElseIf dtAccDet.Rows(0).Item("PCODE").ToString = "PU" Then
                objAcc.cmbAgainst_Own.Text = "PURCHASE"
            ElseIf dtAccDet.Rows(0).Item("PCODE").ToString = "PE" Then
                objAcc.cmbAgainst_Own.Text = "EXPENSE"
            ElseIf dtAccDet.Rows(0).Item("PCODE").ToString = "JB" Then
                objAcc.cmbAgainst_Own.Text = "JOBWORK"
            Else
                objAcc.cmbAgainst_Own.Text = ""
            End If
            objAcc.cmbCostCenter_MAN.Text = dtAccDet.Rows(0).Item("COSTNAME").ToString
            'objAcc.cmbCostCenter_MAN.Enabled = False
            objAcc.Edchqno = dtAccDet.Rows(0).Item("CHQCARDNO").ToString
            objAcc.Edchqdate = IIf(IsDBNull(dtAccDet.Rows(0).Item("CHQDATE")), Nothing, dtAccDet.Rows(0).Item("CHQDATE"))
            objAcc.Edchqdetail = dtAccDet.Rows(0).Item("CHQCARDREF").ToString
            objAcc.txtNarration1.Text = dtAcc.Rows(0).Item("NARRATION1").ToString
            objAcc.txtNarration2.Text = dtAcc.Rows(0).Item("NARRATION2").ToString
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
                'If Rpt_LView_Age And flagAge Then
                '    .Width = .Width - 50
                'End If
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
            If .Columns.Contains("AGE") Then
                With .Columns("AGE")
                    .HeaderText = "AGE"
                    .Width = 50
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    If Rpt_LView_Age And flagAge Then
                        .Visible = True
                    Else
                        .Visible = False
                    End If
                End With
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
                    If gridView_OWN.Columns.Contains("DISCEMPNAME") Then gridView_OWN.Columns("DISCEMPNAME").Visible = .chkDiscAuthEmp.Checked
                    If .chkDiscAuthEmp.Checked And gridView_OWN.Columns.Contains("DISCEMPNAME") Then
                        gridView_OWN.Columns("DISCEMPNAME").SortMode = DataGridViewColumnSortMode.NotSortable
                        gridView_OWN.Columns("DISCEMPNAME").Width = 120
                        gridView_OWN.Columns("DISCEMPNAME").HeaderText = "DISCEMPNAME"
                    End If
                    gridView_OWN.Columns("EMPNAME").Visible = .chkEmp.Checked
                    If .chkEmp.Checked Then
                        gridView_OWN.Columns("EMPNAME").SortMode = DataGridViewColumnSortMode.NotSortable
                        gridView_OWN.Columns("EMPNAME").Width = 120
                        gridView_OWN.Columns("EMPNAME").HeaderText = "EMPNAME"
                    End If
                    If gridView_OWN.Columns.Contains("PANNO") Then gridView_OWN.Columns("PANNO").Visible = .chkPanNo.Checked
                    If .chkPanNo.Checked And gridView_OWN.Columns.Contains("PANNO") Then
                        gridView_OWN.Columns("PANNO").SortMode = DataGridViewColumnSortMode.NotSortable
                        gridView_OWN.Columns("PANNO").Width = 150
                        gridView_OWN.Columns("PANNO").HeaderText = "PAN"
                    End If
                    If gridView_OWN.Columns.Contains("GSTNO") Then gridView_OWN.Columns("GSTNO").Visible = .chkGstNo.Checked
                    If .chkGstNo.Checked And gridView_OWN.Columns.Contains("GSTNO") Then
                        gridView_OWN.Columns("GSTNO").SortMode = DataGridViewColumnSortMode.NotSortable
                        gridView_OWN.Columns("GSTNO").Width = 200
                        gridView_OWN.Columns("GSTNO").HeaderText = "GSTNO"
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
        If chkMultiSelect.Checked = False Then
            strSql = "SELECT ACTYPE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME='" & cmbAcName.Text & "'"
            Dim AcType As String = objGPack.GetSqlValue(strSql, "ACTYPE", "").ToString
            If AcType = "D" Or AcType = "G" Then
                flagAge = True
            End If
            strSql = "SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME='" & cmbAcName.Text & "'"
            _Accode = objGPack.GetSqlValue(strSql, "ACCODE", "").ToString
        End If
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
            qry += " AND COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(chkCmbCompany.Text) & "))"
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
        If cmbLedger.Enabled = True And cmbLedger.Text <> "" Then
            qry += " AND SACCODE = (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbLedger.Text & "')"
        End If

        'qry += " AND COMPANYID = '" & strCompanyId & "' "
        If chkCancelonly.Checked Then
            qry += " AND ISNULL(CANCEL,'') = 'Y'"
        Else
            qry += " AND ISNULL(CANCEL,'') = ''"
        End If

        Return qry
    End Function

    Function funcBillViewDetails(ByVal rwIndex As Integer) As Integer

        '' ''strSql = " SELECT T.TRANNO,CONVERT(VARCHAR,T.TRANDATE,103)AS TRANDATE,HE.ACNAME"
        '' ''strSql += vbCrLf + " ,CASE WHEN T.TRANMODE = 'D' AND T.AMOUNT <> 0 THEN T.AMOUNT ELSE NULL END AS DEBIT"
        '' ''strSql += vbCrLf + " ,CASE WHEN T.TRANMODE = 'C' AND T.AMOUNT <> 0 THEN T.AMOUNT ELSE NULL END AS CREDIT,T.COMPANYID"
        '' ''strSql += vbCrLf + " ,REMARK1 + CASE WHEN ISNULL(REMARK2,'')<>'' THEN  '/' + REMARK2 ELSE REMARK2 END REMARK FROM " & cnStockDb & "..ACCTRAN AS T"
        '' ''strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ACHEAD AS HE ON HE.ACCODE = T.ACCODE"
        '' ''strSql += vbCrLf + " WHERE " 'TRANNO = " & Val(gridView_OWN.Rows(rwIndex).Cells("TRANNO").Value.ToString) & " "
        '' ''strSql += vbCrLf + " T.BATCHNO = '" & gridView_OWN.Rows(rwIndex).Cells("BATCHNO").Value.ToString & "'"
        '' ''strSql += vbCrLf + " AND  T.COMPANYID = '" & strCompanyId & "'"
        '' ''strSql += vbCrLf + " AND  ISNULL(T.CANCEL,'')=''"
        ' '' ''strSql += vbcrlf +  " "


        strSql = " SELECT TRANNO,TRANDATE,ACNAME,CASE WHEN DEBIT - CREDIT > 0  THEN DEBIT - CREDIT ELSE 0 END AS DEBIT"
        strSql += vbCrLf + " ,CASE WHEN CREDIT - DEBIT > 0  THEN CREDIT - DEBIT ELSE 0 END AS CREDIT,REMARK"
        strSql += vbCrLf + " FROM ("
        strSql += vbCrLf + " SELECT TRANNO,CONVERT(VARCHAR,TRANDATE,103)TRANDATE"
        strSql += vbCrLf + " ,(SELECT TOP 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE =T.ACCODE )AS ACNAME"
        strSql += vbCrLf + " ,SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE 0 END) AS DEBIT"
        strSql += vbCrLf + " ,SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE 0 END) AS CREDIT"
        strSql += vbCrLf + " ,T.COMPANYID,REMARK1 + CASE WHEN ISNULL(REMARK2,'')<>'' THEN  '/' + REMARK2 ELSE REMARK2 END REMARK "
        strSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN AS T"
        strSql += vbCrLf + " WHERE " 'TRANNO = " & Val(gridView_OWN.Rows(rwIndex).Cells("TRANNO").Value.ToString) & " "
        strSql += vbCrLf + " BATCHNO = '" & gridView_OWN.Rows(rwIndex).Cells("BATCHNO").Value.ToString & "'"
        strSql += vbCrLf + " AND  T.COMPANYID = '" & strCompanyId & "'"
        strSql += vbCrLf + " GROUP BY ACCODE,TRANNO,TRANDATE,T.COMPANYID,T.REMARK1,T.REMARK2"
        strSql += vbCrLf + " )X"
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

    Private Sub SetSummaryDetails(ByVal rwIndex As Integer, ByVal col1 As String, ByVal col2 As String,
    ByVal col3 As String, ByVal col4 As String, ByVal col5 As String,
    ByVal col6 As String, ByVal col7 As String, ByVal col8 As String)
        If Not rwIndex <= dtSummary.Rows.Count - 1 Then Exit Sub
        With dtSummary.Rows(rwIndex)
            .Item("COL1") = col1
            .Item("COL2") = col2
            .Item("COL3") = col3
            .Item("COL4") = col4
            .Item("COL5") = col5
            .Item("COL6") = col6
            .Item("COL7") = col7
            .Item("COL8") = col8
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
            SetSummaryDetails(0, issue + "NO", .Cells("CHQCARDNO").Value.ToString,
            "PCS", IIf(Val(.Cells("PCS").Value.ToString) <> 0, .Cells("PCS").Value.ToString, ""), "CURSOR BAL", curDebit, curCredit, "")
            SetSummaryDetails(1, issue + "DATE", .Cells("CHQDATE").Value.ToString,
            "GRSWT", IIf(Val(.Cells("GRSWT").Value.ToString) <> 0, .Cells("GRSWT").Value.ToString, ""), "TOTAL", totDebit, totCredit, "")
            SetSummaryDetails(2, "BANKNAME", .Cells("CHQCARDREF").Value.ToString,
            "NETWT", IIf(Val(.Cells("NETWT").Value.ToString) <> 0, .Cells("NETWT").Value.ToString, ""), "BALANCE", balDebit, balCredit, "")
            SetSummaryDetails(3, "ACNAME", .Cells("ACNAME").Value.ToString,
            "REFNO", IIf(.Cells("REFNO").Value.ToString <> "0", .Cells("REFNO").Value.ToString, ""),
            "TOTPCS", totPcs, "", "")
            If .Cells("REFDATE").Value.ToString <> "" Then
                SetSummaryDetails(4, "REMARK1", .Cells("REMARK1").Value.ToString,
                "REFDATE", Format(.Cells("REFDATE").Value, "dd/MM/yyyy"), "TOTGRSWT", totGrswt, "", "")
            Else
                SetSummaryDetails(4, "REMARK1", .Cells("REMARK1").Value.ToString,
    "REFDATE", "", "TOTGRSWT", totGrswt, "", "")
            End If
            'SetSummaryDetails(5, "REMARK2", .Cells("REMARK2").Value.ToString,
            '"TRANTYPE", FlagTran(.Cells("FLAG").Value.ToString), "TOTNETWT", totNetWt, "", "")
            'SetSummaryDetails(6, "UPDATED", .Cells("UPDATED").Value.ToString,
            '"COSTNAME", .Cells("COSTNAME").Value.ToString, "USERNAME", .Cells("USERNAME").Value.ToString, IIf(.Cells("DISCEMPNAME").Value.ToString <> "", "AUTH :" & .Cells("DISCEMPNAME").Value.ToString, ""), .Cells("EMPNAME").Value.ToString)
            If gridView_OWN.Columns.Contains("FLAG") Then
                SetSummaryDetails(5, "REMARK2", .Cells("REMARK2").Value.ToString,
            "TRANTYPE", FlagTran(.Cells("FLAG").Value.ToString), "TOTNETWT", totNetWt, "", "")
            Else
                SetSummaryDetails(5, "REMARK2", .Cells("REMARK2").Value.ToString,
            "TRANTYPE", "", "TOTNETWT", totNetWt, "", "")
            End If
            If gridView_OWN.Columns.Contains("FLAG") Then
                SetSummaryDetails(6, "UPDATED", .Cells("UPDATED").Value.ToString,
            "COSTNAME", .Cells("COSTNAME").Value.ToString, "USERNAME", .Cells("USERNAME").Value.ToString, IIf(.Cells("DISCEMPNAME").Value.ToString <> "", "AUTH :" & .Cells("DISCEMPNAME").Value.ToString, ""), .Cells("EMPNAME").Value.ToString)
            Else
                SetSummaryDetails(6, "UPDATED", .Cells("UPDATED").Value.ToString,
            "COSTNAME", .Cells("COSTNAME").Value.ToString, "USERNAME", .Cells("USERNAME").Value.ToString, "", .Cells("EMPNAME").Value.ToString)
            End If
        End With
        gridSummary.Refresh()
    End Function

    Function FlagTran(ByVal val As String) As String
        ''If val = "C" Then
        ''    Return "CHEQUE"
        ''ElseIf val = "N" Then
        ''    Return "NEFT"
        ''ElseIf val = "I" Then
        ''    Return "IMPS"
        ''ElseIf val = "R" Then
        ''    Return "RTGS"
        ''ElseIf val = "F" Then
        ''    Return "FUND TRANSFER"
        ''End If

        If val <> "" Then
            strSql = " SELECT CASE WHEN ISNULL(X.MODENAME,'')<>'' THEN X.MODENAME ELSE 'CHEQUE' END MODENAME  FROM ( "
            strSql += " SELECT ISNULL((SELECT  ISNULL(MODENAME,'')MODENAME FROM " & cnAdminDb & "..PAYMENTMODE WHERE CONVERT(VARCHAR,MODEID)='" & val & "' GROUP BY MODENAME),'')MODENAME "
            strSql += " )X "
            Return GetSqlValue(cn, strSql)
        End If

        Return ""
    End Function

    Function funcContraWiseSummury() As Integer
        dtContraSummary = New DataTable
        strSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & sysid & "ACCLEDGER_CONTSUMMARY')>0"
        strSql += " DROP TABLE TEMPTABLEDB..TEMP" & sysid & "ACCLEDGER_CONTSUMMARY"
        strSql += " SELECT ACNAME,CONTRA"
        strSql += " ,CASE WHEN SUM(ISNULL(DEBIT,0))-SUM(ISNULL(CREDIT,0))>0 THEN SUM(ISNULL(DEBIT,0))-SUM(ISNULL(CREDIT,0)) ELSE 0 END AS DEBIT"
        strSql += " ,CASE WHEN SUM(ISNULL(CREDIT,0))-SUM(ISNULL(DEBIT,0))>0 THEN SUM(ISNULL(CREDIT,0))-SUM(ISNULL(DEBIT,0)) ELSE 0 END AS CREDIT"
        strSql += " ,2 RESULT"
        strSql += " INTO TEMPTABLEDB..TEMP" & sysid & "ACCLEDGER_CONTSUMMARY"
        strSql += " FROM TEMPTABLEDB..TEMP" & sysid & "ACCLEDGER"
        strSql += " WHERE RESULT = 2"
        strSql += " GROUP BY ACNAME,CONTRA"
        strSql += " UNION ALL"
        strSql += " SELECT ACNAME,CONTRA,SUM(ISNULL(DEBIT,0)) AS DEBIT,SUM(ISNULL(CREDIT,0))CREDIT,1 RESULT"
        strSql += " FROM TEMPTABLEDB..TEMP" & sysid & "ACCLEDGER"
        strSql += " WHERE RESULT = 1"
        strSql += " GROUP BY ACNAME,CONTRA"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        If chkCmbAcName.Text = "ALL" Then
            strSql = " INSERT INTO TEMPTABLEDB..TEMP" & sysid & "ACCLEDGER_CONTSUMMARY(ACNAME,CONTRA,RESULT)"
            strSql += " SELECT DISTINCT ACNAME,ACNAME,0 RESULT FROM TEMP" & sysid & "ACCLEDGER_CONTSUMMARY"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End If
        strSql = " "
        strSql += " INSERT INTO TEMPTABLEDB..TEMP" & sysid & "ACCLEDGER_CONTSUMMARY"
        strSql += " SELECT ACNAME,'TOTAL' CONTRA,SUM(DEBIT)DEBIT,SUM(CREDIT)CREDIT,3 RESULT"
        strSql += " FROM TEMPTABLEDB..TEMP" & sysid & "ACCLEDGER_CONTSUMMARY WHERE RESULT IN(1,2)"
        strSql += " GROUP BY ACNAME"
        strSql += " "
        strSql += " INSERT INTO TEMPTABLEDB..TEMP" & sysid & "ACCLEDGER_CONTSUMMARY"
        strSql += " SELECT ACNAME,'BALANCE' CONTRA"
        strSql += " ,CASE WHEN SUM(ISNULL(DEBIT,0))-SUM(ISNULL(CREDIT,0))>0 THEN SUM(ISNULL(DEBIT,0))-SUM(ISNULL(CREDIT,0)) ELSE 0 END AS DEBIT"
        strSql += " ,CASE WHEN SUM(ISNULL(CREDIT,0))-SUM(ISNULL(DEBIT,0))>0 THEN SUM(ISNULL(CREDIT,0))-SUM(ISNULL(DEBIT,0)) ELSE 0 END AS CREDIT"
        strSql += " ,4 RESULT"
        strSql += " FROM TEMPTABLEDB..TEMP" & sysid & "ACCLEDGER_CONTSUMMARY WHERE RESULT = 3"
        strSql += " GROUP BY ACNAME"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strSql = " SELECT * FROM TEMPTABLEDB..TEMP" & sysid & "ACCLEDGER_CONTSUMMARY ORDER BY ACNAME,RESULT"
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


    Function funcLedgerSummury() As Integer
        dtContraSummary = New DataTable
        strSql = vbCrLf + "  		IF (SELECT 1 FROM SYSOBJECTS WHERE NAME='TEMP" & sysid & "ACCLEDGER_SUMMARY') > 0 DROP TABLE TEMP" & sysid & "ACCLEDGER_SUMMARY"
        strSql += vbCrLf + "  		SELECT ACCODE,ACNAME,"
        strSql += vbCrLf + "  		DATENAME(MONTH,TRANDATE) MONTHNAME,"
        strSql += vbCrLf + "  		DATEPART(MONTH,TRANDATE)MONTH,DATEPART(YEAR,TRANDATE)YEAR,SUM(DEBIT)DEBIT,"
        strSql += vbCrLf + "  		SUM(CREDIT)CREDIT,CONVERT(NUMERIC(15,2),0.00) AS RUNTOT,CONVERT(VARCHAR(500),'') AS RUNTOTAL,RESULT "
        strSql += vbCrLf + "  		INTO TEMP" & sysid & "ACCLEDGER_SUMMARY FROM TEMPTABLEDB..TEMP" & sysid & "ACCLEDGER "
        strSql += vbCrLf + "  		GROUP BY ACCODE,ACNAME,DATEPART(MONTH,TRANDATE),DATEPART(YEAR,TRANDATE),RESULT,DATENAME(MONTH,TRANDATE) ORDER BY RESULT,YEAR,MONTH"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strSql = vbCrLf + "  		DECLARE @ACCODE VARCHAR(10)"
        strSql += vbCrLf + "  		DECLARE @PREACCODE VARCHAR(10)"
        strSql += vbCrLf + "  		DECLARE @DEBIT NUMERIC(20,2)"
        strSql += vbCrLf + "  		DECLARE @CREDIT NUMERIC(20,2)"
        strSql += vbCrLf + "  		DECLARE @RUNTOT NUMERIC(20,2)"
        strSql += vbCrLf + "  		DECLARE RUNCUR CURSOR"
        strSql += vbCrLf + "  		FOR SELECT ISNULL(DEBIT,0)DEBIT,ISNULL(CREDIT,0)CREDIT,ACCODE FROM TEMP" & sysid & "ACCLEDGER_SUMMARY WHERE RESULT IN (1,2) "
        strSql += vbCrLf + "  		OPEN RUNCUR"
        strSql += vbCrLf + "  		WHILE 1=1"
        strSql += vbCrLf + "  		BEGIN"
        strSql += vbCrLf + "  		FETCH NEXT FROM RUNCUR INTO @DEBIT,@CREDIT,@ACCODE"
        strSql += vbCrLf + "  		IF @@FETCH_STATUS = -1 BREAK"
        strSql += vbCrLf + "  		IF ISNULL(@ACCODE,'') <> ISNULL(@PREACCODE ,'')"
        strSql += vbCrLf + "  		BEGIN"
        strSql += vbCrLf + "  		SELECT @PREACCODE = @ACCODE"
        strSql += vbCrLf + "  		SELECT @RUNTOT = 0"
        strSql += vbCrLf + "  		END"
        strSql += vbCrLf + "  		SELECT @RUNTOT = ISNULL(@RUNTOT,0) + ISNULL(@DEBIT,0) - ISNULL(@CREDIT,0)"
        strSql += vbCrLf + "  		PRINT @RUNTOT"
        strSql += vbCrLf + "  		UPDATE TEMP" & sysid & "ACCLEDGER_SUMMARY SET "
        strSql += vbCrLf + "  		RUNTOT = CASE WHEN @RUNTOT > 0 THEN @RUNTOT ELSE -1*@RUNTOT END"
        strSql += vbCrLf + "  		,RUNTOTAL = CONVERT(VARCHAR,(CASE WHEN @RUNTOT > 0 THEN @RUNTOT ELSE -1*@RUNTOT END)) + CASE WHEN @RUNTOT > 0 THEN ' Dr' ELSE ' Cr' END"
        strSql += vbCrLf + "  		WHERE CURRENT OF RUNCUR"
        strSql += vbCrLf + "  		END"
        strSql += vbCrLf + "  		CLOSE RUNCUR"
        strSql += vbCrLf + "  		DEALLOCATE RUNCUR"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + "  		UPDATE TEMP" & sysid & "ACCLEDGER_SUMMARY SET MONTHNAME='OPENING' WHERE RESULT=1"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strSql += vbCrLf + "  		UPDATE TEMP" & sysid & "ACCLEDGER_SUMMARY SET MONTHNAME='EXCLUDING OPENING' WHERE RESULT=3"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strSql += vbCrLf + "  		UPDATE TEMP" & sysid & "ACCLEDGER_SUMMARY SET MONTHNAME='TOTAL' WHERE RESULT=4"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strSql += vbCrLf + "  		UPDATE TEMP" & sysid & "ACCLEDGER_SUMMARY SET MONTHNAME='BALANCE' WHERE RESULT=5"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strSql = vbCrLf + "  		SELECT "
        strSql += vbCrLf + "  		ACNAME,UPPER(MONTHNAME) AS PARTICULARS,DEBIT,CREDIT,RUNTOTAL,RESULT"
        strSql += vbCrLf + "  		FROM TEMP" & sysid & "ACCLEDGER_SUMMARY ORDER BY ACNAME,RESULT,YEAR,MONTH"
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

        With objContraSummary.gridView.Columns("PARTICULARS")
            .HeaderText = "PARTICULARS"
            '.Width = 410
            .SortMode = DataGridViewColumnSortMode.NotSortable
        End With
        With objContraSummary.gridView.Columns("DEBIT")
            '.Width = 170
            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .SortMode = DataGridViewColumnSortMode.NotSortable
            .DefaultCellStyle.Format = "#,##0.00"
        End With
        With objContraSummary.gridView.Columns("CREDIT")
            '.Width = 170
            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .SortMode = DataGridViewColumnSortMode.NotSortable
            .DefaultCellStyle.Format = "#,##0.00"
        End With
        With objContraSummary.gridView.Columns("RUNTOTAL")
            '.Width = 170
            .HeaderText = "RUNNING BALANCE"
            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .SortMode = DataGridViewColumnSortMode.NotSortable
        End With
        'objContraSummary.Size = New Size(778, 550)
        objContraSummary.gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells


        objContraSummary.gridView.BackgroundColor = objContraSummary.BackColor
        objContraSummary.gridView.ColumnHeadersDefaultCellStyle = gridView_OWN.ColumnHeadersDefaultCellStyle
        objContraSummary.gridView.ColumnHeadersHeightSizeMode = gridView_OWN.ColumnHeadersHeightSizeMode
        objContraSummary.gridView.ColumnHeadersHeight = gridView_OWN.ColumnHeadersHeight
        objContraSummary.Text = gridView_OWN.CurrentRow.Cells("ACNAME").Value.ToString() & " [Ledger Summary View]"
        objContraSummary.lblTitle.Text = lblTitle.Text + "[Ledger Summary View]"
        objContraSummary.lblTitle.Text = lblTitle.Text + Environment.NewLine
        objContraSummary.StartPosition = FormStartPosition.CenterScreen
        objContraSummary.Show()
        LedgerSummaryGridViewFormat(objContraSummary.gridView)
    End Function

    Private Function funcSubLedger()
        Dim ftrTrailbal As String = Nothing
        Dim ftrTran As String = Nothing
        ftrTrailbal = funcTrailBalFiltration()
        ftrTran = funcTranFiltration()

        ProgressBarStep("Retrieving Data", 10)
        strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & sysid & "SUB')>0 DROP TABLE TEMP" & sysid & "SUB"
        strSql += vbCrLf + " DECLARE @FRMDATE SMALLDATETIME"
        strSql += vbCrLf + " DECLARE @TODATE SMALLDATETIME"
        strSql += vbCrLf + " SELECT @FRMDATE = '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " SELECT @TODATE = '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " SELECT *"
        strSql += vbCrLf + " INTO TEMP" & sysid & "SUB"
        strSql += vbCrLf + " FROM"
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " SELECT CONVERT(INT,NULL)TRANNO,CONVERT(SMALLDATETIME,NULL) TRANDATE,CONVERT(VARCHAR(40),NULL) PAYMODE,'OPENING...' as CONTRA,''SUBLEDGER"
        strSql += vbCrLf + " ,CASE WHEN SUM(DEBIT) - SUM(CREDIT) > 0 THEN SUM(DEBIT) - SUM(CREDIT) ELSE 0 END AS DEBIT"
        strSql += vbCrLf + " ,CASE WHEN SUM(CREDIT)- SUM(DEBIT) > 0 THEN SUM(CREDIT)- SUM(DEBIT) ELSE 0 END AS CREDIT"
        strSql += vbCrLf + " ,ACCODE"
        strSql += vbCrLf + " ,CONVERT(VARCHAR(11),NULL)BATCHNO,CONVERT(VARCHAR(40),NULL)REFNO,CONVERT(SMALLDATETIME,NULL) REFDATE,CONVERT(VARCHAR(55),NULL)REMARK1,CONVERT(VARCHAR(55),NULL)REMARK2"
        strSql += vbCrLf + " ,1 RESULT"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),NULL)RUNTOT,CONVERT(VARCHAR(1),NULL)CUR "
        strSql += vbCrLf + " ,NULL CHQCARDNO,NULL CARDID,NULL CHQCARDREF,CONVERT(VARCHAR(10),NULL) CHQDATE"
        strSql += vbCrLf + " ,NULL PCS"
        strSql += vbCrLf + " ,NULL GRSWT,NULL NETWT,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = X.ACCODE)AS ACNAME,CONVERT(VARCHAR(2),NULL)FROMFLAG,CONVERT(VARCHAR(3),NULL)COSTID,COMPANYID"
        strSql += vbCrLf + " ,CONVERT(VARCHAR(25),NULL)UPDATED,CONVERT(VARCHAR(50),NULL)USERNAME,CONVERT(VARCHAR(50),NULL)DISCEMPNAME,CONVERT(VARCHAR(50),NULL)EMPNAME"
        strSql += vbCrLf + " FROM "
        strSql += vbCrLf + " 	("
        strSql += vbCrLf + " 	SELECT 'TRI_OPEN'SEP,' 'TRANNO,@FRMDATE TRANDATE"
        strSql += vbCrLf + " 	,(SELECT TOP 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.ACCODE)AS ACNAME"
        strSql += vbCrLf + "    ,'' SUBLEDGER"
        strSql += vbCrLf + " 	,SUM(DEBIT)DEBIT"
        strSql += vbCrLf + " 	,SUM(CREDIT)CREDIT"
        strSql += vbCrLf + " 	,ACCODE,NULL PCS,NULL GRSWT,NULL NETWT,COSTID,COMPANYID"
        strSql += vbCrLf + " 	FROM " & cnStockDb & "..OPENTRAILBALANCE T"
        strSql += ftrTrailbal
        strSql += vbCrLf + " 	GROUP BY ACCODE,COSTID,COMPANYID"
        strSql += vbCrLf + " 	UNION ALL"
        strSql += vbCrLf + " 	SELECT 'ACC_OPEN'SEP,CONVERT(VARCHAR,TRANNO)TRANNO,TRANDATE"
        strSql += vbCrLf + " 	,(SELECT TOP 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.ACCODE) AS ACNAME"
        'strSql += vbCrLf + "  ,(SELECT TOP 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.SACCODE)SUBLEDGER"
        strSql += vbCrLf + "   ,'' SUBLEDGER"
        strSql += vbCrLf + " 	,ISNULL(SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT END),0) AS DEBIT"
        strSql += vbCrLf + " 	,ISNULL(SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT END),0) AS CREDIT"
        strSql += vbCrLf + " 	,ACCODE,PCS"
        strSql += vbCrLf + " 	,GRSWT,NETWT"
        strSql += vbCrLf + " 	,COSTID,COMPANYID"
        strSql += vbCrLf + " 	FROM " & cnStockDb & "..ACCTRAN AS T"
        strSql += vbCrLf + " 	WHERE TRANDATE < @FRMDATE"
        strSql += ftrTran
        strSql += vbCrLf + " 	GROUP BY TRANNO,TRANDATE,ACCODE,SACCODE,PCS,GRSWT,NETWT,COSTID,COMPANYID"
        strSql += vbCrLf + " 	)X "
        strSql += vbCrLf + " GROUP BY ACCODE,COMPANYID"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT CONVERT(VARCHAR,TRANNO)TRANNO,TRANDATE,PAYMODE"
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
        strSql += vbCrLf + " ,(SELECT DISCEMPNAME FROM " & cnAdminDb & "..EMPMASTER WHERE EMPID = T.DISC_EMPID)AS DISCEMPNAME"
        strSql += vbCrLf + " ,(SELECT EMPNAME FROM " & cnAdminDb & "..EMPMASTER WHERE EMPID = T.DISC_EMPID)AS EMPNAME"
        strSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN AS T"
        strSql += vbCrLf + " WHERE TRANDATE BETWEEN @FRMDATE AND @TODATE"
        strSql += ftrTran
        strSql += vbCrLf + " )X"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        ProgressBarStep("Calculating Total", 10)
        strSql = vbCrLf + " SELECT "
        strSql += vbCrLf + " (CASE WHEN ISNULL(SUBLEDGER,'')<>'' THEN SUBLEDGER ELSE '[NO SUB LEDGER]' END)ACNAME,SUBLEDGER"
        strSql += vbCrLf + " ,CASE WHEN AMOUNT > 0 THEN AMOUNT ELSE null END AS DEBIT"
        strSql += vbCrLf + " ,CASE WHEN AMOUNT < 0 THEN -1*AMOUNT ELSE null END AS CREDIT"
        strSql += vbCrLf + " FROM"
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " SELECT ACNAME,SUBLEDGER,ACCODE,SUM(DEBIT)-SUM(CREDIT) AS AMOUNT FROM TEMP" & sysid & "SUB"
        strSql += vbCrLf + " WHERE RESULT IN(1,2)"
        strSql += vbCrLf + " GROUP BY ACNAME,SUBLEDGER,ACCODE"
        strSql += vbCrLf + " )X"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT 'TOTAL BALANCE'ACNAME,'ZZZZ' SUBLEDGER"
        strSql += vbCrLf + " ,CASE WHEN AMOUNT > 0 THEN AMOUNT ELSE null END AS DEBIT"
        strSql += vbCrLf + " ,CASE WHEN AMOUNT < 0 THEN -1*AMOUNT ELSE null END AS CREDIT"
        strSql += vbCrLf + " FROM"
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " SELECT ACCODE,SUM(DEBIT)-SUM(CREDIT) AS AMOUNT FROM TEMP" & sysid & "SUB"
        strSql += vbCrLf + " WHERE RESULT IN(1,2)"
        strSql += vbCrLf + " GROUP BY ACCODE"
        strSql += vbCrLf + " )X"
        strSql += vbCrLf + " ORDER BY SUBLEDGER"
        da = New OleDbDataAdapter(strSql, cn)
        Dim dtSubLedger As DataTable
        dtSubLedger = New DataTable
        da.Fill(dtSubLedger)
        If dtSubLedger.Rows.Count > 0 Then
            DataGridView1.DataSource = Nothing
            DataGridView1.DataSource = dtSubLedger
            lblTitle.Font = New Font("VERDANA", 9, FontStyle.Bold)
            lblTitle.Text = "Sub Ledgers Summary "
            DataGridView1.Visible = True
            DataGridView1.Columns("SUBLEDGER").Visible = False
            DataGridView1.Columns("CREDIT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            DataGridView1.Columns("DEBIT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            DataGridView1.Columns("DEBIT").DefaultCellStyle.Format = "#,##0.00"
            DataGridView1.Columns("CREDIT").DefaultCellStyle.Format = "#,##0.00"
            DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            For Each col As DataGridViewColumn In DataGridView1.Columns
                col.SortMode = DataGridViewColumnSortMode.NotSortable
            Next
            For Each row As DataGridViewRow In DataGridView1.Rows
                With row
                    Select Case .Cells("SUBLEDGER").Value.ToString
                        Case "ZZZZ"
                            .DefaultCellStyle.BackColor = Color.LightGoldenrodYellow
                            .DefaultCellStyle.ForeColor = reportSubTotalStyle1.ForeColor
                            .DefaultCellStyle.Font = reportSubTotalStyle1.Font
                        Case ""
                            .DefaultCellStyle.BackColor = Color.LightGoldenrodYellow
                    End Select
                End With
            Next
            DataGridView1.ColumnHeadersDefaultCellStyle.Font = reportSubTotalStyle1.Font
            'DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
            'DataGridView1.Invalidate()
            'For Each dgvCol As DataGridViewColumn In DataGridView1.Columns
            '    dgvCol.Width = dgvCol.Width
            'Next
            'DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            ProgressBarStep("", 10)
        End If
    End Function

    Function funcAccLedger() As Integer
        Dim ftrTrailbal As String = Nothing
        Dim ftrTran As String = Nothing
        ftrTrailbal = funcTrailBalFiltration()
        ftrTran = funcTranFiltration()
        ProgressBarStep("Retrieving Data", 10)
        If Rpt_LView_Sledger = False Then
            'strSql = " IF (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & sysid & "ACC')>0 DROP TABLE TEMPTABLEDB..TEMP" & sysid & "ACC"
            strSql = "IF OBJECT_ID('TEMPTABLEDB..TEMP" & sysid & "ACC','U') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP" & sysid & "ACC"
            strSql += vbCrLf + " DECLARE @FRMDATE SMALLDATETIME"
            strSql += vbCrLf + " DECLARE @TODATE SMALLDATETIME"
            strSql += vbCrLf + " SELECT @FRMDATE = '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " SELECT @TODATE = '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " SELECT *"
            strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & sysid & "ACC"
            strSql += vbCrLf + " FROM"
            strSql += vbCrLf + " ("
            strSql += vbCrLf + " SELECT CONVERT(INT,NULL)TRANNO,CONVERT(SMALLDATETIME,NULL) TRANDATE,CONVERT(VARCHAR(40),NULL) PAYMODE,'OPENING...' as CONTRA,''SUBLEDGER"
            strSql += vbCrLf + " ,CASE WHEN SUM(DEBIT) - SUM(CREDIT) > 0 THEN SUM(DEBIT) - SUM(CREDIT) ELSE 0 END AS DEBIT"
            strSql += vbCrLf + " ,CASE WHEN SUM(CREDIT)- SUM(DEBIT) > 0 THEN SUM(CREDIT)- SUM(DEBIT) ELSE 0 END AS CREDIT"
            strSql += vbCrLf + " ,ACCODE"
            strSql += vbCrLf + " ,CONVERT(VARCHAR(11),NULL)BATCHNO,CONVERT(VARCHAR(40),NULL)REFNO,CONVERT(SMALLDATETIME,NULL) REFDATE,CONVERT(VARCHAR(55),NULL)REMARK1,CONVERT(VARCHAR(55),NULL)REMARK2"
            strSql += vbCrLf + " ,1 RESULT"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),NULL)RUNTOT,CONVERT(VARCHAR(1),NULL)CUR "
            strSql += vbCrLf + " ,NULL CHQCARDNO,NULL CARDID,NULL CHQCARDREF,CONVERT(VARCHAR(10),NULL) CHQDATE"
            strSql += vbCrLf + " ,NULL PCS"
            strSql += vbCrLf + " ,NULL GRSWT,NULL NETWT,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = X.ACCODE)AS ACNAME,CONVERT(VARCHAR(2),NULL)FROMFLAG,CONVERT(VARCHAR(3),NULL)COSTID,COMPANYID"
            strSql += vbCrLf + " ,CONVERT(VARCHAR(25),NULL)UPDATED,CONVERT(VARCHAR(50),NULL)USERNAME,CONVERT(VARCHAR(50),NULL)DISCEMPNAME,CONVERT(VARCHAR(50),NULL)EMPNAME"
            strSql += vbCrLf + " ,CAST(NULL AS INT)AGE"
            strSql += vbCrLf + " ,(SELECT TOP 1 PAN FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = X.CONTRA)AS PANNO"
            strSql += vbCrLf + " ,(SELECT TOP 1 GSTNO FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = X.CONTRA)AS GSTNO"
            'strSql += vbCrLf + " ,(SELECT ACGRPNAME FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPCODE=(SELECT TOP 1 ACGRPCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = X.ACCODE))AS ACGRPNAME"
            strSql += vbCrLf + " ,FLAG FROM "
            strSql += vbCrLf + " 	("
            strSql += vbCrLf + " 	SELECT 'TRI_OPEN'SEP,' 'TRANNO,@FRMDATE TRANDATE"
            strSql += vbCrLf + " 	,(SELECT TOP 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.ACCODE)AS ACNAME"
            strSql += vbCrLf + "    ,'' SUBLEDGER"
            strSql += vbCrLf + " 	,SUM(DEBIT)DEBIT"
            strSql += vbCrLf + " 	,SUM(CREDIT)CREDIT"
            strSql += vbCrLf + " 	,ACCODE,NULL PCS,NULL GRSWT,NULL NETWT,COSTID,COMPANYID"
            strSql += vbCrLf + " 	,'' FLAG,'' CONTRA FROM " & cnStockDb & "..OPENTRAILBALANCE T"
            strSql += ftrTrailbal
            strSql += vbCrLf + " 	GROUP BY ACCODE,COSTID,COMPANYID"
            strSql += vbCrLf + " 	UNION ALL"
            strSql += vbCrLf + " 	SELECT 'ACC_OPEN'SEP,CONVERT(VARCHAR,TRANNO)TRANNO,TRANDATE"
            strSql += vbCrLf + " 	,(SELECT TOP 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.ACCODE) AS ACNAME"
            'strSql += vbCrLf + "  ,(SELECT TOP 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.SACCODE)SUBLEDGER"
            strSql += vbCrLf + "   ,'' SUBLEDGER"
            strSql += vbCrLf + " 	,ISNULL(SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT END),0) AS DEBIT"
            strSql += vbCrLf + " 	,ISNULL(SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT END),0) AS CREDIT"
            strSql += vbCrLf + " 	,ACCODE,PCS"
            'strSql += vbCrLf + " 	,ISNULL(SUM(CASE WHEN TRANMODE = 'D' THEN GRSWT END),0) AS DEBITWTG"
            'strSql += vbCrLf + " 	,ISNULL(SUM(CASE WHEN TRANMODE = 'C' THEN GRSWT END),0) AS CREDITWTG"
            'strSql += vbCrLf + " 	,ISNULL(SUM(CASE WHEN TRANMODE = 'D' THEN NETWT END),0) AS DEBITWTN"
            'strSql += vbCrLf + " 	,ISNULL(SUM(CASE WHEN TRANMODE = 'C' THEN NETWT END),0) AS CREDITWTN"
            strSql += vbCrLf + " 	,GRSWT,NETWT"
            strSql += vbCrLf + " 	,COSTID,COMPANYID"
            strSql += vbCrLf + " 	,'' FLAG,'' CONTRA FROM " & cnStockDb & "..ACCTRAN AS T"
            strSql += vbCrLf + " 	WHERE TRANDATE < @FRMDATE"
            strSql += ftrTran
            strSql += vbCrLf + " 	GROUP BY TRANNO,TRANDATE,ACCODE,SACCODE,PCS,GRSWT,NETWT,COSTID,COMPANYID"
            strSql += vbCrLf + " 	)X "
            strSql += vbCrLf + " GROUP BY ACCODE,COMPANYID,FLAG,CONTRA"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT TRANNO,TRANDATE,PAYMODE"
            If Rpt_LView And _Accode = "CASH" Then
                strSql += vbCrLf + " ,CASE WHEN FROMFLAG='P' AND ISNULL(SALES,'')='Y' THEN 'Sales BillNo:' + CONVERT(VARCHAR,TRANNO) ELSE CONTRA END AS CONTRA"
            Else
                strSql += vbCrLf + " , CONTRA"
            End If
            strSql += vbCrLf + " ,SUBLEDGER,DEBIT,CREDIT,ACCODE,BATCHNO,REFNO,REFDATE,REMARK1,REMARK2,2 RESULT"
            strSql += vbCrLf + " ,0 RUNTOT,' 'CUR,CHQCARDNO,CARDID,CHQCARDREF,CHQDATE"
            strSql += vbCrLf + " ,PCS,GRSWT,NETWT,ACNAME,FROMFLAG,COSTID,COMPANYID,UPDATED,USERNAME,DISCEMPNAME,(SELECT EMPNAME FROM " & cnAdminDb & "..EMPMASTER WHERE EMPID = A.EMPNAME)EMPNAME "

            strSql += vbCrLf + " ,DATEDIFF(DD,TRANDATE,@TODATE)AS AGE"
            strSql += vbCrLf + " ,(SELECT TOP 1 PAN FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = A.CONTRA)AS PANNO"
            strSql += vbCrLf + " ,(SELECT TOP 1 GSTNO FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = A.CONTRA)AS GSTNO"
            strSql += vbCrLf + " ,FLAG FROM("
            strSql += vbCrLf + " SELECT CONVERT(VARCHAR,TRANNO)TRANNO,TRANDATE,PAYMODE"
            strSql += vbCrLf + " ,(SELECT TOP 1 'Y' FROM " & cnStockDb & "..ISSUE I WHERE I.BATCHNO = T.BATCHNO)SALES"
            strSql += vbCrLf + " ,(SELECT TOP 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.CONTRA)CONTRA"
            strSql += vbCrLf + " ,(SELECT TOP 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.SACCODE)SUBLEDGER"
            strSql += vbCrLf + " ,ISNULL(CASE WHEN TRANMODE = 'D' THEN AMOUNT END,0) AS DEBIT"
            strSql += vbCrLf + " ,ISNULL(CASE WHEN TRANMODE = 'C' THEN AMOUNT END,0) AS CREDIT"
            strSql += vbCrLf + " ,ACCODE,BATCHNO"
            strSql += vbCrLf + " ,CASE WHEN PAYMODE='DU' THEN (SELECT TOP 1 SUBSTRING(RUNNO,6,15) FROM " & cnAdminDb & "..OUTSTANDING WHERE BATCHNO = T.BATCHNO) ELSE REFNO END REFNO"
            strSql += vbCrLf + " ,REFDATE,REMARK1,REMARK2"
            strSql += vbCrLf + " ,2 RESULT"
            strSql += vbCrLf + " ,0 RUNTOT,' 'CUR "
            strSql += vbCrLf + " ,CHQCARDNO,CARDID,CHQCARDREF,CONVERT(VARCHAR,CHQDATE,103)CHQDATE"
            strSql += vbCrLf + " ,PCS,GRSWT,NETWT,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.ACCODE)AS ACNAME,FROMFLAG,COSTID,COMPANYID"
            strSql += vbCrLf + " ,CONVERT(VARCHAR(25),CONVERT(VARCHAR,UPDATED,103)+ ' ' + CONVERT(VARCHAR,UPTIME,108))UPDATED"
            strSql += vbCrLf + " ,'['+CONVERT(VARCHAR,USERID)+']'+ (SELECT USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERID = T.USERID)AS USERNAME"
            strSql += vbCrLf + " ,(SELECT EMPNAME FROM " & cnAdminDb & "..EMPMASTER WHERE EMPID = T.DISC_EMPID)AS DISCEMPNAME"
            strSql += vbCrLf + " ,(SELECT TOP 1 EMPID FROM " & cnStockDb & "..ISSUE I WHERE I.BATCHNO = T.BATCHNO)AS EMPNAME"
            ''strSql += vbCrLf + " ,(SELECT EMPNAME FROM " & cnAdminDb & "..EMPMASTER WHERE EMPID = T.DISC_EMPID)AS DISCEMPNAME"
            'strSql += vbCrLf + " ,(SELECT ACGRPNAME FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPCODE=(SELECT TOP 1 ACGRPCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.ACCODE))AS ACGRPNAME"
            strSql += vbCrLf + " ,FLAG FROM " & cnStockDb & "..ACCTRAN AS T"
            strSql += vbCrLf + " WHERE TRANDATE BETWEEN @FRMDATE AND @TODATE"
            strSql += ftrTran
            strSql += vbCrLf + " )A"
            strSql += vbCrLf + " )X"
            'strSql += vbCrLf + " ORDER BY ACCODE,TRANDATE,TRANNO"
        Else
            Dim _Acc As String = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME='" & cmbAcName.Text & "'", "ACCODE", "").ToString
            Dim _subLedger As Boolean
            strSql = "IF OBJECT_ID('TEMPTABLEDB..TEMP" & systemId & "ACCTRAN','U') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP" & systemId & "ACCTRAN"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
            strSql = vbCrLf + "SELECT TRANNO,TRANDATE,PAYMODE,BATCHNO,TRANMODE,ACCODE,SACCODE,AMOUNT,CONTRA "
            strSql += vbCrLf + ",REFNO,REFDATE,REMARK1,REMARK2,CANCEL"
            strSql += vbCrLf + ",FROMFLAG,COSTID,COMPANYID,PCS,GRSWT,NETWT"
            strSql += vbCrLf + ",CHQCARDNO,CARDID,CHQCARDREF,CHQDATE,UPDATED,UPTIME,USERID,DISC_EMPID"
            strSql += vbCrLf + ",FLAG INTO TEMPTABLEDB..TEMP" & systemId & "ACCTRAN"
            strSql += vbCrLf + "FROM " & cnStockDb & "..ACCTRAN WHERE TRANDATE BETWEEN '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' "
            strSql += vbCrLf + "AND '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "' "
            strSql += vbCrLf + "AND ISNULL(SACCODE,'')<>'' "
            strSql += vbCrLf + "AND CONTRA='" & _Acc & "'"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
            strSql = "SELECT * FROM TEMPTABLEDB..TEMP" & systemId & "ACCTRAN"
            da = New OleDbDataAdapter(strSql, cn)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 And chkMultiSelect.Checked = False And cmbAcName.Text <> "" And cmbAcName.Text <> "" Then
                _subLedger = True
                strSql = vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "ACCTRAN"
                strSql += vbCrLf + "SELECT TRANNO,TRANDATE,PAYMODE,BATCHNO,CASE WHEN TRANMODE='D' THEN 'C' ELSE 'D' END"
                strSql += vbCrLf + ",CONTRA,'',AMOUNT,SACCODE "
                strSql += vbCrLf + ",REFNO,REFDATE,REMARK1,REMARK2,CANCEL"
                strSql += vbCrLf + ",FROMFLAG,COSTID,COMPANYID,PCS,GRSWT,NETWT"
                strSql += vbCrLf + ",CHQCARDNO,CARDID,CHQCARDREF,CHQDATE,UPDATED,UPTIME,USERID,DISC_EMPID"
                strSql += vbCrLf + ",FLAG FROM " & cnStockDb & "..ACCTRAN WHERE TRANDATE BETWEEN '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' "
                strSql += vbCrLf + "AND '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "' "
                strSql += vbCrLf + "AND ISNULL(SACCODE,'')<>'' "
                strSql += vbCrLf + "AND CONTRA='" & _Acc & "'"
                cmd = New OleDbCommand(strSql, cn)
                cmd.ExecuteNonQuery()
            End If

            'strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMPTABLEDB..TEMP" & sysid & "ACC')>0 DROP TABLE TEMPTABLEDB..TEMP" & sysid & "ACC"
            strSql = "IF OBJECT_ID('TEMPTABLEDB..TEMP" & sysid & "ACC','U') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP" & sysid & "ACC"
            strSql += vbCrLf + " DECLARE @FRMDATE SMALLDATETIME"
            strSql += vbCrLf + " DECLARE @TODATE SMALLDATETIME"
            strSql += vbCrLf + " SELECT @FRMDATE = '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " SELECT @TODATE = '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " SELECT *"
            strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & sysid & "ACC"
            strSql += vbCrLf + " FROM"
            strSql += vbCrLf + " ("
            strSql += vbCrLf + " SELECT CONVERT(INT,NULL)TRANNO,CONVERT(SMALLDATETIME,NULL) TRANDATE,CONVERT(VARCHAR(40),NULL) PAYMODE,'OPENING...' as CONTRA,''SUBLEDGER"
            strSql += vbCrLf + " ,CASE WHEN SUM(DEBIT) - SUM(CREDIT) > 0 THEN SUM(DEBIT) - SUM(CREDIT) ELSE 0 END AS DEBIT"
            strSql += vbCrLf + " ,CASE WHEN SUM(CREDIT)- SUM(DEBIT) > 0 THEN SUM(CREDIT)- SUM(DEBIT) ELSE 0 END AS CREDIT"
            strSql += vbCrLf + " ,ACCODE"
            strSql += vbCrLf + " ,CONVERT(VARCHAR(11),NULL)BATCHNO,CONVERT(VARCHAR(40),NULL)REFNO,CONVERT(SMALLDATETIME,NULL) REFDATE,CONVERT(VARCHAR(55),NULL)REMARK1,CONVERT(VARCHAR(55),NULL)REMARK2"
            strSql += vbCrLf + " ,1 RESULT"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),NULL)RUNTOT,CONVERT(VARCHAR(1),NULL)CUR "
            strSql += vbCrLf + " ,NULL CHQCARDNO,NULL CARDID,NULL CHQCARDREF,CONVERT(VARCHAR(10),NULL) CHQDATE"
            strSql += vbCrLf + " ,NULL PCS"
            strSql += vbCrLf + " ,NULL GRSWT,NULL NETWT,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = X.ACCODE)AS ACNAME,CONVERT(VARCHAR(2),NULL)FROMFLAG,CONVERT(VARCHAR(3),NULL)COSTID,COMPANYID"
            strSql += vbCrLf + " ,CONVERT(VARCHAR(25),NULL)UPDATED,CONVERT(VARCHAR(50),NULL)USERNAME,CONVERT(VARCHAR(50),NULL)DISCEMPNAME,CONVERT(VARCHAR(50),NULL)EMPNAME"
            strSql += vbCrLf + " ,CAST(NULL AS INT)AGE"
            strSql += vbCrLf + " ,(SELECT TOP 1 PAN FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = X.CONTRA)AS PANNO"
            strSql += vbCrLf + " ,(SELECT TOP 1 GSTNO FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = X.CONTRA)AS GSTNO"
            strSql += vbCrLf + " ,FLAG FROM "
            strSql += vbCrLf + " 	("
            strSql += vbCrLf + " 	SELECT 'TRI_OPEN'SEP,' 'TRANNO,@FRMDATE TRANDATE"
            strSql += vbCrLf + " 	,(SELECT TOP 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.ACCODE)AS ACNAME"
            strSql += vbCrLf + "    ,'' SUBLEDGER"
            strSql += vbCrLf + " 	,SUM(DEBIT)DEBIT"
            strSql += vbCrLf + " 	,SUM(CREDIT)CREDIT"
            strSql += vbCrLf + " 	,ACCODE,NULL PCS,NULL GRSWT,NULL NETWT,COSTID,COMPANYID"
            strSql += vbCrLf + " 	,''FLAG,'' CONTRA FROM " & cnStockDb & "..OPENTRAILBALANCE T"
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
            strSql += vbCrLf + " 	,COSTID,COMPANYID"
            strSql += vbCrLf + " 	,FLAG,'' CONTRA FROM " & cnStockDb & "..ACCTRAN AS T"
            strSql += vbCrLf + " 	WHERE TRANDATE < @FRMDATE"
            strSql += ftrTran
            strSql += vbCrLf + " 	GROUP BY TRANNO,TRANDATE,ACCODE,SACCODE,PCS,GRSWT,NETWT,COSTID,COMPANYID,FLAG"
            strSql += vbCrLf + " 	)X "
            strSql += vbCrLf + " GROUP BY ACCODE,COMPANYID,FLAG,CONTRA "
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT TRANNO,TRANDATE,PAYMODE"
            If Rpt_LView And _Accode = "CASH" Then
                strSql += vbCrLf + " ,CASE WHEN FROMFLAG='P' AND ISNULL(SALES,'')='Y' THEN 'Sales BillNo:' + CONVERT(VARCHAR,TRANNO) ELSE CONTRA END AS CONTRA"
            Else
                strSql += vbCrLf + " , CONTRA"
            End If
            strSql += vbCrLf + " ,SUBLEDGER,DEBIT,CREDIT,ACCODE,BATCHNO,REFNO,REFDATE,REMARK1,REMARK2,2 RESULT"
            strSql += vbCrLf + " ,0 RUNTOT,' 'CUR,CHQCARDNO,CARDID,CHQCARDREF,CHQDATE"
            strSql += vbCrLf + " ,PCS,GRSWT,NETWT,ACNAME,FROMFLAG,COSTID,COMPANYID,UPDATED,USERNAME,DISCEMPNAME,(SELECT EMPNAME FROM " & cnAdminDb & "..EMPMASTER WHERE EMPID = A.EMPNAME)EMPNAME  "
            strSql += vbCrLf + " ,DATEDIFF(DD,TRANDATE,@TODATE)AS AGE "
            strSql += vbCrLf + " ,(SELECT TOP 1 PAN FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = A.CONTRA)AS PANNO"
            strSql += vbCrLf + " ,(SELECT TOP 1 GSTNO FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = A.CONTRA)AS GSTNO"
            strSql += vbCrLf + " ,FLAG FROM("
            strSql += vbCrLf + " SELECT CONVERT(VARCHAR,TRANNO)TRANNO,TRANDATE,PAYMODE"
            strSql += vbCrLf + " ,(SELECT TOP 1 'Y' FROM " & cnStockDb & "..ISSUE I WHERE I.BATCHNO = T.BATCHNO)SALES"
            strSql += vbCrLf + " ,(SELECT TOP 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.CONTRA)CONTRA"
            strSql += vbCrLf + " ,(SELECT TOP 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.SACCODE)SUBLEDGER"
            strSql += vbCrLf + " ,ISNULL(CASE WHEN TRANMODE = 'D' THEN AMOUNT END,0) AS DEBIT"
            strSql += vbCrLf + " ,ISNULL(CASE WHEN TRANMODE = 'C' THEN AMOUNT END,0) AS CREDIT"
            strSql += vbCrLf + " ,ACCODE,BATCHNO"
            strSql += vbCrLf + " ,CASE WHEN PAYMODE='DU' THEN (SELECT TOP 1 SUBSTRING(RUNNO,6,15) FROM " & cnAdminDb & "..OUTSTANDING WHERE BATCHNO = T.BATCHNO) ELSE REFNO END REFNO"
            strSql += vbCrLf + " ,REFDATE,REMARK1,REMARK2"
            strSql += vbCrLf + " ,2 RESULT"
            strSql += vbCrLf + " ,0 RUNTOT,' 'CUR "
            strSql += vbCrLf + " ,CHQCARDNO,CARDID,CHQCARDREF,CONVERT(VARCHAR,CHQDATE,103)CHQDATE"
            strSql += vbCrLf + " ,PCS,GRSWT,NETWT,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.ACCODE)AS ACNAME,FROMFLAG,COSTID,COMPANYID"
            strSql += vbCrLf + " ,CONVERT(VARCHAR(25),CONVERT(VARCHAR,UPDATED,103)+ ' ' + CONVERT(VARCHAR,UPTIME,108))UPDATED"
            strSql += vbCrLf + " ,'['+CONVERT(VARCHAR,USERID)+']'+ (SELECT USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERID = T.USERID)AS USERNAME"
            strSql += vbCrLf + " ,(SELECT EMPNAME FROM " & cnAdminDb & "..EMPMASTER WHERE EMPID = T.DISC_EMPID)AS DISCEMPNAME"
            strSql += vbCrLf + " ,(SELECT TOP 1 EMPID FROM " & cnStockDb & "..ISSUE I WHERE I.BATCHNO = T.BATCHNO)AS EMPNAME"
            'strSql += vbCrLf + " ,(SELECT EMPNAME FROM " & cnAdminDb & "..EMPMASTER WHERE EMPID = T.DISC_EMPID)AS EMPNAME"
            If _subLedger Then
                strSql += vbCrLf + " ,FLAG FROM TEMPTABLEDB..TEMP" & systemId & "ACCTRAN AS T"
            Else
                strSql += vbCrLf + " ,FLAG FROM " & cnStockDb & "..ACCTRAN AS T"
            End If
            strSql += vbCrLf + " WHERE TRANDATE BETWEEN @FRMDATE AND @TODATE"
            strSql += ftrTran
            strSql += vbCrLf + " )A"
            strSql += vbCrLf + " )X"
        End If

        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        ProgressBarStep("Calculating Total", 10)
        'strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMPTABLEDB..TEMP" & sysid & "ACCTOTAL')>0 DROP TABLE TEMPTABLEDB..TEMP" & sysid & "ACCTOTAL"
        strSql = "IF OBJECT_ID('TEMPTABLEDB..TEMP" & sysid & "ACCTOTAL','U') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP" & sysid & "ACCTOTAL"
        strSql += vbCrLf + "  SELECT 3 RESULT,ACCODE,'EXCLUDING OPENING'CONTRA,SUM(DEBIT)DEBIT,SUM(CREDIT)CREDIT"
        'strSql += vbCrLf + " ,(SELECT ACGRPNAME FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPCODE=(SELECT TOP 1 ACGRPCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = ACCODE))AS ACGRPNAME"
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & sysid & "ACCTOTAL"
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & sysid & "ACC"
        strSql += vbCrLf + "  WHERE RESULT = 2"
        strSql += vbCrLf + "  GROUP BY ACCODE"
        strSql += vbCrLf + "  UNION ALL"
        strSql += vbCrLf + "  SELECT 4 RESULT,ACCODE,'TOTAL'CONTRA,SUM(DEBIT)DEBIT,SUM(CREDIT)CREDIT "
        'strSql += vbCrLf + " ,(SELECT ACGRPNAME FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPCODE=(SELECT TOP 1 ACGRPCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = ACCODE))AS ACGRPNAME"
        strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & sysid & "ACC"
        strSql += vbCrLf + "  WHERE RESULT IN(1,2)"
        strSql += vbCrLf + "  GROUP BY ACCODE"
        strSql += vbCrLf + "  UNION ALL"
        strSql += vbCrLf + "  SELECT 5 RESULT,ACCODE,'BALANCE'CONTRA"
        strSql += vbCrLf + "  ,CASE WHEN AMOUNT > 0 THEN AMOUNT ELSE null END AS DEBIT"
        strSql += vbCrLf + "  ,CASE WHEN AMOUNT < 0 THEN -1*AMOUNT ELSE null END AS CREDIT"
        'strSql += vbCrLf + " ,(SELECT ACGRPNAME FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPCODE=(SELECT TOP 1 ACGRPCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = X.ACCODE))AS ACGRPNAME"
        strSql += vbCrLf + "  	FROM"
        strSql += vbCrLf + "  	("
        strSql += vbCrLf + "  	SELECT ACCODE,SUM(DEBIT)-SUM(CREDIT) AS AMOUNT FROM TEMPTABLEDB..TEMP" & sysid & "ACC"
        strSql += vbCrLf + "  	WHERE RESULT IN(1,2)"
        strSql += vbCrLf + "  	GROUP BY ACCODE"
        strSql += vbCrLf + "  )X"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        ProgressBarStep("", 10)
        strSql = " SELECT * FROM TEMPTABLEDB..TEMP" & sysid & "ACCTOTAL"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtViewTotal)
        ProgressBarStep("", 10)
        strSql = " INSERT INTO TEMPTABLEDB..TEMP" & sysid & "ACC(RESULT,ACCODE,CONTRA,DEBIT,CREDIT,ACNAME)"
        If chkMultiSelect.Checked Then ' If chkCmbAcName.Text = "ALL" Then
            strSql += vbCrLf + " SELECT DISTINCT 0 RESULT,ACCODE"
            strSql += vbCrLf + " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.ACCODE)"
            strSql += vbCrLf + " ,0,0"
            strSql += vbCrLf + " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.ACCODE)"
            'strSql += vbCrLf + " ,ACGRPNAME "
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & sysid & "ACC T"
            strSql += vbCrLf + "  UNION ALL"
        End If
        strSql += vbCrLf + " SELECT * "
        strSql += vbCrLf + " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.ACCODE)"
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & sysid & "ACCTOTAL T"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        If Rpt_LView_ZEROAC = False Then
            strSql = "DELETE FROM TEMPTABLEDB..TEMP" & sysid & "ACC WHERE ACCODE "
            strSql += vbCrLf + "IN (SELECT ACCODE  FROM TEMPTABLEDB..TEMP" & sysid & "ACC T GROUP BY ACCODE"
            strSql += vbCrLf + "HAVING SUM(CREDIT)=0 AND SUM(DEBIT)=0)"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End If

        ProgressBarStep("Generate Ledger Table", 10)
        'strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMPTABLEDB..TEMP" & sysid & "ACCLEDGER')>0 DROP TABLE TEMPTABLEDB..TEMP" & sysid & "ACCLEDGER"
        strSql = "IF OBJECT_ID('TEMPTABLEDB..TEMP" & sysid & "ACCLEDGER','U') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP" & sysid & "ACCLEDGER"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strSql = " CREATE TABLE TEMPTABLEDB..TEMP" & sysid & "ACCLEDGER"
        strSql += vbCrLf + " (TRANNO INT,TRANDATE SMALLDATETIME,PAYMODE VARCHAR (2),CONTRA VARCHAR (1000)"
        strSql += vbCrLf + " ,DEBIT NUMERIC (20,2),CREDIT NUMERIC (20,2),ACCODE VARCHAR (7)"
        strSql += vbCrLf + " ,BATCHNO VARCHAR (15),REFNO VARCHAR (40),REFDATE SMALLDATETIME,REMARK1 VARCHAR (200)"
        strSql += vbCrLf + " ,REMARK2 VARCHAR (200),RESULT INT,RUNTOTAL VARCHAR(20),RUNTOT NUMERIC (20,2),CUR VARCHAR (1)"
        strSql += vbCrLf + " ,CHQCARDNO VARCHAR(30),CARDID INT,CHQCARDREF VARCHAR(100),CHQDATE VARCHAR(10)"
        strSql += vbCrLf + " ,PCS INT,GRSWT NUMERIC(15,3),NETWT NUMERIC(15,3),ACNAME VARCHAR(55),SUBLEDGER VARCHAR(55)"
        strSql += vbCrLf + " ,SNO INT IDENTITY(1,1),FROMFLAG VARCHAR(2),COSTID VARCHAR(3),UPDATED VARCHAR(25)"
        strSql += vbCrLf + " ,COSTNAME VARCHAR(50),USERNAME VARCHAR(50),DISCEMPNAME VARCHAR(50),EMPNAME VARCHAR(50),COMPANYID VARCHAR(50)"
        strSql += vbCrLf + " ,COMPANYNAME VARCHAR(500),AGE INT,FLAG VARCHAR(2),PANNO VARCHAR(50),GSTNO VARCHAR(50))"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strSql = " INSERT INTO TEMPTABLEDB..TEMP" & sysid & "ACCLEDGER"
        strSql += vbCrLf + " (TRANNO,TRANDATE,PAYMODE,CONTRA,DEBIT,CREDIT,ACCODE,BATCHNO,REFNO"
        strSql += vbCrLf + " ,REFDATE,REMARK1,REMARK2,RESULT,RUNTOT,CUR,CHQCARDNO,CARDID,CHQCARDREF"
        strSql += vbCrLf + " ,CHQDATE,PCS,GRSWT,NETWT,ACNAME,FROMFLAG,COSTID,UPDATED,COSTNAME,USERNAME,DISCEMPNAME,EMPNAME,COMPANYID"
        strSql += vbCrLf + " ,COMPANYNAME,SUBLEDGER,AGE,FLAG,PANNO,GSTNO)"
        strSql += vbCrLf + " SELECT"
        strSql += vbCrLf + " TRANNO,TRANDATE,PAYMODE"
        If chkMultiSelect.Checked Then
            strSql += vbCrLf + " ,CASE WHEN RESULT=0 THEN CONTRA + (SELECT '(' + ACGRPNAME + ')' FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPCODE=(SELECT TOP 1 ACGRPCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.ACCODE)) ELSE CONTRA END"
        Else
            strSql += vbCrLf + " ,CONTRA"
        End If
        strSql += vbCrLf + " ,DEBIT,CREDIT,ACCODE,BATCHNO,REFNO"
        strSql += vbCrLf + " ,REFDATE,REMARK1,REMARK2,RESULT,RUNTOT,CUR,CHQCARDNO,CARDID,CHQCARDREF"
        strSql += vbCrLf + " ,CHQDATE,PCS,GRSWT,NETWT,ACNAME,FROMFLAG,T.COSTID,T.UPDATED,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = T.COSTID)AS COSTNAME,USERNAME,DISCEMPNAME,EMPNAME,T.COMPANYID,C.COMPANYNAME,SUBLEDGER,AGE"
        strSql += vbCrLf + " ,FLAG,T.PANNO,T.GSTNO FROM TEMPTABLEDB..TEMP" & sysid & "ACC T"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..COMPANY C ON T.COMPANYID=C.COMPANYID"
        strSql += vbCrLf + " WHERE ISNULL(C.ACTIVE,'')<>'N'"
        If chkOrderbyTranNo.Checked Then
            strSql += vbCrLf + " ORDER BY ACNAME,ACCODE,RESULT,TRANDATE,TRANNO,DEBIT DESC"
        Else
            strSql += vbCrLf + " ORDER BY ACNAME,ACCODE,RESULT,TRANDATE,DEBIT DESC,TRANNO"
        End If
        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        With objMoreOption
            If .rdbWithParticular.Checked = True Then
                If .chkSubledger.Checked Or .chkChequeNo.Checked Or .chkChequeDate.Checked Or .chkBankName.Checked Or .chkRemark1.Checked Or .chkRemark2.Checked Or .chkRefNo.Checked Or .chkRefDate.Checked Or .chkCostName.Checked Or .chkUserName.Checked Or .chkDiscAuthEmp.Checked Then
                    strSql = " UPDATE TEMPTABLEDB..TEMP" & sysid & "ACCLEDGER"
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
                    If .chkDiscAuthEmp.Checked Then
                        strSql += vbCrLf + " + CASE WHEN ISNULL(EMPNAME,'') <> '' THEN CHAR(13) + '  ' + EMPNAME ELSE '' END"
                        strSql += vbCrLf + " + CASE WHEN ISNULL(EMPNAME,'') <> '' THEN CHAR(13) + '  ' + EMPNAME ELSE '' END"
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
        'strSql = " ALTER TABLE TEMP" & sysid & "ACCLEDGER ADD SNO INT IDENTITY"
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
        strSql += vbCrLf + "  DECLARE @SNO NUMERIC(20,2)"
        strSql += vbCrLf + "  DECLARE RUNCUR CURSOR"
        strSql += vbCrLf + "  FOR SELECT DEBIT,CREDIT,ACCODE,SNO FROM TEMPTABLEDB..TEMP" & sysid & "ACCLEDGER WHERE RESULT IN (1,2) ORDER BY SNO"
        strSql += vbCrLf + "  OPEN RUNCUR"
        strSql += vbCrLf + "  WHILE 1=1"
        strSql += vbCrLf + "  BEGIN"
        strSql += vbCrLf + "  	FETCH NEXT FROM RUNCUR INTO @DEBIT,@CREDIT,@ACCODE,@SNO"
        strSql += vbCrLf + "  	IF @@FETCH_STATUS = -1 BREAK"
        strSql += vbCrLf + "  	IF ISNULL(@ACCODE,'') <> ISNULL(@PREACCODE ,'')"
        strSql += vbCrLf + "  	BEGIN"
        strSql += vbCrLf + "  	SELECT @PREACCODE = @ACCODE"
        strSql += vbCrLf + "  	SELECT @RUNTOT = 0"
        strSql += vbCrLf + "  	END"
        strSql += vbCrLf + "  	SELECT @RUNTOT = ISNULL(@RUNTOT,0) + ISNULL(@DEBIT,0) - ISNULL(@CREDIT,0)"
        strSql += vbCrLf + "  	PRINT @RUNTOT"
        strSql += vbCrLf + "  	UPDATE TEMPTABLEDB..TEMP" & sysid & "ACCLEDGER SET "
        strSql += vbCrLf + "  			     RUNTOT = CASE WHEN @RUNTOT > 0 THEN @RUNTOT ELSE -1*@RUNTOT END,"
        strSql += vbCrLf + "  			     CUR    = CASE WHEN @RUNTOT > 0 THEN 'D' ELSE 'C' END"
        'If chkGrpTranNo.Checked Then
        'strSql += vbCrLf + "  ,RUNTOTAL = CASE WHEN @RUNTOT > 0 THEN @RUNTOT ELSE -1*@RUNTOT END"
        ' Else
        strSql += vbCrLf + "                 ,RUNTOTAL = CONVERT(VARCHAR,@RUNTOT) "
        ' End If
        If chkGrpTranNo.Checked = False Then strSql += vbCrLf + "  + CASE WHEN @RUNTOT > 0 THEN ' Dr' ELSE ' Cr' END"
        strSql += vbCrLf + "  			     WHERE SNO=@SNO --CURRENT OF RUNCUR"
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
                If DataGridView1.Visible = True Then
                    DataGridView1.DataSource = Nothing
                    DataGridView1.Visible = False
                    Exit Sub
                End If
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
                    'ResetMoreoptions()
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

    Private Sub ResetMoreoptions()
        objMoreOption = New frmAccLedgerMore
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
        'strSql = " SELECT " & cnAdminDb & ".DBO.WORDCAPS(ACNAME)ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE 1=1"
        'strSql += " ORDER BY ACNAME"
        'da = New OleDbDataAdapter(strSql, cn)
        'da.Fill(dtAccNames)
        'DgvSearch.DataSource = dtAccNames
        'DgvSearch.ColumnHeadersVisible = False
        'DgvSearch.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        'DgvSearch.Visible = False
        If GetAdmindbSoftValue("CANCELLEDGER_VIEW", "N") = "Y" Then chkCancelonly.Visible = True Else chkCancelonly.Visible = False
        btnNew_Click(Me, New EventArgs)
        _Chqformat = Val(GetAdmindbSoftValue("CHQ_FORMAT", "1").ToString)
        If GetAdmindbSoftValue("RPTLEDGER_ADDRESS", "N") = "Y" Then
            lviewDetails.Columns.Add("Name", 120, HorizontalAlignment.Left)
            lviewDetails.Columns.Add("Description", 250, HorizontalAlignment.Left)
            lviewDetails.Visible = True
        Else
            lviewDetails.Visible = False
        End If
        If chkMultiSelect.Checked = True Then
            lviewDetails.Visible = False
        End If
        If Rpt_LView_AccodeSearch Then
            lblAccountCode.Visible = True
            txtAcountCode.Visible = True
            txtAcountCode.Clear()
        Else
            lblAccountCode.Visible = False
            txtAcountCode.Visible = False
            txtAcountCode.Clear()
        End If
    End Sub

    Public Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        Ismaxdrcr = False
        Dim previous As Integer
        Dim page As Integer = 1
        Dim current As Integer
        Dim zeroPage As Integer
        flagAge = False
        sysid = Trim(LSet(Mid(Guid.NewGuid.ToString, 1, InStr(Guid.NewGuid.ToString, "-") - 1), 10))
        funcSummaryGridStyle()
        gridView_OWN.RowTemplate.Height = 18
        gridSummary.BackgroundColor = Me.BackColor
        ' ''Try
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
        'btnView_Search.Enabled = False
        'Me.Cursor = Cursors.WaitCursor
        ProgressBarShow()
        ProgressBarStep("", 10)
        funcAccLedger()
        ProgressBarStep("Fill into Datatable", 10)
        If chkGrpTranNo.Checked Then
            'strSql = " SELECT *"
            'strSql += " FROM TEMP" & sysid & "ACCLEDGER"
            'strSql += " ORDER BY SNO"

            'Dim dtView As New DataTable
            'da = New OleDbDataAdapter(strSql, cn)
            'da.Fill(dtView)
            'If dtView.Rows.Count > 0 Then
            '    dtView.Columns.Contains("RUNTOTAL").ToString()

            'End If
            strSql = " IF (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & sysid & "LEDGER')>0 DROP TABLE TEMPTABLEDB..TEMP" & sysid & "LEDGER"
            strSql += vbCrLf + " SELECT TRANNO,TRANDATE,PAYMODE,CONTRA,DEBIT,CREDIT,ACCODE,'' BATCHNO,REFNO,ISNULL(REFDATE,'')REFDATE,CONVERT(VARCHAR(50),RUNTOTAL)RUNTOTAL"
            strSql += vbCrLf + " ,RUNTOT,PCS,GRSWT,NETWT,CUR,CHQCARDNO,CARDID,ISNULL(CHQDATE,'')CHQDATE,CHQCARDREF,REMARK1,REMARK2,ACNAME,FROMFLAG,COSTID,'' UPDATED,COSTNAME,'' USERNAME,'' EMPNAME,''FLAG"
            strSql += vbCrLf + " ,COMPANYID,RESULT"
            strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & sysid & "LEDGER"
            strSql += vbCrLf + " FROM ("
            strSql += vbCrLf + " SELECT TRANNO,TRANDATE,PAYMODE"
            'strSql += vbCrLf + " ,ACNAME CONTRA"
            strSql += vbCrLf + " ,(CASE WHEN RESULT=1 THEN 'OPENING' "
            strSql += vbCrLf + " WHEN RESULT IN(0,3,4,5) THEN ACNAME "
            strSql += vbCrLf + " ELSE (SELECT TOP 1 PAYDESC FROM " & cnAdminDb & "..TRANPAYMODE WHERE PAYMODE=T.PAYMODE)END) CONTRA"
            strSql += vbCrLf + " ,SUM(DEBIT)DEBIT,SUM(CREDIT)CREDIT,ACCODE,'' BATCHNO,REFNO,REFDATE,SUM(CONVERT(NUMERIC(18,2),RUNTOTAL)) AS  RUNTOTAL"
            strSql += vbCrLf + " ,SUM(RUNTOT)RUNTOT,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,CUR,CHQCARDNO,CARDID,CHQDATE,CHQCARDREF,REMARK1,REMARK2,ACNAME,FROMFLAG,COSTID,'' UPDATED,COSTNAME,'' USERNAME"
            strSql += vbCrLf + " ,COMPANYID,RESULT FROM TEMPTABLEDB..TEMP" & sysid & "ACCLEDGER T"
            strSql += vbCrLf + " GROUP BY TRANNO,TRANDATE,PAYMODE,ACCODE,REFNO,REFDATE,CUR,CHQCARDNO,CARDID,CHQDATE,CHQCARDREF,REMARK1,REMARK2,ACNAME,FROMFLAG,COSTID,COSTNAME"
            strSql += vbCrLf + " ,COMPANYID,RESULT"
            strSql += vbCrLf + " )X"
            strSql += vbCrLf + " ORDER BY TRANDATE"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = " UPDATE TEMPTABLEDB..TEMP" & sysid & "LEDGER SET CONTRA='EXCLUDING OPENING' WHERE RESULT=3"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            strSql = " UPDATE TEMPTABLEDB..TEMP" & sysid & "LEDGER SET CONTRA='TOTAL' WHERE RESULT=4"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            strSql = " UPDATE TEMPTABLEDB..TEMP" & sysid & "LEDGER SET CONTRA='BALANCE' WHERE RESULT=5"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = " SELECT "
            strSql += " TRANNO, TRANDATE, PAYMODE, CONTRA, DEBIT, CREDIT, ACCODE, BATCHNO, REFNO, REFDATE"
            strSql += " ,(CASE WHEN CUR='C' THEN  RUNTOTAL + ' Cr'  ELSE RUNTOTAL +' Dr' END) AS RUNTOTAL    "
            strSql += " ,RUNTOT,PCS,GRSWT,NETWT,CUR,CHQCARDNO,CARDID,CHQDATE,CHQCARDREF,REMARK1,REMARK2,ACNAME,FROMFLAG"
            strSql += " ,COSTID,UPDATED,COSTNAME,USERNAME,EMPNAME,COMPANYID,RESULT "
            strSql += " FROM TEMPTABLEDB..TEMP" & sysid & "LEDGER ORDER BY ACNAME,RESULT"
        Else
            strSql = " UPDATE TEMPTABLEDB..TEMP" & sysid & "ACCLEDGER SET RUNTOTAL=REPLACE(RUNTOTAL,'-','') "
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            If objMoreOption.ChkBillPrefix.Checked Then
                strSql = " ALTER TABLE TEMPTABLEDB..TEMP" & sysid & "ACCLEDGER ALTER COLUMN TRANNO VARCHAR(50)"
                cmd = New OleDbCommand(strSql, cn)
                cmd.ExecuteNonQuery()
                strSql = " UPDATE TEMPTABLEDB..TEMP" & sysid & "ACCLEDGER SET TRANNO=CASE WHEN ISNULL(COSTID,'')<>'' THEN CAST(TRANNO AS VARCHAR) + '/' + COSTID ELSE TRANNO END WHERE RESULT=2 "
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
            End If

            strSql = " SELECT *"
            strSql += " FROM TEMPTABLEDB..TEMP" & sysid & "ACCLEDGER"
            strSql += " ORDER BY SNO"
        End If
        dtGridView = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGridView)
        If Not dtGridView.Rows.Count > 0 Then
            MsgBox("RECORD NOT FOUND")
            chkMultiSelect.Select()
            chkMultiSelect.Focus()
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
        strSql = " SELECT ACCODE,SUM(ISNULL(PCS,0))TOTPCS,SUM(ISNULL(GRSWT,0))TOTGRSWT,SUM(ISNULL(NETWT,0))TOTNETWT FROM TEMPTABLEDB..TEMP" & sysid & "ACCLEDGER"
        strSql += " GROUP BY ACCODE"
        da = New OleDbDataAdapter(strSql, cn)
        dtTotPcsWt.Clear()
        da.Fill(dtTotPcsWt)
        ProgressBarStep("", 10)
        Dim title As String = ""
        If chkCancelonly.Checked Then title += " CANCELED"
        title += " LEDGER FROM " & dtpFrom.Value.ToString("dd-MM-yyyy") & "  TO " & dtpTo.Value.ToString("dd-MM-yyyy") + vbCrLf
        'title = GetSelectedItems(lstAccName, "") & " ACCOUNTS LEDGER " + vbCrLf

        If chkCmbAcGroup.Visible Then
            title += chkCmbAcName.Text
        Else
            strSql = "SELECT ACNAME+' ' + ADDRESS1+' ' + ADDRESS2+' ' + ADDRESS3+' ' + AREA+' ' + CITY+' ' + PINCODE+' ' + MOBILE+' ' + GSTNO FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME='" & cmbAcName.Text & "'"
            title += GetSqlValue(cn, strSql)
            'title += cmbAcName.Text
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
        'For JMT
        'fltrTit += GetCheckedItems(objMoreOption.lstContra, "")
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
        ' ''Catch ex As Exception
        ' ''    MsgBox("ERROR: " + ex.Message + vbCrLf + vbCrLf + "POSITION: " + ex.StackTrace, MsgBoxStyle.Critical, "ERROR MESSAGE")
        ' ''Finally
        ' ''    ProgressBarHide()
        ' ''    btnView_Search.Enabled = True
        ' ''    Me.Cursor = Cursors.Default
        ' ''End Try
        'btnView_Search.Enabled = True
    End Sub
    Function MAXDRCR()
        strSql = " SELECT max(runtot) CR FROM TEMPTABLEDB..TEMP" & sysid & "ACCLEDGER WHERE cur = 'C' AND RESULT = 2"
        Dim maxcr As Decimal = Val(GetSqlValue(cn, strSql).ToString)
        Dim maxcrdate As String
        If maxcr <> 0 Then maxcrdate = GetSqlValue(cn, "SELECT top 1 CONVERT(VARCHAR(13),trandate,103) date FROM TEMPTABLEDB..TEMP" & sysid & "ACCLEDGER WHERE cur = 'C' AND RESULT = 2 AND RUNTOT = " & maxcr).ToString
        strSql = " SELECT max(RUNTOT) dr FROM TEMPTABLEDB..TEMP" & sysid & "ACCLEDGER WHERE CUR = 'D' AND RESULT = 2"
        Dim maxdr As Decimal = Val(GetSqlValue(cn, strSql).ToString)
        Dim maxdrdate As String
        If maxdr <> 0 Then maxdrdate = GetSqlValue(cn, "SELECT top 1 CONVERT(VARCHAR(13),trandate,103) date FROM TEMPTABLEDB..TEMP" & sysid & "ACCLEDGER WHERE cur = 'D' AND RESULT = 2 AND RUNTOT = " & maxdr).ToString


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

    Function ledgerprint() As Integer
        Dim i As Integer
        Dim dt2 As New DataTable
        Dim dtTemp As New DataTable
        Dim da As New OleDbDataAdapter
        Dim cmd As New OleDbCommand
        strSql = " SELECT *"
        strSql += " FROM TEMPTABLEDB..TEMP" & sysid & "ACCLEDGER"
        strSql += " ORDER BY SNO"
        'strsql += " where SNO"
        dtGridView = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt2)
        'dt = ugridView.DataSource
        'dtTemp = dt2.Copy
        If dt2.Rows.Count > 0 Then
            'For i = 0 To dt2.Rows.Count - 1
            '    'dtTemp.Columns.Remove("ACCOUNTID")
            '    'If MsgBox("Do You Want to Print Narration", MsgBoxStyle.YesNo) = MsgBoxResult.No Then dtTemp.Columns.Remove("Narration")
            '    'dtTemp.Columns.Remove("NARRATION")
            '    'dtTemp.Columns("ACCNAME").MaxLength= 27
            '    dt2.Columns("TRANNO").Caption = "Tranno"
            '    dt2.Columns("TRANDATE").Caption = "Trandate"
            '    dt2.Columns("PAYMODE").Caption = "Paymode"
            '    dt2.Columns("CONTRA").Caption = "Contra"
            '    dt2.Columns("DEBIT").Caption = "Debit"
            '    dt2.Columns("CREDIT").Caption = "Credit"
            '    DataGridView1.DataSource = Nothing
            '    DataGridView1.DataSource = dt2
            '    lblTitle.Font = New Font("VERDANA", 9, FontStyle.Bold)
            '    lblTitle.Text = "Ledger Report"
            '    DataGridView1.Visible = True
            '    'dtTemp.Columns("DOCDATE").Caption = "Vou. Date"
            '    'dtTemp.Columns("DOCTYPE").Caption = "Vou. Type"
            '    'ADMPL.PrintDataGridView.Print_View(dtTemp, Nothing, title)
            'Next

            dt2.Columns("TRANNO").Caption = "Tranno"
            dt2.Columns("TRANDATE").Caption = "Trandate"
            dt2.Columns("PAYMODE").Caption = "Paymode"
            dt2.Columns("CONTRA").Caption = "Contra"
            dt2.Columns("DEBIT").Caption = "Debit"
            dt2.Columns("CREDIT").Caption = "Credit"
            DataGridView1.DataSource = Nothing
            DataGridView1.DataSource = dt2
            lblTitle.Font = New Font("VERDANA", 9, FontStyle.Bold)
            lblTitle.Text = "Ledger Report"
            DataGridView1.Visible = True
            DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells

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

    Function LedgerSummaryGridViewFormat(ByVal grid As DataGridView) As Integer
        For Each dgvRow As DataGridViewRow In grid.Rows
            If dgvRow.Cells("PARTICULARS").Value.ToString.Contains(Chr(13)) Then
                dgvRow.DefaultCellStyle.WrapMode = DataGridViewTriState.True
                grid.AutoResizeRow(dgvRow.Index, DataGridViewAutoSizeRowMode.AllCells)
            End If
            If Val(dgvRow.Cells("DEBIT").Value.ToString) = 0 Then
                dgvRow.Cells("DEBIT").Value = DBNull.Value
                dgvRow.Cells("DEBIT").Style.BackColor = dgvRow.Cells("PARTICULARS").Style.BackColor
            Else
                dgvRow.Cells("DEBIT").Style.BackColor = Color.Lavender
            End If
            If Val(dgvRow.Cells("CREDIT").Value.ToString) = 0 Then
                dgvRow.Cells("CREDIT").Value = DBNull.Value
                'dgvRow.Cells("CREDIT").Style.BackColor = dgvRow.Cells("CONTRA").Style.BackColor
            Else
                dgvRow.Cells("CREDIT").Style.BackColor = Color.LavenderBlush
            End If
            Select Case dgvRow.Cells("RESULT").Value.ToString
                Case "0" 'TITLE
                    dgvRow.Cells("PARTICULARS").Value = dgvRow.Cells("ACNAME").Value.ToString
                    dgvRow.DefaultCellStyle.BackColor = Color.LightBlue
                    dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    dgvRow.DefaultCellStyle.ForeColor = Color.Red
                Case "1" 'OPENING
                    dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    If chkMultiSelect.Checked Then dgvRow.Cells("ACNAME").Value = DBNull.Value
                Case "2" 'TRANS
                    dgvRow.Cells("ACNAME").Value = DBNull.Value
                Case "3" 'EXCLUD OPEN
                    dgvRow.DefaultCellStyle.BackColor = Color.LightGoldenrodYellow
                    dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    dgvRow.Cells("ACNAME").Value = DBNull.Value
                Case "4" 'TOTAL
                    dgvRow.DefaultCellStyle.BackColor = Color.LightGoldenrodYellow
                    dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    dgvRow.Cells("ACNAME").Value = DBNull.Value
                Case "5" 'BALANCE
                    dgvRow.DefaultCellStyle.BackColor = Color.LightGoldenrodYellow
                    dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    dgvRow.Cells("ACNAME").Value = DBNull.Value
            End Select
        Next
    End Function

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
        If e.KeyCode = Keys.M Then
            funcLedgerSummury()
        End If
        If e.KeyCode = Keys.I Then
            FunItemDetails()
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

            If e.KeyChar = Chr(Keys.Space) Then
                If gridView_OWN.Rows.Count > 0 Then
                    If gridView_OWN.CurrentRow.DefaultCellStyle.BackColor = Color.LightGreen Then
                        gridView_OWN.CurrentRow.DefaultCellStyle.BackColor = Color.White
                        If Val(gridView_OWN.CurrentRow.Cells("DEBIT").Value.ToString & "") > 0 Then
                            gridView_OWN.CurrentRow.Cells("DEBIT").Style.BackColor = Color.Lavender
                        Else
                            gridView_OWN.CurrentRow.Cells("DEBIT").Style.BackColor = Color.White
                        End If
                        If Val(gridView_OWN.CurrentRow.Cells("CREDIT").Value.ToString & "") > 0 Then
                            gridView_OWN.CurrentRow.Cells("CREDIT").Style.BackColor = Color.LavenderBlush
                        Else
                            gridView_OWN.CurrentRow.Cells("CREDIT").Style.BackColor = Color.White
                        End If
                        If gridView_OWN.CurrentRow.Cells("RUNTOTAL").Value.ToString.ToString.Contains("Dr") Then
                            gridView_OWN.CurrentRow.Cells("RUNTOTAL").Style.BackColor = Color.Lavender
                        Else
                            gridView_OWN.CurrentRow.Cells("RUNTOTAL").Style.BackColor = Color.LavenderBlush
                        End If
                    Else
                        gridView_OWN.CurrentRow.DefaultCellStyle.BackColor = Color.LightGreen
                        gridView_OWN.CurrentRow.Cells("DEBIT").Style.BackColor = Color.LightGreen
                        gridView_OWN.CurrentRow.Cells("CREDIT").Style.BackColor = Color.LightGreen
                        gridView_OWN.CurrentRow.Cells("RUNTOTAL").Style.BackColor = Color.LightGreen
                    End If
                End If
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
        ''strprint = Chr(27) + "@"
        'strprint = ""
        ''FileWrite.WriteLine(strprint)
        'strprint = Chr(27) + Chr(67) + Chr(72)
        'FileWrite.WriteLine(strprint)
        'strprint = Chr(27) + "j" + Chr(180)
        'FileWrite.WriteLine(strprint)
        'strprint = Chr(27) + "j" + Chr(180)
        'FileWrite.WriteLine(strprint)
        'strprint = "TEST PAGE"
        'strprint = Chr(12)
        'FileWrite.WriteLine(strprint)
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
            chkMultiSelect.Focus()
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
        If gridView_OWN.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
            '593
            lblTitle.Text = Replace(lblTitle.Text, vbCrLf, " ")
            strSql = "SELECT EMAILID FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbAcName.Text & "'"
            Dim EMailId As String = objGPack.GetSqlValue(strSql, "EMAILID", "").ToString
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView_OWN, BrightPosting.GExport.GExportType.Export, , , , EMailId)
            '593
        End If
        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView_OWN.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
            gridView_OWN.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
            gridView_OWN.Invalidate()
            For Each dgvCol As DataGridViewColumn In gridView_OWN.Columns
                dgvCol.Width = dgvCol.Width
            Next
            gridView_OWN.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            strSql = "SELECT (COMPANYNAME +' ' + ADDRESS1+' ' +ADDRESS2+' ' +ADDRESS3+' ' +ADDRESS4+' '+PHONE+' '+GSTNO) DETAILS FROM " & cnAdminDb & "..COMPANY WHERE COMPANYID='" & strCompanyId & "'"
            Dim CompanyDetails As String = GetSqlValue(cn, strSql)
            BrightPosting.GExport.Post(Me.Name, "", CompanyDetails & lblTitle.Text, gridView_OWN, BrightPosting.GExport.GExportType.Print) 'strCompanyName
            funcGridViewStyle()
        End If
    End Sub
    '=================================================================================================

    Private Sub btnFindSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindSearch.Click
        FindSearchToolStripMenuItem_Click(Me, New EventArgs)
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        frmAccountsLedger_KeyPress(Me, New KeyPressEventArgs(ChrW(Keys.Escape)))
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
            ''cmbAccGroup.Items.Add("ALL")
            ''strSql = " SELECT ACGRPNAME FROM " & cnAdminDb & "..ACGROUP ORDER BY ACGRPNAME"
            ''objGPack.FillCombo(strSql, cmbAccGroup, False, False)

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
            strSql += " SELECT ACGRPNAME,CONVERT(VARCHAR,ACGRPCODE),2 RESULT FROM " & cnAdminDb & "..ACGROUP"
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
            If gridView_OWN.Rows(e.RowIndex).Cells("FROMFLAG").Value.ToString = "A" _
                        Or gridView_OWN.Rows(e.RowIndex).Cells("FROMFLAG").Value.ToString = "W" _
                        Or gridView_OWN.Rows(e.RowIndex).Cells("FROMFLAG").Value.ToString = "T" Then
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
                If gridView_OWN.Rows(e.RowIndex).Cells("FROMFLAG").Value.ToString = "S" Then
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

    Private Function chkbackdateedit(ByVal ledgerdate As Date) As Boolean

        Dim serverDate As Date = GetActualEntryDate()
        Dim RestrictDays As String = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'DATECONTROL_ACC'")
        If RestrictDays.Contains(",") = False Then
            If Not (ledgerdate >= serverDate.AddDays(-1 * Val(RestrictDays))) Then
                MsgBox("Transaction Date to be edit Not Allowed", MsgBoxStyle.Information)
                Return False
                Exit Function
            End If
        Else
            Dim dayarray() As String = Split(RestrictDays, ",")
            Dim closeday As Integer = Val(dayarray(0).ToString)
            Dim mondiv As Integer = Val(dayarray(1).ToString)
            If closeday > serverDate.Day Then
                Dim mindate As Date = serverDate.AddMonths(-1 * mondiv)
                mindate = mindate.AddDays(-1 * (mindate.Day - 1))
                If Not (ledgerdate >= mindate) Then
                    MsgBox("Transaction Date to be edit Not Allowed", MsgBoxStyle.Information)

                    Return False
                    Exit Function
                End If
            Else
                Dim mindate As Date = serverDate.AddMonths(-1 * 0)
                mindate = mindate.AddDays(-1 * (mindate.Day - 1))
                If Not (ledgerdate >= mindate) Then
                    MsgBox("Transaction Date to be edit Not Allowed", MsgBoxStyle.Information)
                    Return False
                    Exit Function
                End If
            End If
        End If
        Return True
    End Function

    Private Sub btnEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEdit.Click
        If Not gridView_OWN.RowCount > 0 Then Exit Sub

        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Edit) Then Exit Sub

        If ChkDbClosed(cnStockDb) = True Then MsgBox("Transaction for this finincial year Closed." & vbCrLf & "Not able to Edit.", MsgBoxStyle.Information) : Exit Sub

        If chkbackdateedit(gridView_OWN.CurrentRow.Cells("TRANDATE").Value) = False Then Exit Sub

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
        If ChkDbClosed(cnStockDb) = True Then MsgBox("Transaction for this finincial year Closed." & vbCrLf & "Not able to Cancel.", MsgBoxStyle.Information) : Exit Sub
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
            SendmailToInternalTrfAccount(Format(gridView_OWN.CurrentRow.Cells("TRANDATE").Value, "yyyy-MM-dd"), gridView_OWN.CurrentRow.Cells("BATCHNO").Value.ToString)
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
        cmbLedger.Visible = Not chkMultiSelect.Checked
        lblSubledger.Visible = Not chkMultiSelect.Checked
        txtAcname.Visible = False
        ' lviewDetails.Visible = False
        If GetAdmindbSoftValue("RPTLEDGER_ADDRESS", "N") = "Y" Then
            lviewDetails.Columns.Add("Name", 120, HorizontalAlignment.Left)
            lviewDetails.Columns.Add("Description", 250, HorizontalAlignment.Left)
            If chkMultiSelect.Checked = False Then
                lviewDetails.Visible = True
            Else
                lviewDetails.Visible = False
            End If
        End If
    End Sub

    Private Sub cmbAcGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAcGroup.SelectedIndexChanged
        strSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ISNULL(ACTIVE,'Y') <> 'H'"
        If cmbAcGroup.Text <> "ALL" And cmbAcGroup.Text <> "" Then
            strSql += " AND ACGRPCODE = (SELECT ACGRPCODE FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPNAME = '" & cmbAcGroup.Text & "')"
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
        'GetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre)
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
        obj.p_chkDiscEmpName = objMoreOption.chkDiscAuthEmp.Checked
        obj.p_chkCostName = objMoreOption.chkCostName.Checked
        obj.p_rdbWithParticular = objMoreOption.rdbWithParticular.Checked
        obj.p_rdbSepColumns = objMoreOption.rdbSepColumns.Checked
        obj.p_rdbWithBillPrefix = objMoreOption.ChkBillPrefix.Checked
        obj.p_chkEmp = objMoreOption.chkEmp.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmAccountsLedger_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmAccountsLedger_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmAccountsLedger_Properties))
        chkMultiSelect.Checked = obj.p_chkMultiSelect
        SetChecked_CheckedList(chkCmbAcGroup, obj.p_chkCmbAcGroup, "ALL")
        cmbAcGroup.Text = obj.p_cmbAcGroup
        ''SetChecked_CheckedList(chkCmbAcName, obj.p_chkCmbAcName, Nothing)
        If obj.p_chkCmbAcName.Count > 0 Then
            SetChecked_CheckedList(chkCmbAcName, obj.p_chkCmbAcName, Nothing)
        End If
        cmbAcName.Text = obj.p_cmbAcName
        chkWithRunningBalance.Checked = obj.p_chkWithRunningBalance
        chkOrderbyTranNo.Checked = obj.p_chkOrderbyTranNo
        'SetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre, cnCostName)
        'SetChecked_CheckedList(chkCmbCompany, obj.p_chkCmbCompany, strCompanyName)
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
        objMoreOption.chkDiscAuthEmp.Checked = obj.p_chkDiscEmpName
        objMoreOption.chkCostName.Checked = obj.p_chkCostName
        objMoreOption.rdbWithParticular.Checked = obj.p_rdbWithParticular
        objMoreOption.rdbSepColumns.Checked = obj.p_rdbSepColumns
        objMoreOption.ChkBillPrefix.Checked = obj.p_rdbWithBillPrefix
        objMoreOption.chkEmp.Checked = obj.p_chkEmp
    End Sub

    Private Sub btnDuplicatePrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDuplicatePrint.Click
        ''Duplicate Print

        If gridView_OWN.CurrentRow.Cells("TRANDATE").Value.ToString = "" Then Exit Sub
        Dim pBatchno As String = gridView_OWN.CurrentRow.Cells("BATCHNO").Value.ToString
        Dim pBillDate As Date = gridView_OWN.CurrentRow.Cells("TRANDATE").Value
        Dim pfromflag As String = gridView_OWN.CurrentRow.Cells("FROMFLAG").Value.ToString
        Dim pParamStr As String = ""
        Dim prnmemsuffix As String = ""
        Dim ptranno As Integer = gridView_OWN.CurrentRow.Cells("TRANNO").Value
        Dim pPayMode As String = "CR"
        Dim saledbName As String = cnStockDb
        If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId
        Dim BillPrintExe As Boolean = IIf(GetAdmindbSoftValue("CALLBILLPRINTEXE", "Y") = "Y", True, False)
        Dim BillPrint_Format As String = GetAdmindbSoftValue("BILLPRINT_FORMAT", "")
        If GST And BillPrint_Format = "M1" Then
            ''Dim obj As New BrighttechREPORT.frmCashTransactionPrint(pPayMode, saledbName, pBillDate.Date, ptranno, pBatchno, "ACCTRAN", False, True, "OUTSTANDING", "", True)
            Dim obj As New BrighttechREPORT.frmBillPrintDocA4N("ACC", pBatchno, pBillDate.ToString("yyyy-MM-dd"), "Y")
        ElseIf GST And BillPrint_Format = "M2" Then
            Dim obj As New BrighttechREPORT.frmBillPrintDocB5("ACC", pBatchno, pBillDate.ToString("yyyy-MM-dd"), "Y")
        ElseIf GST And BillPrint_Format = "M3" Then
            Dim obj As New BrighttechREPORT.frmBillPrintDocA5("ACC", pBatchno, pBillDate.ToString("yyyy-MM-dd"), "Y")
        ElseIf GST And BillPrint_Format = "M4" Then
            Dim obj As New BrighttechREPORT.frmBillPrintDocB52cpy("ACC", pBatchno, pBillDate.ToString("yyyy-MM-dd"), "Y")
        ElseIf GST And BillPrintExe = False Then
            Dim billDoc As New frmBillPrintDoc("ACC", pBatchno, pBillDate.ToString("yyyy-MM-dd"), "Y")
        Else
            If IO.File.Exists(Application.StartupPath & "\BillPrint.exe") Then
                If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = sysid
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
                write.WriteLine(LSet("FROMFLAG", 15) & ":" & pfromflag & "")
                pParamStr += LSet("DUPLICATE", 15) & ":Y"
                write.Flush()
                write.Close()
                If EXE_WITH_PARAM = False Then
                    System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe")
                Else
                    System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe", pParamStr)
                End If
            End If
        End If

        ''If IO.File.Exists(Application.StartupPath & "\BillPrint.exe") Then
        ''    If gridView_OWN.CurrentRow.Cells("TRANDATE").Value.ToString = "" Then Exit Sub
        ''    Dim pBatchno As String = gridView_OWN.CurrentRow.Cells("BATCHNO").Value.ToString
        ''    Dim pBillDate As Date = gridView_OWN.CurrentRow.Cells("TRANDATE").Value
        ''    Dim pfromflag As String = gridView_OWN.CurrentRow.Cells("FROMFLAG").Value.ToString
        ''    Dim pParamStr As String = ""
        ''    Dim prnmemsuffix As String = ""
        ''    If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = sysid
        ''    Dim memfile As String = "\BillPrint" + prnmemsuffix.Trim & ".mem"

        ''    Dim write As IO.StreamWriter
        ''    write = IO.File.CreateText(Application.StartupPath & memfile)
        ''    write.WriteLine(LSet("TYPE", 15) & ":ACC")
        ''    pParamStr += LSet("TYPE", 15) & ":ACC" & ";"
        ''    write.WriteLine(LSet("BATCHNO", 15) & ":" & pBatchno)
        ''    pParamStr += LSet("BATCHNO", 15) & ":" & pBatchno & ";"
        ''    write.WriteLine(LSet("TRANDATE", 15) & ":" & pBillDate.ToString("yyyy-MM-dd"))
        ''    pParamStr += LSet("TRANDATE", 15) & ":" & pBillDate.ToString("yyyy-MM-dd") & ";"
        ''    write.WriteLine(LSet("DUPLICATE", 15) & ":Y")
        ''    write.WriteLine(LSet("FROMFLAG", 15) & ":" & pfromflag & "")
        ''    pParamStr += LSet("DUPLICATE", 15) & ":Y"
        ''    write.Flush()
        ''    write.Close()

        ''    If EXE_WITH_PARAM = False Then
        ''        System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe")
        ''    Else
        ''        System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe", pParamStr)
        ''    End If
        ''End If
    End Sub

    Private Sub btnChqPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChqPrint.Click
        ''Duplicate Print
        If _Chqformat = 1 Then
            If IO.File.Exists(Application.StartupPath & "\ChequePrint.exe") Then
                If gridView_OWN.CurrentRow.Cells("TRANDATE").Value.ToString = "" Then Exit Sub
                Dim pBatchno As String = gridView_OWN.CurrentRow.Cells("BATCHNO").Value.ToString
                Dim pchqDate As String = gridView_OWN.CurrentRow.Cells("CHQDATE").Value.ToString
                Dim pPayee As String = gridView_OWN.CurrentRow.Cells("CONTRA").Value.ToString
                Dim pChqamt As String = gridView_OWN.CurrentRow.Cells("Credit").Value.ToString

                strSql = " SELECT CASE WHEN SUM(ISNULL(DEBIT,0))-SUM(ISNULL(CREDIT,0))>0 THEN SUM(ISNULL(DEBIT,0))-SUM(ISNULL(CREDIT,0)) ELSE 0 END AS DEBIT"
                strSql += " ,CASE WHEN SUM(ISNULL(CREDIT,0))-SUM(ISNULL(DEBIT,0))>0 THEN SUM(ISNULL(CREDIT,0))-SUM(ISNULL(DEBIT,0)) ELSE 0 END AS CREDIT"
                strSql += " FROM TEMPTABLEDB..TEMP" & sysid & "ACCLEDGER"
                strSql += " WHERE BATCHNO='" & pBatchno & "'"
                Dim dtchqamt As New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtchqamt)
                If dtchqamt.Rows.Count > 0 Then pChqamt = dtchqamt.Rows(0).Item("Credit").ToString

                Dim pChqformat As String = getchqbookfomat(gridView_OWN.CurrentRow.Cells(6).Value, Val(gridView_OWN.CurrentRow.Cells(16).Value))
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
        ElseIf _Chqformat = 2 Then
            If gridView_OWN.CurrentRow.Cells("TRANDATE").Value.ToString = "" Then
                MsgBox("TRANDATE IS EMPTY", MsgBoxStyle.Information)
                Exit Sub
            End If
            Dim pBatchno As String = gridView_OWN.CurrentRow.Cells("BATCHNO").Value.ToString
            Dim pchqDate As String = gridView_OWN.CurrentRow.Cells("CHQDATE").Value.ToString 'Date
            strSql = " SELECT (SELECT TOP 1 PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = C.PSNO)AS NAME "
            strSql += " FROM " & cnAdminDb & "..CUSTOMERINFO AS C"
            strSql += " WHERE BATCHNO = '" & pBatchno & "' "
            Dim pPayee As String = objGPack.GetSqlValue(strSql) ' AcName
            Dim pChqamt As String = gridView_OWN.CurrentRow.Cells("Credit").Value.ToString
            Dim pAcName As String = ""
            strSql = " SELECT ACNAME,CASE WHEN SUM(ISNULL(DEBIT,0))-SUM(ISNULL(CREDIT,0))>0 THEN SUM(ISNULL(DEBIT,0))-SUM(ISNULL(CREDIT,0)) ELSE 0 END AS DEBIT"
            strSql += " ,CASE WHEN SUM(ISNULL(CREDIT,0))-SUM(ISNULL(DEBIT,0))>0 THEN SUM(ISNULL(CREDIT,0))-SUM(ISNULL(DEBIT,0)) ELSE 0 END AS CREDIT"
            strSql += " FROM TEMPTABLEDB..TEMP" & sysid & "ACCLEDGER"
            strSql += " WHERE BATCHNO='" & pBatchno & "' GROUP BY ACNAME"
            Dim dtchqamt As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtchqamt) ' Chqamt & Acname
            If dtchqamt.Rows.Count > 0 Then pChqamt = dtchqamt.Rows(0).Item("Credit").ToString : pAcName = dtchqamt.Rows(0).Item("ACNAME").ToString
            If Val(pChqamt) = 0 Then MsgBox("Cheque Amount was zero", MsgBoxStyle.Critical) : Exit Sub
            If pPayee = "" Then
                pPayee = gridView_OWN.CurrentRow.Cells(18).Value.ToString
            End If
            If pPayee = "" Then MsgBox("Payee Name is Empty ", MsgBoxStyle.Critical) : Exit Sub
            Dim write As IO.StreamWriter
            write = IO.File.CreateText(Application.StartupPath & "\ChequePrint.Ini")
            write.WriteLine(LSet("Cheque Date", 16) & ":" & pchqDate)
            write.WriteLine(LSet("Payee", 16) & ":" & pPayee)
            write.WriteLine(LSet("Cheque Amount", 16) & ":" & pChqamt)
            write.WriteLine(LSet("ACNAME ", 16) & ":" & pAcName)
            write.Flush()
            write.Close()
            chqDate = pchqDate
            chqPayee1 = pPayee
            chqAmtValue = pChqamt
            strSql = "SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = '" & pAcName.ToString & "' "
            chqBankName = objGPack.GetSqlValue(strSql)
            If chqBankName = "" Then chqBankName = pAcName.ToString
            strSql = "SELECT 'CHECK' FROM " & cnAdminDb & "..CHQLAYOUT WHERE LAYOUTNAME = '" & chqBankName.ToString & "'"
            Dim check As String = objGPack.GetSqlValue(strSql)
            If check = "CHECK" Then
                If PrintDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
                    Try
                        PrintDialog1.Document = PrintDocument1
                        PrintDocument1.Print()
                    Catch ex As Exception
                        MessageBox.Show(ex.ToString)
                        Exit Sub
                    End Try
                End If
            Else
                MsgBox(chqBankName.ToString + "Cheque Print Not Design", MsgBoxStyle.Information)
                Exit Sub
            End If
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

        Dim mtemptable As String = "TEMP" & sysid & "ACCINDEX"
        Dim mtemptable1 As String = "TEMP" & sysid & "ACCINDEX1"
        'strSql += vbCrLf + " select acname AS LEDGER,count(*) AS LINES,0 as pageno into master.." & mtemptable & " from master..TEMP" & sysid & "ACCLEDGER group by acname"
        'strSql += vbCrLf + " union "
        'strSql += vbCrLf + " select  acname AS LEDGER,count(*) AS LINES,0 as pageno from master..TEMP" & sysid & "ACCLEDGER where isnull(remark1,'') <> '' group by acname"
        'strSql += vbCrLf + " union  "
        'strSql += vbCrLf + " select  acname AS LEDGER,count(*) AS LINES,0 as pageno  from master..TEMP" & sysid & "ACCLEDGER where isnull(remark2,'') <> '' group by acname"
        'strSql += vbCrLf + " union  "
        'strSql += vbCrLf + " select  acname AS LEDGER,count(*) AS LINES,0 as pageno  from master..TEMP" & sysid & "ACCLEDGER where isnull(remark1,'')  = '' or isnull(remark1,'') = '' group by acname "

        strSql = " IF (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & sysid & "ACCINDEX')>0 DROP TABLE TEMPTABLEDB..TEMP" & sysid & "ACCINDEX"
        strSql += vbCrLf + " IF (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & sysid & "ACCINDEX1')>0 DROP TABLE TEMPTABLEDB..TEMP" & sysid & "ACCINDEX1"
        'strSql += vbCrLf + " SELECT ACNAME AS LEDGER,COUNT(*)/34 AS PAGENO INTO master.." & mtemptable1 & " FROM  master..TEMP" & sysid & "ACCLEDGER GROUP BY ACNAME"
        'strSql += vbCrLf + " SELECT ACNAME AS LEDGER,CEILING(CONVERT(NUMERIC,COUNT(*))/34) AS PAGENO INTO master.." & mtemptable1 & " FROM  master..TEMP" & sysid & "ACCLEDGER GROUP BY ACNAME"
        strSql += vbCrLf + " SELECT ACNAME AS LEDGER,CEILING((CASE WHEN CONVERT(NUMERIC,COUNT(*)) > 34 THEN CONVERT(NUMERIC,COUNT(*)-1)/32 ELSE CONVERT(NUMERIC,COUNT(*))/34 END)) AS PAGENO INTO TEMPTABLEDB.." & mtemptable1 & " FROM  TEMPTABLEDB..TEMP" & sysid & "ACCLEDGER GROUP BY ACNAME"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        'strSql = " update  master.." & mtemptable1 & " set pageno=pageno+1 where  isnull(pageno,0)<>0 "
        'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        'cmd.CommandTimeout = 1000
        'cmd.ExecuteNonQuery()

        Dim indexcount As Decimal
        indexcount = 0
        'indexcount = objGPack.GetSqlValue("SELECT round(convert(numeric(15,2), count(*))/60,0) as count from master.." & mtemptable1 & " ", , )
        indexcount = objGPack.GetSqlValue("SELECT CEILING(convert(numeric(15,2), count(*))/60) as count from TEMPTABLEDB.." & mtemptable1 & " ", , )

        'strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & sysid & "ACCINDEX1')>0 DROP TABLE TEMP" & sysid & "ACCINDEX1"
        'strSql += vbCrLf + " select LEDGER,SUM(LINES) LINES,0 as pageno into master.." & mtemptable1 & " from master.." & mtemptable & " group by LEDGER"
        'strSql += vbCrLf + " order by LEDGER"
        'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        'cmd.CommandTimeout = 1000
        'cmd.ExecuteNonQuery()

        'strSql = " update a set a.pageno = CEILING(lines/65) from master.." & mtemptable1 & " a where isnull(Ledger,'')<>'' "
        'strSql = " update a set a.pageno = round(convert(numeric(15,2), lines)/68,0) from master.." & mtemptable1 & " a where isnull(Ledger,'')<>'' "
        'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        'cmd.CommandTimeout = 1000
        'cmd.ExecuteNonQuery()

        strSql = " SELECT * FROM TEMPTABLEDB.." & mtemptable1 & " ORDER BY LEDGER"
        Dim dt1 As New DataTable()
        Dim temp As String = "0"
        Dim temp1 As String = "0"
        Dim temp2 As String = "0"

        dt1.Clear()
        _ledgerName = ""
        _pageNo = ""
        _sNo = ""

        Dim dttInd As DataTable
        dttInd = New DataTable
        dttInd.Columns.Add("LEDGERNAME1")
        dttInd.Columns.Add("PAGENO1")
        dttInd.Columns.Add("SNO1")
        Dim _ledgerName1 As String = ""
        Dim _pageNo1 As String = ""
        Dim _sNo1 As String = ""

        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt1)

        dt1.Columns.Add("Pgno", Type.GetType("System.Decimal"))
        dt1.Columns.Add("Sno", Type.GetType("System.Int32"))

        If dt1.Rows.Count > 0 Then
            For k As Integer = 0 To dt1.Rows.Count - 1
                'If k = 125 Then MsgBox("")
                If k = 0 Then
                    If indexcount <= 2 Then
                        dt1.Rows(k).Item("Pgno") = 2
                    Else
                        dt1.Rows(k).Item("Pgno") = Format(indexcount, "#0") + 1
                    End If
                    dt1.Rows(k).Item("Sno") = k + 1
                    _sNo += dt1.Rows(k).Item("Sno").ToString + Environment.NewLine
                    _pageNo += dt1.Rows(k).Item("Pgno").ToString + Environment.NewLine
                    '
                    _sNo1 = dt1.Rows(k).Item("Sno").ToString + vbCrLf
                    _pageNo1 = dt1.Rows(k).Item("Pgno").ToString + vbCrLf
                    '
                    temp = (k + 1).ToString
                ElseIf dt1.Rows(k - 1).Item("Pageno").ToString = 0 Then
                    dt1.Rows(k).Item("pgno") = Val(dt1.Rows(k - 1).Item("pgno")) + 1
                    dt1.Rows(k).Item("Sno") = k + 1
                    _sNo += dt1.Rows(k).Item("Sno").ToString + Environment.NewLine
                    _pageNo += dt1.Rows(k).Item("Pgno").ToString + Environment.NewLine
                    '
                    _sNo1 = dt1.Rows(k).Item("Sno").ToString + vbCrLf
                    _pageNo1 = dt1.Rows(k).Item("Pgno").ToString + vbCrLf
                    '
                    temp = (k + 1).ToString
                Else
                    dt1.Rows(k).Item("pgno") = Val(dt1.Rows(k - 1).Item("pgno")) + Val(dt1.Rows(k - 1).Item("Pageno").ToString)
                    dt1.Rows(k).Item("Sno") = k + 1
                    _sNo += dt1.Rows(k).Item("Sno").ToString + Environment.NewLine
                    _pageNo += dt1.Rows(k).Item("Pgno").ToString + Environment.NewLine
                    '
                    _sNo1 = dt1.Rows(k).Item("Sno").ToString + vbCrLf
                    _pageNo1 = dt1.Rows(k).Item("Pgno").ToString + vbCrLf
                    '
                    temp = (k + 1).ToString
                End If
                _ledgerName += dt1.Rows(k).Item("LEDGER").ToString + Environment.NewLine
                '
                _ledgerName1 = dt1.Rows(k).Item("LEDGER").ToString + vbCrLf
                '
                Dim drInd As DataRow
                drInd = dttInd.NewRow
                drInd!LEDGERNAME1 = _ledgerName1
                drInd!PAGENO1 = _pageNo1
                drInd!SNO1 = _sNo1
                dttInd.Rows.Add(drInd)
            Next
        End If

        dt.Clear()
        dt.TableName = "Crystal Report"
        strSql = " SELECT c.CompanyName,Address1,Address2,Address3,Phone,Convert(varchar(50),TranNo) as TranNo,CONVERT(VARCHAR(10), TranDate, 103)as TranDate,Contra,Convert(varchar(50),Debit)as Debit,Convert(varchar(50),Credit)as Credit,ACNAME"
        strSql += vbCrLf + " ,(SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & sysid & "ACCLEDGER AC WHERE AC.contra= Ac.acname) as SNO"
        strSql += vbCrLf + " ,(SELECT   ACGRPNAME FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPCODE=(SELECT TOP 1 ACGRPCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = A.ACCODE)) AS ACGRPNAME"
        strSql += vbCrLf + " ,RESULT"
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & sysid & "ACCLEDGER A"
        strSql += vbCrLf + " Left join " & cnAdminDb & "..Company as C on C.CompanyID=A.CompanyID"
        strSql += vbCrLf + " WHERE ISNULL(C.ACTIVE,'')<>'N'"
        strSql += vbCrLf + " ORDER BY A.SNO"
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
            'For k As Integer = 0 To dttInd.Rows.Count - 1
            '    row = objInvoiceReport.dtLedger.NewRow
            '    row.LEDGERNAME = dttInd.Rows(k)("LEDGERNAME1").ToString
            '    row.SNO = dttInd.Rows(k)("SNO1").ToString
            '    row.INDEXPAGE = dttInd.Rows(k)("PAGENO1").ToString
            '    objInvoiceReport.dtLedger.AdddtLedgerRow(row)
            'Next
            'FOR CARRIED OVER ******
            Dim _strAcName As String = ""
            Dim _sumDebit As Double = 0
            Dim _sumCredit As Double = 0
            '******CARRIED OVER
            Dim _line As Integer = 0
            For i = 0 To dt.Rows.Count - 1
                If _strAcName <> dt.Rows(i).Item("AcName").ToString Then _line = 0
                If _line = 34 And _strAcName = dt.Rows(i).Item("AcName").ToString Then
                    row = objInvoiceReport.dtLedger.NewRow
                    row.COMPANYNAME = dt.Rows(1).Item("COMPANYNAME").ToString
                    row.ADDRESS1 = dt.Rows(1).Item("ADDRESS1").ToString
                    row.ADDRESS2 = dt.Rows(1).Item("ADDRESS2").ToString
                    row.ADDRESS3 = dt.Rows(1).Item("ADDRESS3").ToString
                    row.PHONE = dt.Rows(1).Item("Phone").ToString
                    'row.TRANNO = dt.Rows(i).Item("TRANNO").ToString
                    'row.TRANDATE = dt.Rows(i).Item("TRANDATE").ToString
                    row.CONTRA = "Brought Forward"
                    row.DEBIT = _sumDebit
                    row.CREDIT = _sumCredit
                    row.ACNAME = dt.Rows(i).Item("ACNAME").ToString
                    row.ACGRPNAME = dt.Rows(i).Item("ACGRPNAME").ToString
                    row.FROMDATE = dtpFrom.Value.Date.ToString("dd-MMM-yyyy")
                    row.TODATE = dtpTo.Value.Date.ToString("dd-MMM-yyyy")
                    row.LEDGERNAME = _ledgerName
                    row.SNO = _sNo
                    row.INDEXPAGE = _pageNo
                    objInvoiceReport.dtLedger.AdddtLedgerRow(row)
                    _line = 1
                End If
                If _strAcName <> dt.Rows(i).Item("AcName").ToString Then _strAcName = dt.Rows(i).Item("AcName").ToString : _sumDebit = 0 : _sumCredit = 0

                If dt.Rows(i).Item("Contra").ToString = dt.Rows(i).Item("AcName").ToString Then
                    If dt.Rows(i).Item("TRANNO").ToString = "" And dt.Rows(i).Item("TRANDATE").ToString = "" Then
                        dt.Rows(i).Item("TRANNO") = ""
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
                If Val(dt.Rows(i).Item("RESULT").ToString) = 0 Then
                    row.CONTRA = dt.Rows(i).Item("ACNAME").ToString
                Else
                    row.CONTRA = dt.Rows(i).Item("CONTRA").ToString
                End If
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
                row.ACGRPNAME = dt.Rows(i).Item("ACGRPNAME").ToString
                row.FROMDATE = dtpFrom.Value.Date.ToString("dd-MMM-yyyy")
                row.TODATE = dtpTo.Value.Date.ToString("dd-MMM-yyyy")
                row.LEDGERNAME = _ledgerName
                row.SNO = _sNo
                row.INDEXPAGE = _pageNo
                _sumDebit += Val(dt.Rows(i).Item("DEBIT").ToString & "")
                _sumCredit += Val(dt.Rows(i).Item("CREDIT").ToString & "")
                objInvoiceReport.dtLedger.AdddtLedgerRow(row)
                _line += 1
                If _line = 33 And _strAcName = dt.Rows(i).Item("AcName").ToString And i + 1 <= dt.Rows.Count - 1 Then
                    If _strAcName <> dt.Rows(i + 1).Item("AcName").ToString Then Continue For
                    row = objInvoiceReport.dtLedger.NewRow
                    row.COMPANYNAME = dt.Rows(1).Item("COMPANYNAME").ToString
                    row.ADDRESS1 = dt.Rows(1).Item("ADDRESS1").ToString
                    row.ADDRESS2 = dt.Rows(1).Item("ADDRESS2").ToString
                    row.ADDRESS3 = dt.Rows(1).Item("ADDRESS3").ToString
                    row.PHONE = dt.Rows(1).Item("Phone").ToString
                    'row.TRANNO = dt.Rows(i).Item("TRANNO").ToString
                    'row.TRANDATE = dt.Rows(i).Item("TRANDATE").ToString
                    row.CONTRA = "Carried Over"
                    row.DEBIT = _sumDebit
                    row.CREDIT = _sumCredit
                    row.ACNAME = dt.Rows(i).Item("ACNAME").ToString
                    row.ACGRPNAME = dt.Rows(i).Item("ACGRPNAME").ToString
                    row.FROMDATE = dtpFrom.Value.Date.ToString("dd-MMM-yyyy")
                    row.TODATE = dtpTo.Value.Date.ToString("dd-MMM-yyyy")
                    row.LEDGERNAME = _ledgerName
                    row.SNO = _sNo
                    row.INDEXPAGE = _pageNo
                    objInvoiceReport.dtLedger.AdddtLedgerRow(row)
                    _line += 1
                End If
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

        Dim mtemptable As String = "TEMP" & sysid & "ACCINDEX"
        strSql = " IF (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & sysid & "ACCINDEX')>0 DROP TABLE TEMPTABLEDB..TEMP" & sysid & "ACCINDEX"
        strSql += vbCrLf + " select acname AS LEDGER,count(*) AS LINES,0 as pageno into TEMPTABLEDB.." & mtemptable & " from TEMPTABLEDB..TEMP" & sysid & "ACCLEDGER group by acname"
        'strSql += vbCrLf + " union "
        'strSql += vbCrLf + " select  acname AS LEDGER,count(*) AS LINES,0 as pageno from master..TEMP" & sysid & "ACCLEDGER where remark1 <> '' group by acname"
        'strSql += vbCrLf + " union  "
        'strSql += vbCrLf + " select  acname AS LEDGER,count(*) AS LINES,0 as pageno  from master..TEMP" & sysid & "ACCLEDGER where remark2 <> '' group by acname"
        'strSql += vbCrLf + " order by acname"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        Dim mtemptable1 As String = "TEMP" & sysid & "ACCINDEX1"
        strSql = " IF (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & sysid & "ACCINDEX1')>0 DROP TABLE TEMPTABLEDB..TEMP" & sysid & "ACCINDEX1"
        strSql += vbCrLf + " select LEDGER,SUM(LINES) LINES,0 as pageno into TEMPTABLEDB.." & mtemptable1 & " from TEMPTABLEDB.." & mtemptable & " group by LEDGER"
        strSql += vbCrLf + " order by LEDGER"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        'strSql = " update a set a.pageno = CEILING(lines/63) from master.." & mtemptable1 & " a where isnull(Ledger,'')<>'' "
        strSql = " update a set a.pageno = CEILING((CASE WHEN CONVERT(NUMERIC,lines) > 58 THEN CONVERT(NUMERIC,lines-1)/56 ELSE CONVERT(NUMERIC,lines)/58 END)) from TEMPTABLEDB.." & mtemptable1 & " a where isnull(Ledger,'')<>'' "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " SELECT * FROM TEMPTABLEDB.." & mtemptable1 & " ORDER BY LEDGER"
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
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & sysid & "ACCLEDGER A"
        strSql += vbCrLf + " Left join " & cnAdminDb & "..Company as C on C.CompanyID=A.CompanyID"
        strSql += vbCrLf + " WHERE ISNULL(C.ACTIVE,'')<>'N'"
        strSql += vbCrLf + " ORDER BY SNO"

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
        ' ''FIRST INDEX PAGE PRINTING
        strprint = Chr(27) + "@"
        FileWrite.WriteLine(strprint)
        strprint = Chr(27) + Chr(67) + Chr(72)
        FileWrite.WriteLine(strprint)
        strprint = Chr(27) + "j" + Chr(180)
        FileWrite.WriteLine(strprint)
        'strprint = Chr(27) + "j" + Chr(180)
        'FileWrite.WriteLine(strprint)

        'printindexheader()
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
                'FIRST INDEX PAGE PRINT
                'FileWrite.WriteLine(strprint)
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

                'strprint = Chr(12)
                'FileWrite.WriteLine(strprint)

                'FIRST INDEX PRINTING
                Printheader(headname, CompanyName, Address1, Address2, Address3, Phone)

                'FOR CARRIED OVER ******
                Dim _strAcName As String = ""
                Dim _sumDebit As Double = 0
                Dim _sumCredit As Double = 0
                Dim _sumOpDebit As Double = 0
                Dim _sumOpCredit As Double = 0
                Dim _tempDebit As Double = 0
                Dim _tempCredit As Double = 0

                '******CARRIED OVER
                Dim dtrows() As DataRow = dtledger.Select("AcName ='" & headname & "'")
                If dtrows.Length > 0 Then
                    Dim checkDate As String = ""
                    For i = 0 To dtrows.Length - 1
                        'If headname <> dtrows(i).Item("CONTRA").ToString Then

                        If line = 11 And _strAcName = headname Then
                            str1 = LSet("", 8)
                            str2 = LSet("", 11)
                            str3 = LSet("Brought Forward", 45)
                            str4 = RSet(_sumDebit.ToString, 14)
                            str5 = RSet(_sumCredit.ToString, 14)
                            strprint = str1 + str2 + str3 + str4 + str5
                            FileWrite.WriteLine(strprint)
                            line += 1
                        End If
                        If _strAcName <> headname Then _strAcName = headname : _sumDebit = 0 : _sumCredit = 0
                        mremark = ""
                        str1 = dtrows(i).Item("TRANDATE").ToString
                        'If CheckDate.ToString = "" Then
                        '    CheckDate = dtrows(i).Item("TRANDATE").ToString
                        'End If
                        If (str1.ToString <> "") Then
                            If checkDate = str1 Then
                                str1 = ""
                            Else
                                Dim dtStr As Date = Convert.ToDateTime(str1)
                                Dim dateStr As String = dtStr.ToString("dd/MM/yy")
                                str1 = dateStr ' dtrows(i).Item("TRANDATE").ToString
                                checkDate = dtrows(i).Item("TRANDATE").ToString
                            End If
                        End If

                        str1 = LSet(str1, 8) '+ "|"
                        'str1 = LSet(dtrows(i).Item("TRANDATE").ToString, 11) + "|"
                        'str2 = LSet(dtrows(i).Item("TRANNO").ToString, 8) + "To"
                        'str2 = LSet(dtrows(i).Item("TRANNO").ToString, 8) + "By"

                        _tempDebit = Val(dtrows(i).Item("DEBIT").ToString & "")
                        _tempCredit = Val(dtrows(i).Item("CREDIT").ToString & "")
                        Dim tempContra As String = dtrows(i).Item("CONTRA").ToString

                        If dtrows(i).Item("FROMFLAG").ToString <> "" Then
                            If dtrows(i).Item("CONTRA").ToString <> "TOTAL" Then
                                If dtrows(i).Item("CONTRA").ToString <> "BALANCE" Then
                                    If _tempDebit <> "0.0" Then
                                        str2 = LSet("| To |", 11)
                                    End If
                                    If _tempCredit <> "0.0" Then
                                        str2 = LSet("| By |", 11)
                                    End If
                                    If _tempDebit <> "0.0" And _tempCredit <> "0.0" Then
                                        str2 = LSet("     ", 11)
                                    End If
                                End If
                            End If
                        End If

                        str3 = dtrows(i).Item("CONTRA").ToString
                        If str3.Contains(vbCr) Then mremark = Mid(str3, InStr(str3, vbCr) + 1, Len(str3))
                        If Trim(mremark) <> "" Then str3 = LSet(Mid(str3, 1, InStr(str3, vbCr) - 1), 45)
                        If dtrows(i).Item("DEBIT").ToString <> "0.00" Then
                            str4 = "|" + RSet(dtrows(i).Item("DEBIT").ToString, 13)
                        Else
                            str4 = ""
                        End If
                        If dtrows(i).Item("CREDIT").ToString <> "0.00" Then
                            str5 = "|" + RSet(dtrows(i).Item("CREDIT").ToString, 13)
                        Else
                            str5 = ""
                        End If
                        str1 = LSet(str1, 8)
                        str2 = LSet(str2, 6)
                        str3 = LSet(str3, 45)
                        str4 = LSet(str4, 14)
                        str5 = LSet(str5, 14)

                        strprint = str1 + str2 + str3 + str4 + str5


                        If dtrows(i).Item("FROMFLAG").ToString = "" Then
                            If dtrows(i).Item("CONTRA").ToString = "OPENING..." Then
                                _sumOpDebit = Val(dtrows(i).Item("DEBIT").ToString & "")
                                _sumOpCredit = Val(dtrows(i).Item("CREDIT").ToString & "")
                                str1 = LSet("", 8)
                                str2 = LSet("", 6)
                                str3 = LSet(str3.ToString, 45)
                                str4 = RSet(str4.ToString, 14)
                                str5 = RSet(str5.ToString, 14)
                                strprint = str1 + str2 + str3 + str4 + str5
                            End If
                            If dtrows(i).Item("CONTRA").ToString = "EXCLUDING OPENING" Then
                                strprint = "------------------------------------------------------------------------------------------"
                                FileWrite.WriteLine(strprint)
                                str1 = LSet("", 8)
                                str2 = LSet("", 6)
                                str3 = LSet(str3.ToString, 45)
                                str4 = RSet(Format((Math.Round((_sumDebit - _sumOpDebit), 2)), "0.00").ToString, 13)
                                str5 = RSet(Format((Math.Round((_sumCredit - _sumOpCredit), 2)), "0.00").ToString, 14)
                                strprint = str1 + str2 + str3 + str4 + str5
                                FileWrite.WriteLine(strprint)
                                strprint = "------------------------------------------------------------------------------------------"
                            End If
                            If dtrows(i).Item("CONTRA").ToString = "TOTAL" Then
                                str1 = LSet("", 8)
                                str2 = LSet("", 6)
                                str3 = LSet(str3.ToString, 45)
                                str4 = RSet(Format(Math.Round(_sumDebit, 2), "0.00").ToString, 13)
                                str5 = RSet(Format(Math.Round(_sumCredit, 2), "0.00").ToString, 14)
                                strprint = str1 + str2 + str3 + str4 + str5
                                FileWrite.WriteLine(strprint)
                                strprint = "------------------------------------------------------------------------------------------"
                            End If
                            If dtrows(i).Item("CONTRA").ToString = "BALANCE" Then
                                str1 = LSet("", 8)
                                str2 = LSet("", 6)
                                str3 = LSet(str3.ToString, 45)
                                str4 = RSet(_sumDebit.ToString, 13)
                                str5 = RSet(_sumCredit.ToString, 14)
                                If _sumCredit > _sumDebit Then
                                    str4 = RSet("", 13)
                                    str5 = RSet(Format((Math.Round(_sumCredit, 2) - Math.Round(_sumDebit, 2)), "0.00").ToString, 14)
                                Else
                                    str4 = RSet(Format((Math.Round(_sumDebit, 2) - Math.Round(_sumCredit, 2)), "0.00").ToString, 13)
                                    str5 = RSet("", 14)
                                End If
                                strprint = str1 + str2 + str3 + str4 + str5
                                FileWrite.WriteLine(strprint)
                                strprint = "------------------------------------------------------------------------------------------"
                            End If
                        End If
                        'strprint = str1 + str2 + str3 + str4 + str5
                        strprint = Chr(27) + "@" + Chr(18) + strprint
                        FileWrite.WriteLine(strprint)
                        line += 1


                        If dtrows(i).Item("CONTRA").ToString <> "TOTAL" And dtrows(i).Item("CONTRA").ToString <> "EXCLUDING OPENING" Then
                            _sumDebit += Val(dtrows(i).Item("DEBIT").ToString & "")
                            _sumCredit += Val(dtrows(i).Item("CREDIT").ToString & "")
                        End If

remprint:
                        If Trim(mremark) <> "" Then
                            str3 = Trim(mremark) : mremark = ""
                            If str3.Contains(vbCr) Then mremark = Trim(Mid(str3, InStr(str3, vbCr) + 1, Len(str3)))
                            If Trim(mremark) <> "" Then str3 = LSet(Mid(str3, 1, InStr(str3, vbCr) - 1), 45)
                            str1 = LSet("", 8)
                            str2 = LSet("|", 5)
                            str3 = LSet(str3, 77)
                            str4 = LSet("|", 14)
                            str5 = LSet("|", 14)
                            strprint = str3
                            strprint = Chr(27) + "@" + Chr(18) + Space(8) + "|" + Space(4) + "|" + Chr(27) + Chr(15) + strprint '+ Space(0) + "|" + Space(23) + "|"
                            FileWrite.WriteLine(strprint)
                            line += 1
                        End If

                        If mremark <> "" Then GoTo remprint
                        If line >= 58 And _strAcName = headname And i + 1 <= dtrows.Length - 1 Then
                            If _strAcName <> headname Then Continue For
                            strprint = "------------------------------------------------------------------------------------------"
                            str1 = LSet("", 8)
                            str2 = LSet("", 6)
                            str3 = LSet("Carried Over", 45)
                            str4 = RSet(_sumDebit.ToString, 14)
                            str5 = RSet(_sumCredit.ToString, 14)
                            strprint = Chr(27) + "@" + Chr(18) + str1 + str2 + str3 + str4 + str5
                            FileWrite.WriteLine(strprint)
                            strprint = "------------------------------------------------------------------------------------------"
                            FileWrite.WriteLine(strprint)
                            strprint = Chr(27) + Chr(67) + Chr(72 - (line + 5))
                            FileWrite.WriteLine(strprint)
                            strprint = Chr(12)
                            FileWrite.WriteLine(strprint)

                            Printheader(headname, CompanyName, Address1, Address2, Address3, Phone)
                        End If

                        If dtrows(i).Item("CONTRA").ToString <> "TOTAL" And dtrows(i).Item("CONTRA").ToString <> "EXCLUDING OPENING" And dtrows(i).Item("CONTRA").ToString <> "BALANCE" Then
                            str1 = LSet("", 8)
                            'str2 = LSet("|", 6)
                            str2 = LSet("|    |", 6)
                            str3 = LSet("", 45)
                            str4 = LSet("|", 14)
                            str5 = LSet("|", 14)
                            strprint = str1 + str2 + str3 + str4 + str5
                            strprint = Chr(27) + "@" + Chr(18) + strprint
                            FileWrite.WriteLine(strprint)
                            line += 1
                        End If
                    Next
                End If
                ' End If
            Next
        End If
        strprint = Chr(27) + Chr(67) + Chr(72 - (line + 5))
        FileWrite.WriteLine(strprint)
        strprint = Chr(12)
        FileWrite.WriteLine(strprint)
        'FileWrite.WriteLine("")
        'line += 1
        FileWrite.Close()
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
        'str1 = LSet("VOU NO", 8)
        'str2 = LSet("DATE", 11)
        'str3 = LSet("PARTICULARS ", 45)
        'str4 = RSet("DEBIT ", 12)
        'str5 = RSet("CREDIT ", 12)
        str1 = LSet("DATE", 8)
        str2 = LSet("", 6)
        str3 = LSet("PARTICULARS ", 45)
        str4 = RSet("DEBIT ", 14)
        str5 = RSet("CREDIT ", 14)
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

    Private Sub chkCmbAcName_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCmbAcName.SelectedIndexChanged

    End Sub

    Private Sub cmbAcName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAcName.GotFocus
        txtAcname.Visible = True
        txtAcname.Location = New Point(cmbAcName.Location.X, cmbAcName.Location.Y)
        txtAcname.Size = New Size(cmbAcName.Size.Width, cmbAcName.Size.Height)
        txtAcname.BringToFront()
        txtAcname.Select()
        'DgvSearch.BackgroundColor = Color.White
    End Sub

    Private Sub cmbAcName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbAcName.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            DgvSearch.Visible = False
            txtAcname.Visible = False
            Filldetails()
        End If
    End Sub

    Private Sub cmbAcName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAcName.LostFocus
        DgvSearch.Visible = False
        txtAcname.Visible = False
        Filldetails()
    End Sub

    Private Sub cmbLedger_GotFocus(sender As Object, e As EventArgs) Handles cmbLedger.GotFocus, chkCmbCompany.GotFocus
        DgvSearch.Visible = False
        txtAcname.Visible = False
        Filldetails()
    End Sub

    Private Sub cmbAcName_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAcName.SelectedIndexChanged
        strSql = " SELECT DISTINCT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ISNULL(ACTIVE,'Y') <> 'H' "
        strSql += " AND MACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbAcName.Text & "')"
        strSql += " ORDER BY ACNAME"
        objGPack.FillCombo(strSql, cmbLedger, , False)
        If cmbLedger.Items.Count = 0 Then cmbLedger.Enabled = False Else cmbLedger.Enabled = True
    End Sub

    Private Sub btn_MaxRb_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_MaxRb.Click
        If Ismaxdrcr Then Exit Sub
        MAXDRCR()
    End Sub


    Private Sub DgvSearch_CellPainting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellPaintingEventArgs) Handles DgvSearch.CellPainting
        If e.RowIndex >= 0 And e.ColumnIndex >= 0 Then

            e.Handled = True
            e.PaintBackground(e.CellBounds, True)
            Dim sw As String = txtAcname.Text
            If Not String.IsNullOrEmpty(sw) Then
                'highlight search word

                Dim val As String = DirectCast(e.FormattedValue, String)

                Dim sindx As Integer = val.ToLower.IndexOf(sw.ToLower)
                If sindx >= 0 Then
                    'search word found

                    Dim sf As Font = New Font(e.CellStyle.Font, FontStyle.Bold)
                    Dim sbr As SolidBrush = New SolidBrush(Color.Red)

                    Dim br As SolidBrush
                    If e.State = DataGridViewElementStates.Selected Then
                        br = New SolidBrush(e.CellStyle.SelectionForeColor)
                    Else
                        br = New SolidBrush(e.CellStyle.ForeColor)
                    End If

                    Dim sBefore As String = val.Substring(0, sindx)
                    Dim sBeforeSize As SizeF = e.Graphics.MeasureString(sBefore, e.CellStyle.Font, e.CellBounds.Size)
                    Dim sWord As String = val.Substring(sindx, sw.Length)
                    Dim sWordSize As SizeF = e.Graphics.MeasureString(sWord, sf, e.CellBounds.Size)
                    Dim sAfter As String = val.Substring(sindx + sw.Length, val.Length - (sindx + sw.Length))

                    e.Graphics.DrawString(sBefore, e.CellStyle.Font, br, e.CellBounds)
                    e.Graphics.DrawString(sWord, sf, sbr, e.CellBounds.X + sBeforeSize.Width, e.CellBounds.Location.Y)
                    e.Graphics.DrawString(sAfter, e.CellStyle.Font, br, e.CellBounds.X + sBeforeSize.Width + sWordSize.Width, e.CellBounds.Location.Y)
                Else
                    'paint as usual
                    e.PaintContent(e.CellBounds)
                End If
            Else
                'paint as usual
                e.PaintContent(e.CellBounds)
            End If
        End If
    End Sub
    Private Sub DgvSearch_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles DgvSearch.KeyDown
        If e.KeyCode = Keys.Up Then
            If DgvSearch.CurrentRow Is Nothing Then Exit Sub
            If DgvSearch.CurrentRow.Index = 0 Then
                txtAcname.Select()
            End If
        ElseIf e.KeyCode = Keys.Enter Then
            If DgvSearch.CurrentRow Is Nothing Then Exit Sub
            e.Handled = True
            cmbAcName.Text = DgvSearch.CurrentRow.Cells("ACNAME").Value.ToString
            txtAcname.Text = DgvSearch.CurrentRow.Cells("ACNAME").Value.ToString
            DgvSearch.Visible = False
            txtAcname.Select()
        End If
    End Sub

    Private Sub txtAcname_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAcname.GotFocus
        txtAcname.Text = cmbAcName.Text
        DgvSearch.Location = New Point(cmbAcName.Location.X, cmbAcName.Location.Y + cmbAcName.Height)
        DgvSearch.Size = New Size(cmbAcName.Size.Width, 150)
        DgvSearch.Columns(0).Width = cmbAcName.Size.Width
        DgvSearch.BringToFront()
    End Sub

    Private Sub txtAcname_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtAcname.KeyDown
        If e.KeyCode = Keys.Down Then
            If DgvSearch.Visible Then
                If DgvSearch.RowCount > 0 Then
                    DgvSearch.CurrentCell = DgvSearch.Rows(0).Cells(DgvSearch.FirstDisplayedCell.ColumnIndex)
                    DgvSearch.Select()
                End If
            Else
                'DownRow()
            End If
        ElseIf e.KeyCode = Keys.Up Then
            If DgvSearch.Visible Then
            Else
                'UpperRow()
            End If
        ElseIf e.KeyCode = Keys.Delete Then
            'e.Handled = True
            'Exit Sub
            'cmbAcName.Text = ""
        ElseIf e.KeyCode = Keys.Enter Then
            'KeyEnter(e)
            Exit Sub
            'e.Handled = True
        End If
    End Sub

    Private Sub txtAcountCode_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtAcountCode.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtAcountCode.Text = "" Then Exit Sub
            strSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE='" & txtAcountCode.Text & "'"
            strSql += " AND ISNULL(ACTIVE,'Y') <> 'H' "
            If cmbAcGroup.Text <> "ALL" And cmbAcGroup.Text <> "" Then
                strSql += " AND ACGRPCODE = (SELECT ACGRPCODE FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPNAME = '" & cmbAcGroup.Text & "')"
            End If
            Dim TempStrAcname As String = objGPack.GetSqlValue(strSql, "ACNAME", "")
            If TempStrAcname.ToString <> "" Then
                txtAcname.Text = TempStrAcname.ToString
                txtAcname_KeyPress(Me, e)
            End If
        End If
    End Sub

    Private Sub txtAcname_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtAcname.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            cmbAcName.Text = txtAcname.Text
            Filldetails()
            txtAcname.Visible = False
            DgvSearch.Visible = False
            Me.SelectNextControl(cmbAcName, True, True, True, True)
        End If

    End Sub

    Private Sub txtAcname_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAcname.LostFocus
        'DgvSearch.Visible = False
        cmbAcName.Text = txtAcname.Text
        txtAcname.Visible = False
    End Sub

    Private Sub txtAcname_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAcname.TextChanged
        If txtAcname.Focused = False Then Exit Sub
        If txtAcname.Text = "" Then
            DgvSearch.Visible = False
            Exit Sub
        Else
            DgvSearch.Visible = True
        End If
        Dim sw As String = txtAcname.Text
        Dim RowFilterStr As String
        RowFilterStr = "ACNAME LIKE '%" & sw & "%'"
        dtAccNames.DefaultView.RowFilter = RowFilterStr
    End Sub

    Private Sub DataGridView1_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCancelonly.CheckedChanged

    End Sub

    Private Sub MarkAndUnMarkToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub MarkUmarkAllToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MarkUmarkAllToolStripMenuItem.Click
        If gridView_OWN.Rows.Count = 0 Then Exit Sub
        For i As Integer = 0 To gridView_OWN.Rows.Count - 1
            If gridView_OWN.Rows(i).DefaultCellStyle.BackColor = Color.LightGreen Then
                gridView_OWN.Rows(i).DefaultCellStyle.BackColor = Color.White
            End If
            If gridView_OWN.Rows(i).Cells("DEBIT").Style.BackColor = Color.LightGreen Then
                If Val(gridView_OWN.Rows(i).Cells("DEBIT").Value.ToString & "") > 0 Then
                    gridView_OWN.Rows(i).Cells("DEBIT").Style.BackColor = Color.Lavender
                Else
                    gridView_OWN.Rows(i).Cells("DEBIT").Style.BackColor = Color.White
                End If
            End If
            If gridView_OWN.Rows(i).Cells("CREDIT").Style.BackColor = Color.LightGreen Then
                If Val(gridView_OWN.Rows(i).Cells("CREDIT").Value.ToString & "") > 0 Then
                    gridView_OWN.Rows(i).Cells("CREDIT").Style.BackColor = Color.LavenderBlush
                Else
                    gridView_OWN.Rows(i).Cells("CREDIT").Style.BackColor = Color.White
                End If
            End If
            If gridView_OWN.Rows(i).Cells("RUNTOTAL").Value.ToString.ToString.Contains("Dr") Then
                gridView_OWN.Rows(i).Cells("RUNTOTAL").Style.BackColor = Color.Lavender
            Else
                gridView_OWN.Rows(i).Cells("RUNTOTAL").Style.BackColor = Color.LavenderBlush
            End If
        Next
    End Sub

    ''' <summary>
    ''' CHEQUE PRINTDOCUMENT ONLY
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' 
#Region "Cheque Variable"
    Dim acpayee_x, acpayee_y, acpayee_width As Integer
    Dim payee1_x, payee1_y, payee1_width As Integer
    Dim payee2_x, payee2_y, payee2_width As Integer
    Dim amtword1_x, amtword1_y, amtword1_width As Integer
    Dim amtword2_x, amtword2_y, amtword2_width As Integer
    Dim textline1_x, textline1_y, textline1_width As Integer
    Dim textline2_x, textline2_y, textline2_width As Integer
    Dim textline3_x, textline3_y, textline3_width As Integer
    Dim notaboveamt_x, notaboveamt_y, notaboveamt_width As Integer
    Dim date_x, date_y, date_width As Integer
    Dim bearer_x, bearer_y, bearer_width As Integer
    Dim amt_x, amt_y, amt_width As Integer


    'Fontname,size,style
    Dim acpayee_fontname As String
    Dim acpayee_fontsize As Integer
    'Dim acpayee_fontstyle As String
    Dim acpayee_fontstyle As Integer

    Dim payee1_fontname, payee2_fontname As String
    Dim payee1_fontsize, payee2_fontsize As Integer
    'Dim payee1_fontstyle, payee2_fontstyle As String
    Dim payee1_fontstyle, payee2_fontstyle As Integer

    Dim amtword1_fontname, amtword2_fontname As String
    Dim amtword1_fontsize, amtword2_fontsize As Integer

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click

    End Sub


    'Dim amt1_fontstyle, amt2_fontstyle As String
    Dim amtword1_fontstyle, amtword2_fontstyle As Integer

    Dim textline1_fontname, textline2_fontname, textline3_fontname As String
    Dim textline1_fontsize, textline2_fontsize, textline3_fontsize As Integer
    'Dim txtline1_fontstyle, txtline2_fontstyle, txtline3_fontstyle As String
    Dim textline1_fontstyle, textline2_fontstyle, textline3_fontstyle As Integer

    Dim notaboveamt_fontname As String
    Dim notaboveamt_fontsize As Integer
    'Dim notaboveamt_fontstyle As String
    Dim notaboveamt_fontstyle As Integer

    Dim date_fontname As String
    Dim date_fontsize As Integer
    'Dim date_fontstyle As String
    Dim date_fontstyle As Integer

    Dim bearer_fontname As String
    Dim bearer_fontsize As Integer
    'Dim bearer_fontstyle As String
    Dim bearer_fontstyle As Integer

    Dim amt_fontname As String
    Dim amt_fontsize As Integer
    'Dim amt_fontstyle As String
    Dim amt_fontstyle As Integer

    'Active
    Dim acpayee_active As String
    Dim payee1_active, payee2_active As String
    Dim amtword1_active, amtword2_active As String
    Dim textline1_active, textline2_active, textline3_active As String
    Dim notaboveamt_active, date_active, bearer_active As String
    Dim amt_active As String

#End Region
#Region "Cheque PrintDocumnet"
    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        dt = New DataTable
        Dim Style As FontStyle
        Dim tempbyte As Byte() = Nothing
        Dim imgMemoryStream As MemoryStream = New MemoryStream()
        Dim LayoutName As String
        Dim Datevalue As String
        Dim Dateformat As String
        Dim value As Decimal

        strSql = vbCrLf + "SELECT C.LAYOUTID,C.LAYOUTNAME,C.CHQDATEFORMAT,C.CHQDATESPACE"
        strSql += vbCrLf + ",C.IMAGEPATH,CD.PARTICULARS,CD.XAXIS,CD.YAXIS"
        strSql += vbCrLf + ",CD.WIDTH,CD.FONTNAME,CD.FONTSIZE,CD.FONTSTYLE,CD.ACTIVE"
        strSql += vbCrLf + "FROM " & cnAdminDb & "..CHQLAYOUT AS C"
        strSql += vbCrLf + "INNER JOIN " & cnAdminDb & "..CHQLAYOUTDETAIL AS CD ON CD.LAYOUTID = C.LAYOUTID"
        strSql += vbCrLf + "WHERE C.LAYOUTNAME = '" & chqBankName & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)

        value = Val(chqAmtValue)
        chqAmtName1 = ConvertRupees.RupeesToWord(value, "", "").ToString


        If dt.Rows.Count > 0 Then

            LayoutName = dt.Rows(0).Item("LAYOUTNAME")
            Datevalue = dt.Rows(0).Item("CHQDATESPACE")
            Dateformat = dt.Rows(0).Item("CHQDATEFORMAT")
            tempbyte = dt.Rows(0).Item("IMAGEPATH")

            'If (IsDBNull(dt.Rows(0).Item("IMAGEPATH")) = True) Then
            '    picbxImage.Image = Nothing
            'Else
            '    If tempbyte.Length > 0 Then
            '        imgMemoryStream = New MemoryStream(tempbyte)
            '        picbxImage.Image = Drawing.Image.FromStream(imgMemoryStream)
            '        picbxImage.SizeMode = PictureBoxSizeMode.Zoom
            '    End If
            'End If

            ' Only visible Pdf
            'e.Graphics.DrawImage(picbxImage.Image, 0, 0, Picwidth, picheight)

            If Dateformat.Equals("dd/MM/YYYY", StringComparison.CurrentCultureIgnoreCase) Then
                chqDate = Replace(chqDate, "/", "/")
            ElseIf Dateformat.Equals("dd-MM-YYYY", StringComparison.CurrentCultureIgnoreCase) Then
                chqDate = Replace(chqDate, "/", "-")
            ElseIf Dateformat.Equals("dd-MM-YY", StringComparison.CurrentCultureIgnoreCase) Then
                chqDate = Replace(chqDate, "/", "-")
                chqDate = ddMMyyyyToddmmyyyy(chqDate.ToString)
            ElseIf Dateformat.Equals("dd MM YYYY", StringComparison.CurrentCultureIgnoreCase) Then
                chqDate = Replace(chqDate, "/", " ")
            End If

            acpayee_x = dt.Rows(0).Item("XAXIS")
            acpayee_y = dt.Rows(0).Item("YAXIS")
            acpayee_width = dt.Rows(0).Item("WIDTH")
            acpayee_active = dt.Rows(0).Item("ACTIVE")
            acpayee_fontname = dt.Rows(0).Item("FONTNAME")
            acpayee_fontsize = CInt(dt.Rows(0).Item("FONTSIZE"))
            acpayee_fontstyle = CInt(dt.Rows(0).Item("FONTSTYLE"))

            payee1_x = dt.Rows(1).Item("XAXIS")
            payee1_y = dt.Rows(1).Item("YAXIS")
            payee1_width = dt.Rows(1).Item("WIDTH")
            payee1_active = dt.Rows(1).Item("ACTIVE")
            payee1_fontname = dt.Rows(1).Item("FONTNAME")
            payee1_fontsize = CInt(dt.Rows(1).Item("FONTSIZE"))
            payee1_fontstyle = CInt(dt.Rows(1).Item("FONTSTYLE"))


            payee2_x = dt.Rows(2).Item("XAXIS")
            payee2_y = dt.Rows(2).Item("YAXIS")
            payee2_width = dt.Rows(2).Item("WIDTH")
            payee2_active = dt.Rows(2).Item("ACTIVE")
            payee2_fontname = dt.Rows(2).Item("FONTNAME")
            payee2_fontsize = CInt(dt.Rows(2).Item("FONTSIZE"))
            payee2_fontstyle = CInt(dt.Rows(2).Item("FONTSTYLE"))

            amtword1_x = dt.Rows(3).Item("XAXIS")
            amtword1_y = dt.Rows(3).Item("YAXIS")
            amtword1_width = dt.Rows(3).Item("WIDTH")
            amtword1_active = dt.Rows(3).Item("ACTIVE")
            amtword1_fontname = dt.Rows(3).Item("FONTNAME")
            amtword1_fontsize = CInt(dt.Rows(3).Item("FONTSIZE"))
            amtword1_fontstyle = CInt(dt.Rows(3).Item("FONTSTYLE"))

            amtword2_x = dt.Rows(4).Item("XAXIS")
            amtword2_y = dt.Rows(4).Item("YAXIS")
            amtword2_width = dt.Rows(4).Item("WIDTH")
            amtword2_active = dt.Rows(4).Item("ACTIVE")
            amtword2_fontname = dt.Rows(4).Item("FONTNAME")
            amtword2_fontsize = CInt(dt.Rows(4).Item("FONTSIZE"))
            amtword2_fontstyle = CInt(dt.Rows(4).Item("FONTSTYLE"))

            textline1_x = dt.Rows(5).Item("XAXIS")
            textline1_y = dt.Rows(5).Item("YAXIS")
            textline1_width = dt.Rows(5).Item("WIDTH")
            textline1_active = dt.Rows(5).Item("ACTIVE")
            textline1_fontname = dt.Rows(5).Item("FONTNAME")
            textline1_fontsize = CInt(dt.Rows(5).Item("FONTSIZE"))
            textline1_fontstyle = CInt(dt.Rows(5).Item("FONTSTYLE"))


            textline2_x = dt.Rows(6).Item("XAXIS")
            textline2_y = dt.Rows(6).Item("YAXIS")
            textline2_width = dt.Rows(6).Item("WIDTH")
            textline2_active = dt.Rows(6).Item("ACTIVE")
            textline2_fontname = dt.Rows(6).Item("FONTNAME")
            textline2_fontsize = CInt(dt.Rows(6).Item("FONTSIZE"))
            textline2_fontstyle = CInt(dt.Rows(6).Item("FONTSTYLE"))


            textline3_x = dt.Rows(7).Item("XAXIS")
            textline3_y = dt.Rows(7).Item("YAXIS")
            textline3_width = dt.Rows(7).Item("WIDTH")
            textline3_active = dt.Rows(7).Item("ACTIVE")
            textline3_fontname = dt.Rows(7).Item("FONTNAME")
            textline3_fontsize = CInt(dt.Rows(7).Item("FONTSIZE"))
            textline3_fontstyle = CInt(dt.Rows(7).Item("FONTSTYLE"))


            notaboveamt_x = dt.Rows(8).Item("XAXIS")
            notaboveamt_y = dt.Rows(8).Item("YAXIS")
            notaboveamt_width = dt.Rows(8).Item("WIDTH")
            notaboveamt_active = dt.Rows(8).Item("ACTIVE")
            notaboveamt_fontname = dt.Rows(8).Item("FONTNAME")
            notaboveamt_fontsize = CInt(dt.Rows(8).Item("FONTSIZE"))
            notaboveamt_fontstyle = CInt(dt.Rows(8).Item("FONTSTYLE"))


            date_x = dt.Rows(9).Item("XAXIS")
            date_y = dt.Rows(9).Item("YAXIS")
            date_width = dt.Rows(9).Item("WIDTH")
            date_active = dt.Rows(9).Item("ACTIVE")
            date_fontname = dt.Rows(9).Item("FONTNAME")
            date_fontsize = CInt(dt.Rows(9).Item("FONTSIZE"))
            date_fontstyle = CInt(dt.Rows(9).Item("FONTSTYLE"))


            bearer_x = dt.Rows(10).Item("XAXIS")
            bearer_y = dt.Rows(10).Item("YAXIS")
            bearer_width = dt.Rows(10).Item("WIDTH")
            bearer_active = dt.Rows(10).Item("ACTIVE")
            bearer_fontname = dt.Rows(10).Item("FONTNAME")
            bearer_fontsize = CInt(dt.Rows(10).Item("FONTSIZE"))
            bearer_fontstyle = CInt(dt.Rows(10).Item("FONTSTYLE"))


            amt_x = dt.Rows(11).Item("XAXIS")
            amt_y = dt.Rows(11).Item("YAXIS")
            amt_width = dt.Rows(11).Item("WIDTH")
            amt_active = dt.Rows(11).Item("ACTIVE")
            amt_fontname = dt.Rows(11).Item("FONTNAME")
            amt_fontsize = CInt(dt.Rows(11).Item("FONTSIZE"))
            amt_fontstyle = CInt(dt.Rows(11).Item("FONTSTYLE"))

            '---------------------------------------------------------------
            If acpayee_active = "Y" Then
                Style = acpayee_fontstyle
                ' e.Graphics.DrawString(chqActPayee, New Drawing.Font(acpayee_fontname, acpayee_fontsize, Style), Brushes.Black, acpayee_x, acpayee_y)
                e.Graphics.DrawString("AcPayee", New Drawing.Font(acpayee_fontname, acpayee_fontsize, Style), Brushes.Black, acpayee_x, acpayee_y)
            End If

            '---------------------------------------------------------------
            If payee1_active = "Y" Then
                Style = payee1_fontstyle
                e.Graphics.DrawString(chqPayee1, New Drawing.Font(payee1_fontname, payee1_fontsize, Style), Brushes.Black, payee1_x, payee1_y)
            End If

            '---------------------------------------------------------------
            If payee2_active = "Y" Then
                Style = payee2_fontstyle
                e.Graphics.DrawString(chqPayee2, New Drawing.Font(payee2_fontname, payee2_fontsize, Style), Brushes.Black, payee2_x, payee2_y)
            End If
            '---------------------------------------------------------------
            If amtword1_active = "Y" Then
                Style = amtword1_fontstyle
                e.Graphics.DrawString(chqAmtName1, New Drawing.Font(amtword1_fontname, amtword1_fontsize, Style), Brushes.Black, amtword1_x, amtword1_y)
            End If

            '---------------------------------------------------------------
            If amtword2_active = "Y" Then
                Style = amtword2_fontstyle
                e.Graphics.DrawString(chqAmtName2, New Drawing.Font(amtword2_fontname, amtword2_fontsize, Style), Brushes.Black, amtword2_x, amtword2_y)
            End If
            '---------------------------------------------------------------
            If textline1_active = "Y" Then
                Style = textline1_fontstyle
                e.Graphics.DrawString(chqTxtLine1, New Drawing.Font(textline1_fontname, textline1_fontsize, Style), Brushes.Black, textline1_x, textline1_y)
            End If
            '---------------------------------------------------------------
            If textline2_active = "Y" Then
                Style = textline2_fontstyle
                e.Graphics.DrawString(chqTxtLine2, New Drawing.Font(textline2_fontname, textline2_fontsize, Style), Brushes.Black, textline2_x, textline2_y)
            End If
            '---------------------------------------------------------------
            If textline3_active = "Y" Then
                Style = textline3_fontstyle
                e.Graphics.DrawString(chqTxtLine3, New Drawing.Font(textline3_fontname, textline3_fontsize, Style), Brushes.Black, textline3_x, textline3_y)
            End If
            '---------------------------------------------------------------
            If notaboveamt_active = "Y" Then
                Style = notaboveamt_fontstyle
                e.Graphics.DrawString(chqNotAbove, New Drawing.Font(notaboveamt_fontname, notaboveamt_fontsize, Style), Brushes.Black, notaboveamt_x, notaboveamt_y)
            End If
            '---------------------------------------------------------------
            If date_active = "Y" Then
                Style = date_fontstyle
                Select Case Datevalue
                    Case "1"
                        Dim s As String = chqDate.ToString
                        For i As Integer = s.Length - 1 To 1 Step -1
                            s = s.Insert(i, " ")
                            chqDate = s
                        Next
                    Case "2"
                        Dim s As String = chqDate.ToString
                        For i As Integer = s.Length - 1 To 1 Step -1
                            s = s.Insert(i, "  ")
                            chqDate = s
                        Next
                    Case "3"
                        Dim s As String = chqDate.ToString
                        For i As Integer = s.Length - 1 To 1 Step -1
                            s = s.Insert(i, "  ")
                            chqDate = s
                        Next
                    Case "4"
                        Dim s As String = chqDate.ToString
                        For i As Integer = s.Length - 1 To 1 Step -1
                            s = s.Insert(i, "   ")
                            chqDate = s
                        Next
                    Case "5"
                        Dim s As String = chqDate.ToString
                        For i As Integer = s.Length - 1 To 1 Step -1
                            s = s.Insert(i, "    ")
                            chqDate = s
                        Next
                    Case "6"
                        Dim s As String = chqDate.ToString
                        For i As Integer = s.Length - 1 To 1 Step -1
                            s = s.Insert(i, "     ")
                            chqDate = s
                        Next
                End Select
                e.Graphics.DrawString(chqDate, New Drawing.Font(date_fontname, date_fontsize, Style), Brushes.Black, date_x, date_y)

            End If
            '---------------------------------------------------------------
            If bearer_active = "Y" Then
                Style = bearer_fontstyle
                e.Graphics.DrawString(chqBearer, New Drawing.Font(bearer_fontname, bearer_fontsize, Style), Brushes.Black, bearer_x, bearer_y)
            End If
            '---------------------------------------------------------------
            If amt_active = "Y" Then
                Style = amt_fontstyle
                e.Graphics.DrawString(chqAmtValue, New Drawing.Font(amt_fontname, amt_fontsize, Style), Brushes.Black, amt_x, amt_y)
            End If
        End If
    End Sub
#End Region
#Region "Cheque Function only"
    Function ddMMyyyyToddmmyyyy(ByVal dateformat As String) As String
        Dim mm, dd, yyyy As String
        Dim join As String
        mm = dateformat.Substring(0, 3)
        dd = dateformat.Substring(3, 3)
        yyyy = dateformat.Substring(8, 2)
        join = mm & dd & yyyy
        Return join
    End Function
#End Region

    Private Sub btnSubLedger_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSubLedger.Click
        Dim _AcName As String = ""
        If cmbAcName.Text.ToString().Trim() <> "ALL" And cmbAcName.Text.ToString().Trim() <> "" Then
            _AcName = cmbAcName.Text.ToString().Trim()
        End If
        If chkCmbAcName.Text.ToString().Trim() <> "ALL" And chkCmbAcName.Text.ToString().Trim() <> "" Then
            _AcName = chkCmbAcName.Text.ToString().Trim()
        End If
        If _AcName = "" Or _AcName = "ALL" Then Exit Sub
        strSql = " SELECT TOP 1 1 FROM " & cnAdminDb & "..ACHEAD WHERE MACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & _AcName & "') "
        If Val(objGPack.GetSqlValue(strSql, , "0")) > 0 Then
            funcSubLedger()
        End If

    End Sub
    Function Filldetails()
        Dim ds As New DataSet
        lviewDetails.Items.Clear()
        strSql = "SELECT ACNAME,ADDRESS1,ADDRESS2,ADDRESS3,"
        strSql += "AREA,CITY,STATE,PINCODE,PHONENO,MOBILE,WEBSITE"
        strSql += " FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME='" & cmbAcName.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(ds, "ACNAME")
        If ds.Tables("ACNAME").Rows.Count > 0 Then
            Dim name As String = ""
            With ds.Tables("ACNAME").Rows(0)
                name = .Item("ACNAME")
                funcAddNode("Name", name)
                ' funcAddNode("",.Item("ACName").ToString)
                funcAddNode("Address", .Item("Address1").ToString)
                funcAddNode("", .Item("Address2").ToString)
                funcAddNode("", .Item("Area").ToString)
                If .Item("Pincode").ToString <> "" Then
                    funcAddNode("", .Item("City").ToString + "  -  " + .Item("Pincode").ToString)
                Else
                    funcAddNode("", .Item("City").ToString)
                End If
                funcAddNode("", .Item("State").ToString)
                ' funcAddNode("", .Item("Country").ToString)
                funcAddNode("PHONENO", .Item("PHONENO").ToString)
                funcAddNode("Mobile", .Item("Mobile").ToString)
                funcAddNode("WEBSITE", .Item("WEBSITE").ToString)
                'funcAddNode("Fax", .Item("fax").ToString)
                'funcAddNode("Joindate", .Item("Joindate").ToString)
                'funcAddNode("", "")
                'funcAddNode("Int Emp", .Item("Iemp").ToString)
                'funcAddNode("Int Groupcode", .Item("IGroupCode").ToString)
                'funcAddNode("Int RegNo", .Item("IRegno").ToString)
                'funcAddNode("Remark", .Item("Remark").ToString)
            End With
        End If

    End Function
    Function funcAddNode(ByVal name As String, ByVal des As String)
        Dim node As ListViewItem
        node = New ListViewItem(name)
        node.SubItems.Add(des)
        lviewDetails.Items.Add(node)
    End Function
    Private Sub chkCmbAcName_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles chkCmbAcName.KeyPress

    End Sub
    Private Function FunItemDetails()
        strSql = Nothing
        strSql += vbCrLf + " DECLARE @BATCHNO VARCHAR(20) = '" & gridView_OWN.CurrentRow.Cells("BATCHNO").Value.ToString & "'"
        strSql += vbCrLf + " SELECT * FROM (	"
        strSql += vbCrLf + " SELECT 	"
        strSql += vbCrLf + " 		 CONVERT(VARCHAR(30),(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID ))ITEMNAME"
        strSql += vbCrLf + " 		,ISNULL((SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = I.ITEMID),'')SUBITEMNAME"
        strSql += vbCrLf + " 		,I.PCS,SUM(I.GRSWT)GRSWT,SUM(I.NETWT)NETWT,SUM(I.LESSWT)LESSWT,CONVERT(VARCHAR(20),SUM(I.WASTAGE))WASTAGE"
        strSql += vbCrLf + " 		,SUM(I.MCGRM)MCGRM,SUM(I.STNAMT)STNAMT,SUM(I.RATE)RATE,SUM(I.AMOUNT)AMOUNT,SUM(I.TAX)TAX"
        strSql += vbCrLf + " 		,SUM(ISNULL(I.AMOUNT,0)+ISNULL(I.TAX,0)+ISNULL(I.STNAMT,0))NETAMT,I.BATCHNO,1 RESULT"
        strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE AS I "
        strSql += vbCrLf + " WHERE BATCHNO =@BATCHNO "
        strSql += vbCrLf + " GROUP BY I.BATCHNO,I.ITEMID,I.SUBITEMID,I.PCS"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT  CONVERT(VARCHAR(30),(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = R.ITEMID ))ITEMNAME"
        strSql += vbCrLf + "        ,ISNULL((SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = R.ITEMID),'')SUBITEMNAME"
        strSql += vbCrLf + "        ,PCS,GRSWT,NETWT,LESSWT,CONVERT(VARCHAR(20),WASTAGE)WASTAGE,MCGRM"
        strSql += vbCrLf + "        ,STNAMT,RATE,AMOUNT,TAX,(AMOUNT+TAX)NETAMT,BATCHNO,1 RESULT"
        strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT R WHERE BATCHNO = @BATCHNO AND TRANTYPE IN ('IIS','RPU','RRE') "
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT TOP 1 'TRAN MODE DETAILS' ITEMNAME,''SUBITEMNAME,''PCS,0 GRSWT,0 NETWT,0 LESSWT,'PAY MODE'WASTAGE"
        strSql += vbCrLf + " 		,0 MCGRM,0 STNAMT,0 RATE,0 AMOUNT,0 TAX,0 AS NETAMT,BATCHNO,2 RESULT"
        strSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = @BATCHNO AND TRANMODE = 'D'"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT  ''ITEMNAME,''SUBITEMNAME,''PCS,0 GRSWT,0 NETWT,0 LESSWT"
        strSql += vbCrLf + " 		,CASE WHEN PAYMODE='DU'THEN'DUE'WHEN PAYMODE='CA'THEN'CASE'WHEN PAYMODE='TR'THEN'MATERIAL RECEIPT'WHEN PAYMODE='TI'THEN'MATERIAL ISSUE'END WASTAGE"
        strSql += vbCrLf + " 		,0 MCGRM,0 STNAMT,0 RATE,0 AMOUNT,0 TAX,SUM(AMOUNT) AS NETAMT,BATCHNO"
        strSql += vbCrLf + " 		,CASE WHEN PAYMODE='DU'THEN 3 WHEN PAYMODE='CA'THEN 4 ELSE 3 END  RESULT"
        strSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN AS A WHERE BATCHNO = @BATCHNO AND TRANMODE = 'D' GROUP BY PAYMODE,BATCHNO"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT TOP 1 'TOTAL' ITEMNAME,''SUBITEMNAME,''PCS,0 GRSWT,0 NETWT,0 LESSWT,''WASTAGE"
        strSql += vbCrLf + " 		,0 MCGRM,0 STNAMT,0 RATE,0 AMOUNT,0 TAX,0 AS NETAMT,BATCHNO,5 RESULT"
        strSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = @BATCHNO AND TRANMODE = 'D'"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT  ''ITEMNAME,' GRAND TOTAL 'SUBITEMNAME,SUM(PCS)PCS,SUM(I.GRSWT)GRSWT,SUM(I.NETWT)NETWT"
        strSql += vbCrLf + " 		,SUM(I.LESSWT)LESSWT,CONVERT(VARCHAR(20),SUM(I.WASTAGE))WASTAGE"
        strSql += vbCrLf + " 		,SUM(I.MCGRM)MCGRM,SUM(I.STNAMT)STNAMT,SUM(I.RATE)RATE,SUM(I.AMOUNT)AMOUNT,SUM(I.TAX)TAX"
        strSql += vbCrLf + " 		,SUM(ISNULL(I.AMOUNT,0)+ISNULL(I.TAX,0)+ISNULL(I.STNAMT,0))NETAMT,I.BATCHNO,999 RESULT"
        strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE AS I WHERE BATCHNO = @BATCHNO GROUP BY I.BATCHNO"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT ' REIEPT TOTAL 'ITEMID,''SUBITEMID,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT"
        strSql += vbCrLf + "        ,SUM(LESSWT)LESSWT,CONVERT(VARCHAR(20),SUM(WASTAGE))WASTAGE,SUM(MCGRM)MCGRM"
        strSql += vbCrLf + "        ,SUM(STNAMT)STNAMT,SUM(RATE)RATE,SUM(AMOUNT)AMOUNT,SUM(TAX)TAX,SUM(AMOUNT+TAX)NETAMT,BATCHNO,999 RESULT"
        strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT WHERE BATCHNO = @BATCHNO AND TRANTYPE IN ('IIS','RPU','RRE') GROUP BY BATCHNO"
        strSql += vbCrLf + " )X ORDER BY RESULT ASC ,ITEMNAME DESC "

        Dim DtGrid As DataTable = New DataTable
        DtGrid = GetSqlTable(strSql, cn)
        If DtGrid.Rows.Count > 0 Then
            Dim objItemDetails As New frmGridDispDia
            objItemDetails.BaseName = Me.Name & "_Item Details"
            objItemDetails.Name = Me.Name
            objItemDetails.WindowState = FormWindowState.Normal
            objItemDetails.gridView.RowTemplate.Height = 18
            objItemDetails.gridView.DataSource = DtGrid
            objItemDetails.gridView.Columns("RESULT").Visible = False
            objItemDetails.gridView.Columns("BATCHNO").Visible = False

            objItemDetails.gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells

            objItemDetails.gridView.BackgroundColor = objItemDetails.BackColor
            objItemDetails.gridView.ColumnHeadersDefaultCellStyle = gridView_OWN.ColumnHeadersDefaultCellStyle
            objItemDetails.gridView.ColumnHeadersHeightSizeMode = gridView_OWN.ColumnHeadersHeightSizeMode
            objItemDetails.gridView.ColumnHeadersHeight = gridView_OWN.ColumnHeadersHeight
            objItemDetails.Text = " [Item Details View]"
            objItemDetails.lblTitle.Text = lblTitle.Text + "[Ledger Item Details View]]"
            objItemDetails.lblTitle.Text = lblTitle.Text + Environment.NewLine
            objItemDetails.StartPosition = FormStartPosition.CenterScreen
            objItemDetails.Show()

            objItemDetails.gridView.Columns("RATE").DefaultCellStyle.Format = "#,##0.00"
            objItemDetails.gridView.Columns("TAX").DefaultCellStyle.Format = "#,##0.00"
            objItemDetails.gridView.Columns("AMOUNT").DefaultCellStyle.Format = "#,##0.00"
            objItemDetails.gridView.Columns("NETAMT").DefaultCellStyle.Format = "#,##0.00"
            objItemDetails.gridView.Columns("STNAMT").DefaultCellStyle.Format = "#,##0.00"

            objItemDetails.gridView.Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            objItemDetails.gridView.Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            objItemDetails.gridView.Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            objItemDetails.gridView.Columns("LESSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            objItemDetails.gridView.Columns("WASTAGE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            objItemDetails.gridView.Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            objItemDetails.gridView.Columns("TAX").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            objItemDetails.gridView.Columns("STNAMT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            objItemDetails.gridView.Columns("NETAMT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            objItemDetails.gridView.Columns("MCGRM").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            objItemDetails.gridView.Columns("RATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            For i As Integer = 0 To objItemDetails.gridView.Rows.Count - 1
                If Val(objItemDetails.gridView.Rows(i).Cells("RESULT").Value.ToString) = 2 Then
                    objItemDetails.gridView.Rows(i).DefaultCellStyle.ForeColor = Color.Blue
                    objItemDetails.gridView.Rows(i).DefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
                ElseIf Val(objItemDetails.gridView.Rows(i).Cells("RESULT").Value.ToString) = 3 Then
                    objItemDetails.gridView.Rows(i).DefaultCellStyle.BackColor = Color.Lavender
                    objItemDetails.gridView.Rows(i).DefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
                ElseIf Val(objItemDetails.gridView.Rows(i).Cells("RESULT").Value.ToString) = 4 Then
                    objItemDetails.gridView.Rows(i).DefaultCellStyle.BackColor = Color.Lavender
                    objItemDetails.gridView.Rows(i).DefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
                ElseIf Val(objItemDetails.gridView.Rows(i).Cells("RESULT").Value.ToString) = 5 Then
                    objItemDetails.gridView.Rows(i).DefaultCellStyle.ForeColor = Color.Blue
                    objItemDetails.gridView.Rows(i).DefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
                ElseIf Val(objItemDetails.gridView.Rows(i).Cells("RESULT").Value.ToString) = 999 Then
                    objItemDetails.gridView.Rows(i).DefaultCellStyle.BackColor = Color.LavenderBlush
                    objItemDetails.gridView.Rows(i).DefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
                End If


                If Val(objItemDetails.gridView.Rows(i).Cells("PCS").Value.ToString) = 0 Then
                    objItemDetails.gridView.Rows(i).Cells("PCS").Value = DBNull.Value
                End If
                If Val(objItemDetails.gridView.Rows(i).Cells("GRSWT").Value.ToString) = 0 Then
                    objItemDetails.gridView.Rows(i).Cells("GRSWT").Value = DBNull.Value
                End If
                If Val(objItemDetails.gridView.Rows(i).Cells("NETWT").Value.ToString) = 0 Then
                    objItemDetails.gridView.Rows(i).Cells("NETWT").Value = DBNull.Value
                End If
                If Val(objItemDetails.gridView.Rows(i).Cells("LESSWT").Value.ToString) = 0 Then
                    objItemDetails.gridView.Rows(i).Cells("LESSWT").Value = DBNull.Value
                End If
                If Val(objItemDetails.gridView.Rows(i).Cells("STNAMT").Value.ToString) = 0 Then
                    objItemDetails.gridView.Rows(i).Cells("STNAMT").Value = DBNull.Value
                End If
                If Val(objItemDetails.gridView.Rows(i).Cells("MCGRM").Value.ToString) = 0 Then
                    objItemDetails.gridView.Rows(i).Cells("MCGRM").Value = DBNull.Value
                End If
                If Val(objItemDetails.gridView.Rows(i).Cells("RATE").Value.ToString) = 0 Then
                    objItemDetails.gridView.Rows(i).Cells("RATE").Value = DBNull.Value
                End If
                If Val(objItemDetails.gridView.Rows(i).Cells("AMOUNT").Value.ToString) = 0 Then
                    objItemDetails.gridView.Rows(i).Cells("AMOUNT").Value = DBNull.Value
                End If
                If Val(objItemDetails.gridView.Rows(i).Cells("TAX").Value.ToString) = 0 Then
                    objItemDetails.gridView.Rows(i).Cells("TAX").Value = DBNull.Value
                End If
                If Val(objItemDetails.gridView.Rows(i).Cells("NETAMT").Value.ToString) = 0 Then
                    objItemDetails.gridView.Rows(i).Cells("NETAMT").Value = DBNull.Value
                End If
            Next
        End If
    End Function


End Class


Public Class frmAccountsLedger_Properties
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
    Private chkDiscEmpName As Boolean = False
    Public Property p_chkDiscEmpName() As Boolean
        Get
            Return chkDiscEmpName
        End Get
        Set(ByVal value As Boolean)
            chkDiscEmpName = value
        End Set
    End Property
    Private rdbWithBillPrefix As Boolean = False
    Public Property p_rdbWithBillPrefix() As Boolean
        Get
            Return rdbWithBillPrefix
        End Get
        Set(ByVal value As Boolean)
            rdbWithBillPrefix = value
        End Set
    End Property
    Private chkEmp As Boolean = False
    Public Property p_chkEmp() As Boolean
        Get
            Return chkEmp
        End Get
        Set(ByVal value As Boolean)
            chkEmp = value
        End Set
    End Property
End Class