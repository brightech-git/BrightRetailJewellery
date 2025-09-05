Imports System.Data.OleDb

Public Class frmLanguageSetDialg
    Dim strSql As String = Nothing
    Dim da As OleDbDataAdapter
    Dim dtLanguage As New DataTable
    Dim dtControls As New DataTable
    Dim frmSender As Form
    Dim row As DataRow

    Public Sub New(ByVal f As Form)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        frmSender = f
        txtGridColumn.Visible = False
        ''txtGridColumn.Font = New Font("LT-TM-Kurinji", 12, FontStyle.Regular)
        Try
            strSql = " SELECT LANGID,LANGNAME,FONT FROM " & G_CnAdmindb & "..LANGMASTER ORDER BY LANGNAME"
            da = New OleDbDataAdapter(strSql, G_Cn)
            da.Fill(dtLanguage)
            dtLanguage.AcceptChanges()

            row = dtLanguage.NewRow
            row!LangId = "ENG"
            row!LANGNAME = "ENGLISH"
            row!FONT = "VERDANA"
            dtLanguage.Rows.Add(row)
            dtLanguage.AcceptChanges()

            cmbLanguage.DataSource = dtLanguage
            cmbLanguage.DisplayMember = "LANGNAME"
            cmbLanguage.Text = "ENGLISH"


            ''funcGridStyle(gridView)
            gridView.RowHeadersVisible = False
            gridView.RowTemplate.Height = 21

            dtControls.Columns.Add(New DataColumn("CTRLID"))
            dtControls.Columns.Add(New DataColumn("DEFCTRLCAPTION"))
            dtControls.Columns.Add(New DataColumn("CTRLCAPTION"))
            dtControls.AcceptChanges()
            gridView.DataSource = dtControls
            With gridView
                .Columns("CTRLID").Visible = False
                .Columns("DEFCTRLCAPTION").HeaderText = "NAME"
                .Columns("DEFCTRLCAPTION").DefaultCellStyle.SelectionBackColor = Color.White
                .Columns("DEFCTRLCAPTION").DefaultCellStyle.SelectionForeColor = Color.Black
                .Columns("DEFCTRLCAPTION").Width = 240
                .Columns("CTRLCAPTION").HeaderText = "VALUE"
                .Columns("CTRLCAPTION").Width = txtGridColumn.Width
                ''.Columns("CTRLCAPTION").DefaultCellStyle.Font = New Font("LT-TM-Kurinji", 12, FontStyle.Regular)
            End With
            SetControlNames(frmSender)
            dtControls.AcceptChanges()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub
    Private Sub SetControlNames(ByVal f As Object)
        For Each obj As Object In CType(f, Control).Controls
            If TypeOf obj Is Label Then
                row = dtControls.NewRow
                row!CTRLID = CType(obj, Label).Name
                row!DEFCTRLCAPTION = CType(obj, Label).Text
                row!CTRLCAPTION = "" ''CType(obj, Label).Text
                dtControls.Rows.Add(row)
            Else
                SetControlNames(obj)
            End If
        Next
    End Sub

    Private Sub frmLanguageSetDialg_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        gridView.Columns("CTRLID").Visible = False
    End Sub

    Private Sub gridView_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.CellEnter
        Dim gridViewPt As Point = gridView.Location
        Select Case e.ColumnIndex
            Case 2
                txtGridColumn.Visible = True
                txtGridColumn.BringToFront()
                txtGridColumn.Size = gridView.GetCellDisplayRectangle(gridView.CurrentCell.ColumnIndex, gridView.CurrentCell.RowIndex, True).Size
                txtGridColumn.Location = gridView.GetCellDisplayRectangle(gridView.CurrentCell.ColumnIndex, gridView.CurrentCell.RowIndex, True).Location + gridViewPt
                txtGridColumn.Text = gridView.CurrentCell.Value.ToString
                txtGridColumn.SelectAll()
                txtGridColumn.Focus()
        End Select
    End Sub

    Private Sub gridView_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.CellLeave
        If gridView.CurrentCell.ColumnIndex = 2 Then
            gridView.CurrentCell.Value = txtGridColumn.Text
        End If
        txtGridColumn.Text = ""
        txtGridColumn.Visible = False
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Escape Then
            txtGridColumn.Focus()
        End If
    End Sub

    Private Sub gridView_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.SelectionChanged
        If gridView.CurrentCell.ColumnIndex = 1 Then
            gridView.CurrentCell = gridView.Rows(gridView.CurrentCell.RowIndex).Cells(2)
        End If
    End Sub

    Private Sub txtGridColumn_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtGridColumn.KeyDown
        If e.KeyCode = Keys.Down Then
            If gridView.CurrentRow.Index <> gridView.RowCount - 1 Then
                gridView.CurrentCell = gridView.Rows(gridView.CurrentRow.Index + 1).Cells(2)
            End If
        ElseIf e.KeyCode = Keys.Up Then
            If gridView.CurrentRow.Index <> 0 Then
                gridView.CurrentCell = gridView.Rows(gridView.CurrentRow.Index - 1).Cells(2)
            End If
        ElseIf e.KeyCode = Keys.Left Then
            If gridView.CurrentCell.ColumnIndex <> 0 Then
                gridView.CurrentCell = gridView.Rows(gridView.CurrentRow.Index).Cells(2)
            End If
        ElseIf e.KeyCode = Keys.Right Then
            If gridView.CurrentCell.ColumnIndex <> gridView.ColumnCount - 1 Then
                gridView.CurrentCell = gridView.Rows(gridView.CurrentRow.Index).Cells(2)
            End If
        End If
    End Sub

    Private Sub txtGridColumn_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtGridColumn.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If gridView.CurrentRow.Index <> gridView.RowCount - 1 Then
                gridView.CurrentCell = gridView.Rows(gridView.CurrentRow.Index + 1).Cells(2)
            End If
        End If
    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Dim ctrlId As String = Nothing
        Dim ctrlCaption As String = Nothing
        Try
            G_Tran = G_Cn.BeginTransaction
            G_DTable = New DataTable
            G_Cmd = New OleDbCommand("SELECT TOP 1 LANGID FROM " & G_CnAdmindb & "..LANGMASTER WHERE LANGNAME = '" & cmbLanguage.Text & "'", G_Cn, G_Tran)
            G_DAdapter = New OleDbDataAdapter(G_Cmd)
            G_DAdapter.Fill(G_DTable)
            Dim lagId As String = Nothing
            If G_DTable.Rows.Count > 0 Then
                lagId = G_DTable.Rows(0).Item(0).ToString
            Else
                Exit Sub
            End If
            For i As Integer = 0 To gridView.RowCount - 1
                With gridView.Rows(i)
                    ctrlId = .Cells("CTRLID").Value.ToString
                    ctrlCaption = .Cells("CTRLCAPTION").Value.ToString
                End With
                If Trim(ctrlCaption) <> Nothing Then
                    InsertIntoCaptionMaster( _
                        lagId _
                        , frmSender.Name, ctrlId, ctrlCaption)
                End If
                ctrlId = Nothing
                ctrlCaption = Nothing
            Next
            G_Tran.Commit()
        Catch ex As Exception
            MsgBox("Message : " + ex.Message + vbCrLf + "Stack Trace : " + ex.StackTrace)
        End Try
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub cmbLanguage_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbLanguage.SelectedValueChanged
        Dim ffontName As String
        G_DTable = New DataTable
        G_DAdapter = New OleDbDataAdapter("SELECT TOP 1 FONT FROM " & G_CnAdmindb & "..LANGMASTER WHERE LANGNAME = '" & cmbLanguage.Text & "'", G_Cn)
        G_DAdapter.Fill(G_DTable)
        If G_DTable.Rows.Count > 0 Then
            ffontName = G_DTable.Rows(0).Item(0).ToString
        Else
            ffontName = "VERDANA"
        End If
        txtGridColumn.Font = New Font(ffontName, 12, FontStyle.Regular)
        If gridView.RowCount > 0 Then
            gridView.Columns("CTRLCAPTION").DefaultCellStyle.Font = New Font(ffontName, 12, FontStyle.Regular)
            gridView.RowTemplate.Height = txtGridColumn.Height
        End If
    End Sub

    Public Sub InsertIntoCaptionMaster(ByVal languageId As String, ByVal frmId As String, ByVal ctrlId As String, ByVal ctrlCaption As String)
        strSql = " IF(SELECT 1 FROM " & G_CnAdmindb & "..CAPTIONMASTER WHERE"
        strSql += " LANGID = '" & languageId & "'"
        strSql += " AND FRMID = '" & frmId & "'"
        strSql += " AND CTRLID = '" & ctrlId & "')>0"
        strSql += " DELETE FROM " & G_CnAdmindb & "..CAPTIONMASTER WHERE"
        strSql += " LANGID = '" & languageId & "'"
        strSql += " AND FRMID = '" & frmId & "'"
        strSql += " AND CTRLID = '" & ctrlId & "'"
        G_Cmd = New OleDbCommand(strSql, G_Cn, G_Tran)
        G_Cmd.ExecuteNonQuery()

        strSql = " INSERT INTO " & G_CnAdmindb & "..CAPTIONMASTER"
        strSql += " (LANGID,FRMID,CTRLID,CTRLCAPTION)"
        strSql += " VALUES"
        strSql += " ("
        strSql += " '" & languageId & "'"
        strSql += " ,'" & frmId & "'"
        strSql += " ,'" & ctrlId & "'"
        strSql += " ,'" & ctrlCaption & "'"
        strSql += " )"
        G_Cmd = New OleDbCommand(strSql, G_Cn, G_Tran)
        G_Cmd.ExecuteNonQuery()
    End Sub

    Private Sub txtGridColumn_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtGridColumn.LostFocus
        If gridView.CurrentCell.ColumnIndex = 2 Then
            gridView.CurrentCell.Value = txtGridColumn.Text
        End If
    End Sub
End Class