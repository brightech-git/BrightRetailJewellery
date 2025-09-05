Imports System.Data.OleDb
Public Class frmRangeMaster
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim flagSave As Boolean = False
    Dim flagUpdate As Boolean = False
    Dim Sno As Integer = Nothing
    Dim chkCostName As String = ""
    Dim chkItem As String = ""
    Dim chkSubItem As String = ""
    Dim dtRow As New DataTable
    Dim dtGridView As New DataTable
    Dim dRow As DataRow = Nothing
    Dim dViewRow As DataRow = Nothing

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        funcGridStyle(gridView)
        cmbSubItemSearch_OWN.Items.Add("ALL")
    End Sub
    Function funcCallGrid() As Integer
        strSql = " SELECT SNO,"
        strSql += " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST AS I WHERE I.ITEMID=RM.ITEMID)AS ITEMNAME,"
        strSql += " ISNULL((SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST AS S WHERE S.SUBITEMID=RM.SUBITEMID),'')AS SUBITEMNAME,"
        strSql += " ISNULL((SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE AS C WHERE C.COSTID = RM.COSTID),'')AS COSTNAME,"
        strSql += " FROMWEIGHT,TOWEIGHT,CAPTION"
        strSql += " FROM " & cnAdminDb & "..RANGEMAST AS RM WHERE 1=1"
        If cmbCostCentre_Own.Text <> "" And cmbCostCentre_Own.Text <> "ALL" Then
            strSql += " AND RM.COSTID =(SELECT COSTID FROM  " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME='" & cmbCostCentre_Own.Text & "')"
        End If
        If cmbItem_Own.Text <> "" And cmbItem_Own.Text <> "ALL" Then
            strSql += " AND RM.ITEMID =(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbItem_Own.Text & "')"
        End If
        If cmbSubItem_Own.Text <> "" And UCase(cmbSubItem_Own.Text) <> "ALL" Then
            strSql += " AND RM.SUBITEMID =(SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME='" & cmbSubItem_Own.Text & "')"
        End If
        gridView.Columns.Clear()
        gridView.DataSource = Nothing
        Dim chkcol As New DataGridViewCheckBoxColumn
        chkcol.HeaderText = "Chk"
        chkcol.Name = "SELECT"
        chkcol.Width = 50
        funcOpenGrid(strSql, gridView)
        gridView.Columns.Insert(0, chkcol)
        gridView.Columns("SNO").Visible = False
        gridView.Columns("ITEMNAME").ReadOnly = True
        gridView.Columns("SUBITEMNAME").ReadOnly = True
        gridView.Columns("COSTNAME").ReadOnly = True
        gridView.Columns("ITEMNAME").Width = 200
        gridView.Columns("SUBITEMNAME").Width = 250
        gridView.Columns("COSTNAME").Width = 150
        gridView.Columns("FROMWEIGHT").HeaderText = "FROM WEIGHT"
        gridView.Columns("FROMWEIGHT").Width = 100
        gridView.Columns("FROMWEIGHT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

        gridView.Columns("TOWEIGHT").HeaderText = "TO WEIGHT"
        gridView.Columns("TOWEIGHT").Width = 100
        gridView.Columns("TOWEIGHT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
    End Function
    Function SelectAll(ByVal Obj As Object)
        If gridView.Rows.Count > 0 Then
            For i As Integer = 0 To gridView.Rows.Count - 1
                If Obj.Checked = True Then
                    gridView.Rows(i).Cells("SELECT").Value = True
                Else
                    gridView.Rows(i).Cells("SELECT").Value = False
                End If
            Next
        End If
    End Function
    Function funcNew() As Integer
        objGPack.TextClear(Me)
        flagSave = False
        flagUpdate = False
        funcLoadItemName()
        funcCallGrid()
        dtRow.Clear()
        chkListItem.Items.Clear()
        chkListSubItem.Items.Clear()
        chkListCostCentre.Items.Clear()
        dtGridView.Clear()
        'gridViewWt.Rows.Clear()
        txtFromWt.Focus()
    End Function
    Function funcExit() As Integer
        Me.Close()
    End Function
    Function funcSave() As Integer
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Function
        If objGPack.Validator_Check(Me) Then Exit Function
        If Not gridViewWt.Rows.Count > 0 Then
            MsgBox(Me.GetNextControl(txtWtFrom_Own, False).Text + E0001, MsgBoxStyle.Information)
            txtWtFrom_Own.Focus()
            Exit Function
        End If

        If Not Val(txtWtFrom_Own.Text) <= Val(txtWtTo.Text) Then
            MsgBox(E0005, MsgBoxStyle.Information)
            txtWtFrom_Own.Focus()
            Exit Function
        End If

        If funcUniqueValidation() = True Then
            MsgBox(E0002, MsgBoxStyle.Information)
            txtFromWt.Focus()
            Exit Function
        End If
        Try
            If flagSave = False Then
                funcAdd()
            Else
                funcUpdate()
            End If
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try

    End Function
    Function funcAdd() As Integer
        Dim ItemId As String = Nothing
        Dim SubItemId As String = Nothing
        Dim COSTID As String = Nothing
        Dim dtItem, dtSubItem, dtCost As New DataTable()
        dtItem.Clear()
        dtSubItem.Clear()
        dtCost.Clear()
        ''Find ItemId

        If gridShow.Rows.Count > 0 Then
            For i As Integer = 0 To gridShow.Rows.Count - 1

                strSql = " Select ItemId from " & cnAdminDb & "..ItemMast where ItemName ='" & gridShow("ITEMNAME", i).Value.ToString & "'"
                da = New OleDbDataAdapter(strSql, cn)
                dtItem = New DataTable
                da.Fill(dtItem)
                If dtItem.Rows.Count > 0 Then
                    ItemId = dtItem.Rows(0).Item("ItemId").ToString
                Else
                    ItemId = 0
                End If

                ''Find SubItemId

                strSql = " Select SubItemId from " & cnAdminDb & "..SubItemMast where SubItemName ='" & gridShow("SUBITEMNAME", i).Value.ToString & "'"
                strSql += "  AND ITEMID =" & ItemId
                da = New OleDbDataAdapter(strSql, cn)
                dtSubItem = New DataTable
                da.Fill(dtSubItem)
                If dtSubItem.Rows.Count > 0 Then
                    SubItemId = dtSubItem.Rows(0).Item("SubItemid").ToString
                Else
                    SubItemId = 0
                End If

                ''Find COSTID
                strSql = " Select CostId from " & cnAdminDb & "..CostCentre where CostName ='" & gridShow("COSTCENTRE", i).Value.ToString & "'"
                da = New OleDbDataAdapter(strSql, cn)
                dtCost = New DataTable
                da.Fill(dtCost)
                If dtCost.Rows.Count > 0 Then
                    COSTID = dtCost.Rows(0).Item("CostId")
                Else
                    COSTID = ""
                End If


                strSql = " INSERT INTO " & cnAdminDb & "..RANGEMAST"
                strSql += " ("
                strSql += " ITEMID,SUBITEMID,COSTID,"
                strSql += " FROMWEIGHT,TOWEIGHT,CAPTION,"
                strSql += " USERID,"
                strSql += " UPDATED,UPTIME,DISPLAYORDER)VALUES ("
                strSql += " " & ItemId & "" 'ItemId
                strSql += " ," & SubItemId & "" 'SubItemId
                strSql += " ,'" & COSTID & "'" 'COSTID
                strSql += " ," & gridShow("FROMWEIGHT", i).Value.ToString & "" 'fromweight
                strSql += " ," & gridShow("TOWEIGHT", i).Value.ToString & "" 'toweight
                strSql += " ,'" & gridShow("CAPTION", i).Value.ToString & "'" 'Caption
                strSql += " ," & userId & "" 'Userid
                strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'Updated
                strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'Uptime
                strSql += " , " & gridShow("DISPLAYORDER", i).Value.ToString & ""
                strSql += " )"
                Try
                    ExecQuery(SyncMode.Master, strSql, cn)
                Catch ex As Exception
                    MsgBox(ex.Message)
                    MsgBox(ex.StackTrace)
                End Try
            Next
            funcCallGrid()
            MsgBox("Successfully Saved...")
            txtWtFrom_Own.Clear()
            txtWtTo.Clear()
            funcNew()
            txtWtFrom_Own.Select()

        Else
            MsgBox("Invalid Entry...")
        End If
    End Function
    Function funcUpdate() As Integer
        Dim ItemId As Integer = Nothing
        Dim SubItemId As Integer = Nothing
        Dim COSTID As String = Nothing
        Dim dtItem, dtSubItem, dtCost As New DataTable()
        dtItem.Clear()
        dtSubItem.Clear()
        dtCost.Clear()
        ''Find ItemId

        For i As Integer = 0 To gridShow.Rows.Count - 1

            strSql = " Select ItemId from " & cnAdminDb & "..ItemMast where ItemName ='" & gridShow("ITEMNAME", i).Value.ToString & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtItem)
            If dtItem.Rows.Count > 0 Then
                ItemId = dtItem.Rows(i).Item("ItemId").ToString
            Else
                ItemId = 0
            End If

            ''Find SubItemId

            strSql = " Select SubItemId from " & cnAdminDb & "..SubItemMast where SubItemName ='" & gridShow("SUBITEMNAME", i).Value.ToString & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtSubItem)
            If dtSubItem.Rows.Count > 0 Then
                SubItemId = dtSubItem.Rows(i).Item("SubItemid").ToString
            Else
                SubItemId = 0
            End If

            ''Find COSTID
            strSql = " Select CostId from " & cnAdminDb & "..CostCentre where CostName ='" & gridShow("COSTCENTRE", i).Value.ToString & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtCost)
            If dtCost.Rows.Count > 0 Then
                COSTID = dtCost.Rows(i).Item("CostId")
            Else
                COSTID = ""
            End If

            strSql = " UPDATE " & cnAdminDb & "..RANGEMAST SET"
            strSql += " ITEMID= " & ItemId & ""
            strSql += " ,SUBITEMID= " & SubItemId & ""
            strSql += " ,COSTID='" & COSTID & "'"
            strSql += " ,FROMWEIGHT=" & gridShow("FROMWEIGHT", i).Value.ToString & ""
            strSql += " ,TOWEIGHT=" & gridShow("TOWEIGHT", i).Value.ToString & ""
            strSql += " ,CAPTION='" & gridShow("CAPTION", i).Value.ToString & "'"
            strSql += " ,USERID=" & userId & ""
            strSql += " ,UPDATED='" & Today.Date.ToString("yyyy-MM-dd") & "'"
            strSql += " ,UPTIME='" & Date.Now.ToLongTimeString & "'"
            strSql += " WHERE SNO = '" & Sno & "'"
            Try
                ExecQuery(SyncMode.Master, strSql, cn)
                MsgBox("Successfully updated...")
            Catch ex As Exception
                MsgBox(ex.Message)
                MsgBox(ex.StackTrace)
            End Try
        Next
        funcNew()
    End Function
    Function funcUniqueValidation() As Boolean
        Dim ItemId As Integer = Nothing
        Dim SubItemId As Integer = Nothing
        Dim COSTID As String = Nothing
        Dim j As Integer = Nothing
        Dim dtItem, dtSubItem, dtCost As New DataTable()
        dtItem.Clear()
        dtSubItem.Clear()
        dtCost.Clear()

        If chkListItem.CheckedItems.Count > 0 Or chkListSubItem.CheckedItems.Count > 0 Or chkListCostCentre.CheckedItems.Count > 0 Then
            For i As Integer = 0 To gridShow.Rows.Count - 1

                strSql = " Select ItemId from " & cnAdminDb & "..ItemMast where ItemName ='" & gridShow("ITEMNAME", i).Value.ToString & "'"
                da = New OleDbDataAdapter(strSql, cn)
                dtItem = New DataTable
                da.Fill(dtItem)
                If dtItem.Rows.Count > 0 Then
                    ItemId = dtItem.Rows(0).Item("ItemId").ToString
                Else
                    ItemId = 0
                End If

                ''Find SubItemId

                strSql = " Select SubItemId from " & cnAdminDb & "..SubItemMast where SubItemName ='" & gridShow("SUBITEMNAME", i).Value.ToString & "'"
                da = New OleDbDataAdapter(strSql, cn)
                dtSubItem = New DataTable
                da.Fill(dtSubItem)

                If dtSubItem.Rows.Count > 0 Then
                    SubItemId = dtSubItem.Rows(0).Item("SubItemId").ToString
                Else
                    SubItemId = 0
                End If

                ''Find COSTID
                strSql = " Select CostId from " & cnAdminDb & "..CostCentre where CostName ='" & gridShow("COSTCENTRE", i).Value.ToString & "'"
                da = New OleDbDataAdapter(strSql, cn)
                dtCost = New DataTable
                da.Fill(dtCost)
                If dtCost.Rows.Count > 0 Then
                    COSTID = dtCost.Rows(0).Item("CostId")
                Else
                    COSTID = ""
                End If

                strSql = " Declare @wtFrom as float,@wtTo as Float"
                strSql += " Set @wtFrom = " & gridShow("FROMWEIGHT", i).Value & ""
                strSql += " Set @wtTo = " & gridShow("TOWEIGHT", i).Value & ""
                strSql += " select 1 from " & cnAdminDb & "..RANGEMAST"
                strSql += " where ((fromWeight between @wtFrom and @wtTo) OR (ToWeight between @wtFrom and @wtTo))"
                strSql += " and ItemId =" & ItemId & ""
                strSql += " and SubItemId = " & SubItemId & ""
                If chkListCostCentre.Items.Count > 0 Then
                    strSql += " and COSTID ='" & COSTID & "'"
                End If
                If flagSave = True Then
                    strSql += " and sno <> '" & Sno & "'"
                End If
                Dim dt As New DataTable
                dt.Clear()
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dt)
                If dt.Rows.Count > 0 Then
                    Return True
                    'MsgBox("Already Exist...")
                End If
            Next
        End If
        Return False
    End Function
    'Function CheckExists(ByVal TableName As String, ByVal GetName As String, ByVal Value As String)
    '    If Value = "" And ListAllowEmptyValues.Contains(TableName) Then Return True
    '    strSql = " SELECT 1 FROM " & cnAdminDb & ".." & TableName & " WHERE " & GetName & "='" & Value & "' "
    '    If objGPack.GetSqlValue(strSql, , 0) <> 1 Then
    '        Return False
    '    End If
    '    Return True
    'End Function
    Private Sub LoadGridView()
        Dim subItem As New DataTable()
        Dim subItemName As String
        dtGridView.Clear()
        dViewRow = dtGridView.NewRow()
        Dim _displayorder As Integer = 0
        If gridViewWt.Rows.Count > 0 Then
            For i As Integer = 0 To gridViewWt.Rows.Count - 1
                For j As Integer = 0 To chkListItem.CheckedItems.Count - 1
                    _displayorder = _displayorder + 1
                    If chkListSubItem.Items.Count > 0 Then
                        For k As Integer = 0 To chkListSubItem.CheckedItems.Count - 1
                            If chkListCostCentre.Items.Count > 0 Then
                                For n As Integer = 0 To chkListCostCentre.CheckedItems.Count - 1
                                    dViewRow = dtGridView.NewRow()
                                    dViewRow("FROMWEIGHT") = gridViewWt("FROMWEIGHT", i).Value.ToString
                                    dViewRow("TOWEIGHT") = gridViewWt("TOWEIGHT", i).Value.ToString
                                    dViewRow("CAPTION") = gridViewWt("CAPTION", i).Value.ToString
                                    dViewRow("ITEMNAME") = chkListItem.CheckedItems.Item(j).ToString
                                    ''Find SubItemId
                                    strSql = "SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID IN("
                                    strSql += "SELECT TOP 1 ITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID IN"
                                    strSql += "(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & chkListItem.CheckedItems.Item(j).ToString & "'))"
                                    da = New OleDbDataAdapter(strSql, cn)
                                    subItem = New DataTable
                                    da.Fill(subItem)
                                    If subItem.Rows.Count > 0 Then
                                        If subItem.Rows(0).Item("ITEMNAME").ToString = chkListItem.CheckedItems.Item(j).ToString Then
                                            dViewRow("SUBITEMNAME") = chkListSubItem.CheckedItems.Item(k).ToString
                                        End If
                                    Else
                                        dViewRow("SUBITEMNAME") = ""
                                    End If
                                    dViewRow("COSTCENTRE") = chkListCostCentre.CheckedItems.Item(n).ToString
                                    dViewRow("DISPLAYORDER") = _displayorder
                                    dtGridView.Rows.Add(dViewRow)
                                Next
                            Else
                                dViewRow = dtGridView.NewRow()
                                dViewRow("FROMWEIGHT") = gridViewWt("FROMWEIGHT", i).Value.ToString
                                dViewRow("TOWEIGHT") = gridViewWt("TOWEIGHT", i).Value.ToString
                                dViewRow("CAPTION") = gridViewWt("CAPTION", i).Value.ToString
                                dViewRow("ITEMNAME") = chkListItem.CheckedItems.Item(j).ToString
                                ''Find SubItemId
                                strSql = "SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID IN("
                                strSql += "SELECT TOP 1 ITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID IN"
                                strSql += "(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & chkListItem.CheckedItems.Item(j).ToString & "'))"
                                da = New OleDbDataAdapter(strSql, cn)
                                subItem = New DataTable
                                da.Fill(subItem)
                                If subItem.Rows.Count > 0 Then
                                    If subItem.Rows(0).Item("ITEMNAME").ToString = chkListItem.CheckedItems.Item(j).ToString Then
                                        dViewRow("SUBITEMNAME") = chkListSubItem.CheckedItems.Item(k).ToString
                                    End If
                                Else
                                    dViewRow("SUBITEMNAME") = ""
                                End If
                                dViewRow("COSTCENTRE") = ""
                                dViewRow("DISPLAYORDER") = _displayorder
                                dtGridView.Rows.Add(dViewRow)
                            End If
                        Next
                    Else
                        If chkListCostCentre.Items.Count > 0 Then
                            For n As Integer = 0 To chkListCostCentre.CheckedItems.Count - 1
                                dViewRow = dtGridView.NewRow()
                                dViewRow("FROMWEIGHT") = gridViewWt("FROMWEIGHT", i).Value.ToString
                                dViewRow("TOWEIGHT") = gridViewWt("TOWEIGHT", i).Value.ToString
                                dViewRow("CAPTION") = gridViewWt("CAPTION", i).Value.ToString
                                dViewRow("ITEMNAME") = chkListItem.CheckedItems.Item(j).ToString
                                dViewRow("SUBITEMNAME") = ""
                                dViewRow("COSTCENTRE") = chkListCostCentre.CheckedItems.Item(n).ToString
                                dViewRow("DISPLAYORDER") = _displayorder
                                dtGridView.Rows.Add(dViewRow)
                            Next
                        Else
                            dViewRow = dtGridView.NewRow()
                            dViewRow("FROMWEIGHT") = gridViewWt("FROMWEIGHT", i).Value.ToString
                            dViewRow("TOWEIGHT") = gridViewWt("TOWEIGHT", i).Value.ToString
                            dViewRow("CAPTION") = gridViewWt("CAPTION", i).Value.ToString
                            dViewRow("ITEMNAME") = chkListItem.CheckedItems.Item(j).ToString
                            dViewRow("SUBITEMNAME") = ""
                            dViewRow("COSTCENTRE") = ""
                            dViewRow("DISPLAYORDER") = _displayorder
                            dtGridView.Rows.Add(dViewRow)
                        End If
                    End If
                Next
            Next
            dtGridView.AcceptChanges()
            gridShow.DataSource = dtGridView
            gridShow.Columns("FROMWEIGHT").Width = 85
            gridShow.Columns("TOWEIGHT").Width = 85
            gridShow.Columns("CAPTION").Width = 85
            gridShow.Columns("ITEMNAME").Width = 150
            gridShow.Columns("SUBITEMNAME").Width = 200
            gridShow.Columns("FROMWEIGHT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            gridShow.Columns("TOWEIGHT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            For cnt As Integer = 0 To gridViewWt.ColumnCount - 1
                gridShow.Columns(cnt).HeaderText = UCase(gridShow.Columns(cnt).HeaderText)
            Next
        End If
    End Sub
    Function funcGetDetails(ByVal temp As Integer) As Integer
        Dim chkItem As String = Nothing
        Dim chkSubItem As String = Nothing
        Dim chkCost As String = Nothing

        strSql = " SELECT"
        strSql += " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST AS I WHERE I.ITEMID=RM.ITEMID)AS ITEMNAME,"
        strSql += " ISNULL((SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST AS S WHERE S.SUBITEMID=RM.SUBITEMID),'')AS SUBITEMNAME,"
        strSql += " ISNULL((SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE AS C WHERE C.COSTID = RM.COSTID),'')AS COSTNAME,"
        strSql += " FROMWEIGHT,TOWEIGHT,CAPTION"
        strSql += " FROM " & cnAdminDb & "..RANGEMAST AS RM"
        strSql += " WHERE SNO = '" & temp & "'"
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
            'pnlItem.Enabled = True
            'pnlSubItem.Enabled = True
            pnlCostCentre.Enabled = True
            funcLoadCostCentre()
            funcLoadItemName()
            funcLoadSubItem()
            txtFromWt.Text = .Item("FROMWEIGHT").ToString
            txtToWt.Text = .Item("TOWEIGHT").ToString
            txtCaption.Text = .Item("CAPTION").ToString
            chkItem = dt.Rows(0).Item("ITEMNAME").ToString
            chkSubItem = dt.Rows(0).Item("SUBITEMNAME").ToString
            chkCost = dt.Rows(0).Item("COSTNAME").ToString
            For cnt As Integer = 0 To chkListItem.Items.Count - 1
                If chkListItem.Items(cnt).ToString = chkItem Then
                    chkListItem.SetItemChecked(cnt, True)
                    Exit For
                End If
            Next
            For j As Integer = 0 To chkListSubItem.Items.Count - 1
                If chkListSubItem.Items(j).ToString = chkSubItem Then
                    chkListSubItem.SetItemChecked(j, True)
                    Exit For
                End If
            Next
            For k As Integer = 0 To chkListCostCentre.Items.Count - 1
                If chkListCostCentre.Items(k).ToString = chkCost Then
                    chkListCostCentre.SetItemChecked(k, True)
                    Exit For
                End If
            Next
        End With
        Sno = temp
    End Function
    Function funcLoadCostCentre() As Integer
        If GetAdmindbSoftValue("COSTCENTRE") = "Y" Then
            chkListCostCentre.Enabled = True
            chkListCostCentre.Items.Clear()
            strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            FillCheckedListBox(strSql, chkListCostCentre)
        Else
            chkListCostCentre.Enabled = False
            chkListCostCentre.Items.Clear()
        End If
    End Function
    Function funcLoadItemName() As Integer
        strSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST "
        strSql += " WHERE ACTIVE = 'Y'"
        strSql += " ORDER BY ITEMNAME"
        FillCheckedListBox(strSql, chkListItem)
        SetChecked_CheckedList(chkListItem, chkItemAll.Checked)
    End Function
    Function funcLoadSubItem() As Integer
        chkListSubItem.Items.Clear()
        Dim chkItemName As String = GetChecked_CheckedList(chkListItem)
        strSql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST "
        strSql += " WHERE ACTIVE = 'Y'"
        If chkItemName <> "" Then strSql += " AND ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & chkItemName & ")) "
        strSql += " ORDER BY SUBITEMNAME"
        FillCheckedListBox(strSql, chkListSubItem)
        SetChecked_CheckedList(chkListSubItem, chkSubItemAll.Checked)
    End Function
    Private Sub frmRangeMaster_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub frmRangeMaster_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.tbMain.ItemSize = New Size(1, 1)
        Me.tbMain.Region = New Region(New RectangleF(Me.tbGen.Left, Me.tbGen.Top, Me.tbGen.Width, Me.tbGen.Height))
        Me.tbMain.SelectedTab = tbGen
        'GRPfIELDS.Location = New Point((ScreenWid - GRPfIELDS.Width) / 2, ((ScreenHit - 128) - GRPfIELDS.Height) / 2)

        funcNew()
        dtRow.Columns.Add("FROMWEIGHT")
        dtRow.Columns.Add("TOWEIGHT")
        dtRow.Columns.Add("CAPTION")

        dtGridView.Columns.Add("FROMWEIGHT")
        dtGridView.Columns.Add("TOWEIGHT")
        dtGridView.Columns.Add("CAPTION")
        dtGridView.Columns.Add("ITEMNAME")
        dtGridView.Columns.Add("SUBITEMNAME")
        dtGridView.Columns.Add("COSTCENTRE")

        dtGridView.Columns.Add("DISPLAYORDER")

        funcLoadItemName()
        SearchCostCentre(cmbCostCentre_Own)
        SearchItem(cmbItem_Own)
        If _IsCostCentre = False Then
            cmbCostCentre_Own.Enabled = False
        End If
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        funcSave()
    End Sub

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        tbMain.SelectedIndex = 1
        tbView.Focus()
        funcCallGrid()
        gridView.Focus()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        funcNew()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        funcExit()
    End Sub
    Private Sub gridView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        lblStatus.Visible = True
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            If gridView.RowCount > 0 Then
                gridView.CurrentCell = gridView.CurrentCell
                funcGetDetails(gridView.Item(1, gridView.CurrentRow.Index).Value.ToString)
                tbMain.SelectedTab = tbGen
                txtFromWt.Focus()
            End If
        End If
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If Not gridView.RowCount > 0 Then
            Exit Sub
        End If
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            If MessageBox.Show("Do you want Delete this Item", "Delete Alert", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.OK Then
                For i As Integer = 0 To gridView.Rows.Count - 1
                    With gridView.Rows(i)
                        If gridView.Rows(i).Cells("SELECT").Value = True Then
                            Dim chkQry As String = "SELECT 1 FROM " & cnAdminDb & "..RANGEMAST WHERE 1<>1"
                            Dim delKey As String = gridView.Rows(i).Cells("SNO").Value.ToString
                            DeleteItem_chkgrid(SyncMode.Master, chkQry, "DELETE FROM " & cnAdminDb & "..RANGEMAST WHERE SNO = '" & delKey & "'")
                        End If
                    End With
                Next
                MsgBox("Successfully Deleted..")
            End If
            'Dim chkQry As String = "SELECT 1 FROM " & cnAdminDb & "..RANGEMAST WHERE 1<>1"
            'Dim delKey As String = gridView.Rows(gridView.CurrentRow.Index).Cells("SNO").Value.ToString
            'DeleteItem(SyncMode.Master, chkQry, "DELETE FROM " & cnAdminDb & "..RANGEMAST WHERE SNO = '" & delKey & "'")
        End If

        funcCallGrid()
    End Sub
    Private Sub gridView_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        lblStatus.Visible = False
    End Sub

    Private Sub txtWtTo_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If flagSave = True Then
            Exit Sub
        End If
        strSql = vbCrLf + " SELECT ISNULL(MAX(SR.TOWEIGHT),0) + .01 FROM " & cnAdminDb & "..RANGEMAST AS SR"
        txtWtFrom_Own.Text = objGPack.GetSqlValue(strSql)
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

    Private Sub frmRangeMaster_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = (Keys.F9) Then
            cmbCostCentre_Own.Focus()
            cmbSubItem_Own.Items.Add("ALL")
        End If
        If e.KeyCode = (Keys.Escape) Then
            tbMain.SelectedIndex = 0
            tbGen.Focus()
        End If
        If e.KeyCode = (Keys.F4) Then
            btnWtNext.Focus()
        End If
    End Sub


    Private Sub bnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bnSave.Click
        btnSave_Click(sender, e)
    End Sub

    Private Sub bnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bnOpen.Click
        btnOpen_Click(sender, e)
    End Sub

    Private Sub bnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bnNew.Click
        btnNew_Click(sender, e)
    End Sub

    Private Sub bnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bnExit.Click
        btnExit_Click(sender, e)
    End Sub


    Private Sub chkItemAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkItemAll.CheckedChanged
        SetChecked_CheckedList(chkListItem, chkItemAll.Checked)
    End Sub

    Private Sub chkSubItemAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSubItemAll.CheckedChanged
        SetChecked_CheckedList(chkListSubItem, chkSubItemAll.Checked)
    End Sub

    Private Sub chkCostCentreAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCostCentreAll.CheckedChanged
        SetChecked_CheckedList(chkListCostCentre, chkCostCentreAll.Checked)
    End Sub
    Private Sub Prop_Sets()
        Dim obj As New frmRangeMaster_Properties
        obj.p_chkCostCentreAll = chkCostCentreAll.Checked
        GetChecked_CheckedList(chkListCostCentre, obj.p_chkListCostCentre)
        obj.p_chkItemAll = chkItemAll.Checked
        GetChecked_CheckedList(chkListItem, obj.p_chkListItem)
        obj.p_chkSubItemAll = chkSubItemAll.Checked
        GetChecked_CheckedList(chkListSubItem, obj.p_chkListSubItem)
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmRangeMaster_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmRangeMaster_Properties))
        chkCostCentreAll.Checked = obj.p_chkCostCentreAll
        SetChecked_CheckedList(chkListCostCentre, obj.p_chkListCostCentre, cnCostName)
        chkItemAll.Checked = obj.p_chkItemAll
        SetChecked_CheckedList(chkListItem, obj.p_chkListItem, Nothing)
        chkSubItemAll.Checked = obj.p_chkSubItemAll
        SetChecked_CheckedList(chkListSubItem, obj.p_chkListSubItem, Nothing)
    End Sub

    Private Sub chkListItem_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkListItem.Leave
        If Not chkListItem.CheckedItems.Count > 0 Then
            chkItemAll.Checked = True
        End If
    End Sub

    Private Sub chkListSubItem_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkListSubItem.Leave
        If Not chkListSubItem.CheckedItems.Count > 0 Then
            chkSubItemAll.Checked = True
        End If
    End Sub

    Private Sub chkListCostCentre_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkListCostCentre.Leave
        If Not chkListCostCentre.CheckedItems.Count > 0 Then
            chkCostCentreAll.Checked = True
        End If
    End Sub

    Private Sub btnWtNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWtNext.Click
        'pnlItem.Enabled = True
        chkItemAll.Enabled = True
        If flagSave = False Then
            funcLoadItemName()
        End If
        chkItemAll.Focus()
    End Sub

    Private Sub btnItemNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnItemNext.Click
        'pnlSubItem.Enabled = True
        chkSubItemAll.Enabled = True
        If flagSave = False Then
            funcLoadSubItem()
        End If
    End Sub

    Private Sub btnSubItemNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSubItemNext.Click
        pnlCostCentre.Enabled = True
        chkCostCentreAll.Enabled = True
        If flagSave = False Then
            funcLoadCostCentre()
        End If
    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        dRow = dtRow.NewRow()
        If txtFromWt.Text <> "" And txtToWt.Text <> "" Then
            If gridViewWt.Rows.Count > 0 Then
                For i As Integer = 0 To gridViewWt.Rows.Count - 1
                    If Val(gridViewWt.Rows(i).Cells("FROMWEIGHT").Value.ToString) = Val(txtFromWt.Text) Then
                        MsgBox("Already loaded from weight", MsgBoxStyle.Information)
                        txtFromWt.Focus()
                        Exit Sub
                    End If
                    If Val(gridViewWt.Rows(i).Cells("TOWEIGHT").Value.ToString) = Val(txtToWt.Text) Then
                        MsgBox("Already loaded to weight", MsgBoxStyle.Information)
                        txtToWt.Focus()
                        Exit Sub
                    End If

                Next
            End If
            dRow("FROMWEIGHT") = txtFromWt.Text
            dRow("TOWEIGHT") = txtToWt.Text
            dRow("CAPTION") = txtCaption.Text
            dtRow.Rows.Add(dRow)
            dtRow.AcceptChanges()
            gridViewWt.DataSource = dtRow
            gridViewWt.Columns("FROMWEIGHT").Width = 100
            gridViewWt.Columns("TOWEIGHT").Width = 100
            gridViewWt.Columns("CAPTION").Width = 100
            For cnt As Integer = 0 To gridViewWt.ColumnCount - 1
                gridViewWt.Columns(cnt).HeaderText = UCase(gridViewWt.Columns(cnt).HeaderText)
            Next
            txtFromWt.Focus()
            txtFromWt.Text = gridViewWt("TOWEIGHT", gridViewWt.Rows.Count - 1).Value.ToString + 0.001
            txtToWt.Text = ""
        End If
    End Sub

    Private Sub btnCostCentreNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCostCentreNext.Click
        LoadGridView()
    End Sub

    Private Sub OpenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click
        btnOpen_Click(sender, e)
    End Sub

    Private Sub btnSearch_Own_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch_Own.Click
        strSql = " SELECT SNO,ITEMNAME, SUBITEMNAME,"
        strSql += " ISNULL((SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE AS C WHERE C.COSTID = SR.COSTID),'')AS COSTNAME,"
        strSql += " FROMWEIGHT AS FROMWEIGHT,"
        strSql += " TOWEIGHT as TOWEIGHT,CAPTION"
        strSql += " FROM " & cnAdminDb & "..RANGEMAST AS SR"
        strSql += " LEFT OUTER JOIN " & cnAdminDb & "..ITEMMAST I on I.ITEMID=SR.ITEMID"
        strSql += " LEFT OUTER JOIN " & cnAdminDb & "..SUBITEMMAST S on S.SUBITEMID=SR.SUBITEMID"
        strSql += " LEFT OUTER JOIN " & cnAdminDb & "..COSTCENTRE C on C.COSTID=SR.COSTID"
        strSql += " WHERE 1=1"
        If UCase(cmbSubItem_Own.Text) = "ALL" Then
            If cmbCostCentre_Own.Text <> "" Or cmbItem_Own.Text <> "" Or cmbSubItem_Own.Text <> "" Then
                If cmbCostCentre_Own.Text <> "" And cmbCostCentre_Own.Text <> "ALL" Then
                    strSql += " AND SR.COSTID =(SELECT COSTID FROM  " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME='" & cmbCostCentre_Own.Text & "')"
                End If
                If cmbItem_Own.Text <> "" And cmbItem_Own.Text <> "ALL" Then
                    strSql += " AND SR.ITEMID =(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbItem_Own.Text & "')"
                End If
                gridView.Columns.Clear()
                gridView.DataSource = Nothing
                funcOpenGrid(strSql, gridView)
                If gridView.Rows.Count > 0 Then
                    Dim chkcol As New DataGridViewCheckBoxColumn
                    chkcol.HeaderText = "Chk"
                    chkcol.Name = "SELECT"
                    chkcol.Width = 50
                    With gridView
                        .Columns.Insert(0, chkcol)
                        .Columns("SNO").Width = 120
                        .Columns("ITEMNAME").Width = 200
                        .Columns("SUBITEMNAME").Width = 200
                        .Columns("COSTNAME").Width = 120
                        gridView.Columns("SNO").Visible = False
                        gridView.Columns("ITEMNAME").ReadOnly = True
                        gridView.Columns("SUBITEMNAME").ReadOnly = True
                        gridView.Columns("COSTNAME").ReadOnly = True
                    End With
                End If
                gridView.Select()
                chkselectall.Focus()
            End If
            'ElseIf cmbCostSearch.Text <> "" Or (cmbItemSearch.Text <> "" And cmbSubItemSearch.Text <> "") Then
        Else
            If cmbCostCentre_Own.Text <> "" Or cmbItem_Own.Text <> "" Or cmbSubItem_Own.Text <> "" Then
                If cmbCostCentre_Own.Text <> "" And cmbCostCentre_Own.Text <> "ALL" Then
                    strSql += " AND SR.COSTID =(SELECT COSTID FROM  " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME='" & cmbCostCentre_Own.Text & "')"
                End If
                If cmbItem_Own.Text <> "" And cmbItem_Own.Text <> "ALL" Then
                    strSql += " AND SR.ITEMID =(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbItem_Own.Text & "')"
                End If
                If cmbSubItem_Own.Text <> "" And UCase(cmbSubItem_Own.Text) <> "ALL" Then
                    strSql += " AND SR.SUBITEMID =(SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME='" & cmbSubItem_Own.Text & "')"
                End If

                gridView.Columns.Clear()
                gridView.DataSource = Nothing

                funcOpenGrid(strSql, gridView)
                If gridView.Rows.Count > 0 Then
                    Dim chkcol As New DataGridViewCheckBoxColumn
                    chkcol.HeaderText = "Chk"
                    chkcol.Name = "SELECT"
                    chkcol.Width = 50
                    With gridView
                        .Columns.Insert(0, chkcol)
                        .Columns("SNO").Width = 120
                        .Columns("ITEMNAME").Width = 200
                        .Columns("SUBITEMNAME").Width = 200
                        .Columns("COSTNAME").Width = 120
                        gridView.Columns("SNO").Visible = False
                        gridView.Columns("ITEMNAME").ReadOnly = True
                        gridView.Columns("SUBITEMNAME").ReadOnly = True
                        gridView.Columns("COSTNAME").ReadOnly = True
                    End With
                End If

                gridView.Select()
                chkselectall.Focus()
            End If
        End If
    End Sub
    Private Function SearchCostCentre(ByVal combo As ComboBox) As Integer
        strSql = " Select CostName from " & cnAdminDb & "..CostCentre order by CostName"
        objGPack.FillCombo(strSql, combo, , False)
    End Function
    Private Function SearchItem(ByVal combo As ComboBox) As Integer
        cmbSubItem_Own.Items.Add("ALL")
        strSql = " Select ItemName from " & cnAdminDb & "..ItemMast order by ItemName"
        objGPack.FillCombo(strSql, combo, , False)
        cmbSubItem_Own.Items.Add("ALL")
    End Function
    Private Function SearchSubItem(ByVal combo As ComboBox) As Integer
        strSql = " Select SubItemName from " & cnAdminDb & "..SubItemMast where ItemId= (Select ItemId from " & cnAdminDb & "..ItemMast where ItemName='" & cmbItem_Own.Text & "') order by SubItemName"
        objGPack.FillCombo(strSql, combo, , False)
    End Function

    Private Sub cmbItem_Own_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbItem_Own.SelectedIndexChanged
        SearchSubItem(cmbSubItem_Own)
        cmbSubItem_Own.Items.Add("ALL")
    End Sub

    Private Sub txtToWt_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtToWt.Leave
        If txtFromWt.Text.Trim <> "" Then
            txtCaption.Text = txtFromWt.Text.Trim & "-" & txtToWt.Text.Trim
        End If
    End Sub


    Private Sub bnDelete_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bnDelete.Click
        btnDelete_Click(sender, e)
    End Sub

    Private Sub txtToWt_KeyDown(sender As Object, e As KeyEventArgs) Handles txtToWt.KeyDown
        If e.KeyCode = Keys.Enter Then
            If Val(txtToWt.Text) < Val(txtFromWt.Text) Then
                MsgBox("Invalid Range", MsgBoxStyle.Information)
                Exit Sub
            End If
        End If
    End Sub

    Private Sub chkselectall_CheckedChanged(sender As Object, e As EventArgs) Handles chkselectall.CheckedChanged
        SelectAll(sender)
    End Sub
End Class
Public Class frmRangeMaster_Properties
    Private chkCostCentreAll As Boolean = False
    Public Property p_chkCostCentreAll() As Boolean
        Get
            Return chkCostCentreAll
        End Get
        Set(ByVal value As Boolean)
            chkCostCentreAll = value
        End Set
    End Property
    Private chkListCostCentre As New List(Of String)
    Public Property p_chkListCostCentre() As List(Of String)
        Get
            Return chkListCostCentre
        End Get
        Set(ByVal value As List(Of String))
            chkListCostCentre = value
        End Set
    End Property
    Private chkItemAll As Boolean = False
    Public Property p_chkItemAll() As Boolean
        Get
            Return chkItemAll
        End Get
        Set(ByVal value As Boolean)
            chkItemAll = value
        End Set
    End Property
    Private chkListItem As New List(Of String)
    Public Property p_chkListItem() As List(Of String)
        Get
            Return chkListItem
        End Get
        Set(ByVal value As List(Of String))
            chkListItem = value
        End Set
    End Property
    
    Private chkSubItemAll As Boolean = False
    Public Property p_chkSubItemAll() As Boolean
        Get
            Return chkSubItemAll
        End Get
        Set(ByVal value As Boolean)
            chkSubItemAll = value
        End Set
    End Property
    Private chkListSubItem As New List(Of String)
    Public Property p_chkListSubItem() As List(Of String)
        Get
            Return chkListSubItem
        End Get
        Set(ByVal value As List(Of String))
            chkListSubItem = value
        End Set
    End Property
End Class
