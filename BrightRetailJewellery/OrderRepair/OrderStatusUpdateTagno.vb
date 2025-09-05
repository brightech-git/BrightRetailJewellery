Imports System.Data.OleDb
Public Class OrderStatusUpdateTagno
    Public PRODTAGSEP As Char
    Dim StrSql As String
    Dim Da As OleDbDataAdapter
    Dim OrdSno As String
    Dim OrdNo As String
    Dim OrdCostId As String
    Dim updateflag As Boolean = False
    Dim SMS_ORDER_STATUSUPDATE As String = objGPack.GetSqlValue("SELECT ISNULL(TEMPLATE_DESC,'') AS TEMPLATE_DESC FROM " & cnAdminDb & "..SMSTEMPLATE WHERE TEMPLATE_NAME='SMS_ORDER_STATUSUPDATE' AND ISNULL(ACTIVE,'Y')<>'N'", "TEMPLATE_DESC").ToString

    Public Sub New(ByVal OrdSno As String, ByVal OrdNo As String, ByVal CostId As String, ByVal updflag As Boolean)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        PRODTAGSEP = GetAdmindbSoftValue("PRODTAGSEP", "")
        Me.OrdSno = OrdSno
        Me.OrdNo = OrdNo
        Me.updateflag = updflag
        If CostId <> "" Then
            Me.OrdCostId = CostId
        Else
            Me.OrdCostId = cnCostId
        End If
    End Sub
    Private Sub txtItemId_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtItemId.GotFocus
        Main.ShowHelpText("Press Insert Key to Help")
    End Sub
    Private Sub LoadSalesItemName()
        StrSql = " SELECT"
        StrSql += " ITEMID AS ITEMID,ITEMNAME AS ITEMNAME,"
        StrSql += " CASE WHEN STOCKTYPE = 'T' THEN 'TAGGED' "
        StrSql += " WHEN STOCKTYPE = 'N' THEN 'NON TAGGED' ELSE 'POCKET BASED' END AS STOCK_TYPE,"
        StrSql += " CASE WHEN CALTYPE = 'W' THEN 'WEIGHT'"
        StrSql += " WHEN CALTYPE = 'W' THEN 'WEIGHT'"
        StrSql += " WHEN CALTYPE = 'R' THEN 'RATE'"
        StrSql += " WHEN CALTYPE = 'F' THEN 'FIXED'"
        StrSql += " WHEN CALTYPE = 'B' THEN 'BOTH' ELSE 'METAL RATE' END AS CAL_TYPE"
        StrSql += " FROM " & cnAdminDb & "..ITEMMAST"
        StrSql += " WHERE ISNULL(STUDDED,'') <> 'S' AND ACTIVE = 'Y'"
        StrSql += " AND STOCKTYPE = 'T'"
        StrSql += GetItemQryFilteration()
        Dim itemId As Integer = Val(BrighttechPack.SearchDialog.Show("Find ItemName", StrSql, cn, 1, , , txtItemId.Text))
        If itemId > 0 Then
            txtItemId.Text = itemId
            LoadSalesItemNameDetail()
        Else
            txtItemId.Focus()
            txtItemId.SelectAll()
        End If
    End Sub
    Private Sub txtItemId_NUM_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtItemId.KeyDown
        If e.KeyCode = Keys.Insert Then
            LoadSalesItemName()
        End If
    End Sub
    Private Sub txtItemId_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtItemId.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            Dim sp() As String = txtItemId.Text.Split(PRODTAGSEP)
            If PRODTAGSEP <> "" And txtItemId.Text <> "" Then
                sp = txtItemId.Text.Split(PRODTAGSEP)
                txtItemId.Text = sp(0)
            End If
            Dim sep As String = Nothing
            For Each c As Char In txtItemId.Text
                If Not Char.IsNumber(c) Then sep += c & ","
            Next
            If sep <> Nothing Then
                sep.Remove(sep.Length - 1, 1)
                Dim s() As String = txtItemId.Text.Split(sep)
                txtItemId.Text = s(0)
            End If
            If txtItemId.Text = "" Then
                LoadSalesItemName()
            ElseIf txtItemId.Text <> "" And objGPack.DupCheck("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & Val(txtItemId.Text) & "" & GetItemQryFilteration()) = False Then
                LoadSalesItemName()
            Else
                LoadSalesItemNameDetail()
            End If
            If sp.Length > 1 And PRODTAGSEP <> "" And txtItemId.Text <> "" Then
                txtTagNo.Text = sp(1)
            End If
            If txtTagNo.Text <> "" Then
                txtTagNo.Focus()
            End If
        End If
    End Sub
    Private Sub txtItemId_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtItemId.LostFocus
        Main.HideHelpText()
    End Sub
    Private Sub LoadSalesItemNameDetail()
        If txtItemId.Text = "" Then
            Exit Sub
        End If
        txtItemName.Text = objGPack.GetSqlValue("SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & Val(txtItemId.Text) & " AND ISNULL(STUDDED,'') <> 'S' AND ACTIVE = 'Y'")
        txtTagNo.Focus()
    End Sub

    Private Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub txtItemName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtItemName.GotFocus
        SendKeys.Send("{TAB}")
    End Sub

    Private Sub txtTagNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtTagNo.KeyDown
        If e.KeyCode = Keys.Insert Then
            If txtItemName.Text = "" Then Exit Sub
            StrSql = " SELECT"
            StrSql += " TAGNO AS TAGNO,ITEMID AS ITEMID,"
            StrSql += " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS ITEMNAME,"
            StrSql += " PCS AS PCS,"
            StrSql += " GRSWT AS GRS_WT,NETWT AS NET_WT,RATE AS RATE,"
            StrSql += " SALVALUE AS SALVALUE,TAGVAL AS TAGVAL,"
            StrSql += " ISNULL((SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID  = T.SUBITEMID),'')AS SUBITEM,"
            StrSql += " CONVERT(VARCHAR,RECDATE,103) AS RECDATE,"
            StrSql += " CASE WHEN APPROVAL = 'A' THEN 'APPROVAL' WHEN APPROVAL = 'R' THEN 'RESERVED' ELSE NULL END AS STATUS,"
            StrSql += " CASE WHEN APPROVAL = 'A' THEN '" & Color.MistyRose.Name & "' WHEN APPROVAL = 'R' THEN '" & Color.PaleTurquoise.Name & "' ELSE NULL END AS COLOR_HIDE"
            StrSql += " FROM"
            StrSql += " " & cnAdminDb & "..ITEMTAG AS T"
            StrSql += " WHERE T.ITEMID = " & Val(txtItemId.Text) & ""
            If Not cnCentStock Then StrSql += " AND COMPANYID = '" & GetStockCompId() & "'"
            StrSql += RetailBill.ShowTagFiltration
            StrSql += " AND ISNULL(T.COSTID,'') = '" & OrdCostId & "'"
            StrSql += " AND ISSDATE IS NULL"
            StrSql += " ORDER BY TAGNO"
            txtTagNo.Text = BrighttechPack.SearchDialog.Show("Find TagNo", StrSql, cn, , , , , , , , False)
            txtTagNo.SelectAll()
        End If
    End Sub

    Private Sub txtTagNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtTagNo.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtItemName.Text = "" Then Exit Sub
            If txtTagNo.Text = "" Then
                MsgBox("TagNo Should Not Empty", MsgBoxStyle.Information)
                txtTagNo.Select()
                Exit Sub
            End If
            If LoadTagDetails(txtItemId, txtTagNo) Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Function LoadTagDetails(ByVal txtId As Object, ByVal txtTagNo As Object) As Boolean
        Dim itemId As Integer = Nothing
        Dim tagNo As String = Nothing
        If TypeOf txtId Is TextBox Then
            itemId = Val(CType(txtId, TextBox).Text)
        Else
            itemId = Val(CType(txtId, Integer))
        End If
        If TypeOf txtTagNo Is TextBox Then
            tagNo = CType(txtTagNo, TextBox).Text
        Else
            tagNo = txtTagNo.ToString
        End If
        If objGPack.GetSqlValue("SELECT STOCKTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & itemId & "", , , tran) <> "T" Then
            Exit Function
        End If
        If tagNo = "B" Or tagNo = "C" Then
            Exit Function
        End If
        Dim rwIndex As Integer = -1
        StrSql = " SELECT * FROM " & cnAdminDb & "..ITEMTAG"
        StrSql += " WHERE ITEMID = " & itemId & ""
        StrSql += " AND  TAGNO = '" & tagNo & "'"
        StrSql += " AND ISNULL(COSTID,'') = '" & OrdCostId & "'"
        Dim dtTagCheck As New DataTable
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dtTagCheck)
        If Not dtTagCheck.Rows.Count > 0 Then
            MsgBox(E0004 + Me.GetNextControl(txtTagNo, False).Text, MsgBoxStyle.Information)
            If TypeOf txtTagNo Is TextBox Then
                txtTagNo.Select()
                txtTagNo.SelectAll()
            End If
            Return True
        Else
            Dim errStr As String = Nothing
            With dtTagCheck.Rows(0)
                If .Item("ISSDATE").ToString <> "" Then
                    errStr += "TAG ALREADY ISSUED" + vbCrLf
                    errStr += "TAGNO   :" + .Item("TAGNO").ToString + vbCrLf
                    errStr += "REFNO   : " + .Item("ISSREFNO").ToString + vbCrLf
                    errStr += "REFDATE :" + Format(.Item("ISSDATE"), "dd/MM/yyyy").ToString
                ElseIf .Item("APPROVAL").ToString = "R" Then
                    errStr += "TAG ALREADY RESERVED.." + vbCrLf
                    errStr += "TAGNO    = " & .Item("TAGNO").ToString & "" + vbCrLf
                    errStr += "REFNO    = " & .Item("ISSREFNO").ToString & "" + vbCrLf
                    errStr += "TYPE     = RESERVED" + vbCrLf
                    errStr += "BATCHNO  = " & .Item("BATCHNO").ToString & ""
                ElseIf .Item("APPROVAL").ToString = "A" Then
                    errStr += "TAG ALREADY RESERVED.." + vbCrLf
                    errStr += "TAGNO    = " & .Item("TAGNO").ToString & "" + vbCrLf
                    errStr += "TRANNO   = " & objGPack.GetSqlValue("SELECT TRANNO FROM " & cnStockDb & "..ISSUE WHERE BATCHNO = '" & .Item("BATCHNO").ToString & "'") & "" + vbCrLf
                    errStr += "TYPE     = APPROVAL" + vbCrLf
                    errStr += "BATCHNO  = " & .Item("BATCHNO").ToString & ""
                ElseIf .Item("ORSNO").ToString <> "" Then
                    errStr += IIf(.Item("ORDREPNO").ToString.StartsWith("O"), "ORDERED TAG..", "REPAIR TAG..") + vbCrLf
                    errStr += "TAGNO    = " & .Item("TAGNO").ToString & "" + vbCrLf
                    errStr += "TRANNO   = " & objGPack.GetSqlValue("SELECT TRANNO FROM " & cnStockDb & "..ISSUE WHERE BATCHNO = '" & .Item("BATCHNO").ToString & "'") & "" + vbCrLf
                    errStr += "TYPE     = " & IIf(.Item("ORDREPNO").ToString.StartsWith("O"), "ORDERED", "REPAIR") + vbCrLf
                    errStr += "ORDERNO  = " & .Item("ORDREPNO").ToString & " (" & .Item("ORSNO").ToString & ")" + vbCrLf
                End If
                If errStr <> Nothing Then
                    MsgBox(errStr, MsgBoxStyle.Information)
                    If TypeOf txtTagNo Is TextBox Then
                        txtTagNo.Select()
                        txtTagNo.SelectAll()
                    End If
                    Return True
                End If
            End With
        End If
    End Function

    Private Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        If txtItemName.Text = "" Then
            MsgBox("Invalid Item", MsgBoxStyle.Information)
            txtItemId.Focus()
            Exit Sub
        End If
        If LoadTagDetails(txtItemId, txtTagNo) Then Exit Sub
        StrSql = " SELECT * FROM " & cnAdminDb & "..ITEMTAG"
        StrSql += " WHERE ITEMID = " & Val(txtItemId.Text) & " AND TAGNO = '" & txtTagNo.Text & "'"
        StrSql += " AND ISNULL(COSTID,'') = '" & OrdCostId & "'"
        Dim dtTag As New DataTable
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dtTag)
        If Not dtTag.Rows.Count > 0 Then
            MsgBox("Invalid Tag Info", MsgBoxStyle.Information)
            Exit Sub
        End If
        Dim NewCounterId As Integer = Nothing
        If GetAdmindbSoftValue("ITEMCOUNTER", "N") = "Y" Then
            Select Case MessageBox.Show("Do you want to change counter for selected item", "Counter Change Alert", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
                Case Windows.Forms.DialogResult.Yes
                    NewCounterId = Val(BrighttechPack.SearchDialog.Show("Search Item Counter", "SELECT ITEMCTRID ID,ITEMCTRNAME COUNTER FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ACTIVE = 'Y' ORDER BY COUNTER", cn, 1, 0))
                    If NewCounterId = 0 Then
                        Exit Sub
                    End If
                Case Windows.Forms.DialogResult.No
                    Exit Select
                Case Else
                    Exit Sub
            End Select
        End If

        Dim Ro As DataRow = dtTag.Rows(0)
        Dim costId As String = Ro!COSTID.ToString
        Try
            tran = Nothing
            tran = cn.BeginTransaction
            If updateflag = False Then
                Dim batchno As String = GetNewBatchno(cnCostId, Ro.Item("RECDATE"), tran)
                StrSql = " INSERT INTO " & cnadmindb & "..ORIRDETAIL"
                StrSql += " ("
                StrSql += " SNO,ORSNO,TRANNO,TRANDATE,DESIGNERID,PCS,GRSWT,NETWT,TAGNO,ORSTATUS,CANCEL,COSTID,DESCRIPT,ORNO"
                StrSql += " ,BATCHNO,USERID,UPDATED,UPTIME,APPVER,COMPANYID,ENTRYORDER,ORDSTATE_ID,CATCODE"
                StrSql += " )"
                StrSql += " VALUES"
                StrSql += " ("
                StrSql += " '" & GetNewSno(TranSnoType.ORIRDETAILCODE, tran, "GET_ADMINSNO_TRAN") & "'" 'SNO
                StrSql += " ,'" & OrdSno & "'" 'ORSNO
                StrSql += " ," & Val(objGPack.GetSqlValue(" SELECT ISNULL(MAX(CTLTEXT),0)+1 AS TRANNO FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID  = 'ORDIRBILLNO' AND COMPANYID = '" & strCompanyId & "'", , , tran)) & "" 'TRANNO
                StrSql += " ,'" & Ro.Item("RECDATE") & "'" 'TRANDATE
                StrSql += " ," & Val(Ro.Item("DESIGNERID").ToString) & "" 'DESIGNERID
                StrSql += " ," & Val(Ro.Item("PCS").ToString) & "" 'PCS
                StrSql += " ," & Val(Ro.Item("GRSWT").ToString) & "" 'GRSWT
                StrSql += " ," & Val(Ro.Item("NETWT").ToString) & "" 'NETWT
                StrSql += " ,'" & Ro.Item("TAGNO").ToString & "'" 'TAGNO
                StrSql += " ,'R'" 'ORSTATUS
                StrSql += " ,''" 'CANCEL
                StrSql += " ,'" & costId & "'" 'COSTID
                StrSql += " ,'" & Ro.Item("NARRATION").ToString & "'" 'DESCRIPT
                StrSql += " ,'" & OrdNo & "'" 'ORNO
                StrSql += " ,'" & batchno & "'" 'BATCHNO
                StrSql += " ," & userId & "" 'USERID
                StrSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                StrSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                StrSql += " ,'" & VERSION & "'" 'APPVER
                StrSql += " ,'" & strCompanyId & "'" 'COMPANYID
                StrSql += " ," & objGPack.GetSqlValue("SELECT ISNULL(MAX(ENTRYORDER),0)+1 FROM " & cnadmindb & "..ORIRDETAIL", , , tran) & "" 'ENTRYORDER
                StrSql += " ,4" 'ORDSTATE_ID
                StrSql += " ,'" & objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & Val(txtItemId.Text) & "", , , tran) & "'" 'CATCODE
                StrSql += " )"
                ExecQuery(SyncMode.Stock, StrSql, cn, tran, GetAdmindbSoftValue("SYNC-TO", , tran))
            Else
                StrSql = " UPDATE " & cnadmindb & "..ORIRDETAIL"
                StrSql += " SET TAGNO='" & Ro.Item("TAGNO").ToString & "'"
                StrSql += " ,ORDSTATE_ID=4"
                StrSql += " WHERE ORSNO = '" & OrdSno & "'"
                StrSql += " AND ORNO = '" & OrdNo & "'"
                StrSql += " AND ISNULL(ORSTATUS,'') = 'R' AND COMPANYID = '" & strCompanyId & "'"
                ExecQuery(SyncMode.Stock, StrSql, cn, tran, GetAdmindbSoftValue("SYNC-TO", , tran))
            End If
            StrSql = " UPDATE " & cnAdminDb & "..ITEMTAG"
            StrSql += " SET ORSNO = '" & OrdSno & "'"
            StrSql += " ,ORDREPNO = '" & OrdNo & "'"
            StrSql += " WHERE ITEMID = " & Val(txtItemId.Text) & ""
            StrSql += " AND TAGNO = '" & txtTagNo.Text & "'"
            ExecQuery(SyncMode.Stock, StrSql, cn, tran, GetAdmindbSoftValue("SYNC-TO", , tran))
            If NewCounterId <> 0 Then
                StrSql = " IF (SELECT COUNT(*) FROM " & cnAdminDb & "..CTRANSFER WHERE TAGSNO = '" & Ro.Item("SNO").ToString & "')>0"
                StrSql += "     BEGIN"
                StrSql += "     UPDATE " & cnAdminDb & "..CTRANSFER SET ISSDATE = '" & GetEntryDate(GetServerDate(tran), tran) & "'"
                StrSql += "     WHERE TAGSNO = '" & Ro.Item("SNO").ToString & "'"
                StrSql += "     AND ISSDATE IS NULL"
                StrSql += "     END"
                StrSql += " ELSE"
                StrSql += "     BEGIN"
                StrSql += "     INSERT INTO " & cnAdminDb & "..CTRANSFER"
                StrSql += "     (SNO,TAGSNO,ITEMID,TAGNO,ITEMCTRID,RECDATE,ISSDATE,TAGVAL,USERID"
                StrSql += "     ,UPDATED,UPTIME,APPVER,ENTORDER,COSTID)"
                StrSql += "     SELECT '" & GetNewSno(TranSnoType.CTRANSFERCODE, tran, "GET_ADMINSNO_TRAN") & "' AS SNO,SNO TAGSNO"
                StrSql += "     ,ITEMID,TAGNO,ITEMCTRID,RECDATE,'" & GetEntryDate(GetServerDate(tran), tran) & "' AS ISSDATE"
                StrSql += "     ,TAGVAL," & userId & " USERID,'" & GetEntryDate(GetServerDate(tran), tran) & "'UPDATED,'" & GetServerTime(tran) & "' UPTIME,'" & VERSION & "' APPVER,1,COSTID"
                StrSql += "     FROM " & cnAdminDb & "..ITEMTAG WHERE SNO = '" & Ro.Item("SNO").ToString & "'"
                StrSql += "     END"
                ExecQuery(SyncMode.Stock, StrSql, cn, tran, GetAdmindbSoftValue("SYNC-TO", , tran))

                StrSql = vbCrLf + " DECLARE @ENTORDER INT"
                StrSql += vbCrLf + " SELECT @ENTORDER = ISNULL(MAX(ENTORDER),0)+1 FROM " & cnAdminDb & "..CTRANSFER WHERE TAGSNO = '" & Ro.Item("SNO").ToString & "'"
                StrSql += " INSERT INTO " & cnAdminDb & "..CTRANSFER"
                StrSql += " (SNO,TAGSNO,ITEMID,TAGNO,ITEMCTRID,RECDATE,TAGVAL,USERID"
                StrSql += " ,UPDATED,UPTIME,APPVER,ENTORDER,COSTID)"
                StrSql += " SELECT '" & GetNewSno(TranSnoType.CTRANSFERCODE, tran, "GET_ADMINSNO_TRAN") & "' AS SNO,SNO TAGSNO"
                StrSql += " ,ITEMID,TAGNO," & NewCounterId & ",'" & GetEntryDate(GetServerDate(tran), tran) & "' AS RECDATE"
                StrSql += " ,TAGVAL," & userId & " USERID,'" & GetEntryDate(GetServerDate(tran), tran) & "' UPDATED,'" & GetServerTime(tran) & "' UPTIME,'" & VERSION & "' APPVER,@ENTORDER,COSTID"
                StrSql += " FROM " & cnAdminDb & "..ITEMTAG WHERE SNO = '" & Ro.Item("SNO").ToString & "'"
                ExecQuery(SyncMode.Stock, StrSql, cn, tran, GetAdmindbSoftValue("SYNC-TO", , tran))

                StrSql = " UPDATE " & cnAdminDb & "..ITEMTAG SET ITEMCTRID = " & NewCounterId & ""
                StrSql += " ,TRANSFERDATE = '" & GetEntryDate(GetServerDate(tran), tran) & "'"
                StrSql += " WHERE SNO = '" & Ro.Item("SNO").ToString & "'"
                ExecQuery(SyncMode.Stock, StrSql, cn, tran, GetAdmindbSoftValue("SYNC-TO", , tran))
            End If

            tran.Commit()
            SendSms()
            Me.DialogResult = Windows.Forms.DialogResult.OK
            MsgBox("Updated Succesfully", MsgBoxStyle.Information)
            Me.Close()
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub
    Private Sub SendSms()
        Dim dtemp As New DataTable
        Dim TempMsg As String = ""
        StrSql = vbCrLf + "SELECT DISTINCT ISNULL(P.TITLE,'')TITLE ,ISNULL(P.INITIAL,'')INITIAL ,ISNULL(P.PNAME,'')PNAME ,ISNULL(P.MOBILE,'')MOBILE  FROM " & cnAdminDb & "..ORMAST O "
        StrSql += vbCrLf + "INNER JOIN " & cnAdminDb & "..CUSTOMERINFO C ON O.BATCHNO = C.BATCHNO "
        StrSql += vbCrLf + "INNER JOIN " & cnAdminDb & "..PERSONALINFO P ON P.SNO = C.PSNO "
        StrSql += vbCrLf + "WHERE O.ORNO = '" & OrdNo & "'"
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dtemp)
        If dtemp.Rows.Count > 0 And dtemp.Rows(0).Item("MOBILE").ToString <> "" Then
            If SMS_ORDER_STATUSUPDATE <> "" Then
                TempMsg = SMS_ORDER_STATUSUPDATE
                TempMsg = Replace(SMS_ORDER_STATUSUPDATE, vbCrLf, "")
                TempMsg = Replace(TempMsg, "<NAME>", IIf(dtemp.Rows(0).Item("TITLE").ToString <> "", dtemp.Rows(0).Item("TITLE").ToString, dtemp.Rows(0).Item("INITIAL").ToString) & " " & dtemp.Rows(0).Item("PNAME").ToString)
                TempMsg = Replace(TempMsg, "<ITEMNAME>", txtItemName.Text)
                TempMsg = Replace(TempMsg, "<ORDERNO>", Mid(OrdNo, 6, 20))
                If Mid(OrdNo, 6, 1) = "O" Then
                    TempMsg = Replace(TempMsg, "<BILLTYPE>", "Order")
                ElseIf Mid(OrdNo, 6, 1) = "R" Then
                    TempMsg = Replace(TempMsg, "<BILLTYPE>", "Repair")
                End If
                SmsSend(TempMsg, dtemp.Rows(0).Item("MOBILE").ToString)
            End If
        End If

    End Sub
    Private Sub txtTagNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTagNo.GotFocus
        Main.ShowHelpText("Press Insert Key to Help")
    End Sub

    Private Sub txtTagNo_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTagNo.LostFocus
        Main.HideHelpText()
    End Sub
End Class