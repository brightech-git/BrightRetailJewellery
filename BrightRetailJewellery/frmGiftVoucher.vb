Imports System.Data.OleDb
Public Class frmGiftVoucher
    Dim cmd As OleDbCommand
    Dim strSql As String
    Public objSoftKeys As New SoftKeys
    Dim dtSoftKeys As DataTable = GetAdmindbSoftValueAll()
    Public dtGridGiftVouhcer As New DataTable
    Public dtGridSASR As New DataTable
    Public BILLCOSTID As String
    Dim Mcardid As Integer
    Dim GV_VALIDATE As String = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID ='GV_VALIDATE'", , "N")
    Public mAddressPrevilegeId, mAddressPartyCode, mAddressTitle, mAddressInitial, mAddressName, mAddressDoorNo As String
    Public mAddress1, mAddress2, mAddress3, mAddressarea, mAddresscity, mAddressstate, mAddressCountry, mAddressPincode, mAddressEmailid As String
    Public mAddressFax, mAddressPhoneRes, mAddressMobile, mAddressRegularSno As String
    Dim GV_TYPE As Boolean = IIf(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID ='GV_TYPE_VALIDATE'", , "Y").ToString = "Y", True, False)
    Dim CENTR_DB_BR As Boolean = IIf(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID ='CENTR_DB_ALLCOSTID'", , "N").ToString = "Y", True, False)
    Dim GV_QRCODE As Boolean = IIf(GetAdmindbSoftValue("GV_QRCODE", "N") = "Y", True, False)
    Dim GV_QRCODE_DAYS As String = GetAdmindbSoftValue("GV_QRCODE_DAYS", "7")
    ''BOUNZ API
    Dim BOUNZ_SALES As Boolean = IIf(GetAdmindbSoftValue("BOUNZ_SALES", "N") = "Y", True, False)
    Dim BOUNZ_PUBLICKEY As String = GetAdmindbSoftValuefromDt(dtSoftKeys, "BOUNZ_PUBLICKEY", "")
    Dim BOUNZ_USERNAME As String = GetAdmindbSoftValuefromDt(dtSoftKeys, "BOUNZ_USERNAME", "")
    Dim BOUNZ_PASSWORD As String = GetAdmindbSoftValuefromDt(dtSoftKeys, "BOUNZ_PASSWORD", "")
    Dim BOUNZlOCK_URL As String = GetAdmindbSoftValuefromDt(dtSoftKeys, "BOUNZLOCK_URL", "")
    Dim BOUNZlOCKRELEASE_URL As String = GetAdmindbSoftValuefromDt(dtSoftKeys, "BOUNZlOCKRELEASE_URL", "")
    Dim BOUNZPROFILE_URL As String = GetAdmindbSoftValuefromDt(dtSoftKeys, "BOUNZPROFILE_URL", "")
    Dim BOUNZ_PARTNERID As String = GetAdmindbSoftValuefromDt(dtSoftKeys, "BOUNZ_PARTNERID", "")
    Dim BOUNZ_STOREID As String = GetAdmindbSoftValuefromDt(dtSoftKeys, "BOUNZ_STOREID", "")
    Dim BOUNZ_COUNTRYCODE As String = GetAdmindbSoftValuefromDt(dtSoftKeys, "BOUNZ_COUNTRYCODE", "")
    Dim GIFTWT2WT_STORE_AMT As String = IIf(GetAdmindbSoftValue("GIFTWT2WT_STORE_AMT", "N") = "Y", True, False)
    Public GV_Silverrate As Double = 0
    Public GV_Goldrate As Double = 0


    Private Sub frmGiftVoucher_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            dtGridGiftVouhcer.AcceptChanges()
            cmbGiftVoucherType.Select()
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub frmGiftVoucher_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
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
        strSql += " AND ISNULL(ACTIVE,'Y')<>'N'"
        objGPack.FillCombo(strSql, cmbGiftVoucherType)

        With dtGridGiftVouhcer
            .Columns.Add("CARDTYPE", GetType(String))
            .Columns.Add("RUNNO", GetType(String))
            .Columns.Add("REMARK", GetType(String))
            .Columns.Add("DENOMINATION", GetType(Double))
            '.Columns.Add("UNIT", GetType(Integer))
            .Columns.Add("UNIT", GetType(Decimal))
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
    Private Sub txtGiftDenomination_AMT_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtGiftDenomination_AMT.KeyDown
        If e.KeyCode = Keys.Down Then
            If gridGiftVoucher.RowCount > 0 Then gridGiftVoucher.Select()
        End If
    End Sub

    Private Sub txtGiftDenomination_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtGiftDenomination_AMT.TextChanged
        CalcGiftVoucherAmount()
    End Sub
    Private Sub txtGiftUnit_INT_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtGiftUnit_NUM.KeyDown
        If e.KeyCode = Keys.Down Then
            If gridGiftVoucher.RowCount > 0 Then gridGiftVoucher.Select()
        End If
    End Sub

    Private Sub txtGiftUnit_NUM_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtGiftUnit_NUM.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If cmbGiftVoucherType.Text <> "" And Val(txtGiftUnit_NUM.Text) > 0 Then Call txtGiftAmount_KeyPress(sender, e)
        End If
    End Sub

    Private Sub grpGiftVoucher_Load(sender As Object, e As EventArgs) Handles grpGiftVoucher.Load

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
            ''BOUNZ API
            If BOUNZ_SALES And cmbGiftVoucherType.Text.Contains("BOUNZ") Then
                If BOUNZlOCK_URL.ToString = "" Then
                    MsgBox("BOUNZ LOCKPOINT URL Should not be empty")
                    Exit Sub
                End If
                If BOUNZ_PUBLICKEY.ToString = "" Then
                    MsgBox("BOUNZ LOCKPOINT PUBLICKEY Should not be empty")
                    Exit Sub
                End If
                If BOUNZ_USERNAME.ToString = "" Then
                    MsgBox("BOUNZ LOCKPOINT UserName Should not be empty")
                    Exit Sub
                End If
                If BOUNZ_PASSWORD.ToString = "" Then
                    MsgBox("BOUNZ LOCKPOINT Password Should not be empty")
                    Exit Sub
                End If
                If BOUNZ_COUNTRYCODE.ToString = "" Then
                    MsgBox("BOUNZ COUNTRYCODE Should not be empty")
                    Exit Sub
                End If
                Dim dtgift As New DataTable
                strSql = "SELECT PREVILEGEID loyalty_id,MOBILE mobile_number"
                strSql += vbCrLf + " ,'" & BOUNZ_COUNTRYCODE & "' country_code"
                strSql += vbCrLf + " ,'" & BOUNZ_PARTNERID & "' partner_id"
                strSql += vbCrLf + " ,'" & BOUNZ_STOREID & "' store_id"
                strSql += vbCrLf + " ,'" & Val(txtGiftDenomination_AMT.Text) & "' points"
                strSql += vbCrLf + " ,'" & Val(txtGiftAmount_AMT.Text) & "' amount"
                strSql += vbCrLf + " ,'POSLOCKPOINT' activity_code"
                strSql += vbCrLf + " FROM " & cnAdminDb & "..ACHEAD WHERE PREVILEGEID='" & txtRefNo.Text & "'"
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtgift)
                If dtgift.Rows.Count > 0 Then
                    Dim crd As New CallApi.BounzInv.BOUNZ_TRANACTION
                    crd.USERNAME = BOUNZ_USERNAME
                    crd.PASSWORD = BOUNZ_PASSWORD
                    crd.PublicKey = BOUNZ_PUBLICKEY

                    Dim _api As New CallApi.PushData
                    Dim cls As New CallApi.BounzInv.Para
                    cls.Lockpoint = CallApi.B2BInv.ConvertDataTable(Of CallApi.BounzInv.LockPoint)(dtgift)(0)
                    _api.apiurl = BOUNZlOCK_URL
                    _api.tpkey = BOUNZ_PUBLICKEY
                    _api.username = BOUNZ_USERNAME
                    _api.password = BOUNZ_PASSWORD
                    Dim res As CallApi.BounzInv.BOUNZLOCKRESULT = _api.CallapiBounz1(Newtonsoft.Json.JsonConvert.SerializeObject(cls.Lockpoint))
                    If res.status Then
                        txtGiftRemark.Text = res.values.lock_point_id.ToString
                        Bounzprofile()
                    Else
                        MessageBox.Show(res.status_code.ToString + vbCrLf + res.message, "BOUNZ")
                        Exit Sub
                    End If
                Else
                    MsgBox("Invaild Bounz Membership-ID", MsgBoxStyle.Information)
                    Exit Sub
                End If
            End If


            If GV_QRCODE And Val(txtGiftAmount_AMT.Text) <> 0 Then
                strSql = "SELECT SUM(AMOUNT)AMOUNT FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO ='" & txtRefNo.Text & "' AND TRANTYPE='GV' AND RECPAY='P' AND ISNULL(CANCEL,'')<>'Y' "
                Dim chkamt As Double = Val(GetSqlValue(cn, strSql).ToString)
                If Val(chkamt) <> 0 Then
                    txtGiftAmount_AMT.Text = txtGiftAmount_AMT.Text - Val(chkamt)
                    If Val(txtGiftAmount_AMT.Text) <= 0 Then
                        MsgBox("Refno already adjusted", MsgBoxStyle.Information)
                        txtRefNo.Text = ""
                        txtGiftUnit_NUM.Text = ""
                        txtGiftDenomination_AMT.Text = ""
                        txtRefNo.SelectAll()
                        Exit Sub
                    End If
                End If
            End If
            If Val(txtGiftAmount_AMT.Text) = 0 Then
                Exit Sub
            End If

            If Val(txtGiftRowIndex.Text) = Val("") And txtGiftRowIndex.Text = "" Then
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
    Function BOUNZAPILOCKRELEASE(ByVal bounzid As String, ByVal lockid As String)
        If BOUNZlOCKRELEASE_URL.ToString = "" Then
            MsgBox("BOUNZ RELEASE URL Should not be empty")
            Exit Function
        End If
        If BOUNZ_PUBLICKEY.ToString = "" Then
            MsgBox("BOUNZ PUBLICKEY Should not be empty")
            Exit Function
        End If
        If BOUNZ_USERNAME.ToString = "" Then
            MsgBox("BOUNZ UserName Should not be empty")
            Exit Function
        End If
        If BOUNZ_PASSWORD.ToString = "" Then
            MsgBox("BOUNZ Password Should not be empty")
            Exit Function
        End If
        Dim filterstr As String = ""
        If bounzid.ToString <> "" And lockid.ToString <> "" Then
            filterstr = "RUNNO ='" & bounzid.ToString & "' AND REMARK ='" & lockid.ToString & "' "
        Else
            filterstr = ""
        End If

        For Each ro As DataRow In dtGridGiftVouhcer.Select(filterstr.ToString)
            If Not ro("CARDTYPE").ToString.Contains("BOUNZ") Then Continue For
            Dim dtrelease As New DataTable
            strSql = "SELECT '" & ro!RUNNO & "' loyalty_id"
            strSql += vbCrLf + " ,'" & BOUNZ_PARTNERID & "' partner_id"
            strSql += vbCrLf + " ,'" & BOUNZ_STOREID & "' store_id"
            strSql += vbCrLf + " ,'" & ro!REMARK & "' lock_id "
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtrelease)
            If dtrelease.Rows.Count > 0 Then
                Dim crd As New CallApi.BounzInv.BOUNZ_TRANACTION
                crd.USERNAME = BOUNZ_USERNAME
                crd.PASSWORD = BOUNZ_PASSWORD
                crd.PublicKey = BOUNZ_PUBLICKEY

                Dim _api As New CallApi.PushData
                Dim cls As New CallApi.BounzInv.Para
                cls.Releasaelockpoint = CallApi.B2BInv.ConvertDataTable(Of CallApi.BounzInv.ReleasaeLockPoint)(dtrelease)(0)
                _api.apiurl = BOUNZlOCKRELEASE_URL
                _api.tpkey = BOUNZ_PUBLICKEY
                _api.username = BOUNZ_USERNAME
                _api.password = BOUNZ_PASSWORD
                Dim res As CallApi.BounzInv.BOUNZLOCKRESULT = _api.CallapiBounz1(Newtonsoft.Json.JsonConvert.SerializeObject(cls.Releasaelockpoint))
                If res.status Then
                    'MessageBox.Show(res.status_code.ToString + vbCrLf + res.message, "BOUNZ")
                Else
                    'MessageBox.Show(res.status_code.ToString + vbCrLf + res.message, "BOUNZ")
                End If

            Else
                MsgBox("Invaild Bounz Membership-ID", MsgBoxStyle.Information)
            End If
        Next
    End Function
    Private Sub Bounzprofile()
        lblBalVal.Text = "..."
        If BOUNZPROFILE_URL.ToString = "" Then
            MsgBox("BOUNZ PROFILE URL Should not be empty")
            Exit Sub
        End If
        If BOUNZ_PUBLICKEY.ToString = "" Then
            MsgBox("BOUNZ LOCKPOINT PUBLICKEY Should not be empty")
            Exit Sub
        End If
        If BOUNZ_USERNAME.ToString = "" Then
            MsgBox("BOUNZ LOCKPOINT UserName Should not be empty")
            Exit Sub
        End If
        If BOUNZ_PASSWORD.ToString = "" Then
            MsgBox("BOUNZ LOCKPOINT Password Should not be empty")
            Exit Sub
        End If
        If BOUNZ_COUNTRYCODE.ToString = "" Then
            MsgBox("BOUNZ COUNTRYCODE Should not be empty")
            Exit Sub
        End If
        Dim dtgift As New DataTable
        Dim mobileno As String = GetSqlValue(cn, "SELECT TOP 1 MOBILE FROM " & cnAdminDb & "..ACHEAD WHERE PREVILEGEID='" & txtRefNo.Text & "'")
        If mobileno Is Nothing Then MsgBox("Invaild Bounz Membership-ID") : txtRefNo.Select() : Exit Sub
        strSql = " SELECT '" & txtRefNo.Text & "' loyalty_id "
        strSql += vbCrLf + " ,'" & mobileno.ToString & "' mobile_number"
        strSql += vbCrLf + " ,'" & BOUNZ_COUNTRYCODE & "' country_code"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtgift)
        If dtgift.Rows.Count > 0 Then
            Dim crd As New CallApi.BounzInv.BOUNZ_TRANACTION
            crd.USERNAME = BOUNZ_USERNAME
            crd.PASSWORD = BOUNZ_PASSWORD
            crd.PublicKey = BOUNZ_PUBLICKEY

            Dim _api As New CallApi.PushData
            Dim cls As New CallApi.BounzInv.Para
            cls.profile = CallApi.B2BInv.ConvertDataTable(Of CallApi.BounzInv.Profile)(dtgift)(0)

            _api.apiurl = BOUNZPROFILE_URL
            _api.tpkey = BOUNZ_PUBLICKEY
            _api.username = BOUNZ_USERNAME
            _api.password = BOUNZ_PASSWORD
            Dim res As CallApi.BounzInv.BOUNZPROFILE = _api.CallapiBounz2(Newtonsoft.Json.JsonConvert.SerializeObject(cls.profile))
            If res.status Then
                lblBalVal.Text = res.values(0).point_balance.ToString
            Else
                MessageBox.Show(res.status_code.ToString + vbCrLf + res.message, "BOUNZ")
                Exit Sub
            End If
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
                    txtGiftAmount_AMT.Text = Format(Val(.Item("AMOUNT").ToString), "0.00")
                    txtRefNo.Text = .Item("RUNNO").ToString
                    Mcardid = objGPack.GetSqlValue("SELECT CARDCODE FROM " & cnAdminDb & "..CREDITCARD WHERE CARDTYPE = 'G' AND NAME = '" & cmbGiftVoucherType.Text & "'", "CARDCODE", 0)
                    txtGiftRowIndex.Text = gridGiftVoucher.CurrentRow.Index
                    cmbGiftVoucherType.Select()
                End With
            End If
        End If
    End Sub
    Private Sub gridGiftVoucher_UserDeletingRow(sender As Object, e As DataGridViewRowCancelEventArgs) Handles gridGiftVoucher.UserDeletingRow
        If BOUNZ_SALES And e.Row.Cells("CARDTYPE").Value.ToString.Contains("BOUNZ") Then
            Dim bounzidDelRow As String = e.Row.Cells("RUNNO").Value.ToString
            Dim bounzlockidDelRow As String = e.Row.Cells("REMARK").Value.ToString
            BOUNZAPILOCKRELEASE(bounzidDelRow, bounzlockidDelRow)
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
            .Columns("AMOUNT").Width = txtGiftAmount_AMT.Width + 1
            .Columns("COMMISION").Visible = False
            .Columns("REFNO").Visible = False
        End With
    End Sub
    Private Sub CalcGiftVoucherAmount()
        Dim amt As Double = Nothing
        If GIFTWT2WT_STORE_AMT Then
            amt = CalcRoundoffAmt(Val(txtGiftDenomination_AMT.Text) * Val(txtGiftUnit_NUM.Text), "L")
        Else
            amt = Val(txtGiftDenomination_AMT.Text) * Val(txtGiftUnit_NUM.Text)
        End If
        txtGiftAmount_AMT.Text = IIf(amt <> 0, Format(amt, "0.00"), Nothing)
    End Sub

    Private Sub CalcGiftVoucherTotal()
        Dim amt As Double = Nothing
        For Each ro As DataRow In dtGridGiftVouhcer.Rows
            amt += Val(ro!AMOUNT.ToString)
        Next
        gridGiftVoucherTotal.Rows(0).Cells("AMOUNT").Value = IIf(amt <> 0, amt, DBNull.Value)
    End Sub

    Private Sub frmGiftVoucher_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        gridGiftVoucher.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        gridGiftVoucherTotal.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        gridGiftVoucherTotal.DefaultCellStyle.BackColor = grpGiftVoucher.BackgroundColor
        gridGiftVoucherTotal.DefaultCellStyle.SelectionBackColor = grpGiftVoucher.BackgroundColor
        CalcGiftVoucherAmount()
        If GV_TYPE = False Then cmbGiftVoucherType.Enabled = False
        If GV_QRCODE = True Then txtRefNo.MaxLength = 50 Else txtRefNo.MaxLength = 15

    End Sub

    Private Sub txtRefNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtRefNo.KeyDown
        If e.KeyCode = Keys.Insert Then
            strSql = "SELECT COUNT(*) CNT FROM " & cnAdminDb & "..CREDITCARD WHERE CARDTYPE = 'G'"
            If Val(objGPack.GetSqlValue(strSql).ToString) = 0 Then
                MsgBox("No Record found", MsgBoxStyle.Information)
                Exit Sub
            End If
            Mcardid = objGPack.GetSqlValue("SELECT CARDCODE FROM " & cnAdminDb & "..CREDITCARD WHERE CARDTYPE = 'G' AND NAME = '" & cmbGiftVoucherType.Text & "'", "CARDCODE", 0)
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
            strSql += vbCrLf + " ,CASE WHEN VALUE <> 0 THEN VALUE ELSE NULL END AS VALUE,ISNULL(X.WT ,0)WT,ISNULL(X.REMARK ,'')REMARK"
            strSql += vbCrLf + " ,x.TRANDATE,V.PREVILEGEID,V.ACCODE,V.TITLE,V.INITIAL,V.PNAME,V.DOORNO,V.ADDRESS1,V.ADDRESS2,V.ADDRESS3"
            strSql += vbCrLf + " ,V.AREA,V.CITY,V.STATE,V.COUNTRY,V.PINCODE,V.PHONERES,V.MOBILE,V.EMAIL,V.FAX,V.SNO,X.COMPANYID"
            strSql += vbCrLf + "  FROM"
            strSql += vbCrLf + " ("
            strSql += vbCrLf + " SELECT RUNNO"
            strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE -1*AMOUNT END)AS VALUE"
            strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN GRSWT ELSE 0 END)AS WT"
            strSql += vbCrLf + " ,(SELECT TOP 1 TRANDATE FROM " & cnAdminDb & "..OUTSTANDING WHERE COSTID = '" & BILLCOSTID & "' AND RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  AND ISNULL(CANCEL,'') = '' ORDER BY TRANDATE DESC)AS TRANDATE"
            strSql += vbCrLf + " ,(SELECT TOP 1 BATCHNO FROM " & cnAdminDb & "..OUTSTANDING WHERE COSTID = '" & BILLCOSTID & "' AND RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  AND ISNULL(CANCEL,'') = '' ORDER BY TRANDATE DESC)AS BATCHNO"
            strSql += vbCrLf + " ,(SELECT TOP 1 REMARK1 FROM " & cnAdminDb & "..OUTSTANDING WHERE COSTID = '" & BILLCOSTID & "' AND RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  AND ISNULL(CANCEL,'') = '' AND RECPAY = 'R' ORDER BY TRANDATE DESC)AS REMARK"
            strSql += vbCrLf + " ,O.COMPANYID "
            strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING O"
            strSql += vbCrLf + " WHERE TRANTYPE = 'GV'"
            'If Not withOrderDetail Then strSql += vbCrLf + " AND SUBSTRING(RUNNO,6,20) NOT LIKE 'O%'"
            'If Not withRepairDetail Then strSql += vbCrLf + " AND SUBSTRING(RUNNO,6,20) NOT LIKE 'R%'"
            strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
            strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
            strSql += vbCrLf + " AND COSTID = '" & BILLCOSTID & "'"
            If GV_TYPE Then
                strSql += vbCrLf + " AND REFNO = '" & Mcardid.ToString & "'"
            End If
            If GIFTWT2WT_STORE_AMT Then
                strSql += vbCrLf + " AND NOT EXISTS(SELECT 1 FROM " & cnAdminDb & "..OUTSTANDING WHERE COSTID = '" & BILLCOSTID & "' AND RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID   "
                strSql += vbCrLf + " AND ISNULL(CANCEL,'') = '' AND RECPAY='P' "
                strSql += vbCrLf + " AND EXISTS (SELECT 1 FROM " & cnAdminDb & "..OUTSTANDING WHERE COSTID = '" & BILLCOSTID & "' AND RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  "
                strSql += vbCrLf + " AND ISNULL(CANCEL,'') = '' AND RECPAY='R' AND ISNULL(GRSWT,0)<>0 AND ISNULL(AMOUNT,0)<>0)) "
            End If
            strSql += vbCrLf + " GROUP BY RUNNO,COMPANYID"
            strSql += vbCrLf + " HAVING SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE -1*AMOUNT END) > 0 "
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

                    If GIFTWT2WT_STORE_AMT And Val(.Item("WT").ToString) > 0 And .Item("REMARK").ToString <> "" Then
                        If .Item("REMARK").ToString.Contains("TO SILVER WEIGHT") Then
                            Dim _tempgvcalcamt As Double = 0
                            _tempgvcalcamt = CalcRoundoffAmt(Val(.Item("WT").ToString) * Val(GV_Silverrate), "L")
                            txtGiftUnit_NUM.Text = Val(.Item("WT").ToString)
                            txtGiftDenomination_AMT.Text = Val(GV_Silverrate)
                            txtGiftAmount_AMT.Text = _tempgvcalcamt
                        ElseIf .Item("REMARK").ToString.Contains("TO GOLD WEIGHT") Then
                            Dim _tempgvcalcamt As Double = 0
                            _tempgvcalcamt = CalcRoundoffAmt(Val(.Item("WT").ToString) * Val(GV_Goldrate), "L")
                            txtGiftUnit_NUM.Text = Val(.Item("WT").ToString)
                            txtGiftDenomination_AMT.Text = Val(GV_Goldrate)
                            txtGiftAmount_AMT.Text = _tempgvcalcamt
                        Else
                            txtGiftUnit_NUM.Text = 1
                            txtGiftDenomination_AMT.Text = .Item("VALUE")
                            txtGiftAmount_AMT.Text = .Item("VALUE")
                        End If
                    Else
                        txtGiftUnit_NUM.Text = 1
                        txtGiftDenomination_AMT.Text = .Item("VALUE")
                        txtGiftAmount_AMT.Text = .Item("VALUE")
                    End If

                    ''txtGiftDenomination_AMT.Text = .Item("VALUE")
                    ''txtGiftAmount_AMT.Text = .Item("VALUE")
                    ''txtGiftUnit_NUM.Text = 1

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
            If BOUNZ_SALES And cmbGiftVoucherType.Text.Contains("BOUNZ") Then
                Dim Bounz_RedeemPointVal As Decimal = Val(GetAdmindbSoftValuefromDt(dtSoftKeys, "BOUNZ_REDEEMPOINTVAL", "0"))
                txtGiftUnit_NUM.Text = Val(Bounz_RedeemPointVal)
                lblBal.Visible = True
                lblBalVal.Visible = True
                Bounzprofile()
            Else
                lblBal.Visible = False
                lblBalVal.Visible = False
            End If
            Dim refno As String = ""
            refno = txtRefNo.Text.ToString.Trim

            If refno.StartsWith("SG") Then
                If Val(dtGridSASR.Compute("SUM(DISCOUNT_FINAL)", "TRANTYPE='SA'").ToString) > 0 Then
                    MsgBox("F2 Discount is applied unable to add Gift Voucher...", MsgBoxStyle.Information)
                    txtRefNo.Text = ""
                    txtGiftDenomination_AMT.Text = ""
                    Exit Sub
                End If
                For Each dr As DataRow In dtGridSASR.Rows
                    If String.IsNullOrEmpty(dr("TRANTYPE").ToString) Then Continue For
                    strSql = "SELECT TOP 1 METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID='" & Val(dr("ITEMID").ToString).ToString & "'"
                    If objGPack.GetSqlValue(strSql, "", "").ToString <> "S" Then
                        MsgBox("Gift voucher not valid for this Item...", MsgBoxStyle.Information)
                        txtRefNo.Text = ""
                        txtGiftDenomination_AMT.Text = ""
                        Exit Sub
                    End If
                Next
            End If


            If refno.Contains("-") Then
                Dim Str() As String
                Str = refno.Split("-")
                txtRefNo.Text = Str(0)
                txtGiftDenomination_AMT.Text = Str(1)
                If GV_QRCODE = True And Str.Length = 4 Then
                    Dim Voucherdate As Date = Str(3).Replace("/", "-")
                    Dim chkdate As Date = GetServerDate()
                    If Val(DateDiff(DateInterval.Day, Voucherdate, chkdate).ToString) > Val(GV_QRCODE_DAYS.ToString) Then
                        MsgBox("Expired GiftVoucher", MsgBoxStyle.Information)
                        txtRefNo.Text = ""
                        txtGiftDenomination_AMT.Text = ""
                        Exit Sub
                    End If
                    If gridGiftVoucher.Rows.Count > 0 Then
                        For cnt As Integer = 0 To gridGiftVoucher.Rows.Count - 1
                            If gridGiftVoucher.Rows(cnt).Cells("RUNNO").Value.ToString <> "" Then
                                If txtRefNo.Text = gridGiftVoucher.Rows(cnt).Cells("RUNNO").Value.ToString Then
                                    MsgBox("Already Loaded this Refno", MsgBoxStyle.Information)
                                    txtRefNo.Text = ""
                                    txtGiftDenomination_AMT.Text = ""
                                    Exit Sub
                                End If
                            End If
                        Next
                    End If
                    txtGiftUnit_NUM.Text = Str(2)
                Else
                    txtGiftUnit_NUM.Text = 1
                End If

                txtGiftAmount_AMT.Text = Val(txtGiftDenomination_AMT.Text) * Val(txtGiftUnit_NUM.Text)
                txtGiftAmount_KeyPress(New Object, New KeyPressEventArgs(Chr(Keys.Enter)))
                Exit Sub
            End If
            If GV_VALIDATE = "N" Then txtGiftRemark.Focus() : Exit Sub
            If GV_VALIDATE = "M" And txtRefNo.Text.ToString = "" Then txtGiftDenomination_AMT.Focus() : Exit Sub
            'refno = txtRefNo.Text.ToString.Replace(GetCostId(cnCostId) & GetCompanyId(strCompanyId), "")
            'refno = GetCostId(cnCostId) & GetCompanyId(strCompanyId) & refno
            Mcardid = objGPack.GetSqlValue("SELECT CARDCODE FROM " & cnAdminDb & "..CREDITCARD WHERE CARDTYPE = 'G' AND NAME = '" & cmbGiftVoucherType.Text & "'", "CARDCODE", 0)
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
            strSql += vbCrLf + " ,CASE WHEN VALUE <> 0 THEN VALUE ELSE NULL END AS VALUE,ISNULL(X.WT ,0)WT,ISNULL(X.REMARK ,'')REMARK"
            strSql += vbCrLf + " ,x.TRANDATE,V.PREVILEGEID,V.ACCODE,V.TITLE,V.INITIAL,V.PNAME,V.DOORNO,V.ADDRESS1,V.ADDRESS2,V.ADDRESS3"
            strSql += vbCrLf + " ,V.AREA,V.CITY,V.STATE,V.COUNTRY,V.PINCODE,V.PHONERES,V.MOBILE,V.EMAIL,V.FAX,V.SNO,X.COMPANYID"
            strSql += vbCrLf + " ,RUNNO,CARDNO FROM"
            strSql += vbCrLf + " ("
            strSql += vbCrLf + " SELECT RUNNO"
            strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE -1*AMOUNT END)AS VALUE"
            strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN GRSWT ELSE 0 END)AS WT"
            If CENTR_DB_BR = False Then
                strSql += vbCrLf + " ,(SELECT TOP 1 TRANDATE FROM " & cnAdminDb & "..OUTSTANDING WHERE COSTID = '" & BILLCOSTID & "' AND RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  AND ISNULL(CANCEL,'') = '' ORDER BY TRANDATE DESC)AS TRANDATE"
                strSql += vbCrLf + " ,(SELECT TOP 1 BATCHNO FROM " & cnAdminDb & "..OUTSTANDING WHERE COSTID = '" & BILLCOSTID & "' AND RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  AND ISNULL(CANCEL,'') = '' ORDER BY TRANDATE DESC)AS BATCHNO"
                strSql += vbCrLf + " ,(SELECT TOP 1 REFNO FROM " & cnAdminDb & "..OUTSTANDING WHERE COSTID = '" & BILLCOSTID & "' AND RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  AND ISNULL(CANCEL,'') = '' ORDER BY TRANDATE DESC)AS CARDNO"
                strSql += vbCrLf + " ,(SELECT TOP 1 REMARK1 FROM " & cnAdminDb & "..OUTSTANDING WHERE COSTID = '" & BILLCOSTID & "' AND RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  AND ISNULL(CANCEL,'') = '' AND RECPAY = 'R' ORDER BY TRANDATE DESC)AS REMARK"
            Else
                strSql += vbCrLf + " ,(SELECT TOP 1 TRANDATE FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  AND ISNULL(CANCEL,'') = '' ORDER BY TRANDATE DESC)AS TRANDATE"
                strSql += vbCrLf + " ,(SELECT TOP 1 BATCHNO FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  AND ISNULL(CANCEL,'') = '' ORDER BY TRANDATE DESC)AS BATCHNO"
                strSql += vbCrLf + " ,(SELECT TOP 1 REFNO FROM " & cnAdminDb & "..OUTSTANDING WHERE  RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  AND ISNULL(CANCEL,'') = '' ORDER BY TRANDATE DESC)AS CARDNO"
                strSql += vbCrLf + " ,(SELECT TOP 1 REMARK1 FROM " & cnAdminDb & "..OUTSTANDING WHERE  RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  AND ISNULL(CANCEL,'') = '' AND RECPAY = 'R' ORDER BY TRANDATE DESC)AS REMARK"
            End If
            strSql += vbCrLf + " ,O.COMPANYID "
            strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING O"
            strSql += vbCrLf + " WHERE TRANTYPE = 'GV'"
            strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
            strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
            If CENTR_DB_BR = False Then
                strSql += vbCrLf + " AND COSTID = '" & BILLCOSTID & "'"
            End If
            strSql += vbCrLf + " AND RUNNO = '" & refno & "'"
            If GV_TYPE Then
                strSql += vbCrLf + " AND REFNO = '" & Mcardid.ToString & "'"
            End If
            If GIFTWT2WT_STORE_AMT Then
                strSql += vbCrLf + " AND NOT EXISTS(SELECT 1 FROM " & cnAdminDb & "..OUTSTANDING WHERE COSTID = '" & BILLCOSTID & "' AND RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID   "
                strSql += vbCrLf + " AND ISNULL(CANCEL,'') = '' AND RECPAY='P' "
                strSql += vbCrLf + " AND EXISTS (SELECT 1 FROM " & cnAdminDb & "..OUTSTANDING WHERE COSTID = '" & BILLCOSTID & "' AND RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  "
                strSql += vbCrLf + " AND ISNULL(CANCEL,'') = '' AND RECPAY='R' AND ISNULL(GRSWT,0)<>0 AND ISNULL(AMOUNT,0)<>0)) "
            End If
            strSql += vbCrLf + " GROUP BY RUNNO,COMPANYID"
            strSql += vbCrLf + " HAVING SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE -1*AMOUNT END) > 0 "
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
                    If GIFTWT2WT_STORE_AMT And Val(.Item("WT").ToString) > 0 And .Item("REMARK").ToString <> "" Then
                        If .Item("REMARK").ToString.Contains("TO SILVER WEIGHT") Then
                            Dim _tempgvcalcamt As Double = 0
                            _tempgvcalcamt = CalcRoundoffAmt(Val(.Item("WT").ToString) * Val(GV_Silverrate), "L")
                            txtGiftUnit_NUM.Text = Val(.Item("WT").ToString)
                            txtGiftDenomination_AMT.Text = Val(GV_Silverrate)
                            txtGiftAmount_AMT.Text = _tempgvcalcamt
                        ElseIf .Item("REMARK").ToString.Contains("TO GOLD WEIGHT") Then
                            Dim _tempgvcalcamt As Double = 0
                            _tempgvcalcamt = CalcRoundoffAmt(Val(.Item("WT").ToString) * Val(GV_Goldrate), "L")
                            txtGiftUnit_NUM.Text = Val(.Item("WT").ToString)
                            txtGiftDenomination_AMT.Text = Val(GV_Goldrate)
                            txtGiftAmount_AMT.Text = _tempgvcalcamt
                        Else
                            txtGiftDenomination_AMT.Text = .Item("VALUE")
                            txtGiftAmount_AMT.Text = .Item("VALUE")
                            txtGiftUnit_NUM.Text = 1
                        End If
                    Else
                        txtGiftDenomination_AMT.Text = .Item("VALUE")
                        txtGiftAmount_AMT.Text = .Item("VALUE")
                        txtGiftUnit_NUM.Text = 1
                    End If

                    'txtRefNo.Enabled = False
                    txtGiftDenomination_AMT.Enabled = False
                    txtGiftAmount_AMT.Enabled = False
                    txtGiftUnit_NUM.ReadOnly = True
                    'txtGiftUnit_NUM.Focus()
                    'txtGiftUnit_NUM.Select()
                    strSql = "SELECT NAME FROM " & cnAdminDb & "..CREDITCARD WHERE CARDCODE='" & .Item("CARDNO") & "'"
                    cmbGiftVoucherType.Text = objGPack.GetSqlValue(strSql, "NAME").ToString
                    txtGiftRemark.Focus()
                    txtGiftRemark.Select()

                    If txtGiftAmount_AMT.Enabled = False Then
                        txtGiftAmount_KeyPress(sender, e)
                    End If

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
        Mcardid = objGPack.GetSqlValue("SELECT CARDCODE FROM " & cnAdminDb & "..CREDITCARD WHERE CARDTYPE = 'G' AND NAME = '" & cmbGiftVoucherType.Text & "'", "CARDCODE", 0)
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
        strSql += vbCrLf + " ,CASE WHEN VALUE <> 0 THEN VALUE ELSE NULL END AS VALUE,ISNULL(X.WT ,0)WT,ISNULL(X.REMARK ,'')REMARK"
        strSql += vbCrLf + " ,x.TRANDATE,V.PREVILEGEID,V.ACCODE,V.TITLE,V.INITIAL,V.PNAME,V.DOORNO,V.ADDRESS1,V.ADDRESS2,V.ADDRESS3"
        strSql += vbCrLf + " ,V.AREA,V.CITY,V.STATE,V.COUNTRY,V.PINCODE,V.PHONERES,V.MOBILE,V.EMAIL,V.FAX,V.SNO,X.COMPANYID"
        strSql += vbCrLf + " ,RUNNO FROM"
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " SELECT RUNNO"
        strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE -1*AMOUNT END)AS VALUE"
        strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN GRSWT ELSE 0 END)AS WT"
        strSql += vbCrLf + " ,(SELECT TOP 1 TRANDATE FROM " & cnAdminDb & "..OUTSTANDING WHERE COSTID = '" & BILLCOSTID & "' AND RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  AND ISNULL(CANCEL,'') = '' ORDER BY TRANDATE DESC)AS TRANDATE"
        strSql += vbCrLf + " ,(SELECT TOP 1 BATCHNO FROM " & cnAdminDb & "..OUTSTANDING WHERE COSTID = '" & BILLCOSTID & "' AND RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  AND ISNULL(CANCEL,'') = '' ORDER BY TRANDATE DESC)AS BATCHNO"
        strSql += vbCrLf + " ,(SELECT TOP 1 REMARK1 FROM " & cnAdminDb & "..OUTSTANDING WHERE COSTID = '" & BILLCOSTID & "' AND RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  AND ISNULL(CANCEL,'') = '' AND RECPAY = 'R' ORDER BY TRANDATE DESC)AS REMARK"
        strSql += vbCrLf + " ,O.COMPANYID "
        strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING O"
        strSql += vbCrLf + " WHERE TRANTYPE = 'GV'"
        strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
        strSql += vbCrLf + " AND COSTID = '" & BILLCOSTID & "'"
        strSql += vbCrLf + " AND RUNNO = '" & txtRefNo.Text & "'"
        If GV_TYPE Then
            strSql += vbCrLf + " AND REFNO = '" & Mcardid.ToString & "'"
        End If
        If GIFTWT2WT_STORE_AMT Then
            strSql += vbCrLf + " AND NOT EXISTS(SELECT 1 FROM " & cnAdminDb & "..OUTSTANDING WHERE COSTID = '" & BILLCOSTID & "' AND RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID   "
            strSql += vbCrLf + " AND ISNULL(CANCEL,'') = '' AND RECPAY='P' "
            strSql += vbCrLf + " AND EXISTS (SELECT 1 FROM " & cnAdminDb & "..OUTSTANDING WHERE COSTID = '" & BILLCOSTID & "' AND RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  "
            strSql += vbCrLf + " AND ISNULL(CANCEL,'') = '' AND RECPAY='R' AND ISNULL(GRSWT,0)<>0 AND ISNULL(AMOUNT,0)<>0)) "
        End If
        strSql += vbCrLf + " GROUP BY RUNNO,COMPANYID"
        strSql += vbCrLf + " HAVING SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE -1*AMOUNT END) > 0 "
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
                If GIFTWT2WT_STORE_AMT And Val(.Item("WT").ToString) > 0 And .Item("REMARK").ToString <> "" Then
                    If .Item("REMARK").ToString.Contains("TO SILVER WEIGHT") Then
                        Dim _tempgvcalcamt As Double = 0
                        _tempgvcalcamt = CalcRoundoffAmt(Val(.Item("WT").ToString) * Val(GV_Silverrate), "L")
                        txtGiftDenomination_AMT.Text = _tempgvcalcamt
                        txtGiftAmount_AMT.Text = _tempgvcalcamt
                    ElseIf .Item("REMARK").ToString.Contains("TO GOLD WEIGHT") Then
                        Dim _tempgvcalcamt As Double = 0
                        _tempgvcalcamt = CalcRoundoffAmt(Val(.Item("WT").ToString) * Val(GV_Goldrate), "L")
                        txtGiftDenomination_AMT.Text = _tempgvcalcamt
                        txtGiftAmount_AMT.Text = _tempgvcalcamt
                    Else
                        txtGiftDenomination_AMT.Text = .Item("VALUE")
                        txtGiftAmount_AMT.Text = .Item("VALUE")
                    End If
                Else
                    txtGiftDenomination_AMT.Text = .Item("VALUE")
                    txtGiftAmount_AMT.Text = .Item("VALUE")
                End If
                ''txtGiftDenomination_AMT.Text = .Item("VALUE")
                ''txtGiftAmount_AMT.Text = .Item("VALUE")
                txtGiftUnit_NUM.Text = 1
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


End Class