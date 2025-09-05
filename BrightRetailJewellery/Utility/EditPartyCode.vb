Imports System.Data.OleDb
Public Class EditPartyCode
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter

    Private Sub EditPartyCode_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If cmbAcName.Focused Then
                gridView_OWN.Focus()
            End If
        End If
    End Sub

    Private Sub EditPartyCode_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If gridView_OWN.Focused Then Exit Sub
            If cmbAcName.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub EditPartyCode_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ''ACNAME
        strSql = " SELECT ACNAME,ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE 1=1 "
        strSql += GetAcNameQryFilteration()
        strSql += " ORDER BY ACNAME "
        'objGPack.FillCombo(strSql, cmbAcName)
        Dim Dtachead As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(Dtachead)
        cmbAcName.DataSource = Dtachead
        cmbAcName.DisplayMember = "ACNAME"
        cmbAcName.ValueMember = "ACCODE"

        gridView_OWN.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
        grpContainer.Location = New Point((ScreenWid - grpContainer.Width) / 2, ((ScreenHit - 128) - grpContainer.Height) / 2)
        gridView_OWN.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        dtpFrom.Value = GetEntryDate(GetServerDate)
        dtpTo.Value = GetEntryDate(GetServerDate)
        gridView_OWN.DataSource = Nothing
        cmbAcName.Visible = False
        lblStatus.Visible = False
        dtpFrom.Select()
    End Sub

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        gridView_OWN.DataSource = Nothing
        strSql = vbCrLf + " SELECT TRANNO,TRANDATE"
        strSql += vbCrLf + " ,(SELECT ACNAME FROM " & CNADMINDB & "..ACHEAD WHERE ACCODE = O.ACCODE)aS ACNAME"
        strSql += vbCrLf + " ,RUNNO,AMOUNT"
        strSql += vbCrLf + " ,CASE PAYMODE WHEN 'AA' THEN 'ADVANCE'"
        strSql += vbCrLf + " WHEN 'DR' THEN 'CREDIT SALES'"
        strSql += vbCrLf + " WHEN 'AR' THEN 'CUSTOMER ADVANCE'"
        strSql += vbCrLf + " WHEN 'MR' THEN 'OTHER RECEIPTS'"
        strSql += vbCrLf + " WHEN 'HR' THEN 'SCHEME RECEIPT'"
        strSql += vbCrLf + " WHEN 'OR' THEN 'FURTHER ADVANCE'"
        strSql += vbCrLf + " WHEN 'DP' THEN 'PURCHASE\SALES RETURN'"
        strSql += vbCrLf + " WHEN 'AP' THEN 'ADVANCE REPAY'"
        strSql += vbCrLf + " WHEN 'MP' THEN 'OTHER PAYMENTS'"
        strSql += vbCrLf + " WHEN 'HP' THEN 'SCHEME PAYMENT'"
        strSql += vbCrLf + " ELSE PAYMODE END AS TRAN_DESC"
        strSql += vbCrLf + " ,PAYMODE"
        strSql += vbCrLf + " ,CASE WHEN RECPAY = 'R' THEN 'RECEIPT' ELSE 'PAYMENT' END RECPAY,ACCODE,BATCHNO,COMPANYID,COSTID"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING AS O"
        strSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
        strSql += vbCrLf + " AND AMOUNT <> 0 AND FROMFLAG IN ('P','D','R')"
        strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + " ORDER BY TRANDATE,TRANNO"
        Dim dtGrid As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            dtpFrom.Select()
            Exit Sub
        End If
        gridView_OWN.DataSource = dtGrid
        FormatGridColumns(gridView_OWN, False)
        With gridView_OWN
            .Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns("TRANNO").Width = 70
            .Columns("TRANDATE").Width = 80
            .Columns("ACNAME").Width = 400
            .Columns("RUNNO").Width = 60
            .Columns("AMOUNT").Width = 100
            .Columns("TRAN_DESC").Width = 120
            .Columns("PAYMODE").Visible = False
            .Columns("RECPAY").Visible = False
            .Columns("ACCODE").Visible = False
            .Columns("BATCHNO").Visible = False
            .Columns("COMPANYID").Visible = False
            .Columns("COSTID").Visible = False
        End With
        gridView_OWN.Focus()
        gridView_OWN.CurrentCell = gridView_OWN.Rows(0).Cells("ACNAME")
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub gridView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView_OWN.GotFocus
        lblStatus.Visible = True
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView_OWN.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            If Not gridView_OWN.Rows.Count > 0 Then Exit Sub
            gridView_OWN.CurrentCell = gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("ACNAME")
        End If
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView_OWN.KeyPress
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Edit) Then Exit Sub
        If e.KeyChar = Chr(Keys.Enter) Then
            If Not gridView_OWN.RowCount > 0 Then Exit Sub
            If gridView_OWN.CurrentRow Is Nothing Then Exit Sub
            gridView_OWN.CurrentCell = gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("ACNAME")
            Dim pt As Point = gridView_OWN.Location
            cmbAcName.Visible = True
            pt = pt + gridView_OWN.GetCellDisplayRectangle(gridView_OWN.Columns("ACNAME").Index, gridView_OWN.CurrentRow.Index, False).Location
            cmbAcName.Size = gridView_OWN.GetCellDisplayRectangle(gridView_OWN.Columns("ACNAME").Index, gridView_OWN.CurrentRow.Index, False).Size
            If gridView_OWN.CurrentRow.Cells("ACNAME").Value.ToString <> "" Then
                cmbAcName.Text = gridView_OWN.CurrentRow.Cells("ACNAME").Value
            Else
                cmbAcName.Text = ""
            End If
            cmbAcName.Location = pt
            cmbAcName.Focus()
        End If
    End Sub

    Private Sub gridView_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView_OWN.LostFocus
        lblStatus.Visible = False
    End Sub

    Private Sub gridView_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView_OWN.SelectionChanged
        'gridView.CurrentCell = gridView.Rows(gridView.CurrentRow.Index).Cells("DEFAULTBANK")
    End Sub

    Private Sub cmbAcName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbAcName.KeyDown
        If e.KeyCode = Keys.Enter Then
            Try
                If gridView_OWN.CurrentCell.Value.ToString.ToUpper = cmbAcName.Text.ToUpper Then
                    Exit Sub
                End If
                'If Not cmbAcName.Items.Contains(cmbAcName.Text) Then
                '    MsgBox("Invalid Name", MsgBoxStyle.Information)
                '    cmbAcName.Select()
                '    Exit Sub
                'End If
                If cmbAcName.SelectedValue Is Nothing Then
                    MsgBox("Invalid Name", MsgBoxStyle.Information)
                    cmbAcName.Select()
                    Exit Sub
                End If
                Dim PSno As String = objGPack.GetSqlValue("SELECT PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = '" & gridView_OWN.CurrentRow.Cells("BATCHNO").Value.ToString & "'")
                Dim NewAccode As String = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbAcName.Text & "'")
                Dim Multiple As Boolean = False
                If Val(objGPack.GetSqlValue(" SELECT COUNT(*) FROM " & cnAdminDb & "..CUSTOMERINFO WHERE PSNO = '" & PSno & "'")) > 1 Then
                    Select Case MessageBox.Show("It will affect multiple bills" + vbCrLf + "Do you want to update all the previous bills?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
                        Case Windows.Forms.DialogResult.Yes
                            Multiple = True
                        Case Else
                            Multiple = False
                            'gridView_OWN.Select()
                            'Exit Sub
                    End Select
                End If
                tran = Nothing
                tran = cn.BeginTransaction
                If Multiple Then
                    strSql = vbCrLf + " UPDATE " & cnAdminDb & "..PERSONALINFO SET "
                    strSql += vbCrLf + " ACCODE = H.ACCODE,TITLE = H.TITLE,INITIAL = H.INITIAL,PNAME = H.ACNAME,DOORNO = H.DOORNO"
                    strSql += vbCrLf + " ,ADDRESS1 = H.ADDRESS1,ADDRESS2 = H.ADDRESS2,ADDRESS3 = H.ADDRESS3"
                    strSql += vbCrLf + " ,AREA = H.AREA,CITY = H.CITY,STATE = H.STATE,COUNTRY = H.COUNTRY"
                    strSql += vbCrLf + " ,PINCODE = H.PINCODE,PHONERES = H.PHONENO,MOBILE = H.MOBILE,EMAIL = H.EMAILID,FAX = H.FAX,PREVILEGEID = H.PREVILEGEID"
                    strSql += vbCrLf + " FROM " & cnAdminDb & "..PERSONALINFO AS P," & cnAdminDb & "..ACHEAD AS H "
                    strSql += vbCrLf + " WHERE P.SNO = '" & PSno & "'"
                    strSql += vbCrLf + " AND H.ACCODE = '" & NewAccode & "'"

                    strSql += vbCrLf + " UPDATE " & cnAdminDb & "..OUTSTANDING SET ACCODE = '" & NewAccode & "'"
                    strSql += vbCrLf + " WHERE BATCHNO IN (SELECT BATCHNO FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO = '" & gridView_OWN.CurrentRow.Cells("RUNNO").Value.ToString & "' AND ISNULL(CANCEL,'') = '')"
                    strSql += vbCrLf + " AND ACCODE = '" & gridView_OWN.CurrentRow.Cells("ACCODE").Value.ToString & "'"
                    strSql += vbCrLf + " UPDATE " & cnStockDb & "..ACCTRAN SET ACCODE = '" & NewAccode & "'"
                    strSql += vbCrLf + " WHERE BATCHNO IN (SELECT BATCHNO FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO = '" & gridView_OWN.CurrentRow.Cells("RUNNO").Value.ToString & "' AND ISNULL(CANCEL,'') = '')"
                    strSql += vbCrLf + " AND ACCODE = '" & gridView_OWN.CurrentRow.Cells("ACCODE").Value.ToString & "'"
                    strSql += vbCrLf + " UPDATE " & cnStockDb & "..ACCTRAN SET CONTRA = '" & NewAccode & "'"
                    strSql += vbCrLf + " WHERE BATCHNO IN (SELECT BATCHNO FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO = '" & gridView_OWN.CurrentRow.Cells("RUNNO").Value.ToString & "' AND ISNULL(CANCEL,'') = '')"
                    strSql += vbCrLf + " AND CONTRA = '" & gridView_OWN.CurrentRow.Cells("ACCODE").Value.ToString & "'"
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("COSTID").Value.ToString)
                Else
                    Dim NEWPSNO As String = objGPack.GetSqlValue("SELECT SNO FROM " & cnAdminDb & "..PERSONALINFO WHERE ACCODE = '" & NewAccode & "'", , , tran)
                    If NEWPSNO = "" Then
GENNEWPSNO:
                        strSql = " SELECT * FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE='" & NewAccode & "'"
                        cmd = New OleDbCommand(strSql, cn, tran)
                        Dim dtacchead As New DataTable
                        da = New OleDbDataAdapter(cmd)
                        da.Fill(dtacchead)
                        If dtacchead.Rows.Count > 0 Then
                            NEWPSNO = GetPersonalInfoSno(tran)
                            strSql = " INSERT INTO " & cnAdminDb & "..PERSONALINFO"
                            strSql += " ("
                            strSql += " SNO,ACCODE,TRANDATE,TITLE"
                            strSql += " ,INITIAL,PNAME,DOORNO,ADDRESS1"
                            strSql += " ,ADDRESS2,ADDRESS3,AREA,CITY"
                            strSql += " ,STATE,COUNTRY,PINCODE,PHONERES"
                            strSql += " ,MOBILE,EMAIL,FAX,APPVER"
                            strSql += " ,PREVILEGEID,COMPANYID,COSTID,PAN"
                            strSql += " )"
                            strSql += " VALUES"
                            strSql += " ("
                            strSql += " '" & NEWPSNO & "'" ''SNO
                            strSql += " ,'" & dtacchead.Rows(0).Item("ACCODE").ToString & "'" 'ACCODE
                            strSql += " ,'" & Format(gridView_OWN.CurrentRow.Cells("TRANDATE").Value, "yyyy-MM-dd") & "'" 'TRANDATE
                            strSql += " ,'" & dtacchead.Rows(0).Item("TITLE").ToString & "'" 'TITLE
                            strSql += " ,'" & dtacchead.Rows(0).Item("INITIAL").ToString & "'" 'INITIAL
                            strSql += " ,'" & dtacchead.Rows(0).Item("ACNAME").ToString & "'" 'PNAME
                            strSql += " ,'" & dtacchead.Rows(0).Item("DOORNO").ToString & "'" 'DOORNO
                            strSql += " ,'" & dtacchead.Rows(0).Item("ADDRESS1").ToString & "'" 'ADDRESS1
                            strSql += " ,'" & dtacchead.Rows(0).Item("ADDRESS2").ToString & "'" 'ADDRESS2
                            strSql += " ,'" & dtacchead.Rows(0).Item("ADDRESS3").ToString & "'" 'ADDRESS3
                            strSql += " ,'" & dtacchead.Rows(0).Item("AREA").ToString & "'" 'AREA
                            strSql += " ,'" & dtacchead.Rows(0).Item("CITY").ToString & "'" 'CITY
                            strSql += " ,'" & dtacchead.Rows(0).Item("STATE").ToString & "'" 'STATE
                            strSql += " ,'" & dtacchead.Rows(0).Item("COUNTRY").ToString & "'" 'COUNTRY
                            strSql += " ,'" & dtacchead.Rows(0).Item("PINCODE").ToString & "'" 'PINCODE
                            strSql += " ,'" & dtacchead.Rows(0).Item("PHONENO").ToString & "'" 'PHONERES
                            strSql += " ,'" & dtacchead.Rows(0).Item("MOBILE").ToString & "'" 'MOBILE
                            strSql += " ,'" & dtacchead.Rows(0).Item("EMAILID").ToString & "'" 'EMAIL
                            strSql += " ,'" & dtacchead.Rows(0).Item("FAX").ToString & "'" 'Fax
                            strSql += " ,'" & VERSION & "'" 'APPVER
                            strSql += " ,''" 'PREVILEGEID
                            strSql += " ,'" & strCompanyId & "'" 'COMPANYID
                            strSql += " ,'" & gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("COSTID").Value.ToString & "'" 'COSTID
                            strSql += " ,'" & dtacchead.Rows(0).Item("PAN").ToString & "'" 'PAN
                            strSql += " )"
                            ExecQuery(SyncMode.Transaction, strSql, cn, tran, gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("COSTID").Value.ToString)
                        End If
                    End If
                    If NEWPSNO = "" Then GoTo GENNEWPSNO
                    strSql = vbCrLf + " UPDATE " & cnAdminDb & "..CUSTOMERINFO  SET PSNO = '" & NEWPSNO & "'"
                    strSql += vbCrLf + " WHERE BATCHNO = '" & gridView_OWN.CurrentRow.Cells("BATCHNO").Value.ToString & "'"

                    strSql += vbCrLf + " UPDATE " & cnAdminDb & "..OUTSTANDING SET ACCODE = '" & NewAccode & "'"
                    strSql += vbCrLf + " WHERE TRANDATE = '" & gridView_OWN.CurrentRow.Cells("TRANDATE").Value & "'"
                    strSql += vbCrLf + " AND BATCHNO = '" & gridView_OWN.CurrentRow.Cells("BATCHNO").Value.ToString & "'"
                    strSql += vbCrLf + " AND ACCODE = '" & gridView_OWN.CurrentRow.Cells("ACCODE").Value.ToString & "'"

                    strSql += vbCrLf + " UPDATE " & cnStockDb & "..ACCTRAN SET ACCODE = '" & NewAccode & "'"
                    strSql += vbCrLf + " WHERE TRANDATE = '" & gridView_OWN.CurrentRow.Cells("TRANDATE").Value & "'"
                    strSql += vbCrLf + " AND BATCHNO = '" & gridView_OWN.CurrentRow.Cells("BATCHNO").Value.ToString & "'"
                    strSql += vbCrLf + " AND ACCODE = '" & gridView_OWN.CurrentRow.Cells("ACCODE").Value.ToString & "'"

                    strSql += vbCrLf + " UPDATE " & cnStockDb & "..ACCTRAN SET CONTRA = '" & NewAccode & "'"
                    strSql += vbCrLf + " WHERE TRANDATE = '" & gridView_OWN.CurrentRow.Cells("TRANDATE").Value & "'"
                    strSql += vbCrLf + " AND BATCHNO = '" & gridView_OWN.CurrentRow.Cells("BATCHNO").Value.ToString & "'"
                    strSql += vbCrLf + " AND CONTRA = '" & gridView_OWN.CurrentRow.Cells("ACCODE").Value.ToString & "'"
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("COSTID").Value.ToString)
                End If


                gridView_OWN.CurrentCell.Value = cmbAcName.Text
                cmbAcName.Visible = False
                tran.Commit()
                tran = Nothing
                If Multiple Then
                    btnSearch_Click(Me, New EventArgs)
                End If
            Catch ex As Exception
                If Not tran Is Nothing Then tran.Rollback()
                MsgBox(ex.Message + vbCrLf + ex.StackTrace)
            End Try
            gridView_OWN.Focus()
        End If
    End Sub
    Public Shared Function GetPersonalInfoSno(ByVal tran1 As OleDbTransaction) As String
GETNSNO:
        Dim tSno As Integer = 0
        Dim strSql As String
        Dim cmd As OleDbCommand
        strSql = " SELECT CTLTEXT AS TAGSNO FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'PERSONALINFOCODE'"
        tSno = Val(objGPack.GetSqlValue(strSql, , , tran1))
        ''UPDATING 
        ''TAGNO INTO SOFTCONTROL
        strSql = " UPDATE " & cnAdminDb & "..SOFTCONTROL SET CTLTEXT = '" & tSno + 1 & "' "
        strSql += " WHERE CTLID = 'PERSONALINFOCODE' AND CONVERT(INT,CTLTEXT) = " & tSno & ""
        cmd = New OleDbCommand(strSql, cn, tran1)
        If cmd.ExecuteNonQuery() = 0 Then
            GoTo GETNSNO
        End If
        strSql = " SELECT SNO FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = '" & cnCostId & (tSno + 1).ToString & "'"
        If objGPack.GetSqlValue(strSql, , "-1", tran1) <> "-1" Then
            GoTo GETNSNO
        End If
        Return cnCostId & (tSno + 1).ToString
    End Function


    Private Sub cmbAcName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbAcName.KeyPress
      
    End Sub

    Private Sub cmbAcName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAcName.LostFocus
        cmbAcName.Visible = False
    End Sub
    Private Sub FindSearchToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FindSearchToolStripMenuItem.Click
        If Not gridView_OWN.RowCount > 0 Then
            Exit Sub
        End If
        If Not gridView_OWN.RowCount > 0 Then Exit Sub
        Dim objSearch As New frmGridSearch(gridView_OWN, gridView_OWN.Columns("ACNAME").Index)
        objSearch.ShowDialog()
    End Sub

End Class