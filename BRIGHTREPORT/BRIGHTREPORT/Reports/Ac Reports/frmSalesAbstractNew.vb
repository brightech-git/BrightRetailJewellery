Imports System.Data.OleDb
Imports System.Threading
Imports System.IO
Public Class frmSalesAbstractNew
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim dsGridView As New DataSet
    Dim strFtr As String
    Dim strFtr1 As String
    Dim strFtr2 As String
    Dim SepStnPost As String
    Dim SepDiaPost As String
    Dim SepPrePost As String
    Dim dtCostCentre As New DataTable

    Dim strprint As String
    Dim FileWrite As StreamWriter
    Dim PgNo As Integer
    Dim line As Integer
    Dim SpecificPrint As Boolean = False
    Dim NewFormat As Boolean = IIf(GetAdmindbSoftValue("RPT_SALPUR_TALLY", "N") = "Y", True, False)
    Dim VOUTYPE As Boolean = IIf(GetAdmindbSoftValue("RPT_TSP_VOUTYPE", "N") = "Y", True, False)
    Dim COSTVOUTYPE As Boolean = IIf(GetAdmindbSoftValue("RPT_SALPUR_COSTCENTREPREFIX", "N") = "Y", True, False)
    Dim SalPur_RefNo As Boolean = IIf(GetAdmindbSoftValue("RPT_SALPUR_TALLY_REFNO", "N") = "Y", True, False)

    Function funcGetValues(ByVal field As String, ByVal def As String) As String
        strSql = " Select ctlText from " & cnAdminDb & "..SoftControl"
        strSql += vbCrLf + " where ctlId = '" & field & "'"
        Dim dt As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            Return dt.Rows(0).Item("ctlText").ToString
        End If
        Return def
    End Function
    Private Sub SalesAbs()
        Try
            Dim GstView As Boolean = funcGstView(dtpFrom.Value)
            Dim selCatCode As String = Nothing
            If cmbCategory.Text = "ALL" Then
                selCatCode = "ALL"
            ElseIf cmbMetal.Text <> "" Then
                Dim sql As String = "SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & GetQryString(cmbCategory.Text) & ")"
                Dim dtCat As New DataTable()
                da = New OleDbDataAdapter(sql, cn)
                da.Fill(dtCat)
                If dtCat.Rows.Count > 0 Then
                    For i As Integer = 0 To dtCat.Rows.Count - 1
                        selCatCode += dtCat.Rows(i).Item("CATCODE").ToString + ","
                    Next
                    If selCatCode <> "" Then
                        selCatCode = Mid(selCatCode, 1, selCatCode.Length - 1)
                    End If
                End If
            End If
            Prop_Sets()
            gridView.DataSource = Nothing
            Me.Refresh()
            If NewFormat Then
                If GstView Then
                    strSql = " EXEC " & cnStockDb & "..PROC_RPT_ABSTRACT_NEW_FORMAT_GST"
                Else
                    strSql = " EXEC " & cnStockDb & "..PROC_RPT_ABSTRACT_NEW_FORMAT1"
                End If
                strSql += vbCrLf + " @DBNAME = '" & cnAdminDb & "'"
            Else
                strSql = " EXEC " & cnStockDb & "..SP_RPT_ABSTRACT_NEW"
                strSql += vbCrLf + " @DBNAME = '" & cnAdminDb & "'"
            End If
            strSql += vbCrLf + " ,@SYSTEMID = '" & systemId & "'"
            strSql += vbCrLf + " ,@FRMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@METALNAME = '" & cmbMetal.Text & "'"
            strSql += vbCrLf + " ,@CATCODE = '" & selCatCode & "'"
            strSql += vbCrLf + " ,@CATNAME = '" & cmbCategory.Text & "'"
            strSql += vbCrLf + " ,@COSTNAME = '" & GetQryString(chkCmbCostCentre.Text).Replace("'", "") & "'"
            strSql += vbCrLf + " ,@COMPANYID = '" & strCompanyId & "'"
            strSql += vbCrLf + " ,@TYPE = '" & cmbTrantype.Text & "'"
            If NewFormat Then
                strSql += vbCrLf + " ,@SEPDIA = '" & IIf(chkSepDia.Checked, "Y", "N") & "'"
            End If
            strSql += vbCrLf + " ,@BILLSUM = '" & IIf(chkBillWise.Checked, "Y", "N") & "'"
            cmd = New OleDbCommand(strSql, cn)
            cmd.CommandTimeout = 1000
            da = New OleDbDataAdapter(cmd)
            Dim ds As New DataSet
            Dim dtGrid As New DataTable
            da.Fill(ds)
            dtGrid = ds.Tables(0)
            If Not dtGrid.Rows.Count > 0 Then
                MsgBox("Record Not Found", MsgBoxStyle.Information)
                Exit Sub
            End If
            If chkSepDia.Checked = True And COSTVOUTYPE = False Then
                If VOUTYPE = True Then
                    ''BMJ 
                    strSql = vbCrLf + " ALTER TABLE TEMPTABLEDB..TEMP" & systemId & "TEMPSALESABSFINAL ADD VT VARCHAR(25)"
                    cmd = New OleDbCommand(strSql, cn)
                    cmd.ExecuteNonQuery()
                    strSql = vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "TEMPSALESABSFINAL SET VT=VOUCHERTYPE"
                    cmd = New OleDbCommand(strSql, cn)
                    cmd.ExecuteNonQuery()
                    For I As Integer = 0 To dtGrid.Rows.Count - 1
                        Dim INV As String = dtGrid.Rows(I).Item("INVOICENO").ToString
                        Dim XITEM As Integer

                        strSql = vbCrLf + " SELECT ISNULL(ITEMID,0) FROM " & cnAdminDb & "..SUBITEMMAST "
                        strSql += vbCrLf + " WHERE SUBITEMNAME IN(SELECT  PRODUCTNAME FROM TEMPTABLEDB..TEMP" & systemId & "TEMPSALESABSFINAL WHERE BATCHNO='" & dtGrid.Rows(I).Item("BATCHNO").ToString & "' AND SNO='" & dtGrid.Rows(I).Item("SNO").ToString & "'  )"
                        XITEM = GetSqlValue(cn, strSql)
                        If XITEM = 0 Then
                            strSql = vbCrLf + " SELECT ISNULL(ITEMID,0) FROM " & cnAdminDb & "..ITEMMAST "
                            strSql += vbCrLf + "  WHERE ITEMNAME IN(SELECT PRODUCTNAME FROM TEMPTABLEDB..TEMP" & systemId & "TEMPSALESABSFINAL WHERE BATCHNO='" & dtGrid.Rows(I).Item("BATCHNO").ToString & "' AND SNO='" & dtGrid.Rows(I).Item("SNO").ToString & "'   )"
                            XITEM = GetSqlValue(cn, strSql)
                        End If
                        If XITEM = 0 Then Continue For
                        strSql = vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "TEMPSALESABSFINAL "
                        strSql += vbCrLf + " SET VT =(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID IN "
                        strSql += vbCrLf + " (SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID='" & XITEM & "')) WHERE PRODUCTNAME <>'ORDER ITEM' AND BATCHNO='" & dtGrid.Rows(I).Item("BATCHNO").ToString & "' AND SNO='" & dtGrid.Rows(I).Item("SNO").ToString & "'  "
                        cmd = New OleDbCommand(strSql, cn)
                        cmd.ExecuteNonQuery()

                        'strSql = "  DECLARE @ITEM INT"
                        'strSql += vbCrLf + " SELECT @ITEM=ISNULL(ITEMID,0) FROM " & cnAdminDb & "..SUBITEMMAST "
                        'strSql += vbCrLf + " WHERE SUBITEMNAME IN(SELECT  PRODUCTNAME FROM TEMPTABLEDB..TEMP" & systemId & "TEMPSALESABSFINAL WHERE BATCHNO='" & dtGrid.Rows(I).Item("BATCHNO").ToString & "' AND SNO='" & dtGrid.Rows(I).Item("SNO").ToString & "'  )"
                        'strSql += vbCrLf + " IF @ITEM=0"
                        'strSql += vbCrLf + " BEGIN"
                        'strSql += vbCrLf + " SELECT @ITEM=ISNULL(ITEMID,0) FROM " & cnAdminDb & "..ITEMMAST "
                        'strSql += vbCrLf + "  WHERE ITEMNAME IN(SELECT PRODUCTNAME FROM TEMPTABLEDB..TEMP" & systemId & "TEMPSALESABSFINAL WHERE BATCHNO='" & dtGrid.Rows(I).Item("BATCHNO").ToString & "' AND SNO='" & dtGrid.Rows(I).Item("SNO").ToString & "'   )"
                        'strSql += vbCrLf + " End"
                        'strSql += vbCrLf + " IF  @ITEM<>0 "
                        'strSql += vbCrLf + " BEGIN"
                        'strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "TEMPSALESABSFINAL "
                        'strSql += vbCrLf + " SET VOUCHERTYPE =(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID IN "
                        'strSql += vbCrLf + " (SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=@ITEM)) WHERE PRODUCTNAME <>'ORDER ITEM' AND BATCHNO='" & dtGrid.Rows(I).Item("BATCHNO").ToString & "' AND SNO='" & dtGrid.Rows(I).Item("SNO").ToString & "'  "
                        'strSql += vbCrLf + " End"

                    Next
                    strSql = "SELECT  INVOICEDATE,INVOICENO,"
                    strSql += vbCrLf + " METALTYPE,VT,PARTYNAME,PRODUCTNAME,CATNAME,GRSWT,NETWT,"
                    strSql += vbCrLf + " STNWT,STNAMT,DIAWT,DIAAMT,ROUND(VALUE,0)VALUE"
                    'strSql += vbCrLf + " ,SALESTAX,ROUND(SALESTAXAMT,0)SALESTAXAMT'"
                    strSql += vbCrLf + " ,GST,IGSTRATE,IGSTTAX"
                    strSql += vbCrLf + " ,CGSTRATE,CGSTTAX"
                    strSql += vbCrLf + " ,SGSTRATE,SGSTTAX"
                    strSql += vbCrLf + " ,ROUND(TOTAL,0)TOTAL,BATCHNO,"
                    strSql += vbCrLf + " TRANTYPE, REMARKS, CASH, CARD, CHEQUE, ADVADJ, CREDIT, SNO "
                    strSql += vbCrLf + " ,ROUND(ISNULL(CASH,0)+ISNULL(CARD,0)+ISNULL(CHEQUE,0)+ISNULL(ADVADJ,0)+ISNULL(CREDIT,0),0) AS BAL_AMT    "
                    If SalPur_RefNo Then
                        strSql += vbCrLf + " ,REFNO"
                    End If
                    strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "TEMPSALESABSFINAL WHERE 1=1    "
                    If cmbMetal.Text <> "ALL" Then strSql += vbCrLf + "  AND METALTYPE='" & cmbMetal.Text & "'   "
                    If cmbTrantype.Text <> "ALL" Then strSql += vbCrLf + " AND VOUCHERTYPE='" & cmbTrantype.Text & "'  "
                    strSql += vbCrLf + " ORDER BY INVOICEDATE,CASE WHEN VOUCHERTYPE='SALES RETURN' THEN 'RETURN' ELSE VOUCHERTYPE END DESC,INVOICENO,REMARKS  "
                Else
                    strSql = "SELECT INVOICEDATE,INVOICENO,"
                    strSql += vbCrLf + " METALTYPE,VOUCHERTYPE,PARTYNAME,PRODUCTNAME,GRSWT,NETWT,"
                    strSql += vbCrLf + " STNWT,STNAMT,DIAWT,DIAAMT,ROUND(VALUE,0)VALUE"
                    'strSql += vbCrLf + " ,SALESTAX,ROUND(SALESTAXAMT,0)SALESTAXAMT"
                    strSql += vbCrLf + " ,GST,IGSTRATE,IGSTTAX"
                    strSql += vbCrLf + " ,CGSTRATE,CGSTTAX"
                    strSql += vbCrLf + " ,SGSTRATE,SGSTTAX"
                    strSql += vbCrLf + " ,ROUND(TOTAL,0)TOTAL,BATCHNO,"
                    strSql += vbCrLf + " TRANTYPE, REMARKS, CASH, CARD, CHEQUE, ADVADJ, CREDIT, SNO "
                    strSql += vbCrLf + " ,ROUND(ISNULL(CASH,0)+ISNULL(CARD,0)+ISNULL(CHEQUE,0)+ISNULL(ADVADJ,0)+ISNULL(CREDIT,0),0) AS BAL_AMT    "
                    If SalPur_RefNo Then
                        strSql += vbCrLf + " ,REFNO"
                    End If
                    strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "TEMPSALESABSFINAL WHERE 1=1    "
                    If cmbMetal.Text <> "ALL" Then strSql += vbCrLf + "  AND METALTYPE='" & cmbMetal.Text & "'   "
                    If cmbTrantype.Text <> "ALL" Then strSql += vbCrLf + " AND VOUCHERTYPE='" & cmbTrantype.Text & "'  "
                    strSql += vbCrLf + " ORDER BY INVOICEDATE,CASE WHEN VOUCHERTYPE='SALES RETURN' THEN 'RETURN' ELSE VOUCHERTYPE END DESC,INVOICENO,REMARKS  "
                End If
                cmd = New OleDbCommand(strSql, cn)
                cmd.CommandTimeout = 1000
                da = New OleDbDataAdapter(cmd)
                ds = New DataSet
                dtGrid = New DataTable
                da.Fill(ds)
                dtGrid = ds.Tables(0)
            ElseIf chkSepDia.Checked = True And COSTVOUTYPE = True Then
                ''BMJ 
                strSql = vbCrLf + " ALTER TABLE TEMPTABLEDB..TEMP" & systemId & "TEMPSALESABSFINAL ADD VT VARCHAR(50)"
                cmd = New OleDbCommand(strSql, cn)
                cmd.ExecuteNonQuery()

                strSql = vbCrLf + " ALTER TABLE TEMPTABLEDB..TEMP" & systemId & "TEMPSALESABSFINAL ADD COSTNAME VARCHAR(50)"
                cmd = New OleDbCommand(strSql, cn)
                cmd.ExecuteNonQuery()

                strSql = vbCrLf + " ALTER TABLE TEMPTABLEDB..TEMP" & systemId & "TEMPSALESABSFINAL ADD INVNO VARCHAR(50)"
                cmd = New OleDbCommand(strSql, cn)
                cmd.ExecuteNonQuery()

                strSql = vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "TEMPSALESABSFINAL SET VT=VOUCHERTYPE"
                cmd = New OleDbCommand(strSql, cn)
                cmd.ExecuteNonQuery()
                For I As Integer = 0 To dtGrid.Rows.Count - 1
                    Dim INV As String = dtGrid.Rows(I).Item("INVOICENO").ToString

                    strSql = vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "TEMPSALESABSFINAL "
                    strSql += vbCrLf + " SET VT =(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID ='" & dtGrid.Rows(I).Item("COSTID").ToString & "') "
                    strSql += vbCrLf + " ,INVNO = COSTID +'-'+ LTRIM(INVOICENO) "
                    strSql += vbCrLf + " ,COSTNAME =(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID ='" & dtGrid.Rows(I).Item("COSTID").ToString & "') "
                    strSql += vbCrLf + "  WHERE PRODUCTNAME <>'ORDER ITEM' AND BATCHNO='" & dtGrid.Rows(I).Item("BATCHNO").ToString & "' AND SNO='" & dtGrid.Rows(I).Item("SNO").ToString & "'  "
                    cmd = New OleDbCommand(strSql, cn)
                    cmd.ExecuteNonQuery()
                    strSql = vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "TEMPSALESABSFINAL "
                    strSql += vbCrLf + " SET "
                    strSql += vbCrLf + " INVNO = COSTID +'-'+ LTRIM(INVOICENO) "
                    strSql += vbCrLf + " ,COSTNAME =(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID ='" & dtGrid.Rows(I).Item("COSTID").ToString & "') "
                    strSql += vbCrLf + "  WHERE PRODUCTNAME ='ORDER ITEM' AND BATCHNO='" & dtGrid.Rows(I).Item("BATCHNO").ToString & "' AND SNO='" & dtGrid.Rows(I).Item("SNO").ToString & "'  "
                    cmd = New OleDbCommand(strSql, cn)
                    cmd.ExecuteNonQuery()
                Next
                strSql = "SELECT  INVOICEDATE,INVNO INVOICENO"

                strSql += vbCrLf + " ,METALTYPE,VT VOUCHERTYPE,PARTYNAME,PRODUCTNAME,CATNAME,GRSWT,NETWT"
                If chkSepDia.Checked = True And chkSepDia.Enabled = True Then
                    strSql += vbCrLf + " ,STNWT,STNAMT,DIAWT,DIAAMT"
                End If
                'strSql += vbCrLf + " ,SALESTAX,ROUND(SALESTAXAMT,0)SALESTAXAMT'"
                strSql += vbCrLf + " ,ROUND(VALUE,0)VALUE,GST,IGSTRATE,IGSTTAX"
                strSql += vbCrLf + " ,CGSTRATE,CGSTTAX"
                strSql += vbCrLf + " ,SGSTRATE,SGSTTAX"
                strSql += vbCrLf + " ,ROUND(TOTAL,0)TOTAL,BATCHNO,"
                strSql += vbCrLf + " TRANTYPE, REMARKS, CASH, CARD, CHEQUE, ADVADJ, CREDIT, SNO "
                strSql += vbCrLf + " ,ROUND(ISNULL(CASH,0)+ISNULL(CARD,0)+ISNULL(CHEQUE,0)+ISNULL(ADVADJ,0)+ISNULL(CREDIT,0),0) AS BAL_AMT,COSTNAME    "
                If SalPur_RefNo Then
                    strSql += vbCrLf + " ,REFNO"
                End If
                strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "TEMPSALESABSFINAL WHERE 1=1    "
                If cmbMetal.Text <> "ALL" Then strSql += vbCrLf + "  AND METALTYPE='" & cmbMetal.Text & "'   "
                If cmbTrantype.Text <> "ALL" Then strSql += vbCrLf + " AND VOUCHERTYPE='" & cmbTrantype.Text & "'  "
                strSql += vbCrLf + " ORDER BY INVOICEDATE,CASE WHEN VOUCHERTYPE='SALES RETURN' THEN 'RETURN' ELSE VOUCHERTYPE END DESC,INVOICENO,REMARKS  "
                cmd = New OleDbCommand(strSql, cn)
                cmd.CommandTimeout = 1000
                da = New OleDbDataAdapter(cmd)
                ds = New DataSet
                dtGrid = New DataTable
                da.Fill(ds)
                dtGrid = ds.Tables(0)
            Else
                If dtGrid.Columns.Contains("CATNAME") = True Then dtGrid.Columns.Remove("CATNAME")
            End If
            If dtGrid.Columns.Contains("BATCHNO") = True Then dtGrid.Columns.Remove("BATCHNO")
            If dtGrid.Columns.Contains("TRANTYPE") = True Then dtGrid.Columns.Remove("TRANTYPE")
            If dtGrid.Columns.Contains("COSTID") = True Then dtGrid.Columns.Remove("COSTID")
            If dtGrid.Columns.Contains("TSNO") = True Then dtGrid.Columns.Remove("TSNO")
            gridView.DataSource = dtGrid
            With gridView
                If chkSepDia.Checked = True And chkSepDia.Enabled = True Then
                    If VOUTYPE = True Then .Columns("VT").HeaderText = "VOUCHERTYPE "
                End If
                If .Columns.Contains("INVOICENO") Then .Columns("INVOICENO").Width = 120
                If .Columns.Contains("QTY") Then .Columns("QTY").Width = 60
                If .Columns.Contains("PARTYNAME") Then .Columns("PARTYNAME").Width = 120
                If .Columns.Contains("ADDRESS1") Then .Columns("ADDRESS1").Width = 150
                If .Columns.Contains("PRODUCTNAME") Then .Columns("PRODUCTNAME").Width = 150
                If .Columns.Contains("CONTACTNO") Then .Columns("CONTACTNO").Width = 100
                If .Columns.Contains("SALESTAXAMT") Then .Columns("SALESTAXAMT").Width = 80
                If .Columns.Contains("TOTAL") Then .Columns("TOTAL").Width = 100
                If .Columns.Contains("BAL_AMT") Then .Columns("BAL_AMT").HeaderText = "TOTAL"
                If .Columns.Contains("CATNAME") Then .Columns("CATNAME").HeaderText = "CATEGORY"
                If .Columns.Contains("SNO") Then .Columns("SNO").Visible = False
                If .Columns.Contains("INVOICEDATE") Then .Columns("INVOICEDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
                If .Columns.Contains("REMARKS") Then .Columns("REMARKS").Width = 150
                If .Columns.Contains("REMARKS") Then
                    For Each gv As DataGridViewRow In gridView.Rows
                        With gv
                            Select Case .Cells("REMARKS").Value.ToString
                                Case "STUDDED DIAMOND"
                                    .DefaultCellStyle.BackColor = Color.LightGreen
                            End Select
                        End With
                    Next
                End If
                If .Columns.Contains("STAXAMOUNT") Then .Columns("STAXAMOUNT").DefaultCellStyle.Format = "0.00"
                If .Columns.Contains("STAXAMOUNT") Then .Columns("STAXAMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                If .Columns.Contains("STAXPER") Then .Columns("STAXPER").DefaultCellStyle.Format = "0.00"
                If .Columns.Contains("STAXPER") Then .Columns("STAXPER").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                If .Columns.Contains("IGSTRATE") Then .Columns("IGSTRATE").DefaultCellStyle.Format = "0.00"
                If .Columns.Contains("IGSTTAX") Then .Columns("IGSTTAX").DefaultCellStyle.Format = "0.00"
                If .Columns.Contains("CGSTRATE") Then .Columns("CGSTRATE").DefaultCellStyle.Format = "0.00"
                If .Columns.Contains("CGSTTAX") Then .Columns("CGSTTAX").DefaultCellStyle.Format = "0.00"
                If .Columns.Contains("SGSTRATE") Then .Columns("SGSTRATE").DefaultCellStyle.Format = "0.00"
                If .Columns.Contains("SGSTTAX") Then .Columns("SGSTTAX").DefaultCellStyle.Format = "0.00"
                If .Columns.Contains("GST") Then .Columns("GST").DefaultCellStyle.Format = "0.00"
                If .Columns.Contains("IGSTRATE") Then .Columns("IGSTRATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                If .Columns.Contains("IGSTTAX") Then .Columns("IGSTTAX").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                If .Columns.Contains("CGSTRATE") Then .Columns("CGSTRATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                If .Columns.Contains("CGSTTAX") Then .Columns("CGSTTAX").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                If .Columns.Contains("SGSTRATE") Then .Columns("SGSTRATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                If .Columns.Contains("SGSTTAX") Then .Columns("SGSTTAX").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                If .Columns.Contains("GST") Then .Columns("GST").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            FormatGridColumns(gridView, False, False, True, False)
            btnView_Search.Enabled = True
            With gridView
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                .Invalidate()
                For Each dgvCol As DataGridViewColumn In .Columns
                    dgvCol.Width = dgvCol.Width
                Next
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
                .Focus()
            End With
        Catch ex As Exception
            MsgBox(ex.Message) : Exit Sub
        End Try
    End Sub

    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        'pnlHeading.Visible = False
        SalesAbs()
        Exit Sub
    End Sub

    Private Sub frmSalesAbstractNew_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmSalesAbstractNew_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        cmbMetal.Items.Clear()
        cmbMetal.Items.Add("ALL")
        strSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST ORDER BY METALNAME"
        objGPack.FillCombo(strSql, cmbMetal, False)
        cmbMetal.Text = "ALL"
        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False
        lblCtrlId.Text = "RPT_SALPUR_TALLY:" & IIf(NewFormat, "Y", "N")
        Dim GstView As Boolean = funcGstView(dtpFrom.Value)
        If NewFormat Then
            cmbTrantype.Visible = True
            Label6.Visible = True
            cmbTrantype.Items.Clear()
            cmbTrantype.Items.Add("ALL")
            cmbTrantype.Items.Add("SALES")
            If GstView Then
                cmbTrantype.Items.Add("SALES RETURN")
            End If
            cmbTrantype.Items.Add("PURCHASE")
            cmbTrantype.Items.Add("APRADV-R")
            cmbTrantype.Items.Add("APRADV-P")
            cmbTrantype.Items.Add("ORDADV-R")
            cmbTrantype.Items.Add("ORDADV-P")
            cmbTrantype.SelectedIndex = 0
            chkSepDia.Visible = True
        Else
            cmbTrantype.Visible = True
            Label6.Visible = True
            cmbTrantype.Items.Clear()
            cmbTrantype.Items.Add("ALL")
            cmbTrantype.Items.Add("SALES")
            cmbTrantype.Items.Add("PURCHASE")
            cmbTrantype.Items.Add("REPAIR DELIVERY")
            cmbTrantype.SelectedIndex = 0
            chkSepDia.Visible = False
        End If
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        Me.btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ExtToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExtToolStripMenuItem.Click
        Me.btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        'pnlHeading.Visible = False
        gridView.DataSource = Nothing
        btnView_Search.Enabled = True
        Prop_Sets()
        dtpFrom.Focus()
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        'Me.Cursor = Cursors.WaitCursor
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "", gridView, BrightPosting.GExport.GExportType.Export, , , , , "dd/MM/yyyy")
        End If
        Me.Cursor = Cursors.Arrow
    End Sub
    Function GridViewFormat() As Integer
        For Each dgvView As DataGridViewRow In gridView.Rows
            With dgvView
                Select Case .Cells("RESULT").Value.ToString
                    Case 1
                        .Cells("PARTICULARS").Style.BackColor = reportHeadStyle.BackColor
                        .Cells("PARTICULARS").Style.Font = reportHeadStyle.Font
                End Select
            End With
        Next
    End Function
    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = "X" Or e.KeyChar = "x" Then
            Me.btnExcel_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = "P" Then
            btnPrint_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "", gridView, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub cmbMetal_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbMetal.SelectedIndexChanged
        If cmbMetal.Text <> "" Then
            strSql = vbCrLf + " SELECT 'ALL' CATNAME,'ALL' CATCODE,1 RESULT"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT CATNAME,CONVERT(VARCHAR,CATCODE),2 RESULT FROM " & cnAdminDb & "..CATEGORY"
            strSql += vbCrLf + " ORDER BY RESULT,CATNAME"
            da = New OleDbDataAdapter(strSql, cn)
            Dim dtCat As New DataTable()
            da.Fill(dtCat)
            cmbCategory.Items.Clear()
            BrighttechPack.GlobalMethods.FillCombo(cmbCategory, dtCat, "CATNAME", , "ALL")
        End If
    End Sub

    Private Sub dtpFrom_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        dtpTo.MinimumDate = dtpFrom.Value
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New frmSalesAbstract_Properties

        'GetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre)
        obj.p_cmbMetal = cmbMetal.Text
        obj.p_cmbCategory = cmbCategory.Text
        SetSettingsObj(obj, Me.Name, GetType(frmSalesAbstract_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmSalesAbstract_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmSalesAbstract_Properties))
        'SetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre, "ALL")
        cmbMetal.Text = obj.p_cmbMetal
        cmbCategory.Text = obj.p_cmbCategory
    End Sub

    Private Sub btn_dPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If gridView.Rows.Count > 0 Then
            'DetailPrint()
        End If
    End Sub

    'Function DetailPrint()
    '    Dim CompanyName, Address1, Address2, Address3, Phone As String
    '    Dim dtprint As New DataTable
    '    Dim i As Integer
    '    Dim dt As New DataTable
    '    Dim mremark As String
    '    Dim mode As String
    '    Dim dateflag As Boolean = False

    '    dtprint.Clear()
    '    dtprint = gridView.DataSource

    '    strSql = "SELECT COMPANYNAME,ADDRESS1,ADDRESS2,ADDRESS3,ADDRESS4 FROM " & cnAdminDb & "..COMPANY"
    '    da = New OleDbDataAdapter(strSql, cn)
    '    da.Fill(dt)

    '    CompanyName = dt.Rows(0).Item("COMPANYNAME").ToString
    '    Address1 = dt.Rows(0).Item("ADDRESS1").ToString
    '    Address2 = dt.Rows(0).Item("ADDRESS2").ToString
    '    Address3 = dt.Rows(0).Item("ADDRESS3").ToString


    '    FileWrite = File.CreateText(Application.StartupPath + "\FilePrint.txt")
    '    PgNo = 0
    '    line = 0
    '    'strprint = Chr(27) + "M"
    '    'FileWrite.WriteLine(strprint)
    '    strprint = Chr(15)
    '    FileWrite.WriteLine(strprint)
    '    Dim str1 As String = Space(8) : Dim str1a As String = Space(5) : Dim str2 As String = Space(8) : Dim str3 As String = Space(8)
    '    Dim str4 As String = Space(35) : Dim str5 As String = Space(12) : Dim str6 As String = Space(7)
    '    Dim str7 As String = Space(12) : Dim str8 As String = Space(17) : Dim str9 As String = Space(12)
    '    Dim str10 As String = Space(6) : Dim str11 As String = Space(12)

    '    If dtprint.Rows.Count > 0 Then
    '        Printheader(CompanyName, Address1, Address2, Address3)
    '        For i = 0 To dtprint.Rows.Count - 1
    '            mremark = ""
    '            mode = ""
    '            If dtprint.Rows(i).Item("PARTICULAR").ToString <> "GRAND TOTAL" Then
    '                str1 = LSet(dtprint.Rows(i).Item("TRANNO").ToString, 8)
    '            Else
    '                str1 = LSet("Total:", 8)
    '            End If
    '            If chkPcs.Checked = True Then
    '                str1a = LSet(dtprint.Rows(i).Item("PCS").ToString, 5)
    '            Else
    '                str1a = LSet("", 5)
    '            End If

    '            If chkGrsWt.Checked = True Then
    '                str2 = RSet(dtprint.Rows(i).Item("GRSWT").ToString, 8)
    '            Else
    '                str2 = RSet("", 8)
    '            End If
    '            If chkNetWt.Checked = True Then
    '                str3 = RSet(dtprint.Rows(i).Item("NETWT").ToString, 8)
    '            Else
    '                str3 = RSet("", 8)
    '            End If
    '            str4 = LSet(dtprint.Rows(i).Item("CUSTOMER").ToString, 35)
    '            If Val(dtprint.Rows(i).Item("AMOUNT").ToString) = 0 Then
    '                str5 = RSet(" ", 12)
    '            Else
    '                str5 = RSet((CalcRoundoffAmt(Val(dtprint.Rows(i).Item("AMOUNT").ToString), "F").ToString).ToString, 12)
    '            End If
    '            If Val(dtprint.Rows(i).Item("TAX").ToString) = 0 Then
    '                str6 = RSet(" ", 7)
    '            Else
    '                str6 = RSet((CalcRoundoffAmt(Val(dtprint.Rows(i).Item("TAX").ToString), "F").ToString).ToString, 7)
    '            End If

    '            If (Val(RSet(str5.ToString, 12)) + Val(RSet(str6.ToString, 7))) = 0 Then
    '                str7 = RSet(" ", 12)
    '            Else
    '                str7 = RSet((Val(str5) + Val(str6)).ToString, 12)
    '            End If

    '            str8 = LSet("", 19)
    '            If dtprint.Columns.Contains("CREDIT") = True Then str9 = RSet(dtprint.Rows(i).Item("CREDIT").ToString, 12) Else str9 = Space(12)
    '            If dtprint.Columns.Contains("MODE") = True Then mode = dtprint.Rows(i).Item("MODE").ToString
    '            If dtprint.Rows(i).Item("PARTICULAR").ToString <> "GRAND TOTAL" Then
    '                If mode = "CASH" Then
    '                    str10 = LSet("CASH", 6)
    '                    str11 = RSet(dtprint.Rows(i).Item("AMT").ToString, 12)
    '                ElseIf mode = "CHEQUE" Then
    '                    str10 = LSet("CHEQUE", 6)
    '                    str11 = RSet(dtprint.Rows(i).Item("AMT").ToString, 12)
    '                ElseIf mode = "SALES" Then
    '                    str10 = LSet("SA", 6)
    '                    str11 = RSet(dtprint.Rows(i).Item("AMT").ToString, 12)
    '                ElseIf mode = "CREDITCARD" Then
    '                    str10 = LSet("C CARD", 6)
    '                    str11 = RSet(dtprint.Rows(i).Item("AMT").ToString, 12)
    '                ElseIf mode = "CHIT" Then
    '                    str10 = LSet("CHIT", 6)
    '                    str11 = RSet(dtprint.Rows(i).Item("AMT").ToString, 12)
    '                ElseIf mode = "ADVANCE" Then
    '                    str10 = LSet("ADV", 6)
    '                    str11 = RSet(dtprint.Rows(i).Item("AMT").ToString, 12)
    '                ElseIf mode = "PURCHASE" Then
    '                    str10 = LSet("PU", 6)
    '                    str11 = RSet(dtprint.Rows(i).Item("AMT").ToString, 12)
    '                Else
    '                    str10 = LSet("", 6)
    '                    str11 = RSet("", 12)
    '                End If
    '            Else
    '                str10 = LSet("", 6)
    '                str11 = RSet(dtprint.Rows(i).Item("AMT").ToString, 12)
    '            End If
    '            'str10 = LSet(dtprint.Rows(i).Item("MODE").ToString, 4)
    '            'If mode <> "" Then
    '            '    str11 = RSet(dtprint.Rows(i).Item("AMT").ToString, 12)
    '            'Else
    '            '    str11 = RSet("", 12)
    '            'End If
    '            If dtprint.Rows(i).Item("PARTICULAR").ToString = "GRAND TOTAL" Then
    '                strprint = Space(80)
    '                FileWrite.WriteLine(strprint)
    '                line += 1
    '                strprint = Space(80)
    '                FileWrite.WriteLine(strprint)
    '                line += 1
    '                strprint = "------------------------------------------------------------------------------------------------------------------------------------------------"
    '                FileWrite.WriteLine(strprint)
    '                line += 1
    '            End If

    '            If str1 <> LSet("", 8) Then
    '                strprint = Space(80)
    '                FileWrite.WriteLine(strprint)
    '                line += 1
    '            End If
    '            If dtprint.Rows(i).Item("PARTICULAR").ToString <> "SUB TOTAL" Then
    '                strprint = str1 + Space(1) + str1a + Space(1) + str2 + Space(2) + str3 + Space(3) + str4 + Space(2) + str5 + Space(3) + str6 + Space(3) + str7 + Space(2) + str10 + str11 + Space(2) + str9
    '                FileWrite.WriteLine(strprint)
    '                line += 1
    '            End If

    '            If dtprint.Rows(i).Item("PARTICULAR").ToString = "GRAND TOTAL" Then
    '                strprint = "------------------------------------------------------------------------------------------------------------------------------------------------"
    '                FileWrite.WriteLine(strprint)
    '                strprint = Chr(12)
    '                FileWrite.WriteLine(strprint)
    '            End If
    '            If line >= 61 Then
    '                strprint = "------------------------------------------------------------------------------------------------------------------------------------------------"
    '                FileWrite.WriteLine(strprint)
    '                strprint = Chr(12)
    '                FileWrite.WriteLine(strprint)
    '                Printheader(CompanyName, Address1, Address2, Address3)
    '            End If
    '        Next
    '    End If
    '    FileWrite.Close()
    '    line += 1
    '    Dim frmPrinterSelect As New frmPrinterSelect
    '    frmPrinterSelect.Show()
    'End Function

    Function Printheader(ByVal CompanyName As String, Optional ByVal Address1 As String = Nothing, Optional ByVal Address2 As String = Nothing, Optional ByVal Address3 As String = Nothing) As Integer
        PgNo += 1
        line = 0
        Dim TITLE As String = Nothing
        Dim category As String = Nothing
        'If cmbMetal.Text <> "ALL" Then

        If cmbCategory.Text <> "ALL" Then
            TITLE = "SALES REGISTER FOR " & cmbCategory.Text & " FROM " & dtpFrom.Value & " TO " & dtpTo.Value
            'category = " - " & cmbCategory.Text
            'End If
        Else
            TITLE = "SALES REGISTER FROM " & dtpFrom.Value & " TO " & dtpTo.Value
        End If
        strprint = Space((140 - Len(CompanyName)) / 2) + CompanyName
        FileWrite.WriteLine(strprint) : line += 1
        strprint = Space((140 - Len(Trim(Address1 & "," & Address2 & "," & Address3))) / 2) + Address1 + Address2 + Address3
        FileWrite.WriteLine(strprint) : line += 1
        'strprint = "               ---------------------------------------------------------------------------"
        'FileWrite.WriteLine(Trim(strprint)) : line += 1
        Dim period As String
        period = ("For the Period  from " & dtpFrom.Value.Date.ToString("dd/MM/yyyy") & " to " & dtpTo.Value.Date.ToString("dd/MM/yyyy"))
        'strprint = Space((140 - Len(lblTitle.Text)) / 2) + lblTitle.Text
        'FileWrite.WriteLine(strprint) : line += 1
        strprint = Space((140 - Len(Trim(TITLE))) / 2) + TITLE
        FileWrite.WriteLine(strprint) : line += 1
        strprint = (Space(136) + "Pg #" & PgNo)
        FileWrite.WriteLine(strprint) : line = line + 1
        strprint = "------------------------------------------------------------------------------------------------------------------------------------------------"
        FileWrite.WriteLine(Trim(strprint)) : line += 1

        Dim str1 As String = Space(8) : Dim str1a As String = Space(5) : Dim str2 As String = Space(8) : Dim str3 As String = Space(8)
        Dim str4 As String = Space(35) : Dim str5 As String = Space(12) : Dim str6 As String = Space(7)
        Dim str7 As String = Space(12) : Dim str8 As String = Space(19) : Dim str9 As String = Space(12)
        Dim str10 As String = Space(6) : Dim str11 As String = Space(12)


        str1 = LSet("Bill No.", 8)
        str4 = LSet("Customer Name And Address ", 35)
        str5 = RSet("Amount.", 12)
        str6 = RSet("Vat.", 7)
        str7 = RSet("Bill Amt.", 12)
        str8 = LSet("Receipt Details", 19)
        str9 = RSet("CREDIT ", 12)
        str10 = LSet("Mode", 6)
        str11 = RSet("Amt", 12)
        strprint = str1 + str1a + Space(1) + str2 + Space(2) + str3 + Space(4) + str4 + Space(2) + str5 + Space(3) + str6 + Space(3) + str7 + Space(3) + str8 + Space(2) + str9
        FileWrite.WriteLine(strprint) : line += 1
        strprint = Space(112) + str10 + str11
        FileWrite.WriteLine(strprint) : line += 1
        strprint = "------------------------------------------------------------------------------------------------------------------------------------------------"
        FileWrite.WriteLine(Trim(strprint)) : line += 1
    End Function

    Private Sub chkBillWise_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkBillWise.CheckedChanged
        If chkBillWise.Checked = True Then
            chkSepDia.Enabled = False
            chkSepDia.Checked = False
        Else
            chkSepDia.Enabled = True
        End If
    End Sub
End Class

Public Class frmSalesAbstractNew_Properties
    Private chkPcs As Boolean = True
    Public Property p_chkPcs() As Boolean
        Get
            Return chkPcs
        End Get
        Set(ByVal value As Boolean)
            chkPcs = value
        End Set
    End Property
    Private chkGrsWt As Boolean = True
    Public Property p_chkGrsWt() As Boolean
        Get
            Return chkGrsWt
        End Get
        Set(ByVal value As Boolean)
            chkGrsWt = value
        End Set
    End Property
    Private chkNetWt As Boolean = True
    Public Property p_chkNetWt() As Boolean
        Get
            Return chkNetWt
        End Get
        Set(ByVal value As Boolean)
            chkNetWt = value
        End Set
    End Property
    Private chkWithSR As Boolean = False
    Public Property p_chkWithSR() As Boolean
        Get
            Return chkWithSR
        End Get
        Set(ByVal value As Boolean)
            chkWithSR = value
        End Set
    End Property
    Private chkCmbCostCentre As New List(Of String)
    Public Property p_chkCmbCostCentre() As List(Of String)
        Get
            Return chkCmbCostCentre
        End Get
        Set(ByVal value As List(Of String))
            chkCmbCostCentre = value
        End Set
    End Property
    Private chkVA As Boolean = False
    Public Property p_chkVA() As Boolean
        Get
            Return chkVA
        End Get
        Set(ByVal value As Boolean)
            chkVA = value
        End Set
    End Property
    Private cmbMetal As String = "ALL"
    Public Property p_cmbMetal() As String
        Get
            Return cmbMetal
        End Get
        Set(ByVal value As String)
            cmbMetal = value
        End Set
    End Property
    Private cmbCategory As String = "ALL"
    Public Property p_cmbCategory() As String
        Get
            Return cmbCategory
        End Get
        Set(ByVal value As String)
            cmbCategory = value
        End Set
    End Property
    Private rbtSummary As Boolean = True
    Public Property p_rbtSummary() As Boolean
        Get
            Return rbtSummary
        End Get
        Set(ByVal value As Boolean)
            rbtSummary = value
        End Set
    End Property
    Private rbtMonth As Boolean = False
    Public Property p_rbtMonth() As Boolean
        Get
            Return rbtMonth
        End Get
        Set(ByVal value As Boolean)
            rbtMonth = value
        End Set
    End Property
    Private rbtDate As Boolean = False
    Public Property p_rbtDate() As Boolean
        Get
            Return rbtDate
        End Get
        Set(ByVal value As Boolean)
            rbtDate = value
        End Set
    End Property
    Private rbtBillNo As Boolean = False
    Public Property p_rbtBillNo() As Boolean
        Get
            Return rbtBillNo
        End Get
        Set(ByVal value As Boolean)
            rbtBillNo = value
        End Set
    End Property
End Class