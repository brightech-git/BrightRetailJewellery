<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TagWiseProfitLoss
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
        Me.grpContrainer = New System.Windows.Forms.GroupBox()
        Me.rbtFormat3 = New System.Windows.Forms.RadioButton()
        Me.chkPurchaseWt = New System.Windows.Forms.CheckBox()
        Me.chkMetalSelectAll = New System.Windows.Forms.CheckBox()
        Me.chklistMetal = New System.Windows.Forms.CheckedListBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.chkFormat2 = New System.Windows.Forms.RadioButton()
        Me.chkFormat1 = New System.Windows.Forms.RadioButton()
        Me.cmbGroupBy = New System.Windows.Forms.ComboBox()
        Me.chkCompanySelectAll = New System.Windows.Forms.CheckBox()
        Me.chkLstCompany = New System.Windows.Forms.CheckedListBox()
        Me.chkCostCentreSelectAll = New System.Windows.Forms.CheckBox()
        Me.chkLstCostCentre = New System.Windows.Forms.CheckedListBox()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.chkGP = New System.Windows.Forms.CheckBox()
        Me.grpContrainer.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpContrainer
        '
        Me.grpContrainer.Controls.Add(Me.chkGP)
        Me.grpContrainer.Controls.Add(Me.rbtFormat3)
        Me.grpContrainer.Controls.Add(Me.chkPurchaseWt)
        Me.grpContrainer.Controls.Add(Me.chkMetalSelectAll)
        Me.grpContrainer.Controls.Add(Me.chklistMetal)
        Me.grpContrainer.Controls.Add(Me.Label4)
        Me.grpContrainer.Controls.Add(Me.chkFormat2)
        Me.grpContrainer.Controls.Add(Me.chkFormat1)
        Me.grpContrainer.Controls.Add(Me.cmbGroupBy)
        Me.grpContrainer.Controls.Add(Me.chkCompanySelectAll)
        Me.grpContrainer.Controls.Add(Me.chkLstCompany)
        Me.grpContrainer.Controls.Add(Me.chkCostCentreSelectAll)
        Me.grpContrainer.Controls.Add(Me.chkLstCostCentre)
        Me.grpContrainer.Controls.Add(Me.btnExit)
        Me.grpContrainer.Controls.Add(Me.btnSearch)
        Me.grpContrainer.Controls.Add(Me.btnNew)
        Me.grpContrainer.Controls.Add(Me.dtpTo)
        Me.grpContrainer.Controls.Add(Me.dtpFrom)
        Me.grpContrainer.Controls.Add(Me.Label3)
        Me.grpContrainer.Controls.Add(Me.Label2)
        Me.grpContrainer.Controls.Add(Me.Label1)
        Me.grpContrainer.Location = New System.Drawing.Point(226, 41)
        Me.grpContrainer.Name = "grpContrainer"
        Me.grpContrainer.Size = New System.Drawing.Size(349, 529)
        Me.grpContrainer.TabIndex = 0
        Me.grpContrainer.TabStop = False
        '
        'rbtFormat3
        '
        Me.rbtFormat3.AutoSize = True
        Me.rbtFormat3.Location = New System.Drawing.Point(255, 407)
        Me.rbtFormat3.Name = "rbtFormat3"
        Me.rbtFormat3.Size = New System.Drawing.Size(80, 17)
        Me.rbtFormat3.TabIndex = 15
        Me.rbtFormat3.TabStop = True
        Me.rbtFormat3.Text = "Format 3 "
        Me.rbtFormat3.UseVisualStyleBackColor = True
        '
        'chkPurchaseWt
        '
        Me.chkPurchaseWt.AutoSize = True
        Me.chkPurchaseWt.Checked = True
        Me.chkPurchaseWt.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkPurchaseWt.Location = New System.Drawing.Point(23, 438)
        Me.chkPurchaseWt.Name = "chkPurchaseWt"
        Me.chkPurchaseWt.Size = New System.Drawing.Size(216, 17)
        Me.chkPurchaseWt.TabIndex = 16
        Me.chkPurchaseWt.Text = "Purchase Weight Based On Touch"
        Me.chkPurchaseWt.UseVisualStyleBackColor = True
        Me.chkPurchaseWt.Visible = False
        '
        'chkMetalSelectAll
        '
        Me.chkMetalSelectAll.AutoSize = True
        Me.chkMetalSelectAll.Location = New System.Drawing.Point(23, 259)
        Me.chkMetalSelectAll.Name = "chkMetalSelectAll"
        Me.chkMetalSelectAll.Size = New System.Drawing.Size(56, 17)
        Me.chkMetalSelectAll.TabIndex = 8
        Me.chkMetalSelectAll.Text = "Metal"
        Me.chkMetalSelectAll.UseVisualStyleBackColor = True
        '
        'chklistMetal
        '
        Me.chklistMetal.FormattingEnabled = True
        Me.chklistMetal.Location = New System.Drawing.Point(20, 285)
        Me.chklistMetal.Name = "chklistMetal"
        Me.chklistMetal.Size = New System.Drawing.Size(312, 68)
        Me.chklistMetal.TabIndex = 9
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(20, 409)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(48, 13)
        Me.Label4.TabIndex = 12
        Me.Label4.Text = "Method"
        '
        'chkFormat2
        '
        Me.chkFormat2.AutoSize = True
        Me.chkFormat2.Location = New System.Drawing.Point(173, 407)
        Me.chkFormat2.Name = "chkFormat2"
        Me.chkFormat2.Size = New System.Drawing.Size(76, 17)
        Me.chkFormat2.TabIndex = 14
        Me.chkFormat2.TabStop = True
        Me.chkFormat2.Text = "Format 2"
        Me.chkFormat2.UseVisualStyleBackColor = True
        '
        'chkFormat1
        '
        Me.chkFormat1.AutoSize = True
        Me.chkFormat1.Checked = True
        Me.chkFormat1.Location = New System.Drawing.Point(89, 407)
        Me.chkFormat1.Name = "chkFormat1"
        Me.chkFormat1.Size = New System.Drawing.Size(76, 17)
        Me.chkFormat1.TabIndex = 13
        Me.chkFormat1.TabStop = True
        Me.chkFormat1.Text = "Format 1"
        Me.chkFormat1.UseVisualStyleBackColor = True
        '
        'cmbGroupBy
        '
        Me.cmbGroupBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbGroupBy.FormattingEnabled = True
        Me.cmbGroupBy.Location = New System.Drawing.Point(89, 371)
        Me.cmbGroupBy.Name = "cmbGroupBy"
        Me.cmbGroupBy.Size = New System.Drawing.Size(183, 21)
        Me.cmbGroupBy.TabIndex = 11
        '
        'chkCompanySelectAll
        '
        Me.chkCompanySelectAll.AutoSize = True
        Me.chkCompanySelectAll.Location = New System.Drawing.Point(23, 49)
        Me.chkCompanySelectAll.Name = "chkCompanySelectAll"
        Me.chkCompanySelectAll.Size = New System.Drawing.Size(81, 17)
        Me.chkCompanySelectAll.TabIndex = 4
        Me.chkCompanySelectAll.Text = "Company"
        Me.chkCompanySelectAll.UseVisualStyleBackColor = True
        '
        'chkLstCompany
        '
        Me.chkLstCompany.FormattingEnabled = True
        Me.chkLstCompany.Location = New System.Drawing.Point(20, 68)
        Me.chkLstCompany.Name = "chkLstCompany"
        Me.chkLstCompany.Size = New System.Drawing.Size(312, 84)
        Me.chkLstCompany.TabIndex = 5
        '
        'chkCostCentreSelectAll
        '
        Me.chkCostCentreSelectAll.AutoSize = True
        Me.chkCostCentreSelectAll.Location = New System.Drawing.Point(22, 159)
        Me.chkCostCentreSelectAll.Name = "chkCostCentreSelectAll"
        Me.chkCostCentreSelectAll.Size = New System.Drawing.Size(95, 17)
        Me.chkCostCentreSelectAll.TabIndex = 6
        Me.chkCostCentreSelectAll.Text = "Cost Centre"
        Me.chkCostCentreSelectAll.UseVisualStyleBackColor = True
        '
        'chkLstCostCentre
        '
        Me.chkLstCostCentre.FormattingEnabled = True
        Me.chkLstCostCentre.Location = New System.Drawing.Point(20, 182)
        Me.chkLstCostCentre.Name = "chkLstCostCentre"
        Me.chkLstCostCentre.Size = New System.Drawing.Size(312, 68)
        Me.chkLstCostCentre.TabIndex = 7
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(234, 473)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 19
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(12, 473)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 17
        Me.btnSearch.Text = "&Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(121, 473)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 18
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(239, 26)
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
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(89, 26)
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
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(205, 29)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(20, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "To"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(20, 375)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(61, 13)
        Me.Label2.TabIndex = 10
        Me.Label2.Text = "Group By"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(19, 29)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(67, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Date From"
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
        'chkGP
        '
        Me.chkGP.AutoSize = True
        Me.chkGP.Location = New System.Drawing.Point(255, 438)
        Me.chkGP.Name = "chkGP"
        Me.chkGP.Size = New System.Drawing.Size(71, 17)
        Me.chkGP.TabIndex = 20
        Me.chkGP.Text = "Calc GP"
        Me.chkGP.UseVisualStyleBackColor = True
        '
        'TagWiseProfitLoss
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(748, 619)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.grpContrainer)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "TagWiseProfitLoss"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "TagWiseProfitLoss"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.grpContrainer.ResumeLayout(False)
        Me.grpContrainer.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpContrainer As System.Windows.Forms.GroupBox
    Friend WithEvents chkCompanySelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkLstCompany As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkCostCentreSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkLstCostCentre As System.Windows.Forms.CheckedListBox
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmbGroupBy As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents chkFormat2 As System.Windows.Forms.RadioButton
    Friend WithEvents chkFormat1 As System.Windows.Forms.RadioButton
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents chkMetalSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chklistMetal As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkPurchaseWt As System.Windows.Forms.CheckBox
    Friend WithEvents rbtFormat3 As RadioButton
    Friend WithEvents chkGP As CheckBox
End Class
