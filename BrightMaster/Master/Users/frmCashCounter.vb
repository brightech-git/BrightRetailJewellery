Imports System.Data.OleDb
Public Class frmCashCounter
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim flagSave As Boolean = False
    Dim dtCounter As New DataTable
    Dim NODEWISECACTR As Boolean = False

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub
    Function funcNew() As Integer
        objGPack.TextClear(Me)
        txtCounterId_NUM_MAN.Enabled = True
        cmbManualBillNo.Text = "NO"
        cmbRateTax.Text = "NO"
        cmbAmountLock.Text = "NO"
        cmbSepBillNo.Text = "NO"
        strSql = " Select CostName from " & cnAdminDb & "..CostCentre order by CostName"
        cmbActive.Text = "YES"
        objGPack.FillCombo(strSql, cmbCostCentre)
        funcOpen()
        flagSave = False
        BrighttechPack.GlobalMethods.FillCombo(chkCmbItemCounter, dtCounter, "ITEMCTRNAME", , "ALL")
        txtCounterId_NUM_MAN.Focus()
    End Function
    Function funcOpen() As Integer
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Function
        strSql = " SELECT CASHID,CASHNAME"
        strSql += " ,ISNULL((SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE AS C WHERE C.COSTID = CA.COSTID),'')AS COSTNAME"
        strSql += " ,PASSWORD,STATUS,NODEID,DISCOUNTPER,DISCOUNTAMT"
        strSql += " ,CASE WHEN MANUALBILLNO = 'Y' THEN 'YES' ELSE 'NO' END AS MANUALBILLNO"
        strSql += " ,CASE WHEN RATETAX = 'Y' THEN 'YES' ELSE 'NO' END AS RATETAX"
        strSql += " ,CASE WHEN AMTLOCK = 'Y' THEN 'YES' ELSE 'NO' END AS AMTLOCK"
        strSql += " ,CASE WHEN SEPBILLGEN = 'Y' THEN 'YES' ELSE 'NO' END AS SEPBILLGEN"
        strSql += " ,SALSERNO,PURSERNO,SALBILLNO,PURBILLNO,COUNTERTYPE"
        strSql += " FROM " & cnAdminDb & "..CASHCOUNTER AS CA"
        funcOpenGrid(strSql, gridView)
        gridView.Focus()
    End Function
    Function funcExit() As Integer
        Me.Close()
    End Function
    Function funcSave() As Integer
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Function
        If txtCounterId_NUM_MAN.Text = "" Then
            MsgBox("CounterId Should Not Empty", MsgBoxStyle.Information)
            txtCounterId_NUM_MAN.Focus()
            Return 0
        End If
        If flagSave = False Then
            If objGPack.DupChecker(txtCounterId_NUM_MAN, "SELECT 1 FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHID = '" & txtCounterId_NUM_MAN.Text & "'") Then
                Return 0
            End If
        End If
        If txtCounterName.Text = "" Then
            MsgBox("CounterName Should Not Empty", MsgBoxStyle.Information)
            txtCounterName.Focus()
            Return 0
        End If
        If objGPack.DupChecker(txtCounterName, "SELECT 1 FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME = '" & txtCounterName.Text & "' AND CASHID <> '" & txtCounterId_NUM_MAN.Text & "'") Then
            Return 0
        End If
        If flagSave = False Then
            funcAdd()
            Return 0
        Else
            funcUpdate()
            Return 0
        End If
    End Function

    Private Function GetSelectedCounters() As String
        Dim selectedItemCounters As String = ""
        If chkCmbItemCounter.Text <> "ALL" And chkCmbItemCounter.Text <> "" Then
            For cnt As Integer = 0 To chkCmbItemCounter.Items.Count - 1
                If chkCmbItemCounter.GetItemChecked(cnt) = False Then Continue For
                selectedItemCounters += "''" & dtCounter.Rows(cnt).Item("ITEMCTRID").ToString & "'',"
            Next
            If selectedItemCounters <> "" Then
                selectedItemCounters = Mid(selectedItemCounters, 1, selectedItemCounters.Length - 1)
            End If
        End If
        Return selectedItemCounters
    End Function

    Function funcAdd() As Integer
        Dim CostId As String = Nothing
        strSql = "Select CostId from " & cnAdminDb & "..CostCentre where CostName = '" & cmbCostCentre.Text & "'"
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            CostId = dt.Rows(0).Item("CostId")
        Else
            CostId = ""
        End If
        Dim selectedItemCounters As String = GetSelectedCounters()
        strSql = " Insert into " & cnAdminDb & "..CashCounter"
        strSql += " ("
        strSql += " CashId,CashName,CostId,"
        strSql += " Password,Status,Nodeid,DiscountPer,"
        strSql += " DiscountAmt,ManualBillNo,Ratetax,"
        strSql += " Amtlock,SepBillGen,SalSerNo,"
        strSql += " PurSerNo,SalBillNo,PurBillNo,"
        strSql += " SessionClosed,SessionNo,"
        'strSql += " SessionDate,"
        'strSql += " SesBillDate,"
        strSql += " UserId,Updated,Uptime"
        If _CounterWiseCashMaintanance Then strSql += " ,CASHACCODE"
        strSql += " ,itemctrids"
        strSql += " ,COUNTERTYPE"
        strSql += " ,ACTIVE"
        strSql += " )Values("
        strSql += " '" & txtCounterId_NUM_MAN.Text & "'" 'CashId
        strSql += " ,'" & txtCounterName.Text & "'" 'CashName
        strSql += " ,'" & CostId & "'" 'CostId
        strSql += " ,'" & BrighttechPack.Methods.Encrypt(txtPassword.Text) & "'" 'Password
        strSql += " ," & Val(txtStatus.Text) & "" 'Status
        If NODEWISECACTR Then
            strSql += " ,'" & txtNodeID.Text.ToUpper() & "'" 'Nodeid
        Else
            strSql += " ,'" & systemId & "'" 'Nodeid
        End If
        strSql += " ," & Val(txtDiscountPer_PER.Text) & "" 'DiscountPer
        strSql += " ," & Val(txtDiscountAmt_AMT.Text) & "" 'DiscountAmt
        strSql += " ,'" & Mid(cmbManualBillNo.Text, 1, 1) & "'" 'ManualBillNo
        strSql += " ,'" & Mid(cmbRateTax.Text, 1, 1) & "'" 'Ratetax
        strSql += " ,'" & Mid(cmbAmountLock.Text, 1, 1) & "'" 'Amtlock
        strSql += " ,'" & Mid(cmbSepBillNo.Text, 1, 1) & "'" 'SepBillGen
        strSql += " ," & Val(txtSalesSerialNo_NUM.Text) & "" 'SalSerNo
        strSql += " ," & Val(txtPurchaseSerialNo_NUM.Text) & "" 'PurSerNo
        strSql += " ," & Val(txtSalesBillNo_NUM.Text) & "" 'SalBillNo
        strSql += " ," & Val(txtPurchaseBillNo_NUM.Text) & "" 'PurBillNo
        strSql += " ,''" 'SessionClosed
        strSql += " ,0" 'SessionNo
        'strSql += " ,''" 'SessionDate
        'strSql += " ,''" 'SesBillDate
        strSql += " ," & userId & "" 'UserId
        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'Updated
        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'Uptime
        If _CounterWiseCashMaintanance Then strSql += " ,'" & objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbCashAc.Text & "'") & "'" 'CASHACCODE
        strSql += " ,'" & selectedItemCounters & "'"
        strSql += " ,'" & Mid(cmbCounterType.Text, 1, 1) & "'"
        strSql += " ,'" & Mid(cmbActive.Text, 1, 1) & "'" 'ACTIVE
        strSql += " )"
        Try
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            InsertIntoBillControl("COU-" & txtCounterId_NUM_MAN.Text & "-SAL", "" & txtCounterName.Text & " SALE BILLNO", "N", "N", "", "P")
            InsertIntoBillControl("COU-" & txtCounterId_NUM_MAN.Text & "-SAL-OTH", "" & txtCounterName.Text & " (NON CASH) SALE BILLNO", "N", "N", "", "P")
            InsertIntoBillControl("COU-" & txtCounterId_NUM_MAN.Text & "-PUR", "" & txtCounterName.Text & " PURCH BILLNO", "N", "N", "", "P")
            funcNew()
        Catch ex As OleDbException
            If cn.State = ConnectionState.Open Then

            End If
            If ex.ErrorCode = 2627 Then
                MsgBox("Counter Name Already Exist", MsgBoxStyle.Information)
                txtCounterName.Focus()
            Else
                MsgBox(ex.Message)
                MsgBox(ex.StackTrace)
            End If
        Catch ex As Exception
            If cn.State = ConnectionState.Open Then

            End If
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function
    Function funcUpdate() As Integer
        Dim CostId As String = Nothing
        strSql = "Select CostId from " & cnAdminDb & "..CostCentre where CostName = '" & cmbCostCentre.Text & "'"
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            CostId = dt.Rows(0).Item("CostId")
        Else
            CostId = ""
        End If
        Dim selectedItemCounters As String = GetSelectedCounters()
        strSql = " Update " & cnAdminDb & "..CashCounter Set"
        strSql += " CashId='" & txtCounterId_NUM_MAN.Text & "'"
        strSql += " ,CashName='" & txtCounterName.Text & "'"
        strSql += " ,CostId='" & CostId & "'"
        strSql += " ,Password='" & BrighttechPack.Methods.Encrypt(txtPassword.Text) & "'"
        strSql += " ,Status=" & Val(txtStatus.Text) & ""
        If NODEWISECACTR Then
            strSql += " ,Nodeid='" & txtNodeID.Text.ToUpper() & "'"
        Else
            strSql += " ,Nodeid='" & systemId & "'"
        End If
        strSql += " ,DiscountPer=" & Val(txtDiscountPer_PER.Text) & ""
        strSql += " ,DiscountAmt=" & Val(txtDiscountAmt_AMT.Text) & ""
        strSql += " ,ManualBillNo= '" & Mid(cmbManualBillNo.Text, 1, 1) & "'"
        strSql += " ,Ratetax='" & Mid(cmbRateTax.Text, 1, 1) & "'"
        strSql += " ,Amtlock='" & Mid(cmbAmountLock.Text, 1, 1) & "'"
        strSql += " ,SepBillGen= '" & Mid(cmbSepBillNo.Text, 1, 1) & "'"
        strSql += " ,SalSerNo= " & Val(txtSalesSerialNo_NUM.Text) & ""
        strSql += " ,PurSerNo= " & Val(txtPurchaseSerialNo_NUM.Text) & ""
        strSql += " ,SalBillNo=" & Val(txtSalesBillNo_NUM.Text) & ""
        strSql += " ,PurBillNo=" & Val(txtPurchaseBillNo_NUM.Text) & ""
        strSql += " ,UserId=" & userId & ""
        strSql += " ,Updated='" & Today.Date.ToString("yyyy-MM-dd") & "'"
        strSql += " ,Uptime='" & Date.Now.ToLongTimeString & "'"
        If _CounterWiseCashMaintanance Then
            strSql += " ,CASHACCODE = '" & objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbCashAc.Text & "'") & "'"
        End If
        strSql += " ,ITEMCTRIDS = '" & selectedItemCounters & "'"
        strSql += " ,COUNTERTYPE='" + Mid(cmbCounterType.Text, 1, 1) + "'"
        strSql += " ,ACTIVE='" & Mid(cmbActive.Text, 1, 1) & "'"
        strSql += " Where CashId = '" & txtCounterId_NUM_MAN.Text & "'"
        Try
            ExecQuery(SyncMode.Master, strSql, cn)
            funcNew()
        Catch ex As OleDbException
            If cn.State = ConnectionState.Open Then

            End If
            If ex.ErrorCode = 2627 Then
                MsgBox("Counter Name Already Exist", MsgBoxStyle.Information)
                txtCounterName.Focus()
            Else
                MsgBox(ex.Message)
                MsgBox(ex.StackTrace)
            End If
        Catch ex As Exception
            If cn.State = ConnectionState.Open Then

            End If
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function
    Function funcGetDetails(ByVal temp As String) As Integer
        strSql = " SELECT CASHID,CASHNAME"
        strSql += " ,ISNULL((SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE AS C WHERE C.COSTID = CA.COSTID),'')AS COSTNAME"
        strSql += " ,PASSWORD,STATUS,NODEID,DISCOUNTPER,DISCOUNTAMT"
        strSql += " ,CASE WHEN MANUALBILLNO = 'Y' THEN 'YES' ELSE 'NO' END AS MANUALBILLNO"
        strSql += " ,CASE WHEN RATETAX = 'Y' THEN 'YES' ELSE 'NO' END AS RATETAX"
        strSql += " ,CASE WHEN AMTLOCK = 'Y' THEN 'YES' ELSE 'NO' END AS AMTLOCK"
        strSql += " ,CASE WHEN SEPBILLGEN = 'Y' THEN 'YES' ELSE 'NO' END AS SEPBILLGEN"
        strSql += " ,SALSERNO,PURSERNO,SALBILLNO,PURBILLNO"
        strSql += " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = CA.CASHACCODE)AS CASHACNAME"
        strSql += " ,ISNULL(ITEMCTRIDS,'') AS ITEMCTRIDS,ISNULL(COUNTERTYPE,'') AS COUNTERTYPE"
        strSql += " ,CASE WHEN ISNULL(ACTIVE,'') = 'N' THEN 'NO' ELSE 'YES' END AS ACTIVE"
        strSql += " FROM " & cnAdminDb & "..CASHCOUNTER AS CA"
        strSql += " WHERE CASHID = '" & temp & "'"
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Return 0
        End If
        With dt.Rows(0)
            txtCounterId_NUM_MAN.Text = .Item("CashId")
            txtCounterName.Text = .Item("CashName")
            cmbCostCentre.Text = .Item("CostName")
            txtPassword.Text = BrighttechPack.Methods.Decrypt(.Item("Password"))
            txtStatus.Text = .Item("Status")
            txtDiscountPer_PER.Text = .Item("DiscountPer")
            txtDiscountAmt_AMT.Text = .Item("DiscountAmt")
            cmbManualBillNo.Text = .Item("ManualBillNo")
            cmbRateTax.Text = .Item("Ratetax")
            cmbAmountLock.Text = .Item("Amtlock")
            cmbSepBillNo.Text = .Item("SepBillGen")
            txtSalesSerialNo_NUM.Text = .Item("SalSerNo")
            txtPurchaseSerialNo_NUM.Text = .Item("PurSerNo")
            txtSalesBillNo_NUM.Text = .Item("SalBillNo")
            txtPurchaseBillNo_NUM.Text = .Item("PurBillNo")
            cmbCashAc.Text = .Item("CASHACNAME").ToString
            If .Item("COUNTERTYPE").ToString = "C" Then
                cmbCounterType.Text = "COLLECTION"
            ElseIf .Item("COUNTERTYPE").ToString = "B" Then
                cmbCounterType.Text = "BILLING"
            Else
                cmbCounterType.Text = " "
            End If

            If NODEWISECACTR Then txtNodeID.Text = .Item("Nodeid").ToString
            cmbActive.Text = .Item("ACTIVE")

            Dim selCounters As New List(Of String)
            For Each s As String In .Item("ITEMCTRIDS").ToString.Split(",")
                selCounters.Add(s.Replace("'", ""))
            Next

            If .Item("ITEMCTRIDS").ToString = "" Then
                chkCmbItemCounter.SetItemChecked(0, True)
            Else
                For cnt As Integer = 0 To dtCounter.Rows.Count - 1
                    If selCounters.Contains(dtCounter.Rows(cnt).Item("ITEMCTRID").ToString) Then
                        chkCmbItemCounter.SetItemChecked(cnt, True)
                    Else
                        chkCmbItemCounter.SetItemChecked(cnt, False)
                    End If
                Next
            End If
        End With
        txtCounterId_NUM_MAN.Enabled = False
        flagSave = True
    End Function

    Private Sub frmCashCounter_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtCounterId_NUM_MAN.Focused Then
                Exit Sub
            End If
            If txtCounterName.Focused Then
                Exit Sub
            End If
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub frmCashCounter_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        funcGridStyle(gridView)
        If _CounterWiseCashMaintanance Then
            Label16.Visible = True 'Cash LABEL
            cmbCashAc.Visible = True
            strSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACGRPCODE = '1'"
            objGPack.FillCombo(strSql, cmbCashAc)
        Else
            Label16.Visible = False
            cmbCashAc.Visible = False
        End If
        If GetAdmindbSoftValue("COSTCENTRE").ToUpper = "Y" Then
            strSql = " Select CostName from " & cnAdminDb & "..CostCentre order by CostName"
            objGPack.FillCombo(strSql, cmbCostCentre)
            cmbCostCentre.Enabled = True
        Else
            cmbCostCentre.Enabled = False
        End If
        cmbCounterType.Text = ""
        strSql = " SELECT 'ALL' ITEMCTRNAME,'ALL' AS ITEMCTRID,1 RESULT "
        strSql += " UNION ALL"
        strSql += " SELECT ITEMCTRNAME,CONVERT(vARCHAR,ITEMCTRID),2 RESULT FROM " & cnAdminDb & "..ITEMCOUNTER "
        strSql += " ORDER BY RESULT,ITEMCTRNAME"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCounter)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbItemCounter, dtCounter, "ITEMCTRNAME", , "ALL")
        txtPassword.CharacterCasing = CharacterCasing.Normal
        cmbActive.Items.Add("YES")
        cmbActive.Items.Add("NO")

        NODEWISECACTR = IIf(GetAdmindbSoftValue("NODEWISECACTR", "N").ToUpper() = "Y", True, False)
        If NODEWISECACTR Then
            Label18.Visible = True
            txtNodeID.Visible = True
        End If
        funcNew()
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        funcSave()
    End Sub

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click
        funcOpen()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        funcNew()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        funcExit()
    End Sub

    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        funcSave()
    End Sub

    Private Sub OpenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click
        funcOpen()
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        funcNew()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        funcExit()
    End Sub

    Private Sub gridView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.GotFocus
        lblStatus.Visible = True
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            gridView.CurrentCell = gridView.CurrentCell
            If gridView.RowCount > 0 Then
                funcGetDetails(gridView.Item(0, gridView.CurrentRow.Index).Value)
                txtCounterName.Focus()
            End If
        End If
    End Sub

    Private Sub txtCounterName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCounterName.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtCounterName.Text = "" Then
                MsgBox("Counter Name should Not Empty", MsgBoxStyle.Information)
                txtCounterName.Focus()
                Exit Sub
            ElseIf objGPack.DupChecker(txtCounterName, "SELECT 1 FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME = '" & txtCounterName.Text & "' AND CASHID <> '" & txtCounterId_NUM_MAN.Text & "'") Then
                Exit Sub
            Else
                SendKeys.Send("{TAB}")
            End If
        End If
    End Sub

    Private Sub txtCounterId_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCounterId_NUM_MAN.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtCounterId_NUM_MAN.Text = "" Then
                Exit Sub
                txtCounterId_NUM_MAN.Focus()
            ElseIf objGPack.DupChecker(txtCounterId_NUM_MAN, "SELECT 1 FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHID = '" & txtCounterId_NUM_MAN.Text & "'") Then
                Exit Sub
            Else
                SendKeys.Send("{TAB}")
            End If
        End If
    End Sub

    Private Sub gridView_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.LostFocus
        lblStatus.Visible = False
    End Sub

    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
    End Sub
End Class