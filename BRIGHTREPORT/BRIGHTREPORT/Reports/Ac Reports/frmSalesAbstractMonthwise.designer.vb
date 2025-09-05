<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSalesAbstractMonthwise
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
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.gridView = New System.Windows.Forms.DataGridView
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExtToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ResizeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.gridheader2 = New System.Windows.Forms.DataGridView
        Me.gridheader1 = New System.Windows.Forms.DataGridView
        Me.pnlHeading = New System.Windows.Forms.Panel
        Me.lblTitle = New System.Windows.Forms.Label
        Me.btnView_Search = New System.Windows.Forms.Button
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.btnExport = New System.Windows.Forms.Button
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.rbtcategorywise = New System.Windows.Forms.RadioButton
        Me.rbtmonthwise = New System.Windows.Forms.RadioButton
        Me.Label4 = New System.Windows.Forms.Label
        Me.chkwithtot = New System.Windows.Forms.CheckBox
        Me.chkwithsr = New System.Windows.Forms.CheckBox
        Me.chkreceipt = New System.Windows.Forms.CheckBox
        Me.chksales = New System.Windows.Forms.CheckBox
        Me.chkamt = New System.Windows.Forms.CheckBox
        Me.chkGrsWt = New System.Windows.Forms.CheckBox
        Me.chkPcs = New System.Windows.Forms.CheckBox
        Me.chkcmbmetal = New BrighttechPack.CheckedComboBox
        Me.chkCmbCostCentre = New BrighttechPack.CheckedComboBox
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.Panel2.SuspendLayout()
        Me.Panel3.SuspendLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        CType(Me.gridheader2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridheader1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlHeading.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.Panel3)
        Me.Panel2.Controls.Add(Me.gridheader2)
        Me.Panel2.Controls.Add(Me.gridheader1)
        Me.Panel2.Controls.Add(Me.pnlHeading)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 113)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(1028, 563)
        Me.Panel2.TabIndex = 1
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.gridView)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel3.Location = New System.Drawing.Point(0, 73)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(1028, 490)
        Me.Panel3.TabIndex = 4
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.gridView.ContextMenuStrip = Me.ContextMenuStrip1
        Me.gridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView.Location = New System.Drawing.Point(0, 0)
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.RowHeadersVisible = False
        Me.gridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.gridView.RowTemplate.Height = 21
        Me.gridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridView.Size = New System.Drawing.Size(1028, 490)
        Me.gridView.TabIndex = 0
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.ExtToolStripMenuItem, Me.ResizeToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(207, 70)
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(206, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ExtToolStripMenuItem
        '
        Me.ExtToolStripMenuItem.Name = "ExtToolStripMenuItem"
        Me.ExtToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExtToolStripMenuItem.Size = New System.Drawing.Size(206, 22)
        Me.ExtToolStripMenuItem.Text = "Exit"
        Me.ExtToolStripMenuItem.Visible = False
        '
        'ResizeToolStripMenuItem
        '
        Me.ResizeToolStripMenuItem.Name = "ResizeToolStripMenuItem"
        Me.ResizeToolStripMenuItem.Size = New System.Drawing.Size(206, 22)
        Me.ResizeToolStripMenuItem.Text = "ResizeToolStripMenuItem"
        '
        'gridheader2
        '
        Me.gridheader2.AllowUserToAddRows = False
        Me.gridheader2.AllowUserToDeleteRows = False
        Me.gridheader2.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridheader2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.gridheader2.Dock = System.Windows.Forms.DockStyle.Top
        Me.gridheader2.Enabled = False
        Me.gridheader2.Location = New System.Drawing.Point(0, 49)
        Me.gridheader2.Name = "gridheader2"
        Me.gridheader2.ReadOnly = True
        Me.gridheader2.RowHeadersVisible = False
        Me.gridheader2.RowTemplate.Height = 21
        Me.gridheader2.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridheader2.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.gridheader2.Size = New System.Drawing.Size(1028, 24)
        Me.gridheader2.TabIndex = 1
        '
        'gridheader1
        '
        Me.gridheader1.AllowUserToAddRows = False
        Me.gridheader1.AllowUserToDeleteRows = False
        Me.gridheader1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridheader1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.gridheader1.Dock = System.Windows.Forms.DockStyle.Top
        Me.gridheader1.Enabled = False
        Me.gridheader1.Location = New System.Drawing.Point(0, 25)
        Me.gridheader1.Name = "gridheader1"
        Me.gridheader1.ReadOnly = True
        Me.gridheader1.RowHeadersVisible = False
        Me.gridheader1.RowTemplate.Height = 21
        Me.gridheader1.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridheader1.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.gridheader1.Size = New System.Drawing.Size(1028, 24)
        Me.gridheader1.TabIndex = 0
        '
        'pnlHeading
        '
        Me.pnlHeading.Controls.Add(Me.lblTitle)
        Me.pnlHeading.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlHeading.Location = New System.Drawing.Point(0, 0)
        Me.pnlHeading.Name = "pnlHeading"
        Me.pnlHeading.Size = New System.Drawing.Size(1028, 25)
        Me.pnlHeading.TabIndex = 0
        '
        'lblTitle
        '
        Me.lblTitle.BackColor = System.Drawing.SystemColors.Control
        Me.lblTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(1028, 25)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "Title"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnView_Search
        '
        Me.btnView_Search.Location = New System.Drawing.Point(438, 84)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(100, 30)
        Me.btnView_Search.TabIndex = 19
        Me.btnView_Search.Text = "&View"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(748, 84)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 22
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(540, 84)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 20
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(12, 7)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(79, 21)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Date From"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(12, 33)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(79, 21)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Cost Centre"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(185, 10)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(23, 21)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "To"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(642, 84)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 21
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.SystemColors.Control
        Me.Panel1.Controls.Add(Me.rbtcategorywise)
        Me.Panel1.Controls.Add(Me.rbtmonthwise)
        Me.Panel1.Controls.Add(Me.chkcmbmetal)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.chkwithtot)
        Me.Panel1.Controls.Add(Me.chkwithsr)
        Me.Panel1.Controls.Add(Me.chkreceipt)
        Me.Panel1.Controls.Add(Me.chksales)
        Me.Panel1.Controls.Add(Me.chkCmbCostCentre)
        Me.Panel1.Controls.Add(Me.dtpTo)
        Me.Panel1.Controls.Add(Me.dtpFrom)
        Me.Panel1.Controls.Add(Me.chkamt)
        Me.Panel1.Controls.Add(Me.chkGrsWt)
        Me.Panel1.Controls.Add(Me.chkPcs)
        Me.Panel1.Controls.Add(Me.btnExport)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.btnNew)
        Me.Panel1.Controls.Add(Me.btnExit)
        Me.Panel1.Controls.Add(Me.btnView_Search)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1028, 113)
        Me.Panel1.TabIndex = 0
        '
        'rbtcategorywise
        '
        Me.rbtcategorywise.AutoSize = True
        Me.rbtcategorywise.Location = New System.Drawing.Point(397, 56)
        Me.rbtcategorywise.Name = "rbtcategorywise"
        Me.rbtcategorywise.Size = New System.Drawing.Size(109, 17)
        Me.rbtcategorywise.TabIndex = 18
        Me.rbtcategorywise.TabStop = True
        Me.rbtcategorywise.Text = "Category Wise"
        Me.rbtcategorywise.UseVisualStyleBackColor = True
        '
        'rbtmonthwise
        '
        Me.rbtmonthwise.AutoSize = True
        Me.rbtmonthwise.Location = New System.Drawing.Point(312, 56)
        Me.rbtmonthwise.Name = "rbtmonthwise"
        Me.rbtmonthwise.Size = New System.Drawing.Size(86, 17)
        Me.rbtmonthwise.TabIndex = 17
        Me.rbtmonthwise.TabStop = True
        Me.rbtmonthwise.Text = "Metal Wise"
        Me.rbtmonthwise.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(13, 57)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(73, 21)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Metal"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkwithtot
        '
        Me.chkwithtot.AutoSize = True
        Me.chkwithtot.Checked = True
        Me.chkwithtot.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkwithtot.Location = New System.Drawing.Point(445, 33)
        Me.chkwithtot.Name = "chkwithtot"
        Me.chkwithtot.Size = New System.Drawing.Size(83, 17)
        Me.chkwithtot.TabIndex = 15
        Me.chkwithtot.Text = "With Total"
        Me.chkwithtot.UseVisualStyleBackColor = True
        Me.chkwithtot.UseWaitCursor = True
        '
        'chkwithsr
        '
        Me.chkwithsr.AutoSize = True
        Me.chkwithsr.Location = New System.Drawing.Point(312, 33)
        Me.chkwithsr.Name = "chkwithsr"
        Me.chkwithsr.Size = New System.Drawing.Size(128, 17)
        Me.chkwithsr.TabIndex = 14
        Me.chkwithsr.Text = "With Sales Retrun"
        Me.chkwithsr.UseVisualStyleBackColor = True
        '
        'chkreceipt
        '
        Me.chkreceipt.AutoSize = True
        Me.chkreceipt.Location = New System.Drawing.Point(540, 12)
        Me.chkreceipt.Name = "chkreceipt"
        Me.chkreceipt.Size = New System.Drawing.Size(78, 17)
        Me.chkreceipt.TabIndex = 12
        Me.chkreceipt.Text = "Purchase"
        Me.chkreceipt.UseVisualStyleBackColor = True
        '
        'chksales
        '
        Me.chksales.AutoSize = True
        Me.chksales.Checked = True
        Me.chksales.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chksales.Location = New System.Drawing.Point(481, 12)
        Me.chksales.Name = "chksales"
        Me.chksales.Size = New System.Drawing.Size(57, 17)
        Me.chksales.TabIndex = 11
        Me.chksales.Text = "Sales"
        Me.chksales.UseVisualStyleBackColor = True
        '
        'chkamt
        '
        Me.chkamt.AutoSize = True
        Me.chkamt.Checked = True
        Me.chkamt.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkamt.Location = New System.Drawing.Point(411, 12)
        Me.chkamt.Name = "chkamt"
        Me.chkamt.Size = New System.Drawing.Size(70, 17)
        Me.chkamt.TabIndex = 10
        Me.chkamt.Text = "Amount"
        Me.chkamt.UseVisualStyleBackColor = True
        '
        'chkGrsWt
        '
        Me.chkGrsWt.AutoSize = True
        Me.chkGrsWt.Checked = True
        Me.chkGrsWt.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkGrsWt.Location = New System.Drawing.Point(353, 12)
        Me.chkGrsWt.Name = "chkGrsWt"
        Me.chkGrsWt.Size = New System.Drawing.Size(59, 17)
        Me.chkGrsWt.TabIndex = 9
        Me.chkGrsWt.Text = "Grswt"
        Me.chkGrsWt.UseVisualStyleBackColor = True
        '
        'chkPcs
        '
        Me.chkPcs.AutoSize = True
        Me.chkPcs.Checked = True
        Me.chkPcs.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkPcs.Location = New System.Drawing.Point(312, 12)
        Me.chkPcs.Name = "chkPcs"
        Me.chkPcs.Size = New System.Drawing.Size(45, 17)
        Me.chkPcs.TabIndex = 8
        Me.chkPcs.Text = "Pcs"
        Me.chkPcs.UseVisualStyleBackColor = True
        '
        'chkcmbmetal
        '
        Me.chkcmbmetal.CheckOnClick = True
        Me.chkcmbmetal.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkcmbmetal.DropDownHeight = 1
        Me.chkcmbmetal.FormattingEnabled = True
        Me.chkcmbmetal.IntegralHeight = False
        Me.chkcmbmetal.Location = New System.Drawing.Point(88, 57)
        Me.chkcmbmetal.Name = "chkcmbmetal"
        Me.chkcmbmetal.Size = New System.Drawing.Size(218, 22)
        Me.chkcmbmetal.TabIndex = 7
        Me.chkcmbmetal.ValueSeparator = ", "
        '
        'chkCmbCostCentre
        '
        Me.chkCmbCostCentre.CheckOnClick = True
        Me.chkCmbCostCentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCostCentre.DropDownHeight = 1
        Me.chkCmbCostCentre.FormattingEnabled = True
        Me.chkCmbCostCentre.IntegralHeight = False
        Me.chkCmbCostCentre.Location = New System.Drawing.Point(88, 33)
        Me.chkCmbCostCentre.Name = "chkCmbCostCentre"
        Me.chkCmbCostCentre.Size = New System.Drawing.Size(218, 22)
        Me.chkCmbCostCentre.TabIndex = 5
        Me.chkCmbCostCentre.ValueSeparator = ", "
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(210, 10)
        Me.dtpTo.Mask = "##/##/####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(96, 21)
        Me.dtpTo.TabIndex = 3
        Me.dtpTo.Text = "07/03/9998"
        Me.dtpTo.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(88, 10)
        Me.dtpFrom.Mask = "##/##/####"
        Me.dtpFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom.Size = New System.Drawing.Size(96, 21)
        Me.dtpFrom.TabIndex = 1
        Me.dtpFrom.Text = "01/01/9998"
        Me.dtpFrom.Value = New Date(9998, 1, 1, 0, 0, 0, 0)
        '
        'frmSalesAbstractMonthwise
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1028, 676)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmSalesAbstractMonthwise"
        Me.Text = "SALES & PURCHASE ABSTRACT"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.Panel2.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        CType(Me.gridheader2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridheader1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlHeading.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents pnlHeading As System.Windows.Forms.Panel
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExtToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnView_Search As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents chkCmbCostCentre As BrighttechPack.CheckedComboBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents chkamt As System.Windows.Forms.CheckBox
    Friend WithEvents chkGrsWt As System.Windows.Forms.CheckBox
    Friend WithEvents chkPcs As System.Windows.Forms.CheckBox
    Friend WithEvents gridheader2 As System.Windows.Forms.DataGridView
    Friend WithEvents gridheader1 As System.Windows.Forms.DataGridView
    Friend WithEvents ResizeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents chkreceipt As System.Windows.Forms.CheckBox
    Friend WithEvents chksales As System.Windows.Forms.CheckBox
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents chkwithsr As System.Windows.Forms.CheckBox
    Friend WithEvents chkwithtot As System.Windows.Forms.CheckBox
    Friend WithEvents chkcmbmetal As BrighttechPack.CheckedComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents rbtcategorywise As System.Windows.Forms.RadioButton
    Friend WithEvents rbtmonthwise As System.Windows.Forms.RadioButton
End Class
