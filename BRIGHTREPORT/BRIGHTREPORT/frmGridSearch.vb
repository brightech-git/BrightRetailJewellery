Public Class frmGridSearch
    Dim dgv As DataGridView
    Dim dtCombo As New DataTable
    Dim cnt As Integer
    Dim searchtype As Integer
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
    Public Sub New(ByRef dgv As DataGridView, Optional ByVal currentSearchIndex As Integer = -1)


        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        searchtype = currentSearchIndex
        Me.dgv = dgv
        dtCombo.Columns.Add("Name", GetType(String))
        dtCombo.Columns.Add("HeaderText", GetType(String))
        ' Add any initialization after the InitializeComponent() call.
        cmbSearchKey.Items.Clear()
        For cnt As Integer = 0 To dgv.ColumnCount - 1
            If Not dgv.Columns(cnt).Visible Then Continue For
            Dim ro As DataRow = dtCombo.NewRow
            ro!NAME = dgv.Columns(cnt).Name
            ro!HEADERTEXT = dgv.Columns(cnt).HeaderText
            dtCombo.Rows.Add(ro)
            cmbSearchKey.Items.Add(dgv.Columns(cnt).HeaderText)
        Next
        If dgv.CurrentCell IsNot Nothing Then
            If currentSearchIndex = -1 Then
                cmbSearchKey.Text = dgv.Columns(dgv.CurrentCell.ColumnIndex).HeaderText
            ElseIf currentSearchIndex = 1 Then
                cmbSearchKey.Text = dgv.Columns("CHQCARDNO").HeaderText
            Else
                cmbSearchKey.Text = dgv.Columns(currentSearchIndex).HeaderText
            End If
            Prop_Gets()
        End If
    End Sub

    Public Sub btnFindNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNext.Click
        If dgv.CurrentRow Is Nothing Then Exit Sub
        Dim findRecord As Boolean = False
        Dim stPos As Integer = dgv.CurrentRow.Index
        cnt = dgv.CurrentRow.Index + 1
        If cnt >= dgv.RowCount Then cnt = 0
        While 1 = 1
            If stPos = cnt Then Exit While
            If cnt >= dgv.RowCount Then cnt = 0 : If stPos = 0 Then stPos += 1
            If chkMatchWholeWord.Checked Then
                If Trim(dgv.Rows(cnt).Cells(dtCombo.Rows(cmbSearchKey.SelectedIndex).Item("NAME").ToString).Value.ToString.ToUpper) = txtSearchText.Text.ToUpper Then
                    dgv.CurrentCell = dgv.Rows(cnt).Cells(dtCombo.Rows(cmbSearchKey.SelectedIndex).Item("NAME").ToString)
                    dgv.Select()
                    findRecord = True
                    Exit While
                End If
            Else
                If cmbSearchKey.Text <> "" Then
                    If Trim(dgv.Rows(cnt).Cells(dtCombo.Rows(cmbSearchKey.SelectedIndex).Item("NAME").ToString).Value.ToString).ToUpper.Contains(txtSearchText.Text.ToUpper) Then
                        dgv.CurrentCell = dgv.Rows(cnt).Cells(dtCombo.Rows(cmbSearchKey.SelectedIndex).Item("NAME").ToString)
                        dgv.Select()
                        findRecord = True
                        Exit While
                    End If
                Else
                    For I As Integer = 0 To dgv.ColumnCount - 1
                        If Not dgv.Columns(I).Visible Then Continue For
                        If Trim(dgv.Rows(cnt).Cells(I).Value.ToString).ToUpper.Contains(txtSearchText.Text.ToUpper) Then
                            dgv.CurrentCell = dgv.Rows(cnt).Cells(I)
                            dgv.Select()
                            findRecord = True
                            Exit While
                        End If
                    Next
                End If
            End If
            cnt += 1
        End While
        If findRecord = False Then MsgBox("Record Not Found", MsgBoxStyle.Information) : Exit Sub
        Prop_Sets()
        If searchtype = 1 Then Me.Hide()
    End Sub

    Private Sub frmGridSearch_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        If txtSearchText.Text <> "" And searchtype <> 1 Then
            txtSearchText.Select()
            txtSearchText.SelectAll()
        ElseIf searchtype = 1 Then
            txtSearchText.Select()
            txtSearchText.SelectAll()
        End If
    End Sub

    Private Sub frmGridSearch_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Hide()
        End If
    End Sub

    Private Sub frmGridSearch_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) And (searchtype <> 1 Or txtSearchText.Focused = False) Then
            If searchtype = 1 Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmGridSearch_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    End Sub

    Private Sub txtSearchText_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtSearchText.KeyDown
        If e.KeyCode = Keys.Enter Then
            If searchtype = 1 Then
                btnFindNext_Click(Me, New EventArgs)

            End If
        End If
    End Sub
    Private Sub Prop_Sets()
        Dim obj As New frmGridSearch_Properties
        obj.p_cmbSearchKey = cmbSearchKey.Text
        SetSettingsObj(obj, Me.Name, GetType(frmGridSearch_Properties))
    End Sub
    Private Sub Prop_Gets()
        Dim obj As New frmGridSearch_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmGridSearch_Properties))
        cmbSearchKey.Text = obj.p_cmbSearchKey
    End Sub
End Class
Public Class frmGridSearch_Properties
    Private cmbSearchKey As String = ""
    Public Property p_cmbSearchKey() As String
        Get
            Return cmbSearchKey
        End Get
        Set(ByVal value As String)
            cmbSearchKey = value
        End Set
    End Property
End Class