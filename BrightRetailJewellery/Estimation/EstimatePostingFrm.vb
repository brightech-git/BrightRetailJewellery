Imports System.Data.OleDb
Public Class EstimatePostingFrm
    Dim strSql As String
    Dim cmd As OleDbCommand

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub EstimatePostingFrm_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub

    Private Sub EstimatePostingFrm_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub EstimatePostingFrm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If dtpAsOnDate.MinimumDate <= GetEntryDate(GetServerDate).AddDays(-1) Then
            dtpAsOnDate.Value = GetEntryDate(GetServerDate).AddDays(-1)
        End If
        dtpAsOnDate.Select()
    End Sub

    Private Sub btnPost_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPost.Click

        If GetAdmindbSoftValue("ESTPOST", "Y") = "N" Then
            MsgBox("Estimate post option disabled", MsgBoxStyle.Information)
            Exit Sub
        End If
        Dim RESETESTNO As String = GetAdmindbSoftValue("ESTNORESET", "N")
        Dim Multicostid As String = GetAdmindbSoftValue("COSTCENTRE_SINGLE", "N")
        'If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
        Try
            tran = Nothing
            tran = cn.BeginTransaction
            strSql = " SELECT 'EXIST' FROM "
            strSql += " ("
            strSql += " SELECT 'EXIST' CHECKS FROM " & cnStockDb & "..ESTISSUE WHERE TRANDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
            If strBCostid <> Nothing And Multicostid <> "Y" Then strSql += " and ISNULL(COSTID,'') = '" & strBCostid & "'"
            strSql += " UNION"
            strSql += " SELECT 'EXIST' FROM " & cnStockDb & "..ESTRECEIPT WHERE TRANDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
            If strBCostid <> Nothing And Multicostid <> "Y" Then strSql += " and ISNULL(COSTID,'') = '" & strBCostid & "'"
            strSql += " UNION"
            strSql += " SELECT 'EXIST' FROM " & cnStockDb & "..ESTORMAST WHERE ORDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
            If strBCostid <> Nothing And Multicostid <> "Y" Then strSql += " and ISNULL(COSTID,'') = '" & strBCostid & "'"
            strSql += " UNION"
            strSql += " SELECT 'EXIST' FROM " & cnStockDb & "..ESTACCTRAN WHERE TRANDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
            If strBCostid <> Nothing And Multicostid <> "Y" Then strSql += " and ISNULL(COSTID,'') = '" & strBCostid & "'"
            strSql += " UNION"
            strSql += " SELECT 'EXIST' FROM " & cnStockDb & "..ESTOUTSTANDING WHERE TRANDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
            If strBCostid <> Nothing And Multicostid <> "Y" Then strSql += " and ISNULL(COSTID,'') = '" & strBCostid & "'"
            strSql += " )x"
            If objGPack.GetSqlValue(strSql, , , tran) <> "" Then
                strSql = " UPDATE " & cnStockDb & "..BILLCONTROL"
                strSql += " SET CTLTEXT = '' WHERE CTLID = 'ESTSALESNO'"
                If strBCostid <> Nothing And Multicostid <> "Y" Then strSql += " and ISNULL(COSTID,'') = '" & strBCostid & "'"
                strSql += " UPDATE " & cnStockDb & "..BILLCONTROL"
                strSql += " SET CTLTEXT = '' WHERE CTLID = 'ESTPURNO'"
                If strBCostid <> Nothing And Multicostid <> "Y" Then strSql += " and ISNULL(COSTID,'') = '" & strBCostid & "'"
                strSql += " UPDATE " & cnStockDb & "..BILLCONTROL"
                strSql += " SET CTLTEXT = '' WHERE CTLID = 'ESTSPNO'"
                If strBCostid <> Nothing And Multicostid <> "Y" Then strSql += " and ISNULL(COSTID,'') = '" & strBCostid & "'"
                strSql += " UPDATE " & cnStockDb & "..BILLCONTROL"
                strSql += " SET CTLTEXT = '' WHERE CTLID = 'ESTGENNO'"
                If strBCostid <> Nothing And Multicostid <> "Y" Then strSql += " and ISNULL(COSTID,'') = '" & strBCostid & "'"
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.ExecuteNonQuery()
                'strSql = "SELECT CTLMODE FROM " & cnStockDb & "..BILLCONTROL"
                'strSql += " WHERE CTLID = 'ESTSPNO'"
                'strSql += " AND COMPANYID = '" & _MainCompId & "'"
                'If UCase(objGPack.GetSqlValue(strSql, , , tran)) = "Y" Then
                '    strSql = " UPDATE " & cnStockDb & "..BILLCONTROL"
                '    strSql += " SET CTLTEXT = '' WHERE CTLID = 'ESTPNO'"
                '    cmd = New OleDbCommand(strSql, cn, tran)
                '    cmd.ExecuteNonQuery()
                'Else
                '    strSql = " UPDATE " & cnStockDb & "..BILLCONTROL"
                '    strSql += " SET CTLTEXT = '' WHERE CTLID = 'ESTSALESNO'"
                '    strSql += " UPDATE " & cnStockDb & "..BILLCONTROL"
                '    strSql += " SET CTLTEXT = '' WHERE CTLID = 'ESTPURNO'"
                '    strSql += " UPDATE " & cnStockDb & "..BILLCONTROL"
                '    strSql += " SET CTLTEXT = '' WHERE CTLID = 'ESTPNO'"
                '    cmd = New OleDbCommand(strSql, cn, tran)
                '    cmd.ExecuteNonQuery()
                'End If
            End If
            strSql = " DELETE FROM " & cnStockDb & "..ESTISSMETAL WHERE TRANDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
            If strBCostid <> Nothing And Multicostid <> "Y" Then strSql += " and ISNULL(COSTID,'') = '" & strBCostid & "'"
            strSql += " DELETE FROM " & cnStockDb & "..ESTISSMISC WHERE TRANDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
            If strBCostid <> Nothing And Multicostid <> "Y" Then strSql += " and ISNULL(COSTID,'') = '" & strBCostid & "'"
            strSql += " DELETE FROM " & cnStockDb & "..ESTISSSTONE WHERE TRANDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
            If strBCostid <> Nothing And Multicostid <> "Y" Then strSql += " and ISNULL(COSTID,'') = '" & strBCostid & "'"
            strSql += " DELETE FROM " & cnStockDb & "..ESTISSUE WHERE TRANDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
            If strBCostid <> Nothing And Multicostid <> "Y" Then strSql += " and ISNULL(COSTID,'') = '" & strBCostid & "'"
            strSql += " DELETE FROM " & cnStockDb & "..ESTRECEIPT WHERE TRANDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
            If strBCostid <> Nothing And Multicostid <> "Y" Then strSql += " and ISNULL(COSTID,'') = '" & strBCostid & "'"
            strSql += " DELETE FROM " & cnStockDb & "..ESTRECEIPTMETAL WHERE TRANDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
            If strBCostid <> Nothing And Multicostid <> "Y" Then strSql += " and ISNULL(COSTID,'') = '" & strBCostid & "'"
            strSql += " DELETE FROM " & cnStockDb & "..ESTRECEIPTMISC WHERE TRANDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
            If strBCostid <> Nothing And Multicostid <> "Y" Then strSql += " and ISNULL(COSTID,'') = '" & strBCostid & "'"
            strSql += " DELETE FROM " & cnStockDb & "..ESTRECEIPTSTONE WHERE TRANDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
            If strBCostid <> Nothing And Multicostid <> "Y" Then strSql += " and ISNULL(COSTID,'') = '" & strBCostid & "'"
            strSql += " DELETE FROM " & cnStockDb & "..ESTTAXTRAN WHERE TRANDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
            If strBCostid <> Nothing And Multicostid <> "Y" Then strSql += " and ISNULL(COSTID,'') = '" & strBCostid & "'"
            strSql += " DELETE FROM " & cnStockDb & "..ESTACCTRAN WHERE TRANDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
            If strBCostid <> Nothing And Multicostid <> "Y" Then strSql += " and ISNULL(COSTID,'') = '" & strBCostid & "'"
            strSql += " DELETE FROM " & cnStockDb & "..ESTOUTSTANDING WHERE TRANDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
            If strBCostid <> Nothing And Multicostid <> "Y" Then strSql += " and ISNULL(COSTID,'') = '" & strBCostid & "'"
            strSql += " DELETE FROM " & cnStockDb & "..ESTORMAST WHERE ORDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
            If strBCostid <> Nothing And Multicostid <> "Y" Then strSql += " and ISNULL(COSTID,'') = '" & strBCostid & "'"
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()
            strSql = " SELECT max(tranno) FROM " & cnStockDb & "..ESTISSUE WHERE TRANDATE > '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
            If strBCostid <> Nothing And Multicostid <> "Y" Then strSql += " and ISNULL(COSTID,'') = '" & strBCostid & "'"
            Dim mestno As Integer = Val(objGPack.GetSqlValue(strSql, , , tran))
            If RESETESTNO = "Y" Then mestno = 0
            If mestno > 0 Then
                strSql = " UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT = " & mestno + 1 & " WHERE CTLID = 'ESTSALESNO'"
                If strBCostid <> Nothing And Multicostid <> "Y" Then strSql += " and ISNULL(COSTID,'') = '" & strBCostid & "'"
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.ExecuteNonQuery()
            End If

            strSql = " SELECT max(tranno) FROM " & cnStockDb & "..ESTRECEIPT WHERE TRANDATE > '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
            If strBCostid <> Nothing And Multicostid <> "Y" Then strSql += " and ISNULL(COSTID,'') = '" & strBCostid & "'"
            Dim mpestno As Integer = Val((objGPack.GetSqlValue(strSql, , , tran)))
            If RESETESTNO = "Y" Then mpestno = 0
            If mpestno > 0 Then
                strSql = " UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT = " & mpestno + 1 & " WHERE CTLID = 'ESTPURNO'"
                If strBCostid <> Nothing And Multicostid <> "Y" Then strSql += " and ISNULL(COSTID,'') = '" & strBCostid & "'"
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.ExecuteNonQuery()
            End If

            strSql = " SELECT max(orno) FROM " & cnStockDb & "..ESTORMAST WHERE ORDATE > '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
            If strBCostid <> Nothing And Multicostid <> "Y" Then strSql += " and ISNULL(COSTID,'') = '" & strBCostid & "'"
            Dim mgestno As Integer = Val(objGPack.GetSqlValue(strSql, , , tran))
            If RESETESTNO = "Y" Then mgestno = 0
            If mgestno > 0 Then
                strSql = " UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT = " & mgestno + 1 & " WHERE CTLID = 'ESTGENNO'"
                If strBCostid <> Nothing And Multicostid <> "Y" Then strSql += " and ISNULL(COSTID,'') = '" & strBCostid & "'"
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.ExecuteNonQuery()
            End If

            tran.Commit()
            MsgBox("Estimation Posted Successfully", MsgBoxStyle.Information)
            Me.Close()
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub
End Class