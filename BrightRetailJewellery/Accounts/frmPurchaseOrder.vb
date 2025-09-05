Imports System.Data.OleDb

Public Class frmPurchaseOrder
    Dim strsql As String
    Dim Cmd As OleDbCommand
    Dim Da As OleDbDataAdapter
    Dim dt As DataTable
    Dim dr As OleDbDataReader
    Dim BatchNo As String = Nothing
    Dim obj As New frmStoneDia
    Dim InclCusttype As String = GetAdmindbSoftValue("INCL_CUSTOMER_ISSREC", "N")

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Dim Sno As String
        Dim TranNo As Integer
        Try
            Dim Itemid As Integer = Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbOItem_OWN.Text & "'", , , tran))
            Dim subItemid As Integer = Val(objGPack.GetSqlValue("SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbOSubItem_OWN.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbOItem_OWN.Text & "')", , , tran))

            tran = cn.BeginTransaction
            strsql = " IF (SELECT COUNT(*) FROM " & cnAdminDb & "..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "BILLNO')> 0"
            strsql += " DROP TABLE " & cnAdminDb & "..TEMP" & systemId & "BILLNO"
            Cmd = New OleDbCommand(strsql, cn, tran)
            Cmd.ExecuteNonQuery()
            TranNo = GetBillNoValue("GEN-SM-PONO", tran)
            Sno = GetNewSno(TranSnoType.MR_ORDERCODE, tran)
            BatchNo = GetNewBatchno(cnCostId, dtptrandate.Value.ToString("yyyy-MM-dd"), tran)
            Dim _Accode As String = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME='" & cmbAcName.Text & "'", , , tran)

            strsql = vbCrLf + "INSERT INTO " & cnStockDb & "..MR_ORDER"
            strsql += vbCrLf + "(SNO,TRANNO,TRANDATE,ITEMID,SUBITEMID,PCS,GRSWT,NETWT,LESSWT,COSTID,COMPANYID,BATCHNO,REMARK1"
            strsql += vbCrLf + ",REMARK2,USERID,UPDATED,UPTIME,APPVER,ACCODE) VALUES "
            strsql += vbCrLf + "('" & Sno & "','" & TranNo & "' ,'" & dtptrandate.Value.ToString() & "' , '" & Itemid & "', '" & subItemid & "', "
            strsql += vbCrLf + " '" & txtOPcs_NUM.Text & "', '" & txtOGrsWt_WET.Text & "', '" & Val(txtOGrsWt_WET.Text) - Val(txtOLessWt_WET.Text) & "', '" & txtOLessWt_WET.Text & "', "
            strsql += vbCrLf + " '" & cnCostId & "','" & strCompanyId & "','" & BatchNo & "', '" & txtORemark1.Text & "' , '" & txtORemark2.Text & "' ,"
            strsql += vbCrLf + " '" & userId & "','" & Today.Date.ToString("yyyy-MM-dd") & "','" & Date.Now.ToLongTimeString & "','" & VERSION & "', '" & _Accode & "' )"
            Cmd = New OleDbCommand(strsql, cn, tran)
            Cmd.ExecuteNonQuery()
            funcInsertStoneDetails(TranNo, Sno)
            tran.Commit()
            tran = Nothing
            MsgBox("Saved Successfully...", MsgBoxStyle.Information)
            funcClear()
        Catch ex As Exception
            If Not tran Is Nothing Then
                tran.Rollback()
                tran = Nothing
                MsgBox(ex.Message + vbCrLf + ex.StackTrace, MsgBoxStyle.Information)
            End If
        End Try
    End Sub

    Public Sub AccountStone()
        strsql = " SELECT ACNAME,ACCODE FROM " & cnAdminDb & "..ACHEAD "
        If InclCusttype = "Y" Then
            strsql += " WHERE ACTYPE IN (SELECT DISTINCT TYPEID FROM " & cnAdminDb & "..ACCTYPE "
            strsql += " WHERE ACTYPE IN ('G','D','I','C'))"
        Else
            strsql += " WHERE ACTYPE IN (SELECT DISTINCT TYPEID FROM " & cnAdminDb & "..ACCTYPE "
            strsql += " WHERE ACTYPE IN ('G','D','I'))"
        End If
        strsql += GetAcNameQryFilteration()
        strsql += " ORDER BY ACNAME"
        Dim dtAcName As New DataTable
        Da = New OleDbDataAdapter(strsql, cn)
        Da.Fill(dtAcName)
        cmbAcName.DataSource = dtAcName
        cmbAcName.ValueMember = "ACCODE"
        cmbAcName.DisplayMember = "ACNAME"
        cmbAcName.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cmbAcName.AutoCompleteSource = AutoCompleteSource.ListItems
    End Sub

    Private Sub funcInsertStoneDetails(ByVal Tranno As Integer, ByVal Sno As String)
        For Each stRow As DataRow In obj.dtGridStone.Rows
            Dim oSno = GetNewSno(TranSnoType.MR_ORDERSTONECODE, tran)
            Dim StnItemId As Integer
            Dim StnsubItemId As Integer
            StnItemId = Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "  ..ITEMMAST WHERE ITEMNAME='" & stRow.Item("ITEM").ToString & "'", , , tran))
            StnsubItemId = Val(objGPack.GetSqlValue("SELECT SUBITEMID FROM " & cnAdminDb & "  ..SUBITEMMAST WHERE SUBITEMNAME='" & stRow.Item("SUBITEM").ToString & "'", , , tran))
            strsql = vbCrLf + "INSERT INTO " & cnStockDb & "..MR_ORDERSTONE"
            strsql += vbCrLf + "(SNO,OSNO,TRANNO,TRANDATE,STNITEMID,STNSUBITEMID,STNPCS,STNWT,COSTID,COMPANYID,BATCHNO,APPVER) VALUES "
            strsql += vbCrLf + "('" & oSno & "','" & Sno & "' ,'" & Tranno & "' ,'" & dtptrandate.Value.ToString() & "' ,"
            strsql += vbCrLf + " '" & StnItemId & "','" & StnsubItemId & "',"
            strsql += vbCrLf + " '" & Val(stRow.Item("PCS").ToString) & "','" & Val(stRow.Item("WEIGHT").ToString) & "' ,"
            strsql += vbCrLf + " '" & cnCostId & "' ,'" & strCompanyId & "','" & BatchNo & "','" & VERSION & "' )"
            Cmd = New OleDbCommand(strsql, cn, tran)
            Cmd.ExecuteNonQuery()
        Next
    End Sub

    Public Sub funcClear()
        txtOPcs_NUM.Text = ""
        txtOLessWt_WET.Text = ""
        txtORemark1.Text = ""
        txtORemark2.Text = ""
        funcLoadItem()
        funcLoadSubItem()
        cmbOItem_OWN.Focus()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub frmPurchaseOrder_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmPurchaseOrder_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.BackColor = SystemColors.InactiveCaption
        objGPack.Validator_Object(Me)
        funcLoadItem()
        funcLoadSubItem()
        AccountStone()
    End Sub

    Private Sub funcLoadItem()
        strsql = vbCrLf + "SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ACTIVE,'Y')<>'N'"
        dt = New DataTable()
        Da = New OleDbDataAdapter(strsql, cn)
        Da.Fill(dt)
        cmbOItem_OWN.DataSource = dt
        cmbOItem_OWN.DisplayMember = "ITEMNAME"
        cmbOItem_OWN.ValueMember = "ITEMNAME"
    End Sub

    Private Sub funcLoadSubItem()
        Dim Itemid As Integer = Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbOItem_OWN.Text & "'", , , tran))
        strsql = vbCrLf + "SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = '" & Itemid & "' AND ISNULL(ACTIVE,'Y')<>'N'"
        dt = New DataTable()
        Da = New OleDbDataAdapter(strsql, cn)
        Da.Fill(dt)
        cmbOSubItem_OWN.DataSource = dt
        cmbOSubItem_OWN.DisplayMember = "SUBITEMNAME"
        cmbOSubItem_OWN.ValueMember = "SUBITEMNAME"
    End Sub

    Private Sub cmbOItem_SelectedValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbOItem_OWN.SelectedValueChanged
        funcLoadSubItem()
    End Sub

    Private Sub txtOGrsWt_WET_PreviewKeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles txtOGrsWt_WET.PreviewKeyDown
        If e.KeyCode = Keys.Enter Then
            obj = New frmStoneDia
            obj.grsWt = Val(txtOGrsWt_WET.Text)
            obj.BackColor = Me.BackColor
            obj.StartPosition = FormStartPosition.CenterScreen
            obj.MaximizeBox = False
            obj.StyleGridStone(obj.gridStone)
            obj.FromFlag = "A"
            obj.txtStItem.Select()
            obj.ShowDialog()
            Dim stnWt As Double = Val(obj.gridStoneTotal.Rows(0).Cells("WEIGHT").Value.ToString)
            Me.SelectNextControl(txtOGrsWt_WET, True, True, True, True)
            txtOLessWt_WET.Text = IIf(stnWt <> 0, Format(stnWt, "0.000"), Nothing)
            CalcONetWt()
        End If
    End Sub
    Private Sub CalcONetWt()
        Dim netWt As Decimal = Val(txtOGrsWt_WET.Text) - Val(txtOLessWt_WET.Text)
        txtONetWt_WET.Text = IIf(netWt <> 0, Format(netWt, "0.000"), "")
    End Sub
End Class