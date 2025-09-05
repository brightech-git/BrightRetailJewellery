Imports System.Data.OleDb
Imports System.Threading
Imports System.IO
Public Class frmGSTReConcile
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim dsGridView As New DataSet
    Dim dtCostCentre As New DataTable
    Private Sub SalesAbs()
        Try
            Prop_Sets()
            gridView.DataSource = Nothing
            Dim Sysid As String = Trim(LSet(Mid(Guid.NewGuid.ToString, 1, InStr(Guid.NewGuid.ToString, "-") - 1), 10))
            Dim _Costid As String = ""
            Me.Refresh()

            strSql = " EXEC " & cnAdminDb & "..PROC_GSTRECONCILE"
            strSql += vbCrLf + " @DBNAME = '" & cnStockDb & "'"
            strSql += vbCrLf + " ,@FROMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@COMPANYID = '" & strCompanyId & "'"
            strSql += vbCrLf + " ,@METALNAME = '" & cmbMetal.Text & "'"
            strSql += vbCrLf + " ,@COSTNAME = '" & GetQryString(chkCmbCostCentre.Text).Replace("'", "") & "'"
            cmd = New OleDbCommand(strSql, cn)
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()


            strSql = "SELECT * FROM TEMPTABLEDB..TEMPGSTRPT "
            If rbtMisMatch.Checked Then
                strSql += vbCrLf + " WHERE DIFF<>0"
            ElseIf rbtState.Checked Then
                strSql += vbCrLf + " WHERE (STATE=COMPANYSTATE AND ITAX>0 AND RESULT<>3) OR "
                strSql += vbCrLf + " (STATE<>COMPANYSTATE AND STAX>0 AND RESULT<>3)"
            End If
            strSql += vbCrLf + " ORDER BY RESULT,INVDATE,TRANNO"

            Dim dtGrid As New DataTable
            dtGrid.Columns.Add("KEYNO", GetType(Integer))
            dtGrid.Columns("KEYNO").AutoIncrement = True
            dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
            dtGrid.Columns("KEYNO").AutoIncrementStep = 1
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtGrid)
            If Not dtGrid.Rows.Count > 0 Then
                MsgBox("Record Not Found", MsgBoxStyle.Information)
                dtpFrom.Select()
                Exit Sub
            End If
            dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
            gridView.DataSource = dtGrid
            FormatGridColumns(gridView, False, False, True, False)

            Dim dt As New DataTable
            dt = gridView.DataSource
            dt.AcceptChanges()
            Dim ros() As DataRow = Nothing
            If rbtMisMatch.Checked Then
                ros = dt.Select("DIFF<>0 AND RESULT<>3")
            ElseIf rbtState.Checked Then
                ros = dt.Select("STATE=COMPANYSTATE AND ITAX>0 AND RESULT<>3 ")
                If ros.Length = 0 Then
                    ros = dt.Select("STATE<>COMPANYSTATE AND STAX>0 AND RESULT<>3 ")
                End If
            End If
            'For cnt As Integer = 0 To ros.Length - 1
            '    gridView.Rows(Val(ros(cnt).Item("KEYNO").ToString)).Cells("DIFF").Style.BackColor = Color.Red
            '    gridView.Rows(Val(ros(cnt).Item("KEYNO").ToString)).Cells("DIFF").Style.Font = New Font("VERDANA", 9, FontStyle.Bold)
            'Next
            If Not ros Is Nothing Then
                If ros.Length > 0 Then
                    btnUpdate.Visible = True
                End If
            End If
            ros = dt.Select("COLHEAD = 'G'")
            For cnt As Integer = 0 To ros.Length - 1
                gridView.Rows(Val(ros(cnt).Item("KEYNO").ToString)).DefaultCellStyle = reportTotalStyle
            Next
            With gridView
                .Columns("BATCHNO").Visible = False
                .Columns("SNO").Visible = False
                .Columns("COSTID").Visible = False
                .Columns("KEYNO").Visible = False
                .Columns("RESULT").Visible = False
                .Columns("COLHEAD").Visible = False
                .Columns("TRANNO").Visible = False
                .Columns("IRATE").HeaderText = "Rate"
                .Columns("CRATE").HeaderText = "Rate"
                .Columns("SRATE").HeaderText = "Rate"
                .Columns("ITAX").HeaderText = "Tax"
                .Columns("CTAX").HeaderText = "Tax"
                .Columns("STAX").HeaderText = "Tax"
                .Columns("PARTICULAR").HeaderText = "TranNo"
                .Columns("STATE").HeaderText = "CUSTOMERSTATE"
                .Columns("INVDATE").HeaderText = "Date"
                .Columns("AMOUNT").HeaderText = "Amount"
                .Columns("DIFF").HeaderText = ""

                .Columns("INVDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                .Invalidate()
                For Each dgvCol As DataGridViewColumn In .Columns
                    dgvCol.Width = dgvCol.Width
                Next
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End With
            funcHeaderNew()
            FormatGridColumns(gridViewHead, False, False, True, False)

            Dim TITLE As String = ""

            TITLE += " GST RECONCILE FROM " & dtpFrom.Text & " TO " & dtpTo.Text & ""
            TITLE += "  AT " & chkCmbCostCentre.Text & ""
            lblTitle.Font = New Font("VERDANA", 9, FontStyle.Bold)
            lblTitle.Text = TITLE
            pnlHeading.Visible = True
            btnView_Search.Enabled = True
            gridView.Focus()
        Catch ex As Exception
            MsgBox(ex.Message & vbCrLf & ex.StackTrace, MsgBoxStyle.Information) : Exit Sub
        End Try
    End Sub
    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        If MessageBox.Show("Sure Want to Update?", "Update Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then Exit Sub
        Dim dt As New DataTable
        dt = gridView.DataSource
        dt.AcceptChanges()
        Dim Errorflag As Boolean = False
        Dim ros() As DataRow = Nothing
        Dim Sno As String = ""
        Dim CostId As String = ""
        Dim BillCostId As String = ""
        Dim Batchno As String = ""
        If rbtState.Checked = False Then
            ros = dt.Select("DIFF<>0 AND RESULT<>3")
            Dim Tax As Decimal = 0
            For cnt As Integer = 0 To ros.Length - 1
                Sno = ros(cnt).Item("SNO").ToString
                CostId = ros(cnt).Item("COSTID").ToString
                If Sno = "" Then Continue For
                Tax = Val(ros(cnt).Item("ITAX").ToString) + Val(ros(cnt).Item("CTAX").ToString) + Val(ros(cnt).Item("STAX").ToString)
                strSql = "UPDATE " & cnStockDb & "..ISSUE SET TAX=" & Tax & " WHERE SNO='" & Sno & "'"
                If cnHOCostId = cnCostId Then
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, CostId)
                Else
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnHOCostId)
                End If
                lblStatus.Text = "Processing " & (cnt + 1) & " of " & ros.Length
                lblStatus.Refresh()
            Next
        Else
            ros = dt.Select("STATE=COMPANYSTATE AND ITAX>0 AND RESULT<>3 ")
            If ros.Length = 0 Then
                ros = dt.Select("STATE<>COMPANYSTATE AND STAX>0 AND RESULT<>3 ")
            End If
            Dim Batchnos As New List(Of String)
            For cnt As Integer = 0 To ros.Length - 1
                lblStatus.Text = "Processing " & (cnt + 1) & " of " & ros.Length
                lblStatus.Refresh()
                Sno = ros(cnt).Item("SNO").ToString
                CostId = ros(cnt).Item("COSTID").ToString
                BillCostId = ros(cnt).Item("COSTID").ToString
                Batchno = ros(cnt).Item("BATCHNO").ToString
                If Batchnos.Contains(Batchno) Then Continue For
                Batchnos.Add(Batchno)
                Dim Billtype As String = ros(cnt).Item("BILLTYPE").ToString
                strSql = " SELECT TRANNO,TRANDATE,TRANTYPE,BATCHNO,AMOUNT,TAXPER,ISSSNO,STUDDED,SUM(TAXAMOUNT)TAXAMOUNT FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO='" & Batchno & "' "
                strSql += " GROUP BY TRANNO,TRANDATE,TRANTYPE,BATCHNO,AMOUNT,TAXPER,ISSSNO,STUDDED"
                da = New OleDbDataAdapter(strSql, cn)
                Dim dttax As New DataTable
                da.Fill(dttax)
                Try
                    tran = cn.BeginTransaction()
                    If dttax.Rows.Count > 0 Then
                        Dim TotTax As Decimal = dttax.Compute("SUM(TAXAMOUNT)", Nothing)
                        For i As Integer = 0 To dttax.Rows.Count - 1
                            With dttax.Rows(i)
                                Dim tNo As Integer = .Item("TRANNO").ToString
                                Dim BillDate As Date = .Item("TRANDATE").ToString
                                Dim Type As String = .Item("TRANTYPE").ToString
                                Dim amount As Decimal = Val(.Item("AMOUNT").ToString)
                                Dim GstPer As Decimal = Val(.Item("TAXPER").ToString)
                                Dim GstAmt As Decimal = Val(.Item("TAXAMOUNT").ToString)
                                Dim IsssNo As String = .Item("ISSSNO").ToString
                                Dim Stud As String = .Item("STUDDED").ToString

                                strSql = "DELETE FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO='" & Batchno & "' AND ISSSNO='" & IsssNo & "' AND ISNULL(STUDDED,'')='" & Stud & "' AND TRANTYPE='" & Type & "'"
                                If cnHOCostId = cnCostId Then
                                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, CostId)
                                Else
                                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnHOCostId)
                                End If

                                If Billtype = "SG" Then
                                    GstPer = GstPer / 2
                                    strSql = " INSERT INTO " & cnStockDb & "..TAXTRAN"
                                    strSql += " ("
                                    strSql += " SNO,TRANNO,TRANDATE,TRANTYPE,BATCHNO,TAXID"
                                    strSql += " ,AMOUNT,TAXPER,TAXAMOUNT,TSNO,COSTID,COMPANYID,ISSSNO,STUDDED"
                                    strSql += " )"
                                    strSql += " VALUES("
                                    strSql += " '" & GetNewSno(TranSnoType.TAXTRANCODE, tran) & "'" ''SNO
                                    strSql += " ," & tNo & "" 'TRANNO
                                    strSql += " ,'" & BillDate & "'" 'TRANDATE 'BillDate
                                    strSql += " ,'" & Type & "'" 'TRANTYPE
                                    strSql += " ,'" & Batchno & "'" 'BATCHNO
                                    strSql += " ,'SG'" 'TAXID
                                    strSql += " ," & amount & "" 'AMOUNT
                                    strSql += " ," & GstPer & "" 'TAXPER
                                    strSql += " ," & Math.Abs(Math.Round(GstAmt / 2, 2)) & "" 'TAXAMOUNT
                                    strSql += " ,1" 'TSNO
                                    strSql += " ,'" & BillCostId & "'" 'COSTID 
                                    strSql += " ,'" & strCompanyId & "'" 'COMPANYID
                                    strSql += " ,'" & IsssNo & "'" 'ISSSNO
                                    strSql += " ,'" & Stud & "'" 'STUDDED
                                    strSql += " )"
                                    If cnHOCostId = cnCostId Then
                                        ExecQuery(SyncMode.Transaction, strSql, cn, tran, CostId)
                                    Else
                                        ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnHOCostId)
                                    End If
                                    TotTax -= Math.Abs(Math.Round(GstAmt / 2, 2))
                                    strSql = " INSERT INTO " & cnStockDb & "..TAXTRAN"
                                    strSql += " ("
                                    strSql += " SNO,TRANNO,TRANDATE,TRANTYPE,BATCHNO,TAXID"
                                    strSql += " ,AMOUNT,TAXPER,TAXAMOUNT,TSNO,COSTID,COMPANYID,ISSSNO,STUDDED"
                                    strSql += " )"
                                    strSql += " VALUES("
                                    strSql += " '" & GetNewSno(TranSnoType.TAXTRANCODE, tran) & "'" ''SNO
                                    strSql += " ," & tNo & "" 'TRANNO
                                    strSql += " ,'" & BillDate & "'" 'TRANDATE 'BillDate
                                    strSql += " ,'" & Type & "'" 'TRANTYPE
                                    strSql += " ,'" & Batchno & "'" 'BATCHNO
                                    strSql += " ,'CG'" 'TAXID
                                    strSql += " ," & amount & "" 'AMOUNT
                                    strSql += " ," & GstPer & "" 'TAXPER
                                    strSql += " ," & Math.Abs(Math.Round(GstAmt / 2, 2)) & "" 'TAXAMOUNT
                                    strSql += " ,2" 'TSNO
                                    strSql += " ,'" & BillCostId & "'" 'COSTID 
                                    strSql += " ,'" & strCompanyId & "'" 'COMPANYID
                                    strSql += " ,'" & IsssNo & "'" 'ISSSNO
                                    strSql += " ,'" & Stud & "'" 'STUDDED
                                    strSql += " )"
                                    If cnHOCostId = cnCostId Then
                                        ExecQuery(SyncMode.Transaction, strSql, cn, tran, CostId)
                                    Else
                                        ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnHOCostId)
                                    End If
                                    TotTax -= Math.Abs(Math.Round(GstAmt / 2, 2))
                                Else
                                    GstPer = GstPer * 2
                                    strSql = " INSERT INTO " & cnStockDb & "..TAXTRAN"
                                    strSql += " ("
                                    strSql += " SNO,TRANNO,TRANDATE,TRANTYPE,BATCHNO,TAXID"
                                    strSql += " ,AMOUNT,TAXPER,TAXAMOUNT,TSNO,COSTID,COMPANYID,ISSSNO,STUDDED"
                                    strSql += " )"
                                    strSql += " VALUES("
                                    strSql += " '" & GetNewSno(TranSnoType.TAXTRANCODE, tran) & "'" ''SNO
                                    strSql += " ," & tNo & "" 'TRANNO
                                    strSql += " ,'" & BillDate & "'" 'TRANDATE 'BillDate
                                    strSql += " ,'" & Type & "'" 'TRANTYPE
                                    strSql += " ,'" & Batchno & "'" 'BATCHNO
                                    strSql += " ,'IG'" 'TAXID
                                    strSql += " ," & amount & "" 'AMOUNT
                                    strSql += " ," & GstPer & "" 'TAXPER
                                    strSql += " ," & Math.Abs(GstAmt) & "" 'TAXAMOUNT
                                    strSql += " ,3" 'TSNO
                                    strSql += " ,'" & BillCostId & "'" 'COSTID 
                                    strSql += " ,'" & strCompanyId & "'" 'COMPANYID
                                    strSql += " ,'" & IsssNo & "'" 'ISSSNO
                                    strSql += " ,'" & Stud & "'" 'STUDDED
                                    strSql += " )"
                                    If cnHOCostId = cnCostId Then
                                        ExecQuery(SyncMode.Transaction, strSql, cn, tran, CostId)
                                    Else
                                        ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnHOCostId)
                                    End If
                                    TotTax -= Math.Abs(GstAmt)
                                End If
                            End With
                        Next
                    End If
                    If Billtype = "SG" Then
                        strSql = "SELECT SUM(AMOUNT) FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO='" & Batchno & "' AND PAYMODE='SV' AND ACCODE='0005018' "
                    Else
                        strSql = "SELECT SUM(AMOUNT) FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO='" & Batchno & "' AND PAYMODE='SV' AND ACCODE IN('0005028','0005038') "
                    End If
                    Dim AccTaxAmount As Decimal = Val(objGPack.GetSqlValue(strSql, , , tran).ToString)
                    If Billtype = "SG" Then
                        strSql = "SELECT * FROM " & cnStockDb & "..ACCTRAN A WHERE BATCHNO='" & Batchno & "' AND PAYMODE='SV' AND ACCODE='0005018' "
                    Else
                        strSql = "SELECT * FROM " & cnStockDb & "..ACCTRAN A WHERE BATCHNO='" & Batchno & "' AND PAYMODE='SV' AND ACCODE IN('0005028','0005038') "
                    End If
                    Dim dtacc As New DataTable
                    cmd = New OleDbCommand(strSql, cn, tran)
                    da = New OleDbDataAdapter(cmd)
                    da.Fill(dtacc)
                    If dtacc.Rows.Count > 0 Then
                        For j As Integer = 0 To dtacc.Rows.Count - 1
                            If Billtype = "SG" Then
                                strSql = "DELETE FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO='" & Batchno & "' AND PAYMODE='SV' AND ACCODE='0005018' "
                            Else
                                strSql = "DELETE FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO='" & Batchno & "' AND PAYMODE='SV' AND ACCODE IN('0005028','0005038') "
                            End If
                            If cnHOCostId = cnCostId Then
                                ExecQuery(SyncMode.Transaction, strSql, cn, tran, CostId)
                            Else
                                ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnHOCostId)
                            End If
                            If Billtype = "SG" Then
                                strSql = " INSERT INTO " & cnStockDb & "..ACCTRAN"
                                strSql += " ("
                                strSql += " SNO,TRANNO,TRANDATE,TRANMODE,ACCODE"
                                strSql += " ,AMOUNT,PCS,GRSWT,NETWT"
                                strSql += " ,PAYMODE"
                                strSql += " ,BRSFLAG"
                                strSql += " ,RELIASEDATE,FROMFLAG"
                                strSql += " ,CONTRA,BATCHNO,USERID,UPDATED"
                                strSql += " ,UPTIME,SYSTEMID,CASHID,COSTID,APPVER,COMPANYID"
                                strSql += " )"
                                strSql += " VALUES"
                                strSql += " ("
                                strSql += " '" & GetNewSno(TranSnoType.ACCTRANCODE, tran) & "'" ''SNO
                                strSql += " ," & dtacc.Rows(j).Item("TRANNO").ToString & "" 'TRANNO 
                                strSql += " ,'" & dtacc.Rows(j).Item("TRANDATE") & "'" 'TRANDATE
                                strSql += " ,'C'" 'TRANMODE
                                strSql += " ,'0005038'" 'ACCODE
                                strSql += " ," & Math.Abs(Math.Round(AccTaxAmount / 2, 2)) & "" 'AMOUNT
                                strSql += " ,0" 'PCS
                                strSql += " ,0" 'GRSWT
                                strSql += " ,0" 'NETWT
                                strSql += " ,'SV'" 'PAYMODE
                                strSql += " ,''" 'BRSFLAG
                                strSql += " ,NULL" 'RELIASEDATE
                                strSql += " ,'P'" 'FROMFLAG
                                strSql += " ,'" & dtacc.Rows(j).Item("CONTRA").ToString & "'" 'CONTRA
                                strSql += " ,'" & Batchno & "'" 'BATCHNO
                                strSql += " ,'" & userId & "'" 'USERID
                                strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                                strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                                strSql += " ,'" & dtacc.Rows(j).Item("SYSTEMID").ToString & "'" 'SYSTEMID
                                strSql += " ,'" & dtacc.Rows(j).Item("CASHID").ToString & "'" 'CASHID
                                strSql += " ,'" & BillCostId & "'" 'COSTID
                                strSql += " ,'SGCGIG'" 'APPVER
                                strSql += " ,'" & strCompanyId & "'" 'COMPANYID
                                strSql += " )"
                                If cnHOCostId = cnCostId Then
                                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, CostId)
                                Else
                                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnHOCostId)
                                End If
                                strSql = " INSERT INTO " & cnStockDb & "..ACCTRAN"
                                strSql += " ("
                                strSql += " SNO,TRANNO,TRANDATE,TRANMODE,ACCODE"
                                strSql += " ,AMOUNT,PCS,GRSWT,NETWT"
                                strSql += " ,PAYMODE"
                                strSql += " ,BRSFLAG"
                                strSql += " ,RELIASEDATE,FROMFLAG"
                                strSql += " ,CONTRA,BATCHNO,USERID,UPDATED"
                                strSql += " ,UPTIME,SYSTEMID,CASHID,COSTID,APPVER,COMPANYID"
                                strSql += " )"
                                strSql += " VALUES"
                                strSql += " ("
                                strSql += " '" & GetNewSno(TranSnoType.ACCTRANCODE, tran) & "'" ''SNO
                                strSql += " ," & dtacc.Rows(j).Item("TRANNO").ToString & "" 'TRANNO 
                                strSql += " ,'" & dtacc.Rows(j).Item("TRANDATE") & "'" 'TRANDATE
                                strSql += " ,'C'" 'TRANMODE
                                strSql += " ,'0005028'" 'ACCODE
                                strSql += " ," & Math.Abs(Math.Round(AccTaxAmount / 2, 2)) & "" 'AMOUNT
                                strSql += " ,0" 'PCS
                                strSql += " ,0" 'GRSWT
                                strSql += " ,0" 'NETWT
                                strSql += " ,'SV'" 'PAYMODE
                                strSql += " ,''" 'BRSFLAG
                                strSql += " ,NULL" 'RELIASEDATE
                                strSql += " ,'P'" 'FROMFLAG
                                strSql += " ,'" & dtacc.Rows(j).Item("CONTRA").ToString & "'" 'CONTRA
                                strSql += " ,'" & Batchno & "'" 'BATCHNO
                                strSql += " ,'" & userId & "'" 'USERID
                                strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                                strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                                strSql += " ,'" & dtacc.Rows(j).Item("SYSTEMID").ToString & "'" 'SYSTEMID
                                strSql += " ,'" & dtacc.Rows(j).Item("CASHID").ToString & "'" 'CASHID
                                strSql += " ,'" & BillCostId & "'" 'COSTID
                                strSql += " ,'SGCGIG'" 'APPVER
                                strSql += " ,'" & strCompanyId & "'" 'COMPANYID
                                strSql += " )"
                                If cnHOCostId = cnCostId Then
                                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, CostId)
                                Else
                                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnHOCostId)
                                End If
                            Else
                                strSql = " INSERT INTO " & cnStockDb & "..ACCTRAN"
                                strSql += " ("
                                strSql += " SNO,TRANNO,TRANDATE,TRANMODE,ACCODE"
                                strSql += " ,AMOUNT,PCS,GRSWT,NETWT"
                                strSql += " ,PAYMODE"
                                strSql += " ,BRSFLAG"
                                strSql += " ,RELIASEDATE,FROMFLAG"
                                strSql += " ,CONTRA,BATCHNO,USERID,UPDATED"
                                strSql += " ,UPTIME,SYSTEMID,CASHID,COSTID,APPVER,COMPANYID"
                                strSql += " )"
                                strSql += " VALUES"
                                strSql += " ("
                                strSql += " '" & GetNewSno(TranSnoType.ACCTRANCODE, tran) & "'" ''SNO
                                strSql += " ," & dtacc.Rows(j).Item("TRANNO").ToString & "" 'TRANNO 
                                strSql += " ,'" & dtacc.Rows(j).Item("TRANDATE") & "'" 'TRANDATE
                                strSql += " ,'C'" 'TRANMODE
                                strSql += " ,'0005018'" 'ACCODE
                                strSql += " ," & Math.Abs(AccTaxAmount) & "" 'AMOUNT
                                strSql += " ,0" 'PCS
                                strSql += " ,0" 'GRSWT
                                strSql += " ,0" 'NETWT
                                strSql += " ,'SV'" 'PAYMODE
                                strSql += " ,''" 'BRSFLAG
                                strSql += " ,NULL" 'RELIASEDATE
                                strSql += " ,'P'" 'FROMFLAG
                                strSql += " ,'" & dtacc.Rows(j).Item("CONTRA").ToString & "'" 'CONTRA
                                strSql += " ,'" & Batchno & "'" 'BATCHNO
                                strSql += " ,'" & userId & "'" 'USERID
                                strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                                strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                                strSql += " ,'" & dtacc.Rows(j).Item("SYSTEMID").ToString & "'" 'SYSTEMID
                                strSql += " ,'" & dtacc.Rows(j).Item("CASHID").ToString & "'" 'CASHID
                                strSql += " ,'" & BillCostId & "'" 'COSTID
                                strSql += " ,'SGCGIG'" 'APPVER
                                strSql += " ,'" & strCompanyId & "'" 'COMPANYID
                                strSql += " )"
                                If cnHOCostId = cnCostId Then
                                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, CostId)
                                Else
                                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnHOCostId)
                                End If
                            End If
                            Exit For
                        Next
                    End If
                    Dim balAmt As Double
                    balAmt = Val(objGPack.GetSqlValue("SELECT SUM(CASE WHEN TRANMODE = 'C' THEN isnull(AMOUNT,0) ELSE isnull(AMOUNT,0)*(-1) END) AS BALANCE FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = '" & Batchno & "'", , "0", tran))

                    If Math.Abs(balAmt) = 0.01 Then
                        Dim TranMode As String = "C"
                        If balAmt > 0 Then TranMode = "D"
                        strSql = " INSERT INTO " & cnStockDb & "..ACCTRAN"
                        strSql += " ("
                        strSql += " SNO,TRANNO,TRANDATE,TRANMODE,ACCODE"
                        strSql += " ,AMOUNT,PCS,GRSWT,NETWT"
                        strSql += " ,PAYMODE"
                        strSql += " ,BRSFLAG"
                        strSql += " ,RELIASEDATE,FROMFLAG"
                        strSql += " ,CONTRA,BATCHNO,USERID,UPDATED"
                        strSql += " ,UPTIME,SYSTEMID,CASHID,COSTID,APPVER,COMPANYID"
                        strSql += " )"
                        strSql += " VALUES"
                        strSql += " ("
                        strSql += " '" & GetNewSno(TranSnoType.ACCTRANCODE, tran) & "'" ''SNO
                        strSql += " ," & dtacc.Rows(0).Item("TRANNO").ToString & "" 'TRANNO 
                        strSql += " ,'" & dtacc.Rows(0).Item("TRANDATE") & "'" 'TRANDATE
                        strSql += " ,'" & TranMode & "'" 'TRANMODE
                        strSql += " ,'RNDOFF'" 'ACCODE
                        strSql += " ," & Math.Abs(balAmt) & "" 'AMOUNT
                        strSql += " ,0" 'PCS
                        strSql += " ,0" 'GRSWT
                        strSql += " ,0" 'NETWT
                        strSql += " ,'RO'" 'PAYMODE
                        strSql += " ,''" 'BRSFLAG
                        strSql += " ,NULL" 'RELIASEDATE
                        strSql += " ,'P'" 'FROMFLAG
                        strSql += " ,'" & dtacc.Rows(0).Item("CONTRA").ToString & "'" 'CONTRA
                        strSql += " ,'" & Batchno & "'" 'BATCHNO
                        strSql += " ,'" & userId & "'" 'USERID
                        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                        strSql += " ,'" & dtacc.Rows(0).Item("SYSTEMID").ToString & "'" 'SYSTEMID
                        strSql += " ,'" & dtacc.Rows(0).Item("CASHID").ToString & "'" 'CASHID
                        strSql += " ,'" & BillCostId & "'" 'COSTID
                        strSql += " ,'SGCGIG'" 'APPVER
                        strSql += " ,'" & strCompanyId & "'" 'COMPANYID
                        strSql += " )"
                        If cnHOCostId = cnCostId Then
                            ExecQuery(SyncMode.Transaction, strSql, cn, tran, CostId)
                        Else
                            ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnHOCostId)
                        End If
                    End If
                    If Math.Abs(balAmt) > 0.01 Then
                        tran.Rollback()
                        tran = Nothing
                    Else
                        tran.Commit()
                        tran = Nothing
                    End If
                Catch ex As Exception
                    Errorflag = True
                    If Not tran Is Nothing Then
                        tran.Rollback()
                        tran = Nothing
                    End If
                    Exit For
                End Try
            Next
        End If
        If Errorflag = False Then
            MsgBox("Updated Successfully...", MsgBoxStyle.Information)
        End If
        lblStatus.Visible = False
    End Sub
    Public Function funcHeaderNew() As Integer
        Dim dtheader As New DataTable
        dtheader.Clear()
        Try
            Dim dtMergeHeader As New DataTable("~MERGEHEADER")
            With dtMergeHeader
                .Columns.Add("STATE~PARTICULAR~INVDATE~AMOUNT~GST", GetType(String))
                .Columns.Add("IRATE~ITAX", GetType(String))
                .Columns.Add("CRATE~CTAX", GetType(String))
                .Columns.Add("SRATE~STAX", GetType(String))
                .Columns.Add("BILLTYPE~COMPANYSTATE~DIFF", GetType(String))
                .Columns.Add("SCROLL", GetType(String))
                .Columns("STATE~PARTICULAR~INVDATE~AMOUNT~GST").Caption = "PARTICULAR"
                .Columns("IRATE~ITAX").Caption = "IGST"
                .Columns("CRATE~CTAX").Caption = "CGST"
                .Columns("SRATE~STAX").Caption = "SGST"
                .Columns("BILLTYPE~COMPANYSTATE~DIFF").Caption = "DIFF"
            End With
            With gridViewHead
                .DataSource = dtMergeHeader
                .Columns("STATE~PARTICULAR~INVDATE~AMOUNT~GST").HeaderText = "PARTICULAR"
                .Columns("IRATE~ITAX").HeaderText = "IGST"
                .Columns("CRATE~CTAX").HeaderText = "CGST"
                .Columns("SRATE~STAX").HeaderText = "SGST"
                .Columns("BILLTYPE~COMPANYSTATE~DIFF").HeaderText = "DIFF"
                .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                funcColWidth()
                gridView.Focus()
                Dim colWid As Integer = 0
                For cnt As Integer = 0 To gridView.ColumnCount - 1
                    If gridView.Columns(cnt).Visible Then colWid += gridView.Columns(cnt).Width
                Next
                .Columns("SCROLL").Visible = False
            End With
        Catch ex As Exception
            MsgBox(ex.Message & vbCrLf & ex.StackTrace, MsgBoxStyle.Information)
        End Try
    End Function
    Function funcColWidth() As Integer
        With gridViewHead
            .Columns("STATE~PARTICULAR~INVDATE~AMOUNT~GST").Width = gridView.Columns("PARTICULAR").Width _
                + gridView.Columns("INVDATE").Width + gridView.Columns("AMOUNT").Width _
                + gridView.Columns("GST").Width _
                + gridView.Columns("STATE").Width
            .Columns("IRATE~ITAX").Width = gridView.Columns("IRATE").Width + gridView.Columns("ITAX").Width
            .Columns("CRATE~CTAX").Width = gridView.Columns("CRATE").Width + gridView.Columns("STAX").Width
            .Columns("SRATE~STAX").Width = gridView.Columns("SRATE").Width + gridView.Columns("STAX").Width
            .Columns("BILLTYPE~COMPANYSTATE~DIFF").Width = gridView.Columns("BILLTYPE").Width + gridView.Columns("COMPANYSTATE").Width + gridView.Columns("DIFF").Width
            With .Columns("SCROLL")
                .HeaderText = ""
                .Width = 0
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
        End With
    End Function
    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        btnUpdate.Visible = False
        lblStatus.Visible = False
        pnlHeading.Visible = False
        SalesAbs()
        Exit Sub
    End Sub

    Private Sub frmSalesAbstract_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmSalesAbstract_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        cmbMetal.Items.Clear()
        cmbMetal.Items.Add("ALL")
        strSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST ORDER BY METALNAME"
        objGPack.FillCombo(strSql, cmbMetal, False)
        cmbMetal.Text = "ALL"
        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE WHERE  ISNULL(COMPANYID,'" & strCompanyId & "')LIKE'%" & strCompanyId & "%' "
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        Me.btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ExtToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExtToolStripMenuItem.Click
        Me.btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        pnlHeading.Visible = False
        gridView.DataSource = Nothing
        btnView_Search.Enabled = True
        lblStatus.Visible = False
        Prop_Gets()
        dtpFrom.Focus()
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export, gridViewHead)
        End If
    End Sub

    Private Sub gridView_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs) Handles gridView.ColumnWidthChanged
        If gridViewHead.ColumnCount > 0 Then
            funcColWidth()
        End If
    End Sub
    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = "X" Or e.KeyChar = "x" Then
            Me.btnExcel_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = "P" Then
            btnPrint_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print, gridViewHead)
        End If
    End Sub

    Private Sub dtpFrom_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        dtpTo.MinimumDate = dtpFrom.Value
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New frmGSTReConcile_Properties
        SetSettingsObj(obj, Me.Name, GetType(frmGSTReConcile_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmGSTReConcile_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmGSTReConcile_Properties))
    End Sub

    Private Sub AutoReziseToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AutoReziseToolStripMenuItem.Click
        If gridView.RowCount > 0 Then
            If AutoReziseToolStripMenuItem.Checked Then
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                gridView.Invalidate()
                For Each dgvCol As DataGridViewColumn In gridView.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            Else
                For Each dgvCol As DataGridViewColumn In gridView.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End If
        End If
    End Sub

    Private Sub gridView_Scroll(ByVal sender As Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles gridView.Scroll
        If gridViewHead Is Nothing Then Exit Sub
        If Not gridViewHead.Columns.Count > 0 Then Exit Sub
        If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
            gridViewHead.HorizontalScrollingOffset = e.NewValue
        End If
        Try
            If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
                gridViewHead.HorizontalScrollingOffset = e.NewValue
                gridViewHead.Columns("SCROLL").Visible = CType(gridViewHead.Controls(0), HScrollBar).Visible
                gridViewHead.Columns("SCROLL").Width = CType(gridViewHead.Controls(1), VScrollBar).Width
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try
    End Sub

    
End Class

Public Class frmGSTReConcile_Properties
    Private chkPcs As Boolean = True
    Public Property p_chkPcs() As Boolean
        Get
            Return chkPcs
        End Get
        Set(ByVal value As Boolean)
            chkPcs = value
        End Set
    End Property
    Private chkGrsWt As Boolean = True
    Public Property p_chkGrsWt() As Boolean
        Get
            Return chkGrsWt
        End Get
        Set(ByVal value As Boolean)
            chkGrsWt = value
        End Set
    End Property
    Private chkNetWt As Boolean = True
    Public Property p_chkNetWt() As Boolean
        Get
            Return chkNetWt
        End Get
        Set(ByVal value As Boolean)
            chkNetWt = value
        End Set
    End Property
    Private chkWithSR As Boolean = False
    Public Property p_chkWithSR() As Boolean
        Get
            Return chkWithSR
        End Get
        Set(ByVal value As Boolean)
            chkWithSR = value
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
    Private chkVA As Boolean = False
    Public Property p_chkVA() As Boolean
        Get
            Return chkVA
        End Get
        Set(ByVal value As Boolean)
            chkVA = value
        End Set
    End Property
    Private cmbMetal As String = "ALL"
    Public Property p_cmbMetal() As String
        Get
            Return cmbMetal
        End Get
        Set(ByVal value As String)
            cmbMetal = value
        End Set
    End Property
    Private cmbCategory As String = "ALL"
    Public Property p_cmbCategory() As String
        Get
            Return cmbCategory
        End Get
        Set(ByVal value As String)
            cmbCategory = value
        End Set
    End Property
    Private rbtSummary As Boolean = True
    Public Property p_rbtSummary() As Boolean
        Get
            Return rbtSummary
        End Get
        Set(ByVal value As Boolean)
            rbtSummary = value
        End Set
    End Property
    Private rbtMonth As Boolean = False
    Public Property p_rbtMonth() As Boolean
        Get
            Return rbtMonth
        End Get
        Set(ByVal value As Boolean)
            rbtMonth = value
        End Set
    End Property
    Private rbtDate As Boolean = False
    Public Property p_rbtDate() As Boolean
        Get
            Return rbtDate
        End Get
        Set(ByVal value As Boolean)
            rbtDate = value
        End Set
    End Property
    Private rbtBillNo As Boolean = False
    Public Property p_rbtBillNo() As Boolean
        Get
            Return rbtBillNo
        End Get
        Set(ByVal value As Boolean)
            rbtBillNo = value
        End Set
    End Property
    Private chkBillPrefix As Boolean = False
    Public Property p_chkBillPrefix() As Boolean
        Get
            Return chkBillPrefix
        End Get
        Set(ByVal value As Boolean)
            chkBillPrefix = value
        End Set
    End Property
End Class