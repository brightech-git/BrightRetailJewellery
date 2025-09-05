Imports System.Data.OleDb
Public Class frmCustomerOutstandingMonth
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Dim strSql As String
    Dim dtt As DataTable
    Function funcNew()
        ''Checking CostCentre Status
        strSql = " SELECT 1 FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLTEXT = 'Y' AND CTLID = 'COSTCENTRE'"
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            cmbCostCenter.Enabled = True
            strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            objGPack.FillCombo(strSql, cmbCostCenter, True)
            cmbCostCenter.Items.Add("ALL")
            cmbCostCenter.SelectedIndex = 0
            cmbCostCenter.Text = "ALL"
        Else
            cmbCostCenter.Enabled = False
        End If
        strSql = " SELECT COMPANYNAME FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N'"
        strSql += " ORDER BY COMPANYNAME"
        dt = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dt, "COMPANYNAME", , strCompanyName)
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        lblTitle.Text = ""
        gridView.DataSource = Nothing
        chkCmbCompany.Select()
    End Function
    Private Sub funcTempRpt()
        Try
            Dim sysid As String
            sysid = Trim(LSet(Mid(Guid.NewGuid.ToString, 1, InStr(Guid.NewGuid.ToString, "-") - 1), 10))
            gridView.DataSource = Nothing
            gridView.Refresh()
            AutoResizeToolStripMenuItem.Checked = False
            Dim _compId As String
            _compId = GetQryStringForSp(chkCmbCompany.Text, cnAdminDb & "..COMPANY", "COMPANYID", "COMPANYNAME", False)
            sysid = Mid(sysid, 1, 3)

            strSql = vbCrLf + " IF  EXISTS (SELECT * FROM TEMPTABLEDB..sysobjects WHERE xtype = 'U' AND NAME = 'TEMP" & sysid & "ADVCR' )DROP TABLE TEMPTABLEDB..TEMP" & sysid & "ADVCR"
            cmd = New OleDbCommand(strSql, cn)
            cmd.CommandTimeout = 0
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + " CREATE TABLE TEMPTABLEDB..TEMP" & sysid & "ADVCR ([COMPANYID] [VARCHAR](5),[COSTID] [VARCHAR](5),TRANMODE VARCHAR(2),"
            strSql += vbCrLf + " TRANDATE DATETIME,TRANNO VARCHAR(20),[PARTICULARS] [VARCHAR](500),[OPENING] DECIMAL(12,2) DEFAULT 0,[ACCODE] VARCHAR(10),"
            strSql += vbCrLf + " [RUNNO] VARCHAR(20),[BATCHNO] VARCHAR(20),[DEBIT] DECIMAL(12,2) DEFAULT 0,[CREDIT] DECIMAL(12,2) DEFAULT 0,"
            strSql += vbCrLf + " [BALANCE] DECIMAL(12,2) DEFAULT 0,[RESULT] VARCHAR(2),[PAYMODE] VARCHAR(20),[REMARK] VARCHAR(150),"
            strSql += vbCrLf + " LASTTRANDATE VARCHAR(13)) ON [PRIMARY]"
            cmd = New OleDbCommand(strSql, cn)
            cmd.CommandTimeout = 0
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & sysid & "ADVCR(COMPANYID,COSTID,TRANDATE,TRANNO,TRANMODE,BATCHNO,RUNNO,ACCODE,"
            strSql += vbCrLf + " PARTICULARS,DEBIT,CREDIT,BALANCE,LASTTRANDATE) SELECT COMPANYID,COSTID,TRANDATE,TRANNO,'02',BATCHNO, RUNNO,ACCODE,"
            strSql += vbCrLf + " '',CASE WHEN RECPAY = 'P' THEN ISNULL(AMOUNT,0) END DEBIT,CASE WHEN RECPAY = 'R' THEN ISNULL(AMOUNT,0) END CREDIT,"
            strSql += vbCrLf + " CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE -1*AMOUNT END , TRANDATE FROM  " & cnAdminDb & "..OUTSTANDING "
            'strSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "' "
            strSql += vbCrLf + " WHERE TRANDATE <= '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "' "
            strSql += vbCrLf + " AND TRANTYPE IN('A') AND ISNULL(CANCEL,'') <> 'Y' AND ACCODE LIKE 'ADV%' "
            If _compId <> "" Then strSql += vbCrLf + " AND COMPANYID IN ('" & _compId & "') "
            cmd = New OleDbCommand(strSql, cn)
            cmd.CommandTimeout = 0
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + " UPDATE A SET A.DEBIT =DEBIT-CREDIT,CREDIT=0  FROM TEMPTABLEDB..TEMP" & sysid & "ADVCR A WHERE TRANMODE = '0' AND DEBIT-CREDIT > 0 "
            cmd = New OleDbCommand(strSql, cn)
            cmd.CommandTimeout = 0
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + " UPDATE A SET A.DEBIT =0,CREDIT=CREDIT-DEBIT  FROM TEMPTABLEDB..TEMP" & sysid & "ADVCR A WHERE TRANMODE = '01' AND DEBIT-CREDIT <= 0 "
            cmd = New OleDbCommand(strSql, cn)
            cmd.CommandTimeout = 0
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + " IF  EXISTS (SELECT * FROM sysobjects WHERE xtype = 'U' AND NAME = 'TEMP" & sysid & "OP' )DROP TABLE TEMP" & sysid & "OP "
            strSql += vbCrLf + " (SELECT SUM(CASE WHEN RECPAY ='R' THEN AMOUNT ELSE -1*AMOUNT END)OPBAL,ACCODE INTO TEMP" & sysid & "OP "
            strSql += vbCrLf + " FROM  " & cnAdminDb & "..OUTSTANDING WHERE TRANDATE < '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') <> 'Y'AND TRANTYPE IN('A') "
            If _compId <> "" Then strSql += vbCrLf + " AND COMPANYID IN ('" & _compId & "') "
            strSql += vbCrLf + " AND ACCODE LIKE 'ADV%' GROUP BY ACCODE ) "
            cmd = New OleDbCommand(strSql, cn)
            cmd.CommandTimeout = 0
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + " IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & sysid & "ADVCRCRADV')>0  DROP TABLE TEMPTABLEDB..TEMP" & sysid & "ADVCRCRADV"
            strSql += vbCrLf + "  CREATE TABLE TEMPTABLEDB ..TEMP" & sysid & "ADVCRCRADV "
            strSql += vbCrLf + "  ([ACCODE] VARCHAR(20)NULL,[RUNNO] VARCHAR(2000)NULL,PARTICULARS VARCHAR(500)NULL,[OPENING] DECIMAL(15,2)DEFAULT 0,[DEBIT] DECIMAL(15,2)DEFAULT 0,"
            strSql += vbCrLf + "  [CREDIT] DECIMAL(15,2)DEFAULT 0,[BALANCE] DECIMAL(15,2)DEFAULT 0,RESULT INT NULL,LASTTRANDATE SMALLDATETIME)"
            cmd = New OleDbCommand(strSql, cn)
            cmd.CommandTimeout = 0
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + " INSERT INTO TEMPTABLEDB ..TEMP" & sysid & "ADVCRCRADV (ACCODE,RUNNO,PARTICULARS,RESULT )   "
            strSql += vbCrLf + " SELECT DISTINCT ACCODE,(SELECT TOP 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=A.ACCODE),"
            strSql += vbCrLf + " (SELECT TOP 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=A.ACCODE),0 FROM TEMPTABLEDB..TEMP" & sysid & "ADVCR A  "
            cmd = New OleDbCommand(strSql, cn)
            cmd.CommandTimeout = 0
            cmd.ExecuteNonQuery()

            'strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & sysid & "ADVCRCRADV(ACCODE,RUNNO,PARTICULARS,BALANCE,RESULT )   "
            'strSql += vbCrLf + " SELECT DISTINCT ACCODE,(SELECT TOP 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=A.ACCODE),"
            'strSql += vbCrLf + " (SELECT TOP 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=A.ACCODE),OPBAL,1 FROM TEMP" & sysid & "OP A  "
            'cmd = New OleDbCommand(strSql, cn)
            'cmd.CommandTimeout = 0
            'cmd.ExecuteNonQuery()

            strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & sysid & "ADVCRCRADV(ACCODE,RUNNO,PARTICULARS,OPENING ,DEBIT,CREDIT,BALANCE,RESULT,LASTTRANDATE)   "
            strSql += vbCrLf + " SELECT A.ACCODE,RUNNO,ISNULL(B.ACNAME,'') AS PARTICULARS ,OPENING,SUM(DEBIT)DEBIT,SUM(CREDIT)CREDIT,"
            strSql += vbCrLf + " SUM(ISNULL(CREDIT,0))-SUM(ISNULL(DEBIT,0)) BALANCE,2,MAX(A.TRANDATE)"
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & sysid & "ADVCR A  "
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CUSTOMERINFO AS CINF ON A.BATCHNO = CINF.BATCHNO  "
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..PERSONALINFO AS PINF ON PINF.SNO = CINF.PSNO "
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ACHEAD B ON   A.ACCODE = B.ACCODE "
            strSql += vbCrLf + " WHERE A.ACCODE <>''AND A.PARTICULARS<>'OPENING' AND ISNULL(CANCEL,'') <> 'Y' AND B.ACCODE LIKE 'ADV%' "
            strSql += vbCrLf + " AND B.ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD  WHERE ACCODE LIKE 'ADV%')"
            strSql += vbCrLf + " GROUP BY A.ACCODE,RUNNO,B.ACNAME,A.OPENING HAVING (SUM(ISNULL(DEBIT,0))-SUM(ISNULL(CREDIT,0))) <> 0"
            cmd = New OleDbCommand(strSql, cn)
            cmd.CommandTimeout = 0
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & sysid & "ADVCRCRADV(ACCODE,PARTICULARS,RUNNO,BALANCE,RESULT )   "
            strSql += vbCrLf + " SELECT ACCODE,'SUB TOTAL' AS RUNNO,PARTICULARS,SUM(BALANCE)BALANCE,3 AS RESULT"
            strSql += vbCrLf + " FROM TEMPTABLEDB ..TEMP" & sysid & "ADVCRCRADV WHERE RESULT IN (1,2)"
            strSql += vbCrLf + " GROUP BY ACCODE,PARTICULARS"
            cmd = New OleDbCommand(strSql, cn)
            cmd.CommandTimeout = 0
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & sysid & "ADVCRCRADV(ACCODE,RUNNO,PARTICULARS,BALANCE,RESULT )   "
            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " ACCODE,'OPENING' RUNNO,(SELECT TOP 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=A.ACCODE)PARTICULARS,"
            strSql += vbCrLf + " SUM(BALANCE)BALANCE,1"
            strSql += vbCrLf + " FROM TEMPTABLEDB ..TEMP" & sysid & "ADVCRCRADV AS A WHERE RESULT=2 AND BALANCE < 0"
            strSql += vbCrLf + " GROUP BY ACCODE"
            cmd = New OleDbCommand(strSql, cn)
            cmd.CommandTimeout = 0
            cmd.ExecuteNonQuery()

            strSql = "DELETE FROM TEMPTABLEDB ..TEMP" & sysid & "ADVCRCRADV WHERE RESULT=2 AND BALANCE < 0 "
            cmd = New OleDbCommand(strSql, cn)
            cmd.CommandTimeout = 0
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & sysid & "ADVCRCRADV(ACCODE,PARTICULARS,RUNNO,BALANCE,RESULT )   "
            strSql += vbCrLf + " SELECT 'ZZZZZZZ' ACCODE,'TOTAL' AS RUNNO,'GRAND TOTAL',SUM(BALANCE)BALANCE,4 AS RESULT"
            strSql += vbCrLf + " FROM TEMPTABLEDB ..TEMP" & sysid & "ADVCRCRADV WHERE RESULT IN (1,2)"
            cmd = New OleDbCommand(strSql, cn)
            cmd.CommandTimeout = 0
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + " ALTER TABLE TEMPTABLEDB ..TEMP" & sysid & "ADVCRCRADV  "
            strSql += vbCrLf + " ADD NAME VARCHAR(300),ADDRESS VARCHAR(3000)"
            cmd = New OleDbCommand(strSql, cn)
            cmd.CommandTimeout = 0
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + " UPDATE "
            strSql += vbCrLf + " A SET A.NAME=P.PNAME,"
            strSql += vbCrLf + " A.ADDRESS= ISNULL(P.DOORNO,'') +' '+ISNULL(P.ADDRESS1,'') +' '+ISNULL(P.ADDRESS2,'') +' '+ISNULL(P.ADDRESS3,'') +' '"
            strSql += vbCrLf + " +' '+ISNULL(P.AREA,'') +' '+' '+ISNULL(P.PINCODE,'') "
            strSql += vbCrLf + " FROM TEMPTABLEDB ..TEMP" & sysid & "ADVCRCRADV A  "
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..OUTSTANDING AS O ON O.RUNNO = A.RUNNO"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CUSTOMERINFO AS C ON O.BATCHNO = C.BATCHNO  "
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..PERSONALINFO AS P ON P.SNO = C.PSNO "
            cmd = New OleDbCommand(strSql, cn)
            cmd.CommandTimeout = 0
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + " SELECT ACCODE,RUNNO,NAME,ADDRESS,BALANCE,CONVERT(VARCHAR(12),LASTTRANDATE,103) AS TRANDATE,RESULT"
            strSql += vbCrLf + " FROM TEMPTABLEDB ..TEMP" & sysid & "ADVCRCRADV "
            strSql += vbCrLf + " ORDER BY ACCODE,PARTICULARS,RESULT,LASTTRANDATE"
            cmd = New OleDbCommand(strSql, cn)
            da = New OleDbDataAdapter(cmd)
            Dim dt As DataTable
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                With gridView
                    .DataSource = Nothing
                    .DataSource = dt
                    gridStyleTemp()
                    .Focus()
                    .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                    .Invalidate()
                    For Each dgvCol As DataGridViewColumn In .Columns
                        dgvCol.Width = dgvCol.Width
                    Next
                    .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
                End With
                Dim TITLE As String
                TITLE = " CUSTOMER OUTSTANDING REPORT MONTHWISE FROM " & dtpFrom.Text & " TO " & dtpTo.Text
                TITLE += IIf(cmbCostCenter.Text <> "" And cmbCostCenter.Text <> "ALL", " : " & cmbCostCenter.Text, "")
                lblTitle.Font = New System.Drawing.Font("VERDANA", 9, FontStyle.Bold)
                lblTitle.Text = TITLE
            Else
                lblTitle.Text = ""
                MsgBox("Record Not Found", MsgBoxStyle.Information)
                gridView.DataSource = Nothing
                btnNew.Focus()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub


    Private Sub tallyDroptemptabledb()
        strSql = vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMPOUTSTANDING','U') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPOUTSTANDING"
        strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMPOUTSTANDINGFINAL','U') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPOUTSTANDINGFINAL"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()
    End Sub

    Private Sub tallyreportAdvanceMonthWise()

        strSql = " IF  OBJECT_ID('TEMPTABLEDB..TEMPMONTHWISE','U') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPMONTHWISE "
        strSql += vbCrLf + " IF  OBJECT_ID('TEMPTABLEDB..TEMPMONTHWISEFINAL','U') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPMONTHWISEFINAL "
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()


        Dim _compId As String
        _compId = GetQryStringForSp(chkCmbCompany.Text, cnAdminDb & "..COMPANY", "COMPANYID", "COMPANYNAME", True)

        strSql = vbCrLf + " CREATE TABLE TEMPTABLEDB..TEMPMONTHWISEFINAL"
        strSql += vbCrLf + "("
        strSql += vbCrLf + "  [MONTH] INT"
        strSql += vbCrLf + "  ,[YEAR] INT"
        strSql += vbCrLf + " , DEBIT NUMERIC(15,2)"
        strSql += vbCrLf + " , CREDIT NUMERIC(15,2)"
        strSql += vbCrLf + " , RUNNINGTOTAL NUMERIC(15,2)"
        strSql += vbCrLf + " , RESULT INT, COLHEAD VARCHAR(1))"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + " SELECT * INTO TEMPTABLEDB..TEMPMONTHWISE"
        strSql += vbCrLf + " FROM ("
        strSql += vbCrLf + " SELECT "
        strSql += vbCrLf + " NULL [MONTH], NULL [YEAR]"
        strSql += vbCrLf + " ,0 DEBIT,0 CREDIT"
        strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN (AMOUNT+ISNULL(GSTVAL,0)) ELSE -1 * (AMOUNT+ISNULL(GSTVAL,0)) END) CLOSING"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING WHERE "
        strSql += vbCrLf + " TRANDATE < '" & Format(dtpFrom.Value.Date, "yyyy-MM-dd") & "' "
        strSql += vbCrLf + " AND TRANTYPE= 'A' AND RECPAY IN ('R','P') "
        strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + " " & filterby(_compId) & " "
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT"
        strSql += vbCrLf + " [MONTH]"
        strSql += vbCrLf + " ,[YEAR] "
        strSql += vbCrLf + " ,SUM(DEBIT) DEBIT"
        strSql += vbCrLf + " ,SUM(CREDIT) CREDIT"
        strSql += vbCrLf + " ,0 CLOSING"
        strSql += vbCrLf + " FROM ("
        strSql += vbCrLf + " SELECT 0 DEBIT,SUM(AMOUNT+ISNULL(GSTVAL,0)) CREDIT"
        strSql += vbCrLf + " ,0 CLOSING"
        strSql += vbCrLf + " ,MONTH(TRANDATE) [MONTH], YEAR(TRANDATE) [YEAR]"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING WHERE "
        strSql += vbCrLf + " TRANDATE BETWEEN '" & Format(dtpFrom.Value.Date, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value.Date, "yyyy-MM-dd") & "' "
        strSql += vbCrLf + " AND TRANTYPE= 'A' "
        strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + " " & filterby(_compId) & " "
        strSql += vbCrLf + " AND RECPAY ='R'"
        strSql += vbCrLf + " GROUP BY MONTH(TRANDATE), YEAR(TRANDATE)"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT SUM(AMOUNT+ISNULL(GSTVAL,0)) DEBIT,0 CREDIT"
        strSql += vbCrLf + " , 0 CLOSING"
        strSql += vbCrLf + " ,MONTH(TRANDATE) [MONTH], YEAR(TRANDATE) [YEAR]"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING WHERE "
        strSql += vbCrLf + " TRANDATE BETWEEN '" & Format(dtpFrom.Value.Date, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value.Date, "yyyy-MM-dd") & "' "
        strSql += vbCrLf + " AND TRANTYPE= 'A' "
        strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + " " & filterby(_compId) & " "
        strSql += vbCrLf + " AND RECPAY ='P'"
        strSql += vbCrLf + " GROUP BY MONTH(TRANDATE), YEAR(TRANDATE)"
        strSql += vbCrLf + " )X"
        strSql += vbCrLf + " GROUP BY [MONTH],[YEAR]"
        strSql += vbCrLf + " )X ORDER BY [YEAR],[MONTH]"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + " DECLARE db_cursor CURSOR FOR SELECT * FROM TEMPTABLEDB..TEMPMONTHWISE ORDER BY [YEAR],[MONTH]"
        strSql += vbCrLf + " DECLARE @month int;"
        strSql += vbCrLf + " DECLARE @year INT;"
        strSql += vbCrLf + " DECLARE @debit numeric(15,2);"
        strSql += vbCrLf + " DECLARE @credit numeric(15,2);"
        strSql += vbCrLf + " DECLARE @closing numeric(15,2);"
        strSql += vbCrLf + " declare @running numeric(15,2)"
        strSql += vbCrLf + " set @running = 0"
        strSql += vbCrLf + " OPEN db_cursor;"
        strSql += vbCrLf + " FETCH NEXT FROM db_cursor INTO @month,@year,@debit,@credit,@closing;"
        strSql += vbCrLf + " WHILE @@FETCH_STATUS = 0  "
        strSql += vbCrLf + " BEGIN  "
        strSql += vbCrLf + " 		set @running = @running + @closing+@credit-@debit"
        strSql += vbCrLf + " 		insert into TEMPTABLEDB..TEMPMONTHWISEFINAL([month],[year],[debit],[credit],[runningtotal],result,colhead)"
        strSql += vbCrLf + " 		VALUES(@MONTH,@YEAR,@DEBIT,@CREDIT,@RUNNING,1,'D')"
        strSql += vbCrLf + " 		FETCH NEXT FROM db_cursor INTO @month,@year,@debit,@credit,@closing;"
        strSql += vbCrLf + " END;"
        strSql += vbCrLf + " CLOSE db_cursor;"
        strSql += vbCrLf + " DEALLOCATE db_cursor;"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()

        'strSql = " SELECT * FROM TEMPTABLEDB..TEMPMONTHWISEFINAL "
        strSql = ""
        strSql += vbCrLf + " SELECT [MONTH],[YEAR],DEBIT,CREDIT,RUNNINGTOTAL CLOSING FROM TEMPTABLEDB..TEMPMONTHWISEFINAL"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT NULL [MONTH], NULL [YEAR],SUM(DEBIT) DEBIT,SUM(CREDIT) CREDIT,NULL CLOSING FROM TEMPTABLEDB..TEMPMONTHWISEFINAL " 'SUM(RUNNINGTOTAL)

        Dim dt As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            With gridView
                .DataSource = Nothing
                .DataSource = dt
                FormatGridColumns(gridView, False, False, True, False)
            End With
        Else
            MsgBox("No Record Found", MsgBoxStyle.Information)
            gridView.DataSource = Nothing
            Exit Sub
        End If

    End Sub

    Private Function filterby(ByVal _compId As String) As String
        Dim Qry As String = ""
        If _compId <> "" Then Qry += vbCrLf + " AND COMPANYID IN (" & _compId & ")"
        If cmbCostCenter.Text <> "ALL" And cmbCostCenter.Text <> "" Then Qry += vbCrLf + " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN ('" & cmbCostCenter.Text & "')) "
        Return Qry
    End Function

    Private Sub tallyreportAdvanceDayWise(ByVal Type As String)
        tallyDroptemptabledb()

        strSql = vbCrLf + " CREATE TABLE TEMPTABLEDB..TEMPOUTSTANDINGFINAL"
        strSql += vbCrLf + "(PARTICULAR VARCHAR(150), TRANDATE SMALLDATETIME, DEBIT NUMERIC(15,2)"
        strSql += vbCrLf + " , CREDIT NUMERIC(15,2)"
        strSql += vbCrLf + " , RUNNINGTOTAL NUMERIC(15,2), RESULT INT, COLHEAD VARCHAR(1))"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()

        Dim _compId As String
        _compId = GetQryStringForSp(chkCmbCompany.Text, cnAdminDb & "..COMPANY", "COMPANYID", "COMPANYNAME", True)

        strSql = " SELECT * "
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMPOUTSTANDING "
        strSql += vbCrLf + " FROM ( "
        strSql += vbCrLf + " SELECT 'OPENING' PARTICULAR"
        strSql += vbCrLf + " ,NULL TRANDATE1"
        strSql += vbCrLf + " ,0 DEBIT"
        strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN (AMOUNT+ISNULL(GSTVAL,0)) ELSE -1 * (AMOUNT+ISNULL(GSTVAL,0)) END) CREDIT"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING WHERE "
        strSql += vbCrLf + " TRANDATE < '" & Format(dtpFrom.Value.Date, "yyyy-MM-dd") & "'"
        strSql += vbCrLf + " AND TRANTYPE = 'A' "
        strSql += vbCrLf + " AND RECPAY IN('R','P')"
        strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + " " & filterby(_compId) & " "
        strSql += vbCrLf + " UNION ALL "
        strSql += vbCrLf + " SELECT PARTICULAR,TRANDATE1,SUM(DEBIT) DEBIT, SUM(CREDIT) CREDIT "
        strSql += vbCrLf + " FROM ( "
        strSql += vbCrLf + " SELECT "
        strSql += vbCrLf + " CONVERT(VARCHAR(15),TRANDATE ,103) PARTICULAR,"
        strSql += vbCrLf + " TRANDATE AS TRANDATE1"
        strSql += vbCrLf + " ,0 DEBIT"
        strSql += vbCrLf + " ,SUM(AMOUNT+ISNULL(GSTVAL,0)) CREDIT"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING "
        strSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & Format(dtpFrom.Value.Date, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value.Date, "yyyy-MM-dd") & "' "
        strSql += vbCrLf + " AND TRANTYPE = 'A' "
        strSql += vbCrLf + " AND RECPAY = 'R' "
        strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + " " & filterby(_compId) & ""
        strSql += vbCrLf + " GROUP BY TRANDATE"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT "
        strSql += vbCrLf + " CONVERT(VARCHAR(15),TRANDATE ,103) PARTICULAR,"
        strSql += vbCrLf + " TRANDATE AS TRANDATE1"
        strSql += vbCrLf + " ,SUM(AMOUNT+ISNULL(GSTVAL,0)) DEBIT"
        strSql += vbCrLf + " ,0 CREDIT"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING "
        strSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & Format(dtpFrom.Value.Date, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value.Date, "yyyy-MM-dd") & "' "
        strSql += vbCrLf + " AND TRANTYPE = 'A' "
        strSql += vbCrLf + " AND RECPAY = 'P' "
        strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + " " & filterby(_compId) & ""
        strSql += vbCrLf + " GROUP BY TRANDATE"
        strSql += vbCrLf + " ) X GROUP BY PARTICULAR,TRANDATE1"
        strSql += vbCrLf + " ) X "
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + " DECLARE db_cursor CURSOR FOR SELECT PARTICULAR,TRANDATE1,CREDIT,DEBIT FROM TEMPTABLEDB..TEMPOUTSTANDING ORDER BY TRANDATE1"
        strSql += vbCrLf + " DECLARE @trandate VARCHAR(15);"
        strSql += vbCrLf + " DECLARE @trandate1 smalldatetime;"
        strSql += vbCrLf + " DECLARE @credit numeric(15,2);"
        strSql += vbCrLf + " DECLARE @debit numeric(15,2);"
        strSql += vbCrLf + " DECLARE @runningcredit numeric(15,2)"
        strSql += vbCrLf + " SET @runningcredit = 0"
        strSql += vbCrLf + " OPEN db_cursor;"
        strSql += vbCrLf + " FETCH NEXT FROM db_cursor INTO @trandate, @trandate1, @credit,@debit;"
        strSql += vbCrLf + " WHILE @@FETCH_STATUS = 0  "
        strSql += vbCrLf + " BEGIN  "
        strSql += vbCrLf + " 		set @runningcredit = @runningcredit + @credit - @debit"
        strSql += vbCrLf + " 		PRINT @RUNNINGCREDIT"
        strSql += vbCrLf + " 		INSERT INTO TEMPTABLEDB..TEMPOUTSTANDINGFINAL(PARTICULAR,TRANDATE,CREDIT,DEBIT,RUNNINGTOTAL,RESULT,COLHEAD)"
        strSql += vbCrLf + " 		values(@trandate,@trandate1,@credit,@debit,@runningcredit,1,'D')"
        strSql += vbCrLf + "        FETCH NEXT FROM db_cursor INTO @trandate, @trandate1, @credit,@debit;"
        strSql += vbCrLf + " END;"
        strSql += vbCrLf + " CLOSE db_cursor;"
        strSql += vbCrLf + " DEALLOCATE db_cursor;"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()

        gridView.DataSource = Nothing
        If Type = "D" Then
            Dim dt As DataTable

            strSql = "UPDATE TEMPTABLEDB..TEMPOUTSTANDINGFINAL SET CREDIT = NULL,DEBIT = NULL WHERE PARTICULAR = 'OPENING' "
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()

            strSql = " SELECT PARTICULAR,DEBIT,CREDIT"
            strSql += vbCrLf + " ,CONVERT(VARCHAR,CAST(RUNNINGTOTAL AS MONEY),1) CLOSING "
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMPOUTSTANDINGFINAL "
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT 'GRAND TOTAL' PARTICULAR,SUM(DEBIT) DEBIT,SUM(CREDIT) CREDIT"
            strSql += vbCrLf + " ,NULL CLOSING " 'CONVERT(VARCHAR,CAST(SUM(RUNNINGTOTAL) AS MONEY),1)
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMPOUTSTANDINGFINAL "
            'strSql += vbCrLf + " ORDER BY TRANDATE"
            da = New OleDbDataAdapter(strSql, cn)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                With gridView
                    .DataSource = Nothing
                    .DataSource = dt
                    FormatGridColumns(gridView, False, False, True, False)
                End With
            Else
                MsgBox("No Record Found", MsgBoxStyle.Information)
                Exit Sub
            End If
        ElseIf Type = "M" Then
        End If


        tallyDroptemptabledb()


    End Sub

    Private Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click, ViewToolStripMenuItem.Click
        Try

            If rbtFormat2.Checked = True Then
                gridView.DataSource = Nothing
                If rbtDayWise.Checked = True Then tallyreportAdvanceDayWise("D")
                If rbtMonth.Checked = True Then tallyreportAdvanceMonthWise()
                Exit Sub
            End If
            funcTempRpt()
            Exit Sub
            Dim sysid As String
            sysid = Trim(LSet(Mid(Guid.NewGuid.ToString, 1, InStr(Guid.NewGuid.ToString, " - ") - 1), 10))
            gridView.DataSource = Nothing
            gridView.Refresh()
            AutoResizeToolStripMenuItem.Checked = False
            Dim _compId As String
            _compId = GetQryStringForSp(chkCmbCompany.Text, cnAdminDb & "..COMPANY", "COMPANYID", "COMPANYNAME", False)
            sysid = Mid(sysid, 1, 3)
            'strSql = vbCrLf + " EXEC " & cnAdminDb & "..SP_RPT_CUSTOMEROUTSTANDING_MONTH"
            'strSql += vbCrLf + " @DBNAME = '" & cnStockDb & "'"
            'strSql += vbCrLf + " ,@FROMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            'strSql += vbCrLf + " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            'strSql += vbCrLf + " ,@COSTNAME = " & IIf(cmbCostCenter.Text = "" Or cmbCostCenter.Text = "ALL", "'ALL'", GetQryString(cmbCostCenter.Text)) & ""
            'strSql += vbCrLf + " ,@COMPANYID = '" & _compId & "' "
            strSql = vbCrLf + "  EXEC " & cnAdminDb & "..SP_RPT_NEWCUSTOMEROUTSTANDING_ACCODE_MONTH"
            strSql += vbCrLf + "  @DBNAME= '" & cnStockDb & "'"
            strSql += vbCrLf + "  ,@FROMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + "  ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + "  ,@DISPLAY = 'A'"
            strSql += vbCrLf + "  ,@COSTNAME = " & IIf(cmbCostCenter.Text = "" Or cmbCostCenter.Text = "ALL", "'ALL'", GetQryString(cmbCostCenter.Text)) & ""
            strSql += vbCrLf + "  ,@COMPANYID = '" & _compId & "'"
            strSql += vbCrLf + "  ,@TYPEID = 'A,'"
            strSql += vbCrLf + "  ,@ACFILTER = 'A'"
            strSql += vbCrLf + "  ,@SYSID = '" & sysid & "'"
            cmd = New OleDbCommand(strSql, cn)
            cmd.CommandTimeout = 0
            cmd.ExecuteNonQuery()
            'strSql = " SELECT CONVERT(VARCHAR,SNO)SNO,RUNNO,NAME,CONVERT(VARCHAR(12),TRANDATE,103)TRANDATE,AMOUNT,RESULT FROM TEMPTABLEDB..TEMP_MNOUTSTANDING "
            'strSql += vbCrLf + " ORDER BY TRANYEAR,TRANMONTH,CONVERT(INT,SNO),RUNNO  "
            If chkAddress.Checked Then
                strSql = vbCrLf + " SELECT PARTICULAR,RUNNO,ADDRESS,CONVERT(VARCHAR(12),TRANDATE,103) TRANDATE,DEBIT,CREDIT,RESULT"
                strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP_OUTSTANDING" & sysid & " AS S WHERE ISNULL(PARTICULAR,'')<>'' ORDER BY ACCODE,TRANYEAR,TRANMONTH,RESULT"
            Else
                strSql = vbCrLf + " SELECT PARTICULAR,RUNNO,CONVERT(VARCHAR(12),TRANDATE,103) TRANDATE,DEBIT,CREDIT,RESULT"
                strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP_OUTSTANDING" & sysid & " AS S WHERE ISNULL(PARTICULAR,'')<>'' ORDER BY ACCODE,TRANYEAR,TRANMONTH,RESULT"
            End If

            cmd = New OleDbCommand(strSql, cn)
            da = New OleDbDataAdapter(cmd)
            Dim dt As DataTable
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                With gridView
                    .DataSource = Nothing
                    .DataSource = dt
                    gridStyle()
                    .Focus()
                    .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                    .Invalidate()
                    For Each dgvCol As DataGridViewColumn In .Columns
                        dgvCol.Width = dgvCol.Width
                    Next
                    .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
                End With
                Dim TITLE As String
                TITLE = " CUSTOMER OUTSTANDING REPORT MONTHWISE FROM " & dtpFrom.Text & " TO " & dtpTo.Text
                TITLE += IIf(cmbCostCenter.Text <> "" And cmbCostCenter.Text <> "ALL", " : " & cmbCostCenter.Text, "")
                lblTitle.Font = New System.Drawing.Font("VERDANA", 9, FontStyle.Bold)
                lblTitle.Text = TITLE
            Else
                lblTitle.Text = ""
                MsgBox("Record Not Found", MsgBoxStyle.Information)
                gridView.DataSource = Nothing
                btnNew.Focus()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Function gridStyleTemp()
        With gridView
            For Each col As DataGridViewColumn In .Columns
                col.SortMode = DataGridViewColumnSortMode.NotSortable
                If col.Name <> "RUNNO" And col.Name <> "TRANDATE" And col.Name <> "ADDRESS" And col.Name <> "NAME" Then
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End If
            Next
        End With
        If chkAddress.Checked = False Then gridView.Columns("ADDRESS").Visible = False
        gridView.Columns("ACCODE").Visible = False
        gridView.Columns("RESULT").Visible = False
        For Each Gv As DataGridViewRow In gridView.Rows
            With Gv
                Select Case .Cells("RESULT").Value.ToString
                    Case "0"
                        .Cells("RUNNO").Style.BackColor = reportHeadStyle.BackColor
                        .DefaultCellStyle.ForeColor = reportHeadStyle.ForeColor
                        .DefaultCellStyle.Font = reportHeadStyle2.Font
                    Case "1"
                        .DefaultCellStyle.ForeColor = reportHeadStyle2.ForeColor
                        .DefaultCellStyle.Font = reportHeadStyle2.Font
                        .Cells("RUNNO").Style.BackColor = reportHeadStyle2.BackColor
                    Case "3"
                        .DefaultCellStyle.ForeColor = reportHeadStyle1.ForeColor
                        .DefaultCellStyle.Font = reportHeadStyle1.Font
                        .Cells("RUNNO").Style.BackColor = reportHeadStyle1.BackColor
                    Case "4"
                        .Cells("RUNNO").Style.BackColor = reportHeadStyle.BackColor
                        .DefaultCellStyle.ForeColor = reportHeadStyle.ForeColor
                        .DefaultCellStyle.Font = reportHeadStyle2.Font
                End Select
            End With
        Next
    End Function

    Function gridStyle()
        With gridView
            For Each col As DataGridViewColumn In .Columns
                col.SortMode = DataGridViewColumnSortMode.NotSortable
                If col.Name <> "RUNNO" And col.Name <> "PARTICULAR" And col.Name <> "TRANDATE" And col.Name <> "ADDRESS" Then
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End If
            Next
        End With
        gridView.Columns("RESULT").Visible = False
        For Each Gv As DataGridViewRow In gridView.Rows
            With Gv
                Select Case .Cells("RESULT").Value.ToString
                    Case "-2"
                        .Cells("PARTICULAR").Style.BackColor = reportHeadStyle.BackColor 'Color.PaleGreen
                        .Cells("PARTICULAR").Style.ForeColor = reportHeadStyle.ForeColor 'Color.DarkGreen
                        .DefaultCellStyle.Font = reportHeadStyle2.Font
                    Case "-1"
                        '.Cells("PARTICULAR").Style.BackColor = reportHeadStyle.BackColor
                        .Cells("PARTICULAR").Style.ForeColor = reportHeadStyle.ForeColor
                        .Cells("PARTICULAR").Style.Font = reportSubTotalStyle.Font
                    Case "0"
                        .Cells("PARTICULAR").Style.ForeColor = reportHeadStyle.ForeColor
                        .Cells("PARTICULAR").Style.Font = reportSubTotalStyle.Font
                        '.Cells("SNO").Value = ""
                    Case "2"
                        .DefaultCellStyle.ForeColor = reportHeadStyle1.ForeColor
                        .DefaultCellStyle.Font = reportHeadStyle1.Font
                        '.Cells("SNO").Value = ""
                    Case "3"
                        .DefaultCellStyle.ForeColor = reportHeadStyle2.ForeColor
                        .DefaultCellStyle.Font = reportHeadStyle2.Font
                        '    .Cells("SNO").Value = ""
                    Case "4"
                        .DefaultCellStyle.ForeColor = reportHeadStyle1.ForeColor
                        .DefaultCellStyle.Font = reportHeadStyle1.Font
                        '.Cells("SNO").Value = ""
                    Case "5"
                        .DefaultCellStyle.ForeColor = Color.DarkGreen
                        .Cells("PARTICULAR").Style.BackColor = Color.PaleGreen
                        .DefaultCellStyle.Font = reportHeadStyle2.Font
                        '    .Cells("SNO").Value = ""
                End Select
            End With
        Next
    End Function

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        funcNew()
    End Sub

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click, ExportToolStripMenuItem.Click
        BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export)
    End Sub
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click, PrintToolStripMenuItem.Click
        BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print)
    End Sub

    Private Sub AutoResizeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AutoResizeToolStripMenuItem.Click
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
    Private Sub frmCustomerOutstandingMonth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub frmCustomerOutstandingMonth_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        funcNew()
    End Sub

    Private Sub rbtFormat2_CheckedChanged(sender As Object, e As EventArgs) Handles rbtFormat2.CheckedChanged
        GroupBox1.Visible = rbtFormat2.Checked
        chkAddress.Visible = IIf(rbtFormat2.Checked = True, False, True)
    End Sub
End Class