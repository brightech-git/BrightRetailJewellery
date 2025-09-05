Imports System.Data.OleDb

Public Class frmAccountsTransfer
    Dim dtCompany As New DataTable
    Dim dtCostCentre As New DataTable
    Dim strSql As String
    Dim dtAccDetails As New DataTable
    Dim dvAccDetails As New DataView
    Dim cmd As New OleDbCommand
    Dim da As New OleDbDataAdapter
    Dim tran As OleDbTransaction

    Private Sub frmAccountsTransfer_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.F3 Then
            btnNew_Click(Me, New EventArgs)
        ElseIf e.KeyCode = Keys.F12 Then
            btnExit_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub frmAccountsTransfer_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        strSql = " SELECT 'ALL' COMPANYNAME,'ALL' COMPANYID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COMPANYNAME,COMPANYID,2 RESULT FROM " & cnAdminDb & "..COMPANY "
        strSql += " ORDER BY RESULT,COMPANYNAME"
        dtCompany = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCompany)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dtCompany, "COMPANYNAME", , strCompanyName)

        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , "ALL")

        strSql = " SELECT ACNAME,CASE WHEN ISNULL(DOORNO,'') = '' THEN '' ELSE ',' + DOORNO END DOORNO,CASE WHEN ISNULL(ADDRESS1,'') = '' THEN '' ELSE ',' + ADDRESS1 END ADDRESS1"
        strSql += " ,CASE WHEN ISNULL(CITY,'') = '' THEN '' ELSE ',' + CITY END CITY,CASE WHEN ISNULL(MOBILE,'') = '' THEN '' ELSE ',' + MOBILE END MOBILE FROM " & cnAdminDb & "..ACHEAD WHERE 1=1"
        strSql += GetAcNameQryFilteration()
        strSql += " ORDER BY ACNAME"
        dtAccDetails = New DataTable
        dvAccDetails = New DataView
        dvAccDetails = dtAccDetails.DefaultView
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtAccDetails)
        gridDisplay.BackgroundColor = Color.White
        gridDisplay.Visible = False
        chkCmbCompany.Focus()
    End Sub


    Private Function funcCompanyFilt() As String
        Dim Str As String = ""
        If chkCmbCompany.Text <> "ALL" And chkCmbCompany.Text <> "" Then
            Str += " AND COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(chkCmbCompany.Text) & "))"
        End If
        Return Str
    End Function
    Private Function funcCostcentreFilt() As String
        Dim Str As String = ""
        If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
            Str += " AND COSTID IN"
            Str += "(SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & GetQryString(chkCmbCostCentre.Text) & "))"
        End If
        Return Str
    End Function

    Private Function funcCompanyFiltNot() As String
        Dim Str As String = ""
        If chkCmbCompany.Text <> "ALL" And chkCmbCompany.Text <> "" Then
            Str += " AND COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME NOT IN (" & GetQryString(chkCmbCompany.Text) & "))"
        End If
        Return Str
    End Function
    Private Function funcCostcentreFiltNot() As String
        Dim Str As String = ""
        If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
            Str += " AND COSTID IN"
            Str += "(SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME NOT IN (" & GetQryString(chkCmbCostCentre.Text) & "))"
        End If
        Return Str
    End Function

    Private Sub btnTransfer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTransfer.Click
        Try
            Dim FromAcCode As String = ""
            Dim ToAcCode As String = ""

            strSql = "SELECT ACCODE FROM " & CNADMINDB & "..ACHEAD WHERE ACNAME = '" & txtFromAcName.Text & "'"
            FromAcCode = BrighttechPack.GetSqlValue(cn, strSql, "ACCODE", "", tran)
            strSql = "SELECT ACCODE FROM " & CNADMINDB & "..ACHEAD WHERE ACNAME = '" & txtToAcName.Text & "'"
            ToAcCode = BrighttechPack.GetSqlValue(cn, strSql, "ACCODE", "", tran)

            If txtFromAcName.Text = "" Then
                MsgBox("From AcName ShouldN't Empty")
                txtFromAcName.Focus()
                Exit Sub
            End If

            If FromAcCode = "" Then
                MsgBox("Invalid From AcName")
                txtFromAcName.Focus()
                Exit Sub
            End If

            If txtToAcName.Text = "" Then
                MsgBox("To AcName ShouldN't Empty")
                txtToAcName.Focus()
                Exit Sub
            End If

            If ToAcCode = "" Then
                MsgBox("Invalid To AcName")
                txtToAcName.Focus()
                Exit Sub
            End If
            Dim CompanyFilt As String = funcCompanyFilt()
            Dim CompanyFiltNot As String = funcCompanyFiltNot()
            Dim CostcentreFilt As String = funcCostcentreFilt()
            Dim CostcentreFiltNot As String = funcCostcentreFiltNot()

            tran = cn.BeginTransaction
           
            strSql = " IF OBJECT_ID('TEMPACCODECHK') IS NOT NULL DROP TABLE TEMPACCODECHK"
            strSql += " CREATE TABLE TEMPACCODECHK(ACCODE VARCHAR(100))"
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + "  SELECT S.NAME FROM " & cnAdminDb & "..SYSOBJECTS AS S INNER JOIN " & cnAdminDb & "..SYSCOLUMNS AS C"
            strSql += vbCrLf + "  ON S.ID = C.ID"
            strSql += vbCrLf + "  WHERE S.XTYPE = 'U' AND C.NAME = 'ACCODE'"
            strSql += vbCrLf + "  AND S.NAME NOT IN ('ACHEAD')"
            strSql += vbCrLf + "  ORDER BY S.NAME"
            Dim dtTables As New DataTable
            cmd = New OleDbCommand(strSql, cn, tran)
            da = New OleDbDataAdapter(cmd)
            da.Fill(dtTables)
            If dtTables.Rows.Count > 0 Then
                For cnt As Integer = 0 To dtTables.Rows.Count - 1
                    'If (chkCmbCompany.Text <> "ALL" Or chkCmbCostCentre.Text <> "ALL") And chkAccodeDelete.Checked = True Then
                    '    strSql = " INSERT INTO TEMPACCODECHK(ACCODE)"
                    '    strSql += " SELECT ACCODE FROM " & cnAdminDb & ".." & dtTables.Rows(cnt).Item("NAME").ToString & " WHERE ACCODE ='" & FromAcCode & "'" & CompanyFiltNot
                    '    cmd = New OleDbCommand(strSql, cn, tran)
                    '    cmd.ExecuteNonQuery()
                    'End If
                    strSql = " UPDATE " & cnAdminDb & "..[" & dtTables.Rows(cnt).Item("NAME").ToString & "] SET ACCODE ='" & ToAcCode & "'"
                    strSql += " WHERE ACCODE = '" & FromAcCode & "'"
                    'strSql += CompanyFilt
                    cmd = New OleDbCommand(strSql, cn, tran)
                    cmd.ExecuteNonQuery()
                Next
            End If

            strSql = vbCrLf + "  SELECT S.NAME FROM " & CNADMINDB & "..SYSOBJECTS AS S INNER JOIN " & CNADMINDB & "..SYSCOLUMNS AS C"
            strSql += vbCrLf + "  ON S.ID = C.ID"
            strSql += vbCrLf + "  WHERE S.XTYPE = 'U' AND C.NAME = 'DEFAULTAC'"
            strSql += vbCrLf + "  AND S.NAME NOT IN ('ACHEAD')"
            strSql += vbCrLf + "  ORDER BY S.NAME"
            dtTables = New DataTable
            cmd = New OleDbCommand(strSql, cn, tran)
            da = New OleDbDataAdapter(cmd)
            da.Fill(dtTables)
            If dtTables.Rows.Count > 0 Then
                For cnt As Integer = 0 To dtTables.Rows.Count - 1
                    
                    strSql = " UPDATE " & cnAdminDb & "..[" & dtTables.Rows(cnt).Item("NAME").ToString & "] SET DEFAULTAC ='" & ToAcCode & "'"
                    strSql += " WHERE DEFAULTAC = '" & FromAcCode & "'"
                    'strSql += CompanyFilt
                    cmd = New OleDbCommand(strSql, cn, tran)
                    cmd.ExecuteNonQuery()
                Next
            End If
            Dim dtdbs As New DataTable
            strSql = vbCrLf + "  SELECT DBNAME FROM " & cnAdminDb & "..DBMASTER  ORDER BY DBNAME"
            cmd = New OleDbCommand(strSql, cn, tran)
            da = New OleDbDataAdapter(cmd)
            da.Fill(dtdbs)
            If dtdbs.Rows.Count > 0 Then
                Dim dttrandb As String = ""
                For dcnt As Integer = 0 To dtdbs.Rows.Count - 1
                    dttrandb = dtdbs.Rows(dcnt).Item("DBNAME").ToString
                    If objGPack.GetSqlValue("SELECT TOP 1 1 FROM MASTER..SYSDATABASES WHERE NAME = '" & dttrandb & "'", , "0", tran) <> 1 Then Continue For

                    strSql = vbCrLf + "  UPDATE " & dttrandb & "..LBDETAIL SET ACCODE = '" & ToAcCode & "'"
                    strSql += vbCrLf + "  FROM " & dttrandb & "..LBDETAIL AS L INNER JOIN " & cnAdminDb & "..OUTSTANDING AS O"
                    strSql += vbCrLf + "  ON L.BATCHNO = O.BATCHNO AND L.TRANNO = O.TRANNO AND L.ACCODE = O.ACCODE"
                    strSql += vbCrLf + "  WHERE L.ACCODE = '" & FromAcCode & "'" & CompanyFilt & CostcentreFilt
                    cmd = New OleDbCommand(strSql, cn, tran)
                    cmd.ExecuteNonQuery()

                    strSql = vbCrLf + "  SELECT S.NAME FROM " & dttrandb & "..SYSOBJECTS AS S "
                    strSql += vbCrLf + "  INNER JOIN " & dttrandb & "..SYSCOLUMNS AS C ON S.ID = C.ID"
                    strSql += vbCrLf + "  INNER JOIN " & dttrandb & "..SYSCOLUMNS AS CC ON C.ID = CC.ID"
                    strSql += vbCrLf + "  WHERE S.XTYPE = 'U' AND S.NAME NOT LIKE 'TEMP%' AND C.NAME = 'ACCODE' AND CC.NAME='COMPANYID' AND S.NAME <> 'LBDETAIL'"
                    strSql += vbCrLf + "  ORDER BY S.NAME"
                    dtTables = New DataTable
                    cmd = New OleDbCommand(strSql, cn, tran)
                    da = New OleDbDataAdapter(cmd)
                    da.Fill(dtTables)
                    If dtTables.Rows.Count > 0 Then
                        For cnt As Integer = 0 To dtTables.Rows.Count - 1
                            strSql = vbCrLf + "  SELECT COUNT(*) FROM " & dttrandb & "..SYSOBJECTS AS S "
                            strSql += vbCrLf + "  INNER JOIN " & dttrandb & "..SYSCOLUMNS AS C ON S.ID = C.ID"
                            strSql += vbCrLf + "  WHERE S.XTYPE = 'U' AND  S.NAME ='" & dtTables.Rows(cnt).Item("NAME").ToString & "' AND C.NAME = 'COSTID'"
                            Dim costid As Boolean = IIf(Val(objGPack.GetSqlValue(strSql, , , tran)) > 0, True, False)

                            If (chkCmbCompany.Text <> "ALL" Or chkCmbCostCentre.Text <> "ALL") And chkAccodeDelete.Checked = True Then
                                strSql = " INSERT INTO TEMPACCODECHK(ACCODE)"
                                strSql += " SELECT ACCODE FROM " & dttrandb & "..[" & dtTables.Rows(cnt).Item("NAME").ToString & "] WHERE ACCODE ='" & FromAcCode & "'" & CompanyFiltNot
                                If costid = True Then strSql += CostcentreFiltNot
                                cmd = New OleDbCommand(strSql, cn, tran)
                                cmd.ExecuteNonQuery()
                            End If

                            strSql = " UPDATE " & dttrandb & "..[" & dtTables.Rows(cnt).Item("NAME").ToString & "] SET ACCODE ='" & ToAcCode & "'"
                            strSql += " WHERE ACCODE = '" & FromAcCode & "'"
                            strSql += CompanyFilt
                            If costid = True Then strSql += CostcentreFilt
                            cmd = New OleDbCommand(strSql, cn, tran)
                            cmd.ExecuteNonQuery()
                        Next
                    End If

                    strSql = vbCrLf + "  SELECT S.NAME FROM " & dttrandb & "..SYSOBJECTS AS S INNER JOIN " & dttrandb & "..SYSCOLUMNS AS C"
                    strSql += vbCrLf + "  ON S.ID = C.ID"
                    strSql += vbCrLf + "  WHERE S.XTYPE = 'U' AND (C.NAME = 'ACCODE' OR C.NAME = 'CONTRA')"
                    strSql += vbCrLf + "  GROUP BY S.NAME"
                    strSql += vbCrLf + "  HAVING COUNT(*) > 1"
                    strSql += vbCrLf + "  ORDER BY S.NAME"
                    dtTables = New DataTable
                    cmd = New OleDbCommand(strSql, cn, tran)
                    da = New OleDbDataAdapter(cmd)
                    da.Fill(dtTables)
                    If dtTables.Rows.Count > 0 Then
                        For cnt As Integer = 0 To dtTables.Rows.Count - 1
                            strSql = vbCrLf + "  SELECT COUNT(*) FROM " & dttrandb & "..SYSOBJECTS AS S "
                            strSql += vbCrLf + "  INNER JOIN " & dttrandb & "..SYSCOLUMNS AS C ON S.ID = C.ID"
                            strSql += vbCrLf + "  WHERE S.XTYPE = 'U' AND S.NAME ='" & dtTables.Rows(cnt).Item("NAME").ToString & "' AND C.NAME = 'COSTID'"
                            Dim costid As Boolean = IIf(Val(objGPack.GetSqlValue(strSql, , , tran)) > 0, True, False)

                            If (chkCmbCompany.Text <> "ALL" And chkCmbCostCentre.Text <> "ALL") And chkAccodeDelete.Checked = True Then
                                strSql = " INSERT INTO TEMPACCODECHK(ACCODE)"
                                strSql += " SELECT ACCODE FROM " & dttrandb & "..[" & dtTables.Rows(cnt).Item("NAME").ToString & "] WHERE CONTRA ='" & FromAcCode & "'" & CompanyFiltNot
                                If costid = True Then strSql += CostcentreFiltNot
                                cmd = New OleDbCommand(strSql, cn, tran)
                                cmd.ExecuteNonQuery()
                            End If
                            strSql = " UPDATE " & dttrandb & "..[" & dtTables.Rows(cnt).Item("NAME").ToString & "] SET CONTRA ='" & ToAcCode & "'"
                            strSql += " WHERE CONTRA = '" & FromAcCode & "'"
                            strSql += CompanyFilt
                            If costid = True Then strSql += CostcentreFilt
                            cmd = New OleDbCommand(strSql, cn, tran)
                            cmd.ExecuteNonQuery()
                        Next
                    End If
                Next
            End If

            If chkAccodeDelete.Checked = True Then
                strSql = "SELECT COUNT(*) COUNTROW FROM TEMPACCODECHK"
                If Val(BrighttechPack.GetSqlValue(cn, strSql, "COUNTROW", "0", tran)) > 0 Then
                    tran.Commit()
                    tran.Dispose()
                    MsgBox("You can't delete this accode")
                    Exit Sub
                End If

                strSql = " DELETE FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = '" & FromAcCode & "'"
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.ExecuteNonQuery()

                For Each dr As DataRow In dtAccDetails.Select("ACNAME = '" & txtFromAcName.Text & "'")
                    dtAccDetails.Rows.Remove(dr)
                    dtAccDetails.AcceptChanges()
                Next
            End If

            tran.Commit()
            tran.Dispose()
            btnNew_Click(Me, New EventArgs())
        Catch ex As Exception
            If tran IsNot Nothing Then
                tran.Rollback()
                tran.Dispose()
            End If
            MsgBox(ex.Message + ex.StackTrace)
        End Try
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dtCompany, "COMPANYNAME", , strCompanyName)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , "ALL")
        txtFromAcName.Text = ""
        txtToAcName.Text = ""
        chkAccodeDelete.Checked = False
        gridDisplay.Visible = False
        chkCmbCompany.Focus()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub txtFromAcName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFromAcName.TextChanged
        txtFromAcName.Text = Replace(txtFromAcName.Text, "%", "")
        dvAccDetails.RowFilter = "ACNAME LIKE '%" & txtFromAcName.Text & "%'"
        gridDisplay.DataSource = dvAccDetails
        gridDisplay.Visible = True
        If gridDisplay.Rows.Count > 0 Then
            With gridDisplay
                .Columns("ACNAME").Width = 240
                .Columns("DOORNO").Width = 40
                .Columns("ADDRESS1").Width = 200
                .Columns("CITY").Width = 125
                .Columns("MOBILE").Width = 125
            End With
        End If
    End Sub

    Private Sub txtToAcName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtToAcName.TextChanged
        txtToAcName.Text = Replace(txtToAcName.Text, "%", "")
        dvAccDetails.RowFilter = "ACNAME LIKE '%" & txtToAcName.Text & "%'"
        gridDisplay.DataSource = dvAccDetails
        gridDisplay.Visible = True
        If gridDisplay.Rows.Count > 0 Then
            With gridDisplay
                .Columns("ACNAME").Width = 240
                .Columns("DOORNO").Width = 40
                .Columns("ADDRESS1").Width = 200
                .Columns("CITY").Width = 125
                .Columns("MOBILE").Width = 125
            End With
        End If
    End Sub

    Private Sub txtFromAcName_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFromAcName.Enter
        gridDisplay.DataSource = Nothing
        gridDisplay.Location = New Point(txtFromAcName.Location.X, txtFromAcName.Location.Y + txtFromAcName.Height)
        gridDisplay.Visible = True
        gridDisplay.BringToFront()
    End Sub

    Private Sub txtToAcName_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtToAcName.Enter
        gridDisplay.DataSource = Nothing
        gridDisplay.Location = New Point(txtToAcName.Location.X, txtToAcName.Location.Y + txtToAcName.Height)
        gridDisplay.Visible = True
        gridDisplay.BringToFront()
    End Sub

    Private Sub txtFromAcName_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFromAcName.Leave
        If gridDisplay.Focused = True Then Exit Sub
        gridDisplay.Visible = False
        gridDisplay.DataSource = Nothing
    End Sub

    Private Sub txtToAcName_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtToAcName.Leave
        If gridDisplay.Focused = True Then Exit Sub
        gridDisplay.Visible = False
        gridDisplay.DataSource = Nothing
    End Sub

    Private Sub frmAccountsTransfer_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then 
            If gridDisplay.Focused = True Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtFromAcName_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtFromAcName.KeyDown
        If e.KeyCode = Keys.Down Then
            If gridDisplay.Rows.Count > 0 Then
                gridDisplay.Focus()
            End If
        End If
    End Sub

    Private Sub txtToAcName_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtToAcName.KeyDown
        If e.KeyCode = Keys.Down Then
            If gridDisplay.Rows.Count > 0 Then
                gridDisplay.Focus()
            End If
        End If
    End Sub

    Private Sub gridDisplay_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridDisplay.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            e.Handled = True
            If gridDisplay.Rows.Count > 0 Then
                gridDisplay.CurrentCell = gridDisplay.Rows(gridDisplay.CurrentRow.Index).Cells(gridDisplay.FirstDisplayedCell.ColumnIndex)
                If txtFromAcName.Location.Y + txtFromAcName.Height = gridDisplay.Location.Y Then
                    txtFromAcName.Text = gridDisplay.CurrentRow.Cells("ACNAME").Value.ToString
                    gridDisplay.Visible = False
                    Me.SelectNextControl(txtFromAcName, True, True, True, True)
                    Exit Sub
                End If
                If txtToAcName.Location.Y + txtToAcName.Height = gridDisplay.Location.Y Then
                    txtToAcName.Text = gridDisplay.CurrentRow.Cells("ACNAME").Value.ToString
                    gridDisplay.Visible = False
                    Me.SelectNextControl(txtToAcName, True, True, True, True)
                    Exit Sub
                End If
            End If
        End If
    End Sub

    Private Sub gridDisplay_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridDisplay.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
        End If
    End Sub
End Class