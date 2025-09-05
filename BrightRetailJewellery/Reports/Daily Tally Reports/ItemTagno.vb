Imports System.Data.OleDb
Public Class ItemTagno
    Public PRODTAGSEP As Char
    Dim StrSql As String
    Dim Da As OleDbDataAdapter
    Dim TagCostId As String

    Public TagRow As DataRow = Nothing
    Public pItemId As Integer = 0

    Public Sub New(ByVal CostId As String)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        PRODTAGSEP = GetAdmindbSoftValue("PRODTAGSEP", "")
        If CostId <> "" Then
            Me.TagCostId = CostId
        Else
            Me.TagCostId = cnCostId
        End If
    End Sub
    Public Sub New(ByVal ItemId As Integer, ByVal CostId As String)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        PRODTAGSEP = GetAdmindbSoftValue("PRODTAGSEP", "")
        If CostId <> "" Then
            Me.TagCostId = CostId
        Else
            Me.TagCostId = cnCostId
        End If
        pItemId = ItemId
        txtItemId.Text = ItemId
        LoadSalesItemNameDetail()
    End Sub
    Private Sub txtItemId_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtItemId.GotFocus
        Main.ShowHelpText("Press Insert Key to Help")
        If pItemId <> 0 Then SendKeys.Send("{TAB}")
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
            StrSql += " GRSWT,NETWT,RATE AS RATE,"
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
            StrSql += " AND ISNULL(T.COSTID,'') = '" & TagCostId & "'"
            StrSql += " AND ISSDATE IS NULL"
            StrSql += " ORDER BY TAGNO"
            TagRow = BrighttechPack.SearchDialog.Show_R("Find TagNo", StrSql, cn, , , , , , , , False)
            If TagRow IsNot Nothing Then
                If TagRow.Item("STATUS").ToString <> "" Then
                    MsgBox("CANNOT SELECT " & TagRow.Item("STATUS").ToString & " TAG", MsgBoxStyle.Information)
                    Exit Sub
                End If
                txtTagNo.Text = TagRow.Item("TAGNO").ToString
                txtTagNo.SelectAll()
            End If
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
        StrSql += " AND ISNULL(COSTID,'') = '" & TagCostId & "'"
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
        StrSql += " AND ISNULL(COSTID,'') = '" & TagCostId & "'"
        Dim dtTag As New DataTable
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dtTag)
        If Not dtTag.Rows.Count > 0 Then
            MsgBox("Invalid Tag Info", MsgBoxStyle.Information)
            Exit Sub
        End If
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub
    Private Sub txtTagNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTagNo.GotFocus
        Main.ShowHelpText("Press Insert Key to Help")
    End Sub

    Private Sub txtTagNo_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTagNo.LostFocus
        Main.HideHelpText()
    End Sub
End Class