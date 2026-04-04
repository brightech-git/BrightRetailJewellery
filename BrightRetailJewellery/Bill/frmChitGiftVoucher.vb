Imports System.Data.OleDb
Imports System.Web.WebSockets
Public Class frmChitGiftVoucher
    Dim strSql As String
    Dim cmd As OleDbCommand

    Dim chitMainDb As String = Nothing
    Dim chitTrandb As String = Nothing
    Public dtChitAdj As New DataTable
    Public dtReservedItem As New DataTable
    Dim chitEdit As Boolean = False
    Dim autoPost As Boolean = True
    Public ChitPaymode As String = "SS"
    Public BillDate As Date
    Public Tranflag As String = ""
    Dim dtsoftkeyss As New DataTable
    Dim mPartweightadj As Boolean = False
    Dim mWeight As Decimal = 0
    Dim mBalweight As Decimal = 0
    Dim mAdjweight As Decimal = 0
    Dim mSettrate As Decimal = 0
    Dim mBonweight As Decimal = 0
    Dim Isclosed As String = "Y"
    Dim DaysinYear As Decimal = GetAdmindbSoftValue("DAYSINYEAR", 365)
    Dim PARTPREWTBONUS As Boolean = IIf(GetAdmindbSoftValue("PART_PREADICAL_BONUS", "N") = "Y", True, False)
    Dim IS_USERLEVELPWD As Boolean = IIf(GetAdmindbSoftValue("USERLEVELPWD", "N") = "Y", True, False)
    Dim INTER_TRF_CHITCLOSE As String = GetAdmindbSoftValue("INTER_TRF_CHITCLOSE", "N")
    Public InsBonus As Boolean = True
    Dim AuthPwdPass As Boolean = False
    Dim GstRecCode As String = GetAdmindbSoftValue("GSTACCODE", "")
    Dim GstRecAcc() As String
    Dim SCode As String
    Dim CCode As String
    Dim ICode As String
    Dim ScGstRecCode As String = GetAdmindbSoftValue("GSTACCODE_CHIT", "")
    Dim ScGstRecAcc() As String
    Dim SchemeSCode As String
    Dim SchemeCCode As String
    Dim SchemeICode As String
    Dim GstCalc As String = "I"
    Dim CLIENTID_AS_SLIPNO As Boolean = IIf(GetAdmindbSoftValue("CLIENTID_AS_SLIPNO", "N") = "Y", True, False)
    Dim SPECIFICBONUS As String = GetAdmindbSoftValue("SPECIFICBONUS", "")
    Dim Partlybonusschid As String = GetAdmindbSoftValue("PARTLYBONUSSCHID", "")
    Dim chitClose_mdateDays As String = GetAdmindbSoftValue("CHIT_CLOSE_MDATE_PREDAYS", "0")
    Dim SPECIFICFORMAT As String = GetAdmindbSoftValue("SPECIFICFORMAT", "0")
    Dim _chitNoOffLock As Boolean = False
    Public dt As New DataTable

    Public Sub New(ByVal starting As Boolean)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
    End Sub

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        Dim CTLIDS As String = "" 'Where ctlid like 'CHIT%' or CTLID IN('OTHERSMODE','ACTUALRECEIPT','MONTHBONUS','METALRATE')"
        dtsoftkeyss = GetAdmindbSoftValueAll(CTLIDS)

        If chitdbchk() = False Then Me.Close() : Exit Sub

        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
        objGPack.TextClear(Me)
        ' Add any initialization after the InitializeComponent() call.
        ''CHEQUE
        strSql = " SELECT NAME FROM " & cnAdminDb & "..CREDITCARD WHERE CARDTYPE = 'C' ORDER BY NAME"
        objGPack.FillCombo(strSql, cmbCHITtCardType_MAN)
    End Sub
    Private Sub frmChidAdj_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
            Me.Close()
        End If
    End Sub
    Private Sub frmChidAdj_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Public Function chitdbchk() As Boolean
        If GetAdmindbSoftValuefromDt(dtsoftkeyss, "CHITDB", "N").ToUpper <> "Y" Then
            MsgBox("SCHEME Transaction provision not enabled in this pack", MsgBoxStyle.Information)
            Me.DialogResult = Windows.Forms.DialogResult.Abort
            Return False
            Exit Function
        End If
        chitMainDb = GetAdmindbSoftValuefromDt(dtsoftkeyss, "CHITDBPREFIX", "") + "SAVINGS"
        chitTrandb = GetAdmindbSoftValuefromDt(dtsoftkeyss, "CHITDBPREFIX", "") + "SH0708"
        If Not objGPack.GetSqlValue("SELECT NAME FROM SYSDATABASES WHERE NAME = '" & chitMainDb & "'").Length > 0 Then
            MsgBox("SCHEME main database not found", MsgBoxStyle.Information)
            Me.DialogResult = Windows.Forms.DialogResult.Abort
            Return False
            Exit Function
        End If
        If Not objGPack.GetSqlValue("SELECT NAME FROM SYSDATABASES WHERE NAME = '" & chitTrandb & "'").Length > 0 Then
            MsgBox("SCHEME transaction database not found", MsgBoxStyle.Information)
            Me.DialogResult = Windows.Forms.DialogResult.Abort
            Return False
            Exit Function
        End If
        Return True
    End Function
    Private Sub txtChitCardRegNo_NUM_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCHITCardRegNo_NUM.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If Val(txtCHITCardRegNo_NUM.Text) = 0 Then
                MsgBox("Enter registration number.", MsgBoxStyle.Information)
                txtCHITCardRegNo_NUM.Focus()
                Exit Sub
            End If
            Dim oleDb As New OleDbDataAdapter
            strSql = $"SELECT * FROM {cnAdminDb}..GIFTVOUCHER where Groupcode = '{cmbGroupCode.Text}' and regno = {txtCHITCardRegNo_NUM.Text}"
            oleDb = New OleDbDataAdapter(strSql, cn)
            dt = New DataTable
            oleDb.Fill(dt)
            If dt.Rows.Count > 0 Then
                MsgBox("Gift voucher has been already issued.", MsgBoxStyle.Information)
                txtCHITCardRegNo_NUM.Text = ""
                txtCHITCardRegNo_NUM.Focus()
                Exit Sub
            End If

            strSql = $"select INSAMOUNT.*,SCHEMEMAST.REGNO from {chitMainDb}..INSAMOUNT"
            strSql += vbCrLf + $"JOIN {chitMainDb}..SCHEME on INSAMOUNT.SCHEMEID = SCHEME.SchemeId"
            strSql += vbCrLf + $"JOIN {chitMainDb}..SCHEMEMAST on INSAMOUNT.SCHEMEID = SCHEMEMAST.SCHEMEID AND INSAMOUNT.GROUPCODE = SCHEMEMAST.GROUPCODE"
            strSql += vbCrLf + $"WHERE 1=1"
            strSql += vbCrLf + $"AND SCHEMEMAST.DOCLOSE IS NULL"
            strSql += vbCrLf + $"AND SCHEME.schemeName = '{cmbCHITtCardType_MAN.Text}'"
            strSql += vbCrLf + $"AND INSAMOUNT.GROUPCODE = '{cmbGroupCode.Text}'"
            strSql += vbCrLf + $"AND SCHEMEMAST.REGNO = {txtCHITCardRegNo_NUM.Text}"
            oleDb = New OleDbDataAdapter(strSql, cn)
            dt = New DataTable
            oleDb.Fill(dt)
            If dt.Rows.Count = 0 Then
                MsgBox("Not an valid detail.", MsgBoxStyle.Information)
                txtCHITCardRegNo_NUM.Text = ""
                txtCHITCardRegNo_NUM.Focus()
                Exit Sub
            End If
            If Val(dt.Rows(0)("GIFTVALUE").ToString()) = 0 Then
                MsgBox("Gift value is zero.", MsgBoxStyle.Information)
                txtCHITCardRegNo_NUM.Text = ""
                txtCHITCardRegNo_NUM.Focus()
                Exit Sub
            End If

            Me.DialogResult = Windows.Forms.DialogResult.OK
        End If
    End Sub
    Private Sub cmbCHITtCardType_MAN_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbCHITtCardType_MAN.SelectedIndexChanged
        strSql = $"select INSAMOUNT.GROUPCODE from {chitMainDb}..INSAMOUNT JOIN {chitMainDb}..SCHEME on INSAMOUNT.SCHEMEID = SCHEME.SchemeId  WHERE 1=1 AND SCHEME.schemeName = '{cmbCHITtCardType_MAN.Text}'"
        objGPack.FillCombo(strSql, cmbGroupCode)
    End Sub
End Class