<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmOrderDetailedReport
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
        Me.components = New System.ComponentModel.Container()
        Me.grpContainer = New System.Windows.Forms.GroupBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.rbtboth = New System.Windows.Forms.RadioButton()
        Me.rbtRepair = New System.Windows.Forms.RadioButton()
        Me.rbtorder = New System.Windows.Forms.RadioButton()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.chkCmbItem = New BrighttechPack.CheckedComboBox()
        Me.chkCmbMetal = New BrighttechPack.CheckedComboBox()
        Me.chkCmbCostCentre = New BrighttechPack.CheckedComboBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tabMain = New System.Windows.Forms.TabControl()
        Me.tabView = New System.Windows.Forms.TabPage()
        Me.gridView = New System.Windows.Forms.DataGridView()
        Me.TabGeneral = New System.Windows.Forms.TabPage()
        Me.chkSpecificFormat = New System.Windows.Forms.CheckBox()
        Me.pnlSpecific = New System.Windows.Forms.Panel()
        Me.rbtDueDate = New System.Windows.Forms.RadioButton()
        Me.rbtOrdate = New System.Windows.Forms.RadioButton()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.grpContainer.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.tabMain.SuspendLayout()
        Me.tabView.SuspendLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabGeneral.SuspendLayout()
        Me.pnlSpecific.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpContainer
        '
        Me.grpContainer.Controls.Add(Me.pnlSpecific)
        Me.grpContainer.Controls.Add(Me.chkSpecificFormat)
        Me.grpContainer.Controls.Add(Me.Panel1)
        Me.grpContainer.Controls.Add(Me.Label15)
        Me.grpContainer.Controls.Add(Me.chkCmbItem)
        Me.grpContainer.Controls.Add(Me.chkCmbMetal)
        Me.grpContainer.Controls.Add(Me.chkCmbCostCentre)
        Me.grpContainer.Controls.Add(Me.Label9)
        Me.grpContainer.Controls.Add(Me.Label8)
        Me.grpContainer.Controls.Add(Me.Label3)
        Me.grpContainer.Controls.Add(Me.dtpTo)
        Me.grpContainer.Controls.Add(Me.dtpFrom)
        Me.grpContainer.Controls.Add(Me.btnSearch)
        Me.grpContainer.Controls.Add(Me.btnExit)
        Me.grpContainer.Controls.Add(Me.btnNew)
        Me.grpContainer.Controls.Add(Me.Label2)
        Me.grpContainer.Location = New System.Drawing.Point(279, 174)
        Me.grpContainer.Name = "grpContainer"
        Me.grpContainer.Size = New System.Drawing.Size(457, 267)
        Me.grpContainer.TabIndex = 0
        Me.grpContainer.TabStop = False
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.rbtboth)
        Me.Panel1.Controls.Add(Me.rbtRepair)
        Me.Panel1.Controls.Add(Me.rbtorder)
        Me.Panel1.Location = New System.Drawing.Point(131, 151)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(290, 21)
        Me.Panel1.TabIndex = 10
        '
        'rbtboth
        '
        Me.rbtboth.AutoSize = True
        Me.rbtboth.Checked = True
        Me.rbtboth.Location = New System.Drawing.Point(3, 2)
        Me.rbtboth.Name = "rbtboth"
        Me.rbtboth.Size = New System.Drawing.Size(51, 17)
        Me.rbtboth.TabIndex = 0
        Me.rbtboth.TabStop = True
        Me.rbtboth.Text = "Both"
        Me.rbtboth.UseVisualStyleBackColor = True
        '
        'rbtRepair
        '
        Me.rbtRepair.AutoSize = True
        Me.rbtRepair.Location = New System.Drawing.Point(129, 2)
        Me.rbtRepair.Name = "rbtRepair"
        Me.rbtRepair.Size = New System.Drawing.Size(62, 17)
        Me.rbtRepair.TabIndex = 2
        Me.rbtRepair.Text = "Repair"
        Me.rbtRepair.UseVisualStyleBackColor = True
        '
        'rbtorder
        '
        Me.rbtorder.AutoSize = True
        Me.rbtorder.Location = New System.Drawing.Point(60, 3)
        Me.rbtorder.Name = "rbtorder"
        Me.rbtorder.Size = New System.Drawing.Size(58, 17)
        Me.rbtorder.TabIndex = 1
        Me.rbtorder.Text = "Order"
        Me.rbtorder.UseVisualStyleBackColor = True
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(90, 38)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(36, 13)
        Me.Label15.TabIndex = 0
        Me.Label15.Text = "From"
        '
        'chkCmbItem
        '
        Me.chkCmbItem.CheckOnClick = True
        Me.chkCmbItem.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbItem.DropDownHeight = 1
        Me.chkCmbItem.FormattingEnabled = True
        Me.chkCmbItem.IntegralHeight = False
        Me.chkCmbItem.Location = New System.Drawing.Point(131, 121)
        Me.chkCmbItem.Name = "chkCmbItem"
        Me.chkCmbItem.Size = New System.Drawing.Size(290, 22)
        Me.chkCmbItem.TabIndex = 9
        Me.chkCmbItem.ValueSeparator = ", "
        '
        'chkCmbMetal
        '
        Me.chkCmbMetal.CheckOnClick = True
        Me.chkCmbMetal.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbMetal.DropDownHeight = 1
        Me.chkCmbMetal.FormattingEnabled = True
        Me.chkCmbMetal.IntegralHeight = False
        Me.chkCmbMetal.Location = New System.Drawing.Point(131, 92)
        Me.chkCmbMetal.Name = "chkCmbMetal"
        Me.chkCmbMetal.Size = New System.Drawing.Size(290, 22)
        Me.chkCmbMetal.TabIndex = 7
        Me.chkCmbMetal.ValueSeparator = ", "
        '
        'chkCmbCostCentre
        '
        Me.chkCmbCostCentre.CheckOnClick = True
        Me.chkCmbCostCentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCostCentre.DropDownHeight = 1
        Me.chkCmbCostCentre.FormattingEnabled = True
        Me.chkCmbCostCentre.IntegralHeight = False
        Me.chkCmbCostCentre.Location = New System.Drawing.Point(131, 64)
        Me.chkCmbCostCentre.Name = "chkCmbCostCentre"
        Me.chkCmbCostCentre.Size = New System.Drawing.Size(290, 22)
        Me.chkCmbCostCentre.TabIndex = 5
        Me.chkCmbCostCentre.ValueSeparator = ", "
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(92, 126)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(34, 13)
        Me.Label9.TabIndex = 8
        Me.Label9.Text = "Item"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(89, 97)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(37, 13)
        Me.Label8.TabIndex = 6
        Me.Label8.Text = "Metal"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(50, 69)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(76, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Cost Centre"
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(313, 37)
        Me.dtpTo.Mask = "##/##/####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(108, 21)
        Me.dtpTo.TabIndex = 3
        Me.dtpTo.Text = "07/03/9998"
        Me.dtpTo.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(131, 38)
        Me.dtpFrom.Mask = "##/##/####"
        Me.dtpFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom.Size = New System.Drawing.Size(108, 21)
        Me.dtpFrom.TabIndex = 1
        Me.dtpFrom.Text = "07/03/9998"
        Me.dtpFrom.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(121, 216)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 11
        Me.btnSearch.Tag = "21"
        Me.btnSearch.Text = "&Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(333, 216)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 13
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(227, 216)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 12
        Me.btnNew.Tag = "22"
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(261, 40)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(20, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "To"
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(118, 48)
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'tabMain
        '
        Me.tabMain.Controls.Add(Me.tabView)
        Me.tabMain.Controls.Add(Me.TabGeneral)
        Me.tabMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabMain.Location = New System.Drawing.Point(0, 0)
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(1022, 640)
        Me.tabMain.TabIndex = 0
        '
        'tabView
        '
        Me.tabView.Controls.Add(Me.gridView)
        Me.tabView.Location = New System.Drawing.Point(4, 22)
        Me.tabView.Name = "tabView"
        Me.tabView.Padding = New System.Windows.Forms.Padding(3)
        Me.tabView.Size = New System.Drawing.Size(1014, 614)
        Me.tabView.TabIndex = 1
        Me.tabView.Text = "View"
        Me.tabView.UseVisualStyleBackColor = True
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Location = New System.Drawing.Point(31, 30)
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.Size = New System.Drawing.Size(957, 459)
        Me.gridView.TabIndex = 0
        '
        'TabGeneral
        '
        Me.TabGeneral.Controls.Add(Me.grpContainer)
        Me.TabGeneral.Location = New System.Drawing.Point(4, 22)
        Me.TabGeneral.Name = "TabGeneral"
        Me.TabGeneral.Padding = New System.Windows.Forms.Padding(3)
        Me.TabGeneral.Size = New System.Drawing.Size(1014, 614)
        Me.TabGeneral.TabIndex = 0
        Me.TabGeneral.Text = "General"
        Me.TabGeneral.UseVisualStyleBackColor = True
        '
        'chkSpecificFormat
        '
        Me.chkSpecificFormat.AutoSize = True
        Me.chkSpecificFormat.Location = New System.Drawing.Point(12, 180)
        Me.chkSpecificFormat.Name = "chkSpecificFormat"
        Me.chkSpecificFormat.Size = New System.Drawing.Size(114, 17)
        Me.chkSpecificFormat.TabIndex = 14
        Me.chkSpecificFormat.Text = "Specific Format"
        Me.chkSpecificFormat.UseVisualStyleBackColor = True
        '
        'pnlSpecific
        '
        Me.pnlSpecific.Controls.Add(Me.Label1)
        Me.pnlSpecific.Controls.Add(Me.rbtDueDate)
        Me.pnlSpecific.Controls.Add(Me.rbtOrdate)
        Me.pnlSpecific.Location = New System.Drawing.Point(131, 178)
        Me.pnlSpecific.Name = "pnlSpecific"
        Me.pnlSpecific.Size = New System.Drawing.Size(290, 21)
        Me.pnlSpecific.TabIndex = 11
        '
        'rbtDueDate
        '
        Me.rbtDueDate.AutoSize = True
        Me.rbtDueDate.Location = New System.Drawing.Point(177, 3)
        Me.rbtDueDate.Name = "rbtDueDate"
        Me.rbtDueDate.Size = New System.Drawing.Size(77, 17)
        Me.rbtDueDate.TabIndex = 2
        Me.rbtDueDate.Text = "Due date"
        Me.rbtDueDate.UseVisualStyleBackColor = True
        '
        'rbtOrdate
        '
        Me.rbtOrdate.AutoSize = True
        Me.rbtOrdate.Checked = True
        Me.rbtOrdate.Location = New System.Drawing.Point(76, 3)
        Me.rbtOrdate.Name = "rbtOrdate"
        Me.rbtOrdate.Size = New System.Drawing.Size(89, 17)
        Me.rbtOrdate.TabIndex = 1
        Me.rbtOrdate.TabStop = True
        Me.rbtOrdate.Text = "Order Date"
        Me.rbtOrdate.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(3, 5)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(60, 13)
        Me.Label1.TabIndex = 15
        Me.Label1.Text = "Based on"
        '
        'frmOrderDetailedReport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1022, 640)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.tabMain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmOrderDetailedReport"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Order\Repair Detailed Report"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.grpContainer.ResumeLayout(False)
        Me.grpContainer.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.tabMain.ResumeLayout(False)
        Me.tabView.ResumeLayout(False)
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabGeneral.ResumeLayout(False)
        Me.pnlSpecific.ResumeLayout(False)
        Me.pnlSpecific.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpContainer As System.Windows.Forms.GroupBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tabMain As System.Windows.Forms.TabControl
    Friend WithEvents TabGeneral As System.Windows.Forms.TabPage
    Friend WithEvents tabView As System.Windows.Forms.TabPage
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents chkCmbCostCentre As BrighttechPack.CheckedComboBox
    Friend WithEvents chkCmbMetal As BrighttechPack.CheckedComboBox
    Friend WithEvents chkCmbItem As BrighttechPack.CheckedComboBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents rbtboth As System.Windows.Forms.RadioButton
    Friend WithEvents rbtRepair As System.Windows.Forms.RadioButton
    Friend WithEvents rbtorder As System.Windows.Forms.RadioButton
    Friend WithEvents chkSpecificFormat As CheckBox
    Friend WithEvents pnlSpecific As Panel
    Friend WithEvents Label1 As Label
    Friend WithEvents rbtDueDate As RadioButton
    Friend WithEvents rbtOrdate As RadioButton
End Class
