Imports System.Data.OleDb
Public Class frmChequeBookEntry
    Dim strSql As String

    Dim cmd As OleDbCommand
    Dim tran As OleDbTransaction
    Dim dr As OleDbDataReader
    Dim upflag As Boolean = False
    Dim dt As New DataTable()
    Dim dtCostCentre As New DataTable

    Function funcNew() As Integer
        objGPack.TextClear(Me)
        cmbBankName_MAN.Text = ""
        cmbBankName_MAN.Enabled = True
        txtCheqNo_NUM.ReadOnly = False
        '  BrighttechPack.GlobalMethods.FillCombo(CmbCostCentre, dtCostCentre, "COSTNAME", , "ALL")
        grpFields.Focus()
        upflag = False
    End Function

    Function funcDefaultEvents() As Integer
        'ComboValidation(cmbBankName)
        'objGPack.V()
    End Function

    Function funcChequePrefix(ByVal len As Integer) As String
        Dim str As String = Nothing
        For cnt As Integer = 1 To txtCheqNo_NUM.Text.Length - len
            str += "0"
        Next
        Return str
    End Function

    Private Sub frmChequeBookEntry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Escape) Then
            If tabMain.SelectedTab.Name = tabView.Name Then
                tabMain.SelectedTab = tabGeneral
            End If
        ElseIf e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmChequeBookEntry_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            tabMain.ItemSize = New System.Drawing.Size(1, 1)
            tabMain.SelectedTab = tabGeneral
            ''load Bank Name
            strSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACTYPE='B' ORDER BY ACNAME"
            objGPack.FillCombo(strSql, cmbBankName_MAN, , False)
            objGPack.FillCombo(strSql, cmbOpenBankName, , False)
            If UCase(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COSTCENTRE'")) = "Y" Then
                strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
                objGPack.FillCombo(strSql, cmbCostCenter_MAN, False, False)
                strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE where costid ='" & cnCostId & "'"
                cmbCostCenter_MAN.Text = objGPack.GetSqlValue(strSql)
                cmbCostCenter_MAN.Enabled = True
            Else
                cmbCostCenter_MAN.Enabled = False
            End If
            funcDefaultEvents()
            funcNew()
        Catch ex As Exception
            MsgBox("ERROR :" + ex.Message + vbCrLf + vbCrLf + "POSITION:" + ex.StackTrace)
        End Try
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If txtCheqNo_NUM.Text = "" Then
            MsgBox("CheqNo Should Not Empty", MsgBoxStyle.Information)
            txtCheqNo_NUM.Focus()
            Exit Sub
        End If

        If Not Val(txtNoOfLeafes_NUM.Text) > 0 And upflag = False Then
            MsgBox("Invalid Leaves", MsgBoxStyle.Information)
            txtNoOfLeafes_NUM.Focus()
            Exit Sub
        End If

        If Not Val(txtTranLimit_AMT.Text) > 0 Then
            MsgBox("Transaction Limit Shouldbe GreaterThan Zero", MsgBoxStyle.Information)
            txtTranLimit_AMT.Focus()
            Exit Sub
        End If
        If Not Val(txtchqformat_NUM.Text) > 0 Then
            MsgBox("Invalid Cheque Format", MsgBoxStyle.Information)
            txtchqformat_NUM.Focus()
            Exit Sub
        End If

        Dim bankCode As String = Nothing
        ''Get BankCode 
        strSql = " SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbBankName_MAN.Text & "'"
        cmd = New OleDbCommand(strSql, cn, tran)
        dr = cmd.ExecuteReader
        If dr.Read Then
            bankCode = dr.Item("ACCODE").ToString
        End If
        dr.Close()
        If upflag = False Then
            Dim entryOrder As Integer = Nothing
            Dim chqNo As String = Nothing
            Try
                tran = cn.BeginTransaction
                ''Get EntryOrder
                strSql = " SELECT ISNULL(MAX(ENTRYORDER),0) + 1 AS ENTRYORDER FROM " & cnStockDb & "..CHEQUEBOOK"
                cmd = New OleDbCommand(strSql, cn, tran)
                dr = cmd.ExecuteReader
                If dr.Read Then
                    entryOrder = Val(dr.Item("ENTRYORDER").ToString)
                End If
                dr.Close()
                Dim BatchNo As String
                BatchNo = GetNewBatchno(cnCostId, DateTime.Today.Date, tran)
                Dim MCostid As String = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME='" & cmbCostCenter_MAN.Text & "'", , "", tran)

                For cnt As Integer = 0 To Val(txtNoOfLeafes_NUM.Text) - 1
                    chqNo = Val(txtCheqNo_NUM.Text) + cnt
                    chqNo = funcChequePrefix(chqNo.Length) + chqNo
                    strSql = " INSERT INTO " & cnStockDb & "..CHEQUEBOOK"
                    strSql += " ("
                    strSql += " SNO,BANKCODE,TOPARTY,CHQNUMBER,CHQDATE,CHQISSUEDATE,CHQFORMAT"
                    strSql += " ,AMOUNT,ENTRYORDER,CANCEL,SYSTEMID,USERID,UPDATED,UPTIME,TRANLIMIT,BATCHNO,COSTID"
                    strSql += " )"
                    strSql += " VALUES"
                    strSql += " ("
                    strSql += " '" & GetNewSno(TranSnoType.CHEQUEBOOKCODE, tran) & "'" ''SNO
                    strSql += " ,'" & bankCode & "'" 'BANKCODE
                    strSql += " ,''" 'TOPARTY
                    strSql += " ,'" & chqNo & "'" 'CHQNUMBER
                    strSql += " ,'" & Today.Date & "'" 'CHQDATE
                    strSql += " ,NULL" 'CHQISSUEDATE
                    strSql += " ," & Val(txtchqformat_NUM.Text.ToString()) & "" 'CHQFORMAT
                    strSql += " ,0" 'AMOUNT
                    strSql += " ," & entryOrder + cnt & "" 'ENTRYORDER
                    strSql += " ,''" 'CANCEL
                    strSql += " ,'" & systemId & "'" 'SYSTEMID
                    strSql += " ," & userId & "" 'USERID
                    strSql += " ,'" & Today.Date & "'" 'UPDATED
                    strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                    strSql += " ,'" & Val(txtTranLimit_AMT.Text) & "'" 'Transaction Limit
                    strSql += " ,'" & BatchNo & "'" 'BATCHNO
                    strSql += " ,'" & MCostid & "'" 'COSTID
                    strSql += " )"
                    'cmd = New OleDbCommand(strSql, cn, tran)
                    'cmd.ExecuteNonQuery()
                    ExecQuery(SyncMode.Stock, strSql, cn, tran, MCostid)

                Next

                strSql = "SELECT 1 FROM " & cnAdminDb & "..CHQPRINT_FORMAT WHERE FORMATTYPE=" & Val(txtchqformat_NUM.Text.ToString()) & ""
                cmd = New OleDbCommand(strSql, cn, tran)
                da = New OleDbDataAdapter(cmd)
                dt = New DataTable
                da.Fill(dt)
                If Not dt.Rows.Count > 0 Then
                    InsertIntoCHQPRINT_FORMAT("1", cnCompanyId, cnCostId, txtchqformat_NUM.Text.ToString(), "CHEQUEDATE", 0.2, 32, 12, "DATE", 0, 1, 0, 0, 0, "999", DateTime.Now, 1, 0, 0, "Times New Roman", 0, 0, "dd/MMM/yyyy")
                    InsertIntoCHQPRINT_FORMAT("2", cnCompanyId, cnCostId, txtchqformat_NUM.Text.ToString(), "BEARER", 1.4, 7, 60, "BEARER", 0, 1, 0, 0, 0, "999", DateTime.Now, 1, 0, 0, "Times New Roman", 0, 0, "")
                    InsertIntoCHQPRINT_FORMAT("3", cnCompanyId, cnCostId, txtchqformat_NUM.Text.ToString(), "CHEQUEAMOUNT", 4.9, 24, 15, "Rs.", 0, 1, 1, 0, 0, "999", DateTime.Now, 1, 0, 0, "Times New Roman", 0, 0, "")
                    InsertIntoCHQPRINT_FORMAT("4", cnCompanyId, cnCostId, txtchqformat_NUM.Text.ToString(), "AMOUNTINWORDS", 5, 5, 50, "InWords", 0, 1, 0, 0, 0, "999", DateTime.Now, 1, 0, 0, "Times New Roman", 0, 0, "")
                    InsertIntoCHQPRINT_FORMAT("5", cnCompanyId, cnCostId, txtchqformat_NUM.Text.ToString(), "BEARER1", 2.8, 1, 60, " ", 0, 1, 0, 0, 0, "999", DateTime.Now, 1, 0, 0, "Times New Roman", 0, 0, "")
                    InsertIntoCHQPRINT_FORMAT("6", cnCompanyId, cnCostId, txtchqformat_NUM.Text.ToString(), "AMOUNTINWORDS1", 6.5, 1, 50, " ", 0, 1, 0, 0, 0, "999", DateTime.Now, 1, 0, 0, "Times New Roman", 0, 0, "")
                    InsertIntoCHQPRINT_FORMAT("7", cnCompanyId, cnCostId, txtchqformat_NUM.Text.ToString(), "AC PAYEE", 0.2, 15, 15, "A/C PAYEE", 1, 1, 0, 0, 0, "999", DateTime.Now, 1, 0, 0, "Times New Roman", 0, 0, "")
                End If
                tran.Commit()
                Dim noOfLeafes As Integer = txtNoOfLeafes_NUM.Text
                funcNew()
                MsgBox(noOfLeafes.ToString + " Leaves Generated..")
            Catch ex As Exception
                tran.Rollback()
                MsgBox("MESSAGE    :" + ex.Message + vbCrLf + "STACKTRACE  :" + ex.StackTrace, MsgBoxStyle.Information)
            End Try
        ElseIf upflag = True Then
            Try
                tran = Nothing
                tran = cn.BeginTransaction
                strSql = " UPDATE " & cnStockDb & "..CHEQUEBOOK SET "
                strSql += " CHQFORMAT=" & Val(txtchqformat_NUM.Text.ToString()) & ""
                strSql += " ,TRANLIMIT='" & Val(txtTranLimit_AMT.Text.ToString()) & "'"
                strSql += " FROM " & cnStockDb & "..CHEQUEBOOK AS C WHERE 1=1 "
                strSql += vbCrLf + " BANKCODE='" & bankCode & "'"
                strSql += vbCrLf + " CHQNUMBER  ='" & txtCheqNo_NUM.Text.ToString() & "'"
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.ExecuteNonQuery()
                tran.Commit()
                funcNew()
                MsgBox(" Records Updated..")
            Catch ex As Exception
                tran.Rollback()
                MsgBox("MESSAGE    :" + ex.Message + vbCrLf + "STACKTRACE  :" + ex.StackTrace, MsgBoxStyle.Information)
            End Try
            
        End If
    End Sub
    Private Sub InsertIntoCHQPRINT_FORMAT( _
       ByVal sno As String, ByVal companyid As String, ByVal costid As String _
       , ByVal formattype As String, ByVal colname As String, ByVal PRINTROW As Double, ByVal PRINTCOL As Integer _
, ByVal COLWIDTH As Integer, ByVal LABELDESC As String, ByVal ISLABELPRINT As Integer, ByVal ISBOLD As Integer _
, ByVal ISDOUBLE As Integer, ByVal ISCONDENSE As Integer, ByVal ISUNDERLINE As Integer, ByVal USERID As String _
, ByVal LUPDATE As DateTime, ByVal ACTIVE As Integer, ByVal ISMEDIUM As Integer, ByVal ISCENTRE As Integer _
, ByVal FONTNAME As String, ByVal ISITALIC As Integer, ByVal ALLIGNMENT As Integer, ByVal CUSTOMFORMAT As String)
        Dim dt As New DataTable
        dt.Clear()
        strSql = " SELECT 1 FROM " & cnAdminDb & "..CHQPRINT_FORMAT WHERE  FORMATTYPE=" & formattype & " AND COLNAME='" & colname & "'"
        cmd = New OleDbCommand(strSql, cn, tran)
        da = New OleDbDataAdapter(cmd)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            Exit Sub
        End If
        strSql = " INSERT INTO " & cnAdminDb & "..CHQPRINT_FORMAT"
        strSql += vbCrLf + "  ( SNO,COMPANYID,COSTID,FORMATTYPE"
        strSql += vbCrLf + "  ,COLNAME ,PRINTROW,PRINTCOL,COLWIDTH,LABELDESC,ISLABELPRINT,ISBOLD,ISDOUBLE,ISCONDENSE"
        strSql += vbCrLf + "  ,ISUNDERLINE,USERID,LUPDATE,ACTIVE,ISMEDIUM,ISCENTRE,FONTNAME"
        strSql += vbCrLf + "  ,ISITALIC,ALIGNMENT,CUSTOMFORMAT )"
        strSql += vbCrLf + "  VALUES"
        strSql += vbCrLf + "  ("
        strSql += vbCrLf + "  '" & sno & "','" & companyid & "','" & costid & "'"
        strSql += vbCrLf + "  ," & formattype & ",'" & colname & "'," & PRINTROW & "," & PRINTCOL & ""
        strSql += vbCrLf + "  ," & COLWIDTH & ",'" & LABELDESC & "'," & ISLABELPRINT & "," & ISBOLD & ""
        strSql += vbCrLf + "  ," & ISDOUBLE & "," & ISCONDENSE & "," & ISUNDERLINE & ",'" & USERID & "'"
        strSql += vbCrLf + "  ,'" & LUPDATE & "'," & ACTIVE & "," & ISMEDIUM & "," & ISCENTRE & ""
        strSql += vbCrLf + "  ,'" & FONTNAME & "'," & ISITALIC & "," & ALLIGNMENT & ",'" & CUSTOMFORMAT & "'"
        strSql += vbCrLf + "  )"
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()
    End Sub


    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click
        Try
            tabMain.SelectedTab = tabView
            dtpOpenFrom.Value = Today.Date
            dtpOpenTo.Value = Today.Date
            rbtAll.Checked = True
            gridView_OWN.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            gridView_OWN.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            lblCancel.Font = New Font("VERDANA", 8, FontStyle.Bold)
            pnlGridViewHeader.Visible = False
            gridView_OWN.DataSource = Nothing
            cmbOpenBankName.Text = ""
            dtpOpenFrom.Focus()
        Catch ex As Exception
            MsgBox("ERROR:" + ex.Message + vbCrLf + vbCrLf + "POSITION:" + ex.StackTrace)
        End Try
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        Try
            funcNew()
        Catch ex As Exception
            MsgBox("ERROR:" + ex.Message + vbCrLf + vbCrLf + "POSITION:" + ex.StackTrace)
        End Try
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        btnSave_Click(Me, New EventArgs)
    End Sub

    Private Sub OpenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click
        btnOpen_Click(Me, New EventArgs)
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub btnOpenSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            Dim dtGridView As New DataTable
            gridView_OWN.DataSource = Nothing
            If rbtAll.Checked Then
                strSql = " SELECT"
                strSql += " (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = BANKCODE)AS PARTICULAR"
                strSql += " ,(SELECT TOP 1 CHQNUMBER FROM " & cnStockDb & "..CHEQUEBOOK WHERE BANKCODE = X.BANKCODE AND BATCHNO = X.BATCHNO ORDER BY ENTRYORDER ASC)AS FROMCHQNO"
                strSql += " ,(SELECT TOP 1 CHQNUMBER FROM " & cnStockDb & "..CHEQUEBOOK WHERE BANKCODE = X.BANKCODE AND BATCHNO = X.BATCHNO ORDER BY ENTRYORDER DESC)AS TOCHQNO"
                strSql += ",CHQFORMAT ,TRANLIMIT"
                strSql += " FROM "
                strSql += " ("
                strSql += " SELECT DISTINCT BANKCODE,BATCHNO,CHQFORMAT,TRANLIMIT FROM " & cnStockDb & "..CHEQUEBOOK"
                strSql += " WHERE CHQDATE BETWEEN  '" & dtpOpenFrom.Value & "' AND '" & dtpOpenTo.Value & "'"
                If cmbOpenBankName.Text <> "" Then
                    strSql += " AND BANKCODE = (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbOpenBankName.Text & "')"
                End If
                strSql += " )X ORDER BY PARTICULAR"
                'strSql = " SELECT ''PARTICULAR,''FROMCHQNO,''TOCHQNO WHERE 1<>1"
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtGridView)
                gridView_OWN.DataSource = dtGridView
                If Not dtGridView.Rows.Count > 0 Then
                    MsgBox("Record Not Found", MsgBoxStyle.Information)
                    dtpOpenFrom.Focus()
                    Exit Sub
                End If
                With gridView_OWN
                    With .Columns("PARTICULAR")
                        .Width = 250
                    End With
                    With .Columns("FROMCHQNO")
                        .HeaderText = "FROM CHQ_NO"
                        .Width = 150
                        .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    End With
                    With .Columns("CHQFORMAT")
                        .HeaderText = "CHQ_FORMAT"
                        .Width = 100
                        .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    End With
                    With .Columns("TOCHQNO")
                        .HeaderText = "TO CHQ_NO"
                        .Width = 120
                        .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    End With
                    With .Columns("TRANLIMIT")
                        .HeaderText = "LIMIT"
                        .Width = 120
                        .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    End With
                End With
                lblCancel.Visible = False
            ElseIf rbtUsed.Checked Then
                strSql = " SELECT CHQNO,CHQISSUEDATE,PARTICULAR,AMOUNT,SNO,CHQFORMAT,TRANLIMIT FROM "
                strSql += " ("
                strSql += " SELECT CHQNUMBER CHQNO,CONVERT(VARCHAR,CHQISSUEDATE,103)CHQISSUEDATE"
                strSql += " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = BANKCODE)AS PARTICULAR"
                strSql += " ,AMOUNT"
                strSql += " ,SNO,TRANLIMIT,CHQFORMAT,ENTRYORDER"
                strSql += " FROM " & cnStockDb & "..CHEQUEBOOK AS C"
                strSql += " WHERE CHQDATE BETWEEN  '" & dtpOpenFrom.Value & "' AND '" & dtpOpenTo.Value & "'"
                If cmbOpenBankName.Text <> "" Then
                    strSql += " AND BANKCODE = (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbOpenBankName.Text & "')"
                End If
                strSql += " AND CHQISSUEDATE IS NOT NULL"
                strSql += " AND ISNULL(CANCEL,'') = ''"
                strSql += " )X"
                strSql += " ORDER BY PARTICULAR,ENTRYORDER"
                'strSql = " SELECT ''CHQNO,''CHQISSUEDATE,''PARTICULAR,''AMOUNT WHERE 1<>1"
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtGridView)
                gridView_OWN.DataSource = dtGridView
                If Not dtGridView.Rows.Count > 0 Then
                    MsgBox("Record Not Found", MsgBoxStyle.Information)
                    dtpOpenFrom.Focus()
                    Exit Sub
                End If
                With gridView_OWN
                    With .Columns("CHQNO")
                        .HeaderText = "CHQ_NO"
                        .Width = 120
                    End With
                    With .Columns("CHQISSUEDATE")
                        .HeaderText = "ISS DATE"
                        .Width = 100
                    End With
                    With .Columns("PARTICULAR")
                        .Width = 250
                    End With
                    With .Columns("CHQFORMAT")
                        .HeaderText = "CHQ_FORMAT"
                        .Width = 100
                        .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    End With
                    With .Columns("AMOUNT")
                        .Width = 100
                        .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    End With
                    With .Columns("SNO")
                        .Visible = False
                    End With
                    With .Columns("TRANLIMIT")
                        .HeaderText = "LIMIT"
                        .Width = 120
                        .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    End With
                End With
                lblCancel.Visible = True
            Else
                strSql = " SELECT CHQNO,CHQRECEIPTDATE,PARTICULAR,SNO,CHQFORMAT,TRANLIMIT FROM "
                strSql += " ("
                strSql += " SELECT CHQNUMBER CHQNO,CONVERT(VARCHAR,CHQDATE,103)CHQRECEIPTDATE"
                strSql += " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = BANKCODE)AS PARTICULAR"
                strSql += " ,SNO,TRANLIMIT,CHQFORMAT,ENTRYORDER"
                strSql += " FROM " & cnStockDb & "..CHEQUEBOOK AS C"
                strSql += " WHERE CHQDATE BETWEEN  '" & dtpOpenFrom.Value & "' AND '" & dtpOpenTo.Value & "'"
                If cmbOpenBankName.Text <> "" Then
                    strSql += " AND BANKCODE = (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbOpenBankName.Text & "')"
                End If
                strSql += " AND CHQISSUEDATE IS NULL"
                strSql += " AND ISNULL(CANCEL,'') = ''"
                strSql += " )X"
                strSql += " ORDER BY PARTICULAR,CHQRECEIPTDATE,ENTRYORDER"
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtGridView)
                gridView_OWN.DataSource = dtGridView
                With gridView_OWN
                    With .Columns("CHQNO")
                        .HeaderText = "CHQ_NO"
                        .Width = 100
                    End With
                    With .Columns("CHQFORMAT")
                        .HeaderText = "CHQ_FORMAT"
                        .Width = 100
                        .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    End With
                    With .Columns("CHQRECEIPTDATE")
                        .HeaderText = "RECEIPT DATE"
                        .Width = 130
                    End With
                    With .Columns("PARTICULAR")
                        .Width = 250
                    End With
                    With .Columns("SNO")
                        .Visible = False
                    End With
                    With .Columns("TRANLIMIT")
                        .HeaderText = "LIMIT"
                        .Width = 120
                        .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    End With
                End With
                lblCancel.Visible = True
            End If
            gridView_OWN.Focus()
        Catch ex As Exception
            MsgBox("ERROR :" + ex.Message + vbCrLf + "STACKTRACE  :" + ex.StackTrace, MsgBoxStyle.Information)
        End Try
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView_OWN.KeyDown
        If e.KeyCode = Keys.Enter Then
            'strSql = "SELECT (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=C.BANKCODE)BANKNAME"
            'strSql += vbCrLf + ",CHQNUMBER,CHQFORMAT,TRANLIMIT FROM " & cnStockDb & "..CHEQUEBOOK AS C WHERE 1=1"
            'strSql += vbCrLf + " AND CHQDATE BETWEEN '" & dtpOpenFrom.Value & "' AND '" & dtpOpenTo.Value & "' AND"
            'strSql += vbCrLf + " BANKCODE=(SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME "
            'strSql += vbCrLf + "='" & gridView_OWN.CurrentRow.Cells("PARTICULAR").Value.ToString & "') AND CHQNUMBER  ='" & gridView_OWN.CurrentRow.Cells("CHQNO").Value.ToString & "'"
            'da = New OleDbDataAdapter(strSql, cn)
            'Dim dtt As New DataTable()
            'da.Fill(dtt)
            'If dtt.Rows.Count > 0 Then
            '    cmbBankName_MAN.Text = dtt.Rows(0).Item("BANKNAME").ToString()
            '    txtCheqNo_NUM.Text = dtt.Rows(0).Item("CHQNUMBER").ToString()
            '    txtchqformat_NUM.Text = dtt.Rows(0).Item("CHQFORMAT").ToString()
            '    txtTranLimit_AMT.Text = dtt.Rows(0).Item("TRANLIMIT").ToString()
            '    cmbBankName_MAN.Enabled = False
            '    txtCheqNo_NUM.ReadOnly = True
            '    upflag = True
            'End If

        End If
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView_OWN.KeyPress
        Try
            If UCase(e.KeyChar) = "C" Then
                If rbtPending.Checked Then
                    strSql = " UPDATE " & cnStockDb & "..CHEQUEBOOK SET CANCEL = 'Y' WHERE SNO = '" & gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("SNO").Value.ToString & "'"
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                    If cmd.ExecuteNonQuery > 0 Then
                        MsgBox("Successfully Cancelled..", MsgBoxStyle.Information)
                        btnOpenSearch_Click(Me, New EventArgs)
                    End If
                End If
            End If
        Catch ex As Exception
            MsgBox("ERROR :" + ex.Message + vbCrLf + "STACKTRACE  :" + ex.StackTrace, MsgBoxStyle.Information)
        End Try
    End Sub
    Private Sub btnOpenExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        'Me.Cursor = Cursors.WaitCursor
        If gridView_OWN.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView_OWN, BrightPosting.GExport.GExportType.Export)
        End If
        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub btnOpenPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        'BrighttechPack.PrintDataGridView.Print_DataGridView(btnOpenPrint, gridView)
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView_OWN.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView_OWN, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGeneral.Left, Me.tabGeneral.Top, Me.tabGeneral.Width, Me.tabGeneral.Height))
        tabGeneral.BackgroundImage = bakImage
        tabView.BackgroundImage = bakImage
        tabGeneral.BackgroundImageLayout = ImageLayout.Stretch
        tabView.BackgroundImageLayout = ImageLayout.Stretch
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        If tabMain.SelectedTab.Name = tabView.Name Then
            tabMain.SelectedTab = tabGeneral
        End If
    End Sub

End Class