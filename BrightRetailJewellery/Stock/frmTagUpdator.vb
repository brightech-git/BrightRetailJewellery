Imports System.Data.OleDb
Imports System.Globalization
Imports System.Math

Public Class frmTagUpdator
    Dim strSql As String
    Dim dt As New DataTable
    Dim cmd As OleDbCommand
    Dim costcentre As Boolean = IIf(GetAdmindbSoftValue("COSTCENTRE", "Y").ToUpper = "Y", True, False)
    Dim _MCONGRSNET As Boolean = IIf(GetAdmindbSoftValue("MC_ON_GRSNET", "Y") = "Y", True, False)
    Dim _WASTONGRSNET As Boolean = IIf(GetAdmindbSoftValue("WAST_ON_GRSNET", "Y") = "Y", True, False)
    Dim McWithWastage As Boolean = IIf(GetAdmindbSoftValue("MCWITHWASTAGE", "N") = "Y", True, False)
    Dim TagTable As String = "ITEMTAG"

    Private Sub frmTagUpdator_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If tabMain.SelectedTab.Name = tabView.Name Then
                btnBack_Click(Me, New EventArgs)
            End If
        End If
    End Sub

    Private Sub frmTagUpdator_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If cmbType.Focused Then
                If cmbType.Text = "ITEM" Then ChkCmbItem.Focus() : Exit Sub
                If cmbType.Text = "DESIGNER" Then ChkCmbDesigner.Focus() : Exit Sub
                If cmbType.Text = "TABLE" Then CmbTable.Focus() : Exit Sub
                If cmbType.Text = "TAGTYPE" Then ChkCmbTagType.Focus() : Exit Sub
            End If
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmTagUpdator_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGeneral.Left, Me.tabGeneral.Top, Me.tabGeneral.Width, Me.tabGeneral.Height))
        grpContrainer.Location = New Point((ScreenWid - grpContrainer.Width) / 2, ((ScreenHit - 128) - grpContrainer.Height) / 2)
        pnlContainer_OWN.Location = New Point((ScreenWid - pnlContainer_OWN.Width) / 2, ((ScreenHit - 128) - pnlContainer_OWN.Height) / 2)
        tabMode.ItemSize = New System.Drawing.Size(1, 1)
        tabMode.SendToBack()
        Me.tabMode.Region = New Region(New RectangleF(Me.tabitem.Left, Me.tabitem.Top, Me.tabitem.Width, Me.tabitem.Height))
        cmbType.Items.Clear()
        cmbType.Items.Add("ITEM")
        cmbType.Items.Add("DESIGNER")
        cmbType.Items.Add("TABLE")
        cmbType.Items.Add("TAGTYPE")
        cmbType.SelectedIndex = 0
        strSql = "SELECT COUNT(*)CNT FROM " & cnAdminDb & "..WITEMTAG"
        If Val(objGPack.GetSqlValue(strSql, "CNT", 0).ToString) = 0 Then
            pnlTag.Visible = False
        Else
            pnlTag.Visible = True
        End If
        rbtGrsWt.Checked = True
        rbtTag.Checked = True
    End Sub

    Private Sub cmbMode_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub ChkCmbTagLoad()
        strSql = " SELECT NAME,ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE "
        strSql += " ORDER BY ITEMTYPEID"
        objGPack.FillCombo(strSql, ChkCmbTagType)
    End Sub
    Private Sub ChkCmbItemLoad()
        strSql = " SELECT 'ALL' AS ITEMNAME,0 AS RESULT UNION ALL SELECT ITEMNAME,1 AS RESULT FROM " & cnAdminDb & "..ITEMMAST "
        strSql += " WHERE ACTIVE = 'Y' ORDER BY RESULT,ITEMNAME"
        Dim dtItem As New DataTable
        da = New OleDbDataAdapter
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtItem)
        BrighttechPack.GlobalMethods.FillCombo(ChkCmbItem, dtItem, "ITEMNAME", , "ALL")

        strSql = " SELECT 'ALL' AS NAME,0 AS RESULT UNION ALL SELECT NAME,1 AS RESULT FROM " & cnAdminDb & "..ITEMTYPE ORDER BY RESULT,NAME"
        Dim dtItemTYPE As New DataTable
        da = New OleDbDataAdapter
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtItemTYPE)
        BrighttechPack.GlobalMethods.FillCombo(cmbtagtype, dtItemTYPE, "NAME", , "ALL")
    End Sub

    Private Sub ChkCmbItemLoadDes()
        strSql = " SELECT 'ALL' AS ITEMNAME,0 AS RESULT UNION ALL SELECT ITEMNAME,1 AS RESULT FROM " & cnAdminDb & "..ITEMMAST "
        strSql += " WHERE ACTIVE = 'Y' ORDER BY RESULT,ITEMNAME "
        Dim dtItem As New DataTable
        da = New OleDbDataAdapter
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtItem)
        BrighttechPack.GlobalMethods.FillCombo(ChkCmbItemDes, dtItem, "ITEMNAME", , "ALL")
    End Sub

    Private Sub ChkCmbSubitemLoad()
        Dim SubItemName As String
        SubItemName = GetChecked_CheckedList(ChkCmbItem)
        ChkCmbSubItem.Items.Clear()
        If SubItemName <> "" Then
            strSql = " SELECT 'ALL' AS SUBITEMNAME,0 AS RESULT UNION ALL SELECT SUBITEMNAME,1 AS RESULT FROM " & cnAdminDb & "..SUBITEMMAST WHERE ACTIVE = 'Y'"
            strSql += vbCrLf + " AND ITEMID IN(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN(" & SubItemName & "))"
            strSql += vbCrLf + " ORDER BY RESULT,SUBITEMNAME "
            Dim dt As New DataTable
            da = New OleDbDataAdapter
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            BrighttechPack.GlobalMethods.FillCombo(ChkCmbSubItem, dt, "SUBITEMNAME", , "ALL")
        End If
    End Sub

    Private Sub ChkCmbSubitemLoadDes()
        Dim SubItemName As String
        SubItemName = GetChecked_CheckedList(ChkCmbItemDes)
        ChkCmbSubItemDes.Items.Clear()
        If SubItemName <> "" Then
            strSql = " SELECT 'ALL' AS SUBITEMNAME,0 AS RESULT  UNION ALL  SELECT SUBITEMNAME,1 AS RESULT  FROM " & cnAdminDb & "..SUBITEMMAST WHERE ACTIVE = 'Y'"
            strSql += vbCrLf + " AND ITEMID IN(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN(" & SubItemName & ")) ORDER BY RESULT,SUBITEMNAME"
            Dim dt As New DataTable
            da = New OleDbDataAdapter
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            BrighttechPack.GlobalMethods.FillCombo(ChkCmbSubItemDes, dt, "SUBITEMNAME", , "ALL")
        End If
    End Sub

    Private Sub chkcmbDesignerLoad()
        strSql = " SELECT 'ALL' AS DESIGNERNAME,0 AS RESULT  UNION ALL SELECT DESIGNERNAME,1 AS RESULT  FROM " & cnAdminDb & "..DESIGNER "
        strSql += " WHERE ACTIVE = 'Y' ORDER BY RESULT,DESIGNERNAME "
        Dim dtDesign As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtDesign)
        BrighttechPack.GlobalMethods.FillCombo(ChkCmbDesigner, dtDesign, "DESIGNERNAME", , "ALL")
    End Sub
    Private Sub chkcmbTableLoad()
        strSql = " SELECT DISTINCT TABLECODE FROM " & cnAdminDb & "..WMCTABLE  "
        strSql += " ORDER BY TABLECODE"
        objGPack.FillCombo(strSql, CmbTable)
    End Sub
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If rbtTag.Checked Then
            TagTable = "ITEMTAG"
        Else
            TagTable = "WITEMTAG"
        End If
        gridView.DataSource = Nothing
        If UCase(cmbType.Text) = "ITEM" Then
            GetWastMc()
        ElseIf UCase(cmbType.Text) = "DESIGNER" Then
            GetWastMc()
        ElseIf UCase(cmbType.Text) = "TAGTYPE" Then
            GetWastMc()
        Else
            GetWastMc()
        End If
        If Not gridView.RowCount > 0 Then Exit Sub
        tabMain.SelectedTab = tabView
        gridView.Select()
    End Sub
    Private Sub GetWastMc()
        Dim flag As Boolean = False
        Dim SubItemName, ItemName, SubItemNameDes, ItemNameDes, ItemTypeName As String
        ItemName = GetChecked_CheckedList(ChkCmbItem)
        SubItemName = GetChecked_CheckedList(ChkCmbSubItem)
        ItemNameDes = GetChecked_CheckedList(ChkCmbItemDes)
        SubItemNameDes = GetChecked_CheckedList(ChkCmbSubItemDes)
        ItemTypeName = GetChecked_CheckedList(cmbtagtype)
        Dim ftrDesigner As String = GetChecked_CheckedList(ChkCmbDesigner)
        Dim maincostid As String = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..SYNCCOSTCENTRE WHERE MAIN='Y'", "", )
        ProgressBarShow()
        ProgressBarStep("", 10)
        'Me.Cursor = Cursors.WaitCursor
        strSql = "SELECT FROMWEIGHT,TOWEIGHT FROM " & cnAdminDb & "..WMCTABLE T WHERE 1=1 "
        If UCase(cmbType.Text) = "ITEM" Then
            If ItemName <> "'ALL'" Then strSql += vbCrLf + " AND T.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & ItemName & "))"
            If SubItemName <> "" And SubItemName <> "'ALL'" Then
                strSql += vbCrLf + " AND T.SUBITEMID IN (SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME IN (" & SubItemName & "))"
            End If
            If ItemTypeName <> "'ALL'" Then strSql += " AND T.ITEMTYPE = (SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME='" & Cmbtagtype.Text & "')"
        ElseIf UCase(cmbType.Text) = "DESIGNER" Then
            If ftrDesigner <> "" Then
                strSql += vbCrLf + " AND  T.DESIGNERID IN (SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME IN  (" & ftrDesigner & ")) "
            End If
            If ItemName <> "" And ItemName <> "'ALL'" Then strSql += vbCrLf + " AND T.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & ItemName & "))"
            If SubItemName <> "" And SubItemName <> "'ALL'" Then
                strSql += vbCrLf + " AND T.SUBITEMID IN (SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME IN (" & SubItemName & "))"
            End If
        ElseIf UCase(cmbType.Text) = "TABLE" Then
            strSql += " AND T.TABLECODE = '" & CmbTable.Text & "'"
        ElseIf UCase(cmbType.Text) = "TAGTYPE" Then
            strSql += " AND T.ITEMTYPE = (SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME='" & ChkCmbTagType.Text & "')"
        End If
        If costcentre = True And maincostid <> cnCostId Then strSql += " AND T.COSTID='" & cnCostId & "'"
        Dim dt As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            If Val(dt.Rows(0).Item("FROMWEIGHT").ToString) = 0 And Val(dt.Rows(0).Item("TOWEIGHT").ToString) = 0 Then
                flag = False
            Else
                flag = True
            End If
        End If
        If UCase(cmbType.Text) = "DESIGNER" And dt.Rows.Count = 0 Then
            MsgBox("Please set VA for the designer...")
            Exit Sub
        End If
        strSql = "SELECT SNO,TAGNO,"
        strSql += vbCrLf + " IM.ITEMNAME AS ITEM"
        strSql += vbCrLf + " ,SM.SUBITEMNAME AS SUBITEM,T.GRSWT,T.NETWT"
        strSql += vbCrLf + " ,T.MAXWASTPER [OLD MAXWAST%],WM.MAXWASTPER [NEW MAXWAST%],T.MAXWAST [OLD MAXWASTAGE],"
        If _WASTONGRSNET Then
            strSql += vbCrLf + " (CASE "
            strSql += vbCrLf + " WHEN T.GRSNET='G' AND WM.MAXWASTPER<>0 THEN CONVERT(NUMERIC(15,3),(WM.MAXWASTPER * T.GRSWT)/100) "
            strSql += vbCrLf + " WHEN T.GRSNET='N' AND WM.MAXWASTPER<>0 THEN CONVERT(NUMERIC(15,3),(WM.MAXWASTPER * T.NETWT)/100) "
            strSql += vbCrLf + " WHEN T.GRSNET='G' AND WM.MAXWASTPER=0  THEN CONVERT(NUMERIC(15,3), WM.MAXWAST) "
            strSql += vbCrLf + " WHEN T.GRSNET='N' AND WM.MAXWASTPER=0  THEN CONVERT(NUMERIC(15,3), WM.MAXWAST) "
            strSql += vbCrLf + " END)AS [NEW MAXWASTAGE]"
        Else
            strSql += vbCrLf + " (CASE "
            strSql += vbCrLf + " WHEN T.SUBITEMID<>0 AND SM.MCCALC='G'  AND WM.MAXWASTPER<>0  THEN CONVERT(NUMERIC(15,3),(WM.MAXWASTPER * T.GRSWT)/100) "
            strSql += vbCrLf + " WHEN T.SUBITEMID<>0 AND SM.MCCALC='N'  AND WM.MAXWASTPER<>0  THEN CONVERT(NUMERIC(15,3),(WM.MAXWASTPER * T.NETWT)/100) "
            strSql += vbCrLf + " WHEN T.SUBITEMID=0  AND IM.MCCALC='N'  AND WM.MAXWASTPER<>0  THEN CONVERT(NUMERIC(15,3),(WM.MAXWASTPER * T.NETWT)/100) "
            strSql += vbCrLf + " WHEN T.SUBITEMID=0  AND IM.MCCALC='G'  AND WM.MAXWASTPER<>0  THEN CONVERT(NUMERIC(15,3),(WM.MAXWASTPER * T.GRSWT)/100) "
            strSql += vbCrLf + " WHEN T.SUBITEMID<>0 AND SM.MCCALC='G'  AND WM.MAXWASTPER=0   THEN CONVERT(NUMERIC(15,3),WM.MAXWAST) "
            strSql += vbCrLf + " WHEN T.SUBITEMID<>0 AND SM.MCCALC='N'  AND WM.MAXWASTPER=0   THEN CONVERT(NUMERIC(15,3),WM.MAXWAST) "
            strSql += vbCrLf + " WHEN T.SUBITEMID=0  AND IM.MCCALC='N'  AND WM.MAXWASTPER=0   THEN CONVERT(NUMERIC(15,3),WM.MAXWAST) "
            strSql += vbCrLf + " WHEN T.SUBITEMID=0  AND IM.MCCALC='G'  AND WM.MAXWASTPER=0   THEN CONVERT(NUMERIC(15,3),WM.MAXWAST) "
            strSql += vbCrLf + " END ) AS [NEW MAXWASTAGE]"
        End If
        strSql += vbCrLf + " ,T.MAXMCGRM [OLD MAXMCGRM],WM.MAXMCGRM [NEW MAXMCGRM],T.MAXMC [OLD MAXMC]"
        If _WASTONGRSNET Then
            strSql += vbCrLf + " ,CASE WHEN WM.MAXMCGRM = 0 THEN WM.MAXMC "
            strSql += vbCrLf + " ELSE CONVERT(NUMERIC(15,2),(WM.MAXMCGRM*T.GRSWT)) END AS [NEW MAXMC]"
        Else
            If McWithWastage Then
                strSql += vbCrLf + " ,CASE WHEN WM.MAXMCGRM = 0 THEN WM.MAXMC "
                strSql += vbCrLf + " ELSE "
                strSql += vbCrLf + " CASE WHEN T.SUBITEMID<>0 AND SM.MCCALC='G' THEN "
                strSql += vbCrLf + " CONVERT(NUMERIC(15,2),(WM.MAXMCGRM*(T.GRSWT+CASE WHEN T.SUBITEMID<>0 AND SM.MCCALC='G' THEN CONVERT(NUMERIC(15,3),(WM.MAXWASTPER * T.GRSWT)/100) "
                strSql += vbCrLf + " WHEN T.SUBITEMID<>0 AND SM.MCCALC='N' THEN CONVERT(NUMERIC(15,3),(WM.MAXWASTPER * T.NETWT)/100) "
                strSql += vbCrLf + " WHEN T.SUBITEMID=0 AND IM.MCCALC='N' THEN CONVERT(NUMERIC(15,3),(WM.MAXWASTPER * T.NETWT)/100) "
                strSql += vbCrLf + " WHEN T.SUBITEMID=0 AND IM.MCCALC='G' THEN CONVERT(NUMERIC(15,3),(WM.MAXWASTPER * T.GRSWT)/100) "
                strSql += vbCrLf + " ELSE CONVERT(NUMERIC(15,3),(WM.MAXWASTPER * T.NETWT)/100) END))) "
                strSql += vbCrLf + " WHEN T.SUBITEMID<>0 AND SM.MCCALC='N' THEN "
                strSql += vbCrLf + " CONVERT(NUMERIC(15,2),(WM.MAXMCGRM*(T.NETWT+CASE WHEN T.SUBITEMID<>0 AND SM.MCCALC='G' THEN CONVERT(NUMERIC(15,3),(WM.MAXWASTPER * T.GRSWT)/100) "
                strSql += vbCrLf + " WHEN T.SUBITEMID<>0 AND SM.MCCALC='N' THEN CONVERT(NUMERIC(15,3),(WM.MAXWASTPER * T.NETWT)/100) "
                strSql += vbCrLf + " WHEN T.SUBITEMID=0 AND IM.MCCALC='N' THEN CONVERT(NUMERIC(15,3),(WM.MAXWASTPER * T.NETWT)/100) "
                strSql += vbCrLf + " WHEN T.SUBITEMID=0 AND IM.MCCALC='G' THEN CONVERT(NUMERIC(15,3),(WM.MAXWASTPER * T.GRSWT)/100) "
                strSql += vbCrLf + " ELSE CONVERT(NUMERIC(15,3),(WM.MAXWASTPER * T.NETWT)/100) END))) "
                strSql += vbCrLf + " WHEN T.SUBITEMID=0 AND SM.MCCALC='N' THEN "
                strSql += vbCrLf + " CONVERT(NUMERIC(15,2),(WM.MAXMCGRM*(T.NETWT+CASE WHEN T.SUBITEMID<>0 AND SM.MCCALC='G' THEN CONVERT(NUMERIC(15,3),(WM.MAXWASTPER * T.GRSWT)/100) "
                strSql += vbCrLf + " WHEN T.SUBITEMID<>0 AND SM.MCCALC='N' THEN CONVERT(NUMERIC(15,3),(WM.MAXWASTPER * T.NETWT)/100) "
                strSql += vbCrLf + " WHEN T.SUBITEMID=0 AND IM.MCCALC='N' THEN CONVERT(NUMERIC(15,3),(WM.MAXWASTPER * T.NETWT)/100) "
                strSql += vbCrLf + " WHEN T.SUBITEMID=0 AND IM.MCCALC='G' THEN CONVERT(NUMERIC(15,3),(WM.MAXWASTPER * T.GRSWT)/100) "
                strSql += vbCrLf + " ELSE CONVERT(NUMERIC(15,3),(WM.MAXWASTPER * T.NETWT)/100) END))) "
                strSql += vbCrLf + " WHEN T.SUBITEMID=0 AND SM.MCCALC='G' "
                strSql += vbCrLf + " THEN CONVERT(NUMERIC(15,2),(WM.MAXMCGRM*(T.GRSWT+CASE WHEN T.SUBITEMID<>0 AND SM.MCCALC='G' THEN CONVERT(NUMERIC(15,3),(WM.MAXWASTPER * T.GRSWT)/100) "
                strSql += vbCrLf + " WHEN T.SUBITEMID<>0 AND SM.MCCALC='N' THEN CONVERT(NUMERIC(15,3),(WM.MAXWASTPER * T.NETWT)/100) "
                strSql += vbCrLf + " WHEN T.SUBITEMID=0 AND IM.MCCALC='N' THEN CONVERT(NUMERIC(15,3),(WM.MAXWASTPER * T.NETWT)/100) "
                strSql += vbCrLf + " WHEN T.SUBITEMID=0 AND IM.MCCALC='G' THEN CONVERT(NUMERIC(15,3),(WM.MAXWASTPER * T.GRSWT)/100) "
                strSql += vbCrLf + " ELSE CONVERT(NUMERIC(15,3),(WM.MAXWASTPER * T.NETWT)/100) END))) "
                strSql += vbCrLf + " END END AS [NEW MAXMC]"
            Else
                strSql += vbCrLf + " ,CASE WHEN WM.MAXMCGRM = 0 THEN WM.MAXMC "
                strSql += vbCrLf + " ELSE "
                strSql += vbCrLf + " CASE WHEN T.SUBITEMID<>0 AND SM.MCCALC='G' THEN CONVERT(NUMERIC(15,2),(WM.MAXMCGRM*T.GRSWT)) "
                strSql += vbCrLf + " WHEN T.SUBITEMID<>0 AND SM.MCCALC='N' THEN CONVERT(NUMERIC(15,2),(WM.MAXMCGRM*T.NETWT)) "
                strSql += vbCrLf + " WHEN T.SUBITEMID=0 AND SM.MCCALC='N' THEN CONVERT(NUMERIC(15,2),(WM.MAXMCGRM*T.NETWT)) "
                strSql += vbCrLf + " WHEN T.SUBITEMID=0 AND SM.MCCALC='G' THEN CONVERT(NUMERIC(15,2),(WM.MAXMCGRM*T.GRSWT)) "
                strSql += vbCrLf + " END END AS [NEW MAXMC]"
            End If
        End If
        strSql += vbCrLf + " ,T.MINWASTPER  [OLD MINWAST%],WM.MINWASTPER [NEW MINWAST%],T.MINWAST [OLD MINWASTAGE],"
        ''strSql += vbCrLf + " (CASE WHEN T.GRSNET='G' THEN CONVERT(NUMERIC(15,3),(WM.MINWASTPER * T.GRSWT)/100)"
        ''strSql += vbCrLf + " ELSE CONVERT(NUMERIC(15,3),(WM.MINWASTPER * T.NETWT)/100) END )AS [NEW MINWASTAGE]"
        strSql += vbCrLf + " (CASE "
        strSql += vbCrLf + " WHEN T.GRSNET='G' AND WM.MINWASTPER<>0 THEN CONVERT(NUMERIC(15,3),(WM.MINWASTPER * T.GRSWT)/100) "
        strSql += vbCrLf + " WHEN T.GRSNET='N' AND WM.MINWASTPER<>0 THEN CONVERT(NUMERIC(15,3),(WM.MINWASTPER * T.NETWT)/100) "
        strSql += vbCrLf + " WHEN T.GRSNET='G' AND WM.MINWASTPER=0  THEN CONVERT(NUMERIC(15,3), WM.MINWAST) "
        strSql += vbCrLf + " WHEN T.GRSNET='N' AND WM.MINWASTPER=0  THEN CONVERT(NUMERIC(15,3), WM.MINWAST) "
        strSql += vbCrLf + " END)AS [NEW MINWASTAGE]"
        strSql += vbCrLf + " ,T.MINMCGRM [OLD MINMCGRM],WM.MINMCGRM [NEW MINMCGRM],T.MINMC [OLD MINMC],CASE WHEN WM.MINMCGRM = 0 THEN WM.MINMC ELSE CONVERT(NUMERIC(15,2),(WM.MINMCGRM*T.GRSWT)) END  AS [NEW MINMC]"
        strSql += vbCrLf + " FROM " & cnAdminDb & ".." & TagTable & " T "
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..WMCTABLE WM  "
        If UCase(cmbType.Text) = "ITEM" Then
            strSql += vbCrLf + " ON T.ITEMID=WM.ITEMID AND T.SUBITEMID=WM.SUBITEMID "
            If cmbtagtype.Text <> "ALL" And cmbtagtype.Text <> "" Then strSql += vbCrLf + " AND ISNULL(WM.ITEMTYPE,'')=T.ITEMTYPEID"
            If ChkCmbItem.Text <> "ALL" And ChkCmbItem.Text <> "" Then
                strSql += vbCrLf + " AND T.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & ItemName & "))"
            End If
            If ChkCmbSubItem.Text <> "ALL" And ChkCmbSubItem.Text <> "" Then
                strSql += vbCrLf + " AND T.SUBITEMID IN (SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME IN (" & SubItemName & "))"
            End If
            If cmbtagtype.Text <> "ALL" And cmbtagtype.Text <> "" Then
                strSql += vbCrLf + " AND T.ITEMTYPEID IN (SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME IN (" & ItemTypeName & ") )"
            End If
        ElseIf UCase(cmbType.Text) = "DESIGNER" Then
            strSql += vbCrLf + " ON T.DESIGNERID=WM.DESIGNERID"
            If ChkCmbDesigner.Text <> "ALL" Then
                strSql += vbCrLf + " AND  T.DESIGNERID IN (SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME IN  (" & ftrDesigner & ")) "
                strSql += vbCrLf + " AND T.ITEMID=WM.ITEMID AND T.SUBITEMID=WM.SUBITEMID"
            End If
            If ChkCmbItemDes.Text <> "ALL" And ChkCmbItemDes.Text <> "" Then
                strSql += vbCrLf + " AND T.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & ItemNameDes & "))"
            End If
            If ChkCmbSubItemDes.Text <> "ALL" And ChkCmbSubItemDes.Text <> "" Then
                strSql += vbCrLf + " AND T.SUBITEMID IN (SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME IN (" & SubItemNameDes & "))"
            End If
        ElseIf UCase(cmbType.Text) = "TABLE" Then
            strSql += vbCrLf + " ON WM.TABLECODE =T.TABLECODE"
            If CmbTable.Text <> "" Then strSql += " AND T.TABLECODE = '" & CmbTable.Text & "'"
        ElseIf UCase(cmbType.Text) = "TAGTYPE" Then
            strSql += vbCrLf + " ON T.ITEMTYPEID=WM.ITEMTYPE "
            If ChkCmbTagType.Text <> "" Then strSql += " AND T.ITEMTYPEID = (SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME='" & ChkCmbTagType.Text & "')"
        End If
        If flag Then
            If rbtGrsWt.Checked Then
                strSql += vbCrLf + " AND T.GRSWT BETWEEN WM.FROMWEIGHT AND WM.TOWEIGHT "
            Else
                strSql += vbCrLf + " AND T.NETWT BETWEEN WM.FROMWEIGHT AND WM.TOWEIGHT "
            End If
        End If
        If costcentre = True And maincostid <> cnCostId Then strSql += vbCrLf + " AND T.COSTID=WM.COSTID"
        strSql += vbCrLf + " AND ISSDATE IS NULL"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID = T.ITEMID"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..SUBITEMMAST SM ON SM.ITEMID = T.ITEMID AND SM.SUBITEMID=T.SUBITEMID  AND SM.SUBITEMID <> 0"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMTYPE TT ON TT.ITEMTYPEID = T.ITEMTYPEID"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + "SELECT SNO,TAGNO,"
        strSql += vbCrLf + " IM.ITEMNAME AS ITEM"
        strSql += vbCrLf + " ,SM.SUBITEMNAME AS SUBITEM,T.GRSWT,T.NETWT"
        strSql += vbCrLf + " ,T.MAXWASTPER [OLD MAXWAST%],WM.MAXWASTPER [NEW MAXWAST%],T.MAXWAST [OLD MAXWASTAGE],"
        If _WASTONGRSNET Then
            strSql += vbCrLf + " (CASE "
            strSql += vbCrLf + " WHEN T.GRSNET='G' AND WM.MAXWASTPER<>0 THEN CONVERT(NUMERIC(15,3),(WM.MAXWASTPER * T.GRSWT)/100) "
            strSql += vbCrLf + " WHEN T.GRSNET='N' AND WM.MAXWASTPER<>0 THEN CONVERT(NUMERIC(15,3),(WM.MAXWASTPER * T.NETWT)/100) "
            strSql += vbCrLf + " WHEN T.GRSNET='G' AND WM.MAXWASTPER=0  THEN CONVERT(NUMERIC(15,3), WM.MAXWAST) "
            strSql += vbCrLf + " WHEN T.GRSNET='N' AND WM.MAXWASTPER=0  THEN CONVERT(NUMERIC(15,3), WM.MAXWAST) "
            strSql += vbCrLf + " END)AS [NEW MAXWASTAGE]"
        Else
            strSql += vbCrLf + " (CASE "
            strSql += vbCrLf + " WHEN T.SUBITEMID<>0 AND SM.MCCALC='G'  AND WM.MAXWASTPER<>0  THEN CONVERT(NUMERIC(15,3),(WM.MAXWASTPER * T.GRSWT)/100) "
            strSql += vbCrLf + " WHEN T.SUBITEMID<>0 AND SM.MCCALC='N'  AND WM.MAXWASTPER<>0  THEN CONVERT(NUMERIC(15,3),(WM.MAXWASTPER * T.NETWT)/100) "
            strSql += vbCrLf + " WHEN T.SUBITEMID=0  AND IM.MCCALC='N'  AND WM.MAXWASTPER<>0  THEN CONVERT(NUMERIC(15,3),(WM.MAXWASTPER * T.NETWT)/100) "
            strSql += vbCrLf + " WHEN T.SUBITEMID=0  AND IM.MCCALC='G'  AND WM.MAXWASTPER<>0  THEN CONVERT(NUMERIC(15,3),(WM.MAXWASTPER * T.GRSWT)/100) "
            strSql += vbCrLf + " WHEN T.SUBITEMID<>0 AND SM.MCCALC='G'  AND WM.MAXWASTPER=0   THEN CONVERT(NUMERIC(15,3),WM.MAXWAST) "
            strSql += vbCrLf + " WHEN T.SUBITEMID<>0 AND SM.MCCALC='N'  AND WM.MAXWASTPER=0   THEN CONVERT(NUMERIC(15,3),WM.MAXWAST) "
            strSql += vbCrLf + " WHEN T.SUBITEMID=0  AND IM.MCCALC='N'  AND WM.MAXWASTPER=0   THEN CONVERT(NUMERIC(15,3),WM.MAXWAST) "
            strSql += vbCrLf + " WHEN T.SUBITEMID=0  AND IM.MCCALC='G'  AND WM.MAXWASTPER=0   THEN CONVERT(NUMERIC(15,3),WM.MAXWAST) "
            strSql += vbCrLf + " END ) AS [NEW MAXWASTAGE]"
        End If
        strSql += vbCrLf + " ,T.MAXMCGRM [OLD MAXMCGRM],WM.MAXMCGRM [NEW MAXMCGRM],T.MAXMC [OLD MAXMC]"
        If _WASTONGRSNET Then
            strSql += vbCrLf + " ,CASE WHEN WM.MAXMCGRM = 0 THEN WM.MAXMC "
            strSql += vbCrLf + " ELSE CONVERT(NUMERIC(15,2),(WM.MAXMCGRM*T.GRSWT)) END AS [NEW MAXMC]"
        Else
            If McWithWastage Then
                strSql += vbCrLf + " ,CASE WHEN WM.MAXMCGRM = 0 THEN WM.MAXMC "
                strSql += vbCrLf + " ELSE "
                strSql += vbCrLf + " CASE WHEN T.SUBITEMID<>0 AND SM.MCCALC='G' THEN "
                strSql += vbCrLf + " CONVERT(NUMERIC(15,2),(WM.MAXMCGRM*(T.GRSWT+CASE WHEN T.SUBITEMID<>0 AND SM.MCCALC='G' THEN CONVERT(NUMERIC(15,3),(WM.MAXWASTPER * T.GRSWT)/100) "
                strSql += vbCrLf + " WHEN T.SUBITEMID<>0 AND SM.MCCALC='N' THEN CONVERT(NUMERIC(15,3),(WM.MAXWASTPER * T.NETWT)/100) "
                strSql += vbCrLf + " WHEN T.SUBITEMID=0 AND IM.MCCALC='N' THEN CONVERT(NUMERIC(15,3),(WM.MAXWASTPER * T.NETWT)/100) "
                strSql += vbCrLf + " WHEN T.SUBITEMID=0 AND IM.MCCALC='G' THEN CONVERT(NUMERIC(15,3),(WM.MAXWASTPER * T.GRSWT)/100) "
                strSql += vbCrLf + " ELSE CONVERT(NUMERIC(15,3),(WM.MAXWASTPER * T.NETWT)/100) END))) "
                strSql += vbCrLf + " WHEN T.SUBITEMID<>0 AND SM.MCCALC='N' THEN "
                strSql += vbCrLf + " CONVERT(NUMERIC(15,2),(WM.MAXMCGRM*(T.NETWT+CASE WHEN T.SUBITEMID<>0 AND SM.MCCALC='G' THEN CONVERT(NUMERIC(15,3),(WM.MAXWASTPER * T.GRSWT)/100) "
                strSql += vbCrLf + " WHEN T.SUBITEMID<>0 AND SM.MCCALC='N' THEN CONVERT(NUMERIC(15,3),(WM.MAXWASTPER * T.NETWT)/100) "
                strSql += vbCrLf + " WHEN T.SUBITEMID=0 AND IM.MCCALC='N' THEN CONVERT(NUMERIC(15,3),(WM.MAXWASTPER * T.NETWT)/100) "
                strSql += vbCrLf + " WHEN T.SUBITEMID=0 AND IM.MCCALC='G' THEN CONVERT(NUMERIC(15,3),(WM.MAXWASTPER * T.GRSWT)/100) "
                strSql += vbCrLf + " ELSE CONVERT(NUMERIC(15,3),(WM.MAXWASTPER * T.NETWT)/100) END))) "
                strSql += vbCrLf + " WHEN T.SUBITEMID=0 AND SM.MCCALC='N' THEN "
                strSql += vbCrLf + " CONVERT(NUMERIC(15,2),(WM.MAXMCGRM*(T.NETWT+CASE WHEN T.SUBITEMID<>0 AND SM.MCCALC='G' THEN CONVERT(NUMERIC(15,3),(WM.MAXWASTPER * T.GRSWT)/100) "
                strSql += vbCrLf + " WHEN T.SUBITEMID<>0 AND SM.MCCALC='N' THEN CONVERT(NUMERIC(15,3),(WM.MAXWASTPER * T.NETWT)/100) "
                strSql += vbCrLf + " WHEN T.SUBITEMID=0 AND IM.MCCALC='N' THEN CONVERT(NUMERIC(15,3),(WM.MAXWASTPER * T.NETWT)/100) "
                strSql += vbCrLf + " WHEN T.SUBITEMID=0 AND IM.MCCALC='G' THEN CONVERT(NUMERIC(15,3),(WM.MAXWASTPER * T.GRSWT)/100) "
                strSql += vbCrLf + " ELSE CONVERT(NUMERIC(15,3),(WM.MAXWASTPER * T.NETWT)/100) END))) "
                strSql += vbCrLf + " WHEN T.SUBITEMID=0 AND SM.MCCALC='G' "
                strSql += vbCrLf + " THEN CONVERT(NUMERIC(15,2),(WM.MAXMCGRM*(T.GRSWT+CASE WHEN T.SUBITEMID<>0 AND SM.MCCALC='G' THEN CONVERT(NUMERIC(15,3),(WM.MAXWASTPER * T.GRSWT)/100) "
                strSql += vbCrLf + " WHEN T.SUBITEMID<>0 AND SM.MCCALC='N' THEN CONVERT(NUMERIC(15,3),(WM.MAXWASTPER * T.NETWT)/100) "
                strSql += vbCrLf + " WHEN T.SUBITEMID=0 AND IM.MCCALC='N' THEN CONVERT(NUMERIC(15,3),(WM.MAXWASTPER * T.NETWT)/100) "
                strSql += vbCrLf + " WHEN T.SUBITEMID=0 AND IM.MCCALC='G' THEN CONVERT(NUMERIC(15,3),(WM.MAXWASTPER * T.GRSWT)/100) "
                strSql += vbCrLf + " ELSE CONVERT(NUMERIC(15,3),(WM.MAXWASTPER * T.NETWT)/100) END))) "
                strSql += vbCrLf + " END END AS [NEW MAXMC]"
            Else
                strSql += vbCrLf + " ,CASE WHEN WM.MAXMCGRM = 0 THEN WM.MAXMC "
                strSql += vbCrLf + " ELSE "
                strSql += vbCrLf + " CASE WHEN T.SUBITEMID<>0 AND SM.MCCALC='G' THEN CONVERT(NUMERIC(15,2),(WM.MAXMCGRM*T.GRSWT)) "
                strSql += vbCrLf + " WHEN T.SUBITEMID<>0 AND SM.MCCALC='N' THEN CONVERT(NUMERIC(15,2),(WM.MAXMCGRM*T.NETWT)) "
                strSql += vbCrLf + " WHEN T.SUBITEMID=0 AND SM.MCCALC='N' THEN CONVERT(NUMERIC(15,2),(WM.MAXMCGRM*T.NETWT)) "
                strSql += vbCrLf + " WHEN T.SUBITEMID=0 AND SM.MCCALC='G' THEN CONVERT(NUMERIC(15,2),(WM.MAXMCGRM*T.GRSWT)) "
                strSql += vbCrLf + " END END AS [NEW MAXMC]"
            End If
        End If
        strSql += vbCrLf + " ,T.MINWASTPER  [OLD MINWAST%],WM.MINWASTPER [NEW MINWAST%],T.MINWAST [OLD MINWASTAGE],"
        strSql += vbCrLf + " (CASE WHEN T.GRSNET='G' THEN CONVERT(NUMERIC(15,3),(WM.MINWASTPER * T.GRSWT)/100)"
        strSql += vbCrLf + " ELSE CONVERT(NUMERIC(15,3),(WM.MINWASTPER * T.NETWT)/100) END )AS [NEW MINWASTAGE]"
        strSql += vbCrLf + " ,T.MINMCGRM [OLD MINMCGRM],WM.MINMCGRM [NEW MINMCGRM],T.MINMC [OLD MINMC],CASE WHEN WM.MINMCGRM = 0 THEN WM.MINMC ELSE CONVERT(NUMERIC(15,2),(WM.MINMCGRM*T.GRSWT)) END  AS [NEW MINMC]"
        strSql += vbCrLf + " FROM " & cnAdminDb & ".." & TagTable & " T "
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..WMCTABLE WM  "
        If UCase(cmbType.Text) = "ITEM" Then
            strSql += vbCrLf + " ON T.ITEMID=WM.ITEMID AND WM.SUBITEMID=0 AND WM.SUBITEMID NOT IN (SELECT DISTINCT SUBITEMID FROM " & cnAdminDb & "..WMCTABLE WHERE SUBITEMID <> 0)  "
            If cmbtagtype.Text <> "ALL" And cmbtagtype.Text <> "" Then strSql += vbCrLf + " AND ISNULL(WM.ITEMTYPE,'')=T.ITEMTYPEID"
            If ChkCmbItem.Text <> "ALL" And ChkCmbItem.Text <> "" Then
                strSql += vbCrLf + " AND T.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & ItemName & "))"
            End If
            If ChkCmbSubItem.Text <> "ALL" And ChkCmbSubItem.Text <> "" Then
                strSql += vbCrLf + " AND T.SUBITEMID IN (SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME IN (" & SubItemName & "))"
            End If
            If cmbtagtype.Text <> "ALL" And cmbtagtype.Text <> "" Then
                strSql += vbCrLf + " AND T.ITEMTYPEID IN (SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME IN (" & ItemTypeName & ") )"
            End If
        ElseIf UCase(cmbType.Text) = "DESIGNER" Then
            strSql += vbCrLf + " ON T.DESIGNERID=WM.DESIGNERID"
            If ChkCmbDesigner.Text <> "ALL" Then
                strSql += vbCrLf + " AND  T.DESIGNERID IN (SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME IN  (" & ftrDesigner & ")) "
                strSql += vbCrLf + " AND T.ITEMID=WM.ITEMID AND T.SUBITEMID=0 AND WM.SUBITEMID NOT IN (SELECT DISTINCT SUBITEMID FROM " & cnAdminDb & "..WMCTABLE WHERE SUBITEMID <> 0) "
            End If
            If ChkCmbItemDes.Text <> "ALL" And ChkCmbItemDes.Text <> "" Then
                strSql += vbCrLf + " AND T.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & ItemNameDes & "))"
            End If
            If ChkCmbSubItemDes.Text <> "ALL" And ChkCmbSubItemDes.Text <> "" Then
                strSql += vbCrLf + " AND T.SUBITEMID IN (SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME IN (" & SubItemNameDes & "))"
            End If
        ElseIf UCase(cmbType.Text) = "TABLE" Then
            strSql += vbCrLf + " ON WM.TABLECODE =T.TABLECODE"
            If CmbTable.Text <> "" Then strSql += " AND T.TABLECODE = '" & CmbTable.Text & "'"
        ElseIf UCase(cmbType.Text) = "TAGTYPE" Then
            strSql += vbCrLf + " ON T.ITEMTYPEID=WM.ITEMTYPE "
            If ChkCmbTagType.Text <> "" Then strSql += " AND T.ITEMTYPEID = (SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME='" & ChkCmbTagType.Text & "')"
        End If
        If flag Then
            If rbtGrsWt.Checked Then
                strSql += vbCrLf + " AND T.GRSWT BETWEEN WM.FROMWEIGHT AND WM.TOWEIGHT "
            Else
                strSql += vbCrLf + " AND T.NETWT BETWEEN WM.FROMWEIGHT AND WM.TOWEIGHT "
            End If
        End If
        If costcentre = True And maincostid <> cnCostId Then strSql += vbCrLf + " AND T.COSTID=WM.COSTID"
        strSql += vbCrLf + " AND ISSDATE IS NULL"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID = T.ITEMID"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..SUBITEMMAST SM ON SM.ITEMID = T.ITEMID AND SM.SUBITEMID=T.SUBITEMID AND SM.SUBITEMID NOT IN (SELECT DISTINCT SUBITEMID FROM " & cnAdminDb & "..WMCTABLE WHERE SUBITEMID <> 0) "
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMTYPE TT ON TT.ITEMTYPEID = T.ITEMTYPEID"
        strSql += vbCrLf + " ORDER BY 2,3"
        da = New OleDbDataAdapter
        dt = New DataTable
        Dim dtCol As New DataColumn("CHECK", GetType(Boolean))
        dtCol.DefaultValue = True
        dt.Columns.Add(dtCol)
        da = New OleDbDataAdapter(strSql, cn)
        ProgressBarStep("Fill into Data", 10)
        da.Fill(dt)
        dt.AcceptChanges()
        If dt.Rows.Count > 0 Then
            gridView.DataSource = Nothing
            ProgressBarStep("Fill into Grid", 10)
            gridView.DataSource = dt
            FormatGridColumns(gridView, False, False, , False)
            gridView.Columns("CHECK").ReadOnly = False
            gridView.Columns("SNO").Visible = False
            gridView.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
            gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        Else
            MsgBox("No Record Found", MsgBoxStyle.Information, "BrighttechGold")
        End If
        'Me.Cursor = Cursors.Arrow
        ProgressBarHide()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        objGPack.TextClear(Me)
        tabMain.SelectedTab = tabGeneral
        ChkCmbItem.Items.Clear()
        ChkCmbSubItem.Items.Clear()
        ChkCmbDesigner.Items.Clear()
        cmbType.Select()
       
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGeneral
        cmbType.Select()
    End Sub

    Private Sub cmbType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbType.SelectedIndexChanged
        objGPack.TextClear(tabMode)
        Select Case cmbType.Text.ToUpper
            Case "ITEM"
                tabMode.SelectedTab = tabitem
                ChkCmbItemLoad()
            Case "TAGTYPE"
                tabMode.SelectedTab = tabTagType
                ChkCmbTagLoad()
            Case "DESIGNER"
                tabMode.SelectedTab = tabDesigner
                chkcmbDesignerLoad()
                ChkCmbItemLoadDes()
            Case "TABLE"
                tabMode.SelectedTab = tabTable
                chkcmbTableLoad()
        End Select
        cmbType.Select()
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
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.ExecuteNonQuery()
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
        qry = "UPDATE " & cnAdminDb & ".." & TagTable & " SET "
        qry += vbCrLf + " MAXWASTPER=" & Val(ro.Item("NEW MAXWAST%").ToString) & ""
        qry += vbCrLf + " ,MAXWAST=" & Val(ro.Item("NEW MAXWASTAGE").ToString) & ""
        qry += vbCrLf + " ,MAXMCGRM=" & Val(ro.Item("NEW MAXMCGRM").ToString) & ""
        qry += vbCrLf + " ,MAXMC=" & Val(ro.Item("NEW MAXMC").ToString) & ""
        qry += vbCrLf + " ,MINWASTPER=" & Val(ro.Item("NEW MINWAST%").ToString) & ""
        qry += vbCrLf + " ,MINWAST=" & Val(ro.Item("NEW MINWASTAGE").ToString) & ""
        qry += vbCrLf + " ,MINMCGRM=" & Val(ro.Item("NEW MINMCGRM").ToString) & ""
        qry += vbCrLf + " ,MINMC = " & Val(ro.Item("NEW MINMC").ToString) & ""
        qry += vbCrLf + " WHERE SNO='" & ro.Item("SNO").ToString & "'"
        qry += vbCrLf + " AND TAGNO='" & ro.Item("TAGNO").ToString & "'"
        Return qry
    End Function

    Private Sub ChkCmbItem_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkCmbItem.Leave
        ChkCmbSubitemLoad()
    End Sub

    
    Private Sub ChkCmbItem_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkCmbItem.SelectedIndexChanged
        ChkCmbSubitemLoad()
    End Sub

    Private Sub ChkCmbItem_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkCmbItem.SelectionChangeCommitted
        ChkCmbSubitemLoad()
    End Sub

    Private Sub ChkCmbItemDes_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChkCmbItemDes.Leave
        ChkCmbSubitemLoadDes()
    End Sub

End Class