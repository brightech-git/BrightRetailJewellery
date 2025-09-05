Imports System.Data.OleDb


Public Class frmCostcentreSelection
    Dim StrSql As String = Nothing
    Dim dt As New DataTable
    Dim da As New OleDbDataAdapter
    Dim cms As New OleDbCommand
    Public Usercostids As String = ""

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Function funcLoadCostcentre() As Integer
        Try
            Me.Text = "Costcentre Selection "
            StrSql = "SELECT COSTNAME,COSTID,CASE WHEN ISNULL(ACTIVE,'Y')='Y' THEN 'YES' ELSE'NO' END AS ACTIVE FROM " & cnAdminDb & "..COSTCENTRE "
            StrSql += " WHERE 1=1"
            If Usercostids <> "" Then StrSql += " AND COSTID IN('" & Replace(Usercostids, ",", "','") & "')"
            If strCompanyId <> "" Then StrSql += " AND (ISNULL(COMPANYID,'')='' OR ISNULL(COMPANYID,'') LIKE '%" & strCompanyId & "%')"
            StrSql += " AND ISNULL(ACTIVE,'Y')<>'N'"
            StrSql += " ORDER BY DISPORDER"
            da = New OleDbDataAdapter(StrSql, cn)
            dt = New DataTable
            da.Fill(dt)
            If Not dt.Rows.Count > 0 Then
                MsgBox("CoostCentre Details Not Available" & vbCrLf & "Contact System Administrator", MsgBoxStyle.Information)
                Me.Close()
            End If
            gridView.DataSource = dt
            gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
            funcGridStyle()
            gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            gridView.Refresh()
            gridView.Focus()

        Catch ex As Exception
            MsgBox("ERROR : " & ex.Message & " POSITION : " & ex.StackTrace)
        End Try
    End Function

    Function funcGridStyle() As Integer
        With gridView
            .RowHeadersVisible = False
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            With .Columns("COSTID")
                .SortMode = DataGridViewColumnSortMode.NotSortable
                '.Width = 120
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .Visible = False
            End With
            With .Columns("COSTNAME")
                .HeaderText = "COSTCENTRE NAME"
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .MinimumWidth = 350
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            End With
            .Columns("ACTIVE").Visible = False
        End With
    End Function

    Private Sub gridView_DataBindingComplete(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewBindingCompleteEventArgs) Handles gridView.DataBindingComplete
        gridView.CurrentCell = Nothing
        For cnt As Integer = 0 To gridView.RowCount - 1
            If gridView.Rows(cnt).Cells("COSTID").Value.ToString = strBCostid Then
                If gridView.Rows(cnt).Visible Then
                    gridView.CurrentCell = gridView.Rows(cnt).Cells("COSTNAME")
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
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            strBCostid = ""
            If gridView.CurrentCell Is Nothing Then
                strBCostid = cnCostId  '_MainCompId
                BrighttechREPORT.Globalvariables.strBCostid = strBCostid
                Me.Close()
                Exit Sub
            End If
            strBCostid = gridView.CurrentRow.Cells("COSTID").Value.ToString
            CompanyStateId = Val(objGPack.GetSqlValue("SELECT STATEID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = '" & strBCostid & "'").ToString)
            CompanyState = objGPack.GetSqlValue("SELECT STATENAME FROM " & cnAdminDb & "..STATEMAST WHERE STATEID = '" & CompanyStateId & "'").ToString
            BrighttechREPORT.Globalvariables.CompanyState = CompanyState
            Me.Close()
        ElseIf e.KeyChar = Chr(Keys.Escape) Then
            MsgBox("Please select working Cost centre", MsgBoxStyle.Critical)
            'Me.Close() : Exit Sub
        End If
    End Sub

    Private Sub frmCostcentreSelection_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        gridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        funcLoadCostcentre()
    End Sub
End Class