<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDateWiseStock
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
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExportToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.grpmain = New System.Windows.Forms.GroupBox
        Me.lblTitle = New System.Windows.Forms.Label
        Me.chkcmbMetalType = New BrighttechPack.CheckedComboBox
        Me.btnPrint = New System.Windows.Forms.Button
        Me.btnExport = New System.Windows.Forms.Button
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.btnView_Search = New System.Windows.Forms.Button
        Me.chkCmbCostCentre = New BrighttechPack.CheckedComboBox
        Me.Label = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.chkWithGrsWt = New System.Windows.Forms.CheckBox
        Me.chkWithPurWt = New System.Windows.Forms.CheckBox
        Me.chkWithNetWt = New System.Windows.Forms.CheckBox
        Me.chkcmbCategory = New BrighttechPack.CheckedComboBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.chkCmbCompany = New BrighttechPack.CheckedComboBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.Label5 = New System.Windows.Forms.Label
        Me.gridViewHead = New System.Windows.Forms.DataGridView
        Me.gridView = New System.Windows.Forms.DataGridView
        Me.ContextMenuStrip1.SuspendLayout()
        Me.grpmain.SuspendLayout()
        CType(Me.gridViewHead, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.ExportToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(129, 70)
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ExportToolStripMenuItem
        '
        Me.ExportToolStripMenuItem.Name = "ExportToolStripMenuItem"
        Me.ExportToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.ExportToolStripMenuItem.Text = "Export"
        Me.ExportToolStripMenuItem.Visible = False
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'grpmain
        '
        Me.grpmain.Controls.Add(Me.lblTitle)
        Me.grpmain.Controls.Add(Me.chkcmbMetalType)
        Me.grpmain.Controls.Add(Me.btnPrint)
        Me.grpmain.Controls.Add(Me.btnExport)
        Me.grpmain.Controls.Add(Me.btnExit)
        Me.grpmain.Controls.Add(Me.btnNew)
        Me.grpmain.Controls.Add(Me.btnView_Search)
        Me.grpmain.Controls.Add(Me.chkCmbCostCentre)
        Me.grpmain.Controls.Add(Me.Label)
        Me.grpmain.Controls.Add(Me.Label3)
        Me.grpmain.Controls.Add(Me.chkWithGrsWt)
        Me.grpmain.Controls.Add(Me.chkWithPurWt)
        Me.grpmain.Controls.Add(Me.chkWithNetWt)
        Me.grpmain.Controls.Add(Me.chkcmbCategory)
        Me.grpmain.Controls.Add(Me.Label8)
        Me.grpmain.Controls.Add(Me.Label6)
        Me.grpmain.Controls.Add(Me.chkCmbCompany)
        Me.grpmain.Controls.Add(Me.Label9)
        Me.grpmain.Controls.Add(Me.dtpTo)
        Me.grpmain.Controls.Add(Me.dtpFrom)
        Me.grpmain.Controls.Add(Me.Label5)
        Me.grpmain.Dock = System.Windows.Forms.DockStyle.Top
        Me.grpmain.Location = New System.Drawing.Point(0, 0)
        Me.grpmain.Name = "grpmain"
        Me.grpmain.Size = New System.Drawing.Size(1024, 194)
        Me.grpmain.TabIndex = 0
        Me.grpmain.TabStop = False
        '
        'lblTitle
        '
        Me.lblTitle.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(0, 160)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(1017, 31)
        Me.lblTitle.TabIndex = 20
        Me.lblTitle.Text = "Title"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'chkcmbMetalType
        '
        Me.chkcmbMetalType.CheckOnClick = True
        Me.chkcmbMetalType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkcmbMetalType.DropDownHeight = 1
        Me.chkcmbMetalType.FormattingEnabled = True
        Me.chkcmbMetalType.IntegralHeight = False
        Me.chkcmbMetalType.Location = New System.Drawing.Point(532, 37)
        Me.chkcmbMetalType.Name = "chkcmbMetalType"
        Me.chkcmbMetalType.Size = New System.Drawing.Size(222, 21)
        Me.chkcmbMetalType.TabIndex = 7
        Me.chkcmbMetalType.ValueSeparator = ", "
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(557, 127)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 30)
        Me.btnPrint.TabIndex = 19
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(345, 127)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 17
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(451, 127)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 18
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(239, 127)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 16
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnView_Search
        '
        Me.btnView_Search.Location = New System.Drawing.Point(133, 127)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(100, 30)
        Me.btnView_Search.TabIndex = 15
        Me.btnView_Search.Text = "&View"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'chkCmbCostCentre
        '
        Me.chkCmbCostCentre.CheckOnClick = True
        Me.chkCmbCostCentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCostCentre.DropDownHeight = 1
        Me.chkCmbCostCentre.FormattingEnabled = True
        Me.chkCmbCostCentre.IntegralHeight = False
        Me.chkCmbCostCentre.Location = New System.Drawing.Point(532, 64)
        Me.chkCmbCostCentre.Name = "chkCmbCostCentre"
        Me.chkCmbCostCentre.Size = New System.Drawing.Size(222, 21)
        Me.chkCmbCostCentre.TabIndex = 11
        Me.chkCmbCostCentre.ValueSeparator = ", "
        '
        'Label
        '
        Me.Label.AutoSize = True
        Me.Label.Location = New System.Drawing.Point(411, 69)
        Me.Label.Name = "Label"
        Me.Label.Size = New System.Drawing.Size(62, 13)
        Me.Label.TabIndex = 10
        Me.Label.Text = "Cost Centre"
        Me.Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(411, 38)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(115, 21)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Metal Type"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkWithGrsWt
        '
        Me.chkWithGrsWt.AutoSize = True
        Me.chkWithGrsWt.Location = New System.Drawing.Point(133, 104)
        Me.chkWithGrsWt.Name = "chkWithGrsWt"
        Me.chkWithGrsWt.Size = New System.Drawing.Size(90, 17)
        Me.chkWithGrsWt.TabIndex = 12
        Me.chkWithGrsWt.Text = "Gross Weight"
        Me.chkWithGrsWt.UseVisualStyleBackColor = True
        '
        'chkWithPurWt
        '
        Me.chkWithPurWt.AutoSize = True
        Me.chkWithPurWt.Location = New System.Drawing.Point(244, 104)
        Me.chkWithPurWt.Name = "chkWithPurWt"
        Me.chkWithPurWt.Size = New System.Drawing.Size(85, 17)
        Me.chkWithPurWt.TabIndex = 13
        Me.chkWithPurWt.Text = "Pure Weight"
        Me.chkWithPurWt.UseVisualStyleBackColor = True
        '
        'chkWithNetWt
        '
        Me.chkWithNetWt.AutoSize = True
        Me.chkWithNetWt.Location = New System.Drawing.Point(350, 104)
        Me.chkWithNetWt.Name = "chkWithNetWt"
        Me.chkWithNetWt.Size = New System.Drawing.Size(80, 17)
        Me.chkWithNetWt.TabIndex = 14
        Me.chkWithNetWt.Text = "Net Weight"
        Me.chkWithNetWt.UseVisualStyleBackColor = True
        '
        'chkcmbCategory
        '
        Me.chkcmbCategory.CheckOnClick = True
        Me.chkcmbCategory.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkcmbCategory.DropDownHeight = 1
        Me.chkcmbCategory.FormattingEnabled = True
        Me.chkcmbCategory.IntegralHeight = False
        Me.chkcmbCategory.Location = New System.Drawing.Point(133, 68)
        Me.chkcmbCategory.Name = "chkcmbCategory"
        Me.chkcmbCategory.Size = New System.Drawing.Size(246, 21)
        Me.chkcmbCategory.TabIndex = 9
        Me.chkcmbCategory.ValueSeparator = ", "
        '
        'Label8
        '
        Me.Label8.Location = New System.Drawing.Point(12, 70)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(82, 21)
        Me.Label8.TabIndex = 8
        Me.Label8.Text = "Category"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(236, 16)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(25, 21)
        Me.Label6.TabIndex = 2
        Me.Label6.Text = "To"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkCmbCompany
        '
        Me.chkCmbCompany.CheckOnClick = True
        Me.chkCmbCompany.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCompany.DropDownHeight = 1
        Me.chkCmbCompany.FormattingEnabled = True
        Me.chkCmbCompany.IntegralHeight = False
        Me.chkCmbCompany.Location = New System.Drawing.Point(133, 42)
        Me.chkCmbCompany.Name = "chkCmbCompany"
        Me.chkCmbCompany.Size = New System.Drawing.Size(222, 21)
        Me.chkCmbCompany.TabIndex = 5
        Me.chkCmbCompany.ValueSeparator = ", "
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(12, 46)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(51, 13)
        Me.Label9.TabIndex = 4
        Me.Label9.Text = "Company"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(267, 17)
        Me.dtpTo.Mask = "##/##/####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(88, 20)
        Me.dtpTo.TabIndex = 3
        Me.dtpTo.Text = "07/03/9998"
        Me.dtpTo.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(133, 17)
        Me.dtpFrom.Mask = "##/##/####"
        Me.dtpFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom.Size = New System.Drawing.Size(88, 20)
        Me.dtpFrom.TabIndex = 1
        Me.dtpFrom.Text = "07/03/9998"
        Me.dtpFrom.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(12, 16)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(115, 21)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "Date From"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'gridViewHead
        '
        Me.gridViewHead.AllowUserToAddRows = False
        Me.gridViewHead.AllowUserToDeleteRows = False
        Me.gridViewHead.AllowUserToResizeColumns = False
        Me.gridViewHead.AllowUserToResizeRows = False
        Me.gridViewHead.BackgroundColor = System.Drawing.SystemColors.ControlDark
        Me.gridViewHead.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridViewHead.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridViewHead.Dock = System.Windows.Forms.DockStyle.Top
        Me.gridViewHead.Location = New System.Drawing.Point(0, 194)
        Me.gridViewHead.MultiSelect = False
        Me.gridViewHead.Name = "gridViewHead"
        Me.gridViewHead.ReadOnly = True
        Me.gridViewHead.RowHeadersVisible = False
        Me.gridViewHead.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridViewHead.Size = New System.Drawing.Size(1024, 18)
        Me.gridViewHead.StandardTab = True
        Me.gridViewHead.TabIndex = 2
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.gridView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView.Location = New System.Drawing.Point(0, 212)
        Me.gridView.Name = "gridView"
        Me.gridView.RowHeadersVisible = False
        Me.gridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.gridView.Size = New System.Drawing.Size(1024, 385)
        Me.gridView.TabIndex = 3
        '
        'frmDateWiseStock
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1024, 597)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.gridView)
        Me.Controls.Add(Me.gridViewHead)
        Me.Controls.Add(Me.grpmain)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmDateWiseStock"
        Me.Text = "DateWise Stock"
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.grpmain.ResumeLayout(False)
        Me.grpmain.PerformLayout()
        CType(Me.gridViewHead, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents grpmain As System.Windows.Forms.GroupBox
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnView_Search As System.Windows.Forms.Button
    Friend WithEvents chkCmbCostCentre As BrighttechPack.CheckedComboBox
    Friend WithEvents Label As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents chkWithGrsWt As System.Windows.Forms.CheckBox
    Friend WithEvents chkWithPurWt As System.Windows.Forms.CheckBox
    Friend WithEvents chkWithNetWt As System.Windows.Forms.CheckBox
    Friend WithEvents chkcmbCategory As BrighttechPack.CheckedComboBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents chkCmbCompany As BrighttechPack.CheckedComboBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents chkcmbMetalType As BrighttechPack.CheckedComboBox
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents gridViewHead As System.Windows.Forms.DataGridView
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExportToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class
