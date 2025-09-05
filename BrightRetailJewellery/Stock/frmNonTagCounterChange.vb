Imports System.Data.OleDb
Public Class frmNonTagCounterChange
    Dim strSql As String
    Dim cmd As OleDbCommand

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click, SaveToolStripMenuItem.Click
        If objGPack.Validator_Check(Me) Then Exit Sub
        If txtOldCounterName.Text = "" Then
            MsgBox("Invalid Counter", MsgBoxStyle.Information)
            txtOldCounterId_NUM_MAN.Select()
            Exit Sub
        End If
        If txtNewCounterName.Text = "" Then
            MsgBox("Invalid Counter", MsgBoxStyle.Information)
            txtNewCounterId_NUM_MAN.Select()
            Exit Sub
        End If
        If Not ValidationPcs() Then Exit Sub
        If Not ValidationGrswt() Then Exit Sub
        If Not ValidationNetWt() Then Exit Sub
        Try
            tran = Nothing
            tran = cn.BeginTransaction
            InsertIntoNonTag("I", Val(txtOldCounterId_NUM_MAN.Text))
            InsertIntoNonTag("R", Val(txtNewCounterId_NUM_MAN.Text))
            tran.Commit()
            tran = Nothing
            MsgBox("Transfered Completed..")
            btnNew_Click(Me, New EventArgs)
        Catch ex As Exception
            If tran IsNot Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        objGPack.TextClear(Me)
        cmbItemType_MAN.Enabled = True
        dtpTranDate.Value = GetEntryDate(GetServerDate)
        If cmbCostCentre_MAN.Enabled Then cmbCostCentre_MAN.Select() Else dtpTranDate.Select()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub grpFocus(ByVal sender As Object, ByVal e As EventArgs) Handles _
    txtOldPcs.GotFocus, txtOldGrsWt.GotFocus, txtOldNetWt.GotFocus _
    , txtNewPcs.GotFocus, txtNewGrsWt.GotFocus, txtNewNetWt.GotFocus
        dtpTranDate.Select()
    End Sub

    Private Sub GetCounterStock(ByVal txtCtrId As TextBox)
        If txtCtrId.Name = txtOldCounterId_NUM_MAN.Name Then objGPack.TextClear(grpOld) Else objGPack.TextClear(grpNew)
        strSql = " SELECT "
        strSql += " SUM(CASE WHEN RECISS = 'R' THEN PCS ELSE -1*PCS END) AS PCS"
        strSql += " ,SUM(CASE WHEN RECISS = 'R' THEN GRSWT ELSE -1*GRSWT END) AS GRSWT"
        strSql += " ,SUM(CASE WHEN RECISS = 'R' THEN NETWT ELSE -1*NETWT END) AS NETWT"
        strSql += " FROM " & cnAdminDb & "..ITEMNONTAG"
        strSql += " WHERE ITEMID = " & Val(txtItemId_NUM.Text) & ""
        strSql += " AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & txtSubItemName.Text & "' AND ITEMID = " & Val(txtItemId_NUM.Text) & "),0)"
        strSql += " AND ITEMCTRID = " & Val(txtCtrId.Text) & ""
        strSql += " AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'),'')"
        strSql += " AND COMPANYID = '" & GetStockCompId() & "'"
        strSql += " AND ISNULL(CANCEL,'') <> 'Y'"
        If _HideBackOffice Then strSql += vbCrLf + " AND ISNULL(FLAG,'') <> 'B'"
        Dim dt As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then Exit Sub
        With dt.Rows(0)
            If txtCtrId.Name = txtOldCounterId_NUM_MAN.Name Then
                txtOldPcs.Text = IIf(Val(.Item("PCS").ToString) <> 0, .Item("PCS").ToString, "")
                txtOldGrsWt.Text = IIf(Val(.Item("GRSWT").ToString) <> 0, Format(Val(.Item("GRSWT").ToString), "0.000"), "")
                txtOldNetWt.Text = IIf(Val(.Item("NETWT").ToString) <> 0, Format(Val(.Item("NETWT").ToString), "0.000"), "")
            Else
                txtNewPcs.Text = IIf(Val(.Item("PCS").ToString) <> 0, .Item("PCS").ToString, "")
                txtNewGrsWt.Text = IIf(Val(.Item("GRSWT").ToString) <> 0, Format(Val(.Item("GRSWT").ToString), "0.000"), "")
                txtNewNetWt.Text = IIf(Val(.Item("NETWT").ToString) <> 0, Format(Val(.Item("NETWT").ToString), "0.000"), "")
            End If
        End With
    End Sub


    Private Sub LoadCounter(ByVal txt As TextBox)
        strSql = " SELECT ITEMCTRID ID,ITEMCTRNAME COUNTER FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ACTIVE = 'Y' ORDER BY ITEMCTRNAME"
        Dim itemCtrId As Integer = Nothing
        itemCtrId = Val(BrighttechPack.SearchDialog.Show("Select Counter Name", strSql, cn, 1))
        If itemCtrId <> Nothing Then
            txt.Text = itemCtrId
            LoadCounterDetail(txt)
        End If
    End Sub

    Private Sub LoadCounterDetail(ByVal txtCtrId As TextBox)
        Dim ctrName As String = Nothing
        ctrName = objGPack.GetSqlValue("SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ACTIVE = 'Y' AND ITEMCTRID = " & Val(txtCtrId.Text) & "")
        If ctrName <> "" Then
            GetCounterStock(txtCtrId)
            Me.GetNextControl(txtCtrId, True).Text = ctrName
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtOldCounterId_NUM_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOldCounterId_NUM_MAN.GotFocus
        Main.ShowHelpText("Press Insert Key to Help")
    End Sub

    Private Sub txtOldCounterId_NUM_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOldCounterId_NUM_MAN.LostFocus
        Main.HideHelpText()
    End Sub

    Private Sub txtOldCounterId_NUM_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtOldCounterId_NUM_MAN.KeyDown
        If e.KeyCode = Keys.Insert Then
            LoadCounter(txtOldCounterId_NUM_MAN)
        End If
    End Sub

    Private Sub txtOldCounterId_NUM_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtOldCounterId_NUM_MAN.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If objGPack.GetSqlValue("SELECT 'CHECK' FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = " & Val(txtOldCounterId_NUM_MAN.Text) & " AND ACTIVE = 'Y'").Length > 0 Then
                LoadCounterDetail(txtOldCounterId_NUM_MAN)
            Else
                LoadCounter(txtOldCounterId_NUM_MAN)
            End If
        End If
    End Sub

    Private Sub txtNewCounterId_NUM_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNewCounterId_NUM_MAN.GotFocus
        Main.ShowHelpText("Press Insert Key to Help")
    End Sub

    Private Sub txtNewCounterId_NUM_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtNewCounterId_NUM_MAN.KeyDown
        If e.KeyCode = Keys.Insert Then
            LoadCounter(txtNewCounterId_NUM_MAN)
        End If
    End Sub

    Private Sub txtNewCounterId_NUM_MAN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtNewCounterId_NUM_MAN.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If objGPack.GetSqlValue("SELECT 'CHECK' FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = " & Val(txtNewCounterId_NUM_MAN.Text) & " AND ACTIVE = 'Y'").Length > 0 Then
                LoadCounterDetail(txtNewCounterId_NUM_MAN)
            Else
                LoadCounter(txtNewCounterId_NUM_MAN)
            End If
        End If
    End Sub

    Private Sub txtNewCounterId_NUM_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNewCounterId_NUM_MAN.LostFocus
        Main.HideHelpText()
    End Sub

    Private Sub txtNewCounterName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNewCounterName.GotFocus
        SendKeys.Send("{TAB}")
    End Sub

    Private Sub txtOldCounterName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOldCounterName.GotFocus
        SendKeys.Send("{TAB}")
    End Sub

    Private Sub frmNonTagCounterChange_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtItemId_NUM.Focused Then Exit Sub
            If txtOldCounterId_NUM_MAN.Focused Then Exit Sub
            If txtNewCounterId_NUM_MAN.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Function ValidationPcs() As Boolean
        If Val(txtPcs_NUM.Text) = 0 Then
            MsgBox("Pieces should not empty", MsgBoxStyle.Information)
            Return False
        ElseIf Val(txtPcs_NUM.Text) > Val(txtOldPcs.Text) Then
            MsgBox("Pcs should not exceed the available stock", MsgBoxStyle.Information)
            Return False
        End If
        Return True
    End Function

    Private Function ValidationGrswt() As Boolean
        If txtSubItemName.Text <> "" Then
            strSql = " SELECT CALTYPE FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & txtSubItemName.Text & "'"
        Else
            strSql = " SELECT CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtItemName.Text & "'"
        End If
        If objGPack.GetSqlValue(strSql, , "W") <> "R" Then
            If Val(txtGrsWt_WET.Text) = 0 Then
                MsgBox("GrsWt should not empty", MsgBoxStyle.Information)
                Return False
            ElseIf Val(txtGrsWt_WET.Text) > Val(txtOldGrsWt.Text) Then
                MsgBox("GrsWt should not exceed the available stock", MsgBoxStyle.Information)
                Return False
            End If
        End If
        Return True
    End Function

    Private Function ValidationNetWt() As Boolean
        If txtSubItemName.Text <> "" Then
            strSql = " SELECT CALTYPE FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & txtSubItemName.Text & "'"
        Else
            strSql = " SELECT CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtItemName.Text & "'"
        End If
        If objGPack.GetSqlValue(strSql, , "W") <> "R" Then
            If Val(txtNetWt_WET.Text) = 0 Then
                MsgBox("NetWt should not empty", MsgBoxStyle.Information)
                Return False
            ElseIf Val(txtNetWt_WET.Text) > Val(txtOldNetWt.Text) Then
                MsgBox("Netwt should not exceed the available stock", MsgBoxStyle.Information)
                Return False
            End If
        End If
        Return True
    End Function

    Private Sub txtPcs_NUM_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPcs_NUM.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If Not ValidationPcs() Then txtPcs_NUM.Select()
        End If
    End Sub

    Private Sub txtGrsWt_WET_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtGrsWt_WET.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If Not ValidationGrswt() Then txtGrsWt_WET.Select()
        End If
    End Sub

    Private Sub txtNetWt_WET_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtNetWt_WET.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If Not ValidationNetWt() Then txtNetWt_WET.Select()
        End If
    End Sub

    Private Sub InsertIntoNonTag(ByVal recIss As String, ByVal itemCtrId As Integer)
        Dim COSTID As String = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'", , , tran)
        strSql = " INSERT INTO " & cnAdminDb & "..ITEMNONTAG"
        strSql += " ("
        strSql += " SNO,ITEMID,SUBITEMID,COMPANYID,RECDATE,"
        strSql += " PCS,GRSWT,LESSWT,NETWT,"
        strSql += " FINRATE,ISSTYPE,RECISS,POSTED,"
        strSql += " PACKETNO,DREFNO,ITEMCTRID,"
        strSql += " ORDREPNO,ORSNO,NARRATION,"
        strSql += " RATE,COSTID,"
        strSql += " CTGRM,DESIGNERID,VATEXM,ITEMTYPEID,"
        strSql += " CARRYFLAG,REASON,BATCHNO,CANCEL,"
        strSql += " USERID,UPDATED,UPTIME,SYSTEMID,APPVER)VALUES("
        strSql += " '" & GetNewSno(TranSnoType.ITEMNONTAGCODE, tran, "GET_ADMINSNO_TRAN") & "'" 'SNO
        strSql += " ," & Val(txtItemId_NUM.Text) & "" 'ItemId
        strSql += " ," & Val(objGPack.GetSqlValue("SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & txtSubItemName.Text & "' AND ITEMID = " & Val(txtItemId_NUM.Text) & "", , , tran)) & "" 'SubItemId
        strSql += " ,'" & GetStockCompId() & "'" 'Companyid
        strSql += " ,'" & GetEntryDate(dtpTranDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'Recdate
        strSql += " ," & Val(txtPcs_NUM.Text) & "" 'Pcs
        strSql += " ," & Val(txtGrsWt_WET.Text) & "" 'GrsWt
        strSql += " ," & Val(txtGrsWt_WET.Text) - Val(txtNetWt_WET.Text) & "" 'LessWt
        strSql += " ," & Val(txtNetWt_WET.Text) & "" 'NetWt
        strSql += " ,0" 'FinRate
        strSql += " ,''" 'Isstype
        strSql += " ,'" & recIss & "'" 'RecIss
        strSql += " ,'C'" 'Posted
        strSql += " ,''" 'Packetno
        strSql += " ,0" 'DRefno
        strSql += " ," & itemCtrId & "" 'ItemCtrId
        strSql += " ,''" 'OrdRepNo
        strSql += " ,''" 'OrSNO
        strSql += " ,''" 'Narration
        strSql += " ,0" 'Rate
        strSql += " ,'" & COSTID & "'" 'COSTID
        strSql += " ,''"
        strSql += " ," & Val(objGPack.GetSqlValue("SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & cmbDesigner_MAN.Text & "'", , , tran)) & "" 'DesignerId
        strSql += " ,''" 'VATEXM
        strSql += " ," & Val(objGPack.GetSqlValue("SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & cmbItemType_MAN.Text & "'", , , tran)) & "" 'ItemTypeID
        strSql += " ,''" 'Carryflag
        strSql += " ,'0'" 'Reason
        strSql += " ,''" 'BatchNo
        strSql += " ,''" 'Cancel
        strSql += " ," & userId & "" 'UserId
        strSql += " ,'" & GetEntryDate(GetServerDate(tran), tran) & "'" 'Updated
        strSql += " ,'" & GetServerTime(tran) & "'" 'Uptime
        strSql += " ,'" & systemId & "'" 'Systemid
        strSql += " ,'" & VERSION & "'" 'APPVER
        strSql += " )"
        ExecQuery(SyncMode.Stock, strSql, cn, tran, COSTID)
    End Sub

    Private Sub LoadItem()
        strSql = " SELECT ITEMID,ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE STOCKTYPE = 'N' AND ACTIVE = 'Y' "
        strSql += GetItemQryFilteration("S")
        strSql += " ORDER BY ITEMNAME"
        Dim itemId As Integer = Nothing
        itemId = Val(BrighttechPack.SearchDialog.Show("Select ItemName", strSql, cn, 1))
        If itemId > 0 Then
            txtItemId_NUM.Text = itemId
            LoadItemDetail()
        End If
    End Sub

    Private Sub LoadItemDetail()
        strSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & Val(txtItemId_NUM.Text) & ""
        strSql += GetItemQryFilteration("S")
        Dim itemName As String = Nothing
        itemName = objGPack.GetSqlValue(strSql)
        If itemName <> "" Then
            If objGPack.GetSqlValue("SELECT TAGTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & Val(txtItemId_NUM.Text) & "").ToString = "Y" Then
                cmbItemType_MAN.Enabled = True
            Else
                cmbItemType_MAN.Enabled = False
            End If
            txtItemName.Text = itemName
            Dim DefItem As String = txtSubItemName.Text
            If Not _SubItemOrderByName Then
                DefItem = objGPack.GetSqlValue("SELECT DISPLAYORDER FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & txtSubItemName.Text & "' AND ITEMID = " & Val(txtItemId_NUM.Text) & "")
            End If
            strSql = GetSubItemQry(New String() {"SUBITEMID ID", "DISPLAYORDER DISP", "SUBITEMNAME SUBITEM"}, Val(txtItemId_NUM.Text))
            txtSubItemName.Text = BrighttechPack.SearchDialog.Show("Search SubItem", strSql, cn, IIf(_SubItemOrderByName, 2, 1), 2, , DefItem, , False, True)
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtItemId_NUM_MAN_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtItemId_NUM.GotFocus
        Main.ShowHelpText("Press Insert Key to Help")
    End Sub

    Private Sub txtItemId_NUM_MAN_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtItemId_NUM.LostFocus
        Main.HideHelpText()
    End Sub

    Private Sub txtItemId_NUM_MAN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtItemId_NUM.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If objGPack.GetSqlValue("SELECT 'CHECK' FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & Val(txtItemId_NUM.Text) & "").Length > 0 Then
                LoadItemDetail()
            Else
                LoadItem()
            End If
        End If
    End Sub

    Private Sub txtItemId_NUM_MAN_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtItemId_NUM.KeyDown
        If e.KeyCode = Keys.Insert Then
            LoadItem()
        End If
    End Sub

    Private Sub txtSubItemName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSubItemName.GotFocus
        SendKeys.Send("{TAB}")
    End Sub

    Private Sub txtItemName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtItemName.GotFocus
        SendKeys.Send("{TAB}")
    End Sub

    Private Sub frmNonTagCounterChange_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        strSql = " SELECT DESIGNERNAME FROM " & CNADMINDB & "..DESIGNER WHERE ACTIVE = 'Y' ORDER BY DESIGNERNAME"
        objGPack.FillCombo(strSql, cmbDesigner_MAN)
        strSql = " SELECT NAME FROM " & CNADMINDB & "..ITEMTYPE ORDER BY NAME "
        objGPack.FillCombo(strSql, cmbItemType_MAN)
        If GetAdmindbSoftValue("COSTCENTRE") = "Y" Then
            cmbCostCentre_MAN.Enabled = True
            strSql = " SELECT COSTNAME FROM " & CNADMINDB & "..COSTCENTRE ORDER BY COSTNAME"
            objGPack.FillCombo(strSql, cmbCostCentre_MAN)
        Else
            cmbCostCentre_MAN.Enabled = False
        End If
    End Sub
End Class