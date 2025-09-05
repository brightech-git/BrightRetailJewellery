<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPurchaseMakeWise
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
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.btnView_Search = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.grpOrderBy = New System.Windows.Forms.GroupBox
        Me.rbtnWeight = New System.Windows.Forms.RadioButton
        Me.rbtnRate = New System.Windows.Forms.RadioButton
        Me.rbtnTranno = New System.Windows.Forms.RadioButton
        Me.GroupBox4 = New System.Windows.Forms.GroupBox
        Me.rbtmother = New System.Windows.Forms.RadioButton
        Me.rbtmown = New System.Windows.Forms.RadioButton
        Me.rbtmboth = New System.Windows.Forms.RadioButton
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.rbtSaleReturn = New System.Windows.Forms.RadioButton
        Me.rbtPurch = New System.Windows.Forms.RadioButton
        Me.rbtBoth = New System.Windows.Forms.RadioButton
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.chkCmbItemType = New BrighttechPack.CheckedComboBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.chkcmbcompany = New BrighttechPack.CheckedComboBox
        Me.chkcmbcostcenter = New BrighttechPack.CheckedComboBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.chkCmbMetal = New BrighttechPack.CheckedComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.lblMetalName = New System.Windows.Forms.Label
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.btnPrint = New System.Windows.Forms.Button
        Me.btnExport = New System.Windows.Forms.Button
        Me.gridView = New System.Windows.Forms.DataGridView
        Me.cmbGridShortCut = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ResizeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NEWToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.EXITToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.pnlTitle = New System.Windows.Forms.Panel
        Me.lblTitle = New System.Windows.Forms.Label
        Me.GroupBox1.SuspendLayout()
        Me.grpOrderBy.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.cmbGridShortCut.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.pnlTitle.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(921, 115)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(97, 30)
        Me.btnExit.TabIndex = 8
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(632, 115)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(97, 30)
        Me.btnNew.TabIndex = 5
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnView_Search
        '
        Me.btnView_Search.Location = New System.Drawing.Point(535, 115)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(97, 30)
        Me.btnView_Search.TabIndex = 4
        Me.btnView_Search.Text = "&View"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(189, 15)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(21, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "To"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(5, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(67, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Date From"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.grpOrderBy)
        Me.GroupBox1.Controls.Add(Me.GroupBox4)
        Me.GroupBox1.Controls.Add(Me.GroupBox3)
        Me.GroupBox1.Controls.Add(Me.GroupBox2)
        Me.GroupBox1.Controls.Add(Me.btnPrint)
        Me.GroupBox1.Controls.Add(Me.btnExport)
        Me.GroupBox1.Controls.Add(Me.btnExit)
        Me.GroupBox1.Controls.Add(Me.btnNew)
        Me.GroupBox1.Controls.Add(Me.btnView_Search)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Top
        Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(1022, 151)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        '
        'grpOrderBy
        '
        Me.grpOrderBy.Controls.Add(Me.rbtnWeight)
        Me.grpOrderBy.Controls.Add(Me.rbtnRate)
        Me.grpOrderBy.Controls.Add(Me.rbtnTranno)
        Me.grpOrderBy.Location = New System.Drawing.Point(788, 67)
        Me.grpOrderBy.Name = "grpOrderBy"
        Me.grpOrderBy.Size = New System.Drawing.Size(200, 37)
        Me.grpOrderBy.TabIndex = 3
        Me.grpOrderBy.TabStop = False
        Me.grpOrderBy.Text = "Order By"
        '
        'rbtnWeight
        '
        Me.rbtnWeight.AutoSize = True
        Me.rbtnWeight.Location = New System.Drawing.Point(125, 16)
        Me.rbtnWeight.Name = "rbtnWeight"
        Me.rbtnWeight.Size = New System.Drawing.Size(64, 17)
        Me.rbtnWeight.TabIndex = 2
        Me.rbtnWeight.TabStop = True
        Me.rbtnWeight.Text = "Weight"
        Me.rbtnWeight.UseVisualStyleBackColor = True
        '
        'rbtnRate
        '
        Me.rbtnRate.AutoSize = True
        Me.rbtnRate.Location = New System.Drawing.Point(72, 16)
        Me.rbtnRate.Name = "rbtnRate"
        Me.rbtnRate.Size = New System.Drawing.Size(51, 17)
        Me.rbtnRate.TabIndex = 1
        Me.rbtnRate.TabStop = True
        Me.rbtnRate.Text = "Rate"
        Me.rbtnRate.UseVisualStyleBackColor = True
        '
        'rbtnTranno
        '
        Me.rbtnTranno.AutoSize = True
        Me.rbtnTranno.Checked = True
        Me.rbtnTranno.Location = New System.Drawing.Point(6, 16)
        Me.rbtnTranno.Name = "rbtnTranno"
        Me.rbtnTranno.Size = New System.Drawing.Size(66, 17)
        Me.rbtnTranno.TabIndex = 0
        Me.rbtnTranno.TabStop = True
        Me.rbtnTranno.Text = "TranNo"
        Me.rbtnTranno.UseVisualStyleBackColor = True
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.rbtmother)
        Me.GroupBox4.Controls.Add(Me.rbtmown)
        Me.GroupBox4.Controls.Add(Me.rbtmboth)
        Me.GroupBox4.Location = New System.Drawing.Point(535, 67)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(247, 37)
        Me.GroupBox4.TabIndex = 2
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Making type"
        '
        'rbtmother
        '
        Me.rbtmother.AutoSize = True
        Me.rbtmother.Location = New System.Drawing.Point(146, 16)
        Me.rbtmother.Name = "rbtmother"
        Me.rbtmother.Size = New System.Drawing.Size(57, 17)
        Me.rbtmother.TabIndex = 2
        Me.rbtmother.TabStop = True
        Me.rbtmother.Text = "Other"
        Me.rbtmother.UseVisualStyleBackColor = True
        '
        'rbtmown
        '
        Me.rbtmown.AutoSize = True
        Me.rbtmown.Location = New System.Drawing.Point(63, 16)
        Me.rbtmown.Name = "rbtmown"
        Me.rbtmown.Size = New System.Drawing.Size(50, 17)
        Me.rbtmown.TabIndex = 1
        Me.rbtmown.TabStop = True
        Me.rbtmown.Text = "Own"
        Me.rbtmown.UseVisualStyleBackColor = True
        '
        'rbtmboth
        '
        Me.rbtmboth.AutoSize = True
        Me.rbtmboth.Checked = True
        Me.rbtmboth.Location = New System.Drawing.Point(6, 16)
        Me.rbtmboth.Name = "rbtmboth"
        Me.rbtmboth.Size = New System.Drawing.Size(51, 17)
        Me.rbtmboth.TabIndex = 0
        Me.rbtmboth.TabStop = True
        Me.rbtmboth.Text = "Both"
        Me.rbtmboth.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.rbtSaleReturn)
        Me.GroupBox3.Controls.Add(Me.rbtPurch)
        Me.GroupBox3.Controls.Add(Me.rbtBoth)
        Me.GroupBox3.Location = New System.Drawing.Point(535, 21)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(247, 37)
        Me.GroupBox3.TabIndex = 1
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Trantype "
        '
        'rbtSaleReturn
        '
        Me.rbtSaleReturn.AutoSize = True
        Me.rbtSaleReturn.Location = New System.Drawing.Point(146, 16)
        Me.rbtSaleReturn.Name = "rbtSaleReturn"
        Me.rbtSaleReturn.Size = New System.Drawing.Size(92, 17)
        Me.rbtSaleReturn.TabIndex = 2
        Me.rbtSaleReturn.TabStop = True
        Me.rbtSaleReturn.Text = "Sale Return"
        Me.rbtSaleReturn.UseVisualStyleBackColor = True
        '
        'rbtPurch
        '
        Me.rbtPurch.AutoSize = True
        Me.rbtPurch.Location = New System.Drawing.Point(63, 16)
        Me.rbtPurch.Name = "rbtPurch"
        Me.rbtPurch.Size = New System.Drawing.Size(77, 17)
        Me.rbtPurch.TabIndex = 1
        Me.rbtPurch.TabStop = True
        Me.rbtPurch.Text = "Purchase"
        Me.rbtPurch.UseVisualStyleBackColor = True
        '
        'rbtBoth
        '
        Me.rbtBoth.AutoSize = True
        Me.rbtBoth.Checked = True
        Me.rbtBoth.Location = New System.Drawing.Point(6, 16)
        Me.rbtBoth.Name = "rbtBoth"
        Me.rbtBoth.Size = New System.Drawing.Size(51, 17)
        Me.rbtBoth.TabIndex = 0
        Me.rbtBoth.TabStop = True
        Me.rbtBoth.Text = "Both"
        Me.rbtBoth.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.chkCmbItemType)
        Me.GroupBox2.Controls.Add(Me.Label5)
        Me.GroupBox2.Controls.Add(Me.chkcmbcompany)
        Me.GroupBox2.Controls.Add(Me.chkcmbcostcenter)
        Me.GroupBox2.Controls.Add(Me.Label4)
        Me.GroupBox2.Controls.Add(Me.chkCmbMetal)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Controls.Add(Me.dtpTo)
        Me.GroupBox2.Controls.Add(Me.lblMetalName)
        Me.GroupBox2.Controls.Add(Me.dtpFrom)
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Location = New System.Drawing.Point(12, 0)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(516, 144)
        Me.GroupBox2.TabIndex = 0
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Filtering"
        '
        'chkCmbItemType
        '
        Me.chkCmbItemType.CheckOnClick = True
        Me.chkCmbItemType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbItemType.DropDownHeight = 1
        Me.chkCmbItemType.FormattingEnabled = True
        Me.chkCmbItemType.IntegralHeight = False
        Me.chkCmbItemType.Location = New System.Drawing.Point(87, 120)
        Me.chkCmbItemType.Name = "chkCmbItemType"
        Me.chkCmbItemType.Size = New System.Drawing.Size(410, 22)
        Me.chkCmbItemType.TabIndex = 11
        Me.chkCmbItemType.ValueSeparator = ", "
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(5, 123)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(66, 13)
        Me.Label5.TabIndex = 10
        Me.Label5.Text = "Item Type"
        '
        'chkcmbcompany
        '
        Me.chkcmbcompany.CheckOnClick = True
        Me.chkcmbcompany.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkcmbcompany.DropDownHeight = 1
        Me.chkcmbcompany.FormattingEnabled = True
        Me.chkcmbcompany.IntegralHeight = False
        Me.chkcmbcompany.Location = New System.Drawing.Point(87, 38)
        Me.chkcmbcompany.Name = "chkcmbcompany"
        Me.chkcmbcompany.Size = New System.Drawing.Size(411, 22)
        Me.chkcmbcompany.TabIndex = 5
        Me.chkcmbcompany.ValueSeparator = ", "
        '
        'chkcmbcostcenter
        '
        Me.chkcmbcostcenter.CheckOnClick = True
        Me.chkcmbcostcenter.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkcmbcostcenter.DropDownHeight = 1
        Me.chkcmbcostcenter.FormattingEnabled = True
        Me.chkcmbcostcenter.IntegralHeight = False
        Me.chkcmbcostcenter.Location = New System.Drawing.Point(87, 66)
        Me.chkcmbcostcenter.Name = "chkcmbcostcenter"
        Me.chkcmbcostcenter.Size = New System.Drawing.Size(411, 22)
        Me.chkcmbcostcenter.TabIndex = 7
        Me.chkcmbcostcenter.ValueSeparator = ", "
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(4, 38)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(66, 13)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "Company "
        '
        'chkCmbMetal
        '
        Me.chkCmbMetal.CheckOnClick = True
        Me.chkCmbMetal.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbMetal.DropDownHeight = 1
        Me.chkCmbMetal.FormattingEnabled = True
        Me.chkCmbMetal.IntegralHeight = False
        Me.chkCmbMetal.Location = New System.Drawing.Point(87, 94)
        Me.chkCmbMetal.Name = "chkCmbMetal"
        Me.chkCmbMetal.Size = New System.Drawing.Size(410, 22)
        Me.chkCmbMetal.TabIndex = 9
        Me.chkCmbMetal.ValueSeparator = ", "
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(3, 66)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(76, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Cost Center"
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(216, 11)
        Me.dtpTo.Mask = "##/##/####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(93, 21)
        Me.dtpTo.TabIndex = 3
        Me.dtpTo.Text = "07/03/9998"
        Me.dtpTo.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'lblMetalName
        '
        Me.lblMetalName.AutoSize = True
        Me.lblMetalName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMetalName.Location = New System.Drawing.Point(5, 97)
        Me.lblMetalName.Name = "lblMetalName"
        Me.lblMetalName.Size = New System.Drawing.Size(78, 13)
        Me.lblMetalName.TabIndex = 8
        Me.lblMetalName.Text = "Metal Name "
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(87, 11)
        Me.dtpFrom.Mask = "##/##/####"
        Me.dtpFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom.Size = New System.Drawing.Size(93, 21)
        Me.dtpFrom.TabIndex = 1
        Me.dtpFrom.Text = "07/03/9998"
        Me.dtpFrom.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(824, 115)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(97, 30)
        Me.btnPrint.TabIndex = 7
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(728, 115)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(97, 30)
        Me.btnExport.TabIndex = 6
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.ContextMenuStrip = Me.cmbGridShortCut
        Me.gridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView.Location = New System.Drawing.Point(0, 171)
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.Size = New System.Drawing.Size(1022, 469)
        Me.gridView.TabIndex = 1
        '
        'cmbGridShortCut
        '
        Me.cmbGridShortCut.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ResizeToolStripMenuItem})
        Me.cmbGridShortCut.Name = "ContextMenuStrip2"
        Me.cmbGridShortCut.Size = New System.Drawing.Size(143, 26)
        '
        'ResizeToolStripMenuItem
        '
        Me.ResizeToolStripMenuItem.CheckOnClick = True
        Me.ResizeToolStripMenuItem.Name = "ResizeToolStripMenuItem"
        Me.ResizeToolStripMenuItem.Size = New System.Drawing.Size(142, 22)
        Me.ResizeToolStripMenuItem.Text = "Auto Resize"
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NEWToolStripMenuItem, Me.EXITToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(129, 48)
        '
        'NEWToolStripMenuItem
        '
        Me.NEWToolStripMenuItem.Name = "NEWToolStripMenuItem"
        Me.NEWToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NEWToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.NEWToolStripMenuItem.Text = "New"
        Me.NEWToolStripMenuItem.Visible = False
        '
        'EXITToolStripMenuItem
        '
        Me.EXITToolStripMenuItem.Name = "EXITToolStripMenuItem"
        Me.EXITToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.EXITToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.EXITToolStripMenuItem.Text = "Exit"
        Me.EXITToolStripMenuItem.Visible = False
        '
        'pnlTitle
        '
        Me.pnlTitle.Controls.Add(Me.lblTitle)
        Me.pnlTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlTitle.Location = New System.Drawing.Point(0, 151)
        Me.pnlTitle.Name = "pnlTitle"
        Me.pnlTitle.Size = New System.Drawing.Size(1022, 20)
        Me.pnlTitle.TabIndex = 2
        '
        'lblTitle
        '
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblTitle.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(1022, 20)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "Title"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'frmPurchaseMakeWise
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1022, 640)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.gridView)
        Me.Controls.Add(Me.pnlTitle)
        Me.Controls.Add(Me.GroupBox1)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmPurchaseMakeWise"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Sales Purchase"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.GroupBox1.ResumeLayout(False)
        Me.grpOrderBy.ResumeLayout(False)
        Me.grpOrderBy.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.cmbGridShortCut.ResumeLayout(False)
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.pnlTitle.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnView_Search As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NEWToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EXITToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents pnlTitle As System.Windows.Forms.Panel
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents rbtSaleReturn As System.Windows.Forms.RadioButton
    Friend WithEvents rbtBoth As System.Windows.Forms.RadioButton
    Friend WithEvents rbtPurch As System.Windows.Forms.RadioButton
    Friend WithEvents lblMetalName As System.Windows.Forms.Label
    Friend WithEvents chkCmbMetal As BrighttechPack.CheckedComboBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents chkcmbcompany As BrighttechPack.CheckedComboBox
    Friend WithEvents chkcmbcostcenter As BrighttechPack.CheckedComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents rbtmother As System.Windows.Forms.RadioButton
    Friend WithEvents rbtmown As System.Windows.Forms.RadioButton
    Friend WithEvents rbtmboth As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents chkCmbItemType As BrighttechPack.CheckedComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents ResizeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmbGridShortCut As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents grpOrderBy As System.Windows.Forms.GroupBox
    Friend WithEvents rbtnWeight As System.Windows.Forms.RadioButton
    Friend WithEvents rbtnRate As System.Windows.Forms.RadioButton
    Friend WithEvents rbtnTranno As System.Windows.Forms.RadioButton
End Class
