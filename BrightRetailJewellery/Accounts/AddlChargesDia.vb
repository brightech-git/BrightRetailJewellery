Imports System.Data.OleDb
Public Class AddlChargesDia
    Dim StrSql As String
    Dim Da As OleDbDataAdapter
    Public dtGridAddlCharges As New DataTable
    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        Initiator()
    End Sub

    Private Sub Initiator()
        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
        StrSql = "SELECT CHARGENAME FROM " & cnAdminDb & "..ADDCHARGE "
        StrSql += " WHERE ISNULL(ACTIVE,'') = 'Y' AND CHARGEMODULE = 'A' ORDER BY DISPLAYORDER,CHARGENAME"
        objGPack.FillCombo(StrSql, cmbChargeName)
        With dtGridAddlCharges
            .Columns.Add("CHARGENAME", GetType(String))
            .Columns.Add("AMOUNT", GetType(Decimal))
        End With
        gridAddCharge.DataSource = dtGridAddlCharges
        With gridAddCharge
            .Columns("CHARGENAME").Width = cmbChargeName.Width + 1
            .Columns("AMOUNT").Width = txtChargeAmount_AMT.Width + 1
            .Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("AMOUNT").DefaultCellStyle.Format = "0.00"
        End With
        gridAddChargeTotal.DataSource = dtGridAddlCharges.Clone
        CType(gridAddChargeTotal.DataSource, DataTable).Rows.Add()
        gridAddChargeTotal.Rows(gridAddChargeTotal.RowCount - 1).Cells("CHARGENAME").Value = "TOTAL"
        gridAddChargeTotal.ColumnHeadersVisible = False
        With gridAddChargeTotal
            .Columns("CHARGENAME").Width = cmbChargeName.Width + 1
            .Columns("AMOUNT").Width = txtChargeAmount_AMT.Width + 1
            .Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("AMOUNT").DefaultCellStyle.Format = "0.00"
        End With
    End Sub

    Private Sub AddlChargesDia_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub

    Private Sub AddlChargesDia_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtChargeAmount_AMT.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub AddlChargesDia_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        gridAddCharge.Font = txtChargeAmount_AMT.Font
        gridAddChargeTotal.Font = txtChargeAmount_AMT.Font
    End Sub

    Public Sub CalcAddlChargeTotal()
        dtGridAddlCharges.AcceptChanges()
        Dim totAmt As Decimal = Nothing
        For cnt As Integer = 0 To gridAddCharge.Rows.Count - 1
            totAmt += Val(gridAddCharge.Rows(cnt).Cells("AMOUNT").Value.ToString)
        Next
        gridAddChargeTotal.Rows(0).Cells("AMOUNT").Value = IIf(totAmt <> 0, Format(totAmt, "0.00"), DBNull.Value)
    End Sub

    Private Sub txtChargeAmount_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtChargeAmount_AMT.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If Not cmbChargeName.Items.Contains(cmbChargeName.Text) Then
                MsgBox("Invalid Selection", MsgBoxStyle.Information)
                cmbChargeName.Select()
                Exit Sub
            End If
            If Val(txtChargeAmount_AMT.Text) = 0 Then
                MsgBox("Amount should not Empty", MsgBoxStyle.Information)
                txtChargeAmount_AMT.Select()
                Exit Sub
            End If
            Dim Ro As DataRow = dtGridAddlCharges.NewRow
            Ro.Item("CHARGENAME") = cmbChargeName.Text
            Ro.Item("AMOUNT") = Val(txtChargeAmount_AMT.Text)
            dtGridAddlCharges.Rows.Add(Ro)
            dtGridAddlCharges.AcceptChanges()
            CalcAddlChargeTotal()
            txtChargeAmount_AMT.Clear()
            cmbChargeName.Text = ""
            cmbChargeName.Select()
        End If
    End Sub

    Private Sub gridAddCharge_UserDeletedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles gridAddCharge.UserDeletedRow
        CalcAddlChargeTotal()
    End Sub
End Class