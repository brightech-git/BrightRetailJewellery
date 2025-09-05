Imports System.Data.OleDb
Public Class frmAdvanceLock
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim BillCostId As String
    Dim dtGridView As DataTable
    Public withOrderDetail As Boolean = False
    Public withRepairDetail As Boolean = False
    Public dtGridAdvance As New DataTable
    Dim dtitem As New DataTable
    Public BillDate As Date = GetServerDate(tran)
    Dim Syncdb As String = cnAdminDb
    Dim runno As Boolean = False

    Private Sub txtRunno_MAN_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRunno_MAN.GotFocus
        Main.ShowHelpText("Press Insert Key to Help")
    End Sub

    Private Sub frmAdvanceLock_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmAdvanceLock_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim uprefix As String = Mid(cnAdminDb, 1, 3)
        If GetAdmindbSoftValue("SYNC-SEPDATADB", "N", tran) = "Y" Then
            Dim usuffix As String = "UTILDB"
            If DbChecker(uprefix + usuffix) <> 0 Then Syncdb = uprefix + usuffix
        End If
        If _IsCostCentre Then
            cmbCostCentre.Enabled = True
            cmbCostCentre.Text = cnCostName
            If strUserCentrailsed <> "Y" Then cmbCostCentre.Enabled = False
        Else
            cmbCostCentre.Text = "ALL"
            cmbCostCentre.Enabled = False
        End If
        If cmbCostCentre.Enabled = True Then
            strSql = " select CostName from " & cnAdminDb & "..CostCentre WHERE  ISNULL(COMPANYID,'" & strCompanyId & "')LIKE'%" & strCompanyId & "%' order by CostName"
            cmbCostCentre.Items.Clear()
            cmbCostCentre.Items.Add("ALL")
            objGPack.FillCombo(strSql, cmbCostCentre, False)
            cmbCostCentre.Text = "ALL"
        End If
        funcNew()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        funcNew()
    End Sub
    Function funcNew()
        'strSql = "SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID NOT IN('" & cnCostId & "')"
        'objGPack.FillCombo(strSql, cmbcostCentre, True, False)
        txtRunno_MAN.Text = ""
        GridView.DataSource = ""
        lblLock.Visible = False
        Label3.Visible = False
        'cmbcostCentre.Text = ""
        'cmbcostCentre.Focus()
        txtRunno_MAN.Focus()
    End Function

    Private Sub txtRunno_MAN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtRunno_MAN.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            Dim RNO As String
            RNO = GetCostId(cnCostId) + GetCompanyId(strCompanyId) + txtRunno_MAN.Text
            strSql = " SELECT 1 FROM " & cnAdminDb & "..OUTSTANDING WHERE ISNULL(CANCEL,'')= '' AND RUNNO ='" & RNO & "' HAVING SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE -1*AMOUNT END) <> 0"
            If objGPack.GetSqlValue(strSql, , "0") = "0" Then
                MsgBox("This Advanceno Already Tally", MsgBoxStyle.Information)
                txtRunno_MAN.Focus()
            End If
        End If
    End Sub
    Private Sub txtRunno_MAN_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtRunno_MAN.KeyDown
        Dim SelCostid As String = "ALL"
        If cmbCostCentre.Text <> "ALL" And cmbCostCentre.Text <> "" Then
            strSql = "SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME='" & cmbCostCentre.Text & "'"
            SelCostid = objGPack.GetSqlValue(strSql, "COSTID", "").ToString
        End If

        If e.KeyCode = Keys.Insert And rbtn_advance.Checked = True Then
            strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "_CUSTOMERINFO') > 0"
            strSql += vbCrLf + " DROP VIEW TEMP" & systemId & "_CUSTOMERINFO"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = " CREATE VIEW TEMP" & systemId & "_CUSTOMERINFO"
            strSql += vbCrLf + " AS"
            strSql += vbCrLf + " SELECT C.PSNO as SNO,C.BATCHNO,TRANDATE,PREVILEGEID,ACCODE,TITLE,INITIAL,PNAME,DOORNO,ADDRESS1,ADDRESS2,ADDRESS3,AREA,CITY,STATE,COUNTRY,PINCODE,PHONERES,MOBILE,EMAIL,FAX"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..PERSONALINFO P INNER JOIN " & cnAdminDb & "..CUSTOMERINFO C ON P.SNO = C.PSNO"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = " SELECT SUBSTRING(RUNNO,6,20) REFNO"
            strSql += vbCrLf + " ,CASE WHEN RECEIPT <> 0 THEN RECEIPT ELSE NULL END AS RECEIPT"
            strSql += vbCrLf + " ,CASE WHEN PAYMENT <> 0 THEN PAYMENT ELSE NULL END AS PAYMENT"
            strSql += vbCrLf + " ,CASE WHEN BALANCE <> 0 THEN BALANCE ELSE NULL END AS BALANCE"
            strSql += vbCrLf + " ,x.TRANDATE,V.PREVILEGEID,V.ACCODE,V.TITLE,V.INITIAL,V.PNAME,V.DOORNO,V.ADDRESS1,V.ADDRESS2,V.ADDRESS3"
            strSql += vbCrLf + " ,V.AREA,V.CITY,V.STATE,V.COUNTRY,V.PINCODE,V.PHONERES,V.MOBILE,V.EMAIL,V.FAX,V.SNO,X.COMPANYID"
            strSql += vbCrLf + " FROM"
            strSql += vbCrLf + " ("
            strSql += vbCrLf + " SELECT RUNNO"
            strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE 0 END)AS RECEIPT"
            strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE 0 END)AS PAYMENT"
            strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE -1*AMOUNT END)AS BALANCE"
            If SelCostid <> "ALL" Then
                strSql += vbCrLf + " ,(SELECT TOP 1 TRANDATE FROM " & cnAdminDb & "..OUTSTANDING WHERE COSTID = '" & SelCostid & "' AND RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  AND ISNULL(CANCEL,'') = '' ORDER BY TRANDATE DESC)AS TRANDATE"
                strSql += vbCrLf + " ,(SELECT TOP 1 BATCHNO FROM " & cnAdminDb & "..OUTSTANDING WHERE COSTID = '" & SelCostid & "' AND RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  AND ISNULL(CANCEL,'') = '' ORDER BY TRANDATE DESC)AS BATCHNO"
            Else
                strSql += vbCrLf + " ,(SELECT TOP 1 TRANDATE FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  AND ISNULL(CANCEL,'') = '' ORDER BY TRANDATE DESC)AS TRANDATE"
                strSql += vbCrLf + " ,(SELECT TOP 1 BATCHNO FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  AND ISNULL(CANCEL,'') = '' ORDER BY TRANDATE DESC)AS BATCHNO"
            End If
            strSql += vbCrLf + " ,O.COMPANYID,O.TRANTYPE "
            strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING O"
            strSql += vbCrLf + " WHERE ISNULL(CANCEL,'') = '' AND SUBSTRING(RUNNO,6,1)='A' AND TRANTYPE='A'"
            strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
            If SelCostid <> "ALL" Then
                strSql += vbCrLf + " AND COSTID = '" & SelCostid & "'"
            End If
            strSql += vbCrLf + " GROUP BY RUNNO,COMPANYID,TRANTYPE"
            strSql += vbCrLf + " HAVING SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE -1*AMOUNT END) > 0 "
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT RUNNO"
            strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE 0 END)AS RECEIPT"
            strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE 0 END)AS PAYMENT"
            strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE -1*AMOUNT END)AS BALANCE"
            If SelCostid <> "ALL" Then
                strSql += vbCrLf + " ,(SELECT TOP 1 TRANDATE FROM " & cnAdminDb & "..OUTSTANDING WHERE COSTID = '" & SelCostid & "' AND RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  AND ISNULL(CANCEL,'') = '' ORDER BY TRANDATE DESC)AS TRANDATE"
                strSql += vbCrLf + " ,(SELECT TOP 1 BATCHNO FROM " & cnAdminDb & "..OUTSTANDING WHERE COSTID = '" & SelCostid & "' AND RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  AND ISNULL(CANCEL,'') = '' ORDER BY TRANDATE DESC)AS BATCHNO"
            Else
                strSql += vbCrLf + " ,(SELECT TOP 1 TRANDATE FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  AND ISNULL(CANCEL,'') = '' ORDER BY TRANDATE DESC)AS TRANDATE"
                strSql += vbCrLf + " ,(SELECT TOP 1 BATCHNO FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  AND ISNULL(CANCEL,'') = '' ORDER BY TRANDATE DESC)AS BATCHNO"
            End If
            strSql += vbCrLf + " ,O.COMPANYID,O.TRANTYPE "
            strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING O"
            strSql += vbCrLf + " WHERE ISNULL(CANCEL,'') = ''  AND SUBSTRING(RUNNO,6,1)='A' AND TRANTYPE='A'"
            strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
            If SelCostid <> "ALL" Then
                strSql += vbCrLf + " AND COSTID = '" & SelCostid & "'"
            End If
            strSql += vbCrLf + " GROUP BY RUNNO,COMPANYID,TRANTYPE"
            strSql += vbCrLf + " HAVING SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE -1*AMOUNT END) > 0 "
            strSql += vbCrLf + " )X"
            strSql += vbCrLf + " INNER JOIN TEMP" & systemId & "_CUSTOMERINFO AS V ON V.BATCHNO = X.BATCHNO"
            strSql += vbCrLf + " ORDER BY X.TRANTYPE"
            Dim row As DataRow = Nothing
            row = BrighttechPack.SearchDialog.Show_R("Search Reference No", strSql, cn, 9, , , , , , , False)
            If Not row Is Nothing Then
                With row
                    For Each roAdv As DataRow In dtGridAdvance.Rows
                        If .Item("REFNO").ToString = roAdv!RUNNO.ToString Then
                            MsgBox("Already Exist in this Ref No", MsgBoxStyle.Information)
                            txtRunno_MAN.Select()
                            txtRunno_MAN.SelectAll()
                            Exit Sub
                        End If
                    Next
                    txtRunno_MAN.Text = .Item("REFNO").ToString
                End With
            End If
        End If

        If e.KeyCode = Keys.Insert And rbtnorder.Checked = True Then
            strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "_CUSTOMERINFO') > 0"
            strSql += vbCrLf + " DROP VIEW TEMP" & systemId & "_CUSTOMERINFO"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = " CREATE VIEW TEMP" & systemId & "_CUSTOMERINFO"
            strSql += vbCrLf + " AS"
            strSql += vbCrLf + " SELECT C.PSNO as SNO,C.BATCHNO,TRANDATE,PREVILEGEID,ACCODE,TITLE,INITIAL,PNAME,DOORNO,ADDRESS1,ADDRESS2,ADDRESS3,AREA,CITY,STATE,COUNTRY,PINCODE,PHONERES,MOBILE,EMAIL,FAX"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..PERSONALINFO P INNER JOIN " & cnAdminDb & "..CUSTOMERINFO C ON P.SNO = C.PSNO"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()


            strSql = " SELECT SUBSTRING(RUNNO,6,20) REFNO"
            strSql += vbCrLf + " ,CASE WHEN RECEIPT <> 0 THEN RECEIPT ELSE NULL END AS RECEIPT"
            strSql += vbCrLf + " ,CASE WHEN PAYMENT <> 0 THEN PAYMENT ELSE NULL END AS PAYMENT"
            strSql += vbCrLf + " ,CASE WHEN BALANCE <> 0 THEN BALANCE ELSE NULL END AS BALANCE"
            strSql += vbCrLf + " ,x.TRANDATE,V.PREVILEGEID,V.ACCODE,V.TITLE,V.INITIAL,V.PNAME,V.DOORNO,V.ADDRESS1,V.ADDRESS2,V.ADDRESS3"
            strSql += vbCrLf + " ,V.AREA,V.CITY,V.STATE,V.COUNTRY,V.PINCODE,V.PHONERES,V.MOBILE,V.EMAIL,V.FAX,V.SNO,X.COMPANYID"
            strSql += vbCrLf + " FROM"
            strSql += vbCrLf + " ("
            strSql += vbCrLf + " SELECT RUNNO"
            strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE 0 END)AS RECEIPT"
            strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE 0 END)AS PAYMENT"
            strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE -1*AMOUNT END)AS BALANCE"
            If SelCostid <> "ALL" Then
                strSql += vbCrLf + " ,(SELECT TOP 1 TRANDATE FROM " & cnAdminDb & "..OUTSTANDING WHERE COSTID = '" & SelCostid & "' AND RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  AND ISNULL(CANCEL,'') = '' ORDER BY TRANDATE DESC)AS TRANDATE"
                strSql += vbCrLf + " ,(SELECT TOP 1 BATCHNO FROM " & cnAdminDb & "..OUTSTANDING WHERE COSTID = '" & SelCostid & "' AND RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  AND ISNULL(CANCEL,'') = '' ORDER BY TRANDATE DESC)AS BATCHNO"
            Else
                strSql += vbCrLf + " ,(SELECT TOP 1 TRANDATE FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  AND ISNULL(CANCEL,'') = '' ORDER BY TRANDATE DESC)AS TRANDATE"
                strSql += vbCrLf + " ,(SELECT TOP 1 BATCHNO FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  AND ISNULL(CANCEL,'') = '' ORDER BY TRANDATE DESC)AS BATCHNO"
            End If
            strSql += vbCrLf + " ,O.COMPANYID,O.TRANTYPE "
            strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING O"
            strSql += vbCrLf + " WHERE ISNULL(CANCEL,'') = '' AND SUBSTRING(RUNNO,6,1)='O' AND PAYMODE in ('OR','OP')  "
            strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
            If SelCostid <> "ALL" Then
                strSql += vbCrLf + " AND COSTID = '" & SelCostid & "'"
            End If
            strSql += vbCrLf + " AND RUNNO LIKE COSTID+COMPANYID+'O%'"
            strSql += vbCrLf + " GROUP BY RUNNO,COMPANYID,TRANTYPE"
            strSql += vbCrLf + " HAVING SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE -1*AMOUNT END) > 0 "
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT RUNNO"
            strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE 0 END)AS RECEIPT"
            strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE 0 END)AS PAYMENT"
            strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE -1*AMOUNT END)AS BALANCE"
            If SelCostid <> "ALL" Then
                strSql += vbCrLf + " ,(SELECT TOP 1 TRANDATE FROM " & cnAdminDb & "..OUTSTANDING WHERE COSTID = '" & SelCostid & "' AND RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  AND ISNULL(CANCEL,'') = '' ORDER BY TRANDATE DESC)AS TRANDATE"
                strSql += vbCrLf + " ,(SELECT TOP 1 BATCHNO FROM " & cnAdminDb & "..OUTSTANDING WHERE COSTID = '" & SelCostid & "' AND RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  AND ISNULL(CANCEL,'') = '' ORDER BY TRANDATE DESC)AS BATCHNO"
            Else
                strSql += vbCrLf + " ,(SELECT TOP 1 TRANDATE FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  AND ISNULL(CANCEL,'') = '' ORDER BY TRANDATE DESC)AS TRANDATE"
                strSql += vbCrLf + " ,(SELECT TOP 1 BATCHNO FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO = O.RUNNO AND COMPANYID = O.COMPANYID  AND ISNULL(CANCEL,'') = '' ORDER BY TRANDATE DESC)AS BATCHNO"
            End If
            strSql += vbCrLf + " ,O.COMPANYID,O.TRANTYPE "
            strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING O"
            strSql += vbCrLf + " WHERE ISNULL(CANCEL,'') = ''  AND SUBSTRING(RUNNO,6,1)='O' AND PAYMODE in ('OR','OP')  "
            strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
            If SelCostid <> "ALL" Then
                strSql += vbCrLf + " AND COSTID = '" & SelCostid & "'"
            End If
            strSql += vbCrLf + " AND RUNNO LIKE COSTID+COMPANYID+'O%'"
            strSql += vbCrLf + " GROUP BY RUNNO,COMPANYID,TRANTYPE"
            strSql += vbCrLf + " HAVING SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE -1*AMOUNT END) > 0 "
            strSql += vbCrLf + " )X"
            strSql += vbCrLf + " INNER JOIN TEMP" & systemId & "_CUSTOMERINFO AS V ON V.BATCHNO = X.BATCHNO"
            strSql += vbCrLf + " ORDER BY TRANDATE"
            Dim row As DataRow = Nothing
            row = BrighttechPack.SearchDialog.Show_R("Search Reference No", strSql, cn, 0, , , , , , , False)
            If Not row Is Nothing Then
                With row
                    For Each roAdv As DataRow In dtGridAdvance.Rows
                        If .Item("REFNO").ToString = roAdv!RUNNO.ToString Then
                            MsgBox("Already Exist in this Ref No", MsgBoxStyle.Information)
                            txtRunno_MAN.Select()
                            txtRunno_MAN.SelectAll()
                            Exit Sub
                        End If
                    Next
                    txtRunno_MAN.Text = .Item("REFNO").ToString
                End With
            End If
        End If

    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim RUNNO1 As String
        RUNNO1 = GetCostId(cnCostId) + GetCompanyId(strCompanyId) + txtRunno_MAN.Text
        dtitem = New DataTable
        strSql = "SELECT SNO,RUNNO, CASE WHEN SUBSTRING(RUNNO,6,1)='A' THEN 'ADVANCE' ELSE '' END AS TRANTYPE"
        strSql += vbCrLf + ", CASE WHEN RECPAY='P' THEN 'PAYMENT' ELSE 'RECEIPT' END AS RECPAY,AMOUNT,GRSWT,NETWT,CASE WHEN ISNULL(TRANFLAG,'')='L' THEN 'LOCKED' ELSE 'UNLOCKED' END STATUS,TRANFLAG  FROM " & cnAdminDb & "..OUTSTANDING O "
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..COSTCENTRE C ON O.COSTID=C.COSTID "
        strSql += vbCrLf + " WHERE ISNULL(CANCEL,'') = ''  AND RUNNO='" & RUNNO1 & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtitem)
        If Not dtitem.Rows.Count > 0 Then
            lblLock.Visible = False
            MsgBox("This Advance/Order no Already Tally or wrong Refno", MsgBoxStyle.Information)
            Exit Sub
        End If
        GridView.DataSource = dtitem
        With GridView
            For cnt As Integer = 0 To .ColumnCount - 1
                .Columns(cnt).SortMode = DataGridViewColumnSortMode.NotSortable
            Next
            .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("GRSWT").DefaultCellStyle.Format = "0.000"
            .Columns("NETWT").DefaultCellStyle.Format = "0.000"
            .Columns("AMOUNT").DefaultCellStyle.Format = "0.00"
            .Columns("SNO").Visible = False
            .Columns("TRANFLAG").Visible = False
        End With
        lblLock.Visible = True
        Label3.Visible = True
        If runno = True Then
            txtRunno_MAN.SelectAll()
            txtRunno_MAN.Focus()
        Else
            GridView.Focus()
        End If

    End Sub

    Private Sub GridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles GridView.KeyPress
        'If e.KeyChar = Chr(Keys.Space) Then
        '    If GridView.Rows.Count > 0 Then
        '        Dim SNo As String = GridView.Item("RUNNO", GridView.CurrentRow.Index).Value.ToString
        '        Dim TFLAG As String = ""
        '        TFLAG = GridView.Item("TRANFLAG", GridView.CurrentRow.Index).Value.ToString()
        '        If TFLAG = "L" Then
        '            strSql = " UPDATE " & cnAdminDb & "..OUTSTANDING SET TRANFLAG='' WHERE RUNNO='" & SNo & "'"
        '            ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
        '        Else
        '            strSql = " UPDATE " & cnAdminDb & "..OUTSTANDING SET TRANFLAG='L' WHERE RUNNO='" & SNo & "'"
        '            ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
        '        End If
        '        runno = True
        '        btnSearch_Click(Me, New KeyPressEventArgs(Chr(Keys.Enter)))
        '    End If
        'End If
    End Sub

    Private Sub rbtn_advance_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtn_advance.CheckedChanged
        If rbtn_advance.Checked = True Then
            Label1.Text = "Advance No "
        End If
    End Sub

    Private Sub rbtnorder_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtnorder.CheckedChanged
        If rbtnorder.Checked = True Then
            Label1.Text = "Order No "
        End If
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        btnCancel_Click(Me, New EventArgs)
    End Sub

    Private Sub LockToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LockToolStripMenuItem.Click
        If GridView.Rows.Count > 0 Then
            Dim TFLAG As String = ""
            Dim SNo As String = GridView.Item("RUNNO", GridView.CurrentRow.Index).Value.ToString
            If SNo = "" Then MsgBox("Runno Not found...", MsgBoxStyle.Information) : Exit Sub
            TFLAG = GridView.Item("TRANFLAG", GridView.CurrentRow.Index).Value.ToString()
            If TFLAG = "L" Then MsgBox("Already Locked...", MsgBoxStyle.Information) : Exit Sub
            If MessageBox.Show("Sure want to Lock ?...", "Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                strSql = " UPDATE " & cnAdminDb & "..OUTSTANDING SET TRANFLAG='L' WHERE RUNNO='" & SNo & "'"
                If _IsCostCentre Then
                    If cnCostId = cnHOCostId Then
                        ExecQuery(SyncMode.Master, strSql, cn, tran, cnCostId, , , , , , , False)
                    Else
                        ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                    End If
                Else
                    cmd = New OleDbCommand(strSql, cn, tran)
                    cmd.ExecuteNonQuery()
                End If
                runno = True
                btnSearch_Click(Me, New KeyPressEventArgs(Chr(Keys.Enter)))
            End If
        End If
    End Sub

    Private Sub UnLoToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UnLoToolStripMenuItem.Click
        If GridView.Rows.Count > 0 Then
            Dim TFLAG As String = ""
            Dim SNo As String = GridView.Item("RUNNO", GridView.CurrentRow.Index).Value.ToString
            If SNo = "" Then MsgBox("Runno Not found...", MsgBoxStyle.Information) : Exit Sub
            TFLAG = GridView.Item("TRANFLAG", GridView.CurrentRow.Index).Value.ToString()
            If TFLAG = "" Then MsgBox("Already UnLocked...", MsgBoxStyle.Information) : Exit Sub
            If MessageBox.Show("Sure want to Unlock ?...", "Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                strSql = " UPDATE " & cnAdminDb & "..OUTSTANDING SET TRANFLAG='' WHERE RUNNO='" & SNo & "'"
                If _IsCostCentre Then
                    If cnCostId = cnHOCostId Then
                        ExecQuery(SyncMode.Master, strSql, cn, tran, cnCostId, , , , , , , False)
                    Else
                        ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                    End If
                Else
                    cmd = New OleDbCommand(strSql, cn, tran)
                    cmd.ExecuteNonQuery()
                End If
                runno = True
                btnSearch_Click(Me, New KeyPressEventArgs(Chr(Keys.Enter)))
            End If
        End If
    End Sub
End Class