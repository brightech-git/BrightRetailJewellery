Imports System.IO
Imports System.Data.OleDb

Public Class TagSplit
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim TAGNO As Integer = 0
    Dim Pcs As Double = 0
    Dim GrsWt As Double = 0
    Dim NetWt As Double = 0
    Dim LessWt As Double = 0
    Dim StnPcs As Double = 0
    Dim StnWt As Double = 0
    Dim dtTagDet As New DataTable
    Dim dtTagStnDet As New DataTable
    Dim dtTagSplitDetails As New DataTable
    Dim dtTagSplitStoneDet As New DataTable
    Dim splittagno As Boolean = True
    Public objSoftKeys As New SoftKeys
    Dim objTag As New TagGeneration
    Dim ObjMaxMinValue As TagMaxMinValues
    Dim tagprefix As String = GetAdmindbSoftValue("TAGSPLITPREFIX")
    Dim TAGSPLITDATE As Boolean = IIf(GetAdmindbSoftValue("TAGSPLITDATE", "N") = "Y", True, False)
    Dim MANUAL_TAGNO As Boolean = IIf(GetAdmindbSoftValue("MANUAL_TAGNO", "N") = "Y", True, False)
    Dim addAutowt As Boolean = True
    Dim TAGSPLITPER As String = GetAdmindbSoftValue("TAGSPLIT_PER")
    Dim McWithWastage As Boolean = IIf(GetAdmindbSoftValue("MCWITHWASTAGE", "N") = "Y", True, False)
    Dim _HasPurchase As Boolean = IIf(GetAdmindbSoftValue("PURTAB", "N") = "N", False, True)
    Dim StudWtDedut As String = GetAdmindbSoftValue("STUDWTDEDUCT", "DSP")
    Dim WastageRound As Integer = Val(GetAdmindbSoftValue("ROUNDOFF-WASTAGE", "3"))
    Dim CalcGrsNet As String = "G"
    Dim dtTempTagDet As New DataTable
    Dim Prefix As String = GetAdmindbSoftValue("TAGPREFIX")
    Dim IsMainCostCentre As Boolean = IIf(GetAdmindbSoftValue("SYNC-TO", "N") = "Y", False, True)
    Dim TagSplitNarr As Boolean = IIf(GetAdmindbSoftValue("TAGSPLITNARR", "N") = "Y", True, False)
    Dim TagSplitNoGen_Format2 As Boolean = IIf(GetAdmindbSoftValue("TAGSPLITNOGEN_FORMAT2", "N") = "Y", True, False)
    Dim TagSplitZeroPcs As Boolean = IIf(GetAdmindbSoftValue("TAGSPLITZEROPCS", "Y") = "Y", True, False)
    Dim TagSplitDefltItem As Boolean = IIf(GetAdmindbSoftValue("TAGSPLITDEFLTITEM", "N") = "Y", True, False)
    Dim CallBarcodeExe As Boolean = IIf(GetAdmindbSoftValue("CALLBARCODEEXE_TAGSPLIT", "N") = "Y", True, False)
    Dim TagPrefix_Item As Boolean = IIf(GetAdmindbSoftValue("TAGPREFIX_ITEM", "N") = "Y", True, False)

    Private Sub TagSplit_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.BackColor = SystemColors.InactiveCaption
        objGPack.Validator_Object(Me)
        cmbStUnit.Items.Add("C")
        cmbStUnit.Items.Add("G")
        cmbStUnit.Text = "C"
        cmbStCalc.Items.Add("W")
        cmbStCalc.Items.Add("P")
        cmbStCalc.Text = "W"
        pnlstuddedpcs_OWN.Visible = False
        pnlRatio_OWN.Visible = False
        funcNew()
    End Sub
    Function funcNew()
        funcClear()
        With dtTagSplitDetails.Columns
            .Add("ITEM", GetType(String))
            .Add("SUBITEM", GetType(String))
            .Add("TAGNO", GetType(String))
            .Add("PCS", GetType(Double))
            .Add("GRSWT", GetType(Decimal))
            .Add("NETWT", GetType(Decimal))
            .Add("SALVALUE", GetType(Decimal))
            .Add("LESSWT", GetType(Decimal))
            .Add("STNPCS", GetType(Double))
            .Add("STNWT", GetType(Decimal))
            .Add("KEYNO", GetType(Integer))
            .Add("NEWTAGNO", GetType(String))
        End With
        dtTagSplitDetails.Columns("KEYNO").AutoIncrement = True
        dtTagSplitDetails.Columns("KEYNO").AutoIncrementStep = 1
        dtTagSplitDetails.Columns("KEYNO").AutoIncrementSeed = 1
        gridMultiTag.DataSource = dtTagSplitDetails

        Dim dtTagSplitDetTot As New DataTable
        dtTagSplitDetTot = dtTagSplitDetails.Copy
        dtTagSplitDetTot.Rows.Clear()
        dtTagSplitDetTot.Rows.Add()
        gridMultTagTotal.ColumnHeadersVisible = False
        gridMultTagTotal.DataSource = dtTagSplitDetTot
        'ClearDtGrid(dtTagSplitDetails)
        StyleGrid(gridMultiTag)
        StyleGrid(gridMultTagTotal)
        With dtTagSplitStoneDet.Columns
            .Add("STNITEM", GetType(String))
            .Add("STNSUBITEM", GetType(String))
            .Add("STNTAGNO", GetType(String))
            .Add("STNPCS", GetType(Double))
            .Add("STNWT", GetType(Decimal))
            .Add("STNUNIT", GetType(String))
            .Add("STNCAL", GetType(String))
            .Add("STNRATE", GetType(Decimal))
            .Add("STNAMT", GetType(Decimal))
            .Add("KEYNO", GetType(Integer))
            .Add("METALID", GetType(String))
            .Add("NEWSTNTAGNO", GetType(String))
        End With
        dtTagSplitStoneDet.Columns("KEYNO").AutoIncrement = True
        dtTagSplitStoneDet.Columns("KEYNO").AutoIncrementStep = 1
        dtTagSplitStoneDet.Columns("KEYNO").AutoIncrementSeed = 1
        gridTagStone.DataSource = dtTagSplitStoneDet
        Dim dtTagSplitStoneDetTot As New DataTable
        dtTagSplitStoneDetTot = dtTagSplitStoneDet.Copy
        dtTagSplitStoneDetTot.Rows.Clear()
        dtTagSplitStoneDetTot.Rows.Add()
        gridTagStonetotal.ColumnHeadersVisible = False
        gridTagStonetotal.DataSource = dtTagSplitStoneDetTot
        StyleGridStone(gridTagStone)
        StyleGridStone(gridTagStonetotal)
        txtItemId.Focus()
        showgrid(gridMultiTag)
        showgrid(gridMultTagTotal)
        showgridstone(gridTagStone)
        showgridstone(gridTagStonetotal)
        txtSplitPcs_NUM.Text = ""
        ChkAutoSPlit.Checked = False
        ChkRatio.Checked = False
        addAutowt = True
        splittagno = True
        txtSplitRatio_NUM.Enabled = True
        txtSpiltTag_NUM.Enabled = True
        txt_TagNo.Enabled = True
        TAGNO = 0
        Pcs = 0
        GrsWt = 0
        NetWt = 0
        LessWt = 0
        StnPcs = 0
        StnWt = 0
        CalcGrsNet = "G"
        ObjMaxMinValue = New TagMaxMinValues
        AddHandler ObjMaxMinValue.txtMinWastage_Per.KeyPress, AddressOf ObjMinValues_txtMinWastage_Per_KeyPress
        AddHandler ObjMaxMinValue.txtMinWastage_Per.TextChanged, AddressOf ObjMinValues_txtMinWastage_Per_TextChanged
        AddHandler ObjMaxMinValue.txtMinMcPerGram_Amt.KeyPress, AddressOf ObjMinValues_txtMinMcPerGram_Amt_KeyPress
        AddHandler ObjMaxMinValue.txtMinMcPerGram_Amt.TextChanged, AddressOf ObjMinValues_txtMinMcPerGram_Amt_TextChanged
        AddHandler ObjMaxMinValue.txtMinWastage_Wet.GotFocus, AddressOf ObjMinValue_txtMinWastage_Wet_GotFocus
        AddHandler ObjMaxMinValue.txtMinWastage_Wet.KeyPress, AddressOf ObjMinValue_txtMinWastage_Wet_KeyPress
        AddHandler ObjMaxMinValue.txtMinMkCharge_Amt.GotFocus, AddressOf ObjMinValue_txtMinMkCharge_Amt_GotFocus
        AddHandler ObjMaxMinValue.txtMinMkCharge_Amt.KeyPress, AddressOf ObjMinValue_txtMinMkCharge_Amt_KeyPress
        AddHandler ObjMaxMinValue.txtMinWastage_Wet.TextChanged, AddressOf ObjMinValue_txtMinWastage_Wet_TextChanged
        If cnCostId <> cnHOCostId And cnCostId <> "" Then
            If tagprefix = "" Then tagprefix = IIf(TagSplitNoGen_Format2, "", cnCostId)
        End If
    End Function

    Private Sub ObjMinValue_txtMinWastage_Wet_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.ObjMinValues_txtMinMcPerGram_Amt_TextChanged(Me, New EventArgs)
    End Sub

    Private Sub ObjMinValue_txtMinWastage_Wet_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        If Val(ObjMaxMinValue.txtMinWastage_Per.Text) > 0 Then ObjMaxMinValue.txtMinWastage_Wet.ReadOnly = True Else ObjMaxMinValue.txtMinWastage_Wet.ReadOnly = False
    End Sub

    Private Sub ObjMinValue_txtMinWastage_Wet_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Not Val(ObjMaxMinValue.txtMaxWastage_Wet.Text) >= Val(ObjMaxMinValue.txtMinWastage_Wet.Text + e.KeyChar) Then
            e.Handled = True
            MsgBox("Check Maximum value")
        End If
    End Sub

    Private Sub ObjMinValue_txtMinMkCharge_Amt_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        If Val(ObjMaxMinValue.txtMinMcPerGram_Amt.Text) > 0 Then ObjMaxMinValue.txtMinMcPerGram_Amt.ReadOnly = True Else ObjMaxMinValue.txtMinMcPerGram_Amt.ReadOnly = False
    End Sub

    Private Sub ObjMinValue_txtMinMkCharge_Amt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Not Val(ObjMaxMinValue.txtMaxMkCharge_Amt.Text) >= Val(ObjMaxMinValue.txtMinMkCharge_Amt.Text + e.KeyChar) Then
            e.Handled = True
            MsgBox("Check Maximum value")
        End If
    End Sub
    Private Sub ObjMinValues_txtMinWastage_Per_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Not Val(ObjMaxMinValue.txtMinWastage_Per.Text + e.KeyChar) <= Val(ObjMaxMinValue.txtMaxWastage_Per.Text) Then
            e.Handled = True
        End If
    End Sub

    Private Sub ObjMinValues_txtMinWastage_Per_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim wt As Double = Nothing
        Dim wastwt As Double = 0
        If CalcGrsNet = "G" Then wt = Val(txtGRSWT_Wet.Text) Else wt = Val(txtNETWT_Wet.Text)
        wastwt = wt * (Val(ObjMaxMinValue.txtMinWastage_Per.Text) / 100)
        wastwt = Math.Round(wastwt, WastageRound)
        ObjMaxMinValue.txtMinWastage_Wet.Text = IIf(wastwt <> 0, Format(wastwt, "0.000"), "")
    End Sub

    Private Sub ObjMinValues_txtMinMcPerGram_Amt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Not Val(ObjMaxMinValue.txtMaxMcPerGram_Amt.Text) >= Val(ObjMaxMinValue.txtMinMcPerGram_Amt.Text + e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub ObjMinValues_txtMinMcPerGram_Amt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim mc As Double = Nothing
        Dim wast As Double = IIf(McWithWastage, Val(ObjMaxMinValue.txtMinWastage_Wet.Text), 0)
        Dim wt As Decimal = 0
        If CalcGrsNet Then wt = Val(txtGRSWT_Wet.Text) Else wt = Val(txtNETWT_Wet.Text)
        mc = (wt + wast) * Val(ObjMaxMinValue.txtMinMcPerGram_Amt.Text)
        ObjMaxMinValue.txtMinMkCharge_Amt.Text = IIf(mc <> 0, Format(mc, "0.00"), "")
    End Sub
    Private Sub ClearDtGrid(ByVal dt As DataTable)
        dt.Columns("KeyNo").AutoIncrementSeed = 1
        dt.Rows.Clear()
        For i As Integer = 1 To 5
            dt.Rows.Add()
        Next
        dt.AcceptChanges()
    End Sub
    Function gridformat()
        With gridViewTAG
            .Columns("SNO").Visible = False
            .Columns("GRSNET").Visible = False
            .Columns("ITEM").Width = 210
            .Columns("TAGNO").Width = 160
            .Columns("PCS").Width = 80
            .Columns("GRSWT").Width = 100
            .Columns("NETWT").Width = 100
            .Columns("LESSWT").Width = 100
            .Columns("STNPCS").Width = 80
            .Columns("STNWT").Width = 100
            .Columns("ITEMID").Width = 100
            .Columns("ITEM").HeaderText = "ITEM NAME"
            .Columns("SUBITEM").HeaderText = "SUBITEMNAME"
            .Columns("GRSWT").HeaderText = "GRSWT "
            .Columns("NETWT").HeaderText = "NETWT "
            .Columns("LESSWT").HeaderText = "LESSWT "
            .Columns("STNPCS").HeaderText = "STNPCS "
            .Columns("STNWT").HeaderText = "STNWT "
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8.0!, FontStyle.Regular)
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
            .Invalidate()
            For Each dgvCol As DataGridViewColumn In .Columns
                dgvCol.Width = dgvCol.Width
            Next
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            .Refresh()
        End With
    End Function
    Function gridSTNformat()
        With gridViewTagStone
            .Columns("SNO").Visible = False
            .Columns("STNITEMNAME").Width = 160
            .Columns("STNSUBITEMNAME").Width = 120
            .Columns("STNITEMID").Visible = False
            .Columns("METALID").Visible = False
            .Columns("STNITEMID").Width = 90
            .Columns("TAGNO").Width = 90
            .Columns("STNPCS").Width = 55
            .Columns("STNWT").Width = 90
            .Columns("CAL").Width = 50
            .Columns("UNIT").Width = 50
            .Columns("STNRATE").Width = 80
            .Columns("STNAMT").Width = 95
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8.0!, FontStyle.Regular)
        End With
    End Function

    Private Sub CalcGridtagTotal()
        Dim tagPcs As Integer = Nothing
        Dim tagGrsWt As Decimal = Nothing
        Dim tagNetWt As Decimal = Nothing
        Dim tagLessWt As Decimal = Nothing
        Dim tagStnWt As Decimal = Nothing
        Dim tagStnPcs As Double = Nothing
        Dim tagNo As String = ""
        Dim prevtagNo As String = ""
        For i As Integer = 0 To gridMultiTag.RowCount - 1
            With gridMultiTag.Rows(i)
                .DefaultCellStyle.BackColor = SystemColors.HighlightText
                tagPcs += Val(.Cells("PCS").Value.ToString)
                tagGrsWt += Val(.Cells("GRSWT").Value.ToString)
                tagNetWt += Val(.Cells("NETWT").Value.ToString)
                tagLessWt += Val(.Cells("LESSWT").Value.ToString)
                tagStnPcs += Val(.Cells("STNPCS").Value.ToString)
                tagStnWt += Val(.Cells("STNWT").Value.ToString)
                tagNo = .Cells("TAGNO").Value.ToString
                If prevtagNo = tagNo Or prevtagNo = "" Then
                    .DefaultCellStyle.ForeColor = Color.Blue
                    prevtagNo = tagNo
                Else
                    .DefaultCellStyle.ForeColor = Color.Red
                End If
                .DefaultCellStyle.SelectionForeColor = Color.Blue
            End With
        Next
        With gridMultTagTotal.Rows(0)
            .Cells("TAGNO").Value = "TOTAL"
            .Cells("PCS").Value = IIf(tagPcs <> 0, tagPcs, DBNull.Value)
            .Cells("GRSWT").Value = IIf(tagGrsWt <> 0, Format(tagGrsWt, "0.000"), DBNull.Value)
            .Cells("NETWT").Value = IIf(tagNetWt <> 0, Format(tagNetWt, "0.000"), DBNull.Value)
            .Cells("LESSWT").Value = IIf(tagLessWt <> 0, Format(tagLessWt, "0.000"), DBNull.Value)
            .Cells("STNPCS").Value = IIf(tagStnPcs <> 0, tagStnPcs, DBNull.Value)
            .Cells("STNWT").Value = IIf(tagStnWt <> 0, Format(tagStnWt, FormatNumberStyle(3)), DBNull.Value)

            .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            .DefaultCellStyle.ForeColor = Color.Blue
            .DefaultCellStyle.SelectionForeColor = Color.Blue
        End With
    End Sub
    Private Sub CalcGridtagStoneTotal()
        Dim tagStnWt As Decimal = Nothing
        Dim tagStnPcs As Double = Nothing
        Dim tagStnAmt As Double = Nothing
        Dim tagNo As String = ""
        Dim prevtagNo As String = ""
        For i As Integer = 0 To gridTagStone.RowCount - 1
            With gridTagStone.Rows(i)
                .DefaultCellStyle.BackColor = SystemColors.HighlightText
                tagStnPcs += Val(.Cells("STNPCS").Value.ToString)
                If .Cells("STNUNIT").Value.ToString = "C" Then
                    tagStnWt += Val(.Cells("STNWT").Value.ToString) / 5
                ElseIf .Cells("STNUNIT").Value.ToString = "G" Then
                    tagStnWt += Val(.Cells("STNWT").Value.ToString)
                End If
                tagStnAmt += Val(.Cells("STNAMT").Value.ToString)
                tagNo = .Cells("STNTAGNO").Value.ToString
                If prevtagNo = tagNo Or prevtagNo = "" Then
                    .DefaultCellStyle.ForeColor = Color.Blue
                    prevtagNo = tagNo
                Else
                    .DefaultCellStyle.ForeColor = Color.Red
                End If
                .DefaultCellStyle.SelectionForeColor = Color.Blue
            End With
        Next
        With gridTagStonetotal.Rows(0)
            .Cells("STNTAGNO").Value = "TOTAL"
            .Cells("STNPCS").Value = IIf(tagStnPcs <> 0, tagStnPcs, DBNull.Value)
            .Cells("STNWT").Value = IIf(tagStnWt <> 0, Format(tagStnWt, FormatNumberStyle(3)), DBNull.Value)
            .Cells("STNAMT").Value = IIf(tagStnAmt <> 0, Format(tagStnAmt, "0.000"), DBNull.Value)

            .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            .DefaultCellStyle.ForeColor = Color.Blue
            .DefaultCellStyle.SelectionForeColor = Color.Blue
        End With
    End Sub
    Private Sub CalcFinalTotal(Optional ByVal salevalueblock As Boolean = False)
        CalcNetWt()
        CalcGridtagStoneTotal()
        'CalcMultiMetalTotal()
        'CalcMiscTotalAmount()
        'If chkFixedVa.Visible And (Val(txtMaxMkCharge_Org.Text) <> 0 Or Val(txtMaxWastage_Org.Text) <> 0) Then
        '    If Val(txtMaxMkCharge_Amt.Text) <> Val(txtMaxMkCharge_Org.Text) Or Val(txtMaxWastage_Wet.Text) <> Val(txtMaxWastage_Org.Text) Then chkFixedVa.Checked = True Else chkFixedVa.Checked = False
        'End If
        'If Not salevalueblock Then CalcSaleValue()
    End Sub
    Private Sub CalcNetWt()
        Dim wt As Double = Nothing
        wt = Val(txtGRSWT_Wet.Text) - Val(txtLessWt_Wet.Text)
        strSql = "SELECT ISNULL(NETWTPER,0) NETWTPER FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtITEM.Text & "'"
        Dim mnetwtper As Double = Val(objGPack.GetSqlValue(strSql).ToString)
        If mnetwtper <> 0 Then wt = wt * (mnetwtper / 100)
        txtNETWT_Wet.Text = IIf(wt <> 0, Format(wt, "0.000"), "")
    End Sub
    Private Sub CalcLessWt()
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
        Dim Isdeductstud As Boolean = False
        Dim dirlesswt As Double
        For cnt As Integer = 0 To gridTagStone.RowCount - 1
            With gridTagStone.Rows(cnt)
                If txtStTagno.Text <> .Cells("STNTAGNO").Value.ToString Then Continue For
                If StudWtDedut = "I" Then
                    If txtStSubItem.Text <> "" Then
                        strSql = " SELECT STUDDEDUCT FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & .Cells("STNSUBITEM").Value.ToString & "'"
                    Else
                        strSql = " SELECT STUDDEDUCT  FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Cells("STNITEM").Value.ToString & "'"
                    End If
                    Isdeductstud = IIf(objGPack.GetSqlValue(strSql).ToString.ToUpper = "Y", True, False)
                End If
                Select Case .Cells("METALID").Value.ToString
                    Case "D"
                        diaPcs += Val(.Cells("STNPCS").Value.ToString)
                        diaAmt += Val(.Cells("STNAMT").Value.ToString)
                    Case "S"
                        stoPcs += Val(.Cells("STNPCS").Value.ToString)
                        stoAmt += Val(.Cells("STNAMT").Value.ToString)
                    Case "P"
                        prePcs += Val(.Cells("STNPCS").Value.ToString)
                        preAmt += Val(.Cells("STNAMT").Value.ToString)
                End Select
                Select Case .Cells("STNUNIT").Value.ToString
                    Case "G"
                        If .Cells("METALID").Value.ToString = "S" Then
                            stoGramWt += Val(.Cells("STNWT").Value.ToString)
                        ElseIf .Cells("METALID").Value.ToString = "P" Then
                            preGramWt += Val(.Cells("STNWT").Value.ToString)
                        ElseIf .Cells("METALID").Value.ToString = "D" Then
                            diaGramWt += Val(.Cells("STNWT").Value.ToString)
                        End If
                        If Isdeductstud Then dirlesswt += Val(.Cells("STNWT").Value.ToString)
                    Case "C"
                        If .Cells("METALID").Value.ToString = "S" Then
                            stoCaratWt += Val(.Cells("STNWT").Value.ToString)
                        ElseIf .Cells("METALID").Value.ToString = "P" Then
                            preCaratWt += Val(.Cells("STNWT").Value.ToString)
                        ElseIf .Cells("METALID").Value.ToString = "D" Then
                            diaCaratWt += Val(.Cells("STNWT").Value.ToString)
                        End If
                        If Isdeductstud Then dirlesswt += (Val(.Cells("STNWT").Value.ToString) / 5)
                End Select
            End With
        Next
        Dim lessWt As Double = Nothing
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
        lessWt += dirlesswt
        txtLessWt_Wet.Text = IIf(lessWt <> 0, Format(lessWt, "0.000"), "")
    End Sub

    Function showgrid(ByVal gridView As DataGridView)
        With gridView
            .Columns("ITEM").Width = txtITEM.Width + 1
            .Columns("SUBITEM").Width = txtSUBITEm.Width + 1
            .Columns("TAGNO").Width = txtTAGNO.Width + 1
            .Columns("PCS").Width = txtPCS_Num.Width + 1
            .Columns("GRSWT").Width = txtGRSWT_Wet.Width + 1
            .Columns("NETWT").Width = txtNETWT_Wet.Width + 1
            .Columns("LESSWT").Width = txtLessWt_Wet.Width + 1
            .Columns("STNPCS").Width = txtStnpcs_Num.Width + 1
            .Columns("STNWT").Width = txtStnWt_Wet.Width + 1
            .Columns("KEYNO").Visible = False
            .Columns("NEWTAGNO").Visible = False
        End With
    End Function

    Function showgridstone(ByVal gridView As DataGridView)
        With gridView
            .Columns("STNITEM").Width = txtStItem.Width + 1
            .Columns("STNSUBITEM").Width = txtStSubItem.Width + 1
            .Columns("STNTAGNO").Width = txtStTagno.Width + 1
            .Columns("STNPCS").Width = txtStPcs_NUM.Width + 1
            .Columns("STNWT").Width = txtStWeight_WET.Width + 1
            .Columns("STNUNIT").Width = cmbStUnit.Width + 1
            .Columns("STNCAL").Width = cmbStCalc.Width + 1
            .Columns("STNRATE").Width = txtStRate_Amt.Width + 1
            .Columns("STNAMT").Width = txtStAmount_Amt.Width + 1
            .Columns("KEYNO").Visible = False
            .Columns("METALID").Visible = False
            .Columns("NEWSTNTAGNO").Visible = False
        End With
    End Function

    Function funcClear()
        txt_TagNo.Clear()
        txtItemId.Clear()
        txtITEM.Clear()
        txtSUBITEm.Clear()
        txtTAGNO.Clear()
        txtPCS_Num.Clear()
        txtGRSWT_Wet.Clear()
        txtNETWT_Wet.Clear()
        txtLessWt_Wet.Clear()
        txtStnpcs_Num.Clear()
        txtStnWt_Wet.Clear()
        txtSplitRatio_NUM.Clear()
        txtSpiltTag_NUM.Clear()
        txtSplitPcs_NUM.Clear()
        txtStItem.Clear()
        txtStSubItem.Clear()
        txtStTagno.Clear()
        txtStPcs_NUM.Clear()
        txtStWeight_WET.Clear()
        txtStRate_Amt.Clear()
        txtStAmount_Amt.Clear()
        txtStMetalCode.Clear()
        txtStONERowIndex.Text = ""
        txtStRowIndex.Text = ""
        gridViewTAG.DataSource = Nothing
        gridViewTagStone.DataSource = Nothing
        gridMultiTag.DataSource = Nothing
        gridMultTagTotal.DataSource = Nothing
        gridTagStone.DataSource = Nothing
        gridTagStonetotal.DataSource = Nothing
        gridViewTagStone.Visible = False
        dtTagSplitDetails = New DataTable
        dtTagSplitStoneDet = New DataTable
        dtTempTagDet = New DataTable
    End Function
    Private Sub TagSplit_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtITEM.Focused Then Exit Sub
            If txtSUBITEm.Focused Then Exit Sub
            If txtTAGNO.Focused Then Exit Sub
            If txtPCS_Num.Focused Then Exit Sub
            If txtGRSWT_Wet.Focused Then Exit Sub
            If txtNETWT_Wet.Focused Then Exit Sub
            If txtLessWt_Wet.Focused Then Exit Sub
            If txtStnpcs_Num.Focused Then Exit Sub
            If txtStnWt_Wet.Focused Then Exit Sub

            If txtStItem.Focused Then Exit Sub
            If txtStSubItem.Focused Then Exit Sub
            If txtStPcs_NUM.Focused Then Exit Sub
            If txtStWeight_WET.Focused Then Exit Sub
            If txtStTagno.Focused Then Exit Sub
            If txtStRate_Amt.Focused Then Exit Sub
            If txtStAmount_Amt.Focused Then Exit Sub
            If cmbStCalc.Focused Then Exit Sub
            If cmbStUnit.Focused Then Exit Sub
            If txtSplitPcs_NUM.Focused Then Exit Sub
            If txtSplitRatio_NUM.Focused Then Exit Sub
            If gridMultiTag.Focused Then Exit Sub
            If gridTagStone.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        ElseIf e.KeyChar = Chr(Keys.Escape) Then
            btnSave.Focus()
        End If
    End Sub


    Private Sub txt_TagNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_TagNo.KeyDown
        If e.KeyCode = Keys.Insert Then
            If Val(txtItemId.Text) = 0 Then Exit Sub
            Dim stockType As String = objGPack.GetSqlValue("SELECT STOCKTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = '" & txtItemId.Text & "'")
            If stockType = "T" Then
                strSql = " SELECT"
                strSql += vbCrLf + " TAGNO AS TAGNO,STYLENO,ITEMID AS ITEMID,"
                strSql += vbCrLf + " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS ITEMNAME,"
                strSql += vbCrLf + " PCS AS PCS,"
                strSql += vbCrLf + " GRSWT AS GRS_WT,NETWT AS NET_WT,RATE AS RATE,"
                strSql += vbCrLf + " SALVALUE AS SALVALUE,TAGVAL AS TAGVAL,"
                strSql += vbCrLf + " ISNULL((SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID  = T.SUBITEMID),'')AS SUBITEM,"
                strSql += vbCrLf + " CONVERT(VARCHAR,RECDATE,103) AS RECDATE,"
                strSql += vbCrLf + " CASE WHEN APPROVAL = 'A' THEN 'APPROVAL' WHEN APPROVAL = 'R' THEN 'RESERVED' ELSE NULL END AS STATUS,"
                strSql += vbCrLf + " CASE WHEN APPROVAL = 'A' THEN '" & Color.MistyRose.Name & "' WHEN APPROVAL = 'R' THEN '" & Color.PaleTurquoise.Name & "' ELSE NULL END AS COLOR_HIDE"
                strSql += vbCrLf + " FROM"
                strSql += vbCrLf + " " & cnAdminDb & "..ITEMTAG AS T"
                strSql += vbCrLf + " WHERE T.ITEMID = " & Val(txtItemId.Text) & ""
                'If cnHOCostId <> cnCostId Then strSql += vbCrLf + " AND COSTID = '" & cnCostId & "'"
                strSql += vbCrLf + " AND COSTID = '" & cnCostId & "'"
                'strSql += GridTagNoFiltStr(dtTagDetail)
                'strSql += ShowTagFiltration()
                strSql += vbCrLf + " AND ISSDATE IS NULL AND ISNULL(APPROVAL,'')<>'A'"
                strSql += vbCrLf + " ORDER BY TAGNO"
                txt_TagNo.Text = BrighttechPack.SearchDialog.Show("Find TagNo", strSql, cn, , , , , , , , False)
                txt_TagNo.SelectAll()
            ElseIf stockType = "P" Then
                strSql = vbCrLf + " SELECT"
                strSql += vbCrLf + " PACKETNO AS PACKETNO,ITEMID,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = NT.ITEMID) AS ITEMNAME"
                strSql += vbCrLf + " ,SUM(CASE WHEN RECISS = 'R' THEN PCS ELSE -1*PCS END)AS PCS"
                strSql += vbCrLf + " ,SUM(CASE WHEN RECISS = 'R' THEN GRSWT ELSE -1*GRSWT END)AS GRSWT"
                strSql += vbCrLf + " ,SUM(CASE WHEN RECISS = 'R' THEN NETWT ELSE -1*NETWT END)AS NETWT"
                strSql += vbCrLf + " ,ISNULL((SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID  = NT.SUBITEMID),'')AS SUBITEM"
                strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMNONTAG AS NT"
                strSql += vbCrLf + " WHERE NT.ITEMID = " & Val(txtItemId.Text) & ""
                strSql += vbCrLf + " AND COSTID = '" & cnCostId & "'"
                strSql += vbCrLf + " GROUP BY PACKETNO,ITEMID,SUBITEMID"
                strSql += vbCrLf + " HAVING SUM(CASE WHEN RECISS = 'R' THEN PCS ELSE -1*PCS END) > 0"
                strSql += vbCrLf + " AND SUM(CASE WHEN RECISS = 'R' THEN GRSWT ELSE -1*GRSWT END)> 0"
                strSql += vbCrLf + " AND SUM(CASE WHEN RECISS = 'R' THEN NETWT ELSE -1*NETWT END)> 0"
                txt_TagNo.Text = BrighttechPack.SearchDialog.Show("Find PacketNo", strSql, cn)
                'txtRate_AMT.Text = GetSARate()
                txt_TagNo.SelectAll()
            End If
        End If
    End Sub

    Private Sub txt_TagNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_TagNo.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            Dim itemId As String = ""
            If Len(txt_TagNo.Text) > 21 Then '11 - >20
                MsgBox("Given Tag No Length is Exceed", MsgBoxStyle.Information)
                txt_TagNo.Focus()
                Exit Sub
            End If

            Dim spChar As String = GetAdmindbSoftValue("PRODTAGSEP", "")
            If spChar <> "" Then
                If txtItemId.Text.Contains(spChar) Then
                    Dim sp() As String = txtItemId.Text.Split(spChar)
                    itemId = Trim(sp(0))
                    If sp.Length >= 2 Then
                        txt_TagNo.Text = Trim(sp(1))
                        txtItemId.Text = Trim(sp(0))
                    End If
                End If
            End If
            If txt_TagNo.Text = "" Then
                MsgBox("TagNo should not empty", MsgBoxStyle.Information)
                txt_TagNo.Focus()
                Exit Sub
            End If
            If txtItemId.Text <> "" Then
                itemId = Val(txtItemId.Text)
            End If
            strSql = "SELECT RECDATE,LOTSNO,(SELECT TOP 1 ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=T.ITEMID)ITEMNAME "
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T WHERE TAGNO='" & txt_TagNo.Text.ToString & "'"
            dtTempTagDet = New DataTable
            cmd = New OleDbCommand(strSql, cn)
            da = New OleDbDataAdapter(cmd)
            da.Fill(dtTempTagDet)

TagReCheck:
            dtTagDet.Rows.Clear()
            strSql = vbCrLf + " SELECT "
            strSql += vbCrLf + " T.SNO AS SNO,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS ITEM"
            strSql += vbCrLf + " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = T.ITEMID AND SUBITEMID=T.SUBITEMID)AS SUBITEM"
            strSql += vbCrLf + " ,T.ITEMID AS ITEMID,T.TAGNO AS TAGNO,PCS,GRSWT,NETWT,LESSWT"
            strSql += vbCrLf + " ,T.MAXWASTPER AS WASTPER"
            strSql += vbCrLf + " ,T.MAXWAST AS WASTAGE"
            strSql += vbCrLf + " ,T.MAXMCGRM AS MCPER"
            strSql += vbCrLf + " ,T.MAXMC AS MCHARGE"
            strSql += vbCrLf + " ,GRSNET,SALVALUE"
            strSql += vbCrLf + " ,(SELECT SUM (ISNULL(STNPCS,0)) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO =T.SNO ) STNPCS"
            strSql += vbCrLf + ",(SELECT SUM (ISNULL(STNWT,0)) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO=T.SNO ) STNWT FROM " & cnAdminDb & "..ITEMTAG AS T"
            strSql += vbCrLf + " WHERE T.TAGNO = '" & txt_TagNo.Text & "' AND ISNULL(T.ISSDATE,'')='' AND ISNULL(APPROVAL,'')<>'A'"
            If itemId <> "" Then
                strSql += vbCrLf + " AND T.ITEMID = " & Val(itemId) & ""
            End If
            If cnCostId <> "" Then
                strSql += vbCrLf + " AND T.COSTID = '" & cnCostId & "'"
            End If
            If Not cnCentStock Then strSql += vbCrLf + " AND T.COMPANYID = '" & GetStockCompId() & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtTagDet)

            dtTagStnDet.Rows.Clear()
            strSql = vbCrLf + " SELECT SNO,IM.ITEMNAME AS STNITEMNAME,SM.SUBITEMNAME AS STNSUBITEMNAME"
            strSql += vbCrLf + " ,STNITEMID,TAGNO,STNPCS,STNWT,T.STONEUNIT AS UNIT"
            strSql += vbCrLf + " ,CALCMODE AS CAL,STNRATE,STNAMT,IM.METALID "
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAGSTONE T"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID=T.STNITEMID "
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SM ON SM.ITEMID=T.STNITEMID "
            strSql += vbCrLf + " AND SM.SUBITEMID=T.STNSUBITEMID "
            strSql += vbCrLf + " WHERE TAGNO='" & txt_TagNo.Text & "' AND ISNULL(T.ISSDATE,'')='' "
            If itemId <> "" Then
                strSql += vbCrLf + " AND T.ITEMID = " & Val(itemId) & ""
            End If
            If cnCostId <> "" Then
                strSql += vbCrLf + " AND T.COSTID = '" & cnCostId & "'"
            End If
            If Not cnCentStock Then strSql += vbCrLf + " AND T.COMPANYID = '" & GetStockCompId() & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtTagStnDet)

            If dtTagDet.Rows.Count > 0 Then
                If dtTagDet.Rows.Count = 1 Then
                    gridViewTAG.DataSource = dtTagDet
                    CalcGrsNet = dtTagDet.Rows(0).Item("GRSNET").ToString
                    gridformat()
                    txt_TagNo.Enabled = False
                    FormatGridColumns(gridViewTAG, False, False, , False)
                    'CalcGridtagTotal()
                End If
                If dtTagStnDet.Rows.Count > 0 Then
                    'gridViewTagStone.Visible = True
                    'TabControlstone.Visible = True
                    gridViewTagStone.DataSource = dtTagStnDet
                    gridSTNformat()
                    FormatGridColumns(gridViewTagStone, False, False, , False)
                End If
                'If gridViewTagStone.Visible = True Then
                '    'ChkAutoStudSP.Focus()
                '    Me.SelectNextControl(ChkAutoSPlit, False, True, True, True)
                'Else
                '    Label67.Focus()
                'End If
                Me.SelectNextControl(ChkAutoSPlit, False, True, True, True)
            Else
                MsgBox("TagNo Not Found", MsgBoxStyle.Information)
            End If
        End If
    End Sub

    Private Sub txtSplitRatio_NUM_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtSplitRatio_NUM.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If ChkRatio.Checked = True And Val(txtSpiltTag_NUM.Text) > 0 And addAutowt = True Then
                addAutowt = False
                txtSplitRatio_NUM.Enabled = False
                txtSpiltTag_NUM.Enabled = False
                Dim caltype As String = objGPack.GetSqlValue("SELECT CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & dtTagDet.Rows(0).Item("ITEM").ToString & "'", , "W")
                Dim SNO As String = ""
                Dim Recdate As String = ""
                Dim SplPer As Integer = 0
                Dim Ratio(Val(txtSpiltTag_NUM.Text) - 1) As Integer
                Dim CalcRatio As Integer = 0
                For R As Integer = 0 To Ratio.Length - 1
                    If R = 0 Then
                        Ratio(0) = Val(txtSplitRatio_NUM.Text)
                    Else
                        Ratio(R) = (100 - Val(txtSplitRatio_NUM.Text)) / (Val(txtSpiltTag_NUM.Text) - 1)
                    End If
                    CalcRatio += Ratio(R)
                Next
                If CalcRatio <> 100 Then MsgBox("Ratio not Set Properly", MsgBoxStyle.Information) : Exit Sub
                strSql = "SELECT LOTSNO,RECDATE FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO='" & txt_TagNo.Text & "' AND ITEMID=" & Val(txtItemId.Text)
                Dim dr As DataRow
                dr = GetSqlRow(strSql, cn, tran)
                If Not dr Is Nothing Then
                    SNO = dr("LOTSNO").ToString
                    Recdate = dr("RECDATE").ToString
                End If
                Dim _TagPcs As Integer = Val(dtTagDet.Rows(0).Item("PCS").ToString)
                For k As Integer = 0 To Val(txtSpiltTag_NUM.Text) - 1
                    Dim pstnpcs As Integer = 0
                    Dim pstnwt As Decimal = 0
                    If k = 0 Then SplPer = Ratio(0) Else SplPer = Ratio(k)
                    TAGNO = TAGNO + 1
                    Dim Tag As String

                    If MANUAL_TAGNO Then
                        Dim Ob = New ManualTagNo
                        If Ob.ShowDialog() = DialogResult.OK Then
                            Tag = Ob.ReturnValue
                        Else
                            addAutowt = True
                            Exit Sub
                        End If
                    Else
                        Tag = objTag.GetTagNo(Recdate, dtTagDet.Rows(0).Item("ITEM").ToString, SNO)
                    End If
                    If Tag.Length > 0 And Prefix <> "" Then
                        Tag = Tag.Replace(Prefix, "")
                    End If
                    'Tag += Val(k.ToString)
                    If objTag._TagNoGen = "T" Or objTag._TagNoGen = "D" Then
                        Tag += Val(k.ToString).ToString
                    Else
                        Tag += Val(k.ToString)
                    End If
                    If Tag.Contains(tagprefix) = False And tagprefix <> "" Then
                        Tag = tagprefix & Tag
                    End If
                    'If GetAdmindbSoftValue("TAGNOFROM") = "I" Then ''FROM ITEMMAST OR UNIQUE
                    '    If GetAdmindbSoftValue("TAGNOGEN") = "I" Then ''FROM ITEM
                    '        strSql = " UPDATE " & cnAdminDb & "..ITEMMAST SET CURRENTTAGNO = '" & Tag & "' WHERE ITEMNAME = '" & dtTagDet.Rows(0).Item("ITEM").ToString & "'"
                    '        cmd = New OleDbCommand(strSql, cn, tran)
                    '        cmd.ExecuteNonQuery()
                    '    Else
                    '        strSql = " UPDATE " & cnAdminDb & "..ITEMLOT SET CURTAGNO = '" & Tag & "'"
                    '        cmd = New OleDbCommand(strSql, cn, tran)
                    '        cmd.ExecuteNonQuery()
                    '    End If
                    'ElseIf GetAdmindbSoftValue("TAGNOFROM") = "U" Then  'UNIQUE
                    '    strSql = " UPDATE " & cnAdminDb & "..SOFTCONTROL SET CTLTEXT  = '" & Tag & "' WHERE CTLID = 'LASTTAGNO'"
                    '    cmd = New OleDbCommand(strSql, cn, tran)
                    '    cmd.ExecuteNonQuery()
                    'End If
                    If (Val(dtTagDet.Rows(0).Item("STNWT").ToString) - StnWt) > 0 Then
                        For m As Integer = 0 To dtTagStnDet.Rows.Count - 1
                            Dim spcs As Integer = 0
                            Dim swt As Decimal = 0
                            Dim samt As Double = 0
                            If Val(dtTagStnDet.Rows(m).Item("STNPCS").ToString) > 1 Then
                                spcs = ((Val(dtTagStnDet.Rows(m).Item("STNPCS").ToString) / 100) * SplPer)
                            Else
                                spcs = IIf(k = 0, Val(dtTagStnDet.Rows(m).Item("STNPCS").ToString), 0)
                            End If

                            swt = Math.Round(((Val(dtTagStnDet.Rows(m).Item("STNWT").ToString) / 100) * SplPer), 3)
                            samt = Math.Round(((Val(dtTagStnDet.Rows(m).Item("STNAMT").ToString) / 100) * SplPer), 2)

                            StnPcs += spcs
                            StnWt += swt

                            pstnpcs += spcs
                            pstnwt += swt

                            Dim row As DataRow = Nothing
                            row = dtTagSplitStoneDet.NewRow
                            row("STNITEM") = dtTagStnDet.Rows(m).Item("STNITEMNAME").ToString
                            row("STNSUBITEM") = dtTagStnDet.Rows(m).Item("STNSUBITEMNAME").ToString
                            If TagSplitNoGen_Format2 Then
                                row("STNTAGNO") = tagprefix & TAGNO & dtTagDet.Rows(0).Item("TAGNO").ToString
                            Else
                                row("STNTAGNO") = Tag
                            End If
                            row("STNPCS") = IIf(spcs <> 0, spcs, DBNull.Value)
                            row("STNWT") = IIf(swt <> 0, Format(swt, FormatNumberStyle(3)), DBNull.Value)
                            row("STNCAL") = dtTagStnDet.Rows(m).Item("CAL").ToString
                            row("STNUNIT") = dtTagStnDet.Rows(m).Item("UNIT").ToString
                            row("METALID") = dtTagStnDet.Rows(m).Item("METALID").ToString
                            row("STNRATE") = IIf(Val(dtTagStnDet.Rows(m).Item("STNRATE").ToString) <> 0, Val(dtTagStnDet.Rows(m).Item("STNRATE").ToString), DBNull.Value)
                            row("STNAMT") = IIf(samt <> 0, Format(samt, FormatNumberStyle(3)), DBNull.Value)
                            dtTagSplitStoneDet.Rows.Add(row)
                            dtTagSplitStoneDet.AcceptChanges()
                            gridTagStone.CurrentCell = gridTagStone.Rows(gridTagStone.RowCount - 1).Cells(0)
                            CalcGridtagStoneTotal()
                        Next
                    End If
                    gridTagStone.Refresh()
                    Dim ppcs As Integer = 0
                    Dim pgrswt As Double = 0
                    Dim pnetwt As Double = 0
                    Dim plesswt As Double = 0

                    ppcs = IIf(_TagPcs > 0, 1, 0)
                    _TagPcs -= 1
                    If TagSplitZeroPcs = False Then
                        If ppcs = 0 Then ppcs = 1
                    End If
                    pgrswt = Math.Round(((Val(dtTagDet.Rows(0).Item("GRSWT").ToString) / 100) * SplPer), 3)
                    pnetwt = Math.Round(((Val(dtTagDet.Rows(0).Item("NETWT").ToString) / 100) * SplPer), 3)
                    plesswt = Math.Round(((Val(dtTagDet.Rows(0).Item("LESSWT").ToString) / 100) * SplPer), 3)

                    Pcs += ppcs
                    GrsWt += pgrswt
                    NetWt += pnetwt
                    LessWt += plesswt
                    If k = Val(txtSpiltTag_NUM.Text) - 1 Then
                        If Math.Round(Val(dtTagDet.Rows(0).Item("LESSWT").ToString), 3) <> LessWt Or Math.Round(Val(dtTagDet.Rows(0).Item("NETWT").ToString), 3) <> NetWt Then
                            Dim diffLesswt As Double = Val(dtTagDet.Rows(0).Item("LESSWT").ToString) - LessWt
                            Dim diffNetwt As Double = Val(dtTagDet.Rows(0).Item("NETWT").ToString) - NetWt
                            Dim diffGrswt As Double = Val(dtTagDet.Rows(0).Item("GRSWT").ToString) - GrsWt
                            If Math.Abs(Math.Round(diffLesswt, 3)) = Math.Abs(Math.Round(diffNetwt, 3)) Then
                                If diffNetwt < 0 Then
                                    GrsWt -= Math.Abs(Math.Round(diffGrswt, 3))
                                    NetWt -= Math.Abs(Math.Round(diffNetwt, 3))
                                    pnetwt -= Math.Abs(Math.Round(diffNetwt, 3))
                                    LessWt += Math.Abs(Math.Round(diffNetwt, 3))
                                    plesswt += Math.Abs(Math.Round(diffNetwt, 3))
                                Else
                                    GrsWt += Math.Abs(Math.Round(diffGrswt, 3))
                                    NetWt += Math.Abs(Math.Round(diffNetwt, 3))
                                    pnetwt += Math.Abs(Math.Round(diffNetwt, 3))
                                    LessWt -= Math.Abs(Math.Round(diffNetwt, 3))
                                    plesswt -= Math.Abs(Math.Round(diffNetwt, 3))
                                End If
                            End If
                            If diffGrswt < 0 Then
                                pgrswt -= Math.Abs(Math.Round(diffGrswt, 3))
                            Else
                                pgrswt += Math.Abs(Math.Round(diffGrswt, 3))
                            End If
                            If diffNetwt < 0 Then
                                pnetwt -= Math.Abs(Math.Round(diffNetwt, 3))
                            Else
                                pnetwt += Math.Abs(Math.Round(diffNetwt, 3))
                            End If
                            If diffLesswt < 0 Then
                                plesswt -= Math.Abs(Math.Round(diffLesswt, 3))
                            Else
                                plesswt += Math.Abs(Math.Round(diffLesswt, 3))
                            End If
                        End If
                    End If

                    Dim ro As DataRow = Nothing
                    ro = dtTagSplitDetails.NewRow
                    ro("ITEM") = dtTagDet.Rows(0).Item("ITEM").ToString
                    ro("SUBITEM") = dtTagDet.Rows(0).Item("SUBITEM").ToString
                    ro("TAGNO") = IIf(TagSplitNoGen_Format2, tagprefix & TAGNO & dtTagDet.Rows(0).Item("TAGNO").ToString, Tag)
                    'ro("TAGNO") = Tag
                    ro("PCS") = IIf(ppcs <> 0, ppcs, DBNull.Value)
                    ro("GRSWT") = IIf(pgrswt <> 0, Format(pgrswt, "0.000"), DBNull.Value)
                    ro("NETWT") = IIf(pnetwt <> 0, Format(pnetwt, "0.000"), DBNull.Value)
                    ro("LESSWT") = IIf(plesswt <> 0, Format(plesswt, "0.000"), DBNull.Value)
                    ro("STNPCS") = IIf(pstnpcs <> 0, Format(pstnpcs, "0.000"), DBNull.Value)
                    ro("STNWT") = IIf(pstnwt <> 0, Format(pstnwt, FormatNumberStyle(3)), DBNull.Value)
                    dtTagSplitDetails.Rows.Add(ro)
                    dtTagSplitDetails.AcceptChanges()
                    gridMultiTag.CurrentCell = gridMultiTag.Rows(gridMultiTag.RowCount - 1).Cells(1)
                    CalcGridtagTotal()
                Next
                gridMultiTag.Refresh()
                btnSave.Enabled = True
                If caltype = "R" Then
                    If (Val(dtTagDet.Rows(0).Item("PCS").ToString) - Pcs) <= 0 Then
                        btnSave.Focus()
                        Exit Sub
                    End If
                Else
                    If (Val(dtTagDet.Rows(0).Item("GRSWT").ToString) - Format(GrsWt, "0.000")) <= 0 Then
                        btnSave.Focus()
                        Exit Sub
                    End If
                End If
            Else
                lblItemName.Focus()
            End If
        End If
    End Sub

    Private Sub txtStnSpPcs_NUM_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtSplitPcs_NUM.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If Val(txtItemId.Text.ToString) <= 0 Then
                MsgBox("Item Id Should not be empty.", MsgBoxStyle.Information)
                txtItemId.Focus()
                Exit Sub
            End If
            If Val(txtSplitPcs_NUM.Text) > 0 Then
                If Val(txtSplitPcs_NUM.Text) > 2 Then
                    MsgBox("Given PCS not able to split.", MsgBoxStyle.Information)
                    txtSplitPcs_NUM.Focus()
                    Exit Sub
                End If
                If ChkAutoSPlit.Checked = True And Val(txtSplitPcs_NUM.Text) > 0 And addAutowt = True Then
                    Dim SplitPer() As String = TAGSPLITPER.Split(":")
                    Dim Itemper1 As Integer = 0
                    Dim Itemper2 As Integer = 0
                    If SplitPer.Length > 1 Then
                        Itemper1 = Val(SplitPer(0).ToString)
                        Itemper2 = Val(SplitPer(1).ToString)
                    End If
                    If Itemper1 = 0 Or Itemper2 = 0 Or (Itemper1 + Itemper2) <> 100 Then Itemper1 = 50 : Itemper2 = 50

                    addAutowt = False

                    Dim caltype As String = objGPack.GetSqlValue("SELECT CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & dtTagDet.Rows(0).Item("ITEM").ToString & "'", , "W")
                    Dim SplPer As Integer = 0
                    Dim SNO As String = ""
                    Dim Recdate As String
                    strSql = "SELECT LOTSNO,RECDATE FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO='" & txt_TagNo.Text & "' AND ITEMID=" & Val(txtItemId.Text)
                    Dim dr As DataRow
                    dr = GetSqlRow(strSql, cn, tran)
                    If Not dr Is Nothing Then
                        SNO = dr("LOTSNO").ToString
                        Recdate = dr("RECDATE").ToString
                    End If
                    For k As Integer = 0 To Val(txtSplitPcs_NUM.Text) - 1
                        Dim pstnpcs As Integer = 0
                        Dim pstnwt As Decimal = 0
                        If k = 0 Then SplPer = Itemper1 Else SplPer = Itemper2
                        TAGNO = TAGNO + 1
                        Dim Tag As String

                        If MANUAL_TAGNO Then
                            Dim Ob = New ManualTagNo
                            If Ob.ShowDialog() = DialogResult.OK Then
                                Tag = Ob.ReturnValue
                            Else
                                addAutowt = True
                                Exit Sub
                            End If
                        Else
                            Tag = objTag.GetTagNo(Recdate, dtTagDet.Rows(0).Item("ITEM").ToString, SNO)
                        End If

                        Dim str As String = ""
                        Dim fPart As String = ""
                        Dim sPart As String = ""
                        If IsNumeric(Tag) And TagPrefix_Item = False Then
                            'If True Then Tag = Val(Tag) + 1 Else Tag = Val(Tag) - 1
                        ElseIf TagPrefix_Item = False Then
                            For Each c As Char In Tag
                                If IsNumeric(c) Then
                                    sPart += c
                                Else
                                    fPart += c
                                End If
                            Next
                            'If increament Then TAGNO = fPart + (Val(sPart) + 1).ToString Else TAGNO = fPart + (IIf(Val(sPart) - 1 > 0, Val(sPart) - 1, 0)).ToString
                        End If
                        If fPart.ToString <> "" Then
                            Prefix = fPart.ToString
                            tagprefix = fPart.ToString
                        Else
                            tagprefix = GetAdmindbSoftValue("TAGSPLITPREFIX")
                            Prefix = GetAdmindbSoftValue("TAGPREFIX")
                        End If
                        If TagPrefix_Item = True Then
                            strSql = " SELECT SUBSTRING(SHORTNAME,1,5)SHORTNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & Val(txtItemId.Text)
                            tagprefix = objGPack.GetSqlValue(strSql, , "")
                            Prefix = objGPack.GetSqlValue(strSql, , "")
                        End If
                        If Tag.Length > 0 And Prefix <> "" Then
                            Tag = Tag.Replace(Prefix, "")
                        End If
                        Tag += k
                        If Tag.Contains(tagprefix) = False And tagprefix <> "" Then
                            Tag = tagprefix & Tag
                        End If

                        If (Val(dtTagDet.Rows(0).Item("STNWT").ToString) - StnWt) > 0 Then
                            For m As Integer = 0 To dtTagStnDet.Rows.Count - 1
                                'Dim spcs As Integer = (Val(dtTagStnDet.Rows(m).Item("STNPCS").ToString) / Val(txtSplitPcs_NUM.Text))
                                'Dim swt As Decimal = Math.Round((Val(dtTagStnDet.Rows(m).Item("STNWT").ToString) / Val(txtSplitPcs_NUM.Text)), 3)
                                'Dim samt As Double = Math.Round((Val(dtTagStnDet.Rows(m).Item("STNAMT").ToString) / Val(txtSplitPcs_NUM.Text)), 2)
                                Dim spcs As Integer = 0
                                Dim swt As Decimal = 0
                                Dim samt As Double = 0
                                If Val(dtTagStnDet.Rows(m).Item("STNPCS").ToString) > 1 Then
                                    spcs = ((Val(dtTagStnDet.Rows(m).Item("STNPCS").ToString) / 100) * SplPer)
                                Else
                                    spcs = IIf(k = 0, Val(dtTagStnDet.Rows(m).Item("STNPCS").ToString), 0)
                                End If
                                swt = Math.Round(((Val(dtTagStnDet.Rows(m).Item("STNWT").ToString) / 100) * SplPer), 3)
                                samt = Math.Round(((Val(dtTagStnDet.Rows(m).Item("STNAMT").ToString) / 100) * SplPer), 2)
                                StnPcs += spcs
                                StnWt += swt
                                pstnpcs += spcs
                                pstnwt += swt
                                Dim row As DataRow = Nothing
                                row = dtTagSplitStoneDet.NewRow
                                row("STNITEM") = dtTagStnDet.Rows(m).Item("STNITEMNAME").ToString
                                row("STNSUBITEM") = dtTagStnDet.Rows(m).Item("STNSUBITEMNAME").ToString
                                row("STNTAGNO") = IIf(TagSplitNoGen_Format2, tagprefix & TAGNO & dtTagDet.Rows(0).Item("TAGNO").ToString, Tag)
                                row("STNPCS") = IIf(spcs <> 0, spcs, DBNull.Value)
                                row("STNWT") = IIf(swt <> 0, Format(swt, FormatNumberStyle(3)), DBNull.Value)
                                row("STNCAL") = dtTagStnDet.Rows(m).Item("CAL").ToString
                                row("STNUNIT") = dtTagStnDet.Rows(m).Item("UNIT").ToString
                                row("METALID") = dtTagStnDet.Rows(m).Item("METALID").ToString
                                row("STNRATE") = IIf(Val(dtTagStnDet.Rows(m).Item("STNRATE").ToString) <> 0, Val(dtTagStnDet.Rows(m).Item("STNRATE").ToString), DBNull.Value)
                                row("STNAMT") = IIf(samt <> 0, Format(samt, FormatNumberStyle(3)), DBNull.Value)
                                dtTagSplitStoneDet.Rows.Add(row)
                                dtTagSplitStoneDet.AcceptChanges()
                                gridTagStone.CurrentCell = gridTagStone.Rows(gridTagStone.RowCount - 1).Cells(0)
                                CalcGridtagStoneTotal()
                            Next
                        End If
                        ''Insertion
                        'Dim ppcs As Integer = IIf(k = 0, Val(dtTagDet.Rows(0).Item("PCS").ToString), 0)
                        'Dim pgrswt As Decimal = Math.Round((Val(dtTagDet.Rows(0).Item("GRSWT").ToString) / Val(txtSplitPcs_NUM.Text)), 3)
                        'Dim pnetwt As Decimal = Math.Round((Val(dtTagDet.Rows(0).Item("NETWT").ToString) / Val(txtSplitPcs_NUM.Text)), 3)
                        'Dim plesswt As Decimal = Math.Round((Val(dtTagDet.Rows(0).Item("LESSWT").ToString) / Val(txtSplitPcs_NUM.Text)), 3)

                        Dim ppcs As Integer = 0
                        Dim pgrswt As Decimal = 0
                        Dim pnetwt As Decimal = 0
                        Dim plesswt As Decimal = 0
                        Dim SALVALUE As Decimal = 0

                        ppcs = IIf(k = 0, Val(dtTagDet.Rows(0).Item("PCS").ToString), 0)
                        pgrswt = Math.Round(((Val(dtTagDet.Rows(0).Item("GRSWT").ToString) / 100) * SplPer), 3)
                        pnetwt = Math.Round(((Val(dtTagDet.Rows(0).Item("NETWT").ToString) / 100) * SplPer), 3)
                        plesswt = Math.Round(((Val(dtTagDet.Rows(0).Item("LESSWT").ToString) / 100) * SplPer), 3)
                        SALVALUE = Math.Round(((Val(dtTagDet.Rows(0).Item("SALVALUE").ToString) / 100) * SplPer), 3)

                        If ppcs = 0 And TagSplitZeroPcs = False Then
                            ppcs = 1
                        End If
                        Pcs += ppcs
                        GrsWt += pgrswt
                        NetWt += pnetwt
                        LessWt += plesswt

                        If k = Val(txtSplitPcs_NUM.Text) - 1 Then
                            If Math.Round(Val(dtTagDet.Rows(0).Item("LESSWT").ToString), 3) <> LessWt Or Math.Round(Val(dtTagDet.Rows(0).Item("NETWT").ToString), 3) <> NetWt Then
                                Dim diffLesswt As Double = Val(dtTagDet.Rows(0).Item("LESSWT").ToString) - LessWt
                                Dim diffNetwt As Double = Val(dtTagDet.Rows(0).Item("NETWT").ToString) - NetWt
                                Dim diffGrswt As Double = Val(dtTagDet.Rows(0).Item("GRSWT").ToString) - GrsWt
                                If Math.Abs(Math.Round(diffLesswt, 3)) = Math.Abs(Math.Round(diffNetwt, 3)) Then
                                    If diffNetwt < 0 Then
                                        GrsWt -= Math.Abs(Math.Round(diffGrswt, 3))
                                        NetWt -= Math.Abs(Math.Round(diffNetwt, 3))
                                        pnetwt -= Math.Abs(Math.Round(diffNetwt, 3))
                                        LessWt += Math.Abs(Math.Round(diffNetwt, 3))
                                        plesswt += Math.Abs(Math.Round(diffNetwt, 3))
                                    Else
                                        GrsWt += Math.Abs(Math.Round(diffGrswt, 3))
                                        NetWt += Math.Abs(Math.Round(diffNetwt, 3))
                                        pnetwt += Math.Abs(Math.Round(diffNetwt, 3))
                                        LessWt -= Math.Abs(Math.Round(diffNetwt, 3))
                                        plesswt -= Math.Abs(Math.Round(diffNetwt, 3))
                                    End If
                                End If
                                If diffGrswt < 0 Then
                                    pgrswt -= Math.Abs(Math.Round(diffGrswt, 3))
                                Else
                                    pgrswt += Math.Abs(Math.Round(diffGrswt, 3))
                                End If
                                If diffNetwt < 0 Then
                                    pnetwt -= Math.Abs(Math.Round(diffNetwt, 3))
                                Else
                                    pnetwt += Math.Abs(Math.Round(diffNetwt, 3))
                                End If
                                If diffLesswt < 0 Then
                                    plesswt -= Math.Abs(Math.Round(diffLesswt, 3))
                                Else
                                    plesswt += Math.Abs(Math.Round(diffLesswt, 3))
                                End If
                            End If
                        End If
                        Dim ro As DataRow = Nothing
                        ro = dtTagSplitDetails.NewRow
                        ro("ITEM") = dtTagDet.Rows(0).Item("ITEM").ToString
                        ro("SUBITEM") = dtTagDet.Rows(0).Item("SUBITEM").ToString
                        ro("TAGNO") = IIf(TagSplitNoGen_Format2, tagprefix & TAGNO & dtTagDet.Rows(0).Item("TAGNO").ToString, Tag)
                        ro("PCS") = IIf(ppcs <> 0, ppcs, DBNull.Value)
                        ro("GRSWT") = IIf(pgrswt <> 0, Format(pgrswt, FormatNumberStyle(3)), DBNull.Value)
                        ro("NETWT") = IIf(pnetwt <> 0, Format(pnetwt, FormatNumberStyle(3)), DBNull.Value)
                        ro("LESSWT") = IIf(plesswt <> 0, Format(plesswt, FormatNumberStyle(3)), DBNull.Value)
                        ro("STNPCS") = IIf(pstnpcs <> 0, Format(pstnpcs, FormatNumberStyle(3)), DBNull.Value)
                        ro("STNWT") = IIf(pstnwt <> 0, Format(pstnwt, FormatNumberStyle(3)), DBNull.Value)
                        ro("SALVALUE") = IIf(SALVALUE <> 0, Format(SALVALUE, FormatNumberStyle(3)), DBNull.Value)
                        dtTagSplitDetails.Rows.Add(ro)
                        dtTagSplitDetails.AcceptChanges()
                        dtTagSplitDetails.Columns("TAGNO").ReadOnly = MANUAL_TAGNO
                        gridMultiTag.CurrentCell = gridMultiTag.Rows(gridMultiTag.RowCount - 1).Cells(1)

                        CalcGridtagTotal()
                    Next
                    btnSave.Enabled = True
                    If caltype = "R" Then
                        If (Val(dtTagDet.Rows(0).Item("PCS").ToString) - Pcs) <= 0 Then
                            btnSave.Focus()
                            'Me.SelectNextControl(btnSave, True, True, True, True)
                            Exit Sub
                        End If
                    Else
                        If (Val(dtTagDet.Rows(0).Item("GRSWT").ToString) - Format(GrsWt, "0.000")) <= 0 Then
                            'Me.SelectNextControl(btnSave, True, True, True, True)
                            btnSave.Focus()
                            Exit Sub
                        End If
                    End If
                End If
            Else
                lblItemName.Focus()
            End If
        End If

        If gridMultiTag.Columns.Contains("SALVALUE") Then
            gridMultiTag.Columns("SALVALUE").Visible = False
        End If
    End Sub

    Private Function GenItemTagNo(ByRef TagNo As String, Optional ByVal increament As Boolean = True) As String
        Dim str As String = Nothing
        Dim fPart As String = Nothing
        Dim sPart As String = Nothing
        If IsNumeric(TagNo) Then
            If increament Then TagNo = Val(TagNo) + 1 Else TagNo = Val(TagNo) - 1
        Else
            For Each c As Char In TagNo
                If IsNumeric(c) Then
                    sPart += c
                Else
                    fPart += c
                End If
            Next
            If increament Then TagNo = fPart + (Val(sPart) + 1).ToString Else TagNo = fPart + (IIf(Val(sPart) - 1 > 0, Val(sPart) - 1, 0)).ToString
        End If
        Return tagprefix + TagNo
    End Function

    Private Sub gridMultiTag_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridMultiTag.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            gridMultiTag.CurrentCell = gridMultiTag.Rows(gridMultiTag.CurrentRow.Index).Cells(1)
        End If
    End Sub
    Private Sub gridMultiTag_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridMultiTag.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            With gridMultiTag.Rows(gridMultiTag.CurrentRow.Index)
                txtITEM.Text = .Cells("ITEM").FormattedValue
                txtSUBITEm.Text = .Cells("SUBITEM").FormattedValue
                txtTAGNO.Text = .Cells("TAGNO").FormattedValue
                txtPCS_Num.Text = .Cells("PCS").FormattedValue
                txtGRSWT_Wet.Text = .Cells("GRSWT").FormattedValue
                txtNETWT_Wet.Text = .Cells("NETWT").FormattedValue
                txtLessWt_Wet.Text = .Cells("LESSWT").FormattedValue
                txtStnpcs_Num.Text = .Cells("STNPCS").FormattedValue
                txtStnWt_Wet.Text = .Cells("STNWT").FormattedValue
                txtStRowIndex.Text = gridMultiTag.CurrentRow.Index
                txtITEM.Focus()
                txtITEM.SelectAll()
            End With
        End If
    End Sub

    Private Sub txtSTnWT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtStnWt_Wet.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            ''Validation

            If objGPack.DupCheck("SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtITEM.Text & "' AND ACTIVE = 'Y'") = False Then
                MsgBox("Invalid Item", MsgBoxStyle.Information)
                txtITEM.Focus()
                Exit Sub
            End If
            If UCase(objGPack.GetSqlValue("SELECT SUBITEM FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtSUBITEm.Text & "' AND ACTIVE = 'Y'")) = "Y" Then
                If txtSUBITEm.Text = "" Then
                    MsgBox("SubItem Should Not Empty", MsgBoxStyle.Information)
                    txtSUBITEm.Focus()
                    Exit Sub
                End If
                If objGPack.DupCheck("SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtITEM.Text & "') AND SUBITEMNAME = '" & txtSUBITEm.Text & "'") = False Then
                    MsgBox("Invalid SubItem", MsgBoxStyle.Information)
                    txtSUBITEm.Focus()
                    Exit Sub
                End If
            Else
                ' txtSUBITEm.Clear()
            End If
            Dim caltype As String = objGPack.GetSqlValue("SELECT CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtITEM.Text & "'", , "W")
            If Val(txtPCS_Num.Text) = 0 And Val(txtGRSWT_Wet.Text) = 0 And Val(txtNETWT_Wet.Text) = 0 Then
                MsgBox(Me.GetNextControl(txtPCS_Num, False).Text + "," + Me.GetNextControl(txtGRSWT_Wet, False).Text + E0001, MsgBoxStyle.Information)
                txtITEM.Focus()
                Exit Sub
            End If
            Dim rate As Double = Val(objGPack.GetSqlValue(strSql, , , tran))

            If txtStRowIndex.Text <> "" Then
                'If MessageBox.Show("Would you like to update this Entry", "Update Alert", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.OK Then
                With gridMultiTag.Rows(Val(txtStRowIndex.Text))
                    .Cells("ITEM").Value = txtITEM.Text
                    .Cells("SUBITEM").Value = txtSUBITEm.Text
                    .Cells("TAGNO").Value = txtTAGNO.Text
                    .Cells("PCS").Value = IIf(Val(txtPCS_Num.Text) <> 0, Val(txtPCS_Num.Text), DBNull.Value)
                    .Cells("GRSWT").Value = IIf(Val(txtGRSWT_Wet.Text) <> 0, Format(Val(txtGRSWT_Wet.Text), "0.000"), DBNull.Value)
                    .Cells("NETWT").Value = IIf(Val(txtNETWT_Wet.Text) <> 0, Format(Val(txtNETWT_Wet.Text), "0.000"), DBNull.Value)
                    .Cells("LESSWT").Value = IIf(Val(txtLessWt_Wet.Text) <> 0, Format(Val(txtLessWt_Wet.Text), "0.000"), DBNull.Value)
                    .Cells("STNPCS").Value = IIf(Val(txtStnpcs_Num.Text) <> 0, Val(txtStnpcs_Num.Text), DBNull.Value)
                    .Cells("STNWT").Value = IIf(Val(txtStnWt_Wet.Text) <> 0, Format(Val(txtStnWt_Wet.Text), FormatNumberStyle(3)), DBNull.Value)
                    dtTagSplitDetails.AcceptChanges()
                    CalcGridtagTotal()
                    GoTo AFTERINSERT
                End With
                'End If
            End If
            ''Insertion
            Pcs += Val(txtPCS_Num.Text)
            GrsWt += Val(txtGRSWT_Wet.Text)
            NetWt += Val(txtNETWT_Wet.Text)
            LessWt += Val(txtLessWt_Wet.Text)

            Dim ro As DataRow = Nothing
            ro = dtTagSplitDetails.NewRow
            ro("ITEM") = txtITEM.Text
            ro("SUBITEM") = txtSUBITEm.Text
            ro("TAGNO") = txtTAGNO.Text
            ro("PCS") = IIf(Val(txtPCS_Num.Text) <> 0, Val(txtPCS_Num.Text), DBNull.Value)
            ro("GRSWT") = IIf(Val(txtGRSWT_Wet.Text) <> 0, Format(Val(txtGRSWT_Wet.Text), "0.000"), DBNull.Value)
            ro("NETWT") = IIf(Val(txtNETWT_Wet.Text) <> 0, Format(Val(txtNETWT_Wet.Text), "0.000"), DBNull.Value)
            ro("LESSWT") = IIf(Val(txtLessWt_Wet.Text) <> 0, Format(Val(txtLessWt_Wet.Text), "0.000"), DBNull.Value)
            ro("STNPCS") = IIf(Val(txtStnpcs_Num.Text) <> 0, Val(txtStnpcs_Num.Text), DBNull.Value)
            ro("STNWT") = IIf(Val(txtStnWt_Wet.Text) <> 0, Format(Val(txtStnWt_Wet.Text), FormatNumberStyle(3)), DBNull.Value)
            dtTagSplitDetails.Rows.Add(ro)
            dtTagSplitDetails.AcceptChanges()
            gridMultiTag.CurrentCell = gridMultiTag.Rows(gridMultiTag.RowCount - 1).Cells(1)
            CalcGridtagTotal()
            txtPCS_Num.Text = ""
            txtGRSWT_Wet.Text = ""
            txtNETWT_Wet.Text = ""
            txtLessWt_Wet.Text = ""
            txtStnWt_Wet.Text = ""
            txtStnpcs_Num.Text = ""
            txtTAGNO.Text = ""
            txtITEM.Focus()
            splittagno = True
            If caltype = "R" Then
                If (Val(dtTagDet.Rows(0).Item("PCS").ToString) - Pcs) <= 0 Then
                    txtITEM.Clear()
                    txtSUBITEm.Clear()
                    btnSave.Enabled = True
                    btnSave.Focus()
                    Exit Sub
                End If
            Else
                If (Val(dtTagDet.Rows(0).Item("GRSWT").ToString) - Format(GrsWt, "0.000")) <= 0 Then
                    txtITEM.Clear()
                    txtSUBITEm.Clear()
                    btnSave.Enabled = True
                    btnSave.Focus()
                    Exit Sub
                End If
            End If
AFTERINSERT:
            txtITEM.Clear()
            txtSUBITEm.Clear()
            txtPCS_Num.Clear()
            txtGRSWT_Wet.Clear()
            txtTAGNO.Clear()
            txtNETWT_Wet.Clear()
            txtNETWT_Wet.Clear()
            txtStRowIndex.Text = ""
            txtStnWt_Wet.Clear()
            txtStnpcs_Num.Clear()
            'txtStItem.Focus()
        End If
    End Sub
    Private Sub txtStAmount_Amt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtStAmount_Amt.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If objGPack.DupCheck("SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "' AND ACTIVE = 'Y'") = False Then
                MsgBox("Invalid Item", MsgBoxStyle.Information)
                txtStItem.Focus()
                Exit Sub
            End If
            If UCase(objGPack.GetSqlValue("SELECT SUBITEM FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStSubItem.Text & "' AND ACTIVE = 'Y'")) = "Y" Then
                If txtSUBITEm.Text = "" Then
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
                'txtStSubItem.Clear()
            End If
            If Val(txtStWeight_WET.Text) = 0 Then
                MsgBox(Me.GetNextControl(txtStWeight_WET, False).Text + E0001, MsgBoxStyle.Information)
                txtStItem.Focus()
                Exit Sub
            End If
            Dim stWeight As Double = IIf(cmbStUnit.Text = "C", Val(txtStWeight_WET.Text) / 5, Val(txtStWeight_WET.Text))
            For cnt As Integer = 0 To gridTagStone.RowCount - 1
                If txtStRowIndex.Text <> "" Then If Val(txtStRowIndex.Text) = cnt Then Continue For
                With gridTagStone.Rows(cnt)
                    If .Cells("STNUNIT").Value.ToString = "C" Then
                        stWeight += Val(.Cells("STNWT").Value.ToString) / 5
                    ElseIf .Cells("STNUNIT").Value.ToString = "G" Then
                        stWeight += Val(.Cells("STNWT").Value.ToString)
                    End If
                End With
            Next
            If stWeight > Val(txtGRSWT_Wet.Text) Then
                MsgBox(Me.GetNextControl(txtStWeight_WET, False).Text + E0015 + Me.GetNextControl(txtGRSWT_Wet, False).Text, MsgBoxStyle.Exclamation)
                txtStWeight_WET.Focus()
                Exit Sub
            End If
            If txtStONERowIndex.Text <> "" Then
                With gridTagStone.Rows(Val(txtStONERowIndex.Text))
                    .Cells("STNITEM").Value = txtStItem.Text
                    .Cells("STNSUBITEM").Value = txtStSubItem.Text
                    .Cells("STNTAGNO").Value = txtStTagno.Text
                    .Cells("STNPCS").Value = IIf(Val(txtStPcs_NUM.Text) <> 0, Val(txtStPcs_NUM.Text), DBNull.Value)
                    .Cells("STNWT").Value = IIf(Val(txtStWeight_WET.Text) <> 0, Format(Val(txtStWeight_WET.Text), FormatNumberStyle(3)), DBNull.Value)
                    .Cells("STNCAL").Value = cmbStCalc.Text
                    .Cells("STNUNIT").Value = cmbStUnit.Text
                    .Cells("STNRATE").Value = IIf(Val(txtStRate_Amt.Text) <> 0, Format(Val(txtStRate_Amt.Text), "0.000"), DBNull.Value)
                    .Cells("STNAMT").Value = IIf(Val(txtStAmount_Amt.Text) <> 0, Format(Val(txtStAmount_Amt.Text), "0.000"), DBNull.Value)
                    .Cells("METALID").Value = txtStMetalCode.Text
                    CalcGridtagStoneTotal()
                    dtTagSplitStoneDet.AcceptChanges()
                    GoTo AFTERINSERT
                End With
                'End If
            End If
            ''Insertion

            StnPcs += Val(txtStPcs_NUM.Text)
            StnWt += Val(txtStWeight_WET.Text)

            Dim ro As DataRow = Nothing
            ro = dtTagSplitStoneDet.NewRow
            ro("STNITEM") = txtStItem.Text
            ro("STNSUBITEM") = txtStSubItem.Text
            ro("STNTAGNO") = txtStTagno.Text
            ro("STNPCS") = IIf(Val(txtStPcs_NUM.Text) <> 0, Val(txtStPcs_NUM.Text), DBNull.Value)
            ro("STNWT") = IIf(Val(txtStWeight_WET.Text) <> 0, Format(Val(txtStWeight_WET.Text), FormatNumberStyle(3)), DBNull.Value)
            ro("STNCAL") = cmbStCalc.Text
            ro("STNUNIT") = cmbStUnit.Text
            ro("STNRATE") = IIf(Val(txtStRate_Amt.Text) <> 0, Format(Val(txtStRate_Amt.Text), "0.000"), DBNull.Value)
            ro("STNAMT") = IIf(Val(txtStAmount_Amt.Text) <> 0, Format(Val(txtStAmount_Amt.Text), "0.000"), DBNull.Value)
            ro("METALID") = txtStMetalCode.Text
            dtTagSplitStoneDet.Rows.Add(ro)
            dtTagSplitStoneDet.AcceptChanges()
            gridTagStone.CurrentCell = gridTagStone.Rows(gridTagStone.RowCount - 1).Cells(0)
            If Val(txtStPcs_NUM.Text) <> 0 Then txtStnpcs_Num.Text = Val(txtStnpcs_Num.Text) + Val(txtStPcs_NUM.Text)
            txtStnWt_Wet.Text = Val(txtStnWt_Wet.Text) + IIf(cmbStUnit.Text = "G", Val(txtStWeight_WET.Text), Val(txtStWeight_WET.Text) / 5)
AFTERINSERT:
            CalcLessWt()
            CalcFinalTotal()
            StyleGridStone(gridTagStonetotal)
            txtStItem.Clear()
            txtStSubItem.Clear()
            txtStPcs_NUM.Clear()
            txtStWeight_WET.Clear()
            txtStTagno.Clear()
            txtStRate_Amt.Clear()
            txtStAmount_Amt.Clear()
            txtStONERowIndex.Text = ""
            txtStRowIndex.Text = ""
            If (Val(dtTagDet.Rows(0).Item("STNWT").ToString) - StnWt) <= 0 Then
                txtStnWt_Wet.Focus()
                Exit Sub
            End If
            txtStItem.Focus()
        End If
    End Sub

    Private Sub LoadItemName()
        strSql = " SELECT"
        strSql += " ITEMID AS ITEMID,ITEMNAME AS ITEMNAME"
        strSql += " FROM " & cnAdminDb & "..ITEMMAST"
        strSql += " WHERE ITEMID=" & dtTagDet.Rows(0).Item("ITEMID").ToString & " AND ISNULL(ACTIVE,'Y') <> 'N'"
        Dim itemName As String = BrighttechPack.SearchDialog.Show("Find ItemName", strSql, cn, 1, 1, , txtITEM.Text)
        If itemName <> "" Then
            txtITEM.Text = itemName
            LoaditemDetails()
        Else
            txtITEM.Focus()
            txtITEM.SelectAll()
        End If
    End Sub
    Private Sub LoaditemDetails()
        If objGPack.GetSqlValue("SELECT SUBITEM FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtITEM.Text & "'") = "Y" Then
            Dim DefItem As String = txtSUBITEm.Text
            Dim itemId As Integer = Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtITEM.Text & "'"))
            If Not _SubItemOrderByName Then
                DefItem = objGPack.GetSqlValue("SELECT DISPLAYORDER FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & txtSUBITEm.Text & "' AND ITEMID = " & itemId & "")
            End If
            If TagSplitDefltItem = True Then
                strSql = "SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST  WHERE SUBITEMID IN (SELECT TOP 1 SUBITEMID FROM  " & cnAdminDb & "..ITEMTAG WHERE TAGNO = '" & txt_TagNo.Text.ToString & "')"
                txtSUBITEm.Text = BrighttechPack.SearchDialog.Show("Search SubItem", strSql, cn, 0, 0, , DefItem, , False, True)
            Else
                strSql = GetSubItemQry(New String() {"SUBITEMID ID", "DISPLAYORDER DISP", "SUBITEMNAME SUBITEM"}, itemId)
                txtSUBITEm.Text = BrighttechPack.SearchDialog.Show("Search SubItem", strSql, cn, IIf(_SubItemOrderByName, 2, 1), 2, , DefItem, , False, True)
            End If


            Me.SelectNextControl(txtITEM, True, True, True, True)
        Else
            txtSUBITEm.Clear()
            Me.SelectNextControl(txtSUBITEm, True, True, True, True)
        End If
    End Sub

    Private Sub LoadStoneItemName()
        strSql = " SELECT"
        strSql += " ITEMID AS ITEMID,ITEMNAME AS ITEMNAME"
        strSql += " FROM " & cnAdminDb & "..ITEMMAST"
        strSql += " WHERE ISNULL(STUDDED,'') IN('S','B') AND ISNULL(ACTIVE,'Y') <> 'N'"
        strSql += " AND ITEMID IN(SELECT STNITEMID FROM " & cnAdminDb & "..ITEMTAGSTONE  WHERE TAGNO='" & txt_TagNo.Text & "')"
        Dim itemName As String = BrighttechPack.SearchDialog.Show("Find ItemName", strSql, cn, 1, 1, , txtStItem.Text)
        If itemName <> "" Then
            txtStItem.Text = itemName
            LoadStoneitemDetails()
        Else
            txtStItem.Focus()
            txtStItem.SelectAll()
        End If
    End Sub

    Private Sub LoadStoneitemDetails()
        If objGPack.GetSqlValue("SELECT SUBITEM FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "'") = "Y" Then
            Dim DefItem As String = txtStSubItem.Text
            Dim itemId As Integer = Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "'"))
            If Not _SubItemOrderByName Then
                DefItem = objGPack.GetSqlValue("SELECT DISPLAYORDER FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & txtStSubItem.Text & "' AND ITEMID = " & itemId & "")
            End If
            strSql = GetSubItemQry(New String() {"SUBITEMID ID", "DISPLAYORDER DISP", "SUBITEMNAME SUBITEM"}, itemId)
            txtStSubItem.Text = BrighttechPack.SearchDialog.Show("Search SubItem", strSql, cn, IIf(_SubItemOrderByName, 2, 1), 2, , DefItem, , False, True)
            txtStSubItem.Enabled = True
            Me.SelectNextControl(txtStSubItem, True, True, True, True)
        Else
            txtStSubItem.Clear()
            txtStSubItem.Enabled = False
            Me.SelectNextControl(txtStSubItem, True, True, True, True)
        End If
        strSql = " SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "'"
        txtStMetalCode.Text = objGPack.GetSqlValue(strSql, "DIASTONE", , tran)
    End Sub

    Private Sub txtITEM_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtITEM.GotFocus
        Main.ShowHelpText("Press Insert Key to Help")
    End Sub

    Private Sub txtITEM_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtITEM.KeyDown
        If e.KeyCode = Keys.Insert Then
            LoadItemName()
        ElseIf e.KeyCode = Keys.Down Then
            If gridMultiTag.RowCount > 0 Then
                gridMultiTag.Focus()
            End If
        End If
    End Sub

    Private Sub txtITEM_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtITEM.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtITEM.Text = "" Then
                LoadItemName()
            ElseIf txtITEM.Text <> "" And objGPack.DupCheck("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtITEM.Text & "'") = False Then
                LoadItemName()
            Else
                LoaditemDetails()
            End If
        End If
    End Sub

    Private Sub txtITEM_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtITEM.LostFocus
        Main.HideHelpText()
    End Sub

    Private Sub txtSUBITEm_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSUBITEm.GotFocus
        SendKeys.Send("{TAB}")
    End Sub

    Private Sub txtTAGNO_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTAGNO.GotFocus
        If txtStRowIndex.Text <> "" Then Exit Sub
        If splittagno = True Then
            TAGNO = TAGNO + 1
            splittagno = False
        End If
        Dim _chkNewTag As Boolean = False
        If txtTAGNO.Text.ToString <> "" Then
            strSql = " SELECT 1 FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO='" & txtTAGNO.Text.ToString & "' "
            If Val(objGPack.GetSqlValue(strSql)) > 0 Then
                _chkNewTag = True
            Else
                For Each gv As DataGridViewRow In gridMultiTag.Rows
                    With gv
                        Select Case .Cells("TAGNO").Value.ToString
                            Case txt_TagNo.Text.ToString
                                _chkNewTag = True
                        End Select
                    End With
                Next
            End If
        End If
        Dim _strRecDate As String = ""
        strSql = "SELECT RECDATE,LOTSNO,(SELECT TOP 1 ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=T.ITEMID)ITEMNAME "
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T WHERE TAGNO='" & txt_TagNo.Text.ToString & "'"
        dtTempTagDet = New DataTable
        cmd = New OleDbCommand(strSql, cn)
        da = New OleDbDataAdapter(cmd)
        da.Fill(dtTempTagDet)

        If TagSplitNoGen_Format2 Then
            'TAGNO = TAGNO + 1
            txtTAGNO.Text = tagprefix & TAGNO & dtTagDet.Rows(0).Item("TAGNO").ToString
        Else
            If _chkNewTag = False And txtTAGNO.Text = "" Then
                If gridMultiTag.Rows.Count > 0 Then
                    Dim _tagNo As String = Nothing
                    _tagNo = gridMultiTag.Rows(gridMultiTag.Rows.Count - 1).Cells("TAGNO").Value.ToString
                    If Prefix.Length > 0 And Prefix <> "" And TagPrefix_Item Then
                        _tagNo = _tagNo.Replace(Prefix, "")
                    End If
                    txtTAGNO.Text = GenTagNo(_tagNo)
                Else
                    txtTAGNO.Text = objTag.GetTagNo(Format(dtTempTagDet.Rows(0).Item("RECDATE"), "yyyy-MM-dd"), dtTempTagDet.Rows(0).Item("ITEMNAME").ToString, dtTempTagDet.Rows(0).Item("LOTSNO").ToString)
                    If Prefix.Length > 0 And Prefix <> "" Then
                        txtTAGNO.Text = txtTAGNO.Text.Replace(Prefix, "")
                    End If
                    If TagPrefix_Item = True Then
                        strSql = " SELECT SUBSTRING(SHORTNAME,1,5)SHORTNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & Val(txtItemId.Text)
                        tagprefix = objGPack.GetSqlValue(strSql, , "")
                        Prefix = objGPack.GetSqlValue(strSql, , "")
                    End If
                    If Prefix.Length > 0 And Prefix <> "" Then
                        txtTAGNO.Text = txtTAGNO.Text.Replace(Prefix, "")
                    End If
                    If txtTAGNO.Text.Contains(tagprefix) = False And tagprefix <> "" Then
                        txtTAGNO.Text = tagprefix & txtTAGNO.Text
                    End If
                End If
            ElseIf _chkNewTag = False And txtTAGNO.Text <> "" And gridMultiTag.Rows.Count > 0 Then
                Dim _tagNo As String = Nothing
                _tagNo = gridMultiTag.Rows(gridMultiTag.Rows.Count - 1).Cells("TAGNO").Value.ToString
                If Prefix.Length > 0 And Prefix <> "" And TagPrefix_Item Then
                    _tagNo = _tagNo.Replace(Prefix, "")
                End If
                txtTAGNO.Text = GenTagNo(_tagNo)
            ElseIf _chkNewTag = False And txtTAGNO.Text <> "" Then
                txtTAGNO.Text = txtTAGNO.Text
            End If
        End If

        'txtTAGNO.Text = tagprefix & TAGNO & dtTagDet.Rows(0).Item("TAGNO").ToString
        txtPCS_Num.Text = IIf((Val(dtTagDet.Rows(0).Item("PCS").ToString) - Pcs) > 0, Val(dtTagDet.Rows(0).Item("PCS").ToString) - Pcs, 0)
        txtGRSWT_Wet.Text = Format(Val(dtTagDet.Rows(0).Item("GRSWT").ToString) - GrsWt, "0.000")
        txtNETWT_Wet.Text = Format(Val(dtTagDet.Rows(0).Item("NETWT").ToString) - NetWt, "0.000")
        'txtLESSWT_Wet.Text = Format(Val(dtTagDet.Rows(0).Item("LESSWT").ToString) - LessWt, "0.000")
        'txtSTNPCS.Text = Val(IIf(Val(dtTagDet.Rows(0).Item("STNPCS").ToString) = 0, 0, dtTagDet.Rows(0).Item("STNPCS").ToString)) - StnPcs
        'txtSTnWT.Text = Format(Val(IIf(Val(dtTagDet.Rows(0).Item("STNWT").ToString) = 0, 0, dtTagDet.Rows(0).Item("STNWT").ToString)) - StnWt, "0.000")
    End Sub
    Private Function GenTagNo(ByRef TagNo As String, Optional ByVal tran As OleDbTransaction = Nothing, Optional ByVal increament As Boolean = True) As String
        Dim str As String = Nothing
        Dim fPart As String = Nothing
        Dim sPart As String = Nothing
        If IsNumeric(TagNo) Then
            If increament Then TagNo = Val(TagNo) + 1 Else TagNo = Val(TagNo) - 1
        Else
            For Each c As Char In TagNo
                If IsNumeric(c) Then
                    sPart += c
                Else
                    fPart += c
                End If
            Next
            If increament Then TagNo = fPart + (Val(sPart) + 1).ToString Else TagNo = fPart + (IIf(Val(sPart) - 1 > 0, Val(sPart) - 1, 0)).ToString
        End If
        If TagNo.Contains(tagprefix) Or fPart <> "" Then
            Return TagNo
        Else
            Return tagprefix + TagNo
        End If

    End Function

    Private Function BasedOnUniqueNumeric(ByVal tagNo As String) As String
        Dim retStr As String = Nothing
        Dim numericPart As Integer = Val(tagNo)
        numericPart += 1
        retStr = numericPart.ToString '  Format(numericPart, "00000000")
        Return retStr
    End Function

    Private Function BasedOnUniqueYear(ByVal tagNo As String) As String
        Dim retStr As String = Nothing
        Dim yearPart As String = Mid(tagNo, 1, 2)
        Dim alphaPart As String = Mid(tagNo, 3, 1)
        Dim numericPart As Integer = Nothing
        If IsMainCostCentre Then
            numericPart = Val(Mid(tagNo, 4))
        Else
            numericPart = Val(Mid(tagNo, 6))
        End If
        If numericPart < 99999 Then
            numericPart += 1
        Else
            numericPart = 1
            If alphaPart <> "Z" Then
                alphaPart = Chr(Asc(alphaPart) + 1)
            Else
                alphaPart = "A"
                yearPart = Format(Val(yearPart) + 1, "00")
            End If
        End If
        'retStr = yearPart & alphaPart & Format(numericPart, "00000")
        If IsMainCostCentre Then
            retStr = yearPart & alphaPart & FormatStringCustom(numericPart, "0", 5)
        Else
            retStr = yearPart & alphaPart & cnCostId & FormatStringCustom(numericPart, "0", 3)
        End If
        Return retStr
    End Function

    Private Function BasedOnUniqueMonthYear(ByVal LastTagNo As String, ByVal RecDate As Date) As String
        Dim retStr As String = Nothing
        Dim MonthPart As String = Mid(LastTagNo, 1, 2)
        Dim YearPart As String = Mid(LastTagNo, 3, 2)
        Dim AlphaPart As Char = Mid(LastTagNo, 5, 1)
        Dim Limit As String
        Dim NumericPart As Integer
        If Char.IsLetter(AlphaPart) Then 'Alpha Contains
            'Limit = "999"
            NumericPart = Mid(LastTagNo, 6, 3)
        Else
            'Limit = "9999"
            'AlphaPart = ""
            NumericPart = Val(Mid(LastTagNo, 5, 4))
        End If
        If Val(MonthPart) <> Val(RecDate.Month) Then
            NumericPart = 0
            MonthPart = Format(RecDate.Month, "00")
            AlphaPart = ""
        End If
        If Val(YearPart) <> Val(Mid(RecDate.Year, 3, 2)) Then
            NumericPart = 0
            YearPart = Format(Val(Mid(RecDate.Year, 3, 2)), "00")
            AlphaPart = ""
        End If
        If Char.IsLetter(AlphaPart) Then 'Alpha Contains
            Limit = "999"
        Else
            Limit = "9999"
        End If
        If NumericPart < Val(Limit) Then
            NumericPart += 1
        Else
            NumericPart = 1
            Limit = 999
            If AlphaPart = Nothing Then
                AlphaPart = "A"
            ElseIf AlphaPart <> "Z" Then
                AlphaPart = Chr(Asc(AlphaPart) + 1)
            ElseIf Val(MonthPart) <> 12 Then
                AlphaPart = "A"
                MonthPart = Format(Val(MonthPart) + 1, "00")
            Else
                AlphaPart = "A"
                MonthPart = "01"
            End If
        End If
        Dim num As String = FormatStringCustom(NumericPart, "0", Limit.Length)
        retStr = MonthPart & YearPart
        If AlphaPart <> Nothing And (num.Length <= 3) Then retStr += AlphaPart
        retStr += num
        Return retStr
    End Function

    Private Sub txtStTagno_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtStTagno.GotFocus
        txtStTagno.Text = txtTAGNO.Text
        If txtStONERowIndex.Text <> "" Then Exit Sub
        txtStPcs_NUM.Text = Val(IIf(Val(dtTagDet.Rows(0).Item("STNPCS").ToString) = 0, 0, dtTagDet.Rows(0).Item("STNPCS").ToString)) - StnPcs
        txtStWeight_WET.Text = Format(Val(IIf(Val(dtTagDet.Rows(0).Item("STNWT").ToString) = 0, 0, dtTagDet.Rows(0).Item("STNWT").ToString)) - StnWt, "0.000")
    End Sub
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        funcNew()
    End Sub
    Private Sub StyleGridStone(ByVal grid As DataGridView)
        With grid
            .Columns("STNPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("STNWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("STNRATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("STNAMT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        End With
    End Sub
    Private Sub StyleGrid(ByVal grid As DataGridView)
        With grid
            .Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("LESSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("STNPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("STNWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        End With
    End Sub
    Private Sub CalcStoneAmount()
        Dim amt As Double = Nothing
        If cmbStCalc.Text = "P" Then
            amt = Val(txtStRate_Amt.Text) * Val(txtStPcs_NUM.Text)
        Else
            amt = Val(txtStRate_Amt.Text) * Val(txtStWeight_WET.Text)
        End If
        txtStAmount_Amt.Text = IIf(amt <> 0, Format(amt, "0.00"), "")
    End Sub

    Function funcAdd()
        Dim TagSno As String = Nothing
        Dim SNO As String
        Dim CompanyId As String
        Dim dtsave As New DataTable
        strSql = " SELECT * FROM " & cnAdminDb & "..ITEMTAG AS T"
        strSql += " WHERE SNO='" & dtTagDet.Rows(0).Item("SNO").ToString & "' AND ISNULL(ISSDATE,'')=''"
        If Not cnCentStock Then strSql += " AND T.COMPANYID = '" & GetStockCompId() & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtsave)
        If dtsave.Rows.Count = 0 Then MsgBox("Tag Info Not Found", MsgBoxStyle.Information) : Exit Function
        CompanyId = dtsave.Rows(0).Item("COMPANYID").ToString
        strSql = " SELECT * FROM " & cnAdminDb & "..ITEMTAGSTONE "
        strSql += " WHERE TAGSNO='" & dtTagDet.Rows(0).Item("SNO").ToString & "'"
        If Not cnCentStock Then strSql += " AND COMPANYID = '" & GetStockCompId() & "'"
        Dim dtStone As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtStone)
        If Val(gridMultTagTotal.Rows(0).Cells("GRSWT").Value.ToString) <> Val(dtsave.Rows(0).Item("GRSWT").ToString) Then
            MsgBox("GrsWt Not Matched.", MsgBoxStyle.Information) : Exit Function
            'If ChkRatio.Checked Then
            '    Dim chkRatiowt As Double = 0
            '    chkRatiowt = Math.Round(Val(gridMultTagTotal.Rows(0).Cells("GRSWT").Value.ToString) - Val(dtsave.Rows(0).Item("GRSWT").ToString), 3)
            '    If Not (chkRatiowt = 0.001 Or chkRatiowt = -0.001) Then
            '        MsgBox("GrsWt Not Matched.", MsgBoxStyle.Information) : Exit Function
            '    End If
            'Else
            '    MsgBox("GrsWt Not Matched.", MsgBoxStyle.Information) : Exit Function
            'End If
        End If
        If TagSplitZeroPcs And Val(gridMultTagTotal.Rows(0).Cells("PCS").Value.ToString) <> Val(dtsave.Rows(0).Item("PCS").ToString) Then
            MsgBox("Pcs Not Matched.", MsgBoxStyle.Information) : Exit Function
        End If
        If Val(gridMultTagTotal.Rows(0).Cells("NETWT").Value.ToString) <> Val(dtsave.Rows(0).Item("NETWT").ToString) Then
            MsgBox("NetWt Not Matched.", MsgBoxStyle.Information) : Exit Function
            'Dim chkRatiowt As Double = 0
            'chkRatiowt = Math.Round(Val(gridMultTagTotal.Rows(0).Cells("NETWT").Value.ToString) - Val(dtsave.Rows(0).Item("NETWT").ToString), 3)
            'If Not (chkRatiowt = 0.001 Or chkRatiowt = -0.001) Then
            '    MsgBox("NetWt Not Matched.", MsgBoxStyle.Information) : Exit Function
            'End If
        End If
        If Val(gridMultTagTotal.Rows(0).Cells("LESSWT").Value.ToString) <> Val(dtsave.Rows(0).Item("LESSWT").ToString) Then
            'MsgBox("LessWt Not Matched.", MsgBoxStyle.Information) : Exit Function
            'Dim chkRatiowt As Double = 0
            'chkRatiowt = Math.Round(Val(gridMultTagTotal.Rows(0).Cells("LESSWT").Value.ToString) - Val(dtsave.Rows(0).Item("LESSWT").ToString), 3)
            'If Not (chkRatiowt = 0.001 Or chkRatiowt = -0.001) Then
            '    MsgBox("LessWt Not Matched.", MsgBoxStyle.Information) : Exit Function
            'End If
        End If
        Dim tagnos As String = ""
        Dim entrydate As DateTime = GetEntryDate(GetServerDate, tran).ToString
        SNO = dtsave.Rows(0).Item("LOTSNO").ToString
        Dim DupTagFound As Boolean = False
        Try
            strSql = " SELECT ISNULL(CALTYPE,'') CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ITEMID,'') ='" & txtItemId.Text & "'"
            Dim Caltype As String = objGPack.GetSqlValue(strSql, "CALTYPE", "")
            tran = Nothing
            tran = cn.BeginTransaction()
            Dim mtagno As String = ""
            If gridMultiTag.Rows.Count > 0 Then
                Dim newitemid As Integer
                Dim newsubitemid As Integer
                Dim maxmcgrm As Decimal = 0
                Dim maxwastper As Decimal = 0
                Dim minmcgrm As Decimal = 0
                Dim minwastper As Decimal = 0
                Dim StkType As String = ""
                Dim wt As Decimal = 0
                Dim GrsNet As String = "G"
                maxmcgrm = Val(dtsave.Rows(0).Item("MAXMCGRM").ToString)
                minmcgrm = Val(dtsave.Rows(0).Item("MINMCGRM").ToString)
                maxwastper = Val(dtsave.Rows(0).Item("MAXWASTPER").ToString)
                minwastper = Val(dtsave.Rows(0).Item("MINWASTPER").ToString)
                GrsNet = dtsave.Rows(0).Item("GRSNET").ToString
                wt = IIf(GrsNet = "G", Val(dtsave.Rows(0).Item("GRSWT").ToString), Val(dtsave.Rows(0).Item("NETWT").ToString))
                If wt <> 0 Then
                    If maxmcgrm = 0 Then maxmcgrm = Math.Round(Val(dtsave.Rows(0).Item("MAXMC").ToString) / wt, 2)
                    If minmcgrm = 0 Then minmcgrm = Math.Round(Val(dtsave.Rows(0).Item("MINMC").ToString) / wt, 2)
                    If maxwastper = 0 Then maxwastper = Math.Round(Val(dtsave.Rows(0).Item("MAXWAST").ToString) / (wt / 100), 2)
                    If minwastper = 0 Then minwastper = Math.Round(Val(dtsave.Rows(0).Item("MINWAST").ToString) / (wt / 100), 2)
                End If
                For I As Integer = 0 To gridMultiTag.Rows.Count - 1
                    Dim maxwast As Decimal = Val((IIf(GrsNet = "G", Val(gridMultiTag.Rows(I).Cells("GRSWT").Value.ToString), Val(gridMultiTag.Rows(I).Cells("NETWT").Value.ToString)) / 100) * maxwastper)
                    Dim maxmc As Decimal = CalcRoundoffAmt(Math.Abs(Val((Val(IIf(GrsNet = "G", gridMultiTag.Rows(I).Cells("GRSWT").Value.ToString, gridMultiTag.Rows(I).Cells("NETWT").Value.ToString)) + IIf(McWithWastage = True, maxwast, 0)) * maxmcgrm)), objSoftKeys.RoundOff_Gross)
                    Dim minwast As Decimal = Val((IIf(GrsNet = "G", Val(gridMultiTag.Rows(I).Cells("GRSWT").Value.ToString), Val(gridMultiTag.Rows(I).Cells("NETWT").Value.ToString)) / 100) * minwastper)
                    Dim minmc As Decimal = CalcRoundoffAmt(Math.Abs(Val((Val(IIf(GrsNet = "G", gridMultiTag.Rows(I).Cells("GRSWT").Value.ToString, gridMultiTag.Rows(I).Cells("GRSWT").Value.ToString)) + +IIf(McWithWastage = True, minwast, 0)) * minmcgrm)), objSoftKeys.RoundOff_Gross)

                    strSql = " SELECT ISNULL(STKTYPE,'') STKTYPE FROM " & cnAdminDb & "..ITEMTAG WHERE 1=1 "
                    strSql += " AND SNO='" & dtTagDet.Rows(0).Item("SNO").ToString & "' "
                    StkType = objGPack.GetSqlValue(strSql, "STKTYPE", "", tran)
                    mtagno = gridMultiTag.Rows(I).Cells("TAGNO").Value.ToString
                    Dim NewTagNo As String = ""
                    strSql = " SELECT COUNT(*) CNT FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO='" & mtagno & "'"
                    If Val(objGPack.GetSqlValue(strSql, "CNT", , tran)) > 0 Then
                        DupTagFound = True
                        NewTagNo = objTag.GetTagNo(Format(dtTempTagDet.Rows(0).Item("RECDATE"), "yyyy-MM-dd"), dtTempTagDet.Rows(0).Item("ITEMNAME").ToString, dtTempTagDet.Rows(0).Item("LOTSNO").ToString, tran)
                        If Prefix <> "" Then
                            NewTagNo = NewTagNo.Replace(Prefix, "")
                        End If
                        If NewTagNo.Contains(tagprefix) = False And tagprefix <> "" Then
                            NewTagNo = tagprefix & NewTagNo
                        End If
                        If gridTagStone.Rows.Count > 0 Then
                            For k As Integer = 0 To gridTagStone.Rows.Count - 1
                                If gridMultiTag.Rows(I).Cells("TAGNO").Value = gridTagStone.Rows(k).Cells("STNTAGNO").Value Then
                                    gridTagStone.Rows(k).Cells("NEWSTNTAGNO").Value = NewTagNo
                                End If
                            Next
                        End If
                        gridMultiTag.Rows(I).Cells("NEWTAGNO").Value = NewTagNo
                        mtagno = NewTagNo
                    End If

                    tagnos = tagnos & mtagno & ","

                    strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & gridMultiTag.Rows(I).Cells("ITEM").Value.ToString & "'"
                    newitemid = Val(objGPack.GetSqlValue(strSql, "ITEMID", , tran))

                    strSql = " SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & gridMultiTag.Rows(I).Cells("SUBITEM").Value.ToString & "' AND ITEMID = " & newitemid & ""
                    newsubitemid = Val(objGPack.GetSqlValue(strSql, "SUBITEMID", , tran))
                    Dim tagval As Int64 = 0
                    ''Find TagVal
                    tagval = objTag.GetTagVal(gridMultiTag.Rows(I).Cells("TAGNO").Value.ToString, tran, Format(dtTempTagDet.Rows(0).Item("RECDATE"), "yyyy-MM-dd"))

                    If objTag._TagNoGen = "T" Or objTag._TagNoGen = "D" Then
                        TagSno = IIf(COSTCENTRE_SINGLE = False, cnCostId, cnCostId) & mtagno
                        'Future Will Be Done
                        'If TAGSPLITDATE = True Then
                        '    TagSno = objTag.BasedOnSNOGenerator("", entrydate, "")
                        'Else
                        '    Dim _entryDate As Date = dtsave.Rows(0).Item("RECDATE")
                        '    TagSno = objTag.BasedOnSNOGenerator("", _entryDate.Date, "")
                        'End If
                    Else
                        TagSno = GetNewSno(TranSnoType.ITEMTAGCODE, tran, "GET_ADMINSNO_TRAN")
                    End If

                    ''INSERTING ITEMTAG
                    strSql = " INSERT INTO " & cnAdminDb & "..ITEMTAG"
                    strSql += " ("
                    strSql += " SNO,RECDATE,COSTID,ITEMID,ORDREPNO,ORSNO,ORDSALMANCODE,SUBITEMID,SIZEID,ITEMCTRID"
                    strSql += " ,TABLECODE,DESIGNERID,TAGNO,PCS,GRSWT,LESSWT,NETWT,RATE,FINERATE,MAXWASTPER,MAXMCGRM"
                    strSql += " ,MAXWAST,MAXMC,MINWASTPER,MINMCGRM,MINWAST,MINMC,TAGKEY,TAGVAL,LOTSNO,COMPANYID,SALVALUE"
                    strSql += " ,PURITY,NARRATION,DESCRIP,REASON,ENTRYMODE,GRSNET,ISSDATE,ISSREFNO,ISSPCS,ISSWT,FROMFLAG"
                    strSql += " ,TOFLAG,APPROVAL,SALEMODE,BATCHNO,MARK,PCTFILE,OLDTAGNO,ITEMTYPEID,ACTUALRECDATE,WEIGHTUNIT"
                    strSql += " ,TRANSFERWT,CHKDATE,CHKTRAY,CARRYFLAG,BRANDID,PRNFLAG,MCDISCPER,WASTDISCPER,RESDATE,TRANINVNO"
                    strSql += " ,SUPBILLNO,WORKDAYS,USERID,UPDATED,UPTIME,SYSTEMID,STYLENO,APPVER,TRANSFERDATE,TRANSFERED"
                    strSql += " ,BOARDRATE,RFID,TOUCH,HM_BILLNO,HM_CENTER,ADD_VA_PER,REFVALUE,VALUEADDEDTYPE,TCOSTID,EXTRAWT"
                    strSql += " ,USRATE,INDRS,REFDATE,STKTYPE"
                    strSql += " )VALUES("

                    strSql += " '" & TagSno & "'" 'SNO
                    If TAGSPLITDATE = True Then
                        strSql += " ,'" & Format(entrydate, "yyyy-MM-dd") & "'" 'RECDATE 'dtpRecieptDate.Value.Date.ToString("yyyy-MM-dd")
                    Else
                        strSql += " ,'" & Format(dtsave.Rows(0).Item("RECDATE"), "yyyy-MM-dd") & "'" 'RECDATE 'dtpRecieptDate.Value.Date.ToString("yyyy-MM-dd")
                    End If
                    strSql += " ,'" & dtsave.Rows(0).Item("COSTID").ToString & "'" 'COSTID
                    strSql += " ," & IIf(newitemid <> 0, newitemid, Val(dtsave.Rows(0).Item("ITEMID").ToString)) & "" 'ITEMID
                    strSql += " ,'" & dtsave.Rows(0).Item("ORDREPNO").ToString & "'" 'ORDREPNO
                    strSql += " ,'" & dtsave.Rows(0).Item("ORSNO").ToString & "'" 'ORsno
                    strSql += " ,''" 'ORDSALMANCODE
                    strSql += " ," & newsubitemid & "" 'SUBITEMID
                    strSql += " ,'" & dtsave.Rows(0).Item("SIZEID").ToString & "'" 'SIZEID
                    strSql += " ," & dtsave.Rows(0).Item("ITEMCTRID").ToString & "" 'ITEMCTRID
                    strSql += " ,'" & dtsave.Rows(0).Item("TABLECODE").ToString & "'"
                    strSql += " ," & Val(dtsave.Rows(0).Item("DESIGNERID").ToString) & "" 'DESIGNERID
                    strSql += " ,'" & mtagno & "'" 'TAGNO
                    strSql += " ," & Val(gridMultiTag.Rows(I).Cells("PCS").Value.ToString) & "" 'PCS
                    strSql += " ," & Val(gridMultiTag.Rows(I).Cells("GRSWT").Value.ToString) & "" 'GRSWT
                    strSql += " ," & Val(gridMultiTag.Rows(I).Cells("LESSWT").Value.ToString) & "" 'LESSWT
                    strSql += " ," & Val(gridMultiTag.Rows(I).Cells("NETWT").Value.ToString) & "" 'NETWT
                    strSql += " ," & Val(dtsave.Rows(0).Item("RATE").ToString) & "" 'RATE
                    strSql += ",0" 'FINERATE
                    strSql += " ," & Val(dtsave.Rows(0).Item("MAXWASTPER").ToString) & "" 'MAXWASTPER
                    strSql += " ," & Val(dtsave.Rows(0).Item("MAXMCGRM").ToString) & "" 'MAXMCGRM
                    strSql += " ," & maxwast & "" 'MAXWAST
                    strSql += " ," & maxmc & "" 'MAXMC
                    strSql += " ," & Val(dtsave.Rows(0).Item("MINWASTPER").ToString) & "" 'MINWASTPER
                    strSql += " ," & Val(dtsave.Rows(0).Item("MINMCGRM").ToString) & "" 'MINMCGRM
                    strSql += " ," & minwast & "" 'MINWAST
                    strSql += " ," & minmc & "" 'MINMC
                    If objTag._TagNoGen = "D" Then
                        strSql += " ,'" & gridMultiTag.Rows(I).Cells("TAGNO").Value.ToString & "'" 'TAGKEY
                    Else
                        strSql += " ,'" & newitemid & gridMultiTag.Rows(I).Cells("TAGNO").Value.ToString & "'" 'TAGKEY
                    End If
                    strSql += " ," & tagval & "" 'TAGVAL
                    strSql += " ,'" & dtsave.Rows(0).Item("LOTSNO").ToString & "'" 'LOTSNO
                    strSql += " ,'" & CompanyId & "'" 'COMPANYID
                    If Caltype = "R" Then
                        strSql += " ," & Val(gridMultiTag.Rows(I).Cells("SALVALUE").Value.ToString) & "" 'SALVALUE
                    Else
                        strSql += " ," & dtsave.Rows(0).Item("SALVALUE").ToString & "" 'SALVALUE
                    End If
                    strSql += " ," & dtsave.Rows(0).Item("PURITY").ToString & "" 'PURITY
                    strSql += " ,'" & IIf(TagSplitNarr, "SPLITTED", "") & "'" 'NARRATION
                    strSql += " ,'" & dtsave.Rows(0).Item("DESCRIP").ToString & "'"
                    strSql += " ,''" 'REASON
                    strSql += " ,'" & dtsave.Rows(0).Item("ENTRYMODE").ToString & "'"
                    strSql += " ,'" & GrsNet & "'" 'GRSNET
                    strSql += " ,NULL" 'ISSDATE
                    strSql += " ,0" 'ISSREFNO
                    strSql += " ,0" 'ISSPCS
                    strSql += " ,0" 'ISSWT
                    strSql += " ,''" 'FROMFLAG
                    strSql += " ,''" 'TOFLAG
                    strSql += " ,''" 'APPROVAL
                    strSql += " ,'" & dtsave.Rows(0).Item("SALEMODE").ToString & "'" 'SALEMODE
                    strSql += " ,''" 'BATCHNO
                    strSql += " ,0" 'MARK
                    strSql += " ,'" & dtsave.Rows(0).Item("PCTFILE").ToString & "'" 'pctfile
                    'strSql += " ,'" & IIf(picPath <> Nothing, "L" + txtLotNo_Num_Man.Text + "I" + itemId.ToString + "T" + TagNo.Replace(":", "") + "." + picExtension.ToString, picPath) & "'" 'PCTFILE
                    strSql += " ,'" & txt_TagNo.Text & "'" 'OLDTAGNO
                    strSql += " ," & Val(dtsave.Rows(0).Item("ITEMTYPEID").ToString) & "" 'ITEMTYPEID
                    If TAGSPLITDATE = True Then
                        strSql += " ,'" & dtsave.Rows(0).Item("RECDATE").ToString & "'" 'ACTUALRECDATE
                    Else
                        strSql += " ,'" & dtsave.Rows(0).Item("ACTUALRECDATE").ToString & "'" 'ACTUALRECDATE
                    End If
                    strSql += " ,''" 'WEIGHTUNIT
                    strSql += " ,0" 'TRANSFERWT
                    strSql += " ,NULL" 'CHKDATE
                    strSql += " ,''" 'CHKTRAY
                    strSql += " ,''" 'CARRYFLAG
                    strSql += " ,''" 'BRANDID
                    strSql += " ,''" 'PRNFLAG
                    strSql += " ,0" 'MCDISCPER
                    strSql += " ,0" 'WASTDISCPER
                    strSql += " ,NULL" 'RESDATE
                    strSql += " ,'" & dtsave.Rows(0).Item("TRANINVNO").ToString & "'" 'TRANINVNO
                    strSql += " ,'" & dtsave.Rows(0).Item("SUPBILLNO").ToString & "'" 'SUPBILLNO
                    strSql += " ,''" 'WORKDAYS
                    strSql += " ," & userId & "" 'USERID
                    strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                    strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                    strSql += " ,'" & systemId & "'" 'SYSTEMID
                    strSql += " ,'" & dtsave.Rows(0).Item("STYLENO").ToString & "'" 'STYLENO
                    strSql += " ,'" & VERSION & "'" 'APPVER
                    If IsDBNull(dtsave.Rows(0).Item("TRANSFERDATE")) = True Then
                        strSql += " ,NULL" 'TRANSFERDATE
                    Else
                        strSql += " ,'" & Format(dtsave.Rows(0).Item("TRANSFERDATE"), "yyyy-MM-dd") & "'" 'TRANSFERDATE
                    End If
                    strSql += " ,'" & dtsave.Rows(0).Item("TRANSFERED").ToString & "'" 'TRANSFERED
                    strSql += " ," & Val(dtsave.Rows(0).Item("BOARDRATE").ToString) & "" 'BOARDRATE
                    strSql += " ,'" & dtsave.Rows(0).Item("RFID").ToString & "'" 'RFID
                    strSql += " ," & Val(dtsave.Rows(0).Item("TOUCH").ToString) & "" 'TOUCH
                    strSql += " ,'" & dtsave.Rows(0).Item("HM_BILLNO").ToString & "'" 'HM_BILLNO
                    strSql += " ,'" & dtsave.Rows(0).Item("HM_CENTER").ToString & "'" 'HM_CENTER
                    strSql += " ," & Val(dtsave.Rows(0).Item("ADD_VA_PER").ToString) & "" 'ADD_VA_PER
                    strSql += " ," & Val(dtsave.Rows(0).Item("REFVALUE").ToString) & "" 'REFVALUE
                    strSql += " ,'" & dtsave.Rows(0).Item("VALUEADDEDTYPE").ToString & "'" 'VALUEADDEDTYPE
                    strSql += " ,'" & dtsave.Rows(0).Item("TCOSTID").ToString & "'" 'TCOSTID
                    strSql += " ,'" & Val(dtsave.Rows(0).Item("EXTRAWT").ToString) & "'" 'EXTRAWT
                    strSql += " ," & Val(dtsave.Rows(0).Item("USRATE").ToString) & "" 'USRATE
                    strSql += " ," & Val(dtsave.Rows(0).Item("INDRS").ToString) & "" 'INDRS
                    strSql += " ,'" & Format(entrydate, "yyyy-MM-dd") & "'" 'REFDATE
                    strSql += " ,'" & StkType & "'" 'STKTYPE
                    strSql += " )"
                    'cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)

                    If _HasPurchase Then
                        Dim dtPurItemtag As New DataTable
                        strSql = " SELECT * FROM " & cnAdminDb & "..PURITEMTAG AS T"
                        strSql += " WHERE TAGSNO='" & dtTagDet.Rows(0).Item("SNO").ToString & "'"
                        If Not cnCentStock Then strSql += " AND T.COMPANYID = '" & GetStockCompId() & "'"
                        cmd = New OleDbCommand(strSql, cn, tran)
                        da = New OleDbDataAdapter(cmd)
                        da.Fill(dtPurItemtag)
                        If dtPurItemtag.Rows.Count > 0 Then
                            Dim Pmaxmcgrm As Decimal = 0
                            Dim Pmaxwastper As Decimal = 0
                            Dim Pwt As Decimal = 0
                            Pwt = Val(dtPurItemtag.Rows(0).Item("PURNETWT").ToString)
                            If Pwt <> 0 Then
                                If Pmaxmcgrm = 0 Then Pmaxmcgrm = Math.Round(Val(dtPurItemtag.Rows(0).Item("PURMC").ToString) / Pwt, 2)
                                If Pmaxwastper = 0 Then Pmaxwastper = Math.Round(Val(dtPurItemtag.Rows(0).Item("PURWASTAGE").ToString) / (Pwt / 100), 2)
                            End If
                            Dim Pmaxwast As Decimal = Val((Val(gridMultiTag.Rows(I).Cells("NETWT").Value.ToString) / 100) * Pmaxwastper)
                            Dim Pmaxmc As Decimal = Format(Val((Val(gridMultiTag.Rows(I).Cells("NETWT").Value.ToString) + IIf(McWithWastage = True, maxwast, 0)) * Pmaxmcgrm), "0.00")

                            Dim Purvalue As Decimal = Format(((Val(dtPurItemtag.Rows(0).Item("PURVALUE").ToString)) / (Val(dtPurItemtag.Rows(0).Item("PURNETWT").ToString))) * Val(gridMultiTag.Rows(I).Cells("NETWT").Value.ToString), "0.00")
                            Dim Purtax As Decimal = Format(((Val(dtPurItemtag.Rows(0).Item("PURTAX").ToString)) / (Val(dtPurItemtag.Rows(0).Item("PURNETWT").ToString))) * Val(gridMultiTag.Rows(I).Cells("NETWT").Value.ToString), "0.00")

                            ''ITEM PUR DETAIL
                            strSql = vbCrLf + " INSERT INTO " & cnAdminDb & "..PURITEMTAG"
                            strSql += vbCrLf + " ("
                            strSql += vbCrLf + " TAGSNO,ITEMID,TAGNO,PURLESSWT,PURNETWT,PURRATE,PURGRSNET"
                            strSql += vbCrLf + " ,PURWASTAGE,PURTOUCH,PURMC,PURVALUE,PURTAX,RECDATE,COMPANYID,COSTID"
                            strSql += vbCrLf + " )"
                            strSql += vbCrLf + " VALUES"
                            strSql += vbCrLf + " ("
                            strSql += vbCrLf + " '" & TagSno & "'" 'TAGSNO
                            strSql += vbCrLf + " ," & IIf(newitemid <> 0, newitemid, Val(dtsave.Rows(0).Item("ITEMID").ToString)) & "" 'ITEMID
                            strSql += vbCrLf + " ,'" & mtagno & "'" 'TAGNO
                            strSql += vbCrLf + " ," & Val(gridMultiTag.Rows(I).Cells("LESSWT").Value.ToString) & "" ' PURLESSWT
                            strSql += vbCrLf + " ," & Val(gridMultiTag.Rows(I).Cells("NETWT").Value.ToString) & "" ' PURNETWT"
                            strSql += vbCrLf + " ," & Val(dtPurItemtag.Rows(0).Item("PURRATE").ToString) & "" ' PURRATE"
                            strSql += vbCrLf + " ,'" & dtPurItemtag.Rows(0).Item("PURGRSNET").ToString & "'" ' PURGRSNET"
                            strSql += vbCrLf + " ," & Pmaxwast & "" ' PURWASTAGE"
                            strSql += vbCrLf + " ," & Val(dtPurItemtag.Rows(0).Item("PURTOUCH").ToString) & "" ' PURTOUCH"
                            strSql += vbCrLf + " ," & Pmaxmc & "" ' PURMC"
                            strSql += vbCrLf + " ," & Purvalue & "" ' PURVALUE"
                            strSql += vbCrLf + " ," & Purtax & "" 'PURTAX
                            If TAGSPLITDATE = True Then
                                strSql += " ,'" & Format(entrydate, "yyyy-MM-dd") & "'" 'RECDATE 
                            Else
                                strSql += " ,'" & Format(dtsave.Rows(0).Item("RECDATE"), "yyyy-MM-dd") & "'" 'RECDATE 
                            End If
                            strSql += vbCrLf + " ,'" & CompanyId & "'" 'COMPANYID
                            strSql += vbCrLf + " ,'" & dtPurItemtag.Rows(0).Item("COSTID").ToString & "'" 'COSTID
                            strSql += vbCrLf + " )"
                            ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId, , , "TPURITEMTAG", , True)
                        End If
                    End If
                Next
            End If
            If gridTagStone.Rows.Count > 0 Then
                For k As Integer = 0 To gridTagStone.Rows.Count - 1
                    Dim newstnitemid As Integer
                    Dim newstnsubitemid As Integer
                    Dim TagStoneSno As String
                    Dim StnTagNo As String
                    If gridTagStone.Rows(k).Cells("NEWSTNTAGNO").Value.ToString <> "" Then
                        strSql = " SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO='" & gridTagStone.Rows(k).Cells("NEWSTNTAGNO").Value.ToString & "'"
                        StnTagNo = gridTagStone.Rows(k).Cells("NEWSTNTAGNO").Value.ToString
                    Else
                        strSql = " SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO='" & gridTagStone.Rows(k).Cells("STNTAGNO").Value.ToString & "'"
                        StnTagNo = gridTagStone.Rows(k).Cells("STNTAGNO").Value.ToString
                    End If

                    TagStoneSno = objGPack.GetSqlValue(strSql, "SNO", , tran)

                    strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & gridTagStone.Rows(k).Cells("STNITEM").Value.ToString & "'"
                    newstnitemid = Val(objGPack.GetSqlValue(strSql, "ITEMID", , tran))

                    strSql = " SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & gridTagStone.Rows(k).Cells("STNSUBITEM").Value.ToString & "' AND ITEMID = " & newstnitemid & ""
                    newstnsubitemid = Val(objGPack.GetSqlValue(strSql, "SUBITEMID", , tran))

                    Dim stnSno As String = ""
                    If objTag._TagNoGen = "T" Or objTag._TagNoGen = "D" Then
                        'stnSno = IIf(COSTCENTRE_SINGLE = False, cnCostId, cnCostId) & mtagno & k
                        If TAGSPLITDATE = True Then
                            stnSno = objTag.BasedOnSNOGenerator("", entrydate, "") & k
                        Else
                            Dim _entryDate As Date = dtsave.Rows(0).Item("RECDATE")
                            stnSno = objTag.BasedOnSNOGenerator("", _entryDate, "") & k
                        End If
                    Else
                        stnSno = GetNewSno(TranSnoType.ITEMTAGSTONECODE, tran, "GET_ADMINSNO_TRAN")
                    End If

                    strSql = " INSERT INTO " & cnAdminDb & "..ITEMTAGSTONE("
                    strSql += " SNO,TAGSNO,ITEMID,COMPANYID,STNITEMID,STNSUBITEMID,TAGNO,STNPCS,STNWT,"
                    strSql += " STNRATE,STNAMT,DESCRIP,RECDATE,CALCMODE,MINRATE,SIZECODE,STONEUNIT,ISSDATE,"
                    strSql += " OLDTAGNO,CARRYFLAG,COSTID,SYSTEMID,APPVER,TRANSFERED,USRATE,INDRS"
                    strSql += " )VALUES("
                    strSql += " '" & stnSno & "'" ''SNO
                    strSql += " ,'" & TagStoneSno & "'" 'TAGSNO
                    strSql += " ,'" & Val(dtsave.Rows(0).Item("ITEMID").ToString) & "'" 'ITEMID
                    strSql += " ,'" & CompanyId & "'" 'COMPANYID
                    strSql += " ," & newstnitemid & "" 'STNITEMID
                    strSql += " ," & newstnsubitemid & "" 'STNSUBITEMID
                    strSql += " ,'" & StnTagNo & "'" 'TAGNO
                    strSql += " ," & Val(gridTagStone.Rows(k).Cells("STNPCS").Value.ToString) & "" 'STNPCS
                    strSql += " ," & Val(gridTagStone.Rows(k).Cells("STNWT").Value.ToString) & "" 'STNWT
                    strSql += " ," & Val(gridTagStone.Rows(k).Cells("STNRATE").Value.ToString) & "" 'STNRATE
                    strSql += " ," & Val(gridTagStone.Rows(k).Cells("STNAMT").Value.ToString) & "" 'STNAMT
                    If newstnsubitemid <> 0 Then 'DESCRIP
                        strSql += " ,'" & gridTagStone.Rows(k).Cells("STNITEM").Value.ToString & "'"
                    Else
                        strSql += " ,'" & gridTagStone.Rows(k).Cells("STNSUBITEM").Value.ToString & "'"
                    End If
                    If TAGSPLITDATE = True Then
                        strSql += " ,'" & Format(entrydate, "yyyy-MM-dd") & "'" 'RECDATE
                    Else
                        strSql += " ,'" & Format(dtsave.Rows(0).Item("RECDATE"), "yyyy-MM-dd") & "'" 'RECDATE
                    End If
                    strSql += " ,'" & gridTagStone.Rows(k).Cells("STNCAL").Value.ToString & "'" 'CALCMODE
                    strSql += " ,0" 'MINRATE
                    strSql += " ,0" 'SIZECODE
                    strSql += " ,'" & gridTagStone.Rows(k).Cells("STNUNIT").Value.ToString & "'" 'STONEUNIT
                    strSql += " ,NULL" 'ISSDATE
                    strSql += " ,'" & txt_TagNo.Text & "'" 'OLDTAGNO
                    strSql += " ,''" 'CARRYFLAG
                    strSql += " ,'" & dtsave.Rows(0).Item("COSTID").ToString & "'" 'COSTID
                    strSql += " ,'" & systemId & "'" 'SYSTEMID
                    strSql += " ,'" & VERSION & "'" 'APPVER
                    strSql += " ,'" & dtStone.Rows(0).Item("TRANSFERED").ToString & "'" 'USRATE
                    strSql += " ," & Val(dtStone.Rows(0).Item("USRATE").ToString) & "" 'USRATE
                    strSql += " ," & Val(dtStone.Rows(0).Item("INDRS").ToString) & "" 'INDRS
                    strSql += " )"
                    'cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)

                    If _HasPurchase Then
                        Dim Purrate As Decimal = 0
                        Dim Purdiv As Decimal = 0
                        Dim Purvalue As Decimal = 0
                        Dim dtPurItemtagstone As New DataTable
                        strSql = " SELECT * FROM " & cnAdminDb & "..PURITEMTAGSTONE WHERE TAGSNO='" & dtTagDet.Rows(0).Item("SNO").ToString & "'"
                        If k < gridViewTagStone.Rows.Count And gridViewTagStone.Rows.Count > 0 Then
                            strSql += vbCrLf + " AND STNSNO='" & gridViewTagStone.Rows(k).Cells("SNO").Value.ToString & "'"
                        ElseIf gridViewTagStone.Rows.Count > 0 Then
                            strSql += vbCrLf + " AND STNSNO='" & gridViewTagStone.Rows(0).Cells("SNO").Value.ToString & "'"
                        End If
                        cmd = New OleDbCommand(strSql, cn, tran)
                        da = New OleDbDataAdapter(cmd)
                        da.Fill(dtPurItemtagstone)

                        If dtPurItemtagstone.Rows.Count > 0 Then
                            Purrate = Val(dtPurItemtagstone.Rows(0).Item("PURRATE").ToString)
                            If Val(dtPurItemtagstone.Rows(0).Item("STNWT").ToString) > 0 Then
                                Purdiv = Val(dtPurItemtagstone.Rows(0).Item("PURAMT").ToString) / Val(dtPurItemtagstone.Rows(0).Item("STNWT").ToString)
                            End If
                            Purvalue = Format(Val(Purdiv) * Val(gridTagStone.Rows(k).Cells("STNWT").Value.ToString), "0.00")


                        End If

                        strSql = vbCrLf + " INSERT INTO " & cnAdminDb & "..PURITEMTAGSTONE"
                        strSql += vbCrLf + " ("
                        strSql += vbCrLf + " TAGSNO,ITEMID,TAGNO,STNITEMID,STNSUBITEMID,STNPCS,STNWT,STNRATE,STNAMT"
                        strSql += vbCrLf + " ,STONEUNIT,CALCMODE,PURRATE,PURAMT,COMPANYID,COSTID,STNSNO"
                        strSql += vbCrLf + " )"
                        strSql += vbCrLf + " VALUES"
                        strSql += vbCrLf + " ("
                        strSql += vbCrLf + " '" & TagStoneSno & "'" 'TAGSNO
                        strSql += vbCrLf + " ," & Val(dtsave.Rows(0).Item("ITEMID").ToString) & "" 'ITEMID
                        If gridTagStone.Rows(k).Cells("NEWSTNTAGNO").Value.ToString <> "" Then
                            strSql += " ,'" & gridTagStone.Rows(k).Cells("NEWSTNTAGNO").Value.ToString & "'" 'TAGNO
                        Else
                            strSql += " ,'" & gridTagStone.Rows(k).Cells("STNTAGNO").Value.ToString & "'" 'TAGNO
                        End If
                        strSql += vbCrLf + " ," & newstnitemid & "" 'STNITEMID
                        strSql += vbCrLf + " ," & newstnsubitemid & "" 'STNSUBITEMID
                        strSql += vbCrLf + " ," & Val(gridTagStone.Rows(k).Cells("STNPCS").Value.ToString) & "" 'STNPCS
                        strSql += vbCrLf + " ," & Val(gridTagStone.Rows(k).Cells("STNWT").Value.ToString) & "" 'STNWT
                        strSql += vbCrLf + " ," & Val(gridTagStone.Rows(k).Cells("STNRATE").Value.ToString) & "" 'STNRATE
                        strSql += vbCrLf + " ," & Val(gridTagStone.Rows(k).Cells("STNAMT").Value.ToString) & "" 'STNAMT
                        strSql += vbCrLf + " ,'" & gridTagStone.Rows(k).Cells("STNUNIT").Value.ToString & "'" 'STONEUNIT
                        strSql += vbCrLf + " ,'" & gridTagStone.Rows(k).Cells("STNCAL").Value.ToString & "'" 'CALCMODE
                        strSql += vbCrLf + " ," & Purrate & "" 'PURRATE
                        strSql += vbCrLf + " ," & Purvalue & "" 'PURAMT
                        strSql += vbCrLf + " ,'" & CompanyId & "'" 'COMPANYID
                        strSql += vbCrLf + " ,'" & dtsave.Rows(0).Item("COSTID").ToString & "'" 'COSTID
                        strSql += vbCrLf + " ,'" & stnSno & "'" 'STNSNO
                        strSql += vbCrLf + " )"
                        ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId, , , "TPURITEMTAGSTONE", , True)
                    End If
                Next
            End If
            If TAGSPLITDATE = True Then
                strSql = "INSERT INTO " & cnAdminDb & "..CTRANSFER"
                strSql += " (SNO,TAGSNO,ITEMID,TAGNO,ITEMCTRID,RECDATE,ISSDATE,TAGVAL,USERID"
                strSql += " ,UPDATED,UPTIME,APPVER,ENTORDER,COSTID,REASON)"
                If objTag._TagNoGen = "T" Or objTag._TagNoGen = "D" Then
                    If TAGSPLITDATE = True Then
                        strSql += " SELECT '" & objTag.BasedOnSNOGenerator("", entrydate, "") & "' AS SNO" ''strSql += " SELECT '" & IIf(COSTCENTRE_SINGLE = False, cnCostId, cnCostId) & mtagno & "' AS SNO"
                    Else
                        strSql += " SELECT '" & objTag.BasedOnSNOGenerator("", entrydate, "") & "' AS SNO"
                    End If
                Else
                    strSql += " SELECT '" & GetNewSno(TranSnoType.CTRANSFERCODE, tran, "GET_ADMINSNO_TRAN") & "' AS SNO"
                End If
                strSql += " ,SNO TAGSNO"
                strSql += " ,ITEMID,TAGNO,ITEMCTRID,RECDATE,'" & Format(entrydate, "yyyy-MM-dd") & "' AS ISSDATE"
                strSql += " ,TAGVAL," & userId & " USERID"
                strSql += " ,'" & GetEntryDate(GetServerDate(tran), tran) & "'UPDATED"
                strSql += " ,'" & GetServerTime(tran) & "' UPTIME"
                strSql += " ,'" & VERSION & "' APPVER,1"
                strSql += " ,COSTID"
                strSql += " ,'TAG SPLITTED'"
                strSql += " FROM " & cnAdminDb & "..ITEMTAG WHERE SNO = '" & dtTagDet.Rows(0).Item("SNO").ToString & "'"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)

                strSql = " UPDATE " & cnAdminDb & "..ITEMTAG SET ISSDATE='" & Format(entrydate, "yyyy-MM-dd") & "',TOFLAG='MI'"
                strSql += " ,NARRATION='TAG SPLITTED'"
                strSql += " WHERE SNO='" & dtTagDet.Rows(0).Item("SNO").ToString & "'"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)

                strSql = " UPDATE " & cnAdminDb & "..ITEMTAGSTONE SET ISSDATE='" & Format(entrydate, "yyyy-MM-dd") & "'"
                strSql += " WHERE TAGSNO='" & dtTagDet.Rows(0).Item("SNO").ToString & "'"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
            Else
                strSql = "INSERT INTO " & cnAdminDb & "..CTRANSFER"
                strSql += " (SNO,TAGSNO,ITEMID,TAGNO,ITEMCTRID,RECDATE,ISSDATE,TAGVAL,USERID"
                strSql += " ,UPDATED,UPTIME,APPVER,ENTORDER,COSTID,REASON)"
                If objTag._TagNoGen = "T" Or objTag._TagNoGen = "D" Then
                    'strSql += " SELECT '" & IIf(COSTCENTRE_SINGLE = False, cnCostId, cnCostId) & mtagno & "' AS SNO"
                    strSql += " SELECT '" & objTag.BasedOnSNOGenerator("", entrydate, "") & "' AS SNO"
                Else
                    strSql += " SELECT '" & GetNewSno(TranSnoType.CTRANSFERCODE, tran, "GET_ADMINSNO_TRAN") & "' AS SNO"
                End If
                strSql += " ,SNO TAGSNO"
                strSql += " ,ITEMID,TAGNO,ITEMCTRID,RECDATE,'" & Format(dtsave.Rows(0).Item("RECDATE"), "yyyy-MM-dd") & "' AS ISSDATE"
                strSql += " ,TAGVAL," & userId & " USERID"
                strSql += " ,'" & GetEntryDate(GetServerDate(tran), tran) & "'UPDATED"
                strSql += " ,'" & GetServerTime(tran) & "' UPTIME"
                strSql += " ,'" & VERSION & "' APPVER,1"
                strSql += " ,COSTID"
                strSql += " ,'TAG SPLITTED'"
                strSql += " FROM " & cnAdminDb & "..ITEMTAG WHERE SNO = '" & dtTagDet.Rows(0).Item("SNO").ToString & "'"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)

                strSql = " UPDATE " & cnAdminDb & "..ITEMTAG SET ISSDATE='" & Format(dtsave.Rows(0).Item("RECDATE"), "yyyy-MM-dd") & "',TOFLAG='MI'"
                strSql += " ,NARRATION='TAG SPLITTED'"
                strSql += " WHERE SNO='" & dtTagDet.Rows(0).Item("SNO").ToString & "'"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)

                strSql = " UPDATE " & cnAdminDb & "..ITEMTAGSTONE SET ISSDATE='" & Format(dtsave.Rows(0).Item("RECDATE"), "yyyy-MM-dd") & "'"
                strSql += " WHERE TAGSNO='" & dtTagDet.Rows(0).Item("SNO").ToString & "'"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
            End If
            If gridMultiTag.Rows.Count > 0 Then
                Dim _lastTagNo As String = ""
                If gridMultiTag.Rows(gridMultiTag.Rows.Count - 1).Cells("NEWTAGNO").Value.ToString <> "" Then
                    _lastTagNo = gridMultiTag.Rows(gridMultiTag.Rows.Count - 1).Cells("NEWTAGNO").Value.ToString
                Else
                    _lastTagNo = gridMultiTag.Rows(gridMultiTag.Rows.Count - 1).Cells("TAGNO").Value.ToString
                End If
                strSql = "SELECT 1 FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO='" & _lastTagNo & "' AND COSTID='" & cnCostId & "' "
                If Val(objGPack.GetSqlValue(strSql, , , tran)) > 3 Then
                    If Not tran Is Nothing Then tran.Rollback()
                    MsgBox("Tagno already found - " & _lastTagNo)
                End If
                If TagSplitNoGen_Format2 = False Then
                    strSql = " UPDATE " & cnAdminDb & "..SOFTCONTROL SET CTLTEXT  = '" & Replace(_lastTagNo, tagprefix, "") & "' WHERE CTLID = 'LASTTAGNO'"
                    cmd = New OleDbCommand(strSql, cn, tran)
                    cmd.ExecuteNonQuery()
                End If
            End If

            strSql = " Insert into " & cnAdminDb & "..MITEMTAG "
            strSql += " select * from " & cnAdminDb & "..itemtag WHERE TOFLAG='MI'"
            strSql += " and NARRATION='TAG SPLITTED'"
            strSql += " and SNO='" & dtTagDet.Rows(0).Item("SNO").ToString & "'"
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)


            strSql = " UPDATE " & cnAdminDb & "..MITEMTAG SET ISSDATE='" & Format(entrydate, "yyyy-MM-dd") & "' WHERE TOFLAG='MI'"
            strSql += " and NARRATION='TAG SPLITTED'"
            strSql += " and SNO='" & dtTagDet.Rows(0).Item("SNO").ToString & "'"
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)


            tran.Commit()
            tran = Nothing
            If tagnos <> "" Then tagnos = Mid(tagnos, 1, tagnos.Length - 1)
            MsgBox("Tag Splitted Successfully Completed", MsgBoxStyle.Information)


            If CallBarcodeExe = True Then
                Dim oldItem As Integer = Nothing
                Dim paramStr As String = ""
                Dim prnmemsuffix As String = ""
                If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId
                Dim write As StreamWriter
                Dim memfile As String = "\Barcodeprint" & prnmemsuffix & ".mem"
                write = IO.File.CreateText(Application.StartupPath & memfile)
                For Each ro As String In tagnos.Split(",")
                    If oldItem <> Val(txtItemId.Text.ToString()) Then
                        write.WriteLine(LSet("PROC", 7) & ":" & txtItemId.Text.ToString())
                        paramStr += LSet("PROC", 7) & ":" & txtItemId.Text.ToString() & ";"
                        oldItem = Val(txtItemId.Text.ToString())
                    End If
                    write.WriteLine(LSet("TAGNO", 7) & ":" & ro.ToString)
                    paramStr += LSet("TAGNO", 7) & ":" & ro.ToString & ";"
                Next
                If paramStr.EndsWith(";") Then
                    paramStr = Mid(paramStr, 1, paramStr.Length - 1)
                End If
                write.Flush()
                write.Close()
                If EXE_WITH_PARAM = False Then
                    If IO.File.Exists(Application.StartupPath & "\PrjBarcodePrint.exe") Then
                        System.Diagnostics.Process.Start(Application.StartupPath & "\PrjBarcodePrint.exe")
                    Else
                        MsgBox("Barcode print exe not found", MsgBoxStyle.Information)
                    End If
                Else
                    If IO.File.Exists(Application.StartupPath & "\PrjBarcodePrint.exe") Then
                        System.Diagnostics.Process.Start(Application.StartupPath & "\PrjBarcodePrint.exe", paramStr)
                    Else
                        MsgBox("Barcode print exe not found", MsgBoxStyle.Information)
                    End If
                End If
            End If

            If DupTagFound Then
                MsgBox("Duplicate Tag Found,Tag Regenerated(" & tagnos & ")", MsgBoxStyle.Information)
            End If
            DupTagFound = False
            funcNew()
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback() : tran = Nothing
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
            Exit Function
        End Try
    End Function
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click, SaveToolStripMenuItem.Click
        funcAdd()
    End Sub

    Private Sub txtItemId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtItemId.KeyDown
        If e.KeyCode = Keys.Insert Then
            LoadItemNames()
        End If
    End Sub

    Private Sub LoadItemNames()
        strSql = " SELECT"
        strSql += " ITEMID AS ITEMID,ITEMNAME AS ITEMNAME"
        strSql += " FROM " & cnAdminDb & "..ITEMMAST"
        strSql += " WHERE ISNULL(ACTIVE,'Y') <> 'N'"
        Dim itemid As String = BrighttechPack.SearchDialog.Show("Find ItemName", strSql, cn, 0, 0, , txtItemId.Text)
        If itemid <> "" Then
            txtItemId.Text = itemid
        Else
            txtItemId.Focus()
            txtItemId.SelectAll()
        End If
        Me.SelectNextControl(txtItemId, True, True, True, True)
    End Sub
    Private Sub txtItemId_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtItemId.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            Dim spChar As String = GetAdmindbSoftValue("PRODTAGSEP", "")
            If spChar <> "" Then
                If txtItemId.Text.Contains(spChar) Then
                    Dim sp() As String = txtItemId.Text.Split(spChar)
                    If sp.Length >= 2 Then
                        txt_TagNo.Text = Trim(sp(1))
                        txtItemId.Text = Trim(sp(0))
                    End If
                End If
            End If
        End If
    End Sub

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
    End Sub

    Private Sub txtStItem_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtStItem.GotFocus
        Main.ShowHelpText("Press Insert Key to Help")
    End Sub

    Private Sub txtStItem_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtStItem.KeyDown
        If e.KeyCode = Keys.Insert Then
            LoadStoneItemName()
        ElseIf e.KeyCode = Keys.Down Then
            If gridTagStone.RowCount > 0 Then
                gridTagStone.Focus()
            End If
        End If
    End Sub

    Private Sub txtStItem_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtStItem.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtStItem.Text = "" Then
                LoadStoneItemName()
            ElseIf txtStItem.Text <> "" And objGPack.DupCheck("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "'") = False Then
                LoadStoneItemName()
            Else
                LoadStoneitemDetails()
            End If
        ElseIf e.KeyChar = Chr(Keys.Escape) Then
            txtStnWt_Wet.Focus()
        End If
    End Sub

    Private Sub txtTAGNO_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtTAGNO.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            txtPCS_Num.Focus()
        End If
    End Sub

    Private Sub txtPCS_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPCS_Num.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            txtGRSWT_Wet.Focus()
        End If
    End Sub
    Private Sub txtGRSWT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtGRSWT_Wet.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            Dim _pcs As Double = 0 : Dim _Wt As Double = 0
            For Each gv As DataGridViewRow In gridMultiTag.Rows
                With gv
                    _pcs += Val(.Cells("PCS").Value.ToString)
                    _Wt += Val(.Cells("GRSWT").Value.ToString)
                End With
            Next
            _pcs += Val(txtPCS_Num.Text.ToString)
            _Wt += Math.Round(Val(txtGRSWT_Wet.Text.ToString), 3)
            _Wt = Math.Round(Val(_Wt), 3)
            If TagSplitZeroPcs And _pcs > Val(dtTagDet.Compute("SUM(PCS)", "PCS<>0").ToString) Then
                MsgBox("Tag pcs Exceeded...")
                txtPCS_Num.Text = ""
                txtPCS_Num.Focus()
            ElseIf _Wt > Val(dtTagDet.Compute("SUM(GRSWT)", "GRSWT<>0").ToString) Then
                MsgBox("Tag GrsWt Exceeded...")
                txtGRSWT_Wet.Text = ""
                txtGRSWT_Wet.Focus()
            End If
            txtNETWT_Wet.Text = Format(Val(txtGRSWT_Wet.Text), "0.000")
            If (Val(IIf(Val(dtTagDet.Rows(0).Item("STNWT").ToString) = 0, 0, dtTagDet.Rows(0).Item("STNWT").ToString)) - StnWt) > 0 Or (Val(IIf(Val(dtTagDet.Rows(0).Item("STNPCS").ToString) = 0, 0, dtTagDet.Rows(0).Item("STNPCS").ToString)) - StnPcs) > 0 Then
                txtStMetalCode.Visible = False
                txtStONERowIndex.Visible = False
                txtStnpcs_Num.Text = ""
                txtStnWt_Wet.Text = ""
                ObjMaxMinValue = New TagMaxMinValues
                AddHandler ObjMaxMinValue.txtMinWastage_Per.TextChanged, AddressOf ObjMinValues_txtMinWastage_Per_TextChanged
                AddHandler ObjMaxMinValue.txtMinMcPerGram_Amt.TextChanged, AddressOf ObjMinValues_txtMinMcPerGram_Amt_TextChanged
                'If tagEdit And TAGEDITDISABLE.Contains("MC") Then ObjMaxMinValue.txtMinMcPerGram_Amt.ReadOnly = True : ObjMaxMinValue.txtMinMkCharge_Amt.ReadOnly = True
                'If tagEdit And TAGEDITDISABLE.Contains("WS") Then ObjMaxMinValue.txtMinWastage_Per.ReadOnly = True : ObjMaxMinValue.txtMinWastage_Wet.ReadOnly = True
                'ObjMaxMinValue.txtMaxWastage_Per.Focus()
                'ObjMaxMinValue.ShowDialog()
                'Me.SelectNextControl(txtMaxMkCharge_Amt, True, True, True, True)
                txtStItem.Focus()
            Else
                txtStnWt_Wet.Focus()
            End If
        End If
    End Sub

    Private Sub txtStTagno_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtStTagno.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            txtStPcs_NUM.Focus()
        ElseIf e.KeyChar = Chr(Keys.Escape) Then
            txtStnWt_Wet.Focus()
        End If
    End Sub

    Private Sub txtStPcs_NUM_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtStPcs_NUM.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            txtStWeight_WET.Focus()
        ElseIf e.KeyChar = Chr(Keys.Escape) Then
            txtStnWt_Wet.Focus()
        End If
    End Sub
    Private Sub txtStWeight_WET_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtStWeight_WET.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            cmbStUnit.Focus()
        End If
    End Sub

    Private Sub txtStRate_Amt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtStRate_Amt.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            CalcStoneAmount()
            txtStAmount_Amt.Focus()
        End If
    End Sub

    Private Sub txtStRate_Amt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtStRate_Amt.TextChanged
        CalcStoneAmount()
    End Sub
    Private Sub cmbStUnit_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbStUnit.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            cmbStCalc.Focus()
        End If
    End Sub

    Private Sub cmbStCalc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbStCalc.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            txtStRate_Amt.Focus()
        End If
    End Sub

    Private Sub ChkAutoStudSP_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkAutoSPlit.CheckedChanged
        If ChkAutoSPlit.Checked = True Then
            pnlstuddedpcs_OWN.Visible = True
            pnlRatio_OWN.Visible = False
        Else
            pnlstuddedpcs_OWN.Visible = False
        End If
    End Sub

    Private Sub ChkAutoStudSP_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles ChkAutoSPlit.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If ChkAutoSPlit.Checked = False Then
                'lblItemName.Focus()
            End If
        End If
    End Sub

    Private Sub gridTagStone_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridTagStone.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            gridTagStone.CurrentCell = gridTagStone.Rows(gridTagStone.CurrentRow.Index).Cells(1)
        End If
    End Sub

    Private Sub gridTagStone_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridTagStone.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            With gridTagStone.Rows(gridTagStone.CurrentRow.Index)
                txtStItem.Text = .Cells("STNITEM").FormattedValue
                txtStSubItem.Text = .Cells("STNSUBITEM").FormattedValue
                txtStPcs_NUM.Text = .Cells("STNPCS").FormattedValue
                txtStWeight_WET.Text = .Cells("STNWT").FormattedValue
                cmbStUnit.Text = .Cells("STNUNIT").FormattedValue
                cmbStCalc.Text = .Cells("STNCAL").FormattedValue
                txtStRate_Amt.Text = .Cells("STNRATE").FormattedValue
                txtStAmount_Amt.Text = .Cells("STNAMT").FormattedValue
                txtStMetalCode.Text = .Cells("METALID").FormattedValue()
                txtStONERowIndex.Text = gridTagStone.CurrentRow.Index
                txtStItem.Focus()
                txtStItem.SelectAll()
            End With
        End If
    End Sub

    Private Sub txtStItem_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtStItem.LostFocus
        Main.HideHelpText()
    End Sub

    Private Sub cmbStCalc_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbStCalc.SelectedIndexChanged
        CalcStoneAmount()
    End Sub

    Private Sub cmbStUnit_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbStUnit.SelectedIndexChanged
        CalcStoneAmount()
    End Sub

    Private Sub txtStPcs_NUM_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtStPcs_NUM.TextChanged
        CalcStoneAmount()
    End Sub

    Private Sub txtStWeight_WET_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtStWeight_WET.TextChanged
        Dim cent As Double = 0
        strSql = " SELECT ISNULL(CALTYPE,'') CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ITEMNAME,'') ='" & txtStItem.Text & "'"
        Dim mCaltype As String = objGPack.GetSqlValue(strSql, "CALTYPE", "")
        If txtStSubItem.Enabled = True And txtStSubItem.Text <> "" Then
            strSql = " SELECT ISNULL(CALTYPE,'') CALTYPE FROM " & cnAdminDb & "..SUBITEMMAST WHERE ISNULL(SUBITEMNAME,'') ='" & txtStSubItem.Text & "' "
            strSql += vbCrLf + " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "')"
            mCaltype = objGPack.GetSqlValue(strSql, "CALTYPE", "")
        End If
        If mCaltype = "D" Then
            cent = Val(txtStWeight_WET.Text)
        Else
            cent = (Val(txtStWeight_WET.Text) / IIf(Val(txtStPcs_NUM.Text) = 0, 1, Val(txtStPcs_NUM.Text)))
        End If
        cent *= 100
        strSql = " DECLARE @CENT FLOAT"
        strSql += vbCrLf + " SET @CENT = " & cent & ""
        strSql += vbCrLf + " SELECT MAXRATE FROM " & cnAdminDb & "..CENTRATE WHERE"
        strSql += vbCrLf + " @CENT BETWEEN FROMCENT AND TOCENT "
        strSql += vbCrLf + " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "')"
        If txtStSubItem.Enabled = True And txtStSubItem.Text <> "" Then
            strSql += vbCrLf + " AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & txtStSubItem.Text & "' "
            strSql += vbCrLf + " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "')),0)"
        End If
        'If CENTRATE_DESIGNER Then strSql += " AND ISNULL(DESIGNERID,0) =ISNULL((SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME='" & cmbDesigner_MAN.Text & "'),0)"
        'If cmbCostCentre_Man.Text.Trim <> "" Then strSql += " AND COSTID IN(SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME='" & cmbCostCentre_Man.Text & "')"
        Dim rate As Double = Val(objGPack.GetSqlValue(strSql, "MAXRATE", "", tran))
        If rate <> 0 Then
            txtStRate_Amt.Text = IIf(rate <> 0, Format(rate, "0.00"), "")
        Else
            Dim XpurRate As Double = Val(objGPack.GetSqlValue(Replace(strSql, "MAXRATE", "PURRATE"), "PURRATE", "", tran).ToString)
            Dim SaleRateper As Double = Val(objGPack.GetSqlValue(Replace(strSql, "MAXRATE", "SALESPER"), "SALESPER", "", tran).ToString)
            If SaleRateper <> 0 And XpurRate <> 0 Then rate = XpurRate + (XpurRate * (SaleRateper / 100))
            txtStRate_Amt.Text = IIf(rate <> 0, Format(rate, "0.00"), "")
        End If
        Dim PieRate As String = objGPack.GetSqlValue(Replace(strSql, "MAXRATE", "ISNULL(PIERATE,'N') PIERATE"), "PIERATE", "N", tran).ToString
        If PieRate = "Y" Then cmbStCalc.Text = "P"
        CalcStoneAmount()
    End Sub

    Private Sub ChkRatio_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkRatio.CheckedChanged
        If ChkRatio.Checked Then
            pnlRatio_OWN.Visible = True
            pnlstuddedpcs_OWN.Visible = False
            txtSpiltTag_NUM.Text = "2"
            txtSplitRatio_NUM.Text = "50"
        Else
            pnlRatio_OWN.Visible = False
        End If
    End Sub



    Private Sub txtITEM_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtITEM.TextChanged

    End Sub

    Private Sub txt_TagNo_Leave(sender As Object, e As EventArgs) Handles txt_TagNo.Leave
        If dtTagDet.Rows.Count = 0 Then
            txt_TagNo_KeyPress(Me, New KeyPressEventArgs(Chr(Keys.Enter)))
        End If
    End Sub
End Class