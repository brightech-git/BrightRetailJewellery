Imports System.Data.OleDb
Public Class TagSearch
    Dim StrSql As String
    Public OrderRow As DataRow = Nothing
    Public SelectedOrder As String = Nothing
    Public REFNOPREFIXAUTO As String = ""
    Public PendingAdjustment As Boolean = False
    Public BillCostId As String
    Public ORDREP As String
    Public OrdItemid As Integer
    Public dtOrdtag As New DataTable
    Dim Cmd As OleDbCommand
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
        objGPack.TextClear(Me)

    End Sub

    Private Sub BillOrderSearch_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Insert Then
            If OrdItemid = 0 Then Exit Sub
            Dim stockType As String = objGPack.GetSqlValue("SELECT STOCKTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = '" & OrdItemid & "'")
            If stockType = "T" Then
                StrSql = " SELECT"
                StrSql += vbCrLf + " TAGNO AS TAGNO,STYLENO,ITEMID AS ITEMID,"
                StrSql += vbCrLf + " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS ITEMNAME,"
                StrSql += vbCrLf + " T.PCS AS PCS,"
                StrSql += vbCrLf + " GRSWT AS GRS_WT,NETWT AS NET_WT,RATE AS RATE,"
                StrSql += vbCrLf + " SALVALUE AS SALVALUE,"
                StrSql += vbCrLf + " ISNULL((SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID  = T.SUBITEMID),'')AS SUBITEM,"
                StrSql += vbCrLf + " CONVERT(VARCHAR,RECDATE,103) AS RECDATE,"
                StrSql += vbCrLf + " CASE WHEN APPROVAL = 'A' THEN 'APPROVAL' WHEN APPROVAL = 'R' THEN 'RESERVED' ELSE NULL END AS STATUS,"
                StrSql += vbCrLf + " CASE WHEN APPROVAL = 'A' THEN '" & Color.MistyRose.Name & "' WHEN APPROVAL = 'R' THEN '" & Color.PaleTurquoise.Name & "' ELSE NULL END AS COLOR_HIDE,"
                StrSql += vbCrLf + " (SELECT SIZENAME FROM " & cnAdminDb & "..ITEMSIZE WHERE SIZEID = T.SIZEID)AS SIZE,"
                StrSql += vbCrLf + " ISNULL(IC.ITEMCTRNAME,'') COUNTER "
                StrSql += vbCrLf + " FROM"
                StrSql += vbCrLf + " " & cnAdminDb & "..ITEMTAG AS T "
                StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMCOUNTER AS IC ON T.ITEMCTRID = IC.ITEMCTRID"
                StrSql += vbCrLf + " WHERE T.ITEMID = " & OrdItemid & ""
                StrSql += GridTagNoFiltStr(dtOrdtag)

                'StrSql += ShowTagFiltration()
                StrSql += vbCrLf + " AND COSTID = '" & BillCostId & "'"
                ' If COMPANYTAG Then StrSql += " AND COMPANYID = '" & GetStockCompId() & "'"
                StrSql += vbCrLf + " AND ISSDATE IS NULL"
                StrSql += " ORDER BY TAGNO"
                txtTagNo.Text = BrighttechPack.SearchDialog.Show("Find TagNo", StrSql, cn, , , , , , , , False)
                txtTagNo.SelectAll()
            ElseIf stockType = "P" Then
                'If saPacketRecBaseIss Then
                StrSql = vbCrLf + " IF OBJECT_ID('TEMP_PKTISS') IS NOT NULL DROP VIEW TEMP_PKTISS"
                Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
                StrSql = vbCrLf + " CREATE VIEW TEMP_PKTISS"
                StrSql += vbCrLf + " AS"
                StrSql += vbCrLf + " SELECT NT.PACKETNO,NT.ITEMID,IM.ITEMNAME"
                StrSql += vbCrLf + " ,NT.PCS,NT.GRSWT,NT.NETWT,SM.SUBITEMNAME"
                StrSql += vbCrLf + " ,NT.RECISS,NT.NARRATION"
                StrSql += vbCrLf + " ,CASE WHEN NT.RECISS = 'R' THEN SNO ELSE NT.RECSNO END AS SNO"
                StrSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMNONTAG AS NT"
                StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = NT.ITEMID"
                StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = NT.ITEMID AND SM.SUBITEMID = NT.SUBITEMID"
                StrSql += vbCrLf + " WHERE ISNULL(NT.PACKETNO,'') <> '' AND ISNULL(CANCEL,'') = ''"
                StrSql += vbCrLf + " AND NT.ITEMID = " & OrdItemid & ""
                If Not cnCentStock Then StrSql += " AND COMPANYID = '" & GetStockCompId() & "'"
                StrSql += vbCrLf + " AND NT.COSTID = '" & BillCostId & "'"
                Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
                StrSql = vbCrLf + " SELECT PACKETNO,ITEMID,ITEMNAME,SUBITEMNAME"
                StrSql += vbCrLf + " ,SUM(CASE WHEN RECISS = 'R' THEN PCS ELSE -1*PCS END)AS PCS"
                StrSql += vbCrLf + " ,SUM(CASE WHEN RECISS = 'R' THEN GRSWT ELSE -1*GRSWT END)AS GRSWT"
                StrSql += vbCrLf + " ,SUM(CASE WHEN RECISS = 'R' THEN NETWT ELSE -1*NETWT END)AS NETWT"
                StrSql += vbCrLf + " ,(SELECT TOP 1 NARRATION FROM TEMP_PKTISS WHERE PACKETNO = X.PACKETNO AND ITEMID = X.ITEMID AND RECISS = 'R')NARRATION,SNO"
                StrSql += vbCrLf + " FROM TEMP_PKTISS AS X"
                StrSql += vbCrLf + " GROUP BY PACKETNO,ITEMID,ITEMNAME,SUBITEMNAME,SNO"
                StrSql += vbCrLf + " HAVING SUM(CASE WHEN RECISS = 'R' THEN PCS ELSE -1*PCS END) > 0"
                StrSql += vbCrLf + " OR (SUM(CASE WHEN RECISS = 'R' THEN GRSWT ELSE -1*GRSWT END)> 0"
                StrSql += vbCrLf + " AND SUM(CASE WHEN RECISS = 'R' THEN NETWT ELSE -1*NETWT END)> 0)"
                Dim Rw As DataRow = Nothing : Rw = BrighttechPack.SearchDialog.Show_R("Find PacketNo", StrSql, cn)
                If Rw IsNot Nothing Then
                    txtTagNo.Text = Rw.Item("PACKETNO").ToString
                    'txtSARate_AMT.Text = GetSARate()
                    'txtDetSubItem.Text = Rw.Item("SUBITEMNAME").ToString
                    Dim saPacketNoSno As String = Rw.Item("SNO").ToString
                    Dim saPKTPCS As Integer = Val(Rw.Item("PCS").ToString)
                    Dim dtTemp As New DataTable
                    StrSql = " SELECT CO.ITEMCTRNAME AS COUNTER,IT.NAME AS ITEMTYPE,DESIGNERID"
                    StrSql += " FROM " & cnAdminDb & "..ITEMNONTAG AS NT "
                    StrSql += " LEFT JOIN " & cnAdminDb & "..ITEMCOUNTER AS CO ON CO.ITEMCTRID = NT.ITEMCTRID"
                    StrSql += " LEFT JOIN " & cnAdminDb & "..ITEMTYPE AS IT ON IT.ITEMTYPEID = NT.ITEMTYPEID"
                    StrSql += " WHERE NT.SNO = '" & saPacketNoSno & "'"
                    da = New OleDbDataAdapter(StrSql, cn)
                    da.Fill(dtTemp)
                    If dtTemp.Rows.Count > 0 Then
                        'txtDetCounter.Text = dtTemp.Rows(0).Item("COUNTER").ToString
                        'txtDetItemType.Text = dtTemp.Rows(0).Item("ITEMTYPE").ToString
                        'saDesignerId = Val(dtTemp.Rows(0).Item("DESIGNERID").ToString)
                    End If
                    'If Not ValidateItemCounter(GetCounterId) Then
                    '    txtSATagNo.Clear()
                    '    txtSATagNo.Select()
                    '    Exit Sub
                    'End If
                    'Me.SelectNextControl(txtSATagNo, True, True, True, True)
                End If

            End If

            
        ElseIf e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub

    Private Sub BillOrderSearch_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If txtTagNo.Text <> "" Then

        End If
    End Sub
    Private Function GridTagNoFiltStr(ByVal dt As DataTable) As String
        Dim retStr As String = Nothing
        Dim itemId As String = Nothing
        Dim tagNo As String = Nothing
        For cnt As Integer = 0 To dt.Rows.Count - 1
            If Val(dt.Rows(cnt).Item("ITEMID").ToString) = 0 Then Continue For
            itemId += Val(dt.Rows(cnt).Item("ITEMID").ToString).ToString + ","
            tagNo += "'" & dt.Rows(cnt).Item("TAGNO").ToString & "'" + ","
        Next
        If itemId <> Nothing Then
            If itemId.EndsWith(",") Then itemId = itemId.Remove(itemId.Length - 1, 1)
            If tagNo.EndsWith(",") Then tagNo = tagNo.Remove(tagNo.Length - 1, 1)
            retStr += " AND (ITEMID NOT IN (" & itemId & ") OR TAGNO NOT IN (" & tagNo & "))"
        End If
        Return retStr
    End Function

    'Private Sub txtOrderRepNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTagNo.GotFocus
    '    Main.ShowHelpText("Press Insert Key to Help")
    '    If REFNOPREFIXAUTO = "Y" And txtTagNo.Text.Trim = "" Then txtTagNo.Text = "O" + Mid(Format(cnTranToDate, "dd/MM/yyyy"), 9, 2).ToString : SendKeys.Send("{END}")
    'End Sub

    Private Sub txtTagNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtTagNo.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        ElseIf e.KeyChar = Chr(Keys.Escape) Then
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
            Me.Close()
        End If
    End Sub


    Private Sub txtOrderRepNo_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTagNo.LostFocus
        Main.HideHelpText()
    End Sub

    Private Sub Label1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label1.Click

    End Sub
End Class