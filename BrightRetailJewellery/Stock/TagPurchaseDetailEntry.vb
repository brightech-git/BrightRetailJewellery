Imports System.Data.OleDb
Public Class TagPurchaseDetailEntry
    Dim StrSql As String
    Public dtGridStone As New DataTable
    Public dtGridMisc As New DataTable
    Public dtGridMultiMetal As New DataTable
    Dim WastageRound As Integer = Val(GetAdmindbSoftValue("ROUNDOFF-WASTAGE", "3"))
    Dim McRnd As Integer = Val(GetAdmindbSoftValue("ROUNDOFF-MC", "2"))
    Dim StudWtDedut As String = GetAdmindbSoftValue("STUDWTDEDUCT", "DSP")
    Public CalcMode As String = ""
    Public salePcs As Integer
    Public Loadd As Boolean = False
    Public saleValue As Decimal
    Dim purratecal As Boolean = False
    Public saveflag As Boolean = False

    Private EditRow As Integer = -1
    Private EditCol As Integer = -1
    Private celWasEndEdit As DataGridViewCell
    Dim PUR_LANDCOST As Boolean = IIf(GetAdmindbSoftValue("PUR_LANDCOST", "N").ToUpper = "Y", True, False)
    Dim PUR_RATECALC As Boolean = IIf(GetAdmindbSoftValue("PUR_MRATECALC", "N").ToUpper = "Y", True, False)
    Dim PUR_LANDCOSTPER As Decimal = Val(GetAdmindbSoftValue("PUR_LANDCOSTPER", "0").ToString)
    Dim PUR_TAB_EDIT As Boolean = IIf(GetAdmindbSoftValue("PUR_TAB_EDIT", "Y").ToUpper = "Y", True, False)
    Public purGrsNet As String = ""


    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Initializer()

    End Sub

    Public Sub New(ByVal str As Boolean)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Initializer()

        saveflag = str
        If saveflag = True Then
            btnSave.Visible = True
        Else
            btnSave.Visible = False
        End If
    End Sub

    Private Sub Initializer()
        Me.BackColor = frmBackColor
        objGPack.Validator_Object(Me)
        cmbPurCalMode.Items.Clear()
        cmbPurCalMode.Items.Add("GRS WT")
        cmbPurCalMode.Items.Add("NET WT")
        Initializer_Stone()
        Initializer_Miscellaneous()
        Initializer_MultiMetal()
        'If PUR_LANDCOST Then
        '    PnlLandCost.Visible = True
        'Else
        '    txtSaleRate_NUM.Location = New Drawing.Point(txtSaleRate_NUM.Location.X, txtSaleRate_NUM.Location.Y - PnlLandCost.Height)
        '    lblSalRatePer.Location = New Drawing.Point(lblSalRatePer.Location.X, lblSalRatePer.Location.Y - PnlLandCost.Height)
        '    PnlLandCost.Visible = False
        'End If
    End Sub

    Private Sub TagPurchaseDetailEntry_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If objGPack.IsActive(pnlStoneGrid) Then
                If gridMisc.RowCount > 0 Then
                    gridMisc.Select()
                Else
                    txtPurLessWt_Wet.Select()
                End If
                Exit Sub
            End If
            If gridMisc.Focused Then
                txtPurLessWt_Wet.Select()
                Exit Sub
            End If
            If gridStone.Focused Then
                If gridMisc.RowCount > 0 Then
                    gridMisc.Select()
                Else
                    txtPurLessWt_Wet.Select()
                End If
                Exit Sub
            End If
            Me.Close()
        ElseIf e.KeyCode = Keys.F1 Then
            Call btnSave_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub TagPurchaseDetail_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtPurGrossWt_Wet.Focused Then Exit Sub
            'If txtPurPurchaseVal_Amt.Focused And Not txtSaleRate_PER.Enabled Then Exit Sub
            'If txtSaleRate_PER.Focused Then Exit Sub
            If txtStPuAmount.Focused Then Exit Sub
            If gridStone.Focused Then Exit Sub
            If gridMisc.Focused Then Exit Sub
            'If gridMultimetal.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    'private change to public
    Public Sub TagPurchaseDetail_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        dtpPurchaseDate.MinimumDate = (New DateTimePicker).MinDate
        dtpPurchaseDate.MaximumDate = (New DateTimePicker).MaxDate
        lblStatus.Visible = False
        Load_Stone()
        Load_Misc()
        Load_MultiMetal()
        If Not Loadd Then
            txtPurGrossWt_Wet_TextChanged(Me, New EventArgs)
            txtPurTouch_TextChanged(Me, New EventArgs)
            'txtPurWastagePer_TextChanged(Me, New EventArgs)
            txtPurWastage_Wet_TextChanged(Me, New EventArgs)
            'txtPurMcPerGrm_TextChanged(Me, New EventArgs)
            txtPurMakingChrg_Amt_TextChanged(Me, New EventArgs)
            CalcPurchaseGrossValue()
            CalcPurchaseValue()
            Loadd = True
        End If
        If PUR_TAB_EDIT = False Then PnlTop.Enabled = False
        If PUR_LANDCOSTPER <> 0 Then txtSaleRate_PER.Text = Format(PUR_LANDCOSTPER, "0.00")
        dtpPurchaseDate.Select()
        'txtPurGrossWt_Wet.Select()
    End Sub

    Private Sub txtPurPurchaseVal_Amt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPurPurchaseVal_Amt.KeyPress
        'e.Handled = True
        'If e.KeyChar = Chr(Keys.Enter) Then
        '    If txtSaleRate_PER.Enabled Then txtSaleRate_PER.Focus() : Exit Sub
        '    If saveflag = False Then
        '        Me.Close()
        '    Else
        '        btnSave.Focus()
        '    End If
        'End If
    End Sub

    Private Sub txtPurNetWt_Wet_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPurNetWt_Wet.GotFocus
        SendKeys.Send("{TAB}")
    End Sub

    Private Sub txtPurGrossWt_Wet_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPurGrossWt_Wet.KeyPress
        e.Handled = True
        If e.KeyChar = Chr(Keys.Enter) Then
            ShowSubDetailDia()
        End If
    End Sub

    Private Sub txtPurLessWt_Wet_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPurLessWt_Wet.LostFocus
        If Val(txtPurLessWt_Wet.Text) >= Val(txtPurGrossWt_Wet.Text) Then
            txtPurLessWt_Wet.Clear()
        End If
        If Val(txtPurLessWt_Wet.Text) = 0 Then
            If purGrsNet = "" Then cmbPurCalMode.Text = "GRS WT"
        End If
    End Sub

    Private Sub txtPurLessWt_Wet_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPurLessWt_Wet.TextChanged
        Dim netWT As Double = Val(txtPurGrossWt_Wet.Text) - Val(txtPurLessWt_Wet.Text)
        txtPurNetWt_Wet.Text = IIf(netWT <> 0, Format(netWT, "0.000"), Nothing)
    End Sub
    Private Sub txtPurMakingChrg_Amt_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPurMakingChrg_Amt.GotFocus
        If Val(txtPurMcPerGrm_Amt.Text) > 0 Then SendKeys.Send("{TAB}")
    End Sub
    Private Sub txtPurMakingChrg_Amt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPurMakingChrg_Amt.TextChanged
        CalcPurchaseGrossValue()
    End Sub
    Public Sub txtPurMcPerGrm_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPurMcPerGrm_Amt.TextChanged
        If Not Val(txtPurMcPerGrm_Amt.Text) = 0 Then
            Dim amt As Double = Nothing
            If cmbPurCalMode.SelectedIndex = 0 Then ''GRS WT
                amt = Val(txtPurGrossWt_Wet.Text) * Val(txtPurMcPerGrm_Amt.Text)
            Else ''NET WT
                amt = Val(txtPurNetWt_Wet.Text) * Val(txtPurMcPerGrm_Amt.Text)
            End If
            txtPurMakingChrg_Amt.Text = IIf(amt <> 0, Format(amt, "0.00"), "")
        End If
    End Sub
    Private Sub txtPurPurchaseVal_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPurPurchaseVal_Amt.GotFocus
        CalcPurchaseValue()
    End Sub

    Private Sub txtPurRate_Amt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPurRate_Amt.TextChanged
        CalcPurchaseGrossValue()
    End Sub

    Private Sub cmbPurCalMode_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbPurCalMode.GotFocus
        If Val(txtPurGrossWt_Wet.Text) = Val(txtPurNetWt_Wet.Text) Then
            If purGrsNet = "" Then cmbPurCalMode.Text = "GRS WT"
        End If
        CalcPurchaseGrossValue()
        If Val(txtPurGrossWt_Wet.Text) = Val(txtPurNetWt_Wet.Text) Then
            SendKeys.Send("{TAB}")
        End If

    End Sub

    Private Sub cmbPurCalMode_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbPurCalMode.SelectedIndexChanged
        CalcPurchaseGrossValue()
    End Sub

    Private Sub txtPurTouch_Amt_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPurTouch_Amt.GotFocus
        If Val(txtPurWastage_Per.Text) > 0 Or Val(txtPurWastage_Wet.Text) > 0 Then
            'SendKeys.Send("{TAB}")
        End If
    End Sub
    Public Sub txtPurTouch_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPurTouch_Amt.TextChanged
        CalcPurchaseGrossValue()
    End Sub

    Private Sub txtPurWastage_Per_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPurWastage_Per.GotFocus
        If Val(txtPurTouch_Amt.Text) > 0 Then
            'SendKeys.Send("{TAB}")
        End If
    End Sub
    Public Sub txtPurWastagePer_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPurWastage_Per.TextChanged
        Dim wt As Double = Nothing
        If Val(txtPurWastage_Per.Text) = 0 Then Exit Sub
        If cmbPurCalMode.SelectedIndex = 0 Then ''GRS WT
            wt = Val(txtPurGrossWt_Wet.Text) * (Val(txtPurWastage_Per.Text) / 100)
        Else ''NET WT
            wt = Val(txtPurNetWt_Wet.Text) * (Val(txtPurWastage_Per.Text) / 100)
        End If
        wt = Math.Round(wt, WastageRound)
        txtPurWastage_Wet.Text = IIf(wt <> 0, Format(wt, "0.000"), "")
    End Sub
    Private Sub txtPurWastage_Wet_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPurWastage_Wet.GotFocus
        If Val(txtPurTouch_Amt.Text) > 0 Then
            'SendKeys.Send("{TAB}")
            Exit Sub
        End If
        If Val(txtPurWastage_Per.Text) > 0 Then
            'SendKeys.Send("{TAB}")
            Exit Sub
        End If
    End Sub

    Private Sub txtPurWastage_Wet_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPurWastage_Wet.TextChanged
        CalcPurchaseGrossValue()
    End Sub

    Public Sub CalcPurchaseGrossValue()
        Dim pval As Decimal 
        pval = GetPurGrossValue()
        If CurrencyDecimal = 3 Then
            txtPurGrossValue_AMT.Text = IIf(pval <> 0, Format(pval, "0.000"), "")
            txtPurLCost_NUM.Text = IIf(pval <> 0, Format(pval, "0.000"), "")
        Else
            txtPurGrossValue_AMT.Text = IIf(pval <> 0, Format(pval, "0.00"), "")
            txtPurLCost_NUM.Text = IIf(pval <> 0, Format(pval, "0.00"), "")
        End If
    End Sub
    Public Sub CalcPurchaseValue()
        Dim pval As Decimal
        pval = Val(txtPurGrossValue_AMT.Text)
        pval += Val(txtPurTax_AMT.Text)
        pval = Math.Round(pval)
        If CurrencyDecimal = 3 Then
            txtPurPurchaseVal_Amt.Text = IIf(pval <> 0, Format(pval, "0.000"), "")
        Else
            txtPurPurchaseVal_Amt.Text = IIf(pval <> 0, Format(pval, "0.00"), "")
        End If
        If PUR_LANDCOST Then
            If Val(txtSaleRate_PER.Text.ToString) <> 0 Then
                saleValue = Format(Val(txtPurGrossValue_AMT.Text.ToString) * Val(txtSaleRate_PER.Text.ToString), "0.00")
            End If
        End If
    End Sub
    Private Function GetPurGrossValue() As Decimal
        Dim pval As Decimal = 0
        Dim purMultiAmt As Double = 0
        Dim PurStuddedAmt As Double = 0
        Dim purMisc As Double = 0
        Dim wt As Double = 0

        Dim MultiMetalWt As Decimal = 0


        If cmbPurCalMode.SelectedIndex = 0 Then ''GRS WT
            wt = Val(txtPurGrossWt_Wet.Text)
        Else ''NET WT
            wt = Val(txtPurNetWt_Wet.Text)
        End If

        For cnt As Integer = 0 To gridMultimetal.RowCount - 1
            purMultiAmt += Val(gridMultimetal.Rows(cnt).Cells("PURAMOUNT").Value.ToString)
            MultiMetalWt += Val(gridMultimetal.Rows(cnt).Cells("WEIGHT").Value.ToString)
        Next

        ' ''Find PurStuddedAmt
        For cnt As Integer = 0 To gridStone.RowCount - 1
            PurStuddedAmt += Val(gridStone.Rows(cnt).Cells("PURVALUE").Value.ToString)
        Next
        For cnt As Integer = 0 To gridMisc.Rows.Count - 1
            purMisc += Val(gridMisc.Rows(cnt).Cells("PURAMOUNT").Value.ToString)
        Next
        wt = wt - MultiMetalWt
        If Val(txtPurTouch_Amt.Text) > 0 Then
            pval = (wt * (Val(txtPurTouch_Amt.Text) / 100)) * Val(txtPurRate_Amt.Text)
            pval += PurStuddedAmt + purMisc + purMultiAmt + Val(txtPurMakingChrg_Amt.Text)
        Else
            If CalcMode = "R" Then
                pval = Val(salePcs) * Val(txtPurRate_Amt.Text)
            Else
                pval = ((wt + Val(txtPurWastage_Wet.Text)) * Val(txtPurRate_Amt.Text)) + Val(txtPurMakingChrg_Amt.Text) + PurStuddedAmt + purMisc + purMultiAmt
            End If
        End If
        Return pval
    End Function
    Private Sub ShowSubDetailDia()
        'ShowMultiMetalDia()
        If gridStone.RowCount > 0 Then
            gridStone.Select()
            gridStone_SelectionChanged(Me, New EventArgs)
        ElseIf gridMisc.RowCount > 0 Then
            gridMisc.Select()
            gridMisc_SelectionChanged(Me, New EventArgs)
        ElseIf gridMultimetal.RowCount > 0 Then
            gridMultimetal.Select()
            gridMultimetal_SelectionChanged(Me, New EventArgs)
        Else
            txtPurLessWt_Wet.Select()
        End If
        CalcPurchaseGrossValue()
        'Me.SelectNextControl(txtPurGrossWt_Wet, True, True, True, True)
    End Sub



#Region "Stone Details"
    Private Sub Initializer_Stone()
        cmbStUnit.Items.Add("C")
        cmbStUnit.Items.Add("G")
        cmbStUnit.Text = "C"
        cmbStCalc.Items.Add("W")
        cmbStCalc.Items.Add("P")
        cmbStCalc.Text = "W"
        ''Stone
        With dtGridStone.Columns
            .Add("ITEM", GetType(String))
            .Add("SUBITEM", GetType(String))
            .Add("PCS", GetType(Int32))
            .Add("WEIGHT", GetType(Decimal))
            .Add("UNIT", GetType(String))
            .Add("CALC", GetType(String))
            .Add("RATE", GetType(Decimal))
            .Add("AMOUNT", GetType(Decimal))
            .Add("METALID", GetType(String))
            .Add("PURRATE", GetType(Decimal))
            .Add("PURVALUE", GetType(Decimal))
            .Add("KEYNO", GetType(Integer))
            .Add("STNSNO", GetType(String))
        End With
        With gridStone
            .DataSource = dtGridStone
            For i As Integer = 0 To .ColumnCount - 1
                If .Columns(i).ValueType.Name Is GetType(Decimal).Name Then
                    .Columns(i).DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
                    .Columns(i).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                ElseIf .Columns(i).ValueType.Name Is GetType(Double).Name Then
                    .Columns(i).DefaultCellStyle.Format = "0.00"
                    .Columns(i).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                ElseIf .Columns(i).ValueType.Name Is GetType(Integer).Name Then
                    .Columns(i).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End If
                .Columns(i).Resizable = DataGridViewTriState.False
            Next
            .Columns("WEIGHT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
        End With
        StyleGridStone(gridStone)

        Dim dtGridStoneTotal As New DataTable
        dtGridStoneTotal = dtGridStone.Copy
        dtGridStoneTotal.Rows.Clear()
        dtGridStoneTotal.Rows.Add()
        dtGridStoneTotal.Rows(0).Item("ITEM") = "Total"
        dtGridStoneTotal.AcceptChanges()
        With gridStoneFooter
            .DataSource = dtGridStoneTotal
            For Each col As DataGridViewColumn In gridStone.Columns
                With gridStoneFooter.Columns(col.Name)
                    .Visible = col.Visible
                    .Width = col.Width
                    .DefaultCellStyle = col.DefaultCellStyle
                End With
            Next
        End With
        CalcStoneWtAmount()
        StyleGridStone(gridStoneFooter)
    End Sub


    Private Sub txtStItem_MAN_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtStItem.GotFocus
        Main.ShowHelpText("Press Insert Key to Help")
    End Sub

    Private Sub txtStItem_MAN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtStItem.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtStItem.Text = "" Then
                LoadStoneItemName()
            ElseIf txtStItem.Text <> "" And objGPack.DupCheck("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "'") = False Then
                LoadStoneItemName()
            Else
                LoadStoneitemDetails()
            End If
        End If
    End Sub

    Private Sub txtStItem_Man_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtStItem.LostFocus
        Main.HideHelpText()
    End Sub

    Private Sub txtStSubItem_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtStSubItem.GotFocus
        'If objGPack.DupCheck("SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem_MAN.Text & "')") = False Then
        SendKeys.Send("{TAB}")
        'End If
    End Sub

    Private Sub txtStItem_MAN_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtStItem.KeyDown
        If e.KeyCode = Keys.Insert Then
            LoadStoneItemName()
        ElseIf e.KeyCode = Keys.Down Then
            If gridStone.RowCount > 0 Then
                gridStone.Focus()
            End If
        End If
    End Sub

    Private Sub LoadStoneItemName()
        StrSql = " SELECT"
        StrSql += " ITEMID AS ITEMID,ITEMNAME AS ITEMNAME"
        StrSql += " FROM " & cnAdminDb & "..ITEMMAST"
        StrSql += " WHERE ISNULL(STUDDED,'') = 'S' AND ACTIVE = 'Y'"
        Dim itemName As String = BrighttechPack.SearchDialog.Show("Find ItemName", StrSql, cn, 1, 1, , txtStItem.Text)
        If itemName <> "" Then
            txtStItem.Text = itemName
            LoadStoneitemDetails()
        Else
            txtStItem.Focus()
            txtStItem.SelectAll()
        End If
    End Sub

    Private Sub LoadStoneitemDetails()
        'txtStSubItem.Clear()
        'txtStPcs_Num.Clear()
        'txtStWeight.Clear()
        'txtStRate_Amt.Clear()
        'txtStAmount_Amt.Clear()
        If objGPack.GetSqlValue("SELECT SUBITEM FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "'") = "Y" Then
            Dim qry As String = "SELECT SUBITEMID ID,SUBITEMNAME SUBITEM FROM " & cnAdminDb & "..SUBITEMMAST "
            qry += " WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "')"
            qry += " AND ACTIVE = 'Y'"
            txtStSubItem.Text = BrighttechPack.SearchDialog.Show("Search SubItem", qry, cn, 1, 1, , txtStSubItem.Text, , False, True)
        Else
            txtStSubItem.Clear()
        End If

        If txtStSubItem.Text <> "" Then
            StrSql = " SELECT CALTYPE FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & txtStSubItem.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "')"
        Else
            StrSql = " SELECT CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "'"
        End If
        Dim calType As String = objGPack.GetSqlValue(StrSql, , , tran)
        cmbStCalc.Text = IIf(calType = "R", "P", "W")

        If txtStSubItem.Text <> "" Then
            StrSql = " SELECT STONEUNIT FROM " & cnAdminDb & "..SUBITEMMAST  WHERE SUBITEMNAME = '" & txtStSubItem.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "')"
        Else
            StrSql = " SELECT STONEUNIT FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "'"
        End If
        cmbStUnit.Text = objGPack.GetSqlValue(StrSql, , , tran)


        StrSql = " SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "'"
        txtStMetalCode.Text = objGPack.GetSqlValue(StrSql, "DIASTONE", , tran)
        'If txtStMetalCode.Text = "S" Then cmbStUnit.Text = "G" Else cmbStUnit.Text = "C"
        Me.SelectNextControl(txtStItem, True, True, True, True)
    End Sub

    Public Sub CalcStoneWtAmount()
        Dim diaCaratWt As Double = 0
        Dim diaGramWt As Double = 0
        Dim diaPcs As Integer = 0
        Dim diaAmt As Double = 0

        Dim preCaratWt As Double = 0
        Dim preGramWt As Double = 0
        Dim prePcs As Integer = 0
        Dim preAmt As Double = 0

        Dim stoCaratWt As Double = 0
        Dim stoGramWt As Double = 0
        Dim stoPcs As Integer = 0
        Dim stoAmt As Double = 0
        Dim purAmt As Decimal = 0

        For cnt As Integer = 0 To gridStone.RowCount - 1
            With gridStone.Rows(cnt)
                purAmt += Val(.Cells("PURVALUE").Value.ToString)
                StrSql = " SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Cells("ITEM").Value.ToString & "'"
                Dim METALID As String = objGPack.GetSqlValue(StrSql).ToUpper
                Select Case METALID  '.Cells("METALID").Value.ToString
                    Case "D"
                        diaPcs += Val(.Cells("PCS").Value.ToString)
                        diaAmt += Val(.Cells("AMOUNT").Value.ToString)
                    Case "S"
                        stoPcs += Val(.Cells("PCS").Value.ToString)
                        stoAmt += Val(.Cells("AMOUNT").Value.ToString)
                    Case "P"
                        prePcs += Val(.Cells("PCS").Value.ToString)
                        preAmt += Val(.Cells("AMOUNT").Value.ToString)
                End Select
                Select Case .Cells("UNIT").Value.ToString
                    Case "G"
                        If METALID = "S" Then
                            stoGramWt += Val(.Cells("WEIGHT").Value.ToString)
                        ElseIf METALID = "P" Then
                            preGramWt += Val(.Cells("WEIGHT").Value.ToString)
                        ElseIf METALID = "D" Then
                            diaGramWt += Val(.Cells("WEIGHT").Value.ToString)
                        End If
                    Case "C"
                        If .Cells("METALID").Value.ToString = "S" Then
                            stoCaratWt += Val(.Cells("WEIGHT").Value.ToString)
                        ElseIf .Cells("METALID").Value.ToString = "P" Then
                            preCaratWt += Val(.Cells("WEIGHT").Value.ToString)
                        ElseIf .Cells("METALID").Value.ToString = "D" Then
                            diaCaratWt += Val(.Cells("WEIGHT").Value.ToString)
                        End If
                End Select
            End With
        Next

        Dim lessWt As Double = Nothing '(diaCaratWt + stoCaratWt + preCaratWt) / 5 + (diaGramWt + stoGramWt + preGramWt)

        If StudWtDedut.Contains("D") Then
            lessWt += (diaCaratWt / 5) + diaGramWt
        End If
        If StudWtDedut.Contains("S") Then
            lessWt += (stoCaratWt / 5) + stoGramWt
        End If
        If StudWtDedut.Contains("P") Then
            lessWt += (preCaratWt / 5) + preGramWt
        End If
        If StudWtDedut.Contains("N") Then
            lessWt = 0
        End If
        Dim stnAmt As Double = diaAmt + stoAmt + preAmt
        If gridStoneFooter.RowCount > 0 Then
            gridStoneFooter.Rows(0).Cells("WEIGHT").Value = IIf(lessWt <> 0, lessWt, DBNull.Value)
            txtPurLessWt_Wet.Text = IIf(lessWt <> 0, Format(lessWt, FormatNumberStyle(DiaRnd)), "")
            gridStoneFooter.Rows(0).Cells("AMOUNT").Value = IIf(stnAmt <> 0, stnAmt, DBNull.Value)
            gridStoneFooter.Rows(0).Cells("PURVALUE").Value = IIf(purAmt <> 0, purAmt, DBNull.Value)
            CalcPurchaseGrossValue()
        End If
    End Sub
    Public Sub StyleGridStone(ByVal gridStone As DataGridView)
        With gridStone
            .Columns("ITEM").Width = txtStItem.Width + 1
            .Columns("SUBITEM").Width = txtStSubItem.Width + 1
            .Columns("UNIT").Width = cmbStUnit.Width + 1
            .Columns("CALC").Width = cmbStCalc.Width + 1
            .Columns("PCS").Width = txtStPcs_Num.Width + 1
            .Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("WEIGHT").Width = txtStWeight.Width + 1
            .Columns("WEIGHT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("RATE").Width = txtStRate_Amt.Width + 1
            .Columns("RATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("RATE").DefaultCellStyle.Format = "0.00"
            .Columns("AMOUNT").Width = txtStAmount_Amt.Width + 1
            .Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("AMOUNT").DefaultCellStyle.Format = "0.00"
            .Columns("PURRATE").Width = txtStPurRate.Width + 1
            .Columns("PURVALUE").Width = txtStPuAmount.Width + 1
            .Columns("METALID").Visible = False
            .Columns("KEYNO").Visible = False
            With .Columns("PURRATE")
                '.Width = 80
                .Visible = True
                .DefaultCellStyle.Format = "0.00"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("PURVALUE")
                '.Width = 99
                .Visible = True
                .DefaultCellStyle.Format = "0.00"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            .Columns("STNSNO").Visible = False
        End With
    End Sub

    Private Sub gridStone_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridStone.CellEndEdit

        If Val(gridStone.Rows(e.RowIndex).Cells("PURRATE").Value.ToString) <= Val(gridStone.Rows(e.RowIndex).Cells("RATE").Value.ToString) Then
            If gridStone.Columns(gridStone.CurrentCell.ColumnIndex).Name.ToString = "PURRATE" Then
                Dim amt As Double = Nothing
                If gridStone.Rows(e.RowIndex).Cells("CALC").Value.ToString = "P" Then
                    amt = Val(gridStone.Rows(e.RowIndex).Cells("PURRATE").Value.ToString) * Val(gridStone.Rows(e.RowIndex).Cells("PCS").Value.ToString)
                Else
                    amt = Val(gridStone.Rows(e.RowIndex).Cells("PURRATE").Value.ToString) * Val(gridStone.Rows(e.RowIndex).Cells("WEIGHT").Value.ToString)
                End If
                gridStone.Rows(e.RowIndex).Cells("PURVALUE").Value = IIf(amt <> 0, Format(amt, "0.00"), DBNull.Value)
            Else
                Dim rt As Double = Nothing
                If gridStone.Rows(e.RowIndex).Cells("CALC").Value.ToString = "P" Then
                    rt = Val(gridStone.Rows(e.RowIndex).Cells("PURVALUE").Value.ToString) / Val(gridStone.Rows(e.RowIndex).Cells("PCS").Value.ToString)
                Else
                    rt = Val(gridStone.Rows(e.RowIndex).Cells("PURVALUE").Value.ToString) / Val(gridStone.Rows(e.RowIndex).Cells("WEIGHT").Value.ToString)
                End If
                gridStone.Rows(e.RowIndex).Cells("PURRATE").Value = IIf(rt <> 0, Format(rt, "0.00"), DBNull.Value)
            End If
        Else
            If Val(gridStone.Rows(e.RowIndex).Cells("RATE").Value.ToString) = 0 And Val(gridStone.Rows(e.RowIndex).Cells("PURRATE").Value.ToString) <> 0 Then
                If gridStone.Columns(gridStone.CurrentCell.ColumnIndex).Name.ToString = "PURRATE" Then
                    Dim amt As Double = Nothing
                    If gridStone.Rows(e.RowIndex).Cells("CALC").Value.ToString = "P" Then
                        amt = Val(gridStone.Rows(e.RowIndex).Cells("PURRATE").Value.ToString) * Val(gridStone.Rows(e.RowIndex).Cells("PCS").Value.ToString)
                    Else
                        amt = Val(gridStone.Rows(e.RowIndex).Cells("PURRATE").Value.ToString) * Val(gridStone.Rows(e.RowIndex).Cells("WEIGHT").Value.ToString)
                    End If
                    gridStone.Rows(e.RowIndex).Cells("PURVALUE").Value = IIf(amt <> 0, Format(amt, "0.00"), DBNull.Value)
                Else
                    Dim rt As Double = Nothing
                    If gridStone.Rows(e.RowIndex).Cells("CALC").Value.ToString = "P" Then
                        rt = Val(gridStone.Rows(e.RowIndex).Cells("PURVALUE").Value.ToString) / Val(gridStone.Rows(e.RowIndex).Cells("PCS").Value.ToString)
                    Else
                        rt = Val(gridStone.Rows(e.RowIndex).Cells("PURVALUE").Value.ToString) / Val(gridStone.Rows(e.RowIndex).Cells("WEIGHT").Value.ToString)
                    End If
                    gridStone.Rows(e.RowIndex).Cells("PURRATE").Value = IIf(rt <> 0, Format(rt, "0.00"), DBNull.Value)
                End If
            Else
                MsgBox("Invalid Rate", MsgBoxStyle.Information)
                gridStone.Rows(e.RowIndex).Cells("PURRATE").Value = DBNull.Value
                gridStone.CurrentCell = gridStone.Rows(e.RowIndex).Cells("PURRATE")
                gridStone.Select()
            End If
        End If
        If gridStone.Columns(gridStone.CurrentCell.ColumnIndex).Name = "PURRATE" Then
            gridStone.CurrentCell = gridStone.Rows(e.RowIndex).Cells("PURVALUE")
            gridStone.Select()
        End If
        dtGridStone.AcceptChanges()
        CalcStoneWtAmount()
    End Sub

    Private Sub Load_MultiMetal()
        StyleGridMultiMetal()
        With gridMultimetal
            .ColumnHeadersDefaultCellStyle.Font = New Font("ARIAL", 7, FontStyle.Regular)
            gridMultiMetalTotal.DefaultCellStyle.Font = New Font("ARIAL", 7, FontStyle.Regular)
            gridMultiMetalTotal.DefaultCellStyle.BackColor = Me.BackColor
            gridMultiMetalTotal.DefaultCellStyle.ForeColor = Color.Red
            gridMultiMetalTotal.DefaultCellStyle.SelectionBackColor = Me.BackColor
            gridMultiMetalTotal.DefaultCellStyle.SelectionForeColor = Color.Red
            .SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
            .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Regular)
            .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
            .ColumnHeadersVisible = True
        End With
        If gridMultimetal.RowCount > 0 Then
            CalcMetalTotalAmount()
            'gridMultimetal.CurrentCell = gridStone.FirstDisplayedCell
            gridMultimetal_SelectionChanged(Me, New EventArgs)
        End If
        For cnt As Integer = 0 To gridMultimetal.Columns.Count - 1
            gridMultimetal.Columns(cnt).SortMode = DataGridViewColumnSortMode.NotSortable
        Next
    End Sub

    Private Sub Load_Stone()
        StyleGridStone(gridStone)
        StyleGridStone(gridStoneFooter)
        With gridStone
            gridStoneFooter.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            gridStoneFooter.DefaultCellStyle.BackColor = Me.BackColor
            gridStoneFooter.DefaultCellStyle.ForeColor = Color.Red
            gridStoneFooter.DefaultCellStyle.SelectionBackColor = Me.BackColor
            gridStoneFooter.DefaultCellStyle.SelectionForeColor = Color.Red
            .SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
            .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Regular)
            .ColumnHeadersVisible = False
        End With
        If gridStone.RowCount > 0 Then
            CalcStoneWtAmount()
            gridStone.CurrentCell = gridStone.FirstDisplayedCell
            gridStone_SelectionChanged(Me, New EventArgs)
        End If
        For cnt As Integer = 0 To gridStone.Columns.Count - 1
            gridStone.Columns(cnt).SortMode = DataGridViewColumnSortMode.NotSortable
        Next
    End Sub

    Private Sub gridStone_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridStone.SelectionChanged
        If Not gridStone.RowCount > 0 Then Exit Sub
        If gridStone.CurrentCell Is Nothing Then Exit Sub
        'For k As Integer = 0 To gridStone.Columns.Count - 1
        '    If k <= 9 Then
        '        gridStone.Columns(k).ReadOnly = True
        '    Else
        '        gridStone.Columns(k).ReadOnly = False
        '    End If
        'Next
        If gridStone.CurrentCell.ColumnIndex < 9 Then
            gridStone.CurrentCell = gridStone.Rows(gridStone.CurrentCell.RowIndex).Cells("PURRATE")
            'Else
            '    gridStone.CurrentCell = gridStone.Rows(gridStone.CurrentCell.RowIndex).Cells("PURRATE")
        End If
        'If gridStone.DisplayedColumnCount(False) < 9 Then
        '    gridStone.CurrentCell = gridStone.Rows(gridStone.CurrentCell.RowIndex).Cells("PURRATE")
        'End If
        ' ''If gridStone.DisplayedColumnCount(False) = 10 Then
        ' ''    gridStone.CurrentCell = gridStone.Rows(gridStone.CurrentCell.RowIndex).Cells("PURVALUE")
        ' ''Else
        ' ''    gridStone.CurrentCell = gridStone.Rows(gridStone.CurrentCell.RowIndex).Cells("PURRATE")
        ' ''End If
        'If gridStone.Columns(gridStone.CurrentCell.ColumnIndex).Name.ToString = "PURRATE" Then
        '    gridStone.CurrentCell = gridStone.Rows(gridStone.CurrentCell.RowIndex).Cells("PURVALUE")
        'Else
        '    gridStone.CurrentCell = gridStone.Rows(gridStone.CurrentCell.RowIndex).Cells("PURRATE")
        'End If
    End Sub
    Private Sub gridStone_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles gridStone.EditingControlShowing
        'This event is fired when the user tries to edit the content of a cell: 
        If (gridStone.CurrentCell.ColumnIndex = gridStone.Columns("PURRATE").Index Or gridStone.CurrentCell.ColumnIndex = gridStone.Columns("PURVALUE").Index) And Not e.Control Is Nothing Then
            Dim tb As TextBox = CType(e.Control, TextBox)
            AddHandler tb.KeyPress, AddressOf txtAmt_KeyPress
        End If
    End Sub

    Private Sub gridStone_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridStone.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            gridStone.CurrentCell = gridStone.Rows(gridStone.CurrentRow.Index).Cells(0)
        ElseIf e.KeyCode = Keys.Delete Then
            If Not gridStone.RowCount > 0 Then Exit Sub
            If gridStone.CurrentRow Is Nothing Then Exit Sub
            gridStone.Rows.Remove(gridStone.CurrentRow)
            dtGridStone.AcceptChanges()
            CalcStoneWtAmount()
        End If
    End Sub

    Private Sub gridStone_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridStone.KeyPress
        If UCase(e.KeyChar) = "E" Then
            e.Handled = True
            With gridStone.Rows(gridStone.CurrentRow.Index)
                txtStItem.Text = .Cells("ITEM").FormattedValue
                txtStSubItem.Text = .Cells("SUBITEM").FormattedValue
                txtStPcs_Num.Text = .Cells("PCS").FormattedValue
                txtStWeight.Text = .Cells("WEIGHT").FormattedValue
                cmbStUnit.Text = .Cells("UNIT").FormattedValue
                cmbStCalc.Text = .Cells("CALC").FormattedValue
                txtStRate_Amt.Text = .Cells("RATE").FormattedValue
                txtStAmount_Amt.Text = .Cells("AMOUNT").FormattedValue
                txtStPurRate.Text = .Cells("PURRATE").FormattedValue
                txtStPuAmount.Text = .Cells("PURVALUE").FormattedValue

                txtStRowIndex.Text = gridStone.CurrentRow.Index
                txtStItem.Focus()
                txtStItem.SelectAll()
            End With
        ElseIf e.KeyChar = Chr(Keys.Enter) Then
            gridStone.CurrentCell = gridStone.Rows(gridStone.CurrentRow.Index).Cells("PURRATE")
            If gridStone.CurrentRow.Index = gridStone.RowCount - 1 Then
                Me.SelectNextControl(gridStone, True, True, True, True)
            End If
        End If
    End Sub

    Private Sub gridStone_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridStone.GotFocus
        lblStatus.Visible = True
        If Not gridStone.RowCount > 0 Then SendKeys.Send("{TAB}")
    End Sub


    Private Sub CalcStonePurAmount()
        Dim amt As Double = Nothing
        If cmbStCalc.Text = "P" Then
            amt = Val(txtStPurRate.Text) * Val(txtStPcs_Num.Text)
        Else
            amt = Val(txtStPurRate.Text) * Val(txtStWeight.Text)
        End If
        txtStPuAmount.Text = IIf(amt <> 0, Format(amt, "0.00"), "")
    End Sub


    Private Sub CalcStoneAmount()
        Dim amt As Double = Nothing
        If cmbStCalc.Text = "P" Then
            amt = Val(txtStRate_Amt.Text) * Val(txtStPcs_Num.Text)
        Else
            amt = Val(txtStRate_Amt.Text) * Val(txtStWeight.Text)
        End If
        txtStAmount_Amt.Text = IIf(amt <> 0, Format(amt, "0.00"), "")
    End Sub

    Private Sub txtStRate_Amt_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtStRate_Amt.KeyDown
        If e.KeyCode = Keys.Down Then
            gridStone.Select()
        End If
    End Sub

    Private Sub txtStRate_Amt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtStRate_Amt.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            Dim cent As Double = 0
            StrSql = " SELECT ISNULL(CALTYPE,'') CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ITEMNAME,'') ='" & txtStItem.Text & "'"
            Dim mCaltype As String = objGPack.GetSqlValue(StrSql, "CALTYPE", "")
            StrSql = " SELECT ISNULL(CALTYPE,'') CALTYPE FROM " & cnAdminDb & "..SUBITEMMAST WHERE ISNULL(SUBITEMNAME,'') ='" & txtStSubItem.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "')"
            mCaltype = objGPack.GetSqlValue(StrSql, "CALTYPE", "")
            If mCaltype = "D" Then
                cent = Val(txtStWeight.Text)
            Else
                cent = (Val(txtStWeight.Text) / IIf(Val(txtStPcs_Num.Text) = 0, 1, Val(txtStPcs_Num.Text)))
            End If

            'If cmbStUnit.Text = "C" Then cent = (Val(txtStWeight.Text) / IIf(Val(txtStPcs_Num.Text) = 0, 1, Val(txtStPcs_Num.Text))) Else cent = Val(txtStWeight.Text) / IIf(Val(txtStPcs_Num.Text) = 0, 1, Val(txtStPcs_Num.Text))
            cent *= 100
            StrSql = " DECLARE @CENT FLOAT"
            StrSql += " SET @CENT = " & cent & ""
            StrSql += " SELECT MINRATE FROM " & cnAdminDb & "..CENTRATE WHERE"
            StrSql += " @CENT BETWEEN FROMCENT AND TOCENT "
            StrSql += " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "')"
            StrSql += " AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & txtStSubItem.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "')),0)"
            Dim rate As Double = Val(objGPack.GetSqlValue(StrSql, , , tran))
            If Val(txtStRate_Amt.Text) < rate Then
                MsgBox(Me.GetNextControl(txtStRate_Amt, False).Text + E0020 + rate.ToString)
                txtStRate_Amt.Focus()
            End If
        End If
    End Sub

    Private Sub txtStRate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtStRate_Amt.TextChanged
        CalcStoneAmount()
    End Sub

    Private Sub txtStAmount_Amt_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtStAmount_Amt.KeyDown
        If e.KeyCode = Keys.Down Then
            gridStone.Select()
        End If
    End Sub


    Private Sub txtStWeight_Wet_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtStWeight.KeyDown
        If e.KeyCode = Keys.Down Then
            gridStone.Select()
        End If
    End Sub

    Private Sub txtStWeight_Wet_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtStWeight.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            Dim stWeight As Double = IIf(cmbStUnit.Text = "C", Val(txtStWeight.Text) / 5, Val(txtStWeight.Text))
            For cnt As Integer = 0 To gridStone.RowCount - 1
                If txtStRowIndex.Text <> "" Then If Val(txtStRowIndex.Text) = cnt Then Continue For
                With gridStone.Rows(cnt)
                    If .Cells("UNIT").Value.ToString = "C" Then
                        stWeight += Val(.Cells("WEIGHT").Value.ToString) / 5
                    ElseIf .Cells("UNIT").Value.ToString = "G" Then
                        stWeight += Val(.Cells("WEIGHT").Value.ToString)
                    End If
                End With
            Next
            If stWeight > Val(txtPurGrossWt_Wet.Text) Then
                MsgBox(Me.GetNextControl(txtStWeight, False).Text + E0015 + Me.GetNextControl(txtPurGrossWt_Wet, False).Text, MsgBoxStyle.Exclamation)
                txtStWeight.Focus()
                Exit Sub
            End If
        Else
            WeightValidation(txtStWeight, e, DiaRnd)
        End If
    End Sub

    Private Sub txtStWeight_Wet_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtStWeight.LostFocus
        cmbStCalc.Text = IIf(Val(txtStWeight.Text) > 0, "W", "P")
        txtStWeight.Text = IIf(Val(txtStWeight.Text) <> 0, Format(Val(txtStWeight.Text), FormatNumberStyle(DiaRnd)), txtStWeight.Text)
    End Sub

    Private Sub txtStWeight_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtStWeight.TextChanged
        Dim cent As Double = 0
        StrSql = " SELECT ISNULL(CALTYPE,'') CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ITEMNAME,'') ='" & txtStItem.Text & "'"
        Dim mCaltype As String = objGPack.GetSqlValue(StrSql, "CALTYPE", "")
        StrSql = " SELECT ISNULL(CALTYPE,'') CALTYPE FROM " & cnAdminDb & "..SUBITEMMAST WHERE ISNULL(SUBITEMNAME,'') ='" & txtStSubItem.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "')"
        mCaltype = objGPack.GetSqlValue(StrSql, "CALTYPE", "")
        If mCaltype = "D" Then
            cent = Val(txtStWeight.Text)
        Else
            cent = (Val(txtStWeight.Text) / IIf(Val(txtStPcs_Num.Text) = 0, 1, Val(txtStPcs_Num.Text)))
        End If
        '        If cmbStUnit.Text = "C" Then cent = (Val(txtStWeight.Text) / IIf(Val(txtStPcs_Num.Text) = 0, 1, Val(txtStPcs_Num.Text))) Else cent = Val(txtStWeight.Text) / IIf(Val(txtStPcs_Num.Text) = 0, 1, Val(txtStPcs_Num.Text))
        cent *= 100
        StrSql = " DECLARE @CENT FLOAT"
        StrSql += " SET @CENT = " & cent & ""
        StrSql += " SELECT MAXRATE FROM " & cnAdminDb & "..CENTRATE WHERE"
        StrSql += " @CENT BETWEEN FROMCENT AND TOCENT "
        StrSql += " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "')"
        StrSql += " AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & txtStSubItem.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "')),0)"
        Dim rate As Double = Val(objGPack.GetSqlValue(StrSql, "MAXRATE", "", tran))
        If rate <> 0 Then
            txtStRate_Amt.Text = IIf(rate <> 0, Format(rate, "0.00"), "")
        End If
        CalcStoneAmount()
    End Sub

    Private Sub cmbStCalc_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbStCalc.SelectedIndexChanged
        CalcStoneAmount()
    End Sub

    Private Sub txtStPcs_Num_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtStPcs_Num.KeyDown
        If e.KeyCode = Keys.Down Then
            gridStone.Select()
        End If
    End Sub

    Private Sub txtStPcs_Num_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtStPcs_Num.TextChanged
        CalcStoneAmount()
    End Sub

    Private Sub txtStPuAmount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtStPuAmount.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            ''Validation
            If objGPack.Validator_Check(grpStoneDetails) Then Exit Sub
            If objGPack.DupCheck("SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "' AND  STUDDED = 'S' AND ACTIVE = 'Y'") = False Then
                MsgBox("Invalid Item", MsgBoxStyle.Information)
                txtStItem.Focus()
                Exit Sub
            End If
            If UCase(objGPack.GetSqlValue("SELECT SUBITEM FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "' AND STUDDED = 'S' AND ACTIVE = 'Y'")) = "Y" Then
                If txtStSubItem.Text = "" Then
                    MsgBox("SubItem Should Not Empty", MsgBoxStyle.Information)
                    txtStSubItem.Focus()
                    Exit Sub
                End If
                If objGPack.DupCheck("SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "') AND SUBITEMNAME = '" & txtStSubItem.Text & "'") = False Then
                    MsgBox("Invalid SubItem", MsgBoxStyle.Information)
                    txtStSubItem.Focus()
                    Exit Sub
                End If
            Else
                txtStSubItem.Clear()
            End If
            If Val(txtStPcs_Num.Text) = 0 And Val(txtStWeight.Text) = 0 And Val(txtStAmount_Amt.Text) = 0 Then
                MsgBox(Me.GetNextControl(txtStPcs_Num, False).Text + "," + Me.GetNextControl(txtStWeight, False).Text + E0001, MsgBoxStyle.Information)
                txtStItem.Focus()
                Exit Sub
            End If
            Dim cent As Double = Nothing
            StrSql = " SELECT ISNULL(CALTYPE,'') CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ITEMNAME,'') ='" & txtStItem.Text & "'"
            Dim mCaltype As String = objGPack.GetSqlValue(StrSql, "CALTYPE", "")
            StrSql = " SELECT ISNULL(CALTYPE,'') CALTYPE FROM " & cnAdminDb & "..SUBITEMMAST WHERE ISNULL(SUBITEMNAME,'') ='" & txtStSubItem.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "')"
            mCaltype = objGPack.GetSqlValue(StrSql, "CALTYPE", "")
            If mCaltype = "D" Then
                cent = Val(txtStWeight.Text)
            Else
                cent = (Val(txtStWeight.Text) / IIf(Val(txtStPcs_Num.Text) = 0, 1, Val(txtStPcs_Num.Text)))
            End If
            '            If cmbStUnit.Text = "C" Then cent = (Val(txtStWeight.Text) / IIf(Val(txtStPcs_Num.Text) = 0, 1, Val(txtStPcs_Num.Text))) Else cent = Val(txtStWeight.Text) / IIf(Val(txtStPcs_Num.Text) = 0, 1, Val(txtStPcs_Num.Text))
            cent *= 100
            StrSql = " DECLARE @CENT FLOAT"
            StrSql += " SET @CENT = " & cent & ""
            StrSql += " SELECT MINRATE FROM " & cnAdminDb & "..CENTRATE WHERE"
            StrSql += " @CENT BETWEEN FROMCENT AND TOCENT "
            StrSql += " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "')"
            StrSql += " AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & txtStSubItem.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "')),0)"
            Dim rate As Double = Val(objGPack.GetSqlValue(StrSql, , , tran))
            If Val(txtStRate_Amt.Text) < rate Then
                MsgBox(Me.GetNextControl(txtStRate_Amt, False).Text + E0020 + rate.ToString)
                txtStRate_Amt.Focus()
                Exit Sub
            End If
            Dim stWeight As Double = IIf(cmbStUnit.Text = "C", Val(txtStWeight.Text) / 5, Val(txtStWeight.Text))
            For cnt As Integer = 0 To gridStone.RowCount - 1
                If txtStRowIndex.Text <> "" Then If Val(txtStRowIndex.Text) = cnt Then Continue For
                With gridStone.Rows(cnt)
                    If .Cells("UNIT").Value.ToString = "C" Then
                        stWeight += Val(.Cells("WEIGHT").Value.ToString) / 5
                    ElseIf .Cells("UNIT").Value.ToString = "G" Then
                        stWeight += Val(.Cells("WEIGHT").Value.ToString)
                    End If
                End With
            Next
            If stWeight > Val(txtPurGrossWt_Wet.Text) Then
                MsgBox(Me.GetNextControl(txtStWeight, False).Text + E0015 + Me.GetNextControl(txtPurGrossWt_Wet, False).Text, MsgBoxStyle.Exclamation)
                txtStWeight.Focus()
                Exit Sub
            End If
            If txtStRowIndex.Text <> "" Then
                'If MessageBox.Show("Would you like to update this Entry", "Update Alert", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.OK Then
                With gridStone.Rows(Val(txtStRowIndex.Text))
                    .Cells("ITEM").Value = txtStItem.Text
                    .Cells("SUBITEM").Value = txtStSubItem.Text
                    .Cells("UNIT").Value = cmbStUnit.Text
                    .Cells("CALC").Value = cmbStCalc.Text
                    .Cells("PCS").Value = IIf(Val(txtStPcs_Num.Text) <> 0, Val(txtStPcs_Num.Text), DBNull.Value)
                    .Cells("WEIGHT").Value = IIf(Val(txtStWeight.Text) <> 0, Format(Val(txtStWeight.Text), FormatNumberStyle(DiaRnd)), DBNull.Value)
                    .Cells("RATE").Value = IIf(Val(txtStRate_Amt.Text) <> 0, Format(Val(txtStRate_Amt.Text), "0.00"), DBNull.Value)
                    .Cells("AMOUNT").Value = IIf(Val(txtStAmount_Amt.Text) <> 0, Format(Val(txtStAmount_Amt.Text), "0.00"), DBNull.Value)
                    .Cells("PURRATE").Value = IIf(Val(txtStPurRate.Text) <> 0, Format(Val(txtStPurRate.Text), "0.00"), DBNull.Value)
                    .Cells("PURVALUE").Value = IIf(Val(txtStPuAmount.Text) <> 0, Format(Val(txtStPuAmount.Text), "0.00"), DBNull.Value)
                    .Cells("METALID").Value = txtStMetalCode.Text
                    dtGridStone.AcceptChanges()
                    GoTo AFTERINSERT
                End With
                'End If
            End If
            ''Insertion
            Dim ro As DataRow = Nothing
            ro = dtGridStone.NewRow
            ro("ITEM") = txtStItem.Text
            ro("SUBITEM") = txtStSubItem.Text
            ro("PCS") = IIf(Val(txtStPcs_Num.Text) <> 0, Val(txtStPcs_Num.Text), DBNull.Value)
            ro("UNIT") = cmbStUnit.Text
            ro("CALC") = cmbStCalc.Text
            ro("WEIGHT") = IIf(Val(txtStWeight.Text) <> 0, Format(Val(txtStWeight.Text), FormatNumberStyle(DiaRnd)), DBNull.Value)
            ro("RATE") = IIf(Val(txtStRate_Amt.Text) <> 0, Format(Val(txtStRate_Amt.Text), "0.00"), DBNull.Value)
            ro("AMOUNT") = IIf(Val(txtStAmount_Amt.Text) <> 0, Format(Val(txtStAmount_Amt.Text), "0.00"), DBNull.Value)
            ro("PURRATE") = IIf(Val(txtStPurRate.Text) <> 0, Format(Val(txtStPurRate.Text), "0.00"), DBNull.Value)
            ro("PURVALUE") = IIf(Val(txtStPuAmount.Text) <> 0, Format(Val(txtStPuAmount.Text), "0.00"), DBNull.Value)
            ro("METALID") = txtStMetalCode.Text
            dtGridStone.Rows.Add(ro)
            dtGridStone.AcceptChanges()
            gridStone.CurrentCell = gridStone.Rows(gridStone.RowCount - 1).Cells(0)
AFTERINSERT:
            CalcStoneWtAmount()
            Me.txtPurLessWt_Wet_TextChanged(Me, New EventArgs)
            CalcPurchaseGrossValue()

            ''CLEAR
            'cmbStItem_Man.Text = ""
            'cmbStSubItem_Man.Text = ""
            txtStPcs_Num.Clear()
            txtStWeight.Clear()
            txtStRate_Amt.Clear()
            txtStAmount_Amt.Clear()
            txtStMetalCode.Clear()
            txtStPurRate.Clear()
            txtStPuAmount.Clear()
            txtStItem.Focus()
            txtStRowIndex.Clear()
        End If
    End Sub

    Private Sub txtStPurRate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtStPurRate.TextChanged
        CalcStonePurAmount()
    End Sub
#End Region

    Private Sub Initializer_MultiMetal()
        ''MultiMetal
        With dtGridMultiMetal.Columns
            .Add("CATEGORY", GetType(String))
            .Add("RATE", GetType(Double))
            .Add("WEIGHT", GetType(Double))
            .Add("WASTAGEPER", GetType(Double))
            .Add("WASTAGE", GetType(Double))
            .Add("MCPERGRM", GetType(Double))
            .Add("MC", GetType(Double))
            .Add("AMOUNT", GetType(Double))
            .Add("PURRATE", GetType(Double))
            .Add("PURWASTAGEPER", GetType(Double))
            .Add("PURWASTAGE", GetType(Double))
            .Add("PURMCPERGRM", GetType(Double))
            .Add("PURMC", GetType(Double))
            .Add("PURAMOUNT", GetType(Double))
            .Add("KEYNO", GetType(Integer))
        End With
        gridMultimetal.DataSource = dtGridMultiMetal
        StyleGridMultiMetal()

        Dim dtMultiMetalTotal As New DataTable
        dtMultiMetalTotal = dtGridMultiMetal.Copy
        dtMultiMetalTotal.Rows.Add()
        dtMultiMetalTotal.Rows(0).Item("CATEGORY") = "TOTAL"
        dtMultiMetalTotal.AcceptChanges()
        With gridMultiMetalTotal
            .DataSource = dtMultiMetalTotal
            .Columns("CATEGORY").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            For cnt As Integer = 0 To gridMultimetal.ColumnCount - 1
                .Columns(cnt).Width = gridMultimetal.Columns(cnt).Width
                .Columns(cnt).Visible = gridMultimetal.Columns(cnt).Visible
                .Columns(cnt).DefaultCellStyle = gridMultimetal.Columns(cnt).DefaultCellStyle
                .Columns(cnt).ReadOnly = True
            Next
        End With
    End Sub
    Private Sub StyleGridMultiMetal()

        With gridMultimetal
            .Columns("CATEGORY").Width = 150
            .Columns("RATE").Visible = False
            Dim salColFont As New Font("ARIAL", 7, FontStyle.Regular)
            .DefaultCellStyle.Font = salColFont
            With .Columns("WEIGHT")
                .HeaderText = "WEIGHT"
                .Width = 60
                .DefaultCellStyle.Format = "0.000"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("WASTAGEPER")
                .HeaderText = "WAST %"
                .Width = 40
                .DefaultCellStyle.Format = "0.00"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Visible = True
            End With
            With .Columns("WASTAGE")
                .HeaderText = "WAST"
                .Width = 60
                .DefaultCellStyle.Format = "0.000"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Visible = True
            End With
            With .Columns("MCPERGRM")
                .HeaderText = "MC/ GRM"
                .Width = 55
                .DefaultCellStyle.Format = "0.00"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Visible = True
            End With
            With .Columns("MC")
                .HeaderText = "MC"
                .Width = 65
                .DefaultCellStyle.Format = "0.00"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Visible = True
                .DefaultCellStyle.Font = salColFont
            End With
            With .Columns("AMOUNT")
                .HeaderText = "SALE FIXVALUE"
                .Width = 70
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.Format = "0.00"
                .Visible = True
            End With

            With .Columns("PURWASTAGEPER")
                .HeaderText = "PUR WAST%"
                .Width = 45
                .DefaultCellStyle.Format = "0.00"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Visible = True
            End With
            With .Columns("PURWASTAGE")
                .HeaderText = "PUR WAST"
                .Width = 60
                .DefaultCellStyle.Format = "0.000"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("PURMCPERGRM")
                .HeaderText = "PUR MC/GRM"
                .Width = 60
                .DefaultCellStyle.Format = "0.00"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Visible = True
            End With
            With .Columns("PURMC")
                .HeaderText = "PUR MC"
                .Width = 65
                .DefaultCellStyle.Format = "0.00"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("PURRATE")
                .Width = 80
                .HeaderText = "PURRATE"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.Format = "0.00"
                .Visible = False
            End With
            With .Columns("PURAMOUNT")
                .Width = 70
                .HeaderText = "PUR FIXEVAL"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.Format = "0.00"
                .Visible = True
            End With
            .Columns("KEYNO").Visible = False
        End With
        For cnt As Integer = 0 To gridMultimetal.Columns.Count - 1
            gridMultimetal.Columns(cnt).ReadOnly = True
        Next
        gridMultimetal.Columns("PURWASTAGEPER").ReadOnly = False
        gridMultimetal.Columns("PURWASTAGE").ReadOnly = False
        gridMultimetal.Columns("PURMCPERGRM").ReadOnly = False
        gridMultimetal.Columns("PURMC").ReadOnly = False
        gridMultimetal.Columns("PURAMOUNT").ReadOnly = False
    End Sub
#Region "Miscellaneous Details"
    Private Sub Initializer_Miscellaneous()
        With dtGridMisc.Columns
            .Add("MISC", GetType(String))
            .Add("AMOUNT", GetType(Double))
            .Add("PURAMOUNT", GetType(Double))
            .Add("KEYNO", GetType(Integer))
        End With
        gridMisc.DataSource = dtGridMisc
        FormatGridColumns(gridMisc)
        StyleGridMisc(gridMisc)

        Dim dtGridMiscTotal As New DataTable
        dtGridMiscTotal = dtGridMisc.Copy
        dtGridMiscTotal.Rows.Clear()
        dtGridMiscTotal.Rows.Add()
        dtGridMiscTotal.Rows(0).Item("MISC") = "Total"
        With gridMiscTotal
            .DataSource = dtGridMiscTotal
            For Each col As DataGridViewColumn In gridMisc.Columns
                With gridMiscTotal.Columns(col.Name)
                    .Visible = col.Visible
                    .Width = col.Width
                    .DefaultCellStyle = col.DefaultCellStyle
                End With
            Next
        End With
        StyleGridMisc(gridMiscTotal)
    End Sub
    Public Sub StyleGridMisc(ByVal gridMisc As DataGridView)
        With gridMisc
            With .Columns("MISC")
                .HeaderText = "MISCELLANEOUS"
                .Width = 298
            End With
            With .Columns("AMOUNT")
                .HeaderText = "AMOUNT"
                .Width = 99
                .DefaultCellStyle.Format = "0.00"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("PURAMOUNT")
                .Visible = True
                .Width = 99
                .ReadOnly = False
                .DefaultCellStyle.Format = "0.00"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            .Columns("KEYNO").Visible = False
        End With
    End Sub

    Private Sub Load_Misc()
        StyleGridMisc(gridMisc)
        StyleGridMisc(gridMiscTotal)
        With gridMisc
            gridMiscTotal.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            gridMiscTotal.DefaultCellStyle.BackColor = Me.BackColor
            gridMiscTotal.DefaultCellStyle.ForeColor = Color.Red
            gridMiscTotal.DefaultCellStyle.SelectionBackColor = Me.BackColor
            gridMiscTotal.DefaultCellStyle.SelectionForeColor = Color.Red
            .SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
            .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Regular)
            .ColumnHeadersVisible = True
        End With
        If gridMisc.RowCount > 0 Then
            CalcMiscTotalAmount()
            gridMisc.CurrentCell = gridMisc.FirstDisplayedCell
            gridMisc_SelectionChanged(Me, New EventArgs)
        End If
        For cnt As Integer = 0 To gridMisc.Columns.Count - 1
            gridMisc.Columns(cnt).SortMode = DataGridViewColumnSortMode.NotSortable
        Next
    End Sub

    Private Sub gridMisc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridMisc.GotFocus
        If Not gridMisc.RowCount > 0 Then SendKeys.Send("{TAB}")
    End Sub
    Private Sub gridMisc_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridMisc.SelectionChanged
        If Not gridMisc.RowCount > 0 Then Exit Sub
        If gridMisc.CurrentCell Is Nothing Then Exit Sub
        gridMisc.CurrentCell = gridMisc.Rows(gridMisc.CurrentCell.RowIndex).Cells("PURAMOUNT")
    End Sub
    Private Sub gridMisc_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles gridMisc.EditingControlShowing
        'This event is fired when the user tries to edit the content of a cell: 
        If gridMisc.CurrentCell.ColumnIndex = gridMisc.Columns("PURAMOUNT").Index And Not e.Control Is Nothing Then
            Dim tb As TextBox = CType(e.Control, TextBox)
            AddHandler tb.KeyPress, AddressOf txtAmt_KeyPress
        End If
    End Sub
    Private Sub txtAmt_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs)
        If e.KeyChar = "-" Then
            e.Handled = True
            Exit Sub
        End If
        If UCase(e.KeyChar) = "E" Then
            gridStone_KeyPress(Me, New KeyPressEventArgs(Chr(Keys.E)))
            Exit Sub
        End If
        AmountValidation(sender, e)
    End Sub

    Private Sub gridMisc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridMisc.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            e.Handled = True
            gridMisc.CurrentCell = gridMisc.Rows(gridMisc.CurrentRow.Index).Cells("PURAMOUNT")
            If gridMisc.CurrentRow.Index = gridMisc.RowCount - 1 Then
                Me.SelectNextControl(gridMisc, True, True, True, True)
            End If
        End If
    End Sub

    Private Sub gridMisc_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridMisc.CellEndEdit
        If Not Val(gridMisc.Rows(e.RowIndex).Cells("PURAMOUNT").Value.ToString) <= Val(gridMisc.Rows(e.RowIndex).Cells("AMOUNT").Value.ToString) Then
            MsgBox("Invalid Amount", MsgBoxStyle.Information)
            gridMisc.Rows(e.RowIndex).Cells("PURAMOUNT").Value = DBNull.Value
            gridMisc.CurrentCell = gridMisc.Rows(e.RowIndex).Cells("PURAMOUNT")
            gridMisc.Select()
        End If
        dtGridMisc.AcceptChanges()
        CalcMiscTotalAmount()
    End Sub

    Public Sub CalcMetalTotalAmount()
        Dim metal As Decimal = Nothing
        For cnt As Integer = 0 To gridMultimetal.Rows.Count - 1
            metal += Val(gridMultimetal.Rows(cnt).Cells("PURAMOUNT").Value.ToString)
        Next
        gridMultiMetalTotal.Rows(0).Cells("PURAMOUNT").Value = IIf(metal <> 0, Format(metal, "0.00"), DBNull.Value)
    End Sub

    Public Sub CalcMiscTotalAmount()
        Dim miscTot As Double = Nothing
        Dim miscTotpUR As Double = Nothing
        For cnt As Integer = 0 To gridMisc.Rows.Count - 1
            miscTot += Val(gridMisc.Rows(cnt).Cells("AMOUNT").Value.ToString)
            miscTotpUR += Val(gridMisc.Rows(cnt).Cells("PURAMOUNT").Value.ToString)
        Next
        gridMiscTotal.Rows(0).Cells("AMOUNT").Value = IIf(miscTot <> 0, Format(miscTot, "0.00"), DBNull.Value)
        gridMiscTotal.Rows(0).Cells("PURAMOUNT").Value = IIf(miscTotpUR <> 0, Format(miscTotpUR, "0.00"), DBNull.Value)
        CalcPurchaseGrossValue()
    End Sub
#End Region

    Private Sub txtPurGrossWt_Wet_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPurGrossWt_Wet.TextChanged
        Me.txtPurTouch_TextChanged(Me, e)
        Me.txtPurWastagePer_TextChanged(Me, e)
        Me.txtPurMcPerGrm_TextChanged(Me, e)
        Me.txtPurLessWt_Wet_TextChanged(Me, New EventArgs)
    End Sub

    Private Sub gridStone_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridStone.LostFocus
        lblStatus.Visible = False
    End Sub

    Private Sub txtPurTax_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPurTax_AMT.TextChanged
        CalcPurchaseValue()
    End Sub

    Private Sub txtPurTaxPer_PER_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPurTaxPer_PER.TextChanged
        CalcTax()
    End Sub

    Private Sub CalcTax()
        Dim tax As Decimal = Nothing
        'tax = GetPurGrossValue() * (Val(txtPurTaxPer_PER.Text) / 100)
        tax = Val(txtPurGrossValue_AMT.Text) * (Val(txtPurTaxPer_PER.Text) / 100)
        txtPurTax_AMT.Text = IIf(tax <> 0, Format(tax, "0.00"), "")
    End Sub

    Private Sub txtPurGrossValue_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPurGrossValue_AMT.GotFocus
        If Val(txtPurRate_Amt.Text) = 0 Then purratecal = True
    End Sub

    Private Sub txtPurGrossValue_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPurGrossValue_AMT.KeyPress
        If Not (e.KeyChar = Chr(Keys.Enter)) Then Exit Sub
        If purratecal And PUR_RATECALC And Val(txtPurGrossWt_Wet.Text) <> 0 And Val(txtPurGrossValue_AMT.Text) <> 0 Then
            txtPurRate_Amt.Text = Format(Val(txtPurGrossValue_AMT.Text) / Val(txtPurGrossWt_Wet.Text), "0.00")
        End If
    End Sub

    Private Sub txtPurGrossValue_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPurGrossValue_AMT.TextChanged
        CalcTax()
        CalcPurchaseValue()
    End Sub

    Private Sub gridMultimetal_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridMultimetal.CellEndEdit
        celWasEndEdit = gridMultimetal(e.ColumnIndex, e.RowIndex)
        Select Case gridMultimetal.Columns(e.ColumnIndex).Name
            Case "PURWASTAGEPER"
                If Not Val(gridMultimetal.Rows(e.RowIndex).Cells("PURWASTAGEPER").Value.ToString) <= Val(gridMultimetal.Rows(e.RowIndex).Cells("WASTAGEPER").Value.ToString) Then
                    MsgBox("Invalid Wastage%", MsgBoxStyle.Information)
                    gridMultimetal.Rows(e.RowIndex).Cells("PURWASTAGEPER").Value = DBNull.Value
                    gridMultimetal.CurrentCell = gridMultimetal.Rows(e.RowIndex).Cells("PURWASTAGEPER")
                    gridMultimetal.Select()
                Else
                    Dim was As Decimal = Val(gridMultimetal.CurrentRow.Cells("WEIGHT").Value.ToString) * Val(gridMultimetal.CurrentRow.Cells("PURWASTAGEPER").Value.ToString) / 100
                    gridMultimetal.CurrentRow.Cells("PURWASTAGE").Value = IIf(was <> 0, Format(was, "0.000"), DBNull.Value)
                End If
            Case "PURWASTAGE"
                If Not Val(gridMultimetal.Rows(e.RowIndex).Cells("PURWASTAGE").Value.ToString) <= Val(gridMultimetal.Rows(e.RowIndex).Cells("WASTAGE").Value.ToString) And Val(gridMultimetal.Rows(e.RowIndex).Cells("WASTAGE").Value.ToString) <> 0 Then
                    MsgBox("Invalid Wastage", MsgBoxStyle.Information)
                    gridMultimetal.Rows(e.RowIndex).Cells("PURWASTAGE").Value = DBNull.Value
                    gridMultimetal.CurrentCell = gridMultimetal.Rows(e.RowIndex).Cells("PURWASTAGE")
                    gridMultimetal.Select()
                End If
            Case "PURMCPERGRM"
                If Not Val(gridMultimetal.Rows(e.RowIndex).Cells("PURMCPERGRM").Value.ToString) <= Val(gridMultimetal.Rows(e.RowIndex).Cells("MCPERGRM").Value.ToString) And Val(gridMultimetal.Rows(e.RowIndex).Cells("MCPERGRM").Value.ToString) <> 0 Then
                    MsgBox("Invalid Mc%", MsgBoxStyle.Information)
                    gridMultimetal.Rows(e.RowIndex).Cells("PURMCPERGRM").Value = DBNull.Value
                    gridMultimetal.CurrentCell = gridMultimetal.Rows(e.RowIndex).Cells("PURMCPERGRM")
                    gridMultimetal.Select()
                Else
                    Dim mc As Decimal = Val(gridMultimetal.CurrentRow.Cells("WEIGHT").Value.ToString) * Val(gridMultimetal.CurrentRow.Cells("PURMCPERGRM").Value.ToString)
                    gridMultimetal.CurrentRow.Cells("PURMC").Value = IIf(mc <> 0, Format(mc, "0.00"), DBNull.Value)
                End If
            Case "PURMC"
                If Not Val(gridMultimetal.Rows(e.RowIndex).Cells("PURMC").Value.ToString) <= Val(gridMultimetal.Rows(e.RowIndex).Cells("MC").Value.ToString) Then
                    If Val(gridMultimetal.Rows(e.RowIndex).Cells("MC").Value.ToString) <> 0 Then
                        MsgBox("Invalid Mc", MsgBoxStyle.Information)
                        gridMultimetal.Rows(e.RowIndex).Cells("PURMC").Value = DBNull.Value
                        gridMultimetal.CurrentCell = gridMultimetal.Rows(e.RowIndex).Cells("PURMC")
                        gridMultimetal.Select()
                    End If
                End If
            Case "PURAMOUNT"
                If Not Val(gridMultimetal.Rows(e.RowIndex).Cells("PURAMOUNT").Value.ToString) <= Val(gridMultimetal.Rows(e.RowIndex).Cells("AMOUNT").Value.ToString) Then
                    If Val(gridMultimetal.Rows(e.RowIndex).Cells("AMOUNT").Value.ToString) <> 0 Then
                        MsgBox("Invalid Amount", MsgBoxStyle.Information)
                        gridMultimetal.Rows(e.RowIndex).Cells("PURAMOUNT").Value = DBNull.Value
                        gridMultimetal.CurrentCell = gridMultimetal.Rows(e.RowIndex).Cells("PURAMOUNT")
                        gridMultimetal.Select()
                    End If
                End If
        End Select
        EditRow = e.RowIndex
        EditCol = e.ColumnIndex
        dtGridMultiMetal.AcceptChanges()
        CalcMetalTotalAmount()
    End Sub

    Private Sub gridMultimetal_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles gridMultimetal.EditingControlShowing
        'This event is fired when the user tries to edit the content of a cell: 
        If Not e.Control Is Nothing Then
            If gridMultimetal.CurrentRow Is Nothing Then Exit Sub
            Select Case gridMultimetal.Columns(gridMultimetal.CurrentCell.ColumnIndex).Name
                Case "PURWASTAGEPER", "PURWASTAGE", "PURMCPERGRM", "PURMC", "PURAMOUNT"
                    Dim tb As TextBox = CType(e.Control, TextBox)
                    AddHandler tb.KeyPress, AddressOf txtAmt_KeyPress
            End Select
        End If
    End Sub
    Private Sub gridMultimetal_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridMultimetal.SelectionChanged
        If gridMultimetal.CurrentRow Is Nothing Then Exit Sub
        'If celWasEndEdit IsNot Nothing AndAlso _
        '     gridMultimetal.CurrentCell IsNot Nothing Then
        '    ' if we are currently in the next line of last edit cell
        '    If (gridMultimetal.CurrentCell.RowIndex = celWasEndEdit.RowIndex + 1 AndAlso _
        '       gridMultimetal.CurrentCell.ColumnIndex = celWasEndEdit.ColumnIndex) Then
        '        Dim iColNew As Integer
        '        Dim iRowNew As Integer
        '        ' if we at the last column
        '        If celWasEndEdit.ColumnIndex >= gridMultimetal.ColumnCount - 1 Then
        '            iColNew = 0                         ' move to first column
        '            iRowNew = gridMultimetal.CurrentCell.RowIndex   ' and move to next row
        '        Else ' else it means we are NOT at the last column
        '            ' move to next column
        '            iColNew = celWasEndEdit.ColumnIndex + 1
        '            ' but row should remain same
        '            iRowNew = celWasEndEdit.RowIndex
        '        End If
        '        celWasEndEdit = gridMultimetal(iColNew, iRowNew)   ' ok set the current column
        '        If Not celWasEndEdit.Visible Or celWasEndEdit.ReadOnly Then
        '            For rIndex As Integer = iRowNew To gridMultimetal.RowCount - 1
        '                'If gridMultimetal.Rows(rIndex).Cells("COLHEAD").Value.ToString <> "" Then Continue For
        '                For cIndex As Integer = iColNew To gridMultimetal.Columns.Count - 1
        '                    celWasEndEdit = gridMultimetal.Rows(rIndex).Cells(cIndex)
        '                    If celWasEndEdit.Visible And Not celWasEndEdit.ReadOnly Then Exit For
        '                Next
        '                If celWasEndEdit.Visible And Not celWasEndEdit.ReadOnly Then Exit For
        '                iColNew = 0
        '            Next
        '        End If
        '        gridMultimetal.CurrentCell = celWasEndEdit
        '    End If
        'End If
        'celWasEndEdit = Nothing
        Select Case gridMultimetal.Columns(gridMultimetal.CurrentCell.ColumnIndex).Name
            Case "PURWASTAGEPER", "PURWASTAGE", "PURMCPERGRM", "PURMC", "PURAMOUNT"
                gridMultimetal.CurrentCell = gridMultimetal.Rows(gridMultimetal.CurrentRow.Index).Cells(gridMultimetal.CurrentCell.ColumnIndex)
            Case Else
                gridMultimetal.CurrentCell = gridMultimetal.Rows(gridMultimetal.CurrentRow.Index).Cells("PURWASTAGEPER")
        End Select
        'EditRow = -1
        'EditCol = -1
    End Sub
    Private Sub gridMultimetal_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridMultimetal.GotFocus
        If Not gridMultimetal.RowCount > 0 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub gridMultimetal_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridMultimetal.KeyDown
        'If e.KeyCode = Keys.Enter Then
        '    e.SuppressKeyPress = True
        'End If
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
        End If
    End Sub

    Private Sub gridMultimetal_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridMultimetal.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            e.Handled = True
            With gridMultimetal
                If .CurrentCell Is Nothing Then Exit Sub
                .CurrentCell = .Rows(.CurrentRow.Index).Cells(.CurrentCell.ColumnIndex)
                If .CurrentRow.Index = .RowCount - 1 Then
                    SendKeys.Send("{TAB}")
                End If
                'Dim curIndex As Integer = .CurrentRow.Index
                'Select Case .Columns(.CurrentCell.ColumnIndex).Name
                '    Case "PURWASTAGEPER"
                '        .CurrentCell = .Rows(curIndex).Cells("PURWASTAGE")
                '    Case "PURWASTAGE"
                '        .CurrentCell = .Rows(curIndex).Cells("PURMCPERGRM")
                '    Case "PURMCPERGRM"
                '        .CurrentCell = .Rows(curIndex).Cells("PURMC")
                '    Case "PURMC"
                '        .CurrentCell = .Rows(curIndex).Cells("PURAMOUNT")
                '    Case "PURAMOUNT"
                '        If Not .CurrentRow.Index = .RowCount - 1 Then
                '            .CurrentCell = .Rows(curIndex + 1).Cells("PURWASTAGEPER")
                '        Else    
                '            SendKeys.Send("{TAB}")
                '        End If
                '    Case Else
                '        .CurrentCell = .Rows(curIndex).Cells(.CurrentCell.ColumnIndex)
                'End Select
            End With
        End If
    End Sub

    Private Sub txtPurNetWt_Wet_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPurNetWt_Wet.TextChanged
        Me.txtPurTouch_TextChanged(Me, e)
        Me.txtPurWastagePer_TextChanged(Me, e)
        Me.txtPurMcPerGrm_TextChanged(Me, e)
    End Sub


    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If PUR_LANDCOST Then
            If Val(txtSaleRate_PER.Text.ToString) <> 0 Then
                saleValue = Format(Val(txtPurGrossValue_AMT.Text.ToString) * Val(txtSaleRate_PER.Text.ToString), "0.00")
            End If
        End If
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()

    End Sub

    Private Sub txtpURFixedValueVa_AMT_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtpURFixedValueVa_AMT.TextChanged
        If Val(txtPurGrossValue_AMT.Text) > 0 Then
            saleValue = (Val(txtPurGrossValue_AMT.Text) * ((Val(txtpURFixedValueVa_AMT.Text)) / 100))
            saleValue = Val(txtPurGrossValue_AMT.Text) + saleValue
        End If
    End Sub

    Private Sub txtSaleRate_PER_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtSaleRate_PER.KeyPress
        'e.Handled = True 
        'If e.KeyChar = Chr(Keys.Enter) Then
        '    If saveflag = False Then
        '        Me.Close()
        '    Else
        '        btnSave.Focus()
        '    End If
        'End If
    End Sub

    Private Sub txtPurPurchaseVal_Amt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPurPurchaseVal_Amt.TextChanged

    End Sub

    Private Sub txtPurLCostper_PER_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPurLCostper_NUM.TextChanged
        If Val(txtPurPurchaseVal_Amt.Text) > 0 Then
            Dim LandCost As Decimal = (Val(txtPurPurchaseVal_Amt.Text) * ((Val(txtPurLCostper_NUM.Text)) / 100))
            If CurrencyDecimal = 3 Then
                txtPurLCost_NUM.Text = IIf(Val(txtPurPurchaseVal_Amt.Text) + LandCost <> 0, Format(Val(txtPurPurchaseVal_Amt.Text) + LandCost, "0.000"), "")
            Else
                txtPurLCost_NUM.Text = IIf(Val(txtPurPurchaseVal_Amt.Text) + LandCost <> 0, Format(Val(txtPurPurchaseVal_Amt.Text) + LandCost, "0.00"), "")
            End If
        End If
    End Sub
    Private Sub txtSaleRate_PER_Leave(sender As Object, e As EventArgs) Handles txtSaleRate_PER.Leave
        If PUR_LANDCOST Then
            If Val(txtSaleRate_PER.Text.ToString) <> 0 Then
                saleValue = Format(Val(txtPurGrossValue_AMT.Text.ToString) * Val(txtSaleRate_PER.Text.ToString), "0.00")
            End If
        End If
    End Sub

    Private Sub TagPurchaseDetailEntry_Leave(sender As Object, e As EventArgs) Handles Me.Leave
        If PUR_LANDCOST Then
            If Val(txtSaleRate_PER.Text.ToString) <> 0 Then
                saleValue = Format(Val(txtPurGrossValue_AMT.Text.ToString) * Val(txtSaleRate_PER.Text.ToString), "0.00")
            End If
        End If
    End Sub
End Class