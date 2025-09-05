Imports System.Data.OleDb
Public Class frmPurchaseOrderReceiptReject
#Region "Variable"
    Dim strsql As String
    Dim dtAdd As New DataTable
    Dim dt As DataTable
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Dim systemName As String = Environment.MachineName
    Dim flagupdate As Boolean = False
    Dim _tran As OleDbTransaction = Nothing
    Dim Batchno As String = ""
    Dim SNO As String = ""
    Dim updateSNO As String = ""
#End Region

#Region "Form Load"
    Private Sub frmPurchaseOrderReceiptReject_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        ElseIf e.KeyCode = Keys.Escape Then
            tabGeneral.SelectedTab = tabMain
        End If
    End Sub

    Private Sub frmPurchaseOrderReceiptReject_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        tabGeneral.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabGeneral.Region = New Region(New RectangleF(Me.tabMain.Left, Me.tabMain.Top, Me.tabMain.Width, Me.tabMain.Height))
        btnNew_Click(Me, New System.EventArgs)
    End Sub
#End Region

#Region "Gridview Events "
    Private Sub gridViewEdit_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridViewEdit.KeyDown
        If e.KeyCode = Keys.Enter Then
            ' If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Edit) Then Exit Sub
            Dim curTableName As String
            Dim ptype As Boolean = False
            With gridViewEdit
                If .CurrentRow.Cells("TYPE").Value.ToString = "PURCHASEORDER" Then
                ElseIf .CurrentRow.Cells("TYPE").Value.ToString = "PURCHASERECEIPT" Then
                ElseIf .CurrentRow.Cells("TYPE").Value.ToString = "PURCHASEREJECT" Then
                End If
                dtpDate.Value = .CurrentRow.Cells("TRANDATE").Value
                cmbDesigner_Man.Text = .CurrentRow.Cells("DESIGNERNAME").Value
                cmbCostCentre_Man.Text = .CurrentRow.Cells("COSTNAME").Value.ToString
                txtItemCode_Num_Man.Text = .CurrentRow.Cells("ITEMID").Value
                txtItemName.Text = .CurrentRow.Cells("ITEMNAME").Value
                cmbSubItemName_Man.Text = .CurrentRow.Cells("SUBITEMNAME").Value
                txtDiamondPieces_Num.Text = .CurrentRow.Cells("PCS").Value
                txtDidmondWeight_Wet.Text = .CurrentRow.Cells("GRSWT").Value
                txtNetWeight_Wet.Text = .CurrentRow.Cells("NETWT").Value
                updateSNO = .CurrentRow.Cells("SNO").Value
                flagupdate = True
                tabGeneral.SelectedTab = tabMain
            End With
        ElseIf e.KeyCode = Keys.D Then
            With gridViewEdit
                Dim BillPrintType As String = ""
                If .CurrentRow.Cells("TYPE").Value.ToString = "PURCHASEORDER" Then
                    BillPrintType = "PPO"
                ElseIf .CurrentRow.Cells("TYPE").Value.ToString = "PURCHASERECEIPT" Then
                    BillPrintType = "PRE"
                ElseIf .CurrentRow.Cells("TYPE").Value.ToString = "PURCHASEREJECT" Then
                    BillPrintType = "PRJ"
                End If

                If IO.File.Exists(Application.StartupPath & "\BillPrint.exe") Then
                    Dim prnmemsuffix As String = ""
                    If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId
                    Dim memfile As String = "\BillPrint" + prnmemsuffix.Trim & ".mem"

                    Dim write As IO.StreamWriter
                    write = IO.File.CreateText(Application.StartupPath & memfile)
                    write.WriteLine(LSet("TYPE", 15) & ":" & BillPrintType & "")
                    write.WriteLine(LSet("BATCHNO", 15) & ":" & .CurrentRow.Cells("BATCHNO").Value.ToString)
                    write.WriteLine(LSet("TRANDATE", 15) & ":" & dtpDate.Value.Date.ToString("yyyy-MM-dd"))
                    write.WriteLine(LSet("DUPLICATE", 15) & ":Y")
                    write.Flush()
                    write.Close()
                    If EXE_WITH_PARAM = False Then
                        System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe")
                    Else
                        System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe",
                        LSet("TYPE", 15) & ":" & BillPrintType & ";" &
                        LSet("BATCHNO", 15) & ":" & .CurrentRow.Cells("BATCHNO").Value.ToString & ";" &
                        LSet("TRANDATE", 15) & ":" & dtpDate.Value.Date.ToString("yyyy-MM-dd") & ";" &
                        LSet("DUPLICATE", 15) & ":Y")
                    End If
                Else
                    MsgBox("Billprint exe not found", MsgBoxStyle.Information)
                End If
            End With
        End If
    End Sub
#End Region

#Region "Button Event 2 "
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabGeneral.SelectedTab = tabMain
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        gridViewEdit.DataSource = Nothing
        If cmbPurchaseType_Man.Text = "Order" Then
            purchaseSearch("PURCHASEORDER", "PURCHASEORDER")
        ElseIf cmbPurchaseType_Man.Text = "Receipt" Then
            purchaseSearch("PURCHASERECEIPT", "PURCHASERECEIPT")
        ElseIf cmbPurchaseType_Man.Text = "Reject" Then
            purchaseSearch("PURCHASEREJECT", "PURCHASEREJECT")
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Cancel) Then Exit Sub
        If gridViewEdit.Rows.Count = 0 Then Exit Sub
        With gridViewEdit
            Try
                _tran = Nothing
                _tran = cn.BeginTransaction
                Dim CurTableName As String = .CurrentRow.Cells("TYPE").Value.ToString
                Dim currentSno As String = .CurrentRow.Cells("SNO").Value.ToString
                strsql = " UPDATE " & cnStockDb & ".." & CurTableName & " "
                strsql += vbCrLf + "  CANCEL = 'Y'"
                strsql += vbCrLf + " , CDATE = '" & Now & "'"
                strsql += vbCrLf + "  WHERE SNO = '" & currentSno & "'"
                cmd = New OleDbCommand(strsql, cn, _tran)
                cmd.ExecuteNonQuery()
                _tran.Commit()
                _tran = Nothing
            Catch ex As Exception
                If Not _tran Is Nothing Then
                    _tran.Rollback()
                    _tran = Nothing
                    MessageBox.Show(ex.ToString)
                    Exit Sub
                Else
                    MessageBox.Show(ex.ToString)
                    Exit Sub
                End If
            End Try
        End With
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
        If gridViewEdit.Rows.Count = 0 Then Exit Sub
    End Sub
#End Region

#Region "Button Events"



    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        objGPack.TextClear(Me)
        dtpDate.Value = GetServerDate()
        funcCostCentre()
        funcDesigner()
        gridView.DataSource = Nothing
        dtAdd = New DataTable
        dtAdd = GetTable()
        flagupdate = False
        Batchno = ""
        Lblfalse()
        SNO = ""
        rbtPurchaseOrder.Enabled = True
        rbtPurchaseReject.Enabled = True
        rbtPurchaseReceipt.Enabled = True

        cmbPurchaseType_Man.Items.Clear()
        cmbPurchaseType_Man.Items.Add("Order")
        cmbPurchaseType_Man.Items.Add("Receipt")
        cmbPurchaseType_Man.Items.Add("Reject")
        cmbPurchaseType_Man.Text = "Order"
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        updateSNO = ""

    End Sub

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click, OpenToolStripMenuItem.Click
        tabGeneral.SelectedTab = tabOpen
        gridViewEdit.DataSource = Nothing
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click, SaveToolStripMenuItem.Click
        ' If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
        If gridView.RowCount = 0 Then Exit Sub
        If rbtPurchaseOrder.Checked = True Then
            If flagupdate = True Then
            Else
                POInsert("GEN-P-PUORDER", "PURCHASEORDER", TranSnoType.PURCHASEORDER, "P.Order ")
            End If
        ElseIf rbtPurchaseReceipt.Checked = True Then
            If flagupdate = True Then
            Else
                POInsert("GEN-P-PURECEIPT", "PURCHASERECEIPT", TranSnoType.PURCHASERECEIPT, "P.Receipt ")
            End If
        ElseIf rbtPurchaseReject.Checked = True Then
            If flagupdate = True Then
            Else
                POInsert("GEN-P-PUREJECT", "PURCHASEREJECT", TranSnoType.PURCHASEREJECT, "P.Reject ")
            End If
        End If
    End Sub

    Private Sub POInsert(ByVal ctlname As String, ByVal tablename As String, ByVal snoname As String, ByVal MsgTitle As String)
        Try
            _tran = Nothing
            _tran = cn.BeginTransaction
            Dim dtPo As New DataTable
            Dim DesignerId As Integer = 0
            Dim Costid As String = ""
            Dim itemId As Integer = 0
            Dim subitemId As Integer = 0
            Dim PurchaseOrderNo As Integer = 0
            dtPo = gridView.DataSource
            For i As Integer = 0 To dtPo.Rows.Count - 1
                With dtPo.Rows(i)
GENPONO:
                    strsql = " SELECT CTLTEXT FROM " & cnStockDb & "..TBILLCONTROL WHERE CTLID = '" & ctlname & "'"
                    PurchaseOrderNo = Val(objGPack.GetSqlValue(strsql, , , _tran))
                    strsql = " UPDATE " & cnStockDb & "..TBILLCONTROL SET CTLTEXT = '" & PurchaseOrderNo + 1 & "' "
                    strsql += " WHERE CTLID = '" & ctlname & "' AND CTLTEXT = " & PurchaseOrderNo & ""
                    cmd = New OleDbCommand(strsql, cn, _tran)
                    If cmd.ExecuteNonQuery() = 0 Then
                        GoTo GENPONO
                    End If
                    PurchaseOrderNo += 1

                    strsql = " SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & .Item("DESIGNERNAME") & "'"
                    DesignerId = objGPack.GetSqlValue(strsql, , , _tran)

                    strsql = "SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & .Item("COSTNAME") & "'"
                    Costid = objGPack.GetSqlValue(strsql, , , _tran)


                    strsql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Item("ITEMNAME") & "'"
                    itemId = Val(objGPack.GetSqlValue(strsql, , , _tran))

                    strsql = " SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST "
                    strsql += " WHERE SUBITEMNAME = '" & .Item("SUBITEMNAME") & "' and ITEMID = " & itemId & ""
                    subitemId = Val(objGPack.GetSqlValue(strsql, , , _tran))

                    

                    Batchno = GetNewBatchno(cnCostId, dtpDate.Value.Date, _tran)
                    Dim NewSno As String = GetNewSno(snoname, _tran, "GET_TRANSNO_TRAN")

                    If Batchno = "" Then
                        MsgBox("Batchno is Empty")
                        If Not _tran Is Nothing Then _tran.Rollback() : _tran = Nothing
                        Exit Sub
                    End If
                    strsql = vbCrLf + " INSERT INTO " & cnStockDb & ".." & tablename & "(SNO,COMPANYID"
                    strsql += vbCrLf + " ,TRANDATE,TRANNO,BATCHNO "
                    strsql += vbCrLf + " ,REFDATE,COSTID"
                    strsql += vbCrLf + " ,DESIGNERID,ITEMID,SUBITEMID "
                    strsql += vbCrLf + " ,PCS"
                    strsql += vbCrLf + " ,GRSWT,NETWT,PUREWT"
                    strsql += vbCrLf + " ,CDATE "
                    strsql += vbCrLf + " ,CANCEL,CREMARKS"
                    strsql += vbCrLf + " ,USERID "
                    strsql += vbCrLf + " ,EMPID,SYSTEMID,APPVER "
                    strsql += vbCrLf + " ,UPDATED,UPTIME"
                    strsql += vbCrLf + " ) "
                    strsql += vbCrLf + " VALUES"
                    strsql += vbCrLf + " ( "
                    strsql += vbCrLf + " '" & NewSno & "' " 'SNO
                    strsql += vbCrLf + " ,'" & strCompanyId & "'" 'COMPANYID
                    strsql += vbCrLf + " ,'" & Format(.Item("BILLDATE"), "yyyy-MM-dd") & "'" 'TRANDATE
                    strsql += vbCrLf + " ," & PurchaseOrderNo & " " 'TRANNO
                    strsql += vbCrLf + " ,'" & Batchno & "'" 'BATCHNO
                    strsql += vbCrLf + " ,NULL " 'REFDATE
                    strsql += vbCrLf + " ,'" & Costid & "' " 'COSTID
                    strsql += vbCrLf + " ," & DesignerId & " " 'DEIGNERID
                    strsql += vbCrLf + " ," & itemId & " " 'ITEMID
                    strsql += vbCrLf + " ," & subitemId & " " 'SUBITEMID
                    strsql += vbCrLf + " ," & Val(.Item("QTY").ToString) & " " 'PCS
                    strsql += vbCrLf + " ," & Val(.Item("GRSWT").ToString) & " " 'GRSWT
                    strsql += vbCrLf + " ," & Val(.Item("NETWT").ToString) & " " 'NETWT
                    strsql += vbCrLf + " ,0 " 'PUREWT
                    strsql += vbCrLf + " ,NULL,'','' " 'CDATE,CANCEL,CREMARKS
                    strsql += vbCrLf + " ," & userId & " " 'USERID
                    strsql += vbCrLf + " ,0 " 'EMPID
                    strsql += vbCrLf + " ,'" & systemId & "' " 'SYSTEMID
                    strsql += vbCrLf + " ,'" & VERSION & "' " 'APPVER
                    strsql += vbCrLf + " ,'" & Format(Now.Date, "yyyy-MM-dd") & "' "
                    strsql += vbCrLf + " , '" & Date.Now.ToLongTimeString & "' "
                    strsql += vbCrLf + " ) "
                    cmd = New OleDbCommand(strsql, cn, _tran)
                    cmd.ExecuteNonQuery()
                    If rbtPurchaseReceipt.Checked = True Then
                        strsql = " UPDATE  " & cnStockDb & "..PURCHASEORDER "
                        strsql += vbCrLf + " SET CPCS = ISNULL(CPCS,0) + " & Val(.Item("QTY").ToString) & " "
                        strsql += vbCrLf + " ,CGRSWT = ISNULL(CGRSWT,0) + " & Val(.Item("GRSWT").ToString) & " "
                        strsql += vbCrLf + " ,CNETWT = ISNULL(CNETWT,0) + " & Val(.Item("NETWT").ToString) & " "
                        strsql += vbCrLf + " ,CSNO = '" & NewSno & "' "
                        strsql += vbCrLf + " WHERE SNO = '" & .Item("CSNO").ToString & "' "
                        cmd = New OleDbCommand(strsql, cn, _tran)
                        cmd.ExecuteNonQuery()
                    End If
                End With
            Next
            _tran.Commit()
            _tran = Nothing

            Dim BillPrintType As String = ""
            If rbtPurchaseOrder.Checked = True Then
                BillPrintType = "PPO"
            ElseIf rbtPurchaseReceipt.Checked = True Then
                BillPrintType = "PRE"
            ElseIf rbtPurchaseReject.Checked = True Then
                BillPrintType = "PRJ"
            End If

            If IO.File.Exists(Application.StartupPath & "\BillPrint.exe") Then
                Dim prnmemsuffix As String = ""
                If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId
                Dim memfile As String = "\BillPrint" + prnmemsuffix.Trim & ".mem"

                Dim write As IO.StreamWriter
                write = IO.File.CreateText(Application.StartupPath & memfile)
                write.WriteLine(LSet("TYPE", 15) & ":" & BillPrintType & "")
                write.WriteLine(LSet("BATCHNO", 15) & ":" & Batchno)
                write.WriteLine(LSet("TRANDATE", 15) & ":" & dtpDate.Value.Date.ToString("yyyy-MM-dd"))
                write.WriteLine(LSet("DUPLICATE", 15) & ":N")
                write.Flush()
                write.Close()
                If EXE_WITH_PARAM = False Then
                    System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe")
                Else
                    System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe",
                    LSet("TYPE", 15) & ":" & BillPrintType & ";" &
                    LSet("BATCHNO", 15) & ":" & Batchno & ";" &
                    LSet("TRANDATE", 15) & ":" & dtpDate.Value.Date.ToString("yyyy-MM-dd") & ";" &
                    LSet("DUPLICATE", 15) & ":N")
                End If
            Else
                MsgBox("Billprint exe not found", MsgBoxStyle.Information)
            End If



            btnNew_Click(Me, New System.EventArgs)
            MsgBox(MsgTitle & " Generated")
            dtpDate.Focus()
        Catch ex As Exception
            If Not _tran Is Nothing Then
                _tran.Rollback()
                _tran = Nothing
                MessageBox.Show(ex.ToString)
                Exit Sub
            Else
                MessageBox.Show(ex.ToString)
                Exit Sub
            End If
        End Try
    End Sub

    Private Sub POUpdate(ByVal tablename As String)


    End Sub


    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        If LoadGrid() = False Then
            Exit Sub
        End If
        If dtAdd.Rows.Count > 0 Then
            Dim r() As DataRow = dtAdd.Select("ITEMNAME = '" & txtItemName.Text & "' AND SUBITEMNAME = '" & cmbSubItemName_Man.Text & "'")
            If r.Length > 0 Then
                MsgBox("Already ItemName And SubitemName Loaded... !", MsgBoxStyle.Information)
                txtItemCode_Num_Man.Focus()
                txtItemCode_Num_Man.SelectAll()
                Exit Sub
            End If
        End If
        Dim row As DataRow = Nothing
        row = dtAdd.NewRow
        With row
            .Item("BILLDATE") = dtpDate.Value.Date
            .Item("DESIGNERNAME") = cmbDesigner_Man.Text
            .Item("COSTNAME") = cmbCostCentre_Man.Text
            .Item("ITEMNAME") = txtItemName.Text
            .Item("SUBITEMNAME") = cmbSubItemName_Man.Text
            .Item("QTY") = Val(txtDiamondPieces_Num.Text)
            .Item("GRSWT") = Val(txtDidmondWeight_Wet.Text)
            .Item("NETWT") = Val(txtNetWeight_Wet.Text)
            .Item("CSNO") = SNO
        End With
        dtAdd.Rows.Add(row)
        gridView.DataSource = Nothing
        gridView.DataSource = dtAdd
        FormatGridColumns(gridView, False, True, False, False)
        AutoSize1()
        If gridView.RowCount > 0 Then btnSave.Enabled = True
        objGPack.TextClear(Me)
        dtpDate.Focus()
    End Sub
#End Region

#Region "User Define"

    Private Sub purchaseSearch(ByVal tablename As String, ByVal type As String)

        strsql = vbCrLf + " SELECT PO.SNO,PO.TRANDATE "
        strsql += vbCrLf + " ,PO.DESIGNERID "
        strsql += vbCrLf + " ,DE.DESIGNERNAME "
        strsql += vbCrLf + " ,PO.COSTID"
        strsql += vbCrLf + " ,CC.COSTNAME"
        strsql += vbCrLf + " ,PO.ITEMID "
        strsql += vbCrLf + " ,IM.ITEMNAME "
        strsql += vbCrLf + " ,PO.SUBITEMID "
        strsql += vbCrLf + " ,SIM.SUBITEMNAME "
        strsql += vbCrLf + " ,PO.PCS "
        strsql += vbCrLf + " ,PO.GRSWT "
        strsql += vbCrLf + " ,PO.NETWT "
        strsql += vbCrLf + " ,'" & type & "' [TYPE]"
        strsql += vbCrLf + " ,PO.BATCHNO "
        strsql += vbCrLf + " FROM " & cnStockDb & ".." & tablename & "	AS PO "
        strsql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST IM ON PO.ITEMID = IM.ITEMID "
        strsql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SIM ON SIM.ITEMID = IM.ITEMID AND PO.SUBITEMID = SIM.SUBITEMID "
        strsql += vbCrLf + " INNER JOIN " & cnAdminDb & "..DESIGNER AS DE ON DE.DESIGNERID = PO.DESIGNERID "
        strsql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..COSTCENTRE AS CC ON CC.COSTID = PO.COSTID "
        strsql += vbCrLf + " WHERE "
        strsql += vbCrLf + " TRANDATE BETWEEN '" & Format(dtpFrom.Value.Date, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value.Date, "yyyy-MM-dd") & "' "
        strsql += vbCrLf + " AND ISNULL(CANCEL,'') = '' "
        If tablename <> "PURCHASEREJECT" Then strsql += vbCrLf + " AND ISNULL(CSNO,'') = '' "
        dt = New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            With gridViewEdit
                .DataSource = Nothing
                .DataSource = dt
                FormatGridColumns(gridViewEdit, False, True, False, False)
                AutoSize2()
            End With
        Else
            MsgBox("No Record Found", MsgBoxStyle.Information)
            gridViewEdit.DataSource = Nothing
            Exit Sub
        End If
    End Sub

    Private Sub AutoSize2()
        gridViewEdit.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
        gridViewEdit.Invalidate()
        For Each dgvCol As DataGridViewColumn In gridViewEdit.Columns
            dgvCol.Width = dgvCol.Width
        Next
        gridViewEdit.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        For cnt As Integer = 0 To gridViewEdit.ColumnCount - 1
            gridViewEdit.Columns(cnt).SortMode = DataGridViewColumnSortMode.NotSortable
        Next
    End Sub
    Private Sub AutoSize1()
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
        gridView.Invalidate()
        For Each dgvCol As DataGridViewColumn In gridView.Columns
            dgvCol.Width = dgvCol.Width
        Next
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        For cnt As Integer = 0 To gridView.ColumnCount - 1
            gridView.Columns(cnt).SortMode = DataGridViewColumnSortMode.NotSortable
        Next
    End Sub

    Private Sub purchaseDetail(ByVal tablename As String, ByVal Title As String)
        Dim dr As DataRow = Nothing
        strsql = " SELECT * FROM " & cnStockDb & ".." & tablename & " WHERE "
        strsql += vbCrLf + " ISNULL(CANCEL,'') = '' "
        strsql += vbCrLf + " AND ISNULL(CSNO,'') = ''"
        SNO = BrighttechPack.SearchDialog.Show("Searching " & Title, strsql, cn, 1, 0)
        strsql = " SELECT * FROM " & cnStockDb & ".." & tablename & " WHERE "
        strsql += vbCrLf + " SNO = '" & SNO & "' "
        strsql += vbCrLf + " AND ISNULL(CANCEL,'') = '' "
        strsql += vbCrLf + " AND ISNULL(CSNO,'') = '' "
        dr = GetSqlRow(strsql, cn)
        Dim designId As Integer = 0
        Dim costid As String = ""
        If Not dr Is Nothing Then
            dtpDate.Text = ""
            'TRANO
            txt_Num_Man.Text = dr.Item("TRANNO").ToString
            'DESIGNERNAME
            designId = dr.Item("DESIGNERID").ToString
            strsql = " SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = " & designId & ""
            cmbDesigner_Man.Text = GetSqlValue(cn, strsql)
            'COSTID
            costid = dr.Item("COSTID").ToString
            strsql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = '" & costid & "'"
            cmbCostCentre_Man.Text = GetSqlValue(cn, strsql)
            'ITEMID
            txtItemCode_Num_Man.Text = Val(dr.Item("ITEMID").ToString)
            strsql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & Val(txtItemCode_Num_Man.Text) & ""
            txtItemName.Text = GetSqlValue(cn, strsql)
            'SUBITEMNAME
            strsql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = " & Val(txtItemCode_Num_Man.Text) & ""
            strsql += " AND SUBITEMID = '" & dr.Item("SUBITEMID").ToString & "'"
            cmbSubItemName_Man.Text = GetSqlValue(cn, strsql)
            'VALUES
            txtDiamondPieces_Num.Text = dr.Item("PCS").ToString
            txtDidmondWeight_Wet.Text = dr.Item("GRSWT").ToString
            txtNetWeight_Wet.Text = dr.Item("NETWT").ToString
        Else
            MsgBox("No Record Found", MsgBoxStyle.Information)
            btnNew_Click(Me, New System.EventArgs)
            Exit Sub
        End If
    End Sub

    Function LoadGrid() As Boolean
        ''If (cmbSubItemName_Man.Text = "ALL" Or cmbSubItemName_Man.Text = "") And cmbSubItemName_Man.Enabled = True Then
        ''    MsgBox("Invalid SubitemName", MsgBoxStyle.Information)
        ''    cmbSubItemName_Man.Focus()
        ''    cmbSubItemName_Man.SelectAll()
        ''    Return False
        ''End If

        strsql = " SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & cmbDesigner_Man.Text & "'"
        If objGPack.GetSqlValue(strsql) = "" Then
            MsgBox("Invalid Supplier", MsgBoxStyle.Information)
            cmbDesigner_Man.Select()
            Return False
        End If
        If Val(txtDidmondWeight_Wet.Text) <> 0 And Val(txtNetWeight_Wet.Text) > Val(txtDidmondWeight_Wet.Text) Then
            MsgBox("NetWeight Should not Exceed " + txtDidmondWeight_Wet.Text, MsgBoxStyle.Information)
            txtNetWeight_Wet.Focus()
            txtNetWeight_Wet.SelectAll()
            Return False
        End If

        If Val(txtDiamondPieces_Num.Text) = 0 Then
            MsgBox("Pieces is Empty", MsgBoxStyle.Information)
            txtDiamondPieces_Num.Focus()
            txtDiamondPieces_Num.Select()
            Return False
        End If

        If Val(txtDidmondWeight_Wet.Text) = 0 Then
            MsgBox("Gross Weight is Empty", MsgBoxStyle.Information)
            txtDidmondWeight_Wet.Focus()
            txtDidmondWeight_Wet.SelectAll()
            Return False
        End If

        If Val(txtNetWeight_Wet.Text) = 0 Then
            MsgBox("Net Weight is Empty", MsgBoxStyle.Information)
            txtNetWeight_Wet.Focus()
            txtNetWeight_Wet.SelectAll()
            Return False
        End If

        If Val(txtDidmondWeight_Wet.Text) = 0 And Val(txtNetWeight_Wet.Text) = 0 Then
            MsgBox("Gross Weight and Net Weight is Empty", MsgBoxStyle.Information)
            txtDidmondWeight_Wet.Focus()
            txtDidmondWeight_Wet.SelectAll()
            Return False
        End If
        Return True
    End Function

    Function funcCostCentre() As Integer
        cmbCostCentre_Man.Items.Clear()
        strsql = " select CostName from " & cnAdminDb & "..CostCentre "
        strsql += " order by CostName"
        objGPack.FillCombo(strsql, cmbCostCentre_Man, False)
        cmbCostCentre_Man.Text = ""
    End Function

    Function funcDesigner() As Integer
        cmbDesigner_Man.Items.Clear()
        strsql = " select DESIGNERNAME from " & cnAdminDb & "..DESIGNER "
        strsql += " order by DESIGNERNAME"
        objGPack.FillCombo(strsql, cmbDesigner_Man, False)
        cmbDesigner_Man.Text = ""
    End Function

    Function GetTable() As DataTable
        Dim table As New DataTable
        table.Columns.Add("SNO", GetType(Integer))
        table.Columns.Add("BILLDATE", GetType(DateTime))
        table.Columns.Add("DESIGNERNAME", GetType(String))
        table.Columns.Add("COSTNAME", GetType(String))
        table.Columns.Add("ITEMNAME", GetType(String))
        table.Columns.Add("SUBITEMNAME", GetType(String))
        table.Columns.Add("QTY", GetType(Integer))
        table.Columns.Add("GRSWT", GetType(Decimal))
        table.Columns.Add("NETWT", GetType(Decimal))
        table.Columns.Add("CSNO", GetType(String))
        table.Columns("SNO").AutoIncrement = True
        table.Columns("SNO").AutoIncrementSeed = 1
        table.Columns("SNO").AutoIncrementStep = 1
        Return table
    End Function

#End Region

#Region "Textbox Events"


    Private Sub txtItemCode_Num_Man_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtItemCode_Num_Man.KeyDown
        If e.KeyCode = Keys.Insert Then  ' Insert Key
            strsql = " SELECT ITEMID,ITEMNAME,"
            strsql += " CASE WHEN STOCKTYPE = 'T' THEN 'TAGED' "
            strsql += " WHEN STOCKTYPE = 'N' THEN 'NON TAGED' ELSE 'PACKET BASED' END AS STOCKTYPE,"
            strsql += " CASE WHEN SUBITEM = 'Y' THEN 'YES' ELSE 'NO' END AS SUBITEM, "
            strsql += " CASE WHEN CALTYPE = 'W' THEN 'WEIGHT'"
            strsql += " WHEN CALTYPE = 'R' THEN 'RATE'"
            strsql += " WHEN CALTYPE = 'F' THEN 'FIXED'"
            strsql += " WHEN CALTYPE = 'B' THEN 'BOTH'"
            strsql += " WHEN CALTYPE = 'M' THEN 'METAL RATE' END AS CALTYPE"
            strsql += " FROM " & cnAdminDb & "..ITEMMAST"
            strsql += " WHERE ITEMID LIKE '" & txtItemCode_Num_Man.Text & "%'"
            strsql += " AND ACTIVE = 'Y'"
            strsql += " AND STUDDED <> 'S'"
            strsql += GetItemQryFilteration("S")
            strsql += " ORDER BY ITEMNAME"
            txtItemCode_Num_Man.Text = BrighttechPack.SearchDialog.Show("Search ItemId", strsql, cn)
        End If
    End Sub

    Private Sub txtItemCode_Num_Man_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtItemCode_Num_Man.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then 'Enter Key
            If txtItemCode_Num_Man.Text = "" Then
                Exit Sub
            End If
            Dim itemName As String = objGPack.GetSqlValue(" select ItemName from " & cnAdminDb & "..ItemMast where ItemId = '" & txtItemCode_Num_Man.Text & "' AND ACTIVE = 'Y' AND STUDDED <> 'S'" & GetItemQryFilteration("S"), "itemname")
            txtItemName.Text = itemName
            ''SUBITEMSETTING
            strsql = GetSubItemQry(New String() {"SUBITEMNAME"}, Val(txtItemCode_Num_Man.Text))
            cmbSubItemName_Man.Items.Clear()
            cmbSubItemName_Man.Items.Add("ALL")
            objGPack.FillCombo(strsql, cmbSubItemName_Man, False)
            cmbSubItemName_Man.Text = "ALL"
            If Not cmbSubItemName_Man.Items.Count > 1 Then
                cmbSubItemName_Man.Enabled = False
            Else
                cmbSubItemName_Man.Enabled = True
            End If
        End If
    End Sub

    Private Sub txt_Num_Man_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Num_Man.KeyDown
        If e.KeyCode = Keys.Insert Then
            If rbtPurchaseReceipt.Checked = True Then
                purchaseDetail("PURCHASEORDER", "Purchase Order")
            ElseIf rbtPurchaseReject.Checked = True Then
                purchaseDetail("PURCHASERECEIPT", "Purchase Receipt")
            End If
        End If
    End Sub
#End Region

#Region "Radio Button Events"
    Private Sub rbtPurchaseReject_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtPurchaseReject.CheckedChanged
        If rbtPurchaseReject.Checked = True Then
            txt_Num_Man.Text = ""
            Label4.Visible = True
            Label4.Text = "PO RECNO."
            txt_Num_Man.Visible = True
            rbtPurchaseOrder.Enabled = False
            rbtPurchaseReceipt.Enabled = False
        End If
    End Sub

    Private Sub rbtPurchaseReceipt_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtPurchaseReceipt.CheckedChanged
        If rbtPurchaseReceipt.Checked Then
            txt_Num_Man.Text = ""
            Label4.Visible = True
            Label4.Text = "PO NO."
            txt_Num_Man.Visible = True
            rbtPurchaseOrder.Enabled = False
            rbtPurchaseReject.Enabled = False
        End If
    End Sub

    Private Sub rbtPurchaseOrder_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtPurchaseOrder.CheckedChanged
        If rbtPurchaseOrder.Checked = True Then
            Lblfalse()
            rbtPurchaseReceipt.Enabled = False
            rbtPurchaseReject.Enabled = False
        End If
    End Sub

    Private Sub Lblfalse()
        If rbtPurchaseOrder.Checked = True Then
            Label4.Text = "..."
            Label4.Visible = False
            txt_Num_Man.Visible = False
            txt_Num_Man.Text = ""
        End If
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        If gridViewEdit.Rows.Count > 0 Then
            Dim title As String
            title = "PURCHASE " + cmbPurchaseType_Man.Text.ToString.ToUpper + " REPORT"
            title += vbCrLf + " DATE FROM " + dtpFrom.Text.ToString + " TO " + dtpTo.Text.ToString
            BrightPosting.GExport.Post(Me.Name, strCompanyName, title, gridViewEdit, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub btnExcel_Click(sender As Object, e As EventArgs) Handles btnExcel.Click
        If gridViewEdit.Rows.Count > 0 Then
            Dim title As String
            title = "PURCHASE " + cmbPurchaseType_Man.Text.ToString.ToUpper + " REPORT"
            title += vbCrLf + " DATE FROM " + dtpFrom.Text.ToString + " TO " + dtpTo.Text.ToString
            BrightPosting.GExport.Post(Me.Name, strCompanyName, title, gridViewEdit, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub
#End Region

End Class