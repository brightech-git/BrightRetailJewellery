Imports System.Data.OleDb

Public Class FRM_MANUALBILLNO
    Dim StrSql As String
    Dim TranTypeCol As New List(Of String)

    Dim GEN_GENSEPRETURN As Boolean = False

    Private TRANNO As Integer = 0
    Public ReadOnly Property pTRANNO() As Integer
        Get
            If txtSales_NUM.Enabled Then
                TRANNO = Val(txtSales_NUM.Text)
            ElseIf txtReturn_NUM.Enabled Then
                TRANNO = Val(txtReturn_NUM.Text)
            ElseIf txtPurchase_NUM.Enabled Then
                TRANNO = Val(txtPurchase_NUM.Text)
            ElseIf txtReceipt_NUM.Enabled Then
                TRANNO = Val(txtReceipt_NUM.Text)
            ElseIf txtPayment_NUM.Enabled Then
                TRANNO = Val(txtPayment_NUM.Text)
            End If
            Return TRANNO
        End Get
    End Property

    Public Sub New(ByVal TranTypeCol As List(Of String))

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
        Me.TranTypeCol = TranTypeCol


        If TranTypeCol.Contains("SA") Then
            lblSale.Text = "Sales"
            txtSales_NUM.Enabled = True
        End If
        If TranTypeCol.Contains("SR") Then
            txtReturn_NUM.Enabled = True
            If GetBillControlValue("GEN-GENSEPRETURN", tran) = "Y" Then
                GEN_GENSEPRETURN = True
            End If
        End If
        If TranTypeCol.Contains("PU") Then txtPurchase_NUM.Enabled = True
        If TranTypeCol.Contains("AI") Then
            txtSales_NUM.Enabled = True
            lblSale.Text = "App Issue"
        End If
        If TranTypeCol.Contains("AR") Then
            txtSales_NUM.Enabled = True
            lblSale.Text = "App Receipt"
        End If
        If TranTypeCol.Contains("MI") Then
            txtSales_NUM.Enabled = True
            lblSale.Text = "Misc Issue"
        End If
        If TranTypeCol.Contains("RE") Then txtReceipt_NUM.Enabled = True
        If TranTypeCol.Contains("PE") Then txtPayment_NUM.Enabled = True
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Dim isDuplicateBillCheck As Boolean = False
        StrSql = "SELECT ctltext FROM " + cnAdminDb + "..softcontrol WHERE ctlid='DUP_BILL_NO' AND ctltext='Y'"
        If objGPack.GetSqlValue(StrSql, , "").ToString() = "Y" Then
            isDuplicateBillCheck = True
        End If

        If txtSales_NUM.Enabled Then
            If Val(txtSales_NUM.Text) = 0 Then
                MsgBox("BillNo should not be empty", MsgBoxStyle.Information)
                txtSales_NUM.Focus()
                Exit Sub
            End If
            If TranTypeCol.Contains("AR") Then
                StrSql = " SELECT TOP 1 TRANNO FROM " & cnStockDb & "..RECEIPT WHERE TRANNO = " & Val(txtSales_NUM.Text) & ""
                StrSql += " AND TRANTYPE = 'AR'"
            Else
                StrSql = " SELECT TOP 1 TRANNO FROM " & cnStockDb & "..ISSUE WHERE TRANNO = " & Val(txtSales_NUM.Text) & ""
                If TranTypeCol.Contains("AI") Then
                    StrSql += " AND TRANTYPE = 'AI'"
                ElseIf TranTypeCol.Contains("MI") Then
                    StrSql += " AND TRANTYPE = 'MI'"
                Else
                    StrSql += " AND TRANTYPE IN ('SA','OD','RD')"
                End If
            End If
            If Val(objGPack.GetSqlValue(StrSql)) <> 0 Then
                If isDuplicateBillCheck Then
                    If MsgBox(lblSale.Text & " BillNo Already Exists.", MsgBoxStyle.Information) Then
                        txtSales_NUM.Focus()
                        Exit Sub
                    End If
                Else
                    If MessageBox.Show(lblSale.Text & " BillNo Already Exist. Do you wish to Continue?", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) <> Windows.Forms.DialogResult.OK Then
                        Exit Sub
                    End If
                End If
            End If
        End If
        If txtReturn_NUM.Enabled Then
            If Val(txtReturn_NUM.Text) = 0 Then
                MsgBox("Return BillNo should not empty", MsgBoxStyle.Information)
                txtReturn_NUM.Focus()
                Exit Sub
            End If
            StrSql = " SELECT TOP 1 TRANNO FROM " & cnStockDb & "..RECEIPT WHERE TRANNO = " & Val(txtReturn_NUM.Text) & ""
            StrSql += " AND TRANTYPE = 'SR'"
            If Val(objGPack.GetSqlValue(StrSql)) <> 0 Then
                If MessageBox.Show("Return BillNo Already Exist. Do you wish to Continue?", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) <> Windows.Forms.DialogResult.OK Then
                    Exit Sub
                End If
            End If
        End If
        If txtPurchase_NUM.Enabled Then
            If Val(txtPurchase_NUM.Text) = 0 Then
                MsgBox("Purchase BillNo should not empty", MsgBoxStyle.Information)
                txtPurchase_NUM.Focus()
                Exit Sub
            End If
            StrSql = " SELECT TOP 1 TRANNO FROM " & cnStockDb & "..RECEIPT WHERE TRANNO = " & Val(txtPurchase_NUM.Text) & ""
            StrSql += " AND TRANTYPE = 'PU'"
            If Val(objGPack.GetSqlValue(StrSql)) <> 0 Then
                If isDuplicateBillCheck Then
                    If MsgBox("Purchase BillNo Already Exists.", MsgBoxStyle.Information) Then
                        txtPurchase_NUM.Focus()
                        Exit Sub
                    End If
                Else
                    If MessageBox.Show("Purchase BillNo Already Exist. Do you wish to Continue?", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) <> Windows.Forms.DialogResult.OK Then
                        Exit Sub
                    End If
                End If
            End If
        End If
        If txtReceipt_NUM.Enabled Then
            If Val(txtReceipt_NUM.Text) = 0 Then
                MsgBox("Receipt BillNo should not empty", MsgBoxStyle.Information)
                txtReceipt_NUM.Focus()
                Exit Sub
            End If
            StrSql = " SELECT TOP 1 TRANNO FROM " & cnStockDb & "..ACCTRAN WHERE TRANNO = " & Val(txtReceipt_NUM.Text) & ""
            StrSql += " AND PAYMODE IN ('AR','DR','MR','OR','HR')"
            If Val(objGPack.GetSqlValue(StrSql)) <> 0 Then
                If MessageBox.Show("Receipt BillNo Already Exist. Do you wish to Continue?", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) <> Windows.Forms.DialogResult.OK Then
                    Exit Sub
                End If
            End If
        End If
        If txtPayment_NUM.Enabled Then
            If Val(txtPayment_NUM.Text) = 0 Then
                MsgBox("Payment BillNo should not empty", MsgBoxStyle.Information)
                txtPayment_NUM.Focus()
                Exit Sub
            End If
            StrSql = " SELECT TOP 1 TRANNO FROM " & cnStockDb & "..ACCTRAN WHERE TRANNO = " & Val(txtPayment_NUM.Text) & ""
            StrSql += " AND PAYMODE IN ('AP','DP','MP','HP')"
            If Val(objGPack.GetSqlValue(StrSql)) <> 0 Then
                If MessageBox.Show("Payment BillNo Already Exist. Do you wish to Continue?", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) <> Windows.Forms.DialogResult.OK Then
                    Exit Sub
                End If
            End If
        End If
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub FRM_MANUALBILLNO_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtSales_NUM_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSales_NUM.TextChanged
        If GEN_GENSEPRETURN = False And txtReturn_NUM.Enabled Then
            txtReturn_NUM.Text = txtSales_NUM.Text
        End If
    End Sub

    Private Sub txtReturn_NUM_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtReturn_NUM.Enter
        If GEN_GENSEPRETURN = False Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
End Class