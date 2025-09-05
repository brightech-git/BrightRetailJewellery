Imports System.Data.OleDb



Public Class frmBillInitialize
    Public Enum BillTypee
        Retail = 0
        WholeSale = 1
        CashPoint = 2
    End Enum
    Dim strSql As String
    Public BillType As BillTypee = BillTypee.Retail
    Dim NODEWISECACTR As Boolean = False
    Dim cmd As OleDbCommand
    Dim DATECHANGE_DISABLE As String = GetAdmindbSoftValue("DATECHANGE_DISABLE", "").ToUpper()
    Dim GLBDATERESTRICT As String = GetAdmindbSoftValue("GLOBALDATERESTRICT", "W").ToUpper()
    Dim CASHCTR_COSTID As Boolean = IIf((GetAdmindbSoftValue("CASHCTR_COSTID", "N").ToString) = "Y", True, False)

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub frmBillInitialize_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub frmBillInitialize_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        NODEWISECACTR = IIf(GetAdmindbSoftValue("NODEWISECACTR", "N").ToUpper() = "Y", True, False)
        If userId <> 999 Then
            strSql = "select ISNULL(PWDACCESS,'Y') PWDACCESS FROM " & cnAdminDb & "..ROLEMASTER WHERE ROLEID = (SELECT TOP 1 ROLEID FROM " & cnAdminDb & "..USERROLE WHERE USERID = " & userId & ")"
            If objGPack.GetSqlValue(strSql) = "N" Then lblChangePassword.Visible = False 'MsgBox("Access Denied") : Exit Sub
        End If
        Dim billtypestr As String = IIf(BillType = 0, "P", "C")
        strSql = " SELECT CASHNAME FROM " & cnAdminDb & "..CASHCOUNTER WHERE ISNULL(ACTIVE,'Y') <> 'N'"
        If NODEWISECACTR Then strSql += " AND  NODEID='" & systemId & "'"
        If cnCostId <> "" Then strSql += " AND ISNULL(COSTID,'') <> ''"
        If CASHCTR_COSTID And cnCostId <> "" Then
            strSql += " AND ISNULL(COSTID,'') = '" & cnCostId & "'"
        End If
        If BillType = 2 Then strSql += " and isnull(Countertype,'C') IN ('','C')"
        If BillType = 0 Or BillType = 1 Then strSql += " and isnull(Countertype,'B') IN('','B')"
        strSql += " ORDER BY CASHNAME"

        objGPack.FillCombo(strSql, cmbCashCounter)
        Dim serverDate As Date = GetEntryDate(GetServerDate(tran))
        dtpBillDate.MaximumDate = cnTranToDate ' (New DateTimePicker).MaxDate
        dtpBillDate.MinimumDate = cnTranFromDate  '(New DateTimePicker).MinDate
        dtpBillDate.Value = serverDate.Date.ToString("yyyy-MM-dd")
        If DATECHANGE_DISABLE.Contains(billtypestr) = True Then
            dtpBillDate.MaximumDate = serverDate ' (New DateTimePicker).MaxDate
            dtpBillDate.MinimumDate = serverDate '(New DateTimePicker).MinDate
        End If
        txtPassword.CharacterCasing = CharacterCasing.Normal

        rbtAmount.Checked = True
        chkTouchBased.Checked = False
        lblBalanceIn.Visible = False
        rbtAmount.Visible = False
        rbtWeight.Visible = False
        chkTouchBased.Visible = False
        If BillType = BillTypee.Retail And WTAMTOPT Then
            lblBalanceIn.Visible = True
            rbtAmount.Visible = True
            rbtWeight.Visible = True
            chkTouchBased.Visible = True
        End If
        dtpBillDate.Focus()
    End Sub
    Private Sub ShowRetail()
        Dim CostId As String = Nothing
        Dim CashId As String = Nothing
        Dim objBill As New RetailBill
        objBill.Hide()
        strSql = " select * from " & cnAdminDb & "..CashCounter where CashName = '" & cmbCashCounter.Text & "' and Password = '" & objGPack.Encrypt(txtPassword.Text) & "'"
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            CashId = dt.Rows(0).Item("CashId").ToString
            CostId = dt.Rows(0).Item("CostId").ToString
            If cnCostId <> "" And CostId = "" Then
                MsgBox("Cashcounter Costcentre is Empty", MsgBoxStyle.Information)
                Exit Sub
            End If
            If cnCostId <> CostId Then
                MsgBox("Base Costcentre and Cash Counter costcentre differ" & vbCrLf & cnCostId & "<>" & CostId, MsgBoxStyle.Information)
                'btnOk.Enabled = True
                'Exit Sub
            End If

            objBill.BillDate = dtpBillDate.Value.Date.ToString("yyyy-MM-dd")
            objBill.BillCashCounterId = dt.Rows(0).Item("CashId").ToString
            objBill.BillCostId = dt.Rows(0).Item("CostId").ToString
            objBill.lblUserName.Text = cnUserName
            objBill.lblSystemId.Text = systemId
            objBill.lblCashCounter.Text = dt.Rows(0).Item("CASHNAME").ToString
            objBill.lblBillDate.Text = dtpBillDate.Text
            objBill.Set916Rate(dtpBillDate.Value)
            objBill.MANBILLNO = IIf(UCase(dt.Rows(0).Item("MANUALBILLNO").ToString) = "Y", True, False)
            If Val(objBill.lblGoldRate.Text) = 0 Then
                MsgBox("Rate not updated in Rate Master", MsgBoxStyle.Information)
                btnOk.Enabled = True
                Exit Sub
            End If
        Else
            btnOk.Enabled = True
            MsgBox("Invalid Password")
            txtPassword.Focus()
            Exit Sub
        End If
        txtPassword.Clear()
        dtpBillDate.Focus()
        objBill.WindowState = FormWindowState.Minimized
        'objBill.MaximumSize = Main.Size
        GiritechPack.LanguageChange.Set_Language_Form(objBill, LangId)
        objGPack.Validator_Object(objBill)
        objBill.KeyPreview = True
        objBill.Size = New Size(1032, 745)
        objBill.MaximumSize = New Size(1032, 745)
        objBill.StartPosition = FormStartPosition.Manual
        objBill.Location = New Point((ScreenWid - objBill.Width) / 2, ((ScreenHit - 25) - objBill.Height) / 2)
        'objBill.Location = New Point(-3, -3)
        objBill.MaximizeBox = False
        objBill.Text = "Retail : " & objBill.Text & " "
        If BillType = BillTypee.Retail And WTAMTOPT Then
            objBill.pBALANCEINWEIGHT = rbtWeight.Checked
            objBill.TOUCHBASED = chkTouchBased.Checked
        End If
        objBill.Show()
        objBill.WindowState = FormWindowState.Normal
        btnOk.Enabled = True
        Me.Hide()
    End Sub

    Private Sub ShowCashPoint()
        Dim CostId As String = Nothing
        Dim CashId As String = Nothing
        Dim objBill As New cashpoint
        strSql = " select * from " & cnAdminDb & "..CashCounter where CashName = '" & cmbCashCounter.Text & "' and Password = '" & objGPack.Encrypt(txtPassword.Text) & "'"
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            CashId = dt.Rows(0).Item("CashId").ToString
            CostId = dt.Rows(0).Item("CostId").ToString
            objBill.MdiParent = Main
            objBill.BillDate = dtpBillDate.Value.Date.ToString("yyyy-MM-dd")
            objBill.BillCashCounterId = dt.Rows(0).Item("CashId").ToString
            objBill.BillCostId = dt.Rows(0).Item("CostId").ToString
            objBill.lblUserName.Text = cnUserName
            objBill.lblSystemId.Text = systemId
            objBill.lblCashCounter.Text = dt.Rows(0).Item("CASHNAME").ToString
            objBill.lblBillDate.Text = dtpBillDate.Text
            Dim gRate As Double = Nothing
            Dim sRate As Double = Nothing
            strSql = " SELECT TOP 1 SRATE FROM " & cnAdminDb & "..RATEMAST R"
            strSql += vbCrLf + " WHERE RDATE = '" & objBill.BillDate.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " AND RATEGROUP = (SELECT MAX(RATEGROUP) FROM " & cnAdminDb & "..RATEMAST WHERE RDATE = R.RDATE)"
            strSql += vbCrLf + " AND PURITY BETWEEN 91.6 AND 92 AND METALID = 'G'"
            strSql += vbCrLf + " AND SRATE <> 0"
            strSql += vbCrLf + " ORDER BY METALID,PURITY"
            gRate = Val(GiritechPack.GlobalMethods.GetSqlValue(cn, strSql))
            strSql = " SELECT TOP 1 SRATE FROM " & cnAdminDb & "..RATEMAST R"
            strSql += vbCrLf + " WHERE RDATE = '" & objBill.BillDate.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " AND RATEGROUP = (SELECT MAX(RATEGROUP) FROM " & cnAdminDb & "..RATEMAST WHERE RDATE = R.RDATE)"
            strSql += vbCrLf + " AND PURITY BETWEEN 91.6 AND 92 AND METALID = 'S'"
            strSql += vbCrLf + " AND SRATE <> 0"
            strSql += vbCrLf + " ORDER BY METALID,PURITY"
            sRate = Val(GiritechPack.GlobalMethods.GetSqlValue(cn, strSql))
            objBill.lblGoldRate.Text = IIf(gRate <> 0, Format(gRate, "0.00"), Nothing)
            objBill.lblSilverRate.Text = IIf(sRate <> 0, Format(sRate, "0.00"), Nothing)

            If Val(objBill.lblGoldRate.Text) = 0 Then
                MsgBox("Rate not updated in Rate Master", MsgBoxStyle.Information)
                btnOk.Enabled = True
                Exit Sub
            End If
        Else
            btnOk.Enabled = True
            MsgBox("Invalid Password")
            txtPassword.Focus()
            Exit Sub
        End If
        txtPassword.Clear()
        dtpBillDate.Focus()
        objGPack.Validator_Object(objBill)
        objBill.KeyPreview = True
        objBill.MaximizeBox = False
        objBill.Name = "CASHPOINT" & objBill.Name
        objBill.Show()
        btnOk.Enabled = True
        Me.Hide()
    End Sub

    Private Sub ShowWholeSale()
        Dim CostId As String = Nothing
        Dim CashId As String = Nothing
        Dim cashname As String = Nothing
        Dim Ismanualbill As Boolean = False
        strSql = " select * from " & cnAdminDb & "..CashCounter where CashName = '" & cmbCashCounter.Text & "' and Password = '" & objGPack.Encrypt(txtPassword.Text) & "'"
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            cashname = dt.Rows(0).Item("Cashname").ToString
            CashId = dt.Rows(0).Item("CashId").ToString
            CostId = dt.Rows(0).Item("CostId").ToString
            Ismanualbill = IIf(UCase(dt.Rows(0).Item("MANUALBILLNO").ToString) = "Y", True, False)
        Else
            btnOk.Enabled = True
            MsgBox("Invalid Password")
            txtPassword.Focus()
            Exit Sub
        End If
        If GetAdmindbSoftValue("WHOLESALETYPE", "D").ToUpper = "D" Then
            Dim objBill As New WholeSaleBill_ShortEntry
            objBill.Hide()
            objBill.BillDate = dtpBillDate.Value.Date.ToString("yyyy-MM-dd")
            objBill.BillCashCounterId = CashId
            objBill.BillCostId = CostId
            objBill.lblUserName.Text = cnUserName
            objBill.lblSystemId.Text = systemId
            objBill.lblCashCounter.Text = cashname
            objBill.lblBillDate.Text = dtpBillDate.Text
            objBill.Set916Rate(dtpBillDate.Value)
            objBill.MANBILLNO = Ismanualbill
            objBill.Text = "WholeSale : " & objBill.Text & " "
            objBill.ShowIcon = False
            If Val(objBill.lblGoldRate.Text) = 0 Then
                MsgBox("Rate not updated in Rate Master", MsgBoxStyle.Information)
                btnOk.Enabled = True
                Exit Sub
            End If
            txtPassword.Clear()
            dtpBillDate.Focus()
            objBill.WindowState = FormWindowState.Minimized
            'objBill.MaximumSize = Main.Size
            GiritechPack.LanguageChange.Set_Language_Form(objBill, LangId)
            objGPack.Validator_Object(objBill)
            objBill.KeyPreview = True
            objBill.Size = New Size(1032, 745)
            objBill.MaximumSize = New Size(1032, 745)
            objBill.StartPosition = FormStartPosition.Manual
            objBill.Location = New Point((ScreenWid - objBill.Width) / 2, ((ScreenHit - 25) - objBill.Height) / 2)
            'objBill.Location = New Point(-3, -3)
            objBill.MaximizeBox = False
            objBill.Show()
            objBill.WindowState = FormWindowState.Normal
        Else
            Dim objBill As New WholeSaleBill
            objBill.Hide()
            objBill.BillDate = dtpBillDate.Value.Date.ToString("yyyy-MM-dd")
            objBill.BillCashCounterId = CashId
            objBill.BillCostId = CostId
            objBill.lblUserName.Text = cnUserName
            objBill.lblSystemId.Text = systemId
            objBill.lblCashCounter.Text = cashname
            objBill.lblBillDate.Text = dtpBillDate.Text
            objBill.Set916Rate(dtpBillDate.Value)
            objBill.MANBILLNO = Ismanualbill
            objBill.Text = "WholeSale : " & objBill.Text & " "
            objBill.ShowIcon = False
            If Val(objBill.lblGoldRate.Text) = 0 Then
                MsgBox("Rate not updated in Rate Master", MsgBoxStyle.Information)
                btnOk.Enabled = True
                Exit Sub
            End If
            txtPassword.Clear()
            dtpBillDate.Focus()
            objBill.WindowState = FormWindowState.Minimized
            'objBill.MaximumSize = Main.Size
            GiritechPack.LanguageChange.Set_Language_Form(objBill, LangId)
            objGPack.Validator_Object(objBill)
            objBill.KeyPreview = True
            objBill.Size = New Size(1032, 745)
            objBill.MaximumSize = New Size(1032, 745)
            objBill.StartPosition = FormStartPosition.Manual
            objBill.Location = New Point((ScreenWid - objBill.Width) / 2, ((ScreenHit - 25) - objBill.Height) / 2)
            'objBill.Location = New Point(-3, -3)
            objBill.MaximizeBox = False
            objBill.Show()
            objBill.WindowState = FormWindowState.Normal
        End If
        Me.Hide()
        btnOk.Enabled = True
    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        If CheckTrialDate(dtpBillDate.Value) = False Then Exit Sub
        Dim serverDate1 As Date = GetServerDate()
        Dim serverDate As Date = GetActualEntryDate()

        'Dim minAllowDays As Integer = Val(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'DATECONTROL'"))
        'If Not (dtpBillDate.Value >= serverDate.AddDays(-1 * minAllowDays)) Then
        '    MsgBox("Invalid Billdate", MsgBoxStyle.Information)
        '    dtpBillDate.Focus()
        '    Exit Sub
        'End If
        If UCase(objGPack.GetSqlValue("SELECT DBNAME FROM " & cnAdminDb & "..DBMASTER WHERE '" & Format(dtpBillDate.Value.Date, "yyyy-MM-dd") & "' BETWEEN STARTDATE AND ENDDATE ", , "")) <> cnStockDb Then
            MsgBox("INVALID TRANSACTION DATABASE AND DATE SELECTION", MsgBoxStyle.Information)
            dtpBillDate.Focus()
            Exit Sub
        End If

        If Not CheckBckDaysEntry(userId, "", dtpBillDate.Value, "Retail") Then
            dtpBillDate.Focus()
            Exit Sub
        Else
            GoTo ExTL2
        End If
ExTL2:

        Dim DATECHANGEALLOW As Boolean = True
        If GetAdmindbSoftValue("GLOBALDATE", "N").ToUpper = "Y" Then DATECHANGEALLOW = False
        If UCase(objGPack.GetSqlValue("SELECT MANUALBILLNO FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME = '" & cmbCashCounter.Text & "'", , "N")) = "Y" Then DATECHANGEALLOW = True

        Dim glbDate As Date = GetAdmindbSoftValue("GLOBALDATEVAL", serverDate1)
        Dim RestrictDays As String = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'DATECONTROL'")
        Dim POS_ALLOW_BKDATE As Boolean = IIf(GetAdmindbSoftValue("POS_ALLOW_BKDATE", "N") = "Y", True, False)
        If RestrictDays.Contains(",") = False Then
            If Not DATECHANGEALLOW And Not POS_ALLOW_BKDATE Then RestrictDays = "0"
            If Not (dtpBillDate.Value >= serverDate.AddDays(-1 * Val(RestrictDays))) Then
                MsgBox("Invalid Bill Date", MsgBoxStyle.Information)
                dtpBillDate.Focus()
                Exit Sub
            End If
        Else
            Dim dayarray() As String = Split(RestrictDays, ",")
            Dim closeday As Integer = Val(dayarray(0).ToString)
            Dim mondiv As Integer = Val(dayarray(1).ToString)
            If Not DATECHANGEALLOW Then mondiv = "0"
            If closeday > serverDate.Day Then
                Dim mindate As Date = serverDate.AddMonths(-1 * mondiv)
                mindate = mindate.AddDays(-1 * (mindate.Day - 1))
                If Not (dtpBillDate.Value >= mindate) Then
                    MsgBox("Invalid Bill Date", MsgBoxStyle.Information)
                    dtpBillDate.Focus()
                    Exit Sub
                End If
            Else
                Dim mindate As Date = serverDate.AddMonths(-1 * 0)
                mindate = mindate.AddDays(-1 * (mindate.Day - 1))
                If Not (dtpBillDate.Value >= mindate) Then
                    MsgBox("Invalid Bill Date", MsgBoxStyle.Information)
                    dtpBillDate.Focus()
                    Exit Sub
                End If
            End If
        End If


        If dtpBillDate.Value < serverDate1 Then
            If GLBDATERESTRICT = "W" Then
                If MsgBox("System date more than Global date " & vbCrLf & "Can you proceed in Global date", MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                    dtpBillDate.Focus()
                    Exit Sub
                End If
            Else
                MsgBox("Global date Not more than System date Allowed", MsgBoxStyle.Critical)
                dtpBillDate.Focus()
                Exit Sub
            End If
        End If
        If objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'GLOBALDATE'").ToString = "Y" Then
            If (dtpBillDate.Value > glbDate) Then
                MsgBox("Billdate not more than Global date", MsgBoxStyle.Information)
                dtpBillDate.Focus()
                Exit Sub
            End If
        End If

        btnOk.Enabled = False
        If cmbCashCounter.Text = "" Then
            MsgBox(Me.GetNextControl(cmbCashCounter, False).Text + E0001, MsgBoxStyle.Information)
            Exit Sub
        End If


        txtPassword.CharacterCasing = CharacterCasing.Normal
        Dim counterName As String = UCase(objGPack.GetSqlValue("SELECT CASHNAME FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME = '" & cmbCashCounter.Text & "'", , Nothing))
        If counterName = Nothing Then
            MsgBox("Invalid Counter Name", MsgBoxStyle.Information)
            cmbCashCounter.Focus()
            cmbCashCounter.SelectAll()
            btnOk.Enabled = True
            Exit Sub
        End If
        Dim pwd As String = objGPack.GetSqlValue("SELECT PASSWORD FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME = '" & cmbCashCounter.Text & "' AND PASSWORD = '" & objGPack.Encrypt(txtPassword.Text) & "'", , Nothing)
        If GiritechPack.Methods.Encrypt(txtPassword.Text) <> pwd Then
            MsgBox("Invalid Password", MsgBoxStyle.Information)
            txtPassword.Focus()
            txtPassword.SelectAll()
            btnOk.Enabled = True
            Exit Sub
        End If
        If BillType = BillTypee.Retail Then
            ShowRetail()
        ElseIf BillType = BillTypee.WholeSale Then
            ShowWholeSale()
        ElseIf BillType = BillTypee.CashPoint Then
            ShowCashPoint()
        End If
    End Sub

    Private Sub cmbCashCounter_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbCashCounter.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If cmbCashCounter.Text = "" Then
                MsgBox(Me.GetNextControl(cmbCashCounter, False).Text + E0001, MsgBoxStyle.Information)
                Exit Sub
            ElseIf UCase(objGPack.GetSqlValue("SELECT MANUALBILLNO FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME = '" & cmbCashCounter.Text & "'")) = "Y" Then
                MsgBox("This Counter Having Manual Billno Generation", MsgBoxStyle.Information)
                SendKeys.Send("{TAB}")
            End If
        End If
    End Sub

    Private Sub dtpBillDate_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtpBillDate.GotFocus

    End Sub

    Private Sub dtpBillDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtpBillDate.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If CheckTrialDate(dtpBillDate.Value) = False Then Exit Sub
        End If
    End Sub

    Private Sub rbtAmount_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtAmount.CheckedChanged
        If rbtAmount.Checked Then
            chkTouchBased.Checked = False
        End If
        chkTouchBased.Visible = Not rbtAmount.Checked
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub dtpBillDate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtpBillDate.TextChanged

    End Sub

    Private Sub lblChangePassword_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblChangePassword.Click
        If cmbCashCounter.Text = "" Then
            cmbCashCounter.Focus()
            Exit Sub
        End If
        Dim pwd As String = objGPack.GetSqlValue("SELECT PASSWORD FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME = '" & cmbCashCounter.Text & "' ", , Nothing)
        If pwd = Nothing Then
            MsgBox("Invalid Password", MsgBoxStyle.Information)
            txtPassword.Focus()
            txtPassword.SelectAll()
            Exit Sub
        End If
        If GiritechPack.Methods.Encrypt(txtPassword.Text) <> pwd Then
            MsgBox("Invalid Password", MsgBoxStyle.Information)
            txtPassword.Focus()
            txtPassword.SelectAll()
            Exit Sub
        End If
        
        Dim objNewPwd As New frmAdminNewPassword
        objNewPwd.txtAuthPassword.Visible = False : objNewPwd.lblAuthpwd.Visible = False

        If objNewPwd.ShowDialog = Windows.Forms.DialogResult.OK Then
            Try
                Dim updated As Date = GetEntryDate(GetServerDate())
                tran = cn.BeginTransaction
                strSql = " UPDATE " & cnAdminDb & "..CASHCOUNTER SET"
                strSql += " PASSWORD = '" & GiritechPack.Methods.Encrypt(objNewPwd.txtNewPwd.Text) & "'"
                strSql += " WHERE CASHNAME = '" & cmbCashCounter.Text & "'"
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.ExecuteNonQuery()
                tran.Commit()
                tran = Nothing
                txtPassword.Clear()
                txtPassword.Focus()
            Catch ex As Exception
                If Not tran Is Nothing Then tran.Rollback() : tran = Nothing
                MsgBox(ex.Message + vbCrLf + ex.StackTrace)
                Exit Sub
            End Try
        End If


    End Sub
End Class