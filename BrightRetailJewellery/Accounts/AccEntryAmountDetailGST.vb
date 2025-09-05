Imports System.Data.OleDb
Public Class AccEntryAmountDetailGST
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Public dtWeightDetail As DataTable
    Dim tranMode As String
    Public InterStateBill As Boolean
    Public Enum Type
        SALES = 0
        SMITH_ISSUE = 1
        PURCHASE_RETURN = 2
        PURCHASE = 3
        SMITH_RECEIPT = 4
        SMITH_PURCHASE = 5
    End Enum
    Private Sub Initialize()
        objGPack.Validator_Object(Me)
        objGPack.TextClear(Me)
        strSql = " SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY ORDER BY CATNAME"
        objGPack.FillCombo(strSql, cmbWCategory_MAN, , False)
        dtWeightDetail = New DataTable
        dtWeightDetail.Columns.Add("CATNAME", GetType(String))
        dtWeightDetail.Columns.Add("PCS", GetType(Integer))
        dtWeightDetail.Columns.Add("GRSWT", GetType(Decimal))
        dtWeightDetail.Columns.Add("NETWT", GetType(Decimal))
        dtWeightDetail.Columns.Add("PUREWT", GetType(Decimal))
        dtWeightDetail.Columns.Add("CALCMODE", GetType(String))
        dtWeightDetail.Columns.Add("UNIT", GetType(String))
        dtWeightDetail.Columns.Add("RATE", GetType(Decimal))
        dtWeightDetail.Columns.Add("AMOUNT", GetType(Decimal))
        dtWeightDetail.Columns.Add("TYPE", GetType(String))
        dtWeightDetail.Columns.Add("REFNO", GetType(String))
        dtWeightDetail.Columns.Add("REFDATE", GetType(String))
        dtWeightDetail.Columns.Add("VAT", GetType(Decimal))
        dtWeightDetail.Columns.Add("SGST", GetType(Decimal))
        dtWeightDetail.Columns.Add("CGST", GetType(Decimal))
        dtWeightDetail.Columns.Add("IGST", GetType(Decimal))
    End Sub
    Public Sub New(ByVal tranMode As String)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Initialize()
        cmbWType.Items.Clear()
        If tranMode.ToUpper = "CR" Then
            cmbWType.Items.Add(Type.SALES.ToString) 'SA
            cmbWType.Items.Add(Type.SMITH_ISSUE.ToString) 'ISS
            cmbWType.Items.Add(Type.PURCHASE_RETURN.ToString) 'IPU
            cmbWType.Text = Type.SALES.ToString
        Else
            cmbWType.Items.Add(Type.PURCHASE.ToString) 'PU
            cmbWType.Items.Add(Type.SMITH_RECEIPT.ToString) 'RRE
            cmbWType.Items.Add(Type.SMITH_PURCHASE.ToString) 'RPU
            cmbWType.Text = Type.PURCHASE.ToString
        End If
        Me.tranMode = tranMode
        PnlSg.BackColor = Color.Lavender
        PnlIg.BackColor = Color.Lavender
    End Sub
    Private Sub AccEntryWeightDetail_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub btnWCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
    Private Sub btnWOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnWOk.Click
        If objGPack.Validator_Check(Me) Then Exit Sub
        If Not (Val(txtWamt_AMT.Text) <> 0) Then
            txtWamt_AMT.Focus()
            MsgBox("Amount should not empty", MsgBoxStyle.Information)
            Exit Sub
        End If
        Dim ro As DataRow = dtWeightDetail.NewRow
        ro("CATNAME") = cmbWCategory_MAN.Text
        ro("PCS") = 0
        ro("GRSWT") = 0
        ro("NETWT") = 0
        ro("PUREWT") = 0
        ro("CALCMODE") = ""
        ro("UNIT") = ""
        ro("RATE") = 0
        ro("TYPE") = cmbWType.Text
        ro("AMOUNT") = Val(txtWamt_AMT.Text)
        ro("VAT") = Val(txtVat_AMT.Text)
        ro("SGST") = Val(txtSgst_AMT.Text)
        ro("CGST") = Val(txtCgst_AMT.Text)
        ro("IGST") = Val(txtIgst_AMT.Text)
        ro("REFNO") = Val(txtRefno.Text)
        ro("REFDATE") = dtpRefDate.Value
        dtWeightDetail.Rows.Add(ro)
        dtWeightDetail.AcceptChanges()
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub
    Private Sub funcCalcGst()
        txtSgst_AMT.Text = Format(Math.Round(Val(txtWamt_AMT.Text) * (Val(txtSgstPer_PER.Text) / 100), 2), "0.00")
        txtCgst_AMT.Text = Format(Math.Round(Val(txtWamt_AMT.Text) * (Val(txtCgstPer_PER.Text) / 100), 2), "0.00")
        txtIgst_AMT.Text = Format(Math.Round(Val(txtWamt_AMT.Text) * (Val(txtIgstPer_PER.Text) / 100), 2), "0.00")
        If Val(txtSgstPer_PER.Text) = 0 Then txtSgst_AMT.Text = DBNull.Value.ToString
        If Val(txtCgstPer_PER.Text) = 0 Then txtCgst_AMT.Text = DBNull.Value.ToString
        If Val(txtIgstPer_PER.Text) = 0 Then txtIgst_AMT.Text = DBNull.Value.ToString
        Dim Gst As Double = Val(txtSgst_AMT.Text) + Val(txtCgst_AMT.Text) + Val(txtIgst_AMT.Text)
        txtVat_AMT.Text = IIf(Gst <> 0, Format(Gst, "0.00"), DBNull.Value.ToString)
    End Sub
    Private Sub txtCgstPer_PER_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCgstPer_PER.TextChanged
        funcCalcGst()
    End Sub
    Private Sub txtIgstPer_PER_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtIgstPer_PER.TextChanged
        funcCalcGst()
    End Sub
    Private Sub txtSgstPer_PER_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSgstPer_PER.TextChanged
        funcCalcGst()
    End Sub
    Private Sub txtWamt_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtWamt_AMT.TextChanged
        funcCalcGst()
    End Sub
    Private Sub cmbWCategory_MAN_SelectedIndexChanged_1(sender As Object, e As EventArgs) Handles cmbWCategory_MAN.SelectedIndexChanged
        strSql = " SELECT 'CHECK' FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & cmbWCategory_MAN.Text & "'"
        strSql += " AND METALID IN ('D','T')"
        strSql = " SELECT * FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & cmbWCategory_MAN.Text & "'"
        Dim dr As DataRow = GetSqlRow(strSql, cn)
        If Not dr Is Nothing Then
            If tranMode.ToUpper = "CR" Then
                If InterStateBill = False Then
                    txtSgstPer_PER.Text = IIf(Val(dr("S_SGSTTAX").ToString) > 0, Format(Val(dr("S_SGSTTAX").ToString), "0.00"), DBNull.Value.ToString)
                    txtCgstPer_PER.Text = IIf(Val(dr("S_CGSTTAX").ToString) > 0, Format(Val(dr("S_CGSTTAX").ToString), "0.00"), DBNull.Value.ToString)
                    txtIgstPer_PER.Text = DBNull.Value.ToString
                    PnlSg.Enabled = True
                    PnlIg.Enabled = False
                Else
                    txtSgstPer_PER.Text = DBNull.Value.ToString
                    txtCgstPer_PER.Text = DBNull.Value.ToString
                    txtIgstPer_PER.Text = IIf(Val(dr("S_IGSTTAX").ToString) > 0, Format(Val(dr("S_IGSTTAX").ToString), "0.00"), DBNull.Value.ToString)
                    PnlSg.Enabled = False
                    PnlIg.Enabled = True
                End If
            Else
                If InterStateBill = False Then
                    txtSgstPer_PER.Text = IIf(Val(dr("P_SGSTTAX").ToString) > 0, Format(Val(dr("P_SGSTTAX").ToString), "0.00"), DBNull.Value.ToString)
                    txtCgstPer_PER.Text = IIf(Val(dr("P_CGSTTAX").ToString) > 0, Format(Val(dr("P_CGSTTAX").ToString), "0.00"), DBNull.Value.ToString)
                    txtIgstPer_PER.Text = DBNull.Value.ToString
                    PnlSg.Enabled = True
                    PnlIg.Enabled = False
                Else
                    txtSgstPer_PER.Text = DBNull.Value.ToString
                    txtCgstPer_PER.Text = DBNull.Value.ToString
                    txtIgstPer_PER.Text = IIf(Val(dr("P_IGSTTAX").ToString) > 0, Format(Val(dr("P_IGSTTAX").ToString), "0.00"), DBNull.Value.ToString)
                    PnlSg.Enabled = False
                    PnlIg.Enabled = True
                End If
            End If
            Dim GstPer As Decimal = Val(txtSgstPer_PER.Text) + Val(txtCgstPer_PER.Text) + Val(txtIgstPer_PER.Text)
            txtVat_per.Text = IIf(GstPer <> 0, Format(GstPer, "0.00"), DBNull.Value.ToString)
        End If
    End Sub
End Class