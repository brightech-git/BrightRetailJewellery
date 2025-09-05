Imports System.Data.OleDb
Public Class frmPieceWiseStockReOrder
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim flagSave As Boolean = False
    Dim flagUpdate As Boolean = False
    Dim Sno As String = Nothing
    Dim blnValid As Boolean = False
    Dim blnIsCheckValid As Boolean = False
    Dim Editmode As Boolean = False
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        funcGridStyle(gridView)
        funcLoadDesignerName(cmbDesignerName)
        funcLoadMetalName()
        funcLoadCostCentre()
        funcNew()
        SearchCostCentre(cmbCostSearch_OWN)
        SearchItem(cmbItemSearch_OWN)
        'cmbSubItemSearch_OWN.Items.Add("ALL")
    End Sub
    Public Sub New(ByVal edit As Boolean, ByVal ssno As String)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        funcGridStyle(gridView)
        funcLoadDesignerName(cmbDesignerName)
        funcLoadMetalName()
        funcLoadCostCentre()
        funcNew()
        SearchCostCentre(cmbCostSearch_OWN)
        SearchItem(cmbItemSearch_OWN)
        Editmode = edit
        flagSave = edit
        Sno = ssno
    End Sub
    Function funcCallGrid() As Integer
        strSql = " SELECT SNO,"
        strSql += " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST AS I WHERE I.ITEMID=SR.ITEMID)AS ITEMNAME"
        strSql += " ,ISNULL((SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST AS S WHERE S.SUBITEMID=SR.SUBITEMID),'')AS SUBITEMNAME"
        strSql += " ,ISNULL((SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE AS C WHERE C.COSTID = SR.COSTID),'')AS COSTNAME"
        strSql += " ,MINPIECE"
        strSql += " ,PIECE"
        strSql += " ,(SELECT SIZENAME FROM " & cnAdminDb & "..ITEMSIZE WHERE SIZEID = SR.SIZEID)AS SIZENAME"
        strSql += " ,(Select DesignerName from " & cnAdminDb & "..DESIGNER Where DesignerId=SR.DesignId) as DESIGNERNAME"
        strSql += " FROM " & cnAdminDb & "..STKREORDER AS SR WHERE FROMDAY IS NULL"
        funcOpenGrid(strSql, gridView)
        gridView.Columns("SNO").Visible = False
        gridView.Columns("ITEMNAME").Width = 200
        gridView.Columns("SUBITEMNAME").Width = 150
        gridView.Columns("COSTNAME").Width = 150
        gridView.Columns("DESIGNERNAME").HeaderText = "DESIGNER"
        gridView.Columns("DESIGNERNAME").Width = 150
        gridView.Columns("PIECE").HeaderText = "MAX PIECE"
        gridView.Columns("PIECE").Width = 70
        gridView.Columns("PIECE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridView.Columns("MINPIECE").HeaderText = "MIN PIECE"
        gridView.Columns("MINPIECE").Width = 70
        gridView.Columns("MINPIECE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridView.Columns("SIZENAME").HeaderText = "SIZE"
        gridView.Columns("SIZENAME").Width = 100
    End Function
    Function funcNew() As Integer
        objGPack.TextClear(Me)
        flagSave = False
        flagUpdate = False
        funcCallGrid()
        cmbMetal_Man.Focus()
    End Function
    Function funcLoadDesignerName(ByVal combo As ComboBox) As Integer
        strSql = " Select DesignerName from " & cnAdminDb & "..Designer order by DesignerName"
        objGPack.FillCombo(strSql, combo, , False)
    End Function
    Function funcExit() As Integer
        Me.Close()
    End Function
    Function funcSave() As Integer
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Function
        If objGPack.Validator_Check(Me) Then Exit Function
        If Val(txtMaxPiece.Text) = 0 Then
            MsgBox(Me.GetNextControl(txtMaxPiece, False).Text + E0001, MsgBoxStyle.Information)
            txtMaxPiece.Focus()
            Exit Function
        End If
        If Val(txtMinPcs.Text) = 0 Then
            MsgBox(Me.GetNextControl(txtMinPcs, False).Text + E0001, MsgBoxStyle.Information)
            txtMinPcs.Focus()
            Exit Function

        End If
        If flagSave = False Then
            funcAdd()
        Else
            funcUpdate()
        End If
    End Function
    Function funcAdd() As Integer
        Dim ItemId As Integer = Nothing
        Dim DesignId As Integer = Nothing
        Dim SubItemId As Integer = Nothing
        Dim COSTID As String = Nothing
        Dim ds As New Data.DataSet
        ds.Clear()
        ''Find ItemId
        strSql = " Select ItemId from " & cnAdminDb & "..ItemMast where ItemName = '" & cmbItem_Man.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(ds, "ItemId")
        If ds.Tables("ItemId").Rows.Count > 0 Then
            ItemId = Val(ds.Tables("Itemid").Rows(0).Item("ItemId").ToString)
        Else
            ItemId = 0
        End If
        ''Find DesignerID
        strSql = " Select DESIGNERID from " & cnAdminDb & "..DESIGNER where DESIGNERNAME = '" & cmbDesignerName.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(ds, "Designid")
        If ds.Tables("Designid").Rows.Count > 0 Then
            DesignId = Val(ds.Tables("Designid").Rows(0).Item("DESIGNERID").ToString)
        Else
            DesignId = 0
        End If
       
        ''Find SubItemId
        strSql = " Select SubItemId from " & cnAdminDb & "..SubItemMast where SubItemName = '" & cmbSubItem_Man.Text & "' AND ITEMID = " & ItemId & ""
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(ds, "SubItemId")
        If ds.Tables("SubItemId").Rows.Count > 0 Then
            SubItemId = Val(ds.Tables("SubItemId").Rows(0).Item("SubItemid").ToString)
        Else
            SubItemId = 0
        End If

        ''Find COSTID
        If cmbCostCentre.Text <> "" Then
            strSql = " Select CostId from " & cnAdminDb & "..CostCentre where CostName = '" & cmbCostCentre.Text & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(ds, "CostId")
            If ds.Tables("CostId").Rows.Count > 0 Then
                COSTID = ds.Tables("CostId").Rows(0).Item("CostId")
            Else
                COSTID = ""
            End If
        Else
            COSTID = ""
        End If

        Sno = GetNewSno(TranSnoType.STKREORDERCODE, tran, "GET_SNO_ADMIN")

        strSql = " Insert into " & cnAdminDb & "..StkReorder"
        strSql += " ("
        strSql += " SNO,ItemId,SubItemId,COSTID,"
        strSql += " piece,Userid,"
        strSql += "MINPIECE,DESIGNID,"
        strSql += " Updated,Uptime,sizeid)Values ("
        strSql += " '" & Sno & "'"
        strSql += " ," & ItemId & "" 'ItemId
        strSql += " ," & SubItemId & "" 'SubItemId
        strSql += " ,'" & COSTID & "'" 'COSTID
        strSql += " ," & Val(txtMaxPiece.Text) & "" 'piece
        strSql += " ," & userId & "" 'Userid
        strSql += "," & Val("" & txtMinPcs.Text) & "," & DesignId
        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'Updated
        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'Uptime
        strSql += " ," & Val(objGPack.GetSqlValue("select sizeid from " & cnAdminDb & "..itemsize where sizename = '" & cmbSize.Text & "'")) & "" 'sizeid
        strSql += " )"
        Try
            ExecQuery(SyncMode.Master, strSql, cn)
            funcCallGrid()
            txtMaxPiece.Clear()
            txtMinPcs.Clear()
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function
    Function funcUpdate() As Integer
        Dim ItemId As Integer = Nothing
        Dim Designid As Integer = Nothing
        Dim SubItemId As Integer = Nothing
        Dim COSTID As String = Nothing
        Dim ds As New Data.DataSet
        ds.Clear()
        ''Find ItemId
        strSql = " Select ItemId from " & cnAdminDb & "..ItemMast where ItemName = '" & cmbItem_Man.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(ds, "ItemId")
        If ds.Tables("ItemId").Rows.Count > 0 Then
            ItemId = Val(ds.Tables("Itemid").Rows(0).Item("ItemId").ToString)
        Else
            ItemId = 0
        End If

        ''Find SubItemId
        strSql = " Select SubItemId from " & cnAdminDb & "..SubItemMast where SubItemName = '" & cmbSubItem_Man.Text & "' AND ITEMID = " & ItemId & ""
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(ds, "SubItemId")
        If ds.Tables("SubItemId").Rows.Count > 0 Then
            SubItemId = Val(ds.Tables("SubItemId").Rows(0).Item("SubItemid").ToString)
        Else
            SubItemId = 0
        End If
        strSql = " Select DESIGNERID from " & cnAdminDb & "..DESIGNER where DESIGNERNAME = '" & cmbDesignerName.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(ds, "DESIGNERID")
        If ds.Tables("DESIGNERID").Rows.Count > 0 Then
            Designid = Val(ds.Tables("DESIGNERID").Rows(0).Item("DESIGNERID").ToString)
        Else
            Designid = 0
        End If

        ''Find COSTID
        If cmbCostCentre.Text <> "" Then
            strSql = " Select CostId from " & cnAdminDb & "..CostCentre where CostName = '" & cmbCostCentre.Text & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(ds, "CostId")
            If ds.Tables("CostId").Rows.Count > 0 Then
                COSTID = ds.Tables("CostId").Rows(0).Item("CostId")
            Else
                COSTID = ""
            End If
        Else
            COSTID = ""
        End If

        strSql = " Update " & cnAdminDb & "..StkReorder Set"
        strSql += " ItemId= " & ItemId & ""
        strSql += " ,SubItemId= " & SubItemId & ""
        strSql += " ,COSTID='" & COSTID & "'"
        strSql += " ,piece=" & Val(txtMaxPiece.Text) & ""
        strSql += " ,MINPIECE=" & Val(txtMinPcs.Text) & ",DESIGNID=" & Designid
        strSql += " ,Userid=" & userId & ""
        strSql += " ,Updated='" & Today.Date.ToString("yyyy-MM-dd") & "'"
        strSql += " ,Uptime='" & Date.Now.ToLongTimeString & "'"
        strSql += " ,sizeid = " & Val(objGPack.GetSqlValue("select sizeid from " & cnAdminDb & "..itemsize where sizename = '" & cmbSize.Text & "'")) & "" 'sizeid
        strSql += " where Sno = '" & Sno & "'"
        Try
            ExecQuery(SyncMode.Master, strSql, cn)
            funcNew()
            If Editmode = True Then
                Me.Close()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function
    Function funcGetDetails(ByVal temp As String) As String
            strSql = " Select"
        strSql += " (Select MetalName from " & cnAdminDb & "..MetalMast as M, " & cnAdminDb & "..ItemMast Im where M.MetalId=Im.MetalId and Im.ItemId=SR.ItemId)as MetalName,"
        strSql += " (select ItemName from " & cnAdminDb & "..ItemMast as i where i.ItemId=SR.Itemid)as ItemName,"
        strSql += " isnull((select SubItemName from " & cnAdminDb & "..SubItemMast as s where s.SubItemId=SR.SubItemId),'')as SubItemName,"
        strSql += " isnull((select CostName from " & cnAdminDb & "..CostCentre as c where c.Costid = SR.COSTID),'')as CostName,"
        strSql += " isnull((select SIZENAME from " & cnAdminDb & "..ITEMSIZE as c where c.SIZEID = SR.SIZEID),'')as SIZENAME,"
        strSql += " isnull((select DESIGNERNAME from " & cnAdminDb & "..DESIGNER as DS where DS.DESIGNERID= SR.DESIGNID),'')as DESIGNERNAME,"
        strSql += " Piece"
        strSql += ",minpiece,Designid"
        strSql += " from " & cnAdminDb & "..StkReorder as SR"
        strSql += " Where Sno = '" & temp & "'"
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
            cmbMetal_Man.Text = .Item("MetalName").ToString
            strSql = " Select ItemName from " & cnAdminDb & "..ItemMast"
            strSql += " where METALID = (select Metalid from " & cnAdminDb & "..MetalMast where MetalName = '" & cmbMetal_Man.Text & "')"
            strSql += " Order by ItemName"
            objGPack.FillCombo(strSql, cmbItem_Man)

            cmbItem_Man.Text = .Item("ItemName").ToString
            cmbSubItem_Man.Text = .Item("SubItemName").ToString
            cmbCostCentre.Text = .Item("CostName").ToString
            cmbSize.Text = .Item("SIZENAME").ToString
            txtMaxPiece.Text = .Item("piece").ToString
            txtMinPcs.Text = .Item("Minpiece").ToString
            Me.cmbDesignerName.Text = .Item("Designername").ToString
        End With
        Sno = temp
    End Function
    Function funcLoadMetalName() As Integer
        strSql = " Select Metalname from " & cnAdminDb & "..MetalMast Order by DISPLAYORDER"
        objGPack.FillCombo(strSql, cmbMetal_Man)
    End Function
    Function funcLoadCostCentre() As Integer
        If GetAdmindbSoftValue("COSTCENTRE", "N") = "Y" Then
            strSql = " select CostName from " & cnAdminDb & "..CostCentre order by CostName"
            objGPack.FillCombo(strSql, cmbCostCentre)
            cmbCostCentre.Enabled = True
        Else
            cmbCostCentre.Enabled = False
        End If
    End Function
    Function funcUniqueValidation() As Boolean
        strSql += " select 1 from " & cnAdminDb & "..StkReorder"
        strSql += " where "
        strSql += " ItemId = (select ItemId from " & cnAdminDb & "..ItemMast where ItemName = '" & cmbItem_Man.Text & "')"
        strSql += " and SubItemId = isnull((select SubItemId from " & cnAdminDb & "..SubItemMast where SubItemName = '" & cmbSubItem_Man.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_Man.Text & "')),0)"
        If cmbCostCentre.Text <> "" Then strSql += " and COSTID = isnull((select CostId from " & cnAdminDb & "..CostCentre where CostName = '" & cmbCostCentre.Text & "'),'')"
        If cmbSize.Text <> "" Then strSql += " and ISNULL(sizeid,0) = ISNULL((select sizeid FROM " & cnAdminDb & "..ITEMSIZE WHERE SIZENAME = '" & cmbSize.Text & "'),0)"
        If blnValid = False Then
            If flagSave = True Then
                strSql += " and sno <> '" & Sno & "'"
            End If
        End If
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            Return True
            'MsgBox("Already Exist...")
        End If
        Return False
    End Function

    Private Sub frmPieceWiseStockReOrder_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmPieceWiseStockReOrder_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        
    End Sub

    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        funcSave()
    End Sub

    Private Sub NEwToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NEwToolStripMenuItem.Click
        funcNew()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        funcExit()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Val(txtMaxPiece.Text) < Val(txtMinPcs.Text) Then MsgBox("Invalid data") : txtMaxPiece.Focus()
        funcSave()
    End Sub

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        funcCallGrid()
        gridView.Focus()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        funcNew()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        funcExit()
    End Sub

    Private Sub cmbMetal_Man_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbMetal_Man.SelectedIndexChanged
        If flagSave = True Then
            Exit Sub
        End If
        strSql = " Select ItemName from " & cnAdminDb & "..ItemMast"
        strSql += " where METALID = (select Metalid from " & cnAdminDb & "..MetalMast where MetalName = '" & cmbMetal_Man.Text & "')"
        strSql += " Order by ItemName"
        objGPack.FillCombo(strSql, cmbItem_Man)
    End Sub

    Private Sub cmbItem_Man_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbItem_Man.SelectedIndexChanged
        cmbSubItem_Man.Text = ""
        strSql = " Select SubItemName from " & cnAdminDb & "..SubItemMast where "
        strSql += " ItemId = (select ItemId from " & cnAdminDb & "..ItemMast where ItemName = '" & cmbItem_Man.Text & "' and SubItem = 'Y')"
        strSql += " Order by SubItemName"
        If flagSave = True Then
            objGPack.FillCombo(strSql, cmbSubItem_Man, , False)
        Else
            objGPack.FillCombo(strSql, cmbSubItem_Man)
        End If
        If cmbSubItem_Man.Items.Count > 0 Then cmbSubItem_Man.Enabled = True Else cmbSubItem_Man.Enabled = False

        cmbSize.Text = ""
        strSql = " SELECT SIZESTOCK FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_Man.Text & "'"
        If objGPack.GetSqlValue(strSql, , "N") = "Y" Then
            strSql = " SELECT SIZENAME FROM " & cnAdminDb & "..ITEMSIZE ORDER BY SIZENAME"
            If flagSave = True Then
                objGPack.FillCombo(strSql, cmbSize, , False)
            Else
                objGPack.FillCombo(strSql, cmbSize)
            End If
        End If
        If cmbSize.Items.Count > 0 Then cmbSize.Enabled = True Else cmbSize.Enabled = False
    End Sub
    Private Sub gridView_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            If gridView.RowCount > 0 Then
                funcGetDetails(gridView.Item(0, gridView.CurrentRow.Index).Value)
                cmbMetal_Man.Focus()
            End If
        ElseIf e.KeyCode = Keys.Escape Then
            cmbMetal_Man.Focus()
        End If
        If e.KeyCode = Keys.Delete Then
            If gridView.Rows.Count > 0 Then
                Dim result = MessageBox.Show("Do you want to Delete...", "Message", MessageBoxButtons.YesNo)
                If result = Windows.Forms.DialogResult.Yes Then
                    strSql = " Delete from " & cnAdminDb & "..STKREORDER where Sno='" & gridView("SNo", gridView.CurrentRow.Index).Value & "'"
                    cmd = New OleDbCommand(strSql, cn)
                    If Not tran Is Nothing Then cmd.Transaction = tran
                    cmd.ExecuteNonQuery()
                    funcCallGrid()
                    txtMaxPiece.Text = ""
                    txtMinPcs.Text = ""
                    Call btnSearch_Click(sender, e)

                ElseIf result = Windows.Forms.DialogResult.No Then
                    Exit Sub
                End If
            End If
        End If
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If Not gridView.RowCount > 0 Then
            Exit Sub
        End If
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
        Dim chkQry As String = "SELECT 1 FROM " & cnAdminDb & "..STKREORDER WHERE 1<>1"
        Dim delKey As String = gridView.Rows(gridView.CurrentRow.Index).Cells("SNO").Value.ToString
        strSql = " SELECT * FROM " & cnAdminDb & "..STKREORDER WHERE SNO = '" & delKey & "'"
        Dim dtReorder As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtReorder)
        strSql = vbCrLf + " SELECT COUNT(*) FROM " & cnAdminDb & "..ITEMTAG WHERE ISSDATE IS NULL AND ITEMID=" & Val(dtReorder.Rows(0).Item("ITEMID").ToString) & " "
        strSql += vbCrLf + " AND SUBITEMID= " & Val(dtReorder.Rows(0).Item("SUBITEMID").ToString) & " AND COSTID='" & dtReorder.Rows(0).Item("COSTID").ToString & "' "
        ' strSql += vbCrLf + " HAVING COUNT(*) BETWEEN " & Val(dtReorder.Rows(0).Item("MINPIECE").ToString) & " AND " & Val(dtReorder.Rows(0).Item("PIECE").ToString) & " "
        'strSql += vbCrLf + " AND PCS BETWEEN " & Val(dtReorder.Rows(0).Item("MINPIECE").ToString) & " AND " & Val(dtReorder.Rows(0).Item("PIECE").ToString) & " "
        If GetSqlValue(cn, strSql) > 0 Then
            MsgBox("This Item cannot delete stock will be available ", MsgBoxStyle.Information)
            Exit Sub
        End If
        strSql = vbCrLf + " SELECT COUNT(*) FROM " & cnAdminDb & "..ORMAST WHERE ISNULL(ODBATCHNO,'')='' AND ISNULL(ODSNO,'')='' "
        strSql += vbCrLf + " AND ITEMID=" & Val(dtReorder.Rows(0).Item("ITEMID").ToString) & "  AND SUBITEMID= " & Val(dtReorder.Rows(0).Item("SUBITEMID").ToString) & " "
        strSql += vbCrLf + " AND COSTID='" & dtReorder.Rows(0).Item("COSTID").ToString & "'"
        ' strSql += vbCrLf + " HAVING COUNT(*) BETWEEN " & Val(dtReorder.Rows(0).Item("MINPIECE").ToString) & " AND " & Val(dtReorder.Rows(0).Item("PIECE").ToString) & " "

        If GetSqlValue(cn, strSql) > 0 Then
            MsgBox("This Item cannot delete Order will be available", MsgBoxStyle.Information)
            Exit Sub
        End If

        DeleteItem(SyncMode.Master, chkQry, "DELETE FROM " & cnAdminDb & "..STKREORDER WHERE SNO = '" & delKey & "'")
        funcCallGrid()
    End Sub
    Private Sub gridView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.GotFocus
        lblStatus.Visible = True
    End Sub
    Private Sub gridView_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.LostFocus
        lblStatus.Visible = False
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim calWt As Boolean = False
        Dim calRate As Boolean = False
        Dim rangeMode As Boolean = False
        strSql = " Select RangeMode from " & cnAdminDb & "..STKREORDER Where ItemId=(Select ItemId from " & cnAdminDb & "..ItemMast Where ItemName='" & cmbItemSearch_OWN.Text & "')"
        Dim dta As New DataTable
        dta.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dta)
        If dta.Rows.Count > 0 Then
            If dta.Rows(0).Item("RangeMode").ToString = "R" Then
                calRate = True
            ElseIf dta.Rows(0).Item("RangeMode").ToString = "W" Then
                calWt = True
            Else
                Dim sSql As String = "Select CalType from " & cnAdminDb & "..ItemMast where CalType in ('R','F','M','B')"
                sSql += " and ItemId = (select ItemId from " & cnAdminDb & "..ItemMast where ItemName = '" & cmbItemSearch_OWN.Text & "')"
                Dim dts As New DataTable
                dts.Clear()
                da = New OleDbDataAdapter(sSql, cn)
                da.Fill(dts)
                If dts.Rows.Count > 0 Then
                    rangeMode = True
                End If

                Dim sSql1 As String = "Select CalType from " & cnAdminDb & "..ItemMast where CalType in ('W')"
                sSql1 += " and ItemId = (select ItemId from " & cnAdminDb & "..ItemMast where ItemName = '" & cmbItemSearch_OWN.Text & "')"
                Dim dts1 As New DataTable
                dts1.Clear()
                da = New OleDbDataAdapter(sSql1, cn)
                da.Fill(dts1)
                If dts1.Rows.Count > 0 Then
                    rangeMode = False
                End If
            End If
        End If

        strSql = " SELECT SNO,"
        strSql += " ITEMNAME, "
        strSql += " SUBITEMNAME,"
        strSql += " ISNULL((SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE AS C WHERE C.COSTID = SR.COSTID),'')AS COSTNAME,"
        If calWt = True Then
            strSql += " FROMWEIGHT AS FROMWEIGHT,"
            strSql += " TOWEIGHT as TOWEIGHT,"
            strSql += " PIECE,"
        ElseIf calRate = True Then
            strSql += " FROMWEIGHT AS FROMRATE,"
            strSql += " TOWEIGHT as TORATE,"
            strSql += " PIECE,"
        ElseIf rangeMode = True Then
            'lblWtFrom.Text = "From Rate"
            'lblWtTo.Text = "To"
            'lblWt.Text = "Rate"
            'cmbRangeMode.Text = "R"
            strSql += " FROMWEIGHT AS FROMRATE,"
            strSql += " TOWEIGHT as TORATE,"
            strSql += " PIECE,"
        ElseIf rangeMode = False Then
            strSql += " FROMWEIGHT AS FROMWEIGHT,"
            strSql += " TOWEIGHT as TOWEIGHT,"
            strSql += " PIECE,"
        End If
        If calWt = True Then
            strSql += " WEIGHT as [AVG WEIGHT],"
        ElseIf calRate = True Then
            strSql += " WEIGHT as [AVG RATE],"
        ElseIf rangeMode = True Then
            strSql += " WEIGHT as [AVG RATE],"
        ElseIf rangeMode = False Then
            strSql += " WEIGHT as [AVG WEIGHT],"
        End If
        strSql += " (SELECT SIZENAME FROM " & cnAdminDb & "..ITEMSIZE WHERE SIZEID = SR.SIZEID)AS SIZENAME"
        strSql += " FROM " & cnAdminDb & "..STKREORDER AS SR"
        strSql += " LEFT OUTER JOIN " & cnAdminDb & "..ITEMMAST I on I.ITEMID=SR.ITEMID"
        strSql += " LEFT OUTER JOIN " & cnAdminDb & "..SUBITEMMAST S on S.SUBITEMID=SR.SUBITEMID"
        strSql += " LEFT OUTER JOIN " & cnAdminDb & "..COSTCENTRE C on C.COSTID=SR.COSTID"
        If UCase(cmbSubItemSearch_OWN.Text) = "ALL" Then
            If cmbCostSearch_OWN.Text <> "" Or cmbItemSearch_OWN.Text <> "" Or cmbSubItemSearch_OWN.Text <> "" Then
                strSql += " where C.CostName='" & cmbCostSearch_OWN.Text & "'"
                strSql += " and I.ItemName='" & cmbItemSearch_OWN.Text & "' AND FROMDAY IS NULL"
                'strSql += " and S.SubItemName='" & cmbSubItemSearch.Text & "'"
                funcOpenGrid(strSql, gridView)
                With gridView
                    .Columns("SNO").Width = 120
                    .Columns("ITEMNAME").Width = 200
                    .Columns("SUBITEMNAME").Width = 200
                    .Columns("COSTNAME").Width = 120
                End With
                gridView.Select()
                gridView.Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                gridView.Columns(5).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                gridView.Columns(7).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End If
            'ElseIf cmbCostSearch.Text <> "" Or (cmbItemSearch.Text <> "" And cmbSubItemSearch.Text <> "") Then
        Else
            If cmbCostSearch_OWN.Text <> "" Or cmbItemSearch_OWN.Text <> "" Or cmbSubItemSearch_OWN.Text <> "" Then
                strSql += " where C.CostName='" & cmbCostSearch_OWN.Text & "'"
                strSql += " and I.ItemName='" & cmbItemSearch_OWN.Text & "'"
                strSql += " and S.SubItemName='" & cmbSubItemSearch_OWN.Text & "' AND FROMDAY IS NULL"
                funcOpenGrid(strSql, gridView)
                With gridView
                    .Columns("SNO").Width = 120
                    .Columns("ITEMNAME").Width = 200
                    .Columns("SUBITEMNAME").Width = 200
                    .Columns("COSTNAME").Width = 120
                End With
                gridView.Select()
                gridView.Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                gridView.Columns(5).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                gridView.Columns(7).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End If
        End If
    End Sub
    Private Function SearchCostCentre(ByVal combo As ComboBox) As Integer
        strSql = " Select CostName from " & cnAdminDb & "..CostCentre order by CostName"
        objGPack.FillCombo(strSql, combo, , False)
    End Function
    Private Function SearchItem(ByVal combo As ComboBox) As Integer
        cmbSubItemSearch_OWN.Items.Add("ALL")
        strSql = " Select ItemName from " & cnAdminDb & "..ItemMast order by ItemName"
        da = New OleDbDataAdapter(strSql, cn)
        Dim dt As New DataTable()
        da.Fill(dt)
        cmbItemSearch_OWN.DataSource = dt
        cmbItemSearch_OWN.DisplayMember = "ItemName"
        cmbItemSearch_OWN.ValueMember = "ItemName"
        'objGPack.FillCombo(strSql, combo, , False)
        'cmbSubItemSearch_OWN.Items.Add("ALL")
    End Function
    Private Function SearchSubItem(ByVal combo As ComboBox) As Integer
        'cmbSubItemSearch_OWN.Items.Add("ALL")
        cmbSubItemSearch_OWN.Items.Clear()
        strSql = vbCrLf + " SELECT 'ALL' SUBITEMNAME,'ALL'SUBITEMID,1 RESULT,0 DISPLAYORDER"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT SUBITEMNAME,CONVERT(vARCHAR,SUBITEMID),2 RESULT,DISPLAYORDER FROM " & cnAdminDb & "..SUBITEMMAST"
        strSql += vbCrLf + " WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemSearch_OWN.Text & "') order by SubItemName"

        'strSql = vbCrLf + " SELECT 'ALL' SUBITEMNAME"
        'strSql += vbCrLf + " UNION ALL"
        'strSql += " Select SubItemName from " & cnAdminDb & "..SubItemMast where ItemId= (Select ItemId from " & cnAdminDb & "..ItemMast where ItemName='" & cmbItemSearch_OWN.Text & "') order by SubItemName"
        objGPack.FillCombo(strSql, combo, , False)
        'cmbSubItemSearch_OWN.Items.Add("ALL")
    End Function

    Private Sub cmbItemSearch_OWN_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbItemSearch_OWN.SelectedIndexChanged
        SearchSubItem(cmbSubItemSearch_OWN)
    End Sub

    Private Sub frmPieceWiseStockReOrder_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = (Keys.F9) Then
            cmbCostSearch_OWN.Focus()
            'cmbSubItemSearch_OWN.Items.Add("ALL")
        End If
    End Sub

    Private Sub txtMaxPiece_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtMaxPiece.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If Val(txtMaxPiece.Text) < Val(txtMinPcs.Text) Then MsgBox("Invalid data") : txtMaxPiece.Focus()
        End If
    End Sub

 
End Class