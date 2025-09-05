Imports System.Data.OleDb
Public Class BillOrderSearch
    Dim StrSql As String
    Public OrderRow As DataRow = Nothing
    Public SelectedOrder As String = Nothing
    Public SelectedTags As String = Nothing
    Public REFNOPREFIXAUTO As String = ""
    Public PendingAdjustment As Boolean = False
    Public BillCostId As String
    Public ORDREP As String
    Public IsRmitems As Boolean = False
    Dim Cmd As OleDbCommand
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
        objGPack.TextClear(Me)
        If ORDREP = "R" Then Label1.Text = "Repair No" Else Label1.Text = "Order No"
    End Sub

    Private Sub BillOrderSearch_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Insert Then
            'StrSql = vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP" & systemId & "ORMAST','V') IS NOT NULL DROP VIEW TEMPTABLEDB..TEMP" & systemId & "ORMAST"
            'StrSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP" & systemId & "ORMAST','U') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP" & systemId & "ORMAST"
            'Cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            'Cmd.ExecuteNonQuery()

            StrSql = vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP" & systemId & "ORMAST','U') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP" & systemId & "ORMAST"
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()

            'StrSql = vbCrLf + " CREATE VIEW TEMPTABLEDB..TEMP" & systemId & "ORMAST"
            'StrSql += vbCrLf + " AS"
            StrSql = vbCrLf + "  SELECT O.ORNO"
            StrSql += vbCrLf + "  ,O.ITEMID"
            StrSql += vbCrLf + "  ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = O.ITEMID)AS ITEM"
            StrSql += vbCrLf + "  ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = O.SUBITEMID AND ITEMID = O.ITEMID)AS SUBITEM"
            StrSql += vbCrLf + "  ,(SELECT TOP 1 TAGNO FROM " & cnadmindb & "..ORIRDETAIL WHERE ORNO = O.ORNO AND ORSNO = O.SNO AND ISNULL(ORSTATUS,'') = 'R' AND ISNULL(CANCEL,'') = '')AS TAGNO"
            StrSql += vbCrLf + "  ,O.PCS,O.GRSWT,O.NETWT,O.RATE,O.ORRATE,O.SNO"
            StrSql += vbCrLf + "  ,O.BATCHNO,EMPID,O.DISCOUNT"
            StrSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "ORMAST FROM " & cnAdminDb & "..ORMAST O"
            StrSql += vbCrLf + " WHERE ISNULL(ODBATCHNO,'') = ''"
            StrSql += vbCrLf + " AND ORTYPE = '" & ORDREP & "' AND ISNULL(CANCEL,'') = ''"
            StrSql += vbCrLf + " AND ISNULL(COSTID,'') = '" & BillCostId & "'"
            StrSql += vbCrLf + " AND ISNULL(COMPANYID,'') = '" & strCompanyId & "'"
            StrSql += vbCrLf + " AND ISNULL(ORDCANCEL,'') = ''"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()

            StrSql = vbCrLf + " SELECT (SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = O.BATCHNO))AS NAME"
            StrSql += vbCrLf + " ,SUBSTRING(ORNO,6,20)ORNO,ITEMID"
            StrSql += vbCrLf + " ,ITEM,SUBITEM,TAGNO"
            StrSql += vbCrLf + " ,PCS,GRSWT,NETWT,RATE,ORRATE,SNO,EMPID,O.DISCOUNT"
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ORMAST AS O"
            Dim msgTit As String
            If ORDREP = "R" Then msgTit = "Select RepairItem" Else msgTit = "Select OrderItem"
            OrderRow = BrighttechPack.SearchDialog.Show_R(msgTit, StrSql, cn, , , , , , False)
            If Not OrderRow Is Nothing Then
                txtOrderRepNo.Text = OrderRow.Item("ORNO").ToString
                StrSql = vbCrLf + " SELECT TRANFLAG FROM " & cnAdminDb & "..OUTSTANDING AS O"
                StrSql += vbCrLf + " WHERE ISNULL(O.CANCEL,'') = ''  "
                StrSql += vbCrLf + " AND ISNULL(O.COSTID,'') = '" & BillCostId & "'"
                StrSql += vbCrLf + " AND ISNULL(O.COMPANYID,'') = '" & strCompanyId & "'"
                StrSql += vbCrLf + " AND O.RUNNO = '" & GetCostId(BillCostId) & GetCompanyId(strCompanyId) & txtOrderRepNo.Text & "'"
                If objGPack.GetSqlValue(StrSql, , "").ToString = "L" Then
                    MsgBox("This Order advance has been blocked.", MsgBoxStyle.Information)
                    Exit Sub
                End If
                txtOrderRepNo.Text = OrderRow.Item("ORNO").ToString
                Me.DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()
            Else
                ''Order Info Not Found
                If MessageBox.Show("Booked Order Not Found" + vbCrLf + "Do you want Adjust Pending Order Advance?", "Order Pending Advance", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                    StrSql = vbCrLf + " SELECT SUBSTRING(O.ORNO,6,20)ORNO,PER.PNAME"
                    StrSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN O.AMOUNT END)[REC AMT]"
                    StrSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN O.WEIGHT END)[REC WT]"
                    StrSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'P' THEN O.AMOUNT END)[PAY AMT]"
                    StrSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'P' THEN O.WEIGHT END)[PAY WT]"
                    StrSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN O.AMOUNT ELSE -1*O.AMOUNT END)[BAL AMT]"
                    StrSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN O.WEIGHT ELSE -1*O.WEIGHT END)[BAL WT]"
                    StrSql += vbCrLf + " ,O.COSTID,O.COMPANYID,CONVERT(VARCHAR(15),'')SNO,CONVERT(VARCHAR(12),'')BATCHNO"
                    StrSql += vbCrLf + " FROM"
                    StrSql += vbCrLf + " ("
                    StrSql += vbCrLf + " SELECT O.RUNNO AS ORNO,AMOUNT,NETWT AS WEIGHT"
                    StrSql += vbCrLf + " ,O.COSTID,O.COMPANYID,O.RECPAY"
                    StrSql += vbCrLf + " ,(SELECT TOP 1 BATCHNO FROM " & cnAdminDb & "..OUTSTANDING WHERE COSTID = O.COSTID AND COMPANYID = O.COMPANYID AND RUNNO = O.RUNNO AND ISNULL(CANCEL,'') = '' ORDER BY TRANDATE) AS BATCHNO"
                    StrSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING AS O"
                    StrSql += vbCrLf + " WHERE COSTID = '" & BillCostId & "' AND COMPANYID = '" & strCompanyId & "' AND SUBSTRING(RUNNO,6,1) = 'O'"
                    StrSql += vbCrLf + " AND BATCHNO NOT IN (SELECT BATCHNO FROM TEMPTABLEDB..TEMP" & systemId & "ORMAST)"
                    StrSql += vbCrLf + " )AS O"
                    StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CUSTOMERINFO AS CUS ON CUS.BATCHNO = O.BATCHNO"
                    StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..PERSONALINFO AS PER ON PER.SNO = CUS.PSNO"
                    StrSql += vbCrLf + " GROUP BY O.ORNO,PER.PNAME,O.COSTID,O.COMPANYID"
                    StrSql += vbCrLf + " HAVING SUM(CASE WHEN RECPAY = 'R' THEN O.AMOUNT ELSE -1*O.AMOUNT END) > 0"
                    StrSql += vbCrLf + " OR SUM(CASE WHEN RECPAY = 'R' THEN O.WEIGHT ELSE -1*O.WEIGHT END) > 0"
                    OrderRow = BrighttechPack.SearchDialog.Show_R("Select Pending Order Advance", StrSql, cn)
                    If Not OrderRow Is Nothing Then
                        txtOrderRepNo.Text = OrderRow.Item("ORNO").ToString
                        PendingAdjustment = True
                        Me.DialogResult = Windows.Forms.DialogResult.OK
                        Me.Close()
                    End If
                End If
            End If
        ElseIf e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub

    Private Sub BillOrderSearch_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If txtOrderRepNo.Text <> "" Then
            txtOrderRepNo_KeyPress(Me, New KeyPressEventArgs(Chr(Keys.Enter)))
        End If
        If ORDREP = "R" Then
            Label1.Text = "Repair No"
            Me.Text = "BillRepairSearch"
        Else
            Label1.Text = "Order No"
            Me.Text = "BillOrderSearch"
        End If
    End Sub

    Private Sub txtOrderRepNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOrderRepNo.GotFocus
        Main.ShowHelpText("Press Insert Key to Help")
        If REFNOPREFIXAUTO = "Y" And txtOrderRepNo.Text.Trim = "" Then txtOrderRepNo.Text = "O" + Mid(Format(cnTranToDate, "dd/MM/yyyy"), 9, 2).ToString : SendKeys.Send("{END}")
    End Sub

    Private Sub txtOrderRepNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtOrderRepNo.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If ORDREP <> Mid(txtOrderRepNo.Text, 1, 1) Then
                MsgBox("Invalid Type Selected", MsgBoxStyle.Information)
                Exit Sub
            End If
            StrSql = " SELECT "
            StrSql += vbCrLf + " NAME"
            StrSql += vbCrLf + " ,SUBSTRING(ORNO,6,30)AS ORNO,ITEMID"
            StrSql += vbCrLf + "  ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = X.ITEMID)AS ITEM"
            StrSql += vbCrLf + "  ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = X.ITEMID AND SUBITEMID = X.SUBITEMID)AS SUBITEM"
            StrSql += vbCrLf + "  ,TAGNO,PCS,GRSWT,NETWT,NARRATION,RATE,ORRATE,SNO,EMPID,DISCOUNT FROM "
            StrSql += vbCrLf + "  ("
            StrSql += vbCrLf + "  SELECT isnull(PI.PNAME,'') AS NAME,O.ORNO"
            StrSql += vbCrLf + "  ,CASE WHEN T.TAGNO <> '' THEN T.ITEMID ELSE O.ITEMID END ITEMID"
            StrSql += vbCrLf + "  ,CASE WHEN T.TAGNO <> '' THEN T.SUBITEMID ELSE O.SUBITEMID END SUBITEMID"
            StrSql += vbCrLf + "  ,T.TAGNO"
            StrSql += vbCrLf + "  ,CASE WHEN T.TAGNO <> '' THEN T.PCS ELSE O.PCS END PCS"
            StrSql += vbCrLf + "  ,CASE WHEN T.TAGNO <> '' THEN T.GRSWT ELSE O.GRSWT END GRSWT"
            StrSql += vbCrLf + "  ,CASE WHEN T.TAGNO <> '' THEN T.NETWT ELSE O.NETWT END NETWT,T.NARRATION"
            StrSql += vbCrLf + "  ,O.RATE"
            StrSql += vbCrLf + "  ,O.ORRATE,O.SNO,O.BATCHNO,O.EMPID,O.DISCOUNT"
            StrSql += vbCrLf + "  FROM " & cnAdminDb & "..ORMAST O "
            StrSql += vbCrLf + "  LEFT OUTER JOIN " & cnAdminDb & "..ITEMTAG AS T ON T.ORSNO = O.SNO AND T.ISSDATE IS NULL"
            StrSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..CUSTOMERINFO C ON C.BATCHNO = O.BATCHNO"
            StrSql += vbCrLf + "LEFT JOIN " & cnAdminDb & "..PERSONALINFO PI ON PI.SNO = C.PSNO"
            StrSql += vbCrLf + "  WHERE ISNULL(ODBATCHNO,'') = ''  AND ISNULL(O.CANCEL,'') = ''"
            StrSql += vbCrLf + "  AND ORTYPE = '" & Mid(txtOrderRepNo.Text, 1, 1) & "'"
            StrSql += vbCrLf + "  AND ISNULL(O.COSTID,'') = '" & BillCostId & "'"
            StrSql += vbCrLf + "  AND ISNULL(O.COMPANYID,'') = '" & strCompanyId & "'"
            StrSql += vbCrLf + "  AND ORNO = '" & GetCostId(BillCostId) & GetCompanyId(strCompanyId) & txtOrderRepNo.Text & "'"
            StrSql += vbCrLf + "  AND ISNULL(ORDCANCEL,'') = ''"
            'FOR SPLITTED TAG
            If SelectedOrder <> Nothing And SelectedTags = Nothing Then
                StrSql += vbCrLf + "  AND O.SNO NOT IN (" & SelectedOrder & ")"
            End If
            If SelectedTags <> Nothing And SelectedTags <> "" Then
                StrSql += vbCrLf + "  AND ISNULL(T.TAGNO,'') NOT IN (" & SelectedTags & ")"
            End If
            '
            StrSql += vbCrLf + "  )X"
            Dim msgTit As String
            If ORDREP = "R" Then msgTit = "Select RepairItem" Else msgTit = "Select OrderItem"
            OrderRow = BrighttechPack.SearchDialog.Show_R(msgTit, StrSql, cn, , , , , , False)

            If Not OrderRow Is Nothing Then
                StrSql = vbCrLf + " SELECT TRANFLAG FROM " & cnAdminDb & "..OUTSTANDING AS O"
                StrSql += vbCrLf + " WHERE ISNULL(O.CANCEL,'') = ''  "
                StrSql += vbCrLf + " AND ISNULL(O.COSTID,'') = '" & BillCostId & "'"
                StrSql += vbCrLf + " AND ISNULL(O.COMPANYID,'') = '" & strCompanyId & "'"
                StrSql += vbCrLf + " AND O.RUNNO = '" & GetCostId(BillCostId) & GetCompanyId(strCompanyId) & txtOrderRepNo.Text & "'"
                If objGPack.GetSqlValue(StrSql, , "").ToString = "L" Then
                    MsgBox("This Order advance has been blocked.", MsgBoxStyle.Information)
                    Exit Sub
                End If
                txtOrderRepNo.Text = OrderRow.Item("ORNO").ToString
                Me.DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()
            Else
                StrSql = vbCrLf + " SELECT TRANFLAG FROM " & cnAdminDb & "..OUTSTANDING AS O"
                StrSql += vbCrLf + " WHERE ISNULL(O.CANCEL,'') = ''  "
                StrSql += vbCrLf + " AND ISNULL(O.COSTID,'') = '" & BillCostId & "'"
                StrSql += vbCrLf + " AND ISNULL(O.COMPANYID,'') = '" & strCompanyId & "'"
                StrSql += vbCrLf + " AND O.RUNNO = '" & GetCostId(BillCostId) & GetCompanyId(strCompanyId) & txtOrderRepNo.Text & "'"
                If objGPack.GetSqlValue(StrSql, , "").ToString = "L" Then
                    MsgBox("This Order advance has been blocked.", MsgBoxStyle.Information)
                    Exit Sub
                End If
                If GetAdmindbSoftValue("ORDADV_ORDADJ", "N") = "Y" Then
                    MsgBox("Booked Order Items Not Found", MsgBoxStyle.Information)
                    Exit Sub
                End If
                StrSql = vbCrLf + " IF OBJECT_ID('TEMP" & systemId & "ORMAST','V') IS NOT NULL DROP VIEW TEMP" & systemId & "ORMAST"
                StrSql += vbCrLf + " IF OBJECT_ID('TEMP" & systemId & "ORMAST','U') IS NOT NULL DROP TABLE TEMP" & systemId & "ORMAST"
                Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
                Cmd.ExecuteNonQuery()
                StrSql = vbCrLf + " CREATE VIEW TEMP" & systemId & "ORMAST"
                StrSql += vbCrLf + " AS"
                StrSql += vbCrLf + "  SELECT O.ORNO"
                StrSql += vbCrLf + "  ,O.ITEMID"
                StrSql += vbCrLf + "  ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = O.ITEMID)AS ITEM"
                StrSql += vbCrLf + "  ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = O.SUBITEMID AND ITEMID = O.ITEMID)AS SUBITEM"
                StrSql += vbCrLf + "  ,(SELECT TOP 1 TAGNO FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORNO = O.ORNO AND ORSNO = O.SNO AND ISNULL(ORSTATUS,'') = 'R' AND ISNULL(CANCEL,'') = '')AS TAGNO"
                StrSql += vbCrLf + "  ,O.PCS,O.GRSWT,O.NETWT,O.RATE,O.ORRATE,O.SNO,O.EMPID,O.DISCOUNT"
                StrSql += vbCrLf + "  ,O.BATCHNO"
                StrSql += vbCrLf + "  FROM " & cnAdminDb & "..ORMAST O"
                StrSql += vbCrLf + " WHERE ISNULL(ODBATCHNO,'') = ''"
                StrSql += vbCrLf + " AND ORTYPE = '" & ORDREP & "' AND ISNULL(CANCEL,'') = ''"
                StrSql += vbCrLf + " AND ISNULL(COSTID,'') = '" & BillCostId & "'"
                StrSql += vbCrLf + " AND ISNULL(COMPANYID,'') = '" & strCompanyId & "'"
                StrSql += vbCrLf + " AND ORNO = '" & GetCostId(BillCostId) & GetCompanyId(strCompanyId) & txtOrderRepNo.Text & "'"
                StrSql += vbCrLf + " AND ISNULL(ORDCANCEL,'') = ''"
                Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
                Cmd.ExecuteNonQuery()

                ''Order Info Not Found
                If MessageBox.Show("Booked Order Items Not Found" + vbCrLf + "Do you want Adjust Pending Order Advance with Readymade Items?", "Order Pending Advance", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                    StrSql = vbCrLf + " SELECT SUBSTRING(O.ORNO,6,20)ORNO,PER.PNAME"
                    StrSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN O.AMOUNT END)[REC AMT]"
                    StrSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN O.WEIGHT END)[REC WT]"
                    StrSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'P' THEN O.AMOUNT END)[PAY AMT]"
                    StrSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'P' THEN O.WEIGHT END)[PAY WT]"
                    StrSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN O.AMOUNT ELSE -1*O.AMOUNT END)[BAL AMT]"
                    StrSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN O.WEIGHT ELSE -1*O.WEIGHT END)[BAL WT]"
                    StrSql += vbCrLf + " ,O.COSTID,O.COMPANYID,CONVERT(VARCHAR(15),'')SNO,CONVERT(VARCHAR(12),'')BATCHNO"
                    StrSql += vbCrLf + " FROM"
                    StrSql += vbCrLf + " ("
                    StrSql += vbCrLf + " SELECT O.RUNNO AS ORNO,AMOUNT,NETWT AS WEIGHT"
                    StrSql += vbCrLf + " ,O.COSTID,O.COMPANYID,O.RECPAY"
                    StrSql += vbCrLf + " ,(SELECT TOP 1 BATCHNO FROM " & cnAdminDb & "..OUTSTANDING WHERE COSTID = O.COSTID AND COMPANYID = O.COMPANYID AND RUNNO = O.RUNNO AND ISNULL(CANCEL,'') = '' ORDER BY TRANDATE) AS BATCHNO"
                    StrSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING AS O"
                    StrSql += vbCrLf + " WHERE COSTID = '" & BillCostId & "' AND COMPANYID = '" & strCompanyId & "' AND SUBSTRING(RUNNO,6,1) = 'O' AND ISNULL(CANCEL,'') = ''"
                    StrSql += vbCrLf + " AND BATCHNO NOT IN (SELECT BATCHNO FROM TEMP" & systemId & "ORMAST)"
                    StrSql += vbCrLf + " AND RUNNO = '" & GetCostId(BillCostId) & GetCompanyId(strCompanyId) & txtOrderRepNo.Text & "'"
                    StrSql += vbCrLf + " AND ISNULL(COSTID,'') = '" & BillCostId & "'"
                    StrSql += vbCrLf + " AND ISNULL(COMPANYID,'') = '" & strCompanyId & "'"
                    StrSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
                    StrSql += vbCrLf + " )AS O"
                    StrSql += vbCrLf + " left JOIN " & cnAdminDb & "..CUSTOMERINFO AS CUS ON CUS.BATCHNO = O.BATCHNO"
                    StrSql += vbCrLf + " left JOIN " & cnAdminDb & "..PERSONALINFO AS PER ON PER.SNO = CUS.PSNO"
                    StrSql += vbCrLf + " GROUP BY O.ORNO,PER.PNAME,O.COSTID,O.COMPANYID"
                    StrSql += vbCrLf + " HAVING SUM(CASE WHEN RECPAY = 'R' THEN O.AMOUNT ELSE -1*O.AMOUNT END) > 0"
                    StrSql += vbCrLf + " OR SUM(CASE WHEN RECPAY = 'R' THEN O.WEIGHT ELSE -1*O.WEIGHT END) > 0"
                    Dim dtOrd As New DataTable
                    da = New OleDbDataAdapter(StrSql, cn)
                    da.Fill(dtOrd)
                    If dtOrd.Rows.Count > 0 Then
                        OrderRow = dtOrd.Rows(0)
                    End If
                    If Not OrderRow Is Nothing Then
                        txtOrderRepNo.Text = OrderRow.Item("ORNO").ToString
                        PendingAdjustment = True
                        Me.DialogResult = Windows.Forms.DialogResult.OK
                        Me.Close()
                    Else
                        If SelectedOrder <> Nothing Then Me.DialogResult = Windows.Forms.DialogResult.OK : IsRmitems = True : Me.Close()
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub txtOrderRepNo_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOrderRepNo.LostFocus
        Main.HideHelpText()
    End Sub
End Class