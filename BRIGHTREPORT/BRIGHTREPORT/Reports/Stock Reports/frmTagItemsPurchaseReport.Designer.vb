<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTagItemsPurchaseReport
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
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.grpContainer = New System.Windows.Forms.GroupBox()
        Me.PnlMark = New System.Windows.Forms.Panel()
        Me.rbtLocal = New System.Windows.Forms.RadioButton()
        Me.rbtOutstation = New System.Windows.Forms.RadioButton()
        Me.rbtBothDesigner = New System.Windows.Forms.RadioButton()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Rbgwt = New System.Windows.Forms.RadioButton()
        Me.rbnwt = New System.Windows.Forms.RadioButton()
        Me.chkWithStudded = New System.Windows.Forms.CheckBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.rbtDetailed = New System.Windows.Forms.RadioButton()
        Me.rbtSummary = New System.Windows.Forms.RadioButton()
        Me.dtpAsOnDate = New BrighttechPack.DatePicker(Me.components)
        Me.chkStockOnly = New System.Windows.Forms.CheckBox()
        Me.txtTranInvNo = New System.Windows.Forms.TextBox()
        Me.chkLstItem = New System.Windows.Forms.CheckedListBox()
        Me.chkItemSelectAll = New System.Windows.Forms.CheckBox()
        Me.chkLstDesigner = New System.Windows.Forms.CheckedListBox()
        Me.chkItemCounterSelectAll = New System.Windows.Forms.CheckBox()
        Me.chkDesignerSelectAll = New System.Windows.Forms.CheckBox()
        Me.chkLstCostCentre = New System.Windows.Forms.CheckedListBox()
        Me.chkMetalSelectAll = New System.Windows.Forms.CheckBox()
        Me.chkLstMetal = New System.Windows.Forms.CheckedListBox()
        Me.chkLstItemCounter = New System.Windows.Forms.CheckedListBox()
        Me.chkCostCentreSelectAll = New System.Windows.Forms.CheckBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.grpContainer.SuspendLayout()
        Me.PnlMark.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
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
        'grpContainer
        '
        Me.grpContainer.Controls.Add(Me.PnlMark)
        Me.grpContainer.Controls.Add(Me.GroupBox1)
        Me.grpContainer.Controls.Add(Me.chkWithStudded)
        Me.grpContainer.Controls.Add(Me.GroupBox2)
        Me.grpContainer.Controls.Add(Me.dtpAsOnDate)
        Me.grpContainer.Controls.Add(Me.chkStockOnly)
        Me.grpContainer.Controls.Add(Me.txtTranInvNo)
        Me.grpContainer.Controls.Add(Me.chkLstItem)
        Me.grpContainer.Controls.Add(Me.chkItemSelectAll)
        Me.grpContainer.Controls.Add(Me.chkLstDesigner)
        Me.grpContainer.Controls.Add(Me.chkItemCounterSelectAll)
        Me.grpContainer.Controls.Add(Me.chkDesignerSelectAll)
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
        Me.grpContainer.Location = New System.Drawing.Point(219, 6)
        Me.grpContainer.Name = "grpContainer"
        Me.grpContainer.Size = New System.Drawing.Size(430, 515)
        Me.grpContainer.TabIndex = 0
        Me.grpContainer.TabStop = False
        '
        'PnlMark
        '
        Me.PnlMark.Controls.Add(Me.rbtLocal)
        Me.PnlMark.Controls.Add(Me.rbtOutstation)
        Me.PnlMark.Controls.Add(Me.rbtBothDesigner)
        Me.PnlMark.Location = New System.Drawing.Point(138, 414)
        Me.PnlMark.Name = "PnlMark"
        Me.PnlMark.Size = New System.Drawing.Size(249, 29)
        Me.PnlMark.TabIndex = 17
        '
        'rbtLocal
        '
        Me.rbtLocal.AutoSize = True
        Me.rbtLocal.Location = New System.Drawing.Point(72, 4)
        Me.rbtLocal.Name = "rbtLocal"
        Me.rbtLocal.Size = New System.Drawing.Size(54, 17)
        Me.rbtLocal.TabIndex = 1
        Me.rbtLocal.Text = "Local"
        Me.rbtLocal.UseVisualStyleBackColor = True
        '
        'rbtOutstation
        '
        Me.rbtOutstation.AutoSize = True
        Me.rbtOutstation.Location = New System.Drawing.Point(142, 4)
        Me.rbtOutstation.Name = "rbtOutstation"
        Me.rbtOutstation.Size = New System.Drawing.Size(83, 17)
        Me.rbtOutstation.TabIndex = 2
        Me.rbtOutstation.Text = "Outstation"
        Me.rbtOutstation.UseVisualStyleBackColor = True
        '
        'rbtBothDesigner
        '
        Me.rbtBothDesigner.AutoSize = True
        Me.rbtBothDesigner.Checked = True
        Me.rbtBothDesigner.Location = New System.Drawing.Point(5, 3)
        Me.rbtBothDesigner.Name = "rbtBothDesigner"
        Me.rbtBothDesigner.Size = New System.Drawing.Size(51, 17)
        Me.rbtBothDesigner.TabIndex = 0
        Me.rbtBothDesigner.TabStop = True
        Me.rbtBothDesigner.Text = "Both"
        Me.rbtBothDesigner.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.Rbgwt)
        Me.GroupBox1.Controls.Add(Me.rbnwt)
        Me.GroupBox1.Location = New System.Drawing.Point(53, 439)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(336, 30)
        Me.GroupBox1.TabIndex = 18
        Me.GroupBox1.TabStop = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 9)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(110, 13)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Amount Based On"
        '
        'Rbgwt
        '
        Me.Rbgwt.Checked = True
        Me.Rbgwt.Location = New System.Drawing.Point(130, 9)
        Me.Rbgwt.Name = "Rbgwt"
        Me.Rbgwt.Size = New System.Drawing.Size(82, 18)
        Me.Rbgwt.TabIndex = 1
        Me.Rbgwt.TabStop = True
        Me.Rbgwt.Text = "GRS WT"
        '
        'rbnwt
        '
        Me.rbnwt.Location = New System.Drawing.Point(233, 9)
        Me.rbnwt.Name = "rbnwt"
        Me.rbnwt.Size = New System.Drawing.Size(86, 18)
        Me.rbnwt.TabIndex = 2
        Me.rbnwt.Text = "NET WT"
        '
        'chkWithStudded
        '
        Me.chkWithStudded.AutoSize = True
        Me.chkWithStudded.Checked = True
        Me.chkWithStudded.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkWithStudded.Location = New System.Drawing.Point(113, 383)
        Me.chkWithStudded.Name = "chkWithStudded"
        Me.chkWithStudded.Size = New System.Drawing.Size(110, 17)
        Me.chkWithStudded.TabIndex = 15
        Me.chkWithStudded.Text = "Studded Detail"
        Me.chkWithStudded.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.rbtDetailed)
        Me.GroupBox2.Controls.Add(Me.rbtSummary)
        Me.GroupBox2.Location = New System.Drawing.Point(231, 377)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(186, 31)
        Me.GroupBox2.TabIndex = 16
        Me.GroupBox2.TabStop = False
        '
        'rbtDetailed
        '
        Me.rbtDetailed.Checked = True
        Me.rbtDetailed.Location = New System.Drawing.Point(5, 12)
        Me.rbtDetailed.Name = "rbtDetailed"
        Me.rbtDetailed.Size = New System.Drawing.Size(82, 17)
        Me.rbtDetailed.TabIndex = 0
        Me.rbtDetailed.TabStop = True
        Me.rbtDetailed.Text = "Detail"
        '
        'rbtSummary
        '
        Me.rbtSummary.Location = New System.Drawing.Point(93, 12)
        Me.rbtSummary.Name = "rbtSummary"
        Me.rbtSummary.Size = New System.Drawing.Size(86, 18)
        Me.rbtSummary.TabIndex = 1
        Me.rbtSummary.Text = "Summary "
        '
        'dtpAsOnDate
        '
        Me.dtpAsOnDate.Location = New System.Drawing.Point(84, 13)
        Me.dtpAsOnDate.Mask = "##/##/####"
        Me.dtpAsOnDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpAsOnDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpAsOnDate.Name = "dtpAsOnDate"
        Me.dtpAsOnDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpAsOnDate.Size = New System.Drawing.Size(93, 21)
        Me.dtpAsOnDate.TabIndex = 1
        Me.dtpAsOnDate.Text = "07/03/9998"
        Me.dtpAsOnDate.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'chkStockOnly
        '
        Me.chkStockOnly.AutoSize = True
        Me.chkStockOnly.Location = New System.Drawing.Point(12, 383)
        Me.chkStockOnly.Name = "chkStockOnly"
        Me.chkStockOnly.Size = New System.Drawing.Size(88, 17)
        Me.chkStockOnly.TabIndex = 14
        Me.chkStockOnly.Text = "Stock Only"
        Me.chkStockOnly.UseVisualStyleBackColor = True
        '
        'txtTranInvNo
        '
        Me.txtTranInvNo.Location = New System.Drawing.Point(256, 13)
        Me.txtTranInvNo.Name = "txtTranInvNo"
        Me.txtTranInvNo.Size = New System.Drawing.Size(160, 21)
        Me.txtTranInvNo.TabIndex = 3
        '
        'chkLstItem
        '
        Me.chkLstItem.FormattingEnabled = True
        Me.chkLstItem.Location = New System.Drawing.Point(9, 275)
        Me.chkLstItem.Name = "chkLstItem"
        Me.chkLstItem.Size = New System.Drawing.Size(201, 100)
        Me.chkLstItem.TabIndex = 11
        '
        'chkItemSelectAll
        '
        Me.chkItemSelectAll.AutoSize = True
        Me.chkItemSelectAll.Location = New System.Drawing.Point(12, 257)
        Me.chkItemSelectAll.Name = "chkItemSelectAll"
        Me.chkItemSelectAll.Size = New System.Drawing.Size(53, 17)
        Me.chkItemSelectAll.TabIndex = 10
        Me.chkItemSelectAll.Text = "Item"
        Me.chkItemSelectAll.UseVisualStyleBackColor = True
        '
        'chkLstDesigner
        '
        Me.chkLstDesigner.FormattingEnabled = True
        Me.chkLstDesigner.Location = New System.Drawing.Point(9, 58)
        Me.chkLstDesigner.MultiColumn = True
        Me.chkLstDesigner.Name = "chkLstDesigner"
        Me.chkLstDesigner.Size = New System.Drawing.Size(410, 100)
        Me.chkLstDesigner.TabIndex = 5
        '
        'chkItemCounterSelectAll
        '
        Me.chkItemCounterSelectAll.AutoSize = True
        Me.chkItemCounterSelectAll.Location = New System.Drawing.Point(223, 257)
        Me.chkItemCounterSelectAll.Name = "chkItemCounterSelectAll"
        Me.chkItemCounterSelectAll.Size = New System.Drawing.Size(103, 17)
        Me.chkItemCounterSelectAll.TabIndex = 12
        Me.chkItemCounterSelectAll.Text = "Item Counter"
        Me.chkItemCounterSelectAll.UseVisualStyleBackColor = True
        '
        'chkDesignerSelectAll
        '
        Me.chkDesignerSelectAll.AutoSize = True
        Me.chkDesignerSelectAll.Location = New System.Drawing.Point(12, 40)
        Me.chkDesignerSelectAll.Name = "chkDesignerSelectAll"
        Me.chkDesignerSelectAll.Size = New System.Drawing.Size(77, 17)
        Me.chkDesignerSelectAll.TabIndex = 4
        Me.chkDesignerSelectAll.Text = "Designer"
        Me.chkDesignerSelectAll.UseVisualStyleBackColor = True
        '
        'chkLstCostCentre
        '
        Me.chkLstCostCentre.FormattingEnabled = True
        Me.chkLstCostCentre.Location = New System.Drawing.Point(9, 183)
        Me.chkLstCostCentre.Name = "chkLstCostCentre"
        Me.chkLstCostCentre.Size = New System.Drawing.Size(201, 68)
        Me.chkLstCostCentre.TabIndex = 7
        '
        'chkMetalSelectAll
        '
        Me.chkMetalSelectAll.AutoSize = True
        Me.chkMetalSelectAll.Location = New System.Drawing.Point(223, 164)
        Me.chkMetalSelectAll.Name = "chkMetalSelectAll"
        Me.chkMetalSelectAll.Size = New System.Drawing.Size(56, 17)
        Me.chkMetalSelectAll.TabIndex = 8
        Me.chkMetalSelectAll.Text = "Metal"
        Me.chkMetalSelectAll.UseVisualStyleBackColor = True
        '
        'chkLstMetal
        '
        Me.chkLstMetal.FormattingEnabled = True
        Me.chkLstMetal.Location = New System.Drawing.Point(220, 183)
        Me.chkLstMetal.Name = "chkLstMetal"
        Me.chkLstMetal.Size = New System.Drawing.Size(199, 68)
        Me.chkLstMetal.TabIndex = 9
        '
        'chkLstItemCounter
        '
        Me.chkLstItemCounter.FormattingEnabled = True
        Me.chkLstItemCounter.Location = New System.Drawing.Point(220, 275)
        Me.chkLstItemCounter.Name = "chkLstItemCounter"
        Me.chkLstItemCounter.Size = New System.Drawing.Size(199, 100)
        Me.chkLstItemCounter.TabIndex = 13
        '
        'chkCostCentreSelectAll
        '
        Me.chkCostCentreSelectAll.AutoSize = True
        Me.chkCostCentreSelectAll.Location = New System.Drawing.Point(12, 164)
        Me.chkCostCentreSelectAll.Name = "chkCostCentreSelectAll"
        Me.chkCostCentreSelectAll.Size = New System.Drawing.Size(95, 17)
        Me.chkCostCentreSelectAll.TabIndex = 6
        Me.chkCostCentreSelectAll.Text = "Cost Centre"
        Me.chkCostCentreSelectAll.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(183, 17)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(75, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Tran Inv No"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(9, 17)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(72, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "As On Date"
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(77, 479)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 19
        Me.btnSearch.Text = "&Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(289, 479)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 21
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(183, 479)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 20
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'frmTagItemsPurchaseReport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(868, 533)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.grpContainer)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmTagItemsPurchaseReport"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Taged Items Purchase Report"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.grpContainer.ResumeLayout(False)
        Me.grpContainer.PerformLayout()
        Me.PnlMark.ResumeLayout(False)
        Me.PnlMark.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
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
    Friend WithEvents chkDesignerSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkMetalSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkLstMetal As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkLstItemCounter As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkCostCentreSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkLstDesigner As System.Windows.Forms.CheckedListBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents chkStockOnly As System.Windows.Forms.CheckBox
    Friend WithEvents txtTranInvNo As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents dtpAsOnDate As BrighttechPack.DatePicker
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents rbtDetailed As System.Windows.Forms.RadioButton
    Friend WithEvents rbtSummary As System.Windows.Forms.RadioButton
    Friend WithEvents chkWithStudded As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Rbgwt As System.Windows.Forms.RadioButton
    Friend WithEvents rbnwt As System.Windows.Forms.RadioButton
    Friend WithEvents PnlMark As System.Windows.Forms.Panel
    Friend WithEvents rbtLocal As System.Windows.Forms.RadioButton
    Friend WithEvents rbtOutstation As System.Windows.Forms.RadioButton
    Friend WithEvents rbtBothDesigner As System.Windows.Forms.RadioButton
End Class
