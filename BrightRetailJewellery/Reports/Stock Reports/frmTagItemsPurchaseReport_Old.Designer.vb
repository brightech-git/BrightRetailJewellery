<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTagItemsPurchaseReport_Old
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
        Me.chktranno = New System.Windows.Forms.CheckBox()
        Me.GrpStkType = New System.Windows.Forms.GroupBox()
        Me.rbtExem = New System.Windows.Forms.RadioButton()
        Me.rbtTrading = New System.Windows.Forms.RadioButton()
        Me.rbtManufacturin = New System.Windows.Forms.RadioButton()
        Me.rbtAll = New System.Windows.Forms.RadioButton()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.rbtDetailed = New System.Windows.Forms.RadioButton()
        Me.rbtSummary = New System.Windows.Forms.RadioButton()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.chkNone = New System.Windows.Forms.CheckBox()
        Me.chkItemCategory = New System.Windows.Forms.CheckBox()
        Me.chkWithCategory = New System.Windows.Forms.CheckBox()
        Me.chkWithImage = New System.Windows.Forms.CheckBox()
        Me.ChkWithPend = New System.Windows.Forms.CheckBox()
        Me.ChkWithTrf = New System.Windows.Forms.CheckBox()
        Me.lblAsOnDate = New System.Windows.Forms.Label()
        Me.PnlTodate = New System.Windows.Forms.Panel()
        Me.lblTo = New System.Windows.Forms.Label()
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.rbtStockOnly = New System.Windows.Forms.RadioButton()
        Me.rbtReceipt = New System.Windows.Forms.RadioButton()
        Me.rbtIssue = New System.Windows.Forms.RadioButton()
        Me.dtpAsOnDate = New BrighttechPack.DatePicker(Me.components)
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
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.grpContainer.SuspendLayout()
        Me.GrpStkType.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.PnlTodate.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
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
        Me.grpContainer.Controls.Add(Me.chktranno)
        Me.grpContainer.Controls.Add(Me.GrpStkType)
        Me.grpContainer.Controls.Add(Me.GroupBox3)
        Me.grpContainer.Controls.Add(Me.GroupBox2)
        Me.grpContainer.Controls.Add(Me.chkWithImage)
        Me.grpContainer.Controls.Add(Me.ChkWithPend)
        Me.grpContainer.Controls.Add(Me.ChkWithTrf)
        Me.grpContainer.Controls.Add(Me.lblAsOnDate)
        Me.grpContainer.Controls.Add(Me.PnlTodate)
        Me.grpContainer.Controls.Add(Me.GroupBox1)
        Me.grpContainer.Controls.Add(Me.dtpAsOnDate)
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
        Me.grpContainer.Controls.Add(Me.btnSearch)
        Me.grpContainer.Controls.Add(Me.btnExit)
        Me.grpContainer.Controls.Add(Me.btnNew)
        Me.grpContainer.Location = New System.Drawing.Point(99, 19)
        Me.grpContainer.Name = "grpContainer"
        Me.grpContainer.Size = New System.Drawing.Size(446, 584)
        Me.grpContainer.TabIndex = 0
        Me.grpContainer.TabStop = False
        '
        'chktranno
        '
        Me.chktranno.AutoSize = True
        Me.chktranno.Location = New System.Drawing.Point(347, 422)
        Me.chktranno.Name = "chktranno"
        Me.chktranno.Size = New System.Drawing.Size(95, 17)
        Me.chktranno.TabIndex = 19
        Me.chktranno.Text = "With Tranno"
        Me.chktranno.UseVisualStyleBackColor = True
        '
        'GrpStkType
        '
        Me.GrpStkType.Controls.Add(Me.rbtExem)
        Me.GrpStkType.Controls.Add(Me.rbtTrading)
        Me.GrpStkType.Controls.Add(Me.rbtManufacturin)
        Me.GrpStkType.Controls.Add(Me.rbtAll)
        Me.GrpStkType.Location = New System.Drawing.Point(9, 509)
        Me.GrpStkType.Name = "GrpStkType"
        Me.GrpStkType.Size = New System.Drawing.Size(407, 35)
        Me.GrpStkType.TabIndex = 22
        Me.GrpStkType.TabStop = False
        Me.GrpStkType.Text = "Stock Type"
        '
        'rbtExem
        '
        Me.rbtExem.AutoSize = True
        Me.rbtExem.Location = New System.Drawing.Point(251, 11)
        Me.rbtExem.Name = "rbtExem"
        Me.rbtExem.Size = New System.Drawing.Size(82, 17)
        Me.rbtExem.TabIndex = 2
        Me.rbtExem.Text = "Exempted"
        '
        'rbtTrading
        '
        Me.rbtTrading.AutoSize = True
        Me.rbtTrading.Checked = True
        Me.rbtTrading.Location = New System.Drawing.Point(181, 11)
        Me.rbtTrading.Name = "rbtTrading"
        Me.rbtTrading.Size = New System.Drawing.Size(68, 17)
        Me.rbtTrading.TabIndex = 1
        Me.rbtTrading.TabStop = True
        Me.rbtTrading.Text = "Trading"
        '
        'rbtManufacturin
        '
        Me.rbtManufacturin.AutoSize = True
        Me.rbtManufacturin.Location = New System.Drawing.Point(75, 11)
        Me.rbtManufacturin.Name = "rbtManufacturin"
        Me.rbtManufacturin.Size = New System.Drawing.Size(105, 17)
        Me.rbtManufacturin.TabIndex = 0
        Me.rbtManufacturin.Text = "Manufacturing"
        '
        'rbtAll
        '
        Me.rbtAll.AutoSize = True
        Me.rbtAll.Location = New System.Drawing.Point(340, 11)
        Me.rbtAll.Name = "rbtAll"
        Me.rbtAll.Size = New System.Drawing.Size(39, 17)
        Me.rbtAll.TabIndex = 3
        Me.rbtAll.Text = "All"
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.rbtDetailed)
        Me.GroupBox3.Controls.Add(Me.rbtSummary)
        Me.GroupBox3.Location = New System.Drawing.Point(9, 477)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(407, 35)
        Me.GroupBox3.TabIndex = 21
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Filter By"
        '
        'rbtDetailed
        '
        Me.rbtDetailed.Checked = True
        Me.rbtDetailed.Location = New System.Drawing.Point(181, 11)
        Me.rbtDetailed.Name = "rbtDetailed"
        Me.rbtDetailed.Size = New System.Drawing.Size(82, 18)
        Me.rbtDetailed.TabIndex = 1
        Me.rbtDetailed.TabStop = True
        Me.rbtDetailed.Text = "Detail"
        '
        'rbtSummary
        '
        Me.rbtSummary.Location = New System.Drawing.Point(75, 11)
        Me.rbtSummary.Name = "rbtSummary"
        Me.rbtSummary.Size = New System.Drawing.Size(86, 18)
        Me.rbtSummary.TabIndex = 0
        Me.rbtSummary.Text = "Summary "
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.chkNone)
        Me.GroupBox2.Controls.Add(Me.chkItemCategory)
        Me.GroupBox2.Controls.Add(Me.chkWithCategory)
        Me.GroupBox2.Location = New System.Drawing.Point(9, 443)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(407, 33)
        Me.GroupBox2.TabIndex = 20
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Group By"
        '
        'chkNone
        '
        Me.chkNone.AutoSize = True
        Me.chkNone.Location = New System.Drawing.Point(278, 14)
        Me.chkNone.Name = "chkNone"
        Me.chkNone.Size = New System.Drawing.Size(55, 17)
        Me.chkNone.TabIndex = 2
        Me.chkNone.Text = "None"
        Me.chkNone.UseVisualStyleBackColor = True
        '
        'chkItemCategory
        '
        Me.chkItemCategory.AutoSize = True
        Me.chkItemCategory.Location = New System.Drawing.Point(162, 14)
        Me.chkItemCategory.Name = "chkItemCategory"
        Me.chkItemCategory.Size = New System.Drawing.Size(106, 17)
        Me.chkItemCategory.TabIndex = 1
        Me.chkItemCategory.Text = "ItemCategory"
        Me.chkItemCategory.UseVisualStyleBackColor = True
        '
        'chkWithCategory
        '
        Me.chkWithCategory.AutoSize = True
        Me.chkWithCategory.Location = New System.Drawing.Point(73, 14)
        Me.chkWithCategory.Name = "chkWithCategory"
        Me.chkWithCategory.Size = New System.Drawing.Size(79, 17)
        Me.chkWithCategory.TabIndex = 0
        Me.chkWithCategory.Text = "Category"
        Me.chkWithCategory.UseVisualStyleBackColor = True
        '
        'chkWithImage
        '
        Me.chkWithImage.AutoSize = True
        Me.chkWithImage.Location = New System.Drawing.Point(254, 422)
        Me.chkWithImage.Name = "chkWithImage"
        Me.chkWithImage.Size = New System.Drawing.Size(92, 17)
        Me.chkWithImage.TabIndex = 18
        Me.chkWithImage.Text = "With Image"
        Me.chkWithImage.UseVisualStyleBackColor = True
        '
        'ChkWithPend
        '
        Me.ChkWithPend.AutoSize = True
        Me.ChkWithPend.Location = New System.Drawing.Point(9, 422)
        Me.ChkWithPend.Name = "ChkWithPend"
        Me.ChkWithPend.Size = New System.Drawing.Size(152, 17)
        Me.ChkWithPend.TabIndex = 16
        Me.ChkWithPend.Text = "With Pending Transfer"
        Me.ChkWithPend.UseVisualStyleBackColor = True
        '
        'ChkWithTrf
        '
        Me.ChkWithTrf.AutoSize = True
        Me.ChkWithTrf.Location = New System.Drawing.Point(161, 422)
        Me.ChkWithTrf.Name = "ChkWithTrf"
        Me.ChkWithTrf.Size = New System.Drawing.Size(94, 17)
        Me.ChkWithTrf.TabIndex = 17
        Me.ChkWithTrf.Text = "With Transit"
        Me.ChkWithTrf.UseVisualStyleBackColor = True
        '
        'lblAsOnDate
        '
        Me.lblAsOnDate.AutoSize = True
        Me.lblAsOnDate.Location = New System.Drawing.Point(45, 49)
        Me.lblAsOnDate.Name = "lblAsOnDate"
        Me.lblAsOnDate.Size = New System.Drawing.Size(72, 13)
        Me.lblAsOnDate.TabIndex = 2
        Me.lblAsOnDate.Text = "As On Date"
        '
        'PnlTodate
        '
        Me.PnlTodate.Controls.Add(Me.lblTo)
        Me.PnlTodate.Controls.Add(Me.dtpTo)
        Me.PnlTodate.Location = New System.Drawing.Point(244, 43)
        Me.PnlTodate.Name = "PnlTodate"
        Me.PnlTodate.Size = New System.Drawing.Size(144, 29)
        Me.PnlTodate.TabIndex = 3
        '
        'lblTo
        '
        Me.lblTo.AutoSize = True
        Me.lblTo.Location = New System.Drawing.Point(4, 8)
        Me.lblTo.Name = "lblTo"
        Me.lblTo.Size = New System.Drawing.Size(21, 13)
        Me.lblTo.TabIndex = 0
        Me.lblTo.Text = "To"
        Me.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(31, 4)
        Me.dtpTo.Mask = "##/##/####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(93, 21)
        Me.dtpTo.TabIndex = 1
        Me.dtpTo.Text = "07/03/9998"
        Me.dtpTo.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.rbtStockOnly)
        Me.GroupBox1.Controls.Add(Me.rbtReceipt)
        Me.GroupBox1.Controls.Add(Me.rbtIssue)
        Me.GroupBox1.Location = New System.Drawing.Point(9, 9)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(407, 34)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        '
        'rbtStockOnly
        '
        Me.rbtStockOnly.Checked = True
        Me.rbtStockOnly.Location = New System.Drawing.Point(47, 11)
        Me.rbtStockOnly.Name = "rbtStockOnly"
        Me.rbtStockOnly.Size = New System.Drawing.Size(104, 18)
        Me.rbtStockOnly.TabIndex = 0
        Me.rbtStockOnly.TabStop = True
        Me.rbtStockOnly.Text = "Stock Only"
        '
        'rbtReceipt
        '
        Me.rbtReceipt.Location = New System.Drawing.Point(255, 11)
        Me.rbtReceipt.Name = "rbtReceipt"
        Me.rbtReceipt.Size = New System.Drawing.Size(104, 18)
        Me.rbtReceipt.TabIndex = 2
        Me.rbtReceipt.Text = "Receipt"
        '
        'rbtIssue
        '
        Me.rbtIssue.Location = New System.Drawing.Point(151, 11)
        Me.rbtIssue.Name = "rbtIssue"
        Me.rbtIssue.Size = New System.Drawing.Size(104, 18)
        Me.rbtIssue.TabIndex = 1
        Me.rbtIssue.Text = "Issue"
        '
        'dtpAsOnDate
        '
        Me.dtpAsOnDate.Location = New System.Drawing.Point(138, 48)
        Me.dtpAsOnDate.Mask = "##/##/####"
        Me.dtpAsOnDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpAsOnDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpAsOnDate.Name = "dtpAsOnDate"
        Me.dtpAsOnDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpAsOnDate.Size = New System.Drawing.Size(93, 21)
        Me.dtpAsOnDate.TabIndex = 3
        Me.dtpAsOnDate.Text = "07/03/9998"
        Me.dtpAsOnDate.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'txtTranInvNo
        '
        Me.txtTranInvNo.Location = New System.Drawing.Point(90, 396)
        Me.txtTranInvNo.Name = "txtTranInvNo"
        Me.txtTranInvNo.Size = New System.Drawing.Size(120, 21)
        Me.txtTranInvNo.TabIndex = 15
        '
        'chkLstItem
        '
        Me.chkLstItem.FormattingEnabled = True
        Me.chkLstItem.Location = New System.Drawing.Point(9, 291)
        Me.chkLstItem.Name = "chkLstItem"
        Me.chkLstItem.Size = New System.Drawing.Size(201, 100)
        Me.chkLstItem.TabIndex = 11
        '
        'chkItemSelectAll
        '
        Me.chkItemSelectAll.AutoSize = True
        Me.chkItemSelectAll.Location = New System.Drawing.Point(9, 273)
        Me.chkItemSelectAll.Name = "chkItemSelectAll"
        Me.chkItemSelectAll.Size = New System.Drawing.Size(53, 17)
        Me.chkItemSelectAll.TabIndex = 10
        Me.chkItemSelectAll.Text = "Item"
        Me.chkItemSelectAll.UseVisualStyleBackColor = True
        '
        'chkLstDesigner
        '
        Me.chkLstDesigner.FormattingEnabled = True
        Me.chkLstDesigner.Location = New System.Drawing.Point(9, 90)
        Me.chkLstDesigner.MultiColumn = True
        Me.chkLstDesigner.Name = "chkLstDesigner"
        Me.chkLstDesigner.Size = New System.Drawing.Size(410, 84)
        Me.chkLstDesigner.TabIndex = 5
        '
        'chkItemCounterSelectAll
        '
        Me.chkItemCounterSelectAll.AutoSize = True
        Me.chkItemCounterSelectAll.Location = New System.Drawing.Point(223, 273)
        Me.chkItemCounterSelectAll.Name = "chkItemCounterSelectAll"
        Me.chkItemCounterSelectAll.Size = New System.Drawing.Size(103, 17)
        Me.chkItemCounterSelectAll.TabIndex = 12
        Me.chkItemCounterSelectAll.Text = "Item Counter"
        Me.chkItemCounterSelectAll.UseVisualStyleBackColor = True
        '
        'chkDesignerSelectAll
        '
        Me.chkDesignerSelectAll.AutoSize = True
        Me.chkDesignerSelectAll.Location = New System.Drawing.Point(9, 72)
        Me.chkDesignerSelectAll.Name = "chkDesignerSelectAll"
        Me.chkDesignerSelectAll.Size = New System.Drawing.Size(77, 17)
        Me.chkDesignerSelectAll.TabIndex = 4
        Me.chkDesignerSelectAll.Text = "Designer"
        Me.chkDesignerSelectAll.UseVisualStyleBackColor = True
        '
        'chkLstCostCentre
        '
        Me.chkLstCostCentre.FormattingEnabled = True
        Me.chkLstCostCentre.Location = New System.Drawing.Point(9, 201)
        Me.chkLstCostCentre.Name = "chkLstCostCentre"
        Me.chkLstCostCentre.Size = New System.Drawing.Size(201, 68)
        Me.chkLstCostCentre.TabIndex = 7
        '
        'chkMetalSelectAll
        '
        Me.chkMetalSelectAll.AutoSize = True
        Me.chkMetalSelectAll.Location = New System.Drawing.Point(223, 182)
        Me.chkMetalSelectAll.Name = "chkMetalSelectAll"
        Me.chkMetalSelectAll.Size = New System.Drawing.Size(56, 17)
        Me.chkMetalSelectAll.TabIndex = 8
        Me.chkMetalSelectAll.Text = "Metal"
        Me.chkMetalSelectAll.UseVisualStyleBackColor = True
        '
        'chkLstMetal
        '
        Me.chkLstMetal.FormattingEnabled = True
        Me.chkLstMetal.Location = New System.Drawing.Point(220, 201)
        Me.chkLstMetal.Name = "chkLstMetal"
        Me.chkLstMetal.Size = New System.Drawing.Size(199, 68)
        Me.chkLstMetal.TabIndex = 9
        '
        'chkLstItemCounter
        '
        Me.chkLstItemCounter.FormattingEnabled = True
        Me.chkLstItemCounter.Location = New System.Drawing.Point(220, 291)
        Me.chkLstItemCounter.Name = "chkLstItemCounter"
        Me.chkLstItemCounter.Size = New System.Drawing.Size(199, 100)
        Me.chkLstItemCounter.TabIndex = 13
        '
        'chkCostCentreSelectAll
        '
        Me.chkCostCentreSelectAll.AutoSize = True
        Me.chkCostCentreSelectAll.Location = New System.Drawing.Point(9, 182)
        Me.chkCostCentreSelectAll.Name = "chkCostCentreSelectAll"
        Me.chkCostCentreSelectAll.Size = New System.Drawing.Size(95, 17)
        Me.chkCostCentreSelectAll.TabIndex = 6
        Me.chkCostCentreSelectAll.Text = "Cost Centre"
        Me.chkCostCentreSelectAll.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(9, 399)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(75, 13)
        Me.Label2.TabIndex = 14
        Me.Label2.Text = "Tran Inv No"
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(56, 548)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 23
        Me.btnSearch.Text = "&Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(274, 548)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 25
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(165, 548)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 24
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'frmTagItemsPurchaseReport_Old
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(629, 614)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.grpContainer)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmTagItemsPurchaseReport_Old"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "frmTagItemsPurchaseReport_Old"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.grpContainer.ResumeLayout(False)
        Me.grpContainer.PerformLayout()
        Me.GrpStkType.ResumeLayout(False)
        Me.GrpStkType.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.PnlTodate.ResumeLayout(False)
        Me.PnlTodate.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents grpContainer As System.Windows.Forms.GroupBox
    Friend WithEvents rbtDetailed As System.Windows.Forms.RadioButton
    Friend WithEvents rbtSummary As System.Windows.Forms.RadioButton
    Friend WithEvents dtpAsOnDate As BrighttechPack.DatePicker
    Friend WithEvents txtTranInvNo As System.Windows.Forms.TextBox
    Friend WithEvents chkLstItem As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkItemSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkLstDesigner As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkItemCounterSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkDesignerSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkLstCostCentre As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkMetalSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkLstMetal As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkLstItemCounter As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkCostCentreSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents chkWithImage As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents rbtStockOnly As System.Windows.Forms.RadioButton
    Friend WithEvents rbtReceipt As System.Windows.Forms.RadioButton
    Friend WithEvents rbtIssue As System.Windows.Forms.RadioButton
    Friend WithEvents PnlTodate As System.Windows.Forms.Panel
    Friend WithEvents lblTo As System.Windows.Forms.Label
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents lblAsOnDate As System.Windows.Forms.Label
    Friend WithEvents chkWithCategory As System.Windows.Forms.CheckBox
    Friend WithEvents ChkWithTrf As System.Windows.Forms.CheckBox
    Friend WithEvents ChkWithPend As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents chkItemCategory As System.Windows.Forms.CheckBox
    Friend WithEvents chkNone As System.Windows.Forms.CheckBox
    Friend WithEvents GrpStkType As System.Windows.Forms.GroupBox
    Friend WithEvents rbtTrading As System.Windows.Forms.RadioButton
    Friend WithEvents rbtManufacturin As System.Windows.Forms.RadioButton
    Friend WithEvents rbtAll As System.Windows.Forms.RadioButton
    Friend WithEvents rbtExem As System.Windows.Forms.RadioButton
    Friend WithEvents chktranno As System.Windows.Forms.CheckBox
End Class
