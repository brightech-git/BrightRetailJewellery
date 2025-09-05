Imports System.Data.OleDb
Public Class frmDiscount
    Dim _Cmd As OleDbCommand
    Dim strSql As String
    Dim _Da As OleDbDataAdapter
    Dim _DtTemp As DataTable
    Dim _DiscId As Integer = Nothing
    Dim _flagUpdate As Boolean
    Dim MAIN_DISCNAME As Boolean = IIf(GetAdmindbSoftValue("MAIN_DISCNAME", "N") = "Y", True, False)
    Dim TAG_DISCOUNT As Boolean = IIf(GetAdmindbSoftValue("TAG_DISCOUNT", "N") = "Y", True, False)
    Dim TAG_DISCRANGETYPE As String = GetAdmindbSoftValue("TAG_DISCRANGETYPE", "D")
    Dim TAG_DISCRANGE As Boolean = IIf(GetAdmindbSoftValue("TAG_DISCRANGE", "N") = "Y", True, False)

    Dim objDisc As New frmDisc_Name


    Private Sub frmDiscount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If tabMain.SelectedTab.Name = tabView.Name Then btnBack_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub frmDiscount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If gridView.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmDiscount_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGeneral.Left, Me.tabGeneral.Top, Me.tabGeneral.Width, Me.tabGeneral.Height))
        pnlControls.Location = New Point((ScreenWid - pnlControls.Width) / 2, ((ScreenHit - 128) - pnlControls.Height) / 2)
        cmbType.Items.Add("ESTIMATE")
        cmbType.Items.Add("SALES")
        cmbType.Items.Add("PURCHASE")
        cmbType.Items.Add("CUSTOMER PRIVILEGE")

        cmbAfterTax.Items.Add("YES")
        cmbAfterTax.Items.Add("NO")
        cmbActive.Items.Add("YES")
        cmbActive.Items.Add("NO")

        cmbDiscountGroup.Items.Add("ITEM")
        cmbDiscountGroup.Items.Add("METAL")
        cmbDiscountGroup.Items.Add("TAG")
        cmbDiscountGroup.Items.Add("COUNTER")
        cmbDiscountGroup.Items.Add("BILLAMOUNT")


        cmbBasedOn.Items.Add("GRSWT")
        cmbBasedOn.Items.Add("NETWT")
        cmbBasedOn.Text = "NETWT"

        cmbWithWastage.Items.Add("YES")
        cmbWithWastage.Items.Add("NO")
        cmbWithWastage.Text = "NO"

        cmbDiscRange.Items.Clear()
        cmbDiscRange.Items.Add("DAYS")
        cmbDiscRange.Items.Add("GRAM")
        cmbDiscRange.Items.Add("FINAL AMOUNT")
        cmbDiscRange.Items.Add("WASTAGE")
        cmbDiscRange.Text = "GRAM"

        strSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST ORDER BY DISPLAYORDER"
        objGPack.FillCombo(strSql, cmbMetalName, , False)

        cmbprivilegeType.Items.Clear()
        strSql = " SELECT TYPENAME FROM " & cnAdminDb & "..PRIVILEGETYPE ORDER BY TYPENAME"
        objGPack.FillCombo(strSql, cmbprivilegeType, , False)
        cmbprivilegeType.Text = ""

        strSql = " SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ISNULL(ACTIVE,'')<>'N' ORDER BY DISPLAYORDER,ITEMCTRNAME"
        objGPack.FillCombo(strSql, CmbItemCounter, True, False)

        cmbState.Items.Clear()
        If GetAdmindbSoftValue("COSTCENTRE").ToUpper = "Y" Then
            cmbState.Items.Add("ALL")
            strSql = " SELECT STATENAME FROM " & cnAdminDb & "..STATEMAST ORDER BY STATENAME"
            objGPack.FillCombo(strSql, cmbState, False, False)
            cmbState.Enabled = True
            chkLstCostcentre.Enabled = True
        Else
            cmbState.Enabled = False
            chkLstCostcentre.Enabled = False
        End If
        If TAG_DISCOUNT = True Then
            txtRangefrom_WET.Enabled = True
            txtRangeTo_WET.Enabled = True
            If TAG_DISCRANGETYPE = "D" Then lblRange1.Text = "Stock day from" : lblRange2.Text = "Stock day to"
            If TAG_DISCRANGETYPE = "W" Then lblRange1.Text = "Wastage from" : lblRange2.Text = "Wastage to"
            If TAG_DISCRANGETYPE = "F" Then lblRange1.Text = "Amount from" : lblRange2.Text = "Amount to"
        Else
            txtRangefrom_WET.Enabled = False
            txtRangeTo_WET.Enabled = False
        End If
        If TAG_DISCRANGETYPE = "G" Then lblRange1.Text = "Weight from" : lblRange2.Text = "Weight to" : txtRangefrom_WET.Enabled = True : txtRangeTo_WET.Enabled = True
        If TAG_DISCRANGE Then
            cmbDiscRange.Enabled = True
            If cmbDiscRange.Text = "DAYS" Then lblRange1.Text = "Stock day from" : lblRange2.Text = "Stock day to"
            If cmbDiscRange.Text = "WASTAGE" Then lblRange1.Text = "Wastage from" : lblRange2.Text = "Wastage to"
            If cmbDiscRange.Text = "FINAL AMOUNT" Then lblRange1.Text = "Amount from" : lblRange2.Text = "Amount to"
            If cmbDiscRange.Text = "GRAM" Then lblRange1.Text = "Weight from" : lblRange2.Text = "Weight to"
        Else
            cmbDiscRange.Enabled = False
        End If
        btnNew_Click(Me, New EventArgs)
        'pnlMetalGroup.Visible = False
        'pnlMetalGroup.Enabled = False
        'Panel4.Location = New Point(521, )
        'pnlControls.Location = New Point(, 22 + pnlItemGroup.Height)
    End Sub

    Private Sub SetCheckedList(ByVal chklst As CheckedListBox, ByVal bool As Boolean)
        For cnt As Integer = 0 To chklst.Items.Count - 1
            chklst.SetItemChecked(cnt, bool)
        Next
    End Sub

    Private Sub CallGRid()
        gridView.DataSource = Nothing
        strSql = " SELECT "
        strSql += " (SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = D.COSTID)AS COSTNAME"
        strSql += " ,DISCID,CASE WHEN TYPE = 'S' THEN 'SALES' WHEN TYPE ='E' THEN 'ESTIMATE' WHEN TYPE ='P' THEN 'PURCHASE' ELSE 'CUSTOMER PRIVILEGE' END AS TYPE"
        strSql += " ,CASE WHEN DISCGROUP = 'I' THEN 'ITEM' WHEN DISCGROUP = 'C' THEN 'COUNTER' WHEN DISCGROUP = 'B' THEN 'BILLAMOUNT' WHEN DISCGROUP = 'T' THEN 'TAG' ELSE 'METAL' END AS DISCGROUP"
        strSql += " ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = D.METAL)AS METAL"
        strSql += " ,(SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = D.ITEMCTRID)AS ITEMCOUNTER"
        strSql += " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = D.ITEMID)AS ITEM"
        strSql += " ,ISNULL((SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = D.SUBITEMID),'')AS SUBITEM"
        strSql += " ,CASE WHEN FINALAMTPER <> 0 THEN FINALAMTPER ELSE NULL END [FINALAMT%]"
        strSql += " ,CASE WHEN FINALAMT  <> 0 THEN FINALAMT  ELSE NULL END [FINALAMT]"
        strSql += " ,CASE WHEN WASTAGEPER  <> 0 THEN WASTAGEPER  ELSE NULL END [WAST%]"
        strSql += " ,CASE WHEN LESSWASTAGEPER  <> 0 THEN LESSWASTAGEPER  ELSE NULL END [LESSWAST%]"
        strSql += " ,CASE WHEN MAKINGCHARGEPER  <> 0 THEN MAKINGCHARGEPER  ELSE NULL END [MC%]"
        strSql += " ,CASE WHEN MAKINGCHARGEGRM  <> 0 THEN MAKINGCHARGEGRM  ELSE NULL END [MC/GRM]"
        strSql += " ,CASE WHEN STUDSTNPER  <> 0 THEN STUDSTNPER  ELSE NULL END [STONE%]"
        strSql += " ,CASE WHEN STUDSTNAMT  <> 0 THEN STUDSTNAMT  ELSE NULL END [STONEAMT]"
        strSql += " ,CASE WHEN STUDDIAPER  <> 0 THEN STUDDIAPER  ELSE NULL END [DIA%]"
        strSql += " ,CASE WHEN STUDDIAAMT  <> 0 THEN STUDDIAAMT  ELSE NULL END [DIAAMT]"
        strSql += " ,CASE WHEN BOARDRATE  <> 0 THEN BOARDRATE  ELSE NULL END BOARDRATE"
        strSql += " ,CASE AFTERTAX WHEN  'Y' THEN 'YES' ELSE 'NO' END AFTERTAX"
        strSql += " ,CASE ACTIVE WHEN 'Y' THEN 'YES' ELSE 'NO' END ACTIVE"
        strSql += " ,CASE DISCGRSNET WHEN 'N' THEN 'NETWT' ELSE 'GRSWT' END GRSNET"
        strSql += " ,CASE DISCWITHWAST WHEN 'Y' THEN 'YES' ELSE 'NO' END WITHWAST"
        strSql += " ,ISNULL(CONVERT(DECIMAL(15,3),RANGEFROM),0) RANGEFROM,ISNULL(CONVERT(DECIMAL(15,3),RANGETO),0) RANGETO"
        strSql += " ,LESSONSALWAST,LESSONSALMC,STUDSTNRATE,STUDDIARATE"
        strSql += " ,FROMDATE,TODATE"
        strSql += " ,ISNULL(TAGNO,'') TAGNO "
        strSql += " ,(SELECT TYPENAME FROM " & cnAdminDb & "..PRIVILEGETYPE WHERE TYPEID=D.PREVILEGETYPEID)PREVILEGETYPE,FLATWASTPER,FLATMCPER"
        strSql += " fROM " & cnAdminDb & "..DISCMASTER D"
        Dim dtGrid As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        gridView.DataSource = dtGrid
        gridView.Columns("COSTNAME").Visible = chkLstCostcentre.Enabled
        gridView.Columns("LESSONSALWAST").Visible = False
        gridView.Columns("LESSONSALMC").Visible = False
        gridView.Columns("STUDSTNRATE").Visible = False
        gridView.Columns("STUDDIARATE").Visible = False
        If TAG_DISCRANGETYPE = "" Then
            gridView.Columns("RANGEFROM").Visible = False
            gridView.Columns("RANGETO").Visible = False
        End If
        gridView.Columns("FROMDATE").DefaultCellStyle.Format = "dd-MM-yyyy"
        gridView.Columns("TODATE").DefaultCellStyle.Format = "dd-MM-yyyy"
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
    End Sub

    Private Sub InsertIntoDiscMaster(ByVal cId As String, ByVal MetalId As String, ByVal itemId As Integer, ByVal subItemId As Integer _
                                     , ByVal ItemCtrId As Integer, ByVal PREVILEGETYPEID As String, Optional ByVal TAGNO As String = "")
        strSql = " INSERT INTO " & cnAdminDb & "..DISCMASTER"
        strSql += " ("
        strSql += " DISCID,DISCGROUP,TYPE,TAGNO,METAL,ITEMID,SUBITEMID,RANGETYPE,"
        strSql += " FINALAMTPER,FINALAMT,WASTAGEPER,MAKINGCHARGEGRM,"
        strSql += " MAKINGCHARGEPER,STUDSTNPER,STUDDIAPER,BOARDRATE,"
        strSql += " AFTERTAX,ACTIVE,USERID,UPDATED,UPTIME,DISCGRSNET,DISCWITHWAST,STUDSTNAMT,STUDDIAAMT,COSTID"
        strSql += " ,LESSWASTAGEPER,RANGEFROM,RANGETO,LESSONSALWAST,LESSONSALMC,STUDSTNRATE,STUDDIARATE,ITEMCTRID"
        strSql += " ,FROMDATE,TODATE,PREVILEGETYPEID,FLATWASTPER,FLATMCPER"
        strSql += " )VALUES ("
        strSql += " " & _DiscId & "" 'DISCID
        strSql += " ,'" & Mid(cmbDiscountGroup.Text, 1, 1) & "'" 'DISCGROUP
        strSql += " ,'" & Mid(cmbType.Text, 1, 1) & "'" ' TYPE
        strSql += " ,'" & TAGNO & "'" 'TAGNO
        strSql += " ,'" & MetalId & "'" 'METAL
        strSql += " ," & itemId & "" 'ITEMID
        strSql += " ," & subItemId & "" 'SUBITEMID
        If cmbDiscRange.Enabled Then
            strSql += " ,'" & Mid(cmbDiscRange.Text, 1, 1) & "'" 'RANGETYPE
        Else
            strSql += " ,'" & TAG_DISCRANGETYPE & "'" 'RANGETYPE
        End If
        strSql += " ," & Val(txtOnFinalAmt_Per.Text) & "" 'FINALAMTPER
        strSql += " ," & Val(txtOnFinal_Amt.Text) & "" 'FINALAMT
        strSql += " ," & Val(txtWastage_Per.Text) & "" 'WASTAGEPER
        strSql += " ," & Val(txtMakingCharge_Amt.Text) & "" 'MAKINGCHARGEGRM
        strSql += " ," & Val(txtMaking_Per.Text) & "" 'MAKINGCHARGEPER
        strSql += " ," & Val(txtStuddedStones_Per.Text) & "" 'STUDSTNPER
        strSql += " ," & Val(txtStuddedDiamond_Per.Text) & "" 'STUDDIAPER
        strSql += " ," & Val(txtBoardRate_Amt.Text) & "" 'BOARDRATE
        strSql += " ,'" & Mid(cmbAfterTax.Text, 1, 1) & "'" 'AFTERTAX
        strSql += " ,'" & Mid(cmbActive.Text, 1, 1) & "'" 'ACTIVE
        strSql += " ,'" & userId & "'" 'USERID
        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
        strSql += " ,'" & Mid(cmbBasedOn.Text, 1, 1) & "'" 'DISCGRSNET
        strSql += " ,'" & Mid(cmbWithWastage.Text, 1, 1) & "'" 'DISCWITHWAST
        strSql += " ," & Val(txtStuddedStonesRs_AMT.Text) & "" 'STUDSTNAMT
        strSql += " ," & Val(txtStuddedDiamondRs_AMT.Text) & "" 'STUDDIAAMT
        strSql += " ,'" & cId & "'" 'COSTID
        strSql += " ," & Val(txtLessWastPer_PER.Text) & "" 'LESSWASTAGEPER
        strSql += " ," & Val(txtRangefrom_WET.Text) & "" 'RANGEFROM
        strSql += " ," & Val(txtRangeTo_WET.Text) & "" 'RANGETO
        strSql += " ,'" & IIf(ChkLessWastPer.Checked And Val(txtLessWastPer_PER.Text) > 0, "Y", "N") & "'" 'LESSONSALWAST
        strSql += " ,'" & IIf(ChkLessMakingPer.Checked And (Val(txtMaking_Per.Text) > 0 Or Val(txtMakingCharge_Amt.Text) > 0), "Y", "N") & "'" 'LESSONSALMC
        strSql += " ," & Val(txtStudStnrate_AMT.Text) & "" 'STUDSTNERATE
        strSql += " ," & Val(txtStudDia_Amt.Text) & "" 'STUDDIARATE
        strSql += " ," & ItemCtrId & "" 'STUDDIARATE
        strSql += " ,'" & dtpFrom.Value.ToString("yyyy-MM-dd") & "','" & dtpTo.Value.ToString("yyyy-MM-dd") & "'" 'UPDATED
        strSql += " ,'" & Val(PREVILEGETYPEID.ToString.Trim) & "'" 'PREVILEGETYPEID
        strSql += " ,'" & Val(txtFlatWastPer.Text.ToString.Trim) & "'" 'FlatWastPer
        strSql += " ,'" & Val(txtFlatMcPer_PER.Text.ToString.Trim) & "'" 'FLATMCPER
        strSql += " )"
        ExecQuery(SyncMode.Master, strSql, cn, tran, cId)
    End Sub

    Private Sub InsertIntoMultiDisc(ByVal DiscName As String, ByVal Fromdate As Date, ByVal Todate As Date)
        strSql = " INSERT INTO " & cnAdminDb & "..MULTIDISCOUNT"
        strSql += " ("
        strSql += " DISCID,DISCNAME,FROMDATE,TODATE)"
        strSql += " VALUES ("
        strSql += " '" & _DiscId & "'" 'DISCID
        strSql += " ,'" & DiscName & "'" 'DISCNAME
        strSql += " ,'" & Format(Fromdate, "yyyy-MM-dd") & "'" ' Fromdate
        strSql += " ,'" & Format(Todate, "yyyy-MM-dd") & "'" 'Todate
        strSql += " )"
        'ExecQuery(SyncMode.Master, strSql, cn, tran)
        _Cmd = New OleDbCommand(strSql, cn, tran)
        _Cmd.ExecuteNonQuery()
    End Sub

    Function Updat(ByVal cId As String) As Integer
        strSql = " UPDATE " & cnAdminDb & "..DISCMASTER SET"
        strSql += " FINALAMTPER = " & Val(txtOnFinalAmt_Per.Text) & "" 'FINALAMTPER
        strSql += " ,FINALAMT = " & Val(txtOnFinal_Amt.Text) & "" 'FINALAMT
        strSql += " ,WASTAGEPER = " & Val(txtWastage_Per.Text) & "" 'WASTAGEPER
        strSql += " ,MAKINGCHARGEGRM = " & Val(txtMakingCharge_Amt.Text) & "" 'MAKINGCHARGEGRM
        strSql += " ,MAKINGCHARGEPER = " & Val(txtMaking_Per.Text) & "" 'MAKINGCHARGEPER
        strSql += " ,STUDSTNPER = " & Val(txtStuddedStones_Per.Text) & "" 'STUDSTNPER
        strSql += " ,STUDDIAPER = " & Val(txtStuddedDiamond_Per.Text) & "" 'STUDDIAPER
        strSql += " ,BOARDRATE = " & Val(txtBoardRate_Amt.Text) & "" 'BOARDRATE
        strSql += " ,AFTERTAX = '" & Mid(cmbAfterTax.Text, 1, 1) & "'" 'AFTERTAX
        strSql += " ,ACTIVE = '" & Mid(cmbActive.Text, 1, 1) & "'" 'ACTIVE
        strSql += " ,USERID = " & userId & "" 'USERID
        If cmbDiscRange.Enabled Then
            strSql += " ,RANGETYPE= '" & Mid(cmbDiscRange.Text, 1, 1) & "'" 'RANGETYPE
        Else
            strSql += " ,RANGETYPE= '" & TAG_DISCRANGETYPE & "'" 'RANGETYPE
        End If
        strSql += " ,DISCGRSNET = '" & Mid(cmbBasedOn.Text, 1, 1) & "'" 'DISCGRSNET
        strSql += " ,DISCWITHWAST = '" & Mid(cmbWithWastage.Text, 1, 1) & "'" 'DISCWITHWAST
        strSql += " ,STUDSTNAMT = " & Val(txtStuddedStonesRs_AMT.Text) & "" 'STUDSTNAMT
        strSql += " ,STUDDIAAMT = " & Val(txtStuddedDiamondRs_AMT.Text) & "" 'STUDDIAAMT
        strSql += " ,LESSWASTAGEPER = " & Val(txtLessWastPer_PER.Text) & "" 'LESSWASTAGEPER
        strSql += " ,RANGEFROM = " & Val(txtRangefrom_WET.Text) & "" 'RANGEFROM
        strSql += " ,RANGETO = " & Val(txtRangeTo_WET.Text) & "" 'RANGETO
        strSql += " ,LESSONSALWAST = '" & IIf(ChkLessWastPer.Checked And Val(txtLessWastPer_PER.Text) > 0, "Y", "N") & "'" 'LESSONSALWAST
        strSql += " ,LESSONSALMC = '" & IIf(ChkLessMakingPer.Checked And (Val(txtMaking_Per.Text) > 0 Or Val(txtMakingCharge_Amt.Text) > 0), "Y", "N") & "'" 'LESSONSALMC
        strSql += " ,STUDSTNRATE='" & Val(txtStudStnrate_AMT.Text) & "'" 'STUDSTNERATE
        strSql += " ,STUDDIARATE='" & Val(txtStudDia_Amt.Text) & "'" 'STUDDIARATE
        strSql += " ,FLATWASTPER='" & Val(txtFlatWastPer.Text) & "'" 'FLATWASTPER
        strSql += " ,FLATMCPER='" & Val(txtFlatMcPer_PER.Text) & "'" 'FLATMCPER
        strSql += " ,FROMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "',TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += " ,PREVILEGETYPEID='" & Val(objGPack.GetSqlValue("SELECT TOP 1 ISNULL(TYPEID,0)TYPEID FROM " & cnAdminDb & "..PRIVILEGETYPE WHERE  TYPENAME = '" & cmbprivilegeType.Text & "'", , , tran)) & "'" 'PREVILEGETYPEID
        strSql += " WHERE DISCID = " & _DiscId & " AND COSTID = '" & cId & "'"
        ExecQuery(SyncMode.Master, strSql, cn, tran, cId)
    End Function

    Function UpdateMultiDisc(ByVal DiscName As String, ByVal Fromdate As Date, ByVal Todate As Date) As Integer
        strSql = " UPDATE " & cnAdminDb & "..MULTIDISCOUNT SET"
        strSql += " DISCNAME = '" & DiscName & "'" 'DISCNAME
        strSql += " ,FROMDATE = '" & Format(Fromdate, "yyyy-MM-dd") & "'" 'FROMDATE
        strSql += " ,TODATE = '" & Format(Todate, "yyyy-MM-dd") & "'" 'TODATE
        strSql += " WHERE DISCID = '" & _DiscId & "'"
        'ExecQuery(SyncMode.Master, strSql, cn, tran)
        _Cmd = New OleDbCommand(strSql, cn, tran)
        _Cmd.ExecuteNonQuery()
    End Function

    Function Insert(ByVal cId As String) As Integer
        Dim itemCtrId As Integer
        Dim itemId As Integer
        Dim validSubId As Integer
        Dim subItemId As Integer
        Dim metalid As String = ""
        Dim PrivilegeTypeId As String = ""
        If cmbDiscountGroup.Text = "TAG" Then
            metalid = objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetalName.Text & "'", , , tran)
            InsertIntoDiscMaster(cId, metalid, 0, 0, 0, txtTagNo.Text, 0)
        ElseIf cmbDiscountGroup.Text = "COUNTER" Then
            itemCtrId = Val(objGPack.GetSqlValue("SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & CmbItemCounter.Text & "'", , , tran))
            metalid = objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetalName.Text & "'", , , tran)
            InsertIntoDiscMaster(cId, metalid, 0, 0, itemCtrId, 0)
        ElseIf cmbDiscountGroup.Text = "ITEM" Then
            itemId = Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "'", , , tran))
            subItemId = Val(objGPack.GetSqlValue("SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem.Text & "' AND ITEMID = " & itemId & "", , , tran))
            metalid = objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & itemId & "", , , tran)
            InsertIntoDiscMaster(cId, metalid, itemId, subItemId, 0, 0)
        ElseIf cmbDiscountGroup.Text = "BILLAMOUNT" Then
            metalid = objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetalName.Text & "'", , , tran)
            InsertIntoDiscMaster(cId, metalid, 0, 0, 0, 0, 0)
        Else
            For cnt As Integer = 0 To chkLstItem.CheckedItems.Count - 1
                itemId = Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & chkLstItem.CheckedItems.Item(cnt).ToString & "'", , , tran))
                subItemId = Val(objGPack.GetSqlValue("SELECT TOP 1 SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE  ITEMID = " & itemId & "", , , tran))
                PrivilegeTypeId = Val(objGPack.GetSqlValue("SELECT TOP 1 ISNULL(TYPEID,0)TYPEID FROM " & cnAdminDb & "..PRIVILEGETYPE WHERE  TYPENAME = '" & cmbprivilegeType.Text & "'", , , tran))
                If subItemId = 0 Then
                    validSubId = 0
                End If
                metalid = objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & itemId & "", , , tran)
                For cnt1 As Integer = 0 To chkLstSubItem.CheckedItems.Count - 1
                    subItemId = Val(objGPack.GetSqlValue("SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & chkLstSubItem.CheckedItems.Item(cnt1).ToString & "' AND ITEMID = " & itemId & "", , , tran))
                    validSubId = Val(validSubId + Val(subItemId + 1))
                    If subItemId > 0 Then
                        InsertIntoDiscMaster(cId, metalid, itemId, subItemId, 0, PrivilegeTypeId)
                    End If
                    If validSubId = 1 Then
                        InsertIntoDiscMaster(cId, metalid, itemId, 0, 0, PrivilegeTypeId)
                    End If
                Next
                If chkLstSubItem.CheckedItems.Count = 0 Then
                    InsertIntoDiscMaster(cId, metalid, itemId, 0, 0, PrivilegeTypeId)
                End If
            Next
        End If
    End Function

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

    Private Sub cmbItemName_Man_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        'If objGPack.GetSqlValue("SELECT STUDDED FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "'") = "S" Then
        '    txtStuddedDiamond_Per.Enabled = True
        '    txtStuddedDiamondRs_AMT.Enabled = True
        '    txtStuddedStones_Per.Enabled = True
        '    txtStuddedStonesRs_AMT.Enabled = True
        'Else
        '    txtStuddedDiamond_Per.Clear()
        '    txtStuddedDiamondRs_AMT.Clear()
        '    txtStuddedStones_Per.Clear()
        '    txtStuddedStonesRs_AMT.Clear()
        '    txtStuddedDiamond_Per.Enabled = False
        '    txtStuddedDiamondRs_AMT.Enabled = False
        '    txtStuddedStones_Per.Enabled = False
        '    txtStuddedStonesRs_AMT.Enabled = False
        'End If
    End Sub



    Private Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGeneral
        txtOnFinalAmt_Per.Select()
    End Sub

    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If Not gridView.RowCount > 0 Then
            Exit Sub
        End If
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
        Dim chkQry As String = "SELECT 1 FROM " & cnAdminDb & "..DISCMASTER WHERE 1<>1"
        Dim delKey As String = gridView.Rows(gridView.CurrentRow.Index).Cells("DISCID").Value.ToString
        DeleteItem(SyncMode.Master, chkQry, "DELETE FROM " & cnAdminDb & "..DISCMASTER WHERE DISCID = '" & delKey & "'")
        CallGRid()
        If gridView.RowCount > 0 Then gridView.Focus() Else btnBack_Click(Me, New EventArgs)
    End Sub



    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then e.Handled = True
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            e.Handled = True
            If Not gridView.RowCount > 0 Then Exit Sub
            gridView.CurrentCell = gridView.Rows(gridView.CurrentRow.Index).Cells("ITEM")
            With gridView.CurrentRow
                cmbType.Text = .Cells("TYPE").Value.ToString
                cmbDiscountGroup.Text = .Cells("DISCGROUP").Value.ToString
                cmbMetalName.Text = .Cells("METAL").Value.ToString
                If cmbState.Enabled Then cmbState.Text = "ALL"
                chkLstItem.Items.Clear()
                chkLstSubItem.Items.Clear()
                chkLstCostcentre.Items.Clear()
                txtTagNo.Clear()
                _DiscId = Val(.Cells("DISCID").Value.ToString)
                If cmbDiscountGroup.Text = "ITEM" Then
                    strSql = " SELECT "
                    strSql += " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = D.ITEMID)AS ITEM"
                    strSql += " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = D.SUBITEMID)AS SUBITEM"
                    strSql += " FROM " & cnAdminDb & "..DISCMASTER AS D WHERE DISCID = " & _DiscId & ""
                    _DtTemp = New DataTable
                    da = New OleDbDataAdapter(strSql, cn)
                    da.Fill(_DtTemp)
                    cmbItemName_Man.Text = _DtTemp.Rows(0).Item("ITEM").ToString
                    cmbSubItem.Text = _DtTemp.Rows(0).Item("SUBITEM").ToString
                ElseIf cmbDiscountGroup.Text = "COUNTER" Then
                    strSql = " SELECT "
                    strSql += " (SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = D.ITEMCTRID)AS ITEMCOUNTER"
                    strSql += " FROM " & cnAdminDb & "..DISCMASTER AS D WHERE DISCID = " & _DiscId & ""
                    _DtTemp = New DataTable
                    da = New OleDbDataAdapter(strSql, cn)
                    da.Fill(_DtTemp)
                    CmbItemCounter.Text = _DtTemp.Rows(0).Item("ITEMCOUNTER").ToString
                Else
                    strSql = " SELECT "
                    strSql += " DISTINCT (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = D.ITEMID)AS ITEM"
                    strSql += " FROM " & cnAdminDb & "..DISCMASTER AS D WHERE DISCID = " & _DiscId & ""
                    _DtTemp = New DataTable
                    da = New OleDbDataAdapter(strSql, cn)
                    da.Fill(_DtTemp)
                    For Each ro As DataRow In _DtTemp.Rows
                        chkLstItem.Items.Add(ro("ITEM").ToString)
                        chkLstItem.SetItemChecked(chkLstItem.Items.Count - 1, True)
                    Next
                End If
                If GetAdmindbSoftValue("COSTCENTRE").ToUpper = "Y" Then
                    strSql = " SELECT "
                    strSql += " DISTINCT (SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = D.COSTID)AS COSTNAME"
                    strSql += " FROM " & cnAdminDb & "..DISCMASTER AS D WHERE DISCID = " & _DiscId & ""
                    _DtTemp = New DataTable
                    da = New OleDbDataAdapter(strSql, cn)
                    da.Fill(_DtTemp)
                    For Each ro As DataRow In _DtTemp.Rows
                        chkLstCostcentre.Items.Add(ro("COSTNAME").ToString)
                        chkLstCostcentre.SetItemChecked(chkLstCostcentre.Items.Count - 1, True)
                    Next
                End If

                txtOnFinalAmt_Per.Text = IIf(Val(.Cells("FINALAMT%").Value.ToString) <> 0, .Cells("FINALAMT%").Value.ToString, Nothing)
                txtOnFinal_Amt.Text = IIf(Val(.Cells("FINALAMT").Value.ToString) <> 0, .Cells("FINALAMT").Value.ToString, Nothing)
                txtWastage_Per.Text = IIf(Val(.Cells("WAST%").Value.ToString) <> 0, .Cells("WAST%").Value.ToString, Nothing)
                txtLessWastPer_PER.Text = IIf(Val(.Cells("LESSWAST%").Value.ToString) <> 0, .Cells("LESSWAST%").Value.ToString, Nothing)
                txtMakingCharge_Amt.Text = IIf(Val(.Cells("MC/GRM").Value.ToString) <> 0, .Cells("MC/GRM").Value.ToString, Nothing)
                txtMaking_Per.Text = IIf(Val(.Cells("MC%").Value.ToString) <> 0, .Cells("MC%").Value.ToString, Nothing)
                txtStuddedStones_Per.Text = IIf(Val(.Cells("STONE%").Value.ToString) <> 0, .Cells("STONE%").Value.ToString, Nothing)
                txtStuddedStonesRs_AMT.Text = IIf(Val(.Cells("STONEAMT").Value.ToString) <> 0, .Cells("STONEAMT").Value.ToString, Nothing)
                txtStuddedDiamond_Per.Text = IIf(Val(.Cells("DIA%").Value.ToString) <> 0, .Cells("DIA%").Value.ToString, Nothing)
                txtStuddedDiamondRs_AMT.Text = IIf(Val(.Cells("DIAAMT").Value.ToString) <> 0, .Cells("DIAAMT").Value.ToString, Nothing)
                txtBoardRate_Amt.Text = IIf(Val(.Cells("BOARDRATE").Value.ToString) <> 0, .Cells("BOARDRATE").Value.ToString, Nothing)
                txtRangefrom_WET.Text = Val(.Cells("RANGEFROM").Value.ToString)
                txtRangeTo_WET.Text = Val(.Cells("RANGETO").Value.ToString)
                cmbAfterTax.Text = .Cells("AFTERTAX").Value.ToString
                cmbActive.Text = .Cells("ACTIVE").Value.ToString
                cmbBasedOn.Text = .Cells("GRSNET").Value.ToString
                cmbWithWastage.Text = .Cells("WITHWAST").Value.ToString
                ChkLessWastPer.Checked = IIf((.Cells("LESSONSALWAST").Value.ToString) = "Y", True, False)
                ChkLessMakingPer.Checked = IIf((.Cells("LESSONSALMC").Value.ToString) = "Y", True, False)
                txtStudStnrate_AMT.Text = IIf(Val(.Cells("STUDSTNRATE").Value.ToString) <> 0, .Cells("STUDSTNRATE").Value.ToString, Nothing)
                txtStudDia_Amt.Text = IIf(Val(.Cells("STUDDIARATE").Value.ToString) <> 0, .Cells("STUDDIARATE").Value.ToString, Nothing)
                dtpFrom.Value = .Cells("FROMDATE").Value : dtpTo.Value = .Cells("TODATE").Value
                txtTagNo.Text = .Cells("TAGNO").Value.ToString
                txtFlatWastPer.Text = IIf(Val(.Cells("FLATWASTPER").Value.ToString) <> 0, .Cells("FLATWASTPER").Value.ToString, Nothing)
                txtFlatMcPer_PER.Text = IIf(Val(.Cells("FLATMCPER").Value.ToString) <> 0, .Cells("FLATMCPER").Value.ToString, Nothing)
                If .Cells("TYPE").Value.ToString.Trim = "CUSTOMER PRIVILEGE" Then
                    cmbprivilegeType.Visible = True
                    lblPrivilegeType.Visible = True
                    cmbprivilegeType.Enabled = True
                    lblPrivilegeType.Enabled = True
                    cmbprivilegeType.Text = .Cells("PREVILEGETYPE").Value.ToString.Trim
                Else
                    cmbprivilegeType.Visible = False
                    lblPrivilegeType.Visible = False
                End If

                _flagUpdate = True
                btnBack_Click(Me, New EventArgs)
            End With
        End If
    End Sub
    Private Sub skip_Gotfocus(ByVal sender As Object, ByVal e As System.EventArgs)
        If _flagUpdate Then Me.SelectNextControl(CType(sender, Control), True, True, True, True)
    End Sub

    Private Sub cmbDiscountGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbDiscountGroup.SelectedIndexChanged
        'pnlItemGroup.Visible = False
        'pnlMetalGroup.Visible = False
        CmbItemCounter.Visible = False
        pnlItemGroup.Enabled = False
        pnlMetalGroup.Enabled = False
        cmbMetalName.Visible = True
        Label2.Text = "Metal Name"
        'cmbMetalName_Man.Text = ""
        If cmbDiscountGroup.Text = "ITEM" Then
            'pnlControls.Location = New Point(258, 22 + pnlItemGroup.Height)
            'pnlItemGroup.Visible = True
            pnlItemGroup.Enabled = True
        ElseIf cmbDiscountGroup.Text = "COUNTER" Then
            CmbItemCounter.Visible = True
            cmbMetalName.Visible = False
            Label2.Text = "Counter Name"
            'PnlCounter.Visible = True
            'PnlCounter.Enabled = True
        ElseIf cmbDiscountGroup.Text = "TAG" Then
            cmbItemName_Man.Enabled = False
            cmbSubItem.Enabled = False
            cmbMetalName.Enabled = False
            cmbDiscRange.Enabled = False
            'comment on 25-06-2018
            'txtRangefrom_WET.Enabled = False
            'txtRangeTo_WET.Enabled = False
            pnlItemGroup.Enabled = True
        Else
            'pnlControls.Location = New Point(258, 22)
            'pnlMetalGroup.Visible = True
            pnlMetalGroup.Enabled = True
        End If
        If cmbType.Text = "CUSTOMER PRIVILEGE" Then
            lblPrivilegeType.Visible = True
            cmbprivilegeType.Visible = True
            lblPrivilegeType.Enabled = True
            cmbprivilegeType.Enabled = True
            cmbprivilegeType.Text = ""
        Else
            lblPrivilegeType.Visible = False
            cmbprivilegeType.Visible = False
            cmbprivilegeType.Text = ""
        End If
    End Sub
    Private Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click, SaveToolStripMenuItem.Click
        ''VALIDATION
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
        If objGPack.Validator_Check(Me) Then Exit Sub
        If cmbDiscountGroup.Text = "TAG" Then
            If txtTagNo.Text = "" Then
                MsgBox("Tag No Empty", MsgBoxStyle.Information)
                txtTagNo.Focus()
                Exit Sub
            End If
        ElseIf cmbDiscountGroup.Text = "COUNTER" Then
            If CmbItemCounter.Text.Trim = "" Then
                MsgBox("Counter should not Empty", MsgBoxStyle.Information)
                CmbItemCounter.Focus()
                Exit Sub
            End If
        ElseIf cmbDiscountGroup.Text = "BILLAMOUNT" Then
            If cmbMetalName.Text = "" Then
                MsgBox(Me.GetNextControl(cmbMetalName, False).Text + E0001, MsgBoxStyle.Information)
                cmbMetalName.Focus()
                Exit Sub
            End If
        Else
            If cmbDiscountGroup.Text = "ITEM" Then
                If cmbItemName_Man.Text = "" Then
                    MsgBox(Me.GetNextControl(cmbItemName_Man, False).Text + E0001, MsgBoxStyle.Information)
                    cmbItemName_Man.Focus()
                    Exit Sub
                End If
            Else
                If cmbMetalName.Text = "" Then
                    MsgBox(Me.GetNextControl(cmbMetalName, False).Text + E0001, MsgBoxStyle.Information)
                    cmbMetalName.Focus()
                    Exit Sub
                End If
                If cmbDiscountGroup.Text = "METAL" Then
                    If Not chkLstItem.CheckedItems.Count > 0 Then
                        MsgBox("Item selection should not empty", MsgBoxStyle.Information)
                        Exit Sub
                    End If
                End If
            End If
        End If
        If cmbDiscountGroup.Text <> "TAG" Then
            If Val(txtOnFinalAmt_Per.Text) = 0 And Val(txtOnFinal_Amt.Text) = 0 And
            Val(txtWastage_Per.Text) = 0 And Val(txtMaking_Per.Text) = 0 And
            Val(txtMakingCharge_Amt.Text) = 0 And Val(txtStuddedStones_Per.Text) = 0 And
            Val(txtStuddedStonesRs_AMT.Text) = 0 And Val(txtStuddedDiamond_Per.Text) = 0 And
            Val(txtStuddedDiamondRs_AMT.Text) = 0 And Val(txtBoardRate_Amt.Text) = 0 And Val(txtLessWastPer_PER.Text) = 0 And
            Val(txtStudStnrate_AMT.Text) = 0 And Val(txtStudDia_Amt.Text) = 0 And
            Val(txtFlatWastPer.Text) = 0 And Val(txtFlatMcPer_PER.Text) = 0 Then
                txtOnFinalAmt_Per.Focus()
                Exit Sub
            End If
        End If
        If TAG_DISCOUNT And txtTagNo.Text = "" Then
            If txtRangefrom_WET.Text = "" Then
                MsgBox("From Weight is empty", MsgBoxStyle.Information)
                txtRangefrom_WET.Focus()
                Exit Sub
            End If
            If txtRangeTo_WET.Text = "" Then
                MsgBox("To Weight is empty", MsgBoxStyle.Information)
                txtRangeTo_WET.Focus()
                Exit Sub
            End If
        Else
            If TAG_DISCRANGETYPE <> "N" Then
                If txtRangefrom_WET.Text = "" Then
                    MsgBox("Select range from days", MsgBoxStyle.Information)
                    txtRangefrom_WET.Focus()
                    Exit Sub
                End If
                If txtRangeTo_WET.Text = "" Then
                    MsgBox("Select range to days", MsgBoxStyle.Information)
                    txtRangeTo_WET.Focus()
                    Exit Sub
                End If
            End If
        End If

        If chkLstCostcentre.Items.Count > 0 Then
            If Not chkLstCostcentre.CheckedItems.Count > 0 Then
                MsgBox("CostCentre selection should not empty", MsgBoxStyle.Information)
                chkLstCostcentre.Focus()
                Exit Sub
            End If
        End If
        If cmbDiscountGroup.Text = "TAG" Then

        End If

        strSql = " SELECT 'CHECK' FROM " & cnAdminDb & "..DISCMASTER"
        strSql += " WHERE DISCID <> " & _DiscId & ""
        If cmbDiscountGroup.Text = "TAG" Then
            strSql += " AND DISCGROUP = 'T'"
            strSql += " AND TAGNO = '" & txtTagNo.Text & "'"
        ElseIf cmbDiscountGroup.Text = "COUNTER" Then
            strSql += " AND DISCGROUP = 'C'"
            strSql += " AND ITEMCTRID = (SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & CmbItemCounter.Text & "')"
        ElseIf cmbDiscountGroup.Text = "ITEM" Then
            strSql += " AND DISCGROUP = 'I'"
            strSql += " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "')"
            strSql += " AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem.Text & "' AND ITEMID = (sELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "')),0)"
            strSql += " AND TYPE = '" & Mid(cmbType.Text, 1, 1) & "'"
        ElseIf cmbDiscountGroup.Text = "METAL" Then
            strSql += " AND DISCGROUP = 'M'"
            strSql += " AND METAL = (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetalName.Text & "')"
            strSql += " AND ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & GetCheckedItem(chkLstItem) & "))"
            strSql += " AND TYPE = '" & Mid(cmbType.Text, 1, 1) & "'"
        ElseIf cmbDiscountGroup.Text = "BILLAMOUNT" Then
            strSql += " AND DISCGROUP = 'B'"
            strSql += " AND METAL = (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetalName.Text & "')"
            strSql += " AND ITEMID = 0"
            strSql += " AND TYPE = '" & Mid(cmbType.Text, 1, 1) & "'"
        End If
        Dim costNames As String = GetCheckedItem(chkLstCostcentre)
        If costNames <> Nothing Then strSql += " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & GetCheckedItem(chkLstCostcentre) & "))"
        If TAG_DISCOUNT And cmbDiscountGroup.Text <> "TAG" Then strSql += " AND (RANGEFROM BETWEEN " & txtRangefrom_WET.Text & " AND " & txtRangeTo_WET.Text & " ) AND (RANGETO BETWEEN " & txtRangefrom_WET.Text & " AND " & txtRangeTo_WET.Text & ")"
        If TAG_DISCRANGETYPE <> "N" And TAG_DISCOUNT = False Then strSql += " AND RANGETYPE = '" & TAG_DISCRANGETYPE & "' AND (RANGEFROM BETWEEN " & txtRangefrom_WET.Text & " AND " & txtRangeTo_WET.Text & " ) AND (RANGETO BETWEEN " & txtRangefrom_WET.Text & " AND " & txtRangeTo_WET.Text & ")"
        If objGPack.GetSqlValue(strSql).Length > 0 Then
            MsgBox("This Group Already Exist")
            Exit Sub
        End If
        Dim MulDiscName As String = Nothing
        Dim Frmdate As Date
        Dim Todate As Date
        If MAIN_DISCNAME Then
            If objDisc.Visible Then Exit Sub
            objDisc.BackColor = Me.BackColor
            objDisc.StartPosition = FormStartPosition.CenterScreen
            objDisc.MaximizeBox = False
            objDisc.ShowDialog()
            MulDiscName = objDisc.txtDiscName.Text
            Frmdate = objDisc.dtpFrom.Value
            Todate = objDisc.dtpTo.Value
        End If
        If Not _flagUpdate Then 'SAVE
            If chkLstCostcentre.Items.Count > 0 Then
                For cnt As Integer = 0 To chkLstCostcentre.CheckedItems.Count - 1
                    Insert(objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & chkLstCostcentre.CheckedItems.Item(cnt).ToString & "'"))
                Next
            Else
                Insert("")
            End If
            If MAIN_DISCNAME Then
                InsertIntoMultiDisc(MulDiscName, Frmdate, Todate)
            End If
            MsgBox("Saved..")
        Else
            If chkLstCostcentre.Items.Count > 0 Then
                For cnt As Integer = 0 To chkLstCostcentre.CheckedItems.Count - 1
                    Updat(objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & chkLstCostcentre.CheckedItems.Item(cnt).ToString & "'"))
                Next
            Else
                Updat("")
            End If
            If MAIN_DISCNAME Then
                UpdateMultiDisc(MulDiscName, Frmdate, Todate)
            End If
            MsgBox("Updated..")
        End If
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        objGPack.TextClear(Me)
        tabMain.SelectedTab = tabGeneral
        _DiscId = Val(objGPack.GetSqlValue("SELECT ISNULL(MAX(DISCID),0)+1 AS DISCID FROM " & cnAdminDb & "..DISCMASTER"))
        _flagUpdate = False
        chkLstItem.Items.Clear()
        chkLstSubItem.Items.Clear()
        txtTagNo.Text = ""
        cmbState.Text = "ALL"

        strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE"
        If cmbState.Text <> "ALL" And cmbState.Text <> "" Then
            strSql += " WHERE COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..SYNCCOSTCENTRE WHERE STATEID = (SELECT STATEID FROM " & cnAdminDb & "..STATEMAST WHERE STATENAME = '" & cmbState.Text & "'))"
        End If
        chkLstCostcentre.Items.Clear()
        Dim dt As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        For Each ro As DataRow In dt.Rows
            chkLstCostcentre.Items.Add(ro.Item(0))
        Next

        SetCheckedList(chkLstCostcentre, False)
        SetCheckedList(chkLstItem, False)
        SetCheckedList(chkLstSubItem, False)
        cmbType.Text = "SALES"
        cmbDiscountGroup.Text = "ITEM"
        cmbAfterTax.Text = "NO"
        cmbActive.Text = "YES"
        cmbBasedOn.Text = "NETWT"
        objDisc.txtDiscName.Text = ""
        ChkLessMakingPer.Checked = False
        ChkLessWastPer.Checked = False
        objDisc.dtpFrom.Value = GetServerDate()
        objDisc.dtpTo.Value = GetServerDate()
        cmbItemName_Man.Enabled = True
        cmbSubItem.Enabled = True
        cmbMetalName.Enabled = True
        txtRangefrom_WET.Enabled = True
        txtRangeTo_WET.Enabled = True
        If TAG_DISCRANGE Then
            cmbDiscRange.Enabled = True
            If cmbDiscRange.Text = "DAYS" Then lblRange1.Text = "Stock day from" : lblRange2.Text = "Stock day to"
            If cmbDiscRange.Text = "WASTAGE" Then lblRange1.Text = "Wastage from" : lblRange2.Text = "Wastage to"
            If cmbDiscRange.Text = "FINAL AMOUNT" Then lblRange1.Text = "Amount from" : lblRange2.Text = "Amount to"
            If cmbDiscRange.Text = "GRAM" Then lblRange1.Text = "Weight from" : lblRange2.Text = "Weight to"
        Else
            cmbDiscRange.Enabled = False
        End If
        cmbType.Select()
    End Sub

    Private Sub cmbMetalName_Man_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbMetalName.SelectedIndexChanged
        cmbItemName_Man.Items.Clear()
        cmbSubItem.Items.Clear()
        chkLstItem.Items.Clear()
        chkLstSubItem.Items.Clear()
        If cmbMetalName.Text = "" Then Exit Sub
        strSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE METALID = "
        strSql += " (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetalName.Text & "' AND ACTIVE = 'Y')"
        If cmbDiscountGroup.Text = "ITEM" Then
            objGPack.FillCombo(strSql, cmbItemName_Man, True, False)
        Else
            _DtTemp = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(_DtTemp)
            For Each ro As DataRow In _DtTemp.Rows
                chkLstItem.Items.Add(ro(0).ToString)
            Next
        End If
    End Sub

    Private Sub cmbState_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbState.SelectedIndexChanged
        strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE"
        If cmbState.Text <> "ALL" And cmbState.Text <> "" Then
            strSql += " WHERE COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..SYNCCOSTCENTRE WHERE STATEID = (SELECT STATEID FROM " & cnAdminDb & "..STATEMAST WHERE STATENAME = '" & cmbState.Text & "'))"
        End If
        chkLstCostcentre.Items.Clear()
        Dim dt As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        For Each ro As DataRow In dt.Rows
            chkLstCostcentre.Items.Add(ro.Item(0))
        Next
    End Sub

    Private Sub cmbItemName_Man_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbItemName_Man.SelectedIndexChanged
        cmbSubItem.Items.Clear()
        cmbSubItem.Text = ""
        If cmbItemName_Man.Text = "" Then Exit Sub
        cmbSubItem.Items.Add("ALL")
        strSql = "SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE"
        strSql += " ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "')"
        objGPack.FillCombo(strSql, cmbSubItem, False, False)
        cmbSubItem.Text = "ALL"

    End Sub

    Private Sub chkLstCostcentre_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLstCostcentre.GotFocus
        If _flagUpdate Then Me.SelectNextControl(chkLstCostcentre, True, True, True, True)
    End Sub

    Private Sub ChkLessMakingPer_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkLessMakingPer.LostFocus
        If Not (Val(txtMaking_Per.Text) > 0 Or Val(txtMakingCharge_Amt.Text) > 0) Then
            ChkLessMakingPer.Checked = False
        End If
    End Sub

    Private Sub ChkLessWastPer_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkLessWastPer.LostFocus
        If Not Val(txtLessWastPer_PER.Text) > 0 Then
            ChkLessWastPer.Checked = False
        End If
    End Sub

    Private Sub txtMaking_Per_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMaking_Per.LostFocus
        If Not (Val(txtMaking_Per.Text) > 0 Or Val(txtMakingCharge_Amt.Text) > 0) Then
            ChkLessMakingPer.Checked = False
            ChkLessMakingPer.Enabled = False
        Else
            ChkLessMakingPer.Enabled = True
        End If
    End Sub

    Private Sub txtLessWastPer_PER_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtLessWastPer_PER.LostFocus
        If Not Val(txtLessWastPer_PER.Text) > 0 Then
            ChkLessWastPer.Enabled = False
            ChkLessWastPer.Checked = False
        Else
            ChkLessWastPer.Enabled = True
        End If
    End Sub

    Private Sub txtTagNo_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTagNo.Enter
        If txtTagNo.Text.Trim = "" Then Exit Sub
        strSql = " SELECT A.itemid,b.itemname FROM " & cnAdminDb & "..ITEMTAG A INNER JOIN " & cnAdminDb & "..ITEMMAST B ON A.ITEMID =B.ITEMID"
        strSql += " WHERE TAGNO = '" & txtTagNo.Text & "'"
        Dim dtTagCheck As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtTagCheck)
        If dtTagCheck.Rows.Count <> 0 Then
            cmbItemName_Man.Text = dtTagCheck.Rows(0).Item(1)
        Else
            MsgBox("Given Tag is not found")
            txtTagNo.Focus()
        End If
    End Sub

    Private Sub chkLstItem_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLstItem.LostFocus
        LoadSubitem()
    End Sub
    Private Sub LoadSubitem()
        Dim chkItemNames As String = GetChecked_CheckedList(chkLstItem)
        chkLstSubItem.Items.Clear()
        If chkItemNames <> "" Then
            strSql = ""
            strSql += " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST "
            strSql += " WHERE ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN(" & GetCheckedItem(chkLstItem) & "))"
            strSql += "  AND ISNULL(ACTIVE,'')<>'N'"
            If strSql <> "" Then
                strSql += " ORDER BY SUBITEMNAME"
                FillCheckedListBox(strSql, chkLstSubItem, , False)
            End If
        End If
    End Sub

    Private Sub btnOpen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOpen.Click, OpenToolStripMenuItem.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        CallGRid()
        If Not gridView.RowCount > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        tabMain.SelectedTab = tabView
    End Sub

    Private Sub chkItemSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkItemSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstItem, chkItemSelectAll.Checked)
    End Sub

    Private Sub chkSubItemSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSubItemSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstSubItem, chkSubItemSelectAll.Checked)
    End Sub

    Private Sub cmbDiscRange_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbDiscRange.SelectedIndexChanged
        If TAG_DISCRANGE Then
            cmbDiscRange.Enabled = True
            If cmbDiscRange.Text = "DAYS" Then lblRange1.Text = "Stock day from" : lblRange2.Text = "Stock day to"
            If cmbDiscRange.Text = "WASTAGE" Then lblRange1.Text = "Wastage from" : lblRange2.Text = "Wastage to"
            If cmbDiscRange.Text = "FINAL AMOUNT" Then lblRange1.Text = "Amount from" : lblRange2.Text = "Amount to"
            If cmbDiscRange.Text = "GRAM" Then lblRange1.Text = "Weight from" : lblRange2.Text = "Weight to"
        Else
            cmbDiscRange.Enabled = False
        End If
    End Sub

    Private Sub cmbType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbType.SelectedIndexChanged
        If cmbType.Text = "CUSTOMER PRIVILEGE" Then
            lblPrivilegeType.Visible = True
            cmbprivilegeType.Visible = True
            cmbprivilegeType.Text = ""
        Else
            lblPrivilegeType.Visible = False
            cmbprivilegeType.Visible = False
            cmbprivilegeType.Text = ""
        End If
    End Sub
End Class