Imports System.Data.OleDb
Public Class frmNonTagIssueRecieptView
    Dim da As OleDbDataAdapter
    Dim strSql As String
    Dim dtCompany As New DataTable
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        ''Load ItemName
        gridView.BorderStyle = BorderStyle.None
        cmbItemName.Items.Clear()
        cmbItemName.Items.Add("ALL")
        strSql = " SELECT DISTINCT"
        strSql += " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST IM WHERE IM.ITEMID = IT.ITEMID " & GetItemQryFilteration("S") & " )AS ITEMNAME"
        strSql += " FROM " & cnAdminDb & "..ITEMNONTAG AS IT"
        objGPack.FillCombo(strSql, cmbItemName, False)

        Dim dt As New DataTable
        dt.Clear()
        ''Set CostCentre Status
        strSql = " Select 1 from " & cnAdminDb & "..SoftControl where ctlId = 'CostCentre' and ctlText = 'Y'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            cmbCostCentre.Enabled = True
            cmbCostCentre.Items.Clear()
            cmbCostCentre.Items.Add("ALL")
            strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            objGPack.FillCombo(strSql, cmbCostCentre, False)
        Else
            cmbCostCentre.Enabled = False
        End If


        cmbItemCounter.Items.Clear()
        cmbItemCounter.Items.Add("ALL")
        strSql = " SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER ORDER BY ITEMCTRNAME"
        objGPack.FillCombo(strSql, cmbItemCounter, False)
        cmbItemCounter.Text = "ALL"

        cmbType.Items.Clear()
        cmbType.Items.Add("RECIEPT")
        cmbType.Items.Add("ISSUE")
        cmbType.Items.Add("BOTH")
        cmbType.Text = "RECIEPT"
    End Sub
    Private Sub ClearSummary()
        txtRecPcs.Text = ""
        txtRecGrsWt.Text = ""
        txtRecNetWt.Text = ""

        txtIssPcs.Text = ""
        txtIssGrsWt.Text = ""
        txtIssNetWt.Text = ""

        txtBothPcs.Text = ""
        txtBothGrsWt.Text = ""
        txtBothNetWt.Text = ""
    End Sub
    Function funcNew() As Integer
        cmbItemName.Text = "ALL"
        ''Load Cost
        If cmbCostCentre.Enabled = True Then
            cmbCostCentre.Text = "ALL"
        End If
        cmbItemCounter.Text = "ALL"
        txtPacketNo.Clear()
        chkDate.Checked = True
        dtpFrom.Value = GetEntryDate(GetServerDate(tran), tran)
        dtpTo.Value = GetEntryDate(GetServerDate(tran), tran)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dtCompany, "COMPANYNAME", , IIf(cnCentStock, "ALL", strCompanyName))
        gridView.DataSource = Nothing
        ClearSummary()
        chkCmbCompany.Select()
    End Function
    Function funcExit() As Integer
        Me.Close()
    End Function

    Private Sub frmNonTagIssueRecieptView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub frmNonTagIssueRecieptView_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        strSql = " SELECT 'ALL' COMPANYNAME,'ALL' COMPANYID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COMPANYNAME,COMPANYID,2 RESULT FROM " & cnAdminDb & "..COMPANY "
        strSql += " ORDER BY RESULT,COMPANYNAME"
        dtCompany = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCompany)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dtCompany, "COMPANYNAME", , strCompanyName)
        funcNew()
    End Sub
    Private Sub chkDate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkDate.CheckedChanged
        If chkDate.Checked = True Then
            pnlDate.Enabled = True
        Else
            pnlDate.Enabled = False
        End If
    End Sub
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        btnSearch.Enabled = False
        Try
            ClearSummary()
            gridView.DataSource = Nothing
            Me.Refresh()
            strSql = " SELECT SNO,"
            'strSql += " CASE WHEN IT.RECISS = 'R' THEN LOTNO ELSE DREFNO END AS REFNO,"\
            strSql += " LOTNO AS LOTNO,"
            'strSql += " CONVERT(VARCHAR(12),IT.RECDATE,103)AS RECDATE,"
            strSql += " IT.RECDATE AS RECDATE,"
            strSql += " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST AS IM WHERE IM.ITEMID = IT.ITEMID)AS ITEM,"
            strSql += " ISNULL((SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST AS IM WHERE IM.SUBITEMID = IT.SUBITEMID),'')AS SUBITEM,"
            strSql += " PACKETNO,CASE WHEN IT.RECISS = 'R' THEN 'RECEIPT' ELSE 'ISSUE' END AS RECISS,"
            strSql += " CASE WHEN PCS <> 0 THEN PCS ELSE NULL END PCS,"
            strSql += " CASE WHEN GRSWT <> 0 THEN GRSWT ELSE NULL END GRSWT,"
            strSql += " CASE WHEN NETWT <> 0 THEN NETWT ELSE NULL END NETWT,"
            strSql += " CASE WHEN MC <> 0 THEN MC ELSE NULL END MC,"
            strSql += " CASE WHEN WASTAGE <> 0 THEN WASTAGE ELSE NULL END WASTAGE,"
            strSql += " CASE WHEN CTGRM = 'C' THEN 'CARAT'"
            strSql += " WHEN CTGRM = 'G' THEN 'GRAM' ELSE '' END AS CTGRM"
            strSql += " ,CASE WHEN RATE > 0 THEN RATE ELSE NULL END AS SALERATE"
            strSql += " ,CASE WHEN PURRATE > 0 THEN PURRATE ELSE NULL END AS PURRATE"
            strSql += " ,CASE WHEN ISSTYPE='SA' and ISNULL(BATCHNO,'')<>'' THEN ISNULL((SELECT REMARK1 FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO=IT.BATCHNO),'')"
            strSql += " ELSE NARRATION END NARRATION"
            strSql += " ,POSTED,BATCHNO,COSTID,LOTSNO"
            strSql += " ,ISNULL((SELECT DESIGNERNAME   FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID=IT.DESIGNERID),'') DESIGNER"
            strSql += " FROM " & cnAdminDb & "..ITEMNONTAG IT"
            strSql += " WHERE ISNULL(CANCEL,'')<>'Y'"
            If cmbItemName.Text <> "ALL" Then
                strSql += " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName.Text & "')"
            End If
            If cmbCostCentre.Enabled = True Then
                If cmbCostCentre.Text <> "ALL" Then
                    strSql += " AND COSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre.Text & "')"
                End If
            End If
            If cmbItemCounter.Text <> "ALL" Then
                strSql += " AND ITEMCTRID = (SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & cmbItemCounter.Text & "')"
            End If
            If txtPacketNo.Text <> "" Then
                strSql += " AND PACKETNO = '" & txtPacketNo.Text & "'"
            End If
            If chkDate.Checked = True Then
                strSql += " AND (RECDATE BETWEEN '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "')"
            End If
            If cmbType.Text <> "BOTH" Then
                strSql += " AND RECISS = '" & Mid(cmbType.Text, 1, 1) & "'"
            End If
            If chkCmbCompany.Text <> "ALL" And chkCmbCompany.Text <> "" Then
                strSql += " AND IT.COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(chkCmbCompany.Text) & "))"
            End If
            If txtLotNo.Text <> "" Then
                strSql += " AND LOTNO = '" & Val(txtLotNo.Text) & "'"
            End If
            'If Not cnCentStock Then strSql += " AND COMPANYID = '" & GetStockCompId() & "'"
            Dim dtGridView As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtGridView)
            gridView.DataSource = dtGridView
            With gridView
                With .Columns("LOTNO")
                    .Width = 65
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End With
                With .Columns("RECDATE")
                    .Width = 80
                    .DefaultCellStyle.Format = "dd/MM/yyyy"
                End With
                With .Columns("ITEM")
                    .Width = 200
                End With
                With .Columns("SUBITEM")
                    .Width = 200
                End With
                With .Columns("PACKETNO")
                    .HeaderText = "PACKET NO"
                    .Width = 65
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End With
                With .Columns("RECISS")
                    .Width = 70
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End With
                With .Columns("PCS")
                    .Width = 65
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End With
                With .Columns("GRSWT")
                    .Width = 75
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .DefaultCellStyle.Format = "0.000"
                End With
                With .Columns("NETWT")
                    .Width = 75
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .DefaultCellStyle.Format = "0.000"
                End With
                With .Columns("MC")
                    .Width = 75
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .DefaultCellStyle.Format = "0.00"
                End With
                With .Columns("WASTAGE")
                    .Width = 75
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .DefaultCellStyle.Format = "0.00"
                End With
                With .Columns("CTGRM")
                    .Width = 65
                End With
                .Columns("SALERATE").Width = 75
                .Columns("SALERATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("PURRATE").Width = 75
                .Columns("PURRATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("NARRATION").Width = 120
                .Columns("POSTED").Visible = False
                .Columns("BATCHNO").Visible = False
                .Columns("COSTID").Visible = False
                .Columns("LOTSNO").Visible = False
            End With
            strSql = " SELECT "
            strSql += " ISNULL(SUM(CASE WHEN RECISS = 'R' THEN PCS END),0)RECEIPTPCS,"
            strSql += " ISNULL(SUM(CASE WHEN RECISS = 'I' THEN PCS END),0)ISSUEPCS,"
            strSql += " ISNULL(SUM(CASE WHEN RECISS = 'R' THEN GRSWT END),0)RECEIPTGRSWT,"
            strSql += " ISNULL(SUM(CASE WHEN RECISS = 'I' THEN GRSWT END),0)ISSUEGRSWT,"
            strSql += " ISNULL(SUM(CASE WHEN RECISS = 'R' THEN NETWT END),0)RECEIPTNETWT,"
            strSql += " ISNULL(SUM(CASE WHEN RECISS = 'I' THEN NETWT END),0)ISSUENETWT"
            strSql += " FROM " & cnAdminDb & "..ITEMNONTAG"
            strSql += " WHERE 1=1"
            If cmbItemName.Text <> "ALL" Then
                strSql += " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName.Text & "')"
            End If
            If cmbCostCentre.Enabled = True Then
                If cmbCostCentre.Text <> "ALL" Then
                    strSql += " AND COSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre.Text & "')"
                End If
            End If
            If cmbItemCounter.Text <> "ALL" Then
                strSql += " AND ITEMCTRID = (SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & cmbItemCounter.Text & "')"
            End If
            If txtPacketNo.Text <> "" Then
                strSql += " AND PACKETNO = '" & txtPacketNo.Text & "'"
            End If
            If chkDate.Checked = True Then
                strSql += " AND (RECDATE BETWEEN '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "')"
            End If
            If cmbType.Text <> "BOTH" Then
                strSql += " AND RECISS = '" & Mid(cmbType.Text, 1, 1) & "'"
            End If
            If txtLotNo.Text <> "" Then
                strSql += " AND LOTNO = '" & Val(txtLotNo.Text) & "'"
            End If
            If Not cnCentStock Then strSql += " AND COMPANYID = '" & GetStockCompId() & "'"
            Dim dtSummary As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtSummary)
            If Not dtSummary.Rows.Count > 0 Then
                Exit Sub
            End If
            With dtSummary.Rows(0)
                txtRecPcs.Text = IIf(Val(.Item("RECEIPTPCS").ToString) <> 0, Val(.Item("RECEIPTPCS").ToString), "")
                txtRecGrsWt.Text = IIf(Val(.Item("RECEIPTGRSWT").ToString) <> 0, Format(Val(.Item("RECEIPTGRSWT").ToString), "0.000"), "")
                txtRecNetWt.Text = IIf(Val(.Item("RECEIPTNETWT").ToString) <> 0, Format(Val(.Item("RECEIPTNETWT").ToString), "0.000"), "")

                txtIssPcs.Text = IIf(Val(.Item("ISSUEPCS").ToString) <> 0, Val(.Item("ISSUEPCS").ToString), "")
                txtIssGrsWt.Text = IIf(Val(.Item("ISSUEGRSWT").ToString) <> 0, Format(Val(.Item("ISSUEGRSWT").ToString), "0.000"), "")
                txtIssNetWt.Text = IIf(Val(.Item("ISSUENETWT").ToString) <> 0, Format(Val(.Item("ISSUENETWT").ToString), "0.000"), "")
                Dim bothPcs As Integer = Val(.Item("RECEIPTPCS").ToString) - Val(.Item("ISSUEPCS").ToString)
                Dim bothGrsWt As Double = Val(.Item("RECEIPTGRSWT").ToString) - Val(.Item("ISSUEGRSWT").ToString)
                Dim bothNetWt As Double = Val(.Item("RECEIPTNETWT").ToString) - Val(.Item("ISSUENETWT").ToString)

                txtBothPcs.Text = IIf(bothPcs <> 0, bothPcs, "")
                txtBothGrsWt.Text = IIf(bothGrsWt <> 0, Format(bothGrsWt, "0.000"), "")
                txtBothNetWt.Text = IIf(bothNetWt <> 0, Format(bothNetWt, "0.000"), "")
            End With
            gridView.Select()
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            btnSearch.Enabled = True
        End Try
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        funcNew()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        funcExit()
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then e.Handled = True
        If e.KeyCode = Keys.E Then
            If gridView.CurrentRow.Cells("RECISS").Value.ToString <> "ISSUE" Then
                If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Edit) Then Exit Sub
                Dim obj As New frmNonTagReceipt(gridView.Rows(gridView.CurrentRow.Index).Cells("SNO").Value.ToString)
                obj.ShowDialog()
            End If
        ElseIf e.KeyCode = Keys.D Then
            Dim drpkt As DataRow
            strSql = " select itemid,packetno FROM " & cnAdminDb & "..ITEMNONTAG WHERE SNO = '" & gridView.CurrentRow.Cells("SNO").Value.ToString & "'"
            drpkt = GetSqlRow(strSql, cn)
            If Val(drpkt.Item(1).ToString) <> 0 And GetAdmindbSoftValue("PKTNOTAGPRINT", "N") = "Y" Then
                Dim prnmemsuffix As String = ""
                If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId
                Dim oldItem As Integer = Nothing
                Dim memfile As String = "\Barcodeprint" & prnmemsuffix.Trim & ".mem"
                Dim write As IO.StreamWriter
                write = IO.File.CreateText(Application.StartupPath & memfile)
                write.WriteLine(LSet("PROC", 7) & ":" & drpkt.Item(0).ToString)
                write.WriteLine(LSet("PKTNO", 7) & ":" & drpkt.Item(1).ToString)
                write.Flush()
                write.Close()
                If IO.File.Exists(Application.StartupPath & "\PrjBarcodePrint.exe") Then
                    System.Diagnostics.Process.Start(Application.StartupPath & "\PrjBarcodePrint.exe")
                Else
                    MsgBox("Barcode print exe not found", MsgBoxStyle.Information)
                End If
            End If
            'End If
        ElseIf e.KeyCode = Keys.Delete Then
            If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
            If gridView.CurrentRow.Cells("BATCHNO").Value.ToString <> "" Then
                MsgBox("You cannot delete this entry", MsgBoxStyle.Information)
                Exit Sub
            End If
            Dim objSecret As New frmAdminPassword()
            If objSecret.ShowDialog <> Windows.Forms.DialogResult.OK Then
                Exit Sub
            End If
            Try
                tran = Nothing
                tran = cn.BeginTransaction
                strSql = " UPDATE " & cnAdminDb & "..ITEMLOT SET CPCS = CPCS - " & Val(gridView.CurrentRow.Cells("PCS").Value.ToString) & ""
                strSql += " ,CGRSWT = CGRSWT - " & Val(gridView.CurrentRow.Cells("GRSWT").Value.ToString) & ""
                strSql += " ,CNETWT = CNETWT - " & Val(gridView.CurrentRow.Cells("NETWT").Value.ToString) & ""
                strSql += " WHERE SNO = '" & gridView.CurrentRow.Cells("LOTSNO").Value.ToString & "'"
                ExecQuery(SyncMode.Stock, strSql, cn, tran, gridView.CurrentRow.Cells("COSTID").Value.ToString)
                strSql = " DELETE FROM " & cnAdminDb & "..ITEMNONTAG "
                strSql += " WHERE SNO = '" & gridView.CurrentRow.Cells("SNO").Value.ToString & "'"
                ExecQuery(SyncMode.Stock, strSql, cn, tran, gridView.CurrentRow.Cells("COSTID").Value.ToString)
                tran.Commit()
                tran = Nothing
                MsgBox("Successfully Deleted", MsgBoxStyle.Information)
                btnSearch_Click(Me, New EventArgs)
            Catch ex As Exception
                If Not tran Is Nothing Then tran.Rollback()
                MsgBox(ex.Message + vbCrLf + ex.StackTrace)
            End Try
        End If
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "NonTag Issue-Receipt View", gridView, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Function funcGetDetails(ByVal tempMetalId As String, ByVal RecDate As String)

        strSql = " SELECT SNO,RECDATE,PCS,GRSWT,LESSWT,NETWT,FINRATE,ISSTYPE,RECISS,LOTNO,PACKETNO,DREFNO,ORDREPNO,NARRATION,"
        strSql += " PURWASTAGE,PURRATE,PURMC,RATE,CTGRM,MC,WASTPER,WASTAGE,MCPERGRM,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID) AS ITEMNAME,"
        strSql += " (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = I.SUBITEMID) AS SUBITEM,"
        strSql += " (SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = I.ITEMCTRID) AS ITEMCTRNAME,"
        strSql += " (SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = I.COSTID) AS COSTNAME,"
        strSql += " (SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = I.DESIGNERID) AS DESIGNER,"
        strSql += " ISNULL((SELECT NAME FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = I.ITEMTYPEID),'') AS TYPENAME"
        strSql += " FROM " & cnAdminDb & "..ITEMNONTAG I "
        strSql += " WHERE RECISS = 'R' AND LOTNO = '" & tempMetalId & "' AND RECDATE = '" & RecDate & "'"
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            With dt.Rows(0)
                frmNonTagReceipt.txtLotNo_Num_Man.Text = .Item("LOTNO").ToString
                frmNonTagReceipt.dtpTranDate.Value = .Item("RECDATE").ToString
                frmNonTagReceipt.txtPieces_Num.Text = .Item("PCS").ToString
                frmNonTagReceipt.txtWeight_Wet.Text = .Item("GRSWT").ToString
                frmNonTagReceipt.txtLessWt.Text = .Item("LESSWT").ToString
                frmNonTagReceipt.txtNetWeight_Wet.Text = .Item("NETWT").ToString
                frmNonTagReceipt.txtMcPerGrm_AMT.Text = .Item("MCPERGRM").ToString
                frmNonTagReceipt.txtMc_AMT.Text = .Item("MC").ToString
                frmNonTagReceipt.txtSaleRate_Amt.Text = .Item("Rate").ToString
                frmNonTagReceipt.txtWastagePer_PER.Text = .Item("WASTPER").ToString
                frmNonTagReceipt.txtWastage_WET.Text = .Item("WASTAGE").ToString
                frmNonTagReceipt.txtMetalRate_Amt.Text = .Item("FINRATE").ToString
                frmNonTagReceipt.txtPacketNo__Man.Text = .Item("PACKETNO").ToString
                frmNonTagReceipt.txtItem_Man.Text = .Item("ITEMNAME").ToString
                frmNonTagReceipt.cmbSubItem_Man.Text = .Item("SUBITEM").ToString
                frmNonTagReceipt.cmbNarration_OWN.Text = .Item("NARRATION").ToString
                frmNonTagReceipt.cmbItemType_MAN.Text = .Item("TYPENAME").ToString
                frmNonTagReceipt.cmbCounter_MAN.Text = .Item("ITEMCTRNAME").ToString
                frmNonTagReceipt.cmbCostCentre_MAN.Text = .Item("COSTNAME").ToString
                frmNonTagReceipt.cmbDesigner_MAN.Text = .Item("DESIGNER").ToString

            End With
        End If

        'flagSave = True
        frmNonTagReceipt.txtLotNo_Num_Man.Enabled = False
        frmNonTagReceipt.dtpTranDate.Enabled = False
        frmNonTagReceipt.txtItem_Man.Enabled = False
        frmNonTagReceipt.txtMetalRate_Amt.Enabled = False
        frmNonTagReceipt.txtPacketNo__Man.Enabled = False



        Return 0
    End Function

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "NonTag Issue-Receipt View", gridView, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub
End Class