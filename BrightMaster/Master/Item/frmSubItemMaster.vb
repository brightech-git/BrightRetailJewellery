Imports System.Data.OleDb
Public Class frmSubItemMaster
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim flagSave As Boolean = False
    Dim PctFile As String
    Dim OpenFileDia As New OpenFileDialog
    Dim studdeductsoft As String
    Dim Lang As String = GetAdmindbSoftValue("BILLNAME_LANG", "en")
    Dim Pic_SubItem As Boolean = IIf(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'PIC_ITEMWISE'") = "Y", True, False)
    Dim PicPath_SubItem As String = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'PICPATH'")
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        objGPack.TextClear(grpInfo)
        cmbOpenItemName.Items.Clear()
        cmbOpenItemName.Items.Add("ALL")
        cmbOpenItemName.Text = "ALL"
        strSql = " select ItemName from " & cnAdminDb & "..ItemMast where SubItem = 'Y' Order by ItemName"
        objGPack.FillCombo(strSql, cmbOpenItemName, False, False)

        funcGridStyle(gridView)
        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGeneral.Left, Me.tabGeneral.Top, Me.tabGeneral.Width, Me.tabGeneral.Height))

        tabGeneral.BackgroundImage = bakImage
        tabView.BackgroundImage = bakImage
        tabGeneral.BackgroundImageLayout = ImageLayout.Stretch
        tabView.BackgroundImageLayout = ImageLayout.Stretch


        cmbCalType.Items.Add("WEIGHT")
        cmbCalType.Items.Add("RATE")
        cmbCalType.Items.Add("BOTH")
        cmbCalType.Items.Add("FIXED")
        cmbCalType.Items.Add("METALRATE")
        cmbCalType.Items.Add("PIECES")

        cmbStuddedStone.Items.Add("YES")
        cmbStuddedStone.Items.Add("NO")
        cmbStuddedStone.Text = "NO"

        cmbSetItem.Items.Add("YES")
        cmbSetItem.Items.Add("NO")
        cmbSetItem.Text = "NO"

        cmbHallmark.Items.Add("YES")
        cmbHallmark.Items.Add("NO")
        cmbHallmark.Text = "NO"

        cmbTaxInclusive.Items.Add("YES")
        cmbTaxInclusive.Items.Add("NO")

        cmbRateGet.Items.Add("YES")
        cmbRateGet.Items.Add("NO")

        cmbGrossWtDiff.Items.Add("YES")
        cmbGrossWtDiff.Items.Add("NO")

        cmbOtherCharge.Items.Add("YES")
        cmbOtherCharge.Items.Add("NO")

        cmbMcCalcOn.Items.Add("GRS WT")
        cmbMcCalcOn.Items.Add("NET WT")

        cmbWastCalcon.Items.Add("GRS WT")
        cmbWastCalcon.Items.Add("NET WT")

        cmbActive.Items.Add("YES")
        cmbActive.Items.Add("NO")

        cmbStoneUnit.Items.Add("CARAT")
        cmbStoneUnit.Items.Add("GRM")

        cmbBeeds.Items.Add("YES")
        cmbBeeds.Items.Add("NO")

        cmbFixedVa.Items.Add("YES")
        cmbFixedVa.Items.Add("NO")
        cmbFixedVa.Text = "NO"

        CmbMcAsVaPer.Items.Add("YES")
        CmbMcAsVaPer.Items.Add("NO")
        CmbMcAsVaPer.Text = "NO"
        cmb4C.Items.Add("YES")
        cmb4C.Items.Add("NO")
        cmb4C.Text = "NO"

        If objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'MC_ON_WTAMT'", , "W") = "W" Then
            CmbMcAsVaPer.Visible = False
            lblMcVaper.Visible = False
        End If
        ''Loading SubItemGroup
        Dim dt As New DataTable
        cmbGroup.Items.Clear()
        cmbGroup.Items.Add("[NONE]")
        strSql = " SELECT SGROUPNAME FROM " & cnAdminDb & "..SUBITEMGROUP ORDER BY SGROUPNAME"
        objGPack.FillCombo(strSql, cmbGroup, False)
        cmbGroup.Text = "[NONE]"
        studdeductsoft = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'STUDWTDEDUCT'", , "")
        funcNew()
    End Sub
    Function funcCallGrid() As Integer
        strSql = " SELECT "
        strSql += " SUBITEMID,HSN,SUBITEMNAME,"
        strSql += " SHORTNAME,STYLECODE,"
        strSql += " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST AS I WHERE I.ITEMID = S.ITEMID)AS ITEMNAME,"
        strSql += " CASE WHEN CALTYPE = 'B' THEN 'BOTH' "
        strSql += "        WHEN CALTYPE = 'R' THEN 'RATE'"
        strSql += "        WHEN CALTYPE = 'F' THEN 'FIXED'"
        strSql += "        WHEN CALTYPE = 'D' THEN 'DIRECT'"
        strSql += "        WHEN CALTYPE = 'M' THEN 'METALRATE'"
        strSql += "        WHEN CALTYPE = 'P' THEN 'PIECES'"
        strSql += "        ELSE 'WEIGHT' END CALTYPE,"
        strSql += " TABLECODE,"
        strSql += "         CASE WHEN STUDDEDSTONE = 'Y' THEN 'YES'"
        strSql += "         WHEN STUDDEDSTONE = 'N' THEN 'NO' ELSE '' END STUDDEDSTONE,"
        strSql += " (SELECT SGROUPNAME FROM " & cnAdminDb & "..SUBITEMGROUP WHERE SGROUPID = S.SGROUPID)GROUPNAME,"
        strSql += "         CASE WHEN TAXINCLUCIVE = 'Y' THEN 'YES' ELSE 'NO' END TAXINCLUCIVE,"
        strSql += "         CASE WHEN RATEGET = 'Y' THEN 'YES' ELSE 'NO' END RATEGET,"
        strSql += "         CASE WHEN GROSSNETWTDIFF = 'Y' THEN 'YES' ELSE 'NO' END GROSSNETWTDIFF,"
        strSql += "         CASE WHEN OTHCHARGE = 'Y' THEN 'YES' ELSE 'NO' END OTHCHARGE,"
        strSql += "         CASE WHEN ACTIVE = 'Y' THEN 'YES' ELSE 'NO' END ACTIVE,"
        strSql += " ISNULL((SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE  ITEMCTRID = DEFAULTCOUNTER),'')AS ITEMCOUNTER,"
        strSql += " METALRATE,PIECERATE,PIECERATE_PUR,"
        strSql += "         CASE WHEN STONEUNIT = 'C' THEN 'CARAT' "
        strSql += "         WHEN STONEUNIT = 'G' THEN 'GRM' ELSE '' END STONEUNIT,"
        strSql += "         CASE WHEN BEEDS = 'Y' THEN 'YES' ELSE 'NO' END BEEDS"
        strSql += " ,PURTOUCH,GRSWT,GIFTVALUE,DISPLAYORDER"
        strSql += " ,CASE WHEN ISNULL(MCCALC,'N') = 'G' THEN 'GRS WT' ELSE 'NET WT' END AS MCCALC"
        strSql += " ,CASE WHEN ISNULL(VALUECALC,'N') = 'G' THEN 'GRS WT' ELSE 'NET WT' END AS VALUECALC"
        strSql += " ,CASE WHEN ISNULL(FIXEDVA,'N') = 'N' THEN 'NO' ELSE 'YES' END AS FIXEDVA"
        strSql += " ,  CASE WHEN MAINTAIN4C= 'Y' THEN 'YES' ELSE 'NO' END MAINTAIN4C"
        strSql += " , PCTFILE"
        strSql += " FROM " & cnAdminDb & "..SUBITEMMAST AS S"
        If cmbOpenItemName.Text <> "ALL" Then
            strSql += " WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbOpenItemName.Text & "')"
        End If
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        gridView.DataSource = dt
        With gridView
            .Columns("SUBITEMID").HeaderText = "ID"
            .Columns("SUBITEMID").Width = 60
            .Columns("HSN").Width = 50
            .Columns("SUBITEMNAME").Width = 225
            .Columns("SHORTNAME").Width = 120
            .Columns("STYLECODE").Width = 100
            .Columns("ITEMNAME").Width = 250
        End With
        For CNT As Integer = 6 To gridView.ColumnCount - 1
            gridView.Columns(CNT).Visible = False
        Next
    End Function
    Function funcClear() As Integer
        txtHsnCode.Clear()
        txtBillName.Clear()
        txtSubItemId_Num_Man.Clear()
        txtSubItemName__Man.Clear()
        txtShortName.Clear()
        txtTableCode.Clear()
        txtMetalRate_Amt.Clear()
        txtPieceRate_Amt.Clear()
        txtPieceRatePur_Amt.Clear()
        txtPurTouch_PER.Clear()
        txtStyleCode.Clear()
        txtGrsWt_WET.Clear()
        txtGiftValue_AMT.Clear()
        txtDisplayOrder_NUM.Clear()
        txtNoOfPie.Clear()
        txtGp_NUM.Clear()
        If Pic_SubItem Then
            AutoImageSizer("", picItemImage, PictureBoxSizeMode.CenterImage)
            AutoImageSizer("", picImageView, PictureBoxSizeMode.CenterImage)
            PctFile = Nothing
        End If
    End Function
    Function funcNew() As Integer
        funcClear()
        Dim dt As New DataTable
        dt.Clear()
        strSql = "select isnull(max(subitemid),0)+1 as subItemid from " & cnAdminDb & "..subitemMast"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            txtSubItemId_Num_Man.Text = dt.Rows(0).Item("SubItemid")
        End If
        cmbCalType.Text = "WEIGHT"
        cmbStuddedStone.Text = "NO"
        cmbHallmark.Text = "NO"
        cmbBeeds.Text = "NO"
        cmbBookStock.Text = "BOTH"
        funcLoadItemName()
        funcLoadItemCounter()
        flagSave = False
        txtSubItemId_Num_Man.Enabled = True
        cmbGroup.Text = "[NONE]"
        cmbFixedVa.Text = "NO"

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


        cmbItemName_Man.Focus()
    End Function
    Function funcExit() As Integer
        Me.Close()
    End Function
    Function funcSave() As Integer
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Function
        If objGPack.Validator_Check(Me) Then Exit Function
        If flagSave = False Then
            If objGPack.DupChecker(txtSubItemId_Num_Man, "SELECT 1 FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = '" & txtSubItemId_Num_Man.Text & "'") Then
                Exit Function
            End If
        End If
        If objGPack.DupChecker(txtSubItemName__Man, "SELECT 1 FROM " & cnAdminDb & "..SUBITEMMAST WHERE ISNULL(ITEMID,'') = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ITEMNAME,'') = '" & cmbItemName_Man.Text & "') AND SUBITEMNAME = '" & txtSubItemName__Man.Text & "' AND SUBITEMID <> '" & txtSubItemId_Num_Man.Text & "'") Then
            Exit Function
        End If
        If flagSave = False Then
            funcAdd()
        ElseIf flagSave = True Then
            funcUpdate()
        End If
    End Function
    Function funcAdd() As Integer
        Dim itemId As Integer = Nothing
        Dim sgroupId As Integer = Nothing
        Dim dt As New DataTable
        Dim dt1 As New DataTable
        dt1.Clear()
        dt.Clear()
        strSql = " Select ItemId from " & cnAdminDb & "..ItemMast where ItemName = '" & cmbItemName_Man.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            itemId = dt.Rows(0).Item("ItemId")
        End If

        ''Find GroupId
        If cmbGroup.Text = "" Then
            sgroupId = 0
        Else
            strSql = " SELECT SGROUPID FROM " & cnAdminDb & "..SUBITEMGROUP WHERE SGROUPNAME = '" & cmbGroup.Text & "'"
            dt.Clear()
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                sgroupId = Val(dt.Rows(0).Item("SGROUPID").ToString)
            End If

        End If

        ''Find DefaultCounter
        Dim defaultCounter As String = Nothing
        If cmbItemCounter.Text = "" Then
            defaultCounter = ""
        Else
            strSql = " Select ItemCtrId from " & cnAdminDb & "..ItemCounter where ItemCtrName = '" & cmbItemCounter.Text & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                defaultCounter = dt1.Rows(0).Item("ItemCtrId")
            Else
                defaultCounter = ""
            End If
        End If
        Dim View4c As String = ""
        If ChkCmb4CView.Visible = True Then
            For Each s As String In ChkCmb4CView.Text.Split(",")
                View4c += Mid(s, 1, 2) + ","
            Next
        End If
        If View4c <> "" Then View4c = Mid(View4c, 1, Len(View4c) - 1)

        strSql = " INSERT INTO " & cnAdminDb & "..SUBITEMMAST"
        strSql += " ("
        strSql += " ITEMID,HSN,SUBITEMID,SUBITEMNAME,SHORTNAME,"
        strSql += " CALTYPE,TABLECODE,STUDDEDSTONE,TAXINCLUCIVE,"
        strSql += " RATEGET,GROSSNETWTDIFF,OTHCHARGE,DEFAULTCOUNTER,"
        strSql += " ACTIVE,METALRATE,PIECERATE,PIECERATE_PUR,STONEUNIT,BEEDS,"
        strSql += " USERID,UPDATED,UPTIME,SGROUPID,PURTOUCH,STYLECODE,GRSWT,GIFTVALUE"
        strSql += " ,DISPLAYORDER,MCCALC,VALUECALC,FIXEDVA,BOOKSTOCK,MCASVAPER"
        strSql += " ,NOOFPIECE,MAINTAIN4C,VIEW4C,STUDDEDUCT,SETITEM"
        If Pic_SubItem Then
            strSql += ",PCTFILE"
        End If
        If Lang.ToLower <> "en" Then
            strSql += ",BILLNAME"
        End If
        strSql += ",GPPER,HALLMARK"
        strSql += " )VALUES ("
        strSql += " '" & itemId & "'" 'ITEMID
        strSql += " ,'" & txtHsnCode.Text & "'" 'HSN
        strSql += " ,'" & txtSubItemId_Num_Man.Text & "'" 'SUBITEMID
        strSql += " ,'" & txtSubItemName__Man.Text & "'" 'SUBITEMNAME
        strSql += " ,'" & txtShortName.Text & "'" 'SHORTNAME
        strSql += " ,'" & Mid(cmbCalType.Text, 1, 1) & "'" 'CALTYPE
        strSql += " ,'" & txtTableCode.Text & "'" 'TABLECODE
        strSql += " ,'" & Mid(cmbStuddedStone.Text, 1, 1) & "'" 'STUDDEDSTONE
        strSql += " ,'" & Mid(cmbTaxInclusive.Text, 1, 1) & "'" 'TAXINCLUCIVE
        strSql += " ,'" & Mid(cmbRateGet.Text, 1, 1) & "'" 'RATEGET
        strSql += " ,'" & Mid(cmbGrossWtDiff.Text, 1, 1) & "'" 'GROSSNETWTDIFF
        strSql += " ,'" & Mid(cmbOtherCharge.Text, 1, 1) & "'" 'OTHCHARGE
        strSql += " ,'" & defaultCounter & "'" 'DEFAULTCOUNTER
        strSql += " ,'" & Mid(cmbActive.Text, 1, 1) & "'" 'ACTIVE
        strSql += " ," & Val(txtMetalRate_Amt.Text) & "" 'METALRATE
        strSql += " ," & Val(txtPieceRate_Amt.Text) & "" 'PIECERATE
        strSql += " ," & Val(txtPieceRatePur_Amt.Text) & "" 'PIECERATE_PUR
        If cmbStoneUnit.Enabled Then
            strSql += " ,'" & Mid(cmbStoneUnit.Text, 1, 1) & "'" 'STONEUNIT
        Else
            strSql += " ,''" 'STONEUNIT
        End If
        If cmbBeeds.Enabled Then
            strSql += " ,'" & Mid(cmbBeeds.Text, 1, 1) & "'" 'BEEDS
        Else
            strSql += " ,''" 'BEEDS
        End If
        strSql += " ," & userId & "" 'USERID
        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
        strSql += " ," & sgroupId & "" 'SGROUPID
        strSql += " ," & Val(txtPurTouch_PER.Text) & "" 'PURTOUCH
        strSql += " ,'" & txtStyleCode.Text & "'" 'STYLECODE
        strSql += " ," & IIf(txtGrsWt_WET.Enabled, Val(txtGrsWt_WET.Text), 0) & "" 'GRSWT
        strSql += " ," & Val(txtGiftValue_AMT.Text) & "" 'GIFTVALUE
        strSql += " ," & Val(txtDisplayOrder_NUM.Text) & "" 'DISPLAYORDER
        strSql += " ,'" & Mid(cmbMcCalcOn.Text, 1, 1) & "'" 'MCCALC
        strSql += " ,'" & Mid(cmbWastCalcon.Text, 1, 1) & "'" 'MCCALC
        strSql += " ,'" & Mid(cmbFixedVa.Text, 1, 1) & "'" ' FIXED VA
        strSql += " ,'" & Mid(cmbBookStock.Text, 1, 1) & "'" 'BOOKSTOCK
        strSql += " ,'" & Mid(CmbMcAsVaPer.Text, 1, 1) & "'" 'MCASVAPER
        strSql += " ," & Val(txtNoOfPie.Text)
        strSql += " ,'" & Mid(cmb4C.Text, 1, 1) & "'" 'MAINTAIN4C
        strSql += " ,'" & View4c & "'" 'View4c
        If cmbStuddedless.Enabled Then
            strSql += " ,'" & Mid(cmbStuddedless.Text, 1, 1) & "'" 'STUDLESS
        Else
            strSql += " ,''" 'STUDLESS
        End If
        strSql += " ,'" & Mid(cmbSetItem.Text, 1, 1) & "'" 'SETITEM
        If Pic_SubItem Then
            strSql += " ,'" & PctFile & "'" 'PCTFILE
        End If
        If Lang.ToLower <> "en" Then
            strSql += " ,N'" & txtBillName.Text & "'"
        End If
        strSql += " ," & Val(txtGp_NUM.Text.ToString) 'GP PERCENTAGE
        strSql += " ,'" & Mid(cmbHallmark.Text, 1, 1) & "'" 'HALLMARK
        strSql += " )"
        Try
            ExecQuery(SyncMode.Master, strSql, cn)
            If Pic_SubItem Then
                If PctFile <> "" Then
                    IO.File.Copy(OpenFileDia.FileName, PicPath_SubItem & "\" & PctFile, True)
                End If
            End If
            Dim itemname As String = cmbItemName_Man.Text
            'If Lang.ToLower <> "en" Then
            '    strSql = "UPDATE " & cnAdminDb & "..SUBITEMMAST SET "
            '    strSql += " BILLNAME = N'" & txtBillName.Text & "'"
            '    strSql += " WHERE SUBITEMID = '" & txtSubItemId_Num_Man.Text & "'"
            '    cmd = New OleDbCommand(strSql, cn, tran)
            '    cmd.ExecuteNonQuery()
            'End If
            funcNew()
            cmbItemName_Man.Text = itemname
            txtSubItemName__Man.Select()
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function
    Function funcUpdate() As Integer
        Dim itemId As Integer = Nothing
        Dim sGroupId As Integer = Nothing
        Dim dt, dt1 As New DataTable
        dt1.Clear()
        dt.Clear()
        strSql = " Select ItemId from " & cnAdminDb & "..ItemMast where ItemName = '" & cmbItemName_Man.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            itemId = dt.Rows(0).Item("ItemId")
        End If

        ''Find GroupId
        If cmbGroup.Text = "" Then
            sGroupId = 0
        Else
            strSql = " SELECT SGROUPID FROM " & cnAdminDb & "..SUBITEMGROUP WHERE SGROUPNAME = '" & cmbGroup.Text & "'"
            dt.Clear()
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                sGroupId = Val(dt.Rows(0).Item("SGROUPID").ToString)
            End If

        End If

        ''Find DefaultCounter
        Dim defaultCounter As String = Nothing
        If cmbItemCounter.Text = "" Then
            defaultCounter = ""
        Else
            strSql = " Select ItemCtrId from " & cnAdminDb & "..ItemCounter where ItemCtrName = '" & cmbItemCounter.Text & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                defaultCounter = dt1.Rows(0).Item("ItemCtrId")
            Else
                defaultCounter = ""
            End If
        End If
        Dim View4c As String = ""
        If ChkCmb4CView.Visible = True Then
            For Each s As String In ChkCmb4CView.Text.Split(",")
                View4c += Mid(s, 1, 2) + ","
            Next
        End If
        If View4c <> "" Then View4c = Mid(View4c, 1, Len(View4c) - 1)


        strSql = " Update " & cnAdminDb & "..SubItemMast Set"
        strSql += " ItemId='" & itemId & "'"
        If Lang.ToLower <> "en" Then
            strSql += " ,BILLNAME= N'" & txtBillName.Text & "'"
        End If
        strSql += " ,HSN='" & txtHsnCode.Text & "'"
        strSql += " ,SubItemName='" & txtSubItemName__Man.Text & "'"
        strSql += " ,Shortname='" & txtShortName.Text & "'"
        strSql += " ,CalType='" & Mid(cmbCalType.Text, 1, 1) & "'"
        strSql += " ,TABLECODE='" & txtTableCode.Text & "'"
        strSql += " ,StuddedStone='" & Mid(cmbStuddedStone.Text, 1, 1) & "'"
        strSql += " ,TaxInclucive='" & Mid(cmbTaxInclusive.Text, 1, 1) & "'"
        strSql += " ,RateGet='" & Mid(cmbRateGet.Text, 1, 1) & "'"
        strSql += " ,GrossNetWtDiff='" & Mid(cmbGrossWtDiff.Text, 1, 1) & "'"
        strSql += " ,OthCharge='" & Mid(cmbOtherCharge.Text, 1, 1) & "'"
        strSql += " ,DefaultCounter='" & defaultCounter & "'"
        strSql += " ,Active='" & Mid(cmbActive.Text, 1, 1) & "'"
        strSql += " ,MetalRate=" & Val(txtMetalRate_Amt.Text) & ""
        strSql += " ,PieceRate=" & Val(txtPieceRate_Amt.Text) & ""
        strSql += " ,NOOFPIECE=" & Val(txtNoOfPie.Text) & ""
        strSql += " ,PieceRate_PUR=" & Val(txtPieceRatePur_Amt.Text) & ""
        If cmbStoneUnit.Enabled Then
            strSql += " ,STONEUNIT='" & Mid(cmbStoneUnit.Text, 1, 1) & "'"
        Else
            strSql += " ,STONEUNIT=''"
        End If
        If cmbBeeds.Enabled Then
            strSql += " ,Beeds='" & Mid(cmbBeeds.Text, 1, 1) & "'"
        Else
            strSql += " ,Beeds=''"
        End If
        strSql += " ,UserId=" & userId & ""
        strSql += " ,Updated='" & Today.Date.ToString("yyyy-MM-dd") & "'"
        strSql += " ,Uptime='" & Date.Now.ToLongTimeString & "'"
        strSql += " ,SGroupId = " & sGroupId & ""
        strSql += " ,PURTOUCH = " & Val(txtPurTouch_PER.Text) & ""
        strSql += " ,STYLECODE = '" & txtStyleCode.Text & "'"
        strSql += " ,GRSWT = " & IIf(txtGrsWt_WET.Enabled, Val(txtGrsWt_WET.Text), 0) & "" 'GRSWT
        strSql += " ,GIFTVALUE = " & Val(txtGiftValue_AMT.Text) & "" 'GIFTVALUE
        strSql += " ,DISPLAYORDER = " & Val(txtDisplayOrder_NUM.Text) & "" 'DISPLAYORDER
        strSql += " ,MCCALC = '" & Mid(cmbMcCalcOn.Text, 1, 1) & "'" 'MCCALC
        strSql += " ,VALUECALC = '" & Mid(cmbWastCalcon.Text, 1, 1) & "'" 'MCCALC
        strSql += " ,FIXEDVA = '" & Mid(cmbFixedVa.Text, 1, 1) & "'" 'FIXEDVA
        strSql += " ,BOOKSTOCK = '" & Mid(cmbBookStock.Text, 1, 1) & "'" 'BOOKSTOCK
        strSql += " ,MCASVAPER = '" & Mid(CmbMcAsVaPer.Text, 1, 1) & "'" 'MCASVAPER
        strSql += " ,MAINTAIN4C = '" & Mid(cmb4C.Text, 1, 1) & "'" 'MAINTAIN4C
        strSql += " ,VIEW4C = '" & View4c & "'" 'View4c
        If cmbStuddedless.Enabled Then
            strSql += " ,STUDDEDUCT = '" & Mid(cmbStuddedless.Text, 1, 1) & "'" 'STUDLESS
        Else
            strSql += " ,STUDDEDUCT = ''" 'STUDLESS
        End If
        strSql += " ,SETITEM= '" & Mid(cmbSetItem.Text, 1, 1) & "'" 'SETITEM
        strSql += " ,HALLMARK= '" & Mid(cmbHallmark.Text, 1, 1) & "'" 'HALLMARK
        If Pic_SubItem Then
            strSql += " ,PCTFILE = '" & PctFile & "'" 'PCTFILE
        End If
        strSql += " ,GPPER = " & Val(txtGp_NUM.Text.ToString) & "" 'GP PERCENTAGE
        strSql += " WHERE SUBITEMID = '" & txtSubItemId_Num_Man.Text & "'"
        Try
            ExecQuery(SyncMode.Master, strSql, cn)
            If Pic_SubItem And PctFile <> "" Then
                If OpenFileDia.FileName <> PicPath_SubItem & PctFile Then
                    IO.File.Copy(OpenFileDia.FileName, PicPath_SubItem & PctFile, True)
                End If
            End If
            funcNew()
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function
    Function funcOpen() As Integer
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Function
        tabMain.SelectedTab = tabView
        funcCallGrid()
        cmbOpenItemName.Focus()
    End Function
    Function funcLoadItemName() As Integer
        Dim dt As New DataTable
        dt.Clear()
        cmbItemName_Man.Items.Clear()
        strSql = " select ItemName from " & cnAdminDb & "..ItemMast where SubItem = 'Y' Order by ItemName"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Return 0
        End If
        Dim cnt As Integer
        For cnt = 0 To dt.Rows.Count - 1
            cmbItemName_Man.Items.Add(dt.Rows(cnt).Item("ItemName"))
        Next
        cmbItemName_Man.Text = dt.Rows(0).Item("ItemName")
    End Function
    Function funcGetDetails(ByVal tempSubItemId As Integer) As Integer
        Dim dt As New DataTable
        dt.Clear()
        strSql = " SELECT "
        strSql += " SUBITEMID,HSN,SUBITEMNAME,"
        strSql += " SHORTNAME,BILLNAME,"
        strSql += " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST AS I WHERE I.ITEMID = S.ITEMID)AS ITEMNAME,"
        strSql += " (SELECT METALID FROM " & cnAdminDb & "..ITEMMAST AS I WHERE I.ITEMID = S.ITEMID)AS METALID,"
        strSql += " CASE WHEN CALTYPE = 'B' THEN 'BOTH' "
        strSql += "        WHEN CALTYPE = 'R' THEN 'RATE'"
        strSql += "        WHEN CALTYPE = 'F' THEN 'FIXED'"
        strSql += "        WHEN CALTYPE = 'D' THEN 'DIRECT'"
        strSql += "        WHEN CALTYPE = 'M' THEN 'METALRATE'"
        strSql += "        WHEN CALTYPE = 'P' THEN 'PIECES'"
        strSql += "        ELSE 'WEIGHT' END CALTYPE,"
        strSql += " TABLECODE,"
        strSql += "         CASE WHEN STUDDEDSTONE = 'Y' THEN 'YES'"
        strSql += "         WHEN STUDDEDSTONE = 'N' THEN 'NO' ELSE '' END STUDDEDSTONE,"
        strSql += " (SELECT SGROUPNAME FROM " & cnAdminDb & "..SUBITEMGROUP WHERE SGROUPID = S.SGROUPID)AS GROUPNAME,"
        strSql += "         CASE WHEN TAXINCLUCIVE = 'Y' THEN 'YES' ELSE 'NO' END TAXINCLUCIVE,"
        strSql += "         CASE WHEN RATEGET = 'Y' THEN 'YES' ELSE 'NO' END RATEGET,"
        strSql += "         CASE WHEN GROSSNETWTDIFF = 'Y' THEN 'YES' ELSE 'NO' END GROSSNETWTDIFF,"
        strSql += "         CASE WHEN OTHCHARGE = 'Y' THEN 'YES' ELSE 'NO' END OTHCHARGE,"
        strSql += "         CASE WHEN ACTIVE = 'Y' THEN 'YES' ELSE 'NO' END ACTIVE,"
        strSql += " ISNULL((SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE  ITEMCTRID = DEFAULTCOUNTER),'')AS ITEMCOUNTER,"
        strSql += " METALRATE,PIECERATE,PIECERATE_PUR,"
        strSql += "         CASE WHEN STONEUNIT = 'C' THEN 'CARAT' "
        strSql += "         WHEN STONEUNIT = 'G' THEN 'GRM' ELSE '' END STONEUNIT,"
        strSql += "         CASE WHEN BEEDS = 'Y' THEN 'YES' ELSE 'NO' END BEEDS"
        strSql += " ,PURTOUCH,STYLECODE,GRSWT,GIFTVALUE,DISPLAYORDER"
        strSql += " ,CASE WHEN ISNULL(MCCALC,'N') = 'G' THEN 'GRS WT' ELSE 'NET WT' END AS MCCALC "
        strSql += " ,CASE WHEN ISNULL(VALUECALC,'N') = 'G' THEN 'GRS WT' ELSE 'NET WT' END AS VALUECALC "
        strSql += " ,CASE WHEN ISNULL(FIXEDVA,'N') = 'N' THEN 'NO' ELSE 'YES' END AS FIXEDVA "
        strSql += " ,CASE ISNULL(BOOKSTOCK,'B') WHEN 'B' THEN 'BOTH' WHEN 'N' THEN 'NONE' WHEN 'P' THEN 'PCS' ELSE 'WEIGHT' END AS BOOKSTOCK"
        strSql += " ,CASE WHEN ISNULL(MCASVAPER,'N') = 'Y' THEN 'YES' ELSE 'NO' END AS MCASVAPER "
        strSql += " ,CASE WHEN ISNULL(MAINTAIN4C,'N') = 'Y' THEN 'YES' ELSE 'NO' END AS MAINTAIN4C"
        strSql += " ,CASE WHEN ISNULL(SETITEM,'N') = 'Y' THEN 'YES' ELSE 'NO' END AS SETITEM"
        strSql += " ,CASE WHEN ISNULL(HALLMARK,'Y') = 'Y' THEN 'YES' ELSE 'NO' END AS HALLMARK"
        strSql += " ,VIEW4C, CASE WHEN STUDDEDUCT= 'Y' THEN 'YES' WHEN STUDDEDUCT= 'N' THEN 'NO' ELSE '' END STUDDEDUCT,NOOFPIECE,PCTFILE,GPPER"
        strSql += " FROM " & cnAdminDb & "..SUBITEMMAST AS S"
        strSql += " WHERE SUBITEMID = '" & tempSubItemId & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Return 0
        End If
        flagSave = True
        With dt.Rows(0)
            cmbItemName_Man.Text = .Item("ITEMNAME").ToString
            txtSubItemId_Num_Man.Text = .Item("SUBITEMID").ToString
            txtHsnCode.Text = .Item("HSN").ToString
            txtBillName.Text = .Item("BILLNAME").ToString
            txtSubItemName__Man.Text = .Item("SUBITEMNAME").ToString
            txtShortName.Text = .Item("SHORTNAME").ToString
            If .Item("CALTYPE").ToString = "DIRECT" Then cmbCalType.Items.Remove("DIRECT") : cmbCalType.Items.Add("DIRECT")
            cmbCalType.Text = .Item("CALTYPE").ToString
            txtTableCode.Text = .Item("TABLECODE").ToString
            cmbStuddedStone.Text = .Item("STUDDEDSTONE").ToString
            cmbTaxInclusive.Text = .Item("TAXINCLUCIVE").ToString
            cmbRateGet.Text = .Item("RATEGET").ToString
            cmbGrossWtDiff.Text = .Item("GROSSNETWTDIFF").ToString
            cmbOtherCharge.Text = .Item("OTHCHARGE").ToString
            cmbItemCounter.Text = .Item("ITEMCOUNTER").ToString
            cmbActive.Text = .Item("ACTIVE").ToString
            txtMetalRate_Amt.Text = .Item("METALRATE").ToString
            txtPieceRate_Amt.Text = .Item("PIECERATE").ToString
            txtPieceRatePur_Amt.Text = .Item("PIECERATE_PUR").ToString
            cmbStoneUnit.Text = .Item("STONEUNIT").ToString
            cmbBeeds.Text = .Item("BEEDS").ToString
            If .Item("GROUPNAME").ToString = "" Then
                cmbGroup.Text = "[NONE]"
            Else
                cmbGroup.Text = .Item("GROUPNAME").ToString
            End If
            txtPurTouch_PER.Text = .Item("PURTOUCH").ToString
            txtStyleCode.Text = .Item("STYLECODE").ToString
            txtGrsWt_WET.Text = IIf(Val(.Item("GRSWT").ToString) > 0, Format(Val(.Item("GRSWT").ToString), "0.000"), "")
            txtGiftValue_AMT.Text = IIf(Val(.Item("GIFTVALUE").ToString) > 0, Format(Val(.Item("GIFTVALUE").ToString), "0.00"), "")
            txtDisplayOrder_NUM.Text = IIf(Val(.Item("DISPLAYORDER").ToString) > 0, Val(.Item("DISPLAYORDER").ToString), "")
            cmbMcCalcOn.Text = .Item("MCCALC").ToString
            cmbWastCalcon.Text = .Item("VALUECALC").ToString
            cmbFixedVa.Text = .Item("FIXEDVA").ToString
            cmbBookStock.Text = .Item("BOOKSTOCK").ToString
            CmbMcAsVaPer.Text = .Item("MCASVAPER").ToString
            cmb4C.Text = .Item("MAINTAIN4C").ToString
            txtNoOfPie.Text = .Item("NOOFPIECE").ToString
            cmbStuddedless.Text = .Item("STUDDEDUCT").ToString
            cmbSetItem.Text = .Item("SETITEM").ToString
            cmbHallmark.Text = .Item("HALLMARK").ToString
            txtGp_NUM.Text = .Item("GPPER").ToString

            If .Item("VIEW4C").ToString() = "" And cmb4C.Text.Trim <> "YES" Then
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

            If Pic_SubItem Then
                PctFile = PicPath_SubItem & "\" & .Item("PCTFILE").ToString
                OpenFileDia.FileName = PctFile
                AutoImageSizer(PctFile, picItemImage, PictureBoxSizeMode.CenterImage)
            End If
            If .Item("METALID").ToString = "D" Or .Item("METALID").ToString = "T" Then cmb4C.Enabled = True Else cmb4C.Enabled = False
        End With
        txtSubItemId_Num_Man.Enabled = False
    End Function
    Function funcLoadItemCounter() As Integer
        cmbItemCounter.Items.Clear()
        cmbItemCounter.Items.Add("")
        strSql = " select ItemCtrName from " & cnAdminDb & "..ItemCounter order by ItemCtrName"
        objGPack.FillCombo(strSql, cmbItemCounter, False)
        cmbItemCounter.Text = ""
    End Function

    Private Sub frmSubItemMaster_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If tabMain.SelectedTab.Name = tabView.Name Then
                tabMain.SelectedTab = tabGeneral
                cmbItemName_Man.Select()
            End If
        End If
    End Sub

    Private Sub frmSubItemMaster_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtSubItemId_Num_Man.Focused Then
                Exit Sub
            End If
            If gridView.Focused Then Exit Sub
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

    Private Sub cmbItemName_Man_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbItemName_Man.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            Dim mmetalid As String = objGPack.GetSqlValue("select metalid from " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "'")
            If mmetalid = "D" Or mmetalid = "T" Then cmb4C.Enabled = True : cmbCalType.Items.Add("DIRECT") Else cmb4C.Enabled = False : cmbCalType.Items.Remove("DIRECT")
            Dim Studless As String = objGPack.GetSqlValue("SELECT STUDDEDUCT FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "'")
            If Studless = "Y" Then cmbStuddedless.Enabled = True Else cmbStuddedless.Enabled = False
        End If

    End Sub
    Private Sub cmbItemName_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbItemName_Man.SelectedIndexChanged
        If flagSave = True Then
            Exit Sub
        End If
        Dim dt As New DataTable
        dt.Clear()
        strSql = " SELECT METALID,"
        strSql += " CASE WHEN CALTYPE = 'W' THEN 'WEIGHT' "
        strSql += " WHEN CALTYPE = 'R' THEN 'RATE'"
        strSql += " WHEN CALTYPE = 'F' THEN 'FIXED'"
        strSql += " WHEN CALTYPE = 'B' THEN 'BOTH'"
        strSql += " WHEN CALTYPE = 'D' THEN 'DIRECT'"
        strSql += " WHEN CALTYPE = 'P' THEN 'PIECES'"
        strSql += " WHEN CALTYPE = 'M' THEN 'METAL RATE' END CALTYPE,"
        strSql += " STOCKTYPE,"
        strSql += " VALUEADDEDTYPE,"
        strSql += " TABLECODE,"
        strSql += " CASE WHEN STUDDEDSTONE = 'Y' THEN 'YES'"
        strSql += " WHEN STUDDEDSTONE = 'N' THEN 'NO' ELSE '' END STUDDEDSTONE,"
        strSql += " CASE WHEN TAXINCLUCIVE = 'Y' THEN 'YES' ELSE 'NO' END TAXINCLUCIVE,"
        strSql += " CASE WHEN RATEGET = 'Y' THEN 'YES' ELSE 'NO' END RATEGET,"
        strSql += " CASE WHEN GROSSNETWTDIFF = 'Y' THEN 'YES' ELSE 'NO' END GROSSNETWTDIFF,"
        strSql += " CASE WHEN OTHCHARGE = 'Y' THEN 'YES' ELSE 'NO' END OTHCHARGE,"
        strSql += " CASE WHEN ACTIVE = 'Y' THEN 'YES' ELSE 'NO' END ACTIVE,"
        strSql += " METALRATE,PIECERATE,PIECERATE_PUR,"
        strSql += " CASE WHEN STONEUNIT = 'C' THEN 'CARAT' "
        strSql += " WHEN STONEUNIT = 'G' THEN 'GRM' ELSE '' END STONEUNIT,"
        strSql += " CASE WHEN BEEDS = 'Y' THEN 'YES' WHEN BEEDS = 'N' THEN 'NO' ELSE '' END BEEDS"
        strSql += " ,CASE WHEN ISNULL(MCCALC,'N') = 'G' THEN 'GRS WT' ELSE 'NET WT' END AS MCCALC "
        strSql += " ,CASE WHEN ISNULL(FIXEDVA,'N') = 'N' THEN 'NO' ELSE 'YES' END AS FIXEDVA "
        strSql += " ,CASE ISNULL(BOOKSTOCK,'B') WHEN 'B' THEN 'BOTH' WHEN 'N' THEN 'NONE' WHEN 'P' THEN 'PCS' ELSE 'WEIGHT' END AS BOOKSTOCK"
        strSql += " ,CASE WHEN STUDDEDUCT= 'Y' THEN 'YES' WHEN STUDDEDUCT= 'N' THEN 'NO' ELSE '' END AS STUDDEDUCT"
        strSql += " ,ISNULL(SETITEM,'N')SETITEM,ISNULL(HALLMARK,'Y')HALLMARK FROM " & cnAdminDb & "..ITEMMAST WHERE "
        strSql += " ITEMNAME = '" & cmbItemName_Man.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Exit Sub
        End If
        With dt.Rows(0)
            cmbCalType.Text = .Item("CalType").ToString
            If .Item("ValueAddedType") = "T" Then
                txtTableCode.Enabled = True
                txtTableCode.Text = .Item("tablecode").ToString
            Else
                txtTableCode.Enabled = False
                txtTableCode.Clear()
            End If
            If .Item("SETITEM") = "Y" Then
                cmbSetItem.Enabled = True
            Else
                cmbSetItem.Enabled = False
                cmbSetItem.Text = "NO"
            End If
            If .Item("HALLMARK") = "Y" Then
                cmbHallmark.Enabled = True
                cmbHallmark.Text = "YES"
            Else
                cmbHallmark.Enabled = False
                cmbHallmark.Text = "NO"
            End If
            cmbStuddedStone.Text = .Item("StuddedStone").ToString
            cmbTaxInclusive.Text = .Item("TaxInclucive").ToString
            cmbRateGet.Text = .Item("RateGet").ToString
            cmbGrossWtDiff.Text = .Item("GrossNetWtDiff").ToString
            cmbOtherCharge.Text = .Item("OthCharge").ToString
            cmbActive.Text = .Item("Active").ToString
            If .Item("Caltype") = "METAL RATE" Then
                txtMetalRate_Amt.Enabled = True
                txtMetalRate_Amt.Text = .Item("MetalRate").ToString
            Else
                txtMetalRate_Amt.Enabled = False
                txtMetalRate_Amt.Clear()
            End If
            If .Item("Stocktype") = "N" Then
                'txtPieceRate_Amt.Enabled = True
                txtPieceRate_Amt.Text = .Item("PieceRate").ToString
                txtPieceRatePur_Amt.Text = .Item("PieceRate_PUR").ToString
                'Else
                'txtPieceRate_Amt.Enabled = False
                'txtPieceRate_Amt.Clear()
            End If
            If .Item("StoneUnit").ToString = "" Then
                cmbStoneUnit.Enabled = False
            Else
                cmbStoneUnit.Enabled = True
                cmbStoneUnit.Text = .Item("StoneUnit").ToString
                cmbStuddedStone.Text = "NO"
            End If
            If .Item("Beeds").ToString = "" Then
                cmbBeeds.Enabled = False
            Else
                cmbBeeds.Enabled = True
                cmbBeeds.Text = .Item("Beeds").ToString
            End If
            If .Item("STUDDEDUCT").ToString = "YES" And studdeductsoft = "I" Then
                cmbStuddedless.Enabled = True
            Else
                cmbStuddedless.Enabled = False : cmbStuddedless.Text = "NO"
            End If

            cmbMcCalcOn.Text = .Item("MCCALC").ToString
            cmbFixedVa.Text = .Item("FIXEDVA").ToString
            If .Item("METALID").ToString = "D" Or .Item("METALID").ToString = "T" Then cmb4C.Enabled = True : cmbCalType.Items.Add("DIRECT") Else cmb4C.Enabled = False : cmbCalType.Items.Remove("DIRECT")
        End With

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
                'cmbItemName_Man.Focus()
                Me.SelectNextControl(cmbItemName_Man, False, True, True, True)
            End If
        End If
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        funcCallGrid()
        gridView.Select()
        btnExport.Enabled = True
    End Sub


    Private Sub txtSubItemId_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtSubItemId_Num_Man.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If objGPack.DupChecker(txtSubItemId_Num_Man, "SELECT 1 FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = '" & txtSubItemId_Num_Man.Text & "'") Then
                Exit Sub
            Else
                SendKeys.Send("{TAB}")
            End If
        End If
    End Sub

    Private Sub txtSubItemName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtSubItemName__Man.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If objGPack.DupChecker(txtSubItemName__Man, "SELECT 1 FROM " & cnAdminDb & "..SUBITEMMAST WHERE ISNULL(ITEMID,'') = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ITEMNAME,'') = '" & cmbItemName_Man.Text & "') AND SUBITEMNAME = '" & txtSubItemName__Man.Text & "' AND SUBITEMID <> '" & txtSubItemId_Num_Man.Text & "'") Then
                Exit Sub
            End If
        End If
    End Sub

    Private Sub gridView_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.LostFocus
        lblStatus.Visible = False

    End Sub

    Private Sub gridView_RowEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.RowEnter
        objGPack.TextClear(grpInfo)
        With gridView.Rows(e.RowIndex)
            detCalType.Text = .Cells("CALTYPE").Value.ToString
            detTableCode.Text = .Cells("TABLECODE").Value.ToString
            detStuddedStone.Text = .Cells("STUDDEDSTONE").Value.ToString
            detTaxInclusive.Text = .Cells("TAXINCLUCIVE").Value.ToString
            detRateGet.Text = .Cells("RATEGET").Value.ToString
            detGrossWtDiff.Text = .Cells("GROSSNETWTDIFF").Value.ToString
            detOtherCharge.Text = .Cells("OTHCHARGE").Value.ToString
            detDefaultCntr.Text = .Cells("ITEMCOUNTER").Value.ToString
            detActive.Text = .Cells("ACTIVE").Value.ToString
            detMetalRate.Text = .Cells("METALRATE").Value.ToString
            detPieceRate.Text = "SA:" & .Cells("PIECERATE").Value.ToString & "| PU:" & .Cells("PIECERATE_PUR").Value.ToString
            detStoneUnit.Text = .Cells("STONEUNIT").Value.ToString
            detBeeds.Text = .Cells("BEEDS").Value.ToString
            detGroup.Text = .Cells("GROUPNAME").Value.ToString
            detStyleCode.Text = .Cells("STYLECODE").Value.ToString
            detGrsWt.Text = IIf(Val(.Cells("GRSWT").Value.ToString) > 0, Format(Val(.Cells("GRSWT").Value.ToString), "0.000"), "")
            detDisplayOrder.Text = IIf(Val(.Cells("DISPLAYORDER").Value.ToString) > 0, Val(.Cells("DISPLAYORDER").Value.ToString), "")
            detMcCalcOn.Text = .Cells("MCCALC").Value.ToString
            detFixedVa.Text = .Cells("FIXEDVA").Value.ToString
            If Pic_SubItem Then
                PctFile = PicPath_SubItem & "\" & .Cells("PCTFILE").Value.ToString
                OpenFileDia.FileName = PctFile
                AutoImageSizer(PctFile, picImageView, PictureBoxSizeMode.CenterImage)
            End If
        End With
    End Sub

    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
        If Not gridView.RowCount > 0 Then Exit Sub
        Dim list As New List(Of String)
        list.Add("SUBITEMID")
        list.Add("STNSUBITEMID")
        Dim subItemId As Integer = Val(gridView.CurrentRow.Cells("SUBITEMID").Value.ToString)
        If DeleteItem(SyncMode.Master, list, "DELETE FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = '" & subItemId & "'", subItemId, "SUBITEMMAST") Then
            funcCallGrid()
            gridView.Focus()
        End If
    End Sub

    Private Sub cmbCalType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCalType.SelectedIndexChanged
        If cmbCalType.Text = "FIXED" Then
            cmbFixedVa.Visible = True
            lblFixedVa.Visible = True
        Else
            cmbFixedVa.Text = "NO"
            cmbFixedVa.Visible = False
            lblFixedVa.Visible = False
        End If
        If cmbCalType.Text = "METALRATE" Or cmbCalType.Text = "WEIGHT" Then txtMetalRate_Amt.Enabled = True Else txtMetalRate_Amt.Enabled = False
    End Sub

    Private Sub frmSubItemMaster_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        picItemImage.Visible = Pic_SubItem
        btnImageBrowse.Visible = Pic_SubItem
        picImageView.Visible = Pic_SubItem
        If Lang.ToLower = "ar" Then
            txtBillName.Font = New Font("Arial", 16, FontStyle.Regular)
        ElseIf Lang.ToLower = "ta" Then
            txtBillName.Font = New Font("TAMMADURAM", 12, FontStyle.Regular)
        End If
        If Lang.ToLower = "en" Then
            txtBillName.Visible = False
            lblBillName.Visible = False
        End If
    End Sub

    Private Sub btnImageBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImageBrowse.Click
        If Not IO.Directory.Exists(PicPath_SubItem) Then
            MsgBox(PicPath_SubItem & vbCrLf & " Destination Path not found" & vbCrLf & "Please make the correct path", MsgBoxStyle.Information)
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
                PctFile = "I" & objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "'") & "_S" & txtSubItemId_Num_Man.Text & Finfo.Extension
            End If
        Catch ex As Exception
            MsgBox(E0016, MsgBoxStyle.Exclamation)
        End Try
    End Sub

    Private Sub cmb4C_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmb4C.SelectedIndexChanged
        If cmb4C.Enabled = True And cmb4C.Text.Trim = "YES" Then
            ChkCmb4CView.Visible = True
            lbl4Cview.Visible = True
        Else
            ChkCmb4CView.Visible = False
            lbl4Cview.Visible = False
        End If
    End Sub

    Private Sub cmbStuddedStone_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbStuddedStone.SelectedIndexChanged
       
    End Sub
    Private Sub details()
        
        
    End Sub

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Dim comboSelectItemName As String
        Dim comboSelectItemName1 As String = cmbOpenItemName.SelectedItem.ToString()
        If comboSelectItemName1 = "ALL" Then
            comboSelectItemName = ""
        Else
            comboSelectItemName = "[" + comboSelectItemName1 + "] "
        End If

        Dim lbltitle As String = "SUB ITEM MASTER DETAILS " + comboSelectItemName
        Dim formattedDate As String = DateTime.Now.ToString("dd/MM/yyyy  hh:mm:ss tt")
        lbltitle = lbltitle + formattedDate
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        'Me.Cursor = Cursors.WaitCursor
        If gridView.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lbltitle, gridView, BrightPosting.GExport.GExportType.Export)
        End If
        Me.Cursor = Cursors.Arrow
        btnExport.Enabled = False
    End Sub

    Private Sub tabView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tabView.Click

    End Sub

    Private Sub grpInfo_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles grpInfo.Enter

    End Sub

    Private Sub tabGeneral_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tabGeneral.Click

    End Sub

    Private Sub cmbOpenItemName_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbOpenItemName.SelectedIndexChanged
        cmbOpenItemName.DropDownStyle = ComboBoxStyle.DropDownList
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        tabMain.SelectedTab = tabGeneral
    End Sub
End Class