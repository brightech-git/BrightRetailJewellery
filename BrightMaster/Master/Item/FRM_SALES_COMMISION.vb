Imports System.Data.OleDb
Public Class FRM_SALES_COMMISION
    Dim StrSql As String
    Dim Da As OleDbDataAdapter
    Dim Tran As OleDbTransaction = Nothing
    Dim UPDATE_SNO As Integer = 0
    Dim Costcentre As Boolean = IIf(GetAdmindbSoftValue("COSTCENRE", "") = "Y", True, False)
    Dim Comm_Costcentre_Based As Boolean = IIf(GetAdmindbSoftValue("COMM_COSTCENTRE_BASED", "") = "Y", True, False)

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()
        'dtpFromDate_OWN.Value = Today
        'dtpToDate_OWN.Value = Today
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub FRM_SALES_COMMISION_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            If gridView.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        ElseIf e.KeyCode = Keys.F1 Then
            btnNew_Click(Me, New EventArgs)
        ElseIf e.KeyCode = Keys.F2 Then
            btnOpen_Click(Me, New EventArgs)
        ElseIf e.KeyCode = Keys.F3 Then
            btnNew_Click(Me, New EventArgs)
        ElseIf e.KeyCode = Keys.F12 Then
            btnExit_Click(Me, New EventArgs)
        ElseIf e.KeyCode = Keys.Escape Then
            If tabMain.SelectedTab.Name = tabView.Name Then btnBack_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub FRM_SALES_COMMISION_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGeneral.Left, Me.tabGeneral.Top, Me.tabGeneral.Width, Me.tabGeneral.Height))
        grpContainer.Location = New Point((ScreenWid - grpContainer.Width) / 2, ((ScreenHit - 128) - grpContainer.Height) / 2)
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub LoadCounter(ByVal CmbCounter As ComboBox)
        CmbCounter.Items.Clear()
        StrSql = " SELECT 'ALL' ITEMCTRNAME,0 RESULT UNION ALL SELECT ITEMCTRNAME,1 RESULT FROM " & cnAdminDb & "..ITEMCOUNTER ORDER BY RESULT,ITEMCTRNAME"
        objGPack.FillCombo(StrSql, CmbCounter, False, False)
        CmbCounter.Text = "ALL"
    End Sub

    Private Sub LoadCostcentre(ByVal CmbCounter As ComboBox, ByVal DefalultCostId As String)
        CmbCounter.Items.Clear()
        StrSql = " SELECT 'ALL' COSTNAME,'' COSTID,0 RESULT UNION ALL SELECT COSTNAME,COSTID,1 RESULT FROM " & cnAdminDb & "..COSTCENTRE ORDER BY RESULT,COSTNAME"
        objGPack.FillCombo(StrSql, cmbCostcentre, False, False)
        If DefalultCostId = "" Then
            cmbCostcentre.Text = "ALL"
        Else
            cmbCostcentre.Text = GetSqlValue(cn, "SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = '" & DefalultCostId & "'")
        End If
    End Sub
    Private Sub LoadvCostcenter(ByVal CmbCounter As ComboBox, ByVal DefalultCostId As String)
        CmbCounter.Items.Clear()
        StrSql = " SELECT 'ALL' COSTNAME,'' COSTID,0 RESULT UNION ALL SELECT COSTNAME,COSTID,1 RESULT FROM " & cnAdminDb & "..COSTCENTRE ORDER BY RESULT,COSTNAME"
        objGPack.FillCombo(StrSql, cmbvcostcenter, False, False)
        If DefalultCostId = "" Then
            cmbvcostcenter.Text = "ALL"
        Else
            cmbvcostcenter.Text = GetSqlValue(cn, "SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = '" & DefalultCostId & "'")
        End If
    End Sub

    Private Sub LoadItem(ByVal CmbItem As ComboBox)
        CmbItem.Items.Clear()
        StrSql = " SELECT 'ALL' ITEMNAME,0 RESULT "
        StrSql += " UNION ALL "
        StrSql += " SELECT ITEMNAME,1 RESULT FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ACTIVE,'')<>'N' "
        If cmbMetalType.Text <> "ALL" And cmbMetalType.Text <> "" Then
            StrSql += " AND METALID IN(SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME='" & cmbMetalType.Text & "')"
        End If
        StrSql += " ORDER BY RESULT,ITEMNAME"
        objGPack.FillCombo(StrSql, CmbItem, False, False)
        CmbItem.Text = "ALL"
    End Sub
    Private Sub Loadmetal(ByVal cmbMetalType As ComboBox)
        cmbMetalType.Items.Clear()
        StrSql = " SELECT 'ALL' METALNAME,0 RESULT "
        StrSql += " UNION ALL "
        StrSql += " SELECT METALNAME,1 RESULT  FROM " & cnAdminDb & "..METALMAST WHERE ISNULL(ACTIVE,'')<>'N'"
        StrSql += " ORDER BY RESULT ,METALNAME "
        objGPack.FillCombo(StrSql, cmbMetalType, False, False)
        cmbMetalType.Text = "ALL"
    End Sub

    Private Sub LoadCmbBasedOn(ByVal CmbBasedOn As ComboBox)
        CmbBasedOn.Items.Clear()
        CmbBasedOn.Items.Add("ALL")
        CmbBasedOn.Items.Add("WEIGHT")
        CmbBasedOn.Items.Add("VALUE")
        CmbBasedOn.Items.Add("PCS")
        CmbBasedOn.Items.Add("TAG")
        CmbBasedOn.Items.Add("AGE")
    End Sub

    Private Sub LoadSubItem()
        chkcmbsubitem.Items.Clear()
        StrSql = " SELECT 'ALL' SUBITEMNAME,'ALL' SUBITEMID,1 RESULT"
        StrSql += " UNION ALL"
        StrSql += " SELECT SUBITEMNAME,CONVERT(vARCHAR,SUBITEMID),2 RESULT FROM " & cnAdminDb & "..SUBITEMMAST WHERE ISNULL(ACTIVE,'')<>'N' "
        StrSql += " AND ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbItem_MAN.Text.ToString & "')"
        StrSql += " ORDER BY RESULT,SUBITEMNAME"
        Dim dtsubitem = New DataTable
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dtsubitem)
        BrighttechPack.GlobalMethods.FillCombo(chkcmbsubitem, dtsubitem, "SUBITEMNAME", , "ALL")
    End Sub
    Private Sub LoadSubItemOpen()
        cmbOpenSubItem.Items.Clear()
        StrSql = " SELECT 'ALL' SUBITEMNAME,'ALL' SUBITEMID,1 RESULT"
        StrSql += " UNION ALL"
        StrSql += " SELECT SUBITEMNAME,CONVERT(vARCHAR,SUBITEMID),2 RESULT FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbOpenItem.Text.ToString & "')"
        StrSql += " ORDER BY RESULT,SUBITEMNAME"
        Dim dtsubitem As New DataTable
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dtsubitem)

        For i As Integer = 0 To dtsubitem.Rows.Count - 1
            cmbOpenSubItem.Items.Add(dtsubitem.Rows(i).Item(0))
        Next
        'BrighttechPack.GlobalMethods.FillCombo(cmbOpenSubItem, dtsubitem, "SUBITEMNAME", , "ALL")
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
        'If objGPack.Validator_Check(Me) Then Exit Sub

        Dim ItemCtrId As Integer
        Dim ItemId As Integer
        Dim SubItemId As String
        Dim _Costid As String = ""
        If cmbCounter_MAN.Text <> "ALL" And cmbCounter_MAN.Text <> "" Then ItemCtrId = Val(objGPack.GetSqlValue("SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & cmbCounter_MAN.Text & "'"))
        If cmbItem_MAN.Text <> "ALL" And cmbItem_MAN.Text <> "" Then ItemId = Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'"))
        If chkcmbsubitem.Text <> "ALL" And chkcmbsubitem.Text <> "" Then SubItemId = GetSelectedSubitemid(chkcmbsubitem, False, True, ItemId)
        If chkcmbsubitem.Text = "ALL" Then SubItemId = GetSelectedSubitemid(chkcmbsubitem, False, False, ItemId)
        If ItemCtrId = 0 And ItemId = 0 Then
            MsgBox("You must select Counter or Item", MsgBoxStyle.Information)
            cmbCounter_MAN.Select()
            Exit Sub
        End If
        If ValidateToWeight() = False Then Exit Sub
        If rbtTag.Checked = False And rbtRecdate.Checked = False And rbtAge.Checked = False Then
            If Val(txtFrom_WET.Text) = 0 Then
                MsgBox("From Value should not empty", MsgBoxStyle.Information)
                txtCommFlat_AMT.Focus()
                Exit Sub
            End If
            If Val(txtTo_WET.Text) = 0 Then
                MsgBox("To Value should not empty", MsgBoxStyle.Information)
                txtCommFlat_AMT.Focus()
                Exit Sub
            End If
        End If

        'If rbtAge.Checked = False Then
        '    If Val(txtAgeFrom_NUM.Text) = 0 Then
        '        MsgBox("From Value should not empty", MsgBoxStyle.Information)
        '        txtAgeFrom_NUM.Focus()
        '        Exit Sub
        '    End If
        '    If Val(txtAgeTo_NUM.Text) = 0 Then
        '        MsgBox("To Value should not empty", MsgBoxStyle.Information)
        '        txtAgeTo_NUM.Focus()
        '        Exit Sub
        '    End If
        'End If

        If Val(txtCommFlat_AMT.Text) = 0 And Val(txtCommPerGrm_AMT.Text) = 0 And Val(txtCommPercentage_AMT.Text) = 0 Then
            MsgBox("Commision Amount should not empty", MsgBoxStyle.Information)
            txtCommFlat_AMT.Focus()
            Exit Sub
        End If
        Dim entVal As Integer = 0
        If Val(txtCommFlat_AMT.Text) <> 0 Then entVal += 1
        If Val(txtCommPerGrm_AMT.Text) <> 0 Then entVal += 1
        If Val(txtCommPercentage_AMT.Text) <> 0 Then entVal += 1

        'If entVal <> 1 Then
        '    MsgBox("Only one commision value should accept", MsgBoxStyle.Information)
        '    txtCommFlat_AMT.Focus()
        '    Exit Sub
        'End If
        If UPDATE_SNO <> Nothing Then
            StrSql = " SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME='" & chkcmbsubitem.Text.ToString & "' "
            SubItemId = GetSqlValue(cn, StrSql)
        End If
        If Comm_Costcentre_Based And cmbCostcentre.Text <> "" And cmbCostcentre.Text <> "ALL" Then
            _Costid = GetSqlValue(cn, " SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME= '" & cmbCostcentre.Text & "'")
        Else
            _Costid = ""
        End If

        If UPDATE_SNO <> Nothing Then GoTo INS
        Dim Row As DataRow = Nothing
        If rbtTag.Checked Then
            StrSql = " SELECT SUBITEMID ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = I.SUBITEMID) SUBITEMNAME,TAGNO FROM " & cnAdminDb & "..ITEMTAG I WHERE ITEMID='" & ItemId & "' AND TAGNO = '" & txtTagno.Text & "'"
        ElseIf rbtRecdate.Checked Then
            StrSql = " SELECT (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID)ITEMNAME,SUBITEMID,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = I.SUBITEMID) SUBITEMNAME "
            StrSql += vbCrLf + " ,RECDATE_FROM,RECDATE_TO"
            StrSql += vbCrLf + " FROM " & cnAdminDb & "..SALES_COMMISION I "
            StrSql += vbCrLf + " WHERE ITEMID='" & ItemId & "'"
            If chkcmbsubitem.Text <> "ALL" And chkcmbsubitem.CheckedItems.Count > 0 Then StrSql += vbCrLf + " AND SUBITEMID IN(" & GetSelectedSubitemid(chkcmbsubitem, True, True, ItemId) & ")"
        Else
            If chkcmbsubitem.Text = "ALL" Or chkcmbsubitem.Text = "" Then StrSql = " SELECT SUBITEMID,SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID='" & ItemId & "'"
            If chkcmbsubitem.Text <> "ALL" And chkcmbsubitem.CheckedItems.Count > 0 Then
                StrSql = " SELECT SUBITEMID,SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID='" & ItemId & "' AND SUBITEMID IN(" & GetSelectedSubitemid(chkcmbsubitem, True, True, ItemId) & ")"
            End If
        End If

        Dim dt As New DataTable
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dt)
        If dt.Rows.Count > 0 Then
            For i As Integer = 0 To dt.Rows.Count - 1
                StrSql = " SELECT COMMISION FROM " & cnAdminDb & "..SALES_COMMISION"
                StrSql += " WHERE ITEMCTRID = " & ItemCtrId & " AND ITEMID = " & ItemId & " AND SUBITEMID = " & dt.Rows(i).Item("SUBITEMID").ToString & ""
                If rbtWeight.Checked Then
                    StrSql += " AND BASEDON = 'W'"
                ElseIf rbtValue.Checked Then
                    StrSql += " AND BASEDON = 'V'"
                ElseIf rbtPcs.Checked Then
                    StrSql += " AND BASEDON = 'P'"
                ElseIf rbtTag.Checked Then
                    StrSql += " AND BASEDON = 'T'"
                ElseIf rbtRecdate.Checked Then
                    StrSql += " AND BASEDON = 'R'"
                ElseIf rbtAge.Checked Then
                    StrSql += " AND BASEDON = 'A'"
                End If
                If rbtTag.Checked Then
                    StrSql += " AND TAGNO = '" & txtTagno.Text & "'"
                ElseIf rbtRecdate.Checked Then
                    StrSql += " AND NOT (RECDATE_FROM > '" & dtpToDate_OWN.Value.Date.ToString("yyyy-MM-dd") & "' OR RECDATE_TO < '" & dtpFromDate_OWN.Value.Date.ToString("yyyy-MM-dd") & "')"
                Else
                    StrSql += " AND FROM_VAL BETWEEN " & Val(txtFrom_WET.Text) & " AND " & Val(txtTo_WET.Text) & ""
                    StrSql += " AND TO_VAL BETWEEN " & Val(txtFrom_WET.Text) & "AND " & Val(txtTo_WET.Text) & ""
                End If
                If Comm_Costcentre_Based And cmbCostcentre.Text <> "" And cmbCostcentre.Text <> "ALL" Then
                    StrSql += " AND COSTID ='" & _Costid.ToString & "'"
                End If
                StrSql += " AND SNO <> " & UPDATE_SNO & ""
                If objGPack.GetSqlValue(StrSql, , "-1") <> "-1" Then
                    If rbtTag.Checked Then
                        MsgBox("Already Exists For Tagno" & dt.Rows(i).Item("TAGNO").ToString & "", MsgBoxStyle.Information)
                        txtTagno.Select()
                    ElseIf rbtRecdate.Checked Then
                        MsgBox("Already Exists For Date Range From " & vbCrLf & dtpToDate_OWN.Value.Date.ToString("yyyy-MM-dd") & " To " & dtpToDate_OWN.Value.Date.ToString("yyyy-MM-dd") & "", MsgBoxStyle.Information)
                        txtFrom_WET.Select()
                    Else
                        MsgBox("Already Exists For Subitem " & dt.Rows(i).Item("SUBITEMNAME").ToString & "", MsgBoxStyle.Information)
                        txtFrom_WET.Select()
                    End If
                    Exit Sub
                End If
            Next
        End If
INS:
        Try
            Tran = Nothing
            Tran = cn.BeginTransaction
            If UPDATE_SNO <> Nothing Then
                StrSql = " UPDATE " & cnAdminDb & "..SALES_COMMISION SET"
                StrSql += " ITEMCTRID = " & ItemCtrId
                StrSql += " ,ITEMID = " & ItemId
                StrSql += " ,SUBITEMID = " & IIf(SubItemId = "", 0, SubItemId)
                If rbtWeight.Checked Then
                    StrSql += " ,BASEDON = 'W'"
                ElseIf rbtValue.Checked Then
                    StrSql += " ,BASEDON = 'V'"
                ElseIf rbtPcs.Checked Then
                    StrSql += " ,BASEDON = 'P'"
                ElseIf rbtTag.Checked Then
                    StrSql += " ,BASEDON = 'T'"
                ElseIf rbtRecdate.Checked Then
                    StrSql += " AND BASEDON = 'R'"
                ElseIf rbtAge.Checked Then
                    StrSql += " AND BASEDON = 'A'"
                End If
                StrSql += " ,FROM_VAL  = " & Val(txtFrom_WET.Text) & ""
                StrSql += " ,TO_VAL = " & Val(txtTo_WET.Text) & ""
                StrSql += " ,TAGNO = '" & txtTagno.Text & "'"
                StrSql += " ,COMMISION = " & Val(txtCommFlat_AMT.Text) & ""
                StrSql += " ,COMMISIONGRM = " & Val(txtCommPerGrm_AMT.Text) & ""
                StrSql += " ,COMMISIONPER = " & Val(txtCommPercentage_AMT.Text) & ""
                If Comm_Costcentre_Based And cmbCostcentre.Text <> "" And cmbCostcentre.Text <> "ALL" Then
                    StrSql += " ,COSTID ='" & _Costid.ToString & "'"
                End If
                StrSql += " WHERE SNO = " & UPDATE_SNO & ""
                ExecQuery(SyncMode.Master, StrSql, cn, Tran)
            Else
                Dim selectedsubitems() As String
                If SubItemId <> "" Then
                    selectedsubitems = SubItemId.Split(",")
                    For i As Integer = 0 To selectedsubitems.Length - 1
                        Dim DtSalesCommition As New DataTable
                        DtSalesCommition = BrighttechPack.GlobalMethods.GetTableStructure(cnAdminDb, "SALES_COMMISION", cn, Tran)
                        Row = DtSalesCommition.NewRow
                        Row.Item("SNO") = Val(objGPack.GetSqlValue("SELECT ISNULL(MAX(SNO),0)+1 FROM " & cnAdminDb & "..SALES_COMMISION", , , Tran))
                        Row.Item("ITEMCTRID") = ItemCtrId
                        Row.Item("ITEMID") = ItemId
                        If Val(SubItemId) <> 0 Then
                            Row.Item("SUBITEMID") = selectedsubitems(i)
                        Else
                            Row.Item("SUBITEMID") = 0
                        End If
                        If rbtWeight.Checked Then
                            Row.Item("BASEDON") = "W"
                        ElseIf rbtValue.Checked Then
                            Row.Item("BASEDON") = "V"
                        ElseIf rbtPcs.Checked Then
                            Row.Item("BASEDON") = "P"
                        ElseIf rbtTag.Checked Then
                            Row.Item("BASEDON") = "T"
                        ElseIf rbtRecdate.Checked Then
                            Row.Item("BASEDON") = "R"
                        ElseIf rbtAge.Checked Then
                            Row.Item("BASEDON") = "A"
                        End If
                        Row.Item("FROM_VAL") = Val(txtFrom_WET.Text)
                        Row.Item("TO_VAL") = Val(txtTo_WET.Text)
                        Row.Item("TAGNO") = txtTagno.Text
                        If rbtRecdate.Checked Then
                            Row.Item("RECDATE_FROM") = dtpFromDate_OWN.Value.Date.ToString("yyyy-MM-dd")
                            Row.Item("RECDATE_TO") = dtpToDate_OWN.Value.Date.ToString("yyyy-MM-dd")
                        End If
                        Row.Item("COMMISION") = Val(txtCommFlat_AMT.Text)
                        Row.Item("COMMISIONGRM") = Val(txtCommPerGrm_AMT.Text)
                        Row.Item("COMMISIONPER") = Val(txtCommPercentage_AMT.Text)
                        Row.Item("USERID") = userId
                        Row.Item("UPDATED") = GetEntryDate(GetServerDate(Tran), Tran)
                        Row.Item("UPTIME") = GetServerTime(Tran)
                        Row.Item("UPTIME") = Date.Now.ToLongTimeString
                        If Comm_Costcentre_Based And cmbCostcentre.Text <> "" And cmbCostcentre.Text <> "ALL" Then
                            Row.Item("COSTID") = _Costid.ToString
                        End If
                        DtSalesCommition.Rows.Add(Row)
                        InsertData(SyncMode.Master, DtSalesCommition, cn, Tran)
                    Next
                Else
                    Dim DtSalesCommition As New DataTable
                    DtSalesCommition = BrighttechPack.GlobalMethods.GetTableStructure(cnAdminDb, "SALES_COMMISION", cn, Tran)
                    Row = DtSalesCommition.NewRow
                    Row.Item("SNO") = Val(objGPack.GetSqlValue("SELECT ISNULL(MAX(SNO),0)+1 FROM " & cnAdminDb & "..SALES_COMMISION", , , Tran))
                    Row.Item("ITEMCTRID") = ItemCtrId
                    Row.Item("ITEMID") = ItemId
                    Row.Item("SUBITEMID") = IIf(SubItemId = "", 0, SubItemId)
                    If rbtWeight.Checked Then
                        Row.Item("BASEDON") = "W"
                    ElseIf rbtValue.Checked Then
                        Row.Item("BASEDON") = "V"
                    ElseIf rbtPcs.Checked Then
                        Row.Item("BASEDON") = "P"
                    ElseIf rbtTag.Checked Then
                        Row.Item("BASEDON") = "T"
                    ElseIf rbtRecdate.Checked Then
                        Row.Item("BASEDON") = "R"
                    ElseIf rbtAge.Checked Then
                        Row.Item("BASEDON") = "A"
                    End If
                    Row.Item("FROM_VAL") = Val(txtFrom_WET.Text)
                    Row.Item("TO_VAL") = Val(txtTo_WET.Text)
                    Row.Item("TAGNO") = txtTagno.Text
                    If rbtRecdate.Checked Then
                        Row.Item("RECDATE_FROM") = dtpFromDate_OWN.Value.Date.ToString("yyyy-MM-dd")
                        Row.Item("RECDATE_TO") = dtpToDate_OWN.Value.Date.ToString("yyyy-MM-dd")
                    End If
                    Row.Item("COMMISION") = Val(txtCommFlat_AMT.Text)
                    Row.Item("COMMISIONGRM") = Val(txtCommPerGrm_AMT.Text)
                    Row.Item("COMMISIONPER") = Val(txtCommPercentage_AMT.Text)
                    Row.Item("USERID") = userId
                    Row.Item("UPDATED") = GetEntryDate(GetServerDate(Tran), Tran)
                    Row.Item("UPTIME") = Date.Now.ToLongTimeString
                    If Comm_Costcentre_Based And cmbCostcentre.Text <> "" And cmbCostcentre.Text <> "ALL" Then
                        Row.Item("COSTID") = _Costid.ToString
                    End If
                    DtSalesCommition.Rows.Add(Row)
                    InsertData(SyncMode.Master, DtSalesCommition, cn, Tran)
                End If

            End If
            Tran.Commit()
            Fnew()
            'txtFrom_WET.Clear()
            'txtTo_WET.Clear()
            'txtCommFlat_AMT.Clear()
            'txtFrom_WET.Focus()
        Catch ex As Exception
            If Tran IsNot Nothing Then
                If Tran.Connection IsNot Nothing Then
                    Tran.Rollback()
                End If
            End If
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        Fnew()
    End Sub
    Function Fnew()
        objGPack.TextClear(Me)
        UPDATE_SNO = 0
        rbtWeight.Checked = True
        LoadCounter(cmbCounter_MAN)
        LoadItem(cmbItem_MAN)
        txtTagno.Text = ""
        LoadCostcentre(cmbCostcentre, "")

        If Comm_Costcentre_Based Then
            cmbCostcentre.Enabled = True
        Else
            cmbCostcentre.Enabled = False
        End If
        ''cmbCostcentre.Enabled = False
        Label12.Text = ""
        Label13.Text = ""
        'txtAgeFrom_NUM.Text = ""
        'txtAgeTo_NUM.Text = ""
        'txtAgeFrom_NUM.Enabled = False
        'txtAgeTo_NUM.Enabled = False
        dtpFromDate_OWN.Value = Today
        dtpToDate_OWN.Value = Today
        dtpFromDate_OWN.Enabled = False
        dtpToDate_OWN.Enabled = False
        If cmbCostcentre.Enabled Then
            cmbCostcentre.Select()
        Else
            cmbCounter_MAN.Select()
        End If
        txtTagno.Enabled = False
    End Function

    Private Sub CallGrid()
        Dim _Costid As String = ""
        If cmbvcostcenter.Text <> "" And cmbvcostcenter.Text <> "ALL" Then
            _Costid = GetSqlValue(cn, " SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME= '" & cmbvcostcenter.Text & "'")
        Else
            _Costid = ""
        End If

        StrSql = " SELECT (SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = SA.ITEMCTRID)AS COUNTER"
        StrSql += vbCrLf + " ,IM.ITEMID,IM.ITEMNAME +' (' +CONVERT (VARCHAR,IM .ITEMID)+')' AS ITEM,"
        StrSql += vbCrLf + " (CASE WHEN IM.CALTYPE='W' THEN 'WEIGHT'"
        StrSql += vbCrLf + " WHEN IM.CALTYPE='B'THEN 'BOTH'"
        StrSql += vbCrLf + " WHEN IM.CALTYPE='F'THEN 'FIXED'"
        StrSql += vbCrLf + " WHEN IM.CALTYPE='R'THEN 'RATE'"
        StrSql += vbCrLf + " WHEN IM.CALTYPE='M'THEN 'METAL RATE'"
        StrSql += vbCrLf + " WHEN IM.CALTYPE='P'THEN 'PIECES' ELSE ''END)ITYPE, "
        StrSql += vbCrLf + " SM.SUBITEMID,SM.SUBITEMNAME +' (' +CONVERT (VARCHAR,SM .SUBITEMID)+')' AS SUBITEM"
        StrSql += vbCrLf + " ,(CASE WHEN SM.CALTYPE='W' THEN 'WEIGHT'"
        StrSql += vbCrLf + " WHEN SM.CALTYPE='B'THEN 'BOTH'"
        StrSql += vbCrLf + " WHEN SM.CALTYPE='F'THEN 'FIXED'"
        StrSql += vbCrLf + " WHEN SM.CALTYPE='R'THEN 'RATE'"
        StrSql += vbCrLf + " WHEN SM.CALTYPE='M'THEN 'METAL RATE'"
        StrSql += vbCrLf + " WHEN SM.CALTYPE='P'THEN 'PIECES' ELSE ''END)SITYPE "
        StrSql += vbCrLf + " ,CASE WHEN SA.BASEDON = 'W' THEN 'WEIGHT' WHEN  SA.BASEDON = 'P'  THEN 'PIECES' WHEN  SA.BASEDON = 'T' THEN 'TAG' ELSE 'VALUE' END AS BASEDON"
        StrSql += vbCrLf + " ,SA.TAGNO,SA.FROM_VAL,SA.TO_VAL,SA.COMMISION,SA.COMMISIONGRM,SA.COMMISIONPER,SA.SNO"
        StrSql += vbCrLf + " FROM " & cnAdminDb & "..SALES_COMMISION AS SA"
        StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = SA.ITEMID "
        StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = SA.ITEMID AND SM.SUBITEMID = SA.SUBITEMID"
        StrSql += vbCrLf + " WHERE 1=1"
        If cmbvcostcenter.Text <> "" And cmbvcostcenter.Text <> "ALL" Then
            StrSql += "  AND SA.COSTID ='" & _Costid.ToString & "'"
        End If
        If cmbOpenCounter.Text <> "ALL" And cmbOpenCounter.Text <> "" Then
            StrSql += vbCrLf + " AND SA.ITEMCTRID = (SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & cmbOpenCounter.Text & "')"
        End If
        If cmbOpenItem.Text <> "ALL" And cmbOpenItem.Text <> "" Then
            StrSql += vbCrLf + " AND IM.ITEMNAME = '" & cmbOpenItem.Text & "'"
        End If
        If cmbMetalType.Text <> "ALL" And cmbMetalType.Text <> "" Then
            StrSql += vbCrLf + " AND IM.METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME='" & cmbMetalType.Text & "')"
        End If
        If cmbOpenSubItem.Text <> "ALL" And cmbOpenSubItem.Text <> "" Then
            StrSql += vbCrLf + " AND SM.SUBITEMNAME = '" & cmbOpenSubItem.Text & "'"
        End If

        If CmbBasedOn.Text = "WEIGHT" Then
            StrSql += vbCrLf + " AND BASEDON ='W'"
        ElseIf CmbBasedOn.Text = "VALUE" Then
            StrSql += vbCrLf + " AND BASEDON = 'V'"
        ElseIf CmbBasedOn.Text = "PCS" Then
            StrSql += vbCrLf + " AND BASEDON = 'P'"
        ElseIf CmbBasedOn.Text = "TAG" Then
            StrSql += vbCrLf + " AND BASEDON = 'T'"
        ElseIf CmbBasedOn.Text = "AGE" Then
            StrSql += vbCrLf + " AND BASEDON = 'A'"
        End If
        If cmbActiveitem.Text = "NO" Then
            StrSql += vbCrLf + " AND IM.ACTIVE <>'Y'"
            StrSql += vbCrLf + " AND SM.ACTIVE <>'Y' "
        ElseIf cmbActiveitem.Text = "YES" Then
            StrSql += vbCrLf + " AND IM.ACTIVE ='Y'"
            StrSql += vbCrLf + " AND SM.ACTIVE ='Y' "
        End If
        Dim dtGrid As New DataTable
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dtGrid)
        gridView.DataSource = Nothing
        gridView.DataSource = dtGrid
        gridView.Columns("SNO").Visible = False
        gridView.Columns("ITEMID").Visible = False
        gridView.Columns("SUBITEMID").Visible = False
        With gridView
            '.Columns("COUNTER").Width = 150
            '.Columns("ITEM").Width = 180
            '.Columns("SUBITEM").Width = 150
            '.Columns("BASEDON").Width = 70
            '.Columns("FROM_VAL").Width = 100
            '.Columns("TO_VAL").Width = 100
            '.Columns("COMMISION").Width = 80
            '.Columns("COMMISIONGRM").Width = 80
            '.Columns("COMMISIONPER").Width = 80
            '.Columns("ITYPE").Width = 80
            '.Columns("SITYPE").Width = 80

            .Columns("FROM_VAL").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("TO_VAL").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("COMMISION").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("COMMISIONGRM").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("COMMISIONPER").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            .Columns("ITYPE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns("SITYPE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft

            .Columns("COMMISION").HeaderText = "COMM FLAT"
            .Columns("COMMISIONGRM").HeaderText = "COMM GRM"
            .Columns("COMMISIONPER").HeaderText = "COMM%"
            .Columns("ITYPE").HeaderText = "ITEM CAL TYPE"
            .Columns("SITYPE").HeaderText = "SUBITEM CAL TYPE"
            .Columns("BASEDON").HeaderText = "COMM BASED ON "
            For i As Integer = 0 To .ColumnCount - 5
                .Columns(i).ReadOnly = True
            Next
            .Columns("COMMISION").ReadOnly = False
            .Columns("COMMISIONGRM").ReadOnly = False
            .Columns("COMMISIONPER").ReadOnly = False
            .Columns("SNO").ReadOnly = True
            '.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
        End With
        AutoResizrToolStripMenuItem_Click(Me, New EventArgs)
        'FormatGridColumns(gridView, False, False, True, False)
    End Sub

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click
        tabMain.SelectedTab = tabView
        LoadCounter(cmbOpenCounter)
        LoadItem(cmbOpenItem)
        Loadmetal(cmbMetalType)
        LoadCmbBasedOn(CmbBasedOn)
        LoadvCostcenter(cmbvcostcenter, "")
        cmbvcostcenter.Text = "ALL"
        CallGrid()
        cmbOpenSubItem.Text = "ALL"
        cmbActiveitem.Text = "ALL"
        CmbBasedOn.Text = "ALL"


        cmbMetalType.Select()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub cmbItem_MAN_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        LoadSubItem() '(cmbItem_MAN, cmbSubItem_MAN)
    End Sub

    Private Sub txtFrom_WET_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFrom_WET.GotFocus
        'If txtFrom_WET.Text <> "" Then Exit Sub
        'Dim ItemCtrId As Integer = Val(objGPack.GetSqlValue("SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & cmbCounter_MAN.Text & "'"))
        'Dim ItemId As Integer = Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'"))
        'Dim SubItemId As Integer = Val(objGPack.GetSqlValue("SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem_MAN.Text & "' AND ITEMID = " & ItemId & ""))
        'StrSql = " SELECT ISNULL(MAX(TO_VAL),0)+.001 FROM " & cnAdminDb & "..SALES_COMMISION"
        'StrSql += vbCrLf + " WHERE ITEMCTRID = " & ItemCtrId & " AND ITEMID = " & ItemId & ""
        'StrSql += vbCrLf + " AND SUBITEMID = " & SubItemId & ""
        'StrSql += vbCrLf + " AND BASEDON = '" & IIf(rbtWeight.Checked, "W", "V") & "'"
        'Dim frmVal As Decimal = Val(objGPack.GetSqlValue(StrSql))
        'txtFrom_WET.Text = IIf(frmVal <> 0, Format(frmVal, "0.000"), "")
    End Sub

    Private Function ValidateToWeight() As Boolean
        If Val(txtFrom_WET.Text) <= Val(txtTo_WET.Text) Then
            Return True
        Else
            MsgBox("Invalid To Value" + vbCrLf + "To value should greater than From Value", MsgBoxStyle.Information)
            txtTo_WET.Select()
        End If
    End Function

    Private Sub txtTo_WET_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtTo_WET.KeyDown
        If e.KeyCode = Keys.Enter Then
            ValidateToWeight()
        End If
    End Sub

    Private Sub cmbOpenItem_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbOpenItem.SelectedIndexChanged
        LoadSubItemOpen()
        CallGrid()
    End Sub

    Private Sub cmbOpenSubItem_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbOpenSubItem.SelectedIndexChanged
        CallGrid()
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGeneral
        cmbCounter_MAN.Select()
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
    Private Sub gridView_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.CellEndEdit
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
        If Not gridView.RowCount > 0 Then Exit Sub
        If gridView.CurrentRow Is Nothing Then Exit Sub
        If Val(gridView.CurrentRow.Cells("SNO").Value.ToString) = 0 Then MsgBox("Please Check the Sno", MsgBoxStyle.Information) : Exit Sub
        Try
            Tran = Nothing
            Tran = cn.BeginTransaction
            StrSql = " UPDATE " & cnAdminDb & "..SALES_COMMISION SET"
            If Me.gridView.CurrentCell.ColumnIndex = gridView.Columns("COMMISION").Index Then
                StrSql += " COMMISION=" & Val(gridView.CurrentRow.Cells("COMMISION").Value.ToString) & ""
            ElseIf Me.gridView.CurrentCell.ColumnIndex = gridView.Columns("COMMISIONGRM").Index Then
                StrSql += " COMMISIONGRM=" & Val(gridView.CurrentRow.Cells("COMMISIONGRM").Value.ToString) & ""
            ElseIf Me.gridView.CurrentCell.ColumnIndex = gridView.Columns("COMMISIONPER").Index Then
                StrSql += " COMMISIONPER=" & Val(gridView.CurrentRow.Cells("COMMISIONPER").Value.ToString) & ""
            End If
            StrSql += " WHERE SNO = " & Val(gridView.CurrentRow.Cells("SNO").Value.ToString) & ""
            ExecQuery(SyncMode.Master, StrSql, cn, Tran)
            Tran.Commit()
            Tran = Nothing
        Catch ex As Exception
            If Not Tran Is Nothing Then Tran.Rollback() : Tran = Nothing
            MsgBox(ex.Message + vbCrLf + ex.StackTrace, MsgBoxStyle.Information)
        End Try
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            'e.Handled = True
            'If Not DataGridView1.RowCount > 0 Then Exit Sub
            'If DataGridView1.CurrentRow Is Nothing Then Exit Sub
            'With DataGridView1.Rows(DataGridView1.CurrentRow.Index)
            '    cmbCounter_MAN.Text = IIf(.Cells("COUNTER").Value.ToString <> "", .Cells("COUNTER").Value.ToString, "ALL")
            '    cmbItem_MAN.Text = IIf(.Cells("ITEM").Value.ToString <> "", .Cells("ITEM").Value.ToString, "ALL")
            '    chkcmbsubitem.Text = IIf(.Cells("SUBITEM").Value.ToString <> "", .Cells("SUBITEM").Value.ToString, "ALL")
            '    If .Cells("BASEDON").Value.ToString = "WEIGHT" Then rbtWeight.Checked = True Else rbtValue.Checked = True
            '    txtFrom_WET.Text = Format(Val(.Cells("FROM_VAL").Value.ToString), "0.000")
            '    txtTo_WET.Text = Format(Val(.Cells("TO_VAL").Value.ToString), "0.000")
            '    txtCommFlat_AMT.Text = Format(Val(.Cells("COMMISION").Value.ToString), "0.00")
            '    txtCommPercentage_AMT.Text = Format(Val(IIf(.Cells("COMMISIONPER").Value.ToString <> "", .Cells("COMMISIONPER").Value.ToString, 0)), "0.00")
            '    txtCommPerGrm_AMT.Text = Format(Val(IIf(.Cells("COMMISIONGRM").Value.ToString <> "", .Cells("COMMISIONGRM").Value.ToString, 0)), "0.00")
            '    UPDATE_SNO = .Cells("SNO").Value.ToString
            '    tabMain.SelectedTab = tabGeneral
            '    cmbItem_MAN.Select()
            'End With
        End If
    End Sub

    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If Not gridView.RowCount > 0 Then Exit Sub
        If gridView.CurrentRow Is Nothing Then Exit Sub
        StrSql = " DELETE FROM " & cnAdminDb & "..SALES_COMMISION"
        StrSql += " WHERE SNO = " & Val(gridView.CurrentRow.Cells("SNO").Value.ToString)
        Try
            Tran = Nothing
            Tran = cn.BeginTransaction
            ExecQuery(SyncMode.Master, StrSql, cn, Tran)
            Tran.Commit()
            Tran = Nothing
            CallGrid()
        Catch ex As Exception
            If Tran IsNot Nothing Then
                Tran.Rollback()
            End If
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub

    Private Sub rbtWeight_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtWeight.CheckedChanged
        'txtCommPercentage_AMT.Enabled = Not rbtWeight.Checked
        'txtCommPerGrm_AMT.Enabled = rbtWeight.Checked
        'If rbtWeight.Checked = False Then txtCommPercentage_AMT.Clear()
        'If rbtWeight.Checked = False Then txtCommPerGrm_AMT.Clear()
    End Sub


    Private Sub cmbOpenCounter_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbOpenCounter.SelectedIndexChanged
        LoadItem(cmbOpenItem)
        CallGrid()
    End Sub

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        'If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "Sales Commision", gridView, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub btnEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEdit.Click
        If Not gridView.RowCount > 0 Then Exit Sub
        If gridView.CurrentRow Is Nothing Then Exit Sub
        With gridView.Rows(gridView.CurrentRow.Index)
            Dim tempitemid As String = GetSqlValue(cn, "SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID='" & gridView.CurrentRow.Cells("ITEMID").Value.ToString & "'")
            Dim tempsubitemid As String = GetSqlValue(cn, "SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID='" & gridView.CurrentRow.Cells("SUBITEMID").Value.ToString & "'")
            cmbCounter_MAN.Text = IIf(.Cells("COUNTER").Value.ToString <> "", .Cells("COUNTER").Value.ToString, "ALL")
            'cmbItem_MAN.Text = IIf(.Cells("ITEM").Value.ToString <> "", .Cells("ITEM").Value.ToString, "ALL")
            cmbItem_MAN.Text = tempitemid
            txtItemCode_NUM.Text = gridView.CurrentRow.Cells("ITEMID").Value.ToString
            chkcmbsubitem.Text = tempsubitemid
            'chkcmbsubitem.Text = IIf(.Cells("SUBITEM").Value.ToString <> "", .Cells("SUBITEM").Value.ToString, "ALL")
            If .Cells("BASEDON").Value.ToString = "WEIGHT" Then
                rbtWeight.Checked = True
            ElseIf .Cells("BASEDON").Value.ToString = "PIECES" Then
                rbtPcs.Checked = True
            ElseIf .Cells("BASEDON").Value.ToString = "TAG" Then
                rbtTag.Checked = True
            Else
                rbtValue.Checked = True
            End If
            txtTagno.Text = .Cells("TAGNO").Value.ToString
            txtFrom_WET.Text = Format(Val(.Cells("FROM_VAL").Value.ToString), "0.000")
            txtTo_WET.Text = Format(Val(.Cells("TO_VAL").Value.ToString), "0.000")
            txtCommFlat_AMT.Text = Format(Val(.Cells("COMMISION").Value.ToString), "0.00")
            txtCommPercentage_AMT.Text = Format(Val(IIf(.Cells("COMMISIONPER").Value.ToString <> "", .Cells("COMMISIONPER").Value.ToString, 0)), "0.00")
            txtCommPerGrm_AMT.Text = Format(Val(IIf(.Cells("COMMISIONGRM").Value.ToString <> "", .Cells("COMMISIONGRM").Value.ToString, 0)), "0.00")
            UPDATE_SNO = .Cells("SNO").Value.ToString
            tabMain.SelectedTab = tabGeneral
            txtItemCode_NUM.Select()
        End With
    End Sub

    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        CallGrid()
        If gridView.Rows.Count > 0 Then gridView.Select()
    End Sub

    Private Sub txtItemCode_NUM_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtItemCode_NUM.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            StrSql = " Select ITEMNAME,CALTYPE from " & cnAdminDb & "..itemMast where itemId = '" & txtItemCode_NUM.Text & "'"
            Dim dt As New DataTable
            dt.Clear()
            Da = New OleDbDataAdapter(StrSql, cn)
            Da.Fill(dt)
            If dt.Rows.Count > 0 Then
                cmbItem_MAN.Text = dt.Rows(0).Item("itemName")
                If dt.Rows(0).Item("CALTYPE") = "W" Then
                    Label12.Text = "CAL TYPE: WEIGHT"
                ElseIf dt.Rows(0).Item("CALTYPE") = "R" Then
                    Label12.Text = "CAL TYPE: RATE"
                ElseIf dt.Rows(0).Item("CALTYPE") = "B" Then
                    Label12.Text = "CAL TYPE: BOTH"
                ElseIf dt.Rows(0).Item("CALTYPE") = "F" Then
                    Label12.Text = "CAL TYPE: FIXED"
                ElseIf dt.Rows(0).Item("CALTYPE") = "M" Then
                    Label12.Text = "CAL TYPE: METAL RATE"
                ElseIf dt.Rows(0).Item("CALTYPE") = "P" Then
                    Label12.Text = "CAL TYPE: PIECES"
                Else
                    Label12.Text = ""
                End If
            Else
                ' cmbItem_MAN.Clear()
            End If
            If cmbItem_MAN.Text <> "" Then
                chkcmbsubitem.Items.Clear()
                chkcmbsubitem.Items.Add("ALL")
                StrSql = " Select SubItemName from " & cnAdminDb & "..SubItemMast where itemId = '" & txtItemCode_NUM.Text & "'"
                Dim dtSItem As DataTable
                dtSItem = New DataTable
                Da = New OleDbDataAdapter(StrSql, cn)
                Da.Fill(dtSItem)
                BrighttechPack.GlobalMethods.FillCombo(chkcmbsubitem, dtSItem, "SUBITEMNAME", False)
                'objGPack.FillCombo(strSql, cmbSubItemName, False)
                chkcmbsubitem.Text = "ALL"
                If chkcmbsubitem.Items.Count > 0 Then
                    If rbtTag.Checked = False Then
                        chkcmbsubitem.Enabled = True
                    End If
                Else
                    ' chkcmbsubitem.Enabled = False
                End If
            Else
                chkcmbsubitem.Items.Clear()
                'chkcmbsubitem.Enabled = False
            End If
        End If
    End Sub

    Private Sub txtItemCode_NUM_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtItemCode_NUM.KeyDown
        Dim itemId As String
        If e.KeyCode = Keys.Insert Then
            StrSql = " SELECT DISTINCT"
            StrSql += vbCrLf + " ITEMID, "
            StrSql += vbCrLf + " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS ITEMNAME"
            StrSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMMAST AS T"
            itemId = BrighttechPack.SearchDialog.Show("Find ItemId", StrSql, cn)
            StrSql = " Select itemName from " & cnAdminDb & "..itemMast where itemId = '" & itemId & "'"
            Dim dt As New DataTable
            dt.Clear()
            Da = New OleDbDataAdapter(StrSql, cn)
            Da.Fill(dt)
            If dt.Rows.Count > 0 Then
                txtItemCode_NUM.Text = itemId
                cmbItem_MAN.Text = dt.Rows(0).Item("itemName")
            End If
        End If
    End Sub

    Private Sub cmbItem_MAN_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbItem_MAN.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            StrSql = ""
            StrSql = "SELECT DISTINCT ITEMID,CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbItem_MAN.Text & "'"
            Dim dt As New DataTable
            dt.Clear()
            Da = New OleDbDataAdapter(StrSql, cn)
            Da.Fill(dt)
            If dt.Rows.Count > 0 Then
                txtItemCode_NUM.Text = dt.Rows(0).Item("ITEMID").ToString
                If dt.Rows(0).Item("CALTYPE") = "W" Then
                    Label12.Text = "CAL TYPE: WEIGHT"
                ElseIf dt.Rows(0).Item("CALTYPE") = "R" Then
                    Label12.Text = "CAL TYPE: RATE"
                ElseIf dt.Rows(0).Item("CALTYPE") = "B" Then
                    Label12.Text = "CAL TYPE: BOTH"
                ElseIf dt.Rows(0).Item("CALTYPE") = "F" Then
                    Label12.Text = "CAL TYPE: FIXED"
                ElseIf dt.Rows(0).Item("CALTYPE") = "M" Then
                    Label12.Text = "CAL TYPE: METAL RATE"
                ElseIf dt.Rows(0).Item("CALTYPE") = "P" Then
                    Label12.Text = "CAL TYPE: PIECES"
                Else
                    Label12.Text = ""
                End If
            End If

            If cmbItem_MAN.Text <> "" Then
                chkcmbsubitem.Items.Clear()
                chkcmbsubitem.Items.Add("ALL")
                StrSql = " Select SubItemName from " & cnAdminDb & "..SubItemMast where itemId = '" & txtItemCode_NUM.Text & "'"
                Dim dtSItem As DataTable
                dtSItem = New DataTable
                Da = New OleDbDataAdapter(StrSql, cn)
                Da.Fill(dtSItem)
                BrighttechPack.GlobalMethods.FillCombo(chkcmbsubitem, dtSItem, "SUBITEMNAME", False)
                'objGPack.FillCombo(strSql,cmbSubItemName, False)
                chkcmbsubitem.Text = "ALL"
                If chkcmbsubitem.Items.Count > 0 Then
                    chkcmbsubitem.Enabled = True
                Else
                    chkcmbsubitem.Enabled = False
                End If
            Else
                chkcmbsubitem.Items.Clear()
                chkcmbsubitem.Enabled = False
            End If
        End If
    End Sub

    Private Sub cmbItem_MAN_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbItem_MAN.SelectedValueChanged
        StrSql = ""
        StrSql = "SELECT DISTINCT ITEMID,CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbItem_MAN.Text & "'"
        Dim dt As New DataTable
        dt.Clear()
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dt)
        If dt.Rows.Count > 0 Then
            txtItemCode_NUM.Text = dt.Rows(0).Item("ITEMID").ToString
            If dt.Rows(0).Item("CALTYPE") = "W" Then
                Label12.Text = "CAL TYPE: WEIGHT"
            ElseIf dt.Rows(0).Item("CALTYPE") = "R" Then
                Label12.Text = "CAL TYPE: RATE"
            ElseIf dt.Rows(0).Item("CALTYPE") = "B" Then
                Label12.Text = "CAL TYPE: BOTH"
            ElseIf dt.Rows(0).Item("CALTYPE") = "F" Then
                Label12.Text = "CAL TYPE: FIXED"
            ElseIf dt.Rows(0).Item("CALTYPE") = "M" Then
                Label12.Text = "CAL TYPE: METAL RATE"
            ElseIf dt.Rows(0).Item("CALTYPE") = "P" Then
                Label12.Text = "CAL TYPE: PIECES"
            Else
                Label12.Text = ""
            End If
        End If
        If cmbItem_MAN.Text = "ALL" Then
            txtItemCode_NUM.Text = ""
            Label12.Text = ""
        End If
    End Sub

    Private Sub cmbItem_MAN_SelectionChangeCommitted(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbItem_MAN.SelectionChangeCommitted


    End Sub

    Private Sub txtsubitemid_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtsubitemid.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtsubitemid.Text <> "" Then
                chkcmbsubitem.Items.Clear()
                chkcmbsubitem.Items.Add("ALL")
                StrSql = " Select SubItemName from " & cnAdminDb & "..SubItemMast where itemId = '" & txtItemCode_NUM.Text & "'"
                Dim dtSItem As DataTable
                dtSItem = New DataTable
                Da = New OleDbDataAdapter(StrSql, cn)
                Da.Fill(dtSItem)
                StrSql = "SELECT DISTINCT SUBITEMNAME,CALTYPE FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID='" & txtsubitemid.Text & "'"
                Dim dt As New DataTable
                dt.Clear()
                Da = New OleDbDataAdapter(StrSql, cn)
                Da.Fill(dt)
                If dt.Rows.Count > 0 Then
                    'chkcmbsubitem.SelectedItem = dt.Rows(0).Item("SUBITEMNAME").ToString()
                    BrighttechPack.GlobalMethods.FillCombo(chkcmbsubitem, dtSItem, "SUBITEMNAME", , dt.Rows(0).Item("SUBITEMNAME").ToString())
                    ' chkcmbsubitem.CheckedItems.Item(dt.Rows(0).Item("SUBITEMNAME").ToString) = IIf(dt.Rows(0).Item("SUBITEMNAME").ToString() <> "", dt.Rows(0).Item("SUBITEMNAME").ToString(), "ALL")
                    'chkcmbsubitem. = dt.Rows(0).Item("SUBITEMNAME").ToString()
                    If dt.Rows(0).Item("CALTYPE") = "W" Then
                        Label13.Text = "CAL TYPE: WEIGHT"
                    ElseIf dt.Rows(0).Item("CALTYPE") = "R" Then
                        Label13.Text = "CAL TYPE: RATE"
                    ElseIf dt.Rows(0).Item("CALTYPE") = "B" Then
                        Label13.Text = "CAL TYPE: BOTH"
                    ElseIf dt.Rows(0).Item("CALTYPE") = "F" Then
                        Label13.Text = "CAL TYPE: FIXED"
                    ElseIf dt.Rows(0).Item("CALTYPE") = "M" Then
                        Label13.Text = "CAL TYPE: METAL RATE"
                    ElseIf dt.Rows(0).Item("CALTYPE") = "P" Then
                        Label13.Text = "CAL TYPE: PIECES"
                    Else
                        Label13.Text = ""
                    End If
                End If
            Else
                chkcmbsubitem.Items.Clear()
                chkcmbsubitem.Items.Add("ALL")
                StrSql = "SELECT 'ALL'SubItemName,0 RESULT"
                StrSql += " UNION ALL "
                StrSql += " Select SubItemName,1 AS RESULT from " & cnAdminDb & "..SubItemMast where itemId = '" & txtItemCode_NUM.Text & "'"
                StrSql += " order by result "
                Dim dtSItem As DataTable
                dtSItem = New DataTable
                Da = New OleDbDataAdapter(StrSql, cn)
                Da.Fill(dtSItem)
                BrighttechPack.GlobalMethods.FillCombo(chkcmbsubitem, dtSItem, "SUBITEMNAME", , "ALL")
            End If
        End If
    End Sub

    Private Sub chkcmbsubitem_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles chkcmbsubitem.KeyPress
        Dim TEMPVAL As String = Nothing
        If e.KeyChar = Chr(Keys.Enter) Then
            If chkcmbsubitem.CheckedItems.Count > 1 Then
                txtsubitemid.Text = ""
                Label13.Text = ""
            Else
                TEMPVAL = GetSelectedSubitemid(chkcmbsubitem, False, True, txtItemCode_NUM.Text)
                txtsubitemid.Text = TEMPVAL
                StrSql = " SELECT CALTYPE FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID='" & TEMPVAL & "'"
                Dim DTT As DataTable
                DTT = New DataTable
                Da = New OleDbDataAdapter(StrSql, cn)
                Da.Fill(DTT)
                If DTT.Rows.Count > 0 Then
                    If DTT.Rows(0).Item("CALTYPE") = "W" Then
                        Label13.Text = "CAL TYPE: WEIGHT"
                    ElseIf DTT.Rows(0).Item("CALTYPE") = "R" Then
                        Label13.Text = "CAL TYPE: RATE"
                    ElseIf DTT.Rows(0).Item("CALTYPE") = "B" Then
                        Label13.Text = "CAL TYPE: BOTH"
                    ElseIf DTT.Rows(0).Item("CALTYPE") = "F" Then
                        Label13.Text = "CAL TYPE: FIXED"
                    ElseIf DTT.Rows(0).Item("CALTYPE") = "M" Then
                        Label13.Text = "CAL TYPE: METAL RATE"
                    ElseIf DTT.Rows(0).Item("CALTYPE") = "P" Then
                        Label13.Text = "CAL TYPE: PIECES"
                    Else
                        Label13.Text = ""
                    End If
                End If
            End If
            If chkcmbsubitem.Text = "ALL" Then
                Label13.Text = ""
                txtsubitemid.Text = ""
            End If
        End If
    End Sub

    Private Sub btnopenNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnopenNew.Click
        LoadCounter(cmbOpenCounter)
        LoadItem(cmbOpenItem)
        Loadmetal(cmbMetalType)
        LoadCmbBasedOn(CmbBasedOn)
        LoadCostcentre(cmbCostcentre, "")
        CallGrid()
        cmbOpenSubItem.Text = "ALL"
        cmbActiveitem.Text = "ALL"
        CmbBasedOn.Text = "ALL"
        UPDATE_SNO = 0
        cmbMetalType.Select()

    End Sub

    Private Sub cmbMetalType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbMetalType.SelectedIndexChanged
        LoadItem(cmbOpenItem)
        CallGrid()
    End Sub

    Private Sub AutoResizrToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AutoResizrToolStripMenuItem.Click
        If gridView.RowCount > 0 Then
            AutoResizrToolStripMenuItem.Checked = True
            AutoResizrToolStripMenuItem.Checked = True
            If AutoResizrToolStripMenuItem.Checked Then
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                gridView.Invalidate()
                For Each dgvCol As DataGridViewColumn In gridView.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            Else
                For Each dgvCol As DataGridViewColumn In gridView.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End If
            AutoResizrToolStripMenuItem.Checked = False
        End If
    End Sub

    Private Sub CmbBasedOn_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmbBasedOn.SelectedIndexChanged
        LoadItem(cmbOpenItem)
        CallGrid()
    End Sub

    Private Sub cmbActiveitem_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbActiveitem.SelectedIndexChanged
        LoadItem(cmbOpenItem)
        CallGrid()
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnopenNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub rbtTag_CheckedChanged(sender As Object, e As EventArgs) Handles rbtTag.CheckedChanged
        If rbtTag.Checked Then
            cmbCounter_MAN.Enabled = False
            cmbItem_MAN.Enabled = False
            chkcmbsubitem.Enabled = False
            cmbMetalType.Enabled = False
            txtFrom_WET.Enabled = False
            txtTo_WET.Enabled = False
            txtsubitemid.Enabled = False
            txtTagno.Enabled = True
            If Costcentre = True Then
                cmbCostcentre.Enabled = True
                LoadCostcentre(cmbCostcentre, cnCostId)
            End If
        Else
            cmbCounter_MAN.Enabled = True
            cmbItem_MAN.Enabled = True
            chkcmbsubitem.Enabled = True
            cmbMetalType.Enabled = True
            txtFrom_WET.Enabled = True
            txtTo_WET.Enabled = True
            txtsubitemid.Enabled = True
            txtTagno.Enabled = False
            If Comm_Costcentre_Based Then
                cmbCostcentre.Enabled = True
            Else
                cmbCostcentre.Enabled = False
            End If
            ''cmbCostcentre.Enabled = False
        End If
    End Sub

    Private Sub txtTagno_Leave(sender As Object, e As EventArgs) Handles txtTagno.Leave
        If txtTagno.Enabled Then
            Dim dt As New DataTable
            StrSql = "SELECT (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID) ITEMNAME, "
            StrSql += vbCrLf + "(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = I.ITEMID AND SUBITEMID = I.SUBITEMID) SUBITEMNAME, "
            StrSql += vbCrLf + "(SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = I.ITEMCTRID ) COUNTERNAME, "
            StrSql += vbCrLf + "NETWT,TAGNO,ISSDATE ,TOFLAG FROM " & cnAdminDb & "..ITEMTAG I WHERE ITEMID = '" & txtItemCode_NUM.Text & "' AND TAGNO = '" & txtTagno.Text & "'"
            Da = New OleDbDataAdapter(StrSql, cn)
            dt = New DataTable
            Da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If dt.Rows(0).Item("ISSDATE").ToString <> "" And dt.Rows(0).Item("TOFLAG") = "SA" Then
                    MsgBox("TAGNO IS ALREADY ISSUED", MsgBoxStyle.OkOnly)
                    txtTagno.Select()
                    Exit Sub
                End If
                If dt.Rows(0).Item("ISSDATE").ToString = "" Then
                    cmbItem_MAN.Text = dt.Rows(0).Item("ITEMNAME").ToString
                    chkcmbsubitem.Text = dt.Rows(0).Item("SUBITEMNAME").ToString
                    cmbCounter_MAN.Text = dt.Rows(0).Item("COUNTERNAME").ToString
                End If
            Else
                MsgBox("Record Not Found", MsgBoxStyle.OkOnly)
                'txtTagno.Select()
            End If
        End If
    End Sub

    Private Sub txtTagno_KeyDown(sender As Object, e As KeyEventArgs) Handles txtTagno.KeyDown
        If e.KeyCode = Keys.Insert Then
            If Val(txtItemCode_NUM.Text) = 0 Then Exit Sub
            Dim stockType As String = objGPack.GetSqlValue("SELECT STOCKTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = '" & txtItemCode_NUM.Text & "'")
            If stockType = "T" Then
                StrSql = " SELECT"
                StrSql += vbCrLf + " TAGNO AS TAGNO,STYLENO,ITEMID AS ITEMID,"
                StrSql += vbCrLf + " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS ITEMNAME,NARRATION,"
                StrSql += vbCrLf + " T.PCS AS PCS,"
                StrSql += vbCrLf + " GRSWT AS GRS_WT,NETWT AS NET_WT,RATE AS RATE,"
                StrSql += vbCrLf + " SALVALUE AS SALVALUE,"
                StrSql += vbCrLf + " ISNULL((SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE  SUBITEMID  = T.SUBITEMID),'')AS SUBITEM,"
                StrSql += vbCrLf + " CONVERT(VARCHAR,RECDATE,103) AS RECDATE,"
                StrSql += vbCrLf + " CASE WHEN APPROVAL = 'A' THEN 'APPROVAL' WHEN APPROVAL = 'R' THEN 'RESERVED' ELSE NULL END AS STATUS,"
                StrSql += vbCrLf + " CASE WHEN APPROVAL = 'A' THEN '" & Color.MistyRose.Name & "' WHEN APPROVAL = 'R' THEN '" & Color.PaleTurquoise.Name & "' ELSE NULL END AS COLOR_HIDE,"
                StrSql += vbCrLf + " (SELECT SIZENAME FROM " & cnAdminDb & "..ITEMSIZE WHERE SIZEID = T.SIZEID)AS SIZE,"
                StrSql += vbCrLf + " ISNULL(IC.ITEMCTRNAME,'') COUNTER "
                StrSql += vbCrLf + " FROM"
                StrSql += vbCrLf + " " & cnAdminDb & "..ITEMTAG AS T "
                StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMCOUNTER AS IC ON T.ITEMCTRID = IC.ITEMCTRID"
                StrSql += vbCrLf + " WHERE T.ITEMID = " & Val(txtItemCode_NUM.Text) & ""
                If Costcentre = True Then
                    StrSql += vbCrLf + " AND COSTID = '" & cnCostId & "'"
                End If
                StrSql += " AND COMPANYID = '" & cnCompanyId & "'"
                StrSql += vbCrLf + " AND ISSDATE IS NULL"
                StrSql += " ORDER BY TAGNO"
                txtTagno.Text = BrighttechPack.SearchDialog.Show("Find TagNo", StrSql, cn, , , , , , , , False)
                txtTagno.SelectAll()
            End If
        End If
    End Sub

    Private Sub rbtTag_KeyPress(sender As Object, e As KeyPressEventArgs) Handles rbtTag.KeyPress

    End Sub

    Private Sub rbtRecdate_CheckedChanged(sender As Object, e As EventArgs) Handles rbtRecdate.CheckedChanged
        If rbtRecdate.Checked Then
            cmbCounter_MAN.Enabled = False
            cmbItem_MAN.Enabled = False
            chkcmbsubitem.Enabled = False
            cmbMetalType.Enabled = False
            txtFrom_WET.Enabled = False
            txtTo_WET.Enabled = False
            txtsubitemid.Enabled = False
            txtTagno.Enabled = False
            'txtAgeFrom_NUM.Enabled = False
            'txtAgeTo_NUM.Enabled = False
            If Costcentre = True Then
                cmbCostcentre.Enabled = True
                LoadCostcentre(cmbCostcentre, cnCostId)
            End If
            dtpFromDate_OWN.Enabled = True
            dtpToDate_OWN.Enabled = True
        ElseIf rbtAge.Checked Then
            cmbCounter_MAN.Enabled = False
            cmbItem_MAN.Enabled = False
            chkcmbsubitem.Enabled = False
            cmbMetalType.Enabled = False
            txtsubitemid.Enabled = False
            txtTagno.Enabled = False
            If Costcentre = True Then
                cmbCostcentre.Enabled = True
                LoadCostcentre(cmbCostcentre, cnCostId)
            End If
            dtpFromDate_OWN.Enabled = False
            dtpToDate_OWN.Enabled = False
            'txtAgeFrom_NUM.Enabled = True
            'txtAgeTo_NUM.Enabled = True
        Else
            cmbCounter_MAN.Enabled = True
            cmbItem_MAN.Enabled = True
            chkcmbsubitem.Enabled = True
            cmbMetalType.Enabled = True
            txtFrom_WET.Enabled = True
            txtTo_WET.Enabled = True
            txtsubitemid.Enabled = True
            txtTagno.Enabled = False
            ''cmbCostcentre.Enabled = False
            If Comm_Costcentre_Based Then
                cmbCostcentre.Enabled = True
            Else
                cmbCostcentre.Enabled = False
            End If
            dtpFromDate_OWN.Enabled = False
            dtpToDate_OWN.Enabled = False
        End If
    End Sub

    Private Sub rbtAge_CheckedChanged(sender As Object, e As EventArgs) Handles rbtAge.CheckedChanged
        If rbtRecdate.Checked Then
            cmbCounter_MAN.Enabled = False
            cmbItem_MAN.Enabled = False
            chkcmbsubitem.Enabled = False
            cmbMetalType.Enabled = False
            txtFrom_WET.Enabled = False
            txtTo_WET.Enabled = False
            txtsubitemid.Enabled = False
            txtTagno.Enabled = False
            'txtAgeFrom_NUM.Enabled = False
            'txtAgeTo_NUM.Enabled = False
            If Costcentre = True Then
                cmbCostcentre.Enabled = True
                LoadCostcentre(cmbCostcentre, cnCostId)
            End If
            dtpFromDate_OWN.Enabled = True
            dtpToDate_OWN.Enabled = True
        ElseIf rbtAge.Checked Then
            cmbCounter_MAN.Enabled = False
            cmbItem_MAN.Enabled = False
            chkcmbsubitem.Enabled = False
            cmbMetalType.Enabled = False
            txtsubitemid.Enabled = False
            txtTagno.Enabled = False
            If Costcentre = True Then
                cmbCostcentre.Enabled = True
                LoadCostcentre(cmbCostcentre, cnCostId)
            End If
            dtpFromDate_OWN.Enabled = False
            dtpToDate_OWN.Enabled = False
            txtFrom_WET.Enabled = True
            txtTo_WET.Enabled = True
            'txtAgeFrom_NUM.Enabled = True
            'txtAgeTo_NUM.Enabled = True
        Else
            cmbCounter_MAN.Enabled = True
            cmbItem_MAN.Enabled = True
            chkcmbsubitem.Enabled = True
            cmbMetalType.Enabled = True
            txtFrom_WET.Enabled = True
            txtTo_WET.Enabled = True
            txtsubitemid.Enabled = True
            txtTagno.Enabled = False
            ''cmbCostcentre.Enabled = False
            If Comm_Costcentre_Based Then
                cmbCostcentre.Enabled = True
            Else
                cmbCostcentre.Enabled = False
            End If
            dtpFromDate_OWN.Enabled = False
            dtpToDate_OWN.Enabled = False
        End If
    End Sub
End Class
