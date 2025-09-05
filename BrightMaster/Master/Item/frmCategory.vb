Imports System.Data.OleDb
Public Class frmCategory
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim flagSave As Boolean = False
    Dim CatCode As String = Nothing
    Dim tran As OleDbTransaction = Nothing
    Dim catViewFilt As Boolean = IIf(GetAdmindbSoftValue("CAT_VIEW_FILTER", "N", ) = "Y", True, False)
    Dim POS_ED_SEPPOST As Boolean = IIf(GetAdmindbSoftValue("POS_ED_SEPPOST", "N") = "Y", True, False)

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        objGPack.TextClear(grpInfo)
        funcGridStyle(gridView)
        gridView.BorderStyle = BorderStyle.None


        tabGeneral.BackgroundImage = bakImage
        tabView.BackgroundImage = bakImage
        tabGeneral.BackgroundImageLayout = ImageLayout.Stretch
        tabView.BackgroundImageLayout = ImageLayout.Stretch


        cmbSmithTrans.Items.Add("BOTH")
        cmbSmithTrans.Items.Add("ISSUE")
        cmbSmithTrans.Items.Add("RECEIPT")
        cmbSmithTrans.Text = "BOTH"

        cmbStockGroup.Items.Add("BAR")
        cmbStockGroup.Items.Add("OLD")
        cmbStockGroup.Items.Add("REPAIR")
        cmbStockGroup.Items.Add("ORDER")
        cmbStockGroup.Items.Add("ORNAMENTS")
        cmbStockGroup.Items.Add("PARTY METALS")
        cmbStockGroup.Text = "BAR"

        cmbSalesPurchase.Items.Add("BOTH")
        cmbSalesPurchase.Items.Add("SALES")
        cmbSalesPurchase.Items.Add("PURCHASE")
        cmbSalesPurchase.Text = "BOTH"

        cmbLedgerPrint.Items.Add("YES")
        cmbLedgerPrint.Items.Add("NO")
        cmbLedgerPrint.Text = "YES"

        cmbHaving.Items.Add("")
        cmbHaving.Items.Add("Wastage")
        cmbHaving.Items.Add("Alloy")
        cmbHaving.Text = ""


        cmbOpenMetal_Man.Items.Clear()
        cmbOpenMetal_Man.Items.Add("ALL")
        strSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE ISNULL(ACTIVE,'') <> 'N' ORDER BY DISPLAYORDER"
        objGPack.FillCombo(strSql, cmbOpenMetal_Man, False, False)

        cmbCategoryGroup.Items.Clear()
        cmbCategoryGroup.Items.Add("[NONE]")
        strSql = " SELECT CGROUPNAME FROM " & cnAdminDb & "..CATEGORYGROUP ORDER BY CGROUPNAME"
        objGPack.FillCombo(strSql, cmbCategoryGroup, False, False)

        pnlStone.Enabled = False
    End Sub
    Function funcAcNameGeneration() As Integer
        Dim dt As New DataTable
        dt.Clear()
        If flagSave = False Then
            strSql = " select"
            strSql += " isnull((select metalId from " & cnAdminDb & "..metalmast where metalName = '" & cmbMetal.Text & "')+"
            If cmbPurity.Text <> "" Then
                strSql += " (select purityId from " & cnAdminDb & "..purityMast where PurityName = '" & cmbPurity.Text & "')+"
            End If
            strSql += " (select TaxCode from " & cnAdminDb & "..TaxMast where TaxName = '" & cmbTaxMode.Text & "'),'')as Code,"
            strSql += " stax,ssc,sasc,ptax,psc,pasc from " & cnAdminDb & "..taxmast where taxname = '" & cmbTaxMode.Text & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            cmbSAC_OWN.Items.Clear()
            cmbSTax_OWN.Items.Clear()
            cmbSSurcharge_OWN.Items.Clear()
            cmbSASCharge_OWN.Items.Clear()

            cmbPAC_OWN.Items.Clear()
            cmbPTax_OWN.Items.Clear()
            cmbPSurcharge_OWN.Items.Clear()
            cmbPASCharge_OWN.Items.Clear()
            funcLoadAcHeadNames()
            If dt.Rows.Count > 0 Then
                Dim catName As String = txtName__Man.Text 'cmbMetal.Text + " " + cmbPurity.Text + " " + cmbTaxMode.Text
                'txtName.Text = catName

                ''Sales Account
                cmbSAC_OWN.Items.Add("SALES " + catName + " A/C")
                cmbSAC_OWN.Text = "SALES " + catName + " A/C"

                cmbS_SGST_OWN.Items.Add("SALES " + catName + " SGST A/C")
                cmbS_SGST_OWN.Text = "SALES " + catName + " SGST A/C"
                cmbS_CGST_OWN.Items.Add("SALES " + catName + " CGST A/C")
                cmbS_CGST_OWN.Text = "SALES " + catName + " CGST A/C"
                cmbS_IGST_OWN.Items.Add("SALES " + catName + " IGST A/C")
                cmbS_IGST_OWN.Text = "SALES " + catName + " IGST A/C"


                txtSTax_Per.Text = dt.Rows(0).Item("STAX").ToString
                cmbSTax_OWN.Items.Add("VAT OUTPUT " + catName + " A/C")
                cmbSTax_OWN.Text = "VAT OUTPUT " + catName + " A/C"

                txtSSurcharge_Per.Text = dt.Rows(0).Item("SSC").ToString
                cmbSSurcharge_OWN.Items.Add("SALES " + catName + " SC A/C")
                cmbSSurcharge_OWN.Text = "SALES " + catName + " SC A/C"

                txtSASCharge_Per.Text = dt.Rows(0).Item("SASC").ToString
                cmbSASCharge_OWN.Items.Add("SALES  " + catName + " ADL SC A/C")
                cmbSASCharge_OWN.Text = "SALES  " + catName + " ADL SC A/C"


                ''PURCHASE ACCOUNT
                cmbPAC_OWN.Items.Add("PURCHASE " + catName + " A/C")
                cmbPAC_OWN.Text = "PURCHASE " + catName + " A/C"

                cmbP_SGST_OWN.Items.Add("PURCHASE " + catName + " SGST A/C")
                cmbP_SGST_OWN.Text = "PURCHASE " + catName + " SGST A/C"
                cmbP_CGST_OWN.Items.Add("PURCHASE " + catName + " CGST A/C")
                cmbP_CGST_OWN.Text = "PURCHASE " + catName + " CGST A/C"
                cmbP_IGST_OWN.Items.Add("PURCHASE " + catName + " IGST A/C")
                cmbP_IGST_OWN.Text = "PURCHASE " + catName + " IGST A/C"

                txtPTax_Per.Text = dt.Rows(0).Item("STAX").ToString
                cmbPTax_OWN.Items.Add("VAT INPUT " + catName + " A/C")
                cmbPTax_OWN.Text = "VAT INPUT " + catName + " A/C"

                txtPSurcharge_Per.Text = dt.Rows(0).Item("SSC").ToString
                cmbPSurcharge_OWN.Items.Add("PURCHASE " + catName + " SC A/C")
                cmbPSurcharge_OWN.Text = "PURCHASE " + catName + " SC A/C"

                txtPASCharge_Per.Text = dt.Rows(0).Item("SASC").ToString
                cmbPASCharge_OWN.Items.Add("PURCHASE  " + catName + " ADL SC A/C")
                cmbPASCharge_OWN.Text = "PURCHASE  " + catName + " ADL SC A/C"
            End If
        End If
    End Function

    Function funcAcNameGenerationMis() As Integer
        Dim dt As New DataTable
        dt.Clear()
        If flagSave Then
            strSql = " select"
            strSql += " isnull((select metalId from " & cnAdminDb & "..metalmast where metalName = '" & cmbMetal.Text & "')+"
            If cmbPurity.Text <> "" Then
                strSql += " (select purityId from " & cnAdminDb & "..purityMast where PurityName = '" & cmbPurity.Text & "')+"
            End If
            strSql += " (select TaxCode from " & cnAdminDb & "..TaxMast where TaxName = '" & cmbTaxMode.Text & "'),'')as Code,"
            strSql += " stax,ssc,sasc,ptax,psc,pasc from " & cnAdminDb & "..taxmast where taxname = '" & cmbTaxMode.Text & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                Dim catName As String = txtName__Man.Text 'cmbMetal.Text + " " + cmbPurity.Text + " " + cmbTaxMode.Text
                ''Sales Account
                If cmbS_SGST_OWN.Text = "" Then
                    cmbS_SGST_OWN.Items.Clear()
                    cmbS_SGST_OWN.Items.Add("SALES " + catName + " SGST A/C")
                    cmbS_SGST_OWN.Text = "SALES " + catName + " SGST A/C"
                End If
                If cmbS_CGST_OWN.Text = "" Then
                    cmbS_CGST_OWN.Items.Clear()
                    cmbS_CGST_OWN.Items.Add("SALES " + catName + " CGST A/C")
                    cmbS_CGST_OWN.Text = "SALES " + catName + " CGST A/C"
                End If
                If cmbS_IGST_OWN.Text = "" Then
                    cmbS_IGST_OWN.Items.Clear()
                    cmbS_IGST_OWN.Items.Add("SALES " + catName + " IGST A/C")
                    cmbS_IGST_OWN.Text = "SALES " + catName + " IGST A/C"
                End If

                ''PURCHASE ACCOUNT
                If cmbP_SGST_OWN.Text = "" Then
                    cmbP_SGST_OWN.Items.Clear()
                    cmbP_SGST_OWN.Items.Add("PURCHASE " + catName + " SGST A/C")
                    cmbP_SGST_OWN.Text = "PURCHASE " + catName + " SGST A/C"
                End If
                If cmbP_CGST_OWN.Text = "" Then
                    cmbP_CGST_OWN.Items.Clear()
                    cmbP_CGST_OWN.Items.Add("PURCHASE " + catName + " CGST A/C")
                    cmbP_CGST_OWN.Text = "PURCHASE " + catName + " CGST A/C"
                End If
                If cmbP_IGST_OWN.Text = "" Then
                    cmbP_IGST_OWN.Items.Clear()
                    cmbP_IGST_OWN.Items.Add("PURCHASE " + catName + " IGST A/C")
                    cmbP_IGST_OWN.Text = "PURCHASE " + catName + " IGST A/C"
                End If
            End If
        End If
    End Function

    Function funcClear()
        CatCode = Nothing
        txtName__Man.Clear()
        txtShortName.Clear()
        txtDiscount_Per.Clear()
        txtDisplayOrder_Num.Clear()

        cmbMetal.Items.Clear()
        cmbPurity.Items.Clear()
        cmbTaxMode.Items.Clear()


        ''Sales Account
        cmbSAC_OWN.Items.Clear()
        cmbSAC_OWN.Text = ""
        txtSTax_Per.Clear()
        cmbSTax_OWN.Items.Clear()
        cmbSTax_OWN.Text = ""
        txtSSurcharge_Per.Clear()
        cmbSSurcharge_OWN.Items.Clear()
        cmbSSurcharge_OWN.Text = ""
        txtSASCharge_Per.Clear()
        cmbSASCharge_OWN.Items.Clear()
        cmbSASCharge_OWN.Text = ""

        cmbS_SGST_OWN.Items.Clear()
        cmbS_SGST_OWN.Text = ""
        cmbS_CGST_OWN.Items.Clear()
        cmbS_CGST_OWN.Text = ""
        cmbS_IGST_OWN.Items.Clear()
        cmbS_IGST_OWN.Text = ""

        cmbP_SGST_OWN.Items.Clear()
        cmbP_SGST_OWN.Text = ""
        cmbP_CGST_OWN.Items.Clear()
        cmbP_CGST_OWN.Text = ""
        cmbP_IGST_OWN.Items.Clear()
        cmbP_IGST_OWN.Text = ""

        ''Purchase Account
        cmbPAC_OWN.Items.Clear()
        cmbPAC_OWN.Text = ""
        txtPTax_Per.Clear()
        cmbPTax_OWN.Items.Clear()
        cmbPTax_OWN.Text = ""
        txtPSurcharge_Per.Clear()
        cmbPSurcharge_OWN.Items.Clear()
        cmbPSurcharge_OWN.Text = ""
        txtPASCharge_Per.Clear()
        txtS_SGSTTax_Per.Text = ""
        txtS_CGSTTax_Per.Text = ""
        txtS_IGSTTax_Per.Text = ""
        txtP_SGSTTax_Per.Text = ""
        txtP_CGSTTax_Per.Text = ""
        txtP_IGSTTax_Per.Text = ""
        cmbPASCharge_OWN.Items.Clear()
        cmbPASCharge_OWN.Text = ""
        Return 0
    End Function
    Function funcNew()
        Dim dt As New DataTable
        dt.Clear()
        funcClear()
        strSql = " select isnull(max(displayorder),0)+1 as displayOrder from " & cnAdminDb & "..Category"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            txtDisplayOrder_Num.Text = dt.Rows(0).Item("displayOrder").ToString
        End If
        flagSave = False
        chkBill.Checked = False
        chkAccount.Checked = False
        pnlCategoryType.Enabled = True
        rbtStoneDiamond.Checked = True
        rbtMetal.Checked = True
        rbtMetal.Focus()

        strSql = " SELECT ISNULL(MAX(CATCODE),0)+1 CATCODE FROM " & cnAdminDb & "..CATEGORY "
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            CatCode = funcSetNumberStyle(dt.Rows(0).Item("CATCODE").ToString, 5)
        End If
        funcLoadTaxMode()
        cmbSmithTrans.Text = "BOTH"
        cmbStockGroup.Text = "BAR"
        cmbSalesPurchase.Text = "BOTH"
        cmbLedgerPrint.Text = "YES"
        cmbHaving.Text = ""        
        cmbActive.Text = "YES"
        chkEstimation.Checked = False
        chkBill.Checked = False
        chkAccount.Checked = False
        chkMetal.Checked = True
        rbtStone.Checked = True
        funcLoadAcHeadNames()
        cmbOpenMetal_Man.Text = "ALL"
        cmbCategoryGroup.Text = "[NONE]"
        ChkCmbViewFilter.Items.Clear()
        ChkCmbViewFilter.Items.Add("SALES")
        ChkCmbViewFilter.Items.Add("PURCHASE")
        ChkCmbViewFilter.Items.Add("ORDER")
        ChkCmbViewFilter.Text = ""
        If catViewFilt = False Then ChkCmbViewFilter.Visible = False : lblviewFilter.Visible = False
        If POS_ED_SEPPOST = False Then
            cmbESAC_OWN.Enabled = False
            cmbESTax_OWN.Enabled = False
            cmbEPAC_OWN.Enabled = False
            cmbEPTax_OWN.Enabled = False
        End If
        Me.SelectNextControl(Me, True, True, True, True)
        Return 0
    End Function
    Function funcExit()
        Me.Close()
        Return 0
    End Function
    Function funcSave() As Integer
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Function
        If objGPack.Validator_Check(Me) Then Exit Function
        strSql = " SELECT 'CHECK' FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetal.Text & "'"
        If cmbMetal.Text = "" Then
            MsgBox("MetalName Should not empty", MsgBoxStyle.Information)
            cmbMetal.Focus()
            Exit Function
        ElseIf Not objGPack.GetSqlValue(strSql).Length > 0 Then
            MsgBox("Invalid MetalName", MsgBoxStyle.Information)
            cmbMetal.Focus()
            Exit Function
        End If
        strSql = " SELECT 'CHECK' FROM " & cnAdminDb & "..TAXMAST WHERE TAXNAME = '" & cmbTaxMode.Text & "'"
        If Not objGPack.GetSqlValue(strSql).Length > 0 Then
            MsgBox("Invalid Taxmode", MsgBoxStyle.Information)
            cmbTaxMode.Focus()
            Exit Function
        End If
        strSql = " SELECT 'CHECK' FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & txtName__Man.Text & "' AND CATCODE <> '" & CatCode & "'"
        If objGPack.GetSqlValue(strSql).Length > 0 Then
            MsgBox("Catname Already Exist", MsgBoxStyle.Information)
            txtName__Man.Focus()
            Exit Function
        End If
        If cmbSAC_OWN.Text = "" Then
            MsgBox(objGPack.GetNextLable(Me, cmbSAC_OWN, False).Text + E0001, MsgBoxStyle.Information)
            cmbSAC_OWN.Select()
            Exit Function
        End If
        If cmbSTax_OWN.Text = "" Then
            MsgBox(objGPack.GetNextLable(Me, cmbSTax_OWN, False).Text + E0001, MsgBoxStyle.Information)
            cmbSTax_OWN.Select()
            Exit Function
        End If
        If Val(txtSSurcharge_Per.Text) > 0 And cmbSSurcharge_OWN.Text = "" Then
            MsgBox(objGPack.GetNextLable(Me, cmbSSurcharge_OWN, False).Text + E0001, MsgBoxStyle.Information)
            cmbSSurcharge_OWN.Select()
            Exit Function
        End If
        If Val(txtSASCharge_Per.Text) > 0 And cmbSASCharge_OWN.Text = "" Then
            MsgBox(objGPack.GetNextLable(Me, cmbSASCharge_OWN, False).Text + E0001, MsgBoxStyle.Information)
            cmbSASCharge_OWN.Select()
            Exit Function
        End If
        If cmbPAC_OWN.Text = "" Then
            MsgBox(objGPack.GetNextLable(Me, cmbPAC_OWN, False).Text + E0001, MsgBoxStyle.Information)
            cmbPAC_OWN.Select()
            Exit Function
        End If
        If cmbPTax_OWN.Text = "" Then
            MsgBox(objGPack.GetNextLable(Me, cmbPTax_OWN, False).Text + E0001, MsgBoxStyle.Information)
            cmbPTax_OWN.Select()
            Exit Function
        End If
        If Val(txtPSurcharge_Per.Text) > 0 And cmbPSurcharge_OWN.Text = "" Then
            MsgBox(objGPack.GetNextLable(Me, cmbPSurcharge_OWN, False).Text + E0001, MsgBoxStyle.Information)
            cmbPSurcharge_OWN.Select()
            Exit Function
        End If
        If funcAcNameLengthValidation() = True Then
            Exit Function
        End If
        If flagSave = False Then
            funcAdd()
        ElseIf flagSave = True Then
            funcUpdate()
        End If
    End Function
    Function funcAcNameCheck(ByVal obj As ComboBox) As Boolean
        If cmbSAC_OWN.Name <> obj.Name And obj.Text = cmbSAC_OWN.Text Then Return True
        If cmbSTax_OWN.Name <> obj.Name And obj.Text = cmbSTax_OWN.Text Then Return True
        If cmbSSurcharge_OWN.Name <> obj.Name And obj.Text = cmbSSurcharge_OWN.Text Then Return True
        If cmbPAC_OWN.Name <> obj.Name And obj.Text = cmbPAC_OWN.Text Then Return True
        If cmbPTax_OWN.Name <> obj.Name And obj.Text = cmbPTax_OWN.Text Then Return True
        If cmbPSurcharge_OWN.Name <> obj.Name And obj.Text = cmbPSurcharge_OWN.Text Then Return True
    End Function
    'Function funcAcNameDupCheck() As Boolean
    '    strSql = " SELECT 1 FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & txtName__Man.Text & "'"
    '    If funcCheckDup(strSql) = True Then
    '        MsgBox("Name Already Exist", MsgBoxStyle.Information)
    '        txtName__Man.Focus()
    '        Return True
    '    End If
    '    strSql = " Select 1 from " & cnAdminDb & "..AcHead where AcName = '" & cmbSAC_OWN.Text & "'"
    '    If funcCheckDup(strSql) = True Or funcAcNameCheck(cmbSAC_OWN) Then
    '        MsgBox("Sales Id Already Exist", MsgBoxStyle.Information)
    '        cmbSAC_OWN.Focus()
    '        Return True
    '    End If
    '    strSql = " Select 1 from " & cnAdminDb & "..AcHead where AcName = '" & cmbSTax_OWN.Text & "'"
    '    If funcCheckDup(strSql) = True Or funcAcNameCheck(cmbSTax_OWN) Then
    '        MsgBox("Sales Tax Id Already Exist", MsgBoxStyle.Information)
    '        cmbSTax_OWN.Focus()
    '        Return True
    '    End If
    '    strSql = " Select 1 from " & cnAdminDb & "..AcHead where AcName = '" & cmbSSurcharge_OWN.Text & "'"
    '    If funcCheckDup(strSql) = True Or funcAcNameCheck(cmbSSurcharge_OWN) Then
    '        MsgBox("Sales SurCharge Id Already Exist", MsgBoxStyle.Information)
    '        cmbSSurcharge_OWN.Focus()
    '        Return True
    '    End If
    '    '''''''''''''
    '    strSql = " Select 1 from " & cnAdminDb & "..AcHead where AcName = '" & cmbPAC_OWN.Text & "'"
    '    If funcCheckDup(strSql) = True Or funcAcNameCheck(cmbPAC_OWN) Then
    '        MsgBox("Purchase Id Already Exist", MsgBoxStyle.Information)
    '        cmbPAC_OWN.Focus()
    '        Return True
    '    End If
    '    strSql = " Select 1 from " & cnAdminDb & "..AcHead where AcName = '" & cmbPTax_OWN.Text & "'"
    '    If funcCheckDup(strSql) = True Or funcAcNameCheck(cmbPTax_OWN) Then
    '        MsgBox("Purchase Tax Id Already Exist", MsgBoxStyle.Information)
    '        cmbPTax_OWN.Focus()
    '        Return True
    '    End If
    '    strSql = " Select 1 from " & cnAdminDb & "..AcHead where AcName = '" & cmbPSurcharge_OWN.Text & "'"
    '    If funcCheckDup(strSql) = True Or funcAcNameCheck(cmbPSurcharge_OWN) Then
    '        MsgBox("Purchase SurCharge Id Already Exist", MsgBoxStyle.Information)
    '        cmbPSurcharge_OWN.Focus()
    '        Return True
    '    End If
    '    Return False
    'End Function
    Function funcCheckDup(ByVal qry As String) As Boolean
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(qry, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            Return True
        End If
        Return False
    End Function
    Function funcOpen()
        Dim dt As New DataTable
        dt.Clear()
        strSql = " SELECT"
        strSql += " CATCODE,CATNAME,SHORTNAME,DISPLAYORDER,"
        strSql += " (SELECT METALNAME FROM " & cnAdminDb & "..METALMAST AS M WHERE M.METALID = C.METALID)AS METALNAME,"
        strSql += " (SELECT TAXNAME FROM " & cnAdminDb & "..TAXMAST AS T WHERE T.TAXCODE = C.TAXID)AS TAXNAME,"
        strSql += " ISNULL((SELECT PURITYNAME FROM " & cnAdminDb & "..PURITYMAST AS P WHERE P.PURITYID = C.PURITYID),'')AS PURITYNAME,"
        strSql += " CASE WHEN CATMODE = 'S' THEN 'SALES'"
        strSql += " WHEN CATMODE = 'P' THEN 'PURCHASE' ELSE 'BOTH' END AS CATMODE,"
        strSql += " CASE WHEN TRANTYPE = 'I' THEN 'ISSUE'"
        strSql += " WHEN TRANTYPE = 'R' THEN 'RECEIPT' ELSE 'BOTH' END AS TRANTYPE,"
        strSql += " CASE WHEN CATGROUP = 'B' THEN 'BAR'"
        strSql += " WHEN CATGROUP = 'L' THEN 'OLD' "
        strSql += " WHEN CATGROUP = 'R' THEN 'REPAIR'"
        strSql += " WHEN CATGROUP = 'O' THEN 'ORDER'"
        strSql += " WHEN CATGROUP = 'P' THEN 'ORNAMENTS' ELSE 'PARTY METAL' END AS CATGROUP,"
        strSql += " DIASTNTYPE,"
        strSql += " CASE WHEN GS11 = 'Y' THEN 'YES' ELSE 'NO' END AS GS11,"
        strSql += " CASE WHEN GS12 = 'Y' THEN 'YES' ELSE 'NO' END AS GS12,"
        strSql += " CASE WHEN ALLOY = 'W' THEN 'WEIGHT'"
        strSql += " WHEN ALLOY = 'A' THEN 'ALLOY' ELSE '' END ALLOY,"
        strSql += " CASE WHEN LEDGERPRINT = 'Y' THEN 'YES' ELSE 'NO' END AS LEDGERPRINT,"
        strSql += " MINDISCPER,"
        strSql += " CASE WHEN ESTDISPLAY = 'Y' THEN 'YES' ELSE 'NO' END AS ESTDISPLAY,"
        strSql += " CASE WHEN BILLDISPLAY = 'Y' THEN 'YES' ELSE 'NO' END AS BILLDISPLAY,"
        strSql += " CASE WHEN ACCTDISPLAY = 'Y' THEN 'YES' ELSE 'NO' END AS ACCTDISPLAY,"
        strSql += " (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = C.SALESID)AS SALESIDNAME,"
        strSql += " (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = C.STAXID)AS STAXIDNAME,"
        strSql += " (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = C.SSCID)AS SSCIDNAME,"
        strSql += " (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = C.SASCID)AS SASCIDNAME,"
        strSql += " SALESTAX,SSC,SASC,SRETURNID,"
        strSql += " (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = C.PURCHASEID)AS PURCHASEIDNAME,"
        strSql += " (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = C.PTAXID)AS PTAXIDNAME,"
        strSql += " (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = C.PSCID)AS PSCIDNAME,"
        strSql += " (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = C.PASCID)AS PASCIDNAME,"
        strSql += " PTAX,PSC,PASC,SPRETURNID,"
        strSql += " OPNCODE,CLSCODE"
        strSql += " ,ISNULL((SELECT CGROUPNAME FROM " & cnAdminDb & "..CATEGORYGROUP WHERE CGROUPID = C.CGROUPID),'[NONE]')AS CGROUPNAME"
        strSql += " FROM " & cnAdminDb & "..CATEGORY AS C"
        If cmbOpenMetal_Man.Text <> "ALL" And cmbOpenMetal_Man.Text <> "" Then
            strSql += " WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbOpenMetal_Man.Text & "')"
        End If
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        gridView.DataSource = dt
        With gridView
            .Columns("CATCODE").Width = 70
            .Columns("CATNAME").Width = 300
            .Columns("SHORTNAME").Width = 200
            .Columns("DISPLAYORDER").Width = 50
            .Columns("DISPLAYORDER").HeaderText = "ORDER"
            .Columns("METALNAME").Width = 100
            .Columns("TAXNAME").Width = 100
            .Columns("PURITYNAME").Width = 150
        End With
        For CNT As Integer = 7 To gridView.ColumnCount - 1
            gridView.Columns(CNT).Visible = False
        Next
        Return 0
    End Function

    Function funcAdd()
        Dim ds As New Data.DataSet
        ds.Clear()

        Dim metalId As String = Nothing
        Dim purityId As String = Nothing
        Dim taxCode As String = Nothing
        ''Find MetalId
        strSql = " Select Metalid from " & cnAdminDb & "..MetalMast where"
        strSql += " MetalName = '" & cmbMetal.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(ds, "MetalId")
        If ds.Tables("MetalId").Rows.Count > 0 Then
            metalId = ds.Tables("MetalId").Rows(0).Item("MetalId").ToString
        End If

        ''Find PurityId
        strSql = " Select Purityid from " & cnAdminDb & "..PurityMast where"
        strSql += " PurityName = '" & cmbPurity.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(ds, "PurityId")
        If ds.Tables("PurityId").Rows.Count > 0 Then
            purityId = ds.Tables("PurityId").Rows(0).Item("PurityId").ToString
        End If

        ''Find TaxCode
        strSql = " Select TaxCode from " & cnAdminDb & "..TaxMast where"
        strSql += " TaxName = '" & cmbTaxMode.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(ds, "TaxCode")
        If ds.Tables("TaxCode").Rows.Count > 0 Then
            taxCode = ds.Tables("TaxCode").Rows(0).Item("TaxCode").ToString
        End If
        Try
            tran = cn.BeginTransaction
            Dim saId As String = Nothing
            Dim saTaxId As String = Nothing
            Dim saScId As String = Nothing
            Dim saSacId As String = Nothing

            Dim puId As String = Nothing
            Dim puTaxId As String = Nothing
            Dim puScId As String = Nothing
            Dim puSacId As String = Nothing
            Dim saEDId As String = Nothing
            Dim saEDTaxId As String = Nothing
            Dim puEDId As String = Nothing
            Dim puEDTaxId As String = Nothing

            Dim saId_SGST As String = Nothing
            Dim saId_CGST As String = Nothing
            Dim saId_IGST As String = Nothing
            Dim puId_SGST As String = Nothing
            Dim puId_CGST As String = Nothing
            Dim puId_IGST As String = Nothing

            If objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbSAC_OWN.Text & "'", , , tran) <> "" Then
                saId = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbSAC_OWN.Text & "'", , , tran)
            Else
                saId = "C" + CatCode.Remove(0, 1) & "01"
            End If
            If objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbSTax_OWN.Text & "'", , , tran) <> "" Then
                saTaxId = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbSTax_OWN.Text & "'", , , tran)
            Else
                saTaxId = "C" + CatCode.Remove(0, 1) & "02"
            End If
            If objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbSAC_OWN.Text & "'", , , tran) <> "" Then
                saSacId = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbSAC_OWN.Text & "'", , , tran)
            ElseIf Val(txtSASCharge_Per.Text) > 0 Then
                saSacId = "C" + CatCode.Remove(0, 1) & "03"
            End If
            If objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbSSurcharge_OWN.Text & "'", , , tran) <> "" Then
                saScId = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbSSurcharge_OWN.Text & "'", , , tran)
            ElseIf Val(txtSSurcharge_Per.Text) > 0 Then
                saScId = "C" + CatCode.Remove(0, 1) & "04"
            End If
            If objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbPAC_OWN.Text & "'", , , tran) <> "" Then
                puId = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbPAC_OWN.Text & "'", , , tran)
            Else
                puId = "C" + CatCode.Remove(0, 1) & "06"
            End If
            If objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbPTax_OWN.Text & "'", , , tran) <> "" Then
                puTaxId = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbPTax_OWN.Text & "'", , , tran)
            Else
                puTaxId = "C" + CatCode.Remove(0, 1) & "07"
            End If
            If objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbPAC_OWN.Text & "'", , , tran) <> "" Then
                puSacId = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbPAC_OWN.Text & "'", , , tran)
            ElseIf Val(txtPASCharge_Per.Text) > 0 Then
                puSacId = "C" + CatCode.Remove(0, 1) & "08"
            End If
            If objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbPSurcharge_OWN.Text & "'", , , tran) <> "" Then
                puScId = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbPSurcharge_OWN.Text & "'", , , tran)
            ElseIf Val(txtPSurcharge_Per.Text) > 0 Then
                puScId = "C" + CatCode.Remove(0, 1) & "09"
            End If
            strSql = "SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbPMC_OWN.Text & "'"
            Dim pMcId As String = objGPack.GetSqlValue(strSql, "ACCODE", "GMAKP", tran)


            If objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbS_SGST_OWN.Text & "'", , , tran) <> "" Then
                saId_SGST = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbS_SGST_OWN.Text & "'", , , tran)
            Else
                saId_SGST = "C" + CatCode.Remove(0, 1) & "11"
            End If

            If objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbS_CGST_OWN.Text & "'", , , tran) <> "" Then
                saId_CGST = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbS_CGST_OWN.Text & "'", , , tran)
            Else
                saId_CGST = "C" + CatCode.Remove(0, 1) & "12"
            End If

            If objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbS_IGST_OWN.Text & "'", , , tran) <> "" Then
                saId_IGST = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbS_IGST_OWN.Text & "'", , , tran)
            Else
                saId_IGST = "C" + CatCode.Remove(0, 1) & "13"
            End If

            If objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbP_SGST_OWN.Text & "'", , , tran) <> "" Then
                puId_SGST = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbP_SGST_OWN.Text & "'", , , tran)
            Else
                puId_SGST = "C" + CatCode.Remove(0, 1) & "14"
            End If

            If objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbP_CGST_OWN.Text & "'", , , tran) <> "" Then
                puId_CGST = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbP_CGST_OWN.Text & "'", , , tran)
            Else
                puId_CGST = "C" + CatCode.Remove(0, 1) & "15"
            End If

            If objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbP_IGST_OWN.Text & "'", , , tran) <> "" Then
                puId_IGST = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbP_IGST_OWN.Text & "'", , , tran)
            Else
                puId_IGST = "C" + CatCode.Remove(0, 1) & "16"
            End If


            ''ED
            If objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbESAC_OWN.Text & "'", , , tran) <> "" Then
                saEDId = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbESAC_OWN.Text & "'", , , tran)
            End If
            If objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbESTax_OWN.Text & "'", , , tran) <> "" Then
                saEDTaxId = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbESTax_OWN.Text & "'", , , tran)
            End If
            If objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbEPAC_OWN.Text & "'", , , tran) <> "" Then
                puEDId = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbEPAC_OWN.Text & "'", , , tran)
            End If
            If objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbEPTax_OWN.Text & "'", , , tran) <> "" Then
                puEDTaxId = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbEPTax_OWN.Text & "'", , , tran)
            End If

            Dim ViewFilter As String = ""
            If ChkCmbViewFilter.Visible = True Then
                For Each s As String In ChkCmbViewFilter.Text.Split(",")
                    ViewFilter += Mid(s, 1, 2) + ","
                Next
            End If
            If ViewFilter <> "" Then ViewFilter = Mid(ViewFilter, 1, Len(ViewFilter) - 1)

            strSql = " insert into " & cnAdminDb & "..Category"
            strSql += " ("
            strSql += " Catcode,Catname,Shortname,Displayorder,Metalid,Taxid,"
            strSql += " PurityID,Catmode,TranType,CatGroup,DiaStnType,Gs11,"
            strSql += " Gs12,Alloy,LedgerPrint,MinDiscPer,EstDisplay,"
            strSql += " BILLDisplay,AcctDisplay,SalesID,STAXID,SSCID,"
            strSql += " SASCID,SalesTax,SSC,SASC,SReturnID,PurchaseID,"
            strSql += " PTAXID,PSCID,PASCID,PTax,PSC,PASC,SPReturnID,OpnCode,"
            strSql += " ClsCode,UserId,Updated,Uptime,CGROUPID,"
            strSql += " Commodity,taxcategory,VIEWFILTER,"
            strSql += " PMCID,ACTIVE"
            If POS_ED_SEPPOST Then
                strSql += " ,EDSALESID,EDSTAXID"
                strSql += " ,EDPURCHASEID,EDPTAXID"
            End If
            'strSql += " ,S_SGSTID,S_SGSTTAX"
            'strSql += " ,S_CGSTID,S_CGSTTAX"
            'strSql += " ,S_IGSTID,S_IGSTTAX"
            'strSql += " ,P_SGSTID,P_SGSTTAX"
            'strSql += " ,P_CGSTID,P_CGSTTAX"
            'strSql += " ,P_IGSTID,P_IGSTTAX"
            strSql += " ) Values ("
            strSql += " '" & CatCode & "'"  ''Catcode
            strSql += " ,'" & txtName__Man.Text & "'" ''Catname
            strSql += " ,'" & txtShortName.Text & "'" ''Shortname
            strSql += " ,'" & txtDisplayOrder_Num.Text & "'" ''Displayorder
            strSql += " ,'" & metalId & "'" ''Metalid
            strSql += " ,'" & taxCode & "'" ''Taxid
            strSql += " ,'" & purityId & "'" ''PurityID
            strSql += " ,'" & Mid(cmbSalesPurchase.Text, 1, 1) & "'" ''Catmode
            strSql += " ,'" & Mid(cmbSmithTrans.Text, 1, 1) & "'" ''TranType
            strSql += " ,'" & funcGetCatGroup() & "'" ''CatGroup
            If pnlStone.Enabled = False Then
                strSql += " ,''" ''DiaStnType'''''''''''''''''''''''''''
            Else
                If rbtStone.Checked = True Then
                    strSql += " ,'T'" ''DiaStnType'''''''''''''''''''''''''''
                ElseIf rbtDiamond.Checked = True Then
                    strSql += " ,'D'" ''DiaStnType'''''''''''''''''''''''''''
                Else
                    strSql += " ,'P'" ''DiaStnType'''''''''''''''''''''''''''
                End If
            End If
            strSql += " ,'" & IIf(chkMetal.Checked = True, "Y", "N") & "'" ''Gs11
            strSql += " ,'" & IIf(chkOrnament.Checked = True, "Y", "N") & "'" ''Gs12
            strSql += " ,'" & Mid(cmbHaving.Text, 1, 1) & "'" ''Alloy
            strSql += " ,'" & Mid(cmbLedgerPrint.Text, 1, 1) & "'" ''LedgerPrint
            strSql += " ," & Val(txtDiscount_Per.Text) & "" ''MinDiscPer
            strSql += " ,'" & IIf(chkEstimation.Checked = True, "Y", "N") & "'"   ''EstDisplay
            strSql += " ,'" & IIf(chkBill.Checked = True, "Y", "N") & "'"   ''BILLDisplay
            strSql += " ,'" & IIf(chkAccount.Checked = True, "Y", "N") & "'"   ''AcctDisplay
            strSql += " ,'" & saId & "'" ''SalesID
            strSql += " ,'" & saTaxId & "'" ''STAXID
            strSql += " ,'" & saScId & "'" ''SSCID
            strSql += " ,'" & saSacId & "'" ''SASCID
            strSql += " ," & Val(txtSTax_Per.Text) & "" ''SalesTax
            strSql += " ," & Val(txtSSurcharge_Per.Text) & "" ''SSC
            strSql += " ," & Val(txtSASCharge_Per.Text) & "" ''SASC
            strSql += " ,'" & "C" + CatCode.Remove(0, 1) & "05'" ''SReturnID
            strSql += " ,'" & puId & "'" ''PurchaseID
            strSql += " ,'" & puTaxId & "'" ''PTAXID
            strSql += " ,'" & puScId & "'" ''PSCID
            strSql += " ,'" & puSacId & "'" ''PASCID
            strSql += " ," & Val(txtPTax_Per.Text) & "" ''PTax
            strSql += " ," & Val(txtPSurcharge_Per.Text) & "" ''PSC
            strSql += " ," & Val(txtPASCharge_Per.Text) & "" ''PASC
            strSql += " ,'" & "C" + CatCode.Remove(0, 1) & "10'" ''SPReturnID
            strSql += " ,'" & "C" + CatCode.Remove(0, 1) & "OP'" ''OpnCode
            strSql += " ,'" & "C" + CatCode.Remove(0, 1) & "CL'" ''ClsCode
            strSql += " ," & userId & "" ''UserId
            strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" ''Updated
            strSql += " ,'" & Date.Now.ToLongTimeString & "'" ''Uptime
            strSql += " ," & Val(objGPack.GetSqlValue("SELECT CGROUPID FROM " & cnAdminDb & "..CATEGORYGROUP WHERE CGROUPNAME = '" & cmbCategoryGroup.Text & "'", , , tran)) & "" 'CGROUPID
            strSql += " ,'" & txtCommCode.Text & "'" ''SalesTax
            strSql += " ,'" & txtTaxCat.Text & "'" ''SSC
            strSql += " ,'" & ViewFilter & "'" 'ViewFilter
            strSql += " ,'" & pMcId & "'" 'PUR MCHARGE
            strSql += " ,'" & Mid(cmbActive.Text, 1, 1) & "'" 'Active
            If POS_ED_SEPPOST Then
                strSql += " ,'" & saEDId & "'" ''EDSALESID
                strSql += " ,'" & saEDTaxId & "'" ''EDSTAXID
                strSql += " ,'" & puEDId & "'" ''EDPURCHASEID
                strSql += " ,'" & puEDTaxId & "'" ''EDPTAXID
            End If
            'strSql += " ,'" & saId_SGST & "'" ''S_CGST
            'strSql += " ," & Val(txtS_SGSTTax_Per.Text) & "" ''S_SGSTTAX
            'strSql += " ,'" & saId_CGST & "'" ''S_CGST
            'strSql += " ," & Val(txtS_CGSTTax_Per.Text) & "" ''S_CGSTTAX
            'strSql += " ,'" & saId_IGST & "'" ''S_CGST
            'strSql += " ," & Val(txtS_IGSTTax_Per.Text) & "" ''S_IGSTTAX
            'strSql += " ,'" & puId_SGST & "'" ''P_CGST
            'strSql += " ," & Val(txtP_SGSTTax_Per.Text) & "" ''P_SGSTTAX
            'strSql += " ,'" & puId_CGST & "'" ''P_CGST
            'strSql += " ," & Val(txtP_CGSTTax_Per.Text) & "" ''P_CGSTTAX
            'strSql += " ,'" & puId_IGST & "'" ''P_CGST
            'strSql += " ," & Val(txtP_IGSTTax_Per.Text) & "" ''P_IGSTTAX
            strSql += ")"
            ExecQuery(SyncMode.Master, strSql, cn, tran)

            'Inserting''''''''''Sales Account
            ''SalesId
            InsertIntoAcHead(saId, cmbSAC_OWN.Text, 3, "Y")
            ''SalesTaxId
            InsertIntoAcHead(saTaxId, cmbSTax_OWN.Text, 3)
            ''SSCId
            InsertIntoAcHead(saScId, cmbSSurcharge_OWN.Text, 3)
            ''SSACId
            InsertIntoAcHead(saSacId, cmbSASCharge_OWN.Text, 3)
            ''SalesRETURNId
            InsertIntoAcHead("C" + CatCode.Remove(0, 1) + "05", "SALES RETURN " + txtName__Man.Text, 3, "Y")
            'Inserting''''''''''Purchase Account
            ''PurchaseId
            InsertIntoAcHead(puId, cmbPAC_OWN.Text, 4, "Y")
            ''PurchaseTaxId
            InsertIntoAcHead(puTaxId, cmbPTax_OWN.Text, 4)
            ''PSCId
            InsertIntoAcHead(puScId, cmbPSurcharge_OWN.Text, 4)
            ''PSACId
            InsertIntoAcHead(puSacId, cmbPAC_OWN.Text, 4)
            ''Purchase RETURNId
            InsertIntoAcHead("C" + CatCode.Remove(0, 1) + "10", "PURCHASE RETURN " + txtName__Man.Text, 4)
            ''Open
            InsertIntoAcHead("C" + CatCode.Remove(0, 1) + "OP", "OPEN STOCK " + txtName__Man.Text, 3)
            ''Close
            InsertIntoAcHead("C" + CatCode.Remove(0, 1) + "CL", "CLOSE STOCK " + txtName__Man.Text, 3)

            ''SalesId_SGST
            '''''''''''''''''InsertIntoAcHead(saId_SGST, cmbS_SGST_OWN.Text, 3, "Y")
            ''SalesId_SGST
            '''''''''''''''''InsertIntoAcHead(saId_CGST, cmbS_CGST_OWN.Text, 3, "Y")
            ''SalesId_SGST
            '''''''''''''''''InsertIntoAcHead(saId_IGST, cmbS_IGST_OWN.Text, 3, "Y")

            ''PurchaseId_SGST
            '''''''''''''''''InsertIntoAcHead(puId_SGST, cmbP_SGST_OWN.Text, 3, "Y")
            ''PurchaseId_SGST
            '''''''''''''''''InsertIntoAcHead(puId_CGST, cmbP_CGST_OWN.Text, 3, "Y")
            ''PurchaseId_SGST
            '''''''''''''''''InsertIntoAcHead(puId_IGST, cmbP_IGST_OWN.Text, 3, "Y")

            InsertIntoBillControl("CAT-" + CatCode + "-SAL", txtName__Man.Text + " SALES BILLNO", "N", "N", "", "P", tran)
            InsertIntoBillControl("CAT-" + CatCode + "-PUR", txtName__Man.Text + " PURCHASE BILLNO", "N", "N", "", "P", tran)

            tran.Commit()
            funcNew()
        Catch ex As Exception
            tran.Rollback()
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        Finally
            tran.Dispose()
        End Try
        Return 0
    End Function

    Private Sub InsertIntoAcHead(ByVal AcCode As String, ByVal AcName As String, ByVal AcGrpCode As String, Optional ByVal Inventory As String = "")
        Dim str As String = Nothing
        If AcCode = Nothing Then Exit Sub
        If objGPack.DupCheck("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = '" & AcCode & "'", tran) = False Then
            str = " insert into " & cnAdminDb & "..AcHead("
            str += " AcCode,AcName,ACGrpCode,ACSubGrpCode,"
            str += " AcType,DoorNo,Address1,Address2,"
            str += " Address3,Area,City,Pincode,"
            str += " PhoneNo,Mobile,"
            str += " Emailid,"
            str += " WebSite,Ledprint,TdsFlag,TdsPer,"
            str += " Depflag,Depper,Outstanding,AutoGen,"
            str += " VATEX,LocalOutst,LocalTaxNo,CentralTaxNo,"
            str += " Userid,CrDate,CrTime,INVENTORY)values("
            str += " '" & AcCode & "','" & AcName & "','" & AcGrpCode & "','0',"
            str += " 'O','','','',"
            str += " '','','','',"
            str += " '','',"
            str += " '',"
            str += " '','','',0,"
            str += " '',0,'','',"
            str += " '','','','',"
            str += " " & userId & ",'" & Today.Date.ToString("yyyy-MM-dd") & "','" & Date.Now.ToLongTimeString & "','" & Inventory & "')"
        Else
            str = " UPDATE " & cnAdminDb & "..ACHEAD"
            str += " SET ACNAME = '" & AcName & "'"
            str += " WHERE ACCODE = '" & AcCode & "'"
        End If
        ExecQuery(SyncMode.Master, str, cn, tran)
    End Sub

    Function funcAcNameLengthValidation() As Boolean
        ''Sales Account
        If cmbSAC_OWN.Text.Length > 55 Then
            MsgBox("Sales A/C Name Length Overflow" + vbCrLf + "Length with in 55 Characters", MsgBoxStyle.Information)
            cmbSAC_OWN.Focus()
            Return True
        End If
        If cmbSTax_OWN.Text.Length > 55 Then
            MsgBox("Sales Tax Name Length Overflow" + vbCrLf + "Length with in 55 Characters", MsgBoxStyle.Information)
            cmbSTax_OWN.Focus()
            Return True
        End If
        If cmbSSurcharge_OWN.Text.Length > 55 Then
            MsgBox("Sales Surcharge Name Length Overflow" + vbCrLf + "Length with in 55 Characters", MsgBoxStyle.Information)
            cmbSSurcharge_OWN.Focus()
            Return True
        End If
        If cmbSASCharge_OWN.Text.Length > 55 Then
            MsgBox("Sales Adl-Surcharge Name Length Overflow" + vbCrLf + "Length with in 55 Characters", MsgBoxStyle.Information)
            cmbSASCharge_OWN.Focus()
            Return True
        End If

        ''Purchase Account
        If cmbPAC_OWN.Text.Length > 55 Then
            MsgBox("Purchase A/C Name Length Overflow" + vbCrLf + "Length with in 55 Characters", MsgBoxStyle.Information)
            cmbPAC_OWN.Focus()
            Return True
        End If
        If cmbPTax_OWN.Text.Length > 55 Then
            MsgBox("Purchase Tax Name Length Overflow" + vbCrLf + "Length with in 55 Characters", MsgBoxStyle.Information)
            cmbPTax_OWN.Focus()
            Return True
        End If
        If cmbPSurcharge_OWN.Text.Length > 55 Then
            MsgBox("Purchase Surcharge Name Length Overflow" + vbCrLf + "Length with in 55 Characters", MsgBoxStyle.Information)
            cmbPSurcharge_OWN.Focus()
            Return True
        End If
        If cmbPASCharge_OWN.Text.Length > 55 Then
            MsgBox("Purchase Adl-Surcharge Name Length Overflow" + vbCrLf + "Length with in 55 Characters", MsgBoxStyle.Information)
            cmbPASCharge_OWN.Focus()
            Return True
        End If
        Return False
    End Function
    Function funcUpdate() As Integer
        Dim ds As New Data.DataSet
        ds.Clear()
        Dim metalId As String = Nothing
        Dim purityId As String = Nothing
        Dim taxCode As String = Nothing

        Dim SalesID As String = Nothing
        Dim STAXID As String = Nothing
        Dim SSCID As String = Nothing
        Dim SASCID As String = Nothing

        Dim PurchaseID As String = Nothing
        Dim PTAXID As String = Nothing
        Dim PSCID As String = Nothing
        Dim PASCID As String = Nothing

        Dim saId_SGST As String = Nothing
        Dim saId_CGST As String = Nothing
        Dim saId_IGST As String = Nothing
        Dim puId_SGST As String = Nothing
        Dim puId_CGST As String = Nothing
        Dim puId_IGST As String = Nothing

        Try
            tran = cn.BeginTransaction
            ''Find MetalId
            strSql = " SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE"
            strSql += vbCrLf + "  METALNAME = '" & cmbMetal.Text & "'"
            metalId = objGPack.GetSqlValue(strSql, , , tran)
            ''Find PurityId
            strSql = " SELECT PURITYID FROM " & cnAdminDb & "..PURITYMAST WHERE"
            strSql += vbCrLf + "  PURITYNAME = '" & cmbPurity.Text & "'"
            purityId = objGPack.GetSqlValue(strSql, , , tran)
            ''Find TaxCode
            strSql = " SELECT TAXCODE FROM " & cnAdminDb & "..TAXMAST WHERE"
            strSql += vbCrLf + "  TAXNAME = '" & cmbTaxMode.Text & "'"
            taxCode = objGPack.GetSqlValue(strSql, , , tran)
            ''Find SalesId
            strSql = " SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD"
            strSql += vbCrLf + "  WHERE ACNAME = '" & cmbSAC_OWN.Text & "'"
            SalesID = objGPack.GetSqlValue(strSql, , , tran)
            ''Find STaxId
            strSql = " SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD"
            strSql += vbCrLf + "  WHERE ACNAME = '" & cmbSTax_OWN.Text & "'"
            STAXID = objGPack.GetSqlValue(strSql, , , tran)
            ''Find SSCId
            strSql = " SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD"
            strSql += vbCrLf + "  WHERE ACNAME = '" & cmbSSurcharge_OWN.Text & "'"
            SSCID = objGPack.GetSqlValue(strSql, , Nothing, tran)
            If SSCID = Nothing And cmbSSurcharge_OWN.Text <> "" Then
                SSCID = "C" + CatCode.Remove(0, 1) & "03"
                InsertIntoAcHead(SSCID, cmbSSurcharge_OWN.Text, 3)
            End If
            ''Find SASCId
            strSql = " SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD"
            strSql += vbCrLf + "  WHERE ACNAME = '" & cmbSASCharge_OWN.Text & "'"
            SASCID = objGPack.GetSqlValue(strSql, , Nothing, tran)
            If SASCID = Nothing And cmbSASCharge_OWN.Text <> "" Then
                SASCID = "C" + CatCode.Remove(0, 1) & "04"
                InsertIntoAcHead(SASCID, cmbSASCharge_OWN.Text, 3)
            End If

            ''Find PurchaseId
            strSql = " SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD"
            strSql += vbCrLf + "  WHERE ACNAME = '" & cmbPAC_OWN.Text & "'"
            PurchaseID = objGPack.GetSqlValue(strSql, , , tran)
            ''Find PTaxId
            strSql = " SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD"
            strSql += vbCrLf + "  WHERE ACNAME = '" & cmbPTax_OWN.Text & "'"
            PTAXID = objGPack.GetSqlValue(strSql, , , tran)
            ''Find PSCId
            strSql = " SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD"
            strSql += vbCrLf + "  WHERE ACNAME = '" & cmbPSurcharge_OWN.Text & "'"
            PSCID = objGPack.GetSqlValue(strSql, , Nothing, tran)
            If PSCID = Nothing And cmbPSurcharge_OWN.Text <> "" Then
                PSCID = "C" + CatCode.Remove(0, 1) & "08"
                InsertIntoAcHead(PSCID, cmbPSurcharge_OWN.Text, 4)
            End If
            ''Find PASCId
            strSql = " SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD"
            strSql += vbCrLf + "  WHERE ACNAME = '" & cmbPASCharge_OWN.Text & "'"
            PASCID = objGPack.GetSqlValue(strSql, , Nothing, tran)
            If PASCID = Nothing And cmbPASCharge_OWN.Text <> "" Then
                PASCID = "C" + CatCode.Remove(0, 1) & "09"
                InsertIntoAcHead(PASCID, cmbPSurcharge_OWN.Text, 4)
            End If
            strSql = "SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbPMC_OWN.Text & "'"
            Dim pMcId As String = objGPack.GetSqlValue(strSql, "ACCODE", "GMAKP", tran)

            Dim ViewFilter As String = ""
            If ChkCmbViewFilter.Visible = True Then
                For Each s As String In ChkCmbViewFilter.Text.Split(",")
                    ViewFilter += Mid(s, 1, 2) + ","
                Next
            End If
            If ViewFilter <> "" Then ViewFilter = Mid(ViewFilter, 1, Len(ViewFilter) - 1)

            Dim EDSALESID As String = Nothing
            Dim EDSTAXID As String = Nothing
            Dim EDPURCHASEID As String = Nothing
            Dim EDPTAXID As String = Nothing
            ''Find EDSalesId
            strSql = " SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD"
            strSql += vbCrLf + "  WHERE ACNAME = '" & cmbESAC_OWN.Text & "'"
            EDSalesID = objGPack.GetSqlValue(strSql, , , tran)
            ''Find EDSTaxId
            strSql = " SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD"
            strSql += vbCrLf + "  WHERE ACNAME = '" & cmbESTax_OWN.Text & "'"
            EDSTAXID = objGPack.GetSqlValue(strSql, , , tran)
            ''Find EDPurchaseId
            strSql = " SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD"
            strSql += vbCrLf + "  WHERE ACNAME = '" & cmbEPAC_OWN.Text & "'"
            EDPurchaseID = objGPack.GetSqlValue(strSql, , , tran)
            ''Find EDPTaxId
            strSql = " SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD"
            strSql += vbCrLf + "  WHERE ACNAME = '" & cmbEPTax_OWN.Text & "'"
            EDPTAXID = objGPack.GetSqlValue(strSql, , , tran)


            ' ''Find saId_SGST
            'strSql = " SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD"
            'strSql += vbCrLf + "  WHERE ACNAME = '" & cmbS_SGST_OWN.Text & "'"
            'saId_SGST = objGPack.GetSqlValue(strSql, , Nothing, tran)
            'If saId_SGST = Nothing And cmbS_SGST_OWN.Text <> "" Then
            '    saId_SGST = "C" + CatCode.Remove(0, 1) & "11"
            '    InsertIntoAcHead(saId_SGST, cmbS_SGST_OWN.Text, 3)
            'End If
            ' ''Find SALES GSTId
            'strSql = " SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD"
            'strSql += vbCrLf + "  WHERE ACNAME = '" & cmbS_SGST_OWN.Text & "'"
            'saId_SGST = objGPack.GetSqlValue(strSql, , , tran)

            ' ''Find saId_cGST
            'strSql = " SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD"
            'strSql += vbCrLf + "  WHERE ACNAME = '" & cmbS_cGST_OWN.Text & "'"
            'saId_cGST = objGPack.GetSqlValue(strSql, , Nothing, tran)
            'If saId_cGST = Nothing And cmbS_cGST_OWN.Text <> "" Then
            '    saId_CGST = "C" + CatCode.Remove(0, 1) & "12"
            '    InsertIntoAcHead(saId_CGST, cmbS_CGST_OWN.Text, 3)
            'End If
            'strSql = " SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD"
            'strSql += vbCrLf + "  WHERE ACNAME = '" & cmbS_CGST_OWN.Text & "'"
            'saId_CGST = objGPack.GetSqlValue(strSql, , , tran)

            ' ''Find saId_IGST
            'strSql = " SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD"
            'strSql += vbCrLf + "  WHERE ACNAME = '" & cmbS_IGST_OWN.Text & "'"
            'saId_IGST = objGPack.GetSqlValue(strSql, , Nothing, tran)
            'If saId_IGST = Nothing And cmbS_IGST_OWN.Text <> "" Then
            '    saId_IGST = "C" + CatCode.Remove(0, 1) & "13"
            '    InsertIntoAcHead(saId_IGST, cmbS_IGST_OWN.Text, 3)
            'End If
            'strSql = " SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD"
            'strSql += vbCrLf + "  WHERE ACNAME = '" & cmbS_IGST_OWN.Text & "'"
            'saId_IGST = objGPack.GetSqlValue(strSql, , , tran)

            ' ''Find PURCHASE GSTId
            ' ''Find puId_SGST
            'strSql = " SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD"
            'strSql += vbCrLf + "  WHERE ACNAME = '" & cmbP_SGST_OWN.Text & "'"
            'puId_SGST = objGPack.GetSqlValue(strSql, , Nothing, tran)
            'If puId_SGST = Nothing And cmbP_SGST_OWN.Text <> "" Then
            '    puId_SGST = "C" + CatCode.Remove(0, 1) & "14"
            '    InsertIntoAcHead(puId_SGST, cmbP_SGST_OWN.Text, 3)
            'End If
            'strSql = " SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD"
            'strSql += vbCrLf + "  WHERE ACNAME = '" & cmbP_SGST_OWN.Text & "'"
            'puId_SGST = objGPack.GetSqlValue(strSql, , , tran)
            ' ''Find puId_CGST
            'strSql = " SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD"
            'strSql += vbCrLf + "  WHERE ACNAME = '" & cmbP_CGST_OWN.Text & "'"
            'puId_CGST = objGPack.GetSqlValue(strSql, , Nothing, tran)
            'If puId_CGST = Nothing And cmbP_CGST_OWN.Text <> "" Then
            '    puId_CGST = "C" + CatCode.Remove(0, 1) & "15"
            '    InsertIntoAcHead(puId_CGST, cmbP_CGST_OWN.Text, 3)
            'End If
            'strSql = " SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD"
            'strSql += vbCrLf + "  WHERE ACNAME = '" & cmbP_CGST_OWN.Text & "'"
            'puId_CGST = objGPack.GetSqlValue(strSql, , , tran)
            ' ''Find puId_IGST
            'strSql = " SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD"
            'strSql += vbCrLf + "  WHERE ACNAME = '" & cmbP_iGST_OWN.Text & "'"
            'puId_iGST = objGPack.GetSqlValue(strSql, , Nothing, tran)
            'If puId_iGST = Nothing And cmbP_iGST_OWN.Text <> "" Then
            '    puId_IGST = "C" + CatCode.Remove(0, 1) & "16"
            '    InsertIntoAcHead(puId_IGST, cmbP_IGST_OWN.Text, 3)
            'End If
            'strSql = " SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD"
            'strSql += vbCrLf + "  WHERE ACNAME = '" & cmbP_IGST_OWN.Text & "'"
            'puId_IGST = objGPack.GetSqlValue(strSql, , , tran)


            strSql = " Update " & cnAdminDb & "..Category Set"
            strSql += vbCrLf + "  Catname = '" & txtName__Man.Text & "'"
            strSql += vbCrLf + "  ,Shortname = '" & txtShortName.Text & "'"
            strSql += vbCrLf + "  ,Displayorder = '" & txtDisplayOrder_Num.Text & "'"
            strSql += vbCrLf + "  ,Metalid = '" & metalId & "'"
            strSql += vbCrLf + "  ,Taxid = '" & taxCode & "'"
            strSql += vbCrLf + "  ,PurityID = '" & purityId & "'"
            strSql += vbCrLf + "  ,Catmode = '" & Mid(cmbSalesPurchase.Text, 1, 1) & "'"
            strSql += vbCrLf + "  ,TranType = '" & Mid(cmbSmithTrans.Text, 1, 1) & "'"
            strSql += vbCrLf + "  ,CatGroup = '" & funcGetCatGroup() & "'"
            strSql += vbCrLf + "  ,DiaStnType = ''"
            strSql += vbCrLf + "  ,Gs11 = '" & IIf(chkMetal.Checked = True, "Y", "N") & "'"
            strSql += vbCrLf + "  ,Gs12 = '" & IIf(chkOrnament.Checked = True, "Y", "N") & "'"
            strSql += vbCrLf + "  ,Alloy = '" & Mid(cmbHaving.Text, 1, 1) & "'"
            strSql += vbCrLf + "  ,LedgerPrint = '" & Mid(cmbLedgerPrint.Text, 1, 1) & "'"
            strSql += vbCrLf + "  ,MinDiscPer = '" & Val(txtDiscount_Per.Text) & "' "
            strSql += vbCrLf + "  ,EstDisplay = '" & IIf(chkEstimation.Checked = True, "Y", "N") & "'"
            strSql += vbCrLf + "  ,BILLDisplay = '" & IIf(chkBill.Checked = True, "Y", "N") & "'"
            strSql += vbCrLf + "  ,AcctDisplay = '" & IIf(chkAccount.Checked = True, "Y", "N") & "'"
            strSql += vbCrLf + "  ,SalesID = '" & SalesID & "'"
            strSql += vbCrLf + "  ,STAXID = '" & STAXID & "'"
            strSql += vbCrLf + "  ,SSCID = '" & SSCID & "'"
            strSql += vbCrLf + "  ,SASCID = '" & SASCID & "'"
            strSql += vbCrLf + "  ,SalesTax = " & Val(txtSTax_Per.Text) & ""
            strSql += vbCrLf + "  ,SSC = " & Val(txtSSurcharge_Per.Text) & ""
            strSql += vbCrLf + "  ,SASC = " & Val(txtSASCharge_Per.Text) & ""
            'strSql +=vbCrLf + "  ,SReturnID"
            strSql += vbCrLf + "  ,PurchaseID = '" & PurchaseID & "'"
            strSql += vbCrLf + "  ,PTAXID = '" & PTAXID & "'"
            strSql += vbCrLf + "  ,PSCID = '" & PSCID & "'"
            strSql += vbCrLf + "  ,PASCID = '" & PASCID & "'"
            strSql += vbCrLf + "  ,PTax = " & Val(txtPTax_Per.Text) & ""
            strSql += vbCrLf + "  ,PSC = " & Val(txtPSurcharge_Per.Text) & ""
            strSql += vbCrLf + "  ,PASC = " & Val(txtPASCharge_Per.Text) & ""
            'strSql +=vbCrLf + "  ,SPReturnID"
            'strSql +=vbCrLf + "  ,OpnCode"
            'strSql +=vbCrLf + "  ,ClsCode"
            strSql += vbCrLf + "  ,COMMODITY = '" & txtCommCode.Text & "'"
            strSql += vbCrLf + "  ,TAXCATEGORY = '" & txtTaxCat.Text & "'"
            strSql += vbCrLf + "  ,UserId = '" & userId & "'"
            strSql += vbCrLf + "  ,Updated = '" & Today.Date.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + "  ,Uptime = '" & Date.Now.ToLongTimeString & "'"
            strSql += vbCrLf + "  ,CGROUPID = " & Val(objGPack.GetSqlValue("SELECT CGROUPID FROM " & cnAdminDb & "..CATEGORYGROUP WHERE CGROUPNAME = '" & cmbCategoryGroup.Text & "'", , , tran)) & ""
            strSql += vbCrLf + "  ,VIEWFILTER = '" & ViewFilter & "'"
            strSql += vbCrLf + "  ,PMCID = '" & pMcId & "'"
            strSql += vbCrLf + "  ,ACTIVE= '" & Mid(cmbActive.Text, 1, 1) & "'" 'Active
            If POS_ED_SEPPOST Then
                strSql += vbCrLf + "  ,EDSALESID = '" & EDSALESID & "'"
                strSql += vbCrLf + "  ,EDSTAXID = '" & EDSTAXID & "'"
                strSql += vbCrLf + "  ,EDPURCHASEID = '" & EDPURCHASEID & "'"
                strSql += vbCrLf + "  ,EDPTAXID = '" & EDPTAXID & "'"
            End If
            'strSql += vbCrLf + "  ,S_SGSTID = '" & saId_SGST & "'"
            'strSql += vbCrLf + "  ,S_SGSTTAX = '" & Val(txtS_SGSTTax_Per.Text) & "'"
            'strSql += vbCrLf + "  ,S_CGSTID = '" & saId_CGST & "'"
            'strSql += vbCrLf + "  ,S_CGSTTAX = '" & Val(txtS_CGSTTax_Per.Text) & "'"
            'strSql += vbCrLf + "  ,S_IGSTID = '" & saId_IGST & "'"
            'strSql += vbCrLf + "  ,S_IGSTTAX = '" & Val(txtS_IGSTTax_Per.Text) & "'"
            'strSql += vbCrLf + "  ,P_SGSTID = '" & puId_SGST & "'"
            'strSql += vbCrLf + "  ,P_SGSTTAX = '" & Val(txtP_SGSTTax_Per.Text) & "'"
            'strSql += vbCrLf + "  ,P_CGSTID = '" & puId_CGST & "'"
            'strSql += vbCrLf + "  ,P_CGSTTAX = '" & Val(txtP_CGSTTax_Per.Text) & "'"
            'strSql += vbCrLf + "  ,P_IGSTID = '" & puId_IGST & "'"
            'strSql += vbCrLf + "  ,P_IGSTTAX = '" & Val(txtP_IGSTTax_Per.Text) & "'"
            strSql += vbCrLf + "  where CatCode = '" & CatCode & "'"
            ExecQuery(SyncMode.Master, strSql, cn, tran)
            tran.Commit()
            funcNew()
        Catch ex As Exception
            If tran IsNot Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        Finally
            tran.Dispose()
        End Try
    End Function
    Function funcGetDetails(ByVal tempCatCode As String)
        Dim dt As New DataTable
        dt.Clear()
        flagSave = True
        strSql = " SELECT"
        strSql += vbCrLf + "  CATCODE,CATNAME,SHORTNAME,DISPLAYORDER,"
        strSql += vbCrLf + "  (SELECT METALNAME FROM " & cnAdminDb & "..METALMAST AS M WHERE M.METALID = C.METALID)AS METALNAME,"
        strSql += vbCrLf + "  (SELECT TAXNAME FROM " & cnAdminDb & "..TAXMAST AS T WHERE T.TAXCODE = C.TAXID)AS TAXNAME,"
        strSql += vbCrLf + "  ISNULL((SELECT PURITYNAME FROM " & cnAdminDb & "..PURITYMAST AS P WHERE P.PURITYID = C.PURITYID),'')AS PURITYNAME,"
        strSql += vbCrLf + "  CASE WHEN CATMODE = 'S' THEN 'SALES'"
        strSql += vbCrLf + "  WHEN CATMODE = 'P'  THEN 'PURCHASE' ELSE 'BOTH' END AS CATMODE,"
        strSql += vbCrLf + "  CASE WHEN TRANTYPE = 'I' THEN 'ISSUE'"
        strSql += vbCrLf + "  WHEN TRANTYPE = 'R' THEN 'RECEIPT' ELSE 'BOTH' END AS TRANTYPE,"
        strSql += vbCrLf + "  CASE WHEN CATGROUP = 'B' THEN 'BAR'"
        strSql += vbCrLf + "  WHEN CATGROUP = 'L' THEN 'OLD' "
        strSql += vbCrLf + "  WHEN CATGROUP = 'R' THEN 'REPAIR'"
        strSql += vbCrLf + "  WHEN CATGROUP = 'O' THEN 'ORDER'"
        strSql += vbCrLf + "  WHEN CATGROUP = 'P' THEN 'ORNAMENTS' ELSE 'PARTY METAL' END AS CATGROUP,"
        strSql += vbCrLf + "  DIASTNTYPE,"
        strSql += vbCrLf + "  CASE WHEN GS11 = 'Y' THEN 'YES' ELSE 'NO' END AS GS11,"
        strSql += vbCrLf + "  CASE WHEN GS12 = 'Y' THEN 'YES' ELSE 'NO' END AS GS12,"
        strSql += vbCrLf + "  CASE WHEN ALLOY = 'W' THEN 'WEIGHT'"
        strSql += vbCrLf + "  WHEN ALLOY = 'A' THEN 'ALLOY' ELSE '' END ALLOY,"
        strSql += vbCrLf + "  CASE WHEN LEDGERPRINT = 'Y' THEN 'YES' ELSE 'NO' END AS LEDGERPRINT,"
        strSql += vbCrLf + "  MINDISCPER,"
        strSql += vbCrLf + "  CASE WHEN ESTDISPLAY = 'Y' THEN 'YES' ELSE 'NO' END AS ESTDISPLAY,"
        strSql += vbCrLf + "  CASE WHEN BILLDISPLAY = 'Y' THEN 'YES' ELSE 'NO' END AS BILLDISPLAY,"
        strSql += vbCrLf + "  CASE WHEN ACCTDISPLAY = 'Y' THEN 'YES' ELSE 'NO' END AS ACCTDISPLAY,"
        strSql += vbCrLf + "  (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = C.SALESID)AS SALESIDNAME,"
        strSql += vbCrLf + "  (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = C.STAXID)AS STAXIDNAME,"
        strSql += vbCrLf + "  (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = C.SSCID)AS SSCIDNAME,"
        strSql += vbCrLf + "  (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = C.SASCID)AS SASCIDNAME,"
        strSql += vbCrLf + "  SALESTAX,SSC,SASC,SRETURNID,"
        strSql += vbCrLf + "  (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = C.PURCHASEID)AS PURCHASEIDNAME,"
        strSql += vbCrLf + "  (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = C.PTAXID)AS PTAXIDNAME,"
        strSql += vbCrLf + "  (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = C.PSCID)AS PSCIDNAME,"
        strSql += vbCrLf + "  (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = C.PASCID)AS PASCIDNAME,"
        strSql += vbCrLf + "  PTAX,PSC,PASC,SPRETURNID,"
        strSql += vbCrLf + "  OPNCODE,CLSCODE"
        strSql += vbCrLf + "  ,ISNULL((SELECT CGROUPNAME FROM " & cnAdminDb & "..CATEGORYGROUP WHERE CGROUPID = C.CGROUPID),'[NONE]') AS CGROUPNAME"
        strSql += vbCrLf + "  ,VIEWFILTER"
        strSql += vbCrLf + "  ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = C.PMCID)AS PMCID"
        strSql += vbCrLf + "  ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = C.EDSALESID)AS EDSALESID"
        strSql += vbCrLf + "  ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = C.EDSTAXID)AS EDSTAXID"
        strSql += vbCrLf + "  ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = C.EDPURCHASEID)AS EDPURCHASEID"
        strSql += vbCrLf + "  ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = C.EDPTAXID)AS EDPTAXID"

        strSql += vbCrLf + "  ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = C.S_SGSTID)AS SALES_SGST"
        strSql += vbCrLf + "  ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = C.S_CGSTID)AS SALES_CGST"
        strSql += vbCrLf + "  ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = C.S_IGSTID)AS SALES_IGST"
        strSql += vbCrLf + "  ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = C.P_SGSTID)AS PURCHASE_SGST"
        strSql += vbCrLf + "  ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = C.P_CGSTID)AS PURCHASE_CGST"
        strSql += vbCrLf + "  ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = C.P_IGSTID)AS PURCHASE_IGST"
        strSql += vbCrLf + "  ,S_SGSTTAX, S_CGSTTAX, S_IGSTTAX, P_SGSTTAX, P_CGSTTAX, P_IGSTTAX"
        strSql += vbCrLf + "  FROM " & cnAdminDb & "..CATEGORY AS C WHERE CATCODE = '" & tempCatCode & "'"
        da = New OleDbDataAdapter(strSql, cn)   
        da.Fill(dt)     
        If Not dt.Rows.Count > 0 Then
            Return 0
        End If
        With dt.Rows(0)
            strSql = " SELECT METALTYPE FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYNAME = '" & .Item("PURITYNAME").ToString & "'"
            Select Case objGPack.GetSqlValue(strSql).ToUpper
                Case "M"
                    rbtMetal.Checked = True
                Case "O"
                    rbtOrnament.Checked = True
                Case Else
                    rbtStoneDiamond.Checked = True
            End Select
            funcLoadMetal()
            'funcLoadTaxMode()
            CatCode = .Item("Catcode").ToString
            txtName__Man.Text = .Item("Catname").ToString
            txtShortName.Text = .Item("Shortname").ToString
            txtDisplayOrder_Num.Text = .Item("Displayorder").ToString
            cmbMetal.Text = .Item("MetalName").ToString
            cmbTaxMode.Text = .Item("TaxName").ToString
            cmbPurity.Text = .Item("PurityName").ToString
            cmbSalesPurchase.Text = .Item("Catmode").ToString
            cmbSmithTrans.Text = .Item("TranType").ToString
            cmbStockGroup.Text = .Item("CatGroup").ToString
            If .Item("DiaStnType").ToString <> "" Then
                pnlStone.Enabled = True
                If .Item("DiaStnType").ToString = "D" Then
                    rbtDiamond.Checked = True
                ElseIf .Item("DiaStnType").ToString = "T" Then
                    rbtStone.Checked = True
                Else
                    rbtPrecious.Checked = True
                End If
            Else
                pnlStone.Enabled = False
            End If
            If .Item("Gs11").ToString = "YES" Then
                chkMetal.Checked = True
            Else
                chkMetal.Checked = False
            End If
            If .Item("Gs12").ToString = "YES" Then
                chkOrnament.Checked = True
            Else
                chkOrnament.Checked = False
            End If
            cmbHaving.Text = .Item("Alloy").ToString
            cmbLedgerPrint.Text = .Item("LedgerPrint").ToString
            '=.item("MinDiscPer")
            If .Item("EstDisplay").ToString = "YES" Then
                chkEstimation.Checked = True
            Else
                chkEstimation.Checked = False
            End If
            If .Item("BILLDisplay").ToString = "YES" Then
                chkBill.Checked = True
            Else
                chkBill.Checked = False
            End If
            If .Item("AcctDisplay").ToString = "YES" Then
                chkAccount.Checked = True
            Else
                chkAccount.Checked = False
            End If
            funcLoadAcHeadNames()
            cmbSAC_OWN.Text = .Item("SalesIDName").ToString
            cmbSTax_OWN.Text = .Item("STAXIDName").ToString
            cmbSSurcharge_OWN.Text = .Item("SSCIDName").ToString
            cmbSASCharge_OWN.Text = .Item("SASCIDName").ToString
            txtSTax_Per.Text = .Item("SalesTax").ToString
            txtSSurcharge_Per.Text = .Item("SSC").ToString
            txtSASCharge_Per.Text = .Item("SASC").ToString
            '=.item("SReturnID")
            cmbPAC_OWN.Text = .Item("PurchaseIDName").ToString
            cmbPTax_OWN.Text = .Item("PTAXIDName").ToString
            cmbPSurcharge_OWN.Text = .Item("PSCIDName").ToString
            cmbPASCharge_OWN.Text = .Item("PASCIDName").ToString
            txtPTax_Per.Text = .Item("PTax").ToString
            txtPSurcharge_Per.Text = .Item("PSC").ToString
            txtPASCharge_Per.Text = .Item("PASC").ToString
            cmbCategoryGroup.Text = .Item("CGROUPNAME").ToString
            cmbPMC_OWN.Text = .Item("PMCID").ToString
            cmbESAC_OWN.Text = .Item("EDSALESID").ToString
            cmbESTax_OWN.Text = .Item("EDSTAXID").ToString
            cmbEPAC_OWN.Text = .Item("EDPURCHASEID").ToString
            cmbEPTax_OWN.Text = .Item("EDPTAXID").ToString
            cmbS_SGST_OWN.Text = .Item("SALES_SGST").ToString
            cmbS_CGST_OWN.Text = .Item("SALES_CGST").ToString
            cmbS_IGST_OWN.Text = .Item("SALES_IGST").ToString
            cmbP_SGST_OWN.Text = .Item("PURCHASE_SGST").ToString
            cmbP_CGST_OWN.Text = .Item("PURCHASE_CGST").ToString
            cmbP_IGST_OWN.Text = .Item("PURCHASE_IGST").ToString
            txtS_SGSTTax_Per.Text = .Item("S_SGSTTAX").ToString
            txtS_CGSTTax_Per.Text = .Item("S_CGSTTAX").ToString
            txtS_IGSTTax_Per.Text = .Item("S_IGSTTAX").ToString
            txtP_SGSTTax_Per.Text = .Item("P_SGSTTAX").ToString
            txtP_CGSTTax_Per.Text = .Item("P_CGSTTAX").ToString
            txtP_IGSTTax_Per.Text = .Item("P_IGSTTAX").ToString

            If .Item("VIEWFILTER").ToString() <> "" Then
                For Each s As String In .Item("VIEWFILTER").ToString().Split(",")
                    For cnt As Integer = 0 To ChkCmbViewFilter.Items.Count - 1
                        If ChkCmbViewFilter.Items(cnt).ToString.Contains(s) Then
                            ChkCmbViewFilter.SetItemChecked(cnt, True)
                        End If
                    Next
                Next
            Else
                For cnt As Integer = 0 To ChkCmbViewFilter.Items.Count - 1
                    ChkCmbViewFilter.SetItemChecked(cnt, False)
                Next
                ChkCmbViewFilter.Text = ""
            End If
        End With
        Return 0
    End Function
    Function funcLoadAcHeadNames()
        Dim dt As New DataTable
        dt.Clear()
        ''Sales Account
        cmbSAC_OWN.Items.Clear()
        cmbSTax_OWN.Items.Clear()
        cmbSSurcharge_OWN.Items.Clear()
        cmbSASCharge_OWN.Items.Clear()
        cmbESAC_OWN.Items.Clear()
        cmbESTax_OWN.Items.Clear()
        ''Purchase Account
        cmbPAC_OWN.Items.Clear()
        cmbPTax_OWN.Items.Clear()
        cmbPSurcharge_OWN.Items.Clear()
        cmbPASCharge_OWN.Items.Clear()
        cmbPMC_OWN.Items.Clear()
        cmbEPAC_OWN.Items.Clear()
        cmbEPTax_OWN.Items.Clear()
        strSql = " select AcName from " & cnAdminDb & "..AcHead "
        strSql += " where actype='O'"
        'strSql += " where AcGrpCode = 3 "
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            objGPack.FillCombonew(dt, cmbSAC_OWN, , False)
            objGPack.FillCombonew(dt, cmbSTax_OWN, , False)
            objGPack.FillCombonew(dt, cmbSSurcharge_OWN, , False)
            objGPack.FillCombonew(dt, cmbSASCharge_OWN, , False)
            objGPack.FillCombonew(dt, cmbESAC_OWN, , False)
            objGPack.FillCombonew(dt, cmbESTax_OWN, , False)

            objGPack.FillCombonew(dt, cmbS_CGST_OWN, , False)
            objGPack.FillCombonew(dt, cmbS_SGST_OWN, , False)
            objGPack.FillCombonew(dt, cmbS_IGST_OWN, , False)

            'Dim cnt As Integer
            'For cnt = 0 To dt.Rows.Count - 1
            '    With dt.Rows(cnt)
            '        cmbSAC_OWN.Items.Add(.Item("AcName").ToString)
            '        cmbSTax_OWN.Items.Add(.Item("AcName").ToString)
            '        cmbSSurcharge_OWN.Items.Add(.Item("AcName").ToString)
            '        cmbSASCharge_OWN.Items.Add(.Item("AcName").ToString)
            '        cmbESAC_OWN.Items.Add(.Item("AcName").ToString)
            '        cmbESTax_OWN.Items.Add(.Item("AcName").ToString)
            '    End With
            'Next
        End If

        strSql = " select AcName from " & cnAdminDb & "..AcHead "
        'strSql += " where AcGrpCode = 4 "
        strSql += " where actype='O'"
        dt = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            objGPack.FillCombonew(dt, cmbPAC_OWN, , False)
            objGPack.FillCombonew(dt, cmbPTax_OWN, , False)
            objGPack.FillCombonew(dt, cmbPSurcharge_OWN, , False)
            objGPack.FillCombonew(dt, cmbPASCharge_OWN, , False)
            objGPack.FillCombonew(dt, cmbEPAC_OWN, , False)
            objGPack.FillCombonew(dt, cmbEPTax_OWN, , False)

            objGPack.FillCombonew(dt, cmbP_CGST_OWN, , False)
            objGPack.FillCombonew(dt, cmbP_SGST_OWN, , False)
            objGPack.FillCombonew(dt, cmbP_IGST_OWN, , False)
            'Dim cnt As Integer
            'For cnt = 0 To dt.Rows.Count - 1
            '    With dt.Rows(cnt)
            '        cmbPAC_OWN.Items.Add(.Item("AcName").ToString)
            '        cmbPTax_OWN.Items.Add(.Item("AcName").ToString)
            '        cmbPSurcharge_OWN.Items.Add(.Item("AcName").ToString)
            '        cmbPASCharge_OWN.Items.Add(.Item("AcName").ToString)
            '        cmbEPAC_OWN.Items.Add(.Item("AcName").ToString)
            '        cmbEPTax_OWN.Items.Add(.Item("AcName").ToString)
            '    End With
            'Next
        End If
        strSql = " select AcName from " & cnAdminDb & "..AcHead where accode = 'GMAKP' "
        dt = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            objGPack.FillCombonew(dt, cmbPMC_OWN, , False)
            'Dim cnt As Integer
            'For cnt = 0 To dt.Rows.Count - 1
            '    With dt.Rows(cnt)
            '        cmbPMC_OWN.Items.Add(.Item("AcName").ToString)
            '    End With
            'Next
        End If
        Return 0
    End Function
    Function funcLoadMetal() As Integer
        Dim dt As New DataTable
        dt.Clear()
        cmbMetal.Items.Clear()
        If rbtStoneDiamond.Checked = False Then
            strSql = " Select MetalName from " & cnAdminDb & "..MetalMast where ISNULL(ACTIVE,'') <> 'N' ORDER BY DISPLAYORDER"
        Else
            strSql = " Select MetalName from " & cnAdminDb & "..MetalMast "
            strSql += " where ISNULL(ACTIVE,'') <> 'N' AND MetalId = 'D' or MetalId = 'T' ORDER BY DISPLAYORDER "
        End If
        objGPack.FillCombo(strSql, cmbMetal)
    End Function
    Function funcLoadPurity()
        Dim dt As New DataTable
        dt.Clear()
        cmbPurity.Items.Clear()
        Dim type As String = Nothing
        If rbtMetal.Checked = True Then
            type = "M"
        ElseIf rbtOrnament.Checked = True Then
            type = "O"
        End If
        strSql = "select PurityName from " & cnAdminDb & "..PurityMast where MetalType = '" & type & "'"
        strSql += " and metalid = (select metalid from " & cnAdminDb & "..MetalMast where MetalName = '" & cmbMetal.Text & "')"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            Dim cnt As Integer
            For cnt = 0 To dt.Rows.Count - 1
                cmbPurity.Items.Add(dt.Rows(cnt).Item("PurityName").ToString)
            Next
            cmbPurity.Text = dt.Rows(0).Item("PurityName").ToString
            'Else
            '    If rbtOrnament.Checked = True Then
            '        rbtMetal.Checked = True
            '        rbtMetal.Focus()
            '        MsgBox("Ornament Purity does Not exist in Purity Master", MsgBoxStyle.Information)
            '        Return 0
            '    End If
        End If
        Return 0
    End Function
    Function funcLoadTaxMode()
        Dim dt As New DataTable
        dt.Clear()
        cmbTaxMode.Items.Clear()
        strSql = " Select TaxName from " & cnAdminDb & "..TaxMast"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            Dim cnt As Integer
            For cnt = 0 To dt.Rows.Count - 1
                cmbTaxMode.Items.Add(dt.Rows(cnt).Item("TaxName").ToString)
            Next
            cmbTaxMode.Text = "GST"
        End If
        Return 0
    End Function
    Function funcGetCatGroup() As String
        Select Case cmbStockGroup.Text
            Case "BAR"
                Return "B"
            Case "OLD"
                Return "L"
            Case "REPAIR"
                Return "R"
            Case "ORDER"
                Return "O"
            Case "ORNAMENTS"
                Return "P"
            Case "PARTY METALS"
                Return "Y"
            Case Else
                Return "Y"
        End Select
    End Function

    Private Sub frmCategory_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If tabMain.SelectedTab.Name = tabView.Name Then
                tabMain.SelectedTab = tabGeneral
                cmbMetal.Focus()
            End If
        End If
    End Sub

    Private Sub frmCategory_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub frmCategory_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Me.WindowState = FormWindowState.Maximized
        cmbActive.Items.Add("YES")
        cmbActive.Items.Add("NO")
        cmbActive.Text = "YES"
        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGeneral.Left, Me.tabGeneral.Top, Me.tabGeneral.Width, Me.tabGeneral.Height))
        funcNew()
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        funcSave()
    End Sub
    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        funcOpen()
        tabMain.SelectedTab = tabView
        cmbOpenMetal_Man.Select()
    End Sub
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        funcNew()
    End Sub
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        funcExit()
    End Sub
    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        funcSave()
    End Sub
    Private Sub OpenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click
        btnOpen_Click(Me, New EventArgs)
    End Sub
    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub
    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        btnExit_Click(Me, New EventArgs)
    End Sub
    Private Sub rbtMetal_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtMetal.CheckedChanged
        cmbMetal.Text = ""
        cmbPurity.Text = ""
        cmbTaxMode.Text = ""
        funcLoadMetal()
        If flagSave = True Then
            Exit Sub
        End If
        'cmbMetal.Items.Clear()
        'cmbPurity.Items.Clear()
        'cmbTaxMode.Items.Clear()
        If rbtMetal.Checked = True Then
            chkMetal.Checked = True
            chkOrnament.Checked = False
            cmbStockGroup.Text = "BAR"
            'funcLoadPurity()
        End If
    End Sub

    Private Sub rbtOrnament_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtOrnament.CheckedChanged
        cmbMetal.Text = ""
        cmbPurity.Text = ""
        cmbTaxMode.Text = ""
        funcLoadMetal()
        If flagSave = True Then
            Exit Sub
        End If
        'cmbMetal.Items.Clear()
        'cmbPurity.Items.Clear()
        'cmbTaxMode.Items.Clear()
        If rbtOrnament.Checked = True Then
            chkMetal.Checked = False
            chkOrnament.Checked = True
            cmbStockGroup.Text = "ORNAMENTS"
            'funcLoadPurity()
        End If
    End Sub

    Private Sub rbtStoneDiamond_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtStoneDiamond.CheckedChanged
        cmbMetal.Text = ""
        cmbPurity.Text = ""
        cmbTaxMode.Text = ""
        funcLoadMetal()
        If flagSave = True Then
            Exit Sub
        End If
        'cmbMetal.Items.Clear()
        'cmbPurity.Items.Clear()
        'cmbTaxMode.Items.Clear()
        'cmbPurity.Items.Clear()
        If rbtStoneDiamond.Checked = True Then
            pnlStone.Enabled = True
            rbtStone.Checked = True
        Else
            pnlStone.Enabled = False
        End If
    End Sub

    Private Sub cmbMetal_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbMetal.GotFocus

    End Sub

    Private Sub cmbPurity_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbPurity.GotFocus
        If Not cmbPurity.Items.Count > 0 Then
            cmbPurity.Text = ""
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub cmbTaxMode_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbTaxMode.GotFocus

    End Sub
    Private Sub txtName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtName__Man.GotFocus
        If flagSave = False Then
            If rbtStoneDiamond.Checked Then
                txtName__Man.Text = cmbMetal.Text + " " + cmbTaxMode.Text
            Else
                txtName__Man.Text = cmbPurity.Text + " " + cmbTaxMode.Text
            End If

            funcAcNameGeneration()
        Else
            funcAcNameGenerationMis()
        End If
    End Sub

    Private Sub gridView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.GotFocus
        lblStatus.Visible = True
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            If gridView.RowCount > 0 Then
                gridView.CurrentCell = gridView.CurrentCell
                funcGetDetails(gridView.Item(0, gridView.CurrentRow.Index).Value)
                tabMain.SelectedTab = tabGeneral
                cmbMetal.Focus()
            End If
        End If
    End Sub

    Private Sub chkOrnament_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkOrnament.CheckedChanged
        If chkOrnament.Checked = True Then
            chkMetal.Checked = False
        Else
            chkMetal.Checked = True
        End If
    End Sub

    Private Sub chkMetal_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkMetal.CheckedChanged
        If chkMetal.Checked = True Then
            chkOrnament.Checked = False
        Else
            chkOrnament.Checked = True
        End If
    End Sub

    Private Sub txtName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtName__Man.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            strSql = " SELECT 'CHECK' FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & txtName__Man.Text & "' AND CATCODE <> '" & CatCode & "'"
            If objGPack.GetSqlValue(strSql).Length > 0 Then
                MsgBox("Catname Already Exist", MsgBoxStyle.Information)
                txtName__Man.Focus()
                Exit Sub
            End If
            If flagSave = False Then
                funcAcNameGeneration()
            Else
                funcAcNameGenerationMis()
            End If
        End If
    End Sub

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        funcOpen()
        tabMain.SelectedTab = tabView
        cmbOpenMetal_Man.Select()
        gridView.Select()
    End Sub

    Private Sub gridView_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.LostFocus
        lblStatus.Visible = False

    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If Not gridView.RowCount > 0 Then
            Exit Sub
        End If
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
        Dim delKey As String = gridView.Rows(gridView.CurrentRow.Index).Cells("CATCODE").Value.ToString
        Dim chkQry As String = Nothing
        strSql = " SELECT DBNAME FROM " & cnAdminDb & "..DBMASTER"
        Dim dtDb As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtDb)
        If dtDb.Rows.Count > 0 Then
            chkQry += " SELECT TOP 1 CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE CATCODE = '" & delKey & "'"
            chkQry += " UNION"
            chkQry += " SELECT TOP 1 CATCODE FROM " & cnAdminDb & "..ITEMTYPE WHERE CATCODE = '" & delKey & "'"
            chkQry += " UNION"
            chkQry += " SELECT TOP 1 CATCODE FROM " & cnAdminDb & "..MISCCHARGES WHERE CATCODE = '" & delKey & "'"
            chkQry += " UNION"
            chkQry += " SELECT TOP 1 CATCODE FROM " & cnAdminDb & "..OUTSTANDING WHERE CATCODE = '" & delKey & "'"
            chkQry += " UNION"
            chkQry += " SELECT TOP 1 CATCODE FROM " & cnAdminDb & "..ITEMTAGMETAL WHERE CATCODE = '" & delKey & "'"
            For cnt As Integer = 0 To dtDb.Rows.Count - 1
                With dtDb.Rows(cnt)
                    If GetSqlValue(cn, "SELECT TOP 1 1 FROM MASTER..SYSDATABASES WHERE NAME = '" & .Item("DBNAME").ToString & "'") <> 1 Then Continue For
                    chkQry += " UNION"
                    chkQry += " SELECT TOP 1 CATCODE FROM " & .Item("DBNAME").ToString & "..ISSSTONE WHERE CATCODE = '" & delKey & "'"
                    chkQry += " UNION"
                    chkQry += " SELECT TOP 1 CATCODE FROM " & .Item("DBNAME").ToString & "..RECEIPT WHERE CATCODE = '" & delKey & "'"
                    chkQry += " UNION"
                    chkQry += " SELECT TOP 1 CATCODE FROM " & .Item("DBNAME").ToString & "..RECEIPTSTONE WHERE CATCODE = '" & delKey & "'"
                    chkQry += " UNION"
                    chkQry += " SELECT TOP 1 CATCODE FROM " & .Item("DBNAME").ToString & "..OPENWEIGHT WHERE CATCODE = '" & delKey & "'"
                    chkQry += " UNION"
                    chkQry += " SELECT TOP 1 CATCODE FROM " & .Item("DBNAME").ToString & "..OPENITEM WHERE CATCODE = '" & delKey & "'"
                    chkQry += " UNION"
                    chkQry += " SELECT TOP 1 CATCODE FROM " & .Item("DBNAME").ToString & "..ESTISSUE WHERE CATCODE = '" & delKey & "'"
                    chkQry += " UNION"
                    chkQry += " SELECT TOP 1 CATCODE FROM " & .Item("DBNAME").ToString & "..ESTISSSTONE WHERE CATCODE = '" & delKey & "'"
                    chkQry += " UNION"
                    chkQry += " SELECT TOP 1 CATCODE FROM " & .Item("DBNAME").ToString & "..ESTRECEIPT WHERE CATCODE = '" & delKey & "'"
                    chkQry += " UNION"
                    chkQry += " SELECT TOP 1 CATCODE FROM " & .Item("DBNAME").ToString & "..ESTRECEIPTSTONE WHERE CATCODE = '" & delKey & "'"
                    chkQry += " UNION"
                    chkQry += " SELECT TOP 1 CATCODE FROM " & .Item("DBNAME").ToString & "..ISSUE WHERE CATCODE = '" & delKey & "'"

                    'If cnt <> dtDb.Rows.Count - 1 Then
                    '    chkQry += " UNION "
                    'End If
                End With
            Next
            DeleteItem(SyncMode.Master, chkQry, "DELETE FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = '" & delKey & "' AND ISNULL(AUTOGENERATOR,'') = ''")
            funcOpen()
        End If
    End Sub

    Private Sub gridView_RowEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.RowEnter
        objGPack.TextClear(grpInfo)
        With gridView.Rows(e.RowIndex)
            detSalPur.Text = .Cells("CATMODE").Value.ToString
            detSmithTrans.Text = .Cells("TRANTYPE").Value.ToString
            detStockGroup.Text = .Cells("CATGROUP").Value.ToString
            If .Cells("DIASTNTYPE").Value.ToString <> "" Then
                If .Cells("DIASTNTYPE").Value.ToString = "D" Then
                    detStone.Text = "DIAMOND"
                ElseIf .Cells("DIASTNTYPE").Value.ToString = "T" Then
                    detStone.Text = "STONE"
                Else
                    detStone.Text = "PRECEIOUS"
                End If
            End If
            If .Cells("GS11").Value.ToString = "YES" Then
                detStockType.Text += "GS11,"
            End If
            If .Cells("GS12").Value.ToString = "YES" Then
                detStockType.Text += "GS12"
            End If
            detDisplayOrder.Text = .Cells("DISPLAYORDER").Value.ToString
            detHaving.Text = .Cells("ALLOY").Value.ToString
            detLedgerPrint.Text = .Cells("LEDGERPRINT").Value.ToString
            If .Cells("ESTDISPLAY").Value.ToString = "YES" Then
                detDisplay.Text += "EST,"
            End If
            If .Cells("BILLDISPLAY").Value.ToString = "YES" Then
                detDisplay.Text += "BILL,"
            End If
            If .Cells("ACCTDISPLAY").Value.ToString = "YES" Then
                detDisplay.Text += "ACC,"
            End If
            detSalAc.Text = .Cells("SALESIDNAME").Value.ToString
            detSalTaxAc.Text = .Cells("STAXIDNAME").Value.ToString
            detSalScAc.Text = .Cells("SSCIDNAME").Value.ToString
            detSalAdlScAc.Text = .Cells("SASCIDNAME").Value.ToString
            detSalTax.Text = .Cells("SALESTAX").Value.ToString
            detSalSc.Text = .Cells("SSC").Value.ToString
            detSalAdlSc.Text = .Cells("SASC").Value.ToString

            detPurAc.Text = .Cells("PURCHASEIDNAME").Value.ToString
            detPurTaxAc.Text = .Cells("PTAXIDNAME").Value.ToString
            detPurScAC.Text = .Cells("PSCIDNAME").Value.ToString
            detPurAdlScAc.Text = .Cells("PASCIDNAME").Value.ToString
            detPurTax.Text = .Cells("PTAX").Value.ToString
            detPurSc.Text = .Cells("PSC").Value.ToString
            detPurAdlSc.Text = .Cells("PASC").Value.ToString

        End With
    End Sub

    Private Sub cmbSAC_OWN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbSAC_OWN.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If cmbSAC_OWN.Text = "" Then
                MsgBox(objGPack.GetNextLable(Me, cmbSAC_OWN, False).Text + E0001, MsgBoxStyle.Information)
            ElseIf flagSave And cmbSAC_OWN.Items.Contains(cmbSAC_OWN.Text) = False Then
                MsgBox(E0004 + objGPack.GetNextLable(Me, cmbSAC_OWN, False).Text, MsgBoxStyle.Information)
            End If
        End If
    End Sub

    Private Sub cmbSTax_OWN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbSTax_OWN.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If cmbSTax_OWN.Text = "" Then
                MsgBox(objGPack.GetNextLable(Me, cmbSTax_OWN, False).Text + E0001, MsgBoxStyle.Information)
            ElseIf flagSave And cmbSTax_OWN.Items.Contains(cmbSTax_OWN.Text) = False Then
                MsgBox(E0004 + objGPack.GetNextLable(Me, cmbSTax_OWN, False).Text, MsgBoxStyle.Information)
            End If
        End If
    End Sub

    Private Sub cmbSSurcharge_OWN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbSSurcharge_OWN.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If Val(txtSSurcharge_Per.Text) > 0 And cmbSSurcharge_OWN.Text = "" Then
                MsgBox(objGPack.GetNextLable(Me, cmbSSurcharge_OWN, False).Text + E0001, MsgBoxStyle.Information)
            End If
        End If
    End Sub

    Private Sub cmbSASCharge_OWN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbSASCharge_OWN.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If Val(txtSASCharge_Per.Text) > 0 And cmbSASCharge_OWN.Text = "" Then
                MsgBox(objGPack.GetNextLable(Me, cmbSASCharge_OWN, False).Text + E0001, MsgBoxStyle.Information)
            End If
        End If
    End Sub

    Private Sub cmbPAC_OWN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbPAC_OWN.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If cmbPAC_OWN.Text = "" Then
                MsgBox(objGPack.GetNextLable(Me, cmbPAC_OWN, False).Text + E0001, MsgBoxStyle.Information)
            ElseIf flagSave And cmbPAC_OWN.Items.Contains(cmbPAC_OWN.Text) = False Then
                MsgBox(E0004 + objGPack.GetNextLable(Me, cmbPAC_OWN, False).Text, MsgBoxStyle.Information)
            End If
        End If
    End Sub

    Private Sub cmbPTax_OWN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbPTax_OWN.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If cmbPTax_OWN.Text = "" Then
                MsgBox(objGPack.GetNextLable(Me, cmbPTax_OWN, False).Text + E0001, MsgBoxStyle.Information)
            ElseIf flagSave And cmbPTax_OWN.Items.Contains(cmbPTax_OWN.Text) = False Then
                MsgBox(E0004 + objGPack.GetNextLable(Me, cmbPTax_OWN, False).Text, MsgBoxStyle.Information)
            End If
        End If
    End Sub

    Private Sub cmbPSurcharge_OWN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbPSurcharge_OWN.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If Val(txtPSurcharge_Per.Text) > 0 And cmbPSurcharge_OWN.Text = "" Then
                MsgBox(objGPack.GetNextLable(Me, cmbPSurcharge_OWN, False).Text + E0001, MsgBoxStyle.Information)
            End If
        End If
    End Sub

    Private Sub cmbPASCharge_OWN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbPASCharge_OWN.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If Val(txtPASCharge_Per.Text) > 0 And cmbPASCharge_OWN.Text = "" Then
                MsgBox(objGPack.GetNextLable(Me, cmbPASCharge_OWN, False).Text + E0001, MsgBoxStyle.Information)
            End If
        End If
    End Sub

    Private Sub cmbMetal_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbMetal.SelectedIndexChanged
        funcLoadPurity()
    End Sub

    Private Sub cmbTaxMode_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbTaxMode.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If Not cmbTaxMode.Items.Count > 0 Then
                MsgBox("Tax should not empty", MsgBoxStyle.Information)
                cmbTaxMode.Focus()
            ElseIf cmbTaxMode.Items.Contains(cmbTaxMode.Text) = False Then
                MsgBox("Invalid tax", MsgBoxStyle.Information)
                cmbTaxMode.Focus()
            End If
        End If
    End Sub

    Private Sub cmbPurity_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbPurity.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            Dim dt As New DataTable
            dt.Clear()
            Dim type As String = Nothing
            If rbtMetal.Checked = True Then
                type = "M"
            ElseIf rbtOrnament.Checked = True Then
                type = "O"
            End If
            strSql = "select PurityName from " & cnAdminDb & "..PurityMast where MetalType = '" & type & "'"
            strSql += " and metalid = (select metalid from " & cnAdminDb & "..MetalMast where MetalName = '" & cmbMetal.Text & "')"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If Not dt.Rows.Count > 0 And rbtOrnament.Checked Then
                rbtMetal.Checked = True
                rbtMetal.Focus()
                MsgBox("Ornament Purity does Not exist in Purity Master", MsgBoxStyle.Information)
            End If
        End If
    End Sub


End Class