Imports System.Data.OleDb
Public Class frmTagDiscount
    Dim _Cmd As OleDbCommand
    Dim strSql As String
    Dim _Da As OleDbDataAdapter
    Dim _DtTemp As DataTable
    Dim _DiscId As Integer = Nothing
    Dim _flagUpdate As Boolean
    Dim objDisc As New frmDisc_Name


    Private Sub frmTagDiscount_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If tabMain.SelectedTab.Name = tabView.Name Then btnBack_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub frmTagDiscount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If gridView.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmTagDiscount_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGeneral.Left, Me.tabGeneral.Top, Me.tabGeneral.Width, Me.tabGeneral.Height))

        cmbAfterTax.Items.Add("YES")
        cmbAfterTax.Items.Add("NO")
        cmbActive.Items.Add("YES")
        cmbActive.Items.Add("NO")

        cmbBasedOn.Items.Add("GRSWT")
        cmbBasedOn.Items.Add("NETWT")
        cmbBasedOn.Text = "NETWT"

        cmbWithWastage.Items.Add("YES")
        cmbWithWastage.Items.Add("NO")
        cmbWithWastage.Text = "NO"

        strSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST ORDER BY DISPLAYORDER"
        objGPack.FillCombo(strSql, cmbMetalName_Man, , False)

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
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub SetCheckedList(ByVal chklst As CheckedListBox, ByVal bool As Boolean)
        For cnt As Integer = 0 To chklst.Items.Count - 1
            chklst.SetItemChecked(cnt, bool)
        Next
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        objGPack.TextClear(Me)
        tabMain.SelectedTab = tabGeneral
        _DiscId = Val(objGPack.GetSqlValue("SELECT ISNULL(MAX(DISCID),0)+1 AS DISCID FROM " & cnAdminDb & "..TAGDISCMASTER"))
        _flagUpdate = False

        cmbState.Text = "ALL"
        SetCheckedList(chkLstCostcentre, False)
        
        cmbAfterTax.Text = "NO"
        cmbActive.Text = "YES"
        cmbBasedOn.Text = "NETWT"
        objDisc.txtDiscName.Text = ""
        objDisc.dtpFrom.Value = GetServerDate()
        objDisc.dtpTo.Value = GetServerDate()
        cmbMetalName_Man.Select()
    End Sub

    Private Sub cmbDiscountGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        pnlItemGroup.Visible = False
        pnlItemGroup.Enabled = False
        cmbMetalName_Man.Text = ""
        pnlControls.Location = New Point(258, 22 + pnlItemGroup.Height)
        pnlItemGroup.Visible = True
        pnlItemGroup.Enabled = True
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub CallGRid()
        gridView.DataSource = Nothing
        strSql = " SELECT "
        strSql += " (SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = D.COSTID)AS COSTNAME"
        strSql += " ,DISCID,RANGEFROM,RANGETO"
        strSql += " ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = D.METAL)AS METAL"
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
        strSql += " FROM " & cnAdminDb & "..TAGDISCMASTER D"
        Dim dtGrid As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        gridView.DataSource = dtGrid
        gridView.Columns("COSTNAME").Visible = chkLstCostcentre.Enabled
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
    End Sub

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click, OpenToolStripMenuItem.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        CallGRid()
        If Not gridView.RowCount > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        tabMain.SelectedTab = tabView
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click, SaveToolStripMenuItem.Click
        ''VALIDATION
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
        If objGPack.Validator_Check(Me) Then Exit Sub
        If cmbItemName_Man.Text = "" Then
            MsgBox(Me.GetNextControl(cmbItemName_Man, False).Text + E0001, MsgBoxStyle.Information)
            cmbItemName_Man.Focus()
            Exit Sub
        End If
        If cmbMetalName_Man.Text = "" Then
            MsgBox(Me.GetNextControl(cmbMetalName_Man, False).Text + E0001, MsgBoxStyle.Information)
            cmbMetalName_Man.Focus()
            Exit Sub
        End If
        If Val(txtOnFinalAmt_Per.Text) = 0 And Val(txtOnFinal_Amt.Text) = 0 And _
        Val(txtWastage_Per.Text) = 0 And Val(txtMaking_Per.Text) = 0 And _
        Val(txtMakingCharge_Amt.Text) = 0 And Val(txtStuddedStones_Per.Text) = 0 And _
        Val(txtStuddedStonesRs_AMT.Text) = 0 And Val(txtStuddedDiamond_Per.Text) = 0 And _
        Val(txtStuddedDiamondRs_AMT.Text) = 0 And Val(txtBoardRate_Amt.Text) = 0 And Val(txtLessWastPer_PER.Text) = 0 Then
            txtOnFinalAmt_Per.Focus()
            Exit Sub
        End If

        If chkLstCostcentre.Items.Count > 0 Then
            If Not chkLstCostcentre.CheckedItems.Count > 0 Then
                MsgBox("CostCentre selection should not empty", MsgBoxStyle.Information)
                chkLstCostcentre.Focus()
                Exit Sub
            End If
        End If
        strSql = " SELECT 'CHECK' FROM " & cnAdminDb & "..TAGDISCMASTER"
        strSql += " WHERE DISCID <> " & _DiscId & ""
        strSql += " AND METAL = (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetalName_Man.Text & "')"
        strSql += " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "')"
        strSql += " AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem.Text & "' AND ITEMID = (sELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "')),0)"
        strSql += " AND RANGEFROM=" & Val(txtRangefrom_NUM.Text) & " AND RANGETO=" & Val(txtRangeTo_NUM.Text) & " "
        Dim costNames As String = GetCheckedItem(chkLstCostcentre)
        If costNames <> Nothing Then strSql += " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & GetCheckedItem(chkLstCostcentre) & "))"
        If objGPack.GetSqlValue(strSql).Length > 0 Then
            MsgBox("This Group Already Exist")
            Exit Sub
        End If

        If Not _flagUpdate Then 'SAVE
            If chkLstCostcentre.Items.Count > 0 Then
                For cnt As Integer = 0 To chkLstCostcentre.CheckedItems.Count - 1
                    Insert(objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & chkLstCostcentre.CheckedItems.Item(cnt).ToString & "'"))
                Next
            Else
                Insert("")
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
            MsgBox("Updated..")
        End If
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub InsertIntoDiscMaster(ByVal cId As String, ByVal itemId As Integer, ByVal subItemId As Integer)
        strSql = " INSERT INTO " & cnAdminDb & "..TAGDISCMASTER"
        strSql += " ("
        strSql += " DISCID,RANGEFROM,RANGETO,METAL,ITEMID,SUBITEMID,"
        strSql += " FINALAMTPER,FINALAMT,WASTAGEPER,MAKINGCHARGEGRM,"
        strSql += " MAKINGCHARGEPER,STUDSTNPER,STUDDIAPER,BOARDRATE,"
        strSql += " AFTERTAX,ACTIVE,USERID,UPDATED,UPTIME,DISCGRSNET,DISCWITHWAST,STUDSTNAMT,STUDDIAAMT,COSTID"
        strSql += " ,LESSWASTAGEPER)VALUES ("
        strSql += " " & _DiscId & "" 'DISCID
        strSql += " ," & Val(txtRangefrom_NUM.Text) & " " 'RANGEFROM
        strSql += " ," & Val(txtRangeTo_NUM.Text) & "" ' RANGETO
        strSql += " ,'" & objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & itemId & "", , , tran) & "'" 'METAL
        strSql += " ," & itemId & "" 'ITEMID
        strSql += " ," & subItemId & "" 'SUBITEMID
        strSql += " ," & Val(txtOnFinalAmt_Per.Text) & "" 'FINALAMTPER
        strSql += " ," & Val(txtOnFinal_Amt.Text) & "" 'FINALAMT
        strSql += " ," & Val(txtWastage_Per.Text) & "" 'WASTAGEPER
        strSql += " ," & Val(txtMakingCharge_Amt.Text) & "" 'MAKINGCHARGEGRM
        strSql += " ," & Val(txtMaking_Per.Text) & "" 'MAKINGCHARGEPER
        strSql += " ," & Val(txtStuddedDiamond_Per.Text) & "" 'STUDSTNPER
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
        strSql += " )"
        ExecQuery(SyncMode.Master, strSql, cn, tran, cId)
    End Sub

    Function Updat(ByVal cId As String) As Integer
        strSql = " UPDATE " & cnAdminDb & "..TAGDISCMASTER SET"
        strSql += " RANGEFROM=" & Val(txtRangefrom_NUM.Text) & "" 'RANGE FROM
        strSql += " ,RANGETO=" & Val(txtRangeTo_NUM.Text) & "" 'RANGE TO
        strSql += " ,FINALAMTPER = " & Val(txtOnFinalAmt_Per.Text) & "" 'FINALAMTPER
        strSql += " ,FINALAMT = " & Val(txtOnFinal_Amt.Text) & "" 'FINALAMT
        strSql += " ,WASTAGEPER = " & Val(txtWastage_Per.Text) & "" 'WASTAGEPER
        strSql += " ,MAKINGCHARGEGRM = " & Val(txtMakingCharge_Amt.Text) & "" 'MAKINGCHARGEGRM
        strSql += " ,MAKINGCHARGEPER = " & Val(txtMaking_Per.Text) & "" 'MAKINGCHARGEPER
        strSql += " ,STUDSTNPER = " & Val(txtStuddedDiamond_Per.Text) & "" 'STUDSTNPER
        strSql += " ,STUDDIAPER = " & Val(txtStuddedDiamond_Per.Text) & "" 'STUDDIAPER
        strSql += " ,BOARDRATE = " & Val(txtBoardRate_Amt.Text) & "" 'BOARDRATE
        strSql += " ,AFTERTAX = '" & Mid(cmbAfterTax.Text, 1, 1) & "'" 'AFTERTAX
        strSql += " ,ACTIVE = '" & Mid(cmbActive.Text, 1, 1) & "'" 'ACTIVE
        strSql += " ,USERID = " & userId & "" 'USERID
        strSql += " ,DISCGRSNET = '" & Mid(cmbBasedOn.Text, 1, 1) & "'" 'DISCGRSNET
        strSql += " ,DISCWITHWAST = '" & Mid(cmbWithWastage.Text, 1, 1) & "'" 'DISCWITHWAST
        strSql += " ,STUDSTNAMT = " & Val(txtStuddedStonesRs_AMT.Text) & "" 'STUDSTNAMT
        strSql += " ,STUDDIAAMT = " & Val(txtStuddedDiamondRs_AMT.Text) & "" 'STUDDIAAMT
        strSql += " ,LESSWASTAGEPER = " & Val(txtLessWastPer_PER.Text) & "" 'LESSWASTAGEPER
        strSql += " WHERE DISCID = " & _DiscId & " AND COSTID = '" & cId & "'"
        ExecQuery(SyncMode.Master, strSql, cn, tran, cId)
    End Function

    Function Insert(ByVal cId As String) As Integer
        Dim itemId As Integer
        Dim subItemId As Integer
        itemId = Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "'", , , tran))
        subItemId = Val(objGPack.GetSqlValue("SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem.Text & "' AND ITEMID = " & itemId & "", , , tran))
        InsertIntoDiscMaster(cId, itemId, subItemId)
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

    Private Sub cmbMetalName_Man_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbMetalName_Man.SelectedIndexChanged
        cmbItemName_Man.Items.Clear()
        cmbSubItem.Items.Clear()
        If cmbMetalName_Man.Text = "" Then Exit Sub
        strSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE METALID = "
        strSql += " (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetalName_Man.Text & "' AND ACTIVE = 'Y')"
        objGPack.FillCombo(strSql, cmbItemName_Man, True, False)

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

    Private Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGeneral
        txtOnFinalAmt_Per.Select()
    End Sub

    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If Not gridView.RowCount > 0 Then
            Exit Sub
        End If
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
        Dim chkQry As String = "SELECT 1 FROM " & cnAdminDb & "..TAGDISCMASTER WHERE 1<>1"
        Dim delKey As String = gridView.Rows(gridView.CurrentRow.Index).Cells("DISCID").Value.ToString
        DeleteItem(SyncMode.Master, chkQry, "DELETE FROM " & cnAdminDb & "..TAGDISCMASTER WHERE DISCID = '" & delKey & "'")
        CallGRid()
        If gridView.RowCount > 0 Then gridView.Focus() Else btnBack_Click(Me, New EventArgs)
    End Sub

    Private Sub chkLstCostcentre_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLstCostcentre.GotFocus
        If _flagUpdate Then Me.SelectNextControl(chkLstCostcentre, True, True, True, True)
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
                cmbMetalName_Man.Text = .Cells("METAL").Value.ToString
                If cmbState.Enabled Then cmbState.Text = "ALL"
                chkLstCostcentre.Items.Clear()
                _DiscId = Val(.Cells("DISCID").Value.ToString)

                strSql = " SELECT "
                strSql += " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = D.ITEMID)AS ITEM"
                strSql += " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = D.SUBITEMID)AS SUBITEM"
                strSql += " FROM " & cnAdminDb & "..TAGDISCMASTER AS D WHERE DISCID = " & _DiscId & ""
                _DtTemp = New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(_DtTemp)
                cmbItemName_Man.Text = _DtTemp.Rows(0).Item("ITEM").ToString
                cmbSubItem.Text = _DtTemp.Rows(0).Item("SUBITEM").ToString

                strSql = " SELECT "
                strSql += " DISTINCT (SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = D.COSTID)AS COSTNAME"
                strSql += " FROM " & cnAdminDb & "..TAGDISCMASTER AS D WHERE DISCID = " & _DiscId & ""
                _DtTemp = New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(_DtTemp)
                For Each ro As DataRow In _DtTemp.Rows
                    chkLstCostcentre.Items.Add(ro("COSTNAME").ToString)
                    chkLstCostcentre.SetItemChecked(chkLstCostcentre.Items.Count - 1, True)
                Next

                txtRangefrom_NUM.Text = IIf(Val(.Cells("RANGEFROM").Value.ToString) <> 0, .Cells("RANGEFROM").Value.ToString, Nothing)
                txtRangeTo_NUM.Text = IIf(Val(.Cells("RANGETO").Value.ToString) <> 0, .Cells("RANGETO").Value.ToString, Nothing)

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
                cmbAfterTax.Text = .Cells("AFTERTAX").Value.ToString
                cmbActive.Text = .Cells("ACTIVE").Value.ToString
                cmbBasedOn.Text = .Cells("GRSNET").Value.ToString
                cmbWithWastage.Text = .Cells("WITHWAST").Value.ToString
                _flagUpdate = True
                btnBack_Click(Me, New EventArgs)
            End With
        End If

    End Sub
    Private Sub skip_Gotfocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbMetalName_Man.Enter, cmbItemName_Man.Enter, cmbSubItem.Enter _
    , chkLstCostcentre.Enter
        If _flagUpdate Then Me.SelectNextControl(CType(sender, Control), True, True, True, True)
    End Sub

    Private Sub txtRangeTo_NUM_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRangeTo_NUM.Leave
        strSql = " SELECT 'CHECK' FROM " & cnAdminDb & "..TAGDISCMASTER"
        strSql += " WHERE DISCID <> " & _DiscId & ""
        strSql += " AND METAL = (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetalName_Man.Text & "')"
        strSql += " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "')"
        strSql += " AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem.Text & "' AND ITEMID = (sELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "')),0)"
        strSql += " AND RANGEFROM=" & Val(txtRangefrom_NUM.Text) & " AND RANGETO=" & Val(txtRangeTo_NUM.Text) & " "
        Dim costNames As String = GetCheckedItem(chkLstCostcentre)
        If costNames <> Nothing Then strSql += " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & GetCheckedItem(chkLstCostcentre) & "))"
        If objGPack.GetSqlValue(strSql).Length > 0 Then
            MsgBox("This Group Already Exist")
            txtRangeTo_NUM.Focus()
            Exit Sub
        End If
    End Sub
End Class