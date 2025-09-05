Imports System.Data.OleDb
Public Class frmHallMarkMaterialIssueReceipt
    Dim Da As OleDbDataAdapter
    Dim Cmd As OleDbCommand
    Dim StrSql As String
    Dim _AccAudit As Boolean = IIf(GetAdmindbSoftValue("ACC_AUDIT", "N") = "Y", True, False)
    Dim STOCKVALIDATION As Boolean = True 'IIf(GetAdmindbSoftValue("MRMISTOCKLOCK", "N") = "Y", True, False)

    Private Sub frmHallMarkMaterialIssueReceipt_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmHallMarkMaterialIssueReceipt_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        btnNew_Click(Me, New EventArgs)
        gridView.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        dtpAsOnDate.Value = GetEntryDate(GetServerDate)
        dtpToDate.Value = GetEntryDate(GetServerDate)
        dtpTrandate.Value = GetEntryDate(GetServerDate)
        StrSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACTIVE = 'Y' ORDER BY ACNAME"
        objGPack.FillCombo(StrSql, cmbDesigner_MAN)
        cmbDesigner_MAN.Enabled = True
        gridView.DataSource = Nothing
        chkSelectAll.Checked = False
        btnIssue.Enabled = False
        lblHelp.Text = ""
        ChkAsOn.Select()
        Prop_Gets()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub gridView_CurrentCellDirtyStateChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles gridView.CurrentCellDirtyStateChanged
        gridView.CommitEdit(DataGridViewDataErrorContexts.Commit)
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        gridView.DataSource = Nothing
        gridView.Refresh()
        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("CHECK", GetType(Boolean))
        dtGrid.Columns("CHECK").DefaultValue = chkSelectAll.Checked
        Prop_Sets()
        If STOCKVALIDATION Then
            If rbtIssue.Checked Then
                StrSql = vbCrLf + "SELECT "
                StrSql += vbCrLf + "(SELECT TRANNO FROM " & cnStockDb & "..RECEIPT WHERE SNO=X.SNO)TRANNO"
                StrSql += vbCrLf + ",(SELECT TRANDATE FROM " & cnStockDb & "..RECEIPT WHERE SNO=X.SNO)TRANDATE"
                StrSql += vbCrLf + ",(SELECT TOP 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE="
                StrSql += vbCrLf + "(SELECT TOP 1 ACCODE FROM " & cnStockDb & "..RECEIPT WHERE SNO = X.SNO))ACNAME"
                StrSql += vbCrLf + ",(SELECT TOP 1 ITEMNAME FROM " & cnAdminDb & "..ITEMMAST  WHERE ITEMID ="
                StrSql += vbCrLf + "(SELECT TOP 1 ITEMID FROM " & cnStockDb & "..RECEIPT WHERE SNO = X.SNO))ITEM"
                StrSql += vbCrLf + ",(SELECT TOP 1 SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST  WHERE SUBITEMID ="
                StrSql += vbCrLf + "(SELECT TOP 1 SUBITEMID FROM " & cnStockDb & "..RECEIPT WHERE SNO = X.SNO))SUBITEM"
                StrSql += vbCrLf + ",SUM(PCS)PCS,SUM(GRSWT)GRSWT"
                StrSql += vbCrLf + ",SUM(NETWT)NETWT,SUM(PUREWT)PUREWT"
                'StrSql += vbCrLf + ",SUM(WASTAGE)WASTAGE ,SUM(MCHARGE)MCHARGE"
                StrSql += vbCrLf + ",(SELECT TOUCH FROM " & cnStockDb & "..RECEIPT WHERE SNO=X.SNO)TOUCH"
                StrSql += vbCrLf + ",(SELECT ITEMTYPEID FROM " & cnStockDb & "..RECEIPT WHERE SNO=X.SNO)ITEMTYPEID"
                StrSql += vbCrLf + ",(SELECT TOP 1 ACCODE FROM " & cnStockDb & "..RECEIPT WHERE SNO = X.SNO)ACCODE"
                StrSql += vbCrLf + ",(SELECT TOP 1 CATCODE FROM " & cnStockDb & "..RECEIPT WHERE SNO = X.SNO)CATCODE"
                StrSql += vbCrLf + ",(SELECT TOP 1 OCATCODE FROM " & cnStockDb & "..RECEIPT WHERE SNO = X.SNO)OCATCODE"
                StrSql += vbCrLf + ",(SELECT TOP 1 METALID FROM " & cnStockDb & "..RECEIPT WHERE SNO = X.SNO)METALID"
                StrSql += vbCrLf + ",SNO"
                StrSql += vbCrLf + "FROM ("
                StrSql += vbCrLf + "SELECT "
                StrSql += vbCrLf + "SUM(R.PCS)PCS"
                StrSql += vbCrLf + ",SUM(R.GRSWT)GRSWT"
                StrSql += vbCrLf + ",SUM(R.NETWT)NETWT"
                StrSql += vbCrLf + ",SUM(R.PUREWT)PUREWT"
                StrSql += vbCrLf + ",SUM(R.WASTAGE)WASTAGE"
                StrSql += vbCrLf + ",SUM(R.MCHARGE)MCHARGE"
                StrSql += vbCrLf + ",R.SNO "
                StrSql += vbCrLf + "FROM " & cnStockDb & "..RECEIPT R"
                If ChkAsOn.Checked Then
                    StrSql += vbCrLf + "WHERE R.TRANDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
                Else
                    StrSql += vbCrLf + "WHERE R.TRANDATE BETWEEN  '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpToDate.Value.ToString("yyyy-MM-dd") & "'"
                End If
                StrSql += vbCrLf + "AND ISNULL(R.CANCEL,'') = '' "
                StrSql += vbCrLf + "AND (R.GRSWT <> 0 OR R.PCS<>0)"
                StrSql += vbCrLf + "AND LEN(R.TRANTYPE) > 2 "
                If cnCostId <> "" Then StrSql += vbCrLf + "AND R.COSTID='" & cnCostId & "'"
                StrSql += vbCrLf + "AND ISNULL(R.ITEMID,0)<>0 "
                StrSql += vbCrLf + "GROUP BY R.SNO"
                StrSql += vbCrLf + "UNION ALL"
                StrSql += vbCrLf + "SELECT "
                StrSql += vbCrLf + "-1*SUM(I.PCS)PCS"
                StrSql += vbCrLf + ",-1*SUM(I.GRSWT)GRSWT"
                StrSql += vbCrLf + ",-1*SUM(I.NETWT)NETWT"
                StrSql += vbCrLf + ",-1*SUM(I.PUREWT)PUREWT"
                StrSql += vbCrLf + ",-1*SUM(I.WASTAGE)WASTAGE"
                StrSql += vbCrLf + ",-1*SUM(I.MCHARGE)MCHARGE"
                StrSql += vbCrLf + ",I.RESNO AS SNO"
                StrSql += vbCrLf + "FROM " & cnStockDb & "..ISSUE I"
                StrSql += vbCrLf + "WHERE ISNULL(I.CANCEL,'') = '' "
                StrSql += vbCrLf + "AND (I.GRSWT <> 0 OR I.PCS<>0)"
                StrSql += vbCrLf + "AND LEN(I.TRANTYPE) > 2 "
                If cnCostId <> "" Then StrSql += vbCrLf + "AND I.COSTID='" & cnCostId & "'"
                StrSql += vbCrLf + "AND ISNULL(I.ITEMID,0)<>0 "
                StrSql += vbCrLf + "AND I.RESNO IN"
                StrSql += vbCrLf + "(SELECT SNO FROM " & cnStockDb & "..RECEIPT WHERE "
                If ChkAsOn.Checked Then
                    StrSql += vbCrLf + " TRANDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
                Else
                    StrSql += vbCrLf + " TRANDATE BETWEEN  '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpToDate.Value.ToString("yyyy-MM-dd") & "'"
                End If
                StrSql += vbCrLf + "AND ISNULL(CANCEL,'') = ''  "
                StrSql += vbCrLf + "AND LEN(TRANTYPE)>2"
                If cnCostId <> "" Then StrSql += vbCrLf + "AND COSTID='" & cnCostId & "'"
                StrSql += vbCrLf + ")"
                StrSql += vbCrLf + "GROUP BY I.RESNO "
                StrSql += vbCrLf + "UNION ALL"
                StrSql += vbCrLf + "SELECT "
                StrSql += vbCrLf + "-1*SUM(I.PCS)PCS"
                StrSql += vbCrLf + ",-1*SUM(I.GRSWT)GRSWT"
                StrSql += vbCrLf + ",-1*SUM(I.NETWT)NETWT"
                StrSql += vbCrLf + ",0 PUREWT"
                StrSql += vbCrLf + ",0 WASTAGE"
                StrSql += vbCrLf + ",0 MCHARGE"
                StrSql += vbCrLf + ",I.RECSNO AS SNO "
                StrSql += vbCrLf + "FROM " & cnStockDb & "..LOTISSUE I"
                StrSql += vbCrLf + "WHERE ISNULL(I.CANCEL,'') = '' "
                StrSql += vbCrLf + "AND ISNULL(I.ITEMID,0)<>0 "
                StrSql += vbCrLf + "AND I.RECSNO IN"
                StrSql += vbCrLf + "(SELECT SNO FROM " & cnStockDb & "..RECEIPT WHERE "
                If ChkAsOn.Checked Then
                    StrSql += vbCrLf + "TRANDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
                Else
                    StrSql += vbCrLf + "TRANDATE BETWEEN  '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpToDate.Value.ToString("yyyy-MM-dd") & "'"
                End If
                StrSql += vbCrLf + "AND ISNULL(CANCEL,'') = ''  "
                StrSql += vbCrLf + "AND LEN(TRANTYPE)>2"
                If cnCostId <> "" Then StrSql += vbCrLf + "AND COSTID='" & cnCostId & "'"
                StrSql += vbCrLf + ")"
                StrSql += vbCrLf + "GROUP BY I.RECSNO "
                StrSql += vbCrLf + ")X GROUP BY SNO HAVING (SUM(PCS)>0 OR SUM(GRSWT)>0)"
                StrSql += vbCrLf + "ORDER BY TRANDATE,TRANNO"
            ElseIf rdbReceipt.Checked Then
                StrSql = vbCrLf + "SELECT "
                StrSql += vbCrLf + "(SELECT TRANNO FROM " & cnStockDb & "..ISSUE WHERE SNO=X.SNO)TRANNO"
                StrSql += vbCrLf + ",(SELECT TRANDATE FROM " & cnStockDb & "..ISSUE WHERE SNO=X.SNO)TRANDATE"
                StrSql += vbCrLf + ",(SELECT TOP 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE="
                StrSql += vbCrLf + "(SELECT TOP 1 ACCODE FROM " & cnStockDb & "..ISSUE WHERE SNO = X.SNO))ACNAME"
                StrSql += vbCrLf + ",(SELECT TOP 1 ITEMNAME FROM " & cnAdminDb & "..ITEMMAST  WHERE ITEMID ="
                StrSql += vbCrLf + "(SELECT TOP 1 ITEMID FROM " & cnStockDb & "..ISSUE WHERE SNO = X.SNO))ITEM"
                StrSql += vbCrLf + ",(SELECT TOP 1 SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST  WHERE SUBITEMID ="
                StrSql += vbCrLf + "(SELECT TOP 1 SUBITEMID FROM " & cnStockDb & "..ISSUE WHERE SNO = X.SNO))SUBITEM"
                StrSql += vbCrLf + ",SUM(PCS)PCS,SUM(GRSWT)GRSWT"
                StrSql += vbCrLf + ",SUM(NETWT)NETWT,SUM(PUREWT)PUREWT"
                'StrSql += vbCrLf + ",SUM(WASTAGE)WASTAGE,SUM(MCHARGE)MCHARGE"
                StrSql += vbCrLf + ",(SELECT TOUCH FROM " & cnStockDb & "..ISSUE WHERE SNO=X.SNO)TOUCH"
                StrSql += vbCrLf + ",(SELECT ITEMTYPEID FROM " & cnStockDb & "..ISSUE WHERE SNO=X.SNO)ITEMTYPEID"
                StrSql += vbCrLf + ",(SELECT TOP 1 ACCODE FROM " & cnStockDb & "..ISSUE WHERE SNO = X.SNO)ACCODE"
                StrSql += vbCrLf + ",(SELECT TOP 1 CATCODE FROM " & cnStockDb & "..ISSUE WHERE SNO = X.SNO)CATCODE"
                StrSql += vbCrLf + ",(SELECT TOP 1 OCATCODE FROM " & cnStockDb & "..ISSUE WHERE SNO = X.SNO)OCATCODE"
                StrSql += vbCrLf + ",(SELECT TOP 1 METALID FROM " & cnStockDb & "..ISSUE WHERE SNO = X.SNO)METALID"
                StrSql += vbCrLf + ",SNO"
                StrSql += vbCrLf + "FROM ("
                StrSql += vbCrLf + "SELECT "
                StrSql += vbCrLf + "SUM(R.PCS)PCS"
                StrSql += vbCrLf + ",SUM(R.GRSWT)GRSWT"
                StrSql += vbCrLf + ",SUM(R.NETWT)NETWT"
                StrSql += vbCrLf + ",SUM(R.PUREWT)PUREWT"
                StrSql += vbCrLf + ",SUM(R.WASTAGE)WASTAGE"
                StrSql += vbCrLf + ",SUM(R.MCHARGE)MCHARGE"
                StrSql += vbCrLf + ",R.SNO "
                StrSql += vbCrLf + "FROM " & cnStockDb & "..ISSUE R WHERE"
                If ChkAsOn.Checked Then
                    StrSql += vbCrLf + "R.TRANDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
                Else
                    StrSql += vbCrLf + "R.TRANDATE BETWEEN  '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpToDate.Value.ToString("yyyy-MM-dd") & "'"
                End If
                StrSql += vbCrLf + "AND ISNULL(R.CANCEL,'') = '' "
                StrSql += vbCrLf + "AND ISNULL(TRANTYPE,'') = 'IHM' "
                StrSql += vbCrLf + "AND R.GRSWT <> 0 AND LEN(R.TRANTYPE) > 2 "
                If cnCostId <> "" Then StrSql += vbCrLf + "AND R.COSTID='" & cnCostId & "'"
                StrSql += vbCrLf + "AND ISNULL(R.ITEMID,0)<>0 "
                StrSql += vbCrLf + "GROUP BY R.SNO"
                StrSql += vbCrLf + "UNION ALL"
                StrSql += vbCrLf + "SELECT "
                StrSql += vbCrLf + "-1*SUM(I.PCS)PCS"
                StrSql += vbCrLf + ",-1*SUM(I.GRSWT)GRSWT"
                StrSql += vbCrLf + ",-1*SUM(I.NETWT)NETWT"
                StrSql += vbCrLf + ",-1*SUM(I.PUREWT)PUREWT"
                StrSql += vbCrLf + ",-1*SUM(I.WASTAGE)WASTAGE"
                StrSql += vbCrLf + ",-1*SUM(I.MCHARGE)MCHARGE"
                StrSql += vbCrLf + ",I.REFNO AS SNO "
                StrSql += vbCrLf + "FROM " & cnStockDb & "..RECEIPT I"
                StrSql += vbCrLf + "WHERE ISNULL(I.CANCEL,'') = '' "
                StrSql += vbCrLf + "AND ISNULL(TRANTYPE,'') = 'RHM' "
                StrSql += vbCrLf + "AND I.GRSWT <> 0 AND LEN(I.TRANTYPE) > 2 "
                If cnCostId <> "" Then StrSql += vbCrLf + "AND I.COSTID='" & cnCostId & "'"
                StrSql += vbCrLf + "AND ISNULL(I.ITEMID,0)<>0 "
                StrSql += vbCrLf + "AND I.REFNO IN"
                StrSql += vbCrLf + "(SELECT SNO FROM " & cnStockDb & "..ISSUE WHERE "
                If ChkAsOn.Checked Then
                    StrSql += vbCrLf + "TRANDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
                Else
                    StrSql += vbCrLf + "TRANDATE BETWEEN  '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpToDate.Value.ToString("yyyy-MM-dd") & "'"
                End If
                StrSql += vbCrLf + "AND ISNULL(CANCEL,'') = '' AND ISNULL(TRANTYPE,'') = 'IHM' "
                StrSql += vbCrLf + "AND LEN(TRANTYPE)>2"
                If cnCostId <> "" Then StrSql += vbCrLf + "AND COSTID='" & cnCostId & "'"
                StrSql += vbCrLf + ")"
                StrSql += vbCrLf + "GROUP BY I.REFNO "
                StrSql += vbCrLf + ")X GROUP BY SNO HAVING (SUM(PCS)>0 OR SUM(GRSWT)>0)"
                StrSql += vbCrLf + "ORDER BY TRANDATE,TRANNO"
            End If
        Else
            If rbtIssue.Checked Then
                StrSql = vbCrLf + "  SELECT I.TRANNO,I.TRANDATE"
                StrSql += vbCrLf + "  ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = I.ACCODE)ACNAME"
                StrSql += vbCrLf + "  ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID) AS ITEM"
                StrSql += vbCrLf + "  ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = I.ITEMID AND SUBITEMID = I.SUBITEMID) AS SUBITEM"
                StrSql += vbCrLf + "  ,SUM(I.PCS - ISNULL(L.PCS,0))PCS"
                StrSql += vbCrLf + "  ,SUM(I.GRSWT - ISNULL(L.GRSWT,0))GRSWT"
                StrSql += vbCrLf + "  ,SUM(I.NETWT - ISNULL(L.NETWT,0))NETWT"
                StrSql += vbCrLf + "  ,I.TOUCH"
                StrSql += vbCrLf + "  ,I.PUREWT PUREWT,I.WASTAGE,I.MCHARGE"
                StrSql += vbCrLf + "  ,I.SNO,I.ITEMTYPEID,I.ACCODE,I.METALID"
                StrSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT I"
                StrSql += vbCrLf + "  LEFT OUTER JOIN " & cnStockDb & "..LOTISSUE AS L ON L.TRANNO = I.TRANNO AND L.TRANDATE = I.TRANDATE AND L.RECSNO = I.SNO"
                StrSql += vbCrLf + "  INNER JOIN " & cnStockDb & "..ISSUE AS ISS ON ISNULL(ISS.REFNO,'')<>I.SNO AND ISS.SNO=I.SNO AND ISS.REFNO=I.REFNO"
                If ChkAsOn.Checked Then
                    StrSql += vbCrLf + "  WHERE I.TRANDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
                Else
                    StrSql += vbCrLf + "  WHERE I.TRANDATE BETWEEN '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpToDate.Value.ToString("yyyy-MM-dd") & "'"
                End If
                StrSql += vbCrLf + "  AND ISNULL(I.CANCEL,'') = '' AND ISNULL(L.CANCEL,'') = ''"
                StrSql += vbCrLf + "  AND I.GRSWT <> 0 AND LEN(I.TRANTYPE) > 2 "
                StrSql += vbCrLf + "  AND I.COSTID='" + cnCostId + "'"
                StrSql += vbCrLf + "  AND ISNULL(I.ITEMID,0)<>0 "
                'StrSql += vbCrLf + "  AND ISNULL(I.SUBITEMID,0)<>0"
                StrSql += vbCrLf + "  GROUP BY I.TRANNO,I.TRANDATE,I.ACCODE,I.ITEMID,I.SUBITEMID,I.TOUCH,I.PUREWT,I.SNO,I.ITEMTYPEID,I.WASTAGE,I.MCHARGE,I.METALID"
                'StrSql += vbCrLf + "  HAVING "
                'StrSql += vbCrLf + "  (SUM(I.PCS - ISNULL(L.PCS,0)) > 0"
                'StrSql += vbCrLf + "  OR SUM(I.GRSWT - ISNULL(L.GRSWT,0)) > 0)"
                StrSql += vbCrLf + "  ORDER BY I.TRANDATE,I.TRANNO"
            ElseIf rdbReceipt.Checked Then
                StrSql = vbCrLf + "  SELECT I.TRANNO,I.TRANDATE"
                StrSql += vbCrLf + "  ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = I.ACCODE)ACNAME"
                StrSql += vbCrLf + "  ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID)AS ITEM"
                StrSql += vbCrLf + "  ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = I.ITEMID AND SUBITEMID = I.SUBITEMID)AS SUBITEM"
                StrSql += vbCrLf + "  ,SUM(I.PCS - ISNULL(L.PCS,0))PCS"
                StrSql += vbCrLf + "  ,SUM(I.GRSWT - ISNULL(L.GRSWT,0))GRSWT"
                StrSql += vbCrLf + "  ,SUM(I.NETWT - ISNULL(L.NETWT,0))NETWT"
                StrSql += vbCrLf + "  ,I.TOUCH"
                StrSql += vbCrLf + "  ,I.PUREWT PUREWT,I.WASTAGE,I.MCHARGE"
                StrSql += vbCrLf + "  ,I.SNO,I.ITEMTYPEID,I.ACCODE,I.METALID"
                StrSql += vbCrLf + "  FROM " & cnStockDb & "..ISSUE I"
                StrSql += vbCrLf + "  LEFT OUTER JOIN " & cnStockDb & "..LOTISSUE AS L ON L.TRANNO = I.TRANNO AND L.TRANDATE = I.TRANDATE AND L.RECSNO = I.SNO"
                StrSql += vbCrLf + "  INNER JOIN " & cnStockDb & "..RECEIPT AS ISS ON ISNULL(ISS.REFNO,'')<>I.SNO AND ISS.SNO=I.SNO AND ISS.REFNO=I.REFNO"
                If ChkAsOn.Checked Then
                    StrSql += vbCrLf + "  WHERE I.TRANDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
                Else
                    StrSql += vbCrLf + "  WHERE I.TRANDATE BETWEEN '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpToDate.Value.ToString("yyyy-MM-dd") & "'"
                End If
                StrSql += vbCrLf + "  AND ISNULL(I.CANCEL,'') = '' AND ISNULL(L.CANCEL,'') = ''"
                StrSql += vbCrLf + "  AND I.GRSWT <> 0 AND LEN(I.TRANTYPE) > 2 "
                StrSql += vbCrLf + "  AND I.COSTID='" + cnCostId + "'"
                StrSql += vbCrLf + "  AND ISNULL(I.ITEMID,0)<>0 "
                'StrSql += vbCrLf + "  AND ISNULL(I.SUBITEMID,0)<>0"
                StrSql += vbCrLf + "  GROUP BY I.TRANNO,I.TRANDATE,I.ACCODE,I.ITEMID,I.SUBITEMID,I.TOUCH,I.PUREWT,I.SNO,I.ITEMTYPEID,I.WASTAGE,I.MCHARGE,I.METALID"
                'StrSql += vbCrLf + "  HAVING "
                'StrSql += vbCrLf + "  (SUM(I.PCS - ISNULL(L.PCS,0)) > 0"
                'StrSql += vbCrLf + "  OR SUM(I.GRSWT - ISNULL(L.GRSWT,0)) > 0)"
                StrSql += vbCrLf + "  ORDER BY I.TRANDATE,I.TRANNO"
            End If
        End If
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dtGrid)
        If Not dtGrid.Rows.Count > 0 Then
            lblHelp.Text = ""
            MsgBox("Record not found", MsgBoxStyle.Information)
            btnNew.Focus()
            Exit Sub
        End If
        If rbtIssue.Checked Then lblHelp.Text = "Select an Item to HM Issue" Else lblHelp.Text = "Select an Item to HM Receipt"
        gridView.DataSource = dtGrid
        BrighttechPack.GlobalMethods.FormatGridColumns(gridView)

        gridView.Columns("TRANNO").Width = 60
        gridView.Columns("TRANDATE").Width = 80
        gridView.Columns("ACNAME").Width = 150
        gridView.Columns("ITEM").Width = 120
        gridView.Columns("SUBITEM").Width = 120
        gridView.Columns("PCS").Width = 70
        gridView.Columns("GRSWT").Width = 80
        gridView.Columns("NETWT").Width = 80
        gridView.Columns("TOUCH").Width = 80
        gridView.Columns("PUREWT").Width = 80
        gridView.Columns("CHECK").Width = 50
        gridView.Columns("CHECK").ReadOnly = False
        gridView.Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
        gridView.Columns("GRSWT").DefaultCellStyle.Format = "0.000"
        gridView.Columns("NETWT").DefaultCellStyle.Format = "0.000"
        gridView.Columns("PUREWT").DefaultCellStyle.Format = "0.000"
        gridView.Columns("TOUCH").DefaultCellStyle.Format = "0.00"
        gridView.Columns("SNO").Visible = False
        gridView.Columns("ITEMTYPEID").Visible = False
        gridView.Columns("PUREWT").Visible = False
        gridView.Columns("TOUCH").Visible = False
        gridView.Columns("ACCODE").Visible = False
        If gridView.Columns.Contains("METALID") Then gridView.Columns("METALID").Visible = False
        If gridView.Columns.Contains("CATCODE") Then gridView.Columns("CATCODE").Visible = False
        If gridView.Columns.Contains("OCATCODE") Then gridView.Columns("OCATCODE").Visible = False
        gridView.Focus()
    End Sub

    Private Sub btnIssue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIssue.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
        If Not CheckDate(dtpTrandate.Value) Then Exit Sub
        If CheckEntryDate(dtpTrandate.Value) Then Exit Sub
        Dim dt As New DataTable
        dt = CType(gridView.DataSource, DataTable).Copy
        Dim ros() As DataRow = Nothing
        ros = dt.Select("CHECK = TRUE")
        If Not ros.Length > 0 Then
            MsgBox("There is No Row Selected", MsgBoxStyle.Information)
            Exit Sub
        End If
        Dim Tran As OleDbTransaction = Nothing
        Dim DesignerId As String = Nothing
        Dim COSTID As String = Nothing
        Dim itemId As Integer = Nothing
        Dim stockType As String = Nothing
        Dim subItemId As Integer = Nothing
        Dim RoIssue As DataRow() = Nothing
        Dim noOfTag As Integer = Nothing
        Dim itemCounterId As Integer = Nothing
        Dim entryType As String = Nothing
        Dim dtSelected As New DataTable
        Dim pcs As Integer
        Dim grswt As Decimal
        Dim netwt As Decimal
        Dim purewt As Decimal
        Dim issSno As String = Nothing
        Dim mcharge As Decimal
        Dim wast As Decimal
        Dim itemtypeid As Integer
        Dim accode As String
        Dim touch As String
        Dim tranno As Integer
        Dim trandate As String
        Dim sno As String
        Dim BatchNo As String
        Dim Catcode, OCatcode, MetalId, Acname As String
        Dim Part As Boolean = False
        StrSql = " SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbDesigner_MAN.Text & "'"
        accode = objGPack.GetSqlValue(StrSql)
        If accode = "" Then MsgBox("Dealer Accode Not found", MsgBoxStyle.Information) : cmbDesigner_MAN.Focus() : Exit Sub
        If MessageBox.Show("Do you Want to Issue Part Weight", "Partly Issue", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
            If ros.Length > 1 Then
                MsgBox("Select Single Entry to Part Issue", MsgBoxStyle.Information)
                Exit Sub
            End If
            For Each row As DataRow In ros
                StrSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & row!ITEM.ToString & "'"
                itemId = Val(objGPack.GetSqlValue(StrSql, , , Tran))
                StrSql = " SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & row!SUBITEM.ToString & "'"
                subItemId = Val(objGPack.GetSqlValue(StrSql, , , Tran))
                pcs = Val(row!PCS)
                grswt = Val(row!GRSWT)
                netwt = Val(row!NETWT)
            Next
            Dim objHallmarkIssue As New frmHallmarkIssueReceiptDia(itemId, subItemId, pcs, grswt, netwt)
            If objHallmarkIssue.ShowDialog <> Windows.Forms.DialogResult.OK Then
                Exit Sub
            Else
                pcs = Val(objHallmarkIssue.txtPieces_Num_Man.Text)
                grswt = Val(objHallmarkIssue.txtGrossWt_Wet.Text)
                netwt = Val(objHallmarkIssue.txtNetWt_Wet.Text)
                itemId = Val(objHallmarkIssue.txtItemCode_Num_Man.Text)
                StrSql = " SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & objHallmarkIssue.cmbSubItemName_Man.Text & "'"
                subItemId = Val(objGPack.GetSqlValue(StrSql, , , Tran))
                Part = True
            End If
        End If
        Try
            Tran = cn.BeginTransaction()
            StrSql = " IF (SELECT COUNT(*) FROM " & cnAdminDb & "..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "BILLNO')> 0"
            StrSql += " DROP TABLE " & cnAdminDb & "..TEMP" & systemId & "BILLNO"
            Cmd = New OleDbCommand(StrSql, cn, Tran)
            Cmd.ExecuteNonQuery()
            tranno = GetBillNoValue("GEN-HM-ISS", Tran)
            BatchNo = GetNewBatchno(cnCostId, GetServerDate(Tran), Tran)
            For Each row As DataRow In ros
                If Part = False Then
                    StrSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & row!ITEM.ToString & "'"
                    itemId = Val(objGPack.GetSqlValue(StrSql, , , Tran))
                    StrSql = " SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & row!SUBITEM.ToString & "'"
                    subItemId = Val(objGPack.GetSqlValue(StrSql, , , Tran))
                    pcs = Val(row!PCS)
                    grswt = Val(row!GRSWT)
                    netwt = Val(row!NETWT)
                    purewt = Val(row!PUREWT)
                End If
                issSno = GetNewSno(IIf(_AccAudit, TranSnoType.TISSUECODE, TranSnoType.ISSUECODE), Tran)
                StrSql = " INSERT INTO " & cnStockDb & "..ISSUE"
                StrSql += " ("
                StrSql += "  SNO,TRANNO,TRANDATE,TRANTYPE,PCS"
                StrSql += " ,GRSWT,NETWT,LESSWT,PUREWT"
                StrSql += " ,TAGNO,ITEMID,SUBITEMID,WASTPER"
                StrSql += " ,WASTAGE,MCGRM,MCHARGE,AMOUNT"
                StrSql += " ,RATE,BOARDRATE,SALEMODE,GRSNET"
                StrSql += " ,TRANSTATUS,REFNO,REFDATE,COSTID"
                StrSql += " ,COMPANYID,FLAG,EMPID,TAGGRSWT"
                StrSql += " ,TAGNETWT,TAGRATEID,TAGSVALUE,TAGDESIGNER"
                StrSql += " ,ITEMCTRID,ITEMTYPEID,PURITY,TABLECODE"
                StrSql += " ,INCENTIVE,WEIGHTUNIT,CATCODE,OCATCODE"
                StrSql += " ,ACCODE,ALLOY,BATCHNO,REMARK1"
                StrSql += " ,REMARK2,USERID,UPDATED,UPTIME,SYSTEMID,DISCOUNT,RUNNO,CASHID,TAX,TDS"
                StrSql += " ,STNAMT,MISCAMT,METALID,STONEUNIT,APPVER,TOUCH,ORDSTATE_ID"
                If STOCKVALIDATION Then StrSql += " ,RESNO"
                StrSql += " )"
                StrSql += " VALUES("
                StrSql += " '" & issSno & "'" ''SNO
                StrSql += " ," & tranno & "" 'TRANNO
                StrSql += " ,'" & Format(dtpTrandate.Value, "yyyy-MM-dd") & "'" 'TRANDATE
                StrSql += " ,'IHM'" 'TRANTYPE
                StrSql += " ," & Val(pcs) & "" 'PCS
                StrSql += " ," & Val(grswt) & "" 'GRSWT
                StrSql += " ," & Val(netwt) & "" 'NETWT
                StrSql += " ,0"  'LESSWT
                StrSql += " ," & Val(purewt) & "" 'PUREWT
                StrSql += " ,''" 'TAGNO
                StrSql += " ," & Val(itemId) 'ITEMID
                StrSql += " ," & Val(subItemId) 'SUBITEMID
                StrSql += " ,0" 'WASTPER
                StrSql += " ,0" 'WASTAGE
                StrSql += " ,0" 'MCGRM
                StrSql += " ,0" 'MCHARGE
                StrSql += " ,0" 'AMOUNT
                StrSql += " ,0" 'RATE
                StrSql += " ,0" 'BOARDRATE
                StrSql += " ,''" 'SALEMODE
                StrSql += " ,0"  'GRSNET
                StrSql += " ,''" 'TRANSTATUS ''
                StrSql += " ,'" & row!SNO & "'" 'REFNO ''
                StrSql += " ,''"  'REFDATE NULL
                StrSql += " ,'" & cnCostId & "'" 'COSTID 
                StrSql += " ,'" & strCompanyId & "'" 'COMPANYID
                StrSql += " ,''"  'FLAG
                StrSql += " ,0" 'EMPID
                StrSql += " ,0" 'TAGGRSWT
                StrSql += " ,0" 'TAGNETWT
                StrSql += " ,0" 'TAGRATEID
                StrSql += " ,0" 'TAGSVALUE
                StrSql += " ,''" 'TAGDESIGNER  
                StrSql += " ,0" 'ITEMCTRID
                StrSql += " ," & Val(row!ITEMTYPEID) & "" 'ITEMTYPEID
                StrSql += " ,0"  'PURITY
                StrSql += " ,''" 'TABLECODE
                StrSql += " ,''" 'INCENTIVE
                StrSql += " ,''"  'WEIGHTUNIT
                StrSql += " ,'" & row!CATCODE & "'"  'CATCODE
                StrSql += " ,'" & row!OCATCODE & "'"  'OCATCODE
                StrSql += " ,'" & accode & "'" 'ACCODE
                StrSql += " ,0" 'ALLOY
                StrSql += " ,'" & BatchNo & "'"  'BATCHNO
                StrSql += " ,''"  'REMARK1
                StrSql += " ,''"  'REMARK2
                StrSql += " ," & userId  'USERID
                StrSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                StrSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                StrSql += " ,'" & systemId & "'" 'SYSTEMID
                StrSql += " ,0" 'DISCOUNT
                StrSql += " ,''" 'RUNNO
                StrSql += " ,''" 'CASHID
                StrSql += " ,0" 'TAX
                StrSql += " ,0" 'TDS
                StrSql += " ,0" 'STNAMT
                StrSql += " ,0" 'MISCAMT
                StrSql += " ,'" & row!METALID & "'" 'METALID
                StrSql += " ,''" 'STONEUNIT
                StrSql += " ,'" & VERSION & "'" 'APPVER
                StrSql += " ,'" & row!TOUCH & "'" 'APPVER
                StrSql += " ,0" 'ORDSTATE_ID
                If STOCKVALIDATION Then StrSql += " ,'" & row!SNO & "'" 'RESNO ''
                StrSql += " )"
                ExecQuery(SyncMode.Transaction, StrSql, cn, Tran, COSTID)
            Next
            Tran.Commit()
            Tran = Nothing
            MsgBox("Hallmark Issue No:" + tranno.ToString + " Generated...", MsgBoxStyle.Information)
            If IO.File.Exists(Application.StartupPath & "\BillPrint.exe") Then
                Dim write As IO.StreamWriter
                Dim prnmemsuffix As String = ""
                If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId
                Dim memfile As String = "\BillPrint" + prnmemsuffix.Trim & ".mem"
                write = IO.File.CreateText(Application.StartupPath & memfile)
                write.WriteLine(LSet("TYPE", 15) & ":HMI")
                write.WriteLine(LSet("BATCHNO", 15) & ":" & BatchNo)
                write.WriteLine(LSet("TRANDATE", 15) & ":" & Format(dtpTrandate.Value, "yyyy-MM-dd"))
                write.WriteLine(LSet("DUPLICATE", 15) & ":N")
                write.Flush()
                write.Close()
                If EXE_WITH_PARAM = False Then
                    System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe")
                Else
                    System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe", _
                    LSet("TYPE", 15) & ":HMI" & ";" & _
                    LSet("BATCHNO", 15) & ":" & BatchNo & ";" & _
                    LSet("TRANDATE", 15) & ":" & Format(dtpTrandate.Value, "yyyy-MM-dd") & ";" & _
                    LSet("DUPLICATE", 15) & ":N")
                End If
            Else
                MsgBox("Billprint exe not found", MsgBoxStyle.Information)
            End If
        Catch ex As Exception
            If Not Tran Is Nothing Then Tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        Finally
            If Not Tran Is Nothing Then Tran.Dispose()
        End Try
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub gridView_CellClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.CellClick
        Dim dt As New DataTable
        dt = CType(gridView.DataSource, DataTable)
        dt.AcceptChanges()
        If rbtIssue.Checked Then
            btnIssue.Enabled = True
            btnReceipt.Enabled = False
        ElseIf rdbReceipt.Checked Then
            btnIssue.Enabled = False
            btnReceipt.Enabled = True
        End If
    End Sub

    Private Sub btnReceipt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReceipt.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
        If Not CheckDate(dtpTrandate.Value) Then Exit Sub
        If CheckEntryDate(dtpTrandate.Value) Then Exit Sub
        Dim dt As New DataTable
        dt = CType(gridView.DataSource, DataTable).Copy
        Dim ros() As DataRow = Nothing
        ros = dt.Select("CHECK = TRUE")
        If Not ros.Length > 0 Then
            MsgBox("There is No Row Selected", MsgBoxStyle.Information)
            Exit Sub
        End If
        Dim Tran As OleDbTransaction = Nothing
        Dim DesignerId As String = Nothing
        Dim COSTID As String = Nothing
        Dim itemId As Integer = Nothing
        Dim stockType As String = Nothing
        Dim subItemId As Integer = Nothing
        Dim RoIssue As DataRow() = Nothing
        Dim noOfTag As Integer = Nothing
        Dim itemCounterId As Integer = Nothing
        Dim entryType As String = Nothing
        Dim dtSelected As New DataTable
        Dim pcs As Integer
        Dim grswt As Decimal
        Dim netwt As Decimal
        Dim purewt As Decimal
        Dim issSno As String = Nothing
        Dim mcharge As Decimal
        Dim wast As Decimal
        Dim itemtypeid As Integer
        Dim accode, Acname As String
        Dim touch As String
        Dim tranno As Integer
        Dim sno As String
        Dim BatchNo As String
        Dim Catcode, OCatcode, MetalId As String
        COSTID = cnCostId
        Try
            Tran = cn.BeginTransaction()
            StrSql = " IF (SELECT COUNT(*) FROM " & cnAdminDb & "..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "BILLNO')> 0"
            StrSql += " DROP TABLE " & cnAdminDb & "..TEMP" & systemId & "BILLNO"
            Cmd = New OleDbCommand(StrSql, cn, Tran)
            Cmd.ExecuteNonQuery()
            tranno = GetBillNoValue("GEN-HM-REC", Tran)
            BatchNo = GetNewBatchno(cnCostId, GetServerDate(Tran), Tran)
            For Each row As DataRow In ros
                StrSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & row!ITEM.ToString & "'"
                itemId = Val(objGPack.GetSqlValue(StrSql, , , Tran))
                StrSql = " SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & row!SUBITEM.ToString & "'"
                subItemId = Val(objGPack.GetSqlValue(StrSql, , , Tran))
                issSno = GetNewSno(IIf(_AccAudit, TranSnoType.TRECEIPTCODE, TranSnoType.RECEIPTCODE), Tran)
                StrSql = " INSERT INTO " & cnStockDb & "..RECEIPT"
                StrSql += " ("
                StrSql += "  SNO,TRANNO,TRANDATE,TRANTYPE,PCS"
                StrSql += " ,GRSWT,NETWT,LESSWT,PUREWT"
                StrSql += " ,TAGNO,ITEMID,SUBITEMID,WASTPER"
                StrSql += " ,WASTAGE,MCGRM,MCHARGE,AMOUNT"
                StrSql += " ,RATE,BOARDRATE,SALEMODE,GRSNET"
                StrSql += " ,TRANSTATUS,REFNO,REFDATE,COSTID"
                StrSql += " ,COMPANYID,FLAG,EMPID,TAGGRSWT"
                StrSql += " ,TAGNETWT,TAGRATEID,TAGSVALUE,TAGDESIGNER"
                StrSql += " ,ITEMCTRID,ITEMTYPEID,PURITY,TABLECODE"
                StrSql += " ,INCENTIVE,WEIGHTUNIT,CATCODE,OCATCODE"
                StrSql += " ,ACCODE,ALLOY,BATCHNO,REMARK1"
                StrSql += " ,REMARK2,USERID,UPDATED,UPTIME,SYSTEMID,DISCOUNT,RUNNO,CASHID,TAX,TDS"
                StrSql += " ,STNAMT,MISCAMT,METALID,STONEUNIT,APPVER,TOUCH,ORDSTATE_ID"
                StrSql += " )"
                StrSql += " VALUES("
                StrSql += " '" & issSno & "'" ''SNO
                StrSql += " ," & tranno & "" 'TRANNO
                StrSql += " ,'" & Format(dtpTrandate.Value, "yyyy-MM-dd") & "'" 'TRANDATE
                StrSql += " ,'RHM'" 'TRANTYPE
                StrSql += " ," & Val(row!PCS.ToString) & "" 'PCS
                StrSql += " ," & Val(row!GRSWT.ToString) & "" 'GRSWT
                StrSql += " ," & Val(row!NETWT.ToString) & "" 'NETWT
                StrSql += " ,0"  'LESSWT
                StrSql += " ," & Val(row!PUREWT.ToString) & "" 'PUREWT
                StrSql += " ,''" 'TAGNO
                StrSql += " ," & Val(itemId) 'ITEMID
                StrSql += " ," & Val(subItemId) 'SUBITEMID
                StrSql += " ,0" 'WASTPER
                StrSql += " ,0" 'WASTAGE
                StrSql += " ,0" 'MCGRM
                StrSql += " ,0" 'MCHARGE
                StrSql += " ,0"  'AMOUNT
                StrSql += " ,0"  'RATE
                StrSql += " ,0" 'BOARDRATE
                StrSql += " ,''" 'SALEMODE
                StrSql += " ,0"  'GRSNET
                StrSql += " ,''" 'TRANSTATUS ''
                StrSql += " ,'" & row!SNO.ToString & "'" 'REFNO ''
                StrSql += " ,''"  'REFDATE NULL
                StrSql += " ,'" & cnCostId & "'" 'COSTID 
                StrSql += " ,'" & strCompanyId & "'" 'COMPANYID
                StrSql += " ,''"  'FLAG
                StrSql += " ,0" 'EMPID
                StrSql += " ,0" 'TAGGRSWT
                StrSql += " ,0" 'TAGNETWT
                StrSql += " ,0" 'TAGRATEID
                StrSql += " ,0" 'TAGSVALUE
                StrSql += " ,''" 'TAGDESIGNER  
                StrSql += " ,0" 'ITEMCTRID
                StrSql += " ," & Val(row!ITEMTYPEID.ToString) & "" 'ITEMTYPEID
                StrSql += " ,0"  'PURITY
                StrSql += " ,''" 'TABLECODE
                StrSql += " ,''" 'INCENTIVE
                StrSql += " ,''"  'WEIGHTUNIT
                StrSql += " ,'" & row!CATCODE.ToString & "'"  'CATCODE
                StrSql += " ,'" & row!OCATCODE.ToString & "'"  'OCATCODE
                StrSql += " ,'" & row!ACCODE.ToString & "'" 'ACCODE
                StrSql += " ,0" 'ALLOY
                StrSql += " ,'" & BatchNo & "'"  'BATCHNO
                StrSql += " ,''"  'REMARK1
                StrSql += " ,''"  'REMARK2
                StrSql += " ," & userId  'USERID
                StrSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                StrSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                StrSql += " ,'" & systemId & "'" 'SYSTEMID
                StrSql += " ,0" 'DISCOUNT
                StrSql += " ,''" 'RUNNO
                StrSql += " ,''" 'CASHID
                StrSql += " ,0" 'TAX
                StrSql += " ,0" 'TDS
                StrSql += " ,0" 'STNAMT
                StrSql += " ,0" 'MISCAMT
                StrSql += " ,'" & row!METALID.ToString & "'" 'METALID
                StrSql += " ,''" 'STONEUNIT
                StrSql += " ,'" & VERSION & "'" 'APPVER
                StrSql += " ,'" & Val(row!TOUCH.ToString) & "'" 'APPVER
                StrSql += " ,0" 'ORDSTATE_ID
                StrSql += " )"
                ExecQuery(SyncMode.Transaction, StrSql, cn, Tran, COSTID)
            Next
            Tran.Commit()
            Tran = Nothing
            MsgBox("Hallmark Receipt No:" + tranno.ToString + " Generated...", MsgBoxStyle.Information)
            If IO.File.Exists(Application.StartupPath & "\BillPrint.exe") Then
                Dim write As IO.StreamWriter
                Dim prnmemsuffix As String = ""
                If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId
                Dim memfile As String = "\BillPrint" + prnmemsuffix.Trim & ".mem"
                write = IO.File.CreateText(Application.StartupPath & memfile)
                write.WriteLine(LSet("TYPE", 15) & ":HMR")
                write.WriteLine(LSet("BATCHNO", 15) & ":" & BatchNo)
                write.WriteLine(LSet("TRANDATE", 15) & ":" & Format(dtpTrandate.Value, "yyyy-MM-dd"))
                write.WriteLine(LSet("DUPLICATE", 15) & ":N")
                write.Flush()
                write.Close()
                If EXE_WITH_PARAM = False Then
                    System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe")
                Else
                    System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe", _
                    LSet("TYPE", 15) & ":HMR" & ";" & _
                    LSet("BATCHNO", 15) & ":" & BatchNo & ";" & _
                    LSet("TRANDATE", 15) & ":" & Format(dtpTrandate.Value, "yyyy-MM-dd") & ";" & _
                    LSet("DUPLICATE", 15) & ":N")
                End If
            Else
                MsgBox("Billprint exe not found", MsgBoxStyle.Information)
            End If

        Catch ex As Exception
            If Not Tran Is Nothing Then Tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        Finally
            If Not Tran Is Nothing Then Tran.Dispose()
        End Try
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub chkSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSelectAll.CheckedChanged
        If gridView.RowCount = 0 Then Exit Sub
        For cnt As Integer = 0 To gridView.RowCount - 1
            gridView.Rows(cnt).Cells("CHECK").Value = chkSelectAll.Checked
        Next
        If rbtIssue.Checked Then
            btnIssue.Enabled = True
            btnReceipt.Enabled = False
        ElseIf rdbReceipt.Checked Then
            btnIssue.Enabled = False
            btnReceipt.Enabled = True
        End If
        gridView.Select()
    End Sub

    Private Sub ChkAsOn_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkAsOn.CheckedChanged
        If ChkAsOn.Checked Then
            lblToDate.Visible = False
            dtpToDate.Visible = False
            ChkAsOn.Text = "AsOnDate"
        Else
            lblToDate.Visible = True
            dtpToDate.Visible = True
            ChkAsOn.Text = "FromDate"
        End If
    End Sub
    Private Sub Prop_Sets()
        Dim obj As New frmHallMarkMaterialIssueReceipt_Properties
        obj.p_chkAsOn = ChkAsOn.Checked
        obj.p_rbtIssue = rbtIssue.Checked
        obj.p_rbtReceipt = rdbReceipt.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmHallMarkMaterialIssueReceipt_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmHallMarkMaterialIssueReceipt_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmHallMarkMaterialIssueReceipt_Properties))
        ChkAsOn.Checked = obj.p_chkAsOn
        rbtIssue.Checked = obj.p_rbtIssue
        rdbReceipt.Checked = obj.p_rbtReceipt
    End Sub

    Private Sub rdbReceipt_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdbReceipt.CheckedChanged
        If rdbReceipt.Checked Then
            cmbDesigner_MAN.Enabled = False
        Else
            cmbDesigner_MAN.Enabled = True
        End If
    End Sub
End Class
Public Class frmHallMarkMaterialIssueReceipt_Properties
    Private chkAsOn As Boolean = True
    Public Property p_chkAsOn() As Boolean
        Get
            Return chkAsOn
        End Get
        Set(ByVal value As Boolean)
            chkAsOn = value
        End Set
    End Property
    Private rbtReceipt As Boolean = True
    Public Property p_rbtReceipt() As Boolean
        Get
            Return rbtReceipt
        End Get
        Set(ByVal value As Boolean)
            rbtReceipt = value
        End Set
    End Property
    Private rbtIssue As Boolean = True
    Public Property p_rbtIssue() As Boolean
        Get
            Return rbtIssue
        End Get
        Set(ByVal value As Boolean)
            rbtIssue = value
        End Set
    End Property
End Class