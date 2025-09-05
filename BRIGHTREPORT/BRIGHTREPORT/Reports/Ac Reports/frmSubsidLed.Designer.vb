<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSubsidLed
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
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.pnlContainer = New System.Windows.Forms.Panel
        Me.chkwithcancel = New System.Windows.Forms.CheckBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.cmbMetal = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnSearch = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.chkCmbAcName = New BrighttechPack.CheckedComboBox
        Me.chkCmbCostCentre = New BrighttechPack.CheckedComboBox
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.GroupBox1.SuspendLayout()
        Me.pnlContainer.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.pnlContainer)
        Me.GroupBox1.Location = New System.Drawing.Point(154, 107)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(456, 256)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        '
        'pnlContainer
        '
        Me.pnlContainer.Controls.Add(Me.chkwithcancel)
        Me.pnlContainer.Controls.Add(Me.chkCmbAcName)
        Me.pnlContainer.Controls.Add(Me.Label5)
        Me.pnlContainer.Controls.Add(Me.cmbMetal)
        Me.pnlContainer.Controls.Add(Me.Label2)
        Me.pnlContainer.Controls.Add(Me.chkCmbCostCentre)
        Me.pnlContainer.Controls.Add(Me.Label3)
        Me.pnlContainer.Controls.Add(Me.dtpTo)
        Me.pnlContainer.Controls.Add(Me.dtpFrom)
        Me.pnlContainer.Controls.Add(Me.Label4)
        Me.pnlContainer.Controls.Add(Me.Label1)
        Me.pnlContainer.Controls.Add(Me.btnExit)
        Me.pnlContainer.Controls.Add(Me.btnSearch)
        Me.pnlContainer.Controls.Add(Me.btnNew)
        Me.pnlContainer.Location = New System.Drawing.Point(6, 19)
        Me.pnlContainer.Name = "pnlContainer"
        Me.pnlContainer.Size = New System.Drawing.Size(437, 218)
        Me.pnlContainer.TabIndex = 0
        '
        'chkwithcancel
        '
        Me.chkwithcancel.AutoSize = True
        Me.chkwithcancel.Location = New System.Drawing.Point(101, 150)
        Me.chkwithcancel.Name = "chkwithcancel"
        Me.chkwithcancel.Size = New System.Drawing.Size(182, 17)
        Me.chkwithcancel.TabIndex = 10
        Me.chkwithcancel.Text = "With cancel  (Total Not included)"
        Me.chkwithcancel.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(19, 56)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(57, 13)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "A/C Name"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbMetal
        '
        Me.cmbMetal.FormattingEnabled = True
        Me.cmbMetal.Location = New System.Drawing.Point(100, 117)
        Me.cmbMetal.Name = "cmbMetal"
        Me.cmbMetal.Size = New System.Drawing.Size(290, 21)
        Me.cmbMetal.TabIndex = 9
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(19, 116)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(79, 21)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "Subsidy Book"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(19, 83)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(79, 21)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Cost Centre"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(203, 30)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(20, 13)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "To"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(19, 30)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(56, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Date From"
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(296, 174)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(90, 30)
        Me.btnExit.TabIndex = 14
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(100, 174)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(97, 30)
        Me.btnSearch.TabIndex = 12
        Me.btnSearch.Text = "&Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(203, 174)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(87, 30)
        Me.btnNew.TabIndex = 13
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
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
        'chkCmbAcName
        '
        Me.chkCmbAcName.CheckOnClick = True
        Me.chkCmbAcName.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbAcName.DropDownHeight = 1
        Me.chkCmbAcName.FormattingEnabled = True
        Me.chkCmbAcName.IntegralHeight = False
        Me.chkCmbAcName.Location = New System.Drawing.Point(100, 53)
        Me.chkCmbAcName.Name = "chkCmbAcName"
        Me.chkCmbAcName.Size = New System.Drawing.Size(290, 21)
        Me.chkCmbAcName.TabIndex = 5
        Me.chkCmbAcName.ValueSeparator = ", "
        '
        'chkCmbCostCentre
        '
        Me.chkCmbCostCentre.CheckOnClick = True
        Me.chkCmbCostCentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCostCentre.DropDownHeight = 1
        Me.chkCmbCostCentre.FormattingEnabled = True
        Me.chkCmbCostCentre.IntegralHeight = False
        Me.chkCmbCostCentre.Location = New System.Drawing.Point(100, 84)
        Me.chkCmbCostCentre.Name = "chkCmbCostCentre"
        Me.chkCmbCostCentre.Size = New System.Drawing.Size(290, 21)
        Me.chkCmbCostCentre.TabIndex = 7
        Me.chkCmbCostCentre.ValueSeparator = ", "
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(233, 27)
        Me.dtpTo.Mask = "##/##/####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(93, 20)
        Me.dtpTo.TabIndex = 3
        Me.dtpTo.Text = "07/03/9998"
        Me.dtpTo.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(100, 27)
        Me.dtpFrom.Mask = "##/##/####"
        Me.dtpFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom.Size = New System.Drawing.Size(93, 20)
        Me.dtpFrom.TabIndex = 1
        Me.dtpFrom.Text = "07/03/9998"
        Me.dtpFrom.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'frmSubsidLed
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(787, 503)
        Me.Controls.Add(Me.GroupBox1)
        Me.KeyPreview = True
        Me.Name = "frmSubsidLed"
        Me.Text = "SubsidyBook"
        Me.GroupBox1.ResumeLayout(False)
        Me.pnlContainer.ResumeLayout(False)
        Me.pnlContainer.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents pnlContainer As System.Windows.Forms.Panel
    Friend WithEvents cmbMetal As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents chkCmbCostCentre As BrighttechPack.CheckedComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents chkCmbAcName As BrighttechPack.CheckedComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents chkwithcancel As System.Windows.Forms.CheckBox
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class
