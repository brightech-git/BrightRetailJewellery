Public Class frmFinalDiscSal_Dia
    Dim dtSoftKeys As DataTable = GetAdmindbSoftValueAll()
    Dim FIN_DISC_ADJ_CTRLF2 As Boolean = IIf(GetAdmindbSoftValuefromDt(dtSoftKeys, "FINALDISC_ADJMODE_F2", "N") = "Y", True, False)
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
        objGPack.TextClear(Me)
    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles lblAmtWord.Click

    End Sub

    Private Sub txtFinalAmount_AMT_TextChanged(sender As Object, e As EventArgs) Handles txtFinalAmount_AMT.TextChanged
        If Val(txtFinalAmount_AMT.Text.ToString) <> 0 Then
            lblAmtWord.Text = ConvertRupees.RupeesToWord(Val(txtFinalAmount_AMT.Text.ToString), "", "")
        Else
            lblAmtWord.Text = ""
        End If
    End Sub

    Private Sub frmFinalDiscSal_Dia_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        gbadjOption.Visible = FIN_DISC_ADJ_CTRLF2
    End Sub
End Class