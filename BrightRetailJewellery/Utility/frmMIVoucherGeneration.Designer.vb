<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMIVoucherGeneration
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
        Me.grpEstimateWeight = New CodeVendor.Controls.Grouper()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.cmbMetal = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.cmbCostCentreName = New System.Windows.Forms.ComboBox()
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.Label2 = New System.Windows.Forms.Label()
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ChkDatePosting = New System.Windows.Forms.CheckBox()
        Me.grpEstimateWeight.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpEstimateWeight
        '
        Me.grpEstimateWeight.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpEstimateWeight.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpEstimateWeight.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpEstimateWeight.BorderColor = System.Drawing.Color.Transparent
        Me.grpEstimateWeight.BorderThickness = 1.0!
        Me.grpEstimateWeight.Controls.Add(Me.ChkDatePosting)
        Me.grpEstimateWeight.Controls.Add(Me.Label5)
        Me.grpEstimateWeight.Controls.Add(Me.cmbMetal)
        Me.grpEstimateWeight.Controls.Add(Me.Label3)
        Me.grpEstimateWeight.Controls.Add(Me.cmbCostCentreName)
        Me.grpEstimateWeight.Controls.Add(Me.dtpTo)
        Me.grpEstimateWeight.Controls.Add(Me.Label2)
        Me.grpEstimateWeight.Controls.Add(Me.dtpFrom)
        Me.grpEstimateWeight.Controls.Add(Me.Label1)
        Me.grpEstimateWeight.Controls.Add(Me.btnExit)
        Me.grpEstimateWeight.Controls.Add(Me.btnSave)
        Me.grpEstimateWeight.Controls.Add(Me.btnNew)
        Me.grpEstimateWeight.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpEstimateWeight.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpEstimateWeight.GroupImage = Nothing
        Me.grpEstimateWeight.GroupTitle = ""
        Me.grpEstimateWeight.Location = New System.Drawing.Point(0, 0)
        Me.grpEstimateWeight.Name = "grpEstimateWeight"
        Me.grpEstimateWeight.Padding = New System.Windows.Forms.Padding(20)
        Me.grpEstimateWeight.PaintGroupBox = False
        Me.grpEstimateWeight.RoundCorners = 10
        Me.grpEstimateWeight.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpEstimateWeight.ShadowControl = False
        Me.grpEstimateWeight.ShadowThickness = 3
        Me.grpEstimateWeight.Size = New System.Drawing.Size(345, 181)
        Me.grpEstimateWeight.TabIndex = 0
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(6, 111)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(75, 21)
        Me.Label5.TabIndex = 6
        Me.Label5.Text = "Metal"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbMetal
        '
        Me.cmbMetal.FormattingEnabled = True
        Me.cmbMetal.Location = New System.Drawing.Point(85, 111)
        Me.cmbMetal.Name = "cmbMetal"
        Me.cmbMetal.Size = New System.Drawing.Size(222, 21)
        Me.cmbMetal.TabIndex = 7
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(6, 84)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(78, 21)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Cost Centre"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbCostCentreName
        '
        Me.cmbCostCentreName.FormattingEnabled = True
        Me.cmbCostCentreName.Location = New System.Drawing.Point(85, 84)
        Me.cmbCostCentreName.Name = "cmbCostCentreName"
        Me.cmbCostCentreName.Size = New System.Drawing.Size(220, 21)
        Me.cmbCostCentreName.TabIndex = 5
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(211, 57)
        Me.dtpTo.Mask = "##/##/####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(94, 21)
        Me.dtpTo.TabIndex = 3
        Me.dtpTo.Text = "07/03/9998"
        Me.dtpTo.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(185, 61)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(20, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "To"
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(85, 57)
        Me.dtpFrom.Mask = "##/##/####"
        Me.dtpFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom.Size = New System.Drawing.Size(94, 21)
        Me.dtpFrom.TabIndex = 1
        Me.dtpFrom.Text = "07/03/9998"
        Me.dtpFrom.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 61)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(67, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Date From"
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(224, 139)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 10
        Me.btnExit.Text = "&Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(12, 139)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(100, 30)
        Me.btnSave.TabIndex = 8
        Me.btnSave.Text = "Generate"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(118, 139)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 9
        Me.btnNew.Text = "&New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SaveToolStripMenuItem, Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(119, 70)
        '
        'SaveToolStripMenuItem
        '
        Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        Me.SaveToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(118, 22)
        Me.SaveToolStripMenuItem.Text = "Save"
        Me.SaveToolStripMenuItem.Visible = False
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(118, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(118, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'ChkDatePosting
        '
        Me.ChkDatePosting.AutoSize = True
        Me.ChkDatePosting.Location = New System.Drawing.Point(85, 33)
        Me.ChkDatePosting.Name = "ChkDatePosting"
        Me.ChkDatePosting.Size = New System.Drawing.Size(205, 17)
        Me.ChkDatePosting.TabIndex = 11
        Me.ChkDatePosting.Text = "Posting Based On Current Date"
        Me.ChkDatePosting.UseVisualStyleBackColor = True
        '
        'frmMIVoucherGeneration
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(345, 181)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpEstimateWeight)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmMIVoucherGeneration"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "MiscIssue Voucher Generation"
        Me.grpEstimateWeight.ResumeLayout(False)
        Me.grpEstimateWeight.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpEstimateWeight As CodeVendor.Controls.Grouper
    Friend WithEvents btnSave As Button
    Friend WithEvents btnNew As Button
    Friend WithEvents btnExit As Button
    Friend WithEvents ContextMenuStrip1 As ContextMenuStrip
    Friend WithEvents SaveToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents Label1 As Label
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents cmbCostCentreName As ComboBox
    Friend WithEvents Label5 As Label
    Friend WithEvents cmbMetal As ComboBox
    Friend WithEvents ChkDatePosting As CheckBox
End Class
