Imports System.Data.OleDb
Public Class InterestAnalysys
    Dim objGridShower As frmGridDispDia
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim dtAcName As New DataTable
    Public dtFilteration As New DataTable
    Public FormReSize As Boolean = True
    Public FormReLocation As Boolean = True

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        fnew()
    End Sub
    Function fnew()
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        txtCrInterestPer.Text = ""
        txtDrInterestPer.Text = ""
        chkDefInt.Checked = True
        cmbAcGroup_Man.SelectedIndex = 0
        gridview.DataSource = Nothing
        btn_Post.Enabled = False
        chkInclDate.Visible = False
        chkInclDate.Checked = False
        dtpFrom.Focus()
    End Function

    Private Sub InterestAnalysys_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAb}")
        End If
    End Sub

    Private Sub InterestAnalysys_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        strSql = " SELECT 'ALL' COMPANYNAME,'ALL' COMPANYID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COMPANYNAME,CONVERT(VARCHAR,COMPANYID),2 RESULT FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N'"
        strSql += " ORDER BY RESULT,COMPANYNAME"
        Dim dtcompany = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtcompany)
        BrighttechPack.GlobalMethods.FillCombo(chkcmbCompany, dtcompany, "COMPANYNAME", , "ALL")

        If GetAdmindbSoftValue("COSTCENTRE") = "Y" Then
            strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
            strSql += " UNION ALL"
            strSql += " SELECT COSTNAME,CONVERT(VARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
            strSql += " ORDER BY RESULT,COSTNAME"
            Dim dtCostCentre As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtCostCentre)
            BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , cnCostName)
            If strUserCentrailsed <> "Y" Then chkCmbCostCentre.Enabled = False
        Else
            chkCmbCostCentre.Enabled = False
            chkCmbCostCentre.Items.Clear()
        End If

        strSql = " SELECT 'ALL' ACGRPNAME,'ALL' ACGRPCODE,'1' RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT ACGRPNAME,CONVERT(VARCHAR,ACGRPCODE),2 RESULT FROM " & cnAdminDb & "..ACGROUP"
        strSql += " ORDER BY RESULT,ACGRPNAME"
        objGPack.FillCombo(strSql, cmbAcGroup_Man, , True)
        btnNew_Click(Me, New EventArgs)

    End Sub

    Private Sub cmbAcname_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAcname.Enter
        ''ACNAME
        If rbtDetailWise.Checked Then
            strSql = " SELECT ACNAME,CONVERT(vARCHAR,ACCODE),2 RESULT FROM " & cnAdminDb & "..ACHEAD"
            If cmbAcGroup_Man.Text <> "ALL" Then
                strSql += " WHERE ISNULL(ACGRPCODE,0) IN (SELECT ACGRPCODE FROM " & cnAdminDb & "..ACGROUP WHERE ISNULL(ACGRPNAME,0) IN ('" & cmbAcGroup_Man.Text & "'))"
            End If
            strSql += " ORDER BY RESULT,ACNAME"
        Else
            strSql = " SELECT 'ALL' ACNAME,'ALL' ACCODE,1 RESULT"
            strSql += " UNION ALL"
            strSql += " SELECT ACNAME,CONVERT(vARCHAR,ACCODE),2 RESULT FROM " & cnAdminDb & "..ACHEAD"
            If cmbAcGroup_Man.Text <> "ALL" Then
                strSql += " WHERE ISNULL(ACGRPCODE,0) IN (SELECT ACGRPCODE FROM " & cnAdminDb & "..ACGROUP WHERE ISNULL(ACGRPNAME,0) IN ('" & cmbAcGroup_Man.Text & "'))"
            End If
            strSql += " ORDER BY RESULT,ACNAME"
        End If
        objGPack.FillCombo(strSql, cmbAcname, , True)
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        Dim selectcompid As String = IIf(chkcmbCompany.Text <> "ALL", GetSelectedCompanyId(chkcmbCompany, False), "")
        Dim SelectedCostid As String = ""
        If Not chkCmbCostCentre.Text = "ALL" And chkCmbCostCentre.Enabled = True Then
            SelectedCostid = GetSelectedCostId(chkCmbCostCentre, False)
        End If
        Dim SelectedAcgrpId As String = ""
        If cmbAcGroup_Man.Text <> "ALL" Then
            strSql = "SELECT ACGRPCODE FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPNAME IN ('" & cmbAcGroup_Man.Text & "')"
            SelectedAcgrpId = GetSqlValue(cn, strSql)
        End If
        Dim SelectedAccode As String = ""
        If cmbAcname.Text <> "ALL" Then
            strSql = "SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME ='" & cmbAcname.Text & "'"
            SelectedAccode = GetSqlValue(cn, strSql)
        End If
        'If chkInclDate.Checked = True Then dtpTo.Value = DateAdd(DateInterval.Day, 1, dtpTo.Value)

        strSql = " EXEC " & cnAdminDb & "..SP_RPT_INTERANLYSIS"
        strSql += vbCrLf + " @DBNAME='" & cnStockDb & "' "
        strSql += vbCrLf + " ,@TEMPTBL='TEMP" & systemId & "INTEREST'"
        strSql += vbCrLf + " ,@FROMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@CRINTPER = '" & Val(txtCrInterestPer.Text) / 100 & "'"
        strSql += vbCrLf + " ,@DRINTPER = '" & Val(txtDrInterestPer.Text) / 100 & "'"
        strSql += vbCrLf + " ,@COSTID ='" & SelectedCostid & "'"
        strSql += vbCrLf + " ,@ACGRPID = '" & SelectedAcgrpId & "'"
        strSql += vbCrLf + " ,@ACCCODE = '" & SelectedAccode & "'"
        strSql += vbCrLf + " ,@COMPANYID = '" & selectcompid & "'"
        strSql += vbCrLf + " ,@DETAIL = '" & IIf(rbtDetailWise.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " ,@DEFPERCENT = '" & IIf(chkDefInt.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " ,@WITHTDS = '" & IIf(chkTDS.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " ,@INCLDATE = '" & IIf(chkInclDate.Checked, "Y", "N") & "'"
        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 1000
        da = New OleDbDataAdapter(cmd)
        Dim ds As New DataSet
        Dim dtGrid As New DataTable
        da.Fill(ds)
        dtGrid = ds.Tables(0)
        gridview.DataSource = Nothing
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        Dim IntBalance As Decimal = 0
        Dim Balance As Decimal = 0


        If rbtDetailWise.Checked = True Then
            For i As Integer = 0 To dtGrid.Rows.Count - 1
                IntBalance = Val(dtGrid.Rows(i).Item("INTBALANCE").ToString)
                Balance = Val(dtGrid.Rows(i).Item("BALANCE").ToString)

                If IntBalance > 0 Then
                    dtGrid.Rows(i).Item("INTBALANCE") = Format(IntBalance, "#0.00").ToString + " Dr"
                ElseIf IntBalance < 0 Then
                    dtGrid.Rows(i).Item("INTBALANCE") = Format(IntBalance * (-1), "#0.00").ToString + " Cr"
                Else
                    dtGrid.Rows(i).Item("INTBALANCE") = Format(IntBalance, "#0.00").ToString
                End If
                If Balance > 0 Then
                    dtGrid.Rows(i).Item("BALANCE") = Format(Balance, "#0.00").ToString + " Dr"
                ElseIf Balance < 0 Then
                    dtGrid.Rows(i).Item("BALANCE") = Format(Balance * (-1), "#0.00").ToString + " Cr"
                Else
                    dtGrid.Rows(i).Item("BALANCE") = Format(Balance, "#0.00").ToString
                End If
                If chkTDS.Checked Then
                    If Val(dtGrid.Rows(i).Item("TDS").ToString) > 0 Then
                        dtGrid.Rows(i).Item("TDS") = Format(Val(dtGrid.Rows(i).Item("TDS").ToString), "#0.00").ToString + " Dr"
                    ElseIf Balance < 0 Then
                        dtGrid.Rows(i).Item("TDS") = Format(Val(dtGrid.Rows(i).Item("TDS").ToString) * (-1), "#0.00").ToString + " Cr"
                    Else
                        dtGrid.Rows(i).Item("TDS") = Format(Val(dtGrid.Rows(i).Item("TDS").ToString), "#0.00").ToString
                    End If
                End If

            Next
        ElseIf rbtSummaryWise.Checked = True Then
            For i As Integer = 0 To dtGrid.Rows.Count - 1
                IntBalance = Val(dtGrid.Rows(i).Item("INTBALANCE").ToString)
                Balance = Val(dtGrid.Rows(i).Item("BALANCE").ToString)

                If IntBalance > 0 Then
                    dtGrid.Rows(i).Item("INTBALANCE") = Format(IntBalance, "#0.00").ToString + " Dr"
                ElseIf IntBalance < 0 Then
                    dtGrid.Rows(i).Item("INTBALANCE") = Format(IntBalance * (-1), "#0.00").ToString + " Cr"
                Else
                    dtGrid.Rows(i).Item("INTBALANCE") = Format(IntBalance, "#0.00").ToString
                End If
                If Balance > 0 Then
                    dtGrid.Rows(i).Item("BALANCE") = Format(Balance, "#0.00").ToString + " Dr"
                ElseIf Balance < 0 Then
                    dtGrid.Rows(i).Item("BALANCE") = Format(Balance * (-1), "#0.00").ToString + " Cr"
                Else
                    dtGrid.Rows(i).Item("BALANCE") = Format(Balance, "#0.00").ToString
                End If
                If chkTDS.Checked Then
                    If Val(dtGrid.Rows(i).Item("TDS").ToString) > 0 Then
                        dtGrid.Rows(i).Item("TDS") = Format(Val(dtGrid.Rows(i).Item("TDS").ToString), "#0.00").ToString + " Dr"
                    ElseIf Balance < 0 Then
                        dtGrid.Rows(i).Item("TDS") = Format(Val(dtGrid.Rows(i).Item("TDS").ToString) * (-1), "#0.00").ToString + " Cr"
                    Else
                        dtGrid.Rows(i).Item("TDS") = Format(Val(dtGrid.Rows(i).Item("TDS").ToString), "#0.00").ToString
                    End If
                End If
            Next
        End If
        gridview.DataSource = dtGrid


        If rbtSummaryWise.Checked Then
            btn_Post.Enabled = True
        Else
            btn_Post.Enabled = False
        End If
        If rbtDetailWise.Checked Then
            FillGridGroupStyle_KeyNoWise1(gridview, "PARTICULAR")
        End If
        DataGridView_SummaryFormatting(gridview)
    End Sub
    Public Sub FillGridGroupStyle_KeyNoWise1(ByVal gridView As DataGridView, Optional ByVal FirstColumnName As String = Nothing)
        Dim dt As New DataTable
        dt = CType(gridView.DataSource, DataTable).Copy
        ''title
        For cnt As Integer = 0 To dt.Rows.Count - 1
            If dt.Rows(cnt).Item("COLHEAD").ToString = "S1" Then
                gridView.Rows(cnt).Cells("PARTICULAR").Style.ForeColor = Color.Blue
                gridView.Rows(cnt).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            End If
            If dt.Rows(cnt).Item("COLHEAD").ToString = "G" Then
                gridView.Rows(cnt).Cells("PARTICULAR").Style.ForeColor = Color.LightYellow
                gridView.Rows(cnt).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            End If
        Next
        'ROWTITLE = DT.Select("COLHEAD = 'S'"
    End Sub
    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView)
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                dgvCol.Visible = True
                dgvCol.SortMode = DataGridViewColumnSortMode.NotSortable
            Next
            For i As Integer = 0 To dgv.Rows.Count - 1
                if not chkTDS.Checked then exit For 
                If dgv.Rows(i).Cells("TDS").Value.ToString <> "FORM-G" Then
                    dgv.Rows(i).Cells("TDS").Style.Alignment = DataGridViewContentAlignment.MiddleRight
                End If
            Next
            .Columns("DEBIT").DefaultCellStyle.Format = "#,##0.00"
            .Columns("CREDIT").DefaultCellStyle.Format = "#,##0.00"
            .Columns("INTDEBIT").DefaultCellStyle.Format = "#,##0.00"
            .Columns("INTCREDIT").DefaultCellStyle.Format = "#,##0.00"
            .Columns("INTBALANCE").DefaultCellStyle.Format = "#,##0.00"
            .Columns("TRANDATE").DefaultCellStyle.Format = "dd-MM-yyyy"
            .Columns("FROMDATE").DefaultCellStyle.Format = "dd-MM-yyyy"
            .Columns("TODATE").DefaultCellStyle.Format = "dd-MM-yyyy"
            .Columns("RESULT").Visible = False
            If rbtDetailWise.Checked Then
                .Columns("COLHEAD").Visible = False
            Else
                .Columns("TRANDATE").Visible = False
                .Columns("FROMDATE").Visible = False
                .Columns("TODATE").Visible = False
                .Columns("DUEDAYS").Visible = False
                If chkTDS.Checked Then .Columns("TDS").Visible = False
            End If
            .Columns("ACCODE").Visible = False
            If chkTDS.Checked Then
                .Columns("TDS").Visible = True
            End If
            .Columns("TRANNO").Visible = False
            .Columns("DEBIT").Visible = True
            .Columns("CREDIT").Visible = True
            .Columns("INTDEBIT").Visible = False
            .Columns("INTCREDIT").Visible = False
            .Columns("TRANDATE").Visible = False

            .Columns("TRANNO").Width = 60
            .Columns("DUEDAYS").Width = 80
            .Columns("FROMDATE").Width = 80
            .Columns("TODATE").Width = 80

            .Columns("TRANDATE").Width = 80
            .Columns("INTCREDIT").Width = 90
            .Columns("INTDEBIT").Width = 90
            .Columns("BALANCE").Width = 130
            .Columns("INTBALANCE").Width = 100
            .Columns("PARTICULAR").Width = 200
            FormatGridColumns(dgv, False, False, , False)
            .Columns("BALANCE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("INTBALANCE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        End With
    End Sub

    
    Private Sub btn_Post_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Post.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
        Dim objSecret As New frmAdminPassword()
        If objSecret.ShowDialog <> Windows.Forms.DialogResult.OK Then
            Exit Sub
        End If
        Try
            Dim TranNo As Integer
            Dim ctrlId As String
            ctrlId = "GEN-JOURNAL"
            Dim batchno As String
            batchno = GetNewBatchno(cnCostId, dtpTo.Value, tran)
            Dim Accode As String
            Dim Intaccode As String = GetAdmindbSoftValue("INTRSTACCODE", "INTRST")
            If Intaccode = "" Then MsgBox("Unable to Post" & vbCrLf & "Interest Posting Account Code Is Empty") : Exit Sub
            If rbtSummaryWise.Checked Then
                For i As Integer = 0 To gridview.RowCount - 1
                    If gridview.Rows(i).Cells("ACCODE").Value.ToString = "" Then GoTo NEXXT
                    Dim mtmode1 As String
                    Dim mtmode2 As String
                    Dim Intamtstr As String = gridview.Rows(i).Cells("INTBALANCE").Value.ToString
                    If Intamtstr = "0.00" Then GoTo NEXXT
                    If UCase(Mid(Intamtstr, Len(Intamtstr) - 2, 2)) = "DR" Then mtmode1 = "D" : mtmode2 = "C" Else mtmode1 = "C" : mtmode2 = "D"
                    Dim Intamt As Decimal = Val(Mid(Intamtstr, 1, Len(Intamtstr) - 2))
                    If Intamt = 0 Then GoTo NEXXT
                    Dim Isfirst As Boolean = True
GenBillNo:
                    TranNo = Val(GetBillControlValue(ctrlId, tran, Not Isfirst))
                    strSql = " UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT = '" & TranNo + 1 & "'"
                    strSql += " WHERE CTLID = '" & ctrlId & "' AND COMPANYID = '" & strCompanyId & "'"
                    strSql += " AND CONVERT(INT,CTLTEXT) = '" & TranNo & "'"
                    If strBCostid <> Nothing And Isfirst Then strSql += " AND isnull(COSTID,'') = '" & strBCostid & "'"
                    cmd = New OleDbCommand(strSql, cn, tran)
                    If Not cmd.ExecuteNonQuery() > 0 Then
                        Isfirst = False
                        GoTo GenBillNo
                    End If


                    TranNo += 1
                    Accode = gridview.Rows(i).Cells("ACCODE").Value.ToString
                    IntInsertIntoAccTran(i, TranNo, mtmode1, Accode, Intamt, batchno, "JE", Intaccode, 0, 0, 0, "Interest Posting")
                    IntInsertIntoAccTran(i, TranNo, mtmode2, Intaccode, Intamt, batchno, "JE", Accode, 0, 0, 0, "Interest Posting")

                    If chkTDS.Checked Then
                        Dim AccodeTds As String = ""
                        Dim TdsAmtstr As String = gridview.Rows(i).Cells("INTBALANCE").Value.ToString
                        If TdsAmtstr = "0.00" Then GoTo NEXXT
                        If UCase(Mid(TdsAmtstr, Len(TdsAmtstr) - 2, 2)) = "DR" Then mtmode1 = "D" : mtmode2 = "C" Else mtmode1 = "C" : mtmode2 = "D"
                        Dim TdsAmt As Decimal = Val(Mid(TdsAmtstr, 1, Len(TdsAmtstr) - 2))
                        If TdsAmt = 0 Then GoTo NEXXT
                        strSql = "SELECT ACCODE FROM " & cnAdminDb & "..TDSCATEGORY WHERE TDSCATID IN(SELECT TOP 1 TDSCATID FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = '" & Accode & "')"
                        AccodeTds = objGPack.GetSqlValue(strSql)
                        If AccodeTds <> "" And Val(TdsAmt) > 0 Then
                            IntInsertIntoAccTran(i, TranNo, mtmode1, Accode, TdsAmt, batchno, "JE", AccodeTds, 0, 0, 0, "Interest TDS Posting")
                            IntInsertIntoAccTran(i, TranNo, mtmode2, AccodeTds, TdsAmt, batchno, "JE", Accode, 0, 0, 0, "Interest TDS Posting")
                        End If

                    End If
NEXXT:
                Next
            End If
            fnew()
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub

    Private Sub IntInsertIntoAccTran _
(ByVal EntryOrder As Integer, ByVal tNo As Integer, _
ByVal tranMode As String, _
ByVal accode As String, _
ByVal amount As Double, _
ByVal batchno As String, _
ByVal payMode As String, _
ByVal contra As String, _
ByVal TDSCATID As Integer, _
ByVal TDSPER As Decimal, _
ByVal TDSAMOUNT As Decimal, _
Optional ByVal Remark1 As String = Nothing, _
Optional ByVal Remark2 As String = Nothing, _
Optional ByVal fLAG As String = Nothing, _
Optional ByVal SAccode As String = "" _
)
        If amount = 0 Then Exit Sub

        Dim costid As String = cnCostId
        strSql = " INSERT INTO " & cnStockDb & ".." & "ACCTRAN" & ""
        strSql += " ("
        strSql += " SNO,TRANNO,TRANDATE,TRANMODE,ACCODE,SACCODE"
        strSql += " ,AMOUNT,PAYMODE"
        strSql += " ,FROMFLAG,REMARK1,REMARK2"
        strSql += " ,CONTRA,BATCHNO,USERID,UPDATED"
        strSql += " ,UPTIME,SYSTEMID,CASHID,COSTID,VATEXM,APPVER,COMPANYID,WT_ENTORDER,TDSCATID,TDSPER,TDSAMOUNT,FLAG"
        strSql += " )"
        strSql += " VALUES"
        strSql += " ("
        strSql += " '" & GetNewSno(TranSnoType.ACCTRANCODE, tran) & "'" ''SNO
        strSql += " ," & tNo & "" 'TRANNO 
        strSql += " ,'" & dtpTo.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
        strSql += " ,'" & tranMode & "'" 'TRANMODE
        strSql += " ,'" & accode & "'" 'ACCODE
        strSql += " ,'" & SAccode & "'" 'ACCODE
        strSql += " ," & Math.Abs(amount) & "" 'AMOUNT
        strSql += " ,'" & payMode & "'" 'PAYMODE
        strSql += " ,'A'" 'FROMFLAG
        strSql += " ,'" & Remark1 & "'" 'REMARK1
        strSql += " ,'" & Remark2 & "'" 'REMARK2
        strSql += " ,'" & contra & "'" 'CONTRA
        strSql += " ,'" & batchno & "'" 'BATCHNO
        strSql += " ,'" & userId & "'" 'USERID
        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
        strSql += " ,'" & systemId & "'" 'SYSTEMID
        strSql += " ,''" 'CASHID
        strSql += " ,'" & CostId & "'" 'COSTID
        strSql += " ,''" 'VATEXM
        strSql += " ,'" & VERSION & "'" 'APPVER
        strSql += " ,'" & strCompanyId & "'" 'COMPANYID
        strSql += " ," & EntryOrder & "" 'WT_ENTORDER
        strSql += " ," & TDSCATID & "" 'TDSCATID
        strSql += " ," & TDSPER & "" 'TDSPER
        strSql += " ," & TDSAMOUNT & "" 'TDSAMOUNT
        strSql += " ,'" & fLAG & "'" 'FLAG
        strSql += " )"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, CostId)
        strSql = ""
        cmd = Nothing
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridview.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "INTEREST ANALYSIS", gridview, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridview.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "INTEREST ANALYSIS", gridview, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub gridView_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs) Handles gridview.ColumnWidthChanged
        If FormReSize Then
            Dim wid As Double = Nothing
            For cnt As Integer = 0 To gridView.ColumnCount - 1
                If gridView.Columns(cnt).Visible Then
                    wid += gridView.Columns(cnt).Width
                End If
            Next
            wid += 10
            If CType(gridView.Controls(1), VScrollBar).Visible Then
                wid += 20
            End If
            wid += 20
            If wid > ScreenWid Then
                wid = ScreenWid
            End If
            Me.Size = New Size(wid, Me.Height)
        End If
        If FormReLocation Then SetCenterLocation(Me)
    End Sub
    Private Sub ResizeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResizeToolStripMenuItem.Click
        If gridview.RowCount > 0 Then
            If ResizeToolStripMenuItem.Checked Then
                gridview.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                gridview.Invalidate()
                For Each dgvCol As DataGridViewColumn In gridview.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridview.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            Else
                For Each dgvCol As DataGridViewColumn In gridview.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridview.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End If
            'gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        End If
    End Sub
    Public Sub SetFilterStr(ByVal filterColumnName As String, ByVal setValue As Object)
        If Not dtFilteration.Columns.Contains(filterColumnName) Then Exit Sub
        If Not dtFilteration.Rows.Count > 0 Then Exit Sub
        dtFilteration.Rows(0).Item(filterColumnName) = setValue
    End Sub

    Public Function GetFilterStr(ByVal filterColumnName As String) As String
        Dim ftrStr As String = Nothing
        If Not dtFilteration.Columns.Contains(filterColumnName) Then Return ftrStr
        If Not dtFilteration.Rows.Count > 0 Then Return ftrStr
        Return dtFilteration.Rows(0).Item(filterColumnName).ToString
        Return ftrStr
    End Function

    Private Sub cmbAcname_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAcname.Leave
        If cmbAcname.Text <> "" And cmbAcname.Text <> "ALL" Then
            strSql = "SELECT isnull(intper,0) FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME ='" & cmbAcname.Text & "'"
            'Dim intper As Decimal = Val(GetSqlValue(cn, strSql).ToString)
            'If intper <> 0 Then txtCrInterestPer.Text = Format(intper, "0.00") : txtDrInterestPer.Text = Format(intper, "0.00") : txtCrInterestPer.Enabled = False : txtDrInterestPer.Enabled = False Else txtCrInterestPer.Enabled = True : txtDrInterestPer.Enabled = True
        End If
    End Sub

    Private Sub chkDefInt_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkDefInt.CheckedChanged
        If chkDefInt.Checked = True Then
            txtCrInterestPer.Text = ""
            txtDrInterestPer.Text = ""
            txtCrInterestPer.Enabled = False
            txtDrInterestPer.Enabled = False
        Else
            txtCrInterestPer.Enabled = True
            txtDrInterestPer.Enabled = True
        End If
    End Sub

    Private Sub gridview_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridview.KeyPress
        If UCase(e.KeyChar) = "P" Then
            Me.btnPrint_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = "X" Then
            Me.btnExport_Click(Me, New EventArgs)
        End If
    End Sub
End Class
Public Class Interest_Properties
    Private chkCompanySelectAll As Boolean = False
    Public Property p_chkCompanySelectAll() As Boolean
        Get
            Return chkCompanySelectAll
        End Get
        Set(ByVal value As Boolean)
            chkCompanySelectAll = value
        End Set
    End Property
    Private chkLstCompany As New List(Of String)
    Public Property p_chkLstCompany() As List(Of String)
        Get
            Return chkLstCompany
        End Get
        Set(ByVal value As List(Of String))
            chkLstCompany = value
        End Set
    End Property
    Private chkCostCentreSelectAll As Boolean = False
    Public Property p_chkCostCentreSelectAll() As Boolean
        Get
            Return chkCostCentreSelectAll
        End Get
        Set(ByVal value As Boolean)
            chkCostCentreSelectAll = value
        End Set
    End Property
    Private chkLstCostCentre As New List(Of String)
    Public Property p_chkLstCostCentre() As List(Of String)
        Get
            Return chkLstCostCentre
        End Get
        Set(ByVal value As List(Of String))
            chkLstCostCentre = value
        End Set
    End Property
End Class