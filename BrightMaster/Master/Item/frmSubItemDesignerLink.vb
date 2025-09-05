Imports System.Data.OleDb
Public Class frmSubItemDesignerLink
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim flagSave As Boolean = False
    Dim flagUpdate As Boolean = False
    Dim Sno As String = Nothing
    Dim chkMetal As String = ""
    Dim chkItem As String = ""
    Dim chkMargin As String = ""
    Dim dtRow As New DataTable
    Dim dtGridView As New DataTable
    Dim dRow As DataRow = Nothing
    Dim dViewRow As DataRow = Nothing
    Dim dAccode As String = Nothing


    Private Sub frmSubItemDesignerLink_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        tbMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tbMain.Region = New Region(New RectangleF(Me.tbGen.Left, Me.tbGen.Top, Me.tbGen.Width, Me.tbGen.Height))

        dtGridView.Columns.Add("ITEMNAME")
        dtGridView.Columns.Add("SUBITEMNAME")
        dtGridView.Columns.Add("STNITEMNAME")
        dtGridView.Columns.Add("STNSUBITEMNAME")
        dtGridView.Columns.Add("DESIGNERNAME")
        funcNew()

    End Sub


    Private Sub frmSubItemDesignerLink_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        ElseIf e.KeyChar = Chr(Keys.Escape) Then
            If tbMain.SelectedIndex = 1 Then tbMain.SelectedIndex = 0
        End If
    End Sub

    Private Sub bnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bnNew.Click, NewToolStripMenuItem.Click
        funcNew()
    End Sub

    Private Sub bnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bnExit.Click, ExitToolStripMenuItem.Click
        funcExit()
    End Sub
    Function funcExit() As Integer
        Me.Close()
    End Function
    Function funcNew() As Integer
        objGPack.TextClear(Me)
        flagSave = False
        flagUpdate = False
        funcCallGrid()
    
        SearchDesigner(cmb_Designer)
        SearchItem(cmb_itemName)

        cmb_StuddedItemName.Items.Clear()
        cmbSubItem_OWN.Items.Add("ALL")
        strSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST "
        strSql += " WHERE ISNULL(STUDDED,'') = 'S' AND ACTIVE = 'Y' ORDER BY ITEMNAME"
        objGPack.FillCombo(strSql, cmb_StuddedItemName, , False)
        cmbSubItem_OWN.Items.Add("ALL")


        SearchDesigner(cmbDesigner_OWN)
        SearchItem(cmbItem_OWN)

        dtRow.Clear()
        cmbActive.Text = "YES"
        cmb_Designer.Focus()
    End Function

 
    Function funcSave() As Integer
        ' If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Function
        If objGPack.Validator_Check(Me) Then Exit Function

        If flagSave = False Then
            funcAdd()
        Else
            funcUpdate()
        End If
    End Function
    Private Sub funcAdd()
        Dim ItemId As Integer = Nothing
        Dim SubItemId As Integer = Nothing
        Dim StnItemId As Integer = Nothing
        Dim StnSubItemId As Integer = Nothing
        Dim DesignerId As Integer = Nothing
        Dim dtItem, dtSubItem, dtStnItem, dtStnSubItem, dtDesignerId As New DataTable()
        dtItem.Clear()
        dtSubItem.Clear()
        dtDesignerId.Clear()
        dtStnItem.Clear()
        dtStnSubItem.Clear()
        If cmb_itemName.Text = "" Then
            MsgBox("Item Name should not empty", MsgBoxStyle.Information)
            cmb_itemName.Focus()
            Exit Sub
        End If
        If cmb_Designer.Text = "" Then
            MsgBox("Designer Name should not empty", MsgBoxStyle.Information)
            cmb_Designer.Focus()
            Exit Sub
        End If
        strSql = " Select ItemId from " & cnAdminDb & "..ItemMast where ItemName= '" & cmb_itemName.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtItem)
        If dtItem.Rows.Count > 0 Then
            ItemId = dtItem.Rows(0).Item("ITEMID").ToString
        Else
            ItemId = 0
        End If

        strSql = " Select SubItemId from " & cnAdminDb & "..SubItemMast where SubItemName= '" & cmbSubitemName.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtSubItem)
        If dtSubItem.Rows.Count > 0 Then
            SubItemId = dtSubItem.Rows(0).Item("SUBITEMID").ToString
        Else
            SubItemId = 0
        End If

        strSql = " Select ItemId from " & cnAdminDb & "..ItemMast where ItemName= '" & cmb_StuddedItemName.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtStnItem)
        If dtStnItem.Rows.Count > 0 Then
            StnItemId = dtStnItem.Rows(0).Item("ITEMID").ToString
        Else
            StnItemId = 0
        End If

        strSql = " Select SubItemId from " & cnAdminDb & "..SubItemMast where SubItemName= '" & cmbStuddedSubitem.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtStnSubItem)
        If dtStnSubItem.Rows.Count > 0 Then
            StnSubItemId = dtStnSubItem.Rows(0).Item("SUBITEMID").ToString
        Else
            StnSubItemId = 0
        End If

        If cmb_Designer.Text <> "ALL" Then
            Dim sql As String = "SELECT DESIGNERID  FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME= '" & cmb_Designer.Text & "'"
            da = New OleDbDataAdapter(sql, cn)
            'dtAccode.Clear()
            da.Fill(dtDesignerId)
            If dtDesignerId.Rows.Count > 0 Then
                DesignerId = dtDesignerId.Rows(0).Item("DESIGNERID").ToString
            Else
                DesignerId = 0
            End If
        Else
            DesignerId = 0
        End If
        strSql = " SELECT COUNT(*) FROM " & cnAdminDb & "..DESIGNERSTONE WHERE ITEMID=" & ItemId & " AND SUBITEMID=" & SubItemId & " AND DESIGNERID=" & DesignerId & ""
        If Val(GetSqlValue(cn, strSql)) > 0 Then
            MsgBox("This Designer and Item already saved ", MsgBoxStyle.Information)
            cmb_Designer.Focus()
            Exit Sub
        End If
        Dim designersno As String = Nothing
        designersno = GetNewSno(TranSnoType.DESIGNERSTONECODE, tran, "GET_SNO_ADMIN")

        strSql = " INSERT INTO " & cnAdminDb & "..DESIGNERSTONE "
        strSql += " (SNO,DESIGNERID,ITEMID,SUBITEMID,STNITEMID,STNSUBITEMID,STNDEDPER,STN_RATE,STONEUNIT,CALTYPE,ACTIVE,USERID,UPDATED,UPTIME)"
        strSql += " VALUES("
        strSql += "'" & designersno & "'" 'SNO
        strSql += "," & DesignerId & "" 'DESIGNERID
        strSql += "," & ItemId & "" 'ITEMID
        strSql += "," & SubItemId & "" 'SUBITEMID
        strSql += "," & StnItemId & "" 'STNITEMID
        strSql += "," & StnSubItemId & "" 'STNSUBITEMID
        strSql += "," & Val(txtStnPer.Text) & "" 'STNDEDPER
        strSql += "," & Val(txtSTNRATE.Text) & "" 'STN_RATE
        strSql += ",'" & Mid(CmbUnit.Text, 1, 1) & "'" 'STONEUNIT
        strSql += ",'" & Mid(CmbCalcType.Text, 1, 1) & "'" 'CALTYPE
        strSql += ",'" & Mid(cmbActive.Text, 1, 1) & "'" 'ACTIVE
        strSql += "," & userId & "" 'USERID
        strSql += ",'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        strSql += ",'" & Date.Now.ToLongTimeString & "')" 'UPTIME
        Try
            ExecQuery(SyncMode.Master, strSql, cn)
            funcNew()
            MsgBox("Saved Successfully...")
        Catch ex As Exception
            MsgBox(ex.Message & vbCrLf & ex.StackTrace, MsgBoxStyle.Information)
        End Try
    End Sub
    Private Sub funcUpdate()
        Dim ItemId As Integer = Nothing
        Dim SubItemId As Integer = Nothing
        Dim StnItemId As Integer = Nothing
        Dim StnSubItemId As Integer = Nothing
        Dim DesignerId As Integer = Nothing
        Dim dtItem, dtSubItem, dtStnItem, dtStnSubItem, dtDesignerId As New DataTable()
        dtItem.Clear()
        dtSubItem.Clear()
        dtDesignerId.Clear()
        dtStnItem.Clear()
        dtStnSubItem.Clear()

        ''Find ItemId

        strSql = " Select ItemId from " & cnAdminDb & "..ItemMast where ItemName= '" & cmb_itemName.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtItem)
        If dtItem.Rows.Count > 0 Then
            ItemId = dtItem.Rows(0).Item("ITEMID").ToString
        Else
            ItemId = 0
        End If

        ''Find SubItemId

        strSql = " Select SubItemId from " & cnAdminDb & "..SubItemMast where SubItemName ='" & cmbSubitemName.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtSubItem)
        If dtSubItem.Rows.Count > 0 Then
            SubItemId = dtSubItem.Rows(0).Item("SubItemid").ToString
        Else
            SubItemId = 0
        End If

        strSql = " Select ItemId from " & cnAdminDb & "..ItemMast where ItemName= '" & cmb_StuddedItemName.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtStnItem)
        If dtStnItem.Rows.Count > 0 Then
            StnItemId = dtStnItem.Rows(0).Item("ITEMID").ToString
        Else
            StnItemId = 0
        End If

        strSql = " Select SubItemId from " & cnAdminDb & "..SubItemMast where SubItemName= '" & cmbStuddedSubitem.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtStnSubItem)
        If dtStnSubItem.Rows.Count > 0 Then
            StnSubItemId = dtStnSubItem.Rows(0).Item("SUBITEMID").ToString
        Else
            StnSubItemId = 0
        End If

        ''Find DESIGNERID
        Dim sql As String = "SELECT DESIGNERID  FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME= '" & cmb_Designer.Text & "'"
        da = New OleDbDataAdapter(sql, cn)
        'dtAccode.Clear()
        da.Fill(dtDesignerId)
        If dtDesignerId.Rows.Count > 0 Then
            DesignerId = dtDesignerId.Rows(0).Item("DESIGNERID").ToString
        Else
            DesignerId = 0
        End If

        strSql = " UPDATE " & cnAdminDb & "..DESIGNERSTONE SET"
        strSql += " DESIGNERID=" & DesignerId & "" 'ITEMID
        strSql += " ,ITEMID=" & ItemId & "" 'ITEMID
        strSql += " ,SUBITEMID=" & SubItemId & "" 'SUBITEMID
        strSql += " ,STNITEMID=" & StnItemId & "" 'STNITEMID
        strSql += " ,STNSUBITEMID=" & StnSubItemId & "" 'STNSUBITEMID
        strSql += " ,STNDEDPER=" & Val(txtStnPer.Text) & "" 'STNDEDPER
        strSql += " ,STN_RATE=" & Val(txtSTNRATE.Text) & "" 'STN_RATE
        strSql += " ,STONEUNIT='" & Mid(CmbUnit.Text, 1, 1) & "'" 'STONEUNIT
        strSql += " ,CALTYPE='" & Mid(CmbCalcType.Text, 1, 1) & "'" 'CALTYPE
        strSql += " ,ACTIVE='" & Mid(cmbActive.Text, 1, 1) & "'" 'ACTIVE
        strSql += " ,USERID=" & userId & "" 'USERID
        strSql += " ,UPDATED='" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        strSql += " ,UPTIME='" & Date.Now.ToLongTimeString & "'" 'UPTIME
        strSql += " WHERE SNO = '" & Sno & "'"
        Try
            ExecQuery(SyncMode.Master, strSql, cn)
            MsgBox("Updated Successfully...")
            funcNew()
        Catch ex As Exception
            MsgBox(ex.Message & vbCrLf & ex.StackTrace, MsgBoxStyle.Information)
        End Try
    End Sub

    Private Sub bnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bnSave.Click, SaveToolStripMenuItem.Click
        funcSave()
    End Sub


    Private Sub bnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bnOpen.Click, OpenToolStripMenuItem.Click
        ' If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        tbMain.SelectedIndex = 1
        tbView.Focus()
        funcCallGrid()
        gridView.Focus()
    End Sub
    Function funcCallGrid() As Integer
        strSql = " SELECT SNO,(SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER AS D WHERE D.DESIGNERID = DS.DESIGNERID)AS DESIGNERNAME,"
        strSql += " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST AS I WHERE I.ITEMID=DS.ITEMID)AS ITEMNAME,"
        strSql += " (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST AS I WHERE I.SUBITEMID=DS.SUBITEMID)AS SUBITEMNAME,"
        strSql += " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST AS I WHERE I.ITEMID=DS.STNITEMID)AS STNITEMNAME,"
        strSql += " (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST AS I WHERE I.SUBITEMID=DS.STNSUBITEMID)AS STNSUBITEMNAME"
        strSql += " ,STNDEDPER,STN_RATE FROM " & cnAdminDb & "..DESIGNERSTONE AS DS "
        funcOpenGrid(strSql, gridView)
        gridView.Columns("SNO").Visible = False
        gridView.Columns("DESIGNERNAME").Width = 150
        gridView.Columns("ITEMNAME").Width = 150
        gridView.Columns("SUBITEMNAME").Width = 150
        gridView.Columns("STNITEMNAME").Width = 150
        gridView.Columns("STNSUBITEMNAME").Width = 150

    End Function

    Private Sub gridView_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            If gridView.RowCount > 0 Then
                gridView.CurrentCell = gridView.CurrentCell
                'Dim designerid As Integer
                'Dim sql As String = "SELECT DESIGNERID  FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME= '" & gridView.Item(0, gridView.CurrentRow.Index).Value.ToString & "'"
                'designerid = GetSqlValue(cn, sql)
                funcGetDetails(gridView.Item(0, gridView.CurrentRow.Index).Value.ToString)
                tbMain.SelectedTab = tbGen
            End If
        End If
    End Sub
    Function funcGetDetails(ByVal temp As String) As Integer
        Dim chkSubItem As String = Nothing
        Dim chkAcName As String = Nothing

        strSql = " SELECT (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST AS I WHERE I.ITEMID=DS.ITEMID)AS ITEMNAME"
        strSql += " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST AS I WHERE I.SUBITEMID=DS.SUBITEMID)AS SUBITEMNAME"
        strSql += " ,(SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER AS D WHERE D.DESIGNERID = DS.DESIGNERID)AS DESIGNERNAME"
        strSql += " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST AS I WHERE I.ITEMID=DS.STNITEMID)AS STNITEMNAME"
        strSql += " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST AS I WHERE I.SUBITEMID=DS.STNSUBITEMID)AS STNSUBITEMNAME"
        strSql += " ,STNDEDPER,STN_RATE"
        strSql += " ,CASE WHEN STONEUNIT='C' THEN 'CARAT' ELSE 'GRAM' END STONEUNIT "
        strSql += " ,CASE WHEN CALTYPE='P' THEN 'PCS' ELSE 'WEIGHT' END CALTYPE "
        strSql += " ,CASE WHEN ACTIVE='Y' THEN 'YES' ELSE 'NO' END ACTIVE "
        strSql += " FROM " & cnAdminDb & "..DESIGNERSTONE AS DS "
        strSql += " WHERE SNO='" & temp & "'"
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If Not dt.Rows.Count > 0 Then
            Return 0
        End If
        flagSave = True
        flagUpdate = True
        With dt.Rows(0)
            cmb_itemName.Text = dt.Rows(0).Item("ITEMNAME").ToString
            cmbSubitemName.Text = dt.Rows(0).Item("SUBITEMNAME").ToString
            cmb_StuddedItemName.Text = dt.Rows(0).Item("STNITEMNAME").ToString
            cmbStuddedSubitem.Text = dt.Rows(0).Item("STNSUBITEMNAME").ToString
            cmb_Designer.Text = IIf(dt.Rows(0).Item("DESIGNERNAME").ToString <> "", dt.Rows(0).Item("DESIGNERNAME").ToString, "ALL")
            txtStnPer.Text = dt.Rows(0).Item("STNDEDPER").ToString
            txtSTNRATE.Text = dt.Rows(0).Item("STN_RATE").ToString
            cmbActive.Text = .Item("ACTIVE").ToString
            CmbUnit.Text = .Item("STONEUNIT").ToString
            CmbCalcType.Text = .Item("CALTYPE").ToString
        End With
        Sno = temp
    End Function

    Private Sub btnSearch_Own_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch_Own.Click
        strSql = " SELECT SNO"
        strSql += " ,I.ITEMNAME AS ITEMNAME "
        strSql += " ,S.SUBITEMNAME AS SUBITEMNAME"
        strSql += " ,C.DESIGNERNAME AS DESIGNERNAME "
        strSql += " ,SI.ITEMNAME AS STNITEMNAME "
        strSql += " ,STS.SUBITEMNAME AS STNSUBITEMNAME"
        strSql += " FROM " & cnAdminDb & "..DESIGNERSTONE AS SR"
        strSql += " LEFT OUTER JOIN " & cnAdminDb & "..ITEMMAST I on I.ITEMID=SR.ITEMID"
        strSql += " LEFT OUTER JOIN " & cnAdminDb & "..SUBITEMMAST S on S.SUBITEMID=SR.SUBITEMID"
        strSql += " LEFT OUTER JOIN " & cnAdminDb & "..DESIGNER C on C.DESIGNERID=SR.DESIGNERID"
        strSql += " LEFT OUTER JOIN " & cnAdminDb & "..ITEMMAST SI on SI.ITEMID=SR.STNITEMID"
        strSql += " LEFT OUTER JOIN " & cnAdminDb & "..SUBITEMMAST STS on STS.SUBITEMID=SR.STNSUBITEMID"
        strSql += " WHERE 1=1"
        If cmbItem_OWN.Text.Trim <> "" And cmbItem_OWN.Text <> "ALL" Then
            strSql += " AND SR.ITEMID=(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbItem_OWN.Text & "')"
        End If
        If cmbSubItem_OWN.Text <> "" And cmbSubItem_OWN.Text <> "ALL" Then
            strSql += " AND SR.SUBITEMID=(SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME='" & cmbSubItem_OWN.Text & "')"
        End If
        If cmbDesigner_OWN.Text <> "" And cmbDesigner_OWN.Text <> "ALL" Then
            strSql += " AND SR.DESIGNERID=(SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME='" & cmbDesigner_OWN.Text & "')"
        End If
        strSql += " ORDER BY SR.DESIGNERID"
        funcOpenGrid(strSql, gridView)
        With gridView
            .Columns("SNO").Visible = False
            .Columns("ITEMNAME").Width = 150
            .Columns("SUBITEMNAME").Width = 150
            .Columns("STNITEMNAME").Width = 150
            .Columns("STNSUBITEMNAME").Width = 150
            .Columns("DESIGNERNAME").Width = 200
        End With
        gridView.Select()
    End Sub
    Private Function SearchDesigner(ByVal combo As ComboBox) As Integer
        combo.Items.Clear()
        combo.Items.Add("ALL")
        strSql = "SELECT DESIGNERNAME from " & cnAdminDb & "..DESIGNER order by DESIGNERNAME"
        objGPack.FillCombo(strSql, combo, False, False)

    End Function
    Private Function SearchItem(ByVal combo As ComboBox) As Integer
        combo.Items.Clear()
        cmbSubItem_OWN.Items.Add("ALL")
        strSql = " Select ItemName from " & cnAdminDb & "..ItemMast order by ItemName"
        objGPack.FillCombo(strSql, combo, , False)
        cmbSubItem_OWN.Items.Add("ALL")
    End Function
    Private Function SearchSubItem(ByVal combo As ComboBox, ByVal Itemname As String) As Integer
        combo.Items.Clear()
        strSql = " Select SubItemName from " & cnAdminDb & "..SubItemMast where ItemId= (Select ItemId from " & cnAdminDb & "..ItemMast where ItemName='" & Itemname & "') order by SubItemName"
        objGPack.FillCombo(strSql, combo, , False)
    End Function
    Private Function SearchStuddedSubItem(ByVal combo As ComboBox, ByVal Itemname As String) As Integer
        combo.Items.Clear()
        strSql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST "
        strSql += " WHERE ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & Itemname & "' AND ISNULL(STUDDED,'') = 'S' AND ACTIVE = 'Y') ORDER BY SUBITEMNAME"
        objGPack.FillCombo(strSql, combo, , False)
    End Function

    Private Sub cmbItem_OWN_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbItem_OWN.SelectedIndexChanged
        SearchSubItem(cmbSubItem_OWN, cmbItem_OWN.Text)
        cmbSubItem_OWN.Items.Add("ALL")
    End Sub

    Private Sub cmb_itemName_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmb_itemName.SelectedIndexChanged
        SearchSubItem(cmbSubitemName, cmb_itemName.Text)
    End Sub

    Private Sub cmb_StuddedItemName_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmb_StuddedItemName.SelectedIndexChanged
        SearchSubItem(cmbStuddedSubitem, cmb_StuddedItemName.Text)
    End Sub
End Class