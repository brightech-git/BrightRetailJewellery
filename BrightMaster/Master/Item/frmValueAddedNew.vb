Imports System.Data.OleDb
Imports System.IO
Public Class frmValueAddedNew
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim Dt As New DataTable
    Dim strSql As String
    Dim strpath As String
    Dim flagSave As Boolean = False
    Dim sno As Integer = Nothing ''For Updating purpose
    Dim VAL_ITEMTYPE_TABLE As Boolean = IIf(GetAdmindbSoftValue("VALIDATE_ITEMTYPE_TABLE", "N") = "Y", True, False)
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        funcGridStyle(gridView)
        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGeneral.Left, Me.tabGeneral.Top, Me.tabGeneral.Width, Me.tabGeneral.Height))
        tabGeneral.BackgroundImage = bakImage
        tabView.BackgroundImage = bakImage
        tabGeneral.BackgroundImageLayout = ImageLayout.Stretch
        tabView.BackgroundImageLayout = ImageLayout.Stretch

        cmbType_Man.Items.Add("CUSTOMER")
        cmbType_Man.Items.Add("ITEM")
        cmbType_Man.Items.Add("TABLE")
        cmbType_Man.Items.Add("DESIGNER")
        cmbType_Man.Items.Add("TAG")

        cmbOpenType.Items.Add("CUSTOMER")
        cmbOpenType.Items.Add("ITEM")
        cmbOpenType.Items.Add("TABLE")
        cmbOpenType.Items.Add("DESIGNER")
        cmbOpenType.Items.Add("TAG")
    End Sub
    Function funcNew() As Integer
        objGPack.TextClear(Me)
        If GetAdmindbSoftValue("COSTCENTRE").ToUpper = "Y" Then
            strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            chkLstCostcentre.Items.Clear()
            Dim dt As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            For Each ro As DataRow In dt.Rows
                chkLstCostcentre.Items.Add(ro.Item(0))
            Next
            chkLstCostcentre.Enabled = True
        Else
            chkLstCostcentre.Enabled = False
        End If
        sno = Nothing
        flagSave = False
        grpControls.Enabled = True
        If _IsWholeSaleType Then
            cmbType_Man.Text = "CUSTOMER"
            cmbOpenType.Text = "CUSTOMER"
        Else
            cmbType_Man.Text = "ITEM"
            cmbOpenType.Text = "ITEM"
        End If
        If cmbType_Man.Text = "DESIGNER" Then
            grpMainPurchase.Visible = True
        Else
            grpMainPurchase.Visible = False
        End If
        For cnt As Integer = 0 To chkLstCostcentre.Items.Count - 1
            chkLstCostcentre.SetItemChecked(cnt, False)
            If chkLstCostcentre.Items(cnt).ToString = cnCostName Then
                chkLstCostcentre.SetItemChecked(cnt, True)
                Exit For
            End If
        Next
        If chkLstCostcentre.Enabled Then
            chkLstCostcentre.Select()
        Else
            cmbType_Man.Select()
        End If
        txtDiaFrmPcs_NUM.Text = ""
        txtDiaToPcs_NUM.Text = ""
        txtSalVal_Amt.Text = ""
        txtSalRate_Amt.Text = ""
        ChkDisplayList.Items.Clear()
        ChkDisplayList.Items.Add("FROMWEIGHT")
        ChkDisplayList.Items.Add("TOWEIGHT")
        ChkDisplayList.Items.Add("MAXWASTPER")
        ChkDisplayList.Items.Add("MINWASTPER")
        ChkDisplayList.Items.Add("MAXMCGRM")
        ChkDisplayList.Items.Add("MINMCGRM")
        ChkDisplayList.Items.Add("MAXWAST")
        ChkDisplayList.Items.Add("MINWAST")
        ChkDisplayList.Items.Add("MAXMC")
        ChkDisplayList.Items.Add("MINMC")
        ChkDisplayList.Items.Add("MAXWASTPER_PUR")
        ChkDisplayList.Items.Add("MINWASTPER_PUR")
        ChkDisplayList.Items.Add("MAXMCGRM_PUR")
        ChkDisplayList.Items.Add("MINMCGRM_PUR")
        ChkDisplayList.Items.Add("MAXWAST_PUR")
        ChkDisplayList.Items.Add("MINWAST_PUR")
        ChkDisplayList.Items.Add("MAXMC_PUR")
        ChkDisplayList.Items.Add("MINMC_PUR")
        ChkDisplayList.Items.Add("TOUCH")
        ChkDisplayList.Items.Add("TOUCH_PUR")
        ChkDisplayList.Items.Add("DISCOUNTPER")
        ChkDisplayList.Items.Add("DISCOUNTPER_PUR")
        ChkDisplayList.Items.Add("DIAFROMPCS")
        ChkDisplayList.Items.Add("DIATOPCS")
        ChkDisplayList.Items.Add("SALVALUE")
        Prop_Gets()
    End Function
    Function funcCheckEmpty() As Boolean
        If txtWtFrom_Wet.Text = "" And txtWtTo_Wet.Text = "" And txtMaxWastage_Per.Text = "" And txtMaxMcGrm_Amt.Text = "" And txtMaxFlagWastage_Wet.Text = "" And txtMaxFlatMc_Amt.Text = "" And txtMinWastage_Per.Text = "" And txtMinMcGrm_Amt.Text = "" And txtMinFlagWastage_Wet.Text = "" And txtMinFlatMc_Amt.Text = "" Then
            txtWtFrom_Wet.Focus()
            Return True
        End If
    End Function
    Function funcSave() As Integer
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Function
        If chkLstCostcentre.Enabled Then
            If Not chkLstCostcentre.CheckedItems.Count > 0 Then
                MsgBox("Please Select Costcentre", MsgBoxStyle.Information)
                chkLstCostcentre.Select()
                Exit Function
            End If
        End If
        If objGPack.Validator_Check(Me) Then Exit Function
        If cmbType_Man.Text = "ITEM" Then
            If cmbItemName_Man.Text = "" Then
                MsgBox(Me.GetNextControl(cmbItemName_Man, False).Text + E0001, MsgBoxStyle.Information)
                cmbItemName_Man.Focus()
                Exit Function
            End If
        ElseIf cmbType_Man.Text = "TABLE" Then
            If txtTableCode__Man.Text = "" Then
                MsgBox(Me.GetNextControl(txtTableCode__Man, False).Text + E0001, MsgBoxStyle.Information)
                txtTableCode__Man.Focus()
                Exit Function
            End If
        ElseIf cmbType_Man.Text = "DESIGNER" Then
            If cmbItemName_Man.Text = "" Then
                MsgBox(Me.GetNextControl(cmbItemName_Man, False).Text + E0001, MsgBoxStyle.Information)
                cmbItemName_Man.Focus()
                Exit Function
            End If
            If cmbDesignerName_Man.Text = "" Then
                MsgBox(Me.GetNextControl(cmbDesignerName_Man, False).Text + E0001, MsgBoxStyle.Information)
                cmbDesignerName_Man.Focus()
                Exit Function
            End If
        ElseIf cmbType_Man.Text = "CUSTOMER" Then

        Else
            If cmbItemType_Man.Text = "" Then
                MsgBox(Me.GetNextControl(cmbItemType_Man, False).Text + E0001, MsgBoxStyle.Information)
                cmbItemType_Man.Focus()
                Exit Function
            End If
        End If
        If Not Val(txtWtFrom_Wet.Text) <= Val(txtWtTo_Wet.Text) Then
            MsgBox(E0005 + vbCrLf + E0006 + txtWtTo_Wet.Text, MsgBoxStyle.Information)
            txtWtFrom_Wet.Focus()
            Exit Function
        End If
        If funcCheckEmpty() = True Then Exit Function
        If flagSave = False Then
            funcAdd()
        ElseIf flagSave = True Then
            funcUpdate()
        End If
    End Function

    Function funcAdd() As Integer
        strSql = " Declare @wtFrom float,@wtTo float,@pcsfrom int,@pcsto int"
        strSql += vbCrLf + " Set @wtFrom = " & Val(txtWtFrom_Wet.Text) & ""
        strSql += vbCrLf + " Set @wtTo = " & Val(txtWtTo_Wet.Text) & ""
        If rbtDiamond.Checked Then
            strSql += vbCrLf + " Set @pcsfrom = " & Val(txtDiaFrmPcs_NUM.Text) & ""
            strSql += vbCrLf + " Set @pcsto = " & Val(txtDiaToPcs_NUM.Text) & ""
        End If
        strSql += vbCrLf + " select 1 from " & cnAdminDb & "..WMCtable where "
        strSql += vbCrLf + " ((@wtFrom between fromWeight and ToWeight)"
        strSql += vbCrLf + " or"
        strSql += vbCrLf + " (@wtTo between fromWeight and ToWeight))"
        If rbtDiamond.Checked Then
            strSql += vbCrLf + " and ((@pcsfrom between diafrompcs and diatopcs)"
            strSql += vbCrLf + " or"
            strSql += vbCrLf + " (@pcsto between diafrompcs and diatopcs))"
        End If
        Select Case cmbType_Man.Text
            Case "ITEM"
                strSql += vbCrLf + " AND ITEMID = ISNULL((SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "'),0)"
                strSql += vbCrLf + " AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItemName_Man.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "')),0)"
                strSql += vbCrLf + " AND ITEMTYPE = ISNULL((SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & cmbItemType_Man.Text & "'),0)"
                strSql += vbCrLf + " AND ISNULL(DESIGNERID,0) = 0"
                strSql += vbCrLf + " AND ISNULL(TABLECODE,'') = ''"
                strSql += vbCrLf + " AND ISNULL(ACCODE,'') = ''"
            Case "TABLE"
                strSql += vbCrLf + " AND TABLECODE = '" & txtTableCode__Man.Text & "'"
                strSql += vbCrLf + " AND ISNULL(ACCODE,'') = ''"
                If cmbItemType_Man.Text.ToString <> "ALL" And cmbItemType_Man.Text.ToString <> "" And VAL_ITEMTYPE_TABLE Then
                    strSql += vbCrLf + " AND ITEMTYPE = ISNULL((SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & cmbItemType_Man.Text & "'),0)"
                End If
            Case "DESIGNER"
                strSql += vbCrLf + " AND DESIGNERID = ISNULL((SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & cmbDesignerName_Man.Text & "'),0)"
                strSql += vbCrLf + " AND ITEMID = ISNULL((SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "'),0)"
                strSql += vbCrLf + " AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItemName_Man.Text & "' AND ITEMID = (sELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "')),0)"
                strSql += vbCrLf + " AND ISNULL(ACCODE,'') = ''"
            Case "TAG"
                strSql += vbCrLf + " AND ITEMTYPE = ISNULL((SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & cmbItemType_Man.Text & "'),0)"
                strSql += vbCrLf + " AND ISNULL(ACCODE,'') = ''"
            Case "CUSTOMER"
                strSql += vbCrLf + " AND ITEMID = ISNULL((SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "'),0)"
                strSql += vbCrLf + " AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItemName_Man.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "')),0)"
                strSql += vbCrLf + " AND ISNULL(ACCODE,'') = ISNULL((SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbParty_MAN.Text & "'),'')"
                strSql += vbCrLf + " AND ITEMTYPE = ISNULL((SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & cmbItemType_Man.Text & "'),0)"
        End Select
        Dim costNames As String = GetCheckedItem(chkLstCostcentre)
        If costNames <> Nothing Then strSql += vbCrLf + " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & GetCheckedItem(chkLstCostcentre) & "))"
        If flagSave = True Then
            strSql += vbCrLf + " and VID <> '" & sno & "'"
        End If
        If objGPack.DupCheck(strSql) Then
            MsgBox(E0002, MsgBoxStyle.Information)
            cmbType_Man.Focus()
            Exit Function
        End If

        Try
            Dim VId As Integer = objGPack.GetMax("VID", "WMCTABLE", cnAdminDb)
            Dim CId As String = ""
            tran = Nothing
            tran = cn.BeginTransaction
            If chkLstCostcentre.Enabled Then
                For cnt As Integer = 0 To chkLstCostcentre.CheckedItems.Count - 1
                    CId = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & chkLstCostcentre.CheckedItems.Item(cnt).ToString & "'", , , tran)
                    InsertWmc(CId, VId)
                Next
            Else
                InsertWmc(CId, VId)
            End If
            tran.Commit()
            tran = Nothing
            txtWtFrom_Wet.Clear()
            txtWtTo_Wet.Clear()
            objGPack.TextClear(GroupBox1)
            objGPack.TextClear(GroupBox2)
            objGPack.TextClear(grpMaxPur)
            objGPack.TextClear(grpMinPur)
            txtDiscountPer_Per.Clear()
            txtDiscountPerPur_Per.Clear()
            txtTouch_AMT.Clear()
            txtTouchPur_AMT.Clear()
            txtDiaFrmPcs_NUM.Text = ""
            txtDiaToPcs_NUM.Text = ""
            txtWtFrom_Wet.Focus()
            txtSalRate_Amt.Clear()
        Catch ex As Exception
            If tran IsNot Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Function
    Private Sub InsertWmc(ByVal cId As String, ByVal vId As Integer)
        strSql = " INSERT INTO " & cnAdminDb & "..WMCTABLE"
        strSql += " ("
        strSql += " ITEMID,SUBITEMID,TABLECODE,DESIGNERID,ITEMTYPE,FROMWEIGHT,TOWEIGHT,"
        strSql += " MAXWASTPER,MAXMCGRM,MAXWAST,MAXMC,MINWASTPER,MINMCGRM,MINWAST,MINMC,"
        strSql += " MAXWASTPER_PUR,MAXMCGRM_PUR,MAXWAST_PUR,MAXMC_PUR,MINWASTPER_PUR,MINMCGRM_PUR,"
        strSql += " MINWAST_PUR,MINMC_PUR,DISCOUNTPER,DISCOUNTPER_PUR,"
        strSql += " DIAFROMPCS,DIATOPCS,"
        strSql += " USERID,UPDATED,UPTIME,ACCODE,TOUCH,TOUCH_PUR,VID,COSTID,SALVALUE,RATE"
        strSql += " )VALUES("
        strSql += " " & Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "'", , , tran)) & "" 'ItemId
        strSql += " ," & Val(objGPack.GetSqlValue("SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItemName_Man.Text & "' AND ITEMID = (sELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "')", , , tran)) & "" 'SubItemId
        strSql += " ,'" & txtTableCode__Man.Text & "'" 'TableCode
        strSql += " ," & Val(objGPack.GetSqlValue("SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & cmbDesignerName_Man.Text & "'", , , tran)) & "" 'DesignerId
        strSql += " ," & Val(objGPack.GetSqlValue("SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & cmbItemType_Man.Text & "'", , , tran)) & "" 'ItemType
        strSql += " ," & Val(txtWtFrom_Wet.Text) & "" 'FromWeight
        strSql += " ," & Val(txtWtTo_Wet.Text) & "" 'ToWeight

        strSql += " ," & Val(txtMaxWastage_Per.Text) & "" 'MaxWastPer
        strSql += " ," & Val(txtMaxMcGrm_Amt.Text) & "" 'MaxMcGrm
        strSql += " ," & Val(txtMaxFlagWastage_Wet.Text) & "" 'MaxWast
        strSql += " ," & Val(txtMaxFlatMc_Amt.Text) & "" 'MaxMc
        strSql += " ," & Val(txtMinWastage_Per.Text) & "" 'MinWastPer
        strSql += " ," & Val(txtMinMcGrm_Amt.Text) & "" 'MinMcGrm
        strSql += " ," & Val(txtMinFlagWastage_Wet.Text) & "" 'MinWast
        strSql += " ," & Val(txtMinFlatMc_Amt.Text) & "" 'MinMc

        strSql += " ," & Val(txtMaxWastagePur_Per.Text) & "" 'MaxWastPer_Purchase
        strSql += " ," & Val(txtMaxMcGrmPur_Amt.Text) & "" 'MaxMcGrm_Purchase
        strSql += " ," & Val(txtMaxFlagWastagePur_Wet.Text) & "" 'MaxWast_Purchase
        strSql += " ," & Val(txtMaxFlatMcPur_Amt.Text) & "" 'MaxMc_Purchase
        strSql += " ," & Val(txtMinWastagePur_Per.Text) & "" 'MinWastPer_Purchase
        strSql += " ," & Val(txtMinMcGrmPur_Amt.Text) & "" 'MinMcGrm_Purchase
        strSql += " ," & Val(txtMinFlagWastagePur_Wet.Text) & "" 'MinWast_Purchase
        strSql += " ," & Val(txtMinFlatMcPur_Amt.Text) & "" 'MinMc_Purchase
        strSql += " ," & Val(txtDiscountPer_Per.Text) & "" 'DiscountPer
        strSql += " ," & Val(txtDiscountPerPur_Per.Text) & "" 'DiscountPer
        strSql += " ," & Val(txtDiaFrmPcs_NUM.Text) & "" 'DIAFROMPCS
        strSql += " ," & Val(txtDiaToPcs_NUM.Text) & "" 'DIATOPCS
        strSql += " ,'" & userId & "'" 'UserId
        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'Updated
        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'Uptime
        strSql += " ,'" & objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbParty_MAN.Text & "' AND ISNULL(ACNAME,'') <> ''", , , tran) & "'"
        strSql += " ," & Val(txtTouch_AMT.Text) & ""
        strSql += " ," & Val(txtTouchPur_AMT.Text) & ""
        strSql += " ," & vId & ""
        strSql += " ,'" & cId & "'"
        strSql += " ," & Val(txtSalVal_Amt.Text) & ""
        strSql += " ," & Val(txtSalRate_Amt.Text) & ""
        strSql += ")"
        ExecQuery(SyncMode.Master, strSql, cn, tran)
        ''funcNew()
    End Sub
    Private Function GetCheckedItem(ByVal chklst As CheckedListBox) As String
        Dim _RetStr As String = Nothing
        If chklst.Items.Count > 0 Then
            If chklst.CheckedItems.Count > 0 Then
                For cnt As Integer = 0 To chklst.CheckedItems.Count - 1
                    _RetStr += "'" & chklst.CheckedItems.Item(cnt).ToString & "'"
                    If cnt <> chklst.CheckedItems.Count - 1 Then
                        _RetStr += ","
                    End If
                Next
            End If
        End If
        Return _RetStr
    End Function
    Private Sub funcUpdate(ByVal VId As Integer, ByVal CId As String)
        strSql = " UPDATE " & cnAdminDb & "..WMCTABLE SET"
        strSql += " FROMWEIGHT=" & Val(txtWtFrom_Wet.Text) & ""
        strSql += " ,TOWEIGHT=" & Val(txtWtTo_Wet.Text) & ""
        strSql += " ,MAXWASTPER=" & Val(txtMaxWastage_Per.Text) & ""
        strSql += " ,MAXMCGRM=" & Val(txtMaxMcGrm_Amt.Text) & ""
        strSql += " ,MAXWAST=" & Val(txtMaxFlagWastage_Wet.Text) & ""
        strSql += " ,MAXMC=" & Val(txtMaxFlatMc_Amt.Text) & ""
        strSql += " ,MINWASTPER=" & Val(txtMinWastage_Per.Text) & ""
        strSql += " ,MINMCGRM=" & Val(txtMinMcGrm_Amt.Text) & ""
        strSql += " ,MINWAST=" & Val(txtMinFlagWastage_Wet.Text) & ""
        strSql += " ,MINMC=" & Val(txtMinFlatMc_Amt.Text) & ""

        strSql += " ,MAXWASTPER_PUR=" & Val(txtMaxWastagePur_Per.Text) & ""
        strSql += " ,MAXMCGRM_PUR=" & Val(txtMaxMcGrmPur_Amt.Text) & ""
        strSql += " ,MAXWAST_PUR=" & Val(txtMaxFlagWastagePur_Wet.Text) & ""
        strSql += " ,MAXMC_PUR=" & Val(txtMaxFlatMcPur_Amt.Text) & ""
        strSql += " ,MINWASTPER_PUR=" & Val(txtMinWastagePur_Per.Text) & ""
        strSql += " ,MINMCGRM_PUR=" & Val(txtMinMcGrmPur_Amt.Text) & ""
        strSql += " ,MINWAST_PUR=" & Val(txtMinFlagWastagePur_Wet.Text) & ""
        strSql += " ,MINMC_PUR=" & Val(txtMinFlatMcPur_Amt.Text) & ""

        strSql += " ,DISCOUNTPER=" & Val(txtDiscountPer_Per.Text) & ""
        strSql += " ,DISCOUNTPER_PUR=" & Val(txtDiscountPerPur_Per.Text) & ""
        strSql += " ,DIAFROMPCS=" & Val(txtDiaFrmPcs_NUM.Text) & ""
        strSql += " ,DIATOPCS=" & Val(txtDiaToPcs_NUM.Text) & ""
        strSql += " ,USERID='" & userId & "'"
        strSql += " ,UPDATED='" & Today.Date.ToString("yyyy-MM-dd") & "'"
        strSql += " ,UPTIME='" & Date.Now.ToLongTimeString & "'"
        If txtTouch_AMT.Visible Then
            strSql += " ,TOUCH = " & Val(txtTouch_AMT.Text) & ""
            strSql += " ,TOUCH_PUR = " & Val(txtTouchPur_AMT.Text) & ""
        End If
        strSql += " ,SALVALUE=" & Val(txtSalVal_Amt.Text) & ""
        strSql += " ,RATE=" & Val(txtSalRate_Amt.Text) & ""
        strSql += " WHERE ISNULL(COSTID,'') = '" & CId & "' AND VID = " & VId
        ExecQuery(SyncMode.Master, strSql, cn, tran)
    End Sub
    Function funcUpdate() As Integer
        Dim ItemId As Integer = Nothing
        Dim SubItemId As Integer = Nothing
        Dim TableCode As String = Nothing
        Dim DesignerId As Integer = Nothing
        Dim ItemType As Integer = Nothing
        strSql = " Declare @wtFrom float,@wtTo float"
        strSql += " Set @wtFrom = " & Val(txtWtFrom_Wet.Text) & ""
        strSql += " Set @wtTo = " & Val(txtWtTo_Wet.Text) & ""
        strSql += " select 1 from " & cnAdminDb & "..WMCtable where "
        strSql += " ((@wtFrom between fromWeight and ToWeight)"
        strSql += " or"
        strSql += " (@wtTo between fromWeight and ToWeight))"
        Select Case cmbType_Man.Text
            Case "ITEM"
                strSql += " AND ITEMID = ISNULL((SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "'),0)"
                strSql += " AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItemName_Man.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "')),0)"
                strSql += " AND ITEMTYPE = ISNULL((SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & cmbItemType_Man.Text & "'),0)"
                strSql += " AND ISNULL(DESIGNERID,0) = 0"
                strSql += " AND ISNULL(TABLECODE,'') = ''"
                strSql += " AND ISNULL(ACCODE,'') = ''"
            Case "TABLE"
                strSql += " AND TABLECODE = '" & txtTableCode__Man.Text & "'"
                strSql += " AND ISNULL(ACCODE,'') = ''"
            Case "DESIGNER"
                strSql += " AND DESIGNERID = ISNULL((SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & cmbDesignerName_Man.Text & "'),0)"
                strSql += " AND ITEMID = ISNULL((SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "'),0)"
                strSql += " AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItemName_Man.Text & "' AND ITEMID = (sELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "')),0)"
                strSql += " AND ISNULL(ACCODE,'') = ''"
            Case "TAG"
                strSql += " AND ITEMTYPE = ISNULL((SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & cmbItemType_Man.Text & "'),0)"
                strSql += " AND ISNULL(ACCODE,'') = ''"
            Case "CUSTOMER"
                strSql += " AND ITEMID = ISNULL((SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "'),0)"
                strSql += " AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItemName_Man.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "')),0)"
                strSql += " AND ISNULL(ACCODE,'') = ISNULL((SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbParty_MAN.Text & "'),'')"
                strSql += " AND ITEMTYPE = ISNULL((SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & cmbItemType_Man.Text & "'),0)"
        End Select
        Dim costNames As String = GetCheckedItem(chkLstCostcentre)
        If costNames <> Nothing Then strSql += " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & GetCheckedItem(chkLstCostcentre) & "))"
        If flagSave = True Then
            strSql += " and VID <> '" & sno & "'"
        End If
        If objGPack.DupCheck(strSql) Then
            MsgBox(E0002, MsgBoxStyle.Information)
            cmbType_Man.Focus()
            Exit Function
        End If
        Try
            Dim Cid As String = ""
            tran = Nothing
            tran = cn.BeginTransaction
            If chkLstCostcentre.Items.Count > 0 Then
                For cnt As Integer = 0 To chkLstCostcentre.CheckedItems.Count - 1
                    Cid = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & chkLstCostcentre.CheckedItems.Item(cnt).ToString & "'", , , tran)
                    funcUpdate(sno, Cid)
                Next
            Else
                funcUpdate(sno, Cid)
            End If
            tran.Commit()
            tran = Nothing
            funcNew()
        Catch ex As Exception
            If tran IsNot Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try

    End Function
    Function funcCheckUnique(ByVal field As String, ByVal Value As String) As Boolean
        Dim str As String = Nothing
        Dim dt As New DataTable
        dt.Clear()
        str = " Declare @wtFrom float,@wtTo float"
        str += vbCrLf + "  Set @wtFrom = " & Val(txtWtFrom_Wet.Text) & ""
        str += vbCrLf + "  Set @wtTo = " & Val(txtWtTo_Wet.Text) & ""
        str += vbCrLf + "  select 1 from " & cnAdminDb & "..WMCtable where "
        str += vbCrLf + "  ((@wtFrom between fromWeight and ToWeight)"
        str += vbCrLf + "  or"
        str += vbCrLf + "  (@wtTo between fromWeight and ToWeight))"
        str += vbCrLf + "  and " & field & " = '" & Value & "'"
        If flagSave = True Then
            str += vbCrLf + "  and VID <> '" & sno & "'"
        End If
        da = New OleDbDataAdapter(str, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            Return True ''Already Exist
        End If
        Return False
    End Function
    Function funcCheckUnique(ByVal field1 As String, ByVal Value1 As String, ByVal field2 As String, ByVal Value2 As String) As Boolean
        Dim str As String = Nothing
        Dim dt As New DataTable
        dt.Clear()
        str = " Declare @wtFrom float,@wtTo float"
        str += vbCrLf + "  Set @wtFrom = " & Val(txtWtFrom_Wet.Text) & ""
        str += vbCrLf + "  Set @wtTo = " & Val(txtWtTo_Wet.Text) & ""
        str += vbCrLf + "  select 1 from " & cnAdminDb & "..WMCtable where "
        str += vbCrLf + "  ((@wtFrom between fromWeight and ToWeight)"
        str += vbCrLf + "  or"
        str += vbCrLf + "  (@wtTo between fromWeight and ToWeight))"
        str += vbCrLf + "  and " & field1 & " = '" & Value1 & "'"
        str += vbCrLf + "  and " & field2 & " = '" & Value2 & "'"
        If cmbType_Man.Text <> "CUSTOMER" Then
            str += vbCrLf + "  and isnull(accode,'') = ''"
        Else
            str += vbCrLf + "  AND ACCODE(SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbParty_MAN.Text & "')"
        End If
        If flagSave = True Then
            str += vbCrLf + "  and VID <> '" & sno & "'"
        End If
        da = New OleDbDataAdapter(str, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            Return True ''Already Exist
        End If
        Return False
    End Function
    Function funcCheckUnique(ByVal field1 As String, ByVal Value1 As String, ByVal field2 As String, ByVal Value2 As String, ByVal field3 As String, ByVal Value3 As String) As Boolean
        Dim str As String = Nothing
        Dim dt As New DataTable
        dt.Clear()
        str = " Declare @wtFrom float,@wtTo float"
        str += vbCrLf + "  Set @wtFrom = " & Val(txtWtFrom_Wet.Text) & ""
        str += vbCrLf + "  Set @wtTo = " & Val(txtWtTo_Wet.Text) & ""
        str += vbCrLf + "  select 1 from " & cnAdminDb & "..WMCtable where "
        str += vbCrLf + "  ((@wtFrom between fromWeight and ToWeight)"
        str += vbCrLf + "  or"
        str += vbCrLf + "  (@wtTo between fromWeight and ToWeight))"
        str += vbCrLf + "  and " & field1 & " = '" & Value1 & "'"
        str += vbCrLf + "  and " & field2 & " = '" & Value2 & "'"
        str += vbCrLf + "  and " & field3 & " = '" & Value3 & "'"
        If cmbType_Man.Text <> "CUSTOMER" Then
            str += vbCrLf + "  and isnull(accode,'') = ''"
        Else
            str += vbCrLf + "  AND ACCODE(SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbParty_MAN.Text & "')"
        End If
        If flagSave = True Then
            str += vbCrLf + "  and VID <> '" & sno & "'"
        End If
        da = New OleDbDataAdapter(str, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            Return True ''Already Exist
        End If
        Return False
    End Function

    Function funcExit() As Integer
        Me.Close()
    End Function
    Sub LoadParty(ByVal combo As ComboBox)
        strSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACTYPE in('C','D') ORDER BY ACNAME"
        objGPack.FillCombo(strSql, combo, , False)
    End Sub
    Function funcLoadItemName(ByVal combo As ComboBox, Optional ByVal Allitem As Boolean = False) As Integer
        combo.Items.Clear()
        If Allitem = True Then combo.Items.Add("ALL")
        strSql = " select ItemName from " & cnAdminDb & "..ItemMast Order by ItemName"
        objGPack.FillCombo(strSql, combo, False, False)
    End Function
    Function funcLoadSubItemName(ByVal combo As ComboBox) As Integer
        combo.Text = ""
        combo.Items.Clear()
        combo.Items.Add("ALL")
        strSql = " Select SubItemName from " & cnAdminDb & "..SubItemMast "
        strSql += " Where ItemId = "
        strSql += "(select ItemId from " & cnAdminDb & "..ItemMast where ItemName = '" & cmbItemName_Man.Text & "')"
        strSql += " Order by SubItemName"
        objGPack.FillCombo(strSql, combo, False, False)
        If Not combo.Items.Count > 0 Then combo.Enabled = False Else combo.Enabled = True
    End Function
    Function funcLoadOpenSubItemName(ByVal combo As ComboBox) As Integer
        combo.Text = ""
        combo.Items.Clear()
        combo.Items.Add("ALL")
        strSql = " Select SubItemName from " & cnAdminDb & "..SubItemMast "
        strSql += " Where ItemId = "
        strSql += "(select ItemId from " & cnAdminDb & "..ItemMast where ItemName = '" & cmbOpenItemName.Text & "')"
        strSql += " Order by SubItemName"
        objGPack.FillCombo(strSql, combo, False)
        If Not combo.Items.Count > 0 Then combo.Enabled = False Else combo.Enabled = True
    End Function
    Function funcLoadDesignerName(ByVal combo As ComboBox) As Integer
        strSql = " Select DesignerName from " & cnAdminDb & "..Designer order by DesignerName"
        objGPack.FillCombo(strSql, combo, , False)
    End Function
    Function funcLoadItemType(ByVal combo As ComboBox) As Integer
        combo.Items.Clear()
        'If tabMain.SelectedTab.Name = tabView.Name Then
        '    If cmbOpenType.Text = "ITEM" Then
        '        combo.Items.Add("ALL")
        '    End If
        'Else
        '    If cmbType_Man.Text = "ITEM" Then
        '        combo.Items.Add("ALL")
        '    End If
        'End If
        combo.Items.Add("ALL")
        strSql = " Select Name from " & cnAdminDb & "..ItemType order by Name"
        objGPack.FillCombo(strSql, combo, False, False)
    End Function
    Function funcGetDetails(ByVal tempSno As Integer) As Integer
        strSql = " SELECT "
        strSql += " ISNULL((SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST AS I WHERE I.ITEMID = W.ITEMID),'')AS ITEMNAME,"
        strSql += " ISNULL((SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST AS S WHERE S.SUBITEMID = W.SUBITEMID),'')AS SUBITEMNAME,"
        strSql += " ISNULL((SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER AS D WHERE D.DESIGNERID = W.DESIGNERID),'')AS DESIGNERNAME,"
        strSql += " ISNULL((SELECT NAME FROM " & cnAdminDb & "..ITEMTYPE AS T WHERE T.ITEMTYPEID = W.ITEMTYPE),'')AS ITEMTYPE,"
        strSql += " ISNULL((SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD AS T WHERE T.ACCODE = W.ACCODE),'')AS ACNAME,"
        strSql += " ISNULL(TABLECODE,'')AS TABLECODE, "
        strSql += " FROMWEIGHT,TOWEIGHT, "
        strSql += " MAXWASTPER,MAXMCGRM,MAXWAST,MAXMC, "
        strSql += " MINWASTPER,MINMCGRM,MINWAST,MINMC, "
        strSql += " MAXWASTPER_PUR,MAXMCGRM_PUR,MAXWAST_PUR,MAXMC_PUR, "
        strSql += " MINWASTPER_PUR,MINMCGRM_PUR,MINWAST_PUR,MINMC_PUR, "
        strSql += " DISCOUNTPER,DISCOUNTPER_PUR,TOUCH,TOUCH_PUR,VID,COSTID FROM " & cnAdminDb & "..WMCTABLE AS W"
        strSql += " WHERE VID = " & tempSno & " "
        If gridView.Rows(gridView.CurrentRow.Index).Cells("COSTNAME").Value.ToString <> "" Then
            strSql += " AND COSTID=(SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME ='" & gridView.Rows(gridView.CurrentRow.Index).Cells("COSTNAME").Value & "')"
        End If
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Return 0
        End If
        With dt.Rows(0)
            grpMainPurchase.Visible = False
            If .Item("ACNAME").ToString <> "" Then
                cmbType_Man.Text = "CUSTOMER"
            ElseIf .Item("DESIGNERNAME").ToString <> "" Then
                cmbType_Man.Text = "DESIGNER"
                grpMainPurchase.Visible = True
            ElseIf .Item("ITEMNAME").ToString <> "" Then
                cmbType_Man.Text = "ITEM"
            ElseIf .Item("ItemType").ToString <> "" Then
                cmbType_Man.Text = "TAG"
            ElseIf .Item("TableCode").ToString <> "" Then
                cmbType_Man.Text = "TABLE"
            Else
                cmbType_Man.Text = "ITEM"
            End If
            cmbParty_MAN.Text = .Item("ACNAME").ToString
            cmbItemName_Man.Text = .Item("ItemName").ToString
            cmbSubItemName_Man.Text = .Item("SubItemName").ToString
            txtTableCode__Man.Text = .Item("TableCode").ToString
            cmbDesignerName_Man.Text = .Item("DesignerName").ToString
            cmbItemType_Man.Text = .Item("ItemType").ToString
            txtWtFrom_Wet.Text = .Item("FromWeight").ToString
            txtWtTo_Wet.Text = .Item("ToWeight").ToString
            txtMaxWastage_Per.Text = .Item("MaxWastPer").ToString
            txtMaxMcGrm_Amt.Text = .Item("MaxMcGrm").ToString
            txtMaxFlagWastage_Wet.Text = .Item("MaxWast").ToString
            txtMaxFlatMc_Amt.Text = .Item("MaxMc").ToString
            txtMinWastage_Per.Text = .Item("MinWastPer").ToString
            txtMinMcGrm_Amt.Text = .Item("MinMcGrm").ToString
            txtMinFlagWastage_Wet.Text = .Item("MinWast").ToString
            txtMinFlatMc_Amt.Text = .Item("MinMc").ToString

            txtMaxWastagePur_Per.Text = .Item("MaxWastPer_Pur").ToString
            txtMaxMcGrmPur_Amt.Text = .Item("MaxMcGrm_Pur").ToString
            txtMaxFlagWastagePur_Wet.Text = .Item("MaxWast_Pur").ToString
            txtMaxFlatMcPur_Amt.Text = .Item("MaxMc_Pur").ToString
            txtMinWastagePur_Per.Text = .Item("MinWastPer_Pur").ToString
            txtMinMcGrmPur_Amt.Text = .Item("MinMcGrm_Pur").ToString
            txtMinFlagWastagePur_Wet.Text = .Item("MinWast_Pur").ToString
            txtMinFlatMcPur_Amt.Text = .Item("MinMc_Pur").ToString

            txtDiscountPer_Per.Text = .Item("DiscountPer").ToString
            txtDiscountPerPur_Per.Text = .Item("DiscountPer_PUR").ToString
            txtTouch_AMT.Text = .Item("TOUCH").ToString
            txtTouchPur_AMT.Text = .Item("TOUCH_PUR").ToString
        End With
        sno = tempSno
        'If chkLstCostcentre.Enabled = False Then
        chkLstCostcentre.Items.Clear()
        strSql = " SELECT "
        strSql += " DISTINCT (SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE ISNULL(COSTID,'') = D.COSTID)AS COSTNAME"
        strSql += " FROM " & cnAdminDb & "..WMCTABLE AS D WHERE VID = " & sno & " "
        If gridView.Rows(gridView.CurrentRow.Index).Cells("COSTNAME").Value.ToString <> "" Then
            strSql += " AND D.COSTID=(SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME ='" & gridView.Rows(gridView.CurrentRow.Index).Cells("COSTNAME").Value & "')"
        End If
        Dim _DtTemp As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(_DtTemp)
        For Each ro As DataRow In _DtTemp.Rows
            chkLstCostcentre.Items.Add(ro("COSTNAME").ToString)
            chkLstCostcentre.SetItemChecked(chkLstCostcentre.Items.Count - 1, True)
        Next
        'End If
        flagSave = True
        grpControls.Enabled = False
    End Function

    Private Sub frmValueAdded_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        ElseIf e.KeyChar = Chr(Keys.Escape) Then
            If tabMain.SelectedTab.Name = tabView.Name Then
                tabMain.SelectedTab = tabGeneral
            End If
        End If
    End Sub

    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        funcSave()
    End Sub

    Private Sub OpenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click
        btnOpen_Click(Me, New EventArgs)
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
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        gridView.DataSource = Nothing
        tabMain.SelectedTab = tabView
        If _IsWholeSaleType Then
            cmbOpenType.Text = "CUSTOMER"
        Else
            cmbOpenType.Text = "ITEM"
        End If
        cmbOpenType.Select()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        funcNew()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        funcExit()
    End Sub

    Private Sub cmbType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbType_Man.SelectedIndexChanged
        If flagSave = True Then
            Exit Sub
        End If
        cmbParty_MAN.Visible = False
        lblParty.Visible = False
        cmbItemName_Man.Enabled = False
        cmbSubItemName_Man.Enabled = False
        txtTableCode__Man.Enabled = False
        cmbDesignerName_Man.Enabled = False
        cmbItemType_Man.Enabled = False
        cmbParty_MAN.Enabled = False
        cmbItemName_Man.Text = ""
        cmbSubItemName_Man.Text = ""
        cmbDesignerName_Man.Text = ""
        cmbItemType_Man.Text = ""
        cmbParty_MAN.Text = ""
        cmbItemName_Man.Items.Clear()
        cmbSubItemName_Man.Items.Clear()
        txtTableCode__Man.Clear()
        cmbDesignerName_Man.Items.Clear()
        cmbItemType_Man.Items.Clear()
        cmbParty_MAN.Items.Clear()
        If cmbType_Man.Text = "ITEM" Then
            cmbItemName_Man.Enabled = True
            cmbSubItemName_Man.Enabled = True
            funcLoadItemName(cmbItemName_Man)
            cmbItemType_Man.Enabled = True
            funcLoadItemType(cmbItemType_Man)
        ElseIf cmbType_Man.Text = "TABLE" Then
            txtTableCode__Man.Enabled = True
            If VAL_ITEMTYPE_TABLE Then
                cmbItemType_Man.Enabled = True
                funcLoadItemType(cmbItemType_Man)
            End If
        ElseIf cmbType_Man.Text = "DESIGNER" Then
            cmbItemName_Man.Enabled = True
            cmbSubItemName_Man.Enabled = True

            funcLoadItemName(cmbItemName_Man, True)
            funcLoadSubItemName(cmbSubItemName_Man)
            cmbDesignerName_Man.Enabled = True
            funcLoadDesignerName(cmbDesignerName_Man)
        ElseIf cmbType_Man.Text = "CUSTOMER" Then
            cmbItemName_Man.Enabled = True
            cmbSubItemName_Man.Enabled = True
            cmbItemType_Man.Enabled = True
            funcLoadItemName(cmbItemName_Man)
            funcLoadSubItemName(cmbSubItemName_Man)
            cmbParty_MAN.Visible = True
            cmbParty_MAN.Enabled = True
            LoadParty(cmbParty_MAN)
            funcLoadItemType(cmbItemType_Man)
        Else ''TAG
            cmbItemType_Man.Enabled = True
            funcLoadItemType(cmbItemType_Man)
        End If
    End Sub

    Private Sub cmbOpenType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbOpenType.SelectedIndexChanged
        If flagSave = True Then
            Exit Sub
        End If
        cmbOpenParty.Visible = False
        lblOpenParty.Visible = False
        cmbOpenParty.Visible = False
        lblOpenParty.Visible = False
        cmbOpenItemName.Enabled = False
        cmbOpenSubItemName.Enabled = False
        txtOpenTableCode.Enabled = False
        cmbOpenDesignerName.Enabled = False
        cmbOpenItemType.Enabled = False
        cmbOpenParty.Enabled = False
        cmbOpenItemName.Text = ""
        cmbOpenSubItemName.Text = ""
        cmbOpenDesignerName.Text = ""
        cmbOpenItemType.Text = ""
        cmbOpenParty.Text = ""
        cmbOpenItemName.Items.Clear()
        cmbOpenSubItemName.Items.Clear()
        txtOpenTableCode.Clear()
        cmbOpenDesignerName.Items.Clear()
        cmbOpenItemType.Items.Clear()
        cmbOpenParty.Items.Clear()
        If cmbOpenType.Text = "ITEM" Then
            cmbOpenItemName.Enabled = True
            cmbOpenSubItemName.Enabled = True
            funcLoadItemName(cmbOpenItemName)
            cmbOpenItemType.Enabled = True
            funcLoadItemType(cmbOpenItemType)
        ElseIf cmbOpenType.Text = "TABLE" Then
            txtOpenTableCode.Enabled = True
            cmbOpenItemType.Enabled = True
            funcLoadItemType(cmbOpenItemType)
        ElseIf cmbOpenType.Text = "DESIGNER" Then
            cmbOpenItemName.Enabled = True
            cmbOpenSubItemName.Enabled = True
            cmbOpenDesignerName.Enabled = True
            funcLoadItemName(cmbOpenItemName,True)
            funcLoadSubItemName(cmbOpenSubItemName)
            funcLoadDesignerName(cmbOpenDesignerName)
        ElseIf cmbOpenType.Text = "CUSTOMER" Then
            cmbOpenItemName.Enabled = True
            cmbOpenSubItemName.Enabled = True
            cmbOpenItemType.Enabled = True
            funcLoadItemName(cmbOpenItemName)
            funcLoadSubItemName(cmbOpenSubItemName)
            cmbOpenParty.Visible = True
            lblOpenParty.Visible = True
            cmbOpenParty.Enabled = True
            LoadParty(cmbOpenParty)
            funcLoadItemType(cmbOpenItemType)
        Else ''TAG
            cmbOpenItemType.Enabled = True
            funcLoadItemType(cmbOpenItemType)
        End If
    End Sub
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Prop_Sets()
        If cmbOpenType.Text = "" Or cmbOpenType.Items.Contains(cmbOpenType.Text) = False Then
            cmbOpenType.Select()
            Exit Sub
        End If
        strSql = vbCrLf + " IF OBJECT_ID('MASTER..TEMP_WMCTABLE') IS NOT NULL DROP TABLE MASTER..TEMP_WMCTABLE"
        strSql += vbCrLf + " SELECT W.VID,HE.ACNAME AS ACNAME,IM.ITEMNAME,SM.SUBITEMNAME,TY.NAME AS ITEMTYPE"
        strSql += vbCrLf + " ,DE.DESIGNERNAME,W.TABLECODE,W.FROMWEIGHT,W.TOWEIGHT"
        strSql += vbCrLf + " ,W.MAXWASTPER,W.MAXMCGRM,W.MAXWAST,W.MAXMC,W.MINWASTPER,W.MINMCGRM,W.MINWAST,W.MINMC"
        strSql += vbCrLf + " ,W.MAXWASTPER_PUR,W.MAXMCGRM_PUR,W.MAXWAST_PUR,W.MAXMC_PUR,W.MINWASTPER_PUR,W.MINMCGRM_PUR,W.MINWAST_PUR,W.MINMC_PUR"
        strSql += vbCrLf + " ,W.TOUCH,W.DISCOUNTPER,W.TOUCH_PUR,W.DISCOUNTPER_PUR"
        strSql += vbCrLf + " ,W.DIAFROMPCS AS DIAFROMPCS,W.DIATOPCS AS DIATOPCS"
        strSql += vbCrLf + " ,CONVERT(VARCHAR(20),NULL)WMCTYPE"
        strSql += vbCrLf + " ,SALVALUE "
        strSql += vbCrLf + " ,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE ISNULL(COSTID,'') = W.COSTID)AS COSTNAME"
        strSql += vbCrLf + " INTO MASTER..TEMP_WMCTABLE"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..WMCTABLE AS W"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = W.ITEMID"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.SUBITEMID = W.SUBITEMID AND SM.ITEMID = W.ITEMID"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMTYPE AS TY ON TY.ITEMTYPEID = W.ITEMTYPE"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ACHEAD AS HE ON HE.ACCODE = W.ACCODE"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..DESIGNER AS DE ON DE.DESIGNERID = W.DESIGNERID"
        strSql += vbCrLf + " "
        strSql += vbCrLf + " UPDATE MASTER..TEMP_WMCTABLE SET WMCTYPE = CASE"
        If cmbOpenType.Text = "CUSTOMER" Then
            strSql += vbCrLf + "    WHEN ISNULL(ACNAME,'') <> '' THEN 'CUSTOMER'"
        ElseIf cmbOpenType.Text = "DESIGNER" Then
            strSql += vbCrLf + " 	WHEN ISNULL(DESIGNERNAME,'') <> '' THEN 'DESIGNER'"
        ElseIf cmbOpenType.Text = "TABLE" Then
            strSql += vbCrLf + " 	WHEN ISNULL(TABLECODE,'') <> '' THEN 'TABLE'"
        ElseIf cmbOpenType.Text = "ITEM" Then
            strSql += vbCrLf + " 	WHEN ISNULL(ITEMNAME,'') <> '' THEN 'ITEM'"
        ElseIf cmbOpenType.Text = "TAG" Then
            strSql += vbCrLf + " 	WHEN ISNULL(ITEMTYPE,'') <> '' THEN 'TAG'"
        End If
        strSql += vbCrLf + " 	ELSE 'O' END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strSql = vbCrLf + " SELECT * FROM MASTER..TEMP_WMCTABLE"
        If cmbOpenType.Text = "ITEM" Then
            strSql += vbCrLf + " WHERE WMCTYPE = '" & cmbOpenType.Text & "'"
        ElseIf cmbOpenType.Text = "CUSTOMER" Then
            strSql += vbCrLf + " WHERE WMCTYPE = '" & cmbOpenType.Text & "'"
        ElseIf cmbOpenType.Text = "DESIGNER" Then
            strSql += vbCrLf + " WHERE WMCTYPE = '" & cmbOpenType.Text & "'"
        ElseIf cmbOpenType.Text = "TABLE" Then
            strSql += vbCrLf + " WHERE WMCTYPE='" & cmbOpenType.Text & "'"
            If txtOpenTableCode.Text <> "" Then
                strSql += vbCrLf + " AND TABLECODE = '" & txtOpenTableCode.Text & "' "
            End If
        ElseIf cmbOpenType.Text = "TAG" Then
            strSql += vbCrLf + " WHERE WMCTYPE = '" & cmbOpenType.Text & "'"
        End If
        If cmbOpenParty.Text <> "ALL" And cmbOpenParty.Text <> "" Then
            strSql += vbCrLf + " AND ACNAME = '" & cmbOpenParty.Text & "'"
        End If
        If cmbOpenItemName.Text <> "ALL" And cmbOpenItemName.Text <> "" Then
            strSql += vbCrLf + " AND ITEMNAME = '" & cmbOpenItemName.Text & "'"
        End If
        If cmbOpenSubItemName.Text <> "ALL" And cmbOpenSubItemName.Text <> "" Then
            strSql += vbCrLf + " AND SUBITEMNAME = '" & cmbOpenSubItemName.Text & "'"
        End If
        If cmbOpenDesignerName.Text <> "ALL" And cmbOpenDesignerName.Text <> "" Then
            strSql += vbCrLf + " AND DESIGNERNAME = '" & cmbOpenDesignerName.Text & "'"
        End If
        If cmbOpenItemType.Text <> "ALL" And cmbOpenItemType.Text <> "" Then
            strSql += vbCrLf + " AND ITEMTYPE = '" & cmbOpenItemType.Text & "'"
        End If
        strSql += vbCrLf + " ORDER BY "
        If cmbOpenType.Text = "TABLE" Then
            strSql += vbCrLf + " TABLECODE,ACNAME,ITEMNAME,SUBITEMNAME,ITEMTYPE,DESIGNERNAME,FROMWEIGHT"
        Else
            strSql += vbCrLf + " ACNAME,ITEMNAME,SUBITEMNAME,ITEMTYPE,DESIGNERNAME,TABLECODE,FROMWEIGHT"
        End If
        funcOpenGrid(strSql, gridView)
        With gridView
            .Columns("ACNAME").Visible = False
            .Columns("ITEMNAME").Visible = False
            .Columns("SUBITEMNAME").Visible = False
            .Columns("DESIGNERNAME").Visible = False
            .Columns("ITEMTYPE").Visible = True
            .Columns("TABLECODE").Visible = False
            .Columns("WMCTYPE").Visible = False
            Select Case cmbOpenType.Text
                Case "CUSTOMER"
                    .Columns("ACNAME").Visible = True
                    .Columns("ITEMNAME").Visible = True
                    .Columns("SUBITEMNAME").Visible = True
                    .Columns("ITEMTYPE").Visible = True
                Case "DESIGNER"
                    .Columns("DESIGNERNAME").Visible = True
                    .Columns("ITEMNAME").Visible = True
                    .Columns("SUBITEMNAME").Visible = True
                Case "ITEMTYPE"
                    .Columns("ITEMTYPE").Visible = True
                Case "TABLE"
                    .Columns("TABLECODE").Visible = True
                Case "ITEM"
                    .Columns("ITEMNAME").Visible = True
                    .Columns("SUBITEMNAME").Visible = True
                    .Columns("ITEMTYPE").Visible = True
            End Select
        End With
        For i As Integer = 0 To gridView.ColumnCount - 1
            gridView.Columns(i).ReadOnly = True
        Next
        For CNT As Integer = 7 To gridView.ColumnCount - 1
            gridView.Columns(CNT).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            gridView.Columns(CNT).Visible = False
        Next
        gridView.Columns("DIAFROMPCS").Visible = True
        gridView.Columns("DIATOPCS").Visible = True

        For cnt As Integer = 0 To ChkDisplayList.CheckedItems.Count - 1
            If ChkDisplayList.CheckedItems.Item(cnt).ToString = "DIAFROMPCS" Or ChkDisplayList.CheckedItems.Item(cnt).ToString = "DIATOPCS" Then
                If cmbOpenItemName.Text <> "" Then
                    If ChkDiaitem(cmbOpenItemName.Text) = False Then Continue For
                Else
                    Continue For
                End If
            End If
            gridView.Columns(ChkDisplayList.CheckedItems.Item(cnt).ToString).Visible = True
            gridView.Columns(ChkDisplayList.CheckedItems.Item(cnt).ToString).ReadOnly = False
        Next
        gridView.Columns("COSTNAME").Visible = True
        gridView.Columns("COSTNAME").ReadOnly = True
        If gridView.Columns.Contains("FROMWEIGHT") Then gridView.Columns("FROMWEIGHT").ReadOnly = True
        If gridView.Columns.Contains("TOWEIGHT") Then gridView.Columns("TOWEIGHT").ReadOnly = True
        gridView.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        gridView.Select()
    End Sub

    Private Sub cmbItemName_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbItemName_Man.SelectedIndexChanged
        If cmbItemName_Man.Text = "" Then
            cmbSubItemName_Man.Items.Clear()
        End If
        If flagSave = True Then
            Exit Sub
        End If
        funcLoadSubItemName(cmbSubItemName_Man)
        If cmbItemName_Man.Text <> "" Then
            If ChkDiaitem(cmbItemName_Man.Text) = True Then
                txtDiaFrmPcs_NUM.Text = ""
                txtDiaToPcs_NUM.Text = ""
                pnlDia.Visible = True
            Else
                txtDiaFrmPcs_NUM.Text = ""
                txtDiaToPcs_NUM.Text = ""
                pnlDia.Visible = False
            End If
        End If
    End Sub
    Private Function ChkDiaitem(ByVal ITEMNAME As String) As Boolean
        strSql = " SELECT ISNULL(STUDDEDSTONE,'N') FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & ITEMNAME & "'"
        If objGPack.GetSqlValue(strSql, "", "", ) = "Y" Then
            Return True
        Else
            Return False
        End If

    End Function
    Private Sub cmbOpenItemName_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbOpenItemName.SelectedIndexChanged
        If cmbOpenItemName.Text = "" Then
            cmbOpenSubItemName.Items.Clear()
        End If
        funcLoadOpenSubItemName(cmbOpenSubItemName)
    End Sub

    Private Sub gridView_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.CellEndEdit
        If Not gridView.RowCount > 0 Then Exit Sub
        If gridView.CurrentRow Is Nothing Then Exit Sub
        Dim Costid As String = ""
        If chkLstCostcentre.Items.Count > 0 Then
            For cnt As Integer = 0 To chkLstCostcentre.CheckedItems.Count - 1
                Costid = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & gridView.CurrentRow.Cells("COSTNAME").Value.ToString & "'", , , tran)
            Next
        End If
        strSql = " UPDATE " & cnAdminDb & "..WMCTABLE SET"
        If Me.gridView.CurrentCell.ColumnIndex = gridView.Columns("FROMWEIGHT").Index Then
            strSql += " FROMWEIGHT=" & Val(gridView.CurrentRow.Cells("FROMWEIGHT").Value.ToString) & ""
        ElseIf Me.gridView.CurrentCell.ColumnIndex = gridView.Columns("TOWEIGHT").Index Then
            strSql += " TOWEIGHT=" & Val(gridView.CurrentRow.Cells("TOWEIGHT").Value.ToString) & ""
        ElseIf Me.gridView.CurrentCell.ColumnIndex = gridView.Columns("MAXWASTPER").Index Then
            strSql += " MAXWASTPER=" & Val(gridView.CurrentRow.Cells("MAXWASTPER").Value.ToString) & ""
        ElseIf Me.gridView.CurrentCell.ColumnIndex = gridView.Columns("MAXMCGRM").Index Then
            strSql += " MAXMCGRM=" & Val(gridView.CurrentRow.Cells("MAXMCGRM").Value.ToString) & ""
        ElseIf Me.gridView.CurrentCell.ColumnIndex = gridView.Columns("MAXWAST").Index Then
            strSql += " MAXWAST=" & Val(gridView.CurrentRow.Cells("MAXWAST").Value.ToString) & ""
        ElseIf Me.gridView.CurrentCell.ColumnIndex = gridView.Columns("MAXMC").Index Then
            strSql += " MAXMC=" & Val(gridView.CurrentRow.Cells("MAXMC").Value.ToString) & ""
        ElseIf Me.gridView.CurrentCell.ColumnIndex = gridView.Columns("MINWASTPER").Index Then
            strSql += " MINWASTPER=" & Val(gridView.CurrentRow.Cells("MINWASTPER").Value.ToString) & ""
        ElseIf Me.gridView.CurrentCell.ColumnIndex = gridView.Columns("MINMCGRM").Index Then
            strSql += " MINMCGRM=" & Val(gridView.CurrentRow.Cells("MINMCGRM").Value.ToString) & ""
        ElseIf Me.gridView.CurrentCell.ColumnIndex = gridView.Columns("MINWAST").Index Then
            strSql += " MINWAST=" & Val(gridView.CurrentRow.Cells("MINWAST").Value.ToString) & ""
        ElseIf Me.gridView.CurrentCell.ColumnIndex = gridView.Columns("MINMC").Index Then
            strSql += " MINMC=" & Val(gridView.CurrentRow.Cells("MINMC").Value.ToString) & ""
        ElseIf Me.gridView.CurrentCell.ColumnIndex = gridView.Columns("MAXWASTPER_PUR").Index Then
            strSql += " MAXWASTPER_PUR=" & Val(gridView.CurrentRow.Cells("MAXWASTPER_PUR").Value.ToString) & ""
        ElseIf Me.gridView.CurrentCell.ColumnIndex = gridView.Columns("MAXMCGRM_PUR").Index Then
            strSql += " MAXMCGRM_PUR=" & Val(gridView.CurrentRow.Cells("MAXMCGRM_PUR").Value.ToString) & ""
        ElseIf Me.gridView.CurrentCell.ColumnIndex = gridView.Columns("MAXWAST_PUR").Index Then
            strSql += " MAXWAST_PUR=" & Val(gridView.CurrentRow.Cells("MAXWAST_PUR").Value.ToString) & ""
        ElseIf Me.gridView.CurrentCell.ColumnIndex = gridView.Columns("MAXMC_PUR").Index Then
            strSql += " MAXMC_PUR=" & Val(gridView.CurrentRow.Cells("MAXMC_PUR").Value.ToString) & ""
        ElseIf Me.gridView.CurrentCell.ColumnIndex = gridView.Columns("MINWASTPER_PUR").Index Then
            strSql += " MINWASTPER_PUR=" & Val(gridView.CurrentRow.Cells("MINWASTPER_PUR").Value.ToString) & ""
        ElseIf Me.gridView.CurrentCell.ColumnIndex = gridView.Columns("MINMCGRM_PUR").Index Then
            strSql += " MINMCGRM_PUR=" & Val(gridView.CurrentRow.Cells("MINMCGRM_PUR").Value.ToString) & ""
        ElseIf Me.gridView.CurrentCell.ColumnIndex = gridView.Columns("MINWAST_PUR").Index Then
            strSql += " MINWAST_PUR=" & Val(gridView.CurrentRow.Cells("MINWAST_PUR").Value.ToString) & ""
        ElseIf Me.gridView.CurrentCell.ColumnIndex = gridView.Columns("MINMC_PUR").Index Then
            strSql += " MINMC_PUR=" & Val(gridView.CurrentRow.Cells("MINMC_PUR").Value.ToString) & ""
        ElseIf Me.gridView.CurrentCell.ColumnIndex = gridView.Columns("DISCOUNTPER").Index Then
            strSql += " DISCOUNTPER=" & Val(gridView.CurrentRow.Cells("DISCOUNTPER").Value.ToString) & ""
        ElseIf Me.gridView.CurrentCell.ColumnIndex = gridView.Columns("DISCOUNTPER_PUR").Index Then
            strSql += " DISCOUNTPER_PUR=" & Val(gridView.CurrentRow.Cells("DISCOUNTPER_PUR").Value.ToString) & ""
        ElseIf Me.gridView.CurrentCell.ColumnIndex = gridView.Columns("TOUCH").Index Then
            strSql += " TOUCH = " & Val(gridView.CurrentRow.Cells("TOUCH").Value.ToString) & ""
        ElseIf Me.gridView.CurrentCell.ColumnIndex = gridView.Columns("TOUCH_PUR").Index Then
            strSql += " TOUCH_PUR = " & Val(gridView.CurrentRow.Cells("TOUCH_PUR").Value.ToString) & ""
        ElseIf Me.gridView.CurrentCell.ColumnIndex = gridView.Columns("DIAFROMPCS").Index Then
            strSql += " DIAFROMPCS=" & Val(gridView.CurrentRow.Cells("DIAFROMPCS").Value.ToString) & ""
        ElseIf Me.gridView.CurrentCell.ColumnIndex = gridView.Columns("DIATOPCS").Index Then
            strSql += " DIATOPCS=" & Val(gridView.CurrentRow.Cells("DIATOPCS").Value.ToString) & ""
        ElseIf Me.gridView.CurrentCell.ColumnIndex = gridView.Columns("SALVALUE").Index Then
            strSql += " SALVALUE=" & Val(gridView.CurrentRow.Cells("SALVALUE").Value.ToString) & ""
        End If

        strSql += " ,USERID='" & userId & "'"
        strSql += " ,UPDATED='" & Today.Date.ToString("yyyy-MM-dd") & "'"
        strSql += " ,UPTIME='" & Date.Now.ToLongTimeString & "'"
        strSql += " WHERE ISNULL(COSTID,'') = '" & Costid & "' AND VID = " & Val(gridView.CurrentRow.Cells("VID").Value.ToString) & ""
        ExecQuery(SyncMode.Master, strSql, cn, tran)
    End Sub

    Private Sub gridView_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles gridView.EditingControlShowing
        If Not e.Control Is Nothing Then
            Dim tb As TextBox = CType(e.Control, TextBox)
            If gridView.Columns(gridView.CurrentCell.ColumnIndex).HeaderText.Contains("PER") Then
                tb.Tag = "PER"
            Else
                tb.Tag = "AMOUNT"
            End If
            AddHandler tb.KeyPress, AddressOf TextKeyPressEvent
        End If
    End Sub
    Private Sub TextKeyPressEvent(ByVal sender As Object, ByVal e As KeyPressEventArgs)
        If CType(sender, TextBox).Tag = "AMOUNT" Then
            textBoxKeyPressValidator(CType(sender, TextBox), e, Datatype.Amount)
        ElseIf CType(sender, TextBox).Tag = "PER" Then
            textBoxKeyPressValidator(CType(sender, TextBox), e, Datatype.Percentage)
        Else
            textBoxKeyPressValidator(CType(sender, TextBox), e, Datatype.Weight)
        End If
    End Sub
    Private Sub gridView_RowEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.RowEnter
        objGPack.TextClear(grpInfo)
        With gridView.Rows(e.RowIndex)
            detWeightFrom.Text = .Cells("FromWeight").Value.ToString
            detWeightTo.Text = .Cells("ToWeight").Value.ToString
            detMaxWatagePer.Text = .Cells("MaxWastPer").Value.ToString
            detMaxMcGrm.Text = .Cells("MaxMcGrm").Value.ToString
            detMaxFlatWastage.Text = .Cells("MaxWast").Value.ToString
            detMaxFlatMc.Text = .Cells("MaxMc").Value.ToString
            detMinWatagePer.Text = .Cells("MinWastPer").Value.ToString
            detMinMcGrm.Text = .Cells("MinMcGrm").Value.ToString
            detMinFlatWatage.Text = .Cells("MinWast").Value.ToString
            detMinFlatMc.Text = .Cells("MinMc").Value.ToString

            detMaxWatagePerPur.Text = .Cells("MaxWastPer_Pur").Value.ToString
            detMaxMcGrmPur.Text = .Cells("MaxMcGrm_Pur").Value.ToString
            detMaxFlatWastagePur.Text = .Cells("MaxWast_Pur").Value.ToString
            detMaxFlatMcPur.Text = .Cells("MaxMc_Pur").Value.ToString
            detMinWatagePerPur.Text = .Cells("MinWastPer_Pur").Value.ToString
            detMinMcGrmPur.Text = .Cells("MinMcGrm_Pur").Value.ToString
            detMinFlatWatagePur.Text = .Cells("MinWast_Pur").Value.ToString
            detMinFlatMcPur.Text = .Cells("MinMc_Pur").Value.ToString

            detDiscountPer.Text = .Cells("DiscountPer").Value.ToString
            detDiscountPerPur.Text = .Cells("DiscountPer_PUR").Value.ToString
            detTouch.Text = .Cells("TOUCH").Value.ToString
            detTouchPur.Text = .Cells("TOUCH_PUR").Value.ToString
        End With
    End Sub

    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
        If Not gridView.RowCount > 0 Then
            Exit Sub
        End If
        Dim chkQry As String = "SELECT 1 FROM " & cnAdminDb & "..WMCTABLE WHERE 1<>1"
        Dim delKey As String = gridView.Rows(gridView.CurrentRow.Index).Cells("VID").Value.ToString
        DeleteItem(SyncMode.Master, chkQry, "DELETE FROM " & cnAdminDb & "..WMCTABLE WHERE VID = '" & delKey & "'")
        objGPack.TextClear(grpInfo)
        btnSearch_Click(Me, New EventArgs)
    End Sub

    Private Sub txtWtFrom_Wet_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtWtFrom_Wet.GotFocus
        If flagSave = True Then Exit Sub
        strSql = " SELECT MAX(TOWEIGHT) FROM " & cnAdminDb & "..WMCTABLE"
        strSql += vbCrLf + " WHERE "
        strSql += vbCrLf + " ITEMID = ISNULL((SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "'),0)"
        strSql += vbCrLf + " AND ISNULL(SUBITEMID,0) = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItemName_Man.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "')),0)"
        strSql += vbCrLf + " AND ISNULL(TABLECODE,'') = '" & txtTableCode__Man.Text & "'"
        strSql += vbCrLf + " AND ISNULL(DESIGNERID,0) = ISNULL((SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & cmbDesignerName_Man.Text & "'),0)"
        strSql += vbCrLf + " AND ISNULL(ITEMTYPE,0) = ISNULL((SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & cmbItemType_Man.Text & "'),0)"
        Dim costNames As String = GetCheckedItem(chkLstCostcentre)
        If costNames <> Nothing Then strSql += " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & GetCheckedItem(chkLstCostcentre) & "))"
        If cmbParty_MAN.Visible Then strSql += vbCrLf + " AND ISNULL(ACCODE,'') = ISNULL((SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbParty_MAN.Text & "'),'')" Else strSql += vbCrLf + " AND ISNULL(ACCODE,'') = ''"
        Dim wt As Double = Val(objGPack.GetSqlValue(strSql).ToString)
        txtWtFrom_Wet.Text = Format(wt + 0.001, "0.000") '  IIf(wt <> 0, wt + 0.001, "")
    End Sub

    Private Sub txtTouch_AMT_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTouch_AMT.LostFocus
        If Val(txtTouch_AMT.Text) > 112 Then
            txtTouch_AMT.Focus()
        End If
    End Sub

    Private Sub frmValueAdded_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub cmbParty_MAN_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbParty_MAN.KeyDown
        If e.KeyCode = Keys.Enter Then
            If cmbParty_MAN.Text = "" Or cmbParty_MAN.Items.Contains(cmbParty_MAN.Text) = False Then
                MsgBox("Invalid Party", MsgBoxStyle.Information)
                cmbParty_MAN.Select()
                Exit Sub
            Else
                SendKeys.Send("{TAB}")
            End If
        End If
    End Sub

    Private Sub cmbOpenParty_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbOpenParty.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "VALUE ADDED BASED ON " & cmbOpenType.Text, gridView, BrightPosting.GExport.GExportType.Export)
        End If
        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "VALUE ADDED BASED ON " & cmbOpenType.Text, gridView, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub cmbType_Man_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbType_Man.Leave
        If cmbType_Man.Text = "DESIGNER" Then
            grpMainPurchase.Visible = True
        Else
            grpMainPurchase.Visible = False
        End If
    End Sub

    Private Sub txtMinWastage_Per_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMinWastage_Per.Leave
     
        If Val(txtMinWastage_Per.Text) <> 0 And Val(txtMinWastage_Per.Text) > Val(txtMaxWastage_Per.Text) Then
            MsgBox("Minimum Wastage Percentage Shouldbe Lessthan Maximum Wastage Percentage...")
            txtMinWastage_Per.Focus()
        End If

        
    End Sub

    Private Sub txtMinMcGrm_Amt_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMinMcGrm_Amt.Leave
        
        If Val(txtMinMcGrm_Amt.Text) <> 0 And Val(txtMinMcGrm_Amt.Text) > Val(txtMaxMcGrm_Amt.Text) Then
            MsgBox("Minimum Mc/Grm Shouldbe Lessthan Maximum Mc/Grm...")
            txtMinMcGrm_Amt.Focus()
        End If

    End Sub

    Private Sub txtMinFlagWastage_Wet_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMinFlagWastage_Wet.Leave

        If Val(txtMinFlagWastage_Wet.Text) <> 0 And Val(txtMinFlagWastage_Wet.Text) > Val(txtMaxFlagWastage_Wet.Text) Then
            MsgBox("Minimum Flat Wastage Shouldbe Lessthan Maximum Flat Wastage...")
            txtMinFlagWastage_Wet.Focus()
        End If

    End Sub

    Private Sub txtMinFlatMc_Amt_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMinFlatMc_Amt.Leave

        If Val(txtMinFlatMc_Amt.Text) <> 0 And Val(txtMinFlatMc_Amt.Text) > Val(txtMaxFlatMc_Amt.Text) Then
            MsgBox("Minimum Flat Making Charge Shouldbe Lessthan Maximum Flat Making Charge...")
            txtMinFlatMc_Amt.Focus()
        End If
    End Sub

    Private Sub txtMinWastagePur_Per_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMinWastagePur_Per.Leave

        If Val(txtMinWastagePur_Per.Text) <> 0 And Val(txtMinWastagePur_Per.Text) > Val(txtMaxWastagePur_Per.Text) Then
            MsgBox("Minimum Wastage Percentage Shouldbe Lessthan Maximum Wastage Percentage...")
            txtMinWastagePur_Per.Focus()
        End If
    End Sub

    Private Sub txtMinMcGrmPur_Amt_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMinMcGrmPur_Amt.Leave

        If Val(txtMinMcGrmPur_Amt.Text) <> 0 And Val(txtMinMcGrmPur_Amt.Text) > Val(txtMaxMcGrmPur_Amt.Text) Then
            MsgBox("Minimum Mc/Grm Shouldbe Lessthan Maximum Mc/Grm...")
            txtMinMcGrmPur_Amt.Focus()
        End If

    End Sub

    Private Sub txtMinFlagWastagePur_Wet_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMinFlagWastagePur_Wet.Leave
        
        If Val(txtMinFlagWastagePur_Wet.Text) <> 0 And Val(txtMinFlagWastagePur_Wet.Text) > Val(txtMaxFlagWastagePur_Wet.Text) Then
            MsgBox("Minimum Flat Wastage Shouldbe Lessthan Maximum Flat Wastage...")
            txtMinFlagWastagePur_Wet.Focus()
        End If

    End Sub

    Private Sub txtMinFlatMcPur_Amt_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMinFlatMcPur_Amt.Leave

        If Val(txtMinFlatMcPur_Amt.Text) <> 0 And Val(txtMinFlatMcPur_Amt.Text) > Val(txtMaxFlatMcPur_Amt.Text) Then
            MsgBox("Minimum Flat Making Charge Shouldbe Lessthan Maximum Flat Making Charge...")
            txtMinFlatMcPur_Amt.Focus()
        End If

    End Sub

    Private Sub txtTouchPur_AMT_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTouchPur_AMT.Leave
        If Val(txtTouchPur_AMT.Text) > 112 Then
            txtTouchPur_AMT.Focus()
        End If
    End Sub
    Private Sub loadexcel()
        Dim MyConnection As System.Data.OleDb.OleDbConnection
        MyConnection = New OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;Data Source='" & strpath & "';Extended Properties=Excel 8.0;")
        strSql = "SELECT * FROM [SHEET1$] "
        da = New OleDbDataAdapter(strSql, MyConnection)
        Dt = New DataTable
        da.Fill(Dt)

        Dim dttabl As New DataTable
        dttabl = Dt.Clone
        Select Case cmbOpenType.Text
            Case "ITEM"
                For Each dr1 As DataRow In Dt.Select("ISNULL(ITEMNAME,'')<>'' AND ISNULL(DESIGNERNAME,'') ='' AND ISNULL(TABLE,'') = ''AND ISNULL(ACNAME,'') = ''")
                    dttabl.ImportRow(dr1)
                Next
            Case "TABLE"
                For Each dr1 As DataRow In Dt.Select("ISNULL(TABLE,'') <> ''AND ISNULL(ACNAME,'') = ''")
                    dttabl.ImportRow(dr1)
                Next
            Case "DESIGNER"
                For Each dr1 As DataRow In Dt.Select("ISNULL(DESIGNERNAME,'') <>''AND ISNULL(ITEMNAME,'')<>''AND ISNULL(ACNAME,'') = ''")
                    dttabl.ImportRow(dr1)
                Next
            Case "TAG"
                For Each dr1 As DataRow In Dt.Select("ISNULL(ITEMTYPE,'')<>''AND ISNULL(ACNAME,'') = ''")
                    dttabl.ImportRow(dr1)
                Next
            Case "CUSTOMER"
                For Each dr1 As DataRow In Dt.Select("ISNULL(ACNAME,'') <>'' AND ISNULL(ITEMNAME,'')<>''")
                    dttabl.ImportRow(dr1)
                Next
        End Select

        gridView.DataSource = dttabl
        If gridView.Rows.Count > 0 Then BtnUpdate.Enabled = True
        MyConnection.Close()
    End Sub
    Private Sub BtnImport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnImport.Click
        Dim OpenDialog As New OpenFileDialog

        Dim Str As String
        Str = "(*.xls) | *.xls"
        Str += "|(*.xlsx) | *.xlsx"
        OpenDialog.Filter = Str
        If OpenDialog.ShowDialog = System.Windows.Forms.DialogResult.OK Then
            strpath = OpenDialog.FileName
            If strpath = "" Then
            Else
                loadexcel()
            End If
        End If
    End Sub
    Private Sub InsertExcelWmc(ByVal cId As String, ByVal vId As Integer, ByVal cnt As Integer)
        strSql = " Insert into " & cnAdminDb & "..WMCtable"
        strSql += " ("
        strSql += " ItemId,SubItemId,TableCode,DesignerId,"
        strSql += " ItemType,FromWeight,ToWeight,"
        strSql += " MaxWastPer,MaxMcGrm,MaxWast,MaxMc,"
        strSql += " MinWastPer,MinMcGrm,MinWast,MinMc,"
        strSql += " MaxWastPer_Pur,MaxMcGrm_Pur,MaxWast_Pur,MaxMc_Pur,"
        strSql += " MinWastPer_Pur,MinMcGrm_Pur,MinWast_Pur,MinMc_Pur,"
        strSql += " DiscountPer,DiscountPer_Pur,"
        strSql += " Salvalue,Salvalue_Pur,"
        strSql += " Rate,Rate_Pur,"
        strSql += " UserId,Updated,Uptime"
        strSql += " ,ACCODE,TOUCH,TOUCH_PUR,VID,COSTID"
        strSql += " )Values("
        strSql += " " & Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & gridView.Item("ITEMNAME", cnt).Value.ToString & "'", , , tran)) & "" 'ItemId
        strSql += " ," & Val(objGPack.GetSqlValue("SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & gridView.Item("SUBITEMNAME", cnt).Value & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & gridView.Item("itemname", cnt).Value.ToString & "')", , , tran)) & "" 'SubItemId
        strSql += " ,'" & gridView.Item("table", cnt).Value.ToString & "'" 'TableCode
        strSql += " ," & Val(objGPack.GetSqlValue("SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & gridView.Item("DESIGNERNAME", cnt).Value.ToString & "'", , , tran)) & "" 'DesignerId
        strSql += " ,'" & Val(objGPack.GetSqlValue("SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & gridView.Item("ITEMTYPE", cnt).Value.ToString & "'", , , tran)) & "'" 'ItemType
        strSql += " ," & Val(gridView.Item("FROMWEIGHT", cnt).Value.ToString) & "" 'FromWeight
        strSql += " ," & Val(gridView.Item("TOWEIGHT", cnt).Value.ToString) & "" 'ToWeight

        strSql += " ," & Val(gridView.Item("maxwastper", cnt).Value.ToString) & "" 'MaxWastPer
        strSql += " ," & Val(gridView.Item("maxmcgrm", cnt).Value.ToString) & "" 'MaxMcGrm
        strSql += " ," & Val(gridView.Item("maxwast", cnt).Value.ToString) & "" 'MaxWast
        strSql += " ," & Val(gridView.Item("maxmc", cnt).Value.ToString) & "" 'MaxMc
        strSql += " ," & Val(gridView.Item("minwastper", cnt).Value.ToString) & "" 'MinWastPer
        strSql += " ," & Val(gridView.Item("minmcgrm", cnt).Value.ToString) & "" 'MinMcGrm
        strSql += " ," & Val(gridView.Item("minwast", cnt).Value.ToString) & "" 'MinWast
        strSql += " ," & Val(gridView.Item("minmc", cnt).Value.ToString) & "" 'MinMc
        strSql += " ," & Val(gridView.Item("maxwastper_pur", cnt).Value.ToString) & "" 'MaxWastPer_Purchase
        strSql += " ," & Val(gridView.Item("maxmcgrm_pur", cnt).Value.ToString) & "" 'MaxMcGrm_Purchase
        strSql += " ," & Val(gridView.Item("maxwast_pur", cnt).Value.ToString) & "" 'MaxWast_Purchase
        strSql += " ," & Val(gridView.Item("maxmc_pur", cnt).Value.ToString) & "" 'MaxMc_Purchase
        strSql += " ," & Val(gridView.Item("maxwastper_pur", cnt).Value.ToString) & "" 'MinWastPer_Purchase
        strSql += " ," & Val(gridView.Item("maxmcgrm_pur", cnt).Value.ToString) & "" 'MinMcGrm_Purchase
        strSql += " ," & Val(gridView.Item("minwast_pur", cnt).Value.ToString) & "" 'MinWast_Purchase
        strSql += " ," & Val(gridView.Item("minmc_pur", cnt).Value.ToString) & "" 'MinMc_Purchase

        strSql += " ," & Val(gridView.Item("discountper", cnt).Value.ToString) & "" 'DiscountPer
        strSql += " ," & Val(gridView.Item("discountper_pur", cnt).Value.ToString) & "" 'DiscountPer
        strSql += " ," & Val(gridView.Item("SalValue", cnt).Value.ToString) & "" 'SaleValue
        strSql += " ," & Val(gridView.Item("SalValue_pur", cnt).Value.ToString) & "" 'SaleValue_Pur
        strSql += " ," & Val(gridView.Item("Rate", cnt).Value.ToString) & "" 'Rate
        strSql += " ," & Val(gridView.Item("Rate_pur", cnt).Value.ToString) & "" 'Rate_pur
        strSql += " ,'" & userId & "'" 'UserId
        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'Updated
        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'Uptime
        strSql += " ,'" & objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & gridView.Item("acname", cnt).Value.ToString & "' AND ISNULL(ACNAME,'') <> ''", , , tran) & "'"
        strSql += " ," & Val(gridView.Item("touch", cnt).Value.ToString) & ""
        strSql += " ," & Val(gridView.Item("touch_pur", cnt).Value.ToString) & ""
        strSql += " ," & vId & ""
        strSql += " ,'" & cId & "'"
        strSql += ")"
        ExecQuery(SyncMode.Master, strSql, cn, tran)
        ''funcNew()
    End Sub

    Private Sub BtnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnUpdate.Click
        Dim _Accode As String = ""
        Dim _Acname As String = ""
        Dim _Acname1 As String = ""
        Dim _Costname As String = ""
        Dim _Costname1 As String = ""
        Dim _Costname2 As String = ""
        For cnt As Integer = 0 To gridView.Rows.Count - 1
            _Accode = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & gridView.Item("acname", cnt).Value.ToString & "' AND ISNULL(ACNAME,'') <> ''", , , tran)
            If _Accode = "" Then
                If _Acname1 <> gridView.Item("ACNAME", cnt).Value.ToString Then
                    _Acname1 = gridView.Item("ACNAME", cnt).Value.ToString
                    If Not _Acname.Contains(_Acname1) Then
                        _Acname = _Acname & vbCrLf & _Acname1
                    End If
                End If
            End If
            _Costname = objGPack.GetSqlValue("SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & gridView.Item("costname", cnt).Value.ToString & "'", , , tran)
            If _Costname = "" Then
                If _Costname1 <> gridView.Item("COSTNAME", cnt).Value.ToString Then
                    _Costname1 = gridView.Item("COSTNAME", cnt).Value.ToString
                    If Not _Costname.Contains(_Costname1) Then
                        _Costname2 = _Costname2 & vbCrLf & _Costname1
                    End If
                End If
            End If
        Next
        If cmbOpenType.Text = "CUSTOMER" And _Acname <> "" Then
            MsgBox("Accode Not Found In Achead For Acname " & _Acname, MsgBoxStyle.Information)
            Exit Sub
        End If
        If cmbOpenType.Text = "CUSTOMER" And _Costname2 <> "" Then
            MsgBox("Invalid CostName " & _Costname2, MsgBoxStyle.Information)
            Exit Sub
        End If
        For cnt As Integer = 0 To gridView.Rows.Count - 1
            Dim CId As String = ""
            CId = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & gridView.Item("costname", cnt).Value.ToString & "'", , , tran)

            strSql = " Declare @wtFrom float,@wtTo float"
            strSql += " Set @wtFrom = " & Val(gridView.Item("fromweight", cnt).Value.ToString) & ""
            strSql += " Set @wtTo = " & Val(gridView.Item("toweight", cnt).Value.ToString) & ""
            strSql += " select 1 from " & cnAdminDb & "..WMCtable where "
            strSql += " ((@wtFrom between fromWeight and ToWeight)"
            strSql += " or"
            strSql += " (@wtTo between fromWeight and ToWeight))"
            Select Case cmbOpenType.Text
                Case "ITEM"
                    strSql += " AND ITEMID = ISNULL((SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & gridView.Item("ITEMNAME", cnt).Value.ToString & "'),0)"
                    strSql += " AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & gridView.Item("SUBITEMNAME", cnt).Value.ToString & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbOpenItemName.Text & "')),0)"
                    strSql += " AND ITEMTYPE = ISNULL((SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & gridView.Item("ITEMTYPE", cnt).Value.ToString & "'),0)"
                    strSql += " AND ISNULL(DESIGNERID,0) = 0"
                    strSql += " AND ISNULL(TABLECODE,'') = ''"
                    strSql += " AND ISNULL(ACCODE,'') = ''"
                Case "TABLE"
                    strSql += " AND TABLECODE = '" & Val(gridView.Item("table", cnt).Value.ToString) & "'"
                    strSql += " AND ISNULL(ACCODE,'') = ''"
                Case "DESIGNER"
                    strSql += " AND DESIGNERID = ISNULL((SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & gridView.Item("DESIGNERNAME", cnt).Value.ToString & "'),0)"
                    strSql += " AND ITEMID = ISNULL((SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & gridView.Item("ITEMNAME", cnt).Value.ToString & "'),0)"
                    strSql += " AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & gridView.Item("SUBITEMNAME", cnt).Value.ToString & "' AND ITEMID = (sELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbOpenItemName.Text & "')),0)"
                    strSql += " AND ISNULL(ACCODE,'') = ''"
                Case "TAG"
                    strSql += " AND ITEMTYPE = ISNULL((SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & gridView.Item("ITEMTYPE", cnt).Value.ToString & "'),0)"
                    strSql += " AND ISNULL(ACCODE,'') = ''"
                Case "CUSTOMER"
                    strSql += " AND ITEMID = ISNULL((SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & gridView.Item("ITEMNAME", cnt).Value.ToString & "'),0)"
                    strSql += " AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & gridView.Item("SUBITEMNAME", cnt).Value.ToString & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbOpenItemName.Text & "')),0)"
                    strSql += " AND ISNULL(ACCODE,'') = ISNULL((SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & gridView.Item("ACNAME", cnt).Value.ToString & "'),'')"
                    strSql += " AND ITEMTYPE = ISNULL((SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & gridView.Item("ITEMTYPE", cnt).Value.ToString & "'),0)"
            End Select
            If CId <> "" Then strSql += " AND COSTID IN ('" & CId & "')"
            If objGPack.DupCheck(strSql) Then
                Continue For
            End If

            Dim VId As Integer = objGPack.GetMax("VID", "WMCTABLE", cnAdminDb)

            InsertExcelWmc(CId, VId, cnt)
        Next
        gridView.DataSource = Nothing
        BtnUpdate.Enabled = False
        MsgBox("Inserted Successfully")
    End Sub

    Private Sub BtnTemplate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnTemplate.Click
        exceltemplate()
        Exit Sub
    End Sub
    Function exceltemplate()
        Dim rwStart As Integer = 0
        Dim oXL As Excel.Application
        Dim oWB As Excel.Workbook
        Dim oSheet As Excel.Worksheet
        Dim oRng As Excel.Range

        oXL = CreateObject("Excel.Application")
        oXL.Visible = True
        oWB = oXL.Workbooks.Add
        oSheet = oWB.ActiveSheet

        oSheet.Range("A1").Value = "ITEMNAME"
        oSheet.Range("A1").ColumnWidth = 10.29
        oSheet.Range("B1").Value = "SUBITEMNAME"
        oSheet.Range("B1").ColumnWidth = 13.86
        oSheet.Range("C1").Value = "TABLE"
        oSheet.Range("C1").ColumnWidth = 5.57
        oSheet.Range("D1").Value = "DESIGNERNAME"
        oSheet.Range("D1").ColumnWidth = 14.86
        oSheet.Range("E1").Value = "ACNAME"
        oSheet.Range("E1").ColumnWidth = 8
        oSheet.Range("F1").Value = "ITEMTYPE"
        oSheet.Range("F1").ColumnWidth = 9
        oSheet.Range("G1").Value = "FROMWEIGHT"
        oSheet.Range("G1").ColumnWidth = 13.14
        oSheet.Range("H1").Value = "TOWEIGHT"
        oSheet.Range("H1").ColumnWidth = 10
        oSheet.Range("I1").Value = "MAXWASTPER"
        oSheet.Range("I1").ColumnWidth = 13.29
        oSheet.Range("J1").Value = "MAXMCGRM"
        oSheet.Range("J1").ColumnWidth = 12
        oSheet.Range("K1").Value = "MINWASTPER"
        oSheet.Range("K1").ColumnWidth = 12.86
        oSheet.Range("L1").Value = "MINMCGRM"
        oSheet.Range("L1").ColumnWidth = 11.57
        oSheet.Range("M1").Value = "MINWAST"
        oSheet.Range("M1").ColumnWidth = 9.43
        oSheet.Range("N1").Value = "MINMC"
        oSheet.Range("N1").ColumnWidth = 7.14
        oSheet.Range("O1").Value = "MINWASTPER_PUR"
        oSheet.Range("O1").ColumnWidth = 17.71
        oSheet.Range("P1").Value = "MINMCGRM_PUR"
        oSheet.Range("P1").ColumnWidth = 16.57
        oSheet.Range("Q1").Value = "MINWAST_PUR"
        oSheet.Range("Q1").ColumnWidth = 14.29
        oSheet.Range("R1").Value = "MINMC_PUR"
        oSheet.Range("R1").ColumnWidth = 11.86
        oSheet.Range("S1").Value = "MAXWAST"
        oSheet.Range("S1").ColumnWidth = 9.86
        oSheet.Range("T1").Value = "MAXMC"
        oSheet.Range("T1").ColumnWidth = 7.57
        oSheet.Range("U1").Value = "MAXWASTPER_PUR"
        oSheet.Range("U1").ColumnWidth = 18.14
        oSheet.Range("V1").Value = "MAXMCGRM_PUR"
        oSheet.Range("V1").ColumnWidth = 17
        oSheet.Range("W1").Value = "MAXWAST_PUR"
        oSheet.Range("W1").ColumnWidth = 14.71
        oSheet.Range("X1").Value = "MAXMC_PUR"
        oSheet.Range("X1").ColumnWidth = 12.43
        oSheet.Range("Y1").Value = "DISCOUNTPER"
        oSheet.Range("Y1").ColumnWidth = 13
        oSheet.Range("Z1").Value = "DISCOUNTPER_PUR"
        oSheet.Range("Z1").ColumnWidth = 17.86
        oSheet.Range("AA1").Value = "TOUCH"
        oSheet.Range("AA1").ColumnWidth = 6.43
        oSheet.Range("AB1").Value = "TOUCH_PUR"
        oSheet.Range("AB1").ColumnWidth = 11.29
        oSheet.Range("AC1").Value = "SALVALUE"
        oSheet.Range("AC1").ColumnWidth = 10
        oSheet.Range("AD1").Value = "SALVALUE_PUR"
        oSheet.Range("AD1").ColumnWidth = 15
        oSheet.Range("AE1").Value = "RATE"
        oSheet.Range("AE1").ColumnWidth = 8
        oSheet.Range("AF1").Value = "RATE_PUR"
        oSheet.Range("AF1").ColumnWidth = 8
        oSheet.Range("AG1").Value = "COSTNAME"
        oSheet.Range("AG1").ColumnWidth = 12
        oSheet.Range("A1:B1:C1:D1:E1:F1:G1:H1:I1:J1:K1:L1:M1:N1:O1:P1:Q1:R1:S1:T1:U1:V1:W1:X1:Y1:Z1:AA1:AB1:AC1:AD1:AE1:AF1:AG1").Font.Bold = True
    End Function

    Private Sub releaseObject(ByVal obj As Object)
        Try
            System.Runtime.InteropServices.Marshal.ReleaseComObject(obj)
            obj = Nothing
        Catch ex As Exception
            obj = Nothing
        Finally
            GC.Collect()
        End Try
    End Sub
    Private Sub Prop_Sets()
        Dim obj As New frmValueAddedNew_Properties
        GetChecked_CheckedList(ChkDisplayList, obj.p_chkDisplayList)
        SetSettingsObj(obj, Me.Name, GetType(frmValueAddedNew_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmValueAddedNew_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmValueAddedNew_Properties))
        SetChecked_CheckedList(ChkDisplayList, obj.p_chkDisplayList, Nothing)
    End Sub

    Private Sub rbtDiamond_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtDiamond.CheckedChanged
        txtDiaFrmPcs_NUM.Enabled = rbtDiamond.Checked
        txtDiaToPcs_NUM.Enabled = rbtDiamond.Checked
    End Sub

    Private Sub gridView_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.CellContentClick

    End Sub

    Private Sub btnSave_ContextMenuStripChanged(sender As Object, e As EventArgs) Handles btnSave.ContextMenuStripChanged

    End Sub
End Class
Public Class frmValueAddedNew_Properties
    Private chkDisplayList As New List(Of String)
    Public Property p_chkDisplayList() As List(Of String)
        Get
            Return chkDisplayList
        End Get
        Set(ByVal value As List(Of String))
            chkDisplayList = value
        End Set
    End Property
End Class
