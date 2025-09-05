Imports System.Data.OleDb
Imports System.Data
Public Class frmGstRegister_New

    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim ins As Integer = 0
    Dim dt As DataTable
    Dim da As OleDbDataAdapter
    Dim dtGrid As New DataTable
    Dim dtGridTot As New DataTable
    Dim objTax As New frmGSTTax_New
    Dim objCreditCard As New frmCreditCardAdj
    Dim objCheaque As New frmChequeAdj
    Dim editFlag As Boolean = False
    Dim stateId As Integer
    Dim Round_Off As String = GetAdmindbSoftValue("ROUNDOFF_GSTGENERAL_NEW", "N")

    Private Sub GridTotal()
        Dim drk As DataRow
        dtGridTot.Rows.Clear()
        drk = dtGridTot.NewRow
        If dtGrid.Rows.Count > 0 Then
            drk.Item("DESCRIPTION") = "Total"
            'drk.Item("PCS") = dtGrid.Compute("SUM(PCS)", Nothing)
            'drk.Item("WEIGHT") = dtGrid.Compute("SUM(WEIGHT)", Nothing)
            drk.Item("AMOUNT") = dtGrid.Compute("SUM(AMOUNT)", Nothing)
            drk.Item("SGST") = dtGrid.Compute("SUM(SGST)", Nothing)
            drk.Item("CGST") = dtGrid.Compute("SUM(CGST)", Nothing)
            drk.Item("IGST") = dtGrid.Compute("SUM(IGST)", Nothing)
            drk.Item("TDS") = dtGrid.Compute("SUM(TDS)", Nothing)
            drk.Item("OTHERS") = dtGrid.Compute("SUM(OTHERS)", Nothing)
        Else
            drk.Item("DESCRIPTION") = "Total"
            'drk.Item("PCS") = 0
            'drk.Item("WEIGHT") = 0
            drk.Item("AMOUNT") = 0
            drk.Item("SGST") = 0
            drk.Item("CGST") = 0
            drk.Item("IGST") = 0
            drk.Item("OTHERS") = 0
            drk.Item("TDS") = 0
        End If
        dtGridTot.Rows.Add(drk)
        GridViewGstTot.DataSource = Nothing
        GridViewGstTot.DataSource = dtGridTot
        StyleGridSASR(GridViewGstTot)
    End Sub

    Public Sub funcClear()
        ' dtpDate.Value = GetEntryDate(GetServerDate)

        txtRefno.Text = ""
        txtNarr_MAN.Text = ""
        'txtTranno.Text = ""
        objGPack.TextClear(Me)

        txtAdjRoundoff_AMT.Text = "0"
        txtAdjRoundoff_AMT.Clear()

        txtAdjCash_AMT.Text = ""
        txtAdjCredit_AMT.Text = ""
        txtAdjCheque_AMT.Text = ""
        txtDescrip_MAN.Text = ""
        txthsn.Text = ""
        'txtPcs_NUM.Text = ""
        txtWeight_WET.Text = ""
        txtamt_AMT.Text = ""
        'txtRate_AMT.Text = ""
        txtGstPer_AMT.Text = ""
        txtSgst_per.Text = ""
        txtSgst_AMT.Text = ""
        txtCgst_per.Text = ""
        txtCgst_AMT.Text = ""
        txtIgst_per.Text = ""
        txtIgst_AMT.Text = ""
        txtTDSamt.Text = ""
        txtTDSper_amt.Text = ""
        cmbGstClaim.Text = "Yes"
        cmbAcName_MAN.Text = ""
        CmbContra_Man.Text = ""
        CmbCostcentre_OWN.Text = ""
        'cmbDepartment.Text = ""
        cmbStateName_MAN.Text = ""
        txtGsttot_Amt.Text = ""
        'txtSupName.Text = ""
        lblEditKeyNo.Text = ""
        txtGSTIN.Text = ""
        txtAddress1.Text = ""
        txtAddress2.Text = ""
        chkRCM.Checked = False
        lblEditKeyNo.Visible = False
        Dim tdate As Date = Format(dtpDate.Value, "yyyy-MM-dd").ToString
        Dim enddate As Date = objGPack.GetSqlValue("SELECT ENDDATE FROM " & cnAdminDb & "..DBMASTER WHERE '" & tdate & "' <= 'STARTDATE'  AND DBNAME='" & cnStockDb & "'  ", , , tran)
        If enddate <> Nothing And Format(enddate, "yyyy-MM-dd") < Format(dtpDate.Value, "yyyy-MM-dd") Then
            dtpDate.Text = Format(enddate, "dd-MM-yyyy")
            dtpRefDate.Text = Format(enddate, "dd-MM-yyyy")


        Else
            dtpDate.Value = GetEntryDate(GetServerDate)
            Dim rdate As Date
            dtpRefDate.Value = GetEntryDate(GetServerDate)
            ' dtpRefDate.Text = Format(rdate, "dd-MM-yyyy")
        End If

        txtAdjCredit_AMT.Clear()

        dtGrid = New DataTable
        objTax = New frmGSTTax_New
        With dtGrid
            Dim col As New DataColumn("KEYNO")
            col.AutoIncrement = True
            col.AutoIncrementSeed = 1
            col.AutoIncrementStep = 1
            .Columns.Add(col)
            .Columns.Add("DESCRIPTION", GetType(String))
            .Columns.Add("HSN", GetType(String))
            '.Columns.Add("PCS", GetType(Integer))
            '.Columns.Add("WEIGHT", GetType(Double))
            '.Columns.Add("RATE", GetType(Double))
            .Columns.Add("AMOUNT", GetType(Double))
            .Columns.Add("TAX", GetType(Double))
            .Columns.Add("TAXAMOUNT", GetType(Double))
            .Columns.Add("SGST_PER", GetType(Double))
            .Columns.Add("SGST", GetType(Double))
            .Columns.Add("CGST_PER", GetType(Double))
            .Columns.Add("CGST", GetType(Double))
            .Columns.Add("IGST_PER", GetType(Double))
            .Columns.Add("IGST", GetType(Double))
            .Columns.Add("OTHERS", GetType(Double))
            .Columns.Add("STATEID", GetType(Integer))
            .Columns.Add("ACCODE", GetType(String))
            .Columns.Add("COSTID", GetType(String))
            '.Columns.Add("DEPARTID", GetType(Integer))
            .Columns.Add("BILLNO", GetType(String))
            .Columns.Add("BILLDATE", GetType(String))
            .Columns.Add("GSTCLAIM", GetType(String))
            .Columns.Add("GSTNO", GetType(String))
            .Columns.Add("ADDRESS1", GetType(String))
            .Columns.Add("ADDRESS2", GetType(String))
            .Columns.Add("TDSPER", GetType(Double))
            .Columns.Add("TDS", GetType(Double))
        End With
        GridViewGst.DataSource = Nothing
        GridViewGst.DataSource = dtGrid
        StyleGridSASR(GridViewGst)

        'dtGrid.Rows.Clear()
        'For i As Integer = 1 To 30
        '    dtGrid.Rows.Add()
        'Next
        'dtGrid.AcceptChanges()
        funcTranno()
        dtGridTot = dtGrid.Copy
        GridViewGstTot.DataSource = Nothing
        GridViewGstTot.DataSource = dtGridTot
        GridTotal()
        GridViewGst.Focus()
        dtpDate.Focus()
    End Sub

    Private Sub frmGstRegister_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Leave

    End Sub
    Private Sub frmGstRegister_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGen.Left, Me.tabGen.Top, Me.tabGen.Width, Me.tabGen.Height))
        pnlGroupFilter.Location = New Point((ScreenWid - pnlGroupFilter.Width) / 2, ((ScreenHit - 128) - pnlGroupFilter.Height) / 2)
        dtpDate.Select()
        cmbGstClaim.Items.Add("Yes")
        cmbGstClaim.Items.Add("No")
        cmbGstClaim.Text = "Yes"

        Me.BackColor = Color.Lavender
        tabGen.BackColor = Color.Lavender
        pnlGroupFilter.BackColor = Color.Lavender
        Panel1.BackColor = Color.Lavender
        Panel2.BackColor = Color.Lavender
        Panel3.BackColor = Color.Lavender
        Panel4.BackColor = Color.Lavender
        Panel5.BackColor = Color.Lavender
        GroupBox1.BackColor = Color.Lavender
        GroupBox2.BackColor = Color.Lavender
        GroupBox3.BackColor = Color.Lavender
        funcClear()
        StateName()
        AcName()
        funcdepart()
        funccostcentre()
        contra()
        tabMain.SelectedTab = tabGen

    End Sub
    Function contra()
        strSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD ORDER BY ACNAME"
        objGPack.FillCombo(strSql, CmbContra_Man, , False)
    End Function
    Function funcdepart()
        'strSql = " SELECT DEPARTNAME FROM " & cnAdminDb & "..Department ORDER BY DEPARTNAME"
        'objGPack.FillCombo(strSql, cmbDepartment, , False)
    End Function

    Function funccostcentre()
        strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..Costcentre ORDER BY COSTNAME"
        objGPack.FillCombo(strSql, CmbCostcentre_OWN, , False)
    End Function
    Public Function funcTranno() As Integer
        Dim Trano_inc As Integer = 0
        Dim Trano As String
        cmd = New OleDbCommand("SELECT TOP 1 TRANNO FROM " & cnStockDb & "..GSTREGISTER ORDER BY TRANNO DESC", cn)
        dt = New DataTable
        da = New OleDbDataAdapter(cmd)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            Trano = dt.Rows(0).Item("TRANNO").ToString()
            Trano_inc = Val(Val(Trano) + 1)
            txtTranno.Text = Trano_inc.ToString()
        Else
            txtTranno.Text = "1"
        End If
        Return Val(txtTranno.Text.ToString)
    End Function

    Private Sub frmGstRegister_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Enter Then
            If cmbGstClaim.Focused Then Exit Sub
            If txtamt_AMT.Focused Then Exit Sub
            If GridViewGst.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        ElseIf e.KeyCode = Keys.Escape Then
            If txtDescrip_MAN.Focused Then
                Dim Amt As Double
                If Not GridViewGstTot Is Nothing Then
                    Amt = Val(GridViewGstTot.Rows(0).Cells("AMOUNT").Value.ToString) _
                        + Val(GridViewGstTot.Rows(0).Cells("IGST").Value.ToString) _
                        + Val(GridViewGstTot.Rows(0).Cells("CGST").Value.ToString) _
                        + Val(GridViewGstTot.Rows(0).Cells("SGST").Value.ToString) _
                        + Val(GridViewGstTot.Rows(0).Cells("OTHERS").Value.ToString) _
                    - Val(GridViewGstTot.Rows(0).Cells("TDS").Value.ToString)

                    '  - Val(txtTDSamt.Text).ToString

                    If Round_Off <> "N" Then
                        txtAdjRoundoff_AMT.Text = CalcRoundoffAmt(Amt, Round_Off)
                        ' txtAdjRoundoff_AMT.Text = Math.Round(Math.Round(Amt, 0) - Math.Round(Amt, 2), 2)
                        If Val(txtAdjRoundoff_AMT.Text) <> 0 Then
                            Amt += Val(txtAdjRoundoff_AMT.Text)
                        End If
                    End If
                    txtAdjCredit_AMT.Text = Format(Amt, "0.00")

                    txtSgst_per.Text = ""
                    txtCgst_per.Text = ""
                    txtIgst_per.Text = ""
                    txtGstPer_AMT.Text = ""
                    txtGsttot_Amt.Text = ""
                    txtTDSper_amt.Text = ""
                    txtTDSamt.Text = ""

                End If
                txtNarr_MAN.Focus()
                txtNarr_MAN.Select()
                Exit Sub
            ElseIf txtNarr_MAN.Focused Then
                txtNarr_MAN.Focus()
                txtNarr_MAN.Select()
            Else
                dtpDate.Focus()
                tabMain.SelectedTab = tabGen
            End If
        End If
    End Sub
    Private Sub StyleGridSASR(ByVal GridViewGst As DataGridView)
        With GridViewGst
            .Columns("ACCODE").Visible = False
            .Columns("STATEID").Visible = False

            '.Columns("WEIGHT").Visible = False
            .Columns("SGST_PER").Visible = False
            .Columns("CGST_PER").Visible = False
            .Columns("IGST_PER").Visible = False
            .Columns("TAX").Visible = False
            .Columns("TAXAMOUNT").Visible = False
            '.Columns("DEPARTID").Visible = False
            .Columns("COSTID").Visible = False
            .Columns("BILLNO").Visible = False
            .Columns("BILLDATE").Visible = False
            .Columns("TDSPER").Visible = False
            .Columns("TDS").Visible = False

            .Columns("DESCRIPTION").Width = txtDescrip_MAN.Width + 2
            .Columns("HSN").Width = txthsn.Width + 0
            '.Columns("PCS").Width = txtPcs_NUM.Width + 0
            '.Columns("RATE").Width = txtRate_AMT.Width + 0
            .Columns("AMOUNT").Width = txtamt_AMT.Width + 0
            .Columns("TAX").Width = txtGstPer_AMT.Width + 0
            .Columns("TAXAMOUNT").Width = txtGsttot_Amt.Width + 0
            .Columns("SGST_PER").Width = txtSgst_per.Width + 0
            .Columns("SGST").Width = txtSgst_AMT.Width + 0
            .Columns("CGST_PER").Width = txtCgst_per.Width + 0
            .Columns("CGST").Width = txtCgst_AMT.Width + 0
            .Columns("IGST_PER").Width = txtIgst_per.Width + 0
            .Columns("IGST").Width = txtIgst_AMT.Width + 0
            .Columns("OTHERS").Width = txtOthers_AMT.Width + 0
            .Columns("STATEID").Width = cmbStateName_MAN.Width
            .Columns("ACCODE").Width = cmbAcName_MAN.Width + 0
            '.Columns("DEPARTID").Width = cmbDepartment.Width + 0
            .Columns("BILLNO").Width = txtRefno.Width + 0
            .Columns("BILLDATE").Width = dtpRefDate.Width + 0
            .Columns("GSTCLAIM").Width = cmbGstClaim.Width + 4
            '.Columns("WEIGHT").Width = txtWeight_WET.Width + 0
            .Columns("TDSPER").Width = txtTDSper_amt.Width + 0
            .Columns("TDS").Width = txttds.Width + 0

            '.Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            '.Columns("RATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("TAX").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("TAXAMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("SGST_PER").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("SGST").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("CGST_PER").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("CGST").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("IGST_PER").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("IGST").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("OTHERS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("TDSPER").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("TDS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            '.Columns("RATE").DefaultCellStyle.Format = "0.00"
            .Columns("AMOUNT").DefaultCellStyle.Format = "0.00"
            .Columns("TAX").DefaultCellStyle.Format = "0.00"
            .Columns("TAXAMOUNT").DefaultCellStyle.Format = "0.00"
            .Columns("SGST_PER").DefaultCellStyle.Format = "0.00"
            .Columns("SGST").DefaultCellStyle.Format = "0.00"
            .Columns("CGST_PER").DefaultCellStyle.Format = "0.00"
            .Columns("CGST").DefaultCellStyle.Format = "0.00"
            .Columns("IGST_PER").DefaultCellStyle.Format = "0.00"
            .Columns("IGST").DefaultCellStyle.Format = "0.00"
            .Columns("OTHERS").DefaultCellStyle.Format = "0.00"
            .Columns("TDSPER").DefaultCellStyle.Format = "0.00"
            .Columns("TDS").DefaultCellStyle.Format = "0.00"
            If editFlag = True And GridViewGst.Name <> "GridViewGstTot" Then
                If .Columns.Contains("SNO") Then
                    .Columns("SNO").Visible = False
                End If
                If .Columns.Contains("TRANNO") Then
                    .Columns("TRANNO").Visible = False
                End If
                If .Columns.Contains("TRANDATE") Then
                    .Columns("TRANDATE").Visible = False
                End If
                If .Columns.Contains("STATE") Then
                    .Columns("STATE").Visible = False
                End If
                If .Columns.Contains("ACNAME") Then
                    .Columns("ACNAME").Visible = False
                End If
                '.Columns("TRANNO").Visible = False
                '.Columns("TRANDATE").Visible = False
                '.Columns("STATE").Visible = False
                '.Columns("ACNAME").Visible = False
            End If
            If GridViewGst.Name = "GridViewGstTot" Then
                .DefaultCellStyle.Font = New Font("verdana", 9, FontStyle.Bold)
                .DefaultCellStyle.ForeColor = Color.Red
            End If
        End With
    End Sub

    Function FUNCSAVE()
        If Not GridViewGst.Rows.Count > 0 Then Exit Function
        Dim accode As String = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbAcName_MAN.Text & "'", , , tran)
        Dim contra As String = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & CmbContra_Man.Text & "'", , , tran)
        Dim OthersId As String = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = 'OTHER EXPENSES'", , , tran)
        Dim stateid As String = objGPack.GetSqlValue("SELECT STATEID FROM " & cnAdminDb & "..STATEMAST WHERE STATENAME= '" & cmbStateName_MAN.Text & "' ", , , tran)
        'Dim departid As String = objGPack.GetSqlValue("SELECT DEPARTID FROM " & cnAdminDb & "..DEPARTMENT WHERE DEPARTNAME= '" & cmbDepartment.Text & "' ", , , tran)
        Dim departid As String = ""
        Dim COSTID As String = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME= '" & CmbCostcentre_OWN.Text & "' ", , , tran)
        Try
            Dim Tranno As Integer
            Dim Batchno As String = ""
            Dim crAccode As String = ""
            Dim drAccode As String = ""
            'strSql = "SELECT * FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME='SUNDRY CREDITORS'"
            'crAccode = objGPack.GetSqlValue(strSql, "", "")
            'If crAccode = "" Then
            '    strSql = "SELECT * FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE='CRS'"
            '    crAccode = objGPack.GetSqlValue(strSql, "", "")
            'End If
            'If crAccode = "" Then
            '    MsgBox("SUNDRY CREDITORS Accode not found in master.", MsgBoxStyle.Information) : Exit Sub
            'End If

            crAccode = contra
            Tranno = funcTranno()
            tran = Nothing
            tran = cn.BeginTransaction
            Batchno = GetNewBatchno(cnCostId, GetServerDate(tran), tran)
            For Each ro As DataGridViewRow In GridViewGst.Rows
                Dim Sno As String = GetNewSno(TranSnoType.GSTREGISTERCODE, tran)
                Dim refDateAr As String() = ro.Cells("BILLDATE").Value.ToString.Split("-")
                Dim refDate As Date
                If refDateAr.Length = 3 Then
                    refDate = New Date(refDateAr(2).ToString, refDateAr(1).ToString, refDateAr(0).ToString)
                End If
                strSql = " INSERT INTO " & cnStockDb & "..GSTREGISTER(SNO,BATCHNO,TRANDATE,TRANNO,DESCRIPTION,HSN,PCS,WEIGHT,RATE,AMOUNT,"
                strSql += vbCrLf + " TAX,TAXAMOUNT,SGST_PER,SGST,CGST_PER,CGST,IGST_PER,IGST,STATEID"
                strSql += vbCrLf + " ,ACCODE,CONTRA,SUPNAME,REFNO,REFDATE,GSTCLAIM"
                strSql += vbCrLf + " ,GSTIN,ADDRESS1,ADDRESS2/*,DEPARTID*/,COSTID,TDSPER,TDS,OTHERAMT,RCM,RNDOFF,COMPANYID"
                strSql += vbCrLf + " )"
                strSql += vbCrLf + " VALUES("
                strSql += vbCrLf + " '" & Sno & "'"
                strSql += vbCrLf + " ,'" & Batchno & "'"
                strSql += vbCrLf + " ,'" & dtpDate.Value.ToString("yyyy-MM-dd") & "'"
                strSql += vbCrLf + " , '" & txtTranno.Text & "' "
                strSql += vbCrLf + " ,'" & ro.Cells("DESCRIPTION").Value.ToString & "'"
                strSql += vbCrLf + " ,'" & ro.Cells("HSN").Value.ToString & "'"
                'strSql += vbCrLf + " ,'" & Val(ro.Cells("PCS").Value.ToString) & "'"
                'strSql += vbCrLf + " ,'" & Val(ro.Cells("WEIGHT").Value.ToString) & "'"
                'strSql += vbCrLf + " ,'" & Val(ro.Cells("RATE").Value.ToString) & "'"
                strSql += vbCrLf + " ,'" & 0 & "'"
                strSql += vbCrLf + " ,'" & 0 & "'"
                strSql += vbCrLf + " ,'" & 0 & "'"
                strSql += vbCrLf + " ,'" & Val(ro.Cells("AMOUNT").Value.ToString) & "'"
                strSql += vbCrLf + " ,'" & Val(ro.Cells("TAX").Value.ToString) & "'"
                strSql += vbCrLf + " ,'" & Val(ro.Cells("TAXAMOUNT").Value.ToString) & "'"
                strSql += vbCrLf + " ,'" & Val(ro.Cells("SGST_PER").Value.ToString) & "'"
                strSql += vbCrLf + " ,'" & Val(ro.Cells("SGST").Value.ToString) & "' "
                strSql += vbCrLf + " ,'" & Val(ro.Cells("CGST_PER").Value.ToString) & "'"
                strSql += vbCrLf + " ,'" & Val(ro.Cells("CGST").Value.ToString) & "'"
                strSql += vbCrLf + " ,'" & Val(ro.Cells("IGST_PER").Value.ToString) & "'"
                strSql += vbCrLf + " ,'" & Val(ro.Cells("IGST").Value.ToString) & "'"
                strSql += vbCrLf + " ,'" & stateid & "'"
                strSql += vbCrLf + " ,'" & accode & "'"
                strSql += vbCrLf + " ,'" & contra & "'"
                strSql += vbCrLf + " ,''"
                If ro.Cells("BILLNO").Value.ToString = "" Then
                    strSql += vbCrLf + " ,'" & GridViewGst.Rows(0).Cells("BILLNO").Value.ToString & "'"
                Else
                    strSql += vbCrLf + " ,'" & ro.Cells("BILLNO").Value.ToString & "'"
                End If
                strSql += vbCrLf + " ,'" & refDate.ToString("yyyy-MM-dd") & "'"
                strSql += vbCrLf + " ,'" & Mid(ro.Cells("GSTCLAIM").Value.ToString, 1, 1) & "' "
                strSql += vbCrLf + " ,'" & ro.Cells("GSTNO").Value.ToString & "' "
                strSql += vbCrLf + " ,'" & ro.Cells("ADDRESS1").Value.ToString & "' "
                strSql += vbCrLf + " ,'" & ro.Cells("ADDRESS2").Value.ToString & "' "
                'strSql += vbCrLf + " ,'" & departid & "' "
                strSql += vbCrLf + " ,'" & COSTID & "' "
                strSql += vbCrLf + " ,'" & ro.Cells("TDSPER").Value.ToString & "' "
                strSql += vbCrLf + " ,'" & ro.Cells("TDS").Value.ToString & "' "
                strSql += vbCrLf + " ,'" & Val(ro.Cells("OTHERS").Value.ToString) & "' "
                strSql += vbCrLf + " ,'" & IIf(chkRCM.Checked, "Y", "N") & "' "
                strSql += vbCrLf + " ,'" & Val(txtAdjRoundoff_AMT.Text.ToString) & "'"
                'strsql +=vbcrlf +=",'"
                strSql += vbCrLf + " ,'" & strCompanyId & "'"
                strSql += vbCrLf + ")"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnHOCostId)
            Next

            'Insert into Acctran
            Dim Trfamount As Double = 0
            Dim tdsAccode As String = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..TDSCATEGORY WHERE TDSCATID = '" & objTax.straccode & "'", , , tran)
            drAccode = accode

            'If Val(txtAdjCash_AMT.Text) > 0 Then
            '    InsertIntoAccTran(Tranno, "D", "CASH", Val(txtAdjCash_AMT.Text) + Val(dtGrid.Compute("SUM(TDS)", Nothing).ToString), "JE", drAccode, Batchno, , , , , , , txtNarr_MAN.Text.ToString, "GRE")
            'End If
            'If Val(txtAdjCheque_AMT.Text) > 0 Then
            '    For Each ro As DataRow In objCheaque.dtGridCheque.Rows
            '        Dim BANKCODE As String = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & ro!DEFAULTBANK.ToString & "'", , , tran).ToString
            '        InsertIntoAccTran(Tranno, "D", BANKCODE, Val(ro!AMOUNT.ToString) + Val(dtGrid.Compute("SUM(TDS)", Nothing).ToString), "CH", drAccode _
            '         , Batchno, , , ro!CHEQUENO.ToString, ro!DATE.ToString, , ro!BANKNAME.ToString)
            '    Next
            'End If


            'InsertIntoAccTran(Tranno, "D", crAccode, Val(txtAdjCredit_AMT.Text) + Val(dtGrid.Compute("SUM(TDS)", Nothing).ToString), "JE", drAccode, Batchno, , , , , , , txtNarr_MAN.Text.ToString, "GRE")

            'Trfamount = Val(dtGrid.Compute("SUM(TDS)", Nothing).ToString)
            'InsertIntoAccTran(Tranno, "D", tdsAccode, Trfamount, "JE", drAccode, Batchno, , , , , , , txtNarr_MAN.Text.ToString, "GRE")

            'Trfamount = Val(dtGrid.Compute("SUM(AMOUNT)", Nothing).ToString)
            'InsertIntoAccTran(Tranno, "C", drAccode, Trfamount, "JE", crAccode, Batchno, , , , , , , txtNarr_MAN.Text.ToString, "GRE")

            'Trfamount = Val(dtGrid.Compute("SUM(IGST)", Nothing).ToString)
            'InsertIntoAccTran(Tranno, "C", "IGST", Trfamount, "JE", crAccode, Batchno, , , , , , , txtNarr_MAN.Text.ToString, "GRE")

            'Trfamount = Val(dtGrid.Compute("SUM(CGST)", Nothing).ToString)
            'InsertIntoAccTran(Tranno, "C", "CGST", Trfamount, "JE", crAccode, Batchno, , , , , , , txtNarr_MAN.Text.ToString, "GRE")

            'Trfamount = Val(dtGrid.Compute("SUM(SGST)", Nothing).ToString)
            'InsertIntoAccTran(Tranno, "C", "SGST", Trfamount, "JE", crAccode, Batchno, , , , , , , txtNarr_MAN.Text.ToString, "GRE")

            'If chkRCM.Checked = True Then
            '    Trfamount = Val(dtGrid.Compute("SUM(IGST)", Nothing).ToString)
            '    InsertIntoAccTran(Tranno, "D", "IGST", Trfamount, "JE", crAccode, Batchno, , , , , , , txtNarr_MAN.Text.ToString, "GRE")
            '    InsertIntoAccTran(Tranno, "C", crAccode, Trfamount, "JE", "IGST", Batchno, , , , , , , txtNarr_MAN.Text.ToString, "GRE")

            '    Trfamount = Val(dtGrid.Compute("SUM(CGST)", Nothing).ToString)
            '    InsertIntoAccTran(Tranno, "D", "CGST", Trfamount, "JE", crAccode, Batchno, , , , , , , txtNarr_MAN.Text.ToString, "GRE")
            '    InsertIntoAccTran(Tranno, "C", crAccode, Trfamount, "JE", "IGST", Batchno, , , , , , , txtNarr_MAN.Text.ToString, "GRE")

            '    Trfamount = Val(dtGrid.Compute("SUM(SGST)", Nothing).ToString)
            '    InsertIntoAccTran(Tranno, "D", "SGST", Trfamount, "JE", crAccode, Batchno, , , , , , , txtNarr_MAN.Text.ToString, "GRE")
            '    InsertIntoAccTran(Tranno, "C", crAccode, Trfamount, "JE", "IGST", Batchno, , , , , , , txtNarr_MAN.Text.ToString, "GRE")
            'End If



            'Trfamount = Val(dtGrid.Compute("SUM(AMOUNT)", Nothing).ToString)
            'InsertIntoAccTran(Tranno, "C", crAccode, Trfamount, "JE", drAccode, Batchno, , , , , , , txtNarr_MAN.Text.ToString, "GRE")

            'Trfamount = Val(dtGrid.Compute("SUM(AMOUNT)", Nothing).ToString) + Val(dtGrid.Compute("SUM(IGST)", Nothing).ToString) _
            '+ Val(dtGrid.Compute("SUM(CGST)", Nothing).ToString) + Val(dtGrid.Compute("SUM(SGST)", Nothing).ToString) _
            '- Val(dtGrid.Compute("SUM(SGST)", Nothing).ToString)

            'Trfamount = Val(txtAdjCredit_AMT.Text.ToString)
            'InsertIntoAccTran(Tranno, "D", drAccode, Trfamount, "JE", crAccode, Batchno, , , , , , , txtNarr_MAN.Text.ToString, "GRE")

            'Trfamount = Val(dtGrid.Compute("SUM(TDS)", Nothing).ToString)
            'InsertIntoAccTran(Tranno, "D", tdsAccode, Trfamount, "JE", drAccode, Batchno, , , , , , , txtNarr_MAN.Text.ToString, "GRE")

            'Trfamount = Val(dtGrid.Compute("SUM(IGST)", Nothing).ToString)
            'InsertIntoAccTran(Tranno, "C", "IGST", Trfamount, "JE", crAccode, Batchno, , , , , , , txtNarr_MAN.Text.ToString, "GRE")

            'Trfamount = Val(dtGrid.Compute("SUM(CGST)", Nothing).ToString)
            'InsertIntoAccTran(Tranno, "C", "CGST", Trfamount, "JE", crAccode, Batchno, , , , , , , txtNarr_MAN.Text.ToString, "GRE")

            'Trfamount = Val(dtGrid.Compute("SUM(SGST)", Nothing).ToString)
            'InsertIntoAccTran(Tranno, "C", "SGST", Trfamount, "JE", crAccode, Batchno, , , , , , , txtNarr_MAN.Text.ToString, "GRE")

            'Trfamount = Val(dtGrid.Compute("SUM(OTHERS)", Nothing).ToString)
            'InsertIntoAccTran(Tranno, "C", OthersId, Trfamount, "JE", crAccode, Batchno, , , , , , , txtNarr_MAN.Text.ToString, "GRE")


            Trfamount = Val(dtGrid.Compute("SUM(AMOUNT)", Nothing).ToString)
            InsertIntoAccTran(Tranno, "D", crAccode, Trfamount, "JE", drAccode, Batchno, txtRefno.Text, dtpRefDate.Value.ToString("yyyy-MM-dd"), , , , , txtNarr_MAN.Text.ToString, "GRE", , , , , , departid)

            Trfamount = Val(dtGrid.Compute("SUM(IGST)", Nothing).ToString)
            InsertIntoAccTran(Tranno, "D", "IGST", Trfamount, "JE", crAccode, Batchno, txtRefno.Text, dtpRefDate.Value.ToString("yyyy-MM-dd"), , , , , txtNarr_MAN.Text.ToString, "GRE", , , , , , departid)

            Trfamount = Val(dtGrid.Compute("SUM(CGST)", Nothing).ToString)
            InsertIntoAccTran(Tranno, "D", "CGST", Trfamount, "JE", crAccode, Batchno, txtRefno.Text, dtpRefDate.Value.ToString("yyyy-MM-dd"), , , , , txtNarr_MAN.Text.ToString, "GRE", , , , , , departid)

            Trfamount = Val(dtGrid.Compute("SUM(SGST)", Nothing).ToString)
            InsertIntoAccTran(Tranno, "D", "SGST", Trfamount, "JE", crAccode, Batchno, txtRefno.Text, dtpRefDate.Value.ToString("yyyy-MM-dd"), , , , , txtNarr_MAN.Text.ToString, "GRE", , , , , , departid)

            Trfamount = Val(dtGrid.Compute("SUM(OTHERS)", Nothing).ToString)
            InsertIntoAccTran(Tranno, "D", OthersId, Trfamount, "JE", crAccode, Batchno, txtRefno.Text, dtpRefDate.Value.ToString("yyyy-MM-dd"), , , , , txtNarr_MAN.Text.ToString, "GRE", , , , , , departid)

            Trfamount = Val(dtGrid.Compute("SUM(AMOUNT)", Nothing).ToString) + Val(dtGrid.Compute("SUM(IGST)", Nothing).ToString) _
            + Val(dtGrid.Compute("SUM(CGST)", Nothing).ToString) + Val(dtGrid.Compute("SUM(SGST)", Nothing).ToString) _
            - Val(dtGrid.Compute("SUM(SGST)", Nothing).ToString)

            'Trfamount = Val(txtAdjCredit_AMT.Text.ToString)
            'InsertIntoAccTran(Tranno, "C", drAccode, Trfamount, "JE", crAccode, Batchno, , , , , , , txtNarr_MAN.Text.ToString, "GRE")

            Trfamount = Val(txtAdjCredit_AMT.Text.ToString) + Val(dtGrid.Compute("SUM(TDS)", Nothing).ToString)
            InsertIntoAccTran(Tranno, "C", drAccode, Trfamount, "JE", crAccode, Batchno, , dtpRefDate.Value.ToString("yyyy-MM-dd"), , , , , txtNarr_MAN.Text.ToString, "GRE", , , , , , departid)

            Trfamount = Val(dtGrid.Compute("SUM(TDS)", Nothing).ToString)
            InsertIntoAccTran(Tranno, "D", drAccode, Trfamount, "JE", tdsAccode, Batchno, , dtpRefDate.Value.ToString("yyyy-MM-dd"), , , , , txtNarr_MAN.Text.ToString, "GRE", , , , , , departid)

            Trfamount = Val(dtGrid.Compute("SUM(TDS)", Nothing).ToString)
            InsertIntoAccTran(Tranno, "C", tdsAccode, Trfamount, "JE", drAccode, Batchno, , dtpRefDate.Value.ToString("yyyy-MM-dd"), , , , , txtNarr_MAN.Text.ToString, "GRE", , , , , , departid)

            If Val(txtAdjRoundoff_AMT.Text.ToString) <> 0 Then
                If Val(txtAdjRoundoff_AMT.Text) > 0 Then
                    InsertIntoAccTran(Tranno, IIf(Val(txtAdjRoundoff_AMT.Text) > 0, "D", "C"),
                    "RNDOFF", Val(txtAdjRoundoff_AMT.Text), "RO", IIf(Val(txtAdjRoundoff_AMT.Text) > 0, drAccode, crAccode), Batchno, , , , , , , txtNarr_MAN.Text.ToString, "GRE", , , , , , departid)
                Else
                    InsertIntoAccTran(Tranno, IIf(Val(txtAdjRoundoff_AMT.Text) < 0, "C", "D"),
                    "RNDOFF", Val(txtAdjRoundoff_AMT.Text), "RO", IIf(Val(txtAdjRoundoff_AMT.Text) > 0, crAccode, drAccode), Batchno, , , , , , , txtNarr_MAN.Text.ToString, "GRE", , , , , , departid)
                End If
            End If

            'Dim bAmt As Decimal = Val(txtBalAmount_AMT.Text) - Val(settledAmt)
            'bAmt = Math.Round(BalAmount - Val(txtBalAmount_AMT.Text), 2)
            'If bAmt <> 0 Then
            '    If bAmt < 0 Then
            '        InsertIntoAccTran(Tranno, "C", "RNDOFF", bAmt, 0, 0, 0, 0, "RO", _Accode, , , , , , , txtRemark.Text, , "RO")
            '        InsertIntoAccTran(Tranno, "D", _Accode, bAmt, 0, 0, 0, 0, "RO", "RNDOFF", , , , , , , txtRemark.Text, , "RO")
            '    Else
            '        InsertIntoAccTran(Tranno, "D", "RNDOFF", bAmt, 0, 0, 0, 0, "RO", _Accode, , , , , , , txtRemark.Text, , "RO")
            '        InsertIntoAccTran(Tranno, "C", _Accode, bAmt, 0, 0, 0, 0, "RO", "RNDOFF", , , , , , , txtRemark.Text, , "RO")
            '    End If
            'End If

            'If chkRCM.Checked Then
            '    Trfamount = Val(dtGrid.Compute("SUM(IGST)", Nothing).ToString)
            '    InsertIntoAccTran(Tranno, "C", "IGST", Trfamount, "JE", crAccode, Batchno, , , , , , , txtNarr_MAN.Text.ToString, "GRE")

            '    Trfamount = Val(dtGrid.Compute("SUM(CGST)", Nothing).ToString)
            '    InsertIntoAccTran(Tranno, "C", "CGST", Trfamount, "JE", crAccode, Batchno, , , , , , , txtNarr_MAN.Text.ToString, "GRE")

            '    Trfamount = Val(dtGrid.Compute("SUM(SGST)", Nothing).ToString)
            '    InsertIntoAccTran(Tranno, "C", "SGST", Trfamount, "JE", crAccode, Batchno, , , , , , , txtNarr_MAN.Text.ToString, "GRE")

            '    Trfamount = Val(dtGrid.Compute("SUM(SGST)", Nothing).ToString) + Val(dtGrid.Compute("SUM(CGST)", Nothing).ToString) + Val(dtGrid.Compute("SUM(IGST)", Nothing).ToString)
            '    InsertIntoAccTran(Tranno, "D", crAccode, Trfamount, "JE", drAccode, Batchno, , , , , , , txtNarr_MAN.Text.ToString, "GRE")
            'End If


            ''Like Tds Entry
            'Trfamount = Val(dtGrid.Compute("SUM(TDS)", Nothing).ToString)
            'InsertIntoAccTran(Tranno, "D", drAccode, Trfamount, "JE", tdsAccode, Batchno, , , , , , , txtNarr_MAN.Text.ToString, "GRE")

            Dim debCrNotTally As String = ""
            Dim balAmt As Double = Val(objGPack.GetSqlValue("SELECT SUM(CASE WHEN TRANMODE = 'C' THEN isnull(AMOUNT,0) ELSE isnull(AMOUNT,0)*(-1) END) AS BALANCE FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = '" & Batchno & "'", , "0", tran))
            If Math.Abs(balAmt) > 0 Then
                tran.Rollback()
                tran.Dispose()
                tran = Nothing
                MsgBox("Credit Debit Not Tally : " + balAmt.ToString, MsgBoxStyle.Information)
                Exit Function
            End If


            tran.Commit()
            tran = Nothing
            MsgBox("Saved Successfully...", MsgBoxStyle.Information)
            Dim pBatchno As String = Batchno
            Dim pBillDate As Date = dtpDate.Value.ToString("yyyy-MM-dd") 'dtpDate.Value
            Dim pParamStr As String = ""
            ' btnNew_Click(Me, New EventArgs)
            If IO.File.Exists(Application.StartupPath & "\BILLPRINT.EXE") Then
                Dim write As IO.StreamWriter
                Dim prnmemsuffix As String = ""
                If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId
                Dim memfile As String = "\BillPrint" + prnmemsuffix.Trim & ".mem"

                write = IO.File.CreateText(Application.StartupPath & memfile)
                write.WriteLine(LSet("TYPE", 15) & ":ACC")
                pParamStr += LSet("TYPE", 15) & ":ACC" & ";"
                write.WriteLine(LSet("BATCHNO", 15) & ":" & pBatchno)
                pParamStr += LSet("BATCHNO", 15) & ":" & pBatchno & ";"
                write.WriteLine(LSet("TRANDATE", 15) & ":" & pBillDate.ToString("yyyy-MM-dd"))
                pParamStr += LSet("TRANDATE", 15) & ":" & pBillDate.ToString("yyyy-MM-dd") & ";"
                write.WriteLine(LSet("DUPLICATE", 15) & ":N")
                pParamStr += LSet("DUPLICATE", 15) & ":N"
                write.Flush()
                write.Close()
                If EXE_WITH_PARAM = False Then
                    System.Diagnostics.Process.Start(Application.StartupPath & "\BILLPRINT.EXE")
                Else
                    System.Diagnostics.Process.Start(Application.StartupPath & "\BILLPRINT.EXE", pParamStr)
                End If
            End If


            funcClear()
            btnNew_Click(Me, New EventArgs)
            funcTranno()
        Catch ex As Exception
            If Not tran Is Nothing Then
                tran.Rollback()
                tran = Nothing
            End If
            MsgBox(ex.Message & vbCrLf & ex.StackTrace, MsgBoxStyle.Information)
        End Try
    End Function
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click, SaveToolStripMenuItem.Click
        If editFlag = False Then
            FUNCSAVE()
        Else
            funcupdate()
        End If
    End Sub
    Function funcupdate()

        If Not GridViewGst.Rows.Count > 0 Then Exit Function
        dt = New DataTable
        dtGrid = GridViewGst.DataSource
        Dim Tranno As Integer
        Dim Batchno As String = ""
        Dim crAccode As String = ""
        Dim drAccode As String = ""
        Dim trandate As Date
        Dim refdate As Date
        Dim accode As String = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbAcName_MAN.Text & "'", , , tran)
        Dim contra As String = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & CmbContra_Man.Text & "'", , , tran)
        Dim OthersId As String = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = 'OTHER EXPENSES'", , , tran)
        Dim stateid As String = objGPack.GetSqlValue("SELECT STATEID FROM " & cnAdminDb & "..STATEMAST WHERE STATENAME= '" & cmbStateName_MAN.Text & "' ", , , tran)
        'Dim departid As String = objGPack.GetSqlValue("SELECT DEPARTID FROM " & cnAdminDb & "..DEPARTMENT WHERE DEPARTNAME= '" & cmbDepartment.Text & "' ", , , tran)
        Dim departid As String = ""
        Dim COSTID As String = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME= '" & CmbCostcentre_OWN.Text & "' ", , , tran)
        'Dim refDateAr As String() = ro.Cells("BILLDATE").Value.ToString.Split("-")
        'Dim refDate As Date
        'If refDateAr.Length = 3 Then
        '    refDate = New Date(refDateAr(2).ToString, refDateAr(1).ToString, refDateAr(0).ToString)
        'End If
        'Dim AMT As Double = Val(txtAdjCredit_AMT.Text).ToString
        'Dim RNDOFF As Double = Math.Round(AMT, 0) - Math.Round(AMT, 2)
        Dim tdate As Date = Format(dtpDate.Value, "yyyy-MM-dd").ToString
        Try
            For i As Integer = 0 To dtGrid.Rows.Count - 1
                Tranno = dtGrid.Rows(i).Item("TRANNO").ToString
                Batchno = dtGrid.Rows(i).Item("BATCHNO").ToString
                'trandate = dtGrid.Rows(i).Item("trandate").ToString
                'refdate = dtGrid.Rows(i).Item("refdate").ToString
                strSql = "UPDATE " & cnStockDb & "..GSTREGISTER SET"
                strSql += vbCrLf + " TRANNO='" & dtGrid.Rows(i).Item("TRANNO").ToString & "' "
                strSql += vbCrLf + " ,REFNO='" & txtRefno.Text & "' "
                strSql += vbCrLf + " ,trandate='" & tdate & "' " '" & dtGrid.Rows(i).Item("trandate").ToString("yyyy-MM-dd") & "' " '" & dtpTranDate1.Value.ToString("yyyy-MM-dd") & "'"
                strSql += vbCrLf + " ,refdate= '" & dtpRefDate.Value.ToString("yyyy-MM-dd") & "' " '" & dtGrid.Rows(i).Item("billdate").ToString("yyyy-MM-dd") & "' " '" & dtpRefDate.Value.ToString("yyyy-MM-dd") & "'"
                strSql += vbCrLf + " ,BATCHNO='" & dtGrid.Rows(i).Item("BATCHNO") & "' "
                strSql += vbCrLf + " ,AMOUNT='" & dtGrid.Rows(i).Item("AMOUNT") & "' "
                strSql += vbCrLf + " ,HSN='" & dtGrid.Rows(i).Item("HSN") & "' "
                strSql += vbCrLf + " ,TAX='" & dtGrid.Rows(i).Item("TAX") & "' "
                strSql += vbCrLf + " ,TAXAMOUNT='" & dtGrid.Rows(i).Item("TAXAMOUNT") & "' "
                strSql += vbCrLf + " ,SGST='" & dtGrid.Rows(i).Item("SGST") & "' "
                strSql += vbCrLf + " ,SGST_PER='" & dtGrid.Rows(i).Item("SGST_PER") & "' "
                strSql += vbCrLf + " ,CGST='" & dtGrid.Rows(i).Item("CGST") & "' "
                strSql += vbCrLf + " ,CGST_PER='" & dtGrid.Rows(i).Item("CGST_PER") & "' "
                strSql += vbCrLf + " ,IGST='" & dtGrid.Rows(i).Item("IGST") & "' "
                strSql += vbCrLf + " ,IGST_PER='" & dtGrid.Rows(i).Item("IGST_PER") & "' "
                strSql += vbCrLf + " ,ACCODE='" & accode & "' "
                strSql += vbCrLf + " ,CONTRA='" & contra & "' "
                strSql += vbCrLf + " ,stateid='" & stateid & "' "
                'strSql += vbCrLf + " ,departid='" & departid & "' "
                strSql += vbCrLf + " ,costid='" & COSTID & "' "
                strSql += vbCrLf + " ,Address1='" & txtAddress1.Text & "'"
                strSql += vbCrLf + " ,Address2='" & txtAddress2.Text & "'"
                strSql += vbCrLf + " ,TDS='" & dtGrid.Rows(i).Item("TDS") & "'"
                strSql += vbCrLf + " ,TDSPER='" & dtGrid.Rows(i).Item("TDSPER") & "'"
                'strSql += vbCrLf + " ,GSTCLAIM='" & dtGrid.Rows(i).Item("GSTCLAIM") & "'"
                strSql += vbCrLf + " ,RCM='" & dtGrid.Rows(i).Item("RCM") & "'"
                'If chkRCM.Checked Then
                '    strSql += vbCrLf + ",RCM='Y'"
                'Else
                '    strSql += vbCrLf + ",RCM='N'"
                'End If
                strSql += vbCrLf + ",OTHERAMT='" & dtGrid.Rows(i).Item("OTHERS") & "'"
                If dtGrid.Rows(i).Item("GSTCLAIM") = "YES" Then
                    strSql += vbCrLf + ",GSTCLAIM ='Y'  "
                Else
                    strSql += vbCrLf + ",GSTCLAIM ='N'  "
                End If
                strSql += vbCrLf + ",RNDOFF='" & txtAdjRoundoff_AMT.Text & "'"
                strSql += vbCrLf + " WHERE SNO ='" & dtGrid.Rows(i).Item("SNO") & "'"
                'cmd = New OleDbCommand(strSql, cn)
                'cmd.ExecuteNonQuery()
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, COSTID)
            Next

            crAccode = contra
            Dim tdsAccode As String
            If objTax.straccode = "0" Then
                tdsAccode = objGPack.GetSqlValue("select ACCODE  from " & cnStockDb & "..ACCTRAN where batchno ='" & Batchno & "' AND ACCODE in(select accode from " & cnAdminDb & "..TDSCATEGORY)", , , tran)
            End If
            If editFlag Then
                strSql = " DELETE FROM " & cnStockDb & "..ACCTRAN"
                strSql += " WHERE BATCHNO = '" & Batchno & "'"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, COSTID)
            End If
            Dim Trfamount As Double = 0




            ' cmd.ExecuteNonQuery()

            If tdsAccode = Nothing Then
                tdsAccode = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..TDSCATEGORY WHERE TDSCATID = '" & objTax.straccode & "'", , , tran)
            End If

            drAccode = accode
            Trfamount = Val(dtGrid.Compute("SUM(AMOUNT)", Nothing).ToString)

            InsertIntoAccTran(Tranno, "D", crAccode, Trfamount, "JE", drAccode, Batchno, txtRefno.Text, dtpRefDate.Value.ToString("yyyy-MM-dd"), , , , , txtNarr_MAN.Text.ToString, "GRE", , , , , , departid)

            Trfamount = Val(dtGrid.Compute("SUM(IGST)", Nothing).ToString)
            InsertIntoAccTran(Tranno, "D", "IGST", Trfamount, "JE", crAccode, Batchno, txtRefno.Text, dtpRefDate.Value.ToString("yyyy-MM-dd"), , , , , txtNarr_MAN.Text.ToString, "GRE", , , , , , departid)

            Trfamount = Val(dtGrid.Compute("SUM(CGST)", Nothing).ToString)
            InsertIntoAccTran(Tranno, "D", "CGST", Trfamount, "JE", crAccode, Batchno, txtRefno.Text, dtpRefDate.Value.ToString("yyyy-MM-dd"), , , , , txtNarr_MAN.Text.ToString, "GRE", , , , , , departid)

            Trfamount = Val(dtGrid.Compute("SUM(SGST)", Nothing).ToString)
            InsertIntoAccTran(Tranno, "D", "SGST", Trfamount, "JE", crAccode, Batchno, txtRefno.Text, dtpRefDate.Value.ToString("yyyy-MM-dd"), , , , , txtNarr_MAN.Text.ToString, "GRE", , , , , , departid)

            Trfamount = Val(dtGrid.Compute("SUM(OTHERS)", Nothing).ToString)
            InsertIntoAccTran(Tranno, "D", OthersId, Trfamount, "JE", crAccode, Batchno, txtRefno.Text, dtpRefDate.Value.ToString("yyyy-MM-dd"), , , , , txtNarr_MAN.Text.ToString, "GRE", , , , , , departid)

            Trfamount = Val(dtGrid.Compute("SUM(AMOUNT)", Nothing).ToString) + Val(dtGrid.Compute("SUM(IGST)", Nothing).ToString) _
            + Val(dtGrid.Compute("SUM(CGST)", Nothing).ToString) + Val(dtGrid.Compute("SUM(SGST)", Nothing).ToString) _
            - Val(dtGrid.Compute("SUM(SGST)", Nothing).ToString)

            'Trfamount = Val(txtAdjCredit_AMT.Text.ToString)
            'InsertIntoAccTran(Tranno, "C", drAccode, Trfamount, "JE", crAccode, Batchno, , , , , , , txtNarr_MAN.Text.ToString, "GRE")

            Trfamount = Val(txtAdjCredit_AMT.Text.ToString) + Val(dtGrid.Compute("SUM(TDS)", Nothing).ToString)
            InsertIntoAccTran(Tranno, "C", drAccode, Trfamount, "JE", crAccode, Batchno, , , , , , , txtNarr_MAN.Text.ToString, "GRE", , , , , , departid)

            Trfamount = Val(dtGrid.Compute("SUM(TDS)", Nothing).ToString)
            InsertIntoAccTran(Tranno, "D", drAccode, Trfamount, "JE", tdsAccode, Batchno, , , , , , , txtNarr_MAN.Text.ToString, "GRE", , , , , , departid)

            Trfamount = Val(dtGrid.Compute("SUM(TDS)", Nothing).ToString)
            InsertIntoAccTran(Tranno, "C", tdsAccode, Trfamount, "JE", drAccode, Batchno, , , , , , , txtNarr_MAN.Text.ToString, "GRE", , , , , , departid)

            If Val(txtAdjRoundoff_AMT.Text.ToString) <> 0 Then
                If Val(txtAdjRoundoff_AMT.Text) > 0 Then
                    InsertIntoAccTran(Tranno, IIf(Val(txtAdjRoundoff_AMT.Text) > 0, "D", "C"),
                    "RNDOFF", Val(txtAdjRoundoff_AMT.Text), "RO", IIf(Val(txtAdjRoundoff_AMT.Text) > 0, drAccode, crAccode), Batchno, , , , , , , txtNarr_MAN.Text.ToString, "GRE", , , , , , departid)
                Else
                    InsertIntoAccTran(Tranno, IIf(Val(txtAdjRoundoff_AMT.Text) < 0, "C", "D"),
                    "RNDOFF", Val(txtAdjRoundoff_AMT.Text), "RO", IIf(Val(txtAdjRoundoff_AMT.Text) > 0, crAccode, drAccode), Batchno, , , , , , , txtNarr_MAN.Text.ToString, "GRE", , , , , , departid)
                End If
            End If
            Dim debCrNotTally As String = ""
            Dim balAmt As Double = Val(objGPack.GetSqlValue("SELECT SUM(CASE WHEN TRANMODE = 'C' THEN isnull(AMOUNT,0) ELSE isnull(AMOUNT,0)*(-1) END) AS BALANCE FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = '" & Batchno & "'", , "0", tran))
            If Math.Abs(balAmt) > 0 Then
                tran.Rollback()
                tran.Dispose()
                tran = Nothing
                MsgBox("Credit Debit Not Tally : " + balAmt.ToString, MsgBoxStyle.Information)
                Exit Function
            End If


            ' tran.Commit()
            tran = Nothing
            MsgBox("Updated Successfully...", MsgBoxStyle.Information)
            Dim pBatchno As String = Batchno
            Dim pBillDate As Date = dtpDate.Value.ToString("yyyy-MM-dd") 'dtpDate.Value
            Dim pParamStr As String = ""
            ' btnNew_Click(Me, New EventArgs)
            If IO.File.Exists(Application.StartupPath & "\BILLPRINT.EXE") Then
                Dim write As IO.StreamWriter
                Dim prnmemsuffix As String = ""
                If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId
                Dim memfile As String = "\BillPrint" + prnmemsuffix.Trim & ".mem"

                write = IO.File.CreateText(Application.StartupPath & memfile)
                write.WriteLine(LSet("TYPE", 15) & ":ACC")
                pParamStr += LSet("TYPE", 15) & ":ACC" & ";"
                write.WriteLine(LSet("BATCHNO", 15) & ":" & pBatchno)
                pParamStr += LSet("BATCHNO", 15) & ":" & pBatchno & ";"
                write.WriteLine(LSet("TRANDATE", 15) & ":" & pBillDate.ToString("yyyy-MM-dd"))
                pParamStr += LSet("TRANDATE", 15) & ":" & pBillDate.ToString("yyyy-MM-dd") & ";"
                write.WriteLine(LSet("DUPLICATE", 15) & ":N")
                pParamStr += LSet("DUPLICATE", 15) & ":N"
                write.Flush()
                write.Close()
                If EXE_WITH_PARAM = False Then
                    System.Diagnostics.Process.Start(Application.StartupPath & "\BILLPRINT.EXE")
                Else
                    System.Diagnostics.Process.Start(Application.StartupPath & "\BILLPRINT.EXE", pParamStr)
                End If
            End If
            editFlag = False

            funcClear()
            btnNew_Click(Me, New EventArgs)
            funcTranno()
        Catch ex As Exception
            If Not tran Is Nothing Then
                tran.Rollback()
                tran = Nothing
            End If
            MsgBox(ex.Message & vbCrLf & ex.StackTrace, MsgBoxStyle.Information)
        End Try


    End Function
    Private Sub InsertIntoAccTran _
        (ByVal tNo As Integer,
        ByVal tranMode As String,
        ByVal accode As String,
        ByVal amount As Double,
        ByVal payMode As String,
        ByVal contra As String,
        ByVal Batchno As String,
        Optional ByVal refNo As String = Nothing, Optional ByVal refDate As String = Nothing,
        Optional ByVal chqCardNo As String = Nothing,
        Optional ByVal chqDate As String = Nothing,
        Optional ByVal chqCardId As Integer = Nothing,
        Optional ByVal chqCardRef As String = Nothing,
        Optional ByVal Remark1 As String = Nothing,
        Optional ByVal Remark2 As String = Nothing,
        Optional ByVal baltopay As Double = Nothing,
        Optional ByVal BillCashCounterId As String = Nothing,
        Optional ByVal VATEXM As String = Nothing,
        Optional ByVal Transfered As String = Nothing,
        Optional ByVal Wt_EntOrder As String = Nothing,
        Optional ByVal departid As String = Nothing
        )
        If amount = 0 Then Exit Sub

        strSql = " INSERT INTO " & cnStockDb & "..ACCTRAN"
        strSql += " ("
        strSql += " SNO,TRANNO,TRANDATE,TRANMODE,ACCODE"
        strSql += " ,AMOUNT,BALANCE"
        strSql += " ,REFNO,REFDATE,PAYMODE,CHQCARDNO"
        strSql += " ,CARDID,CHQCARDREF,CHQDATE,BRSFLAG"
        strSql += " ,RELIASEDATE,FROMFLAG,REMARK1,REMARK2"
        strSql += " ,CONTRA,BATCHNO,USERID,UPDATED"
        strSql += " ,UPTIME,SYSTEMID,CASHID,COSTID,APPVER,COMPANYID"
        strSql += " ,DISC_EMPID,TRANSFERED,WT_ENTORDER"
        strSql += " )"
        strSql += " VALUES"
        strSql += " ("
        strSql += " '" & GetNewSno(TranSnoType.ACCTRANCODE, tran) & "'" ''SNO
        strSql += " ," & tNo & "" 'TRANNO 
        ' strSql += " ,'" & GetServerDate(tran) & "'" 'TRANDATE
        strSql += " ,'" & dtpDate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
        strSql += " ,'" & tranMode & "'" 'TRANMODE
        strSql += " ,'" & accode & "'" 'ACCODE
        strSql += " ," & Math.Abs(amount) & "" 'AMOUNT
        strSql += " ," & baltopay & "" 'AMOUNT        
        strSql += " ,'" & refNo & "'" 'REFNO
        If refDate = Nothing Then
            strSql += " ,NULL" 'REFDATE
        Else
            strSql += " ,'" & refDate & "'" 'REFDATE
        End If
        strSql += " ,'" & payMode & "'" 'PAYMODE
        strSql += " ,'" & chqCardNo & "'" 'CHQCARDNO
        strSql += " ," & chqCardId & "" 'CARDID
        strSql += " ,'" & chqCardRef & "'" 'CHQCARDREF
        If chqDate = Nothing Then
            strSql += " ,NULL" 'CHQDATE
        Else
            strSql += " ,'" & chqDate & "'" 'CHQDATE
        End If
        strSql += " ,''" 'BRSFLAG
        strSql += " ,NULL" 'RELIASEDATE
        strSql += " ,'A'" 'FROMFLAG
        strSql += " ,'" & Remark1 & "'" 'REMARK1
        strSql += " ,'" & Remark2 & "'" 'REMARK2
        strSql += " ,'" & contra & "'" 'CONTRA
        strSql += " ,'" & Batchno & "'" 'BATCHNO
        strSql += " ,'" & userId & "'" 'USERID
        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
        strSql += " ,'" & systemId & "'" 'SYSTEMID
        strSql += " ,'" & BillCashCounterId & "'" 'CASHID
        strSql += " ,'" & cnCostId & "'" 'COSTID
        strSql += " ,'" & VERSION & "'" 'APPVER
        strSql += " ,'" & strCompanyId & "'" 'COMPANYID
        strSql += " ,0" ' DISC_EMPID
        strSql += " ,'" & Transfered & "'" 'TRANSFERED
        strSql += " ,'" & Val(Wt_EntOrder) & "'" 'TRANSFERED
        strSql += " )"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
        strSql = ""
        cmd = Nothing
    End Sub

    Public Sub StateName()
        strSql = " SELECT STATENAME FROM " & cnAdminDb & "..STATEMAST ORDER BY STATENAME"
        objGPack.FillCombo(strSql, cmbStateName_MAN, , False)
        strSql = "SELECT STATENAME FROM " & cnAdminDb & "..STATEMAST WHERE STATEID=" & CompanyStateId
        cmbStateName_MAN.Text = objGPack.GetSqlValue(strSql, "STATENAME", "")
    End Sub

    Public Sub AcName()
        strSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD ORDER BY ACNAME"
        objGPack.FillCombo(strSql, cmbAcName_MAN, , False)
    End Sub
    Private Sub txtGst_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtGstPer_AMT.TextChanged
        'calculation()
    End Sub

    Public Sub calculation(ByVal SGST As Double, ByVal CGST As Double, ByVal IGST As Double, ByVal TDSPER As Double)
        Dim stateid As String = objGPack.GetSqlValue("SELECT STATEID FROM " & cnAdminDb & "..STATEMAST WHERE STATENAME= '" & cmbStateName_MAN.Text & "' ", , , tran)
        Dim cmpystateid As String = objGPack.GetSqlValue("SELECT STATEID FROM " & cnAdminDb & "..COMPANY WHERE STATEID= '" & stateid & "' ", , , tran)
        Dim amt_per, tax, per, res As Decimal

        If cmbStateName_MAN.Text = "" Then
            MessageBox.Show("Select State Name")
            txtGstPer_AMT.Text = ""
        Else
            txtSgst_per.Text = Format(SGST, "0.00")
            txtSgst_AMT.Text = Format(Math.Round(Val(txtamt_AMT.Text) * SGST / 100, 2), "0.00")

            txtCgst_per.Text = Format(CGST, "0.00")
            txtCgst_AMT.Text = Format(Math.Round(Val(txtamt_AMT.Text) * CGST / 100, 2), "0.00")

            txtIgst_per.Text = Format(IGST, "0.00")
            txtIgst_AMT.Text = Format(Math.Round(Val(txtamt_AMT.Text) * IGST / 100, 2), "0.00")

            txtGstPer_AMT.Text = Format(Math.Round(SGST + CGST + IGST), "0.00")
            txtGsttot_Amt.Text = Format(Math.Round(Val(txtSgst_AMT.Text.ToString) + Val(txtCgst_AMT.Text.ToString) + Val(txtIgst_AMT.Text.ToString)), "0.00")
            txtTDSper_amt.Text = Format(TDSPER, "0.00")
            ''txtTDSamt.Text = Format(Math.Round(Val(txtamt_AMT.Text) * TDSPER / 100, 2), "0.00")
            txtTDSamt.Text = Format(CalcRoundoffAmt(Math.Round(Val(txtamt_AMT.Text) * Val(TDSPER) / 100, 2), GetAdmindbSoftValue("ROUNDOFF-ACC-TDS", "N")), "0.00")
            txttds.Text = Format(Math.Round(Val(txtamt_AMT.Text) * TDSPER / 100, 2), "0.00")

        End If


        'If cmbStateName_MAN.Text = "" Then
        '    MessageBox.Show("Select State Name")
        '    txtGstPer_AMT.Text = ""
        'ElseIf stateid = cmpystateid Then

        '    res = Val(Val(txtamt_AMT.Text) / 100)
        '    per = Val(res * Val(txtGstPer_AMT.Text))
        '    txtGsttot_Amt.Text = per.ToString()
        '    txtSgst_AMT.Text = Val(Val(per) / 2).ToString()
        '    txtCgst_AMT.Text = Val(Val(per) / 2).ToString()

        '    amt_per = Val(Val(txtGstPer_AMT.Text) / 2)
        '    txtSgst_per.Text = amt_per.ToString()
        '    txtCgst_per.Text = amt_per.ToString()
        '    txtIgst_AMT.Text = ""
        '    txtIgst_per.Text = ""
        '    txtIgst_AMT.ReadOnly = True
        '    txtIgst_per.ReadOnly = True
        'Else
        '    res = Val(Val(txtamt_AMT.Text) / 100)
        '    per = Val(res * Val(txtGstPer_AMT.Text))
        '    txtIgst_AMT.Text = per.ToString()
        '    txtGsttot_Amt.Text = per.ToString()

        '    txtIgst_per.Text = txtGstPer_AMT.Text
        '    txtSgst_AMT.Text = ""
        '    txtCgst_AMT.Text = ""
        '    txtSgst_per.Text = ""
        '    txtCgst_per.Text = ""
        '    txtSgst_AMT.ReadOnly = True
        '    txtCgst_AMT.ReadOnly = True
        '    txtSgst_per.ReadOnly = True
        '    txtCgst_per.ReadOnly = True
        'End If
    End Sub

    Private Sub txtRate_AMT_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'If Val(txtPcs_NUM.Text) <> 0 Then
        '    txtamt_AMT.Text = Format(Val(Val(txtPcs_NUM.Text.ToString) * Val(txtRate_AMT.Text.ToString)).ToString(), "0.00")
        'Else
        '    txtamt_AMT.Text = Format(Val(txtRate_AMT.Text.ToString), "0.00")
        'End If
    End Sub
    Private Sub txtSgst_per_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSgst_per.TextChanged
        If Val(txtGstPer_AMT.Text) > 0 Then CaltxtSgst_per()
    End Sub

    Public Sub CaltxtSgst_per()
        Exit Sub
        txtCgst_per.Text = Val(Val(txtGstPer_AMT.Text) - Val(txtSgst_per.Text)).ToString()
        txtSgst_AMT.Text = Val(Val(Val(txtamt_AMT.Text) / 100) * Val(txtSgst_per.Text)).ToString()
    End Sub

    Private Sub txtCgst_per_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCgst_per.TextChanged
        If Val(txtGstPer_AMT.Text) > 0 Then CaltxtCgst_per()
    End Sub

    Public Sub CaltxtCgst_per()
        Exit Sub
        txtSgst_per.Text = Val(Val(txtGstPer_AMT.Text) - Val(txtCgst_per.Text)).ToString()
        txtCgst_AMT.Text = Val(Val(Val(txtamt_AMT.Text) / 100) * Val(txtCgst_per.Text)).ToString()
    End Sub
    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView.Click, ViewToolStripMenuItem.Click
        tabMain.SelectedTab = TabPage2
        'GridLoad()
        'GridDesign()
        Gridview.DataSource = Nothing
        Dim frmdate As String = Today.Date.ToString("dd-MM-yyyy")
        dtpfinddate.Text = frmdate
        dtpFinddate2.Text = frmdate
        txtTranno1.Text = ""
        dtpfinddate.Focus()

    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        dtpDate.Focus()
        tabMain.SelectedTab = tabGen
    End Sub

    Private Sub cmbStateName_SelectedValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbStateName_MAN.SelectedValueChanged
        txtGstPer_AMT.Text = ""
        txtGsttot_Amt.Text = ""
        txtSgst_per.Text = ""
        txtSgst_AMT.Text = ""
        txtCgst_per.Text = ""
        txtCgst_AMT.Text = ""
        txtIgst_per.Text = ""
        txtIgst_AMT.Text = ""
        txtamt_AMT.Text = ""
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        funcClear()
    End Sub

    Private Sub txtamt_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtamt_AMT.GotFocus
        'txtamt_AMT.Text = Format(Math.Round(IIf(Val(txtPcs_NUM.Text) = 0, 1, Val(txtPcs_NUM.Text)) * Val(txtRate_AMT.Text), 2), "0.00")
    End Sub

    Private Sub txtamt_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtamt_AMT.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            'txtamt_AMT.Text = Format(Math.Round(IIf(Val(txtPcs_NUM.Text) = 0, 1, Val(txtPcs_NUM.Text)) * Val(txtRate_AMT.Text), 2), "0.00")
            txtamt_AMT.Text = Format(Val(txtamt_AMT.Text.ToString), "0.00")
            objTax = New frmGSTTax_New
            objTax.txtSgst_per_AMT.Focus()
            If objTax.ShowDialog = Windows.Forms.DialogResult.OK Then
                calculation(Val(objTax.txtSgst_per_AMT.Text.ToString), Val(objTax.txtCgst_per_AMT.Text.ToString), Val(objTax.txtIgst_per_AMT.Text.ToString), Val(objTax.txtTdsPer_PER.Text.ToString))
            End If
            txtSgst_AMT.Focus()
        End If
    End Sub

    Private Sub txtamt_AMT_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtamt_AMT.TextChanged
        'txtGstPer_AMT.Text = ""
        'txtGsttot_Amt.Text = ""
        'txtSgst_per.Text = ""
        'txtSgst_AMT.Text = ""
        'txtCgst_per.Text = ""
        'txtCgst_AMT.Text = ""
        'txtIgst_per.Text = ""
        'txtIgst_AMT.Text = ""
    End Sub
    Private Sub GridLoad()

        strSql = "SELECT SNO,TRANNO,TRANDATE,DESCRIPTION,HSN,PCS,WEIGHT,RATE,AMOUNT,TAX,TAXAMOUNT,SGST_PER,SGST,CGST_PER,CGST,IGST_PER,IGST,STATEID,ACCODE,REFNO,REFDATE,"
        strSql += vbCrLf + "GSTCLAIM, TRANTYPE, UPLOAD, BATCHNO,"
        strSql += vbCrLf + "NULL AS ORIGINAL_INVOICE_DATE,NULL AS ORIGINAL_INVOICE_NUMBER,NULL AS INVOICE_CATEGORY,NULL AS INVOICE_DUE_TERMS,"
        strSql += vbCrLf + "NULL AS INVOICE_DUE_DATE,NULL AS ORDER_NUMBER,NULL AS ORDER_DATE,NULL AS SUPPLY_TYPE,NULL AS FLAG_EXPORT_INVOICE,"
        strSql += vbCrLf + "NULL AS FLAG_EXPORT_GST_PAYMENT,NULL AS EXPORT_SHIPPING_BILL_NUMBER,NULL AS EXPORT_SHIIPING_BILL_DATE,"
        strSql += vbCrLf + "NULL AS EXPORT_DESTINATION_COUNTRY_CODE,NULL AS CUSTOMER_BILLING_NAME,NULL AS CUSTOMER_BILLING_ADDRESS,"
        strSql += vbCrLf + "NULL AS CUSTOMER_BILLING_CITY,NULL AS CUSTOMER_BILLING_PINCODE,NULL AS CUSTOMER_BILLING_STATE,NULL AS CUSTOMER_BILLING_STATECODE,"
        strSql += vbCrLf + "NULL AS CUSTOMER_BILLING_GSTIN,NULL AS CONSIGNEE_SHIPPING_NAME,NULL AS CONSIGNEE_SHIPPING_ADDRESS,NULL AS CONSIGNEE_SHIPPING_CITY,"
        strSql += vbCrLf + "NULL AS CONSIGNEE_SHIPPING_PINCODE,NULL AS CONSIGNEE_SHIIPING_STATE,NULL AS CONSIGNEE_SHIPPING_STATECODE,NULL AS CONSIGNEE_SHIPPING_GSTIN,"
        strSql += vbCrLf + "NULL AS ITEM_CATEGORY,NULL AS ITEM_QUANTITY,NULL AS ITEM_UNITOFMEASUREMENT,NULL AS ITEM_RATE,NULL AS ITEM_TOTAL_BEFORE_DISCOUNT,"
        strSql += vbCrLf + "NULL AS ITEM_DISCOUNT,NULL AS ITEM_TAXABLE_VALUE,NULL AS ITEM_TOTAL_INCLUDING_GST,NULL AS NIL_RATED_AMOUNT,"
        strSql += vbCrLf + "NULL AS EXEMPTED_AMOUNT,NULL AS NON_GST_AMOUNT,NULL AS FLAG_REVERSE_CHARGE,NULL AS PERCENT_REVERSE_CHARGE_RATE,"
        strSql += vbCrLf + "NULL AS FLAG_TAXPAID_PROVISIONAL_ASSESSMENT,NULL AS MERCHANT_ID_ISSUED_BY_ECOMMERCE,NULL AS GSTIN_ECOMMERCE,"
        strSql += vbCrLf + "NULL AS TAXABLE_VALUE_ON_WHICH_TCS_HAS_BEEN_DEDUCTED,NULL AS TCS_CGST_RATE,NULL AS TCS_CGST_AMOUNT,NULL AS TCS_SGST_RATE,"
        strSql += vbCrLf + "NULL AS TCS_SGST_AMOUNT,NULL AS TCS_IGST_RATE,NULL AS TCS_IGST_AMOUNT,NULL AS ADVANCEPAYMENT_DATE,NULL AS ADVANCEPAYMENT_DOCUMNET_NUMBER,"
        strSql += vbCrLf + "NULL AS SUPPLIER_NAME,NULL AS SUPPLIER_ADDRESS,NULL AS SUPPLIER_CITY,NULL AS SUPPLIER_PINCODE,NULL AS SUPPLIER_STATE,"
        strSql += vbCrLf + "NULL AS SUPPLIER_STATECODE,NULL AS SUPPLIER_GSTIN,NULL AS GSTIN_TDS_DEDUCTOR,NULL AS VALUE_ON_WHICH_TDS_DEDUCTED,"
        strSql += vbCrLf + "NULL AS DATE_OF_PAYMENT_TO_DEDUCTEE_TDS,NULL AS TDS_CGST_RATE,NULL AS TDS_CGST_AMOUNT,NULL AS TDS_SGST_RATE,NULL AS TDS_SGST_AMOUNT,"
        strSql += vbCrLf + "NULL AS TDS_IGST_RATE,NULL AS TDS_IGST_AMOUNT,NULL AS CUSTOM_FIELD_1,NULL AS CUSTOM_FIELD_2,NULL AS CUSTOM_FIELD_3,"
        strSql += vbCrLf + "NULL AS CUSTOM_FIELD_4,NULL AS CUSTOM_FIELD_5" ',TDSPER,TDS"
        strSql += vbCrLf + " FROM " & cnStockDb & "..GSTREGISTER WHERE ISNULL(CANCEL,'')='' ORDER BY TRANDATE "
        cmd = New OleDbCommand(strSql, cn)
        dt = New DataTable
        da = New OleDbDataAdapter(cmd)
        da.Fill(dt)


        Gridview.DataSource = dt
        funcGridViewStyle()
    End Sub
    Function funcGridViewStyle() As Integer
        With Gridview
            With .Columns("TRANDATE")
                .HeaderText = "TRANDATE"
                .Width = 80
                .DefaultCellStyle.Format = "dd-MM-yyyy"
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("REFDATE")
                .HeaderText = "REFDATE"
                .Width = 80
                .DefaultCellStyle.Format = "dd-MM-yyyy"
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("SNO")
                .Visible = False
            End With
        End With
    End Function
    Private Sub cmbGstClaim_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbGstClaim.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If funcvalidation() = False Then Exit Sub
            If lblEditKeyNo.Text.ToString = "" Then
                Dim dr As DataRow
                dr = dtGrid.NewRow
                dr.Item("DESCRIPTION") = txtDescrip_MAN.Text.ToString
                dr.Item("HSN") = txthsn.Text.ToString
                'dr.Item("PCS") = Val(txtPcs_NUM.Text.ToString)
                'dr.Item("WEIGHT") = Format(Val(txtWeight_WET.Text.ToString), "0.00")
                'dr.Item("RATE") = Format(Val(txtRate_AMT.Text.ToString), "0.00")
                dr.Item("AMOUNT") = Format(Val(txtamt_AMT.Text.ToString), "0.00")
                dr.Item("TAX") = Format(Val(txtGstPer_AMT.Text.ToString), "0.00")
                dr.Item("TAXAMOUNT") = Format(Val(txtGsttot_Amt.Text.ToString), "0.00")
                dr.Item("SGST_PER") = Format(Val(txtSgst_per.Text.ToString), "0.00")
                dr.Item("SGST") = Format(Val(txtSgst_AMT.Text.ToString), "0.00")
                dr.Item("CGST_PER") = Format(Val(txtCgst_per.Text.ToString), "0.00")
                dr.Item("CGST") = Format(Val(txtCgst_AMT.Text.ToString), "0.00")
                dr.Item("IGST_PER") = Format(Val(txtIgst_per.Text.ToString), "0.00")
                dr.Item("IGST") = Format(Val(txtIgst_AMT.Text.ToString), "0.00")
                dr.Item("OTHERS") = Format(Val(txtOthers_AMT.Text.ToString), "0.00")
                'dr.Item("STATEID") = Val(txtDescrip_MAN.Text.ToString)
                'dr.Item("ACCODE") = Val(txtDescrip_MAN.Text.ToString)
                'dr.Item("DEPARTID") = Val(txtDescrip_MAN.Text.ToString)
                dr.Item("BILLNO") = txtRefno.Text.ToString
                dr.Item("BILLDATE") = dtpRefDate.Value.ToString("dd-MM-yyyy")
                dr.Item("GSTCLAIM") = cmbGstClaim.Text
                dr.Item("GSTNO") = txtGSTIN.Text.ToString
                dr.Item("ADDRESS1") = Mid(txtAddress1.Text.ToString, 1, 30)
                dr.Item("ADDRESS2") = Mid(txtAddress2.Text.ToString, 1, 30)
                dr.Item("TDSPER") = Format(Val(txtTDSper_amt.Text.ToString), "0.00")
                dr.Item("TDS") = Format(Val(txttds.Text.ToString), "0.00")
                'dr.Item("DEPARTMENT")=
                dtGrid.Rows.Add(dr)
                GridViewGst.DataSource = Nothing
                GridViewGst.DataSource = dtGrid
                StyleGridSASR(GridViewGst)
            Else
                With dtGrid.Rows(Val(lblEditKeyNo.Text.ToString))
                    .Item("DESCRIPTION") = txtDescrip_MAN.Text.ToString
                    .Item("HSN") = txthsn.Text.ToString
                    '.Item("PCS") = Val(txtPcs_NUM.Text.ToString)
                    '.Item("WEIGHT") = Format(Val(txtWeight_WET.Text.ToString), "0.00")
                    '.Item("RATE") = Format(Val(txtRate_AMT.Text.ToString), "0.00")
                    .Item("AMOUNT") = Format(Val(txtamt_AMT.Text.ToString), "0.00")
                    .Item("TAX") = Format(Val(txtGstPer_AMT.Text.ToString), "0.00")
                    .Item("TAXAMOUNT") = Format(Val(txtGstPer_AMT.Text.ToString), "0.00")
                    .Item("SGST_PER") = Format(Val(txtSgst_per.Text.ToString), "0.00")
                    .Item("SGST") = Format(Val(txtSgst_AMT.Text.ToString), "0.00")
                    .Item("CGST_PER") = Format(Val(txtCgst_per.Text.ToString), "0.00")
                    .Item("CGST") = Format(Val(txtCgst_AMT.Text.ToString), "0.00")
                    .Item("IGST_PER") = Format(Val(txtIgst_per.Text.ToString), "0.00")
                    .Item("IGST") = Format(Val(txtIgst_AMT.Text.ToString), "0.00")
                    .Item("OTHERS") = Format(Val(txtOthers_AMT.Text.ToString), "0.00")
                    '.Item("State") = cmbStateName_MAN.Text 'Val(txtDescrip_MAN.Text.ToString)
                    '.Item("ACCODE") = cmbAcName_MAN.Text 'Val(txtDescrip_MAN.Text.ToString)
                    '.Item("DEPARTMENT") = Val(txtDescrip_MAN.Text.ToString)
                    .Item("BILLNO") = txtRefno.Text.ToString
                    '.Item("BILLDATE") = dtpRefDate.Value.ToString("dd-MM-yyyy")
                    .Item("GSTCLAIM") = cmbGstClaim.Text
                    .Item("GSTNO") = txtGSTIN.Text.ToString
                    .Item("ADDRESS1") = Mid(txtAddress1.Text.ToString, 1, 30)
                    .Item("ADDRESS2") = Mid(txtAddress2.Text.ToString, 1, 30)
                    .Item("TDS") = Format(Val(txttds.Text.ToString), "0.00")
                    dtGrid.AcceptChanges()
                    GridViewGst.DataSource = Nothing
                    GridViewGst.DataSource = dtGrid
                    StyleGridSASR(GridViewGst)
                End With
            End If
            'Clear TextBox
            txtDescrip_MAN.Text = ""
            txthsn.Text = ""
            'txtPcs_NUM.Text = ""
            txtWeight_WET.Text = ""
            'txtRate_AMT.Text = ""
            txtamt_AMT.Text = ""
            'txtGstPer_AMT.Text = ""
            'txtGsttot_Amt.Text = ""
            ' txtSgst_per.Text = ""
            txtSgst_AMT.Text = ""
            'txtCgst_per.Text = ""
            txtCgst_AMT.Text = ""
            'txtIgst_per.Text = ""
            txtIgst_AMT.Text = ""
            txtOthers_AMT.Text = ""
            'txtRefno.Text = ""
            lblEditKeyNo.Text = ""
            GridTotal()
            'cmbGstClaim.Text = "NO"
            txtDescrip_MAN.Focus()
        End If
    End Sub

    Private Function funcvalidation() As Boolean
        If txtDescrip_MAN.Text.ToString.Trim = "" Then MsgBox("Description should not be empty...", MsgBoxStyle.Information) : txtDescrip_MAN.Focus() : Return False : Exit Function
        'If CmbCostcentre_OWN.Text.ToString.Trim = "" Then MsgBox("Costcentre should not be empty...", MsgBoxStyle.Information) : CmbCostcentre_OWN.Focus() : Return False : Exit Function
        ' If txthsn.Text.ToString.Trim = "" Then MsgBox("HSN Code should not be empty...", MsgBoxStyle.Information) : txthsn.Focus() : Return False : Exit Function
        If txtamt_AMT.Text.ToString.Trim = "" Then MsgBox("Amount should not be empty...", MsgBoxStyle.Information) : txtamt_AMT.Focus() : Return False : Exit Function
        If txtGstPer_AMT.Text.ToString.Trim = "" Then MsgBox("GST % should not be empty...", MsgBoxStyle.Information) : txtGstPer_AMT.Focus() : Return False : Exit Function
        Return True
    End Function

    Public Sub GridDesign()
        With Gridview
            With .Columns("PARTICULAR")
                ' .HeaderText = "PARTICULAR"
                .Width = 100
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = True
            End With

            With .Columns("BATCHNO")
                .HeaderText = "BATCHNO"
                '.Width = 100
                '.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                '.SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("SNO")
                .HeaderText = "SNO"
                .Width = 100
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("TRANNO")
                .HeaderText = "INVOICE_NUMBER"
                .Width = 200
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("TRANDATE")
                .HeaderText = "INVOICE_DATE"
                .Width = 200
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("DESCRIPTION")
                .HeaderText = "ITEM_DESCRIPTION"
                .Width = 300
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("HSN")
                .HeaderText = "HSN_SAC_CODE"
                .Width = 200
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With

            With .Columns("PCS")
                .HeaderText = "PCS"
                .Width = 100
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("WEIGHT")
                .HeaderText = "WEIGHT"
                .Width = 100
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("RATE")
                .HeaderText = "RATE"
                .Width = 100
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("AMOUNT")
                .HeaderText = "ITEM_RATE"
                .Width = 100
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("TAX")
                .HeaderText = "TAX"
                .Width = 100
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("TAXAMOUNT")
                .HeaderText = "TAXAMOUNT"
                .Width = 100
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("SGST_PER")
                .HeaderText = "SGST_RATE"
                .Width = 100
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("SGST")
                .HeaderText = "SGST_AMOUNT"
                .Width = 100
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("CGST_PER")
                .HeaderText = "CGST_RATE"
                .Width = 100
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("CGST")
                .HeaderText = "CGST_AMOUNT"
                .Width = 100
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("IGST_PER")
                .HeaderText = "IGST_RATE"
                .Width = 100
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("IGST")
                .HeaderText = "IGST_AMOUNT"
                .Width = 100
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            'With .Columns("OTHERS")
            '    .HeaderText = "OTHERS"
            '    .Width = 100
            '    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            '    .SortMode = DataGridViewColumnSortMode.NotSortable
            'End With
            With .Columns("STATEID")
                .HeaderText = "STATECODE_PLACE_OF_SUPPLY"
                .Width = 200
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("ACCODE")
                .HeaderText = "SUPPLIER_NAME"
                .Width = 100
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("REFNO")
                .HeaderText = "REFNO"
                .Width = 100
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("REFDATE")
                .HeaderText = "REFDATE"
                .Width = 100
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("GSTCLAIM")
                .HeaderText = "GSTCLAIM"
                .Width = 100
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("TRANTYPE")
                .HeaderText = "TRANTYPE"
                .Width = 100
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("UPLOAD")
                .HeaderText = "UPLOAD"
                .Width = 100
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("ORIGINAL_INVOICE_DATE")
                .HeaderText = "ORIGINAL_INVOICE_DATE"
                .Width = 200
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("ORIGINAL_INVOICE_NUMBER")
                .HeaderText = "ORIGINAL_INVOICE_NUMBER"
                .Width = 200
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("INVOICE_CATEGORY")
                .HeaderText = "INVOICE_CATEGORY"
                .Width = 300
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("INVOICE_DUE_TERMS")
                .HeaderText = "INVOICE_DUE_TERMS"

                .Width = 300
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("INVOICE_DUE_DATE")
                .HeaderText = "INVOICE_DUE_DATE"
                .Width = 200

                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("ORDER_NUMBER")
                .HeaderText = "ORDER_NUMBER"
                .Width = 200
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("ORDER_DATE")
                .HeaderText = "ORDER_DATE"
                .Width = 200
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("SUPPLY_TYPE")
                .HeaderText = "SUPPLY_TYPE"
                .Width = 200
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("FLAG_EXPORT_INVOICE")
                .HeaderText = "FLAG_EXPORT_INVOICE"
                .Width = 200
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("FLAG_EXPORT_GST_PAYMENT")
                .HeaderText = "FLAG_EXPORT_GST_PAYMENT"
                .Width = 250
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("EXPORT_SHIPPING_BILL_NUMBER")
                .HeaderText = "EXPORT_SHIPPING_BILL_NUMBER"
                .Width = 250
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("EXPORT_SHIIPING_BILL_DATE")
                .HeaderText = "EXPORT_SHIIPING_BILL_DATE"
                .Width = 300
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("EXPORT_DESTINATION_COUNTRY_CODE")
                .HeaderText = "EXPORT_DESTINATION_COUNTRY_CODE"
                .Width = 300
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("CUSTOMER_BILLING_NAME")
                .HeaderText = "CUSTOMER_BILLING_NAME"
                .Width = 300
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("CUSTOMER_BILLING_ADDRESS")
                .HeaderText = "CUSTOMER_BILLING_ADDRESS"
                .Width = 300
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("CUSTOMER_BILLING_CITY")
                .HeaderText = "CUSTOMER_BILLING_CITY"
                .Width = 300
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("CUSTOMER_BILLING_PINCODE")
                .HeaderText = "CUSTOMER_BILLING_PINCODE"
                .Width = 300
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("CUSTOMER_BILLING_STATE")
                .HeaderText = "CUSTOMER_BILLING_STATE"
                .Width = 300
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("CUSTOMER_BILLING_STATECODE")
                .HeaderText = "CUSTOMER_BILLING_STATECODE"
                .Width = 300
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("CUSTOMER_BILLING_GSTIN")
                .HeaderText = "CUSTOMER_BILLING_GSTIN"
                .Width = 300
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("CONSIGNEE_SHIPPING_NAME")
                .HeaderText = "CONSIGNEE_SHIPPING_NAME"
                .Width = 300
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("CONSIGNEE_SHIPPING_ADDRESS")
                .HeaderText = "CONSIGNEE_SHIPPING_ADDRESS"
                .Width = 300
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("CONSIGNEE_SHIPPING_CITY")
                .HeaderText = "CONSIGNEE_SHIPPING_CITY"
                .Width = 300
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("CONSIGNEE_SHIPPING_PINCODE")
                .HeaderText = "CONSIGNEE_SHIPPING_PINCODE"
                .Width = 300
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("CONSIGNEE_SHIIPING_STATE")
                .HeaderText = "CONSIGNEE_SHIIPING_STATE"
                .Width = 300
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("CONSIGNEE_SHIPPING_STATECODE")
                .HeaderText = "CONSIGNEE_SHIPPING_STATECODE"
                .Width = 300
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("CONSIGNEE_SHIPPING_GSTIN")
                .HeaderText = "CONSIGNEE_SHIPPING_GSTIN"
                .Width = 300
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("ITEM_CATEGORY")
                .HeaderText = "ITEM_CATEGORY"
                .Width = 300
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("ITEM_QUANTITY")
                .HeaderText = "ITEM_QUANTITY"
                .Width = 300
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("ITEM_UNITOFMEASUREMENT")
                .HeaderText = "ITEM_UNITOFMEASUREMENT"
                .Width = 300
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("ITEM_RATE")
                .HeaderText = "ITEM_RATE"
                .Width = 200
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("ITEM_TOTAL_BEFORE_DISCOUNT")
                .HeaderText = "ITEM_TOTAL_BEFORE_DISCOUNT"
                .Width = 300
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("ITEM_DISCOUNT")
                .HeaderText = "ITEM_DISCOUNT"
                .Width = 200
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("ITEM_TAXABLE_VALUE")
                .HeaderText = "ITEM_TAXABLE_VALUE"
                .Width = 200
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("ITEM_TOTAL_INCLUDING_GST")
                .HeaderText = "ITEM_TOTAL_INCLUDING_GST"
                .Width = 300
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("NIL_RATED_AMOUNT")
                .HeaderText = "NIL_RATED_AMOUNT"
                .Width = 200
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("EXEMPTED_AMOUNT")
                .HeaderText = "EXEMPTED_AMOUNT"
                .Width = 200
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("NON_GST_AMOUNT")
                .HeaderText = "NON_GST_AMOUNT"
                .Width = 200
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("FLAG_REVERSE_CHARGE")
                .HeaderText = "FLAG_REVERSE_CHARGE"
                .Width = 200
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("PERCENT_REVERSE_CHARGE_RATE")
                .HeaderText = "PERCENT_REVERSE_CHARGE_RATE"
                .Width = 300
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("FLAG_TAXPAID_PROVISIONAL_ASSESSMENT")
                .HeaderText = "FLAG_TAXPAID_PROVISIONAL_ASSESSMENT"
                .Width = 350
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("MERCHANT_ID_ISSUED_BY_ECOMMERCE")
                .HeaderText = "MERCHANT_ID_ISSUED_BY_ECOMMERCE"
                .Width = 300
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("GSTIN_ECOMMERCE")
                .HeaderText = "GSTIN_ECOMMERCE"
                .Width = 200
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("TAXABLE_VALUE_ON_WHICH_TCS_HAS_BEEN_DEDUCTED")
                .HeaderText = "TAXABLE VALUE ON WHICH TCS HAS BEEN DEDUCTED"
                .Width = 400
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("TCS_CGST_RATE")
                .HeaderText = "TCS_CGST_RATE"
                .Width = 200
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("TCS_CGST_AMOUNT")
                .HeaderText = "TCS_CGST_AMOUNT"
                .Width = 200
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("TCS_SGST_RATE")
                .HeaderText = "TCS_SGST_RATE"
                .Width = 200
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("TCS_SGST_AMOUNT")
                .HeaderText = "TCS_SGST_AMOUNT"
                .Width = 200
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("TCS_IGST_RATE")
                .HeaderText = "TCS_IGST_RATE"
                .Width = 200
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("TCS_IGST_AMOUNT")
                .HeaderText = "TCS_IGST_AMOUNT"
                .Width = 200
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("ADVANCEPAYMENT_DATE")
                .HeaderText = "ADVANCEPAYMENT_DATE"
                .Width = 200
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("ADVANCEPAYMENT_DOCUMNET_NUMBER")
                .HeaderText = "ADVANCEPAYMENT_DOCUMNET_NUMBER"
                .Width = 200
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("SUPPLIER_NAME")
                .HeaderText = "SUPPLIER_NAME"
                .Width = 200
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("SUPPLIER_ADDRESS")
                .HeaderText = "SUPPLIER_ADDRESS"
                .Width = 200
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("SUPPLIER_CITY")
                .HeaderText = "SUPPLIER_CITY"
                .Width = 200
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("SUPPLIER_PINCODE")
                .HeaderText = "SUPPLIER_PINCODE"
                .Width = 200
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("SUPPLIER_STATE")
                .HeaderText = "SUPPLIER_STATE"
                .Width = 200
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("SUPPLIER_STATECODE")
                .HeaderText = "SUPPLIER_STATECODE"
                .Width = 200
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("SUPPLIER_GSTIN")
                .HeaderText = "SUPPLIER_GSTIN"
                .Width = 200
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("GSTIN_TDS_DEDUCTOR")
                .HeaderText = "GSTIN_TDS_DEDUCTOR"
                .Width = 200
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("VALUE_ON_WHICH_TDS_DEDUCTED")
                .HeaderText = "VALUE_ON_WHICH_TDS_DEDUCTED"
                .Width = 300
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("DATE_OF_PAYMENT_TO_DEDUCTEE_TDS")
                .HeaderText = "DATE_OF_PAYMENT_TO_DEDUCTEE_TDS"
                .Width = 300
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("TDS_CGST_RATE")
                .HeaderText = "TDS_CGST_RATE"
                .Width = 200
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("TDS_CGST_AMOUNT")
                .HeaderText = "TDS_CGST_AMOUNT"
                .Width = 200
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("TDS_SGST_RATE")
                .HeaderText = "TDS_SGST_RATE"
                .Width = 200
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("TDS_SGST_AMOUNT")
                .HeaderText = "TDS_SGST_AMOUNT"
                .Width = 200
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("TDS_IGST_RATE")
                .HeaderText = "TDS_IGST_RATE"
                .Width = 200
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("TDS_IGST_AMOUNT")
                .HeaderText = "TDS_IGST_AMOUNT"
                .Width = 200
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("CUSTOM_FIELD_1")
                .HeaderText = "CUSTOM FIELD 1"
                .Width = 200
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("CUSTOM_FIELD_2")
                .HeaderText = "CUSTOM FIELD 2"
                .Width = 200
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("CUSTOM_FIELD_3")
                .HeaderText = "CUSTOM FIELD 3"
                .Width = 200
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("CUSTOM_FIELD_4")
                .HeaderText = "CUSTOM FIELD 4"
                .Width = 200
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("CUSTOM_FIELD_5")
                .HeaderText = "CUSTOM FIELD 5"
                .Width = 200
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
        End With
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub GridViewGst_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridViewGst.GotFocus

    End Sub

    Private Sub GridViewGst_UserDeletedRow(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles GridViewGst.UserDeletedRow
        dtGrid.AcceptChanges()
        GridTotal()
    End Sub

    Private Sub GridViewGst_UserDeletingRow(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewRowCancelEventArgs) Handles GridViewGst.UserDeletingRow

    End Sub

    Private Sub GridViewGst_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles GridViewGst.KeyPress
        'If e.KeyChar = Chr(Keys.Enter) Then
        'If GridViewGst.Rows.Count > 0 Then
        'With GridViewGst.CurrentRow
        '    txtDescrip_MAN.Text = .Cells("DESCRIPTION").Value.ToString
        '    txthsn.Text = .Cells("HSN").Value.ToString
        '    'txtPcs_NUM.Text = .Cells("PCS").Value.ToString
        '    'txtWeight_WET.Text = .Cells("WEIGHT").Value.ToString
        '    'txtRate_AMT.Text = .Cells("RATE").Value.ToString
        '    txtamt_AMT.Text = .Cells("AMOUNT").Value.ToString
        '    txtGstPer_AMT.Text = .Cells("TAX").Value.ToString
        '    txtGstPer_AMT.Text = .Cells("TAXAMOUNT").Value.ToString
        '    txtSgst_per.Text = .Cells("SGST_PER").Value.ToString
        '    txtSgst_AMT.Text = .Cells("SGST").Value.ToString
        '    txtCgst_per.Text = .Cells("CGST_PER").Value.ToString
        '    txtCgst_AMT.Text = .Cells("CGST").Value.ToString
        '    txtIgst_per.Text = .Cells("IGST_PER").Value.ToString
        '    txtIgst_AMT.Text = .Cells("IGST").Value.ToString
        '    txtOthers_AMT.Text = .Cells("OTHERS").Value.ToString
        '    txtRefno.Text = .Cells("BILLNO").Value.ToString
        '    dtpRefDate.Text = .Cells("BILLDATE").Value.ToString
        '    cmbGstClaim.Text = .Cells("GSTCLAIM").Value.ToString
        '    lblEditKeyNo.Text = .Index.ToString
        '    txtDescrip_MAN.Focus()
        'End With
        ' End If
        '  End If
    End Sub

    Private Sub txtPcs_NUM_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'If Val(txtPcs_NUM.Text) <> 0 Then
        '    txtamt_AMT.Text = Format(Val(Val(txtPcs_NUM.Text.ToString) * Val(txtRate_AMT.Text.ToString)).ToString(), "0.00")
        'Else
        '    txtamt_AMT.Text = Format(Val(txtRate_AMT.Text.ToString), "0.00")
        'End If
    End Sub

    Private Sub cmbAcName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbAcName_MAN.KeyPress
        If cmbAcName_MAN.Text <> "" Then
            strSql = "SELECT GSTNO,ADDRESS1,ADDRESS2 FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME='" & cmbAcName_MAN.Text & "'"
            Dim dr As DataRow = GetSqlRow(strSql, cn)
            If Not dr Is Nothing Then
                txtGSTIN.Text = dr("GSTNO").ToString
                If dr("GSTNO").ToString <> "" Then txtGSTIN.ReadOnly = True Else txtGSTIN.ReadOnly = False
                txtAddress1.Text = dr("ADDRESS1").ToString
                If dr("ADDRESS1").ToString <> "" Then txtAddress1.ReadOnly = True Else txtAddress1.ReadOnly = False
                txtAddress2.Text = dr("ADDRESS2").ToString
                If dr("ADDRESS2").ToString <> "" Then txtAddress2.ReadOnly = True Else txtAddress2.ReadOnly = False
            End If
        End If
    End Sub
    Private Sub txtAdjCheque_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAdjCheque_AMT.GotFocus
        If objCheaque.Visible Then Exit Sub
        objCheaque.BackColor = Me.BackColor
        objCheaque.StartPosition = FormStartPosition.CenterScreen
        objCheaque.grpCheque.BackgroundColor = Color.Lavender
        objCheaque.ShowDialog()
        Dim chqAmt As Double = Val(objCheaque.gridChequeTotal.Rows(0).Cells("AMOUNT").Value.ToString)
        txtAdjCheque_AMT.Text = IIf(chqAmt <> 0, Format(chqAmt, "0.00"), Nothing)
        txtAdjCash_AMT.Focus()
    End Sub

    Private Sub txtAdjCheque_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtAdjCheque_AMT.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            txtAdjCash_AMT.Select()
            txtAdjCash_AMT.SelectAll()
        End If
    End Sub

    Private Sub txtAdjCash_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtAdjCash_AMT.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            txtAdjCredit_AMT.Select()
        End If
    End Sub
    Private Sub funcCalcBal()
        If GridViewGstTot Is Nothing Then Exit Sub
        If GridViewGstTot.Rows.Count = 0 Then Exit Sub
        Dim Amt As Double = Val(GridViewGstTot.Rows(0).Cells("AMOUNT").Value.ToString) _
                    + Val(GridViewGstTot.Rows(0).Cells("IGST").Value.ToString) _
                    + Val(GridViewGstTot.Rows(0).Cells("CGST").Value.ToString) _
                    + Val(GridViewGstTot.Rows(0).Cells("SGST").Value.ToString) _
                    + Val(GridViewGstTot.Rows(0).Cells("OTHERS").Value.ToString) _
                    - Val(GridViewGstTot.Rows(0).Cells("TDS").Value.ToString)
        Amt -= Val(txtAdjCash_AMT.Text)
        Amt -= Val(txtAdjCheque_AMT.Text)

        txtAdjRoundoff_AMT.Text = Math.Round(Math.Round(Amt, 0) - Math.Round(Amt, 2), 2)
        If Val(txtAdjRoundoff_AMT.Text) <> 0 Then
            Amt += Val(txtAdjRoundoff_AMT.Text)
        End If
        txtAdjCredit_AMT.Text = IIf(Amt <> 0, Format(Amt, "0.00"), DBNull.Value.ToString)
    End Sub

    Private Sub txtAdjCash_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAdjCash_AMT.TextChanged
        funcCalcBal()
    End Sub

    Private Sub txtAdjCheque_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAdjCheque_AMT.TextChanged
        funcCalcBal()
    End Sub

    Private Sub ChequeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChequeToolStripMenuItem.Click
        txtAdjCheque_AMT.Focus()
        txtAdjCheque_AMT.SelectAll()
    End Sub

    Private Sub CashToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CashToolStripMenuItem.Click
        txtAdjCash_AMT.Focus()
        txtAdjCash_AMT.SelectAll()
    End Sub

    Private Sub CreditToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CreditToolStripMenuItem.Click
        txtAdjCredit_AMT.Focus()
        txtAdjCredit_AMT.SelectAll()
    End Sub

    Private Sub txtTDSper_amt_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTDSper_amt.TextChanged
        If Val(txtTDSper_amt.Text) > 0 Then Caltxttdsamt_per()
    End Sub
    Function Caltxttdsamt_per()
        txttds.Text = Val(Val(Val(txtamt_AMT.Text) / 100) * Val(txtTDSper_amt.Text)).ToString()
    End Function

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click

    End Sub

    Private Sub Gridview_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Gridview.KeyDown
        'If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Edit) Then Exit Sub
        If e.KeyCode = Keys.Enter Then
            If Gridview.RowCount > 0 Then

                strSql = vbCrLf + " SELECT COUNT(*) AS CNT FROM " & cnStockDb & "..GSTREGISTER AS T"
                strSql += vbCrLf + " INNER JOIN " & cnStockDb & "..ACCTRAN AS B"
                strSql += vbCrLf + " ON T.TRANDATE = B.TRANDATE AND T.BATCHNO = B.BATCHNO"
                strSql += vbCrLf + " AND T.ACCODE = B.ACCODE"
                strSql += vbCrLf + " AND T.COSTID = B.COSTID"
                strSql += vbCrLf + " WHERE T.BATCHNO = '" & Gridview.CurrentRow.Cells("BATCHNO").Value.ToString & "'"
                ' strSql += vbCrLf + " AND T.TRANDATE = '" & Format(Gridview.CurrentRow.Cells("TRANDATE").Value, "yyyy-MM-dd") & "'"
                Dim dtChequeCheck As New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtChequeCheck)
                If dtChequeCheck.Rows.Count > 0 Then
                    'If Val(dtChequeCheck.Rows(0).Item(0).ToString) > 0 Then
                    '    MsgBox("It cannot be edit.This Entry Cheque Reconciled", MsgBoxStyle.Information)
                    '    Exit Sub
                    'End If
                End If
                funcgstentry(Gridview.CurrentRow.Index)
                tabMain.SelectedTab = tabGen
                editFlag = True
                GridViewGst.Select()
                GridViewGst.Focus()
            End If
        End If
    End Sub

    Private Sub Gridview_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Gridview.KeyPress

    End Sub
    Function funcgstentry(ByVal rwIndex As String) As Integer
        '   strSql = vbCrLf + " SELECT * FROM " & cnStockDb & "..GSTREGISTER AS T"
        Dim dtAcc As New DataTable

        strSql = vbCrLf + "SELECT SNO,TRANNO,convert(varchar(25),TRANDATE,105)TRANDATE ,DESCRIPTION,HSN,AMOUNT,TAX,TAXAMOUNT,SGST_PER,SGST,CGST_PER,CGST,IGST_PER,IGST,"
        strSql += vbCrLf + " (SELECT STATENAME FROM " & cnAdminDb & "..STATEMAST WHERE STATEID=T.STATEID)STATE,("
        strSql += vbCrLf + " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=T.ACCODE)ACNAME,"
        strSql += vbCrLf + " SUPNAME,REFNO BILLNO,convert(varchar(25),REFDATE,105)BILLDATE,GSTCLAIM,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID=T.COSTID)COSTNAME,"
        strSql += vbCrLf + " ADDRESS1,ADDRESS2/*,(SELECT DEPARTNAME FROM " & cnAdminDb & "..DEPARTMENT WHERE DEPARTID=T.DEPARTID)DEPARTMENT*/"
        strSql += vbCrLf + " ,OTHERAMT as OTHERS,RCM,TDSPER,TDS,GSTIN as GSTNO,ACCODE,STATEID/*,DEPARTID*/,COSTID"
        strSql += vbCrLf + " ,(SELECT TOP 1 REMARK1 FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO=T.BATCHNO)REMARK,BATCHNO"
        strSql += vbCrLf + ",(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE =T.CONTRA)DEBIT"
        strSql += vbCrLf + "  FROM " & cnStockDb & "..GSTREGISTER AS T"
        strSql += vbCrLf + " WHERE T.BATCHNO = '" & Gridview.CurrentRow.Cells("BATCHNO").Value.ToString & "'"

        da = New OleDbDataAdapter(strSql, cn)

        da.Fill(dtAcc)
        If Not dtAcc.Rows.Count > 0 Then
RECORD_NotFound:
            MsgBox("Record Not Found", MsgBoxStyle.Information)
            Exit Function
        Else



            GridViewGst.DataSource = Nothing
            GridViewGst.DataSource = dtAcc
            txtTranno.Text = dtAcc.Rows(0).Item("TRANNO").ToString
            dtpDate.Text = dtAcc.Rows(0).Item("TRANDATE").ToString
            txtRefno.Text = dtAcc.Rows(0).Item("BILLNO").ToString
            dtpRefDate.Text = dtAcc.Rows(0).Item("BILLDATE").ToString
            'cmbDepartment.Text = dtAcc.Rows(0).Item("DEPARTMENT").ToString
            cmbAcName_MAN.Text = dtAcc.Rows(0).Item("ACNAME").ToString
            txtAddress1.Text = dtAcc.Rows(0).Item("ADDRESS1").ToString
            txtAddress2.Text = dtAcc.Rows(0).Item("ADDRESS2").ToString
            txtGSTIN.Text = dtAcc.Rows(0).Item("GSTNO").ToString
            CmbCostcentre_OWN.Text = dtAcc.Rows(0).Item("COSTNAME").ToString
            txtNarr_MAN.Text = dtAcc.Rows(0).Item("REMARK").ToString
            CmbContra_Man.Text = dtAcc.Rows(0).Item("DEBIT").ToString
            cmbStateName_MAN.Text = dtAcc.Rows(0).Item("STATE").ToString
            If dtAcc.Rows(0).Item("RCM").ToString = "Y" Then
                chkRCM.Checked = True
            Else
                chkRCM.Checked = False
            End If
            With GridViewGst
                .Columns("TRANNO").Visible = False
                .Columns("TRANDATE").Visible = False
                .Columns("TAX").Visible = False
                .Columns("TAXAMOUNT").Visible = False
                .Columns("SGST_PER").Visible = False
                .Columns("CGST_PER").Visible = False
                .Columns("IGST_PER").Visible = False
                .Columns("REMARK").Visible = False
                .Columns("TDSPER").Visible = False
                .Columns("TDS").Visible = True
                .Columns("SNO").Visible = False
                .Columns("BATCHNO").Visible = False

                .Columns("COSTID").Visible = False
                .Columns("BILLDATE").Visible = False
                .Columns("BILLNO").Visible = False

            End With

        End If
        dtGrid = GridViewGst.DataSource
        GridTotal()
        funcCalcBal()
        'StyleGridSASR(GridViewGst)
    End Function

    Private Sub GridViewGst_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridViewGst.SelectionChanged
        ' If e.KeyChar = Chr(Keys.Enter) Then
        'If GridViewGst.Rows.Count > 0 Then
        '    With GridViewGst.CurrentRow
        '        txtDescrip_MAN.Text = .Cells("DESCRIPTION").Value.ToString
        '        txthsn.Text = .Cells("HSN").Value.ToString
        '        'txtPcs_NUM.Text = .Cells("PCS").Value.ToString
        '        'txtWeight_WET.Text = .Cells("WEIGHT").Value.ToString
        '        'txtRate_AMT.Text = .Cells("RATE").Value.ToString
        '        txtamt_AMT.Text = .Cells("AMOUNT").Value.ToString
        '        txtGstPer_AMT.Text = .Cells("TAX").Value.ToString
        '        txtGstPer_AMT.Text = .Cells("TAXAMOUNT").Value.ToString
        '        txtSgst_per.Text = .Cells("SGST_PER").Value.ToString
        '        txtSgst_AMT.Text = .Cells("SGST").Value.ToString
        '        txtCgst_per.Text = .Cells("CGST_PER").Value.ToString
        '        txtCgst_AMT.Text = .Cells("CGST").Value.ToString
        '        txtIgst_per.Text = .Cells("IGST_PER").Value.ToString
        '        txtIgst_AMT.Text = .Cells("IGST").Value.ToString
        '        txtOthers_AMT.Text = .Cells("OTHERS").Value.ToString
        '        txtRefno.Text = .Cells("BILLNO").Value.ToString
        '        dtpRefDate.Text = .Cells("BILLDATE").Value.ToString
        '        cmbGstClaim.Text = .Cells("GSTCLAIM").Value.ToString
        '        lblEditKeyNo.Text = .Index.ToString
        '        txtDescrip_MAN.Focus()
        '    End With
        'End If
        ' End If
    End Sub

    Private Sub GridViewGst_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles GridViewGst.KeyDown
        If e.KeyCode = Keys.Enter Then
            With GridViewGst.CurrentRow
                txtDescrip_MAN.Text = .Cells("DESCRIPTION").Value.ToString
                txthsn.Text = .Cells("HSN").Value.ToString
                'txtPcs_NUM.Text = .Cells("PCS").Value.ToString
                'txtWeight_WET.Text = .Cells("WEIGHT").Value.ToString
                'txtRate_AMT.Text = .Cells("RATE").Value.ToString
                txtamt_AMT.Text = .Cells("AMOUNT").Value.ToString
                txtGstPer_AMT.Text = .Cells("TAX").Value.ToString
                txtGstPer_AMT.Text = .Cells("TAXAMOUNT").Value.ToString
                txtSgst_per.Text = .Cells("SGST_PER").Value.ToString
                txtSgst_AMT.Text = .Cells("SGST").Value.ToString
                txtCgst_per.Text = .Cells("CGST_PER").Value.ToString
                txtCgst_AMT.Text = .Cells("CGST").Value.ToString
                txtIgst_per.Text = .Cells("IGST_PER").Value.ToString
                txtIgst_AMT.Text = .Cells("IGST").Value.ToString
                txtOthers_AMT.Text = .Cells("OTHERS").Value.ToString
                txtRefno.Text = .Cells("BILLNO").Value.ToString
                dtpRefDate.Text = .Cells("BILLDATE").Value.ToString
                cmbGstClaim.Text = .Cells("GSTCLAIM").Value.ToString
                lblEditKeyNo.Text = .Index.ToString
                txtDescrip_MAN.Focus()
            End With

        End If

    End Sub

    Private Sub btnView_search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_search.Click
        ' tabMain.SelectedTab = TabPage2
        'GridLoad()

        If txtTranno1.Text <> "" Then
            strSql = "SELECT SNO,TRANNO,TRANDATE,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=G.ACCODE)PARTICULAR"
            strSql += vbCrLf + ",isnull((SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=G.CONTRA),'')DEBIT "
            strSql += vbCrLf + ",DESCRIPTION ,(CASE WHEN ISNULL(ACCODE,'')= '' THEN SUPNAME ELSE (SELECT TOP 1 ACNAME FROM " & cnAdminDb & ".. ACHEAD A  WHERE A.ACCODE = G.ACCODE)  END)SUP_NAME ,ISNULL(GSTIN,'')GSTIN,HSN,PCS,WEIGHT,RATE,AMOUNT,TAX,TAXAMOUNT,SGST_PER,SGST,CGST_PER,CGST,IGST_PER,IGST "
            strSql += vbCrLf + ",TDSPER,TDS,STATEID,ACCODE,REFNO,REFDATE,GSTCLAIM, TRANTYPE, UPLOAD, BATCHNO,  "
            strSql += vbCrLf + "NULL AS ORIGINAL_INVOICE_DATE,NULL AS ORIGINAL_INVOICE_NUMBER,NULL AS INVOICE_CATEGORY,NULL AS INVOICE_DUE_TERMS,"
            strSql += vbCrLf + "NULL AS INVOICE_DUE_DATE,NULL AS ORDER_NUMBER,NULL AS ORDER_DATE,NULL AS SUPPLY_TYPE,NULL AS FLAG_EXPORT_INVOICE,"
            strSql += vbCrLf + "NULL AS FLAG_EXPORT_GST_PAYMENT,NULL AS EXPORT_SHIPPING_BILL_NUMBER,NULL AS EXPORT_SHIIPING_BILL_DATE,"
            strSql += vbCrLf + "NULL AS EXPORT_DESTINATION_COUNTRY_CODE,NULL AS CUSTOMER_BILLING_NAME,NULL AS CUSTOMER_BILLING_ADDRESS,"
            strSql += vbCrLf + "NULL AS CUSTOMER_BILLING_CITY,NULL AS CUSTOMER_BILLING_PINCODE,NULL AS CUSTOMER_BILLING_STATE,NULL AS CUSTOMER_BILLING_STATECODE,"
            strSql += vbCrLf + "NULL AS CUSTOMER_BILLING_GSTIN,NULL AS CONSIGNEE_SHIPPING_NAME,NULL AS CONSIGNEE_SHIPPING_ADDRESS,NULL AS CONSIGNEE_SHIPPING_CITY,"
            strSql += vbCrLf + "NULL AS CONSIGNEE_SHIPPING_PINCODE,NULL AS CONSIGNEE_SHIIPING_STATE,NULL AS CONSIGNEE_SHIPPING_STATECODE,NULL AS CONSIGNEE_SHIPPING_GSTIN,"
            strSql += vbCrLf + "NULL AS ITEM_CATEGORY,NULL AS ITEM_QUANTITY,NULL AS ITEM_UNITOFMEASUREMENT,NULL AS ITEM_RATE,NULL AS ITEM_TOTAL_BEFORE_DISCOUNT,"
            strSql += vbCrLf + "NULL AS ITEM_DISCOUNT,NULL AS ITEM_TAXABLE_VALUE,NULL AS ITEM_TOTAL_INCLUDING_GST,NULL AS NIL_RATED_AMOUNT,"
            strSql += vbCrLf + "NULL AS EXEMPTED_AMOUNT,NULL AS NON_GST_AMOUNT,NULL AS FLAG_REVERSE_CHARGE,NULL AS PERCENT_REVERSE_CHARGE_RATE,"
            strSql += vbCrLf + "NULL AS FLAG_TAXPAID_PROVISIONAL_ASSESSMENT,NULL AS MERCHANT_ID_ISSUED_BY_ECOMMERCE,NULL AS GSTIN_ECOMMERCE,"
            strSql += vbCrLf + "NULL AS TAXABLE_VALUE_ON_WHICH_TCS_HAS_BEEN_DEDUCTED,NULL AS TCS_CGST_RATE,NULL AS TCS_CGST_AMOUNT,NULL AS TCS_SGST_RATE,"
            strSql += vbCrLf + "NULL AS TCS_SGST_AMOUNT,NULL AS TCS_IGST_RATE,NULL AS TCS_IGST_AMOUNT,NULL AS ADVANCEPAYMENT_DATE,NULL AS ADVANCEPAYMENT_DOCUMNET_NUMBER,"
            strSql += vbCrLf + "NULL AS SUPPLIER_NAME,NULL AS SUPPLIER_ADDRESS,NULL AS SUPPLIER_CITY,NULL AS SUPPLIER_PINCODE,NULL AS SUPPLIER_STATE,"
            strSql += vbCrLf + "NULL AS SUPPLIER_STATECODE,NULL AS SUPPLIER_GSTIN,NULL AS GSTIN_TDS_DEDUCTOR,NULL AS VALUE_ON_WHICH_TDS_DEDUCTED,"
            strSql += vbCrLf + "NULL AS DATE_OF_PAYMENT_TO_DEDUCTEE_TDS,NULL AS TDS_CGST_RATE,NULL AS TDS_CGST_AMOUNT,NULL AS TDS_SGST_RATE,NULL AS TDS_SGST_AMOUNT,"
            strSql += vbCrLf + "NULL AS TDS_IGST_RATE,NULL AS TDS_IGST_AMOUNT,NULL AS CUSTOM_FIELD_1,NULL AS CUSTOM_FIELD_2,NULL AS CUSTOM_FIELD_3,"
            strSql += vbCrLf + "NULL AS CUSTOM_FIELD_4,NULL AS CUSTOM_FIELD_5,"
            strSql += vbCrLf + "NULL DEPARTMENT"
            strSql += vbCrLf + " FROM " & cnStockDb & "..GSTREGISTER G WHERE ISNULL(CANCEL,'')='' and TRANDATE between '" & dtpfinddate.Value.ToString("yyyy-MM-dd") & "'  and '" & dtpFinddate2.Value.ToString("yyyy-MM-dd") & "' and tranno='" & txtTranno1.Text & "' ORDER BY TRANDATE "
        Else
            strSql = "SELECT SNO,TRANNO,TRANDATE,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=G.ACCODE)PARTICULAR"
            strSql += vbCrLf + ",isnull((SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=G.CONTRA),'')DEBIT "
            strSql += vbCrLf + ",DESCRIPTION,(CASE WHEN ISNULL(ACCODE,'')= '' THEN SUPNAME ELSE (SELECT TOP 1 ACNAME FROM " & cnAdminDb & ".. ACHEAD A  WHERE A.ACCODE = G.ACCODE)  END)SUP_NAME ,ISNULL(GSTIN,'')GSTIN,HSN,PCS,WEIGHT,RATE,AMOUNT,TAX,TAXAMOUNT,SGST_PER,SGST "
            strSql += vbCrLf + ",CGST_PER,CGST,IGST_PER,IGST,TDSPER,TDS,STATEID,ACCODE,REFNO,REFDATE,  "
            strSql += vbCrLf + "GSTCLAIM, TRANTYPE, UPLOAD, BATCHNO,"
            strSql += vbCrLf + "NULL AS ORIGINAL_INVOICE_DATE,NULL AS ORIGINAL_INVOICE_NUMBER,NULL AS INVOICE_CATEGORY,NULL AS INVOICE_DUE_TERMS,"
            strSql += vbCrLf + "NULL AS INVOICE_DUE_DATE,NULL AS ORDER_NUMBER,NULL AS ORDER_DATE,NULL AS SUPPLY_TYPE,NULL AS FLAG_EXPORT_INVOICE,"
            strSql += vbCrLf + "NULL AS FLAG_EXPORT_GST_PAYMENT,NULL AS EXPORT_SHIPPING_BILL_NUMBER,NULL AS EXPORT_SHIIPING_BILL_DATE,"
            strSql += vbCrLf + "NULL AS EXPORT_DESTINATION_COUNTRY_CODE,NULL AS CUSTOMER_BILLING_NAME,NULL AS CUSTOMER_BILLING_ADDRESS,"
            strSql += vbCrLf + "NULL AS CUSTOMER_BILLING_CITY,NULL AS CUSTOMER_BILLING_PINCODE,NULL AS CUSTOMER_BILLING_STATE,NULL AS CUSTOMER_BILLING_STATECODE,"
            strSql += vbCrLf + "NULL AS CUSTOMER_BILLING_GSTIN,NULL AS CONSIGNEE_SHIPPING_NAME,NULL AS CONSIGNEE_SHIPPING_ADDRESS,NULL AS CONSIGNEE_SHIPPING_CITY,"
            strSql += vbCrLf + "NULL AS CONSIGNEE_SHIPPING_PINCODE,NULL AS CONSIGNEE_SHIIPING_STATE,NULL AS CONSIGNEE_SHIPPING_STATECODE,NULL AS CONSIGNEE_SHIPPING_GSTIN,"
            strSql += vbCrLf + "NULL AS ITEM_CATEGORY,NULL AS ITEM_QUANTITY,NULL AS ITEM_UNITOFMEASUREMENT,NULL AS ITEM_RATE,NULL AS ITEM_TOTAL_BEFORE_DISCOUNT,"
            strSql += vbCrLf + "NULL AS ITEM_DISCOUNT,NULL AS ITEM_TAXABLE_VALUE,NULL AS ITEM_TOTAL_INCLUDING_GST,NULL AS NIL_RATED_AMOUNT,"
            strSql += vbCrLf + "NULL AS EXEMPTED_AMOUNT,NULL AS NON_GST_AMOUNT,NULL AS FLAG_REVERSE_CHARGE,NULL AS PERCENT_REVERSE_CHARGE_RATE,"
            strSql += vbCrLf + "NULL AS FLAG_TAXPAID_PROVISIONAL_ASSESSMENT,NULL AS MERCHANT_ID_ISSUED_BY_ECOMMERCE,NULL AS GSTIN_ECOMMERCE,"
            strSql += vbCrLf + "NULL AS TAXABLE_VALUE_ON_WHICH_TCS_HAS_BEEN_DEDUCTED,NULL AS TCS_CGST_RATE,NULL AS TCS_CGST_AMOUNT,NULL AS TCS_SGST_RATE,"
            strSql += vbCrLf + "NULL AS TCS_SGST_AMOUNT,NULL AS TCS_IGST_RATE,NULL AS TCS_IGST_AMOUNT,NULL AS ADVANCEPAYMENT_DATE,NULL AS ADVANCEPAYMENT_DOCUMNET_NUMBER,"
            strSql += vbCrLf + "NULL AS SUPPLIER_NAME,NULL AS SUPPLIER_ADDRESS,NULL AS SUPPLIER_CITY,NULL AS SUPPLIER_PINCODE,NULL AS SUPPLIER_STATE,"
            strSql += vbCrLf + "NULL AS SUPPLIER_STATECODE,NULL AS SUPPLIER_GSTIN,NULL AS GSTIN_TDS_DEDUCTOR,NULL AS VALUE_ON_WHICH_TDS_DEDUCTED,"
            strSql += vbCrLf + "NULL AS DATE_OF_PAYMENT_TO_DEDUCTEE_TDS,NULL AS TDS_CGST_RATE,NULL AS TDS_CGST_AMOUNT,NULL AS TDS_SGST_RATE,NULL AS TDS_SGST_AMOUNT,"
            strSql += vbCrLf + "NULL AS TDS_IGST_RATE,NULL AS TDS_IGST_AMOUNT,NULL AS CUSTOM_FIELD_1,NULL AS CUSTOM_FIELD_2,NULL AS CUSTOM_FIELD_3,"
            strSql += vbCrLf + "NULL AS CUSTOM_FIELD_4,NULL AS CUSTOM_FIELD_5,"
            strSql += vbCrLf + "NULL DEPARTMENT"
            strSql += vbCrLf + " FROM " & cnStockDb & "..GSTREGISTER G WHERE ISNULL(CANCEL,'')='' and TRANDATE BETWEEN '" & dtpfinddate.Value.ToString("yyyy-MM-dd") & "'  and '" & dtpFinddate2.Value.ToString("yyyy-MM-dd") & "' ORDER BY TRANDATE "

        End If
        cmd = New OleDbCommand(strSql, cn)
        dt = New DataTable
        da = New OleDbDataAdapter(cmd)
        da.Fill(dt)
        AutoResizeToolStripMenuItem.Checked = False
        If dt.Rows.Count > 0 Then
            Gridview.DataSource = dt
            funcGridViewStyle()
            GridDesign()
        Else
            MsgBox("Record Not Found", MsgBoxStyle.Information)
        End If


    End Sub

    Private Sub AutoResizeToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AutoResizeToolStripMenuItem.Click
        AutoResize()
    End Sub
    Public Sub AutoResize()
        If Gridview.RowCount > 0 Then
            If AutoResizeToolStripMenuItem.Checked Then
                AutoResizeToolStripMenuItem.Checked = False
            Else
                AutoResizeToolStripMenuItem.Checked = True
            End If
            If AutoResizeToolStripMenuItem.Checked Then
                Gridview.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                Gridview.Invalidate()
                For Each dgvCol As DataGridViewColumn In Gridview.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                Gridview.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            Else
                For Each dgvCol As DataGridViewColumn In Gridview.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                Gridview.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End If
            'gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        End If
    End Sub

    Private Sub btnExprt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExprt.Click
        Me.Cursor = Cursors.WaitCursor
        Dim STRTITLE As String = ""
        STRTITLE = "GST REGISTER BETWEEN"
        STRTITLE += "" & dtpfinddate.Value.ToString & ""
        STRTITLE += " AND " & dtpFinddate2.Value.ToString & ""
        If Gridview.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, STRTITLE, Gridview, BrightPosting.GExport.GExportType.Export)
        End If
        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub txtAdjRoundoff_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAdjRoundoff_AMT.TextChanged


        funcCalcBal()
    End Sub

    Private Sub txtRefno_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtRefno.KeyDown

    End Sub

    Private Sub cmbAcName_MAN_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAcName_MAN.Leave

    End Sub

    Private Sub cmbAcName_MAN_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAcName_MAN.LostFocus
        strSql = "SELECT   count(*) FROM " & cnStockDb & "..GSTREGISTER WHERE REFNO='" & txtRefno.Text & "' AND ISNULL(CANCEL,'')='' AND ACCODE IN(SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME='" & cmbAcName_MAN.Text & "')"
        Dim num As Integer = objGPack.GetSqlValue(strSql)
        If num > 0 Then
            MsgBox("Bill No Already Used...", MsgBoxStyle.Information) : txtRefno.Focus()
            Exit Sub
        End If
    End Sub
End Class