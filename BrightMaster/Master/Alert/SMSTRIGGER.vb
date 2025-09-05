
Imports System.Data.OleDb
Public Class SMSTRIGGER
    Dim StrSql As String
    Dim Cmd As OleDbCommand
    Dim da As New OleDbDataAdapter
    Dim dt As New DataTable
    Dim admincn As New OleDbConnection
    Dim trancn As New OleDbConnection
    Dim chitadmincn As New OleDbConnection
    Dim chittrancn As New OleDbConnection
    Dim Addrcn As New OleDbConnection
    Dim Ischit As String = GetAdmindbSoftValue("CHITDB", "N")
    Dim IsAddr As String = GetAdmindbSoftValue("ADDRESSDBPREFIX", "")
    Dim chitdbprefix As String = GetAdmindbSoftValue("CHITDBPREFIX", "")
    Dim chitmdb As String = chitdbprefix & "SAVINGS"
    Dim chittdb As String = chitdbprefix & "SH0708"
    Dim colCheckbox As New DataGridViewCheckBoxColumn()
    Function funcCheckDB(ByVal DbName As String, ByVal _Cn As OleDbConnection) As Boolean
        StrSql = " SELECT NAME FROM MASTER..SYSDATABASES WHERE NAME = '" & DbName & "'"
        da = New OleDbDataAdapter(StrSql, _Cn)
        dt = New DataTable
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            Return True
        End If
        Return False
    End Function
    Function funcCheckTriggerAvail(ByVal TrigName As String, ByVal _Cn As OleDbConnection) As Boolean
        StrSql = "SELECT COUNT(*) FROM SYSOBJECTS WHERE XTYPE = 'TR' AND NAME = '" & TrigName & "'"
        da = New OleDbDataAdapter(StrSql, _Cn)
        dt = New DataTable
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            Return True
        End If
        Return False
    End Function
    Function funcCheckTriggerStatus(ByVal TrigName As String, ByVal _Cn As OleDbConnection) As Boolean
        StrSql = "SELECT ISNULL(OBJECTPROPERTY(OBJECT_ID('" & TrigName & "'),'EXECISTRIGGERDISABLED'),1)"
        da = New OleDbDataAdapter(StrSql, _Cn)
        dt = New DataTable
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0).Item(0).ToString() = 1 Then
                Return False
            Else
                Return True
            End If
        End If
        Return False
    End Function
    Private Sub SMSTRIGGER_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ConInfo = New BrighttechPack.Coninfo(Application.StartupPath + "\ConInfo.ini")
        If ConInfo.lDbLoginType.ToUpper = "W" Then
            admincn = New OleDbConnection("Provider=SQLOLEDB.1;Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=" & cnAdminDb & ";Data Source=" & ConInfo.lServerName & "")
        Else
            admincn = New OleDbConnection(String.Format("PROVIDER = SQLOLEDB.1;Persist Security Info=False; Initial Catalog=" & cnAdminDb & ";Data Source={0};User Id=" & IIf(ConInfo.lDbUserId <> "", ConInfo.lDbUserId, "SA") & ";password=" & BrighttechPack.Decrypt(ConInfo.lDbPwd) & ";", ConInfo.lServerName))
        End If
        admincn.Open()
        If ConInfo.lDbLoginType.ToUpper = "W" Then
            trancn = New OleDbConnection("Provider=SQLOLEDB.1;Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=" & cnStockDb & ";Data Source=" & ConInfo.lServerName & "")
        Else
            trancn = New OleDbConnection(String.Format("PROVIDER = SQLOLEDB.1;Persist Security Info=False; Initial Catalog=" & cnStockDb & ";Data Source={0};User Id=" & IIf(ConInfo.lDbUserId <> "", ConInfo.lDbUserId, "SA") & ";password=" & BrighttechPack.Decrypt(ConInfo.lDbPwd) & ";", ConInfo.lServerName))
        End If
        trancn.Open()
        If Ischit = "Y" Then
            If funcCheckDB(chitmdb, cn) = True Then
                If ConInfo.lDbLoginType.ToUpper = "W" Then
                    chitadmincn = New OleDbConnection("Provider=SQLOLEDB.1;Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=" & chitmdb & ";Data Source=" & ConInfo.lServerName & "")
                Else
                    chitadmincn = New OleDbConnection(String.Format("PROVIDER = SQLOLEDB.1;Persist Security Info=False; Initial Catalog=" & chitmdb & ";Data Source={0};User Id=" & IIf(ConInfo.lDbUserId <> "", ConInfo.lDbUserId, "SA") & ";password=" & BrighttechPack.Decrypt(ConInfo.lDbPwd) & ";", ConInfo.lServerName))
                End If
                chitadmincn.Open()
                If ConInfo.lDbLoginType.ToUpper = "W" Then
                    chittrancn = New OleDbConnection("Provider=SQLOLEDB.1;Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=" & chittdb & ";Data Source=" & ConInfo.lServerName & "")
                Else
                    chittrancn = New OleDbConnection(String.Format("PROVIDER = SQLOLEDB.1;Persist Security Info=False; Initial Catalog=" & chittdb & ";Data Source={0};User Id=" & IIf(ConInfo.lDbUserId <> "", ConInfo.lDbUserId, "SA") & ";password=" & BrighttechPack.Decrypt(ConInfo.lDbPwd) & ";", ConInfo.lServerName))
                End If
                chittrancn.Open()
            Else
                Ischit = "N"
            End If
        End If
        If IsAddr <> "" Then
            IsAddr = IsAddr & "ADDRESSBOOK"
            If funcCheckDB(IsAddr, cn) = True Then
                If ConInfo.lDbLoginType.ToUpper = "W" Then
                    Addrcn = New OleDbConnection("Provider=SQLOLEDB.1;Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=" & IsAddr & ";Data Source=" & ConInfo.lServerName & "")
                Else
                    Addrcn = New OleDbConnection(String.Format("PROVIDER = SQLOLEDB.1;Persist Security Info=False; Initial Catalog=" & IsAddr & ";Data Source={0};User Id=" & IIf(ConInfo.lDbUserId <> "", ConInfo.lDbUserId, "SA") & ";password=" & ConInfo.lDbPwd & ";", ConInfo.lServerName))
                End If
                Addrcn.Open()
            Else
                IsAddr = ""
            End If
        End If
        funcLoad()
    End Sub
    Private Sub funcLoad()
        GridView_OWN.DataSource = Nothing
        GridView_OWN.Refresh()
        StrSql = "SELECT PARTICULARS,TRIGGERNAME,DBTYPE,TABLENAME"
        StrSql += ",CONVERT(BIT,0)ENABLE FROM SMSMASTER WHERE ACTIVE='Y' ORDER BY SNO"
        da = New OleDbDataAdapter(StrSql, admincn)
        dt = New DataTable
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            With GridView_OWN
                .DataSource = Nothing
                .DataSource = dt
                For i As Integer = 0 To .Columns.Count - 1
                    .Columns(i).ReadOnly = True
                Next
                .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                .ColumnHeadersDefaultCellStyle.Font = New Font("Verdana", 8, FontStyle.Bold)
                .DefaultCellStyle.Font = New Font("Verdana", 8, FontStyle.Regular)
                .Columns("Enable").ReadOnly = False
                .Columns("DBTYPE").Visible = False
                .Columns("TABLENAME").Visible = False
                .Columns("TRIGGERNAME").Visible = False
                For J As Integer = 0 To .Rows.Count - 1
                    Dim TrigName As String = .Rows(J).Cells("TRIGGERNAME").Value.ToString
                    Dim DbType As String = .Rows(J).Cells("DBTYPE").Value.ToString
                    If DbType = "T" Then
                        .Rows(J).Cells("ENABLE").Value = IIf(funcCheckTriggerStatus(TrigName, trancn) = True, CheckState.Checked, CheckState.Unchecked)
                    ElseIf DbType = "S" Then
                        If Ischit = "Y" Then
                            .Rows(J).Cells("ENABLE").Value = IIf(funcCheckTriggerStatus(TrigName, chittrancn) = True, CheckState.Checked, CheckState.Unchecked)
                        Else
                            .Rows(J).Cells("ENABLE").ReadOnly = True
                        End If
                    ElseIf DbType = "B" Then
                        If IsAddr <> "" Then
                            .Rows(J).Cells("ENABLE").Value = IIf(funcCheckTriggerStatus(TrigName, Addrcn) = True, CheckState.Checked, CheckState.Unchecked)
                        Else
                            .Rows(J).Cells("ENABLE").ReadOnly = True
                        End If
                    Else
                        .Rows(J).Cells("ENABLE").Value = IIf(funcCheckTriggerStatus(TrigName, admincn) = True, CheckState.Checked, CheckState.Unchecked)
                    End If
                Next
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                .Invalidate()
                For Each dgvCol As DataGridViewColumn In .Columns
                    dgvCol.Width = dgvCol.Width
                    dgvCol.SortMode = DataGridViewColumnSortMode.NotSortable
                Next
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
                GridView_OWN.Refresh()
                GridView_OWN.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            End With
        End If
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        If chittrancn.State = ConnectionState.Open Then chittrancn.Close()
        If trancn.State = ConnectionState.Open Then trancn.Close()
        If admincn.State = ConnectionState.Open Then admincn.Close()
        Me.Close()
    End Sub

    Private Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        If GridView_OWN.Rows.Count = 0 Then Exit Sub
        Me.Cursor = Cursors.WaitCursor
        For J As Integer = 0 To GridView_OWN.Rows.Count - 1
            Dim TrigName As String = GridView_OWN.Rows(J).Cells("TRIGGERNAME").Value.ToString
            Dim DbType As String = GridView_OWN.Rows(J).Cells("DBTYPE").Value.ToString
            Dim TableName As String = GridView_OWN.Rows(J).Cells("TABLENAME").Value.ToString
            If DbType = "T" Then
                If funcCheckTriggerAvail(TrigName, trancn) = False Then Continue For
                If CType(GridView_OWN.Rows(J).Cells("Enable").Value, Boolean) = False Then
                    funcSetTriggerStatus(trancn, TableName, TrigName, "DISABLE")
                Else
                    funcSetTriggerStatus(trancn, TableName, TrigName, "ENABLE")
                End If
            ElseIf DbType = "S" Then
                If Ischit = "Y" Then
                    If funcCheckTriggerAvail(TrigName, chittrancn) = False Then Continue For
                    If CType(GridView_OWN.Rows(J).Cells("Enable").Value, Boolean) = False Then
                        funcSetTriggerStatus(chittrancn, TableName, TrigName, "DISABLE")
                    Else
                        funcSetTriggerStatus(chittrancn, TableName, TrigName, "ENABLE")
                    End If
                End If
            Else
                If funcCheckTriggerAvail(TrigName, admincn) = False Then Continue For
                If CType(GridView_OWN.Rows(J).Cells("Enable").Value, Boolean) = False Then
                    funcSetTriggerStatus(admincn, TableName, TrigName, "DISABLE")
                Else
                    funcSetTriggerStatus(admincn, TableName, TrigName, "ENABLE")
                End If
            End If
        Next
        MsgBox("Trigger Status Updated.", MsgBoxStyle.Information)
        funcLoad()
        Me.Cursor = Cursors.Default
    End Sub
    Private Sub funcSetTriggerStatus(ByVal CnCon As OleDbConnection, ByVal TableName As String, ByVal TrigName As String, ByVal Status As String)
        StrSql = "ALTER TABLE " & TableName & " " & Status & " TRIGGER " & TrigName
        Cmd = New OleDbCommand(StrSql, CnCon)
        Cmd.ExecuteNonQuery()
    End Sub
End Class
