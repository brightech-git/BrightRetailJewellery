Imports System.Drawing
Imports System.Windows.Forms
Imports System.Data.OleDb

Public Class frmSalesIncentive
    Inherits Form

    Private dgv As DataGridView
    Private lblTotalWeight As Label
    Private lblGreeting As Label
    Private lblCaption As Label
    Private lblTotalIncentiveBig As Label
    Private lblFooter As Label
    Private btnOk As Button
    Private pnlIncentiveBox As Panel

    Private _goldRatePerGram As Decimal
    Private _staffName As String

    Public Sub New(goldRatePerGram As Decimal, staffName As String)
        _goldRatePerGram = goldRatePerGram
        _staffName = staffName
        Me.KeyPreview = True
        BuildUI()
    End Sub

    Private Sub BuildUI()
        Me.Text = "Sales Incentive Alert"
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.BackColor = SystemColors.InactiveCaption
        Me.Width = 700
        Me.Height = 580
        Me.Font = New Font("Segoe UI", 10.0F)

        ' Header
        Dim pnlHeader As New Panel
        pnlHeader.Dock = DockStyle.Top
        pnlHeader.Height = 48
        pnlHeader.BackColor = Color.Lavender
        Dim lblTitle As New Label
        lblTitle.AutoSize = True
        lblTitle.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
        lblTitle.ForeColor = SystemColors.ControlText
        lblTitle.Text = "INCENTIVE ALERT  |  COMBINATION SALE"
        lblTitle.Location = New Point(16, 12)
        pnlHeader.Controls.Add(lblTitle)
        Me.Controls.Add(pnlHeader)

        ' Total Weight label
        lblTotalWeight = New Label
        lblTotalWeight.AutoSize = True
        lblTotalWeight.Font = New Font("Segoe UI", 14.0F, FontStyle.Bold)
        lblTotalWeight.ForeColor = SystemColors.ControlText
        lblTotalWeight.Location = New Point(16, 60)
        lblTotalWeight.Text = "Total weight: 0.000 g"
        Me.Controls.Add(lblTotalWeight)

        ' DataGridView
        dgv = New DataGridView
        dgv.Location = New Point(16, 98)
        dgv.Width = 652
        dgv.Height = 220
        dgv.AllowUserToAddRows = False
        dgv.AllowUserToDeleteRows = False
        dgv.ReadOnly = True
        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgv.RowHeadersVisible = False
        dgv.BorderStyle = BorderStyle.FixedSingle
        dgv.BackgroundColor = SystemColors.Window
        dgv.GridColor = SystemColors.ControlDark
        dgv.DefaultCellStyle.BackColor = SystemColors.Window
        dgv.DefaultCellStyle.ForeColor = SystemColors.ControlText
        dgv.DefaultCellStyle.Font = New Font("Segoe UI", 10.0F)
        dgv.DefaultCellStyle.Padding = New Padding(4, 2, 4, 2)
        dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.Lavender
        dgv.ColumnHeadersDefaultCellStyle.ForeColor = SystemColors.ControlText
        dgv.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 10.0F, FontStyle.Bold)
        dgv.EnableHeadersVisualStyles = False
        dgv.ColumnHeadersHeight = 34
        dgv.RowTemplate.Height = 30
        AddColumns()
        Me.Controls.Add(dgv)

        ' Incentive highlight box
        pnlIncentiveBox = New Panel
        pnlIncentiveBox.Location = New Point(16, 330)
        pnlIncentiveBox.Width = 652
        pnlIncentiveBox.Height = 140
        pnlIncentiveBox.BackColor = Color.Lavender

        ' Greeting: "Congratulations [Name]"
        lblGreeting = New Label
        lblGreeting.AutoSize = False
        lblGreeting.Width = 652
        lblGreeting.Height = 30
        lblGreeting.Location = New Point(0, 8)
        lblGreeting.TextAlign = ContentAlignment.MiddleCenter
        lblGreeting.Font = New Font("Segoe UI", 11.0F, FontStyle.Bold)
        lblGreeting.BackColor = Color.Transparent
        lblGreeting.ForeColor = SystemColors.ControlText
        lblGreeting.Text = "Congratulations " & _staffName

        ' Sub caption
        lblCaption = New Label
        lblCaption.AutoSize = False
        lblCaption.Width = 652
        lblCaption.Height = 22
        lblCaption.Location = New Point(0, 44)
        lblCaption.TextAlign = ContentAlignment.MiddleCenter
        lblCaption.Font = New Font("Segoe UI", 10.0F)
        lblCaption.BackColor = Color.Transparent
        lblCaption.ForeColor = SystemColors.ControlText
        lblCaption.Text = "Your total individual incentive on this sale :-"

        ' Big incentive amount
        lblTotalIncentiveBig = New Label
        lblTotalIncentiveBig.AutoSize = False
        lblTotalIncentiveBig.Width = 652
        lblTotalIncentiveBig.Height = 52
        lblTotalIncentiveBig.Location = New Point(0, 70)
        lblTotalIncentiveBig.TextAlign = ContentAlignment.MiddleCenter
        lblTotalIncentiveBig.Font = New Font("Segoe UI", 26.0F, FontStyle.Bold)
        lblTotalIncentiveBig.BackColor = Color.Transparent
        lblTotalIncentiveBig.ForeColor = Color.DarkBlue
        lblTotalIncentiveBig.Text = ChrW(8377) & "0"

        pnlIncentiveBox.Controls.Add(lblGreeting)
        pnlIncentiveBox.Controls.Add(lblCaption)
        pnlIncentiveBox.Controls.Add(lblTotalIncentiveBig)
        Me.Controls.Add(pnlIncentiveBox)

        ' Ok button
        btnOk = New Button
        btnOk.Location = New Point(16, 482)
        btnOk.Width = 652
        btnOk.Height = 44
        btnOk.Text = "Ok"
        btnOk.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
        btnOk.BackColor = SystemColors.Window
        btnOk.ForeColor = SystemColors.ControlText
        btnOk.FlatStyle = FlatStyle.Flat
        btnOk.FlatAppearance.BorderColor = SystemColors.ControlDark
        btnOk.FlatAppearance.BorderSize = 1
        btnOk.Cursor = Cursors.Hand
        AddHandler btnOk.Click, AddressOf btnOk_Click
        Me.Controls.Add(btnOk)

        ' Footer
        lblFooter = New Label
        lblFooter.AutoSize = False
        lblFooter.Width = 652
        lblFooter.Height = 22
        lblFooter.Location = New Point(16, 532)
        lblFooter.TextAlign = ContentAlignment.MiddleCenter
        lblFooter.Font = New Font("Segoe UI", 9.0F)
        lblFooter.ForeColor = SystemColors.ControlText
        lblFooter.Text = "Sample: value calculated at " & ChrW(8377) & "10,000 per gram"
        Me.Controls.Add(lblFooter)
    End Sub

    Private Sub AddColumns()
        Dim colItem As New DataGridViewTextBoxColumn
        colItem.Name = "colItem"
        colItem.HeaderText = "Item"
        colItem.Width = 150

        Dim colSlab As New DataGridViewTextBoxColumn
        colSlab.Name = "colSlab"
        colSlab.HeaderText = "Slab"
        colSlab.Width = 115

        Dim colWeight As New DataGridViewTextBoxColumn
        colWeight.Name = "colWeight"
        colWeight.HeaderText = "Weight"
        colWeight.Width = 75
        colWeight.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

        Dim colValue As New DataGridViewTextBoxColumn
        colValue.Name = "colValue"
        colValue.HeaderText = "Value"
        colValue.Width = 110
        colValue.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

        Dim colRate As New DataGridViewTextBoxColumn
        colRate.Name = "colRate"
        colRate.HeaderText = "Rate"
        colRate.Width = 90
        colRate.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

        Dim colIncentive As New DataGridViewTextBoxColumn
        colIncentive.Name = "colIncentive"
        colIncentive.HeaderText = "Incentive"
        colIncentive.Width = 100
        colIncentive.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

        dgv.Columns.AddRange(colItem, colSlab, colWeight, colValue, colRate, colIncentive)
    End Sub

    Public Sub LoadData(items As System.Collections.Generic.List(Of IncentiveItem))
        dgv.Rows.Clear()
        Dim totalWeight As Decimal = 0
        Dim totalValue As Decimal = 0
        Dim totalIncentive As Decimal = 0

        For Each itm As IncentiveItem In items
            totalWeight += itm.WeightGrams
            totalValue += itm.Value
            totalIncentive += itm.IncentiveAmt
            Dim idx As Integer = dgv.Rows.Add()
            dgv.Rows(idx).Cells("colItem").Value = itm.ItemName
            dgv.Rows(idx).Cells("colSlab").Value = itm.Slab
            dgv.Rows(idx).Cells("colWeight").Value = Format(itm.WeightGrams, "0.000") & " g"
            dgv.Rows(idx).Cells("colValue").Value = ChrW(8377) & Format(itm.Value, "#,##0.00")
            dgv.Rows(idx).Cells("colRate").Value = itm.RateDisplay
            dgv.Rows(idx).Cells("colIncentive").Value = ChrW(8377) & Format(itm.IncentiveAmt, "#,##0.00")
        Next

        ' Total row
        Dim totIdx As Integer = dgv.Rows.Add()
        dgv.Rows(totIdx).DefaultCellStyle.Font = New Font("Segoe UI", 10.0F, FontStyle.Bold)
        dgv.Rows(totIdx).DefaultCellStyle.BackColor = Color.Lavender
        dgv.Rows(totIdx).Cells("colItem").Value = "Total"
        dgv.Rows(totIdx).Cells("colSlab").Value = ""
        dgv.Rows(totIdx).Cells("colWeight").Value = Format(totalWeight, "0.000") & " g"
        dgv.Rows(totIdx).Cells("colValue").Value = ChrW(8377) & Format(totalValue, "#,##0.00")
        dgv.Rows(totIdx).Cells("colRate").Value = ""
        dgv.Rows(totIdx).Cells("colIncentive").Value = ChrW(8377) & Format(totalIncentive, "#,##0.00")

        Dim roundedIncentive As Decimal = Math.Round(totalIncentive, 0, MidpointRounding.AwayFromZero)

        lblTotalWeight.Text = "Total weight: " & Format(totalWeight, "0.000") & " g"
        lblGreeting.Text = "Congratulations " & _staffName
        lblTotalIncentiveBig.Text = ChrW(8377) & Format(roundedIncentive, "#,##0")

        If _goldRatePerGram > 0 Then
            lblFooter.Text = "Sample: value calculated at " & ChrW(8377) & Format(_goldRatePerGram, "#,##0") & " per gram"
        End If
    End Sub

    Private Sub btnOk_Click(sender As Object, e As EventArgs)
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Protected Overrides Sub OnKeyDown(e As KeyEventArgs)
        MyBase.OnKeyDown(e)
        If e.KeyCode = Keys.Escape OrElse e.KeyCode = Keys.Enter Then btnOk_Click(Me, EventArgs.Empty)
    End Sub
End Class

Public Class IncentiveItem
    Public ItemName As String
    Public WeightGrams As Decimal
    Public Value As Decimal
    Public Slab As String
    Public RateDisplay As String
    Public IncentiveAmt As Decimal
End Class
