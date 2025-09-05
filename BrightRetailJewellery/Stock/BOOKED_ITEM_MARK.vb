Imports System.Data.OleDb
Public Class BOOKED_ITEM_MARK

    Dim strSql As String = ""
    Dim BatchNo As String = ""

    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim tran As OleDbTransaction
    Dim dtGrid As DataTable
    Dim objRemark As New frmBillRemark
    Dim SKUTAGNOS As String = ""
    Private Sub txtRunNo_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtRunNo.KeyDown
        If e.KeyCode = Keys.Insert Then
            strSql = vbCrLf + "  SELECT "
            strSql += vbCrLf + "  DISTINCT SUBSTRING(RUNNO,6,20)AS RUNNO"
            strSql += vbCrLf + "  ,SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT END) RECEIPT"
            strSql += vbCrLf + "  ,SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT END) ADJUSTED"
            strSql += vbCrLf + "  ,SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE -1 * AMOUNT END) AMOUNT"
            strSql += vbCrLf + "  ,PNAME"
            strSql += vbCrLf + "  ,CASE WHEN ISNULL(DOORNO,'') = '' THEN ADDRESS1 ELSE ISNULL(DOORNO,'') + ISNULL(ADDRESS1,'') END ADDRESS1"
            strSql += vbCrLf + "  ,ADDRESS2,MOBILE"
            strSql += vbCrLf + "  ,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = O.COSTID) COSTCENTRE_HIDE"
            strSql += vbCrLf + "  ,(SELECT TOP 1 BATCHNO FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO = O.RUNNO) BATCHNO_HIDE"
            strSql += vbCrLf + "  ,RUNNO AS RUNNO_HIDE"
            strSql += vbCrLf + "  FROM " & cnAdminDb & "..OUTSTANDING AS O "
            strSql += vbCrLf + "  LEFT OUTER JOIN " & cnAdminDb & "..CUSTOMERINFO AS C ON O.BATCHNO = C.BATCHNO"
            strSql += vbCrLf + "  LEFT OUTER JOIN " & cnAdminDb & "..PERSONALINFO AS P ON C.PSNO = P.SNO"
            strSql += vbCrLf + "  WHERE O.COMPANYID = '" + cnCompanyId + "' "
            strSql += vbCrLf + "  AND O.COSTID = '" + cnCostId + "' "
            strSql += vbCrLf + "  AND RUNNO != '' AND ISNULL(O.CANCEL,'') != 'Y'"
            strSql += vbCrLf + "  AND TRANTYPE IN('A','C')"
            strSql += vbCrLf + "  AND NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..ITEMTAG WHERE BATCHNO=O.BATCHNO)"
            strSql += vbCrLf + "  GROUP BY RUNNO,PNAME,DOORNO,ADDRESS1,ADDRESS2,MOBILE,O.COSTID"
            strSql += vbCrLf + "  ORDER BY RUNNO"
            Dim dr As DataRow
            dr = Nothing
            dr = BrighttechPack.SearchDialog.Show_R("Find Run No", strSql, cn, , , , , , , , False)
            If dr IsNot Nothing Then
                txtRunNo.Text = dr.Item("RUNNO").ToString
                BatchNo = dr.Item("BATCHNO_HIDE").ToString
                lblRunNoCustInfo.Text = dr.Item("PNAME").ToString & IIf(dr.Item("ADDRESS1").ToString = "", "", "," & dr.Item("ADDRESS1").ToString) & IIf(dr.Item("ADDRESS2").ToString = "", "", "," & dr.Item("ADDRESS2").ToString) & IIf(dr.Item("MOBILE").ToString = "", "", "," & dr.Item("MOBILE").ToString)
            End If
            txtRunNo.SelectAll()
        End If
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If txtItemId_NUM.Text = "" Then
            MsgBox("Item Id ShouldN't Empty...", MsgBoxStyle.Information)
            txtItemId_NUM.Focus()
            Return
        End If
        gridView.DataSource = Nothing
        strSql = vbCrLf + "  SELECT "
        strSql += vbCrLf + "  T.BATCHNO,APPROVAL"
        strSql += vbCrLf + "  ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID) ITEM"
        strSql += vbCrLf + "  ,TAGNO,ITEMID,PCS,GRSWT,NETWT"
        strSql += vbCrLf + "  ,ISNULL((SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO "
        strSql += vbCrLf + "  AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')),0) DIAWT"
        strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),SALVALUE) SALVALUE,PSNO"
        strSql += vbCrLf + "  ,CASE WHEN ISNULL(INITIAL,'') = '' THEN PNAME ELSE ISNULL(INITIAL,'') + ISNULL(PNAME,'') END PNAME"
        strSql += vbCrLf + "  ,CASE WHEN ISNULL(DOORNO,'')  = '' THEN ADDRESS1 ELSE ISNULL(DOORNO,'') + ISNULL(ADDRESS1,'') END ADDRESS1"
        strSql += vbCrLf + "  ,MOBILE PHONE"
        strSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMTAG AS T "
        strSql += vbCrLf + "  LEFT OUTER JOIN " & cnAdminDb & "..CUSTOMERINFO AS C ON T.BATCHNO = C.BATCHNO"
        strSql += vbCrLf + "  LEFT OUTER JOIN " & cnAdminDb & "..PERSONALINFO AS P ON C.PSNO = P.SNO"
        strSql += vbCrLf + "  WHERE ISNULL(APPROVAL,'') = '' AND ISSDATE IS NULL"
        strSql += vbCrLf + "  AND ISNULL(T.COMPANYID,'') = '" + GetStockCompId() + "' "
        If COSTCENTRE_SINGLE = False Then
            strSql += vbCrLf + " AND ISNULL(T.COSTID,'') = '" + cnCostId + "'"
        End If
        If txtItemId_NUM.Text <> "" Then
            strSql += vbCrLf + " AND T.ITEMID = '" & txtItemId_NUM.Text + "'"
        End If
        If txtTagNo.Text <> "" Then
            strSql += vbCrLf + " AND T.TAGNO = '" & txtTagNo.Text & "'"
        End If
        If cmbCounter_MAN.Text <> "ALL" Then
            strSql += " AND T.ITEMCTRID IN (SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & cmbCounter_MAN.Text & "')"
        End If
        dtGrid = New DataTable
        dtGrid.Columns.Add("KEYNO", GetType(Integer))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        Dim dtCol As New DataColumn("CHECK", GetType(Boolean))
        dtCol.DefaultValue = True
        dtGrid.Columns.Add(dtCol)
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        gridView.DataSource = dtGrid
        FormatGridColumns(gridView, False)
        gridView.Columns("CHECK").ReadOnly = False
        gridView.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
        With gridView
            .Columns("CHECK").HeaderText = ""
            .Columns("CHECK").Width = 30
            .Columns("ITEM").Width = 175
            .Columns("ITEMID").Width = 50
            .Columns("TAGNO").Width = 100
            .Columns("PCS").Width = 50
            .Columns("GRSWT").Width = 90
            .Columns("NETWT").Width = 90
            .Columns("SALVALUE").Width = 90
            .Columns("DIAWT").Width = 90
            .Columns("PNAME").Visible = False
            .Columns("ADDRESS1").Visible = False
            .Columns("PHONE").Visible = False
            .Columns("BATCHNO").Visible = False
            .Columns("APPROVAL").Visible = False
            .Columns("PSNO").Visible = False
            .Columns("KEYNO").Visible = False
        End With
        Dim dtGridHeader As New DataTable
        dtGridHeader = dtGrid.Clone
        dtGridHeader.Columns("CHECK").DataType = GetType(String)
        dtGridHeader.Rows.Add()
        dtGridHeader.Rows.Add()
        dtGridHeader.Rows(0).Item("CHECK") = ""
        dtGridHeader.Rows(1).Item("CHECK") = ""
        dtGridHeader.Rows(0).Item("ITEM") = "TOTAL"
        dtGridHeader.Rows(1).Item("ITEM") = "SELECTED"
        dtGridHeader.Rows(0).Item("TAGNO") = "TAG COUNT :"
        dtGridHeader.Rows(1).Item("TAGNO") = "TAG COUNT :"

        ',PCS,GRSWT,NETWT,SALVALUE
        Dim obj As Object
        obj = dtGrid.Compute("SUM(PCS)", "PCS IS NOT NULL")
        dtGridHeader.Rows(0).Item("PCS") = IIf(Val(obj.ToString) <> 0, obj, DBNull.Value)
        dtGridHeader.Rows(1).Item("PCS") = IIf(Val(obj.ToString) <> 0, obj, DBNull.Value)
        obj = dtGrid.Compute("SUM(GRSWT)", "GRSWT IS NOT NULL")
        dtGridHeader.Rows(0).Item("GRSWT") = IIf(Val(obj.ToString) <> 0, Format(obj, "0.000"), DBNull.Value)
        dtGridHeader.Rows(1).Item("GRSWT") = IIf(Val(obj.ToString) <> 0, Format(obj, "0.000"), DBNull.Value)
        obj = dtGrid.Compute("SUM(NETWT)", "NETWT IS NOT NULL")
        dtGridHeader.Rows(0).Item("NETWT") = IIf(Val(obj.ToString) <> 0, Format(obj, "0.000"), DBNull.Value)
        dtGridHeader.Rows(1).Item("NETWT") = IIf(Val(obj.ToString) <> 0, Format(obj, "0.000"), DBNull.Value)
        obj = dtGrid.Compute("SUM(DIAWT)", "DIAWT IS NOT NULL")
        dtGridHeader.Rows(0).Item("DIAWT") = IIf(Val(obj.ToString) <> 0, Format(obj, "0.000"), DBNull.Value)
        dtGridHeader.Rows(1).Item("DIAWT") = IIf(Val(obj.ToString) <> 0, Format(obj, "0.000"), DBNull.Value)
        obj = dtGrid.Compute("SUM(SALVALUE)", "SALVALUE IS NOT NULL")
        dtGridHeader.Rows(0).Item("SALVALUE") = IIf(Val(obj.ToString) <> 0, Format(obj, "0.000"), DBNull.Value)
        dtGridHeader.Rows(1).Item("SALVALUE") = IIf(Val(obj.ToString) <> 0, Format(obj, "0.000"), DBNull.Value)
        obj = dtGrid.Compute("COUNT(PCS)", String.Empty)
        dtGridHeader.Rows(0).Item("ITEMID") = IIf(Val(obj.ToString) <> 0, obj, DBNull.Value)
        dtGridHeader.Rows(1).Item("ITEMID") = IIf(Val(obj.ToString) <> 0, obj, DBNull.Value)
        If Val(obj.ToString) > 0 Then btnMark.Enabled = True
        gridViewFooter.DataSource = Nothing
        gridViewFooter.DataSource = dtGridHeader
        FormatGridColumns(gridViewFooter, False)
        gridViewFooter.DefaultCellStyle.BackColor = lblRowDet1.BackColor
        gridViewFooter.DefaultCellStyle.SelectionBackColor = lblRowDet1.BackColor
        gridViewFooter.DefaultCellStyle.SelectionForeColor = Color.Red
        gridViewFooter.DefaultCellStyle.ForeColor = Color.Red
        gridViewFooter.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        For cnt As Integer = 0 To gridView.Columns.Count - 1
            gridViewFooter.Columns(cnt).Visible = gridView.Columns(cnt).Visible
            gridViewFooter.Columns(cnt).Width = gridView.Columns(cnt).Width
        Next
        gridViewFooter.Columns("TAGNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridView.Select()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        objGPack.TextClear(Me)
        lblRowDet1.Text = ""
        lblRowDet1.Visible = False
        gridView.DataSource = Nothing
        gridViewFooter.DataSource = Nothing
        btnMark.Enabled = False
        objRemark.cmbRemark1_OWN.Text = ""
        objRemark.cmbRemark2_OWN.Text = ""
        lblRunNoCustInfo.Text = ""
        cmbCounter_MAN.Text = "ALL"
        BatchNo = ""
        Me.Refresh()
        txtRunNo.Focus()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub btnMark_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMark.Click
        Try
            objRemark = New frmBillRemark
            objRemark.Text = "Booked Item Unmark Remark"
            If objRemark.ShowDialog() <> Windows.Forms.DialogResult.OK Then
                Exit Sub
            End If
            tran = cn.BeginTransaction
            For cnt As Integer = 0 To gridView.Rows.Count - 1
                With gridView.Rows(cnt)
                    If .Cells("CHECK").Value = True Then
                        strSql = vbCrLf + "  INSERT INTO " & cnStockDb & "..BOOKEDITEM_HISTORY"
                        strSql += vbCrLf + "  (ITEMID,TAGNO,BATCHNO,FLAG,USERID,UPDATED,UPTIME,REMARK1,REMARK2,COMPANYID,COSTID,TRANDATE)"
                        strSql += vbCrLf + "  VALUES"
                        strSql += vbCrLf + "  ("
                        strSql += vbCrLf + "  '" + .Cells("ITEMID").Value.ToString() + "','" + .Cells("TAGNO").Value.ToString + "','" + BatchNo + "'"
                        strSql += vbCrLf + " ,'M','" + userId.ToString + "'"
                        strSql += vbCrLf + " ,'" + DateTime.Today.Date.ToString("yyyy-MM-dd") + "'"
                        strSql += vbCrLf + " ,'" + Date.Now.ToLongTimeString + "'"
                        strSql += vbCrLf + " ,'" + objRemark.cmbRemark1_OWN.Text + "'"
                        strSql += vbCrLf + " ,'" + objRemark.cmbRemark2_OWN.Text + "'"
                        strSql += vbCrLf + " ,'" + cnCompanyId + "'"
                        strSql += vbCrLf + " ,'" + cnCostId + "'"
                        strSql += vbCrLf + " ,'" + GetEntryDate(GetServerDate(tran), tran) + "'"
                        strSql += vbCrLf + "  )"
                        ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                        SKUTAGNOS += "'" & .Cells("TAGNO").Value.ToString & "',"
                    End If
                End With
            Next cnt
            tran.Commit()
            tran = Nothing
            MsgBox(" Successfully Inserted...")
            If SKUTAGNOS <> "" Then GenerateSkuFile(cn, tran, "", SKUTAGNOS)
            SKUTAGNOS = ""
            btnNew_Click(Me, New EventArgs())
        Catch ex As Exception
            If tran IsNot Nothing Then tran.Rollback()
            MsgBox("Error   :" + ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub

    Private Sub BOOKED_ITEM_MARK_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        If e.KeyChar = Convert.ToChar(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub CalcSelectedValues()
        Dim dt As New DataTable
        dt = CType(gridView.DataSource, DataTable)
        Dim Cnt As Integer = Val(dt.Compute("COUNT(CHECK)", "CHECK = TRUE").ToString())
        If Cnt > 0 Then
            btnMark.Enabled = True
        Else
            btnMark.Enabled = False
        End If
    End Sub

    Private Sub gridView_CellValueChanged(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.CellValueChanged
        'CalcSelectedValues()
        If Not e.RowIndex > -1 Then Exit Sub
        gridView.CurrentCell = gridView.Rows(e.RowIndex).Cells("CHECK")
        ''CalcSelected Values
        With gridView.Rows(e.RowIndex)
            Dim pcs As Integer = Val(gridViewFooter.Rows(1).Cells("PCS").Value.ToString)
            Dim grsWt As Decimal = Val(gridViewFooter.Rows(1).Cells("GRSWT").Value.ToString)
            Dim DiaWt As Decimal = Val(gridViewFooter.Rows(1).Cells("DIAWT").Value.ToString)
            Dim netWT As Decimal = Val(gridViewFooter.Rows(1).Cells("NETWT").Value.ToString)
            Dim salValue As Decimal = Val(gridViewFooter.Rows(1).Cells("SALVALUE").Value.ToString)
            Dim cnt As Decimal = Val(gridViewFooter.Rows(1).Cells("ITEMID").Value.ToString)
            If CType(gridView.Rows(e.RowIndex).Cells("CHECK").Value, Boolean) = True Then
                pcs += Val(.Cells("PCS").Value.ToString)
                grsWt += Val(.Cells("GRSWT").Value.ToString)
                netWT += Val(.Cells("NETWT").Value.ToString)
                DiaWt += Val(.Cells("DIAWT").Value.ToString)
                salValue += Val(.Cells("SALVALUE").Value.ToString)
                cnt += 1
            Else
                pcs -= Val(.Cells("PCS").Value.ToString)
                grsWt -= Val(.Cells("GRSWT").Value.ToString)
                netWT -= Val(.Cells("NETWT").Value.ToString)
                DiaWt -= Val(.Cells("DIAWT").Value.ToString)
                salValue -= Val(.Cells("SALVALUE").Value.ToString)
                cnt -= 1
            End If
            If cnt > 0 Then
                btnMark.Enabled = True
            Else
                btnMark.Enabled = False
            End If
            gridViewFooter.Rows(1).Cells("TAGNO").Value = "TAG COUNT :"
            gridViewFooter.Rows(1).Cells("TAGNO").Style.Alignment = DataGridViewContentAlignment.MiddleRight
            gridViewFooter.Rows(1).Cells("ITEMID").Value = IIf(cnt <> 0, cnt, DBNull.Value)
            gridViewFooter.Rows(1).Cells("PCS").Value = IIf(pcs <> 0, pcs, DBNull.Value)
            gridViewFooter.Rows(1).Cells("GRSWT").Value = IIf(grsWt <> 0, Format(grsWt, "0.000"), DBNull.Value)
            gridViewFooter.Rows(1).Cells("NETWT").Value = IIf(netWT <> 0, Format(netWT, "0.000"), DBNull.Value)
            gridViewFooter.Rows(1).Cells("DIAWT").Value = IIf(DiaWt <> 0, Format(DiaWt, "0.000"), DBNull.Value)
            gridViewFooter.Rows(1).Cells("SALVALUE").Value = IIf(salValue <> 0, Format(salValue, "0.000"), DBNull.Value)
        End With
    End Sub

    Private Sub gridView_CurrentCellDirtyStateChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles gridView.CurrentCellDirtyStateChanged
        gridView.CommitEdit(DataGridViewDataErrorContexts.Commit)
    End Sub

    Private Sub BOOKED_ITEM_MARK_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        strSql = " SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ACTIVE = 'Y' ORDER BY DISPLAYORDER,ITEMCTRNAME"
        cmbCounter_MAN.Items.Add("ALL")
        objGPack.FillCombo(strSql, cmbCounter_MAN, False, False)
        cmbCounter_MAN.Text = "ALL"
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub gridView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.GotFocus
        lblRowDet1.Visible = False
    End Sub

    Private Sub gridView_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.LostFocus
        lblRowDet1.Visible = True
    End Sub

    Private Sub gridView_RowEnter(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.RowEnter
        With gridView.Rows(e.RowIndex)
            lblRowDet1.Text = "Party : " + .Cells("PNAME").Value.ToString + " "
            lblRowDet1.Text += "" + IIf(.Cells("ADDRESS1").Value.ToString = "", "", "," + .Cells("ADDRESS1").ToString()) + " "
            lblRowDet1.Text += "" + IIf(.Cells("PHONE").Value.ToString = "", "", "PH :" + .Cells("PHONE").Value.ToString) + ""
        End With
    End Sub

    Private Sub txtItemId_NUM_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtItemId_NUM.KeyDown
        If e.KeyCode = Keys.Insert Then LoadItemName()
    End Sub

    Private Sub txtTagNo_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtTagNo.KeyDown
        If e.KeyCode = Keys.Insert Then
            strSql = " SELECT"
            strSql += vbCrLf + " TAGNO AS TAGNO,STYLENO,ITEMID AS ITEMID,"
            strSql += vbCrLf + " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS ITEMNAME,"
            strSql += vbCrLf + " PCS AS PCS,"
            strSql += vbCrLf + " GRSWT AS GRS_WT,NETWT AS NET_WT,RATE AS RATE,"
            strSql += vbCrLf + " SALVALUE AS SALVALUE,"
            strSql += vbCrLf + " ISNULL((SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID  = T.SUBITEMID),'')AS SUBITEM,"
            strSql += vbCrLf + " CONVERT(VARCHAR,RECDATE,103) AS RECDATE,"
            strSql += vbCrLf + " CASE WHEN APPROVAL = 'A' THEN 'APPROVAL' WHEN APPROVAL = 'R' THEN 'RESERVED' ELSE NULL END AS STATUS,"
            strSql += vbCrLf + " CASE WHEN APPROVAL = 'A' THEN '" & Color.MistyRose.Name & "' WHEN APPROVAL = 'R' THEN '" & Color.PaleTurquoise.Name & "' ELSE NULL END AS COLOR_HIDE,"
            strSql += vbCrLf + " (SELECT SIZENAME FROM " & cnAdminDb & "..ITEMSIZE WHERE SIZEID = T.SIZEID)AS SIZE"
            strSql += vbCrLf + " FROM"
            strSql += vbCrLf + " " & cnAdminDb & "..ITEMTAG AS T"
            strSql += vbCrLf + " WHERE ISNULL(APPROVAL,'') = '' "
            strSql += vbCrLf + " AND ISSDATE IS NULL"
            strSql += vbCrLf + " AND COMPANYID = '" & cnCompanyId & "'"
            strSql += vbCrLf + " AND COSTID = '" & cnCostId & "'"
            If Val(txtItemId_NUM.Text) <> 0 Then
                strSql += vbCrLf + " AND T.ITEMID = " & Val(txtItemId_NUM.Text) & ""
            End If
            strSql += " ORDER BY TAGNO"
            txtTagNo.Text = BrighttechPack.SearchDialog.Show("Find TagNo", strSql, cn, , , , , , , , False)
            txtTagNo.SelectAll()
        End If
    End Sub

    Private Sub LoadItemName()
        strSql = " SELECT"
        strSql += " ITEMID AS ITEMID,ITEMNAME AS ITEMNAME,"
        strSql += " CASE WHEN STOCKTYPE = 'T' THEN 'TAGGED' "
        strSql += " WHEN STOCKTYPE = 'N' THEN 'NON TAGGED' ELSE 'POCKET BASED' END AS STOCK_TYPE,"
        strSql += " CASE WHEN CALTYPE = 'W' THEN 'WEIGHT'"
        strSql += " WHEN CALTYPE = 'W' THEN 'WEIGHT'"
        strSql += " WHEN CALTYPE = 'R' THEN 'RATE'"
        strSql += " WHEN CALTYPE = 'F' THEN 'FIXED'"
        strSql += " WHEN CALTYPE = 'B' THEN 'BOTH' ELSE 'METAL RATE' END AS CAL_TYPE"
        strSql += " FROM " & cnAdminDb & "..ITEMMAST"
        strSql += " WHERE ISNULL(STUDDED,'') <> 'S' AND ACTIVE = 'Y'"
        Dim itemId As Integer = Val(BrighttechPack.SearchDialog.Show("Find ItemName", strSql, cn, 1, , , txtItemId_NUM.Text))
        If itemId > 0 Then
            txtItemId_NUM.Text = itemId
        Else
            txtItemId_NUM.Focus()
            txtItemId_NUM.SelectAll()
        End If
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Escape Then
            btnMark.Focus()
        End If
    End Sub

    Private Sub txtRunNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtRunNo.KeyPress

    End Sub
End Class