Imports System.Data.OleDb
Imports System.IO
Public Class frmTagedItemsStockViewOnlProduct
    'LAST MODIFIED ON 20/08/2015-Removed Grouper Total and add it in Query Itself coz slow in for Loop Done by-Jegan
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Dim strSql As String
    Dim tempDt As New DataTable("OtherDetails")
    Dim dtStoneDetails As New DataTable("StoneDetails")
    Dim dtGrandTotalDetails As New DataTable("GrandTotal")
    Dim defaultPic As String = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'PICPATH'")
    Dim headerBgColor As New System.Drawing.Color
    Dim DiaRnd As Integer = Val(GetAdmindbSoftValue("ROUNDOFF-DIA", 4))
    Dim dtCompany As New DataTable
    Dim dtMetal As New DataTable
    Dim dt As New DataTable
    Dim dtCategory As New DataTable
    Dim RowFillState As Boolean = False
    Dim objSearch As Object = Nothing
    Dim Studded As Boolean = False
    Dim OTHmaster As Boolean = False
    Dim _TagDupPrint As Boolean = IIf(GetAdmindbSoftValue("TAGCHKDUPPRINT", "N") = "Y", True, False)
    Dim Include As String = ""
    Dim dtCostCentre As New DataTable
    Function funcExit() As Integer
        Me.Close()
    End Function

    Private Sub frmTagedItemsStockViewOnlProduct_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Escape) Then
            If tabMain.SelectedTab.Name = tabView.Name Then
                btnBack_Click(Me, New EventArgs)
            End If
        ElseIf e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmTagedItemsStockViewOnlProduct_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not defaultPic.EndsWith("\") Then defaultPic += "\"

        'Me.WindowState = FormWindowState.Maximized
        'tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGeneral.Left, Me.tabGeneral.Top, Me.tabGeneral.Width, Me.tabGeneral.Height))
        grpFiltration.Location = New Point((ScreenWid - grpFiltration.Width) / 2, ((ScreenHit - 128) - grpFiltration.Height) / 2)

        pnlTotalGridView.Dock = DockStyle.Fill
        headerBgColor = System.Drawing.SystemColors.ControlLight

        tempDt.Columns.Add("MaxDescription", GetType(String))
        tempDt.Columns.Add("MaxValues", GetType(String))
        tempDt.Columns.Add("MinDescription", GetType(String))
        tempDt.Columns.Add("MinValues", GetType(String))
        tempDt.Columns.Add("OtherDesc1", GetType(String))
        tempDt.Columns.Add("OtherVal1", GetType(String))
        tempDt.Columns.Add("OtherDesc2", GetType(String))
        tempDt.Columns.Add("OtherVal2", GetType(String))

        dtStoneDetails.Columns.Add("Description", GetType(String))
        dtStoneDetails.Columns.Add("Diamond", GetType(String))
        dtStoneDetails.Columns.Add("Stone", GetType(String))
        dtStoneDetails.Columns.Add("Precious", GetType(String))

        dtGrandTotalDetails.Columns.Add("Description", GetType(String))
        dtGrandTotalDetails.Columns.Add("Pcs", GetType(String))
        dtGrandTotalDetails.Columns.Add("GrsWt", GetType(String))
        dtGrandTotalDetails.Columns.Add("LessWt", GetType(String))
        dtGrandTotalDetails.Columns.Add("NetWt", GetType(String))
        dtGrandTotalDetails.Columns.Add("ExtraWt", GetType(String))
        dtGrandTotalDetails.Columns.Add("SalValue", GetType(String))

        ''Checking CostCentre Status
        strSql = " select 1 from " & cnAdminDb & "..softcontrol where ctlText = 'Y' and ctlId = 'COSTCENTRE'"
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        'If dt.Rows.Count > 0 Then
        '    cmbCostCenter.Enabled = True
        'Else
        '    cmbCostCenter.Enabled = False
        'End If
        If dt.Rows.Count > 0 Then
            chkCmbCostCentre.Enabled = True
        Else
            chkCmbCostCentre.Enabled = False
        End If

        strSql = " SELECT 'ALL' METALNAME,'ALL' METALID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT METALNAME,METALID,2 RESULT FROM " & cnAdminDb & "..METALMAST "
        strSql += " ORDER BY RESULT,METALNAME"
        dtMetal = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtMetal)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbMetal, dtMetal, "METALNAME", , "ALL")


        strSql = " SELECT 'ALL' COMPANYNAME,'ALL' COMPANYID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COMPANYNAME,COMPANYID,2 RESULT FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N'"
        strSql += " ORDER BY RESULT,COMPANYNAME"
        dtCompany = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCompany)

        BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dtCompany, "COMPANYNAME", , strCompanyName)
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click

        If Not CheckBckDays(userId, Me.Name, dtpAsOnDate.Value) Then dtpAsOnDate.Focus() : Exit Sub
        Try

            strSql = " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ONLPRODUCT')>0"
            strSql += vbCrLf + " DROP TABLE TEMP" & systemId & "ONLPRODUCT"
            strSql += vbCrLf + " SELECT I.TAGNO,IM.CATCODE,SI.SUBITEMNAME,SI.SUBITEMID,DESCRIP "
            strSql += vbCrLf + " ,I.PCTFILE"
            strSql += vbCrLf + " ,IM.ITEMNAME,IM.ITEMID"
            strSql += vbCrLf + " ,IT.NAME TAGTYPE,I.GRSWT,I.NETWT"
            strSql += vbCrLf + " ,I.MAXWASTPER VAT_PERCENT,'' VATOFFER,I.MAXMC MC,I.TOFLAG,'' COLHEAD,i.APPROVAL"
            strSql += vbCrLf + " INTO TEMP" & systemId & "ONLPRODUCT "
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG I"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID=I.ITEMID"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.SUBITEMID=I.SUBITEMID AND SI.ITEMID=IM.ITEMID"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMTYPE IT ON IT.ITEMTYPEID=I.ITEMTYPEID"
            'strSql += vbCrLf + " WHERE ISSDATE IS NULL"
            If chkdate.Checked = True Then
                strSql += vbCrLf + "WHERE I.RECDATE BETWEEN '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
            Else
                strSql += vbCrLf + "WHERE I.RECDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "' "
            End If
            If txtTagno.Text <> "" Then
                strSql += vbCrLf + " and i.tagno='" & txtTagno.Text & "' "
            End If
            If chkCmbCostCentre.Text = "ALL" Or chkCmbCostCentre.Text <> "" Then
                strSql += vbCrLf + "AND I.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE GROUP BY COSTID)"
            Else
                strSql += vbCrLf + "AND I.COSTID IN (select costid from " & cnAdminDb & "..costcentre where costname in (" & GetQryString(chkCmbCostCentre.Text) & "))"
            End If

            If chkCmbCompany.Text <> "ALL" And chkCmbCompany.Text <> "" Then
                strSql += vbCrLf + " AND I.COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(chkCmbCompany.Text) & "))"
            Else
                strSql += vbCrLf + " AND I.COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N')"
            End If

            If txtItemCode_NUM.Text <> "" Then
                strSql += vbCrLf + " AND I.ITEMID = '" & txtItemCode_NUM.Text & "'"
            End If

            If cmbSubItemName.Enabled = True Then
                If cmbSubItemName.Text <> "ALL" And cmbSubItemName.Text <> "" Then
                    strSql += vbCrLf + " AND I.SUBITEMID IN (SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME IN (" & GetQryString(cmbSubItemName.Text) & ") AND ITEMID = " & Val(txtItemCode_NUM.Text) & ")"
                End If
            End If

            If chkCmbMetal.Text = "ALL" Or chkCmbMetal.Text = "" Then
                strSql += vbCrLf + "AND IM.METALID IN (SELECT METALID FROM " & cnAdminDb & "..ITEMMAST GROUP BY METALID)"
            Else
                strSql += vbCrLf + "AND IM.METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & GetQryString(chkCmbMetal.Text) & "))"

            End If
            If cmbCounterName.Text <> "ALL" And cmbCounterName.Text <> "" Then
                strSql += vbCrLf + " AND I.ITEMCTRID IN (SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME IN (" & GetQryString(cmbCounterName.Text) & "))"
            Else
                strSql += vbCrLf + " AND I.ITEMCTRID IN (SELECT DISTINCT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER )"
            End If

            If cmbItemType.Text <> "ALL" And cmbItemType.Text <> "" Then
                strSql += vbCrLf + "  AND ISNULL(I.ITEMTYPEID,0) = ISNULL((SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & cmbItemType.Text & "'),0)"
            End If
            strSql += vbCrLf + "AND I.TOFLAG NOT IN ('MI','SA','TR')"
            If chkApproval.Checked = True Then
                strSql += vbCrLf + "AND  I.APPROVAL IN ('A','','R') ORDER BY I.TAGNO "
            Else
                strSql += vbCrLf + "AND  I.APPROVAL IN ('') ORDER BY I.TAGNO "
            End If
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = "ALTER TABLE TEMP" & systemId & "ONLPRODUCT ALTER COLUMN SUBITEMID VARCHAR(20)"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()

            'strSql = "INSERT INTO TEMP" & systemId & "ONLPRODUCT (TAGNO,SALVALUE,NETWT,GRSWT,VAT_PERCENT,MC,COLHEAD,SUBITEMID,OFFER,VATOFFER) "
            'strSql += vbCrLf + "SELECT 'TOTAL',SUM(SALVALUE)SALVALUE,SUM(NETWT)NETWT,SUM(GRSWT)GRSWT,SUM(VAT_PERCENT) AS VAT_PERCENT,SUM(MC)MC,'G','','','' FROM TEMP" & systemId & "ONLPRODUCT "
            strSql = "INSERT INTO TEMP" & systemId & "ONLPRODUCT (TAGNO,NETWT,GRSWT,MC,COLHEAD,SUBITEMID,VATOFFER) "
            strSql += vbCrLf + "SELECT 'TOTAL',SUM(NETWT)NETWT,SUM(GRSWT)GRSWT,SUM(MC)MC,'G','','' FROM TEMP" & systemId & "ONLPRODUCT "
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()

            strSql = "UPDATE  TEMP" & systemId & "ONLPRODUCT SET SUBITEMID='' WHERE SUBITEMID=0 "
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()

            strSql = "SELECT * FROM TEMP" & systemId & "ONLPRODUCT ORDER BY COLHEAD"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
            dt = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If Not dt.Rows.Count > 1 Then
                btnView_Search.Enabled = True
                MsgBox("STOCK Not Available", MsgBoxStyle.Information)
                Exit Sub
            End If
            Dim title As String
            If chkdate.Checked = True Then
                title = "ONL PRODUCT DETAILS FROM " & dtpAsOnDate.Text & " TO " & dtpTo.Text & ""
            Else
                title = "ONL PRODUCT DETAILS AS ON DATE " & dtpAsOnDate.Text & " "
            End If
            lblReportTitle.Text = title
            lblReportTitle.Font = New Font("VERDANA", 9, FontStyle.Bold)
            lblReportTitle.Visible = True
            gridTotalView.DataSource = dt
            FormatGridColumns(gridTotalView, False, False, True, False)
            tabMain.SelectedTab = tabView
            'Prop_Sets()
            With gridTotalView
                .Columns("COLHEAD").Visible = False
                .Columns("APPROVAL").Visible = False
                .Columns("SUBITEMID").DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomRight
            End With
            ResizeToolStripMenuItem_Click(sender, e)
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        Finally
            btnView_Search.Enabled = True
        End Try
    End Sub

    Private Sub gridtotalview_cellformatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles gridTotalView.CellFormatting
        If gridTotalView.Rows(e.RowIndex).Cells("colhead").Value.ToString = "G" Then
            gridTotalView.Rows(e.RowIndex).DefaultCellStyle = reportTotalStyle
        ElseIf gridTotalView.Rows(e.RowIndex).Cells("approval").Value.ToString = "A" Then
            gridTotalView.Rows(e.RowIndex).DefaultCellStyle.BackColor = Color.Orchid
        ElseIf gridTotalView.Rows(e.RowIndex).Cells("approval").Value.ToString = "R" Then
            gridTotalView.Rows(e.RowIndex).DefaultCellStyle.BackColor = Color.Orchid
        End If
    End Sub
   
    Private Sub gridFullDetails_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs)
        If e.ColumnIndex = 0 Or e.ColumnIndex = 2 Or e.ColumnIndex = 4 Or e.ColumnIndex = 6 Then
            e.CellStyle.BackColor = headerBgColor
            e.CellStyle.SelectionBackColor = headerBgColor
        End If
        If e.ColumnIndex = 1 Or e.ColumnIndex = 3 Or e.ColumnIndex = 5 Or e.ColumnIndex = 7 Then
            e.CellStyle.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold)
        End If
    End Sub

    Private Sub gridGrandTotal_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs)
        If e.ColumnIndex = 0 Or e.RowIndex = 0 Then
            e.CellStyle.BackColor = headerBgColor
            e.CellStyle.SelectionBackColor = headerBgColor
        End If
        If e.RowIndex <> 0 And e.ColumnIndex <> 0 Then
            e.CellStyle.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold)
        End If
    End Sub

    Private Sub gridStoneDetails_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs)
        If e.ColumnIndex = 0 Or e.RowIndex = 0 Then
            e.CellStyle.BackColor = headerBgColor
            e.CellStyle.SelectionBackColor = headerBgColor
        End If
        If e.RowIndex <> 0 And e.ColumnIndex <> 0 Then
            e.CellStyle.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold)
        End If
    End Sub

    Private Sub txtItemCode_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtItemCode_NUM.KeyDown
        Dim itemId As String
        If e.KeyCode = Keys.Insert Then
            strSql = " SELECT DISTINCT"
            strSql += vbCrLf + " ITEMID, "
            strSql += vbCrLf + " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS ITEMNAME"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMMAST AS T"
            If chkCmbMetal.Text <> "ALL" And chkCmbMetal.Text <> "" Then
                strSql += vbCrLf + " WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & GetQryString(chkCmbMetal.Text) & "))"
            End If
            If chkCmbCategory.Text <> "ALL" And chkCmbCategory.Text <> "" Then
                strSql += vbCrLf + " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN(" & GetQryString(chkCmbCategory.Text) & "))"
            End If
            itemId = BrighttechPack.SearchDialog.Show("Find ItemId", strSql, cn, 1)
            strSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = '" & itemId & "'"

            Dim dt As New DataTable
            dt.Clear()
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                txtItemCode_NUM.Text = itemId
                txtItemName.Text = dt.Rows(0).Item("ITEMNAME")
            End If
        End If
    End Sub

    Private Sub txtItemCode_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtItemCode_NUM.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            strSql = " Select itemName,STUDDEDSTONE from " & cnAdminDb & "..itemMast where itemId = '" & txtItemCode_NUM.Text & "'"
            If chkCmbMetal.Text <> "ALL" And chkCmbMetal.Text <> "" Then
                strSql += vbCrLf + " AND METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & GetQryString(chkCmbMetal.Text) & "))"
            End If
            Dim dt As New DataTable
            dt.Clear()
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                txtItemName.Text = dt.Rows(0).Item("itemName")
                If dt.Rows(0).Item("STUDDEDSTONE").ToString = "Y" Then
                    Studded = True
                Else
                    Studded = False
                End If
            Else
                txtItemName.Clear()
            End If
          
        End If
    End Sub

    Private Sub txtItemName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtItemName.TextChanged
        If txtItemName.Text <> "" Then
            Dim dtSItem As DataTable
            cmbSubItemName.Items.Clear()
            cmbSubItemName.Items.Add("ALL")
            strSql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = '" & txtItemCode_NUM.Text & "' order by SubItemName"
            dtSItem = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtSItem)
            BrighttechPack.GlobalMethods.FillCombo(cmbSubItemName, dtSItem, "SUBITEMNAME", False)
            'objGPack.FillCombo(strSql, cmbSubItemName, False)
            cmbSubItemName.Text = "ALL"
            If cmbSubItemName.Items.Count > 0 Then
                cmbSubItemName.Enabled = True
            Else
                cmbSubItemName.Enabled = False
            End If

            cmbSize.Items.Clear()
            cmbSize.Items.Add("ALL")
            strSql = " SELECT SIZENAME FROM " & cnAdminDb & "..ITEMSIZE WHERE ITEMID = " & Val(txtItemCode_NUM.Text) & " ORDER BY SIZENAME"
            objGPack.FillCombo(strSql, cmbSize, False)
            cmbSize.Text = "ALL"
            If cmbSize.Items.Count > 0 Then
                cmbSize.Enabled = True
            Else
                cmbSize.Enabled = False
            End If
        Else
            cmbSubItemName.Items.Clear()
            cmbSubItemName.Enabled = False
        End If
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        funcExit()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        funcExit()
    End Sub



    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        objGPack.TextClear(Me)
        cmbSubItemName.Items.Clear()
        cmbSubItemName.Enabled = False
        ' NEWLY ADD ON 18/08/2015
        CmbGroupBy.Items.Clear()
        CmbGroupBy.Items.Add("SUBITEM")
        ''Counter
        cmbCounterName.Items.Clear()
        cmbCounterName.Items.Add("ALL")
        strSql = " SELECT ITEMCTRNAME FROM " & cnAdminDb & "..itemCounter order by itemCtrName"
        Dim dtCtr As DataTable
        dtCtr = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCtr)
        BrighttechPack.GlobalMethods.FillCombo(cmbCounterName, dtCtr, "ITEMCTRNAME", False)

        'objGPack.FillCombo(strSql, cmbCounterName, False)
        cmbCounterName.Text = "ALL"

        ''ItemType
        cmbItemType.Items.Clear()
        cmbItemType.Items.Add("ALL")
        strSql = " Select Name from " & cnAdminDb & "..itemType order by Name"
        objGPack.FillCombo(strSql, cmbItemType, False)
        cmbItemType.Text = "ALL"


        ''stockType
        cmbStockType.Items.Clear()
        cmbStockType.Items.Add("ALL")
        cmbStockType.Items.Add("MANUFACTURING")
        cmbStockType.Items.Add("TRADING")
        cmbStockType.Text = "ALL"


        ''CostCenter

        If chkCmbCostCentre.Enabled = True Then
            strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
            strSql += vbCrLf + " ORDER BY RESULT,COSTNAME"
            dtCostCentre = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtCostCentre)
            BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
            If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False
        End If

        BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dtCompany, "COMPANYNAME", , strCompanyName)
        dtpAsOnDate.Value = GetEntryDate(GetServerDate)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbMetal, dtMetal, "METALNAME", , "ALL")
        strSql = vbCrLf + " SELECT DISTINCT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(METALID,'') IN ('T','D')"
        strSql += vbCrLf + " ORDER BY ITEMNAME"
        Dim dtItem As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtItem)
        'If dtItem.Rows.Count > 0 Then
        '    chkCmbStoneName.Items.Add("ALL", True)
        '    BrighttechPack.GlobalMethods.FillCombo(chkCmbStoneName, dtItem, "ITEMNAME", False, "ALL")
        'End If
        'Studded = False
        'chkCmbStoneName.Enabled = Studded
        'chkCmbStSubItemName.Enabled = Studded
        chkCmbMetal.Select()
        Prop_Gets()

    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGeneral
        txtItemCode_NUM.Focus()
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridTotalView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblReportTitle.Text, gridTotalView, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridTotalView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblReportTitle.Text, gridTotalView, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub chkCmbMetal_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chkCmbMetal.KeyDown
        If chkCmbMetal.ValueChanged Then
            strSql = " SELECT 'ALL' CATNAME,'ALL' CATCODE,1 RESULT"
            strSql += " UNION ALL"
            strSql += " SELECT CATNAME,CATCODE,2 RESULT FROM " & cnAdminDb & "..CATEGORY WHERE METALID in (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN  (" & GetQryString(chkCmbMetal.Text) & "))"
            strSql += " ORDER BY RESULT,CATNAME"
            dtCategory = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtCategory)
            BrighttechPack.GlobalMethods.FillCombo(chkCmbCategory, dtCategory, "CATNAME", , "ALL")
        End If
    End Sub
    Private Sub Prop_Sets()
        'Dim obj As New frmTagedItemsStockViewOnlProduct_Properties
        'GetChecked_CheckedList(ChklstboxInclude, obj.p_ChklstboxInclude)
        'SetSettingsObj(obj, Me.Name, GetType(frmTagedItemsStockViewOnlProduct_Properties))
    End Sub

    Private Sub Prop_Gets()
        'Dim obj As New frmTagedItemsStockViewOnlProduct_Properties
        'GetSettingsObj(obj, Me.Name, GetType(frmTagedItemsStockViewOnlProduct_Properties))
        'SetChecked_CheckedList(ChklstboxInclude, obj.p_ChklstboxInclude, Nothing)
    End Sub


    Private Sub chkDate_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkdate.CheckedChanged
        If chkdate.Checked Then
            dateTo.Visible = True
            dtpTo.Visible = True
            chkdate.Text = "From"
        Else
            dateTo.Visible = False
            dtpTo.Visible = False
            chkdate.Text = "AsOnDate"
        End If
    End Sub
 
    Private Sub ResizeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResizeToolStripMenuItem.Click
        ResizeToolStripMenuItem.Checked = True
        If gridTotalView.RowCount > 0 Then
            If ResizeToolStripMenuItem.Checked Then
                gridTotalView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                gridTotalView.Invalidate()
                For Each dgvCol As DataGridViewColumn In gridTotalView.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridTotalView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            Else
                For Each dgvCol As DataGridViewColumn In gridTotalView.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridTotalView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End If
             
        End If
    End Sub

    
    Private Sub grpFiltration_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles grpFiltration.Enter

    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles txtTagno.TextChanged

    End Sub
End Class

