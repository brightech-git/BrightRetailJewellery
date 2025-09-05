Imports System.Data.OleDb
Imports System.IO
Public Class ReSend_Master
    Dim StrSql As String
    Dim Da As OleDbDataAdapter
    Dim Cmd As OleDbCommand
    Dim Tablename As New List(Of String)
    Private _Cn As OleDbConnection
    Private _Tran As OleDbTransaction = Nothing
    Dim Syncdb As String = cnAdminDb
    Dim Qry As String = ""

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub ReSend_Master_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub ReSend_Master_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me._Tran = tran
        Me._Cn = cn
        Dim uprefix As String = Mid(cnAdminDb, 1, 3)
        If GetAdmindbSoftValue("SYNC-SEPDATADB", "N", tran) = "Y" Then
            Dim usuffix As String = "UTILDB"
            If DbChecker(uprefix + usuffix) <> 0 Then Syncdb = uprefix + usuffix
        End If
        Dim SenderId As String = objGPack.GetSqlValue(" SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COSTID'").ToUpper
        If objGPack.GetSqlValue(" SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'SYNC-TO'").ToUpper = "" Then
            StrSql = " SELECT COSTID FROM " & cnAdminDb & "..SYNCCOSTCENTRE "
            StrSql += " WHERE COSTID <> '" & SenderId & "' AND ISNULL(ACTIVE,'')<>'N'"
            objGPack.FillCombo(StrSql, cmbCostId)
            cmbCostId.Enabled = True
            btnSend.Enabled = True
        Else
            cmbCostId.Enabled = False
            btnSend.Enabled = False
            MsgBox("Master Cannot Transfer from this Costcentre", MsgBoxStyle.Information)
        End If


        If cmbCostId.Items.Count > 0 Then
            cmbCostId.SelectedIndex = 0
        End If

        Dim dtGridView As New DataTable
        Dim dtCol As DataColumn
        dtCol = New DataColumn("CHECK", GetType(Boolean))
        dtCol.DefaultValue = False
        dtGridView.Columns.Add(dtCol)
        StrSql = " SELECT NAME AS TABLENAME FROM " & cnAdminDb & "..SYSOBJECTS WHERE XTYPE='U' AND NAME NOT LIKE'TEMP%'  AND NAME NOT LIKE'%TAG%' AND NAME NOT LIKE'OR%' AND NAME NOT LIKE'OUTST%' "
        StrSql += " AND NAME IN (SELECT TABLENAME FROM " & cnAdminDb & "..SYNCMAST WHERE SYNC='Y') AND NAME NOT IN ('ACHEAD')"
        StrSql += " ORDER BY NAME "
        Cmd = New OleDbCommand(StrSql, cn)
        Da = New OleDbDataAdapter(Cmd)
        Da.Fill(dtGridView)
        gridView_OWN.DataSource = dtGridView
        gridView_OWN.Columns("CHECK").Width = 60
        gridView_OWN.Columns("TABLENAME").Width = 250
        gridView_OWN.Columns("CHECK").ReadOnly = False
        cmbCostId.Focus()
    End Sub

    Private Sub btnSend_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSend.Click

        If cmbCostId.Text = "" Then
            MsgBox("Cost Id should not empty", MsgBoxStyle.Information)
            cmbCostId.Focus()
            Exit Sub
        End If
        Dim frmCostId As String = objGPack.GetSqlValue(" SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COSTID'").ToUpper
        Dim toCostId As String = cmbCostId.Text


        Try
            _Tran = Nothing
            _Tran = cn.BeginTransaction
            For i As Integer = 0 To gridView_OWN.Rows.Count - 1
                If (gridView_OWN.Rows(i).Cells("CHECK").Value) = True Then
                    If Val(objGPack.GetSqlValue("SELECT COUNT(*) FROM " & cnAdminDb & ".." & gridView_OWN.Rows(i).Cells("TABLENAME").Value.ToString & "", , "0", _Tran)) > 0 Then
                        Dim SelCostid As Boolean = False
                        If ChkSelcostid.Checked Then
                            StrSql = " SELECT 1 FROM " & cnAdminDb & "..SYSCOLUMNS  WHERE ID IN(SELECT ID FROM " & cnAdminDb & "..SYSOBJECTS WHERE XTYPE='U' AND NAME='" & gridView_OWN.Rows(i).Cells("TABLENAME").Value.ToString & "' ) AND NAME='COSTID'"
                            If objGPack.GetSqlValue(StrSql, , "0", _Tran) = "1" Then
                                SelCostid = True
                            End If
                        End If

                        StrSql = vbCrLf + "DELETE FROM " & cnAdminDb & ".." & gridView_OWN.Rows(i).Cells("TABLENAME").Value.ToString & " "
                        If SelCostid = True Then StrSql += " WHERE COSTID='" & toCostId & "'"
                        Qry = vbCrLf + " INSERT INTO " & Syncdb & "..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS,UPDFILE)"
                        Qry += vbCrLf + " SELECT '" & frmCostId & "','" & toCostId & "','" & StrSql.Replace("'", "''") & "','N' AS STATUS,'' UPDFILE"
                        Cmd = New OleDbCommand(Qry, _Cn, _Tran)
                        Cmd.ExecuteNonQuery()

                        InsertQry(cnAdminDb, gridView_OWN.Rows(i).Cells("TABLENAME").Value.ToString, frmCostId, toCostId)
                    End If
                End If
            Next
            _Tran.Commit()
            _Tran = Nothing

            MsgBox("Resend Master Completed", MsgBoxStyle.Information)
            cmbCostId.SelectedIndex = 0
            ChkAll.Checked = False
        Catch ex As Exception
            If _Tran IsNot Nothing Then _Tran.Rollback() : _Tran = Nothing
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try

    End Sub

    Private Sub InsertQry(ByVal dbname As String, ByVal TableName As String, ByVal FromCostId As String, ByVal ToCostId As String)
        Dim Qry As String = Nothing
        Dim dtTemp As New DataTable
        Dim SelCostid As Boolean = False
        If ChkSelcostid.Checked Then
            StrSql = " SELECT 1 FROM " & dbname & "..SYSCOLUMNS  WHERE ID IN(SELECT ID FROM " & dbname & "..SYSOBJECTS WHERE XTYPE='U' AND NAME='" & TableName & "' ) AND NAME='COSTID'"
            If objGPack.GetSqlValue(StrSql, , "0", _Tran) = "1" Then
                SelCostid = True
            End If
        End If
        Dim Colname As String = ""
        StrSql = " SELECT NAME FROM " & dbname & "..SYSCOLUMNS  WHERE ID IN(SELECT ID FROM " & dbname & "..SYSOBJECTS WHERE XTYPE='U' AND NAME='" & TableName & "' ) "
        StrSql += vbCrLf + " AND COLSTAT<>1 ORDER BY COLID "
        Dim dtColname As New DataTable
        Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
        Da = New OleDbDataAdapter(Cmd)
        Da.Fill(dtColname)
        If dtColname.Rows.Count > 0 Then
            For i As Integer = 0 To dtColname.Rows.Count - 1
                Colname += "," & dtColname.Rows(i).Item("NAME").ToString
            Next
            If Colname <> "" Then Colname = Mid(Colname, 2, Len(Colname))
        End If

        StrSql = " IF OBJECT_ID('" & cnStockDb & "..INS_" & TableName & "', 'U') IS NOT NULL DROP TABLE " & cnStockDb & "..INS_" & TableName & ""
        If Colname <> "" Then
            StrSql += vbCrLf + " SELECT " & Colname & " INTO " & cnStockDb & "..INS_" & TableName & " FROM " & dbname & ".." & TableName & " "
        Else
            StrSql += vbCrLf + " SELECT * INTO " & cnStockDb & "..INS_" & TableName & " FROM " & dbname & ".." & TableName & " "
        End If

        If SelCostid = True Then StrSql += vbCrLf + " WHERE COSTID='" & ToCostId & "'"
        Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
        Cmd.ExecuteNonQuery()


        Dim mtempqrytb As String = "TEMPQRYTB"
        StrSql = " EXEC " & cnStockDb & "..INSERTQRYGENERATOR_TABLENEW "
        StrSql += vbCrLf + " @DBNAME = '" & dbname & "',@TABLENAME = 'INS_" & TableName & "',@MASK_TABLENAME = '" & TableName & "',@TEMPTABLE='" & mtempqrytb & "'"
        Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
        Cmd.ExecuteNonQuery()

        StrSql = " INSERT INTO " & Syncdb & "..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS,UPDFILE)    "
        StrSql += vbCrLf + " SELECT '" & FromCostId & "','" & ToCostId & "', SQLTEXT,'N' AS STATUS,'' UPDFILE FROM " & cnStockDb & ".." & mtempqrytb
        Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
        Cmd.ExecuteNonQuery()

        StrSql = " DROP TABLE " & cnStockDb & "..INS_" & TableName & ""
        Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
        Cmd.ExecuteNonQuery()
        StrSql = " DROP TABLE " & cnStockDb & ".." & mtempqrytb & ""
        Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
        Cmd.ExecuteNonQuery()


    End Sub


    Private Sub gridView_OWN_CellContentClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView_OWN.CellContentClick
        For i As Integer = 0 To gridView_OWN.Rows.Count - 1
            If gridView_OWN.Rows(i).Cells("CHECK").Selected = True Then
                If gridView_OWN.Rows(i).Cells("CHECK").Value = False Then
                    gridView_OWN.Rows(i).Cells("CHECK").Value = True
                Else
                    gridView_OWN.Rows(i).Cells("CHECK").Value = False
                End If
            End If
        Next
    End Sub

    Private Sub ChkAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkAll.CheckedChanged
        For j As Integer = 0 To Me.gridView_OWN.RowCount - 1
            gridView_OWN.Rows(j).Cells("CHECK").Value = ChkAll.Checked
        Next
        gridView_OWN.Focus()
    End Sub
End Class