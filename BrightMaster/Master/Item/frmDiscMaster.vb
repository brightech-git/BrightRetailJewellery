Imports System.Data.OleDb
Public Class frmDiscMaster
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim flagSave As Boolean = False
    Dim DiscId As Integer = Nothing
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        funcGridStyle(gridView)
    End Sub
    Function funcCallGrid() As Integer
        strSql = " SELECT DISCID,"
        strSql += " CASE WHEN TYPE = 'S' THEN 'SALES' ELSE 'PURCHASE' END AS TYPE,"
        strSql += " CASE WHEN DISCGROUP = 'I' THEN 'ITEM'"
        strSql += " WHEN DISCGROUP = 'M' THEN 'METAL' ELSE 'BOARD RATE' END AS DISCGROUP,"
        strSql += " ISNULL((SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = METAL),'')AS METALNAME,"
        strSql += " ISNULL((SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST AS I WHERE I.ITEMID = D.ITEMID),'')AS ITEMNAME,"
        strSql += " ISNULL((SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST AS S WHERE S.SUBITEMID = D.SUBITEMID),'')AS SUBITEMNAME,"
        strSql += " FINALAMTPER,FINALAMT,WASTAGEPER,"
        strSql += " MAKINGCHARGEGRM,MAKINGCHARGEPER,STUDSTNPER,STUDSTNAMT,"
        strSql += " STUDDIAPER,STUDDIAAMT,BOARDRATE,"
        strSql += " CASE WHEN AFTERTAX = 'Y' THEN 'YES' ELSE 'NO' END AS AFTERTAX,"
        strSql += " CASE WHEN ACTIVE = 'Y' THEN 'YES' ELSE 'NO' END AS ACTIVE"
        strSql += " ,CASE WHEN DISCGRSNET = 'G' THEN 'GRSWT' ELSE 'NETWT' END AS DISCGRSNET"
        strSql += " ,CASE WHEN DISCWITHWAST = 'Y' THEN 'YES' ELSE 'NO' END AS DISCWITHWAST"
        strSql += " ,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = D.COSTID)AS COSTNAME"
        strSql += " FROM " & cnAdminDb & "..DISCMASTER AS D"
        funcOpenGrid(strSql, gridView)
        gridView.Columns("DISCID").Visible = False
        With gridView
            .Columns("DISCGROUP").Width = 80
            .Columns("ITEMNAME").Width = 120
            .Columns("SUBITEMNAME").Width = 120
            With .Columns("FINALAMTPER")
                .HeaderText = "FINAL AMTPER"
                .Width = 70
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("FINALAMT")
                .HeaderText = "FINAL AMT"
                .Width = 70
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("WASTAGEPER")
                .HeaderText = "WASTAGE PER"
                .Width = 70
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("MAKINGCHARGEGRM")
                .HeaderText = "MAKING CHARGE GRM"
                .Width = 70
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("MAKINGCHARGEPER")
                .HeaderText = "MAKING CHARGE PER"
                .Width = 70
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("STUDSTNPER")
                .HeaderText = "STUD STN PER"
                .Width = 70
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("STUDSTNAMT")
                .HeaderText = "STUD STN AMT"
                .Width = 70
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("STUDDIAPER")
                .HeaderText = "STUD DIA PER"
                .Width = 70
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("STUDDIAAMT")
                .HeaderText = "STUD DIA AMT"
                .Width = 70
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("BOARDRATE")
                .HeaderText = "BOARD RATE"
                .Width = 70
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With

            With .Columns("AFTERTAX")
                .HeaderText = "AFTER TAX"
                .Width = 50
            End With
            With .Columns("ACTIVE")
                .HeaderText = "ACTIVE"
                .Width = 50
            End With
            With .Columns("DISCGRSNET")
                .HeaderText = "DISC GRSNET"
                .Width = 70
            End With
            With .Columns("DISCWITHWAST")
                .HeaderText = "DISC WITHWAST"
                .Width = 70
            End With
        End With
    End Function
    Private Sub loadCostCentre()
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
    Function funcNew() As Integer
        objGPack.TextClear(Me)
        DiscId = Nothing
        flagSave = False
        tabMain.SelectedTab = tabGeneral
        funcCallGrid()
        DiscId = Val(objGPack.GetSqlValue("SELECT ISNULL(MAX(DISCID),0)+1 AS DISCID FROM " & cnAdminDb & "..DISCMASTER"))
        cmbState.Text = "ALL"
        cmbType.Text = "SALES"
        cmbDiscountGroup.Text = "ITEM"
        cmbAfterTax.Text = "NO"
        cmbActive.Text = "YES"
        cmbBasedOn.Text = "NetWt"
        pnlControls.Enabled = True
        cmbType.Select()
    End Function
    Function funcExit() As Integer
        Me.Close()
    End Function
    Function funcSave() As Integer
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Function
        If objGPack.Validator_Check(Me) Then Exit Function
        If cmbDiscountGroup.Text = "ITEM" Then
            If cmbItemName_Man.Text = "" Then
                MsgBox(Me.GetNextControl(cmbItemName_Man, False).Text + E0001, MsgBoxStyle.Information)
                cmbItemName_Man.Focus()
                Return 0
            End If
        Else
            If cmbMetalName_Man.Text = "" Then
                MsgBox(Me.GetNextControl(cmbMetalName_Man, False).Text + E0001, MsgBoxStyle.Information)
                cmbMetalName_Man.Focus()
                Return 0
            End If
        End If
        If chkLstCostcentre.Items.Count > 0 Then
            If Not chkLstCostcentre.CheckedItems.Count > 0 Then
                MsgBox("CostCentre selection should not empty", MsgBoxStyle.Information)
                chkLstCostcentre.Focus()
                Exit Function
            End If
        End If

        strSql = " SELECT 'CHECK' FROM " & cnAdminDb & "..DISCMASTER"
        strSql += " WHERE DISCID <> " & DiscId & ""
        If cmbDiscountGroup.Text = "ITEM" Then
            strSql += " AND DISCGROUP = 'I'"
            strSql += " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "')"
            strSql += " AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "')),0)"
            strSql += " AND TYPE = '" & Mid(cmbType.Text, 1, 1) & "'"
        ElseIf cmbDiscountGroup.Text = "METAL" Then
            strSql += " AND DISCGROUP = 'M'"
            strSql += " AND METAL = (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetalName_Man.Text & "')"
            strSql += " AND TYPE = '" & Mid(cmbType.Text, 1, 1) & "'"
        Else 'BOARD RATE
            strSql += " AND DISCGROUP = 'B'"
            strSql += " AND METAL = (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetalName_Man.Text & "')"
            strSql += " AND TYPE = '" & Mid(cmbType.Text, 1, 1) & "'"
        End If
        If chkLstCostcentre.Items.Count > 0 Then
            If chkLstCostcentre.CheckedItems.Count > 0 Then
                strSql += " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN ("
                For cnt As Integer = 0 To chkLstCostcentre.CheckedItems.Count - 1
                    strSql += "'" & chkLstCostcentre.CheckedItems.Item(cnt).ToString & "'"
                    If cnt <> chkLstCostcentre.CheckedItems.Count - 1 Then
                        strSql += ","
                    End If
                Next
                strSql += " ))"
            End If
        End If
        If objGPack.GetSqlValue(strSql).Length > 0 Then
            MsgBox("This Group Already Exist")
            Exit Function
        End If
        Try
            If flagSave = False Then
                If chkLstCostcentre.Items.Count > 0 Then
                    For cnt As Integer = 0 To chkLstCostcentre.CheckedItems.Count - 1
                        funcAdd(objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & chkLstCostcentre.CheckedItems.Item(cnt).ToString & "'"))
                    Next
                Else
                    funcAdd("")
                End If
                MsgBox("Saved..")
            Else
                funcUpdate()
                MsgBox("Updated")
            End If
            funcNew()
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
        Return 0
    End Function
    Function funcAdd(ByVal cId As String) As Integer
        strSql = " INSERT INTO " & cnAdminDb & "..DISCMASTER"
        strSql += " ("
        strSql += " DISCID,DISCGROUP,TYPE,METAL,ITEMID,SUBITEMID,"
        strSql += " FINALAMTPER,FINALAMT,WASTAGEPER,MAKINGCHARGEGRM,"
        strSql += " MAKINGCHARGEPER,STUDSTNPER,STUDDIAPER,BOARDRATE,"
        strSql += " AFTERTAX,ACTIVE,USERID,UPDATED,UPTIME,DISCGRSNET,DISCWITHWAST,STUDSTNAMT,STUDDIAAMT,COSTID"
        strSql += " )VALUES ("
        strSql += " " & Val(objGPack.GetSqlValue("SELECT ISNULL(MAX(DISCID),0)+1 AS DISCID FROM " & cnAdminDb & "..DISCMASTER")) & "" 'DISCID
        strSql += " ,'" & Mid(cmbDiscountGroup.Text, 1, 1) & "'" 'DISCGROUP
        strSql += " ,'" & Mid(cmbType.Text, 1, 1) & "'" ' TYPE
        strSql += " ,'" & objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetalName_Man.Text & "'") & "'" 'METAL
        strSql += " ," & Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "'")) & "" 'ITEMID
        strSql += " ," & Val(objGPack.GetSqlValue("SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "')")) & "" 'SUBITEMID
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
        strSql += " )"
        ExecQuery(SyncMode.Master, strSql, cn)
    End Function
    Function funcUpdate() As Integer
        strSql = " UPDATE " & cnAdminDb & "..DISCMASTER SET"
        strSql += " DISCGROUP='" & Mid(cmbDiscountGroup.Text, 1, 1) & "'"
        strSql += " ,TYPE = '" & Mid(cmbType.Text, 1, 1) & "'"
        strSql += " ,METAL='" & objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetalName_Man.Text & "'") & "'"
        strSql += " ,ITEMID=" & Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "'")) & ""
        strSql += " ,SUBITEMID=" & Val(objGPack.GetSqlValue("SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "')")) & ""
        strSql += " ,FINALAMTPER=" & Val(txtOnFinalAmt_Per.Text) & ""
        strSql += " ,FINALAMT=" & Val(txtOnFinal_Amt.Text) & ""
        strSql += " ,WASTAGEPER=" & Val(txtWastage_Per.Text) & ""
        strSql += " ,MAKINGCHARGEGRM=" & Val(txtMakingCharge_Amt.Text) & ""
        strSql += " ,MAKINGCHARGEPER=" & Val(txtMaking_Per.Text) & ""
        strSql += " ,STUDSTNPER=" & Val(txtStuddedStones_Per.Text) & ""
        strSql += " ,STUDDIAPER=" & Val(txtStuddedDiamond_Per.Text) & ""
        strSql += " ,BOARDRATE=" & Val(txtBoardRate_Amt.Text) & ""
        strSql += " ,AFTERTAX='" & Mid(cmbAfterTax.Text, 1, 1) & "'"
        strSql += " ,ACTIVE='" & Mid(cmbActive.Text, 1, 1) & "'"
        strSql += " ,USERID='" & userId & "'"
        strSql += " ,UPDATED='" & Today.Date.ToString("yyyy-MM-dd") & "'"
        strSql += " ,UPTIME='" & Date.Now.ToLongTimeString & "'"
        strSql += " ,DISCGRSNET = '" & Mid(cmbBasedOn.Text, 1, 1) & "'"
        strSql += " ,DISCWITHWAST = '" & Mid(cmbWithWastage.Text, 1, 1) & "'"
        strSql += " ,STUDSTNAMT = " & Val(txtStuddedStonesRs_AMT.Text) & ""
        strSql += " ,STUDDIAAMT = " & Val(txtStuddedDiamondRs_AMT.Text) & ""
        strSql += " WHERE DISCID = '" & DiscId & "'"
        ExecQuery(SyncMode.Master, strSql, cn)
    End Function
    Function funcGetDetails(ByVal temp As Integer) As Boolean
        strSql = " select DiscId,"
        strSql += " CASE WHEN TYPE = 'S' THEN 'SALES' ELSE 'PURCHASE' END AS TYPE,"
        strSql += " Case when DiscGroup = 'I' then 'ITEM'"
        strSql += " when DiscGroup = 'M' then 'METAL' else 'BOARD RATE' end as DiscGroup,"
        strSql += " isnull((select MetalName from " & cnAdminDb & "..MetalMast where MetalId = Metal),'')as MetalName,"
        strSql += " isnull((select ItemName from " & cnAdminDb & "..ItemMast as i where i.ItemId = D.ItemId),'')as ItemName,"
        strSql += " isnull((select SubItemName from " & cnAdminDb & "..SubItemMast as s where s.SubItemId = D.SubItemId),'')as SubItemName,"
        strSql += " FinalAmtPer,FinalAmt,WastagePer,"
        strSql += " MakingChargeGrm,MakingChargePer,StudStnPer,"
        strSql += " StudDiaPer,BoardRate,"
        strSql += " case when Aftertax = 'Y' then 'YES' else 'NO' end as AfterTax,"
        strSql += " case when Active = 'Y' then 'YES' else 'NO' end as Active"
        strSql += " ,case when DiscGrsNet = 'G' then 'GrsWt' else 'NetWt' end as DiscGrsNet"
        strSql += " ,case when DiscWithWast = 'Y' then 'YES' else 'No' end as DiscWithWast,STUDDIAAMT,STUDSTNAMT"
        strSql += " ,(select costname from " & cnAdminDb & "..costcentre where costid = d.costid)as costname"
        strSql += " from " & cnAdminDb & "..DiscMaster as D"
        strSql += " Where DiscId = '" & temp & "'"
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Return False
        End If
        flagSave = True
        With dt.Rows(0)
            cmbState.Text = "ALL"
            cmbType.Text = .Item("TYPE").ToString
            If .Item("DiscGroup") = "ITEM" Then
                cmbDiscountGroup.Text = .Item("DiscGroup").ToString
                funcLoadItemName()
                cmbItemName_Man.Text = .Item("ItemName").ToString
                cmbSubItem.Text = .Item("SubItemName").ToString
                cmbMetalName_Man.Items.Clear()
            Else
                cmbDiscountGroup.Text = .Item("DiscGroup").ToString
                funcLoadMetalName()
                cmbMetalName_Man.Text = .Item("MetalName").ToString
                cmbItemName_Man.Items.Clear()
                cmbSubItem.Items.Clear()
            End If
            txtOnFinalAmt_Per.Text = IIf(Val(.Item("FinalAmtPer").ToString) <> 0, .Item("FinalAmtPer").ToString, Nothing)
            txtOnFinal_Amt.Text = IIf(Val(.Item("FinalAmt").ToString) <> 0, .Item("FinalAmt").ToString, Nothing)
            txtWastage_Per.Text = IIf(Val(.Item("WastagePer").ToString) <> 0, .Item("WastagePer").ToString, Nothing)
            txtMakingCharge_Amt.Text = IIf(Val(.Item("MakingChargeGrm").ToString) <> 0, .Item("MakingChargeGrm").ToString, Nothing)
            txtMaking_Per.Text = IIf(Val(.Item("MakingChargePer").ToString) <> 0, .Item("MakingChargePer").ToString, Nothing)
            txtStuddedStones_Per.Text = IIf(Val(.Item("StudStnPer").ToString) <> 0, .Item("StudStnPer").ToString, Nothing)
            txtStuddedDiamond_Per.Text = IIf(Val(.Item("StudDiaPer").ToString) <> 0, .Item("StudDiaPer").ToString, Nothing)
            txtBoardRate_Amt.Text = IIf(Val(.Item("BoardRate").ToString) <> 0, .Item("BoardRate").ToString, Nothing)
            cmbAfterTax.Text = .Item("Aftertax").ToString
            cmbActive.Text = .Item("Active").ToString
            cmbBasedOn.Text = .Item("DiscGrsNet").ToString
            cmbWithWastage.Text = .Item("DiscWithWast").ToString
            txtStuddedStonesRs_AMT.Text = IIf(Val(.Item("studstnamt").ToString) <> 0, .Item("studstnamt").ToString, Nothing)
            txtStuddedDiamondRs_AMT.Text = IIf(Val(.Item("studdiaamt").ToString) <> 0, .Item("studdiaamt").ToString, Nothing)
            If chkLstCostcentre.Items.Count > 0 Then
                For cnt As Integer = 0 To chkLstCostcentre.Items.Count - 1
                    If chkLstCostcentre.Items.Item(cnt).ToString = .Item("COSTNAME").ToString Then
                        chkLstCostcentre.SetItemChecked(cnt, True)
                    Else
                        chkLstCostcentre.SetItemChecked(cnt, False)
                    End If
                Next
            End If
        End With
        DiscId = temp
        pnlControls.Enabled = False
        tabMain.SelectedTab = tabGeneral
        txtOnFinalAmt_Per.Select()
        Return True
    End Function
    Function funcLoadMetalName() As Integer
        strSql = " Select MetalName from " & cnAdminDb & "..MetalMast Order by DISPLAYORDER"
        objGPack.FillCombo(strSql, cmbMetalName_Man)
    End Function
    Function funcLoadItemName() As Integer
        cmbItemName_Man.Text = ""
        strSql = " Select ItemName from " & cnAdminDb & "..ItemMast Order by ItemName"
        objGPack.FillCombo(strSql, cmbItemName_Man)
    End Function
    Function funcLoadSubItemName() As Integer
        cmbSubItem.Text = ""
        cmbSubItem.Items.Clear()
        cmbSubItem.Items.Add("ALL")
        strSql = " Select SubItemName from " & cnAdminDb & "..SubItemMast Where ItemId = "
        strSql += " (select ItemId from " & cnAdminDb & "..ItemMast where ItemName = '" & cmbItemName_Man.Text & "' and SubItem = 'Y')"
        strSql += " Order by SubItemName"
        objGPack.FillCombo(strSql, cmbSubItem, False)
        cmbSubItem.Text = "ALL"
    End Function

    Private Sub frmDiscMaster_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If tabMain.SelectedTab.Name = tabView.Name Then
                btnBack_Click(Me, New EventArgs)
            End If
        End If
    End Sub

    Private Sub frmDiscMaster_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If gridView.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub frmDiscMaster_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGeneral.Left, Me.tabGeneral.Top, Me.tabGeneral.Width, Me.tabGeneral.Height))

        cmbType.Items.Add("SALES")
        cmbType.Items.Add("PURCHASE")

        cmbAfterTax.Items.Add("YES")
        cmbAfterTax.Items.Add("NO")
        cmbActive.Items.Add("YES")
        cmbActive.Items.Add("NO")

        cmbDiscountGroup.Items.Add("ITEM")
        cmbDiscountGroup.Items.Add("METAL")
        cmbDiscountGroup.Items.Add("BOARD RATE")

        cmbBasedOn.Items.Add("GrsWT")
        cmbBasedOn.Items.Add("NetWt")
        cmbBasedOn.Text = "NetWt"

        cmbWithWastage.Items.Add("YES")
        cmbWithWastage.Items.Add("NO")
        cmbWithWastage.Text = "NO"

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



        funcNew()
    End Sub


    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        funcSave()
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

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click, OpenToolStripMenuItem.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        funcCallGrid()
        tabMain.SelectedTab = tabView
        gridView.Focus()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        funcNew()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        funcExit()
    End Sub

    Private Sub cmbDiscountGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbDiscountGroup.SelectedIndexChanged
        'If flagSave = True Then
        '    Exit Sub
        'End If
        txtOnFinalAmt_Per.Clear()
        txtOnFinal_Amt.Clear()
        txtWastage_Per.Clear()
        txtMaking_Per.Clear()
        txtMakingCharge_Amt.Clear()
        txtStuddedDiamond_Per.Clear()
        txtStuddedDiamondRs_AMT.Clear()
        txtStuddedStones_Per.Clear()
        txtStuddedStonesRs_AMT.Clear()
        txtBoardRate_Amt.Clear()
        If cmbDiscountGroup.Text = "ITEM" Then
            cmbMetalName_Man.Items.Clear()
            cmbMetalName_Man.Enabled = False
            cmbMetalName_Man.Text = ""
            cmbItemName_Man.Enabled = True
            cmbSubItem.Enabled = True
            funcLoadItemName()
        Else
            cmbItemName_Man.Items.Clear()
            cmbSubItem.Items.Clear()
            cmbItemName_Man.Text = ""
            cmbSubItem.Text = ""
            cmbItemName_Man.Enabled = False
            cmbSubItem.Enabled = False
            cmbMetalName_Man.Enabled = True
            funcLoadMetalName()
        End If

    End Sub

    Private Sub cmbItemName_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbItemName_Man.SelectedIndexChanged
        funcLoadSubItemName()
        strSql = " SELECT STUDDED,DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "'"
        Dim dtTemp As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtTemp)
        If Not dtTemp.Rows.Count > 0 Then Exit Sub
        txtOnFinalAmt_Per.Enabled = False
        txtOnFinal_Amt.Enabled = False
        txtWastage_Per.Enabled = False
        txtMaking_Per.Enabled = False
        txtMakingCharge_Amt.Enabled = False
        txtStuddedDiamond_Per.Enabled = False
        txtStuddedDiamondRs_AMT.Enabled = False
        txtStuddedStones_Per.Enabled = False
        txtStuddedStonesRs_AMT.Enabled = False
        txtOnFinalAmt_Per.Clear()
        txtOnFinal_Amt.Clear()
        txtWastage_Per.Clear()
        txtMaking_Per.Clear()
        txtMakingCharge_Amt.Clear()
        txtStuddedDiamond_Per.Clear()
        txtStuddedDiamondRs_AMT.Clear()
        txtStuddedStones_Per.Clear()
        txtStuddedStonesRs_AMT.Clear()
        txtBoardRate_Amt.Clear()
        With dtTemp.Rows(0)
            If .Item("STUDDED").ToString = "" Then
                txtOnFinalAmt_Per.Enabled = True
                txtOnFinal_Amt.Enabled = True
                txtWastage_Per.Enabled = True
                txtMaking_Per.Enabled = True
                txtMakingCharge_Amt.Enabled = True
            ElseIf .Item("DIASTONE").ToString = "D" Then
                txtStuddedDiamond_Per.Enabled = True
                txtStuddedDiamondRs_AMT.Enabled = True
            ElseIf .Item("DIASTONE").ToString = "S" Then
                txtStuddedStones_Per.Enabled = True
                txtStuddedStonesRs_AMT.Enabled = True
            End If
        End With

    End Sub

    Private Sub gridView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.GotFocus
        lblStatus.Visible = True
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
                If gridView.RowCount > 0 Then
                If funcGetDetails(gridView.Item(0, gridView.CurrentRow.Index).Value.ToString) Then
                    txtOnFinalAmt_Per.Focus()
                Else
                    cmbDiscountGroup.Focus()
                End If
            End If
        ElseIf e.KeyCode = Keys.Escape Then
            If e.KeyCode = Keys.Escape Then
                If pnlControls.Enabled = True Then
                    cmbDiscountGroup.Focus()
                Else
                    txtOnFinalAmt_Per.Focus()
                End If
            End If
        End If
    End Sub

    Private Sub gridView_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.LostFocus
        lblStatus.Visible = False
    End Sub

    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If Not gridView.RowCount > 0 Then
            Exit Sub
        End If
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
        Dim chkQry As String = "SELECT 1 FROM " & cnAdminDb & "..DISCMASTER WHERE 1<>1"
        Dim delKey As String = gridView.Rows(gridView.CurrentRow.Index).Cells("DISCID").Value.ToString
        DeleteItem(SyncMode.Master, chkQry, "DELETE FROM " & cnAdminDb & "..DISCMASTER WHERE DISCID = '" & delKey & "'")
        funcCallGrid()
    End Sub

    Private Sub txtBoardRate_Amt_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBoardRate_Amt.GotFocus
        If cmbDiscountGroup.Text <> "BOARD RATE" Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtOnFinalAmt_Per_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOnFinalAmt_Per.GotFocus
        If cmbDiscountGroup.Text = "BOARD RATE" Then
            SendKeys.Send("{TAB}")
            Exit Sub
        End If
    End Sub

    Private Sub txtOnFinalAmt_Per_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOnFinalAmt_Per.TextChanged
        txtOnFinal_Amt.Clear()
    End Sub

    Private Sub txtOnFinal_Amt_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOnFinal_Amt.GotFocus
        If cmbDiscountGroup.Text = "BOARD RATE" Then
            SendKeys.Send("{TAB}")
            Exit Sub
        End If
        If Val(txtOnFinalAmt_Per.Text) > 0 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtMaking_Per_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMaking_Per.GotFocus
        If cmbDiscountGroup.Text = "BOARD RATE" Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtMaking_Per_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMaking_Per.TextChanged
        txtMakingCharge_Amt.Clear()
    End Sub

    Private Sub txtMakingCharge_Amt_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMakingCharge_Amt.GotFocus
        If cmbDiscountGroup.Text = "BOARD RATE" Then
            SendKeys.Send("{TAB}")
            Exit Sub
        End If
        If Val(txtMaking_Per.Text) > 0 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtStuddedDiamond_Per_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtStuddedDiamond_Per.GotFocus
        If cmbDiscountGroup.Text = "BOARD RATE" Then
            SendKeys.Send("{TAB}")
            Exit Sub
        End If
    End Sub


    Private Sub txtStuddedDiamond_Per_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtStuddedDiamond_Per.TextChanged
        txtStuddedDiamondRs_AMT.Clear()
    End Sub

    Private Sub txtStuddedDiamondRs_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtStuddedDiamondRs_AMT.GotFocus
        If cmbDiscountGroup.Text = "BOARD RATE" Then
            SendKeys.Send("{TAB}")
            Exit Sub
        End If
        If Val(txtStuddedDiamond_Per.Text) > 0 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub cmbBasedOn_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbBasedOn.GotFocus
        If cmbDiscountGroup.Text <> "BOARD RATE" Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub cmbWithWastage_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbWithWastage.GotFocus
        If cmbDiscountGroup.Text <> "BOARD RATE" Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtStuddedStones_Per_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtStuddedStones_Per.GotFocus
        If cmbDiscountGroup.Text = "BOARD RATE" Then
            SendKeys.Send("{TAB}")
            Exit Sub
        End If
    End Sub

    Private Sub txtStuddedStones_Per_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtStuddedStones_Per.TextChanged
        txtStuddedStonesRs_AMT.Clear()
    End Sub

    Private Sub txtStuddedStonesRs_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtStuddedStonesRs_AMT.GotFocus
        If cmbDiscountGroup.Text = "BOARD RATE" Then
            SendKeys.Send("{TAB}")
            Exit Sub
        End If
        If Val(txtStuddedStones_Per.Text) > 0 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtWastage_Per_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtWastage_Per.GotFocus
        If cmbDiscountGroup.Text = "BOARD RATE" Then
            SendKeys.Send("{TAB}")
            Exit Sub
        End If
    End Sub

    Private Sub cmbState_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbState.GotFocus
        If flagSave Then
            Me.SelectNextControl(chkLstCostcentre, True, True, True, True)
        End If
    End Sub

    Private Sub cmbState_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbState.SelectedIndexChanged
        loadCostCentre()
    End Sub

    Private Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGeneral
        cmbType.Select()
    End Sub

    Private Sub chkLstCostcentre_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLstCostcentre.GotFocus
        If flagSave Then
            Me.SelectNextControl(chkLstCostcentre, True, True, True, True)
        End If
    End Sub
End Class