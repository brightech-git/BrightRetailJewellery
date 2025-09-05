Imports System.Data.OleDb
Public Class frmItemMaster
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim flagSave As Boolean = False
    Dim dtCompany As New DataTable
    Dim PctFile As String
    Dim OpenFileDia As New OpenFileDialog
    Dim studdeductsoft As String
    Dim STKCHK_ITEMEDIT As Boolean = IIf(GetAdmindbSoftValue("STKCHK_ITEMEDIT", "Y") = "Y", True, False)
    Dim Lang As String = GetAdmindbSoftValue("BILLNAME_LANG", "en")
    Dim ITEMMAST_WT As Boolean = IIf(GetAdmindbSoftValue("ITEMMAST_WT", "Y") = "Y", True, False)
    Dim Itemmast_PctPath As Boolean = IIf(GetAdmindbSoftValue("PICPATHFROM", "S") = "I", True, False)

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        lblStatus.Visible = False

        funcGridStyle(gridView)
        gridView.RowTemplate.Height = 18
        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGeneral.Left, Me.tabGeneral.Top, Me.tabGeneral.Width, Me.tabGeneral.Height))
        tabGeneral.BackgroundImage = bakImage
        tabView.BackgroundImage = bakImage
        tabGeneral.BackgroundImageLayout = ImageLayout.Stretch
        tabView.BackgroundImageLayout = ImageLayout.Stretch

        cmbDesignCode.Items.Add("YES")
        cmbDesignCode.Items.Add("NO")

        cmbExtraWt.Items.Add("NO")
        cmbExtraWt.Items.Add("YES")
        cmbExtraWt.Text = "NO"

        cmbCoverWt.Items.Add("NO")
        cmbCoverWt.Items.Add("YES")
        cmbCoverWt.Text = "NO"

        cmbTagWt.Items.Add("NO")
        cmbTagWt.Items.Add("YES")
        cmbTagWt.Text = "NO"

        cmbStockType.Items.Add("TAGED")
        cmbStockType.Items.Add("NON TAGED")
        cmbStockType.Items.Add("PACKET BASED")

        cmbStockType.Text = "TAGED"

        cmbSubItem.Items.Add("YES")
        cmbSubItem.Items.Add("NO")
        cmbSubItem.Text = "NO"

        cmbCalType.Items.Add("WEIGHT")
        cmbCalType.Items.Add("RATE")
        cmbCalType.Items.Add("BOTH")
        cmbCalType.Items.Add("FIXED")
        cmbCalType.Items.Add("METAL RATE")
        cmbCalType.Items.Add("PIECES")
        cmbCalType.Text = "WEIGHT"

        cmbSetItem.Items.Add("NO")
        cmbSetItem.Items.Add("YES")
        cmbSetItem.Text = "NO"

        CmbHallMark.Items.Add("NO")
        CmbHallMark.Items.Add("YES")
        CmbHallMark.Text = "NO"

        cmbIsService.Items.Clear()
        cmbIsService.Items.Add("NO")
        cmbIsService.Items.Add("YES")
        cmbIsService.Text = "NO"

        cmbTouchBased.Items.Clear()
        cmbTouchBased.Items.Add("NO")
        cmbTouchBased.Items.Add("YES")
        cmbTouchBased.Text = "NO"


        cmbValueAddType.Items.Add("ITEM")
        cmbValueAddType.Items.Add("TABLE")
        cmbValueAddType.Items.Add("DESIGNER")
        cmbValueAddType.Items.Add("TAG")
        cmbValueAddType.Text = "ITEM"

        cmbStockSize.Items.Add("YES")
        cmbStockSize.Items.Add("NO")
        cmbStockSize.Text = "NO"

        cmbMcCalcOn.Items.Add("GRS WT")
        cmbMcCalcOn.Items.Add("NET WT")
        cmbMcCalcOn.Text = "NET WT"

        cmbWastCalcon.Items.Add("GRS WT")
        cmbWastCalcon.Items.Add("NET WT")
        cmbWastCalcon.Text = "NET WT"

        cmbStuddedStone.Items.Add("YES")
        cmbStuddedStone.Items.Add("NO")
        cmbStuddedStone.Text = "NO"

        '''''''''''''''''''''''''''''''''''
        '''''''''''''''''''''''''''''''''''
        cmbTaxInclusive.Items.Add("YES")
        cmbTaxInclusive.Items.Add("NO")
        cmbTaxInclusive.Text = "NO"

        cmbRateGet.Items.Add("YES")
        cmbRateGet.Items.Add("NO")
        cmbRateGet.Text = "NO"

        cmbGrossWtDiff.Items.Add("YES")
        cmbGrossWtDiff.Items.Add("NO")
        cmbGrossWtDiff.Text = "YES"

        cmbOtherCharge.Items.Add("YES")
        cmbOtherCharge.Items.Add("NO")
        cmbOtherCharge.Text = "NO"

        cmbMultiMetal.Items.Add("YES")
        cmbMultiMetal.Items.Add("NO")
        cmbMultiMetal.Text = "NO"

        cmbStockReport.Items.Add("YES")
        cmbStockReport.Items.Add("NO")
        cmbStockReport.Text = "YES"

        cmbActive.Items.Add("YES")
        cmbActive.Items.Add("NO")
        cmbActive.Text = "YES"

        cmbRangeStock.Items.Add("YES")
        cmbRangeStock.Items.Add("NO")
        cmbRangeStock.Text = "NO"

        cmbTagType.Items.Add("YES")
        cmbTagType.Items.Add("NO")
        cmbTagType.Text = "NO"

        cmbFocusPiece.Items.Add("YES")
        cmbFocusPiece.Items.Add("NO")
        cmbFocusPiece.Text = "YES"

        cmbZeroWast.Items.Add("YES")
        cmbZeroWast.Items.Add("NO")
        cmbZeroWast.Text = "NO"
        ''''Studded
        cmbStudded.Items.Add("STUDDED")
        cmbStudded.Items.Add("LOOSE")
        cmbStudded.Items.Add("BOTH")
        cmbStudded.Text = "STUDDED"


        cmbStoneUnit.Items.Add("CARET")
        cmbStoneUnit.Items.Add("GRM")
        cmbStoneUnit.Text = "CARET"

        cmbDiaStone.Items.Add("DIA")
        cmbDiaStone.Items.Add("STONE")
        cmbDiaStone.Items.Add("PRECISIOUS")
        cmbDiaStone.Text = "STONE"

        cmbBeeds.Items.Add("YES")
        cmbBeeds.Items.Add("NO")
        cmbBeeds.Text = "YES"

        cmbAllowZeroPcs.Items.Add("YES")
        cmbAllowZeroPcs.Items.Add("NO")
        cmbAllowZeroPcs.Text = "NO"

        cmbFixedVa.Items.Add("YES")
        cmbFixedVa.Items.Add("NO")
        cmbFixedVa.Text = "NO"

        cmbTagvalid.Items.Add("YES")
        cmbTagvalid.Items.Add("NO")
        cmbTagvalid.Text = "YES"

        cmbTagLock.Items.Add(" ")
        cmbTagLock.Items.Add("YES")
        cmbTagLock.Items.Add("NO")

        cmbTagLock.Text = ""

        CmbMcAsVaPer.Items.Add("YES")
        CmbMcAsVaPer.Items.Add("NO")
        CmbMcAsVaPer.Text = "NO"

        cmb4C.Items.Add("YES")
        cmb4C.Items.Add("NO")
        cmb4C.Text = "NO"

        pnlGrid.Dock = DockStyle.Fill
        pnlGrid.BringToFront()
        If objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'MC_ON_WTAMT'", , "W") = "W" Then
            CmbMcAsVaPer.Visible = False
            lblMcVaper.Visible = False
        End If

        If objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'ALLOYGOLD_SALEMODE'", , "N") = "Y" Then
            pnlAlloySale.Enabled = True
        End If

        studdeductsoft = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'STUDWTDEDUCT'", , "")
        'pnlGrid.Size = Panel1.Size
        'pnlGrid.Location = Panel1.Location
        funcNew()
    End Sub
    Function funcCallGrid() As Integer
        Dim dt As New DataTable
        dt.Clear()
        strSql = " SELECT"
        strSql += vbCrLf + " ITEMID,HSN,ITEMNAME,SHORTNAME,"
        strSql += vbCrLf + " (SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = I.METALID)AS METALNAME,"
        strSql += vbCrLf + " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY AS C WHERE C.CATCODE = I.CATCODE)AS CATNAME,"
        strSql += vbCrLf + " CASE WHEN STOCKTYPE = 'T' THEN 'TAGED' ELSE 'NON TAGED' END AS STOCKTYPE,"
        strSql += vbCrLf + " CASE WHEN SUBITEM = 'Y' THEN 'YES' ELSE 'NO' END AS SUBITEM,"
        strSql += vbCrLf + " CASE WHEN CALTYPE = 'B' THEN 'BOTH' "
        strSql += vbCrLf + " WHEN CALTYPE = 'F' THEN 'FIXED'"
        strSql += vbCrLf + " WHEN CALTYPE = 'M' THEN 'METAL RATE'"
        strSql += vbCrLf + " WHEN CALTYPE = 'D' THEN 'DIRECT'"
        strSql += vbCrLf + " WHEN CALTYPE = 'R' THEN 'RATE'"
        strSql += vbCrLf + " WHEN CALTYPE = 'P' THEN 'PIECES'"
        strSql += vbCrLf + " ELSE 'WEIGHT' END CALTYPE,"

        strSql += vbCrLf + " CASE WHEN VALUEADDEDTYPE = 'I' THEN 'ITEM' "
        strSql += vbCrLf + " WHEN VALUEADDEDTYPE = 'T' THEN 'TABLE' WHEN VALUEADDEDTYPE = 'D' THEN 'DESIGNER' WHEN VALUEADDEDTYPE = 'P' THEN 'TAG' ELSE '' END VALUEADDEDTYPE,"
        strSql += vbCrLf + " TABLECODE,"
        strSql += vbCrLf + " CASE WHEN STUDDEDSTONE = 'Y' THEN 'YES' "
        strSql += vbCrLf + " WHEN STUDDEDSTONE = 'N' THEN 'NO'"
        strSql += vbCrLf + " ELSE '' END STUDDEDSTONE,"
        strSql += vbCrLf + " CASE WHEN SIZESTOCK = 'Y' THEN 'YES' "
        strSql += vbCrLf + " WHEN SIZESTOCK ='N' THEN 'NO'"
        strSql += vbCrLf + " ELSE '' END AS SIZESTOCK,"
        strSql += vbCrLf + " CASE WHEN TAXINCLUCIVE = 'Y' THEN 'YES' ELSE 'NO' END AS TAXINCLUCIVE,"

        strSql += vbCrLf + " CASE WHEN RATEGET = 'Y' THEN 'YES' "
        strSql += vbCrLf + " WHEN RATEGET = 'N' THEN 'NO'"
        strSql += vbCrLf + " ELSE '' END AS RATEGET,"
        strSql += vbCrLf + " CASE WHEN GROSSNETWTDIFF = 'Y' THEN 'YES' "
        strSql += vbCrLf + " WHEN GROSSNETWTDIFF = 'N' THEN 'NO'"
        strSql += vbCrLf + " ELSE '' END AS GROSSNETWTDIFF,"
        strSql += vbCrLf + " CASE WHEN OTHCHARGE = 'Y' THEN 'YES' "
        strSql += vbCrLf + " WHEN OTHCHARGE = 'N' THEN 'NO'"
        strSql += vbCrLf + " ELSE '' END AS OTHCHARGE,"
        strSql += vbCrLf + " CASE WHEN MULTIMETAL = 'Y' THEN 'YES' "
        strSql += vbCrLf + " WHEN MULTIMETAL = 'N' THEN 'NO'"
        strSql += vbCrLf + " ELSE '' END AS MULTIMETAL,"

        strSql += vbCrLf + " ISNULL((SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE  ITEMCTRID = DEFAULTCOUNTER),'')AS ITEMCOUNTER,"
        strSql += vbCrLf + " CASE WHEN STOCKREPORT = 'Y' THEN 'YES' ELSE 'NO' END AS STOCKREPORT,"
        strSql += vbCrLf + " CASE WHEN ACTIVE = 'Y' THEN 'YES' ELSE 'NO' END AS ACTIVE,"
        strSql += vbCrLf + " (SELECT GROUPNAME FROM " & cnAdminDb & "..ITEMGROUPMAST AS C WHERE C.GROUPID = I.ITEMGROUP)AS ITEMGROUP,"

        strSql += vbCrLf + " CASE WHEN RANGESTOCK = 'Y' THEN 'YES' "
        strSql += vbCrLf + " WHEN RANGESTOCK = 'N' THEN 'NO'"
        strSql += vbCrLf + " ELSE '' END AS RANGESTOCK,"
        strSql += vbCrLf + " CASE WHEN TAGTYPE = 'Y' THEN 'YES' "
        strSql += vbCrLf + " WHEN TAGTYPE = 'N' THEN 'NO'"
        strSql += vbCrLf + " ELSE '' END AS TAGTYPE,"

        strSql += vbCrLf + " STARTTAG,ENDTAGNO,CURRENTTAGNO,"
        strSql += vbCrLf + " CASE WHEN FOCUSPIECE = 'Y' THEN 'YES' ELSE 'NO' END AS FOCUSPIECE,"
        strSql += vbCrLf + " NOOFPIECE,TAGPRINTSTYLE,METALRATE,PIECERATE,PIECERATE_PUR,"
        strSql += vbCrLf + " SALTOUCH,NETWTPER,GPPER,"
        strSql += vbCrLf + " CASE WHEN STUDDED ='S' THEN 'STUDDED'"
        strSql += vbCrLf + " WHEN STUDDED = 'L' THEN 'LOOSE' WHEN STUDDED ='B' THEN 'BOTH' ELSE '' END STUDDED,"
        strSql += vbCrLf + " CASE WHEN STONEUNIT = 'S' THEN 'STONE'"
        strSql += vbCrLf + " WHEN STONEUNIT = 'G' THEN 'GRM' ELSE '' END STONEUNIT,"
        strSql += vbCrLf + " CASE WHEN DIASTONE = 'D' THEN 'DIA'"
        strSql += vbCrLf + " WHEN DIASTONE ='S' THEN 'STONE'"
        strSql += vbCrLf + " WHEN DIASTONE ='P' THEN 'PRECIOUS' ELSE '' END DIASTONE,"
        strSql += vbCrLf + " CASE WHEN BEEDS = 'Y' THEN 'YES' ELSE 'NO' END AS BEEDS,"
        strSql += vbCrLf + " CASE WHEN STYLENO = 'Y' THEN 'YES' ELSE 'NO' END AS STYLENO"
        strSql += vbCrLf + " ,PURTOUCH,STYLECODE,CURRENT_STYLENO,GRSWT,FROMWT,TOWT,GIFTVALUE"
        strSql += vbCrLf + " ,CASE WHEN ISNULL(MCCALC,'N') = 'G' THEN 'GRS WT' ELSE 'NET WT' END AS MCCALC"
        strSql += vbCrLf + " ,CASE WHEN ISNULL(ALLOWZEROPCS,'N') = 'N' THEN 'NO' ELSE 'YES' END AS ALLOWZEROPCS"
        strSql += vbCrLf + " ,CASE WHEN ISNULL(FIXEDVA,'N') = 'N' THEN 'NO' ELSE 'YES' END AS FIXEDVA"
        strSql += vbCrLf + " ,CASE ISNULL(BOOKSTOCK,'B') WHEN 'B' THEN 'BOTH' WHEN 'N' THEN 'NONE' WHEN 'P' THEN 'PCS' ELSE 'WEIGHT' END AS BOOKSTOCK"
        strSql += vbCrLf + " ,CASE WHEN ISNULL(ZEROWASTAGE,'N') = 'N' THEN 'NO' ELSE 'YES' END AS ZEROWASTAGE"
        strSql += vbCrLf + " ,CASE WHEN ISNULL(EXTRAWT,'N') = 'N' THEN 'NO' ELSE 'YES' END AS EXTRAWT"
        strSql += vbCrLf + " ,CASE WHEN ISNULL(TAGWT,'N') = 'N' THEN 'NO' ELSE 'YES' END AS TAGWT"
        strSql += vbCrLf + " ,CASE WHEN ISNULL(COVERWT,'N') = 'N' THEN 'NO' ELSE 'YES' END AS COVERWT"
        strSql += vbCrLf + " ,ISNULL((SELECT NAME FROM " & cnAdminDb & "..ITEMTYPE WHERE  ITEMTYPEID = I.DEFITEMTYPEID),'') AS ITEMTYPENAME"
        strSql += vbCrLf + " ,ISNULL((SELECT EMPNAME FROM " & cnAdminDb & "..EMPMASTER WHERE  EMPID = I.DEFAULTEMPID),'') AS EMPNAME"
        If Itemmast_PctPath Then
            strSql += " ,ISNULL(ITEMPCTPATH,'')ITEMPCTPATH"
        End If
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMMAST AS I"
        If cmbOpenMetalName.Text <> "ALL" And cmbOpenMetalName.Text <> "" Then
            strSql += vbCrLf + " WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbOpenMetalName.Text & "')"
        End If
        If cmbOpenCategory.Text <> "ALL" And cmbOpenCategory.Text <> "" Then
            strSql += vbCrLf + " AND CATCODE = (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & cmbOpenCategory.Text & "')"
        End If
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        gridView.DataSource = dt
        With gridView
            .Columns("ITEMID").Width = 55
            .Columns("HSN").Width = 55
            .Columns("ITEMNAME").Width = 220
            .Columns("SHORTNAME").Width = 100
            .Columns("METALNAME").Width = 120
            .Columns("CATNAME").Width = 250
        End With
        For CNT As Integer = 6 To gridView.ColumnCount - 1
            gridView.Columns(CNT).Visible = False
        Next
        If ITEMMAST_WT Then
            gridView.Columns("FROMWT").Visible = True
            gridView.Columns("TOWT").Visible = True

            gridView.Columns("FROMWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            gridView.Columns("TOWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        End If
        gridView.Focus()

    End Function
    Function funcNew()
        funcClear()
        objGPack.TextClear(grpInfo)
        cmbOpenMetalName.Items.Clear()
        cmbOpenMetalName.Items.Add("ALL")
        cmbOpenMetalName.Text = "ALL"
        strSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE ISNULL(ACTIVE,'') <> 'N' ORDER BY DISPLAYORDER,METALNAME"
        objGPack.FillCombo(strSql, cmbOpenMetalName, False, False)

        cmbItemType.Items.Clear()
        cmbItemType.Items.Add("")
        strSql = " SELECT NAME FROM " & cnAdminDb & "..ITEMTYPE ORDER BY NAME"
        objGPack.FillCombo(strSql, cmbItemType, False, False)
        cmbItemType.Text = ""
        txtEmpId_NUM.Text = ""

        cmbAssorted.Items.Clear()
        cmbAssorted.Items.Add("YES")
        cmbAssorted.Items.Add("NO")


        funcDefaultValues()
        funcLoadMetalName()
        txtItemId_Num_Man.Enabled = True
        flagSave = False
        cmbMetalName_Man.Enabled = True
        funcLoadItemCounter()
        funcLoadItemGroup()
        ChkCmb4CView.Items.Clear()
        ChkCmb4CView.Items.Add("CUT")
        ChkCmb4CView.Items.Add("COLOR")
        ChkCmb4CView.Items.Add("CLARITY")
        ChkCmb4CView.Items.Add("SHAPE")
        ChkCmb4CView.Items.Add("SIZE")
        ChkCmb4CView.Items.Add("SETTING TYPE")
        ChkCmb4CView.Items.Add("HEIGHT")
        ChkCmb4CView.Items.Add("WIDTH")
        ChkCmb4CView.Text = ""

        cmbStuddedless.Items.Clear()
        cmbStuddedless.Items.Add("YES")
        cmbStuddedless.Items.Add("NO")
        cmbStuddedless.Text = "NO"
        cmbSetItem.Text = "NO"
        CmbHallMark.Text = "NO"
        cmbIsService.Text = "NO"
        cmbTouchBased.Text = "NO"
        chkCmbCompany.Select()

        txtItemId_Num_Man.Text = objGPack.GetSqlValue("SELECT ISNULL(MAX(ITEMID),0)+1 ITEMID  FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID NOT IN (9998,9999)", "ITEMID", 1)
        Return 0
    End Function
    Function funcClear()
        txtHsnCode.Clear()
        txtBillName.Clear()
        txtItemId_Num_Man.Clear()
        txtItemName__Man.Clear()
        txtShortName.Clear()
        txtTableCode.Clear()
        '''''''''''''''''''
        cmbItemGroup.Items.Clear()
        txtStartTagNo.Clear()
        txtEndTagNo.Clear()
        txtCurrentTagNo.Clear()
        txtNoOfPiece_Num.Clear()
        txtTagPrintStyle_Num.Clear()
        txtMetalRate_Amt.Clear()
        txtPieceRate_Amt.Clear()
        txtPieceRatePur_Amt.Clear()
        txtPurTouch_PER.Clear()
        txtStyleCode.Clear()
        txtCurrentStyleNo_NUM.Clear()
        txtGrsWt_WET.Clear()
        txtFromWt_WET.Clear()
        txtToWt_WET.Clear()
        txtNetWt_PER.Clear()
        txtgp_NUM.Clear()
        txtSaleTouch_PER.Clear()
        txtGiftValue_AMT.Clear()
        txtItemPctPath.Clear()
        If GlobalVariables.PIC_ITEMWISE Then
            AutoImageSizer("", picItemImage, PictureBoxSizeMode.CenterImage)
            PctFile = Nothing
        End If
        lblItemPctPath.Visible = Itemmast_PctPath
        txtItemPctPath.Visible = Itemmast_PctPath
        txtItemPctPath.Text = ""
        Return 0
    End Function
    Function funcLoadCombo(ByRef combo As ComboBox, ByVal str As String) As ComboBox
        Dim dt As New DataTable
        dt.Clear()
        combo.Items.Clear()
        da = New OleDbDataAdapter(str, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            Dim cnt As Integer
            For cnt = 0 To dt.Rows.Count - 1
                combo.Items.Add(dt.Rows(cnt).Item(0))
            Next
            combo.Text = dt.Rows(0).Item(0)
        End If
        Return combo
    End Function
    Function funcLoadMetalName()
        strSql = " select MetalName from " & cnAdminDb & "..MetalMast "
        strSql += " where isnull(active,'') <> 'N' order by displayorder,MetalName"
        funcLoadCombo(cmbMetalName_Man, strSql)
        Return 0
    End Function
    Function funcLoadCatName()
        cmbCategoryName_Man.Text = ""
        strSql = " select CATNAME from " & cnAdminDb & "..Category where "
        strSql += " MetalId = (select MetalId from " & cnAdminDb & "..MetalMast where MetalName = '" & cmbMetalName_Man.Text & "')"
        'strSql += " and CatGroup = 'P' "
        strSql += " ORDER BY CATNAME"
        funcLoadCombo(cmbCategoryName_Man, strSql)
        Return 0
    End Function
    Function funcSave() As Integer
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Function
        If objGPack.Validator_Check(Me) Then Exit Function
        If flagSave = False Then
            If objGPack.DupChecker(txtItemId_Num_Man, "SELECT 1 FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = '" & txtItemId_Num_Man.Text & "'") Then
                Exit Function
            End If
        End If
        If objGPack.DupChecker(txtItemName__Man, "SELECT 1 FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtItemName__Man.Text & "' AND ITEMID <> '" & txtItemId_Num_Man.Text & "'") Then
            Exit Function
        End If
        If (cmbActive.Text = "NO" Or cmbStockReport.Text = "NO") And flagSave And STKCHK_ITEMEDIT Then
            If funcStockCheck() = True Then
                If cmbActive.Text = "NO" Then
                    MsgBox("Stock Available cannot set Acitve [NO]", MsgBoxStyle.Information)
                    cmbActive.Focus()
                Else
                    MsgBox("Stock Available cannot set StockReport [No]", MsgBoxStyle.Information)
                    cmbStockReport.Focus()
                End If
                Exit Function
            End If
        End If
        If flagSave And STKCHK_ITEMEDIT Then
            If funcStockCheckforNT() = True Then
                MsgBox("Stock Available in NonTag cannot change to Taged Item...", MsgBoxStyle.Information)
                cmbStockType.Focus()
                Exit Function
            End If
        End If
        If flagSave = False Then
            funcAdd()
        ElseIf flagSave = True Then
            funcUpdate()
        End If
    End Function
    Function funcStockCheckforNT() As Boolean
        If cmbStockType.Text = "TAGED" Then
            strSql = "SELECT COUNT(*)CNT "
            strSql += " FROM " & cnAdminDb & "..ITEMNONTAG WHERE ITEMID=" & Val(txtItemId_Num_Man.Text)
            strSql += " AND ISNULL(CANCEL,'')=''"
            If Val(objGPack.GetSqlValue(strSql, "CNT", 0)) > 0 Then
                Return True
            Else
                Return False
            End If
        End If
        Return False
    End Function
    Function funcStockCheck() As Boolean
        If cmbStockType.Text = "TAGED" Or cmbStockType.Text = "NON TAGED" Then
            If cmbStockType.Text = "NON TAGED" Then
                strSql = "SELECT SUM(CASE WHEN RECISS='R' THEN GRSWT ELSE -1*GRSWT END)GRSWT "
                strSql += " FROM " & cnAdminDb & "..ITEMNONTAG WHERE ITEMID=" & Val(txtItemId_Num_Man.Text)
                strSql += " AND ISNULL(CANCEL,'')=''"
                If Val(objGPack.GetSqlValue(strSql, "GRSWT", 0)) <> 0 Then
                    Return True
                Else
                    Return False
                End If
            Else
                strSql = "SELECT SUM(GRSWT)GRSWT "
                strSql += " FROM " & cnAdminDb & "..ITEMTAG WHERE ITEMID=" & Val(txtItemId_Num_Man.Text)
                strSql += " AND ISNULL(ISSDATE,'')='' "
                If Val(objGPack.GetSqlValue(strSql, "GRSWT", 0)) <> 0 Then
                    Return True
                Else
                    Return False
                End If
            End If
        Else
            Return False
        End If
        strSql = ""
        Return False
    End Function

    Function funcLoadItemCounter() As Integer
        cmbItemCounter.Items.Clear()
        cmbItemCounter.Items.Add("")
        strSql = " select ItemCtrName from " & cnAdminDb & "..ItemCounter order by ItemCtrName"
        objGPack.FillCombo(strSql, cmbItemCounter, False)
        cmbItemCounter.Text = ""
    End Function
    Function funcLoadItemGroup() As Integer
        cmbItemGroup.Items.Clear()
        cmbItemGroup.Items.Add("")
        strSql = " SELECT GROUPNAME FROM " & cnAdminDb & "..ITEMGROUPMAST WHERE GROUPTYPE='I' ORDER BY GROUPNAME"
        objGPack.FillCombo(strSql, cmbItemGroup, False)
        cmbItemGroup.Text = ""
    End Function
    Function funcAdd()
        Dim ds As New Data.DataSet
        ds.Clear()
        Dim METALID As String = Nothing
        Dim CatCode As String = Nothing

        ''Find METALID
        strSql = " Select MetalId from " & cnAdminDb & "..MetalMast where"
        strSql += " MetalName = '" & cmbMetalName_Man.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(ds, "MetalId")
        If ds.Tables("MetalId").Rows.Count > 0 Then
            METALID = ds.Tables("MetalId").Rows(0).Item("MetalId")
        End If

        ''Find CatCode
        strSql = " Select CatCode from " & cnAdminDb & "..Category where"
        strSql += " CatName = '" & cmbCategoryName_Man.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(ds, "CatCode")
        If ds.Tables("CatCode").Rows.Count > 0 Then
            CatCode = ds.Tables("CatCode").Rows(0).Item("CatCode")
        End If

        ''Find DefaultCounter
        Dim defaultCounter As String = Nothing
        If cmbItemCounter.Text = "" Then
            defaultCounter = ""
        Else
            strSql = " Select ItemCtrId from " & cnAdminDb & "..ItemCounter where ItemCtrName = '" & cmbItemCounter.Text & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(ds, "ItemCtrId")
            If ds.Tables("ItemCtrId").Rows.Count > 0 Then
                defaultCounter = ds.Tables("ItemCtrId").Rows(0).Item("ItemCtrId")
            Else
                defaultCounter = ""
            End If
        End If
        ''FIND DEFAULTGROUP
        Dim defaultGroup As String = Nothing
        If cmbItemGroup.Text = "" Then
            defaultCounter = ""
        Else
            strSql = " SELECT GROUPID FROM " & cnAdminDb & "..ITEMGROUPMAST WHERE GROUPNAME = '" & cmbItemGroup.Text & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(ds, "GROUPID")
            If ds.Tables("GROUPID").Rows.Count > 0 Then
                defaultGroup = ds.Tables("GROUPID").Rows(0).Item("GROUPID")
            Else
                defaultGroup = ""
            End If
        End If

        ''Find defaultitemtype
        Dim defaultitemtype As String = Nothing
        If cmbItemType.Text = "" Then
            defaultitemtype = ""
        Else
            strSql = " Select TOP 1 ITEMTYPEID from " & cnAdminDb & "..ITEMTYPE where Name = '" & cmbItemType.Text & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(ds, "ITEMTYPEID")
            If ds.Tables("ITEMTYPEID").Rows.Count > 0 Then
                defaultitemtype = ds.Tables("ITEMTYPEID").Rows(0).Item("ITEMTYPEID")
            Else
                defaultitemtype = ""
            End If
        End If

        Dim CompId As String = ""
        strSql = " SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(chkCmbCompany.Text) & ")"
        Dim dtCompId As New DataTable
        cmd = New OleDbCommand(strSql, cn, tran)
        da = New OleDbDataAdapter(cmd)
        da.Fill(dtCompId)
        If dtCompId.Rows.Count > 0 Then
            For cnt As Integer = 0 To dtCompId.Rows.Count - 1
                CompId += dtCompId.Rows(cnt)("COMPANYID").ToString()
                CompId += ","
            Next
        End If
        Dim View4c As String = ""
        If ChkCmb4CView.Visible = True Then
            For Each s As String In ChkCmb4CView.Text.Split(",")
                View4c += Mid(s, 1, 2) + ","
            Next
        End If
        If View4c <> "" Then View4c = Mid(View4c, 1, Len(View4c) - 1)

        Try
            tran = Nothing
            tran = cn.BeginTransaction
            Dim DtItemMast As New DataTable
            DtItemMast = BrighttechPack.GlobalMethods.GetTableStructure(cnAdminDb, "ITEMMAST", cn, tran)
            Dim RoIns As DataRow = DtItemMast.NewRow
            RoIns("CATCODE") = CatCode
            RoIns("ITEMID") = Val(txtItemId_Num_Man.Text)
            RoIns("HSN") = Val(txtHsnCode.Text)
            RoIns("ITEMNAME") = txtItemName__Man.Text
            RoIns("SHORTNAME") = txtShortName.Text
            RoIns("STOCKTYPE") = Mid(cmbStockType.Text, 1, 1)
            RoIns("SUBITEM") = Mid(cmbSubItem.Text, 1, 1)
            RoIns("CALTYPE") = Mid(cmbCalType.Text, 1, 1)
            If pnlStone2.Enabled = True Then
                Select Case cmbValueAddType.Text
                    Case "ITEM"
                        RoIns("VALUEADDEDTYPE") = "I"
                    Case "TABLE"
                        RoIns("VALUEADDEDTYPE") = "T"
                    Case "DESIGNER"
                        RoIns("VALUEADDEDTYPE") = "D"
                    Case "TAG"
                        RoIns("VALUEADDEDTYPE") = "P"
                End Select
                If txtTableCode.Enabled = True Then 'TABLECODE
                    RoIns("TABLECODE") = txtTableCode.Text
                End If
                RoIns("STUDDEDSTONE") = Mid(cmbStuddedStone.Text, 1, 1)
                RoIns("SIZESTOCK") = Mid(cmbStockSize.Text, 1, 1)
            End If
            RoIns("TAXINCLUCIVE") = Mid(cmbTaxInclusive.Text, 1, 1)
            If pnlStone3.Enabled = True Then
                RoIns("RATEGET") = Mid(cmbRateGet.Text, 1, 1)
                RoIns("GROSSNETWTDIFF") = Mid(cmbGrossWtDiff.Text, 1, 1)
                RoIns("OTHCHARGE") = Mid(cmbOtherCharge.Text, 1, 1)
                RoIns("MULTIMETAL") = Mid(cmbMultiMetal.Text, 1, 1)
            End If
            RoIns("DEFAULTCOUNTER") = Val(defaultCounter)
            RoIns("STOCKREPORT") = Mid(cmbStockReport.Text, 1, 1)
            RoIns("ACTIVE") = Mid(cmbActive.Text, 1, 1)
            RoIns("ITEMGROUP") = Val(defaultGroup)
            If pnlStone3.Enabled = True Then
                RoIns("RANGESTOCK") = Mid(cmbRangeStock.Text, 1, 1)
                RoIns("TAGTYPE") = Mid(cmbTagType.Text, 1, 1)
            End If
            RoIns("STARTTAG") = txtStartTagNo.Text
            RoIns("ENDTAGNO") = txtEndTagNo.Text
            RoIns("CURRENTTAGNO") = txtCurrentTagNo.Text
            RoIns("FOCUSPIECE") = Mid(cmbFocusPiece.Text, 1, 1)
            RoIns("NOOFPIECE") = Val(txtNoOfPiece_Num.Text)
            RoIns("TAGPRINTSTYLE") = Val(txtTagPrintStyle_Num.Text)
            RoIns("METALRATE") = IIf(txtMetalRate_Amt.Enabled, Val(txtMetalRate_Amt.Text), 0)
            RoIns("PIECERATE") = IIf(txtPieceRate_Amt.Enabled, Val(txtPieceRate_Amt.Text), 0)
            RoIns("PIECERATE_PUR") = IIf(txtPieceRatePur_Amt.Enabled, Val(txtPieceRatePur_Amt.Text), 0)
            If pnlStone1.Enabled Then
                RoIns("STUDDED") = Mid(cmbStudded.Text, 1, 1)
                RoIns("STONEUNIT") = Mid(cmbStoneUnit.Text, 1, 1)
                RoIns("DIASTONE") = Mid(cmbDiaStone.Text, 1, 1)
                RoIns("BEEDS") = Mid(cmbBeeds.Text, 1, 1)
            End If
            RoIns("USERID") = userId
            RoIns("UPDATED") = Today.Date.ToString("yyyy-MM-dd")
            RoIns("UPTIME") = Date.Now.ToLongTimeString
            RoIns("STYLENO") = Mid(cmbDesignCode.Text, 1, 1)
            RoIns("METALID") = METALID
            RoIns("ASSORTED") = Mid(cmbAssorted.Text, 1, 1)
            RoIns("STYLECODE") = txtStyleCode.Text
            RoIns("CURRENT_STYLENO") = Val(txtCurrentStyleNo_NUM.Text)
            RoIns("PURTOUCH") = Val(txtPurTouch_PER.Text)
            RoIns("GRSWT") = IIf(txtGrsWt_WET.Enabled, Val(txtGrsWt_WET.Text), 0)
            RoIns("FROMWT") = IIf(txtFromWt_WET.Enabled, Val(txtFromWt_WET.Text), 0)
            RoIns("TOWT") = IIf(txtToWt_WET.Enabled, Val(txtToWt_WET.Text), 0)
            RoIns("GIFTVALUE") = Val(txtGiftValue_AMT.Text)
            RoIns("COMPANYID") = CompId
            RoIns("MCCALC") = Mid(cmbMcCalcOn.Text, 1, 1)
            RoIns("VALUECALC") = Mid(cmbWastCalcon.Text, 1, 1)
            RoIns("ALLOWZEROPCS") = Mid(cmbAllowZeroPcs.Text, 1, 1)
            RoIns("FIXEDVA") = Mid(cmbFixedVa.Text, 1, 1)
            RoIns("BOOKSTOCK") = Mid(cmbBookStock.Text, 1, 1)
            RoIns("EXTRAWT") = Mid(cmbExtraWt.Text, 1, 1)
            RoIns("COVERWT") = Mid(cmbCoverWt.Text, 1, 1)
            RoIns("TAGWT") = Mid(cmbTagWt.Text, 1, 1)
            RoIns("TAGVALID") = Mid(cmbTagvalid.Text, 1, 1)
            If cmbTagLock.Text <> "" Then RoIns("TAGLOCK") = Mid(cmbTagLock.Text, 1, 1)
            RoIns("MCASVAPER") = Mid(CmbMcAsVaPer.Text, 1, 1)
            RoIns("MAINTAIN4C") = Mid(cmb4C.Text, 1, 1)
            RoIns("VIEW4C") = View4c
            If GlobalVariables.PIC_ITEMWISE Then
                RoIns("PCTFILE") = PctFile
            End If
            RoIns("ZEROWASTAGE") = Mid(cmbZeroWast.Text, 1, 1)
            RoIns("SALTOUCH") = Val(txtSaleTouch_PER.Text)
            RoIns("NETWTPER") = Val(txtNetWt_PER.Text)
            If cmbStuddedless.Enabled Then RoIns("STUDDEDUCT") = Mid(cmbStuddedless.Text, 1, 1)
            RoIns("SETITEM") = Mid(cmbSetItem.Text, 1, 1)
            RoIns("TAGIMAGE") = Mid(cmbTagImage.Text, 1, 1)
            If Lang.ToLower <> "en" Then
                RoIns("BILLNAME") = txtBillName.Text.Trim
            End If
            RoIns("GPPER") = Val(txtgp_NUM.Text.ToString)
            If cmbCompliments.Enabled Then
                RoIns("COMPLIMENTS") = Mid(cmbCompliments.Text, 1, 1)
            End If
            RoIns("HALLMARK") = Mid(CmbHallMark.Text, 1, 1)
            RoIns("DEFITEMTYPEID") = Val(defaultitemtype.ToString)
            RoIns("DEFAULTEMPID") = Val(txtEmpId_NUM.Text.ToString)
            If Itemmast_PctPath Then
                RoIns("ITEMPCTPATH") = txtItemPctPath.Text.Trim
            End If
            RoIns("ISSERVICE") = Mid(cmbIsService.Text, 1, 1)
            RoIns("TOUCHBASED") = Mid(cmbTouchBased.Text, 1, 1)
            DtItemMast.Rows.Add(RoIns)
            InsertData(SyncMode.Master, DtItemMast, cn, tran)
            If GlobalVariables.PIC_ITEMWISE Then
                If PctFile <> "" Then
                    IO.File.Copy(OpenFileDia.FileName, GlobalVariables.PICPATH & PctFile, True)
                End If
            End If
            'If Lang.ToLower <> "en" Then
            '    strSql = "UPDATE " & cnAdminDb & "..ITEMMAST SET "
            '    strSql += " BILLNAME = N'" & txtBillName.Text & "'"
            '    strSql += " WHERE ITEMID = '" & txtItemId_Num_Man.Text & "'"
            '    cmd = New OleDbCommand(strSql, cn, tran)
            '    cmd.ExecuteNonQuery()
            'End If
            tran.Commit()
            tran = Nothing
            Dim metal As String = cmbMetalName_Man.Text
            Dim cat As String = cmbCategoryName_Man.Text
            funcNew()
            cmbMetalName_Man.Text = metal
            cmbCategoryName_Man.Text = cat
            txtItemName__Man.Select()
        Catch ex As Exception
            If tran IsNot Nothing Then
                If tran.Connection IsNot Nothing Then
                    tran.Rollback()
                End If
            End If

            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
        Return 0
    End Function
    Function funcUpdate()
        Dim ds As New Data.DataSet
        ds.Clear()
        Dim METALID As String = Nothing
        Dim CatCode As String = Nothing
        ''FIND METALID
        strSql = " SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE"
        strSql += vbCrLf + " METALNAME = '" & cmbMetalName_Man.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(ds, "METALID")
        If ds.Tables("METALID").Rows.Count > 0 Then
            METALID = ds.Tables("METALID").Rows(0).Item("METALID")

        End If

        ''FIND CATCODE
        strSql = " SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE"
        strSql += vbCrLf + " CATNAME = '" & cmbCategoryName_Man.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(ds, "CATCODE")
        If ds.Tables("CATCODE").Rows.Count > 0 Then
            CatCode = ds.Tables("CATCODE").Rows(0).Item("CATCODE")
        End If

        ''Find DefaultCounter
        Dim defaultCounter As String = Nothing
        If cmbItemCounter.Text = "" Then
            defaultCounter = ""
        Else
            strSql = " SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & cmbItemCounter.Text & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(ds, "ITEMCTRID")
            If ds.Tables("ITEMCTRID").Rows.Count > 0 Then
                defaultCounter = ds.Tables("ITEMCTRID").Rows(0).Item("ITEMCTRID")
            Else
                defaultCounter = ""
            End If
        End If

        ''FIND DEFAULTGROUP
        Dim defaultGroup As String = Nothing
        If cmbItemGroup.Text = "" And defaultCounter = "" Then
            defaultCounter = ""
        Else
            strSql = " SELECT GROUPID FROM " & cnAdminDb & "..ITEMGROUPMAST WHERE GROUPNAME = '" & cmbItemGroup.Text & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(ds, "GROUPID")
            If ds.Tables("GROUPID").Rows.Count > 0 Then
                defaultGroup = ds.Tables("GROUPID").Rows(0).Item("GROUPID")
            Else
                defaultGroup = ""
            End If
        End If

        ''Find defaultitemtype
        Dim defaultitemtype As String = Nothing
        If cmbItemType.Text = "" Then
            defaultitemtype = ""
        Else
            strSql = " Select TOP 1 ITEMTYPEID from " & cnAdminDb & "..ITEMTYPE where Name = '" & cmbItemType.Text & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(ds, "ITEMTYPEID")
            If ds.Tables("ITEMTYPEID").Rows.Count > 0 Then
                defaultitemtype = ds.Tables("ITEMTYPEID").Rows(0).Item("ITEMTYPEID")
            Else
                defaultitemtype = ""
            End If
        End If

        Dim CompId As String
        strSql = " SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(chkCmbCompany.Text) & ")"
        Dim dtCompId As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCompId)
        If dtCompId.Rows.Count > 0 Then
            For cnt As Integer = 0 To dtCompId.Rows.Count - 1
                CompId += dtCompId.Rows(cnt)("COMPANYID").ToString()
                CompId += ","
                'If cnt <> dtCompId.Rows.Count - 1 Then
                'End If
            Next
        End If
        Dim View4c As String = ""
        If ChkCmb4CView.Visible = True Then
            For Each s As String In ChkCmb4CView.Text.Split(",")
                View4c += Mid(s, 1, 2) + ","
            Next
        End If
        If View4c <> "" Then View4c = Mid(View4c, 1, Len(View4c) - 1)

        strSql = " UPDATE " & cnAdminDb & "..ITEMMAST SET"
        strSql += vbCrLf + " METALID= '" & METALID & "'"
        strSql += vbCrLf + " ,CATCODE = '" & CatCode & "'"
        strSql += vbCrLf + " ,HSN= '" & txtHsnCode.Text & "'"
        strSql += vbCrLf + " ,ITEMNAME= '" & txtItemName__Man.Text & "'"
        strSql += vbCrLf + " ,SHORTNAME='" & txtShortName.Text & "'"
        If Lang.ToLower <> "en" Then
            strSql += vbCrLf + " ,BILLNAME= N'" & txtBillName.Text & "'"
        End If
        strSql += vbCrLf + " ,STOCKTYPE='" & Mid(cmbStockType.Text, 1, 1) & "'"
        strSql += vbCrLf + " ,SUBITEM='" & Mid(cmbSubItem.Text, 1, 1) & "'"
        strSql += vbCrLf + " ,CALTYPE='" & Mid(cmbCalType.Text, 1, 1) & "'"
        If pnlStone2.Enabled = True Then
            Dim valueAdded As String = Nothing
            Select Case cmbValueAddType.Text
                Case "ITEM"
                    valueAdded = "I"
                Case "TABLE"
                    valueAdded = "T"
                Case "DESIGNER"
                    valueAdded = "D"
                Case "TAG"
                    valueAdded = "P"
            End Select
            strSql += vbCrLf + " ,VALUEADDEDTYPE='" & valueAdded & "'"
            If txtTableCode.Enabled = True Then
                strSql += vbCrLf + " ,TABLECODE='" & txtTableCode.Text & "'"
            Else
                strSql += vbCrLf + " ,TABLECODE=''"
            End If
            strSql += vbCrLf + " ,STUDDEDSTONE='" & Mid(cmbStuddedStone.Text, 1, 1) & "'"
            strSql += vbCrLf + " ,SIZESTOCK='" & Mid(cmbStockSize.Text, 1, 1) & "'"
        Else
            strSql += vbCrLf + " ,VALUEADDEDTYPE=''"
            strSql += vbCrLf + " ,TABLECODE=''"
            strSql += vbCrLf + " ,STUDDEDSTONE=''"
            strSql += vbCrLf + " ,SIZESTOCK=''"
        End If
        strSql += vbCrLf + " ,TAXINCLUCIVE='" & Mid(cmbTaxInclusive.Text, 1, 1) & "'"
        If pnlStone3.Enabled = True Then
            strSql += vbCrLf + " ,RATEGET='" & Mid(cmbRateGet.Text, 1, 1) & "'"
            strSql += vbCrLf + " ,GROSSNETWTDIFF='" & Mid(cmbGrossWtDiff.Text, 1, 1) & "'"
            strSql += vbCrLf + " ,OTHCHARGE='" & Mid(cmbOtherCharge.Text, 1, 1) & "'"
            strSql += vbCrLf + " ,MULTIMETAL='" & Mid(cmbMultiMetal.Text, 1, 1) & "'"
        Else
            strSql += vbCrLf + " ,RATEGET=''"
            strSql += vbCrLf + " ,GROSSNETWTDIFF=''"
            strSql += vbCrLf + " ,OTHCHARGE=''"
            strSql += vbCrLf + " ,MULTIMETAL=''"
        End If

        strSql += vbCrLf + " ,DEFAULTCOUNTER='" & defaultCounter & "'"
        strSql += vbCrLf + " ,STOCKREPORT='" & Mid(cmbStockReport.Text, 1, 1) & "'"
        strSql += vbCrLf + " ,ACTIVE='" & Mid(cmbActive.Text, 1, 1) & "'"
        strSql += vbCrLf + " ,ITEMGROUP='" & Val(defaultGroup) & "'"
        If pnlStone3.Enabled = True Then
            strSql += vbCrLf + " ,RANGESTOCK='" & Mid(cmbRangeStock.Text, 1, 1) & "'"
            strSql += vbCrLf + " ,TAGTYPE='" & Mid(cmbTagType.Text, 1, 1) & "'"
        Else
            strSql += vbCrLf + " ,RANGESTOCK=''"
            strSql += vbCrLf + " ,TAGTYPE=''"
        End If
        strSql += vbCrLf + " ,STARTTAG='" & txtStartTagNo.Text & "'"
        strSql += vbCrLf + " ,ENDTAGNO='" & txtEndTagNo.Text & "'"
        strSql += vbCrLf + " ,CURRENTTAGNO='" & txtCurrentTagNo.Text & "'"
        strSql += vbCrLf + " ,FOCUSPIECE='" & Mid(cmbFocusPiece.Text, 1, 1) & "'"
        strSql += vbCrLf + " ,NOOFPIECE='" & txtNoOfPiece_Num.Text & "'"
        strSql += vbCrLf + " ,TAGPRINTSTYLE='" & txtTagPrintStyle_Num.Text & "'"
        strSql += vbCrLf + " ,METALRATE=" & Val(txtMetalRate_Amt.Text) & ""
        strSql += vbCrLf + " ,PIECERATE=" & Val(txtPieceRate_Amt.Text) & ""
        strSql += vbCrLf + " ,PIECERATE_PUR=" & Val(txtPieceRatePur_Amt.Text) & ""
        If pnlStone1.Enabled Then
            strSql += vbCrLf + " ,STUDDED='" & Mid(cmbStudded.Text, 1, 1) & "'"
            strSql += vbCrLf + " ,STONEUNIT='" & Mid(cmbStoneUnit.Text, 1, 1) & "'"
            strSql += vbCrLf + " ,DIASTONE='" & Mid(cmbDiaStone.Text, 1, 1) & "'"
            strSql += vbCrLf + " ,BEEDS='" & Mid(cmbBeeds.Text, 1, 1) & "'"
        Else
            strSql += vbCrLf + " ,STUDDED=''"
            strSql += vbCrLf + " ,STONEUNIT=''"
            strSql += vbCrLf + " ,DIASTONE=''"
            strSql += vbCrLf + " ,BEEDS=''"
        End If
        strSql += vbCrLf + " ,USERID='" & userId & "'"
        strSql += vbCrLf + " ,UPDATED='" & Today.Date.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,UPTIME='" & Date.Now.ToLongTimeString & "'"
        strSql += vbCrLf + " ,STYLENO = '" & Mid(cmbDesignCode.Text, 1, 1) & "'"
        strSql += vbCrLf + " ,ASSORTED = '" & Mid(cmbAssorted.Text, 1, 1) & "'"
        strSql += vbCrLf + " ,PURTOUCH = " & Val(txtPurTouch_PER.Text) & "" 'PURTOUCH
        strSql += vbCrLf + " ,STYLECODE = '" & txtStyleCode.Text & "'" 'STYLECODE
        strSql += vbCrLf + " ,CURRENT_STYLENO = " & Val(txtCurrentStyleNo_NUM.Text) & "" 'CURRENTSTYLENO
        strSql += vbCrLf + " ,GRSWT = " & IIf(txtGrsWt_WET.Enabled, Val(txtGrsWt_WET.Text), 0) & "" 'GRSWT
        strSql += vbCrLf + " ,FROMWT = " & IIf(txtFromWt_WET.Enabled, Val(txtFromWt_WET.Text), 0) & "" 'FROMWT
        strSql += vbCrLf + " ,TOWT = " & IIf(txtToWt_WET.Enabled, Val(txtToWt_WET.Text), 0) & "" 'TOWT
        strSql += vbCrLf + " ,GIFTVALUE = " & Val(txtGiftValue_AMT.Text) & ""
        strSql += vbCrLf + " ,COMPANYID = '" & CompId & "'" ' COMPANYID
        strSql += vbCrLf + " ,MCCALC = '" & Mid(cmbMcCalcOn.Text, 1, 1) & "'" 'MCCALC
        strSql += vbCrLf + " ,VALUECALC = '" & Mid(cmbWastCalcon.Text, 1, 1) & "'" 'MCCALC
        strSql += vbCrLf + " ,ALLOWZEROPCS = '" & Mid(cmbAllowZeroPcs.Text, 1, 1) & "'" 'ALLOWZEROPCS
        strSql += vbCrLf + " ,FIXEDVA = '" & Mid(cmbFixedVa.Text, 1, 1) & "'" 'FIXEDVA
        strSql += vbCrLf + " ,BOOKSTOCK = '" & Mid(cmbBookStock.Text, 1, 1) & "'" ' BOOKSTOCK
        strSql += vbCrLf + " ,EXTRAWT = '" & Mid(cmbExtraWt.Text, 1, 1) & "'" ' EXTRAWT
        strSql += vbCrLf + " ,COVERWT = '" & Mid(cmbCoverWt.Text, 1, 1) & "'" ' COVERWT
        strSql += vbCrLf + " ,TAGWT = '" & Mid(cmbTagWt.Text, 1, 1) & "'" ' TAGWT
        strSql += vbCrLf + " ,TAGVALID = '" & Mid(cmbTagvalid.Text, 1, 1) & "'" ' TAGVALID
        strSql += vbCrLf + " ,TAGLOCK = '" & IIf(cmbTagLock.Text <> "", Mid(cmbTagLock.Text, 1, 1), "") & "'"
        strSql += vbCrLf + " ,MCASVAPER = '" & Mid(CmbMcAsVaPer.Text, 1, 1) & "'" ' MCASVAPER
        strSql += vbCrLf + " ,MAINTAIN4C= '" & Mid(cmb4C.Text, 1, 1) & "'" ' MAINTAIN4C
        strSql += vbCrLf + " ,VIEW4C= '" & View4c & "'" ' View4c
        strSql += vbCrLf + " ,ZEROWASTAGE= '" & Mid(cmbZeroWast.Text, 1, 1) & "'" ' MAINTAIN4C
        strSql += vbCrLf + " ,SALTOUCH= " & Val(txtSaleTouch_PER.Text)
        strSql += vbCrLf + " ,NETWTPER= " & Val(txtNetWt_PER.Text)
        If cmbStuddedless.Enabled Then
            strSql += vbCrLf + " ,STUDDEDUCT='" & Mid(cmbStuddedless.Text, 1, 1) & "'"
        Else
            strSql += vbCrLf + " ,STUDDEDUCT=''"
        End If
        If cmbCompliments.Enabled Then
            strSql += vbCrLf + " ,COMPLIMENTS='" & Mid(cmbCompliments.Text, 1, 1) & "'"
        Else
            strSql += vbCrLf + " ,COMPLIMENTS=''"
        End If
        If GlobalVariables.PIC_ITEMWISE Then
            strSql += vbCrLf + " ,PCTFILE = '" & PctFile & "'" 'PCTFILE
        End If
        strSql += vbCrLf + " ,SETITEM = '" & Mid(cmbSetItem.Text, 1, 1) & "'"
        strSql += vbCrLf + " ,HALLMARK = '" & Mid(CmbHallMark.Text, 1, 1) & "'"
        strSql += vbCrLf + " ,TAGIMAGE = '" & Mid(cmbTagImage.Text, 1, 1) & "'"
        strSql += vbCrLf + " ,GPPER = " & Val(txtgp_NUM.Text)
        strSql += vbCrLf + " ,DEFITEMTYPEID = " & Val(defaultitemtype.ToString)
        strSql += vbCrLf + " ,DEFAULTEMPID = " & Val(txtEmpId_NUM.Text.ToString)
        If Itemmast_PctPath Then
            strSql += vbCrLf + " ,ITEMPCTPATH = '" & txtItemPctPath.Text.ToString & "'"
        Else
            strSql += vbCrLf + " ,ITEMPCTPATH = ''"
        End If
        strSql += vbCrLf + " ,ISSERVICE = '" & Mid(cmbIsService.Text, 1, 1) & "'"
        strSql += vbCrLf + " ,TOUCHBASED = '" & Mid(cmbTouchBased.Text, 1, 1) & "'"
        strSql += vbCrLf + "  WHERE ITEMID = '" & txtItemId_Num_Man.Text & "'"
        Try
            ExecQuery(SyncMode.Master, strSql, cn)
            If GlobalVariables.PIC_ITEMWISE And PctFile <> "" Then
                If OpenFileDia.FileName <> GlobalVariables.PICPATH & PctFile Then
                    IO.File.Copy(OpenFileDia.FileName, GlobalVariables.PICPATH & PctFile, True)
                End If
            End If
            funcNew()
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
        Return 0
    End Function
    Function funcExit()
        Me.Close()
        Return 0
    End Function
    Function funcOpen()
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Function
        tabMain.SelectedTab = tabView
        funcCallGrid()
        cmbOpenMetalName.Focus()
        Return 0
    End Function
    Function funcGetDetails(ByVal tempItemid As String)
        Dim dt As New DataTable
        dt.Clear()
        strSql = " SELECT"
        strSql += " ITEMID,HSN,ITEMNAME,SHORTNAME,BILLNAME,"
        strSql += " (SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = I.METALID)AS METALNAME,"
        strSql += " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY AS C WHERE C.CATCODE = I.CATCODE)AS CATNAME,"
        strSql += " CASE WHEN STOCKTYPE = 'T' THEN 'TAGED' "
        strSql += " WHEN STOCKTYPE = 'N' THEN 'NON TAGED' ELSE 'PACKET BASED' END AS STOCKTYPE,"
        strSql += " CASE WHEN SUBITEM = 'Y' THEN 'YES' ELSE 'NO' END AS SUBITEM,"
        strSql += " CASE WHEN CALTYPE = 'B' THEN 'BOTH' "
        strSql += " WHEN CALTYPE = 'R' THEN 'RATE'"
        strSql += " WHEN CALTYPE = 'F' THEN 'FIXED'"
        strSql += " WHEN CALTYPE = 'D' THEN 'DIRECT'"
        strSql += " WHEN CALTYPE = 'M' THEN 'METAL RATE'"
        strSql += " WHEN CALTYPE = 'P' THEN 'PIECES'"
        strSql += " ELSE 'WEIGHT' END CALTYPE,"
        strSql += " CASE WHEN VALUEADDEDTYPE = 'I' THEN 'ITEM' "
        strSql += " WHEN VALUEADDEDTYPE = 'T' THEN 'TABLE' WHEN VALUEADDEDTYPE = 'D' THEN 'DESIGNER' WHEN VALUEADDEDTYPE = 'P' THEN 'TAG' ELSE '' END VALUEADDEDTYPE,"
        strSql += " TABLECODE,"
        strSql += " CASE WHEN STUDDEDSTONE = 'Y' THEN 'YES' "
        strSql += " WHEN STUDDEDSTONE = 'N' THEN 'NO'"
        strSql += " ELSE '' END STUDDEDSTONE,"
        strSql += " CASE WHEN SIZESTOCK = 'Y' THEN 'YES' "
        strSql += " WHEN SIZESTOCK ='N' THEN 'NO'"
        strSql += " ELSE '' END AS SIZESTOCK,"
        strSql += " CASE WHEN TAXINCLUCIVE = 'Y' THEN 'YES' ELSE 'NO' END AS TAXINCLUCIVE,"

        strSql += " CASE WHEN RATEGET = 'Y' THEN 'YES' "
        strSql += " WHEN RATEGET = 'N' THEN 'NO'"
        strSql += " ELSE '' END AS RATEGET,"
        strSql += " CASE WHEN GROSSNETWTDIFF = 'Y' THEN 'YES' "
        strSql += " WHEN GROSSNETWTDIFF = 'N' THEN 'NO'"
        strSql += " ELSE '' END AS GROSSNETWTDIFF,"
        strSql += " CASE WHEN OTHCHARGE = 'Y' THEN 'YES' "
        strSql += " WHEN OTHCHARGE = 'N' THEN 'NO'"
        strSql += " ELSE '' END AS OTHCHARGE,"
        strSql += " CASE WHEN MULTIMETAL = 'Y' THEN 'YES' "
        strSql += " WHEN MULTIMETAL = 'N' THEN 'NO'"
        strSql += " ELSE '' END AS MULTIMETAL,"

        strSql += " ISNULL((SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE  ITEMCTRID = DEFAULTCOUNTER),'')AS ITEMCOUNTER,"
        strSql += " CASE WHEN STOCKREPORT = 'Y' THEN 'YES' ELSE 'NO' END AS STOCKREPORT,"
        strSql += " CASE WHEN ACTIVE = 'Y' THEN 'YES' ELSE 'NO' END AS ACTIVE,"
        strSql += " ITEMGROUP,"

        strSql += " CASE WHEN RANGESTOCK = 'Y' THEN 'YES' "
        strSql += " WHEN RANGESTOCK = 'N' THEN 'NO'"
        strSql += " ELSE '' END AS RANGESTOCK,"
        strSql += " CASE WHEN TAGTYPE = 'Y' THEN 'YES' "
        strSql += " WHEN TAGTYPE = 'N' THEN 'NO'"
        strSql += " ELSE '' END AS TAGTYPE,"
        strSql += " STARTTAG,ENDTAGNO,CURRENTTAGNO,"
        strSql += " CASE WHEN FOCUSPIECE = 'Y' THEN 'YES' ELSE 'NO' END AS FOCUSPIECE,"
        strSql += " NOOFPIECE,TAGPRINTSTYLE,METALRATE,PIECERATE,PIECERATE_PUR,"
        strSql += " CASE WHEN STUDDED ='S' THEN 'STUDDED'"
        strSql += " WHEN STUDDED = 'L' THEN 'LOOSE' WHEN STUDDED ='B' THEN 'BOTH' ELSE '' END STUDDED,"
        strSql += " CASE WHEN STONEUNIT = 'S' THEN 'STONE'"
        strSql += " WHEN STONEUNIT = 'G' THEN 'GRM' ELSE '' END STONEUNIT,"
        strSql += " CASE WHEN DIASTONE = 'D' THEN 'DIA'"
        strSql += " WHEN DIASTONE ='S' THEN 'STONE'"
        strSql += " WHEN DIASTONE ='P' THEN 'PRECIOUS' ELSE '' END DIASTONE,"
        strSql += " CASE WHEN BEEDS = 'Y' THEN 'YES' ELSE 'NO' END AS BEEDS,"
        strSql += " CASE WHEN STYLENO = 'Y' THEN 'YES' ELSE 'NO' END AS STYLENO"
        strSql += " ,CASE WHEN ISNULL(ASSORTED,'') = 'Y' THEN 'YES' ELSE 'NO' END AS ASSORTED"
        strSql += " ,PURTOUCH,STYLECODE,CURRENT_STYLENO,GRSWT,FROMWT,TOWT,GIFTVALUE,COMPANYID"
        strSql += " ,CASE WHEN ISNULL(MCCALC,'N') = 'G' THEN 'GRS WT' ELSE 'NET WT' END AS MCCALC "
        strSql += " ,CASE WHEN ISNULL(ALLOWZEROPCS,'N') = 'N' THEN 'NO' ELSE 'YES' END AS ALLOWZEROPCS"
        strSql += " ,CASE WHEN ISNULL(FIXEDVA,'N') = 'N' THEN 'NO' ELSE 'YES' END AS FIXEDVA"
        strSql += " ,CASE ISNULL(BOOKSTOCK,'B') WHEN 'B' THEN 'BOTH' WHEN 'N' THEN 'NONE' WHEN 'P' THEN 'PCS' ELSE 'WEIGHT' END AS BOOKSTOCK"
        strSql += " ,CASE WHEN ISNULL(EXTRAWT,'N') = 'N' THEN 'NO' ELSE 'YES' END AS EXTRAWT"
        strSql += " ,CASE WHEN ISNULL(COVERWT,'N') = 'N' THEN 'NO' ELSE 'YES' END AS COVERWT"
        strSql += " ,CASE WHEN ISNULL(TAGWT,'N') = 'N' THEN 'NO' ELSE 'YES' END AS TAGWT"
        strSql += " ,CASE WHEN TAGVALID = 'N' THEN 'NO' ELSE 'YES' END AS TAGVALID"
        strSql += " ,CASE WHEN TAGLOCK = 'N' THEN 'NO' WHEN TAGLOCK ='Y' THEN 'YES' ELSE '' END AS TAGLOCK"
        strSql += " ,CASE WHEN MCASVAPER = 'Y' THEN 'YES' ELSE 'NO' END AS MCASVAPER"
        strSql += " ,CASE WHEN MAINTAIN4C = 'Y' THEN 'YES' ELSE 'NO' END AS MAINTAIN4C"
        strSql += " ,CASE WHEN ZEROWASTAGE= 'Y' THEN 'YES' ELSE 'NO' END AS ZEROWASTAGE"
        strSql += " ,CASE WHEN SETITEM= 'Y' THEN 'YES' ELSE 'NO' END AS SETITEM"
        strSql += " ,CASE WHEN ISNULL(HALLMARK,'Y') = 'Y' THEN 'YES' ELSE 'NO' END AS HALLMARK"
        strSql += " ,CASE WHEN ISNULL(ISSERVICE,'N') = 'Y' THEN 'YES' ELSE 'NO' END AS ISSERVICE"
        strSql += " ,CASE WHEN (ISNULL(TOUCHBASED,'N') = 'Y') THEN 'YES' ELSE 'NO' END AS TOUCHBASED"
        strSql += " ,NETWTPER,SALTOUCH"
        strSql += " ,VIEW4C,CASE WHEN STUDDEDUCT= 'Y' THEN 'YES' WHEN STUDDEDUCT= 'N' THEN 'NO' ELSE '' END AS STUDDEDUCT"
        strSql += " ,PCTFILE,GPPER"
        strSql += " ,CASE WHEN COMPLIMENTS = 'Y' THEN 'YES' ELSE 'NO' END AS COMPLIMENTS"
        strSql += " ,CASE WHEN TAGIMAGE= 'Y' THEN 'YES' ELSE 'NO' END AS TAGIMAGE"
        strSql += " ,ISNULL((SELECT NAME FROM " & cnAdminDb & "..ITEMTYPE WHERE  ITEMTYPEID = I.DEFITEMTYPEID),'') AS ITEMTYPENAME"
        strSql += " ,ISNULL(DEFAULTEMPID,'')DEFAULTEMPID"
        If Itemmast_PctPath Then
            strSql += " ,ISNULL(ITEMPCTPATH,'')ITEMPCTPATH"
        End If
        strSql += " FROM " & cnAdminDb & "..ITEMMAST AS I WHERE ITEMID ='" & tempItemid & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Return 0
        End If
        cmbTagLock.Text = ""
        With dt.Rows(0)
            cmbCompliments.Text = .Item("COMPLIMENTS").ToString
            cmbMetalName_Man.Text = .Item("METALNAME").ToString
            cmbCategoryName_Man.Text = .Item("CATNAME").ToString
            txtItemId_Num_Man.Text = .Item("ITEMID").ToString
            txtHsnCode.Text = .Item("HSN").ToString
            txtBillName.Text = .Item("BILLNAME").ToString
            txtItemName__Man.Text = .Item("ITEMNAME").ToString
            txtShortName.Text = .Item("SHORTNAME").ToString
            cmbStockType.Text = .Item("STOCKTYPE").ToString
            cmbSubItem.Text = .Item("SUBITEM").ToString
            If .Item("CALTYPE").ToString = "DIRECT" Then cmbCalType.Items.Remove("DIRECT") : cmbCalType.Items.Add("DIRECT")
            cmbCalType.Text = .Item("CALTYPE").ToString
            cmbValueAddType.Text = .Item("VALUEADDEDTYPE").ToString
            txtTableCode.Text = .Item("TABLECODE").ToString
            cmbStuddedStone.Text = .Item("STUDDEDSTONE").ToString
            cmbStockSize.Text = .Item("SIZESTOCK").ToString
            cmbTaxInclusive.Text = .Item("TAXINCLUCIVE").ToString
            cmbRateGet.Text = .Item("RATEGET").ToString
            cmbGrossWtDiff.Text = .Item("GROSSNETWTDIFF").ToString
            cmbOtherCharge.Text = .Item("OTHCHARGE").ToString
            cmbMultiMetal.Text = .Item("MULTIMETAL").ToString
            If .Item("ITEMCOUNTER") = "" Then
                cmbItemCounter.Text = ""
            Else
                cmbItemCounter.Text = .Item("ITEMCOUNTER").ToString
            End If

            cmbStockReport.Text = .Item("STOCKREPORT").ToString
            cmbActive.Text = .Item("ACTIVE").ToString
            cmbSetItem.Text = .Item("SETITEM").ToString
            CmbHallMark.Text = .Item("HALLMARK").ToString
            cmbIsService.Text = .Item("ISSERVICE").ToString
            cmbTouchBased.Text = .Item("TOUCHBASED").ToString

            cmbTagImage.Text = .Item("TAGIMAGE").ToString
            cmbItemGroup.Text = ""
            cmbRangeStock.Text = .Item("RANGESTOCK").ToString
            cmbTagType.Text = .Item("TAGTYPE").ToString
            txtStartTagNo.Text = .Item("STARTTAG").ToString
            txtEndTagNo.Text = .Item("ENDTAGNO").ToString
            txtCurrentTagNo.Text = .Item("CURRENTTAGNO").ToString
            cmbFocusPiece.Text = .Item("FOCUSPIECE").ToString
            txtNoOfPiece_Num.Text = .Item("NOOFPIECE").ToString
            txtTagPrintStyle_Num.Text = .Item("TAGPRINTSTYLE").ToString
            txtMetalRate_Amt.Text = .Item("METALRATE").ToString
            txtPieceRate_Amt.Text = .Item("PIECERATE").ToString
            txtPieceRatePur_Amt.Text = .Item("PIECERATE_PUR").ToString
            cmbStudded.Text = .Item("STUDDED").ToString
            cmbStoneUnit.Text = .Item("STONEUNIT").ToString
            cmbDiaStone.Text = .Item("DIASTONE").ToString
            cmbBeeds.Text = .Item("BEEDS").ToString
            cmbDesignCode.Text = .Item("STYLENO").ToString
            cmbAssorted.Text = .Item("ASSORTED").ToString
            txtPurTouch_PER.Text = .Item("PURTOUCH").ToString
            txtStyleCode.Text = .Item("STYLECODE").ToString
            txtCurrentStyleNo_NUM.Text = .Item("CURRENT_STYLENO").ToString
            txtGrsWt_WET.Text = IIf(Val(.Item("GRSWT").ToString) > 0, Format(Val(.Item("GRSWT").ToString), "0.000"), "")
            txtFromWt_WET.Text = IIf(Val(.Item("FROMWT").ToString) > 0, Format(Val(.Item("FROMWT").ToString), "0.000"), "")
            txtToWt_WET.Text = IIf(Val(.Item("TOWT").ToString) > 0, Format(Val(.Item("TOWT").ToString), "0.000"), "")
            txtGiftValue_AMT.Text = IIf(Val(.Item("GIFTVALUE").ToString) > 0, Format(Val(.Item("GIFTVALUE").ToString), "0.00"), "")
            cmbMcCalcOn.Text = .Item("MCCALC").ToString
            cmbAllowZeroPcs.Text = .Item("ALLOWZEROPCS").ToString
            cmbFixedVa.Text = .Item("FIXEDVA").ToString
            cmbBookStock.Text = .Item("BOOKSTOCK").ToString
            cmbExtraWt.Text = .Item("EXTRAWT").ToString
            cmbTagWt.Text = .Item("TAGWT").ToString
            cmbCoverWt.Text = .Item("COVERWT").ToString
            cmbTagvalid.Text = .Item("TAGVALID").ToString
            cmbTagLock.Text = .Item("TAGLOCK").ToString
            CmbMcAsVaPer.Text = .Item("MCASVAPER").ToString

            cmb4C.Text = .Item("MAINTAIN4C").ToString
            If .Item("STUDDED").ToString = "STUDDED" Then cmbStuddedless.Enabled = True Else cmbStuddedless.Enabled = False
            cmbStuddedless.Text = .Item("STUDDEDUCT").ToString

            Dim selCompId As New List(Of String)
            For Each s As String In .Item("COMPANYID").ToString().Split(",")
                selCompId.Add(s.Replace("'", ""))
            Next

            If .Item("COMPANYID").ToString() = "" Then
                chkCmbCompany.SetItemChecked(0, True)
            Else
                For cnt As Integer = 0 To dtCompany.Rows.Count - 1
                    If selCompId.Contains(dtCompany.Rows(cnt).Item("COMPANYID").ToString) Then
                        chkCmbCompany.SetItemChecked(cnt, True)
                    Else
                        chkCmbCompany.SetItemChecked(cnt, False)
                    End If
                Next
            End If

            If .Item("VIEW4C").ToString() = "" Or cmb4C.Text.Trim <> "YES" Then
                'ChkCmb4CView.SetItemChecked(0, True)
                For cnt As Integer = 0 To ChkCmb4CView.Items.Count - 1
                    ChkCmb4CView.SetItemChecked(cnt, False)
                Next
                ChkCmb4CView.Text = ""
            Else
                For Each s As String In .Item("VIEW4C").ToString().Split(",")
                    For cnt As Integer = 0 To ChkCmb4CView.Items.Count - 1
                        If ChkCmb4CView.Items(cnt).ToString.Contains(s) Then
                            ChkCmb4CView.SetItemChecked(cnt, True)
                        End If
                    Next

                Next
            End If

            If GlobalVariables.PIC_ITEMWISE Then
                PctFile = .Item("PCTFILE").ToString
                OpenFileDia.FileName = GlobalVariables.PICPATH & .Item("PCTFILE").ToString
                AutoImageSizer(GlobalVariables.PICPATH & .Item("PCTFILE").ToString, picItemImage, PictureBoxSizeMode.CenterImage)
            End If
            txtNetWt_PER.Text = Val(.Item("NETWTPER").ToString)
            txtSaleTouch_PER.Text = Val(.Item("SALTOUCH").ToString)
            cmbZeroWast.Text = .Item("ZEROWASTAGE").ToString
            txtgp_NUM.Text = Val(.Item("GPPER").ToString)
            cmbItemType.Text = .Item("ITEMTYPENAME").ToString
            txtEmpId_NUM.Text = IIf(Val(.Item("DEFAULTEMPID").ToString) > 0, Val(.Item("DEFAULTEMPID").ToString), "")
            If Itemmast_PctPath Then
                txtItemPctPath.Text = .Item("ITEMPCTPATH").ToString
            Else
                txtItemPctPath.Text = ""
            End If
        End With

        If cmbTagType.Text = "YES" And cmbStockType.Text = "NON TAGED" Then
            cmbItemType.Enabled = True
            txtEmpId_NUM.Enabled = True
        Else
            cmbItemType.Enabled = False
            txtEmpId_NUM.Enabled = False
            cmbItemType.Text = ""
            txtEmpId_NUM.Text = ""
        End If

        Dim mmetalid As String = UCase(objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & cmbCategoryName_Man.Text & "'"))
        If mmetalid = "D" Or mmetalid = "T" Then cmb4C.Enabled = True
        txtItemId_Num_Man.Enabled = False
        flagSave = True
        cmbMetalName_Man.Enabled = False
        Return 0
    End Function
    Function funcDefaultValues()
        cmbDesignCode.Text = "NO"
        cmbStockType.Text = "TAGED"
        cmbSubItem.Text = "NO"
        cmbCalType.Text = "WEIGHT"
        cmbValueAddType.Text = "ITEM"
        cmbStuddedStone.Text = "NO"
        cmbStockSize.Text = "NO"
        '''''''''''''''''
        cmbTaxInclusive.Text = "NO"
        cmbRateGet.Text = "NO"
        cmbGrossWtDiff.Text = "YES"
        cmbOtherCharge.Text = "NO"
        cmbMultiMetal.Text = "NO"
        cmbStockReport.Text = "YES"
        cmbActive.Text = "YES"
        cmbRangeStock.Text = "NO"
        cmbTagType.Text = "NO"
        cmbTagLock.Text = ""
        cmbFocusPiece.Text = "YES"

        ''''Studded
        cmbStudded.Text = "STUDDED"
        cmbStoneUnit.Text = "CARET"
        If UCase(objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & cmbCategoryName_Man.Text & "'")) = "D" Then
            cmbDiaStone.Text = "DIA"
        Else
            cmbDiaStone.Text = "STONE"
        End If
        cmbBeeds.Text = "NO"

        cmbAssorted.Text = "NO"
        cmbAllowZeroPcs.Text = "NO"
        cmbFixedVa.Text = "NO"
        cmbBookStock.Text = "BOTH"
        cmbExtraWt.Text = "NO"
        cmbCoverWt.Text = "NO"
        cmbTagWt.Text = "NO"
        CmbMcAsVaPer.Text = "NO"
        cmb4C.Text = "NO"
        cmbZeroWast.Text = "NO"
        Return 0
    End Function

    Private Sub frmItemMaster_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If tabMain.SelectedTab.Name = tabView.Name Then
                tabMain.SelectedTab = tabGeneral
                cmbMetalName_Man.Focus()
            End If
        ElseIf e.KeyCode = Keys.Enter Then
            If txtItemId_Num_Man.Focused Then
                Exit Sub
            End If
            If txtItemName__Man.Focused Then
                Exit Sub
            End If
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        funcSave()
    End Sub

    Private Sub OpenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click
        funcOpen()
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        funcNew()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        funcExit()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        funcSave()
    End Sub

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click
        funcOpen()
        btnExport.Enabled = False
    End Sub
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        funcNew()
    End Sub
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        funcExit()
    End Sub

    Private Sub cmbMetalName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbMetalName_Man.GotFocus
        cmbMetalName_SelectedIndexChanged(Me, e)
    End Sub
    Private Sub cmbMetalName_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbMetalName_Man.SelectedIndexChanged
        Dim dt As New DataTable
        dt.Clear()
        strSql = " select MetalId from " & cnAdminDb & "..MetalMast where MetalName = '" & cmbMetalName_Man.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Exit Sub
        End If
        If dt.Rows(0).Item("MetalId") = "D" Or dt.Rows(0).Item("MetalId") = "T" Then
            pnlStone1.Enabled = True
            pnlStone2.Enabled = False
            pnlStone3.Enabled = False
            cmb4C.Enabled = True
            cmbCalType.Items.Remove("DIRECT")
            cmbCalType.Items.Add("DIRECT")
        Else
            pnlStone1.Enabled = False
            pnlStone2.Enabled = True
            pnlStone3.Enabled = True
            cmb4C.Enabled = False
            cmbCalType.Items.Remove("DIRECT")
        End If
        funcLoadCatName()
    End Sub

    Private Sub cmbValueAddType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbValueAddType.SelectedIndexChanged, cmbStuddedStone.SelectedIndexChanged
        If cmbValueAddType.Text = "TABLE" Then
            txtTableCode.Enabled = True
        Else
            txtTableCode.Enabled = False
        End If

    End Sub

    Private Sub cmbCalType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCalType.SelectedIndexChanged
        If cmbCalType.Text = "METAL RATE" Or cmbCalType.Text = "WEIGHT" Then
            txtMetalRate_Amt.Enabled = True
        Else
            txtMetalRate_Amt.Enabled = False
        End If
        If cmbCalType.Text = "WEIGHT" Then
            txtGrsWt_WET.Enabled = True
        Else
            txtGrsWt_WET.Enabled = False
        End If
        If cmbCalType.Text = "FIXED" Then
            cmbFixedVa.Visible = True
            lblFixedVa.Visible = True
        Else
            cmbFixedVa.Text = "NO"
            cmbFixedVa.Visible = False
            lblFixedVa.Visible = False
        End If
    End Sub

    Private Sub cmbOpenMetalName_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbOpenMetalName.SelectedIndexChanged
        Dim dt As New DataTable
        dt.Clear()
        cmbOpenCategory.Items.Clear()
        cmbOpenCategory.Items.Add("ALL")
        cmbOpenCategory.Text = "ALL"
        strSql = " SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE "
        strSql += " METALID = (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbOpenMetalName.Text & "')"
        strSql += " ORDER BY CATNAME "
        objGPack.FillCombo(strSql, cmbOpenCategory, False, False)
        cmbOpenMetalName.DropDownStyle = ComboBoxStyle.DropDownList
    End Sub

    Private Sub gridView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.GotFocus
        lblStatus.Visible = True
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            If gridView.RowCount > 0 Then
                gridView.CurrentCell = gridView.CurrentCell
                funcGetDetails(gridView.Item(0, gridView.CurrentRow.Index).Value.ToString)
                tabMain.SelectedTab = tabGeneral
                If cmbMetalName_Man.Enabled = True Then
                    cmbMetalName_Man.Focus()
                Else
                    cmbCategoryName_Man.Focus()
                End If
            End If
        End If
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        funcCallGrid()
        btnExport.Enabled = True
    End Sub


    Private Sub txtItemId_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtItemId_Num_Man.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If objGPack.DupChecker(txtItemId_Num_Man, "SELECT 1 FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = '" & txtItemId_Num_Man.Text & "'") Then
                Exit Sub
            Else
                SendKeys.Send("{TAB}")
            End If
        End If
    End Sub

    Private Sub txtItemName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtItemName__Man.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If objGPack.DupChecker(txtItemName__Man, "SELECT 1 FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtItemName__Man.Text & "' AND ITEMID <> '" & txtItemId_Num_Man.Text & "'") Then
                Exit Sub
            Else
                SendKeys.Send("{TAB}")
            End If
        End If
    End Sub

    Private Sub gridView_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.LostFocus
        lblStatus.Visible = False

    End Sub

    Private Sub gridView_RowEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.RowEnter
        objGPack.TextClear(grpInfo)
        With gridView.Rows(e.RowIndex)
            detStockType.Text = .Cells("STOCKTYPE").Value.ToString
            detSubItem.Text = .Cells("SUBITEM").Value.ToString
            detCalType.Text = .Cells("CALTYPE").Value.ToString
            detValueAddedType.Text = .Cells("VALUEADDEDTYPE").Value.ToString
            detTableCode.Text = .Cells("TABLECODE").Value.ToString
            detStuddedStone.Text = .Cells("STUDDEDSTONE").Value.ToString
            detItemSize.Text = .Cells("SIZESTOCK").Value.ToString
            detTaxInclusive.Text = .Cells("TAXINCLUCIVE").Value.ToString
            detRateGet.Text = .Cells("RATEGET").Value.ToString
            detGrossWtDiff.Text = .Cells("GROSSNETWTDIFF").Value.ToString
            detOtherCharge.Text = .Cells("OTHCHARGE").Value.ToString
            detMultiMetal.Text = .Cells("MULTIMETAL").Value.ToString
            detItemCounter.Text = .Cells("ITEMCOUNTER").Value.ToString
            detStockReport.Text = .Cells("STOCKREPORT").Value.ToString
            detActive.Text = .Cells("ACTIVE").Value.ToString
            detItemGroup.Text = .Cells("ITEMGROUP").Value.ToString
            detRangeStock.Text = .Cells("RANGESTOCK").Value.ToString
            detTagType.Text = .Cells("TAGTYPE").Value.ToString
            detStartTagNo.Text = .Cells("STARTTAG").Value.ToString
            detEndTagNo.Text = .Cells("ENDTAGNO").Value.ToString
            detCurrentTagNo.Text = .Cells("CURRENTTAGNO").Value.ToString
            detFocusPiece.Text = .Cells("FOCUSPIECE").Value.ToString
            detNoOfPiece.Text = .Cells("NOOFPIECE").Value.ToString
            detTagPrintStyle.Text = .Cells("TAGPRINTSTYLE").Value.ToString
            detMetalRate.Text = .Cells("METALRATE").Value.ToString
            detPieceRate.Text = "SA:" & .Cells("PIECERATE").Value.ToString & "| PU:" & .Cells("PIECERATE_PUR").Value.ToString
            detStudded.Text = .Cells("STUDDED").Value.ToString
            detStoneUnit.Text = .Cells("STONEUNIT").Value.ToString
            detDiaStone.Text = .Cells("DIASTONE").Value.ToString
            detBeeds.Text = .Cells("BEEDS").Value.ToString
            detStyleCode.Text = .Cells("STYLECODE").Value.ToString
            detCurrentStyleCode.Text = .Cells("CURRENT_STYLENO").Value.ToString
            detGrsWt.Text = IIf(Val(.Cells("GRSWT").Value.ToString) > 0, Format(Val(.Cells("GRSWT").Value.ToString), "0.000"), "")
            detMcCalcOn.Text = .Cells("MCCALC").Value.ToString
            detFixedVa.Text = .Cells("FIXEDVA").Value.ToString
            detBookStock.Text = .Cells("BOOKSTOCK").Value.ToString
            detItemTypeName.Text = .Cells("ITEMTYPENAME").Value.ToString
            detEmployeeName.Text = .Cells("EMPNAME").Value.ToString
            'DETGPPER.Text = .Cells("GPPER").Value.ToString

        End With
    End Sub

    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
        If Not gridView.RowCount > 0 Then Exit Sub
        Dim list As New List(Of String)
        list.Add("ITEMID")
        list.Add("STNITEMID")
        Dim ItemId As Integer = Val(gridView.CurrentRow.Cells("ITEMID").Value.ToString)
        If DeleteItem(SyncMode.Master, list, "DELETE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = '" & ItemId & "' AND ISNULL(AUTOGENERATOR,'') = ''", ItemId, "ITEMMAST") Then
            funcCallGrid()
            gridView.Focus()
        End If
    End Sub

    Private Sub cmbCategoryName_Man_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCategoryName_Man.SelectedIndexChanged
        If UCase(objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & cmbCategoryName_Man.Text & "'")) = "D" Then
            cmbDiaStone.Text = "DIA"
            cmb4C.Enabled = True
        ElseIf UCase(objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & cmbCategoryName_Man.Text & "'")) = "T" Then
            cmbDiaStone.Text = "STONE"
            cmb4C.Enabled = True
        Else
            cmb4C.Enabled = False
        End If
    End Sub
    Private Sub cmbAssorted_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAssorted.SelectedValueChanged
        If cmbAssorted.Text = "YES" And cmbSubItem.Text = "YES" Then
            MsgBox("SubItem product could not have assorted property", MsgBoxStyle.Information)
            cmbAssorted.Text = "NO"
        End If
    End Sub
    Private Sub frmItemMaster_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        funcLoadCompany()
        chkCmbCompany.Focus()
        picItemImage.Visible = GlobalVariables.PIC_ITEMWISE
        btnImageBrowse.Visible = GlobalVariables.PIC_ITEMWISE
        If Lang.ToLower = "ar" Then
            txtBillName.Font = New Font("Arial", 16, FontStyle.Regular)
        ElseIf Lang.ToLower = "ta" Then
            txtBillName.Font = New Font("TAMMADURAM", 12, FontStyle.Regular)
        End If
        If Lang.ToLower = "en" Then
            txtBillName.Visible = False
            lblBillName.Visible = False
        End If
        lblFromWt.Visible = ITEMMAST_WT
        lblToWt.Visible = ITEMMAST_WT
        txtFromWt_WET.Visible = ITEMMAST_WT
        txtToWt_WET.Visible = ITEMMAST_WT
        lblItemPctPath.Visible = Itemmast_PctPath
        txtItemPctPath.Visible = Itemmast_PctPath
        txtItemPctPath.Text = ""
    End Sub

    Private Sub funcLoadCompany()
        strSql = " SELECT 'ALL' COMPANYNAME,'ALL' COMPANYID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COMPANYNAME,COMPANYID,2 RESULT FROM " & cnAdminDb & "..COMPANY "
        strSql += " ORDER BY RESULT,COMPANYNAME"
        dtCompany = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCompany)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dtCompany, "COMPANYNAME", , "ALL")
    End Sub


    Private Sub cmbStockType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbStockType.SelectedIndexChanged
        If cmbStockType.Text = "NON TAGED" Then
            cmbAllowZeroPcs.Enabled = True
            cmbCompliments.Enabled = True
            txtEmpId_NUM.Enabled = True
            If cmbTagType.Text = "YES" Then
                cmbItemType.Enabled = True
            Else
                cmbItemType.Enabled = False
                cmbItemType.Text = ""
            End If
        Else
            cmbAllowZeroPcs.Text = "NO"
            cmbAllowZeroPcs.Enabled = False
            cmbCompliments.Enabled = False
            cmbCompliments.SelectedItem = 0
            txtEmpId_NUM.Enabled = False
            txtEmpId_NUM.Text = ""
            cmbItemType.Enabled = False
            cmbItemType.Text = ""
        End If
    End Sub

    Private Sub btnImageBrowse_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImageBrowse.Click
        If Not IO.Directory.Exists(GlobalVariables.PICPATH) Then
            MsgBox(GlobalVariables.PICPATH & vbCrLf & " Destination Path not found" & vbCrLf & "Please make the correct path", MsgBoxStyle.Information)
            Exit Sub
        End If
        Try
            Dim str As String
            str = "JPEG(*.jpg)|*.jpg"
            str += "|Bitmap(*.bmp)|*.bmp"
            str += "|GIF(*.gif)|*.gif"
            str += "|All Files(*.*)|*.*"
            OpenFileDia.Filter = str
            If OpenFileDia.ShowDialog = Windows.Forms.DialogResult.OK Then
                Dim Finfo As IO.FileInfo
                Finfo = New IO.FileInfo(OpenFileDia.FileName)
                AutoImageSizer(OpenFileDia.FileName, picItemImage)
                PctFile = "I" & txtItemId_Num_Man.Text & Finfo.Extension
            End If
        Catch ex As Exception
            MsgBox(E0016, MsgBoxStyle.Exclamation)
        End Try
    End Sub

    Private Sub btnImageBrowse_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles btnImageBrowse.KeyDown
        If e.KeyCode = Keys.Escape Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub cmb4C_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmb4C.SelectedIndexChanged
        If cmb4C.Enabled = True And cmb4C.Text = "YES" Then
            ChkCmb4CView.Visible = True
            lbl4Cview.Visible = True
        Else
            ChkCmb4CView.Visible = False
            lbl4Cview.Visible = False
        End If
    End Sub

    Private Sub cmbStudded_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbStudded.SelectedIndexChanged
        If cmbStudded.Enabled = True And cmbStudded.Text = "STUDDED" And studdeductsoft = "I" Then
            cmbStuddedless.Enabled = True
        Else
            cmbStuddedless.Enabled = False
            cmbStuddedless.Text = "NO"
        End If
    End Sub

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Dim comboSelectMetalName As String
        Dim comboSelectCategory As String
        Dim comboSelectMetalName1 As String = cmbOpenMetalName.SelectedItem.ToString()
        Dim comboSelectCategory1 As String = cmbOpenCategory.SelectedItem.ToString()
        If comboSelectMetalName1 = "ALL" And comboSelectCategory1 = "ALL" Then
            comboSelectMetalName = ""
            comboSelectCategory = ""
        ElseIf comboSelectMetalName1 = "ALL" And comboSelectCategory1 <> "ALL" Then
            comboSelectMetalName = ""
            comboSelectCategory = " [" + cmbOpenCategory.SelectedItem.ToString() + "] "
        ElseIf comboSelectMetalName1 <> "ALL" And comboSelectCategory1 = "ALL" Then
            comboSelectMetalName = " [" + cmbOpenMetalName.SelectedItem.ToString() + "] "
            comboSelectCategory = ""
        ElseIf comboSelectMetalName1 <> "ALL" And comboSelectCategory1 <> "ALL" Then
            comboSelectMetalName = " [" + cmbOpenMetalName.SelectedItem.ToString() + "] "
            comboSelectCategory = " [" + cmbOpenCategory.SelectedItem.ToString() + "] "
        End If
        Dim lbltitle1 As String = "ITEM MASTER DETAILS " + comboSelectMetalName + comboSelectCategory
        Dim formattedDate As String = DateTime.Now.ToString("dd/MM/yyyy  hh:mm:ss tt")
        Dim lbltitle = lbltitle1 + formattedDate
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        'Me.Cursor = Cursors.WaitCursor
        If gridView.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lbltitle, gridView, BrightPosting.GExport.GExportType.Export)
        End If
        Me.Cursor = Cursors.Arrow
        btnExport.Enabled = False
    End Sub

    Private Sub cmbOpenCategory_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbOpenCategory.SelectedIndexChanged
        cmbOpenCategory.DropDownStyle = ComboBoxStyle.DropDownList
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        tabMain.SelectedTab = tabGeneral
    End Sub

    'Private Sub txtItemName__Man_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtItemName__Man.Leave
    '    If Lang.ToLower = "ar" Then
    '        txtBillName.Font = New Font("Alhambra", 12, FontStyle.Regular)
    '    ElseIf Lang.ToLower = "ta" Then
    '        'Dim obj As New Translator
    '        'txtBillName.Text =obj.funcTranslate("en", Lang.ToLower, txtItemName__Man.Text)
    '        txtBillName.Font = New Font("TAMMADURAM", 12, FontStyle.Regular)
    '    End If
    '    txtBillName.Text = txtItemName__Man.Text
    'End Sub

    Private Sub txtBillName_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBillName.Leave

        'txtBillName.Text = txtBillName.Text
    End Sub

    Private Sub cmbTagType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbTagType.SelectedIndexChanged
        If cmbTagType.Text = "YES" And cmbStockType.Text = "NON TAGED" Then
            cmbItemType.Enabled = True
            cmbItemType.Text = ""
            txtEmpId_NUM.Text = ""
        Else
            cmbItemType.Enabled = False
            cmbItemType.Text = ""
            txtEmpId_NUM.Text = ""
        End If
    End Sub
End Class