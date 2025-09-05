<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCategoryWisePurchase
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
        Me.ExtToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.btnView_Search = New System.Windows.Forms.Button
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.chkCmbCostCentre = New BrighttechPack.CheckedComboBox
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.rbtBothMake = New System.Windows.Forms.RadioButton
        Me.rbtOthers = New System.Windows.Forms.RadioButton
        Me.rbtOwn = New System.Windows.Forms.RadioButton
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.rbtBothPurExch = New System.Windows.Forms.RadioButton
        Me.rbtExchange = New System.Windows.Forms.RadioButton
        Me.rbtPurchase = New System.Windows.Forms.RadioButton
        Me.chkcmbCategory = New BrighttechPack.CheckedComboBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.chkcmbemployee = New BrighttechPack.CheckedComboBox
        Me.ContextMenuStrip1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.ExtToolStripMenuItem})
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
        'ExtToolStripMenuItem
        '
        Me.ExtToolStripMenuItem.Name = "ExtToolStripMenuItem"
        Me.ExtToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExtToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.ExtToolStripMenuItem.Text = "Exit"
        Me.ExtToolStripMenuItem.Visible = False
        '
        'btnView_Search
        '
        Me.btnView_Search.Location = New System.Drawing.Point(28, 266)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(100, 30)
        Me.btnView_Search.TabIndex = 12
        Me.btnView_Search.Text = "&View"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(244, 266)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 14
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(136, 266)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 13
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(25, 30)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(79, 21)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Date From"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(25, 76)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(76, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Cost Centre"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(212, 30)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(23, 21)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "To"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(108, 31)
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
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(242, 31)
        Me.dtpTo.Mask = "##/##/####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(91, 21)
        Me.dtpTo.TabIndex = 3
        Me.dtpTo.Text = "07/03/9998"
        Me.dtpTo.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'chkCmbCostCentre
        '
        Me.chkCmbCostCentre.CheckOnClick = True
        Me.chkCmbCostCentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCostCentre.DropDownHeight = 1
        Me.chkCmbCostCentre.FormattingEnabled = True
        Me.chkCmbCostCentre.IntegralHeight = False
        Me.chkCmbCostCentre.Location = New System.Drawing.Point(110, 73)
        Me.chkCmbCostCentre.Name = "chkCmbCostCentre"
        Me.chkCmbCostCentre.Size = New System.Drawing.Size(269, 22)
        Me.chkCmbCostCentre.TabIndex = 5
        Me.chkCmbCostCentre.ValueSeparator = ", "
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.GroupBox2)
        Me.Panel1.Controls.Add(Me.GroupBox1)
        Me.Panel1.Controls.Add(Me.chkcmbCategory)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.chkcmbemployee)
        Me.Panel1.Controls.Add(Me.chkCmbCostCentre)
        Me.Panel1.Controls.Add(Me.dtpTo)
        Me.Panel1.Controls.Add(Me.dtpFrom)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.btnNew)
        Me.Panel1.Controls.Add(Me.btnExit)
        Me.Panel1.Controls.Add(Me.btnView_Search)
        Me.Panel1.Location = New System.Drawing.Point(238, 116)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(399, 360)
        Me.Panel1.TabIndex = 0
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.rbtBothMake)
        Me.GroupBox2.Controls.Add(Me.rbtOthers)
        Me.GroupBox2.Controls.Add(Me.rbtOwn)
        Me.GroupBox2.Location = New System.Drawing.Point(110, 167)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(269, 35)
        Me.GroupBox2.TabIndex = 10
        Me.GroupBox2.TabStop = False
        '
        'rbtBothMake
        '
        Me.rbtBothMake.AutoSize = True
        Me.rbtBothMake.Checked = True
        Me.rbtBothMake.Location = New System.Drawing.Point(11, 12)
        Me.rbtBothMake.Name = "rbtBothMake"
        Me.rbtBothMake.Size = New System.Drawing.Size(51, 17)
        Me.rbtBothMake.TabIndex = 1
        Me.rbtBothMake.TabStop = True
        Me.rbtBothMake.Text = "Both"
        Me.rbtBothMake.UseVisualStyleBackColor = True
        '
        'rbtOthers
        '
        Me.rbtOthers.AutoSize = True
        Me.rbtOthers.Location = New System.Drawing.Point(134, 12)
        Me.rbtOthers.Name = "rbtOthers"
        Me.rbtOthers.Size = New System.Drawing.Size(63, 17)
        Me.rbtOthers.TabIndex = 0
        Me.rbtOthers.Text = "Others"
        Me.rbtOthers.UseVisualStyleBackColor = True
        '
        'rbtOwn
        '
        Me.rbtOwn.AutoSize = True
        Me.rbtOwn.Location = New System.Drawing.Point(71, 12)
        Me.rbtOwn.Name = "rbtOwn"
        Me.rbtOwn.Size = New System.Drawing.Size(50, 17)
        Me.rbtOwn.TabIndex = 2
        Me.rbtOwn.Text = "Own"
        Me.rbtOwn.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.rbtBothPurExch)
        Me.GroupBox1.Controls.Add(Me.rbtExchange)
        Me.GroupBox1.Controls.Add(Me.rbtPurchase)
        Me.GroupBox1.Location = New System.Drawing.Point(110, 208)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(269, 37)
        Me.GroupBox1.TabIndex = 11
        Me.GroupBox1.TabStop = False
        '
        'rbtBothPurExch
        '
        Me.rbtBothPurExch.AutoSize = True
        Me.rbtBothPurExch.Checked = True
        Me.rbtBothPurExch.Location = New System.Drawing.Point(10, 14)
        Me.rbtBothPurExch.Name = "rbtBothPurExch"
        Me.rbtBothPurExch.Size = New System.Drawing.Size(51, 17)
        Me.rbtBothPurExch.TabIndex = 0
        Me.rbtBothPurExch.TabStop = True
        Me.rbtBothPurExch.Text = "Both"
        Me.rbtBothPurExch.UseVisualStyleBackColor = True
        '
        'rbtExchange
        '
        Me.rbtExchange.AutoSize = True
        Me.rbtExchange.Location = New System.Drawing.Point(169, 14)
        Me.rbtExchange.Name = "rbtExchange"
        Me.rbtExchange.Size = New System.Drawing.Size(80, 17)
        Me.rbtExchange.TabIndex = 2
        Me.rbtExchange.Text = "Exchange"
        Me.rbtExchange.UseVisualStyleBackColor = True
        '
        'rbtPurchase
        '
        Me.rbtPurchase.AutoSize = True
        Me.rbtPurchase.Location = New System.Drawing.Point(69, 14)
        Me.rbtPurchase.Name = "rbtPurchase"
        Me.rbtPurchase.Size = New System.Drawing.Size(90, 17)
        Me.rbtPurchase.TabIndex = 1
        Me.rbtPurchase.Text = "Cash Purch"
        Me.rbtPurchase.UseVisualStyleBackColor = True
        '
        'chkcmbCategory
        '
        Me.chkcmbCategory.CheckOnClick = True
        Me.chkcmbCategory.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkcmbCategory.DropDownHeight = 1
        Me.chkcmbCategory.FormattingEnabled = True
        Me.chkcmbCategory.IntegralHeight = False
        Me.chkcmbCategory.Location = New System.Drawing.Point(110, 106)
        Me.chkcmbCategory.Name = "chkcmbCategory"
        Me.chkcmbCategory.Size = New System.Drawing.Size(269, 22)
        Me.chkcmbCategory.TabIndex = 7
        Me.chkcmbCategory.ValueSeparator = ", "
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(25, 108)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(60, 13)
        Me.Label6.TabIndex = 6
        Me.Label6.Text = "Category"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(25, 141)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(69, 13)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "Emp Name"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkcmbemployee
        '
        Me.chkcmbemployee.CheckOnClick = True
        Me.chkcmbemployee.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkcmbemployee.DropDownHeight = 1
        Me.chkcmbemployee.FormattingEnabled = True
        Me.chkcmbemployee.IntegralHeight = False
        Me.chkcmbemployee.Location = New System.Drawing.Point(110, 138)
        Me.chkcmbemployee.Name = "chkcmbemployee"
        Me.chkcmbemployee.Size = New System.Drawing.Size(269, 22)
        Me.chkcmbemployee.TabIndex = 9
        Me.chkcmbemployee.ValueSeparator = ", "
        '
        'frmCategoryWisePurchase
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(892, 606)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmCategoryWisePurchase"
        Me.Text = " CATEGORY  PURCHASE REPORT"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExtToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnView_Search As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents chkCmbCostCentre As BrighttechPack.CheckedComboBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents chkcmbemployee As BrighttechPack.CheckedComboBox
    Friend WithEvents chkcmbCategory As BrighttechPack.CheckedComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents rbtBothPurExch As System.Windows.Forms.RadioButton
    Friend WithEvents rbtExchange As System.Windows.Forms.RadioButton
    Friend WithEvents rbtPurchase As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents rbtBothMake As System.Windows.Forms.RadioButton
    Friend WithEvents rbtOthers As System.Windows.Forms.RadioButton
    Friend WithEvents rbtOwn As System.Windows.Forms.RadioButton
End Class
