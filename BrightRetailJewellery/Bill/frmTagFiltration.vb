Imports System.Data.OleDb
Public Class frmTagFiltration
    Dim dtSoftKeys As DataTable = GetAdmindbSoftValueAll()
    Dim AmtWiseSearch As Boolean = IIf(GetAdmindbSoftValuefromDt(dtSoftKeys, "AMTWISESEARCH_EST", "Y") = "Y", True, False)
    Public AmtFilter As Boolean = False
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
        objGPack.TextClear(Me)
    End Sub
    Private Sub frmTagFiltration_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If Val(txtWeightFrom.Text) = 0 And Val(txtWeightTo.Text) = 0 Then
                Me.DialogResult = Windows.Forms.DialogResult.Cancel
            ElseIf Val(txtWeightFrom.Text) > Val(txtWeightTo.Text) Then
                MsgBox("To weight should not less than from weight", MsgBoxStyle.Information)
                Exit Sub
            Else
                Me.DialogResult = Windows.Forms.DialogResult.OK
            End If
            Me.Close()
        ElseIf e.KeyCode = Keys.Space Then
            If Not AmtWiseSearch Then Exit Sub
            AmtFilter = True
            lblWeight.Text = "Amount From"
            Me.Text = "Tag Amount Filteration"
        End If
    End Sub

    Private Sub frmTagFiltration_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtWeightTo.Focused Then
                If Val(txtWeightFrom.Text) = 0 And Val(txtWeightTo.Text) = 0 Then
                    Me.DialogResult = Windows.Forms.DialogResult.Cancel
                ElseIf Val(txtWeightFrom.Text) > Val(txtWeightTo.Text) Then
                    MsgBox("To weight should not less than from weight", MsgBoxStyle.Information)
                    Exit Sub
                Else
                    Me.DialogResult = Windows.Forms.DialogResult.OK
                End If
                Exit Sub
            End If
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmTagFiltration_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        AmtFilter = False
        lblWeight.Text = "Weight From"
        If Not AmtWiseSearch Then
            Me.Text = "Tag Weight Filtration"
        Else
            Me.Text = "Tag Weight Filtration [Press Space Bar For Amount Wise]"
        End If
        txtWeightFrom.Select()
    End Sub
End Class