Public Class frmMultiMetalDia
    Public dtGridMultiMetal As New DataTable
    Dim strSql As String
    Public IsPOS As Boolean = False
    Public Billdate As Date
    Public Ispartsale As Boolean = False
    Dim RateChangeF4 As Boolean = IIf(GetAdmindbSoftValue("RATECHANGEF4", "Y") = "Y", True, False)
    Dim METALBASERATELOCK As String = GetAdmindbSoftValue("METALBASE_RATELOCK", "")
    Dim GstPer As Double = 0
    Public _touch As Double = 100
    Public InterStateBill As Boolean = False
    Dim MetalBasedStone As Boolean = IIf(GetAdmindbSoftValue("MULTIMETALBASEDSTONE", "N") = "Y", True, False)
    Public MMgrsnet As String = ""
    Public lesswt As Double = 0
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        ''multimetal
        objGPack.Validator_Object(Me)
        objGPack.TextClear(Me)
        gridMultimetal.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        gridMultiMetalTotal.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        gridMultiMetalTotal.DefaultCellStyle.BackColor = grpMultiMetal.BackgroundColor
        gridMultiMetalTotal.DefaultCellStyle.SelectionBackColor = grpMultiMetal.BackgroundColor

        If MetalBasedStone Then
            lblMMWeight.Text = "GRSWT"
            lblMMNetWt.Visible = True
            txtMMNetWeight_Wet.Visible = True
            txtMMNetWeight_Wet.Enabled = True
        Else
            lblMMWeight.Text = "WEIGHT"
            lblMMNetWt.Visible = False
            txtMMNetWeight_Wet.Visible = False
        End If

        With dtGridMultiMetal.Columns
            .Add("KEYNO", GetType(Integer))
            .Add("TRANTYPE", GetType(String))
            .Add("CATEGORY", GetType(String))
            .Add("WEIGHT", GetType(Decimal))
            If MetalBasedStone Then
                .Add("NETWT", GetType(Decimal))
            End If
            .Add("RATE", GetType(Double))
            .Add("WASTAGEPER", GetType(Double))
            .Add("WASTAGE", GetType(Decimal))
            .Add("MCPERGRM", GetType(Double))
            .Add("MC", GetType(Double))
            .Add("AMOUNT", GetType(Double))
            .Add("VATPER", GetType(Double))
            .Add("VAT", GetType(Double))
            .Add("NETAMOUNT", GetType(Double))
            .Add("METALID", GetType(String))
            .Add("PURITYID", GetType(String))
            'GST
            .Add("SGSTPER", GetType(Double))
            .Add("CGSTPER", GetType(Double))
            .Add("IGSTPER", GetType(Double))
            .Add("SGST", GetType(Double))
            .Add("CGST", GetType(Double))
            .Add("IGST", GetType(Double))
            .Add("CESSPER", GetType(Double))
            .Add("CESS", GetType(Double))


            .Add("OWASTAGE", GetType(Decimal))
            .Add("OMC", GetType(Double))
            .Add("OAMOUNT", GetType(Double))
            .Add("OVATPER", GetType(Double))
            .Add("OVAT", GetType(Double))
            .Add("OSGST", GetType(Double))
            .Add("OCGST", GetType(Double))
            .Add("OIGST", GetType(Double))
            .Add("OCESS", GetType(Double))
            If MetalBasedStone Then
                .Add("TAGSNO", GetType(String))
            End If
        End With
        With gridMultimetal
            .DataSource = dtGridMultiMetal
            For i As Integer = 0 To .ColumnCount - 1
                If .Columns(i).ValueType.Name Is GetType(Decimal).Name Then
                    .Columns(i).DefaultCellStyle.Format = "0.000"
                    .Columns(i).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                ElseIf .Columns(i).ValueType.Name Is GetType(Double).Name Then
                    .Columns(i).DefaultCellStyle.Format = "0.00"
                    .Columns(i).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                ElseIf .Columns(i).ValueType.Name Is GetType(Integer).Name Then
                    .Columns(i).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End If
                .Columns(i).Resizable = DataGridViewTriState.False
            Next
        End With

        StyleGridMultiMetal(gridMultimetal)
        Dim dtGridMultiMetalTotal As New DataTable
        dtGridMultiMetalTotal = dtGridMultiMetal.Copy
        dtGridMultiMetalTotal.Rows.Clear()
        dtGridMultiMetalTotal.Rows.Add()
        dtGridMultiMetalTotal.Rows(0).Item("CATEGORY") = "Total"
        dtGridMultiMetalTotal.AcceptChanges()
        With gridMultiMetalTotal
            .DataSource = dtGridMultiMetalTotal
            For Each col As DataGridViewColumn In gridMultimetal.Columns
                With gridMultiMetalTotal.Columns(col.Name)
                    .Visible = col.Visible
                    .Width = col.Width
                    .DefaultCellStyle = col.DefaultCellStyle
                End With
            Next
        End With
        CalcGridMultiMetalTotal()
        StyleGridMultiMetal(gridMultiMetalTotal)
    End Sub
    Public Sub StyleGridMultiMetal(ByVal grid As DataGridView)
        Dim _temploc As Integer = 0
        With grid
            .Columns("CATEGORY").Width = txtMMCategory.Width + 1
            .Columns("WEIGHT").Width = txtMMWeight_Wet.Width + 1
            If MetalBasedStone Then
                With .Columns("NETWT")
                    .HeaderText = "NETWT"
                    .Width = txtMMNetWeight_Wet.Width + 1
                    .DefaultCellStyle.Format = "0.000"
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End With
                .Columns("TAGSNO").Visible = False
            Else
                _temploc = Val(txtMMRate.Location.X) - Val(txtMMNetWeight_Wet.Location.X)
                lblMMRate.Left = Val(lblMMRate.Location.X) - Val(_temploc)
                txtMMRate.Left = Val(txtMMRate.Location.X) - Val(_temploc)
                lblMMWastPer.Left = Val(lblMMWastPer.Location.X) - Val(_temploc)
                txtMMWastagePer_PER.Left = Val(txtMMWastagePer_PER.Location.X) - Val(_temploc)
                lblMMwastage.Left = Val(lblMMwastage.Location.X) - Val(_temploc)
                txtMMWastage_WET.Left = Val(txtMMWastage_WET.Location.X) - Val(_temploc)
                lblMMMcGrm.Left = Val(lblMMMcGrm.Location.X) - Val(_temploc)
                txtMMMcPerGRm_AMT.Left = Val(txtMMMcPerGRm_AMT.Location.X) - Val(_temploc)
                lblMMMc.Left = Val(lblMMMc.Location.X) - Val(_temploc)
                txtMMMc_AMT.Left = Val(txtMMMc_AMT.Location.X) - Val(_temploc)
                lblMMGrossAmt.Left = Val(lblMMGrossAmt.Location.X) - Val(_temploc)
                txtMMAmount_AMT.Left = Val(txtMMAmount_AMT.Location.X) - Val(_temploc)
                lblMMTax.Left = Val(lblMMTax.Location.X) - Val(_temploc)
                txtMMVAT_AMT.Left = Val(txtMMVAT_AMT.Location.X) - Val(_temploc)
                lblMMNetAmt.Left = Val(lblMMNetAmt.Location.X) - Val(_temploc)
                txtMMNet_AMT.Left = Val(txtMMNet_AMT.Location.X) - Val(_temploc)
            End If
            .Columns("WASTAGEPER").Width = txtMMWastagePer_PER.Width + 1
            .Columns("WASTAGE").Width = txtMMWastage_WET.Width + 1
            .Columns("MCPERGRM").Width = txtMMMcPerGRm_AMT.Width + 1
            .Columns("MC").Width = txtMMMc_AMT.Width + 1
            .Columns("AMOUNT").Width = txtMMAmount_AMT.Width + 1
            If .Columns.Contains("TRANTYPE") Then .Columns("TRANTYPE").Visible = False
            If .Columns.Contains("KEYNO") Then .Columns("KEYNO").Visible = False
            If .Columns.Contains("RATE") Then .Columns("RATE").Width = txtMMRate.Width + 1 : .Columns("RATE").Visible = True
            If .Columns.Contains("VATPER") Then .Columns("VATPER").Visible = False : .Columns("VAT").Width = txtMMVAT_AMT.Width + 1
            If .Columns.Contains("NETAMOUNT") Then .Columns("NETAMOUNT").Width = txtMMNet_AMT.Width + 1
            'If IsPOS Then
            .Columns("PURITYID").Visible = False
            .Columns("METALID").Visible = False
            .Columns("SGSTPER").Visible = False
            .Columns("CGSTPER").Visible = False
            .Columns("IGSTPER").Visible = False
            .Columns("SGST").Visible = False
            .Columns("CGST").Visible = False
            .Columns("IGST").Visible = False
            .Columns("CESSPER").Visible = False
            .Columns("CESS").Visible = False

            'End If

        End With
    End Sub
    Public Sub CalcGrsamt()
        Dim MVALUE As Decimal = 0
        Dim _MMlesswt As Decimal = 0
        Dim _MMisssno As String = ""
        If Ispartsale And Val(txtMMWeight_Wet.Text) > 0 And MetalBasedStone Then
            txtMMNetWeight_Wet.Text = Val(txtMMWeight_Wet.Text) - Val(lesswt)
        End If
        If MetalBasedStone And Val(txtMMNetWeight_Wet.Text) > 0 Then
            If MMgrsnet.ToUpper = "NET WT" Then
                MVALUE = Math.Round(((Val(txtMMNetWeight_Wet.Text) + Val(txtMMWastage_WET.Text)) * Val(txtMMRate.Text)) + Val(txtMMMc_AMT.Text), 2)
            Else
                MVALUE = Math.Round(((Val(txtMMWeight_Wet.Text) + Val(txtMMWastage_WET.Text)) * Val(txtMMRate.Text)) + Val(txtMMMc_AMT.Text), 2)
            End If
        Else
            MVALUE = Math.Round(((Val(txtMMWeight_Wet.Text) + Val(txtMMWastage_WET.Text)) * Val(txtMMRate.Text)) + Val(txtMMMc_AMT.Text), 2)
        End If
        Dim MVAT As Decimal = 0
        If Not txtMMVAT_AMT.Tag Is Nothing Then MVAT = Math.Round(MVALUE * (Val(txtMMVAT_AMT.Tag.ToString) / 100), 2)
        MVAT = Math.Round(MVALUE * (Val(GstPer) / 100), 2)
        txtMMAmount_AMT.Text = Math.Round(MVALUE, 2)
        txtMMVAT_AMT.Text = Math.Round(MVAT, 2)
        txtMMNet_AMT.Text = Math.Round(MVALUE + MVAT, 2)
    End Sub

    Private Function SaRatelock(ByVal mcatname As String) As Boolean
        Dim metalid As String = objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME= '" & mcatname & "'")
        If METALBASERATELOCK.Contains(metalid) = True Then Return True Else Return False
    End Function
    Private Function SaWastageLock(ByVal mcatname As String) As Boolean
        Dim metalid As String = objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME= '" & mcatname & "'")
        If GetAdmindbSoftValue("WASTLOCK_" & metalid, "N") = "Y" Then
            Return True
        End If
        Return False
    End Function

    Private Function SaMcLock(ByVal mcatname As String) As Boolean
        Dim metalid As String = objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME= '" & mcatname & "'")
        If GetAdmindbSoftValue("MCLOCK_" & metalid, "N") = "Y" Then
            Return True
        End If
        Return False
    End Function

    Private Function SaWastMcPerLock(ByVal mcatname As String) As Boolean
        Dim metalid As String = objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME= '" & mcatname & "'")
        If GetAdmindbSoftValue("WASTMCPERLOCK_" & metalid, "N") = "Y" Then
            Return True
        End If
        Return False
    End Function


    Public Sub CalcGridMultiMetalTotal(Optional ByVal Isedit As Boolean = False, Optional ByVal Vatper As Decimal = 0 _
    , Optional ByVal Sgstper As Decimal = 0, Optional ByVal CgstPer As Decimal = 0, Optional ByVal IgstPer As Decimal = 0, Optional ByVal CessPer As Decimal = 0)
        Dim wt As Decimal = Nothing
        Dim Gwt As Decimal = Nothing
        Dim amt As Double = Nothing
        Dim wast As Double = Nothing
        Dim vat As Double = Nothing
        Dim Netamt As Double = Nothing
        Dim mc As Double = Nothing
        Dim RndOff_Vat As String = GetAdmindbSoftValue("ROUNDOFF-VAT", "N")
        Dim RndOff_Gross As String = GetAdmindbSoftValue("ROUNDOFF-GROSS", "N")
        dtGridMultiMetal.AcceptChanges()
        'wt = Math.Round(wt * _touch / 100, 3)
        For Each ro As DataRow In dtGridMultiMetal.Rows
            'If IsPOS Then
            Dim MRATE As Decimal
            If Not Isedit Then MRATE = GetRate_Purity(Billdate, ro!purityid.ToString) Else MRATE = Val(ro!rate.ToString)
            If MRATE = 0 Then MRATE = Val(ro!rate.ToString)
            Dim MVALUE As Decimal = 0
            If MetalBasedStone Then
                If Val(ro!NETWT.ToString) > 0 Then
                    MVALUE = ((Val(ro!NETWT.ToString) + Val(ro!WASTAGE.ToString)) * MRATE) + Val(ro!MC.ToString)
                Else
                    MVALUE = ((Val(ro!WEIGHT.ToString) + Val(ro!WASTAGE.ToString)) * MRATE) + Val(ro!MC.ToString)
                End If
            Else
                MVALUE = ((Val(ro!WEIGHT.ToString) + Val(ro!WASTAGE.ToString)) * MRATE) + Val(ro!MC.ToString)
            End If

            If _touch <> 100 Then
                If MetalBasedStone Then
                    If Val(ro!NETWT.ToString) > 0 Then
                        MVALUE = (((Val(ro!NETWT.ToString) * _touch / 100) + Val(ro!WASTAGE.ToString)) * MRATE) + Val(ro!MC.ToString)
                    Else
                        MVALUE = (((Val(ro!WEIGHT.ToString) * _touch / 100) + Val(ro!WASTAGE.ToString)) * MRATE) + Val(ro!MC.ToString)
                    End If
                Else
                    MVALUE = (((Val(ro!WEIGHT.ToString) * _touch / 100) + Val(ro!WASTAGE.ToString)) * MRATE) + Val(ro!MC.ToString)
                End If
            End If
            MVALUE = CalcRoundoffAmt(MVALUE, RndOff_Gross)
            Dim MVAT As Decimal
            Dim SGST As Decimal
            Dim CGST As Decimal
            Dim IGST As Decimal
            Dim CESS As Decimal
            If Vatper <> 0 Then
                MVAT = MVALUE * (Vatper / 100)
                SGST = MVALUE * (Sgstper / 100)
                CGST = MVALUE * (CgstPer / 100)
                CESS = MVALUE * (CessPer / 100)
            Else
                MVAT = MVALUE * (Val(ro!VATPER.ToString) / 100)
                If InterStateBill Then
                    IGST = MVALUE * (Val(ro!IGSTPER.ToString) / 100)
                Else
                    SGST = MVALUE * (Val(ro!SGSTPER.ToString) / 100)
                    CGST = MVALUE * (Val(ro!CGSTPER.ToString) / 100)
                End If
                CESS = MVALUE * (Val(ro!CESSPER.ToString) / 100)
            End If
            'MVAT = Math.Round(MVAT, 2)
            'SGST = Math.Round(SGST, 2)
            'CGST = Math.Round(CGST, 2)
            If RndOff_Vat = "N" Then
                MVAT = Math.Round(MVAT, 2)
                SGST = Math.Round(SGST, 2)
                CGST = Math.Round(CGST, 2)
                IGST = Math.Round(IGST, 2)
                CESS = Math.Round(CESS, 2)
            End If
            MVAT = CalcRoundoffAmt(MVAT, RndOff_Vat)
            SGST = CalcRoundoffAmt(SGST, RndOff_Vat)
            CGST = CalcRoundoffAmt(CGST, RndOff_Vat)
            IGST = CalcRoundoffAmt(IGST, RndOff_Vat)
            CESS = CalcRoundoffAmt(CESS, RndOff_Vat)
            If GST Then
                ro!SGST = SGST
                ro!CGST = CGST
                ro!IGST = IGST
                ro!CESS = CESS
                ro!VAT = CGST + SGST + IGST + CESS
                ro!OSGST = SGST
                ro!OCGST = CGST
                ro!OIGST = IGST
                ro!OCESS = CESS
                ro!OVAT = CGST + SGST + IGST + CESS
            Else
                ro!VAT = MVAT
                ro!OVAT = MVAT
            End If
            If Val(Vatper.ToString) > 0 And Val(Sgstper.ToString) > 0 And Val(CgstPer.ToString) > 0 Then
                ro!VATPER = Val(Vatper.ToString) 'Newly add
                ro!SGSTPER = Val(Sgstper.ToString)
                ro!CGSTPER = Val(CgstPer.ToString)
                ro!IGSTPER = Val(IgstPer.ToString)
                ro!CESSPER = Val(CessPer.ToString)
            End If
            ro!RATE = MRATE
            ro!AMOUNT = MVALUE
            ro!OAMOUNT = MVALUE
            ro!NETAMOUNT = MVALUE + Val(ro!VAT.ToString)
            wast += Val(ro!WASTAGE.ToString)
            vat += Val(ro!VAT.ToString)
            Netamt += Val(ro!NETAMOUNT.ToString)
            mc += Val(ro!mc.ToString)
            'End If
            Gwt += Val(ro!WEIGHT.ToString)
            If MetalBasedStone Then
                If Val(ro!NETWT.ToString) > 0 Then
                    wt += Val(ro!NETWT.ToString)
                Else
                    wt += Val(ro!WEIGHT.ToString)
                End If
            Else
                wt += Val(ro!WEIGHT.ToString)
            End If

            amt += Val(ro!AMOUNT.ToString)
        Next
        dtGridMultiMetal.AcceptChanges()
        If gridMultiMetalTotal.RowCount > 0 Then
            gridMultiMetalTotal.Rows(0).Cells("WEIGHT").Value = IIf(Gwt <> 0, Gwt, DBNull.Value)
            If MetalBasedStone Then
                gridMultiMetalTotal.Rows(0).Cells("NETWT").Value = IIf(wt <> 0, wt, DBNull.Value)
            End If
            gridMultiMetalTotal.Rows(0).Cells("AMOUNT").Value = IIf(amt <> 0, amt, DBNull.Value)
            gridMultiMetalTotal.Rows(0).Cells("RATE").Value = IIf(amt / (wt + wast) <> 0, amt / (wt + wast), DBNull.Value)
            gridMultiMetalTotal.Rows(0).Cells("WASTAGE").Value = IIf(wast <> 0, wast, DBNull.Value)
            gridMultiMetalTotal.Rows(0).Cells("OWASTAGE").Value = IIf(wast <> 0, wast, DBNull.Value)
            gridMultiMetalTotal.Rows(0).Cells("MC").Value = IIf(mc <> 0, mc, DBNull.Value)
            gridMultiMetalTotal.Rows(0).Cells("OMC").Value = IIf(mc <> 0, mc, DBNull.Value)
            gridMultiMetalTotal.Rows(0).Cells("VAT").Value = IIf(vat <> 0, vat, DBNull.Value)
            gridMultiMetalTotal.Rows(0).Cells("NETAMOUNT").Value = IIf(Netamt <> 0, Netamt, DBNull.Value)
        End If
    End Sub

    Private Sub txtMMWastagePer_PER_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtMMWastagePer_PER.KeyDown
        If e.KeyCode = Keys.Down Then
            If gridMultimetal.RowCount > 0 Then
                gridMultimetal.Focus()
            End If
        End If
    End Sub
    Private Sub txtMMWastagePer_PER_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMMWastagePer_PER.TextChanged
        Dim wt As Double = 0
        If MetalBasedStone And Val(txtMMNetWeight_Wet.Text) > 0 Then
            wt = Val(txtMMNetWeight_Wet.Text) * (Val(txtMMWastagePer_PER.Text) / 100)
        Else
            wt = Val(txtMMWeight_Wet.Text) * (Val(txtMMWastagePer_PER.Text) / 100)
        End If

        txtMMWastage_WET.Text = IIf(wt <> 0, Format(wt, "0.000"), "")
    End Sub

    Private Sub txtMMWastage_WET_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMMWastage_WET.GotFocus
        If Val(txtMMWastagePer_PER.Text) > 0 Then
            SendKeys.Send("{TAB}")
        End If
        If SaWastageLock(txtMMCategory.Text) Then Exit Sub
    End Sub

    Private Sub txtMMWastage_WET_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtMMWastage_WET.KeyDown
        If e.KeyCode = Keys.Down Then
            If gridMultimetal.RowCount > 0 Then
                gridMultimetal.Focus()
            End If
        End If
    End Sub
    Private Sub txtMMWastage_WET_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtMMWastage_WET.KeyPress
        If Val(txtMMWastagePer_PER.Text) > 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtMMMcPerGRm_AMT_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtMMMcPerGRm_AMT.KeyDown
        If e.KeyCode = Keys.Down Then
            If gridMultimetal.RowCount > 0 Then
                gridMultimetal.Focus()
            End If
        End If
    End Sub
    Private Sub txtMMMcPerGRm_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMMMcPerGRm_AMT.TextChanged
        Dim mc As Double = 0
        If MetalBasedStone And Val(txtMMNetWeight_Wet.Text) > 0 Then
            mc = Val(txtMMNetWeight_Wet.Text) * Val(txtMMMcPerGRm_AMT.Text)
        Else
            mc = Val(txtMMWeight_Wet.Text) * Val(txtMMMcPerGRm_AMT.Text)
        End If
        txtMMMc_AMT.Text = IIf(mc <> 0, Format(mc, "0.00"), "")
    End Sub

    Private Sub txtMMMc_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMMMc_AMT.GotFocus
        If Val(txtMMMcPerGRm_AMT.Text) > 0 Then
            SendKeys.Send("{TAB}")
            Exit Sub
        End If
        If SaMcLock(txtMMCategory.Text) Then Exit Sub
    End Sub

    Private Sub txtMMMc_AMT_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtMMMc_AMT.KeyDown
        If e.KeyCode = Keys.Down Then
            If gridMultimetal.RowCount > 0 Then
                gridMultimetal.Focus()
            End If
        End If
    End Sub
    Private Sub txtMMMc_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtMMMc_AMT.KeyPress
        If Val(txtMMMcPerGRm_AMT.Text) > 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtMMAmount_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMMAmount_AMT.GotFocus

    End Sub
    Private Sub txtMMAmount_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtMMAmount_AMT.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtMMCategory.Text = "" Then
                MsgBox("Category Should Not Empty", MsgBoxStyle.Information)
                txtMMCategory.Focus()
                Exit Sub
            ElseIf objGPack.DupCheck("SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & txtMMCategory.Text & "'") = False Then
                MsgBox("Invalid Category", MsgBoxStyle.Information)
                txtMMCategory.Focus()
                Exit Sub
            ElseIf (Not Val(txtMMWeight_Wet.Text) > 0) And (Not Val(txtMMAmount_AMT.Text)) Then
                MsgBox("Weight,Rate,Amount Should not Empty", MsgBoxStyle.Information)
                txtMMWeight_Wet.Focus()
                Exit Sub
            End If
            Dim Isedit As Boolean = False
            If txtMMRowIndex.Text <> "" Then
                With dtGridMultiMetal.Rows(Val(txtMMRowIndex.Text))
                    .Item("CATEGORY") = txtMMCategory.Text
                    .Item("WEIGHT") = IIf(Val(txtMMWeight_Wet.Text) <> 0, Val(txtMMWeight_Wet.Text), DBNull.Value)
                    If MetalBasedStone Then
                        .Item("NETWT") = IIf(Val(txtMMNetWeight_Wet.Text) <> 0, Val(txtMMNetWeight_Wet.Text), DBNull.Value)
                    End If
                    .Item("WASTAGEPER") = IIf(Val(txtMMWastagePer_PER.Text) <> 0, Val(txtMMWastagePer_PER.Text), DBNull.Value)
                    .Item("WASTAGE") = IIf(Val(txtMMWastage_WET.Text) <> 0, Val(txtMMWastage_WET.Text), DBNull.Value)
                    .Item("OWASTAGE") = IIf(Val(txtMMWastage_WET.Text) <> 0, Val(txtMMWastage_WET.Text), DBNull.Value)
                    .Item("MCPERGRM") = IIf(Val(txtMMMcPerGRm_AMT.Text) <> 0, Val(txtMMMcPerGRm_AMT.Text), DBNull.Value)
                    .Item("MC") = IIf(Val(txtMMMc_AMT.Text) <> 0, Val(txtMMMc_AMT.Text), DBNull.Value)
                    .Item("OMC") = IIf(Val(txtMMMc_AMT.Text) <> 0, Val(txtMMMc_AMT.Text), DBNull.Value)
                    .Item("AMOUNT") = IIf(Val(txtMMAmount_AMT.Text) <> 0, Val(txtMMAmount_AMT.Text), DBNull.Value)
                    .Item("OAMOUNT") = IIf(Val(txtMMAmount_AMT.Text) <> 0, Val(txtMMAmount_AMT.Text), DBNull.Value)
                    .Item("RATE") = IIf(Val(txtMMRate.Text) <> 0, Val(txtMMRate.Text), DBNull.Value)
                    .Item("VAT") = IIf(Val(txtMMVAT_AMT.Text) <> 0, Val(txtMMVAT_AMT.Text), DBNull.Value)
                    .Item("OVAT") = IIf(Val(txtMMVAT_AMT.Text) <> 0, Val(txtMMVAT_AMT.Text), DBNull.Value)
                    .Item("NETAMOUNT") = IIf(Val(txtMMNet_AMT.Text) <> 0, Val(txtMMNet_AMT.Text), DBNull.Value)
                    dtGridMultiMetal.AcceptChanges()
                    Isedit = True
                    GoTo AFTERINSERT
                End With
            End If
            Dim ro As DataRow = Nothing
            ro = dtGridMultiMetal.NewRow
            ro("CATEGORY") = txtMMCategory.Text
            ro("WEIGHT") = IIf(Val(txtMMWeight_Wet.Text) <> 0, Val(txtMMWeight_Wet.Text), DBNull.Value)
            If MetalBasedStone Then
                ro("NETWT") = IIf(Val(txtMMNetWeight_Wet.Text) <> 0, Val(txtMMNetWeight_Wet.Text), DBNull.Value)
            End If
            ro("WASTAGEPER") = IIf(Val(txtMMWastagePer_PER.Text) <> 0, Val(txtMMWastagePer_PER.Text), DBNull.Value)
            ro("WASTAGE") = IIf(Val(txtMMWastage_WET.Text) <> 0, Val(txtMMWastage_WET.Text), DBNull.Value)
            ro("OWASTAGE") = IIf(Val(txtMMWastage_WET.Text) <> 0, Val(txtMMWastage_WET.Text), DBNull.Value)
            ro("MCPERGRM") = IIf(Val(txtMMMcPerGRm_AMT.Text) <> 0, Val(txtMMMcPerGRm_AMT.Text), DBNull.Value)
            ro("MC") = IIf(Val(txtMMMc_AMT.Text) <> 0, Val(txtMMMc_AMT.Text), DBNull.Value)
            ro("OMC") = IIf(Val(txtMMMc_AMT.Text) <> 0, Val(txtMMMc_AMT.Text), DBNull.Value)
            ro("AMOUNT") = IIf(Val(txtMMAmount_AMT.Text) <> 0, Val(txtMMAmount_AMT.Text), DBNull.Value)
            ro("OAMOUNT") = IIf(Val(txtMMAmount_AMT.Text) <> 0, Val(txtMMAmount_AMT.Text), DBNull.Value)
            ro("RATE") = IIf(Val(txtMMRate.Text) <> 0, Val(txtMMRate.Text), DBNull.Value)
            ro("VAT") = IIf(Val(txtMMVAT_AMT.Text) <> 0, Val(txtMMVAT_AMT.Text), DBNull.Value)
            ro("OVAT") = IIf(Val(txtMMVAT_AMT.Text) <> 0, Val(txtMMVAT_AMT.Text), DBNull.Value)
            ro("NETAMOUNT") = IIf(Val(txtMMNet_AMT.Text) <> 0, Val(txtMMNet_AMT.Text), DBNull.Value)
            dtGridMultiMetal.Rows.Add(ro)
            dtGridMultiMetal.AcceptChanges()
            gridMultimetal.CurrentCell = gridMultimetal.Rows(gridMultimetal.RowCount - 1).Cells("CATEGORY")
AFTERINSERT:
            If GST Then
                Dim dr As DataRow
                strSql = "SELECT S_SGSTTAX,S_CGSTTAX,S_IGSTTAX,CESSTAX FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME='" & txtMMCategory.Text & "'"
                dr = GetSqlRow(strSql, cn)
                If Not dr Is Nothing Then
                    CalcGridMultiMetalTotal(Isedit, Val(dr("S_SGSTTAX").ToString) + Val(dr("S_CGSTTAX").ToString), Val(dr("S_SGSTTAX").ToString), Val(dr("S_CGSTTAX").ToString), Val(dr("S_IGSTTAX").ToString), Val(dr("CESSTAX").ToString))
                Else
                    CalcGridMultiMetalTotal(Isedit)
                End If
            Else
                CalcGridMultiMetalTotal(Isedit)
            End If
            objGPack.TextClear(grpMultiMetal)
            txtMMCategory.Select()
        End If
    End Sub

    Private Sub frmMultiMetalDia_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            dtGridMultiMetal.AcceptChanges()
            txtMMCategory.Select()
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
        'If e.Control Then
        '    If e.KeyCode = Keys.R Then
        '        tstripRatechange()
        '        txtMMRate.Focus()
        '    End If
        'End If
    End Sub

    Private Sub frmMultiMetalDia_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtMMCategory.Focused Then Exit Sub
            If txtMMAmount_AMT.Focused Then Exit Sub
            If gridMultimetal.Focused Then Exit Sub
            If txtMMNet_AMT.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtMMCategory_MAN_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMMCategory.GotFocus
        Main.ShowHelpText("Press Insert Key to Help")
    End Sub

    Private Sub txtMMCategory_MAN_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtMMCategory.KeyDown
        If e.KeyCode = Keys.Insert Then
            LoadCatName()
        ElseIf e.KeyCode = Keys.Down Then
            If gridMultimetal.RowCount > 0 Then
                gridMultimetal.Focus()
            End If
        End If
    End Sub

    Private Sub txtMMCategory_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtMMCategory.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtMMCategory.Text = "" Then
                LoadCatName()
            ElseIf txtMMCategory.Text <> "" And objGPack.DupCheck("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & txtMMCategory.Text & "'") = False Then
                LoadCatName()
            Else
                LoadCatDetails()
            End If
        End If
    End Sub

    Private Sub txtMMCategory_MAN_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMMCategory.LostFocus
        Main.HideHelpText()
    End Sub
    Private Sub LoadCatName()
        strSql = " SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY "
        strSql += " WHERE PURITYID IN (SELECT PURITYID FROM " & cnAdminDb & "..PURITYMAST WHERE METALTYPE = 'O')"
        strSql += " ORDER BY CATNAME"
        Dim catName As String = BrighttechPack.SearchDialog.Show("Find CatName", strSql, cn, , , , txtMMCategory.Text)
        If catName <> "" Then
            txtMMCategory.Text = catName
            LoadCatDetails()
        Else
            txtMMCategory.Select()
            txtMMCategory.SelectAll()
        End If
    End Sub
    Private Sub LoadCatDetails()
        If txtMMCategory.Text <> "" Then
            txtMMRate.Text = Val(GetRate(GetEntryDate(GetServerDate), objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & txtMMCategory.Text & "'")))
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtMMRate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtMMRate.KeyDown
        If e.KeyCode = Keys.Down Then
            If gridMultimetal.RowCount > 0 Then
                gridMultimetal.Focus()
            End If
        End If
    End Sub

    Private Sub txtMMWeight_Wet_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMMWeight_Wet.GotFocus
        'If Ispartsale Then txtMMWeight_Wet.ReadOnly = False Else txtMMWeight_Wet.ReadOnly = True : Exit Sub
    End Sub

    Private Sub txtMMWeight_Wet_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtMMWeight_Wet.KeyDown
        If e.KeyCode = Keys.Down Then
            If gridMultimetal.RowCount > 0 Then
                gridMultimetal.Focus()
            End If
        End If
    End Sub

    Private Sub gridMultimetal_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridMultimetal.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
        End If
    End Sub

    Private Sub gridMultimetal_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridMultimetal.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If gridMultimetal.RowCount > 0 Then
                gridMultimetal.CurrentCell = gridMultimetal.CurrentRow.Cells("CATEGORY")
                With gridMultimetal.Rows(gridMultimetal.CurrentRow.Index)
                    txtMMCategory.Text = .Cells("CATEGORY").FormattedValue
                    txtMMWeight_Wet.Text = .Cells("WEIGHT").FormattedValue
                    If MetalBasedStone Then
                        txtMMNetWeight_Wet.Text = .Cells("NETWT").FormattedValue
                    End If
                    txtMMRate.Text = .Cells("RATE").FormattedValue
                    txtMMWastagePer_PER.Text = .Cells("WASTAGEPER").FormattedValue
                    txtMMWastage_WET.Text = .Cells("WASTAGE").FormattedValue
                    txtMMMcPerGRm_AMT.Text = .Cells("MCPERGRM").FormattedValue
                    txtMMMc_AMT.Text = .Cells("MC").FormattedValue
                    txtMMAmount_AMT.Text = .Cells("AMOUNT").FormattedValue
                    txtMMVAT_AMT.Text = .Cells("vat").FormattedValue
                    txtMMVAT_AMT.Tag = Val(.Cells("vatper").Value.ToString)
                    GstPer = Val(.Cells("vatper").Value.ToString)
                    txtMMNet_AMT.Text = .Cells("NETAMOUNT").FormattedValue
                    txtMMRowIndex.Text = gridMultimetal.CurrentRow.Index
                    txtMMWeight_Wet.Focus()
                End With
            End If
        End If
    End Sub

    Private Sub gridMultimetal_UserDeletedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles gridMultimetal.UserDeletedRow
        dtGridMultiMetal.AcceptChanges()
        CalcGridMultiMetalTotal()
        If Not gridMultimetal.RowCount > 0 Then txtMMCategory.Focus()
    End Sub

    Private Sub frmMultiMetalDia_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        gridMultimetal.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        gridMultiMetalTotal.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        gridMultiMetalTotal.DefaultCellStyle.BackColor = grpMultiMetal.BackgroundColor
        gridMultiMetalTotal.DefaultCellStyle.SelectionBackColor = grpMultiMetal.BackgroundColor
        If GST Then
            lblMMTax.Text = "GST"
        End If
    End Sub

    Private Sub txtMMRate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMMRate.TextChanged
        CalcGrsamt()
    End Sub

    Private Sub txtMMWastage_WET_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMMWastage_WET.TextChanged
        CalcGrsamt()
    End Sub

    Private Sub txtMMMc_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMMMc_AMT.TextChanged
        CalcGrsamt()
    End Sub

    Private Sub tstripRatechange()
        If RateChangeF4 Then
            txtMMRate.ReadOnly = False
            txtMMRate.Focus()
        Else
            txtMMRate.ReadOnly = True
        End If
    End Sub

    Private Sub RateChangeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RateChangeToolStripMenuItem.Click
        tstripRatechange()
        txtMMRate.Focus()
    End Sub

    Private Sub txtMMNet_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtMMNet_AMT.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtMMCategory.Text = "" Then
                MsgBox("Category Should Not Empty", MsgBoxStyle.Information)
                txtMMCategory.Focus()
                Exit Sub
            ElseIf objGPack.DupCheck("SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & txtMMCategory.Text & "'") = False Then
                MsgBox("Invalid Category", MsgBoxStyle.Information)
                txtMMCategory.Focus()
                Exit Sub
            ElseIf (Not Val(txtMMWeight_Wet.Text) > 0) And (Not Val(txtMMAmount_AMT.Text)) Then
                MsgBox("Weight,Rate,Amount Should not Empty", MsgBoxStyle.Information)
                txtMMWeight_Wet.Focus()
                Exit Sub
            End If
            Dim Isedit As Boolean = False
            If txtMMRowIndex.Text <> "" Then
                With dtGridMultiMetal.Rows(Val(txtMMRowIndex.Text))
                    .Item("CATEGORY") = txtMMCategory.Text
                    .Item("WEIGHT") = IIf(Val(txtMMWeight_Wet.Text) <> 0, Val(txtMMWeight_Wet.Text), DBNull.Value)
                    If MetalBasedStone Then
                        .Item("NETWT") = IIf(Val(txtMMNetWeight_Wet.Text) <> 0, Val(txtMMNetWeight_Wet.Text), DBNull.Value)
                    End If
                    .Item("WASTAGEPER") = IIf(Val(txtMMWastagePer_PER.Text) <> 0, Val(txtMMWastagePer_PER.Text), DBNull.Value)
                    .Item("WASTAGE") = IIf(Val(txtMMWastage_WET.Text) <> 0, Val(txtMMWastage_WET.Text), DBNull.Value)
                    .Item("OWASTAGE") = IIf(Val(txtMMWastage_WET.Text) <> 0, Val(txtMMWastage_WET.Text), DBNull.Value)
                    .Item("MCPERGRM") = IIf(Val(txtMMMcPerGRm_AMT.Text) <> 0, Val(txtMMMcPerGRm_AMT.Text), DBNull.Value)
                    .Item("MC") = IIf(Val(txtMMMc_AMT.Text) <> 0, Val(txtMMMc_AMT.Text), DBNull.Value)
                    .Item("OMC") = IIf(Val(txtMMMc_AMT.Text) <> 0, Val(txtMMMc_AMT.Text), DBNull.Value)
                    .Item("AMOUNT") = IIf(Val(txtMMAmount_AMT.Text) <> 0, Val(txtMMAmount_AMT.Text), DBNull.Value)
                    .Item("OAMOUNT") = IIf(Val(txtMMAmount_AMT.Text) <> 0, Val(txtMMAmount_AMT.Text), DBNull.Value)
                    .Item("RATE") = IIf(Val(txtMMRate.Text) <> 0, Val(txtMMRate.Text), DBNull.Value)
                    .Item("VAT") = IIf(Val(txtMMVAT_AMT.Text) <> 0, Val(txtMMVAT_AMT.Text), DBNull.Value)
                    .Item("OVAT") = IIf(Val(txtMMVAT_AMT.Text) <> 0, Val(txtMMVAT_AMT.Text), DBNull.Value)
                    .Item("NETAMOUNT") = IIf(Val(txtMMNet_AMT.Text) <> 0, Val(txtMMNet_AMT.Text), DBNull.Value)
                    dtGridMultiMetal.AcceptChanges()
                    Isedit = True
                    GoTo AFTERINSERT
                End With
            End If
            Dim ro As DataRow = Nothing
            ro = dtGridMultiMetal.NewRow
            ro("CATEGORY") = txtMMCategory.Text
            ro("WEIGHT") = IIf(Val(txtMMWeight_Wet.Text) <> 0, Val(txtMMWeight_Wet.Text), DBNull.Value)
            If MetalBasedStone Then
                ro("NETWT") = IIf(Val(txtMMNetWeight_Wet.Text) <> 0, Val(txtMMNetWeight_Wet.Text), DBNull.Value)
            End If
            ro("WASTAGEPER") = IIf(Val(txtMMWastagePer_PER.Text) <> 0, Val(txtMMWastagePer_PER.Text), DBNull.Value)
            ro("WASTAGE") = IIf(Val(txtMMWastage_WET.Text) <> 0, Val(txtMMWastage_WET.Text), DBNull.Value)
            ro("OWASTAGE") = IIf(Val(txtMMWastage_WET.Text) <> 0, Val(txtMMWastage_WET.Text), DBNull.Value)
            ro("MCPERGRM") = IIf(Val(txtMMMcPerGRm_AMT.Text) <> 0, Val(txtMMMcPerGRm_AMT.Text), DBNull.Value)
            ro("MC") = IIf(Val(txtMMMc_AMT.Text) <> 0, Val(txtMMMc_AMT.Text), DBNull.Value)
            ro("OMC") = IIf(Val(txtMMMc_AMT.Text) <> 0, Val(txtMMMc_AMT.Text), DBNull.Value)
            ro("AMOUNT") = IIf(Val(txtMMAmount_AMT.Text) <> 0, Val(txtMMAmount_AMT.Text), DBNull.Value)
            ro("OAMOUNT") = IIf(Val(txtMMAmount_AMT.Text) <> 0, Val(txtMMAmount_AMT.Text), DBNull.Value)
            ro("RATE") = IIf(Val(txtMMRate.Text) <> 0, Val(txtMMRate.Text), DBNull.Value)
            ro("VAT") = IIf(Val(txtMMVAT_AMT.Text) <> 0, Val(txtMMVAT_AMT.Text), DBNull.Value)
            ro("OVAT") = IIf(Val(txtMMVAT_AMT.Text) <> 0, Val(txtMMVAT_AMT.Text), DBNull.Value)
            ro("NETAMOUNT") = IIf(Val(txtMMNet_AMT.Text) <> 0, Val(txtMMNet_AMT.Text), DBNull.Value)
            dtGridMultiMetal.Rows.Add(ro)
            dtGridMultiMetal.AcceptChanges()
            gridMultimetal.CurrentCell = gridMultimetal.Rows(gridMultimetal.RowCount - 1).Cells("CATEGORY")
AFTERINSERT:
            CalcGridMultiMetalTotal(Isedit)

            objGPack.TextClear(grpMultiMetal)
            If MetalBasedStone Then
                txtMMNetWeight_Wet.Text = ""
            End If
            txtMMCategory.Select()
        End If
    End Sub

    Private Sub txtMMWeight_Wet_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMMWeight_Wet.TextChanged
        CalcGrsamt()
    End Sub
End Class