Imports System.Data.OleDb
Public Class frmPMValidation
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim flagSave As Boolean = False
    Dim flagUpdate As Boolean = False
    Dim Sno As String = Nothing
    Dim COMPL_ISS_MODE As String = GetAdmindbSoftValue("COMPL_ISS_MODE", "N")
    Dim blnValid As Boolean = False
    Dim blnIsCheckValid As Boolean = False

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        funcGridStyle(gridView)
        'cmbSubItemSearch_OWN.Items.Add("ALL")
    End Sub
    Function funcCallGrid() As Integer
        strSql = " SELECT SNO,(CASE WHEN SR.ITEMID=0 THEN 'AMOUNT WISE' ELSE I.ITEMNAME END)ITEMNAME,"
        strSql += " (SELECT PMNAME FROM " & cnAdminDb & "..PMMAST AS I WHERE I.PMID=SR.PMID)AS PMNAME,"
        strSql += " ISNULL((SELECT PMSUBNAME FROM " & cnAdminDb & "..PMSUBMAST AS S WHERE S.PMSUBID=SR.PMSUBID),'')AS PMSUBNAME,"
        strSql += " ISNULL((SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE AS C WHERE C.COSTID = SR.COSTID),'')AS COSTNAME,"
        strSql += " FROM_WT,TO_WT,FROM_PCS,TO_PCS,FROM_AMT,TO_AMT"
        '        strSql += " ,(SELECT SIZENAME FROM " & cnAdminDb & "..ITEMSIZE WHERE SIZEID = SR.SIZEID)AS SIZENAME"
        strSql += " FROM " & cnAdminDb & "..PMVALIDATE AS SR "
        strSql += " LEFT JOIN " & cnAdminDb & "..ITEMMAST AS I ON I.ITEMID = SR.ITEMID "
        funcOpenGrid(strSql, gridView)
        gridView.Columns("SNO").Visible = False
        gridView.Columns("PMNAME").Width = 200
        gridView.Columns("PMSUBNAME").Width = 150
        gridView.Columns("COSTNAME").Width = 150
        gridView.Columns("FROM_WT").HeaderText = "FROMWEIGHT"
        gridView.Columns("FROM_WT").Width = 90
        gridView.Columns("FROM_WT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

        gridView.Columns("TO_WT").HeaderText = "TOWEIGHT"
        gridView.Columns("TO_WT").Width = 90
        gridView.Columns("TO_WT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

        gridView.Columns("FROM_PCS").HeaderText = "TO PCS"
        gridView.Columns("FROM_PCS").Width = 70
        gridView.Columns("FROM_PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

        gridView.Columns("TO_PCS").HeaderText = "FROM PCS"
        gridView.Columns("TO_PCS").Width = 70
        gridView.Columns("TO_PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

        gridView.Columns("FROM_AMT").HeaderText = "FROM AMT"
        gridView.Columns("FROM_AMT").Width = 70
        gridView.Columns("FROM_AMT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        
        gridView.Columns("TO_AMT").HeaderText = "TO AMT"
        gridView.Columns("TO_AMT").Width = 70
        gridView.Columns("TO_AMT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight


    End Function

    Function funcNew() As Integer
        objGPack.TextClear(Me)
        flagSave = False
        flagUpdate = False
        strSql = " Select Pmname from " & cnAdminDb & "..PMMAST Order by PMNAME"
        objGPack.FillCombo(strSql, cmbComItem_Man)
        If COMPL_ISS_MODE = "A" Then
            txtWtFrom.Enabled = False
            txtWtTo.Enabled = False
            txtPieceFrom.Enabled = False
            txtPieceTo.Enabled = False
            cmbItem_OWN.Enabled = False
        End If
        funcLoadMetalName()
        funcLoadCostCentre()
        funcCallGrid()
        cmbMetal_Man.Focus()

    End Function

    Function funcExit() As Integer
        Me.Close()
    End Function
    Function funcSave() As Integer
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Function
        If objGPack.Validator_Check(Me) Then Exit Function
        If cmbComItem_Man.Text = "" Then MsgBox("Complement Item is empty", MsgBoxStyle.Information) : Exit Function
        If COMPL_ISS_MODE = "A" Then
            If Val(txtFromAmt.Text) = 0 Then MsgBox("From Amount not Valid...", MsgBoxStyle.Information) : Exit Function
            If Val(txtToAmt.Text) = 0 Then MsgBox("To Amount not Valid...", MsgBoxStyle.Information) : Exit Function
        End If
        If flagSave = False Then
            funcAdd()
        Else
            funcUpdate()
        End If
    End Function
    Function funcAdd() As Integer
        Dim ItemId As Integer = Nothing
        Dim ComItemId As Integer = Nothing

        Dim SubItemId As Integer = Nothing
        Dim COSTID As String = Nothing
        Dim ds As New Data.DataSet
        ds.Clear()
        ''Find ItemId
        strSql = " Select Itemid from " & cnAdminDb & "..ItemMast where ItemName = '" & cmbItem_OWN.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(ds, "Itemid")
        If ds.Tables("Itemid").Rows.Count > 0 Then
            ItemId = Val(ds.Tables("Itemid").Rows(0).Item("ItemId").ToString)
        Else
            ItemId = 0
        End If

        strSql = " Select pmid from " & cnAdminDb & "..pmMast where pmName = '" & cmbComItem_Man.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(ds, "pmid")
        If ds.Tables("pmid").Rows.Count > 0 Then
            ComItemId = Val(ds.Tables("pmid").Rows(0).Item("pmId").ToString)
        Else
            comItemId = 0
        End If

        ''Find SubItemId
        If cmbSubItem_Man.Text <> "ALL" Then
            strSql = " Select Pmsubid from " & cnAdminDb & "..PmSubMast where PmSubName = '" & cmbSubItem_Man.Text & "' AND  Pmid = " & ComItemId
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(ds, "PmSubid")
            If ds.Tables("PmSubId").Rows.Count > 0 Then
                SubItemId = Val(ds.Tables("PmSubId").Rows(0).Item("PmSubid").ToString)
            Else
                SubItemId = 0
            End If
        Else
            SubItemId = 0
        End If

        ''Find COSTID
        '598
        If cmbCostCentre_own.Text <> "" Then
            strSql = " Select CostId from " & cnAdminDb & "..CostCentre where CostName = '" & cmbCostCentre_own.Text & "'"
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

        'If COSTID = "" Then
        'strSql = "  SELECT RIGHT(MAX(SNO),LEN(MAX(SNO))-5)+1 as SNO FROM " & cnAdminDb & "..PMVALIDATE "
        strSql = "  SELECT MAX(CONVERT(NUMERIC,SNO)) as SNO FROM " & cnAdminDb & "..PMVALIDATE "
        Dim dt As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        Sno = Val(dt.Rows(0).Item("SNO").ToString)
        If Sno = 0 Then Sno += 1
        'Sno = GetCostId(COSTID) + GetCompanyId(strCompanyId) + Sno.ToString
        Sno = Sno.ToString
        'Else
        'Sno = GetNewSno(TranSnoType.STKREORDERCODE, tran, "GET_SNO_ADMIN")
        'End If

        If COMPL_ISS_MODE = "A" Then
            strSql = " Insert into " & cnAdminDb & "..PMVALIDATE"
            strSql += " ("
            strSql += " SNO,Itemid,PmId,PmSubId,COSTID,"
            strSql += " from_wt,to_wt,FROM_PCS,to_pcs,from_amt,to_amt,Userid,"
            strSql += " Updated,Uptime)Values ("
            strSql += " '" & Sno & "'"
            strSql += " ,0" 'ItemId
            strSql += " ," & ComItemId & "" 'ItemId
            strSql += " ," & SubItemId & "" 'SubItemId
            strSql += " ,'" & COSTID & "'" 'COSTID
            strSql += " ,'0.001'" 'fromweight
            strSql += " ,'999.999'" 'toweight
            strSql += " ,'0'" 'piece
            strSql += " ,'100'" 'weight
            strSql += " ," & Val(txtFromAmt.Text) & "" 'piece
            strSql += " ," & Val(txtToAmt.Text) & "" 'weight
            strSql += " ," & userId & "" 'Userid
            strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'Updated
            strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'Uptime
            strSql += " )"
        Else
            strSql = " Insert into " & cnAdminDb & "..PMVALIDATE"
            strSql += " ("
            strSql += " SNO,Itemid,PmId,PmSubId,COSTID,"
            strSql += " from_wt,to_wt,FROM_PCS,to_pcs,from_amt,to_amt,Userid,"
            strSql += " Updated,Uptime)Values ("
            strSql += " '" & Sno & "'"
            strSql += " ," & ItemId & "" 'ItemId
            strSql += " ," & ComItemId & "" 'ItemId
            strSql += " ," & SubItemId & "" 'SubItemId
            strSql += " ,'" & COSTID & "'" 'COSTID
            strSql += " ," & Val(txtWtFrom.Text) & "" 'fromweight
            strSql += " ," & Val(txtWtTo.Text) & "" 'toweight
            strSql += " ," & Val(txtPieceFrom.Text) & "" 'piece
            strSql += " ," & Val(txtPieceTo.Text) & "" 'weight
            strSql += " ," & Val(txtFromAmt.Text) & "" 'piece
            strSql += " ," & Val(txtToAmt.Text) & "" 'weight
            strSql += " ," & userId & "" 'Userid
            strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'Updated
            strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'Uptime
            strSql += " )"
        End If
        Try
            ExecQuery(SyncMode.Master, strSql, cn, tran)
            funcCallGrid()
            txtWtFrom.Clear()
            txtWtTo.Clear()
            txtPieceFrom.Clear()
            txtPieceTo.Clear()
            txtFromAmt.Clear()
            txtToAmt.Clear()
            txtWtFrom.Select()
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function
    Function funcUpdate() As Integer
        Dim ItemId As Integer = Nothing
        Dim comItemid As Integer = Nothing
        Dim SubItemId As Integer = Nothing
        Dim COSTID As String = Nothing
        Dim ds As New Data.DataSet
        ds.Clear()
        ''Find ItemId
        strSql = " Select ItemId from " & cnAdminDb & "..ItemMast where ItemName = '" & cmbItem_OWN.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(ds, "ItemId")
        If ds.Tables("ItemId").Rows.Count > 0 Then
            ItemId = Val(ds.Tables("Itemid").Rows(0).Item("ItemId").ToString)
        Else
            ItemId = 0
        End If
        strSql = " Select pmid from " & cnAdminDb & "..pmMast where pmName = '" & cmbComItem_Man.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(ds, "pmid")
        If ds.Tables("pmid").Rows.Count > 0 Then
            ComItemId = Val(ds.Tables("pmid").Rows(0).Item("pmId").ToString)
        Else
            comItemId = 0
        End If
        ''Find SubItemId
        '598
        If cmbSubItem_Man.Text <> "ALL" Then
            strSql = " Select Pmsubid from " & cnAdminDb & "..PmSubMast where PmSubName = '" & cmbSubItem_Man.Text & "'  and Pmid = " & comItemid
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(ds, "Pmsubid")
            If ds.Tables("Pmsubid").Rows.Count > 0 Then
                SubItemId = Val(ds.Tables("Pmsubid").Rows(0).Item("PmSubid").ToString)
            Else
                SubItemId = 0
            End If
        Else
            SubItemId = 0
        End If

        ''Find COSTID
        If cmbCostCentre_own.Text <> "" Then
            strSql = " Select CostId from " & cnAdminDb & "..CostCentre where CostName = '" & cmbCostCentre_own.Text & "'"
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

        strSql = " Update " & cnAdminDb & "..Pmvalidate Set"
        strSql += " ItemId= " & ItemId & ""
        strSql += " ,PmId= " & comItemid & ""
        strSql += " ,PmSubId= " & SubItemId & ""
        strSql += " ,COSTID='" & COSTID & "'"
        strSql += " ,from_wt=" & Val(txtWtFrom.Text) & ""
        strSql += " ,to_wt=" & Val(txtWtTo.Text) & ""
        strSql += " ,from_pcs=" & Val(txtPieceFrom.Text) & ""
        strSql += " ,to_pcs=" & Val(txtPieceTo.Text) & ""
        strSql += " ,from_amt=" & Val(txtFromAmt.Text) & ""
        strSql += " ,to_amt=" & Val(txtToAmt.Text) & ""
        strSql += " ,Userid=" & userId & ""
        strSql += " ,Updated='" & Today.Date.ToString("yyyy-MM-dd") & "'"
        strSql += " ,Uptime='" & Date.Now.ToLongTimeString & "'"
        strSql += " where Sno = '" & Sno & "'"
        Try
            ExecQuery(SyncMode.Master, strSql, cn)
            flagSave = False
            flagUpdate = False

            funcCallGrid()
            txtWtFrom.Clear()
            txtWtTo.Clear()
            txtPieceFrom.Clear()
            txtPieceTo.Clear()
            txtToAmt.Clear()
            txtFromAmt.Clear()
            txtWtFrom.Select()
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function
    Function funcGetDetails(ByVal temp As String) As String
        strSql = " SELECT"
        strSql += vbCrLf + " (SELECT METALNAME FROM " & cnAdminDb & "..METALMAST AS M WHERE M.METALID=I.METALID)AS METALNAME,"
        strSql += vbCrLf + " I.ITEMNAME,"
        strSql += vbCrLf + " (SELECT PMNAME FROM " & cnAdminDb & "..PMMAST AS I WHERE I.PMID=SR.PMID)AS COMNAME,"
        strSql += vbCrLf + " ISNULL((SELECT PMSUBNAME FROM " & cnAdminDb & "..PMSUBMAST AS S WHERE S.PMSUBID=SR.PMSUBID),'')AS SUBNAME,"
        strSql += vbCrLf + " ISNULL((SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE AS C WHERE C.COSTID = SR.COSTID),'')AS COSTNAME,"
        strSql += vbCrLf + " FROM_WT,TO_WT,FROM_PCS,TO_PCS, FROM_AMT,TO_AMT FROM " & cnAdminDb & "..PMVALIDATE AS SR"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST I ON SR.ITEMID = I.ITEMID"
        strSql += vbCrLf + " WHERE SNO = '" & temp & "'"
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
            strSql += vbCrLf + " where METALID = (select Metalid from " & cnAdminDb & "..MetalMast where MetalName = '" & cmbMetal_Man.Text & "')"
            strSql += vbCrLf + " Order by ItemName"
            objGPack.FillCombo(strSql, cmbItem_OWN)

            cmbItem_OWN.Text = .Item("ItemName").ToString
            cmbComItem_Man.Text = .Item("ComName").ToString
            cmbSubItem_Man.Text = .Item("SubName").ToString
            cmbCostCentre_own.Text = .Item("CostName").ToString

            txtWtFrom.Text = .Item("from_wt").ToString
            txtWtTo.Text = .Item("to_wt").ToString
            txtPieceFrom.Text = .Item("from_pcs").ToString
            txtPieceTo.Text = .Item("to_pcs").ToString
            txtFromAmt.Text = .Item("from_Amt").ToString
            txtToAmt.Text = .Item("to_Amt").ToString

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
            objGPack.FillCombo(strSql, cmbCostCentre_own)
            cmbCostCentre_own.Enabled = True
        Else
            cmbCostCentre_own.Enabled = False
        End If
    End Function
    Function funcUniqueValidation() As Boolean
        strSql = " Declare @wtFrom as float,@wtTo as Float,@pcFrom as int,@pcTo as int,@amtFrom as float,@amtTo as Float"
        strSql += " Set @wtFrom = " & Val(txtWtFrom.Text) & ""
        strSql += " Set @wtTo = " & Val(txtWtTo.Text) & ""
        strSql += " Set @pcFrom = " & Val(txtPieceFrom.Text) & ""
        strSql += " Set @pcTo = " & Val(txtPieceTo.Text) & ""
        strSql += " Set @amtFrom = " & Val(txtFromAmt.Text) & ""
        strSql += " Set @amtTo = " & Val(txtToAmt.Text) & ""
        strSql += " select 1 from " & cnAdminDb & "..Pmvalidate"
        strSql += " where "
        strSql += " ItemId = (select ItemId from " & cnAdminDb & "..ItemMast where ItemName = '" & cmbItem_OWN.Text & "')"
        If cmbCostCentre_own.Text <> "" Then strSql += " and COSTID = isnull((select CostId from " & cnAdminDb & "..CostCentre where CostName = '" & cmbCostCentre_own.Text & "'),'')"
        strSql += " and"
        strSql += " ((from_Wt between @wtFrom and @wtTo) OR (To_Wt between @wtFrom and @wtTo))"
        strSql += " and"
        strSql += " ((from_pcs between @pcFrom and @pcTo) OR (To_pcs between @pcFrom and @pcTo))"
        strSql += " and"
        strSql += " ((from_amt between @amtFrom and @amtTo) OR (To_amt between @amtFrom and @amtTo))"

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

    Private Sub frmStockReorder_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
        'If e.KeyChar = Chr(Keys.F9) Then
        '    cmbCostSearch.Focus()
        'End If
    End Sub
    Private Sub frmStockReorder_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        funcNew()
        SearchCostCentre(cmbCostSearch_OWN)
        SearchItem(cmbItemSearch_OWN)
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
        funcSave()
    End Sub
    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click, OpenToolStripMenuItem.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        funcCallGrid()
        cmbCostSearch_OWN.Focus()
        'gridView.Focus()
    End Sub
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        funcNew()
    End Sub
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        funcExit()
    End Sub
    Private Sub cmbMetal_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbMetal_Man.SelectedIndexChanged
        If flagSave = True Then
            Exit Sub
        End If
        cmbItem_OWN.Items.Clear()
        cmbItem_OWN.Items.Add("ALL")
        strSql = " Select ItemName from " & cnAdminDb & "..ItemMast"
        strSql += " where METALID = (select Metalid from " & cnAdminDb & "..MetalMast where MetalName = '" & cmbMetal_Man.Text & "')"
        strSql += " Order by ItemName"
        objGPack.FillCombo(strSql, cmbItem_OWN)
        cmbItem_OWN.Text = "ALL"
        If COMPL_ISS_MODE = "A" Then cmbItem_OWN.Enabled = False
    End Sub


    Private Sub cmbItem_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbItem_OWN.SelectedIndexChanged


    End Sub

    Private Sub gridView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.GotFocus
        lblStatus.Visible = True
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
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
                'Dim result = MessageBox.Show("Do you want to Delete...", "Message", MessageBoxButtons.YesNo)
                'If result = Windows.Forms.DialogResult.Yes Then
                Dim chkQry As String = "SELECT 1 FROM " & cnAdminDb & "..PMVALIDATE WHERE 1<>1"
                Dim delKey As String = gridView.Rows(gridView.CurrentRow.Index).Cells("SNO").Value.ToString
                DeleteItem(SyncMode.Master, chkQry, "DELETE FROM " & cnAdminDb & "..PMVALIDATE WHERE SNO = '" & delKey & "' ")
                'strSql = " Delete from " & cnAdminDb & "..STKREORDER where Sno='" & gridView("SNo", gridView.CurrentRow.Index).Value & "'"
                'cmd = New OleDbCommand(strSql, cn)
                'If Not tran Is Nothing Then cmd.Transaction = tran
                'ExecQuery(SyncMode.Master, strSql, cn, tran)
                funcCallGrid()
                'funcNew()
                Call btnSearch_Click(sender, e)
                'ElseIf result = Windows.Forms.DialogResult.No Then
                '   Exit Sub
                'End If
            End If
        End If
    End Sub

    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If Not gridView.RowCount > 0 Then
            Exit Sub
        End If
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
        Dim chkQry As String = "SELECT 1 FROM " & cnAdminDb & "..STKREORDER WHERE 1<>1"
        Dim delKey As String = gridView.Rows(gridView.CurrentRow.Index).Cells("SNO").Value.ToString
        DeleteItem(SyncMode.Master, chkQry, "DELETE FROM " & cnAdminDb & "..STKREORDER WHERE SNO = '" & delKey & "' ")
        funcCallGrid()
    End Sub

    Private Sub gridView_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.LostFocus
        lblStatus.Visible = False
    End Sub
    Private Sub txtPiece_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPieceFrom.GotFocus

        If funcUniqueValidation() = True Then
            txtWtFrom.Focus()
            MsgBox(E0002, MsgBoxStyle.Information)
            Exit Sub
        End If
    End Sub
    Private Sub GroupBox1_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub txtWtTo_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtWtTo.Leave
        'If flagSave = True Then
        '    Exit Sub
        'End If
        Dim wt As Double
        If txtWtFrom.Text <> "" Then
            If txtWtTo.Text <> "" Then
                wt = (Convert.ToDouble(txtWtFrom.Text) + Convert.ToDouble(txtWtTo.Text)) / 2
                txtPieceTo.Text = wt
            End If
        End If
        'If Not (txtWtFrom.Focus()) Then

    End Sub

    Private Sub txtPiece_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPieceFrom.Leave
        Dim wt As Double
        If txtPieceTo.Text <> "" Then
            wt = txtPieceTo.Text
            txtPieceTo.Text = wt * Val(txtPieceFrom.Text)
        End If
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim calWt As Boolean = False
        Dim calRate As Boolean = False


        strSql = " SELECT SNO,"
        strSql += vbCrLf + " (CASE WHEN SR.ITEMID=0 THEN 'AMOUNT WISE' ELSE I.ITEMNAME END)ITEMNAME, "
        strSql += vbcrlf + " P.PMNAME, "
        strSql += vbcrlf + " PS.PMSUBNAME,"
        strSql += vbcrlf + " C.COSTNAME ,"
        strSql += vbcrlf + " FROM_WT AS FROMWEIGHT,"
        strSql += vbcrlf + " TO_WT as TOWEIGHT,"
        strSql += vbcrlf + " FROM_PCS,TO_PCS,"
        strSql += vbcrlf + " FROM_AMT AS FROMAMOUNT ,"
        strSql += vbcrlf + " TO_AMT as TOAMOUNT"
        strSql += vbcrlf + " FROM " & cnAdminDb & "..PMVALIDATE AS SR"
        strSql += vbcrlf + " LEFT OUTER JOIN " & cnAdminDb & "..ITEMMAST I on I.ITEMID=SR.ITEMID"
        strSql += vbcrlf + " LEFT OUTER JOIN " & cnAdminDb & "..PMMAST P on P.PMID=SR.PMID"
        strSql += vbcrlf + " LEFT OUTER JOIN " & cnAdminDb & "..PMSUBMAST PS on PS.PMSUBID=SR.PMSUBID"
        strSql += vbcrlf + " LEFT OUTER JOIN " & cnAdminDb & "..COSTCENTRE C on C.COSTID=SR.COSTID"
        If UCase(cmbSubItemSearch_OWN.Text) = "ALL" Then
            If cmbCostSearch_OWN.Text <> "" And cmbItemSearch_OWN.Text <> "" Or cmbSubItemSearch_OWN.Text <> "" Then
                If cmbCostSearch_OWN.Text <> "" And cmbCostSearch_OWN.Text <> "ALL" Then strSql += vbCrLf + " where C.CostName='" & cmbCostSearch_OWN.Text & "'"
                If cmbItemSearch_OWN.Text <> "" And cmbItemSearch_OWN.Text <> "ALL" Then strSql += vbCrLf + " and P.PMName='" & cmbItemSearch_OWN.Text & "' "
                If cmbSubItemSearch_OWN.Text <> "" And cmbSubItemSearch_OWN.Text <> "ALL" Then strSql += vbCrLf + " and PS.SubName='" & cmbSubItemSearch_OWN.Text & "'"
                funcOpenGrid(strSql, gridView)
                With gridView
                    .Columns("SNO").Width = 0
                    .Columns("ITEMNAME").Width = 200
                    .Columns("PMNAME").Width = 200
                    .Columns("PMSUBNAME").Width = 200
                    .Columns("COSTNAME").Width = 120
                End With
                gridView.Select()
                gridView.Columns(5).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                gridView.Columns(6).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                gridView.Columns(7).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                gridView.Columns(8).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                gridView.Columns(9).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                gridView.Columns(10).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End If
            'ElseIf cmbCostSearch.Text <> "" Or (cmbItemSearch.Text <> "" And cmbSubItemSearch.Text <> "") Then
        Else
            If cmbCostSearch_OWN.Text <> "" Or cmbItemSearch_OWN.Text <> "" Or cmbSubItemSearch_OWN.Text <> "" Then
                strSql += vbCrLf + " where 1=1 "
                If cmbCostSearch_OWN.Text <> "" And cmbCostSearch_OWN.Text <> "ALL" Then strSql += vbCrLf + " and C.CostName='" & cmbCostSearch_OWN.Text & "'"
                If cmbItemSearch_OWN.Text <> "" And cmbItemSearch_OWN.Text <> "ALL" Then strSql += vbCrLf + " and P.PMName='" & cmbItemSearch_OWN.Text & "' "
                If cmbSubItemSearch_OWN.Text <> "" And cmbSubItemSearch_OWN.Text <> "ALL" Then strSql += vbCrLf + " and PS.PMSUBNAME='" & cmbSubItemSearch_OWN.Text & "'"
                funcOpenGrid(strSql, gridView)
                With gridView
                    .Columns("SNO").Width = 0
                    .Columns("ITEMNAME").Width = 200
                    .Columns("PMNAME").Width = 200
                    .Columns("PMSUBNAME").Width = 200
                    .Columns("COSTNAME").Width = 120
                End With
                gridView.Select()
                gridView.Columns(5).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                gridView.Columns(6).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                gridView.Columns(7).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                gridView.Columns(8).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                gridView.Columns(9).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                gridView.Columns(10).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End If
        End If
    End Sub

    Private Function SearchCostCentre(ByVal combo As ComboBox) As Integer
        strSql = " Select CostName from " & cnAdminDb & "..CostCentre order by CostName"
        objGPack.FillCombo(strSql, combo, , False)
    End Function
    Private Function SearchItem(ByVal combo As ComboBox) As Integer
        cmbSubItemSearch_OWN.Items.Add("ALL")
        strSql = " Select PMName from " & cnAdminDb & "..PmMast order by PmName"
        da = New OleDbDataAdapter(strSql, cn)
        Dim dt As New DataTable()
        da.Fill(dt)
        cmbItemSearch_OWN.DataSource = dt
        cmbItemSearch_OWN.DisplayMember = "PmName"
        cmbItemSearch_OWN.ValueMember = "PmName"

    End Function
    Private Function SearchSubItem(ByVal combo As ComboBox) As Integer
        'cmbSubItemSearch_OWN.Items.Add("ALL")
        cmbSubItemSearch_OWN.Items.Clear()
        strSql = vbCrLf + " SELECT 'ALL' PMSUBNAME,'ALL' PMSUBID,1 RESULT"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT PMSUBNAME,CONVERT(vARCHAR,PMSUBID),2 RESULT FROM " & cnAdminDb & "..PMSUBMAST"
        If cmbItemSearch_OWN.Text <> "" Then strSql += vbCrLf + " WHERE PMID = (SELECT PMID FROM " & cnAdminDb & "..PMMAST WHERE PMNAME = '" & cmbItemSearch_OWN.Text & "')"
        strSql += " order by PMSubName"
        objGPack.FillCombo(strSql, combo, , False)

    End Function

    Private Sub cmbItemSearch_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbItemSearch_OWN.SelectedIndexChanged
        SearchSubItem(cmbSubItemSearch_OWN)
        ' cmbSubItemSearch_OWN.Items.Add("ALL")
    End Sub

    Private Sub frmStockReorder_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = (Keys.F9) Then
            cmbCostSearch_OWN.Focus()
            'cmbSubItemSearch_OWN.Items.Add("ALL")
        End If
    End Sub

    Private Sub txtWtFrom_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtWtFrom.Leave

    End Sub

    Private Sub txtWtTo_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtWtTo.TextChanged
        If flagSave = True Then
            Exit Sub
        End If
    End Sub

    Private Sub btnDelete_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles btnDelete.KeyDown

    End Sub

    Private Sub Label2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label2.Click

    End Sub

    Private Sub cmbComItem_Man_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbComItem_Man.SelectedIndexChanged
        cmbSubItem_Man.Text = ""
        '598
        cmbSubItem_Man.Items.Add("ALL")
        strSql = " Select PMSubName from " & cnAdminDb & "..PMSubMast where "
        strSql += " PMId = (select PMId from " & cnAdminDb & "..PMMast where PMName = '" & cmbComItem_Man.Text & "')"
        strSql += " Order by PMSubName"
        If flagSave = True Then
            objGPack.FillCombo(strSql, cmbSubItem_Man, False, False)
        Else
            objGPack.FillCombo(strSql, cmbSubItem_Man, False)
        End If
        If cmbSubItem_Man.Items.Count > 0 Then cmbSubItem_Man.Enabled = True Else cmbSubItem_Man.Enabled = False

    End Sub

    Private Sub txtFromAmt_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtFromAmt.KeyDown
        If e.KeyCode = Keys.Enter And COMPL_ISS_MODE = "A" And flagUpdate = False Then
            If txtFromAmt.Text = "" Then MsgBox("From Amount is empty") : txtFromAmt.Focus() : Exit Sub
            strSql = "SELECT FROM_AMT,TO_AMT FROM " & cnAdminDb & "..PMVALIDATE WHERE ITEMID='0' AND " & txtFromAmt.Text & " BETWEEN FROM_AMT AND TO_AMT "
            Dim dtChk As DataTable
            dtChk = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtChk)
            If dtChk.Rows.Count > 0 Then
                MsgBox("From Amount alredy in Range " & dtChk.Rows(0)("FROM_AMT").ToString & " - " & dtChk.Rows(0)("TO_AMT").ToString)
                txtFromAmt.Text = ""
                txtFromAmt.Focus()
            End If
        End If
    End Sub

    Private Sub txtToAmt_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtToAmt.KeyDown
        If e.KeyCode = Keys.Enter And COMPL_ISS_MODE = "A" And flagUpdate = False Then
            If txtToAmt.Text = "" Then MsgBox("To Amount is empty") : txtToAmt.Focus() : Exit Sub
            strSql = "SELECT FROM_AMT,TO_AMT FROM " & cnAdminDb & "..PMVALIDATE WHERE ITEMID='0' AND " & txtToAmt.Text & " BETWEEN FROM_AMT AND TO_AMT "
            Dim dtChk As DataTable
            dtChk = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtChk)
            If dtChk.Rows.Count > 0 Then
                MsgBox("To Amount alredy in Range " & dtChk.Rows(0)("FROM_AMT").ToString & " - " & dtChk.Rows(0)("TO_AMT").ToString)
                txtToAmt.Text = ""
                txtToAmt.Focus()
            End If
            strSql = "SELECT FROM_AMT,TO_AMT FROM " & cnAdminDb & "..PMVALIDATE WHERE ITEMID='0' AND " & Val(txtFromAmt.Text) & " < FROM_AMT AND " & Val(txtToAmt.Text) & " > TO_AMT  "
            dtChk = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtChk)
            If dtChk.Rows.Count > 0 Then
                MsgBox("Range " & dtChk.Rows(0)("FROM_AMT").ToString & " - " & dtChk.Rows(0)("TO_AMT").ToString & " Between these Amount")
                txtToAmt.Text = ""
                txtToAmt.Focus()
            End If
        End If
    End Sub
End Class