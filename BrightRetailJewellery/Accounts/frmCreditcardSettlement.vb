Imports System.Data.OleDb

Public Class frmCreditcardSettlement
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    'Dim dtpGridFlag As Boolean
    'Dim preTranDb As String
    'Dim curTranDb As String
    'Dim objSearch As Object
    'Dim dtCostCentre As New DataTable
    'Dim EditMode As Boolean = False
    'Dim commonTranDb As New ArrayList()
    Dim dtGrid As New DataTable()
    Dim Dt As New DataTable
    Dim billctrl_ctlid As String = ""
    Public TranNo As Integer = Nothing
    Public BatchNo As String = Nothing

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub frmBrsExcelDownload_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub frmBrsExcelDownload_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        LoadAcname()
        lblStatus.Text = ""
    End Sub

    Private Sub LoadAcname()
        strSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD "
        strSql += " WHERE ACCODE IN(SELECT DEFAULTAC FROM " & cnAdminDb & "..CREDITCARD WHERE ISNULL(ACTIVE,'Y')<>'N' AND CARDTYPE<>'C')  "
        strSql += GetAcNameQryFilteration()
        strSql += vbCrLf + "  ORDER BY ACNAME"
        objGPack.FillCombo(strSql, cmbBank_MAN)

        strSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD "
        strSql += " WHERE 1=1"
        'strSql += GetAcNameQryFilteration()
        strSql += vbCrLf + "  ORDER BY ACNAME"
        objGPack.FillCombo(strSql, cmbchargesAC_OWN)
        objGPack.FillCombo(strSql, cmbschargesAC_OWN)
        objGPack.FillCombo(strSql, cmbstaxac_OWN)

        strSql = "  SELECT * FROM ("
        strSql += vbCrLf + "  SELECT 'JOURNAL'CAPTION,'JE'PAYMODE,0 RESULT"
        strSql += vbCrLf + "  UNION ALL"
        strSql += vbCrLf + "  SELECT CAPTION,PAYMODE,1 FROM " & cnAdminDb & "..ACCENTRYMASTER WHERE PAYMODE NOT IN('CR','CP','JE','DN','CN'))X "
        strSql += vbCrLf + "  ORDER BY RESULT"
        Dim pdt As New DataTable
        pdt = GetSqlTable(strSql, cn)
        If pdt.Rows.Count > 0 Then
            cmbGenerateVoucher_OWN.DataSource = Nothing
            cmbGenerateVoucher_OWN.DataSource = pdt
            cmbGenerateVoucher_OWN.DisplayMember = "CAPTION"
            cmbGenerateVoucher_OWN.ValueMember = "PAYMODE"
        Else
            cmbGenerateVoucher_OWN.DataSource = Nothing
        End If
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        cmbBank_MAN.Text = ""
        cmbchargesAC_OWN.Text = ""
        cmbschargesAC_OWN.Text = ""
        cmbstaxac_OWN.Text = ""
        txtPath.Clear()
        gridView_OWN.DataSource = Nothing
        lblBankName.Text = ""
        lblStatus.Text = ""
        LoadAcname()
        dtpTranDate.Select()
    End Sub

    Private Sub frmBankReconciliation_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
       
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView_OWN.KeyDown
        If Not gridView_OWN.RowCount > 0 Then Exit Sub
        If e.KeyCode = Keys.E Then
            e.Handled = True
            If gridView_OWN.CurrentRow.Cells("TRANDATE").Value.ToString = "" Then Exit Sub
            gridView_OWN.CurrentCell = gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("RELIASEDATE")
        ElseIf e.KeyCode = Keys.Enter Then
            'UpdateRealise()
        End If
    End Sub

    Private Sub dtpGridRealise_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        lblStatus.Visible = True
    End Sub

    Private Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnPath_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPath.Click
        Dim OpenDialog As New OpenFileDialog
        Dim Str As String
        Str = "(*.xls) | *.xls"
        Str += "|(*.xlsx) | *.xlsx"
        OpenDialog.Filter = Str
        If OpenDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            txtPath.Text = OpenDialog.FileName
            btnSearch.Focus()
        End If
    End Sub
    Private Sub getExcelData()
        Try
            Dim MyConnection As System.Data.OleDb.OleDbConnection
            MyConnection = New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" & txtPath.Text & "';Extended Properties=""Excel 12.0 Xml;HDR=YES;""")
            'MyConnection = New OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;Data Source='" & txtPath.Text & "';Extended Properties=Excel 8.0;")
            strSql = "SELECT * FROM [SHEET1$]"
            da = New OleDbDataAdapter(strSql, MyConnection)
            Dt = New DataTable
            da.Fill(Dt)
            MyConnection.Close()
            If Dt.Columns.Count <> 7 Then MsgBox("Excel Template Not in Format", MsgBoxStyle.Information) : btnNew_Click(Me, New EventArgs) : Exit Sub
        Catch ex As Exception
            MsgBox("Excel Template Not in Format")
        End Try
    End Sub

    Private Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView_OWN.Rows.Count > 0 Then
            Dim Title As String = cmbBank_MAN.Text + vbCrLf
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "", gridView_OWN, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView_OWN.Rows.Count > 0 Then
            Dim Title As String = cmbBank_MAN.Text + vbCrLf
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "", gridView_OWN, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub txtPath_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPath.GotFocus
        lblStatus.Text = "Press F6 to See Sample Template For Excel"
    End Sub
    Private Sub txtPath_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtPath.KeyDown
        If e.KeyCode = Keys.F6 Then
            Dim dtex As New DataTable
            With dtex.Columns
                .Add("COSTID", GetType(String))
                .Add("TRANDATE", GetType(String))
                .Add("CREDITCARDACCOUNT", GetType(Double))
                .Add("BANKACCOUNT", GetType(Double))
                .Add("CREDITCARDCHARGESACCOUNT", GetType(Double))
                .Add("CREDITCARDSERVICETAXACCOUNT", GetType(Double))
                .Add("REMARK", GetType(String))
            End With
            gridView_OWN.DataSource = Nothing
            Dim row As DataRow
            row = dtex.NewRow
            dtex.Rows.Add(row)
            row = dtex.NewRow
            dtex.Rows.Add(row)
            gridView_OWN.DataSource = dtex

            gridView_OWN.Columns("TRANDATE").Width = 90
            gridView_OWN.Columns("CREDITCARDACCOUNT").Width = 120
            gridView_OWN.Columns("BANKACCOUNT").Width = 120
            gridView_OWN.Columns("CREDITCARDCHARGESACCOUNT").Width = 180
            gridView_OWN.Columns("CREDITCARDSERVICETAXACCOUNT").Width = 180


            gridView_OWN.Columns("TRANDATE").HeaderText = "Trandate"
            gridView_OWN.Columns("CREDITCARDACCOUNT").HeaderText = "CreditCard Account"
            gridView_OWN.Columns("BANKACCOUNT").HeaderText = "Bank Account"
            gridView_OWN.Columns("CREDITCARDCHARGESACCOUNT").HeaderText = "CreditCard Charges Account"
            gridView_OWN.Columns("CREDITCARDSERVICETAXACCOUNT").HeaderText = "CreditCard ServiceTax Account"

            Me.Refresh()
        End If
    End Sub

    Private Sub txtPath_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPath.LostFocus
        lblStatus.Text = ""
        gridView_OWN.DataSource = Nothing
    End Sub

    Private Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click, saveToolStripMenuItem1.Click
        billctrl_ctlid = "GEN-" & cmbGenerateVoucher_OWN.SelectedValue.ToString
        Try
            If gridView_OWN.Rows.Count <= 0 Then MsgBox("No Record to Save.") : Exit Sub
            If CheckTrialDate(dtpTranDate.Value) = False Then Exit Sub
            Dim sdt As New DataTable
            sdt = CType(gridView_OWN.DataSource, DataTable)
            tran = Nothing
            tran = cn.BeginTransaction()
            For Each ro As DataRow In sdt.Rows
                Dim Isfirst As Boolean = True
GenBillNo:
                If IIf(IsDBNull(ro!COSTID.ToString), "", ro!COSTID.ToString) = "" Then
                    MsgBox("Costid should not empty." & vbCrLf & "Pls update Excel.", MsgBoxStyle.Information)
                    gridView_OWN.DataSource = Nothing
                    txtPath.Text = ""
                    txtPath.Focus()
                    Exit Sub
                ElseIf Val(IIf(IsDBNull(ro!CREDITCARDACCOUNT.ToString), 0, ro!CREDITCARDACCOUNT.ToString)) = 0 Then
                    MsgBox("CreditCard Account should not empty." & vbCrLf & "Pls update Excel.", MsgBoxStyle.Information)
                    gridView_OWN.DataSource = Nothing
                    txtPath.Text = ""
                    txtPath.Focus()
                    Exit Sub
                End If
                If objGPack.GetSqlValue("SELECT COUNT(*) FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = '" & ro!COSTID.ToString & "'", , 0, tran) = 0 Then MsgBox("Costid not Found." & vbCrLf & "Pls update Excel.", MsgBoxStyle.Information) : Exit Sub
                Dim Creditvalue As Double = Val(IIf(IsDBNull(ro!CREDITCARDACCOUNT.ToString), 0, Math.Round(Val(ro!CREDITCARDACCOUNT.ToString), 2)))
                Dim Debitvalue As Double = Math.Round(Val(IIf(IsDBNull(ro!BANKACCOUNT.ToString), 0, Val(ro!BANKACCOUNT.ToString)) _
                + IIf(IsDBNull(ro!CREDITCARDCHARGESACCOUNT.ToString), 0, Val(ro!CREDITCARDCHARGESACCOUNT.ToString)) _
                + IIf(IsDBNull(ro!CREDITCARDSERVICETAXACCOUNT.ToString), 0, Val(ro!CREDITCARDSERVICETAXACCOUNT.ToString))), 2)

                If (Creditvalue - Debitvalue) <> 0 Then gridView_OWN.DataSource = Nothing : MsgBox("Debit Credit Not Tally." & vbCrLf & "Pls update Excel.", MsgBoxStyle.Information) : Exit Sub
                TranNo = Val(GetBillControlValue(billctrl_ctlid, tran, Not Isfirst))
                strSql = " UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT = '" & TranNo + 1 & "'"
                strSql += " WHERE CTLID = '" & billctrl_ctlid & "' AND COMPANYID = '" & strCompanyId & "'"
                If Isfirst And strBCostid <> Nothing Then strSql += " AND COSTID ='" & strBCostid & "'"
                strSql += " AND CONVERT(INT,CTLTEXT) = '" & TranNo & "'"
                cmd = New OleDbCommand(strSql, cn, tran)
                If Not cmd.ExecuteNonQuery() > 0 Then
                    If strBCostid <> Nothing Then MsgBox("Tran No. empty. Please check Bill control") : tran.Rollback() : tran.Dispose() : tran = Nothing : Exit Sub
                    Isfirst = False
                    GoTo GenBillNo
                End If
                TranNo += 1
                BatchNo = GetNewBatchno(cnCostId, dtpTranDate.Value.ToString("yyyy-MM-dd"), tran)
                Dim ACCODE As String = ""
                Dim CONTRA As String = ""
                Dim ENTRYORDER As Integer = 0
                'CREDITCARDACCOUNT
                ACCODE = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbBank_MAN.Text.ToString & "'", , , tran)
                CONTRA = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbchargesAC_OWN.Text.ToString & "'", , , tran)
                Dim Trandate As String = CType(ro!TRANDATE.ToString, Date).ToString
                If Val(IIf(IsDBNull(ro!BANKACCOUNT.ToString), 0, ro!BANKACCOUNT.ToString)) <> 0 Then ENTRYORDER += 1

                InsertIntoAccTran(ENTRYORDER, TranNo, ro!TRANDATE.ToString, "C", ACCODE, Val(IIf(IsDBNull(ro!BANKACCOUNT.ToString), 0, Math.Round(Val(ro!BANKACCOUNT.ToString), 2))) _
                , 0, 0, 0, cmbGenerateVoucher_OWN.SelectedValue.ToString, CONTRA, 0, 0, 0, IIf(IsDBNull(ro!COSTID.ToString), "" _
                , ro!COSTID.ToString), Nothing, Nothing, , , , , IIf(IsDBNull(ro!REMARK.ToString), "", ro!REMARK.ToString), IIf(IsDBNull(ro!REMARK.ToString), "", ro!REMARK.ToString), , _
                , IIf(IsDBNull(ro!COSTID.ToString), "", ro!COSTID.ToString))

                CONTRA = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbschargesAC_OWN.Text.ToString & "'", , , tran)
                InsertIntoAccTran(ENTRYORDER, TranNo, ro!TRANDATE.ToString, "C", ACCODE, Val(IIf(IsDBNull(ro!CREDITCARDCHARGESACCOUNT.ToString), 0, Math.Round(Val(ro!CREDITCARDCHARGESACCOUNT.ToString), 2))) _
                , 0, 0, 0, cmbGenerateVoucher_OWN.SelectedValue.ToString, CONTRA, 0, 0, 0, IIf(IsDBNull(ro!COSTID.ToString), "" _
                , ro!COSTID.ToString), Nothing, Nothing, , , , , IIf(IsDBNull(ro!REMARK.ToString), "", ro!REMARK.ToString), IIf(IsDBNull(ro!REMARK.ToString), "", ro!REMARK.ToString), , _
                , IIf(IsDBNull(ro!COSTID.ToString), "", ro!COSTID.ToString))

                CONTRA = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbstaxac_OWN.Text.ToString & "'", , , tran)
                InsertIntoAccTran(ENTRYORDER, TranNo, ro!TRANDATE.ToString, "C", ACCODE, Val(IIf(IsDBNull(ro!CREDITCARDSERVICETAXACCOUNT.ToString), 0, Math.Round(Val(ro!CREDITCARDSERVICETAXACCOUNT.ToString), 2))) _
                , 0, 0, 0, cmbGenerateVoucher_OWN.SelectedValue.ToString, CONTRA, 0, 0, 0, IIf(IsDBNull(ro!COSTID.ToString), "" _
                , ro!COSTID.ToString), Nothing, Nothing, , , , , IIf(IsDBNull(ro!REMARK.ToString), "", ro!REMARK.ToString), IIf(IsDBNull(ro!REMARK.ToString), "", ro!REMARK.ToString), , _
                , IIf(IsDBNull(ro!COSTID.ToString), "", ro!COSTID.ToString))

                'BANKACCOUNT
                ACCODE = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbchargesAC_OWN.Text.ToString & "'", , , tran)
                CONTRA = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbBank_MAN.Text.ToString & "'", , , tran)
                If Val(IIf(IsDBNull(ro!BANKACCOUNT.ToString), 0, ro!BANKACCOUNT.ToString)) <> 0 Then ENTRYORDER += 1
                InsertIntoAccTran(ENTRYORDER, TranNo, ro!TRANDATE.ToString, "D", ACCODE, Val(IIf(IsDBNull(ro!BANKACCOUNT.ToString), 0, Math.Round(Val(ro!BANKACCOUNT.ToString), 2))) _
                , 0, 0, 0, cmbGenerateVoucher_OWN.SelectedValue.ToString, CONTRA, 0, 0, 0, IIf(IsDBNull(ro!COSTID.ToString), "" _
                , ro!COSTID.ToString), Nothing, Nothing, , , , , IIf(IsDBNull(ro!REMARK.ToString), "", ro!REMARK.ToString), , , _
                , IIf(IsDBNull(ro!COSTID.ToString), "", ro!COSTID.ToString))

                'CREDITCARDCHARGESACCOUNT
                ACCODE = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbschargesAC_OWN.Text.ToString & "'", , , tran)
                If Val(IIf(IsDBNull(ro!CREDITCARDCHARGESACCOUNT.ToString), 0, ro!CREDITCARDCHARGESACCOUNT.ToString)) <> 0 Then ENTRYORDER += 1
                InsertIntoAccTran(ENTRYORDER, TranNo, ro!TRANDATE.ToString, "D", ACCODE, Val(IIf(IsDBNull(ro!CREDITCARDCHARGESACCOUNT.ToString), 0, Math.Round(Val(ro!CREDITCARDCHARGESACCOUNT.ToString), 2))) _
                , 0, 0, 0, cmbGenerateVoucher_OWN.SelectedValue.ToString, CONTRA, 0, 0, 0, IIf(IsDBNull(ro!COSTID.ToString), "" _
                , ro!COSTID.ToString), Nothing, Nothing, , , , , IIf(IsDBNull(ro!REMARK.ToString), "", ro!REMARK.ToString), , , _
                , IIf(IsDBNull(ro!COSTID.ToString), "", ro!COSTID.ToString))

                'CREDITCARDSERVICETAXACCOUNT
                ACCODE = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbstaxac_OWN.Text.ToString & "'", , , tran)
                If Val(IIf(IsDBNull(ro!CREDITCARDSERVICETAXACCOUNT.ToString), 0, ro!CREDITCARDSERVICETAXACCOUNT.ToString)) <> 0 Then ENTRYORDER += 1
                InsertIntoAccTran(ENTRYORDER, TranNo, ro!TRANDATE.ToString, "D", ACCODE, Val(IIf(IsDBNull(ro!CREDITCARDSERVICETAXACCOUNT.ToString), 0, Math.Round(Val(ro!CREDITCARDSERVICETAXACCOUNT.ToString), 2))) _
                , 0, 0, 0, cmbGenerateVoucher_OWN.SelectedValue.ToString, CONTRA, 0, 0, 0, IIf(IsDBNull(ro!COSTID.ToString), "" _
                , ro!COSTID.ToString), Nothing, Nothing, , , , , IIf(IsDBNull(ro!REMARK.ToString), "", ro!REMARK.ToString), , , _
                , IIf(IsDBNull(ro!COSTID.ToString), "", ro!COSTID.ToString))

            Next
            tran.Commit()
            tran = Nothing
            MsgBox("Downloaded Sucessfully..")
            btnNew_Click(Me, New EventArgs)
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            If ex.Message.Contains("to type 'Date' is not valid.") Then MsgBox("Invalid Date Format," & vbCrLf & " Format should be in mm/dd/yyyy", MsgBoxStyle.Information) Else MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        Finally
            If Not tran Is Nothing Then tran.Dispose()
        End Try
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If txtPath.Text Is Nothing Or txtPath.Text = "" Then MsgBox("Select Excel File Path", MsgBoxStyle.Information) : txtPath.Focus() : Exit Sub
        getExcelData()
        If Dt.Rows.Count > 0 Then
            For i As Integer = 0 To Dt.Rows.Count - 1
                If Dt.Rows(i).Item("COSTID").ToString.Length > 2 Then
                    gridView_OWN.DataSource = Nothing
                    txtPath.Text = ""
                    txtPath.Focus()
                    MsgBox("Costid length should not be greaterthen two.", MsgBoxStyle.Information)
                    Exit Sub
                End If
            Next
            gridView_OWN.DataSource = Dt

            gridView_OWN.Columns("CREDITCARDACCOUNT").DefaultCellStyle.Format = "#0.00"
            gridView_OWN.Columns("BANKACCOUNT").DefaultCellStyle.Format = "#0.00"
            gridView_OWN.Columns("CREDITCARDCHARGESACCOUNT").DefaultCellStyle.Format = "#0.00"
            gridView_OWN.Columns("CREDITCARDSERVICETAXACCOUNT").DefaultCellStyle.Format = "#0.00"

            gridView_OWN.Columns("CREDITCARDACCOUNT").Width = 150
            gridView_OWN.Columns("BANKACCOUNT").Width = 120
            gridView_OWN.Columns("CREDITCARDCHARGESACCOUNT").Width = 180
            gridView_OWN.Columns("CREDITCARDSERVICETAXACCOUNT").Width = 180

            gridView_OWN.Columns("CREDITCARDACCOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            gridView_OWN.Columns("BANKACCOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            gridView_OWN.Columns("CREDITCARDCHARGESACCOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            gridView_OWN.Columns("CREDITCARDSERVICETAXACCOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            gridView_OWN.Columns("CREDITCARDACCOUNT").HeaderText = "CreditCard Account"
            gridView_OWN.Columns("BANKACCOUNT").HeaderText = "Bank Account"
            gridView_OWN.Columns("CREDITCARDCHARGESACCOUNT").HeaderText = "CreditCard Charges Account"
            gridView_OWN.Columns("CREDITCARDSERVICETAXACCOUNT").HeaderText = "CreditCard ServiceTax Account"
        Else
            gridView_OWN.DataSource = Nothing
            MsgBox("Record not found.", MsgBoxStyle.Information)
        End If

    End Sub

    Public Sub InsertIntoAccTran _
    (ByVal EntryOrder As Integer, ByVal tNo As Integer, _
    ByVal trandate As String, _
    ByVal tranMode As String, _
    ByVal accode As String, _
    ByVal amount As Double, _
    ByVal pcs As Integer, _
    ByVal grsWT As Double, _
    ByVal netWT As Double, _
    ByVal payMode As String, _
    ByVal contra As String, _
    ByVal TDSCATID As Integer, _
    ByVal TDSPER As Decimal, _
    ByVal TDSAMOUNT As Decimal, _
    ByVal costid As String, _
    Optional ByVal refNo As String = Nothing, Optional ByVal refDate As String = Nothing, _
    Optional ByVal chqCardNo As String = Nothing, _
    Optional ByVal chqDate As String = Nothing, _
    Optional ByVal chqCardId As Integer = Nothing, _
    Optional ByVal chqCardRef As String = Nothing, _
    Optional ByVal Remark1 As String = Nothing, _
    Optional ByVal Remark2 As String = Nothing, _
    Optional ByVal fLAG As String = Nothing, _
    Optional ByVal SAccode As String = "", _
    Optional ByVal SCostid As String = "" _
    )
        If amount = 0 Then Exit Sub
        strSql = " INSERT INTO " & cnStockDb & "..ACCTRAN"
        strSql += " ("
        strSql += " SNO,TRANNO,TRANDATE,TRANMODE,ACCODE,SACCODE"
        strSql += " ,AMOUNT,PCS,GRSWT,NETWT,REFNO,REFDATE,PAYMODE,CHQCARDNO"
        strSql += " ,CARDID,CHQCARDREF,CHQDATE,BRSFLAG,RELIASEDATE,FROMFLAG,REMARK1,REMARK2"
        strSql += " ,CONTRA,BATCHNO,USERID,UPDATED,UPTIME,SYSTEMID,CASHID,COSTID,SCOSTID"
        strSql += " ,APPVER,COMPANYID,WT_ENTORDER,TDSCATID,TDSPER,TDSAMOUNT,FLAG"
        strSql += " )"
        strSql += " VALUES"
        strSql += " ("
        strSql += " '" & GetNewSno(TranSnoType.ACCTRANCODE, tran) & "'" ''SNO
        strSql += " ," & tNo & "" 'TRANNO 
        strSql += " ,'" & trandate.ToString & "'" 'TRANDATE
        strSql += " ,'" & tranMode & "'" 'TRANMODE
        strSql += " ,'" & accode & "'" 'ACCODE
        strSql += " ,'" & SAccode & "'" 'ACCODE
        strSql += " ," & Math.Abs(amount) & "" 'AMOUNT
        strSql += " ," & Math.Abs(pcs) & "" 'PCS
        strSql += " ," & Math.Abs(grsWT) & "" 'GRSWT
        strSql += " ," & Math.Abs(netWT) & "" 'NETWT
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
        strSql += " ,'" & BatchNo & "'" 'BATCHNO
        strSql += " ,'" & userId & "'" 'USERID
        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
        strSql += " ,'" & systemId & "'" 'SYSTEMID
        strSql += " ,''" 'CASHID
        strSql += " ,'" & costid & "'" 'COSTID
        strSql += " ,'" & SCostid & "'" 'FLAG
        strSql += " ,'" & VERSION & "'" 'APPVER
        strSql += " ,'" & strCompanyId & "'" 'COMPANYID
        strSql += " ," & EntryOrder & "" 'WT_ENTORDER
        strSql += " ," & TDSCATID & "" 'TDSCATID
        strSql += " ," & TDSPER & "" 'TDSPER
        strSql += " ," & TDSAMOUNT & "" 'TDSAMOUNT
        strSql += " ,'" & fLAG & "'" 'FLAG
        strSql += " )"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, costid)
        strSql = ""
        cmd = Nothing
    End Sub

    Private Sub btntemplate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btntemplate.Click
        Dim rwStart As Integer = 0
        Dim oXL As Excel.Application
        Dim oWB As Excel.Workbook
        Dim oSheet As Excel.Worksheet

        oXL = CreateObject("Excel.Application")
        oXL.Visible = True
        oWB = oXL.Workbooks.Add
        oSheet = oWB.ActiveSheet
        oSheet.Range("A1").Value = "COSTID"
        oSheet.Range("A1").ColumnWidth = 30
        oSheet.Range("B1").Value = "CREDITCARD A/C_CR"
        oSheet.Range("B1").ColumnWidth = 50
        oSheet.Range("C1").Value = "BANK A/C_DR"
        oSheet.Range("C1").ColumnWidth = 20
        oSheet.Range("D1").Value = "CREDITCARDCHARGES A/C_DR"
        oSheet.Range("D1").ColumnWidth = 20
        oSheet.Range("E1").Value = "CREDITCARD SERVICETAX A/C_DR"
        oSheet.Range("E1").ColumnWidth = 30
        oSheet.Range("F1").Value = "REMARK"
        oSheet.Range("F1").ColumnWidth = 30
        oSheet.Range("A1:B1:C1:D1:E1:F1").Font.Bold = True
        oSheet.Range("A1:B1:C1:D1:E1:F1").Font.Name = "VERDANA"
        oSheet.Range("A1:B1:C1:D1:E1:F1").Font.Size = 8
        oSheet.Range("A1:B1:C1:D1:E1:F1").HorizontalAlignment = Excel.Constants.xlCenter
    End Sub
End Class