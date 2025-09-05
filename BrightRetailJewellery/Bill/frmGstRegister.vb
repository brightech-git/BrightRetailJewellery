Imports System.Data.OleDb
Imports System.Data
Public Class frmGstRegister
    Dim dtSoftKeys As DataTable = GetAdmindbSoftValueAll()
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim ins As Integer = 0
    Dim dt As DataTable
    Dim da As OleDbDataAdapter
    Dim dtGrid As New DataTable
    Dim dtGridTot As New DataTable
    Dim objTax As New frmGSTTax("Y", stateId, CompanyStateId)
    Dim objTds As New frmAccTdsGST
    'Dim taxType As String
    Dim objCreditCard As New frmCreditCardAdj
    Dim objCheaque As New frmChequeAdj
    'Dim objAddressDia As New frmAddressDia(True)
    Dim stateId As Integer
    Dim gstCatId As Integer
    Dim TdsCatId As Integer
    Dim tranDate As Date
    Dim tranDateTO As Date
    Dim _Accode As String = Nothing
    'Dim _CashCode As String = "CASH"
    Dim existGstCatId As Integer
    Dim EditBckId As String = Nothing
    Dim editBatchno As String = Nothing
    Dim EditTranNo As Integer = Nothing
    Dim EditCostId As String = Nothing
    Dim Addvalid As String = GetAdmindbSoftValuefromDt(dtSoftKeys, "PER_INFO_VALID", "")
    Dim GST_GEN_TAXEDIT As Boolean = IIf(GetAdmindbSoftValuefromDt(dtSoftKeys, "GST_GEN_TAXEDIT", "N") = "Y", True, False)
    Dim GST_GEN_WITHCRACC As Boolean = IIf(GetAdmindbSoftValuefromDt(dtSoftKeys, "GST_GEN_WITHCRACC", "N") = "Y", True, False)
    Dim _CashCode As String = GetAdmindbSoftValuefromDt(dtSoftKeys, "GST_GEN_CASH_ACCODE", "") '"CASH"
    Dim GST_GEN_CTRLID As String = GetAdmindbSoftValuefromDt(dtSoftKeys, "GST_GEN_CTRLID", "")
    Dim GST_GEN_CACC_POST_REG As Boolean = IIf(GetAdmindbSoftValuefromDt(dtSoftKeys, "GST_GEN_CACC_POST_REG", "N") = "Y", True, False)
    Dim _BankCode As String = ""
    Dim dtLoadGstClaimRegister As New DataTable
    Dim dtLoadGstClaimUnRegister As New DataTable
    Dim dtLoadUnRegistercr As New DataTable
    Dim dtLoadRegisterCR As New DataTable
    Private Sub GridTotal()
        Dim drk As DataRow
        dtGridTot.Rows.Clear()
        drk = dtGridTot.NewRow
        If dtGrid.Rows.Count > 0 Then
            drk.Item("DESCRIPTION") = "Total"
            drk.Item("PCS") = dtGrid.Compute("SUM(PCS)", Nothing)
            drk.Item("WEIGHT") = dtGrid.Compute("SUM(WEIGHT)", Nothing)
            drk.Item("AMOUNT") = dtGrid.Compute("SUM(AMOUNT)", Nothing)
            drk.Item("TDS") = dtGrid.Compute("SUM(TDS)", Nothing)
            drk.Item("TCS") = dtGrid.Compute("SUM(TCS)", Nothing)
            drk.Item("SGST") = dtGrid.Compute("SUM(SGST)", Nothing)
            drk.Item("CGST") = dtGrid.Compute("SUM(CGST)", Nothing)
            drk.Item("IGST") = dtGrid.Compute("SUM(IGST)", Nothing)
        Else
            drk.Item("DESCRIPTION") = "Total"
            drk.Item("PCS") = 0
            drk.Item("WEIGHT") = 0
            drk.Item("AMOUNT") = 0
            drk.Item("SGST") = 0
            drk.Item("CGST") = 0
            drk.Item("IGST") = 0
            drk.Item("TDS") = 0
            drk.Item("TCS") = 0
        End If
        dtGridTot.Rows.Add(drk)
        GridViewGstTot.DataSource = Nothing
        GridViewGstTot.DataSource = dtGridTot
        StyleGridSASR(GridViewGstTot)
        Dim wt As Double = Nothing
        For Each Ro As DataRow In dtGridTot.Rows
            wt = Val(Ro.Item("AMOUNT").ToString) + Val(Ro.Item("SGST").ToString) + Val(Ro.Item("CGST").ToString) + Val(Ro.Item("IGST").ToString) + Val(Ro.Item("TCS").ToString)
            TxtAmount.Text = IIf(wt <> 0, Format(wt, "0.00"), "")
        Next
    End Sub
    Public Sub funcClear()
        txtRefno.Text = ""
        txtAdjCash_AMT.Text = ""
        txtAdjCredit_AMT.Text = ""
        txtAdjCheque_AMT.Text = ""
        txtAdjDue_AMT.Text = ""
        txtDescrip_MAN.Text = ""
        txthsn.Text = ""
        txtPcs_NUM.Text = ""
        txtWeight_WET.Text = ""
        txtamt_AMT.Text = ""
        txtRate_AMT.Text = ""
        txtGstPer_AMT.Text = ""
        txtRemark1.Text = ""
        txtRemark2.Text = ""
        txtTDS_AMT.Text = ""
        txtTCS_AMT.Text = ""
        txtSgst_per.Text = ""
        txtSgst_AMT.Text = ""
        txtCgst_per.Text = ""
        txtCgst_AMT.Text = ""
        txtIgst_per.Text = ""
        txtIgst_AMT.Text = ""
        TxtAmount.Text = ""
        'cmbGstClaim.Text = "GST"
        cmbDrAcName_OWN.Text = ""
        cmbCrAcName_OWN.Text = ""
        cmbStateName_MAN.Text = ""
        txtGsttot_Amt.Text = ""
        TxtAmountTotal.Text = ""
        txtSupName.Text = ""
        lblEditKeyNo.Text = ""
        txtGSTIN.Text = ""
        txtAddress1.Text = ""
        txtAddress2.Text = ""
        lblEditKeyNo.Visible = False
        dtGrid = New DataTable
        objTax = New frmGSTTax("Y", stateId, CompanyStateId)
        With dtGrid
            Dim col As New DataColumn("KEYNO")
            col.AutoIncrement = True
            col.AutoIncrementSeed = 1
            col.AutoIncrementStep = 1
            .Columns.Add(col)
            .Columns.Add("GSTCLAIM", GetType(String))
            .Columns.Add("GSTCLAIMID", GetType(String))
            .Columns.Add("CRACNAME", GetType(String))
            .Columns.Add("CRACCCODE", GetType(String))
            .Columns.Add("DESCRIPTION", GetType(String))
            .Columns.Add("HSN", GetType(String))
            .Columns.Add("PCS", GetType(Integer))
            .Columns.Add("WEIGHT", GetType(Double))
            .Columns.Add("RATE", GetType(Double))
            .Columns.Add("AMOUNT", GetType(Double))
            .Columns.Add("TDS_PER", GetType(Double))
            .Columns.Add("TDS", GetType(Double))
            .Columns.Add("TAX", GetType(Double))
            .Columns.Add("TAXAMOUNT", GetType(Double))
            .Columns.Add("SGST_PER", GetType(Double))
            .Columns.Add("SGST", GetType(Double))
            .Columns.Add("CGST_PER", GetType(Double))
            .Columns.Add("CGST", GetType(Double))
            .Columns.Add("IGST_PER", GetType(Double))
            .Columns.Add("IGST", GetType(Double))
            .Columns.Add("STATEID", GetType(Integer))
            .Columns.Add("ACCODE", GetType(String))
            .Columns.Add("TCS", GetType(Double))
            .Columns.Add("REFDATE", GetType(String))
            .Columns.Add("REFNO", GetType(String))
            .Columns.Add("GSTNO", GetType(String))
            .Columns.Add("ADDRESS1", GetType(String))
            .Columns.Add("ADDRESS2", GetType(String))
            .Columns.Add("GSTCATID", GetType(Integer))
        End With
        GridViewGst.DataSource = Nothing
        GridViewGst.DataSource = dtGrid
        StyleGridSASR(GridViewGst)
        'objAddressDia = New frmAddressDia()
        dtGridTot = dtGrid.Copy
        GridViewGstTot.DataSource = Nothing
        GridViewGstTot.DataSource = dtGridTot
        GridTotal()
        GridViewGst.Focus()
        dtpTranDate.Focus()

        If GST_GEN_TAXEDIT Then
            txtSgst_AMT.ReadOnly = True
            txtCgst_AMT.ReadOnly = True
            txtIgst_AMT.ReadOnly = True
        Else
            txtSgst_AMT.ReadOnly = False
            txtCgst_AMT.ReadOnly = False
            txtIgst_AMT.ReadOnly = False
        End If
        rbtWOAcc_CheckedChanged(Me, New EventArgs)
    End Sub

    Private Sub funLoaddatatableRegister()
        cmbGstClaim.DataSource = Nothing
        cmbGstClaim.DataSource = dtLoadGstClaimRegister
        cmbGstClaim.ValueMember = "GSTCLAIMID"
        cmbGstClaim.DisplayMember = "GSTCLAIMNAME"
    End Sub

    Private Sub funLoaddatatableUnRegister()
        cmbGstClaim.DataSource = Nothing
        cmbGstClaim.DataSource = dtLoadGstClaimUnRegister
        cmbGstClaim.ValueMember = "GSTCLAIMID"
        cmbGstClaim.DisplayMember = "GSTCLAIMNAME"
    End Sub

    Private Sub funGstClaimLoadRegister(ByVal Enttype As String)
        cmbGstClaim.DataSource = Nothing
        strSql = ""
        strSql = ""
        strSql += vbCrLf + " SELECT GSTCLAIMID,GSTCLAIMNAME "
        strSql += vbCrLf + " FROM " & cnAdminDb & "..GSTCLAIM "
        strSql += vbCrLf + " WHERE ENTTYPE='" & Enttype & "' "
        strSql += vbCrLf + "  AND ACTIVE ='Y'"
        strSql += vbCrLf + " ORDER BY DISPLAYORDER"
        da = New OleDbDataAdapter(strSql, cn)
        dt = New DataTable
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            If Enttype = "U" Then
                dtLoadGstClaimUnRegister = dt.Copy
            ElseIf Enttype = "R" Then
                dtLoadGstClaimRegister = dt.Copy
            End If
        End If
        strSql = ""
        dt = New DataTable
        da = New OleDbDataAdapter
    End Sub

    Private Sub frmGstRegister_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGen.Left, Me.tabGen.Top, Me.tabGen.Width, Me.tabGen.Height))
        pnlGroupFilter.Location = New Point((ScreenWid - pnlGroupFilter.Width) / 2, ((ScreenHit - 128) - pnlGroupFilter.Height) / 2)
        rbtWOAcc.Select()
        'cmbGstClaim.Items.Add("GST")
        'cmbGstClaim.Items.Add("RCM")
        'cmbGstClaim.Text = "GST"
        'taxType = "Y"
        btnSave.Enabled = False
        'funcTranno()
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
        grpPayment.BackColor = Color.Lavender
        Panel6.BackColor = Color.Lavender
        pnlRemark.BackColor = Color.Lavender
        funGstClaimLoadRegister("U")
        funGstClaimLoadRegister("R")
        AcName()
        LoadcmbxCreditAccount()
        funcClear()
        StateName()
        CostName()
        If _IsCostCentre = False Then
            cmbCostCentre_MAN.Enabled = False
        End If

        'tranDate = dtpSrcTrandate.Value.Date.ToString("dd-MM-yyyy")
    End Sub
    Private Sub frmGstRegister_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Enter Then
            If txtamt_AMT.Focused Then Exit Sub
            If txtRefno.Focused Then Exit Sub
            If GridViewGst.Focused Then e.Handled = True : Exit Sub
            SendKeys.Send("{TAB}")
        ElseIf e.KeyCode = Keys.Escape Then
            If txtDescrip_MAN.Focused Then
                Dim Amt As Double
                If Not GridViewGstTot Is Nothing Then
                    Amt = Val(GridViewGstTot.Rows(0).Cells("AMOUNT").Value.ToString) _
                        + Val(GridViewGstTot.Rows(0).Cells("IGST").Value.ToString) _
                        + Val(GridViewGstTot.Rows(0).Cells("CGST").Value.ToString) _
                        + Val(GridViewGstTot.Rows(0).Cells("SGST").Value.ToString) + Val(GridViewGstTot.Rows(0).Cells("TCS").Value.ToString)
                    txtAdjCash_AMT.Text = Format(Amt, "0.00")
                End If
                If rbtWithAcc.Checked Then
                    If GST_GEN_WITHCRACC Then
                        txtAdjCredit_AMT.Focus()
                        txtAdjCredit_AMT.SelectAll()
                    Else
                        If GST_GEN_CACC_POST_REG = True Then
                            txtAdjDue_AMT.Text = Val(txtAdjCash_AMT.Text) + Val(txtAdjCheque_AMT.Text)
                            txtAdjCash_AMT.Clear()
                            txtAdjCheque_AMT.Clear()
                            txtAdjDue_AMT.Focus()
                            txtAdjDue_AMT.SelectAll()
                        Else
                            btnSave.Focus()
                        End If
                    End If
                Else
                    txtAdjCash_AMT.Focus()
                    txtAdjCash_AMT.Select()
                End If
                Exit Sub
            End If
            dtpTranDate.Focus()
            tabMain.SelectedTab = tabGen
        End If
    End Sub
    Private Sub StyleGridSASR(ByVal GridViewGst As DataGridView)
        With GridViewGst
            .Columns("CRACCCODE").Visible = False
            .Columns("ACCODE").Visible = False
            .Columns("STATEID").Visible = False
            .Columns("KEYNO").Visible = False
            .Columns("WEIGHT").Visible = False
            .Columns("GSTCATID").Visible = False
            .Columns("SGST_PER").Visible = False
            .Columns("CGST_PER").Visible = False
            .Columns("IGST_PER").Visible = False
            .Columns("TDS_PER").Visible = False
            .Columns("TAX").Visible = False
            .Columns("TAXAMOUNT").Visible = False
            .Columns("GSTCLAIMID").Visible = False
            .Columns("GSTCLAIM").Width = cmbGstClaim.Width + 1
            .Columns("CRACNAME").Width = cmbCrAcName_OWN.Width + 2
            .Columns("DESCRIPTION").Width = txtDescrip_MAN.Width + 2
            .Columns("HSN").Width = txthsn.Width + 0
            .Columns("PCS").Width = txtPcs_NUM.Width + 1
            .Columns("RATE").Width = txtRate_AMT.Width + 1
            .Columns("AMOUNT").Width = txtamt_AMT.Width + 1
            .Columns("TDS").Width = txtTDS_AMT.Width + 1
            .Columns("TCS").Width = txtTCS_AMT.Width + 1
            .Columns("TAX").Width = txtGstPer_AMT.Width + 1
            .Columns("TAXAMOUNT").Width = txtGsttot_Amt.Width + 1
            .Columns("SGST_PER").Width = txtSgst_per.Width + 1
            .Columns("SGST").Width = txtSgst_AMT.Width + 1
            .Columns("CGST_PER").Width = txtCgst_per.Width + 1
            .Columns("CGST").Width = txtCgst_AMT.Width + 1
            .Columns("IGST_PER").Width = txtIgst_per.Width + 1
            .Columns("IGST").Width = txtIgst_AMT.Width + 1
            .Columns("STATEID").Width = cmbStateName_MAN.Width
            .Columns("ACCODE").Width = cmbDrAcName_OWN.Width + 0
            .Columns("REFDATE").Width = dtpRefDate.Width + 2
            .Columns("REFNO").Width = txtRefno.Width + 0
            '.Columns("WEIGHT").Width = txtWeight_WET.Width + 0
            .Columns("GSTNO").Visible = False
            .Columns("ADDRESS1").Visible = False
            .Columns("ADDRESS2").Visible = False
            .Columns("GSTCATID").Visible = False

            .Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("RATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("TDS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("TCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("TAX").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("TAXAMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("SGST_PER").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("SGST").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("CGST_PER").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("CGST").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("IGST_PER").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("IGST").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            .Columns("RATE").DefaultCellStyle.Format = "0.00"
            .Columns("AMOUNT").DefaultCellStyle.Format = "0.00"
            .Columns("TAX").DefaultCellStyle.Format = "0.00"
            .Columns("TAXAMOUNT").DefaultCellStyle.Format = "0.00"
            .Columns("SGST_PER").DefaultCellStyle.Format = "0.00"
            .Columns("SGST").DefaultCellStyle.Format = "0.00"
            .Columns("CGST_PER").DefaultCellStyle.Format = "0.00"
            .Columns("CGST").DefaultCellStyle.Format = "0.00"
            .Columns("IGST_PER").DefaultCellStyle.Format = "0.00"
            .Columns("IGST").DefaultCellStyle.Format = "0.00"
            .Columns("TDS").DefaultCellStyle.Format = "0.00"
            .Columns("TCS").DefaultCellStyle.Format = "0.00"
            If GridViewGst.Name = "GridViewGstTot" Then
                .DefaultCellStyle.Font = New Font("verdana", 9, FontStyle.Bold)
                .DefaultCellStyle.ForeColor = Color.Red
            End If
        End With
    End Sub

    Private Function ShowAddressDia() As Boolean
        'If objAddressDia.Visible Then Exit Function
        'objAddressDia.BackColor = Panel5.BackColor
        'objAddressDia.StartPosition = FormStartPosition.CenterScreen
        'objAddressDia.MaximizeBox = False
        'objAddressDia.grpAddress.BackgroundColor = Color.Lavender
        'objAddressDia.txtMobile.Select()
        'If _IsWholeSaleType Then
        '    If objAddressDia.ShowDialog() = Windows.Forms.DialogResult.OK Then
        '        Return True
        '    End If
        'Else
        '    If objAddressDia.ShowDialog() = Windows.Forms.DialogResult.OK Then
        '        btnSave.Select()
        '    End If
        'End If
    End Function

    Private Sub LoadcmcraccReg()
        cmbCrAcName_OWN.DataSource = Nothing
        cmbCrAcName_OWN.DataSource = dtLoadRegisterCR
        cmbCrAcName_OWN.ValueMember = "ACCODE"
        cmbCrAcName_OWN.DisplayMember = "ACNAME"
    End Sub

    Private Sub LoadcmcraccunReg()
        cmbCrAcName_OWN.DataSource = Nothing
        cmbCrAcName_OWN.DataSource = dtLoadUnRegistercr
        cmbCrAcName_OWN.ValueMember = "ACCODE"
        cmbCrAcName_OWN.DisplayMember = "ACNAME"
    End Sub

    Private Sub LoadcmbxCreditAccount()
        If editBatchno <> "" Then
            Exit Sub
        End If
        strSql = ""
        da = New OleDbDataAdapter
        dt = New DataTable
        If True Then 'rbtWithAcc.Checked
            'crAccode = objGPack.GetSqlValue("Select ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbCrAcName.Text & "'", , , tran)
            strSql = " SELECT ACCODE,ACNAME FROM " & cnAdminDb & "..ACHEAD "
            strSql += vbCrLf + " WHERE ACTYPE IN (SELECT TYPEID FROM " & cnAdminDb & "..ACCTYPE /*WHERE ACTYPE='E'*/)"
            strSql += vbCrLf + " ORDER BY ACNAME"
            da = New OleDbDataAdapter(strSql, cn)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                dtLoadRegisterCR = dt.Copy
            End If
            'Else
            'strSql = "SELECT * FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME='SUNDRY CREDITORS'"
            'crAccode = objGPack.GetSqlValue(strSql, "", "")
            'If crAccode = "" Then
            '    strSql = "SELECT * FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE='CRS'"
            '    crAccode = objGPack.GetSqlValue(strSql, "", "")
            'End If
            strSql = " SELECT ACCODE,ACNAME FROM " & cnAdminDb & "..ACHEAD " '/*WHERE ACNAME IN ('SUNDRY CREDITORS','CRS')*/
            da = New OleDbDataAdapter(strSql, cn)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                'cmbCrAcName_OWN.DataSource = Nothing
                'cmbCrAcName_OWN.DataSource = dt
                'cmbCrAcName_OWN.ValueMember = "ACCODE"
                'cmbCrAcName_OWN.DisplayMember = "ACNAME"
                dtLoadUnRegistercr = dt.Copy
            End If
        End If
        LoadcmcraccunReg()
        'LoadcmcraccReg() No Need 
        strSql = ""
        da = New OleDbDataAdapter
        dt = New DataTable
        'If crAccode = "" Then
        '    MsgBox("SUNDRY CREDITORS Accode not found in master.", MsgBoxStyle.Information) : Exit Sub
        'End If
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click, btnSave.Click
        If Not GridViewGst.Rows.Count > 0 Then Exit Sub
        Dim accode As String = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbDrAcName_OWN.Text & "'", , , tran)
        Dim stateid As String = objGPack.GetSqlValue("SELECT STATEID FROM " & cnAdminDb & "..STATEMAST WHERE STATENAME= '" & cmbStateName_MAN.Text & "' ", , , tran)
        Dim costId As String = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME= '" & cmbCostCentre_MAN.Text & "' ", , , tran)
        Dim GstDR As DataRow = Nothing
        Dim GSTCGSTAC As String = Nothing
        Dim GSTSGSTAC As String = Nothing
        Dim GSTIGSTAC As String = Nothing
        Dim GSTRCMFLG As String = Nothing
        Dim RGSTCGSTAC As String = Nothing
        Dim RGSTSGSTAC As String = Nothing
        Dim RGSTIGSTAC As String = Nothing
        Dim itemDesc As String = ""
        Dim RCMCGST As Decimal = Nothing
        Dim RCMSGST As Decimal = Nothing
        Dim RCMIGST As Decimal = Nothing
        If Val(txtAdjCredit_AMT.Text) = 0 Then
        ElseIf Val(txtAdjCredit_AMT.Text) > -5 And Val(txtAdjCredit_AMT.Text) < 5 Then
        Else
            MsgBox("Round Off Range Mininum From (-5) to Maximum (5) Allowed ", MsgBoxStyle.Information)
            txtAdjCash_AMT.Focus()
            txtAdjCash_AMT.SelectAll()
            Exit Sub
        End If
        Try
            Dim Tranno As Integer
            Dim Batchno As String = ""
            Dim crAccode As String = ""
            'Dim drAccode As String = ""
            If rbtWithAcc.Checked = False Then
                'If Addvalid.Contains("N") And objAddressDia.txtAddressName.Text = "" And objAddressDia.txtAddressName.ReadOnly = False Then MsgBox("Please check, Address Empty", MsgBoxStyle.Information) : ShowAddressDia() : objAddressDia.txtAddressName.Focus() : Exit Sub
                'If Addvalid.Contains("AD1") And objAddressDia.txtAddress1.Text = "" And objAddressDia.txtAddress1.ReadOnly = False Then MsgBox("Please check, Address Empty", MsgBoxStyle.Information) : ShowAddressDia() : objAddressDia.txtAddress1.Focus() : Exit Sub
                'If Addvalid.Contains("PAN") And objAddressDia.txtAddressPan.Text = "" And objAddressDia.txtAddressPan.ReadOnly = False Then MsgBox("Please check, Address Empty", MsgBoxStyle.Information) : ShowAddressDia() : objAddressDia.txtAddressPan.Focus() : Exit Sub
                'If Addvalid.Contains("AD2") And objAddressDia.txtAddress2.Text = "" And objAddressDia.txtAddress2.ReadOnly = False Then MsgBox("Please check, Address Empty", MsgBoxStyle.Information) : ShowAddressDia() : objAddressDia.txtAddress2.Focus() : Exit Sub
                'If Addvalid.Contains("AD3") And objAddressDia.txtAddress3.Text = "" And objAddressDia.txtAddress3.ReadOnly = False Then MsgBox("Please check, Address Empty", MsgBoxStyle.Information) : ShowAddressDia() : objAddressDia.txtAddress3.Focus() : Exit Sub
                'If Addvalid.Contains("DR") And objAddressDia.txtAddressDoorNo.Text = "" And objAddressDia.txtAddressDoorNo.ReadOnly = False Then MsgBox("Please check, Address Empty", MsgBoxStyle.Information) : ShowAddressDia() : objAddressDia.txtAddressDoorNo.Focus() : Exit Sub
                'If Addvalid.Contains("EL") And objAddressDia.txtAddressEmailId_OWN.Text = "" And objAddressDia.txtAddressEmailId_OWN.ReadOnly = False Then MsgBox("Please check, Address Empty", MsgBoxStyle.Information) : ShowAddressDia() : objAddressDia.txtAddressEmailId_OWN.Focus() : Exit Sub
                'If Addvalid.Contains("FX") And objAddressDia.txtAddressFax.Text = "" And objAddressDia.txtAddressFax.ReadOnly = False Then MsgBox("Please check, Address Empty", MsgBoxStyle.Information) : ShowAddressDia() : objAddressDia.txtAddressFax.Focus() : Exit Sub
                'If Addvalid.Contains("M") And objAddressDia.txtAddressMobile.Text = "" And objAddressDia.txtAddressMobile.ReadOnly = False Then MsgBox("Please check, Address Empty", MsgBoxStyle.Information) : ShowAddressDia() : objAddressDia.txtAddressMobile.Focus() : Exit Sub
                'If Addvalid.Contains("PR") And objAddressDia.txtAddressPhoneRes.Text = "" And objAddressDia.txtAddressPhoneRes.ReadOnly = False Then MsgBox("Please check, Address Empty", MsgBoxStyle.Information) : ShowAddressDia() : objAddressDia.txtAddressPhoneRes.Focus() : Exit Sub
                'If Addvalid.Contains("PC") And objAddressDia.txtAddressPincode_NUM.Text = "" And objAddressDia.txtAddressPincode_NUM.ReadOnly = False Then ShowAddressDia() : objAddressDia.txtAddressPincode_NUM.Focus() : Exit Sub
                'If Addvalid.Contains("AR") And objAddressDia.cmbAddressArea_OWN.Text = "" And objAddressDia.cmbAddressArea_OWN.Enabled = True Then MsgBox("Please check, Address Empty", MsgBoxStyle.Information) : ShowAddressDia() : objAddressDia.cmbAddressArea_OWN.Focus() : Exit Sub
                'If Addvalid.Contains("CY") And objAddressDia.cmbAddressCity_OWN.Text = "" And objAddressDia.cmbAddressCity_OWN.Enabled = True Then MsgBox("Please check, Address Empty", MsgBoxStyle.Information) : ShowAddressDia() : objAddressDia.cmbAddressCity_OWN.Focus() : Exit Sub
                'If Addvalid.Contains("ST") And objAddressDia.cmbAddressState.Text = "" And objAddressDia.cmbAddressState.Enabled = True Then MsgBox("Please check, Address Empty", MsgBoxStyle.Information) : ShowAddressDia() : objAddressDia.cmbAddressState.Focus() : Exit Sub
                'If Addvalid.Contains("CN") And objAddressDia.cmbAddressCountry_OWN.Text = "" And objAddressDia.cmbAddressCountry_OWN.Enabled = True Then MsgBox("Please check, Address Empty", MsgBoxStyle.Information) : ShowAddressDia() : objAddressDia.cmbAddressCountry_OWN.Focus() : Exit Sub
            End If
            tran = Nothing
            tran = cn.BeginTransaction

            Dim _GRtrantype As String = "JE"
            If GST_GEN_CTRLID.ToString = "GEN-JE" Then
                _GRtrantype = "JE"
            Else
                _GRtrantype = "PE"
            End If
            If editBatchno = Nothing Then
                Dim Isfirst As Boolean = True
                Dim ctrlId As String = GST_GEN_CTRLID '"GEN-JE"

GenBillNo:
                Tranno = Val(GetBillControlValue(ctrlId, tran, Not Isfirst))
                strSql = " UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT = '" & Tranno + 1 & "'"
                strSql += " WHERE CTLID = '" & ctrlId & "' AND COMPANYID = '" & strCompanyId & "'"
                If Isfirst And strBCostid <> Nothing Then strSql += " AND COSTID ='" & strBCostid & "'"
                strSql += " AND CONVERT(INT,CTLTEXT) = '" & Tranno & "'"
                cmd = New OleDbCommand(strSql, cn, tran)
                If Not cmd.ExecuteNonQuery() > 0 Then
                    If strBCostid <> Nothing Then MsgBox("Tran No. empty. Please check Bill control") : tran.Rollback() : tran.Dispose() : tran = Nothing : Exit Sub
                    Isfirst = False
                    GoTo GenBillNo
                End If
                Tranno += 1
                Batchno = GetNewBatchno(cnCostId, GetServerDate(tran), tran)
            Else
                strSql = " DELETE FROM " & cnStockDb & "..GSTREGISTER WHERE BATCHNO = '" & editBatchno & "'"
                strSql += vbCrLf + " DELETE FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = '" & editBatchno & "'"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId)
                If costId <> EditCostId Then
                    Exec(strSql.Replace("'", "''"), cn, EditCostId, Nothing, tran)
                End If
                Tranno = EditTranNo
                Batchno = editBatchno
            End If
            'objAddressDia.PersonalinfoInsert = False
            Dim PSno As String = ""
            'If rbtWithAcc.Checked = False Then
            '    If objAddressDia.txtAddressName.Text <> "" Or objAddressDia.txtRemark1.Text <> "" Then PSno = objAddressDia.InsertIntoPersonalInfo(GetServerDate(tran), cnCostId, Batchno, tran)
            'End If
            Dim GstAccRefno As String = ""
            Dim GstAccRefdate As String = ""
            For Each ro As DataGridViewRow In GridViewGst.Rows
                Dim Sno As String = GetNewSno(TranSnoType.GSTREGISTERCODE, tran)
                Dim refDateAr As String() = ro.Cells("REFDATE").Value.ToString.Split("-")
                'itemDesc = itemDesc + "-" + ro.Cells("DESCRIPTION").Value.ToString
                Dim refDate As Date
                If refDateAr.Length = 3 Then
                    refDate = New Date(refDateAr(2).ToString, refDateAr(1).ToString, refDateAr(0).ToString)
                End If
                strSql = " INSERT INTO " & cnStockDb & "..GSTREGISTER(SNO,BATCHNO "
                strSql += vbCrLf + " ,TRANDATE,TRANNO,DESCRIPTION,HSN,PCS,WEIGHT,RATE,AMOUNT,"
                strSql += vbCrLf + " TAX,TAXAMOUNT,SGST_PER,SGST,CGST_PER,CGST,IGST_PER,IGST,STATEID"
                strSql += vbCrLf + " ,ACCODE,CONTRA,SUPNAME,REFNO,REFDATE,GSTCLAIM"
                strSql += vbCrLf + " ,GSTIN,ADDRESS1,ADDRESS2,COSTID,GSTCATID,COMPANYID,TDSPER,TDS,ENTTYPE"
                strSql += vbCrLf + " ,TRANTYPE"
                strSql += vbCrLf + " )"
                strSql += vbCrLf + " VALUES("
                strSql += vbCrLf + " '" & Sno & "'"
                strSql += vbCrLf + " ,'" & Batchno & "'"
                strSql += vbCrLf + " ,'" & dtpTranDate.Value.ToString("yyyy-MM-dd") & "'"
                strSql += vbCrLf + " , '" & Tranno & "' "
                strSql += vbCrLf + " ,'" & ro.Cells("DESCRIPTION").Value.ToString & "'"
                strSql += vbCrLf + " ,'" & ro.Cells("HSN").Value.ToString & "'"
                strSql += vbCrLf + " ,'" & Val(ro.Cells("PCS").Value.ToString) & "'"
                strSql += vbCrLf + " ,'" & Val(ro.Cells("WEIGHT").Value.ToString) & "'"
                strSql += vbCrLf + " ,'" & Val(ro.Cells("RATE").Value.ToString) & "'"
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
                strSql += vbCrLf + " ,'" & ro.Cells("CRACCCODE").Value.ToString & "'" 'crAccode
                strSql += vbCrLf + " ,'" & txtSupName.Text.ToString & "'"
                strSql += vbCrLf + " ,'" & ro.Cells("REFNO").Value.ToString & "'"
                strSql += vbCrLf + " ,'" & refDate.ToString("yyyy-MM-dd") & "'"
                strSql += vbCrLf + " ,'" & ro.Cells("GSTCLAIMID").Value.ToString & "' "
                strSql += vbCrLf + " ,'" & ro.Cells("GSTNO").Value.ToString & "' "
                strSql += vbCrLf + " ,'" & ro.Cells("ADDRESS1").Value.ToString & "' "
                strSql += vbCrLf + " ,'" & ro.Cells("ADDRESS2").Value.ToString & "' "
                strSql += vbCrLf + " ,'" & costId & "'"
                strSql += vbCrLf + " ,'" & ro.Cells("GSTCATID").Value.ToString & "'"
                strSql += vbCrLf + " ,'" & strCompanyId & "'"   'COMPANYID
                strSql += vbCrLf + " ,'" & Val(ro.Cells("TDS_PER").Value.ToString) & "'"
                strSql += vbCrLf + " ,'" & Val(ro.Cells("TDS").Value.ToString) & "'"
                If rbtWithAcc.Checked = True Then
                    strSql += vbCrLf + " ,'R'"
                ElseIf rbtWOAcc.Checked = True Then
                    strSql += vbCrLf + " ,'U'"
                End If
                strSql += vbCrLf + " ,'" & _GRtrantype & "'"
                strSql += vbCrLf + ")"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnHOCostId)
                GstAccRefno = ro.Cells("REFNO").Value.ToString
                GstAccRefdate = refDate.ToString("yyyy-MM-dd")
                crAccode = ro.Cells("CRACCCODE").Value.ToString
            Next
            'Insert into Acctran
            Dim Trfamount As Double = 0
            Trfamount = Val(dtGrid.Compute("SUM(AMOUNT)", Nothing).ToString)
            Dim TdsAmt As Double = 0
            TdsAmt = Val(dtGrid.Compute("SUM(TDS)", Nothing).ToString)

            Dim TCsAmt As Double = 0
            TCsAmt = Val(dtGrid.Compute("SUM(TCS)", Nothing).ToString)
            If rbtWithAcc.Checked = True Then
                Trfamount = 0
                Trfamount = Val(dtGrid.Compute("SUM(AMOUNT)", Nothing).ToString) _
                    + Val(dtGrid.Compute("SUM(IGST)", Nothing).ToString) _
                    + Val(dtGrid.Compute("SUM(CGST)", Nothing).ToString) _
                    + Val(dtGrid.Compute("SUM(SGST)", Nothing).ToString)
                InsertIntoAccTran(Tranno, "C", accode, Trfamount, _GRtrantype.ToString, crAccode, Batchno, GstAccRefno, GstAccRefdate, , , , , txtRemark1.Text, txtRemark2.Text)
            ElseIf rbtWOAcc.Checked = True Then
                If Val(txtAdjCash_AMT.Text) > 0 Then
                    _CashCode = _CashCode
                End If
                If Val(txtAdjCheque_AMT.Text) > 0 Then
                    For Each ro As DataRow In objCheaque.dtGridCheque.Rows
                        Dim BANKCODE As String = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & ro!DEFAULTBANK.ToString & "'", , , tran).ToString
                        _BankCode = BANKCODE
                    Next
                End If
            End If
            Dim dtGst As New DataTable
            dtGst = GridViewGst.DataSource
            dtGst.AcceptChanges()
            Dim dtGstId As DataTable = dtGst.DefaultView.ToTable(True, "GSTCATID", "CRACCCODE")
            For cnt As Integer = 0 To dtGstId.Rows.Count - 1
                With dtGstId.Rows(cnt)
                    Dim FilterBy As String = ""
                    Dim gstCrAccode As String = ""
                    gstCatId = Val(.Item("GSTCATID").ToString)
                    gstCrAccode = .Item("CRACCCODE").ToString
                    FilterBy = "GSTCATID=" & gstCatId & " AND CRACCCODE='" & gstCrAccode & "'"
                    If gstCatId <> 0 Then
                        GstDR = GetSqlRow("SELECT * FROM " & cnAdminDb & "..GSTCATEGORY WHERE GSTCATID='" & gstCatId & "'", cn, tran)
                        With GstDR

                            GSTCGSTAC = .Item("CGSTAC").ToString()
                            GSTSGSTAC = .Item("SGSTAC").ToString()
                            GSTIGSTAC = .Item("IGSTAC").ToString()

                            RGSTCGSTAC = .Item("RCGSTAC").ToString()
                            RGSTSGSTAC = .Item("RSGSTAC").ToString()
                            RGSTIGSTAC = .Item("RIGSTAC").ToString()
                            GSTRCMFLG = .Item("RD").ToString()
                        End With
                        If GSTRCMFLG = "N" And (RGSTCGSTAC <> "" Or RGSTSGSTAC <> "" Or RGSTIGSTAC <> "") Then
                            Trfamount = Val(dtGrid.Compute("SUM(AMOUNT)", FilterBy).ToString)
                            InsertIntoAccTran(Tranno, "D", .Item("CRACCCODE").ToString, Trfamount, _GRtrantype.ToString, accode, Batchno, GstAccRefno, GstAccRefdate, , , , , txtRemark1.Text, txtRemark2.Text, , , , , , TdsCatId, Val(objTds.txtTdsPer_PER.Text), TdsAmt)
                            'RCM 
                            Trfamount = Val(dtGrid.Compute("SUM(IGST)", FilterBy).ToString)
                            RCMIGST = Val(RCMIGST + Trfamount)
                            InsertIntoAccTran(Tranno, "D", GSTIGSTAC, Trfamount, _GRtrantype.ToString, accode, Batchno, GstAccRefno, GstAccRefdate, , , , , itemDesc.ToString, "GRE")
                            Trfamount = Val(dtGrid.Compute("SUM(IGST)", FilterBy).ToString)
                            InsertIntoAccTran(Tranno, "C", RGSTIGSTAC, Trfamount, _GRtrantype.ToString, accode, Batchno, GstAccRefno, GstAccRefdate, , , , , itemDesc.ToString, "GRE")

                            Trfamount = Val(dtGrid.Compute("SUM(CGST)", FilterBy).ToString)
                            InsertIntoAccTran(Tranno, "D", GSTCGSTAC, Trfamount, _GRtrantype.ToString, accode, Batchno, GstAccRefno, GstAccRefdate, , , , , itemDesc.ToString, "GRE")
                            RCMCGST = Val(RCMCGST + Trfamount)
                            Trfamount = Val(dtGrid.Compute("SUM(CGST)", FilterBy).ToString)
                            InsertIntoAccTran(Tranno, "C", RGSTCGSTAC, Trfamount, _GRtrantype.ToString, accode, Batchno, GstAccRefno, GstAccRefdate, , , , , itemDesc.ToString, "GRE")

                            Trfamount = Val(dtGrid.Compute("SUM(SGST)", FilterBy).ToString)
                            InsertIntoAccTran(Tranno, "D", GSTSGSTAC, Trfamount, _GRtrantype.ToString, accode, Batchno, GstAccRefno, GstAccRefdate, , , , , itemDesc.ToString, "GRE")
                            RCMSGST = Val(RCMSGST + Trfamount)
                            Trfamount = Val(dtGrid.Compute("SUM(SGST)", FilterBy).ToString)
                            InsertIntoAccTran(Tranno, "C", RGSTSGSTAC, Trfamount, _GRtrantype.ToString, accode, Batchno, GstAccRefno, GstAccRefdate, , , , , itemDesc.ToString, "GRE")
                        Else
                            'GST
                            If rbtWithAcc.Checked Then
                                Trfamount = Val(dtGrid.Compute("SUM(AMOUNT)", FilterBy).ToString)
                                InsertIntoAccTran(Tranno, "D", .Item("CRACCCODE").ToString, Trfamount, _GRtrantype.ToString, accode, Batchno, GstAccRefno, GstAccRefdate, , , , , txtRemark1.Text, txtRemark2.Text, , , , , , TdsCatId, Val(objTds.txtTdsPer_PER.Text), TdsAmt)

                                Trfamount = Val(dtGrid.Compute("SUM(IGST)", FilterBy).ToString)
                                InsertIntoAccTran(Tranno, "D", GSTIGSTAC, Trfamount, _GRtrantype.ToString, accode, Batchno, GstAccRefno, GstAccRefdate, , , , , txtRemark1.Text, txtRemark2.Text)
                                RCMIGST = Val(RCMIGST + Trfamount)

                                Trfamount = Val(dtGrid.Compute("SUM(CGST)", FilterBy).ToString)
                                InsertIntoAccTran(Tranno, "D", GSTCGSTAC, Trfamount, _GRtrantype.ToString, accode, Batchno, GstAccRefno, GstAccRefdate, , , , , txtRemark1.Text, txtRemark2.Text)
                                RCMCGST = Val(RCMCGST + Trfamount)

                                Trfamount = Val(dtGrid.Compute("SUM(SGST)", FilterBy).ToString)
                                InsertIntoAccTran(Tranno, "D", GSTSGSTAC, Trfamount, _GRtrantype.ToString, accode, Batchno, GstAccRefno, GstAccRefdate, , , , , txtRemark1.Text, txtRemark2.Text)
                                RCMSGST = Val(RCMSGST + Trfamount)

                            ElseIf rbtWOAcc.Checked = True Then
                                Dim remark As String = ""
                                remark = txtRemark1.Text & " , " & txtRemark2.Text & ""

                                Trfamount = Val(dtGrid.Compute("SUM(AMOUNT)", FilterBy).ToString)
                                InsertIntoAccTran(Tranno, "D", .Item("CRACCCODE").ToString, Trfamount, _GRtrantype.ToString, accode, Batchno, GstAccRefno, GstAccRefdate, , , , , txtRemark1.Text, txtRemark2.Text, , , , , , TdsCatId, Val(objTds.txtTdsPer_PER.Text), TdsAmt)

                                Trfamount = Val(dtGrid.Compute("SUM(IGST)", FilterBy).ToString)
                                InsertIntoAccTran(Tranno, "D", "IGST", Trfamount, _GRtrantype.ToString, accode, Batchno, GstAccRefno, GstAccRefdate, , , , , remark, "GRE")

                                Trfamount = Val(dtGrid.Compute("SUM(CGST)", FilterBy).ToString)
                                InsertIntoAccTran(Tranno, "D", "CGST", Trfamount, _GRtrantype.ToString, accode, Batchno, GstAccRefno, GstAccRefdate, , , , , remark, "GRE")

                                Trfamount = Val(dtGrid.Compute("SUM(SGST)", FilterBy).ToString)
                                InsertIntoAccTran(Tranno, "D", "SGST", Trfamount, _GRtrantype.ToString, accode, Batchno, GstAccRefno, GstAccRefdate, , , , , remark, "GRE")

                            End If
                        End If
                    Else
                        Trfamount = Val(dtGrid.Compute("SUM(AMOUNT)", FilterBy).ToString)
                        InsertIntoAccTran(Tranno, "D", .Item("CRACCCODE").ToString, Trfamount, _GRtrantype.ToString, accode, Batchno, GstAccRefno, GstAccRefdate, , , , , txtRemark1.Text, txtRemark2.Text, , , , , , TdsCatId, Val(objTds.txtTdsPer_PER.Text), TdsAmt)

                        Trfamount = Val(dtGrid.Compute("SUM(IGST)", FilterBy).ToString)
                        InsertIntoAccTran(Tranno, "D", "IGST", Trfamount, _GRtrantype.ToString, accode, Batchno, GstAccRefno, GstAccRefdate, , , , , itemDesc.ToString, "GRE")

                        Trfamount = Val(dtGrid.Compute("SUM(CGST)", FilterBy).ToString)
                        InsertIntoAccTran(Tranno, "D", "CGST", Trfamount, _GRtrantype.ToString, accode, Batchno, GstAccRefno, GstAccRefdate, , , , , itemDesc.ToString, "GRE")

                        Trfamount = Val(dtGrid.Compute("SUM(SGST)", FilterBy).ToString)
                        InsertIntoAccTran(Tranno, "D", "SGST", Trfamount, _GRtrantype.ToString, accode, Batchno, GstAccRefno, GstAccRefdate, , , , , itemDesc.ToString, "GRE")
                    End If
                End With
            Next
            If rbtWithAcc.Checked Then
                If TdsAmt > 0 Then
                    Trfamount = Val(dtGrid.Compute("SUM(AMOUNT)", "").ToString)
                    Dim tdsAccode As String = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..TDSCATEGORY WHERE TDSCATID = '" & TdsCatId & "'", , , tran)
                    InsertIntoAccTran(Tranno, "C", tdsAccode, TdsAmt, _GRtrantype.ToString, accode, Batchno, GstAccRefno, GstAccRefdate, , , , , "TDS " & Val(objTds.txtTdsPer_PER.Text) & " ON " & Trfamount & " ", "")
                    InsertIntoAccTran(Tranno, "D", accode, TdsAmt, _GRtrantype.ToString, tdsAccode, Batchno, GstAccRefno, GstAccRefdate, , , , , "TDS " & Val(objTds.txtTdsPer_PER.Text) & " ON " & Trfamount & " ", "", , , , , , TdsCatId, Val(objTds.txtTdsPer_PER.Text), TdsAmt)
                End If
                If TCsAmt > 0 Then
                    Dim tcsAccode As String = "TCS"
                    InsertIntoAccTran(Tranno, "D", tcsAccode, TCsAmt, _GRtrantype.ToString, accode, Batchno, GstAccRefno, GstAccRefdate, , , , , txtRemark1.Text, txtRemark2.Text)
                    InsertIntoAccTran(Tranno, "C", accode, TCsAmt, _GRtrantype.ToString, tcsAccode, Batchno, GstAccRefno, GstAccRefdate, , , , , txtRemark1.Text, txtRemark2.Text)
                End If
                If Val(txtAdjCredit_AMT.Text) <> 0 Then
                    If Val(txtAdjCredit_AMT.Text) > 0 Then
                        InsertIntoAccTran(Tranno, "C", "RNDOFF", Val(txtAdjCredit_AMT.Text), _GRtrantype.ToString, accode, Batchno, GstAccRefno, GstAccRefdate, , , , , itemDesc.ToString, "GRE")
                        InsertIntoAccTran(Tranno, "D", accode, Val(txtAdjCredit_AMT.Text), _GRtrantype.ToString, "RNDOFF", Batchno, GstAccRefno, GstAccRefdate, , , , , itemDesc.ToString, "GRE")
                    Else
                        InsertIntoAccTran(Tranno, "D", "RNDOFF", -1 * Val(txtAdjCredit_AMT.Text), _GRtrantype.ToString, accode, Batchno, GstAccRefno, GstAccRefdate, , , , , itemDesc.ToString, "GRE")
                        InsertIntoAccTran(Tranno, "C", accode, -1 * Val(txtAdjCredit_AMT.Text), _GRtrantype.ToString, "RNDOFF", Batchno, GstAccRefno, GstAccRefdate, , , , , itemDesc.ToString, "GRE")
                    End If
                End If
                Dim TPostAmt As Double = 0
                If Val(txtAdjCash_AMT.Text) > 0 Then
                    TPostAmt = TPostAmt + Val(txtAdjCash_AMT.Text)
                End If
                If Val(txtAdjCheque_AMT.Text) > 0 Then
                    TPostAmt = TPostAmt + Val(txtAdjCheque_AMT.Text)
                End If
                If Val(txtAdjDue_AMT.Text) > 0 Then
                    TPostAmt = TPostAmt + Val(txtAdjDue_AMT.Text)
                End If
                If GST_GEN_CACC_POST_REG = True Then
                    If rbtWithAcc.Checked = True Then
                        Dim remark As String = ""
                        remark = txtRemark1.Text & " , " & txtRemark2.Text & ""
                        If Val(txtAdjCash_AMT.Text) > 0 Then
                            InsertIntoAccTran(Tranno, "C", _CashCode, Val(txtAdjCash_AMT.Text), _GRtrantype.ToString, accode, Batchno, GstAccRefno, GstAccRefdate, , , , , remark, "GRE")
                            InsertIntoAccTran(Tranno, "D", accode, Val(txtAdjCash_AMT.Text), _GRtrantype.ToString, _CashCode, Batchno, GstAccRefno, GstAccRefdate, , , , , txtRemark1.Text, txtRemark2.Text)
                        End If
                        If Val(txtAdjCheque_AMT.Text) > 0 Then
                            Dim BANKCODE As String = ""
                            For Each ro As DataRow In objCheaque.dtGridCheque.Rows
                                BANKCODE = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & ro!DEFAULTBANK.ToString & "'", , , tran).ToString
                            Next
                            InsertIntoAccTran(Tranno, "C", BANKCODE, Val(txtAdjCheque_AMT.Text), _GRtrantype.ToString, accode, Batchno, GstAccRefno, GstAccRefdate, , , , , remark, "GRE")
                            InsertIntoAccTran(Tranno, "D", accode, Val(txtAdjCheque_AMT.Text), _GRtrantype.ToString, BANKCODE, Batchno, GstAccRefno, GstAccRefdate, , , , , txtRemark1.Text, txtRemark2.Text)
                        End If
                    End If
                End If
            ElseIf rbtWOAcc.Checked = True Then
                Dim remark As String = ""
                remark = txtRemark1.Text & " , " & txtRemark2.Text & ""
                If TCsAmt > 0 Then
                    Dim tcsAccode As String = "TCS"
                    InsertIntoAccTran(Tranno, "D", tcsAccode, TCsAmt, _GRtrantype.ToString, accode, Batchno, GstAccRefno, GstAccRefdate, , , , , txtRemark1.Text, txtRemark2.Text)
                End If
                If Val(txtAdjCash_AMT.Text) > 0 Then
                    InsertIntoAccTran(Tranno, "C", accode, Val(txtAdjCash_AMT.Text) - Val(RCMIGST + RCMCGST + RCMSGST), _GRtrantype.ToString, crAccode, Batchno, GstAccRefno, GstAccRefdate, , , , , remark, "GRE")
                End If
                If Val(txtAdjCheque_AMT.Text) > 0 Then
                    For Each ro As DataRow In objCheaque.dtGridCheque.Rows
                        Dim BANKCODE As String = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & ro!DEFAULTBANK.ToString & "'", , , tran).ToString
                        InsertIntoAccTran(Tranno, "C", accode, Val(ro!AMOUNT.ToString) - Val(RCMIGST + RCMCGST + RCMSGST), "CH", crAccode _
                         , Batchno, GstAccRefno, GstAccRefdate, ro!CHEQUENO.ToString, ro!DATE.ToString, , ro!BANKNAME.ToString, itemDesc.ToString, "H")
                        crAccode = BANKCODE
                        RCMIGST = 0 : RCMCGST = 0 : RCMSGST = 0
                    Next
                End If
                If Val(txtAdjCredit_AMT.Text) <> 0 Then
                    If Val(txtAdjCredit_AMT.Text) > 0 Then
                        InsertIntoAccTran(Tranno, "C", "RNDOFF", Val(txtAdjCredit_AMT.Text), _GRtrantype.ToString, accode, Batchno, GstAccRefno, GstAccRefdate, , , , , remark, "GRE")
                    Else
                        InsertIntoAccTran(Tranno, "D", "RNDOFF", -1 * Val(txtAdjCredit_AMT.Text), _GRtrantype.ToString, accode, Batchno, GstAccRefno, GstAccRefdate, , , , , remark, "GRE")
                    End If
                End If
            End If
            ''USER DATE CONTROL EDIT 
            If ((EditBckId) > 0 And editBatchno <> "") Then
                strSql = " INSERT INTO " & cnStockDb & "..EDITHISTORY (BATCHNO,BCKID,USERID,SYSTEMID,COSTID,COMPANYID)"
                strSql += vbCrLf + " VALUES('" & editBatchno & "'," & EditBckId & "," & userId & ",'" & systemId & "','" & cnCostId & "','" & strCompanyId & "')"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
            End If
            Dim debCrNotTally As String = ""
            Dim balAmt As Double = Val(objGPack.GetSqlValue("SELECT SUM(CASE WHEN TRANMODE = 'C' THEN isnull(AMOUNT,0) ELSE isnull(AMOUNT,0)*(-1) END) AS BALANCE FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = '" & Batchno & "'", , "0", tran))
            If Math.Abs(balAmt) > 0 Then
                tran.Rollback()
                tran.Dispose()
                tran = Nothing
                existGstCatId = 0
                MsgBox("Credit Debit Not Tally : " + balAmt.ToString, MsgBoxStyle.Information)
                Exit Sub
            End If
            Dim accodeChecking As Integer = 0
            accodeChecking = (objGPack.GetSqlValue("SELECT COUNT(*)CNT FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = '" & Batchno & "' AND ISNULL(ACCODE,'')=''", Nothing, "", tran))
            If (accodeChecking) > 0 Then
                tran.Rollback()
                tran.Dispose()
                tran = Nothing
                existGstCatId = 0
                MsgBox("Accode should not Empty ", MsgBoxStyle.Information)
                Exit Sub
            End If
            tran.Commit()
            tran = Nothing
            MsgBox("Tranno Generated  : " + Tranno.ToString + " " + vbCrLf + " Successfully...", MsgBoxStyle.Information)
            Dim pBatchno As String = Batchno
            Dim pBillDate As Date = dtpTranDate.Value
            Dim pParamStr As String = ""
            Dim prnmemsuffix As String = ""
            If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId
            Dim BillPrintExe As Boolean = IIf(GetAdmindbSoftValue("CALLBILLPRINTEXE", "Y") = "Y", True, False)
            Dim BillPrint_Format As String = GetAdmindbSoftValue("BILLPRINT_FORMAT", "")
            If GST And BillPrint_Format = "M1" Then
                Dim obj As New BrighttechREPORT.frmBillPrintDocA4N("GST", pBatchno, pBillDate.ToString("yyyy-MM-dd"), "")
            ElseIf GST And BillPrint_Format = "M2" Then
                Dim obj As New BrighttechREPORT.frmBillPrintDocB5("GST", pBatchno, pBillDate.ToString("yyyy-MM-dd"), "")
            ElseIf GST And BillPrint_Format = "M3" Then
                Dim obj As New BrighttechREPORT.frmBillPrintDocA5("GST", pBatchno, pBillDate.ToString("yyyy-MM-dd"), "")
            ElseIf GST And BillPrint_Format = "M4" Then
                Dim obj As New BrighttechREPORT.frmBillPrintDocB52cpy("GST", pBatchno, pBillDate.ToString("yyyy-MM-dd"), "")
            ElseIf GST And BillPrintExe = False Then
                Dim billDoc As New frmBillPrintDoc("POS", pBatchno, pBillDate.ToString("yyyy-MM-dd"), "")
            Else
                If IO.File.Exists(Application.StartupPath & "\BillPrint.exe") Then
                    Dim write As IO.StreamWriter
                    ''Dim prnmemsuffix As String = ""
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
                        System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe")
                    Else
                        System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe", pParamStr)
                    End If
                End If
            End If
            existGstCatId = 0
            editBatchno = Nothing
            EditCostId = Nothing
            funcClear()
        Catch ex As Exception
            If Not tran Is Nothing Then
                tran.Rollback()
                tran = Nothing
            End If
            MsgBox(ex.Message & vbCrLf & ex.StackTrace, MsgBoxStyle.Information)
        End Try
    End Sub

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
        Optional ByVal TdsId As Integer = 0,
        Optional ByVal TdsPer As Integer = 0,
        Optional ByVal TdsAmt As Integer = 0
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
        strSql += " ,TDSCATID,TDSPER,TDSAMOUNT"
        strSql += " )"
        strSql += " VALUES"
        strSql += " ("
        strSql += " '" & GetNewSno(TranSnoType.ACCTRANCODE, tran) & "'" ''SNO
        strSql += " ," & tNo & "" 'TRANNO 
        strSql += " ,'" & dtpTranDate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
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
        strSql += " ,'" & Val(TdsId) & "'" 'TDSID
        strSql += " ,'" & Val(TdsPer) & "'" 'TDSPER
        strSql += " ,'" & Val(TdsAmt) & "'" 'TDSAMT
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
    Public Sub CostName()
        strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE  ORDER BY COSTNAME"
        objGPack.FillCombo(strSql, cmbCostCentre_MAN, , False)
        strSql = "SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE  WHERE COSTID='" & cnCostId & "'"
        cmbCostCentre_MAN.Text = objGPack.GetSqlValue(strSql, "COSTNAME", "")
    End Sub
    Public Sub AcName()
        If editBatchno <> "" Then
            Exit Sub
        End If
        da = New OleDbDataAdapter
        dt = New DataTable
        If rbtWithAcc.Checked = True Then
            strSql = " SELECT ACNAME,ACCODE FROM " & cnAdminDb & "..ACHEAD "
            'strSql += vbCrLf + " WHERE ACTYPE = 'G'"
            strSql += vbCrLf + " ORDER BY ACNAME"
        ElseIf rbtWOAcc.Checked = True Then
            strSql = " SELECT ACNAME,ACCODE FROM " & cnAdminDb & "..ACHEAD "
            'strSql += vbCrLf + " WHERE ACTYPE ='E' "
            strSql += vbCrLf + " ORDER BY ACNAME"
        End If
        da = New OleDbDataAdapter(strSql, cn)
        dt = New DataTable
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            cmbDrAcName_OWN.DataSource = Nothing
            cmbDrAcName_OWN.DataSource = dt
            cmbDrAcName_OWN.ValueMember = "ACCODE"
            cmbDrAcName_OWN.DisplayMember = "ACNAME"
        End If
        'objGPack.FillCombo(strSql, cmbDrAcName_OWN, , False)
    End Sub
    Private Sub txtGst_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'calculation()
    End Sub

    Public Sub calculation(ByVal SGST As Double, ByVal CGST As Double, ByVal IGST As Double)
        Dim stateid As String = objGPack.GetSqlValue("SELECT STATEID FROM " & cnAdminDb & "..STATEMAST WHERE STATENAME= '" & cmbStateName_MAN.Text & "' ", , , tran)
        Dim cmpystateid As String = objGPack.GetSqlValue("SELECT STATEID FROM " & cnAdminDb & "..COMPANY WHERE STATEID= '" & stateid & "' ", , , tran)
        Dim amt_per, tax, per, res As Decimal

        If cmbStateName_MAN.Text = "" Then
            MessageBox.Show("Select State Name")
            txtGstPer_AMT.Text = ""
        Else

            txtGsttot_Amt.Text = Format(Math.Round(Val(txtSgst_AMT.Text.ToString) + Val(txtCgst_AMT.Text.ToString) + Val(txtIgst_AMT.Text.ToString) - Val(txtTDS_AMT.Text.ToString)), "0.00")

            txtSgst_per.Text = Format(SGST, "0.00")
            txtSgst_AMT.Text = Format(Math.Round(Val(txtamt_AMT.Text) * SGST / 100, 2), "0.00")

            txtCgst_per.Text = Format(CGST, "0.00")
            txtCgst_AMT.Text = Format(Math.Round(Val(txtamt_AMT.Text) * CGST / 100, 2), "0.00")

            txtIgst_per.Text = Format(IGST, "0.00")
            txtIgst_AMT.Text = Format(Math.Round(Val(txtamt_AMT.Text) * IGST / 100, 2), "0.00")

            txtGstPer_AMT.Text = Format(Math.Round(SGST + CGST + IGST), "0.00")

            TxtAmountTotal.Text = Format(Math.Round(Val(txtamt_AMT.Text.ToString) + Val(txtSgst_AMT.Text.ToString) + Val(txtCgst_AMT.Text.ToString) + Val(txtIgst_AMT.Text.ToString) + Val(txtTCS_AMT.Text.ToString)), "0.00")
        End If

    End Sub

    Public Sub calculation()
        Dim SGST As Double = Val(txtSgst_per.Text.ToString)
        Dim CGST As Double = Val(txtCgst_per.Text.ToString)
        Dim IGST As Double = Val(txtIgst_per.Text.ToString)
        Dim amt_per, tax, per, res As Decimal
        If cmbStateName_MAN.Text = "" Then
            MessageBox.Show("Select State Name")
            txtGstPer_AMT.Text = ""
        Else
            txtSgst_AMT.Text = Format(Math.Round(Val(txtSgst_AMT.Text.ToString), 2), "0.00")
            txtSgst_per.Text = Format(SGST, "0.00")

            txtCgst_AMT.Text = Format(Math.Round(Val(txtCgst_AMT.Text.ToString), 2), "0.00")
            txtCgst_per.Text = Format(CGST, "0.00")

            txtIgst_AMT.Text = Format(Math.Round(Val(txtIgst_AMT.Text.ToString), 2), "0.00")
            txtIgst_per.Text = Format(IGST, "0.00")

            txtGsttot_Amt.Text = Format(Math.Round(Val(txtSgst_AMT.Text.ToString) + Val(txtCgst_AMT.Text.ToString) + Val(txtIgst_AMT.Text.ToString) - Val(txtTDS_AMT.Text.ToString)), "0.00")
            txtGstPer_AMT.Text = Format(Math.Round(SGST + CGST + IGST), "0.00")
            TxtAmountTotal.Text = Format(Math.Round(Val(txtamt_AMT.Text.ToString) + Val(txtSgst_AMT.Text.ToString) + Val(txtCgst_AMT.Text.ToString) + Val(txtIgst_AMT.Text.ToString) + Val(txtTCS_AMT.Text.ToString)), "0.00")
        End If
    End Sub

    Private Sub txtRate_AMT_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtRate_AMT.TextChanged
        If Val(txtPcs_NUM.Text) <> 0 Then
            txtamt_AMT.Text = Format(Val(Val(txtPcs_NUM.Text.ToString) * Val(txtRate_AMT.Text.ToString)).ToString(), "0.00")
        End If
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
        txtSgst_per.Text = Val(Val(txtGstPer_AMT.Text) - Val(txtCgst_per.Text)).ToString()
        txtCgst_AMT.Text = Val(Val(Val(txtamt_AMT.Text) / 100) * Val(txtCgst_per.Text)).ToString()
    End Sub
    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewToolStripMenuItem.Click
        tabMain.SelectedTab = TabPage2
        GridLoad()
        GridDesign()
        Call GridCanceDesign()
        dtpSrcTrandate.Focus()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click, btnExit.Click
        Me.Close()
    End Sub


    Private Sub cmbStateName_MAN_Leave(sender As Object, e As System.EventArgs) Handles cmbStateName_MAN.Leave
        strSql = " SELECT STATEID FROM " & cnAdminDb & "..STATEMAST WHERE STATENAME='" & cmbStateName_MAN.Text & "'"
        stateId = GetSqlValue(cn, strSql)
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

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click, btnNew.Click
        'objAddressDia = New frmAddressDia()
        funcClear()
    End Sub


    Private Sub txtamt_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtamt_AMT.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If Val(txtRate_AMT.Text) > 0 Then
                txtamt_AMT.Text = Format(Math.Round(IIf(Val(txtPcs_NUM.Text) = 0, 1, Val(txtPcs_NUM.Text)) * Val(txtRate_AMT.Text), 2), "0.00")
            Else
                txtamt_AMT.Text = Format(Math.Round(Val(txtamt_AMT.Text), 2), "0.00")
            End If
            If Val(txtamt_AMT.Text) = 0 Then MsgBox("Amount should not empty..", MsgBoxStyle.Information) : Exit Sub
            Dim TdsFlag As Boolean = False
            If objGPack.GetSqlValue("SELECT ISNULL(TDSFLAG,'') FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbDrAcName_OWN.Text & "'").ToUpper = "Y" Then
                TdsFlag = True
            End If
            If objGPack.GetSqlValue("SELECT ISNULL(TDSFLAG,'') FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbCrAcName_OWN.Text & "'").ToUpper = "Y" Then
                TdsFlag = True
            End If
            If rbtWithAcc.Checked Then
                If TdsFlag Then
                    objTds = New frmAccTdsGST
                    objTds.StrGstCrAcname = cmbCrAcName_OWN.Text
                    objTds.StrGstDrAcname = cmbDrAcName_OWN.Text
                    objTds.GetTdsDefregistervalue()
                    objTds.cmbTdsCategory_OWN.Focus()
                    If objTds.ShowDialog = Windows.Forms.DialogResult.OK Then
                        ''txtTDS_AMT.Text = Format(Math.Round(Val(txtamt_AMT.Text) * Val(objTds.txtTdsPer_PER.Text) / 100, 2), "0.00")
                        txtTDS_AMT.Text = Format(CalcRoundoffAmt(Math.Round(Val(txtamt_AMT.Text) * Val(objTds.txtTdsPer_PER.Text) / 100, 2), GetAdmindbSoftValue("ROUNDOFF-ACC-TDS", "N")), "0.00")
                        TdsCatId = GetSqlValue(cn, "SELECT TDSCATID FROM " & cnAdminDb & "..TDSCATEGORY WHERE TDSCATNAME='" & objTds.cmbTdsCategory_OWN.Text & "'")
                    End If
                End If
            End If
            Dim CompStateId As Integer = Val(objGPack.GetSqlValue("SELECT STATEID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME= '" & cmbCostCentre_MAN.Text & "' ", , , tran).ToString)
            If CompStateId = 0 Then CompStateId = CompanyStateId
            Dim taxType As String = ""
            strSql = " SELECT TAXTYPE FROM " & cnAdminDb & "..GSTCLAIM WHERE GSTCLAIMID='" & cmbGstClaim.SelectedValue.ToString & "'"
            taxType = GetSqlValue(cn, strSql).ToString
            objTax = New frmGSTTax(taxType, stateId, CompStateId)
            objTax.txtSgst_per_AMT.Focus()
            If objTax.ShowDialog = Windows.Forms.DialogResult.OK Then
                calculation(Val(objTax.txtSgst_per_AMT.Text.ToString), Val(objTax.txtCgst_per_AMT.Text.ToString), Val(objTax.txtIgst_per_AMT.Text.ToString))
                gstCatId = GetSqlValue(cn, "SELECT GSTCATID FROM " & cnAdminDb & "..GSTCATEGORY WHERE GSTCATNAME='" & objTax.CmbGstCategory.Text & "'")
            End If
            If TdsFlag Then
                txtTDS_AMT.Focus()
            Else
                txtSgst_AMT.Focus()
            End If
            If GST_GEN_TAXEDIT Then
                If stateId = CompStateId Then
                    txtIgst_AMT.ReadOnly = True
                    txtSgst_AMT.ReadOnly = False
                    txtCgst_AMT.ReadOnly = False
                Else
                    txtSgst_AMT.ReadOnly = True
                    txtCgst_AMT.ReadOnly = True
                    txtIgst_AMT.ReadOnly = False
                End If
            End If
        End If
    End Sub
    Private Sub cmbGstClaim_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbGstClaim.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If cmbGstClaim.Text = "GST" Then
                'taxType = "Y"
            Else
                'taxType = "N"
            End If
        End If
    End Sub

    Private Function funcvalidation() As Boolean
        If txtDescrip_MAN.Text.ToString.Trim = "" Then MsgBox("Description should not be empty...", MsgBoxStyle.Information) : txtDescrip_MAN.Focus() : Return False : Exit Function
        If cmbGstClaim.Text = "GST" Then If txthsn.Text.ToString.Trim = "" Then MsgBox("HSN Code should not be empty...", MsgBoxStyle.Information) : txthsn.Focus() : Return False : Exit Function
        If txtamt_AMT.Text.ToString.Trim = "" Then MsgBox("Amount should not be empty...", MsgBoxStyle.Information) : txtamt_AMT.Focus() : Return False : Exit Function
        If txtGstPer_AMT.Text.ToString.Trim = "" Then MsgBox("GST % should not be empty...", MsgBoxStyle.Information) : txtGstPer_AMT.Focus() : Return False : Exit Function
        If cmbGstClaim.Text = "GST" Then If txtRefno.Text.ToString.Trim = "" Then MsgBox("Refno should not be empty...", MsgBoxStyle.Information) : txtRefno.Focus() : Return False : Exit Function
        If cmbCrAcName_OWN.SelectedValue Is Nothing Then MsgBox("Credit Account should not empty", MsgBoxStyle.Information) : cmbCrAcName_OWN.Focus() : cmbCrAcName_OWN.SelectAll() : Return False : Exit Function
        If cmbCrAcName_OWN.Text = "" Then MsgBox("Credit Account should not empty", MsgBoxStyle.Information) : cmbCrAcName_OWN.Focus() : cmbCrAcName_OWN.SelectAll() : Return False : Exit Function
        Return True
    End Function

    Public Sub GridDesign()
        With gvGstRegister
            With .Columns("SNO")
                .HeaderText = "SNO"
                .Width = 100
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("CUSTOMER_BILLING_PINCODE")
                .HeaderText = "CUSTOMER_BILLING_PINCODE"
                .Width = 100
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("CUSTOMER_BILLING_CITY")
                .HeaderText = "CUSTOMER_BILLING_CITY"
                .Width = 100
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("BATCHNO")
                .HeaderText = "BATCHNO"
                .Width = 100
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("COSTID")
                .HeaderText = "COSTID"
                .Width = 100
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("TRANNO")
                .HeaderText = "INV NO"
                .Width = 70
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("TRANDATE")
                .HeaderText = "INV DATE"
                .Width = 100
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("DESCRIPTION")
                .HeaderText = "DESCRIPTION"
                .Width = 250
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("HSN")
                .HeaderText = "HSN_SAC_CODE"
                .Width = 100
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With

            With .Columns("PCS")
                .HeaderText = "PCS"
                .Width = 50
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("WEIGHT")
                .HeaderText = "WEIGHT"
                .Width = 50
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("RATE")
                .HeaderText = "RATE"
                .Width = 60
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("AMOUNT")
                .HeaderText = "ITEM_RATE"
                .Width = 60
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("TAX")
                .HeaderText = "TAX"
                .Width = 60
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("TAXAMOUNT")
                .HeaderText = "TAXAMOUNT"
                .Width = 60
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("TOTALAMOUNT")
                .HeaderText = "TOTALAMOUNT"
                .Width = 60
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("SGST_PER")
                .HeaderText = "SGST_RATE"
                .Width = 60
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("SGST")
                .HeaderText = "SGST_AMOUNT"
                .Width = 60
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("CGST_PER")
                .HeaderText = "CGST_RATE"
                .Width = 60
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("CGST")
                .HeaderText = "CGST_AMOUNT"
                .Width = 60
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("IGST_PER")
                .HeaderText = "IGST_RATE"
                .Width = 60
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("IGST")
                .HeaderText = "IGST_AMOUNT"
                .Width = 60
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("STATEID")
                .HeaderText = "STATECODE_PLACE_OF_SUPPLY"
                .Width = 60
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("ACCODE")
                .HeaderText = "SUPPLIER_NAME"
                .Width = 100
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("REFNO")
                .HeaderText = "REFNO"
                .Width = 50
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("REFDATE")
                .HeaderText = "REFDATE"
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("GSTCLAIM")
                .HeaderText = "GSTCLAIM"
                .Width = 35
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("TRANTYPE")
                .HeaderText = "TRANTYPE"
                .Width = 50
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
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("ORIGINAL_INVOICE_NUMBER")
                .HeaderText = "ORIGINAL_INVOICE_NUMBER"
                .Width = 60
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
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("ORDER_NUMBER")
                .HeaderText = "ORDER_NUMBER"
                .Width = 60
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("ORDER_DATE")
                .HeaderText = "ORDER_DATE"
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("SUPPLY_TYPE")
                .HeaderText = "SUPPLY_TYPE"
                .Width = 50
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("FLAG_EXPORT_INVOICE")
                .HeaderText = "FLAG_EXPORT_INVOICE"
                .Width = 50
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("FLAG_EXPORT_GST_PAYMENT")
                .HeaderText = "FLAG_EXPORT_GST_PAYMENT"
                .Width = 50
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("EXPORT_SHIPPING_BILL_NUMBER")
                .HeaderText = "EXPORT_SHIPPING_BILL_NUMBER"
                .Width = 50
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("EXPORT_SHIIPING_BILL_DATE")
                .HeaderText = "EXPORT_SHIIPING_BILL_DATE"
                .Width = 50
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("EXPORT_DESTINATION_COUNTRY_CODE")
                .HeaderText = "EXPORT_DESTINATION_COUNTRY_CODE"
                .Width = 50
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
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
            With .Columns("CANCEL")
                .HeaderText = "CANCEL"
                .Width = 100
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
            With .Columns("TYPE")
                .HeaderText = "TYPE"
                .Width = 100
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

    Private Sub GridViewGst_UserDeletedRow(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles GridViewGst.UserDeletedRow
        dtGrid.AcceptChanges()
        GridTotal()
    End Sub



    Private Sub GridViewGst_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles GridViewGst.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If GridViewGst.Rows.Count > 0 Then
                'Dim rwIndex As Integer = GridViewGst.CurrentRow.Index - 1 commented in 17-12-2020
                Dim rwIndex As Integer = GridViewGst.CurrentRow.Index
                If rwIndex = -1 Then
                    rwIndex = 0
                End If
                With GridViewGst
                    .Columns("KEYNO").Visible = True
                End With
                GridViewGst.CurrentCell = GridViewGst.Rows(GridViewGst.CurrentRow.Index).Cells("KEYNO")
                With GridViewGst.Rows(rwIndex)
                    txtDescrip_MAN.Text = .Cells("DESCRIPTION").Value.ToString
                    cmbCrAcName_OWN.Text = .Cells("CRACNAME").Value.ToString
                    txthsn.Text = .Cells("HSN").Value.ToString
                    txtPcs_NUM.Text = .Cells("PCS").Value.ToString
                    txtWeight_WET.Text = .Cells("WEIGHT").Value.ToString
                    txtRate_AMT.Text = .Cells("RATE").Value.ToString
                    txtamt_AMT.Text = .Cells("AMOUNT").Value.ToString
                    txtGstPer_AMT.Text = .Cells("TAX").Value.ToString
                    txtGsttot_Amt.Text = .Cells("TAXAMOUNT").Value.ToString
                    txtSgst_per.Text = .Cells("SGST_PER").Value.ToString
                    txtSgst_AMT.Text = .Cells("SGST").Value.ToString
                    txtCgst_per.Text = .Cells("CGST_PER").Value.ToString
                    txtCgst_AMT.Text = .Cells("CGST").Value.ToString
                    txtIgst_per.Text = .Cells("IGST_PER").Value.ToString
                    txtIgst_AMT.Text = .Cells("IGST").Value.ToString
                    txtRefno.Text = .Cells("REFNO").Value.ToString
                    dtpRefDate.Text = .Cells("REFDATE").Value.ToString
                    cmbGstClaim.Text = .Cells("GSTCLAIM").Value.ToString
                    txtTDS_AMT.Text = .Cells("TDS").Value.ToString
                    lblEditKeyNo.Text = .Index.ToString
                    txtDescrip_MAN.Focus()
                End With
                With GridViewGst
                    .Columns("KEYNO").Visible = False
                End With
            End If
        End If
    End Sub

    Private Sub txtPcs_NUM_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPcs_NUM.TextChanged
        'If Val(txtPcs_NUM.Text) <> 0 Then
        '    txtamt_AMT.Text = Format(Val(Val(txtPcs_NUM.Text.ToString) * Val(txtRate_AMT.Text.ToString)).ToString(), "0.00")
        'Else
        '    txtamt_AMT.Text = Format(Val(txtRate_AMT.Text.ToString), "0.00")
        'End If
    End Sub

    Private Sub cmbAcName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbDrAcName_OWN.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If cmbDrAcName_OWN.Text = "" Then
                MsgBox("Account Head should not empty..", MsgBoxStyle.Information)
                cmbDrAcName_OWN.Focus()
                Exit Sub
            End If
        End If
    End Sub
    Private Sub txtAdjCheque_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChequeToolStripMenuItem.Click
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
            If btnSave.Enabled = False Then btnSave.Enabled = True
            'objAddressDia.BackColor = Me.BackColor
            'objAddressDia.StartPosition = FormStartPosition.CenterScreen
            'objAddressDia.txtMobile.Select()
            If editBatchno <> "" Then
                'objAddressDia.editBatchno = editBatchno
                'objAddressDia.editBatchno = EditCostId
                ShowAddressDia()
            Else
                ShowAddressDia()
            End If
        End If
    End Sub
    Private Sub funcCalcBal()
        If GridViewGstTot Is Nothing Then Exit Sub
        If GridViewGstTot.Rows.Count = 0 Then Exit Sub
        Dim Amt As Double = Val(GridViewGstTot.Rows(0).Cells("AMOUNT").Value.ToString) _
                    + Val(GridViewGstTot.Rows(0).Cells("IGST").Value.ToString) _
                    + Val(GridViewGstTot.Rows(0).Cells("CGST").Value.ToString) _
                    + Val(GridViewGstTot.Rows(0).Cells("SGST").Value.ToString) + Val(GridViewGstTot.Rows(0).Cells("TCS").Value.ToString)
        If txtAdjCash_AMT.Focused Then
            Amt -= Val(txtAdjCash_AMT.Text)
            Amt -= Val(txtAdjCheque_AMT.Text)
            Amt -= Val(txtAdjDue_AMT.Text)
            txtAdjCredit_AMT.Text = IIf(Amt <> 0, Format(Amt, "0.00"), DBNull.Value.ToString)
        ElseIf txtAdjCredit_AMT.Focused Then
            txtAdjCash_AMT.Text = Amt - Val(txtAdjCredit_AMT.Text)
        ElseIf txtAdjDue_AMT.Focused Then
            If Val(txtAdjDue_AMT.Text) = 0 Then
                txtAdjCash_AMT.Text = Amt - Val(txtAdjCredit_AMT.Text)
            End If
        End If
    End Sub

    Private Sub txtAdjCash_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAdjCash_AMT.TextChanged
        funcCalcBal()
    End Sub
    Private Sub txtAdjCheque_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAdjCheque_AMT.TextChanged
        funcCalcBal()
    End Sub
    Private Sub txtAdjDue_AMT_TextChanged(sender As Object, e As EventArgs) Handles txtAdjDue_AMT.TextChanged
        funcCalcBal()
    End Sub
    Private Sub txtAmt_Leave(sender As Object, e As EventArgs) Handles txtAdjDue_AMT.Leave, txtAdjCheque_AMT.Leave, txtAdjCash_AMT.Leave
        funcCalcBal()
    End Sub
    Private Sub txtSupName_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtSupName.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtSupName.Text = "" Then
                MsgBox("Supplier name should not empty..", MsgBoxStyle.Information)
                txtSupName.Focus()
                Exit Sub
            End If
        End If
    End Sub
    Private Sub txtRefno_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtRefno.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If funcvalidation() = False Then Exit Sub
            If lblEditKeyNo.Text.ToString = "" Then
                Dim dr As DataRow
                dr = dtGrid.NewRow
                dr.Item("CRACNAME") = cmbCrAcName_OWN.Text
                dr.Item("CRACCCODE") = cmbCrAcName_OWN.SelectedValue.ToString
                dr.Item("DESCRIPTION") = txtDescrip_MAN.Text.ToString
                dr.Item("HSN") = txthsn.Text.ToString
                dr.Item("PCS") = Val(txtPcs_NUM.Text.ToString)
                dr.Item("WEIGHT") = Format(Val(txtWeight_WET.Text.ToString), "0.00")
                dr.Item("RATE") = Format(Val(txtRate_AMT.Text.ToString), "0.00")
                dr.Item("AMOUNT") = Format(Val(txtamt_AMT.Text.ToString), "0.00")
                dr.Item("TDS") = Format(Val(txtTDS_AMT.Text.ToString), "0.00")
                dr.Item("TDS_PER") = Format(Val(objTds.txtTdsPer_PER.Text.ToString), "0.00")
                dr.Item("TCS") = Format(Val(txtTCS_AMT.Text.ToString), "0.00")
                dr.Item("TAX") = Format(Val(txtGstPer_AMT.Text.ToString), "0.00")
                dr.Item("TAXAMOUNT") = Format(Val(txtGsttot_Amt.Text.ToString), "0.00")
                dr.Item("SGST_PER") = Format(Val(txtSgst_per.Text.ToString), "0.00")
                dr.Item("SGST") = Format(Val(txtSgst_AMT.Text.ToString), "0.00")
                dr.Item("CGST_PER") = Format(Val(txtCgst_per.Text.ToString), "0.00")
                dr.Item("CGST") = Format(Val(txtCgst_AMT.Text.ToString), "0.00")
                dr.Item("IGST_PER") = Format(Val(txtIgst_per.Text.ToString), "0.00")
                dr.Item("IGST") = Format(Val(txtIgst_AMT.Text.ToString), "0.00")
                dr.Item("STATEID") = Val(stateId)
                dr.Item("ACCODE") = Val(txtDescrip_MAN.Text.ToString)
                dr.Item("REFNO") = txtRefno.Text.ToString
                dr.Item("REFDATE") = dtpRefDate.Value.Date.ToString("dd-MM-yyyy")
                dr.Item("GSTCLAIM") = cmbGstClaim.Text
                dr.Item("GSTCLAIMID") = cmbGstClaim.SelectedValue.ToString
                dr.Item("GSTNO") = txtGSTIN.Text.ToString
                dr.Item("ADDRESS1") = Mid(txtAddress1.Text.ToString, 1, 30)
                dr.Item("ADDRESS2") = Mid(txtAddress2.Text.ToString, 1, 30)
                dr.Item("GSTCATID") = gstCatId
                dtGrid.Rows.Add(dr)
                GridViewGst.DataSource = Nothing
                GridViewGst.DataSource = dtGrid
                StyleGridSASR(GridViewGst)
            Else
                With dtGrid.Rows(Val(lblEditKeyNo.Text.ToString))
                    .Item("DESCRIPTION") = txtDescrip_MAN.Text.ToString
                    .Item("CRACNAME") = cmbCrAcName_OWN.Text
                    .Item("CRACCCODE") = cmbCrAcName_OWN.SelectedValue.ToString
                    .Item("HSN") = txthsn.Text.ToString
                    .Item("PCS") = Val(txtPcs_NUM.Text.ToString)
                    .Item("WEIGHT") = Format(Val(txtWeight_WET.Text.ToString), "0.00")
                    .Item("RATE") = Format(Val(txtRate_AMT.Text.ToString), "0.00")
                    .Item("AMOUNT") = Format(Val(txtamt_AMT.Text.ToString), "0.00")
                    .Item("TAX") = Format(Val(txtGstPer_AMT.Text.ToString), "0.00")
                    .Item("TAXAMOUNT") = Format(Val(txtGsttot_Amt.Text.ToString), "0.00")
                    .Item("TDS_PER") = Format(Val(objTds.txtTdsPer_PER.Text.ToString), "0.00")
                    .Item("TDS") = Format(Val(txtTDS_AMT.Text.ToString), "0.00")
                    .Item("TCS") = Format(Val(txtTCS_AMT.Text.ToString), "0.00")
                    .Item("SGST_PER") = Format(Val(txtSgst_per.Text.ToString), "0.00")
                    .Item("SGST") = Format(Val(txtSgst_AMT.Text.ToString), "0.00")
                    .Item("CGST_PER") = Format(Val(txtCgst_per.Text.ToString), "0.00")
                    .Item("CGST") = Format(Val(txtCgst_AMT.Text.ToString), "0.00")
                    .Item("IGST_PER") = Format(Val(txtIgst_per.Text.ToString), "0.00")
                    .Item("IGST") = Format(Val(txtIgst_AMT.Text.ToString), "0.00")
                    .Item("STATEID") = Val(stateId)
                    .Item("ACCODE") = Val(txtDescrip_MAN.Text.ToString)
                    .Item("REFNO") = txtRefno.Text.ToString
                    .Item("REFDATE") = dtpRefDate.Value.Date.ToString("dd-MM-yyyy")
                    .Item("GSTCLAIM") = cmbGstClaim.Text
                    .Item("GSTCLAIMID") = cmbGstClaim.SelectedValue.ToString
                    .Item("GSTNO") = txtGSTIN.Text.ToString
                    .Item("ADDRESS1") = Mid(txtAddress1.Text.ToString, 1, 30)
                    .Item("ADDRESS2") = Mid(txtAddress2.Text.ToString, 1, 30)
                    .Item("GSTCATID") = gstCatId
                    dtGrid.AcceptChanges()
                    GridViewGst.DataSource = Nothing
                    GridViewGst.DataSource = dtGrid
                    StyleGridSASR(GridViewGst)
                End With
            End If
            'Clear TextBox
            txtDescrip_MAN.Text = ""
            txthsn.Text = ""
            txtPcs_NUM.Text = ""
            txtWeight_WET.Text = ""
            txtRate_AMT.Text = ""
            txtamt_AMT.Text = ""
            txtGstPer_AMT.Text = ""
            txtGsttot_Amt.Text = ""
            TxtAmountTotal.Text = ""
            txtSgst_per.Text = ""
            txtSgst_AMT.Text = ""
            txtCgst_per.Text = ""
            txtCgst_AMT.Text = ""
            txtTDS_AMT.Text = ""
            txtTCS_AMT.Text = ""
            txtIgst_per.Text = ""
            txtIgst_AMT.Text = ""
            txtRefno.Text = ""
            TxtAmount.Text = ""
            lblEditKeyNo.Text = ""
            GridTotal()
            txtDescrip_MAN.Focus()
        End If
    End Sub
    Private Sub CashToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles CashToolStripMenuItem.Click
        If Val(txtAdjCash_AMT.Text) > 0 Then
            txtAdjCash_AMT.Focus()
        End If
    End Sub

    Private Sub gvGstRegister_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles gvGstRegister.KeyPress
        If UCase(e.KeyChar) = "C" Then
            If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Cancel) Then Exit Sub
            If ChkDbClosed(cnStockDb) = True Then MsgBox("Transaction for this finincial year Closed." & vbCrLf & "Not able to Cancel.", MsgBoxStyle.Information) : Exit Sub
            Dim dgv As DataGridView = CType(sender, DataGridView)
            If dgv.CurrentRow Is Nothing Then Exit Sub
            If dgv.CurrentRow.Cells("BATCHNO").Value.ToString = "" Then Exit Sub
            If MessageBox.Show("Do you want to Cancel?", "Cancel Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
                Exit Sub
            End If
            Dim toCostId As String = dgv.CurrentRow.Cells("COSTID").Value.ToString
            Try
                If dgv.Item("CANCEL", dgv.CurrentRow.Index).Value.ToString = "CANCEL" Then
                    MsgBox("ALREADY CANCELLED", MsgBoxStyle.Information)
                    dgv.Focus()
                    Exit Sub
                End If
                Dim serverDate1 As Date = GetServerDate()
                Dim RestrictDays As String = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'DATECONTROL_MIMR'")
                Dim ledgerdate As Date = dgv.CurrentRow.Cells("TRANDATE").Value
                If RestrictDays.Contains(",") = False Then
                    If Not ledgerdate >= serverDate1.AddDays(-1 * Val(RestrictDays)) Or ledgerdate.Month <> serverDate1.Month Then
                        If ChkBckdateEditControl(ledgerdate, userId) = False Then
                            Exit Sub
                        End If
                    End If
                End If
                Dim objSecret As New frmAdminPassword()
                If objSecret.ShowDialog <> Windows.Forms.DialogResult.OK Then
                    Exit Sub
                End If
                Dim costId As String = dgv.Item("costid", dgv.CurrentRow.Index).Value.ToString
                Dim objRemark As New frmBillRemark
                objRemark.Text = "Cancel Remark"
                If objRemark.ShowDialog() <> Windows.Forms.DialogResult.OK Then
                    Exit Sub
                End If
                tran = Nothing
                tran = cn.BeginTransaction
                strSql = " INSERT INTO " & cnStockDb & "..CANCELLEDTRAN"
                strSql += vbCrLf + "  (TRANDATE,BATCHNO,UPDATED,UPTIME,FLAG,REMARK1,REMARK2,USERID)"
                strSql += vbCrLf + "  VALUES"
                strSql += vbCrLf + "  ("
                strSql += vbCrLf + "  '" & Format(dgv.CurrentRow.Cells("TRANDATE").Value, "yyyy-MM-dd") & "'"
                strSql += vbCrLf + "  ,'" & dgv.CurrentRow.Cells("BATCHNO").Value.ToString & "'"
                strSql += vbCrLf + "  ,'" & GetEntryDate(GetServerDate(tran), tran) & "'"
                strSql += vbCrLf + "  ,'" & GetServerTime(tran) & "'"
                strSql += vbCrLf + "  ,'C'" 'FLAG CANCEL OR DELETE
                strSql += vbCrLf + "  ,'" & objRemark.cmbRemark1_OWN.Text & "'" 'REMARK1
                strSql += vbCrLf + "  ,'" & objRemark.cmbRemark2_OWN.Text & "'" 'REMARK2
                strSql += vbCrLf + "  ," & userId & ""
                strSql += vbCrLf + "  )"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId)
                tran.Commit()
                tran = Nothing
                MsgBox("Successfully Cancelled", MsgBoxStyle.Information)
                btnView_Click(Me, New EventArgs)
                dgv.Focus()
            Catch ex As Exception
                If Not tran Is Nothing Then tran.Rollback()
                MsgBox(ex.Message + vbCrLf + ex.StackTrace)
            End Try
        ElseIf UCase(e.KeyChar) = "D" Then
            Dim dgv As DataGridView = CType(sender, DataGridView)
            If dgv.CurrentRow Is Nothing Then Exit Sub
            If dgv.CurrentRow.Cells("BATCHNO").Value.ToString = "" Then Exit Sub
            Dim toCostId As String = dgv.CurrentRow.Cells("COSTID").Value.ToString
            If IO.File.Exists(Application.StartupPath & "\BillPrint.exe") Then
                Dim prnmemsuffix As String = ""
                If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId
                Dim memfile As String = "\BillPrint" + prnmemsuffix.Trim & ".mem"

                Dim write As IO.StreamWriter
                write = IO.File.CreateText(Application.StartupPath & memfile)
                If dgv.CurrentRow.Cells("TYPE").Value.ToString = "I" Then
                    write.WriteLine(LSet("TYPE", 15) & ":ACC")
                Else
                    write.WriteLine(LSet("TYPE", 15) & ":ACC")
                End If
                write.WriteLine(LSet("BATCHNO", 15) & ":" & dgv.CurrentRow.Cells("BATCHNO").Value.ToString)
                write.WriteLine(LSet("TRANDATE", 15) & ":" & Format(dgv.CurrentRow.Cells("TRANDATE").Value, "yyyy-MM-dd"))
                write.WriteLine(LSet("DUPLICATE", 15) & ":Y")
                write.Flush()
                write.Close()
                If EXE_WITH_PARAM = False Then
                    System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe")
                Else
                    System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe",
                    LSet("TYPE", 15) & ":" & IIf(dgv.CurrentRow.Cells("TYPE").Value.ToString = "I", "ACC", "ACC") & ";" &
                    LSet("BATCHNO", 15) & ":" & dgv.CurrentRow.Cells("BATCHNO").Value.ToString & ";" &
                    LSet("TRANDATE", 15) & ":" & Format(dgv.CurrentRow.Cells("TRANDATE").Value, "yyyy-MM-dd") & ";" &
                    LSet("DUPLICATE", 15) & ":Y")
                End If
            Else
                MsgBox("Billprint exe not found", MsgBoxStyle.Information)
            End If
        ElseIf UCase(e.KeyChar) = "E" Then
            Dim dgv As DataGridView = CType(sender, DataGridView)
            If dgv.CurrentRow Is Nothing Then Exit Sub
            If ChkDbClosed(cnStockDb) = True Then MsgBox("Transaction for this finincial year Closed." & vbCrLf & "Not able to Edit.", MsgBoxStyle.Information) : Exit Sub
            If dgv.CurrentRow.Cells("BATCHNO").Value.ToString = "" Then Exit Sub
            If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Edit) Then Exit Sub
            'If chkbackdateedit(dgv.CurrentRow.Cells("TRANDATE").Value) = False Then Exit Sub
            editBatchno = dgv.CurrentRow.Cells("BATCHNO").Value.ToString()
            LoadEditValues()
        End If
    End Sub
    Private Sub LoadEditValues()
        strSql = vbCrLf + "  SELECT I.COSTID,I.ACCODE,I.TRANDATE,I.TRANNO,I.STATEID"
        strSql += vbCrLf + "  ,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = I.COSTID)AS COSTNAME"
        strSql += vbCrLf + "  ,(SELECT STATENAME FROM " & cnAdminDb & "..STATEMAST WHERE STATEID = I.STATEID)AS STATENAME"
        strSql += vbCrLf + "  ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = I.ACCODE)AS ACNAME"
        strSql += vbCrLf + "  ,(SELECT TOP 1 REMARK1 FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = I.BATCHNO AND REMARK1 <> '')AS REMARK1"
        strSql += vbCrLf + "  ,(SELECT TOP 1 REMARK2 FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = I.BATCHNO AND REMARK2 <> '')AS REMARK2"
        strSql += vbCrLf + "  FROM " & cnStockDb & "..GSTREGISTER AS I"
        strSql += vbCrLf + "  WHERE BATCHNO = '" & editBatchno & "' AND ISNULL(CANCEL,'')=''"
        Dim DtCommanInfo As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(DtCommanInfo)
        If Not DtCommanInfo.Rows.Count > 0 Then Exit Sub
        Dim Ro As DataRow = DtCommanInfo.Rows(0)
        EditCostId = Ro.Item("COSTID").ToString
        stateId = Val(Ro.Item("STATEID").ToString)
        cmbCostCentre_MAN.Text = Ro.Item("COSTNAME").ToString
        cmbDrAcName_OWN.Text = Ro.Item("ACNAME").ToString
        cmbStateName_MAN.Text = Ro.Item("STATENAME").ToString
        _Accode = Ro.Item("ACCODE").ToString
        dtpTranDate.Value = Ro.Item("TRANDATE")
        EditTranNo = Val(Ro.Item("TRANNO").ToString)
        strSql = vbCrLf + "  SELECT  row_number() over (order by newid()) KEYNO,"
        strSql += vbCrLf + "  I.SNO,I.TRANNO,I.TRANDATE,I.DESCRIPTION,I.HSN,I.PCS,I.WEIGHT,I.RATE,I.AMOUNT,I.TAX GSTPER,TDS,TDSPER"
        strSql += vbCrLf + "  ,I.TAXAMOUNT GSTAMOUNT,I.SGST_PER,I.SGST,I.CGST_PER,I.CGST,I.IGST_PER "
        strSql += vbCrLf + "  ,I.IGST,I.STATEID,I.ACCODE,I.REFNO,I.REFDATE "
        strSql += vbCrLf + "  ,(SELECT GSTCLAIMNAME FROM " & cnAdminDb & "..GSTCLAIM WHERE GSTCLAIMID=I.GSTCLAIM) GSTCLAIM "
        strSql += vbCrLf + "  ,I.GSTCLAIM GSTCLAIMID "
        strSql += vbCrLf + "  ,I.GSTCATID"
        strSql += vbCrLf + "  ,CONTRA"
        strSql += vbCrLf + "  ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE =I.CONTRA)CONTRANAME"
        strSql += vbCrLf + "  ,ISNULL(ENTTYPE,'R') ENTTYPE"
        strSql += vbCrLf + "  FROM " & cnStockDb & "..GSTREGISTER AS I"
        strSql += vbCrLf + "  WHERE BATCHNO = '" & editBatchno & "' "
        strSql += vbCrLf + "  And ISNULL(CANCEL,'')='' "
        Dim dtTranInfo As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtTranInfo)
        If Not dtTranInfo.Rows.Count > 0 Then Exit Sub
        For Each Ro In dtTranInfo.Rows
            If Ro.Item("SNO").ToString = "" Then Ro.Item("SNO") = DBNull.Value
            If Val(Ro.Item("TRANNO").ToString) = 0 Then Ro.Item("TRANNO") = DBNull.Value
            If Ro.Item("DESCRIPTION").ToString = "" Then Ro.Item("DESCRIPTION") = DBNull.Value
            If Val(Ro.Item("HSN").ToString) = 0 Then Ro.Item("HSN") = DBNull.Value
            If Val(Ro.Item("PCS").ToString) = 0 Then Ro.Item("PCS") = DBNull.Value
            If Val(Ro.Item("WEIGHT").ToString) = 0 Then Ro.Item("WEIGHT") = DBNull.Value
            If Val(Ro.Item("RATE").ToString) = 0 Then Ro.Item("RATE") = DBNull.Value
            If Val(Ro.Item("AMOUNT").ToString) = 0 Then Ro.Item("AMOUNT") = DBNull.Value
            If Val(Ro.Item("GSTPER").ToString) = 0 Then Ro.Item("GSTPER") = DBNull.Value
            If Val(Ro.Item("GSTAMOUNT").ToString) = 0 Then Ro.Item("GSTAMOUNT") = DBNull.Value
            If Val(Ro.Item("SGST_PER").ToString) = 0 Then Ro.Item("SGST_PER") = DBNull.Value
            If Val(Ro.Item("SGST").ToString) = 0 Then Ro.Item("SGST") = DBNull.Value
            If Val(Ro.Item("CGST_PER").ToString) = 0 Then Ro.Item("CGST_PER") = DBNull.Value
            If Val(Ro.Item("CGST").ToString) = 0 Then Ro.Item("CGST") = DBNull.Value
            If Val(Ro.Item("IGST_PER").ToString) = 0 Then Ro.Item("IGST_PER") = DBNull.Value
            If Val(Ro.Item("IGST").ToString) = 0 Then Ro.Item("IGST") = DBNull.Value
            If Val(Ro.Item("TDSPER").ToString) = 0 Then Ro.Item("TDSPER") = DBNull.Value
            If Val(Ro.Item("TDS").ToString) = 0 Then Ro.Item("TDS") = DBNull.Value
            If Ro.Item("REFDATE").ToString = "" Then Ro.Item("REFDATE") = DBNull.Value
            If Val(Ro.Item("REFNO").ToString) = 0 Then Ro.Item("REFNO") = DBNull.Value
            If Ro.Item("GSTCLAIM").ToString = "" Then Ro.Item("GSTCLAIM") = DBNull.Value
            If Val(Ro.Item("GSTCATID").ToString) = 0 Then Ro.Item("GSTCATID") = DBNull.Value
            If Ro.Item("REFDATE").ToString <> "" Then dtpRefDate.Value = Ro.Item("REFDATE")
            If Ro.Item("ENTTYPE").ToString = "R" Then
                rbtWithAcc.Checked = True
            Else
                rbtWOAcc.Checked = True
            End If
            Dim dr As DataRow
            dr = dtGrid.NewRow
            dr.Item("KEYNO") = Ro.Item("KEYNO").ToString
            dr.Item("CRACNAME") = Ro.Item("CONTRANAME").ToString
            dr.Item("CRACCCODE") = Ro.Item("CONTRA").ToString
            dr.Item("DESCRIPTION") = Ro.Item("DESCRIPTION").ToString
            dr.Item("HSN") = Ro.Item("HSN").ToString
            dr.Item("PCS") = Val(Ro.Item("PCS").ToString)
            dr.Item("WEIGHT") = Format(Val(Ro.Item("WEIGHT").ToString), "0.00")
            dr.Item("RATE") = Format(Val(Ro.Item("RATE").ToString), "0.00")
            dr.Item("AMOUNT") = Format(Val(Ro.Item("AMOUNT").ToString), "0.00")
            dr.Item("TAX") = Format(Val(Ro.Item("GSTPER").ToString), "0.00")
            dr.Item("TAXAMOUNT") = Format(Val(Ro.Item("GSTAMOUNT").ToString), "0.00")
            dr.Item("SGST_PER") = Format(Val(Ro.Item("SGST_PER").ToString), "0.00")
            dr.Item("SGST") = Format(Val(Ro.Item("SGST").ToString), "0.00")
            dr.Item("CGST_PER") = Format(Val(Ro.Item("CGST_PER").ToString), "0.00")
            dr.Item("CGST") = Format(Val(Ro.Item("CGST").ToString), "0.00")
            dr.Item("IGST_PER") = Format(Val(Ro.Item("IGST_PER").ToString), "0.00")
            dr.Item("IGST") = Format(Val(Ro.Item("IGST").ToString), "0.00")
            dr.Item("TDS_PER") = Format(Val(Ro.Item("TDSPER").ToString), "0.00")
            dr.Item("TDS") = Format(Val(Ro.Item("TDS").ToString), "0.00")
            dr.Item("STATEID") = Val(Ro.Item("STATEID").ToString)
            dr.Item("ACCODE") = Val(Ro.Item("ACCODE").ToString)
            dr.Item("REFNO") = Ro.Item("REFNO").ToString
            dr.Item("REFDATE") = dtpRefDate.Value.Date.ToString("dd-MM-yyyy")
            dr.Item("GSTCLAIM") = Ro.Item("GSTCLAIM").ToString
            dr.Item("GSTCLAIMID") = Ro.Item("GSTCLAIMID").ToString
            dr.Item("GSTCATID") = Ro.Item("GSTCATID").ToString
            dtGrid.Rows.Add(dr)
        Next
        dtpTranDate.Focus()
        tabMain.SelectedTab = tabGen
        GridViewGst.DataSource = Nothing
        GridViewGst.DataSource = dtGrid
        StyleGridSASR(GridViewGst)
        'Clear TextBox
        txtDescrip_MAN.Text = ""
        txthsn.Text = ""
        txtPcs_NUM.Text = ""
        txtWeight_WET.Text = ""
        txtRate_AMT.Text = ""
        txtamt_AMT.Text = ""
        txtGstPer_AMT.Text = ""
        txtGsttot_Amt.Text = ""
        txtSgst_per.Text = ""
        txtSgst_AMT.Text = ""
        txtCgst_per.Text = ""
        txtCgst_AMT.Text = ""
        txtIgst_per.Text = ""
        txtIgst_AMT.Text = ""
        txtRefno.Text = ""
        lblEditKeyNo.Text = ""
        GridTotal()
        txtDescrip_MAN.Focus()

    End Sub
    Private Function ChkBckdateEditControl(ByVal ledgerdate As Date, ByVal userId As Integer) As Boolean
        Dim tranDate As Date = GetActualEntryDate()
        Dim serverDate As Date = GetServerDate()
        Dim bckExpDate, editDate As Date
        Dim opType As String
        Dim opYr As String
        Dim opMonth As String
        Dim toCostid As String
        Dim dtBck As New DataTable

        Dim trnMonth As String = GetSqlValue(cn, "SELECT DATENAME (MONTH,'" & ledgerdate & "')MONTH")
        Dim trnYr As String = GetSqlValue(cn, "SELECT DATENAME (YEAR,'" & ledgerdate & "')YEAR")
        strSql = Nothing
        strSql = "SELECT BCKUSERID FROM " & cnAdminDb & "..BACKDATEENTRY WHERE BCKOPTIONNAME ='ACCOUNTS_BACKDATE' AND BCKSTATUS NOT IN('C','E')  "
        strSql += vbCrLf + " AND COSTID='" & cnCostId & "' AND BCKMONTH='" & trnMonth & "' AND BCKYEAR='" & trnYr & "' AND BCKUSERID=" & userId
        Dim bckUserId As Integer = GetSqlValue(cn, strSql)
        If bckUserId <> 0 Then
            dtBck = New DataTable
            strSql = " SELECT BCKOPTIONTYPE,BCKVALUE,BCKVALUEDATE,BCKID,BCKYEAR,BCKMONTH  FROM " & cnAdminDb & "..BACKDATEENTRY WHERE BCKOPTIONNAME ='MRMI_BACKDATE' AND"
            strSql += vbCrLf + " BCKSTATUS NOT IN('C','E')   AND BCKMONTH='" & trnMonth & "' AND BCKYEAR='" & trnYr & "'  AND COSTID='" & cnCostId & "' AND BCKUSERID= " & bckUserId
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtBck)
            If ((dtBck.Rows.Count) > 0) Then
                opType = dtBck.Rows(0).Item("BCKOPTIONTYPE")
                opYr = dtBck.Rows(0).Item("BCKYEAR")
                opMonth = dtBck.Rows(0).Item("BCKMONTH")
                editDate = CType(dtBck.Rows(0).Item("BCKVALUEDATE"), Date)
                EditBckId = dtBck.Rows(0).Item("BCKID")
                If opType = "DT" Then
                    If Not (ledgerdate = editDate) Then
                        MsgBox("Transaction Date to be edit Not Allowed", MsgBoxStyle.Information)
                        Return False
                        Exit Function
                    End If
                Else

                    If Not (GetSqlValue(cn, "SELECT DATENAME (MONTH,'" & ledgerdate & "')MONTH") = opMonth) And Not (GetSqlValue(cn, "SELECT DATENAME (YEAR,'" & ledgerdate & "')YEAR") = opYr) Then
                        MsgBox("Transaction Date to be edit Not Allowed", MsgBoxStyle.Information)
                        Return False
                        Exit Function
                    End If
                End If
            Else
                MsgBox("Transaction Date to be edit Not Allowed", MsgBoxStyle.Information)
                Return False
                Exit Function
            End If
        Else
            dtBck = New DataTable
            strSql = " SELECT BCKOPTIONTYPE,BCKVALUE,BCKVALUEDATE,BCKID,BCKYEAR,BCKMONTH  FROM " & cnAdminDb & "..BACKDATEENTRY WHERE BCKOPTIONNAME ='ACCOUNTS_BACKDATE' AND"
            strSql += vbCrLf + " BCKSTATUS NOT IN('C','E')  AND BCKMONTH='" & trnMonth & "' AND BCKYEAR='" & trnYr & "'   AND COSTID='" & cnCostId & "' AND BCKUSERID= 0"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtBck)
            If ((dtBck.Rows.Count) > 0) Then
                opType = dtBck.Rows(0).Item("BCKOPTIONTYPE")
                opYr = dtBck.Rows(0).Item("BCKYEAR")
                opMonth = dtBck.Rows(0).Item("BCKMONTH")
                editDate = CType(dtBck.Rows(0).Item("BCKVALUEDATE"), Date)
                EditBckId = dtBck.Rows(0).Item("BCKID")
                If opType = "DT" Then
                    If Not (ledgerdate = editDate) Then
                        MsgBox("Transaction Date to be edit Not Allowed", MsgBoxStyle.Information)
                        Return False
                        Exit Function
                    End If
                Else
                    If Not (GetSqlValue(cn, "SELECT DATENAME (MONTH,'" & ledgerdate & "')MONTH") = opMonth) And Not (GetSqlValue(cn, "SELECT DATENAME (YEAR,'" & ledgerdate & "')YEAR") = opYr) Then
                        MsgBox("Transaction Date to be edit Not Allowed", MsgBoxStyle.Information)
                        Return False
                        Exit Function
                    End If
                End If
            Else
                MsgBox("Transaction Date to be edit Not Allowed", MsgBoxStyle.Information)
                Return False
                Exit Function
            End If
        End If
        Return True
    End Function
    Private Function chkbackdateedit(ByVal ledgerdate As Date) As Boolean

        Dim serverDate As Date = GetActualEntryDate()
        Dim RestrictDays As String = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'DATECONTROL_MIMR'")
        If RestrictDays.Contains(",") = False Then
            If Not (ledgerdate >= serverDate.AddDays(-1 * Val(RestrictDays))) Or ledgerdate.Month <> serverDate.Month Then
                If ChkBckdateEditControl(ledgerdate, userId) = False Then
                    Return False
                    Exit Function
                Else
                    Return True
                    Exit Function
                End If
                MsgBox("Transaction Date to be edit Not Allowed", MsgBoxStyle.Information)
                Return False
                Exit Function
            End If
        Else
            Dim dayarray() As String = Split(RestrictDays, ",")
            Dim closeday As Integer = Val(dayarray(0).ToString)
            Dim mondiv As Integer = Val(dayarray(1).ToString)
            If mondiv = ledgerdate.Month Then
                Return True
                Exit Function
            End If
            If closeday > serverDate.Day Then
                Dim mindate As Date = serverDate.AddMonths(-1 * mondiv)
                mindate = mindate.AddDays(-1 * (mindate.Day - 1))
                If Not (ledgerdate >= mindate) Then
                    MsgBox("Transaction Date to be edit Not Allowed", MsgBoxStyle.Information)

                    Return False
                    Exit Function
                End If
            Else
                Dim mindate As Date = serverDate.AddMonths(-1 * 0)
                mindate = mindate.AddDays(-1 * (mindate.Day - 1))
                If Not (ledgerdate >= mindate) Then
                    MsgBox("Transaction Date to be edit Not Allowed", MsgBoxStyle.Information)
                    Return False
                    Exit Function
                End If
            End If
        End If
        Return True
    End Function
    Private Sub GridLoad()
        strSql = "SELECT SNO,TRANDATE,TRANNO,DESCRIPTION,HSN,PCS,WEIGHT"
        strSql += vbCrLf + ",RATE,AMOUNT,TAX,TAXAMOUNT,AMOUNT+TAXAMOUNT TOTALAMOUNT"
        strSql += vbCrLf + ",SGST_PER,SGST,CGST_PER,CGST,IGST_PER,IGST,STATEID,ACCODE,REFNO,REFDATE,"
        strSql += vbCrLf + "GSTCLAIM, TRANTYPE, UPLOAD,BATCHNO,COSTID,CASE WHEN CANCEL='Y' THEN 'CANCEL' ELSE 'NO' END CANCEL,'I' TYPE,"
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
        strSql += vbCrLf + "(SELECT TOP 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = G.CONTRA) ACNAME,"
        strSql += vbCrLf + "(SELECT TOP 1 GSTNO FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = G.CONTRA) GSTNO"
        strSql += vbCrLf + " FROM " & cnStockDb & "..GSTREGISTER G"
        strSql += vbCrLf + " WHERE TRANDATE BETWEEN'" & dtpSrcTrandate.Value.Date.ToString("yyyy-MM-dd") & "' "
        strSql += vbCrLf + " And '" & dtpsrctotrandate.Value.Date.ToString("yyyy-MM-dd") & "' "
        strSql += vbCrLf + " And ISNULL(TRANTYPE,'') in ('','JE','PE') "
        strSql += vbCrLf + " And ISNULL(CANCEL,'')='' "
        strSql += vbCrLf + " ORDER BY TRANDATE,TRANNO"
        cmd = New OleDbCommand(strSql, cn)
        dt = New DataTable
        da = New OleDbDataAdapter(cmd)
        da.Fill(dt)
        gvGstRegister.DataSource = dt
    End Sub
    Public Sub GridCanceDesign()
        For Each dgvRow As DataGridViewRow In gvGstRegister.Rows
            With dgvRow
                Select Case .Cells("CANCEL").Value.ToString
                    Case "CANCEL"
                        .DefaultCellStyle.BackColor = Color.Red
                        .DefaultCellStyle.Font = New Font("VERDANA", 7, FontStyle.Bold) 'reportHeadStyle.Font                   
                End Select
            End With
        Next
    End Sub


    Private Sub ResizeToolStripMenuItem_Click_1(sender As System.Object, e As System.EventArgs) Handles ResizeToolStripMenuItem.Click
        If gvGstRegister.RowCount > 0 Then
            If ResizeToolStripMenuItem.Checked Then
                gvGstRegister.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                gvGstRegister.Invalidate()
                For Each dgvCol As DataGridViewColumn In gvGstRegister.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gvGstRegister.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            Else
                For Each dgvCol As DataGridViewColumn In gvGstRegister.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gvGstRegister.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End If
        End If
    End Sub
    Private Sub rbtWOAcc_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtWOAcc.CheckedChanged
        If rbtWOAcc.Checked Then
            lblDrAcc.Text = "A/C Name"
            'lblCrAcc.Visible = False
            'cmbCrAcName.Visible = False
            lblCrAcc.Visible = True
            cmbCrAcName_OWN.Visible = True
            cmbCrAcName_OWN.Enabled = True
            grpPayment.Visible = True
            Label26.Visible = True
            txtAdjCredit_AMT.Visible = True
            txtAdjCredit_AMT.ReadOnly = True

            pnlRemark.Visible = True
            If GST_GEN_WITHCRACC Then
                lblDrAcc.Text = "A/C Name"
                lblCrAcc.Visible = True
                cmbCrAcName_OWN.Visible = True
                pnlRemark.Visible = True
            End If
            'LoadcmbxCreditAccount()
            'AcName()
            'funGstClaimLoadRegister("U")
            funLoaddatatableUnRegister()

        Else
            lblDrAcc.Text = "A/C Name"
            lblCrAcc.Visible = True
            cmbCrAcName_OWN.Visible = True
            cmbCrAcName_OWN.Enabled = True
            grpPayment.Visible = False
            If GST_GEN_WITHCRACC Then
                grpPayment.Visible = True
                Label26.Visible = True
                txtAdjCredit_AMT.Visible = True
                txtAdjCredit_AMT.ReadOnly = False
            ElseIf GST_GEN_CACC_POST_REG = True Then
                grpPayment.Visible = True
                Label26.Visible = True
                txtAdjCredit_AMT.Visible = True
                txtAdjCredit_AMT.ReadOnly = False
            Else
                Label26.Visible = False
                txtAdjCredit_AMT.Visible = False
            End If
            cmbStateName_MAN.Text = CompanyState
            btnSave.Enabled = True
            pnlRemark.Visible = True
            'LoadcmbxCreditAccount()
            'AcName()
            'funGstClaimLoadRegister("R")
            funLoaddatatableRegister()

        End If
    End Sub

    'Private Sub rbtWithAcc_CheckedChanged(sender As Object, e As System.EventArgs) Handles rbtWithAcc.CheckedChanged
    '    If rbtWithAcc.Checked Then
    '        lblDrAcc.Text = "Dr"
    '        lblCrAcc.Visible = True
    '        cmbCrAcName.Visible = True
    '        grpPayment.Visible = False
    '        cmbStateName_MAN.Text = CompanyState
    '        btnSave.Enabled = True
    '        pnlRemark.Visible = True
    '    Else
    '        lblDrAcc.Text = "A/C Name"
    '        lblCrAcc.Visible = False
    '        cmbCrAcName.Visible = False
    '        grpPayment.Visible = True
    '        pnlRemark.Visible = False
    '    End If
    'End Sub


    Private Sub txthsn_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txthsn.KeyDown
        If e.KeyCode = Keys.Enter And rbtWithAcc.Checked = True Then
            'txtRate_AMT.Select()
            'txtRate_AMT.Focus()
        End If
    End Sub

    Private Sub txtSgst_AMT_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtSgst_AMT.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            calculation()
        End If
    End Sub

    Private Sub txthsn_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txthsn.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If cmbGstClaim.Text = "GST" Then If txthsn.Text.ToString.Trim = "" Then MsgBox("HSN Code should not be empty...", MsgBoxStyle.Information) : txthsn.Focus() : Exit Sub
        End If
    End Sub

    Private Sub txtAdjCredit_AMT_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAdjCredit_AMT.TextChanged
        funcCalcBal()
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        'If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        'Me.Cursor = Cursors.WaitCursor
        Dim title As String
        title = "GST REGISTER REPORT "
        If gvGstRegister.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, title, gvGstRegister, BrightPosting.GExport.GExportType.Export)
        End If
        Me.Cursor = Cursors.Arrow

    End Sub

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        'If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        Dim title As String
        title = "GST REGISTER REPORT "
        If gvGstRegister.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, title, gvGstRegister, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        'tranDate = dtpSrcTrandate.Value.Date.ToString("dd-MM-yyyy")
        tranDate = dtpSrcTrandate.Value.Date.ToString
        tranDateTO = dtpsrctotrandate.Value.Date.ToString
        Call GridLoad()
        Call GridDesign()
        Call GridCanceDesign()
    End Sub

    Private Sub txtamt_AMT_GotFocus(sender As Object, e As EventArgs) Handles txtamt_AMT.GotFocus
        If Val(txtRate_AMT.Text) > 0 Then
            txtamt_AMT.Text = Format(Math.Round(IIf(Val(txtPcs_NUM.Text) = 0, 1, Val(txtPcs_NUM.Text)) * Val(txtRate_AMT.Text), 2), "0.00")
        Else
            txtamt_AMT.ReadOnly = False
        End If
    End Sub
    Private Sub txtTCS_AMT_TextChanged(sender As Object, e As EventArgs) Handles txtTCS_AMT.TextChanged
        calculation()
    End Sub
    Private Sub DueToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DueToolStripMenuItem.Click
        txtAdjDue_AMT.Focus()
        txtAdjDue_AMT.SelectAll()
    End Sub
    Private Sub txtAdjDue_AMT_GotFocus(sender As Object, e As EventArgs) Handles txtAdjDue_AMT.GotFocus
        txtAdjDue_AMT.Focus()
        txtAdjDue_AMT.SelectAll()
    End Sub
    Private Sub txtAdjCheque_AMT_GotFocus1(sender As Object, e As EventArgs) Handles txtAdjCheque_AMT.GotFocus
        txtAdjCheque_AMT.Focus()
        txtAdjCheque_AMT.SelectAll()
    End Sub
    Private Sub txtAdjCash_AMT_GotFocus(sender As Object, e As EventArgs) Handles txtAdjCash_AMT.GotFocus
        txtAdjCash_AMT.Focus()
        txtAdjCash_AMT.SelectAll()
    End Sub

    Private Sub rbtWithAcc_CheckedChanged(sender As Object, e As EventArgs) Handles rbtWithAcc.CheckedChanged

    End Sub
End Class