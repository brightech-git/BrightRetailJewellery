Imports System.Data.OleDb
Public Class frmReportsrole
    Dim strSql As String
    Dim dtGridView As New DataTable
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Dim costId As String = Nothing
    Dim lastData As Object

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub frmOpeningTrailBalance_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If gridView_OWN.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub FillDatatable()


        strSql = " select A.ROLEID,A.MENUID,RM.ROLENAME,MM.MENUTEXT MENUNAME,A.VIEWDAYS from " & cnAdminDb & "..roletran A INNER JOIN " & cnAdminDb & "..ROLEMASTER RM ON A.ROLEID = RM.ROLEID "
        strSql += " INNER JOIN " & cnAdminDb & "..MENUMASTER MM ON A.MENUID = MM.MENUID WHERE  (_PRINT ='Y' or _VIEW='Y')"
        If cmbRole_OWN.Text <> "ALL" Then
            Dim ROLEID As Integer = objGPack.GetSqlValue("SELECT ROLEID FROM " & cnAdminDb & "..ROLEMASTER WHERE ROLENAME = '" & cmbRole_OWN.Text & "'")
            strSql += " AND A.ROLEID IN (" & ROLEID & ")"
        End If
        strSql += " ORDER BY MM.MENUTEXT"
        da = New OleDbDataAdapter(strSql, cn)
        dtGridView = New DataTable
        da.Fill(dtGridView)
        gridView_OWN.DataSource = dtGridView
        GridStyle(gridView_OWN)
    End Sub

    Private Sub GridStyle(ByVal dgv As DataGridView)
        With dgv
            .Columns("MENUNAME").Width = 576
            .Columns("MENUNAME").SortMode = DataGridViewColumnSortMode.NotSortable
            .Columns("MENUNAME").ReadOnly = True
            .Columns("VIEWDAYS").Width = 200
            .Columns("VIEWDAYS").SortMode = DataGridViewColumnSortMode.NotSortable
            .Columns("VIEWDAYS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("VIEWDAYS").DefaultCellStyle.Format = "#,##,###.00"
            .Columns("VIEWDAYS").ReadOnly = False
            For cnt As Integer = 0 To 1
                .Columns(cnt).Visible = False
            Next
        End With
    End Sub

    Private Sub frmOpeningTrailBalance_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        gridView_OWN.RowTemplate.Resizable = DataGridViewTriState.False
        gridView_OWN.BorderStyle = BorderStyle.None
        gridView_OWN.RowHeadersVisible = False
        gridView_OWN.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
        gridView_OWN.RowTemplate.Height = 21
        gridView_OWN.Font = New Font("VERDANA", 9, FontStyle.Regular)
        gridView_OWN.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 9, FontStyle.Bold)
        cmbRole_OWN.Items.Clear()
        cmbRole_OWN.Items.Add("ALL")
        strSql = " SELECT ROLENAME FROM " & cnAdminDb & "..ROLEMASTER ORDER BY ROLENAME"
        objGPack.FillCombo(strSql, cmbRole_OWN, False, False)
        cmbRole_OWN.Text = "ALL"


        
        gridViewHeader.ColumnHeadersVisible = False
        gridViewHeader.RowTemplate.Height = 21
        gridViewHeader.Font = New Font("VERDANA", 9, FontStyle.Regular)
        Dim dtGridViewHeader As New DataTable
        dtGridViewHeader.Columns.Add("REPORT NAME", GetType(String))
        dtGridViewHeader.Columns.Add("VIEWDAYS", GetType(Double))
        dtGridViewHeader.Rows.Add()
        dtGridViewHeader.Rows.Add()
        gridViewHeader.DataSource = dtGridViewHeader
        gridViewHeader.DefaultCellStyle.BackColor = Color.LightGoldenrodYellow
        gridViewHeader.DefaultCellStyle.SelectionBackColor = Color.LightGoldenrodYellow
        gridViewHeader.DefaultCellStyle.ForeColor = Color.Black
        gridViewHeader.DefaultCellStyle.SelectionForeColor = Color.Black
        gridViewHeader.DefaultCellStyle.Font = New Font("VERDANA", 9, FontStyle.Bold)
        '       GridStyle(gridViewHeader)
        '        gridViewHeader.Enabled = False
        lblFindHelp.Visible = False
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub SaveIntoRoletran()
        Dim rwIndex As Integer = gridView_OWN.CurrentCell.RowIndex
        Dim days As Double = 0
        Dim credit As Double = 0

        days = Val(gridView_OWN.CurrentRow.Cells("VIEWDAYS").Value.ToString)


        Try
            tran = Nothing
            tran = cn.BeginTransaction
            With gridView_OWN.Rows(rwIndex)
                ''UPDATE
                strSql = " UPDATE " & cnAdminDb & "..ROLETRAN SET"
                strSql += " VIEWDAYS= " & days
                strSql += " WHERE ROLEID = '" & .Cells("ROLEID").Value.ToString & "'"
                strSql += " AND MENUID = '" & .Cells("MENUID").Value.ToString & "'"
                'strSql += " AND COMPANYID = '" & strCompanyId & "'"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId)
            End With
            tran.Commit()

        Catch ex As Exception
            If Not tran Is Nothing Then
                tran.Rollback()
            End If
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try

    End Sub


    Private Sub gridView_CellBeginEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellCancelEventArgs) Handles gridView_OWN.CellBeginEdit
        lastData = gridView_OWN.CurrentCell.Value
    End Sub


    Private Sub gridView_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView_OWN.CellEndEdit
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
        If Not Val(gridView_OWN.CurrentCell.Value.ToString) > 0 Then
            gridView_OWN.CurrentCell.Value = DBNull.Value
        End If
        SaveIntoRoletran()
    End Sub

    Private Sub gridView_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles gridView_OWN.CellFormatting
        'Select Case gridView_OWN.Rows(e.RowIndex).Cells("RESULT").Value.ToString
        '   Case "0" 'TITLE
        gridView_OWN.Rows(e.RowIndex).DefaultCellStyle.Font = New Font("VERDANA", 10, FontStyle.Bold)
        gridView_OWN.Rows(e.RowIndex).Cells("MENUNAME").Style.BackColor = Color.LightBlue
        gridView_OWN.Rows(e.RowIndex).Cells("MENUNAME").Style.ForeColor = Color.Black
        gridView_OWN.Rows(e.RowIndex).Cells("MENUNAME").Style.SelectionBackColor = Color.LightBlue
        gridView_OWN.Rows(e.RowIndex).Cells("MENUNAME").Style.SelectionForeColor = Color.Black
        gridView_OWN.Rows(e.RowIndex).Cells("VIEWDAYS").ReadOnly = False
        gridView_OWN.Rows(e.RowIndex).Cells("MENUNAME").ReadOnly = True
        gridView_OWN.Rows(e.RowIndex).Cells("ROLENAME").ReadOnly = True
    

        'Case "1" 'TRAN
        '    Select Case gridView_OWN.Columns(e.ColumnIndex).Name
        '        Case "DEBIT"
        '            If Val(e.Value.ToString) > 0 Then
        '                gridView_OWN.Rows(e.RowIndex).Cells(e.ColumnIndex).Style.BackColor = Color.Lavender
        '            Else
        '                gridView_OWN.Rows(e.RowIndex).Cells(e.ColumnIndex).Style.BackColor = Color.White
        '            End If
        '        Case "CREDIT"
        '            If Val(e.Value.ToString) > 0 Then
        '                gridView_OWN.Rows(e.RowIndex).Cells(e.ColumnIndex).Style.BackColor = Color.LavenderBlush
        '            Else
        '                gridView_OWN.Rows(e.RowIndex).Cells(e.ColumnIndex).Style.BackColor = Color.White
        '            End If
        '    End Select
        'Case "2" 'TOTAL,
        '    gridView_OWN.Rows(e.RowIndex).ReadOnly = True
        '    gridView_OWN.Rows(e.RowIndex).DefaultCellStyle.BackColor = Color.LightGoldenrodYellow
        '    gridView_OWN.Rows(e.RowIndex).DefaultCellStyle.Font = New Font("VERDANA", 9, FontStyle.Bold)
        'Case "3" 'BALANCE
        '    gridView_OWN.Rows(e.RowIndex).ReadOnly = True
        '    gridView_OWN.Rows(e.RowIndex).DefaultCellStyle.BackColor = Color.LightGoldenrodYellow
        '    gridView_OWN.Rows(e.RowIndex).DefaultCellStyle.Font = New Font("VERDANA", 9, FontStyle.Bold)
        'End Select
    End Sub

    Private Sub gridView_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs) Handles gridView_OWN.ColumnWidthChanged
        'gridViewHeader.Columns("MENUNAME").Width = gridView_OWN.Columns("MENUNAME").Width
        'gridViewHeader.Columns("VIEWDAYS").Width = gridView_OWN.Columns("VIEWDAYS").Width

    End Sub

    Private Sub gridView_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles gridView_OWN.DataError

    End Sub

    Private Sub gridView_Own_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles gridView_OWN.EditingControlShowing
        Select Case gridView_OWN.Columns(gridView_OWN.CurrentCell.ColumnIndex).Name
            Case "VIEWDAYS"
                Dim tb As TextBox = CType(e.Control, TextBox)
                AddHandler tb.KeyPress, AddressOf txtAmt_KeyPress
        End Select
    End Sub

    Private Sub txtAmt_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs)
        If e.KeyChar = "-" Then
            e.Handled = True
            Exit Sub
        End If
        AmountValidation(sender, e)
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        ' If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        FillDatatable()
        If gridView_OWN.RowCount > 0 Then
            gridView_OWN.CurrentCell = gridView_OWN.Rows(0).Cells("VIEWDAYS")
            gridView_OWN.Focus()
        End If
    End Sub


    Private Sub cmbAcGroup_OWN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbRole_OWN.KeyPress
        e.KeyChar = UCase(e.KeyChar)
    End Sub


    Private Sub gridView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView_OWN.GotFocus
        lblFindHelp.Visible = True
    End Sub

    Private Sub gridView_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles gridView_OWN.EditingControlShowing
        Select Case gridView_OWN.Columns(gridView_OWN.CurrentCell.ColumnIndex).Name
            Case "VIEWDAYS"
                Dim tb As TextBox = CType(e.Control, TextBox)
                AddHandler tb.KeyPress, AddressOf txtAmt_KeyPress
        End Select
     
    End Sub
    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView_OWN.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
        End If
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView_OWN.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            e.Handled = True
            If gridView_OWN.RowCount > 0 Then
                gridView_OWN.CurrentCell = gridView_OWN.Rows(gridView_OWN.CurrentCell.RowIndex).Cells(gridView_OWN.CurrentCell.ColumnIndex)
            End If
        End If
    End Sub

    Private Sub btnNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNew.Click
        objGPack.TextClear(Me)

        gridView_OWN.DataSource = Nothing
        dtGridView = New DataTable
        cmbRole_OWN.Text = "ALL"
        FillDatatable()

        
    End Sub

    Private Sub cmbAcName_OWN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        e.KeyChar = UCase(e.KeyChar)
    End Sub

    
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        btnExit_Click(Me, New EventArgs)
    End Sub

    
    'Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
    '    If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
    '    If gridView_OWN.Rows.Count > 0 Then
    '        BrightPosting.GExport.Post(Me.Name, strCompanyName, "Opening TrailBalance", gridView_OWN, BrightPosting.GExport.GExportType.Print)
    '    End If
    'End Sub

    Private Sub FindToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FindToolStripMenuItem.Click
        If Not gridView_OWN.RowCount > 0 Then
            Exit Sub
        End If
        If Not gridView_OWN.RowCount > 0 Then Exit Sub
        Dim objSearch As New frmGridSearch(gridView_OWN)
        objSearch.ShowDialog()
    End Sub

    Private Sub gridView_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView_OWN.LostFocus
        lblFindHelp.Visible = False
    End Sub
End Class