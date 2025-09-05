<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmNonTagIssueRecieptView
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.gridView = New System.Windows.Forms.DataGridView
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.Label5 = New System.Windows.Forms.Label
        Me.cmbType = New System.Windows.Forms.ComboBox
        Me.cmbItemCounter = New System.Windows.Forms.ComboBox
        Me.txtPacketNo = New System.Windows.Forms.TextBox
        Me.btnSearch = New System.Windows.Forms.Button
        Me.chkDate = New System.Windows.Forms.CheckBox
        Me.pnlDate = New System.Windows.Forms.Panel
        Me.Label6 = New System.Windows.Forms.Label
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.cmbItemName = New System.Windows.Forms.ComboBox
        Me.cmbCostCentre = New System.Windows.Forms.ComboBox
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.txtLotNo = New System.Windows.Forms.TextBox
        Me.Label14 = New System.Windows.Forms.Label
        Me.chkCmbCompany = New BrighttechPack.CheckedComboBox
        Me.Label13 = New System.Windows.Forms.Label
        Me.btnPrint = New System.Windows.Forms.Button
        Me.btnExport = New System.Windows.Forms.Button
        Me.grpSummary = New System.Windows.Forms.GroupBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.txtBothNetWt = New System.Windows.Forms.Label
        Me.txtRecPcs = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.txtRecGrsWt = New System.Windows.Forms.Label
        Me.txtBothGrsWt = New System.Windows.Forms.Label
        Me.txtRecNetWt = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.txtIssPcs = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.txtIssGrsWt = New System.Windows.Forms.Label
        Me.txtBothPcs = New System.Windows.Forms.Label
        Me.txtIssNetWt = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlDate.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.grpSummary.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(10, 45)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(71, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Item Name"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(350, 19)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(76, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Cost Centre"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(10, 69)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(84, 13)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "Item Counter"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(350, 43)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(64, 13)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Packet No"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Location = New System.Drawing.Point(12, 172)
        Me.gridView.MultiSelect = False
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridView.Size = New System.Drawing.Size(998, 456)
        Me.gridView.TabIndex = 0
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(319, 117)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 17
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(213, 117)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 16
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(350, 67)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(35, 13)
        Me.Label5.TabIndex = 10
        Me.Label5.Text = "Type"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbType
        '
        Me.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbType.FormattingEnabled = True
        Me.cmbType.Location = New System.Drawing.Point(432, 63)
        Me.cmbType.Name = "cmbType"
        Me.cmbType.Size = New System.Drawing.Size(132, 21)
        Me.cmbType.TabIndex = 11
        '
        'cmbItemCounter
        '
        Me.cmbItemCounter.FormattingEnabled = True
        Me.cmbItemCounter.Location = New System.Drawing.Point(107, 65)
        Me.cmbItemCounter.Name = "cmbItemCounter"
        Me.cmbItemCounter.Size = New System.Drawing.Size(234, 21)
        Me.cmbItemCounter.TabIndex = 9
        '
        'txtPacketNo
        '
        Me.txtPacketNo.Location = New System.Drawing.Point(432, 39)
        Me.txtPacketNo.Name = "txtPacketNo"
        Me.txtPacketNo.Size = New System.Drawing.Size(96, 21)
        Me.txtPacketNo.TabIndex = 7
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(107, 117)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 15
        Me.btnSearch.Text = "&Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'chkDate
        '
        Me.chkDate.AutoSize = True
        Me.chkDate.Location = New System.Drawing.Point(13, 94)
        Me.chkDate.Name = "chkDate"
        Me.chkDate.Size = New System.Drawing.Size(86, 17)
        Me.chkDate.TabIndex = 12
        Me.chkDate.Text = "Date From"
        Me.chkDate.UseVisualStyleBackColor = True
        '
        'pnlDate
        '
        Me.pnlDate.Controls.Add(Me.Label6)
        Me.pnlDate.Controls.Add(Me.dtpTo)
        Me.pnlDate.Controls.Add(Me.dtpFrom)
        Me.pnlDate.Location = New System.Drawing.Point(107, 89)
        Me.pnlDate.Name = "pnlDate"
        Me.pnlDate.Size = New System.Drawing.Size(237, 26)
        Me.pnlDate.TabIndex = 9
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(109, 2)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(21, 13)
        Me.Label6.TabIndex = 1
        Me.Label6.Text = "To"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(137, 1)
        Me.dtpTo.Mask = "##/##/####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(96, 21)
        Me.dtpTo.TabIndex = 2
        Me.dtpTo.Text = "07/03/9998"
        Me.dtpTo.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(0, 0)
        Me.dtpFrom.Mask = "##/##/####"
        Me.dtpFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom.Size = New System.Drawing.Size(96, 21)
        Me.dtpFrom.TabIndex = 0
        Me.dtpFrom.Text = "07/03/9998"
        Me.dtpFrom.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'cmbItemName
        '
        Me.cmbItemName.FormattingEnabled = True
        Me.cmbItemName.Location = New System.Drawing.Point(107, 41)
        Me.cmbItemName.Name = "cmbItemName"
        Me.cmbItemName.Size = New System.Drawing.Size(234, 21)
        Me.cmbItemName.TabIndex = 5
        '
        'cmbCostCentre
        '
        Me.cmbCostCentre.FormattingEnabled = True
        Me.cmbCostCentre.Location = New System.Drawing.Point(432, 15)
        Me.cmbCostCentre.Name = "cmbCostCentre"
        Me.cmbCostCentre.Size = New System.Drawing.Size(206, 21)
        Me.cmbCostCentre.TabIndex = 3
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(129, 48)
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.NewToolStripMenuItem.Text = "New"
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.txtLotNo)
        Me.GroupBox1.Controls.Add(Me.Label14)
        Me.GroupBox1.Controls.Add(Me.chkCmbCompany)
        Me.GroupBox1.Controls.Add(Me.Label13)
        Me.GroupBox1.Controls.Add(Me.btnPrint)
        Me.GroupBox1.Controls.Add(Me.btnExport)
        Me.GroupBox1.Controls.Add(Me.grpSummary)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.btnExit)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.cmbCostCentre)
        Me.GroupBox1.Controls.Add(Me.btnNew)
        Me.GroupBox1.Controls.Add(Me.cmbType)
        Me.GroupBox1.Controls.Add(Me.cmbItemName)
        Me.GroupBox1.Controls.Add(Me.cmbItemCounter)
        Me.GroupBox1.Controls.Add(Me.pnlDate)
        Me.GroupBox1.Controls.Add(Me.txtPacketNo)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.btnSearch)
        Me.GroupBox1.Controls.Add(Me.chkDate)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(998, 154)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        '
        'txtLotNo
        '
        Me.txtLotNo.Location = New System.Drawing.Point(432, 88)
        Me.txtLotNo.Name = "txtLotNo"
        Me.txtLotNo.Size = New System.Drawing.Size(96, 21)
        Me.txtLotNo.TabIndex = 14
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(350, 92)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(43, 13)
        Me.Label14.TabIndex = 13
        Me.Label14.Text = "Lot No"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkCmbCompany
        '
        Me.chkCmbCompany.CheckOnClick = True
        Me.chkCmbCompany.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCompany.DropDownHeight = 1
        Me.chkCmbCompany.FormattingEnabled = True
        Me.chkCmbCompany.IntegralHeight = False
        Me.chkCmbCompany.Location = New System.Drawing.Point(107, 16)
        Me.chkCmbCompany.Name = "chkCmbCompany"
        Me.chkCmbCompany.Size = New System.Drawing.Size(237, 22)
        Me.chkCmbCompany.TabIndex = 1
        Me.chkCmbCompany.ValueSeparator = ", "
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(10, 17)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(62, 13)
        Me.Label13.TabIndex = 0
        Me.Label13.Text = "Company"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(531, 117)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 30)
        Me.btnPrint.TabIndex = 19
        Me.btnPrint.Text = "Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(425, 117)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 18
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'grpSummary
        '
        Me.grpSummary.Controls.Add(Me.Label9)
        Me.grpSummary.Controls.Add(Me.Label10)
        Me.grpSummary.Controls.Add(Me.txtBothNetWt)
        Me.grpSummary.Controls.Add(Me.txtRecPcs)
        Me.grpSummary.Controls.Add(Me.Label8)
        Me.grpSummary.Controls.Add(Me.txtRecGrsWt)
        Me.grpSummary.Controls.Add(Me.txtBothGrsWt)
        Me.grpSummary.Controls.Add(Me.txtRecNetWt)
        Me.grpSummary.Controls.Add(Me.Label7)
        Me.grpSummary.Controls.Add(Me.txtIssPcs)
        Me.grpSummary.Controls.Add(Me.Label12)
        Me.grpSummary.Controls.Add(Me.txtIssGrsWt)
        Me.grpSummary.Controls.Add(Me.txtBothPcs)
        Me.grpSummary.Controls.Add(Me.txtIssNetWt)
        Me.grpSummary.Controls.Add(Me.Label11)
        Me.grpSummary.Location = New System.Drawing.Point(665, 19)
        Me.grpSummary.Name = "grpSummary"
        Me.grpSummary.Size = New System.Drawing.Size(300, 113)
        Me.grpSummary.TabIndex = 20
        Me.grpSummary.TabStop = False
        Me.grpSummary.Text = "Summary"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.ForeColor = System.Drawing.SystemColors.ActiveCaption
        Me.Label9.Location = New System.Drawing.Point(221, 12)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(69, 13)
        Me.Label9.TabIndex = 2
        Me.Label9.Text = "Net Weight"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.ForeColor = System.Drawing.SystemColors.ActiveCaption
        Me.Label10.Location = New System.Drawing.Point(6, 37)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(49, 13)
        Me.Label10.TabIndex = 3
        Me.Label10.Text = "Receipt"
        '
        'txtBothNetWt
        '
        Me.txtBothNetWt.BackColor = System.Drawing.SystemColors.Window
        Me.txtBothNetWt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.txtBothNetWt.Location = New System.Drawing.Point(219, 84)
        Me.txtBothNetWt.Name = "txtBothNetWt"
        Me.txtBothNetWt.Size = New System.Drawing.Size(73, 21)
        Me.txtBothNetWt.TabIndex = 14
        Me.txtBothNetWt.Text = "Label13"
        Me.txtBothNetWt.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtRecPcs
        '
        Me.txtRecPcs.BackColor = System.Drawing.SystemColors.Window
        Me.txtRecPcs.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.txtRecPcs.Location = New System.Drawing.Point(61, 33)
        Me.txtRecPcs.Name = "txtRecPcs"
        Me.txtRecPcs.Size = New System.Drawing.Size(73, 21)
        Me.txtRecPcs.TabIndex = 4
        Me.txtRecPcs.Text = "Label13"
        Me.txtRecPcs.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.ForeColor = System.Drawing.SystemColors.ActiveCaption
        Me.Label8.Location = New System.Drawing.Point(141, 12)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(70, 13)
        Me.Label8.TabIndex = 1
        Me.Label8.Text = "Grs Weight"
        '
        'txtRecGrsWt
        '
        Me.txtRecGrsWt.BackColor = System.Drawing.SystemColors.Window
        Me.txtRecGrsWt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.txtRecGrsWt.Location = New System.Drawing.Point(140, 33)
        Me.txtRecGrsWt.Name = "txtRecGrsWt"
        Me.txtRecGrsWt.Size = New System.Drawing.Size(73, 21)
        Me.txtRecGrsWt.TabIndex = 5
        Me.txtRecGrsWt.Text = "Label13"
        Me.txtRecGrsWt.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtBothGrsWt
        '
        Me.txtBothGrsWt.BackColor = System.Drawing.SystemColors.Window
        Me.txtBothGrsWt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.txtBothGrsWt.Location = New System.Drawing.Point(140, 84)
        Me.txtBothGrsWt.Name = "txtBothGrsWt"
        Me.txtBothGrsWt.Size = New System.Drawing.Size(73, 21)
        Me.txtBothGrsWt.TabIndex = 13
        Me.txtBothGrsWt.Text = "Label13"
        Me.txtBothGrsWt.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtRecNetWt
        '
        Me.txtRecNetWt.BackColor = System.Drawing.SystemColors.Window
        Me.txtRecNetWt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.txtRecNetWt.Location = New System.Drawing.Point(219, 33)
        Me.txtRecNetWt.Name = "txtRecNetWt"
        Me.txtRecNetWt.Size = New System.Drawing.Size(73, 21)
        Me.txtRecNetWt.TabIndex = 6
        Me.txtRecNetWt.Text = "Label13"
        Me.txtRecNetWt.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.ForeColor = System.Drawing.SystemColors.ActiveCaption
        Me.Label7.Location = New System.Drawing.Point(76, 12)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(43, 13)
        Me.Label7.TabIndex = 0
        Me.Label7.Text = "Pieces"
        '
        'txtIssPcs
        '
        Me.txtIssPcs.BackColor = System.Drawing.SystemColors.Window
        Me.txtIssPcs.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.txtIssPcs.Location = New System.Drawing.Point(61, 59)
        Me.txtIssPcs.Name = "txtIssPcs"
        Me.txtIssPcs.Size = New System.Drawing.Size(73, 21)
        Me.txtIssPcs.TabIndex = 8
        Me.txtIssPcs.Text = "Label13"
        Me.txtIssPcs.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.ForeColor = System.Drawing.SystemColors.ActiveCaption
        Me.Label12.Location = New System.Drawing.Point(6, 88)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(33, 13)
        Me.Label12.TabIndex = 11
        Me.Label12.Text = "Both"
        '
        'txtIssGrsWt
        '
        Me.txtIssGrsWt.BackColor = System.Drawing.SystemColors.Window
        Me.txtIssGrsWt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.txtIssGrsWt.Location = New System.Drawing.Point(140, 59)
        Me.txtIssGrsWt.Name = "txtIssGrsWt"
        Me.txtIssGrsWt.Size = New System.Drawing.Size(73, 21)
        Me.txtIssGrsWt.TabIndex = 9
        Me.txtIssGrsWt.Text = "Label13"
        Me.txtIssGrsWt.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtBothPcs
        '
        Me.txtBothPcs.BackColor = System.Drawing.SystemColors.Window
        Me.txtBothPcs.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.txtBothPcs.Location = New System.Drawing.Point(61, 84)
        Me.txtBothPcs.Name = "txtBothPcs"
        Me.txtBothPcs.Size = New System.Drawing.Size(73, 21)
        Me.txtBothPcs.TabIndex = 12
        Me.txtBothPcs.Text = "Label13"
        Me.txtBothPcs.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtIssNetWt
        '
        Me.txtIssNetWt.BackColor = System.Drawing.SystemColors.Window
        Me.txtIssNetWt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.txtIssNetWt.Location = New System.Drawing.Point(219, 59)
        Me.txtIssNetWt.Name = "txtIssNetWt"
        Me.txtIssNetWt.Size = New System.Drawing.Size(73, 21)
        Me.txtIssNetWt.TabIndex = 10
        Me.txtIssNetWt.Text = "Label13"
        Me.txtIssNetWt.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.ForeColor = System.Drawing.SystemColors.ActiveCaption
        Me.Label11.Location = New System.Drawing.Point(6, 63)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(38, 13)
        Me.Label11.TabIndex = 7
        Me.Label11.Text = "Issue"
        '
        'frmNonTagIssueRecieptView
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1022, 640)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.gridView)
        Me.Controls.Add(Me.GroupBox1)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmNonTagIssueRecieptView"
        Me.Text = "Issue Reciept View"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlDate.ResumeLayout(False)
        Me.pnlDate.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.grpSummary.ResumeLayout(False)
        Me.grpSummary.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtPacketNo As System.Windows.Forms.TextBox
    Friend WithEvents cmbItemCounter As System.Windows.Forms.ComboBox
    Friend WithEvents cmbCostCentre As System.Windows.Forms.ComboBox
    Friend WithEvents cmbItemName As System.Windows.Forms.ComboBox
    Friend WithEvents chkDate As System.Windows.Forms.CheckBox
    Friend WithEvents pnlDate As System.Windows.Forms.Panel
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents cmbType As System.Windows.Forms.ComboBox
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents grpSummary As System.Windows.Forms.GroupBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents txtBothPcs As System.Windows.Forms.Label
    Friend WithEvents txtIssPcs As System.Windows.Forms.Label
    Friend WithEvents txtRecGrsWt As System.Windows.Forms.Label
    Friend WithEvents txtRecPcs As System.Windows.Forms.Label
    Friend WithEvents txtBothNetWt As System.Windows.Forms.Label
    Friend WithEvents txtBothGrsWt As System.Windows.Forms.Label
    Friend WithEvents txtIssNetWt As System.Windows.Forms.Label
    Friend WithEvents txtIssGrsWt As System.Windows.Forms.Label
    Friend WithEvents txtRecNetWt As System.Windows.Forms.Label
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents chkCmbCompany As BrighttechPack.CheckedComboBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents txtLotNo As System.Windows.Forms.TextBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
End Class
