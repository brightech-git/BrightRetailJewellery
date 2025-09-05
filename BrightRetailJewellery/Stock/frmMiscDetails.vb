Imports System
Imports System.Data.OleDb
Public Class frmMiscDetails
    Dim strsql As String = ""
    Dim cmd As New OleDbCommand
    Dim da As New OleDbDataAdapter
    Public dtMiscDetails As New DataTable
    Dim dtLoadMisc As New DataTable
    Dim MiscIndex As Integer = 0
    Dim TotalAmt As Double

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        With dtMiscDetails
            .Columns.Add("KEYNO", GetType(Integer))
            .Columns.Add("MISC", GetType(String))
            .Columns.Add("AMOUNT", GetType(Double))
            .Columns("KEYNO").AutoIncrement = True
            .Columns("KEYNO").AutoIncrementSeed = 0
            .Columns("KEYNO").AutoIncrementStep = 1
        End With
        objGPack.Validator_Object(Me)
    End Sub

#Region "Miscellaneous Procedures"
    Private Sub CalcMiscTotalAmount()
        Dim miscTot As Double = Nothing
        For cnt As Integer = 0 To gridMisc.Rows.Count - 1
            miscTot += Val(gridMisc.Rows(cnt).Cells("AMOUNT").Value.ToString)
        Next
        gridMiscFooter.Rows(0).Cells("AMOUNT").Value = IIf(miscTot <> 0, Format(miscTot, "0.00"), DBNull.Value)
        txtMiscTotAmt.Text = IIf(miscTot <> 0, Format(miscTot, "0.00"), "")
    End Sub

    Private Sub txtMiscAmount_Amt_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtMiscAmount_AMT.KeyDown
        If e.KeyCode = Keys.Down Then
            gridMisc.CurrentCell = gridMisc.Rows(gridMisc.RowCount - 1).Cells(0)
            gridMisc.Select()
        ElseIf e.KeyCode = Keys.Escape Then
            dtMiscDetails.AcceptChanges()
            Me.Close()
        End If
    End Sub
    Private Sub txtMiscAmount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtMiscAmount_AMT.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If cmbMiscDetails.Text = "" Then
                MsgBox(Me.GetNextControl(cmbMiscDetails, False).Text + E0001, MsgBoxStyle.Information)
                cmbMiscDetails.Select()
                Exit Sub
            End If
            If objGPack.DupCheck("SELECT MISCNAME FROM " & cnAdminDb & "..MISCCHARGES WHERE MISCNAME = '" & cmbMiscDetails.Text & "' AND  ACTIVE = 'Y'") = False Then
                MsgBox("Invalid Misc", MsgBoxStyle.Information)
                cmbMiscDetails.Focus()
                Exit Sub
            End If
            If Not Val(txtMiscAmount_AMT.Text) > 0 Then
                MsgBox(Me.GetNextControl(txtMiscAmount_AMT, False).Text + E0001, MsgBoxStyle.Information)
                txtMiscAmount_AMT.Select()
                Exit Sub
            End If
            If txtMiscRowIndex.Text <> "" Then
                With gridMisc.Rows(Val(txtMiscRowIndex.Text))
                    .Cells("MISC").Value = cmbMiscDetails.Text
                    .Cells("AMOUNT").Value = IIf(Val(txtMiscAmount_AMT.Text) <> 0, Val(txtMiscAmount_AMT.Text), DBNull.Value)
                    dtMiscDetails.AcceptChanges()
                    GoTo AFTERINSERT
                End With
            End If
            Dim ro As DataRow = Nothing
            ro = dtMiscDetails.NewRow
            ro("MISC") = cmbMiscDetails.Text
            ro("AMOUNT") = IIf(Val(txtMiscAmount_AMT.Text) <> 0, Val(txtMiscAmount_AMT.Text), DBNull.Value)
            dtMiscDetails.Rows.Add(ro)
            dtMiscDetails.AcceptChanges()
            MiscIndex = cmbMiscDetails.SelectedIndex
AFTERINSERT:

            If Not cmbMiscDetails.Items.Count = Val(MiscIndex) + 1 Then
                cmbMiscDetails.SelectedIndex = Val(MiscIndex) + 1
            Else
                cmbMiscDetails.SelectedIndex = 0
            End If
            CalculateTotalAmt()
            txtMiscTotAmt.Text = TotalAmt
            cmbMiscDetails.Select()
            txtMiscRowIndex.Clear()
        End If
    End Sub
    Private Sub CalculateTotalAmt()
        txtMiscAmount_AMT.Clear()
        For cnt As Integer = 0 To dtMiscDetails.Rows.Count - 1
            TotalAmt = Val(TotalAmt) + Val(dtMiscDetails.Rows(cnt).Item("AMOUNT").ToString)
        Next
    End Sub
    Private Sub gridMisc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridMisc.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
        End If
    End Sub

    Private Sub gridMisc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridMisc.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            'gridMisc.CurrentCell = gridMisc.Rows(gridMisc.CurrentRow.Index).Cells(0)
            If gridMisc.CurrentRow IsNot Nothing Then
                With gridMisc.Rows(gridMisc.CurrentRow.Index)
                    cmbMiscDetails.Text = .Cells("MISC").FormattedValue
                    txtMiscAmount_AMT.Text = .Cells("AMOUNT").FormattedValue
                    txtMiscRowIndex.Text = gridMisc.CurrentRow.Index
                    MiscIndex = cmbMiscDetails.SelectedIndex
                    cmbMiscDetails.Focus()
                    cmbMiscDetails.SelectAll()
                End With
            End If
        ElseIf e.KeyChar = Chr(Keys.Escape) Then
            dtMiscDetails.AcceptChanges()
            Me.Close()
        Else
            CalcMiscTotalAmount()
        End If
    End Sub

    Private Sub gridMisc_UserDeletedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles gridMisc.UserDeletedRow
        dtMiscDetails.AcceptChanges()
        'CalcFinalTotal()
        If Not gridMisc.RowCount > 0 Then
            cmbMiscDetails.Select()
        End If
    End Sub

    Private Sub frmMiscDetails_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadMiscName()
        gridMisc.DataSource = dtMiscDetails
        StyleGrid()
        If cmbMiscDetails.Items.Count > 0 Then cmbMiscDetails.SelectedIndex = 0 : MiscIndex = 0
        cmbMiscDetails.Select()
    End Sub
    Private Sub LoadMiscName()
        strsql = " SELECT MISCNAME FROM " & cnAdminDb & "..MISCCHARGES WHERE ACTIVE = 'Y'"
        dtLoadMisc = New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dtLoadMisc)
        BrighttechPack.FillCombo(cmbMiscDetails, dtLoadMisc, "MISCNAME", True)
    End Sub
    Private Sub LoadMiscDetails()
        If cmbMiscDetails.Text <> "" Then
            strsql = " SELECT DEFAULTVALUE FROM " & cnAdminDb & "..MISCCHARGES WHERE MISCNAME = '" & cmbMiscDetails.Text & "'"
            Dim amt As Double = Val(objGPack.GetSqlValue(strsql, "DEFAULTVALUE", , tran))
            txtMiscAmount_AMT.Text = IIf(amt <> 0, Format(amt, "0.00"), "")
            Me.SelectNextControl(cmbMiscDetails, True, True, True, True)
        End If
    End Sub
    Private Sub StyleGrid()
        With gridMisc
            For cnt As Integer = 0 To dtMiscDetails.Columns.Count - 1
                If gridMisc.Columns(cnt).Name = "KEYNO" Then gridMisc.Columns(cnt).Visible = False
                If gridMisc.Columns(cnt).Name = "MISC" Then gridMisc.Columns(cnt).Width = cmbMiscDetails.Width
                If gridMisc.Columns(cnt).Name = "AMOUNT" Then gridMisc.Columns(cnt).Width = txtMiscAmount_AMT.Width
            Next
        End With
    End Sub

    Private Sub frmMiscDetails_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Enter Then
        ElseIf e.KeyCode = Keys.Escape Then
            dtMiscDetails.AcceptChanges()
            Me.Close()
        End If
    End Sub

    Private Sub txtMiscMisc_KeyPress(sender As Object, e As KeyPressEventArgs)
        If e.KeyChar = Chr(Keys.Enter) Then
            Me.SelectNextControl(cmbMiscDetails, True, True, True, True)
        ElseIf e.KeyChar = Chr(Keys.Escape) Then
            dtMiscDetails.AcceptChanges()
            Me.Close()
        End If
    End Sub

    Private Sub cmbMiscDetails_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cmbMiscDetails.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            Name = cmbMiscDetails.Text
            If Name <> "" Then
                cmbMiscDetails.Text = Name
                LoadMiscDetails()
            Else
                cmbMiscDetails.Focus()
                cmbMiscDetails.Select()
            End If
        ElseIf e.KeyChar = Chr(Keys.Escape) Then
            dtMiscDetails.AcceptChanges()
            Me.Close()
        End If
    End Sub
    Private Sub frmMiscDetails_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
        ElseIf e.KeyChar = chr(Keys.Escape) Then
            dtMiscDetails.AcceptChanges()
            Me.Close()
        End If
    End Sub

    Private Sub ClearToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ClearToolStripMenuItem.Click
        dtMiscDetails.Clear()
        dtMiscDetails.AcceptChanges()
        txtMiscTotAmt.Clear()
        txtMiscRowIndex.Clear()
        txtMiscAmount_AMT.Clear()
        MiscIndex = 0
        TotalAmt = 0
        txtMiscRowIndex.Text = ""
        txtMiscTotAmt.Text = ""
        cmbMiscDetails.SelectedIndex = 0
    End Sub
#End Region
End Class