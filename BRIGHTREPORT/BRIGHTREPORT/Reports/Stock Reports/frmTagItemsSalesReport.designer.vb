<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTagItemsSalesReport
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
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.grpContainer = New System.Windows.Forms.GroupBox
        Me.chkLstItem = New System.Windows.Forms.CheckedListBox
        Me.chkItemSelectAll = New System.Windows.Forms.CheckBox
        Me.chkItemCounterSelectAll = New System.Windows.Forms.CheckBox
        Me.chkLstCostCentre = New System.Windows.Forms.CheckedListBox
        Me.chkMetalSelectAll = New System.Windows.Forms.CheckBox
        Me.chkLstMetal = New System.Windows.Forms.CheckedListBox
        Me.chkLstItemCounter = New System.Windows.Forms.CheckedListBox
        Me.chkCostCentreSelectAll = New System.Windows.Forms.CheckBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.btnSearch = New System.Windows.Forms.Button
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.ContextMenuStrip1.SuspendLayout()
        Me.grpContainer.SuspendLayout()
        Me.SuspendLayout()
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
        Me.NewToolStripMenuItem.Visible = False
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'grpContainer
        '
        Me.grpContainer.Controls.Add(Me.dtpTo)
        Me.grpContainer.Controls.Add(Me.dtpFrom)
        Me.grpContainer.Controls.Add(Me.chkLstItem)
        Me.grpContainer.Controls.Add(Me.chkItemSelectAll)
        Me.grpContainer.Controls.Add(Me.chkItemCounterSelectAll)
        Me.grpContainer.Controls.Add(Me.chkLstCostCentre)
        Me.grpContainer.Controls.Add(Me.chkMetalSelectAll)
        Me.grpContainer.Controls.Add(Me.chkLstMetal)
        Me.grpContainer.Controls.Add(Me.chkLstItemCounter)
        Me.grpContainer.Controls.Add(Me.chkCostCentreSelectAll)
        Me.grpContainer.Controls.Add(Me.Label2)
        Me.grpContainer.Controls.Add(Me.Label1)
        Me.grpContainer.Controls.Add(Me.btnSearch)
        Me.grpContainer.Controls.Add(Me.btnExit)
        Me.grpContainer.Controls.Add(Me.btnNew)
        Me.grpContainer.Location = New System.Drawing.Point(209, 79)
        Me.grpContainer.Name = "grpContainer"
        Me.grpContainer.Size = New System.Drawing.Size(430, 316)
        Me.grpContainer.TabIndex = 0
        Me.grpContainer.TabStop = False
        '
        'chkLstItem
        '
        Me.chkLstItem.FormattingEnabled = True
        Me.chkLstItem.Location = New System.Drawing.Point(9, 151)
        Me.chkLstItem.Name = "chkLstItem"
        Me.chkLstItem.Size = New System.Drawing.Size(201, 116)
        Me.chkLstItem.TabIndex = 9
        '
        'chkItemSelectAll
        '
        Me.chkItemSelectAll.AutoSize = True
        Me.chkItemSelectAll.Location = New System.Drawing.Point(12, 133)
        Me.chkItemSelectAll.Name = "chkItemSelectAll"
        Me.chkItemSelectAll.Size = New System.Drawing.Size(53, 17)
        Me.chkItemSelectAll.TabIndex = 8
        Me.chkItemSelectAll.Text = "Item"
        Me.chkItemSelectAll.UseVisualStyleBackColor = True
        '
        'chkItemCounterSelectAll
        '
        Me.chkItemCounterSelectAll.AutoSize = True
        Me.chkItemCounterSelectAll.Location = New System.Drawing.Point(223, 133)
        Me.chkItemCounterSelectAll.Name = "chkItemCounterSelectAll"
        Me.chkItemCounterSelectAll.Size = New System.Drawing.Size(103, 17)
        Me.chkItemCounterSelectAll.TabIndex = 10
        Me.chkItemCounterSelectAll.Text = "Item Counter"
        Me.chkItemCounterSelectAll.UseVisualStyleBackColor = True
        '
        'chkLstCostCentre
        '
        Me.chkLstCostCentre.FormattingEnabled = True
        Me.chkLstCostCentre.Location = New System.Drawing.Point(9, 59)
        Me.chkLstCostCentre.Name = "chkLstCostCentre"
        Me.chkLstCostCentre.Size = New System.Drawing.Size(201, 68)
        Me.chkLstCostCentre.TabIndex = 5
        '
        'chkMetalSelectAll
        '
        Me.chkMetalSelectAll.AutoSize = True
        Me.chkMetalSelectAll.Location = New System.Drawing.Point(220, 40)
        Me.chkMetalSelectAll.Name = "chkMetalSelectAll"
        Me.chkMetalSelectAll.Size = New System.Drawing.Size(56, 17)
        Me.chkMetalSelectAll.TabIndex = 6
        Me.chkMetalSelectAll.Text = "Metal"
        Me.chkMetalSelectAll.UseVisualStyleBackColor = True
        '
        'chkLstMetal
        '
        Me.chkLstMetal.FormattingEnabled = True
        Me.chkLstMetal.Location = New System.Drawing.Point(220, 59)
        Me.chkLstMetal.Name = "chkLstMetal"
        Me.chkLstMetal.Size = New System.Drawing.Size(199, 68)
        Me.chkLstMetal.TabIndex = 7
        '
        'chkLstItemCounter
        '
        Me.chkLstItemCounter.FormattingEnabled = True
        Me.chkLstItemCounter.Location = New System.Drawing.Point(220, 151)
        Me.chkLstItemCounter.Name = "chkLstItemCounter"
        Me.chkLstItemCounter.Size = New System.Drawing.Size(199, 116)
        Me.chkLstItemCounter.TabIndex = 11
        '
        'chkCostCentreSelectAll
        '
        Me.chkCostCentreSelectAll.AutoSize = True
        Me.chkCostCentreSelectAll.Location = New System.Drawing.Point(12, 40)
        Me.chkCostCentreSelectAll.Name = "chkCostCentreSelectAll"
        Me.chkCostCentreSelectAll.Size = New System.Drawing.Size(95, 17)
        Me.chkCostCentreSelectAll.TabIndex = 4
        Me.chkCostCentreSelectAll.Text = "Cost Centre"
        Me.chkCostCentreSelectAll.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(192, 17)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(21, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "To"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(18, 17)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(67, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Date From"
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(9, 273)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 12
        Me.btnSearch.Text = "&Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(221, 273)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 14
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(115, 273)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 13
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(220, 13)
        Me.dtpTo.Mask = "##/##/####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(93, 21)
        Me.dtpTo.TabIndex = 3
        Me.dtpTo.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(91, 13)
        Me.dtpFrom.Mask = "##/##/####"
        Me.dtpFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom.Size = New System.Drawing.Size(93, 21)
        Me.dtpFrom.TabIndex = 1
        Me.dtpFrom.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'frmTagItemsSalesReport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(868, 533)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.grpContainer)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmTagItemsSalesReport"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Taged Items Sales Report"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.grpContainer.ResumeLayout(False)
        Me.grpContainer.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents grpContainer As System.Windows.Forms.GroupBox
    Friend WithEvents chkLstItem As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkItemSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkItemCounterSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkLstCostCentre As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkMetalSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkLstMetal As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkLstItemCounter As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkCostCentreSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
End Class
