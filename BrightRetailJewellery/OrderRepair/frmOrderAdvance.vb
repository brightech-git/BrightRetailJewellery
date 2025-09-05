Imports System.Data.OleDb
Public Class frmOrderAdvance
    Public dtGridPur As New DataTable
    Dim strSql As String
    Public FurtherAdvance As String = ""
    Dim BillDate As Date = GetEntryDate(GetServerDate)
    Dim fromFormName As String
    Dim PurAlloy As Decimal
    Dim CASHID As String
    Dim CTLIDS As String = "Where ctlid in ('ROUNDOFF-WASTAGE','PUR22KT','PUR22KT_TYPE','PURALLOYPER','ESTCALLING')"
    Dim dtSoftKeyss As DataTable = GetAdmindbSoftValueAll(CTLIDS)
    Dim WastageRound As Integer = Val(GetAdmindbSoftValuefromDt(dtSoftKeyss, "ROUNDOFF-WASTAGE", "3"))
    Dim PUR22KT As Decimal = Val(GetAdmindbSoftValuefromDt(dtSoftKeyss, "PUR22KT", "0"))
    Dim PUR22KT_TYPE As Decimal = Val(GetAdmindbSoftValuefromDt(dtSoftKeyss, "PUR22KT_TYPE", "0"))
    Dim RndFinalAmt As String = GetAdmindbSoftValue("ROUNDOFF-FINAL", "N")
    Dim PURALLOYPER As Decimal = Val(GetAdmindbSoftValuefromDt(dtSoftKeyss, "PURALLOYPER", "0"))
    Dim ORDER_OLD_CAT As String = Nothing
    Dim ObjPurchaseEstNo As New FRM_ESTIMATENO
    Dim HasEst As Boolean = IIf(GetAdmindbSoftValuefromDt(dtSoftKeyss, "ESTCALLING", "N") = "Y", True, False)
    Dim CatViewFilter As Boolean = IIf(GetAdmindbSoftValue("CAT_VIEW_FILTER", "N") = "Y", True, False)
    Dim MetalId As String = "G"
    Dim GSTADVCALC_INCL As String = GetAdmindbSoftValue("GSTADVCALC", "N")
    Dim RndGst As String = GetAdmindbSoftValue("ROUNDOFF-GST", "N")
    Public objStone As New frmStoneDia
    Dim dtStoneDetails As New DataTable
    Public Property pMetalId() As String
        Get
            Return MetalId
        End Get
        Set(ByVal value As String)
            MetalId = value
            If MetalId = "S" Then
                ORDER_OLD_CAT = objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = '" & GetAdmindbSoftValue("ORDER_OLD_CAT_S", "00013") & "'", , "00013")
            End If
        End Set
    End Property
    Public Sub New(ByVal Start As Boolean)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
        objGPack.TextClear(Me)
    End Sub
    Public Sub New(ByVal CashAccode As String)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        CASHID = CashAccode
        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
        objGPack.TextClear(Me)
        'Me.fromFormName = fromFormName
        ''Purchase Group
        ORDER_OLD_CAT = objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = '" & GetAdmindbSoftValue("ORDER_OLD_CAT", "00011") & "'", , "00011")

        With dtGridPur
            .Columns.Add("MODE", GetType(String))
            .Columns.Add("CATNAME", GetType(String))
            .Columns.Add("PURITY", GetType(Double))
            .Columns.Add("PCS", GetType(Integer))
            .Columns.Add("GRSWT", GetType(Decimal))
            .Columns.Add("DUSTWT", GetType(Decimal))
            .Columns.Add("WASTAGEPER", GetType(Decimal))
            .Columns.Add("WASTAGE", GetType(Decimal))
            .Columns.Add("LESSWT", GetType(Decimal))
            .Columns.Add("NETWT", GetType(Decimal))
            .Columns.Add("RATE", GetType(Double))
            .Columns.Add("VAT", GetType(Double))
            .Columns.Add("AMOUNT", GetType(Double))
            .Columns.Add("ALLOYWT", GetType(Decimal))
            .Columns.Add("ESTNO", GetType(Integer))
            .Columns.Add("FLAG", GetType(String))
            .Columns.Add("EMPID", GetType(Integer))
            .Columns.Add("REMARK1", GetType(String))
            .Columns.Add("BOARDRATE", GetType(Decimal))
            .Columns.Add("ITEM", GetType(Integer))
            .Columns.Add("SUBITEM", GetType(Integer))
            .Columns.Add("ITEMTYPEID", GetType(Integer))
            .Columns.Add("MELTWT", GetType(Decimal))
            .Columns.Add("GST", GetType(Double))

            Dim col As New DataColumn("KEYNO")
            col.AutoIncrement = True
            col.AutoIncrementSeed = 1
            col.AutoIncrementStep = 1
            .Columns.Add(col)
            .Columns.Add("ENTFLAG", GetType(String))
            .Columns.Add("GROSSAMT", GetType(Double))
        End With
        dtGridPur.AcceptChanges()
        gridPur.DataSource = dtGridPur
        gridPur.ColumnHeadersVisible = False
        FormatGridColumns(gridPur)
        ClearDtGrid(dtGridPur)
        StyleGridPur(gridPur)

        ''Stone
        With dtStoneDetails.Columns
            .Add("KEYNO", GetType(Integer))
            .Add("TRANTYPE", GetType(String))
            .Add("ITEM", GetType(String))
            .Add("SUBITEM", GetType(String))
            .Add("PCS", GetType(Int32))
            .Add("WEIGHT", GetType(Decimal))
            .Add("UNIT", GetType(String))
            .Add("CALC", GetType(String))
            .Add("RATE", GetType(Double))
            .Add("AMOUNT", GetType(Double))
            .Add("METALID", GetType(String))
            .Add("DISCOUNT", GetType(Double))
            .Add("TAGSTNPCS", GetType(Integer))
            .Add("TAGSTNWT", GetType(Decimal))
            .Add("TAGSNO", GetType(String))
            .Add("R_VAT", GetType(Decimal))
            .Add("ISSSNO", GetType(String))
            .Add("CUTID", GetType(Integer))
            .Add("COLORID", GetType(Integer))
            .Add("CLARITYID", GetType(Integer))
            .Add("SHAPEID", GetType(Integer))
            .Add("SETTYPEID", GetType(Integer))
            .Add("HEIGHT", GetType(Decimal))
            .Add("WIDTH", GetType(Decimal))
            .Add("SGST", GetType(Decimal))
            .Add("CGST", GetType(Decimal))
            .Add("IGST", GetType(Decimal))
        End With

        Dim dtgridPurTotal As New DataTable
        dtgridPurTotal = dtGridPur.Copy
        dtgridPurTotal.Rows.Clear()
        dtgridPurTotal.Rows.Add()
        dtgridPurTotal.Rows.Add()
        gridPurTotal.ColumnHeadersVisible = False
        gridPurTotal.DataSource = dtgridPurTotal
        For Each col As DataGridViewColumn In gridPur.Columns
            With gridPurTotal.Columns(col.Name)
                .Visible = col.Visible
                .Width = col.Width
                .DefaultCellStyle = col.DefaultCellStyle
            End With
        Next
        CalcGridPurTotal()
        StyleGridPur(gridPurTotal)
        cmbPurMode.Items.Add("WEIGHT")
        cmbPurMode.Items.Add("AMOUNT")
        cmbPurMode.Items.Add("PURCHASE")
        cmbPurMode.Items.Add("EXCHANGE")

        ''PUR
        cmbPurMode.Text = "WEIGHT"
        ClearDtGrid(dtGridPur)
        CalcGridPurTotal()
        If GST Then
            Label18.Text = "GST"
        End If
    End Sub

    Private Sub CalcPUAmountAtVatChange()
        Dim stnAmt As Double = Nothing
        If objStone.gridStoneTotal.RowCount > 0 Then
            stnAmt = Val(objStone.gridStoneTotal.Rows(0).Cells("AMOUNT").Value.ToString)
        End If

        Dim grAmt As Double = Nothing
        grAmt = (Val(txtPUNetWt_WET.Text) * Val(txtPURate_AMT.Text)) + stnAmt

        Dim amt As Double = grAmt + Val(txtPUVat_AMT.Text)
        amt = CalcRoundoffAmt(amt, RndFinalAmt)
        txtPUAmount_AMT.Text = IIf(amt > 0, Format(amt, "0.00"), Nothing)
    End Sub


    Private Sub CalcPUAmount()
        Dim stnAmt As Double = Nothing
        If objStone.gridStoneTotal.RowCount > 0 Then
            stnAmt = Val(objStone.gridStoneTotal.Rows(0).Cells("AMOUNT").Value.ToString)
        End If
        Dim grAmt As Double = Nothing
        grAmt = Math.Round((Val(txtPUNetWt_WET.Text) * Val(txtPURate_AMT.Text)) + stnAmt, 2)
        If txtPUCategory.Text = "" Then Exit Sub
        Dim puTaxPer As Double = Nothing
        If GST Then
            strSql = " SELECT * FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & txtPUCategory.Text & "'"
            Dim dr As DataRow = GetSqlRow(strSql, cn)
            If Not dr Is Nothing Then
                puTaxPer = Val(dr("P_SGSTTAX").ToString)
                puTaxPer += Val(dr("P_CGSTTAX").ToString)
            End If
        Else
            strSql = " SELECT PTAX FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & txtPUCategory.Text & "'"
            puTaxPer = Val(objGPack.GetSqlValue(strSql))
        End If
        Dim tax As Double = (grAmt * (puTaxPer / 100))
        tax = CalcRoundoffAmt(tax, "H")
        Dim amt As Double = 0
        If GetAdmindbSoftValue("VBC_" & strCompanyId, "N").ToString = "Y" Then
            amt = grAmt
        Else
            amt = grAmt + Val(txtPUVat_AMT.Text)
            If cmbPurMode.Text <> "AMOUNT" Then txtPUVat_AMT.Text = IIf(tax <> 0, Format(tax, "0.00"), Nothing)
        End If
        'Dim amt As Double = grAmt + Val(txtPUVat_AMT.Text)
        amt = CalcRoundoffAmt(amt, RndFinalAmt)

        txtPUAmount_AMT.Text = IIf(amt > 0, Format(amt, "0.00"), Nothing)
    End Sub

    Private Sub CalcGridPurTotal()
        Dim puGrsWt As Decimal = Nothing
        Dim puDustWt As Decimal = Nothing
        Dim puStnWt As Decimal = Nothing
        Dim puWastage As Decimal = Nothing
        Dim puNetWt As Decimal = Nothing
        Dim puAmt As Double = Nothing
        Dim puVat As Double = Nothing
        Dim totWt As Double = Nothing
        Dim totAmt As Double = Nothing
        For i As Integer = 0 To gridPur.RowCount - 1
            With gridPur.Rows(i)
                If .Cells("EntFlag").Value.ToString = "" Then
                    Exit For
                End If
                puGrsWt += Val(.Cells("GRSWT").Value.ToString)
                puDustWt += Val(.Cells("DUSTWT").Value.ToString)
                puStnWt += Val(.Cells("LESSWT").Value.ToString)
                puWastage += Val(.Cells("WASTAGE").Value.ToString)
                puNetWt += Val(.Cells("NETWT").Value.ToString)
                puAmt += Val(.Cells("AMOUNT").Value.ToString)
                puVat += Val(.Cells("VAT").Value.ToString)

                Select Case .Cells("MODE").Value.ToString
                    Case "WEIGHT"
                        totWt += .Cells("NETWT").Value.ToString
                    Case "AMOUNT"
                        totWt += .Cells("NETWT").Value.ToString
                    Case "PURCHASE"
                        totAmt += .Cells("AMOUNT").Value.ToString
                    Case "EXCHANGE"
                        totAmt += .Cells("AMOUNT").Value.ToString
                    Case "TRANSFER" 'FOR POS
                        totAmt += .Cells("AMOUNT").Value.ToString
                    Case "ADVANCE" 'FOR POS
                        totAmt += .Cells("AMOUNT").Value.ToString
                End Select
                .DefaultCellStyle.ForeColor = Color.Red
                .DefaultCellStyle.SelectionForeColor = Color.Red
            End With
        Next
        gridPurTotal.Rows(0).Cells("CATNAME").Value = "PURCHASE TOT"
        gridPurTotal.Rows(0).Cells("GRSWT").Value = IIf(puGrsWt <> 0, puGrsWt, DBNull.Value)
        gridPurTotal.Rows(0).Cells("DUSTWT").Value = IIf(puDustWt <> 0, puDustWt, DBNull.Value)
        gridPurTotal.Rows(0).Cells("LESSWT").Value = IIf(puStnWt <> 0, puStnWt, DBNull.Value)
        gridPurTotal.Rows(0).Cells("WASTAGE").Value = IIf(puWastage <> 0, puWastage, DBNull.Value)
        gridPurTotal.Rows(0).Cells("NETWT").Value = IIf(puNetWt <> 0, puNetWt, DBNull.Value)
        gridPurTotal.Rows(0).Cells("AMOUNT").Value = IIf(puAmt <> 0, puAmt, DBNull.Value)
        gridPurTotal.Rows(0).Cells("VAT").Value = IIf(puVat <> 0, puVat, DBNull.Value)
        gridPurTotal.Rows(0).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        gridPurTotal.Rows(0).DefaultCellStyle.ForeColor = Color.Red
        gridPurTotal.Rows(0).DefaultCellStyle.SelectionForeColor = Color.Red
        txtPurTotalWeight.Text = IIf(totWt <> 0, Format(totWt, "0.000"), Nothing)
        txtPurTotalAmount.Text = IIf(totAmt <> 0, Format(totAmt, "0.00"), Nothing)
        'CalcFinalAmount()
    End Sub

    Private Sub CalcPUNetWt()


        Dim Autolesswt As String = GetAdmindbSoftValuefromDt(dtSoftKeyss, "ESTAUTOLESSWT", "N")
        Dim wt As Double = Nothing
        wt = Val(txtPUGrsWt_WET.Text) - Val(txtPUDustWt_WET.Text) - IIf(Autolesswt = "Y", Val(txtPUStoneWt_WET.Text), 0) - Val(txtPUWastage_WET.Text) + PurAlloy
        If Val(PUR22KT_TYPE) = 4 Then
            Dim metalId As String = objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & txtPUCategory.Text & "'")
            If metalId = "S" Then wt = (wt * (Val(txtPuPurity_PER.Text) / 100))
        End If
        If Val(PUR22KT_TYPE) = 2 Then
            If Val(txtPuPurity_PER.Text) = 100 Then GoTo netwtassign
            If Val(PUR22KT) = Val(txtPuPurity_PER.Text) Then GoTo netwtassign
            Dim mnetwt As Double
            Dim purityId As String = objGPack.GetSqlValue("SELECT PURITYID FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & txtPUCategory.Text & "'")
            Dim orgpurity As Double = Val(objGPack.GetSqlValue("SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = '" & purityId & "'").ToString)

            If Val(PUR22KT) > Val(txtPuPurity_PER.Text) And orgpurity <> 100 Then
                mnetwt = (wt * (Val(txtPuPurity_PER.Text) / 100)) * (109 / 100)
                wt = mnetwt
            Else
                mnetwt = (wt * (Val(txtPuPurity_PER.Text) / 100))
                wt = mnetwt
            End If
        End If
netwtassign:
        txtPUNetWt_WET.Text = IIf(wt <> 0, Format(wt, "0.000"), Nothing)


        'Dim wt As Decimal = Nothing
        'wt = Val(txtPUGrsWt_WET.Text) - Val(txtPUDustWt_WET.Text) - Val(txtPUStoneWt_WET.Text) - Val(txtPUWastage_WET.Text) + PurAlloy
        'txtPUNetWt_WET.Text = IIf(wt <> 0, Format(wt, "0.000"), Nothing)
    End Sub

    Private Sub StyleGridPur(ByVal gridPurView As DataGridView)
        With gridPurView
            .Columns("MODE").Width = cmbPurMode.Width + 1
            .Columns("CATNAME").Width = txtPUCategory.Width + 1
            .Columns("PURITY").Width = txtPuPurity_PER.Width + 1
            .Columns("PCS").Width = txtPUPcs_NUM.Width + 1
            .Columns("GRSWT").Width = txtPUGrsWt_WET.Width + 1
            .Columns("DUSTWT").Width = txtPUDustWt_WET.Width + 1
            .Columns("WASTAGEPER").Width = txtPuWastage_PER.Width + 1
            .Columns("WASTAGE").Width = txtPUWastage_WET.Width + 1
            .Columns("LESSWT").Width = txtPUStoneWt_WET.Width + 1
            .Columns("NETWT").Width = txtPUNetWt_WET.Width + 1
            .Columns("RATE").Width = txtPURate_AMT.Width + 1
            .Columns("VAT").Width = txtPUVat_AMT.Width + 1
            .Columns("AMOUNT").Width = txtPUAmount_AMT.Width + 1
            .Columns("LESSWT").Visible = False
            .Columns("ALLOYWT").Visible = False
            .Columns("MELTWT").Visible = False
            .Columns("ESTNO").Visible = False
            For i As Integer = 13 To .Columns.Count - 1
                .Columns(i).Visible = False
            Next
        End With
    End Sub



    Private Sub frmOrderAdvance_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            'dtGridStone.AcceptChanges()
            'txtStItem.Select()
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub LoadCategory()
        strSql = " SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE 1=1 "
        If CatViewFilter Then strSql += " AND VIEWFILTER LIKE'%OR%'"
        strSql += " ORDER BY DISPLAYORDER,CATNAME"
        Dim catName As String = BrighttechPack.SearchDialog.Show("Find Category", strSql, cn, 0, 0, , txtPUCategory.Text)
        If catName <> "" Then
            txtPUCategory.Text = catName
            LoadCategoryDetails()
        Else
            txtPUCategory.Focus()
            txtPUCategory.SelectAll()
        End If
    End Sub
    Private Sub LoadCategoryDetails()
        Dim purity As Double = Nothing
        strSql = "SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = "
        strSql += " (SELECT PURITYID FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & txtPUCategory.Text & "')"
        purity = Val(objGPack.GetSqlValue(strSql))
        PurAlloy = Nothing
        txtPuPurity_PER.Text = IIf(purity <> 0, Format(purity, "0.00"), Nothing)
        Dim rate As Double = Val(GetRate(BillDate, objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & txtPUCategory.Text & "'")))
        txtPURate_AMT.Text = IIf(rate, Format(rate, "0.00"), Nothing)
        SendKeys.Send("{TAB}")
        'Me.SelectNextControl(txtPUCategory, True, True, True, True)
    End Sub

    Private Sub txtPUAmount_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPUAmount_AMT.GotFocus

    End Sub

    Private Sub txtPUAmount_AMT_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtPUAmount_AMT.KeyDown
        If cmbPurMode.Text <> "AMOUNT" And cmbPurMode.Text <> "TRANSFER" And cmbPurMode.Text <> "ADVANCE" _
        And cmbPurMode.Text <> "PURCHASE" And cmbPurMode.Text <> "EXCHANGE" Then
            e.Handled = True
        End If
        If e.KeyCode = Keys.Down Then
            gridPur.Select()
        End If
    End Sub

    Private Sub txtPUAmount_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPUAmount_AMT.KeyPress
        If cmbPurMode.Text <> "AMOUNT" And cmbPurMode.Text <> "TRANSFER" And cmbPurMode.Text <> "ADVANCE" _
                And cmbPurMode.Text <> "PURCHASE" And cmbPurMode.Text <> "EXCHANGE" Then
            e.Handled = True
        End If
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtPUCategory.Text = "" And cmbPurMode.Text <> "TRANSFER" And cmbPurMode.Text <> "ADVANCE" Then
                MsgBox(E0004 + Me.GetNextControl(txtPUCategory, False).Text, MsgBoxStyle.Information)
                txtPUCategory.Focus()
                Exit Sub
            End If
            If cmbPurMode.Text = "AMOUNT" And cmbPurMode.Text <> "TRANSFER" And cmbPurMode.Text <> "ADVANCE" Then
                Dim Amt As Double = Val(txtPUAmount_AMT.Text)
                If Val(txtPURate_AMT.Text) = 0 Then
                    txtPURate_AMT.Text = Val(GetRate(BillDate, "00011"))
                End If
                'Dim vatPer As Double = Val(objGPack.GetSqlValue("SELECT PTAX FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = '00011'"))
                'Dim grsAmt As Double = (Val(txtPUAmount_AMT.Text) * 100) / (100 + vatPer)
                'Dim vatAmt As Double = grsAmt * (vatPer / 100)
                Dim Wt As Double = Val(txtPUAmount_AMT.Text) / Val(txtPURate_AMT.Text)
                txtPUGrsWt_WET.Text = IIf(Wt <> 0, Format(Wt, "0.000"), Nothing)
                txtPUNetWt_WET.Text = IIf(Wt <> 0, Format(Wt, "0.000"), Nothing)
                Amt = CalcRoundoffAmt(Amt, RndFinalAmt)
                txtPUAmount_AMT.Text = IIf(Amt <> 0, Format(Amt, "0.00"), Nothing)
            End If
            If Val(txtPUGrsWt_WET.Text) = 0 And cmbPurMode.Text <> "TRANSFER" And cmbPurMode.Text <> "ADVANCE" Then
                MsgBox(Me.GetNextControl(txtPUGrsWt_WET, False).Text + E0001, MsgBoxStyle.Information)
                txtPUGrsWt_WET.Select()
                Exit Sub
            End If
            If Val(txtPUAmount_AMT.Text) = 0 Then
                MsgBox(Me.GetNextControl(txtPUAmount_AMT, False).Text + E0001, MsgBoxStyle.Information)
                txtPUAmount_AMT.Select()
                Exit Sub
            End If
            Dim puTaxPer As Decimal
            strSql = " SELECT STAX FROM " & cnAdminDb & "..TAXMAST WHERE TAXCODE='GT'"
            puTaxPer = Val(objGPack.GetSqlValue(strSql, "STAX", 0).ToString)
            Dim _vbc As Boolean = False
            If GetAdmindbSoftValue("VBC_" & strCompanyId, "N").ToString = "Y" Then
                _vbc = True
            End If

            dtGridPur.AcceptChanges()
            Dim index As Integer = 0
            If txtPURowIndex.Text = "" Then
                For i As Integer = 0 To dtGridPur.Rows.Count - 1
                    With dtGridPur.Rows(i)
                        If .Item("ENTFLAG").ToString <> "Y" Then
                            .Item("MODE") = cmbPurMode.Text
                            .Item("CATNAME") = txtPUCategory.Text
                            .Item("PURITY") = IIf(Val(txtPuPurity_PER.Text) <> 0, txtPuPurity_PER.Text, DBNull.Value)
                            .Item("PCS") = IIf(Val(txtPUPcs_NUM.Text) > 0, txtPUPcs_NUM.Text, DBNull.Value)
                            .Item("GRSWT") = IIf(Val(txtPUGrsWt_WET.Text) > 0, txtPUGrsWt_WET.Text, DBNull.Value)
                            .Item("DUSTWT") = IIf(Val(txtPUDustWt_WET.Text) > 0, txtPUDustWt_WET.Text, DBNull.Value)
                            .Item("WASTAGE") = IIf(Val(txtPUWastage_WET.Text) > 0, txtPUWastage_WET.Text, DBNull.Value)
                            .Item("LESSWT") = IIf(Val(txtPUStoneWt_WET.Text) > 0, txtPUStoneWt_WET.Text, DBNull.Value)
                            .Item("NETWT") = IIf(Val(txtPUNetWt_WET.Text) > 0, txtPUNetWt_WET.Text, DBNull.Value)
                            .Item("RATE") = IIf(Val(txtPURate_AMT.Text) > 0, txtPURate_AMT.Text, DBNull.Value)
                            .Item("VAT") = IIf(Val(txtPUVat_AMT.Text) > 0, txtPUVat_AMT.Text, DBNull.Value)
                            .Item("AMOUNT") = IIf(Val(txtPUAmount_AMT.Text) > 0, txtPUAmount_AMT.Text, DBNull.Value)
                            .Item("GROSSAMT") = Val(txtPUAmount_AMT.Text) - Val(txtPUVat_AMT.Text)
                            .Item("ENTFLAG") = "Y"
                            .Item("WASTAGEPER") = 0
                            .Item("ALLOYWT") = PurAlloy
                            .Item("MELTWT") = 0
                            Dim GstAmt As Double = 0
                            Dim SGstAmt As Double = 0
                            Dim CGstAmt As Double = 0
                            If GST And _vbc = False Then
                                If GSTADVCALC_INCL = "I" Or GSTADVCALC_INCL = "E" Then
                                    SGstAmt = (Val(txtPUAmount_AMT.Text) * (puTaxPer / 2)) / (100 + puTaxPer)
                                    CGstAmt = (Val(txtPUAmount_AMT.Text) * (puTaxPer / 2)) / (100 + puTaxPer)
                                    SGstAmt = Math.Round(SGstAmt, 2)
                                    CGstAmt = Math.Round(CGstAmt, 2)
                                    SGstAmt = CalcRoundoffAmt(SGstAmt, RndGst)
                                    CGstAmt = CalcRoundoffAmt(CGstAmt, RndGst)
                                    GstAmt = SGstAmt + CGstAmt
                                ElseIf GSTADVCALC_INCL = "E" Then
                                    SGstAmt = (Val(txtPUAmount_AMT.Text) * (puTaxPer / 2)) / 100
                                    CGstAmt = (Val(txtPUAmount_AMT.Text) * (puTaxPer / 2)) / 100
                                    SGstAmt = Math.Round(SGstAmt, 2)
                                    CGstAmt = Math.Round(CGstAmt, 2)
                                    SGstAmt = CalcRoundoffAmt(SGstAmt, RndGst)
                                    CGstAmt = CalcRoundoffAmt(CGstAmt, RndGst)
                                    GstAmt = SGstAmt + CGstAmt
                                End If
                            End If
                            .Item("GST") = GstAmt
                            index = i
                            gridPur.CurrentCell = gridPur.Rows(i).Cells("CATNAME")
                            dtGridPur.Rows.Add()
                            Exit For
                        End If
                    End With
                Next
            Else
                With dtGridPur.Rows(Val(txtPURowIndex.Text))
                    .Item("MODE") = cmbPurMode.Text
                    .Item("CATNAME") = txtPUCategory.Text
                    .Item("PURITY") = IIf(Val(txtPuPurity_PER.Text) <> 0, txtPuPurity_PER.Text, DBNull.Value)
                    .Item("PCS") = IIf(Val(txtPUPcs_NUM.Text) > 0, txtPUPcs_NUM.Text, DBNull.Value)
                    .Item("GRSWT") = IIf(Val(txtPUGrsWt_WET.Text) > 0, txtPUGrsWt_WET.Text, DBNull.Value)
                    .Item("DUSTWT") = IIf(Val(txtPUDustWt_WET.Text) > 0, txtPUDustWt_WET.Text, DBNull.Value)
                    .Item("WASTAGE") = IIf(Val(txtPUWastage_WET.Text) > 0, txtPUWastage_WET.Text, DBNull.Value)
                    .Item("LESSWT") = IIf(Val(txtPUStoneWt_WET.Text) > 0, txtPUStoneWt_WET.Text, DBNull.Value)
                    .Item("NETWT") = IIf(Val(txtPUNetWt_WET.Text) > 0, txtPUNetWt_WET.Text, DBNull.Value)
                    .Item("RATE") = IIf(Val(txtPURate_AMT.Text) > 0, txtPURate_AMT.Text, DBNull.Value)
                    .Item("VAT") = IIf(Val(txtPUVat_AMT.Text) > 0, txtPUVat_AMT.Text, DBNull.Value)
                    .Item("AMOUNT") = IIf(Val(txtPUAmount_AMT.Text) > 0, txtPUAmount_AMT.Text, DBNull.Value)
                    .Item("GROSSAMT") = Val(txtPUAmount_AMT.Text) - Val(txtPUVat_AMT.Text)
                    .Item("ENTFLAG") = "Y"
                    .Item("WASTAGEPER") = 0
                    .Item("ALLOYWT") = PurAlloy
                    .Item("MELTWT") = 0
                    Dim GstAmt As Double = 0
                    Dim SGstAmt As Double = 0
                    Dim CGstAmt As Double = 0
                    If GST And _vbc = False Then
                        If GSTADVCALC_INCL = "I" Or GSTADVCALC_INCL = "E" Then
                            SGstAmt = (Val(txtPUAmount_AMT.Text) * (puTaxPer / 2)) / (100 + puTaxPer)
                            CGstAmt = (Val(txtPUAmount_AMT.Text) * (puTaxPer / 2)) / (100 + puTaxPer)
                            SGstAmt = Math.Round(SGstAmt, 2)
                            CGstAmt = Math.Round(CGstAmt, 2)
                            SGstAmt = CalcRoundoffAmt(SGstAmt, RndGst)
                            CGstAmt = CalcRoundoffAmt(CGstAmt, RndGst)
                            GstAmt = SGstAmt + CGstAmt
                        ElseIf GSTADVCALC_INCL = "E" Then
                            SGstAmt = (Val(txtPUAmount_AMT.Text) * (puTaxPer / 2)) / 100
                            CGstAmt = (Val(txtPUAmount_AMT.Text) * (puTaxPer / 2)) / 100
                            SGstAmt = Math.Round(SGstAmt, 2)
                            CGstAmt = Math.Round(CGstAmt, 2)
                            SGstAmt = CalcRoundoffAmt(SGstAmt, RndGst)
                            CGstAmt = CalcRoundoffAmt(CGstAmt, RndGst)
                            GstAmt = SGstAmt + CGstAmt
                        End If
                    End If
                    .Item("GST") = GstAmt
                    'dtStoneDetails.AcceptChanges()
                    'For Each ro As DataRow In dtStoneDetails.Rows
                    '    If ro!KEYNO.ToString = .Item("KEYNO").ToString Then
                    '        ro.Delete()
                    '    End If
                    'Next
                    'dtStoneDetails.AcceptChanges()
                End With
            End If

            ''Stone
            For rwIndex As Integer = 0 To objStone.dtGridStone.Rows.Count - 1
                Dim ro As DataRow = dtStoneDetails.NewRow
                ro("KEYNO") = dtGridPur.Rows(index).Item("KEYNO").ToString
                For colIndex As Integer = 2 To objStone.dtGridStone.Columns.Count - 1
                    ro(colIndex) = objStone.dtGridStone.Rows(rwIndex).Item(colIndex)
                Next
                dtStoneDetails.Rows.Add(ro)
            Next
            dtStoneDetails.AcceptChanges()

            dtGridPur.AcceptChanges()
            CalcGridPurTotal()
            ''Clear
            Dim totWt As Double = Val(txtPurTotalWeight.Text)
            Dim totAmt As Double = Val(txtPurTotalAmount.Text)
            objGPack.TextClear(grpPur)
            PurAlloy = Nothing
            txtPurTotalWeight.Text = totWt
            txtPurTotalAmount.Text = totAmt
            cmbPurMode.Focus()
        End If
    End Sub

    Private Sub txtPUDustWt_WET_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPUDustWt_WET.GotFocus
        If cmbPurMode.Text = "AMOUNT" Then
            SendKeys.Send("{TAB}")
            Exit Sub
        End If
    End Sub

    Private Sub txtPUDustWt_WET_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtPUDustWt_WET.KeyDown
        If e.KeyCode = Keys.Down Then
            gridPur.Select()
        End If
    End Sub

    Private Sub txtPUDustWt_WET_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPUDustWt_WET.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If Val(txtPUGrsWt_WET.Text) > 0 Then
                If Val(txtPUNetWt_WET.Text) < 0 Then
                    MsgBox("Net Weight Goes to Negative Value. It Does not Allow", MsgBoxStyle.Information)
                    txtPUDustWt_WET.Select()
                    Exit Sub
                End If
            End If
        End If
    End Sub

    Private Sub txtPUDustWt_WET_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPUDustWt_WET.TextChanged
        CalcPuWastagePer()
        CalcPuWastage()
        CalcPuAlloy()
        CalcPUNetWt()
    End Sub

    Private Sub txtPUGrsWt_WET_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPUGrsWt_WET.GotFocus
        If cmbPurMode.Text = "AMOUNT" Then
            SendKeys.Send("{TAB}")
            Exit Sub
        End If
    End Sub
    Private Sub txtPUGrsWt_WET_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtPUGrsWt_WET.KeyDown
        If e.KeyCode = Keys.Down Then
            gridPur.Select()
        End If
    End Sub
    Private Sub ShowStoneDia()
        If objStone.Visible Then Exit Sub
        objStone.grsWt = Val(txtPUGrsWt_WET.Text)
        'objStone.BackColor = grpPur.BackColor
        objStone.StartPosition = FormStartPosition.CenterScreen
        objStone.MaximizeBox = False
        objStone.grpStone.BackgroundColor = grpPur.BackgroundColor
        objStone.StyleGridStone(objStone.gridStone)
            objStone.txtStItem.Select()
        objStone.ShowDialog()
        Dim stnWt As Double = Val(objStone.gridStoneTotal.Rows(0).Cells("WEIGHT").Value.ToString)
        Dim stnAmt As Double = Val(objStone.gridStoneTotal.Rows(0).Cells("AMOUNT").Value.ToString)
        Dim ntWt As Double = Val(txtPUGrsWt_WET.Text) - stnWt
        Me.SelectNextControl(txtPUGrsWt_WET, True, True, True, True)
    End Sub
    Private Sub txtPUGrsWt_WET_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPUGrsWt_WET.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If Val(txtPUGrsWt_WET.Text) = 0 Then
                MsgBox(Me.GetNextControl(txtPUGrsWt_WET, False).Text + E0001, MsgBoxStyle.Information)
                txtPUGrsWt_WET.Select()
                Exit Sub
            End If
        End If
    End Sub
    Private Sub txtPUGrsWt_WET_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPUGrsWt_WET.TextChanged
        CalcPuWastagePer()
        CalcPuWastage()
        CalcPuAlloy()
        CalcPUNetWt()
    End Sub
    Private Sub txtPUNetWt_WET_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPUNetWt_WET.GotFocus
        SendKeys.Send("{TAB}")
    End Sub

    Private Sub txtPUNetWt_WET_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPUNetWt_WET.TextChanged
        CalcPUAmount()
    End Sub

    Private Sub txtPUPcs_NUM_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPUPcs_NUM.GotFocus
        If cmbPurMode.Text = "AMOUNT" Then
            SendKeys.Send("{TAB}")
            Exit Sub
        End If
    End Sub
    Private Sub txtPUPcs_NUM_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtPUPcs_NUM.KeyDown
        If e.KeyCode = Keys.Down Then
            gridPur.Select()
        End If
    End Sub

    Private Sub txtPUPcs_NUM_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPUPcs_NUM.TextChanged
        CalcPUAmount()
    End Sub
    Private Sub txtPURate_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPURate_AMT.GotFocus
        Dim value As Double = Nothing
        value = Nothing
        value = Val(GetRate(billdate, objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & txtPUCategory.Text & "'")))
        txtPURate_AMT.Text = IIf(value > 0, Format(value, "0.00"), Nothing)
    End Sub

    Private Sub txtPURate_AMT_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtPURate_AMT.KeyDown
        If e.KeyCode = Keys.Down Then
            gridPur.Select()
        End If
    End Sub

    Private Sub txtPURate_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPURate_AMT.TextChanged
        If cmbPurMode.Text = "AMOUNT" Then Exit Sub
        CalcPUAmount()
    End Sub

    Private Sub txtPUVat_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPUVat_AMT.GotFocus
        If cmbPurMode.Text = "AMOUNT" Then
            SendKeys.Send("{TAB}")
            Exit Sub
        End If
    End Sub
    Private Sub txtPUVat_AMT_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtPUVat_AMT.KeyDown
        If e.KeyCode = Keys.Down Then
            gridPur.Select()
        End If

    End Sub

    Private Sub txtPUVat_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPUVat_AMT.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then CalcPUAmountAtVatChange()
    End Sub

    Private Sub txtPUCategory_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPUCategory.GotFocus
        'If cmbPurMode.Text = "AMOUNT" Or cmbPurMode.Text = "WEIGHT" Then
        If cmbPurMode.Text = "WEIGHT" Then
            txtPUCategory.Text = objGPack.GetSqlValue("SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = '" & ORDER_OLD_CAT & "'")
            LoadCategoryDetails()
            Exit Sub
        End If
        Main.ShowHelpText("Press Insert Key to Help")
    End Sub

    Private Sub txtPUCategory_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtPUCategory.KeyDown
        If e.KeyCode = Keys.Insert Then
            LoadCategory()
        End If
    End Sub

    Private Sub txtPUCategory_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPUCategory.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtPUCategory.Text = "" Then
                LoadCategory()
            ElseIf txtPUCategory.Text <> "" And objGPack.DupCheck("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & txtPUCategory.Text & "'") = False Then
                LoadCategory()
            Else
                LoadCategoryDetails()
            End If
        End If
    End Sub

    Private Sub txtPUCategory_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPUCategory.LostFocus
        Main.HideHelpText()
    End Sub

    Private Sub txtPUWastage_WET_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPUWastage_WET.GotFocus
        If cmbPurMode.Text = "AMOUNT" Then
            SendKeys.Send("{TAB}")
            Exit Sub
        End If
    End Sub
    Private Sub txtPUWastage_WET_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtPUWastage_WET.KeyDown
        If e.KeyCode = Keys.Down Then
            gridPur.Select()
        End If
    End Sub

    Private Sub txtPUWastage_WET_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPUWastage_WET.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If Val(txtPUGrsWt_WET.Text) > 0 Then
                If Val(txtPUNetWt_WET.Text) < 0 Then
                    MsgBox("Net Weight Goes to Negative Value. It Does not Allow", MsgBoxStyle.Information)
                    txtPUWastage_WET.Select()
                    Exit Sub
                End If
            End If
        End If
    End Sub

    Private Sub txtPUWastage_WET_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPuWastage_PER.TextChanged
        CalcPuAlloy()
        CalcPUNetWt()
    End Sub

    Private Sub txtPUStoneWt_WET_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPUStoneWt_WET.GotFocus
        SendKeys.Send("{TAB}")
    End Sub

    Private Sub txtPuPurity_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPURate_AMT.GotFocus
        If cmbPurMode.Text = "AMOUNT" Then
            SendKeys.Send("{TAB}")
            Exit Sub
        End If
    End Sub

    Private Sub gridPur_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridPur.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            If Not gridPur.RowCount > 0 Then Exit Sub
            gridPur.CurrentCell = gridPur.CurrentRow.Cells("CATNAME")
            With gridPur.Rows(gridPur.CurrentRow.Index)
                If .Cells("ENTFLAG").Value.ToString <> "Y" Then Exit Sub
                cmbPurMode.Text = .Cells("MODE").FormattedValue
                txtPUCategory.Text = .Cells("CATNAME").FormattedValue
                txtPuPurity_PER.Text = .Cells("PURITY").FormattedValue
                txtPUPcs_NUM.Text = .Cells("PCS").FormattedValue
                txtPUGrsWt_WET.Text = .Cells("GRSWT").FormattedValue
                txtPUDustWt_WET.Text = .Cells("DUSTWT").FormattedValue
                txtPUWastage_WET.Text = .Cells("WASTAGE").FormattedValue
                txtPUStoneWt_WET.Text = .Cells("LESSWT").FormattedValue
                txtPUNetWt_WET.Text = .Cells("NETWT").FormattedValue
                txtPURate_AMT.Text = .Cells("RATE").FormattedValue
                txtPUVat_AMT.Text = .Cells("VAT").FormattedValue
                txtPUAmount_AMT.Text = .Cells("AMOUNT").FormattedValue
                PurAlloy = .Cells("ALLOYWT").FormattedValue
                txtPURowIndex.Text = gridPur.CurrentRow.Index
                cmbPurMode.Focus()

            End With
        End If
    End Sub

    Private Sub gridPur_UserDeletedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles gridPur.UserDeletedRow
        dtGridPur.AcceptChanges()
        If Not gridPur.RowCount > 0 Then cmbPurMode.Focus()
        CalcGridPurTotal()
    End Sub

    Private Sub gridPur_UserDeletingRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowCancelEventArgs) Handles gridPur.UserDeletingRow
        If gridPur.Rows(e.Row.Index).Cells("EntFlag").Value.ToString <> "Y" Then
            dtGridPur.Rows.Add()
        End If
    End Sub
    Private Sub CalcPuWastagePer()
        

        Dim balWt As Double = Val(txtPUGrsWt_WET.Text) - Val(txtPUStoneWt_WET.Text) - Val(txtPUDustWt_WET.Text)
        Dim netWt As Double '= balWt * Val(txtPuPurity_PER.Text) / 100
        Dim was As Double '= balWt - netWt
        Dim wasPer As Double '= IIf(balWt <> 0, was / balWt * 100, 0)
        If wasPer = 0 Then
            netWt = balWt * Val(txtPuPurity_PER.Text) / 100
            If Val(PUR22KT) <> 0 And Val(PUR22KT_TYPE) = 0 Then netWt = (netWt / Val(PUR22KT)) * 100
            was = balWt - netWt
            wasPer = IIf(balWt <> 0, was / balWt * 100, 0)
            If Val(txtPuPurity_PER.Text) = 0 Then
                was = 0
                wasPer = 0
            End If
        Else
            netWt = balWt
            was = netWt * (wasPer / 100)
        End If


        
        If cmbPurMode.Text <> "PURCHASE" And cmbPurMode.Text <> "EXCHANGE" And cmbPurMode.Text <> "WEIGHT" Then
            txtPuWastage_PER.Text = IIf(wasPer <> 0, Format(wasPer, "0.00"), Nothing)
            Exit Sub
        End If
        If Val(PUR22KT) <> 0 Then
            If Val(PUR22KT_TYPE) = 0 Then
                netWt = (netWt / Val(PUR22KT)) * 100
                was = balWt - netWt
                txtPUWastage_WET.Text = IIf(was <> 0, Format(was, "0.000"), "")
            ElseIf Val(PUR22KT_TYPE) = 1 Then
                If was = 0 Then was = balWt * ((Val(PUR22KT) - Val(txtPuPurity_PER.Text)) / 100)
                txtPUWastage_WET.Text = IIf(was <> 0, Format(was, "0.000"), "")
                txtPuWastage_PER.Text = IIf(wasPer <> 0, Format(wasPer, "0.00"), Nothing)
            ElseIf Val(PUR22KT_TYPE) = 3 Then
                If Val(txtPuPurity_PER.Text) = 100 Then Exit Sub
                If Val(txtPuPurity_PER.Text) = 100 Then Exit Sub
                If was = 0 Then was = balWt * ((Val(PUR22KT) - Val(txtPuPurity_PER.Text)) / 100)
                txtPUWastage_WET.Text = IIf(was <> 0, Format(was, "0.000"), "")
                txtPuWastage_PER.Text = IIf(wasPer <> 0, Format(wasPer, "0.00"), Nothing)
            ElseIf Val(PUR22KT_TYPE) = 2 Then
                If Val(txtPuPurity_PER.Text) = 100 Then Exit Sub
                wasPer = 0 : balWt = 0 : was = 0
                txtPUWastage_WET.Text = IIf(was <> 0, Format(was, "0.000"), "")
                txtPuWastage_PER.Text = IIf(wasPer <> 0, Format(wasPer, "0.00"), Nothing)
            ElseIf Val(PUR22KT_TYPE) = 4 Then

                If Val(txtPuPurity_PER.Text) = 100 Then Exit Sub
                netWt = (netWt / Val(PUR22KT)) * 100
                was = balWt - netWt
                txtPUWastage_WET.Text = IIf(was <> 0, Format(was, "0.000"), "")

            End If
        Else
            If Val(PURALLOYPER) <> 0 And Val(txtPuPurity_PER.Text) = 91.6 Then
                txtPuWastage_PER.Text = ""
            Else
                txtPuWastage_PER.Text = IIf(wasPer <> 0, Format(wasPer, "0.00"), Nothing)
            End If
        End If



        'If Val(txtPuPurity_PER.Text) = 0 Then
        '    was = 0
        '    wasPer = 0
        'End If
        'If Val(PUR22KT) <> 0 Then
        '    If Val(PUR22KT_TYPE) = 0 Then
        '        netWt = (netWt / Val(PUR22KT)) * 100
        '        was = balWt - netWt
        '        txtPUWastage_WET.Text = IIf(was <> 0, Format(was, "0.000"), "")
        '    ElseIf Val(PUR22KT_TYPE) = 1 Then
        '        was = balWt * ((Val(PUR22KT) - Val(txtPuPurity_PER.Text)) / 100)
        '        txtPUWastage_WET.Text = IIf(was <> 0, Format(was, "0.000"), "")
        '    End If
        'Else
        '    If Val(PURALLOYPER) <> 0 And Val(txtPuPurity_PER.Text) = 91.6 Then
        '        txtPuWastage_PER.Text = ""
        '    Else
        '        txtPuWastage_PER.Text = IIf(wasPer <> 0, Format(wasPer, "0.00"), Nothing)
        '    End If
        'End If
    End Sub

    Private Sub CalcPuWastage()
        If Val(txtPuWastage_PER.Text) = 0 And Val(txtPUWastage_WET.Text) <> 0 Then Exit Sub
        Dim wast As Double = Nothing
        wast = (Val(txtPUGrsWt_WET.Text) - Val(txtPUDustWt_WET.Text) - Val(txtPUStoneWt_WET.Text)) * (Val(txtPuWastage_PER.Text) / 100)
        wast = Math.Round(wast, WastageRound)
        txtPUWastage_WET.Text = IIf(wast <> 0, Format(wast, "0.000"), Nothing)
    End Sub

    Private Sub txtPuPurity_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPuPurity_PER.TextChanged
        txtPuWastage_PER.Clear()
        txtPUWastage_WET.Clear()
        CalcPuWastagePer()
        CalcPuWastage()
        CalcPuAlloy()
        CalcPUNetWt()
    End Sub

    Private Sub txtPuWastage_PER_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPuWastage_PER.GotFocus
        If cmbPurMode.Text = "AMOUNT" Then
            SendKeys.Send("{TAB}")
            Exit Sub
        End If
    End Sub
    Private Sub txtPuWastage_PER_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPuWastage_PER.TextChanged
        CalcPuWastage()
    End Sub

    Private Sub cmbPurMode_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbPurMode.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
                If cmbPurMode.Text = "PURCHASE" And HasEst Then
                ObjPurchaseEstNo.BillDate = BillDate
ShowEstDia:
                If ObjPurchaseEstNo.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    If ObjPurchaseEstNo.txtEstNo_NUM.Text = "" Then
                        MsgBox("EstNo Should not Empty", MsgBoxStyle.Information)
                        Exit Sub
                    End If
                    For Each ro As DataRow In dtGridPur.Rows
                        If ro!ENTFLAG.ToString = "" Then Exit For
                        If ro!ESTNO.ToString = ObjPurchaseEstNo.txtEstNo_NUM.Text Then
                            MsgBox("This EstNo Already Loaded", MsgBoxStyle.Information)
                            Exit Sub
                        End If
                    Next
                    Dim index As Integer = 0
                    strSql = " SELECT *"
                    strSql += " ,(SELECT CATNAME FROM  " & cnAdminDb & "..CATEGORY WHERE CATCODE = E.CATCODE)AS CATNAME"
                    strSql += "  FROM " & cnStockDb & "..ESTRECEIPT AS E"
                    strSql += " WHERE TRANNO = " & Val(ObjPurchaseEstNo.txtEstNo_NUM.Text) & ""
                    strSql += " AND TRANTYPE = 'PU'"
                    strSql += " AND TRANDATE = '" & BillDate.Date & "'"
                    Dim dtEstDet As New DataTable
                    da = New OleDbDataAdapter(strSql, cn)
                    da.Fill(dtEstDet)
                    If Not dtEstDet.Rows.Count > 0 Then
                        MsgBox(Me.GetNextControl(ObjPurchaseEstNo.txtEstNo_NUM, False).Text + E0022, MsgBoxStyle.Information)
                        Exit Sub
                    End If
                    If dtEstDet.Rows(0).Item("BATCHNO").ToString <> "" Then
                        MsgBox("Already Billed", MsgBoxStyle.Information)
                        Exit Sub
                    End If
                    Dim puTaxPer As Decimal
                    strSql = " SELECT STAX FROM " & cnAdminDb & "..TAXMAST WHERE TAXCODE='GT'"
                    puTaxPer = Val(objGPack.GetSqlValue(strSql, "STAX", 0).ToString)
                    Dim _vbc As Boolean = False
                    If GetAdmindbSoftValue("VBC_" & strCompanyId, "N").ToString = "Y" Then
                        _vbc = True
                    End If
                    For Each roEst As DataRow In dtEstDet.Rows
                        For i As Integer = 0 To dtGridPur.Rows.Count - 1
                            With dtGridPur.Rows(i)
                                If .Item("MODE").ToString = "" Then
                                    .Item("MODE") = "PURCHASE"
                                    .Item("ESTNO") = Val(ObjPurchaseEstNo.txtEstNo_NUM.Text)
                                    .Item("CATNAME") = roEst!CATNAME.ToString
                                    .Item("PURITY") = Val(roEst!PURITY.ToString)
                                    .Item("PCS") = IIf(Val(roEst!PCS.ToString) > 0, roEst!PCS, DBNull.Value)
                                    .Item("GRSWT") = IIf(Val(roEst!GRSWT.ToString) > 0, roEst!GRSWT, DBNull.Value)
                                    .Item("DUSTWT") = IIf(Val(roEst!DUSTWT.ToString) > 0, roEst!DUSTWT, DBNull.Value)
                                    .Item("WASTAGE") = IIf(Val(roEst!WASTAGE.ToString) > 0, roEst!WASTAGE, DBNull.Value)
                                    .Item("LESSWT") = IIf(Val(roEst!LESSWT.ToString) > 0, roEst!LESSWT, DBNull.Value)
                                    .Item("NETWT") = IIf(Val(roEst!NETWT.ToString) > 0, roEst!NETWT, DBNull.Value)
                                    .Item("RATE") = IIf(Val(roEst!RATE.ToString) > 0, roEst!RATE, DBNull.Value)
                                    .Item("VAT") = IIf(Val(roEst!TAX.ToString) > 0, roEst!TAX, DBNull.Value)
                                    Dim amt As Double = IIf(Val(roEst!AMOUNT.ToString) + Val(roEst!TAX.ToString) > 0, Val(roEst!AMOUNT.ToString) + Val(roEst!TAX.ToString), 0)
                                    amt = CalcRoundoffAmt(amt, RndFinalAmt)
                                    .Item("AMOUNT") = IIf(amt > 0, amt, DBNull.Value)
                                    .Item("GROSSAMT") = IIf(Val(roEst!AMOUNT.ToString) > 0, Val(roEst!AMOUNT.ToString), DBNull.Value)
                                    .Item("ENTFLAG") = "Y"
                                    '.Item("TRANTYPE") = "PU"
                                    Dim GstAmt As Double = 0
                                    Dim SGstAmt As Double = 0
                                    Dim CGstAmt As Double = 0
                                    If GST And _vbc = False Then
                                        If GSTADVCALC_INCL = "I" Or GSTADVCALC_INCL = "E" Then
                                            SGstAmt = ((Val(roEst!AMOUNT.ToString) + Val(roEst!TAX.ToString)) * (puTaxPer / 2)) / (100 + puTaxPer)
                                            CGstAmt = ((Val(roEst!AMOUNT.ToString) + Val(roEst!TAX.ToString)) * (puTaxPer / 2)) / (100 + puTaxPer)
                                            SGstAmt = Math.Round(SGstAmt, 2)
                                            CGstAmt = Math.Round(CGstAmt, 2)
                                            SGstAmt = CalcRoundoffAmt(SGstAmt, RndGst)
                                            CGstAmt = CalcRoundoffAmt(CGstAmt, RndGst)
                                            GstAmt = SGstAmt + CGstAmt
                                        ElseIf GSTADVCALC_INCL = "E" Then
                                            SGstAmt = ((Val(roEst!AMOUNT.ToString) + Val(roEst!TAX.ToString)) * (puTaxPer / 2)) / 100
                                            CGstAmt = ((Val(roEst!AMOUNT.ToString) + Val(roEst!TAX.ToString)) * (puTaxPer / 2)) / 100
                                            SGstAmt = Math.Round(SGstAmt, 2)
                                            CGstAmt = Math.Round(CGstAmt, 2)
                                            SGstAmt = CalcRoundoffAmt(SGstAmt, RndGst)
                                            CGstAmt = CalcRoundoffAmt(CGstAmt, RndGst)
                                            GstAmt = SGstAmt + CGstAmt
                                        End If
                                    End If
                                    .Item("GST") = GstAmt
                                    .Item("FLAG") = roEst!FLAG
                                    .Item("EMPID") = roEst!EMPID
                                    .Item("REMARK1") = roEst!REMARK1.ToString
                                    .Item("BOARDRATE") = roEst!BOARDRATE
                                    .Item("WASTAGEPER") = 0
                                    .Item("ITEM") = Val(objGPack.GetSqlValue("SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & Val(roEst!ITEMID.ToString) & ""))
                                    .Item("SUBITEM") = Val(objGPack.GetSqlValue("SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = " & Val(roEst!SUBITEMID.ToString) & ""))
                                    .Item("ITEMTYPEID") = roEst!ITEMTYPEID
                                    .Item("ALLOYWT") = roEst!ALLOY
                                    .Item("MELTWT") = roEst!MELTWT
                                    index = i
                                    gridPur.CurrentCell = gridPur.Rows(i).Cells("CATNAME")
                                    dtGridPur.Rows.Add()
                                    Exit For
                                End If
                            End With
                        Next
                    Next

                    'If objAddressDia.txtAddressName.Text = "" Then
                    '    strSql = " SELECT SNO,PREVILEGEID,ACCODE,TRANDATE,TITLE"
                    '    strSql += " ,INITIAL,PNAME,DOORNO,ADDRESS1"
                    '    strSql += " ,ADDRESS2,ADDRESS3,AREA,CITY"
                    '    strSql += " ,STATE,COUNTRY,PINCODE,PHONERES"
                    '    strSql += " ,MOBILE,EMAIL,FAX"
                    '    strSql += " FROM " & cnAdminDb & "..PERSONALINFO"
                    '    strSql += " WHERE SNO = '" & dtEstDet.Rows(0).Item("PSNO").ToString & "'"
                    '    Dim dtAddress As New DataTable
                    '    da = New OleDbDataAdapter(strSql, cn)
                    '    da.Fill(dtAddress)
                    '    If dtAddress.Rows.Count > 0 Then
                    '        With dtAddress.Rows(0)
                    '            objAddressDia.txtAddressPrevilegeId.Text = .Item("PREVILEGEID").ToString
                    '            objAddressDia.txtAddressPartyCode.Text = .Item("ACCODE").ToString
                    '            objAddressDia.cmbAddressTitle_OWN.Text = .Item("TITLE").ToString
                    '            objAddressDia.txtAddressInitial.Text = .Item("INITIAL").ToString
                    '            objAddressDia.txtAddressName.Text = .Item("PNAME").ToString
                    '            objAddressDia.txtAddressDoorNo.Text = .Item("DOORNO").ToString
                    '            objAddressDia.txtAddress1.Text = .Item("ADDRESS1").ToString
                    '            objAddressDia.txtAddress2.Text = .Item("ADDRESS2").ToString
                    '            objAddressDia.txtAddress3.Text = .Item("ADDRESS3").ToString
                    '            objAddressDia.cmbAddressArea_OWN.Text = .Item("AREA").ToString
                    '            objAddressDia.cmbAddressCity_OWN.Text = .Item("CITY").ToString
                    '            objAddressDia.cmbAddressState_OWN.Text = .Item("STATE").ToString
                    '            objAddressDia.cmbAddressCountry_OWN.Text = .Item("COUNTRY").ToString
                    '            objAddressDia.txtAddressPincode_NUM.Text = .Item("PINCODE").ToString
                    '            objAddressDia.txtAddressPhoneRes.Text = .Item("PHONERES").ToString
                    '            objAddressDia.txtAddressMobile.Text = .Item("MOBILE").ToString
                    '            objAddressDia.txtAddressEmailId_OWN.Text = .Item("EMAIL").ToString
                    '            objAddressDia.txtAddressFax.Text = .Item("FAX").ToString
                    '            objAddressDia.EstSno = .Item("SNO").ToString()
                    '            txtPaymentAmount_Amt.Select()
                    '            txtPaymentAmount_Amt.SelectAll()
                    '        End With
                    '    End If
                    'End If
                    dtGridPur.AcceptChanges()
                    CalcGridPurTotal()
                    ''Clear
                    Dim totWt As Double = Val(txtPurTotalWeight.Text)
                    Dim totAmt As Double = Val(txtPurTotalAmount.Text)
                    Dim PURINDEX As String = txtPURowIndex.Text
                    objGPack.TextClear(grpPur)
                    txtPURowIndex.Text = PURINDEX
                    PurAlloy = Nothing
                    txtPurTotalWeight.Text = totWt
                    txtPurTotalAmount.Text = totAmt
                    cmbPurMode.Focus()
                    Exit Sub
                End If
            Else
                Dim PURINDEX As String = txtPURowIndex.Text
                objGPack.TextClear(grpPur)
                txtPURowIndex.Text = PURINDEX
            End If
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub cmbPurMode_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbPurMode.SelectedIndexChanged
        Dim PURINDEX As String = txtPURowIndex.Text
        objGPack.TextClear(grpPur)
        txtPURowIndex.Text = PURINDEX
    End Sub

    Private Sub ClearDtGrid(ByVal dt As DataTable) ''Only For gridOrder
        dt.Columns("KeyNo").AutoIncrementSeed = 1
        dt.Rows.Clear()
        For i As Integer = 1 To 20
            dt.Rows.Add()
        Next
        dt.AcceptChanges()
    End Sub


    Public Function InsertOrderAdvanceDetail( _
    ByVal batchno As String, ByVal tNo As Integer, ByVal billdate As Date, ByVal billCashCounterId As String, _
    ByVal billCostId As String, ByVal VatExm As String, ByVal tran As OleDbTransaction _
    , ByVal orderNo As String, ByVal Accode As String, ByVal frmFlag As String, Optional ByVal EmpId As Integer = Nothing, Optional ByVal Remark1 As String = Nothing, Optional ByVal Remark2 As String = Nothing, Optional ByVal DtTempTb As String = Nothing, Optional ByVal alterno As Integer = Nothing, Optional ByVal FixRate As Double = Nothing, _
    Optional ByVal Paymode As String = "OR", _
    Optional ByVal GstVal As Double = 0, _
    Optional ByVal GstInclusive As Boolean = False _
    ) As Boolean
        If GSTADVCALC_INCL = "E" Then GSTADVCALC_INCL = "I"
        ''Advance weight Trans ( Pur)
        For Each ro As DataRow In dtGridPur.Rows
            If ro!ENTFLAG.ToString = "" Then Exit For
            Select Case ro.Item("MODE").ToString
                Case "TRANSFER" 'FROM POS
                    InsertIntoAccTran(frmFlag, tran, billdate, batchno, billCashCounterId, billCostId, VatExm, tNo, _
                    IIf(Val(ro!AMOUNT.ToString) > 0, "C", "D"), _
                    Accode, Val(ro!AMOUNT.ToString), 0, 0, 0, "OR", "", , , , , , , Remark1, Remark2, DtTempTb)
                    InsertIntoOustanding("T", tran, tNo, billdate, billCostId, billCashCounterId, batchno, VatExm, _
                     "A", orderNo, Val(ro!AMOUNT.ToString), "R", "OR", 0, 0, 0, FixRate, 0, , , 0, , , Accode, EmpId, Remark1, Remark2)
                Case "ADVANCE" 'FROM POS
                    InsertIntoAccTran(frmFlag, tran, billdate, batchno, billCashCounterId, billCostId, VatExm, tNo, _
                    IIf(Val(ro!AMOUNT.ToString) > 0, "C", "D"), _
                    Accode, Val(ro!AMOUNT.ToString) - IIf(GstInclusive, GstVal, 0), 0, 0, 0, Paymode, "", , , , , , , Remark1, Remark2, DtTempTb)
                    InsertIntoOustanding("F", tran, tNo, billdate, billCostId, billCashCounterId, batchno, VatExm, _
                     "A", orderNo, Val(ro!AMOUNT.ToString) - IIf(GstInclusive, GstVal, 0), "R", Paymode, 0, 0, 0, FixRate, 0, , , 0, , , Accode, EmpId, Remark1, Remark2, , GstVal)
                Case "AMOUNT"
                    InsertIntoAccTran(frmFlag, tran, billdate, batchno, billCashCounterId, billCostId, VatExm, tNo, IIf(Val(ro!AMOUNT.ToString) > 0, "D", "C"), _
                    "ADVANCE", Val(ro!AMOUNT.ToString), 0, 0, 0, "AR", "", , , , , , , Remark1, Remark2, DtTempTb)
                    InsertIntoAccTran(frmFlag, tran, billdate, batchno, billCashCounterId, billCostId, VatExm, tNo, IIf(Val(ro!AMOUNT.ToString) > 0, "C", "D"), _
                    CASHID _
                    , Val(ro!AMOUNT.ToString), 0, Val(ro!GRSWT.ToString), Val(ro!NETWT.ToString), "CA", "", , , , , , , Remark1, Remark2, DtTempTb)
                Case "PURCHASE"
                    Dim Vatamt As Double = Val(ro!VAT.ToString)
                    InsertIntoAccTran(frmFlag, tran, billdate, batchno, billCashCounterId, billCostId, VatExm, tNo, IIf(Val(ro!AMOUNT.ToString) - Val(ro!VAT.ToString) > 0, "D", "C"), _
                    objGPack.GetSqlValue("SELECT PURCHASEID FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & ro.Item("CATNAME").ToString & "'", , , tran) _
                    , Val(ro!AMOUNT.ToString) - Val(ro!VAT.ToString), 0, Val(ro!GRSWT.ToString), Val(ro!NETWT.ToString), "PU", "", , , , , , , Remark1, Remark2, DtTempTb)
                    If GST Then
                        strSql = "SELECT * FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & ro.Item("CATNAME").ToString & "'"
                        Dim dr As DataRow = GetSqlRow(strSql, cn, tran)
                        If Not dr Is Nothing Then
                            InsertIntoAccTran(frmFlag, tran, billdate, batchno, billCashCounterId, billCostId, VatExm, tNo, IIf(Val(ro!VAT.ToString) > 0, "D", "C"), _
                                    dr("P_SGSTID").ToString, (Vatamt / 2), 0, 0, 0, "PV", "", , , , , , , Remark1, Remark2, DtTempTb)
                            InsertIntoAccTran(frmFlag, tran, billdate, batchno, billCashCounterId, billCostId, VatExm, tNo, IIf(Val(ro!VAT.ToString) > 0, "D", "C"), _
                                    dr("P_CGSTID").ToString, (Vatamt / 2), 0, 0, 0, "PV", "", , , , , , , Remark1, Remark2, DtTempTb)
                        End If
                        InsertIntoAccTran(frmFlag, tran, billdate, batchno, billCashCounterId, billCostId, VatExm, IIf(alterno <> 0, alterno, tNo), IIf(Val(ro!AMOUNT.ToString) > 0, "C", "D"), _
                                    Accode, (Val(ro!AMOUNT.ToString) - Val(ro!GST.ToString)), 0, 0, 0, "OR", "", , , , , , , Remark1, Remark2, DtTempTb)
                    Else
                        InsertIntoAccTran(frmFlag, tran, billdate, batchno, billCashCounterId, billCostId, VatExm, tNo, IIf(Val(ro!VAT.ToString) > 0, "D", "C"), _
                                objGPack.GetSqlValue("SELECT PTAXID FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & ro.Item("CATNAME").ToString & "'", , , tran) _
                                , Vatamt, 0, 0, 0, "PV", "", , , , , , , Remark1, Remark2, DtTempTb)
                        InsertIntoAccTran(frmFlag, tran, billdate, batchno, billCashCounterId, billCostId, VatExm, IIf(alterno <> 0, alterno, tNo), IIf(Val(ro!AMOUNT.ToString) > 0, "C", "D"), _
                                Accode, (Val(ro!AMOUNT.ToString)), 0, 0, 0, "OR", "", , , , , , , Remark1, Remark2, DtTempTb)
                    End If

                Case "EXCHANGE"
                    Dim Vatamt As Double = Val(ro!VAT.ToString)
                    InsertIntoAccTran(frmFlag, tran, billdate, batchno, billCashCounterId, billCostId, VatExm, tNo, IIf(Val(ro!AMOUNT.ToString) - Val(ro!VAT.ToString) > 0, "D", "C"), _
                    objGPack.GetSqlValue("SELECT SRETURNID FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & ro.Item("CATNAME").ToString & "'", , , tran) _
                    , Val(ro!AMOUNT.ToString) - Val(ro!VAT.ToString), 0, 0, 0, "SR", "", , , , , , , Remark1, Remark2, DtTempTb)
                    If GST Then
                        strSql = "SELECT * FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & ro.Item("CATNAME").ToString & "'"
                        Dim dr As DataRow = GetSqlRow(strSql, cn, tran)
                        If Not dr Is Nothing Then
                            InsertIntoAccTran(frmFlag, tran, billdate, batchno, billCashCounterId, billCostId, VatExm, tNo, IIf(Val(ro!VAT.ToString) > 0, "D", "C"), _
                                dr("S_SGSTID").ToString, (Vatamt / 2), 0, 0, 0, "RV", "", , , , , , , Remark1, Remark2, DtTempTb)
                            InsertIntoAccTran(frmFlag, tran, billdate, batchno, billCashCounterId, billCostId, VatExm, tNo, IIf(Val(ro!VAT.ToString) > 0, "D", "C"), _
                                dr("S_CGSTID").ToString, (Vatamt / 2), 0, 0, 0, "RV", "", , , , , , , Remark1, Remark2, DtTempTb)
                        End If
                        InsertIntoAccTran(frmFlag, tran, billdate, batchno, billCashCounterId, billCostId, VatExm, tNo, IIf(Val(ro!AMOUNT.ToString) > 0, "C", "D"), _
                            Accode, (Val(ro!AMOUNT.ToString) - Val(ro!GST.ToString)), 0, 0, 0, "OR", "", , , , , , , Remark1, Remark2, DtTempTb)
                    Else
                        InsertIntoAccTran(frmFlag, tran, billdate, batchno, billCashCounterId, billCostId, VatExm, tNo, IIf(Val(ro!VAT.ToString) > 0, "D", "C"), _
                            objGPack.GetSqlValue("SELECT STAXID FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & ro.Item("CATNAME").ToString & "'", , , tran) _
                            , Val(ro!VAT.ToString), 0, 0, 0, "RV", "", , , , , , , Remark1, Remark2, DtTempTb)
                        InsertIntoAccTran(frmFlag, tran, billdate, batchno, billCashCounterId, billCostId, VatExm, tNo, IIf(Val(ro!AMOUNT.ToString) > 0, "C", "D"), _
                            Accode, Val(ro!AMOUNT.ToString), 0, 0, 0, "OR", "", , , , , , , Remark1, Remark2, DtTempTb)
                    End If
            End Select
            InsertIntoReceipt(ro, tNo, billdate, billCostId, billCashCounterId, batchno, VatExm, tran)
            If ro.Item("MODE").ToString = "WEIGHT" Or ro.Item("MODE").ToString = "AMOUNT" Then
                InsertIntoOustanding(frmFlag, tran, tNo, billdate, billCostId, billCashCounterId, batchno, VatExm, _
                "A", orderNo, 0, "R", "OR", Val(ro!GRSWT.ToString) _
                , Val(ro!NETWT.ToString) _
                , objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & ro.Item("CATNAME").ToString & "'", , , tran) _
                , FixRate, Val(ro!AMOUNT.ToString), , , Val(ro!PURITY.ToString), , , Accode, EmpId)
            ElseIf ro.Item("MODE").ToString = "PURCHASE" Or ro.Item("MODE").ToString = "EXCHANGE" Then
                'PURCHASE AND EXCHANGE
                InsertIntoOustanding(frmFlag, tran, IIf(alterno <> 0, alterno, tNo), billdate, billCostId, billCashCounterId, batchno, VatExm, _
                "A", orderNo, Val(ro!AMOUNT.ToString) - Val(ro!GST.ToString), "R", "OR", 0 _
                , 0 _
                , objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & ro.Item("CATNAME").ToString & "'", , , tran) _
                , FixRate, Val(ro!AMOUNT.ToString) - Val(ro!GST.ToString), , , Val(ro!PURITY.ToString), , , Accode, EmpId, , , , Val(ro!GST.ToString))
            End If
        Next
    End Function
    Public Sub InsertIntoReceipt(ByVal purRo As DataRow, ByVal tNo As Integer, ByVal billDate As Date, ByVal billCostId As String _
    , ByVal billCashCounterId As String, ByVal batchNo As String, ByVal vatExm As String, ByVal tran As OleDbTransaction)
        With purRo
            Dim puEx As String = Nothing
            Dim tranType As String
            If .Item("MODE").ToString = "WEIGHT" Or .Item("MODE").ToString = "AMOUNT" Then
                tranType = "AD"
            ElseIf .Item("MODE").ToString = "EXCHANGE" Then
                tranType = "SR"
                puEx = "X"
            Else
                tranType = "PU"
                puEx = "P"
            End If
            Dim issSno As String = GetNewSno(TranSnoType.RECEIPTCODE, tran)
            Dim catCode As String = objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & .Item("CATNAME").ToString & "'", , , tran)
            If Val(.Item("GRSWT").ToString) = 0 Then Exit Sub
            strSql = " INSERT INTO " & cnStockDb & "..RECEIPT"
            strSql += " ("
            strSql += " SNO,TRANNO,TRANDATE,TRANTYPE,PCS"
            strSql += " ,GRSWT,NETWT,LESSWT,PUREWT"
            strSql += " ,TAGNO,ITEMID,SUBITEMID,WASTPER"
            strSql += " ,WASTAGE,MCGRM,MCHARGE,AMOUNT"
            strSql += " ,RATE,BOARDRATE,SALEMODE,GRSNET"
            strSql += " ,TRANSTATUS,REFNO,REFDATE,COSTID"
            strSql += " ,COMPANYID,FLAG,EMPID,TAGGRSWT"
            strSql += " ,TAGNETWT,TAGRATEID,TAGSVALUE,TAGDESIGNER"
            strSql += " ,ITEMCTRID,ITEMTYPEID,PURITY,TABLECODE"
            strSql += " ,INCENTIVE,WEIGHTUNIT,CATCODE,OCATCODE"
            strSql += " ,ACCODE,ALLOY,BATCHNO,REMARK1"
            strSql += " ,REMARK2,USERID,UPDATED,UPTIME,SYSTEMID,DISCOUNT,RUNNO,CASHID,TAX"
            strSql += " ,DUSTWT,PUREXCH,MAKE,STNAMT,MISCAMT,METALID,STONEUNIT,APPVER"
            strSql += " ,MELTWT"
            strSql += " )"
            strSql += " VALUES("
            strSql += " '" & issSno & "'" ''SNO
            strSql += " ," & tNo & "" 'TRANNO
            strSql += " ,'" & GetEntryDate(billDate, tran).ToString("yyyy-MM-dd") & "'" 'TRANDATE
            strSql += " ,'" & tranType & "'" 'TRANTYPE
            strSql += " ," & Val(.Item("PCS").ToString) & "" 'PCS
            strSql += " ," & Val(.Item("GRSWT").ToString) & "" 'GRSWT
            strSql += " ," & Val(.Item("NETWT").ToString) & "" 'NETWT
            strSql += " ," & Val(.Item("LESSWT").ToString) & "" 'LESSWT
            strSql += " ," & Val(.Item("NETWT").ToString) & "" 'PUREWT '0
            strSql += " ,''" 'TAGNO
            strSql += " ," & Val(.Item("ITEM").ToString) & "" 'ITEMID
            strSql += " ," & Val(.Item("SubItem").ToString) & "" 'SUBITEMID
            strSql += " ," & Val(.Item("WASTAGEPER").ToString) & "" 'WASTPER
            strSql += " ," & Val(.Item("WASTAGE").ToString) & "" 'WASTAGE
            strSql += " ,0" 'MCGRM
            strSql += " ,0" 'MCHARGE
            strSql += " ," & Val(.Item("GROSSAMT").ToString) & "" 'AMOUNT
            strSql += " ," & Val(.Item("RATE").ToString) & "" 'RATE
            strSql += " ," & Val(.Item("BOARDRATE").ToString) & "" 'BOARDRATE
            strSql += " ,''" 'SALEMODE
            strSql += " ,''" 'GRSNET
            strSql += " ,''" 'TRANSTATUS ''
            strSql += " ,''" 'REFNO ''
            strSql += " ,NULL" 'REFDATE NULL
            strSql += " ,'" & billCostId & "'" 'COSTID 
            strSql += " ,'" & strCompanyId & "'" 'COMPANYID
            strSql += " ,'" & .Item("FLAG").ToString & "'" 'FLAG 
            strSql += " ," & Val(.Item("EMPID").ToString) & "" 'EMPID
            strSql += " ,0" 'TAGGRSWT
            strSql += " ,0" 'TAGNETWT
            strSql += " ,0" 'TAGRATEID
            strSql += " ,0" 'TAGSVALUE
            strSql += " ,''" 'TAGDESIGNER  
            strSql += " ,0" 'ITEMCTRID
            strSql += " ," & Val(.Item("ITEMTYPEID").ToString) & "" 'ITEMTYPEID
            strSql += " ," & Val(.Item("PURITY").ToString) & "" 'PURITY
            strSql += " ,''" 'TABLECODE
            strSql += " ,''" 'INCENTIVE
            strSql += " ,''" 'WEIGHTUNIT
            strSql += " ,'" & catCode & "'" 'CATCODE
            strSql += " ,''" 'OCATCODE
            strSql += " ,''" 'ACCODE
            strSql += " ," & Val(.Item("ALLOYWT").ToString) & "" 'ALLOYWT
            strSql += " ,'" & batchNo & "'" 'BATCHNO
            strSql += " ,'" & .Item("REMARK1").ToString & "'" 'REMARK1
            strSql += " ,''" 'REMARK2
            strSql += " ,'" & userId & "'" 'USERID
            strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
            strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
            strSql += " ,'" & systemId & "'" 'SYSTEMID
            strSql += " ,0" 'DISCOUNT
            strSql += " ,''" 'RUNNO
            strSql += " ,'" & billCashCounterId & "'" 'CASHID
            strSql += " ," & Val(.Item("VAT").ToString) & "" 'TAX
            strSql += " ," & Val(.Item("DUSTWT").ToString) & "" 'DUSTWT
            strSql += " ,'" & puEx & "'" 'PUREXCH
            strSql += " ,''" 'MAKE
            strSql += " ,0" 'STONEAMT
            strSql += " ,0" 'MISCAMT
            strSql += " ,'" & objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE= '" & catCode & "'", , , tran) & "'" 'METALID
            strSql += " ,'" & objGPack.GetSqlValue("SELECT CASE WHEN DIASTNTYPE = 'T' THEN 'G' WHEN DIASTNTYPE = 'D' THEN 'C' WHEN DIASTNTYPE = 'P' THEN 'C' ELSE '' END AS STONEUNIT FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = '" & catCode & "'", , , tran) & "'" 'STONEUNIT
            strSql += " ,'" & VERSION & "'" 'APPVER
            strSql += " ," & Val(.Item("MELTWT").ToString) & "" 'MELTWT
            strSql += " )"
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, billCostId)

            ''Stone
            If tranType = "PU" Then
                For Each stRow As DataRow In dtStoneDetails.Rows
                    If .Item("KEYNO").ToString = stRow("KEYNO") Then
                        stRow("TRANTYPE") = tranType
                        InsertStoneDetails(issSno, tNo, stRow, batchNo)
                    End If
                Next
            End If

            If .Item("ESTNO").ToString <> "" Then
                strSql = " UPDATE " & cnStockDb & "..ESTRECEIPT SET BATCHNO = '" & batchNo & "'"
                strSql += " WHERE TRANNO = " & Val(.Item("ESTNO").ToString) & ""
                'If objSoftKeys.HasEstPost Then strSql += " AND TRANDATE = '" & GetEntryDate(billDate, tran).ToString("yyyy-MM-dd") & "'"
                strSql += " AND TRANTYPE = 'PU'"
                Dim Cmd As OleDbCommand
                Cmd = New OleDbCommand(strSql, cn, tran)
                Cmd.ExecuteNonQuery()
            End If
        End With
    End Sub

    Private Sub InsertStoneDetails(ByVal issSno As String, ByVal TNO As Integer, ByVal stRow As DataRow, ByVal batchNo As String)
        Dim stnCatCode As String = Nothing
        Dim stnItemId As Integer = 0
        Dim stnSubItemid As Integer = 0
        ''Find stnCatCode,Itemid
        strSql = " SELECT ITEMID,CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & stRow.Item("ITEM").ToString & "'"
        Dim dr As DataRow
        dr = GetSqlRow(strSql, cn, tran)
        If Not dr Is Nothing Then
            stnCatCode = dr("CATCODE").ToString
            stnItemId = Val(dr("ITEMID").ToString)
        End If

        ''Find subItemId
        strSql = " SELECT ISNULL(SUBITEMID,0)AS SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & stRow.Item("SUBITEM").ToString & "' AND ITEMID = " & stnItemId & ""
        stnSubItemid = Val(objGPack.GetSqlValue(strSql, , , tran))

        Dim sno As String = Nothing
        strSql = " INSERT INTO " & cnStockDb & "..RECEIPTSTONE"
        sno = GetNewSno(TranSnoType.RECEIPTSTONECODE, tran)

        strSql += " ("
        strSql += " SNO,ISSSNO,TRANNO,TRANDATE,TRANTYPE"
        strSql += " ,STNPCS,STNWT,STNRATE,STNAMT"
        strSql += " ,STNITEMID,STNSUBITEMID,CALCMODE,STONEUNIT"
        strSql += " ,STONEMODE,TRANSTATUS,COSTID,COMPANYID"
        strSql += " ,BATCHNO,SYSTEMID,CATCODE,TAX,APPVER,DISCOUNT)"
        strSql += " VALUES"
        strSql += " ("
        strSql += " '" & sno & "'" ''SNO
        strSql += " ,'" & issSno & "'" 'ISSSNO
        strSql += " ," & TNO & "" 'TRANNO
        strSql += " ,'" & BillDate & "'" 'TRANDATE
        strSql += " ,'PU'" 'TRANTYPE 
        strSql += " ,0" 'STNPCS
        strSql += " ," & Val(stRow.Item("WEIGHT").ToString) 'STNWT
        strSql += " ,0" 'STNRATE
        strSql += " ," & Val(stRow.Item("AMOUNT").ToString) 'STNAMT
        strSql += " ," & stnItemId 'STNITEMID
        strSql += " ," & stnSubItemid  'STNSUBITEMID
        strSql += " ,'" & Mid(stRow.Item("CALC").ToString, 1, 1) & "'" 'CALCMODE
        strSql += " ,'" & Mid(stRow.Item("UNIT").ToString, 1, 1) & "'" 'STONEUNIT
        strSql += " ,''" 'STONEMODE 
        strSql += " ,''" 'TRANSTATUS
        strSql += " ,'" & cnCostId & "'" 'COSTID
        strSql += " ,'" & strCompanyId & "'" 'COMPANYID
        strSql += " ,'" & batchNo & "'" 'BATCHNO
        strSql += " ,'" & systemId & "'" 'SYSTEMID
        strSql += " ,'" & stnCatCode & "'" 'CATCODE
        strSql += " ,0" 'TAX
        strSql += " ,'" & VERSION & "'" 'APPVER
        strSql += " ,0" 'DISCOUNT
        strSql += " )"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)

    End Sub

    Private Sub InsertIntoOustanding _
   (ByVal frmFlag As String, ByVal tran As OleDbTransaction, ByVal tranNo As Integer, ByVal billDate As Date, ByVal billCostId As String, ByVal billCashCounterId As String, ByVal batchNo As String, ByVal vatExm As String, _
   ByVal tType As String, ByVal RunNo As String, ByVal Amount As Double, _
   ByVal RecPay As String, _
   ByVal Paymode As String, _
   Optional ByVal GrsWt As Double = 0, _
   Optional ByVal NetWt As Double = 0, _
   Optional ByVal CatCode As String = Nothing, _
   Optional ByVal Rate As Double = Nothing, _
   Optional ByVal Value As Double = Nothing, _
   Optional ByVal refNo As String = Nothing, _
   Optional ByVal refDate As String = Nothing, _
   Optional ByVal purity As Double = Nothing, _
   Optional ByVal proId As Integer = Nothing, _
   Optional ByVal dueDate As String = Nothing, _
   Optional ByVal aCCode As String = Nothing, _
   Optional ByVal EmpId As Integer = Nothing, _
   Optional ByVal Remark1 As String = Nothing, _
   Optional ByVal Remark2 As String = Nothing, _
   Optional ByVal ADVFIXWTPER As Double = 0, _
   Optional ByVal GSTVAL As Double = 0 _
   )
        If Amount = 0 And GrsWt = 0 Then Exit Sub
        strSql = " INSERT INTO " & cnAdminDb & "..OUTSTANDING"
        strSql += " ("
        strSql += " SNO,TRANNO,TRANDATE,TRANTYPE,RUNNO"
        strSql += " ,AMOUNT,GRSWT,NETWT,RECPAY"
        strSql += " ,REFNO,REFDATE,EMPID"
        strSql += " ,TRANSTATUS,PURITY,CATCODE,BATCHNO"
        strSql += " ,USERID,UPDATED,UPTIME,SYSTEMID"
        strSql += " ,RATE,VALUE,CASHID,REMARK1,REMARK2,CTRANCODE"
        strSql += " ,DUEDATE,APPVER,COMPANYID,ACCODE,COSTID,FROMFLAG"
        strSql += " ,FLAG,PAYMODE,ADVFIXWTPER,GSTVAL)"
        strSql += " VALUES"
        strSql += " ("
        strSql += " '" & GetNewSno(TranSnoType.OUTSTANDINGCODE, tran, "GET_ADMINSNO_TRAN") & "'" ''SNO
        strSql += " ," & tranNo & "" 'TRANNO
        strSql += " ,'" & GetEntryDate(BillDate, tran).ToString("yyyy-MM-dd") & "'" 'TRANDATE
        strSql += " ,'" & tType & "'" 'TRANTYPE
        strSql += " ,'" & RunNo & "'" 'RUNNO
        strSql += " ," & Math.Abs(Amount) & "" 'AMOUNT
        strSql += " ," & Math.Abs(GrsWt) & "" 'GRSWT
        strSql += " ," & Math.Abs(NetWt) & "" 'NETWT
        strSql += " ,'" & RecPay & "'" 'RECPAY
        strSql += " ,'" & refNo & "'" 'REFNO
        If refDate <> Nothing Then
            strSql += " ,'" & refDate & "'" 'REFDATE
        Else
            strSql += " ,NULL" 'REFDATE
        End If

        strSql += " ," & EmpId & "" 'EMPID
        strSql += " ,''" 'TRANSTATUS
        strSql += " ," & purity & "" 'PURITY
        strSql += " ,'" & CatCode & "'" 'CATCODE
        strSql += " ,'" & batchNo & "'" 'BATCHNO
        strSql += " ," & userId & "" 'USERID

        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
        strSql += " ,'" & systemId & "'" 'SYSTEMID
        strSql += " ," & Rate & "" 'RATE
        strSql += " ," & Value & "" 'VALUE
        strSql += " ,'" & billCashCounterId & "'" 'CASHID
        strSql += " ,'" & Remark1 & "'" 'REMARK1
        strSql += " ,'" & Remark2 & "'" 'REMARK1
        strSql += " ," & proId & "" 'CTRANCODE
        If DueDate <> Nothing Then
            strSql += " ,'" & DueDate & "'" 'DUEDATE
        Else
            strSql += " ,NULL" 'DUEDATE
        End If
        strSql += " ,'" & VERSION & "'" 'APPVER
        strSql += " ,'" & strCompanyId & "'" 'COMPANYID
        strSql += " ,'" & aCCode & "'" 'Accode
        strSql += " ,'" & billCostId & "'" 'COSTID
        strSql += " ,'" & frmFlag & "'" ' FROMFLAG
        strSql += " ,'" & FurtherAdvance & "'" 'FLAG FOR FURTHER ADVANCE
        strSql += " ,'" & Paymode & "'" ' PAYMODE
        strSql += " ," & ADVFIXWTPER   ' ADVFIXWTPER
        strSql += " ," & GSTVAL   ' GSTVAL
        strSql += " )"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, billCostId)
    End Sub

    Private Sub InsertIntoAccTran _
  (ByVal frmFlag As String, ByVal tran As OleDbTransaction, ByVal billdate As Date, ByVal batchno As String, ByVal billCashcounterId As String, ByVal billCostId As String, ByVal vatExm As String, ByVal tNo As Integer, _
  ByVal tranMode As String, _
  ByVal accode As String, _
  ByVal amount As Double, _
  ByVal pcs As Integer, _
  ByVal grsWT As Double, _
  ByVal netWT As Double, _
  ByVal payMode As String, _
  ByVal contra As String, _
  Optional ByVal refNo As String = Nothing, Optional ByVal refDate As String = Nothing, _
  Optional ByVal chqCardNo As String = Nothing, _
  Optional ByVal chqDate As String = Nothing, _
  Optional ByVal chqCardId As Integer = Nothing, _
  Optional ByVal chqCardRef As String = Nothing, _
  Optional ByVal Remark1 As String = Nothing, _
  Optional ByVal REmark2 As String = Nothing, _
  Optional ByVal Dttemptb As String = Nothing _
  )
        If amount = 0 Then Exit Sub
        If Dttemptb = Nothing Then
            strSql = " INSERT INTO " & cnStockDb & "..ACCTRAN"
        Else
            strSql = " INSERT INTO " & cnStockDb & ".." & Dttemptb & ""
        End If
        strSql += " ("
        strSql += " SNO,TRANNO,TRANDATE,TRANMODE,ACCODE"
        strSql += " ,AMOUNT,PCS,GRSWT,NETWT"
        strSql += " ,REFNO,REFDATE,PAYMODE,CHQCARDNO"
        strSql += " ,CARDID,CHQCARDREF,CHQDATE,BRSFLAG"
        strSql += " ,RELIASEDATE,FROMFLAG,REMARK1,REMARK2"
        strSql += " ,CONTRA,BATCHNO,USERID,UPDATED"
        strSql += " ,UPTIME,SYSTEMID,CASHID,COSTID,APPVER,COMPANYID"
        strSql += " )"
        strSql += " VALUES"
        strSql += " ("
        strSql += " '" & GetNewSno(TranSnoType.ACCTRANCODE, tran) & "'" ''SNO
        strSql += " ," & tNo & "" 'TRANNO 
        strSql += " ,'" & GetEntryDate(billdate, tran).ToString("yyyy-MM-dd") & "'" 'TRANDATE
        strSql += " ,'" & tranMode & "'" 'TRANMODE
        strSql += " ,'" & accode & "'" 'ACCODE
        strSql += " ," & Math.Abs(amount) & "" 'AMOUNT
        strSql += " ," & Math.Abs(pcs) & "" 'PCS
        strSql += " ," & Math.Abs(grsWT) & "" 'GRSWT
        strSql += " ," & Math.Abs(netWT) & "" 'NETWT
        strSql += " ,'" & refNo & "'" 'REFNO
        If refDate = Nothing Then
            strSql += " ,NULL" 'REFDATE
        Else
            strSql += " ,'" & refDate & "'" 'REFDATE
        End If
        strSql += " ,'" & payMode & "'" 'PAYMODE
        strSql += " ,'" & chqCardNo & "'" 'CHQCARDNO
        strSql += " ," & chqCardId & "" 'CARDID
        strSql += " ,'" & chqCardRef & "'" 'CHQCARDREF
        If chqDate = Nothing Then
            strSql += " ,NULL" 'CHQDATE
        Else
            strSql += " ,'" & chqDate & "'" 'CHQDATE
        End If
        strSql += " ,''" 'BRSFLAG
        strSql += " ,NULL" 'RELIASEDATE
        strSql += " ,'" & frmFlag & "'" 'FROMFLAG
        strSql += " ,'" & Remark1 & "'" 'REMARK1
        strSql += " ,'" & REmark2 & "'" 'REMARK2
        strSql += " ,'" & contra & "'" 'CONTRA
        strSql += " ,'" & batchno & "'" 'BATCHNO
        strSql += " ,'" & userId & "'" 'USERID
        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
        strSql += " ,'" & systemId & "'" 'SYSTEMID
        strSql += " ,'" & billCashcounterId & "'" 'CASHID
        strSql += " ,'" & billCostId & "'" 'COSTID
        strSql += " ,'" & VERSION & "'" 'APPVER
        strSql += " ,'" & strCompanyId & "'" 'COMPANYID
        strSql += " )"
        If Dttemptb = Nothing Then
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, billCostId)
        Else
            Dim cmd As OleDbCommand
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()
        End If
        strSql = ""
    End Sub

    Private Sub Transfer_GotFocus(ByVal sender As Object, ByVal e As EventArgs) Handles _
    txtPUCategory.GotFocus, txtPUPcs_NUM.GotFocus, txtPUGrsWt_WET.GotFocus, txtPUDustWt_WET.GotFocus, txtPuPurity_PER.GotFocus _
    , txtPuWastage_PER.GotFocus, txtPUWastage_WET.GotFocus, txtPUNetWt_WET.GotFocus, txtPURate_AMT.GotFocus, txtPUVat_AMT.GotFocus
        If cmbPurMode.Text = "TRANSFER" Or cmbPurMode.Text = "ADVANCE" Then txtPUAmount_AMT.Focus()
    End Sub

    Private Sub txtPUWastage_WET_TextChanged1(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPUWastage_WET.TextChanged
        CalcPUNetWt()
    End Sub
    Private Sub CalcPuAlloy()

        If Val(txtPUGrsWt_WET.Text) = 0 Then Exit Sub
        If cmbPurMode.Text <> "PURCHASE" And cmbPurMode.Text <> "EXCHANGE" And cmbPurMode.Text <> "WEIGHT" Then Exit Sub

        If txtPUCategory.Text <> "" Then
            Dim metalId As String = objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & txtPUCategory.Text & "'")
            Dim _puralloymetal As String = GetAdmindbSoftValue("PURALLOY_METAL", "").ToString
            If _puralloymetal <> "" And Not _puralloymetal.Contains(metalId.ToString) Then
                Exit Sub
            End If
        End If

        Dim AlloyWt As Decimal = Nothing
        If PURALLOYPER <> 0 And Val(txtPuPurity_PER.Text) <> 91.6 Then
            AlloyWt = Val(txtPUGrsWt_WET.Text) - Val(txtPUDustWt_WET.Text) - Val(txtPUStoneWt_WET.Text) - Val(txtPUWastage_WET.Text)
            AlloyWt = AlloyWt * (PURALLOYPER / 100)
        End If
        PurAlloy = AlloyWt
    End Sub

    Private Sub frmOrderAdvance_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If cmbPurMode.Focused Then Exit Sub
            If txtPUAmount_AMT.Focused Then Exit Sub
            If txtPUCategory.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub StuddedToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StuddedToolStripMenuItem.Click
        ShowStoneDia()
        Me.SelectNextControl(txtPUGrsWt_WET, True, True, True, True)
    End Sub
End Class