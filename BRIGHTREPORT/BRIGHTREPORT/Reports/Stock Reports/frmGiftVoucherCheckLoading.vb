Imports System.Data.OleDb
Imports System.IO
Public Class frmGiftVoucherCheckLoading
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim dtFullView As New DataTable
    Dim dtLastView As New DataTable
    Dim defaultPic As String = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'PICPATH'")

    Private WithEvents btnExcelFullView As New Button
    Private WithEvents btnPrintFullView As New Button
    Private WithEvents btnExcelStoneDetails As New Button
    Private WithEvents btnPrintStoneDetails As New Button
    Dim PRODTAGSEP As String = GetAdmindbSoftValue("PRODTAGSEP")
    Dim chkwttotal As Boolean = IIf(GetAdmindbSoftValue("STKCHKREP_TOTWT", "N") = "Y", True, False)
    Dim chkimportdataarry() As String = GetAdmindbSoftValue("STOCK_IMPORT_DATA", "N///,").ToString.Split("/")
    Dim BARCODE2DSEP As String = GetAdmindbSoftValue("BARCODE2DSEP")
    Dim chkpcstotal As Boolean = True
    Dim Authorize As Boolean = False
    Function funcAddNodeListTotal(ByVal desc As String, ByVal Tag As Integer, ByVal Pcs As Integer, ByVal grsWt As Double, ByVal netWt As Double) As Integer
        Dim node As New ListViewItem(desc)
        node.SubItems.Add(Tag)
        node.SubItems.Add(Pcs)
        node.SubItems.Add(grsWt)
        node.SubItems.Add(netWt)

    End Function

    Function funcLastViewStyle(ByVal grid As DataGridView) As Integer
        With grid
            With .Columns("TRANDATE")
                .HeaderText = "TRANDATE"
                .Width = 90
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Resizable = DataGridViewTriState.False
            End With
            With .Columns("RUNNO")
                .HeaderText = "RUNNO"
                .Width = 100
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Resizable = DataGridViewTriState.False
            End With
            With .Columns("QTY")
                .HeaderText = "QTY"
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Resizable = DataGridViewTriState.False
            End With
            With .Columns("AMOUNT")
                .HeaderText = "AMOUNT"
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Resizable = DataGridViewTriState.False
            End With
            With .Columns("BATCHNO")
                .HeaderText = "BATCH NO"
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Resizable = DataGridViewTriState.False
            End With
            With .Columns("COSTID")
                .HeaderText = "COSTID"
                .Width = 150
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Resizable = DataGridViewTriState.False
            End With

            'With .Columns("subItemName")
            '    .HeaderText = "SUBITEM"
            '    .Width = 130
            '    .SortMode = DataGridViewColumnSortMode.NotSortable
            '    .Resizable = DataGridViewTriState.False
            'End With
            'With .Columns("CounterName")
            '    .HeaderText = "COUNTER"
            '    .Width = 120
            '    .SortMode = DataGridViewColumnSortMode.NotSortable
            '    .Resizable = DataGridViewTriState.False
            'End With
            'With .Columns("chkTray")
            '    .Visible = False
            'End With
            'With .Columns("Approval")
            '    .Visible = False
            'End With
            'With .Columns("picFileName")
            '    .Visible = False
            'End With
            'With .Columns("RESULT")
            '    .Visible = False
            'End With
        End With
    End Function

    

    

    'Private Sub frmStockCheckLoading_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
    '    If cmbCostCentre_MAN.Enabled Then
    '        cmbCostCentre_MAN.Select()
    '    Else

    '    End If

    'End Sub

    Private Sub frmStockCheckLoading_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmStockCheckLoading_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not defaultPic.EndsWith("\") Then defaultPic += "\"
        Authorize = BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Authorize, False)
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        Dim dtItemDet As New DataTable
        If txtRunno.Text <> "" Then
            Dim ScanStr As String = txtRunno.Text
            strSql = " SELECT TRANDATE, RUNNO, QTY, AMOUNT, BATCHNO, COSTID, CHECKED FROM " & cnStockDb & "..GVTRAN WHERE RUNNO= '" + txtRunno.Text + "'"
            dtItemDet.Clear()
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtItemDet)
            If dtItemDet.Rows.Count > 0 Then
                strSql = "UPDATE " & cnStockDb & "..GVTRAN SET CHECKED = 'Y' WHERE RUNNO= '" + txtRunno.Text + "'"
                cmd = New OleDbCommand(strSql, cn)
                cmd.ExecuteNonQuery()
            End If

        ElseIf chkMarked.Checked = True Then
            chkUnMarked.Checked = False
            chkBoth.Checked = False
            Dim ScanStr As String = txtRunno.Text
            strSql = " SELECT TRANDATE, RUNNO, QTY, AMOUNT, BATCHNO, COSTID, CHECKED FROM " & cnStockDb & "..GVTRAN WHERE CHECKED = 'Y'"
            dtItemDet.Clear()
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtItemDet)
            If dtItemDet.Rows.Count > 0 Then
                gridFullView.DataSource = dtItemDet
            End If

            For cnt As Integer = 0 To dtItemDet.Rows.Count - 1
                With dtItemDet.Rows(cnt)
                    If .Item("CHECKED") = "Y" Then
                        gridFullView.Rows(cnt).DefaultCellStyle.BackColor = Color.LightBlue
                        gridFullView.Rows(cnt).DefaultCellStyle.SelectionBackColor = Color.LightBlue
                    End If
                End With
            Next

        ElseIf chkUnMarked.Checked = True Then
            Dim ScanStr As String = txtRunno.Text
            strSql = " SELECT TRANDATE, RUNNO, QTY, AMOUNT, BATCHNO, COSTID, CHECKED FROM " & cnStockDb & "..GVTRAN WHERE CHECKED = 'N'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtItemDet)
            If dtItemDet.Rows.Count > 0 Then
                gridFullView.DataSource = dtItemDet
            End If
        ElseIf chkBoth.Checked = True Then
            chkMarked.Checked = False
            chkUnMarked.Checked = False

            Dim ScanStr As String = txtRunno.Text
            strSql = " SELECT TRANDATE, RUNNO, QTY, AMOUNT, BATCHNO, COSTID, CHECKED FROM " & cnStockDb & "..GVTRAN WHERE CHECKED in ('Y','N')"

            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtItemDet)
            If dtItemDet.Rows.Count > 0 Then
                gridFullView.DataSource = dtItemDet
            End If
            For cnt As Integer = 0 To dtItemDet.Rows.Count - 1
                With dtItemDet.Rows(cnt)
                    If .Item("CHECKED") = "Y" Then
                        gridFullView.Rows(cnt).DefaultCellStyle.BackColor = Color.LightBlue
                        gridFullView.Rows(cnt).DefaultCellStyle.SelectionBackColor = Color.LightBlue
                    End If
                End With
            Next
        Else

            Dim dtEmpty As New DataTable
            'gridStone.DataSource = dtEmpty
            gridFullView.DataSource = dtEmpty

            Dim ScanStr As String = txtRunno.Text
            strSql = " SELECT TRANDATE, RUNNO, QTY, AMOUNT, BATCHNO, COSTID, CHECKED FROM " & cnStockDb & "..GVTRAN WHERE CHECKED in ('Y','N')"
            dtItemDet.Clear()
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtItemDet)

            'strSql = " IF OBJECT_ID('TEMP" & systemId & "STOCKCHECK') IS NOT NULL DROP TABLE TEMP" & systemId & "STOCKCHECK"
            'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            'cmd.ExecuteNonQuery()

            'strSql = " DECLARE @DEFPATH VARCHAR(200)"
            'strSql += " SELECT @DEFPATH = '" & defaultPic & "'"
            'strSql += " select itemId"
            'strSql += " ,TagNo"
            'strSql += " ,Pcs,GrsWt,NetWt"
            'strSql += " ,(select isnull(itemName,'') from " & cnAdminDb & "..itemmast where itemId = t.itemId)as itemName"
            'strSql += " ,isnull((select isnull(subItemName,'') from " & cnAdminDb & "..subItemMast where subItemid = t.subItemId),'')as subItemName"
            'strSql += " ,isnull((select isnull(itemCtrName,'') from " & cnAdminDb & "..itemCounter where itemCtrId = t.itemCtrId),'')as CounterName"
            'strSql += " ,Approval"
            'strSql += " ,chkTray"
            'strSql += " ,@DEFPATH + PCTFILE AS picFileName,1 RESULT"
            'strSql += " INTO TEMP" & systemId & "STOCKCHECK"
            'strSql += " from " & cnAdminDb & "..ITEMTAG as t"
            'strSql += " where 1=1 "

            'If cmbCostCentre_MAN.Enabled = True Then
            '    strSql += " AND COSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "')"
            'End If
            'strSql += " AND ISSDATE IS NULL"
            'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            'cmd.ExecuteNonQuery()

            'strSql = vbCrLf + " SELECT ITEMID,TAGNO,PCS,GRSWT,NETWT,ITEMNAME,SUBITEMNAME,COUNTERNAME,APPROVAL,CONVERT(VARCHAR,CHKTRAY) CHKTRAY"
            'strSql += vbCrLf + " ,PICFILENAME,RESULT FROM TEMP" & systemId & "STOCKCHECK "

            'strSql += vbCrLf + " ORDER BY RESULT ASC,CHKTRAY DESC,itemName ASC"
            'dtFullView.Clear()
            'dtFullView = New DataTable
            'da = New OleDbDataAdapter(strSql, cn)
            'da.Fill(dtFullView)
            'gridFullView.DataSource = dtFullView
            'If Not dtFullView.Rows.Count > 0 Then
            '    Exit Sub
            'End If
            'funcLastViewStyle(gridFullView)
            Dim totTag As Integer = 0
            Dim totPcs As Integer = 0
            Dim totGrsWt As Double = 0
            Dim totNetWt As Double = 0
            Dim markTag As Integer = 0
            Dim markPcs As Integer = 0
            Dim markGrsWt As Double = 0
            Dim markNetWt As Double = 0
            'For cnt As Integer = 0 To dtFullView.Rows.Count - 1
            '    With dtFullView.Rows(cnt)
            '        If .Item("chkTray") <> "" Then
            '            gridFullView.Rows(cnt).DefaultCellStyle.BackColor = Color.LightBlue
            '            gridFullView.Rows(cnt).DefaultCellStyle.SelectionBackColor = Color.LightBlue
            '            markTag += 1
            '            markPcs += Val(.Item("Pcs").ToString())
            '            markGrsWt += Val(.Item("GrsWt").ToString())
            '            markNetWt += Val(.Item("NetWt").ToString())
            '        End If
            '        totTag += 1
            '        totPcs += Val(.Item("Pcs").ToString())
            '        totGrsWt += Val(.Item("GrsWt").ToString())
            '        totNetWt += Val(.Item("NetWt").ToString())
            '    End With
            'Next
            'funcAddNodeListTotal("MARKED", markTag, markPcs, markGrsWt, markNetWt)
            'funcAddNodeListTotal("GRAND", totTag, IIf(chkpcstotal = True And Authorize = True, totPcs, 0), IIf(chkwttotal = True And Authorize = True, totGrsWt, 0), IIf(chkwttotal = True And Authorize = True, totNetWt, 0))

            'txtTrayNo.Clear()
            'txtItemCode.Clear()
            txtRunno.Clear()
            'txtTrayNo.Focus()
        End If
        If dtItemDet.Rows.Count > 0 Then
            gridFullView.DataSource = dtItemDet
            For cnt As Integer = 0 To dtItemDet.Rows.Count - 1
                With dtItemDet.Rows(cnt)
                    If .Item("CHECKED") = "Y" Then
                        gridFullView.Rows(cnt).DefaultCellStyle.BackColor = Color.LightBlue
                        gridFullView.Rows(cnt).DefaultCellStyle.SelectionBackColor = Color.LightBlue
                    End If
                End With
            Next
        Else
            MsgBox("Record not found", MsgBoxStyle.Information)
        End If
    End Sub



    Private Sub gridFullView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridFullView.KeyPress
        If e.KeyChar = Chr(Keys.Space) Then
            If Not gridFullView.Rows.Count > 0 Then Exit Sub
            If gridFullView.CurrentRow Is Nothing Then Exit Sub
            With gridFullView
                If .Item("chkTray", gridFullView.CurrentRow.Index).Value <> "" Then
                    strSql = " Update " & cnAdminDb & "..ITEMTAG Set chkTray = '',chkDate = null"
                    strSql += " where itemid = '" & gridFullView.Item("itemId", gridFullView.CurrentRow.Index).Value & "'"
                    strSql += " and TagNo = '" & gridFullView.Item("tagNo", gridFullView.CurrentRow.Index).Value & "'"
                    'strSql += " AND COMPANYID = '" & GetStockCompId() & "'"
                    gridFullView.Item("chkTray", gridFullView.CurrentRow.Index).Value = ""
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                    cmd.ExecuteNonQuery()

                    Dim rwIndex As Integer = gridFullView.CurrentRow.Index
                    gridFullView.Rows(gridFullView.CurrentRow.Index).DefaultCellStyle.BackColor = System.Drawing.SystemColors.HighlightText
                    gridFullView.Rows(gridFullView.CurrentRow.Index).DefaultCellStyle.SelectionBackColor = System.Drawing.SystemColors.HighlightText
                    
                End If
            End With
        End If
        If e.KeyChar = "X" Or e.KeyChar = "x" Then
            Me.btnExcelFullView_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = "P" Then
            btnPrintFullView_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub gridFullView_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridFullView.SelectionChanged
        If gridFullView.RowCount > 0 Then
            'If gridFullView.CurrentRow.Cells("RESULT").Value.ToString = "1" Then
            'funcStuddedDetails(gridFullView.Item("itemId", gridFullView.CurrentRow.Index).Value, gridFullView.Item("tagNo", gridFullView.CurrentRow.Index).Value, gridFullView.Item("picFileName", gridFullView.CurrentRow.Index).Value)
        End If
        'End If
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub



    Private Sub btnExcelFullView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExcelFullView.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridFullView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "Stock Checking", gridFullView, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub btnExcelStoneDetails_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExcelStoneDetails.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        'If gridStone.Rows.Count > 0 Then
        '    BrightPosting.GExport.Post(Me.Name, strCompanyName, "Stock Checking", gridStone, BrightPosting.GExport.GExportType.Export)
        'End If
    End Sub

    Private Sub btnPrintFullView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintFullView.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridFullView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "Stock Checking", gridFullView, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub btnPrintStoneDetails_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintStoneDetails.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        'If gridStone.Rows.Count > 0 Then
        '    BrightPosting.GExport.Post(Me.Name, strCompanyName, "Stock Checking", gridStone, BrightPosting.GExport.GExportType.Export)
        'End If
    End Sub

    Private Sub gridStone_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If UCase(e.KeyChar) = "X" Then
            Me.btnExcelStoneDetails_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = "P" Then
            btnPrintStoneDetails_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        objGPack.TextClear(Me)
        dtFullView.Clear()
        dtLastView.Clear()


        'chkAsonDate.Checked = False
        'chkCheckingByScan.Checked = False
        ' chkWithApproval.Checked = False

        gridFullView.DataSource = Nothing
        'gridLastView.DataSource = Nothing


        ''Set GridStyles
        'strSql = " Select ''TRANDATE,''RUNNO,''QTY,''AMOUNT,''BATCHNO,''COSTID"
        'strSql += " ,''Approval"
        'da = New OleDbDataAdapter(strSql, cn)
        'da.Fill(dtLastView)
        'gridFullView.DataSource = dtLastView

        'funcLastViewStyle(gridFullView)

        ''Adding Total into list View         
        'funcAddNodeListTotal("MARKED", 0, 0, 0, 0)
        'funcAddNodeListTotal("GRAND", 0, 0, 0, 0)

        ''lOAD cOSTCENTRE
        cmbCostCentre_MAN.Text = ""
        cmbCostCentre_MAN.Items.Clear()
        If GetAdmindbSoftValue("COSTCENTRE", "N") = "Y" Then
            cmbCostCentre_MAN.Enabled = True
            strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            objGPack.FillCombo(strSql, cmbCostCentre_MAN, False)
            cmbCostCentre_MAN.Text = IIf(cnDefaultCostId, "", cnCostName)
            If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then cmbCostCentre_MAN.Enabled = False
        Else
            cmbCostCentre_MAN.Enabled = False
        End If

        ''Load Company
        CmbCompany.Items.Clear()
        strSql = " Select Companyname from " & cnAdminDb & "..company  where isnull(active,'')='' or isnull(active,'')='Y' order by companyname"
        CmbCompany.Items.Add("ALL")
        objGPack.FillCombo(strSql, CmbCompany, False)
        CmbCompany.Text = "ALL"

    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub txtItemCode_NUM_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'If e.KeyCode = Keys.Insert Then
        '    strSql = "SELECT ITEMID, ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE STOCKTYPE = 'T' ORDER BY ITEMID"
        '    txtItemCode.Text = BrighttechPack.SearchDialog.Show("Find Itemid", strSql, cn)
        'End If
    End Sub

    '    Private Sub txtItemCode_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
    '        If e.KeyChar = Chr(Keys.Enter) Then
    '            Dim barcode2d() As String = txtItemCode.Text.Split(BARCODE2DSEP)
    '            If barcode2d.Length > 2 Then Call Barcode2ddetails(txtItemCode.Text) : Exit Sub

    '            Dim sp() As String = txtItemCode.Text.Split(PRODTAGSEP)
    '            Dim ScanStr As String = txtItemCode.Text
    '            If PRODTAGSEP <> "" And txtItemCode.Text <> "" Then
    '                sp = txtItemCode.Text.Split(PRODTAGSEP)
    '                txtItemCode.Text = Trim(sp(0))
    '            End If
    '            If txtItemCode.Text.StartsWith("#") Then txtItemCode.Text = txtItemCode.Text.Remove(0, 1)
    'CheckItem:
    '            If txtItemCode.Text = "" Then
    '                MsgBox("Item Id should not empty", MsgBoxStyle.Information)
    '                Exit Sub
    '            End If
    '            If PRODTAGSEP <> "" Then
    '                strSql = " SELECT ITEMID,TAGNO FROM " & cnAdminDb & "..ITEMTAG WHERE TAGKEY = '" & Trim(ScanStr).Replace(PRODTAGSEP, "") & "'"
    '                Dim dtItemDet As New DataTable
    '                da = New OleDbDataAdapter(strSql, cn)
    '                da.Fill(dtItemDet)
    '                If dtItemDet.Rows.Count > 0 Then
    '                    txtItemCode.Text = dtItemDet.Rows(0).Item("ITEMID").ToString
    '                    txtRunno.Text = dtItemDet.Rows(0).Item("TAGNO").ToString
    '                    GoTo LoadItemInfo
    '                End If
    '            ElseIf IsNumeric(ScanStr) = True And ScanStr.Contains(PRODTAGSEP) = False Then 'And objGPack.DupCheck("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE CONVERT(VARCHAR,ITEMID) = '" & Val(txtItemCode.Text) & "'" & GetItemQryFilteration()) = True Then
    '                'SendKeys.Send("{TAB}")
    '                'MsgBox("Invalid ItemId", MsgBoxStyle.Information)
    '                Exit Sub
    '            ElseIf txtItemCode.Text <> "" And objGPack.DupCheck("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE CONVERT(VARCHAR,ITEMID) = '" & Trim(txtItemCode.Text) & "'") = False Then
    '                strSql = " SELECT ITEMID,TAGNO FROM " & cnAdminDb & "..ITEMTAG WHERE TAGKEY = '" & Trim(txtItemCode.Text) & "'"
    '                Dim dtItemDet As New DataTable
    '                da = New OleDbDataAdapter(strSql, cn)
    '                da.Fill(dtItemDet)
    '                If dtItemDet.Rows.Count > 0 Then
    '                    txtItemCode.Text = dtItemDet.Rows(0).Item("ITEMID").ToString
    '                    txtRunno.Text = dtItemDet.Rows(0).Item("TAGNO").ToString
    '                    txtTagNo_KeyPress(Me, New KeyPressEventArgs(Chr(Keys.Enter)))
    '                    Exit Sub
    '                    GoTo CheckItem
    '                End If
    '            End If
    'LoadItemInfo:
    '            If sp.Length > 1 And PRODTAGSEP <> "" And txtItemCode.Text <> "" Then
    '                txtRunno.Text = Trim(sp(1))
    '            End If
    '            If txtRunno.Text <> "" Then
    '                txtTagNo_KeyPress(Me, New KeyPressEventArgs(Chr(Keys.Enter)))
    '            End If
    '        End If
    '    End Sub


    Private Sub DatacheckedasStock(ByVal filename As String)
        'Try
        Dim dtEmpty As New DataTable
        Dim totTag As Integer = 0
        Dim totPcs As Integer = 0
        Dim totGrsWt As Double = 0
        Dim totNetWt As Double = 0
        Dim markTag As Integer = 0
        Dim markPcs As Integer = 0
        Dim markGrsWt As Double = 0
        Dim markNetWt As Double = 0
        'gridStone.DataSource = dtEmpty
        gridFullView.DataSource = dtEmpty
        strSql = " IF OBJECT_ID('TEMP" & systemId & "STOCKCHECK') IS NOT NULL DROP TABLE TEMP" & systemId & "STOCKCHECK"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strSql += " select  itemId,TagNo,Pcs,GrsWt,NetWt,CONVERT(VARCHAR(50),'') as itemName,CONVERT(VARCHAR(50),'') as subItemName,CONVERT(VARCHAR(50),'') as CounterName"
        strSql += " ,Approval,chkTray,CONVERT(VARCHAR(50),'') AS picFileName,0 RESULT"
        strSql += " INTO TEMP" & systemId & "STOCKCHECK"
        strSql += " from " & cnAdminDb & "..ITEMTAG as t where 1 =2 "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        Dim startat As Integer = chkimportdataarry(2)
        Dim seperatorcol As String = chkimportdataarry(3)
        Dim fil As New FileStream(filename, FileMode.Open, FileAccess.Read)
        Dim fs As New StreamReader(fil)
        fs.BaseStream.Seek(0, SeekOrigin.Begin)
        Dim scantag As String
        Dim Itemid As String
        Dim Tagno As String
        Dim Inline As String
        Do While Not fs.EndOfStream
            Dim readline() As String
            Inline = fs.ReadLine.ToString
            If Inline.ToString = "" Or Inline Is Nothing Then Exit Do
            readline = Inline.Split(seperatorcol)
            scantag = readline(startat - 1)
            Dim sp() As String = scantag.Split(PRODTAGSEP)
            Dim ScanStr As String = scantag
            If PRODTAGSEP <> "" And scantag <> "" Then
                sp = scantag.Split(PRODTAGSEP)
                Itemid = Trim(sp(0))
                Tagno = Trim(sp(1))
            ElseIf scantag <> "" Then
                strSql = "select tagno,itemid from " & cnAdminDb & "..itemtag where tagkey ='" & scantag & "'"
                Dim dr As DataRow = GetSqlRow(strSql, cn)
                If Not dr Is Nothing Then
                    Itemid = dr.Item(1)
                    Tagno = dr.Item(0)
                End If
            End If
            strSql = "select 1 from TEMP" & systemId & "STOCKCHECK where itemid =" & Itemid & " and Tagno ='" & Tagno & "'"
            If Val(objGPack.GetSqlValue(strSql).ToString) > 0 Then GoTo nextloop
            strSql = " DECLARE @DEFPATH VARCHAR(200)"
            strSql += " SELECT @DEFPATH = '" & defaultPic & "'"
            strSql += " INSERT INTO TEMP" & systemId & "STOCKCHECK "
            strSql += " (itemId,TagNo,Pcs,GrsWt,NetWt,itemName,subItemName,CounterName,Approval,chkTray"
            strSql += " ,picFileName,RESULT)"
            strSql += " select itemId"
            strSql += " ,TagNo"
            strSql += " ,isnull(Pcs,0),isnull(GrsWt,0),isnull(NetWt,0)"
            strSql += " ,(select isnull(itemName,'') from " & cnAdminDb & "..itemmast where itemId = t.itemId)as itemName"
            strSql += " ,isnull((select isnull(subItemName,'') from " & cnAdminDb & "..subItemMast where subItemid = t.subItemId),'')as subItemName"
            strSql += " ,isnull((select isnull(itemCtrName,'') from " & cnAdminDb & "..itemCounter where itemCtrId = t.itemCtrId),'')as CounterName"
            strSql += " ,isnull(Approval,'')"
            strSql += " ,isnull(chkTray,0)"
            strSql += " ,@DEFPATH + isnull(PCTFILE,'') AS picFileName,1 RESULT"
            strSql += " from " & cnAdminDb & "..ITEMTAG as t"
            strSql += " where itemid=" & Itemid & " and tagno = '" & Tagno & "'"
            strSql += " AND ISSDATE IS NULL"
            If cmbCostCentre_MAN.Enabled Then
                strSql += " AND COSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "')"
            End If
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
nextloop:
        Loop
        strSql = vbCrLf + " SELECT ITEMID,TAGNO,PCS,GRSWT,NETWT,ITEMNAME,SUBITEMNAME,COUNTERNAME,APPROVAL,CONVERT(VARCHAR,CHKTRAY) CHKTRAY"
        strSql += vbCrLf + " ,PICFILENAME,RESULT FROM TEMP" & systemId & "STOCKCHECK "
        strSql += vbCrLf + " ORDER BY RESULT ASC,CHKTRAY DESC,itemName ASC"
        dtFullView.Clear()
        dtFullView = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtFullView)
        gridFullView.DataSource = dtFullView
        If Not dtFullView.Rows.Count > 0 Then
            Exit Sub
        End If
        'funcLastViewStyle(gridFullView)
        dataupdate()
        'Catch ex As Exception
        '    MsgBox(ex.Message)
        'End Try
    End Sub

    Private Sub dataupdate()
        Dim rwIndex As Integer = -1
        With dtFullView
            For cnt As Integer = 0 To .Rows.Count - 1
                rwIndex = cnt
                Dim itemid As String = gridFullView.Rows(cnt).Cells("itemid").Value.ToString
                Dim tagno As String = gridFullView.Rows(cnt).Cells("tagno").Value.ToString
                strSql = " Update " & cnAdminDb & "..ITEMTAG Set chkTray = '" & txtRunno.Text & "',chkDate = '" & GetEntryDate(GetServerDate(tran), tran) & "'"
                strSql += " where itemid = " & Val(itemid) & ""
                strSql += " and TagNo = '" & tagno & "'"
                If cmbCostCentre_MAN.Enabled Then
                    strSql += " AND COSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "')"
                End If
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
                'dtFullView.Rows(rwIndex).Item("CHKTRAY") = txtTrayNo.Text
                'CType(gridFullView.DataSource, DataTable).Rows(rwIndex).Item("CHKTRAY") = IIf(txtTrayNo.Text <> "", txtTrayNo.Text, 1)
                'dtFullView.AcceptChanges()                
                gridFullView.Rows(rwIndex).DefaultCellStyle.BackColor = Color.LightBlue
                gridFullView.Rows(rwIndex).DefaultCellStyle.SelectionBackColor = Color.LightBlue
            Next
        End With

        '22/05/2015
        'Dim MarkVal As Double
        'MarkVal = Val(CType(gridFullView.DataSource, DataTable).Compute("SUM(PCS)", "CHKTRAY <> '' AND RESULT =1"))
        'gridFullView.Rows(gridFullView.Rows.Count - 3).Cells("PCS").Value = MarkVal

        'MarkVal = Val(CType(gridFullView.DataSource, DataTable).Compute("SUM(GRSWT)", "CHKTRAY <> '' AND RESULT =1"))
        'gridFullView.Rows(gridFullView.Rows.Count - 3).Cells("GRSWT").Value = MarkVal
        'MarkVal = Val(CType(gridFullView.DataSource, DataTable).Compute("SUM(NETWT)", "CHKTRAY <> '' AND RESULT =1"))
        'gridFullView.Rows(gridFullView.Rows.Count - 3).Cells("NETWT").Value = MarkVal

    End Sub

    Private Sub gridFullView_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridFullView.CellContentClick

    End Sub

    Private Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Me.btnExcelFullView_Click(sender, e)

    End Sub

    Private Sub txtItemCode_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub txtRunno_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtRunno.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            Dim ScanStr As String = txtRunno.Text
            strSql = " SELECT TRANDATE, RUNNO, QTY, AMOUNT, BATCHNO, COSTID, CHECKED FROM " & cnStockDb & "..GVTRAN WHERE RUNNO = '" & Trim(ScanStr).Replace(PRODTAGSEP, "") & "'"
            Dim dtItemDet As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtItemDet)
            If dtItemDet.Rows.Count > 0 Then
                gridFullView.DataSource = dtItemDet
            End If

            Dim rwIndex As Integer = -1
            With dtFullView
                For cnt As Integer = 0 To .Rows.Count - 1
                    If Val(.Rows(cnt).Item("Runno").ToString) = Val(txtRunno.Text) And .Rows(cnt).Item("Runno").ToString = txtRunno.Text Then
                        rwIndex = cnt
                        Exit For
                    End If
                Next
                If rwIndex <> -1 Then
                    strSql = " Update " & cnStockDb & "..GVTRAN Set CHECKED = 'Y',chkDate = '" & GetEntryDate(GetServerDate(tran), tran) & "'"
                    strSql += " where RUNNO = '" & txtRunno.Text & "'"
                    If cmbCostCentre_MAN.Enabled Then
                        strSql += " AND COSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "')"
                    End If
                    'strSql += " AND COMPANYID = '" & GetStockCompId() & "'"
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                    cmd.ExecuteNonQuery()
                    'dtFullView.Rows(rwIndex).Item("CHKTRAY") = txtTrayNo.Text
                    CType(gridFullView.DataSource, DataTable).Rows(rwIndex).Item("CHKRUNNO") = txtRunno.Text

                    ''dtFullView.AcceptChanges()
                    'lstTotal.Items(0).SubItems(1).Text = Val(lstTotal.Items(0).SubItems(1).Text) + 1
                    'lstTotal.Items(0).SubItems(2).Text = (Val(lstTotal.Items(0).SubItems(2).Text) + Val(gridFullView.Item("Pcs", rwIndex).Value)).ToString
                    'lstTotal.Items(0).SubItems(3).Text = (Val(lstTotal.Items(0).SubItems(3).Text) + Val(gridFullView.Item("GrsWt", rwIndex).Value)).ToString
                    'lstTotal.Items(0).SubItems(4).Text = (Val(lstTotal.Items(0).SubItems(4).Text) + Val(gridFullView.Item("NetWt", rwIndex).Value)).ToString

                    gridFullView.Rows(rwIndex).DefaultCellStyle.BackColor = Color.LightBlue
                    gridFullView.Rows(rwIndex).DefaultCellStyle.SelectionBackColor = Color.LightBlue

                    dtLastView.Rows.RemoveAt(dtLastView.Rows.Count - 1)
                    Dim ro As DataRow = dtLastView.NewRow
                    For cnt As Integer = 0 To dtFullView.Columns.Count - 1
                        ro(cnt) = dtFullView.Rows(rwIndex).Item(cnt)
                    Next
                    dtLastView.Rows.Add(ro)
                    'gridLastView.DataSource = dtLastView

                    ' ''Stone details
                    'funcStuddedDetails(Val(txtItemCode.Text), txtTagNo.Text, gridLastView.Item("picFileName", gridLastView.CurrentRow.Index).Value.ToString())
                    'If chkCheckingByScan.Checked Then
                    '    txtItemCode.Clear()
                    '    txtTagNo.Clear()
                    '    txtItemCode.Focus()
                    'End If
                Else
                    'MsgBox("Invalid TagNo", MsgBoxStyle.Exclamation)
                    'If chkCheckingByScan.Checked Then
                    '    txtItemCode.Clear()
                    'End If
                    'txtTagNo.Clear()
                    'If chkCheckingByScan.Checked Then txtItemCode.Focus() Else txtTagNo.Focus()
                    ''txtTagNo.Focus()
                End If
            End With
        End If
        'txtRunno.Clear()
        txtRunno.Focus()
    End Sub

    Private Sub txtRunno_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtRunno.KeyDown

    End Sub

    Private Sub chkMarked_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMarked.CheckedChanged
        If chkMarked.Checked = True Then
            chkUnMarked.Checked = False
            chkBoth.Checked = False
        End If
    End Sub

    Private Sub chkUnMarked_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkUnMarked.CheckedChanged
        If chkUnMarked.Checked = True Then
            chkMarked.Checked = False
            chkBoth.Checked = False
        End If
    End Sub

    Private Sub chkBoth_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkBoth.CheckedChanged
        If chkBoth.Checked = True Then
            chkMarked.Checked = False
            chkUnMarked.Checked = False
        End If
    End Sub

    Private Sub txtRunno_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtRunno.TextChanged

    End Sub
End Class


