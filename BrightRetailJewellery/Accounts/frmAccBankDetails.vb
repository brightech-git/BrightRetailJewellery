Public Class frmAccBankDetails

    Dim strSql As String
    Public DefDate As Date = GetEntryDate(GetServerDate)
    Public MANDATORY As Boolean = False
    Public _accode As String = ""
    Public _chqnumber As String = ""
    Dim ChequeBook As Boolean = IIf(GetAdmindbSoftValue("CHEQUENO_FROM_BOOK", "N") = "Y", True, False)
    Public oneTimeClear As Boolean = False
    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        strSql = " SELECT DISTINCT FAVNAME FROM " & cnAdminDb & "..ACCCHQFAVOUR WHERE active= 'Y' "
        objGPack.FillCombo(strSql, CmbChqDetail_OWN)
        ClearBankDetails()
    End Sub
    Public Sub ClearBankDetails()
        If oneTimeClear = False Then
            CmbChqDetail_OWN.Text = ""
            txtChqNo.Clear()
            dtpBankDate.Value = DefDate
            txtChqNo.Select()
        End If
    End Sub
    Private Sub frmAccBankDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If ChequeBook Then
                If CheckChqNo() = False Then Exit Sub
            End If
            If txtChqNo.Text = "" Then
                'If MANDATORY Then
                '    MsgBox("Cheque No should not Empty", MsgBoxStyle.Information)
                '    txtChqNo.Select()
                '    Exit Sub
                'End If
            End If
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub frmAccBankDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If CmbChqDetail_OWN.Focused Then Exit Sub
            If txtChqNo.Focused Then Exit Sub
            'strSql = " SELECT DISTINCT CHQCARDREF FROM " & cnStockDb & "..ACCTRAN WHERE FROMFLAG='A' and CHQCARDREF IS NOT NULL and CHQDATE is not null"
            'objGPack.FillCombo(strSql, CmbChqDetail_OWN)
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub CmbChqDetail_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles CmbChqDetail_OWN.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtChqNo.Text = "" Then

                'If MANDATORY Then
                '    MsgBox("Cheque No should not Empty", MsgBoxStyle.Information)
                '    txtChqNo.Select()
                '    Exit Sub
                'End If
            End If
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub txtChqNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtChqNo.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtChqNo.Text = "" Then
                'If MANDATORY Then
                '    MsgBox("Cheque No should not Empty", MsgBoxStyle.Information)
                '    txtChqNo.Select()
                '    Exit Sub
                'End If
                Me.DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()
            Else
                SendKeys.Send("{TAB}")
            End If
        End If
    End Sub

    Private Sub frmAccBankDetails_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        txtChqNo.Select()
    End Sub

    Private Sub btnRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRefresh.Click
        CmbChqDetail_OWN.DataSource = Nothing
        strSql = " SELECT DISTINCT FAVNAME FROM " & cnAdminDb & "..ACCCHQFAVOUR WHERE active= 'Y' "
        objGPack.FillCombo(strSql, CmbChqDetail_OWN)
    End Sub

    Private Sub txtChqNo_Leave(sender As Object, e As EventArgs) Handles txtChqNo.Leave
        CheckChqNo()
    End Sub

    Private Function CheckChqNo() As Boolean
        If ChequeBook Then
            If txtChqNo.Text.ToString <> "" Then
                _chqnumber = Val(objGPack.GetSqlValue("SELECT TOP 1 CHQNUMBER FROM " & cnStockDb & "..CHEQUEBOOK WHERE " & IIf(cnCostId <> "", "COSTID='" & cnCostId & "' AND", "") & " BANKCODE='" & _accode & "' AND CHQNUMBER='" & Val(txtChqNo.Text.ToString) & "' AND ISNULL(CANCEL,'')='' AND CHQISSUEDATE IS NULL order by chqnumber", "CHQNUMBER", "0"))
                If _chqnumber = 0 Then
                    MsgBox("Invalid Cheque No")
                    txtChqNo.Focus()
                    txtChqNo.Select()
                    txtChqNo.SelectAll()
                    txtChqNo.Text = objGPack.GetSqlValue("SELECT TOP 1 CHQNUMBER FROM " & cnStockDb & "..CHEQUEBOOK WHERE " & IIf(cnCostId <> "", "COSTID='" & cnCostId & "' AND", "") & " BANKCODE='" & _accode & "' AND ISNULL(CANCEL,'')='' AND CHQISSUEDATE IS NULL order by chqnumber", "CHQNUMBER", "0")
                    Return False
                End If
            End If
            Return True
        End If
    End Function

End Class