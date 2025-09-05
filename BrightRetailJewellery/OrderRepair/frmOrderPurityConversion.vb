Imports System.IO
Imports System.Data.OleDb
Public Class frmorderpurityconversion

#Region "Variable Declaration"

    Dim CostId As String = ""
    Dim cmd As OleDbCommand
    Dim dt As New DataTable
    Dim strsql As String
    Dim recgrswt As Double = 0
    Dim convgrswt As Double = 0
    Dim convnetwt As Double = 0
    Dim curpurity As Double = 0
    Dim curgrswt As Double = 0
    Dim curnetwt As Double = 0
    Dim retval As Double = 0

#End Region

#Region "Events"

    Private Sub frmOrderPurityConversion_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        funcnew()
    End Sub

    Private Sub exitToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles exitToolStripMenuItem1.Click
        Me.Close()
    End Sub

    Private Sub newToolStripMenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles newToolStripMenuItem2.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub salesToolStripMenuItem3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles salesToolStripMenuItem3.Click
        btnSave_Click(Me, New EventArgs)
    End Sub

    Private Sub frmorderpurityconversion_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Enter And txtorderNo.Focused = False Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

   

    Private Sub txtorderNo_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtorderNo.KeyDown

        If e.KeyCode = Keys.Insert Then
            CostId = cnCostId
            strsql = vbCrLf + "  SELECT"
            strsql += vbCrLf + "  	 DISTINCT SUBSTRING(ORNO,6,20)ORNO,OU.GRSWT,OU.NETWT,OU.PURITY,CA.CATNAME AS CATEGORY,O.COMPANYID COMPANYID_HIDE,O.COSTID COSTID_HIDE"
            strsql += vbCrLf + "  	,PNAME"
            strsql += vbCrLf + "  	,CASE WHEN ISNULL(DOORNO,'') = '' THEN ADDRESS1 ELSE ISNULL(DOORNO,'') + ISNULL(ADDRESS1,'') END ADDRESS1"
            strsql += vbCrLf + "  	,ADDRESS2,MOBILE"
            strsql += vbCrLf + "  	,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = O.COSTID) COSTCENTRE"
            strsql += vbCrLf + "  FROM " & cnadmindb & "..ORMAST AS O "
            strsql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..OUTSTANDING AS OU ON O.BATCHNO=OU.BATCHNO "
            strsql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..CATEGORY AS CA ON OU.CATCODE=CA.CATCODE "
            strsql += vbCrLf + "  LEFT OUTER JOIN " & cnAdminDb & "..CUSTOMERINFO AS C ON O.BATCHNO = C.BATCHNO "
            strsql += vbCrLf + "  LEFT OUTER JOIN " & cnAdminDb & "..PERSONALINFO AS P"
            strsql += vbCrLf + "  ON C.PSNO = P.SNO WHERE ISNULL(O.CANCEL,'') != 'Y' AND OU.RECPAY='R' AND ISNULL(O.ODBATCHNO,'')=''"
            strsql += vbCrLf + "  AND ISNULL(O.COSTID,'') = '" & CostId & "' AND ISNULL(OU.SETTLED,'')<>'C' AND  OU.GRSWT<>0"
            Dim dr As DataRow
            dr = Nothing
            dr = BrighttechPack.SearchDialog.Show_R("Find Order No", strsql, cn, , , , , , , , False)
            If dr IsNot Nothing Then
                txtcategory.Text = dr.Item("CATEGORY").ToString
                txtorderNo.Text = dr.Item("ORNO").ToString
                txtadvancepurity_WET.Text = Val(dr.Item("PURITY").ToString)
                txtadvancewt_WET.Text = Val(dr.Item("NETWT").ToString)
                recgrswt = Val(dr.Item("GRSWT").ToString)
                cmbPurity_OWN.Focus()
                cmbPurity_OWN.SelectAll()
            Else
                MsgBox("No records found.", MsgBoxStyle.Information, "Brighttech Information")
            End If
        ElseIf e.KeyCode = Keys.Enter Then
            strsql = "SELECT DISTINCT SUBSTRING(ORNO,6,20)ORNO,OU.GRSWT,OU.NETWT,OU.PURITY,CA.CATNAME AS CATEGORY"
            strsql += vbCrLf + " FROM " & cnadmindb & "..ORMAST O"
            strsql += vbCrLf + " INNER JOIN " & cnAdminDb & "..OUTSTANDING OU ON O.BATCHNO=OU.BATCHNO"
            strsql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY CA ON OU.CATCODE=CA.CATCODE"
            strsql += vbCrLf + " WHERE ISNULL(O.CANCEL,'') != 'Y'"
            strsql += vbCrLf + " AND ISNULL(O.COSTID,'') = '" & CostId & "' AND ISNULL(OU.SETTLED,'')<>'C' AND OU.RECPAY='R' AND ISNULL(O.ODBATCHNO,'')='' AND  OU.GRSWT<>0"
            strsql += vbCrLf + " AND ISNULL(O.ORNO,'')='" & GetCostId(CostId) & GetCompanyId(strCompanyId) + txtorderNo.Text.ToString & "'"
            dt = New DataTable
            da = New OleDbDataAdapter(strsql, cn)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                txtcategory.Text = dt.Rows(0).Item("CATEGORY").ToString
                txtorderNo.Text = dt.Rows(0).Item("ORNO").ToString
                txtadvancepurity_WET.Text = Val(dt.Rows(0).Item("PURITY").ToString)
                txtadvancewt_WET.Text = Val(dt.Rows(0).Item("NETWT").ToString)
                recgrswt = Val(dt.Rows(0).Item("GRSWT").ToString)
                cmbPurity_OWN.Focus()
                cmbPurity_OWN.SelectAll()
            Else
                MsgBox("No records found.", MsgBoxStyle.Information, "Brighttech Information")
            End If
        End If
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        funcsave()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        funcnew()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

#End Region

#Region "User Defined Function"

    Private Sub funcnew()
        strsql = " SELECT DISTINCT PURITY FROM " & cnAdminDb & "..PURITYMAST"
        dt = New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            cmbPurity_OWN.DataSource = Nothing
            cmbPurity_OWN.DataSource = dt
            cmbPurity_OWN.DisplayMember = "PURITY"
        End If
        txtorderNo.Text = ""
        txtcategory.Text = ""
        txtadvancepurity_WET.Text = ""
        txtadvancewt_WET.Text = ""
        txtconvtwt_WET.Text = ""
        rbtPurity.Checked = True
        cmbPurity_OWN.Enabled = True : txtconvtwt_WET.Enabled = True
        txtConvRate_AMT.Enabled = False : txtconvAmt_AMT.Enabled = False
        txtConvRate_AMT.Text = "" : txtconvAmt_AMT.Text = ""
        txtorderNo.Focus()
        txtorderNo.SelectAll()
    End Sub

    Private Sub funcsave()
        Try
            If rbtPurity.Checked = True Then
                If Val(cmbPurity_OWN.Text) = Val(txtadvancepurity_WET.Text) Then
                    MsgBox("Converted Purity Shouldn't be same as Advance purity", MsgBoxStyle.Information)
                    cmbPurity_OWN.Focus()
                    Exit Sub
                End If
                If (Val(txtconvtwt_WET.Text) = 0) Then
                    MsgBox("Converted Weight Shouldn't be Empty", MsgBoxStyle.Information)
                    Exit Sub
                End If
            End If
            If rbtAmount.Checked = True Then
                If Val(txtadvancewt_WET.Text) <> 0 And Val(txtconvAmt_AMT.Text) = 0 Then
                    MsgBox("Converted Amount Shouldn't be zero", MsgBoxStyle.Information)
                    Exit Sub
                End If
            End If
            strsql = vbCrLf + " SELECT SNO,TRANNO,TRANDATE,TRANTYPE,RUNNO,AMOUNT,GRSWT, NETWT,PUREWT,RECPAY,REFNO"
            strsql += vbCrLf + " ,REFDATE,EMPID,TRANSTATUS,100 PURITY,CATCODE,BATCHNO,USERID,UPDATED,UPTIME,SYSTEMID,RATE"
            strsql += vbCrLf + " ,VALUE,CANCEL,CASHID,VATEXM,REMARK1,REMARK2,FROMFLAG,ACCODE,CTRANCODE,DUEDATE"
            strsql += vbCrLf + " ,APPVER,COSTID,COMPANYID,TRANSFERED,FLAG,PAYMODE,TRANFLAG,SETTLED,SETTLEBATCHNO"
            strsql += vbCrLf + "  FROM " & cnAdminDb & "..OUTSTANDING WHERE BATCHNO IN(SELECT BATCHNO FROM " & cnadmindb & "..ORMAST  "
            strsql += vbCrLf + " WHERE ORNO='" & GetCostId(CostId) & GetCompanyId(cnCompanyId) + txtorderNo.Text.ToString & "') AND ISNULL(SETTLED,'')<>'C' "
            da = New OleDbDataAdapter(strsql, cn)
            dt = New DataTable()
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                tran = Nothing
                tran = cn.BeginTransaction()
                strsql = " UPDATE " & cnAdminDb & "..OUTSTANDING SET SETTLED='C' WHERE BATCHNO IN(SELECT BATCHNO FROM " & cnadmindb & "..ORMAST  "
                strsql += vbCrLf + " WHERE ORNO='" & GetCostId(CostId) & GetCompanyId(cnCompanyId) + txtorderNo.Text.ToString & "') AND ISNULL(SETTLED,'')<>'C' "
                ExecQuery(SyncMode.Transaction, strsql, cn, tran, CostId)

                Dim SNO As String = GetNewSno(TranSnoType.OUTSTANDINGCODE, tran, "GET_ADMINSNO_TRAN")
                With dt.Rows(0)
                    strsql = vbCrLf + " INSERT INTO " & cnAdminDb & "..OUTSTANDING (SNO,TRANNO,TRANDATE,TRANTYPE,RUNNO,AMOUNT,GRSWT,NETWT,PUREWT,RECPAY,REFNO"
                    strsql += vbCrLf + " ,REFDATE,EMPID,TRANSTATUS,PURITY,CATCODE,BATCHNO,USERID,UPDATED,UPTIME,SYSTEMID,RATE"
                    strsql += vbCrLf + " ,VALUE,CANCEL,CASHID,VATEXM,REMARK1,REMARK2,FROMFLAG,ACCODE,CTRANCODE,DUEDATE"
                    strsql += vbCrLf + " ,APPVER,COSTID,COMPANYID,TRANSFERED,FLAG,PAYMODE,TRANFLAG,SETTLED,SETTLEBATCHNO"
                    strsql += vbCrLf + " )"
                    strsql += vbCrLf + " SELECT '" & SNO & "'"
                    strsql += vbCrLf + "," & Val(.Item("TRANNO").ToString) & "" 'TRANNO
                    strsql += vbCrLf + ",'" & DateTime.Now.ToString("yyyy-MM-dd") & "'" ' TRANDATE
                    strsql += vbCrLf + ",'" & .Item("TRANTYPE").ToString & "'" 'TRANTYPE
                    strsql += vbCrLf + ",'" & .Item("RUNNO").ToString & "'" 'RUNNO
                    strsql += vbCrLf + "," & Val(.Item("AMOUNT").ToString) & "" 'AMOUNT
                    strsql += vbCrLf + "," & Val(.Item("GRSWT").ToString) & "" ' GRSWT
                    strsql += vbCrLf + "," & Val(.Item("NETWT").ToString) & "" ' NETWT
                    strsql += vbCrLf + "," & Val(.Item("PUREWT").ToString) & "" 'PUREWT
                    strsql += vbCrLf + ",'P'" ' RECPAY
                    strsql += vbCrLf + ",'" & .Item("REFNO").ToString & "'" 'REFNO"
                    strsql += vbCrLf + " ,'" & .Item("REFDATE").ToString & "'" 'REFDATE
                    strsql += vbCrLf + " ," & Val(.Item("EMPID").ToString) & "" 'EMPID
                    strsql += vbCrLf + " ,'" & .Item("TRANSTATUS").ToString & "'" 'TRANSTATUS
                    strsql += vbCrLf + "," & Val(.Item("PURITY").ToString) & "" ' PURITY
                    strsql += vbCrLf + ",'" & .Item("CATCODE").ToString & "'" 'CATCODE
                    strsql += vbCrLf + ",'" & .Item("BATCHNO").ToString & "'" 'BATCHNO
                    strsql += vbCrLf + "," & .Item("USERID").ToString & "" 'USERID
                    strsql += vbCrLf + ",'" & DateTime.Now.ToString("yyyy-MM-dd") & "'" 'UPDATED
                    strsql += vbCrLf + ",'" & Date.Now.ToLongTimeString & "'" 'UPTIME"
                    strsql += vbCrLf + ",'" & systemId & "'" 'SYSTEMID"
                    strsql += vbCrLf + "," & Val(.Item("RATE").ToString) & "" 'RATE"
                    strsql += vbCrLf + " ," & Val(.Item("VALUE").ToString) & "" 'VALUE"
                    strsql += vbCrLf + ",'" & .Item("CANCEL").ToString & "'" 'CANCEL"
                    strsql += vbCrLf + ",'" & .Item("CASHID").ToString & "'" 'CASHID"
                    strsql += vbCrLf + ",'" & .Item("VATEXM").ToString & "'" 'VATEXM"
                    strsql += vbCrLf + ",'" & .Item("REMARK1").ToString & "'" 'REMARK1"
                    strsql += vbCrLf + ",'" & .Item("REMARK2").ToString & "'" 'REMARK2"
                    strsql += vbCrLf + ",'" & .Item("FROMFLAG").ToString & "'" 'FROMFLAG"
                    strsql += vbCrLf + ",'" & .Item("ACCODE").ToString & "'" 'ACCODE"
                    strsql += vbCrLf + ",'" & .Item("CTRANCODE").ToString & "'" 'CTRANCODE"
                    strsql += vbCrLf + ",'" & .Item("DUEDATE").ToString & "'" 'DUEDATE"
                    strsql += vbCrLf + ",'" & .Item("APPVER").ToString & "'" 'APPVER"
                    strsql += vbCrLf + ",'" & .Item("COSTID").ToString & "'" 'COSTID"
                    strsql += vbCrLf + ",'" & .Item("COMPANYID").ToString & "'" 'COMPANYID"
                    strsql += vbCrLf + ",'" & .Item("TRANSFERED").ToString & "'" 'TRANSFERED"
                    strsql += vbCrLf + ",'" & .Item("FLAG").ToString & "'" 'FLAG"
                    strsql += vbCrLf + ",'" & .Item("PAYMODE").ToString & "'" 'PAYMODE"
                    strsql += vbCrLf + ",'" & .Item("TRANFLAG").ToString & "'" 'TRANFLAG"
                    strsql += vbCrLf + ",'C' SETTLED"
                    strsql += vbCrLf + ",'" & .Item("SETTLEBATCHNO").ToString & "'" 'SETTLEBATCHNO"
                    ExecQuery(SyncMode.Transaction, strsql, cn, tran, CostId)


                    Dim NewSNO As String = GetNewSno(TranSnoType.OUTSTANDINGCODE, tran, "GET_ADMINSNO_TRAN")

                    strsql = vbCrLf + " INSERT INTO " & cnAdminDb & "..OUTSTANDING (SNO,TRANNO,TRANDATE,TRANTYPE,RUNNO,AMOUNT,GRSWT,NETWT,PUREWT,RECPAY,REFNO"
                    strsql += vbCrLf + " ,REFDATE,EMPID,TRANSTATUS,PURITY,CATCODE,BATCHNO,USERID,UPDATED,UPTIME,SYSTEMID,RATE"
                    strsql += vbCrLf + " ,VALUE,CANCEL,CASHID,VATEXM,REMARK1,REMARK2,FROMFLAG,ACCODE,CTRANCODE,DUEDATE"
                    strsql += vbCrLf + " ,APPVER,COSTID,COMPANYID,TRANSFERED,FLAG,PAYMODE,TRANFLAG,SETTLED,SETTLEBATCHNO"
                    strsql += vbCrLf + " )"
                    strsql += vbCrLf + " SELECT '" & NewSNO & "'"
                    strsql += vbCrLf + "," & Val(.Item("TRANNO").ToString) & "" 'TRANNO
                    strsql += vbCrLf + ",'" & DateTime.Now.ToString("yyyy-MM-dd") & "'" ' TRANDATE
                    strsql += vbCrLf + ",'" & .Item("TRANTYPE").ToString & "'" 'TRANTYPE
                    strsql += vbCrLf + ",'" & .Item("RUNNO").ToString & "'" 'RUNNO
                    If rbtPurity.Checked = True Then
                        strsql += vbCrLf + "," & Val(.Item("AMOUNT").ToString) & "" 'AMOUNT
                        strsql += vbCrLf + "," & Val(Format(curgrswt, "0.000")) & "" ' GRSWT
                        strsql += vbCrLf + "," & Val(Format(curnetwt, "0.000")) & "" ' NETWT
                        strsql += vbCrLf + "," & Val(.Item("PUREWT").ToString) & "" 'PUREWT
                    Else
                        strsql += vbCrLf + "," & Val(txtconvAmt_AMT.Text) & "" 'AMOUNT
                        strsql += vbCrLf + ",0" ' GRSWT
                        strsql += vbCrLf + ",0" ' NETWT
                        strsql += vbCrLf + ",0" 'PUREWT
                    End If
                    strsql += vbCrLf + ",'R'" ' RECPAY
                    strsql += vbCrLf + ",'" & .Item("REFNO").ToString & "'" 'REFNO"
                    strsql += vbCrLf + " ,'" & .Item("REFDATE").ToString & "'" 'REFDATE
                    strsql += vbCrLf + " ," & Val(.Item("EMPID").ToString) & "" 'EMPID
                    strsql += vbCrLf + " ,'" & .Item("TRANSTATUS").ToString & "'" 'TRANSTATUS
                    strsql += vbCrLf + "," & Val(cmbPurity_OWN.Text.ToString) & "" ' PURITY
                    strsql += vbCrLf + ",'" & .Item("CATCODE").ToString & "'" 'CATCODE
                    strsql += vbCrLf + ",'" & .Item("BATCHNO").ToString & "'" 'BATCHNO
                    strsql += vbCrLf + "," & .Item("USERID").ToString & "" 'USERID
                    strsql += vbCrLf + ",'" & DateTime.Now.ToString("yyyy-MM-dd") & "'" 'UPDATED
                    strsql += vbCrLf + ",'" & Date.Now.ToLongTimeString & "'" 'UPTIME"
                    strsql += vbCrLf + ",'" & systemId & "'" 'SYSTEMID"
                    strsql += vbCrLf + "," & Val(.Item("RATE").ToString) & "" 'RATE"
                    strsql += vbCrLf + " ," & Val(.Item("VALUE").ToString) & "" 'VALUE"
                    strsql += vbCrLf + ",'" & .Item("CANCEL").ToString & "'" 'CANCEL"
                    strsql += vbCrLf + ",'" & .Item("CASHID").ToString & "'" 'CASHID"
                    strsql += vbCrLf + ",'" & .Item("VATEXM").ToString & "'" 'VATEXM"
                    strsql += vbCrLf + ",'" & .Item("REMARK1").ToString & "'" 'REMARK1"
                    strsql += vbCrLf + ",'" & .Item("REMARK2").ToString & "'" 'REMARK2"
                    strsql += vbCrLf + ",'" & .Item("FROMFLAG").ToString & "'" 'FROMFLAG"
                    strsql += vbCrLf + ",'" & .Item("ACCODE").ToString & "'" 'ACCODE"
                    strsql += vbCrLf + ",'" & .Item("CTRANCODE").ToString & "'" 'CTRANCODE"
                    strsql += vbCrLf + ",'" & .Item("DUEDATE").ToString & "'" 'DUEDATE"
                    strsql += vbCrLf + ",'" & .Item("APPVER").ToString & "'" 'APPVER"
                    strsql += vbCrLf + ",'" & .Item("COSTID").ToString & "'" 'COSTID"
                    strsql += vbCrLf + ",'" & .Item("COMPANYID").ToString & "'" 'COMPANYID"
                    strsql += vbCrLf + ",'" & .Item("TRANSFERED").ToString & "'" 'TRANSFERED"
                    strsql += vbCrLf + ",'" & .Item("FLAG").ToString & "'" 'FLAG"
                    strsql += vbCrLf + ",'" & .Item("PAYMODE").ToString & "'" 'PAYMODE"
                    strsql += vbCrLf + ",'" & .Item("TRANFLAG").ToString & "'" 'TRANFLAG"
                    strsql += vbCrLf + ",''" 'SETTLED"
                    strsql += vbCrLf + ",''" 'SETTLEBATCHNO"

                    ExecQuery(SyncMode.Transaction, strsql, cn, tran, CostId)
                End With

                tran.Commit()
                tran = Nothing
                MsgBox("Purity/Amount converted Successfully.")
                funcnew()
            End If

        Catch ex As Exception
            tran.Rollback()
            tran.Dispose()
            tran = Nothing
            MsgBox(ex.Message)
        End Try
    End Sub

    Function puritycalc()
        curpurity = 0 : curgrswt = 0 : curnetwt = 0
        curpurity = Val(txtadvancepurity_WET.Text.ToString) - Val(cmbPurity_OWN.Text.ToString)
        Dim netwt = ((Val(txtadvancewt_WET.Text.ToString) * curpurity) / 100)
        Dim grswt = ((Val(recgrswt) * curpurity) / 100)
        If curpurity <= Val(txtadvancepurity_WET.Text.ToString) Then
            curnetwt = Val(txtadvancewt_WET.Text.ToString) + netwt
            curgrswt = Val(recgrswt) + grswt
        Else
            curnetwt = Val(txtadvancewt_WET.Text.ToString) - netwt
            curgrswt = Val(recgrswt) - grswt
        End If
    End Function
#End Region

    Private Sub cmbPurity_OWN_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbPurity_OWN.Leave
        If cmbPurity_OWN.Text <> "" Then
            puritycalc()
            txtconvtwt_WET.Text = Format(curnetwt, "0.000")
        End If
    End Sub

    Private Sub cmbPurity_OWN_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbPurity_OWN.SelectedIndexChanged
        If cmbPurity_OWN.Text <> "" Then
            puritycalc()
            txtconvtwt_WET.Text = Format(curnetwt, "0.000")
        End If
    End Sub


    Private Sub rbtPurity_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtPurity.CheckedChanged
        If rbtPurity.Checked = True Then
            cmbPurity_OWN.Enabled = True : txtconvtwt_WET.Enabled = True
            txtConvRate_AMT.Enabled = False : txtconvAmt_AMT.Enabled = False
            cmbPurity_OWN.Focus()
        End If
    End Sub

    
    Private Sub rbtAmount_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtAmount.CheckedChanged
        If rbtAmount.Checked = True Then
            cmbPurity_OWN.Enabled = False : txtconvtwt_WET.Enabled = False
            txtconvtwt_WET.Text = ""
            txtConvRate_AMT.Enabled = True : txtconvAmt_AMT.Enabled = True
            txtConvRate_AMT.Focus()
        End If
    End Sub

    Private Sub txtConvRate_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtConvRate_AMT.TextChanged
        If Val(txtadvancewt_WET.Text) <> 0 Then
            txtconvAmt_AMT.Text = Val(txtadvancewt_WET.Text) * Val(txtConvRate_AMT.Text)
        End If
    End Sub
End Class