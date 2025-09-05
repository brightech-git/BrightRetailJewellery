Imports System.Data.OleDb
Imports System.Globalization
Imports System.Math

Public Class frmTagDetailUpdator
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim SALVALUEROUND As Decimal = Val(GetAdmindbSoftValue("STKSALVALUEROUND", "0"))
    Dim _MCONGRSNET As Boolean = IIf(GetAdmindbSoftValuefromDt(dtSoftKeys, "MC_ON_GRSNET", "Y") = "Y", True, False)
    Dim _WASTONGRSNET As Boolean = IIf(GetAdmindbSoftValuefromDt(dtSoftKeys, "WAST_ON_GRSNET", "Y") = "Y", True, False)
    Dim McWithWastage As Boolean = IIf(GetAdmindbSoftValue("MCWITHWASTAGE", "N") = "Y", True, False)
    Dim CostCentre As Boolean = IIf(GetAdmindbSoftValue("CostCentre", "N") = "Y", True, False)
    
    Private Sub frmTagDetailUpdator_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If tabMain.SelectedTab.Name = tabView.Name Then
                btnBack_Click(Me, New EventArgs)
            End If
        End If
    End Sub

    Private Sub frmTagDetailUpdator_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmTagDetailUpdator_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGeneral.Left, Me.tabGeneral.Top, Me.tabGeneral.Width, Me.tabGeneral.Height))
        grpContrainer.Location = New Point((ScreenWid - grpContrainer.Width) / 2, ((ScreenHit - 128) - grpContrainer.Height) / 2)
        pnlContainer_OWN.Location = New Point((ScreenWid - pnlContainer_OWN.Width) / 2, ((ScreenHit - 128) - pnlContainer_OWN.Height) / 2)
        tabMode.ItemSize = New System.Drawing.Size(1, 1)
        'tabMode.SendToBack()
        Me.tabMode.Region = New Region(New RectangleF(Me.tabWastage.Left, Me.tabWastage.Top, Me.tabWastage.Width, Me.tabWastage.Height))

        cmbCalcType.Items.Add("WEIGHT")
        cmbCalcType.Items.Add("RATE")
        'cmbCalcType.Items.Add("BOTH")
        cmbCalcType.Items.Add("FIXED")
        cmbCalcType.Items.Add("METAL RATE")
        cmbCalcType.Text = "RATE"

        cmbItem.Items.Clear()
        cmbItem.Items.Add("ALL")
        strSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = ''"
        strSql += GetItemQryFilteration("S")
        strSql += " AND ACTIVE = 'Y'"
        objGPack.FillCombo(strSql, cmbItem, False, False)
        cmbItem.Text = "ALL"
        strSql = " SELECT 'ALL' ITEMCTRNAME,0 ITEMCTRID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT ITEMCTRNAME,ITEMCTRID,2 RESULT FROM " & cnAdminDb & "..ITEMCOUNTER "
        strSql += " ORDER BY RESULT,ITEMCTRNAME"
        Dim dtcounter As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtcounter)
        BrighttechPack.GlobalMethods.FillCombo(chkcmbcounter, dtcounter, "ITEMCTRNAME", , "ALL")

        cmbitemtype.Items.Clear()
        cmbitemtype.Items.Add("ALL")
        strSql = " SELECT NAME FROM " & cnAdminDb & "..ITEMTYPE "
        objGPack.FillCombo(strSql, cmbitemtype, False, False)
        

        strSql = " SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE ACTIVE = 'Y'"
        objGPack.FillCombo(strSql, cmbDesignerOld)
        objGPack.FillCombo(strSql, cmbDesignerNew)
        Dim dt As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        For Each ro As DataRow In dt.Rows
            chkLstDesigner.Items.Add(ro(0).ToString)
        Next

        strSql = " SELECT 'ALL'COSTNAME " + vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE isnull(ACTIVE,'') <> 'N'"
        objGPack.FillCombo(strSql, cmbCostCentre)
        cmbCostCentre.Text = "ALL"

        strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE isnull(ACTIVE,'') <> 'N'"
        objGPack.FillCombo(strSql, cmbCostcentre_Old)
        objGPack.FillCombo(strSql, cmbCostcentre_New)

        strSql = " SELECT 'ALL' ITEMNAME,'ALL' ITEMID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT ITEMNAME,CONVERT(VARCHAR,ITEMID),2 RESULT FROM " & cnAdminDb & "..ITEMMAST"
        strSql += " WHERE ACTIVE = 'Y'"
        strSql += " AND METALID IN ('T','D')"
        strSql += " ORDER BY RESULT,ITEMNAME"
        Dim dtsItem = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtsItem)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbItem, dtsItem, "ITEMNAME", , "ALL")

        cmbCostCentre.Enabled = CostCentre
        btnNew_Click(Me, New EventArgs)
    End Sub


    Private Sub cmbMode_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbMode.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub cmbMode_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbMode.SelectedIndexChanged
        objGPack.TextClear(tabMode)
        Select Case cmbMode.Text.ToUpper
            Case "WASTAGE"
                tabMode.SelectedTab = tabWastage
            Case "MC"
                tabMode.SelectedTab = tabMc
            Case "RATE"
                tabMode.SelectedTab = tabRate
            Case "PUR RATE"
                tabMode.SelectedTab = tabRate
            Case "SUBITEM"
                tabMode.SelectedTab = tabSubItem
            Case "DESIGNER"
                tabMode.SelectedTab = tabDesigner
            Case "COSTCENTRE"
                tabMode.SelectedTab = tabCostid
            Case "SALE VALUE"
                tabMode.SelectedTab = tabSaleValue
            Case "TABLE"
                tabMode.SelectedTab = tabTable
            Case "STONERATE"
                tabMode.SelectedTab = tabStoneRate
            Case "ITEMSIZE"
                Dim titemid As Integer = Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbItem.Text & "'", , , ))
                If titemid > 0 Then
                    tabMode.SelectedTab = TabITemSize
                Else
                    MsgBox("Select any item for this type", MsgBoxStyle.Information)
                    cmbItem.Focus()
                    Exit Sub
                End If
            Case Else
                If Val(objGPack.GetSqlValue("SELECT COUNT(*) FROM " & cnAdminDb & "..OTHERMASTERENTRY WHERE ISNULL(ACTIVE,'')<>'N'", , "", )) > 0 Then
                    tabMode.SelectedTab = tabOtherMaster
                    Dim MISCID As Integer = Val(objGPack.GetSqlValue("SELECT MISCID FROM " & cnAdminDb & "..OTHERMASTERENTRY WHERE MISCNAME='" & cmbMode.Text & "'", "", "", ))
                    loadOtherMaster(MISCID)
                    lblOldOther.Text = "Old " & cmbMode.Text
                    lblNewOther.Text = "New " & cmbMode.Text
                    grpOther.Text = cmbMode.Text
                Else
                    tabMode.SelectedTab = tabWastage
                End If
        End Select
        cmbMode.Select()
    End Sub
    Private Function loadOtherMaster(ByVal miscid As Integer)
        strSql = " SELECT NAME FROM " & cnAdminDb & "..OTHERMASTER WHERE MISCID ='" & miscid & "' AND ISNULL(ACTIVE,'')<>'N' "
        strSql += " ORDER BY NAME"
        objGPack.FillCombo(strSql, cmbOldOther, True, False)
        objGPack.FillCombo(strSql, cmbNewOther_MAN, True, False)
    End Function
    Private Sub GenRateGrid()
        Dim ftrDesigner As String = GetCheckedItemStr(chkLstDesigner)
        Dim ftrcounter As String = GetSelectedCounderid(chkcmbcounter, True)
        strSql = " SELECT T.SNO,"
        strSql += vbCrLf + "  (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS ITEM"
        strSql += vbCrLf + "  ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = T.SUBITEMID)AS SUBITEM"
        If cmbMode.Text.ToUpper = "RATE" Then
            strSql += vbCrLf + "  ,T.TAGNO,T.PCS,T.RATE [OLD RATE]"
            strSql += vbCrLf + "  ,T.SALVALUE [OLD VALUE]"
            If Val(txtRateOld_AMT.Text) <> 0 Or Val(txtRateNew_AMT.Text) <> 0 Then
                strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2)," & Val(txtRateNew_AMT.Text) & ") AS [NEW RATE]"
                strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),T.PCS*" & Val(txtRateNew_AMT.Text) & ") AS [NEW VALUE]"
            ElseIf Val(txtRateIncPer_PER.Text) <> 0 Then
                strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2)," & Val(txtRateIncPer_PER.Text) & ") AS [INC%]"
                If chkRateRound.Checked Then
                    strSql += vbCrLf + "  ,ROUND(CONVERT(NUMERIC(15,2),T.RATE+(" & Val(txtRateIncPer_PER.Text) & "*(T.RATE/100))),-1) AS [NEW RATE]"
                    strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),T.PCS*(T.RATE+ROUND((" & Val(txtRateIncPer_PER.Text) & "*(T.RATE/100)),-1))) AS [NEW VALUE]"
                ElseIf chkRateRound1.Checked Then
                    strSql += vbCrLf + "  ,ROUND(CONVERT(NUMERIC(15,2),T.RATE+(" & Val(txtRateIncPer_PER.Text) & "*(T.RATE/100))),0) AS [NEW RATE]"
                    strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),ROUND(T.PCS*(T.RATE+(" & Val(txtRateIncPer_PER.Text) & "*(T.RATE/100))),0)) AS [NEW VALUE]"
                Else
                    strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),T.RATE+(" & Val(txtRateIncPer_PER.Text) & "*(T.RATE/100))) AS [NEW RATE]"
                    strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),T.PCS*(T.RATE+(" & Val(txtRateIncPer_PER.Text) & "*(T.RATE/100)))) AS [NEW VALUE]"
                End If
            End If
        ElseIf UCase(cmbMode.Text) = "SUBITEM" Then
            strSql += vbCrLf + "  ,'" & cmbSubItemOld_MAN.Text & "' AS [OLD SUBITEM]"
            strSql += vbCrLf + "  ,'" & cmbSubItemNew_MAN.Text & "' AS [NEW SUBITEM]"
            strSql += vbCrLf + "  ,SALVALUE"
        ElseIf UCase(cmbMode.Text) = "DESIGNER" Then
            strSql += vbCrLf + "  ,'" & cmbDesignerOld.Text & "' AS [OLD DESIGNER]"
            strSql += vbCrLf + "  ,'" & cmbDesignerNew.Text & "' AS [NEW DESIGNER]"
        ElseIf UCase(cmbMode.Text) = "COSTCENTRE" Then
            strSql += vbCrLf + "  ,'" & cmbCostcentre_Old.Text & "' AS [OLD COSTCENTRE]"
            strSql += vbCrLf + "  ,'" & cmbCostcentre_New.Text & "' AS [NEW COSTCENTRE]"

        ElseIf cmbMode.Text.ToUpper = "PUR RATE" Then
            strSql += vbCrLf + "  ,T.TAGNO,T.PCS,P.PURRATE [OLD RATE]"
            strSql += vbCrLf + "  ,P.PURVALUE [OLD VALUE]"
            If Val(txtRateOld_AMT.Text) <> 0 Or Val(txtRateNew_AMT.Text) <> 0 Then
                strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2)," & Val(txtRateNew_AMT.Text) & ") AS [NEW RATE]"
                strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),T.PCS*" & Val(txtRateNew_AMT.Text) & ") AS [NEW VALUE]"
            ElseIf Val(txtRateIncPer_PER.Text) <> 0 Then
                strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2)," & Val(txtRateIncPer_PER.Text) & ") AS [INC%]"
                If chkRateRound.Checked Then
                    strSql += vbCrLf + "  ,ROUND(CONVERT(NUMERIC(15,2),P.PURRATE+(" & Val(txtRateIncPer_PER.Text) & "*(P.PURRATE/100))),-1) AS [NEW RATE]"
                    strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),T.PCS*(P.PURRATE+ROUND((" & Val(txtRateIncPer_PER.Text) & "*(P.PURRATE/100)),-1))) AS [NEW VALUE]"
                ElseIf chkRateRound.Checked Then
                    strSql += vbCrLf + "  ,ROUND(CONVERT(NUMERIC(15,2),P.PURRATE+(" & Val(txtRateIncPer_PER.Text) & "*(P.PURRATE/100))),0) AS [NEW RATE]"
                    strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),T.PCS*(P.PURRATE+ROUND((" & Val(txtRateIncPer_PER.Text) & "*(P.PURRATE/100)),0))) AS [NEW VALUE]"
                Else
                    strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),P.PURRATE+(" & Val(txtRateIncPer_PER.Text) & "*(P.PURRATE/100))) AS [NEW RATE]"
                    strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),T.PCS*(P.PURRATE+(" & Val(txtRateIncPer_PER.Text) & "*(P.PURRATE/100)))) AS [NEW VALUE]"
                End If
            End If
        ElseIf UCase(cmbMode.Text) = "ITEMSIZE" Then
            strSql += vbCrLf + "  ,'" & CmbOldItemsize_MAN.Text & "' AS [OLD ITEMSIZE]"
            strSql += vbCrLf + "  ,'" & CmbNewItemSize_MAN.Text & "' AS [NEW ITEMSIZE]"
        End If
        strSql += vbCrLf + "   FROM " & cnAdminDb & "..ITEMTAG T"
        If cmbMode.Text.ToUpper = "PUR RATE" Then
            strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..PURITEMTAG AS P ON P.TAGSNO = T.SNO"
        End If
        strSql += vbCrLf + "  WHERE T.ISSDATE IS NULL"
        If Not cnCentStock Then strSql += vbCrLf + "  AND T.COMPANYID = '" & GetStockCompId() & "'"
        If UCase(cmbMode.Text) = "SUBITEM" Then
            strSql += vbCrLf + "  AND T.SUBITEMID = (SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItemOld_MAN.Text & "' and itemid = (select itemid from " & cnAdminDb & "..itemmast where itemname = '" & cmbItem.Text & "'))"
        End If

        If UCase(cmbMode.Text) = "COSTCENTRE" Then
            strSql += vbCrLf + "  AND T.TCOSTID= (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME= '" & cmbCostcentre_Old.Text & "' )"
        End If

        If cmbMode.Text.ToUpper = "RATE" Then
            If Val(txtRateOld_AMT.Text) <> 0 Or Val(txtRateNew_AMT.Text) <> 0 Then
                strSql += vbCrLf + "  AND T.RATE = " & Val(txtRateOld_AMT.Text) & ""
            End If
        ElseIf cmbMode.Text.ToUpper = "PUR RATE" Then
            If Val(txtRateOld_AMT.Text) <> 0 Or Val(txtRateNew_AMT.Text) <> 0 Then
                strSql += vbCrLf + "  AND P.PURRATE = " & Val(txtRateOld_AMT.Text) & ""
            End If
        End If
        If UCase(cmbMode.Text) = "ITEMSIZE" Then
            strSql += vbCrLf + "  AND T.SIZEID = (SELECT SIZEID FROM " & cnAdminDb & "..ITEMSIZE WHERE SIZENAME = '" & CmbOldItemsize_MAN.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem.Text & "'))"
        End If
        If cmbItem.Text <> "ALL" And cmbItem.Text <> "" Then
            strSql += vbCrLf + "  AND T.ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem.Text & "')"
        End If
        If cmbCostCentre.Text <> "ALL" And cmbCostCentre.Text <> "" Then
            strSql += vbCrLf + "  AND T.COSTID = (SELECT TOP 1 COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre.Text & "')"
        End If
        If cmbSubItem_MAN.Text <> "ALL" And cmbSubItem_MAN.Text <> "" Then
            strSql += vbCrLf + "  AND T.SUBITEMID = (SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem_MAN.Text & "' and itemid = (select itemid from " & cnAdminDb & "..itemmast where itemname = '" & cmbItem.Text & "'))"
        End If
        If cmbitemtype.Text <> "ALL" And cmbitemtype.Text <> "" Then
            strSql += " AND T.ITEMTYPEID = (SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & cmbitemtype.Text & "')"
        End If
        If txtStyleNo.Text <> "" Then
            strSql += " AND T.STYLENO = '" & txtStyleNo.Text & "'"
        End If
        If ftrDesigner <> Nothing Then
            strSql += vbCrLf + "  AND T.DESIGNERID IN (SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME IN (" & ftrDesigner & "))"
        End If
        If chkcmbcounter.Text <> "" And chkcmbcounter.Text <> "ALL" Then
            strSql += " AND T.ITEMCTRID IN (" & ftrcounter & ")"
        End If
        strSql += vbCrLf + "  AND T.SALEMODE = '" & Mid(cmbCalcType.Text, 1, 1) & "'"
        strSql += vbCrLf + "  ORDER BY ITEM,T.TAGNO"
        Dim dtGrid As New DataTable
        Dim dtCol As New DataColumn("CHECK", GetType(Boolean))
        dtCol.DefaultValue = True
        dtGrid.Columns.Add(dtCol)

        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        dtGrid.AcceptChanges()
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        tabView.Show()
        gridView.DataSource = dtGrid
        FormatGridColumns(gridView, False, False)
        gridView.Columns("CHECK").ReadOnly = False
        gridView.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        With gridView
            .Columns("SNO").Visible = False
            If Val(txtRateOld_AMT.Text) <> 0 Or Val(txtRateNew_AMT.Text) <> 0 Then
                .Columns("OLD RATE").DefaultCellStyle.BackColor = Color.Bisque
                .Columns("OLD VALUE").DefaultCellStyle.BackColor = Color.Bisque
                .Columns("NEW RATE").DefaultCellStyle.BackColor = Color.Lavender
                .Columns("NEW VALUE").DefaultCellStyle.BackColor = Color.Lavender
                If cmbMode.Text.ToUpper = "PUR RATE" Then
                    .Columns("OLD RATE").HeaderText = "OLD PRATE"
                    .Columns("OLD VALUE").HeaderText = "OLD PVALUE"
                    .Columns("NEW RATE").HeaderText = "NEW PRATE"
                    .Columns("NEW VALUE").HeaderText = "NEW PVALUE"
                End If
            ElseIf cmbMode.Text.ToUpper = "SUBITEM" Then
                .Columns("OLD SUBITEM").DefaultCellStyle.BackColor = Color.Bisque
                .Columns("NEW SUBITEM").DefaultCellStyle.BackColor = Color.Lavender
            ElseIf cmbMode.Text.ToUpper = "ITEMSIZE" Then
                .Columns("OLD ITEMSIZE").DefaultCellStyle.BackColor = Color.Bisque
                .Columns("NEW ITEMSIZE").DefaultCellStyle.BackColor = Color.Lavender
            ElseIf cmbMode.Text.ToUpper = "DESIGNER" Then
                .Columns("OLD DESIGNER").DefaultCellStyle.BackColor = Color.Bisque
                .Columns("NEW DESIGNER").DefaultCellStyle.BackColor = Color.Lavender
            ElseIf cmbMode.Text.ToUpper = "COSTCENTRE" Then
                .Columns("OLD COSTCENTRE").DefaultCellStyle.BackColor = Color.Bisque
                .Columns("NEW COSTCENTRE").DefaultCellStyle.BackColor = Color.Lavender
            End If
        End With
    End Sub

    Private Sub GenStnRateGrid()
        Dim ftrDesigner As String = GetCheckedItemStr(chkLstDesigner)
        Dim ftrcounter As String = GetSelectedCounderid(chkcmbcounter, True)
        strSql = " SELECT P.SNO,"
        strSql += vbCrLf + "  I.ITEMNAME AS ITEM ,SI.SUBITEMNAME AS SUBITEM,IST.ITEMNAME AS STONEITEM ,SIS.SUBITEMNAME AS SUBITEMSTONE"
        If cmbMode.Text.ToUpper = "STONERATE" Then
            strSql += vbCrLf + "  ,P.TAGNO,P.STNPCS,P.STNWT,P.STNRATE [OLD RATE]"
            strSql += vbCrLf + "  ,P.STNAMT [OLD VALUE]," & Val(txtStnRateNew.Text) & " AS [NEW RATE]"
            strSql += vbCrLf + "  ,ROUND(CONVERT(NUMERIC(15,2)," & Val(txtStnRateNew.Text) & "*(CASE WHEN CALCMODE ='P' THEN P.STNPCS ELSE P.STNWT END)),0)AS [NEW VALUE]"
        End If

        strSql += vbCrLf + "  ,T.SNO AS TAGSNO FROM " & cnAdminDb & "..ITEMTAG T"
        strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMTAGSTONE AS P ON P.TAGSNO = T.SNO"
        strSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..ITEMMAST AS I ON I.ITEMID= T.ITEMID"
        strSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SI ON SI.SUBITEMID= T.SUBITEMID"
        strSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..ITEMMAST AS IST ON IST.ITEMID= P.STNITEMID"
        strSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SIS ON SIS.SUBITEMID= P.STNSUBITEMID"

        strSql += vbCrLf + "  WHERE T.ISSDATE IS NULL"
        If Not cnCentStock Then strSql += vbCrLf + "  AND T.COMPANYID = '" & GetStockCompId() & "'"

        If Val(txtStnRateOld.Text) <> 0 Then strSql += vbCrLf + "  AND P.STNRATE = " & Val(txtStnRateOld.Text) & ""

        If cmbItem.Text <> "ALL" And cmbItem.Text <> "" Then
            strSql += vbCrLf + "  AND T.ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem.Text & "')"
        End If
        If cmbCostCentre.Text <> "ALL" And cmbCostCentre.Text <> "" Then
            strSql += vbCrLf + "  AND T.COSTID = (SELECT TOP 1 COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre.Text & "')"
        End If
        If cmbitemtype.Text <> "ALL" And cmbitemtype.Text <> "" Then
            strSql += " AND T.ITEMTYPEID = (SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & cmbitemtype.Text & "')"
        End If
        If cmbSubItem_MAN.Text <> "ALL" And cmbSubItem_MAN.Text <> "" Then
            strSql += vbCrLf + "  AND T.SUBITEMID = (SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem_MAN.Text & "' and itemid = (select itemid from " & cnAdminDb & "..itemmast where itemname = '" & cmbItem.Text & "'))"
        End If
        If txtStyleNo.Text <> "" Then strSql += " AND T.STYLENO = '" & txtStyleNo.Text & "'"
        If ftrDesigner <> Nothing Then
            strSql += vbCrLf + "  AND T.DESIGNERID IN (SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME IN (" & ftrDesigner & "))"
        End If
        If chkcmbcounter.Text <> "" And chkcmbcounter.Text <> "ALL" Then strSql += " AND T.ITEMCTRID IN (" & ftrcounter & ")"
        If chkCmbItem.Text <> "" And chkCmbItem.Text <> "ALL" Then strSql += " AND P.STNITEMID IN (SELECT itemid FROM " & cnAdminDb & "..itemmast WHERE itemname IN (" & GetQryString(chkCmbItem.Text) & "))"
        If chkSubItem.Text <> "" And chkSubItem.Text <> "ALL" Then strSql += " AND P.STNSUBITEMID IN (SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME IN (" & GetQryString(chkSubItem.Text) & "))"
        strSql += vbCrLf + "  ORDER BY ITEM,T.TAGNO"
        Dim dtGrid As New DataTable
        Dim dtCol As New DataColumn("CHECK", GetType(Boolean))
        dtCol.DefaultValue = True
        dtGrid.Columns.Add(dtCol)

        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        dtGrid.AcceptChanges()
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        tabView.Show()
        gridView.DataSource = dtGrid
        FormatGridColumns(gridView, False, False)
        gridView.Columns("CHECK").ReadOnly = False
        gridView.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        With gridView
            .Columns("SNO").Visible = False
            If Val(txtStnRateOld.Text) <> 0 Then
                .Columns("OLD RATE").DefaultCellStyle.BackColor = Color.Bisque
                .Columns("OLD VALUE").DefaultCellStyle.BackColor = Color.Bisque
                .Columns("NEW RATE").DefaultCellStyle.BackColor = Color.Lavender
                .Columns("NEW VALUE").DefaultCellStyle.BackColor = Color.Lavender
            End If
        End With
    End Sub
    Private Sub GenWeightGrid()
        Dim ftrDesigner As String = GetCheckedItemStr(chkLstDesigner)
        Dim ftrcounter As String = GetSelectedCounderid(chkcmbcounter, True)
        strSql = ""
        If cmbCalcType.Text = "WEIGHT" Then
            strSql = " DECLARE @WTFROM AS FLOAT,@WTTO AS FLOAT"
            strSql += vbCrLf + " SET @WTFROM =" & Val(txtFromWt.Text) & ""
            strSql += vbCrLf + " SET @WTTO =" & Val(txtToWt.Text) & ""
        End If
        strSql += vbCrLf + " SELECT SNO,"
        strSql += vbCrLf + " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS ITEM"
        strSql += vbCrLf + " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = T.SUBITEMID)AS SUBITEM"
        strSql += vbCrLf + " ,TAGNO,PCS,GRSWT,LESSWT,NETWT"
        If Val(txtWastagePerOld_PER.Text) <> 0 Or Val(txtWastagePerNew_PER.Text) <> 0 Or _
        Val(txtWastageOld_WET.Text) <> 0 Or Val(txtWastageNew_WET.Text) <> 0 Then
            strSql += vbCrLf + " ,MAXWASTPER [OLD WAST%],MAXWAST [OLD WASTAGE]"
            If Val(txtWastagePerOld_PER.Text) <> 0 Or Val(txtWastagePerNew_PER.Text) <> 0 Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2)," & Val(txtWastagePerNew_PER.Text) & ")AS [NEW WAST%]"
                If _WASTONGRSNET Then
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3)," & Val(txtWastagePerNew_PER.Text) & "*(CASE WHEN GRSNET = 'G' THEN GRSWT ELSE NETWT END /100.00)) [NEW WASTAGE]"
                Else
                    strSql += vbCrLf + " , CONVERT(NUMERIC(15,3)," & Val(txtWastagePerNew_PER.Text) & "*(CASE WHEN (CASE WHEN T.SUBITEMID <> 0 THEN (SELECT MCCALC FROM " & cnAdminDb & "..SUBITEMMAST AS S WHERE S.ITEMID=T.ITEMID AND S.SUBITEMID=T.SUBITEMID)"
                    'strSql += vbCrLf + " ELSE (SELECT MCCALC FROM " & cnAdminDb & "..ITEMMAST AS S WHERE S.ITEMID=T.ITEMID) END)='N' THEN NETWT ELSE GRSWT END)/100.00)[NEW WASTAGE] "
                    strSql += vbCrLf + " ELSE (SELECT MCCALC FROM " & cnAdminDb & "..ITEMMAST AS S WHERE S.ITEMID=T.ITEMID) END)='N' THEN NETWT ELSE (CASE WHEN ISNULL(GRSNET,'G')='G' THEN GRSWT ELSE NETWT END) END)/100.00)[NEW WASTAGE] "
                End If
            ElseIf Val(txtWastageOld_WET.Text) <> 0 Or Val(txtWastageNew_WET.Text) <> 0 Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),0)AS [NEW WAST%]"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2)," & Val(txtWastageNew_WET.Text) & ")AS [NEW WASTAGE]"
            End If
        ElseIf Val(txtMcPerGrmOld_AMT.Text) <> 0 Or Val(txtMcPerGrmNew_AMT.Text) <> 0 Or _
        Val(txtMcOld_AMT.Text) <> 0 Or Val(txtMcNew_AMT.Text) <> 0 Then 'MC
            strSql += vbCrLf + " ,MAXMCGRM [OLD MCGRM],MAXMC [OLD MC]"
            If Val(txtMcPerGrmOld_AMT.Text) <> 0 Or Val(txtMcPerGrmNew_AMT.Text) <> 0 Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2)," & Val(txtMcPerGrmNew_AMT.Text) & ")AS [NEW MCGRM%]"
                If _MCONGRSNET Then
                    If McWithWastage Then
                        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3)," & Val(txtMcPerGrmNew_AMT.Text) & "*((CASE WHEN GRSNET = 'G' THEN GRSWT ELSE NETWT END)+ ISNULL(MAXWAST,0))) [NEW MC]"
                    Else
                        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3)," & Val(txtMcPerGrmNew_AMT.Text) & "*(CASE WHEN GRSNET = 'G' THEN GRSWT ELSE NETWT END)) [NEW MC]"
                    End If
                Else
                    If McWithWastage Then
                        strSql += vbCrLf + " , CONVERT(NUMERIC(15,3)," & Val(txtMcPerGrmNew_AMT.Text) & "*(CASE WHEN (CASE WHEN T.SUBITEMID <> 0 THEN (SELECT MCCALC FROM " & cnAdminDb & "..SUBITEMMAST AS S WHERE S.ITEMID=T.ITEMID AND S.SUBITEMID=T.SUBITEMID)"
                        strSql += vbCrLf + " ELSE (SELECT MCCALC FROM " & cnAdminDb & "..ITEMMAST AS S WHERE S.ITEMID=T.ITEMID) END)='N' THEN NETWT+ ISNULL(MAXWAST,0) ELSE GRSWT+ ISNULL(MAXWAST,0) END))[NEW MC] "
                    Else
                        strSql += vbCrLf + " , CONVERT(NUMERIC(15,3)," & Val(txtMcPerGrmNew_AMT.Text) & "*(CASE WHEN (CASE WHEN T.SUBITEMID <> 0 THEN (SELECT MCCALC FROM " & cnAdminDb & "..SUBITEMMAST AS S WHERE S.ITEMID=T.ITEMID AND S.SUBITEMID=T.SUBITEMID)"
                        strSql += vbCrLf + " ELSE (SELECT MCCALC FROM " & cnAdminDb & "..ITEMMAST AS S WHERE S.ITEMID=T.ITEMID) END)='N' THEN NETWT ELSE GRSWT END))[NEW MC] "
                    End If
                End If
            ElseIf Val(txtMcOld_AMT.Text) <> 0 Or Val(txtMcNew_AMT.Text) <> 0 Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),0)AS [NEW MCGRM%]"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2)," & Val(txtMcNew_AMT.Text) & ")AS [NEW MC]"
            End If
        ElseIf UCase(cmbMode.Text) = "SUBITEM" Then
            strSql += vbCrLf + " ,'" & cmbSubItemOld_MAN.Text & "' AS [OLD SUBITEM]"
            strSql += vbCrLf + " ,'" & cmbSubItemNew_MAN.Text & "' AS [NEW SUBITEM]"
        ElseIf UCase(cmbMode.Text) = "ITEMSIZE" Then
            strSql += vbCrLf + " ,'" & CmbOldItemsize_MAN.Text & "' AS [OLD ITEMSIZE]"
            strSql += vbCrLf + " ,'" & CmbNewItemSize_MAN.Text & "' AS [NEW ITEMSIZE]"
        ElseIf UCase(cmbMode.Text) = "DESIGNER" Then
            strSql += vbCrLf + " ,'" & cmbDesignerOld.Text & "' AS [OLD DESIGNER]"
            strSql += vbCrLf + " ,'" & cmbDesignerNew.Text & "' AS [NEW DESIGNER]"
        ElseIf UCase(cmbMode.Text) = "COSTCENTRE" Then
            strSql += vbCrLf + " ,'" & cmbCostcentre_Old.Text & "' AS [OLD COSTCENTRE]"
            strSql += vbCrLf + " ,'" & cmbCostcentre_New.Text & "' AS [NEW COSTCENTRE]"
        ElseIf UCase(cmbMode.Text) = "TABLE" Then
            strSql += vbCrLf + " ,'" & cmbTableold.Text & "' AS [OLD TABLE]"
            strSql += vbCrLf + " ,'" & cmbTablenew.Text & "' AS [NEW TABLE]"
        ElseIf UCase(cmbMode.Text) = "RATE" Then
            strSql += vbCrLf + " ,'" & txtRateOld_AMT.Text & "' AS [OLD RATE]"
            strSql += vbCrLf + " ,'" & txtRateNew_AMT.Text & "' AS [NEW RATE]"
            strSql += vbCrLf + " ,SALVALUE AS [OLD VALUE]"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),(SALVALUE/" & Val(txtRateOld_AMT.Text) & ")*" & Val(txtRateNew_AMT.Text) & ") AS [NEW VALUE]"

        ElseIf UCase(cmbMode.Text) = "SALE VALUE" Then
            strSql += vbCrLf + " ,SALVALUE [OLD SALEVALUE]"
            If Val(txtSaleValueRs_AMT.Text) <> 0 Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),SALVALUE+" & Val(txtSaleValueRs_AMT.Text) & ") AS [NEW SALEVALUE]"
            ElseIf Val(txtSaleValuePer_PER.Text) <> 0 Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2)," & Val(txtSaleValuePer_PER.Text) & ") AS [INC%]"
                If chkSaleValueRound.Checked Then
                    strSql += vbCrLf + " ,ROUND(CONVERT(NUMERIC(15,2),(" & Val(txtSaleValuePer_PER.Text) & "*(SALVALUE/100))),-1) AS [INC VAL]"
                    strSql += vbCrLf + " ,ROUND(CONVERT(NUMERIC(15,2),SALVALUE+(" & Val(txtSaleValuePer_PER.Text) & "*(SALVALUE/100))),-1) AS [NEW SALEVALUE]"
                ElseIf chkSaleValueRound1.Checked Then
                    strSql += vbCrLf + " ,ROUND(CONVERT(NUMERIC(15,2),(" & Val(txtSaleValuePer_PER.Text) & "*(SALVALUE/100))),0) AS [INC VAL]"
                    strSql += vbCrLf + " ,ROUND(CONVERT(NUMERIC(15,2),SALVALUE+(" & Val(txtSaleValuePer_PER.Text) & "*(SALVALUE/100))),0) AS [NEW SALEVALUE]"
                Else
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),(" & Val(txtSaleValuePer_PER.Text) & "*(SALVALUE/100))) AS [INC VAL]"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),SALVALUE+(" & Val(txtSaleValuePer_PER.Text) & "*(SALVALUE/100))) AS [NEW SALEVALUE]"
                End If
            End If
        End If
        strSql += vbCrLf + " ,GRSNET [G/N]"
        strSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMTAG T"
        strSql += vbCrLf + " WHERE ISSDATE IS NULL"
        If Not cnCentStock Then strSql += vbCrLf + " AND COMPANYID = '" & GetStockCompId() & "'"
        If Val(txtWastagePerOld_PER.Text) <> 0 Or Val(txtWastagePerNew_PER.Text) <> 0 Then
            strSql += vbCrLf + " AND MAXWASTPER = " & Val(txtWastagePerOld_PER.Text) & ""
        ElseIf Val(txtWastageOld_WET.Text) <> 0 Or Val(txtWastageNew_WET.Text) <> 0 Then
            strSql += vbCrLf + " AND MAXWAST = " & Val(txtWastageOld_WET.Text) & ""
            strSql += vbCrLf + " AND MAXWASTPER = 0"
        ElseIf Val(txtMcPerGrmOld_AMT.Text) <> 0 Or Val(txtMcPerGrmNew_AMT.Text) <> 0 Then
            strSql += vbCrLf + " AND MAXMCGRM = " & Val(txtMcPerGrmOld_AMT.Text) & ""
        ElseIf Val(txtMcOld_AMT.Text) <> 0 Or Val(txtMcNew_AMT.Text) <> 0 Then
            strSql += vbCrLf + " AND MAXMC = " & Val(txtMcOld_AMT.Text) & ""
            strSql += vbCrLf + " AND MAXMCGRM = 0"
        ElseIf UCase(cmbMode.Text) = "SUBITEM" Then
            strSql += vbCrLf + " AND SUBITEMID = (SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItemOld_MAN.Text & "'"
            strSql += vbCrLf + " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem.Text & "'))"
        ElseIf UCase(cmbMode.Text) = "ITEMSIZE" Then
            strSql += vbCrLf + " AND SIZEID = (SELECT SIZEID FROM " & cnAdminDb & "..ITEMSIZE WHERE SIZENAME = '" & CmbOldItemsize_MAN.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem.Text & "'))"
        ElseIf UCase(cmbMode.Text) = "DESIGNER" Then
            strSql += vbCrLf + " AND DESIGNERID = (SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & cmbDesignerOld.Text & "')"
        ElseIf UCase(cmbMode.Text) = "COSTCENTRE" Then
            strSql += vbCrLf + " AND TCOSTID= (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME= '" & cmbCostcentre_Old.Text & "')"
            strSql += vbCrLf + " AND RECDATE='" & Format(dtpDate.Value, "yyyy-MM-dd") & "'"
        ElseIf UCase(cmbMode.Text) = "TABLE" Then
            strSql += vbCrLf + " AND TABLECODE = '" & cmbTableold.Text & "'"
        ElseIf UCase(cmbMode.Text) = "RATE" Then
            'strSql += vbCrLf + " AND BOARDRATE = " & Val(txtRateOld_AMT.Text)
            If cmbCalcType.Text = "METAL RATE" Then
                If Val(txtRateOld_AMT.Text) <> 0 Or Val(txtRateNew_AMT.Text) <> 0 Then
                    strSql += vbCrLf + "  AND T.RATE = " & Val(txtRateOld_AMT.Text) & ""
                End If
            Else
                strSql += vbCrLf + " AND BOARDRATE = " & Val(txtRateOld_AMT.Text)
            End If

        End If
        If cmbItem.Text <> "ALL" And cmbItem.Text <> "" Then
            strSql += vbCrLf + " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem.Text & "')"
        End If
        If cmbCostCentre.Text <> "ALL" And cmbCostCentre.Text <> "" Then
            strSql += vbCrLf + "  AND T.COSTID = (SELECT TOP 1 COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre.Text & "')"
        End If
        If cmbitemtype.Text <> "ALL" And cmbitemtype.Text <> "" Then
            strSql += vbCrLf + " AND T.ITEMTYPEID = (SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & cmbitemtype.Text & "')"
        End If
        If cmbSubItem_MAN.Text <> "ALL" And cmbSubItem_MAN.Text <> "" Then
            strSql += vbCrLf + " AND SUBITEMID = (SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem_MAN.Text & "'"
            strSql += vbCrLf + " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem.Text & "'))"
        End If
        If txtStyleNo.Text <> "" Then
            strSql += vbCrLf + " AND T.STYLENO = '" & txtStyleNo.Text & "'"
        End If
        If ftrDesigner <> Nothing Then
            strSql += vbCrLf + " AND DESIGNERID IN (SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME IN (" & ftrDesigner & "))"
        End If
        strSql += vbCrLf + " AND SALEMODE = '" & Mid(cmbCalcType.Text, 1, 1) & "'"
        If chkcmbcounter.Text <> "" And chkcmbcounter.Text <> "ALL" Then
            strSql += vbCrLf + " AND T.ITEMCTRID IN (" & ftrcounter & ")"
        End If
        If cmbCalcType.Text = "WEIGHT" Then
            If txtFromWt.Text <> "" Or txtToWt.Text <> "" Then
                strSql += vbCrLf + " AND ((GRSWT between @wtFrom and @wtTo) OR (NETWT between @wtFrom and @wtTo))"
            End If
        End If
        strSql += vbCrLf + " ORDER BY ITEM,TAGNO"
        Dim dtGrid As New DataTable
        Dim dtCol As New DataColumn("CHECK", GetType(Boolean))
        dtCol.DefaultValue = True
        dtGrid.Columns.Add(dtCol)

        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        dtGrid.AcceptChanges()
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        tabView.Show()
        gridView.DataSource = dtGrid
        FormatGridColumns(gridView, False, False)
        gridView.Columns("CHECK").ReadOnly = False
        gridView.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        With gridView
            .Columns("SNO").Visible = False
            If Val(txtWastagePerOld_PER.Text) <> 0 Or Val(txtWastagePerNew_PER.Text) <> 0 Or _
            Val(txtWastageOld_WET.Text) <> 0 Or Val(txtWastageNew_WET.Text) <> 0 Then
                .Columns("OLD WASTAGE").DefaultCellStyle.BackColor = Color.Bisque
                .Columns("OLD WAST%").DefaultCellStyle.BackColor = Color.Bisque
                .Columns("NEW WASTAGE").DefaultCellStyle.BackColor = Color.Lavender
                .Columns("NEW WAST%").DefaultCellStyle.BackColor = Color.Lavender
            ElseIf Val(txtMcPerGrmOld_AMT.Text) <> 0 Or Val(txtMcPerGrmNew_AMT.Text) <> 0 Or _
            Val(txtMcOld_AMT.Text) <> 0 Or Val(txtMcNew_AMT.Text) <> 0 Then 'MC
                .Columns("OLD MCGRM").DefaultCellStyle.BackColor = Color.Bisque
                .Columns("OLD MC").DefaultCellStyle.BackColor = Color.Bisque
                .Columns("NEW MCGRM%").DefaultCellStyle.BackColor = Color.Lavender
                .Columns("NEW MC").DefaultCellStyle.BackColor = Color.Lavender
            ElseIf cmbMode.Text.ToUpper = "SUBITEM" Then
                .Columns("OLD SUBITEM").DefaultCellStyle.BackColor = Color.Bisque
                .Columns("NEW SUBITEM").DefaultCellStyle.BackColor = Color.Lavender
            ElseIf cmbMode.Text.ToUpper = "ITEMSIZE" Then
                .Columns("OLD ITEMSIZE").DefaultCellStyle.BackColor = Color.Bisque
                .Columns("NEW ITEMSIZE").DefaultCellStyle.BackColor = Color.Lavender
            ElseIf cmbMode.Text.ToUpper = "DESIGNER" Then
                .Columns("OLD DESIGNER").DefaultCellStyle.BackColor = Color.Bisque
                .Columns("NEW DESIGNER").DefaultCellStyle.BackColor = Color.Lavender
            ElseIf cmbMode.Text.ToUpper = "COSTCENTRE" Then
                .Columns("OLD COSTCENTRE").DefaultCellStyle.BackColor = Color.Bisque
                .Columns("NEW COSTCENTRE").DefaultCellStyle.BackColor = Color.Lavender
            ElseIf UCase(cmbMode.Text) = "RATE" Then
                .Columns("OLD RATE").DefaultCellStyle.BackColor = Color.Bisque
                .Columns("NEW RATE").DefaultCellStyle.BackColor = Color.Lavender
            ElseIf UCase(cmbMode.Text) = "SALE VALUE" Then
                .Columns("OLD SALEVALUE").DefaultCellStyle.BackColor = Color.Bisque
                .Columns("NEW SALEVALUE").DefaultCellStyle.BackColor = Color.Lavender
                If Val(txtSaleValuePer_PER.Text) <> 0 Then
                    .Columns("INC%").DefaultCellStyle.BackColor = Color.Lavender
                    .Columns("INC VAL").DefaultCellStyle.BackColor = Color.Lavender
                End If
            End If
        End With
    End Sub

    Public Function GetSelectedCounderid(ByVal chkLst As BrighttechPack.CheckedComboBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Items.Count > 0 Then
            For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
                If WithQuotes Then retStr += "'"
                retStr += objGPack.GetSqlValue("SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME= '" & chkLst.CheckedItems.Item(cnt).ToString & "'")
                If WithQuotes Then retStr += "'"
                If cnt <> chkLst.CheckedItems.Count - 1 Then
                    retStr += ","
                End If
            Next
        Else
            retStr = "''"
        End If
        Return retStr
    End Function

    Private Sub GetOtherGrid()
        Dim ftrDesigner As String = GetCheckedItemStr(chkLstDesigner)
        Dim ftrcounter As String = GetSelectedCounderid(chkcmbcounter, True)
        strSql = ""
        If cmbCalcType.Text = "WEIGHT" Then
            strSql = " DECLARE @WTFROM AS FLOAT,@WTTO AS FLOAT"
            strSql += " SET @WTFROM =" & Val(txtFromWt.Text) & ""
            strSql += " SET @WTTO =" & Val(txtToWt.Text) & ""
        End If
        strSql += " SELECT SNO,ITEMID,"
        strSql += " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS ITEM"
        strSql += " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = T.SUBITEMID)AS SUBITEM"
        strSql += " ,TAGNO,PCS,GRSWT,LESSWT,NETWT"
        strSql += " ,'" & cmbOldOther.Text & "' AS [OLD_" & cmbMode.Text & "]"
        strSql += " ,'" & cmbNewOther_MAN.Text & "' AS [NEW_" & cmbMode.Text & "]"
        strSql += " ,GRSNET [G/N],RECDATE,COSTID"
        strSql += "  FROM " & cnAdminDb & "..ITEMTAG T"
        strSql += " WHERE ISSDATE IS NULL"
        If Not cnCentStock Then strSql += " AND COMPANYID = '" & GetStockCompId() & "'"

        If UCase(cmbOldOther.Text) <> "" Then
            strSql += " AND TAGNO IN (SELECT TAGNO FROM " & cnAdminDb & "..ADDINFOITEMTAG WHERE OTHID IN (SELECT ID FROM " & cnAdminDb & "..OTHERMASTER WHERE NAME='" & cmbOldOther.Text & "'))"
        Else
            strSql += " AND TAGNO NOT IN (SELECT TAGNO FROM " & cnAdminDb & "..ADDINFOITEMTAG WHERE ITEMID=T.ITEMID AND TAGNO=T.TAGNO AND OTHID IN (SELECT ID FROM " & cnAdminDb & "..OTHERMASTER WHERE MISCID IN(SELECT MISCID FROM " & cnAdminDb & "..OTHERMASTERENTRY WHERE MISCNAME='" & cmbMode.Text & "')))"
        End If
        If cmbItem.Text <> "ALL" And cmbItem.Text <> "" Then
            strSql += " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem.Text & "')"
        End If
        If cmbitemtype.Text <> "ALL" And cmbitemtype.Text <> "" Then
            strSql += " AND T.ITEMTYPEID = (SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & cmbitemtype.Text & "')"
        End If
        If cmbCostCentre.Text <> "ALL" And cmbCostCentre.Text <> "" Then
            strSql += vbCrLf + "  AND T.COSTID = (SELECT TOP 1 COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre.Text & "')"
        End If
        If cmbSubItem_MAN.Text <> "ALL" And cmbSubItem_MAN.Text <> "" Then
            strSql += " AND SUBITEMID = (SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem_MAN.Text & "'"
            strSql += " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem.Text & "'))"
        End If
        If txtStyleNo.Text <> "" Then
            strSql += " AND T.STYLENO = '" & txtStyleNo.Text & "'"
        End If
        If ftrDesigner <> Nothing Then
            strSql += " AND DESIGNERID IN (SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME IN (" & ftrDesigner & "))"
        End If
        strSql += " AND SALEMODE = '" & Mid(cmbCalcType.Text, 1, 1) & "'"
        If chkcmbcounter.Text <> "" And chkcmbcounter.Text <> "ALL" Then
            strSql += " AND T.ITEMCTRID IN (" & ftrcounter & ")"
        End If
        If cmbCalcType.Text = "WEIGHT" Then
            If txtFromWt.Text <> "" Or txtToWt.Text <> "" Then
                strSql += " AND ((GRSWT between @wtFrom and @wtTo) OR (NETWT between @wtFrom and @wtTo))"
            End If
        End If
        strSql += " ORDER BY ITEM,TAGNO"
        Dim dtGrid As New DataTable
        Dim dtCol As New DataColumn("CHECK", GetType(Boolean))
        dtCol.DefaultValue = True
        dtGrid.Columns.Add(dtCol)

        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        dtGrid.AcceptChanges()
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        tabView.Show()
        gridView.DataSource = dtGrid
        FormatGridColumns(gridView, False, False)
        gridView.Columns("CHECK").ReadOnly = False
        gridView.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        With gridView
            .Columns("SNO").Visible = False
            .Columns("ITEMID").Visible = False
            .Columns("COSTID").Visible = False
            .Columns("RECDATE").Visible = False
            .Columns("OLD_" & cmbMode.Text & "").DefaultCellStyle.BackColor = Color.Bisque
            .Columns("NEW_" & cmbMode.Text & "").DefaultCellStyle.BackColor = Color.Lavender
        End With
    End Sub
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        gridView.DataSource = Nothing
        If UCase(cmbMode.Text) = "STONERATE" Then
            GenStnRateGrid()
        ElseIf Val(objGPack.GetSqlValue("SELECT COUNT(*) FROM " & cnAdminDb & "..OTHERMASTERENTRY WHERE MISCNAME='" & cmbMode.Text & "'", , "", )) > 0 Then
            If cmbNewOther_MAN.Text.Trim = "" Then MsgBox("New " & cmbMode.Text & " cannot empty.", MsgBoxStyle.Information) : Exit Sub
            GetOtherGrid()
        Else
            If UCase(cmbCalcType.Text) = "WEIGHT" Or cmbCalcType.Text = "METAL RATE" Or cmbCalcType.Text = "FIXED" Then
                GenWeightGrid()
            ElseIf UCase(cmbCalcType.Text) = "RATE" Then
                GenRateGrid()
            End If
        End If
        'If UCase(cmbCalcType.Text) = "WEIGHT" Or cmbCalcType.Text = "METAL RATE" Or cmbCalcType.Text = "FIXED" Then
        '    GenWeightGrid()
        'ElseIf UCase(cmbCalcType.Text) = "RATE" Then
        '    GenRateGrid()
        'End If
        If Not gridView.RowCount > 0 Then Exit Sub
        tabMain.SelectedTab = tabView
        gridView.Select()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        objGPack.TextClear(Me)
        tabMain.SelectedTab = tabGeneral
        cmbItem.Text = "ALL"
        cmbSubItem_MAN.Text = "ALL"
        cmbCalcType.Text = "WEIGHT"
        cmbitemtype.Text = "ALL"
        cmbItem.Select()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub CheckList(ByVal chkLst As CheckedListBox, ByVal state As Boolean)
        For cnt As Integer = 0 To chkLst.Items.Count - 1
            chkLst.SetItemChecked(cnt, state)
        Next
    End Sub

    Private Sub chkDesigner_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkDesigner.CheckedChanged
        CheckList(chkLstDesigner, chkDesigner.Checked)
    End Sub

    Private Sub chkLstDesigner_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLstDesigner.GotFocus
        If Not chkLstDesigner.Items.Count > 0 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGeneral
        cmbItem.Select()
    End Sub

    Private Function GetCheckedItemStr(ByVal chkLst As CheckedListBox) As String
        Dim ret As String = Nothing
        If Not chkLst.Items.Count > 0 Then Return ""
        For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
            ret += "'" & chkLst.CheckedItems.Item(cnt).ToString & "',"
        Next
        If ret <> Nothing Then
            ret = Mid(ret, 1, ret.Length - 1)
        End If
        Return ret
    End Function

    Private Sub cmbItem_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbItem.SelectedIndexChanged
        cmbSubItem_MAN.Items.Clear()
        cmbSubItem_MAN.Items.Add("ALL")
        Dim titemid As Integer = 0
        If cmbItem.Text <> "ALL" And cmbItem.Text <> "" Then
            strSql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = (sELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem.Text & "')"
            strSql += " ORDER BY SUBITEMNAME"
            objGPack.FillCombo(strSql, cmbSubItem_MAN, False, True)
            titemid = Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbItem.Text & "'", , , ))

            LoadItemsize(CmbOldItemsize_MAN, titemid)
            LoadItemsize(CmbNewItemSize_MAN, titemid)
        End If
        strSql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ACTIVE = 'Y' "
        strSql += " AND CALTYPE = '" & Mid(cmbCalcType.Text, 1, 1) & "'"
        If cmbItem.Text <> "" And cmbItem.Text <> "ALL" And titemid <> 0 Then strSql += " AND ITEMID IN('" & titemid & "')"
        strSql += " ORDER BY SUBITEMNAME"
        objGPack.FillCombo(strSql, cmbSubItemOld_MAN, True)
        cmbSubItem_MAN.Text = "ALL"
    End Sub

    Private Sub cmbCalcType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCalcType.SelectedIndexChanged
        cmbMode.Items.Clear()
        If cmbCalcType.Text <> "RATE" Then
            cmbMode.Items.Add("Wastage")
            cmbMode.Items.Add("Mc")
            cmbMode.Items.Add("Table")
        End If
        cmbMode.Items.Add("Rate")
        If cmbCalcType.Text = "FIXED" Then
            cmbMode.Items.Add("Sale Value")
        End If
        cmbMode.Items.Add("SubItem")
        cmbMode.Items.Add("Designer")
        If cmbCalcType.Text <> "RATE" Then
            cmbMode.Text = "Wastage"
        Else
            cmbMode.Items.Add("Pur Rate")
            cmbMode.Text = "Rate"
        End If
        cmbMode.Items.Add("StoneRate")
        cmbMode.Items.Add("ItemSize")
        strSql = "SELECT * FROM " & cnAdminDb & "..OTHERMASTERENTRY WHERE ISNULL(ACTIVE,'')<>'N'"
        Dim dtOther As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtOther)
        If dtOther.Rows.Count > 0 Then
            For v As Integer = 0 To dtOther.Rows.Count - 1
                cmbMode.Items.Add(dtOther.Rows(v).Item("MISCNAME").ToString)
            Next
        End If
        If GetAdmindbSoftValue("COSTCENTRE", "N") = "Y" Then
            cmbMode.Items.Add("COSTCENTRE")
        End If
        If cmbCalcType.Text = "WEIGHT" Then
            lblFrmWt.Visible = True
            lbToWt.Visible = True
            txtFromWt.Visible = True
            txtToWt.Visible = True
        Else
            lblFrmWt.Visible = False
            lbToWt.Visible = False
            txtFromWt.Visible = False
            txtToWt.Visible = False
        End If
        strSql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ACTIVE = 'Y' "
        strSql += " AND CALTYPE = '" & Mid(cmbCalcType.Text, 1, 1) & "'"
        If cmbItem.Text <> "" And cmbItem.Text <> "ALL" Then strSql += " AND ITEMID IN( SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME ='" & cmbItem.Text & "')"
        strSql += " ORDER BY SUBITEMNAME"
        objGPack.FillCombo(strSql, cmbSubItemOld_MAN, True)

        strSql = " SELECT DISTINCT TABLECODE FROM " & cnAdminDb & "..WMCTABLE  "
        strSql += " ORDER BY TABLECODE"
        objGPack.FillCombo(strSql, cmbTableold)
        objGPack.FillCombo(strSql, cmbTablenew)

        cmbCalcType.Select()
    End Sub

#Region "WastageTab Events"
    Private Sub txtWastagePerOld_PER_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtWastagePerOld_PER.GotFocus
        If Val(txtWastageOld_WET.Text) <> 0 Or Val(txtWastageNew_WET.Text) <> 0 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtWastagePerNew_PER_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtWastagePerNew_PER.GotFocus
        If Val(txtWastageOld_WET.Text) <> 0 Or Val(txtWastageNew_WET.Text) <> 0 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtWastageOld_WET_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtWastageOld_WET.GotFocus
        If Val(txtWastagePerOld_PER.Text) <> 0 Or Val(txtWastagePerNew_PER.Text) <> 0 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtWastageNew_WET_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtWastageNew_WET.GotFocus
        If Val(txtWastagePerOld_PER.Text) <> 0 Or Val(txtWastagePerNew_PER.Text) <> 0 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
#End Region

#Region "McTab Events"

    Private Sub txtMcPerGrmOld_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMcPerGrmOld_AMT.GotFocus
        If Val(txtMcOld_AMT.Text) <> 0 Or Val(txtMcNew_AMT.Text) <> 0 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtMcPerGrmNew_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMcPerGrmNew_AMT.GotFocus
        If Val(txtMcOld_AMT.Text) <> 0 Or Val(txtMcNew_AMT.Text) <> 0 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtMcOld_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMcOld_AMT.GotFocus
        If Val(txtMcPerGrmOld_AMT.Text) <> 0 Or Val(txtMcPerGrmNew_AMT.Text) <> 0 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtMcNew_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMcNew_AMT.GotFocus
        If Val(txtMcPerGrmOld_AMT.Text) <> 0 Or Val(txtMcPerGrmNew_AMT.Text) <> 0 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

#End Region

#Region "RateTab Events"
    Private Sub RateAutoGroup_GotFocus(ByVal sender As Object, ByVal e As EventArgs) Handles _
     txtRateIncPer_PER.GotFocus _
    , txtRateIncRs_AMT.GotFocus _
    , chkRateRound.GotFocus
        If Val(txtRateOld_AMT.Text) <> 0 Or Val(txtRateNew_AMT.Text) <> 0 Then
            SendKeys.Send("{TAB}")
            Exit Sub
        End If
        If CType(sender, Control).Name = txtRateIncRs_AMT.Name Then
            If Val(txtRateIncPer_PER.Text) <> 0 Then SendKeys.Send("{TAB}")
        Else
            If Val(txtRateIncRs_AMT.Text) <> 0 Then SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub RateManualGroup_GotFocus(ByVal sender As Object, ByVal e As EventArgs) Handles _
txtRateOld_AMT.GotFocus _
, txtRateNew_AMT.GotFocus
        If Val(txtRateIncPer_PER.Text) <> 0 Or Val(txtRateIncRs_AMT.Text) <> 0 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub


#End Region

#Region "SaleValue Events"
    Private Sub SaleValueGroup1_Gotfocus(ByVal sender As Object, ByVal e As EventArgs) Handles _
    txtSaleValuePer_PER.GotFocus, chkSaleValueRound.GotFocus
        If Val(txtSaleValueRs_AMT.Text) <> 0 Then SendKeys.Send("{TAB}")
    End Sub
    Private Sub SaleValueGroup2_Gotfocus(ByVal sender As Object, ByVal e As EventArgs) Handles _
    txtSaleValueRs_AMT.GotFocus
        If Val(txtSaleValuePer_PER.Text) <> 0 Then SendKeys.Send("{TAB}")
    End Sub
#End Region


    Private Sub cmbSubItemOld_MAN_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSubItemOld_MAN.SelectedIndexChanged
        cmbSubItemNew_MAN.Items.Clear()
        Dim olditemid As String = GetSqlValue(cn, "SELECT ITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItemOld_MAN.Text & "' ")
        cmbSubItem_MAN.Text = ""
        strSql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST AS S"
        strSql += " WHERE EXISTS (SELECT 1 FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = S.ITEMID AND CALTYPE = S.CALTYPE AND SUBITEMNAME = '" & cmbSubItemOld_MAN.Text & "')"
        strSql += " AND ITEMID='" & olditemid & "'"
        objGPack.FillCombo(strSql, cmbSubItemNew_MAN)
    End Sub

    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        Dim dr() As DataRow
        dr = CType(gridView.DataSource, DataTable).Select("CHECK = TRUE")
        If Not dr.Length > 0 Then
            MsgBox("There is no selected record", MsgBoxStyle.Information)
            Exit Sub
        End If
        Try
            tran = Nothing
            tran = cn.BeginTransaction
            For Each ro As DataRow In dr
                strSql = GenUpdateQry(ro)
                'cmd = New OleDbCommand(strSql, cn, tran)
                'cmd.ExecuteNonQuery()
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnHOCostId)
            Next
            tran.Commit()
            tran = Nothing
            MsgBox("Updated Successfully", MsgBoxStyle.Information)
            btnNew_Click(Me, New EventArgs)
        Catch ex As Exception
            If tran IsNot Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub

    Private Function GenUpdateQry(ByVal ro As DataRow) As String
        Dim qry As String = Nothing

        If cmbMode.Text.ToUpper = "STONERATE" Then
            qry += " UPDATE " & cnAdminDb & "..ITEMTAGSTONE SET"
            qry += " STNRATE = " & Val(ro.Item("NEW RATE").ToString) & ""
            qry += " ,STNAMT = " & Val(ro.Item("NEW VALUE").ToString) & ""
            qry += " WHERE SNO = '" & ro.Item("SNO").ToString & "'"
            qry += " UPDATE A SET A.SALVALUE= A.SALVALUE- " & Val(ro.Item("OLD VALUE").ToString) & "+" & Val(ro.Item("NEW VALUE").ToString)
            qry += " FROM " & cnAdminDb & "..ITEMTAG A "
            qry += " WHERE A.SNO = '" & ro.Item("TAGSNO").ToString & "'"

        ElseIf cmbMode.Text.ToUpper = "PUR RATE" Then
            qry += " UPDATE " & cnAdminDb & "..PURITEMTAG SET"
            qry += " PURRATE = " & Val(ro.Item("NEW RATE").ToString) & ""
            qry += " ,PURVALUE = " & Val(ro.Item("NEW VALUE").ToString) & ""
            qry += " WHERE TAGSNO = '" & ro.Item("SNO").ToString & "'"
        ElseIf Val(objGPack.GetSqlValue("SELECT COUNT(*) FROM " & cnAdminDb & "..OTHERMASTERENTRY WHERE MISCNAME='" & cmbMode.Text & "'", , "", tran)) > 0 Then
            Dim NEWOTHID As Integer = Val(objGPack.GetSqlValue("SELECT ID FROM " & cnAdminDb & "..OTHERMASTER WHERE NAME = '" & ro.Item("NEW_" & cmbMode.Text & "").ToString & "'", , "", tran))
            Dim OLDOTHID As Integer = Val(objGPack.GetSqlValue("SELECT ID FROM " & cnAdminDb & "..OTHERMASTER WHERE NAME = '" & ro.Item("OLD_" & cmbMode.Text & "").ToString & "'", , "", tran))
            If Val(objGPack.GetSqlValue("SELECT COUNT(*) FROM " & cnAdminDb & "..ADDINFOITEMTAG WHERE ITEMID='" & Val(ro.Item("ITEMID").ToString) & "' AND TAGNO='" & ro.Item("TAGNO").ToString & "' AND OTHID='" & OLDOTHID & "'", , "", tran)) > 0 Then
                qry += " UPDATE " & cnAdminDb & "..ADDINFOITEMTAG  SET"
                qry += " OTHID ='" & NEWOTHID & "' "
                qry += " WHERE ITEMID='" & Val(ro.Item("ITEMID").ToString) & "' AND TAGNO='" & ro.Item("TAGNO").ToString & "' AND OTHID='" & OLDOTHID & "'"
            Else
                qry += " INSERT INTO " & cnAdminDb & "..ADDINFOITEMTAG("
                qry += " SNO,TAGSNO,OTHID,ITEMID,TAGNO,RECDATE,COMPANYID,COSTID,SYSTEMID,APPVER"
                qry += " )VALUES("
                qry += " '" & GetNewSno(TranSnoType.ADDINFOITEMTAGCODE, tran, "GET_ADMINSNO_TRAN") & "'" 'SNO  
                qry += " ,'" & ro.Item("SNO").ToString & "'" 'TAGSNO
                qry += " ,'" & NEWOTHID & "'" 'OTHID
                qry += " ,'" & Val(ro.Item("ITEMID").ToString) & "'" 'ITEMID
                qry += " ,'" & ro.Item("TAGNO").ToString & "'" 'TAGNO
                qry += " ,'" & Format(ro.Item("RECDATE"), "yyyy-MM-dd") & "'" 'RECDATE
                qry += " ,'" & GetStockCompId() & "'" 'COMPANYID
                qry += " ,'" & ro.Item("COSTID").ToString & "'" 'COSTID
                qry += " ,'" & systemId & "'" 'SYSTEMID
                qry += " ,'" & VERSION & "'" 'APPVER
                qry += " )"
            End If
        Else
            qry += " UPDATE " & cnAdminDb & "..ITEMTAG SET"
            If cmbMode.Text.ToUpper = "WASTAGE" Then
                qry = ""
                qry += " UPDATE T SET"
                qry += " MAXWASTPER = " & Val(ro.Item("NEW WAST%").ToString)
                qry += " ,MAXWAST = " & Val(ro.Item("NEW WASTAGE").ToString)
                If McWithWastage Then
                    qry += " ,MAXMC = "
                    qry += vbCrLf + " CONVERT(NUMERIC(15,3),MAXMCGRM*(CASE WHEN (CASE WHEN T.SUBITEMID <> 0 THEN (SELECT VALUECALC FROM " & cnAdminDb & "..SUBITEMMAST AS S WHERE S.ITEMID=T.ITEMID AND S.SUBITEMID=T.SUBITEMID)"
                    qry += vbCrLf + " ELSE (SELECT VALUECALC FROM " & cnAdminDb & "..ITEMMAST AS S WHERE S.ITEMID=T.ITEMID) END)='N' THEN NETWT+ ISNULL(" & Val(ro.Item("NEW WASTAGE").ToString) & ",0) ELSE GRSWT+ ISNULL(" & Val(ro.Item("NEW WASTAGE").ToString) & ",0) END)) "
                End If
            ElseIf cmbMode.Text.ToUpper = "MC" Then
                qry += " MAXMCGRM = " & Val(ro.Item("NEW MCGRM%").ToString)
                qry += " ,MAXMC = " & Val(ro.Item("NEW MC").ToString)
            ElseIf cmbMode.Text.ToUpper = "TABLE" Then
                qry = " UPDATE A SET TABLECODE = '" & cmbTablenew.Text & "',"
                qry += "A.MAXWASTPER=N.MAXWASTPER,A.MAXWAST=CASE WHEN N.MAXWAST = 0 AND A.MAXWAST <> 0 AND A.MAXWASTPER <> 0 THEN (A.MAXWAST/(A.MAXWASTPER/100)*(N.MAXWASTPER/100)) ELSE CASE WHEN N.MAXWAST=0 AND N.MAXWASTPER <> 0 THEN A.GRSWT*(N.MAXWASTPER/100) ELSE N.MAXWAST END END,"
                qry += "A.MAXMCGRM=N.MAXMCGRM,A.MAXMC=CASE WHEN N.MAXMC = 0 AND A.MAXMC <> 0 AND A.MAXMCGRM <> 0 AND N.MAXMCGRM <> 0 THEN (A.MAXMC/A.MAXMCGRM)*N.MAXMCGRM ELSE CASE WHEN N.MAXMC = 0 AND N.MAXMCGRM <> 0 THEN  A.GRSWT*N.MAXMCGRM ELSE N.MAXMC END END"
                qry += " FROM " & cnAdminDb & "..ITEMTAG A," & cnAdminDb & "..WMCTABLE N "
            ElseIf cmbMode.Text.ToUpper = "RATE" Then
                'qry += " BOARDRATE = " & Val(ro.Item("NEW RATE").ToString)
                qry += " RATE = " & Val(ro.Item("NEW RATE").ToString)
                qry += " ,BOARDRATE = " & Val(ro.Item("NEW RATE").ToString)
                qry += " ,SALVALUE = " & IIf(SALVALUEROUND <> 0, SALEVALUE_ROUND(Val(ro.Item("NEW VALUE").ToString)), Val(ro.Item("NEW VALUE").ToString))
            ElseIf cmbMode.Text.ToUpper = "SALE VALUE" Then
                qry += " SALVALUE = " & IIf(SALVALUEROUND <> 0, SALEVALUE_ROUND(Val(ro.Item("NEW SALEVALUE").ToString)), Val(ro.Item("NEW SALEVALUE").ToString))
            ElseIf cmbMode.Text.ToUpper = "SUBITEM" Then
                qry += " SUBITEMID = (SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & ro.Item("NEW SUBITEM").ToString & "')"
            ElseIf cmbMode.Text.ToUpper = "ITEMSIZE" Then
                qry += " SIZEID = (SELECT SIZEID FROM " & cnAdminDb & "..ITEMSIZE WHERE SIZENAME = '" & ro.Item("NEW ITEMSIZE").ToString & "' AND ITEMID IN(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbItem.Text & "'))"
            ElseIf cmbMode.Text.ToUpper = "DESIGNER" Then
                qry += " DESIGNERID = (SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & ro.Item("NEW DESIGNER").ToString & "')"
            ElseIf cmbMode.Text.ToUpper = "COSTCENTRE" Then
                qry += " TCOSTID= (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & ro.Item("NEW COSTCENTRE").ToString & "')"
            End If
            If cmbMode.Text.ToUpper <> "WASTAGE" Then
                qry += " WHERE SNO = '" & ro.Item("SNO").ToString & "'"
            Else
                qry += " FROM " & cnAdminDb & "..ITEMTAG AS T WHERE SNO = '" & ro.Item("SNO").ToString & "'"
            End If
            If cmbMode.Text.ToUpper = "TABLE" Then qry += " AND N.TABLECODE ='" & cmbTablenew.Text & "' AND A.GRSWT BETWEEN N.FROMWEIGHT AND N.TOWEIGHT"
        End If
        Return qry
    End Function

    Private Function SALEVALUE_ROUND(ByVal svalue As Decimal) As Decimal
        If SALVALUEROUND <> 0 Then
            If svalue <> 0 Then
                Dim wholepart As Decimal = Val(svalue) / SALVALUEROUND
                Dim intpart As Decimal = Int(wholepart)
                Dim decpart As Decimal = Round(wholepart - intpart)
                svalue = (intpart + decpart) * SALVALUEROUND
            End If
        End If
        Return svalue
    End Function
    Private Sub grpContrainer_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles grpContrainer.Enter

    End Sub

    Private Sub loadsubstoneitems()
        strSql = " SELECT 'ALL' SUBITEMNAME,'ALL' ITEMID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT SUBITEMNAME,CONVERT(vARCHAR,SUBITEMID),2 RESULT FROM " & cnAdminDb & "..SUBITEMMAST"
        strSql += " WHERE ACTIVE = 'Y'"
        strSql += " AND itemid IN (SELECT ITEMID FROM " & cnAdminDb & "..itemmast "
        If chkCmbItem.Text <> "ALL" And chkCmbItem.Text <> "" Then
            strSql += " WHERE itemname IN (" & GetQryString(chkCmbItem.Text) & "))"
        Else
            strSql += " WHERE metalid IN ('D','T'))"
        End If
        strSql += " ORDER BY RESULT,SUBITEMNAME"
        Dim dtsItems = New DataTable

        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtsItems)
        BrighttechPack.GlobalMethods.FillCombo(chkSubItem, dtsItems, "SUBITEMNAME", , "ALL")
    End Sub

    Private Sub chkCmbItem_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCmbItem.TextChanged
        loadsubstoneitems()
    End Sub
    Private Function LoadItemsize(ByVal Cmb As ComboBox, ByVal itemid As Integer)
        strSql = " SELECT SIZENAME FROM " & cnAdminDb & "..ITEMSIZE "
        strSql += " WHERE  ITEMID = '" & itemid & "'"
        strSql += " ORDER BY SIZENAME"
        objGPack.FillCombo(strSql, Cmb, True)
    End Function

    Private Sub chkRateRound_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkRateRound.CheckedChanged
        If chkRateRound.Checked = True Then chkRateRound1.Checked = False
    End Sub

    Private Sub chkRateRound1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkRateRound1.CheckedChanged
        If chkRateRound1.Checked = True Then chkRateRound.Checked = False
    End Sub

    Private Sub chkSaleValueRound_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSaleValueRound.CheckedChanged
        If chkSaleValueRound.Checked = True Then chkSaleValueRound1.Checked = False
    End Sub

    Private Sub chkSaleValueRound1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSaleValueRound1.CheckedChanged
        If chkSaleValueRound1.Checked = True Then chkSaleValueRound.Checked = False
    End Sub
End Class