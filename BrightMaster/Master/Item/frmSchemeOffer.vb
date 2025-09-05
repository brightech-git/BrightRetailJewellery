Imports System.Data.OleDb
Public Class frmSchemeOffer
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim flagUpdate As Boolean
    Dim strSql As String
    Dim dt As New DataTable
    Dim dtcost As New DataTable
    Dim da1 As OleDbDataAdapter
    Dim CSchemeid As Integer
    Dim CItemid As Integer
    Dim CSubItemid As Integer
    Dim flper As Integer
    Dim vasl As Integer
    Dim CCostid As String
    Dim dtCostCentre As New DataTable


    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        'CmbItem.Text = "ALL"
        strSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST ORDER BY ITEMNAME"
        objGPack.FillCombo(strSql, CmbItem, False, False)

        strSql = " SELECT NAME FROM " & cnAdminDb & "..CREDITCARD WHERE CARDTYPE='C' ORDER BY NAME"
        objGPack.FillCombo(strSql, cmbScheme, False, False)

        CmbActive.Items.Add("YES")
        CmbActive.Items.Add("NO")

        cmbVbc.Items.Add("YES")
        cmbVbc.Items.Add("NO")
        funcNew()

    End Sub
   
    Function funcClear()
        cmbScheme.Text = ""
        CmbItem.Text = ""
        cmbSubItem.Items.Clear()
        cmbSubItem.Text = ""
        txtVaSlab.Clear()
        txtWastageslab.Clear()
        txtMcSlab.Clear()
        txtFlatPer.Clear()

        Return 0
    End Function
    Function funcNew()
        tabMain.SelectedTab = tabGen
        flagUpdate = False
        funcClear()
        cmbScheme.Focus()
        cmbVbc.Text = "YES"
        CmbActive.Text = "YES"
        'cmbCostCentre.Text = ""
        chkCmbCostCentre.Enabled = True
        chkCmbCostCentre.Text = ""
        chkCmbCostCentre.Items.Clear()
        If GetAdmindbSoftValue("COSTCENTRE", "N") = "Y" Then
            strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            'objGPack.FillCombo(strSql, cmbCostCentre, False, False)
            dtCostCentre = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtCostCentre)
            BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , )
            chkCmbCostCentre.Enabled = True
            'cmbCostCentre.Enabled = True
        Else
            chkCmbCostCentre.Enabled = False
            'cmbCostCentre.Enabled = False
        End If
        Return 0
    End Function
    Function funcExit() As Integer
        Me.Close()
    End Function
    Function funcSave() As Integer
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Function
        If objGPack.Validator_Check(Me) Then Exit Function
        If Not flagUpdate Then
            funcAdd()
        Else
            funcUpdate()
        End If
    End Function
    Function funcAdd()

        Dim itemid, subitemid, schemeid, flatper, vaslab, wastslab, mcslab As Integer
        Dim costid As String = ""
        Dim flagadd As Boolean = False

        If cmbScheme.Text = "" Then
            MsgBox("Scheme name should not empty", MsgBoxStyle.Information)
            cmbScheme.Focus()
            Exit Function
        End If
        If CmbItem.Text = "" Then
            MsgBox("Item name should not empty", MsgBoxStyle.Information)
            CmbItem.Focus()
            Exit Function
        End If

        If (txtVaSlab.Text = "" And txtWastageslab.Text = "" And txtMcSlab.Text = "") And txtFlatPer.Text = "" Then
            MsgBox("Flat % and vaslab % should not empty", MsgBoxStyle.Information)
            txtFlatPer.Focus()
            Exit Function
        End If
        If chkCmbCostCentre.Enabled = True And chkCmbCostCentre.CheckedItems.Count = 0 Then
            MsgBox("CostCentre should not empty", MsgBoxStyle.Information)
            chkCmbCostCentre.Focus()
            Exit Function
        End If

        Dim dtscheme As New DataTable
        strSql = " SELECT CARDCODE FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & cmbScheme.Text & "' AND CARDTYPE='C'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtscheme)
        If dtscheme.Rows.Count > 0 Then
            schemeid = Val(dtscheme.Rows(0).Item("CARDCODE").ToString)
        End If

        Dim dtitem As New DataTable
        strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & CmbItem.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtitem)
        If dtitem.Rows.Count > 0 Then
            itemid = Val(dtitem.Rows(0).Item("ItemId").ToString)
        End If

        If cmbSubItem.Text <> "" Or cmbSubItem.Text <> "ALL" Then
            Dim dtsubItem As New DataTable
            strSql = " SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem.Text & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtsubItem)
            If dtsubItem.Rows.Count > 0 Then
                subitemid = Val(dtsubItem.Rows(0).Item("SUBITEMID").ToString)
            End If
        Else
            subitemid = 0
        End If

        Dim dtflatper As New DataTable
        strSql = " SELECT FLATPER FROM " & cnAdminDb & "..SCHEMEOFFERMAST WHERE FLATPER= " & Val(txtFlatPer.Text)
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtflatper)
        If dtflatper.Rows.Count > 0 Then
            flatper = Val(dtflatper.Rows(0).Item("FLATPER").ToString)
        End If

        Dim dtvaslab As New DataTable
        strSql = " SELECT VA_SLAB FROM " & cnAdminDb & "..SCHEMEOFFERMAST WHERE VA_SLAB= '" & Val(txtVaSlab.Text) & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtvaslab)
        If dtvaslab.Rows.Count > 0 Then
            vaslab = Val(dtvaslab.Rows(0).Item("VA_SLAB").ToString)
        End If

        Dim dtwastslab As New DataTable
        strSql = " SELECT WAST_SLAB FROM " & cnAdminDb & "..SCHEMEOFFERMAST WHERE WAST_SLAB= '" & Val(txtWastageslab.Text) & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtwastslab)
        If dtwastslab.Rows.Count > 0 Then
            wastslab = Val(dtwastslab.Rows(0).Item("WAST_SLAB").ToString)
        End If

        Dim dtmcslab As New DataTable
        strSql = " SELECT MC_SLAB FROM " & cnAdminDb & "..SCHEMEOFFERMAST WHERE MC_SLAB= '" & Val(txtMcSlab.Text) & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtmcslab)
        If dtmcslab.Rows.Count > 0 Then
            mcslab = Val(dtmcslab.Rows(0).Item("MC_SLAB").ToString)
        End If

        If chkCmbCostCentre.Enabled = True And chkCmbCostCentre.CheckedItems.Count > 0 Then
            For i As Integer = 0 To chkCmbCostCentre.CheckedItems.Count - 1
                If chkCmbCostCentre.Text <> "" Then
                    Dim dtcost As New DataTable
                    strSql = " SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE  COSTNAME = '" & chkCmbCostCentre.CheckedItems(i).ToString() & "'"
                    da = New OleDbDataAdapter(strSql, cn)
                    da.Fill(dtcost)
                    If dtcost.Rows.Count > 0 Then
                        costid = dtcost.Rows(0).Item("COSTID").ToString
                    Else
                        costid = ""
                    End If
                Else
                    costid = ""
                End If

                strSql = " SELECT 'CHECK' FROM " & cnAdminDb & "..SCHEMEOFFERMAST"
                strSql += " WHERE SCHEMEID = " & schemeid & ""
                strSql += " AND ITEMID = " & itemid & ""
                strSql += " AND SUBITEMID = " & subitemid & ""
                strSql += " AND FLATPER = " & flatper & ""
                strSql += " AND VA_SLAB = " & vaslab & ""
                strSql += " AND WAST_SLAB = " & wastslab & ""
                strSql += " AND MC_SLAB = " & mcslab & ""
                If chkCmbCostCentre.Text <> "" Or chkCmbCostCentre.Text <> "ALL" Then
                    strSql += " AND COSTID ='" & costid & "'"
                End If
                If objGPack.GetSqlValue(strSql).Length > 0 Then
                    'MsgBox("This Group Already Exist")
                    Continue For
                End If

                Try
                    strSql = " INSERT INTO " & cnAdminDb & "..SCHEMEOFFERMAST"
                    strSql += vbCrLf + " (SCHEMEID,ITEMID,SUBITEMID,FLATPER,VA_SLAB,WAST_SLAB,MC_SLAB,VBC,ACTIVE,USERID,UPDATED,UPTIME,COSTID"
                    strSql += vbCrLf + " ) VALUES ("
                    strSql += vbCrLf + " " & schemeid & "" 'SCHEMEID
                    strSql += vbCrLf + "," & itemid & "" 'ITEMID
                    strSql += vbCrLf + "," & subitemid & "" 'SUBITEMID
                    strSql += vbCrLf + "," & Val(txtFlatPer.Text) & "" 'FLATPER
                    strSql += vbCrLf + "," & Val(txtVaSlab.Text) & "" 'VA_SLAB
                    strSql += vbCrLf + "," & Val(txtWastageslab.Text) & "" 'WAST_SLAB
                    strSql += vbCrLf + "," & Val(txtMcSlab.Text) & "" 'VA_SLAB
                    strSql += vbCrLf + ",'" & Mid(cmbVbc.Text, 1, 1) & "'" 'VBC
                    strSql += vbCrLf + ",'" & Mid(CmbActive.Text, 1, 1) & "'" 'ACTIVE
                    strSql += vbCrLf + "," & userId & "" 'USERID
                    strSql += vbCrLf + ",'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                    strSql += vbCrLf + ",'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                    strSql += vbCrLf + ",'" & costid & "')" 'COSTID
                    ExecQuery(SyncMode.Master, strSql, cn, tran, costid)
                    flagadd = True
                    'cmd = New OleDbCommand(strSql, cn)
                    'cmd.ExecuteNonQuery()

                Catch ex As Exception
                    If cn.State = ConnectionState.Open Then

                    End If
                    MsgBox(ex.Message)
                    MsgBox(ex.StackTrace)
                Finally
                    If cn.State = ConnectionState.Open Then

                    End If
                End Try
            Next
        Else
            strSql = " SELECT 'CHECK' FROM " & cnAdminDb & "..SCHEMEOFFERMAST"
            strSql += " WHERE SCHEMEID = " & schemeid & ""
            strSql += " AND ITEMID = " & itemid & ""
            strSql += " AND SUBITEMID = " & subitemid & ""
            strSql += " AND FLATPER = " & flatper & ""
            strSql += " AND VA_SLAB = " & vaslab & ""
            strSql += " AND WAST_SLAB = " & wastslab & ""
            strSql += " AND MC_SLAB = " & mcslab & ""
            If objGPack.GetSqlValue(strSql).Length > 0 Then
                MsgBox("This Group Already Exist")
                cmbScheme.Focus()
                Exit Function
            End If


            strSql = " INSERT INTO " & cnAdminDb & "..SCHEMEOFFERMAST"
            strSql += vbCrLf + " (SCHEMEID,ITEMID,SUBITEMID,FLATPER,VA_SLAB,WAST_SLAB,MC_SLAB,VBC,ACTIVE,USERID,UPDATED,UPTIME,COSTID"
            strSql += vbCrLf + " ) VALUES ("
            strSql += vbCrLf + " " & schemeid & "" 'SCHEMEID
            strSql += vbCrLf + "," & itemid & "" 'ITEMID
            strSql += vbCrLf + "," & subitemid & "" 'SUBITEMID
            strSql += vbCrLf + "," & Val(txtFlatPer.Text) & "" 'FLATPER
            strSql += vbCrLf + "," & Val(txtVaSlab.Text) & "" 'VA_SLAB
            strSql += vbCrLf + "," & Val(txtWastageslab.Text) & "" 'WAST_SLAB
            strSql += vbCrLf + "," & Val(txtMcSlab.Text) & "" 'MC_SLAB
            strSql += vbCrLf + ",'" & Mid(cmbVbc.Text, 1, 1) & "'" 'VBC
            strSql += vbCrLf + ",'" & Mid(CmbActive.Text, 1, 1) & "'" 'ACTIVE
            strSql += vbCrLf + "," & userId & "" 'USERID
            strSql += vbCrLf + ",'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
            strSql += vbCrLf + ",'" & Date.Now.ToLongTimeString & "'" 'UPTIME
            strSql += vbCrLf + ",'" & costid & "')" 'COSTID
            flagadd = True
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
        End If
        If flagadd = True Then
            MsgBox("Saved..", MsgBoxStyle.Information)
            funcNew()
        Else
            MsgBox("Group Already Exist", MsgBoxStyle.Information)
        End If
        Return 0
    End Function


    'If cmbCostCentre.Text <> "" Then
    '    Dim dtcost As New DataTable
    '    strSql = " SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre.Text & "'"
    '    da = New OleDbDataAdapter(strSql, cn)
    '    da.Fill(dtcost)
    '    If dtcost.Rows.Count > 0 Then
    '        costid = dtcost.Rows(0).Item("COSTID").ToString
    '    Else
    '        costid = ""
    '    End If
    'Else
    '    costid = ""
    'End If
    'strSql = " SELECT 'CHECK' FROM " & cnAdminDb & "..SCHEMEOFFERMAST"
    'strSql += " WHERE SCHEMEID = " & schemeid & ""
    'strSql += " AND ITEMID = " & itemid & ""
    'strSql += " AND SUBITEMID = " & subitemid & ""
    'If cmbCostCentre.Text <> "" Or cmbCostCentre.Text <> "ALL" Then
    '    strSql += " AND COSTID ='" & costid & "'"
    'End If
    'If objGPack.GetSqlValue(strSql).Length > 0 Then
    '    MsgBox("This Group Already Exist")
    '    Exit Function
    'End If
    '    Try
    '        strSql = vbCrLf + " INSERT INTO " & cnAdminDb & "..SCHEMEOFFERMAST"
    '        strSql += vbCrLf + " (SCHEMEID,ITEMID,SUBITEMID,FLATPER,VA_SLAB,VBC,ACTIVE,USERID,UPDATED,UPTIME,COSTID"
    '        strSql += vbCrLf + " ) VALUES ("
    '        strSql += vbCrLf + " " & schemeid & "" 'SCHEMEID
    '        strSql += vbCrLf + "," & itemid & "" 'ITEMID
    '        strSql += vbCrLf + "," & subitemid & "" 'SUBITEMID
    '        strSql += vbCrLf + "," & Val(txtFlatPer.Text) & "" 'FLATPER
    '        strSql += vbCrLf + "," & Val(txtVaSlab.Text) & "" 'VA_SLAB
    '        strSql += vbCrLf + ",'" & Mid(cmbVbc.Text, 1, 1) & "'" 'VBC
    '        strSql += vbCrLf + ",'" & Mid(CmbActive.Text, 1, 1) & "'" 'ACTIVE
    '        strSql += vbCrLf + "," & userId & "" 'USERID
    '        strSql += vbCrLf + ",'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
    '        strSql += vbCrLf + ",'" & Date.Now.ToLongTimeString & "'" 'UPTIME
    '        strSql += vbCrLf + ",'" & costid & "')" 'COSTID

    '        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
    '        cmd.ExecuteNonQuery()
    '        MsgBox("Saved..", MsgBoxStyle.Information)
    '        funcNew()
    '    Catch ex As Exception
    '        If cn.State = ConnectionState.Open Then

    '        End If
    '        MsgBox(ex.Message)
    '        MsgBox(ex.StackTrace)
    '    Finally
    '        If cn.State = ConnectionState.Open Then

    '        End If
    '    End Try
    '    Return 0
    'End Function
    Function funcUpdate()

        Dim itemid, subitemid, schemeid, flatper, vaslab, wastslab, mcslab As Integer
        Dim costid As String

        Dim dtscheme As New DataTable
        strSql = " SELECT CARDCODE FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & cmbScheme.Text & "' AND CARDTYPE='C'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtscheme)
        If dtscheme.Rows.Count > 0 Then
            schemeid = Val(dtscheme.Rows(0).Item("CARDCODE").ToString)
        End If


        Dim dtitem As New DataTable
        strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & CmbItem.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtitem)
        If dtitem.Rows.Count > 0 Then
            itemid = Val(dtitem.Rows(0).Item("ItemId").ToString)
        End If

        If cmbSubItem.Text <> "" Or cmbSubItem.Text <> "ALL" Then
            Dim dtsubItem As New DataTable
            strSql = " SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem.Text & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtsubItem)
            If dtsubItem.Rows.Count > 0 Then
                subitemid = Val(dtsubItem.Rows(0).Item("SUBITEMID").ToString)
            End If
        Else
            subitemid = 0
        End If

        If chkCmbCostCentre.Text <> "" Then
            Dim dtcost As New DataTable
            strSql = " SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & chkCmbCostCentre.Text & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtcost)
            If dtcost.Rows.Count > 0 Then
                costid = dtcost.Rows(0).Item("COSTID").ToString
            Else
                costid = ""
            End If
        Else
            costid = ""
        End If

        Dim dtflatper As New DataTable
        strSql = " SELECT FLATPER FROM " & cnAdminDb & "..SCHEMEOFFERMAST WHERE FLATPER= '" & Val(txtFlatPer.Text) & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtflatper)
        If dtflatper.Rows.Count > 0 Then
            flatper = Val(dtflatper.Rows(0).Item("FLATPER").ToString)
        End If

        Dim dtvaslab As New DataTable
        strSql = " SELECT VA_SLAB FROM " & cnAdminDb & "..SCHEMEOFFERMAST WHERE VA_SLAB= '" & Val(txtVaSlab.Text) & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtvaslab)
        If dtvaslab.Rows.Count > 0 Then
            vaslab = Val(dtvaslab.Rows(0).Item("VA_SLAB").ToString)
        End If
        Dim dtwastslab As New DataTable
        strSql = " SELECT WAST_SLAB FROM " & cnAdminDb & "..SCHEMEOFFERMAST WHERE WAST_SLAB= '" & Val(txtWastageslab.Text) & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtwastslab)
        If dtwastslab.Rows.Count > 0 Then
            wastslab = Val(dtwastslab.Rows(0).Item("WAST_SLAB").ToString)
        End If

        Dim dtmcslab As New DataTable
        strSql = " SELECT MC_SLAB FROM " & cnAdminDb & "..SCHEMEOFFERMAST WHERE MC_SLAB= '" & Val(txtMcSlab.Text) & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtmcslab)
        If dtmcslab.Rows.Count > 0 Then
            mcslab = Val(dtmcslab.Rows(0).Item("MC_SLAB").ToString)
        End If

        strSql = " SELECT 'CHECK' FROM " & cnAdminDb & "..SCHEMEOFFERMAST"
        strSql += " WHERE SCHEMEID = " & schemeid & ""
        strSql += " AND ITEMID = " & itemid & ""
        strSql += " AND SUBITEMID = " & subitemid & ""
        strSql += " AND FLATPER = " & Val(txtFlatPer.Text) & ""
        strSql += " AND VA_SLAB = " & Val(txtVaSlab.Text) & ""
        strSql += " AND WAST_SLAB = " & Val(txtWastageslab.Text) & ""
        strSql += " AND MC_SLAB = " & Val(txtMcSlab.Text) & ""
        If chkCmbCostCentre.Enabled = True And chkCmbCostCentre.CheckedItems.Count > 0 Then
            strSql += " AND COSTID ='" & costid & "'"
        End If
        If objGPack.GetSqlValue(strSql).Length > 0 Then
            MsgBox("This Group Already Exist")
            cmbScheme.Focus()
            Exit Function
        End If


        strSql = " UPDATE " & cnAdminDb & "..SCHEMEOFFERMAST SET "
        strSql += vbCrLf + " SCHEMEID=" & schemeid & "" 'SCHEMEID
        strSql += vbCrLf + " ,ITEMID=" & itemid & "" 'ITEMID
        strSql += vbCrLf + " ,SUBITEMID=" & subitemid & "" 'SUBITEMID
        strSql += vbCrLf + " ,FLATPER=" & Val(txtFlatPer.Text) & "" 'FLATPER
        strSql += vbCrLf + " ,VA_SLAB=" & Val(txtVaSlab.Text) & "" 'VA_SLAB
        strSql += vbCrLf + " ,WAST_SLAB=" & Val(txtWastageslab.Text) & "" 'WAST_SLAB
        strSql += vbCrLf + " ,MC_SLAB=" & Val(txtMcSlab.Text) & "" 'MC_SLAB
        strSql += vbCrLf + " ,VBC='" & Mid(cmbVbc.Text, 1, 1) & "'" 'VBC
        strSql += vbCrLf + " ,ACTIVE='" & Mid(CmbActive.Text, 1, 1) & "'" 'ACTIVE
        strSql += vbCrLf + " ,USERID=" & userId & "" 'USERID
        strSql += vbCrLf + " ,UPDATED='" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        strSql += vbCrLf + " ,UPTIME='" & Date.Now.ToLongTimeString & "'" 'UPTIME
        strSql += vbCrLf + " ,COSTID='" & costid & "'" 'COSTID
        strSql += vbCrLf + " WHERE COSTID = '" & CCostid & "' AND ITEMID=" & CItemid & " AND SUBITEMID=" & CSubItemid & ""
        strSql += vbCrLf + " AND SCHEMEID= " & CSchemeid & ""
        Try
            ExecQuery(SyncMode.Master, strSql, cn, tran, costid)
            MsgBox("Updated..", MsgBoxStyle.Information)
            funcNew()
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
        Return 0
    End Function

    Private Sub frmSchemeOffer_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        ElseIf e.KeyChar = Chr(Keys.Escape) Then
            btnBack_Click(Me, New EventArgs)
        End If
    End Sub
    Private Sub frmSchemeOffer_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.tabMain.ItemSize = New Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGen.Left, Me.tabGen.Top, Me.tabGen.Width, Me.tabGen.Height))
        Me.tabMain.SelectedTab = tabGen
        funcNew()
        cmbScheme.Select()
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        funcSave()
    End Sub

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click, btnOpen.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        funcView()
        gridView.Focus()
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


    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        funcNew()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        funcExit()
    End Sub


    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            If Not gridView.RowCount > 0 Then Exit Sub
            gridView.CurrentCell = gridView.Rows(gridView.CurrentRow.Index).Cells("SCHEME")
            With gridView.CurrentRow
                cmbScheme.Text = .Cells("SCHEME").Value.ToString
                CmbItem.Text = .Cells("ITEMNAME").Value.ToString
                cmbSubItem.Text = .Cells("SUBITEMNAME").Value.ToString
                txtFlatPer.Text = .Cells("FLATPER").Value.ToString
                txtVaSlab.Text = .Cells("VA_SLAB").Value.ToString
                txtWastageslab.Text = .Cells("WAST_SLAB").Value.ToString
                txtMcSlab.Text = .Cells("MC_SLAB").Value.ToString
                chkCmbCostCentre.Text = .Cells("COSTNAME").Value.ToString
                'chkCmbCostCentre.Enabled = False

                'cmbCostCentre.Text = .Cells("COSTNAME").Value.ToString
                cmbVbc.Text = .Cells("VBC").Value.ToString
                CmbActive.Text = IIf((.Cells("ACTIVE").Value.ToString) = "Y", "YES", "NO")
                CSchemeid = Val(.Cells("SCHEMEID").Value.ToString)
                CItemid = Val(.Cells("ITEMID").Value.ToString)
                CSubItemid = Val(.Cells("SUBITEMID").Value.ToString)
                CCostid = .Cells("COSTID").Value.ToString
            End With
            tabMain.SelectedTab = tabGen
            cmbScheme.Focus()
            flagUpdate = True
        End If
    End Sub



    Function funcGridStyle() As Integer
        With gridView
            .RowHeadersVisible = False
            .Columns("SCHEMEID").Visible = False
            .Columns("ITEMID").Visible = False
            .Columns("SUBITEMID").Visible = False
            .Columns("COSTID").Visible = False
            .Columns("ACTIVE").Visible = False
            .Columns("COSTNAME").HeaderText = "COSTCENTRE"
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Regular)
        End With

    End Function

    Function funcView() As Integer
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Function
        Try
            Me.btnOpen.Enabled = False
            'Me.Cursor = Cursors.WaitCursor

            strSql = vbCrLf + " SELECT S.SCHEMEID,S.ITEMID,S.SUBITEMID,S.COSTID,C.NAME AS SCHEME,I.ITEMNAME"
            strSql += vbCrLf + " ,ISNULL(SI.SUBITEMNAME,'') AS SUBITEMNAME,FLATPER,VA_SLAB,WAST_SLAB,MC_SLAB"
            strSql += vbCrLf + " ,CASE WHEN VBC='Y' THEN 'YES' ELSE 'NO' END VBC,S.ACTIVE"
            strSql += vbCrLf + " ,ISNULL(CO.COSTNAME,'') AS COSTNAME FROM " & cnAdminDb & "..SCHEMEOFFERMAST S"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CREDITCARD C ON C.CARDCODE=S.SCHEMEID "
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST  I ON I.ITEMID=S.ITEMID "
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.SUBITEMID =S.SUBITEMID  "
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..COSTCENTRE CO ON CO.COSTID=S.COSTID "

            dt = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)

            If dt.Rows.Count <= 0 Then
                MsgBox("No Records to View", MsgBoxStyle.Information)
                Exit Function
            End If

            gridView.DataSource = dt
            funcGridStyle()
            If gridView.Rows.Count > 0 Then
                tabMain.SelectedTab = tabView
                gridView.Focus()
            End If
        Catch ex As Exception
            MsgBox("Error : " & ex.Message & " Position : " & MsgBox(ex.StackTrace), MsgBoxStyle.Critical)
        Finally
            Me.btnOpen.Enabled = True
            Me.Cursor = Cursors.Arrow
        End Try
    End Function

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGen
        cmbScheme.Focus()
    End Sub

    Private Sub CmbItem_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmbItem.SelectedIndexChanged
        If CmbItem.Text <> "" Then
            cmbSubItem.Text = ""
            cmbSubItem.Items.Clear()
            cmbSubItem.Items.Add("ALL")
            strSql = " Select SubItemName from " & cnAdminDb & "..SubItemMast "
            strSql += " Where ItemId = "
            strSql += "(select ItemId from " & cnAdminDb & "..ItemMast where ItemName = '" & CmbItem.Text & "')"
            strSql += " Order by SubItemName"
            objGPack.FillCombo(strSql, cmbSubItem, False, False)
        End If
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If Not gridView.RowCount > 0 Then
            Exit Sub
        End If
        Dim wastsl, mcsl As Integer
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
        Dim chkQry As String = "SELECT 1 FROM " & cnAdminDb & "..SCHEMEOFFERMAST WHERE 1<>1"
        Dim schmid As String = gridView.Rows(gridView.CurrentRow.Index).Cells("SCHEMEID").Value.ToString
        Dim itmid As String = gridView.Rows(gridView.CurrentRow.Index).Cells("ITEMID").Value.ToString
        Dim subitmid As String = gridView.Rows(gridView.CurrentRow.Index).Cells("SUBITEMID").Value.ToString
        Dim cstid As String = gridView.Rows(gridView.CurrentRow.Index).Cells("COSTID").Value.ToString
        flper = gridView.Rows(gridView.CurrentRow.Index).Cells("FLATPER").Value.ToString
        vasl = gridView.Rows(gridView.CurrentRow.Index).Cells("VA_SLAB").Value.ToString
        wastsl = gridView.Rows(gridView.CurrentRow.Index).Cells("WAST_SLAB").Value.ToString
        mcsl = gridView.Rows(gridView.CurrentRow.Index).Cells("MC_SLAB").Value.ToString

        DeleteItem(SyncMode.Master, chkQry, "DELETE FROM " & cnAdminDb & "..SCHEMEOFFERMAST WHERE SCHEMEID = '" & schmid & "'AND ITEMID='" & itmid & "' AND SUBITEMID='" & subitmid & "' AND COSTID='" & cstid & "' AND FLATPER='" & flper & "' AND VA_SLAB='" & vasl & "' AND WAST_SLAB='" & wastsl & "' AND MC_SLAB='" & mcsl & "'")
        funcView()
        If gridView.RowCount > 0 Then gridView.Focus() Else btnBack_Click(Me, New EventArgs)


    End Sub
End Class