Imports System.Data.OleDb
Public Class OrderStatusUpdate
    Dim strSql As String
    Public OrdRepNo As String
    Public OrdSno As String
    Dim cmd As OleDbCommand
    Dim objRemark As New frmBillRemark

    Dim IssTranNo As String = ""
    Dim RecTranNo As String = ""
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
    End Sub

    Private Sub OrderStatusUpdate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub rbtIssued_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtIssued.CheckedChanged
        grpIssuedDet.Enabled = rbtIssued.Checked
        IssTranNo = ""
        RecTranNo = ""
        objGPack.TextClear(grpIssuedDet)
    End Sub

    Private Sub rbtReceived_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtReceived.CheckedChanged
        grpReceived.Enabled = rbtReceived.Checked
        IssTranNo = ""
        RecTranNo = ""
        objGPack.TextClear(grpReceived)
    End Sub

    Private Sub OrderStatusUpdate_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ''Load Designer
        strSql = " SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE ACTIVE = 'Y' ORDER BY DESIGNERNAME"
        objGPack.FillCombo(strSql, cmbIssDesigner_MAN, False, False)
        objGPack.FillCombo(strSql, cmbRecDesigner_MAN, False, False)
        rbtDelivered.Checked = True
        rbtDelivered.Select()

        dtpBillDate.MinimumDate = (New DateTimePicker).MinDate
        dtpBillDate.MaximumDate = (New DateTimePicker).MaxDate

        dtpRecBillDate.MinimumDate = (New DateTimePicker).MinDate
        dtpRecBillDate.MaximumDate = (New DateTimePicker).MaxDate

        IssTranNo = ""
        RecTranNo = ""
    End Sub

    Private Sub UpdateIssue()
        Try '
            If Val(txtIssTranNo_NUM.Text) = 0 Then
                MessageBox.Show("Please enter Billno")
                txtIssTranNo_NUM.Focus()
                Exit Sub
            End If
            strSql = " SELECT 1 CNTROW FROM " & cnadmindb & "..ORIRDETAIL WHERE ISNULL(TRANNO,'') <> '" & IssTranNo & "' AND ISNULL(TRANNO,'') = '" & txtIssTranNo_NUM.Text & "'"
            strSql += " AND ISNULL(ORSTATUS,'') = 'I'"
            Dim dtManualNo As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtManualNo)
            If dtManualNo.Rows.Count > 0 Then
                If MessageBox.Show("Billno already exits, Do you want allow it?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then
                    txtIssTranNo_NUM.Focus()
                    Exit Sub
                End If
            End If

            tran = Nothing
            tran = cn.BeginTransaction
            strSql = " UPDATE " & cnadmindb & "..ORIRDETAIL SET"
            strSql += "  TRANNO = '" & txtIssTranNo_NUM.Text & "'"
            strSql += " ,TRANDATE = '" & Convert.ToDateTime(dtpBillDate.Value).ToString("yyyy-MM-dd") & "'"
            strSql += " ,PCS = " & Val(txtIssPcs_NUM.Text) & ""
            strSql += " ,GRSWT = " & Val(txtIssGrsWt_WET.Text) & ""
            strSql += " ,NETWT = " & Val(txtIssNetWt_WET.Text) & ""
            strSql += " ,DESIGNERID = (SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & cmbIssDesigner_MAN.Text & "')"
            strSql += " WHERE ORNO = '" & OrdRepNo & "'"
            strSql += " AND ORSNO = '" & OrdSno & "'"
            strSql += " AND ISNULL(ORSTATUS,'') = 'I'"
            strSql += " AND COMPANYID = '" & strCompanyId & "'"
            Dim costId As String = objGPack.GetSqlValue("SELECT COSTID FROM " & cnadmindb & "..ORIRDETAIL WHERE ORNO = '" & OrdRepNo & "' AND ORSNO = '" & OrdSno & "'  AND ISNULL(CANCEL,'') = ''", , , tran)
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId)
            tran.Commit()
            tran = Nothing
            Me.DialogResult = Windows.Forms.DialogResult.OK
            MsgBox("Order Issue Updated", MsgBoxStyle.Information)
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub

    Private Sub UpdateReceipt()
        Try '
            If Val(txtRecTranNo_NUM.Text) = 0 Then
                MessageBox.Show("Please enter Billno")
                txtRecTranNo_NUM.Focus()
                Exit Sub
            End If
            strSql = " SELECT 1 CNTROW FROM " & cnadmindb & "..ORIRDETAIL WHERE ISNULL(TRANNO,'') <> '" & RecTranNo & "' AND ISNULL(TRANNO,'') = '" & txtRecTranNo_NUM.Text & "'"
            strSql += " AND ISNULL(ORSTATUS,'') = 'R'"
            Dim dtManualNo As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtManualNo)
            If dtManualNo.Rows.Count > 0 Then
                If MessageBox.Show("Billno already exits, Do you want allow it?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then
                    txtRecTranNo_NUM.Focus()
                    Exit Sub
                End If
            End If
            tran = Nothing
            tran = cn.BeginTransaction
            strSql = " UPDATE " & cnadmindb & "..ORIRDETAIL SET"
            strSql += "  TRANNO = '" & txtRecTranNo_NUM.Text & "'"
            strSql += " ,TRANDATE = '" & Convert.ToDateTime(dtpRecBillDate.Value).ToString("yyyy-MM-dd") & "'"
            strSql += " ,PCS = " & Val(txtRecPcs_NUM.Text) & ""
            strSql += " ,GRSWT = " & Val(txtRecGrsWt_WET.Text) & ""
            strSql += " ,NETWT = " & Val(txtRecNetWt_WET.Text) & ""
            strSql += " ,WASTAGE = " & Val(txtRecWastage_WET.Text) & ""
            strSql += " ,MC = " & Val(txtRecMc_AMT.Text) & ""
            strSql += " ,DESIGNERID = (SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & cmbRecDesigner_MAN.Text & "')"
            strSql += " WHERE ORNO = '" & OrdRepNo & "'"
            strSql += " AND ORSNO = '" & OrdSno & "'"
            strSql += " AND ISNULL(ORSTATUS,'') = 'R'"
            strSql += " AND COMPANYID = '" & strCompanyId & "'"
            Dim costId As String = objGPack.GetSqlValue("SELECT COSTID FROM " & cnadmindb & "..ORIRDETAIL WHERE ORNO = '" & OrdRepNo & "' AND ORSNO = '" & OrdSno & "'", , , tran)
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId)
            tran.Commit()
            tran = Nothing
            Me.DialogResult = Windows.Forms.DialogResult.OK
            MsgBox("Order Receipt Updated", MsgBoxStyle.Information)
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If rbtIssued.Checked Then
            UpdateIssue()
        ElseIf rbtReceived.Checked Then
            UpdateReceipt()
        Else
            If MessageBox.Show("Do you want to manually update the status to delivered for this order item?", "Update Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                Try

                    objRemark = New frmBillRemark
                    objRemark.Text = "Status Update Reason"
                    If objRemark.ShowDialog() <> Windows.Forms.DialogResult.OK Then
                        Exit Sub
                    End If

                    tran = Nothing
                    tran = cn.BeginTransaction
                    Dim costId As String = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORNO = '" & OrdRepNo & "' AND ORSNO = '" & OrdSno & "'  AND ISNULL(CANCEL,'') = ''", , , tran)
                    If costId = "" Then
                        costId = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..ORMAST WHERE ORNO = '" & OrdRepNo & "' ", , , tran)
                    End If
                    Dim batch As String = GetNewBatchno(cnCostId, GetEntryDate(GetServerDate(tran), tran), tran)
                    strSql = " UPDATE " & cnAdminDb & "..ORMAST SET"
                    strSql += " ODBATCHNO = '" & batch & "'"
                    strSql += " ,ODSNO = 'M1'"
                    strSql += " ,REASON=CONVERT(VARCHAR(200),'" & objRemark.cmbRemark1_OWN.Text + objRemark.cmbRemark2_OWN.Text & "') "
                    strSql += " WHERE ORNO = '" & OrdRepNo & "'"
                    strSql += " AND SNO = '" & OrdSno & "'"
                    strSql += " AND COMPANYID = '" & strCompanyId & "'"
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId)
                    tran.Commit()
                    tran = Nothing

                    Me.DialogResult = Windows.Forms.DialogResult.OK
                    MsgBox("Delivered Updated", MsgBoxStyle.Information)
                Catch ex As Exception
                    If Not tran Is Nothing Then tran.Rollback()
                    MsgBox(ex.Message + vbCrLf + ex.StackTrace)
                End Try
            Else
                Exit Sub
            End If
        End If
        Me.Close()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub rbtIssued_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles rbtIssued.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            objGPack.TextClear(grpIssuedDet)
            strSql = " SELECT TRANNO,TRANDATE,PCS,GRSWT,NETWT"
            strSql += " ,(SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = O.DESIGNERID)AS DESIGNER"
            strSql += " FROM " & cnadmindb & "..ORIRDETAIL AS O WHERE ORNO = '" & OrdRepNo & "' AND ORSNO = '" & OrdSno & "' AND ISNULL(CANCEL,'') = ''"
            strSql += " AND ISNULL(ORSTATUS,'') = 'I'"
            strSql += " AND COMPANYID = '" & strCompanyId & "'"
            Dim dt As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If Not dt.Rows.Count > 0 Then
                MsgBox("Issue Detail not found", MsgBoxStyle.Information)
                btnExit_Click(Me, New EventArgs)
                Exit Sub
            End If
            With dt.Rows(0)
                txtIssTranNo_NUM.Text = .Item("TRANNO").ToString
                IssTranNo = txtIssTranNo_NUM.Text
                dtpBillDate.Value = .Item("TRANDATE").ToString
                txtIssPcs_NUM.Text = .Item("PCS").ToString
                txtIssGrsWt_WET.Text = .Item("GRSWT").ToString
                txtIssNetWt_WET.Text = .Item("NETWT").ToString
                cmbIssDesigner_MAN.Text = .Item("DESIGNER").ToString
            End With
        End If
    End Sub

    Private Sub UpdateTagInfo(ByVal update As Boolean)
        Dim costid = Mid(OrdRepNo, 1, 2)
        If costid = "" Or costid = "00" Then
            costid = cnCostId
        End If
        Dim obj As New OrderStatusUpdateTagno(OrdSno, OrdRepNo, costid, update)
        obj.ShowDialog()
    End Sub

    Private Sub rbtReceived_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles rbtReceived.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            objGPack.TextClear(grpReceived)
            strSql = " SELECT TRANNO,TRANDATE,PCS,GRSWT,NETWT"
            strSql += " ,(SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = O.DESIGNERID)AS DESIGNER"
            strSql += " ,WASTAGE,MC"
            strSql += " FROM " & cnadmindb & "..ORIRDETAIL AS O WHERE ORNO = '" & OrdRepNo & "' AND ORSNO = '" & OrdSno & "'  AND ISNULL(CANCEL,'') = ''"
            strSql += " AND ISNULL(ORSTATUS,'') = 'R'"
            strSql += " AND COMPANYID = '" & strCompanyId & "'"
            Dim dt As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If Not dt.Rows.Count > 0 Then
                If MessageBox.Show("Receipt Detail not found" & vbCrLf & "Do you want to update Tagged Detail", "Update Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                    UpdateTagInfo(False)
                End If
                btnExit_Click(Me, New EventArgs)
                Exit Sub
            Else
                If MessageBox.Show("Do you want to update Tagged Detail", "Update Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                    UpdateTagInfo(True)
                End If
            End If
            With dt.Rows(0)
                txtRecTranNo_NUM.Text = .Item("TRANNO").ToString
                RecTranNo = txtRecTranNo_NUM.Text
                dtpRecBillDate.Value = .Item("TRANDATE").ToString
                txtRecPcs_NUM.Text = .Item("PCS").ToString
                txtRecGrsWt_WET.Text = .Item("GRSWT").ToString
                txtRecNetWt_WET.Text = .Item("NETWT").ToString
                txtRecWastage_WET.Text = .Item("WASTAGE").ToString
                txtRecMc_AMT.Text = .Item("MC").ToString
                cmbRecDesigner_MAN.Text = .Item("DESIGNER").ToString
            End With
        End If
    End Sub

    Private Sub txtIssNetWt_WET_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtIssNetWt_WET.Leave
        If Val(txtIssNetWt_WET.Text) > Val(txtIssGrsWt_WET.Text) Then
            txtIssNetWt_WET.Text = txtIssGrsWt_WET.Text
        End If
    End Sub

    Private Sub txtRecNetWt_WET_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRecNetWt_WET.Leave
        If Val(txtRecNetWt_WET.Text) > Val(txtRecGrsWt_WET.Text) Then
            txtRecNetWt_WET.Text = txtRecGrsWt_WET.Text
        End If
    End Sub
End Class