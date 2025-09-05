Imports System.Data.OleDb
Public Class frmMIMRInitialize
    Public isReceipt As Boolean
    Dim strSql As String
    Dim NODEWISECACTR As Boolean = False
    Dim DATECHANGE_DISABLE As String = GetAdmindbSoftValue("DATECHANGE_DISABLE", "").ToUpper()
    Dim GLBDATERESTRICT As String = GetAdmindbSoftValue("GLOBALDATERESTRICT", "W").ToUpper()
    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click

        If CheckTrialDate(dtpBillDate.Value) = False Then Exit Sub
        Dim serverDate1 As Date = GetServerDate()
        Dim serverDate As Date = GetActualEntryDate()
        Dim DATECHANGEALLOW As Boolean = True
        If GetAdmindbSoftValue("GLOBALDATE", "N").ToUpper = "Y" Then DATECHANGEALLOW = False
        If UCase(objGPack.GetSqlValue("SELECT MANUALBILLNO FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME = '" & cmbCashCounter.Text & "'", , "N")) = "Y" Then DATECHANGEALLOW = True

        Dim glbDate As Date = GetAdmindbSoftValue("GLOBALDATEVAL", serverDate1)
        Dim RestrictDays As String = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'DATECONTROL'")

        If RestrictDays.Contains(",") = False Then
            If Not DATECHANGEALLOW Then RestrictDays = "0"
            If Not (dtpBillDate.Value >= serverDate.AddDays(-1 * Val(RestrictDays))) Then
                MsgBox("Invalid Order Date", MsgBoxStyle.Information)
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
                    MsgBox("Invalid Order Date", MsgBoxStyle.Information)
                    dtpBillDate.Focus()
                    Exit Sub
                End If
            Else
                Dim mindate As Date = serverDate.AddMonths(-1 * 0)
                mindate = mindate.AddDays(-1 * (mindate.Day - 1))
                If Not (dtpBillDate.Value >= mindate) Then
                    MsgBox("Invalid Order Date", MsgBoxStyle.Information)
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
                MsgBox("Order Booking date not more than Global date", MsgBoxStyle.Information)
                dtpBillDate.Focus()
                Exit Sub
            End If
        End If
        Dim CostId As String = Nothing
        Dim CashId As String = Nothing
        Dim objMIMR
        If isReceipt Then
            objMIMR = New MaterialIssRecTran(MaterialIssRecTran.MaterialType.Receipt)
        Else
            objMIMR = New MaterialIssRecTran(MaterialIssRecTran.MaterialType.Issue)
        End If
        Dim MANBILLCOUNTER As Boolean = False
        objMIMR.Hide()
        btnOk.Enabled = False
        If cmbCashCounter.Text = "" Then
            MsgBox(Me.GetNextControl(cmbCashCounter, False).Text + E0001, MsgBoxStyle.Information)
            Exit Sub
        End If
        txtPassword.CharacterCasing = CharacterCasing.Normal
        Dim counterName As String = UCase(objGPack.GetSqlValue("SELECT CASHNAME FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME = '" & cmbCashCounter.Text & "'", , Nothing))
        If counterName = Nothing Then
            MsgBox("Invalid CounterName", MsgBoxStyle.Information)
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

        strSql = " select * from " & cnAdminDb & "..CashCounter where CashName = '" & cmbCashCounter.Text & "' and Password = '" & objGPack.Encrypt(txtPassword.Text) & "'"
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            objMIMR._CashCtr = dt.Rows(0).Item("CashId").ToString
            CostId = dt.Rows(0).Item("CostId").ToString
            If cnCostId <> "" And CostId = "" Then
                MsgBox("Cashcounter Costcentre is Empty", MsgBoxStyle.Information)
                Exit Sub
            End If
            If cnCostId <> CostId Then
                MsgBox("Base Costcentre and Cash Counter costcentre differ" & vbCrLf & cnCostId & "<>" & CostId, MsgBoxStyle.Information)
            End If
        Else
            btnOk.Enabled = True
            MsgBox("Invalid Password")
            txtPassword.Focus()
            Exit Sub
        End If
        txtPassword.Clear()
        dtpBillDate.Focus()
        GiritechPack.LanguageChange.Set_Language_Form(objMIMR, LangId)
        objGPack.Validator_Object(objMIMR)
        objMIMR.KeyPreview = True
        objMIMR.MdiParent = Main
        objMIMR.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        objMIMR.Dock = DockStyle.Fill
        objMIMR.WindowState = FormWindowState.Maximized

        'Dim f As New Form
        objMIMR.Show()
        btnOk.Enabled = True
        Me.Hide()
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

    Private Sub dtpBillDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtpBillDate.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If CheckTrialDate(dtpBillDate.Value) = False Then Exit Sub
        End If
    End Sub

    Private Sub frmMIMRInitialize_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmMIMRInitialize_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        NODEWISECACTR = IIf(GetAdmindbSoftValue("NODEWISECACTR", "N").ToUpper() = "Y", True, False)
        strSql = " SELECT CASHNAME FROM " & cnAdminDb & "..CASHCOUNTER WHERE ISNULL(ACTIVE,'Y') <> 'N'"
        If NODEWISECACTR Then strSql += " AND NODEID='" & systemId & "'"
        If cnCostId <> "" Then strSql += " AND ISNULL(COSTID,'') <> ''"
        strSql += " ORDER BY CASHNAME"
        objGPack.FillCombo(strSql, cmbCashCounter)
        dtpBillDate.MaximumDate = (New DateTimePicker).MaxDate
        dtpBillDate.MinimumDate = (New DateTimePicker).MinDate
        dtpBillDate.Value = GetEntryDate(GetServerDate)
        If DATECHANGE_DISABLE.Contains("O") = True Then
            dtpBillDate.MaximumDate = dtpBillDate.Value
            dtpBillDate.MinimumDate = dtpBillDate.Value
        End If
        txtPassword.CharacterCasing = CharacterCasing.Normal
        dtpBillDate.Focus()
    End Sub

End Class