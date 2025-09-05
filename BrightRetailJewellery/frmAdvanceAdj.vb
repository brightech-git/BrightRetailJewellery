Imports System.Data.OleDb
Public Class frmAdvanceAdj
    Public dtGridAdvance As New DataTable
    Dim cmd As OleDbCommand
    Dim strSql As String
    Public withOrderDetail As Boolean = False
    Public withRepairDetail As Boolean = False
    Public REFNOPREFIXAUTO As String = ""
    Public BILLDATE As Date = Nothing
    Public BillCostId As String
    Dim RunNoCheck As Boolean = IIf(GetAdmindbSoftValue("CHECK_RUNNO", "Y") = "Y", True, False)
    Dim ADVADJDAYS As Integer = GetAdmindbSoftValue("CHECK_ADVADJDAYS", 0)
    Dim IsOrder_OrdadvOnly As Boolean = IIf(GetAdmindbSoftValue("EST_ORDADV_ADV", "N") = "Y", True, False)
    Dim WTADV2AMTCURRRATE As Boolean = IIf(GetAdmindbSoftValue("WTADV2AMTCURRRATE", "N") = "Y", True, False)
    Dim ISMAINT_CENTADV As Boolean = IIf(GetAdmindbSoftValue("MAINT_CENTADV", "N") = "Y", True, False)
    Dim OTHBRANCH_ADVADJ_AUT As Boolean = IIf(GetAdmindbSoftValue("OTHBRANCH_ADVADJ_AUT", "Y") = "Y", True, False)
    Dim IS_USERLEVELPWD As Boolean = IIf(GetAdmindbSoftValue("USERLEVELPWD", "N") = "Y", True, False)
    Dim ADVREMATERT As Boolean = True
    Public EnableSchemeoffer As Boolean = IIf(GetAdmindbSoftValue("ESTSCHOFFER_ENABLE", "N") = "N", False, True)
    Public mRemark As String = ""
    Dim OrgRunno As String = ""
    Dim ORGBATCHNO As String = ""
    Public IsreservedTag_unlock As Boolean = False
    Public Ordernos As String = ""
    Dim AdvCostid As String = BillCostId
    Dim RunNoChecked As Boolean = False
    Dim AuthPwdPass As Boolean = False
    Dim GstPer As Decimal
    Dim GstFlag As Boolean
    Dim GSTADVCALC_INCL As String = GetAdmindbSoftValue("GSTADVCALC", "N")
    Dim Round_Gst As String = GetAdmindbSoftValue("ROUNDOFF-GST", "F")
    Dim AdjDelearPurchase As Boolean = IIf(GetAdmindbSoftValue("ADJDELEARPURCHASE", "N") = "Y", True, False)
    Dim ADVcustomerotp As Boolean = IIf(GetAdmindbSoftValue("ADVADJ_OTP", "O") = "C", True, False)

    Public Sub New(ByVal BillCostId As String)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        objGPack.TextClear(Me)
        objGPack.Validator_Object(Me)
        gridAdvance.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        gridAdvanceTotal.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        ' Add any initialization after the InitializeComponent() call.
        ''Advance
        Me.BillCostId = BillCostId
        dtpAdvanceDate.MinimumDate = (New DateTimePicker).MinDate
        dtpAdvanceDate.MaximumDate = (New DateTimePicker).MaxDate
        With dtGridAdvance
            .Columns.Add("COSTID", GetType(String))
            .Columns.Add("RUNNO", GetType(String))
            .Columns.Add("DATE", GetType(Date))
            .Columns.Add("RECEIVEDAMT", GetType(Double))
            .Columns.Add("ADJUSTEDAMT", GetType(Double))
            .Columns.Add("AMOUNT", GetType(Double))
            .Columns.Add("PNAME", GetType(String))
            .Columns.Add("ADDRESS1", GetType(String))
            .Columns.Add("ADDRESS2", GetType(String))
            .Columns.Add("ADDRESS3", GetType(String))
            .Columns.Add("REFNO", GetType(String))
            .Columns.Add("ACCODE", GetType(String))
            .Columns.Add("ISEDIT", GetType(Char))
            .Columns.Add("COMPANYID", GetType(String))
            .Columns.Add("ADVFIXWTPER", GetType(Double))
            .Columns.Add("REMARK", GetType(String))
            .Columns.Add("ORGRUNNO", GetType(String))
            .Columns.Add("ORGBATCHNO", GetType(String))
            .Columns.Add("BALAMOUNT", GetType(Double))
            .Columns.Add("GST", GetType(Double))

        End With
        gridAdvance.DataSource = dtGridAdvance
        FormatGridColumns(gridAdvance)
        StyleGridAdvance(gridAdvance)
        Dim dtGridAdvanceTotal As New DataTable
        dtGridAdvanceTotal = dtGridAdvance.Copy
        gridAdvanceTotal.DataSource = dtGridAdvanceTotal
        dtGridAdvanceTotal.Rows.Clear()
        dtGridAdvanceTotal.Rows.Add()
        dtGridAdvanceTotal.Rows(0).Item("RUNNO") = "Total"
        With gridAdvanceTotal
            .DataSource = dtGridAdvanceTotal
            For Each col As DataGridViewColumn In gridAdvance.Columns
                With gridAdvanceTotal.Columns(col.Name)
                    .Visible = col.Visible
                    .Width = col.Width
                    .DefaultCellStyle = col.DefaultCellStyle
                End With
            Next
        End With
        FormatGridColumns(gridAdvanceTotal)
        StyleGridAdvance(gridAdvanceTotal)
        txtAdvCostid.Text = BillCostId
        If Not ISMAINT_CENTADV Then
            txtAdvCostid.Visible = False : lblAdvCostid.Visible = False
            txtAdvanceNo.Left -= txtAdvCostid.Width : dtpAdvanceDate.Left -= txtAdvCostid.Width : txtAdvanceAdjusted_AMT.Left -= txtAdvCostid.Width : txtAdvanceBalance_AMT.Left -= txtAdvCostid.Width : txtAdvanceReceived_AMT.Left -= txtAdvCostid.Width
            txtAdvanceGST_AMT.Left -= txtAdvCostid.Width
            txtAdvanceAmt_AMT.Left -= txtAdvCostid.Width
            lblAdvNo.Left = txtAdvanceNo.Left : lblAdvdate.Left = dtpAdvanceDate.Left : lblAdvRecd.Left = txtAdvanceReceived_AMT.Left : lblAdvAdj.Left = txtAdvanceAdjusted_AMT.Left : lblAdvBal.Left = txtAdvanceBalance_AMT.Left
            lblAdvGst.Left = txtAdvanceGST_AMT.Left
            lblAdvAmt.Left = txtAdvanceAmt_AMT.Left
            PnlAddr.Left -= txtAdvCostid.Width
            gridAdvance.Size = New Size(gridAdvance.Width - (txtAdvCostid.Width), gridAdvance.Height)
            gridAdvanceTotal.Size = New Size(gridAdvanceTotal.Width - (txtAdvCostid.Width), gridAdvanceTotal.Height)
        End If
        PnlAddr.BackColor = Color.Lavender
        GstPer = Val(objGPack.GetSqlValue("SELECT STAX FROM " & cnAdminDb & "..TAXMAST WHERE TAXCODE='GT'", , "0").ToString)
        txtAdvanceNo.Focus()
    End Sub

    Public Sub StyleGridAdvance(ByVal grid As DataGridView)
        With grid
            If ISMAINT_CENTADV Then .Columns("COSTID").Width = txtAdvCostid.Width + 1 Else .Columns("COSTID").Visible = False
            .Columns("RUNNO").Width = txtAdvanceNo.Width + 1
            .Columns("DATE").Width = dtpAdvanceDate.Width + 1
            .Columns("RECEIVEDAMT").Width = txtAdvanceReceived_AMT.Width + 1
            .Columns("ADJUSTEDAMT").Width = txtAdvanceAdjusted_AMT.Width + 1
            .Columns("BALAMOUNT").Width = txtAdvanceAmt_AMT.Width + 1
            .Columns("AMOUNT").Width = txtAdvanceBalance_AMT.Width + 1
            .Columns("GST").Width = txtAdvanceGST_AMT.Width + 1
            For cnt As Integer = 6 To grid.ColumnCount - 1
                .Columns(cnt).Visible = False
            Next
            .Columns("GST").Visible = True
            .Columns("BALAMOUNT").Visible = True
        End With
    End Sub


    Private Sub txtAdvanceAdjusted_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAdvanceAdjusted_AMT.GotFocus
        SendKeys.Send("{TAB}")
    End Sub

    Private Sub txtAdvanceBalance_AMT_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtAdvanceBalance_AMT.KeyDown
        If e.KeyCode = Keys.Down Then
            If gridAdvance.RowCount > 0 Then gridAdvance.Select()
        End If
    End Sub
    Private Sub txtAdvanceBalance_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtAdvanceBalance_AMT.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtAdvanceNo.Text = "" Then
                MsgBox(Me.GetNextControl(txtAdvanceNo, False).Text, MsgBoxStyle.Information)
                txtAdvanceNo.Focus()
                Exit Sub
            End If
            If Val(txtAdvanceBalance_AMT.Text) = 0 Then
                Exit Sub
            End If


            strSql = " IF (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "_CUSTOMERINFO') > 0"
            strSql += " DROP TABLE TEMPTABLEDB..TEMP" & systemId & "_CUSTOMERINFO"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = " SELECT C.PSNO as SNO,C.BATCHNO,TRANDATE,PREVILEGEID,ACCODE,TITLE"
            strSql += " ,INITIAL,PNAME,DOORNO,ADDRESS1,ADDRESS2,ADDRESS3,AREA,CITY,STATE,COUNTRY"
            strSql += " ,PINCODE,PHONERES,MOBILE,EMAIL,FAX,C.REMARK1"
            strSql += " INTO TEMPTABLEDB..TEMP" & systemId & "_CUSTOMERINFO "
            strSql += " FROM " & cnAdminDb & "..PERSONALINFO P "
            strSql += " INNER JOIN " & cnAdminDb & "..CUSTOMERINFO C ON P.SNO = C.PSNO"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = " SELECT SUBSTRING(RUNNO,6,20) REFNO"
            If ISMAINT_CENTADV Then strSql += ", SUBSTRING(RUNNO,1,2) AS COSTID"
            strSql += " ,CASE WHEN RECEIPT <> 0 THEN RECEIPT ELSE NULL END AS RECEIPT"
            strSql += " ,CASE WHEN PAYMENT <> 0 THEN PAYMENT ELSE NULL END AS PAYMENT"
            strSql += " ,CASE WHEN BALANCE <> 0 THEN BALANCE ELSE NULL END AS BALANCE"
            strSql += " ,x.TRANDATE,V.PREVILEGEID,V.ACCODE ACCODE,X.ACCODE ADVCODE,V.TITLE,V.INITIAL,V.PNAME,V.DOORNO,V.ADDRESS1,V.ADDRESS2,V.ADDRESS3"
            strSql += " ,V.AREA,V.CITY,V.STATE,V.COUNTRY,V.PINCODE,V.PHONERES,V.MOBILE,V.EMAIL,V.FAX,V.SNO,X.COMPANYID,V.REMARK1"
            strSql += " ,RUNNO AS ORGRUNNO,X.BATCHNO AS ORGBATCHNO  FROM"
            strSql += " ("
            strSql += " SELECT RUNNO"
            strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT+ISNULL(GSTVAL,0) ELSE 0 END)AS RECEIPT"
            strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT+ISNULL(GSTVAL,0) ELSE 0 END)AS PAYMENT"
            strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT+ISNULL(GSTVAL,0) ELSE -1*(AMOUNT+ISNULL(GSTVAL,0)) END)AS BALANCE"
            strSql += " ,(SELECT TOP 1 TRANDATE FROM " & cnAdminDb & "..OUTSTANDING WHERE " & IIf(ISMAINT_CENTADV, "", "COSTID = '" & BillCostId & "' AND ") & " RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  AND ISNULL(CANCEL,'') = '' ORDER BY TRANDATE,SNO DESC)AS TRANDATE"
            strSql += " ,(SELECT TOP 1 BATCHNO FROM " & cnAdminDb & "..OUTSTANDING WHERE " & IIf(ISMAINT_CENTADV, "", "COSTID = '" & BillCostId & "' AND ") & " RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  AND ISNULL(CANCEL,'') = '' ORDER BY TRANDATE,SNO DESC)AS BATCHNO"
            strSql += " ,O.ACCODE ACCODE,O.COMPANYID"
            strSql += " FROM " & cnAdminDb & "..OUTSTANDING O"
            strSql += " WHERE (RUNNO = '" & GetCostId(BillCostId) & GetCompanyId(strCompanyId) & txtAdvanceNo.Text & "' OR (TRANSTATUS = 'T' AND RUNNO LIKE '%" & GetCompanyId(strCompanyId) & txtAdvanceNo.Text & "'))" ' RUNNO = '" & GetCostId(BillCostId) & GetCompanyId(strCompanyId) & txtAdvanceNo.Text & "'"
            strSql += " AND TRANTYPE = 'A' "
            If Not withOrderDetail Then strSql += " AND SUBSTRING(RUNNO,6,20) NOT LIKE 'O%'"
            If Not withRepairDetail Then strSql += " AND SUBSTRING(RUNNO,6,20) NOT LIKE 'R%'"
            strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
            If Not ISMAINT_CENTADV Then strSql += vbCrLf + " AND COSTID = '" & BillCostId & "'"
            strSql += " AND ISNULL(CANCEL,'') = ''"
            strSql += " GROUP BY RUNNO,COMPANYID,O.ACCODE"
            strSql += " )X"
            strSql += " INNER JOIN TEMPTABLEDB..TEMP" & systemId & "_CUSTOMERINFO AS V ON V.BATCHNO = X.BATCHNO"
            Dim dt As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If Not dt.Rows.Count > 0 Then
                If RunNoCheck = False And RunNoChecked = False Then
                    If RunNoChecked = True Then Exit Sub
                    If MessageBox.Show("Runno Info Not Found. Do you wish to continue?", "Invalid Runno Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
                        txtAdvanceNo.Focus()
                        Exit Sub
                    Else
                        RunNoChecked = True
                    End If
                End If
            End If


            For Each roAdv As DataRow In dtGridAdvance.Rows
                If txtAdvanceRowIndex.Text <> "" Then Exit For
                If txtAdvanceNo.Text = roAdv!RUNNO.ToString Then
                    MsgBox("Already Exist in this Advance No", MsgBoxStyle.Information)
                    txtAdvanceNo.Select()
                    txtAdvanceNo.SelectAll()
                    Exit Sub
                End If
            Next
            If RunNoCheck = True Then
                If txtAdvanceNo.Text <> txtAdvanceEntAdvanceNo.Text Then
                    MsgBox("Invalid Advance No", MsgBoxStyle.Information)
                    txtAdvanceNo.Select()
                    Exit Sub
                End If
                If Math.Round(Val(txtAdvanceBalance_AMT.Text), 2) > Math.Round(Val(txtAdvanceReceived_AMT.Text) - Val(txtAdvanceAdjusted_AMT.Text), 2) Then

                    MsgBox("Advance Balance " + txtAdvanceEntAmount.Text + " Only", MsgBoxStyle.Information)
                    txtAdvanceBalance_AMT.Select()
                    Exit Sub
                End If
            End If
            If ORGBATCHNO <> "" Then
                strSql = " select count(*) from " & cnAdminDb & "..ITEMTAG WHERE APPROVAL = 'R' AND BATCHNO= '" & ORGBATCHNO & "'"
                Dim mcntTag As Integer = Val(objGPack.GetSqlValue(strSql).ToString)
                If mcntTag > 0 Then
                    If MsgBox(mcntTag.ToString & " Tags are Reserved." & vbCrLf & "Can you unlock for sales", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then IsreservedTag_unlock = True Else IsreservedTag_unlock = False
                End If
            End If
            If CheckAuth() Then Exit Sub
            AuthPwdPass = False
            If txtAdvanceRowIndex.Text = "" Then
                Dim ro As DataRow = Nothing
                ro = dtGridAdvance.NewRow
                ro("RUNNO") = txtAdvanceNo.Text
                ro("DATE") = dtpAdvanceDate.Value.Date.ToString("yyyy-MM-dd")
                ro("RECEIVEDAMT") = IIf(Val(txtAdvanceReceived_AMT.Text) <> 0, Val(txtAdvanceReceived_AMT.Text), DBNull.Value)
                ro("ADJUSTEDAMT") = IIf(Val(txtAdvanceAdjusted_AMT.Text) <> 0, Val(txtAdvanceAdjusted_AMT.Text), DBNull.Value)
                ro("AMOUNT") = IIf(Val(txtAdvanceBalance_AMT.Text) <> 0, Val(txtAdvanceBalance_AMT.Text), DBNull.Value)
                ro("BALAMOUNT") = IIf(Val(txtAdvanceAmt_AMT.Text) <> 0, Val(txtAdvanceAmt_AMT.Text), DBNull.Value)
                ro("GST") = IIf(Val(txtAdvanceGST_AMT.Text) <> 0, Val(txtAdvanceGST_AMT.Text), DBNull.Value)
                ro("PNAME") = txtAdvanceName.Text
                ro("ADDRESS1") = txtAdvanceAddress1.Text
                ro("ADDRESS2") = txtAdvanceAddress2.Text
                ro("ADDRESS3") = txtAdvanceAddress3.Text
                ro("REFNO") = txtAdvanceRefNo.Text
                ro("ACCODE") = txtAdvanceAcCode.Text
                ro("COMPANYID") = txtAdvanceCompanyId.Text
                ro("ORGRUNNO") = OrgRunno
                ro("ORGBATCHNO") = IIf(IsreservedTag_unlock, ORGBATCHNO, "")
                ro("COSTID") = txtAdvCostid.Text

                dtGridAdvance.Rows.Add(ro)

            Else
                With gridAdvance.Rows(Val(txtAdvanceRowIndex.Text))
                    .Cells("RUNNO").Value = txtAdvanceNo.Text
                    .Cells("DATE").Value = dtpAdvanceDate.Value.Date.ToString("yyyy-MM-dd")
                    .Cells("RECEIVEDAMT").Value = IIf(Val(txtAdvanceReceived_AMT.Text) <> 0, Val(txtAdvanceReceived_AMT.Text), DBNull.Value)
                    .Cells("ADJUSTEDAMT").Value = IIf(Val(txtAdvanceAdjusted_AMT.Text) <> 0, Val(txtAdvanceAdjusted_AMT.Text), DBNull.Value)
                    .Cells("BALAMOUNT").Value = IIf(Val(txtAdvanceAmt_AMT.Text) <> 0, Val(txtAdvanceAmt_AMT.Text), DBNull.Value)
                    .Cells("AMOUNT").Value = IIf(Val(txtAdvanceBalance_AMT.Text) <> 0, Val(txtAdvanceBalance_AMT.Text), DBNull.Value)
                    .Cells("GST").Value = IIf(Val(txtAdvanceGST_AMT.Text) <> 0, Val(txtAdvanceGST_AMT.Text), DBNull.Value)
                    .Cells("PNAME").Value = txtAdvanceName.Text
                    .Cells("ADDRESS1").Value = txtAdvanceAddress1.Text
                    .Cells("ADDRESS2").Value = txtAdvanceAddress2.Text
                    .Cells("ADDRESS3").Value = txtAdvanceAddress3.Text
                    .Cells("REFNO").Value = txtAdvanceRefNo.Text
                    .Cells("ACCODE").Value = txtAdvanceAcCode.Text
                    .Cells("COMPANYID").Value = txtAdvanceCompanyId.Text
                    .Cells("ORGRUNNO").Value = OrgRunno
                    .Cells("ORGBATCHNO").Value = IIf(IsreservedTag_unlock, ORGBATCHNO, "")
                    .Cells("COSTID").Value = txtAdvCostid.Text
                End With

            End If
            dtGridAdvance.AcceptChanges()
            CalcGridAdvanceTotal()
            gridAdvance.CurrentCell = gridAdvance.Rows(gridAdvance.RowCount - 1).Cells("RUNNO")
            objGPack.TextClear(grpAdvance)
            txtAdvCostid.Text = BillCostId
            txtAdvanceNo.Text = "A" + Mid(Format(cnTranToDate, "dd/MM/yyyy"), 9, 2).ToString
            GstFlag = False
            txtAdvanceNo.Focus()
            txtAdvanceNo.SelectionStart = txtAdvanceNo.TextLength
        End If
    End Sub
    Public Sub CalcGridAdvanceTotal()
        gridAdvanceTotal.DefaultCellStyle.SelectionBackColor = grpAddress.BackgroundColor
        dtGridAdvance.AcceptChanges()
        Dim amt As Double = Nothing
        Dim Gstamt As Double = Nothing
        Dim Balamt As Double = Nothing
        For Each ro As DataRow In dtGridAdvance.Rows
            amt += Val(ro!AMOUNT.ToString)
            Balamt += Val(ro!BALAMOUNT.ToString)
            Gstamt += Val(ro!GST.ToString)
        Next
        gridAdvanceTotal.Rows(0).Cells("AMOUNT").Value = IIf(amt <> 0, amt, DBNull.Value)
        gridAdvanceTotal.Rows(0).Cells("BALAMOUNT").Value = IIf(Balamt <> 0, Balamt, DBNull.Value)
        gridAdvanceTotal.Rows(0).Cells("GST").Value = IIf(Gstamt <> 0, Gstamt, DBNull.Value)
    End Sub

    Private Sub txtAdvanceNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAdvanceNo.GotFocus
        Main.ShowHelpText("Press Insert Key to Help")
        If IsOrder_OrdadvOnly Then withOrderDetail = True
        If REFNOPREFIXAUTO = "Y" And txtAdvanceNo.Text.Trim = "" Then txtAdvanceNo.Text = "A" + Mid(Format(cnTranToDate, "dd/MM/yyyy"), 9, 2).ToString : SendKeys.Send("{END}")
    End Sub
    Private Sub txtAdvanceNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtAdvanceNo.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
            Exit Sub
        End If
        Dim objBilltype As frmCR
        objBilltype = New frmCR
        objBilltype.IsAdvance = True
        Dim billtype As String = "A"
        If e.KeyCode = Keys.Down Then
            If gridAdvance.RowCount > 0 Then gridAdvance.Select()
        ElseIf e.KeyCode = Keys.Insert Then
            If objBilltype.ShowDialog() = Windows.Forms.DialogResult.OK Then
                billtype = objBilltype.BILLTYPE
            End If
            strSql = " IF (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "_CUSTOMERINFO' AND XTYPE='U') > 0"
            strSql += vbCrLf + " DROP TABLE TEMPTABLEDB..TEMP" & systemId & "_CUSTOMERINFO"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            'strSql = " CREATE VIEW TEMP" & systemId & "_CUSTOMERINFO"
            'strSql += vbCrLf + " AS"
            'strSql += vbCrLf + " SELECT C.PSNO as SNO,C.BATCHNO,TRANDATE,PREVILEGEID,ACCODE,TITLE,INITIAL,PNAME,DOORNO,ADDRESS1,ADDRESS2,ADDRESS3,AREA,CITY,STATE,COUNTRY,PINCODE,PHONERES,MOBILE,EMAIL,FAX,C.REMARK1"
            'strSql += vbCrLf + " FROM " & cnAdminDb & "..PERSONALINFO P INNER JOIN " & cnAdminDb & "..CUSTOMERINFO C ON P.SNO = C.PSNO"
            'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            'cmd.ExecuteNonQuery()
            strSql = vbCrLf + " SELECT C.PSNO as SNO,C.BATCHNO,TRANDATE,PREVILEGEID"
            strSql += vbCrLf + " ,ACCODE,TITLE,INITIAL,PNAME,DOORNO,ADDRESS1,ADDRESS2,ADDRESS3"
            strSql += vbCrLf + " ,AREA,CITY,STATE,COUNTRY,PINCODE,PHONERES,MOBILE,EMAIL,FAX,C.REMARK1"
            strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "_CUSTOMERINFO"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..PERSONALINFO P "
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CUSTOMERINFO C ON P.SNO = C.PSNO"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = " IF (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "_OUTST' AND XTYPE='U') > 0"
            strSql += vbCrLf + " DROP TABLE TEMPTABLEDB..TEMP" & systemId & "_OUTST"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()

            strSql = " SELECT RUNNO "
            strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE -1*AMOUNT END)AS BALANCE"
            strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "_OUTST "
            strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING "
            strSql += vbCrLf + " WHERE TRANTYPE IN('A','C'" & IIf(AdjDelearPurchase, ",'D'", "") & ") "
            strSql += vbCrLf + " AND ISNULL(CANCEL,'') = '' "
            strSql += vbCrLf + " GROUP BY RUNNO "
            strSql += vbCrLf + " HAVING SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE -1*AMOUNT END) > 0 "
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = " SELECT SUBSTRING(RUNNO,6,20) REFNO"
            strSql += vbCrLf + " ,CASE WHEN RECEIPT <> 0 THEN RECEIPT ELSE NULL END AS RECEIPT"
            strSql += vbCrLf + " ,CASE WHEN PAYMENT <> 0 THEN PAYMENT ELSE NULL END AS PAYMENT"
            strSql += vbCrLf + " ,CASE WHEN BALANCE <> 0 THEN BALANCE ELSE NULL END AS BALANCE"
            strSql += vbCrLf + " ,V.TITLE,V.INITIAL,V.PNAME,V.DOORNO,V.ADDRESS1,V.ADDRESS2,V.ADDRESS3"
            strSql += vbCrLf + " ,V.AREA,V.CITY,V.STATE,V.COUNTRY,V.PINCODE,V.PHONERES,V.MOBILE,X.TRANDATE,V.PREVILEGEID,V.ACCODE ACCODE,X.ACCODE ADVCODE,V.EMAIL,V.FAX,V.SNO,X.COMPANYID,V.REMARK1,TRANFLAG"
            strSql += vbCrLf + " ,RUNNO ORGRUNNO,X.BATCHNO ORGBATCHNO,GST FROM"
            strSql += vbCrLf + " ("
            strSql += vbCrLf + " SELECT RUNNO"
            strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT+ISNULL(GSTVAL,0) ELSE 0 END)AS RECEIPT"
            strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT+ISNULL(GSTVAL,0) ELSE 0 END)AS PAYMENT"
            strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT+ISNULL(GSTVAL,0) ELSE -1*(AMOUNT+ISNULL(GSTVAL,0)) END)AS BALANCE"
            strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN GSTVAL ELSE -1*GSTVAL END)AS GST"
            strSql += vbCrLf + " ,(SELECT TOP 1 TRANDATE FROM " & cnAdminDb & "..OUTSTANDING WHERE COSTID = '" & IIf(txtAdvCostid.Text <> "", txtAdvCostid.Text, BillCostId) & "' AND RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  AND ISNULL(CANCEL,'') = '' ORDER BY TRANDATE DESC)AS TRANDATE"
            strSql += vbCrLf + " ,(SELECT TOP 1 BATCHNO FROM " & cnAdminDb & "..OUTSTANDING WHERE COSTID = '" & IIf(txtAdvCostid.Text <> "", txtAdvCostid.Text, BillCostId) & "' AND RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  AND ISNULL(CANCEL,'') = '' ORDER BY TRANDATE DESC)AS BATCHNO"
            strSql += vbCrLf + " ,O.ACCODE ACCODE,O.COMPANYID ,ISNULL(TRANFLAG,'') AS TRANFLAG"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING O"
            If billtype = "A" Then
                strSql += vbCrLf + " WHERE TRANTYPE = 'A'"
                If Not withOrderDetail Then strSql += vbCrLf + " AND SUBSTRING(RUNNO,6,20) NOT LIKE 'O%'"
                If Not withRepairDetail Then strSql += vbCrLf + " AND SUBSTRING(RUNNO,6,20) NOT LIKE 'R%'"
                strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
                strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
                'strSql += vbCrLf + " AND COSTID = '" & BillCostId & "'"
                strSql += vbCrLf + " AND isnull(COSTID,'') = '" & IIf(txtAdvCostid.Text <> "", txtAdvCostid.Text, BillCostId) & "'"
                If IsOrder_OrdadvOnly Then
                    If Ordernos.ToString <> "" Then strSql += vbCrLf + " AND Runno in('" & Replace(Ordernos, ",", "','") & "')"
                End If
            Else
                If AdjDelearPurchase = False Then
                    strSql += vbCrLf + " WHERE TRANTYPE ='C'"
                    strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
                    strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
                    strSql += vbCrLf + " AND ISNULL(COSTID,'') = '" & IIf(txtAdvCostid.Text <> "", txtAdvCostid.Text, BillCostId) & "'"
                    strSql += vbCrLf + " AND PAYMODE IN('RP','RA')"
                Else
                    strSql += vbCrLf + " WHERE TRANTYPE IN ('C','D','A')"
                    strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
                    strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
                    strSql += vbCrLf + " AND ISNULL(COSTID,'') = '" & IIf(txtAdvCostid.Text <> "", txtAdvCostid.Text, BillCostId) & "'"
                    strSql += vbCrLf + " AND PAYMODE IN('RP','RA','DU','AA')"
                End If
            End If
            strSql += vbCrLf + " AND RUNNO IN(SELECT RUNNO FROM TEMPTABLEDB..TEMP" & systemId & "_OUTST)"
            strSql += vbCrLf + " GROUP BY ISNULL(TRANFLAG,'') ,RUNNO,COMPANYID,ACCODE"
            strSql += vbCrLf + " HAVING SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE -1*AMOUNT END) > 0 "
            strSql += vbCrLf + " )X"
            strSql += vbCrLf + " INNER JOIN TEMPTABLEDB..TEMP" & systemId & "_CUSTOMERINFO AS V ON V.BATCHNO = X.BATCHNO"
            Dim row As DataRow = Nothing
            row = BrighttechPack.SearchDialog.Show_R("Search Advance Reference No", strSql, cn, 6, , , , , , , False)
            If Not row Is Nothing Then
                With row
                    For Each roAdv As DataRow In dtGridAdvance.Rows
                        If .Item("REFNO").ToString = roAdv!RUNNO.ToString Then
                            MsgBox("Already Exist in this Advance No", MsgBoxStyle.Information)
                            txtAdvanceNo.Select()
                            txtAdvanceNo.SelectAll()
                            Exit Sub
                        End If
                    Next
                    If .Item("TRANFLAG").ToString = "L" Then
                        MsgBox("This Advance No has been locked", MsgBoxStyle.Information)
                        txtAdvanceNo.Select()
                        txtAdvanceNo.SelectAll()
                        Exit Sub
                    End If


                    ''''If ADVADJDAYS <> 0 Then   '''commented on 04-12-2019 for advance adjustment otp purpose
                    ''''    If DateDiff(DateInterval.Day, dtpAdvanceDate.Value, BILLDATE) > ADVADJDAYS Then
                    ''''        Dim pwdid As Integer = 0 : Dim pwdpass As Boolean = False
                    ''''        Dim Sqlqry As String = "Select Optionid from " & cnAdminDb & "..PRJPWDOPTION where OPTIONNAME ='ADVADJ' AND active = 'Y'"
                    ''''        Dim Optionid As Integer = Val("" & objGPack.GetSqlValue(Sqlqry, , , tran))
                    ''''        If userId <> 999 And Optionid <> 0 Then
                    ''''            pwdid = GetuserPwd(Optionid, cnCostId, userId)
                    ''''            If pwdid <> 0 Then
                    ''''                Dim objUpwd As New frmUserPassword(pwdid, Optionid)
                    ''''                If objUpwd.ShowDialog <> Windows.Forms.DialogResult.OK Then MsgBox("Access Denied.") : Exit Sub Else pwdpass = True
                    ''''            Else
                    ''''                Dim objUpwd As New frmUserPassword(pwdid, Optionid)
                    ''''                If objUpwd.ShowDialog <> Windows.Forms.DialogResult.OK Then
                    ''''                    MsgBox("Access Denied.")
                    ''''                    Exit Sub
                    ''''                Else
                    ''''                    pwdpass = True
                    ''''                End If
                    ''''            End If
                    ''''        Else
                    ''''            pwdpass = True
                    ''''        End If
                    ''''        If pwdpass = False And txtAdvanceNo.Text <> "" Then
                    ''''            'If Authorize Then
                    ''''            '        Dim objSecret As New frmAdminPassword()
                    ''''            '        If objSecret.ShowDialog <> Windows.Forms.DialogResult.OK Then MsgBox("Access Denied.") : Exit Sub
                    ''''            '    Else
                    ''''            txtAdvanceNo.Text = ""
                    ''''            txtAdvanceAdjusted_AMT.Text = ""
                    ''''            txtAdvanceNo.Focus()
                    ''''            txtAdvanceBalance_AMT.Text = ""
                    ''''            txtAdvanceReceived_AMT.Text = ""
                    ''''            MsgBox("Access Denied.")
                    ''''            Exit Sub
                    ''''            '    End If
                    ''''        End If
                    ''''    End If
                    ''''End If

                    If ADVADJDAYS <> 0 Then

                        ''''''new added for customer otp
                        Dim advmobileno As String = ""
                        Dim sqlqryy As String = ""
                        If ADVcustomerotp Then
                            sqlqryy = " SELECT TOP 1  MOBILE FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO =( "
                            sqlqryy += " SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = ( "
                            sqlqryy += " SELECT DISTINCT BATCHNO FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO LIKE '%" & .Item("REFNO").ToString.Trim & "' "
                            sqlqryy += " AND TRANTYPE='A' AND RECPAY='R' AND COMPANYID='" & strCompanyId & "' "
                            If txtAdvCostid.Text <> "00" Then
                                sqlqryy += " And COSTID='" & txtAdvCostid.Text & "' "
                            End If
                            sqlqryy += " And ISNULL(CANCEL,'')='')) "
                            advmobileno = GetSqlValue(cn, sqlqryy)
                        Else
                            advmobileno = ""
                        End If
                        sqlqryy = ""
                        sqlqryy = " SELECT TOP 1 TRANDATE FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO LIKE '%" & .Item("REFNO").ToString.Trim & "' "
                        sqlqryy += " AND TRANTYPE='A' AND RECPAY='R' AND COMPANYID='" & strCompanyId & "' "
                        If txtAdvCostid.Text <> "00" Then
                            sqlqryy += " And COSTID='" & txtAdvCostid.Text & "' "
                        End If
                        sqlqryy += " And ISNULL(CANCEL,'')='' ORDER BY TRANDATE"
                        Dim ADVDATECHK As Date = Nothing
                        ADVDATECHK = GetSqlValue(cn, sqlqryy)

                        If ADVDATECHK.ToString.Length > 0 Then
                            dtpAdvanceDate.Value = ADVDATECHK.Date
                        End If

                        ''''''END new added for customer otp

                        If DateDiff(DateInterval.Day, dtpAdvanceDate.Value, BILLDATE) > ADVADJDAYS Then
                            Dim pwdid As Integer = 0 : Dim pwdpass As Boolean = False
                            Dim Sqlqry As String = "Select Optionid from " & cnAdminDb & "..PRJPWDOPTION where OPTIONNAME ='ADVADJ' AND active = 'Y'"
                            Dim Optionid As Integer = Val("" & objGPack.GetSqlValue(Sqlqry, , , tran))
                            If userId <> 999 And Optionid <> 0 Then
                                pwdid = GetuserPwd(Optionid, cnCostId, userId)
                                If pwdid <> 0 Then
                                    Dim objUpwd As New frmUserPassword(pwdid, Optionid, True, advmobileno.ToString.Trim)
                                    If objUpwd.ShowDialog <> Windows.Forms.DialogResult.OK Then MsgBox("Access Denied.") : Exit Sub Else pwdpass = True
                                Else
                                    Dim objUpwd As New frmUserPassword(pwdid, Optionid, True, advmobileno.ToString.Trim)
                                    If objUpwd.ShowDialog <> Windows.Forms.DialogResult.OK Then
                                        MsgBox("Access Denied.")
                                        Exit Sub
                                    Else
                                        pwdpass = True
                                    End If
                                End If
                            Else
                                pwdpass = True
                            End If
                            If pwdpass = False And txtAdvanceNo.Text <> "" Then
                                'If Authorize Then
                                '        Dim objSecret As New frmAdminPassword()
                                '        If objSecret.ShowDialog <> Windows.Forms.DialogResult.OK Then MsgBox("Access Denied.") : Exit Sub
                                '    Else
                                txtAdvanceNo.Text = ""
                                txtAdvanceAdjusted_AMT.Text = ""
                                txtAdvanceNo.Focus()
                                txtAdvanceBalance_AMT.Text = ""
                                txtAdvanceReceived_AMT.Text = ""
                                MsgBox("Access Denied.")
                                Exit Sub
                                '    End If
                            End If
                        End If
                    End If


                    Dim selectcostid As String = txtAdvCostid.Text
                    If selectcostid <> BillCostId And selectcostid <> "" And OTHBRANCH_ADVADJ_AUT Then
                        If MsgBox("Can you adjust Other Cost centre Advance ?" & vbCrLf & "If Yes, continue with OTP authendication only.", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                            If Not usrpwdokonly("OTHBRANCH_ADVADJ", IS_USERLEVELPWD) Then Exit Sub
                        Else
                            Exit Sub
                        End If
                    End If
                    dtpAdvanceDate.Value = .Item("TRANDATE")
                    txtAdvanceReceived_AMT.Text = IIf(Val(.Item("RECEIPT").ToString) <> 0, Format(Val(.Item("RECEIPT").ToString), "0.00"), Nothing)
                    txtAdvanceAdjusted_AMT.Text = IIf(Val(.Item("PAYMENT").ToString) <> 0, Format(Val(.Item("PAYMENT").ToString), "0.00"), Nothing)
                    txtAdvanceBalance_AMT.Text = IIf(Val(.Item("BALANCE").ToString) <> 0, Format(Val(.Item("BALANCE").ToString), "0.00"), Nothing)
                    If Val(.Item("GST").ToString) > 0 Then
                        GstFlag = True
                    End If
                    If GST And GstFlag Then

                        Dim Per As Decimal = 103
                        'If GSTADVCALC_INCL = "I" Then Per = 103
                        Dim GstAmt As Double
                        Dim SGstAmt As Double = (Val(txtAdvanceBalance_AMT.Text) * (GstPer / 2) / Per)
                        Dim CGstAmt As Double = (Val(txtAdvanceBalance_AMT.Text) * (GstPer / 2) / Per)
                        SGstAmt = Math.Round(SGstAmt, 2)
                        CGstAmt = Math.Round(CGstAmt, 2)
                        SGstAmt = CalcRoundoffAmt(SGstAmt, Round_Gst)
                        CGstAmt = CalcRoundoffAmt(CGstAmt, Round_Gst)

                        GstAmt = SGstAmt + CGstAmt
                        txtAdvanceGST_AMT.Text = IIf(GstAmt <> 0, Format(GstAmt, "0.00"), Nothing)
                        Dim TotAmt As Double = Val(txtAdvanceBalance_AMT.Text)
                        TotAmt -= Val(txtAdvanceGST_AMT.Text)
                        txtAdvanceAmt_AMT.Text = IIf(TotAmt <> 0, Format(TotAmt, "0.00"), Nothing)
                    End If
                    txtAdvanceName.Text = .Item("PNAME").ToString
                    txtAdvanceAddress1.Text = .Item("DOORNO").ToString + " " + .Item("ADDRESS1").ToString
                    txtAdvanceAddress2.Text = .Item("ADDRESS2").ToString
                    txtAdvanceAddress3.Text = .Item("ADDRESS3").ToString
                    txtAdvanceAcCode.Text = .Item("ADVCODE").ToString
                    If .Item("ADVCODE").ToString = "" Then
                        If txtAdvanceNo.Text <> "" Then
                            If txtAdvanceNo.Text.StartsWith("O") Then
                                txtAdvanceAcCode.Text = "ADVORD"
                            End If
                        Else
                            If .Item("REFNO").ToString.Contains("O") Then txtAdvanceAcCode.Text = "ADVORD"
                        End If
                    End If

                    txtAddressPrevilegeId.Text = .Item("PREVILEGEID").ToString
                    txtAddressPartyCode.Text = .Item("ACCODE").ToString
                    cmbAddressTitle_OWN.Text = .Item("TITLE").ToString
                    txtAddressInitial.Text = .Item("INITIAL").ToString
                    txtAddressName.Text = .Item("PNAME").ToString
                    txtAddressDoorNo.Text = .Item("DOORNO").ToString
                    txtAddress1.Text = .Item("ADDRESS1").ToString
                    txtAddress2.Text = .Item("ADDRESS2").ToString
                    txtAddress3.Text = .Item("ADDRESS3").ToString
                    cmbAddressArea_OWN.Text = .Item("AREA").ToString
                    cmbAddressCity_OWN.Text = .Item("CITY").ToString
                    cmbAddressState_OWN.Text = .Item("STATE").ToString
                    cmbAddressCountry_OWN.Text = .Item("COUNTRY").ToString
                    txtAddressPincode_NUM.Text = .Item("PINCODE").ToString
                    txtAddressEmailId_OWN.Text = .Item("EMAIL").ToString
                    txtAddressFax.Text = .Item("FAX").ToString
                    txtAddressPhoneRes.Text = .Item("PHONERES").ToString
                    txtAddressMobile.Text = .Item("MOBILE").ToString
                    txtAddressRegularSno.Text = .Item("SNO").ToString
                    OrgRunno = .Item("ORGRUNNO").ToString
                    If OrgRunno <> "" Then txtAdvCostid.Text = Mid(OrgRunno, 1, 2).ToString
                    ORGBATCHNO = .Item("ORGBATCHNO").ToString

                    txtAdvanceNo.Text = .Item("REFNO").ToString
                    txtAdvanceEntAdvanceNo.Text = txtAdvanceNo.Text
                    txtAdvanceEntAmount.Text = txtAdvanceBalance_AMT.Text
                    txtAdvanceCompanyId.Text = .Item("COMPANYID").ToString
                    txtAdvanceBalance_AMT.Focus()

                    strSql = "SELECT TOP 1 REMARK1 FROM " & cnAdminDb & "..OUTSTANDING "
                    strSql += " WHERE TRANTYPE = 'A' AND RUNNO = '" & GetCostId(BillCostId) & GetCompanyId(strCompanyId) & row.Item("REFNO") & "'"
                    strSql += " AND COMPANYID = '" & strCompanyId & "'"
                    strSql += vbCrLf + " AND isnull(COSTID,'') = '" & IIf(txtAdvCostid.Text <> "", txtAdvCostid.Text, BillCostId) & "'"

                    mRemark = GetSqlValue(cn, strSql)
                    txtRemark.Text = mRemark
                    mRemark = mRemark & vbCrLf & .Item("REMARK1").ToString
                    If Replace(mRemark, vbCrLf, "") <> "" Then
                        MsgBox(mRemark, MsgBoxStyle.Information, "Remarks..")
                    End If
                End With
            End If
        End If
    End Sub

    Private Function CheckAuth() As Boolean
        If AuthPwdPass = True Then Return False
        strSql = "SELECT COUNT(*)CNT FROM " & cnAdminDb & "..BILLAUTHORIZE"
        If Val(objGPack.GetSqlValue(strSql, "CNT", 0)) > 0 Then
            Dim Runno As String = GetCostId(BillCostId) & GetCompanyId(strCompanyId) & txtAdvanceNo.Text
            strSql = "SELECT  DATEDIFF(DD,TRANDATE,'" & BILLDATE & "')AS DAYSDIFF  "
            strSql += " FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO='" & Runno & "' AND RECPAY='R' ORDER BY TRANDATE"
            Dim DaysDiff As Integer = Val(objGPack.GetSqlValue(strSql, "DAYSDIFF", 0))
            strSql = "SELECT (SELECT USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERID=A.USERID)USERNAME,* "
            strSql += " FROM " & cnAdminDb & "..BILLAUTHORIZE A WHERE " & DaysDiff & " BETWEEN FROMDAYS AND TODAYS"
            Dim dr As DataRow = GetSqlRow(strSql, cn)
            If Not dr Is Nothing Then
                strSql = "SELECT COUNT(*)CNT FROM " & cnAdminDb & "..PWDAUTHORIZE "
                strSql += " WHERE REFNO='" & Runno & "' "
                If Val(objGPack.GetSqlValue(strSql, "CNT", 0).ToString) = 0 Then
                    Dim PwdId As Integer = 0
                    strSql = "SELECT ISNULL(MAX(PWDID),0) FROM " & cnAdminDb & "..PWDAUTHORIZE"
                    PwdId = Val(GetSqlValue(cn, strSql)) + 1
                    strSql = "INSERT INTO " & cnAdminDb & "..PWDAUTHORIZE"
                    strSql += " (PWDID,REFNO,CRUSERID,AUTHORIZE,PWDTYPE,DAYS,UPDATED,PWDDATE)"
                    strSql += " VALUES"
                    strSql += " ("
                    strSql += " " & PwdId
                    strSql += " ,'" & Runno & "'"
                    strSql += " ," & userId
                    strSql += " ,'N'"
                    strSql += " ,'A'"
                    strSql += " ," & DaysDiff
                    strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'Updated
                    strSql += " ,'" & GetServerDate() & "'" 'Updated
                    strSql += " )"
                    cmd = New OleDbCommand(strSql, cn)
                    cmd.ExecuteNonQuery()
                    MsgBox("Authorization Required from " & dr("USERNAME").ToString, MsgBoxStyle.Information)
                    Return True
                End If
                strSql = "SELECT ISNULL(AUTHORIZE,'N')AUTHORIZE FROM " & cnAdminDb & "..PWDAUTHORIZE "
                strSql += " WHERE REFNO='" & Runno & "' "
                If objGPack.GetSqlValue(strSql, "AUTHORIZE", "N") = "Y" Then
                    AuthPwdPass = True
                    Return False
                Else
                    MsgBox("Authorization Required from " & dr("USERNAME").ToString, MsgBoxStyle.Information)
                    Return True
                End If
            Else
                AuthPwdPass = True
                Return False
            End If
        Else
            AuthPwdPass = True
            Return False
        End If
    End Function

    Private Sub txtAdvanceNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtAdvanceNo.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtAdvanceNo.Text = "" Then
                MsgBox(Me.GetNextControl(txtAdvanceNo, False).Text + E0001, MsgBoxStyle.Information)
                txtAdvanceNo.Focus()
                Exit Sub
            End If
            For Each roAdv As DataRow In dtGridAdvance.Rows
                If txtAdvanceRowIndex.Text <> "" Then Exit For
                If txtAdvanceNo.Text = roAdv!RUNNO.ToString Then
                    MsgBox("Already Exist in this Advance No", MsgBoxStyle.Information)
                    txtAdvanceNo.Select()
                    txtAdvanceNo.SelectAll()
                    Exit Sub
                End If
                If EnableSchemeoffer Then
                    Dim mrunchitchk As String = roAdv!RUNNO.ToString
                    Dim dtadv As New DataTable
                    Dim sqlqry As String
                    sqlqry = " SELECT BATCHNO, AMOUNT FROM " & cnAdminDb & "..OUTSTANDING WHERE PAYMODE = 'AR' AND RECPAY = 'R' AND RUNNO='" & GetCostId(BillCostId) & GetCompanyId(strCompanyId) & mrunchitchk & "' AND ISNULL(CANCEL,'')='' "
                    Dim advoutdr As DataRow = GetSqlRow(sqlqry, cn)
                    Dim advbatchno As String = advoutdr.Item(0).ToString
                    sqlqry = "Select CHQCARDREF,CHQCARDNO,SUM(AMOUNT)AMOUNT,PAYMODE FROM " & cnStockDb & "..ACCTRAN WHERE PAYMODE IN('SS','CB') AND ISNULL(CANCEL,'') <> 'Y' AND BATCHNO = '" & advbatchno & "'"
                    sqlqry += " GROUP BY CHQCARDREF,CHQCARDNO,PAYMODE ORDER BY PAYMODE DESC"
                    Dim dtAdvCheck As New DataTable
                    da = New OleDbDataAdapter(sqlqry, cn)
                    da.Fill(dtAdvCheck)
                    If dtAdvCheck.Rows.Count > 0 Then
                        MsgBox("Saving scheme card Advance adjusted" & vbCrLf & "No more advance to adjust", MsgBoxStyle.Information)
                        txtAdvanceNo.Select()
                        txtAdvanceNo.SelectAll()
                        Exit Sub
                    End If
                End If
            Next
            If CheckAuth() Then Exit Sub





            If ADVADJDAYS <> 0 Then

                ''''''new added for customer otp
                Dim advmobileno As String = ""
                Dim sqlqryy As String = ""
                If ADVcustomerotp Then
                    sqlqryy = " SELECT TOP 1  MOBILE FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO =( "
                    sqlqryy += " SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = ( "
                    sqlqryy += " SELECT DISTINCT BATCHNO FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO LIKE '%" & txtAdvanceNo.Text.Trim & "' "
                    sqlqryy += " AND TRANTYPE='A' AND RECPAY='R' AND COMPANYID='" & strCompanyId & "' "
                    If txtAdvCostid.Text <> "00" Then
                        sqlqryy += " And COSTID='" & txtAdvCostid.Text & "' "
                    End If
                    sqlqryy += " And ISNULL(CANCEL,'')='')) "
                    advmobileno = GetSqlValue(cn, sqlqryy)
                Else
                    advmobileno = ""
                End If
                sqlqryy = ""
                sqlqryy = " SELECT TOP 1 TRANDATE FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO LIKE '%" & txtAdvanceNo.Text.Trim & "' "
                sqlqryy += " AND TRANTYPE='A' AND RECPAY='R' AND COMPANYID='" & strCompanyId & "' "
                If txtAdvCostid.Text <> "00" Then
                    sqlqryy += " And COSTID='" & txtAdvCostid.Text & "' "
                End If
                sqlqryy += " And ISNULL(CANCEL,'')='' ORDER BY TRANDATE"
                Dim ADVDATECHK As Date = Nothing
                ADVDATECHK = GetSqlValue(cn, sqlqryy)

                If ADVDATECHK.ToString.Length > 0 Then
                    dtpAdvanceDate.Value = ADVDATECHK.Date
                End If

                ''''''END new added for customer otp

                If DateDiff(DateInterval.Day, dtpAdvanceDate.Value, BILLDATE) > ADVADJDAYS Then
                    Dim pwdid As Integer = 0 : Dim pwdpass As Boolean = False
                    Dim Sqlqry As String = "Select Optionid from " & cnAdminDb & "..PRJPWDOPTION where OPTIONNAME ='ADVADJ' AND active = 'Y'"
                    Dim Optionid As Integer = Val("" & objGPack.GetSqlValue(Sqlqry, , , tran))
                    If userId <> 999 And Optionid <> 0 Then
                        pwdid = GetuserPwd(Optionid, cnCostId, userId)
                        If pwdid <> 0 Then
                            Dim objUpwd As New frmUserPassword(pwdid, Optionid, True, advmobileno.ToString.Trim)
                            If objUpwd.ShowDialog <> Windows.Forms.DialogResult.OK Then MsgBox("Access Denied.") : Exit Sub Else pwdpass = True
                        Else
                            Dim objUpwd As New frmUserPassword(pwdid, Optionid, True, advmobileno.ToString.Trim)
                            If objUpwd.ShowDialog <> Windows.Forms.DialogResult.OK Then
                                MsgBox("Access Denied.")
                                Exit Sub
                            Else
                                pwdpass = True
                            End If
                        End If
                    Else
                        pwdpass = True
                    End If
                    If pwdpass = False And txtAdvanceNo.Text <> "" Then
                        'If Authorize Then
                        '        Dim objSecret As New frmAdminPassword()
                        '        If objSecret.ShowDialog <> Windows.Forms.DialogResult.OK Then MsgBox("Access Denied.") : Exit Sub
                        '    Else
                        txtAdvanceNo.Text = ""
                        txtAdvanceAdjusted_AMT.Text = ""
                        txtAdvanceNo.Focus()
                        txtAdvanceBalance_AMT.Text = ""
                        txtAdvanceReceived_AMT.Text = ""
                        MsgBox("Access Denied.")
                        Exit Sub
                        '    End If
                    End If
                End If
            End If

            Main.HideHelpText()







            strSql = " IF (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "_CUSTOMERINFO' AND TYPE='U') > 0"
            strSql += vbCrLf + " DROP TABLE TEMPTABLEDB..TEMP" & systemId & "_CUSTOMERINFO"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = " SELECT C.PSNO AS SNO,C.BATCHNO,TRANDATE,PREVILEGEID,ACCODE,TITLE"
            strSql += vbCrLf + " ,INITIAL,PNAME,DOORNO,ADDRESS1,ADDRESS2,ADDRESS3,AREA,CITY,STATE,COUNTRY"
            strSql += vbCrLf + " ,PINCODE,PHONERES,MOBILE,EMAIL,FAX,C.REMARK1"
            strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "_CUSTOMERINFO"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..PERSONALINFO P "
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CUSTOMERINFO C ON P.SNO = C.PSNO"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = " IF (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "_OUTST' AND XTYPE='U') > 0"
            strSql += vbCrLf + " DROP TABLE TEMPTABLEDB..TEMP" & systemId & "_OUTST"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()

            strSql = " SELECT RUNNO "
            strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE -1*AMOUNT END)AS BALANCE"
            strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "_OUTST "
            strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING "
            strSql += vbCrLf + " WHERE TRANTYPE IN('A','C') "
            strSql += vbCrLf + " AND ISNULL(CANCEL,'') = '' "
            strSql += vbCrLf + " GROUP BY RUNNO "
            strSql += vbCrLf + " HAVING SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE -1*AMOUNT END) > 0 "
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            If txtAdvanceNo.Text.Substring(0, 1).ToUpper = "C" Then
                Dim selectcostid As String = txtAdvCostid.Text
                AdvCostid = selectcostid
                strSql = " IF (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "_CUSTOMERINFO' AND XTYPE='U') > 0"
                strSql += vbCrLf + " DROP TABLE TEMPTABLEDB..TEMP" & systemId & "_CUSTOMERINFO"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                strSql = vbCrLf + " SELECT C.PSNO as SNO,C.BATCHNO,TRANDATE,PREVILEGEID"
                strSql += vbCrLf + " ,ACCODE,TITLE,INITIAL,PNAME,DOORNO,ADDRESS1,ADDRESS2,ADDRESS3"
                strSql += vbCrLf + " ,AREA,CITY,STATE,COUNTRY,PINCODE,PHONERES,MOBILE,EMAIL,FAX,C.REMARK1"
                strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "_CUSTOMERINFO"
                strSql += vbCrLf + " FROM " & cnAdminDb & "..PERSONALINFO P "
                strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CUSTOMERINFO C ON P.SNO = C.PSNO"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                strSql = " SELECT SUBSTRING(RUNNO,6,20) REFNO"
                strSql += vbCrLf + " ,CASE WHEN RECEIPT <> 0 THEN RECEIPT ELSE NULL END AS RECEIPT"
                strSql += vbCrLf + " ,CASE WHEN PAYMENT <> 0 THEN PAYMENT ELSE NULL END AS PAYMENT"
                strSql += vbCrLf + " ,CASE WHEN BALANCE <> 0 THEN BALANCE ELSE NULL END AS BALANCE"
                strSql += vbCrLf + " ,x.TRANDATE,V.PREVILEGEID,V.ACCODE ACCODE,X.ACCODE ADVCODE,V.TITLE,V.INITIAL,V.PNAME,V.DOORNO,V.ADDRESS1,V.ADDRESS2,V.ADDRESS3"
                strSql += vbCrLf + " ,V.AREA,V.CITY,V.STATE,V.COUNTRY,V.PINCODE,V.PHONERES,V.MOBILE,V.EMAIL,V.FAX,V.SNO,X.COMPANYID,V.REMARK1,TRANFLAG"
                strSql += vbCrLf + " ,RUNNO ORGRUNNO,X.BATCHNO ORGBATCHNO,GST FROM"
                strSql += vbCrLf + " ("
                strSql += vbCrLf + " SELECT RUNNO"
                strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT+ISNULL(GSTVAL,0) ELSE 0 END)AS RECEIPT"
                strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT+ISNULL(GSTVAL,0) ELSE 0 END)AS PAYMENT"
                strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT+ISNULL(GSTVAL,0) ELSE -1*(AMOUNT+ISNULL(GSTVAL,0)) END)AS BALANCE"
                strSql += vbCrLf + " ,(SELECT TOP 1 TRANDATE FROM " & cnAdminDb & "..OUTSTANDING WHERE COSTID = '" & selectcostid & "' AND RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  AND ISNULL(CANCEL,'') = '' ORDER BY TRANDATE DESC)AS TRANDATE"
                strSql += vbCrLf + " ,(SELECT TOP 1 BATCHNO FROM " & cnAdminDb & "..OUTSTANDING WHERE COSTID = '" & selectcostid & "' AND RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  AND ISNULL(CANCEL,'') = '' ORDER BY TRANDATE DESC)AS BATCHNO"
                strSql += vbCrLf + " ,O.ACCODE ACCODE,O.COMPANYID ,ISNULL(TRANFLAG,'') AS TRANFLAG"
                strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN GSTVAL ELSE -1*GSTVAL END)AS GST"
                strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING O"
                strSql += vbCrLf + " WHERE TRANTYPE ='C'"
                strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
                strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
                strSql += vbCrLf + " AND COSTID = '" & selectcostid & "'"
                strSql += vbCrLf + " AND SUBSTRING(RUNNO,6,20) = '" & txtAdvanceNo.Text & "'"
                strSql += vbCrLf + " AND PAYMODE IN('RP','RA')"
                strSql += vbCrLf + " AND RUNNO IN(SELECT RUNNO FROM TEMPTABLEDB..TEMP" & systemId & "_OUTST)"
                strSql += vbCrLf + " GROUP BY ISNULL(TRANFLAG,'') ,RUNNO,COMPANYID,ACCODE"
                strSql += vbCrLf + " HAVING SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE -1*AMOUNT END) > 0 "
                strSql += vbCrLf + " )X"
                strSql += vbCrLf + " INNER JOIN TEMPTABLEDB..TEMP" & systemId & "_CUSTOMERINFO AS V ON V.BATCHNO = X.BATCHNO"
                Dim Adrow As DataRow
                Adrow = GetSqlRow(strSql, cn)
                Dim row As DataRow = Nothing
                If Not Adrow Is Nothing Then
                    If Adrow.Table.Rows.Count = 1 Then
                        row = Adrow
                    Else
                        row = BrighttechPack.SearchDialog.Show_R("Search Advance Reference No", strSql, cn, 9, , , , , , , False)
                    End If
                    With row
                        For Each roAdv As DataRow In dtGridAdvance.Rows
                            If txtAdvanceRowIndex.Text <> "" Then Exit For
                            If .Item("REFNO").ToString = roAdv!RUNNO.ToString Then
                                MsgBox("Already Exist...", MsgBoxStyle.Information)
                                txtAdvanceNo.Select()
                                txtAdvanceNo.SelectAll()
                                Exit Sub
                            End If
                        Next
                        If .Item("TRANFLAG").ToString = "L" Then
                            MsgBox("This has been locked", MsgBoxStyle.Information)
                            txtAdvanceNo.Select()
                            txtAdvanceNo.SelectAll()
                            Exit Sub
                        End If
                        dtpAdvanceDate.Value = .Item("TRANDATE")
                        txtAdvanceReceived_AMT.Text = IIf(Val(.Item("RECEIPT").ToString) <> 0, Format(Val(.Item("RECEIPT").ToString), "0.00"), Nothing)
                        txtAdvanceAdjusted_AMT.Text = IIf(Val(.Item("PAYMENT").ToString) <> 0, Format(Val(.Item("PAYMENT").ToString), "0.00"), Nothing)
                        txtAdvanceBalance_AMT.Text = IIf(Val(.Item("BALANCE").ToString) <> 0, Format(Val(.Item("BALANCE").ToString), "0.00"), Nothing)
                        If Val(.Item("GST").ToString) > 0 Then
                            GstFlag = True
                        End If
                        If GST And GstFlag Then
                            Dim Per As Decimal = 103
                            'If GSTADVCALC_INCL = "I" Then Per = 103
                            Dim GstAmt As Double
                            Dim SGstAmt As Double = (Val(txtAdvanceBalance_AMT.Text) * (GstPer / 2) / Per)
                            Dim CGstAmt As Double = (Val(txtAdvanceBalance_AMT.Text) * (GstPer / 2) / Per)
                            SGstAmt = Math.Round(SGstAmt, 2)
                            CGstAmt = Math.Round(CGstAmt, 2)
                            SGstAmt = CalcRoundoffAmt(SGstAmt, Round_Gst)
                            CGstAmt = CalcRoundoffAmt(CGstAmt, Round_Gst)

                            GstAmt = SGstAmt + CGstAmt
                            txtAdvanceGST_AMT.Text = IIf(GstAmt <> 0, Format(GstAmt, "0.00"), Nothing)
                            Dim TotAmt As Double = Val(txtAdvanceBalance_AMT.Text)
                            TotAmt -= Val(txtAdvanceGST_AMT.Text)
                            txtAdvanceAmt_AMT.Text = IIf(TotAmt <> 0, Format(TotAmt, "0.00"), Nothing)
                        End If
                        txtAdvanceName.Text = .Item("PNAME").ToString
                        txtAdvanceAddress1.Text = .Item("DOORNO").ToString + " " + .Item("ADDRESS1").ToString
                        txtAdvanceAddress2.Text = .Item("ADDRESS2").ToString
                        txtAdvanceAddress3.Text = .Item("ADDRESS3").ToString
                        txtAdvanceAcCode.Text = "DRS"
                        txtAddressPrevilegeId.Text = .Item("PREVILEGEID").ToString
                        txtAddressPartyCode.Text = .Item("ACCODE").ToString
                        cmbAddressTitle_OWN.Text = .Item("TITLE").ToString
                        txtAddressInitial.Text = .Item("INITIAL").ToString
                        txtAddressName.Text = .Item("PNAME").ToString
                        txtAddressDoorNo.Text = .Item("DOORNO").ToString
                        txtAddress1.Text = .Item("ADDRESS1").ToString
                        txtAddress2.Text = .Item("ADDRESS2").ToString
                        txtAddress3.Text = .Item("ADDRESS3").ToString
                        cmbAddressArea_OWN.Text = .Item("AREA").ToString
                        cmbAddressCity_OWN.Text = .Item("CITY").ToString
                        cmbAddressState_OWN.Text = .Item("STATE").ToString
                        cmbAddressCountry_OWN.Text = .Item("COUNTRY").ToString
                        txtAddressPincode_NUM.Text = .Item("PINCODE").ToString
                        txtAddressEmailId_OWN.Text = .Item("EMAIL").ToString
                        txtAddressFax.Text = .Item("FAX").ToString
                        txtAddressPhoneRes.Text = .Item("PHONERES").ToString
                        txtAddressMobile.Text = .Item("MOBILE").ToString
                        txtAddressRegularSno.Text = .Item("SNO").ToString
                        OrgRunno = .Item("ORGRUNNO").ToString
                        ORGBATCHNO = .Item("ORGBATCHNO").ToString
                        txtAdvanceNo.Text = .Item("REFNO").ToString
                        txtAdvanceEntAdvanceNo.Text = txtAdvanceNo.Text
                        txtAdvanceEntAmount.Text = txtAdvanceBalance_AMT.Text
                        txtAdvanceCompanyId.Text = .Item("COMPANYID").ToString
                        txtAdvanceBalance_AMT.Focus()
                    End With
                Else
                    strSql = "SELECT TRANNO,CONVERT(VARCHAR,TRANDATE,103)AS TRANDATE"
                    strSql += vbCrLf + " ,(SELECT TOP 1 USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERID=O.USERID)USERNAME"
                    strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING O"
                    strSql += vbCrLf + " WHERE (RUNNO = '" & GetCostId(BillCostId) & GetCompanyId(strCompanyId) & txtAdvanceNo.Text & "' "
                    strSql += vbCrLf + " OR (TRANSTATUS = 'T' AND RUNNO LIKE '%" & GetCompanyId(strCompanyId) & txtAdvanceNo.Text & "'))"
                    strSql += vbCrLf + " AND TRANTYPE = 'C' "
                    strSql += vbCrLf + " AND COSTID = '" & BillCostId & "'"
                    strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
                    strSql += vbCrLf + " AND RECPAY='P' ORDER BY TRANDATE DESC"
                    Dim dtAdvAdj As New DataTable
                    da = New OleDbDataAdapter(strSql, cn)
                    da.Fill(dtAdvAdj)
                    If dtAdvAdj.Rows.Count > 0 Then
                        Dim Msg As String = "Already Adjusted" & vbCrLf &
                        "BillNo:" & dtAdvAdj.Rows(0).Item("TRANNO").ToString & vbCrLf &
                        "Billdate:" & dtAdvAdj.Rows(0).Item("TRANDATE").ToString & vbCrLf &
                        "UserName:" & dtAdvAdj.Rows(0).Item("USERNAME").ToString
                        MsgBox(Msg, MsgBoxStyle.Information)
                        txtAdvanceNo.Focus()
                    End If
                End If
            ElseIf txtAdvanceNo.Text.Substring(0, 1).ToUpper <> "Z" Then
                strSql = " SELECT SUBSTRING(RUNNO,6,20) REFNO"
                If ISMAINT_CENTADV Then strSql += vbCrLf + ", SUBSTRING(RUNNO,1,2) AS COSTID"
                strSql += vbCrLf + " ,CASE WHEN RECEIPT <> 0 THEN RECEIPT ELSE NULL END AS RECEIPT"
                strSql += vbCrLf + " ,CASE WHEN PAYMENT <> 0 THEN PAYMENT ELSE NULL END AS PAYMENT"
                strSql += vbCrLf + " ,CASE WHEN BALANCE <> 0 THEN BALANCE ELSE NULL END AS BALANCE"
                strSql += vbCrLf + " ,x.TRANDATE,V.PREVILEGEID,V.ACCODE ACCODE,X.ACCODE ADVCODE,V.TITLE,V.INITIAL,V.PNAME,V.DOORNO,V.ADDRESS1,V.ADDRESS2,V.ADDRESS3"
                strSql += vbCrLf + " ,V.AREA,V.CITY,V.STATE,V.COUNTRY,V.PINCODE,V.PHONERES,V.MOBILE,V.EMAIL,V.FAX,V.SNO,X.COMPANYID,V.REMARK1"
                strSql += vbCrLf + " ,RUNNO AS ORGRUNNO,X.BATCHNO AS ORGBATCHNO,GST  FROM"
                strSql += vbCrLf + " ("

                strSql += vbCrLf + " SELECT RUNNO"
                strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT+ISNULL(GSTVAL,0) ELSE 0 END)AS RECEIPT"
                strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT+ISNULL(GSTVAL,0) ELSE 0 END)AS PAYMENT"
                strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT+ISNULL(GSTVAL,0) ELSE -1*(AMOUNT+ISNULL(GSTVAL,0)) END)AS BALANCE"
                strSql += vbCrLf + " ,(SELECT TOP 1 TRANDATE FROM " & cnAdminDb & "..OUTSTANDING WHERE " & IIf(ISMAINT_CENTADV, "", "COSTID = '" & BillCostId & "' AND ") & " RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  AND ISNULL(CANCEL,'') = '' ORDER BY TRANDATE,SNO DESC)AS TRANDATE"
                strSql += vbCrLf + " ,(SELECT TOP 1 BATCHNO FROM " & cnAdminDb & "..OUTSTANDING WHERE " & IIf(ISMAINT_CENTADV, "", "COSTID = '" & BillCostId & "' AND ") & " RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  AND ISNULL(CANCEL,'') = '' ORDER BY TRANDATE,SNO DESC)AS BATCHNO"
                strSql += vbCrLf + " ,O.ACCODE ACCODE,O.COMPANYID"
                strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN GSTVAL ELSE -1*GSTVAL END)AS GST"
                strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING O"
                strSql += vbCrLf + " WHERE (RUNNO = '" & GetCostId(BillCostId) & GetCompanyId(strCompanyId) & txtAdvanceNo.Text & "' OR (TRANSTATUS = 'T' AND RUNNO LIKE '%" & GetCompanyId(strCompanyId) & txtAdvanceNo.Text & "'))" ' RUNNO = '" & GetCostId(BillCostId) & GetCompanyId(strCompanyId) & txtAdvanceNo.Text & "'"
                strSql += vbCrLf + " AND TRANTYPE = 'A' "

                If Not withOrderDetail Then strSql += vbCrLf + " AND SUBSTRING(RUNNO,6,20) NOT LIKE 'O%'"
                If Not withRepairDetail Then strSql += vbCrLf + " AND SUBSTRING(RUNNO,6,20) NOT LIKE 'R%'"
                strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
                If Not ISMAINT_CENTADV Then strSql += vbCrLf + " AND COSTID = '" & BillCostId & "'"
                strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
                strSql += vbCrLf + " AND RUNNO IN(SELECT RUNNO FROM TEMPTABLEDB..TEMP" & systemId & "_OUTST)"
                strSql += vbCrLf + " GROUP BY RUNNO,COMPANYID,O.ACCODE"
                strSql += vbCrLf + " )X"
                strSql += vbCrLf + " INNER JOIN TEMPTABLEDB..TEMP" & systemId & "_CUSTOMERINFO AS V ON V.BATCHNO = X.BATCHNO"
                Dim dt As New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dt)
                If Not dt.Rows.Count > 0 Then
                    If RunNoCheck = False Then
                        If MessageBox.Show("Runno Info Not Found. Do you wish to continue?", "Invalid Runno Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
                            RunNoChecked = False
                            Exit Sub
                        Else
                            RunNoChecked = True
                            txtAdvanceBalance_AMT.Focus()
                        End If
                        Exit Sub
                    End If
                    MsgBox(E0004 + Me.GetNextControl(txtAdvanceNo, False).Text, MsgBoxStyle.Information)
                    Exit Sub
                Else
                    If dt.Rows.Count = 0 Then
                        With dt.Rows(0)
                            If Not Val(.Item("BALANCE").ToString) > 0 Then
                                strSql = " SELECT BATCHNO,TRANNO,CONVERT(VARCHAR,TRANDATE,103)AS TRANDATE,AMOUNT,REMARK1"
                                strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING O"
                                'strSql += vbCrLf + " WHERE RUNNO = '" & GetCostId(BillCostId) & GetCompanyId(strCompanyId) & txtAdvanceNo.Text & "'"
                                strSql += vbCrLf + " WHERE (RUNNO = '" & GetCostId(BillCostId) & GetCompanyId(strCompanyId) & txtAdvanceNo.Text & "' OR (TRANSTATUS = 'T' AND RUNNO LIKE '%" & GetCompanyId(strCompanyId) & txtAdvanceNo.Text & "'))"
                                strSql += vbCrLf + " AND TRANTYPE = 'A' AND RECPAY = 'P'"
                                If Not withOrderDetail Then strSql += vbCrLf + " AND SUBSTRING(RUNNO,6,20) NOT LIKE 'O%'"
                                If Not withRepairDetail Then strSql += vbCrLf + " AND SUBSTRING(RUNNO,6,20) NOT LIKE 'R%'"
                                strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
                                strSql += vbCrLf + " AND COSTID = '" & BillCostId & "'"
                                strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
                                strSql += vbCrLf + " ORDER BY TRANDATE DESC"
                                Dim DtAdjInfo As New DataTable
                                da = New OleDbDataAdapter(strSql, cn)
                                da.Fill(DtAdjInfo)
                                If DtAdjInfo.Rows.Count > 0 Then
                                    Dim adjStr As String = "Already Adjusted" + vbCrLf
                                    For Each ro As DataRow In DtAdjInfo.Rows
                                        adjStr += vbCrLf + " TranNo : " & LSet(ro.Item("TRANNO").ToString, 5) & " TranDate : " & ro.Item("TRANDATE").ToString & " Amount : " & Format(Val(ro.Item("AMOUNT").ToString), "0.00")
                                    Next
                                    MsgBox(adjStr, MsgBoxStyle.Information)
                                    Exit Sub
                                End If
                            End If
                            dtpAdvanceDate.Value = .Item("TRANDATE")
                            txtAdvanceReceived_AMT.Text = IIf(Val(.Item("RECEIPT").ToString) <> 0, Format(Val(.Item("RECEIPT").ToString), "0.00"), Nothing)
                            txtAdvanceAdjusted_AMT.Text = IIf(Val(.Item("PAYMENT").ToString) <> 0, Format(Val(.Item("PAYMENT").ToString), "0.00"), Nothing)
                            txtAdvanceBalance_AMT.Text = IIf(Val(.Item("BALANCE").ToString) <> 0, Format(Val(.Item("BALANCE").ToString), "0.00"), Nothing)
                            txtAdvanceName.Text = .Item("PNAME").ToString
                            txtAdvanceAddress1.Text = .Item("DOORNO").ToString + " " + .Item("ADDRESS1").ToString
                            txtAdvanceAddress2.Text = .Item("ADDRESS2").ToString
                            txtAdvanceAddress3.Text = .Item("ADDRESS3").ToString
                            txtAdvanceAcCode.Text = .Item("ACCODE").ToString
                            ORGBATCHNO = .Item("ORGBATCHNO").ToString
                            If .Item("ACCODE").ToString = "" Then
                                If txtAdvanceNo.Text.StartsWith("O") Then
                                    txtAdvanceAcCode.Text = "ADVORD"
                                End If
                            End If

                            txtAddressPrevilegeId.Text = .Item("PREVILEGEID").ToString
                            txtAddressPartyCode.Text = .Item("ACCODE").ToString
                            cmbAddressTitle_OWN.Text = .Item("TITLE").ToString
                            txtAddressInitial.Text = .Item("INITIAL").ToString
                            txtAddressName.Text = .Item("PNAME").ToString
                            txtAddressDoorNo.Text = .Item("DOORNO").ToString
                            txtAddress1.Text = .Item("ADDRESS1").ToString
                            txtAddress2.Text = .Item("ADDRESS2").ToString
                            txtAddress3.Text = .Item("ADDRESS3").ToString
                            cmbAddressArea_OWN.Text = .Item("AREA").ToString
                            cmbAddressCity_OWN.Text = .Item("CITY").ToString
                            cmbAddressState_OWN.Text = .Item("STATE").ToString
                            cmbAddressCountry_OWN.Text = .Item("COUNTRY").ToString
                            txtAddressPincode_NUM.Text = .Item("PINCODE").ToString
                            txtAddressEmailId_OWN.Text = .Item("EMAIL").ToString
                            txtAddressFax.Text = .Item("FAX").ToString
                            txtAddressPhoneRes.Text = .Item("PHONERES").ToString
                            txtAddressMobile.Text = .Item("MOBILE").ToString
                            txtAddressRegularSno.Text = .Item("SNO").ToString

                            txtAdvanceNo.Text = .Item("REFNO").ToString
                            txtAdvanceEntAdvanceNo.Text = txtAdvanceNo.Text
                            txtAdvanceEntAmount.Text = txtAdvanceBalance_AMT.Text
                            txtAdvanceCompanyId.Text = .Item("COMPANYID").ToString

                            strSql = "SELECT TOP 1 REMARK1,TRANFLAG FROM " & cnAdminDb & "..outstanding where TRANTYPE = 'A' AND runno = '" & GetCostId(BillCostId) & GetCompanyId(strCompanyId) & txtAdvanceNo.Text & "'"
                            strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
                            strSql += vbCrLf + " AND COSTID = '" & BillCostId & "'"
                            Dim dtosr As DataRow = GetSqlRow(strSql, cn)
                            If Not dtosr Is Nothing Then
                                mRemark = dtosr.Item("REMARK1").ToString 'GetSqlValue(cn, strSql)
                                If dtosr.Item("TRANFLAG").ToString = "L" Then
                                    MsgBox("Advance No. has been locked" & vbCrLf & "Please release the lock", MsgBoxStyle.Information, "Access denied..")
                                    txtAdvanceEntAdvanceNo.Text = ""
                                    txtAdvanceEntAmount.Text = ""
                                    txtAdvanceBalance_AMT.Text = ""
                                    txtAdvanceReceived_AMT.Text = ""
                                    txtAdvanceNo.Text = ""
                                    txtAdvanceNo.SelectAll()
                                    Exit Sub
                                End If
                            End If
                            txtRemark.Text = mRemark
                            mRemark = mRemark & vbCrLf & .Item("REMARK1").ToString
                            If mRemark <> "" And ADVREMATERT = True Then
                                MsgBox(mRemark, MsgBoxStyle.Information, "Remarks..")
                            End If
                            txtAdvanceBalance_AMT.Focus()
                        End With
                    Else
                        Dim selectcostid As String = txtAdvCostid.Text
                        If ISMAINT_CENTADV And dt.Rows.Count > 1 Then
                            strSql = " SELECT DISTINCT O.COSTID,C.COSTNAME FROM " & cnAdminDb & "..OUTSTANDING O LEFT JOIN " & cnAdminDb & "..COSTCENTRE C ON O.COSTID = C.COSTID"
                            strSql += vbCrLf + " WHERE (RUNNO = '" & GetCostId(BillCostId) & GetCompanyId(strCompanyId) & txtAdvanceNo.Text & "' OR (TRANSTATUS = 'T' AND RUNNO LIKE '%" & GetCompanyId(strCompanyId) & txtAdvanceNo.Text & "'))"
                            strSql += vbCrLf + " AND TRANTYPE = 'A' AND RECPAY = 'R'"
                            If Not withOrderDetail Then strSql += vbCrLf + " AND SUBSTRING(RUNNO,6,20) NOT LIKE 'O%'"
                            If Not withRepairDetail Then strSql += vbCrLf + " AND SUBSTRING(RUNNO,6,20) NOT LIKE 'R%'"
                            strSql += vbCrLf + " AND O.COMPANYID = '" & strCompanyId & "'"
                            strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
                            strSql += vbCrLf + " ORDER BY C.COSTNAME DESC"
                            selectcostid = BrighttechPack.SearchDialog.Show("Select Advance Cost centre", strSql, cn, 1, 0)
                        End If
                        If BillCostId <> "" And selectcostid <> "00" And selectcostid <> BillCostId And selectcostid <> "" And OTHBRANCH_ADVADJ_AUT Then
                            If MsgBox("Can you adjust Other Cost centre Advance ?" & vbCrLf & "If Yes, continue with OTP authendication only.", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                                If Not usrpwdokonly("OTHBRANCH_ADVADJ", IS_USERLEVELPWD) Then Exit Sub
                            Else
                                Exit Sub
                            End If
                        End If
                        If selectcostid = "00" Then selectcostid = ""
                        If selectcostid = "" And cnCostId <> "" Then selectcostid = cnCostId
                        AdvCostid = selectcostid
                        strSql = " IF (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "_CUSTOMERINFO' AND XTYPE='U') > 0"
                        strSql += vbCrLf + " DROP TABLE TEMPTABLEDB..TEMP" & systemId & "_CUSTOMERINFO"
                        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                        cmd.ExecuteNonQuery()

                        strSql = vbCrLf + " SELECT C.PSNO AS SNO,C.BATCHNO,TRANDATE,PREVILEGEID,ACCODE"
                        strSql += vbCrLf + " ,TITLE,INITIAL,PNAME,DOORNO,ADDRESS1,ADDRESS2,ADDRESS3,AREA,CITY"
                        strSql += vbCrLf + " ,STATE,COUNTRY,PINCODE,PHONERES,MOBILE,EMAIL,FAX,C.REMARK1"
                        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "_CUSTOMERINFO"
                        strSql += vbCrLf + " FROM " & cnAdminDb & "..PERSONALINFO P "
                        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CUSTOMERINFO C ON P.SNO = C.PSNO"
                        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                        cmd.ExecuteNonQuery()

                        strSql = " SELECT SUBSTRING(RUNNO,6,20) REFNO"
                        strSql += vbCrLf + " ,CASE WHEN RECEIPT <> 0 THEN RECEIPT ELSE NULL END AS RECEIPT"
                        strSql += vbCrLf + " ,CASE WHEN PAYMENT <> 0 THEN PAYMENT ELSE NULL END AS PAYMENT"
                        strSql += vbCrLf + " ,CASE WHEN BALANCE <> 0 THEN BALANCE ELSE NULL END AS BALANCE"
                        strSql += vbCrLf + " ,x.TRANDATE,V.PREVILEGEID,V.ACCODE ACCODE,X.ACCODE ADVCODE,V.TITLE,V.INITIAL,V.PNAME,V.DOORNO,V.ADDRESS1,V.ADDRESS2,V.ADDRESS3"
                        strSql += vbCrLf + " ,V.AREA,V.CITY,V.STATE,V.COUNTRY,V.PINCODE,V.PHONERES,V.MOBILE,V.EMAIL,V.FAX,V.SNO,X.COMPANYID,V.REMARK1,TRANFLAG"
                        strSql += vbCrLf + " ,RUNNO ORGRUNNO,X.BATCHNO ORGBATCHNO,GST FROM"
                        strSql += vbCrLf + " ("
                        strSql += vbCrLf + " SELECT RUNNO"
                        strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT+ISNULL(GSTVAL,0) ELSE 0 END)AS RECEIPT"
                        strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT+ISNULL(GSTVAL,0) ELSE 0 END)AS PAYMENT"
                        strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT+ISNULL(GSTVAL,0) ELSE -1*(AMOUNT+ISNULL(GSTVAL,0)) END)AS BALANCE"
                        strSql += vbCrLf + " ,(SELECT TOP 1 TRANDATE FROM " & cnAdminDb & "..OUTSTANDING WHERE COSTID = '" & selectcostid & "' AND RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  AND ISNULL(CANCEL,'') = '' ORDER BY TRANDATE DESC)AS TRANDATE"
                        strSql += vbCrLf + " ,(SELECT TOP 1 BATCHNO FROM " & cnAdminDb & "..OUTSTANDING WHERE COSTID = '" & selectcostid & "' AND RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  AND ISNULL(CANCEL,'') = '' ORDER BY TRANDATE DESC)AS BATCHNO"
                        strSql += vbCrLf + " ,O.ACCODE ACCODE,O.COMPANYID ,ISNULL(TRANFLAG,'') AS TRANFLAG"
                        strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN GSTVAL ELSE -1*GSTVAL END)AS GST"
                        strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING O"
                        strSql += vbCrLf + " WHERE TRANTYPE = 'A'"
                        If Not withOrderDetail Then strSql += vbCrLf + " AND SUBSTRING(RUNNO,6,20) NOT LIKE 'O%'"
                        If Not withRepairDetail Then strSql += vbCrLf + " AND SUBSTRING(RUNNO,6,20) NOT LIKE 'R%'"
                        strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
                        strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
                        strSql += vbCrLf + " AND COSTID = '" & selectcostid & "'"
                        strSql += vbCrLf + " AND SUBSTRING(RUNNO,6,20) = '" & txtAdvanceNo.Text & "'"
                        strSql += vbCrLf + " AND RUNNO IN(SELECT RUNNO FROM TEMPTABLEDB..TEMP" & systemId & "_OUTST)"
                        strSql += vbCrLf + " GROUP BY ISNULL(TRANFLAG,'') ,RUNNO,COMPANYID,ACCODE"
                        strSql += vbCrLf + " HAVING SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE -1*AMOUNT END) > 0 "
                        strSql += vbCrLf + " )X"
                        strSql += vbCrLf + " INNER JOIN TEMPTABLEDB..TEMP" & systemId & "_CUSTOMERINFO AS V ON V.BATCHNO = X.BATCHNO"
                        Dim Adrow As DataRow
                        Adrow = GetSqlRow(strSql, cn)
                        Dim row As DataRow = Nothing
                        If Not Adrow Is Nothing Then
                            If Adrow.Table.Rows.Count = 1 Then
                                row = Adrow
                            Else
                                row = BrighttechPack.SearchDialog.Show_R("Search Advance Reference No", strSql, cn, 9, , , , , , , False)
                            End If
                            With row
                                For Each roAdv As DataRow In dtGridAdvance.Rows
                                    If txtAdvanceRowIndex.Text <> "" Then Exit For
                                    If .Item("REFNO").ToString = roAdv!RUNNO.ToString Then
                                        MsgBox("Already Exist in this Advance No", MsgBoxStyle.Information)
                                        txtAdvanceNo.Select()
                                        txtAdvanceNo.SelectAll()
                                        Exit Sub
                                    End If
                                Next
                                If .Item("TRANFLAG").ToString = "L" Then
                                    MsgBox("This Advance No has been locked", MsgBoxStyle.Information)
                                    txtAdvanceNo.Select()
                                    txtAdvanceNo.SelectAll()
                                    Exit Sub
                                End If
                                dtpAdvanceDate.Value = .Item("TRANDATE")
                                txtAdvanceReceived_AMT.Text = IIf(Val(.Item("RECEIPT").ToString) <> 0, Format(Val(.Item("RECEIPT").ToString), "0.00"), Nothing)
                                txtAdvanceAdjusted_AMT.Text = IIf(Val(.Item("PAYMENT").ToString) <> 0, Format(Val(.Item("PAYMENT").ToString), "0.00"), Nothing)
                                txtAdvanceBalance_AMT.Text = IIf(Val(.Item("BALANCE").ToString) <> 0, Format(Val(.Item("BALANCE").ToString), "0.00"), Nothing)
                                If Val(.Item("GST").ToString) > 0 Then
                                    GstFlag = True
                                End If
                                If GST And GstFlag Then
                                    Dim Per As Decimal = 103
                                    'If GSTADVCALC_INCL = "I" Then Per = 103

                                    Dim GstAmt As Double
                                    Dim SGstAmt As Double = (Val(txtAdvanceBalance_AMT.Text) * (GstPer / 2) / Per)
                                    Dim CGstAmt As Double = (Val(txtAdvanceBalance_AMT.Text) * (GstPer / 2) / Per)
                                    SGstAmt = Math.Round(SGstAmt, 2)
                                    CGstAmt = Math.Round(CGstAmt, 2)
                                    SGstAmt = CalcRoundoffAmt(SGstAmt, Round_Gst)
                                    CGstAmt = CalcRoundoffAmt(CGstAmt, Round_Gst)

                                    GstAmt = SGstAmt + CGstAmt

                                    txtAdvanceGST_AMT.Text = IIf(GstAmt <> 0, Format(GstAmt, "0.00"), Nothing)
                                    Dim TotAmt As Double = Val(txtAdvanceBalance_AMT.Text)
                                    TotAmt -= Val(txtAdvanceGST_AMT.Text)
                                    txtAdvanceAmt_AMT.Text = IIf(TotAmt <> 0, Format(TotAmt, "0.00"), Nothing)
                                End If
                                txtAdvanceName.Text = .Item("PNAME").ToString
                                txtAdvanceAddress1.Text = .Item("DOORNO").ToString + " " + .Item("ADDRESS1").ToString
                                txtAdvanceAddress2.Text = .Item("ADDRESS2").ToString
                                txtAdvanceAddress3.Text = .Item("ADDRESS3").ToString
                                txtAdvanceAcCode.Text = .Item("ADVCODE").ToString
                                If .Item("ADVCODE").ToString = "" Then
                                    If txtAdvanceNo.Text.StartsWith("O") Then
                                        txtAdvanceAcCode.Text = "ADVORD"
                                    Else
                                    End If
                                End If

                                txtAddressPrevilegeId.Text = .Item("PREVILEGEID").ToString
                                txtAddressPartyCode.Text = .Item("ACCODE").ToString
                                cmbAddressTitle_OWN.Text = .Item("TITLE").ToString
                                txtAddressInitial.Text = .Item("INITIAL").ToString
                                txtAddressName.Text = .Item("PNAME").ToString
                                txtAddressDoorNo.Text = .Item("DOORNO").ToString
                                txtAddress1.Text = .Item("ADDRESS1").ToString
                                txtAddress2.Text = .Item("ADDRESS2").ToString
                                txtAddress3.Text = .Item("ADDRESS3").ToString
                                cmbAddressArea_OWN.Text = .Item("AREA").ToString
                                cmbAddressCity_OWN.Text = .Item("CITY").ToString
                                cmbAddressState_OWN.Text = .Item("STATE").ToString
                                cmbAddressCountry_OWN.Text = .Item("COUNTRY").ToString
                                txtAddressPincode_NUM.Text = .Item("PINCODE").ToString
                                txtAddressEmailId_OWN.Text = .Item("EMAIL").ToString
                                txtAddressFax.Text = .Item("FAX").ToString
                                txtAddressPhoneRes.Text = .Item("PHONERES").ToString
                                txtAddressMobile.Text = .Item("MOBILE").ToString
                                txtAddressRegularSno.Text = .Item("SNO").ToString
                                OrgRunno = .Item("ORGRUNNO").ToString
                                If OrgRunno <> "" Then txtAdvCostid.Text = Mid(OrgRunno, 1, 2).ToString
                                ORGBATCHNO = .Item("ORGBATCHNO").ToString
                                txtAdvanceNo.Text = .Item("REFNO").ToString
                                txtAdvanceEntAdvanceNo.Text = txtAdvanceNo.Text
                                txtAdvanceEntAmount.Text = txtAdvanceBalance_AMT.Text
                                txtAdvanceCompanyId.Text = .Item("COMPANYID").ToString
                                txtAdvanceBalance_AMT.Focus()

                                strSql = "SELECT TOP 1 REMARK1 FROM " & cnAdminDb & "..OUTSTANDING "
                                strSql += vbCrLf + " WHERE TRANTYPE = 'A' AND RUNNO = '" & GetCostId(BillCostId) & GetCompanyId(strCompanyId) & row.Item("REFNO") & "'"
                                strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
                                strSql += vbCrLf + " AND COSTID = '" & BillCostId & "'"
                                mRemark = GetSqlValue(cn, strSql)
                                txtRemark.Text = mRemark
                                mRemark = mRemark & vbCrLf & .Item("REMARK1").ToString
                                If mRemark <> "" Then
                                    MsgBox(mRemark, MsgBoxStyle.Information, "Remarks..")
                                End If
                            End With
                        Else
                            strSql = "SELECT TRANNO,CONVERT(VARCHAR,TRANDATE,103)AS TRANDATE"
                            strSql += vbCrLf + " ,(SELECT TOP 1 USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERID=O.USERID)USERNAME"
                            strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING O"
                            strSql += vbCrLf + " WHERE (RUNNO = '" & GetCostId(BillCostId) & GetCompanyId(strCompanyId) & txtAdvanceNo.Text & "' "
                            strSql += vbCrLf + " OR (TRANSTATUS = 'T' AND RUNNO LIKE '%" & GetCompanyId(strCompanyId) & txtAdvanceNo.Text & "'))"
                            strSql += vbCrLf + " AND TRANTYPE = 'A' "
                            If Not ISMAINT_CENTADV Then strSql += vbCrLf + " AND COSTID = '" & BillCostId & "'"
                            strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
                            strSql += vbCrLf + " AND RECPAY='P' ORDER BY TRANDATE DESC"
                            Dim dtAdvAdj As New DataTable
                            da = New OleDbDataAdapter(strSql, cn)
                            da.Fill(dtAdvAdj)
                            If dtAdvAdj.Rows.Count > 0 Then
                                Dim Msg As String = "Advance Already Adjusted" & vbCrLf &
                                "BillNo:" & dtAdvAdj.Rows(0).Item("TRANNO").ToString & vbCrLf &
                                "Billdate:" & dtAdvAdj.Rows(0).Item("TRANDATE").ToString & vbCrLf &
                                "UserName:" & dtAdvAdj.Rows(0).Item("USERNAME").ToString
                                MsgBox(Msg, MsgBoxStyle.Information)
                                txtAdvanceNo.Focus()
                            End If
                        End If
                    End If
                End If
            Else
                strSql = "SELECT * FROM " & cnStockDb & "..ISSUE"
                strSql += vbCrLf + " WHERE TRANDATE='" + BILLDATE.ToString("yyyy-MM-dd") + "'"
                strSql += vbCrLf + " AND TRANTYPE='SA' AND COMPANYID<>'" + strCompanyId + "' AND ISNULL(CANCEL,'') = '' AND TRANNO=" + txtAdvanceNo.Text.Substring(1).ToString()
                Dim dt As New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dt)
                Dim batchno As String = Nothing
                If Not dt.Rows.Count > 0 Then
                    MessageBox.Show("Invoice Info Not Found. Do you wish to continue?", "Invalid Invoice No Alert", MessageBoxButtons.OK, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)
                    MsgBox(E0004 + Me.GetNextControl(txtAdvanceNo, False).Text, MsgBoxStyle.Information)
                    Exit Sub
                ElseIf dt.Rows.Count = 1 Then
                    batchno = dt.Rows(0).Item("BATCHNO").ToString()
                Else
                    strSql = "SELECT BATCHNO,TRANNO,TRANDATE,COMPANYID,AMOUNT FROM " & cnStockDb & "..ISSUE"
                    strSql += vbCrLf + " WHERE TRANDATE='" + BILLDATE.ToString("yyyy-MM-dd") + "'"
                    strSql += vbCrLf + " AND TRANTYPE='SA' AND ISNULL(CANCEL,'') = '' AND COMPANYID<>'" + strCompanyId + "' AND TRANNO=" + txtAdvanceNo.Text.Substring(1).ToString()
                    batchno = BrighttechPack.SearchDialog.Show("Select INVOICE NO", strSql, cn, 2)
                End If
                Dim amount As Double = Nothing
                strSql = vbCrLf + " SELECT SUM(CASE WHEN TRANMODE ='C' THEN AMOUNT ELSE -1*AMOUNT END) AMOUNT FROM " & cnStockDb & "..ACCTRAN A "
                strSql += vbCrLf + " WHERE PAYMODE IN ('SA','SV','SC') AND BATCHNO = '" + batchno + "'"
                amount = objGPack.GetSqlValue(strSql, "AMOUNT", "0")
                strSql = vbCrLf + " SELECT 'Z'+CONVERT(VARCHAR(20),TRANNO) REFNO, " + amount.ToString() + " RECEIPT,NULL PAYMENT, " + amount.ToString() + " BALANCE  ,x.TRANDATE"
                strSql += vbCrLf + "   ,V.PREVILEGEID  ,V.ACCODE  ,V.TITLE  ,V.INITIAL  ,V.PNAME  ,V.DOORNO  ,V.ADDRESS1  ,V.ADDRESS2  ,V.ADDRESS3 "
                strSql += vbCrLf + "   ,V.AREA  ,V.CITY  ,V.STATE  ,V.COUNTRY  ,V.PINCODE  ,V.PHONERES  ,V.MOBILE  ,V.EMAIL  ,V.FAX  ,V.SNO"
                strSql += vbCrLf + "   ,X.COMPANYID FROM TEMP" & systemId & "_CUSTOMERINFO AS V INNER JOIN "
                strSql += vbCrLf + "   (SELECT DISTINCT COMPANYID,TRANNO,TRANDATE,BATCHNO FROM " & cnStockDb & "..ISSUE ) X ON V.BATCHNO = X.BATCHNO "
                strSql += vbCrLf + " WHERE v.BATCHNO  = '" + batchno + "'"
                dt = New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dt)
                If Not dt.Rows.Count > 0 Then
                    MessageBox.Show("Runno Info Not Found. Do you wish to continue?", "Invalid Runno Alert", MessageBoxButtons.OK, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)
                    MsgBox(E0004 + Me.GetNextControl(txtAdvanceNo, False).Text, MsgBoxStyle.Information)
                    Exit Sub
                Else
                    With dt.Rows(0)
                        dtpAdvanceDate.Value = .Item("TRANDATE")
                        txtAdvanceReceived_AMT.Text = IIf(Val(.Item("RECEIPT").ToString) <> 0, Format(Val(.Item("RECEIPT").ToString), "0.00"), Nothing)
                        txtAdvanceAdjusted_AMT.Text = IIf(Val(.Item("PAYMENT").ToString) <> 0, Format(Val(.Item("PAYMENT").ToString), "0.00"), Nothing)
                        txtAdvanceBalance_AMT.Text = IIf(Val(.Item("BALANCE").ToString) <> 0, Format(Val(.Item("BALANCE").ToString), "0.00"), Nothing)
                        txtAdvanceName.Text = .Item("PNAME").ToString
                        txtAdvanceAddress1.Text = .Item("DOORNO").ToString + " " + .Item("ADDRESS1").ToString
                        txtAdvanceAddress2.Text = .Item("ADDRESS2").ToString
                        txtAdvanceAddress3.Text = .Item("ADDRESS3").ToString
                        txtAdvanceAcCode.Text = .Item("ACCODE").ToString
                        txtAddressPrevilegeId.Text = .Item("PREVILEGEID").ToString
                        txtAddressPartyCode.Text = .Item("ACCODE").ToString
                        cmbAddressTitle_OWN.Text = .Item("TITLE").ToString
                        txtAddressInitial.Text = .Item("INITIAL").ToString
                        txtAddressName.Text = .Item("PNAME").ToString
                        txtAddressDoorNo.Text = .Item("DOORNO").ToString
                        txtAddress1.Text = .Item("ADDRESS1").ToString
                        txtAddress2.Text = .Item("ADDRESS2").ToString
                        txtAddress3.Text = .Item("ADDRESS3").ToString
                        cmbAddressArea_OWN.Text = .Item("AREA").ToString
                        cmbAddressCity_OWN.Text = .Item("CITY").ToString
                        cmbAddressState_OWN.Text = .Item("STATE").ToString
                        cmbAddressCountry_OWN.Text = .Item("COUNTRY").ToString
                        txtAddressPincode_NUM.Text = .Item("PINCODE").ToString
                        txtAddressEmailId_OWN.Text = .Item("EMAIL").ToString
                        txtAddressFax.Text = .Item("FAX").ToString
                        txtAddressPhoneRes.Text = .Item("PHONERES").ToString
                        txtAddressMobile.Text = .Item("MOBILE").ToString
                        txtAddressRegularSno.Text = .Item("SNO").ToString
                        txtAdvanceNo.Text = .Item("REFNO").ToString
                        txtAdvanceEntAdvanceNo.Text = txtAdvanceNo.Text
                        txtAdvanceEntAmount.Text = txtAdvanceBalance_AMT.Text
                        txtAdvanceCompanyId.Text = .Item("COMPANYID").ToString
                        txtAdvanceBalance_AMT.Focus()
                    End With
                End If
            End If
        End If
    End Sub

    Private Sub txtAdvanceNo_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAdvanceNo.Leave
        'strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "_CUSTOMERINFO') > 0"
        'strSql += " DROP VIEW TEMP" & systemId & "_CUSTOMERINFO"
        'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        'cmd.ExecuteNonQuery()

        'strSql = " CREATE VIEW TEMP" & systemId & "_CUSTOMERINFO"
        'strSql += " AS"
        'strSql += " SELECT C.PSNO as SNO,C.BATCHNO,TRANDATE,PREVILEGEID,ACCODE,TITLE,INITIAL,PNAME,DOORNO,ADDRESS1,ADDRESS2,ADDRESS3,AREA,CITY,STATE,COUNTRY,PINCODE,PHONERES,MOBILE,EMAIL,FAX,C.REMARK1"
        'strSql += " FROM " & cnAdminDb & "..PERSONALINFO P INNER JOIN " & cnAdminDb & "..CUSTOMERINFO C ON P.SNO = C.PSNO"
        'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        'cmd.ExecuteNonQuery()

        'strSql = " SELECT SUBSTRING(RUNNO,6,20) REFNO"
        'If ISMAINT_CENTADV Then strSql += ", SUBSTRING(RUNNO,1,2) AS COSTID"
        'strSql += " ,CASE WHEN RECEIPT <> 0 THEN RECEIPT ELSE NULL END AS RECEIPT"
        'strSql += " ,CASE WHEN PAYMENT <> 0 THEN PAYMENT ELSE NULL END AS PAYMENT"
        'strSql += " ,CASE WHEN BALANCE <> 0 THEN BALANCE ELSE NULL END AS BALANCE"
        'strSql += " ,x.TRANDATE,V.PREVILEGEID,V.ACCODE ACCODE,X.ACCODE ADVCODE,V.TITLE,V.INITIAL,V.PNAME,V.DOORNO,V.ADDRESS1,V.ADDRESS2,V.ADDRESS3"
        'strSql += " ,V.AREA,V.CITY,V.STATE,V.COUNTRY,V.PINCODE,V.PHONERES,V.MOBILE,V.EMAIL,V.FAX,V.SNO,X.COMPANYID,V.REMARK1"
        'strSql += " ,RUNNO AS ORGRUNNO,X.BATCHNO AS ORGBATCHNO  FROM"
        'strSql += " ("
        'strSql += " SELECT RUNNO"
        'strSql += " ,SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE 0 END)AS RECEIPT"
        'strSql += " ,SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE 0 END)AS PAYMENT"
        'strSql += " ,SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE -1*AMOUNT END)AS BALANCE"
        'strSql += " ,(SELECT TOP 1 TRANDATE FROM " & cnAdminDb & "..OUTSTANDING WHERE " & IIf(ISMAINT_CENTADV, "", "COSTID = '" & BillCostId & "' AND ") & " RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  AND ISNULL(CANCEL,'') = '' ORDER BY TRANDATE,SNO DESC)AS TRANDATE"
        'strSql += " ,(SELECT TOP 1 BATCHNO FROM " & cnAdminDb & "..OUTSTANDING WHERE " & IIf(ISMAINT_CENTADV, "", "COSTID = '" & BillCostId & "' AND ") & " RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  AND ISNULL(CANCEL,'') = '' ORDER BY TRANDATE,SNO DESC)AS BATCHNO"
        'strSql += " ,O.ACCODE ACCODE,O.COMPANYID"
        'strSql += " FROM " & cnAdminDb & "..OUTSTANDING O"
        'strSql += " WHERE (RUNNO = '" & GetCostId(BillCostId) & GetCompanyId(strCompanyId) & txtAdvanceNo.Text & "' OR (TRANSTATUS = 'T' AND RUNNO LIKE '%" & GetCompanyId(strCompanyId) & txtAdvanceNo.Text & "'))" ' RUNNO = '" & GetCostId(BillCostId) & GetCompanyId(strCompanyId) & txtAdvanceNo.Text & "'"
        'strSql += " AND TRANTYPE = 'A' "
        'If Not withOrderDetail Then strSql += " AND SUBSTRING(RUNNO,6,20) NOT LIKE 'O%'"
        'If Not withRepairDetail Then strSql += " AND SUBSTRING(RUNNO,6,20) NOT LIKE 'R%'"
        'strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
        'If Not ISMAINT_CENTADV Then strSql += vbCrLf + " AND COSTID = '" & BillCostId & "'"
        'strSql += " AND ISNULL(CANCEL,'') = ''"
        'strSql += " GROUP BY RUNNO,COMPANYID,O.ACCODE"
        'strSql += " )X"
        'strSql += " INNER JOIN TEMP" & systemId & "_CUSTOMERINFO AS V ON V.BATCHNO = X.BATCHNO"
        'Dim dt As New DataTable
        'da = New OleDbDataAdapter(strSql, cn)
        'da.Fill(dt)
        'If Not dt.Rows.Count > 0 Then
        '    If RunNoCheck = False Then
        '        If RunNoChecked = True Then Exit Sub
        '        If MessageBox.Show("Runno Info Not Found. Do you wish to continue?", "Invalid Runno Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
        '            txtAdvanceNo.Focus()
        '            Exit Sub
        '        Else
        '            RunNoChecked = True
        '            txtAdvanceBalance_AMT.Focus()
        '        End If
        '        Exit Sub
        '    End If
        '    MsgBox(E0004 + Me.GetNextControl(txtAdvanceNo, False).Text, MsgBoxStyle.Information)
        '    txtAdvanceNo.Focus()
        '    Exit Sub
        'End If
    End Sub
    Private Sub txtAdvanceNo_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAdvanceNo.LostFocus
        Exit Sub
        If ADVADJDAYS <> 0 Then
            If DateDiff(DateInterval.Day, dtpAdvanceDate.Value, BILLDATE) > ADVADJDAYS Then
                Dim pwdid As Integer = 0 : Dim pwdpass As Boolean = False
                Dim Sqlqry As String = "Select Optionid from " & cnAdminDb & "..PRJPWDOPTION where OPTIONNAME ='ADVADJ' AND active = 'Y'"
                Dim Optionid As Integer = Val("" & objGPack.GetSqlValue(Sqlqry, , , tran))
                If userId <> 999 And Optionid <> 0 Then
                    pwdid = GetuserPwd(Optionid, cnCostId, userId)
                    If pwdid <> 0 Then
                        Dim objUpwd As New frmUserPassword(pwdid, Optionid)
                        If objUpwd.ShowDialog <> Windows.Forms.DialogResult.OK Then MsgBox("Access Denied.") : Exit Sub Else pwdpass = True
                    Else
                        Dim objUpwd As New frmUserPassword(pwdid, Optionid)
                        If objUpwd.ShowDialog <> Windows.Forms.DialogResult.OK Then
                            MsgBox("Access Denied.")
                            Exit Sub
                        Else
                            pwdpass = True
                        End If
                    End If
                Else
                    pwdpass = True
                End If
                If pwdpass = False And txtAdvanceNo.Text <> "" Then
                    'If Authorize Then
                    '        Dim objSecret As New frmAdminPassword()
                    '        If objSecret.ShowDialog <> Windows.Forms.DialogResult.OK Then MsgBox("Access Denied.") : Exit Sub
                    '    Else
                    txtAdvanceNo.Text = ""
                    txtAdvanceAdjusted_AMT.Text = ""
                    txtAdvanceNo.Focus()
                    txtAdvanceBalance_AMT.Text = ""
                    txtAdvanceReceived_AMT.Text = ""
                    MsgBox("Access Denied.")
                    Exit Sub
                    '    End If
                End If
            End If
        End If

        Main.HideHelpText()
    End Sub
    Private Sub txtAdvanceReceived_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAdvanceReceived_AMT.GotFocus
        SendKeys.Send("{TAB}")
    End Sub
    Private Sub dtpAdvanceDate_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        SendKeys.Send("{TAB}")
    End Sub


    Private Sub frmAdvanceAdj_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtAdvanceNo.Focused Then Exit Sub
            If txtAdvanceBalance_AMT.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub gridAdvance_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridAdvance.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            If gridAdvance.RowCount > 0 Then
                gridAdvance.CurrentCell = gridAdvance.Rows(gridAdvance.CurrentRow.Index).Cells("RUNNO")
                With gridAdvance.Rows(gridAdvance.CurrentRow.Index)
                    If .Cells("ISEDIT").Value.ToString = "N" Then
                        MsgBox("Booked advance info cannot modify", MsgBoxStyle.Information)
                        Exit Sub
                    End If
                    txtAdvanceNo.Text = .Cells("RUNNO").Value.ToString
                    txtAdvanceEntAdvanceNo.Text = .Cells("RUNNO").Value.ToString
                    dtpAdvanceDate.Value = .Cells("DATE").Value
                    txtAdvanceReceived_AMT.Text = .Cells("RECEIVEDAMT").FormattedValue
                    txtAdvanceAdjusted_AMT.Text = .Cells("ADJUSTEDAMT").FormattedValue
                    txtAdvanceBalance_AMT.Text = .Cells("AMOUNT").FormattedValue
                    txtAdvanceEntAmount.Text = .Cells("AMOUNT").FormattedValue
                    txtAdvanceName.Text = .Cells("PNAME").Value.ToString
                    txtAdvanceAddress1.Text = .Cells("ADDRESS1").Value.ToString
                    txtAdvanceAddress2.Text = .Cells("ADDRESS2").Value.ToString
                    txtAdvanceAddress3.Text = .Cells("ADDRESS3").Value.ToString
                    txtAdvanceRefNo.Text = .Cells("REFNO").Value.ToString
                    txtAdvanceAcCode.Text = .Cells("ACCODE").Value.ToString
                    txtAdvanceCompanyId.Text = .Cells("COMPANYID").Value.ToString
                    If .Cells("COSTID").Value.ToString <> "" Then
                        txtAdvCostid.Text = .Cells("COSTID").Value.ToString
                    End If
                    txtAdvanceRowIndex.Text = gridAdvance.CurrentRow.Index
                    txtAdvanceNo.Select()
                End With
            End If
        ElseIf e.KeyCode = Keys.Delete Then
            If Not gridAdvance.RowCount > 0 Then Exit Sub
            If gridAdvance.CurrentRow.Cells("ISEDIT").Value.ToString = "N" Then
                MsgBox("Booked advance info cannot modify", MsgBoxStyle.Information)
                Exit Sub
            End If
            dtGridAdvance.Rows.RemoveAt(gridAdvance.CurrentRow.Index)
            dtGridAdvance.AcceptChanges()
            CalcGridAdvanceTotal()
            If Not gridAdvance.RowCount > 0 Then
                txtAdvanceNo.Select()
            End If
        End If
    End Sub

    Private Sub frmAdvanceAdj_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GstPer = Val(objGPack.GetSqlValue("SELECT STAX FROM " & cnAdminDb & "..TAXMAST WHERE TAXCODE='GT'", , "0").ToString)
        If gridAdvance.Rows.Count > 0 Then Exit Sub
        txtAdvanceNo.Text = "A" + Mid(Format(cnTranToDate, "dd/MM/yyyy"), 9, 2).ToString
        RunNoChecked = False
        txtAdvanceNo.Focus()
        txtAdvanceNo.SelectionStart = txtAdvanceNo.TextLength
    End Sub

    Private Sub txtAdvanceBalance_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAdvanceBalance_AMT.TextChanged
        If GST And GstFlag Then
            Dim Per As Decimal = 103
            'If GSTADVCALC_INCL = "I" Then Per = 103
            Dim GstAmt As Double
            Dim SGstAmt As Double = (Val(txtAdvanceBalance_AMT.Text) * (GstPer / 2) / Per)
            Dim CGstAmt As Double = (Val(txtAdvanceBalance_AMT.Text) * (GstPer / 2) / Per)
            SGstAmt = Math.Round(SGstAmt, 2)
            CGstAmt = Math.Round(CGstAmt, 2)
            SGstAmt = CalcRoundoffAmt(SGstAmt, Round_Gst)
            CGstAmt = CalcRoundoffAmt(CGstAmt, Round_Gst)

            GstAmt = SGstAmt + CGstAmt
            txtAdvanceGST_AMT.Text = IIf(GstAmt <> 0, Format(GstAmt, "0.00"), Nothing)
            Dim TotAmt As Double = Val(txtAdvanceBalance_AMT.Text)
            TotAmt -= Val(txtAdvanceGST_AMT.Text)
            txtAdvanceAmt_AMT.Text = IIf(TotAmt <> 0, Format(TotAmt, "0.00"), Nothing)
        End If
    End Sub
End Class