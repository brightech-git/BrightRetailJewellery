Imports System.Data.OleDb
Public Class frmTradingNew
    'Calno 560 :Alter by vasanth : Multiple costcentre By cloumn wise
    'CALNO 290414 : Alter by vasanth : Exchange negative value
    Dim strSql As String = ""
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Dim dt As DataTable
    Dim dtCostCentre As DataTable
    Dim dtCompany As DataTable
    Dim DtCostid As DataTable

    Dim Rempty As Boolean = False
    Dim MultiCostcentre As Boolean = IIf(GetAdmindbSoftValue("CCWISE_FINRPT", "N", ) = "Y", True, False)
    Dim Closin_stk As String = GetAdmindbSoftValue("CLSSTK_POST_PL", "L", )
    Dim CnStartMonth As String = "04"

    Public Sub New()

    End Sub

    Private Sub frmTrading_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmTrading_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.WindowState = FormWindowState.Maximized


        ''COSTCENTRE
        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False

        strSql = " SELECT COMPANYNAME,COMPANYID,2 RESULT FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N' "
        strSql += " ORDER BY RESULT,COMPANYNAME"
        dtCompany = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCompany)
        BrighttechPack.GlobalMethods.FillCombo(cmbCompany, dtCompany, "COMPANYNAME", True)
        btnView.Focus()
        chkTrading.Checked = True
        ChkPandL.Checked = True
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub funcViewMulCost(ByVal strcompid As String, ByVal strCostId As String, ByVal summary As Boolean)

        dt = New DataTable()
        If chkTrading.Checked Then funcTradingMulCost(strcompid, strCostId, summary)
        If ChkPandL.Checked Then funcPLMulCost(strcompid, strCostId, summary)
        gridView.DataSource = dt

        If gridView.RowCount > 0 Then
            gridView.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 9, FontStyle.Bold)
            gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            gridView.Font = New Font("VERDANA", 7, FontStyle.Regular)
            For I As Integer = 0 To DtCostid.Rows.Count - 1
                gridView.Columns("EXPACGROUPNAME1" & DtCostid.Rows(I).Item(0).ToString.Trim).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                gridView.Columns("EXPACGROUPNAME1" & DtCostid.Rows(I).Item(0).ToString.Trim).HeaderText = "PARTICULARS"
                gridView.Columns("EXPACGROUPNAME1" & DtCostid.Rows(I).Item(0).ToString.Trim).DefaultCellStyle.Font = New Font("VERDANA", 7, FontStyle.Regular)
                gridView.Columns("EXPACGROUPNAME1" & DtCostid.Rows(I).Item(0).ToString.Trim).Width = 202
                gridView.Columns("EXPENSES" & DtCostid.Rows(I).Item(0).ToString.Trim).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                gridView.Columns("EXPENSES" & DtCostid.Rows(I).Item(0).ToString.Trim).HeaderText = "AMOUNT"
                gridView.Columns("EXPENSES" & DtCostid.Rows(I).Item(0).ToString.Trim).Width = 100
                If summary Then
                    gridView.Columns("EXPENSES1" & DtCostid.Rows(I).Item(0).ToString.Trim).Visible = False
                    gridView.Columns("EXPENSES2" & DtCostid.Rows(I).Item(0).ToString.Trim).Visible = False
                Else
                    gridView.Columns("EXPENSES1" & DtCostid.Rows(I).Item(0).ToString.Trim).Visible = True
                    gridView.Columns("EXPENSES2" & DtCostid.Rows(I).Item(0).ToString.Trim).Visible = True
                    gridView.Columns("EXPENSES1" & DtCostid.Rows(I).Item(0).ToString.Trim).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    gridView.Columns("EXPENSES1" & DtCostid.Rows(I).Item(0).ToString.Trim).HeaderText = "AMOUNT"
                    gridView.Columns("EXPENSES1" & DtCostid.Rows(I).Item(0).ToString.Trim).Width = 100
                    gridView.Columns("EXPENSES2" & DtCostid.Rows(I).Item(0).ToString.Trim).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    gridView.Columns("EXPENSES2" & DtCostid.Rows(I).Item(0).ToString.Trim).HeaderText = "AMOUNT"
                    gridView.Columns("EXPENSES2" & DtCostid.Rows(I).Item(0).ToString.Trim).Width = 100
                End If
                gridView.Columns("INCACGROUPNAME1" & DtCostid.Rows(I).Item(0).ToString.Trim).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                gridView.Columns("INCACGROUPNAME1" & DtCostid.Rows(I).Item(0).ToString.Trim).HeaderText = "PARTICULARS"
                gridView.Columns("INCACGROUPNAME1" & DtCostid.Rows(I).Item(0).ToString.Trim).Width = 160
                gridView.Columns("EXPACGROUPNAME1" & DtCostid.Rows(I).Item(0).ToString.Trim).DefaultCellStyle.Font = New Font("VERDANA", 7, FontStyle.Regular)
                gridView.Columns("INCOME" & DtCostid.Rows(I).Item(0).ToString.Trim).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                gridView.Columns("INCOME" & DtCostid.Rows(I).Item(0).ToString.Trim).Width = 100
                gridView.Columns("INCOME" & DtCostid.Rows(I).Item(0).ToString.Trim).HeaderText = "AMOUNT"
                If summary Then
                    gridView.Columns("INCOME1" & DtCostid.Rows(I).Item(0).ToString.Trim).Visible = False
                    gridView.Columns("INCOME2" & DtCostid.Rows(I).Item(0).ToString.Trim).Visible = False
                Else
                    gridView.Columns("INCOME1" & DtCostid.Rows(I).Item(0).ToString.Trim).Visible = True
                    gridView.Columns("INCOME1" & DtCostid.Rows(I).Item(0).ToString.Trim).Visible = True
                    gridView.Columns("INCOME1" & DtCostid.Rows(I).Item(0).ToString.Trim).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    gridView.Columns("INCOME1" & DtCostid.Rows(I).Item(0).ToString.Trim).Width = 100
                    gridView.Columns("INCOME1" & DtCostid.Rows(I).Item(0).ToString.Trim).HeaderText = "AMOUNT"
                    gridView.Columns("INCOME2" & DtCostid.Rows(I).Item(0).ToString.Trim).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    gridView.Columns("INCOME2" & DtCostid.Rows(I).Item(0).ToString.Trim).Width = 100
                    gridView.Columns("INCOME2" & DtCostid.Rows(I).Item(0).ToString.Trim).HeaderText = "AMOUNT"
                End If
                gridView.Columns("COLHEAD3" & DtCostid.Rows(I).Item(0).ToString.Trim).Visible = False
                gridView.Columns("COLHEAD4" & DtCostid.Rows(I).Item(0).ToString.Trim).Visible = False
            Next

            For M As Integer = 0 To DtCostid.Rows.Count - 1
                For i As Integer = 0 To gridView.RowCount - 1
                    If gridView.Rows(i).Cells("COLHEAD3" & DtCostid.Rows(M).Item(0).ToString.Trim).Value.ToString() = "T" Then
                        gridView.Rows(i).Cells("EXPACGROUPNAME1" & DtCostid.Rows(M).Item(0).ToString.Trim).Style.BackColor = Color.LightBlue
                        gridView.Rows(i).Cells("EXPACGROUPNAME1" & DtCostid.Rows(M).Item(0).ToString.Trim).Style.ForeColor = Color.Blue
                        gridView.Rows(i).Cells("EXPACGROUPNAME1" & DtCostid.Rows(M).Item(0).ToString.Trim).Style.Font = New Font("VERDANA", 7, FontStyle.Bold)
                    ElseIf gridView.Rows(i).Cells("COLHEAD3" & DtCostid.Rows(M).Item(0).ToString.Trim).Value.ToString() = "T1" OrElse gridView.Rows(i).Cells("COLHEAD3" & DtCostid.Rows(M).Item(0).ToString.Trim).Value.ToString() = "T2" Then
                        gridView.Rows(i).Cells("EXPACGROUPNAME1" & DtCostid.Rows(M).Item(0).ToString.Trim).Style.Font = New Font("VERDANA", 7, FontStyle.Bold)
                        gridView.Rows(i).Cells("EXPENSES" & DtCostid.Rows(M).Item(0).ToString.Trim).Style.Font = New Font("VERDANA", 7, FontStyle.Bold)
                    ElseIf gridView.Rows(i).Cells("COLHEAD3" & DtCostid.Rows(M).Item(0).ToString.Trim).Value.ToString() = "S" OrElse gridView.Rows(i).Cells("COLHEAD3" & DtCostid.Rows(M).Item(0).ToString.Trim).Value.ToString() = "S1" OrElse gridView.Rows(i).Cells("COLHEAD3" & DtCostid.Rows(M).Item(0).ToString.Trim).Value.ToString() = "S2" Then
                        gridView.Rows(i).Cells("EXPENSES" & DtCostid.Rows(M).Item(0).ToString.Trim).Style.Font = New Font("VERDANA", 7, FontStyle.Bold)
                        gridView.Rows(i).Cells("EXPENSES1" & DtCostid.Rows(M).Item(0).ToString.Trim).Style.Font = New Font("VERDANA", 7, FontStyle.Bold)
                        gridView.Rows(i).Cells("EXPENSES2" & DtCostid.Rows(M).Item(0).ToString.Trim).Style.Font = New Font("VERDANA", 7, FontStyle.Bold)
                    ElseIf gridView.Rows(i).Cells("COLHEAD3" & DtCostid.Rows(M).Item(0).ToString.Trim).Value.ToString() = "G" Then
                        If Convert.ToDouble((IIf(gridView.Rows(i).Cells("EXPENSES" & DtCostid.Rows(M).Item(0).ToString.Trim).Value.ToString() <> "", gridView.Rows(i).Cells("EXPENSES" & DtCostid.Rows(M).Item(0).ToString.Trim).Value.ToString(), "0"))) <> 0 Then
                            gridView.Rows(i).Cells("EXPACGROUPNAME1" & DtCostid.Rows(M).Item(0).ToString.Trim).Style.BackColor = Color.LavenderBlush
                            gridView.Rows(i).Cells("EXPACGROUPNAME1" & DtCostid.Rows(M).Item(0).ToString.Trim).Style.Font = New Font("VERDANA", 7, FontStyle.Bold)
                            gridView.Rows(i).Cells("EXPENSES" & DtCostid.Rows(M).Item(0).ToString.Trim).Style.BackColor = Color.LavenderBlush
                            gridView.Rows(i).Cells("EXPENSES" & DtCostid.Rows(M).Item(0).ToString.Trim).Style.Font = New Font("VERDANA", 7, FontStyle.Bold)
                            gridView.Rows(i).Cells("EXPENSES1" & DtCostid.Rows(M).Item(0).ToString.Trim).Style.Font = New Font("VERDANA", 7, FontStyle.Bold)
                            gridView.Rows(i).Cells("EXPENSES2" & DtCostid.Rows(M).Item(0).ToString.Trim).Style.Font = New Font("VERDANA", 7, FontStyle.Bold)
                        End If
                    ElseIf gridView.Rows(i).Cells("COLHEAD3" & DtCostid.Rows(M).Item(0).ToString.Trim).Value.ToString() = "G1" OrElse gridView.Rows(i).Cells("COLHEAD3" & DtCostid.Rows(M).Item(0).ToString.Trim).Value.ToString() = "G2" Then
                        gridView.Rows(i).Cells("EXPENSES" & DtCostid.Rows(M).Item(0).ToString.Trim).Style.Font = New Font("VERDANA", 7, FontStyle.Bold)
                        gridView.Rows(i).Cells("EXPENSES1" & DtCostid.Rows(M).Item(0).ToString.Trim).Style.Font = New Font("VERDANA", 7, FontStyle.Bold)
                        gridView.Rows(i).Cells("EXPENSES2" & DtCostid.Rows(M).Item(0).ToString.Trim).Style.Font = New Font("VERDANA", 7, FontStyle.Bold)
                    End If


                    If gridView.Rows(i).Cells("COLHEAD4" & DtCostid.Rows(M).Item(0).ToString.Trim).Value.ToString() = "T" Then
                        'gridView.Rows[i].Cells["INCACGROUPNAME1"].Style.BackColor = Color.LightBlue;
                        'gridView.Rows[i].Cells["INCACGROUPNAME1"].Style.ForeColor = Color.Blue;
                        gridView.Rows(i).Cells("INCACGROUPNAME1" & DtCostid.Rows(M).Item(0).ToString.Trim).Style.Font = New Font("VERDANA", 7, FontStyle.Bold)
                    ElseIf gridView.Rows(i).Cells("COLHEAD4" & DtCostid.Rows(M).Item(0).ToString.Trim).Value.ToString() = "T1" OrElse gridView.Rows(i).Cells("COLHEAD4" & DtCostid.Rows(M).Item(0).ToString.Trim).Value.ToString() = "T2" Then
                        gridView.Rows(i).Cells("INCACGROUPNAME1" & DtCostid.Rows(M).Item(0).ToString.Trim).Style.Font = New Font("VERDANA", 7, FontStyle.Bold)
                        gridView.Rows(i).Cells("INCOME" & DtCostid.Rows(M).Item(0).ToString.Trim).Style.Font = New Font("VERDANA", 7, FontStyle.Bold)
                    ElseIf gridView.Rows(i).Cells("COLHEAD4" & DtCostid.Rows(M).Item(0).ToString.Trim).Value.ToString() = "S" OrElse gridView.Rows(i).Cells("COLHEAD4" & DtCostid.Rows(M).Item(0).ToString.Trim).Value.ToString() = "S1" OrElse gridView.Rows(i).Cells("COLHEAD4" & DtCostid.Rows(M).Item(0).ToString.Trim).Value.ToString() = "S2" Then
                        gridView.Rows(i).Cells("INCOME" & DtCostid.Rows(M).Item(0).ToString.Trim).Style.Font = New Font("VERDANA", 7, FontStyle.Bold)
                        gridView.Rows(i).Cells("INCOME1" & DtCostid.Rows(M).Item(0).ToString.Trim).Style.Font = New Font("VERDANA", 7, FontStyle.Bold)
                        gridView.Rows(i).Cells("INCOME2" & DtCostid.Rows(M).Item(0).ToString.Trim).Style.Font = New Font("VERDANA", 7, FontStyle.Bold)
                    ElseIf gridView.Rows(i).Cells("COLHEAD4" & DtCostid.Rows(M).Item(0).ToString.Trim).Value.ToString() = "G" Then
                        If Convert.ToDouble((IIf(gridView.Rows(i).Cells("INCOME" & DtCostid.Rows(M).Item(0).ToString.Trim).Value.ToString() <> "", gridView.Rows(i).Cells("INCOME" & DtCostid.Rows(M).Item(0).ToString.Trim).Value.ToString(), "0"))) <> 0 Then
                            gridView.Rows(i).Cells("INCACGROUPNAME1" & DtCostid.Rows(M).Item(0).ToString.Trim).Style.BackColor = Color.LavenderBlush
                            gridView.Rows(i).Cells("INCACGROUPNAME1" & DtCostid.Rows(M).Item(0).ToString.Trim).Style.Font = New Font("VERDANA", 7, FontStyle.Bold)
                            gridView.Rows(i).Cells("INCOME" & DtCostid.Rows(M).Item(0).ToString.Trim).Style.BackColor = Color.LavenderBlush
                            gridView.Rows(i).Cells("INCOME" & DtCostid.Rows(M).Item(0).ToString.Trim).Style.Font = New Font("VERDANA", 7, FontStyle.Bold)
                            gridView.Rows(i).Cells("INCOME1" & DtCostid.Rows(M).Item(0).ToString.Trim).Style.Font = New Font("VERDANA", 7, FontStyle.Bold)
                            gridView.Rows(i).Cells("INCOME2" & DtCostid.Rows(M).Item(0).ToString.Trim).Style.Font = New Font("VERDANA", 7, FontStyle.Bold)
                        End If
                    ElseIf gridView.Rows(i).Cells("COLHEAD4" & DtCostid.Rows(M).Item(0).ToString.Trim).Value.ToString() = "G1" OrElse gridView.Rows(i).Cells("COLHEAD4" & DtCostid.Rows(M).Item(0).ToString.Trim).Value.ToString() = "G2" Then
                        gridView.Rows(i).Cells("INCOME" & DtCostid.Rows(M).Item(0).ToString.Trim).Style.Font = New Font("VERDANA", 7, FontStyle.Bold)
                        gridView.Rows(i).Cells("INCOME1" & DtCostid.Rows(M).Item(0).ToString.Trim).Style.Font = New Font("VERDANA", 7, FontStyle.Bold)
                        gridView.Rows(i).Cells("INCOME2" & DtCostid.Rows(M).Item(0).ToString.Trim).Style.Font = New Font("VERDANA", 7, FontStyle.Bold)
                    End If
                Next
            Next
            lblTitle.Text = cmbCompany.Text & "TRADING & PROFIT AND LOSS FOR THE PERIOD OF - 01/" & CnStartMonth & "/" & (IIf(dtpDate.Value.Date.Month > 3, dtpDate.Value.Date.Year, dtpDate.Value.Date.Year - 1)).ToString() & " - " & dtpDate.Text.Replace("-", "/")
            If chkCmbCostCentre.Text <> "" Then lblTitle.Text = lblTitle.Text & " COST CENTRE :" & chkCmbCostCentre.Text
            gridView.Focus()
        Else
            Rempty = True
            MessageBox.Show("Message", "Records not found...", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            dtpDate.Focus()
        End If
    End Sub
    Private Sub funcTradingMulCost(ByVal strcompid As String, ByVal strCostId As String, ByVal summary As Boolean)

        strSql = "  IF EXISTS (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & userId.ToString() & "TRADE1')"
        strSql += vbCrLf + "  DROP TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE1"
        strSql += vbCrLf + "  CREATE TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE1"
        strSql += vbCrLf + "  (COMPANYID VARCHAR(3),COSTID VARCHAR(3),ACID VARCHAR(25),TRANMODE VARCHAR(1),AMOUNT NUMERIC(15,2),DEBIT NUMERIC(15,2),CREDIT NUMERIC(15,2)"
        strSql += vbCrLf + "  ,TRANDATE SMALLDATETIME,PAYMODE VARCHAR(20),TRANNO VARCHAR(25),SEP VARCHAR(1))"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE1"
        strSql += vbCrLf + "  (COMPANYID,COSTID,ACID,TRANMODE,AMOUNT,DEBIT,CREDIT,TRANDATE,PAYMODE,TRANNO,SEP)"
        strSql += vbCrLf + "  SELECT COMPANYID,ISNULL(COSTID,'')COSTID,ACCODE ACID,CASE WHEN ISNULL(DEBIT,0) > ISNULL(CREDIT,0) THEN 'D' ELSE 'C' END TRANMODE"
        strSql += vbCrLf + "  ,CASE WHEN ISNULL(DEBIT,0) <> 0 THEN DEBIT ELSE CREDIT END AMOUNT"
        strSql += vbCrLf + "  ,DEBIT,CREDIT,NULL TRANDATE,'' PAYMODE,NULL TRANNO,'O' SEP"
        strSql += vbCrLf + "  FROM " & cnStockDb & "..OPENTRAILBALANCE WHERE ISNULL(COMPANYID,'') = '" & strcompid & "'"
        strSql += strCostId
        strSql += vbCrLf + "  UNION ALL"
        strSql += vbCrLf + "  SELECT COMPANYID,ISNULL(COSTID,'')COSTID,ACCODE ACID,TRANMODE,AMOUNT"
        strSql += vbCrLf + "  ,CASE WHEN TRANMODE = 'D' THEN AMOUNT END DEBIT,CASE WHEN TRANMODE = 'C' THEN AMOUNT END CREDIT"
        strSql += vbCrLf + "  ,TRANDATE,PAYMODE,TRANNO,'T' SEP"
        strSql += vbCrLf + "  FROM " & cnStockDb & "..ACCTRAN WHERE ISNULL(COMPANYID,'') = '" & strcompid & "' AND TRANDATE >= '" & cnTranFromDate & "' AND TRANDATE <= '" & dtpDate.Value.Date.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') != 'Y'"
        strSql += strCostId
        'strSql += vbCrLf + " UNION ALL"
        'strSql += vbCrLf + " (COMPANYID,COSTID,CATCODE,CLOSING,RATE,VALUE,UPDATED,UPTIME)"
        'strSql += vbCrLf + " SELECT"
        'strSql += vbCrLf + " COMPANYID,'CL"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "  IF EXISTS (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & userId.ToString() & "TRADE2')"
        strSql += vbCrLf + "  DROP TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2"
        If summary Then
            strSql += vbCrLf + "  SELECT '' ACID,COSTID,G.ACGRPCODE,SG.ACGRPNAME ACGRPNAME,G.ACMAINCODE,SG.GRPLEDGER,SG.GRPTYPE"
            'strSql += vbCrLf + "   A.ACCODE ACID,G.ACGRPCODE,ACGRPNAME ACGRPNAME,ACMAINCODE,G.GRPLEDGER,G.GRPTYPE"
            strSql += vbCrLf + "  ,SUM(DEBIT) DEBIT,SUM(CREDIT) CREDIT"
            strSql += vbCrLf + "  ,'' ACNAME,'' TRANDATE,CASE WHEN SUM(AMOUNT) > 0 THEN 'D' ELSE 'C' END TRANMODE,SUM(AMOUNT) AMOUNT,'' PAYMODE,'' TRANNO"
            strSql += vbCrLf + "  ,(SELECT ACGRPNAME FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPCODE = G.ACMAINCODE) ACMAINGRPNAME"
            strSql += vbCrLf + "  INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2"
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE1 AS T INNER JOIN " & cnAdminDb & "..ACHEAD AS A"
            strSql += vbCrLf + "  ON T.ACID = A.ACCODE INNER JOIN " & cnAdminDb & "..ACGROUP AS G ON G.ACGRPCODE = A.ACGRPCODE"
            strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ACGROUP AS SG ON SG.ACGRPCODE = A.ACGRPCODE"
            strSql += vbCrLf + "  AND (SG.GRPLEDGER IN ('T') OR G.GRPLEDGER IN ('T')) "
            strSql += vbCrLf + "  GROUP BY G.ACGRPCODE,SG.ACGRPNAME,G.ACMAINCODE,SG.GRPLEDGER,SG.GRPTYPE,COSTID"
            If rbtCls_Stock_Man.Checked Then  'Val(objGPack.GetSqlValue("SELECT COUNT(*) FROM " & cnStockDb & "..CLOSINGVAL WHERE 1=1 " & strCostId, , , )) > 0 Then
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT 'CLSTK' ACID,COSTID,'0' ACGRPCODE,'CLOSING STOCK' ACGRPNAME,'0' ACMAINCODE "
                strSql += vbCrLf + " ,'T' GRPLEDGER,CASE WHEN SUM(VALUE) > 0 THEN 'I' ELSE 'E' END GRPTYPE "
                strSql += vbCrLf + " ,CASE WHEN SUM(VALUE) < 0 THEN SUM(VALUE)*(-1) END DEBIT"
                strSql += vbCrLf + " ,CASE WHEN SUM(VALUE) > 0 THEN SUM(VALUE) END CREDIT "
                strSql += vbCrLf + " ,'CLOSING STOCK' ACNAME,'' TRANDATE "
                strSql += vbCrLf + " ,CASE WHEN SUM(VALUE) > 0 THEN 'D' ELSE 'C' END TRANMODE,SUM(VALUE)AMOUNT "
                strSql += vbCrLf + " ,'' PAYMODE,'' TRANNO,'CLOSING STOCK' ACMAINGRPNAME "
                strSql += vbCrLf + " FROM " & cnStockDb & "..CLOSINGVAL A "
                strSql += vbCrLf + " WHERE ISNULL(A.COMPANYID,'') = '" & strcompid & "'"
                strSql += vbCrLf + strCostId + " GROUP BY COSTID"
                strSql += vbCrLf + " HAVING SUM(VALUE) <> 0"
            Else
                For K As Integer = 0 To DtCostid.Rows.Count - 1
                    Dim Qry As String = ""
                    ProgressBarShow()
                    If rbtCls_Stock_AvgRate.Checked Then
                        ProgressBarStep("Closing Stock arrival Weighted avg method", 10)
                        Qry = " EXEC " & cnAdminDb & "..SP_CLOSING_STOCK"
                    Else
                        ProgressBarStep("Closing Stock arrival LIFO method", 10)
                        Qry = " EXEC " & cnAdminDb & "..SP_CLOSINGSTOCK_LIFO"
                    End If
                    Qry += vbCrLf + " @DBNAME = '" & cnStockDb & "',@ADMINDB = '" & cnAdminDb & "'"
                    Qry += vbCrLf + " ,@FRMDATE = '" & Format(dtpDate.Value, "yyyy-MM-dd") & "'"
                    Qry += vbCrLf + " ,@TODATE = '" & Format(dtpDate.Value, "yyyy-MM-dd") & "'"
                    Qry += vbCrLf + " ,@METALNAME = 'ALL',@CATCODE = 'ALL',@CATNAME = 'ALL'"
                    Qry += vbCrLf + " ,@COSTNAME = '" & DtCostid.Rows(K).Item(1).ToString.Trim & "'"
                    Qry += vbCrLf + " ,@COMPANYID = '" & strcompid & "',@RPTTYPE = 'S'"
                    Dim SYSTEMID As String = Trim(LSet(Mid(Guid.NewGuid.ToString, 1, InStr(Guid.NewGuid.ToString, "-") - 1), 10))
                    Qry += vbCrLf + " ,@SYSTEMID = '" & SYSTEMID & "'"
                    cmd = New OleDbCommand(Qry, cn)
                    cmd.CommandTimeout = 2000
                    cmd.ExecuteNonQuery()
                    Qry = "  IF EXISTS (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMPCSTKVALUE4" & DtCostid.Rows(K).Item(0).ToString.Trim & "')"
                    Qry += vbCrLf + "  DROP TABLE TEMPTABLEDB..TEMPCSTKVALUE4" & DtCostid.Rows(K).Item(0).ToString.Trim & ""
                    Qry += vbCrLf + "  SELECT *,'" & DtCostid.Rows(K).Item(0).ToString.Trim & "' COSTID INTO TEMPTABLEDB..TEMPCSTKVALUE4" & DtCostid.Rows(K).Item(0).ToString.Trim & " FROM TEMPTABLEDB..TEMPCSTKVALUE4" & SYSTEMID & ""
                    cmd = New OleDbCommand(Qry, cn)
                    cmd.ExecuteNonQuery()

                    strSql += vbCrLf + " UNION ALL"
                    strSql += vbCrLf + " SELECT 'CLSTK' ACID,COSTID,'0' ACGRPCODE,'CLOSING STOCK' ACGRPNAME,'1' ACMAINCODE"
                    strSql += vbCrLf + " ,'T' GRPLEDGER,CASE WHEN SUM(CAMOUNT) > 0 THEN 'I' ELSE 'E' END GRPTYPE "
                    strSql += vbCrLf + " ,CASE WHEN SUM(CAMOUNT) < 0 THEN SUM(CAMOUNT)*(-1) END DEBIT "
                    strSql += vbCrLf + " ,CASE WHEN SUM(CAMOUNT) > 0 THEN SUM(CAMOUNT) END CREDIT "
                    strSql += vbCrLf + " ,'CLOSING STOCK' ACNAME,'' TRANDATE ,CASE WHEN SUM(CAMOUNT) > 0 THEN 'D' ELSE 'C' END TRANMODE,SUM(CAMOUNT) AMOUNT ,'' PAYMODE,'' TRANNO,'CLOSING STOCK' ACMAINGRPNAME "
                    strSql += vbCrLf + " FROM TEMPTABLEDB..TEMPCSTKVALUE4" & DtCostid.Rows(K).Item(0).ToString.Trim & " A "
                    strSql += vbCrLf + " WHERE A.RESULT=1"
                    strSql += vbCrLf + " GROUP BY COSTID"
                    strSql += vbCrLf + " HAVING SUM(CAMOUNT) <> 0"
                Next

            End If
        Else
            strSql += vbCrLf + "  SELECT A.ACCODE ACID,COSTID,G.ACGRPCODE,SG.ACGRPNAME ACGRPNAME,G.ACMAINCODE,SG.GRPLEDGER,SG.GRPTYPE"
            'strSql += vbCrLf + "   A.ACCODE ACID,G.ACGRPCODE,ACGRPNAME ACGRPNAME,ACMAINCODE,G.GRPLEDGER,G.GRPTYPE"
            strSql += vbCrLf + "  ,DEBIT,CREDIT,ACNAME ACNAME,TRANDATE,TRANMODE,AMOUNT,PAYMODE,TRANNO"
            strSql += vbCrLf + "  ,(SELECT ACGRPNAME FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPCODE = G.ACMAINCODE) ACMAINGRPNAME"
            strSql += vbCrLf + "  INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2"
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE1 AS T INNER JOIN " & cnAdminDb & "..ACHEAD AS A"
            strSql += vbCrLf + "  ON T.ACID = A.ACCODE INNER JOIN " & cnAdminDb & "..ACGROUP AS G ON G.ACGRPCODE = A.ACGRPCODE"
            strSql += vbCrLf + "  left JOIN " & cnAdminDb & "..ACGROUP AS SG ON SG.ACGRPCODE = A.ACGRPCODE"
            strSql += vbCrLf + "  WHERE (SG.GRPLEDGER IN ('T') OR G.GRPLEDGER IN ('T'))"

            If rbtCls_Stock_Man.Checked Then 'Val(objGPack.GetSqlValue("SELECT COUNT(*) FROM " & cnStockDb & "..CLOSINGVAL WHERE 1=1 " & strCostId, , , )) > 0 Then
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT C.CLSCODE ACID,COSTID,H.ACGRPCODE,G.ACGRPNAME ACGRPNAME,'0' ACMAINCODE 	,'T' GRPLEDGER,"
                strSql += vbCrLf + " CASE WHEN SUM(VALUE) > 0 THEN 'I' ELSE 'E' END GRPTYPE ,CASE WHEN SUM(VALUE) < 0 THEN SUM(VALUE)*(-1) END DEBIT,"
                strSql += vbCrLf + " CASE WHEN SUM(VALUE) > 0 THEN SUM(VALUE) END CREDIT ,H.ACNAME ACNAME,'' TRANDATE ,CASE WHEN SUM(VALUE) > 0 THEN 'D' ELSE 'C' END TRANMODE,SUM(VALUE)AMOUNT ,'' PAYMODE,'' TRANNO,'CLOSING STOCK' ACMAINGRPNAME "
                strSql += vbCrLf + " FROM " & cnStockDb & "..CLOSINGVAL A LEFT JOIN " & cnAdminDb & "..CATEGORY C ON A.CATCODE = C.CATCODE LEFT JOIN " & cnAdminDb & "..ACHEAD H ON C.CLSCODE = H.ACCODE "
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ACGROUP G ON H.ACGRPCODE = G.ACGRPCODE"
                strSql += vbCrLf + " WHERE ISNULL(A.COMPANYID,'') = '" & strcompid & "'"
                strSql += vbCrLf + strCostId + " GROUP BY H.ACGRPCODE,C.CLSCODE,H.ACNAME,G.ACGRPNAME,COSTID"
                strSql += vbCrLf + " HAVING SUM(VALUE) <> 0"
            Else
                For K As Integer = 0 To DtCostid.Rows.Count - 1
                    Dim Qry As String = ""
                    ProgressBarShow()
                    If rbtCls_Stock_AvgRate.Checked Then
                        ProgressBarStep("Closing Stock arrival Weighted avg method", 10)
                        Qry = " EXEC " & cnAdminDb & "..SP_CLOSING_STOCK"
                    Else
                        ProgressBarStep("Closing Stock arrival LIFO method", 10)
                        Qry = " EXEC " & cnAdminDb & "..SP_CLOSINGSTOCK_LIFO"
                    End If
                    Qry += vbCrLf + " @DBNAME = '" & cnStockDb & "'"
                    Qry += vbCrLf + " ,@ADMINDB = '" & cnAdminDb & "'"
                    Qry += vbCrLf + " ,@FRMDATE = '" & Format(dtpDate.Value, "yyyy-MM-dd") & "'"
                    Qry += vbCrLf + " ,@TODATE = '" & Format(dtpDate.Value, "yyyy-MM-dd") & "'"
                    Qry += vbCrLf + " ,@METALNAME = 'ALL'"
                    Qry += vbCrLf + " ,@CATCODE = 'ALL'"
                    Qry += vbCrLf + " ,@CATNAME = 'ALL'"
                    Qry += vbCrLf + " ,@COSTNAME = '" & DtCostid.Rows(K).Item(1).ToString.Trim & "'"
                    Qry += vbCrLf + " ,@COMPANYID = '" & strcompid & "'"
                    Qry += vbCrLf + " ,@RPTTYPE = 'S'"
                    Dim SYSTEMID As String = Trim(LSet(Mid(Guid.NewGuid.ToString, 1, InStr(Guid.NewGuid.ToString, "-") - 1), 10))
                    Qry += vbCrLf + " ,@SYSTEMID = '" & SYSTEMID & "'"
                    cmd = New OleDbCommand(Qry, cn)
                    cmd.CommandTimeout = 2000
                    cmd.ExecuteNonQuery()
                    Qry = "  IF EXISTS (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMPCSTKVALUE4" & DtCostid.Rows(K).Item(0).ToString.Trim & "')"
                    Qry += vbCrLf + "  DROP TABLE TEMPTABLEDB..TEMPCSTKVALUE4" & DtCostid.Rows(K).Item(0).ToString.Trim & ""
                    Qry += vbCrLf + "  SELECT *,'" & DtCostid.Rows(K).Item(0).ToString.Trim & "' COSTID INTO TEMPTABLEDB..TEMPCSTKVALUE4" & DtCostid.Rows(K).Item(0).ToString.Trim & " FROM TEMPTABLEDB..TEMPCSTKVALUE4" & SYSTEMID & ""
                    cmd = New OleDbCommand(Qry, cn)
                    cmd.ExecuteNonQuery()

                    strSql += vbCrLf + " UNION ALL"
                    strSql += vbCrLf + " SELECT C.CLSCODE ACID,COSTID,H.ACGRPCODE,G.ACGRPNAME ACGRPNAME,'0' ACMAINCODE ,'T' GRPLEDGER,"
                    strSql += vbCrLf + " CASE WHEN SUM(CAMOUNT) > 0 THEN 'I' ELSE 'E' END GRPTYPE ,CASE WHEN SUM(CAMOUNT) < 0 THEN SUM(CAMOUNT)*(-1) END DEBIT 	,"
                    strSql += vbCrLf + " CASE WHEN SUM(CAMOUNT) > 0 THEN SUM(CAMOUNT) END CREDIT ,H.ACNAME ACNAME,'' TRANDATE ,CASE WHEN SUM(CAMOUNT) > 0 THEN 'D' ELSE 'C' END TRANMODE,SUM(CAMOUNT) AMOUNT ,'' PAYMODE,'' TRANNO,'CLOSING STOCK' ACMAINGRPNAME "
                    strSql += vbCrLf + " FROM TEMPTABLEDB..TEMPCSTKVALUE4" & DtCostid.Rows(K).Item(0).ToString.Trim & " A "
                    strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON A.CATEGORY = C.CATNAME "
                    strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ACHEAD H ON C.CLSCODE = H.ACCODE "
                    strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ACGROUP G ON H.ACGRPCODE = G.ACGRPCODE"
                    strSql += vbCrLf + " WHERE A.RESULT=1"
                    strSql += vbCrLf + " GROUP BY H.ACGRPCODE,C.CLSCODE,H.ACNAME,G.ACGRPNAME,COSTID"
                    strSql += vbCrLf + " HAVING SUM(CAMOUNT) <> 0"
                Next
            End If
        End If
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        ProgressBarHide()
        'Dim DtCostid As New DataTable
        'strSql = " SELECT DISTINCT COSTID FROM TEMP" & systemId & userId.ToString() & "TRADE2 ORDER BY COSTID"
        'da = New OleDbDataAdapter(strSql, cn)
        'da.Fill(DtCostid)
        For K As Integer = 0 To DtCostid.Rows.Count - 1
            strSql = "  IF EXISTS (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & userId.ToString() & "TRADE3" & DtCostid.Rows(K).Item(0).ToString.Trim & "')"
            strSql += vbCrLf + "  DROP TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE3" & DtCostid.Rows(K).Item(0).ToString.Trim & ""
            strSql += vbCrLf + "  CREATE TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE3" & DtCostid.Rows(K).Item(0).ToString.Trim & ""
            strSql += vbCrLf + "  (EXPACGROUPNAME1 VARCHAR(250),EXPENSES1 VARCHAR(50),EXPENSES2 VARCHAR(50),EXPENSES VARCHAR(50)"
            strSql += vbCrLf + "  ,INCACGROUPNAME1 VARCHAR(250),INCOME1 VARCHAR(50),INCOME2 VARCHAR(50),INCOME VARCHAR(50)"
            strSql += vbCrLf + "  ,RESULT INT,SEP INT,EXPACMAINCODE VARCHAR(50),INCACMAINCODE VARCHAR(50),EXPACGROUP VARCHAR(250),INCACGROUP VARCHAR(250)"
            strSql += vbCrLf + "  ,EXPACID VARCHAR(50),EXPACNAME VARCHAR(250),EXPACGROUPNAME VARCHAR(250),EXPACMAINGRPNAME VARCHAR(250)"
            strSql += vbCrLf + "  ,INCACID VARCHAR(50),INCACNAME VARCHAR(250),INCACGROUPNAME VARCHAR(250),INCACMAINGRPNAME VARCHAR(250),SNO INT IDENTITY(1,1),COLHEAD VARCHAR(2),COSTID VARCHAR(2)"
            strSql += vbCrLf + "  )"
            strSql += vbCrLf + "  "
            strSql += vbCrLf + "  IF EXISTS (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & userId.ToString() & "TRADE4" & DtCostid.Rows(K).Item(0).ToString.Trim & "')"
            strSql += vbCrLf + "  DROP TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE4" & DtCostid.Rows(K).Item(0).ToString.Trim & ""
            strSql += vbCrLf + "  CREATE TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE4" & DtCostid.Rows(K).Item(0).ToString.Trim & ""
            strSql += vbCrLf + "  (EXPACGROUPNAME1 VARCHAR(250),EXPENSES1 VARCHAR(50),EXPENSES2 VARCHAR(50),EXPENSES VARCHAR(50)"
            strSql += vbCrLf + "  ,INCACGROUPNAME1 VARCHAR(250),INCOME1 VARCHAR(50),INCOME2 VARCHAR(50),INCOME VARCHAR(50)"
            strSql += vbCrLf + "  ,RESULT INT,SEP INT,EXPACMAINCODE VARCHAR(50),INCACMAINCODE VARCHAR(50),EXPACGROUP VARCHAR(250),INCACGROUP VARCHAR(250)"
            strSql += vbCrLf + "  ,EXPACID VARCHAR(50),EXPACNAME VARCHAR(250),EXPACGROUPNAME VARCHAR(250),EXPACMAINGRPNAME VARCHAR(250)"
            strSql += vbCrLf + "  ,INCACID VARCHAR(50),INCACNAME VARCHAR(250),INCACGROUPNAME VARCHAR(250),INCACMAINGRPNAME VARCHAR(250),SNO INT IDENTITY(1,1),COLHEAD VARCHAR(2),COSTID VARCHAR(2)"
            strSql += vbCrLf + "  )"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            If summary Then

                strSql = "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE3" & DtCostid.Rows(K).Item(0).ToString.Trim & ""
                strSql += vbCrLf + "  (EXPACGROUPNAME1,EXPENSES1,EXPENSES2,EXPENSES,INCACGROUPNAME1,INCOME1,INCOME2,INCOME,RESULT,SEP,EXPACMAINCODE,INCACMAINCODE,EXPACGROUP,INCACGROUP,EXPACID"
                strSql += vbCrLf + "  ,EXPACNAME,EXPACGROUPNAME,EXPACMAINGRPNAME,INCACID,INCACNAME,INCACGROUPNAME,INCACMAINGRPNAME,COLHEAD,COSTID)"
                strSql += vbCrLf + "  SELECT DISTINCT ACMAINGRPNAME EXPACGROUPNAME1 ,NULL EXPENSES1,NULL EXPENSES2"
                strSql += vbCrLf + "  ,NULL EXPENSES,NULL INCACGROUPNAME1,NULL INCOME2,NULL INCOME1,NULL INCOME"
                strSql += vbCrLf + "  ,'1' RESULT,'1' SEP,ACMAINCODE EXPACMAINCODE,'' INCACMAINCODE,'' EXPACGROUP,'' INCACGROUP"
                strSql += vbCrLf + "  ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,ACMAINGRPNAME EXPACMAINGRPNAME"
                strSql += vbCrLf + "  ,''INCACID,''INCACNAME,''INCACGROUPNAME,'' INCACMAINGRPNAME,'T1' COLHEAD,COSTID"
                strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'E' AND ISNULL(COSTID,'')='" & DtCostid.Rows(K).Item(0).ToString & "'  AND ISNULL(ACGRPCODE,0)<>0"
                strSql += vbCrLf + "  UNION ALL"
                strSql += vbCrLf + "  SELECT DISTINCT '  ' + ACGRPNAME EXPACGROUPNAME1,NULL EXPENSES2,NULL EXPENSES1"
                strSql += vbCrLf + "  ,CONVERT(VARCHAR,ISNULL(SUM(DEBIT),0) - ISNULL(SUM(CREDIT),0)) EXPENSES,NULL INCACGROUPNAME1,NULL INCOME2,NULL INCOME1"
                strSql += vbCrLf + "  ,NULL INCOME,'7' RESULT,'3' SEP,ACMAINCODE EXPACMAINCODE,'' INCACMAINCODE"
                strSql += vbCrLf + "  ,ACGRPCODE EXPACGROUP,'' INCACGROUP,'' EXPACID,'' EXPACNAME,ACGRPNAME EXPACGROUPNAME"
                strSql += vbCrLf + "  ,ACMAINGRPNAME EXPACMAINGRPNAME,''INCACID,''INCACNAME,''INCACGROUPNAME,'' INCACMAINGRPNAME,'T2' COLHEAD,COSTID"
                strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'E' AND ISNULL(ACMAINCODE,'') != ISNULL(ACGRPCODE,'')  AND ISNULL(COSTID,'')='" & DtCostid.Rows(K).Item(0).ToString & "'"
                strSql += vbCrLf + "  GROUP BY ACGRPNAME,ACGRPCODE,ACMAINGRPNAME,ACMAINCODE,COSTID"
                strSql += vbCrLf + "  HAVING (ISNULL(SUM(DEBIT),0) - ISNULL(SUM(CREDIT),0)) >0"
                strSql += vbCrLf + "  ORDER BY EXPACMAINGRPNAME,SEP,EXPACGROUPNAME,RESULT,EXPACNAME,COSTID"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                strSql = "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE4" & DtCostid.Rows(K).Item(0).ToString.Trim & ""
                strSql += vbCrLf + "  (EXPACGROUPNAME1,EXPENSES1,EXPENSES2,EXPENSES,INCACGROUPNAME1,INCOME1,INCOME2,INCOME,RESULT,SEP,EXPACMAINCODE,INCACMAINCODE,EXPACGROUP,INCACGROUP,EXPACID"
                strSql += vbCrLf + "  ,EXPACNAME,EXPACGROUPNAME,EXPACMAINGRPNAME,INCACID,INCACNAME,INCACGROUPNAME,INCACMAINGRPNAME,COLHEAD,COSTID)"
                strSql += vbCrLf + "  SELECT DISTINCT NULL EXPACGROUPNAME1,NULL EXPENSES1,NULL EXPENSES2,NULL EXPENSES"
                strSql += vbCrLf + "  ,ACMAINGRPNAME INCACGROUPNAME1,NULL INCOME2,NULL INCOME1,NULL INCOME,'1' RESULT,'1' SEP"
                strSql += vbCrLf + "  ,'' EXPACMAINCODE,ACMAINCODE INCACMAINCODE,'' EXPACGROUP,'' INCACGROUP"
                strSql += vbCrLf + "  ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,'' EXPACMAINGRPNAME"
                strSql += vbCrLf + "  ,'' INCACID,''INCACNAME,''INCACGROUPNAME,ACMAINGRPNAME INCACMAINGRPNAME,'T1' COLHEAD,COSTID"
                strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'I'  AND ISNULL(COSTID,'')='" & DtCostid.Rows(K).Item(0).ToString & "' AND ISNULL(ACGRPCODE,0)<>0"
                strSql += vbCrLf + "  UNION ALL"
                strSql += vbCrLf + "  SELECT DISTINCT NULL EXPACGROUPNAME1,NULL EXPENSES2,NULL EXPENSES1,NULL EXPENSES"
                strSql += vbCrLf + "  ,'  ' + ACGRPNAME INCACGROUPNAME1,NULL INCOME2,NULL INCOME1,CONVERT(VARCHAR,ISNULL(SUM(CREDIT),0) - ISNULL(SUM(DEBIT),0)) INCOME,'7' RESULT,'3' SEP"
                strSql += vbCrLf + "  ,'' EXPACMAINCODE,ACMAINCODE INCACMAINCODE,'' EXPACGROUP,ACGRPCODE INCACGROUP"
                strSql += vbCrLf + "  ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,'' EXPACMAINGRPNAME"
                strSql += vbCrLf + "  ,''INCACID,''INCACNAME,ACGRPNAME INCACGROUPNAME,ACMAINGRPNAME INCACMAINGRPNAME,'T2' COLHEAD,COSTID"
                strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'I' " 'AND ISNULL(ACMAINCODE,'') != ISNULL(ACGRPCODE,'')  "
                strSql += vbCrLf + "  AND ISNULL(COSTID,'')='" & DtCostid.Rows(K).Item(0).ToString & "'"
                strSql += vbCrLf + "  GROUP BY ACGRPNAME,ACGRPCODE,ACMAINGRPNAME,ACMAINCODE,COSTID"
                strSql += vbCrLf + "  HAVING (ISNULL(SUM(CREDIT), 0) - ISNULL(SUM(DEBIT), 0))>0"
                strSql += vbCrLf + "  ORDER BY INCACMAINGRPNAME,SEP,INCACGROUPNAME,RESULT,INCACNAME,COSTID"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
            Else
                strSql = "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE3" & DtCostid.Rows(K).Item(0).ToString.Trim & ""
                strSql += vbCrLf + "  (EXPACGROUPNAME1,EXPENSES1,EXPENSES2,EXPENSES,INCACGROUPNAME1,INCOME1,INCOME2,INCOME,RESULT,SEP,EXPACMAINCODE,INCACMAINCODE,EXPACGROUP,INCACGROUP,EXPACID"
                strSql += vbCrLf + "  ,EXPACNAME,EXPACGROUPNAME,EXPACMAINGRPNAME,INCACID,INCACNAME,INCACGROUPNAME,INCACMAINGRPNAME,COLHEAD,COSTID)"
                strSql += vbCrLf + "  SELECT DISTINCT ACMAINGRPNAME EXPACGROUPNAME1 ,NULL EXPENSES1,NULL EXPENSES2"
                strSql += vbCrLf + "  ,NULL EXPENSES,NULL INCACGROUPNAME1,NULL INCOME2,NULL INCOME1,NULL INCOME"
                strSql += vbCrLf + "  ,'1' RESULT,'1' SEP,ACMAINCODE EXPACMAINCODE,'' INCACMAINCODE,'' EXPACGROUP,'' INCACGROUP"
                strSql += vbCrLf + "  ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,ACMAINGRPNAME EXPACMAINGRPNAME"
                strSql += vbCrLf + "  ,''INCACID,''INCACNAME,''INCACGROUPNAME,'' INCACMAINGRPNAME,'T1' COLHEAD,COSTID"
                strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'E' AND ISNULL(COSTID,'')='" & DtCostid.Rows(K).Item(0).ToString & "'"
                strSql += vbCrLf + "  UNION ALL"
                strSql += vbCrLf + "  SELECT DISTINCT '  ' + ACGRPNAME EXPACGROUPNAME1,NULL EXPENSES2,NULL EXPENSES1"
                strSql += vbCrLf + "  ,NULL EXPENSES,NULL INCACGROUPNAME1,NULL INCOME2,NULL INCOME1"
                strSql += vbCrLf + "  ,NULL INCOME,'2' RESULT,'2' SEP,ACMAINCODE EXPACMAINCODE,'' INCACMAINCODE"
                strSql += vbCrLf + "  ,ACGRPCODE EXPACGROUP,'' INCACGROUP,'' EXPACID,'' EXPACNAME,ACGRPNAME EXPACGROUPNAME"
                strSql += vbCrLf + "  ,ACMAINGRPNAME EXPACMAINGRPNAME,''INCACID,''INCACNAME,''INCACGROUPNAME,'' INCACMAINGRPNAME,'T2' COLHEAD,COSTID"
                strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'E' AND ISNULL(ACMAINCODE,'') != ISNULL(ACGRPCODE,'')  AND ISNULL(COSTID,'')='" & DtCostid.Rows(K).Item(0).ToString & "'"
                strSql += vbCrLf + "  UNION ALL"
                strSql += vbCrLf + "  SELECT DISTINCT CASE WHEN ACMAINGRPNAME = ACGRPNAME THEN '  ' ELSE '    ' END + ACNAME EXPACGROUPNAME1"
                strSql += vbCrLf + "  ,CASE WHEN ACMAINGRPNAME != ACGRPNAME THEN CONVERT(VARCHAR,ISNULL(SUM(DEBIT),0) - ISNULL(SUM(CREDIT),0)) END EXPENSES2"
                strSql += vbCrLf + "  ,CASE WHEN ACMAINGRPNAME = ACGRPNAME THEN CONVERT(VARCHAR,ISNULL(SUM(DEBIT),0) - ISNULL(SUM(CREDIT),0)) END EXPENSES1,NULL EXPENSES"
                strSql += vbCrLf + "  ,NULL INCACGROUPNAME1,NULL INCOME2,NULL INCOME1,NULL INCOME,'3' RESULT,'2' SEP"
                strSql += vbCrLf + "  ,ACMAINCODE EXPACMAINCODE,'' INCACMAINCODE,ACGRPCODE EXPACGROUP,'' INCACGROUP"
                strSql += vbCrLf + "  ,ACID EXPACID,ACNAME EXPACNAME,ACGRPNAME EXPACGROUPNAME,ACMAINGRPNAME EXPACMAINGRPNAME"
                strSql += vbCrLf + "  ,''INCACID,''INCACNAME,''INCACGROUPNAME,'' INCACMAINGRPNAME,'' COLHEAD,COSTID"
                strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'E'  AND ISNULL(COSTID,'')='" & DtCostid.Rows(K).Item(0).ToString & "'"
                strSql += vbCrLf + "  GROUP BY ACMAINGRPNAME,ACGRPNAME,ACMAINCODE,ACGRPCODE,ACID,ACNAME,COSTID"
                strSql += vbCrLf + "  HAVING (ISNULL(SUM(DEBIT),0) - ISNULL(SUM(CREDIT),0)) >0"
                strSql += vbCrLf + "  UNION ALL"
                strSql += vbCrLf + "  SELECT DISTINCT '' EXPACGROUPNAME1"
                strSql += vbCrLf + "  ,CASE WHEN ACMAINGRPNAME != ACGRPNAME THEN '-------------------' END EXPENSES2"
                strSql += vbCrLf + "  ,CASE WHEN ACMAINGRPNAME  = ACGRPNAME THEN '-------------------' END EXPENSES1,NULL EXPENSES"
                strSql += vbCrLf + "  ,NULL INCACGROUPNAME1 ,NULL INCOME2,NULL INCOME1,NULL INCOME,'4' RESULT,'2' SEP"
                strSql += vbCrLf + "  ,ACMAINCODE EXPACMAINCODE,'' INCACMAINCODE,ACGRPCODE EXPACGROUP,'' INCACGROUP"
                strSql += vbCrLf + "  ,'' EXPACID,'' EXPACNAME,ACGRPNAME EXPACGROUPNAME,ACMAINGRPNAME EXPACMAINGRPNAME"
                strSql += vbCrLf + "  ,''INCACID,''INCACNAME,''INCACGROUPNAME,'' INCACMAINGRPNAME,'S2' COLHEAD,COSTID"
                strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'E'  AND ISNULL(COSTID,'')='" & DtCostid.Rows(K).Item(0).ToString & "'"
                strSql += vbCrLf + "  GROUP BY ACMAINGRPNAME,ACGRPNAME,ACMAINCODE,ACGRPCODE,COSTID"
                strSql += vbCrLf + "  UNION ALL"
                strSql += vbCrLf + "  SELECT DISTINCT ' ' EXPACGROUPNAME1"
                strSql += vbCrLf + "  ,NULL EXPENSES1,CONVERT(VARCHAR,ISNULL(SUM(DEBIT),0) - ISNULL(SUM(CREDIT),0)) EXPENSES2,NULL EXPENSES"
                strSql += vbCrLf + "  ,NULL INCACGROUPNAME1,NULL INCOME2,NULL INCOME1,NULL INCOME,'5' RESULT,'2' SEP"
                strSql += vbCrLf + "  ,ACMAINCODE EXPACMAINCODE,'' INCACMAINCODE,ACGRPCODE EXPACGROUP,'' INCACGROUP"
                strSql += vbCrLf + "  ,'' EXPACID,'' EXPACNAME,ACGRPNAME EXPACGROUPNAME,ACMAINGRPNAME EXPACMAINGRPNAME"
                strSql += vbCrLf + "  ,''INCACID,''INCACNAME,''INCACGROUPNAME,'' INCACMAINGRPNAME,'S2' COLHEAD,COSTID"
                strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'E' AND ISNULL(ACMAINCODE,'') != ISNULL(ACGRPCODE,'')  AND ISNULL(COSTID,'')='" & DtCostid.Rows(K).Item(0).ToString & "'"
                strSql += vbCrLf + "  GROUP BY ACMAINGRPNAME,ACGRPNAME,ACMAINCODE,ACGRPCODE,COSTID"
                strSql += vbCrLf + "  HAVING (ISNULL(SUM(DEBIT),0) - ISNULL(SUM(CREDIT),0)) >0"
                strSql += vbCrLf + "  UNION ALL"
                strSql += vbCrLf + "  SELECT DISTINCT '  ' EXPACGROUPNAME1,NULL EXPENSES1,'-------------------' EXPENSES2,NULL EXPENSES"
                strSql += vbCrLf + "  ,NULL INCACGROUPNAME1,NULL INCOME2,NULL INCOME1,NULL INCOME,'6' RESULT,'2' SEP"
                strSql += vbCrLf + "  ,ACMAINCODE EXPACMAINCODE,'' INCACMAINCODE,ACGRPCODE EXPACGROUP,'' INCACGROUP"
                strSql += vbCrLf + "  ,'' EXPACID,'' EXPACNAME,ACGRPNAME EXPACGROUPNAME,ACMAINGRPNAME EXPACMAINGRPNAME"
                strSql += vbCrLf + "  ,''INCACID,''INCACNAME,''INCACGROUPNAME,'' INCACMAINGRPNAME,'S1' COLHEAD,COSTID"
                strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'E' AND ISNULL(ACMAINCODE,'') != ISNULL(ACGRPCODE,'')  AND ISNULL(COSTID,'')='" & DtCostid.Rows(K).Item(0).ToString & "'"
                strSql += vbCrLf + "  GROUP BY ACMAINGRPNAME,ACGRPNAME,ACMAINCODE,ACGRPCODE,COSTID"
                strSql += vbCrLf + "  UNION ALL"
                strSql += vbCrLf + "  SELECT DISTINCT '' EXPACGROUPNAME1"
                strSql += vbCrLf + "  ,NULL EXPENSES2,NULL EXPENSES1,CONVERT(VARCHAR,ISNULL(SUM(DEBIT),0) - ISNULL(SUM(CREDIT),0)) EXPENSES"
                strSql += vbCrLf + "  ,NULL INCACGROUPNAME1,NULL INCOME2,NULL INCOME1,NULL INCOME,'7' RESULT,'3' SEP"
                strSql += vbCrLf + "  ,ACMAINCODE EXPACMAINCODE,'' INCACMAINCODE,'' EXPACGROUP,'' INCACGROUP"
                strSql += vbCrLf + "  ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,ACMAINGRPNAME EXPACMAINGRPNAME"
                strSql += vbCrLf + "  ,''INCACID,''INCACNAME,''INCACGROUPNAME,'' INCACMAINGRPNAME,'G1' COLHEAD,COSTID"
                strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'E'  AND ISNULL(COSTID,'')='" & DtCostid.Rows(K).Item(0).ToString & "'"
                strSql += vbCrLf + "  GROUP BY ACMAINGRPNAME,ACMAINCODE,COSTID"
                strSql += vbCrLf + "  HAVING (ISNULL(SUM(DEBIT),0) - ISNULL(SUM(CREDIT),0)) >0"
                strSql += vbCrLf + "  UNION ALL"
                strSql += vbCrLf + "  SELECT DISTINCT '' EXPACGROUPNAME1,NULL EXPENSES2,NULL EXPENSES1,'-------------------' EXPENSES"
                strSql += vbCrLf + "  ,NULL INCACGROUPNAME1,NULL INCOME2,NULL INCOME1,NULL INCOME,'8' RESULT,'4' SEP"
                strSql += vbCrLf + "  ,ACMAINCODE EXPACMAINCODE,'' INCACMAINCODE,'' EXPACGROUP,'' INCACGROUP"
                strSql += vbCrLf + "  ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,ACMAINGRPNAME EXPACMAINGRPNAME"
                strSql += vbCrLf + "  ,''INCACID,''INCACNAME,''INCACGROUPNAME,'' INCACMAINGRPNAME,'G1' COLHEAD,COSTID"
                strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'E'  AND ISNULL(COSTID,'')='" & DtCostid.Rows(K).Item(0).ToString & "'"
                strSql += vbCrLf + "  GROUP BY ACMAINGRPNAME,ACMAINCODE,COSTID"
                'CALNO 290414 
                If Val(objGPack.GetSqlValue("SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'I'  AND ISNULL(COSTID,'')='" & DtCostid.Rows(K).Item(0).ToString & "' HAVING (ISNULL(SUM(DEBIT),0) - ISNULL(SUM(CREDIT),0)) >0", , "", )) > 0 Then
                    strSql += vbCrLf + "  UNION ALL"
                    strSql += vbCrLf + "  SELECT DISTINCT '  ' + ACGRPNAME EXPACGROUPNAME1,NULL EXPENSES2,NULL EXPENSES1"
                    strSql += vbCrLf + "  ,NULL EXPENSES,NULL INCACGROUPNAME1,NULL INCOME2,NULL INCOME1"
                    strSql += vbCrLf + "  ,NULL INCOME,'2' RESULT,'2' SEP,ACMAINCODE EXPACMAINCODE,'' INCACMAINCODE"
                    strSql += vbCrLf + "  ,ACGRPCODE EXPACGROUP,'' INCACGROUP,'' EXPACID,'' EXPACNAME,ACGRPNAME EXPACGROUPNAME"
                    strSql += vbCrLf + "  ,ACMAINGRPNAME EXPACMAINGRPNAME,''INCACID,''INCACNAME,''INCACGROUPNAME,'' INCACMAINGRPNAME,'T2' COLHEAD,COSTID"
                    strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'I' AND ISNULL(ACMAINCODE,'') != ISNULL(ACGRPCODE,'')  AND ISNULL(COSTID,'')='" & DtCostid.Rows(K).Item(0).ToString & "'"
                    'strSql += vbCrLf + "  HAVING (ISNULL(SUM(DEBIT),0) - ISNULL(SUM(CREDIT),0)) >0"
                End If
                strSql += vbCrLf + "  UNION ALL"
                strSql += vbCrLf + "  SELECT DISTINCT CASE WHEN ACMAINGRPNAME = ACGRPNAME THEN '  ' ELSE '    ' END + ACNAME EXPACGROUPNAME1"
                strSql += vbCrLf + "  ,CASE WHEN ACMAINGRPNAME != ACGRPNAME THEN CONVERT(VARCHAR,ISNULL(SUM(DEBIT),0) - ISNULL(SUM(CREDIT),0)) END EXPENSES2"
                strSql += vbCrLf + "  ,CASE WHEN ACMAINGRPNAME = ACGRPNAME THEN CONVERT(VARCHAR,ISNULL(SUM(DEBIT),0) - ISNULL(SUM(CREDIT),0)) END EXPENSES1,NULL EXPENSES"
                strSql += vbCrLf + "  ,NULL INCACGROUPNAME1,NULL INCOME2,NULL INCOME1,NULL INCOME,'3' RESULT,'2' SEP"
                strSql += vbCrLf + "  ,ACMAINCODE EXPACMAINCODE,'' INCACMAINCODE,ACGRPCODE EXPACGROUP,'' INCACGROUP"
                strSql += vbCrLf + "  ,ACID EXPACID,ACNAME EXPACNAME,ACGRPNAME EXPACGROUPNAME,ACMAINGRPNAME EXPACMAINGRPNAME"
                strSql += vbCrLf + "  ,''INCACID,''INCACNAME,''INCACGROUPNAME,'' INCACMAINGRPNAME,'' COLHEAD,COSTID"
                strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'I'  AND ISNULL(COSTID,'')='" & DtCostid.Rows(K).Item(0).ToString & "'"
                strSql += vbCrLf + "  GROUP BY ACMAINGRPNAME,ACGRPNAME,ACMAINCODE,ACGRPCODE,ACID,ACNAME,COSTID"
                strSql += vbCrLf + "  HAVING (ISNULL(SUM(DEBIT),0) - ISNULL(SUM(CREDIT),0)) >0"
                strSql += vbCrLf + "  UNION ALL"
                strSql += vbCrLf + "  SELECT DISTINCT '' EXPACGROUPNAME1"
                strSql += vbCrLf + "  ,CASE WHEN ACMAINGRPNAME != ACGRPNAME THEN '-------------------' END EXPENSES2"
                strSql += vbCrLf + "  ,CASE WHEN ACMAINGRPNAME  = ACGRPNAME THEN '-------------------' END EXPENSES1,NULL EXPENSES"
                strSql += vbCrLf + "  ,NULL INCACGROUPNAME1 ,NULL INCOME2,NULL INCOME1,NULL INCOME,'4' RESULT,'2' SEP"
                strSql += vbCrLf + "  ,ACMAINCODE EXPACMAINCODE,'' INCACMAINCODE,ACGRPCODE EXPACGROUP,'' INCACGROUP"
                strSql += vbCrLf + "  ,'' EXPACID,'' EXPACNAME,ACGRPNAME EXPACGROUPNAME,ACMAINGRPNAME EXPACMAINGRPNAME"
                strSql += vbCrLf + "  ,''INCACID,''INCACNAME,''INCACGROUPNAME,'' INCACMAINGRPNAME,'S2' COLHEAD,COSTID"
                strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'I'  AND ISNULL(COSTID,'')='" & DtCostid.Rows(K).Item(0).ToString & "'"
                strSql += vbCrLf + "  GROUP BY ACMAINGRPNAME,ACGRPNAME,ACMAINCODE,ACGRPCODE,COSTID"
                strSql += vbCrLf + "  HAVING (ISNULL(SUM(DEBIT),0) - ISNULL(SUM(CREDIT),0)) >0"
                strSql += vbCrLf + "  UNION ALL"
                strSql += vbCrLf + "  SELECT DISTINCT ' ' EXPACGROUPNAME1"
                strSql += vbCrLf + "  ,NULL EXPENSES1,CONVERT(VARCHAR,ISNULL(SUM(DEBIT),0) - ISNULL(SUM(CREDIT),0)) EXPENSES2,NULL EXPENSES"
                strSql += vbCrLf + "  ,NULL INCACGROUPNAME1,NULL INCOME2,NULL INCOME1,NULL INCOME,'5' RESULT,'2' SEP"
                strSql += vbCrLf + "  ,ACMAINCODE EXPACMAINCODE,'' INCACMAINCODE,ACGRPCODE EXPACGROUP,'' INCACGROUP"
                strSql += vbCrLf + "  ,'' EXPACID,'' EXPACNAME,ACGRPNAME EXPACGROUPNAME,ACMAINGRPNAME EXPACMAINGRPNAME"
                strSql += vbCrLf + "  ,''INCACID,''INCACNAME,''INCACGROUPNAME,'' INCACMAINGRPNAME,'S2' COLHEAD,COSTID"
                strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'I' AND ISNULL(ACMAINCODE,'') != ISNULL(ACGRPCODE,'')  AND ISNULL(COSTID,'')='" & DtCostid.Rows(K).Item(0).ToString & "'"
                strSql += vbCrLf + "  GROUP BY ACMAINGRPNAME,ACGRPNAME,ACMAINCODE,ACGRPCODE,COSTID"
                strSql += vbCrLf + "  HAVING (ISNULL(SUM(DEBIT),0) - ISNULL(SUM(CREDIT),0)) >0"
                strSql += vbCrLf + "  UNION ALL"
                strSql += vbCrLf + "  SELECT DISTINCT '  ' EXPACGROUPNAME1,NULL EXPENSES1,'-------------------' EXPENSES2,NULL EXPENSES"
                strSql += vbCrLf + "  ,NULL INCACGROUPNAME1,NULL INCOME2,NULL INCOME1,NULL INCOME,'6' RESULT,'2' SEP"
                strSql += vbCrLf + "  ,ACMAINCODE EXPACMAINCODE,'' INCACMAINCODE,ACGRPCODE EXPACGROUP,'' INCACGROUP"
                strSql += vbCrLf + "  ,'' EXPACID,'' EXPACNAME,ACGRPNAME EXPACGROUPNAME,ACMAINGRPNAME EXPACMAINGRPNAME"
                strSql += vbCrLf + "  ,''INCACID,''INCACNAME,''INCACGROUPNAME,'' INCACMAINGRPNAME,'S1' COLHEAD,COSTID"
                strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'I' AND ISNULL(ACMAINCODE,'') != ISNULL(ACGRPCODE,'')  AND ISNULL(COSTID,'')='" & DtCostid.Rows(K).Item(0).ToString & "'"
                strSql += vbCrLf + "  GROUP BY ACMAINGRPNAME,ACGRPNAME,ACMAINCODE,ACGRPCODE,COSTID"
                strSql += vbCrLf + "  HAVING (ISNULL(SUM(DEBIT),0) - ISNULL(SUM(CREDIT),0)) >0"
                strSql += vbCrLf + "  UNION ALL"
                strSql += vbCrLf + "  SELECT DISTINCT '' EXPACGROUPNAME1"
                strSql += vbCrLf + "  ,NULL EXPENSES2,NULL EXPENSES1,CONVERT(VARCHAR,ISNULL(SUM(DEBIT),0) - ISNULL(SUM(CREDIT),0)) EXPENSES"
                strSql += vbCrLf + "  ,NULL INCACGROUPNAME1,NULL INCOME2,NULL INCOME1,NULL INCOME,'7' RESULT,'3' SEP"
                strSql += vbCrLf + "  ,ACMAINCODE EXPACMAINCODE,'' INCACMAINCODE,'' EXPACGROUP,'' INCACGROUP"
                strSql += vbCrLf + "  ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,ACMAINGRPNAME EXPACMAINGRPNAME"
                strSql += vbCrLf + "  ,''INCACID,''INCACNAME,''INCACGROUPNAME,'' INCACMAINGRPNAME,'G1' COLHEAD,COSTID"
                strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'I'  AND ISNULL(COSTID,'')='" & DtCostid.Rows(K).Item(0).ToString & "'"
                strSql += vbCrLf + "  GROUP BY ACMAINGRPNAME,ACMAINCODE,COSTID"
                strSql += vbCrLf + "  HAVING (ISNULL(SUM(DEBIT),0) - ISNULL(SUM(CREDIT),0)) >0"
                strSql += vbCrLf + "  UNION ALL"
                strSql += vbCrLf + "  SELECT DISTINCT '' EXPACGROUPNAME1,NULL EXPENSES2,NULL EXPENSES1,'-------------------' EXPENSES"
                strSql += vbCrLf + "  ,NULL INCACGROUPNAME1,NULL INCOME2,NULL INCOME1,NULL INCOME,'8' RESULT,'4' SEP"
                strSql += vbCrLf + "  ,ACMAINCODE EXPACMAINCODE,'' INCACMAINCODE,'' EXPACGROUP,'' INCACGROUP"
                strSql += vbCrLf + "  ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,ACMAINGRPNAME EXPACMAINGRPNAME"
                strSql += vbCrLf + "  ,''INCACID,''INCACNAME,''INCACGROUPNAME,'' INCACMAINGRPNAME,'G1' COLHEAD,COSTID"
                strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'I'  AND ISNULL(COSTID,'')='" & DtCostid.Rows(K).Item(0).ToString & "'"
                strSql += vbCrLf + "  GROUP BY ACMAINGRPNAME,ACMAINCODE,COSTID"
                strSql += vbCrLf + "  HAVING (ISNULL(SUM(DEBIT),0) - ISNULL(SUM(CREDIT),0)) >0"
                'CALNO 290414
                strSql += vbCrLf + "  ORDER BY EXPACMAINGRPNAME,SEP,EXPACGROUPNAME,RESULT,EXPACNAME,COSTID"

                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                strSql = "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE4" & DtCostid.Rows(K).Item(0).ToString.Trim & ""
                strSql += vbCrLf + "  (EXPACGROUPNAME1,EXPENSES1,EXPENSES2,EXPENSES,INCACGROUPNAME1,INCOME1,INCOME2,INCOME,RESULT,SEP,EXPACMAINCODE,INCACMAINCODE,EXPACGROUP,INCACGROUP,EXPACID"
                strSql += vbCrLf + "  ,EXPACNAME,EXPACGROUPNAME,EXPACMAINGRPNAME,INCACID,INCACNAME,INCACGROUPNAME,INCACMAINGRPNAME,COLHEAD,COSTID)"
                strSql += vbCrLf + "  SELECT DISTINCT NULL EXPACGROUPNAME1,NULL EXPENSES1,NULL EXPENSES2,NULL EXPENSES"
                strSql += vbCrLf + "  ,ACMAINGRPNAME INCACGROUPNAME1,NULL INCOME2,NULL INCOME1,NULL INCOME,'1' RESULT,'1' SEP"
                strSql += vbCrLf + "  ,'' EXPACMAINCODE,ACMAINCODE INCACMAINCODE,'' EXPACGROUP,'' INCACGROUP"
                strSql += vbCrLf + "  ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,'' EXPACMAINGRPNAME"
                strSql += vbCrLf + "  ,'' INCACID,''INCACNAME,''INCACGROUPNAME,ACMAINGRPNAME INCACMAINGRPNAME,'T1' COLHEAD,COSTID"
                strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'I'  AND ISNULL(COSTID,'')='" & DtCostid.Rows(K).Item(0).ToString & "'"
                strSql += vbCrLf + "  UNION ALL"
                strSql += vbCrLf + "  SELECT DISTINCT NULL EXPACGROUPNAME1,NULL EXPENSES2,NULL EXPENSES1,NULL EXPENSES"
                strSql += vbCrLf + "  ,'  ' + ACGRPNAME INCACGROUPNAME1,NULL INCOME2,NULL INCOME1,NULL INCOME,'2' RESULT,'2' SEP"
                strSql += vbCrLf + "  ,'' EXPACMAINCODE,ACMAINCODE INCACMAINCODE,'' EXPACGROUP,ACGRPCODE INCACGROUP"
                strSql += vbCrLf + "  ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,'' EXPACMAINGRPNAME"
                strSql += vbCrLf + "  ,''INCACID,''INCACNAME,ACGRPNAME INCACGROUPNAME,ACMAINGRPNAME INCACMAINGRPNAME,'T2' COLHEAD,COSTID"
                strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'I' AND ISNULL(ACMAINCODE,'') != ISNULL(ACGRPCODE,'')  AND ISNULL(COSTID,'')='" & DtCostid.Rows(K).Item(0).ToString & "'"
                strSql += vbCrLf + "  UNION ALL"
                strSql += vbCrLf + "  SELECT DISTINCT '' EXPACGROUPNAME1,NULL EXPENSES2,NULL EXPENSES1,NULL EXPENSES"
                strSql += vbCrLf + "  ,CASE WHEN ACMAINGRPNAME = ACGRPNAME THEN '  ' ELSE '    ' END + ACNAME INCACGROUPNAME1"
                strSql += vbCrLf + "  ,CASE WHEN ACMAINGRPNAME != ACGRPNAME THEN CONVERT(VARCHAR,ISNULL(SUM(CREDIT),0) - ISNULL(SUM(DEBIT),0)) END INCOME2"
                strSql += vbCrLf + "  ,CASE WHEN ACMAINGRPNAME  = ACGRPNAME THEN CONVERT(VARCHAR,ISNULL(SUM(CREDIT),0) - ISNULL(SUM(DEBIT),0)) END INCOME1,NULL INCOME,'3' RESULT,'2' SEP"
                strSql += vbCrLf + "  ,'' EXPACMAINCODE,ACMAINCODE INCACMAINCODE,'' EXPACGROUP,ACGRPCODE INCACGROUP"
                strSql += vbCrLf + "  ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,'' EXPACMAINGRPNAME"
                strSql += vbCrLf + "  ,ACID INCACID,ACNAME INCACNAME,ACGRPNAME INCACGROUPNAME,ACMAINGRPNAME INCACMAINGRPNAME,'' COLHEAD,COSTID"
                strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'I'  AND ISNULL(COSTID,'')='" & DtCostid.Rows(K).Item(0).ToString & "'"
                strSql += vbCrLf + "  GROUP BY ACMAINGRPNAME,ACGRPNAME,ACMAINCODE,ACGRPCODE,ACID,ACNAME,COSTID"
                strSql += vbCrLf + "  HAVING (ISNULL(SUM(CREDIT), 0) - ISNULL(SUM(DEBIT), 0))>0"
                strSql += vbCrLf + "  UNION ALL"
                strSql += vbCrLf + "  SELECT DISTINCT '' EXPACGROUPNAME1,'' EXPENSES2,NULL EXPENSES1,NULL EXPENSES"
                strSql += vbCrLf + "  ,NULL INCACGROUPNAME1,CASE WHEN ACMAINGRPNAME != ACGRPNAME THEN '-------------------' END INCOME2"
                strSql += vbCrLf + "  ,CASE WHEN ACMAINGRPNAME  = ACGRPNAME THEN '-------------------' END INCOME1,NULL INCOME,'4' RESULT,'2' SEP"
                strSql += vbCrLf + "  ,'' EXPACMAINCODE,ACMAINCODE INCACMAINCODE,'' EXPACGROUP,ACGRPCODE INCACGROUP"
                strSql += vbCrLf + "  ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,'' EXPACMAINGRPNAME"
                strSql += vbCrLf + "  ,''INCACID,''INCACNAME,ACGRPNAME INCACGROUPNAME,ACMAINGRPNAME INCACMAINGRPNAME,'S2' COLHEAD,COSTID"
                strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'I'  AND ISNULL(COSTID,'')='" & DtCostid.Rows(K).Item(0).ToString & "'"
                strSql += vbCrLf + "  GROUP BY ACMAINGRPNAME,ACGRPNAME,ACMAINCODE,ACGRPCODE,COSTID"
                strSql += vbCrLf + "  UNION ALL"
                strSql += vbCrLf + "  SELECT DISTINCT ' ' EXPACGROUPNAME1,NULL EXPENSES2,NULL EXPENSES1,NULL EXPENSES,NULL INCACGROUPNAME1"
                strSql += vbCrLf + "  ,NULL INCOME2,CONVERT(VARCHAR,ISNULL(SUM(CREDIT),0) - ISNULL(SUM(DEBIT),0)) INCOME1,NULL INCOME,'5' RESULT,'2' SEP"
                strSql += vbCrLf + "  ,'' EXPACMAINCODE,ACMAINCODE INCACMAINCODE,'' EXPACGROUP,ACGRPCODE INCACGROUP"
                strSql += vbCrLf + "  ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,'' EXPACMAINGRPNAME"
                strSql += vbCrLf + "  ,''INCACID,''INCACNAME,ACGRPNAME INCACGROUPNAME,ACMAINGRPNAME INCACMAINGRPNAME,'S1' COLHEAD,COSTID"
                strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'I' AND ISNULL(ACMAINCODE,'') != ISNULL(ACGRPCODE,'')  AND ISNULL(COSTID,'')='" & DtCostid.Rows(K).Item(0).ToString & "'"
                strSql += vbCrLf + "  GROUP BY ACMAINGRPNAME,ACGRPNAME,ACMAINCODE,ACGRPCODE,COSTID"
                strSql += vbCrLf + "  HAVING (ISNULL(SUM(CREDIT), 0) - ISNULL(SUM(DEBIT), 0))>0"
                strSql += vbCrLf + "  UNION ALL"
                strSql += vbCrLf + "  SELECT DISTINCT '  ' EXPACGROUPNAME1,NULL EXPENSES2,'' EXPENSES1,NULL EXPENSES"
                strSql += vbCrLf + "  ,NULL INCACGROUPNAME1,NULL INCOME2,'-------------------' INCOME1,NULL INCOME,'6' RESULT,'2' SEP"
                strSql += vbCrLf + "  ,ACMAINCODE EXPACMAINCODE,'' INCACMAINCODE,ACGRPCODE EXPACGROUP,'' INCACGROUP"
                strSql += vbCrLf + "  ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME ,'' EXPACMAINGRPNAME"
                strSql += vbCrLf + "  ,''INCACID,''INCACNAME,ACGRPNAME INCACGROUPNAME,ACMAINGRPNAME INCACMAINGRPNAME,'S2' COLHEAD,COSTID"
                strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'I' AND ISNULL(ACMAINCODE,'') != ISNULL(ACGRPCODE,'')  AND ISNULL(COSTID,'')='" & DtCostid.Rows(K).Item(0).ToString & "'"
                strSql += vbCrLf + "  GROUP BY ACMAINGRPNAME,ACGRPNAME,ACMAINCODE,ACGRPCODE,COSTID"
                strSql += vbCrLf + "  UNION ALL"
                strSql += vbCrLf + "  SELECT DISTINCT '' EXPACGROUPNAME1,NULL EXPENSES2,NULL EXPENSES1,NULL EXPENSES"
                strSql += vbCrLf + "  ,NULL INCACGROUPNAME1,NULL INCOME2,NULL INCOME1,CONVERT(VARCHAR,ISNULL(SUM(CREDIT),0) - ISNULL(SUM(DEBIT),0)) INCOME"
                strSql += vbCrLf + "  ,'7' RESULT,'3' SEP,'' EXPACMAINCODE,ACMAINCODE INCACMAINCODE,'' EXPACGROUP,'' INCACGROUP"
                strSql += vbCrLf + "  ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,'' EXPACMAINGRPNAME"
                strSql += vbCrLf + "  ,''INCACID,''INCACNAME,''INCACGROUPNAME,ACMAINGRPNAME INCACMAINGRPNAME,'G1' COLHEAD,COSTID"
                strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'I'  AND ISNULL(COSTID,'')='" & DtCostid.Rows(K).Item(0).ToString & "'"
                strSql += vbCrLf + "  GROUP BY ACMAINGRPNAME,ACMAINCODE,COSTID"
                strSql += vbCrLf + "  HAVING (ISNULL(SUM(CREDIT), 0) - ISNULL(SUM(DEBIT), 0))>0"
                strSql += vbCrLf + "  UNION ALL"
                strSql += vbCrLf + "  SELECT DISTINCT '' EXPACGROUPNAME1,NULL EXPENSES2,NULL EXPENSES1,'' EXPENSES"
                strSql += vbCrLf + "  ,NULL INCACGROUPNAME1,NULL INCOME2,NULL INCOME1,'-------------------' INCOME"
                strSql += vbCrLf + "  ,'8' RESULT,'4' SEP,'' EXPACMAINCODE,ACMAINCODE INCACMAINCODE,'' EXPACGROUP,'' INCACGROUP"
                strSql += vbCrLf + "  ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,'' EXPACMAINGRPNAME"
                strSql += vbCrLf + "  ,''INCACID,''INCACNAME,''INCACGROUPNAME,ACMAINGRPNAME INCACMAINGRPNAME,'G1' COLHEAD,COSTID"
                strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'I'  AND ISNULL(COSTID,'')='" & DtCostid.Rows(K).Item(0).ToString & "'"
                strSql += vbCrLf + "  GROUP BY ACMAINGRPNAME,ACMAINCODE,COSTID"
                'CALNO 290414
                If Val(objGPack.GetSqlValue("SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'E'  AND ISNULL(COSTID,'')='" & DtCostid.Rows(K).Item(0).ToString & "' HAVING (ISNULL(SUM(CREDIT), 0) - ISNULL(SUM(DEBIT), 0))>0", , , )) > 0 Then
                    strSql += vbCrLf + "  UNION ALL"
                    strSql += vbCrLf + "  SELECT DISTINCT NULL EXPACGROUPNAME1,NULL EXPENSES2,NULL EXPENSES1,NULL EXPENSES"
                    strSql += vbCrLf + "  ,'  ' + ACGRPNAME INCACGROUPNAME1,NULL INCOME2,NULL INCOME1,NULL INCOME,'2' RESULT,'2' SEP"
                    strSql += vbCrLf + "  ,'' EXPACMAINCODE,ACMAINCODE INCACMAINCODE,'' EXPACGROUP,ACGRPCODE INCACGROUP"
                    strSql += vbCrLf + "  ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,'' EXPACMAINGRPNAME"
                    strSql += vbCrLf + "  ,''INCACID,''INCACNAME,ACGRPNAME INCACGROUPNAME,ACMAINGRPNAME INCACMAINGRPNAME,'T2' COLHEAD,COSTID"
                    strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'E' AND ISNULL(ACMAINCODE,'') != ISNULL(ACGRPCODE,'')  AND ISNULL(COSTID,'')='" & DtCostid.Rows(K).Item(0).ToString & "'"
                    'strSql += vbCrLf + "  HAVING (ISNULL(SUM(CREDIT), 0) - ISNULL(SUM(DEBIT), 0))>0"
                End If
                strSql += vbCrLf + "  UNION ALL"
                strSql += vbCrLf + "  SELECT DISTINCT '' EXPACGROUPNAME1,NULL EXPENSES2,NULL EXPENSES1,NULL EXPENSES"
                strSql += vbCrLf + "  ,CASE WHEN ACMAINGRPNAME = ACGRPNAME THEN '  ' ELSE '    ' END + ACNAME INCACGROUPNAME1"
                strSql += vbCrLf + "  ,CASE WHEN ACMAINGRPNAME != ACGRPNAME THEN CONVERT(VARCHAR,ISNULL(SUM(CREDIT),0) - ISNULL(SUM(DEBIT),0)) END INCOME2"
                strSql += vbCrLf + "  ,CASE WHEN ACMAINGRPNAME  = ACGRPNAME THEN CONVERT(VARCHAR,ISNULL(SUM(CREDIT),0) - ISNULL(SUM(DEBIT),0)) END INCOME1,NULL INCOME,'3' RESULT,'2' SEP"
                strSql += vbCrLf + "  ,'' EXPACMAINCODE,ACMAINCODE INCACMAINCODE,'' EXPACGROUP,ACGRPCODE INCACGROUP"
                strSql += vbCrLf + "  ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,'' EXPACMAINGRPNAME"
                strSql += vbCrLf + "  ,ACID INCACID,ACNAME INCACNAME,ACGRPNAME INCACGROUPNAME,ACMAINGRPNAME INCACMAINGRPNAME,'' COLHEAD,COSTID"
                strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'E'  AND ISNULL(COSTID,'')='" & DtCostid.Rows(K).Item(0).ToString & "'"
                strSql += vbCrLf + "  GROUP BY ACMAINGRPNAME,ACGRPNAME,ACMAINCODE,ACGRPCODE,ACID,ACNAME,COSTID"
                strSql += vbCrLf + "  HAVING (ISNULL(SUM(CREDIT), 0) - ISNULL(SUM(DEBIT), 0))>0"
                strSql += vbCrLf + "  UNION ALL"
                strSql += vbCrLf + "  SELECT DISTINCT '' EXPACGROUPNAME1,'' EXPENSES2,NULL EXPENSES1,NULL EXPENSES"
                strSql += vbCrLf + "  ,NULL INCACGROUPNAME1,CASE WHEN ACMAINGRPNAME != ACGRPNAME THEN '-------------------' END INCOME2"
                strSql += vbCrLf + "  ,CASE WHEN ACMAINGRPNAME  = ACGRPNAME THEN '-------------------' END INCOME1,NULL INCOME,'4' RESULT,'2' SEP"
                strSql += vbCrLf + "  ,'' EXPACMAINCODE,ACMAINCODE INCACMAINCODE,'' EXPACGROUP,ACGRPCODE INCACGROUP"
                strSql += vbCrLf + "  ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,'' EXPACMAINGRPNAME"
                strSql += vbCrLf + "  ,''INCACID,''INCACNAME,ACGRPNAME INCACGROUPNAME,ACMAINGRPNAME INCACMAINGRPNAME,'S2' COLHEAD,COSTID"
                strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'E'  AND ISNULL(COSTID,'')='" & DtCostid.Rows(K).Item(0).ToString & "'"
                strSql += vbCrLf + "  GROUP BY ACMAINGRPNAME,ACGRPNAME,ACMAINCODE,ACGRPCODE,COSTID"
                strSql += vbCrLf + "  HAVING (ISNULL(SUM(CREDIT), 0) - ISNULL(SUM(DEBIT), 0))>0"
                strSql += vbCrLf + "  UNION ALL"
                strSql += vbCrLf + "  SELECT DISTINCT ' ' EXPACGROUPNAME1,NULL EXPENSES2,NULL EXPENSES1,NULL EXPENSES,NULL INCACGROUPNAME1"
                strSql += vbCrLf + "  ,NULL INCOME2,CONVERT(VARCHAR,ISNULL(SUM(CREDIT),0) - ISNULL(SUM(DEBIT),0)) INCOME1,NULL INCOME,'5' RESULT,'2' SEP"
                strSql += vbCrLf + "  ,'' EXPACMAINCODE,ACMAINCODE INCACMAINCODE,'' EXPACGROUP,ACGRPCODE INCACGROUP"
                strSql += vbCrLf + "  ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,'' EXPACMAINGRPNAME"
                strSql += vbCrLf + "  ,''INCACID,''INCACNAME,ACGRPNAME INCACGROUPNAME,ACMAINGRPNAME INCACMAINGRPNAME,'S1' COLHEAD,COSTID"
                strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'E' AND ISNULL(ACMAINCODE,'') != ISNULL(ACGRPCODE,'')  AND ISNULL(COSTID,'')='" & DtCostid.Rows(K).Item(0).ToString & "'"
                strSql += vbCrLf + "  GROUP BY ACMAINGRPNAME,ACGRPNAME,ACMAINCODE,ACGRPCODE,COSTID"
                strSql += vbCrLf + "  HAVING (ISNULL(SUM(CREDIT), 0) - ISNULL(SUM(DEBIT), 0))>0"
                strSql += vbCrLf + "  UNION ALL"
                strSql += vbCrLf + "  SELECT DISTINCT '  ' EXPACGROUPNAME1,NULL EXPENSES2,'' EXPENSES1,NULL EXPENSES"
                strSql += vbCrLf + "  ,NULL INCACGROUPNAME1,NULL INCOME2,'-------------------' INCOME1,NULL INCOME,'6' RESULT,'2' SEP"
                strSql += vbCrLf + "  ,ACMAINCODE EXPACMAINCODE,'' INCACMAINCODE,ACGRPCODE EXPACGROUP,'' INCACGROUP"
                strSql += vbCrLf + "  ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME ,'' EXPACMAINGRPNAME"
                strSql += vbCrLf + "  ,''INCACID,''INCACNAME,ACGRPNAME INCACGROUPNAME,ACMAINGRPNAME INCACMAINGRPNAME,'S2' COLHEAD,COSTID"
                strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'E' AND ISNULL(ACMAINCODE,'') != ISNULL(ACGRPCODE,'')  AND ISNULL(COSTID,'')='" & DtCostid.Rows(K).Item(0).ToString & "'"
                strSql += vbCrLf + "  GROUP BY ACMAINGRPNAME,ACGRPNAME,ACMAINCODE,ACGRPCODE,COSTID"
                strSql += vbCrLf + "  HAVING (ISNULL(SUM(CREDIT), 0) - ISNULL(SUM(DEBIT), 0))>0"
                strSql += vbCrLf + "  UNION ALL"
                strSql += vbCrLf + "  SELECT DISTINCT '' EXPACGROUPNAME1,NULL EXPENSES2,NULL EXPENSES1,NULL EXPENSES"
                strSql += vbCrLf + "  ,NULL INCACGROUPNAME1,NULL INCOME2,NULL INCOME1,CONVERT(VARCHAR,ISNULL(SUM(CREDIT),0) - ISNULL(SUM(DEBIT),0)) INCOME"
                strSql += vbCrLf + "  ,'7' RESULT,'3' SEP,'' EXPACMAINCODE,ACMAINCODE INCACMAINCODE,'' EXPACGROUP,'' INCACGROUP"
                strSql += vbCrLf + "  ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,'' EXPACMAINGRPNAME"
                strSql += vbCrLf + "  ,''INCACID,''INCACNAME,''INCACGROUPNAME,ACMAINGRPNAME INCACMAINGRPNAME,'G1' COLHEAD,COSTID"
                strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'E'  AND ISNULL(COSTID,'')='" & DtCostid.Rows(K).Item(0).ToString & "'"
                strSql += vbCrLf + "  GROUP BY ACMAINGRPNAME,ACMAINCODE,COSTID"
                strSql += vbCrLf + "  HAVING (ISNULL(SUM(CREDIT), 0) - ISNULL(SUM(DEBIT), 0))>0"
                strSql += vbCrLf + "  UNION ALL"
                strSql += vbCrLf + "  SELECT DISTINCT '' EXPACGROUPNAME1,NULL EXPENSES2,NULL EXPENSES1,'' EXPENSES"
                strSql += vbCrLf + "  ,NULL INCACGROUPNAME1,NULL INCOME2,NULL INCOME1,'-------------------' INCOME"
                strSql += vbCrLf + "  ,'8' RESULT,'4' SEP,'' EXPACMAINCODE,ACMAINCODE INCACMAINCODE,'' EXPACGROUP,'' INCACGROUP"
                strSql += vbCrLf + "  ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,'' EXPACMAINGRPNAME"
                strSql += vbCrLf + "  ,''INCACID,''INCACNAME,''INCACGROUPNAME,ACMAINGRPNAME INCACMAINGRPNAME,'G1' COLHEAD,COSTID"
                strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'E'  AND ISNULL(COSTID,'')='" & DtCostid.Rows(K).Item(0).ToString & "'"
                strSql += vbCrLf + "  GROUP BY ACMAINGRPNAME,ACMAINCODE,COSTID"
                strSql += vbCrLf + "  HAVING (ISNULL(SUM(CREDIT), 0) - ISNULL(SUM(DEBIT), 0))>0"
                'CALNO 290414
                strSql += vbCrLf + "  ORDER BY INCACMAINGRPNAME,SEP,INCACGROUPNAME,RESULT,INCACNAME,COSTID"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
            End If

        Next



        strSql = "  IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & userId.ToString() & "TRADE5')"
        strSql += vbCrLf + "  DROP TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE5"
        strSql += vbCrLf + "  SELECT "
        For j As Integer = 0 To DtCostid.Rows.Count - 1
            strSql += vbCrLf + "  T3" & j.ToString & ".EXPACGROUPNAME1 AS EXPACGROUPNAME1" & DtCostid.Rows(j).Item(0).ToString.Trim & ",T3" & j.ToString & ".EXPENSES1 AS EXPENSES1" & DtCostid.Rows(j).Item(0).ToString.Trim & ","
            strSql += vbCrLf + "  T3" & j.ToString & ".EXPENSES2 AS EXPENSES2" & DtCostid.Rows(j).Item(0).ToString.Trim & ",T3" & j.ToString & ".EXPENSES EXPENSES" & DtCostid.Rows(j).Item(0).ToString.Trim & ","
            strSql += vbCrLf + "  T4" & j.ToString & ".INCACGROUPNAME1 INCACGROUPNAME1" & DtCostid.Rows(j).Item(0).ToString.Trim & ",T4" & j.ToString & ".INCOME1 AS INCOME1" & DtCostid.Rows(j).Item(0).ToString.Trim & ","
            strSql += vbCrLf + "  T4" & j.ToString & ".INCOME2 AS INCOME2" & DtCostid.Rows(j).Item(0).ToString.Trim & ",T4" & j.ToString & ".INCOME AS INCOME" & DtCostid.Rows(j).Item(0).ToString.Trim & ","
            strSql += vbCrLf + "  T3" & j.ToString & ".COLHEAD COLHEAD3" & DtCostid.Rows(j).Item(0).ToString.Trim & ",T4" & j.ToString & ".COLHEAD COLHEAD4" & DtCostid.Rows(j).Item(0).ToString.Trim & ","
            strSql += vbCrLf + "  T3" & j.ToString & ".RESULT RESULT3" & DtCostid.Rows(j).Item(0).ToString.Trim & ",T4" & j.ToString & ".RESULT RESULT4" & DtCostid.Rows(j).Item(0).ToString.Trim & ","
        Next
        strSql += vbCrLf + "  T30.SNO SNO3,T40.SNO SNO4,T30.SEP SEP3,T40.SEP SEP4"
        strSql += vbCrLf + "  INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE5"
        Dim joinTbl As String = ""
        Dim MaxCount As Integer
        Dim ChkCount As Integer
        For j As Integer = 0 To DtCostid.Rows.Count - 1
            If j = 0 Then
                MaxCount = Val(objGPack.GetSqlValue("SELECT  COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE3" & DtCostid.Rows(j).Item(0).ToString.Trim, , "", ))
                strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE3" & DtCostid.Rows(j).Item(0).ToString.Trim & " AS T3" & j.ToString & " "
                joinTbl = "T3" & j.ToString
                strSql += vbCrLf + "  FULL JOIN TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE4" & DtCostid.Rows(j).Item(0).ToString.Trim & " AS T4" & j.ToString & " ON T4" & j.ToString & ".SNO =" & joinTbl & ".SNO"
                ChkCount = Val(objGPack.GetSqlValue("SELECT  COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE4" & DtCostid.Rows(j).Item(0).ToString.Trim, , "", ))
                If ChkCount > MaxCount Then MaxCount = ChkCount : joinTbl = "T4" & j.ToString
            Else
                strSql += vbCrLf + "  FULL JOIN TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE3" & DtCostid.Rows(j).Item(0).ToString.Trim & " AS T3" & j.ToString & " ON T3" & j.ToString & ".SNO = " & joinTbl & ".SNO"
                ChkCount = Val(objGPack.GetSqlValue("SELECT  COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE3" & DtCostid.Rows(j).Item(0).ToString.Trim, , "", ))
                If ChkCount > MaxCount Then MaxCount = ChkCount : joinTbl = "T3" & j.ToString
                strSql += vbCrLf + "  FULL JOIN TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE4" & DtCostid.Rows(j).Item(0).ToString.Trim & " AS T4" & j.ToString & " ON T4" & j.ToString & ".SNO = " & joinTbl & ".SNO"
                ChkCount = Val(objGPack.GetSqlValue("SELECT  COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE4" & DtCostid.Rows(j).Item(0).ToString.Trim, , "", ))
                If ChkCount > MaxCount Then MaxCount = ChkCount : joinTbl = "T4" & j.ToString
            End If
        Next
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " SELECT DISTINCT "
        For j As Integer = 0 To DtCostid.Rows.Count - 1
            strSql += vbCrLf + " 'TRADING - 01/" & CnStartMonth & "/" & (IIf(dtpDate.Value.[Date].Month > 3, dtpDate.Value.[Date].Year, dtpDate.Value.[Date].Year - 1)).ToString() & " - " & dtpDate.Text.Replace("-", "/") & "' EXPACGROUPNAME1" & DtCostid.Rows(j).Item(0).ToString.Trim & ""
            strSql += vbCrLf + " ,' ' EXPENSES1" & DtCostid.Rows(j).Item(0).ToString.Trim & ",' ' EXPENSES2" & DtCostid.Rows(j).Item(0).ToString.Trim & ",' 'EXPENSES" & DtCostid.Rows(j).Item(0).ToString.Trim & " "
            strSql += vbCrLf + " ,' ' INCACGROUPNAME1" & DtCostid.Rows(j).Item(0).ToString.Trim & ",' 'INCOME1" & DtCostid.Rows(j).Item(0).ToString.Trim & ",' 'INCOME2" & DtCostid.Rows(j).Item(0).ToString.Trim & ",' 'INCOME" & DtCostid.Rows(j).Item(0).ToString.Trim & ","
            If j = DtCostid.Rows.Count - 1 Then
                strSql += vbCrLf + " 'T' COLHEAD3" & DtCostid.Rows(j).Item(0).ToString.Trim & ",'T' COLHEAD4" & DtCostid.Rows(j).Item(0).ToString.Trim & " "
            Else
                strSql += vbCrLf + " 'T' COLHEAD3" & DtCostid.Rows(j).Item(0).ToString.Trim & ",'T' COLHEAD4" & DtCostid.Rows(j).Item(0).ToString.Trim & ", "
            End If
        Next
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE5"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT "
        For j As Integer = 0 To DtCostid.Rows.Count - 1
            strSql += vbCrLf + " EXPACGROUPNAME1" & DtCostid.Rows(j).Item(0).ToString.Trim & ","
            strSql += vbCrLf + " CASE WHEN RESULT3" & DtCostid.Rows(j).Item(0).ToString.Trim & " NOT IN (4,6,8) AND ISNULL(EXPENSES1" & DtCostid.Rows(j).Item(0).ToString.Trim & ",'') != '' THEN " & cnStockDb & ".[DBO].FUNC_RUPEES_WITHCOMMA(CONVERT(NUMERIC(15,2),CASE WHEN ISNULL(EXPENSES1" & DtCostid.Rows(j).Item(0).ToString.Trim & ",'') = '' THEN '0' ELSE EXPENSES1" & DtCostid.Rows(j).Item(0).ToString.Trim & " END),'D','Y') ELSE EXPENSES1" & DtCostid.Rows(j).Item(0).ToString.Trim & " END  EXPENSES1" & DtCostid.Rows(j).Item(0).ToString.Trim & ","
            strSql += vbCrLf + " CASE WHEN RESULT3" & DtCostid.Rows(j).Item(0).ToString.Trim & " NOT IN (4,6,8) AND ISNULL(EXPENSES2" & DtCostid.Rows(j).Item(0).ToString.Trim & ",'') != '' THEN " & cnStockDb & ".[DBO].FUNC_RUPEES_WITHCOMMA(CONVERT(NUMERIC(15,2),CASE WHEN ISNULL(EXPENSES2" & DtCostid.Rows(j).Item(0).ToString.Trim & ",'') = '' THEN '0' ELSE EXPENSES2" & DtCostid.Rows(j).Item(0).ToString.Trim & " END),'D','Y') ELSE EXPENSES2" & DtCostid.Rows(j).Item(0).ToString.Trim & " END EXPENSES2" & DtCostid.Rows(j).Item(0).ToString.Trim & ","
            strSql += vbCrLf + " CASE WHEN RESULT3" & DtCostid.Rows(j).Item(0).ToString.Trim & " NOT IN (4,6,8) AND ISNULL(EXPENSES" & DtCostid.Rows(j).Item(0).ToString & ",'') != '' THEN " & cnStockDb & ".[DBO].FUNC_RUPEES_WITHCOMMA(CONVERT(NUMERIC(15,2),CASE WHEN ISNULL(EXPENSES" & DtCostid.Rows(j).Item(0).ToString.Trim & ",'') = '' THEN '0' ELSE EXPENSES" & DtCostid.Rows(j).Item(0).ToString.Trim & " END),'D','Y') ELSE EXPENSES" & DtCostid.Rows(j).Item(0).ToString.Trim & " END EXPENSES" & DtCostid.Rows(j).Item(0).ToString.Trim & ","
            strSql += vbCrLf + " INCACGROUPNAME1" & DtCostid.Rows(j).Item(0).ToString.Trim & ","
            strSql += vbCrLf + " CASE WHEN RESULT4" & DtCostid.Rows(j).Item(0).ToString.Trim & " NOT IN (4,6,8) AND ISNULL(INCOME1" & DtCostid.Rows(j).Item(0).ToString.Trim & ",'') != '' THEN " & cnStockDb & ".[DBO].FUNC_RUPEES_WITHCOMMA(CONVERT(NUMERIC(15,2),CASE WHEN ISNULL(INCOME1" & DtCostid.Rows(j).Item(0).ToString & ",'') = '' THEN '0' ELSE INCOME1" & DtCostid.Rows(j).Item(0).ToString.Trim & " END),'D','Y') ELSE INCOME1" & DtCostid.Rows(j).Item(0).ToString.Trim & " END  INCOME1" & DtCostid.Rows(j).Item(0).ToString.Trim & ","
            strSql += vbCrLf + " CASE WHEN RESULT4" & DtCostid.Rows(j).Item(0).ToString.Trim & " NOT IN (4,6,8) AND ISNULL(INCOME2" & DtCostid.Rows(j).Item(0).ToString.Trim & ",'') != '' THEN " & cnStockDb & ".[DBO].FUNC_RUPEES_WITHCOMMA(CONVERT(NUMERIC(15,2),CASE WHEN ISNULL(INCOME2" & DtCostid.Rows(j).Item(0).ToString & ",'') = '' THEN '0' ELSE INCOME2" & DtCostid.Rows(j).Item(0).ToString.Trim & " END),'D','Y') ELSE INCOME2" & DtCostid.Rows(j).Item(0).ToString.Trim & " END INCOME2" & DtCostid.Rows(j).Item(0).ToString.Trim & ","
            strSql += vbCrLf + " CASE WHEN RESULT4" & DtCostid.Rows(j).Item(0).ToString.Trim & " NOT IN (4,6,8) AND ISNULL(INCOME" & DtCostid.Rows(j).Item(0).ToString.Trim & ",'') != '' THEN " & cnStockDb & ".[DBO].FUNC_RUPEES_WITHCOMMA(CONVERT(NUMERIC(15,2),CASE WHEN ISNULL(INCOME" & DtCostid.Rows(j).Item(0).ToString & ",'') = '' THEN '0' ELSE INCOME" & DtCostid.Rows(j).Item(0).ToString.Trim & " END),'D','Y') ELSE INCOME" & DtCostid.Rows(j).Item(0).ToString.Trim & " END INCOME" & DtCostid.Rows(j).Item(0).ToString.Trim & ","
            If j = DtCostid.Rows.Count - 1 Then
                strSql += vbCrLf + " COLHEAD3" & DtCostid.Rows(j).Item(0).ToString.Trim & ",COLHEAD4" & DtCostid.Rows(j).Item(0).ToString.Trim & ""
            Else
                strSql += vbCrLf + " COLHEAD3" & DtCostid.Rows(j).Item(0).ToString.Trim & ",COLHEAD4" & DtCostid.Rows(j).Item(0).ToString.Trim & ","
            End If
        Next
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE5"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT"
        For j As Integer = 0 To DtCostid.Rows.Count - 1
            strSql += vbCrLf + " '' EXPACGROUPNAME1" & DtCostid.Rows(j).Item(0).ToString.Trim & ",NULL EXPENSES1" & DtCostid.Rows(j).Item(0).ToString.Trim & ",NULL EXPENSES2" & DtCostid.Rows(j).Item(0).ToString.Trim & ",CONVERT(VARCHAR," & cnStockDb & ".[DBO].FUNC_RUPEES_WITHCOMMA(SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT3" & DtCostid.Rows(j).Item(0).ToString.Trim & " IN ('0','7') AND ISNULL(EXPENSES" & DtCostid.Rows(j).Item(0).ToString.Trim & ",'') = '' THEN CONVERT(NUMERIC(15,2),CASE WHEN ISNULL(EXPENSES" & DtCostid.Rows(j).Item(0).ToString.Trim & ",'') = '' THEN '0' ELSE EXPENSES" & DtCostid.Rows(j).Item(0).ToString.Trim & " END) ELSE '0' END)),'D','Y')) EXPENSES" & DtCostid.Rows(j).Item(0).ToString.Trim & ","
            strSql += vbCrLf + " '' INCACGROUPNAME1" & DtCostid.Rows(j).Item(0).ToString.Trim & ",NULL INCOME1" & DtCostid.Rows(j).Item(0).ToString.Trim & ",NULL INCOME2" & DtCostid.Rows(j).Item(0).ToString.Trim & ",CONVERT(VARCHAR," & cnStockDb & ".[DBO].FUNC_RUPEES_WITHCOMMA(SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT4" & DtCostid.Rows(j).Item(0).ToString.Trim & " IN ('0','7') AND ISNULL(INCOME" & DtCostid.Rows(j).Item(0).ToString.Trim & ",'') = '' THEN CONVERT(NUMERIC(15,2),CASE WHEN ISNULL(INCOME" & DtCostid.Rows(j).Item(0).ToString.Trim & ",'') = '' THEN '0' ELSE INCOME" & DtCostid.Rows(j).Item(0).ToString.Trim & " END) ELSE '0' END)),'D','Y')) INCOME" & DtCostid.Rows(j).Item(0).ToString.Trim & ","
            If j = DtCostid.Rows.Count - 1 Then
                strSql += vbCrLf + "  'G1' COLHEAD3" & DtCostid.Rows(j).Item(0).ToString.Trim & ",'G1' COLHEAD4" & DtCostid.Rows(j).Item(0).ToString.Trim & ""
            Else
                strSql += vbCrLf + "  'G1' COLHEAD3" & DtCostid.Rows(j).Item(0).ToString.Trim & ",'G1' COLHEAD4" & DtCostid.Rows(j).Item(0).ToString.Trim & ","
            End If
        Next
        strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE5"
        strSql += vbCrLf + "  UNION ALL"
        strSql += vbCrLf + "  SELECT DISTINCT "
        For j As Integer = 0 To DtCostid.Rows.Count - 1
            strSql += vbCrLf + "  '' EXPACGROUPNAME1" & DtCostid.Rows(j).Item(0).ToString.Trim & ",NULL EXPENSES1" & DtCostid.Rows(j).Item(0).ToString.Trim & ",NULL EXPENSES2" & DtCostid.Rows(j).Item(0).ToString.Trim & ",'-------------------' EXPENSES" & DtCostid.Rows(j).Item(0).ToString.Trim & ","
            strSql += vbCrLf + "  '' INCACGROUPNAME1" & DtCostid.Rows(j).Item(0).ToString.Trim & ",NULL INCOME1" & DtCostid.Rows(j).Item(0).ToString.Trim & ",NULL INCOME2" & DtCostid.Rows(j).Item(0).ToString.Trim & ",'-------------------' INCOME" & DtCostid.Rows(j).Item(0).ToString.Trim & ","
            If j = DtCostid.Rows.Count - 1 Then
                strSql += vbCrLf + "  'G1' COLHEAD3" & DtCostid.Rows(j).Item(0).ToString.Trim & ",'G1' COLHEAD4" & DtCostid.Rows(j).Item(0).ToString.Trim & ""
            Else
                strSql += vbCrLf + "  'G1' COLHEAD3" & DtCostid.Rows(j).Item(0).ToString.Trim & ",'G1' COLHEAD4" & DtCostid.Rows(j).Item(0).ToString.Trim & ","
            End If
        Next
        strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE5"
        strSql += vbCrLf + "  UNION ALL"
        strSql += vbCrLf + "  SELECT"
        For j As Integer = 0 To DtCostid.Rows.Count - 1
            strSql += vbCrLf + "  CASE WHEN SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT3" & DtCostid.Rows(j).Item(0).ToString.Trim & " IN ('0','7') THEN EXPENSES" & DtCostid.Rows(j).Item(0).ToString.Trim & " ELSE '0' END)) - SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT4" & DtCostid.Rows(j).Item(0).ToString.Trim & " IN ('0','7') THEN INCOME" & DtCostid.Rows(j).Item(0).ToString.Trim & " ELSE '0' END)) > 0 THEN 'GROSS LOSS' END EXPACGROUPNAME1" & DtCostid.Rows(j).Item(0).ToString.Trim & ",NULL EXPENSES1" & DtCostid.Rows(j).Item(0).ToString & ",NULL EXPENSES2" & DtCostid.Rows(j).Item(0).ToString.Trim & ","
            strSql += vbCrLf + "  CONVERT(VARCHAR,CASE WHEN SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT3" & DtCostid.Rows(j).Item(0).ToString.Trim & " IN ('0','7') THEN EXPENSES" & DtCostid.Rows(j).Item(0).ToString.Trim & " ELSE '0' END)) - SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT4" & DtCostid.Rows(j).Item(0).ToString.Trim & " IN ('0','7') THEN INCOME" & DtCostid.Rows(j).Item(0).ToString.Trim & " ELSE '0' END)) > 0 THEN " & cnStockDb & ".[DBO].FUNC_RUPEES_WITHCOMMA(SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT3" & DtCostid.Rows(j).Item(0).ToString.Trim & " IN ('0','7') THEN EXPENSES" & DtCostid.Rows(j).Item(0).ToString & " ELSE '0' END)) - SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT4" & DtCostid.Rows(j).Item(0).ToString.Trim & " IN ('0','7') THEN INCOME" & DtCostid.Rows(j).Item(0).ToString.Trim & " ELSE '0' END)),'D','Y') END) EXPENSES" & DtCostid.Rows(j).Item(0).ToString.Trim & ","
            strSql += vbCrLf + "  CASE WHEN SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT4" & DtCostid.Rows(j).Item(0).ToString.Trim & " IN ('0','7') THEN INCOME" & DtCostid.Rows(j).Item(0).ToString.Trim & " ELSE '0' END)) - SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT3" & DtCostid.Rows(j).Item(0).ToString.Trim & " IN ('0','7') THEN EXPENSES" & DtCostid.Rows(j).Item(0).ToString.Trim & " ELSE '0' END)) > 0 THEN 'GROSS PROFIT' END INCACGROUPNAME1" & DtCostid.Rows(j).Item(0).ToString.Trim & " ,NULL INCOME1" & DtCostid.Rows(j).Item(0).ToString.Trim & ",NULL INCOME2" & DtCostid.Rows(j).Item(0).ToString & ","
            strSql += vbCrLf + "  CONVERT(VARCHAR,CASE WHEN SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT4" & DtCostid.Rows(j).Item(0).ToString.Trim & " IN ('0','7') THEN INCOME" & DtCostid.Rows(j).Item(0).ToString.Trim & " ELSE '0' END)) - SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT3" & DtCostid.Rows(j).Item(0).ToString.Trim & " IN ('0','7') THEN EXPENSES" & DtCostid.Rows(j).Item(0).ToString.Trim & " ELSE '0' END)) > 0 THEN " & cnStockDb & ".[DBO].FUNC_RUPEES_WITHCOMMA(SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT4" & DtCostid.Rows(j).Item(0).ToString.Trim & " IN ('0','7') THEN INCOME" & DtCostid.Rows(j).Item(0).ToString.Trim & " ELSE '0' END)) - SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT3" & DtCostid.Rows(j).Item(0).ToString.Trim & " IN ('0','7') THEN EXPENSES" & DtCostid.Rows(j).Item(0).ToString.Trim & " ELSE '0' END)),'D','Y') END) INCOME" & DtCostid.Rows(j).Item(0).ToString.Trim & ","
            If j = DtCostid.Rows.Count - 1 Then
                strSql += vbCrLf + "  'G' COLHEAD3" & DtCostid.Rows(j).Item(0).ToString.Trim & ",'G' COLHEAD4" & DtCostid.Rows(j).Item(0).ToString.Trim & ""
            Else
                strSql += vbCrLf + "  'G' COLHEAD3" & DtCostid.Rows(j).Item(0).ToString.Trim & ",'G' COLHEAD4" & DtCostid.Rows(j).Item(0).ToString.Trim & ","
            End If
        Next
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE5"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count <= 2 Then
            dt = New DataTable()
        End If
        'gridView.DataSource = dt
    End Sub
    Private Sub funcPLMulCost(ByVal strcompid As String, ByVal strCostId As String, ByVal summary As Boolean)
        strSql = " IF EXISTS (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & userId.ToString() & "PROFITLOSS1')"
        strSql += vbCrLf + " DROP TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS1"
        strSql += vbCrLf + " CREATE TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS1"
        strSql += vbCrLf + " (COMPANYID VARCHAR(3),COSTID VARCHAR(2),ACID VARCHAR(25),TRANMODE VARCHAR(1),AMOUNT NUMERIC(15,2),DEBIT NUMERIC(15,2),CREDIT NUMERIC(15,2)"
        strSql += vbCrLf + " ,TRANDATE SMALLDATETIME,PAYMODE VARCHAR(20),TRANNO VARCHAR(25),SEP VARCHAR(1))"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " INSERT INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS1"
        strSql += vbCrLf + " (COMPANYID,COSTID,ACID,TRANMODE,AMOUNT,DEBIT,CREDIT,TRANDATE,PAYMODE,TRANNO,SEP)"
        strSql += vbCrLf + " SELECT "
        strSql += vbCrLf + "  COMPANYID,ISNULL(COSTID,'') COSTID,ACCODE ACID,CASE WHEN ISNULL(DEBIT,0) > ISNULL(CREDIT,0) THEN 'D' ELSE 'C' END TRANMODE"
        strSql += vbCrLf + " ,CASE WHEN ISNULL(DEBIT,0) <> 0 THEN DEBIT ELSE CREDIT END AMOUNT"
        strSql += vbCrLf + " ,DEBIT,CREDIT,NULL TRANDATE,'' PAYMODE,NULL TRANNO,'O' SEP"
        strSql += vbCrLf + " FROM " & cnStockDb & "..OPENTRAILBALANCE WHERE ISNULL(COMPANYID,'') = '" & strcompid & "'"
        strSql += strCostId
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT "
        strSql += vbCrLf + "  COMPANYID,ISNULL(COSTID,'') COSTID,ACCODE ACID,TRANMODE,AMOUNT"
        strSql += vbCrLf + " ,CASE WHEN TRANMODE = 'D' THEN AMOUNT END DEBIT,CASE WHEN TRANMODE = 'C' THEN AMOUNT END CREDIT"
        strSql += vbCrLf + " ,TRANDATE,PAYMODE,TRANNO,'T' SEP"
        strSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN WHERE ISNULL(COMPANYID,'') = '" & strcompid & "' AND TRANDATE >= '" & cnTranFromDate & "' AND TRANDATE <= '" & dtpDate.Value.Date.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') != 'Y'"
        strSql += strCostId
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " IF EXISTS (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & userId.ToString() & "PROFITLOSS2')"
        strSql += vbCrLf + " DROP TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS2"
        If summary Then
            strSql += vbCrLf + "  SELECT '' ACID,COSTID,G.ACGRPCODE,SG.ACGRPNAME ACGRPNAME,G.ACMAINCODE,SG.GRPLEDGER,SG.GRPTYPE"
            'strSql += vbCrLf + "   A.ACCODE ACID,G.ACGRPCODE,ACGRPNAME ACGRPNAME,ACMAINCODE,G.GRPLEDGER,G.GRPTYPE" 
            strSql += vbCrLf + "  ,SUM(DEBIT) DEBIT,SUM(CREDIT) CREDIT"
            strSql += vbCrLf + "  ,'' ACNAME,'' TRANDATE,CASE WHEN SUM(AMOUNT) > 0 THEN 'D' ELSE 'C' END TRANMODE,SUM(AMOUNT) AMOUNT,'' PAYMODE,'' TRANNO"
            strSql += vbCrLf + "  ,(SELECT ACGRPNAME FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPCODE = G.ACMAINCODE) ACMAINGRPNAME"
            strSql += vbCrLf + "  INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS2"
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS1 AS T INNER JOIN " & cnAdminDb & "..ACHEAD AS A"
            strSql += vbCrLf + "  ON T.ACID = A.ACCODE INNER JOIN " & cnAdminDb & "..ACGROUP AS G ON G.ACGRPCODE = A.ACGRPCODE"
            strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ACGROUP AS SG ON SG.ACGRPCODE = A.ACGRPCODE"
            strSql += vbCrLf + "  AND (SG.GRPLEDGER IN ('P') OR G.GRPLEDGER IN ('P')) "
            strSql += vbCrLf + "  GROUP BY G.ACGRPCODE,SG.ACGRPNAME,G.ACMAINCODE,SG.GRPLEDGER,SG.GRPTYPE,COSTID"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        Else

            strSql += vbCrLf + " SELECT A.ACCODE ACID,COSTID,G.ACGRPCODE,ACGRPNAME,ACMAINCODE,GRPLEDGER,GRPTYPE"
            strSql += vbCrLf + " ,DEBIT,CREDIT"
            strSql += vbCrLf + " ,ACNAME ACNAME,TRANDATE,TRANMODE,AMOUNT,PAYMODE,TRANNO"
            strSql += vbCrLf + " ,(SELECT ACGRPNAME FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPCODE = G.ACMAINCODE) ACMAINGRPNAME"
            strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS2"
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS1 AS T INNER JOIN " & cnAdminDb & "..ACHEAD AS A"
            strSql += vbCrLf + " ON T.ACID = A.ACCODE INNER JOIN " & cnAdminDb & "..ACGROUP AS G ON G.ACGRPCODE = A.ACGRPCODE"
            strSql += vbCrLf + " AND GRPLEDGER IN ('P')"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End If
        strSql = " IF EXISTS (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & userId.ToString() & "PROFITLOSS0')"
        strSql += vbCrLf + " DROP TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS0"
        If dt.Rows.Count > 0 Then
            For K As Integer = 0 To DtCostid.Rows.Count - 1
                Dim ExpProfit() As DataRow = dt.Select("EXPACGROUPNAME1" & DtCostid.Rows(K).Item(0).ToString.Trim & "='GROSS LOSS'")
                Dim IncProfit() As DataRow = dt.Select("INCACGROUPNAME1" & DtCostid.Rows(K).Item(0).ToString.Trim & "='GROSS PROFIT'")
                If K = 0 Then
                    If IncProfit.Length > 0 Then
                        strSql += vbCrLf + " SELECT 'TRADING A/C' PARTICULARS,'" & DtCostid.Rows(K).Item(0).ToString.Trim & "' COSTID"
                        strSql += vbCrLf + "  ,CAST(((-1)* " & IncProfit(0).Item("INCOME" & DtCostid.Rows(K).Item(0).ToString.Trim).ToString.Replace(",", "") & ") AS NUMERIC(38,2)) AMOUNT"
                        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS0"
                    ElseIf ExpProfit.Length > 0 Then
                        strSql += vbCrLf + " SELECT "
                        strSql += vbCrLf + " 'TRADING A/C' PARTICULARS,'" & DtCostid.Rows(K).Item(0).ToString.Trim & "' COSTID"
                        strSql += vbCrLf + " ,CAST((" & ExpProfit(0).Item("EXPENSES" & DtCostid.Rows(K).Item(0).ToString.Trim).ToString.Replace(",", "") & ") AS NUMERIC(38,2)) AMOUNT"
                        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS0"
                    End If
                Else
                    If IncProfit.Length > 0 Then
                        strSql += vbCrLf + " UNION ALL"
                        strSql += vbCrLf + " SELECT 'TRADING A/C' PARTICULARS,'" & DtCostid.Rows(K).Item(0).ToString.Trim & "' COSTID"
                        strSql += vbCrLf + "  ,CAST(((-1)* " & IncProfit(0).Item("INCOME" & DtCostid.Rows(K).Item(0).ToString.Trim).ToString.Replace(",", "") & ") AS NUMERIC(38,2)) AMOUNT"
                    ElseIf ExpProfit.Length > 0 Then
                        strSql += vbCrLf + " UNION ALL"
                        strSql += vbCrLf + " SELECT "
                        strSql += vbCrLf + " 'TRADING A/C' PARTICULARS,'" & DtCostid.Rows(K).Item(0).ToString.Trim & "' COSTID"
                        strSql += vbCrLf + " ,CAST((" & ExpProfit(0).Item("EXPENSES" & DtCostid.Rows(K).Item(0).ToString.Trim).ToString.Replace(",", "") & ") AS NUMERIC(38,2)) AMOUNT"
                    End If
                End If
            Next
        Else
            strSql += vbCrLf + " SELECT 'TRADING A/C' PARTICULARS,''COSTID"
            strSql += vbCrLf + "  ,0 AMOUNT"
            strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS0"
        End If

        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        For K As Integer = 0 To DtCostid.Rows.Count - 1
            strSql = " IF EXISTS (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & userId.ToString() & "PROFITLOSS3" & DtCostid.Rows(K).Item(0).ToString.Trim & "')"
            strSql += vbCrLf + " DROP TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS3" & DtCostid.Rows(K).Item(0).ToString.Trim & ""
            strSql += vbCrLf + " CREATE TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS3" & DtCostid.Rows(K).Item(0).ToString.Trim & ""
            strSql += vbCrLf + " (EXPACGROUPNAME1 VARCHAR(250),EXPENSES1 VARCHAR(50),EXPENSES2 VARCHAR(50),EXPENSES VARCHAR(50)"
            strSql += vbCrLf + " ,INCACGROUPNAME1 VARCHAR(250),INCOME1 VARCHAR(50),INCOME2 VARCHAR(50),INCOME VARCHAR(50)"
            strSql += vbCrLf + " ,RESULT INT,SEP INT,EXPACMAINCODE VARCHAR(50),INCACMAINCODE VARCHAR(50),EXPACGROUP VARCHAR(250),INCACGROUP VARCHAR(250)"
            strSql += vbCrLf + " ,EXPACID VARCHAR(50),EXPACNAME VARCHAR(250),EXPACGROUPNAME VARCHAR(250),EXPACMAINGRPNAME VARCHAR(250)"
            strSql += vbCrLf + " ,INCACID VARCHAR(50),INCACNAME VARCHAR(250),INCACGROUPNAME VARCHAR(250),INCACMAINGRPNAME VARCHAR(250),SNO INT IDENTITY(1,1),COLHEAD VARCHAR(2),COSTID VARCHAR(2)"
            strSql += vbCrLf + " )"
            strSql += vbCrLf + " "
            strSql += vbCrLf + " IF EXISTS (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & userId.ToString() & "PROFITLOSS4" & DtCostid.Rows(K).Item(0).ToString.Trim & "')"
            strSql += vbCrLf + " DROP TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS4" & DtCostid.Rows(K).Item(0).ToString.Trim & ""
            strSql += vbCrLf + " CREATE TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS4" & DtCostid.Rows(K).Item(0).ToString.Trim & ""
            strSql += vbCrLf + " (EXPACGROUPNAME1 VARCHAR(250),EXPENSES1 VARCHAR(50),EXPENSES2 VARCHAR(50),EXPENSES VARCHAR(50)"
            strSql += vbCrLf + " ,INCACGROUPNAME1 VARCHAR(250),INCOME1 VARCHAR(50),INCOME2 VARCHAR(50),INCOME VARCHAR(50)"
            strSql += vbCrLf + " ,RESULT INT,SEP INT,EXPACMAINCODE VARCHAR(50),INCACMAINCODE VARCHAR(50),EXPACGROUP VARCHAR(250),INCACGROUP VARCHAR(250)"
            strSql += vbCrLf + " ,EXPACID VARCHAR(50),EXPACNAME VARCHAR(250),EXPACGROUPNAME VARCHAR(250),EXPACMAINGRPNAME VARCHAR(250)"
            strSql += vbCrLf + " ,INCACID VARCHAR(50),INCACNAME VARCHAR(250),INCACGROUPNAME VARCHAR(250),INCACMAINGRPNAME VARCHAR(250),SNO INT IDENTITY(1,1),COLHEAD VARCHAR(2),COSTID VARCHAR(2)"
            strSql += vbCrLf + " )"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            If summary Then
                strSql = " INSERT INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS3" & DtCostid.Rows(K).Item(0).ToString.Trim & ""
                strSql += vbCrLf + " (EXPACGROUPNAME1,EXPENSES,INCACGROUPNAME1,INCOME,RESULT,SEP,EXPACMAINCODE,INCACMAINCODE,EXPACGROUP,INCACGROUP,EXPACID"
                strSql += vbCrLf + " ,EXPACNAME,EXPACGROUPNAME,EXPACMAINGRPNAME,INCACID,INCACNAME,INCACGROUPNAME,INCACMAINGRPNAME,COLHEAD,COSTID)"
                strSql += vbCrLf + " SELECT PARTICULARS EXPACGROUPNAME1,CONVERT(NUMERIC(15,2),AMOUNT) EXPENSES,NULL INCACGROUPNAME1"
                strSql += vbCrLf + " ,NULL INCOME,'3' RESULT,'2' SEP,'' EXPACMAINCODE,'' INCACMAINCODE,'' EXPACGROUP,'' INCACGROUP"
                strSql += vbCrLf + " ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,'' EXPACMAINGRPNAME"
                strSql += vbCrLf + " ,''INCACID,''INCACNAME,''INCACGROUPNAME,'' INCACMAINGRPNAME,'T1' COLHEAD,COSTID"
                strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS0 WHERE AMOUNT >= 0 AND ISNULL(COSTID,'')='" & DtCostid.Rows(K).Item(0).ToString.Trim & "'"
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT DISTINCT ACGRPNAME EXPACGROUPNAME1,CONVERT(NUMERIC(15,2),ISNULL(SUM(DEBIT),0) - ISNULL(SUM(CREDIT),0)) EXPENSES"
                strSql += vbCrLf + " ,NULL INCACGROUPNAME1,NULL INCOME,'3' RESULT,'2' SEP"
                strSql += vbCrLf + " ,ACMAINCODE EXPACMAINCODE,'' INCACMAINCODE,ACGRPCODE EXPACGROUP,'' INCACGROUP"
                strSql += vbCrLf + " ,ACID EXPACID,ACNAME EXPACNAME,ACGRPNAME EXPACGROUPNAME,ACMAINGRPNAME EXPACMAINGRPNAME"
                strSql += vbCrLf + " ,''INCACID,''INCACNAME,''INCACGROUPNAME,'' INCACMAINGRPNAME,'' COLHEAD,COSTID"
                strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS2 WHERE ISNULL(GRPTYPE,'') = 'E' AND ISNULL(COSTID,'')='" & DtCostid.Rows(K).Item(0).ToString.Trim & "'"
                strSql += vbCrLf + " GROUP BY ACMAINGRPNAME,ACGRPNAME,ACMAINCODE,ACGRPCODE,ACID,ACNAME,COSTID"
                strSql += vbCrLf + " HAVING(ISNULL(SUM(DEBIT), 0) - ISNULL(SUM(CREDIT), 0) > 0)"
                'CALNO 290414
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT DISTINCT ACGRPNAME EXPACGROUPNAME1,CONVERT(NUMERIC(15,2),ISNULL(SUM(DEBIT),0) - ISNULL(SUM(CREDIT),0)) EXPENSES"
                strSql += vbCrLf + " ,NULL INCACGROUPNAME1,NULL INCOME,'3' RESULT,'2' SEP"
                strSql += vbCrLf + " ,ACMAINCODE EXPACMAINCODE,'' INCACMAINCODE,ACGRPCODE EXPACGROUP,'' INCACGROUP"
                strSql += vbCrLf + " ,ACID EXPACID,ACNAME EXPACNAME,ACGRPNAME EXPACGROUPNAME,ACMAINGRPNAME EXPACMAINGRPNAME"
                strSql += vbCrLf + " ,''INCACID,''INCACNAME,''INCACGROUPNAME,'' INCACMAINGRPNAME,'' COLHEAD,COSTID"
                strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS2 WHERE ISNULL(GRPTYPE,'') = 'I' AND ISNULL(COSTID,'')='" & DtCostid.Rows(K).Item(0).ToString.Trim & "'"
                strSql += vbCrLf + " GROUP BY ACMAINGRPNAME,ACGRPNAME,ACMAINCODE,ACGRPCODE,ACID,ACNAME,COSTID"
                strSql += vbCrLf + " HAVING(ISNULL(SUM(DEBIT), 0) - ISNULL(SUM(CREDIT), 0) > 0)"
                'CALNO 290414
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
            Else
                strSql = " INSERT INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS3" & DtCostid.Rows(K).Item(0).ToString.Trim & ""
                strSql += vbCrLf + " (EXPACGROUPNAME1,EXPENSES,INCACGROUPNAME1,INCOME,RESULT,SEP,EXPACMAINCODE,INCACMAINCODE,EXPACGROUP,INCACGROUP,EXPACID"
                strSql += vbCrLf + " ,EXPACNAME,EXPACGROUPNAME,EXPACMAINGRPNAME,INCACID,INCACNAME,INCACGROUPNAME,INCACMAINGRPNAME,COLHEAD,COSTID)"
                strSql += vbCrLf + " SELECT PARTICULARS EXPACGROUPNAME1,CONVERT(VARCHAR,AMOUNT) EXPENSES,NULL INCACGROUPNAME1"
                strSql += vbCrLf + " ,NULL INCOME,'3' RESULT,'2' SEP,'' EXPACMAINCODE,'' INCACMAINCODE,'' EXPACGROUP,'' INCACGROUP"
                strSql += vbCrLf + " ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,'' EXPACMAINGRPNAME"
                strSql += vbCrLf + " ,''INCACID,''INCACNAME,''INCACGROUPNAME,'' INCACMAINGRPNAME,'T1' COLHEAD,COSTID"
                strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS0 WHERE AMOUNT >= 0 AND ISNULL(COSTID,'')='" & DtCostid.Rows(K).Item(0).ToString.Trim & "'"
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT DISTINCT ACGRPNAME EXPACGROUPNAME1,NULL EXPENSES,NULL INCACGROUPNAME1"
                strSql += vbCrLf + " ,NULL INCOME,'3' RESULT,'2' SEP,ACMAINCODE EXPACMAINCODE,'' INCACMAINCODE,ACGRPCODE EXPACGROUP,'' INCACGROUP"
                strSql += vbCrLf + " ,'' EXPACID,'' EXPACNAME,ACGRPNAME EXPACGROUPNAME,ACMAINGRPNAME EXPACMAINGRPNAME"
                strSql += vbCrLf + " ,''INCACID,''INCACNAME,''INCACGROUPNAME,'' INCACMAINGRPNAME,'T1' COLHEAD,COSTID"
                strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS2 WHERE ISNULL(GRPTYPE,'') = 'E' AND ISNULL(COSTID,'')='" & DtCostid.Rows(K).Item(0).ToString.Trim & "'"
                strSql += vbCrLf + " GROUP BY ACMAINGRPNAME,ACGRPNAME,ACMAINCODE,ACGRPCODE,COSTID"
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT DISTINCT '  '+ACNAME EXPACGROUPNAME1"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),ISNULL(SUM(DEBIT),0) - ISNULL(SUM(CREDIT),0)) EXPENSES"
                strSql += vbCrLf + " ,NULL INCACGROUPNAME1,NULL INCOME,'4' RESULT,'2' SEP"
                strSql += vbCrLf + " ,ACMAINCODE EXPACMAINCODE,'' INCACMAINCODE,ACGRPCODE EXPACGROUP,'' INCACGROUP"
                strSql += vbCrLf + " ,ACID EXPACID,ACNAME EXPACNAME,ACGRPNAME EXPACGROUPNAME,ACMAINGRPNAME EXPACMAINGRPNAME"
                strSql += vbCrLf + " ,''INCACID,''INCACNAME,''INCACGROUPNAME,'' INCACMAINGRPNAME,'' COLHEAD,COSTID"
                strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS2 WHERE ISNULL(GRPTYPE,'') = 'E' AND ISNULL(COSTID,'')='" & DtCostid.Rows(K).Item(0).ToString.Trim & "'"
                strSql += vbCrLf + " GROUP BY ACMAINGRPNAME,ACGRPNAME,ACMAINCODE,ACGRPCODE,ACID,ACNAME,COSTID"
                strSql += vbCrLf + " HAVING(ISNULL(SUM(DEBIT), 0) - ISNULL(SUM(CREDIT), 0) > 0)"
                'CALNO 290414
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT DISTINCT ACGRPNAME EXPACGROUPNAME1,NULL EXPENSES,NULL INCACGROUPNAME1"
                strSql += vbCrLf + " ,NULL INCOME,'3' RESULT,'2' SEP,ACMAINCODE EXPACMAINCODE,'' INCACMAINCODE,ACGRPCODE EXPACGROUP,'' INCACGROUP"
                strSql += vbCrLf + " ,'' EXPACID,'' EXPACNAME,ACGRPNAME EXPACGROUPNAME,ACMAINGRPNAME EXPACMAINGRPNAME"
                strSql += vbCrLf + " ,''INCACID,''INCACNAME,''INCACGROUPNAME,'' INCACMAINGRPNAME,'T1' COLHEAD,COSTID"
                strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS2 WHERE ISNULL(GRPTYPE,'') = 'I' AND ISNULL(COSTID,'')='" & DtCostid.Rows(K).Item(0).ToString.Trim & "'"
                strSql += vbCrLf + " GROUP BY ACMAINGRPNAME,ACGRPNAME,ACMAINCODE,ACGRPCODE,COSTID"
                strSql += vbCrLf + " HAVING(ISNULL(SUM(DEBIT), 0) - ISNULL(SUM(CREDIT), 0) > 0)"
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT DISTINCT '  '+ACNAME EXPACGROUPNAME1"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),ISNULL(SUM(DEBIT),0) - ISNULL(SUM(CREDIT),0)) EXPENSES"
                strSql += vbCrLf + " ,NULL INCACGROUPNAME1,NULL INCOME,'4' RESULT,'2' SEP"
                strSql += vbCrLf + " ,ACMAINCODE EXPACMAINCODE,'' INCACMAINCODE,ACGRPCODE EXPACGROUP,'' INCACGROUP"
                strSql += vbCrLf + " ,ACID EXPACID,ACNAME EXPACNAME,ACGRPNAME EXPACGROUPNAME,ACMAINGRPNAME EXPACMAINGRPNAME"
                strSql += vbCrLf + " ,''INCACID,''INCACNAME,''INCACGROUPNAME,'' INCACMAINGRPNAME,'' COLHEAD,COSTID"
                strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS2 WHERE ISNULL(GRPTYPE,'') = 'I' AND ISNULL(COSTID,'')='" & DtCostid.Rows(K).Item(0).ToString.Trim & "'"
                strSql += vbCrLf + " GROUP BY ACMAINGRPNAME,ACGRPNAME,ACMAINCODE,ACGRPCODE,ACID,ACNAME,COSTID"
                strSql += vbCrLf + " HAVING(ISNULL(SUM(DEBIT), 0) - ISNULL(SUM(CREDIT), 0) > 0)"
                'CALNO 290414
                strSql += vbCrLf + " ORDER BY EXPACGROUP,SEP,RESULT,INCACNAME,COSTID"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
            End If

            If summary Then
                strSql = " INSERT INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS4" & DtCostid.Rows(K).Item(0).ToString.Trim & ""
                strSql += vbCrLf + " (EXPACGROUPNAME1,EXPENSES,INCACGROUPNAME1,INCOME,RESULT,SEP,EXPACMAINCODE,INCACMAINCODE,EXPACGROUP,INCACGROUP,EXPACID"
                strSql += vbCrLf + " ,EXPACNAME,EXPACGROUPNAME,EXPACMAINGRPNAME,INCACID,INCACNAME,INCACGROUPNAME,INCACMAINGRPNAME,COLHEAD,COSTID)"
                strSql += vbCrLf + " SELECT NULL EXPACGROUPNAME1,NULL EXPENSES,PARTICULARS INCACGROUPNAME1"
                strSql += vbCrLf + " ,ABS(AMOUNT) INCOME,'3' RESULT,'2' SEP"
                strSql += vbCrLf + " ,'' EXPACMAINCODE,'' INCACMAINCODE,'' EXPACGROUP,'' INCACGROUP"
                strSql += vbCrLf + " ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,'' EXPACMAINGRPNAME"
                strSql += vbCrLf + " ,''INCACID,''INCACNAME,''INCACGROUPNAME,'' INCACMAINGRPNAME,'T1' COLHEAD,COSTID"
                strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS0 WHERE AMOUNT < 0 AND ISNULL(COSTID,'')='" & DtCostid.Rows(K).Item(0).ToString.Trim & "'"
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT DISTINCT '' EXPACGROUPNAME1,NULL EXPENSES,ACGRPNAME INCACGROUPNAME1"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),ISNULL(SUM(CREDIT),0) - ISNULL(SUM(DEBIT),0)) INCOME,'3' RESULT,'2' SEP"
                strSql += vbCrLf + " ,'' EXPACMAINCODE,ACMAINCODE INCACMAINCODE,'' EXPACGROUP,ACGRPCODE INCACGROUP"
                strSql += vbCrLf + " ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,'' EXPACMAINGRPNAME"
                strSql += vbCrLf + " ,ACID INCACID,ACNAME INCACNAME,ACGRPNAME INCACGROUPNAME,ACMAINGRPNAME INCACMAINGRPNAME,'' COLHEAD,COSTID"
                strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS2 WHERE ISNULL(GRPTYPE,'') = 'I' AND ISNULL(COSTID,'')='" & DtCostid.Rows(K).Item(0).ToString.Trim & "'"
                strSql += vbCrLf + " GROUP BY ACMAINGRPNAME,ACGRPNAME,ACMAINCODE,ACGRPCODE,ACID,ACNAME,COSTID"
                strSql += vbCrLf + " HAVING(ISNULL(SUM(CREDIT), 0) - ISNULL(SUM(DEBIT), 0) > 0)"
                'CALNO 290414
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT DISTINCT '' EXPACGROUPNAME1,NULL EXPENSES,ACGRPNAME INCACGROUPNAME1"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),ISNULL(SUM(CREDIT),0) - ISNULL(SUM(DEBIT),0)) INCOME,'3' RESULT,'2' SEP"
                strSql += vbCrLf + " ,'' EXPACMAINCODE,ACMAINCODE INCACMAINCODE,'' EXPACGROUP,ACGRPCODE INCACGROUP"
                strSql += vbCrLf + " ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,'' EXPACMAINGRPNAME"
                strSql += vbCrLf + " ,ACID INCACID,ACNAME INCACNAME,ACGRPNAME INCACGROUPNAME,ACMAINGRPNAME INCACMAINGRPNAME,'' COLHEAD,COSTID"
                strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS2 WHERE ISNULL(GRPTYPE,'') = 'E' AND ISNULL(COSTID,'')='" & DtCostid.Rows(K).Item(0).ToString.Trim & "'"
                strSql += vbCrLf + " GROUP BY ACMAINGRPNAME,ACGRPNAME,ACMAINCODE,ACGRPCODE,ACID,ACNAME,COSTID"
                strSql += vbCrLf + " HAVING(ISNULL(SUM(CREDIT), 0) - ISNULL(SUM(DEBIT), 0) > 0)"
                'CALNO 290414
                strSql += vbCrLf + " ORDER BY INCACMAINGRPNAME,SEP,INCACGROUPNAME,RESULT,INCACNAME,COSTID"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
            Else
                strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS4" & DtCostid.Rows(K).Item(0).ToString.Trim & ""
                strSql += vbCrLf + " (EXPACGROUPNAME1,EXPENSES,INCACGROUPNAME1,INCOME,RESULT,SEP,EXPACMAINCODE,INCACMAINCODE,EXPACGROUP,INCACGROUP,EXPACID"
                strSql += vbCrLf + " ,EXPACNAME,EXPACGROUPNAME,EXPACMAINGRPNAME,INCACID,INCACNAME,INCACGROUPNAME,INCACMAINGRPNAME,COLHEAD,COSTID)"
                strSql += vbCrLf + " SELECT NULL EXPACGROUPNAME1,NULL EXPENSES,PARTICULARS INCACGROUPNAME1"
                strSql += vbCrLf + " ,ABS(AMOUNT) INCOME,'3' RESULT,'2' SEP,'' EXPACMAINCODE,'' INCACMAINCODE,'' EXPACGROUP,'' INCACGROUP"
                strSql += vbCrLf + " ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,'' EXPACMAINGRPNAME"
                strSql += vbCrLf + " ,''INCACID,''INCACNAME,''INCACGROUPNAME,'' INCACMAINGRPNAME,'T1' COLHEAD,COSTID"
                strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS0 WHERE AMOUNT < 0 AND ISNULL(COSTID,'')='" & DtCostid.Rows(K).Item(0).ToString.Trim & "'"
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT DISTINCT '' EXPACGROUPNAME1,NULL EXPENSES,ACGRPNAME INCACGROUPNAME1"
                strSql += vbCrLf + " ,NULL INCOME,'3' RESULT,'2' SEP,'' EXPACMAINCODE,ACMAINCODE INCACMAINCODE,'' EXPACGROUP,ACGRPCODE INCACGROUP"
                strSql += vbCrLf + " ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,'' EXPACMAINGRPNAME"
                strSql += vbCrLf + " ,'' INCACID,'' INCACNAME,ACGRPNAME INCACGROUPNAME,ACMAINGRPNAME INCACMAINGRPNAME,'T1' COLHEAD,COSTID"
                strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS2 WHERE ISNULL(GRPTYPE,'') = 'I' AND ISNULL(COSTID,'')='" & DtCostid.Rows(K).Item(0).ToString.Trim & "'"
                strSql += vbCrLf + " GROUP BY ACMAINGRPNAME,ACGRPNAME,ACMAINCODE,ACGRPCODE,COSTID"
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT DISTINCT '' EXPACGROUPNAME1,NULL EXPENSES,ACNAME INCACGROUPNAME1"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),ISNULL(SUM(CREDIT),0) - ISNULL(SUM(DEBIT),0)) INCOME,'3' RESULT,'2' SEP"
                strSql += vbCrLf + " ,'' EXPACMAINCODE,ACMAINCODE INCACMAINCODE,'' EXPACGROUP,ACGRPCODE INCACGROUP"
                strSql += vbCrLf + " ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,'' EXPACMAINGRPNAME"
                strSql += vbCrLf + " ,ACID INCACID,ACNAME INCACNAME,ACGRPNAME INCACGROUPNAME,ACMAINGRPNAME INCACMAINGRPNAME,'' COLHEAD,COSTID"
                strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS2 WHERE ISNULL(GRPTYPE,'') = 'I' AND ISNULL(COSTID,'')='" & DtCostid.Rows(K).Item(0).ToString.Trim & "'"
                strSql += vbCrLf + " GROUP BY ACMAINGRPNAME,ACGRPNAME,ACMAINCODE,ACGRPCODE,ACID,ACNAME,COSTID"
                strSql += vbCrLf + " HAVING(ISNULL(SUM(CREDIT), 0) - ISNULL(SUM(DEBIT), 0) > 0)"
                'CALNO 290414
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT DISTINCT '' EXPACGROUPNAME1,NULL EXPENSES,ACGRPNAME INCACGROUPNAME1"
                strSql += vbCrLf + " ,NULL INCOME,'3' RESULT,'2' SEP,'' EXPACMAINCODE,ACMAINCODE INCACMAINCODE,'' EXPACGROUP,ACGRPCODE INCACGROUP"
                strSql += vbCrLf + " ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,'' EXPACMAINGRPNAME"
                strSql += vbCrLf + " ,'' INCACID,'' INCACNAME,ACGRPNAME INCACGROUPNAME,ACMAINGRPNAME INCACMAINGRPNAME,'T1' COLHEAD,COSTID"
                strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS2 WHERE ISNULL(GRPTYPE,'') = 'E' AND ISNULL(COSTID,'')='" & DtCostid.Rows(K).Item(0).ToString.Trim & "'"
                strSql += vbCrLf + " GROUP BY ACMAINGRPNAME,ACGRPNAME,ACMAINCODE,ACGRPCODE,COSTID"
                strSql += vbCrLf + " HAVING(ISNULL(SUM(CREDIT), 0) - ISNULL(SUM(DEBIT), 0) > 0)"
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT DISTINCT '' EXPACGROUPNAME1,NULL EXPENSES,ACNAME INCACGROUPNAME1"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),ISNULL(SUM(CREDIT),0) - ISNULL(SUM(DEBIT),0)) INCOME,'3' RESULT,'2' SEP"
                strSql += vbCrLf + " ,'' EXPACMAINCODE,ACMAINCODE INCACMAINCODE,'' EXPACGROUP,ACGRPCODE INCACGROUP"
                strSql += vbCrLf + " ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,'' EXPACMAINGRPNAME"
                strSql += vbCrLf + " ,ACID INCACID,ACNAME INCACNAME,ACGRPNAME INCACGROUPNAME,ACMAINGRPNAME INCACMAINGRPNAME,'' COLHEAD,COSTID"
                strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS2 WHERE ISNULL(GRPTYPE,'') = 'E' AND ISNULL(COSTID,'')='" & DtCostid.Rows(K).Item(0).ToString.Trim & "'"
                strSql += vbCrLf + " GROUP BY ACMAINGRPNAME,ACGRPNAME,ACMAINCODE,ACGRPCODE,ACID,ACNAME,COSTID"
                strSql += vbCrLf + " HAVING(ISNULL(SUM(CREDIT), 0) - ISNULL(SUM(DEBIT), 0) > 0)"
                'CALNO 290414
                strSql += vbCrLf + " ORDER BY INCACMAINGRPNAME,SEP,INCACGROUPNAME,RESULT,INCACNAME,COSTID"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
            End If
        Next

        strSql = vbCrLf + " IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & userId.ToString() & "PROFITLOSS5')"
        strSql += vbCrLf + " DROP TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS5"
        strSql += vbCrLf + " SELECT "
        For J As Integer = 0 To DtCostid.Rows.Count - 1
            strSql += vbCrLf + " T3" & J.ToString & ".EXPACGROUPNAME1 AS EXPACGROUPNAME1" & DtCostid.Rows(J).Item(0).ToString.Trim & ",T3" & J.ToString & ".EXPENSES AS EXPENSES" & DtCostid.Rows(J).Item(0).ToString.Trim & ","
            strSql += vbCrLf + " T4" & J.ToString & ".INCACGROUPNAME1 AS INCACGROUPNAME1" & DtCostid.Rows(J).Item(0).ToString.Trim & ",T4" & J.ToString & ".INCOME AS INCOME" & DtCostid.Rows(J).Item(0).ToString.Trim & ","
            strSql += vbCrLf + " T3" & J.ToString & ".COLHEAD COLHEAD3" & DtCostid.Rows(J).Item(0).ToString.Trim & ",T4" & J.ToString & ".COLHEAD COLHEAD4" & DtCostid.Rows(J).Item(0).ToString.Trim & ","
            strSql += vbCrLf + " T3" & J.ToString & ".RESULT RESULT3" & DtCostid.Rows(J).Item(0).ToString.Trim & ",T4" & J.ToString & ".RESULT RESULT4" & DtCostid.Rows(J).Item(0).ToString.Trim & ","
        Next
        strSql += vbCrLf + " T30.SNO SNO3,T40.SNO SNO4,T30.SEP SEP3,T40.SEP SEP4 "
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS5"
        Dim joinTbl As String = ""
        Dim MaxCount As Integer
        Dim ChkCount As Integer
        For J As Integer = 0 To DtCostid.Rows.Count - 1
            If J = 0 Then
                MaxCount = Val(objGPack.GetSqlValue("SELECT  COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS3" & DtCostid.Rows(J).Item(0).ToString.Trim, , "", ))
                strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS3" & DtCostid.Rows(J).Item(0).ToString.Trim & " AS T3" & J.ToString & " "
                joinTbl = "T3" & J.ToString
                strSql += vbCrLf + " FULL JOIN TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS4" & DtCostid.Rows(J).Item(0).ToString.Trim & " AS T4" & J.ToString & " ON T4" & J.ToString & ".SNO = " & joinTbl & ".SNO"
                ChkCount = Val(objGPack.GetSqlValue("SELECT  COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS4" & DtCostid.Rows(J).Item(0).ToString.Trim, , "", ))
                If ChkCount > MaxCount Then MaxCount = ChkCount : joinTbl = "T4" & J.ToString
            Else
                strSql += vbCrLf + " FULL JOIN TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS3" & DtCostid.Rows(J).Item(0).ToString.Trim & " AS T3" & J.ToString & " ON T3" & J.ToString & ".SNO = " & joinTbl & ".SNO"
                ChkCount = Val(objGPack.GetSqlValue("SELECT  COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS3" & DtCostid.Rows(J).Item(0).ToString.Trim, , "", ))
                If ChkCount > MaxCount Then MaxCount = ChkCount : joinTbl = "T3" & J.ToString
                strSql += vbCrLf + " FULL JOIN TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS4" & DtCostid.Rows(J).Item(0).ToString.Trim & " AS T4" & J.ToString & " ON T4" & J.ToString & ".SNO = " & joinTbl & ".SNO"
                ChkCount = Val(objGPack.GetSqlValue("SELECT  COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS4" & DtCostid.Rows(J).Item(0).ToString.Trim, , "", ))
                If ChkCount > MaxCount Then MaxCount = ChkCount : joinTbl = "T4" & J.ToString
            End If
        Next
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + " SELECT DISTINCT "
        For J As Integer = 0 To DtCostid.Rows.Count - 1
            strSql += vbCrLf + " 'P & L - 01/" & CnStartMonth & "/" & (IIf(dtpDate.Value.[Date].Month > 3, dtpDate.Value.[Date].Year, dtpDate.Value.[Date].Year - 1)).ToString() & " - " & dtpDate.Text.Replace("-", "/") & "' EXPACGROUPNAME1" & DtCostid.Rows(J).Item(0).ToString.Trim & ","
            strSql += vbCrLf + " ' 'EXPENSES1" & DtCostid.Rows(J).Item(0).ToString.Trim & ",' ' EXPENSES2" & DtCostid.Rows(J).Item(0).ToString.Trim & ",' 'EXPENSES" & DtCostid.Rows(J).Item(0).ToString.Trim & " "
            strSql += vbCrLf + " ,' ' INCACGROUPNAME1" & DtCostid.Rows(J).Item(0).ToString.Trim & ",' 'INCOME1" & DtCostid.Rows(J).Item(0).ToString.Trim & ",' 'INCOME2" & DtCostid.Rows(J).Item(0).ToString.Trim & ",' 'INCOME" & DtCostid.Rows(J).Item(0).ToString.Trim & ","
            If J = DtCostid.Rows.Count - 1 Then
                strSql += vbCrLf + " 'T' COLHEAD3" & DtCostid.Rows(J).Item(0).ToString.Trim & ",'T' COLHEAD4" & DtCostid.Rows(J).Item(0).ToString.Trim & ""
            Else
                strSql += vbCrLf + " 'T' COLHEAD3" & DtCostid.Rows(J).Item(0).ToString.Trim & ",'T' COLHEAD4" & DtCostid.Rows(J).Item(0).ToString.Trim & ","
            End If
        Next
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS5"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT "
        For J As Integer = 0 To DtCostid.Rows.Count - 1
            strSql += vbCrLf + " EXPACGROUPNAME1" & DtCostid.Rows(J).Item(0).ToString.Trim & ",' 'EXPENSES1" & DtCostid.Rows(J).Item(0).ToString.Trim & ",' ' EXPENSES2" & DtCostid.Rows(J).Item(0).ToString.Trim & "," & cnStockDb & ".[DBO].FUNC_RUPEES_WITHCOMMA(CASE WHEN ISNULL(EXPENSES" & DtCostid.Rows(J).Item(0).ToString.Trim & ",'') = '' THEN '0' ELSE EXPENSES" & DtCostid.Rows(J).Item(0).ToString.Trim & " END,'D','Y') EXPENSES" & DtCostid.Rows(J).Item(0).ToString.Trim & " "
            strSql += vbCrLf + " ,INCACGROUPNAME1" & DtCostid.Rows(J).Item(0).ToString.Trim & ",' 'INCOME1" & DtCostid.Rows(J).Item(0).ToString.Trim & ",' 'INCOME2" & DtCostid.Rows(J).Item(0).ToString.Trim & "," & cnStockDb & ".[DBO].FUNC_RUPEES_WITHCOMMA(CASE WHEN ISNULL(INCOME" & DtCostid.Rows(J).Item(0).ToString.Trim & ",'') = '' THEN '0' ELSE INCOME" & DtCostid.Rows(J).Item(0).ToString.Trim & " END,'D','Y') INCOME" & DtCostid.Rows(J).Item(0).ToString.Trim & ","
            If J = DtCostid.Rows.Count - 1 Then
                strSql += vbCrLf + " COLHEAD3" & DtCostid.Rows(J).Item(0).ToString.Trim & ",COLHEAD4" & DtCostid.Rows(J).Item(0).ToString.Trim & ""
            Else
                strSql += vbCrLf + " COLHEAD3" & DtCostid.Rows(J).Item(0).ToString.Trim & ",COLHEAD4" & DtCostid.Rows(J).Item(0).ToString.Trim & ","
            End If
        Next
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS5"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT DISTINCT "
        For J As Integer = 0 To DtCostid.Rows.Count - 1
            strSql += vbCrLf + " '' EXPACGROUPNAME1" & DtCostid.Rows(J).Item(0).ToString.Trim & ",'' EXPENSES1" & DtCostid.Rows(J).Item(0).ToString.Trim & ",'' EXPENSES2" & DtCostid.Rows(J).Item(0).ToString.Trim & ",'-------------------' EXPENSES" & DtCostid.Rows(J).Item(0).ToString.Trim & ","
            strSql += vbCrLf + " '' INCACGROUPNAME1" & DtCostid.Rows(J).Item(0).ToString.Trim & ",'' INCOME1" & DtCostid.Rows(J).Item(0).ToString.Trim & ",'' INCOME2" & DtCostid.Rows(J).Item(0).ToString.Trim & ",'-------------------' INCOME" & DtCostid.Rows(J).Item(0).ToString.Trim & ","
            If J = DtCostid.Rows.Count - 1 Then
                strSql += vbCrLf + " 'G1' COLHEAD3" & DtCostid.Rows(J).Item(0).ToString.Trim & ",'G1' COLHEAD4" & DtCostid.Rows(J).Item(0).ToString.Trim & " "
            Else
                strSql += vbCrLf + " 'G1' COLHEAD3" & DtCostid.Rows(J).Item(0).ToString.Trim & ",'G1' COLHEAD4" & DtCostid.Rows(J).Item(0).ToString.Trim & ","
            End If
        Next
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS5"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT"
        For J As Integer = 0 To DtCostid.Rows.Count - 1
            strSql += vbCrLf + " '' EXPACGROUPNAME1" & DtCostid.Rows(J).Item(0).ToString.Trim & ",'' EXPENSES1" & DtCostid.Rows(J).Item(0).ToString.Trim & ",'' EXPENSES2" & DtCostid.Rows(J).Item(0).ToString.Trim & ",CONVERT(VARCHAR," & cnStockDb & ".[DBO].FUNC_RUPEES_WITHCOMMA(SUM(CONVERT(NUMERIC(15,2),EXPENSES" & DtCostid.Rows(J).Item(0).ToString.Trim & ")),'D','Y')) EXPENSES" & DtCostid.Rows(J).Item(0).ToString.Trim & ","
            strSql += vbCrLf + " '' INCACGROUPNAME1" & DtCostid.Rows(J).Item(0).ToString.Trim & " ,'' INCOME1" & DtCostid.Rows(J).Item(0).ToString.Trim & ",'' INCOME2" & DtCostid.Rows(J).Item(0).ToString.Trim & ",CONVERT(VARCHAR," & cnStockDb & ".[DBO].FUNC_RUPEES_WITHCOMMA(SUM(CONVERT(NUMERIC(15,2),INCOME" & DtCostid.Rows(J).Item(0).ToString.Trim & ")),'D','Y')) INCOME" & DtCostid.Rows(J).Item(0).ToString.Trim & ","
            If J = DtCostid.Rows.Count - 1 Then
                strSql += vbCrLf + " 'G1' COLHEAD3" & DtCostid.Rows(J).Item(0).ToString.Trim & ",'G1' COLHEAD4" & DtCostid.Rows(J).Item(0).ToString.Trim & ""
            Else
                strSql += vbCrLf + " 'G1' COLHEAD3" & DtCostid.Rows(J).Item(0).ToString.Trim & ",'G1' COLHEAD4" & DtCostid.Rows(J).Item(0).ToString.Trim & ","
            End If
        Next
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS5"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT DISTINCT"
        For J As Integer = 0 To DtCostid.Rows.Count - 1
            strSql += vbCrLf + "  '' EXPACGROUPNAME1" & DtCostid.Rows(J).Item(0).ToString.Trim & ",'' EXPENSES1" & DtCostid.Rows(J).Item(0).ToString.Trim & ",'' EXPENSES2" & DtCostid.Rows(J).Item(0).ToString.Trim & ",'-------------------' EXPENSES" & DtCostid.Rows(J).Item(0).ToString.Trim & ","
            strSql += vbCrLf + " '' INCACGROUPNAME1" & DtCostid.Rows(J).Item(0).ToString.Trim & ",'' INCOME1" & DtCostid.Rows(J).Item(0).ToString.Trim & ",'' INCOME2" & DtCostid.Rows(J).Item(0).ToString.Trim & ",'-------------------' INCOME" & DtCostid.Rows(J).Item(0).ToString.Trim & ","
            If J = DtCostid.Rows.Count - 1 Then
                strSql += vbCrLf + " 'G1' COLHEAD3" & DtCostid.Rows(J).Item(0).ToString.Trim & ",'G1' COLHEAD4" & DtCostid.Rows(J).Item(0).ToString.Trim & ""
            Else
                strSql += vbCrLf + " 'G1' COLHEAD3" & DtCostid.Rows(J).Item(0).ToString.Trim & ",'G1' COLHEAD4" & DtCostid.Rows(J).Item(0).ToString.Trim & ","
            End If
        Next
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS5"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT"
        For J As Integer = 0 To DtCostid.Rows.Count - 1
            strSql += vbCrLf + " CASE WHEN ISNULL(SUM(CONVERT(NUMERIC(15,2),EXPENSES" & DtCostid.Rows(J).Item(0).ToString.Trim & ")),0) - ISNULL(SUM(CONVERT(NUMERIC(15,2),INCOME" & DtCostid.Rows(J).Item(0).ToString.Trim & ")),0) > 0 THEN 'GROSS LOSS' END EXPACGROUPNAME1" & DtCostid.Rows(J).Item(0).ToString.Trim & ",'' EXPENSES1" & DtCostid.Rows(J).Item(0).ToString.Trim & ",'' EXPENSES2" & DtCostid.Rows(J).Item(0).ToString.Trim & ","
            strSql += vbCrLf + " CONVERT(VARCHAR,CASE WHEN ISNULL(SUM(CONVERT(NUMERIC(15,2),EXPENSES" & DtCostid.Rows(J).Item(0).ToString.Trim & ")),0) - ISNULL(SUM(CONVERT(NUMERIC(15,2),INCOME" & DtCostid.Rows(J).Item(0).ToString.Trim & ")),0) > 0 THEN " & cnStockDb & ".[DBO].FUNC_RUPEES_WITHCOMMA(ISNULL(SUM(CONVERT(NUMERIC(15,2),EXPENSES" & DtCostid.Rows(J).Item(0).ToString.Trim & ")),0) - ISNULL(SUM(CONVERT(NUMERIC(15,2),INCOME" & DtCostid.Rows(J).Item(0).ToString.Trim & ")),0),'D','Y') END) EXPENSES" & DtCostid.Rows(J).Item(0).ToString.Trim & ","
            strSql += vbCrLf + " CASE WHEN ISNULL(SUM(CONVERT(NUMERIC(15,2),INCOME" & DtCostid.Rows(J).Item(0).ToString.Trim & ")),0) - ISNULL(SUM(CONVERT(NUMERIC(15,2),EXPENSES" & DtCostid.Rows(J).Item(0).ToString.Trim & ")),0) > 0 THEN 'GROSS PROFIT' END INCACGROUPNAME1" & DtCostid.Rows(J).Item(0).ToString.Trim & ",'' INCOME1" & DtCostid.Rows(J).Item(0).ToString.Trim & ",'' INCOME2" & DtCostid.Rows(J).Item(0).ToString.Trim & ","
            strSql += vbCrLf + " CONVERT(VARCHAR,CASE WHEN ISNULL(SUM(CONVERT(NUMERIC(15,2),INCOME" & DtCostid.Rows(J).Item(0).ToString.Trim & ")),0) - ISNULL(SUM(CONVERT(NUMERIC(15,2),EXPENSES" & DtCostid.Rows(J).Item(0).ToString.Trim & ")),0) > 0 THEN " & cnStockDb & ".[DBO].FUNC_RUPEES_WITHCOMMA(ISNULL(SUM(CONVERT(NUMERIC(15,2),INCOME" & DtCostid.Rows(J).Item(0).ToString.Trim & ")),0) - ISNULL(SUM(CONVERT(NUMERIC(15,2),EXPENSES" & DtCostid.Rows(J).Item(0).ToString.Trim & ")),0),'D','Y') END) INCOME" & DtCostid.Rows(J).Item(0).ToString.Trim & ","
            If J = DtCostid.Rows.Count - 1 Then
                strSql += vbCrLf + " 'G' COLHEAD3" & DtCostid.Rows(J).Item(0).ToString.Trim & ",'G' COLHEAD4" & DtCostid.Rows(J).Item(0).ToString.Trim & ""
            Else
                strSql += vbCrLf + " 'G' COLHEAD3" & DtCostid.Rows(J).Item(0).ToString.Trim & ",'G' COLHEAD4" & DtCostid.Rows(J).Item(0).ToString.Trim & ","
            End If
        Next
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS5"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
    End Sub

    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView.Click
        If chkTrading.Checked = False And ChkPandL.Checked = False Then MsgBox("Select any one option.", MsgBoxStyle.Information) : chkTrading.Focus() : Exit Sub
        If MultiCostcentre = True Then
            Rempty = False
            gridViewHead.DataSource = Nothing
            gridView.DataSource = Nothing
            Dim Summary As Boolean = chkSummary.Checked
            strSql = " SELECT COMPANYID  FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & cmbCompany.Text & "' AND ISNULL(ACTIVE,'')<>'N' "
            Dim strCompId As String = BrighttechPack.GetSqlValue(cn, strSql, "COMPANYID", "")
            Dim strCostId As String = ""
            If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
                strCostId += " AND COSTID IN"
                strCostId += "(SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & GetQryString(chkCmbCostCentre.Text) & "))"

                strSql = "SELECT DISTINCT COSTID,COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & GetQryString(chkCmbCostCentre.Text) & ") ORDER BY COSTID"
                da = New OleDbDataAdapter(strSql, cn)
                DtCostid = New DataTable
                da.Fill(DtCostid)
            Else
                strSql = "SELECT DISTINCT COSTID,COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTID"
                da = New OleDbDataAdapter(strSql, cn)
                DtCostid = New DataTable
                da.Fill(DtCostid)
                If DtCostid.Rows.Count = 0 Then
                    strSql = "SELECT '' COSTID,'' COSTNAME "
                    da = New OleDbDataAdapter(strSql, cn)
                    DtCostid = New DataTable
                    da.Fill(DtCostid)
                End If
            End If
            funcViewMulCost(strCompanyId, strCostId, Summary)
            funcHeaderNew()
            With gridViewHead
                .ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
                .Height = gridView.ColumnHeadersHeight
                Dim colWid As Integer = 0
                For cnt As Integer = 0 To gridView.ColumnCount - 1
                    If gridView.Columns(cnt).Visible Then colWid += gridView.Columns(cnt).Width
                Next
                If Not Rempty Then
                    If colWid >= gridView.Width Then
                        .Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
                        .Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
                    Else
                        .Columns("SCROLL").Visible = False
                    End If
                End If
            End With
            'Calno 560 End
        Else
            gridView.DataSource = Nothing
            funcView()
            gridViewHead.Visible = False
        End If
        Prop_Sets()
    End Sub
    Public Function funcHeaderNew() As Integer
        Dim dtheader As New DataTable
        dtheader.Clear()
        Try
            Dim dtMergeHeader As New DataTable("~MERGEHEADER")
            With dtMergeHeader
                For i As Integer = 0 To DtCostid.Rows.Count - 1
                    .Columns.Add("EXPACGROUPNAME1" & DtCostid.Rows(i).Item(0).ToString.Trim & "~EXPENSES" & DtCostid.Rows(i).Item(0).ToString.Trim & "~EXPENSES1" & DtCostid.Rows(i).Item(0).ToString.Trim & "~EXPENSES2" & DtCostid.Rows(i).Item(0).ToString.Trim & "~INCACGROUPNAME1" & DtCostid.Rows(i).Item(0).ToString.Trim & "~INCOME" & DtCostid.Rows(i).Item(0).ToString.Trim & "~INCOME1" & DtCostid.Rows(i).Item(0).ToString.Trim & "~INCOME2" & DtCostid.Rows(i).Item(0).ToString.Trim & "", GetType(String))
                    .Columns.Add("COLHEAD3" & DtCostid.Rows(i).Item(0).ToString.Trim, GetType(String))
                    .Columns.Add("COLHEAD4" & DtCostid.Rows(i).Item(0).ToString.Trim, GetType(String))
                Next
                .Columns.Add("SCROLL", GetType(String))
                For i As Integer = 0 To DtCostid.Rows.Count - 1
                    .Columns("EXPACGROUPNAME1" & DtCostid.Rows(i).Item(0).ToString.Trim & "~EXPENSES" & DtCostid.Rows(i).Item(0).ToString.Trim & "~EXPENSES1" & DtCostid.Rows(i).Item(0).ToString.Trim & "~EXPENSES2" & DtCostid.Rows(i).Item(0).ToString.Trim & "~INCACGROUPNAME1" & DtCostid.Rows(i).Item(0).ToString.Trim & "~INCOME" & DtCostid.Rows(i).Item(0).ToString.Trim & "~INCOME1" & DtCostid.Rows(i).Item(0).ToString.Trim & "~INCOME2" & DtCostid.Rows(i).Item(0).ToString.Trim & "").Caption = DtCostid.Rows(i).Item(1).ToString
                    .Columns("COLHEAD3" & DtCostid.Rows(i).Item(0).ToString.Trim).Caption = ""
                    .Columns("COLHEAD4" & DtCostid.Rows(i).Item(0).ToString.Trim).Caption = ""
                Next
                .Columns("SCROLL").Caption = ""
            End With
            gridViewHead.DataSource = dtMergeHeader

            funcGridHeaderStyle()
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function
    Public Function funcGridHeaderStyle() As Integer
        With gridViewHead
            For i As Integer = 0 To DtCostid.Rows.Count - 1
                With .Columns("EXPACGROUPNAME1" & DtCostid.Rows(i).Item(0).ToString.Trim & "~EXPENSES" & DtCostid.Rows(i).Item(0).ToString.Trim & "~EXPENSES1" & DtCostid.Rows(i).Item(0).ToString.Trim & "~EXPENSES2" & DtCostid.Rows(i).Item(0).ToString.Trim & "~INCACGROUPNAME1" & DtCostid.Rows(i).Item(0).ToString.Trim & "~INCOME" & DtCostid.Rows(i).Item(0).ToString.Trim & "~INCOME1" & DtCostid.Rows(i).Item(0).ToString.Trim & "~INCOME2" & DtCostid.Rows(i).Item(0).ToString.Trim)
                    .Width = gridView.Columns("EXPACGROUPNAME1" & DtCostid.Rows(i).Item(0).ToString.Trim).Width + gridView.Columns("EXPENSES" & DtCostid.Rows(i).Item(0).ToString.Trim).Width _
                    + IIf(gridView.Columns("EXPENSES1" & DtCostid.Rows(i).Item(0).ToString.Trim).Visible, gridView.Columns("EXPENSES1" & DtCostid.Rows(i).Item(0).ToString.Trim).Width, 0) _
                    + IIf(gridView.Columns("EXPENSES2" & DtCostid.Rows(i).Item(0).ToString.Trim).Visible, gridView.Columns("EXPENSES2" & DtCostid.Rows(i).Item(0).ToString.Trim).Width, 0) _
                    + gridView.Columns("INCACGROUPNAME1" & DtCostid.Rows(i).Item(0).ToString.Trim).Width + gridView.Columns("INCOME" & DtCostid.Rows(i).Item(0).ToString.Trim).Width _
                    + IIf(gridView.Columns("INCOME1" & DtCostid.Rows(i).Item(0).ToString.Trim).Visible, gridView.Columns("INCOME1" & DtCostid.Rows(i).Item(0).ToString.Trim).Width, 0) _
                    + IIf(gridView.Columns("INCOME2" & DtCostid.Rows(i).Item(0).ToString.Trim).Visible, gridView.Columns("INCOME2" & DtCostid.Rows(i).Item(0).ToString.Trim).Width, 0)
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .HeaderText = DtCostid.Rows(i).Item(1).ToString
                End With
                With .Columns("COLHEAD3" & DtCostid.Rows(i).Item(0).ToString.Trim)
                    .Width = gridView.Columns("COLHEAD3" & DtCostid.Rows(i).Item(0).ToString.Trim).Width
                    .Visible = gridView.Columns("COLHEAD3" & DtCostid.Rows(i).Item(0).ToString.Trim).Visible
                    .HeaderText = ""
                End With
                With .Columns("COLHEAD4" & DtCostid.Rows(i).Item(0).ToString.Trim)
                    .Width = gridView.Columns("COLHEAD4" & DtCostid.Rows(i).Item(0).ToString.Trim).Width
                    .Visible = gridView.Columns("COLHEAD4" & DtCostid.Rows(i).Item(0).ToString.Trim).Visible
                    .HeaderText = ""
                End With
            Next
            With .Columns("SCROLL")
                .Width = 20
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .HeaderText = ""
            End With
            gridViewHead.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 9, FontStyle.Bold)
            gridViewHead.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        End With
    End Function
    Private Sub funcView()
        Dim Summary As Boolean = chkSummary.Checked
        strSql = " SELECT COMPANYID  FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & cmbCompany.Text & "' AND ISNULL(ACTIVE,'')<>'N' "
        Dim strCompId As String = BrighttechPack.GetSqlValue(cn, strSql, "COMPANYID", "")
        Dim strCostId As String = ""
        If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
            strCostId += " AND COSTID IN"
            strCostId += "(SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & GetQryString(chkCmbCostCentre.Text) & "))"
        End If
        dt = New DataTable()

        If chkAsOn.Checked Then
            If chkTrading.Checked Then funcTrading(strCompId, strCostId, Summary)
            If ChkPandL.Checked Then funcPL(strCompId, strCostId, Summary)
        Else
            If chkTrading.Checked Then funcTradingBetweenDates(strCompId, strCostId, Summary)
            If ChkPandL.Checked Then funcPLBetweenDates(strCompId, strCostId, Summary)
        End If


        gridView.DataSource = dt

        If gridView.RowCount > 0 Then
            gridView.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 9, FontStyle.Bold)
            gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            gridView.Font = New Font("VERDANA", 7, FontStyle.Regular)
            gridView.Columns("EXPACGROUPNAME1").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            gridView.Columns("EXPACGROUPNAME1").HeaderText = "PARTICULARS"
            gridView.Columns("EXPACGROUPNAME1").DefaultCellStyle.Font = New Font("VERDANA", 7, FontStyle.Regular)
            gridView.Columns("EXPACGROUPNAME1").Width = 202
            gridView.Columns("EXPENSES").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            gridView.Columns("EXPENSES").HeaderText = ""
            gridView.Columns("EXPENSES").Width = 100
            gridView.Columns("EXPENSES1").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            gridView.Columns("EXPENSES1").HeaderText = ""
            gridView.Columns("EXPENSES1").Width = 100
            gridView.Columns("EXPENSES2").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            gridView.Columns("EXPENSES2").HeaderText = "EXPENSES"
            gridView.Columns("EXPENSES2").Width = 100
            gridView.Columns("INCACGROUPNAME1").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            gridView.Columns("INCACGROUPNAME1").HeaderText = "PARTICULARS"
            gridView.Columns("INCACGROUPNAME1").Width = 160
            gridView.Columns("EXPACGROUPNAME1").DefaultCellStyle.Font = New Font("VERDANA", 7, FontStyle.Regular)
            gridView.Columns("INCOME").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            gridView.Columns("INCOME").Width = 100
            gridView.Columns("INCOME").HeaderText = ""
            gridView.Columns("INCOME1").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            gridView.Columns("INCOME1").Width = 100
            gridView.Columns("INCOME1").HeaderText = ""
            gridView.Columns("INCOME2").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            gridView.Columns("INCOME2").Width = 100
            gridView.Columns("INCOME2").HeaderText = "INCOME"
            gridView.Columns("COLHEAD3").Visible = False
            gridView.Columns("COLHEAD4").Visible = False

            For Each col As DataGridViewColumn In gridView.Columns
                col.SortMode = DataGridViewColumnSortMode.NotSortable
            Next

            For i As Integer = 0 To gridView.RowCount - 1
                If gridView.Rows(i).Cells("COLHEAD3").Value.ToString() = "T" Then
                    gridView.Rows(i).Cells("EXPACGROUPNAME1").Style.BackColor = Color.LightBlue
                    gridView.Rows(i).Cells("EXPACGROUPNAME1").Style.ForeColor = Color.Blue
                    gridView.Rows(i).Cells("EXPACGROUPNAME1").Style.Font = New Font("VERDANA", 7, FontStyle.Bold)
                ElseIf gridView.Rows(i).Cells("COLHEAD3").Value.ToString() = "T1" OrElse gridView.Rows(i).Cells("COLHEAD3").Value.ToString() = "T2" Then
                    gridView.Rows(i).Cells("EXPACGROUPNAME1").Style.Font = New Font("VERDANA", 7, FontStyle.Bold)
                    gridView.Rows(i).Cells("EXPENSES").Style.Font = New Font("VERDANA", 7, FontStyle.Bold)
                ElseIf gridView.Rows(i).Cells("COLHEAD3").Value.ToString() = "S" OrElse gridView.Rows(i).Cells("COLHEAD3").Value.ToString() = "S1" OrElse gridView.Rows(i).Cells("COLHEAD3").Value.ToString() = "S2" Then
                    gridView.Rows(i).Cells("EXPENSES").Style.Font = New Font("VERDANA", 7, FontStyle.Bold)
                    gridView.Rows(i).Cells("EXPENSES1").Style.Font = New Font("VERDANA", 7, FontStyle.Bold)
                    gridView.Rows(i).Cells("EXPENSES2").Style.Font = New Font("VERDANA", 7, FontStyle.Bold)
                ElseIf gridView.Rows(i).Cells("COLHEAD3").Value.ToString() = "G" Then
                    If Convert.ToDouble((IIf(gridView.Rows(i).Cells("EXPENSES").Value.ToString() <> "", gridView.Rows(i).Cells("EXPENSES").Value.ToString(), "0"))) <> 0 Then
                        gridView.Rows(i).Cells("EXPACGROUPNAME1").Style.BackColor = Color.LavenderBlush
                        gridView.Rows(i).Cells("EXPACGROUPNAME1").Style.Font = New Font("VERDANA", 7, FontStyle.Bold)
                        gridView.Rows(i).Cells("EXPENSES").Style.BackColor = Color.LavenderBlush
                        gridView.Rows(i).Cells("EXPENSES").Style.Font = New Font("VERDANA", 7, FontStyle.Bold)
                        gridView.Rows(i).Cells("EXPENSES1").Style.Font = New Font("VERDANA", 7, FontStyle.Bold)
                        gridView.Rows(i).Cells("EXPENSES2").Style.Font = New Font("VERDANA", 7, FontStyle.Bold)
                    End If
                ElseIf gridView.Rows(i).Cells("COLHEAD3").Value.ToString() = "G1" OrElse gridView.Rows(i).Cells("COLHEAD3").Value.ToString() = "G2" Then
                    gridView.Rows(i).Cells("EXPENSES").Style.Font = New Font("VERDANA", 7, FontStyle.Bold)
                    gridView.Rows(i).Cells("EXPENSES1").Style.Font = New Font("VERDANA", 7, FontStyle.Bold)
                    gridView.Rows(i).Cells("EXPENSES2").Style.Font = New Font("VERDANA", 7, FontStyle.Bold)
                End If


                If gridView.Rows(i).Cells("COLHEAD4").Value.ToString() = "T" Then
                    'gridView.Rows[i].Cells["INCACGROUPNAME1"].Style.BackColor = Color.LightBlue;
                    'gridView.Rows[i].Cells["INCACGROUPNAME1"].Style.ForeColor = Color.Blue;
                    gridView.Rows(i).Cells("INCACGROUPNAME1").Style.Font = New Font("VERDANA", 7, FontStyle.Bold)
                ElseIf gridView.Rows(i).Cells("COLHEAD4").Value.ToString() = "T1" OrElse gridView.Rows(i).Cells("COLHEAD4").Value.ToString() = "T2" Then
                    gridView.Rows(i).Cells("INCACGROUPNAME1").Style.Font = New Font("VERDANA", 7, FontStyle.Bold)
                    gridView.Rows(i).Cells("INCOME").Style.Font = New Font("VERDANA", 7, FontStyle.Bold)
                ElseIf gridView.Rows(i).Cells("COLHEAD4").Value.ToString() = "S" OrElse gridView.Rows(i).Cells("COLHEAD4").Value.ToString() = "S1" OrElse gridView.Rows(i).Cells("COLHEAD4").Value.ToString() = "S2" Then
                    gridView.Rows(i).Cells("INCOME").Style.Font = New Font("VERDANA", 7, FontStyle.Bold)
                    gridView.Rows(i).Cells("INCOME1").Style.Font = New Font("VERDANA", 7, FontStyle.Bold)
                    gridView.Rows(i).Cells("INCOME2").Style.Font = New Font("VERDANA", 7, FontStyle.Bold)
                ElseIf gridView.Rows(i).Cells("COLHEAD4").Value.ToString() = "G" Then
                    If Convert.ToDouble((IIf(gridView.Rows(i).Cells("INCOME").Value.ToString() <> "", gridView.Rows(i).Cells("INCOME").Value.ToString(), "0"))) <> 0 Then
                        gridView.Rows(i).Cells("INCACGROUPNAME1").Style.BackColor = Color.LavenderBlush
                        gridView.Rows(i).Cells("INCACGROUPNAME1").Style.Font = New Font("VERDANA", 7, FontStyle.Bold)
                        gridView.Rows(i).Cells("INCOME").Style.BackColor = Color.LavenderBlush
                        gridView.Rows(i).Cells("INCOME").Style.Font = New Font("VERDANA", 7, FontStyle.Bold)
                        gridView.Rows(i).Cells("INCOME1").Style.Font = New Font("VERDANA", 7, FontStyle.Bold)
                        gridView.Rows(i).Cells("INCOME2").Style.Font = New Font("VERDANA", 7, FontStyle.Bold)
                    End If
                ElseIf gridView.Rows(i).Cells("COLHEAD4").Value.ToString() = "G1" OrElse gridView.Rows(i).Cells("COLHEAD4").Value.ToString() = "G2" Then
                    gridView.Rows(i).Cells("INCOME").Style.Font = New Font("VERDANA", 7, FontStyle.Bold)
                    gridView.Rows(i).Cells("INCOME1").Style.Font = New Font("VERDANA", 7, FontStyle.Bold)
                    gridView.Rows(i).Cells("INCOME2").Style.Font = New Font("VERDANA", 7, FontStyle.Bold)
                End If
            Next
            lblTitle.Text = cmbCompany.Text & vbCrLf & "TRADING & PROFIT AND LOSS FOR THE PERIOD OF - 01/" & CnStartMonth & "/" & (IIf(dtpDate.Value.Date.Month > 3, dtpDate.Value.Date.Year, dtpDate.Value.Date.Year - 1)).ToString() & " - " & dtpDate.Text.Replace("-", "/")
            If chkCmbCostCentre.Text <> "" Then lblTitle.Text = lblTitle.Text & vbCrLf & " COST CENTRE :" & chkCmbCostCentre.Text
            gridView.Focus()
        Else
            MessageBox.Show("Message", "Records not found...", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            dtpDate.Focus()
        End If

    End Sub
    Private Sub funcTrading(ByVal strcompid As String, ByVal strCostId As String, ByVal summary As Boolean, Optional ByVal ForPL As Boolean = False)
        Dim cnttchkk As Integer = 0
        Try
TTTT:
            strSql = "  IF EXISTS (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & userId.ToString() & "TRADE1')"
            strSql += vbCrLf + "  DROP TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE1"
            strSql += vbCrLf + "  CREATE TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE1"
            strSql += vbCrLf + "  (COMPANYID VARCHAR(3),ACID VARCHAR(25),TRANMODE VARCHAR(1),AMOUNT NUMERIC(15,2),DEBIT NUMERIC(15,2),CREDIT NUMERIC(15,2)"
            strSql += vbCrLf + "  ,TRANDATE SMALLDATETIME,PAYMODE VARCHAR(20),TRANNO VARCHAR(25),SEP VARCHAR(1))"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            strSql += vbCrLf + " (COMPANYID,COSTID,CATCODE,CLOSING,RATE,VALUE,UPDATED,UPTIME)"
            strSql = "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE1"
            strSql += vbCrLf + "  (COMPANYID,ACID,TRANMODE,AMOUNT,DEBIT,CREDIT,TRANDATE,PAYMODE,TRANNO,SEP)"
            strSql += vbCrLf + "  SELECT "
            strSql += vbCrLf + "   COMPANYID,ACCODE ACID,CASE WHEN ISNULL(DEBIT,0) > ISNULL(CREDIT,0) THEN 'D' ELSE 'C' END TRANMODE"
            strSql += vbCrLf + "  ,CASE WHEN ISNULL(DEBIT,0) <> 0 THEN DEBIT ELSE CREDIT END AMOUNT"
            strSql += vbCrLf + "  ,DEBIT,CREDIT,NULL TRANDATE,'' PAYMODE,NULL TRANNO,'O' SEP"
            strSql += vbCrLf + "  FROM " & cnStockDb & "..OPENTRAILBALANCE WHERE ISNULL(COMPANYID,'') = '" & strcompid & "'"
            strSql += strCostId
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT "
            strSql += vbCrLf + "   COMPANYID,ACCODE ACID,TRANMODE,AMOUNT"
            strSql += vbCrLf + "  ,CASE WHEN TRANMODE = 'D' THEN AMOUNT END DEBIT,CASE WHEN TRANMODE = 'C' THEN AMOUNT END CREDIT"
            strSql += vbCrLf + "  ,TRANDATE,PAYMODE,TRANNO,'T' SEP"
            strSql += vbCrLf + "  FROM " & cnStockDb & "..ACCTRAN WHERE TRANDATE >= '" & cnTranFromDate & "' "
            strSql += vbCrLf + "  AND TRANDATE <= '" & dtpDate.Value.Date.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') != 'Y' "
            strSql += vbCrLf + "  AND ISNULL(COMPANYID,'') = '" & strcompid & "' "
            strSql += strCostId
            'strSql +=VBCRLF + " UNION ALL"
            'strSql +=VBCRLF + " (COMPANYID,COSTID,CATCODE,CLOSING,RATE,VALUE,UPDATED,UPTIME)"
            'strSql +=VBCRLF + " SELECT"
            'strSql +=VBCRLF + "     COMPANYID,'CL"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            Dim StrNew As String
            Dim Startdate As Date
            StrNew = "SELECT STARTDATE FROM " & cnAdminDb & "..DBMASTER WHERE '" & Format(dtpDate.Value, "yyyy-MM-dd") & "' BETWEEN STARTDATE AND ENDDATE "
            Startdate = GetSqlValue(cn, StrNew)

            strSql = "  IF EXISTS (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & userId.ToString() & "TRADE2')"
            strSql += vbCrLf + "  DROP TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2"
            If summary Then
                strSql += vbCrLf + "  SELECT "
                strSql += vbCrLf + "   '' ACID,G.ACGRPCODE,SG.ACGRPNAME ACGRPNAME,G.ACMAINCODE,SG.GRPLEDGER,SG.GRPTYPE"
                'strSql +=VBCRLF + "   A.ACCODE ACID,G.ACGRPCODE,ACGRPNAME ACGRPNAME,ACMAINCODE,G.GRPLEDGER,G.GRPTYPE"
                strSql += vbCrLf + "  ,SUM(DEBIT) DEBIT,SUM(CREDIT) CREDIT"
                strSql += vbCrLf + "  ,'' ACNAME,'' TRANDATE,CASE WHEN SUM(AMOUNT) > 0 THEN 'D' ELSE 'C' END TRANMODE,SUM(AMOUNT) AMOUNT,'' PAYMODE,'' TRANNO"
                strSql += vbCrLf + "  ,(SELECT ACGRPNAME FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPCODE = G.ACMAINCODE) ACMAINGRPNAME"
                strSql += vbCrLf + "  INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2"
                strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE1 AS T INNER JOIN " & cnAdminDb & "..ACHEAD AS A"
                strSql += vbCrLf + "  ON T.ACID = A.ACCODE INNER JOIN " & cnAdminDb & "..ACGROUP AS G ON G.ACGRPCODE = A.ACGRPCODE"
                strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ACGROUP AS SG ON SG.ACGRPCODE = A.ACGRPCODE"
                strSql += vbCrLf + "  AND (SG.GRPLEDGER IN ('T') OR G.GRPLEDGER IN ('T')) "
                strSql += vbCrLf + "  GROUP BY G.ACGRPCODE,SG.ACGRPNAME,G.ACMAINCODE,SG.GRPLEDGER,SG.GRPTYPE"
                If rbtCls_Stock_Man.Checked Then  'Val(objGPack.GetSqlValue("SELECT COUNT(*) FROM " & cnStockDb & "..CLOSINGVAL WHERE 1=1 " & strCostId, , , )) > 0 Then
                    strSql += vbCrLf + " UNION ALL"
                    strSql += vbCrLf + " SELECT 'CLSTK' ACID,'0' ACGRPCODE,'CLOSING STOCK' ACGRPNAME,'0' ACMAINCODE"
                    strSql += vbCrLf + " ,'T' GRPLEDGER,CASE WHEN SUM(VALUE) > 0 THEN 'I' ELSE 'E' END GRPTYPE"
                    strSql += vbCrLf + " ,CASE WHEN SUM(VALUE) < 0 THEN SUM(VALUE)*(-1) END DEBIT"
                    strSql += vbCrLf + " ,CASE WHEN SUM(VALUE) > 0 THEN SUM(VALUE) END CREDIT"
                    strSql += vbCrLf + " ,'CLOSING STOCK' ACNAME,'' TRANDATE"
                    strSql += vbCrLf + " ,CASE WHEN SUM(VALUE) > 0 THEN 'D' ELSE 'C' END TRANMODE,SUM(VALUE) AMOUNT"
                    strSql += vbCrLf + " ,'' PAYMODE,'' TRANNO,'CLOSING STOCK' ACMAINGRPNAME"
                    strSql += vbCrLf + " FROM " & cnStockDb & "..CLOSINGVAL WHERE ISNULL(COMPANYID,'') = '" & strcompid & "'"
                    strSql += strCostId
                    strSql += vbCrLf + " HAVING SUM(VALUE) <> 0"
                    'strSql += vbCrLf + " UNION ALL"
                    'strSql += vbCrLf + " SELECT 'OPSTK' ACID,'0' ACGRPCODE,'OPENING STOCK' ACGRPNAME,'0' ACMAINCODE"
                    'strSql += vbCrLf + " ,'T' GRPLEDGER"
                    'strSql += vbCrLf + " ,CASE WHEN SUM(AMOUNT) < 0 THEN 'I' ELSE 'E' END GRPTYPE"
                    'strSql += vbCrLf + " ,CASE WHEN SUM(AMOUNT) > 0 THEN SUM(AMOUNT) END CREDIT"
                    'strSql += vbCrLf + " ,CASE WHEN SUM(AMOUNT) < 0 THEN SUM(AMOUNT)*(-1) END DEBIT"
                    'strSql += vbCrLf + " ,'OPENING STOCK' ACNAME,'' TRANDATE"
                    'strSql += vbCrLf + " ,CASE WHEN SUM(AMOUNT) > 0 THEN 'D' ELSE 'C' END TRANMODE"
                    'strSql += vbCrLf + " ,SUM(AMOUNT) AMOUNT"
                    'strSql += vbCrLf + " ,'' PAYMODE,'' TRANNO,'OPENING STOCK' ACMAINGRPNAME"
                    'strSql += vbCrLf + " FROM " & cnStockDb & "..OPENWEIGHT WHERE ISNULL(COMPANYID,'') = '" & strcompid & "'"
                    'strSql += strCostId
                    'strSql += vbCrLf + " HAVING SUM(AMOUNT) <> 0"
                Else
                    Dim Qry As String

                    ProgressBarShow()
                    If rbtCls_Stock_AvgRate.Checked Then
                        ProgressBarStep("Closing Stock arrival Weighted avg method", 10)
                        Qry = " EXEC " & cnAdminDb & "..SP_CLOSING_STOCK"
                    Else
                        ProgressBarStep("Closing Stock arrival LIFO method", 10)
                        Qry = " EXEC " & cnAdminDb & "..SP_CLOSINGSTOCK_LIFO"
                    End If
                    Qry += vbCrLf + " @DBNAME = '" & cnStockDb & "'"
                    Qry += vbCrLf + " ,@ADMINDB = '" & cnAdminDb & "'"
                    Qry += vbCrLf + " ,@FRMDATE = '" & Format(Startdate, "yyyy-MM-dd") & "'"
                    Qry += vbCrLf + " ,@TODATE = '" & Format(dtpDate.Value, "yyyy-MM-dd") & "'"
                    Qry += vbCrLf + " ,@METALNAME = 'ALL'"
                    Qry += vbCrLf + " ,@CATCODE = 'ALL'"
                    Qry += vbCrLf + " ,@CATNAME = 'ALL'"
                    Qry += vbCrLf + " ,@COSTNAME = '" & GetQryString(chkCmbCostCentre.Text).Replace("'", "") & "'"
                    Qry += vbCrLf + " ,@COMPANYID = '" & strcompid & "'"
                    Qry += vbCrLf + " ,@RPTTYPE = 'S'"
                    Dim SYSTEMID As String = Trim(LSet(Mid(Guid.NewGuid.ToString, 1, InStr(Guid.NewGuid.ToString, "-") - 1), 10))
                    Qry += vbCrLf + " ,@SYSTEMID = '" & SYSTEMID & "'"
                    cmd = New OleDbCommand(Qry, cn)
                    cmd.CommandTimeout = 2000
                    cmd.ExecuteNonQuery()

                    strSql += vbCrLf + " UNION ALL"
                    strSql += vbCrLf + " SELECT 	'CLSTK' ACID,'0' ACGRPCODE,'CLOSING STOCK' ACGRPNAME,'0' ACMAINCODE ,'T' GRPLEDGER,"
                    strSql += vbCrLf + " CASE WHEN SUM(CAMOUNT) > 0 THEN 'I' ELSE 'E' END GRPTYPE "
                    strSql += vbCrLf + " ,CASE WHEN SUM(CAMOUNT) < 0 THEN SUM(CAMOUNT)*(-1) END DEBIT,"
                    strSql += vbCrLf + " CASE WHEN SUM(CAMOUNT) > 0 THEN SUM(CAMOUNT) END CREDIT "
                    strSql += vbCrLf + " ,'CLOSING STOCK' ACNAME,'' TRANDATE "
                    strSql += vbCrLf + " ,CASE WHEN SUM(CAMOUNT) > 0 THEN 'D' ELSE 'C' END TRANMODE"
                    strSql += vbCrLf + " ,SUM(CAMOUNT) AMOUNT ,'' PAYMODE,'' TRANNO,'CLOSING STOCK' ACMAINGRPNAME "
                    strSql += vbCrLf + " FROM TEMPTABLEDB..TEMPCSTKVALUE4" & SYSTEMID & " A "
                    strSql += vbCrLf + " WHERE A.RESULT=1"
                    strSql += vbCrLf + " HAVING SUM(CAMOUNT) <> 0"
                End If
            Else
                strSql += vbCrLf + "  SELECT "
                strSql += vbCrLf + "   A.ACCODE ACID,G.ACGRPCODE,SG.ACGRPNAME ACGRPNAME,G.ACMAINCODE,SG.GRPLEDGER,SG.GRPTYPE"
                'strSql +=VBCRLF + "   A.ACCODE ACID,G.ACGRPCODE,ACGRPNAME ACGRPNAME,ACMAINCODE,G.GRPLEDGER,G.GRPTYPE"
                strSql += vbCrLf + "  ,DEBIT,CREDIT"
                strSql += vbCrLf + "  ,ACNAME ACNAME,TRANDATE,TRANMODE,AMOUNT,PAYMODE,TRANNO"
                strSql += vbCrLf + "  ,(SELECT ACGRPNAME FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPCODE = G.ACMAINCODE) ACMAINGRPNAME"
                strSql += vbCrLf + "  INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2"
                strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE1 AS T INNER JOIN " & cnAdminDb & "..ACHEAD AS A"
                strSql += vbCrLf + "  ON T.ACID = A.ACCODE INNER JOIN " & cnAdminDb & "..ACGROUP AS G ON G.ACGRPCODE = A.ACGRPCODE"
                strSql += vbCrLf + "  left JOIN " & cnAdminDb & "..ACGROUP AS SG ON SG.ACGRPCODE = A.ACGRPCODE"
                strSql += vbCrLf + "  WHERE (SG.GRPLEDGER IN ('T') OR G.GRPLEDGER IN ('T'))"
                If rbtCls_Stock_Man.Checked Then 'Val(objGPack.GetSqlValue("SELECT COUNT(*) FROM " & cnStockDb & "..CLOSINGVAL WHERE 1=1 " & strCostId, , , )) > 0 Then
                    strSql += vbCrLf + " UNION ALL"
                    strSql += vbCrLf + " SELECT 	C.CLSCODE ACID,H.ACGRPCODE,G.acgrpname ACGRPNAME,'0' ACMAINCODE 	,'T' GRPLEDGER,"
                    strSql += vbCrLf + " CASE WHEN SUM(VALUE) > 0 THEN 'I' ELSE 'E' END GRPTYPE 	,CASE WHEN SUM(VALUE) < 0 THEN SUM(VALUE)*(-1) END DEBIT 	,"
                    strSql += vbCrLf + " CASE WHEN SUM(VALUE) > 0 THEN SUM(VALUE) END CREDIT 	,H.ACNAME  ACNAME,'' TRANDATE 	,CASE WHEN SUM(VALUE) > 0 THEN 'D' ELSE 'C' END TRANMODE,SUM(VALUE) AMOUNT 	,'' PAYMODE,'' TRANNO,'CLOSING STOCK' ACMAINGRPNAME "
                    strSql += vbCrLf + " FROM " & cnStockDb & "..CLOSINGVAL A LEFT JOIN " & cnAdminDb & "..CATEGORY C ON A.CATCODE = C.CATCODE LEFT JOIN " & cnAdminDb & "..ACHEAD H ON C.CLSCODE = H.ACCODE "
                    strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..acgroup G ON H.ACGRPCODE = G.ACGRPCODE"
                    strSql += vbCrLf + " WHERE ISNULL(A.COMPANYID,'') = '" & strcompid & "'"
                    strSql += Replace(strCostId, "AND COSTID", "AND A.COSTID") + " GROUP BY H.ACGRPCODE,C.CLSCODE,H.ACNAME,G.ACGRPNAME"
                    strSql += vbCrLf + " HAVING SUM(VALUE) <> 0"
                    'strSql +=VBCRLF + " UNION ALL"
                    'strSql +=VBCRLF + " SELECT 	C.OPNCODE ACID,H.ACGRPCODE,G.ACGRPNAME ACGRPNAME,'0' ACMAINCODE 	,'T' GRPLEDGER,"
                    'strSql +=VBCRLF + " CASE WHEN SUM(AMOUNT) < 0 THEN 'I' ELSE 'E' END GRPTYPE 	"
                    'strSql +=VBCRLF + " ,CASE WHEN SUM(AMOUNT) > 0 THEN SUM(AMOUNT) END CREDIT 	"
                    'strSql +=VBCRLF + " ,CASE WHEN SUM(AMOUNT) < 0 THEN SUM(AMOUNT)*(-1) END DEBIT 	"
                    'strSql +=VBCRLF + " ,H.ACNAME  ACNAME,'' TRANDATE 	"
                    'strSql +=VBCRLF + " ,CASE WHEN SUM(AMOUNT) > 0 THEN 'D' ELSE 'C' END TRANMODE"
                    'strSql +=VBCRLF + " ,SUM(AMOUNT) AMOUNT 	,'' PAYMODE,'' TRANNO,'OPENING STOCK' ACMAINGRPNAME "
                    'strSql +=VBCRLF + " FROM " & cnStockDb & "..OPENWEIGHT A "
                    'strSql +=VBCRLF + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON A.CATCODE = C.CATCODE "
                    'strSql +=VBCRLF + " LEFT JOIN " & cnAdminDb & "..ACHEAD H ON C.OPNCODE = H.ACCODE "
                    'strSql +=VBCRLF + " LEFT JOIN " & cnAdminDb & "..ACGROUP G ON H.ACGRPCODE = G.ACGRPCODE"
                    'strSql +=VBCRLF + " WHERE ISNULL(A.COMPANYID,'') = '" & strcompid & "'"
                    'strSql +=VBCRLF + " AND A.STOCKTYPE='C'"
                    'strSql += Replace(strCostId, "AND COSTID", "AND A.COSTID") + " GROUP BY H.ACGRPCODE,C.OPNCODE,H.ACNAME,G.ACGRPNAME"
                    'strSql +=VBCRLF + " HAVING SUM(AMOUNT) <> 0"
                Else
                    ProgressBarShow()
                    Dim Qry As String = ""

                    If rbtCls_Stock_AvgRate.Checked Then
                        ProgressBarStep("Closing Stock arrival Weighted avg method", 10)
                        Qry = " EXEC " & cnAdminDb & "..SP_CLOSING_STOCK"
                    Else
                        ProgressBarStep("Closing Stock arrival LIFO method", 10)
                        Qry = " EXEC " & cnAdminDb & "..SP_CLOSINGSTOCK_LIFO"
                    End If
                    Qry += vbCrLf + " @DBNAME = '" & cnStockDb & "'"
                    Qry += vbCrLf + " ,@ADMINDB = '" & cnAdminDb & "'"
                    Qry += vbCrLf + " ,@FRMDATE = '" & Format(Startdate, "yyyy-MM-dd") & "'"
                    Qry += vbCrLf + " ,@TODATE = '" & Format(dtpDate.Value, "yyyy-MM-dd") & "'"
                    Qry += vbCrLf + " ,@METALNAME = 'ALL'"
                    Qry += vbCrLf + " ,@CATCODE = 'ALL'"
                    Qry += vbCrLf + " ,@CATNAME = 'ALL'"
                    Qry += vbCrLf + " ,@COSTNAME = '" & GetQryString(chkCmbCostCentre.Text).Replace("'", "") & "'"
                    Qry += vbCrLf + " ,@COMPANYID = '" & strcompid & "'"
                    Qry += vbCrLf + " ,@RPTTYPE = 'S'"
                    Dim SYSTEMID As String = Trim(LSet(Mid(Guid.NewGuid.ToString, 1, InStr(Guid.NewGuid.ToString, "-") - 1), 10))
                    Qry += vbCrLf + " ,@SYSTEMID = '" & SYSTEMID & "'"
                    cmd = New OleDbCommand(Qry, cn)
                    cmd.CommandTimeout = 2000
                    cmd.ExecuteNonQuery()


                    'strSql += vbCrLf + " UNION ALL"
                    'strSql += vbCrLf + " SELECT 	C.CLSCODE ACID,H.ACGRPCODE,G.ACGRPNAME ACGRPNAME,'0' ACMAINCODE ,'T' GRPLEDGER,"
                    'strSql += vbCrLf + " CASE WHEN SUM(CAMOUNT) > 0 THEN 'I' ELSE 'E' END GRPTYPE ,CASE WHEN SUM(CAMOUNT) < 0 THEN SUM(CAMOUNT)*(-1) END DEBIT 	,"
                    'strSql += vbCrLf + " CASE WHEN SUM(CAMOUNT) > 0 THEN SUM(CAMOUNT) END CREDIT ,H.ACNAME ACNAME,'' TRANDATE ,CASE WHEN SUM(CAMOUNT) > 0 THEN 'D' ELSE 'C' END TRANMODE,SUM(CAMOUNT) AMOUNT ,'' PAYMODE,'' TRANNO,'CLOSING STOCK' ACMAINGRPNAME "
                    'strSql += vbCrLf + " FROM TEMPTABLEDB..TEMPCSTKVALUE4" & SYSTEMID & " A "
                    'strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON A.CATEGORY = C.CATNAME "
                    'strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ACHEAD H ON C.CLSCODE = H.ACCODE "
                    'strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ACGROUP G ON H.ACGRPCODE = G.ACGRPCODE"
                    'strSql += vbCrLf + " WHERE A.RESULT=1"
                    'strSql += vbCrLf + " GROUP BY H.ACGRPCODE,C.CLSCODE,H.ACNAME,G.ACGRPNAME"
                    'strSql += vbCrLf + " HAVING SUM(CAMOUNT) <> 0"

                    strSql += vbCrLf + " UNION ALL"
                    strSql += vbCrLf + " SELECT 	C.CLSCODE ACID,H.ACGRPCODE,G.ACGRPNAME ACGRPNAME,'0' ACMAINCODE ,'T' GRPLEDGER"
                    strSql += vbCrLf + " ,'I' GRPTYPE ,0 DEBIT"
                    strSql += vbCrLf + " ,SUM(CAMOUNT) CREDIT ,H.ACNAME ACNAME,'' TRANDATE ,CASE WHEN SUM(CAMOUNT) > 0 THEN 'D' ELSE 'C' END TRANMODE,SUM(CAMOUNT) AMOUNT ,'' PAYMODE,'' TRANNO,'CLOSING STOCK' ACMAINGRPNAME "
                    strSql += vbCrLf + " FROM TEMPTABLEDB..TEMPCSTKVALUE4" & SYSTEMID & " A "
                    strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON A.CATEGORY = C.CATNAME "
                    strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ACHEAD H ON C.CLSCODE = H.ACCODE "
                    strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ACGROUP G ON H.ACGRPCODE = G.ACGRPCODE"
                    strSql += vbCrLf + " WHERE A.RESULT=1"
                    strSql += vbCrLf + " GROUP BY H.ACGRPCODE,C.CLSCODE,H.ACNAME,G.ACGRPNAME"
                    strSql += vbCrLf + " HAVING SUM(CAMOUNT) <> 0"

                End If
            End If

            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()


            strSql = " UPDATE A SET ACGRPCODE=(SELECT TOP 1 ACGRPCODE FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPNAME=A.ACMAINGRPNAME) FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 AS A WHERE ISNULL(ACGRPCODE,0)=0"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()


            strSql = " UPDATE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 SET ACMAINCODE=ACGRPCODE WHERE ISNULL(ACMAINCODE, 0)=0"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            ProgressBarHide()




            strSql = "  IF EXISTS (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & userId.ToString() & "TRADE3')"
            strSql += vbCrLf + "  DROP TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE3"
            strSql += vbCrLf + "  CREATE TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE3"
            strSql += vbCrLf + "  (EXPACGROUPNAME1 VARCHAR(250),EXPENSES1 VARCHAR(50),EXPENSES2 VARCHAR(50),EXPENSES VARCHAR(50)"
            strSql += vbCrLf + "  ,INCACGROUPNAME1 VARCHAR(250),INCOME1 VARCHAR(50),INCOME2 VARCHAR(50),INCOME VARCHAR(50)"
            strSql += vbCrLf + "  ,RESULT INT,SEP INT,EXPACMAINCODE VARCHAR(50),INCACMAINCODE VARCHAR(50),EXPACGROUP VARCHAR(250),INCACGROUP VARCHAR(250)"
            strSql += vbCrLf + "  ,EXPACID VARCHAR(50),EXPACNAME VARCHAR(250),EXPACGROUPNAME VARCHAR(250),EXPACMAINGRPNAME VARCHAR(250)"
            strSql += vbCrLf + "  ,INCACID VARCHAR(50),INCACNAME VARCHAR(250),INCACGROUPNAME VARCHAR(250),INCACMAINGRPNAME VARCHAR(250),SNO INT IDENTITY(1,1),COLHEAD VARCHAR(2)"
            strSql += vbCrLf + "  )"
            strSql += vbCrLf + "  "
            strSql += vbCrLf + "  IF EXISTS (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & userId.ToString() & "TRADE4')"
            strSql += vbCrLf + "  DROP TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE4"
            strSql += vbCrLf + "  CREATE TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE4"
            strSql += vbCrLf + "  (EXPACGROUPNAME1 VARCHAR(250),EXPENSES1 VARCHAR(50),EXPENSES2 VARCHAR(50),EXPENSES VARCHAR(50)"
            strSql += vbCrLf + "  ,INCACGROUPNAME1 VARCHAR(250),INCOME1 VARCHAR(50),INCOME2 VARCHAR(50),INCOME VARCHAR(50)"
            strSql += vbCrLf + "  ,RESULT INT,SEP INT,EXPACMAINCODE VARCHAR(50),INCACMAINCODE VARCHAR(50),EXPACGROUP VARCHAR(250),INCACGROUP VARCHAR(250)"
            strSql += vbCrLf + "  ,EXPACID VARCHAR(50),EXPACNAME VARCHAR(250),EXPACGROUPNAME VARCHAR(250),EXPACMAINGRPNAME VARCHAR(250)"
            strSql += vbCrLf + "  ,INCACID VARCHAR(50),INCACNAME VARCHAR(250),INCACGROUPNAME VARCHAR(250),INCACMAINGRPNAME VARCHAR(250),SNO INT IDENTITY(1,1),COLHEAD VARCHAR(2)"
            strSql += vbCrLf + "  )"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE3"
            strSql += vbCrLf + "  (EXPACGROUPNAME1,EXPENSES1,EXPENSES2,EXPENSES,INCACGROUPNAME1,INCOME1,INCOME2,INCOME,RESULT,SEP,EXPACMAINCODE,INCACMAINCODE,EXPACGROUP,INCACGROUP,EXPACID"
            strSql += vbCrLf + "  ,EXPACNAME,EXPACGROUPNAME,EXPACMAINGRPNAME,INCACID,INCACNAME,INCACGROUPNAME,INCACMAINGRPNAME,COLHEAD)"
            strSql += vbCrLf + "  SELECT "
            strSql += vbCrLf + "  DISTINCT ACMAINGRPNAME EXPACGROUPNAME1"
            strSql += vbCrLf + "  ,NULL EXPENSES1,NULL EXPENSES2,NULL EXPENSES"
            strSql += vbCrLf + "  ,NULL INCACGROUPNAME1"
            strSql += vbCrLf + "  ,NULL INCOME2,NULL INCOME1,NULL INCOME,'1' RESULT,'1' SEP"
            strSql += vbCrLf + "  ,ACMAINCODE EXPACMAINCODE,'' INCACMAINCODE,'' EXPACGROUP,'' INCACGROUP"
            strSql += vbCrLf + "  ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,ACMAINGRPNAME EXPACMAINGRPNAME"
            strSql += vbCrLf + "  ,''INCACID,''INCACNAME,''INCACGROUPNAME,'' INCACMAINGRPNAME,'T1' COLHEAD"
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'E'"
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT "
            strSql += vbCrLf + "  DISTINCT '  ' + ACGRPNAME EXPACGROUPNAME1"
            strSql += vbCrLf + "  ,NULL EXPENSES2,NULL EXPENSES1,NULL EXPENSES"
            strSql += vbCrLf + "  ,NULL INCACGROUPNAME1"
            strSql += vbCrLf + "  ,NULL INCOME2,NULL INCOME1,NULL INCOME,'2' RESULT,'2' SEP"
            strSql += vbCrLf + "  ,ACMAINCODE EXPACMAINCODE,'' INCACMAINCODE,ACGRPCODE EXPACGROUP,'' INCACGROUP"
            strSql += vbCrLf + "  ,'' EXPACID,'' EXPACNAME,ACGRPNAME EXPACGROUPNAME,ACMAINGRPNAME EXPACMAINGRPNAME"
            strSql += vbCrLf + "  ,''INCACID,''INCACNAME,''INCACGROUPNAME,'' INCACMAINGRPNAME,'T2' COLHEAD" & vbTab + Environment.NewLine
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'E' AND ISNULL(ACMAINCODE,'') != ISNULL(ACGRPCODE,'')"
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT "
            strSql += vbCrLf + "  DISTINCT CASE WHEN ACMAINGRPNAME = ACGRPNAME THEN '  ' ELSE '    ' END + ACNAME EXPACGROUPNAME1"
            strSql += vbCrLf + "  ,CASE WHEN ACMAINGRPNAME != ACGRPNAME THEN CONVERT(VARCHAR,ISNULL(SUM(DEBIT),0) - ISNULL(SUM(CREDIT),0)) END EXPENSES2"
            strSql += vbCrLf + "  ,CASE WHEN ACMAINGRPNAME = ACGRPNAME THEN CONVERT(VARCHAR,ISNULL(SUM(DEBIT),0) - ISNULL(SUM(CREDIT),0)) END EXPENSES1,NULL EXPENSES"
            strSql += vbCrLf + "  ,NULL INCACGROUPNAME1"
            strSql += vbCrLf + "  ,NULL INCOME2,NULL INCOME1,NULL INCOME,'3' RESULT,'2' SEP"
            strSql += vbCrLf + "  ,ACMAINCODE EXPACMAINCODE,'' INCACMAINCODE,ACGRPCODE EXPACGROUP,'' INCACGROUP"
            strSql += vbCrLf + "  ,ACID EXPACID,ACNAME EXPACNAME,ACGRPNAME EXPACGROUPNAME,ACMAINGRPNAME EXPACMAINGRPNAME"
            strSql += vbCrLf + "  ,''INCACID,''INCACNAME,''INCACGROUPNAME,'' INCACMAINGRPNAME,'' COLHEAD" & vbTab + Environment.NewLine
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'E'"
            strSql += vbCrLf + "  GROUP BY ACMAINGRPNAME,ACGRPNAME,ACMAINCODE,ACGRPCODE,ACID,ACNAME"
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT "
            strSql += vbCrLf + "  DISTINCT '' EXPACGROUPNAME1"
            strSql += vbCrLf + "  ,CASE WHEN ACMAINGRPNAME != ACGRPNAME THEN '-------------------' END EXPENSES2"
            strSql += vbCrLf + "  ,CASE WHEN ACMAINGRPNAME  = ACGRPNAME THEN '-------------------' END EXPENSES1,NULL EXPENSES"
            strSql += vbCrLf + "  ,NULL INCACGROUPNAME1"
            strSql += vbCrLf + "  ,NULL INCOME2,NULL INCOME1,NULL INCOME,'4' RESULT,'2' SEP"
            strSql += vbCrLf + "  ,ACMAINCODE EXPACMAINCODE,'' INCACMAINCODE,ACGRPCODE EXPACGROUP,'' INCACGROUP"
            strSql += vbCrLf + "  ,'' EXPACID,'' EXPACNAME,ACGRPNAME EXPACGROUPNAME,ACMAINGRPNAME EXPACMAINGRPNAME"
            strSql += vbCrLf + "  ,''INCACID,''INCACNAME,''INCACGROUPNAME,'' INCACMAINGRPNAME,'S2' COLHEAD" & vbTab + Environment.NewLine
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'E'"
            strSql += vbCrLf + "  GROUP BY ACMAINGRPNAME,ACGRPNAME,ACMAINCODE,ACGRPCODE"
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT "
            strSql += vbCrLf + "  DISTINCT ' ' EXPACGROUPNAME1"
            strSql += vbCrLf + "  ,NULL EXPENSES1,CONVERT(VARCHAR,ISNULL(SUM(DEBIT),0) - ISNULL(SUM(CREDIT),0)) EXPENSES2,NULL EXPENSES"
            strSql += vbCrLf + "  ,NULL INCACGROUPNAME1"
            strSql += vbCrLf + "  ,NULL INCOME2,NULL INCOME1,NULL INCOME,'5' RESULT,'2' SEP"
            strSql += vbCrLf + "  ,ACMAINCODE EXPACMAINCODE,'' INCACMAINCODE,ACGRPCODE EXPACGROUP,'' INCACGROUP"
            strSql += vbCrLf + "  ,'' EXPACID,'' EXPACNAME,ACGRPNAME EXPACGROUPNAME,ACMAINGRPNAME EXPACMAINGRPNAME"
            strSql += vbCrLf + "  ,''INCACID,''INCACNAME,''INCACGROUPNAME,'' INCACMAINGRPNAME,'S2' COLHEAD" & vbTab + Environment.NewLine
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'E' AND ISNULL(ACMAINCODE,'') != ISNULL(ACGRPCODE,'')"
            strSql += vbCrLf + "  GROUP BY ACMAINGRPNAME,ACGRPNAME,ACMAINCODE,ACGRPCODE"
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT "
            strSql += vbCrLf + "  DISTINCT '  ' EXPACGROUPNAME1"
            strSql += vbCrLf + "  ,NULL EXPENSES1,'-------------------' EXPENSES2,NULL EXPENSES"
            strSql += vbCrLf + "  ,NULL INCACGROUPNAME1"
            strSql += vbCrLf + "  ,NULL INCOME2,NULL INCOME1,NULL INCOME,'6' RESULT,'2' SEP"
            strSql += vbCrLf + "  ,ACMAINCODE EXPACMAINCODE,'' INCACMAINCODE,ACGRPCODE EXPACGROUP,'' INCACGROUP"
            strSql += vbCrLf + "  ,'' EXPACID,'' EXPACNAME,ACGRPNAME EXPACGROUPNAME,ACMAINGRPNAME EXPACMAINGRPNAME"
            strSql += vbCrLf + "  ,''INCACID,''INCACNAME,''INCACGROUPNAME,'' INCACMAINGRPNAME,'S1' COLHEAD" & vbTab + Environment.NewLine
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'E' AND ISNULL(ACMAINCODE,'') != ISNULL(ACGRPCODE,'')"
            strSql += vbCrLf + "  GROUP BY ACMAINGRPNAME,ACGRPNAME,ACMAINCODE,ACGRPCODE"
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT "
            strSql += vbCrLf + "  DISTINCT '' EXPACGROUPNAME1"
            strSql += vbCrLf + "  ,NULL EXPENSES2,NULL EXPENSES1,CONVERT(VARCHAR,ISNULL(SUM(DEBIT),0) - ISNULL(SUM(CREDIT),0)) EXPENSES"
            strSql += vbCrLf + "  ,NULL INCACGROUPNAME1"
            strSql += vbCrLf + "  ,NULL INCOME2,NULL INCOME1,NULL INCOME"
            strSql += vbCrLf + "  ,'7' RESULT,'3' SEP"
            strSql += vbCrLf + "  ,ACMAINCODE EXPACMAINCODE,'' INCACMAINCODE,'' EXPACGROUP,'' INCACGROUP"
            strSql += vbCrLf + "  ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,ACMAINGRPNAME EXPACMAINGRPNAME"
            strSql += vbCrLf + "  ,''INCACID,''INCACNAME,''INCACGROUPNAME,'' INCACMAINGRPNAME,'G1' COLHEAD" & vbTab + Environment.NewLine
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'E'"
            strSql += vbCrLf + "  GROUP BY ACMAINGRPNAME,ACMAINCODE"
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT "
            strSql += vbCrLf + "  DISTINCT '' EXPACGROUPNAME1"
            strSql += vbCrLf + "  ,NULL EXPENSES2,NULL EXPENSES1,'-------------------' EXPENSES"
            strSql += vbCrLf + "  ,NULL INCACGROUPNAME1"
            strSql += vbCrLf + "  ,NULL INCOME2,NULL INCOME1,NULL INCOME"
            strSql += vbCrLf + "  ,'8' RESULT,'4' SEP"
            strSql += vbCrLf + "  ,ACMAINCODE EXPACMAINCODE,'' INCACMAINCODE,'' EXPACGROUP,'' INCACGROUP"
            strSql += vbCrLf + "  ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,ACMAINGRPNAME EXPACMAINGRPNAME"
            strSql += vbCrLf + "  ,''INCACID,''INCACNAME,''INCACGROUPNAME,'' INCACMAINGRPNAME,'G1' COLHEAD" & vbTab + Environment.NewLine
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'E'"
            strSql += vbCrLf + "  GROUP BY ACMAINGRPNAME,ACMAINCODE"
            strSql += vbCrLf + "  ORDER BY EXPACMAINCODE,EXPACMAINGRPNAME,SEP,EXPACGROUPNAME,RESULT,EXPACNAME"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE4"
            strSql += vbCrLf + "  (EXPACGROUPNAME1,EXPENSES1,EXPENSES2,EXPENSES,INCACGROUPNAME1,INCOME1,INCOME2,INCOME,RESULT,SEP,EXPACMAINCODE,INCACMAINCODE,EXPACGROUP,INCACGROUP,EXPACID"
            strSql += vbCrLf + "  ,EXPACNAME,EXPACGROUPNAME,EXPACMAINGRPNAME,INCACID,INCACNAME,INCACGROUPNAME,INCACMAINGRPNAME,COLHEAD)"
            strSql += vbCrLf + "  SELECT "
            strSql += vbCrLf + "  DISTINCT NULL EXPACGROUPNAME1"
            strSql += vbCrLf + "  ,NULL EXPENSES1,NULL EXPENSES2,NULL EXPENSES"
            strSql += vbCrLf + "  ,ACMAINGRPNAME INCACGROUPNAME1"
            strSql += vbCrLf + "  ,NULL INCOME2,NULL INCOME1,NULL INCOME,'1' RESULT,'1' SEP"
            strSql += vbCrLf + "  ,'' EXPACMAINCODE,ACMAINCODE INCACMAINCODE,'' EXPACGROUP,'' INCACGROUP"
            strSql += vbCrLf + "  ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,'' EXPACMAINGRPNAME"
            strSql += vbCrLf + "  ,'' INCACID,''INCACNAME,''INCACGROUPNAME,ACMAINGRPNAME INCACMAINGRPNAME,'T1' COLHEAD" & vbTab + Environment.NewLine
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'I'"
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT "
            strSql += vbCrLf + "  DISTINCT NULL EXPACGROUPNAME1"
            strSql += vbCrLf + "  ,NULL EXPENSES2,NULL EXPENSES1,NULL EXPENSES"
            strSql += vbCrLf + "  ,'  ' + ACGRPNAME INCACGROUPNAME1"
            strSql += vbCrLf + "  ,NULL INCOME2,NULL INCOME1,NULL INCOME,'2' RESULT,'2' SEP"
            strSql += vbCrLf + "  ,'' EXPACMAINCODE,ACMAINCODE INCACMAINCODE,'' EXPACGROUP,ACGRPCODE INCACGROUP"
            strSql += vbCrLf + "  ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,'' EXPACMAINGRPNAME"
            strSql += vbCrLf + "  ,''INCACID,''INCACNAME,ACGRPNAME INCACGROUPNAME,ACMAINGRPNAME INCACMAINGRPNAME,'T2' COLHEAD" & vbTab + Environment.NewLine
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'I' AND ISNULL(ACMAINCODE,'') != ISNULL(ACGRPCODE,'')"
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT "
            strSql += vbCrLf + "  DISTINCT '' EXPACGROUPNAME1"
            strSql += vbCrLf + "  ,NULL EXPENSES2,NULL EXPENSES1,NULL EXPENSES"
            strSql += vbCrLf + "  ,CASE WHEN ACMAINGRPNAME = ACGRPNAME THEN '  ' ELSE '    ' END + ACNAME INCACGROUPNAME1"
            strSql += vbCrLf + "  ,CASE WHEN ACMAINGRPNAME != ACGRPNAME THEN CONVERT(VARCHAR,ISNULL(SUM(CREDIT),0) - ISNULL(SUM(DEBIT),0)) END INCOME2"
            strSql += vbCrLf + "  ,CASE WHEN ACMAINGRPNAME  = ACGRPNAME THEN CONVERT(VARCHAR,ISNULL(SUM(CREDIT),0) - ISNULL(SUM(DEBIT),0)) END INCOME1,NULL INCOME,'3' RESULT,'2' SEP"
            strSql += vbCrLf + "  ,'' EXPACMAINCODE,ACMAINCODE INCACMAINCODE,'' EXPACGROUP,ACGRPCODE INCACGROUP"
            strSql += vbCrLf + "  ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,'' EXPACMAINGRPNAME"
            strSql += vbCrLf + "  ,ACID INCACID,ACNAME INCACNAME,ACGRPNAME INCACGROUPNAME,ACMAINGRPNAME INCACMAINGRPNAME,'' COLHEAD" & vbTab + Environment.NewLine
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'I'"
            strSql += vbCrLf + "  GROUP BY ACMAINGRPNAME,ACGRPNAME,ACMAINCODE,ACGRPCODE,ACID,ACNAME"
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT "
            strSql += vbCrLf + "  DISTINCT '' EXPACGROUPNAME1"
            strSql += vbCrLf + "  ,'' EXPENSES2,NULL EXPENSES1,NULL EXPENSES"
            strSql += vbCrLf + "  ,NULL INCACGROUPNAME1"
            strSql += vbCrLf + "  ,CASE WHEN ACMAINGRPNAME != ACGRPNAME THEN '-------------------' END INCOME2"
            strSql += vbCrLf + "  ,CASE WHEN ACMAINGRPNAME  = ACGRPNAME THEN '-------------------' END INCOME1,NULL INCOME,'4' RESULT,'2' SEP"
            strSql += vbCrLf + "  ,'' EXPACMAINCODE,ACMAINCODE INCACMAINCODE,'' EXPACGROUP,ACGRPCODE INCACGROUP"
            strSql += vbCrLf + "  ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,'' EXPACMAINGRPNAME"
            strSql += vbCrLf + "  ,''INCACID,''INCACNAME,ACGRPNAME INCACGROUPNAME,ACMAINGRPNAME INCACMAINGRPNAME,'S2' COLHEAD"
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'I'"
            strSql += vbCrLf + "  GROUP BY ACMAINGRPNAME,ACGRPNAME,ACMAINCODE,ACGRPCODE"
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT "
            strSql += vbCrLf + "  DISTINCT ' ' EXPACGROUPNAME1"
            strSql += vbCrLf + "  ,NULL EXPENSES2,NULL EXPENSES1,NULL EXPENSES"
            strSql += vbCrLf + "  ,NULL INCACGROUPNAME1"
            strSql += vbCrLf + "  ,NULL INCOME2,CONVERT(VARCHAR,ISNULL(SUM(CREDIT),0) - ISNULL(SUM(DEBIT),0)) INCOME1,NULL INCOME,'5' RESULT,'2' SEP"
            strSql += vbCrLf + "  ,'' EXPACMAINCODE,ACMAINCODE INCACMAINCODE,'' EXPACGROUP,ACGRPCODE INCACGROUP"
            strSql += vbCrLf + "  ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,'' EXPACMAINGRPNAME"
            strSql += vbCrLf + "  ,''INCACID,''INCACNAME,ACGRPNAME INCACGROUPNAME,ACMAINGRPNAME INCACMAINGRPNAME,'S1' COLHEAD" & vbTab + Environment.NewLine
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'I' AND ISNULL(ACMAINCODE,'') != ISNULL(ACGRPCODE,'')"
            strSql += vbCrLf + "  GROUP BY ACMAINGRPNAME,ACGRPNAME,ACMAINCODE,ACGRPCODE"
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT "
            strSql += vbCrLf + "  DISTINCT '  ' EXPACGROUPNAME1"
            strSql += vbCrLf + "  ,NULL EXPENSES2,'' EXPENSES1,NULL EXPENSES"
            strSql += vbCrLf + "  ,NULL INCACGROUPNAME1"
            strSql += vbCrLf + "  ,NULL INCOME2,'-------------------' INCOME1,NULL INCOME,'6' RESULT,'2' SEP"
            strSql += vbCrLf + "  ,ACMAINCODE EXPACMAINCODE,'' INCACMAINCODE,ACGRPCODE EXPACGROUP,'' INCACGROUP"
            strSql += vbCrLf + "  ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME ,'' EXPACMAINGRPNAME"
            strSql += vbCrLf + "  ,''INCACID,''INCACNAME,ACGRPNAME INCACGROUPNAME,ACMAINGRPNAME INCACMAINGRPNAME,'S2' COLHEAD" & vbTab + Environment.NewLine
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'I' AND ISNULL(ACMAINCODE,'') != ISNULL(ACGRPCODE,'')"
            strSql += vbCrLf + "  GROUP BY ACMAINGRPNAME,ACGRPNAME,ACMAINCODE,ACGRPCODE"
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT "
            strSql += vbCrLf + "  DISTINCT '' EXPACGROUPNAME1"
            strSql += vbCrLf + "  ,NULL EXPENSES2,NULL EXPENSES1,NULL EXPENSES"
            strSql += vbCrLf + "  ,NULL INCACGROUPNAME1"
            strSql += vbCrLf + "  ,NULL INCOME2,NULL INCOME1,CONVERT(VARCHAR,ISNULL(SUM(CREDIT),0) - ISNULL(SUM(DEBIT),0)) INCOME"
            strSql += vbCrLf + "  ,'7' RESULT,'3' SEP"
            strSql += vbCrLf + "  ,'' EXPACMAINCODE,ACMAINCODE INCACMAINCODE,'' EXPACGROUP,'' INCACGROUP"
            strSql += vbCrLf + "  ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,'' EXPACMAINGRPNAME"
            strSql += vbCrLf + "  ,''INCACID,''INCACNAME,''INCACGROUPNAME,ACMAINGRPNAME INCACMAINGRPNAME,'G1' COLHEAD" & vbTab + Environment.NewLine
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'I'"
            'FOR ANB STARTS
            'strSql += vbCrLf + "  AND ISNULL(ACMAINCODE,'') != ISNULL(ACGRPCODE,'')"
            'FOR ANB ENDS
            strSql += vbCrLf + "  GROUP BY ACMAINGRPNAME,ACMAINCODE"
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT "
            strSql += vbCrLf + "  DISTINCT '' EXPACGROUPNAME1"
            strSql += vbCrLf + "  ,NULL EXPENSES2,NULL EXPENSES1,'' EXPENSES"
            strSql += vbCrLf + "  ,NULL INCACGROUPNAME1"
            strSql += vbCrLf + "  ,NULL INCOME2,NULL INCOME1,'-------------------' INCOME"
            strSql += vbCrLf + "  ,'8' RESULT,'4' SEP"
            strSql += vbCrLf + "  ,'' EXPACMAINCODE,ACMAINCODE INCACMAINCODE,'' EXPACGROUP,'' INCACGROUP"
            strSql += vbCrLf + "  ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,'' EXPACMAINGRPNAME"
            strSql += vbCrLf + "  ,''INCACID,''INCACNAME,''INCACGROUPNAME,ACMAINGRPNAME INCACMAINGRPNAME,'G1' COLHEAD" & vbTab + Environment.NewLine
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'I'"
            strSql += vbCrLf + "  GROUP BY ACMAINGRPNAME,ACMAINCODE"
            strSql += vbCrLf + "  ORDER BY INCACMAINGRPNAME,SEP,INCACGROUPNAME,RESULT,INCACNAME"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = "  IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & userId.ToString() & "TRADE5')"
            strSql += vbCrLf + "  DROP TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE5"
            strSql += vbCrLf + "  SELECT "
            strSql += vbCrLf + "  T3.EXPACGROUPNAME1,T3.EXPENSES1,T3.EXPENSES2,T3.EXPENSES,T4.INCACGROUPNAME1,T4.INCOME1,T4.INCOME2,T4.INCOME"
            strSql += vbCrLf + "  ,T3.SNO SNO3,T4.SNO SNO4,T3.RESULT RESULT3,T4.RESULT RESULT4,T3.SEP SEP3,T4.SEP SEP4,T3.COLHEAD COLHEAD3,T4.COLHEAD COLHEAD4"
            strSql += vbCrLf + "  INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE5"
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE3 AS T3 FULL JOIN TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE4 AS T4 "
            strSql += vbCrLf + "  ON T3.SNO = T4.SNO"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE5"
            strSql += vbCrLf + " (EXPACGROUPNAME1,EXPENSES,INCACGROUPNAME1,INCOME,COLHEAD3,COLHEAD4,RESULT3,RESULT4)"
            strSql += vbCrLf + " SELECT"
            strSql += vbCrLf + " CASE WHEN SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT4 IN ('0','7') THEN INCOME ELSE '0' END)) "
            strSql += vbCrLf + " - SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT3 IN ('0','7') THEN EXPENSES ELSE '0' END)) > 0 THEN 'GROSS PROFIT' END EXPACGROUPNAME1 "
            strSql += vbCrLf + " ,CONVERT(VARCHAR,CASE WHEN SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT4 IN ('0','7') THEN INCOME ELSE '0' END)) - SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT3 IN ('0','7') "
            strSql += vbCrLf + " THEN EXPENSES ELSE '0' END)) > 0 THEN SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT4 IN ('0','7') THEN INCOME ELSE '0' END)) "
            strSql += vbCrLf + " - SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT3 IN ('0','7') THEN EXPENSES ELSE '0' END)) END) EXPENSES"
            strSql += vbCrLf + " , CASE WHEN SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT3 IN ('0','7') THEN EXPENSES ELSE '0' END)) - SUM(CONVERT(NUMERIC(15,2)"
            strSql += vbCrLf + " ,CASE WHEN RESULT4 IN ('0','7') THEN INCOME ELSE '0' END)) > 0 THEN 'GROSS LOSS' END INCACGROUPNAME1  "
            strSql += vbCrLf + " ,CONVERT(VARCHAR,CASE WHEN SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT3 IN ('0','7') THEN EXPENSES ELSE '0' END)) - SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT4 IN ('0','7') "
            strSql += vbCrLf + " THEN INCOME ELSE '0' END)) > 0 THEN SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT3 IN ('0','7') THEN EXPENSES ELSE '0' END)) "
            strSql += vbCrLf + " - SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT4 IN ('0','7') THEN INCOME ELSE '0' END)) END) INCOME"
            strSql += vbCrLf + " ,'G' COLHEAD3,'G' COLHEAD4"
            strSql += vbCrLf + " ,CASE WHEN SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT4 IN ('0','7') THEN INCOME ELSE '0' END)) "
            strSql += vbCrLf + " - SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT3 IN ('0','7') THEN EXPENSES ELSE '0' END)) > 0 THEN '9' END RESULT3"
            strSql += vbCrLf + " , CASE WHEN SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT3 IN ('0','7') THEN EXPENSES ELSE '0' END))"
            strSql += vbCrLf + " - SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT4 IN ('0','7') THEN INCOME ELSE '0' END)) > 0 THEN '9' END RESULT4"
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE5"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = " SELECT "
            strSql += vbCrLf + " DISTINCT 'TRADING - 01/" & CnStartMonth & "/" & (IIf(dtpDate.Value.[Date].Month > 3, dtpDate.Value.[Date].Year, dtpDate.Value.[Date].Year - 1)).ToString() & " - " & dtpDate.Text.Replace("-", "/") & "' EXPACGROUPNAME1,' 'EXPENSES1,' ' EXPENSES2,' 'EXPENSES "
            strSql += vbCrLf + " ,' ' INCACGROUPNAME1,' 'INCOME1,' 'INCOME2,' 'INCOME,'T' COLHEAD3,'T' COLHEAD4"
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE5"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + "  SELECT "
            strSql += vbCrLf + "  EXPACGROUPNAME1"
            strSql += vbCrLf + "     ,CASE WHEN RESULT3 NOT IN (4,6,8) AND ISNULL(EXPENSES1,'') != '' THEN " & cnStockDb & ".[DBO].FUNC_RUPEES_WITHCOMMA(CONVERT(NUMERIC(15,2),CASE WHEN ISNULL(EXPENSES1,'') = '' THEN '0' ELSE EXPENSES1 END),'D','Y') ELSE EXPENSES1 END  EXPENSES1"
            strSql += vbCrLf + "     ,CASE WHEN RESULT3 NOT IN (4,6,8) AND ISNULL(EXPENSES2,'') != '' THEN " & cnStockDb & ".[DBO].FUNC_RUPEES_WITHCOMMA(CONVERT(NUMERIC(15,2),CASE WHEN ISNULL(EXPENSES2,'') = '' THEN '0' ELSE EXPENSES2 END),'D','Y') ELSE EXPENSES2 END EXPENSES2"
            strSql += vbCrLf + "     ,CASE WHEN RESULT3 NOT IN (4,6,8) AND ISNULL(EXPENSES,'') != '' THEN " & cnStockDb & ".[DBO].FUNC_RUPEES_WITHCOMMA(CONVERT(NUMERIC(15,2),CASE WHEN ISNULL(EXPENSES,'') = '' THEN '0' ELSE EXPENSES END),'D','Y') ELSE EXPENSES END EXPENSES "
            strSql += vbCrLf + "  ,INCACGROUPNAME1"
            strSql += vbCrLf + "     ,CASE WHEN RESULT4 NOT IN (4,6,8) AND ISNULL(INCOME1,'') != '' THEN " & cnStockDb & ".[DBO].FUNC_RUPEES_WITHCOMMA(CONVERT(NUMERIC(15,2),CASE WHEN ISNULL(INCOME1,'') = '' THEN '0' ELSE INCOME1 END),'D','Y') ELSE INCOME1 END  INCOME1"
            strSql += vbCrLf + "     ,CASE WHEN RESULT4 NOT IN (4,6,8) AND ISNULL(INCOME2,'') != '' THEN " & cnStockDb & ".[DBO].FUNC_RUPEES_WITHCOMMA(CONVERT(NUMERIC(15,2),CASE WHEN ISNULL(INCOME2,'') = '' THEN '0' ELSE INCOME2 END),'D','Y') ELSE INCOME2 END INCOME2"
            strSql += vbCrLf + "     ,CASE WHEN RESULT4 NOT IN (4,6,8) AND ISNULL(INCOME,'') != '' THEN " & cnStockDb & ".[DBO].FUNC_RUPEES_WITHCOMMA(CONVERT(NUMERIC(15,2),CASE WHEN ISNULL(INCOME,'') = '' THEN '0' ELSE INCOME END),'D','Y') ELSE INCOME END INCOME "
            strSql += vbCrLf + "     ,COLHEAD3,COLHEAD4"
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE5"
            strSql += vbCrLf + "  UNION ALL"
            'strSql += vbCrLf + "  SELECT"
            'strSql += vbCrLf + "    '' EXPACGROUPNAME1 ,NULL EXPENSES1,NULL EXPENSES2,CONVERT(VARCHAR," & cnStockDb & ".[DBO].FUNC_RUPEES_WITHCOMMA(SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT3 IN ('0','7') AND ISNULL(EXPENSES,'') = '' THEN CONVERT(NUMERIC(15,2),CASE WHEN ISNULL(EXPENSES,'') = '' THEN '0' ELSE EXPENSES END) ELSE '0' END)),'D','Y')) EXPENSES"
            'strSql += vbCrLf + "  ,'' INCACGROUPNAME1 ,NULL INCOME1,NULL INCOME2,CONVERT(VARCHAR," & cnStockDb & ".[DBO].FUNC_RUPEES_WITHCOMMA(SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT4 IN ('0','7') AND ISNULL(INCOME,'') = '' THEN CONVERT(NUMERIC(15,2),CASE WHEN ISNULL(INCOME,'') = '' THEN '0' ELSE INCOME END) ELSE '0' END)),'D','Y')) INCOME,'G1' COLHEAD3,'G1' COLHEAD4"
            'strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE5"
            'strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT"
            strSql += vbCrLf + "   DISTINCT '' EXPACGROUPNAME1 ,NULL EXPENSES1,NULL EXPENSES2,'-------------------' EXPENSES"
            strSql += vbCrLf + "  ,'' INCACGROUPNAME1 ,NULL INCOME1,NULL INCOME2,'-------------------' INCOME,'G1' COLHEAD3,'G1' COLHEAD4"
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE5"
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT"
            strSql += vbCrLf + "    '' EXPACGROUPNAME1 ,NULL EXPENSES1,NULL EXPENSES2,CONVERT(VARCHAR," & cnStockDb & ".[DBO].FUNC_RUPEES_WITHCOMMA(SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT3 IN ('0','7','9') /*AND ISNULL(EXPENSES,'') = ''*/ THEN CONVERT(NUMERIC(15,2),CASE WHEN ISNULL(EXPENSES,'') = '' THEN '0' ELSE EXPENSES END) ELSE '0' END)),'D','Y')) EXPENSES"
            strSql += vbCrLf + "  ,'' INCACGROUPNAME1 ,NULL INCOME1,NULL INCOME2,CONVERT(VARCHAR," & cnStockDb & ".[DBO].FUNC_RUPEES_WITHCOMMA(SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT4 IN ('0','7','9') /*AND ISNULL(INCOME,'') = ''*/ THEN CONVERT(NUMERIC(15,2),CASE WHEN ISNULL(INCOME,'') = '' THEN '0' ELSE INCOME END) ELSE '0' END)),'D','Y')) INCOME,'G1' COLHEAD3,'G1' COLHEAD4"
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE5"
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT"
            strSql += vbCrLf + "   DISTINCT '' EXPACGROUPNAME1 ,NULL EXPENSES1,NULL EXPENSES2,'-------------------' EXPENSES"
            strSql += vbCrLf + "  ,'' INCACGROUPNAME1 ,NULL INCOME1,NULL INCOME2,'-------------------' INCOME,'G1' COLHEAD3,'G1' COLHEAD4"
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE5"
            'strSql += vbCrLf + "  UNION ALL"
            'strSql += vbCrLf + "  SELECT"
            'strSql += vbCrLf + "    CASE WHEN SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT3 IN ('0','7') THEN EXPENSES ELSE '0' END)) - SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT4 IN ('0','7') THEN INCOME ELSE '0' END)) > 0 THEN 'GROSS LOSS' END EXPACGROUPNAME1 ,NULL EXPENSES1,NULL EXPENSES2"
            'strSql += vbCrLf + "  ,CONVERT(VARCHAR,CASE WHEN SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT3 IN ('0','7') THEN EXPENSES ELSE '0' END)) - SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT4 IN ('0','7') THEN INCOME ELSE '0' END)) > 0 THEN " & cnStockDb & ".[DBO].FUNC_RUPEES_WITHCOMMA(SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT3 IN ('0','7') THEN EXPENSES ELSE '0' END)) - SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT4 IN ('0','7') THEN INCOME ELSE '0' END)),'D','Y') END) EXPENSES"
            'strSql += vbCrLf + "  ,CASE WHEN SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT4 IN ('0','7') THEN INCOME ELSE '0' END)) - SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT3 IN ('0','7') THEN EXPENSES ELSE '0' END)) > 0 THEN 'GROSS PROFIT' END INCACGROUPNAME1 ,NULL INCOME1,NULL INCOME2"
            'strSql += vbCrLf + "  ,CONVERT(VARCHAR,CASE WHEN SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT4 IN ('0','7') THEN INCOME ELSE '0' END)) - SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT3 IN ('0','7') THEN EXPENSES ELSE '0' END)) > 0 THEN " & cnStockDb & ".[DBO].FUNC_RUPEES_WITHCOMMA(SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT4 IN ('0','7') THEN INCOME ELSE '0' END)) - SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT3 IN ('0','7') THEN EXPENSES ELSE '0' END)),'D','Y') END) INCOME,'G' COLHEAD3,'G' COLHEAD4"
            'strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE5"
            If ForPL = False Then
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dt)
                If dt.Rows.Count <= 2 Then
                    dt = New DataTable()
                End If
            End If
            'gridView.DataSource = dt
        Catch ex As Exception
            If Val(cnttchkk) < 5 Then
                cnttchkk += 1
                GoTo TTTT
            Else
                MsgBox(ex.Message, MsgBoxStyle.Information)
            End If
        End Try
    End Sub
    Private Sub funcPL(ByVal strcompid As String, ByVal strCostId As String, ByVal summary As Boolean)
        If chkTrading.Checked = False Then funcTrading(strcompid, strCostId, summary, True)
        strSql = " IF EXISTS (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & userId.ToString() & "PROFITLOSS1')" & vbCrLf
        strSql += " DROP TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS1" & vbCrLf
        strSql += " CREATE TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS1" & vbCrLf
        strSql += " (COMPANYID VARCHAR(3),ACID VARCHAR(25),TRANMODE VARCHAR(1),AMOUNT NUMERIC(15,2),DEBIT NUMERIC(15,2),CREDIT NUMERIC(15,2)" & vbCrLf
        strSql += " ,TRANDATE SMALLDATETIME,PAYMODE VARCHAR(20),TRANNO VARCHAR(25),SEP VARCHAR(1))" & vbCrLf
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " INSERT INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS1" & vbCrLf
        strSql += " (COMPANYID,ACID,TRANMODE,AMOUNT,DEBIT,CREDIT,TRANDATE,PAYMODE,TRANNO,SEP)" & vbCrLf
        strSql += " SELECT " & vbCrLf
        strSql += "  COMPANYID,ACCODE ACID,CASE WHEN ISNULL(DEBIT,0) > ISNULL(CREDIT,0) THEN 'D' ELSE 'C' END TRANMODE" & vbCrLf
        strSql += " ,CASE WHEN ISNULL(DEBIT,0) <> 0 THEN DEBIT ELSE CREDIT END AMOUNT" & vbCrLf
        strSql += " ,DEBIT,CREDIT,NULL TRANDATE,'' PAYMODE,NULL TRANNO,'O' SEP" & vbCrLf
        strSql += " FROM " & cnStockDb & "..OPENTRAILBALANCE WHERE ISNULL(COMPANYID,'') = '" & strcompid & "'" & vbCrLf
        strSql += strCostId
        strSql += " UNION ALL" & vbCrLf
        strSql += " SELECT " & vbCrLf
        strSql += "  COMPANYID,ACCODE ACID,TRANMODE,AMOUNT" & vbCrLf
        strSql += " ,CASE WHEN TRANMODE = 'D' THEN AMOUNT END DEBIT,CASE WHEN TRANMODE = 'C' THEN AMOUNT END CREDIT" & vbCrLf
        strSql += " ,TRANDATE,PAYMODE,TRANNO,'T' SEP" & vbCrLf
        strSql += " FROM " & cnStockDb & "..ACCTRAN WHERE ISNULL(COMPANYID,'') = '" & strcompid & "' AND TRANDATE >= '" & cnTranFromDate & "' AND TRANDATE <= '" & dtpDate.Value.Date.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') != 'Y'" & vbCrLf
        strSql += strCostId
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " IF EXISTS (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & userId.ToString() & "PROFITLOSS2')" & vbCrLf
        strSql += " DROP TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS2" & vbCrLf
        If summary Then
            strSql += "  SELECT " & vbCrLf
            strSql += "   '' ACID,G.ACGRPCODE,SG.ACGRPNAME ACGRPNAME,G.ACMAINCODE,SG.GRPLEDGER,SG.GRPTYPE" & vbCrLf
            'strSql += "   A.ACCODE ACID,G.ACGRPCODE,ACGRPNAME ACGRPNAME,ACMAINCODE,G.GRPLEDGER,G.GRPTYPE" & vbCrLf
            strSql += "  ,SUM(DEBIT) DEBIT,SUM(CREDIT) CREDIT" & vbCrLf
            strSql += "  ,'' ACNAME,'' TRANDATE,CASE WHEN SUM(AMOUNT) > 0 THEN 'D' ELSE 'C' END TRANMODE,SUM(AMOUNT) AMOUNT,'' PAYMODE,'' TRANNO" & vbCrLf
            strSql += "  ,(SELECT ACGRPNAME FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPCODE = G.ACMAINCODE) ACMAINGRPNAME" & vbCrLf
            strSql += "  INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS2" & vbCrLf
            strSql += "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS1 AS T INNER JOIN " & cnAdminDb & "..ACHEAD AS A" & vbCrLf
            strSql += "  ON T.ACID = A.ACCODE INNER JOIN " & cnAdminDb & "..ACGROUP AS G ON G.ACGRPCODE = A.ACGRPCODE" & vbCrLf
            strSql += "  INNER JOIN " & cnAdminDb & "..ACGROUP AS SG ON SG.ACGRPCODE = A.ACGRPCODE"
            strSql += "  AND (SG.GRPLEDGER IN ('P') OR G.GRPLEDGER IN ('P')) " & vbCrLf
            strSql += "  GROUP BY G.ACGRPCODE,SG.ACGRPNAME,G.ACMAINCODE,SG.GRPLEDGER,SG.GRPTYPE"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        Else
            strSql += " SELECT " & vbCrLf
            strSql += "  A.ACCODE ACID,G.ACGRPCODE,ACGRPNAME,ACMAINCODE,GRPLEDGER,GRPTYPE" & vbCrLf
            strSql += " ,DEBIT,CREDIT" & vbCrLf
            strSql += " ,ACNAME ACNAME,TRANDATE,TRANMODE,AMOUNT,PAYMODE,TRANNO" & vbCrLf
            strSql += " ,(SELECT ACGRPNAME FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPCODE = G.ACMAINCODE) ACMAINGRPNAME" & vbCrLf
            strSql += " INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS2" & vbCrLf
            strSql += " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS1 AS T INNER JOIN " & cnAdminDb & "..ACHEAD AS A" & vbCrLf
            strSql += " ON T.ACID = A.ACCODE INNER JOIN " & cnAdminDb & "..ACGROUP AS G ON G.ACGRPCODE = A.ACGRPCODE" & vbCrLf
            strSql += " AND GRPLEDGER IN ('P')" & vbCrLf
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End If
        strSql = " IF EXISTS (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & userId.ToString() & "PROFITLOSS0')" & vbCrLf
        strSql += " DROP TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS0" & vbCrLf
        'strSql += " SELECT " & vbCrLf
        'strSql += "  'TRADING A/C' PARTICULARS" & vbTab + Environment.NewLine
        'strSql += "  ,ISNULL(SUM(ISNULL((CASE WHEN GRPTYPE = 'E' THEN ISNULL(DEBIT,0) - ISNULL(CREDIT,0) END),0)" & vbCrLf
        'strSql += "    - ISNULL((CASE WHEN GRPTYPE = 'I' THEN ISNULL(CREDIT,0) - ISNULL(DEBIT,0) END),0)),0) "
        'strSql += "    - ISNULL((SELECT SUM(VALUE) FROM " & cnStockDb & "..CLOSINGVAL WHERE COMPANYID = '" & strcompid & "'" & strCostId & "),0) "
        ''strSql += "    - ISNULL((SELECT SUM(AMOUNT) FROM " & cnStockDb & "..OPENWEIGHT WHERE COMPANYID = '" & strcompid & "'" & strCostId & " AND STOCKTYPE='C'),0) "
        'strSql += "    AMOUNT" & vbCrLf
        'strSql += " INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS0" & vbCrLf
        'strSql += " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS1 AS T INNER JOIN " & cnAdminDb & "..ACHEAD AS A" & vbCrLf
        'strSql += " ON T.ACID = A.ACCODE INNER JOIN " & cnAdminDb & "..ACGROUP AS G ON G.ACGRPCODE = A.ACGRPCODE" & vbCrLf
        'strSql += " AND GRPLEDGER IN ('T')" & vbCrLf
        strSql += vbCrLf + "SELECT 'TRADING A/C' PARTICULARS,SUM(AMOUNT) AMOUNT INTO  TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS0 FROM  (  "
        strSql += vbCrLf + "SELECT SUM(CONVERT(NUMERIC(15,2),ISNULL(INCOME,0)))AMOUNT"
        strSql += vbCrLf + "FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE5 WHERE RESULT4 IN ('0','7')"
        strSql += vbCrLf + "UNION ALL"
        strSql += vbCrLf + "SELECT SUM(CONVERT(NUMERIC(15,2),ISNULL(EXPENSES,0)))*-1 AMOUNT"
        strSql += vbCrLf + "FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE5 WHERE RESULT3 IN ('0','7'))X"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " IF EXISTS (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & userId.ToString() & "PROFITLOSS3')" & vbCrLf
        strSql += " DROP TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS3" & vbCrLf
        strSql += " CREATE TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS3" & vbCrLf
        strSql += " (EXPACGROUPNAME1 VARCHAR(250),EXPENSES1 VARCHAR(50),EXPENSES2 VARCHAR(50),EXPENSES VARCHAR(50)" & vbCrLf
        strSql += " ,INCACGROUPNAME1 VARCHAR(250),INCOME1 VARCHAR(50),INCOME2 VARCHAR(50),INCOME VARCHAR(50)" & vbCrLf
        strSql += " ,RESULT INT,SEP INT,EXPACMAINCODE VARCHAR(50),INCACMAINCODE VARCHAR(50),EXPACGROUP VARCHAR(250),INCACGROUP VARCHAR(250)" & vbCrLf
        strSql += " ,EXPACID VARCHAR(50),EXPACNAME VARCHAR(250),EXPACGROUPNAME VARCHAR(250),EXPACMAINGRPNAME VARCHAR(250)" & vbCrLf
        strSql += " ,INCACID VARCHAR(50),INCACNAME VARCHAR(250),INCACGROUPNAME VARCHAR(250),INCACMAINGRPNAME VARCHAR(250),SNO INT IDENTITY(1,1),COLHEAD VARCHAR(2)" & vbCrLf
        strSql += " )" & vbCrLf
        strSql += " " & vbCrLf
        strSql += " IF EXISTS (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & userId.ToString() & "PROFITLOSS4')" & vbCrLf
        strSql += " DROP TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS4" & vbCrLf
        strSql += " CREATE TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS4" & vbCrLf
        strSql += " (EXPACGROUPNAME1 VARCHAR(250),EXPENSES1 VARCHAR(50),EXPENSES2 VARCHAR(50),EXPENSES VARCHAR(50)" & vbCrLf
        strSql += " ,INCACGROUPNAME1 VARCHAR(250),INCOME1 VARCHAR(50),INCOME2 VARCHAR(50),INCOME VARCHAR(50)" & vbCrLf
        strSql += " ,RESULT INT,SEP INT,EXPACMAINCODE VARCHAR(50),INCACMAINCODE VARCHAR(50),EXPACGROUP VARCHAR(250),INCACGROUP VARCHAR(250)" & vbCrLf
        strSql += " ,EXPACID VARCHAR(50),EXPACNAME VARCHAR(250),EXPACGROUPNAME VARCHAR(250),EXPACMAINGRPNAME VARCHAR(250)" & vbCrLf
        strSql += " ,INCACID VARCHAR(50),INCACNAME VARCHAR(250),INCACGROUPNAME VARCHAR(250),INCACMAINGRPNAME VARCHAR(250),SNO INT IDENTITY(1,1),COLHEAD VARCHAR(2)" & vbCrLf
        strSql += " )" & vbCrLf
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()


        If summary Then
            strSql = " INSERT INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS3" & vbCrLf
            strSql += " (EXPACGROUPNAME1,EXPENSES,INCACGROUPNAME1,INCOME,RESULT,SEP,EXPACMAINCODE,INCACMAINCODE,EXPACGROUP,INCACGROUP,EXPACID" & vbCrLf
            strSql += " ,EXPACNAME,EXPACGROUPNAME,EXPACMAINGRPNAME,INCACID,INCACNAME,INCACGROUPNAME,INCACMAINGRPNAME,COLHEAD)" & vbCrLf
            strSql += " SELECT " & vbCrLf
            strSql += "   PARTICULARS EXPACGROUPNAME1" & vbCrLf
            strSql += " ,ABS(AMOUNT) EXPENSES" & vbCrLf
            strSql += " ,NULL INCACGROUPNAME1" & vbCrLf
            strSql += " ,NULL INCOME,'3' RESULT,'2' SEP" & vbCrLf
            strSql += " ,'' EXPACMAINCODE,'' INCACMAINCODE,'' EXPACGROUP,'' INCACGROUP" & vbCrLf
            strSql += " ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,'' EXPACMAINGRPNAME" & vbCrLf
            strSql += " ,''INCACID,''INCACNAME,''INCACGROUPNAME,'' INCACMAINGRPNAME,'T1' COLHEAD" & vbCrLf
            strSql += " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS0 WHERE AMOUNT <= 0" & vbCrLf
            strSql += " UNION ALL" & vbCrLf
            strSql += " SELECT " & vbCrLf
            strSql += " DISTINCT ACGRPNAME EXPACGROUPNAME1" & vbCrLf
            strSql += "  ,CONVERT(VARCHAR,ISNULL(SUM(DEBIT),0) - ISNULL(SUM(CREDIT),0)) EXPENSES" & vbCrLf
            strSql += " ,NULL INCACGROUPNAME1" & vbCrLf
            strSql += " ,NULL INCOME,'3' RESULT,'2' SEP" & vbCrLf
            strSql += " ,ACMAINCODE EXPACMAINCODE,'' INCACMAINCODE,ACGRPCODE EXPACGROUP,'' INCACGROUP" & vbCrLf
            strSql += " ,ACID EXPACID,ACNAME EXPACNAME,ACGRPNAME EXPACGROUPNAME,ACMAINGRPNAME EXPACMAINGRPNAME" & vbCrLf
            strSql += " ,''INCACID,''INCACNAME,''INCACGROUPNAME,'' INCACMAINGRPNAME,'' COLHEAD" & vbCrLf
            strSql += " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS2 WHERE ISNULL(GRPTYPE,'') = 'E'" & vbCrLf
            strSql += " GROUP BY ACMAINGRPNAME,ACGRPNAME,ACMAINCODE,ACGRPCODE,ACID,ACNAME" & vbCrLf
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        Else
            strSql = " INSERT INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS3" & vbCrLf
            strSql += " (EXPACGROUPNAME1,EXPENSES,INCACGROUPNAME1,INCOME,RESULT,SEP,EXPACMAINCODE,INCACMAINCODE,EXPACGROUP,INCACGROUP,EXPACID" & vbCrLf
            strSql += " ,EXPACNAME,EXPACGROUPNAME,EXPACMAINGRPNAME,INCACID,INCACNAME,INCACGROUPNAME,INCACMAINGRPNAME,COLHEAD)" & vbCrLf
            strSql += " SELECT " & vbCrLf
            strSql += "  CASE WHEN AMOUNT < 0 THEN PARTICULARS ELSE NULL END  EXPACGROUPNAME1" & vbCrLf
            strSql += " ,CASE WHEN AMOUNT < 0 THEN AMOUNT ELSE NULL END  EXPENSES" & vbCrLf
            strSql += " ,NULL INCACGROUPNAME1" & vbCrLf
            strSql += " ,NULL INCOME,'3' RESULT,'2' SEP" & vbCrLf
            strSql += " ,'' EXPACMAINCODE,'' INCACMAINCODE,'' EXPACGROUP,'' INCACGROUP" & vbCrLf
            strSql += " ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,'' EXPACMAINGRPNAME" & vbCrLf
            strSql += " ,''INCACID,''INCACNAME,''INCACGROUPNAME,'' INCACMAINGRPNAME,'T1' COLHEAD" & vbCrLf
            strSql += " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS0 " & vbCrLf
            strSql += " UNION ALL" & vbCrLf
            strSql += " SELECT " & vbCrLf
            strSql += " DISTINCT ACGRPNAME EXPACGROUPNAME1" & vbCrLf
            strSql += " ,NULL EXPENSES" & vbCrLf
            strSql += " ,NULL INCACGROUPNAME1" & vbCrLf
            strSql += " ,NULL INCOME,'3' RESULT,'2' SEP" & vbCrLf
            strSql += " ,ACMAINCODE EXPACMAINCODE,'' INCACMAINCODE,ACGRPCODE EXPACGROUP,'' INCACGROUP" & vbCrLf
            strSql += " ,'' EXPACID,'' EXPACNAME,ACGRPNAME EXPACGROUPNAME,ACMAINGRPNAME EXPACMAINGRPNAME" & vbCrLf
            strSql += " ,''INCACID,''INCACNAME,''INCACGROUPNAME,'' INCACMAINGRPNAME,'T1' COLHEAD" & vbCrLf
            strSql += " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS2 WHERE ISNULL(GRPTYPE,'') = 'E'" & vbCrLf
            strSql += " GROUP BY ACMAINGRPNAME,ACGRPNAME,ACMAINCODE,ACGRPCODE" & vbCrLf
            strSql += " UNION ALL" & vbCrLf
            strSql += " SELECT " & vbCrLf
            strSql += " DISTINCT '  '+ACNAME EXPACGROUPNAME1" & vbCrLf
            strSql += " ,CONVERT(VARCHAR,ISNULL(SUM(DEBIT),0) - ISNULL(SUM(CREDIT),0)) EXPENSES" & vbCrLf
            strSql += " ,NULL INCACGROUPNAME1" & vbCrLf
            strSql += " ,NULL INCOME,'4' RESULT,'2' SEP" & vbCrLf
            strSql += " ,ACMAINCODE EXPACMAINCODE,'' INCACMAINCODE,ACGRPCODE EXPACGROUP,'' INCACGROUP" & vbCrLf
            strSql += " ,ACID EXPACID,ACNAME EXPACNAME,ACGRPNAME EXPACGROUPNAME,ACMAINGRPNAME EXPACMAINGRPNAME" & vbCrLf
            strSql += " ,''INCACID,''INCACNAME,''INCACGROUPNAME,'' INCACMAINGRPNAME,'' COLHEAD" & vbCrLf
            strSql += " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS2 WHERE ISNULL(GRPTYPE,'') = 'E'" & vbCrLf
            strSql += " GROUP BY ACMAINGRPNAME,ACGRPNAME,ACMAINCODE,ACGRPCODE,ACID,ACNAME" & vbCrLf
            strSql += " ORDER BY EXPACGROUP,SEP,RESULT,INCACNAME" & vbCrLf
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End If

        If summary Then
            strSql = " INSERT INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS4" & vbCrLf
            strSql += " (EXPACGROUPNAME1,EXPENSES,INCACGROUPNAME1,INCOME,RESULT,SEP,EXPACMAINCODE,INCACMAINCODE,EXPACGROUP,INCACGROUP,EXPACID" & vbCrLf
            strSql += " ,EXPACNAME,EXPACGROUPNAME,EXPACMAINGRPNAME,INCACID,INCACNAME,INCACGROUPNAME,INCACMAINGRPNAME,COLHEAD)" & vbCrLf
            strSql += " SELECT " & vbCrLf
            strSql += "   NULL EXPACGROUPNAME1" & vbCrLf
            strSql += " ,NULL EXPENSES" & vbCrLf
            strSql += " ,PARTICULARS INCACGROUPNAME1" & vbCrLf
            strSql += " ,ABS(AMOUNT) INCOME,'3' RESULT,'2' SEP" & vbCrLf
            strSql += " ,'' EXPACMAINCODE,'' INCACMAINCODE,'' EXPACGROUP,'' INCACGROUP" & vbCrLf
            strSql += " ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,'' EXPACMAINGRPNAME" & vbCrLf
            strSql += " ,''INCACID,''INCACNAME,''INCACGROUPNAME,'' INCACMAINGRPNAME,'T1' COLHEAD" & vbTab + Environment.NewLine
            strSql += " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS0 WHERE AMOUNT > 0" & vbCrLf
            strSql += " UNION ALL" & vbCrLf
            strSql += " SELECT " & vbCrLf
            strSql += " DISTINCT '' EXPACGROUPNAME1" & vbCrLf
            strSql += " ,NULL EXPENSES" & vbCrLf
            strSql += " ,ACGRPNAME INCACGROUPNAME1" & vbCrLf
            strSql += " ,CONVERT(VARCHAR,ISNULL(SUM(CREDIT),0) - ISNULL(SUM(DEBIT),0)) INCOME,'3' RESULT,'2' SEP" & vbCrLf
            strSql += " ,'' EXPACMAINCODE,ACMAINCODE INCACMAINCODE,'' EXPACGROUP,ACGRPCODE INCACGROUP" & vbCrLf
            strSql += " ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,'' EXPACMAINGRPNAME" & vbCrLf
            strSql += " ,ACID INCACID,ACNAME INCACNAME,ACGRPNAME INCACGROUPNAME,ACMAINGRPNAME INCACMAINGRPNAME,'' COLHEAD" & vbTab + Environment.NewLine
            strSql += " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS2 WHERE ISNULL(GRPTYPE,'') = 'I'" & vbCrLf
            strSql += " GROUP BY ACMAINGRPNAME,ACGRPNAME,ACMAINCODE,ACGRPCODE,ACID,ACNAME" & vbCrLf
            strSql += " ORDER BY INCACMAINGRPNAME,SEP,INCACGROUPNAME,RESULT,INCACNAME" & vbCrLf
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        Else
            strSql = " INSERT INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS4" & vbCrLf
            strSql += " (EXPACGROUPNAME1,EXPENSES,INCACGROUPNAME1,INCOME,RESULT,SEP,EXPACMAINCODE,INCACMAINCODE,EXPACGROUP,INCACGROUP,EXPACID" & vbCrLf
            strSql += " ,EXPACNAME,EXPACGROUPNAME,EXPACMAINGRPNAME,INCACID,INCACNAME,INCACGROUPNAME,INCACMAINGRPNAME,COLHEAD)" & vbCrLf
            strSql += " SELECT " & vbCrLf
            strSql += "   NULL EXPACGROUPNAME1" & vbCrLf
            strSql += " ,NULL EXPENSES" & vbCrLf
            'strSql += " ,PARTICULARS INCACGROUPNAME1" & vbCrLf
            'strSql += " ,ABS(AMOUNT) INCOME,'3' RESULT,'2' SEP" & vbCrLf
            strSql += " ,CASE WHEN AMOUNT > 0 THEN PARTICULARS ELSE NULL END INCACGROUPNAME1" & vbCrLf
            strSql += " ,CASE WHEN AMOUNT > 0 THEN AMOUNT ELSE NULL END INCOME,'3' RESULT,'2' SEP" & vbCrLf
            strSql += " ,'' EXPACMAINCODE,'' INCACMAINCODE,'' EXPACGROUP,'' INCACGROUP" & vbCrLf
            strSql += " ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,'' EXPACMAINGRPNAME" & vbCrLf
            strSql += " ,''INCACID,''INCACNAME,''INCACGROUPNAME,'' INCACMAINGRPNAME,'T1' COLHEAD" & vbTab + Environment.NewLine
            strSql += " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS0 " & vbCrLf
            strSql += " UNION ALL" & vbCrLf
            strSql += " SELECT " & vbCrLf
            strSql += " DISTINCT '' EXPACGROUPNAME1" & vbCrLf
            strSql += " ,NULL EXPENSES" & vbCrLf
            strSql += " ,ACGRPNAME INCACGROUPNAME1" & vbCrLf
            strSql += " ,NULL INCOME,'3' RESULT,'2' SEP" & vbCrLf
            strSql += " ,'' EXPACMAINCODE,ACMAINCODE INCACMAINCODE,'' EXPACGROUP,ACGRPCODE INCACGROUP" & vbCrLf
            strSql += " ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,'' EXPACMAINGRPNAME" & vbCrLf
            strSql += " ,'' INCACID,'' INCACNAME,ACGRPNAME INCACGROUPNAME,ACMAINGRPNAME INCACMAINGRPNAME,'T1' COLHEAD" & vbTab + Environment.NewLine
            strSql += " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS2 WHERE ISNULL(GRPTYPE,'') = 'I'" & vbCrLf
            strSql += " GROUP BY ACMAINGRPNAME,ACGRPNAME,ACMAINCODE,ACGRPCODE" & vbCrLf
            strSql += " UNION ALL" & vbCrLf
            strSql += " SELECT " & vbCrLf
            strSql += " DISTINCT '' EXPACGROUPNAME1" & vbCrLf
            strSql += " ,NULL EXPENSES" & vbCrLf
            strSql += " ,ACNAME INCACGROUPNAME1" & vbCrLf
            strSql += " ,CONVERT(VARCHAR,ISNULL(SUM(CREDIT),0) - ISNULL(SUM(DEBIT),0)) INCOME,'3' RESULT,'2' SEP" & vbCrLf
            strSql += " ,'' EXPACMAINCODE,ACMAINCODE INCACMAINCODE,'' EXPACGROUP,ACGRPCODE INCACGROUP" & vbCrLf
            strSql += " ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,'' EXPACMAINGRPNAME" & vbCrLf
            strSql += " ,ACID INCACID,ACNAME INCACNAME,ACGRPNAME INCACGROUPNAME,ACMAINGRPNAME INCACMAINGRPNAME,'' COLHEAD" & vbTab + Environment.NewLine
            strSql += " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS2 WHERE ISNULL(GRPTYPE,'') = 'I'" & vbCrLf
            strSql += " GROUP BY ACMAINGRPNAME,ACGRPNAME,ACMAINCODE,ACGRPCODE,ACID,ACNAME" & vbCrLf
            strSql += " ORDER BY INCACMAINGRPNAME,SEP,INCACGROUPNAME,RESULT,INCACNAME" & vbCrLf
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End If

        strSql = " IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & userId.ToString() & "PROFITLOSS5')" & vbCrLf
        strSql += " DROP TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS5" & vbCrLf
        strSql += " SELECT " & vbCrLf
        strSql += " T3.EXPACGROUPNAME1,T3.EXPENSES,T4.INCACGROUPNAME1,T4.INCOME" & vbCrLf
        strSql += " ,T3.SNO SNO3,T4.SNO SNO4,T3.RESULT RESULT3,T4.RESULT RESULT4,T3.SEP SEP3,T4.SEP SEP4,T3.COLHEAD COLHEAD3,T4.COLHEAD COLHEAD4" & vbCrLf
        strSql += " INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS5" & vbCrLf
        strSql += " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS3 AS T3 FULL JOIN TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS4 AS T4 " & vbCrLf
        strSql += " ON T3.SNO = T4.SNO" & vbCrLf
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " INSERT INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS5"
        strSql += " (EXPACGROUPNAME1,EXPENSES,INCACGROUPNAME1,INCOME,COLHEAD3,COLHEAD4)"
        strSql += " SELECT  CASE WHEN ISNULL(SUM(CONVERT(NUMERIC(15,2),INCOME)),0) - ISNULL(SUM(CONVERT(NUMERIC(15,2),EXPENSES)),0) > 0 THEN 'NET PROFIT' END EXPACGROUPNAME1  "
        strSql += " ,CONVERT(VARCHAR,CASE WHEN ISNULL(SUM(CONVERT(NUMERIC(15,2),INCOME)),0) - ISNULL(SUM(CONVERT(NUMERIC(15,2),EXPENSES)),0) > 0 "
        strSql += " THEN ISNULL(SUM(CONVERT(NUMERIC(15,2),INCOME)),0) - ISNULL(SUM(CONVERT(NUMERIC(15,2),EXPENSES)),0) END) EXPENSES "
        strSql += " ,CASE WHEN ISNULL(SUM(CONVERT(NUMERIC(15,2),EXPENSES)),0) - ISNULL(SUM(CONVERT(NUMERIC(15,2),INCOME)),0) > 0 THEN 'NET LOSS' END INCACGROUPNAME1 "
        strSql += " ,CONVERT(VARCHAR,CASE WHEN ISNULL(SUM(CONVERT(NUMERIC(15,2),EXPENSES)),0) - ISNULL(SUM(CONVERT(NUMERIC(15,2),INCOME)),0) > 0 "
        strSql += " THEN ISNULL(SUM(CONVERT(NUMERIC(15,2),EXPENSES)),0) - ISNULL(SUM(CONVERT(NUMERIC(15,2),INCOME)),0) END) INCOME"
        strSql += " ,'G' COLHEAD3,'G' COLHEAD4 FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS5"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " SELECT " & vbCrLf
        strSql += " DISTINCT 'P & L - 01/" & CnStartMonth & "/" & (IIf(dtpDate.Value.[Date].Month > 3, dtpDate.Value.[Date].Year, dtpDate.Value.[Date].Year - 1)).ToString() & " - " & dtpDate.Text.Replace("-", "/") & "' EXPACGROUPNAME1,' 'EXPENSES1,' ' EXPENSES2,' 'EXPENSES " & vbCrLf
        strSql += " ,' ' INCACGROUPNAME1,' 'INCOME1,' 'INCOME2,' 'INCOME,'T' COLHEAD3,'T' COLHEAD4" & vbCrLf
        strSql += " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS5" & vbCrLf
        strSql += " UNION ALL" & vbCrLf
        strSql += " SELECT " & vbCrLf
        strSql += " EXPACGROUPNAME1,' 'EXPENSES1,' ' EXPENSES2," & cnStockDb & ".[DBO].FUNC_RUPEES_WITHCOMMA(CASE WHEN ISNULL(EXPENSES,'') = '' THEN '0' ELSE EXPENSES END,'D','Y') EXPENSES " & vbCrLf
        strSql += " ,INCACGROUPNAME1,' 'INCOME1,' 'INCOME2," & cnStockDb & ".[DBO].FUNC_RUPEES_WITHCOMMA(CASE WHEN ISNULL(INCOME,'') = '' THEN '0' ELSE INCOME END,'D','Y') INCOME,COLHEAD3,COLHEAD4" & vbCrLf
        strSql += " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS5" & vbCrLf
        strSql += " UNION ALL" & vbCrLf
        strSql += " SELECT" & vbCrLf
        strSql += "  DISTINCT '' EXPACGROUPNAME1 ,'' EXPENSES1,'' EXPENSES2,'-------------------' EXPENSES" & vbCrLf
        strSql += " ,'' INCACGROUPNAME1 ,'' INCOME1,'' INCOME2,'-------------------' INCOME,'G1' COLHEAD3,'G1' COLHEAD4" & vbCrLf
        strSql += " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS5" & vbCrLf
        strSql += " UNION ALL" & vbCrLf
        strSql += " SELECT" & vbCrLf
        strSql += "   '' EXPACGROUPNAME1 ,'' EXPENSES1,'' EXPENSES2,CONVERT(VARCHAR," & cnStockDb & ".[DBO].FUNC_RUPEES_WITHCOMMA(SUM(CONVERT(NUMERIC(15,2),EXPENSES)),'D','Y')) EXPENSES" & vbCrLf
        strSql += " ,'' INCACGROUPNAME1 ,'' INCOME1,'' INCOME2,CONVERT(VARCHAR," & cnStockDb & ".[DBO].FUNC_RUPEES_WITHCOMMA(SUM(CONVERT(NUMERIC(15,2),INCOME)),'D','Y')) INCOME,'G1' COLHEAD3,'G1' COLHEAD4" & vbCrLf
        strSql += " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS5" & vbCrLf
        strSql += " UNION ALL" & vbCrLf
        strSql += " SELECT" & vbCrLf
        strSql += "  DISTINCT '' EXPACGROUPNAME1 ,'' EXPENSES1,'' EXPENSES2,'-------------------' EXPENSES" & vbCrLf
        strSql += " ,'' INCACGROUPNAME1 ,'' INCOME1,'' INCOME2,'-------------------' INCOME,'G1' COLHEAD3,'G1' COLHEAD4" & vbCrLf
        strSql += " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS5" & vbCrLf
        'strSql += " UNION ALL" & vbCrLf
        'strSql += " SELECT" & vbCrLf
        'strSql += "   CASE WHEN ISNULL(SUM(CONVERT(NUMERIC(15,2),EXPENSES)),0) - ISNULL(SUM(CONVERT(NUMERIC(15,2),INCOME)),0) > 0 THEN 'GROSS LOSS' END EXPACGROUPNAME1 ,'' EXPENSES1,'' EXPENSES2" & vbCrLf
        'strSql += " ,CONVERT(VARCHAR,CASE WHEN ISNULL(SUM(CONVERT(NUMERIC(15,2),EXPENSES)),0) - ISNULL(SUM(CONVERT(NUMERIC(15,2),INCOME)),0) > 0 THEN " & cnStockDb & ".[DBO].FUNC_RUPEES_WITHCOMMA(ISNULL(SUM(CONVERT(NUMERIC(15,2),EXPENSES)),0) - ISNULL(SUM(CONVERT(NUMERIC(15,2),INCOME)),0),'D','Y') END) EXPENSES" & vbCrLf
        'strSql += " ,CASE WHEN ISNULL(SUM(CONVERT(NUMERIC(15,2),INCOME)),0) - ISNULL(SUM(CONVERT(NUMERIC(15,2),EXPENSES)),0) > 0 THEN 'GROSS PROFIT' END INCACGROUPNAME1 ,'' INCOME1,'' INCOME2 " & vbCrLf
        'strSql += " ,CONVERT(VARCHAR,CASE WHEN ISNULL(SUM(CONVERT(NUMERIC(15,2),INCOME)),0) - ISNULL(SUM(CONVERT(NUMERIC(15,2),EXPENSES)),0) > 0 THEN " & cnStockDb & ".[DBO].FUNC_RUPEES_WITHCOMMA(ISNULL(SUM(CONVERT(NUMERIC(15,2),INCOME)),0) - ISNULL(SUM(CONVERT(NUMERIC(15,2),EXPENSES)),0),'D','Y') END) INCOME,'G' COLHEAD3,'G' COLHEAD4" & vbCrLf
        'strSql += " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS5" & vbCrLf
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)

    End Sub





    Private Sub funcTradingBetweenDates(ByVal strcompid As String, ByVal strCostId As String, ByVal summary As Boolean, Optional ByVal ForPL As Boolean = False)
        Dim cnttchkk As Integer = 0
        Try
TTTT:

            Dim OPStrNew As String
            Dim OPStartdate As Date
            Dim Fromdate As Date
            Fromdate = dtpDate.Value.Date.AddDays(-1)
            OPStrNew = "SELECT STARTDATE FROM " & cnAdminDb & "..DBMASTER WHERE '" & Format(dtpDate.Value, "yyyy-MM-dd") & "' BETWEEN STARTDATE AND ENDDATE "
            OPStartdate = GetSqlValue(cn, OPStrNew)

            strSql = "  IF EXISTS (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & userId.ToString() & "TRADE1')"
            strSql += vbCrLf + "  DROP TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE1"
            strSql += vbCrLf + "  CREATE TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE1"
            strSql += vbCrLf + "  (COMPANYID VARCHAR(3),ACID VARCHAR(25),TRANMODE VARCHAR(1),AMOUNT NUMERIC(15,2),DEBIT NUMERIC(15,2),CREDIT NUMERIC(15,2)"
            strSql += vbCrLf + "  ,TRANDATE SMALLDATETIME,PAYMODE VARCHAR(20),TRANNO VARCHAR(25),SEP VARCHAR(1))"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            strSql += vbCrLf + " (COMPANYID,COSTID,CATCODE,CLOSING,RATE,VALUE,UPDATED,UPTIME)"
            strSql = "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE1"
            strSql += vbCrLf + "  (COMPANYID,ACID,TRANMODE,AMOUNT,DEBIT,CREDIT,TRANDATE,PAYMODE,TRANNO,SEP)"
            If dtpDate.Value.Date > cnTranFromDate Then
                strSql += vbCrLf + "  SELECT "
                strSql += vbCrLf + "   COMPANYID,ACCODE ACID,CASE WHEN ISNULL(DEBIT,0) > ISNULL(CREDIT,0) THEN 'D' ELSE 'C' END TRANMODE"
                strSql += vbCrLf + "  ,CASE WHEN ISNULL(DEBIT,0) <> 0 THEN DEBIT ELSE CREDIT END AMOUNT"
                strSql += vbCrLf + "  ,DEBIT,CREDIT,NULL TRANDATE,'' PAYMODE,NULL TRANNO,'O' SEP"
                strSql += vbCrLf + "  FROM " & cnStockDb & "..OPENTRAILBALANCE WHERE ISNULL(COMPANYID,'') = '" & strcompid & "'"
                strSql += strCostId
                strSql += vbCrLf + "  UNION ALL"
            End If
            strSql += vbCrLf + "  SELECT "
            strSql += vbCrLf + "   COMPANYID,ACCODE ACID,TRANMODE,AMOUNT"
            strSql += vbCrLf + "  ,CASE WHEN TRANMODE = 'D' THEN AMOUNT END DEBIT,CASE WHEN TRANMODE = 'C' THEN AMOUNT END CREDIT"
            strSql += vbCrLf + "  ,TRANDATE,PAYMODE,TRANNO,'T' SEP"
            strSql += vbCrLf + "  FROM " & cnStockDb & "..ACCTRAN WHERE TRANDATE >= '" & cnTranFromDate & "' "
            strSql += vbCrLf + "  AND TRANDATE <= '" & Format(Fromdate, "yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') != 'Y' "
            strSql += vbCrLf + "  AND ISNULL(COMPANYID,'') = '" & strcompid & "' "
            strSql += strCostId
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()




            strSql = "  IF EXISTS (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & userId.ToString() & "TRADE2')"
            strSql += vbCrLf + "  DROP TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2"


            strSql += vbCrLf + "  SELECT "
            strSql += vbCrLf + "   '' ACID,G.ACGRPCODE,SG.ACGRPNAME ACGRPNAME,G.ACMAINCODE,SG.GRPLEDGER,SG.GRPTYPE"
            'strSql +=VBCRLF + "   A.ACCODE ACID,G.ACGRPCODE,ACGRPNAME ACGRPNAME,ACMAINCODE,G.GRPLEDGER,G.GRPTYPE"
            strSql += vbCrLf + "  ,SUM(DEBIT) DEBIT,SUM(CREDIT) CREDIT"
            strSql += vbCrLf + "  ,'' ACNAME,'' TRANDATE,CASE WHEN SUM(AMOUNT) > 0 THEN 'D' ELSE 'C' END TRANMODE,SUM(AMOUNT) AMOUNT,'' PAYMODE,'' TRANNO"
            strSql += vbCrLf + "  ,(SELECT ACGRPNAME FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPCODE = G.ACMAINCODE) ACMAINGRPNAME"
            strSql += vbCrLf + "  INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2"
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE1 AS T INNER JOIN " & cnAdminDb & "..ACHEAD AS A"
            strSql += vbCrLf + "  ON T.ACID = A.ACCODE INNER JOIN " & cnAdminDb & "..ACGROUP AS G ON G.ACGRPCODE = A.ACGRPCODE"
            strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ACGROUP AS SG ON SG.ACGRPCODE = A.ACGRPCODE"
            strSql += vbCrLf + "  AND (SG.GRPLEDGER IN ('T') OR G.GRPLEDGER IN ('T')) "
            strSql += vbCrLf + "  GROUP BY G.ACGRPCODE,SG.ACGRPNAME,G.ACMAINCODE,SG.GRPLEDGER,SG.GRPTYPE"
            'If rbtCls_Stock_Man.Checked Then  'Val(objGPack.GetSqlValue("SELECT COUNT(*) FROM " & cnStockDb & "..CLOSINGVAL WHERE 1=1 " & strCostId, , , )) > 0 Then
            '    strSql += vbCrLf + " UNION ALL"
            '    strSql += vbCrLf + " SELECT 'CLSTK' ACID,'0' ACGRPCODE,'CLOSING STOCK' ACGRPNAME,'0' ACMAINCODE"
            '    strSql += vbCrLf + " ,'T' GRPLEDGER,CASE WHEN SUM(VALUE) > 0 THEN 'I' ELSE 'E' END GRPTYPE"
            '    strSql += vbCrLf + " ,CASE WHEN SUM(VALUE) < 0 THEN SUM(VALUE)*(-1) END DEBIT"
            '    strSql += vbCrLf + " ,CASE WHEN SUM(VALUE) > 0 THEN SUM(VALUE) END CREDIT"
            '    strSql += vbCrLf + " ,'CLOSING STOCK' ACNAME,'' TRANDATE"
            '    strSql += vbCrLf + " ,CASE WHEN SUM(VALUE) > 0 THEN 'D' ELSE 'C' END TRANMODE,SUM(VALUE) AMOUNT"
            '    strSql += vbCrLf + " ,'' PAYMODE,'' TRANNO,'CLOSING STOCK' ACMAINGRPNAME"
            '    strSql += vbCrLf + " FROM " & cnStockDb & "..CLOSINGVAL WHERE ISNULL(COMPANYID,'') = '" & strcompid & "'"
            '    strSql += strCostId
            '    strSql += vbCrLf + " HAVING SUM(VALUE) <> 0"
            'Else
            '    Dim Qry As String

            '    ProgressBarShow()
            '    If rbtCls_Stock_AvgRate.Checked Then
            '        ProgressBarStep("Closing Stock arrival Weighted avg method", 10)
            '        Qry = " EXEC " & cnAdminDb & "..SP_CLOSING_STOCK"
            '    Else
            '        ProgressBarStep("Closing Stock arrival LIFO method", 10)
            '        Qry = " EXEC " & cnAdminDb & "..SP_CLOSINGSTOCK_LIFO"
            '    End If
            '    Qry += vbCrLf + " @DBNAME = '" & cnStockDb & "'"
            '    Qry += vbCrLf + " ,@ADMINDB = '" & cnAdminDb & "'"
            '    Qry += vbCrLf + " ,@FRMDATE = '" & Format(OPStartdate, "yyyy-MM-dd") & "'"
            '    Qry += vbCrLf + " ,@TODATE = '" & Format(Fromdate, "yyyy-MM-dd") & "'"
            '    Qry += vbCrLf + " ,@METALNAME = 'ALL'"
            '    Qry += vbCrLf + " ,@CATCODE = 'ALL'"
            '    Qry += vbCrLf + " ,@CATNAME = 'ALL'"
            '    Qry += vbCrLf + " ,@COSTNAME = '" & GetQryString(chkCmbCostCentre.Text).Replace("'", "") & "'"
            '    Qry += vbCrLf + " ,@COMPANYID = '" & strcompid & "'"
            '    Qry += vbCrLf + " ,@RPTTYPE = 'S'"
            '    Dim SYSTEMID As String = Trim(LSet(Mid(Guid.NewGuid.ToString, 1, InStr(Guid.NewGuid.ToString, "-") - 1), 10))
            '    Qry += vbCrLf + " ,@SYSTEMID = '" & SYSTEMID & "'"
            '    cmd = New OleDbCommand(Qry, cn)
            '    cmd.CommandTimeout = 2000
            '    cmd.ExecuteNonQuery()

            '    strSql += vbCrLf + " UNION ALL"
            '    strSql += vbCrLf + " SELECT 	'CLSTK' ACID,'0' ACGRPCODE,'CLOSING STOCK' ACGRPNAME,'0' ACMAINCODE ,'T' GRPLEDGER,"
            '    strSql += vbCrLf + " CASE WHEN SUM(CAMOUNT) > 0 THEN 'I' ELSE 'E' END GRPTYPE "
            '    strSql += vbCrLf + " ,CASE WHEN SUM(CAMOUNT) < 0 THEN SUM(CAMOUNT)*(-1) END DEBIT,"
            '    strSql += vbCrLf + " CASE WHEN SUM(CAMOUNT) > 0 THEN SUM(CAMOUNT) END CREDIT "
            '    strSql += vbCrLf + " ,'CLOSING STOCK' ACNAME,'' TRANDATE "
            '    strSql += vbCrLf + " ,CASE WHEN SUM(CAMOUNT) > 0 THEN 'D' ELSE 'C' END TRANMODE"
            '    strSql += vbCrLf + " ,SUM(CAMOUNT) AMOUNT ,'' PAYMODE,'' TRANNO,'CLOSING STOCK' ACMAINGRPNAME "
            '    strSql += vbCrLf + " FROM TEMPTABLEDB..TEMPCSTKVALUE4" & SYSTEMID & " A "
            '    strSql += vbCrLf + " WHERE A.RESULT=1"
            '    strSql += vbCrLf + " HAVING SUM(CAMOUNT) <> 0"
            'End If

            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + " SELECT SUM(CASE WHEN GRPTYPE='I' THEN AMOUNT ELSE -1*AMOUNT END)TRADING FROM ( "
            strSql += vbCrLf + " SELECT ACGRPNAME,GRPTYPE,(CASE WHEN ISNULL(CREDIT,0)>ISNULL(DEBIT,0) THEN ISNULL(CREDIT,0)-ISNULL(DEBIT,0) ELSE ISNULL(DEBIT,0)-ISNULL(CREDIT,0) END)AMOUNT "
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2)X "
            Dim OpenTrading As Double = 0
            OpenTrading = Math.Round(Val(GetSqlValue(cn, strSql).ToString), 2)






            Dim StrNew As String
            Dim Startdate As Date
            StrNew = "SELECT STARTDATE FROM " & cnAdminDb & "..DBMASTER WHERE '" & Format(dtpDate.Value, "yyyy-MM-dd") & "' BETWEEN STARTDATE AND ENDDATE "
            Startdate = GetSqlValue(cn, StrNew)

            strSql = "  IF EXISTS (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & userId.ToString() & "TRADE1')"
            strSql += vbCrLf + "  DROP TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE1"
            strSql += vbCrLf + "  CREATE TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE1"
            strSql += vbCrLf + "  (COMPANYID VARCHAR(3),ACID VARCHAR(25),TRANMODE VARCHAR(1),AMOUNT NUMERIC(15,2),DEBIT NUMERIC(15,2),CREDIT NUMERIC(15,2)"
            strSql += vbCrLf + "  ,TRANDATE SMALLDATETIME,PAYMODE VARCHAR(20),TRANNO VARCHAR(25),SEP VARCHAR(1))"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            strSql += vbCrLf + " (COMPANYID,COSTID,CATCODE,CLOSING,RATE,VALUE,UPDATED,UPTIME)"
            strSql = "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE1"
            strSql += vbCrLf + "  (COMPANYID,ACID,TRANMODE,AMOUNT,DEBIT,CREDIT,TRANDATE,PAYMODE,TRANNO,SEP)"
            If dtpDate.Value.Date = Startdate Then
                strSql += vbCrLf + "  SELECT "
                strSql += vbCrLf + "   COMPANYID,ACCODE ACID,CASE WHEN ISNULL(DEBIT,0) > ISNULL(CREDIT,0) THEN 'D' ELSE 'C' END TRANMODE"
                strSql += vbCrLf + "  ,CASE WHEN ISNULL(DEBIT,0) <> 0 THEN DEBIT ELSE CREDIT END AMOUNT"
                strSql += vbCrLf + "  ,DEBIT,CREDIT,'" & Format(Startdate, "yyyy-MM-dd") & "' TRANDATE,'' PAYMODE,NULL TRANNO,'O' SEP"
                strSql += vbCrLf + "  FROM " & cnStockDb & "..OPENTRAILBALANCE WHERE ISNULL(COMPANYID,'') = '" & strcompid & "'"
                strSql += strCostId
                strSql += vbCrLf + "  UNION ALL"
            End If
            strSql += vbCrLf + "  SELECT "
            strSql += vbCrLf + "   COMPANYID,ACCODE ACID,TRANMODE,AMOUNT"
            strSql += vbCrLf + "  ,CASE WHEN TRANMODE = 'D' THEN AMOUNT END DEBIT,CASE WHEN TRANMODE = 'C' THEN AMOUNT END CREDIT"
            strSql += vbCrLf + "  ,TRANDATE,PAYMODE,TRANNO,'T' SEP"
            strSql += vbCrLf + "  FROM " & cnStockDb & "..ACCTRAN WHERE TRANDATE >= '" & cnTranFromDate & "' "
            strSql += vbCrLf + "  AND TRANDATE BETWEEN '" & dtpDate.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpToDate.Value.Date.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') != 'Y' "
            strSql += vbCrLf + "  AND ISNULL(COMPANYID,'') = '" & strcompid & "' "
            strSql += strCostId
            'strSql +=VBCRLF + " UNION ALL"
            'strSql +=VBCRLF + " (COMPANYID,COSTID,CATCODE,CLOSING,RATE,VALUE,UPDATED,UPTIME)"
            'strSql +=VBCRLF + " SELECT"
            'strSql +=VBCRLF + "     COMPANYID,'CL"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()



            strSql = "  IF EXISTS (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & userId.ToString() & "TRADE2')"
            strSql += vbCrLf + "  DROP TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2"
            If summary Then
                strSql += vbCrLf + "  SELECT "
                strSql += vbCrLf + "   '' ACID,G.ACGRPCODE,SG.ACGRPNAME ACGRPNAME,G.ACMAINCODE,SG.GRPLEDGER,SG.GRPTYPE"
                'strSql +=VBCRLF + "   A.ACCODE ACID,G.ACGRPCODE,ACGRPNAME ACGRPNAME,ACMAINCODE,G.GRPLEDGER,G.GRPTYPE"
                strSql += vbCrLf + "  ,SUM(DEBIT) DEBIT,SUM(CREDIT) CREDIT"
                strSql += vbCrLf + "  ,'' ACNAME,'' TRANDATE,CASE WHEN SUM(AMOUNT) > 0 THEN 'D' ELSE 'C' END TRANMODE,SUM(AMOUNT) AMOUNT,'' PAYMODE,'' TRANNO"
                strSql += vbCrLf + "  ,(SELECT ACGRPNAME FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPCODE = G.ACMAINCODE) ACMAINGRPNAME"
                strSql += vbCrLf + "  INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2"
                strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE1 AS T INNER JOIN " & cnAdminDb & "..ACHEAD AS A"
                strSql += vbCrLf + "  ON T.ACID = A.ACCODE INNER JOIN " & cnAdminDb & "..ACGROUP AS G ON G.ACGRPCODE = A.ACGRPCODE"
                strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ACGROUP AS SG ON SG.ACGRPCODE = A.ACGRPCODE"
                strSql += vbCrLf + "  AND (SG.GRPLEDGER IN ('T') OR G.GRPLEDGER IN ('T')) "
                strSql += vbCrLf + "  GROUP BY G.ACGRPCODE,SG.ACGRPNAME,G.ACMAINCODE,SG.GRPLEDGER,SG.GRPTYPE"
                If rbtCls_Stock_Man.Checked Then  'Val(objGPack.GetSqlValue("SELECT COUNT(*) FROM " & cnStockDb & "..CLOSINGVAL WHERE 1=1 " & strCostId, , , )) > 0 Then
                    strSql += vbCrLf + " UNION ALL"
                    strSql += vbCrLf + " SELECT 'CLSTK' ACID,'0' ACGRPCODE,'CLOSING STOCK' ACGRPNAME,'0' ACMAINCODE"
                    strSql += vbCrLf + " ,'T' GRPLEDGER,CASE WHEN SUM(VALUE) > 0 THEN 'I' ELSE 'E' END GRPTYPE"
                    strSql += vbCrLf + " ,CASE WHEN SUM(VALUE) < 0 THEN SUM(VALUE)*(-1) END DEBIT"
                    strSql += vbCrLf + " ,CASE WHEN SUM(VALUE) > 0 THEN SUM(VALUE) END CREDIT"
                    strSql += vbCrLf + " ,'CLOSING STOCK' ACNAME,'' TRANDATE"
                    strSql += vbCrLf + " ,CASE WHEN SUM(VALUE) > 0 THEN 'D' ELSE 'C' END TRANMODE,SUM(VALUE) AMOUNT"
                    strSql += vbCrLf + " ,'' PAYMODE,'' TRANNO,'CLOSING STOCK' ACMAINGRPNAME"
                    strSql += vbCrLf + " FROM " & cnStockDb & "..CLOSINGVAL WHERE ISNULL(COMPANYID,'') = '" & strcompid & "'"
                    strSql += strCostId
                    strSql += vbCrLf + " HAVING SUM(VALUE) <> 0"

                    strSql += vbCrLf + " UNION ALL"
                    strSql += vbCrLf + " SELECT 	'OPENNING' ACID,'-1' ACGRPCODE,'OPENNING BALANCE' ACGRPNAME,'0' ACMAINCODE ,'T' GRPLEDGER,"
                    strSql += vbCrLf + " CASE WHEN ISNULL(" & Val(OpenTrading.ToString).ToString & ",0) > 0 THEN 'I' ELSE 'E' END GRPTYPE "
                    strSql += vbCrLf + " ,CASE WHEN ISNULL(" & Val(OpenTrading.ToString).ToString & ",0) < 0 THEN ISNULL(" & Val(OpenTrading.ToString).ToString & ",0)*(-1) END DEBIT,"
                    strSql += vbCrLf + " CASE WHEN ISNULL(" & Val(OpenTrading.ToString).ToString & ",0) > 0 THEN ISNULL(" & Val(OpenTrading.ToString).ToString & ",0) END CREDIT "
                    strSql += vbCrLf + " ,'OPENNING BALANCE' ACNAME,'' TRANDATE "
                    strSql += vbCrLf + " ,CASE WHEN ISNULL(" & Val(OpenTrading.ToString).ToString & ",0) > 0 THEN 'D' ELSE 'C' END TRANMODE"
                    strSql += vbCrLf + " ,ISNULL(" & Val(OpenTrading.ToString).ToString & ",0) AMOUNT ,'' PAYMODE,'' TRANNO,'OPENNING BALANCE' ACMAINGRPNAME "

                    'strSql += vbCrLf + " UNION ALL"
                    'strSql += vbCrLf + " SELECT 'OPSTK' ACID,'0' ACGRPCODE,'OPENING STOCK' ACGRPNAME,'0' ACMAINCODE"
                    'strSql += vbCrLf + " ,'T' GRPLEDGER"
                    'strSql += vbCrLf + " ,CASE WHEN SUM(AMOUNT) < 0 THEN 'I' ELSE 'E' END GRPTYPE"
                    'strSql += vbCrLf + " ,CASE WHEN SUM(AMOUNT) > 0 THEN SUM(AMOUNT) END CREDIT"
                    'strSql += vbCrLf + " ,CASE WHEN SUM(AMOUNT) < 0 THEN SUM(AMOUNT)*(-1) END DEBIT"
                    'strSql += vbCrLf + " ,'OPENING STOCK' ACNAME,'' TRANDATE"
                    'strSql += vbCrLf + " ,CASE WHEN SUM(AMOUNT) > 0 THEN 'D' ELSE 'C' END TRANMODE"
                    'strSql += vbCrLf + " ,SUM(AMOUNT) AMOUNT"
                    'strSql += vbCrLf + " ,'' PAYMODE,'' TRANNO,'OPENING STOCK' ACMAINGRPNAME"
                    'strSql += vbCrLf + " FROM " & cnStockDb & "..OPENWEIGHT WHERE ISNULL(COMPANYID,'') = '" & strcompid & "'"
                    'strSql += strCostId
                    'strSql += vbCrLf + " HAVING SUM(AMOUNT) <> 0"
                Else
                    Dim Qry As String

                    ProgressBarShow()
                    If rbtCls_Stock_AvgRate.Checked Then
                        ProgressBarStep("Closing Stock arrival Weighted avg method", 10)
                        Qry = " EXEC " & cnAdminDb & "..SP_CLOSING_STOCK"
                    Else
                        ProgressBarStep("Closing Stock arrival LIFO method", 10)
                        Qry = " EXEC " & cnAdminDb & "..SP_CLOSINGSTOCK_LIFO"
                    End If
                    Qry += vbCrLf + " @DBNAME = '" & cnStockDb & "'"
                    Qry += vbCrLf + " ,@ADMINDB = '" & cnAdminDb & "'"
                    Qry += vbCrLf + " ,@FRMDATE = '" & Format(cnTranFromDate, "yyyy-MM-dd") & "'"
                    Qry += vbCrLf + " ,@TODATE = '" & Format(dtpToDate.Value, "yyyy-MM-dd") & "'"
                    Qry += vbCrLf + " ,@METALNAME = 'ALL'"
                    Qry += vbCrLf + " ,@CATCODE = 'ALL'"
                    Qry += vbCrLf + " ,@CATNAME = 'ALL'"
                    Qry += vbCrLf + " ,@COSTNAME = '" & GetQryString(chkCmbCostCentre.Text).Replace("'", "") & "'"
                    Qry += vbCrLf + " ,@COMPANYID = '" & strcompid & "'"
                    Qry += vbCrLf + " ,@RPTTYPE = 'S'"
                    Dim SYSTEMID As String = Trim(LSet(Mid(Guid.NewGuid.ToString, 1, InStr(Guid.NewGuid.ToString, "-") - 1), 10))
                    Qry += vbCrLf + " ,@SYSTEMID = '" & SYSTEMID & "'"
                    cmd = New OleDbCommand(Qry, cn)
                    cmd.CommandTimeout = 2000
                    cmd.ExecuteNonQuery()

                    strSql += vbCrLf + " UNION ALL"
                    strSql += vbCrLf + " SELECT 	'CLSTK' ACID,'0' ACGRPCODE,'CLOSING STOCK' ACGRPNAME,'0' ACMAINCODE ,'T' GRPLEDGER,"
                    strSql += vbCrLf + " CASE WHEN SUM(CAMOUNT) > 0 THEN 'I' ELSE 'E' END GRPTYPE "
                    strSql += vbCrLf + " ,CASE WHEN SUM(CAMOUNT) < 0 THEN SUM(CAMOUNT)*(-1) END DEBIT,"
                    strSql += vbCrLf + " CASE WHEN SUM(CAMOUNT) > 0 THEN SUM(CAMOUNT) END CREDIT "
                    strSql += vbCrLf + " ,'CLOSING STOCK' ACNAME,'' TRANDATE "
                    strSql += vbCrLf + " ,CASE WHEN SUM(CAMOUNT) > 0 THEN 'D' ELSE 'C' END TRANMODE"
                    strSql += vbCrLf + " ,SUM(CAMOUNT) AMOUNT ,'' PAYMODE,'' TRANNO,'CLOSING STOCK' ACMAINGRPNAME "
                    strSql += vbCrLf + " FROM TEMPTABLEDB..TEMPCSTKVALUE4" & SYSTEMID & " A "
                    strSql += vbCrLf + " WHERE A.RESULT=1"
                    strSql += vbCrLf + " HAVING SUM(CAMOUNT) <> 0"

                    strSql += vbCrLf + " UNION ALL"
                    strSql += vbCrLf + " SELECT 	'OPENNING' ACID,'-1' ACGRPCODE,'OPENNING BALANCE' ACGRPNAME,'0' ACMAINCODE ,'T' GRPLEDGER,"
                    strSql += vbCrLf + " CASE WHEN ISNULL(" & Val(OpenTrading.ToString).ToString & ",0) > 0 THEN 'I' ELSE 'E' END GRPTYPE "
                    strSql += vbCrLf + " ,CASE WHEN ISNULL(" & Val(OpenTrading.ToString).ToString & ",0) < 0 THEN ISNULL(" & Val(OpenTrading.ToString).ToString & ",0)*(-1) END DEBIT,"
                    strSql += vbCrLf + " CASE WHEN ISNULL(" & Val(OpenTrading.ToString).ToString & ",0) > 0 THEN ISNULL(" & Val(OpenTrading.ToString).ToString & ",0) END CREDIT "
                    strSql += vbCrLf + " ,'OPENNING BALANCE' ACNAME,'' TRANDATE "
                    strSql += vbCrLf + " ,CASE WHEN ISNULL(" & Val(OpenTrading.ToString).ToString & ",0) > 0 THEN 'D' ELSE 'C' END TRANMODE"
                    strSql += vbCrLf + " ,ISNULL(" & Val(OpenTrading.ToString).ToString & ",0) AMOUNT ,'' PAYMODE,'' TRANNO,'OPENNING BALANCE' ACMAINGRPNAME "

                End If
            Else
                strSql += vbCrLf + "  SELECT "
                strSql += vbCrLf + "   A.ACCODE ACID,G.ACGRPCODE,SG.ACGRPNAME ACGRPNAME,G.ACMAINCODE,SG.GRPLEDGER,SG.GRPTYPE"
                'strSql +=VBCRLF + "   A.ACCODE ACID,G.ACGRPCODE,ACGRPNAME ACGRPNAME,ACMAINCODE,G.GRPLEDGER,G.GRPTYPE"
                strSql += vbCrLf + "  ,DEBIT,CREDIT"
                strSql += vbCrLf + "  ,ACNAME ACNAME,TRANDATE,TRANMODE,AMOUNT,PAYMODE,TRANNO"
                strSql += vbCrLf + "  ,(SELECT ACGRPNAME FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPCODE = G.ACMAINCODE) ACMAINGRPNAME"
                strSql += vbCrLf + "  INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2"
                strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE1 AS T INNER JOIN " & cnAdminDb & "..ACHEAD AS A"
                strSql += vbCrLf + "  ON T.ACID = A.ACCODE INNER JOIN " & cnAdminDb & "..ACGROUP AS G ON G.ACGRPCODE = A.ACGRPCODE"
                strSql += vbCrLf + "  left JOIN " & cnAdminDb & "..ACGROUP AS SG ON SG.ACGRPCODE = A.ACGRPCODE"
                strSql += vbCrLf + "  WHERE (SG.GRPLEDGER IN ('T') OR G.GRPLEDGER IN ('T'))"
                If rbtCls_Stock_Man.Checked Then 'Val(objGPack.GetSqlValue("SELECT COUNT(*) FROM " & cnStockDb & "..CLOSINGVAL WHERE 1=1 " & strCostId, , , )) > 0 Then
                    strSql += vbCrLf + " UNION ALL"
                    strSql += vbCrLf + " SELECT 	C.CLSCODE ACID,H.ACGRPCODE,G.acgrpname ACGRPNAME,'0' ACMAINCODE 	,'T' GRPLEDGER,"
                    strSql += vbCrLf + " CASE WHEN SUM(VALUE) > 0 THEN 'I' ELSE 'E' END GRPTYPE 	,CASE WHEN SUM(VALUE) < 0 THEN SUM(VALUE)*(-1) END DEBIT 	,"
                    strSql += vbCrLf + " CASE WHEN SUM(VALUE) > 0 THEN SUM(VALUE) END CREDIT 	,H.ACNAME  ACNAME,'' TRANDATE 	,CASE WHEN SUM(VALUE) > 0 THEN 'D' ELSE 'C' END TRANMODE,SUM(VALUE) AMOUNT 	,'' PAYMODE,'' TRANNO,'CLOSING STOCK' ACMAINGRPNAME "
                    strSql += vbCrLf + " FROM " & cnStockDb & "..CLOSINGVAL A LEFT JOIN " & cnAdminDb & "..CATEGORY C ON A.CATCODE = C.CATCODE LEFT JOIN " & cnAdminDb & "..ACHEAD H ON C.CLSCODE = H.ACCODE "
                    strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..acgroup G ON H.ACGRPCODE = G.ACGRPCODE"
                    strSql += vbCrLf + " WHERE ISNULL(A.COMPANYID,'') = '" & strcompid & "'"
                    strSql += Replace(strCostId, "AND COSTID", "AND A.COSTID") + " GROUP BY H.ACGRPCODE,C.CLSCODE,H.ACNAME,G.ACGRPNAME"
                    strSql += vbCrLf + " HAVING SUM(VALUE) <> 0"

                    strSql += vbCrLf + " UNION ALL"
                    strSql += vbCrLf + " SELECT 	'OPENNING' ACID,'-1' ACGRPCODE,'OPENNING BALANCE' ACGRPNAME,'0' ACMAINCODE ,'T' GRPLEDGER,"
                    strSql += vbCrLf + " CASE WHEN ISNULL(" & Val(OpenTrading.ToString).ToString & ",0) > 0 THEN 'I' ELSE 'E' END GRPTYPE "
                    strSql += vbCrLf + " ,CASE WHEN ISNULL(" & Val(OpenTrading.ToString).ToString & ",0) < 0 THEN ISNULL(" & Val(OpenTrading.ToString).ToString & ",0)*(-1) END DEBIT,"
                    strSql += vbCrLf + " CASE WHEN ISNULL(" & Val(OpenTrading.ToString).ToString & ",0) > 0 THEN ISNULL(" & Val(OpenTrading.ToString).ToString & ",0) END CREDIT "
                    strSql += vbCrLf + " ,'OPENNING BALANCE' ACNAME,'' TRANDATE "
                    strSql += vbCrLf + " ,CASE WHEN ISNULL(" & Val(OpenTrading.ToString).ToString & ",0) > 0 THEN 'D' ELSE 'C' END TRANMODE"
                    strSql += vbCrLf + " ,ISNULL(" & Val(OpenTrading.ToString).ToString & ",0) AMOUNT ,'' PAYMODE,'' TRANNO,'OPENNING BALANCE' ACMAINGRPNAME "

                    'strSql +=VBCRLF + " UNION ALL"
                    'strSql +=VBCRLF + " SELECT 	C.OPNCODE ACID,H.ACGRPCODE,G.ACGRPNAME ACGRPNAME,'0' ACMAINCODE 	,'T' GRPLEDGER,"
                    'strSql +=VBCRLF + " CASE WHEN SUM(AMOUNT) < 0 THEN 'I' ELSE 'E' END GRPTYPE 	"
                    'strSql +=VBCRLF + " ,CASE WHEN SUM(AMOUNT) > 0 THEN SUM(AMOUNT) END CREDIT 	"
                    'strSql +=VBCRLF + " ,CASE WHEN SUM(AMOUNT) < 0 THEN SUM(AMOUNT)*(-1) END DEBIT 	"
                    'strSql +=VBCRLF + " ,H.ACNAME  ACNAME,'' TRANDATE 	"
                    'strSql +=VBCRLF + " ,CASE WHEN SUM(AMOUNT) > 0 THEN 'D' ELSE 'C' END TRANMODE"
                    'strSql +=VBCRLF + " ,SUM(AMOUNT) AMOUNT 	,'' PAYMODE,'' TRANNO,'OPENING STOCK' ACMAINGRPNAME "
                    'strSql +=VBCRLF + " FROM " & cnStockDb & "..OPENWEIGHT A "
                    'strSql +=VBCRLF + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON A.CATCODE = C.CATCODE "
                    'strSql +=VBCRLF + " LEFT JOIN " & cnAdminDb & "..ACHEAD H ON C.OPNCODE = H.ACCODE "
                    'strSql +=VBCRLF + " LEFT JOIN " & cnAdminDb & "..ACGROUP G ON H.ACGRPCODE = G.ACGRPCODE"
                    'strSql +=VBCRLF + " WHERE ISNULL(A.COMPANYID,'') = '" & strcompid & "'"
                    'strSql +=VBCRLF + " AND A.STOCKTYPE='C'"
                    'strSql += Replace(strCostId, "AND COSTID", "AND A.COSTID") + " GROUP BY H.ACGRPCODE,C.OPNCODE,H.ACNAME,G.ACGRPNAME"
                    'strSql +=VBCRLF + " HAVING SUM(AMOUNT) <> 0"
                Else
                    ProgressBarShow()
                    Dim Qry As String = ""

                    If rbtCls_Stock_AvgRate.Checked Then
                        ProgressBarStep("Closing Stock arrival Weighted avg method", 10)
                        Qry = " EXEC " & cnAdminDb & "..SP_CLOSING_STOCK"
                    Else
                        ProgressBarStep("Closing Stock arrival LIFO method", 10)
                        Qry = " EXEC " & cnAdminDb & "..SP_CLOSINGSTOCK_LIFO"
                    End If
                    Qry += vbCrLf + " @DBNAME = '" & cnStockDb & "'"
                    Qry += vbCrLf + " ,@ADMINDB = '" & cnAdminDb & "'"
                    Qry += vbCrLf + " ,@FRMDATE = '" & Format(cnTranFromDate, "yyyy-MM-dd") & "'"
                    Qry += vbCrLf + " ,@TODATE = '" & Format(dtpToDate.Value, "yyyy-MM-dd") & "'"
                    Qry += vbCrLf + " ,@METALNAME = 'ALL'"
                    Qry += vbCrLf + " ,@CATCODE = 'ALL'"
                    Qry += vbCrLf + " ,@CATNAME = 'ALL'"
                    Qry += vbCrLf + " ,@COSTNAME = '" & GetQryString(chkCmbCostCentre.Text).Replace("'", "") & "'"
                    Qry += vbCrLf + " ,@COMPANYID = '" & strcompid & "'"
                    Qry += vbCrLf + " ,@RPTTYPE = 'S'"
                    Dim SYSTEMID As String = Trim(LSet(Mid(Guid.NewGuid.ToString, 1, InStr(Guid.NewGuid.ToString, "-") - 1), 10))
                    Qry += vbCrLf + " ,@SYSTEMID = '" & SYSTEMID & "'"
                    cmd = New OleDbCommand(Qry, cn)
                    cmd.CommandTimeout = 2000
                    cmd.ExecuteNonQuery()


                    'strSql += vbCrLf + " UNION ALL"
                    'strSql += vbCrLf + " SELECT 	C.CLSCODE ACID,H.ACGRPCODE,G.ACGRPNAME ACGRPNAME,'0' ACMAINCODE ,'T' GRPLEDGER,"
                    'strSql += vbCrLf + " CASE WHEN SUM(CAMOUNT) > 0 THEN 'I' ELSE 'E' END GRPTYPE ,CASE WHEN SUM(CAMOUNT) < 0 THEN SUM(CAMOUNT)*(-1) END DEBIT 	,"
                    'strSql += vbCrLf + " CASE WHEN SUM(CAMOUNT) > 0 THEN SUM(CAMOUNT) END CREDIT ,H.ACNAME ACNAME,'' TRANDATE ,CASE WHEN SUM(CAMOUNT) > 0 THEN 'D' ELSE 'C' END TRANMODE,SUM(CAMOUNT) AMOUNT ,'' PAYMODE,'' TRANNO,'CLOSING STOCK' ACMAINGRPNAME "
                    'strSql += vbCrLf + " FROM TEMPTABLEDB..TEMPCSTKVALUE4" & SYSTEMID & " A "
                    'strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON A.CATEGORY = C.CATNAME "
                    'strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ACHEAD H ON C.CLSCODE = H.ACCODE "
                    'strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ACGROUP G ON H.ACGRPCODE = G.ACGRPCODE"
                    'strSql += vbCrLf + " WHERE A.RESULT=1"
                    'strSql += vbCrLf + " GROUP BY H.ACGRPCODE,C.CLSCODE,H.ACNAME,G.ACGRPNAME"
                    'strSql += vbCrLf + " HAVING SUM(CAMOUNT) <> 0"

                    strSql += vbCrLf + " UNION ALL"
                    strSql += vbCrLf + " SELECT 	C.CLSCODE ACID,H.ACGRPCODE,G.ACGRPNAME ACGRPNAME,'0' ACMAINCODE ,'T' GRPLEDGER"
                    strSql += vbCrLf + " ,'I' GRPTYPE ,0 DEBIT"
                    strSql += vbCrLf + " ,SUM(CAMOUNT) CREDIT ,H.ACNAME ACNAME,'' TRANDATE ,CASE WHEN SUM(CAMOUNT) > 0 THEN 'D' ELSE 'C' END TRANMODE,SUM(CAMOUNT) AMOUNT ,'' PAYMODE,'' TRANNO,'CLOSING STOCK' ACMAINGRPNAME "
                    strSql += vbCrLf + " FROM TEMPTABLEDB..TEMPCSTKVALUE4" & SYSTEMID & " A "
                    strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON A.CATEGORY = C.CATNAME "
                    strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ACHEAD H ON C.CLSCODE = H.ACCODE "
                    strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ACGROUP G ON H.ACGRPCODE = G.ACGRPCODE"
                    strSql += vbCrLf + " WHERE A.RESULT=1"
                    strSql += vbCrLf + " GROUP BY H.ACGRPCODE,C.CLSCODE,H.ACNAME,G.ACGRPNAME"
                    strSql += vbCrLf + " HAVING SUM(CAMOUNT) <> 0"

                    strSql += vbCrLf + " UNION ALL"
                    strSql += vbCrLf + " SELECT 	'OPENNING' ACID,'-1' ACGRPCODE,'OPENNING BALANCE' ACGRPNAME,'0' ACMAINCODE ,'T' GRPLEDGER,"
                    strSql += vbCrLf + " CASE WHEN ISNULL(" & Val(OpenTrading.ToString).ToString & ",0) > 0 THEN 'I' ELSE 'E' END GRPTYPE "
                    strSql += vbCrLf + " ,CASE WHEN ISNULL(" & Val(OpenTrading.ToString).ToString & ",0) < 0 THEN ISNULL(" & Val(OpenTrading.ToString).ToString & ",0)*(-1) END DEBIT,"
                    strSql += vbCrLf + " CASE WHEN ISNULL(" & Val(OpenTrading.ToString).ToString & ",0) > 0 THEN ISNULL(" & Val(OpenTrading.ToString).ToString & ",0) END CREDIT "
                    strSql += vbCrLf + " ,'OPENNING BALANCE' ACNAME,'' TRANDATE "
                    strSql += vbCrLf + " ,CASE WHEN ISNULL(" & Val(OpenTrading.ToString).ToString & ",0) > 0 THEN 'D' ELSE 'C' END TRANMODE"
                    strSql += vbCrLf + " ,ISNULL(" & Val(OpenTrading.ToString).ToString & ",0) AMOUNT ,'' PAYMODE,'' TRANNO,'OPENNING BALANCE' ACMAINGRPNAME "

                End If
            End If

            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()


            strSql = " UPDATE A SET ACGRPCODE=(SELECT TOP 1 ACGRPCODE FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPNAME=A.ACMAINGRPNAME) FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 AS A WHERE ISNULL(ACGRPCODE,0)=0"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()


            strSql = " UPDATE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 SET ACMAINCODE=ACGRPCODE WHERE ISNULL(ACMAINCODE, 0)=0"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            ProgressBarHide()




            strSql = "  IF EXISTS (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & userId.ToString() & "TRADE3')"
            strSql += vbCrLf + "  DROP TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE3"
            strSql += vbCrLf + "  CREATE TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE3"
            strSql += vbCrLf + "  (EXPACGROUPNAME1 VARCHAR(250),EXPENSES1 VARCHAR(50),EXPENSES2 VARCHAR(50),EXPENSES VARCHAR(50)"
            strSql += vbCrLf + "  ,INCACGROUPNAME1 VARCHAR(250),INCOME1 VARCHAR(50),INCOME2 VARCHAR(50),INCOME VARCHAR(50)"
            strSql += vbCrLf + "  ,RESULT INT,SEP INT,EXPACMAINCODE VARCHAR(50),INCACMAINCODE VARCHAR(50),EXPACGROUP VARCHAR(250),INCACGROUP VARCHAR(250)"
            strSql += vbCrLf + "  ,EXPACID VARCHAR(50),EXPACNAME VARCHAR(250),EXPACGROUPNAME VARCHAR(250),EXPACMAINGRPNAME VARCHAR(250)"
            strSql += vbCrLf + "  ,INCACID VARCHAR(50),INCACNAME VARCHAR(250),INCACGROUPNAME VARCHAR(250),INCACMAINGRPNAME VARCHAR(250),SNO INT IDENTITY(1,1),COLHEAD VARCHAR(2)"
            strSql += vbCrLf + "  )"
            strSql += vbCrLf + "  "
            strSql += vbCrLf + "  IF EXISTS (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & userId.ToString() & "TRADE4')"
            strSql += vbCrLf + "  DROP TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE4"
            strSql += vbCrLf + "  CREATE TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE4"
            strSql += vbCrLf + "  (EXPACGROUPNAME1 VARCHAR(250),EXPENSES1 VARCHAR(50),EXPENSES2 VARCHAR(50),EXPENSES VARCHAR(50)"
            strSql += vbCrLf + "  ,INCACGROUPNAME1 VARCHAR(250),INCOME1 VARCHAR(50),INCOME2 VARCHAR(50),INCOME VARCHAR(50)"
            strSql += vbCrLf + "  ,RESULT INT,SEP INT,EXPACMAINCODE VARCHAR(50),INCACMAINCODE VARCHAR(50),EXPACGROUP VARCHAR(250),INCACGROUP VARCHAR(250)"
            strSql += vbCrLf + "  ,EXPACID VARCHAR(50),EXPACNAME VARCHAR(250),EXPACGROUPNAME VARCHAR(250),EXPACMAINGRPNAME VARCHAR(250)"
            strSql += vbCrLf + "  ,INCACID VARCHAR(50),INCACNAME VARCHAR(250),INCACGROUPNAME VARCHAR(250),INCACMAINGRPNAME VARCHAR(250),SNO INT IDENTITY(1,1),COLHEAD VARCHAR(2)"
            strSql += vbCrLf + "  )"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE3"
            strSql += vbCrLf + "  (EXPACGROUPNAME1,EXPENSES1,EXPENSES2,EXPENSES,INCACGROUPNAME1,INCOME1,INCOME2,INCOME,RESULT,SEP,EXPACMAINCODE,INCACMAINCODE,EXPACGROUP,INCACGROUP,EXPACID"
            strSql += vbCrLf + "  ,EXPACNAME,EXPACGROUPNAME,EXPACMAINGRPNAME,INCACID,INCACNAME,INCACGROUPNAME,INCACMAINGRPNAME,COLHEAD)"
            strSql += vbCrLf + "  SELECT "
            strSql += vbCrLf + "  DISTINCT ACMAINGRPNAME EXPACGROUPNAME1"
            strSql += vbCrLf + "  ,NULL EXPENSES1,NULL EXPENSES2,NULL EXPENSES"
            strSql += vbCrLf + "  ,NULL INCACGROUPNAME1"
            strSql += vbCrLf + "  ,NULL INCOME2,NULL INCOME1,NULL INCOME,'1' RESULT,'1' SEP"
            strSql += vbCrLf + "  ,ACMAINCODE EXPACMAINCODE,'' INCACMAINCODE,'' EXPACGROUP,'' INCACGROUP"
            strSql += vbCrLf + "  ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,ACMAINGRPNAME EXPACMAINGRPNAME"
            strSql += vbCrLf + "  ,''INCACID,''INCACNAME,''INCACGROUPNAME,'' INCACMAINGRPNAME,'T1' COLHEAD"
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'E'"
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT "
            strSql += vbCrLf + "  DISTINCT '  ' + ACGRPNAME EXPACGROUPNAME1"
            strSql += vbCrLf + "  ,NULL EXPENSES2,NULL EXPENSES1,NULL EXPENSES"
            strSql += vbCrLf + "  ,NULL INCACGROUPNAME1"
            strSql += vbCrLf + "  ,NULL INCOME2,NULL INCOME1,NULL INCOME,'2' RESULT,'2' SEP"
            strSql += vbCrLf + "  ,ACMAINCODE EXPACMAINCODE,'' INCACMAINCODE,ACGRPCODE EXPACGROUP,'' INCACGROUP"
            strSql += vbCrLf + "  ,'' EXPACID,'' EXPACNAME,ACGRPNAME EXPACGROUPNAME,ACMAINGRPNAME EXPACMAINGRPNAME"
            strSql += vbCrLf + "  ,''INCACID,''INCACNAME,''INCACGROUPNAME,'' INCACMAINGRPNAME,'T2' COLHEAD" & vbTab + Environment.NewLine
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'E' AND ISNULL(ACMAINCODE,'') != ISNULL(ACGRPCODE,'')"
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT "
            strSql += vbCrLf + "  DISTINCT CASE WHEN ACMAINGRPNAME = ACGRPNAME THEN '  ' ELSE '    ' END + ACNAME EXPACGROUPNAME1"
            strSql += vbCrLf + "  ,CASE WHEN ACMAINGRPNAME != ACGRPNAME THEN CONVERT(VARCHAR,ISNULL(SUM(DEBIT),0) - ISNULL(SUM(CREDIT),0)) END EXPENSES2"
            strSql += vbCrLf + "  ,CASE WHEN ACMAINGRPNAME = ACGRPNAME THEN CONVERT(VARCHAR,ISNULL(SUM(DEBIT),0) - ISNULL(SUM(CREDIT),0)) END EXPENSES1,NULL EXPENSES"
            strSql += vbCrLf + "  ,NULL INCACGROUPNAME1"
            strSql += vbCrLf + "  ,NULL INCOME2,NULL INCOME1,NULL INCOME,'3' RESULT,'2' SEP"
            strSql += vbCrLf + "  ,ACMAINCODE EXPACMAINCODE,'' INCACMAINCODE,ACGRPCODE EXPACGROUP,'' INCACGROUP"
            strSql += vbCrLf + "  ,ACID EXPACID,ACNAME EXPACNAME,ACGRPNAME EXPACGROUPNAME,ACMAINGRPNAME EXPACMAINGRPNAME"
            strSql += vbCrLf + "  ,''INCACID,''INCACNAME,''INCACGROUPNAME,'' INCACMAINGRPNAME,'' COLHEAD" & vbTab + Environment.NewLine
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'E'"
            strSql += vbCrLf + "  GROUP BY ACMAINGRPNAME,ACGRPNAME,ACMAINCODE,ACGRPCODE,ACID,ACNAME"
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT "
            strSql += vbCrLf + "  DISTINCT '' EXPACGROUPNAME1"
            strSql += vbCrLf + "  ,CASE WHEN ACMAINGRPNAME != ACGRPNAME THEN '-------------------' END EXPENSES2"
            strSql += vbCrLf + "  ,CASE WHEN ACMAINGRPNAME  = ACGRPNAME THEN '-------------------' END EXPENSES1,NULL EXPENSES"
            strSql += vbCrLf + "  ,NULL INCACGROUPNAME1"
            strSql += vbCrLf + "  ,NULL INCOME2,NULL INCOME1,NULL INCOME,'4' RESULT,'2' SEP"
            strSql += vbCrLf + "  ,ACMAINCODE EXPACMAINCODE,'' INCACMAINCODE,ACGRPCODE EXPACGROUP,'' INCACGROUP"
            strSql += vbCrLf + "  ,'' EXPACID,'' EXPACNAME,ACGRPNAME EXPACGROUPNAME,ACMAINGRPNAME EXPACMAINGRPNAME"
            strSql += vbCrLf + "  ,''INCACID,''INCACNAME,''INCACGROUPNAME,'' INCACMAINGRPNAME,'S2' COLHEAD" & vbTab + Environment.NewLine
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'E'"
            strSql += vbCrLf + "  GROUP BY ACMAINGRPNAME,ACGRPNAME,ACMAINCODE,ACGRPCODE"
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT "
            strSql += vbCrLf + "  DISTINCT ' ' EXPACGROUPNAME1"
            strSql += vbCrLf + "  ,NULL EXPENSES1,CONVERT(VARCHAR,ISNULL(SUM(DEBIT),0) - ISNULL(SUM(CREDIT),0)) EXPENSES2,NULL EXPENSES"
            strSql += vbCrLf + "  ,NULL INCACGROUPNAME1"
            strSql += vbCrLf + "  ,NULL INCOME2,NULL INCOME1,NULL INCOME,'5' RESULT,'2' SEP"
            strSql += vbCrLf + "  ,ACMAINCODE EXPACMAINCODE,'' INCACMAINCODE,ACGRPCODE EXPACGROUP,'' INCACGROUP"
            strSql += vbCrLf + "  ,'' EXPACID,'' EXPACNAME,ACGRPNAME EXPACGROUPNAME,ACMAINGRPNAME EXPACMAINGRPNAME"
            strSql += vbCrLf + "  ,''INCACID,''INCACNAME,''INCACGROUPNAME,'' INCACMAINGRPNAME,'S2' COLHEAD" & vbTab + Environment.NewLine
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'E' AND ISNULL(ACMAINCODE,'') != ISNULL(ACGRPCODE,'')"
            strSql += vbCrLf + "  GROUP BY ACMAINGRPNAME,ACGRPNAME,ACMAINCODE,ACGRPCODE"
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT "
            strSql += vbCrLf + "  DISTINCT '  ' EXPACGROUPNAME1"
            strSql += vbCrLf + "  ,NULL EXPENSES1,'-------------------' EXPENSES2,NULL EXPENSES"
            strSql += vbCrLf + "  ,NULL INCACGROUPNAME1"
            strSql += vbCrLf + "  ,NULL INCOME2,NULL INCOME1,NULL INCOME,'6' RESULT,'2' SEP"
            strSql += vbCrLf + "  ,ACMAINCODE EXPACMAINCODE,'' INCACMAINCODE,ACGRPCODE EXPACGROUP,'' INCACGROUP"
            strSql += vbCrLf + "  ,'' EXPACID,'' EXPACNAME,ACGRPNAME EXPACGROUPNAME,ACMAINGRPNAME EXPACMAINGRPNAME"
            strSql += vbCrLf + "  ,''INCACID,''INCACNAME,''INCACGROUPNAME,'' INCACMAINGRPNAME,'S1' COLHEAD" & vbTab + Environment.NewLine
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'E' AND ISNULL(ACMAINCODE,'') != ISNULL(ACGRPCODE,'')"
            strSql += vbCrLf + "  GROUP BY ACMAINGRPNAME,ACGRPNAME,ACMAINCODE,ACGRPCODE"
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT "
            strSql += vbCrLf + "  DISTINCT '' EXPACGROUPNAME1"
            strSql += vbCrLf + "  ,NULL EXPENSES2,NULL EXPENSES1,CONVERT(VARCHAR,ISNULL(SUM(DEBIT),0) - ISNULL(SUM(CREDIT),0)) EXPENSES"
            strSql += vbCrLf + "  ,NULL INCACGROUPNAME1"
            strSql += vbCrLf + "  ,NULL INCOME2,NULL INCOME1,NULL INCOME"
            strSql += vbCrLf + "  ,'7' RESULT,'3' SEP"
            strSql += vbCrLf + "  ,ACMAINCODE EXPACMAINCODE,'' INCACMAINCODE,'' EXPACGROUP,'' INCACGROUP"
            strSql += vbCrLf + "  ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,ACMAINGRPNAME EXPACMAINGRPNAME"
            strSql += vbCrLf + "  ,''INCACID,''INCACNAME,''INCACGROUPNAME,'' INCACMAINGRPNAME,'G1' COLHEAD" & vbTab + Environment.NewLine
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'E'"
            strSql += vbCrLf + "  GROUP BY ACMAINGRPNAME,ACMAINCODE"
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT "
            strSql += vbCrLf + "  DISTINCT '' EXPACGROUPNAME1"
            strSql += vbCrLf + "  ,NULL EXPENSES2,NULL EXPENSES1,'-------------------' EXPENSES"
            strSql += vbCrLf + "  ,NULL INCACGROUPNAME1"
            strSql += vbCrLf + "  ,NULL INCOME2,NULL INCOME1,NULL INCOME"
            strSql += vbCrLf + "  ,'8' RESULT,'4' SEP"
            strSql += vbCrLf + "  ,ACMAINCODE EXPACMAINCODE,'' INCACMAINCODE,'' EXPACGROUP,'' INCACGROUP"
            strSql += vbCrLf + "  ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,ACMAINGRPNAME EXPACMAINGRPNAME"
            strSql += vbCrLf + "  ,''INCACID,''INCACNAME,''INCACGROUPNAME,'' INCACMAINGRPNAME,'G1' COLHEAD" & vbTab + Environment.NewLine
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'E'"
            strSql += vbCrLf + "  GROUP BY ACMAINGRPNAME,ACMAINCODE"
            strSql += vbCrLf + "  ORDER BY EXPACMAINCODE,EXPACMAINGRPNAME,SEP,EXPACGROUPNAME,RESULT,EXPACNAME"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE4"
            strSql += vbCrLf + "  (EXPACGROUPNAME1,EXPENSES1,EXPENSES2,EXPENSES,INCACGROUPNAME1,INCOME1,INCOME2,INCOME,RESULT,SEP,EXPACMAINCODE,INCACMAINCODE,EXPACGROUP,INCACGROUP,EXPACID"
            strSql += vbCrLf + "  ,EXPACNAME,EXPACGROUPNAME,EXPACMAINGRPNAME,INCACID,INCACNAME,INCACGROUPNAME,INCACMAINGRPNAME,COLHEAD)"
            strSql += vbCrLf + "  SELECT "
            strSql += vbCrLf + "  DISTINCT NULL EXPACGROUPNAME1"
            strSql += vbCrLf + "  ,NULL EXPENSES1,NULL EXPENSES2,NULL EXPENSES"
            strSql += vbCrLf + "  ,ACMAINGRPNAME INCACGROUPNAME1"
            strSql += vbCrLf + "  ,NULL INCOME2,NULL INCOME1,NULL INCOME,'1' RESULT,'1' SEP"
            strSql += vbCrLf + "  ,'' EXPACMAINCODE,ACMAINCODE INCACMAINCODE,'' EXPACGROUP,'' INCACGROUP"
            strSql += vbCrLf + "  ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,'' EXPACMAINGRPNAME"
            strSql += vbCrLf + "  ,'' INCACID,''INCACNAME,''INCACGROUPNAME,ACMAINGRPNAME INCACMAINGRPNAME,'T1' COLHEAD" & vbTab + Environment.NewLine
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'I'"
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT "
            strSql += vbCrLf + "  DISTINCT NULL EXPACGROUPNAME1"
            strSql += vbCrLf + "  ,NULL EXPENSES2,NULL EXPENSES1,NULL EXPENSES"
            strSql += vbCrLf + "  ,'  ' + ACGRPNAME INCACGROUPNAME1"
            strSql += vbCrLf + "  ,NULL INCOME2,NULL INCOME1,NULL INCOME,'2' RESULT,'2' SEP"
            strSql += vbCrLf + "  ,'' EXPACMAINCODE,ACMAINCODE INCACMAINCODE,'' EXPACGROUP,ACGRPCODE INCACGROUP"
            strSql += vbCrLf + "  ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,'' EXPACMAINGRPNAME"
            strSql += vbCrLf + "  ,''INCACID,''INCACNAME,ACGRPNAME INCACGROUPNAME,ACMAINGRPNAME INCACMAINGRPNAME,'T2' COLHEAD" & vbTab + Environment.NewLine
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'I' AND ISNULL(ACMAINCODE,'') != ISNULL(ACGRPCODE,'')"
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT "
            strSql += vbCrLf + "  DISTINCT '' EXPACGROUPNAME1"
            strSql += vbCrLf + "  ,NULL EXPENSES2,NULL EXPENSES1,NULL EXPENSES"
            strSql += vbCrLf + "  ,CASE WHEN ACMAINGRPNAME = ACGRPNAME THEN '  ' ELSE '    ' END + ACNAME INCACGROUPNAME1"
            strSql += vbCrLf + "  ,CASE WHEN ACMAINGRPNAME != ACGRPNAME THEN CONVERT(VARCHAR,ISNULL(SUM(CREDIT),0) - ISNULL(SUM(DEBIT),0)) END INCOME2"
            strSql += vbCrLf + "  ,CASE WHEN ACMAINGRPNAME  = ACGRPNAME THEN CONVERT(VARCHAR,ISNULL(SUM(CREDIT),0) - ISNULL(SUM(DEBIT),0)) END INCOME1,NULL INCOME,'3' RESULT,'2' SEP"
            strSql += vbCrLf + "  ,'' EXPACMAINCODE,ACMAINCODE INCACMAINCODE,'' EXPACGROUP,ACGRPCODE INCACGROUP"
            strSql += vbCrLf + "  ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,'' EXPACMAINGRPNAME"
            strSql += vbCrLf + "  ,ACID INCACID,ACNAME INCACNAME,ACGRPNAME INCACGROUPNAME,ACMAINGRPNAME INCACMAINGRPNAME,'' COLHEAD" & vbTab + Environment.NewLine
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'I'"
            strSql += vbCrLf + "  GROUP BY ACMAINGRPNAME,ACGRPNAME,ACMAINCODE,ACGRPCODE,ACID,ACNAME"
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT "
            strSql += vbCrLf + "  DISTINCT '' EXPACGROUPNAME1"
            strSql += vbCrLf + "  ,'' EXPENSES2,NULL EXPENSES1,NULL EXPENSES"
            strSql += vbCrLf + "  ,NULL INCACGROUPNAME1"
            strSql += vbCrLf + "  ,CASE WHEN ACMAINGRPNAME != ACGRPNAME THEN '-------------------' END INCOME2"
            strSql += vbCrLf + "  ,CASE WHEN ACMAINGRPNAME  = ACGRPNAME THEN '-------------------' END INCOME1,NULL INCOME,'4' RESULT,'2' SEP"
            strSql += vbCrLf + "  ,'' EXPACMAINCODE,ACMAINCODE INCACMAINCODE,'' EXPACGROUP,ACGRPCODE INCACGROUP"
            strSql += vbCrLf + "  ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,'' EXPACMAINGRPNAME"
            strSql += vbCrLf + "  ,''INCACID,''INCACNAME,ACGRPNAME INCACGROUPNAME,ACMAINGRPNAME INCACMAINGRPNAME,'S2' COLHEAD"
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'I'"
            strSql += vbCrLf + "  GROUP BY ACMAINGRPNAME,ACGRPNAME,ACMAINCODE,ACGRPCODE"
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT "
            strSql += vbCrLf + "  DISTINCT ' ' EXPACGROUPNAME1"
            strSql += vbCrLf + "  ,NULL EXPENSES2,NULL EXPENSES1,NULL EXPENSES"
            strSql += vbCrLf + "  ,NULL INCACGROUPNAME1"
            strSql += vbCrLf + "  ,NULL INCOME2,CONVERT(VARCHAR,ISNULL(SUM(CREDIT),0) - ISNULL(SUM(DEBIT),0)) INCOME1,NULL INCOME,'5' RESULT,'2' SEP"
            strSql += vbCrLf + "  ,'' EXPACMAINCODE,ACMAINCODE INCACMAINCODE,'' EXPACGROUP,ACGRPCODE INCACGROUP"
            strSql += vbCrLf + "  ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,'' EXPACMAINGRPNAME"
            strSql += vbCrLf + "  ,''INCACID,''INCACNAME,ACGRPNAME INCACGROUPNAME,ACMAINGRPNAME INCACMAINGRPNAME,'S1' COLHEAD" & vbTab + Environment.NewLine
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'I' AND ISNULL(ACMAINCODE,'') != ISNULL(ACGRPCODE,'')"
            strSql += vbCrLf + "  GROUP BY ACMAINGRPNAME,ACGRPNAME,ACMAINCODE,ACGRPCODE"
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT "
            strSql += vbCrLf + "  DISTINCT '  ' EXPACGROUPNAME1"
            strSql += vbCrLf + "  ,NULL EXPENSES2,'' EXPENSES1,NULL EXPENSES"
            strSql += vbCrLf + "  ,NULL INCACGROUPNAME1"
            strSql += vbCrLf + "  ,NULL INCOME2,'-------------------' INCOME1,NULL INCOME,'6' RESULT,'2' SEP"
            strSql += vbCrLf + "  ,ACMAINCODE EXPACMAINCODE,'' INCACMAINCODE,ACGRPCODE EXPACGROUP,'' INCACGROUP"
            strSql += vbCrLf + "  ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME ,'' EXPACMAINGRPNAME"
            strSql += vbCrLf + "  ,''INCACID,''INCACNAME,ACGRPNAME INCACGROUPNAME,ACMAINGRPNAME INCACMAINGRPNAME,'S2' COLHEAD" & vbTab + Environment.NewLine
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'I' AND ISNULL(ACMAINCODE,'') != ISNULL(ACGRPCODE,'')"
            strSql += vbCrLf + "  GROUP BY ACMAINGRPNAME,ACGRPNAME,ACMAINCODE,ACGRPCODE"
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT "
            strSql += vbCrLf + "  DISTINCT '' EXPACGROUPNAME1"
            strSql += vbCrLf + "  ,NULL EXPENSES2,NULL EXPENSES1,NULL EXPENSES"
            strSql += vbCrLf + "  ,NULL INCACGROUPNAME1"
            strSql += vbCrLf + "  ,NULL INCOME2,NULL INCOME1,CONVERT(VARCHAR,ISNULL(SUM(CREDIT),0) - ISNULL(SUM(DEBIT),0)) INCOME"
            strSql += vbCrLf + "  ,'7' RESULT,'3' SEP"
            strSql += vbCrLf + "  ,'' EXPACMAINCODE,ACMAINCODE INCACMAINCODE,'' EXPACGROUP,'' INCACGROUP"
            strSql += vbCrLf + "  ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,'' EXPACMAINGRPNAME"
            strSql += vbCrLf + "  ,''INCACID,''INCACNAME,''INCACGROUPNAME,ACMAINGRPNAME INCACMAINGRPNAME,'G1' COLHEAD" & vbTab + Environment.NewLine
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'I'"
            'FOR ANB STARTS
            'strSql += vbCrLf + "  AND ISNULL(ACMAINCODE,'') != ISNULL(ACGRPCODE,'')"
            'FOR ANB ENDS
            strSql += vbCrLf + "  GROUP BY ACMAINGRPNAME,ACMAINCODE"
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT "
            strSql += vbCrLf + "  DISTINCT '' EXPACGROUPNAME1"
            strSql += vbCrLf + "  ,NULL EXPENSES2,NULL EXPENSES1,'' EXPENSES"
            strSql += vbCrLf + "  ,NULL INCACGROUPNAME1"
            strSql += vbCrLf + "  ,NULL INCOME2,NULL INCOME1,'-------------------' INCOME"
            strSql += vbCrLf + "  ,'8' RESULT,'4' SEP"
            strSql += vbCrLf + "  ,'' EXPACMAINCODE,ACMAINCODE INCACMAINCODE,'' EXPACGROUP,'' INCACGROUP"
            strSql += vbCrLf + "  ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,'' EXPACMAINGRPNAME"
            strSql += vbCrLf + "  ,''INCACID,''INCACNAME,''INCACGROUPNAME,ACMAINGRPNAME INCACMAINGRPNAME,'G1' COLHEAD" & vbTab + Environment.NewLine
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE2 WHERE ISNULL(GRPTYPE,'') = 'I'"
            strSql += vbCrLf + "  GROUP BY ACMAINGRPNAME,ACMAINCODE"
            strSql += vbCrLf + "  ORDER BY INCACMAINGRPNAME,SEP,INCACGROUPNAME,RESULT,INCACNAME"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = "  IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & userId.ToString() & "TRADE5')"
            strSql += vbCrLf + "  DROP TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE5"
            strSql += vbCrLf + "  SELECT "
            strSql += vbCrLf + "  T3.EXPACGROUPNAME1,T3.EXPENSES1,T3.EXPENSES2,T3.EXPENSES,T4.INCACGROUPNAME1,T4.INCOME1,T4.INCOME2,T4.INCOME"
            strSql += vbCrLf + "  ,T3.SNO SNO3,T4.SNO SNO4,T3.RESULT RESULT3,T4.RESULT RESULT4,T3.SEP SEP3,T4.SEP SEP4,T3.COLHEAD COLHEAD3,T4.COLHEAD COLHEAD4"
            strSql += vbCrLf + "  INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE5"
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE3 AS T3 FULL JOIN TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE4 AS T4 "
            strSql += vbCrLf + "  ON T3.SNO = T4.SNO"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE5"
            strSql += vbCrLf + " (EXPACGROUPNAME1,EXPENSES,INCACGROUPNAME1,INCOME,COLHEAD3,COLHEAD4,RESULT3,RESULT4)"
            strSql += vbCrLf + " SELECT"
            strSql += vbCrLf + " CASE WHEN SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT4 IN ('0','7') THEN INCOME ELSE '0' END)) "
            strSql += vbCrLf + " - SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT3 IN ('0','7') THEN EXPENSES ELSE '0' END)) > 0 THEN 'GROSS PROFIT' END EXPACGROUPNAME1 "
            strSql += vbCrLf + " ,CONVERT(VARCHAR,CASE WHEN SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT4 IN ('0','7') THEN INCOME ELSE '0' END)) - SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT3 IN ('0','7') "
            strSql += vbCrLf + " THEN EXPENSES ELSE '0' END)) > 0 THEN SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT4 IN ('0','7') THEN INCOME ELSE '0' END)) "
            strSql += vbCrLf + " - SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT3 IN ('0','7') THEN EXPENSES ELSE '0' END)) END) EXPENSES"
            strSql += vbCrLf + " , CASE WHEN SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT3 IN ('0','7') THEN EXPENSES ELSE '0' END)) - SUM(CONVERT(NUMERIC(15,2)"
            strSql += vbCrLf + " ,CASE WHEN RESULT4 IN ('0','7') THEN INCOME ELSE '0' END)) > 0 THEN 'GROSS LOSS' END INCACGROUPNAME1  "
            strSql += vbCrLf + " ,CONVERT(VARCHAR,CASE WHEN SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT3 IN ('0','7') THEN EXPENSES ELSE '0' END)) - SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT4 IN ('0','7') "
            strSql += vbCrLf + " THEN INCOME ELSE '0' END)) > 0 THEN SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT3 IN ('0','7') THEN EXPENSES ELSE '0' END)) "
            strSql += vbCrLf + " - SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT4 IN ('0','7') THEN INCOME ELSE '0' END)) END) INCOME"
            strSql += vbCrLf + " ,'G' COLHEAD3,'G' COLHEAD4"
            strSql += vbCrLf + " ,CASE WHEN SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT4 IN ('0','7') THEN INCOME ELSE '0' END)) "
            strSql += vbCrLf + " - SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT3 IN ('0','7') THEN EXPENSES ELSE '0' END)) > 0 THEN '9' END RESULT3"
            strSql += vbCrLf + " , CASE WHEN SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT3 IN ('0','7') THEN EXPENSES ELSE '0' END))"
            strSql += vbCrLf + " - SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT4 IN ('0','7') THEN INCOME ELSE '0' END)) > 0 THEN '9' END RESULT4"
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE5"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = " SELECT "
            strSql += vbCrLf + " DISTINCT 'TRADING - 01/" & CnStartMonth & "/" & (IIf(dtpDate.Value.[Date].Month > 3, dtpDate.Value.[Date].Year, dtpDate.Value.[Date].Year - 1)).ToString() & " - " & dtpDate.Text.Replace("-", "/") & "' EXPACGROUPNAME1,' 'EXPENSES1,' ' EXPENSES2,' 'EXPENSES "
            strSql += vbCrLf + " ,' ' INCACGROUPNAME1,' 'INCOME1,' 'INCOME2,' 'INCOME,'T' COLHEAD3,'T' COLHEAD4"
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE5"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + "  SELECT "
            strSql += vbCrLf + "  EXPACGROUPNAME1"
            strSql += vbCrLf + "     ,CASE WHEN RESULT3 NOT IN (4,6,8) AND ISNULL(EXPENSES1,'') != '' THEN " & cnStockDb & ".[DBO].FUNC_RUPEES_WITHCOMMA(CONVERT(NUMERIC(15,2),CASE WHEN ISNULL(EXPENSES1,'') = '' THEN '0' ELSE EXPENSES1 END),'D','Y') ELSE EXPENSES1 END  EXPENSES1"
            strSql += vbCrLf + "     ,CASE WHEN RESULT3 NOT IN (4,6,8) AND ISNULL(EXPENSES2,'') != '' THEN " & cnStockDb & ".[DBO].FUNC_RUPEES_WITHCOMMA(CONVERT(NUMERIC(15,2),CASE WHEN ISNULL(EXPENSES2,'') = '' THEN '0' ELSE EXPENSES2 END),'D','Y') ELSE EXPENSES2 END EXPENSES2"
            strSql += vbCrLf + "     ,CASE WHEN RESULT3 NOT IN (4,6,8) AND ISNULL(EXPENSES,'') != '' THEN " & cnStockDb & ".[DBO].FUNC_RUPEES_WITHCOMMA(CONVERT(NUMERIC(15,2),CASE WHEN ISNULL(EXPENSES,'') = '' THEN '0' ELSE EXPENSES END),'D','Y') ELSE EXPENSES END EXPENSES "
            strSql += vbCrLf + "  ,INCACGROUPNAME1"
            strSql += vbCrLf + "     ,CASE WHEN RESULT4 NOT IN (4,6,8) AND ISNULL(INCOME1,'') != '' THEN " & cnStockDb & ".[DBO].FUNC_RUPEES_WITHCOMMA(CONVERT(NUMERIC(15,2),CASE WHEN ISNULL(INCOME1,'') = '' THEN '0' ELSE INCOME1 END),'D','Y') ELSE INCOME1 END  INCOME1"
            strSql += vbCrLf + "     ,CASE WHEN RESULT4 NOT IN (4,6,8) AND ISNULL(INCOME2,'') != '' THEN " & cnStockDb & ".[DBO].FUNC_RUPEES_WITHCOMMA(CONVERT(NUMERIC(15,2),CASE WHEN ISNULL(INCOME2,'') = '' THEN '0' ELSE INCOME2 END),'D','Y') ELSE INCOME2 END INCOME2"
            strSql += vbCrLf + "     ,CASE WHEN RESULT4 NOT IN (4,6,8) AND ISNULL(INCOME,'') != '' THEN " & cnStockDb & ".[DBO].FUNC_RUPEES_WITHCOMMA(CONVERT(NUMERIC(15,2),CASE WHEN ISNULL(INCOME,'') = '' THEN '0' ELSE INCOME END),'D','Y') ELSE INCOME END INCOME "
            strSql += vbCrLf + "     ,COLHEAD3,COLHEAD4"
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE5"
            strSql += vbCrLf + "  UNION ALL"
            'strSql += vbCrLf + "  SELECT"
            'strSql += vbCrLf + "    '' EXPACGROUPNAME1 ,NULL EXPENSES1,NULL EXPENSES2,CONVERT(VARCHAR," & cnStockDb & ".[DBO].FUNC_RUPEES_WITHCOMMA(SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT3 IN ('0','7') AND ISNULL(EXPENSES,'') = '' THEN CONVERT(NUMERIC(15,2),CASE WHEN ISNULL(EXPENSES,'') = '' THEN '0' ELSE EXPENSES END) ELSE '0' END)),'D','Y')) EXPENSES"
            'strSql += vbCrLf + "  ,'' INCACGROUPNAME1 ,NULL INCOME1,NULL INCOME2,CONVERT(VARCHAR," & cnStockDb & ".[DBO].FUNC_RUPEES_WITHCOMMA(SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT4 IN ('0','7') AND ISNULL(INCOME,'') = '' THEN CONVERT(NUMERIC(15,2),CASE WHEN ISNULL(INCOME,'') = '' THEN '0' ELSE INCOME END) ELSE '0' END)),'D','Y')) INCOME,'G1' COLHEAD3,'G1' COLHEAD4"
            'strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE5"
            'strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT"
            strSql += vbCrLf + "   DISTINCT '' EXPACGROUPNAME1 ,NULL EXPENSES1,NULL EXPENSES2,'-------------------' EXPENSES"
            strSql += vbCrLf + "  ,'' INCACGROUPNAME1 ,NULL INCOME1,NULL INCOME2,'-------------------' INCOME,'G1' COLHEAD3,'G1' COLHEAD4"
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE5"
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT"
            strSql += vbCrLf + "    '' EXPACGROUPNAME1 ,NULL EXPENSES1,NULL EXPENSES2,CONVERT(VARCHAR," & cnStockDb & ".[DBO].FUNC_RUPEES_WITHCOMMA(SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT3 IN ('0','7','9') /*AND ISNULL(EXPENSES,'') = ''*/ THEN CONVERT(NUMERIC(15,2),CASE WHEN ISNULL(EXPENSES,'') = '' THEN '0' ELSE EXPENSES END) ELSE '0' END)),'D','Y')) EXPENSES"
            strSql += vbCrLf + "  ,'' INCACGROUPNAME1 ,NULL INCOME1,NULL INCOME2,CONVERT(VARCHAR," & cnStockDb & ".[DBO].FUNC_RUPEES_WITHCOMMA(SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT4 IN ('0','7','9') /*AND ISNULL(INCOME,'') = ''*/ THEN CONVERT(NUMERIC(15,2),CASE WHEN ISNULL(INCOME,'') = '' THEN '0' ELSE INCOME END) ELSE '0' END)),'D','Y')) INCOME,'G1' COLHEAD3,'G1' COLHEAD4"
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE5"
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT"
            strSql += vbCrLf + "   DISTINCT '' EXPACGROUPNAME1 ,NULL EXPENSES1,NULL EXPENSES2,'-------------------' EXPENSES"
            strSql += vbCrLf + "  ,'' INCACGROUPNAME1 ,NULL INCOME1,NULL INCOME2,'-------------------' INCOME,'G1' COLHEAD3,'G1' COLHEAD4"
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE5"
            'strSql += vbCrLf + "  UNION ALL"
            'strSql += vbCrLf + "  SELECT"
            'strSql += vbCrLf + "    CASE WHEN SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT3 IN ('0','7') THEN EXPENSES ELSE '0' END)) - SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT4 IN ('0','7') THEN INCOME ELSE '0' END)) > 0 THEN 'GROSS LOSS' END EXPACGROUPNAME1 ,NULL EXPENSES1,NULL EXPENSES2"
            'strSql += vbCrLf + "  ,CONVERT(VARCHAR,CASE WHEN SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT3 IN ('0','7') THEN EXPENSES ELSE '0' END)) - SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT4 IN ('0','7') THEN INCOME ELSE '0' END)) > 0 THEN " & cnStockDb & ".[DBO].FUNC_RUPEES_WITHCOMMA(SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT3 IN ('0','7') THEN EXPENSES ELSE '0' END)) - SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT4 IN ('0','7') THEN INCOME ELSE '0' END)),'D','Y') END) EXPENSES"
            'strSql += vbCrLf + "  ,CASE WHEN SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT4 IN ('0','7') THEN INCOME ELSE '0' END)) - SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT3 IN ('0','7') THEN EXPENSES ELSE '0' END)) > 0 THEN 'GROSS PROFIT' END INCACGROUPNAME1 ,NULL INCOME1,NULL INCOME2"
            'strSql += vbCrLf + "  ,CONVERT(VARCHAR,CASE WHEN SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT4 IN ('0','7') THEN INCOME ELSE '0' END)) - SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT3 IN ('0','7') THEN EXPENSES ELSE '0' END)) > 0 THEN " & cnStockDb & ".[DBO].FUNC_RUPEES_WITHCOMMA(SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT4 IN ('0','7') THEN INCOME ELSE '0' END)) - SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT3 IN ('0','7') THEN EXPENSES ELSE '0' END)),'D','Y') END) INCOME,'G' COLHEAD3,'G' COLHEAD4"
            'strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE5"
            If ForPL = False Then
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dt)
                If dt.Rows.Count <= 2 Then
                    dt = New DataTable()
                End If
            End If
            'gridView.DataSource = dt
        Catch ex As Exception
            If Val(cnttchkk) < 5 Then
                cnttchkk += 1
                GoTo TTTT
            Else
                MsgBox(ex.Message, MsgBoxStyle.Information)
            End If
        End Try
    End Sub
    Private Sub funcPLBetweenDates(ByVal strcompid As String, ByVal strCostId As String, ByVal summary As Boolean)


        Dim OPStrNew As String
        Dim OPStartdate As Date
        Dim Fromdate As Date
        Fromdate = dtpDate.Value.Date.AddDays(-1)
        OPStrNew = "SELECT STARTDATE FROM " & cnAdminDb & "..DBMASTER WHERE '" & Format(dtpDate.Value, "yyyy-MM-dd") & "' BETWEEN STARTDATE AND ENDDATE "
        OPStartdate = GetSqlValue(cn, OPStrNew)

        If chkTrading.Checked = False Then funcTrading(strcompid, strCostId, summary, True)
        strSql = " IF EXISTS (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & userId.ToString() & "PROFITLOSS1')" & vbCrLf
        strSql += " DROP TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS1" & vbCrLf
        strSql += " CREATE TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS1" & vbCrLf
        strSql += " (COMPANYID VARCHAR(3),ACID VARCHAR(25),TRANMODE VARCHAR(1),AMOUNT NUMERIC(15,2),DEBIT NUMERIC(15,2),CREDIT NUMERIC(15,2)" & vbCrLf
        strSql += " ,TRANDATE SMALLDATETIME,PAYMODE VARCHAR(20),TRANNO VARCHAR(25),SEP VARCHAR(1))" & vbCrLf
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " INSERT INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS1" & vbCrLf
        strSql += " (COMPANYID,ACID,TRANMODE,AMOUNT,DEBIT,CREDIT,TRANDATE,PAYMODE,TRANNO,SEP)" & vbCrLf
        If dtpDate.Value.Date > cnTranFromDate Then
            strSql += " SELECT " & vbCrLf
            strSql += "  COMPANYID,ACCODE ACID,CASE WHEN ISNULL(DEBIT,0) > ISNULL(CREDIT,0) THEN 'D' ELSE 'C' END TRANMODE" & vbCrLf
            strSql += " ,CASE WHEN ISNULL(DEBIT,0) <> 0 THEN DEBIT ELSE CREDIT END AMOUNT" & vbCrLf
            strSql += " ,DEBIT,CREDIT,NULL TRANDATE,'' PAYMODE,NULL TRANNO,'O' SEP" & vbCrLf
            strSql += " FROM " & cnStockDb & "..OPENTRAILBALANCE WHERE ISNULL(COMPANYID,'') = '" & strcompid & "'" & vbCrLf
            strSql += strCostId
            strSql += " UNION ALL" & vbCrLf
        End If
        strSql += " SELECT " & vbCrLf
        strSql += "  COMPANYID,ACCODE ACID,TRANMODE,AMOUNT" & vbCrLf
        strSql += " ,CASE WHEN TRANMODE = 'D' THEN AMOUNT END DEBIT,CASE WHEN TRANMODE = 'C' THEN AMOUNT END CREDIT" & vbCrLf
        strSql += " ,TRANDATE,PAYMODE,TRANNO,'T' SEP" & vbCrLf
        strSql += " FROM " & cnStockDb & "..ACCTRAN WHERE ISNULL(COMPANYID,'') = '" & strcompid & "' AND TRANDATE >= '" & cnTranFromDate & "' AND TRANDATE <= '" & Format(Fromdate, "yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') != 'Y'" & vbCrLf
        strSql += strCostId
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " IF EXISTS (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & userId.ToString() & "PROFITLOSS2')" & vbCrLf
        strSql += " DROP TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS2" & vbCrLf



        strSql += "  SELECT " & vbCrLf
        strSql += "   '' ACID,G.ACGRPCODE,SG.ACGRPNAME ACGRPNAME,G.ACMAINCODE,SG.GRPLEDGER,SG.GRPTYPE" & vbCrLf
        'strSql += "   A.ACCODE ACID,G.ACGRPCODE,ACGRPNAME ACGRPNAME,ACMAINCODE,G.GRPLEDGER,G.GRPTYPE" & vbCrLf
        strSql += "  ,SUM(DEBIT) DEBIT,SUM(CREDIT) CREDIT" & vbCrLf
        strSql += "  ,'' ACNAME,'' TRANDATE,CASE WHEN SUM(AMOUNT) > 0 THEN 'D' ELSE 'C' END TRANMODE,SUM(AMOUNT) AMOUNT,'' PAYMODE,'' TRANNO" & vbCrLf
        strSql += "  ,(SELECT ACGRPNAME FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPCODE = G.ACMAINCODE) ACMAINGRPNAME" & vbCrLf
        strSql += "  INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS2" & vbCrLf
        strSql += "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS1 AS T INNER JOIN " & cnAdminDb & "..ACHEAD AS A" & vbCrLf
        strSql += "  ON T.ACID = A.ACCODE INNER JOIN " & cnAdminDb & "..ACGROUP AS G ON G.ACGRPCODE = A.ACGRPCODE" & vbCrLf
        strSql += "  INNER JOIN " & cnAdminDb & "..ACGROUP AS SG ON SG.ACGRPCODE = A.ACGRPCODE"
        strSql += "  AND (SG.GRPLEDGER IN ('P') OR G.GRPLEDGER IN ('P')) " & vbCrLf
        strSql += "  GROUP BY G.ACGRPCODE,SG.ACGRPNAME,G.ACMAINCODE,SG.GRPLEDGER,SG.GRPTYPE"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()


        strSql = vbCrLf + " SELECT SUM(CASE WHEN GRPTYPE='I' THEN AMOUNT ELSE -1*AMOUNT END)TRADING FROM ( "
        strSql += vbCrLf + " SELECT ACGRPNAME,GRPTYPE,(CASE WHEN ISNULL(CREDIT,0)>ISNULL(DEBIT,0) THEN ISNULL(CREDIT,0)-ISNULL(DEBIT,0) ELSE ISNULL(DEBIT,0)-ISNULL(CREDIT,0) END)AMOUNT "
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS2)X "
        Dim OpenPL As Double = 0
        OpenPL = Math.Round(Val(GetSqlValue(cn, strSql).ToString), 2)




        If chkTrading.Checked = False Then funcTrading(strcompid, strCostId, summary, True)
        strSql = " IF EXISTS (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & userId.ToString() & "PROFITLOSS1')" & vbCrLf
        strSql += " DROP TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS1" & vbCrLf
        strSql += " CREATE TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS1" & vbCrLf
        strSql += " (COMPANYID VARCHAR(3),ACID VARCHAR(25),TRANMODE VARCHAR(1),AMOUNT NUMERIC(15,2),DEBIT NUMERIC(15,2),CREDIT NUMERIC(15,2)" & vbCrLf
        strSql += " ,TRANDATE SMALLDATETIME,PAYMODE VARCHAR(20),TRANNO VARCHAR(25),SEP VARCHAR(1))" & vbCrLf
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " INSERT INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS1" & vbCrLf
        strSql += " (COMPANYID,ACID,TRANMODE,AMOUNT,DEBIT,CREDIT,TRANDATE,PAYMODE,TRANNO,SEP)" & vbCrLf
        If dtpDate.Value.Date = cnTranFromDate Then
            strSql += " SELECT " & vbCrLf
            strSql += "  COMPANYID,ACCODE ACID,CASE WHEN ISNULL(DEBIT,0) > ISNULL(CREDIT,0) THEN 'D' ELSE 'C' END TRANMODE" & vbCrLf
            strSql += " ,CASE WHEN ISNULL(DEBIT,0) <> 0 THEN DEBIT ELSE CREDIT END AMOUNT" & vbCrLf
            strSql += " ,DEBIT,CREDIT,NULL TRANDATE,'' PAYMODE,NULL TRANNO,'O' SEP" & vbCrLf
            strSql += " FROM " & cnStockDb & "..OPENTRAILBALANCE WHERE ISNULL(COMPANYID,'') = '" & strcompid & "'" & vbCrLf
            strSql += strCostId
            strSql += " UNION ALL" & vbCrLf
        End If
        strSql += " SELECT " & vbCrLf
        strSql += "  COMPANYID,ACCODE ACID,TRANMODE,AMOUNT" & vbCrLf
        strSql += " ,CASE WHEN TRANMODE = 'D' THEN AMOUNT END DEBIT,CASE WHEN TRANMODE = 'C' THEN AMOUNT END CREDIT" & vbCrLf
        strSql += " ,TRANDATE,PAYMODE,TRANNO,'T' SEP" & vbCrLf
        strSql += " FROM " & cnStockDb & "..ACCTRAN WHERE ISNULL(COMPANYID,'') = '" & strcompid & "' AND TRANDATE >= '" & dtpDate.Value.Date.ToString("yyyy-MM-dd") & "' AND TRANDATE <= '" & dtpToDate.Value.Date.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') != 'Y'" & vbCrLf
        strSql += strCostId
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " IF EXISTS (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & userId.ToString() & "PROFITLOSS2')" & vbCrLf
        strSql += " DROP TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS2" & vbCrLf
        If summary Then
            strSql += "  SELECT " & vbCrLf
            strSql += "   '' ACID,G.ACGRPCODE,SG.ACGRPNAME ACGRPNAME,G.ACMAINCODE,SG.GRPLEDGER,SG.GRPTYPE" & vbCrLf
            'strSql += "   A.ACCODE ACID,G.ACGRPCODE,ACGRPNAME ACGRPNAME,ACMAINCODE,G.GRPLEDGER,G.GRPTYPE" & vbCrLf
            strSql += "  ,SUM(DEBIT) DEBIT,SUM(CREDIT) CREDIT" & vbCrLf
            strSql += "  ,'' ACNAME,'' TRANDATE,CASE WHEN SUM(AMOUNT) > 0 THEN 'D' ELSE 'C' END TRANMODE,SUM(AMOUNT) AMOUNT,'' PAYMODE,'' TRANNO" & vbCrLf
            strSql += "  ,(SELECT ACGRPNAME FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPCODE = G.ACMAINCODE) ACMAINGRPNAME" & vbCrLf
            strSql += "  INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS2" & vbCrLf
            strSql += "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS1 AS T INNER JOIN " & cnAdminDb & "..ACHEAD AS A" & vbCrLf
            strSql += "  ON T.ACID = A.ACCODE INNER JOIN " & cnAdminDb & "..ACGROUP AS G ON G.ACGRPCODE = A.ACGRPCODE" & vbCrLf
            strSql += "  INNER JOIN " & cnAdminDb & "..ACGROUP AS SG ON SG.ACGRPCODE = A.ACGRPCODE"
            strSql += "  AND (SG.GRPLEDGER IN ('P') OR G.GRPLEDGER IN ('P')) " & vbCrLf
            strSql += "  GROUP BY G.ACGRPCODE,SG.ACGRPNAME,G.ACMAINCODE,SG.GRPLEDGER,SG.GRPTYPE"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        Else
            strSql += " SELECT " & vbCrLf
            strSql += "  A.ACCODE ACID,G.ACGRPCODE,ACGRPNAME,ACMAINCODE,GRPLEDGER,GRPTYPE" & vbCrLf
            strSql += " ,DEBIT,CREDIT" & vbCrLf
            strSql += " ,ACNAME ACNAME,TRANDATE,TRANMODE,AMOUNT,PAYMODE,TRANNO" & vbCrLf
            strSql += " ,(SELECT ACGRPNAME FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPCODE = G.ACMAINCODE) ACMAINGRPNAME" & vbCrLf
            strSql += " INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS2" & vbCrLf
            strSql += " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS1 AS T INNER JOIN " & cnAdminDb & "..ACHEAD AS A" & vbCrLf
            strSql += " ON T.ACID = A.ACCODE INNER JOIN " & cnAdminDb & "..ACGROUP AS G ON G.ACGRPCODE = A.ACGRPCODE" & vbCrLf
            strSql += " AND GRPLEDGER IN ('P')" & vbCrLf
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End If
        strSql = " IF EXISTS (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & userId.ToString() & "PROFITLOSS0')" & vbCrLf
        strSql += " DROP TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS0" & vbCrLf
        'strSql += " SELECT " & vbCrLf
        'strSql += "  'TRADING A/C' PARTICULARS" & vbTab + Environment.NewLine
        'strSql += "  ,ISNULL(SUM(ISNULL((CASE WHEN GRPTYPE = 'E' THEN ISNULL(DEBIT,0) - ISNULL(CREDIT,0) END),0)" & vbCrLf
        'strSql += "    - ISNULL((CASE WHEN GRPTYPE = 'I' THEN ISNULL(CREDIT,0) - ISNULL(DEBIT,0) END),0)),0) "
        'strSql += "    - ISNULL((SELECT SUM(VALUE) FROM " & cnStockDb & "..CLOSINGVAL WHERE COMPANYID = '" & strcompid & "'" & strCostId & "),0) "
        ''strSql += "    - ISNULL((SELECT SUM(AMOUNT) FROM " & cnStockDb & "..OPENWEIGHT WHERE COMPANYID = '" & strcompid & "'" & strCostId & " AND STOCKTYPE='C'),0) "
        'strSql += "    AMOUNT" & vbCrLf
        'strSql += " INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS0" & vbCrLf
        'strSql += " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS1 AS T INNER JOIN " & cnAdminDb & "..ACHEAD AS A" & vbCrLf
        'strSql += " ON T.ACID = A.ACCODE INNER JOIN " & cnAdminDb & "..ACGROUP AS G ON G.ACGRPCODE = A.ACGRPCODE" & vbCrLf
        'strSql += " AND GRPLEDGER IN ('T')" & vbCrLf
        strSql += vbCrLf + "SELECT 'TRADING A/C' PARTICULARS,SUM(AMOUNT) AMOUNT INTO  TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS0 FROM  (  "
        strSql += vbCrLf + "SELECT SUM(CONVERT(NUMERIC(15,2),ISNULL(INCOME,0)))AMOUNT"
        strSql += vbCrLf + "FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE5 WHERE RESULT4 IN ('0','7')"
        strSql += vbCrLf + "UNION ALL"
        strSql += vbCrLf + "SELECT SUM(CONVERT(NUMERIC(15,2),ISNULL(EXPENSES,0)))*-1 AMOUNT"
        strSql += vbCrLf + "FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "TRADE5 WHERE RESULT3 IN ('0','7'))X"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " IF EXISTS (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & userId.ToString() & "PROFITLOSS3')" & vbCrLf
        strSql += " DROP TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS3" & vbCrLf
        strSql += " CREATE TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS3" & vbCrLf
        strSql += " (EXPACGROUPNAME1 VARCHAR(250),EXPENSES1 VARCHAR(50),EXPENSES2 VARCHAR(50),EXPENSES VARCHAR(50)" & vbCrLf
        strSql += " ,INCACGROUPNAME1 VARCHAR(250),INCOME1 VARCHAR(50),INCOME2 VARCHAR(50),INCOME VARCHAR(50)" & vbCrLf
        strSql += " ,RESULT INT,SEP INT,EXPACMAINCODE VARCHAR(50),INCACMAINCODE VARCHAR(50),EXPACGROUP VARCHAR(250),INCACGROUP VARCHAR(250)" & vbCrLf
        strSql += " ,EXPACID VARCHAR(50),EXPACNAME VARCHAR(250),EXPACGROUPNAME VARCHAR(250),EXPACMAINGRPNAME VARCHAR(250)" & vbCrLf
        strSql += " ,INCACID VARCHAR(50),INCACNAME VARCHAR(250),INCACGROUPNAME VARCHAR(250),INCACMAINGRPNAME VARCHAR(250),SNO INT IDENTITY(1,1),COLHEAD VARCHAR(2)" & vbCrLf
        strSql += " )" & vbCrLf
        strSql += " " & vbCrLf
        strSql += " IF EXISTS (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & userId.ToString() & "PROFITLOSS4')" & vbCrLf
        strSql += " DROP TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS4" & vbCrLf
        strSql += " CREATE TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS4" & vbCrLf
        strSql += " (EXPACGROUPNAME1 VARCHAR(250),EXPENSES1 VARCHAR(50),EXPENSES2 VARCHAR(50),EXPENSES VARCHAR(50)" & vbCrLf
        strSql += " ,INCACGROUPNAME1 VARCHAR(250),INCOME1 VARCHAR(50),INCOME2 VARCHAR(50),INCOME VARCHAR(50)" & vbCrLf
        strSql += " ,RESULT INT,SEP INT,EXPACMAINCODE VARCHAR(50),INCACMAINCODE VARCHAR(50),EXPACGROUP VARCHAR(250),INCACGROUP VARCHAR(250)" & vbCrLf
        strSql += " ,EXPACID VARCHAR(50),EXPACNAME VARCHAR(250),EXPACGROUPNAME VARCHAR(250),EXPACMAINGRPNAME VARCHAR(250)" & vbCrLf
        strSql += " ,INCACID VARCHAR(50),INCACNAME VARCHAR(250),INCACGROUPNAME VARCHAR(250),INCACMAINGRPNAME VARCHAR(250),SNO INT IDENTITY(1,1),COLHEAD VARCHAR(2)" & vbCrLf
        strSql += " )" & vbCrLf
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()


        If summary Then
            strSql = " INSERT INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS3" & vbCrLf
            strSql += " (EXPACGROUPNAME1,EXPENSES,INCACGROUPNAME1,INCOME,RESULT,SEP,EXPACMAINCODE,INCACMAINCODE,EXPACGROUP,INCACGROUP,EXPACID" & vbCrLf
            strSql += " ,EXPACNAME,EXPACGROUPNAME,EXPACMAINGRPNAME,INCACID,INCACNAME,INCACGROUPNAME,INCACMAINGRPNAME,COLHEAD)" & vbCrLf
            strSql += vbCrLf + "  SELECT "
            strSql += vbCrLf + "   CASE WHEN " & Val(OpenPL.ToString).ToString & " < 0 THEN 'OPENING BALANCE' ELSE NULL END EXPACGROUPNAME1"
            strSql += vbCrLf + " ,CASE WHEN " & Val(OpenPL.ToString).ToString & " < 0 THEN -1*" & Val(OpenPL.ToString).ToString & " ELSE NULL END EXPENSES"
            strSql += vbCrLf + " ,CASE WHEN " & Val(OpenPL.ToString).ToString & " > 0 THEN 'OPENING BALANCE' ELSE NULL END INCACGROUPNAME1"
            strSql += vbCrLf + " ,CASE WHEN " & Val(OpenPL.ToString).ToString & " > 0 THEN " & Val(OpenPL.ToString).ToString & " ELSE NULL END INCOME,'3' RESULT,'2' SEP"
            strSql += vbCrLf + " ,'' EXPACMAINCODE,'' INCACMAINCODE,'' EXPACGROUP,'' INCACGROUP"
            strSql += vbCrLf + " ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,'' EXPACMAINGRPNAME"
            strSql += vbCrLf + " ,''INCACID,''INCACNAME,''INCACGROUPNAME,'' INCACMAINGRPNAME,'T1' COLHEAD	"
            strSql += vbCrLf + "  UNION ALL "
            strSql += " SELECT " & vbCrLf
            strSql += "   PARTICULARS EXPACGROUPNAME1" & vbCrLf
            strSql += " ,ABS(AMOUNT) EXPENSES" & vbCrLf
            strSql += " ,NULL INCACGROUPNAME1" & vbCrLf
            strSql += " ,NULL INCOME,'3' RESULT,'2' SEP" & vbCrLf
            strSql += " ,'' EXPACMAINCODE,'' INCACMAINCODE,'' EXPACGROUP,'' INCACGROUP" & vbCrLf
            strSql += " ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,'' EXPACMAINGRPNAME" & vbCrLf
            strSql += " ,''INCACID,''INCACNAME,''INCACGROUPNAME,'' INCACMAINGRPNAME,'T1' COLHEAD" & vbCrLf
            strSql += " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS0 WHERE AMOUNT <= 0" & vbCrLf
            strSql += " UNION ALL" & vbCrLf
            strSql += " SELECT " & vbCrLf
            strSql += " DISTINCT ACGRPNAME EXPACGROUPNAME1" & vbCrLf
            strSql += "  ,CONVERT(VARCHAR,ISNULL(SUM(DEBIT),0) - ISNULL(SUM(CREDIT),0)) EXPENSES" & vbCrLf
            strSql += " ,NULL INCACGROUPNAME1" & vbCrLf
            strSql += " ,NULL INCOME,'3' RESULT,'2' SEP" & vbCrLf
            strSql += " ,ACMAINCODE EXPACMAINCODE,'' INCACMAINCODE,ACGRPCODE EXPACGROUP,'' INCACGROUP" & vbCrLf
            strSql += " ,ACID EXPACID,ACNAME EXPACNAME,ACGRPNAME EXPACGROUPNAME,ACMAINGRPNAME EXPACMAINGRPNAME" & vbCrLf
            strSql += " ,''INCACID,''INCACNAME,''INCACGROUPNAME,'' INCACMAINGRPNAME,'' COLHEAD" & vbCrLf
            strSql += " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS2 WHERE ISNULL(GRPTYPE,'') = 'E'" & vbCrLf
            strSql += " GROUP BY ACMAINGRPNAME,ACGRPNAME,ACMAINCODE,ACGRPCODE,ACID,ACNAME" & vbCrLf
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        Else
            strSql = " INSERT INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS3" & vbCrLf
            strSql += " (EXPACGROUPNAME1,EXPENSES,INCACGROUPNAME1,INCOME,RESULT,SEP,EXPACMAINCODE,INCACMAINCODE,EXPACGROUP,INCACGROUP,EXPACID" & vbCrLf
            strSql += " ,EXPACNAME,EXPACGROUPNAME,EXPACMAINGRPNAME,INCACID,INCACNAME,INCACGROUPNAME,INCACMAINGRPNAME,COLHEAD)" & vbCrLf
            strSql += vbCrLf + "  SELECT "
            strSql += vbCrLf + "   CASE WHEN " & Val(OpenPL.ToString).ToString & " < 0 THEN 'OPENING BALANCE' ELSE NULL END EXPACGROUPNAME1"
            strSql += vbCrLf + " ,CASE WHEN " & Val(OpenPL.ToString).ToString & " < 0 THEN -1*" & Val(OpenPL.ToString).ToString & " ELSE NULL END EXPENSES"
            strSql += vbCrLf + " ,CASE WHEN " & Val(OpenPL.ToString).ToString & " > 0 THEN 'OPENING BALANCE' ELSE NULL END INCACGROUPNAME1"
            strSql += vbCrLf + " ,CASE WHEN " & Val(OpenPL.ToString).ToString & " > 0 THEN " & Val(OpenPL.ToString).ToString & " ELSE NULL END INCOME,'3' RESULT,'2' SEP"
            strSql += vbCrLf + " ,'' EXPACMAINCODE,'' INCACMAINCODE,'' EXPACGROUP,'' INCACGROUP"
            strSql += vbCrLf + " ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,'' EXPACMAINGRPNAME"
            strSql += vbCrLf + " ,''INCACID,''INCACNAME,''INCACGROUPNAME,'' INCACMAINGRPNAME,'T1' COLHEAD	"
            strSql += vbCrLf + "  UNION ALL "
            strSql += " SELECT " & vbCrLf
            strSql += "  CASE WHEN AMOUNT < 0 THEN PARTICULARS ELSE NULL END  EXPACGROUPNAME1" & vbCrLf
            strSql += " ,CASE WHEN AMOUNT < 0 THEN AMOUNT ELSE NULL END  EXPENSES" & vbCrLf
            strSql += " ,NULL INCACGROUPNAME1" & vbCrLf
            strSql += " ,NULL INCOME,'3' RESULT,'2' SEP" & vbCrLf
            strSql += " ,'' EXPACMAINCODE,'' INCACMAINCODE,'' EXPACGROUP,'' INCACGROUP" & vbCrLf
            strSql += " ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,'' EXPACMAINGRPNAME" & vbCrLf
            strSql += " ,''INCACID,''INCACNAME,''INCACGROUPNAME,'' INCACMAINGRPNAME,'T1' COLHEAD" & vbCrLf
            strSql += " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS0 " & vbCrLf
            strSql += " UNION ALL" & vbCrLf
            strSql += " SELECT " & vbCrLf
            strSql += " DISTINCT ACGRPNAME EXPACGROUPNAME1" & vbCrLf
            strSql += " ,NULL EXPENSES" & vbCrLf
            strSql += " ,NULL INCACGROUPNAME1" & vbCrLf
            strSql += " ,NULL INCOME,'3' RESULT,'2' SEP" & vbCrLf
            strSql += " ,ACMAINCODE EXPACMAINCODE,'' INCACMAINCODE,ACGRPCODE EXPACGROUP,'' INCACGROUP" & vbCrLf
            strSql += " ,'' EXPACID,'' EXPACNAME,ACGRPNAME EXPACGROUPNAME,ACMAINGRPNAME EXPACMAINGRPNAME" & vbCrLf
            strSql += " ,''INCACID,''INCACNAME,''INCACGROUPNAME,'' INCACMAINGRPNAME,'T1' COLHEAD" & vbCrLf
            strSql += " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS2 WHERE ISNULL(GRPTYPE,'') = 'E'" & vbCrLf
            strSql += " GROUP BY ACMAINGRPNAME,ACGRPNAME,ACMAINCODE,ACGRPCODE" & vbCrLf
            strSql += " UNION ALL" & vbCrLf
            strSql += " SELECT " & vbCrLf
            strSql += " DISTINCT '  '+ACNAME EXPACGROUPNAME1" & vbCrLf
            strSql += " ,CONVERT(VARCHAR,ISNULL(SUM(DEBIT),0) - ISNULL(SUM(CREDIT),0)) EXPENSES" & vbCrLf
            strSql += " ,NULL INCACGROUPNAME1" & vbCrLf
            strSql += " ,NULL INCOME,'4' RESULT,'2' SEP" & vbCrLf
            strSql += " ,ACMAINCODE EXPACMAINCODE,'' INCACMAINCODE,ACGRPCODE EXPACGROUP,'' INCACGROUP" & vbCrLf
            strSql += " ,ACID EXPACID,ACNAME EXPACNAME,ACGRPNAME EXPACGROUPNAME,ACMAINGRPNAME EXPACMAINGRPNAME" & vbCrLf
            strSql += " ,''INCACID,''INCACNAME,''INCACGROUPNAME,'' INCACMAINGRPNAME,'' COLHEAD" & vbCrLf
            strSql += " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS2 WHERE ISNULL(GRPTYPE,'') = 'E'" & vbCrLf
            strSql += " GROUP BY ACMAINGRPNAME,ACGRPNAME,ACMAINCODE,ACGRPCODE,ACID,ACNAME" & vbCrLf
            strSql += " ORDER BY EXPACGROUP,SEP,RESULT,INCACNAME" & vbCrLf
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End If

        If summary Then
            strSql = " INSERT INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS4" & vbCrLf
            strSql += " (EXPACGROUPNAME1,EXPENSES,INCACGROUPNAME1,INCOME,RESULT,SEP,EXPACMAINCODE,INCACMAINCODE,EXPACGROUP,INCACGROUP,EXPACID" & vbCrLf
            strSql += " ,EXPACNAME,EXPACGROUPNAME,EXPACMAINGRPNAME,INCACID,INCACNAME,INCACGROUPNAME,INCACMAINGRPNAME,COLHEAD)" & vbCrLf
            strSql += vbCrLf + "  SELECT "
            strSql += vbCrLf + "   CASE WHEN " & Val(OpenPL.ToString).ToString & " < 0 THEN 'OPENING BALANCE' ELSE NULL END EXPACGROUPNAME1"
            strSql += vbCrLf + " ,CASE WHEN " & Val(OpenPL.ToString).ToString & " < 0 THEN -1*" & Val(OpenPL.ToString).ToString & " ELSE NULL END EXPENSES"
            strSql += vbCrLf + " ,CASE WHEN " & Val(OpenPL.ToString).ToString & " > 0 THEN 'OPENING BALANCE' ELSE NULL END INCACGROUPNAME1"
            strSql += vbCrLf + " ,CASE WHEN " & Val(OpenPL.ToString).ToString & " > 0 THEN " & Val(OpenPL.ToString).ToString & " ELSE NULL END INCOME,'3' RESULT,'2' SEP"
            strSql += vbCrLf + " ,'' EXPACMAINCODE,'' INCACMAINCODE,'' EXPACGROUP,'' INCACGROUP"
            strSql += vbCrLf + " ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,'' EXPACMAINGRPNAME"
            strSql += vbCrLf + " ,''INCACID,''INCACNAME,''INCACGROUPNAME,'' INCACMAINGRPNAME,'T1' COLHEAD	"
            strSql += vbCrLf + "  UNION ALL "
            strSql += " SELECT " & vbCrLf
            strSql += "   NULL EXPACGROUPNAME1" & vbCrLf
            strSql += " ,NULL EXPENSES" & vbCrLf
            strSql += " ,PARTICULARS INCACGROUPNAME1" & vbCrLf
            strSql += " ,ABS(AMOUNT) INCOME,'3' RESULT,'2' SEP" & vbCrLf
            strSql += " ,'' EXPACMAINCODE,'' INCACMAINCODE,'' EXPACGROUP,'' INCACGROUP" & vbCrLf
            strSql += " ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,'' EXPACMAINGRPNAME" & vbCrLf
            strSql += " ,''INCACID,''INCACNAME,''INCACGROUPNAME,'' INCACMAINGRPNAME,'T1' COLHEAD" & vbTab + Environment.NewLine
            strSql += " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS0 WHERE AMOUNT > 0" & vbCrLf
            strSql += " UNION ALL" & vbCrLf
            strSql += " SELECT " & vbCrLf
            strSql += " DISTINCT '' EXPACGROUPNAME1" & vbCrLf
            strSql += " ,NULL EXPENSES" & vbCrLf
            strSql += " ,ACGRPNAME INCACGROUPNAME1" & vbCrLf
            strSql += " ,CONVERT(VARCHAR,ISNULL(SUM(CREDIT),0) - ISNULL(SUM(DEBIT),0)) INCOME,'3' RESULT,'2' SEP" & vbCrLf
            strSql += " ,'' EXPACMAINCODE,ACMAINCODE INCACMAINCODE,'' EXPACGROUP,ACGRPCODE INCACGROUP" & vbCrLf
            strSql += " ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,'' EXPACMAINGRPNAME" & vbCrLf
            strSql += " ,ACID INCACID,ACNAME INCACNAME,ACGRPNAME INCACGROUPNAME,ACMAINGRPNAME INCACMAINGRPNAME,'' COLHEAD" & vbTab + Environment.NewLine
            strSql += " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS2 WHERE ISNULL(GRPTYPE,'') = 'I'" & vbCrLf
            strSql += " GROUP BY ACMAINGRPNAME,ACGRPNAME,ACMAINCODE,ACGRPCODE,ACID,ACNAME" & vbCrLf
            strSql += " ORDER BY INCACMAINGRPNAME,SEP,INCACGROUPNAME,RESULT,INCACNAME" & vbCrLf
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        Else
            strSql = " INSERT INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS4" & vbCrLf
            strSql += " (EXPACGROUPNAME1,EXPENSES,INCACGROUPNAME1,INCOME,RESULT,SEP,EXPACMAINCODE,INCACMAINCODE,EXPACGROUP,INCACGROUP,EXPACID" & vbCrLf
            strSql += " ,EXPACNAME,EXPACGROUPNAME,EXPACMAINGRPNAME,INCACID,INCACNAME,INCACGROUPNAME,INCACMAINGRPNAME,COLHEAD)" & vbCrLf
            strSql += vbCrLf + "  SELECT "
            strSql += vbCrLf + "   CASE WHEN " & Val(OpenPL.ToString).ToString & " < 0 THEN 'OPENING BALANCE' ELSE NULL END EXPACGROUPNAME1"
            strSql += vbCrLf + " ,CASE WHEN " & Val(OpenPL.ToString).ToString & " < 0 THEN -1*" & Val(OpenPL.ToString).ToString & " ELSE NULL END EXPENSES"
            strSql += vbCrLf + " ,CASE WHEN " & Val(OpenPL.ToString).ToString & " > 0 THEN 'OPENING BALANCE' ELSE NULL END INCACGROUPNAME1"
            strSql += vbCrLf + " ,CASE WHEN " & Val(OpenPL.ToString).ToString & " > 0 THEN " & Val(OpenPL.ToString).ToString & " ELSE NULL END INCOME,'3' RESULT,'2' SEP"
            strSql += vbCrLf + " ,'' EXPACMAINCODE,'' INCACMAINCODE,'' EXPACGROUP,'' INCACGROUP"
            strSql += vbCrLf + " ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,'' EXPACMAINGRPNAME"
            strSql += vbCrLf + " ,''INCACID,''INCACNAME,''INCACGROUPNAME,'' INCACMAINGRPNAME,'T1' COLHEAD	"
            strSql += vbCrLf + "  UNION ALL "
            strSql += " SELECT " & vbCrLf
            strSql += "   NULL EXPACGROUPNAME1" & vbCrLf
            strSql += " ,NULL EXPENSES" & vbCrLf
            'strSql += " ,PARTICULARS INCACGROUPNAME1" & vbCrLf
            'strSql += " ,ABS(AMOUNT) INCOME,'3' RESULT,'2' SEP" & vbCrLf
            strSql += " ,CASE WHEN AMOUNT > 0 THEN PARTICULARS ELSE NULL END INCACGROUPNAME1" & vbCrLf
            strSql += " ,CASE WHEN AMOUNT > 0 THEN AMOUNT ELSE NULL END INCOME,'3' RESULT,'2' SEP" & vbCrLf
            strSql += " ,'' EXPACMAINCODE,'' INCACMAINCODE,'' EXPACGROUP,'' INCACGROUP" & vbCrLf
            strSql += " ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,'' EXPACMAINGRPNAME" & vbCrLf
            strSql += " ,''INCACID,''INCACNAME,''INCACGROUPNAME,'' INCACMAINGRPNAME,'T1' COLHEAD" & vbTab + Environment.NewLine
            strSql += " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS0 " & vbCrLf
            strSql += " UNION ALL" & vbCrLf
            strSql += " SELECT " & vbCrLf
            strSql += " DISTINCT '' EXPACGROUPNAME1" & vbCrLf
            strSql += " ,NULL EXPENSES" & vbCrLf
            strSql += " ,ACGRPNAME INCACGROUPNAME1" & vbCrLf
            strSql += " ,NULL INCOME,'3' RESULT,'2' SEP" & vbCrLf
            strSql += " ,'' EXPACMAINCODE,ACMAINCODE INCACMAINCODE,'' EXPACGROUP,ACGRPCODE INCACGROUP" & vbCrLf
            strSql += " ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,'' EXPACMAINGRPNAME" & vbCrLf
            strSql += " ,'' INCACID,'' INCACNAME,ACGRPNAME INCACGROUPNAME,ACMAINGRPNAME INCACMAINGRPNAME,'T1' COLHEAD" & vbTab + Environment.NewLine
            strSql += " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS2 WHERE ISNULL(GRPTYPE,'') = 'I'" & vbCrLf
            strSql += " GROUP BY ACMAINGRPNAME,ACGRPNAME,ACMAINCODE,ACGRPCODE" & vbCrLf
            strSql += " UNION ALL" & vbCrLf
            strSql += " SELECT " & vbCrLf
            strSql += " DISTINCT '' EXPACGROUPNAME1" & vbCrLf
            strSql += " ,NULL EXPENSES" & vbCrLf
            strSql += " ,ACNAME INCACGROUPNAME1" & vbCrLf
            strSql += " ,CONVERT(VARCHAR,ISNULL(SUM(CREDIT),0) - ISNULL(SUM(DEBIT),0)) INCOME,'3' RESULT,'2' SEP" & vbCrLf
            strSql += " ,'' EXPACMAINCODE,ACMAINCODE INCACMAINCODE,'' EXPACGROUP,ACGRPCODE INCACGROUP" & vbCrLf
            strSql += " ,'' EXPACID,'' EXPACNAME,'' EXPACGROUPNAME,'' EXPACMAINGRPNAME" & vbCrLf
            strSql += " ,ACID INCACID,ACNAME INCACNAME,ACGRPNAME INCACGROUPNAME,ACMAINGRPNAME INCACMAINGRPNAME,'' COLHEAD" & vbTab + Environment.NewLine
            strSql += " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS2 WHERE ISNULL(GRPTYPE,'') = 'I'" & vbCrLf
            strSql += " GROUP BY ACMAINGRPNAME,ACGRPNAME,ACMAINCODE,ACGRPCODE,ACID,ACNAME" & vbCrLf
            strSql += " ORDER BY INCACMAINGRPNAME,SEP,INCACGROUPNAME,RESULT,INCACNAME" & vbCrLf
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End If

        strSql = " IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & userId.ToString() & "PROFITLOSS5')" & vbCrLf
        strSql += " DROP TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS5" & vbCrLf
        strSql += " SELECT " & vbCrLf
        strSql += " T3.EXPACGROUPNAME1,T3.EXPENSES,T4.INCACGROUPNAME1,T4.INCOME" & vbCrLf
        strSql += " ,T3.SNO SNO3,T4.SNO SNO4,T3.RESULT RESULT3,T4.RESULT RESULT4,T3.SEP SEP3,T4.SEP SEP4,T3.COLHEAD COLHEAD3,T4.COLHEAD COLHEAD4" & vbCrLf
        strSql += " INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS5" & vbCrLf
        strSql += " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS3 AS T3 FULL JOIN TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS4 AS T4 " & vbCrLf
        strSql += " ON T3.SNO = T4.SNO" & vbCrLf
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " INSERT INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS5"
        strSql += " (EXPACGROUPNAME1,EXPENSES,INCACGROUPNAME1,INCOME,COLHEAD3,COLHEAD4)"
        strSql += " SELECT  CASE WHEN ISNULL(SUM(CONVERT(NUMERIC(15,2),INCOME)),0) - ISNULL(SUM(CONVERT(NUMERIC(15,2),EXPENSES)),0) > 0 THEN 'NET PROFIT' END EXPACGROUPNAME1  "
        strSql += " ,CONVERT(VARCHAR,CASE WHEN ISNULL(SUM(CONVERT(NUMERIC(15,2),INCOME)),0) - ISNULL(SUM(CONVERT(NUMERIC(15,2),EXPENSES)),0) > 0 "
        strSql += " THEN ISNULL(SUM(CONVERT(NUMERIC(15,2),INCOME)),0) - ISNULL(SUM(CONVERT(NUMERIC(15,2),EXPENSES)),0) END) EXPENSES "
        strSql += " ,CASE WHEN ISNULL(SUM(CONVERT(NUMERIC(15,2),EXPENSES)),0) - ISNULL(SUM(CONVERT(NUMERIC(15,2),INCOME)),0) > 0 THEN 'NET LOSS' END INCACGROUPNAME1 "
        strSql += " ,CONVERT(VARCHAR,CASE WHEN ISNULL(SUM(CONVERT(NUMERIC(15,2),EXPENSES)),0) - ISNULL(SUM(CONVERT(NUMERIC(15,2),INCOME)),0) > 0 "
        strSql += " THEN ISNULL(SUM(CONVERT(NUMERIC(15,2),EXPENSES)),0) - ISNULL(SUM(CONVERT(NUMERIC(15,2),INCOME)),0) END) INCOME"
        strSql += " ,'G' COLHEAD3,'G' COLHEAD4 FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS5"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " SELECT " & vbCrLf
        strSql += " DISTINCT 'P & L - 01/" & CnStartMonth & "/" & (IIf(dtpDate.Value.[Date].Month > 3, dtpDate.Value.[Date].Year, dtpDate.Value.[Date].Year - 1)).ToString() & " - " & dtpDate.Text.Replace("-", "/") & "' EXPACGROUPNAME1,' 'EXPENSES1,' ' EXPENSES2,' 'EXPENSES " & vbCrLf
        strSql += " ,' ' INCACGROUPNAME1,' 'INCOME1,' 'INCOME2,' 'INCOME,'T' COLHEAD3,'T' COLHEAD4" & vbCrLf
        strSql += " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS5" & vbCrLf
        strSql += " UNION ALL" & vbCrLf
        strSql += " SELECT " & vbCrLf
        strSql += " EXPACGROUPNAME1,' 'EXPENSES1,' ' EXPENSES2," & cnStockDb & ".[DBO].FUNC_RUPEES_WITHCOMMA(CASE WHEN ISNULL(EXPENSES,'') = '' THEN '0' ELSE EXPENSES END,'D','Y') EXPENSES " & vbCrLf
        strSql += " ,INCACGROUPNAME1,' 'INCOME1,' 'INCOME2," & cnStockDb & ".[DBO].FUNC_RUPEES_WITHCOMMA(CASE WHEN ISNULL(INCOME,'') = '' THEN '0' ELSE INCOME END,'D','Y') INCOME,COLHEAD3,COLHEAD4" & vbCrLf
        strSql += " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS5" & vbCrLf
        strSql += " UNION ALL" & vbCrLf
        strSql += " SELECT" & vbCrLf
        strSql += "  DISTINCT '' EXPACGROUPNAME1 ,'' EXPENSES1,'' EXPENSES2,'-------------------' EXPENSES" & vbCrLf
        strSql += " ,'' INCACGROUPNAME1 ,'' INCOME1,'' INCOME2,'-------------------' INCOME,'G1' COLHEAD3,'G1' COLHEAD4" & vbCrLf
        strSql += " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS5" & vbCrLf
        strSql += " UNION ALL" & vbCrLf
        strSql += " SELECT" & vbCrLf
        strSql += "   '' EXPACGROUPNAME1 ,'' EXPENSES1,'' EXPENSES2,CONVERT(VARCHAR," & cnStockDb & ".[DBO].FUNC_RUPEES_WITHCOMMA(SUM(CONVERT(NUMERIC(15,2),EXPENSES)),'D','Y')) EXPENSES" & vbCrLf
        strSql += " ,'' INCACGROUPNAME1 ,'' INCOME1,'' INCOME2,CONVERT(VARCHAR," & cnStockDb & ".[DBO].FUNC_RUPEES_WITHCOMMA(SUM(CONVERT(NUMERIC(15,2),INCOME)),'D','Y')) INCOME,'G1' COLHEAD3,'G1' COLHEAD4" & vbCrLf
        strSql += " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS5" & vbCrLf
        strSql += " UNION ALL" & vbCrLf
        strSql += " SELECT" & vbCrLf
        strSql += "  DISTINCT '' EXPACGROUPNAME1 ,'' EXPENSES1,'' EXPENSES2,'-------------------' EXPENSES" & vbCrLf
        strSql += " ,'' INCACGROUPNAME1 ,'' INCOME1,'' INCOME2,'-------------------' INCOME,'G1' COLHEAD3,'G1' COLHEAD4" & vbCrLf
        strSql += " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS5" & vbCrLf
        'strSql += " UNION ALL" & vbCrLf
        'strSql += " SELECT" & vbCrLf
        'strSql += "   CASE WHEN ISNULL(SUM(CONVERT(NUMERIC(15,2),EXPENSES)),0) - ISNULL(SUM(CONVERT(NUMERIC(15,2),INCOME)),0) > 0 THEN 'GROSS LOSS' END EXPACGROUPNAME1 ,'' EXPENSES1,'' EXPENSES2" & vbCrLf
        'strSql += " ,CONVERT(VARCHAR,CASE WHEN ISNULL(SUM(CONVERT(NUMERIC(15,2),EXPENSES)),0) - ISNULL(SUM(CONVERT(NUMERIC(15,2),INCOME)),0) > 0 THEN " & cnStockDb & ".[DBO].FUNC_RUPEES_WITHCOMMA(ISNULL(SUM(CONVERT(NUMERIC(15,2),EXPENSES)),0) - ISNULL(SUM(CONVERT(NUMERIC(15,2),INCOME)),0),'D','Y') END) EXPENSES" & vbCrLf
        'strSql += " ,CASE WHEN ISNULL(SUM(CONVERT(NUMERIC(15,2),INCOME)),0) - ISNULL(SUM(CONVERT(NUMERIC(15,2),EXPENSES)),0) > 0 THEN 'GROSS PROFIT' END INCACGROUPNAME1 ,'' INCOME1,'' INCOME2 " & vbCrLf
        'strSql += " ,CONVERT(VARCHAR,CASE WHEN ISNULL(SUM(CONVERT(NUMERIC(15,2),INCOME)),0) - ISNULL(SUM(CONVERT(NUMERIC(15,2),EXPENSES)),0) > 0 THEN " & cnStockDb & ".[DBO].FUNC_RUPEES_WITHCOMMA(ISNULL(SUM(CONVERT(NUMERIC(15,2),INCOME)),0) - ISNULL(SUM(CONVERT(NUMERIC(15,2),EXPENSES)),0),'D','Y') END) INCOME,'G' COLHEAD3,'G' COLHEAD4" & vbCrLf
        'strSql += " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "PROFITLOSS5" & vbCrLf
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)

    End Sub





    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        If cmbCompany.Items.Count > 0 Then cmbCompany.SelectedIndex = 0
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        If Closin_stk = "L" Then
            pnlClosingStock.Visible = False
            rbtCls_Stock_LIFO.Checked = True
        ElseIf Closin_stk = "N" Then
            pnlClosingStock.Visible = True
            rbtCls_Stock_Man.Checked = True
        ElseIf Closin_stk = "W" Then
            pnlClosingStock.Visible = True
            rbtCls_Stock_AvgRate.Checked = True
        End If
        Dim Enddate As Date = Nothing
        Dim dt As New DataTable
        strSql = "SELECT ENDDATE FROM " & cnAdminDb & "..DBMASTER WHERE DBNAME = '" & cnStockDb & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        Enddate = dt.Rows(0).Item(0).ToString
        If Enddate < DateTime.Today.Date Then
            dtpDate.Value = Enddate
            dtpToDate.Value = Enddate
        Else
            dtpDate.Value = DateTime.Today.Date
            dtpToDate.Value = DateTime.Today.Date
        End If

        dtpToDate.Enabled = False
        chkAsOn.Text = "As On"
        chkAsOn.Checked = True

        gridView.DataSource = Nothing
        lblTitle.Text = ""
        CnStartMonth = Format(cnTranFromDate.Month, "00")
        Prop_Gets()
        cmbCompany.Focus()
    End Sub
    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.RowCount > 0 Then
            BrightPosting.GExport.Post(Me.Name, "", lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export, gridViewHead)
        End If
    End Sub
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.RowCount > 0 Then
            BrightPosting.GExport.Post(Me.Name, "", lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print, gridViewHead)
        End If
    End Sub
    Private Sub viewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles viewToolStripMenuItem.Click
        btnView_Click(Me, New EventArgs)
    End Sub
    Private Sub newToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles newToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub
    Private Sub exitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles exitToolStripMenuItem.Click
        btnExit_Click(Me, New EventArgs)
    End Sub
    Private Sub Prop_Sets()
        Dim obj As New frmTrading_Properties
        obj.p_cmbCompany = cmbCompany.Text
        'SetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre, "ALL")
        SetSettingsObj(obj, Me.Name, GetType(frmTradingNew_Properties))
    End Sub
    Private Sub Prop_Gets()
        Dim obj As New frmTrading_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmTradingNew_Properties))
        cmbCompany.Text = obj.p_cmbCompany
        'GetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre)
    End Sub

    Private Sub gridView_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs) Handles gridView.ColumnWidthChanged
        With gridViewHead
            If .ColumnCount > 0 Then
                For i As Integer = 0 To DtCostid.Rows.Count - 1
                    .Columns("EXPACGROUPNAME1" & DtCostid.Rows(i).Item(0).ToString.Trim & "~EXPENSES" & DtCostid.Rows(i).Item(0).ToString.Trim & "~EXPENSES1" & DtCostid.Rows(i).Item(0).ToString.Trim & "~EXPENSES2" & DtCostid.Rows(i).Item(0).ToString.Trim & "~INCACGROUPNAME1" & DtCostid.Rows(i).Item(0).ToString.Trim & "~INCOME" & DtCostid.Rows(i).Item(0).ToString.Trim & "~INCOME1" & DtCostid.Rows(i).Item(0).ToString.Trim & "~INCOME2" & DtCostid.Rows(i).Item(0).ToString.Trim).Width = gridView.Columns("EXPACGROUPNAME1" & DtCostid.Rows(i).Item(0).ToString.Trim).Width + gridView.Columns("EXPENSES" & DtCostid.Rows(i).Item(0).ToString.Trim).Width + gridView.Columns("EXPENSES1" & DtCostid.Rows(i).Item(0).ToString.Trim).Width + gridView.Columns("EXPENSES2" & DtCostid.Rows(i).Item(0).ToString.Trim).Width + gridView.Columns("INCACGROUPNAME1" & DtCostid.Rows(i).Item(0).ToString.Trim).Width + gridView.Columns("INCOME" & DtCostid.Rows(i).Item(0).ToString.Trim).Width + gridView.Columns("INCOME1" & DtCostid.Rows(i).Item(0).ToString.Trim).Width + gridView.Columns("INCOME2" & DtCostid.Rows(i).Item(0).ToString.Trim).Width
                    .Columns("COLHEAD3" & DtCostid.Rows(i).Item(0).ToString.Trim).Width = gridView.Columns("COLHEAD3" & DtCostid.Rows(i).Item(0).ToString.Trim).Width
                    .Columns("COLHEAD4" & DtCostid.Rows(i).Item(0).ToString.Trim).Width = gridView.Columns("COLHEAD4" & DtCostid.Rows(i).Item(0).ToString.Trim).Width
                Next
                .Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
                .Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
            End If
        End With
    End Sub

    Private Sub gridView_Scroll(ByVal sender As Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles gridView.Scroll
        If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
            gridViewHead.HorizontalScrollingOffset = e.NewValue
        End If
        If gridViewHead.DataSource Is Nothing Then Exit Sub
        Try
            If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
                gridViewHead.HorizontalScrollingOffset = e.NewValue
                gridViewHead.Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
                gridViewHead.Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try
    End Sub

    Private Sub chkAsOn_CheckedChanged(sender As Object, e As EventArgs) Handles chkAsOn.CheckedChanged
        If chkAsOn.Checked Then
            dtpToDate.Enabled = False
            chkAsOn.Text = "As On"
        Else
            dtpToDate.Enabled = True
            chkAsOn.Text = "From"
        End If
    End Sub
End Class

Public Class frmTradingNew_Properties

    Private cmbCompany As String = strCompanyName
    Public Property p_cmbCompany() As String
        Get
            Return cmbCompany
        End Get
        Set(ByVal value As String)
            cmbCompany = value
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