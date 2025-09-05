Imports System.Data.OleDb
Public Class WSmithIssRec
    Dim StrSql As String
    Dim Cmd As OleDbCommand
    'Dim Tran As OleDbTransaction
    Dim Da As OleDbDataAdapter
    Dim dtGrid As New DataTable
    Dim dtStoneDetails As New DataTable
    Dim objStone As New frmStoneDia
    Dim objBillCollection As New BillCollection
    Dim objPurchaseDetail As BillPurchaseDetail
    Dim saStone As Boolean
    Dim catType As String
    Dim _Accode As String
    Dim TranTypeCol As New List(Of String)
    Dim TranNo As Integer
    Dim Batchno As String
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Initializer()
    End Sub
    Private Sub Initializer()
        With dtGrid
            .Columns.Add("CATNAME", GetType(String))
            .Columns.Add("KAFTWT", GetType(Decimal))
            .Columns.Add("KAFTTOUCH", GetType(Decimal))
            .Columns.Add("PCS", GetType(Integer))
            .Columns.Add("GRSWT", GetType(Decimal))
            .Columns.Add("STNWT", GetType(Decimal))
            .Columns.Add("NETWT", GetType(Decimal))
            .Columns.Add("TOUCH", GetType(Decimal))
            .Columns.Add("PUREWT", GetType(Decimal))
            .Columns.Add("MC", GetType(Double))
            .Columns.Add("TOTALAMT", GetType(Double))

            Dim col As New DataColumn("KEYNO")
            col.AutoIncrement = True
            col.AutoIncrementSeed = 1
            col.AutoIncrementStep = 1
            .Columns.Add(col)
            .Columns.Add("DUSTWT", GetType(Decimal))
            .Columns.Add("WASTAGEPER", GetType(Double))
            .Columns.Add("WASTAGE", GetType(Decimal))
            .Columns.Add("RATE", GetType(Double))
            .Columns.Add("PURITY", GetType(Double))
            .Columns.Add("LESSWT", GetType(Decimal))
            .Columns.Add("TRANTYPE", GetType(String))
            .Columns.Add("FLAG", GetType(String))
            .Columns.Add("REMARK1", GetType(String))
            .Columns.Add("REMARK2", GetType(String))
            .Columns.Add("BOARDRATE", GetType(Double))
            .Columns.Add("STONEAMT", GetType(Double))
            .Columns.Add("GRSNET", GetType(String))
            .Columns.Add("ITEMID", GetType(Integer))
            .Columns.Add("SUBITEMID", GetType(Integer))
            .Columns.Add("CATCODE", GetType(String))
            .Columns.Add("ITEMTYPE", GetType(String))
        End With
        dtGrid.Columns("GRSNET").DefaultValue = "NET WT"
        dtGrid.AcceptChanges()
        gridPur.DataSource = dtGrid
        gridPur.ColumnHeadersVisible = False
        FormatGridColumns(gridPur)
        ClearDtGrid(dtGrid)
        StyleGridPur(gridPur)

        Dim dtgridPurTotal As New DataTable
        dtgridPurTotal = dtGrid.Copy
        dtgridPurTotal.Rows.Clear()
        dtgridPurTotal.Rows.Add()
        dtgridPurTotal.Rows.Add()
        gridPurTotal.ColumnHeadersVisible = False
        gridPurTotal.DataSource = dtgridPurTotal
        For Each col As DataGridViewColumn In gridPur.Columns
            With gridPurTotal.Columns(col.Name)
                .Visible = col.Visible
                .Width = col.Width
                .DefaultCellStyle = col.DefaultCellStyle
            End With
        Next
        CalcGridPurTotal()
        StyleGridPur(gridPurTotal)

        ''Stone
        With dtStoneDetails.Columns
            .Add("KEYNO", GetType(Integer))
            .Add("TRANTYPE", GetType(String))
            .Add("ITEM", GetType(String))
            .Add("SUBITEM", GetType(String))
            .Add("PCS", GetType(Int32))
            .Add("WEIGHT", GetType(Decimal))
            .Add("UNIT", GetType(String))
            .Add("CALC", GetType(String))
            .Add("RATE", GetType(Double))
            .Add("AMOUNT", GetType(Double))
            .Add("METALID", GetType(String))
            .Add("DISCOUNT", GetType(Double))
            .Add("TAGSTNPCS", GetType(Integer))
            .Add("TAGSTNWT", GetType(Decimal))
            .Add("TAGSNO", GetType(String))
            .Add("R_VAT", GetType(Decimal))
            .Add("ISSSNO", GetType(String))
        End With
    End Sub

    Private Sub CalcGridPurTotal(Optional ByVal vbc As Boolean = False)
        Dim puGrsWt As Decimal = Nothing
        Dim puDustWt As Decimal = Nothing
        Dim puStnWt As Decimal = Nothing
        Dim puWastage As Decimal = Nothing
        Dim puNetWt As Decimal = Nothing
        Dim puAmt As Double = Nothing
        Dim puVat As Double = Nothing
        Dim puTotAmt As Decimal = Nothing
        Dim puPureWt As Decimal = Nothing
        Dim puMc As Decimal = Nothing
        'For i As Integer = 0 To gridPur.RowCount - 1
        '    With gridPur.Rows(i)
        '        If .Cells("EntFlag").Value.ToString = "" Then Continue For
        '        If vbc = True Then
        '            .Cells("VAT").Value = DBNull.Value
        '        Else
        '            .Cells("VAT").Value = .Cells("TVAT").Value
        '        End If
        '        .Cells("AMOUNT").Value = IIf(Val(.Cells("GROSSAMT").Value.ToString) + Val(.Cells("VAT").Value.ToString) <> 0, Val(.Cells("GROSSAMT").Value.ToString) + Val(.Cells("VAT").Value.ToString), DBNull.Value)
        '    End With
        'Next
        For i As Integer = 0 To gridPur.RowCount - 1
            With gridPur.Rows(i)
                If .Cells("TRANTYPE").Value.ToString <> "" Then
                    puGrsWt += Val(.Cells("GRSWT").Value.ToString)
                    puDustWt += Val(.Cells("DUSTWT").Value.ToString)
                    puStnWt += Val(.Cells("LESSWT").Value.ToString)
                    puWastage += Val(.Cells("WASTAGE").Value.ToString)
                    puNetWt += Val(.Cells("NETWT").Value.ToString)
                    'puVat += Val(.Cells("VAT").Value.ToString)
                    puPureWt += Val(.Cells("PUREWT").Value.ToString)
                    puTotAmt += Val(.Cells("TOTALAMT").Value.ToString)
                    puMc += Val(.Cells("MC").Value.ToString)
                End If
            End With
        Next
        gridPurTotal.DefaultCellStyle.BackColor = grpHeader.BackgroundColor
        gridPurTotal.DefaultCellStyle.SelectionBackColor = grpHeader.BackgroundColor

        gridPurTotal.Rows(0).Cells("CATNAME").Value = "PURCHASE TOT"
        gridPurTotal.Rows(0).Cells("GRSWT").Value = IIf(puGrsWt <> 0, puGrsWt, DBNull.Value)
        gridPurTotal.Rows(0).Cells("DUSTWT").Value = IIf(puDustWt <> 0, puDustWt, DBNull.Value)
        gridPurTotal.Rows(0).Cells("LESSWT").Value = IIf(puStnWt <> 0, puStnWt, DBNull.Value)
        gridPurTotal.Rows(0).Cells("WASTAGE").Value = IIf(puWastage <> 0, puWastage, DBNull.Value)
        gridPurTotal.Rows(0).Cells("NETWT").Value = IIf(puNetWt <> 0, puNetWt, DBNull.Value)
        'gridPurTotal.Rows(0).Cells("AMOUNT").Value = IIf(puAmt <> 0, puAmt, DBNull.Value)
        'gridPurTotal.Rows(0).Cells("VAT").Value = IIf(puVat <> 0, puVat, DBNull.Value)
        gridPurTotal.Rows(0).Cells("TOTALAMT").Value = IIf(puTotAmt <> 0, puTotAmt, DBNull.Value)
        gridPurTotal.Rows(0).Cells("PUREWT").Value = IIf(puPureWt <> 0, puPureWt, DBNull.Value)
        gridPurTotal.Rows(0).Cells("MC").Value = IIf(puMc <> 0, puMc, DBNull.Value)
        gridPurTotal.Rows(0).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        gridPurTotal.Rows(0).DefaultCellStyle.ForeColor = Color.Red
        gridPurTotal.Rows(0).DefaultCellStyle.SelectionForeColor = Color.Red
        CheckTransaction()
    End Sub

    Private Sub StyleGridPur(ByVal gridPurView As DataGridView)
        With gridPurView
            .Columns("CATNAME").Width = txtPUCategory.Width + 1
            .Columns("KAFTWT").Width = txtPuKatchaWt_WET.Width + 1
            .Columns("KAFTTOUCH").Width = txtPurKaTouch_AMT.Width + 1
            .Columns("PCS").Width = txtPUPcs_NUM.Width + 1
            .Columns("GRSWT").Width = txtPUGrsWt_WET.Width + 1
            .Columns("STNWT").Width = txtPUStoneWt_WET.Width + 1
            .Columns("NETWT").Width = txtPUNetWt_WET.Width + 1
            .Columns("TOUCH").Width = txtPuTouch_AMT.Width + 1
            .Columns("PUREWT").Width = txtPuPureWt_WET.Width + 1
            .Columns("MC").Width = txtPuMc_AMT.Width + 1
            .Columns("TOTALAMT").Width = txtPuTotalAmt_AMT.Width
            For i As Integer = 11 To .Columns.Count - 1
                .Columns(i).Visible = False
            Next
        End With
    End Sub

    Private Sub ClearDtGrid(ByVal dt As DataTable) ''Only For GridSASR and GridPur 
        dt.Columns("KeyNo").AutoIncrementSeed = 1
        dt.Rows.Clear()
        For i As Integer = 1 To 10
            dt.Rows.Add()
        Next
        dt.AcceptChanges()
    End Sub
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        objGPack.TextClear(Me)
        TranTypeCol.Clear()
        ClearDtGrid(dtGrid)
        CalcGridPurTotal()
        lblBalance.Text = ""
        objBillCollection = New BillCollection
        objPurchaseDetail = New BillPurchaseDetail(True)
        objPurchaseDetail.rbtOthers.Enabled = False
        objPurchaseDetail.rbtOwn.Enabled = False
        objStone = New frmStoneDia
        saStone = False
        catType = Nothing
        If cmbTranType.Text = "" Then
            cmbTranType.Text = WholeSaleBill_ShortEntry.EntryMode.Issue.ToString
        End If
        If dtpTranDate.Text = "" Then
            dtpTranDate.Value = GetEntryDate(GetServerDate)
        End If
        TranNo = Nothing
        Batchno = Nothing
        dtpTranDate.Select()
    End Sub

    Private Sub WSmithIssRec_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtPUCategory.Focused Then Exit Sub
            If cmbParty_MAN.Focused Then Exit Sub
            If txtPUGrsWt_WET.Focused Then Exit Sub
            If txtPuTotalAmt_AMT.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub WSmithIssRec_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        gridPur.DefaultCellStyle.Font = txtPUCategory.Font
        gridPurTotal.DefaultCellStyle.Font = txtPUCategory.Font
        Me.BackColor = SystemColors.InactiveCaption
        StrSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACTYPE IN ('G','D','I')"
        objGPack.FillCombo(StrSql, cmbParty_MAN)
        cmbTranType.Items.Add(WholeSaleBill_ShortEntry.EntryMode.Issue.ToString)
        cmbTranType.Items.Add(WholeSaleBill_ShortEntry.EntryMode.Receipt.ToString)
        cmbTranType.Items.Add(WholeSaleBill_ShortEntry.EntryMode.Approval_Issue.ToString)
        cmbTranType.Items.Add(WholeSaleBill_ShortEntry.EntryMode.Approval_Receipt.ToString)
        btnNew_Click(Me, New EventArgs)
    End Sub
    Private Sub ShowStoneDia()
        If objStone.Visible Then Exit Sub
        objStone.grsWt = Val(txtPUGrsWt_WET.Text)
        objStone.BackColor = Me.BackColor
        objStone.StartPosition = FormStartPosition.CenterScreen
        objStone.MaximizeBox = False
        objStone.grpStone.BackgroundColor = grpHeader.BackgroundColor
        objStone.StyleGridStone(objStone.gridStone)
        objStone.txtStItem.Select()
        objStone.ShowDialog()
        Dim stnWt As Double = Val(objStone.gridStoneTotal.Rows(0).Cells("WEIGHT").Value.ToString)
        Dim stnAmt As Double = Val(objStone.gridStoneTotal.Rows(0).Cells("AMOUNT").Value.ToString)
        txtPUStoneWt_WET.Text = IIf(stnWt <> 0, Format(stnWt, "0.000"), Nothing)
        Total()
        Me.SelectNextControl(txtPUGrsWt_WET, True, True, True, True)
    End Sub
    Private Sub txtPuTotalAmt_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPuTotalAmt_AMT.KeyPress
        e.Handled = True
        If e.KeyChar = Chr(Keys.Enter) Then
            If Val(objStone.gridStoneTotal.Rows(0).Cells("WEIGHT").Value.ToString) <> Val(txtPUStoneWt_WET.Text) Then
                ShowStoneDia()
                Exit Sub
            End If
            If txtPUCategory.Text = "" Then
                MsgBox(E0004 + Me.GetNextControl(txtPUCategory, False).Text, MsgBoxStyle.Information)
                txtPUCategory.Select()
                Exit Sub
            End If
            If Val(txtPUGrsWt_WET.Text) = 0 Then
                MsgBox(Me.GetNextControl(txtPUGrsWt_WET, False).Text + E0001, MsgBoxStyle.Information)
                txtPUGrsWt_WET.Select()
                Exit Sub
            End If
            If Val(txtPuPureWt_WET.Text) = 0 Then
                MsgBox("PureWt Should not Empty", MsgBoxStyle.Information)
                Exit Sub
            End If
            dtGrid.AcceptChanges()
            Dim tranType As String = Nothing
            Select Case cmbTranType.Text
                Case WholeSaleBill_ShortEntry.EntryMode.Issue.ToString
                    tranType = "IIS"
                Case WholeSaleBill_ShortEntry.EntryMode.Receipt.ToString
                    tranType = "RRE"
                Case WholeSaleBill_ShortEntry.EntryMode.Approval_Issue.ToString
                    tranType = "IAP"
                Case WholeSaleBill_ShortEntry.EntryMode.Approval_Receipt.ToString
                    tranType = "RAP"
                Case Else
                    Exit Sub
            End Select
            Dim index As Integer = 0
            Dim Row As DataRow = Nothing
            If txtPURowIndex.Text = "" Then
                For i As Integer = 0 To dtGrid.Rows.Count - 1
                    With dtGrid.Rows(i)
                        If .Item("TRANTYPE").ToString = "" Then
                            Row = dtGrid.Rows(i)
                            index = i
                            gridPur.CurrentCell = gridPur.Rows(i).Cells("CATNAME")
                            dtGrid.Rows.Add()
                            Exit For
                        End If
                    End With
                Next
            Else
                With dtGrid.Rows(Val(txtPURowIndex.Text))
                    Row = dtGrid.Rows(Val(txtPURowIndex.Text))
                    dtStoneDetails.AcceptChanges()
                    For Each ro As DataRow In dtStoneDetails.Rows
                        If ro!KEYNO.ToString = .Item("KEYNO").ToString And ro!TRANTYPE.ToString = tranType Then
                            ro.Delete()
                        End If
                    Next
                    dtStoneDetails.AcceptChanges()
                    index = Val(txtPURowIndex.Text)
                End With
            End If
            Row.Item("CATNAME") = txtPUCategory.Text
            Row.Item("PCS") = IIf(Val(txtPUPcs_NUM.Text) > 0, txtPUPcs_NUM.Text, DBNull.Value)
            Row.Item("GRSWT") = IIf(Val(txtPUGrsWt_WET.Text) > 0, txtPUGrsWt_WET.Text, DBNull.Value)
            Row.Item("LESSWT") = IIf(Val(txtPUStoneWt_WET.Text) > 0, txtPUStoneWt_WET.Text, DBNull.Value)
            Row.Item("NETWT") = IIf(Val(txtPUNetWt_WET.Text) > 0, txtPUNetWt_WET.Text, DBNull.Value)
            Row.Item("MC") = IIf(Val(txtPuMc_AMT.Text) <> 0, Format(Val(txtPuMc_AMT.Text), "0.00"), DBNull.Value)
            Row.Item("TRANTYPE") = tranType
            Row.Item("REMARK1") = objBillCollection.txtRemark.Text
            Row.Item("REMARK2") = objPurchaseDetail.txtDescription.Text
            Row.Item("BOARDRATE") = Val(GetRate(dtpTranDate.Value.ToString("yyyy-MM-dd"), objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & txtPUCategory.Text & "'", , , Tran)))
            Row.Item("PUREWT") = Val(txtPuPureWt_WET.Text)
            Row.Item("TOUCH") = Val(txtPuTouch_AMT.Text)
            Row.Item("KAFTWT") = IIf(Val(txtPuKatchaWt_WET.Text) <> 0, Format(Val(txtPuKatchaWt_WET.Text), "0.000"), DBNull.Value)
            Row.Item("KAFTTOUCH") = IIf(Val(txtPurKaTouch_AMT.Text) <> 0, Format(Val(txtPurKaTouch_AMT.Text), "0.00"), DBNull.Value)
            Row.Item("TOTALAMT") = IIf(Val(txtPuTotalAmt_AMT.Text) <> 0, Format(Val(txtPuTotalAmt_AMT.Text), "0.000"), DBNull.Value)
            Row.Item("ITEMID") = Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & objPurchaseDetail.cmbItem.Text & "'"))
            Row.Item("SUBITEMID") = Val(objGPack.GetSqlValue("SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & objPurchaseDetail.cmbSubItem_Own.Text & "'"))
            Row.Item("CATCODE") = objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & objPurchaseDetail.cmbCategory_MAN.Text & "'")
            Row.Item("REMARK1") = objPurchaseDetail.txtDescription.Text
            Row.Item("ITEMTYPE") = objPurchaseDetail.cmbItemType_MAN.Text
            dtGrid.AcceptChanges()
            CalcGridPurTotal()

            ''Stone
            objStone.dtGridStone.AcceptChanges()
            For rwIndex As Integer = 0 To objStone.dtGridStone.Rows.Count - 1
                Dim ro As DataRow = dtStoneDetails.NewRow
                ro("KEYNO") = dtGrid.Rows(index).Item("KEYNO").ToString
                ro("TRANTYPE") = tranType
                For colIndex As Integer = 2 To objStone.dtGridStone.Columns.Count - 1
                    ro(colIndex) = objStone.dtGridStone.Rows(rwIndex).Item(colIndex)
                Next
                dtStoneDetails.Rows.Add(ro)
            Next
            dtStoneDetails.AcceptChanges()

            ''Clear
            ''Stone Clear
            objStone.dtGridStone.Clear()
            objStone.CalcStoneWtAmount()
            txtPUStoneWt_WET.Clear()
            objGPack.TextClear(grpPu)
            objPurchaseDetail = New BillPurchaseDetail
            objStone = New frmStoneDia
            saStone = False
            catType = Nothing
            txtPUCategory.Select()
            CalcGridPurTotal()
        End If
    End Sub
    Private Sub txtPUCategory_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPUCategory.GotFocus
        Main.ShowHelpText("Press Insert Key to Help")
    End Sub
    Private Sub txtPUCategory_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtPUCategory.KeyDown
        If e.KeyCode = Keys.Insert Then
            LoadPurCatName()
        ElseIf e.KeyCode = Keys.Down Then
            gridPur.Focus()
        ElseIf e.KeyCode = Keys.Escape Then
            Dim dueAmt As Double = Nothing
            Dim dueWt As Double = Nothing
            dueAmt += Val(gridPurTotal.Rows(0).Cells("TOTALAMT").Value.ToString)
            dueWt += Val(gridPurTotal.Rows(0).Cells("PUREWT").Value.ToString)
            If TranTypeCol.Contains("RRE") Or TranTypeCol.Contains("RAP") Then
                dueAmt *= -1
                dueWt *= -1
            End If
            objBillCollection.txtDueAmount_AMT.Text = IIf(dueAmt <> 0, Format(dueAmt, "0.00"), "")
            objBillCollection.txtDueWeight_WET.Text = IIf(dueWt <> 0, Format(dueWt, "0.000"), "")
            objBillCollection.CalcBalWtAmt()
            If objBillCollection.ShowDialog() = Windows.Forms.DialogResult.OK Then
                Save()
            End If
        End If
    End Sub
    Public Sub InsertDetails(ByVal index As Integer)
        With gridPur.Rows(index)
            Dim TNO As Integer = TranNo
            Dim issSno As String = Nothing
            Dim runNo As String = Nothing

            Dim catCode As String = Nothing
            catCode = objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & Val(.Cells("ITEMID").Value.ToString) & "", , , Tran)
            Select Case .Cells("TRANTYPE").Value.ToString
                Case "IIS", "IAP"
                    issSno = GetNewSno(TranSnoType.ISSUECODE, Tran)
                Case "RRE", "RAP"
                    issSno = GetNewSno(TranSnoType.RECEIPTCODE, Tran)
            End Select
            Dim subItemId As Integer = Val(.Cells("SUBITEMID").Value.ToString)
            Dim tagSaleMode As String = objGPack.GetSqlValue("SELECT CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & Val(.Cells("ITEMID").Value.ToString) & "", , , Tran)
            Dim tagGrsNet As String = "N"
            Dim tagPcs As Double = Nothing
            Dim tagGrsWt As Double = Nothing
            Dim tagNetWt As Double = Nothing
            Dim tagRate As Double = Nothing
            Dim tagSaleValue As Double = Nothing
            Dim tagDesignerId As String = Nothing
            Dim tagItemCtrId As Integer = 0

            Dim tagItemTypeId As Integer = Val(objGPack.GetSqlValue("SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & .Cells("ITEMTYPE").Value.ToString & "'", , , tran))




            Dim Purity As Double = Val(objGPack.GetSqlValue(" SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = (SELECT PURITYID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = (SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & Val(.Cells("ITEMID").Value.ToString) & "))", , , tran))
            Dim tagTableCode As String = Nothing
INSERT_SASR:
            If .Cells("TRANTYPE").Value.ToString = "RRE" Or .Cells("TRANTYPE").Value.ToString = "RAP" Then
                StrSql = " INSERT INTO " & cnStockDb & "..RECEIPT"
            Else
                StrSql = " INSERT INTO " & cnStockDb & "..ISSUE"
            End If
            StrSql += vbCrLf + "  ("
            StrSql += vbCrLf + "  SNO,TRANNO,TRANDATE,TRANTYPE,PCS"
            StrSql += vbCrLf + "  ,GRSWT,NETWT,LESSWT,PUREWT"
            StrSql += vbCrLf + "  ,TAGNO,ITEMID,SUBITEMID,WASTPER"
            StrSql += vbCrLf + "  ,WASTAGE,MCGRM,MCHARGE,AMOUNT"
            StrSql += vbCrLf + "  ,RATE,BOARDRATE,SALEMODE,GRSNET"
            StrSql += vbCrLf + "  ,TRANSTATUS,REFNO,REFDATE,COSTID"
            StrSql += vbCrLf + "  ,COMPANYID,FLAG,EMPID,TAGPCS,TAGGRSWT"
            StrSql += vbCrLf + "  ,TAGNETWT,TAGRATEID,TAGSVALUE,TAGDESIGNER"
            StrSql += vbCrLf + "  ,ITEMCTRID,ITEMTYPEID,PURITY,TABLECODE"
            StrSql += vbCrLf + "  ,INCENTIVE,WEIGHTUNIT,CATCODE,OCATCODE"
            StrSql += vbCrLf + "  ,ACCODE,ALLOY,BATCHNO,REMARK1"
            StrSql += vbCrLf + "  ,REMARK2,USERID,UPDATED,UPTIME,SYSTEMID,DISCOUNT"
            StrSql += vbCrLf + "  ,RUNNO,CASHID,VATEXM,TAX,STNAMT,MISCAMT,METALID,STONEUNIT,APPVER"
            StrSql += vbCrLf + "  ,TOUCH"
            If .Cells("TRANTYPE").Value.ToString = "RRE" Or .Cells("TRANTYPE").Value.ToString = "RAP" Then
                StrSql += vbCrLf + "  ,ISSNO"
            Else
                StrSql += vbCrLf + "  ,RESNO"
            End If
            StrSql += vbCrLf + "  ,ESTSNO,ORSNO,TRANFLAG"
            If .Cells("TRANTYPE").Value.ToString = "RRE" Or .Cells("TRANTYPE").Value.ToString = "RAP" Then
                StrSql += vbCrLf + " ,PUREXCH,KAFTWT,KAFTTOUCH"
            End If
            StrSql += vbCrLf + " )"
            StrSql += vbCrLf + "  VALUES("
            StrSql += vbCrLf + "  '" & issSno & "'" ''SNO
            StrSql += vbCrLf + "  ," & TNO & "" 'TRANNO
            StrSql += vbCrLf + "  ,'" & dtpTranDate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE 'GetEntryDate(BillDate, tran).ToString("yyyy-MM-dd")
            StrSql += vbCrLf + "  ,'" & .Cells("TRANTYPE").Value.ToString & "'" 'TRANTYPE

            StrSql += vbCrLf + "  ," & Val(.Cells("PCS").Value.ToString) & "" 'PCS
            StrSql += vbCrLf + "  ," & Val(.Cells("GRSWT").Value.ToString) & "" 'GRSWT
            StrSql += vbCrLf + "  ," & Val(.Cells("NETWT").Value.ToString) & "" 'NETWT
            StrSql += vbCrLf + "  ," & Math.Abs(Val(.Cells("GRSWT").Value.ToString) - Val(.Cells("NETWT").Value.ToString)) & "" 'LESSWT
            StrSql += vbCrLf + "  ," & Val(.Cells("PUREWT").Value.ToString) & "" 'PUREWT '
            StrSql += vbCrLf + "  ,''" 'TAGNO
            StrSql += vbCrLf + "  ," & Val(.Cells("ITEMID").Value.ToString) & "" 'ITEMID
            StrSql += vbCrLf + "  ," & subItemId & "" 'SUBITEMID
            StrSql += vbCrLf + "  ," & Val(.Cells("WASTAGEPER").Value.ToString) & "" 'WASTPER
            StrSql += vbCrLf + "  ," & Val(.Cells("WASTAGE").Value.ToString) & "" 'WASTAGE
            StrSql += vbCrLf + "  ,0" 'MCGRM
            StrSql += vbCrLf + "  ," & Val(.Cells("MC").Value.ToString) & "" 'MCHARGE
            StrSql += vbCrLf + "  ,0" 'AMOUNT
            StrSql += vbCrLf + "  ,0" 'RATE
            StrSql += vbCrLf + "  ," & Val(.Cells("BOARDRATE").Value.ToString) & "" 'BOARDRATE
            StrSql += vbCrLf + "  ,'" & tagSaleMode & "'" 'SALEMODE
            StrSql += vbCrLf + "  ,'" & tagGrsNet & "'" 'GRSNET
            StrSql += vbCrLf + "  ,''" 'TRANSTATUS ''
            StrSql += vbCrLf + "  ,''" 'REFNO ''
            StrSql += vbCrLf + "  ,NULL" 'REFDATE NULL
            StrSql += vbCrLf + "  ,'" & cnCostId & "'" 'COSTID 
            StrSql += vbCrLf + "  ,'" & strCompanyId & "'" 'COMPANYID
            StrSql += vbCrLf + "  ,'" & .Cells("FLAG").Value.ToString & "'" 'FLAG 
            StrSql += vbCrLf + "  ," & Val(objBillCollection.txtEmpId_MAN.Text) & "" 'EMPID
            StrSql += vbCrLf + "  ," & tagPcs & "" 'TAGPCS
            StrSql += vbCrLf + "  ," & tagGrsWt & "" 'TAGGRSWT
            StrSql += vbCrLf + "  ," & tagNetWt & "" 'TAGNETWT
            StrSql += vbCrLf + "  ," & tagRate & "" 'TAGRATEID
            StrSql += vbCrLf + "  ," & tagSaleValue & "" 'TAGSVALUE
            StrSql += vbCrLf + "  ,'" & tagDesignerId & "'" 'TAGDESIGNER
            StrSql += vbCrLf + "  ," & tagItemCtrId & "" 'ITEMCTRID
            StrSql += vbCrLf + "  ," & tagItemTypeId & "" 'ITEMTYPEID
            StrSql += vbCrLf + "  ," & Purity & "" 'PURITY
            StrSql += vbCrLf + "  ,'" & tagTableCode & "'" 'TABLECODE
            StrSql += vbCrLf + "  ,''" 'INCENTIVE
            StrSql += vbCrLf + "  ,''" 'WEIGHTUNIT
            StrSql += vbCrLf + "  ,'" & .Cells("CATCODE").Value.ToString & "'" 'CATCODE
            StrSql += vbCrLf + "  ,''" 'OCATCODE
            StrSql += vbCrLf + "  ,'" & _Accode & "'" 'ACCODE
            StrSql += vbCrLf + "  ,0" 'ALLOY
            StrSql += vbCrLf + "  ,'" & Batchno & "'" 'BATCHNO
            StrSql += vbCrLf + "  ,'" & objBillCollection.txtRemark.Text & "'" 'REMARK1
            StrSql += vbCrLf + "  ,'" & .Cells("REMARK1").Value.ToString & "'" 'REMARK2
            StrSql += vbCrLf + "  ,'" & userId & "'" 'USERID
            StrSql += vbCrLf + "  ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
            StrSql += vbCrLf + "  ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
            StrSql += vbCrLf + "  ,'" & systemId & "'" 'SYSTEMID
            StrSql += vbCrLf + "  ,0" 'DISCOUNT
            StrSql += vbCrLf + "  ,''" 'RUNNO
            StrSql += vbCrLf + "  ,''" 'CASHID
            StrSql += vbCrLf + "  ,''" 'VATEXM
            StrSql += vbCrLf + "  ,0" 'TAX
            StrSql += vbCrLf + "  ,0" 'STONEAMT
            StrSql += vbCrLf + "  ,0" 'MISCAMT
            StrSql += vbCrLf + "  ,'" & objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE CATCODE = '" & .Cells("CATCODE").Value.ToString & "'", , , Tran) & "'" 'MTALID
            StrSql += vbCrLf + "  ,''" 'STONEUNIT
            StrSql += vbCrLf + "  ,'" & VERSION & "'" 'APPVER
            StrSql += vbCrLf + "  ," & Val(.Cells("TOUCH").Value.ToString) & "" 'TOUCH'
            StrSql += vbCrLf + "  ,''" 'RESNO OR ISSNO
            StrSql += vbCrLf + "  ,''" 'ORSNO
            StrSql += vbCrLf + "  ,''" 'ORSNO
            StrSql += vbCrLf + "  ,'" & IIf(Val(objBillCollection.txtCutRate_AMT.Text) <> 0, "A", "W") & "'" 'TRANFLAG
            If .Cells("TRANTYPE").Value.ToString = "RRE" Or .Cells("TRANTYPE").Value.ToString = "RAP" Then
                StrSql += vbCrLf + " ,'" & .Cells("FLAG").Value.ToString & "'" ' PUREXCH
                StrSql += vbCrLf + " ," & Val(.Cells("KAFTWT").Value.ToString) & "" ' KAFTWT
                StrSql += vbCrLf + " ," & Val(.Cells("KAFTTOUCH").Value.ToString) & "" 'KAFTTOUCH
            End If
            StrSql += vbCrLf + "  )"
            ExecQuery(SyncMode.Transaction, StrSql, cn, Tran, cnCostId)
            ''Stone
            For Each stRow As DataRow In dtStoneDetails.Rows
                If .Cells("KEYNO").Value = stRow("KEYNO") And _
                .Cells("TRANTYPE").Value.ToString = stRow("TRANTYPE").ToString Then
                    InsertStoneDetails(issSno, TNO, stRow)
                End If
            Next
        End With
    End Sub

    Private Sub InsertStoneDetails(ByVal IssSno As String _
    , ByVal TNO As Integer, ByVal stRow As DataRow)
        Dim stnItemId As Integer = 0
        Dim stnSubItemid As Integer = 0
        Dim stnCatCode As String = Nothing
        Dim vat As Double = Nothing
        Dim sno As String = Nothing
        If stRow.Item("TRANTYPE").ToString = "RRE" Or _
        stRow.Item("TRANTYPE").ToString = "RAP" Then
            sno = GetNewSno(TranSnoType.RECEIPTSTONECODE, tran)
        Else
            sno = GetNewSno(TranSnoType.ISSSTONECODE, tran) 'funcGetTrandbTableId("ISSSTONECODE", tran)
        End If

        ''Find stnCatCode
        StrSql = " Select CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & stRow.Item("ITEM").ToString & "'"
        stnCatCode = objGPack.GetSqlValue(StrSql, , , tran)

        ''Find itemId
        StrSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & stRow.Item("ITEM").ToString & "'"
        stnItemId = Val(objGPack.GetSqlValue(StrSql, , , tran))

        ''Find subItemId
        StrSql = " SELECT ISNULL(SUBITEMID,0)AS SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & stRow.Item("SUBITEM").ToString & "' AND ITEMID = " & stnItemId & ""
        stnSubItemid = Val(objGPack.GetSqlValue(StrSql, , , tran))

        Dim vatPer As Double = 0
        'If (objGPack.GetSqlValue("SELECT VBC FROM " & cnAdminDb & "..COMPANY WHERE COMPANYID = '" & strCompanyId & "' AND ISNULL(VBC,'') = 'Y'", , "N", tran)) = "N" Then
        If stRow.Item("TRANTYPE").ToString = "RRE" Or stRow.Item("TRANTYPE").ToString = "RAP" Then
            StrSql = " INSERT INTO " & cnStockDb & "..RECEIPTSTONE"
        Else
            StrSql = " INSERT INTO " & cnStockDb & "..ISSSTONE"
        End If
        StrSql += " ("
        StrSql += " SNO,ISSSNO,TRANNO,TRANDATE,TRANTYPE"
        StrSql += " ,STNPCS,STNWT,STNRATE,STNAMT"
        StrSql += " ,STNITEMID,STNSUBITEMID,CALCMODE,STONEUNIT"
        StrSql += " ,STONEMODE,TRANSTATUS,COSTID,COMPANYID"
        StrSql += " ,BATCHNO,SYSTEMID,VATEXM,CATCODE,TAX,APPVER,DISCOUNT,TAGSTNPCS,TAGSTNWT)"
        StrSql += " VALUES"
        StrSql += " ("
        StrSql += " '" & sno & "'" ''SNO
        StrSql += " ,'" & IssSno & "'" 'ISSSNO
        StrSql += " ," & TNO & "" 'TRANNO
        StrSql += " ,'" & dtpTranDate.Value.ToString("yyyy-MM-dd") & "'" 'TRANDATE
        StrSql += " ,'" & stRow.Item("TRANTYPE").ToString & "'" 'TRANTYPE 
        StrSql += " ," & Val(stRow.Item("PCS").ToString) & "" 'STNPCS
        StrSql += " ," & Val(stRow.Item("WEIGHT").ToString) & "" 'STNWT
        StrSql += " ," & Val(stRow.Item("RATE").ToString) & "" 'STNRATE
        StrSql += " ," & Val(stRow.Item("AMOUNT").ToString) & "" 'STNAMT
        StrSql += " ," & stnItemId & "" 'STNITEMID
        StrSql += " ," & stnSubItemid & "" 'STNSUBITEMID
        StrSql += " ,'" & Mid(stRow.Item("CALC").ToString, 1, 1) & "'" 'CALCMODE
        StrSql += " ,'" & Mid(stRow.Item("UNIT").ToString, 1, 1) & "'" 'STONEUNIT
        StrSql += " ,''" 'STONEMODE 
        StrSql += " ,''" 'TRANSTATUS
        StrSql += " ,'" & cnCostId & "'" 'COSTID
        StrSql += " ,'" & strCompanyId & "'" 'COMPANYID
        StrSql += " ,'" & Batchno & "'" 'BATCHNO
        StrSql += " ,'" & systemId & "'" 'SYSTEMID
        StrSql += " ,''" 'VATEXM
        StrSql += " ,'" & stnCatCode & "'" 'CATCODE
        StrSql += " ," & vat & "" 'TAX
        StrSql += " ,'" & VERSION & "'" 'APPVER
        StrSql += " ," & Val(stRow.Item("DISCOUNT").ToString) & "" 'DISCOUNT
        StrSql += " ," & Val(stRow.Item("TAGSTNPCS").ToString) & "" 'TAGSTNPCS
        StrSql += " ," & Val(stRow.Item("TAGSTNWT").ToString) & "" 'TAGSTNWT
        StrSql += " )"
        ExecQuery(SyncMode.Transaction, StrSql, cn, tran, cnCostId)
    End Sub

    Private Sub Save()
        Try
            Tran = Nothing
            Tran = cn.BeginTransaction
            TranNo = GetMaxTranNo(dtpTranDate.Value.ToString("yyyy-MM-dd"), Tran)
            Batchno = GetNewBatchno(cnCostId, dtpTranDate.Value.ToString("yyyy-MM-dd"), Tran)
            For cnt As Integer = 0 To gridPur.Rows.Count - 1
                If gridPur.Rows(cnt).Cells("TRANTYPE").Value.ToString = "" Then Continue For
                InsertDetails(cnt)
            Next
            If TranTypeCol.Contains("RAP") Or TranTypeCol.Contains("IAP") Then
                GoTo App
            End If
            objBillCollection.SaveCollection("SMI", cmbTranType.Text, dtpTranDate.Value, _Accode, TranNo, Batchno, cnCostId, "", 0, 0)
app:
            Tran.Commit()
            Tran = Nothing
            Dim pBatchno As String = Batchno
            Dim pBillDate As Date = dtpTranDate.Value.ToString("yyyy-MM-dd")
            btnNew_Click(Me, New EventArgs)
            If IO.File.Exists(Application.StartupPath & "\BillPrint.exe") Then
                Dim write As IO.StreamWriter
                write = IO.File.CreateText(Application.StartupPath & "\BillPrint.mem")
                write.WriteLine(LSet("TYPE", 15) & ":SMI")
                write.WriteLine(LSet("BATCHNO", 15) & ":" & pBatchno)
                write.WriteLine(LSet("TRANDATE", 15) & ":" & pBillDate.ToString("yyyy-MM-dd"))
                write.WriteLine(LSet("DUPLICATE", 15) & ":N")
                write.Flush()
                write.Close()
                System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe")
            End If
            Me.Refresh()
        Catch ex As Exception
            If Tran IsNot Nothing Then Tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub
    Private Sub txtPUCategory_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPUCategory.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            LoadPurCatName()
        End If
    End Sub
    Private Sub txtPUCategory_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPUCategory.LostFocus
        Main.HideHelpText()
        If txtPUCategory.Text = "" Then Exit Sub
    End Sub
    Private Sub LoadPurCatName()
        If _Accode = "" Or cmbParty_MAN.Text = "" Then
            MsgBox("Invalid Name", MsgBoxStyle.Information)
            cmbParty_MAN.Select()
            Exit Sub
        End If
        If ShowPurchaseDetail() Then
            txtPuKatchaWt_WET.Clear()
            txtPurKaTouch_AMT.Clear()
            If objPurchaseDetail.cmbCategory_MAN.Text <> "" And objPurchaseDetail.cmbCategory_MAN.Items.Contains(objPurchaseDetail.cmbCategory_MAN.Text) Then
                txtPUCategory.Text = objPurchaseDetail.cmbCategory_MAN.Text
                LoadPurCatNameDetails()
            Else
                txtPUCategory.Focus()
                txtPUCategory.SelectAll()
            End If
        Else
            txtPUCategory.Focus()
            txtPUCategory.SelectAll()
        End If
    End Sub
    Private Function ShowPurchaseDetail() As Boolean
        If objPurchaseDetail.Visible Then Return False
        objPurchaseDetail.BackColor = Me.BackColor
        objPurchaseDetail.StartPosition = FormStartPosition.CenterScreen
        objPurchaseDetail.grpPurhcase.BackgroundColor = grpHeader.BackgroundColor
        objPurchaseDetail.cmbCategory_MAN.Text = txtPUCategory.Text
        objPurchaseDetail.cmbCategory_MAN.Focus()
        If objPurchaseDetail.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Return True
        End If
        Return False
    End Function
    Private Sub LoadPurCatNameDetails()
        If txtPUCategory.Text <> "" Then
            Dim purityId As String = objGPack.GetSqlValue("SELECT PURITYID FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & txtPUCategory.Text & "'")
            If objGPack.GetSqlValue("SELECT METALTYPE FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = '" & purityId & "'") = "O" Then
                catType = "Purity"
                txtPurKaTouch_AMT.Clear()
                txtPuKatchaWt_WET.Clear()
            Else
                StrSql = " SELECT 'Bar' TYPE"
                StrSql += " UNION"
                StrSql += " SELECT 'Katcha' TYPE"
                StrSql += " UNION"
                StrSql += " SELECT 'FT' TYPE"
                catType = BrighttechPack.SearchDialog.Show("Select Bar/Katcha/FT", StrSql, cn, , , , catType, , , True)
                Select Case catType.ToString.ToUpper
                    Case "BAR"
                        txtPurKaTouch_AMT.Clear()
                        txtPuKatchaWt_WET.Clear()
                End Select
            End If
            With objPurchaseDetail
                If .cmbSubItem_Own.Text <> "" Then
                    StrSql = " SELECT STUDDEDSTONE FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & .cmbSubItem_Own.Text & "'"
                Else
                    StrSql = " SELECT STUDDEDSTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .cmbItem.Text & "'"
                End If
                If objGPack.GetSqlValue(StrSql, , "N") = "Y" Then
                    saStone = True
                Else
                    saStone = False
                End If
            End With
            Me.SelectNextControl(txtPUCategory, True, True, True, True)
        End If
    End Sub
    Private Sub cmbParty_MAN_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbParty_MAN.LostFocus
        _Accode = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbParty_MAN.Text & "'")
        GetPartyBalance()
    End Sub

    Private Sub GetPartyBalance()
        If cmbParty_MAN.Text = "" Then
            lblBalance.Text = ""
            Exit Sub
        End If
        Dim pWt As Decimal = Nothing
        Dim pAmt As Decimal = Nothing
        Dim aWt As Decimal = Nothing
        Dim dt As DataTable
        lblBalance.Text = ""
        StrSql = vbCrLf + "  SELECT SUM(WEIGHT)WEIGHT,SUM(AMOUNT)AMOUNT"
        StrSql += vbCrLf + "  FROM"
        StrSql += vbCrLf + "  ("
        StrSql += vbCrLf + "  SELECT CONVERT(NUMERIC(15,3),0)WEIGHT,DEBIT-CREDIT AMOUNT"
        StrSql += vbCrLf + "  FROM " & cnStockDb & "..OPENTRAILBALANCE WHERE ACCODE = '" & _Accode & "'"
        StrSql += vbCrLf + "  UNION ALL"
        StrSql += vbCrLf + "  SELECT CASE WHEN TRANTYPE = 'I' THEN PUREWT ELSE -1*PUREWT END WEIGHT,0 AMOUNT"
        StrSql += vbCrLf + "  FROM " & cnStockDb & "..OPENWEIGHT WHERE ACCODE = '" & _Accode & "'"
        StrSql += vbCrLf + "  UNION ALL"
        StrSql += vbCrLf + "  SELECT PUREWT,0 AMOUNT "
        StrSql += vbCrLf + "  FROM " & cnStockDb & "..ISSUE WHERE ACCODE = '" & _Accode & "' AND ISNULL(CANCEL,'') = '' AND TRANTYPE NOT IN ('AI','IAP')"
        StrSql += vbCrLf + "  UNION ALL"
        StrSql += vbCrLf + "  SELECT -1*PUREWT,0 AMOUNT "
        StrSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT WHERE ACCODE = '" & _Accode & "' AND ISNULL(CANCEL,'') = '' AND TRANTYPE NOT IN ('AR','RAP')"
        StrSql += vbCrLf + "  UNION ALL"
        StrSql += vbCrLf + "  SELECT 0 WEIGHT,CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END AS AMOUNT "
        StrSql += vbCrLf + "  FROM " & cnStockDb & "..ACCTRAN WHERE ACCODE = '" & _Accode & "' AND ISNULL(CANCEL,'') = ''"
        StrSql += vbCrLf + "  )X"
        dt = New DataTable
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dt)
        If dt.Rows.Count > 0 Then
            pWt = Val(dt.Rows(0).Item("WEIGHT").ToString)
            pAmt = Val(dt.Rows(0).Item("AMOUNT").ToString)
            lblBalance.Text = " BALANCE "
        End If
        StrSql = vbCrLf + "  SELECT SUM(WEIGHT)WEIGHT,SUM(AMOUNT)AMOUNT"
        StrSql += vbCrLf + "  FROM"
        StrSql += vbCrLf + "  ("
        StrSql += vbCrLf + "  SELECT PUREWT WEIGHT,0 AMOUNT "
        StrSql += vbCrLf + "  FROM " & cnStockDb & "..ISSUE WHERE ACCODE = '" & _Accode & "' AND ISNULL(CANCEL,'') = '' AND TRANTYPE IN ('AI','IAP')"
        StrSql += vbCrLf + "  UNION ALL"
        StrSql += vbCrLf + "  SELECT -1*PUREWT,0 AMOUNT "
        StrSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT WHERE ACCODE = '" & _Accode & "' AND ISNULL(CANCEL,'') = '' AND TRANTYPE IN ('AR','RAP')"
        StrSql += vbCrLf + "  )X"
        dt = New DataTable
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dt)
        If dt.Rows.Count > 0 Then
            aWt = Val(dt.Rows(0).Item("WEIGHT").ToString)
            lblBalance.Text = " BALANCE "
        End If
        If pWt <> 0 Then
            If pWt > 0 Then
                lblBalance.Text += "[WT : " & Format(Math.Abs(pWt), "0.000") & " Dr] "
            Else
                lblBalance.Text += "[WT : " & Format(Math.Abs(pWt), "0.000") & " Cr] "
            End If
        End If
        If pAmt <> 0 Then
            If pAmt > 0 Then
                lblBalance.Text += "[AMT : " & Format(Math.Abs(pAmt), "0.000") & " Dr] "
            Else
                lblBalance.Text += "[AMT : " & Format(Math.Abs(pAmt), "0.000") & " Cr] "
            End If
        End If
        If aWt <> 0 Then
            If aWt > 0 Then
                lblBalance.Text += "[APP WT : " & Format(Math.Abs(aWt), "0.000") & " Dr] "
            Else
                lblBalance.Text += "[APP WT : " & Format(Math.Abs(aWt), "0.000") & " Cr] "
            End If
        End If
    End Sub
    Private Sub cmbParty_MAN_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbParty_MAN.KeyDown
        If e.KeyCode = Keys.Enter Then
            If cmbParty_MAN.Text = "" Then
                MsgBox("Ac Name Should not empty", MsgBoxStyle.Information)
                cmbParty_MAN.Select()
                Exit Sub
            End If
            If Not cmbParty_MAN.Items.Contains(cmbParty_MAN.Text) Then
                MsgBox("Invalid Ac Name", MsgBoxStyle.Information)
                cmbParty_MAN.Select()
                Exit Sub
            End If
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtPuKatchaWt_WET_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPuKatchaWt_WET.GotFocus
        Select Case catType.ToString.ToUpper
            Case "KATCHA", "FT"
            Case Else
                SendKeys.Send("{TAB}")
        End Select
    End Sub

    Private Sub txtPurKaTouch_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPurKaTouch_AMT.GotFocus
        Select Case catType.ToString.ToUpper
            Case "KATCHA", "FT"
            Case Else
                SendKeys.Send("{TAB}")
        End Select
    End Sub

    Private Sub CalcPuGrsWt()
        Dim wt As Decimal = Nothing
        If Val(txtPurKaTouch_AMT.Text) > 0 Then
            wt = Val(txtPuKatchaWt_WET.Text) * (Val(txtPurKaTouch_AMT.Text) / 100)
        End If
        If Val(txtPuKatchaWt_WET.Text) > 0 Then
            txtPUGrsWt_WET.Text = IIf(wt <> 0, Format(wt, "0.000"), "")
        End If

    End Sub

    Private Sub CalcPUNetWt()
        Dim wt As Double = Nothing
        wt = Val(txtPUGrsWt_WET.Text) - Val(txtPUStoneWt_WET.Text)
        txtPUNetWt_WET.Text = IIf(wt <> 0, Format(wt, "0.000"), Nothing)
    End Sub

    Private Sub CalcPuPureWt()
        Dim wt As Double = Nothing
        If Val(txtPuTouch_AMT.Text) > 0 Then
            wt = Val(txtPUNetWt_WET.Text) * (Val(txtPuTouch_AMT.Text) / 100)
        End If
        txtPuPureWt_WET.Text = IIf(wt <> 0, Format(wt, "0.000"), "")
    End Sub

    Private Sub CalcPuTotAmt()
        Dim tot As Decimal = Nothing
        tot = Val(txtPuMc_AMT.Text) + Val(objStone.gridStoneTotal.Rows(0).Cells("AMOUNT").Value.ToString)
        txtPuTotalAmt_AMT.Text = IIf(tot <> 0, Format(tot, "0.00"), "")
    End Sub

    Private Sub txtPUStoneWt_WET_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPUStoneWt_WET.GotFocus
        SendKeys.Send("{TAB}")
    End Sub

    Private Sub txtPuTouch_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPuTouch_AMT.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If Val(txtPuTouch_AMT.Text) = 0 Then
                MsgBox("Touch Should not empty", MsgBoxStyle.Information)
                txtPuTouch_AMT.Select()
            End If
        End If
    End Sub

    Private Sub txtPuPureWt_WET_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPuPureWt_WET.GotFocus
        SendKeys.Send("{TAB}")
    End Sub

    Private Sub TextBox_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles _
txtPuKatchaWt_WET.TextChanged _
, txtPurKaTouch_AMT.TextChanged _
, txtPuTouch_AMT.TextChanged _
, txtPUGrsWt_WET.TextChanged _
, txtPUStoneWt_WET.TextChanged _
, txtPUNetWt_WET.TextChanged _
, txtPuPureWt_WET.TextChanged _
, txtPuMc_AMT.TextChanged
        Total()
    End Sub

    Private Sub Total()
        CalcPuGrsWt()
        CalcPUNetWt()
        CalcPuPureWt()
        CalcPuTotAmt()
    End Sub

    Private Sub txtPUGrsWt_WET_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPUGrsWt_WET.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If saStone Then
                ShowStoneDia()
            Else
                SendKeys.Send("{TAB}")
            End If
        End If
    End Sub
    Private Sub gridPur_UserDeletedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles gridPur.UserDeletedRow
        dtGrid.AcceptChanges()
        CalcGridPurTotal()
        CheckTransaction()
    End Sub

    Private Sub gridPur_UserDeletingRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowCancelEventArgs) Handles gridPur.UserDeletingRow
        If gridPur.Rows(e.Row.Index).Cells("trantype").Value.ToString <> "" Then
            For Each ro As DataRow In dtStoneDetails.Rows
                If ro("KEYNO") = e.Row.Cells("KEYNO").Value And ro("TRANTYPE") = e.Row.Cells("TRANTYPE").Value Then
                    ro.Delete()
                End If
            Next
            dtStoneDetails.AcceptChanges()
        End If
        dtGrid.Rows.Add()
        dtGrid.AcceptChanges()
    End Sub
    Private Sub CheckTransaction()
        TranTypeCol.Clear()
        For Each ro As DataRow In dtGrid.Rows
            If ro!TRANTYPE.ToString = "" Then Continue For
            TranTypeCol.Add(ro!TRANTYPE.ToString)
        Next
        TranTypeValidation(TranTypeCol)
    End Sub
    Private Sub TranTypeValidation(ByVal types As List(Of String))
        LoadEntryMode()
    End Sub
    Private Sub LoadEntryMode()
        Dim lastEntry As String = cmbTranType.Text
        cmbTranType.Items.Clear()
        If Not TranTypeCol.Count > 0 Then
            cmbTranType.Items.Add(WholeSaleBill_ShortEntry.EntryMode.Issue.ToString)
            cmbTranType.Items.Add(WholeSaleBill_ShortEntry.EntryMode.Receipt.ToString)
            cmbTranType.Items.Add(WholeSaleBill_ShortEntry.EntryMode.Approval_Issue.ToString)
            cmbTranType.Items.Add(WholeSaleBill_ShortEntry.EntryMode.Approval_Receipt.ToString)
            cmbParty_MAN.Enabled = True
            Exit Sub
        Else
            cmbParty_MAN.Enabled = False
        End If
        If TranTypeCol.Contains("IIS") Then
            cmbTranType.Items.Add(WholeSaleBill_ShortEntry.EntryMode.Issue.ToString)
            cmbTranType.Text = WholeSaleBill_ShortEntry.EntryMode.Issue.ToString
        ElseIf TranTypeCol.Contains("RRE") Then
            cmbTranType.Items.Add(WholeSaleBill_ShortEntry.EntryMode.Receipt.ToString)
            cmbTranType.Text = WholeSaleBill_ShortEntry.EntryMode.Receipt.ToString
        ElseIf TranTypeCol.Contains("IAP") Then
            cmbTranType.Items.Add(WholeSaleBill_ShortEntry.EntryMode.Approval_Issue.ToString)
            cmbTranType.Text = WholeSaleBill_ShortEntry.EntryMode.Approval_Issue.ToString
        ElseIf TranTypeCol.Contains("RAP") Then
            cmbTranType.Items.Add(WholeSaleBill_ShortEntry.EntryMode.Approval_Receipt.ToString)
            cmbTranType.Text = WholeSaleBill_ShortEntry.EntryMode.Approval_Receipt.ToString
        End If
    End Sub
End Class