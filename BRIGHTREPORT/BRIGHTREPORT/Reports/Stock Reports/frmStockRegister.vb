Imports System.Data.OleDb
Public Class frmStockRegister
    Dim strSql As String = Nothing
    Dim dtTemp As New DataTable
    Dim ftrIssue As String
    Dim ftrReceipt As String
    Dim ftrStoneStr As String
    Dim rowTitle As String
    Dim dsGridView As New DataSet
    Dim objGridShower As frmGridDispDia
    Dim cmd As OleDbCommand
    Dim dtCompany As New DataTable
    Dim dtCostCentre As New DataTable
    Dim dtGridView As New DataTable
    Dim flagDetail As Boolean = False
    Dim flagSummary As Boolean = False
    Dim metalTitle As String
    Dim MELTINGACC As String = GetAdmindbSoftValue("MELTINGACC", "")

    Private Sub frmCategoryStock_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Escape) Or e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        AutoResizeToolStripMenuItem.Checked = False
        Me.Cursor = Cursors.WaitCursor
        gridView.DataSource = Nothing
        lblTitle.Text = ""
        Me.Refresh()
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        Dim selCatCode As String = Nothing
        strSql = " EXEC " & cnAdminDb & "..AGR_SP_VATFORM_ANNEXURE_8"
        strSql += vbCrLf + " @DBNAME = '" & cnStockDb & "'"
        strSql += vbCrLf + " ,@ADMINDB = '" & cnAdminDb & "'"
        strSql += vbCrLf + " ,@FROMDATE = '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@TODATE = '" & Format(dtpTo.Value, "yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@METALNAME = '" & cmbMetal.Text & "'"
        strSql += vbCrLf + " ,@COMPANYID = '" & strCompanyId & "'"
        strSql += vbCrLf + " ,@COSTNAME = '" & GetQryString(chkCmbCostCentre.Text).Replace("'", "") & "'"
        strSql += vbCrLf + " ,@SPLITPU = '" & IIf(ChkSplitPur.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " ,@GS11 = '" & IIf(rbtGs11Grswt.Checked, "G", "N") & "'"
        strSql += vbCrLf + " ,@GS12 = '" & IIf(rbtGs12Grswt.Checked, "G", "N") & "'"
        strSql += vbCrLf + " ,@GGS11RATE = '" & IIf(ChkRate.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " ,@SGS11RATE = '" & IIf(ChkSRate.Checked, "Y", "N") & "'"
        Prop_Sets()
        dtGridView = New DataTable
        dsGridView = New DataSet
        gridView.DataSource = Nothing
        cmd = New OleDbCommand(strSql, cn)
        da = New OleDbDataAdapter(cmd)
        da.Fill(dsGridView)
        dtGridView = dsGridView.Tables(0)
        If Not dtGridView.Rows.Count > 0 Then
            MsgBox("Record Not Found", MsgBoxStyle.Information)
            lblTitle.Text = ""
            Exit Sub
        End If
        gridView.DefaultCellStyle = Nothing
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        gridView.DataSource = dtGridView
        If gridView.Columns.Contains("PARTHEAD") Then gridView.Columns("PARTHEAD").Visible = False
        GridViewFormat()
        funcGridViewStyle()
        AutoResizeToolStripMenuItem_Click(Me, New EventArgs)
        Dim title As String = Nothing
        title += " STOCK REGISTER REPORT "
        title += " FROM " + dtpFrom.Text + " TO " + dtpTo.Text
        lblTitle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        title = title + IIf(chkCmbCostCentre.Text <> "" And chkCmbCostCentre.Text <> "ALL", " :" & chkCmbCostCentre.Text, "")
        lblTitle.Text = title

        Dim colWid As Integer = 0
        For cnt As Integer = 0 To gridView.ColumnCount - 1
            If gridView.Columns(cnt).Visible Then colWid += gridView.Columns(cnt).Width
        Next

        gridView.Focus()
        Me.Cursor = Cursors.Default
    End Sub
    Function funcGridViewStyle() As Integer
        With gridView
           
            With .Columns("AMOUNT")
                .Width = 170
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Format = "##,##,##,###.00"
            End With

        End With
    End Function
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        gridView.DataSource = Nothing
        dsGridView.Clear()
        dtpFrom.Value = cnTranFromDate
        dtpTo.Value = GetServerDate()
        dtpFrom.Focus()
        lblTitle.Text = ""
        Prop_Gets()
    End Sub
    Private Sub funcDetailView()
        strSql = vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMPANEX" & userId & systemId & "','U') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPANEX" & userId & systemId & ""
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()
        strSql = ""
        Dim Flag As Boolean = False
        If gridView.CurrentRow.Cells("RESULT").Value = 2 Or gridView.CurrentRow.Cells("RESULT").Value = 37 Or gridView.CurrentRow.Cells("RESULT").Value = 49 Or gridView.CurrentRow.Cells("RESULT").Value = 64 Or gridView.CurrentRow.Cells("RESULT").Value = 80 Then
            strSql = "SELECT TRANNO,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=X.ACCODE)ACNAME,TRANDATE,CASE WHEN TRANTYPE='PU' THEN 'PURCHASE' WHEN TRANTYPE='RPU' THEN 'DIRECT PURCHASE' ELSE TRANTYPE END TRANTYPE"
            strSql += vbCrLf + " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE=X.CATCODE)CATEGORY"
            strSql += vbCrLf + " ,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(AMOUNT)AMOUNT,TRANDATE1,IDENTITY(INT,1,1)SNO "
            strSql += vbCrLf + " INTO TEMPTABLEDB..TEMPANEX" & userId & systemId & ""
            strSql += vbCrLf + " FROM ("
            strSql += vbCrLf + " SELECT"
            strSql += vbCrLf + " CONVERT(VARCHAR(10),TRANNO)+ case when refno<>0 then '  ( BILL-'+CONVERT(VARCHAR(10),case when refno<>0 then refno else '' end )+' )' ELSE '' END TRANNO,ACCODE,CONVERT(VARCHAR(20),TRANDATE,105)TRANDATE,CONVERT(VARCHAR(10),TRANTYPE)TRANTYPE,CATCODE,"
            strSql += vbCrLf + " TRANDATE TRANDATE1,GRSWT GRSWT,NETWT NETWT,"
            strSql += vbCrLf + " AMOUNT-ISNULL((SELECT SUM(STNAMT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = R.SNO),0)  AMOUNT"
            strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT R"
            strSql += vbCrLf + " WHERE BATCHNO NOT IN (SELECT BATCHNO FROM " & cnStockDb & "..MELTINGDETAIL WHERE RECISS='R'  "
            strSql += vbCrLf + " AND COMPANYID='" & strCompanyId & "' AND ISNULL(CANCEL,'')=''  "
            strSql += vbCrLf + " AND TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "') "
            strSql += vbCrLf + " AND COMPANYID='" & strCompanyId & "' AND ISNULL(CANCEL,'')='' "
            If gridView.CurrentRow.Cells("RESULT").Value = 73 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID='P')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 64 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID='D')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 49 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS12='Y' AND METALID='S')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 37 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS11='Y' AND METALID='S')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 18 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS12='Y' AND METALID='G')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 2 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS11='Y' AND METALID='G')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 80 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS12='N' AND METALID='P')"
            End If
            If GetSelectedCostId(chkCmbCostCentre, True) <> "''" Then
                strSql += vbCrLf + " AND COSTID IN('" & GetSelectedCostId(chkCmbCostCentre, True) & "')"
            End If
            strSql += vbCrLf + " AND TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "' "
            strSql += vbCrLf + " AND TRANTYPE NOT IN('RRE','SR','RIN')"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " CONVERT(VARCHAR(10),TRANNO)TRANNO,"
            strSql += vbCrLf + " (SELECT TOP 1 ACCODE FROM " & cnStockDb & "..RECEIPT WHERE BATCHNO=R.BATCHNO)ACCODE"
            strSql += vbCrLf + " ,CONVERT(VARCHAR(20),TRANDATE,105)TRANDATE,CONVERT(VARCHAR(10),TRANTYPE)TRANTYPE,CATCODE,"
            strSql += vbCrLf + " TRANDATE TRANDATE1,STNWT RGRSWT,STNWT RNETWT,STNAMT RAMOUNT"
            strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPTSTONE R"
            strSql += vbCrLf + " WHERE BATCHNO IN (SELECT BATCHNO FROM " & cnStockDb & "..RECEIPT WHERE 1=1  "
            strSql += vbCrLf + " AND COMPANYID='" & strCompanyId & "' "
            strSql += vbCrLf + " AND ISNULL(CANCEL,'')=''  "
            strSql += vbCrLf + " AND TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "') "
            If gridView.CurrentRow.Cells("RESULT").Value = 73 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID='P')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 64 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID='D')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 49 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS12='Y' AND METALID='S')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 37 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS11='Y' AND METALID='S')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 18 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS12='Y' AND METALID IN('G','T'))"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 2 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS11='Y' AND METALID='G')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 80 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS12='N' AND METALID='P')"
            End If
            If GetSelectedCostId(chkCmbCostCentre, True) <> "''" Then
                strSql += vbCrLf + " AND COSTID IN('" & GetSelectedCostId(chkCmbCostCentre, True) & "')"
            End If
            If GetSelectedCostId(chkCmbCostCentre, True) <> "''" Then
                strSql += vbCrLf + " AND COSTID IN('" & GetSelectedCostId(chkCmbCostCentre, True) & "')"
            End If
            strSql += vbCrLf + " AND TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "' "
            strSql += vbCrLf + " AND TRANTYPE NOT IN('RRE','SR','RIN')"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT"
            strSql += vbCrLf + " CONVERT(VARCHAR(10),TRANNO)TRANNO,ACCODE,CONVERT(VARCHAR(20),TRANDATE,105)TRANDATE,CONVERT(VARCHAR(10),TRANTYPE)TRANTYPE,CATCODE,"
            strSql += vbCrLf + " TRANDATE TRANDATE1,-1*GRSWT GRSWT,-1*NETWT NETWT,"
            strSql += vbCrLf + " -1*AMOUNT-ISNULL((SELECT SUM(STNAMT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = R.SNO),0)  AMOUNT"
            strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE R"
            strSql += vbCrLf + " WHERE 1=1"
            strSql += vbCrLf + " AND COMPANYID='" & strCompanyId & "' AND ISNULL(CANCEL,'')='' "
            If gridView.CurrentRow.Cells("RESULT").Value = 73 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID='P')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 64 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID='D')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 49 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS12='Y' AND METALID='S')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 37 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS11='Y' AND METALID='S')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 18 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS12='Y' AND METALID='G')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 2 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS11='Y' AND METALID='G')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 80 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS12='N' AND METALID='P')"
            End If
            If GetSelectedCostId(chkCmbCostCentre, True) <> "''" Then
                strSql += vbCrLf + " AND COSTID IN('" & GetSelectedCostId(chkCmbCostCentre, True) & "')"
            End If
            strSql += vbCrLf + " AND TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "' "
            strSql += vbCrLf + " AND TRANTYPE ='IPU'"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " CONVERT(VARCHAR(10),TRANNO)TRANNO,"
            strSql += vbCrLf + " (SELECT TOP 1 ACCODE FROM " & cnStockDb & "..ISSUE WHERE BATCHNO=R.BATCHNO)ACCODE"
            strSql += vbCrLf + " ,CONVERT(VARCHAR(20),TRANDATE,105)TRANDATE,CONVERT(VARCHAR(10),TRANTYPE)TRANTYPE,CATCODE,"
            strSql += vbCrLf + " TRANDATE TRANDATE1,-1*STNWT RGRSWT,-1*STNWT RNETWT,-1*STNAMT RAMOUNT"
            strSql += vbCrLf + " FROM " & cnStockDb & "..ISSSTONE R"
            strSql += vbCrLf + " WHERE BATCHNO IN (SELECT BATCHNO FROM " & cnStockDb & "..ISSUE WHERE 1=1  "
            strSql += vbCrLf + " AND COMPANYID='" & strCompanyId & "' "
            strSql += vbCrLf + " AND ISNULL(CANCEL,'')=''  "
            strSql += vbCrLf + " AND TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "') "
            If gridView.CurrentRow.Cells("RESULT").Value = 73 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID='P')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 64 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID='D')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 49 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS12='Y' AND METALID='S')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 37 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS11='Y' AND METALID='S')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 18 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS12='Y' AND METALID IN('G','T'))"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 2 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS11='Y' AND METALID='G')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 80 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS12='N' AND METALID='P')"
            End If
            If GetSelectedCostId(chkCmbCostCentre, True) <> "''" Then
                strSql += vbCrLf + " AND COSTID IN('" & GetSelectedCostId(chkCmbCostCentre, True) & "')"
            End If
            If GetSelectedCostId(chkCmbCostCentre, True) <> "''" Then
                strSql += vbCrLf + " AND COSTID IN('" & GetSelectedCostId(chkCmbCostCentre, True) & "')"
            End If
            strSql += vbCrLf + " AND TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "' "
            strSql += vbCrLf + " AND TRANTYPE ='IPU'"
            strSql += vbCrLf + " )X GROUP BY TRANNO,TRANDATE,TRANDATE1,TRANTYPE,CATCODE,ACCODE"
            strSql += vbCrLf + " ORDER BY CONVERT(SMALLDATETIME,TRANDATE1),TRANNO,ACCODE"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
            strSql = vbCrLf + " SELECT * FROM TEMPTABLEDB..TEMPANEX" & userId & systemId & " ORDER BY SNO"
            Flag = True

        ElseIf gridView.CurrentRow.Cells("RESULT").Value = 73 Then
            strSql = "SELECT TRANNO,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=X.ACCODE)ACNAME,TRANDATE,CASE WHEN TRANTYPE='PU' THEN 'PURCHASE' WHEN TRANTYPE='RPU' THEN 'DIRECT PURCHASE' ELSE TRANTYPE END TRANTYPE"
            strSql += vbCrLf + " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE=X.CATCODE)CATEGORY"
            strSql += vbCrLf + " ,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(AMOUNT)AMOUNT,TRANDATE1,IDENTITY(INT,1,1)SNO "
            strSql += vbCrLf + " INTO TEMPTABLEDB..TEMPANEX" & userId & systemId & ""
            strSql += vbCrLf + " FROM ("
            strSql += vbCrLf + " SELECT"
            strSql += vbCrLf + " CONVERT(VARCHAR(10),TRANNO)+ case when refno<>0 then '  ( BILL-'+CONVERT(VARCHAR(10),case when refno<>0 then refno else '' end )+' )' ELSE '' END TRANNO,ACCODE,CONVERT(VARCHAR(20),TRANDATE,105)TRANDATE,CONVERT(VARCHAR(10),TRANTYPE)TRANTYPE,CATCODE,"
            strSql += vbCrLf + " TRANDATE TRANDATE1,GRSWT GRSWT,NETWT NETWT,"
            strSql += vbCrLf + " AMOUNT-ISNULL((SELECT SUM(STNAMT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = R.SNO),0)  AMOUNT"
            strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT R"
            strSql += vbCrLf + " WHERE BATCHNO NOT IN (SELECT BATCHNO FROM " & cnStockDb & "..MELTINGDETAIL WHERE RECISS='R'  "
            strSql += vbCrLf + " AND COMPANYID='" & strCompanyId & "' AND ISNULL(CANCEL,'')=''  "
            strSql += vbCrLf + " AND TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "') "
            strSql += vbCrLf + " AND COMPANYID='" & strCompanyId & "' AND ISNULL(CANCEL,'')='' "
            If gridView.CurrentRow.Cells("RESULT").Value = 73 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID='P')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 64 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID='D')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 49 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS12='Y' AND METALID='S')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 37 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS11='Y' AND METALID='S')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 18 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS12='Y' AND METALID='G')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 2 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS11='Y' AND METALID='G')"
            End If
            If GetSelectedCostId(chkCmbCostCentre, True) <> "''" Then
                strSql += vbCrLf + " AND COSTID IN('" & GetSelectedCostId(chkCmbCostCentre, True) & "')"
            End If
            strSql += vbCrLf + " AND TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "' "
            strSql += vbCrLf + " AND TRANTYPE NOT IN('RRE','SR','RIN','PU')"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " CONVERT(VARCHAR(10),TRANNO)TRANNO,''ACCODE,CONVERT(VARCHAR(20),TRANDATE,105)TRANDATE,CONVERT(VARCHAR(10),TRANTYPE)TRANTYPE,CATCODE,"
            strSql += vbCrLf + " TRANDATE TRANDATE1,STNWT RGRSWT,STNWT RNETWT,STNAMT RAMOUNT"
            strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPTSTONE"
            strSql += vbCrLf + " WHERE BATCHNO IN (SELECT BATCHNO FROM " & cnStockDb & "..RECEIPT WHERE 1=1  "
            strSql += vbCrLf + " AND COMPANYID='" & strCompanyId & "' "
            strSql += vbCrLf + " AND ISNULL(CANCEL,'')=''  "
            strSql += vbCrLf + " AND TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "') "
            If gridView.CurrentRow.Cells("RESULT").Value = 73 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID='P')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 64 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID='D')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 49 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS12='Y' AND METALID='S')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 37 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS11='Y' AND METALID='S')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 18 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS12='Y' AND METALID IN('G','T'))"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 2 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS11='Y' AND METALID='G')"
            End If
            If GetSelectedCostId(chkCmbCostCentre, True) <> "''" Then
                strSql += vbCrLf + " AND COSTID IN('" & GetSelectedCostId(chkCmbCostCentre, True) & "')"
            End If
            If GetSelectedCostId(chkCmbCostCentre, True) <> "''" Then
                strSql += vbCrLf + " AND COSTID IN('" & GetSelectedCostId(chkCmbCostCentre, True) & "')"
            End If
            strSql += vbCrLf + " AND TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "' "
            strSql += vbCrLf + " AND TRANTYPE NOT IN('RRE','SR','RIN')"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT"
            strSql += vbCrLf + " CONVERT(VARCHAR(10),TRANNO)TRANNO,ACCODE,CONVERT(VARCHAR(20),TRANDATE,105)TRANDATE,CONVERT(VARCHAR(10),TRANTYPE)TRANTYPE,CATCODE,"
            strSql += vbCrLf + " TRANDATE TRANDATE1,-1*GRSWT GRSWT,-1*NETWT NETWT,"
            strSql += vbCrLf + " -1*AMOUNT+ISNULL((SELECT SUM(STNAMT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = R.SNO),0)  AMOUNT"
            strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE R"
            strSql += vbCrLf + " WHERE 1=1"
            strSql += vbCrLf + " AND COMPANYID='" & strCompanyId & "' AND ISNULL(CANCEL,'')='' "
            If gridView.CurrentRow.Cells("RESULT").Value = 73 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID='P')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 64 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID='D')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 49 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS12='Y' AND METALID='S')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 37 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS11='Y' AND METALID='S')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 18 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS12='Y' AND METALID='G')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 2 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS11='Y' AND METALID='G')"
            End If
            If GetSelectedCostId(chkCmbCostCentre, True) <> "''" Then
                strSql += vbCrLf + " AND COSTID IN('" & GetSelectedCostId(chkCmbCostCentre, True) & "')"
            End If
            strSql += vbCrLf + " AND TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "' "
            strSql += vbCrLf + " AND TRANTYPE ='IPU'"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " CONVERT(VARCHAR(10),TRANNO)TRANNO,''ACCODE,CONVERT(VARCHAR(20),TRANDATE,105)TRANDATE,CONVERT(VARCHAR(10),TRANTYPE)TRANTYPE,CATCODE,"
            strSql += vbCrLf + " TRANDATE TRANDATE1,-1*STNWT RGRSWT,-1*STNWT RNETWT,-1*STNAMT RAMOUNT"
            strSql += vbCrLf + " FROM " & cnStockDb & "..ISSSTONE"
            strSql += vbCrLf + " WHERE BATCHNO IN (SELECT BATCHNO FROM " & cnStockDb & "..ISSUE WHERE 1=1  "
            strSql += vbCrLf + " AND COMPANYID='" & strCompanyId & "' "
            strSql += vbCrLf + " AND ISNULL(CANCEL,'')=''  "
            strSql += vbCrLf + " AND TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "') "
            If gridView.CurrentRow.Cells("RESULT").Value = 73 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID='P')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 64 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID='D')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 49 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS12='Y' AND METALID='S')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 37 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS11='Y' AND METALID='S')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 18 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS12='Y' AND METALID IN('G','T'))"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 2 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS11='Y' AND METALID='G')"
            End If
            If GetSelectedCostId(chkCmbCostCentre, True) <> "''" Then
                strSql += vbCrLf + " AND COSTID IN('" & GetSelectedCostId(chkCmbCostCentre, True) & "')"
            End If
            If GetSelectedCostId(chkCmbCostCentre, True) <> "''" Then
                strSql += vbCrLf + " AND COSTID IN('" & GetSelectedCostId(chkCmbCostCentre, True) & "')"
            End If
            strSql += vbCrLf + " AND TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "' "
            strSql += vbCrLf + " AND TRANTYPE ='IPU'"
            strSql += vbCrLf + " )X GROUP BY TRANNO,TRANDATE,TRANDATE1,TRANTYPE,CATCODE,ACCODE"
            strSql += vbCrLf + " ORDER BY CONVERT(SMALLDATETIME,TRANDATE1),TRANNO,ACCODE"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
            strSql = vbCrLf + " SELECT * FROM TEMPTABLEDB..TEMPANEX" & userId & systemId & " ORDER BY SNO"
            Flag = True


        ElseIf gridView.CurrentRow.Cells("RESULT").Value = 18 Then
            strSql = "SELECT TRANNO,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=X.ACCODE)ACNAME,TRANDATE,CASE WHEN TRANTYPE='PU' THEN 'PURCHASE' WHEN TRANTYPE='RPU' THEN 'DIRECT PURCHASE' ELSE TRANTYPE END TRANTYPE"
            strSql += vbCrLf + " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE=X.CATCODE)CATEGORY"
            strSql += vbCrLf + " ,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT, AMOUNT,TRANDATE1,IDENTITY(INT,1,1)SNO "
            strSql += vbCrLf + " INTO TEMPTABLEDB..TEMPANEX" & userId & systemId & ""
            strSql += vbCrLf + " FROM ("
            strSql += vbCrLf + " SELECT"
            strSql += vbCrLf + " CONVERT(VARCHAR(10),TRANNO)+ case when refno<>0 then '  ( BILL-'+CONVERT(VARCHAR(10),case when refno<>0 then refno else '' end )+' )' ELSE '' END TRANNO,ACCODE,CONVERT(VARCHAR(20),TRANDATE,105)TRANDATE,CONVERT(VARCHAR(10),TRANTYPE)TRANTYPE,CATCODE,"
            strSql += vbCrLf + " TRANDATE TRANDATE1, CASE WHEN METALID<>'T' THEN GRSWT ELSE 0 END GRSWT,CASE WHEN METALID<>'T' THEN NETWT ELSE 0 END NETWT,"
            strSql += vbCrLf + " AMOUNT-ISNULL((SELECT SUM(STNAMT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = R.SNO),0)  AMOUNT"
            strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT R"
            strSql += vbCrLf + " WHERE BATCHNO NOT IN (SELECT BATCHNO FROM " & cnStockDb & "..MELTINGDETAIL WHERE RECISS='R'  "
            strSql += vbCrLf + " AND COMPANYID='" & strCompanyId & "' AND ISNULL(CANCEL,'')=''  "
            strSql += vbCrLf + " AND TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "') "
            strSql += vbCrLf + " AND COMPANYID='" & strCompanyId & "' AND ISNULL(CANCEL,'')='' "
            If gridView.CurrentRow.Cells("RESULT").Value = 73 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID='P')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 64 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID='D')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 49 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS12='Y' AND METALID='S')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 37 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS11='Y' AND METALID='S')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 18 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS12='Y' AND METALID IN('G','T'))"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 2 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS11='Y' AND METALID='G')"
            End If
            If GetSelectedCostId(chkCmbCostCentre, True) <> "''" Then
                strSql += vbCrLf + " AND COSTID IN('" & GetSelectedCostId(chkCmbCostCentre, True) & "')"
            End If
            strSql += vbCrLf + " AND TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "' "
            strSql += vbCrLf + " AND TRANTYPE  IN('RPU','PU') AND CATCODE NOT IN ('00067')"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " CONVERT(VARCHAR(10),TRANNO)TRANNO,''ACCODE,CONVERT(VARCHAR(20),TRANDATE,105)TRANDATE,CONVERT(VARCHAR(10),TRANTYPE)TRANTYPE,CATCODE,"
            strSql += vbCrLf + " TRANDATE TRANDATE1,CASE WHEN CATCODE NOT IN ('00009','00067') THEN STNWT ELSE 0 END RGRSWT,CASE WHEN CATCODE NOT IN ('00009','00067') THEN STNWT ELSE 0 END RNETWT,STNAMT RAMOUNT"
            strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPTSTONE"
            strSql += vbCrLf + " WHERE BATCHNO IN (SELECT BATCHNO FROM " & cnStockDb & "..RECEIPT WHERE 1=1  "
            strSql += vbCrLf + " AND COMPANYID='" & strCompanyId & "' "
            strSql += vbCrLf + " AND ISNULL(CANCEL,'')=''  "
            strSql += vbCrLf + " AND TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "') "
            If gridView.CurrentRow.Cells("RESULT").Value = 73 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID='P')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 64 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID='D')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 49 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS12='Y' AND METALID='S')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 37 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS11='Y' AND METALID='S')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 18 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS12='Y' AND METALID IN('G','T'))"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 2 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS11='Y' AND METALID='G')"
            End If
            If GetSelectedCostId(chkCmbCostCentre, True) <> "''" Then
                strSql += vbCrLf + " AND COSTID IN('" & GetSelectedCostId(chkCmbCostCentre, True) & "')"
            End If
            If GetSelectedCostId(chkCmbCostCentre, True) <> "''" Then
                strSql += vbCrLf + " AND COSTID IN('" & GetSelectedCostId(chkCmbCostCentre, True) & "')"
            End If
            strSql += vbCrLf + " AND TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "' "
            strSql += vbCrLf + " AND TRANTYPE IN('RPU','PU') "
            strSql += vbCrLf + " )X GROUP BY TRANNO,TRANDATE,TRANDATE1,TRANTYPE,CATCODE,ACCODE,AMOUNT"
            strSql += vbCrLf + " ORDER BY CONVERT(SMALLDATETIME,TRANDATE1),TRANNO,ACCODE"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
            strSql = vbCrLf + " SELECT * FROM TEMPTABLEDB..TEMPANEX" & userId & systemId & " ORDER BY SNO"
            Flag = True

        ElseIf gridView.CurrentRow.Cells("RESULT").Value = 4 Or gridView.CurrentRow.Cells("RESULT").Value = 39 Or gridView.CurrentRow.Cells("RESULT").Value = 6.5 Or gridView.CurrentRow.Cells("RESULT").Value = 41.5 Then
            strSql = "SELECT TRANNO,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=X.ACCODE)ACNAME,TRANDATE,CASE WHEN TRANTYPE='IIS' THEN 'MELTING ISSUE' ELSE TRANTYPE END TRANTYPE"
            strSql += vbCrLf + " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE=X.CATCODE)CATEGORY"
            strSql += vbCrLf + " ,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,NULL AMOUNT,TRANDATE1,IDENTITY(INT,1,1)SNO "
            strSql += vbCrLf + " INTO TEMPTABLEDB..TEMPANEX" & userId & systemId & ""
            strSql += vbCrLf + " FROM ("
            strSql += vbCrLf + "SELECT "
            strSql += vbCrLf + " CONVERT(VARCHAR(10),TRANNO)TRANNO,ACCODE,CONVERT(VARCHAR(20),TRANDATE,105)TRANDATE,CONVERT(VARCHAR(10),TRANTYPE)TRANTYPE,CATCODE,"
            strSql += vbCrLf + " TRANDATE TRANDATE1"
            strSql += vbCrLf + ",GRSWT GRSWT,NETWT NETWT,AMOUNT AMOUNT "
            strSql += vbCrLf + "FROM " & cnStockDb & "..ISSUE I"
            If gridView.CurrentRow.Cells("RESULT").Value = 6.5 Or gridView.CurrentRow.Cells("RESULT").Value = 41.5 Then
                strSql += vbCrLf + "WHERE TRANTYPE='IIN'"
            Else
                strSql += vbCrLf + "WHERE TRANTYPE='IIS' AND ACCODE IN('" & MELTINGACC & "') "
            End If
            strSql += vbCrLf + "AND COMPANYID='" & strCompanyId & "' "
            If gridView.CurrentRow.Cells("RESULT").Value = 4 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS11='Y' AND METALID='G')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 39 Or gridView.CurrentRow.Cells("RESULT").Value = 41.5 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS11='Y' AND METALID='S')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 6.5 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS11='Y' AND METALID='G')"
            End If
            strSql += vbCrLf + "AND ISNULL(CANCEL,'')=''  "
            strSql += vbCrLf + "AND TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "' "
            If GetSelectedCostId(chkCmbCostCentre, True) <> "''" Then
                strSql += vbCrLf + " AND COSTID IN('" & GetSelectedCostId(chkCmbCostCentre, True) & "')"
            End If
            strSql += vbCrLf + " )X GROUP BY TRANNO,TRANDATE,TRANDATE1,TRANTYPE,CATCODE,ACCODE"
            strSql += vbCrLf + " ORDER BY CONVERT(SMALLDATETIME,TRANDATE1),TRANNO,ACCODE"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
            Flag = True
        ElseIf gridView.CurrentRow.Cells("RESULT").Value = 5 Or gridView.CurrentRow.Cells("RESULT").Value = 40 Or gridView.CurrentRow.Cells("RESULT").Value = 18.4 Or gridView.CurrentRow.Cells("RESULT").Value = 49.6 Then
            strSql = "SELECT TRANNO,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=X.ACCODE)ACNAME,TRANDATE,CASE WHEN TRANTYPE='RRE' THEN 'MELTING RECEIPT' ELSE TRANTYPE END TRANTYPE"
            strSql += vbCrLf + " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE=X.CATCODE)CATEGORY"
            strSql += vbCrLf + " ,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,NULL AMOUNT,TRANDATE1,IDENTITY(INT,1,1)SNO "
            strSql += vbCrLf + " INTO TEMPTABLEDB..TEMPANEX" & userId & systemId & ""
            strSql += vbCrLf + " FROM ("
            strSql += vbCrLf + "SELECT "
            strSql += vbCrLf + " CONVERT(VARCHAR(10),TRANNO)+ case when refno<>0 then '  ( BILL-'+CONVERT(VARCHAR(10),case when refno<>0 then refno else '' end )+' )' ELSE '' END TRANNO,ACCODE,CONVERT(VARCHAR(20),TRANDATE,105)TRANDATE,CONVERT(VARCHAR(10),TRANTYPE)TRANTYPE,CATCODE,"
            strSql += vbCrLf + " TRANDATE TRANDATE1"
            strSql += vbCrLf + ",GRSWT GRSWT,NETWT NETWT,AMOUNT AMOUNT "
            strSql += vbCrLf + "FROM " & cnStockDb & "..RECEIPT I"
            If gridView.CurrentRow.Cells("RESULT").Value = 18.4 Or gridView.CurrentRow.Cells("RESULT").Value = 49.6 Then
                strSql += vbCrLf + "WHERE TRANTYPE='RIN' "
            Else
                strSql += vbCrLf + "WHERE TRANTYPE='RRE' AND ACCODE IN('" & MELTINGACC & "')"
            End If
            If gridView.CurrentRow.Cells("RESULT").Value = 5 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS11='Y' AND METALID='G')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 40 Then            
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS11='Y' AND METALID='S')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 49.6 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS11='N' AND METALID='S')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 18.4 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS12='Y' AND METALID='G')"
            End If
            strSql += vbCrLf + "AND COMPANYID='" & strCompanyId & "' "
            strSql += vbCrLf + "AND ISNULL(CANCEL,'')=''  "
            strSql += vbCrLf + "AND TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "' "
            If GetSelectedCostId(chkCmbCostCentre, True) <> "''" Then
                strSql += vbCrLf + " AND COSTID IN('" & GetSelectedCostId(chkCmbCostCentre, True) & "')"
            End If
            strSql += vbCrLf + " )X GROUP BY TRANNO,TRANDATE,TRANDATE1,TRANTYPE,CATCODE,ACCODE"
            strSql += vbCrLf + " ORDER BY CONVERT(SMALLDATETIME,TRANDATE1),TRANNO,ACCODE"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
            Flag = True
        ElseIf gridView.CurrentRow.Cells("RESULT").Value = 8 Or gridView.CurrentRow.Cells("RESULT").Value = 42.5 Or gridView.CurrentRow.Cells("RESULT").Value = 82 Then
            strSql = "SELECT TRANNO,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=X.ACCODE)ACNAME,TRANDATE,TRANTYPE"
            strSql += vbCrLf + " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE=X.CATCODE)CATEGORY"
            strSql += vbCrLf + " ,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(AMOUNT) AMOUNT,TRANDATE1,IDENTITY(INT,1,1)SNO "
            strSql += vbCrLf + " INTO TEMPTABLEDB..TEMPANEX" & userId & systemId & ""
            strSql += vbCrLf + " FROM ("
            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " CONVERT(VARCHAR(10),TRANNO)+ case when refno<>0 then '  ( BILL-'+CONVERT(VARCHAR(10),case when refno<>0 then refno else '' end )+' )' ELSE '' END TRANNO,ACCODE,CONVERT(VARCHAR(20),TRANDATE,105)TRANDATE,CONVERT(VARCHAR(10),TRANTYPE)TRANTYPE,CATCODE,"
            strSql += vbCrLf + " TRANDATE TRANDATE1"
            strSql += vbCrLf + ",GRSWT GRSWT,NETWT NETWT,AMOUNT AMOUNT "
            strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I"
            strSql += vbCrLf + " WHERE BATCHNO NOT IN (SELECT BATCHNO FROM " & cnStockDb & "..MELTINGDETAIL "
            strSql += vbCrLf + " WHERE RECISS='I'  "
            strSql += vbCrLf + " AND COMPANYID='" & strCompanyId & "' "
            strSql += vbCrLf + " AND ISNULL(CANCEL,'')=''  "
            strSql += vbCrLf + " AND TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "') "
            strSql += vbCrLf + " AND TRANTYPE='IIS'"
            strSql += vbCrLf + " AND ACCODE NOT IN('" & MELTINGACC & "')  "
            strSql += vbCrLf + " AND COMPANYID='" & strCompanyId & "' AND ISNULL(CANCEL,'')='' "
            If gridView.CurrentRow.Cells("RESULT").Value = 8 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS11='Y' AND METALID='G')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 42.5 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS11='Y' AND METALID='S')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 82 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS12='N' AND METALID='P')"
            End If
            strSql += vbCrLf + "AND CATCODE<>'00077'"
            strSql += vbCrLf + "AND TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "' "
            If GetSelectedCostId(chkCmbCostCentre, True) <> "''" Then
                strSql += vbCrLf + " AND COSTID IN('" & GetSelectedCostId(chkCmbCostCentre, True) & "')"
            End If
            strSql += vbCrLf + " )X GROUP BY TRANNO,TRANDATE,TRANDATE1,TRANTYPE,CATCODE,ACCODE"
            strSql += vbCrLf + " ORDER BY CONVERT(SMALLDATETIME,TRANDATE1),TRANNO,ACCODE"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
            Flag = True
        ElseIf gridView.CurrentRow.Cells("RESULT").Value = 12 Then
            strSql = "SELECT TRANNO,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=X.ACCODE)ACNAME,TRANDATE,TRANTYPE"
            strSql += vbCrLf + " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE=X.CATCODE)CATEGORY"
            strSql += vbCrLf + " ,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,NULL AMOUNT,TRANDATE1,IDENTITY(INT,1,1)SNO "
            strSql += vbCrLf + " INTO TEMPTABLEDB..TEMPANEX" & userId & systemId & ""
            strSql += vbCrLf + " FROM ("
            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " CONVERT(VARCHAR(10),TRANNO)TRANNO,ACCODE,CONVERT(VARCHAR(20),TRANDATE,105)TRANDATE,CONVERT(VARCHAR(10),TRANTYPE)TRANTYPE,CATCODE,"
            strSql += vbCrLf + " TRANDATE TRANDATE1"
            strSql += vbCrLf + ",GRSWT GRSWT,NETWT NETWT,AMOUNT AMOUNT "
            strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I"
            strSql += vbCrLf + " WHERE BATCHNO NOT IN (SELECT BATCHNO FROM " & cnStockDb & "..MELTINGDETAIL "
            strSql += vbCrLf + " WHERE RECISS='I'  "
            strSql += vbCrLf + " AND COMPANYID='" & strCompanyId & "' "
            strSql += vbCrLf + " AND ISNULL(CANCEL,'')=''  "
            strSql += vbCrLf + " AND TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "') "
            strSql += vbCrLf + " AND TRANTYPE IN('IIS','IAP','IPU')"
            strSql += vbCrLf + " AND COMPANYID='" & strCompanyId & "' AND ISNULL(CANCEL,'')='' "
            strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS12='Y' AND METALID='G')"
            strSql += vbCrLf + "AND CATCODE<>'00077' AND ACCODE NOT IN('" & MELTINGACC & "') "
            strSql += vbCrLf + "AND TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "' "
            If GetSelectedCostId(chkCmbCostCentre, True) <> "''" Then
                strSql += vbCrLf + " AND COSTID IN('" & GetSelectedCostId(chkCmbCostCentre, True) & "')"
            End If
            strSql += vbCrLf + " )X GROUP BY TRANNO,TRANDATE,TRANDATE1,TRANTYPE,CATCODE,ACCODE"
            strSql += vbCrLf + " ORDER BY CONVERT(SMALLDATETIME,TRANDATE1),TRANNO,ACCODE"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
            Flag = True
        ElseIf gridView.CurrentRow.Cells("RESULT").Value = 13 Then
            strSql = "SELECT TRANNO,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=X.ACCODE)ACNAME,TRANDATE,TRANTYPE"
            strSql += vbCrLf + ",(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE=X.CATCODE)CATEGORY"
            strSql += vbCrLf + ",SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,NULL AMOUNT,TRANDATE1,IDENTITY(INT,1,1)SNO "
            strSql += vbCrLf + "INTO TEMPTABLEDB..TEMPANEX" & userId & systemId & ""
            strSql += vbCrLf + "FROM ("
            strSql += vbCrLf + "SELECT  "
            strSql += vbCrLf + "CONVERT(VARCHAR(10),TRANNO)+ case when refno<>0 then '  ( BILL-'+CONVERT(VARCHAR(10),case when refno<>0 then refno else '' end )+' )' ELSE '' END TRANNO,ACCODE,CONVERT(VARCHAR(20),TRANDATE,105)TRANDATE,CONVERT(VARCHAR(10),TRANTYPE)TRANTYPE,CATCODE,"
            strSql += vbCrLf + " TRANDATE TRANDATE1"
            strSql += vbCrLf + ",GRSWT GRSWT,NETWT NETWT,AMOUNT AMOUNT "
            strSql += vbCrLf + "FROM  " & cnStockDb & "..RECEIPT I"
            strSql += vbCrLf + "WHERE BATCHNO NOT IN (SELECT BATCHNO FROM  " & cnStockDb & "..MELTINGDETAIL WHERE   RECISS='R'  "
            strSql += vbCrLf + "AND COMPANYID='" & strCompanyId & "'  AND ISNULL(CANCEL,'')=''  "
            strSql += vbCrLf + "AND TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "' ) "
            strSql += vbCrLf + "AND TRANTYPE IN('RRE','RAP')"
            strSql += vbCrLf + "AND COMPANYID='" & strCompanyId & "' "
            strSql += vbCrLf + "AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS12='Y' AND METALID='G')"
            strSql += vbCrLf + "AND CATCODE<>'00077'"
            strSql += vbCrLf + "AND ISNULL(CANCEL,'')=''  "
            strSql += vbCrLf + "AND TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "' "
            If GetSelectedCostId(chkCmbCostCentre, True) <> "''" Then
                strSql += vbCrLf + " AND COSTID IN('" & GetSelectedCostId(chkCmbCostCentre, True) & "')"
            End If
            strSql += vbCrLf + " )X GROUP BY TRANNO,TRANDATE,TRANDATE1,TRANTYPE,CATCODE,ACCODE"
            strSql += vbCrLf + " ORDER BY CONVERT(SMALLDATETIME,TRANDATE1),TRANNO,ACCODE"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
            Flag = True
        ElseIf gridView.CurrentRow.Cells("RESULT").Value = 26 Or gridView.CurrentRow.Cells("RESULT").Value = 59 _
        Or gridView.CurrentRow.Cells("RESULT").Value = 66 _
        Or gridView.CurrentRow.Cells("RESULT").Value = 7 _
        Or gridView.CurrentRow.Cells("RESULT").Value = 75 Then
            strSql = "SELECT TRANNO,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=X.ACCODE)ACNAME,TRANDATE,"
            strSql += vbCrLf + " CASE WHEN TRANTYPE='SA' THEN 'SALES'"
            strSql += vbCrLf + " WHEN TRANTYPE='SR' THEN 'SALES RETURN'"
            strSql += vbCrLf + " WHEN TRANTYPE='OD' THEN 'ORDER'"
            strSql += vbCrLf + " WHEN TRANTYPE='RD' THEN 'REPAIR' ELSE TRANTYPE END TRANTYPE "
            strSql += vbCrLf + " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE=X.CATCODE)CATEGORY"
            strSql += vbCrLf + " ,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(AMOUNT)AMOUNT,TRANDATE1 ,IDENTITY(INT,1,1)SNO"
            strSql += vbCrLf + " INTO TEMPTABLEDB..TEMPANEX" & userId & systemId & ""
            strSql += vbCrLf + " FROM ("
            strSql += vbCrLf + " SELECT"
            strSql += vbCrLf + " CONVERT(VARCHAR(10),TRANNO)TRANNO,ACCODE,CONVERT(VARCHAR(20),TRANDATE,105)TRANDATE,CONVERT(VARCHAR(10),TRANTYPE)TRANTYPE,I.CATCODE,"
            strSql += vbCrLf + " TRANDATE TRANDATE1"
            strSql += vbCrLf + " ,CASE WHEN I.METALID <> 'T' THEN (CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('W','B')) THEN I.GRSWT ELSE 0 END)ELSE 0 END AS GRSWT"
            strSql += vbCrLf + " ,CASE WHEN I.METALID <> 'T' THEN (CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('W','B')) THEN I.NETWT ELSE 0 END)ELSE 0 END AS NETWT"
            strSql += vbCrLf + " ,AMOUNT-ISNULL((SELECT SUM(STNAMT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO),0)  AMOUNT "
            strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID=I.ITEMID"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = I.ITEMID AND SM.SUBITEMID = I.SUBITEMID"
            strSql += vbCrLf + " WHERE BATCHNO NOT IN (SELECT BATCHNO FROM " & cnStockDb & "..MELTINGDETAIL WHERE  RECISS='I'  AND COMPANYID='" & strCompanyId & "' AND ISNULL(CANCEL,'')=''  "
            strSql += vbCrLf + " AND TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "' ) "
            strSql += vbCrLf + " AND SNO NOT IN(SELECT ISSSNO FROM " & cnStockDb & "..ISSMETAL WHERE 1=1  AND COMPANYID='" & strCompanyId & "' AND ISNULL(CANCEL,'')=''  "
            strSql += vbCrLf + " AND TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "' ) "
            strSql += vbCrLf + " AND I.COMPANYID='" & strCompanyId & "' AND ISNULL(CANCEL,'')='' "
            If gridView.CurrentRow.Cells("RESULT").Value = 26 Then
                strSql += vbCrLf + " AND I.CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS12='Y' AND METALID in('G','T') )"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 7 Then
                strSql += vbCrLf + " AND I.CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS11='Y' AND METALID='G')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 59 Then
                strSql += vbCrLf + " AND I.CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS12='Y' AND METALID='S')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 66 Then
                strSql += vbCrLf + " AND I.CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID='D')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 75 Then
                strSql += vbCrLf + " AND I.CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID='P')"
            End If
            strSql += vbCrLf + " AND TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "' "
            If GetSelectedCostId(chkCmbCostCentre, True) <> "''" Then
                strSql += vbCrLf + " AND COSTID IN('" & GetSelectedCostId(chkCmbCostCentre, True) & "')"
            End If
            strSql += vbCrLf + " AND TRANTYPE IN('SA','OD','RD')"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT  "
            strSql += vbCrLf + " CONVERT(VARCHAR(10),TRANNO)TRANNO,''ACCODE,CONVERT(VARCHAR(20),TRANDATE,105)TRANDATE,CONVERT(VARCHAR(10),TRANTYPE)TRANTYPE,CATCODE,"
            strSql += vbCrLf + " TRANDATE TRANDATE1"
            strSql += vbCrLf + " ,CASE WHEN CATCODE NOT IN ('00009','00067') THEN STNWT ELSE 0 END GRSWT,CASE WHEN CATCODE NOT IN ('00009','00067') THEN STNWT ELSE 0 END NETWT,STNAMT AMOUNT "
            strSql += vbCrLf + " FROM " & cnStockDb & "..ISSSTONE "
            strSql += vbCrLf + " WHERE BATCHNO IN (SELECT BATCHNO FROM " & cnStockDb & "..ISSUE WHERE 1=1  AND COMPANYID='" & strCompanyId & "' AND ISNULL(CANCEL,'')=''  "
            strSql += vbCrLf + " AND TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "' ) "
            If GetSelectedCostId(chkCmbCostCentre, True) <> "''" Then
                strSql += vbCrLf + " AND COSTID IN('" & GetSelectedCostId(chkCmbCostCentre, True) & "')"
            End If
            strSql += vbCrLf + " AND TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "'  "
            If gridView.CurrentRow.Cells("RESULT").Value = 26 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS12='Y' AND METALID in('G','T'))"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 7 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS11='Y' AND METALID='G')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 59 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS12='Y' AND METALID='S')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 66 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID='D')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 75 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID='P')"
            End If
            strSql += vbCrLf + " AND TRANTYPE IN('SA','OD','RD')"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT  "
            strSql += vbCrLf + " CONVERT(VARCHAR(10),TRANNO)TRANNO,''ACCODE,CONVERT(VARCHAR(20),TRANDATE,105)TRANDATE,CONVERT(VARCHAR(10),TRANTYPE)TRANTYPE,CATCODE,"
            strSql += vbCrLf + " TRANDATE TRANDATE1"
            strSql += vbCrLf + " ,GRSWT IGRSWT,GRSWT INETWT,AMOUNT IAMOUNT "
            strSql += vbCrLf + " FROM " & cnStockDb & "..ISSMETAL I"
            strSql += vbCrLf + " WHERE ISSSNO IN (SELECT SNO FROM " & cnStockDb & "..ISSUE WHERE 1=1  AND COMPANYID='" & strCompanyId & "' AND ISNULL(CANCEL,'')=''  "
            strSql += vbCrLf + " AND TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "' ) "
            If GetSelectedCostId(chkCmbCostCentre, True) <> "''" Then
                strSql += vbCrLf + " AND COSTID IN('" & GetSelectedCostId(chkCmbCostCentre, True) & "')"
            End If
            strSql += vbCrLf + " AND TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "'  "
            If gridView.CurrentRow.Cells("RESULT").Value = 26 Then
                strSql += vbCrLf + " AND I.CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS12='Y' AND METALID='G')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 7 Then
                strSql += vbCrLf + " AND I.CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS11='Y' AND METALID='G')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 59 Then
                strSql += vbCrLf + " AND I.CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS12='Y' AND METALID='S')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 66 Then
                strSql += vbCrLf + " AND I.CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID='D')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 75 Then
                strSql += vbCrLf + " AND I.CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID='P')"
            End If
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT  "
            strSql += vbCrLf + " CONVERT(VARCHAR(10),TRANNO)TRANNO,ACCODE,CONVERT(VARCHAR(20),TRANDATE,105)TRANDATE,CONVERT(VARCHAR(10),TRANTYPE),I.CATCODE,"
            strSql += vbCrLf + " TRANDATE TRANDATE1"
            strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('W','B')) THEN -1*I.GRSWT ELSE 0 END AS GRSWT"
            strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('W','B')) THEN -1*I.NETWT ELSE 0 END AS NETWT"
            strSql += vbCrLf + " ,-1* (AMOUNT-ISNULL((SELECT SUM(STNAMT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO),0))  AMOUNT "
            strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT I"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID=I.ITEMID"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = I.ITEMID AND SM.SUBITEMID = I.SUBITEMID"
            strSql += vbCrLf + " WHERE I.COMPANYID='" & strCompanyId & "' AND ISNULL(CANCEL,'')='' "
            If GetSelectedCostId(chkCmbCostCentre, True) <> "''" Then
                strSql += vbCrLf + " AND COSTID IN('" & GetSelectedCostId(chkCmbCostCentre, True) & "')"
            End If
            strSql += vbCrLf + " AND TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "'  "
            If gridView.CurrentRow.Cells("RESULT").Value = 26 Then
                strSql += vbCrLf + " AND I.CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS12='Y' AND METALID='G')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 7 Then
                strSql += vbCrLf + " AND I.CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS11='Y' AND METALID='G')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 59 Then
                strSql += vbCrLf + " AND I.CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS12='Y' AND METALID='S')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 66 Then
                strSql += vbCrLf + " AND I.CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID='D')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 75 Then
                strSql += vbCrLf + " AND I.CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID='P')"
            End If
            strSql += vbCrLf + " AND TRANTYPE='SR'"
            strSql += vbCrLf + " AND BATCHNO NOT IN(SELECT BATCHNO FROM " & cnStockDb & "..RECEIPTMETAL WHERE 1=1  AND COMPANYID='" & strCompanyId & "' AND TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "'  )"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT  "
            strSql += vbCrLf + " CONVERT(VARCHAR(10),TRANNO)TRANNO,''ACCODE,CONVERT(VARCHAR(20),TRANDATE,105)TRANDATE,CONVERT(VARCHAR(10),TRANTYPE)TRANTYPE,CATCODE,"
            strSql += vbCrLf + " TRANDATE TRANDATE1"
            strSql += vbCrLf + " ,-1*CASE WHEN CATCODE NOT IN ('00009','00067') THEN STNWT ELSE 0 END IGRSWT,-1*CASE WHEN CATCODE NOT IN ('00009','00067') THEN STNWT ELSE 0 END INETWT,-1*STNAMT IAMOUNT "
            strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPTSTONE "
            strSql += vbCrLf + " WHERE BATCHNO IN (SELECT BATCHNO FROM " & cnStockDb & "..RECEIPT WHERE 1=1  AND COMPANYID='" & strCompanyId & "' AND ISNULL(CANCEL,'')=''  "
            strSql += vbCrLf + " AND TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "' ) "
            If GetSelectedCostId(chkCmbCostCentre, True) <> "''" Then
                strSql += vbCrLf + " AND COSTID IN('" & GetSelectedCostId(chkCmbCostCentre, True) & "')"
            End If
            strSql += vbCrLf + " AND TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "'  "
            If gridView.CurrentRow.Cells("RESULT").Value = 26 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS12='Y' AND METALID in('G','T'))"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 7 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS11='Y' AND METALID='G')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 59 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE GS12='Y' AND METALID='S')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 66 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID='D')"
            ElseIf gridView.CurrentRow.Cells("RESULT").Value = 75 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID='P')"
            End If
            strSql += vbCrLf + " AND TRANTYPE='SR'"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT  "
            strSql += vbCrLf + " CONVERT(VARCHAR(10),TRANNO)TRANNO,''ACCODE,CONVERT(VARCHAR(20),TRANDATE,105)TRANDATE,CONVERT(VARCHAR(10),TRANTYPE)TRANTYPE,CATCODE,"
            strSql += vbCrLf + " TRANDATE TRANDATE1,-1*GRSWT IGRSWT,-1*GRSWT INETWT,-1*AMOUNT IAMOUNT "
            strSql += vbCrLf + " FROM  " & cnStockDb & "..RECEIPTMETAL I"
            strSql += vbCrLf + " WHERE BATCHNO IN (SELECT BATCHNO FROM " & cnStockDb & "..RECEIPT WHERE 1=1 AND ISNULL(CANCEL,'')='' AND COMPANYID='" & strCompanyId & "' AND TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "' ) "
            If gridView.CurrentRow.Cells("RESULT").Value = 75 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID='P')"
            End If
            If gridView.CurrentRow.Cells("RESULT").Value = 26 Or gridView.CurrentRow.Cells("RESULT").Value = 7 Then
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID='G')"
            End If
            strSql += vbCrLf + " AND TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "' "
            strSql += vbCrLf + " )X GROUP BY TRANNO,TRANDATE,TRANDATE1,TRANTYPE,CATCODE,ACCODE"
            strSql += vbCrLf + " ORDER BY CONVERT(SMALLDATETIME,TRANDATE1),CONVERT(INT,TRANNO),ACCODE"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
            Flag = True
            End If
            If Not Flag Then
            If gridView.CurrentRow.Cells("RESULT").Value = 14 Or gridView.CurrentRow.Cells("RESULT").Value = 72.3 Then
                strSql = vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMPANNEXSMITH1','U') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPANNEXSMITH1"
                cmd = New OleDbCommand(strSql, cn)
                cmd.ExecuteNonQuery()
                strSql = vbCrLf + " SELECT TRANNO,ACCODE"
                strSql += vbCrLf + " ,SUM(ISNULL(NETWT,0)+CASE WHEN TRANTYPE NOT IN ('MI','AR') THEN ISNULL(ALLOY,0)+ISNULL(WASTAGE,0) ELSE 0 END) NETWT"
                strSql += vbCrLf + " ,CATCODE,'R' TRANTYPE"
                strSql += vbCrLf + " INTO TEMPTABLEDB..TEMPANNEXSMITH1 "
                strSql += vbCrLf + " FROM  " & cnStockDb & "..RECEIPT  WHERE TRANTYPE NOT IN('RPU','IPU','RIN') "
                strSql += vbCrLf + "  AND TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "'  AND COMPANYID='" & strCompanyId & "' AND ISNULL(CANCEL,'')=''  AND METALID IN(SELECT METALID FROM TEMPTABLEDB..METALMAST ) "
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN('G','S','O','P','D'))"
                strSql += vbCrLf + " AND ISNULL(ACCODE,'') NOT IN('" & MELTINGACC & "') "
                strSql += vbCrLf + " AND ISNULL(ACCODE,'') <> '' "
                strSql += vbCrLf + " AND ISNULL(CATCODE,'') <> '00077' "
                strSql += vbCrLf + " GROUP BY CATCODE,TRANNO,ACCODE"
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT TRANNO,ACCODE"
                strSql += vbCrLf + " ,SUM(ISNULL(NETWT,0)+CASE WHEN TRANTYPE NOT IN ('MI','AI') THEN ISNULL(ALLOY,0)-ISNULL(WASTAGE,0) ELSE 0 END)  NETWT"
                strSql += vbCrLf + " ,CATCODE "
                strSql += vbCrLf + " ,'I' TRANTYPE FROM " & cnStockDb & "..ISSUE WHERE TRANTYPE NOT IN('RPU','IPU','SA','IIN') "
                strSql += vbCrLf + "  AND TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "'  AND COMPANYID='" & strCompanyId & "' AND ISNULL(CANCEL,'')=''  AND METALID IN(SELECT METALID FROM TEMPTABLEDB..METALMAST ) "
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN('G','S','O','P','D'))"
                strSql += vbCrLf + " AND ISNULL(ACCODE,'') NOT IN('" & MELTINGACC & "') "
                strSql += vbCrLf + " AND ISNULL(ACCODE,'') <> '' "
                strSql += vbCrLf + " AND ISNULL(CATCODE,'') <> '00077' "
                strSql += vbCrLf + " GROUP BY CATCODE,TRANNO,ACCODE"
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT '' TRANNO,ACCODE"
                strSql += vbCrLf + " ,SUM(ISNULL(NETWT,0))  NETWT"
                strSql += vbCrLf + " ,CATCODE "
                strSql += vbCrLf + " ,'I' TRANTYPE FROM " & cnStockDb & "..OPENWEIGHT WHERE TRANTYPE NOT IN('RPU','IPU','SA') "
                strSql += vbCrLf + " AND STOCKTYPE='S' "
                strSql += vbCrLf + " AND COMPANYID ='" & strCompanyId & "'"
                strSql += vbCrLf + " AND TRANTYPE='I'"
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN('G','S','O','P','D'))"
                strSql += vbCrLf + " AND ISNULL(ACCODE,'') NOT IN('" & MELTINGACC & "') "
                strSql += vbCrLf + " AND ISNULL(ACCODE,'') <> '' "
                strSql += vbCrLf + " AND ISNULL(CATCODE,'') <> '00077' "
                strSql += vbCrLf + " GROUP BY CATCODE,ACCODE"
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT '' TRANNO,ACCODE "
                strSql += vbCrLf + " ,SUM(ISNULL(NETWT,0))  NETWT"
                strSql += vbCrLf + " ,CATCODE "
                strSql += vbCrLf + " ,'R' TRANTYPE FROM " & cnStockDb & "..OPENWEIGHT WHERE TRANTYPE NOT IN('RPU','IPU','SA','RIN') "
                strSql += vbCrLf + " AND STOCKTYPE='S' "
                strSql += vbCrLf + " AND COMPANYID ='" & strCompanyId & "'"
                strSql += vbCrLf + " AND TRANTYPE='R'"
                strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN('G','S','O','P','D'))"
                strSql += vbCrLf + " AND ISNULL(ACCODE,'') NOT IN('" & MELTINGACC & "') "
                strSql += vbCrLf + " AND ISNULL(ACCODE,'') <> '' "
                strSql += vbCrLf + " AND ISNULL(CATCODE,'') <> '00077' "
                strSql += vbCrLf + " GROUP BY CATCODE,ACCODE"
                cmd = New OleDbCommand(strSql, cn)
                cmd.ExecuteNonQuery()
                If flagSummary = True Then
                    If gridView.CurrentRow.Cells("RESULT").Value = 14 Then
                        strSql = vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMPANNEXSMITHGTOT','U') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPANNEXSMITHGTOT"
                        cmd = New OleDbCommand(strSql, cn)
                        cmd.ExecuteNonQuery()
                        strSql = vbCrLf + " SELECT CONVERT (VARCHAR(25),'') TRANTYPE,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD A WHERE A.ACCODE=S.ACCODE )ACNAME,"
                        strSql += vbCrLf + " SUM(CASE WHEN S.TRANTYPE='I' THEN NETWT ELSE -1*NETWT END)NETWT"
                        strSql += vbCrLf + " ,'1' RESULT,''COLHEAD"
                        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMPANNEXSMITHGTOT"
                        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMPANNEXSMITH1 S "
                        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE=S.CATCODE WHERE C.METALID='G'"
                        strSql += vbCrLf + " GROUP BY ACCODE"
                        cmd = New OleDbCommand(strSql, cn)
                        cmd.ExecuteNonQuery()
                        strSql = vbCrLf + " INSERT INTO  TEMPTABLEDB..TEMPANNEXSMITHGTOT(TRANTYPE,NETWT,RESULT,COLHEAD)"
                        strSql += vbCrLf + " SELECT 'TOTAL',SUM(NETWT),'2'RESULT,'G'COLHEAD FROM TEMPTABLEDB..TEMPANNEXSMITHGTOT"
                        strSql += vbCrLf + " UNION ALL"
                        strSql += vbCrLf + " Select  DISTINCT 'SMITH DETAILS',NULL,'0'RESULT,'T'COLHEAD FROM TEMPTABLEDB..TEMPANNEXSMITHGTOT"
                        cmd = New OleDbCommand(strSql, cn)
                        cmd.ExecuteNonQuery()
                        strSql = vbCrLf + " SELECT * FROM TEMPTABLEDB..TEMPANNEXSMITHGTOT ORDER BY RESULT"
                        Flag = True
                    End If
                    If gridView.CurrentRow.Cells("RESULT").Value = 72.3 Then
                        strSql = vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMPANNEXSMITHGTOT','U') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPANNEXSMITHGTOT"
                        cmd = New OleDbCommand(strSql, cn)
                        cmd.ExecuteNonQuery()
                        strSql = vbCrLf + " SELECT CONVERT (VARCHAR(25),'') TRANTYPE,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD A WHERE A.ACCODE=S.ACCODE )ACNAME,"
                        strSql += vbCrLf + " SUM(CASE WHEN S.TRANTYPE='I' THEN NETWT ELSE -1*NETWT END)NETWT"
                        strSql += vbCrLf + " ,'1' RESULT,''COLHEAD"
                        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMPANNEXSMITHGTOT"
                        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMPANNEXSMITH1 S "
                        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE=S.CATCODE WHERE C.METALID='P'"
                        strSql += vbCrLf + " GROUP BY ACCODE"
                        cmd = New OleDbCommand(strSql, cn)
                        cmd.ExecuteNonQuery()
                        strSql = vbCrLf + " INSERT INTO  TEMPTABLEDB..TEMPANNEXSMITHGTOT(TRANTYPE,NETWT,RESULT,COLHEAD)"
                        strSql += vbCrLf + " SELECT 'TOTAL',SUM(NETWT),'2'RESULT,'G'COLHEAD FROM TEMPTABLEDB..TEMPANNEXSMITHGTOT"
                        strSql += vbCrLf + " UNION ALL"
                        strSql += vbCrLf + " Select  DISTINCT 'SMITH DETAILS',NULL,'0'RESULT,'T'COLHEAD FROM TEMPTABLEDB..TEMPANNEXSMITHGTOT"
                        cmd = New OleDbCommand(strSql, cn)
                        cmd.ExecuteNonQuery()
                        strSql = vbCrLf + " SELECT * FROM TEMPTABLEDB..TEMPANNEXSMITHGTOT ORDER BY RESULT"
                        Flag = True
                    End If
                End If
            End If
                If Not Flag Then Exit Sub
            Else
                strSql = vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMPANEXFL" & userId & systemId & "','U') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPANEXFL" & userId & systemId & ""
                cmd = New OleDbCommand(strSql, cn)
                cmd.ExecuteNonQuery()
                If flagSummary = True Then
                    'strSql = vbCrLf + " SELECT TRANTYPE,CATEGORY,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(AMOUNT)AMOUNT FROM TEMPTABLEDB..TEMPANEX" & userId & systemId & " GROUP BY CATEGORY,TRANTYPE "
                    strSql = vbCrLf + " SELECT TRANTYPE,CATEGORY,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(AMOUNT)AMOUNT,'1' RESULT,'D'COLHEAD,IDENTITY(INT,1,1)SNO"
                    strSql += vbCrLf + " INTO TEMPTABLEDB..TEMPANEXFL" & userId & systemId & " FROM TEMPTABLEDB..TEMPANEX" & userId & systemId & " GROUP BY CATEGORY,TRANTYPE "
                    cmd = New OleDbCommand(strSql, cn)
                    cmd.ExecuteNonQuery()
                    strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPANEXFL" & userId & systemId & " (TRANTYPE,GRSWT,NETWT,AMOUNT,RESULT,COLHEAD)"
                    strSql += vbCrLf + " SELECT 'TOTAL',SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(AMOUNT)AMOUNT,'2' RESULT,'G'COLHEAD"
                    strSql += vbCrLf + " FROM TEMPTABLEDB..TEMPANEX" & userId & systemId & ""
                    cmd = New OleDbCommand(strSql, cn)
                    cmd.ExecuteNonQuery()
                    strSql = vbCrLf + " SELECT * FROM TEMPTABLEDB..TEMPANEXFL" & userId & systemId & " ORDER BY RESULT"
                Else
                    strSql = vbCrLf + " ALTER TABLE TEMPTABLEDB..TEMPANEX" & userId & systemId & " ADD COLHEAD VARCHAR(1)"
                    cmd = New OleDbCommand(strSql, cn)
                    cmd.ExecuteNonQuery()
                    strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPANEX" & userId & systemId & "(TRANTYPE,GRSWT,NETWT,AMOUNT,COLHEAD)"
                    strSql += vbCrLf + " SELECT 'TOTAL',SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(AMOUNT)AMOUNT,'G' COLHEAD"
                    strSql += vbCrLf + " FROM TEMPTABLEDB..TEMPANEX" & userId & systemId & ""
                    cmd = New OleDbCommand(strSql, cn)
                    cmd.ExecuteNonQuery()
                    strSql = vbCrLf + " SELECT * FROM TEMPTABLEDB..TEMPANEX" & userId & systemId & " ORDER BY SNO"
                End If
            End If
            dtGridView = New DataTable("DETAILED")
            Dim dtCol As New DataColumn("KEYNO")
            dtCol.AutoIncrement = True
            dtCol.AutoIncrementSeed = 0
            dtCol.AutoIncrementStep = 1
            dtGridView.Columns.Add(dtCol)
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtGridView)
            If dtGridView.Rows.Count > 0 Then
                With GridViewDetail
                    .DefaultCellStyle = Nothing
                    .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
                    .DataSource = dtGridView
                    If .Columns.Contains("TRANDATE1") Then .Columns("TRANDATE1").Visible = False
                    If .Columns.Contains("SNO") Then .Columns("SNO").Visible = False
                    FormatGridColumns(GridViewDetail, False, True, , False)
                    .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    If flagSummary = True Then
                        For Each dgvRow As DataGridViewRow In .Rows
                            If dgvRow.Cells("TRANTYPE").Value.ToString = "TOTAL" Then
                                dgvRow.DefaultCellStyle = reportSubTotalStyle
                            End If
                        Next
                    Else
                        For Each dgvRow As DataGridViewRow In .Rows
                            If dgvRow.Cells("TRANNO").Value.ToString = "TOTAL" Then
                                dgvRow.DefaultCellStyle = reportSubTotalStyle
                            End If
                        Next
                    End If
                    .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                    .Invalidate()
                    For Each dgvCol As DataGridViewColumn In .Columns
                        dgvCol.Width = dgvCol.Width
                    Next
                    .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
                    PnlRange.Visible = True
                    flagDetail = True
                    'txtGrsWt.Text = Format(dtGridView.Compute("SUM(GRSWT)", Nothing), "#0.000#")
                    'txtNetWt.Text = Format(dtGridView.Compute("SUM(NETWT)", Nothing), "#0.000#")
                    'txtAmt.Text = Format(dtGridView.Compute("SUM(AMOUNT)", Nothing), "#0.00#")
                    .Select()
                End With
                objGridShower = New frmGridDispDia
                objGridShower.Name = Me.Name
                objGridShower.gridView.RowTemplate.Height = 21
                objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                objGridShower.Size = New Size(615, 376)
                objGridShower.Text = " STOCK REGISTER  " + Label8.Text
                Dim tit As String = rowTitle
                Dim Cname As String = Label8.Text
                objGridShower.lblTitle.Text = metalTitle + Environment.NewLine + tit + Environment.NewLine + " FROM " + dtpFrom.Text + " TO " + dtpTo.Text
                objGridShower.StartPosition = FormStartPosition.CenterScreen
                objGridShower.dsGrid.DataSetName = objGridShower.Name
                objGridShower.dsGrid.Tables.Add(dtGridView)
                objGridShower.gridView.DataSource = Nothing
                objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
                If objGridShower.gridView.Columns.Contains("TRANDATE1") Then objGridShower.gridView.Columns("TRANDATE1").Visible = False
                If objGridShower.gridView.Columns.Contains("RESULT") Then objGridShower.gridView.Columns("RESULT").Visible = False
                'If objGridShower.gridView.Columns.Contains("COLHEAD") Then .Columns("RESULT").Visible = False            
                If objGridShower.gridView.Columns.Contains("COLHEAD") Then objGridShower.gridView.Columns("COLHEAD").Visible = False
                If objGridShower.gridView.Columns.Contains("SNO") Then objGridShower.gridView.Columns("SNO").Visible = False
                If objGridShower.gridView.Columns.Contains("KEYNO") Then objGridShower.gridView.Columns("KEYNO").Visible = False
                FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
                If objGridShower.gridView.Columns.Contains("AMOUNT") Then
                    With objGridShower.gridView
                        With .Columns("AMOUNT")
                            .Width = 170
                            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                            .SortMode = DataGridViewColumnSortMode.NotSortable
                            .DefaultCellStyle.Format = "##,##,##,###.00"
                        End With
                        With .Columns("GRSWT")
                            .Width = 170
                            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                            .SortMode = DataGridViewColumnSortMode.NotSortable
                            .DefaultCellStyle.Format = "##0.000"
                        End With
                        With .Columns("NETWT")
                            .Width = 170
                            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                            .SortMode = DataGridViewColumnSortMode.NotSortable
                            .DefaultCellStyle.Format = "##0.000"
                        End With
                    End With
                End If
                objGridShower.pnlFooter.Visible = False
                objGridShower.FormReLocation = False
                objGridShower.FormReSize = True
                PnlRange.Visible = False
                flagDetail = False
                gridView.Focus()
                objGridShower.Show()
                FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
            End If
    End Sub
     
   
    Private Sub GrpRange_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles GrpRange.KeyDown
        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Escape Then
            GridViewDetail.DataSource = Nothing
            GridViewDetail.Refresh()
            PnlRange.Visible = False
            flagDetail = False
            gridView.Focus()
        End If
    End Sub

    Private Sub GridViewDetail_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles GridViewDetail.KeyDown
        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Escape Then
            GridViewDetail.DataSource = Nothing
            GridViewDetail.Refresh()
            PnlRange.Visible = False
            flagDetail = False
            gridView.Focus()
        End If
        If GridViewDetail.RowCount <= 0 Then Exit Sub
        If e.KeyCode = Keys.X Then
            If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
            If GridViewDetail.Rows.Count > 0 Then
                BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, GridViewDetail, BrightPosting.GExport.GExportType.Export)
            End If
        End If
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub
    Function GridViewFormatshower() As Integer
        For Each dgvRow As DataGridViewRow In gridView.Rows
            With dgvRow
                Select Case .Cells("COLHEAD").Value.ToString
                    Case "S"
                        .DefaultCellStyle.ForeColor = reportSubTotalStyle.ForeColor
                        .DefaultCellStyle.Font = reportSubTotalStyle.Font
                    Case "G"
                        .DefaultCellStyle.ForeColor = reportSubTotalStyle.ForeColor
                        .DefaultCellStyle.Font = reportSubTotalStyle.Font
                    Case "T"
                        .Cells("PARTICULARS").Style.BackColor = reportHeadStyle.BackColor
                        .Cells("PARTICULARS").Style.Font = reportHeadStyle.Font
                End Select
            End With
        Next
    End Function
    Function GridViewFormat() As Integer
        For Each dgvRow As DataGridViewRow In gridView.Rows
            With dgvRow
                Select Case .Cells("COLHEAD").Value.ToString
                    Case "S"
                        .Cells("PARTICULARS").Style.BackColor = reportHeadStyle.BackColor
                        .DefaultCellStyle.ForeColor = reportSubTotalStyle.ForeColor
                        .DefaultCellStyle.Font = reportSubTotalStyle.Font

                    Case "G"
                        '.DefaultCellStyle.ForeColor = reportSubTotalStyle.ForeColor
                        If .Cells("WEIGHT").Value.ToString <> "" Then
                            .Cells("WEIGHT").Style.ForeColor = Color.White
                            .Cells("WEIGHT").Style.BackColor = Color.DarkGreen
                        End If

                        If .Cells("AMOUNT").Value.ToString <> "" Then
                            .Cells("AMOUNT").Style.ForeColor = Color.White
                            .Cells("AMOUNT").Style.BackColor = Color.DarkGreen
                        End If

                        If .Cells("RATE").Value.ToString <> "" Then
                            .Cells("RATE").Style.ForeColor = Color.White
                            .Cells("RATE").Style.BackColor = Color.DarkGreen
                        End If
                        
                        If .Cells("PARTICULARS").Value.ToString <> "" Then
                            .Cells("PARTICULARS").Style.ForeColor = Color.DarkGreen
                        End If

                        .DefaultCellStyle.Font = reportSubTotalStyle.Font
                    Case "T"
                        .Cells("PARTICULARS").Style.BackColor = reportHeadStyle.BackColor
                        .Cells("PARTICULARS").Style.Font = reportHeadStyle.Font

                End Select
            End With
        Next
        With gridView
            'If rbtGs12Grswt.Checked Then
            '    .Columns("NETWT").Visible = False
            'Else
            '    .Columns("GRSWT").Visible = False
            'End If
            .Columns("COLHEAD").Visible = False
            .Columns("CATEGORY").Visible = False
            .Columns("RESULT").Visible = False
            With .Columns("PARTICULARS")
                .HeaderText = "PARTICULAR"
                .ReadOnly = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("WEIGHT")
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .ReadOnly = True
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("GRSWT")
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .ReadOnly = True
                .Visible = False
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("NETWT")
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .ReadOnly = True
                .Visible = False
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("AMOUNT")
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .ReadOnly = True
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("RATE")
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .ReadOnly = True
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        End With
    End Function

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown

        If gridView.RowCount <= 0 Then Exit Sub
        If e.KeyCode = Keys.D And flagDetail = False Then
            Label8.Text = "DETAIL VIEW"
            rowTitle = gridView.CurrentRow.Cells("PARTICULARS").Value.ToString
            metalTitle = gridView.CurrentRow.Cells("PARTHEAD").Value.ToString
            funcDetailView()
        End If
        If e.KeyCode = Keys.S And flagDetail = False Then
            Label8.Text = "SUMMARY VIEW"
            rowTitle = gridView.CurrentRow.Cells("PARTICULARS").Value.ToString
            metalTitle = gridView.CurrentRow.Cells("PARTHEAD").Value.ToString
            flagSummary = True
            funcDetailView()
            flagSummary = False
        End If
    End Sub
    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If gridView.RowCount > 0 Then
            If UCase(e.KeyChar) = "X" Then
                btnExcel_Click(Me, New EventArgs)
            End If
            If UCase(e.KeyChar) = "P" Then
                btnPrint_Click(Me, New EventArgs)
            End If
        End If
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        cmbMetal.Items.Add("ALL")
        strSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST ORDER BY METALNAME"
        objGPack.FillCombo(strSql, cmbMetal, False)
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub dtpFrom_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        dtpTo.MinimumDate = dtpFrom.Value
    End Sub

    Private Sub frmCategoryStock_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        strSql = " SELECT 'ALL' COMPANYNAME,'ALL' COMPANYID,1 RESULT"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT COMPANYNAME,COMPANYID,2 RESULT FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N'"
        strSql += vbCrLf + " ORDER BY RESULT,COMPANYNAME"
        dtCompany = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCompany)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dtCompany, "COMPANYNAME", , "ALL")
        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += vbCrLf + " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False
        btnNew_Click(Me, e)
        If MELTINGACC <> "" Then MELTINGACC = Replace(MELTINGACC, ",", "','")
    End Sub
    Private Sub Prop_Sets()
        Dim obj As New frmStockRegister_Properties
        GetChecked_CheckedList(chkCmbCompany, obj.p_chkCmbCompany)
        'GetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre)
        obj.p_cmbMetal = cmbMetal.Text
        obj.p_rbtGrsWt = rbtGs12Grswt.Checked
        obj.p_rbtNetWt = rbtGs12Netwt.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmStockRegister_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmStockRegister_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmStockRegister_Properties))
        SetChecked_CheckedList(chkCmbCompany, obj.p_chkCmbCompany, "ALL")
        'SetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre, "ALL")
        cmbMetal.Text = obj.p_cmbMetal
        rbtGs12Grswt.Checked = obj.p_rbtGrsWt
        rbtGs12Netwt.Checked = obj.p_rbtNetWt
    End Sub

    Private Sub AutoResizeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AutoResizeToolStripMenuItem.Click
        If AutoResizeToolStripMenuItem.Checked = False Then
            AutoResizeToolStripMenuItem.Checked = True
        Else
            AutoResizeToolStripMenuItem.Checked = False
        End If
        If gridView.RowCount > 0 Then
            If AutoResizeToolStripMenuItem.Checked Then
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

End Class

Public Class frmStockRegister_Properties
    Private chkCmbCompany As New List(Of String)
    Public Property p_chkCmbCompany() As List(Of String)
        Get
            Return chkCmbCompany
        End Get
        Set(ByVal value As List(Of String))
            chkCmbCompany = value
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

    Private cmbMetal As String = "ALL"
    Public Property p_cmbMetal() As String
        Get
            Return cmbMetal
        End Get
        Set(ByVal value As String)
            cmbMetal = value
        End Set
    End Property

    Private cmbMetalType As String = "ALL"
    Public Property p_cmbMetalType() As String
        Get
            Return cmbMetalType
        End Get
        Set(ByVal value As String)
            cmbMetalType = value
        End Set
    End Property
    Private rbtNetWt As Boolean = True
    Public Property p_rbtNetWt() As Boolean
        Get
            Return rbtNetWt
        End Get
        Set(ByVal value As Boolean)
            rbtNetWt = value
        End Set
    End Property

    Private rbtGrsWt As Boolean = False
    Public Property p_rbtGrsWt() As Boolean
        Get
            Return rbtGrsWt
        End Get
        Set(ByVal value As Boolean)
            rbtGrsWt = value
        End Set
    End Property

    Private cmbCatName As String = "ALL"
    Public Property p_cmbCatName() As String
        Get
            Return cmbCatName
        End Get
        Set(ByVal value As String)
            cmbCatName = value
        End Set
    End Property

    Private rbtGeneral As Boolean = True
    Public Property p_rbtGeneral() As Boolean
        Get
            Return rbtGeneral
        End Get
        Set(ByVal value As Boolean)
            rbtGeneral = value
        End Set
    End Property

    Private rbtGram As Boolean = False
    Public Property p_rbtGram() As Boolean
        Get
            Return rbtGram
        End Get
        Set(ByVal value As Boolean)
            rbtGram = value
        End Set
    End Property

    Private rbtCarat As Boolean = False
    Public Property p_rbtCarat() As Boolean
        Get
            Return rbtCarat
        End Get
        Set(ByVal value As Boolean)
            rbtCarat = value
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

    Private rbtMonthWise As Boolean = False
    Public Property p_rbtMonthWise() As Boolean
        Get
            Return rbtMonthWise
        End Get
        Set(ByVal value As Boolean)
            rbtMonthWise = value
        End Set
    End Property

    Private rbtDateWise As Boolean = False
    Public Property p_rbtDateWise() As Boolean
        Get
            Return rbtDateWise
        End Get
        Set(ByVal value As Boolean)
            rbtDateWise = value
        End Set
    End Property

    Private rbtTranNoWise As Boolean = False
    Public Property p_rbtTranNoWise() As Boolean
        Get
            Return rbtTranNoWise
        End Get
        Set(ByVal value As Boolean)
            rbtTranNoWise = value
        End Set
    End Property

    Private chkPcs As Boolean = False
    Public Property p_chkWithPcs() As Boolean
        Get
            Return chkPcs
        End Get
        Set(ByVal value As Boolean)
            chkPcs = value
        End Set
    End Property

    Private chkGrsWt As Boolean = False
    Public Property p_chkWithGrsWt() As Boolean
        Get
            Return chkGrsWt

        End Get
        Set(ByVal value As Boolean)
            chkGrsWt = value
        End Set
    End Property
End Class
