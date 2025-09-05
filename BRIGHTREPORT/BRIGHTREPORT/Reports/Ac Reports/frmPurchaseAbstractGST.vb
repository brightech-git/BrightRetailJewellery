Imports System.Data.OleDb
Imports System.IO

Public Class frmPurchaseAbstractGST
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

    Function funcNew() As Integer
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        pnlHeading.Visible = False
        gridView.DataSource = Nothing
        
        Prop_Gets()
        dtpFrom.Focus()
    End Function

    Private Sub PurchaseAbs()
        Prop_Sets()
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

            gridView.DataSource = Nothing
        Me.Refresh()
        Dim PUTYPE As String = ""
        If chkRd.Checked = False And chkUrd.Checked = False Then MsgBox("Please select Purchase type") : Exit Sub
        If chkRd.Checked = True Then PUTYPE = ",RPU"
        If chkUrd.Checked = True Then PUTYPE = PUTYPE & ",PU"
        PUTYPE = Mid(PUTYPE, 2)

        strSql = " EXEC " & cnAdminDb & "..SP_RPT_ABSTRACT_SASRPU_GST"
        strSql += vbCrLf + " @SUMMARYWISE = '" & IIf(rbtSummary.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " ,@MONTHWISE = '" & IIf(rbtMonth.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " ,@DATEWISE = '" & IIf(rbtDate.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " ,@TRANNOWISE = '" & IIf(rbtBillNo.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " ,@SYSTEMID = '" & systemId & "'"
        strSql += vbCrLf + " ,@WITHSR = 'N'"
        strSql += vbCrLf + " ,@FRMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@RPTTYPE = 'PU'"
        strSql += vbCrLf + " ,@METALNAME = '" & cmbMetal.Text & "'"
        strSql += vbCrLf + " ,@CATCODE = '" & selCatCode & "'"
        strSql += vbCrLf + " ,@CATNAME = '" & cmbCategory.Text & "'"
        strSql += vbCrLf + " ,@COSTNAME = '" & GetQryString(chkCmbCostCentre.Text).Replace("'", "") & "'"
        strSql += vbCrLf + " ,@COMPANYID = '" & strCompanyId & "'"
        strSql += vbCrLf + " ,@VA = '" & IIf(chkVA.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " ,@CHITDBID = '" & cnChitCompanyid & "'"
        strSql += vbCrLf + " ,@PURTYPE = """ & PUTYPE & """"
        strSql += vbCrLf + " ,@WITSTN = 'N'"
        strSql += vbCrLf + " ,@PUREMC = 'N'"
        strSql += vbCrLf + " ,@WITHPR = '" & IIf(chkWithPurReturn.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " ,@WITHTRANNO = 'N'"
        strSql += vbCrLf + " ,@AFTERDISC = 'N'"
        strSql += vbCrLf + " ,@WITHITEM = '" & IIf(ChkItem.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " ,@STKTYPE = 'A'"
        strSql += vbCrLf + " ,@BILLPREFIX = '" & IIf(ChkBillPrefix.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " ,@DBNAME = '" & cnStockDb & "'"
        strSql += vbCrLf + " ,@BILL = 'B'"
        strSql += vbCrLf + " ,@METALIDWISE = '" & IIf(rbtMetal.Checked = True, "Y", "N") & "'"
        If rbtBothGst.Checked Then
            strSql += vbCrLf + " ,@BILLTYPE = 'B'"
        ElseIf rbtIG.Checked Then
            strSql += vbCrLf + " ,@BILLTYPE = 'IG'"
        Else
            strSql += vbCrLf + " ,@BILLTYPE = 'CG'"
        End If
        strSql += vbCrLf + " ,@TYPE = 'ALL'"
        strSql += vbCrLf + " ,@WITHEMPNAME = 'N'"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()


        strSql = "UPDATE TEMPTABLEDB..TEMP" & systemId & "ABSSUMMARY_SA SET PCS = NULL WHERE PCS = 0"
        strSql += " UPDATE TEMPTABLEDB..TEMP" & systemId & "ABSSUMMARY_SA SET GRSWT = NULL WHERE GRSWT = 0"
        strSql += " UPDATE TEMPTABLEDB..TEMP" & systemId & "ABSSUMMARY_SA SET NETWT = NULL WHERE NETWT = 0"
        strSql += " UPDATE TEMPTABLEDB..TEMP" & systemId & "ABSSUMMARY_SA SET AMOUNT = NULL WHERE AMOUNT = 0"
        strSql += " UPDATE TEMPTABLEDB..TEMP" & systemId & "ABSSUMMARY_SA SET TAX = NULL WHERE TAX = 0"
        strSql += " UPDATE TEMPTABLEDB..TEMP" & systemId & "ABSSUMMARY_SA SET SGST = NULL WHERE SGST = 0"
        strSql += " UPDATE TEMPTABLEDB..TEMP" & systemId & "ABSSUMMARY_SA SET CGST = NULL WHERE CGST = 0"
        strSql += " UPDATE TEMPTABLEDB..TEMP" & systemId & "ABSSUMMARY_SA SET IGST = NULL WHERE IGST = 0"
        strSql += " UPDATE TEMPTABLEDB..TEMP" & systemId & "ABSSUMMARY_SA SET TOTAL = NULL WHERE TOTAL = 0"
        strSql += " UPDATE TEMPTABLEDB..TEMP" & systemId & "ABSSUMMARY_SA SET STNCT = NULL WHERE STNCT = 0"
        strSql += " UPDATE TEMPTABLEDB..TEMP" & systemId & "ABSSUMMARY_SA SET STNGT = NULL WHERE STNGT = 0"
        strSql += " UPDATE TEMPTABLEDB..TEMP" & systemId & "ABSSUMMARY_SA SET DIAWT = NULL WHERE DIAWT = 0"
        strSql += " UPDATE TEMPTABLEDB..TEMP" & systemId & "ABSSUMMARY_SA SET TDS = NULL WHERE TDS = 0"
        strSql += " UPDATE TEMPTABLEDB..TEMP" & systemId & "ABSSUMMARY_SA SET TCS = NULL WHERE TCS = 0"

        If rbtBillNo.Checked And chkVA.Checked Then
            strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "ABSSUMMARY_SA SET AMT = NULL WHERE AMT = 0"
            strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "ABSSUMMARY_SA SET CREDIT = NULL WHERE CREDIT = 0"
        End If
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " SELECT * FROM TEMPTABLEDB..TEMP" & systemId & "ABSSUMMARY_SA ORDER BY SNO"


        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("KEYNO", GetType(Integer))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record Not Found", MsgBoxStyle.Information)
            Exit Sub
        End If
        If dtGrid.Columns.Contains("BATCHNO") = True Then dtGrid.Columns.Remove("BATCHNO")
        If dtGrid.Columns.Contains("NSNO1") = True Then dtGrid.Columns.Remove("NSNO1")
        If dtGrid.Columns.Contains("NSNO") = True Then dtGrid.Columns.Remove("NSNO")
        If dtGrid.Columns.Contains("TRANTYPE") = True Then dtGrid.Columns.Remove("TRANTYPE")

        dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        gridView.DataSource = dtGrid
        With gridView
            If .Columns.Contains("TAX") Then .Columns("TAX").HeaderText = "GST"
            If .Columns.Contains("ITEMID") Then .Columns("ITEMID").Visible = False
            If .Columns.Contains("ITEMNAME") Then .Columns("ITEMNAME").Visible = False
            If .Columns.Contains("COSTNAME") Then .Columns("COSTNAME").Visible = False
            If .Columns.Contains("STNGT") Then .Columns("STNGT").Visible = False
            If .Columns.Contains("STNCT") Then .Columns("STNCT").Visible = False
            If .Columns.Contains("DISCOUNT") Then .Columns("DISCOUNT").Visible = False
            'If .Columns.Contains("MCHARGE") Then .Columns("MCHARGE").Visible = False
            If .Columns.Contains("MCHARGE") Then .Columns("MCHARGE").Visible = chkMc.Checked
            If .Columns.Contains("EMPNAME") Then .Columns("EMPNAME").Visible = False
            If .Columns.Contains("COUNTRY") Then .Columns("COUNTRY").Visible = False
            If .Columns.Contains("PINCODE") Then .Columns("PINCODE").Visible = False
            If .Columns.Contains("TAGNO") Then .Columns("TAGNO").Visible = False
            If .Columns.Contains("ADDCHRG") Then .Columns("ADDCHRG").HeaderText = "ADD CHARGE"

            .Columns("KEYNO").Visible = False
            .Columns("RESULT").Visible = False
            .Columns("COLHEAD").Visible = False
            .Columns("SNO").Visible = False
            .Columns("MMONTHID").Visible = False
            .Columns("MMONTHNAME").Visible = False
            .Columns("TRANDATE").Visible = ChkDateSep.Checked
            If .Columns.Contains("TINNO") Then .Columns("TINNO").Visible = rbtBillNo.Checked
            If .Columns.Contains("BILLNO") Then .Columns("BILLNO").Visible = rbtBillNo.Checked
            If .Columns.Contains("BILLDATE") Then .Columns("BILLDATE").Visible = rbtBillNo.Checked

            If rbtSummary.Checked Or rbtMonth.Checked Then
                .Columns("TRANDATE").Visible = False
            End If
            If rbtBillNo.Checked = True Or rbtMetal.Checked = True Then
                ' .Columns("TRANNO").Visible = rbtBillNo.Checked
                .Columns("TRANNO").Visible = True
            Else
                .Columns("TRANNO").Visible = False
            End If
            
            .Columns("METALNAME").Visible = False
            .Columns("CATNAME").Visible = False
            .Columns("CUSTOMER").Visible = rbtBillNo.Checked
            .Columns("GSTNO").Visible = rbtBillNo.Checked
            .Columns("ADDRESS").Visible = rbtBillNo.Checked And Not chkVA.Checked
            .Columns("PHONENO").Visible = rbtBillNo.Checked And Not chkVA.Checked
            .Columns("PCS").Visible = chkPcs.Checked
            .Columns("GRSWT").Visible = chkGrsWt.Checked
            .Columns("NETWT").Visible = chkNetWt.Checked
            .Columns("STNCT").Visible = ChkStnWt.Checked And Chkmore.Checked
            .Columns("DIAWT").Visible = ChkDIAwt.Checked And Chkmore.Checked
            If .Columns.Contains("STNAMT") Then .Columns("STNAMT").Visible = ChkStnAmt.Checked And Chkmore.Checked
            If .Columns.Contains("DIAAMT") Then .Columns("DIAAMT").Visible = ChkDiaAmt.Checked And Chkmore.Checked
            .Columns("PARTICULAR").Visible = Chkparticular.Checked
          
            If .Columns.Contains("COSTID") Then .Columns("COSTID").Visible = False
            .Columns("ADDRESS1").Visible = False
            .Columns("ADDRESS2").Visible = False
            .Columns("ADDRESS3").Visible = False
            .Columns("AREA").Visible = False
            .Columns("CITY").Visible = False
            .Columns("STATENAME").Visible = False
            .Columns("PLACEOFSUPPLY").Visible = False

            .Columns("PARTICULAR").Width = 250
            .Columns("TRANNO").HeaderText = "BILLNO"
            .Columns("TRANNO").Width = 70
            .Columns("PCS").Width = 60
            .Columns("GRSWT").Width = 70
            .Columns("GRSWT").Width = 80
            .Columns("NETWT").Width = 80
            If .Columns.Contains("AMOUNT") Then .Columns("AMOUNT").Width = 100
            .Columns("TAX").Width = 80
            .Columns("TOTAL").Width = 100
            .Columns("STNCT").Width = 75
            .Columns("STNGT").Width = 75
            .Columns("DIAWT").Width = 75
            .Columns("CUSTOMER").Width = 120
            .Columns("ADDRESS").Width = 150
            .Columns("PHONENO").Width = 100
            .Columns("SC").Width = 80
            .Columns("SC").HeaderText = "CESS"
            .Columns("SC").Visible = False


            If .Columns.Contains("STNNAME1") Then .Columns("STNNAME1").Visible = False
            If .Columns.Contains("STNPCS1") Then .Columns("STNPCS1").Visible = False
            If .Columns.Contains("STNWT1") Then .Columns("STNWT1").Visible = False
            If .Columns.Contains("STNRATE1") Then .Columns("STNRATE1").Visible = False
            If .Columns.Contains("STNNAME2") Then .Columns("STNNAME2").Visible = False
            If .Columns.Contains("STNPCS2") Then .Columns("STNPCS2").Visible = False
            If .Columns.Contains("STNWT2") Then .Columns("STNWT2").Visible = False
            If .Columns.Contains("STNRATE2") Then .Columns("STNRATE2").Visible = False
            If .Columns.Contains("STNNAME3") Then .Columns("STNNAME3").Visible = False
            If .Columns.Contains("STNPCS3") Then .Columns("STNPCS3").Visible = False
            If .Columns.Contains("STNWT3") Then .Columns("STNWT3").Visible = False
            If .Columns.Contains("STNRATE3") Then .Columns("STNRATE3").Visible = False
            If .Columns.Contains("DIANAME1") Then .Columns("DIANAME1").Visible = False
            If .Columns.Contains("DIAPCS1") Then .Columns("DIAPCS1").Visible = False
            If .Columns.Contains("DIAWT1") Then .Columns("DIAWT1").Visible = False
            If .Columns.Contains("DIARATE1") Then .Columns("DIARATE1").Visible = False
            If .Columns.Contains("DIANAME2") Then .Columns("DIANAME2").Visible = False
            If .Columns.Contains("DIAPCS2") Then .Columns("DIAPCS2").Visible = False
            If .Columns.Contains("DIAWT2") Then .Columns("DIAWT2").Visible = False
            If .Columns.Contains("DIARATE2") Then .Columns("DIARATE2").Visible = False
            If .Columns.Contains("DIANAME3") Then .Columns("DIANAME3").Visible = False
            If .Columns.Contains("DIAPCS3") Then .Columns("DIAPCS3").Visible = False
            If .Columns.Contains("DIAWT3") Then .Columns("DIAWT3").Visible = False
            If .Columns.Contains("DIARATE3") Then .Columns("DIARATE3").Visible = False
            If .Columns.Contains("TAGNO") Then .Columns("TAGNO").Visible = False
            If .Columns.Contains("TAGTYPE") Then .Columns("TAGTYPE").Visible = False
            If .Columns.Contains("RATE") Then .Columns("RATE").Visible = False
        End With
        FormatGridColumns(gridView, False, False, True, False)
        FillGridGroupStyle_KeyNoWise(gridView)

        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
        gridView.Invalidate()
        For Each dgvCol As DataGridViewColumn In gridView.Columns
            dgvCol.Width = dgvCol.Width
        Next
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None

        Dim TITLE As String
        If rbtSummary.Checked = True Then
            TITLE = "SUMMARY WISE"
            SpecificPrint = False
        ElseIf rbtMonth.Checked = True Then
            TITLE = "MONTH WISE"
            SpecificPrint = False
        ElseIf rbtDate.Checked = True Then
            TITLE = "DATE WISE"
            SpecificPrint = False
        ElseIf rbtMetal.Checked = True Then
            TITLE = "METAL WISE"
            SpecificPrint = False
        Else
            TITLE = "BILLNO WISE"
            If chkVA.Checked = True Then
                SpecificPrint = True
            End If
        End If
        TITLE += " PURCHASE ABSTRACT FROM " & dtpFrom.Text & " TO " & dtpTo.Text & ""
        TITLE += " AT " & chkCmbCostCentre.Text
        lblTitle.Font = New Font("VERDANA", 9, FontStyle.Bold)
        lblTitle.Text = TITLE
        pnlHeading.Visible = True
        btnView_Search.Enabled = True
        gridView.Focus()
    End Sub

    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        If chkPcs.Checked = False And chkGrsWt.Checked = False And chkNetWt.Checked = False Then
            chkGrsWt.Checked = True
        End If
        pnlHeading.Visible = False
        PurchaseAbs()
        If SpecificPrint = True Then
            btn_dPrint.Visible = True
        Else
            btn_dPrint.Visible = False
        End If
    End Sub

    Private Sub frmPurchaseAbstract_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmSalesAbstract_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        funcNew()
        cmbMetal.Items.Clear()
        cmbMetal.Items.Add("ALL")
        strSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST ORDER BY METALNAME"
        objGPack.FillCombo(strSql, cmbMetal, False)
        cmbMetal.Text = "ALL"
        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE WHERE  ISNULL(COMPANYID,'" & strCompanyId & "')LIKE'%" & strCompanyId & "%'"
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False
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
        funcNew()
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export, , , , , , True)
        End If
        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = "X" Or e.KeyChar = "x" Then
            Me.btnExcel_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = "P" Then
            Me.btnPrint_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub dtpFrom_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        dtpTo.MinimumDate = dtpFrom.Value
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New frmPurchaseAbstractGST_Properties
        obj.p_chkPcs = chkPcs.Checked
        obj.p_chkGrsWt = chkGrsWt.Checked
        obj.p_chkNetWt = chkNetWt.Checked
        'GetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre)
        obj.p_cmbMetal = cmbMetal.Text
        obj.p_rbtSummary = rbtSummary.Checked
        obj.p_rbtMonth = rbtMonth.Checked
        obj.p_rbtDate = rbtDate.Checked
        obj.p_rbtBillNo = rbtBillNo.Checked
        obj.p_chkVA = chkVA.Checked
        obj.p_chkBillPrefix = ChkBillPrefix.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmPurchaseAbstractGST_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmPurchaseAbstractGST_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmPurchaseAbstractGST_Properties))
        chkPcs.Checked = obj.p_chkPcs
        chkGrsWt.Checked = obj.p_chkGrsWt
        chkNetWt.Checked = obj.p_chkNetWt
        'SetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre, "ALL")
        cmbMetal.Text = obj.p_cmbMetal
        rbtSummary.Checked = obj.p_rbtSummary
        rbtMonth.Checked = obj.p_rbtMonth
        rbtDate.Checked = obj.p_rbtDate
        rbtBillNo.Checked = obj.p_rbtBillNo
        chkVA.Checked = obj.p_chkVA
        ChkBillPrefix.Checked = obj.p_chkBillPrefix
    End Sub

    Private Sub Chkmore_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Chkmore.CheckedChanged
        If Chkmore.Checked = True Then
            GBmore.Visible = True
        Else
            GBmore.Visible = False
        End If
    End Sub

    Private Sub btn_dPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_dPrint.Click
        If gridView.Rows.Count > 0 Then
            DetailPrint()
        End If

    End Sub

    Function DetailPrint()
        Dim CompanyName, Address1, Address2, Address3, Phone As String
        Dim dtprint As New DataTable
        Dim i As Integer
        Dim dt As New DataTable
        Dim mremark As String
        Dim mode As String
        Dim dateflag As Boolean = False

        dtprint.Clear()
        dtprint = gridView.DataSource

        strSql = "SELECT COMPANYNAME,ADDRESS1,ADDRESS2,ADDRESS3,ADDRESS4 FROM " & cnAdminDb & "..COMPANY where ISNULL(ACTIVE,'')<>'N'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)

        CompanyName = dt.Rows(0).Item("COMPANYNAME").ToString
        Address1 = dt.Rows(0).Item("ADDRESS1").ToString
        Address2 = dt.Rows(0).Item("ADDRESS2").ToString
        Address3 = dt.Rows(0).Item("ADDRESS3").ToString


        FileWrite = File.CreateText(Application.StartupPath + "\FilePrint.txt")
        PgNo = 0
        line = 0
        'strprint = Chr(27) + "M"
        'FileWrite.WriteLine(strprint)
        strprint = Chr(15)
        FileWrite.WriteLine(strprint)
        Dim str1 As String = Space(8) : Dim str1a As String = Space(5) : Dim str2 As String = Space(8) : Dim str3 As String = Space(8)
        Dim str4 As String = Space(35) : Dim str5 As String = Space(12) : Dim str6 As String = Space(7)
        Dim str7 As String = Space(12) : Dim str8 As String = Space(17) : Dim str9 As String = Space(12)
        Dim str10 As String = Space(4) : Dim str11 As String = Space(12)

        If dtprint.Rows.Count > 0 Then
            Printheader(CompanyName, Address1, Address2, Address3)
            For i = 0 To dtprint.Rows.Count - 1
                mremark = ""
                mode = ""
                If dtprint.Rows(i).Item("PARTICULAR").ToString <> "GRAND TOTAL" Then
                    str1 = LSet(dtprint.Rows(i).Item("TRANNO").ToString, 8)
                Else
                    str1 = LSet("Total:", 8)
                End If
                If chkPcs.Checked = True Then
                    str1a = LSet(dtprint.Rows(i).Item("PCS").ToString, 5)
                Else
                    str1a = LSet("", 5)
                End If

                If chkGrsWt.Checked = True Then
                    str2 = RSet(dtprint.Rows(i).Item("GRSWT").ToString, 8)
                Else
                    str2 = RSet("", 8)
                End If
                If chkNetWt.Checked = True Then
                    str3 = RSet(dtprint.Rows(i).Item("NETWT").ToString, 8)
                Else
                    str3 = RSet("", 8)
                End If

                str4 = LSet(dtprint.Rows(i).Item("CUSTOMER").ToString, 35)
                If Val(dtprint.Rows(i).Item("AMOUNT").ToString) = 0 Then
                    str5 = RSet(" ", 12)
                Else
                    str5 = RSet((CalcRoundoffAmt(Val(dtprint.Rows(i).Item("AMOUNT").ToString), "F").ToString).ToString, 12)
                End If
                If Val(dtprint.Rows(i).Item("TAX").ToString) = 0 Then
                    str6 = RSet(" ", 7)
                Else

                    str6 = RSet((CalcRoundoffAmt(Val(dtprint.Rows(i).Item("TAX").ToString), "F").ToString).ToString, 7)
                End If

                If (Val(RSet(str5.ToString, 12)) + Val(RSet(str6.ToString, 7))) = 0 Then
                    str7 = RSet(" ", 12)
                Else
                    str7 = RSet((Val(str5) + Val(str6)).ToString, 12)
                End If

                str8 = LSet("", 17)
                str9 = RSet(dtprint.Rows(i).Item("CREDIT").ToString, 12)
                mode = dtprint.Rows(i).Item("MODE").ToString
                If dtprint.Rows(i).Item("PARTICULAR").ToString <> "GRAND TOTAL" Then
                    If mode = "CASH" Then
                        str10 = LSet("CA", 4)
                        str11 = RSet(dtprint.Rows(i).Item("AMT").ToString, 12)
                    ElseIf mode = "CHEQUE" Then
                        str10 = LSet("CH", 4)
                        str11 = RSet(dtprint.Rows(i).Item("AMT").ToString, 12)
                    ElseIf mode = "SALES" Then
                        str10 = LSet("SA", 4)
                        str11 = RSet(dtprint.Rows(i).Item("AMT").ToString, 12)
                    ElseIf mode = "CREDITCARD" Then
                        str10 = LSet("CC", 4)
                        str11 = RSet(dtprint.Rows(i).Item("AMT").ToString, 12)
                    ElseIf mode = "CHIT" Then
                        str10 = LSet("SS", 4)
                        str11 = RSet(dtprint.Rows(i).Item("AMT").ToString, 12)
                    ElseIf mode = "ADVANCE" Then
                        str10 = LSet("AA", 4)
                        str11 = RSet(dtprint.Rows(i).Item("AMT").ToString, 12)
                    ElseIf mode = "PURCHASE" Then
                        str10 = LSet("PU", 4)
                        str11 = RSet(dtprint.Rows(i).Item("AMT").ToString, 12)
                    Else
                        str10 = LSet("", 4)
                        str11 = RSet("", 12)
                    End If
                Else
                    str10 = LSet("", 4)
                    str11 = RSet(dtprint.Rows(i).Item("AMT").ToString, 12)
                End If


                'str10 = LSet(dtprint.Rows(i).Item("MODE").ToString, 4)
                'If mode <> "" Then
                '    str11 = RSet(dtprint.Rows(i).Item("AMT").ToString, 12)
                'Else
                '    str11 = RSet("", 12)
                'End If
                If dtprint.Rows(i).Item("PARTICULAR").ToString = "GRAND TOTAL" Then
                    strprint = Space(80)
                    FileWrite.WriteLine(strprint)
                    line += 1
                    strprint = Space(80)
                    FileWrite.WriteLine(strprint)
                    line += 1
                    strprint = "----------------------------------------------------------------------------------------------------------------------------------------------"
                    FileWrite.WriteLine(strprint)
                    line += 1
                End If
                If str1 <> LSet("", 8) Then
                    strprint = Space(80)
                    FileWrite.WriteLine(strprint)
                    line += 1
                End If
                If dtprint.Rows(i).Item("PARTICULAR").ToString <> "SUB TOTAL" Then
                    strprint = str1 + Space(1) + str1a + Space(1) + str2 + Space(2) + str3 + Space(3) + str4 + Space(2) + str5 + Space(3) + str6 + Space(3) + str7 + Space(2) + str10 + str11 + Space(2) + str9
                    FileWrite.WriteLine(strprint)
                    line += 1
                End If
                If dtprint.Rows(i).Item("PARTICULAR").ToString = "GRAND TOTAL" Then
                    strprint = "----------------------------------------------------------------------------------------------------------------------------------------------"
                    FileWrite.WriteLine(strprint)
                    strprint = Chr(12)
                    FileWrite.WriteLine(strprint)
                End If
                If line >= 61 Then
                    strprint = "----------------------------------------------------------------------------------------------------------------------------------------------"
                    FileWrite.WriteLine(strprint)
                    strprint = Chr(12)
                    FileWrite.WriteLine(strprint)
                    Printheader(CompanyName, Address1, Address2, Address3)
                End If
            Next
        End If
        FileWrite.Close()
        line += 1
        Dim frmPrinterSelect As New frmPrinterSelect
        frmPrinterSelect.Show()
    End Function

    Function Printheader(ByVal CompanyName As String, Optional ByVal Address1 As String = Nothing, Optional ByVal Address2 As String = Nothing, Optional ByVal Address3 As String = Nothing) As Integer
        PgNo += 1
        line = 0
        Dim TITLE As String = Nothing

        'If cmbMetal.Text <> "ALL" Then
        '    metal = cmbMetal.Text
        If cmbCategory.Text <> "ALL" Then
            TITLE = "PURCHASE ABSTRACT FOR " & cmbCategory.Text & " FROM " & dtpFrom.Value & " TO " & dtpTo.Value
            '    category = " - " & cmbCategory.Text
        Else
            TITLE = "PURCHASE ABSTRACT FROM " & dtpFrom.Value & " TO " & dtpTo.Value
        End If
        strprint = Space((140 - Len(CompanyName)) / 2) + CompanyName
        FileWrite.WriteLine(strprint) : line += 1
        strprint = Space((140 - Len(Trim(Address1 & "," & Address2 & "," & Address3))) / 2) + Address1 + Address2 + Address3
        FileWrite.WriteLine(strprint) : line += 1
        'Dim period As String
        'period = ("For the Period  from " & dtpFrom.Value.Date.ToString("dd/MM/yyyy") & " to " & dtpTo.Value.Date.ToString("dd/MM/yyyy"))
        'strprint = Space((140 - Len(lblTitle.Text)) / 2) + lblTitle.Text
        'FileWrite.WriteLine(strprint) : line += 1
        strprint = Space((140 - Len(Trim(TITLE))) / 2) + TITLE
        FileWrite.WriteLine(strprint) : line += 1
        strprint = (Space(130) + "Pg #" & PgNo)
        FileWrite.WriteLine(strprint) : line = line + 1
        strprint = "----------------------------------------------------------------------------------------------------------------------------------------------"
        FileWrite.WriteLine(Trim(strprint)) : line += 1

        Dim str1 As String = Space(8) : Dim str1a As String = Space(5) : Dim str2 As String = Space(8) : Dim str3 As String = Space(8)
        Dim str4 As String = Space(35) : Dim str5 As String = Space(12) : Dim str6 As String = Space(7)
        Dim str7 As String = Space(12) : Dim str8 As String = Space(17) : Dim str9 As String = Space(12)
        Dim str10 As String = Space(4) : Dim str11 As String = Space(12)


        str1 = LSet("Bill No.", 8)
        If chkPcs.Checked = True Then
            str1a = RSet("Pcs.", 5)
        Else
            str1a = RSet("", 5)
        End If
        If chkGrsWt.Checked = True Then
            str2 = RSet("Gwt.", 8)
        Else
            str2 = RSet("", 8)
        End If
        If chkNetWt.Checked = True Then
            str3 = RSet("Nwt.", 8)
        Else
            str3 = RSet("", 8)
        End If
        str4 = LSet("Customer Name And Address ", 35)
        str5 = RSet("Amount.", 12)
        str6 = LSet("I/P Vat.", 7)
        str7 = RSet("Bill Amt.", 12)
        str8 = LSet("Payment Details", 17)
        str9 = RSet("CREDIT ", 12)
        str10 = LSet("Mode", 4)
        str11 = RSet("Amt", 12)
        strprint = str1 + Space(1) + str1a + Space(1) + str2 + Space(2) + str3 + Space(3) + str4 + Space(2) + str5 + Space(3) + str6 + Space(3) + str7 + Space(2) + str8 + Space(2) + str9
        FileWrite.WriteLine(strprint) : line += 1
        strprint = Space(112) + str10 + str11
        FileWrite.WriteLine(strprint) : line += 1
        strprint = "----------------------------------------------------------------------------------------------------------------------------------------------"
        FileWrite.WriteLine(Trim(strprint)) : line += 1
    End Function


    Private Sub cmbMetal_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbMetal.SelectedIndexChanged
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

    Private Sub AutoReziseToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AutoReziseToolStripMenuItem.Click
        If gridView.RowCount > 0 Then
            If AutoReziseToolStripMenuItem.Checked Then
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                gridView.Invalidate()
                For Each dgvCol As DataGridViewColumn In gridView.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            Else
                For Each dgvCol As DataGridViewColumn In gridView.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End If
        End If
    End Sub
End Class

Public Class frmPurchaseAbstractGST_Properties
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
    Private chkBillPrefix As Boolean = False
    Public Property p_chkBillPrefix() As Boolean
        Get
            Return chkBillPrefix
        End Get
        Set(ByVal value As Boolean)
            chkBillPrefix = value
        End Set
    End Property
End Class