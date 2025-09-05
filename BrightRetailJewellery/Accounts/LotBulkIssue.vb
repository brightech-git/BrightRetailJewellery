Imports System.Data.OleDb
Public Class LotBulkIssue
    Dim Da As OleDbDataAdapter
    Dim Cmd As OleDbCommand
    Dim dt As New DataTable
    Dim StrSql As String
    Dim HideAccLink As Boolean = IIf(GetAdmindbSoftValue("HIDE_ACHARI_ACCLINK", "Y") = "Y", True, False)
    Dim STOCKVALIDATION As Boolean = IIf(GetAdmindbSoftValue("MRMISTOCKLOCK", "N") = "Y", True, False)    
    Dim PrintBulkLotIss As Boolean = IIf(GetAdmindbSoftValue("PRINT_LOT", "N") = "Y", True, False)

    Private Sub LotBulkIssue_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub LotBulkIssue_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        btnNew_Click(Me, New EventArgs)
        gridView.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        dtpAsOnDate.Value = BrighttechPack.GlobalMethods.GetServerDate(cn)
        loadCost()
        gridView.DataSource = Nothing
        chkSelectAll.Checked = False
        btnLotIssue.Enabled = False
        dtpAsOnDate.Select()
    End Sub
    Private Function loadCost()
        StrSql = " SELECT 'ALL' COSTNAME,'ALL' COSTNID,1 RESULT"
        StrSql += vbCrLf + " UNION ALL"
        StrSql += vbCrLf + " SELECT COSTNAME,COSTID,2 RESULT FROM " & cnAdminDb & "..COSTCENTRE WHERE ISNULL(ACTIVE,'')<>'N'"
        StrSql += vbCrLf + " ORDER BY RESULT,COSTNAME"
        dt = New DataTable
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dt)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostName, dt, "COSTNAME", , "ALL")
        chkCmbCostName.Text = cnCostName
    End Function

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        gridView.DataSource = Nothing
        Dim CostId As String = GetQryStringForSp(chkCmbCostName.Text, cnAdminDb & "..COSTCENTRE", "COSTID", "COSTNAME", False)
        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("CHECK", GetType(Boolean))
        dtGrid.Columns("CHECK").DefaultValue = chkSelectAll.Checked
        If STOCKVALIDATION Then
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
            StrSql += vbCrLf + ",SUM(WASTAGE)WASTAGE,SUM(MCHARGE)MCHARGE"
            StrSql += vbCrLf + ",(SELECT TOP 1 COSTNAME FROM  " & cnAdminDb & "..COSTCENTRE WHERE COSTID ="
            StrSql += vbCrLf + "(SELECT TOP 1 COSTID FROM " & cnStockDb & "..RECEIPT WHERE SNO = X.SNO))COSTNAME"
            StrSql += vbCrLf + ",(SELECT TOUCH FROM " & cnStockDb & "..RECEIPT WHERE SNO=X.SNO)TOUCH"
            StrSql += vbCrLf + ",(SELECT ITEMTYPEID FROM " & cnStockDb & "..RECEIPT WHERE SNO=X.SNO)ITEMTYPEID"
            StrSql += vbCrLf + ",(SELECT ACCODE FROM " & cnStockDb & "..RECEIPT WHERE SNO=X.SNO)ACCODE"
            StrSql += vbCrLf + ",SNO,STKTYPE"
            StrSql += vbCrLf + "FROM ("
            StrSql += vbCrLf + "SELECT "
            StrSql += vbCrLf + "SUM(R.PCS)PCS"
            StrSql += vbCrLf + ",SUM(R.GRSWT)GRSWT"
            StrSql += vbCrLf + ",SUM(R.NETWT)NETWT"
            StrSql += vbCrLf + ",SUM(R.PUREWT)PUREWT"
            StrSql += vbCrLf + ",SUM(R.WASTAGE)WASTAGE"
            StrSql += vbCrLf + ",SUM(R.MCHARGE)MCHARGE,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE "
            StrSql += vbCrLf + " COSTID IN(SELECT COSTID FROM " & cnStockDb & "..RECEIPT WHERE SNO=R.SNO ))COSTNAME"
            StrSql += vbCrLf + ",R.SNO,ISNULL(R.STKTYPE,'') STKTYPE "
            StrSql += vbCrLf + "FROM " & cnStockDb & "..RECEIPT R"
            If chkAsonDate.Checked = False Then
                StrSql += vbCrLf + "  WHERE R.TRANDATE BETWEEN '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpToDate.Value.ToString("yyyy-MM-dd") & "'"
            Else
                StrSql += vbCrLf + "  WHERE R.TRANDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
            End If
            If chkCmbCostName.Text <> "ALL" And chkCmbCostName.Text <> "" Then StrSql += vbCrLf + " AND R.COSTID IN('" & CostId.Replace(",", "','") & "') "
            StrSql += vbCrLf + "AND ISNULL(R.CANCEL,'') = '' "
            StrSql += vbCrLf + "AND TRANTYPE<>'PU' "
            StrSql += vbCrLf + "AND ISNULL(R.ITEMID,0)<>0 "
            StrSql += vbCrLf + "GROUP BY R.SNO,R.COSTID,R.STKTYPE "
            StrSql += vbCrLf + "UNION ALL"
            StrSql += vbCrLf + "SELECT "
            StrSql += vbCrLf + "-1*SUM(I.PCS)PCS"
            StrSql += vbCrLf + ",-1*SUM(I.GRSWT)GRSWT"
            StrSql += vbCrLf + ",-1*SUM(I.NETWT)NETWT"
            StrSql += vbCrLf + ",-1*SUM(I.PUREWT)PUREWT"
            StrSql += vbCrLf + ",-1*SUM(I.WASTAGE)WASTAGE"
            StrSql += vbCrLf + ",-1*SUM(I.MCHARGE)MCHARGE,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE "
            StrSql += vbCrLf + " COSTID IN(SELECT COSTID FROM " & cnStockDb & "..RECEIPT WHERE SNO=I.RESNO ))COSTNAME"
            StrSql += vbCrLf + ",I.RESNO,ISNULL(I.STKTYPE,'') STKTYPE "
            StrSql += vbCrLf + "FROM " & cnStockDb & "..ISSUE I"
            StrSql += vbCrLf + "WHERE "
            StrSql += vbCrLf + " ISNULL(I.CANCEL,'') = '' "
            StrSql += vbCrLf + "AND I.GRSWT <> 0 AND LEN(I.TRANTYPE) > 2 "
            StrSql += vbCrLf + "AND ISNULL(I.ITEMID,0)<>0 "
            StrSql += vbCrLf + "AND I.RESNO IN"
            StrSql += vbCrLf + "(SELECT SNO FROM " & cnStockDb & "..RECEIPT    "
            If chkAsonDate.Checked = False Then
                StrSql += vbCrLf + "  WHERE TRANDATE BETWEEN '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpToDate.Value.ToString("yyyy-MM-dd") & "'"
            Else
                StrSql += vbCrLf + "  WHERE TRANDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
            End If
            If chkCmbCostName.Text <> "ALL" And chkCmbCostName.Text <> "" Then StrSql += vbCrLf + " AND COSTID IN('" & CostId.Replace(",", "','") & "') "
            StrSql += vbCrLf + "AND ISNULL(CANCEL,'') = ''  AND TRANTYPE<>'PU' )"
            StrSql += vbCrLf + "GROUP BY I.RESNO,I.STKTYPE "
            StrSql += vbCrLf + "UNION ALL"
            StrSql += vbCrLf + "SELECT "
            StrSql += vbCrLf + "-1*SUM(I.PCS)PCS"
            StrSql += vbCrLf + ",-1*SUM(I.GRSWT)GRSWT"
            StrSql += vbCrLf + ",-1*SUM(I.NETWT)NETWT"
            StrSql += vbCrLf + ",0 PUREWT"
            StrSql += vbCrLf + ",0 WASTAGE"
            StrSql += vbCrLf + ",0 MCHARGE,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE "
            StrSql += vbCrLf + " COSTID IN(SELECT COSTID FROM " & cnStockDb & "..RECEIPT WHERE SNO=I.RECSNO ))COSTNAME"
            StrSql += vbCrLf + ",I.RECSNO AS SNO,ISNULL(I.STKTYPE,'') STKTYPE "
            StrSql += vbCrLf + "FROM " & cnStockDb & "..LOTISSUE I"
            StrSql += vbCrLf + "WHERE "
            StrSql += vbCrLf + "ISNULL(I.CANCEL,'') = '' "
            StrSql += vbCrLf + "AND ISNULL(I.ITEMID,0)<>0 "
            StrSql += vbCrLf + "AND I.RECSNO IN"
            StrSql += vbCrLf + "(SELECT SNO FROM " & cnStockDb & "..RECEIPT "
            If chkAsonDate.Checked = False Then
                StrSql += vbCrLf + "  WHERE TRANDATE BETWEEN '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpToDate.Value.ToString("yyyy-MM-dd") & "'"
            Else
                StrSql += vbCrLf + "  WHERE TRANDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
            End If
            If chkCmbCostName.Text <> "ALL" And chkCmbCostName.Text <> "" Then StrSql += vbCrLf + " AND COSTID IN('" & CostId.Replace(",", "','") & "') "
            StrSql += vbCrLf + "AND ISNULL(CANCEL,'') = ''  AND TRANTYPE<>'PU')"
            StrSql += vbCrLf + "GROUP BY I.RECSNO,I.STKTYPE "
            StrSql += vbCrLf + ")X GROUP BY SNO,STKTYPE HAVING (SUM(PCS)>0 OR SUM(GRSWT)>0)"
            StrSql += vbCrLf + "ORDER BY TRANDATE,TRANNO"
        Else
            StrSql = vbCrLf + "  SELECT I.TRANNO,I.TRANDATE"
            StrSql += vbCrLf + "  ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = I.ACCODE)ACNAME"
            StrSql += vbCrLf + "  ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID)AS ITEM"
            StrSql += vbCrLf + "  ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = I.ITEMID AND SUBITEMID = I.SUBITEMID)AS SUBITEM"
            StrSql += vbCrLf + "  ,SUM(I.PCS - ISNULL(L.PCS,0))PCS"
            StrSql += vbCrLf + "  ,SUM(I.GRSWT - ISNULL(L.GRSWT,0))GRSWT"
            StrSql += vbCrLf + "  ,SUM(I.NETWT - ISNULL(L.NETWT,0))NETWT"
            StrSql += vbCrLf + "  ,I.TOUCH"
            StrSql += vbCrLf + "  ,I.PUREWT PUREWT,I.WASTAGE,I.MCHARGE,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID=I.COSTID)COSTNAME"
            StrSql += vbCrLf + "  ,I.SNO,I.ITEMTYPEID,ACCODE"
            StrSql += vbCrLf + "  ,I.CUTID,I.COLORID,I.CLARITYID,I.SETTYPEID,I.SHAPEID,I.HEIGHT,I.WIDTH,I.STKTYPE"

            StrSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT I"
            StrSql += vbCrLf + "  LEFT OUTER JOIN " & cnStockDb & "..LOTISSUE AS L ON L.TRANNO = I.TRANNO AND L.TRANDATE = I.TRANDATE AND L.RECSNO = I.SNO "
            StrSql += vbCrLf + "  AND (ISNULL(JOBISNO,'')='' OR ISNULL(JOBISNO,'')<>I.SNO) "
            If chkAsonDate.Checked = False Then
                StrSql += vbCrLf + "  WHERE I.TRANDATE BETWEEN '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpToDate.Value.ToString("yyyy-MM-dd") & "'"
            Else
                StrSql += vbCrLf + "  WHERE I.TRANDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
            End If
            StrSql += vbCrLf + "  AND ISNULL(JOBISNO,'') NOT IN(SELECT RECSNO FROM " & cnStockDb & "..LOTISSUE)"
            StrSql += vbCrLf + "  AND NOT EXISTS(SELECT 1 FROM  " & cnStockDb & "..ISSUE WHERE BATCHNO=I.BATCHNO)"
            StrSql += vbCrLf + "  AND ISNULL(I.CANCEL,'') = '' AND ISNULL(L.CANCEL,'') = ''"
            StrSql += vbCrLf + "  AND (I.GRSWT <> 0 OR I.PCS <> 0) AND LEN(I.TRANTYPE) > 2 "            
            If chkCmbCostName.Text <> "ALL" And chkCmbCostName.Text <> "" Then StrSql += vbCrLf + " AND I.COSTID IN('" & CostId.Replace(",", "','") & "')"
            StrSql += vbCrLf + "  GROUP BY I.TRANNO,I.TRANDATE,I.ACCODE,I.ITEMID,I.SUBITEMID,I.TOUCH,I.PUREWT,I.SNO,I.ITEMTYPEID,I.WASTAGE,I.MCHARGE"
            StrSql += vbCrLf + "  ,I.CUTID,I.COLORID,I.CLARITYID,I.SETTYPEID,I.SHAPEID,I.HEIGHT,I.WIDTH,I.COSTID,I.STKTYPE"
            StrSql += vbCrLf + "  HAVING "
            StrSql += vbCrLf + "  (SUM(I.PCS - ISNULL(L.PCS,0)) > 0"
            StrSql += vbCrLf + "  OR SUM(I.GRSWT - ISNULL(L.GRSWT,0)) > 0)"
            StrSql += vbCrLf + "  ORDER BY I.TRANDATE,I.TRANNO"
        End If
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dtGrid)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        gridView.DataSource = dtGrid
        BrighttechPack.GlobalMethods.FormatGridColumns(gridView)

        gridView.Columns("CHECK").Width = 45
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
        gridView.Columns("COSTNAME").Width = 120
        gridView.Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
        gridView.Columns("GRSWT").DefaultCellStyle.Format = "0.000"
        gridView.Columns("NETWT").DefaultCellStyle.Format = "0.000"
        gridView.Columns("PUREWT").DefaultCellStyle.Format = "0.000"
        gridView.Columns("TOUCH").DefaultCellStyle.Format = "0.00"
        gridView.Columns("SNO").Visible = False
        gridView.Columns("CHECK").ReadOnly = False
        gridView.Columns("ITEMTYPEID").Visible = False
        gridView.Columns("PUREWT").Visible = False
        gridView.Columns("TOUCH").Visible = False
        gridView.Columns("ACCODE").Visible = False
        If gridView.Columns.Contains("CUTID") Then gridView.Columns("CUTID").Visible = False
        If gridView.Columns.Contains("COLORID") Then gridView.Columns("COLORID").Visible = False
        If gridView.Columns.Contains("CLARITYID") Then gridView.Columns("CLARITYID").Visible = False
        If gridView.Columns.Contains("SHAPEID") Then gridView.Columns("SHAPEID").Visible = False
        If gridView.Columns.Contains("SETTYPEID") Then gridView.Columns("SETTYPEID").Visible = False
        If gridView.Columns.Contains("HEIGHT") Then gridView.Columns("HEIGHT").Visible = False
        If gridView.Columns.Contains("WIDTH") Then gridView.Columns("WIDTH").Visible = False
        gridView.Focus()
        btnLotIssue.Enabled = chkSelectAll.Checked
    End Sub

    Private Sub btnLotIssue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLotIssue.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
        If Not CheckDate(dtpAsOnDate.Value) Then Exit Sub
        If CheckEntryDate(dtpAsOnDate.Value) Then Exit Sub

        Dim Tran As OleDbTransaction = Nothing
        Dim DesignerId As String = Nothing
        Dim COSTID As String = Nothing
        Dim itemId As Integer = Nothing
        Dim stockType As String = Nothing
        Dim subItemId As Integer = Nothing
        Dim RoIssue As DataRow() = Nothing
        Dim noOfTag As Integer = Nothing
        Dim itemCounterId As Integer = Nothing
        Dim LotNo As Integer
        Dim entryType As String = Nothing
        Dim dtSelected As New DataTable
        Dim dtRepair As New DataTable
        dtSelected = CType(gridView.DataSource, DataTable)
        dtSelected.AcceptChanges()
        RoIssue = dtSelected.Select("CHECK = 'TRUE'", String.Empty)
        dtRepair = CType(gridView.DataSource, DataTable)
        If HideAccLink = False Then
            For cnt As Integer = 0 To RoIssue.Length - 1
                StrSql = " SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER "
                StrSql += vbCrLf + " WHERE ISNULL(ACCODE,'') = '" & RoIssue(cnt).Item("ACCODE").ToString & "'"
                DesignerId = objGPack.GetSqlValue(StrSql, , , Tran)
                If DesignerId = "" Then
                    MsgBox(RoIssue(cnt).Item("ACNAME").ToString & vbCrLf & "Designer info not found", MsgBoxStyle.Information)
                    Exit Sub
                End If
            Next
        End If
        Dim objBulkIssue As New LotBulkIssueDia
        If objBulkIssue.ShowDialog <> Windows.Forms.DialogResult.OK Then Exit Sub
        Try

            Tran = cn.BeginTransaction()
            'Getting LotNo
GENLOTNO:
            StrSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'LOTNO'"
            LotNo = Val(objGPack.GetSqlValue(StrSql, , , Tran))
            StrSql = " UPDATE " & cnAdminDb & "..SOFTCONTROL SET CTLTEXT = '" & LotNo + 1 & "' "
            StrSql += " WHERE CTLID = 'LOTNO' AND CTLTEXT = " & LotNo & ""
            Cmd = New OleDbCommand(StrSql, cn, Tran)
            If Cmd.ExecuteNonQuery() = 0 Then
                GoTo GENLOTNO
            End If
            LotNo += 1

            If objBulkIssue.chkIssToAssort.Checked = True Then
                itemId = Val(objBulkIssue.txtItemCode_Num_Man.Text)
   
                ''Find SubItem Id
                StrSql = " SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST "
                StrSql += " WHERE SUBITEMNAME = '" & objBulkIssue.cmbSubItemName_Man.Text & "'"
                subItemId = Val(objGPack.GetSqlValue(StrSql, , , Tran))
            End If

            For cnt As Integer = 0 To RoIssue.Length - 1
                Dim lotSno As String = GetNewSno(TranSnoType.ITEMLOTCODE, Tran, "GET_ADMINSNO_TRAN") '  GetWSno(TranSnoType.ITEMLOTCODE, Tran, CnStockdb)

                If objBulkIssue.chkIssToAssort.Checked = False Then
                    ''Find ItemId
                    StrSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & RoIssue(cnt).Item("ITEM") & "'"
                    itemId = Val(objGPack.GetSqlValue(StrSql, , , Tran))
                End If


                StrSql = " SELECT STOCKTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & RoIssue(cnt).Item("ITEM") & "'"
                stockType = objGPack.GetSqlValue(StrSql, , , Tran)
                ''Find DesignerId
                If objBulkIssue.cmbDesigner_MAN.Enabled Then
                    StrSql = " SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & objBulkIssue.cmbDesigner_MAN.Text & "'"
                    DesignerId = objGPack.GetSqlValue(StrSql, , , Tran)
                Else
                    StrSql = " SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE ISNULL(ACCODE,'') = '" & RoIssue(cnt).Item("ACCODE").ToString & "'"
                    DesignerId = objGPack.GetSqlValue(StrSql, , , Tran)
                End If
                ''Find COSTID
                StrSql = " SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & objBulkIssue.cmbCostCentre_Man.Text & "'"
                COSTID = objGPack.GetSqlValue(StrSql, , , Tran)

                If objBulkIssue.chkIssToAssort.Checked = False Then
                    ''Find SubItem Id
                    StrSql = " SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST "
                    StrSql += " WHERE SUBITEMNAME = '" & RoIssue(cnt).Item("SUBITEM") & "'"
                    subItemId = Val(objGPack.GetSqlValue(StrSql, , , Tran))
                End If

                ''Find ItemTypeId
                StrSql = " SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & objBulkIssue.cmbItemCounter_MAN.Text & "'"
                itemCounterId = Val(objGPack.GetSqlValue(StrSql, , , Tran))

                StrSql = " SELECT NOOFPIECE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & RoIssue(cnt).Item("ITEM") & "'"
                noOfTag = Val(objGPack.GetSqlValue(StrSql, , , Tran))

                Select Case objBulkIssue.cmbEntryType.Text.ToUpper
                    Case "REGULAR"
                        entryType = "R"
                    Case "ORDER"
                        entryType = "OR"
                    Case "REPAIR"
                        entryType = "RE"
                    Case Else 'NONTAG TO TAG
                        entryType = "NT"
                End Select
                If entryType = "RE" And objBulkIssue.chkIssToAssort.Checked Then
                    Dim Pcs As Integer = 0
                    Dim GrsWt As Decimal = 0
                    Dim NetWt As Decimal = 0
                    Dim Mc As Decimal = 0
                    Dim Wast As Decimal = 0
                    StrSql = "  INSERT INTO " & cnStockDb & "..LOTISSUE"
                    StrSql += "  ("
                    StrSql += "  TRANNO"
                    StrSql += "  ,TRANDATE"
                    StrSql += "  ,GRSWT"
                    StrSql += "  ,NETWT"
                    StrSql += "  ,CANCEL"
                    StrSql += "  ,BATCHNO"
                    StrSql += "  ,USERID"
                    StrSql += "  ,UPDATED"
                    StrSql += "  ,APPVER"
                    StrSql += "  ,COMPANYID"
                    StrSql += "  ,PCS"
                    StrSql += "  ,LOTSNO"
                    StrSql += "  ,ITEMID"
                    StrSql += "  ,SUBITEMID"
                    StrSql += "  ,RECSNO"
                    StrSql += "  ,STKTYPE"
                    StrSql += "  )"
                    StrSql += "  SELECT"
                    StrSql += "  " & RoIssue(cnt).Item("TRANNO") & ""
                    StrSql += "  ,'" & RoIssue(cnt).Item("TRANDATE") & "'"
                    StrSql += "  ," & Val(RoIssue(cnt).Item("GRSWT").ToString) & "" 'GRSWT
                    StrSql += "  ," & Val(RoIssue(cnt).Item("NETWT").ToString) & "" 'NETWT
                    StrSql += "  ,''" 'CANCEL
                    StrSql += "  ,''" 'BATCHNO
                    StrSql += "  ," & userId & "" 'USERID
                    StrSql += "  ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                    StrSql += "  ,'" & VERSION & "'" 'APPVER
                    StrSql += "  ,'" & GetStockCompId() & "'" 'COMPANYID
                    StrSql += "  ," & Val(RoIssue(cnt).Item("PCS").ToString) & "" 'PCS
                    StrSql += "  ,'" & lotSno & "'" 'LOTSNO
                    StrSql += "  ," & itemId & "" 'ITEMID
                    StrSql += "  ," & subItemId & "" 'SUBITEMID
                    StrSql += "  ,'" & RoIssue(cnt).Item("SNO").ToString & "'" 'SNO
                    StrSql += "  ,'" & RoIssue(cnt).Item("STKTYPE").ToString & "'" 'SNO
                    ExecQuery(SyncMode.Stock, StrSql, cn, Tran, COSTID)
                    If cnt = 0 Then
                        Pcs = dtRepair.Compute("SUM(PCS)", "CHECK = 'TRUE'")
                        GrsWt = dtRepair.Compute("SUM(GRSWT)", "CHECK = 'TRUE'")
                        NetWt = dtRepair.Compute("SUM(NETWT)", "CHECK = 'TRUE'")
                        Wast = dtRepair.Compute("SUM(WASTAGE)", "CHECK = 'TRUE'")
                        Mc = dtRepair.Compute("SUM(MCHARGE)", "CHECK = 'TRUE'")
                        StrSql = " INSERT INTO " & cnAdminDb & "..ITEMLOT "
                        StrSql += " ("
                        StrSql += " SNO,ENTRYTYPE,LOTDATE,LOTNO,DESIGNERID,TRANINVNO,"
                        StrSql += " BILLNO,COSTID,ENTRYORDER,ORDREPNO,ORDENTRYORDER,"
                        StrSql += " ITEMID,SUBITEMID,PCS,GRSWT,STNPCS,STNWT,STNUNIT,"
                        StrSql += " DIAPCS,DIAWT,NETWT,NOOFTAG,RATE,ITEMCTRID,WMCTYPE,"
                        StrSql += " BULKLOT,MULTIPLETAGS,NARRATION,FINERATE,TUCH,"
                        StrSql += " WASTPER,MCGRM,OTHCHARGE,STARTTAGNO,ENDTAGNO,CURTAGNO,"
                        StrSql += " COMPANYID,CPCS,CGRSWT,COMPLETED,CANCEL,"
                        StrSql += " ACCESSING,USERID,UPDATED,"
                        StrSql += " UPTIME,SYSTEMID,APPVER,ITEMTYPEID,"
                        StrSql += " CUTID,COLORID,CLARITYID,SETTYPEID,SHAPEID,HEIGHT,WIDTH,STKTYPE "
                        StrSql += " )VALUES("
                        StrSql += " '" & lotSno & "'" 'SNO
                        StrSql += " ,'" & entryType & "'" 'ENTRYTYPE
                        StrSql += " ,'" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'" 'LOTDATE 'DateTime.Parse(RoIssue(CNT).ITEM("LOTDATE"), ukCultureInfo.DateTimeFormat).ToString("yyyy-MM-dd")
                        StrSql += " ," & LotNo & "" 'LOTNO
                        StrSql += " ," & DesignerId & "" 'DESIGNERID
                        StrSql += " ,''" 'TRANINVNO
                        StrSql += " ,''" 'BILLNO
                        StrSql += " ,'" & COSTID & "'" 'COSTID
                        StrSql += " ," & cnt + 1 & "" 'ENTRYORDER
                        StrSql += " ,''" 'ORDREPNO
                        StrSql += " ,0" 'ORDENTRYORDER
                        StrSql += " ," & Val(itemId) & "" 'ITEMID
                        StrSql += " ," & Val(subItemId) & "" 'SUBITEMID
                        StrSql += " ," & Pcs & "" 'PCS
                        StrSql += " ," & GrsWt & "" 'GRSWT
                        StrSql += " ,0" 'STNPCS
                        StrSql += " ,0" 'STNWT
                        StrSql += " ,'G'" 'STNUNIT
                        StrSql += " ,0" 'DIAPCS
                        StrSql += " ,0" 'DIAWT
                        StrSql += " ," & NetWt & "" 'NETWT
                        StrSql += " ," & IIf(noOfTag = 0, 1, noOfTag) & "" 'NOOFTAG
                        StrSql += " ,0" 'RATE
                        StrSql += " ," & Val(itemCounterId) & "" 'ITEMCTRID
                        StrSql += " ,'" & objGPack.GetSqlValue("SELECT VALUEADDEDTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & Val(itemId) & "", , , Tran) & "'" 'WMCTYPE
                        StrSql += " ,'N'" 'BULKLOT
                        StrSql += " ,'N'" 'MULTIPLETAGS
                        StrSql += " ,''" 'NARRATION
                        StrSql += " ,0" 'FINERATE
                        StrSql += " ," & Val(RoIssue(cnt).Item("TOUCH").ToString) & "" 'TUCH
                        If Wast <> 0 Then
                            StrSql += " ," & Math.Round((Wast / GrsWt) * 100, 2) & "" 'WASTPER
                        Else
                            StrSql += " ,NULL" 'WASTPER
                        End If
                        If GrsWt <> 0 Then
                            StrSql += " ," & Math.Round(Mc / GrsWt, 2) & "" 'MCGRM
                        Else
                            StrSql += " ,NULL" 'MCGRM
                        End If
                        StrSql += " ,0" 'OTHCHARGE
                        StrSql += " ,''" 'STARTTAGNO
                        StrSql += " ,''" 'ENDTAGNO
                        StrSql += " ,''" 'CURTAGNO
                        StrSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                        StrSql += " ,0" 'CPIECE
                        StrSql += " ,0" 'CWEIGHT
                        StrSql += " ,''" 'COMPLETED
                        StrSql += " ,''" 'CANCEL
                        StrSql += " ,''" 'ACCESSING
                        StrSql += " ," & userId & "" 'USERID
                        StrSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                        StrSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                        StrSql += " ,'" & systemId & "'" 'SYSTEMID
                        StrSql += " ,'" & VERSION & "'" 'APPVER
                        StrSql += " ," & Val(RoIssue(cnt).Item("ITEMTYPEID").ToString) & "" 'ITEMTYPEID
                        StrSql += " ," & Val(RoIssue(cnt).Item("CUTID").ToString) & "" 'CUTID
                        StrSql += " ," & Val(RoIssue(cnt).Item("COLORID").ToString) & "" 'COLORID
                        StrSql += " ," & Val(RoIssue(cnt).Item("CLARITYID").ToString) & "" 'CLARITYID
                        StrSql += " ," & Val(RoIssue(cnt).Item("SETTYPEID").ToString) & "" 'SETTYPEID
                        StrSql += " ," & Val(RoIssue(cnt).Item("SHAPEID").ToString) & "" 'SHAPEID
                        StrSql += " ," & Val(RoIssue(cnt).Item("HEIGHT").ToString) & "" 'HEIGHT
                        StrSql += " ," & Val(RoIssue(cnt).Item("WIDTH").ToString) & "" 'WIDTH
                        StrSql += " ,'" & RoIssue(cnt).Item("STKTYPE").ToString & "'" 'STKTYPE
                        StrSql += " )"
                        ExecQuery(SyncMode.Stock, StrSql, cn, Tran, COSTID)
                    End If
                Else
                    Dim WastPer As Decimal = IIf(Val(RoIssue(cnt).Item("GRSWT").ToString) <> 0, Math.Round((Val(RoIssue(cnt).Item("WASTAGE").ToString) / Val(RoIssue(cnt).Item("GRSWT").ToString)) * 100, 2), 0)
                    StrSql = " INSERT INTO " & cnAdminDb & "..ITEMLOT "
                    StrSql += " ("
                    StrSql += " SNO,ENTRYTYPE,LOTDATE,LOTNO,DESIGNERID,TRANINVNO,"
                    StrSql += " BILLNO,COSTID,ENTRYORDER,ORDREPNO,ORDENTRYORDER,"
                    StrSql += " ITEMID,SUBITEMID,PCS,GRSWT,STNPCS,STNWT,STNUNIT,"
                    StrSql += " DIAPCS,DIAWT,NETWT,NOOFTAG,RATE,ITEMCTRID,WMCTYPE,"
                    StrSql += " BULKLOT,MULTIPLETAGS,NARRATION,FINERATE,TUCH,"
                    StrSql += " WASTPER,MCGRM,OTHCHARGE,STARTTAGNO,ENDTAGNO,CURTAGNO,"
                    StrSql += " COMPANYID,CPCS,CGRSWT,COMPLETED,CANCEL,"
                    StrSql += " ACCESSING,USERID,UPDATED,"
                    StrSql += " UPTIME,SYSTEMID,APPVER,ITEMTYPEID"
                    If STOCKVALIDATION = False Then
                        StrSql += ",CUTID,COLORID,CLARITYID,SETTYPEID,SHAPEID,HEIGHT,WIDTH "
                    End If
                    StrSql += " ,STKTYPE"
                    StrSql += " )VALUES("
                    StrSql += " '" & lotSno & "'" 'SNO
                    StrSql += " ,'" & entryType & "'" 'ENTRYTYPE
                    StrSql += " ,'" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'" 'LOTDATE 'DateTime.Parse(RoIssue(CNT).ITEM("LOTDATE"), ukCultureInfo.DateTimeFormat).ToString("yyyy-MM-dd")
                    StrSql += " ," & LotNo & "" 'LOTNO
                    StrSql += " ," & DesignerId & "" 'DESIGNERID
                    StrSql += " ,''" 'TRANINVNO
                    StrSql += " ,''" 'BILLNO
                    StrSql += " ,'" & COSTID & "'" 'COSTID
                    StrSql += " ," & cnt + 1 & "" 'ENTRYORDER
                    StrSql += " ,''" 'ORDREPNO
                    StrSql += " ,0" 'ORDENTRYORDER
                    StrSql += " ," & Val(itemId) & "" 'ITEMID
                    StrSql += " ," & Val(subItemId) & "" 'SUBITEMID
                    StrSql += " ," & Val(RoIssue(cnt).Item("PCS").ToString) & "" 'PCS
                    StrSql += " ," & Val(RoIssue(cnt).Item("GRSWT").ToString) & "" 'GRSWT
                    StrSql += " ,0" 'STNPCS
                    StrSql += " ,0" 'STNWT
                    StrSql += " ,'G'" 'STNUNIT
                    StrSql += " ,0" 'DIAPCS
                    StrSql += " ,0" 'DIAWT
                    StrSql += " ," & Val(RoIssue(cnt).Item("NETWT").ToString) & "" 'NETWT
                    StrSql += " ," & IIf(noOfTag = 0, 1, noOfTag) & "" 'NOOFTAG
                    StrSql += " ,0" 'RATE
                    StrSql += " ," & Val(itemCounterId) & "" 'ITEMCTRID
                    StrSql += " ,'" & objGPack.GetSqlValue("SELECT VALUEADDEDTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & RoIssue(cnt).Item("ITEM").ToString & "'", , , Tran) & "'" 'WMCTYPE
                    StrSql += " ,'N'" 'BULKLOT
                    StrSql += " ,'N'" 'MULTIPLETAGS
                    StrSql += " ,''" 'NARRATION
                    StrSql += " ,0" 'FINERATE
                    StrSql += " ," & Val(RoIssue(cnt).Item("TOUCH").ToString) & "" 'TUCH
                    StrSql += " ," & WastPer & "" 'WASTPER
                    StrSql += " ," & IIf(Val(RoIssue(cnt).Item("GRSWT").ToString) <> 0, Math.Round(Val(RoIssue(cnt).Item("MCHARGE").ToString) / Val(RoIssue(cnt).Item("GRSWT").ToString), 2), 0) & "" 'MCGRM
                    StrSql += " ,0" 'OTHCHARGE
                    StrSql += " ,''" 'STARTTAGNO
                    StrSql += " ,''" 'ENDTAGNO
                    StrSql += " ,''" 'CURTAGNO
                    StrSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                    StrSql += " ,0" 'CPIECE
                    StrSql += " ,0" 'CWEIGHT
                    StrSql += " ,''" 'COMPLETED
                    StrSql += " ,''" 'CANCEL
                    StrSql += " ,''" 'ACCESSING
                    StrSql += " ," & userId & "" 'USERID
                    StrSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                    StrSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                    StrSql += " ,'" & systemId & "'" 'SYSTEMID
                    StrSql += " ,'" & VERSION & "'" 'APPVER
                    StrSql += " ," & Val(RoIssue(cnt).Item("ITEMTYPEID").ToString) & "" 'ITEMTYPEID
                    If STOCKVALIDATION = False Then
                        StrSql += " ," & Val(RoIssue(cnt).Item("CUTID").ToString) & "" 'CUTID
                        StrSql += " ," & Val(RoIssue(cnt).Item("COLORID").ToString) & "" 'COLORID
                        StrSql += " ," & Val(RoIssue(cnt).Item("CLARITYID").ToString) & "" 'CLARITYID
                        StrSql += " ," & Val(RoIssue(cnt).Item("SETTYPEID").ToString) & "" 'SETTYPEID
                        StrSql += " ," & Val(RoIssue(cnt).Item("SHAPEID").ToString) & "" 'SHAPEID
                        StrSql += " ," & Val(RoIssue(cnt).Item("HEIGHT").ToString) & "" 'HEIGHT
                        StrSql += " ," & Val(RoIssue(cnt).Item("WIDTH").ToString) & "" 'WIDTH
                    End If
                    StrSql += " ,'" & RoIssue(cnt).Item("STKTYPE").ToString & "'" 'STKTYPE
                    StrSql += " )"
                    ExecQuery(SyncMode.Stock, StrSql, cn, Tran, COSTID)

                    StrSql = "  INSERT INTO " & cnStockDb & "..LOTISSUE"
                    StrSql += "  ("
                    StrSql += "  TRANNO"
                    StrSql += "  ,TRANDATE"
                    StrSql += "  ,GRSWT"
                    StrSql += "  ,NETWT"
                    StrSql += "  ,CANCEL"
                    StrSql += "  ,BATCHNO"
                    StrSql += "  ,USERID"
                    StrSql += "  ,UPDATED"
                    StrSql += "  ,APPVER"
                    StrSql += "  ,COMPANYID"
                    StrSql += "  ,PCS"
                    StrSql += "  ,LOTSNO"
                    StrSql += "  ,ITEMID"
                    StrSql += "  ,SUBITEMID"
                    StrSql += "  ,RECSNO"
                    StrSql += "  ,STKTYPE )"
                    StrSql += "  SELECT"
                    StrSql += "  " & RoIssue(cnt).Item("TRANNO") & ""
                    StrSql += "  ,'" & RoIssue(cnt).Item("TRANDATE") & "'"
                    StrSql += "  ," & Val(RoIssue(cnt).Item("GRSWT").ToString) & "" 'GRSWT
                    StrSql += "  ," & Val(RoIssue(cnt).Item("NETWT").ToString) & "" 'NETWT
                    StrSql += "  ,''" 'CANCEL
                    StrSql += "  ,''" 'BATCHNO
                    StrSql += "  ," & userId & "" 'USERID
                    StrSql += "  ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                    StrSql += "  ,'" & VERSION & "'" 'APPVER
                    StrSql += "  ,'" & GetStockCompId() & "'" 'COMPANYID
                    StrSql += "  ," & Val(RoIssue(cnt).Item("PCS").ToString) & "" 'PCS
                    StrSql += "  ,'" & lotSno & "'" 'LOTSNO
                    StrSql += "  ," & itemId & "" 'ITEMID
                    StrSql += "  ," & subItemId & "" 'SUBITEMID
                    StrSql += "  ,'" & RoIssue(cnt).Item("SNO").ToString & "'" 'SNO
                    StrSql += "  ,'" & RoIssue(cnt).Item("STKTYPE").ToString & "'" 'STKTYPE
                    ExecQuery(SyncMode.Stock, StrSql, cn, Tran, COSTID)
                End If
            Next
            Tran.Commit()
            Tran = Nothing
            MsgBox(LotNo.ToString + " Generated...", MsgBoxStyle.Exclamation)
            Dim prBulkLotNo As Integer = LotNo
            Dim prBulkLotDate As Date = dtpAsOnDate.Value.Date
            If PrintBulkLotIss Then
                Dim objLotPrint As New CLS_LOTPRINT(prBulkLotNo, prBulkLotDate)
                objLotPrint.Print()
            End If
        Catch ex As Exception
            If Not Tran Is Nothing Then Tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        Finally
            If Not Tran Is Nothing Then Tran.Dispose()
        End Try
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub gridView_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.CellValueChanged
        Dim dt As New DataTable
        dt = CType(gridView.DataSource, DataTable)
        dt.AcceptChanges()
        Dim ro() As DataRow = dt.Select("CHECK = 'TRUE'", String.Empty)
        If ro.Length > 0 Then
            btnLotIssue.Enabled = True
        Else
            btnLotIssue.Enabled = False
        End If
    End Sub

    Private Sub gridView_CurrentCellDirtyStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.CurrentCellDirtyStateChanged
        gridView.CommitEdit(DataGridViewDataErrorContexts.Commit)
    End Sub

    Private Sub chkAsonDate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAsonDate.CheckedChanged
        If chkAsonDate.Checked = True Then
            chkAsonDate.Text = "&As OnDate"
            pnlDate.Enabled = False
        Else
            chkAsonDate.Text = "&Date From"
            pnlDate.Enabled = True
        End If
    End Sub
End Class