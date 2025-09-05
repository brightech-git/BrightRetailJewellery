Imports System.Data.OleDb
Public Class frmGVWeight
    Dim cmd As OleDbCommand
    Dim strSql As String
    Public dtGridGiftVouhcer As New DataTable
    Public BILLCOSTID As String
    Public GETSILVERRATE As Double
    Public GETGOLDRATE As Double
    Dim Mcardid As Integer
    Dim GV_VALIDATE As String = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID ='GV_VALIDATE'", , "N")
    Public mAddressPrevilegeId, mAddressPartyCode, mAddressTitle, mAddressInitial, mAddressName, mAddressDoorNo As String
    Public mAddress1, mAddress2, mAddress3, mAddressarea, mAddresscity, mAddressstate, mAddressCountry, mAddressPincode, mAddressEmailid As String
    Public mAddressFax, mAddressPhoneRes, mAddressMobile, mAddressRegularSno As String



    Private Sub frmGSGiftVoucher_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            dtGridGiftVouhcer.AcceptChanges()
            cmbGiftVoucherType.Select()
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub frmGSGiftVoucher_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If gridGiftVoucher.Focused Then Exit Sub
            If cmbGiftVoucherType.Focused Then Exit Sub
            If txtGiftAmount_AMT.Focused Then Exit Sub
            If GV_VALIDATE = "M" And txtRefNo.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        ''GIFTVOUHCER
        objGPack.Validator_Object(Me)
        objGPack.TextClear(Me)
        strSql = " SELECT NAME FROM " & cnAdminDb & "..CREDITCARD "
        strSql += " WHERE CARDTYPE = 'G'"
        objGPack.FillCombo(strSql, cmbGiftVoucherType)

        With dtGridGiftVouhcer
            .Columns.Add("CARDTYPE", GetType(String))
            .Columns.Add("RUNNO", GetType(String))
            .Columns.Add("REMARK", GetType(String))
            .Columns.Add("DENOMINATION", GetType(Double))
            .Columns.Add("UNIT", GetType(Integer))
            .Columns.Add("WEIGHT", GetType(Decimal))
            .Columns.Add("AMOUNT", GetType(Double))
            .Columns.Add("COMMISION", GetType(Double))
            .Columns.Add("REFNO", GetType(String))
        End With
        gridGiftVoucher.DataSource = dtGridGiftVouhcer
        FormatGridColumns(gridGiftVoucher)
        StyleGridGiftVoucher(gridGiftVoucher)
        Dim dtGridGiftVoucherTotal As New DataTable
        dtGridGiftVoucherTotal = dtGridGiftVouhcer.Copy
        dtGridGiftVoucherTotal.Rows.Clear()
        dtGridGiftVoucherTotal.Rows.Add()
        dtGridGiftVoucherTotal.Rows(0).Item("CARDTYPE") = "Total"
        With gridGiftVoucherTotal
            .DataSource = dtGridGiftVoucherTotal
            For Each col As DataGridViewColumn In gridGiftVoucher.Columns
                With gridGiftVoucher.Columns(col.Name)
                    .Visible = col.Visible
                    .Width = col.Width
                    .DefaultCellStyle = col.DefaultCellStyle
                End With
            Next
        End With
        FormatGridColumns(gridGiftVoucherTotal)
        StyleGridGiftVoucher(gridGiftVoucherTotal)
    End Sub
    Private Sub cmbGiftVoucherType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbGiftVoucherType.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If cmbGiftVoucherType.Text = "" Then
                MsgBox(Me.GetNextControl(cmbGiftVoucherType, False).Text + E0001, MsgBoxStyle.Information)
            Else
                Me.SelectNextControl(cmbGiftVoucherType, True, True, True, True)
            End If
        End If
    End Sub

    Private Sub txtGiftRemark_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtGiftRemark.GotFocus
        If txtRefNo.Text = "" Then
            txtRefNo.Focus()
        End If
    End Sub
    Private Sub txtGiftRemark_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtGiftRemark.KeyDown
        If e.KeyCode = Keys.Down Then
            If gridGiftVoucher.RowCount > 0 Then gridGiftVoucher.Select()

        End If
    End Sub

    Private Sub txtGiftDenomination_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtGiftDenomination_AMT.GotFocus
        CalcGiftVoucherAmount()
    End Sub
    Private Sub txtGiftDenomination_AMT_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtGiftDenomination_AMT.KeyDown
        If e.KeyCode = Keys.Down Then
            If gridGiftVoucher.RowCount > 0 Then gridGiftVoucher.Select()
        End If
    End Sub

    Private Sub txtGiftDenomination_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtGiftDenomination_AMT.TextChanged
        CalcGiftVoucherAmount()
    End Sub

    Private Sub txtGiftUnit_NUM_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtGiftUnit_NUM.GotFocus
        CalcGiftVoucherAmount()
    End Sub
    Private Sub txtGiftUnit_INT_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtGiftUnit_NUM.KeyDown
        If e.KeyCode = Keys.Down Then
            If gridGiftVoucher.RowCount > 0 Then gridGiftVoucher.Select()
        End If
    End Sub

    Private Sub txtGiftUnit_NUM_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtGiftUnit_NUM.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If cmbGiftVoucherType.Text <> "" And Val(txtWtAdj_WET.Text) > 0 Then Call txtGiftAmount_KeyPress(sender, e)
        End If
    End Sub

    Private Sub txtGiftUnit_NUM_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtGiftUnit_NUM.Leave
        'txtRefNo.Select()
    End Sub

    Private Sub txtGiftUnit_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtGiftUnit_NUM.TextChanged
        CalcGiftVoucherAmount()
    End Sub
    Private Sub txtGiftAmount_AMT_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtGiftAmount_AMT.KeyDown
        If e.KeyCode = Keys.Down Then
            If gridGiftVoucher.RowCount > 0 Then gridGiftVoucher.Select()
        End If
    End Sub

    Private Sub txtGiftAmount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtGiftAmount_AMT.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If cmbGiftVoucherType.Text = "" Then
                MsgBox(Me.GetNextControl(cmbGiftVoucherType, False).Text + E0001, MsgBoxStyle.Information)
                Exit Sub
            ElseIf cmbGiftVoucherType.Items.Contains(cmbGiftVoucherType.Text) = False Then
                MsgBox(E0004 + Me.GetNextControl(cmbGiftVoucherType, False).Text, MsgBoxStyle.Information)
                Exit Sub
            End If
            CalcGiftVoucherAmount()
            If Val(txtGiftAmount_AMT.Text) = 0 Then
                Exit Sub
            End If
            If Val(txtGiftRowIndex.Text) = Val("") Then
                'If Val(txtGiftAmount_AMT.Text) > Val(txtAdjCash_AMT.Text) Then
                '    MsgBox("Balance Amount " + Val(txtAdjCash_AMT.Text).ToString + " Only", MsgBoxStyle.Information)
                '    txtGiftAmount_AMT.Select()
                '    Exit Sub
                'End If

                Dim ro As DataRow = Nothing
                ro = dtGridGiftVouhcer.NewRow
                ro!CARDTYPE = cmbGiftVoucherType.Text
                ro!REMARK = txtGiftRemark.Text
                ro!RUNNO = txtRefNo.Text
                ro!REFNO = Mcardid.ToString
                ro!DENOMINATION = IIf(Val(txtGiftDenomination_AMT.Text) <> 0, Val(txtGiftDenomination_AMT.Text), DBNull.Value)
                ro!UNIT = IIf(Val(txtGiftUnit_NUM.Text) <> 0, Val(txtGiftUnit_NUM.Text), DBNull.Value)
                ro!WEIGHT = IIf(Val(txtWtAdj_WET.Text) <> 0, Val(txtWtAdj_WET.Text), DBNull.Value)
                ro!AMOUNT = IIf(Val(txtGiftAmount_AMT.Text) <> 0, Val(txtGiftAmount_AMT.Text), DBNull.Value)
                dtGridGiftVouhcer.Rows.Add(ro)
            Else
                With gridGiftVoucher.Rows(Val(txtGiftRowIndex.Text))
                    'If Val(txtGiftAmount_AMT.Text) > Val(txtAdjCash_AMT.Text) + Val(.Cells("AMOUNT").Value.ToString) Then
                    '    MsgBox("Balance Amount " + (Val(txtAdjCash_AMT.Text) + Val(.Cells("AMOUNT").Value.ToString)).ToString + " Only", MsgBoxStyle.Information)
                    '    txtGiftAmount_AMT.Select()
                    '    Exit Sub
                    'End If
                    .Cells("CARDTYPE").Value = cmbGiftVoucherType.Text
                    .Cells("REFNO").Value = Mcardid.ToString
                    .Cells("RUNNO").Value = txtRefNo.Text
                    .Cells("REMARK").Value = txtGiftRemark.Text
                    .Cells("DENOMINATION").Value = IIf(Val(txtGiftDenomination_AMT.Text) <> 0, Val(txtGiftDenomination_AMT.Text), DBNull.Value)
                    .Cells("UNIT").Value = IIf(Val(txtGiftUnit_NUM.Text) <> 0, Val(txtGiftUnit_NUM.Text), DBNull.Value)
                    .Cells("WEIGHT").Value = IIf(Val(txtWtAdj_WET.Text) <> 0, Val(txtWtAdj_WET.Text), DBNull.Value)
                    .Cells("AMOUNT").Value = IIf(Val(txtGiftAmount_AMT.Text) <> 0, Val(txtGiftAmount_AMT.Text), DBNull.Value)
                End With
            End If
            dtGridGiftVouhcer.AcceptChanges()

            CalcGiftVoucherTotal()
            Dim type As String = cmbGiftVoucherType.Text
            objGPack.TextClear(grpGiftVoucher)
            cmbGiftVoucherType.Text = type
            Mcardid = 0
            cmbGiftVoucherType.Select()
        End If
    End Sub
    Private Sub gridGiftVoucher_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridGiftVoucher.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            If gridGiftVoucher.RowCount > 0 Then
                gridGiftVoucher.CurrentCell = gridGiftVoucher.Rows(gridGiftVoucher.CurrentRow.Index).Cells("CARDTYPE")
                With dtGridGiftVouhcer.Rows(gridGiftVoucher.CurrentRow.Index)
                    cmbGiftVoucherType.Text = .Item("CARDTYPE").ToString
                    txtGiftRemark.Text = .Item("REMARK").ToString
                    txtGiftDenomination_AMT.Text = Format(Val(.Item("DENOMINATION").ToString), "0.00")
                    txtGiftUnit_NUM.Text = .Item("UNIT").ToString
                    txtWtAdj_WET.Text = Format(Val(.Item("WEIGHT").ToString), "0.000")
                    txtGiftAmount_AMT.Text = Format(Val(.Item("AMOUNT").ToString), "0.00")
                    txtRefNo.Text = .Item("RUNNO").ToString
                    Mcardid = objGPack.GetSqlValue("SELECT CARDCODE FROM " & cnAdminDb & "..CREDITCARD WHERE CARDTYPE = 'G' AND NAME = '" & cmbGiftVoucherType.Text & "'")
                    txtGiftRowIndex.Text = gridGiftVoucher.CurrentRow.Index
                    cmbGiftVoucherType.Select()
                End With
            End If


        End If
    End Sub

    Private Sub gridGiftVoucher_UserDeletedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles gridGiftVoucher.UserDeletedRow
        dtGridGiftVouhcer.AcceptChanges()
        CalcGiftVoucherTotal()
        If Not gridGiftVoucher.RowCount > 0 Then
            cmbGiftVoucherType.Select()
        End If
    End Sub
    Private Sub StyleGridGiftVoucher(ByVal grid As DataGridView)
        With grid
            .Columns("CARDTYPE").Width = cmbGiftVoucherType.Width + 1
            .Columns("RUNNO").Width = txtRefNo.Width + 1
            .Columns("REMARK").Width = txtGiftRemark.Width + 1
            .Columns("DENOMINATION").Width = txtGiftDenomination_AMT.Width + 1
            .Columns("UNIT").Width = txtGiftUnit_NUM.Width + 1
            .Columns("WEIGHT").Width = txtWtAdj_WET.Width + 1
            .Columns("AMOUNT").Width = txtGiftAmount_AMT.Width + 1
            .Columns("COMMISION").Visible = False
            .Columns("REFNO").Visible = False
        End With
    End Sub
    Private Sub CalcGiftVoucherAmount()
        Dim amt As Double = Nothing
        If txtRefNo.Text.ToString.Contains("GG") Then
            amt = CalcRoundoffAmt(Val(txtWtAdj_WET.Text) * Val(GETGOLDRATE), "L")
        Else
            amt = CalcRoundoffAmt(Val(txtWtAdj_WET.Text) * Val(GETSILVERRATE), "L")
        End If
        txtGiftAmount_AMT.Text = IIf(amt <> 0, Format(amt, "0.00"), Nothing)
    End Sub

    Public Sub CalcGiftVoucherTotal()
        Dim amt As Double = Nothing
        For Each ro As DataRow In dtGridGiftVouhcer.Rows
            amt += Val(ro!AMOUNT.ToString)
        Next
        gridGiftVoucherTotal.Rows(0).Cells("AMOUNT").Value = IIf(amt <> 0, amt, DBNull.Value)
    End Sub

    Private Sub frmGSGiftVoucher_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        gridGiftVoucher.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        gridGiftVoucherTotal.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        gridGiftVoucherTotal.DefaultCellStyle.BackColor = grpGiftVoucher.BackgroundColor
        gridGiftVoucherTotal.DefaultCellStyle.SelectionBackColor = grpGiftVoucher.BackgroundColor
        CalcGiftVoucherAmount()
    End Sub

    Private Sub txtRefNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtRefNo.KeyDown
        If e.KeyCode = Keys.Insert Then
            Mcardid = objGPack.GetSqlValue("SELECT CARDCODE FROM " & cnAdminDb & "..CREDITCARD WHERE CARDTYPE = 'G' AND NAME = '" & cmbGiftVoucherType.Text & "'")
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

            strSql = " SELECT  RUNNO"
            strSql += vbCrLf + " ,CASE WHEN WEIGHT <> 0 THEN WEIGHT ELSE NULL END AS WEIGHT"
            strSql += vbCrLf + " ,x.TRANDATE,V.PREVILEGEID,V.ACCODE,V.TITLE,V.INITIAL,V.PNAME,V.DOORNO,V.ADDRESS1,V.ADDRESS2,V.ADDRESS3"
            strSql += vbCrLf + " ,V.AREA,V.CITY,V.STATE,V.COUNTRY,V.PINCODE,V.PHONERES,V.MOBILE,V.EMAIL,V.FAX,V.SNO,X.COMPANYID"
            strSql += vbCrLf + "  FROM"
            strSql += vbCrLf + " ("
            strSql += vbCrLf + " SELECT RUNNO"
            strSql += vbCrLf + "  ,SUM(CASE WHEN RECPAY = 'R' THEN GRSWT  ELSE -1*GRSWT  END)AS WEIGHT"
            strSql += vbCrLf + " ,(SELECT TOP 1 TRANDATE FROM " & cnAdminDb & "..OUTSTANDING WHERE  RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  AND ISNULL(CANCEL,'') = '' ORDER BY TRANDATE DESC)AS TRANDATE"
            strSql += vbCrLf + " ,(SELECT TOP 1 BATCHNO FROM " & cnAdminDb & "..OUTSTANDING WHERE  RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  AND ISNULL(CANCEL,'') = '' ORDER BY TRANDATE DESC)AS BATCHNO"
            strSql += vbCrLf + " ,O.COMPANYID "
            strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING O"
            strSql += vbCrLf + " WHERE TRANTYPE = 'GV'"
            'If Not withOrderDetail Then strSql += vbCrLf + " AND SUBSTRING(RUNNO,6,20) NOT LIKE 'O%'"
            'If Not withRepairDetail Then strSql += vbCrLf + " AND SUBSTRING(RUNNO,6,20) NOT LIKE 'R%'"
            strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
            strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
            'strSql += vbCrLf + " AND COSTID = '" & BILLCOSTID & "'"
            strSql += vbCrLf + " AND REFNO = '" & Mcardid.ToString & "'"
            strSql += vbCrLf + " GROUP BY RUNNO,COMPANYID"
            strSql += vbCrLf + " HAVING SUM(CASE WHEN RECPAY = 'R' THEN GRSWT ELSE -1*GRSWT END) > 0 "
            strSql += vbCrLf + " )X"
            strSql += vbCrLf + " INNER JOIN TEMP" & systemId & "_CUSTOMERINFO AS V ON V.BATCHNO = X.BATCHNO"
            Dim row As DataRow = Nothing
            row = BrighttechPack.SearchDialog.Show_R("Search Pending Gift Voucher No", strSql, cn, 7, , , , , , , False)
            If Not row Is Nothing Then
                With row
                    For Each roAdv As DataRow In dtGridGiftVouhcer.Rows
                        If .Item("RUNNO").ToString = roAdv!RUNNO.ToString Then
                            MsgBox("Already Exist in this Gift Vou No", MsgBoxStyle.Information)
                            txtGiftRemark.Select()
                            txtGiftRemark.SelectAll()
                            Exit Sub
                        End If
                    Next

                    'dtpAdvanceDate.Value = .Item("TRANDATE")
                    txtRefNo.Text = .Item("RUNNO")

                    txtGiftDenomination_AMT.Text = ""
                    txtGiftAmount_AMT.Text = ""
                    txtWtAdj_WET.Text = .Item("WEIGHT")
                    txtGiftUnit_NUM.Text = ""
                    'txtAdvanceReceived_AMT.Text = IIf(Val(.Item("RECEIPT").ToString) <> 0, Format(Val(.Item("RECEIPT").ToString), "0.00"), Nothing)
                    'txtAdvanceAdjusted_AMT.Text = IIf(Val(.Item("PAYMENT").ToString) <> 0, Format(Val(.Item("PAYMENT").ToString), "0.00"), Nothing)
                    'txtAdvanceBalance_AMT.Text = IIf(Val(.Item("BALANCE").ToString) <> 0, Format(Val(.Item("BALANCE").ToString), "0.00"), Nothing)

                    'txtAdvanceNo.Text = .Item("REFNO").ToString
                    'txtAdvanceEntAdvanceNo.Text = txtAdvanceNo.Text
                    'txtAdvanceEntAmount.Text = txtAdvanceBalance_AMT.Text
                    'txtAdvanceCompanyId.Text = .Item("COMPANYID").ToString
                    'txtAdvanceBalance_AMT.Focus()

                    'strSql = "SELECT TOP 1 REMARK1 FROM " & cnStockDb & ".dbo.outstanding where TRANTYPE = 'GV' AND runno = '" & GetCostId(BILLCOSTID) & GetCompanyId(strCompanyId) & row.Item("REFNO") & "'"
                    'strSql += " AND COMPANYID = '" & strCompanyId & "'"
                    'strSql += " AND COSTID = '" & BILLCOSTID & "'"
                    'Dim mRemark As String = GetSqlValue(cn, strSql)

                End With
            End If
        End If

    End Sub

    Private Sub txtRefNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtRefNo.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If GV_VALIDATE = "N" Then txtGiftRemark.Focus() : Exit Sub
            If GV_VALIDATE = "M" And txtRefNo.Text.ToString = "" Then txtGiftDenomination_AMT.Focus() : Exit Sub
            Dim refno As String = ""
            refno = txtRefNo.Text.ToString.Trim
            'refno = txtRefNo.Text.ToString.Replace(GetCostId(cnCostId) & GetCompanyId(strCompanyId), "")
            'refno = GetCostId(cnCostId) & GetCompanyId(strCompanyId) & refno
            Mcardid = objGPack.GetSqlValue("SELECT CARDCODE FROM " & cnAdminDb & "..CREDITCARD WHERE CARDTYPE = 'G' AND NAME = '" & cmbGiftVoucherType.Text & "'")
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
            strSql = " SELECT SUBSTRING(RUNNO,6,20) REFNO"
            strSql += vbCrLf + " ,CASE WHEN WEIGHT <> 0 THEN WEIGHT ELSE NULL END AS WEIGHT"
            strSql += vbCrLf + " ,x.TRANDATE,V.PREVILEGEID,V.ACCODE,V.TITLE,V.INITIAL,V.PNAME,V.DOORNO,V.ADDRESS1,V.ADDRESS2,V.ADDRESS3"
            strSql += vbCrLf + " ,V.AREA,V.CITY,V.STATE,V.COUNTRY,V.PINCODE,V.PHONERES,V.MOBILE,V.EMAIL,V.FAX,V.SNO,X.COMPANYID"
            strSql += vbCrLf + " ,RUNNO FROM"
            strSql += vbCrLf + " ("
            strSql += vbCrLf + " SELECT RUNNO"
            strSql += vbCrLf + "  ,SUM(CASE WHEN RECPAY = 'R' THEN GRSWT  ELSE -1*GRSWT  END)AS WEIGHT"
            strSql += vbCrLf + " ,(SELECT TOP 1 TRANDATE FROM " & cnAdminDb & "..OUTSTANDING WHERE COSTID = '" & BILLCOSTID & "' AND RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  AND ISNULL(CANCEL,'') = '' ORDER BY TRANDATE DESC)AS TRANDATE"
            strSql += vbCrLf + " ,(SELECT TOP 1 BATCHNO FROM " & cnAdminDb & "..OUTSTANDING WHERE COSTID = '" & BILLCOSTID & "' AND RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  AND ISNULL(CANCEL,'') = '' ORDER BY TRANDATE DESC)AS BATCHNO"
            strSql += vbCrLf + " ,O.COMPANYID "
            strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING O"
            strSql += vbCrLf + " WHERE TRANTYPE = 'GV'"
            strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
            strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
            strSql += vbCrLf + " AND COSTID = '" & BILLCOSTID & "'"
            strSql += vbCrLf + " AND RUNNO = '" & refno & "'"
            strSql += vbCrLf + " AND REFNO = '" & Mcardid.ToString & "'"
            strSql += vbCrLf + " GROUP BY RUNNO,COMPANYID"
            strSql += vbCrLf + " HAVING SUM(CASE WHEN RECPAY = 'R' THEN GRSWT ELSE -1*GRSWT END)  > 0 "
            strSql += vbCrLf + " )X"
            strSql += vbCrLf + " INNER JOIN TEMP" & systemId & "_CUSTOMERINFO AS V ON V.BATCHNO = X.BATCHNO"
            Dim row As DataRow = Nothing
            row = GetSqlRow(strSql, cn)
            If Not row Is Nothing Then
                With row
                    For Each roAdv As DataRow In dtGridGiftVouhcer.Rows
                        If .Item("RUNNO").ToString = roAdv!RUNNO.ToString Then
                            MsgBox("Already Exist in this Gift Vou No", MsgBoxStyle.Information)
                            txtRefNo.Select()
                            txtRefNo.SelectAll()
                            Exit Sub
                        End If
                    Next

                    mAddressPrevilegeId = .Item("PREVILEGEID").ToString
                    mAddressPartyCode = .Item("ACCODE").ToString
                    mAddressTitle = .Item("TITLE").ToString
                    mAddressInitial = .Item("INITIAL").ToString
                    mAddressName = .Item("PNAME").ToString
                    mAddressDoorNo = .Item("DOORNO").ToString
                    mAddress1 = .Item("ADDRESS1").ToString
                    mAddress2 = .Item("ADDRESS2").ToString
                    mAddress3 = .Item("ADDRESS3").ToString
                    mAddressarea = .Item("AREA").ToString
                    mAddresscity = .Item("CITY").ToString
                    mAddressstate = .Item("STATE").ToString
                    mAddressCountry = .Item("COUNTRY").ToString
                    mAddressPincode = .Item("PINCODE").ToString
                    mAddressEmailid = .Item("EMAIL").ToString
                    mAddressFax = .Item("FAX").ToString
                    mAddressPhoneRes = .Item("PHONERES").ToString
                    mAddressMobile = .Item("MOBILE").ToString
                    mAddressRegularSno = .Item("SNO").ToString

                    'dtpAdvanceDate.Value = .Item("TRANDATE")
                    txtRefNo.Text = .Item("RUNNO")
                    Mcardid = Val(.Item("REFNO"))
                    txtGiftDenomination_AMT.Text = ""

                    txtGiftAmount_AMT.Text = ""
                    txtWtAdj_WET.Text = .Item("WEIGHT")
                    txtGiftUnit_NUM.Text = ""
                    'txtRefNo.Enabled = False
                    txtGiftDenomination_AMT.Enabled = False
                    txtGiftAmount_AMT.Enabled = False
                    txtGiftUnit_NUM.ReadOnly = True
                    txtGiftUnit_NUM.Focus()
                End With
            Else
                MsgBox("Please Enter Valid Voucher No", MsgBoxStyle.Information)
                txtRefNo.Text = "" : txtRefNo.Focus()
            End If
        End If

    End Sub

    Private Sub txtRefNo_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRefNo.Leave
        Exit Sub
        If GV_VALIDATE = "N" Then txtGiftRemark.Focus() : Exit Sub
        Mcardid = objGPack.GetSqlValue("SELECT CARDCODE FROM " & cnAdminDb & "..CREDITCARD WHERE CARDTYPE = 'G' AND NAME = '" & cmbGiftVoucherType.Text & "'")
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
        strSql = " SELECT SUBSTRING(RUNNO,6,20) REFNO"
        strSql += vbCrLf + " ,CASE WHEN WEIGHT <> 0 THEN WEIGHT ELSE NULL END AS WEIGHT"
        strSql += vbCrLf + " ,x.TRANDATE,V.PREVILEGEID,V.ACCODE,V.TITLE,V.INITIAL,V.PNAME,V.DOORNO,V.ADDRESS1,V.ADDRESS2,V.ADDRESS3"
        strSql += vbCrLf + " ,V.AREA,V.CITY,V.STATE,V.COUNTRY,V.PINCODE,V.PHONERES,V.MOBILE,V.EMAIL,V.FAX,V.SNO,X.COMPANYID"
        strSql += vbCrLf + " ,RUNNO FROM"
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " SELECT RUNNO"
        strSql += vbCrLf + "  ,SUM(CASE WHEN RECPAY = 'R' THEN GRSWT  ELSE -1*GRSWT  END)AS WEIGHT"
        strSql += vbCrLf + " ,(SELECT TOP 1 TRANDATE FROM " & cnAdminDb & "..OUTSTANDING WHERE COSTID = '" & BILLCOSTID & "' AND RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  AND ISNULL(CANCEL,'') = '' ORDER BY TRANDATE DESC)AS TRANDATE"
        strSql += vbCrLf + " ,(SELECT TOP 1 BATCHNO FROM " & cnAdminDb & "..OUTSTANDING WHERE COSTID = '" & BILLCOSTID & "' AND RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  AND ISNULL(CANCEL,'') = '' ORDER BY TRANDATE DESC)AS BATCHNO"
        strSql += vbCrLf + " ,O.COMPANYID "
        strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING O"
        strSql += vbCrLf + " WHERE TRANTYPE = 'GV'"
        strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
        strSql += vbCrLf + " AND COSTID = '" & BILLCOSTID & "'"
        strSql += vbCrLf + " AND RUNNO = '" & txtRefNo.Text & "'"
        strSql += vbCrLf + " AND REFNO = '" & Mcardid.ToString & "'"
        strSql += vbCrLf + " GROUP BY RUNNO,COMPANYID"
        strSql += vbCrLf + " HAVING SUM(CASE WHEN RECPAY = 'R' THEN GRSWT ELSE -1*GRSWT END) > 0 "
        strSql += vbCrLf + " )X"
        strSql += vbCrLf + " INNER JOIN TEMP" & systemId & "_CUSTOMERINFO AS V ON V.BATCHNO = X.BATCHNO"
        Dim row As DataRow = Nothing
        row = GetSqlRow(strSql, cn)
        If Not row Is Nothing Then
            With row
                For Each roAdv As DataRow In dtGridGiftVouhcer.Rows
                    If .Item("RUNNO").ToString = roAdv!RUNNO.ToString Then
                        MsgBox("Already Exist in this Gift Vou No", MsgBoxStyle.Information)
                        txtRefNo.Select()
                        txtRefNo.SelectAll()
                        Exit Sub
                    End If
                Next

                mAddressPrevilegeId = .Item("PREVILEGEID").ToString
                mAddressPartyCode = .Item("ACCODE").ToString
                mAddressTitle = .Item("TITLE").ToString
                mAddressInitial = .Item("INITIAL").ToString
                mAddressName = .Item("PNAME").ToString
                mAddressDoorNo = .Item("DOORNO").ToString
                mAddress1 = .Item("ADDRESS1").ToString
                mAddress2 = .Item("ADDRESS2").ToString
                mAddress3 = .Item("ADDRESS3").ToString
                mAddressarea = .Item("AREA").ToString
                mAddresscity = .Item("CITY").ToString
                mAddressstate = .Item("STATE").ToString
                mAddressCountry = .Item("COUNTRY").ToString
                mAddressPincode = .Item("PINCODE").ToString
                mAddressEmailid = .Item("EMAIL").ToString
                mAddressFax = .Item("FAX").ToString
                mAddressPhoneRes = .Item("PHONERES").ToString
                mAddressMobile = .Item("MOBILE").ToString
                mAddressRegularSno = .Item("SNO").ToString

                'dtpAdvanceDate.Value = .Item("TRANDATE")
                txtRefNo.Text = .Item("RUNNO")
                Mcardid = Val(.Item("REFNO"))
                txtGiftDenomination_AMT.Text = ""
                txtGiftAmount_AMT.Text = ""
                txtWtAdj_WET.Text = .Item("WEIGHT")
                txtGiftUnit_NUM.Text = ""
                'txtRefNo.Enabled = False
                txtGiftDenomination_AMT.Enabled = False
                txtGiftAmount_AMT.Enabled = False
                txtGiftUnit_NUM.ReadOnly = True
                txtGiftUnit_NUM.Focus()
            End With
        Else
            MsgBox("Please Enter Valid Voucher No", MsgBoxStyle.Information)
            txtRefNo.Text = "" : txtRefNo.Focus()
        End If

    End Sub

    Private Sub txtRefNo_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtRefNo.TextChanged

    End Sub

    Private Sub txtWtAdj_WET_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtWtAdj_WET.TextChanged
        CalcGiftVoucherAmount()
    End Sub
End Class