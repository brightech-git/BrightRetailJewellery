Imports System.Data.OleDb


Public Class frmCompany
    Dim StrSql As String = Nothing
    Dim dt As New DataTable
    Dim da As New OleDbDataAdapter
    Dim cms As New OleDbCommand
    Public Statuschange As Boolean = False
    Dim USERWISECOMPANY As Boolean = IIf(GetAdmindbSoftValue("USERWISECOMPANY", "N") = "Y", False, False)

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Function funcLoadCompany() As Integer
        Try
            Me.Text = "Company Selection "
            If Statuschange Then
                Me.Text += "[SPACE]-Activate/Deactivate Company"
            End If
            If USERWISECOMPANY Then
                StrSql = vbCrLf & "SELECT COMPANYNAME,CASE WHEN ISNULL(SHORTKEY,'') <> '' THEN 'CTRL + ' + SHORTKEY ELSE NULL END SHORTKEY "
                StrSql += vbCrLf & ",COMPANYID,CASE WHEN ISNULL(C.ACTIVE,'Y')='Y' THEN 'YES' ELSE'NO' END AS ACTIVE FROM " & cnAdminDb & "..COMPANY C "
                StrSql += vbCrLf & "INNER JOIN " & cnAdminDb & "..USERMASTER U ON C.COMPANYID  = ISNULL( U.USERCOMPANYID,'') "
                StrSql += vbCrLf & "WHERE U.USERID = " & userId
                StrSql += vbCrLf & "ORDER BY DISPLAYORDER "
            Else
                StrSql = "SELECT COMPANYNAME,CASE WHEN ISNULL(SHORTKEY,'') <> '' THEN 'Ctrl + ' + SHORTKEY ELSE NULL END SHORTKEY,COMPANYID,case when ISNULL(ACTIVE,'Y')='Y' then 'Yes' else'NO' end AS ACTIVE FROM " & cnAdminDb & "..COMPANY ORDER BY DISPLAYORDER"
            End If

            da = New OleDbDataAdapter(StrSql, cn)
            dt = New DataTable
            da.Fill(dt)

            If Not dt.Rows.Count > 0 Then
                MsgBox("Company Details Not Available" & vbCrLf & "Contact System Administrator", MsgBoxStyle.Information)
                If USERWISECOMPANY Then Return 0
                Me.Close()
            End If
            gridView.DataSource = dt
            gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
            funcGridStyle()
            gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            gridView.Refresh()
            gridView.Focus()
            Return 1
        Catch ex As Exception
            MsgBox("ERROR : " & ex.Message & " POSITION : " & ex.StackTrace)
        End Try
    End Function

    Function funcGridStyle() As Integer
        With gridView
            .RowHeadersVisible = False
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            With .Columns("COMPANYID")
                .SortMode = DataGridViewColumnSortMode.NotSortable
                '.Width = 120
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .Visible = False
            End With
            With .Columns("SHORTKEY")
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Width = 70
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .Visible = IIf(GetAdmindbSoftValue("HIDECOMPANYSHORTCUT", "N") = "N", True, False)
            End With
            With .Columns("COMPANYNAME")
                .HeaderText = "COMPANY NAME"
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .MinimumWidth = 350
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            End With
            .Columns("ACTIVE").Visible = False
            If Statuschange Then
                With .Columns("ACTIVE")
                    .HeaderText = "Active"
                    .Visible = True
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .MinimumWidth = 20
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                End With
            End If
        End With
    End Function

    Private Sub gridView_DataBindingComplete(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewBindingCompleteEventArgs) Handles gridView.DataBindingComplete
        gridView.CurrentCell = Nothing
        For cnt As Integer = 0 To gridView.RowCount - 1
            If Mid(gridView.Rows(cnt).Cells("ACTIVE").Value.ToString.ToUpper, 1, 1) = "N" Then
                If Statuschange Then gridView.Rows(cnt).Visible = True Else gridView.Rows(cnt).Visible = False
            Else
                gridView.Rows(cnt).Visible = True
            End If
            If gridView.Rows(cnt).Cells("COMPANYID").Value.ToString = strCompanyId Then
                If gridView.Rows(cnt).Visible Then
                    gridView.CurrentCell = gridView.Rows(cnt).Cells("COMPANYNAME")
                End If
            End If
        Next
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            If gridView.CurrentCell Is Nothing Then Exit Sub
            gridView.CurrentCell = gridView.Rows(gridView.CurrentRow.Index).Cells(0)
        End If
        If e.KeyCode = Keys.Space Then

            If Statuschange Then
                e.Handled = True
                If gridView.CurrentCell Is Nothing Then Exit Sub
                If Mid(gridView.Rows(gridView.CurrentRow.Index).Cells(3).Value.ToString, 1, 1) = "Y" Then
                    If MsgBox("Deactivate the company", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                        StrSql = "update a set a.active = 'N' from " & cnAdminDb & "..company a where companyid = '" & gridView.Rows(gridView.CurrentRow.Index).Cells(2).Value.ToString & "'"
                        cms = New OleDbCommand(StrSql, cn)
                        cms.ExecuteNonQuery()
                        gridView.Rows(gridView.CurrentRow.Index).Cells(3).Value = "No"
                    End If
                Else
                    If MsgBox("Activate the company", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                        StrSql = "update a set a.active = 'Y' from " & cnAdminDb & "..company a where companyid = '" & gridView.Rows(gridView.CurrentRow.Index).Cells(2).Value.ToString & "'"
                        cms = New OleDbCommand(StrSql, cn)
                        cms.ExecuteNonQuery()
                        gridView.Rows(gridView.CurrentRow.Index).Cells(3).Value = "Yes"
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            strCompanyId = ""
            If gridView.CurrentCell Is Nothing Then
                strCompanyId = _MainCompId
                BrighttechREPORT.Globalvariables.strCompanyId = strCompanyId
                Statuschange = False
                Me.Close()
                Exit Sub
            End If

            strCompanyId = gridView.CurrentRow.Cells("COMPANYID").Value.ToString
            strCompanyName = gridView.CurrentRow.Cells("COMPANYNAME").Value.ToString
            BrighttechREPORT.Globalvariables.strCompanyName = strCompanyName
            BrighttechREPORT.Globalvariables.strCompanyId = strCompanyId
            BrighttechMaster.GlobalVariables.strCompanyId = strCompanyId
            BrighttechMaster.GlobalVariables.strCompanyName = strCompanyName

            Main.Text = gridView.CurrentRow.Cells("COMPANYNAME").Value.ToString + IIf(cnCostName <> "", "_" + cnCostName, "") + IIf(_Demo, " " & _DemoLImitDays & " Days Trial", "") + " Version : " + VERSION
            Statuschange = False
            CompanyStateId = Val(objGPack.GetSqlValue("SELECT STATEID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYID = '" & strCompanyId & "'").ToString)
            CompanyState = objGPack.GetSqlValue("SELECT STATENAME FROM " & cnAdminDb & "..STATEMAST WHERE STATEID = '" & CompanyStateId & "'").ToString
            Me.Close()
        ElseIf e.KeyChar = Chr(Keys.Escape) Then
            If Statuschange Then Statuschange = False : Me.Close() : Exit Sub
        End If
    End Sub

    Private Sub frmCompany_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        gridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        If USERWISECOMPANY And funcLoadCompany() = 0 Then Application.Restart()
    End Sub


End Class