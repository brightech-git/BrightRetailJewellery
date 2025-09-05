Imports System.Data.OleDb
Public Class frmRepair
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim searchSender As Control = Nothing
    Dim dtSoftKeys As DataTable = GetAdmindbSoftValueAll()
    Dim McRound As Integer = Val(GetAdmindbSoftValue("ROUNDOFF-MC"))
    Dim LockOrderDate As Boolean = IIf(GetAdmindbSoftValuefromDt(dtSoftKeys, "LOCK_ORDERDATE", "Y") = "Y", True, False)
    Dim OrdRepManDueDate As Double = Val(GetAdmindbSoftValuefromDt(dtSoftKeys, "ORDREPMANDUEDATE", "1").ToString)
    Dim ORDDATE_EDIT As Boolean = IIf(GetAdmindbSoftValuefromDt(dtSoftKeys, "ORDDATE_EDIT", "Y") = "Y", True, False)
    Dim dtGridRepair As New DataTable
    'Dim dtGridStone As New DataTable
    Dim dtStoneDetails As New DataTable

    Public Batchno As String
    Public RepairNo As String = ""
    Dim TranNo As Integer
    Public BillCostId As String
    Public BillDate As Date = GetEntryDate(GetServerDate)
    Public BillCashCounterId As String
    Dim CASHID As String
    Dim VATEXM As String = "Y"
    Public RepType As RepairType = RepairType.RepairEntry
    Dim defalutDestination As String
    ''OrderDet
    Dim calcType As String = Nothing
    Public subItemName As String = Nothing
    Public sizeName As String = Nothing
    Public isStone As Boolean = False
    Dim objCreditCard As New frmCreditCardAdj
    Dim objAdvance As New frmAdvanceAdj(BillCostId)
    Dim objCheaque As New frmChequeAdj
    Dim objChitCard As New frmChitAdj
    Public objAddressDia As New frmAddressDia(False, False)
    Public objStone As New frmStoneDia
    Public MANBILLNO As Boolean = False
    Dim objManualBill As frmManualBillNoGen
    Dim OR_REP_NewCatCode As String = ""
    Dim SMS_MSG_REPBOOK As String = objGPack.GetSqlValue("SELECT ISNULL(TEMPLATE_DESC,'') AS TEMPLATE_DESC FROM " & cnAdminDb & "..SMSTEMPLATE WHERE TEMPLATE_NAME='SMS_MSG_REPBOOK' AND ISNULL(ACTIVE,'Y')<>'N'", "TEMPLATE_DESC").ToString
    Public RepairUpdSno As String = ""
    Public UpdateFileImagePath As String = ""
    Public PICPATH As String = ""
    Public objAddtionalDetails As New frmOrAdditionalDetails
    Dim ORADDITIONALDETAIL As Boolean = IIf(GetAdmindbSoftValue("ORADDITIONALDETAIL", "N") = "Y", True, False)
    Dim dtOrderAdditionalDetails As New DataTable
    Dim MetalBasedStone As Boolean = IIf(GetAdmindbSoftValue("MULTIMETALBASEDSTONE", "N") = "Y", True, False)



    Public Enum RepairType
        RepairEntry = 0
        RepairEdit = 1
        RepairUpdate = 2
    End Enum
    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        objGPack.Validator_Object(objCreditCard)
        objGPack.Validator_Object(objAdvance)
        objGPack.Validator_Object(objCheaque)
        CASHID = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'CASH'", , "CASH")


        ' Add any initialization after the InitializeComponent() call.

        ''Repair
        With dtGridRepair
            .Columns.Add("ITEMNAME", GetType(String))
            .Columns.Add("DESCRIPTION", GetType(String))
            .Columns.Add("PCS", GetType(Integer))
            .Columns.Add("GRSWT", GetType(Decimal))
            .Columns.Add("NETWT", GetType(Decimal))
            .Columns.Add("MCGRM", GetType(Double))
            .Columns.Add("MC", GetType(Double))
            .Columns.Add("GROSSAMT", GetType(Double))
            .Columns.Add("REASON", GetType(String))
            Dim col As New DataColumn("KEYNO", GetType(Integer))
            col.AutoIncrement = True
            col.AutoIncrementSeed = 1
            col.AutoIncrementStep = 1
            .Columns.Add(col)
            .Columns.Add("ENTFLAG", GetType(String))
            .Columns.Add("LESSWT", GetType(Decimal))
            .Columns.Add("STONEAMT", GetType(Double))
            .Columns.Add("MISCAMT", GetType(Double))
            .Columns.Add("SUBITEMNAME", GetType(String))
            .Columns.Add("SIZENAME", GetType(String))
            .Columns.Add("PICTFILE", GetType(String))
        End With
        dtGridRepair.AcceptChanges()
        gridOrder.DataSource = dtGridRepair
        gridOrder.ColumnHeadersVisible = False
        FormatGridColumns(gridOrder)
        ClearDtGrid(dtGridRepair)
        StyleGridOrder(gridOrder)
        ''Order Total
        Dim dtGridRepairTotal As New DataTable
        dtGridRepairTotal = dtGridRepair.Copy
        dtGridRepairTotal.Rows.Clear()
        dtGridRepairTotal.Rows.Add()
        dtGridRepairTotal.Rows(0).Item(0) = "TOTAL"
        gridOrderTotal.ColumnHeadersVisible = False
        gridOrderTotal.Font = New Font("VERDANA", 8, FontStyle.Bold)
        gridOrderTotal.DataSource = dtGridRepairTotal
        For Each col As DataGridViewColumn In gridOrder.Columns
            With gridOrderTotal.Columns(col.Name)
                .Visible = col.Visible
                .Width = col.Width
                .DefaultCellStyle = col.DefaultCellStyle
            End With
        Next
        FormatGridColumns(gridOrderTotal)
        StyleGridOrder(gridOrderTotal)
        CalcGridOrderTotal()

        ''Stone Group
        cmbStUnit.Items.Add("C")
        cmbStUnit.Items.Add("G")
        cmbStUnit.Text = "C"
        cmbStCalc.Items.Add("W")
        cmbStCalc.Items.Add("P")
        cmbStCalc.Text = "W"

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
            .Add("CUTID", GetType(Integer))
            .Add("COLORID", GetType(Integer))
            .Add("CLARITYID", GetType(Integer))
            .Add("SHAPEID", GetType(Integer))
            .Add("SETTYPEID", GetType(Integer))
            .Add("HEIGHT", GetType(Decimal))
            .Add("WIDTH", GetType(Decimal))
            .Add("SGST", GetType(Decimal))
            .Add("CGST", GetType(Decimal))
            .Add("IGST", GetType(Decimal))
            .Add("CESS", GetType(Decimal))
            If MetalBasedStone Then
                .Add("TAGMSNO", GetType(String))
            End If
            .Add("STNGRPID", GetType(Integer))
            .Add("ORGAMOUNT", GetType(Double))
        End With

        'REPADDITIONAL DETAILS
        With dtOrderAdditionalDetails.Columns
            .Add("KEYNO", GetType(Integer))
            .Add("TYPENAME", GetType(String))
            .Add("VALUENAME", GetType(String))
        End With

    End Sub

    Private Function GetControlLocation(ByVal ctrl As Control, ByRef pt As Point) As Point
        If TypeOf ctrl Is Form Then
            Return pt
        Else
            pt += ctrl.Location
            Return GetControlLocation(ctrl.Parent, pt)
        End If
        Return pt
    End Function


    Private Sub ShowRepairAdditionalDetails()
        If ORADDITIONALDETAIL = False Then
            Exit Sub
        End If
        If objAddtionalDetails.Visible Then Exit Sub
        objAddtionalDetails.BackColor = pnlContainer_OWN.BackColor
        objAddtionalDetails.StartPosition = FormStartPosition.CenterScreen
        objAddtionalDetails.MaximizeBox = False
        objAddtionalDetails.cmbType.Select()
        objAddtionalDetails.ShowDialog()
        Me.SelectNextControl(txtOGrsWt_WET, True, True, True, True)
    End Sub
    Private Sub CalcStoneAmount()
        Dim amt As Double = Nothing
        If cmbStCalc.Text = "P" Then
            amt = Val(txtStRate_AMT.Text) * Val(txtStPcs_NUM.Text)
        Else
            amt = Val(txtStRate_AMT.Text) * Val(txtStWeight_WET.Text)

        End If
        txtStAmount_AMT.Text = IIf(amt <> 0, Format(amt, "0.00"), "")
    End Sub

    Private Sub CalcMc()
        Dim mc As Double = Nothing
        mc = Val(txtONetWt_WET.Text) * Val(txtOMcPerGrm_AMT.Text)
        mc = Math.Round(mc, McRound)
        txtOMc_AMT.Text = IIf(mc <> 0, Format(mc, "0.00"), Nothing)
    End Sub

    Private Sub CalcGridOrderTotal()
        Dim pcs As Integer = Nothing
        Dim grsWt As Decimal = Nothing
        Dim netWt As Decimal = Nothing
        Dim wastage As Decimal = Nothing
        Dim mc As Double = Nothing
        Dim grossAmt As Double = Nothing
        Dim vat As Double = Nothing
        Dim amount As Double = Nothing
        dtGridRepair.AcceptChanges()
        For Each dgvRow As DataGridViewRow In gridOrder.Rows
            If dgvRow.Cells("ENTFLAG").Value.ToString = "" Then Continue For
            pcs += Val(dgvRow.Cells("PCS").Value.ToString)
            grsWt += Val(dgvRow.Cells("GRSWT").Value.ToString)
            netWt += Val(dgvRow.Cells("NETWT").Value.ToString)
            mc += Val(dgvRow.Cells("MC").Value.ToString)
            grossAmt += Val(dgvRow.Cells("GROSSAMT").Value.ToString)
        Next
        With gridOrderTotal.Rows(0)
            .Cells("PCS").Value = IIf(pcs <> 0, pcs, DBNull.Value)
            .Cells("GRSWT").Value = IIf(grsWt <> 0, grsWt, DBNull.Value)
            .Cells("NETWT").Value = IIf(netWt <> 0, netWt, DBNull.Value)
            .Cells("MC").Value = IIf(mc <> 0, mc, DBNull.Value)
            .Cells("GROSSAMT").Value = IIf(grossAmt <> 0, grossAmt, DBNull.Value)
        End With
        txtAdjAdvanceWt.Text = IIf(netWt <> 0, Format(netWt, "0.000"), Nothing)
    End Sub

    Private Sub CalcStoneWtAmount()
        Dim diaCaratWt As Double = 0
        Dim diaGramWt As Double = 0
        Dim diaPcs As Integer = 0
        Dim diaAmt As Double = 0

        Dim preCaratWt As Double = 0
        Dim preGramWt As Double = 0
        Dim prePcs As Integer = 0
        Dim preAmt As Double = 0

        Dim stoCaratWt As Double = 0
        Dim stoGramWt As Double = 0
        Dim stoPcs As Integer = 0
        Dim stoAmt As Double = 0

        For cnt As Integer = 0 To gridStone.RowCount - 1
            With gridStone.Rows(cnt)
                Select Case .Cells("METALID").Value.ToString
                    Case "D"
                        diaPcs += Val(.Cells("PCS").Value.ToString)
                        diaAmt += Val(.Cells("AMOUNT").Value.ToString)
                    Case "S"
                        stoPcs += Val(.Cells("PCS").Value.ToString)
                        stoAmt += Val(.Cells("AMOUNT").Value.ToString)
                    Case "P"
                        prePcs += Val(.Cells("PCS").Value.ToString)
                        preAmt += Val(.Cells("AMOUNT").Value.ToString)
                End Select
                Select Case .Cells("UNIT").Value.ToString
                    Case "G"
                        If .Cells("METALID").Value.ToString = "S" Then
                            stoGramWt += Val(.Cells("WEIGHT").Value.ToString)
                        ElseIf .Cells("METALID").Value.ToString = "P" Then
                            preGramWt += Val(.Cells("WEIGHT").Value.ToString)
                        ElseIf .Cells("METALID").Value.ToString = "D" Then
                            diaGramWt += Val(.Cells("WEIGHT").Value.ToString)
                        End If
                    Case "C"
                        If .Cells("METALID").Value.ToString = "S" Then
                            stoCaratWt += Val(.Cells("WEIGHT").Value.ToString)
                        ElseIf .Cells("METALID").Value.ToString = "P" Then
                            preCaratWt += Val(.Cells("WEIGHT").Value.ToString)
                        ElseIf .Cells("METALID").Value.ToString = "D" Then
                            diaCaratWt += Val(.Cells("WEIGHT").Value.ToString)
                        End If
                End Select
            End With
        Next

        Dim lessWt As Double = (diaCaratWt + stoCaratWt + preCaratWt) / 5 + (diaGramWt + stoGramWt + preGramWt)
        Dim stnAmt As Double = diaAmt + stoAmt + preAmt
        If gridStoneTotal.RowCount > 0 Then
            gridStoneTotal.Rows(0).Cells("WEIGHT").Value = IIf(lessWt <> 0, lessWt, DBNull.Value)
            gridStoneTotal.Rows(0).Cells("AMOUNT").Value = IIf(stnAmt <> 0, stnAmt, DBNull.Value)
        End If
        Dim ntWt As Double = Val(txtOGrsWt_WET.Text) - lessWt
        txtONetWt_WET.Text = IIf(ntWt <> 0, Format(ntWt, "0.000"), Nothing)
    End Sub

    Private Sub ClearDtGrid(ByVal dt As DataTable) ''Only For gridOrder
        dt.Columns("KeyNo").AutoIncrementSeed = 1
        dt.Rows.Clear()
        For i As Integer = 1 To 20
            dt.Rows.Add()
        Next
        dt.AcceptChanges()
    End Sub

    Private Sub StyleGridStone(ByVal gridStone As DataGridView)
        With gridStone
            .Columns("KEYNO").Visible = False
            .Columns("METALID").Visible = False
            .Columns("ITEM").Width = txtStItem.Width + 1
            .Columns("SUBITEM").Width = txtStSubItem.Width + 1
            .Columns("PCS").Width = txtStPcs_NUM.Width + 1
            .Columns("UNIT").Width = cmbStUnit.Width + 1
            .Columns("CALC").Width = cmbStCalc.Width + 1
            .Columns("WEIGHT").Width = txtStWeight_WET.Width + 1
            .Columns("RATE").Width = txtStRate_AMT.Width + 1
            .Columns("AMOUNT").Width = txtStAmount_AMT.Width + 1
            .Columns("DISCOUNT").Visible = False
        End With
    End Sub

    Private Sub StyleGridOrder(ByVal grid As DataGridView)
        With grid
            .Columns("ITEMNAME").Width = txtOItemName.Width + 1
            .Columns("DESCRIPTION").Width = txtODescription.Width + 1
            .Columns("PCS").Width = txtOPcs_NUM.Width + 1
            .Columns("GRSWT").Width = txtOGrsWt_WET.Width + 1
            .Columns("NETWT").Width = txtONetWt_WET.Width + 1
            .Columns("MCGRM").Width = txtOMcPerGrm_AMT.Width + 1
            .Columns("MC").Width = txtOMc_AMT.Width + 1
            .Columns("GROSSAMT").Width = txtOAmount_AMT.Width + 1
            .Columns("REASON").Width = txtONatureOfRepair.Width + 1
            For cnt As Integer = 9 To gridOrder.ColumnCount - 1
                .Columns(cnt).Visible = False
            Next
        End With
    End Sub

    Private Sub frmRepairKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
nextGroup:

            If objGPack.IsActive(tabStone) Then
                tabOtherOptions.SelectedTab = tabAddress
                Me.SelectNextControl(txtOGrsWt_WET, True, True, True, True)

            ElseIf objGPack.IsActive(grpAddress) Then
                txtAdjCash_AMT.Focus()
            ElseIf txtOItemName.Focused Then
                If GetEntFlag() Then
                    ShowAddressDia()
                End If
            End If
        End If
    End Sub

    Private Function GetEntFlag() As Boolean
        Dim entFlag As Boolean = False
        For Each ro As DataRow In dtGridRepair.Rows
            If ro!ENTFLAG.ToString = "" Then Exit For
            Return True
        Next
        Return entFlag
    End Function


    Private Sub frmRepairKeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtSalesMan_NUM.Focused Then Exit Sub
            If txtOItemName.Focused Then Exit Sub
            If txtOGrsWt_WET.Focused Then Exit Sub
            If txtONatureOfRepair.Focused Then Exit Sub
            ''stone
            If txtStItem.Focused Then Exit Sub
            If txtStAmount_AMT.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmRepairLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        If _CounterWiseCashMaintanance Then
            CASHID = objGPack.GetSqlValue("SELECT CASHACCODE FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHID = '" & BillCashCounterId & "' AND ISNULL(CASHACCODE,'') <> ''", , GetAdmindbSoftValue("CASH", "CASH"))
        End If
        defalutDestination = GetAdmindbSoftValue("PICPATH")
        If Not defalutDestination.EndsWith("\") And defalutDestination <> "" Then defalutDestination += "\"
        Dim dt As New DateTimePicker


        If LockOrderDate Then
            lblOrderDate.Visible = False
            dtpRepairDate.Visible = False
            dtpDueDate.MinimumDate = BillDate
            dtpDueDate.MaximumDate = dt.MaxDate
            dtpRemDate.MinimumDate = BillDate
            dtpRemDate.MaximumDate = dt.MaxDate
            dtpRepairDate.MinimumDate = dt.MinDate
            dtpRepairDate.MaximumDate = dt.MaxDate
        Else
            dtpDueDate.MinimumDate = dt.MinDate
            dtpDueDate.MaximumDate = dt.MaxDate
            dtpRemDate.MinimumDate = dt.MinDate
            dtpRemDate.MaximumDate = dt.MaxDate
            dtpRepairDate.MinimumDate = dt.MinDate
            dtpRepairDate.MaximumDate = dt.MaxDate
        End If

        

        tabOtherOptions.Region = New Region(New RectangleF(tabAddress.Left, tabAddress.Top, tabAddress.Width, tabAddress.Height))
        pnlOrderType.BackColor = Color.Transparent
        Style5ToolStripMenuItem_Click(Me, New EventArgs)
        pnlContainer_OWN.BorderStyle = BorderStyle.Fixed3D
        pnlContainer_OWN.Location = New Point((ScreenWid - pnlContainer_OWN.Width) / 2, ((ScreenHit - 128) - pnlContainer_OWN.Height) / 2)
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub Style1ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Style1ToolStripMenuItem.Click
        pnlContainer_OWN.BackColor = Color.RosyBrown
        ColorChange(pnlContainer_OWN, Color.MistyRose, pnlContainer_OWN.BackColor)
        pnlOrderType.BackColor = Color.Transparent
    End Sub

    Private Sub Style2ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Style2ToolStripMenuItem.Click
        pnlContainer_OWN.BackColor = Color.SlateGray
        ColorChange(pnlContainer_OWN, Color.MintCream, pnlContainer_OWN.BackColor)
        pnlOrderType.BackColor = Color.Transparent
    End Sub

    Private Sub Style3ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Style3ToolStripMenuItem.Click
        pnlContainer_OWN.BackColor = Color.PaleVioletRed
        ColorChange(pnlContainer_OWN, Color.MistyRose, pnlContainer_OWN.BackColor)
        pnlOrderType.BackColor = Color.Transparent
    End Sub

    Private Sub Style4ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Style4ToolStripMenuItem.Click
        pnlContainer_OWN.BackColor = SystemColors.AppWorkspace
        ColorChange(pnlContainer_OWN, SystemColors.Control, pnlContainer_OWN.BackColor)
        pnlOrderType.BackColor = Color.Transparent
    End Sub

    Private Sub Style5ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Style5ToolStripMenuItem.Click
        pnlContainer_OWN.BackColor = SystemColors.InactiveCaption
        ColorChange(pnlContainer_OWN, Color.Lavender, pnlContainer_OWN.BackColor)
        pnlOrderType.BackColor = Color.Transparent
    End Sub

    Private Sub Style6ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Style6ToolStripMenuItem.Click
        pnlContainer_OWN.BackColor = SystemColors.InactiveCaption
        ColorChange(pnlContainer_OWN, Color.Lavender, pnlContainer_OWN.BackColor)
        pnlOrderType.BackColor = Color.Transparent
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        If MessageBox.Show("Close Alert", "Do you want to Close?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.OK Then
            Me.Close()
        End If
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        objGPack.TextClear(Me)
        Set916Rate(BillDate)
        tran = Nothing
        rbtCustomerOrderType.Checked = True
        dtpDueDate.Value = GetEntryDate(GetServerDate)
        dtpRemDate.Value = GetEntryDate(GetServerDate)
        'BillDate = GetEntryDate(GetServerDate)
        ClearDtGrid(dtGridRepair)
        CalcGridOrderTotal()
        ClearItemDetails()
        If picImage.Image IsNot Nothing Then
            picImage.Image = Nothing
        End If
        If OrdRepManDueDate <> 0 Then
            txtDueDays_NUM.Text = OrdRepManDueDate.ToString
        Else
            txtDueDays_NUM.Text = "0"
        End If
        dtStoneDetails.Rows.Clear()
        CalcStoneWtAmount()
        objCreditCard = New frmCreditCardAdj
        objCheaque = New frmChequeAdj
        objAdvance = New frmAdvanceAdj(BillCostId)
        objAddressDia = New frmAddressDia(False, False)
        objAddtionalDetails = New frmOrAdditionalDetails
        dtOrderAdditionalDetails.Rows.Clear()
        dtOrderAdditionalDetails.AcceptChanges()
        Dim ctlId As String = "REPAIRNO"
        strSql = " SELECT CONVERT(INT,CTLTEXT) FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = '" & ctlId & "' AND COMPANYID = '" & strCompanyId & "'"
        TranNo = Val(objGPack.GetSqlValue(strSql, , , tran)) + 1
        lblNextNo.Text = "R" & Mid(Format(cnTranToDate, "dd/MM/yyyy"), 9, 2).ToString & TranNo
        If MANBILLNO Then
            lblNx.Visible = False
            lblNext.Visible = False
            lblNextNo.Visible = False
        End If
        If rbtCustomerOrderType.Checked Then rbtCustomerOrderType.Focus() Else rbtCompanyOrderType.Focus()
    End Sub

    Function GetTranControlValues(ByVal ctlId As String, Optional ByVal ddefault As Integer = 1, Optional ByVal dbName As String = Nothing) As Integer
        Dim cmd As OleDbCommand = Nothing
        If dbName = Nothing Then
            strSql = " SELECT ISNULL(MAX(CONVERT(INT,CTLTEXT))+1,1) CTLTEXT FROM " & cnStockDb & "..SOFTCONTROLTRAN"
            strSql += " WHERE CTLID = '" & ctlId & "'"
        Else
            strSql = " SELECT ISNULL(MAX(CONVERT(INT,CTLTEXT))+1,1) CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL"
            strSql += " WHERE CTLID = '" & ctlId & "'"
        End If
        Return Val(objGPack.GetSqlValue(strSql, , ddefault, tran))
    End Function

    Private Sub GenBatchNo()
        Batchno = GetNewBatchno(cnCostId, BillDate, tran)
    End Sub

    Private Sub InsertOrderDetail(ByVal index As Integer)
        With gridOrder.Rows(index)
            Dim iio As IO.FileInfo = Nothing
            Dim extension As String = Nothing
            Dim picFileName As String = Nothing
            Dim destFilePath As String = Nothing
            If .Cells("PICTFILE").Value.ToString <> "" And defalutDestination <> "" Then
                If IO.Directory.Exists(defalutDestination) Then
                    iio = New IO.FileInfo(.Cells("PICTFILE").Value.ToString)
                    extension = iio.Extension
                    picFileName = iio.Name
                    picFileName = RepairNo & "_" & (index + 1).ToString + extension
                    destFilePath = defalutDestination & picFileName
                End If
            End If
            Dim orSno As String = GetNewSno(TranSnoType.ORMASTCODE, tran, "GET_ADMINSNO_TRAN")
            strSql = " INSERT INTO " & cnAdminDb & "..ORMAST"
            strSql += " ("
            strSql += " SNO,ORNO,ORDATE,REMDATE,DUEDATE,ORTYPE,COMPANYID,ORRATE"
            strSql += " ,ORMODE,ITEMID,SUBITEMID,DESCRIPT,PCS,GRSWT,NETWT"
            strSql += " ,SIZEID,RATE,NATURE,MCGRM,MC,WASTPER,WAST"
            strSql += " ,COMMPER,COMM,OTHERAMT,CANCEL,ORVALUE,COSTID,BATCHNO"
            strSql += " ,CORNO,PROPSMITH,PICTFILE,EMPID"
            strSql += " ,USERID,UPDATED,UPTIME,APPVER,REASON,SYSTEMID"
            strSql += " )"
            strSql += " VALUES"
            strSql += " ("
            strSql += " '" & orSno & "'" 'SNO
            strSql += " ,'" & RepairNo & "'" 'ORNO
            strSql += " ,'" & GetEntryDate(BillDate, tran).ToString("yyyy-MM-dd") & "'" 'ORDATE
            strSql += " ,'" & dtpRemDate.Value.Date.ToString("yyyy-MM-dd") & "'" 'REMDATE
            strSql += " ,'" & dtpDueDate.Value.Date.ToString("yyyy-MM-dd") & "'" 'DUEDATE
            strSql += " ,'R'" 'ORTYPE
            strSql += " ,'" & strCompanyId & "'" 'COMPANYID
            strSql += " ,'D'" 'ORRATE
            strSql += " ,'" & IIf(rbtCustomerOrderType.Checked, "C", "O") & "'" 'ORMODE
            strSql += " ," & Val(objGPack.GetSqlValue(" SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Cells("ITEMNAME").Value.ToString & "'", , , tran)) & "" 'ITEMID
            strSql += " ," & Val(objGPack.GetSqlValue(" SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & .Cells("SUBITEMNAME").Value.ToString & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Cells("ITEMNAME").Value.ToString & "')", , , tran)) & "" 'SUBITEMID
            strSql += " ,'" & .Cells("DESCRIPTION").Value.ToString & "'" 'DESCRIPT
            strSql += " ," & Val(.Cells("PCS").Value.ToString) & "" 'PCS
            strSql += " ," & Val(.Cells("GRSWT").Value.ToString) & "" 'GRSWT
            strSql += " ," & Val(.Cells("NETWT").Value.ToString) & "" 'NETWT
            strSql += " ," & Val(objGPack.GetSqlValue("SELECT SIZEID FROM " & cnAdminDb & "..ItemSize WHERE SIZENAME = '" & .Cells("SIZENAME").Value.ToString & "'", , , tran)) & "" 'SIZEID
            strSql += " ,0" 'RATE
            strSql += " ,''" 'NATURE
            strSql += " ," & Val(.Cells("MCGRM").Value.ToString) & "" 'MCGRM
            strSql += " ," & Val(.Cells("MC").Value.ToString) & "" 'MC
            strSql += " ,0" 'WASTPER
            strSql += " ,0" 'WAST
            strSql += " ,0" 'COMMPER
            strSql += " ,0" 'COMM
            strSql += " ," & Val(.Cells("STONEAMT").Value.ToString) & "" 'OTHERAMT
            strSql += " ,''" 'CANCEL
            strSql += " ," & Val(.Cells("GROSSAMT").Value.ToString) & "" 'ORVALUE
            strSql += " ,'" & BillCostId & "'" 'COSTID
            strSql += " ,'" & Batchno & "'" 'BATCHNO
            strSql += " ,0" 'CORNO
            strSql += " ,''" 'PROPSMITH
            'strSql += " ,'" & .Cells("PICTFILE").Value.ToString & "'" 'PICTFILE
            strSql += " ,'" & picFileName & "'" 'PICTFILE
            strSql += " ," & Val(txtSalesMan_NUM.Text) & "" 'eMPID
            strSql += " ," & userId & ""
            strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'"
            strSql += " ,'" & Date.Now.ToLongTimeString & "'"
            strSql += " ,'" & VERSION & "'"
            strSql += " ,'" & .Cells("REASON").Value.ToString & "'" 'REASON
            strSql += " ,'" & systemId & "'"
            strSql += " )"
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)

            ''Stone
            For Each stRow As DataRow In dtStoneDetails.Rows
                If .Cells("KEYNO").Value = stRow("KEYNO") Then
                    InsertStoneDetails(stRow, orSno)
                End If
            Next

            'REP ADDTIONAL DETAILS
            For Each stRow As DataRow In dtOrderAdditionalDetails.Rows
                If .Cells("KEYNO").Value = stRow("KEYNO") Then
                    InsertOrderAdditionalDetails(TranNo, orSno, "RE", BillCostId, strCompanyId, stRow, Batchno, tran)
                End If
            Next

        End With
    End Sub



    Private Sub InsertStoneDetails(ByVal stnRow As DataRow, ByVal orSno As String)
        ''Insert Stone
        With stnRow
            strSql = " INSERT INTO " & cnadmindb & "..ORSTONE"
            strSql += " ("
            strSql += " SNO,ORSNO,STNITEMID,STNSUBITEMID,STNPCS,STNWT,STNRATE,STNAMT"
            strSql += " ,BATCHNO,CANCEL,COSTID,APPVER,STNUNIT,CALCMODE,COMPANYID"
            strSql += " )"
            strSql += " VALUES"
            strSql += " ("
            strSql += " '" & GetNewSno(TranSnoType.ORSTONECODE, tran, "GET_ADMINSNO_TRAN") & "'" 'SNO
            strSql += " ,'" & orSno & "'" 'ORSNO
            strSql += " ," & Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Item("ITEM").ToString & "'", , , tran)) & "" 'STNITEMID
            strSql += " ," & Val(objGPack.GetSqlValue("SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & .Item("SUBITEM").ToString & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Item("ITEM").ToString & "')", , , tran)) & "" 'STNSUBITEMID
            strSql += " ," & Val(.Item("PCS").ToString) & "" 'STNPCS
            strSql += " ," & Val(.Item("WEIGHT").ToString) & "" 'STNWT
            strSql += " ," & Val(.Item("RATE").ToString) & "" 'STNRATE
            strSql += " ," & Val(.Item("AMOUNT").ToString) & "" 'STNAMOUNT
            strSql += " ,'" & Batchno & "'" 'BATCHNO
            strSql += " ,''" 'CANCEL
            strSql += " ,'" & BillCostId & "'" 'COSTID
            strSql += " ,'" & VERSION & "'" 'VERSION
            strSql += " ,'" & .Item("UNIT").ToString & "'" 'STNUNIT
            strSql += " ,'" & .Item("CALC").ToString & "'" 'STNCALC
            strSql += " ,'" & strCompanyId & "'" 'COMPANYID
            strSql += " )"
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)
        End With
    End Sub

    Private Sub InsertOrderAdditionalDetails(ByVal tNo As Integer,
          ByVal Orsno As String,
          ByVal Trantype As String,
          ByVal Costid As String,
          ByVal Companyid As String,
          ByVal ro As DataRow,
          ByVal Batchno As String _
          , ByVal tran As OleDbTransaction)
        Dim Typeid As Integer = 0
        Dim Valueid As Integer = 0
        With ro
            Typeid = GetSqlValue("SELECT TYPEID FROM " & cnAdminDb & "..ORADMAST WHERE TYPENAME = '" & .Item("TYPENAME").ToString & "'", cn, tran)
            Valueid = GetSqlValue("SELECT VALUEID FROM " & cnAdminDb & "..ORADVALUEMAST WHERE TYPEID = '" & Typeid & "' AND VALUENAME = '" & .Item("VALUENAME").ToString & "'", cn, tran)
            strSql = " INSERT INTO " & cnAdminDb & "..ORADTRAN"
            strSql += " ("
            strSql += " SNO,TYPEID,VALUENAME,ORSNO,TRANNO,ORNO,TRANDATE"
            strSql += " ,TRANTYPE,COSTID,COMPANYID,BATCHNO,USERID,UPDATED,UPTIME,VALUEID"
            strSql += " )"
            strSql += " VALUES"
            strSql += " ("
            strSql += " '" & GetNewSno(TranSnoType.ORADTRAN, tran, "GET_ADMINSNO_TRAN") & "'" ''SNO
            strSql += " ,'" & Typeid & "'" 'typeid
            strSql += " ,'" & .Item("VALUENAME").ToString & "'" 'VALUENAME
            strSql += " ,'" & Orsno & "'" 'TRANNO 
            strSql += " ," & tNo & "" 'TRANNO 
            strSql += " ,'" & RepairNo & "'" 'TRANNO 
            strSql += " ,'" & GetEntryDate(BillDate, tran).ToString("yyyy-MM-dd") & "'" 'TRANDATE
            strSql += " ,'" & Trantype & "'" 'Trantype
            strSql += " ,'" & BillCostId & "'" 'COSTID
            strSql += " ,'" & strCompanyId & "'" 'COMPANYId
            strSql += " ,'" & Batchno & "'" 'BATCHNO
            strSql += " ,'" & userId & "'" 'USERID
            strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
            strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIMe                    
            strSql += " ,'" & Valueid & "'"
            strSql += " )"
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)
        End With
        strSql = ""
    End Sub

    Private Sub ShowAddressDia()
        If objAddressDia.Visible Then Exit Sub
        objAddressDia.BackColor = pnlContainer_OWN.BackColor
        objAddressDia.StartPosition = FormStartPosition.CenterScreen
        'objAddressDia.StartPosition = FormStartPosition.Manual
        'objAddressDia.Location = New Point(75, 181)
        objAddressDia.MaximizeBox = False
        objAddressDia.grpAddress.BackgroundColor = grpHeader.BackgroundColor
        'objAddressDia.dtpAddressDueDate.Select()
        objAddressDia.txtMobile.Select()
        If objAddressDia.ShowDialog() = Windows.Forms.DialogResult.OK Then
            txtAdjCash_AMT.Focus()
        End If
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
        If Not MANBILLNO Then
            If Not CheckDate(BillDate) Then Exit Sub
            If CheckEntryDate(BillDate) Then Exit Sub
        End If
        ''VALIDATION
        Dim entFlag As Boolean = GetEntFlag()
        If Not entFlag Then
            MsgBox("There is no Record", MsgBoxStyle.Information)
            txtOItemName.Focus()
            Exit Sub
        End If
        If objAddressDia.txtAddressName.Text = "" Then
            MsgBox("Party Name Should Not Empty", MsgBoxStyle.Information)

            ShowAddressDia()
            objAddressDia.txtAddressName.Select()
            Exit Sub
        End If
        If MANBILLNO Then
            objManualBill = New frmManualBillNoGen
            objGPack.Validator_Object(objManualBill)
ReEnterBillNO:
            If objManualBill.ShowDialog = Windows.Forms.DialogResult.OK Then
                TranNo = Val(objManualBill.txtBillNo_NUM.Text)
                strSql = " SELECT DISTINCT ORNO FROM " & cnadmindb & "..ORMAST"
                strSql += " WHERE ORNO = '" & GetCostId(BillCostId) & GetCompanyId(strCompanyId) & "R" & Mid(Format(cnTranToDate, "dd/MM/yyyy"), 9, 2).ToString & TranNo.ToString & "'"
                strSql += " AND COMPANYID = '" & strCompanyId & "'"
                strSql += " UNION"
                strSql += " SELECT DISTINCT RUNNO FROM " & cnAdminDb & "..OUTSTANDING"
                strSql += " WHERE RUNNO = '" & GetCostId(BillCostId) & GetCompanyId(strCompanyId) & "R" & Mid(Format(cnTranToDate, "dd/MM/yyyy"), 9, 2).ToString & TranNo.ToString & "'"
                strSql += " AND COMPANYID = '" & strCompanyId & "'"
                If objGPack.GetSqlValue(strSql, , "-1", tran) <> "-1" Then
                    MsgBox("Order BillNo Already Exist", MsgBoxStyle.Information)
                    GoTo ReEnterBillNO
                End If
            Else
                btnSave.Focus()
                Exit Sub
            End If
        End If
        Me.Refresh()
        Try
StartTrans:
            If RepType = RepairType.RepairUpdate Then
                UpdateRepair()
                strSql = " SELECT BATCHNO,ORDATE FROM " & cnAdminDb & "..ORMAST WHERE SNO = '" & RepairUpdSno & "'"
                Batchno = objGPack.GetSqlValue(strSql, "BATCHNO")
                BillDate = objGPack.GetSqlValue(strSql, "ORDATE")

                Dim BillPrintExe As Boolean = IIf(GetAdmindbSoftValue("CALLBILLPRINTEXE", "Y") = "Y", True, False)
                Dim BillPrint_Format As String = GetAdmindbSoftValue("BILLPRINT_FORMAT", "")
                If GST And BillPrint_Format = "M1" Then
                    Dim obj As New BrighttechREPORT.frmBillPrintDocA4N("POS", Batchno.ToString, BillDate.ToString("yyyy-MM-dd"), "")
                    Me.Refresh()
                ElseIf GST And BillPrint_Format = "M2" Then
                    Dim obj As New BrighttechREPORT.frmBillPrintDocB5("POS", Batchno.ToString, BillDate.ToString("yyyy-MM-dd"), "")
                    Me.Refresh()
                ElseIf GST And BillPrint_Format = "M3" Then
                    Dim obj As New BrighttechREPORT.frmBillPrintDocA5("POS", Batchno.ToString, BillDate.ToString("yyyy-MM-dd"), "")
                    Me.Refresh()
                ElseIf GST And BillPrint_Format = "M4" Then
                    Dim obj As New BrighttechREPORT.frmBillPrintDocB52cpy("POS", Batchno.ToString, BillDate.ToString("yyyy-MM-dd"), "")
                    Me.Refresh()
                ElseIf GST And BillPrintExe = False Then
                    Dim billDoc As New frmBillPrintDoc("POS", Batchno.ToString, BillDate.ToString("yyyy-MM-dd"), "")
                    Me.Refresh()
                Else
                    If IO.File.Exists(Application.StartupPath & "\BillPrint.exe") Then
                        Dim prnmemsuffix As String = ""
                        If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId
                        Dim memfile As String = "\BillPrint" + prnmemsuffix.Trim & ".mem"

                        Dim write As IO.StreamWriter
                        write = IO.File.CreateText(Application.StartupPath & memfile)
                        write.WriteLine(LSet("TYPE", 15) & ":REP")
                        write.WriteLine(LSet("BATCHNO", 15) & ":" & Batchno)
                        write.WriteLine(LSet("TRANDATE", 15) & ":" & BillDate.ToString("yyyy-MM-dd"))
                        write.WriteLine(LSet("DUPLICATE", 15) & ":N")
                        write.Flush()
                        write.Close()
                        If EXE_WITH_PARAM = False Then
                            System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe")
                        Else
                            System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe",
                            LSet("TYPE", 15) & ":REP" & ";" &
                            LSet("BATCHNO", 15) & ":" & Batchno & ";" &
                            LSet("TRANDATE", 15) & ":" & BillDate.ToString("yyyy-MM-dd") & ";" &
                            LSet("DUPLICATE", 15) & ":N")
                        End If

                    Else
                        MsgBox("Billprint exe not found", MsgBoxStyle.Information)
                    End If
                End If
                Me.Close()
                Exit Sub
            Else
                tran = Nothing
                tran = cn.BeginTransaction
                GenBatchNo()
                If Not MANBILLNO Then
                    'Get OrderTranno
                    strSql = " SELECT ISNULL(MAX(CTLTEXT),0)+1 AS REPAIRNO FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = 'REPAIRNO' AND COMPANYID = '" & strCompanyId & "'"
                    TranNo = Val(objGPack.GetSqlValue(strSql, , , tran))
                    ''Update OrderTranno
                    strSql = " UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT = '" & TranNo & "' WHERE CTLID = 'REPAIRNO' AND COMPANYID = '" & strCompanyId & "'"
                    cmd = New OleDbCommand(strSql, cn, tran)
                    cmd.ExecuteNonQuery()
                End If
                RepairNo = GetCostId(BillCostId) & GetCompanyId(strCompanyId) & "R" & Mid(Format(cnTranToDate, "dd/MM/yyyy"), 9, 2).ToString & TranNo

                objAddressDia.InsertIntoPersonalInfo(BillDate, BillCostId, Batchno, tran)

                If defalutDestination <> Nothing Then
                    If IO.Directory.Exists(defalutDestination) Then
                        Try
                            For cnt As Integer = 0 To gridOrder.RowCount - 1
                                With gridOrder.Rows(cnt)
                                    If .Cells("ENTFLAG").Value.ToString = "" Then Continue For
                                    If IO.File.Exists(.Cells("PICTFILE").Value.ToString) Then
                                        Dim iio As New IO.FileInfo(.Cells("PICTFILE").Value.ToString)
                                        Dim extension As String = iio.Extension
                                        Dim destFilePath As String = defalutDestination & RepairNo & "_" & (cnt + 1).ToString + extension
                                        IO.File.Copy(.Cells("PICTFILE").Value.ToString, destFilePath, True)
                                    End If
                                End With
                            Next
                        Catch ex As Exception
                            tran.Rollback()
                            tran.Dispose()
                            tran = Nothing
                            MsgBox(ex.Message)
                            Exit Sub
                        End Try
                    End If
                End If
                For index As Integer = 0 To gridOrder.RowCount - 1
                    With gridOrder.Rows(index)
                        If .Cells("ENTFLAG").Value.ToString <> "Y" Then Continue For
                        InsertOrderDetail(index)
                    End With
                Next
                If InsertAccountsDetail() Then Exit Sub

                Dim balAmt As Double = Val(objGPack.GetSqlValue("SELECT SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) AS BALANCE FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = '" & Batchno & "'", , "0", tran))
                If balAmt <> 0 Then
                    If Not tran Is Nothing Then tran.Dispose()
                    tran = Nothing
                    MsgBox("Credit Debit Not Tally : " + balAmt.ToString, MsgBoxStyle.Information)
                    txtAdjCash_AMT.Focus()
                    Exit Sub
                End If


                tran.Commit()
                tran = Nothing

                MsgBox("RepairNo : " & Mid(RepairNo, 6, 20))
                Dim pBatchno As String = Batchno
                Dim pBilldate As Date = BillDate.ToString("yyyy-MM-dd")
                If SMS_MSG_REPBOOK <> "" Then
                    Dim TempMsg As String = ""
                    TempMsg = SMS_MSG_REPBOOK
                    TempMsg = Replace(SMS_MSG_REPBOOK, vbCrLf, "")
                    TempMsg = Replace(TempMsg, "<NAME>", IIf(objAddressDia.cmbAddressTitle_OWN.Text <> "", objAddressDia.cmbAddressTitle_OWN.Text, "") & " " & objAddressDia.txtAddressName.Text)
                    TempMsg = Replace(TempMsg, "<REPAIRNO>", Mid(RepairNo, 6, 20))
                    TempMsg = Replace(TempMsg, "<REPAIRDATE>", BillDate.ToString("dd.MM.yyyy"))
                    SmsSend(TempMsg, objAddressDia.txtAddressMobile.Text)
                End If
                btnNew_Click(Me, New EventArgs)

                Dim BillPrintExe As Boolean = IIf(GetAdmindbSoftValue("CALLBILLPRINTEXE", "Y") = "Y", True, False)
                Dim BillPrint_Format As String = GetAdmindbSoftValue("BILLPRINT_FORMAT", "")
                If GST And BillPrint_Format = "M1" Then
                    Dim obj As New BrighttechREPORT.frmBillPrintDocA4N("POS", pBatchno.ToString, pBilldate.ToString("yyyy-MM-dd"), "")
                    Me.Refresh()
                ElseIf GST And BillPrint_Format = "M2" Then
                    Dim obj As New BrighttechREPORT.frmBillPrintDocB5("POS", pBatchno.ToString, pBilldate.ToString("yyyy-MM-dd"), "")
                    Me.Refresh()
                ElseIf GST And BillPrint_Format = "M3" Then
                    Dim obj As New BrighttechREPORT.frmBillPrintDocA5("POS", pBatchno.ToString, pBilldate.ToString("yyyy-MM-dd"), "")
                    Me.Refresh()
                ElseIf GST And BillPrint_Format = "M4" Then
                    Dim obj As New BrighttechREPORT.frmBillPrintDocB52cpy("POS", pBatchno.ToString, pBilldate.ToString("yyyy-MM-dd"), "")
                    Me.Refresh()
                ElseIf GST And BillPrintExe = False Then
                    Dim billDoc As New frmBillPrintDoc("POS", pBatchno.ToString, pBilldate.ToString("yyyy-MM-dd"), "")
                    Me.Refresh()
                Else
                    If IO.File.Exists(Application.StartupPath & "\BillPrint.exe") Then
                        Dim prnmemsuffix As String = ""
                        If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId
                        Dim memfile As String = "\BillPrint" + prnmemsuffix.Trim & ".mem"

                        Dim write As IO.StreamWriter
                        write = IO.File.CreateText(Application.StartupPath & memfile)
                        write.WriteLine(LSet("TYPE", 15) & ":REP")
                        write.WriteLine(LSet("BATCHNO", 15) & ":" & pBatchno)
                        write.WriteLine(LSet("TRANDATE", 15) & ":" & pBilldate.ToString("yyyy-MM-dd"))
                        write.WriteLine(LSet("DUPLICATE", 15) & ":N")
                        write.Flush()
                        write.Close()
                        If EXE_WITH_PARAM = False Then
                            System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe")
                        Else
                            System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe",
                            LSet("TYPE", 15) & ":REP" & ";" &
                            LSet("BATCHNO", 15) & ":" & pBatchno & ";" &
                            LSet("TRANDATE", 15) & ":" & pBilldate.ToString("yyyy-MM-dd") & ";" &
                            LSet("DUPLICATE", 15) & ":N")
                        End If

                    Else
                        MsgBox("Billprint exe not found", MsgBoxStyle.Information)

                    End If
                End If
            End If
        Catch ex As OleDbException
            If Not tran Is Nothing Then tran.Rollback()
            If ex.ErrorCode = -2147467259 Then
                GoTo StartTrans
            Else
                MsgBox(ex.Message + vbCrLf + ex.StackTrace)
            End If

        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        Finally
            If Not tran Is Nothing Then tran.Dispose()
        End Try

    End Sub
    Private Sub UpdateRepair()
        With gridOrder.Rows(0)
            ''image ref
            Try
                strSql = " SELECT BATCHNO,ORDATE FROM " & cnAdminDb & "..ORMAST WHERE SNO = '" & RepairUpdSno & "'"
                Batchno = objGPack.GetSqlValue(strSql, "BATCHNO")
                Dim objAddressDia As New frmAddressDia(BillDate, BillCostId, Batchno, True)
                objAddressDia.BackColor = SystemColors.InactiveCaption
                objAddressDia.MaximizeBox = False
                objAddressDia.grpAddress.BackgroundColor = Color.Lavender
                objAddressDia.ShowDialog()
                tran = Nothing
                tran = cn.BeginTransaction
                Dim iio As IO.FileInfo = Nothing
                Dim extension As String = Nothing
                Dim picFileName As String = Nothing
                Dim destFilePath As String = Nothing
                If defalutDestination <> Nothing Then
                    If IO.Directory.Exists(defalutDestination) Then
                        Try
                            For cnt As Integer = 0 To gridOrder.RowCount - 1
                                With gridOrder.Rows(cnt)
                                    If .Cells("ENTFLAG").Value.ToString = "" Then Continue For
                                    If .Cells("STYLENO").Value.ToString <> "" Then Continue For
                                    If IO.File.Exists(.Cells("PICTFILE").Value.ToString) Then
                                        iio = New IO.FileInfo(.Cells("PICTFILE").Value.ToString)
                                        extension = iio.Extension
                                        destFilePath = defalutDestination & RepairNo & "_" & RepairUpdSno + extension
                                        IO.File.Copy(.Cells("PICTFILE").Value.ToString, destFilePath, True)
                                    End If
                                End With
                            Next
                        Catch ex As Exception
                            tran.Rollback()
                            tran.Dispose()
                            tran = Nothing
                            MsgBox(ex.Message)
                            Exit Sub
                        End Try
                    End If
                End If
                If .Cells("PICTFILE").Value.ToString <> "" And defalutDestination <> "" Then
                    If IO.Directory.Exists(defalutDestination) Then
                        iio = New IO.FileInfo(.Cells("PICTFILE").Value.ToString)
                        extension = iio.Extension
                        picFileName = iio.Name
                        If .Cells("STYLENO").Value.ToString = "" Then
                            picFileName = RepairNo & "_" & RepairUpdSno + extension
                            destFilePath = defalutDestination & picFileName
                        End If
                    End If
                End If
                strSql = vbCrLf + " UPDATE " & cnAdminDb & "..ORMAST SET"
                strSql += vbCrLf + " ITEMID = " & Val(objGPack.GetSqlValue(" SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Cells("ITEMNAME").Value.ToString & "'", , , tran)) & "" 'ITEMID
                strSql += vbCrLf + " ,SUBITEMID = " & Val(objGPack.GetSqlValue(" SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & .Cells("SUBITEMNAME").Value.ToString & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Cells("ITEMNAME").Value.ToString & "')", , , tran)) & "" 'SUBITEMID
                If ORDDATE_EDIT Then
                    strSql += vbCrLf + " ,ORDATE='" & BillDate.ToString("yyyy-MM-dd") & "'" 'ORDATE
                    strSql += vbCrLf + " ,REMDATE='" & dtpRemDate.Value.Date.ToString("yyyy-MM-dd") & "'" 'REMDATE
                    strSql += vbCrLf + " ,DUEDATE='" & dtpDueDate.Value.Date.ToString("yyyy-MM-dd") & "'" 'DUEDATE
                End If
                strSql += vbCrLf + " ,DESCRIPT = '" & .Cells("DESCRIPTION").Value.ToString & "'" 'DESCRIPT
                strSql += vbCrLf + " ,PCS = " & Val(.Cells("PCS").Value.ToString) & "" 'PCS
                strSql += vbCrLf + " ,GRSWT = " & Val(.Cells("GRSWT").Value.ToString) & "" 'GRSWT
                strSql += vbCrLf + " ,NETWT = " & Val(.Cells("NETWT").Value.ToString) & "" 'NETWT
                strSql += vbCrLf + " ,SIZEID = " & Val(objGPack.GetSqlValue("SELECT SIZEID FROM " & cnAdminDb & "..ItemSize WHERE SIZENAME = '" & .Cells("SIZENAME").Value.ToString & "'", , , tran)) & "" 'SIZEID                
                strSql += vbCrLf + " ,MCGRM = " & Val(.Cells("MCGRM").Value.ToString) & "" 'MCGRM
                strSql += vbCrLf + " ,MC = " & Val(.Cells("MC").Value.ToString) & "" 'MC
                strSql += vbCrLf + " ,ORVALUE = " & Val(.Cells("GROSSAMT").Value.ToString) & "" 'ORVALUE
                strSql += vbCrLf + " ,PICTFILE = '" & picFileName & "'" 'PICTFILE
                strSql += vbCrLf + " WHERE SNO = '" & RepairUpdSno & "'"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId, , destFilePath)

                ''Stone
                strSql = " DELETE FROM " & cnAdminDb & "..ORSTONE"
                strSql += " WHERE ORSNO = '" & RepairUpdSno & "'"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)
                For Each stRow As DataRow In dtStoneDetails.Rows
                    If .Cells("KEYNO").Value = stRow("KEYNO") Then
                        InsertStoneDetails(stRow, RepairUpdSno)
                    End If
                Next

                ''Additional Details
                strSql = " DELETE FROM " & cnAdminDb & "..ORADTRAN"
                strSql += " WHERE ORSNO = '" & RepairUpdSno & "'"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)
                For Each stRow As DataRow In dtOrderAdditionalDetails.Rows
                    If .Cells("KEYNO").Value = stRow("KEYNO") Then
                        InsertOrderAdditionalDetails(TranNo, RepairUpdSno, "RE", BillCostId, strCompanyId, stRow, Batchno, tran)
                    End If
                Next

                tran.Commit()
                tran = Nothing
            Catch ex As Exception
                If tran IsNot Nothing Then tran.Rollback()
                MsgBox(ex.Message + vbCrLf + ex.StackTrace)
            End Try
        End With
        MsgBox("Updated")
    End Sub
    Private Sub txtSalesMan_NUM_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSalesMan_NUM.GotFocus
        Main.ShowHelpText("Press Insert Key to Help")
    End Sub

    Private Sub txtSalesMan_NUM_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtSalesMan_NUM.KeyDown
        If e.KeyCode = Keys.Insert Then
            LoadSalesMan()
        End If
    End Sub

    Private Sub LoadSalesMan()
        strSql = " SELECT EMPID,EMPNAME FROM " & cnAdminDb & "..EMPMASTER "
        strSql += " WHERE ACTIVE = 'Y'"
        Dim row As DataRow = Nothing
        row = BrighttechPack.SearchDialog.Show_R("Select Employee Name", strSql, cn, 1)
        If Not row Is Nothing Then
            txtSalesMan_NUM.Text = row!EMPID.ToString
            txtSalesManName.Text = row!EMPNAME.ToString
            Me.SelectNextControl(txtSalesMan_NUM, True, True, True, True)
        End If
    End Sub

    Private Sub ClearItemDetails()
        calcType = Nothing
        subItemName = Nothing
        sizeName = Nothing
        isStone = False

        txtOPcs_NUM.Clear()
        txtOGrsWt_WET.Clear()
        txtONetWt_WET.Clear()
        txtOMc_AMT.Clear()
        txtOAmount_AMT.Clear()
        txtODescription.Clear()
        txtOMcPerGrm_AMT.Clear()

        objStone = New frmStoneDia
        'dtGridStone.Rows.Clear()
        'CalcStoneWtAmount()
    End Sub

    Private Function GetOrderRate() As String
        Dim rate As Double = Val(GetRate(BillDate, objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtOItemName.Text & "'")))
        Return IIf(rate <> 0, Format(rate, "0.00"), Nothing)
    End Function

    Private Sub LoadStoneItemName()
        strSql = " SELECT"
        strSql += " ITEMID AS ITEMID,ITEMNAME AS ITEMNAME"
        strSql += " FROM " & cnAdminDb & "..ITEMMAST"
        strSql += " WHERE ISNULL(STUDDED,'') = 'S' AND ACTIVE = 'Y'"
        Dim itemName As String = BrighttechPack.SearchDialog.Show("Find ItemName", strSql, cn, 1, 1, , txtStItem.Text)
        If itemName <> "" Then
            txtStItem.Text = itemName
            LoadStoneitemDetails()
        Else
            txtStItem.Focus()
            txtStItem.SelectAll()
        End If
    End Sub

    Private Sub LoadStoneitemDetails()
        If objGPack.GetSqlValue("SELECT SUBITEM FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "'") = "Y" Then
            Dim DefItem As String = txtStSubItem.Text
            Dim iId As Integer = Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "'"))
            If Not _SubItemOrderByName Then
                DefItem = objGPack.GetSqlValue("SELECT DISPLAYORDER FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & txtStSubItem.Text & "' AND ITEMID = " & iId & "")
            End If
            strSql = GetSubItemQry(New String() {"SUBITEMID ID", "DISPLAYORDER DISP", "SUBITEMNAME SUBITEM"}, iId)
            txtStSubItem.Text = BrighttechPack.SearchDialog.Show("Search SubItem", strSql, cn, IIf(_SubItemOrderByName, 2, 1), 2, , DefItem, , False, True)
            'Dim qry As String = "SELECT SUBITEMID ID,SUBITEMNAME SUBITEM FROM " & cnAdminDb & "..SUBITEMMAST "
            'qry += " WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "')"
            'qry += " AND ACTIVE = 'Y'"
            'txtStSubItem.Text = BrighttechPack.SearchDialog.Show("Search SubItem", qry, cn, 1, 1, , txtStSubItem.Text, , False, True)
        End If

        If txtStSubItem.Text <> "" Then
            strSql = " SELECT CALTYPE FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & txtStSubItem.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "')"
        Else
            strSql = " SELECT CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "'"
        End If
        Dim calType As String = objGPack.GetSqlValue(strSql, , , tran)
        cmbStCalc.Text = IIf(calType = "R", "P", "W")

        strSql = " SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "'"
        txtStMetalCode.Text = objGPack.GetSqlValue(strSql, "DIASTONE", , tran)
        If txtStMetalCode.Text = "T" Then cmbStUnit.Text = "G" Else cmbStUnit.Text = "C"
        Me.SelectNextControl(txtStItem, True, True, True, True)
    End Sub

    Private Sub LoadItemDetails()
        If UCase(objGPack.GetSqlValue("SELECT SUBITEM FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtOItemName.Text & "'")) = "Y" Then
            Dim DefItem As String = subItemName
            Dim iId As Integer = Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtOItemName.Text & "'"))
            If Not _SubItemOrderByName Then
                DefItem = objGPack.GetSqlValue("SELECT DISPLAYORDER FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & subItemName & "' AND ITEMID = " & iId & "")
            End If
            strSql = GetSubItemQry(New String() {"SUBITEMID ID", "DISPLAYORDER DISP", "SUBITEMNAME SUBITEM"}, iId)
            subItemName = BrighttechPack.SearchDialog.Show("Search SubItem", strSql, cn, IIf(_SubItemOrderByName, 2, 1), 2, , DefItem, , False, True)
            'Dim qry As String = "SELECT SUBITEMID ID,SUBITEMNAME SUBITEM FROM " & cnAdminDb & "..SUBITEMMAST "
            'qry += " WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtOItemName.Text & "')"
            'qry += " AND ACTIVE = 'Y'"
            'subItemName = BrighttechPack.SearchDialog.Show("Search SubItem", qry, cn, 1, 1, , subItemName, , False, True)
        End If
        If UCase(objGPack.GetSqlValue("SELECT SIZESTOCK FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtOItemName.Text & "'")) = "Y" Then
            Dim qry As String = " SELECT SIZENAME FROM " & cnAdminDb & "..ITEMSIZE WHERE ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = 'BANGLE')"
            sizeName = BrighttechPack.SearchDialog.Show("Search SizeName", qry, cn, 0, 0, , sizeName, , False, True)
        End If
        If subItemName <> Nothing Then
            strSql = " SELECT STUDDEDSTONE FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & subItemName & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtOItemName.Text & "')"
        Else
            strSql = " SELECT STUDDEDSTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtOItemName.Text & "'"
        End If
        If UCase(objGPack.GetSqlValue(strSql)) = "Y" Then
            isStone = True
        Else
            isStone = False
        End If
        If txtODescription.Text = "" Then
            txtODescription.Text = IIf(subItemName <> "", subItemName, txtOItemName.Text)
        End If
        txtODescription.Focus()
        'Me.SelectNextControl(txtOItemName, True, True, True, True)
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
        strSql += GetItemQryFilteration()
        Dim itemName As String = BrighttechPack.SearchDialog.Show("Find ItemName", strSql, cn, 1, 1, , txtOItemName.Text)
        If itemName <> "" Then
            txtOItemName.Text = itemName
            LoadItemDetails()
        Else
            txtOItemName.Focus()
            txtOItemName.SelectAll()
        End If
    End Sub
    Private Sub LoadItemName_id()
        strSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST"
        strSql += " WHERE ITEMID='" & Val(txtOItemName.Text) & "' AND ISNULL(STUDDED,'') <> 'S' AND ACTIVE = 'Y'"
        strSql += GetItemQryFilteration()
        Dim itemName As String = objGPack.GetSqlValue(strSql, , , )
        If itemName <> "" Then
            txtOItemName.Text = itemName
            LoadItemDetails()
        Else
            txtOItemName.Focus()
            txtOItemName.SelectAll()
        End If
    End Sub

    Private Sub txtSalesMan_NUM_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtSalesMan_NUM.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtSalesMan_NUM.Text = "" Then
                LoadSalesMan()
            ElseIf txtSalesMan_NUM.Text <> "" Then
                txtSalesManName.Text = objGPack.GetSqlValue("SELECT EMPNAME FROM " & cnAdminDb & "..EMPMASTER WHERE EMPID = " & Val(txtSalesMan_NUM.Text) & "")
                If txtSalesManName.Text = Nothing Then
                    LoadSalesMan()
                Else
                    Me.SelectNextControl(txtSalesMan_NUM, True, True, True, True)
                End If
            End If
        End If
    End Sub

    Private Sub txtSalesMan_NUM_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSalesMan_NUM.LostFocus
        Main.HideHelpText()
    End Sub

    Private Sub txtSalesManName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSalesManName.GotFocus
        SendKeys.Send("{TAB}")
    End Sub

    Private Sub txtOItemName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOItemName.GotFocus
        Main.ShowHelpText("Press Insert Key to Help")
    End Sub

    Private Sub txtOItemName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtOItemName.KeyDown
        If e.KeyCode = Keys.Insert Then
            LoadItemName()
        End If
    End Sub

    Private Sub txtOItemName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtOItemName.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtOItemName.Text = "" Then
                LoadItemName()
            ElseIf IsNumeric(txtOItemName.Text) Then
                LoadItemName_id()
            ElseIf txtOItemName.Text <> "" And objGPack.DupCheck("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtOItemName.Text & "'" & GetItemQryFilteration()) = False Then
                LoadItemName()
            Else
                LoadItemDetails()
            End If
        End If
    End Sub

    Private Sub txtOItemName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOItemName.LostFocus
        Main.HideHelpText()
    End Sub

    Private Sub txtOItemName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOItemName.TextChanged
        ClearItemDetails()
    End Sub

    Private Sub txtOGrsWt_WET_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtOGrsWt_WET.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            ShowStoneDia()
            ShowRepairAdditionalDetails()
            Me.SelectNextControl(txtOGrsWt_WET, True, True, True, True)
        End If
    End Sub

    Private Sub ShowStoneDia()
        If objStone.Visible Then Exit Sub
        If Not isStone Then Exit Sub
        objStone.grsWt = Val(txtOGrsWt_WET.Text)
        objStone.BackColor = pnlContainer_OWN.BackColor
        objStone.StartPosition = FormStartPosition.CenterScreen
        objStone.MaximizeBox = False
        objStone.grpStone.BackgroundColor = grpHeader.BackgroundColor
        objStone.StyleGridStone(objStone.gridStone)
        objStone.txtStItem.Select()
        objStone.ShowDialog()
        Dim stnWt As Double = Val(objStone.gridStoneTotal.Rows(0).Cells("WEIGHT").Value.ToString)
        Dim stnAmt As Double = Val(objStone.gridStoneTotal.Rows(0).Cells("AMOUNT").Value.ToString)
        Dim ntWt As Double = Val(txtOGrsWt_WET.Text) - stnWt
        txtONetWt_WET.Text = IIf(ntWt <> 0, Format(ntWt, "0.000"), Nothing)
        Me.SelectNextControl(txtOGrsWt_WET, True, True, True, True)
    End Sub

    Private Sub txtOGrsWt_WET_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOGrsWt_WET.TextChanged
        CalcMc()
        CalcStoneWtAmount()
    End Sub

    Private Sub txtONetWt_WET_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtONetWt_WET.TextChanged
        CalcMc()
    End Sub

    Private Sub txtOMc_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOMc_AMT.TextChanged
        txtOAmount_AMT.Text = IIf(Val(txtOMc_AMT.Text) > 0, Format(Val(txtOMc_AMT.Text), "0.00"), Nothing)
    End Sub

    Private Sub LoadOrderDetail()
        If objGPack.GetSqlValue("SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtOItemName.Text & "'") = "" Then
            MsgBox("Invalid ItemName", MsgBoxStyle.Information)
            txtOItemName.Focus()
            Exit Sub
        End If
        If Not Val(txtOGrsWt_WET.Text) > 0 Then
            MsgBox("Grs Weight Should Not Emtpy", MsgBoxStyle.Information)
            txtOGrsWt_WET.Focus()
            Exit Sub
        End If
        If Not Val(txtONetWt_WET.Text) > 0 Then
            MsgBox("NetWeight Should Not Empty", MsgBoxStyle.Information)
            txtONetWt_WET.Focus()
            Exit Sub
        End If
        If txtODescription.Text = "" Then
            MsgBox("Description Should Not Empty", MsgBoxStyle.Information)
            txtODescription.Focus()
            Exit Sub
        End If
        If txtONatureOfRepair.Text = "" Then
            MsgBox("Nature Of Repair Should Not Empty", MsgBoxStyle.Information)
            txtONatureOfRepair.Focus()
            Exit Sub
        End If
        Dim index As Integer = 0
        If txtOrderRowIndex.Text = "" Then
            For i As Integer = 0 To dtGridRepair.Rows.Count - 1
                With dtGridRepair.Rows(i)
                    If .Item("ENTFLAG").ToString <> "Y" Then
                        .Item("ITEMNAME") = txtOItemName.Text
                        .Item("DESCRIPTION") = txtODescription.Text
                        .Item("PCS") = IIf(Val(txtOPcs_NUM.Text) > 0, txtOPcs_NUM.Text, DBNull.Value)
                        .Item("GRSWT") = IIf(Val(txtOGrsWt_WET.Text) > 0, txtOGrsWt_WET.Text, DBNull.Value)
                        .Item("NETWT") = IIf(Val(txtONetWt_WET.Text) > 0, txtONetWt_WET.Text, DBNull.Value)
                        .Item("MCGRM") = Val(txtOMcPerGrm_AMT.Text)
                        .Item("MC") = IIf(Val(txtOMc_AMT.Text) > 0, txtOMc_AMT.Text, DBNull.Value)
                        .Item("GROSSAMT") = IIf(Val(txtOAmount_AMT.Text) > 0, txtOAmount_AMT.Text, DBNull.Value)
                        .Item("REASON") = txtONatureOfRepair.Text
                        .Item("ENTFLAG") = "Y"
                        .Item("SUBITEMNAME") = subItemName
                        .Item("SIZENAME") = sizeName
                        .Item("STONEAMT") = Val(objStone.gridStoneTotal.Rows(0).Cells("AMOUNT").Value.ToString)
                        .Item("LESSWT") = Val(txtOGrsWt_WET.Text) - Val(txtONetWt_WET.Text)
                        .Item("PICTFILE") = PICPATH
                        gridOrder.CurrentCell = gridOrder.Rows(i).Cells("ITEMNAME")
                        index = i
                        dtGridRepair.Rows.Add()
                        Exit For
                    End If
                End With
            Next
        End If
        dtGridRepair.AcceptChanges()
        CalcGridOrderTotal()

        ''Stone
        For rwIndex As Integer = 0 To objStone.dtGridStone.Rows.Count - 1
            Dim ro As DataRow = dtStoneDetails.NewRow
            ro("KEYNO") = dtGridRepair.Rows(index).Item("KEYNO").ToString
            For colIndex As Integer = 2 To objStone.dtGridStone.Columns.Count - 1
                ro(colIndex) = objStone.dtGridStone.Rows(rwIndex).Item(colIndex)
            Next
            dtStoneDetails.Rows.Add(ro)
        Next
        dtStoneDetails.AcceptChanges()

        ''order Additional Details
        For rwIndex As Integer = 0 To objAddtionalDetails.gridView.Rows.Count - 1
            Dim ro As DataRow = dtOrderAdditionalDetails.NewRow
            ro("KEYNO") = dtGridRepair.Rows(index).Item("KEYNO").ToString
            For colIndex As Integer = 1 To objAddtionalDetails.gridView.Columns.Count - 1
                ro(colIndex) = objAddtionalDetails.DtView.Rows(rwIndex).Item(colIndex)
            Next
            dtOrderAdditionalDetails.Rows.Add(ro)
        Next
        dtOrderAdditionalDetails.AcceptChanges()

        ''Clear
        calcType = Nothing
        objGPack.TextClear(grpOrderDetails)
        objAddtionalDetails.DtView.Clear()
        PICPATH = ""
        chkOImage.Checked = False
        Label1.Focus()
        'txtOItemName.Focus()
    End Sub


    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub gridOrder_UserDeletedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles gridOrder.UserDeletedRow
        dtGridRepair.AcceptChanges()
        CalcGridOrderTotal()
    End Sub

    Private Sub gridOrder_UserDeletingRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowCancelEventArgs) Handles gridOrder.UserDeletingRow
        If gridOrder.Rows(e.Row.Index).Cells("EntFlag").Value.ToString <> "Y" Then
            dtGridRepair.Rows.Add()
            dtGridRepair.AcceptChanges()
        End If
    End Sub
    Private Sub txtOMcPerGrm_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOMcPerGrm_AMT.TextChanged
        CalcMc()
    End Sub

    Private Sub txtAdjAdvance_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAdjAdvance_AMT.GotFocus
        If objAdvance.Visible Then Exit Sub
        objAdvance.BackColor = pnlContainer_OWN.BackColor
        objAdvance.StartPosition = FormStartPosition.CenterScreen
        objAdvance.grpAdvance.BackgroundColor = grpHeader.BackgroundColor
        If objAdvance.ShowDialog() = Windows.Forms.DialogResult.OK Then
            If objAddressDia.txtAddressRegularSno.Text <> "" Then '  objAdvance.dtGridAdvance.Rows.Count > 0 Then
                GetAdvanceAddress()
            End If
            Dim advAmt As Double = Val(objAdvance.gridAdvanceTotal.Rows(0).Cells("AMOUNT").Value.ToString)
            txtAdjAdvance_AMT.Text = IIf(advAmt <> 0, Format(advAmt, "0.00"), Nothing)
            txtAdjCash_AMT.Focus()
        End If


    End Sub

    Private Sub txtAdjCheque_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAdjCheque_AMT.GotFocus
        If objCheaque.Visible Then Exit Sub
        objCheaque.BackColor = pnlContainer_OWN.BackColor
        objCheaque.StartPosition = FormStartPosition.CenterScreen
        objCheaque.grpCheque.BackgroundColor = grpHeader.BackgroundColor
        objCheaque.ShowDialog()
        Dim chqAmt As Double = Val(objCheaque.gridChequeTotal.Rows(0).Cells("AMOUNT").Value.ToString)
        txtAdjCheque_AMT.Text = IIf(chqAmt <> 0, Format(chqAmt, "0.00"), Nothing)
        txtAdjCash_AMT.Focus()

    End Sub

    Private Sub txtAdjCreditCard_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAdjCreditCard_AMT.GotFocus
        If objCreditCard.Visible Then Exit Sub
        objCreditCard.BackColor = pnlContainer_OWN.BackColor
        objCreditCard.StartPosition = FormStartPosition.CenterScreen
        objCreditCard.grpCreditCard.BackgroundColor = grpHeader.BackgroundColor
        objCreditCard.ShowDialog()
        Dim cardAmt As Double = Val(objCreditCard.gridCreditCardTotal.Rows(0).Cells("AMOUNT").Value.ToString)
        txtAdjCreditCard_AMT.Text = IIf(cardAmt <> 0, Format(cardAmt, "0.00"), Nothing)
        txtAdjCash_AMT.Focus()


    End Sub

    Private Sub txtAddressFax_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If e.KeyChar = Chr(Keys.Enter) Then
            txtAdjCash_AMT.Focus()
        End If
    End Sub

    Private Function InsertAccountsDetail() As Boolean
        Dim ContraCode As String = Nothing 'objGPack.GetSqlValue("SELECT TOP 1 ACCODE FROM " & CNSTOCKDB & "..ACCTRAN WHERE BATCHNO = '" & BatchNo & "' AND TRANMODE = '" & IIf(Val(txtAdjReceive_AMT.Text) > 0, "C", "D") & "' ORDER BY SNO", , , tran)
        OR_REP_NewCatCode = objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = '" & GetAdmindbSoftValue("REPAIR_NEW_CAT", "00012", tran) & "'", , "00012", tran)
        If objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & gridOrder.Rows(0).Cells("ITEMNAME").Value.ToString & "'", , "", tran) = "S" Then
            OR_REP_NewCatCode = objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = '" & GetAdmindbSoftValue("REPAIR_NEW_CAT_S", "00014", tran) & "'", , "00014", tran)
        End If
        Dim amount As Double
        amount = Val(txtAdjAdvance_AMT.Text)
        amount += Val(txtAdjChitCard_AMT.Text)
        amount += Val(txtAdjCreditCard_AMT.Text)
        amount += Val(txtAdjCheque_AMT.Text)
        amount += Val(txtAdjCash_AMT.Text)
        If amount <> 0 Then
            InsertIntoAccTran(TranNo, IIf(amount > 0, "C", "D"), _
            IIf(objAddressDia.txtAddressPartyCode.Text <> "", objAddressDia.txtAddressPartyCode.Text, "ADVREP"), amount, 0, 0, 0, "OR", ContraCode)
            InsertIntoOustanding("A", RepairNo, amount, "R", "OR", , , , , , , , , , , IIf(objAddressDia.txtAddressPartyCode.Text <> "", objAddressDia.txtAddressPartyCode.Text, "ADVREP"))
        End If

        ''Cash Transaction
        InsertIntoAccTran(TranNo, IIf(Val(txtAdjCash_AMT.Text) > 0, "D", "C"), _
        CASHID, Val(txtAdjCash_AMT.Text), 0, 0, 0, "CA", ContraCode)
        ''SCHEME Trans
        If Val(txtAdjChitCard_AMT.Text) <> 0 Then
            If objChitCard.InsertChitCardDetail(Batchno, TranNo, BillDate, BillCashCounterId, BillCostId, VATEXM, tran, "R", False) Then Return True
        End If
        ''Advance Trans
        For Each ro As DataRow In objAdvance.dtGridAdvance.Rows
            InsertIntoAccTran(TranNo, IIf(Val(ro!AMOUNT.ToString) > 0, "D", "C"), _
            IIf(ro!ACCODE.ToString <> "", ro!ACCODE.ToString, "ADVANCE"), Val(ro!AMOUNT.ToString), 0, 0, 0, "AA", ContraCode, , , , ro!DATE.ToString)
            InsertIntoOustanding("A", ro!RUNNO.ToString, Val(ro!AMOUNT.ToString), "P", "AA", , , , , , ro!REFNO.ToString, ro!DATE.ToString, , , , IIf(ro!ACCODE.ToString <> "", ro!ACCODE.ToString, "ADVANCE"))
        Next
        ''Advance Wt
        'InsertIntoOustanding("A", RepairNo, 0, "R", "AR", Val(txtAdjAdvanceWt.Text), Val(txtAdjAdvanceWt.Text), "00015", , , , , , , , IIf(objAddressDia.txtAddressPartyCode.Text <> "", objAddressDia.txtAddressPartyCode.Text, "ADVREP"))
        InsertIntoOustanding("A", RepairNo, 0, "R", "AR", Val(txtAdjAdvanceWt.Text), Val(txtAdjAdvanceWt.Text), OR_REP_NewCatCode, , , , , , , , IIf(objAddressDia.txtAddressPartyCode.Text <> "", objAddressDia.txtAddressPartyCode.Text, "ADVREP"))


        ''CreditCard Trans
        For Each ro As DataRow In objCreditCard.dtGridCreditCard.Rows
            InsertIntoAccTran(TranNo, IIf(Val(ro!AMOUNT.ToString) > 0, "D", "C"), _
            objGPack.GetSqlValue(" SELECT ACCODE FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & ro!CARDTYPE.ToString & "'", , , tran) _
            , Val(ro!AMOUNT.ToString), 0, 0, 0, "CC", ContraCode, _
            , , ro!CARDNO.ToString, ro!DATE.ToString, _
             objGPack.GetSqlValue(" SELECT CARDCODE FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & ro!CARDTYPE.ToString & "'", , , tran), _
            ro!APPNO.ToString _
            )

            Dim commision As Double = Format(Val(ro!AMOUNT.ToString) * (Val(objGPack.GetSqlValue(" SELECT COMMISSION FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & ro!CARDTYPE.ToString & "'", , , tran)) / 100), "0.00")
            If commision <> 0 Then
                InsertIntoAccTran(TranNo, IIf(Val(ro!AMOUNT.ToString) > 0, "C", "D"), _
                objGPack.GetSqlValue(" SELECT ACCODE FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & ro!CARDTYPE.ToString & "'", , , tran) _
                , commision, 0, 0, 0, "BC", ContraCode, _
                , , , , )

                InsertIntoAccTran(TranNo, IIf(Val(ro!AMOUNT.ToString) > 0, "D", "C"), _
                "BANKC" _
                , commision, 0, 0, 0, "BC", ContraCode, _
                , , , , )

                Dim sCharge As Double = Format(commision * (Val(objGPack.GetSqlValue(" SELECT SURCHARGE FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & ro!CARDTYPE.ToString & "'", , , tran)) / 100), "0.00")
                If sCharge <> 0 Then
                    InsertIntoAccTran(TranNo, IIf(Val(ro!AMOUNT.ToString) > 0, "C", "D"), _
                    objGPack.GetSqlValue(" SELECT ACCODE FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & ro!CARDTYPE.ToString & "'", , , tran) _
                    , commision, 0, 0, 0, "BS", ContraCode, _
                    , , , , )

                    InsertIntoAccTran(TranNo, IIf(Val(ro!AMOUNT.ToString) > 0, "D", "C"), _
                    "BANKS" _
                    , commision, 0, 0, 0, "BS", ContraCode, _
                    , , , , )
                End If
            End If
        Next
        ''Cheque Trans
        For Each ro As DataRow In objCheaque.dtGridCheque.Rows
            InsertIntoAccTran(TranNo, IIf(Val(ro!AMOUNT.ToString) > 0, "D", "C"), _
            objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & ro!DEFAULTBANK.ToString & "'", , , tran), _
            Val(ro!AMOUNT.ToString), 0, 0, 0, "CH", ContraCode, _
            , , ro!CHEQUENO.ToString, ro!DATE.ToString, , ro!BANKNAME.ToString)
        Next

        ''UPDATE CONTRA
        strSql = " UPDATE " & cnStockDb & "..ACCTRAN SET CONTRA = "
        strSql += " (SELECT TOP 1 ACCODE FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = T.BATCHNO AND TRANMODE = 'C'  AND ACCODE <> T.ACCODE ORDER BY SNO)"
        strSql += " FROM " & cnStockDb & "..ACCTRAN AS T"
        strSql += " WHERE TRANDATE = '" & GetEntryDate(BillDate, tran).ToString("yyyy-MM-dd") & "' AND BATCHNO = '" & Batchno & "' AND TRANMODE = 'D'"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)
        strSql = " UPDATE " & cnStockDb & "..ACCTRAN SET CONTRA = "
        strSql += " (SELECT TOP 1 ACCODE FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = T.BATCHNO AND TRANMODE = 'D'  AND ACCODE <> T.ACCODE ORDER BY SNO)"
        strSql += " FROM " & cnStockDb & "..ACCTRAN AS T"
        strSql += " WHERE TRANDATE = '" & GetEntryDate(BillDate, tran).ToString("yyyy-MM-dd") & "' AND BATCHNO = '" & Batchno & "' AND TRANMODE = 'C'"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)
    End Function
    Private Sub InsertIntoAccTran _
  (ByVal tNo As Integer, _
  ByVal tranMode As String, _
  ByVal accode As String, _
  ByVal amount As Double, _
  ByVal pcs As Integer, _
  ByVal grsWT As Double, _
  ByVal netWT As Double, _
  ByVal payMode As String, _
  ByVal contra As String, _
  Optional ByVal refNo As String = Nothing, Optional ByVal refDate As String = Nothing, _
  Optional ByVal chqCardNo As String = Nothing, _
  Optional ByVal chqDate As String = Nothing, _
  Optional ByVal chqCardId As Integer = Nothing, _
  Optional ByVal chqCardRef As String = Nothing _
  )
        If amount = 0 Then Exit Sub
        strSql = " INSERT INTO " & cnStockDb & "..ACCTRAN"
        strSql += " ("
        strSql += " SNO,TRANNO,TRANDATE,TRANMODE,ACCODE"
        strSql += " ,AMOUNT,PCS,GRSWT,NETWT"
        strSql += " ,REFNO,REFDATE,PAYMODE,CHQCARDNO"
        strSql += " ,CARDID,CHQCARDREF,CHQDATE,BRSFLAG"
        strSql += " ,RELIASEDATE,FROMFLAG,REMARK1,REMARK2"
        strSql += " ,CONTRA,BATCHNO,USERID,UPDATED"
        strSql += " ,UPTIME,SYSTEMID,CASHID,COSTID,VATEXM,APPVER,COMPANYID"
        strSql += " )"
        strSql += " VALUES"
        strSql += " ("
        strSql += " '" & GetNewSno(TranSnoType.ACCTRANCODE, tran) & "'" ''SNO
        strSql += " ," & tNo & "" 'TRANNO 
        strSql += " ,'" & GetEntryDate(BillDate, tran).ToString("yyyy-MM-dd") & "'" 'TRANDATE
        strSql += " ,'" & tranMode & "'" 'TRANMODE
        strSql += " ,'" & accode & "'" 'ACCODE
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
        strSql += " ,'R'" 'FROMFLAG
        strSql += " ,''" 'REMARK1
        strSql += " ,''" 'REMARK2
        strSql += " ,'" & contra & "'" 'CONTRA
        strSql += " ,'" & BatchNo & "'" 'BATCHNO
        strSql += " ,'" & userId & "'" 'USERID
        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
        strSql += " ,'" & systemId & "'" 'SYSTEMID
        strSql += " ,'" & BillCashCounterId & "'" 'CASHID
        strSql += " ,'" & BillCostId & "'" 'COSTID
        strSql += " ,'" & VATEXM & "'" 'VATEXM
        strSql += " ,'" & VERSION & "'" 'APPVER
        strSql += " ,'" & strCompanyId & "'" 'COMPANYID
        strSql += " )"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)
        strSql = ""
        cmd = Nothing
    End Sub
    Private Sub InsertIntoOustanding _
   ( _
   ByVal tType As String, ByVal RunNo As String, ByVal Amount As Double, _
   ByVal RecPay As String, _
   ByVal Paymode As String, _
   Optional ByVal GrsWt As Double = 0, _
   Optional ByVal NetWt As Double = 0, _
   Optional ByVal CatCode As String = Nothing, _
   Optional ByVal Rate As Double = Nothing, _
   Optional ByVal Value As Double = Nothing, _
   Optional ByVal refNo As String = Nothing, _
   Optional ByVal refDate As String = Nothing, _
   Optional ByVal purity As Double = Nothing, _
   Optional ByVal proId As Integer = Nothing, _
   Optional ByVal dueDate As String = Nothing, _
   Optional ByVal aCCode As String = Nothing _
   )
        If Amount = 0 And GrsWt = 0 Then Exit Sub
        strSql = " INSERT INTO " & cnAdminDb & "..OUTSTANDING"
        strSql += " ("
        strSql += " SNO,TRANNO,TRANDATE,TRANTYPE,RUNNO"
        strSql += " ,AMOUNT,GRSWT,NETWT,RECPAY"
        strSql += " ,REFNO,REFDATE,EMPID"
        strSql += " ,TRANSTATUS,PURITY,CATCODE,BATCHNO"
        strSql += " ,USERID,UPDATED,UPTIME,SYSTEMID"
        strSql += " ,RATE,VALUE,CASHID,VATEXM,REMARK1,REMARK2,CTRANCODE,DUEDATE,APPVER,COMPANYID,ACCODE,COSTID,PAYMODE,FROMFLAG)"
        strSql += " VALUES"
        strSql += " ("
        strSql += " '" & GetNewSno(TranSnoType.OUTSTANDINGCODE, tran, "GET_ADMINSNO_TRAN") & "'" ''SNO
        strSql += " ," & TranNo & "" 'TRANNO
        strSql += " ,'" & GetEntryDate(BillDate, tran).ToString("yyyy-MM-dd") & "'" 'TRANDATE
        strSql += " ,'" & tType & "'" 'TRANTYPE
        strSql += " ,'" & RunNo & "'" 'RUNNO
        strSql += " ," & Math.Abs(Amount) & "" 'AMOUNT
        strSql += " ," & Math.Abs(GrsWt) & "" 'GRSWT
        strSql += " ," & Math.Abs(NetWt) & "" 'NETWT
        strSql += " ,'" & RecPay & "'" 'RECPAY
        strSql += " ,'" & refNo & "'" 'REFNO
        If refDate <> Nothing Then
            strSql += " ,'" & refDate & "'" 'REFDATE
        Else
            strSql += " ,NULL" 'REFDATE
        End If

        strSql += " ,0" 'EMPID
        strSql += " ,''" 'TRANSTATUS
        strSql += " ," & purity & "" 'PURITY
        strSql += " ,'" & CatCode & "'" 'CATCODE
        strSql += " ,'" & Batchno & "'" 'BATCHNO
        strSql += " ," & userId & "" 'USERID

        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
        strSql += " ,'" & systemId & "'" 'SYSTEMID
        strSql += " ," & Rate & "" 'RATE
        strSql += " ," & Value & "" 'VALUE
        strSql += " ,'" & BillCashCounterId & "'" 'CASHID
        strSql += " ,'" & VATEXM & "'" 'VATEXM
        strSql += " ,''" 'REMARK1
        strSql += " ,''" 'REMARK1
        strSql += " ," & proId & "" 'CTRANCODE
        If dueDate <> Nothing Then
            strSql += " ,'" & dueDate & "'" 'DUEDATE
        Else
            strSql += " ,NULL" 'DUEDATE
        End If
        strSql += " ,'" & VERSION & "'" 'APPVER
        strSql += " ,'" & strCompanyId & "'" 'COMPANYID
        strSql += " ,'" & aCCode & "'" 'ACCODE
        strSql += " ,'" & BillCostId & "'" 'COSTID
        strSql += " ,'" & Paymode & "'" 'PAYMODE
        strSql += " ,'R'" 'FROMFLAG
        strSql += " )"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)
    End Sub
    Public Sub InsertIntoReceipt(ByVal purRo As DataRow)
        With purRo
            Dim puEx As String = Nothing
            Dim tranType As String
            If .Item("MODE").ToString = "WEIGHT" Or .Item("MODE").ToString = "AMOUNT" Then
                tranType = "AD"
            ElseIf .Item("MODE").ToString = "EXCHANGE" Then
                tranType = "PU"
                puEx = "X"
            Else
                tranType = "PU"
                puEx = "P"
            End If
            Dim TNO As Integer = TranNo
            Dim issSno As String = GetNewSno(TranSnoType.RECEIPTCODE, tran)
            Dim catCode As String = objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & .Item("CATNAME").ToString & "'", , , tran)
            strSql = " INSERT INTO " & cnStockDb & "..RECEIPT"
            strSql += " ("
            strSql += " SNO,TRANNO,TRANDATE,TRANTYPE,PCS"
            strSql += " ,GRSWT,NETWT,LESSWT,PUREWT"
            strSql += " ,TAGNO,ITEMID,SUBITEMID,WASTPER"
            strSql += " ,WASTAGE,MCGRM,MCHARGE,AMOUNT"
            strSql += " ,RATE,BOARDRATE,SALEMODE,GRSNET"
            strSql += " ,TRANSTATUS,REFNO,REFDATE,COSTID"
            strSql += " ,COMPANYID,FLAG,EMPID,TAGGRSWT"
            strSql += " ,TAGNETWT,TAGRATEID,TAGSVALUE,TAGDESIGNER"
            strSql += " ,ITEMCTRID,ITEMTYPEID,PURITY,TABLECODE"
            strSql += " ,INCENTIVE,WEIGHTUNIT,CATCODE,OCATCODE"
            strSql += " ,ACCODE,ALLOY,BATCHNO,REMARK1"
            strSql += " ,REMARK2,USERID,UPDATED,UPTIME,SYSTEMID,DISCOUNT,RUNNO,CASHID,VATEXM,TAX"
            strSql += " ,DUSTWT,PUREXCH,MAKE,STNAMT,MISCAMT,METALID,STONEUNIT,APPVER"
            strSql += " )"
            strSql += " VALUES("
            strSql += " '" & issSno & "'" ''SNO
            strSql += " ," & TNO & "" 'TRANNO
            strSql += " ,'" & GetEntryDate(BillDate, tran).ToString("yyyy-MM-dd") & "'" 'TRANDATE
            strSql += " ,'" & tranType & "'" 'TRANTYPE
            strSql += " ," & Val(.Item("PCS").ToString) & "" 'PCS
            strSql += " ," & Val(.Item("GRSWT").ToString) & "" 'GRSWT
            strSql += " ," & Val(.Item("NETWT").ToString) & "" 'NETWT
            strSql += " ," & Val(.Item("LESSWT").ToString) & "" 'LESSWT
            strSql += " ," & Val(.Item("NETWT").ToString) & "" 'PUREWT '0
            strSql += " ,''" 'TAGNO
            strSql += " ,0" 'ITEMID
            strSql += " ,0" 'SUBITEMID
            strSql += " ," & Val(.Item("WASTAGEPER").ToString) & "" 'WASTPER
            strSql += " ," & Val(.Item("WASTAGE").ToString) & "" 'WASTAGE
            strSql += " ,0" 'MCGRM
            strSql += " ,0" 'MCHARGE
            strSql += " ," & Val(.Item("GROSSAMT").ToString) & "" 'AMOUNT
            strSql += " ," & Val(.Item("RATE").ToString) & "" 'RATE
            strSql += " ," & Val(.Item("RATE").ToString) & "" 'BOARDRATE
            strSql += " ,''" 'SALEMODE
            strSql += " ,''" 'GRSNET
            strSql += " ,''" 'TRANSTATUS ''
            strSql += " ,''" 'REFNO ''
            strSql += " ,NULL" 'REFDATE NULL
            strSql += " ,'" & BillCostId & "'" 'COSTID 
            strSql += " ,'" & strCompanyId & "'" 'COMPANYID
            strSql += " ,''" 'FLAG 
            strSql += " ," & Val(txtSalesMan_NUM.Text) & "" 'EMPID
            strSql += " ,0" 'TAGGRSWT
            strSql += " ,0" 'TAGNETWT
            strSql += " ,0" 'TAGRATEID
            strSql += " ,0" 'TAGSVALUE
            strSql += " ,''" 'TAGDESIGNER  
            strSql += " ,0" 'ITEMCTRID
            strSql += " ,0" 'ITEMTYPEID
            strSql += " ," & Val(.Item("PURITY").ToString) & "" 'PURITY
            strSql += " ,''" 'TABLECODE
            strSql += " ,''" 'INCENTIVE
            strSql += " ,''" 'WEIGHTUNIT
            strSql += " ,'" & catCode & "'" 'CATCODE
            strSql += " ,''" 'OCATCODE
            strSql += " ,''" 'ACCODE
            strSql += " ,0" 'ALLOY
            strSql += " ,'" & Batchno & "'" 'BATCHNO
            strSql += " ,''" 'REMARK1
            strSql += " ,''" 'REMARK2
            strSql += " ,'" & userId & "'" 'USERID
            strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
            strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
            strSql += " ,'" & systemId & "'" 'SYSTEMID
            strSql += " ,0" 'DISCOUNT
            strSql += " ,''" 'RUNNO
            strSql += " ,'" & BillCashCounterId & "'" 'CASHID
            strSql += " ,'" & VATEXM & "'" 'VATEXM
            strSql += " ," & Val(.Item("VAT").ToString) & "" 'TAX

            strSql += " ," & Val(.Item("DUSTWT").ToString) & "" 'DUSTWT
            strSql += " ,'" & puEx & "'" 'PUREXCH
            strSql += " ,''" 'MAKE
            strSql += " ,0" 'STONEAMT
            strSql += " ,0" 'MISCAMT
            strSql += " ,'" & objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE= '" & catCode & "'", , , tran) & "'" 'METALID
            strSql += " ,'" & objGPack.GetSqlValue("SELECT CASE WHEN DIASTNTYPE = 'T' THEN 'G' WHEN DIASTNTYPE = 'D' THEN 'C' WHEN DIASTNTYPE = 'P' THEN 'C' ELSE '' END AS STONEUNIT FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = '" & catCode & "'", , , tran) & "'" 'STONEUNIT
            strSql += " ,'" & VERSION & "'" 'APPVER
            strSql += " )"
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)
        End With
    End Sub

    Private Sub txtDueDays_NUM_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDueDays_NUM.Leave
        If Val(txtDueDays_NUM.Text) > 0 Then
            dtpDueDate.Value = BillDate.AddDays(Val(txtDueDays_NUM.Text)) '(BillDate, tran).AddDays(Val(txtDueDays_NUM.Text))
            'txtDueDays_NUM.Text = "1"
            'txtRemDays_TextChanged(Me, New EventArgs)
        Else
            MsgBox("Due days can't be zero")
            txtDueDays_NUM.Focus()
            Exit Sub
        End If
    End Sub

    Private Sub txtDueDays_NUM_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDueDays_NUM.TextChanged
        If Not Val(txtDueDays_NUM.Text) > 0 Then
            dtpDueDate.Value = GetEntryDate(BillDate, tran).ToString("yyyy-MM-dd")
            txtRemDays.Text = Val(txtDueDays_NUM.Text)
        Else
            dtpDueDate.Value = GetEntryDate(BillDate, tran).AddDays(Val(txtDueDays_NUM.Text))
            'txtRemDays.Text = 1 'Val(txtDueDays_NUM.Text) - 1
            'If OrdRepManDueDate <> 0 Then
            '    txtDueDays_NUM.Text = OrdRepManDueDate.ToString
            'Else
            '    txtDueDays_NUM.Text = "0"
            'End If
            txtRemDays_TextChanged(Me, New EventArgs)
            If txtRemDays.Text = "0" Then
                txtRemDays.Clear()
            End If
        End If
    End Sub

    Private Sub txtRemDays_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRemDays.TextChanged
        If Not Val(txtRemDays.Text) > 0 Then
            dtpRemDate.Value = dtpDueDate.Value
        Else
            dtpRemDate.Value = dtpDueDate.Value.AddDays(-1 * Val(txtRemDays.Text))
        End If

    End Sub

    Private Sub dtpRemDate_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        If dtpRemDate.Value > dtpDueDate.Value Then
            MsgBox("Reminder Date does not exceed DueDate", MsgBoxStyle.Information)
            dtpRemDate.Focus()
            Exit Sub
        End If
        'txtRemDays.Text = (dtpDueDate.Value - dtpRemDate.Value).Days
    End Sub

    Private Sub dtpDueDate_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        If dtpDueDate.Value < GetEntryDate(BillDate, tran).Date Then
            MsgBox("Due Date must exceed OrderDate", MsgBoxStyle.Information)
            dtpDueDate.Focus()
            Exit Sub
        End If
        txtDueDays_NUM.Text = (dtpDueDate.Value - GetEntryDate(BillDate, tran).Date).Days
    End Sub

    Private Sub AdvanceToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AdvanceToolStripMenuItem.Click
        txtAdjAdvance_AMT_GotFocus(Me, New EventArgs)
    End Sub

    Private Sub ChequeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChequeToolStripMenuItem.Click
        txtAdjCheque_AMT_GotFocus(Me, New EventArgs)
    End Sub

    Private Sub CreditCardToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CreditCardToolStripMenuItem.Click
        txtAdjCreditCard_AMT_GotFocus(Me, New EventArgs)
    End Sub

    Private Sub CashToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CashToolStripMenuItem.Click
        txtAdjCash_AMT.Focus()
    End Sub

    Private Sub txtAdjChitCard_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAdjChitCard_AMT.GotFocus
        If objChitCard.Visible Then Exit Sub
        objChitCard.BackColor = pnlContainer_OWN.BackColor
        objChitCard.StartPosition = FormStartPosition.CenterScreen
        objChitCard.grpChit.BackgroundColor = grpHeader.BackgroundColor
        Select Case objChitCard.ShowDialog
            Case Windows.Forms.DialogResult.OK
                Dim chitAmt As Double = Val(objChitCard.gridChitCardTotal.Rows(0).Cells("TOTAL").Value.ToString)
                txtAdjChitCard_AMT.Text = IIf(chitAmt <> 0, Format(chitAmt, "0.00"), Nothing)
                txtAdjCash_AMT.Focus()
            Case Windows.Forms.DialogResult.Abort
                objChitCard = New frmChitAdj
                txtAdjCash_AMT.Focus()
        End Select
    End Sub
    Private Sub txtAdjChitCard_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtAdjChitCard_AMT.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            txtAdjCash_AMT.Focus()
        End If
    End Sub

    Public Sub Set916Rate(ByVal ddate As Date)
        strSql = " SELECT METALID,SRATE,PURITY FROM " & cnAdminDb & "..RATEMAST R"
        strSql += " WHERE RDATE = '" & ddate.ToString("yyyy-MM-dd") & "'"
        strSql += " AND RATEGROUP = (SELECT MAX(RATEGROUP) FROM " & cnAdminDb & "..RATEMAST WHERE RDATE = R.RDATE)"
        strSql += " AND PURITY BETWEEN 91.6 AND 92"
        If RATE_BRANCHWISE Then
            strSql += " AND COSTID='" & cnCostId & "'"
        End If
        strSql += " ORDER BY METALID,PURITY"
        Dim dtRate As New DataTable
        cmd = New OleDbCommand(strSql, cn, tran)
        da = New OleDbDataAdapter(cmd)
        da.Fill(dtRate)
        Dim gRate As Double = Nothing
        Dim sRate As Double = Nothing
        For Each ro As DataRow In dtRate.Rows
            If ro!METALID.ToString = "G" Then
                gRate = Val(ro!SRATE.ToString)
            ElseIf ro!METALID.ToString = "S" Then
                sRate = Val(ro!SRATE.ToString)
            End If
        Next
        lblGoldRate.Text = IIf(gRate <> 0, Format(gRate, "0.00"), Nothing)
        lblSilverRate.Text = IIf(sRate <> 0, Format(sRate, "0.00"), Nothing)
    End Sub
    Private Sub GetAdvanceAddress()
        With objAdvance
            objAddressDia.AddressLock = True
            'objAddressDia.chkRegularCustomer.Enabled = False
            'objAddressDia.chkRegularCustomer.Checked = True
            objAddressDia.txtAddressPrevilegeId.Text = .txtAddressPrevilegeId.Text
            objAddressDia.txtAddressPartyCode.Text = .txtAddressPartyCode.Text
            objAddressDia.cmbAddressTitle_OWN.Text = .cmbAddressTitle_OWN.Text
            objAddressDia.txtAddressInitial.Text = .txtAddressInitial.Text
            objAddressDia.txtAddressName.Text = .txtAddressName.Text
            objAddressDia.txtAddressDoorNo.Text = .txtAddressDoorNo.Text
            objAddressDia.txtAddress1.Text = .txtAddress1.Text
            objAddressDia.txtAddress2.Text = .txtAddress2.Text
            objAddressDia.txtAddress3.Text = .txtAddress3.Text
            objAddressDia.cmbAddressArea_OWN.Text = .cmbAddressArea_OWN.Text
            objAddressDia.cmbAddressCity_OWN.Text = .cmbAddressCity_OWN.Text
            objAddressDia.cmbAddressState.Text = .cmbAddressState_OWN.Text
            objAddressDia.cmbAddressCountry_OWN.Text = .cmbAddressCountry_OWN.Text
            objAddressDia.txtAddressPincode_NUM.Text = .txtAddressPincode_NUM.Text
            objAddressDia.txtAddressEmailId_OWN.Text = .txtAddressEmailId_OWN.Text
            objAddressDia.txtAddressFax.Text = .txtAddressFax.Text
            objAddressDia.txtAddressPhoneRes.Text = .txtAddressPhoneRes.Text
            objAddressDia.txtAddressMobile.Text = .txtAddressMobile.Text
            objAddressDia.txtAddressRegularSno.Text = .txtAddressRegularSno.Text
        End With
    End Sub

    Private Sub txtONatureOfRepair_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtONatureOfRepair.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            chkOImage.Focus()
            ' LoadOrderDetail()
        End If
    End Sub

    Private Sub txtAdjAdvanceWt_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAdjAdvanceWt.GotFocus
        txtAdjCash_AMT.Focus()
    End Sub

    Private Sub ChitCardToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChitCardToolStripMenuItem.Click
        txtAdjChitCard_AMT_GotFocus(Me, New EventArgs)
    End Sub

    Private Sub chkOImage_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOImage.CheckedChanged

    End Sub
    Private Sub chkOImage_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles chkOImage.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If chkOImage.Checked And defalutDestination <> "" Then
                If Not IO.Directory.Exists(defalutDestination) Then
                    MsgBox(defalutDestination & vbCrLf & " Destination Path not found" & vbCrLf & "Please make the correct path", MsgBoxStyle.Information)
                    chkOImage.Checked = False
                    chkOImage.Select()
                    Exit Sub
                End If
                grpImage.Visible = True
                grpImage.BringToFront()
                btnBrowse.Focus()
            Else
                LoadOrderDetail()
            End If
        End If
    End Sub

    Private Sub btnBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse.Click
        Try
            Dim openDia As New OpenFileDialog
            Dim str As String
            str = "JPEG(*.jpg)|*.jpg"
            str += "|Bitmap(*.bmp)|*.bmp"
            str += "|GIF(*.gif)|*.gif"
            str += "|All Files(*.*)|*.*"
            openDia.Filter = str
            If openDia.ShowDialog = Windows.Forms.DialogResult.OK Then
                AutoImageSizer(openDia.FileName, picImage, PictureBoxSizeMode.StretchImage)
                PICPATH = openDia.FileName
                LoadOrderDetail()
            End If
        Catch ex As Exception
            MsgBox(E0016, MsgBoxStyle.Exclamation)
        End Try
    End Sub

    Private Sub dtpRepairDate_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtpRepairDate.LostFocus
        BillDate = dtpRepairDate.Value
    End Sub

    Private Sub dtpRepairDate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtpRepairDate.TextChanged

    End Sub

    Private Sub btnSave_CursorChanged(sender As Object, e As EventArgs) Handles btnSave.CursorChanged

    End Sub
End Class