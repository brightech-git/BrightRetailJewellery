Imports System.Data.OleDb
Public Class frmCategoryWeightTransfer
#Region "Variable Declaration"
    Dim strSql As String
    Dim ds As New DataSet
    Dim dtUpdatable As New DataTable
    Dim dtCompany As New DataTable, dtCostCentre As New DataTable
    Dim cmd As OleDbCommand
    Dim tran As OleDbTransaction
    Dim frmcatcode As String = Nothing
    Dim tocatcode As String = Nothing
    Dim frmcatcodemetal As String = Nothing
    Dim tocatcodemetal As String = Nothing
    Dim selectedcompid As String = Nothing
    Dim selectedcostid As String = Nothing
    Dim dtCatstk As New DataTable
    Dim BillDate As Date
    Dim Batchno As String = ""
    Dim SPECIFICFORMAT As String = GetAdmindbSoftValue("SPECIFICFORMAT", "")
#End Region

#Region "User Defined Function"

    Private Sub Funcnew()
        BillDate = GetEntryDate(GetServerDate)
        frmcatcode = Nothing
        tocatcode = Nothing
        frmcatcodemetal = Nothing
        tocatcodemetal = Nothing
        selectedcompid = Nothing
        selectedcostid = Nothing
        dtpFrom.Value = GetEntryDate(GetServerDate)
        btnTransfer.Enabled = True
        txtCatGrsWt_Wet.Clear()
        txtCatNetWt_Wet.Clear()
        txtCatPcs_NUM.Clear()
        Batchno = ""

        strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COSTCENTRE'"
        If objGPack.GetSqlValue(strSql, , "N") = "Y" Then
            cmbCostCentre_MAN.Items.Clear()
            strSql = " SELECT COSTNAME,CONVERT(VARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
            If SPECIFICFORMAT.ToString = "1" Then
                strSql += vbCrLf + " WHERE COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COMPANYID LIKE '%" & strCompanyId & "%')"
            End If
            strSql += " ORDER BY RESULT,COSTNAME"
            'dtCostCentre = New DataTable
            'da = New OleDbDataAdapter(strSql, cn)
            'da.Fill(dtCostCentre)
            'BrighttechPack.GlobalMethods.FillCombo(cmbCostCentre_MAN, dtCostCentre, "COSTNAME", , cnCostName)
            objGPack.FillCombo(strSql, cmbCostCentre_MAN, True, True)
            cmbCostCentre_MAN.Text = cnCostName
            If _SyncTo <> "" Then
                cmbCostCentre_MAN.Enabled = False
            End If
        Else
            cmbCostCentre_MAN.Items.Clear()
            cmbCostCentre_MAN.Enabled = False
        End If

        strSql = "SELECT CATNAME,1 RESULT FROM " & cnAdminDb & "..CATEGORY ORDER BY RESULT,CATNAME"
        objGPack.FillCombo(strSql, cmbCatNamefrom, True, True)
        objGPack.FillCombo(strSql, cmbCategoryTo, True, True)
        dtpFrom.Focus()
    End Sub

    Private Sub Transfer()
        Dim instkrow As DataRow = Nothing
        btnTransfer.Enabled = False
        BillDate = dtpFrom.Value
        Dim objSoftKeys As New SoftKeys
        Dim curbillcostid As String = ""
        If cmbCostCentre_MAN.Enabled And cmbCostCentre_MAN.Text <> "" Then
            strSql = " SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME='" & cmbCostCentre_MAN.Text.ToString & "'"
            curbillcostid = GetSqlValue(cn, strSql)
        End If
        instkrow = dtCatstk.NewRow
        With instkrow
            .Item("CATCODE") = frmcatcode
            .Item("METALID") = frmcatcodemetal.ToString
            .Item("PCS") = Val(txtCatPcs_NUM.Text).ToString
            .Item("GRSWT") = Format(Val(txtCatGrsWt_Wet.Text), "0.000").ToString
            .Item("NETWT") = Format(Val(txtCatNetWt_Wet.Text), "0.000").ToString
            .Item("TRANTYPE") = "IIN"
            .Item("COSTID") = curbillcostid.ToString
            .Item("REMARK1") = "Category Weight Transfer"
        End With
        dtCatstk.Rows.Add(instkrow)
        instkrow = dtCatstk.NewRow
        With instkrow
            .Item("CATCODE") = tocatcode
            .Item("METALID") = tocatcodemetal.ToString
            .Item("PCS") = Val(txtCatPcs_NUM.Text).ToString
            .Item("GRSWT") = Format(Val(txtCatGrsWt_Wet.Text), "0.000").ToString
            .Item("NETWT") = Format(Val(txtCatNetWt_Wet.Text), "0.000").ToString
            .Item("TRANTYPE") = "RIN"
            .Item("COSTID") = curbillcostid.ToString
            .Item("REMARK1") = "Category Weight Transfer"
        End With
        dtCatstk.Rows.Add(instkrow)
        Try
            tran = Nothing
            tran = cn.BeginTransaction
            If dtCatstk.Rows.Count > 0 Then
                If Batchno = "" Then Batchno = GetNewBatchno(cnCostId, BillDate, tran)
                Dim driss() As DataRow = dtCatstk.Select("TRANTYPE='IIN'")
                If Not driss Is Nothing Then InsertIssueReceipt("IIN", driss)
                Dim drrec() As DataRow = dtCatstk.Select("TRANTYPE='RIN'")
                If Not drrec Is Nothing Then InsertIssueReceipt("RIN", drrec)
            End If
            tran.Commit()
            tran = Nothing
            MsgBox("Transfer Completed..")
            btnTransfer.Enabled = True
            Funcnew()
            btnExit.Select()
        Catch ex As Exception
            If tran IsNot Nothing Then
                tran.Rollback()
            End If
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
            btnTransfer.Enabled = True
        End Try
    End Sub

    Private Sub InsertIssueReceipt(ByVal Trantype As String, ByVal dtr() As DataRow)
        strSql = " IF (SELECT COUNT(*) FROM " & cnAdminDb & "..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "BILLNO')> 0"
        strSql += vbCrLf + " DROP TABLE " & cnAdminDb & "..TEMP" & systemId & "BILLNO"
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()
        Dim billcontrolid As String = "GEN-SM-INTISS"
        If Trantype = "IIN" Then billcontrolid = "GEN-SM-INTISS"
        If Trantype = "RIN" Then billcontrolid = "GEN-SM-INTREC"
        strSql = "SELECT CTLMODE FROM " & cnStockDb & "..BILLCONTROL"
        strSql += vbCrLf + " WHERE CTLID ='" & billcontrolid & "' AND COMPANYID = '" & strCompanyId & "'"
        If strBCostid <> Nothing Then strSql += vbCrLf + " AND ISNULL(COSTID,'') ='" & strBCostid & "'"
        If UCase(objGPack.GetSqlValue(strSql, , , tran)) <> "Y" Then
            billcontrolid = "GEN-STKREFNO"
        End If
        Dim NEWBILLNO As Integer
        strSql = "SELECT CONVERT(INT,CTLTEXT) FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = '" & billcontrolid & "' AND COMPANYID = '" & strCompanyId & "'"
        If strBCostid <> Nothing Then strSql += vbCrLf + " AND ISNULL(COSTID,'') ='" & strBCostid & "'"
        NEWBILLNO = Val(objGPack.GetSqlValue(strSql, , , tran)) + 1
GenerateNewBillNo:
        If Trantype = "IIN" Then
            strSql = vbCrLf + " SELECT TOP 1 SNO FROM " & cnStockDb & "..ISSUE WHERE TRANTYPE = 'IIN' AND COSTID = '" & cnCostId & "' AND TRANNO = " & NEWBILLNO
        Else
            strSql = vbCrLf + " SELECT TOP 1 SNO FROM " & cnStockDb & "..RECEIPT WHERE TRANTYPE = 'RIN' AND COSTID = '" & cnCostId & "' AND TRANNO = " & NEWBILLNO
        End If
        If objGPack.GetSqlValue(strSql, , "-1", tran) <> "-1" Then
            NEWBILLNO = NEWBILLNO + 1
            GoTo GenerateNewBillNo
        End If
        strSql = "UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT = '" & NEWBILLNO.ToString & "'"
        strSql += vbCrLf + " WHERE CTLID ='" & billcontrolid & "' AND COMPANYID = '" & strCompanyId & "'"
        If strBCostid <> Nothing Then strSql += vbCrLf + " AND ISNULL(COSTID,'') ='" & strBCostid & "'"
        cmd = New OleDbCommand(strSql, cn, tran)
        If cmd.ExecuteNonQuery() = 0 Then
            If strBCostid <> Nothing Then MsgBox("No bill control for this cost id " & strBCostid) : Exit Sub
            GoTo GenerateNewBillNo
        End If

        For Each row As DataRow In dtr
            Dim issSno As String = GetNewSno(IIf(Trantype = "IIN", TranSnoType.ISSUECODE, TranSnoType.RECEIPTCODE), tran)
            strSql = "INSERT INTO " & cnStockDb & ".." & IIf(Trantype = "IIN", "ISSUE", "RECEIPT") & "("
            strSql += vbCrLf + " SNO,TRANNO,TRANDATE,TRANTYPE,PCS"
            strSql += vbCrLf + " ,GRSWT,NETWT,LESSWT,PUREWT"
            strSql += vbCrLf + " ,TAGNO,ITEMID,SUBITEMID,WASTPER"
            strSql += vbCrLf + " ,WASTAGE,MCGRM,MCHARGE,AMOUNT"
            strSql += vbCrLf + " ,RATE,BOARDRATE,SALEMODE,GRSNET"
            strSql += vbCrLf + " ,TRANSTATUS,REFNO,REFDATE,COSTID"
            strSql += vbCrLf + " ,COMPANYID,FLAG,EMPID,TAGGRSWT"
            strSql += vbCrLf + " ,TAGNETWT,TAGRATEID,TAGSVALUE,TAGDESIGNER"
            strSql += vbCrLf + " ,ITEMCTRID,ITEMTYPEID,PURITY,TABLECODE"
            strSql += vbCrLf + " ,INCENTIVE,WEIGHTUNIT,CATCODE,OCATCODE"
            strSql += vbCrLf + " ,ACCODE,ALLOY,BATCHNO,REMARK1"
            strSql += vbCrLf + " ,REMARK2,USERID,UPDATED,UPTIME,SYSTEMID,DISCOUNT,RUNNO,CASHID,TAX"
            strSql += vbCrLf + " ,STNAMT,MISCAMT,METALID,STONEUNIT,APPVER,BAGNO"
            strSql += vbCrLf + " )"
            strSql += vbCrLf + " VALUES("
            strSql += vbCrLf + " '" & issSno & "'" ''SNO
            strSql += vbCrLf + " ," & NEWBILLNO & "" 'TRANNO
            strSql += vbCrLf + " ,'" & BillDate.ToString("yyyy-MM-dd") & "'" 'TRANDATE
            strSql += vbCrLf + " ,'" & Trantype & "'" 'TRANTYPE
            strSql += vbCrLf + " ," & Val(row.Item("PCS").ToString) 'PCS
            strSql += vbCrLf + " ," & Val(row.Item("GRSWT").ToString) & "" 'GRSWT
            strSql += vbCrLf + " ," & Val(row.Item("NETWT").ToString) & "" 'NETWT
            strSql += vbCrLf + " ,0" 'LESSWT
            strSql += vbCrLf + " ,0" '& Val(.Cells("PUREWT").Value.ToString) & "" 'PUREWT '0
            strSql += vbCrLf + " ,''" 'TAGNO
            strSql += vbCrLf + " ,0" 'ITEMID
            strSql += vbCrLf + " ,0" 'SUBITEMID
            strSql += vbCrLf + " ,0" 'WASTPER
            strSql += vbCrLf + " ,0" 'WASTAGE
            strSql += vbCrLf + " ,0" 'MCGRM
            strSql += vbCrLf + " ,0" 'MCHARGE
            strSql += vbCrLf + " ,0" 'AMOUNT
            strSql += vbCrLf + " ,0" ' & Val(.Cells("RATE").Value.ToString) & "" 'RATE
            strSql += vbCrLf + " ,0" '& Val(.Cells("RATE").Value.ToString) & "" 'BOARDRATE
            strSql += vbCrLf + " ,''" 'SALEMODE
            strSql += vbCrLf + " ,'N'" 'GRSNET
            strSql += vbCrLf + " ,''" 'TRANSTATUS ''
            strSql += vbCrLf + " ,''" 'REFNO ''
            strSql += vbCrLf + " ,NULL" 'REFDATE NULL
            strSql += vbCrLf + " ,'" & IIf(row.Item("COSTID").ToString = "", cnCostId, row.Item("COSTID").ToString) & "'" 'COSTID 
            strSql += vbCrLf + " ,'" & strCompanyId & "'" 'COMPANYID
            strSql += vbCrLf + " ,'O'" 'FLAG 
            strSql += vbCrLf + " ,0" 'EMPID
            strSql += vbCrLf + " ,0" 'TAGGRSWT
            strSql += vbCrLf + " ,0" 'TAGNETWT
            strSql += vbCrLf + " ,0" 'TAGRATEID
            strSql += vbCrLf + " ,0" 'TAGSVALUE
            strSql += vbCrLf + " ,''" 'TAGDESIGNER  
            strSql += vbCrLf + " ,0" 'ITEMCTRID
            strSql += vbCrLf + " ,0" 'ITEMTYPEID
            strSql += vbCrLf + " ,0" 'PURITY
            strSql += vbCrLf + " ,''" 'TABLECODE
            strSql += vbCrLf + " ,''" 'INCENTIVE
            strSql += vbCrLf + " ,''" 'WEIGHTUNIT
            strSql += vbCrLf + " ,'" & row.Item("catCode").ToString & "'" 'CATCODE
            strSql += vbCrLf + " ,'" & row.Item("catCode").ToString & "'" 'OCATCODE
            strSql += vbCrLf + " ,'STKTRAN'" 'ACCODE
            strSql += vbCrLf + " ,0" 'ALLOY
            strSql += vbCrLf + " ,'" & Batchno & "'" 'BATCHNO
            strSql += vbCrLf + " ,'" & row.Item("REMARK1").ToString & "'" 'REMARK1
            strSql += vbCrLf + " ,''" 'REMARK2
            strSql += vbCrLf + " ,'" & userId & "'" 'USERID
            strSql += vbCrLf + " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
            strSql += vbCrLf + " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
            strSql += vbCrLf + " ,'" & systemId & "'" 'SYSTEMID
            strSql += vbCrLf + " ,0" 'DISCOUNT
            strSql += vbCrLf + " ,''" 'RUNNO
            strSql += vbCrLf + " ,''" 'CASHID
            strSql += vbCrLf + " ,0" 'TAX
            strSql += vbCrLf + " ,0" 'STONEAMT
            strSql += vbCrLf + " ,0" 'MISCAMT
            strSql += vbCrLf + " ,'" & row.Item("METALID").ToString & "'" 'METALID
            strSql += vbCrLf + " ,''" 'STONEUNIT
            strSql += vbCrLf + " ,'" & VERSION & "'" 'APPVER
            strSql += vbCrLf + " ,''" 'BAGNO
            strSql += vbCrLf + " )"
            'ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
            If CENTR_DB_BR Then
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.ExecuteNonQuery()
            Else
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, IIf(row.Item("COSTID").ToString = "", cnCostId, row.Item("COSTID").ToString))
            End If
        Next
    End Sub

#End Region

#Region "Events"

    Private Sub frmCategoryTransfer_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmCategoryTransfer_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        strSql = " SELECT '' METALID,'' TRANTYPE,''CATCODE,''PCS,''GRSWT,'' DUSTWT,''NETWT,'' TOUCH,''PUREWT,'' CNETWT"
        strSql += vbCrLf + " ,'' RATE,''AMOUNT,''REMARK1,''BATCHNO,''COSTID WHERE 1<>1"
        da = New OleDbDataAdapter(strSql, cn)
        dtCatstk = New DataTable
        dtCatstk.Columns.Add("SNO", GetType(Integer))
        dtCatstk.Columns("SNO").AutoIncrement = True
        dtCatstk.Columns("SNO").AutoIncrementSeed = 0
        dtCatstk.Columns("SNO").AutoIncrementStep = 1
        da.Fill(dtCatstk)
        Funcnew()
    End Sub


    Private Sub btnTransfer_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTransfer.Click
        dtCatstk.Rows.Clear()

        If Val(txtCatGrsWt_Wet.Text) = 0 And Val(txtCatNetWt_Wet.Text) = 0 And Val(txtCatPcs_NUM.Text) = 0 Then
            MsgBox("Enter Valid Input", MsgBoxStyle.Information)
            Exit Sub
        End If
        ''If Val(txtCatGrsWt_NUM.Text) = 0 Then
        ''    MsgBox("Enter Valid Gross Weight", MsgBoxStyle.Information)
        ''    Exit Sub
        ''End If
        ''If Val(txtCatNetWt_NUM.Text) = 0 Then
        ''    MsgBox("Enter Valid Net Weight", MsgBoxStyle.Information)
        ''    Exit Sub
        ''End If
        strSql = " SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME='" & cmbCatNamefrom.Text.ToString & "'"
        frmcatcode = GetSqlValue(cn, strSql).ToString
        strSql = " SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME='" & cmbCategoryTo.Text.ToString & "'"
        tocatcode = GetSqlValue(cn, strSql).ToString
        strSql = " SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME='" & cmbCatNamefrom.Text.ToString & "'"
        frmcatcodemetal = GetSqlValue(cn, strSql).ToString
        strSql = " SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME='" & cmbCategoryTo.Text.ToString & "'"
        tocatcodemetal = GetSqlValue(cn, strSql).ToString
        If frmcatcode = "" Then
            MsgBox("Enter Valid From Category", MsgBoxStyle.Information)
            Exit Sub
        End If
        If tocatcode = "" Then
            MsgBox("Enter Valid To Category", MsgBoxStyle.Information)
            Exit Sub
        End If
        If frmcatcodemetal = "" Then
            MsgBox("Enter Valid From Category", MsgBoxStyle.Information)
            Exit Sub
        End If
        If tocatcodemetal = "" Then
            MsgBox("Enter Valid To Category", MsgBoxStyle.Information)
            Exit Sub
        End If
        If frmcatcodemetal.ToString <> tocatcodemetal.ToString Then
            MsgBox("Different Metal Category Cannot Be Transfered", MsgBoxStyle.Information)
            Exit Sub
        End If
        Transfer()
        Exit Sub
    End Sub

    Private Sub NewToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem1.Click
        btnnew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnnew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnnew.Click
        Funcnew()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub cmbCatNamefrom_SelectedValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCatNamefrom.SelectedValueChanged
        If cmbCatNamefrom.Text <> "ALL" Then
            strSql = " SELECT CATNAME,1 RESULT FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME<>'" & cmbCatNamefrom.Text.ToString & "' ORDER BY RESULT,CATNAME"
            objGPack.FillCombo(strSql, cmbCategoryTo, True, True)
        Else
            strSql = "SELECT 'ALL' CATNAME,0 RESULT UNION ALL SELECT CATNAME,1 RESULT FROM " & cnAdminDb & "..CATEGORY ORDER BY RESULT,CATNAME"
            objGPack.FillCombo(strSql, cmbCategoryTo, True, True)
        End If
    End Sub

    Private Sub txtCatGrsWt_NUM_TextChanged(sender As Object, e As EventArgs) Handles txtCatGrsWt_Wet.TextChanged
        If Val(txtCatGrsWt_Wet.Text) <> 0 Then
            txtCatNetWt_Wet.Text = Val(txtCatGrsWt_Wet.Text)
        End If
    End Sub
#End Region
End Class