<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TagReserved
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
        Me.grpContrainer = New System.Windows.Forms.GroupBox
        Me.grpStatus = New System.Windows.Forms.GroupBox
        Me.rbtDelivered = New System.Windows.Forms.RadioButton
        Me.rbtPending = New System.Windows.Forms.RadioButton
        Me.rbtAll = New System.Windows.Forms.RadioButton
        Me.chkASD = New System.Windows.Forms.CheckBox
        Me.ChkInclBK = New System.Windows.Forms.CheckBox
        Me.chkCounter = New System.Windows.Forms.CheckBox
        Me.chkAllCostCentre = New System.Windows.Forms.CheckBox
        Me.chkLstCostCentre = New System.Windows.Forms.CheckedListBox
        Me.chkCompanySelectAll = New System.Windows.Forms.CheckBox
        Me.chkLstCompany = New System.Windows.Forms.CheckedListBox
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnSearch = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.lbToDate = New System.Windows.Forms.Label
        Me.chkItemWise = New System.Windows.Forms.CheckBox
        Me.ContextMenuStrip1.SuspendLayout()
        Me.grpContrainer.SuspendLayout()
        Me.grpStatus.SuspendLayout()
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
        'grpContrainer
        '
        Me.grpContrainer.Controls.Add(Me.chkItemWise)
        Me.grpContrainer.Controls.Add(Me.grpStatus)
        Me.grpContrainer.Controls.Add(Me.chkASD)
        Me.grpContrainer.Controls.Add(Me.ChkInclBK)
        Me.grpContrainer.Controls.Add(Me.chkCounter)
        Me.grpContrainer.Controls.Add(Me.chkAllCostCentre)
        Me.grpContrainer.Controls.Add(Me.chkLstCostCentre)
        Me.grpContrainer.Controls.Add(Me.chkCompanySelectAll)
        Me.grpContrainer.Controls.Add(Me.chkLstCompany)
        Me.grpContrainer.Controls.Add(Me.btnExit)
        Me.grpContrainer.Controls.Add(Me.btnSearch)
        Me.grpContrainer.Controls.Add(Me.btnNew)
        Me.grpContrainer.Controls.Add(Me.dtpTo)
        Me.grpContrainer.Controls.Add(Me.dtpFrom)
        Me.grpContrainer.Controls.Add(Me.lbToDate)
        Me.grpContrainer.Location = New System.Drawing.Point(337, 77)
        Me.grpContrainer.Name = "grpContrainer"
        Me.grpContrainer.Size = New System.Drawing.Size(349, 346)
        Me.grpContrainer.TabIndex = 0
        Me.grpContrainer.TabStop = False
        '
        'grpStatus
        '
        Me.grpStatus.Controls.Add(Me.rbtDelivered)
        Me.grpStatus.Controls.Add(Me.rbtPending)
        Me.grpStatus.Controls.Add(Me.rbtAll)
        Me.grpStatus.Location = New System.Drawing.Point(22, 260)
        Me.grpStatus.Name = "grpStatus"
        Me.grpStatus.Size = New System.Drawing.Size(310, 32)
        Me.grpStatus.TabIndex = 11
        Me.grpStatus.TabStop = False
        '
        'rbtDelivered
        '
        Me.rbtDelivered.AutoSize = True
        Me.rbtDelivered.Location = New System.Drawing.Point(217, 11)
        Me.rbtDelivered.Name = "rbtDelivered"
        Me.rbtDelivered.Size = New System.Drawing.Size(80, 17)
        Me.rbtDelivered.TabIndex = 2
        Me.rbtDelivered.TabStop = True
        Me.rbtDelivered.Text = "Delivered"
        Me.rbtDelivered.UseVisualStyleBackColor = True
        '
        'rbtPending
        '
        Me.rbtPending.AutoSize = True
        Me.rbtPending.Location = New System.Drawing.Point(104, 11)
        Me.rbtPending.Name = "rbtPending"
        Me.rbtPending.Size = New System.Drawing.Size(70, 17)
        Me.rbtPending.TabIndex = 1
        Me.rbtPending.TabStop = True
        Me.rbtPending.Text = "Pending"
        Me.rbtPending.UseVisualStyleBackColor = True
        '
        'rbtAll
        '
        Me.rbtAll.AutoSize = True
        Me.rbtAll.Checked = True
        Me.rbtAll.Location = New System.Drawing.Point(7, 11)
        Me.rbtAll.Name = "rbtAll"
        Me.rbtAll.Size = New System.Drawing.Size(39, 17)
        Me.rbtAll.TabIndex = 0
        Me.rbtAll.TabStop = True
        Me.rbtAll.Text = "All"
        Me.rbtAll.UseVisualStyleBackColor = True
        '
        'chkASD
        '
        Me.chkASD.AutoSize = True
        Me.chkASD.Location = New System.Drawing.Point(20, 28)
        Me.chkASD.Name = "chkASD"
        Me.chkASD.Size = New System.Drawing.Size(91, 17)
        Me.chkASD.TabIndex = 0
        Me.chkASD.Text = "As On Date"
        Me.chkASD.UseVisualStyleBackColor = True
        '
        'ChkInclBK
        '
        Me.ChkInclBK.Location = New System.Drawing.Point(20, 243)
        Me.ChkInclBK.Name = "ChkInclBK"
        Me.ChkInclBK.Size = New System.Drawing.Size(145, 21)
        Me.ChkInclBK.TabIndex = 10
        Me.ChkInclBK.Text = "With Direct Marking"
        Me.ChkInclBK.UseVisualStyleBackColor = True
        '
        'chkCounter
        '
        Me.chkCounter.Location = New System.Drawing.Point(20, 219)
        Me.chkCounter.Name = "chkCounter"
        Me.chkCounter.Size = New System.Drawing.Size(145, 21)
        Me.chkCounter.TabIndex = 8
        Me.chkCounter.Text = "With Item Counter"
        Me.chkCounter.UseVisualStyleBackColor = True
        '
        'chkAllCostCentre
        '
        Me.chkAllCostCentre.AutoSize = True
        Me.chkAllCostCentre.Location = New System.Drawing.Point(22, 143)
        Me.chkAllCostCentre.Name = "chkAllCostCentre"
        Me.chkAllCostCentre.Size = New System.Drawing.Size(95, 17)
        Me.chkAllCostCentre.TabIndex = 6
        Me.chkAllCostCentre.Text = "Cost Centre"
        Me.chkAllCostCentre.UseVisualStyleBackColor = True
        '
        'chkLstCostCentre
        '
        Me.chkLstCostCentre.FormattingEnabled = True
        Me.chkLstCostCentre.Location = New System.Drawing.Point(20, 162)
        Me.chkLstCostCentre.Name = "chkLstCostCentre"
        Me.chkLstCostCentre.Size = New System.Drawing.Size(312, 52)
        Me.chkLstCostCentre.TabIndex = 7
        '
        'chkCompanySelectAll
        '
        Me.chkCompanySelectAll.Location = New System.Drawing.Point(20, 61)
        Me.chkCompanySelectAll.Name = "chkCompanySelectAll"
        Me.chkCompanySelectAll.Size = New System.Drawing.Size(110, 21)
        Me.chkCompanySelectAll.TabIndex = 4
        Me.chkCompanySelectAll.Text = "Company"
        Me.chkCompanySelectAll.UseVisualStyleBackColor = True
        '
        'chkLstCompany
        '
        Me.chkLstCompany.FormattingEnabled = True
        Me.chkLstCompany.Location = New System.Drawing.Point(20, 88)
        Me.chkLstCompany.Name = "chkLstCompany"
        Me.chkLstCompany.Size = New System.Drawing.Size(312, 36)
        Me.chkLstCompany.TabIndex = 5
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(232, 298)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 14
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(20, 298)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 12
        Me.btnSearch.Text = "&Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(126, 298)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 13
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
        Me.dtpFrom.Location = New System.Drawing.Point(113, 26)
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
        'lbToDate
        '
        Me.lbToDate.AutoSize = True
        Me.lbToDate.Location = New System.Drawing.Point(212, 29)
        Me.lbToDate.Name = "lbToDate"
        Me.lbToDate.Size = New System.Drawing.Size(21, 13)
        Me.lbToDate.TabIndex = 2
        Me.lbToDate.Text = "To"
        '
        'chkItemWise
        '
        Me.chkItemWise.Location = New System.Drawing.Point(161, 219)
        Me.chkItemWise.Name = "chkItemWise"
        Me.chkItemWise.Size = New System.Drawing.Size(87, 21)
        Me.chkItemWise.TabIndex = 9
        Me.chkItemWise.Text = "Item Wise"
        Me.chkItemWise.UseVisualStyleBackColor = True
        '
        'TagReserved
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1022, 490)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.grpContrainer)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "TagReserved"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Tag Reserved"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.grpContrainer.ResumeLayout(False)
        Me.grpContrainer.PerformLayout()
        Me.grpStatus.ResumeLayout(False)
        Me.grpStatus.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents grpContrainer As System.Windows.Forms.GroupBox
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents lbToDate As System.Windows.Forms.Label
    Friend WithEvents chkCompanySelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkLstCompany As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkLstCostCentre As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkAllCostCentre As System.Windows.Forms.CheckBox
    Friend WithEvents chkCounter As System.Windows.Forms.CheckBox
    Friend WithEvents ChkInclBK As System.Windows.Forms.CheckBox
    Friend WithEvents chkASD As System.Windows.Forms.CheckBox
    Friend WithEvents grpStatus As System.Windows.Forms.GroupBox
    Friend WithEvents rbtDelivered As System.Windows.Forms.RadioButton
    Friend WithEvents rbtPending As System.Windows.Forms.RadioButton
    Friend WithEvents rbtAll As System.Windows.Forms.RadioButton
    Friend WithEvents chkItemWise As System.Windows.Forms.CheckBox
End Class
