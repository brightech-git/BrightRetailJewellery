Imports System.Data.OleDb
Public Class frmCreditBalanceReport
    Dim strSql As String = ""
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Dim dt As DataTable
    Dim dtCostCentre As DataTable
    Dim dtCompany As DataTable
    Dim acType As String = Nothing
    Dim dtSummary As New DataTable()
    Dim dtoverfinal As New DataTable
    'calno 01111 sathya

    Private Sub frmCreditBalanceReport_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmCreditBalanceReport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.WindowState = FormWindowState.Maximized
        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False

        strSql = " SELECT COMPANYNAME,COMPANYID,2 RESULT FROM " & cnAdminDb & "..COMPANY where ISNULL(ACTIVE,'')<>'N'"
        strSql += " ORDER BY RESULT,COMPANYNAME"
        dtCompany = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCompany)
        BrighttechPack.GlobalMethods.FillCombo(cmbCompany, dtCompany, "COMPANYNAME", True)

        strSql = " SELECT 'ALL' ACGRPNAME,'ALL' ACGRPCODE,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT ACGRPNAME,CONVERT(vARCHAR,ACGRPCODE),2 RESULT FROM " & cnAdminDb & "..ACGROUP"
        strSql += " ORDER BY RESULT,ACGRPNAME"
        Dim dtAcGrp As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtAcGrp)
        BrighttechPack.GlobalMethods.FillCombo(cmbAcGrp, dtAcGrp, "ACGRPNAME", , "ALL")
        btnView.Focus()
        btnNew_Click(Me, New EventArgs)

        cmbAccountType.Items.Add("SMITH")
        cmbAccountType.Items.Add("CUSTOMER")
        cmbAccountType.Items.Add("DEALER")
        cmbAccountType.Items.Add("OTHERS")
        cmbAccountType.Items.Add("INTERNAL")
        cmbAccountType.Text = "SMITH"
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        If cmbCompany.Items.Count > 0 Then cmbCompany.SelectedIndex = 0
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        'dtpDate.Value = DateTime.Today.Date
        
        gridView.DataSource = Nothing
        lblTitle.Text = ""
        Prop_Gets()
        dtpFrom.Value = GetServerDate()
        'cmbCompany.Focus()
        dtpFrom.Focus()
    End Sub

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If gridView.RowCount > 0 Then
            BrightPosting.GExport.Post(Me.Name, "", gridView, lblTitle.Text, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If gridView.RowCount > 0 Then
            BrightPosting.GExport.Post(Me.Name, "", gridView, lblTitle.Text, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar.ToString().ToUpper = "P" Then
            btnPrint_Click(Me, New EventArgs)
        ElseIf e.KeyChar.ToString().ToUpper = "X" Then
            btnExport_Click(Me, New EventArgs)
        End If
    End Sub
    Private Sub Prop_Gets()
        'Dim obj As New frmduetran_Properties
        'GetSettingsObj(obj, Me.Name, GetType(frmduetran_Properties))
        'cmbCompany.Text = obj.p_cmbCompany
        'SetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre, "ALL")
    End Sub

    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView.Click
        Dim selAccName As String = Nothing
        Dim acGrpcode As String
        Dim crDays As String = Nothing
        Dim selCost As String = Nothing
        Dim Issummary As String = Nothing
        Dim acName As String = Nothing
        If cmbAcName_Own.Text <> "" And cmbAcName_Own.Text <> "ALL" Then
            Dim str As String = "SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME IN (" & GetQryString(cmbAcName_Own.Text) & ")"
            Dim dtAc As New DataTable()
            da = New OleDbDataAdapter(str, cn)
            da.Fill(dtAc)
            If dtAc.Rows.Count > 0 Then
                selAccName = dtAc.Rows(0).Item("ACCODE").ToString
            End If
        Else
            If cmbAcName_Own.Text = "" Or cmbAcName_Own.Text = "ALL" Then
                selAccName = "ALL"
                If cmbAccountType.Text <> "" Then
                    If cmbAccountType.Text = "SMITH" Then
                        acType = "G"
                    ElseIf cmbAccountType.Text = "DEALER" Then
                        acType = "D"
                    ElseIf cmbAccountType.Text = "CUSTOMER" Then
                        acType = "C"
                    ElseIf cmbAccountType.Text = "INTERNAL" Then
                        acType = "I"
                    ElseIf cmbAccountType.Text = "OTHERS" Then
                        acType = "O"
                    End If
                End If
            End If
            If cmbAcGrp.Text <> "" Or cmbAcGrp.Text <> "ALL" Then
                acGrpcode = objGPack.GetSqlValue("SELECT ACGRPCODE FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPNAME= '" & cmbAcGrp.Text & "'")
            End If
        End If

        If chkCmbCostCentre.Text = "ALL" Then
            selCost = "ALL"
        ElseIf chkCmbCostCentre.Text <> "" Then
            Dim sql As String = "SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & GetQryString(chkCmbCostCentre.Text) & ")"
            Dim dtCost As New DataTable()
            da = New OleDbDataAdapter(sql, cn)
            da.Fill(dtCost)
            If dtCost.Rows.Count > 0 Then
                For i As Integer = 0 To dtCost.Rows.Count - 1
                    selCost += dtCost.Rows(i).Item("COSTID").ToString + ","
                Next
                If selCost <> "" Then
                    selCost = Mid(selCost, 1, selCost.Length - 1)
                End If
            End If
        End If

        If rdbBillDateWise.Checked Then
            crDays = "B"
        ElseIf rdbTranDateWise.Checked Then
            crDays = "T"
        End If

        If chkSummery.Checked Then
            Issummary = "Y"
        Else
            Issummary = "N"
        End If
        strSql = vbCrLf + " EXEC " & cnStockDb & "..SP_RPT_CREDITBALANCE"
        strSql += vbCrLf + "  @ACCODE = '" & selAccName & "'"
        strSql += vbCrLf + " ,@COSTID = '" & selCost & "'"
        If chkAsonDate.Checked = True Then
            strSql += vbCrLf + " ,@FRMDATE='" & cnTranFromDate.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@ASONDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        Else
            strSql += vbCrLf + " ,@FRMDATE='" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@ASONDATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        End If

        strSql += vbCrLf + " ,@COMPANYID = '" & GetStockCompId() & "'"
        strSql += vbCrLf + " ,@CNADMINDB = '" & cnAdminDb & "'"
        strSql += vbCrLf + " ,@CNSTOCKDB = '" & cnStockDb & "'"
        strSql += vbCrLf + " ,@CRDAYS = '" & crDays & "'"
        strSql += vbCrLf + " ,@ISSUMMARY = '" & Issummary & "'"
        strSql += vbCrLf + " ,@ACTYPE = '" & acType & "'"
        strSql += vbCrLf + " ,@ACGROUP = '" & IIf(acGrpcode Is Nothing, "", acGrpcode) & "'"
        Dim DtGrid As New DataTable("SUMMARY")
        DtGrid.Columns.Add("KEYNO", GetType(Integer))
        DtGrid.Columns("KEYNO").AutoIncrement = True
        DtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        DtGrid.Columns("KEYNO").AutoIncrementStep = 1
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        If chkSummery.Checked Then
            strSql = "  SELECT *,ISNULL(RUNTOT,0) + ISNULL(DEBIT,0) - ISNULL(CREDIT,0) AS RUNNINGTOT"
            If crDays = "T" Then strSql += vbCrLf + " ,CONVERT(VARCHAR(20),DATEADD(DAY,CREDITDAYS,TRANDATE),103)DUEDATE,DATEDIFF(DAY,TRANDATE+CREDITDAYS,GETDATE())OVERDUEBY  "
            If crDays = "B" Then strSql += vbCrLf + " ,CONVERT(VARCHAR(20),DATEADD(DAY,CREDITDAYS,BILLDATE),103)DUEDATE,DATEDIFF(DAY,BILLDATE+CREDITDAYS,GETDATE())OVERDUEBY  "
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMPCREDITSUMMARY  T  "
            strSql += vbCrLf + " ORDER BY PARTY "
            da = New OleDbDataAdapter(strSql, cn)
            DtGrid.Clear()
            da.Fill(DtGrid)
            Dim Checker As String = Nothing
            Dim Party As String = Nothing
            Dim RunAMT As Decimal = 0
            Dim credit As Decimal = 0
            Dim dtledger As New DataTable
            dtledger.Clear()
            dtledger.Merge(DtGrid)

            strSql = " IF (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "TEMPCREDIT')>0 DROP TABLE TEMPTABLEDB..TEMP" & systemId & "TEMPCREDIT"
            strSql += vbCrLf + " CREATE TABLE TEMPTABLEDB..TEMP" & systemId & "TEMPCREDIT"
            strSql += vbCrLf + " (KEYNO INT,TRANNO VARCHAR(50),TRANDATE SMALLDATETIME,PARTY VARCHAR (250),DEBIT NUMERIC(20,3)"
            strSql += vbCrLf + " ,CREDIT NUMERIC (20,3),ACCODE VARCHAR (250)"
            strSql += vbCrLf + " ,OVERDAYS SMALLDATETIME,CREDITDAYS INT,RUNTOT NUMERIC (20,2),CUR VARCHAR (1)"
            strSql += vbCrLf + " ,RUNTOTAL NUMERIC (20,2),RESULT INT,BILLDATE SMALLDATETIME,RUNNINGTOT NUMERIC (20,2),COLHEAD VARCHAR(2))"

            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            For Each Ro As DataRow In DtGrid.Rows
                Dim blnCheck As Boolean = False
                If Ro.Item("PARTY").ToString <> Checker Then
                    Checker = Ro.Item("PARTY").ToString
                    RunAMT = 0 : credit = 0
                End If
                RunAMT += Val(Ro.Item("RUNTOT").ToString) + Val(Ro.Item("DEBIT").ToString) - Val(Ro.Item("CREDIT").ToString)
                credit += Val(Ro.Item("CREDIT").ToString)
                Ro.Item("RUNTOT") = IIf(RunAMT <> 0, Format(RunAMT, "0.000"), DBNull.Value)
                Ro.Item("RUNTOTAL") = IIf(credit <> 0, Format(credit, "0.000"), DBNull.Value)
                If Ro.Item("RUNTOT").ToString <> "" Then
                    If Ro.Item("RUNTOT") > 0 Then
                        Ro.Item("CUR") = "D"
                    Else
                        Ro.Item("CUR") = "C"
                    End If
                End If
                strSql = " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TEMPCREDIT (KEYNO,TRANNO,TRANDATE,PARTY,DEBIT,CREDIT,ACCODE,OVERDAYS,CREDITDAYS,RUNTOT,CUR,RUNTOTAL,RESULT,BILLDATE,RUNNINGTOT)"
                strSql += vbCrLf + " VALUES (" & Val(Ro.Item("KEYNO").ToString) & ",'" & Ro.Item("TRANNO").ToString & "','" & Ro.Item("TRANDATE").ToString & "','" & Ro.Item("PARTY").ToString & "'," & Val(Ro.Item("DEBIT").ToString) & "," & Val(Ro.Item("CREDIT").ToString) & ""
                strSql += vbCrLf + " ,'" & Ro.Item("ACCODE").ToString & "','" & Ro.Item("OVERDAYS").ToString & "'," & Val(Ro.Item("CREDITDAYS").ToString) & "," & Val(Ro.Item("RUNTOT").ToString) & ",'" & Ro.Item("CUR").ToString & "'"
                strSql += vbCrLf + " ," & Val(Ro.Item("RUNTOTAL").ToString) & "," & Val(Ro.Item("RESULT").ToString) & ",'" & Ro.Item("BILLDATE").ToString & "'," & Val(Ro.Item("RUNNINGTOT").ToString) & ")"

                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
            Next

            strSql = " IF (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ACTOTAL')>0 DROP TABLE TEMPTABLEDB..TEMP" & systemId & "ACTOTAL"
            strSql += vbCrLf + "  SELECT 3 RESULT,ACCODE,PARTY,SUM(DEBIT)DEBIT,SUM(CREDIT)CREDIT "
            strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "ACTOTAL"
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "TEMPCREDIT"
            strSql += vbCrLf + "  WHERE RESULT = 2"
            strSql += vbCrLf + "  GROUP BY ACCODE,PARTY"
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT 4 RESULT,ACCODE,'TOTAL'PARTY,SUM(DEBIT)DEBIT,SUM(CREDIT)CREDIT FROM TEMPTABLEDB..TEMP" & systemId & "TEMPCREDIT"
            strSql += vbCrLf + "  WHERE RESULT IN(1,2)"
            strSql += vbCrLf + "  GROUP BY ACCODE"
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT 5 RESULT,ACCODE,'BALANCE'PARTY"
            strSql += vbCrLf + "  ,CASE WHEN AMOUNT > 0 THEN AMOUNT ELSE null END AS DEBIT"
            strSql += vbCrLf + "  ,CASE WHEN AMOUNT < 0 THEN -1*AMOUNT ELSE null END AS CREDIT"
            strSql += vbCrLf + "  	FROM"
            strSql += vbCrLf + "  	("
            strSql += vbCrLf + "  	SELECT ACCODE,SUM(DEBIT)-SUM(CREDIT) AS AMOUNT FROM TEMPTABLEDB..TEMP" & systemId & "TEMPCREDIT"
            strSql += vbCrLf + "  	WHERE RESULT IN(1,2)"
            strSql += vbCrLf + "  	GROUP BY ACCODE"
            strSql += vbCrLf + "  )X"

            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()


            If crDays = "T" Then
                strSql = " IF (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMPCREDITTOTAL')>0 DROP TABLE TEMPTABLEDB..TEMPCREDITTOTAL"
                strSql += vbCrLf + " SELECT TRANDATE,PARTY,CREDITDAYS,CREDIT,RUNTOTAL"
                strSql += vbCrLf + " INTO TEMPTABLEDB..TEMPCREDITTOTAL"
                strSql += vbCrLf + " FROM("
                strSql += vbCrLf + " SELECT MAX(TRANDATE)TRANDATE,PARTY,(SELECT CREDIT FROM TEMPTABLEDB..TEMP" & systemId & "ACTOTAL WHERE PARTY='BALANCE' AND ACCODE=T.ACCODE)RUNTOTAL,CREDITDAYS"
                strSql += vbCrLf + " ,(SELECT CASE WHEN CUR='C' THEN SUM(ISNULL(CREDIT,0)) ELSE -1*SUM(ISNULL(CREDIT,0)) END AS CREDIT from TEMPTABLEDB..TEMP" & systemId & "TEMPCREDIT  TE WHERE TRANDATE>ISNULL(OVERDAYS,'') AND TE.ACCODE=T.ACCODE AND ISNULL(CREDIT,0)<>0 GROUP BY CUR)CREDIT"
                strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "TEMPCREDIT  T WHERE CREDITDAYS<>0 GROUP BY PARTY,ACCODE,CREDITDAYS"
                strSql += vbCrLf + " )X"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                strSql = " IF (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMPCREDITTOTAL1')>0 DROP TABLE TEMPTABLEDB..TEMPCREDITTOTAL1"
                strSql += vbCrLf + " SELECT TRANNO,TRANDATE,PARTY,CREDITDAYS,DUEAMOUNT,RUNTOTAL,OVERDUEBY"
                strSql += vbCrLf + " INTO TEMPTABLEDB..TEMPCREDITTOTAL1"
                strSql += vbCrLf + " FROM("
                strSql += vbCrLf + " SELECT TRANNO,MAX(TRANDATE)TRANDATE,PARTY,(SELECT CREDIT FROM TEMPTABLEDB..TEMP" & systemId & "ACTOTAL WHERE PARTY='BALANCE' AND ACCODE=T.ACCODE)RUNTOTAL,CREDITDAYS"
                strSql += vbCrLf + " ,(SELECT CASE WHEN CUR='C' THEN SUM(ISNULL(CREDIT,0)) ELSE -1*SUM(ISNULL(CREDIT,0)) END AS CREDIT from TEMPTABLEDB..TEMP" & systemId & "TEMPCREDIT  TE WHERE TRANDATE>ISNULL(OVERDAYS,'') AND TE.ACCODE=T.ACCODE GROUP BY CUR)CREDIT"
                If crDays = "T" Then strSql += vbCrLf + " ,CONVERT(VARCHAR(20),DATEADD(DAY,CREDITDAYS,TRANDATE),103)DUEDATE,DATEDIFF(DAY,TRANDATE+CREDITDAYS,GETDATE())OVERDUEBY  "
                If crDays = "B" Then strSql += vbCrLf + " ,CONVERT(VARCHAR(20),DATEADD(DAY,CREDITDAYS,BILLDATE),103)DUEDATE,DATEDIFF(DAY,BILLDATE+CREDITDAYS,GETDATE())OVERDUEBY  "
                strSql += vbCrLf + ",CREDIT AS DUEAMOUNT"
                strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "TEMPCREDIT  T WHERE CREDITDAYS<>0 "
                strSql += vbCrLf + " AND TRANDATE<ISNULL(OVERDAYS,'')"
                strSql += vbCrLf + " GROUP BY PARTY,ACCODE,CREDITDAYS,CREDIT,TRANDATE,TRANNO"
                strSql += vbCrLf + " )X"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
                '01111
                strSql = "SELECT * FROM TEMPTABLEDB..TEMPCREDITTOTAL1 ORDER BY TRANDATE DESC"
                da = New OleDbDataAdapter(strSql, cn)
                Dim dt As New DataTable()
                da.Fill(dt)
                dtoverfinal = New DataTable
                dtoverfinal.Columns.Add("PARTY", Type.GetType("System.String"))
                dtoverfinal.Columns.Add("OVERDUEBY", Type.GetType("System.Decimal"))
                dtoverfinal.AcceptChanges()
                Dim dtparty As New DataTable
                dtparty = dt.DefaultView.ToTable(True, "PARTY")
                Dim overdueBy As Decimal = 0
                Dim billamt As Decimal = 0
                Dim accName As String = Nothing
                Dim trandate As String
                Dim oveCheck, partyCheck As Boolean
                Dim i As Integer = 0
                dt.Columns.Add("CHECK", GetType(Boolean))
                For j As Integer = 0 To dtparty.Rows.Count - 1
                    overdueBy = 0 : billamt = 0
                    accName = dtparty.Rows(j).Item("PARTY").ToString
                    Dim dtro As DataRow()
                    dtro = dt.Select("PARTY='" & accName & "'")
                    For k As Integer = 0 To dtro.Length - 1
                        oveCheck = False
                        Dim blnCheck As Boolean = False
                        With dtro(k)
                            If .Item("TRANDATE").ToString <> trandate Then
                                trandate = .Item("TRANDATE").ToString
                                oveCheck = True
                            End If
                            billamt += .Item("DUEAMOUNT")
                            If .Item("RUNTOTAL").ToString <> "" Then
                                If .Item("RUNTOTAL") > billamt Then
                                    .Item("CHECK") = True
                                    If oveCheck = True Then overdueBy += Val(.Item("OVERDUEBY"))
                                Else
                                    .Item("CHECK") = True
                                    Exit For
                                End If
                            End If
                        End With
                    Next
                    Dim ro As DataRow
                    ro = dtoverfinal.NewRow
                    ro("PARTY") = accName
                    ro("OVERDUEBY") = overdueBy
                    dtoverfinal.Rows.Add(ro)
                Next
            End If

            If crDays = "B" Then
                strSql = " IF (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMPCREDITTOTAL')>0 DROP TABLE TEMPTABLEDB..TEMPCREDITTOTAL"
                strSql += vbCrLf + " SELECT BILLDATE,PARTY,CREDITDAYS,CREDIT,RUNTOTAL"
                strSql += vbCrLf + " INTO TEMPTABLEDB..TEMPCREDITTOTAL"
                strSql += vbCrLf + " FROM("
                strSql += vbCrLf + " SELECT MAX(BILLDATE)BILLDATE,PARTY,(SELECT CREDIT FROM TEMPTABLEDB..TEMP" & systemId & "ACTOTAL WHERE PARTY='BALANCE' AND ACCODE=T.ACCODE)RUNTOTAL,CREDITDAYS"
                strSql += vbCrLf + " ,(SELECT CASE WHEN CUR='C' THEN SUM(ISNULL(CREDIT,0)) ELSE -1*SUM(ISNULL(CREDIT,0)) END AS CREDIT from TEMPTABLEDB..TEMP" & systemId & "TEMPCREDIT  TE WHERE BILLDATE>ISNULL(OVERDAYS,'') AND TE.ACCODE=T.ACCODE GROUP BY CUR)CREDIT"
                strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "TEMPCREDIT  T WHERE CREDITDAYS<>0 GROUP BY PARTY,ACCODE,CREDITDAYS"
                strSql += vbCrLf + " )X"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                strSql = " IF (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMPCREDITTOTAL1')>0 DROP TABLE TEMPTABLEDB..TEMPCREDITTOTAL1"
                strSql += vbCrLf + " SELECT TRANNO,BILLDATE,PARTY,CREDITDAYS,DUEAMOUNT,RUNTOTAL,OVERDUEBY"
                strSql += vbCrLf + " INTO TEMPTABLEDB..TEMPCREDITTOTAL1"
                strSql += vbCrLf + " FROM("
                strSql += vbCrLf + " SELECT TRANNO,MAX(BILLDATE)BILLDATE,PARTY,(SELECT CREDIT FROM TEMPTABLEDB..TEMP" & systemId & "ACTOTAL WHERE PARTY='BALANCE' AND ACCODE=T.ACCODE)RUNTOTAL,CREDITDAYS"
                strSql += vbCrLf + " ,(SELECT CASE WHEN CUR='C' THEN SUM(ISNULL(CREDIT,0)) ELSE -1*SUM(ISNULL(CREDIT,0)) END AS CREDIT from TEMPTABLEDB..TEMP" & systemId & "TEMPCREDIT  TE WHERE BILLDATE>ISNULL(OVERDAYS,'') AND TE.ACCODE=T.ACCODE GROUP BY CUR)CREDIT"
                If crDays = "T" Then strSql += vbCrLf + " ,CONVERT(VARCHAR(20),DATEADD(DAY,CREDITDAYS,TRANDATE),103)DUEDATE,DATEDIFF(DAY,TRANDATE+CREDITDAYS,GETDATE())OVERDUEBY  "
                If crDays = "B" Then strSql += vbCrLf + " ,CONVERT(VARCHAR(20),DATEADD(DAY,CREDITDAYS,BILLDATE),103)DUEDATE,DATEDIFF(DAY,BILLDATE+CREDITDAYS,GETDATE())OVERDUEBY  "
                strSql += vbCrLf + ",CREDIT AS DUEAMOUNT"
                strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "TEMPCREDIT  T WHERE CREDITDAYS<>0 "
                strSql += vbCrLf + " AND TRANDATE<ISNULL(OVERDAYS,'')"
                strSql += vbCrLf + " GROUP BY PARTY,ACCODE,CREDITDAYS,CREDIT,BILLDATE,TRANNO"
                strSql += vbCrLf + " )X"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                strSql = "SELECT * FROM TEMPTABLEDB..TEMPCREDITTOTAL1 ORDER BY BILLDATE DESC"
                da = New OleDbDataAdapter(strSql, cn)
                Dim dt As New DataTable()
                da.Fill(dt)
                dtoverfinal = New DataTable
                dtoverfinal.Columns.Add("PARTY", Type.GetType("System.String"))
                dtoverfinal.Columns.Add("OVERDUEBY", Type.GetType("System.Decimal"))
                dtoverfinal.AcceptChanges()
                Dim dtparty As New DataTable
                dtparty = dt.DefaultView.ToTable(True, "PARTY")
                Dim overdueBy As Decimal = 0
                Dim billamt As Decimal = 0
                Dim accName As String = Nothing
                Dim trandate As String
                Dim oveCheck, partyCheck As Boolean
                Dim i As Integer = 0
                dt.Columns.Add("CHECK", GetType(Boolean))
                For j As Integer = 0 To dtparty.Rows.Count - 1
                    overdueBy = 0 : billamt = 0
                    accName = dtparty.Rows(j).Item("PARTY").ToString
                    Dim dtro As DataRow()
                    dtro = dt.Select("PARTY='" & accName & "'")
                    For k As Integer = 0 To dtro.Length - 1
                        oveCheck = False
                        Dim blnCheck As Boolean = False
                        With dtro(k)
                            If .Item("BILLDATE").ToString <> trandate Then
                                trandate = .Item("BILLDATE").ToString
                                oveCheck = True
                            End If
                            billamt += .Item("DUEAMOUNT")
                            If .Item("RUNTOTAL").ToString <> "" Then
                                If .Item("RUNTOTAL") > billamt Then
                                    .Item("CHECK") = True
                                    If oveCheck = True Then overdueBy += Val(.Item("OVERDUEBY"))
                                Else
                                    .Item("CHECK") = True
                                    Exit For
                                End If
                            End If
                        End With
                    Next
                    Dim ro As DataRow
                    ro = dtoverfinal.NewRow
                    ro("PARTY") = accName
                    ro("OVERDUEBY") = overdueBy
                    dtoverfinal.Rows.Add(ro)
                Next
            End If
            '01111

            strSql = " IF (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMPCREDITBALANCE')>0 DROP TABLE TEMPTABLEDB..TEMPCREDITBALANCE"
            strSql += vbCrLf + " SELECT "
            If crDays = "T" Then strSql += vbCrLf + "  TRANDATE"
            If crDays = "B" Then strSql += vbCrLf + " BILLDATE"
            strSql += vbCrLf + " ,PARTY,CASE WHEN RUNTOTAL>0 THEN ISNULL(RUNTOTAL,0)-ISNULL(CREDIT,0) END  AS OVERDUE,CREDITDAYS,RUNTOTAL  "
            strSql += vbCrLf + " INTO TEMPTABLEDB..TEMPCREDITBALANCE "
            strSql += vbCrLf + " FROM("
            strSql += vbCrLf + " SELECT * FROM TEMPTABLEDB..TEMPCREDITTOTAL"
            strSql += vbCrLf + " )X"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            'here after we have to update overdays
            strSql = " SELECT "
            If crDays = "T" Then strSql += vbCrLf + " CONVERT(VARCHAR(20),TRANDATE,103)TRANDATE"
            If crDays = "B" Then strSql += vbCrLf + " CONVERT(VARCHAR(20),BILLDATE,103)BILLDATE"
            strSql += vbCrLf + " ,PARTY,CREDITDAYS,OVERDUE"
            If crDays = "T" Then strSql += vbCrLf + " ,DATEDIFF(DAY,TRANDATE+CREDITDAYS,GETDATE()) as OVERDUEDAYS "
            If crDays = "B" Then strSql += vbCrLf + " ,DATEDIFF(DAY,BILLDATE+CREDITDAYS,GETDATE()) as OVERDUEDAYS "
            strSql += vbCrLf + " ,RUNTOTAL"
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMPCREDITBALANCE"
            da = New OleDbDataAdapter(strSql, cn)
            dtSummary = New DataTable()
            da.Fill(dtSummary)
            If dtSummary.Rows.Count > 0 Then
                For Each ro As DataRow In dtSummary.Rows
                    Dim pro As DataRow() = dtoverfinal.Select("PARTY='" & ro.Item("PARTY").ToString & "'")
                    If pro.Length > 0 Then
                        ro.Item("OVERDUEDAYS") = Val(pro(0).Item("OVERDUEBY").ToString)
                    Else
                        ro.Item("OVERDUEDAYS") = 0
                    End If

                Next
            End If
            Dim dv As DataView
            dv = dtSummary.DefaultView
            dv.RowFilter = "OVERDUE >0 AND OVERDUEDAYS >0"
            dtSummary = dv.ToTable
            If crDays = "T" Then dtSummary.Columns("TRANDATE").SetOrdinal(0)
            If crDays = "B" Then dtSummary.Columns("BILLDATE").SetOrdinal(0)
            dtSummary.Columns("PARTY").SetOrdinal(1)
            dtSummary.Columns("CREDITDAYS").SetOrdinal(2)
            dtSummary.Columns("OVERDUE").SetOrdinal(3)
            dtSummary.Columns("OVERDUEDAYS").SetOrdinal(4)
            gridView.DataSource = dtSummary
        End If
        If chkSummery.Checked = False Then
            strSql = " IF (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMPCREDITOVERDUE')>0 DROP TABLE TEMPTABLEDB..TEMPCREDITOVERDUE"
            strSql += vbCrLf + " SELECT BILLNO"
            If crDays = "T" Then strSql += vbCrLf + " ,TRANDATE"
            If crDays = "B" Then strSql += vbCrLf + " ,BILLDATE"
            strSql += vbCrLf + " ,PARTY,CREDITDAYS,DUEAMOUNT,TOTALBALANCE,BILLAMOUNT,DUEDATE,OVERDUEBY"
            strSql += vbCrLf + " INTO TEMPTABLEDB..TEMPCREDITOVERDUE"
            strSql += vbCrLf + " FROM("
            strSql += vbCrLf + " SELECT TRANNO AS BILLNO "
            If crDays = "T" Then strSql += vbCrLf + " ,TRANDATE"
            If crDays = "B" Then strSql += vbCrLf + " ,BILLDATE"
            strSql += vbCrLf + " ,PARTY,CREDITDAYS,(CREDIT)DUEAMOUNT"
            strSql += vbCrLf + " ,(SELECT SUM(CREDIT)-SUM(DEBIT) FROM TEMPTABLEDB..TEMPCREDITSUMMARY TC WHERE TC.PARTY=T.PARTY)TOTALBALANCE"
            If crDays = "T" Then strSql += vbCrLf + " ,((SELECT SUM(CREDIT)-SUM(DEBIT) FROM TEMPTABLEDB..TEMPCREDITSUMMARY TC WHERE TC.PARTY=T.PARTY)-(SELECT SUM(CREDIT) FROM  TEMPTABLEDB..TEMPCREDITSUMMARY TP WHERE TP.PARTY=T.PARTY AND TRANDATE>ISNULL(OVERDAYS,'')))BILLAMOUNT"
            If crDays = "B" Then strSql += vbCrLf + " ,((SELECT SUM(CREDIT)-SUM(DEBIT) FROM TEMPTABLEDB..TEMPCREDITSUMMARY TC WHERE TC.PARTY=T.PARTY)-(SELECT SUM(CREDIT) FROM  TEMPTABLEDB..TEMPCREDITSUMMARY TP WHERE TP.PARTY=T.PARTY AND BILLDATE>ISNULL(OVERDAYS,'')))BILLAMOUNT"
            If crDays = "T" Then strSql += vbCrLf + " ,CONVERT(VARCHAR(20),DATEADD(DAY,CREDITDAYS,TRANDATE),103)DUEDATE,DATEDIFF(DAY,TRANDATE+CREDITDAYS,GETDATE())OVERDUEBY  "
            If crDays = "B" Then strSql += vbCrLf + " ,CONVERT(VARCHAR(20),DATEADD(DAY,CREDITDAYS,BILLDATE),103)DUEDATE,DATEDIFF(DAY,BILLDATE+CREDITDAYS,GETDATE())OVERDUEBY  "
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMPCREDITSUMMARY T"
            strSql += vbCrLf + " WHERE(CREDIT <> 0)"
            strSql += vbCrLf + " AND "
            If crDays = "T" Then strSql += vbCrLf + " TRANDATE>ISNULL(OVERDAYS,'')"
            If crDays = "B" Then strSql += vbCrLf + " BILLDATE>ISNULL(OVERDAYS,'')"
            strSql += vbCrLf + " )X"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = "SELECT BILLNO," & IIf(crDays = "T", "TRANDATE", "BILLDATE") & "," & IIf(crDays = "T", "CONVERT(VARCHAR(20),TRANDATE,103) AS TRANDATE1", "CONVERT(VARCHAR(20),BILLDATE,103) AS BILLDATE1") & ",PARTY,CREDITDAYS,DUEAMOUNT,TOTALBALANCE,BILLAMOUNT,DUEDATE,OVERDUEBY FROM TEMPTABLEDB..TEMPCREDITOVERDUE"
            da = New OleDbDataAdapter(strSql, cn)
            Dim dtOverDue As New DataTable
            da.Fill(dtOverDue)

            strSql = " IF (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMPCREDITDETAILS')>0 DROP TABLE TEMPTABLEDB..TEMPCREDITDETAILS"
            strSql += vbCrLf + "  SELECT BILLNO"
            If crDays = "T" Then strSql += vbCrLf + " ,TRANDATE"
            If crDays = "B" Then strSql += vbCrLf + " ,BILLDATE"
            strSql += vbCrLf + " ,PARTY,CREDITDAYS,DUEAMOUNT,TOTALBALANCE,BILLAMOUNT,DUEDATE,OVERDUEBY"
            strSql += vbCrLf + "  INTO TEMPTABLEDB..TEMPCREDITDETAILS"
            strSql += vbCrLf + "  FROM ("
            strSql += vbCrLf + "  SELECT TRANNO AS BILLNO "
            If crDays = "T" Then strSql += vbCrLf + " ,TRANDATE"
            If crDays = "B" Then strSql += vbCrLf + " ,BILLDATE"
            strSql += vbCrLf + " ,PARTY,CREDITDAYS,(CREDIT)DUEAMOUNT"
            strSql += vbCrLf + "  ,(SELECT SUM(CREDIT)-SUM(DEBIT) FROM TEMPTABLEDB..TEMPCREDITSUMMARY TC WHERE TC.PARTY=T.PARTY)TOTALBALANCE"
            If crDays = "T" Then strSql += vbCrLf + " ,((SELECT SUM(CREDIT)-SUM(DEBIT) FROM TEMPTABLEDB..TEMPCREDITSUMMARY TC WHERE TC.PARTY=T.PARTY)-(SELECT SUM(CREDIT) FROM  TEMPTABLEDB..TEMPCREDITSUMMARY TP WHERE TP.PARTY=T.PARTY AND TRANDATE>ISNULL(OVERDAYS,'')))BILLAMOUNT"
            If crDays = "B" Then strSql += vbCrLf + " ,((SELECT SUM(CREDIT)-SUM(DEBIT) FROM TEMPTABLEDB..TEMPCREDITSUMMARY TC WHERE TC.PARTY=T.PARTY)-(SELECT SUM(CREDIT) FROM  TEMPTABLEDB..TEMPCREDITSUMMARY TP WHERE TP.PARTY=T.PARTY AND BILLDATE>ISNULL(OVERDAYS,'')))BILLAMOUNT"
            If crDays = "T" Then strSql += vbCrLf + " ,CONVERT(VARCHAR(20),DATEADD(DAY,CREDITDAYS,TRANDATE),103)DUEDATE,DATEDIFF(DAY,TRANDATE+CREDITDAYS,GETDATE())OVERDUEBY  "
            If crDays = "B" Then strSql += vbCrLf + " ,CONVERT(VARCHAR(20),DATEADD(DAY,CREDITDAYS,BILLDATE),103)DUEDATE,DATEDIFF(DAY,BILLDATE+CREDITDAYS,GETDATE())OVERDUEBY  "
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMPCREDITSUMMARY T"
            strSql += vbCrLf + "  WHERE(CREDIT <> 0)"
            strSql += vbCrLf + "  AND "
            If crDays = "T" Then strSql += vbCrLf + " TRANDATE<ISNULL(OVERDAYS,'')"
            If crDays = "B" Then strSql += vbCrLf + " BILLDATE<ISNULL(OVERDAYS,'')"
            strSql += vbCrLf + "  )X"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = " SELECT BILLNO," & IIf(crDays = "T", "TRANDATE", "BILLDATE") & "," & IIf(crDays = "T", "CONVERT(VARCHAR(20),TRANDATE,103) AS TRANDATE1", "CONVERT(VARCHAR(20),BILLDATE,103) AS BILLDATE1") & ",PARTY,CREDITDAYS,DUEAMOUNT,TOTALBALANCE,BILLAMOUNT,DUEDATE,OVERDUEBY FROM TEMPTABLEDB..TEMPCREDITDETAILS " & IIf(crDays = "B", "ORDER BY MONTH(BILLDATE)DESC,BILLDATE DESC", "ORDER BY MONTH(TRANDATE)DESC,TRANDATE DESC") & ""
            da = New OleDbDataAdapter(strSql, cn)
            DtGrid.Clear()
            DtGrid = New DataTable()
            da.Fill(DtGrid)
            If dtOverDue.Rows.Count > 0 Then
                DtGrid.Merge(dtOverDue)
            End If
            Dim dview1 As DataView
            dview1 = DtGrid.DefaultView()
            dview1.Sort = IIf(crDays = "B", "BILLDATE DESC,DUEAMOUNT DESC", "TRANDATE DESC,DUEAMOUNT DESC")
            DtGrid = dview1.ToTable
            DtGrid.Columns.Add("CHECK", GetType(Boolean))
            Dim billamount As Decimal = 0
            Dim dueamt As Decimal = 0
            For Each Ro As DataRow In DtGrid.Rows
                billamount += Ro.Item("DUEAMOUNT")
                If Ro.Item("TOTALBALANCE") > billamount Then
                    Ro.Item("CHECK") = True
                    dueamt += Ro.Item("DUEAMOUNT")
                    'Ro.Item("DUEAMOUNT") = Val(Ro.Item("BILLAMOUNT")) - billamount

                Else
                    Ro.Item("CHECK") = True
                    Ro.Item("DUEAMOUNT") = Math.Abs(Val(Ro.Item("TOTALBALANCE")) - dueamt)
                    Exit For
                End If
            Next
            Dim dview As DataView
            dview = DtGrid.DefaultView()
            dview.RowFilter = "CHECK=TRUE"
            dview.Sort = IIf(crDays = "B", "BILLDATE ASC,DUEAMOUNT ASC", "TRANDATE ASC,DUEAMOUNT ASC")
            DtGrid = dview.ToTable


            DtGrid.Columns("BILLNO").SetOrdinal(0)
            If crDays = "T" Then DtGrid.Columns("TRANDATE1").SetOrdinal(1)
            If crDays = "B" Then DtGrid.Columns("BILLDATE1").SetOrdinal(1)
            DtGrid.Columns("PARTY").SetOrdinal(2)
            DtGrid.Columns("DUEAMOUNT").SetOrdinal(3)
            DtGrid.Columns("CREDITDAYS").SetOrdinal(4)
            DtGrid.Columns("DUEDATE").SetOrdinal(5)
            DtGrid.Columns("OVERDUEBY").SetOrdinal(6)
            Dim dv As DataView
            dv = DtGrid.DefaultView
            dv.RowFilter = "DUEAMOUNT >0"
            DtGrid = dv.ToTable
            gridView.DataSource = Nothing
            gridView.DataSource = DtGrid
            gridView.Columns("TOTALBALANCE").Visible = False
            gridView.Columns("BILLAMOUNT").Visible = False
            gridView.Columns("CHECK").Visible = False
            If crDays = "T" Then
                gridView.Columns("TRANDATE").Visible = False
                gridView.Columns("TRANDATE1").Visible = True
                gridView.Columns("TRANDATE1").HeaderText = "TRANDATE"
            ElseIf crDays = "B" Then
                gridView.Columns("BILLDATE").Visible = False
                gridView.Columns("BILLDATE1").Visible = True
                gridView.Columns("BILLDATE1").HeaderText = "BILLDATE"
            End If
            gridView.Columns("PARTY").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            gridView.Columns("CREDITDAYS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            gridView.Columns("DUEAMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            gridView.Columns("OVERDUEBY").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            gridView.Columns("PARTY").Width = 300
            'gridView.Columns("BILLNO").Width = 100
            'gridView.Columns("CREDITDAYS").Width = 100
            gridView.Focus()
        Else
            If crDays = "T" Then
                gridView.Columns("TRANDATE").Visible = True
            ElseIf crDays = "B" Then
                gridView.Columns("BILLDATE").Visible = True
            End If

            gridView.Columns("PARTY").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            gridView.Columns("OVERDUE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            gridView.Columns("CREDITDAYS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            gridView.Columns("OVERDUEDAYS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            gridView.Columns("PARTY").Width = 500
            gridView.Columns("OVERDUE").Width = 200
            gridView.Columns("CREDITDAYS").Width = 100
            gridView.Columns("RUNTOTAL").Visible = False
            gridView.Focus()
            strSql = " IF (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "TEMPCREDIT')>0 DROP TABLE TEMPTABLEDB..TEMP" & systemId & "TEMPCREDIT"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            strSql = " IF (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ACTOTAL')>0 DROP TABLE TEMPTABLEDB..TEMP" & systemId & "ACTOTAL"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            strSql = " IF (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMPCREDITTOTAL')>0 DROP TABLE TEMPTABLEDB..TEMPCREDITTOTAL"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            strSql = " IF (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMPCREDITBALANCE')>0 DROP TABLE TEMPTABLEDB..TEMPCREDITBALANCE"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End If
    End Sub

    Private Sub cmbAcGrp_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAcGrp.TextChanged
        
    End Sub

    Private Sub cmbAccountType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAccountType.SelectedIndexChanged
        If cmbAccountType.Text <> "" Then
            If cmbAccountType.Text = "SMITH" Then
                acType = "G"
            ElseIf cmbAccountType.Text = "DEALER" Then
                acType = "D"
            ElseIf cmbAccountType.Text = "CUSTOMER" Then
                acType = "C"
            ElseIf cmbAccountType.Text = "INTERNAL" Then
                acType = "I"
            ElseIf cmbAccountType.Text = "OTHERS" Then
                acType = "O"
            End If
        End If
        strSql = " SELECT 'ALL' ACNAME,'ALL' ACCODE,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT ACNAME,CONVERT(vARCHAR,ACCODE),2 RESULT FROM " & cnAdminDb & "..ACHEAD "
        If cmbAcGrp.Text <> "ALL" And cmbAcGrp.Text <> "" And cmbAccountType.Text <> "" Then
            strSql += " WHERE ACGRPCODE IN (SELECT ACGRPCODE FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPNAME IN (" & GetQryString(cmbAcGrp.Text) & "))"
            strSql += " AND ACTYPE ='" & acType & "'"
        ElseIf cmbAcGrp.Text <> "ALL" And cmbAcGrp.Text <> "" Then
            strSql += " WHERE ACGRPCODE IN (SELECT ACGRPCODE FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPNAME IN (" & GetQryString(cmbAcGrp.Text) & "))"
        ElseIf cmbAccountType.Text <> "" And cmbAcGrp.Text = "ALL" Then
            strSql += " WHERE ACTYPE ='" & acType & "'"

        End If
        strSql += " ORDER BY RESULT,ACNAME"
        objGPack.FillCombo(strSql, cmbAcName_Own, , False)
    End Sub

    Private Sub chkDetailed_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkDetailed.CheckedChanged
        If chkDetailed.Checked Then
            chkSummery.Checked = False
        End If
    End Sub

    Private Sub chkSummery_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSummery.CheckedChanged
        If chkSummery.Checked Then
            chkDetailed.Checked = False
        End If
    End Sub

    Private Sub gridView_DataError(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles gridView.DataError

    End Sub

    Private Sub chkAsonDate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAsonDate.CheckedChanged
        If chkAsonDate.Checked = True Then
            chkAsonDate.Text = "&As OnDate"
            lblTo.Enabled = False
            dtpTo.Enabled = False
        Else
            chkAsonDate.Text = "&Date From"
            lblTo.Enabled = True
            dtpTo.Enabled = True
        End If
    End Sub
End Class
Public Class frmCreditBalanceReport_Properties
    Private cmbCompany As String = "ALL"
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