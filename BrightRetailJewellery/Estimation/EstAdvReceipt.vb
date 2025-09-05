Imports System.Data.OleDb
Public Class EstAdvReceipt

    Public Enum ReceiptType
        CREDIT_SALES = 0
        CUSTOMER_ADVANCE = 1
        OTHER_RECEIPTS = 2
        CHIT_RECEIPT = 3
        FURTHER_ADVANCE = 4
    End Enum

    Public dtGridReceipt As New DataTable
    Public dtGridReserve As New DataTable
    Public objSoftKeys As New SoftKeys
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim strsql As String
    Dim dtSoftKeys As DataTable = GetAdmindbSoftValueAll()
    Dim ADV_RATE_METAL As String = GetAdmindbSoftValuefromDt(dtSoftKeys, "ADV_RATE_METAL", "G")
    Dim RATE_DIFF_HC As String = GetAdmindbSoftValuefromDt(dtSoftKeys, "RATE_DIFF_CHARGE", "N")
    Dim ADVFIXWTPERSTR As String = GetAdmindbSoftValuefromDt(dtSoftKeys, "ADVFIXWTPER", "100")
    Dim ADVFIXWTPER As Decimal = 0
    Dim objAddressDia As New frmAddressDia(True)
    Public Billdate As Date
    Public Billcostid As String


    



    Private Sub cmbReceiptReceiptType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbReceiptReceiptType.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If gridReceipt.Rows(0).Cells("RECEIPTTYPE").Value.ToString <> "" Then
                If gridReceipt.Rows(0).Cells("RECEIPTTYPE").Value.ToString <> cmbReceiptReceiptType.Text Then
                    MsgBox("Multiple Transaction not Allowed", MsgBoxStyle.Information)
                    Exit Sub
                ElseIf gridReceipt.Rows(0).Cells("RECEIPTTYPE").Value.ToString.ToUpper = ReceiptType.CUSTOMER_ADVANCE.ToString.ToUpper Then
                    If cmbReceiptReceiptType.Text = ReceiptType.CUSTOMER_ADVANCE.ToString Then
                        MsgBox("Multiple Customer Advance not Allowed", MsgBoxStyle.Information)
                        Exit Sub
                    End If
                ElseIf gridReceipt.Rows(0).Cells("RECEIPTTYPE").Value.ToString.ToUpper = ReceiptType.FURTHER_ADVANCE.ToString.ToUpper Then
                    If cmbReceiptReceiptType.Text = ReceiptType.FURTHER_ADVANCE.ToString Then
                        MsgBox("Multiple Further Advance not Allowed", MsgBoxStyle.Information)
                        Exit Sub
                    End If
                End If
            End If
            If cmbReceiptReceiptType.Text = ReceiptType.CREDIT_SALES.ToString Then
                txtReceiptRefNo.Select()
            ElseIf cmbReceiptReceiptType.Text = ReceiptType.CUSTOMER_ADVANCE.ToString Then
                txtReceiptAmount_AMT.Select()
            ElseIf cmbReceiptReceiptType.Text = ReceiptType.FURTHER_ADVANCE.ToString Then
                txtReceiptRefNo.Select()
                
            End If

        End If
    End Sub

    Private Sub cmbReceiptTranType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbReceiptTranType.GotFocus
        If cmbReceiptReceiptType.Text <> ReceiptType.OTHER_RECEIPTS.ToString Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub cmbReceiptAccount_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbReceiptAccount.GotFocus
        If cmbReceiptReceiptType.Text <> ReceiptType.OTHER_RECEIPTS.ToString Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtReceiptRefNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtReceiptRefNo.GotFocus
        If cmbReceiptReceiptType.Text <> ReceiptType.CREDIT_SALES.ToString And cmbReceiptReceiptType.Text <> ReceiptType.FURTHER_ADVANCE.ToString Then
            SendKeys.Send("{TAB}")
        Else
            ShowToolTip("Press Insert Key to Help", txtReceiptRefNo)
        End If
    End Sub

    Private Sub txtReceiptRate_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtReceiptRate_AMT.GotFocus
        If cmbReceiptReceiptType.Text = ReceiptType.CUSTOMER_ADVANCE.ToString Then
            strSql = " SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE PURITYID = "
            strSql += " (SELECT TOP 1 PURITYID FROM " & cnAdminDb & "..PURITYMAST WHERE PURITY BETWEEN 91.6 AND 92 "
            If ADV_RATE_METAL <> "N" And Not ADV_RATE_METAL Is Nothing Then strSql += " AND METALID='" & ADV_RATE_METAL & "'"
            strSql += " ORDER BY PURITY DESC)"
            'strSql += " (SELECT MAX(PURITY) FROM " & cnAdminDb & "..PURITYMAST ))"
            Dim purity As Double = Val(GetRate(BillDate, objGPack.GetSqlValue(strSql)))
            txtReceiptRate_AMT.Text = IIf(purity <> 0, Format(purity, "0.00"), Nothing)
        Else
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub chkReceiptRateFix_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkReceiptRateFix.GotFocus
        If cmbReceiptReceiptType.Text <> ReceiptType.CUSTOMER_ADVANCE.ToString Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtReceiptRefNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtReceiptRefNo.KeyDown
        If e.KeyCode = Keys.Insert Then
            strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "_CUSTOMERINFO') > 0"
            strSql += vbCrLf + " DROP VIEW TEMP" & systemId & "_CUSTOMERINFO"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()

            strSql = " CREATE VIEW TEMP" & systemId & "_CUSTOMERINFO"
            strSql += vbCrLf + " AS"
            strSql += vbCrLf + " SELECT C.PSNO as SNO,C.BATCHNO,TRANDATE,PREVILEGEID,ACCODE,TITLE,INITIAL,PNAME,DOORNO,ADDRESS1,ADDRESS2,ADDRESS3,AREA,CITY,STATE,COUNTRY,PINCODE,PHONERES,MOBILE,EMAIL,FAX"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..PERSONALINFO P INNER JOIN " & cnAdminDb & "..CUSTOMERINFO C ON P.SNO = C.PSNO"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()

            Dim row As DataRow = Nothing
            If cmbReceiptReceiptType.Text = ReceiptType.CREDIT_SALES.ToString Then
                strSql = " SELECT SUBSTRING(RUNNO,6,20) REFNO,TRANNO"
                strSql += vbCrLf + " ,CASE WHEN PAYMENT <> 0 THEN PAYMENT ELSE NULL END AS PAYMENT" + vbCrLf
                strSql += vbCrLf + " ,CASE WHEN RECEIPT <> 0 THEN RECEIPT ELSE NULL END AS RECEIPT" + vbCrLf
                strSql += vbCrLf + " ,CASE WHEN BALANCE <> 0 THEN BALANCE ELSE NULL END AS BALANCE" + vbCrLf
                strSql += vbCrLf + " ,x.TRANDATE,V.PREVILEGEID,V.ACCODE,V.TITLE,V.INITIAL,V.PNAME,V.DOORNO,V.ADDRESS1,V.ADDRESS2,V.ADDRESS3" + vbCrLf
                strSql += vbCrLf + " ,V.AREA,V.CITY,V.STATE,V.COUNTRY,V.PINCODE,V.PHONERES,V.MOBILE,V.EMAIL,V.FAX,V.SNO" + vbCrLf
                strSql += vbCrLf + " FROM" + vbCrLf
                strSql += vbCrLf + " (" + vbCrLf
                strSql += vbCrLf + " SELECT RUNNO" + vbCrLf
                strSql += vbCrLf + " ,(SELECT TOP 1 TRANNO FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO = O.RUNNO AND COSTID = '" & BillCostId & "' AND COMPANYID = O.COMPANYID AND ISNULL(FROMFLAG,'') NOT IN ('','S') AND RECPAY = 'P' AND ISNULL(CANCEL,'') = ''  ORDER BY TRANDATE)AS TRANNO" + vbCrLf
                strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE 0 END)AS PAYMENT" + vbCrLf
                strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE 0 END)AS RECEIPT" + vbCrLf
                strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE -1*AMOUNT END)AS BALANCE" + vbCrLf
                strSql += vbCrLf + " ,(SELECT TOP 1 TRANDATE FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO = O.RUNNO AND COSTID = '" & BillCostId & "' AND COMPANYID = O.COMPANYID AND ISNULL(FROMFLAG,'') NOT IN ('','S') AND RECPAY = 'P' AND ISNULL(CANCEL,'') = '' ORDER BY TRANDATE)AS TRANDATE" + vbCrLf
                strSql += vbCrLf + " ,(SELECT TOP 1 BATCHNO FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO = O.RUNNO AND COSTID = '" & BillCostId & "' AND COMPANYID = O.COMPANYID AND ISNULL(FROMFLAG,'') NOT IN ('','S') AND RECPAY = 'P' AND ISNULL(CANCEL,'') = ''  ORDER BY TRANDATE)AS BATCHNO" + vbCrLf
                strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING O" + vbCrLf
                strSql += vbCrLf + " WHERE TRANTYPE = 'D'" + vbCrLf
                strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''" + vbCrLf
                strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
                strSql += vbCrLf + " AND ISNULL(COSTID,'') = '" & BillCostId & "'"
                strSql += vbCrLf + " AND ISNULL(FROMFLAG,'') NOT IN ('','S')"
                strSql += vbCrLf + " GROUP BY RUNNO,COMPANYID" + vbCrLf
                strSql += vbCrLf + " HAVING SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE -1*AMOUNT END) > 0 " + vbCrLf
                strSql += vbCrLf + " )X" + vbCrLf
                'strSql += vbcrlf + " LEFT OUTER JOIN " & cnAdminDb & "..CUSTOMERINFO AS C ON C.BATCHNO = X.BATCHNO"
                'strSql += vbcrlf + " LEFT OUTER JOIN " & cnStockDb & "..PERSONALINFO AS V ON V.SNO = C.PSNO"
                strSql += vbCrLf + " INNER JOIN TEMP" & systemId & "_CUSTOMERINFO AS V ON V.BATCHNO = X.BATCHNO" + vbCrLf
                row = BrighttechPack.SearchDialog.Show_R("Select Reference No", strSql, cn, 3)
            Else 'Further Advance
                strSql = "   SELECT DISTINCT"
                strSql += vbCrLf + "   SUBSTRING(ORNO,6,20) REFNO"
                strSql += vbCrLf + "   ,ORDATE AS TRANDATE,V.PREVILEGEID,V.ACCODE,V.TITLE,V.INITIAL,V.PNAME,V.DOORNO,V.ADDRESS1,V.ADDRESS2,V.ADDRESS3"
                strSql += vbCrLf + "   ,V.AREA,V.CITY,V.STATE,V.COUNTRY,V.PINCODE,V.PHONERES,V.MOBILE,V.EMAIL,V.FAX,V.SNO"
                strSql += vbCrLf + "   FROM " & cnadmindb & "..ORMAST AS O "
                'strSql += vbcrlf + " LEFT OUTER JOIN " & cnAdminDb & "..CUSTOMERINFO AS C ON C.BATCHNO = O.BATCHNO"
                'strSql += vbcrlf + " LEFT OUTER JOIN " & cnStockDb & "..PERSONALINFO AS V ON V.SNO = C.PSNO"
                strsql += vbCrLf + "   INNER JOIN TEMP" & systemId & "_CUSTOMERINFO AS V ON O.BATCHNO = V.BATCHNO"
                strsql += vbCrLf + "   WHERE ISNULL(COSTID,'') = '" & Billcostid & "'"
                strsql += vbCrLf + "   AND COMPANYID = '" & strCompanyId & "' AND ISNULL(O.ORDCANCEL,'') = ''"
                strsql += vbCrLf + "   AND ISNULL(O.ODBATCHNO,'') = '' AND ISNULL(O.ODSNO,'') = ''"
                row = BrighttechPack.SearchDialog.Show_R("Select Reference No", strsql, cn, 0)
            End If
            If Not row Is Nothing Then
                For Each roRec As DataRow In dtGridReceipt.Rows
                    If row.Item("REFNO").ToString = roRec!REFNO.ToString Then
                        MsgBox("Already Exist this RefNo", MsgBoxStyle.Information)
                        txtReceiptRefNo.Select()
                        txtReceiptRefNo.SelectAll()
                        Exit Sub
                    End If
                Next
                With objAddressDia
                    .AddressLock = True
                    '.chkRegularCustomer.Checked = True
                    '.chkRegularCustomer.Enabled = False
                    .txtAddressPrevilegeId.Text = row.Item("PREVILEGEID").ToString
                    .txtAddressPartyCode.Text = row.Item("ACCODE").ToString
                    .cmbAddressTitle_OWN.Text = row.Item("TITLE").ToString
                    .txtAddressInitial.Text = row.Item("INITIAL").ToString
                    .txtAddressName.Text = row.Item("PNAME").ToString
                    .txtAddressDoorNo.Text = row.Item("DOORNO").ToString
                    .txtAddress1.Text = row.Item("ADDRESS1").ToString
                    .txtAddress2.Text = row.Item("ADDRESS2").ToString
                    .txtAddress3.Text = row.Item("ADDRESS3").ToString
                    .cmbAddressArea_OWN.Text = row.Item("AREA").ToString
                    .cmbAddressCity_OWN.Text = row.Item("CITY").ToString
                    .cmbAddressState.Text = row.Item("STATE").ToString
                    .cmbAddressCountry_OWN.Text = row.Item("COUNTRY").ToString
                    .txtAddressPincode_NUM.Text = row.Item("PINCODE").ToString
                    .txtAddressEmailId_OWN.Text = row.Item("EMAIL").ToString
                    .txtAddressFax.Text = row.Item("FAX").ToString
                    .txtAddressPhoneRes.Text = row.Item("PHONERES").ToString
                    .txtAddressMobile.Text = row.Item("MOBILE").ToString
                    .txtAddressRegularSno.Text = row.Item("SNO").ToString
                End With
                If cmbReceiptReceiptType.Text = ReceiptType.CREDIT_SALES.ToString Then
                    txtReceiptAmount_AMT.Text = row.Item("BALANCE").ToString
                    txtReceiptEntAmount.Text = txtReceiptAmount_AMT.Text
                End If
                txtReceiptRefNo.Text = row.Item("REFNO").ToString
                txtReceiptEntRefNo.Text = txtReceiptRefNo.Text

                If cmbReceiptReceiptType.Text = ReceiptType.CREDIT_SALES.ToString Then
                    strsql = " SELECT * FROM "
                    strsql += vbCrLf + " ("
                    strsql += vbCrLf + " SELECT 'TRUE' [CHECK],REMARK1 ITEM"
                    strsql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN PCS ELSE -1*PCS END)PCS"
                    strsql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN GRSWT ELSE -1*GRSWT END)GRSWT"
                    strsql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN NETWT ELSE -1*NETWT END)NETWT"
                    strsql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE -1*AMOUNT END)VALUE"
                    strsql += vbCrLf + " ,REMARK2 REMARK,SUBSTRING(RUNNO,6,20)RUNNO,ISSSNO FROM " & cnAdminDb & "..ITEMDETAIL"
                    ' BELOW LINE CHANGE BY KALAI SIR
                    strsql += vbCrLf + " WHERE RUNNO = '" & GetCostId(Billcostid) & GetCompanyId(strCompanyId) & txtReceiptRefNo.Text & "'"
                    strsql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
                    strsql += vbCrLf + " GROUP BY REMARK2,REMARK1,RUNNO,ISSSNO"
                    strsql += vbCrLf + " )X"
                    strsql += vbCrLf + " WHERE NOT(PCS = 0 AND GRSWT = 0 AND NETWT = 0 AND VALUE = 0)"
                    'objToBeRecDetail.dtToBeReceipt.Rows.Clear()
                    da = New OleDbDataAdapter(strsql, cn)
                    'da.Fill(objToBeRecDetail.dtToBeReceipt)
                    ' If objToBeRecDetail.dtToBeReceipt.Rows.Count > 0 Then
                    '  ShowToBeReceiptDetail()
                    ' End If
                    txtReceiptAmount_AMT.Select()
                    txtReceiptAmount_AMT.SelectAll()
                ElseIf cmbReceiptReceiptType.Text = ReceiptType.FURTHER_ADVANCE.ToString Then
                    'ShowOrderAdvance()
                End If
            End If
        End If
    End Sub

    Private Sub txtReceiptRefNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtReceiptRefNo.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            ''VALIDATION
            If txtReceiptRefNo.Text = "" Then
                MsgBox("SHOULD NOT EMPTY")
                Exit Sub
            End If
            For Each roRec As DataRow In dtGridReceipt.Rows
                If txtReceiptRefNo.Text = roRec!REFNO.ToString Then
                    MsgBox("Already Exist this RefNo", MsgBoxStyle.Information)
                    txtReceiptRefNo.Select()
                    txtReceiptRefNo.SelectAll()
                    Exit Sub
                End If
            Next
            strsql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "_CUSTOMERINFO') > 0"
            strsql += " DROP VIEW TEMP" & systemId & "_CUSTOMERINFO"
            cmd = New OleDbCommand(strsql, cn)
            cmd.ExecuteNonQuery()

            strsql = " CREATE VIEW TEMP" & systemId & "_CUSTOMERINFO"
            strsql += " AS"
            strsql += " SELECT C.PSNO as SNO,C.BATCHNO,TRANDATE,PREVILEGEID,ACCODE,TITLE,INITIAL,PNAME,DOORNO,ADDRESS1,ADDRESS2,ADDRESS3,AREA,CITY,STATE,COUNTRY,PINCODE,PHONERES,MOBILE,EMAIL,FAX"
            strsql += " FROM " & cnAdminDb & "..PERSONALINFO P INNER JOIN " & cnAdminDb & "..CUSTOMERINFO C ON P.SNO = C.PSNO"
            cmd = New OleDbCommand(strsql, cn)
            cmd.ExecuteNonQuery()
            If cmbReceiptReceiptType.Text = ReceiptType.CREDIT_SALES.ToString Then
                strsql = " SELECT SUBSTRING(RUNNO,6,20) REFNO"
                strsql += " ,CASE WHEN PAYMENT <> 0 THEN PAYMENT ELSE NULL END AS PAYMENT"
                strsql += " ,CASE WHEN RECEIPT <> 0 THEN RECEIPT ELSE NULL END AS RECEIPT"
                strsql += " ,CASE WHEN BALANCE <> 0 THEN BALANCE ELSE NULL END AS BALANCE"
                strsql += " ,x.TRANDATE,V.PREVILEGEID,V.ACCODE,V.TITLE,V.INITIAL,V.PNAME,V.DOORNO,V.ADDRESS1,V.ADDRESS2,V.ADDRESS3"
                strsql += " ,V.AREA,V.CITY,V.STATE,V.COUNTRY,V.PINCODE,V.PHONERES,V.MOBILE,V.EMAIL,V.FAX,V.SNO"
                strsql += " FROM"
                strsql += " ("
                strsql += " SELECT RUNNO"
                strsql += " ,SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE 0 END)AS PAYMENT"
                strsql += " ,SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE 0 END)AS RECEIPT"
                strsql += " ,SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE -1*AMOUNT END)AS BALANCE"
                strsql += " ,(SELECT TOP 1 TRANDATE FROM " & cnAdminDb & "..OUTSTANDING WHERE COSTID = '" & Billcostid & "' AND RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID AND ISNULL(FROMFLAG,'') NOT IN ('','S')  AND ISNULL(CANCEL,'') = '' ORDER BY TRANDATE,SNO)AS TRANDATE"
                strsql += " ,(SELECT TOP 1 BATCHNO FROM " & cnAdminDb & "..OUTSTANDING WHERE COSTID = '" & Billcostid & "' AND RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID AND ISNULL(FROMFLAG,'') NOT IN ('','S')  AND ISNULL(CANCEL,'') = '' ORDER BY TRANDATE,SNO DESC)AS BATCHNO"
                strsql += " FROM " & cnAdminDb & "..OUTSTANDING O"
                strsql += " WHERE RUNNO = '" & GetCostId(Billcostid) & GetCompanyId(strCompanyId) & txtReceiptRefNo.Text & "' AND  TRANTYPE = 'D'"
                strsql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
                strsql += " AND ISNULL(COSTID,'') = '" & Billcostid & "'"
                strsql += " AND ISNULL(CANCEL,'') = ''"
                strsql += " AND ISNULL(FROMFLAG,'') NOT IN ('','S')"
                strsql += " GROUP BY RUNNO,COMPANYID"
                strsql += " HAVING SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE -1*AMOUNT END) > 0 "
                strsql += " )X"
                strsql += " INNER JOIN TEMP" & systemId & "_CUSTOMERINFO AS V ON V.BATCHNO = X.BATCHNO"
            Else 'FURTHER ADVANCE
                strsql = "   SELECT DISTINCT"
                strsql += "   SUBSTRING(ORNO,6,20) REFNO"
                strsql += "   ,ORDATE AS TRANDATE,V.PREVILEGEID,V.ACCODE,V.TITLE,V.INITIAL,V.PNAME,V.DOORNO,V.ADDRESS1,V.ADDRESS2,V.ADDRESS3"
                strsql += "   ,V.AREA,V.CITY,V.STATE,V.COUNTRY,V.PINCODE,V.PHONERES,V.MOBILE,V.EMAIL,V.FAX,V.SNO"
                strsql += "   FROM " & cnAdminDb & "..ORMAST AS O INNER JOIN TEMP" & systemId & "_CUSTOMERINFO AS V ON O.BATCHNO = V.BATCHNO"
                strsql += "  WHERE ORNO = '" & GetCostId(Billcostid) & GetCompanyId(strCompanyId) & txtReceiptRefNo.Text & "'"
                strsql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
                strsql += "  AND ISNULL(COSTID,'') = '" & Billcostid & "'"
                strsql += " AND ISNULL(O.ORDCANCEL,'') = ''"
                strsql += vbCrLf + "   AND ISNULL(O.ODBATCHNO,'') = '' AND ISNULL(O.ODSNO,'') = ''"
            End If
            Dim dt As New DataTable
            da = New OleDbDataAdapter(strsql, cn)
            da.Fill(dt)
            If Not dt.Rows.Count > 0 Then
                MsgBox(E0004 + Me.GetNextControl(txtReceiptRefNo, False).Text, MsgBoxStyle.Information)
                txtReceiptRefNo.Select()
                Exit Sub
            Else
                With dt.Rows(0)
                    Me.SelectNextControl(txtReceiptRefNo, True, True, True, True)
                    If cmbReceiptReceiptType.Text = ReceiptType.CREDIT_SALES.ToString Then
                        txtReceiptAmount_AMT.Text = .Item("BALANCE").ToString
                        txtReceiptEntAmount.Text = txtReceiptAmount_AMT.Text
                    End If
                    txtReceiptEntRefNo.Text = txtReceiptRefNo.Text
                    objAddressDia.AddressLock = True
                    'objAddressDia.chkRegularCustomer.Checked = True
                    'objAddressDia.chkRegularCustomer.Enabled = False
                    objAddressDia.txtAddressPrevilegeId.Text = .Item("PREVILEGEID").ToString
                    objAddressDia.txtAddressPartyCode.Text = .Item("ACCODE").ToString
                    objAddressDia.cmbAddressTitle_OWN.Text = .Item("TITLE").ToString
                    objAddressDia.txtAddressInitial.Text = .Item("INITIAL").ToString
                    objAddressDia.txtAddressName.Text = .Item("PNAME").ToString
                    objAddressDia.txtAddressDoorNo.Text = .Item("DOORNO").ToString
                    objAddressDia.txtAddress1.Text = .Item("ADDRESS1").ToString
                    objAddressDia.txtAddress2.Text = .Item("ADDRESS2").ToString
                    objAddressDia.txtAddress3.Text = .Item("ADDRESS3").ToString
                    objAddressDia.cmbAddressArea_OWN.Text = .Item("AREA").ToString
                    objAddressDia.cmbAddressCity_OWN.Text = .Item("CITY").ToString
                    objAddressDia.cmbAddressState.Text = .Item("STATE").ToString
                    objAddressDia.cmbAddressCountry_OWN.Text = .Item("COUNTRY").ToString
                    objAddressDia.txtAddressPincode_NUM.Text = .Item("PINCODE").ToString
                    objAddressDia.txtAddressEmailId_OWN.Text = .Item("EMAIL").ToString
                    objAddressDia.txtAddressFax.Text = .Item("FAX").ToString
                    objAddressDia.txtAddressPhoneRes.Text = .Item("PHONERES").ToString
                    objAddressDia.txtAddressMobile.Text = .Item("MOBILE").ToString
                    objAddressDia.txtAddressRegularSno.Text = .Item("SNO").ToString
                End With
                If cmbReceiptReceiptType.Text = ReceiptType.CREDIT_SALES.ToString Then
                    strsql = " SELECT * FROM "
                    strsql += " ("
                    strsql += " SELECT 'TRUE' [CHECK],REMARK1 ITEM"
                    strsql += " ,SUM(CASE WHEN RECPAY = 'R' THEN PCS ELSE -1*PCS END)PCS"
                    strsql += " ,SUM(CASE WHEN RECPAY = 'R' THEN GRSWT ELSE -1*GRSWT END)GRSWT"
                    strsql += " ,SUM(CASE WHEN RECPAY = 'R' THEN NETWT ELSE -1*NETWT END)NETWT"
                    strsql += " ,SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE -1*AMOUNT END)VALUE"
                    strsql += " ,REMARK2 REMARK,SUBSTRING(RUNNO,6,20)RUNNO,ISSSNO FROM " & cnAdminDb & "..ITEMDETAIL"
                    strsql += " WHERE RUNNO = '" & GetCostId(Billcostid) & GetCompanyId(strCompanyId) & txtReceiptRefNo.Text & "'"
                    strsql += " AND ISNULL(CANCEL,'') = ''"
                    strsql += " GROUP BY REMARK2,REMARK1,RUNNO,ISSSNO"
                    strsql += " )X"
                    strsql += " WHERE NOT(PCS = 0 AND GRSWT = 0 AND NETWT = 0 AND VALUE = 0)"
                    ' objToBeRecDetail.dtToBeReceipt.Rows.Clear()
                    'da = New OleDbDataAdapter(strSql, cn)
                    'da.Fill(objToBeRecDetail.dtToBeReceipt)
                    ' ShowToBeReceiptDetail()
                ElseIf cmbReceiptReceiptType.Text = ReceiptType.FURTHER_ADVANCE.ToString Then
                    'ShowOrderAdvance()
                End If
            End If
            'ShowOrderAdvance()
        End If
    End Sub

    Private Sub txtReceiptAmount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtReceiptAmount_AMT.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If Not Val(txtReceiptAmount_AMT.Text) > 0 Then
                'If Val(objOrderAdvance.gridPurTotal.Rows(0).Cells("GRSWT").Value.ToString()) > 0 Then
                '    SendKeys.Send("{TAB}")
                'Else
                MsgBox("Amount Should Not Empty", MsgBoxStyle.Information)
                txtReceiptAmount_AMT.Focus()
                'End If
            Else
                SendKeys.Send("{TAB}")
            End If
        End If
    End Sub

    Private Sub cmbReceiptReceiptType_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbReceiptReceiptType.LostFocus
        'cmbReceiptCategory.Text = ""
        cmbReceiptTranType.Items.Clear()
        'cmbReceiptCategory.Text = ""
        If cmbReceiptReceiptType.Text = ReceiptType.OTHER_RECEIPTS.ToString Then
            strSql = " SELECT PRONAME FROM " & cnAdminDb & "..PROCESSTYPE WHERE PROTYPE IN ('B','R') AND PROMODULE = 'P'"
            objGPack.FillCombo(strSql, cmbReceiptTranType, , False)
            cmbReceiptTranType.Text = ""
            strsql = " SELECT " & cnAdminDb & ".DBO.WORDCAPS(ACNAME)ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE 1=1"
            objGPack.FillCombo(strsql, cmbReceiptAccount, , False)
            cmbReceiptAccount.Text = ""
        End If
    End Sub
    

    Private Sub cmbReceiptTranType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbReceiptTranType.KeyDown
        If e.KeyCode = Keys.Enter Then
            If cmbReceiptTranType.Text = "" Or cmbReceiptTranType.Items.Contains(cmbReceiptTranType.Text) = False Then
                MsgBox("Invalid Trantype", MsgBoxStyle.Information)
                cmbReceiptTranType.Select()
            Else
                cmbReceiptAccount.Select()
            End If
        End If
    End Sub


    Private Sub txtReceiptRate_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtReceiptRate_AMT.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            strSql = " SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE PURITYID = "
            strSql += " (SELECT TOP 1 PURITYID FROM " & cnAdminDb & "..PURITYMAST WHERE PURITY BETWEEN 91.6 AND 92 ORDER BY PURITY DESC)"
            Dim rate As Double = Val(GetRate(BillDate, objGPack.GetSqlValue(strSql)))
            If Not Val(objSoftKeys.Tolerance_Rate) > 0 Then GoTo afterCheck
            Dim tolValue As Double = Nothing
            tolValue = rate * (Val(objSoftKeys.Tolerance_Rate) / 100)
            If Val(txtReceiptRate_AMT.Text) < rate - tolValue Then
                MsgBox(E0023, MsgBoxStyle.Information)
                txtReceiptRate_AMT.Focus()
                Exit Sub
            End If
            If Val(txtReceiptRate_AMT.Text) > rate + tolValue Then
                MsgBox(E0023, MsgBoxStyle.Information)
                txtReceiptRate_AMT.Focus()
                Exit Sub
            End If
afterCheck:
            SendKeys.Send("{TAB}")
        End If
    End Sub

    'Private Sub cmbReceiptCategory_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbReceiptCategory.LostFocus
    '    strSql = " SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE"
    '    strSql += " PURITYID = (SELECT TOP 1 PURITYID FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & cmbReceiptCategory.Text & "')"
    '    Dim purity As Double = Val(objGPack.GetSqlValue(strSql))
    '    'txtReceiptPurity_AMT.Text = IIf(purity <> 0, Format(purity, "0.00"), Nothing)
    'End Sub

    'Private Sub txtReceiptGrsWt_WET_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtReceiptGrsWt_WET.TextChanged
    '    'CalcReceiptWeightAdvanceTotal()
    'End Sub

    'Private Sub txtReceiptWastage_WET_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtReceiptWastage_WET.TextChanged
    '    'CalcReceiptWeightAdvanceTotal()
    'End Sub

    'Private Sub txtReceiptBullionRate_RATE_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtReceiptBullionRate_RATE.TextChanged
    '    'CalcReceiptWeightAdvanceTotal()
    'End Sub

    'Private Sub txtReceiptValue_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtReceiptValue_AMT.KeyPress
    '    If e.KeyChar = Chr(Keys.Enter) Then
    '        If MessageBox.Show("Do you want to Reserve any Tag's", "Reserve Tag", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.OK Then
    '            'tabOtherOptions.SelectedTab = tabReceiptReserve
    '            'txtReceiptItemId_MAN.Select()
    '        Else
    '            txtReceiptRemark.Focus()
    '        End If
    '    End If
    'End Sub

    Private Sub chkReceiptRateFix_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles chkReceiptRateFix.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If gridReceipt.Rows(0).Cells("RECEIPTTYPE").Value.ToString <> "" And _
            gridReceipt.Rows(0).Cells("RECEIPTTYPE").Value.ToString.ToUpper = ReceiptType.CUSTOMER_ADVANCE.ToString.ToUpper Then
                If cmbReceiptReceiptType.Text = ReceiptType.CUSTOMER_ADVANCE.ToString Then
                    MsgBox("Multiple Customer Advance not Allowed", MsgBoxStyle.Information)
                    Exit Sub
                End If
            End If
            'AddReceiptRow()
            If chkReceiptRateFix.Checked = True Then
                If Split(ADVFIXWTPERSTR, ",").Length > 1 Then
                    Dim objRatefixwt As New frmAdvFixWtper
                    objRatefixwt.BackColor = Me.BackColor
                    objRatefixwt.StartPosition = FormStartPosition.CenterScreen
                    'objRatefixwt.grpDiscount.BackgroundColor = GR.BackgroundColor
                    objRatefixwt.txtRate.Text = txtReceiptRate_AMT.Text
                    objRatefixwt.txtAmount.Text = txtReceiptAmount_AMT.Text
                    objRatefixwt.ADVFIXWTPERSTR = ADVFIXWTPERSTR
                    If objRatefixwt.ShowDialog = Windows.Forms.DialogResult.OK Then ADVFIXWTPER = Val(objRatefixwt.cmbWtper.Text) Else ADVFIXWTPER = 0
                ElseIf Split(ADVFIXWTPERSTR, ",").Length = 1 Then
                    If Val(ADVFIXWTPERSTR) > 100 Then ADVFIXWTPER = Val(ADVFIXWTPERSTR)
                End If
            End If

            '   If MessageBox.Show("Do you want to enter weight advance", "Weight Advance", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.OK Then
            'tabOtherOptions.SelectedTab = tabReceiptWeightAdvance
            'cmbReceiptCategory.Select()
            ''ElseIf MessageBox.Show("Do you want Reserve Tag's", "Tag Reserve", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.OK Then
            'tabOtherOptions.SelectedTab = tabReceiptReserve
            'txtReceiptItemId_MAN.Select()
            'Else
            SendKeys.Send("{TAB}")
            'End If
        End If

    End Sub

    'Private Sub txtReceiptItemId_MAN_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtReceiptItemId_MAN.GotFocus
    '    ShowToolTip("Press Insert Key to Help", txtReceiptItemId_MAN)
    'End Sub

    'Private Sub txtReceiptItemId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtReceiptItemId_MAN.KeyDown
    '    If e.KeyCode = Keys.Insert Then
    '        strSql = " SELECT"
    '        strSql += " ITEMID AS ITEMID,ITEMNAME AS ITEMNAME"
    '        strSql += " FROM " & cnAdminDb & "..ITEMMAST"
    '        strSql += " WHERE STOCKTYPE = 'T' AND ACTIVE = 'Y'"
    '        txtReceiptItemId_MAN.Text = BrighttechPack.SearchDialog.Show("Search ItemId", strSql, cn, 1)
    '        txtReceiptItemId_MAN.SelectAll()
    '    End If
    'End Sub

    'Private Sub txtReceiptItemId_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtReceiptItemId_MAN.KeyPress
    '    If e.KeyChar = Chr(Keys.Enter) Then
    '        If txtReceiptItemId_MAN.Text = "" Then
    '            MsgBox(Me.GetNextControl(txtReceiptItemId_MAN, False).Text + E0001, MsgBoxStyle.Information)
    '            txtReceiptItemId_MAN.Select()
    '            Exit Sub
    '        End If
    '        strSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & Val(txtReceiptItemId_MAN.Text) & " AND ACTIVE = 'Y'"
    '        txtReceiptItemName.Text = objGPack.GetSqlValue(strSql)
    '        If txtReceiptItemName.Text = "" Then
    '            MsgBox(E0004 + Me.GetNextControl(txtReceiptItemId_MAN, False).Text, MsgBoxStyle.Information)
    '            txtReceiptItemId_MAN.Select()
    '        Else
    '            SendKeys.Send("{tab}")
    '        End If
    '    End If
    'End Sub

    'Private Sub txtReceiptTagNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtReceiptTagNo.GotFocus
    '    ShowToolTip("Press Insert Key to Help", txtReceiptTagNo)
    'End Sub

    'Private Sub txtReceiptTagNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtReceiptTagNo.KeyDown
    '    If e.KeyCode = Keys.Insert Then
    '        If txtReceiptItemId_MAN.Text = "" Then Exit Sub
    '        strSql = " SELECT"
    '        strSql += " TAGNO AS TAGNO,STYLENO,ITEMID AS ITEMID,"
    '        strSql += " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS ITEMNAME,"
    '        strSql += " PCS AS PCS,"
    '        strSql += " GRSWT AS GRS_WT,NETWT AS NET_WT,RATE AS RATE,"
    '        strSql += " SALVALUE AS SALVALUE,TAGVAL AS TAGVAL,"
    '        strSql += " ISNULL((SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID  = T.SUBITEMID),'')AS SUBITEM,"
    '        strSql += " CONVERT(VARCHAR,RECDATE,103) AS RECDATE"
    '        strSql += " FROM"
    '        strSql += " " & cnAdminDb & "..ITEMTAG AS T"
    '        strSql += " WHERE T.ITEMID = " & Val(txtReceiptItemId_MAN.Text) & ""
    '        strSql += GridTagNoFiltStr(dtGridSASR)
    '        strSql += GridTagNoFiltStr(dtGridReserve)
    '        strSql += vbCrLf + " AND COSTID = '" & BillCostId & "'"
    '        strSql += ShowTagFiltration()
    '        strSql += " AND ISSDATE IS NULL"
    '        'strSql += " AND APPROVAL = ''"
    '        strSql += " AND ISNULL(APPROVAL,'') = '' AND ISNULL(ORSNO,'') = ''"
    '        strSql += " ORDER BY TAGNO"
    '        txtReceiptTagNo.Text = BrighttechPack.SearchDialog.Show("Search TagNo", strSql, cn)
    '        txtReceiptTagNo.SelectAll()
    '    End If
    'End Sub

    'Private Sub txtReceiptTagNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtReceiptTagNo.KeyPress
    '    If e.KeyChar = Chr(Keys.Enter) Then
    '        If txtReceiptTagNo.Text = "" Then Exit Sub
    '        If TagCheck(txtReceiptItemId_MAN, txtReceiptTagNo) Then Exit Sub
    '        Dim rwIndex As Integer = -1
    '        For Each ro As DataRow In dtGridReserve.Rows
    '            rwIndex += 1
    '            If ro("ITEMID").ToString = txtReceiptItemId_MAN.Text _
    '            And ro("TagNo").ToString = txtReceiptTagNo.Text Then
    '                MsgBox(E0021, MsgBoxStyle.Information)
    '                Exit Sub
    '            End If
    '        Next
    '        strSql = " SELECT"
    '        strSql += " TAGNO AS TAGNO,ITEMID AS ITEMID,"
    '        strSql += " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS ITEMNAME,"
    '        strSql += " PCS AS PCS,"
    '        strSql += " GRSWT,NETWT,"
    '        strSql += " SALVALUE"
    '        strSql += " FROM"
    '        strSql += " " & cnAdminDb & "..ITEMTAG AS T"
    '        strSql += " WHERE T.ITEMID = " & Val(txtReceiptItemId_MAN.Text) & ""
    '        strSql += " AND TAGNO = '" & txtReceiptTagNo.Text & "'"
    '        strSql += GridTagNoFiltStr(dtGridReserve)
    '        strSql += GridTagNoFiltStr(dtGridSASR)
    '        strSql += " AND ISSDATE IS NULL"
    '        strSql += " AND ISNULL(APPROVAL,'') = '' AND ISNULL(ORSNO,'') = ''"
    '        Dim dt As New DataTable
    '        da = New OleDbDataAdapter(strSql, cn)
    '        da.Fill(dt)
    '        If dt.Rows.Count > 0 Then
    '            With dt.Rows(0)
    '                Dim ro As DataRow = dtGridReserve.NewRow
    '                ro!ITEMID = Val(txtReceiptItemId_MAN.Text)
    '                ro!TAGNO = txtReceiptTagNo.Text
    '                ro!ITEMNAME = .Item("ITEMNAME").ToString
    '                ro!PCS = Val(.Item("PCS").ToString)
    '                ro!GRSWT = Val(.Item("GRSWT").ToString)
    '                ro!NETWT = Val(.Item("NETWT").ToString)
    '                ro!VALUE = Val(.Item("SALVALUE").ToString)
    '                dtGridReserve.Rows.Add(ro)
    '                dtGridReserve.AcceptChanges()
    '                objGPack.TextClear(grpRecReservedItem)
    '                txtReceiptItemId_MAN.Select()
    '            End With
    '        Else
    '            MsgBox(E0004 + Me.GetNextControl(txtReceiptTagNo, False).Text, MsgBoxStyle.Information)
    '            txtReceiptTagNo.SelectAll()
    '            Exit Sub
    '        End If
    '    End If
    'End Sub

    'Private Sub gridReceipt_UserDeletedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles gridReceipt.UserDeletedRow
    '    dtGridReceipt.Rows.Add()
    '    dtGridReceipt.AcceptChanges()
    '    CalcReceiptTotal()
    '    CalcFinalAmount()
    '    CalcOrderWeightAdvance()
    '    CheckTransaction()
    'End Sub

    Private Sub dtpAdvanceDate_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        SendKeys.Send("{TAB}")
    End Sub

    Private Sub txtAdvanceAdjusted_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        SendKeys.Send("{TAB}")
    End Sub

    Private Sub txtAdvanceReceived_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        SendKeys.Send("{TAB}")
    End Sub

    'Private Sub txtReceiptItemName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtReceiptItemName.GotFocus
    '    SendKeys.Send("{TAB}")
    'End Sub
    Private Sub ClearDtGridReceiptPayment(ByVal dtRecPay As DataTable)
        With dtRecPay
            .Rows.Clear()
            For i As Integer = 1 To 20
                .Rows.Add()
            Next
            .AcceptChanges()
        End With
    End Sub


    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        cmbReceiptReceiptType.Items.Add(ReceiptType.CREDIT_SALES.ToString)
        cmbReceiptReceiptType.Items.Add(ReceiptType.CUSTOMER_ADVANCE.ToString)
        cmbReceiptReceiptType.Items.Add(ReceiptType.FURTHER_ADVANCE.ToString)
        cmbReceiptReceiptType.Items.Add(ReceiptType.OTHER_RECEIPTS.ToString)
        If GetAdmindbSoftValuefromDt(dtSoftKeys, "CHITDB", "N").ToUpper = "Y" Then
            cmbReceiptReceiptType.Items.Add(ReceiptType.CHIT_RECEIPT.ToString)
        End If
        cmbReceiptReceiptType.Text = ReceiptType.CREDIT_SALES.ToString
        With dtGridReceipt
            .Columns.Add("RECEIPTTYPE", GetType(String))
            .Columns.Add("TRANTYPE", GetType(String))
            .Columns.Add("ACCNAME", GetType(String))
            .Columns.Add("REFNO", GetType(String))
            .Columns.Add("AMOUNT", GetType(Double))
            .Columns.Add("RATE", GetType(Double))
            .Columns.Add("RATEFIX", GetType(Char))
            .Columns.Add("ADVFIXWTPER", GetType(Double))
            .Columns.Add("REMARK", GetType(String))
            .Columns.Add("EMPID", GetType(Integer))
            Dim col As New DataColumn("KEYNO")
            col.AutoIncrement = True
            col.AutoIncrementSeed = 1
            col.AutoIncrementStep = 1
            .Columns.Add(col)
        End With
        gridReceipt.DataSource = dtGridReceipt
        FormatGridColumns(gridReceipt)
        ClearDtGridReceiptPayment(dtGridReceipt)
        StyleGridReceipt(gridReceipt)
        gridReceipt.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)

        Dim dtGridReceiptTotal As New DataTable
        dtGridReceiptTotal = dtGridReceipt.Copy
        dtGridReceiptTotal.Rows.Clear()
        dtGridReceiptTotal.Rows.Add()
        dtGridReceiptTotal.Rows(0).Item("RECEIPTTYPE") = "Total"
        With gridReceiptTotal
            .DataSource = dtGridReceiptTotal
            For Each col As DataGridViewColumn In gridReceipt.Columns
                With gridReceiptTotal.Columns(col.Name)
                    .Visible = col.Visible
                    .Width = col.Width
                    .DefaultCellStyle = col.DefaultCellStyle
                End With
            Next
        End With
        FormatGridColumns(gridReceiptTotal)
        StyleGridReceipt(gridReceiptTotal)
        gridReceiptTotal.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        strsql = " SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY ORDER BY DISPLAYORDER,CATNAME"
        objGPack.FillCombo(strsql, cmbReceiptCategory)
        'dtGridReserve.Columns.Add("ITEMID", GetType(Integer))
        'dtGridReserve.Columns.Add("ITEMNAME", GetType(String))
        'dtGridReserve.Columns.Add("TAGNO", GetType(String))
        'dtGridReserve.Columns.Add("PCS", GetType(Integer))
        'dtGridReserve.Columns.Add("GRSWT", GetType(Decimal))
        'dtGridReserve.Columns.Add("NETWT", GetType(Decimal))
        'dtGridReserve.Columns.Add("VALUE", GetType(Double))
        'gridReceiptReserved.DataSource = dtGridReserve
        'FormatGridColumns(gridReceiptReserved)
        'StyleGridReceiptReseverd()

        ' ''PAYMENT
        'cmbPaymentPaytype.Items.Add("PURCHASE/SALES RETURN")
        'cmbPaymentPaytype.Items.Add("ADVANCE REPAY")
        'cmbPaymentPaytype.Items.Add("ORDER REPAY")
        'cmbPaymentPaytype.Items.Add("OTHER PAYMENTS")
        'If GetAdmindbSoftValuefromDt(dtSoftKeys, "CHITDB", "N").ToUpper = "Y" Then
        '    cmbPaymentPaytype.Items.Add("CHIT PAYMENT")
        'End If
        'cmbPaymentPaytype.Text = "ADVANCE REPAY"
        'With dtGridPayment
        '    .Columns.Add("RECEIPTTYPE", GetType(String))
        '    .Columns.Add("TRANTYPE", GetType(String))
        '    .Columns.Add("ACCNAME", GetType(String))
        '    .Columns.Add("REFNO", GetType(String))
        '    .Columns.Add("AMOUNT", GetType(Double))
        '    .Columns.Add("RATE", GetType(Double))
        '    .Columns.Add("RATEFIX", GetType(Char))
        '    .Columns.Add("REMARK", GetType(String))
        '    .Columns.Add("EMPID", GetType(Integer))
        'End With
        'gridPayment.DataSource = dtGridPayment
        'FormatGridColumns(gridPayment)
        'ClearDtGridReceiptPayment(dtGridPayment)
        'StyleGridPayment(gridPayment)
        'gridPayment.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        'Dim dtGridPaymentTotal As New DataTable
        'dtGridPaymentTotal = dtGridPayment.Copy
        'dtGridPaymentTotal.Rows.Clear()
        'dtGridPaymentTotal.Rows.Add()
        'dtGridPaymentTotal.Rows(0).Item("RECEIPTTYPE") = "Total"
        'With gridPaymentTotal
        '    .DataSource = dtGridPaymentTotal
        '    For Each col As DataGridViewColumn In gridPayment.Columns
        '        With gridPaymentTotal.Columns(col.Name)
        '            .Visible = col.Visible
        '            .Width = col.Width
        '            .DefaultCellStyle = col.DefaultCellStyle
        '        End With
        '    Next
        'End With
        'FormatGridColumns(gridPaymentTotal)
        'StyleGridPayment(gridPaymentTotal)
        'gridPaymentTotal.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)

        '' Add any initialization after the InitializeComponent() call.

    End Sub
    Private Sub ShowToolTip(ByVal text As String, ByVal ctrl As Control, Optional ByVal tipIco As ToolTipIcon = ToolTipIcon.Info)
        ' lblHelpText.Text = text
    End Sub
    Private Sub LoadEmpId(ByVal txtEmpBox As TextBox)
        strSql = " SELECT EMPID,EMPNAME FROM " & cnAdminDb & "..EMPMASTER "
        strSql += " WHERE ACTIVE = 'Y'"
        Dim empId As Integer = Val(BrighttechPack.SearchDialog.Show("Select EmpName", strSql, cn, 1))
        If empId > 0 Then
            txtEmpBox.Text = empId
            txtEmpBox.SelectAll()
        End If
    End Sub
    Private Sub txtReceiptEmpId_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtReceiptEmpId_NUM.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            ''VALIDATION
            If txtReceiptEmpId_NUM.Text = "" Then
                LoadEmpId(txtReceiptEmpId_NUM)
                Exit Sub
            ElseIf Not Val(objGPack.GetSqlValue("SELECT EMPID FROM " & cnAdminDb & "..EMPMASTER WHERE EMPID = " & Val(txtReceiptEmpId_NUM.Text) & "")) > 0 Then
                LoadEmpId(txtReceiptEmpId_NUM)
                Exit Sub
            End If
            If cmbReceiptTranType.Items.Count > 0 And (cmbReceiptTranType.Text = "" Or cmbReceiptTranType.Items.Contains(cmbReceiptTranType.Text) = False) Then
                MsgBox("Invalid Trantype", MsgBoxStyle.Information)
                cmbReceiptTranType.Select()
                Exit Sub
            End If

            'ramesh 070511
            'If Val(txtReceiptAmount_AMT.Text) = 0 Then
            '    If Not (Val(objOrderAdvance.gridPurTotal.Rows(0).Cells("GRSWT").Value.ToString()) > 0) Then
            '        MsgBox("Amount Should Not Empty", MsgBoxStyle.Information)
            '        txtReceiptAmount_AMT.Focus()
            '        Exit Sub
            '    End If
            'End If


            If cmbReceiptReceiptType.Text = ReceiptType.CREDIT_SALES.ToString Then
                If txtReceiptRefNo.Text <> txtReceiptEntRefNo.Text Then
                    MsgBox("Invalid RefNo", MsgBoxStyle.Information)
                    txtReceiptRefNo.Select()
                    Exit Sub
                End If
                If Val(txtReceiptAmount_AMT.Text) > Val(txtReceiptEntAmount.Text) Then
                    MsgBox("This RefNo Balance Amount " + txtReceiptEntAmount.Text + " Only", MsgBoxStyle.Information)
                    txtReceiptAmount_AMT.Select()
                    txtReceiptAmount_AMT.SelectAll()
                    Exit Sub
                End If
                For Each roRec As DataRow In dtGridReceipt.Rows
                    If txtReceiptRefNo.Text = roRec!REFNO.ToString Then
                        MsgBox("Already Exist this RefNo", MsgBoxStyle.Information)
                        txtReceiptRefNo.Select()
                        txtReceiptRefNo.SelectAll()
                        Exit Sub
                    End If
                Next
            ElseIf cmbReceiptReceiptType.Text = ReceiptType.FURTHER_ADVANCE.ToString Then
                If txtReceiptRefNo.Text <> txtReceiptEntRefNo.Text Then
                    MsgBox("Invalid RefNo", MsgBoxStyle.Information)
                    txtReceiptRefNo.Select()
                    Exit Sub
                End If
                For Each roRec As DataRow In dtGridReceipt.Rows
                    If txtReceiptRefNo.Text = roRec!REFNO.ToString Then
                        MsgBox("Already Exist this RefNo", MsgBoxStyle.Information)
                        txtReceiptRefNo.Select()
                        txtReceiptRefNo.SelectAll()
                        Exit Sub
                    End If
                Next
            End If

            AddReceiptRow()
        End If
    End Sub

    Private Function AddReceiptRow() As Integer
        If gridReceipt.Rows(0).Cells("RECEIPTTYPE").Value.ToString <> "" And _
        gridReceipt.Rows(0).Cells("RECEIPTTYPE").Value.ToString <> cmbReceiptReceiptType.Text Then
            MsgBox("Multiple Transaction not Allowed", MsgBoxStyle.Information)
            cmbReceiptReceiptType.Select()
            Exit Function
        End If
        If gridReceipt.Rows(0).Cells("RECEIPTTYPE").Value.ToString.ToUpper = ReceiptType.Customer_Advance.ToString.ToUpper Then
            MsgBox("Multiple Advance Transaction not Allowed", MsgBoxStyle.Information)
            cmbReceiptReceiptType.Select()
            Exit Function
        End If
        If gridReceipt.Rows(0).Cells("RECEIPTTYPE").Value.ToString.ToUpper = ReceiptType.FURTHER_ADVANCE.ToString.ToUpper Then
            MsgBox("Multiple Further Advance Transaction not Allowed", MsgBoxStyle.Information)
            cmbReceiptReceiptType.Select()
            Exit Function
        End If
        'Dim row As DataRow = dtGridReceipt.NewRow
        Dim index As Integer = 0
        For Each row As DataRow In dtGridReceipt.Rows
            If row!RECEIPTTYPE.ToString = "" Then
                row!RECEIPTTYPE = cmbReceiptReceiptType.Text
                row!TRANTYPE = cmbReceiptTranType.Text
                row!ACCNAME = cmbReceiptAccount.Text
                row!REFNO = txtReceiptRefNo.Text
                row!AMOUNT = IIf(Val(txtReceiptAmount_AMT.Text), Val(txtReceiptAmount_AMT.Text), DBNull.Value)
                row!RATE = IIf(Val(txtReceiptRate_AMT.Text), Val(txtReceiptRate_AMT.Text), DBNull.Value)
                row!RATEFIX = IIf(chkReceiptRateFix.Checked, "Y", DBNull.Value)
                row!ADVFIXWTPER = IIf(chkReceiptRateFix.Checked, ADVFIXWTPER, 0)
                row!REMARK = txtReceiptRemark.Text
                row!EMPID = Val(txtReceiptEmpId_NUM.Text)
                dtGridReceipt.Rows.Add()
                Exit For
            End If
            index += 1
        Next
        dtGridReceipt.AcceptChanges()
        ''loadToB Receipt
        'For Each rowTob As DataRow In objToBeRecDetail.dtToBeReceipt.Rows
        '    rowTob.Item("KEYNO") = dtGridReceipt.Rows(index).Item("KEYNO")
        'Next
        'objToBeRecDetail.dtToBeReceipt.AcceptChanges()
        'For Each rowToB As DataRow In objToBeRecDetail.dtToBeReceipt.Rows
        ' dtToBeReceipt.ImportRow(rowToB)
        ' dtToBeReceipt.Rows(dtToBeReceipt.Rows.Count - 1).Item("KEYNO") = dtGridReceipt.Rows(index).Item("KEYNO")
        ' Next
        CalcReceiptTotal()
        CalcFinalAmount()
        '        CalcOrderWeightAdvance()


        objGPack.TextClear(grpReceipt)
        chkReceiptRateFix.Checked = False
        'CheckTransaction()
        'tabOtherOptions.SelectedTab = tabGeneral
        'If cmbReceiptReceiptType.Text <> ReceiptType.OTHER_RECEIPTS.ToString Then
        '   If TranTypeCol.Contains("SA") Or TranTypeCol.Contains("PU") Then tabMain.SelectedTab = tabSaSrPu
        '  If grpAdj.Enabled Then
        'txtAdjCash_AMT.Focus()
        'Else
        'ShowAddressDia()
        'End If
        'Else
        'cmbReceiptReceiptType.Select()
        'End If
    End Function
    Private Sub CalcReceiptTotal()
        Dim mhandcharge As Decimal = 0

        Dim amt As Double = Nothing
        For Each ro As DataRow In dtGridReceipt.Rows
            If ro!RECEIPTTYPE.ToString = "" Then Exit For
            amt += Val(ro!AMOUNT.ToString)
            'If RATE_DIFF_HC <> "N" Then
            '        Dim mbilldet() As String = Split(GetBillrate(ro!refno), ",")
            '        If mbilldet.Length > 0 Then
            '            Dim moldrate As Decimal = mbilldet(1)
            '            Dim mweight As Decimal = mbilldet(0)
            '            Dim mnewrate As Decimal = Val(lblGoldRate.Text)
            '            Dim mdiffrate As Decimal = mnewrate - moldrate
            '            If mdiffrate < 0 And (RATE_DIFF_HC = "A" Or RATE_DIFF_HC = "L") Then
            '                mhandcharge += (mdiffrate * mweight)
            '            ElseIf mdiffrate > 0 And (RATE_DIFF_HC = "A" Or RATE_DIFF_HC = "H") Then
            '                mhandcharge += (mdiffrate * mweight)
            '            End If
            '        End If
            '    End If
        Next
        'Me.txtAdjHandlingCharge_AMT.Text = Math.Round(mhandcharge, 2)
        gridReceiptTotal.Rows(0).Cells("AMOUNT").Value = IIf(amt <> 0, amt, DBNull.Value)
    End Sub

    Private Function CalcFinalAmount(Optional ByVal ReturnOnly As Boolean = False, Optional ByVal Tcsamt As Decimal = 0) As Double
        ''CalcDiscount() ''AFTER TAX SALES DISCOUNT

        'If Val(gridSASRTotal.Rows(0).Cells("AMOUNT").Value.ToString) - Val(gridSASRTotal.Rows(1).Cells("AMOUNT").Value.ToString) - Val(gridPurTotal.Rows(0).Cells("AMOUNT").Value.ToString) + Val(gridReceiptTotal.Rows(0).Cells("AMOUNT").Value.ToString) - Val(gridPaymentTotal.Rows(0).Cells("AMOUNT").Value.ToString) + Val(gridMGiftVoucherTotal.Rows(0).Cells("AMOUNT").Value.ToString) = 0 Then Exit Function

        
        Dim receiveAmt As Double = Nothing
        If Not gridReceiptTotal.RowCount > 0  Then Exit Function
        'If Not Val(txtOrdAdvanceWeight_WET.Text) > 0 Then
        receiveAmt = Val(gridReceiptTotal.Rows(0).Cells("AMOUNT").Value.ToString)
          
        'End If
        'If Not objOrderAdvance Is Nothing Then
        '    ' speed access if and endif only
        '    If Val("" & objOrderAdvance.dtGridPur.Compute("sum(Amount)", Nothing)) <> 0 Then
        '        For Each Ro As DataRow In objOrderAdvance.dtGridPur.Rows
        '            If Ro.Item("MODE").ToString = "PURCHASE" Or Ro.Item("MODE").ToString = "EXCHANGE" Then
        '                receiveAmt -= Val(Ro.Item("AMOUNT").ToString)
        '            End If
        '        Next
        '    End If
        'End If
        'If Tcsamt <> 0 Then
        ' speed access if only
        '        If receiveAmt = 0 Then Return 0
        'receiveAmt += Val(txtTCS_Amt.Text)
        'Dim roundOff As Double = Nothing
        'roundOff = Math.Abs(receiveAmt) - CalcRoundoffAmt(Math.Abs(receiveAmt), objSoftKeys.RoundOff_Final)
        'txtAdjReceive_AMT.Text = IIf(receiveAmt <> 0, Format(receiveAmt, "0.00"), Nothing)
        'txtAdjRoundoff_AMT.Text = IIf(roundOff <> 0, Format(roundOff, "0.00"), Nothing)
        'Dim ActRec As Double = Val(txtAdjReceive_AMT.Text) + Val(txtAdjSrCredit_AMT.Text)
        Dim cash As Double = Nothing
        cash += receiveAmt
        '+ Val(txtAdjSrCredit_AMT.Text) _
        '        - IIf(Val(txtAdjReceive_AMT.Text) + Val(txtAdjSrCredit_AMT.Text) > 0, Val(txtAdjRoundoff_AMT.Text), -1 * Val(txtAdjRoundoff_AMT.Text)) _
        '        - Val(txtAdjAdvance_AMT.Text) _
        '        - IIf(ActRec > 0, Val(txtAdjCredit_AMT.Text), -1 * Val(txtAdjCredit_AMT.Text)) _
        '        - IIf(ActRec > 0, Val(txtAdjGiftVoucher_AMT.Text), -1 * Val(txtAdjGiftVoucher_AMT.Text)) _
        '        - Val(txtAdjChitCard_AMT.Text) _
        '        - IIf(ActRec > 0, Val(txtAdjCheque_AMT.Text), -1 * Val(txtAdjCheque_AMT.Text)) _
        '        - IIf(ActRec > 0, Val(txtAdjCreditCard_AMT.Text), -1 * Val(txtAdjCreditCard_AMT.Text)) _
        '        + IIf(ActRec > 0, Val(txtAdjHandlingCharge_AMT.Text), -1 * Val(txtAdjHandlingCharge_AMT.Text)) _
        '        - Val(txtAdjDiscount_AMT.Text) ' _

        '  If Not ReturnOnly Then
        '      txtAdjCash_AMT.Text = IIf(cash <> 0, Format(cash, "0.00"), Nothing)
        '  End If
        Return cash
    End Function

    Private Sub EstAdvReceipt_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If Val("" & gridReceiptTotal.Rows(0).Cells("AMOUNT").Value) <> 0 Then Me.DialogResult = Windows.Forms.DialogResult.OK Else Me.DialogResult = Windows.Forms.DialogResult.Cancel
            Me.Close()
        End If
    End Sub


    'Private Sub StyleGridReceiptReseverd()
    '    With gridReceiptReserved
    '        .Columns("ITEMID").Width = txtReceiptItemId_MAN.Width + 1
    '        .Columns("TAGNO").Width = txtReceiptTagNo.Width + 1
    '        .Columns("ITEMNAME").Width = txtReceiptItemName.Width + 1
    '        .Columns("PCS").Width = txtReceiptPcs.Width + 1
    '        .Columns("GRSWT").Width = txtReceiptGrsWt.Width + 1
    '        .Columns("NETWT").Width = txtReceiptNetWT.Width + 1
    '        .Columns("VALUE").Width = txtReceiptValue.Width + 1
    '    End With
    'End Sub

    Private Sub EstAdvReceipt_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub chkReceiptRateFix_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkReceiptRateFix.CheckedChanged

    End Sub


    Private Sub txtReceiptRemark_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtReceiptRemark.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txtReceiptEmpId_NUM_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtReceiptEmpId_NUM.TextChanged

    End Sub
    Private Sub StyleGridReceipt(ByVal grid As DataGridView)
        With grid
            .Columns("RECEIPTTYPE").Width = cmbReceiptReceiptType.Width + 1
            .Columns("TRANTYPE").Width = cmbReceiptTranType.Width + 1
            .Columns("ACCNAME").Width = cmbReceiptAccount.Width + 1
            .Columns("REFNO").Width = txtReceiptRefNo.Width + 1
            .Columns("AMOUNT").Width = txtReceiptAmount_AMT.Width + 1
            .Columns("RATE").Width = txtReceiptRate_AMT.Width + 1
            .Columns("RATEFIX").Width = chkReceiptRateFix.Width + 1
            .Columns("REMARK").Width = txtReceiptRemark.Width + 1
            .Columns("EMPID").Width = txtReceiptEmpId_NUM.Width + 1
            .Columns("KEYNO").Visible = False

        End With
    End Sub
End Class