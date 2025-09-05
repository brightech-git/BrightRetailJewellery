Imports System.Data.OleDb
Public Class ClosingDenomination
    Dim strsql As String
    Dim Cmd As New OleDbCommand
    Dim DT As New DataTable
    Dim fdt As New DataTable

    'Private Sub ClosingDenomination_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
    '    If e.KeyCode = Keys.Enter Then
    '        SendKeys.Send("{TAB}")
    '    End If
    'End Sub

    Private Sub ClosingDenomination_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) And txtDenthous.Focused <> True Then
            SendKeys.Send("{TAb}")
        End If
    End Sub
    Private Sub ClosingDenomination_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        InitializeFunc()
    End Sub
#Region "TextBox Events"
    Private Sub txtden5_OWN_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtden5_OWN.TextChanged
        txtAmt5coin_OWN.Text = (Val(txtden5_OWN.Text) * 5).ToString
        AmtCalculation()
    End Sub

    Private Sub txtden1_OTHERS_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtden1_OTHERS.TextChanged
        txtAmtothers_OWN.Text = (Val(txtden1_OTHERS.Text) * 1).ToString
        AmtCalculation()
    End Sub

    Private Sub txtden1_OWN_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtden1_OWN.TextChanged
        txtAmt1_OWN.Text = (Val(txtden1_OWN.Text) * 1).ToString
        AmtCalculation()
    End Sub

    Private Sub txtden2_OWN_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtden2_OWN.TextChanged
        txtAmt2_OWN.Text = (Val(txtden2_OWN.Text) * 2).ToString
        AmtCalculation()
    End Sub

    Private Sub txtden10_OWN_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtden10_OWN.TextChanged
        txtAmt10_OWN.Text = (Val(txtden10_OWN.Text) * 10).ToString
        AmtCalculation()
    End Sub

    Private Sub txtden20_OWN_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtden20_OWN.TextChanged
        txtAmt20_OWN.Text = (Val(txtden20_OWN.Text) * 20).ToString
        AmtCalculation()
    End Sub

    Private Sub txtden50_OWN_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtden50.TextChanged
        txtAmt50_OWN.Text = (Val(txtden50.Text) * 50).ToString
        AmtCalculation()
    End Sub

    Private Sub txtden1hrd_OWN_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtden1hrd.TextChanged
        txtAmt1hrd_OWN.Text = (Val(txtden1hrd.Text) * 100).ToString
        AmtCalculation()
    End Sub

    Private Sub txtden5hrd_OWN_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtden5hrd.TextChanged
        txtAmt5hrd_OWN.Text = (Val(txtden5hrd.Text) * 200).ToString
        AmtCalculation()
    End Sub

    Private Sub txtDenthous_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtDenthous.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
                    SendKeys.Send("{TAb}")
        End If
    End Sub

    Private Sub txtDenthous_OWN_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDenthous.TextChanged
        txtAmttho_OWN.Text = (Val(txtDenthous.Text) * 500).ToString
        AmtCalculation()
    End Sub
#End Region

#Region "Key Events"
    'Private Sub DatePicker1_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles DatePicker1.KeyDown
    '    If e.KeyCode = Keys.Enter Then
    '        cmbCntId_OWN.Focus()
    '    End If
    'End Sub
    'Private Sub DatePicker1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles DatePicker1.KeyPress
    '    If e.KeyChar = Chr(Keys.Enter) Then
    '        cmbCntId_OWN.Focus()
    '    End If
    'End Sub
    
    Private Sub cmbCntId_OWN_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbCntId_OWN.KeyDown
        If e.KeyCode = Keys.Enter Then
            FillData()
        End If
    End Sub
#End Region

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Funsave()
    End Sub

#Region "Defined Function"

    Function InitializeFunc()
        CmbLoad()
        txtDenthous.Text = "0.00"
        txtden5hrd.Text = "0.00"
        txtden1hrd.Text = "0.00"
        txtden50.Text = "0.00"
        txtden20_OWN.Text = "0.00"
        txtden10_OWN.Text = "0.00"
        txtden5_OWN.Text = "0.00"
        txtden2_OWN.Text = "0.00"
        txtden1_OWN.Text = "0.00"
        txtden1_OTHERS.Text = "0.00"

        txtAmttho_OWN.Text = "0.00"
        txtAmt5hrd_OWN.Text = "0.00"
        txtAmt1hrd_OWN.Text = "0.00"
        txtAmt50_OWN.Text = "0.00"
        txtAmt20_OWN.Text = "0.00"
        txtAmt10_OWN.Text = "0.00"
        txtAmt5coin_OWN.Text = "0.00"
        txtAmt2_OWN.Text = "0.00"
        txtAmt1_OWN.Text = "0.00"
        txtAmtothers_OWN.Text = "0.00"
        TextBox1.Text = "0.00"
        txtGvdebit.Text = "0.00"
        txtGvcredit.Text = "0.00"
        txtGvTotal.Text = "0.00"
        txtCrdTotal_OWN.Text = "0.00"
        txtDDebit.Text = "0.00"
        txtCCredit.Text = "0.00"
        txtCCrdDebit_OWN.Text = "0.00"
        txtCCardCredit_OWN.Text = "0.00"
        txtCtotal.Text = "0.00"
        txtCCardTotal_OWN.Text = "0.00"
        txtCrCredit_OWN.Text = "0.00"
        txtCtotal.Text = "0.00"
        txtDNetAmt_OWN.Text = "0.00"
        txtTotNetAmt_OWN.Text = "0.00"
        txtNetBal_OWN.Text = "0.00"
        DatePicker1.Focus()
    End Function

    Function CmbLoad()
        strsql = "SELECT CASHID,CASHNAME FROM " & cnAdminDb & "..CASHCOUNTER"
        DT = New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(DT)
        If DT.Rows.Count > 0 Then
            cmbCntId_OWN.DataSource = Nothing
            cmbCntId_OWN.DataSource = DT
            cmbCntId_OWN.DisplayMember = "CASHNAME"
            cmbCntId_OWN.ValueMember = "CASHID"
        End If
    End Function

    Function GetCashDetails(ByVal cntid As String, ByVal gdate As DateTime) As DataTable
        Dim gdt As New DataTable
        strsql = "SELECT (SELECT SUM(AMOUNT) FROM " & cnStockDb & "..ACCTRAN WHERE TRANDATE='" & Format(gdate, "yyyy-MM-dd") & "' AND CASHID ='" & cntid & "' AND TRANMODE='D' AND PAYMODE='CH')AS CHEQUEDR"
        strsql += vbCrLf + ",(SELECT SUM(AMOUNT) FROM " & cnStockDb & "..ACCTRAN WHERE TRANDATE='" & Format(gdate, "yyyy-MM-dd") & "' AND CASHID ='" & cntid & "' AND TRANMODE='C' AND PAYMODE='CH')AS CHEQUECR"
        strsql += vbCrLf + ",(SELECT SUM(AMOUNT) FROM " & cnStockDb & "..ACCTRAN WHERE TRANDATE='" & Format(gdate, "yyyy-MM-dd") & "' AND CASHID ='" & cntid & "' AND TRANMODE='D' AND PAYMODE='CC')AS CARDDR"
        strsql += vbCrLf + ",(SELECT SUM(AMOUNT) FROM " & cnStockDb & "..ACCTRAN WHERE TRANDATE='" & Format(gdate, "yyyy-MM-dd") & "' AND CASHID ='" & cntid & "' AND TRANMODE='C' AND PAYMODE='CC')AS CARDCR"
        strsql += vbCrLf + ",(SELECT SUM(AMOUNT) FROM " & cnStockDb & "..ACCTRAN WHERE TRANDATE='" & Format(gdate, "yyyy-MM-dd") & "' AND CASHID ='" & cntid & "' AND TRANMODE='C' AND PAYMODE='CA')AS CASHCR"
        strsql += vbCrLf + ",(SELECT SUM(AMOUNT) FROM " & cnStockDb & "..ACCTRAN WHERE TRANDATE='" & Format(gdate, "yyyy-MM-dd") & "' AND CASHID ='" & cntid & "' AND TRANMODE='D' AND PAYMODE='CA')AS CASHDR"
        strsql += vbCrLf + ",(SELECT SUM(AMOUNT) FROM " & cnStockDb & "..ACCTRAN WHERE TRANDATE='" & Format(gdate, "yyyy-MM-dd") & "' AND CASHID ='" & cntid & "' AND TRANMODE='C' AND PAYMODE='GV')AS GVCR"
        strsql += vbCrLf + ",(SELECT SUM(AMOUNT) FROM " & cnStockDb & "..ACCTRAN WHERE TRANDATE='" & Format(gdate, "yyyy-MM-dd") & "' AND CASHID ='" & cntid & "' AND TRANMODE='D' AND PAYMODE='GV')AS GVDR"
        strsql += vbCrLf + ",(SELECT SUM(AMOUNT) FROM " & cnStockDb & "..ACCTRAN WHERE TRANDATE='" & Format(gdate, "yyyy-MM-dd") & "' AND CASHID ='" & cntid & "' AND TRANMODE='D' AND PAYMODE='DU')AS CREDIT "
        gdt = New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(gdt)
        If gdt.Rows.Count > 0 Then
            Return gdt
        End If
    End Function
    Function FillData()
        fdt = New DataTable
        fdt = GetCashDetails(cmbCntId_OWN.SelectedValue.ToString, DatePicker1.Value)
        If fdt.Rows.Count > 0 Then
            If Val(fdt.Rows(0).Item("CHEQUEDR").ToString) <> 0 Then
                txtDDebit.Text = fdt.Rows(0).Item("CHEQUEDR").ToString
            End If
            If Val(fdt.Rows(0).Item("CHEQUECR").ToString) <> 0 Then
                txtCCredit.Text = fdt.Rows(0).Item("CHEQUECR").ToString
            End If
            If Val(fdt.Rows(0).Item("CHEQUEDR").ToString) <> 0 Then
                txtCtotal.Text = (Val(fdt.Rows(0).Item("CHEQUEDR").ToString) - Val(fdt.Rows(0).Item("CHEQUECR").ToString)).ToString
            End If
            If Val(fdt.Rows(0).Item("CARDCR").ToString) <> 0 Then
                txtCCardCredit_OWN.Text = IIf(IsDBNull(fdt.Rows(0).Item("CARDCR").ToString), "0.00", fdt.Rows(0).Item("CARDCR").ToString)
            End If
            If Val(fdt.Rows(0).Item("CARDDR").ToString) <> 0 Then
                txtCCrdDebit_OWN.Text = IIf(IsDBNull(fdt.Rows(0).Item("CARDDR").ToString), "0.00", fdt.Rows(0).Item("CARDDR").ToString)
            End If
            txtCCardTotal_OWN.Text = (Val(fdt.Rows(0).Item("CARDDR").ToString) - Val(fdt.Rows(0).Item("CARDCR").ToString)).ToString
            If Val(fdt.Rows(0).Item("CASHDR").ToString) <> 0 Then
                TextBox1.Text = IIf(IsDBNull(fdt.Rows(0).Item("CASHDR").ToString), "0.00", fdt.Rows(0).Item("CASHDR").ToString)
            End If
            If Val(fdt.Rows(0).Item("CASHCR").ToString) <> 0 Then
                txtCrCredit_OWN.Text = IIf(IsDBNull(fdt.Rows(0).Item("CASHCR").ToString), "0.00", fdt.Rows(0).Item("CASHCR").ToString)
            End If

            If Val(fdt.Rows(0).Item("GVDR").ToString) <> 0 Then
                txtGvdebit.Text = IIf(IsDBNull(fdt.Rows(0).Item("GVDR").ToString), "0.00", fdt.Rows(0).Item("GVDR").ToString)
            End If
            If Val(fdt.Rows(0).Item("GVCR").ToString) <> 0 Then
                txtGvcredit.Text = IIf(IsDBNull(fdt.Rows(0).Item("GVCR").ToString), "0.00", fdt.Rows(0).Item("GVCR").ToString)
            End If

            txtCrdTotal_OWN.Text = Val(TextBox1.Text.ToString) - Val(txtCrCredit_OWN.Text.ToString)
            txtDNetAmt_OWN.Text = Val(txtCrdTotal_OWN.Text.ToString)
        End If
        txtDenthous.Focus()

    End Function

    Function ViewDenom(ByVal CASID As String, ByVal TRANDATE As DateTime)
        strsql = "SELECT * FROM " & cnStockDb & "..DENOMTRAN WHERE 1=1"
        strsql += vbCrLf + " AND CASHID ='" & CASID & "' AND TRANDATE ='" & Format(TRANDATE, "yyyy-MM-dd") & "'"
        DT = New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(DT)
        If DT.Rows.Count > 0 Then
            For i As Integer = 0 To DT.Rows.Count - 1
                If DT.Rows(i).Item("DEN_ID") = "1" Then
                    txtAmttho_OWN.Text = DT.Rows(i).Item("DEN_AMOUNT").ToString
                    txtDenthous.Text = DT.Rows(i).Item("DEN_QTY").ToString
                ElseIf DT.Rows(i).Item("DEN_ID").ToString = "2" Then
                    txtAmt5hrd_OWN.Text = DT.Rows(i).Item("DEN_AMOUNT").ToString
                    txtden5hrd.Text = DT.Rows(i).Item("DEN_QTY").ToString
                ElseIf DT.Rows(i).Item("DEN_ID").ToString = "3" Then
                    txtAmt1hrd_OWN.Text = DT.Rows(i).Item("DEN_AMOUNT").ToString
                    txtden1hrd.Text = DT.Rows(i).Item("DEN_QTY").ToString
                ElseIf DT.Rows(i).Item("DEN_ID").ToString = "4" Then
                    txtAmt50_OWN.Text = DT.Rows(i).Item("DEN_AMOUNT").ToString
                    txtden50.Text = DT.Rows(i).Item("DEN_QTY").ToString
                ElseIf DT.Rows(i).Item("DEN_ID").ToString = "5" Then
                    txtAmt20_OWN.Text = DT.Rows(i).Item("DEN_AMOUNT").ToString
                    txtden20_OWN.Text = DT.Rows(i).Item("DEN_QTY").ToString
                ElseIf DT.Rows(i).Item("DEN_ID").ToString = "6" Then
                    txtAmt10_OWN.Text = DT.Rows(i).Item("DEN_AMOUNT").ToString
                    txtden10_OWN.Text = DT.Rows(i).Item("DEN_QTY").ToString
                ElseIf DT.Rows(i).Item("DEN_ID").ToString = "7" Then
                    txtAmt5coin_OWN.Text = DT.Rows(i).Item("DEN_AMOUNT").ToString
                    txtden5_OWN.Text = DT.Rows(i).Item("DEN_QTY").ToString
                ElseIf DT.Rows(i).Item("DEN_ID").ToString = "8" Then
                    txtAmt2_OWN.Text = DT.Rows(i).Item("DEN_AMOUNT").ToString
                    txtden2_OWN.Text = DT.Rows(i).Item("DEN_QTY").ToString
                ElseIf DT.Rows(i).Item("DEN_ID").ToString = "9" Then
                    txtAmt1_OWN.Text = DT.Rows(i).Item("DEN_AMOUNT").ToString
                    txtden1_OWN.Text = DT.Rows(i).Item("DEN_QTY").ToString
                ElseIf DT.Rows(i).Item("DEN_ID").ToString = "10" Then
                    txtAmtothers_OWN.Text = DT.Rows(i).Item("DEN_AMOUNT").ToString
                    txtden1_OTHERS.Text = DT.Rows(i).Item("DEN_QTY").ToString
                End If
            Next
        End If
    End Function

    Function AmtCalculation()
        txtTotNetAmt_OWN.Text = (Val(txtAmttho_OWN.Text) + Val(txtAmt5hrd_OWN.Text) + Val(txtAmt1hrd_OWN.Text) _
        + Val(txtAmt50_OWN.Text) + Val(txtAmt20_OWN.Text) + Val(txtAmt10_OWN.Text) + Val(txtAmt5coin_OWN.Text) _
        + Val(txtAmt2_OWN.Text) + Val(txtAmt1_OWN.Text) + Val(txtAmtothers_OWN.Text))
        txtNetBal_OWN.Text = (Val(txtTotNetAmt_OWN.Text) - Val(txtDNetAmt_OWN.Text))
        If Val(txtTotNetAmt_OWN.Text) > 0 Then
            txtTotNetAmt_OWN.ForeColor = Color.Black
        Else
            txtTotNetAmt_OWN.ForeColor = Color.LightBlue
        End If

        If Val(txtNetBal_OWN.Text) > 0 Then
            txtNetBal_OWN.ForeColor = Color.LightBlue
        Else
            txtNetBal_OWN.ForeColor = Color.Red
        End If
    End Function

    Function Funsave()
        Dim DENID As String
        If Val(txtTotNetAmt_OWN.Text) <> 0 Then
            strsql = "DELETE FROM " & cnStockDb & "..DENOMTRAN WHERE CASHID='" & cmbCntId_OWN.SelectedValue.ToString & "'"
            strsql += vbCrLf + " AND TRANDATE='" & Format(DatePicker1.Value, "yyyy-MM-dd") & "'"
            Dim cmd As New OleDbCommand
            cmd = New OleDbCommand(strsql, cn)
            cmd.ExecuteNonQuery()
            If Val(txtDenthous.Text) <> 0 Then
                DENID = GetDenid("500")
                Insertdenom(DENID, DatePicker1.Value, cmbCntId_OWN.SelectedValue, Val(txtDenthous.Text.ToString), Val(txtAmttho_OWN.Text.ToString))
            End If
            If Val(txtden5hrd.Text) <> 0 Then
                DENID = GetDenid("200")
                Insertdenom(DENID, DatePicker1.Value, cmbCntId_OWN.SelectedValue, Val(txtden5hrd.Text.ToString), Val(txtAmt5hrd_OWN.Text.ToString))
            End If
            If Val(txtden1hrd.Text) <> 0 Then
                DENID = GetDenid("100")
                Insertdenom(DENID, DatePicker1.Value, cmbCntId_OWN.SelectedValue, Val(txtden1hrd.Text.ToString), Val(txtAmt1hrd_OWN.Text.ToString))
            End If
            If Val(txtden50.Text) <> 0 Then
                DENID = GetDenid("50")
                Insertdenom(DENID, DatePicker1.Value, cmbCntId_OWN.SelectedValue, Val(txtden50.Text.ToString), Val(txtAmt50_OWN.Text.ToString))
            End If
            If Val(txtden20_OWN.Text) <> 0 Then
                DENID = GetDenid("20")
                Insertdenom(DENID, DatePicker1.Value, cmbCntId_OWN.SelectedValue, Val(txtden20_OWN.Text.ToString), Val(txtAmt20_OWN.Text.ToString))
            End If
            If Val(txtden10_OWN.Text) <> 0 Then
                DENID = GetDenid("10")
                Insertdenom(DENID, DatePicker1.Value, cmbCntId_OWN.SelectedValue, Val(txtden10_OWN.Text.ToString), Val(txtAmt10_OWN.Text.ToString))
            End If
            If Val(txtden5_OWN.Text) <> 0 Then
                DENID = GetDenid("5")
                Insertdenom(DENID, DatePicker1.Value, cmbCntId_OWN.SelectedValue, Val(txtden5_OWN.Text.ToString), Val(txtAmt5coin_OWN.Text.ToString))
            End If
            If Val(txtden2_OWN.Text) <> 0 Then
                DENID = GetDenid("2")
                Insertdenom(DENID, DatePicker1.Value, cmbCntId_OWN.SelectedValue, Val(txtden2_OWN.Text.ToString), Val(txtAmt2_OWN.Text.ToString))
            End If
            If Val(txtden1_OWN.Text) <> 0 Then
                DENID = GetDenid("1")
                Insertdenom(DENID, DatePicker1.Value, cmbCntId_OWN.SelectedValue, Val(txtden1_OWN.Text.ToString), Val(txtAmt1_OWN.Text.ToString))
            End If
            If Val(txtden1_OTHERS.Text) <> 0 Then
                DENID = GetDenid("1")
                Insertdenom(DENID, DatePicker1.Value, cmbCntId_OWN.SelectedValue, Val(txtden1_OTHERS.Text.ToString), Val(txtAmtothers_OWN.Text.ToString))
            End If
            If MsgBox("Do You Want Print....!", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                PrintFunc()
            End If
            InitializeFunc()
        Else
            MsgBox("Denomination Should not be Empty...!")
            DatePicker1.Focus()
        End If
    End Function

    Function PrintFunc()
        Dim dtpr As New DataTable
        dtpr.Columns.Add("Denomination", Type.GetType("System.String"))
        dtpr.Columns.Add("No's", Type.GetType("System.String"))
        dtpr.Columns.Add("AMOUNT", Type.GetType("System.String"))
        Dim ro As DataRow
        ro = dtpr.NewRow
        ro("Denomination") = "500 X"
        ro("No's") = Val(txtDenthous.Text).ToString
        ro("AMOUNT") = Val(txtAmttho_OWN.Text).ToString
        dtpr.Rows.Add(ro)
        ro = dtpr.NewRow
        ro("Denomination") = "200 X"
        ro("No's") = Val(txtden5hrd.Text).ToString
        ro("AMOUNT") = Val(txtAmt5hrd_OWN.Text).ToString
        dtpr.Rows.Add(ro)
        ro = dtpr.NewRow
        ro("Denomination") = "100 X"
        ro("No's") = Val(txtden1hrd.Text).ToString
        ro("AMOUNT") = Val(txtAmt1hrd_OWN.Text).ToString
        dtpr.Rows.Add(ro)
        ro = dtpr.NewRow
        ro("Denomination") = "50 X"
        ro("No's") = Val(txtden50.Text).ToString
        ro("AMOUNT") = Val(txtAmt50_OWN.Text).ToString
        dtpr.Rows.Add(ro)
        ro = dtpr.NewRow
        ro("Denomination") = "20 X"
        ro("No's") = Val(txtden20_OWN.Text).ToString
        ro("AMOUNT") = Val(txtAmt20_OWN.Text).ToString
        dtpr.Rows.Add(ro)
        ro = dtpr.NewRow
        ro("Denomination") = "10 X"
        ro("No's") = Val(txtden10_OWN.Text).ToString
        ro("AMOUNT") = Val(txtAmt10_OWN.Text).ToString
        dtpr.Rows.Add(ro)
        ro = dtpr.NewRow
        ro("Denomination") = "5 X"
        ro("No's") = Val(txtden5_OWN.Text).ToString
        ro("AMOUNT") = Val(txtAmt5coin_OWN.Text).ToString
        dtpr.Rows.Add(ro)
        ro = dtpr.NewRow
        ro("Denomination") = "2 X"
        ro("No's") = Val(txtden2_OWN.Text).ToString
        ro("AMOUNT") = Val(txtAmt2_OWN.Text).ToString
        dtpr.Rows.Add(ro)
        ro = dtpr.NewRow
        ro("Denomination") = "1 X"
        ro("No's") = Val(txtden1_OWN.Text).ToString
        ro("AMOUNT") = Val(txtAmt1_OWN.Text).ToString
        dtpr.Rows.Add(ro)
        ro = dtpr.NewRow
        ro("Denomination") = "OTHERS X"
        ro("No's") = Val(txtden1_OTHERS.Text).ToString
        ro("AMOUNT") = Val(txtAmtothers_OWN.Text).ToString
        dtpr.Rows.Add(ro)
        ro = dtpr.NewRow
        dtpr.Rows.Add(ro)
        ro = dtpr.NewRow
        ro("Denomination") = "TOTAL"
        ro("No's") = "="
        ro("AMOUNT") = Val(txtTotNetAmt_OWN.Text).ToString
        dtpr.Rows.Add(ro)
        ro = dtpr.NewRow
        ro("Denomination") = "BALANCE"
        ro("No's") = "="
        ro("AMOUNT") = Val(txtNetBal_OWN.Text).ToString
        dtpr.Rows.Add(ro)
        Grddenom.DataSource = Nothing
        Grddenom.DataSource = dtpr
        Grddenom.AllowUserToAddRows = False
        Grddenom.Columns(0).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        Grddenom.Columns(1).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        Grddenom.Columns(2).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight

        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Function
        If Grddenom.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "CASH   " + txtDNetAmt_OWN.Text.ToString, Grddenom, BrightPosting.GExport.GExportType.Print)
        End If
    End Function
    Function GetDenid(ByVal denval As String) As String
        strsql = "SELECT DEN_ID FROM " & cnAdminDb & "..DENOMMAST WHERE DEN_VALUE='" & denval & "'"
        DT = New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(DT)
        If DT.Rows.Count > 0 Then
            Return DT.Rows(0).Item("DEN_ID").ToString
        Else
            Return Nothing
        End If
    End Function
    Function Insertdenom(ByVal DEN_ID As String, ByVal gdate As DateTime, ByVal cashid As String, ByVal denqty As Double, ByVal denamt As Double) As Boolean
        strsql = "INSERT INTO " & cnStockDb & "..DENOMTRAN (TRANDATE,CASHID,DEN_ID,DEN_QTY,DEN_AMOUNT)"
        strsql += vbCrLf + " VALUES ( '" & Format(gdate, "yyyy-MM-dd") & "' , '" & cashid & "' , " & DEN_ID & ""
        strsql += vbCrLf + "," & denqty & "," & denamt & ")"
        Dim cmd As New OleDbCommand
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()
    End Function
#End Region
    Private Sub SaveToolStripMenuItem1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem1.Click
        btnSave_Click(Me, New EventArgs)
    End Sub

    Private Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub ExitToolStripMenuItem3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem3.Click
        btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub btnView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnView.Click
        FillData()
        ViewDenom(cmbCntId_OWN.SelectedValue.ToString, DatePicker1.Value)
    End Sub

    Private Sub ViewToolStripMenuItem2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ViewToolStripMenuItem2.Click
        btnView_Click(Me, New EventArgs)
    End Sub

    Private Sub ToolStripMenuItem1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem1.Click
        Button1_Click(Me, New EventArgs)
    End Sub
 
    Private Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        InitializeFunc()
    End Sub

  
    Private Sub cmbCntId_OWN_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCntId_OWN.SelectedIndexChanged

    End Sub
End Class