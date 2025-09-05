Imports System.Data.OleDb
Public Class frmAccountType
#Region "Variable Name"
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Dim dt As DataTable
    Dim strSql As String = Nothing
    Dim flag As Boolean = True
#End Region

#Region "Form Event"
    Private Sub frmAccountType_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        btnNew_Click(Me, New System.EventArgs)
    End Sub
#End Region

#Region "Button Event"
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click, SaveToolStripMenuItem.Click
        If flag = True Then
            'Insert

            If txtTypeId.Text = "" Then
                MsgBox("Invalid Id", MsgBoxStyle.Information)
                txtTypeId.Focus()
                Exit Sub
            End If

            If objGPack.GetSqlValue("SELECT 'CHECK' FROM " & cnAdminDb & "..ACCTYPE WHERE TYPEID = '" & txtTypeId.Text & "'").Length > 0 Then
                MsgBox("TypeId Already Exist", MsgBoxStyle.Information)
                txtTypeId.Focus()
                Exit Sub
            End If

            If objGPack.GetSqlValue("SELECT 'CHECK' FROM " & cnAdminDb & "..ACCTYPE WHERE TYPENAME = '" & txtTypeName.Text & "'").Length > 0 Then
                MsgBox("Type Name Already Exist", MsgBoxStyle.Information)
                txtTypeName.Focus()
                Exit Sub
            End If
            If txtTypeName.Text = "" Then
                MsgBox("Invalid Type Name", MsgBoxStyle.Information)
                txtTypeName.Focus()
                Exit Sub
            End If

            If cmbType.Text = "" Then
                MsgBox("Type Should Not Empty")
                cmbType.Focus()
                Exit Sub
            End If

            If objGPack.GetSqlValue("SELECT TYPENAME FROM " & cnAdminDb & "..ACCTYPE WHERE TYPEID = ACTYPE AND TYPENAME = '" & cmbType.Text & "'").Length = 0 Then
                MsgBox("Invalid Type Selected", MsgBoxStyle.Information)
                cmbType.Focus()
                Exit Sub
            End If

            If txtDisOrder_NUM.Text = "" Then
                MsgBox("DisOrder Should not Empty")
                txtDisOrder_NUM.Focus()
                Exit Sub
            End If
            If objGPack.GetSqlValue("SELECT 'CHECK' FROM " & cnAdminDb & "..ACCTYPE WHERE DISORDER = '" & txtDisOrder_NUM.Text & "'").Length > 0 Then
                MsgBox("Disorder Already Exist", MsgBoxStyle.Information)
                txtDisOrder_NUM.Focus()
                Exit Sub
            End If

            Try
                funcAdd()
                funcNew()
            Catch ex As Exception
                MessageBox.Show(ex.ToString)
            End Try

        Else
            'UPDATED QUERY
            If txtTypeName.Text = "" Then
                MsgBox("TypeName should not empty", MsgBoxStyle.Information)
                txtTypeName.Focus()
                Exit Sub
            End If

            If cmbType.Text = "" Then
                MsgBox("Type Should Not Empty")
                cmbType.Focus()
                Exit Sub
            End If

            If objGPack.GetSqlValue("SELECT TYPENAME FROM " & cnAdminDb & "..ACCTYPE WHERE TYPEID = ACTYPE AND TYPENAME = '" & cmbType.Text & "'").Length = 0 Then
                MsgBox("Invalid Type Selected", MsgBoxStyle.Information)
                cmbType.Focus()
                Exit Sub
            End If

            If txtDisOrder_NUM.Text = "" Then
                MsgBox("DisOrder Should not Empty")
                txtDisOrder_NUM.Focus()
                Exit Sub
            End If

            Try

                funcUpdate()
                funcNew()
            Catch ex As Exception
                MessageBox.Show(ex.ToString)
            End Try

        End If
    End Sub

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click, GridToolStripMenuItem.Click
        funcGridview()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        funcNew()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        funcExit()
    End Sub
    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "HEAD OF ACCOUNT TYPE", gridView, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

#End Region

#Region "UserDefine Function"

    Private Sub funcGridview()
        gridView.DataSource = Nothing

        'strSql = "SELECT TYPEID,TYPENAME,ACTYPE FROM " & cnAdminDb & "..ACCTYPE "
        'strSql += " WHERE TYPEID <> ACTYPE ORDER BY TYPEID"

        strSql = "SELECT TYPEID,TYPENAME,ACTYPE,DISORDER FROM " & cnAdminDb & "..ACCTYPE "
        strSql += " ORDER BY DISORDER"
        da = New OleDbDataAdapter(strSql, cn)
        dt = New DataTable
        da.Fill(dt)
        gridView.DataSource = dt
        If gridView.ColumnCount > 0 Then
            gridView.Columns("TYPEID").Width = 100
            gridView.Columns("TYPENAME").Width = 170
            gridView.Columns("ACTYPE").Width = 100
            gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            gridView.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 9, FontStyle.Bold)
            gridView.Focus()
        End If

        For cnt As Integer = 0 To gridView.ColumnCount - 1
            gridView.Columns(cnt).SortMode = DataGridViewColumnSortMode.NotSortable
        Next

    End Sub

    Private Sub funcAdd()
        Dim typeString As String

        strSql = "INSERT INTO " & cnAdminDb & "..ACCTYPE(TYPEID,TYPENAME,ACTYPE,DISORDER) VALUES "
        strSql += " ('" & txtTypeId.Text & "'," 'TypeId
        strSql += " '" & txtTypeName.Text & "'," ' TypeName

        If cmbType.Text = "SMITH" Then
            strSql += " 'G', " ' Type
        ElseIf cmbType.Text = "CASH" Then
            strSql += " 'H',"
        Else
            strSql += " '" & cmbType.Text.Substring(0, 1) & "', " ' Type
        End If

        strSql += " '" & txtDisOrder_NUM.Text & "')" 'DisOrder
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()
        MsgBox("Saved", MsgBoxStyle.Information)
    End Sub


    Private Sub funcUpdate()
        strSql = "UPDATE " & cnAdminDb & "..ACCTYPE SET "
        strSql += " TYPEID = '" & txtTypeId.Text & "' " ' TypeId
        strSql += " ,TYPENAME = '" & txtTypeName.Text & "'" ' TypeName

        If cmbType.Text = "SMITH" Then
            strSql += " ,ACTYPE = 'G'" 'Type
        ElseIf cmbType.Text = "CASH" Then
            strSql += " ,ACTYPE = 'H'"
        Else
            strSql += " ,ACTYPE = '" & cmbType.Text.Substring(0, 1) & "'" 'Type
        End If
        strSql += "  ,DISORDER = '" & txtDisOrder_NUM.Text & "' " ' Disorder
        strSql += " WHERE TYPEID = '" & txtTypeId.Text & "'"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()
        MsgBox("Updated", MsgBoxStyle.Information)
    End Sub

    Private Sub funcExit()
        Me.Close()
    End Sub

    Private Sub funcNew()
        txtTypeId.Focus()
        txtTypeName.Text = ""
        txtTypeId.Text = ""
        txtDisOrder_NUM.Text = ""
        cmbType.Items.Clear()
        strSql = "SELECT TYPENAME FROM " & cnAdminDb & "..ACCTYPE WHERE TYPEID = ACTYPE"
        objGPack.FillCombo(strSql, cmbType)
        funcGridview()
        txtTypeId.Enabled = True
        txtTypeId.Focus()
        txtDisOrder_NUM.Enabled = True
        txtTypeId.CharacterCasing = CharacterCasing.Upper
        flag = True
    End Sub

    Function funcgetDetails(ByVal TypeId As String)
        Dim dt As New DataTable
        dt.Clear()

        strSql = "SELECT TYPEID,TYPENAME,ACTYPE,DISORDER FROM " & cnAdminDb & "..ACCTYPE "
        strSql += " WHERE TYPEID = '" & TypeId & "'"

        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Return 0
        End If
        With dt.Rows(0)
            txtTypeId.Text = .Item("TYPEID").ToString
            txtTypeName.Text = .Item("TYPENAME").ToString
            'Dim strType As String = .Item("ACTYPE").ToString
            'cmbType.Text = objGPack.GetSqlValue("SELECT TYPENAME FROM " & cnAdminDb & "..ACCTYPE WHERE ACTYPE = '" & strType & "'")
            Select Case .Item("ACTYPE").ToString
                Case "G"
                    cmbType.Text = "SMITH"
                Case "C"
                    cmbType.Text = "CUSTOMER"
                Case "D"
                    cmbType.Text = "DEALER"
                Case "B"
                    cmbType.Text = "BANK"
                Case "H"
                    cmbType.Text = "CASH"
                Case "I"
                    cmbType.Text = "INTERAL"
                Case "O"
                    cmbType.Text = "OTHER"
            End Select
            txtDisOrder_NUM.Text = .Item("DISORDER").ToString

        End With
        flag = False
        txtTypeId.Enabled = False
        'txtTypeName.Focus()
        txtTypeName.Select()
        Return 0
    End Function

#End Region

#Region "GridView"
    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            If gridView.RowCount > 0 Then
                e.Handled = True
                gridView.CurrentCell = gridView.CurrentCell

                strSql = "SELECT TYPENAME FROM " & cnAdminDb & "..ACCTYPE "
                strSql += " WHERE TYPEID = ACTYPE AND "
                strSql += " TYPENAME = '" & gridView.Item("TYPENAME", gridView.CurrentRow.Index).Value.ToString & "' "
                strSql += "AND TYPEID = '" & gridView.Item("TYPEID", gridView.CurrentRow.Index).Value.ToString & "' "

                If objGPack.GetSqlValue(strSql).Length > 0 Then
                    MsgBox("Should Not Editable", MsgBoxStyle.Information)
                    Exit Sub
                End If
                funcgetDetails(gridView.Item("TYPEID", gridView.CurrentRow.Index).Value.ToString)
            End If
        End If
    End Sub
#End Region

#Region "TextBox Event"

    Private Sub txtTypeName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtTypeName.KeyDown
        If e.KeyCode = Keys.Enter Then
            If txtTypeName.Text = "" Then
                MsgBox("Invalid Name", MsgBoxStyle.Information)
                txtTypeName.Focus()
                Exit Sub
            End If
            If flag = True Then
                If objGPack.GetSqlValue("SELECT 'CHECK' FROM " & cnAdminDb & "..ACCTYPE WHERE TYPENAME = '" & txtTypeName.Text & "'").Length > 0 Then
                    MsgBox("Type Name Already Exist", MsgBoxStyle.Information)
                    txtTypeName.Focus()
                    Exit Sub
                End If
            End If
        End If
        If e.KeyCode = Keys.Enter Then
            cmbType.Focus()
        End If
    End Sub

    Private Sub txtTypeId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtTypeId.KeyDown

        If e.KeyCode = Keys.Enter Then
            If txtTypeId.Text = "" Then
                MsgBox("Invalid Id", MsgBoxStyle.Information)
                Exit Sub
            End If

            If flag = True Then
                If objGPack.GetSqlValue("SELECT 'CHECK' FROM " & cnAdminDb & "..ACCTYPE WHERE TYPEID = '" & txtTypeId.Text & "'").Length > 0 Then
                    MsgBox("TypeId Already Exist", MsgBoxStyle.Information)
                    txtTypeId.Focus()
                    Exit Sub
                End If
            End If
        End If
        If e.KeyCode = Keys.Enter Then
            txtTypeName.Focus()
        End If
    End Sub

    Private Sub txtDisOrder_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtDisOrder_NUM.KeyDown
        If e.KeyCode = Keys.Enter Then
            If txtDisOrder_NUM.Text = "" Then
                MsgBox("DisOrder Should not Empty")
                txtDisOrder_NUM.Focus()
                Exit Sub
            End If
            If flag = True Then
                'INSERT CHECKING
                If objGPack.GetSqlValue("SELECT 'CHECK' FROM " & cnAdminDb & "..ACCTYPE WHERE DISORDER = '" & txtDisOrder_NUM.Text & "'").Length > 0 Then
                    MsgBox("Disorder Already Exist", MsgBoxStyle.Information)
                    txtDisOrder_NUM.Focus()
                    Exit Sub
                End If
            End If
            btnSave.Focus()
        End If
    End Sub

#End Region

#Region "ComboxEvent"
    Private Sub cmbType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbType.KeyDown

        If e.KeyCode = Keys.Enter Then
            If cmbType.Text = "" Then
                MsgBox("Type Should Not Empty")
                Exit Sub
            End If

            If objGPack.GetSqlValue("SELECT TYPENAME FROM " & cnAdminDb & "..ACCTYPE WHERE TYPEID = ACTYPE AND TYPENAME = '" & cmbType.Text & "'").Length = 0 Then
                MsgBox("Invalid Type Selected", MsgBoxStyle.Information)
                cmbType.Focus()
                Exit Sub
            End If
        End If
        If e.KeyCode = Keys.Enter Then
            txtDisOrder_NUM.Focus()
        End If
    End Sub
#End Region


End Class