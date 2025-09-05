Imports System.Data.OleDb
Imports System.IO
Imports System.Management
Imports System.Linq
Imports UHFAPI_SPC
'Imports RF = GiritechRFID.Class1




Public Class frmStockCheckWithRFID
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
    Dim stk_chk As Boolean = IIf(GetAdmindbSoftValue("STKCHK_VIEW_TOTAL", "N") = "Y", True, False)
    Dim chkimportdataarry() As String = GetAdmindbSoftValue("STOCK_IMPORT_DATA", "N///,").ToString.Split("/")
    Dim BARCODE2DSEP As String = GetAdmindbSoftValue("BARCODE2DSEP")
    Dim chkpcstotal As Boolean = True
    Dim Authorize As Boolean = False
    Dim dtCounter As New DataTable
    Dim STKCHKDEVICELOCK As String = GetAdmindbSoftValue("STKCHKDEVICELOCK", "")

    Private _SPC_RFID As SPC_RFID = Nothing
    'Dim RfId As New RF.GetData

    'Dim reader As New C002
    'Dim cnt As Int32 = 1
    'Public 
    Public Sub New()
        _SPC_RFID = SPC_RFID.getInstance()
        InitializeComponent()
    End Sub
    Function funcAddNodeListTotal(ByVal desc As String, ByVal Tag As Integer, ByVal Pcs As Integer, ByVal grsWt As Double, ByVal netWt As Double) As Integer
        Dim node As New ListViewItem(desc)
        node.SubItems.Add(Tag)
        node.SubItems.Add(Pcs)
        node.SubItems.Add(grsWt)
        node.SubItems.Add(netWt)
        lstTotal.Items.Add(node)
    End Function

    Function funcLastViewStyle(ByVal grid As DataGridView) As Integer
        With grid
            With .Columns("itemId")
                .HeaderText = "ITEM.ID"
                .Width = 60
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Resizable = DataGridViewTriState.False
            End With
            With .Columns("TagNo")
                .HeaderText = "TAGNO"
                .Width = 100
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Resizable = DataGridViewTriState.False
            End With
            With .Columns("Pcs")
                .HeaderText = "PCS"
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Resizable = DataGridViewTriState.False
            End With
            With .Columns("GrsWt")
                .HeaderText = "GRS WT"
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Resizable = DataGridViewTriState.False
            End With
            With .Columns("NetWt")
                .HeaderText = "NET WT"
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Resizable = DataGridViewTriState.False
            End With
            With .Columns("RecDate")
                .HeaderText = "RECDATE"
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Resizable = DataGridViewTriState.False
            End With
            With .Columns("EXTRAWT")
                .HeaderText = "EXTRAWT"
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Resizable = DataGridViewTriState.False
            End With
            With .Columns("itemName")
                .HeaderText = "ITEM NAME"
                .Width = 150
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Resizable = DataGridViewTriState.False
            End With
            With .Columns("subItemName")
                .HeaderText = "SUBITEM"
                .Width = 130
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Resizable = DataGridViewTriState.False
            End With
            With .Columns("CounterName")
                .HeaderText = "COUNTER"
                .Width = 120
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Resizable = DataGridViewTriState.False
            End With
            With .Columns("ItemTypeName")
                .HeaderText = "ITEMTYPE"
                .Width = 120
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Resizable = DataGridViewTriState.False
            End With
            With .Columns("chkTray")
                .Visible = False
            End With
            With .Columns("Approval")
                .Visible = False
            End With
            With .Columns("picFileName")
                .Visible = False
            End With
            With .Columns("RESULT")
                .Visible = False
            End With
        End With
    End Function

    Function funcStoneStyle() As Integer
        With gridStone
            With .Columns("stnType")
                .Visible = False
            End With
            With .Columns("itemName")
                .HeaderText = "STONE"
                .Width = 150
            End With
            With .Columns("subItemName")
                .HeaderText = "SUB STONE"
                .Width = 100
            End With
            With .Columns("stnPcs")
                .HeaderText = "PCS"
                .Width = 60
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("stnWt")
                .HeaderText = "WEIGHT"
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
        End With
    End Function

    Function funcStuddedDetails(ByVal tagNo As String, ByVal picPath As String) As Integer
        Dim dt As New DataTable
        dt.Clear()
        strSql = " select "
        strSql += vbCrLf + " (select itemName from " & cnAdminDb & "..itemMast where ItemId = s.stnItemId)as itemName"
        strSql += vbCrLf + " ,(select subItemName from " & cnAdminDb & "..subitemMast where subItemId = s.stnsubItemId)as subItemName"
        strSql += vbCrLf + " ,stnPcs"
        strSql += vbCrLf + " ,stnWt"
        strSql += vbCrLf + " ,(SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID)AS STNTYPE"
        strSql += vbCrLf + " from " & cnAdminDb & "..ITEMTAGSTONE as s"
        strSql += vbCrLf + " where tagNo = '" & tagNo & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            If picPath <> "" Then
                If File.Exists(picPath) = True Then
                    Dim Finfo As IO.FileInfo
                    Finfo = New IO.FileInfo(picPath)
                    'Finfo.IsReadOnly = False
                    Dim fileStr As New IO.FileStream(picPath, IO.FileMode.Open, FileAccess.Read)
                    picTagImage.Image = Bitmap.FromStream(fileStr)
                    fileStr.Close()
                    'picTagImage.Image = Image.FromFile(picPath)
                Else
                    picTagImage.Image = Nothing
                End If
            Else
                picTagImage.Image = Nothing
            End If
            Dim dtEmpty As New DataTable
            dtEmpty.Clear()
            gridStone.DataSource = dtEmpty
            pnlStoneTotal.Visible = False
            txtStPcs.Clear()
            txtStWt.Clear()
            txtDiaPcs.Clear()
            txtDiaWt.Clear()
            txtPrePcs.Clear()
            txtPreWt.Clear()
            'picTagImage.Image = Nothing
            Exit Function
        End If
        gridStone.DataSource = dt
        funcStoneStyle()
        pnlStoneTotal.Visible = True
        For cnt As Integer = 0 To dt.Rows.Count - 1
            With dt.Rows(cnt)
                If .Item("stnType") = "D" Then
                    txtDiaPcs.Text = Val(txtDiaPcs.Text) + Val(.Item("stnPcs"))
                    txtDiaWt.Text = Val(txtDiaWt.Text) + Val(.Item("stnWt"))
                ElseIf .Item("stnType") = "S" Then
                    txtStPcs.Text = Val(txtStPcs.Text) + Val(.Item("stnPcs"))
                    txtStWt.Text = Val(txtStWt.Text) + Val(.Item("stnWt"))
                Else
                    txtPrePcs.Text = Val(txtPrePcs.Text) + Val(.Item("stnPcs"))
                    txtPreWt.Text = Val(txtPreWt.Text) + Val(.Item("stnWt"))
                End If
            End With
        Next
        If picPath <> "" Then
            If File.Exists(picPath) = True Then
                Dim Finfo As IO.FileInfo
                Finfo = New IO.FileInfo(picPath)
                'Finfo.IsReadOnly = False
                Dim fileStr As New IO.FileStream(picPath, IO.FileMode.Open, FileAccess.Read)
                picTagImage.Image = Bitmap.FromStream(fileStr)
                fileStr.Close()
                'picTagImage.Image = Image.FromFile(picPath)
            Else
                picTagImage.Image = Nothing
            End If
        Else
            picTagImage.Image = Nothing
        End If
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

    Private Sub chkAsonDate_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAsonDate.CheckStateChanged
        If chkAsonDate.Checked = True Then
            dtpAsOnDate.Enabled = True
        Else
            dtpAsOnDate.Enabled = False
        End If
    End Sub

    Private Sub cmbItemName_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbItemName.SelectedIndexChanged
        If cmbItemName.Text <> "ALL" And Trim(cmbItemName.Text) <> "" Then
            Dim dt As New DataTable
            dt.Clear()
            strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName.Text & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            ''Load range
            cmbRange.Items.Clear()
            strSql = " SELECT DISTINCT CAPTION FROM " & cnAdminDb & "..RANGEMAST "
            strSql += vbCrLf + " WHERE ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName.Text & "')"
            strSql += vbCrLf + " ORDER BY CAPTION"
            cmbRange.Items.Add("ALL")
            objGPack.FillCombo(strSql, cmbRange, False)
            cmbRange.Text = "ALL"
        Else
            ''Load range
            cmbRange.Items.Clear()
            strSql = " SELECT DISTINCT CAPTION FROM " & cnAdminDb & "..RANGEMAST "
            strSql += vbCrLf + " ORDER BY CAPTION"
            cmbRange.Items.Add("ALL")
            objGPack.FillCombo(strSql, cmbRange, False)
            cmbRange.Text = "ALL"
        End If
        cmbSubItem.Items.Clear()
        cmbSubItem.Items.Add("ALL")
        strSql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST"
        If cmbItemName.Text <> "ALL" And Trim(cmbItemName.Text) <> "" Then
            strSql += vbCrLf + " WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName.Text & "')"
        End If
        strSql += vbCrLf + " ORDER BY SUBITEMNAME"
        objGPack.FillCombo(strSql, cmbSubItem, False, False)
        cmbSubItem.Text = "ALL"
    End Sub

    Private Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        lblCount.Text = "Scaned Count : "
        If txtTrayNo.Text.Trim = "" Then
            MsgBox("Tray No Should Not Empty", MsgBoxStyle.Exclamation)
            txtTrayNo.Focus()
            Exit Sub
        End If
        Dim dtEmpty As New DataTable
        gridStone.DataSource = dtEmpty
        gridFullView.DataSource = dtEmpty
        strSql = " IF OBJECT_ID('TEMP" & systemId & "STOCKCHECK') IS NOT NULL DROP TABLE TEMP" & systemId & "STOCKCHECK"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " DECLARE @DEFPATH VARCHAR(200)"
        strSql += vbCrLf + " SELECT @DEFPATH = '" & defaultPic & "'"
        strSql += vbCrLf + " SELECT T.ITEMID"
        strSql += vbCrLf + " ,T.TAGNO"
        strSql += vbCrLf + " ,PCS,GRSWT,NETWT,CONVERT(VARCHAR(12),RECDATE,103)RECDATE"
        strSql += vbCrLf + " ,(SELECT ISNULL(ITEMNAME,'') FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS ITEMNAME"
        strSql += vbCrLf + " ,ISNULL((SELECT ISNULL(SUBITEMNAME,'') FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = T.SUBITEMID),'')AS SUBITEMNAME"
        strSql += vbCrLf + " ,ISNULL((SELECT ISNULL(ITEMCTRNAME,'') FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = T.ITEMCTRID),'')AS COUNTERNAME"
        strSql += vbCrLf + " ,(SELECT ISNULL(NAME,'') FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = T.ITEMTYPEID)AS ITEMTYPENAME"
        strSql += vbCrLf + " ,(SELECT ISNULL(DESIGNERNAME,'') FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = T.DESIGNERID)AS DESIGNER"
        strSql += vbCrLf + " ,APPROVAL"
        strSql += vbCrLf + " ,CHKTRAY"
        strSql += vbCrLf + " ,@DEFPATH + PCTFILE AS PICFILENAME,1 RESULT,EXTRAWT"
        strSql += vbCrLf + " INTO TEMP" & systemId & "STOCKCHECK"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T "
        strSql += vbCrLf + " WHERE 1=1 "
        'IF NOT CNCENTSTOCK THEN STRSQL += VBCRLF + " AND T.COMPANYID = '" & GETSTOCKCOMPID() & "'"
        If cmbItemName.Text <> "ALL" And Trim(cmbItemName.Text) <> "" Then
            strSql += vbCrLf + " AND T.ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName.Text & "')"
        End If
        If cmbItemType.Text <> "ALL" And Trim(cmbItemType.Text) <> "" Then
            strSql += vbCrLf + " AND T.ITEMTYPEID = (SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & cmbItemType.Text & "')"
        End If
        If CmbMetal.Text <> "ALL" And Trim(CmbMetal.Text) <> "" Then
            strSql += vbCrLf + " AND T.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME='" & CmbMetal.Text & "'))"
        End If
        If cmbSubItem.Text <> "ALL" And cmbSubItem.Text <> "" Then
            strSql += vbCrLf + " AND T.SUBITEMID IN (SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem.Text & "')"
        End If
        If chkCmbCounter.Text <> "ALL" And Trim(chkCmbCounter.Text) <> "" Then
            strSql += vbCrLf + " AND T.ITEMCTRID IN (SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER "
            strSql += vbCrLf + " WHERE ITEMCTRNAME IN (" & GetChecked_CheckedList(chkCmbCounter, True) & "))"
        End If
        If cmbDesignerName.Text <> "ALL" And Trim(cmbDesignerName.Text) <> "" Then
            strSql += vbCrLf + " AND T.DESIGNERID = (SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & cmbDesignerName.Text & "')"
        End If
        If CmbCompany.Text <> "ALL" And Trim(CmbCompany.Text) <> "" Then
            strSql += vbCrLf + " AND T.COMPANYID = (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & CmbCompany.Text & "')"
        End If
        If chkAsonDate.Checked = True Then
            strSql += vbCrLf + " AND T.RECDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
        End If
        If rbtPending.Checked = True Then
            strSql += vbCrLf + " AND CHKTRAY = ''"
        ElseIf rbtMarked.Checked = True Then
            strSql += vbCrLf + " AND CHKTRAY <> ''"
            If txtTrayNo.Text <> "" Then
                strSql += vbCrLf + " AND CHKTRAY = '" & txtTrayNo.Text & "'"
            End If
        End If
        If chkWithApproval.Checked = False Then
            strSql += vbCrLf + " AND ISNULL(APPROVAL,'') = ''"
        End If
        If cmbCostCentre_MAN.Text <> "" And cmbCostCentre_MAN.Text <> "ALL" Then
            strSql += vbCrLf + " AND COSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "')"
        End If
        If cmbRange.Text <> "ALL" Then
            strSql += vbCrLf + " AND (SELECT TOP 1 CAPTION FROM " & cnAdminDb & "..RANGEMAST AS R WHERE R.ITEMID=T.ITEMID AND R.SUBITEMID=T.SUBITEMID "
            strSql += vbCrLf + " AND T.GRSWT BETWEEN R.FROMWEIGHT AND R.TOWEIGHT)='" & cmbRange.Text.ToString & "'"
        End If
        strSql += vbCrLf + " AND ISSDATE IS NULL"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + " SELECT ITEMID,TAGNO,PCS,GRSWT,NETWT,RECDATE,ITEMNAME,SUBITEMNAME,COUNTERNAME,ITEMTYPENAME,DESIGNER,APPROVAL,CONVERT(VARCHAR,CHKTRAY) CHKTRAY"
        strSql += vbCrLf + " ,PICFILENAME,RESULT,EXTRAWT FROM TEMP" & systemId & "STOCKCHECK "
        'strSql += vbcrlf + " UNION ALL"
        'strSql += vbcrlf + " SELECT NULL ITEMID,'MARKED' TAGNO,SUM(PCS) PCS,SUM(GRSWT) GRSWT,SUM(NETWT) NETWT"
        'strSql += vbCrLf + " ,'' ITEMNAME,'' SUBITEMNAME,'' COUNTERNAME,'' APPROVAL,'' CHKTRAY"
        'strSql += vbCrLf + " ,'' PICFILENAME,2 RESULT FROM TEMP" & systemId & "STOCKCHECK WHERE chkTray <> ''"
        'strSql += vbcrlf + " UNION ALL"
        'strSql += vbcrlf + " SELECT NULL ITEMID,'UNMARKED' TAGNO,SUM(PCS) PCS,SUM(GRSWT) GRSWT,SUM(NETWT) NETWT"
        'strSql += vbcrlf + " ,'' ITEMNAME,'' SUBITEMNAME,'' COUNTERNAME,'' APPROVAL,'' CHKTRAY"
        'strSql += vbCrLf + " ,'' PICFILENAME,3 RESULT FROM TEMP" & systemId & "STOCKCHECK WHERE chkTray = ''"
        'strSql += vbCrLf + " UNION ALL"
        'strSql += vbCrLf + " SELECT NULL ITEMID,'GRAND TOTAL' TAGNO,SUM(PCS) PCS,SUM(GRSWT) GRSWT,SUM(NETWT) NETWT"
        'strSql += vbCrLf + " ,'' ITEMNAME,'' SUBITEMNAME,'' COUNTERNAME,'' APPROVAL,'' CHKTRAY"
        'strSql += vbCrLf + " ,'' PICFILENAME,4 RESULT FROM TEMP" & systemId & "STOCKCHECK"
        strSql += vbCrLf + " ORDER BY RESULT ASC,CHKTRAY DESC,ITEMNAME ASC"
        dtFullView.Clear()
        dtFullView = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtFullView)
        gridFullView.DataSource = dtFullView
        If Not dtFullView.Rows.Count > 0 Then
            Exit Sub
        End If
        funcLastViewStyle(gridFullView)
        Dim totTag As Integer = 0
        Dim totPcs As Integer = 0
        Dim totGrsWt As Double = 0
        Dim totNetWt As Double = 0
        Dim markTag As Integer = 0
        Dim markPcs As Integer = 0
        Dim markGrsWt As Double = 0
        Dim markNetWt As Double = 0
        Dim UnmarkTag As Integer = 0
        Dim UnmarkPcs As Integer = 0
        Dim UnmarkGrsWt As Double = 0
        Dim UnmarkNetWt As Double = 0
        For cnt As Integer = 0 To dtFullView.Rows.Count - 1
            With dtFullView.Rows(cnt)
                If .Item("chkTray") <> "" Then
                    gridFullView.Rows(cnt).DefaultCellStyle.BackColor = Color.LightBlue
                    gridFullView.Rows(cnt).DefaultCellStyle.SelectionBackColor = Color.LightBlue
                    markTag += 1
                    markPcs += Val(.Item("Pcs").ToString())
                    markGrsWt += Val(.Item("GrsWt").ToString())
                    markNetWt += Val(.Item("NetWt").ToString())
                Else
                    UnmarkTag += 1
                    UnmarkPcs += Val(.Item("Pcs").ToString())
                    UnmarkGrsWt += Val(.Item("GrsWt").ToString())
                    UnmarkNetWt += Val(.Item("NetWt").ToString())
                End If
                totTag += 1
                totPcs += Val(.Item("Pcs").ToString())
                totGrsWt += Val(.Item("GrsWt").ToString())
                totNetWt += Val(.Item("NetWt").ToString())
            End With
        Next

        lstTotal.Items.Clear()
        funcAddNodeListTotal("MARKED", markTag, markPcs, markGrsWt, markNetWt)
        funcAddNodeListTotal("UNMARKED", UnmarkTag, UnmarkPcs, UnmarkGrsWt, UnmarkNetWt)
        If stk_chk = True Then
            funcAddNodeListTotal("GRAND", totTag, IIf(chkpcstotal = True And Authorize = True, totPcs, 0), IIf(chkwttotal = True And Authorize = True, totGrsWt, 0), IIf(chkwttotal = True And Authorize = True, totNetWt, 0))
        End If

        txtTagNo.Clear()
        txtTrayNo.Focus()
    End Sub

    Private Sub txtTagNo_TextChanged(sender As Object, e As EventArgs) Handles txtTagNo.TextChanged
        MarkTag(sender.Text)
    End Sub
    Private Sub MarkTag(values As String)
        If Not dtFullView.Rows.Count > 0 Then
            Exit Sub
        End If
        If txtTrayNo.Text.Trim = "" And rbtMarked.Checked Then
            'MsgBox("Tray No Should Not Empty", MsgBoxStyle.Exclamation)
            txtTrayNo.Focus()
            Exit Sub
        End If
        If values = "" Then
            'MsgBox("Tag No Should Not Empty", MsgBoxStyle.Exclamation)
            txtTagNo.Focus()
            Exit Sub
        End If
        Dim rwIndex As Integer = -1
        With dtFullView
            For Each row As DataGridViewRow In gridFullView.Rows
                If row.Cells("TAGNO").Value.ToString = values Then
                    rwIndex = row.Index
                End If
            Next
            If rwIndex <> -1 Then
                If .Rows(rwIndex).Item("chkTray") <> "" Then
                    'MsgBox("Already Checked in Tray No : " + .Rows(rwIndex).Item("chkTray"), MsgBoxStyle.Exclamation)
                    txtTagNo.Focus()
                    Exit Sub
                End If
                strSql = " Update " & cnAdminDb & "..ITEMTAG Set chkTray = '" & txtTrayNo.Text & "',chkDate = '" & GetEntryDate(GetServerDate(tran), tran) & "'"
                strSql += vbCrLf + " where TagNo = '" & values & "'"
                If cmbCostCentre_MAN.Text <> "" And cmbCostCentre_MAN.Text <> "ALL" Then
                    strSql += vbCrLf + " AND COSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "')"
                End If
                'strSql += VBCRLF + " AND COMPANYID = '" & GetStockCompId() & "'"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
                'dtFullView.Rows(rwIndex).Item("CHKTRAY") = txtTrayNo.Text
                CType(gridFullView.DataSource, DataTable).Rows(rwIndex).Item("CHKTRAY") = txtTrayNo.Text

                'dtFullView.AcceptChanges()
                lstTotal.Items(0).SubItems(1).Text = Val(lstTotal.Items(0).SubItems(1).Text) + 1
                lstTotal.Items(0).SubItems(2).Text = (Val(lstTotal.Items(0).SubItems(2).Text) + Val(gridFullView.Item("Pcs", rwIndex).Value)).ToString
                lstTotal.Items(0).SubItems(3).Text = (Val(lstTotal.Items(0).SubItems(3).Text) + Val(gridFullView.Item("GrsWt", rwIndex).Value)).ToString
                lstTotal.Items(0).SubItems(4).Text = (Val(lstTotal.Items(0).SubItems(4).Text) + Val(gridFullView.Item("NetWt", rwIndex).Value)).ToString
                If Val(lstTotal.Items(1).SubItems(1).Text) - 1 <= 0 Then
                    lstTotal.Items(1).SubItems(1).Text = DBNull.Value.ToString
                    lstTotal.Items(1).SubItems(2).Text = DBNull.Value.ToString
                    lstTotal.Items(1).SubItems(3).Text = DBNull.Value.ToString
                    lstTotal.Items(1).SubItems(4).Text = DBNull.Value.ToString
                Else
                    lstTotal.Items(1).SubItems(1).Text = Val(lstTotal.Items(1).SubItems(1).Text) - 1
                    lstTotal.Items(1).SubItems(2).Text = (Val(lstTotal.Items(1).SubItems(2).Text) - Val(gridFullView.Item("Pcs", rwIndex).Value)).ToString
                    lstTotal.Items(1).SubItems(3).Text = (Val(lstTotal.Items(1).SubItems(3).Text) - Val(gridFullView.Item("GrsWt", rwIndex).Value)).ToString
                    lstTotal.Items(1).SubItems(4).Text = (Val(lstTotal.Items(1).SubItems(4).Text) - Val(gridFullView.Item("NetWt", rwIndex).Value)).ToString
                End If


                gridFullView.Rows(rwIndex).DefaultCellStyle.BackColor = Color.LightBlue
                gridFullView.Rows(rwIndex).DefaultCellStyle.SelectionBackColor = Color.LightBlue

                dtLastView.Rows.RemoveAt(dtLastView.Rows.Count - 1)
                Dim ro As DataRow = dtLastView.NewRow
                For cnt As Integer = 0 To dtFullView.Columns.Count - 1
                    ro(cnt) = dtFullView.Rows(rwIndex).Item(cnt)
                Next
                dtLastView.Rows.Add(ro)
                gridLastView.DataSource = dtLastView

                ''Stone details
                funcStuddedDetails(values, gridLastView.Item("picFileName", gridLastView.CurrentRow.Index).Value.ToString())
            End If
        End With
    End Sub
    Private Sub txtTagNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtTagNo.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            MarkTag(sender.Text)
        End If
    End Sub

    Private Sub gridFullView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridFullView.KeyPress
        If e.KeyChar = Chr(Keys.Space) Then
            If Not gridFullView.Rows.Count > 0 Then Exit Sub
            If gridFullView.CurrentRow Is Nothing Then Exit Sub
            With gridFullView
                If .Item("chkTray", gridFullView.CurrentRow.Index).Value <> "" Then
                    strSql = " Update " & cnAdminDb & "..ITEMTAG Set chkTray = '',chkDate = null"
                    strSql += vbCrLf + " where itemid = '" & gridFullView.Item("itemId", gridFullView.CurrentRow.Index).Value & "'"
                    strSql += vbCrLf + " and TagNo = '" & gridFullView.Item("tagNo", gridFullView.CurrentRow.Index).Value & "'"
                    'strSql += VBCRLF + " AND COMPANYID = '" & GetStockCompId() & "'"
                    gridFullView.Item("chkTray", gridFullView.CurrentRow.Index).Value = ""
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                    cmd.ExecuteNonQuery()

                    Dim rwIndex As Integer = gridFullView.CurrentRow.Index
                    gridFullView.Rows(gridFullView.CurrentRow.Index).DefaultCellStyle.BackColor = System.Drawing.SystemColors.HighlightText
                    gridFullView.Rows(gridFullView.CurrentRow.Index).DefaultCellStyle.SelectionBackColor = System.Drawing.SystemColors.HighlightText
                    lstTotal.Items(0).SubItems(1).Text = Val(lstTotal.Items(0).SubItems(1).Text) - 1
                    lstTotal.Items(0).SubItems(2).Text = (Val(lstTotal.Items(0).SubItems(2).Text) - Val(gridFullView.Item("Pcs", rwIndex).Value)).ToString
                    lstTotal.Items(0).SubItems(3).Text = (Val(lstTotal.Items(0).SubItems(3).Text) - Val(gridFullView.Item("GrsWt", rwIndex).Value)).ToString
                    lstTotal.Items(0).SubItems(4).Text = (Val(lstTotal.Items(0).SubItems(4).Text) - Val(gridFullView.Item("NetWt", rwIndex).Value)).ToString
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
            If gridFullView.CurrentRow.Cells("RESULT").Value.ToString = "1" Then
                funcStuddedDetails(gridFullView.Item("tagNo", gridFullView.CurrentRow.Index).Value, gridFullView.Item("picFileName", gridFullView.CurrentRow.Index).Value)
            End If
        End If
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub txtTagNo_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTagNo.LostFocus

    End Sub

    Private Sub btnExcelFullView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExcelFullView.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridFullView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "Stock Checking", gridFullView, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub btnExcelStoneDetails_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExcelStoneDetails.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridStone.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "Stock Checking", gridStone, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub btnPrintFullView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintFullView.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridFullView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "Stock Checking", gridFullView, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub btnPrintStoneDetails_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintStoneDetails.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridStone.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "Stock Checking", gridStone, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub gridStone_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridStone.KeyPress
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

        dtpAsOnDate.Value = GetServerDate()
        'chkAsonDate.Checked = False
        'chkCheckingByScan.Checked = False
        ' chkWithApproval.Checked = False

        lblCount.Text = "Scaned Count : "
        gridFullView.DataSource = Nothing
        gridLastView.DataSource = Nothing
        If chkimportdataarry(0).ToString = "N" Then
            btnImportdata.Visible = False
        Else
            btnImportdata.Visible = True
            btnImportdata.Enabled = True
            If chkimportdataarry(1).ToString = "" Then btnImportdata.Enabled = False
            If chkimportdataarry(2).ToString = "" Then btnImportdata.Enabled = False
            If chkimportdataarry(3).ToString = "" Then btnImportdata.Enabled = False
        End If


        ''Set GridStyles
        strSql = " SELECT ''ITEMID,''TAGNO,''PCS,''GRSWT,''NETWT,'' RECDATE,''ITEMNAME,''SUBITEMNAME,''COUNTERNAME,''ITEMTYPENAME,''DESIGNER"
        strSql += vbCrLf + " ,''APPROVAL"
        strSql += vbCrLf + " ,''CHKTRAY"
        strSql += vbCrLf + " ,''PICFILENAME,'' RESULT,''EXTRAWT"
        da = New OleDbDataAdapter(strSql, cn)
        dtLastView = New DataTable
        da.Fill(dtLastView)
        gridLastView.DataSource = dtLastView
        gridFullView.DataSource = dtLastView
        funcLastViewStyle(gridLastView)
        funcLastViewStyle(gridFullView)

        ''Adding Total into list View 
        lstTotal.Columns.Clear()
        lstTotal.Columns.Add("Description", 100, HorizontalAlignment.Left)
        lstTotal.Columns.Add("TagNo", 60, HorizontalAlignment.Right)
        lstTotal.Columns.Add("Pcs", 80, HorizontalAlignment.Right)
        lstTotal.Columns.Add("GrsWt", 80, HorizontalAlignment.Right)
        lstTotal.Columns.Add("NetWt", 80, HorizontalAlignment.Right)
        lstTotal.Items.Clear()
        funcAddNodeListTotal("MARKED", 0, 0, 0, 0)
        funcAddNodeListTotal("UNMARKED", 0, 0, 0, 0)
        funcAddNodeListTotal("GRAND", 0, 0, 0, 0)

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

        ''Load ItemName
        CmbMetal.Items.Clear()
        strSql = " Select MetalName from " & cnAdminDb & "..metalMast order by MetalName"
        CmbMetal.Items.Add("ALL")
        objGPack.FillCombo(strSql, CmbMetal, False)
        CmbMetal.Text = "ALL"

        ''Load ItemName
        cmbItemName.Items.Clear()
        strSql = " Select itemName from " & cnAdminDb & "..itemMast order by itemName"
        cmbItemName.Items.Add("ALL")
        objGPack.FillCombo(strSql, cmbItemName, False)
        cmbItemName.Text = "ALL"

        ''Load range
        cmbRange.Items.Clear()
        strSql = " SELECT DISTINCT CAPTION FROM " & cnAdminDb & "..RANGEMAST ORDER BY CAPTION"
        cmbRange.Items.Add("ALL")
        objGPack.FillCombo(strSql, cmbRange, False)
        cmbRange.Text = "ALL"

        ''Load ItemType
        cmbItemType.Items.Clear()
        strSql = " SELECT NAME FROM " & cnAdminDb & "..ITEMTYPE ORDER BY NAME"
        cmbItemType.Items.Add("ALL")
        objGPack.FillCombo(strSql, cmbItemType, False)
        cmbItemType.Text = "ALL"

        ''Load Counter
        strSql = " SELECT 'ALL' ITEMCTRNAME,'ALL' ITEMCTRID,1 RESULT"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT ITEMCTRNAME,CONVERT(vARCHAR,ITEMCTRID),2 RESULT FROM " & cnAdminDb & "..ITEMCOUNTER"
        strSql += vbCrLf + " ORDER BY RESULT,ITEMCTRNAME"
        dtCounter = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCounter)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCounter, dtCounter, "ITEMCTRNAME", , "ALL")

        ''Load Company
        CmbCompany.Items.Clear()
        strSql = " Select Companyname from " & cnAdminDb & "..company  where isnull(active,'')='' or isnull(active,'')='Y' order by companyname"
        CmbCompany.Items.Add("ALL")
        objGPack.FillCombo(strSql, CmbCompany, False)
        CmbCompany.Text = "ALL"

        ''Load Designer
        cmbDesignerName.Items.Clear()
        strSql = " Select DesignerName from " & cnAdminDb & "..Designer order by DesignerName"
        cmbDesignerName.Items.Add("ALL")
        objGPack.FillCombo(strSql, cmbDesignerName, False)
        cmbDesignerName.Text = "ALL"
        CmbCompany.Focus()

        'Try
        '    RfId.StopTimer()
        'Catch ex As Exception

        'End Try
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    ''    Private Sub txtItemCode_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtItemCode.KeyPress
    ''        If e.KeyChar = Chr(Keys.Enter) Then
    ''            If txtTrayNo.Text.Trim = "" Then
    ''                MsgBox("Tray No Should Not Empty", MsgBoxStyle.Exclamation)
    ''                txtTrayNo.Focus()
    ''                Exit Sub
    ''            End If

    ''            If chkCheckingByScan.Checked = True Then
    ''                If STKCHKDEVICELOCK <> "" Then
    ''                    Dim r1 As Boolean = isPointingDeviceAttached()
    ''                    Dim r2 As Boolean = isKeyboardAttached()
    ''                    If (r1 = True And r2 = True) And STKCHKDEVICELOCK = "B" Then
    ''                        MsgBox("Detach Keyboard & Mouse", MsgBoxStyle.Information)
    ''                        txtItemCode.Text = ""
    ''                        Exit Sub
    ''                    End If
    ''                    If r1 = True And STKCHKDEVICELOCK = "M" Then
    ''                        MsgBox("Detach Mouse", MsgBoxStyle.Information)
    ''                        txtItemCode.Text = ""
    ''                        Exit Sub
    ''                    ElseIf r2 = True And STKCHKDEVICELOCK = "K" Then
    ''                        MsgBox("Detach Keyboard", MsgBoxStyle.Information)
    ''                        txtItemCode.Text = ""
    ''                        Exit Sub
    ''                    End If
    ''                End If
    ''            End If


    ''            Dim _ItemId As String = ""
    ''            If cmbItemName.Text.ToString <> "ALL" And cmbItemName.Text.ToString <> "" Then
    ''                strSql = "SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName.Text & "'"
    ''                _ItemId = objGPack.GetSqlValue(strSql, , "").ToString
    ''            End If
    ''            Dim barcode2d() As String = txtItemCode.Text.Split(BARCODE2DSEP)
    ''            If barcode2d.Length > 2 Then Call Barcode2ddetails(txtItemCode.Text) : Exit Sub

    ''            Dim sp() As String = txtItemCode.Text.Split(PRODTAGSEP)
    ''            Dim ScanStr As String = txtItemCode.Text
    ''            If PRODTAGSEP <> "" And txtItemCode.Text <> "" Then
    ''                sp = txtItemCode.Text.Split(PRODTAGSEP)
    ''                txtItemCode.Text = Trim(sp(0))
    ''            End If
    ''            If txtItemCode.Text.StartsWith("#") Then txtItemCode.Text = txtItemCode.Text.Remove(0, 1)
    ''CheckItem:
    ''            If txtItemCode.Text = "" Then
    ''                MsgBox("Item Id should not empty", MsgBoxStyle.Information)
    ''                Exit Sub
    ''            End If
    ''            If PRODTAGSEP <> "" Then
    ''                strSql = " SELECT ITEMID,TAGNO FROM " & cnAdminDb & "..ITEMTAG WHERE TAGKEY = '" & Trim(ScanStr).Replace(PRODTAGSEP, "") & "'"
    ''                If _ItemId <> "" Then
    ''                    strSql += vbCrLf + " AND ITEMID='" & _ItemId & "' "
    ''                End If
    ''                Dim dtItemDet As New DataTable
    ''                da = New OleDbDataAdapter(strSql, cn)
    ''                da.Fill(dtItemDet)
    ''                If dtItemDet.Rows.Count > 0 Then
    ''                    txtItemCode.Text = dtItemDet.Rows(0).Item("ITEMID").ToString
    ''                    txtTagNo.Text = dtItemDet.Rows(0).Item("TAGNO").ToString
    ''                    GoTo LoadItemInfo
    ''                End If
    ''            ElseIf IsNumeric(ScanStr) = True And ScanStr.Contains(PRODTAGSEP) = False Then 'And objGPack.DupCheck("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE CONVERT(VARCHAR,ITEMID) = '" & Val(txtItemCode.Text) & "'" & GetItemQryFilteration()) = True Then
    ''                'SendKeys.Send("{TAB}")
    ''                'MsgBox("Invalid ItemId", MsgBoxStyle.Information)
    ''                Exit Sub
    ''            ElseIf txtItemCode.Text <> "" And objGPack.DupCheck("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE CONVERT(VARCHAR,ITEMID) = '" & Trim(txtItemCode.Text) & "'") = False Then
    ''                strSql = " SELECT ITEMID,TAGNO FROM " & cnAdminDb & "..ITEMTAG WHERE TAGKEY = '" & Trim(txtItemCode.Text) & "'"
    ''                Dim dtItemDet As New DataTable
    ''                da = New OleDbDataAdapter(strSql, cn)
    ''                da.Fill(dtItemDet)
    ''                If dtItemDet.Rows.Count > 0 Then
    ''                    txtItemCode.Text = dtItemDet.Rows(0).Item("ITEMID").ToString
    ''                    txtTagNo.Text = dtItemDet.Rows(0).Item("TAGNO").ToString
    ''                    txtTagNo_KeyPress(Me, New KeyPressEventArgs(Chr(Keys.Enter)))
    ''                    Exit Sub
    ''                    GoTo CheckItem
    ''                End If
    ''            End If
    ''LoadItemInfo:
    ''            If sp.Length > 1 And PRODTAGSEP <> "" And txtItemCode.Text <> "" Then
    ''                txtTagNo.Text = Trim(sp(1))
    ''            End If
    ''            If txtTagNo.Text <> "" Then
    ''                txtTagNo_KeyPress(Me, New KeyPressEventArgs(Chr(Keys.Enter)))
    ''            End If
    ''        End If
    ''    End Sub


    Private Function Barcode2ddetails(ByVal barcode2dstring As String)
        Dim barcode2darray1() As String = barcode2dstring.Split(BARCODE2DSEP)
        txtTagNo.Text = barcode2darray1(2).ToString
        If txtTagNo.Text <> "" Then txtTagNo_KeyPress(Me, New KeyPressEventArgs(Chr(Keys.Enter)))
    End Function

    Private Sub btnImportdata_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImportdata.Click
        Try

            If txtTrayNo.Text = "" Then MsgBox("Please enter the tray no.", MsgBoxStyle.Information) : txtTrayNo.Focus() : Exit Sub

            Dim openDia As New OpenFileDialog
            Dim str As String
            If IO.File.Exists(chkimportdataarry(1).ToString) Then openDia.InitialDirectory = chkimportdataarry(1).ToString
            str = "TEXT(*.txt)|*.txt"
            str += "|Documents(*.doc)|*.doc"
            str += "|All Files(*.*)|*.*"
            openDia.Filter = str
            If openDia.ShowDialog = Windows.Forms.DialogResult.OK Then


                '
                'DATAPATH = openDia.FileName
                DatacheckedasStock(openDia.FileName)
                'Me.SelectNextControl(btnAttachImage, True, True, True, True)
            Else

            End If
        Catch ex As Exception
            MsgBox(E0016, MsgBoxStyle.Exclamation)
        End Try
    End Sub



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
        gridStone.DataSource = dtEmpty
        gridFullView.DataSource = dtEmpty
        strSql = " IF OBJECT_ID('TEMP" & systemId & "STOCKCHECK') IS NOT NULL DROP TABLE TEMP" & systemId & "STOCKCHECK"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strSql += vbCrLf + " select  itemId,TagNo,Pcs,GrsWt,NetWt,CONVERT(VARCHAR(50),'') as itemName,CONVERT(VARCHAR(50),'') as subItemName,CONVERT(VARCHAR(50),'') as CounterName"
        strSql += vbCrLf + " ,Approval,chkTray,CONVERT(VARCHAR(50),'') AS picFileName,0 RESULT"
        strSql += vbCrLf + " INTO TEMP" & systemId & "STOCKCHECK"
        strSql += vbCrLf + " from " & cnAdminDb & "..ITEMTAG as t where 1 =2 "
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
            strSql += vbCrLf + " SELECT @DEFPATH = '" & defaultPic & "'"
            strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "STOCKCHECK "
            strSql += vbCrLf + " (itemId,TagNo,Pcs,GrsWt,NetWt,itemName,subItemName,CounterName,Approval,chkTray"
            strSql += vbCrLf + " ,picFileName,RESULT)"
            strSql += vbCrLf + " select itemId"
            strSql += vbCrLf + " ,TagNo"
            strSql += vbCrLf + " ,isnull(Pcs,0),isnull(GrsWt,0),isnull(NetWt,0)"
            strSql += vbCrLf + " ,(select isnull(itemName,'') from " & cnAdminDb & "..itemmast where itemId = t.itemId)as itemName"
            strSql += vbCrLf + " ,isnull((select isnull(subItemName,'') from " & cnAdminDb & "..subItemMast where subItemid = t.subItemId),'')as subItemName"
            strSql += vbCrLf + " ,isnull((select isnull(itemCtrName,'') from " & cnAdminDb & "..itemCounter where itemCtrId = t.itemCtrId),'')as CounterName"
            strSql += vbCrLf + " ,isnull(Approval,'')"
            strSql += vbCrLf + " ,isnull(chkTray,0)"
            strSql += vbCrLf + " ,@DEFPATH + isnull(PCTFILE,'') AS picFileName,1 RESULT"
            strSql += vbCrLf + " from " & cnAdminDb & "..ITEMTAG as t"
            strSql += vbCrLf + " where itemid=" & Itemid & " and tagno = '" & Tagno & "'"
            strSql += vbCrLf + " AND ISSDATE IS NULL"
            If cmbCostCentre_MAN.Text <> "" And cmbCostCentre_MAN.Text <> "ALL" Then
                strSql += vbCrLf + " AND COSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "')"
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
        btnImportdata.Enabled = False
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
                strSql = " Update " & cnAdminDb & "..ITEMTAG Set chkTray = '" & txtTrayNo.Text & "',chkDate = '" & GetEntryDate(GetServerDate(tran), tran) & "'"
                strSql += vbCrLf + " where itemid = " & Val(itemid) & ""
                strSql += vbCrLf + " and TagNo = '" & tagno & "'"
                If cmbCostCentre_MAN.Text <> "" And cmbCostCentre_MAN.Text <> "ALL" Then
                    strSql += vbCrLf + " AND COSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "')"
                End If
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
                'dtFullView.Rows(rwIndex).Item("CHKTRAY") = txtTrayNo.Text
                CType(gridFullView.DataSource, DataTable).Rows(rwIndex).Item("CHKTRAY") = IIf(txtTrayNo.Text <> "", txtTrayNo.Text, 1)
                'dtFullView.AcceptChanges()
                lstTotal.Items(0).SubItems(1).Text = Val(lstTotal.Items(0).SubItems(1).Text) + 1
                lstTotal.Items(0).SubItems(2).Text = (Val(lstTotal.Items(0).SubItems(2).Text) + Val(gridFullView.Item("Pcs", rwIndex).Value)).ToString
                lstTotal.Items(0).SubItems(3).Text = (Val(lstTotal.Items(0).SubItems(3).Text) + Val(gridFullView.Item("GrsWt", rwIndex).Value)).ToString
                lstTotal.Items(0).SubItems(4).Text = (Val(lstTotal.Items(0).SubItems(4).Text) + Val(gridFullView.Item("NetWt", rwIndex).Value)).ToString
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

    Private Sub txtTrayNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtTrayNo.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then

            If chkimportdataarry(0).ToString = "N" Then
                btnImportdata.Visible = False
            Else
                btnImportdata.Visible = True
                btnImportdata.Enabled = True
                If chkimportdataarry(1).ToString = "" Then btnImportdata.Enabled = False
                If chkimportdataarry(2).ToString = "" Then btnImportdata.Enabled = False
                If chkimportdataarry(3).ToString = "" Then btnImportdata.Enabled = False
            End If
        End If

    End Sub

    Private Sub CmbMetal_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmbMetal.SelectedIndexChanged
        If CmbMetal.Text <> "ALL" And CmbMetal.Text <> "" Then
            cmbItemName.Items.Clear()
            strSql = " Select itemName from " & cnAdminDb & "..itemMast "
            strSql += vbCrLf + " where metalid=(select metalid from " & cnAdminDb & "..metalmast where metalname='" & CmbMetal.Text & "')"
            strSql += vbCrLf + " order by itemName"
            cmbItemName.Items.Add("ALL")
            objGPack.FillCombo(strSql, cmbItemName, False)
            cmbItemName.Text = "ALL"
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridFullView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "Stock Checking", gridFullView, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Public Function isPointingDeviceAttached() As Boolean
        Dim searcher As ManagementObjectSearcher = New ManagementObjectSearcher("root\CIMV2", "SELECT * FROM Win32_PointingDevice")
        Dim devCount As Integer = 0
        For Each obj As ManagementObject In searcher.[Get]()
            If obj("Status").ToString().Contains("OK") Then devCount += 1
        Next
        Return devCount > 0
    End Function

    Public Function isKeyboardAttached() As Boolean
        Dim searcher As ManagementObjectSearcher = New ManagementObjectSearcher("root\CIMV2", "SELECT * FROM Win32_Keyboard")
        Dim devCount As Integer = 0
        For Each obj As ManagementObject In searcher.[Get]()
            If obj("Status").ToString().Contains("OK") Then devCount += 1
        Next
        Return devCount > 0
    End Function

    Private Sub btnConnect_Click(sender As Object, e As EventArgs) Handles btnConnect.Click
        Try
            Cursor = Cursors.WaitCursor
            ConnectByEthernet()
            MsgBox("Reader Connected")
        Catch ex As Exception
            MsgBox(ex.Message.ToString)
        Finally
            Cursor = Cursors.Default
        End Try

        Exit Sub

        Try
            Cursor = Cursors.WaitCursor
            _SPC_RFID.tagsDelegate = AddressOf tagAdd
            _SPC_RFID.deviceStatusDelegate = AddressOf devStAdd
            _SPC_RFID.errorDelegate = AddressOf errorAdd
            _SPC_RFID.scanCompleteDelegate = AddressOf scanComplted
            _SPC_RFID.setReader(ReaderMake.Dtr)
            MsgBox("Reader Connected")
        Catch ex As Exception
            MsgBox(ex.Message.ToString)
        Finally
            Cursor = Cursors.Default
        End Try
    End Sub
    Public Sub tagAdd(a As String, r As String)
        Me.Invoke(DirectCast(Sub()
                                 'a = If((Not String.IsNullOrWhiteSpace(txtTagNo.Text)), Convert.ToString(vbCr & vbLf) & a, a)
                                 'txtTagsList.AppendText(a)
                                 'txtTagNo.Text = a
                                 MarkTag(a)
                                 'MsgBox(a)
                                 lblCount.Text = "Scaned Count : " + _SPC_RFID.Tags_id.Count.ToString()

                                 'txtTagsList.ScrollToCaret()

                             End Sub, MethodInvoker))
    End Sub


    Public Sub scanComplted(tag As ArrayList)
        'Me.Invoke(DirectCast(Sub() lblmsg.Text = "ScanCompleted:" + tag.Count.ToString() + " :tags ", MethodInvoker))
    End Sub


    Public Sub errorAdd(a As String)
        'lblError.Text = a;
        Me.Invoke(DirectCast(Sub()

                             End Sub, MethodInvoker))

    End Sub

    Public Sub devStAdd(a As String)
        'Me.Invoke(DirectCast(Sub() lblConnection.Text = a, MethodInvoker))
    End Sub
    Private Sub btnScan_Click(sender As Object, e As EventArgs) Handles btnScan.Click
        'Try
        '    SacnByEthernet()
        'Catch ex As Exception
        '    MsgBox(ex.Message.ToString)
        'End Try

        'Exit Sub


        If txtTrayNo.Text.Trim = "" Then
            'MsgBox("Tray No Should Not Empty", MsgBoxStyle.Exclamation)
            txtTrayNo.Focus()
            Exit Sub
        End If
        Try
            _SPC_RFID.tagsDelegate = AddressOf tagAdd
            _SPC_RFID.deviceStatusDelegate = AddressOf devStAdd
            _SPC_RFID.errorDelegate = AddressOf errorAdd
            _SPC_RFID.scanCompleteDelegate = AddressOf scanComplted

            If btnScan.Text.Equals("Start Scan") Then
                _SPC_RFID.AllowDuplicateTagScan = False
                _SPC_RFID.SetPower(31)
                _SPC_RFID.SetDuty(90)
                Dim result As Boolean = _SPC_RFID.startScan(True)
                If result Then
                    btnScan.Text = "Stop Scan"
                End If
            Else
                _SPC_RFID.stopScan()
                btnScan.Text = "Start Scan"
            End If
        Catch ex As Exception
            MsgBox(ex.Message.ToString)
        End Try
    End Sub


    Public Sub ConnectByEthernet()
        'RfId.CatchData()
    End Sub

    Public Sub SacnByEthernet()
        'Timer1.Start()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        'Dim str As New List(Of String)
        'str = RfId.list
        'If str Is Nothing Then Exit Sub
        'If str.Count > 0 Then
        '    txtTagNo.Text = str(0).ToString
        'End If
    End Sub
End Class


