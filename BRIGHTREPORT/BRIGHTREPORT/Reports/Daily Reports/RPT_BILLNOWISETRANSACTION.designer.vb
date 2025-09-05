<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RPT_BILLNOWISETRANSACTION
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
        Me.GrpContainer = New System.Windows.Forms.GroupBox
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.rbtFormat1 = New System.Windows.Forms.RadioButton
        Me.rbtFormat2 = New System.Windows.Forms.RadioButton
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.rbtGrtwght = New System.Windows.Forms.RadioButton
        Me.rbtBoth = New System.Windows.Forms.RadioButton
        Me.rbtNetWght = New System.Windows.Forms.RadioButton
        Me.cmbCompany = New BrighttechPack.CheckedComboBox
        Me.chkCmbDesigner = New BrighttechPack.CheckedComboBox
        Me.Label = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.chkCmbCostCentre = New BrighttechPack.CheckedComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnSearch = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.rbtFormat3 = New System.Windows.Forms.RadioButton
        Me.GrpContainer.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GrpContainer
        '
        Me.GrpContainer.Controls.Add(Me.GroupBox2)
        Me.GrpContainer.Controls.Add(Me.GroupBox1)
        Me.GrpContainer.Controls.Add(Me.cmbCompany)
        Me.GrpContainer.Controls.Add(Me.chkCmbDesigner)
        Me.GrpContainer.Controls.Add(Me.Label)
        Me.GrpContainer.Controls.Add(Me.Label3)
        Me.GrpContainer.Controls.Add(Me.Label6)
        Me.GrpContainer.Controls.Add(Me.chkCmbCostCentre)
        Me.GrpContainer.Controls.Add(Me.Label1)
        Me.GrpContainer.Controls.Add(Me.dtpTo)
        Me.GrpContainer.Controls.Add(Me.dtpFrom)
        Me.GrpContainer.Controls.Add(Me.btnExit)
        Me.GrpContainer.Controls.Add(Me.btnSearch)
        Me.GrpContainer.Controls.Add(Me.btnNew)
        Me.GrpContainer.Location = New System.Drawing.Point(151, 89)
        Me.GrpContainer.Name = "GrpContainer"
        Me.GrpContainer.Size = New System.Drawing.Size(630, 245)
        Me.GrpContainer.TabIndex = 0
        Me.GrpContainer.TabStop = False
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.rbtFormat3)
        Me.GroupBox2.Controls.Add(Me.rbtFormat1)
        Me.GroupBox2.Controls.Add(Me.rbtFormat2)
        Me.GroupBox2.Location = New System.Drawing.Point(130, 129)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(270, 38)
        Me.GroupBox2.TabIndex = 6
        Me.GroupBox2.TabStop = False
        '
        'rbtFormat1
        '
        Me.rbtFormat1.AutoSize = True
        Me.rbtFormat1.Checked = True
        Me.rbtFormat1.Location = New System.Drawing.Point(6, 14)
        Me.rbtFormat1.Name = "rbtFormat1"
        Me.rbtFormat1.Size = New System.Drawing.Size(76, 17)
        Me.rbtFormat1.TabIndex = 0
        Me.rbtFormat1.TabStop = True
        Me.rbtFormat1.Text = "Format 1"
        Me.rbtFormat1.UseVisualStyleBackColor = True
        '
        'rbtFormat2
        '
        Me.rbtFormat2.AutoSize = True
        Me.rbtFormat2.Location = New System.Drawing.Point(88, 14)
        Me.rbtFormat2.Name = "rbtFormat2"
        Me.rbtFormat2.Size = New System.Drawing.Size(76, 17)
        Me.rbtFormat2.TabIndex = 1
        Me.rbtFormat2.Text = "Format 2"
        Me.rbtFormat2.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.rbtGrtwght)
        Me.GroupBox1.Controls.Add(Me.rbtBoth)
        Me.GroupBox1.Controls.Add(Me.rbtNetWght)
        Me.GroupBox1.Location = New System.Drawing.Point(130, 85)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(270, 38)
        Me.GroupBox1.TabIndex = 5
        Me.GroupBox1.TabStop = False
        '
        'rbtGrtwght
        '
        Me.rbtGrtwght.AutoSize = True
        Me.rbtGrtwght.Checked = True
        Me.rbtGrtwght.Location = New System.Drawing.Point(6, 14)
        Me.rbtGrtwght.Name = "rbtGrtwght"
        Me.rbtGrtwght.Size = New System.Drawing.Size(92, 17)
        Me.rbtGrtwght.TabIndex = 0
        Me.rbtGrtwght.TabStop = True
        Me.rbtGrtwght.Text = "Grs. Weight"
        Me.rbtGrtwght.UseVisualStyleBackColor = True
        '
        'rbtBoth
        '
        Me.rbtBoth.AutoSize = True
        Me.rbtBoth.Location = New System.Drawing.Point(212, 14)
        Me.rbtBoth.Name = "rbtBoth"
        Me.rbtBoth.Size = New System.Drawing.Size(51, 17)
        Me.rbtBoth.TabIndex = 2
        Me.rbtBoth.Text = "Both"
        Me.rbtBoth.UseVisualStyleBackColor = True
        '
        'rbtNetWght
        '
        Me.rbtNetWght.AutoSize = True
        Me.rbtNetWght.Location = New System.Drawing.Point(116, 14)
        Me.rbtNetWght.Name = "rbtNetWght"
        Me.rbtNetWght.Size = New System.Drawing.Size(87, 17)
        Me.rbtNetWght.TabIndex = 1
        Me.rbtNetWght.Text = "Net Weight"
        Me.rbtNetWght.UseVisualStyleBackColor = True
        '
        'cmbCompany
        '
        Me.cmbCompany.CheckOnClick = True
        Me.cmbCompany.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.cmbCompany.DropDownHeight = 1
        Me.cmbCompany.FormattingEnabled = True
        Me.cmbCompany.IntegralHeight = False
        Me.cmbCompany.Location = New System.Drawing.Point(130, 59)
        Me.cmbCompany.Name = "cmbCompany"
        Me.cmbCompany.Size = New System.Drawing.Size(444, 22)
        Me.cmbCompany.TabIndex = 4
        Me.cmbCompany.ValueSeparator = ", "
        '
        'chkCmbDesigner
        '
        Me.chkCmbDesigner.CheckOnClick = True
        Me.chkCmbDesigner.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbDesigner.DropDownHeight = 1
        Me.chkCmbDesigner.FormattingEnabled = True
        Me.chkCmbDesigner.IntegralHeight = False
        Me.chkCmbDesigner.Location = New System.Drawing.Point(130, 214)
        Me.chkCmbDesigner.Name = "chkCmbDesigner"
        Me.chkCmbDesigner.Size = New System.Drawing.Size(151, 22)
        Me.chkCmbDesigner.TabIndex = 7
        Me.chkCmbDesigner.ValueSeparator = ", "
        Me.chkCmbDesigner.Visible = False
        '
        'Label
        '
        Me.Label.AutoSize = True
        Me.Label.Location = New System.Drawing.Point(291, 219)
        Me.Label.Name = "Label"
        Me.Label.Size = New System.Drawing.Size(76, 13)
        Me.Label.TabIndex = 8
        Me.Label.Text = "Cost Centre"
        Me.Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label.Visible = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(26, 64)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(62, 13)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "Company"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(26, 219)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(58, 13)
        Me.Label6.TabIndex = 6
        Me.Label6.Text = "Designer"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label6.Visible = False
        '
        'chkCmbCostCentre
        '
        Me.chkCmbCostCentre.CheckOnClick = True
        Me.chkCmbCostCentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCostCentre.DropDownHeight = 1
        Me.chkCmbCostCentre.FormattingEnabled = True
        Me.chkCmbCostCentre.IntegralHeight = False
        Me.chkCmbCostCentre.Location = New System.Drawing.Point(395, 214)
        Me.chkCmbCostCentre.Name = "chkCmbCostCentre"
        Me.chkCmbCostCentre.Size = New System.Drawing.Size(183, 22)
        Me.chkCmbCostCentre.TabIndex = 9
        Me.chkCmbCostCentre.ValueSeparator = ", "
        Me.chkCmbCostCentre.Visible = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(219, 32)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(21, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "To"
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(246, 29)
        Me.dtpTo.Mask = "##-##-####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(82, 21)
        Me.dtpTo.TabIndex = 2
        Me.dtpTo.Text = "29/09/2010"
        Me.dtpTo.Value = New Date(2010, 9, 29, 0, 0, 0, 0)
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(130, 29)
        Me.dtpFrom.Mask = "##-##-####"
        Me.dtpFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom.Size = New System.Drawing.Size(82, 21)
        Me.dtpFrom.TabIndex = 0
        Me.dtpFrom.Text = "29/09/2010"
        Me.dtpFrom.Value = New Date(2010, 9, 29, 0, 0, 0, 0)
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(342, 178)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 9
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(130, 178)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 7
        Me.btnSearch.Text = "&Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(236, 178)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 8
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExitToolStripMenuItem, Me.NewToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(129, 48)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'rbtFormat3
        '
        Me.rbtFormat3.AutoSize = True
        Me.rbtFormat3.Location = New System.Drawing.Point(177, 14)
        Me.rbtFormat3.Name = "rbtFormat3"
        Me.rbtFormat3.Size = New System.Drawing.Size(76, 17)
        Me.rbtFormat3.TabIndex = 2
        Me.rbtFormat3.Text = "Format 3"
        Me.rbtFormat3.UseVisualStyleBackColor = True
        '
        'RPT_BILLNOWISETRANSACTION
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(849, 500)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.GrpContainer)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.Name = "RPT_BILLNOWISETRANSACTION"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Daily Details"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.GrpContainer.ResumeLayout(False)
        Me.GrpContainer.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GrpContainer As System.Windows.Forms.GroupBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents chkCmbCostCentre As BrighttechPack.CheckedComboBox
    Friend WithEvents chkCmbDesigner As BrighttechPack.CheckedComboBox
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents cmbCompany As BrighttechPack.CheckedComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents rbtBoth As System.Windows.Forms.RadioButton
    Friend WithEvents rbtNetWght As System.Windows.Forms.RadioButton
    Friend WithEvents rbtGrtwght As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents rbtFormat1 As System.Windows.Forms.RadioButton
    Friend WithEvents rbtFormat2 As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents rbtFormat3 As System.Windows.Forms.RadioButton
End Class
