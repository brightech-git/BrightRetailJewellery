Imports System.Data.SqlClient
Imports System.Data.OleDb
Public Class frmPrevilege
    Dim strSql As String
    Dim dt As DataTable
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Dim PREV_RECORD As Boolean = IIf(GetAdmindbSoftValue("PREV_RECORD", "N") = "Y", True, False)
    Dim PRETYPE As Boolean = IIf(GetAdmindbSoftValue("PRETYPE", "N") = "Y", True, False)
    Private Sub frmPrevilege_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub btnSave_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click, SaveToolStripMenuItem.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
        Dim ctrgroup As String = ""
        Dim ctrgrop As String = ""
        Dim Id As Integer
        If txtprevilegeID.Text = "" Then
            MsgBox("Previlege Id Should not Empty", MsgBoxStyle.Information)
        Else
            Id = txtprevilegeID.Text
            strSql = "SELECT COUNT(*) FROM  " & cnAdminDb & "..ACHEAD WHERE PREVILEGEID=" & Id
            If objGPack.GetSqlValue(strSql, "", 0) = 0 Then MsgBox("Previlege Id Not Found", MsgBoxStyle.Information) : Exit Sub
            If rbtAdd.Checked Then
                'strSql = " INSERT INTO " & cnStockDb & "..OPENPREVILEGE"
                'strSql += " VALUES ("
                'strSql += " '" & txtprevilegeID.Text & "'"
                'strSql += " ,'" & Val(txtprevilegepoints.Text) & "'"
                'strSql += " ,'" & Val(txtPrevilegevalue.Text) & "'"
                'strSql += " ,'" & Val(txtAmount.Text) & "')"
                'ExecQuery(SyncMode.Master, strSql, cn)
                'MsgBox("Saved Successfully", MsgBoxStyle.Information)

                Dim BatchNo As String = GetNewBatchno(cnCostId, GetServerDate(), tran)
                Dim ptsno As String = ""
                ptsno = GetNewSno(TranSnoType.PRIVILEGETRANCODE, tran, "GET_ADMINSNO_TRAN")
                strSql = " INSERT INTO " & cnAdminDb & "..PRIVILEGETRAN"
                strSql += " ("
                strSql += " SNO,BATCHNO,PREVILEGEID,TRANTYPE,ENTRYTYPE,TRANDATE,POINTS,PVALUE,USERID)"
                strSql += " VALUES('" & ptsno & "','" & BatchNo & "','" & txtprevilegeID.Text & "','R','M'"
                strSql += " ,'" & GetServerDate() & "','" & Val(txtprevilegepoints.Text) & "','" & Val(txtPrevilegevalue.Text) & "'"
                strSql += " ," & userId & ")"
                ExecQuery(SyncMode.Master, strSql, cn, tran, cnCostId, , , , , , , False) ''Sync to all Location
            Else
                If PREV_RECORD Then
                    strSql = "SELECT SUM(CASE WHEN TRANTYPE='R' THEN POINTS ELSE -1*POINTS END) AS POINTS "
                    strSql += vbCrLf + " ,SUM(CASE WHEN TRANTYPE='R' THEN PVALUE ELSE -1*PVALUE END)AS PVALUE"
                    strSql += vbCrLf + " FROM  " & cnAdminDb & "..PRIVILEGETRAN  "
                    strSql += vbCrLf + " WHERE PREVILEGEID='" & Trim(txtprevilegeID.Text) & "'"
                    strSql += vbCrLf + " AND ISNULL(CANCEL,'')=''  "
                    cmd = New OleDbCommand(strSql, cn)
                    da = New OleDbDataAdapter(cmd)
                    dt = New DataTable
                    da.Fill(dt)
                    If dt.Rows.Count > 0 Then
                        Dim minPvPoints As Integer = Val(dt.Rows(0).Item(0).ToString())
                        If Val(txtprevilegepoints.Text) > minPvPoints Then
                            MsgBox("Redeem Points Should not Exceed Minimum Points...", MsgBoxStyle.Information)
                            txtprevilegepoints.Focus()
                            Exit Sub
                        End If
                    Else
                        MsgBox("Accumulated Points are less than Minimum Points", MsgBoxStyle.Information)
                        txtprevilegepoints.Focus()
                        Exit Sub
                    End If
                Else
                    Dim Sysid As String = Trim(LSet(Mid(Guid.NewGuid.ToString, 1, InStr(Guid.NewGuid.ToString, "-") - 1), 10))
                    Dim StartDate As Date
                    StartDate = objGPack.GetSqlValue("SELECT TOP 1 STARTDATE FROM " & cnAdminDb & "..DBMASTER ORDER BY STARTDATE ASC")
                    Dim Fromdate As Date = IIf(GetAdmindbSoftValue("PREVILEDGE_DATE", StartDate) = "", StartDate, GetAdmindbSoftValue("PREVILEDGE_DATE", StartDate))

                    strSql = " EXEC " & cnStockDb & "..SP_RPT_PREVILEGETRAN"
                    strSql += vbCrLf + " @SYSTEMID = '" & Sysid & "'"
                    strSql += vbCrLf + " ,@FRMDATE = '" & Format(Fromdate, "yyyy-MM-dd") & "'"
                    strSql += vbCrLf + " ,@TODATE = '" & cnTranToDate.Date.ToString("yyyy-MM-dd") & "'"
                    strSql += vbCrLf + " ,@PREVILEGEID = '" & txtprevilegeID.Text & "'"
                    strSql += vbCrLf + " ,@METALNAME = 'ALL'"
                    strSql += vbCrLf + " ,@COSTNAME = 'ALL'"
                    strSql += vbCrLf + " ,@COMPANYID = '" & strCompanyId & "'"
                    If GetAdmindbSoftValue("CHITDB", "N").ToUpper = "Y" Then
                        strSql += vbCrLf + " ,@WITHCHITPOINTS = 'Y'"
                    Else
                        strSql += vbCrLf + " ,@WITHCHITPOINTS = 'N'"
                    End If
                    If PRETYPE Then
                        strSql += vbCrLf + " ,@ONLYPREVILEGE = 'N'"
                    End If
                    cmd = New OleDbCommand(strSql, cn)
                    cmd.CommandTimeout = 1000
                    cmd.ExecuteNonQuery()

                    strSql = "SELECT SUM(ISNULL(POINTS,0)) , SUM(ISNULL(BPOINTVALUES,0)),VALUEINWTRAMT  "
                    strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & Sysid & "PREVILEGETRAN  "
                    strSql += vbCrLf + " WHERE RESULT=1 AND ISNULL(VALUEINWTRAMT,'')<>'' GROUP BY VALUEINWTRAMT"
                    cmd = New OleDbCommand(strSql, cn)
                    da = New OleDbDataAdapter(cmd)
                    dt = New DataTable
                    da.Fill(dt)
                    If dt.Rows.Count > 0 Then
                        Dim minPvPoints As Integer = Val(dt.Rows(0).Item(0).ToString())
                        If Val(txtprevilegepoints.Text) > minPvPoints Then
                            MsgBox("Redeem Points Should not Exceed Minimum Points...", MsgBoxStyle.Information)
                            txtprevilegepoints.Focus()
                            Exit Sub
                        End If
                    Else
                        MsgBox("Accumulated Points are less than Minimum Points", MsgBoxStyle.Information)
                        txtprevilegepoints.Focus()
                        Exit Sub
                    End If
                End If
                Dim BatchNo As String = GetNewBatchno(cnCostId, GetServerDate(), tran)
                If PREV_RECORD Then
                    Dim ptsno As String = ""
                    ptsno = GetNewSno(TranSnoType.PRIVILEGETRANCODE, tran, "GET_ADMINSNO_TRAN")
                    strSql = " INSERT INTO " & cnAdminDb & "..PRIVILEGETRAN"
                    strSql += " ("
                    strSql += " SNO,BATCHNO,PREVILEGEID,TRANTYPE,ENTRYTYPE,TRANDATE,POINTS,PVALUE,USERID)"
                    strSql += " VALUES('" & ptsno & "','" & BatchNo & "','" & txtprevilegeID.Text & "','I','M'"
                    strSql += " ,'" & GetServerDate() & "','" & Val(txtprevilegepoints.Text) & "','" & Val(txtPrevilegevalue.Text) & "'"
                    strSql += " ," & userId & ")"
                    ExecQuery(SyncMode.Master, strSql, cn, tran, cnCostId, , , , , , , False) ''Sync to all Location
                Else
                    strSql = " INSERT INTO " & cnStockDb & "..PRIVILEDGETRAN"
                    strSql += " ("
                    strSql += " BATCHNO,PREVILEGEID,TRANDATE,POINTS,PVALUE)"
                    strSql += " VALUES('" & BatchNo & "','" & txtprevilegeID.Text & "'"
                    strSql += " ,'" & GetServerDate() & "'," & Val(txtprevilegepoints.Text) & ""
                    strSql += " ," & Val(txtPrevilegevalue.Text) & ")"
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                End If
                MsgBox("Saved Successfully", MsgBoxStyle.Information)
            End If
        End If
        btnNew_Click_1(Me, New EventArgs)
    End Sub

    Private Sub btnNew_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        txtprevilegeID.Text = ""
        txtprevilegepoints.Text = ""
        txtPrevilegevalue.Text = ""
        lblPoints.Text = ""
        lblCustomer.Text = ""
        txtAmount.Text = ""
        rbtDeduct.Focus()
    End Sub

    Private Sub btnExit_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnOpen_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click, OpenToolStripMenuItem.Click
        funcView()
    End Sub

    Function funcView()
        strSql = "SELECT *  FROM " & cnAdminDb & "..PRIVILEGETRAN WHERE ENTRYTYPE='M' "
        da = New OleDbDataAdapter(strSql, cn)
        Dim dtgrid As New DataTable
        da.Fill(dtgrid)
        If dtgrid.Rows.Count > 0 Then
            gridView.DataSource = Nothing
            gridView.DataSource = dtgrid
            tabprevilege.SelectedTab = tabview
            With gridView
                .Focus()
                .Columns("SNO").Visible = False
                .Columns("TRANDATE").Visible = False
                .Columns("TRANNO").Visible = False
                .Columns("CANCEL").Visible = False
                .Columns("USERID").Visible = False
                .Columns("UPDATED").Visible = False
                .Columns("UPTIME").Visible = False
                .Columns("BATCHNO").Visible = False
            End With
        Else
            gridView.DataSource = Nothing
            MsgBox("Records not found.", MsgBoxStyle.Information)
        End If
    End Function

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabprevilege.SelectedTab = tabGeneral
    End Sub

    Private Sub frmPrevilege_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        tabprevilege.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabprevilege.Region = New Region(New RectangleF(Me.tabGeneral.Left, Me.tabGeneral.Top, Me.tabGeneral.Width, Me.tabGeneral.Height))
        rbtDeduct.Focus()
    End Sub

    Private Sub txtprevilegeID_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtprevilegeID.KeyDown
        If e.KeyCode = Keys.Insert Then
            custname()
        End If
        If e.KeyCode = Keys.Enter Then
            If txtprevilegeID.Text.ToString() = "" Then
                Dim STRSQL As String
                STRSQL = " SELECT PREVILEGEID,ACCODE ,ACNAME FROM " & cnAdminDb & "..ACHEAD "
                STRSQL += " where  PREVILEGEID <>'' GROUP BY PREVILEGEID,ACCODE ,ACNAME "
                txtprevilegeID.Text = BrighttechPack.SearchDialog.Show("Select CUSTOMER", STRSQL, cn, 2)
            Else
                lblCustomer.Text = (objGPack.GetSqlValue("SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE PREVILEGEID = '" & Trim(txtprevilegeID.Text) & "'", , ))
            End If
            If Trim(txtprevilegeID.Text) <> "" Then
                If PREV_RECORD Then
                    strSql = "SELECT SUM(CASE WHEN TRANTYPE='R' THEN POINTS ELSE -1*POINTS END) AS POINTS "
                    strSql += vbCrLf + " ,SUM(CASE WHEN TRANTYPE='R' THEN PVALUE ELSE -1*PVALUE END)AS PVALUE"
                    strSql += vbCrLf + " FROM  " & cnAdminDb & "..PRIVILEGETRAN  "
                    strSql += vbCrLf + " WHERE PREVILEGEID='" & Trim(txtprevilegeID.Text) & "'"
                    strSql += vbCrLf + " AND ISNULL(CANCEL,'')=''  "
                    cmd = New OleDbCommand(strSql, cn)
                    da = New OleDbDataAdapter(cmd)
                    Dim dt As New DataTable
                    da.Fill(dt)
                    If dt.Rows.Count > 0 Then
                        lblPoints.Text = "Points:" & Val(dt.Rows(0).Item(0).ToString())
                    Else
                        lblPoints.Text = ""
                    End If
                Else
                    Dim StartDate As Date
                    StartDate = objGPack.GetSqlValue("SELECT TOP 1 STARTDATE FROM " & cnAdminDb & "..DBMASTER ORDER BY STARTDATE ASC")
                    Dim Fromdate As Date = IIf(GetAdmindbSoftValue("PREVILEDGE_DATE", StartDate) = "", StartDate, GetAdmindbSoftValue("PREVILEDGE_DATE", StartDate))
                    Dim Sysid As String = Trim(LSet(Mid(Guid.NewGuid.ToString, 1, InStr(Guid.NewGuid.ToString, "-") - 1), 10))

                    strSql = " EXEC " & cnStockDb & "..SP_RPT_PREVILEGETRAN"
                    strSql += vbCrLf + " @SYSTEMID = '" & Sysid & "'"
                    strSql += vbCrLf + " ,@FRMDATE = '" & Format(Fromdate, "yyyy-MM-dd") & "'"
                    strSql += vbCrLf + " ,@TODATE = '" & cnTranToDate.Date.ToString("yyyy-MM-dd") & "'"
                    strSql += vbCrLf + " ,@PREVILEGEID = '" & txtprevilegeID.Text & "'"
                    strSql += vbCrLf + " ,@METALNAME = 'ALL'"
                    strSql += vbCrLf + " ,@COSTNAME = 'ALL'"
                    strSql += vbCrLf + " ,@COMPANYID = '" & strCompanyId & "'"
                    If GetAdmindbSoftValue("CHITDB", "N").ToUpper = "Y" Then
                        strSql += vbCrLf + " ,@WITHCHITPOINTS = 'Y'"
                    Else
                        strSql += vbCrLf + " ,@WITHCHITPOINTS = 'N'"
                    End If
                    If PRETYPE Then
                        strSql += vbCrLf + " ,@ONLYPREVILEGE = 'N'"
                    End If
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                    cmd.ExecuteNonQuery()
                    Dim dt As New DataTable
                     
                    strSql = "SELECT SUM(ISNULL(POINTS,0)) , SUM(ISNULL(BPOINTVALUES,0)),VALUEINWTRAMT  "
                    strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & Sysid & "PREVILEGETRAN  "
                    strSql += vbCrLf + " WHERE RESULT=1  GROUP BY VALUEINWTRAMT"
                    cmd = New OleDbCommand(strSql, cn)
                    da = New OleDbDataAdapter(cmd)
                    da.Fill(dt)
                    If dt.Rows.Count > 0 Then
                        lblPoints.Text = "Points:" & Val(dt.Rows(0).Item(0).ToString)
                    Else
                        lblPoints.Text = ""
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub custname()
        strSql = " SELECT LTRIM(PREVILEGEID)PREVILEGEID,ACCODE ,ACNAME,MOBILE FROM " & cnAdminDb & "..ACHEAD "
        strSql += " WHERE  PREVILEGEID <>'' ORDER BY PREVILEGEID,ACCODE,ACNAME,MOBILE"
        Dim priid As String = BrighttechPack.SearchDialog.Show("Search Previledge Customer ", strSql, cn, 3)
        If Trim(priid) <> "" Then
            txtprevilegeID.Text = priid
        End If
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
        If Not gridView.RowCount > 0 Then Exit Sub
        Dim Sno As String = gridView.CurrentRow.Cells("SNO").Value.ToString
        strSql = " DELETE FROM  " & cnAdminDb & "..PRIVILEGETRAN WHERE SNO ='" & Sno & "'"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()
        MsgBox("Deleted Successfully.", MsgBoxStyle.Information)
        funcView()
    End Sub

    Private Sub rbtAdd_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtAdd.CheckedChanged
        txtAmount.Enabled = rbtAdd.Checked
    End Sub

    Private Sub txtprevilegeID_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtprevilegeID.Leave
        If Trim(txtprevilegeID.Text) <> "" Then
            lblCustomer.Text = (objGPack.GetSqlValue("SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE PREVILEGEID = '" & Trim(txtprevilegeID.Text) & "'", , ))
        End If
    End Sub
End Class