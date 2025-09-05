Imports System.Data.OleDb
Public Class AccEntryWeightDetail
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Public dtWeightDetail As DataTable
    Dim tranMode As String

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
        grpWUnit.BackColor = grpWeightDetail.BackgroundGradientColor
        grpWUnit.Visible = False
        strSql = " SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY ORDER BY CATNAME"
        objGPack.FillCombo(strSql, cmbWCategory_MAN, , False)

        cmbWCalcMode.Items.Add("GROSS")
        cmbWCalcMode.Items.Add("NETWT")
        cmbWCalcMode.Items.Add("PUREWT")
        cmbWCalcMode.Text = "GROSS"

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
    End Sub

    Private Sub AccEntryWeightDetail_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtWGrsWt_WET_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtWGrsWt_WET.Leave
        txtWNetWt_WET.Text = IIf(Val(txtWGrsWt_WET.Text) <> 0, Format(Val(txtWGrsWt_WET.Text), "0.000"), "")
    End Sub

    Private Sub txtWNetWt_WET_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtWNetWt_WET.Leave
        If Val(txtWNetWt_WET.Text) > Val(txtWGrsWt_WET.Text) Then
            txtWNetWt_WET.Text = IIf(Val(txtWGrsWt_WET.Text) <> 0, Format(Val(txtWGrsWt_WET.Text), "0.000"), "")
            txtWNetWt_WET.Focus()
            MsgBox("Net Weight should not exeed with Gross Weight", MsgBoxStyle.Information)
        End If
    End Sub

    Private Sub btnWCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub btnWOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnWOk.Click
        If objGPack.Validator_Check(Me) Then Exit Sub
        If Not (Val(txtWPcs_NUM.Text) <> 0 Or Val(txtWGrsWt_WET.Text)) <> 0 Then
            txtWPcs_NUM.Focus()
            MsgBox("Pcs and Weight should not empty", MsgBoxStyle.Information)
            Exit Sub
        End If
        Dim ro As DataRow = dtWeightDetail.NewRow
        ro("CATNAME") = cmbWCategory_MAN.Text
        ro("PCS") = Val(txtWPcs_NUM.Text)
        ro("GRSWT") = Val(txtWGrsWt_WET.Text)
        ro("NETWT") = Val(txtWNetWt_WET.Text)
        ro("PUREWT") = Val(txtPureWt_WET.Text)
        ro("CALCMODE") = cmbWCalcMode.Text
        If grpWUnit.Visible Then ro("UNIT") = IIf(rbtWGram.Checked, "G", "C") Else ro("UNIT") = "G"
        ro("RATE") = Val(txtWRate_AMT.Text)
        ro("TYPE") = cmbWType.Text
        ro("AMOUNT") = Val(txtWamt_AMT.Text)
        ro("VAT") = Val(txtVat_AMT.Text)
        ro("REFNO") = Val(txtRefno.Text)
        ro("REFDATE") = dtpRefDate.Value
        dtWeightDetail.Rows.Add(ro)
        dtWeightDetail.AcceptChanges()
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub cmbWCalcMode_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbWCalcMode.GotFocus
        If Val(txtWGrsWt_WET.Text) <> Val(txtWNetWt_WET.Text) Then
            cmbWCalcMode.Text = "GROSS"
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub cmbWCategory_MAN_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbWCategory_MAN.SelectedIndexChanged
        strSql = " SELECT 'CHECK' FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & cmbWCategory_MAN.Text & "'"
        strSql += " AND METALID IN ('D','T')"
        If objGPack.GetSqlValue(strSql, , "-1") = "-1" Then
            grpWUnit.Visible = False
        Else
            grpWUnit.Visible = True
        End If
        strSql = " SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID IN (SELECT PURITYID FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & cmbWCategory_MAN.Text & "')"
        txtPure_Per.Text = objGPack.GetSqlValue(strSql, , 100)
    End Sub

    Private Sub TextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtVat_per.TextChanged
        txtVat_AMT.Text = Math.Round(Val(txtWamt_AMT.Text) * (Val(txtVat_per.Text) / 100), 2)
    End Sub

    Private Sub txtPure_Per_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPure_Per.GotFocus
        If Val(txtPure_Per.Text) <> 0 And Val(txtWNetWt_WET.Text) <> 0 Then txtPureWt_WET.Text = Val(txtWNetWt_WET.Text) * (Val(txtPure_Per.Text) / 100)
    End Sub

    Private Sub txtWNetWt_WET_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtWNetWt_WET.TextChanged
        If Val(txtPure_Per.Text) <> 0 And Val(txtWNetWt_WET.Text) <> 0 Then txtPureWt_WET.Text = Val(txtWNetWt_WET.Text) * (Val(txtPure_Per.Text) / 100)
    End Sub

    Private Sub txtWRate_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtWRate_AMT.TextChanged
        If Val(txtWRate_AMT.Text) > 0 Then
            If cmbWCalcMode.Text = "GROSS" Then txtWamt_AMT.Text = Val(txtWGrsWt_WET.Text) * Val(txtWRate_AMT.Text)
            If cmbWCalcMode.Text = "NETWT" Then txtWamt_AMT.Text = Val(txtWNetWt_WET.Text) * Val(txtWRate_AMT.Text)
            If cmbWCalcMode.Text = "PUREWT" Then txtWamt_AMT.Text = Val(txtPureWt_WET.Text) * Val(txtWRate_AMT.Text)
        End If
    End Sub
End Class