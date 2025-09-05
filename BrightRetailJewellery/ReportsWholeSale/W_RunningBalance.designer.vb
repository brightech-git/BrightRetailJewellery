<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class W_RunningBalance
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
        Me.grpControls = New System.Windows.Forms.GroupBox
        Me.cmbPartyName = New System.Windows.Forms.ComboBox
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.rdbExceptApproval = New System.Windows.Forms.RadioButton
        Me.rdbApproval = New System.Windows.Forms.RadioButton
        Me.rdbAll = New System.Windows.Forms.RadioButton
        Me.dtpFrom = New System.Windows.Forms.DateTimePicker
        Me.dtpTo = New System.Windows.Forms.DateTimePicker
        Me.lblDateFrom = New System.Windows.Forms.Label
        Me.lblPartyName = New System.Windows.Forms.Label
        Me.lblTo = New System.Windows.Forms.Label
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.btnView = New System.Windows.Forms.Button
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ViewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ViewAllToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.grpControls.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpControls
        '
        Me.grpControls.Controls.Add(Me.cmbPartyName)
        Me.grpControls.Controls.Add(Me.Panel2)
        Me.grpControls.Controls.Add(Me.dtpFrom)
        Me.grpControls.Controls.Add(Me.dtpTo)
        Me.grpControls.Controls.Add(Me.lblDateFrom)
        Me.grpControls.Controls.Add(Me.lblPartyName)
        Me.grpControls.Controls.Add(Me.lblTo)
        Me.grpControls.Controls.Add(Me.btnExit)
        Me.grpControls.Controls.Add(Me.btnNew)
        Me.grpControls.Controls.Add(Me.btnView)
        Me.grpControls.Location = New System.Drawing.Point(306, 171)
        Me.grpControls.Name = "grpControls"
        Me.grpControls.Size = New System.Drawing.Size(444, 201)
        Me.grpControls.TabIndex = 0
        Me.grpControls.TabStop = False
        '
        'cmbPartyName
        '
        Me.cmbPartyName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cmbPartyName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbPartyName.BackColor = System.Drawing.SystemColors.Window
        Me.cmbPartyName.FormattingEnabled = True
        Me.cmbPartyName.Location = New System.Drawing.Point(102, 32)
        Me.cmbPartyName.Name = "cmbPartyName"
        Me.cmbPartyName.Size = New System.Drawing.Size(320, 21)
        Me.cmbPartyName.TabIndex = 1
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.rdbExceptApproval)
        Me.Panel2.Controls.Add(Me.rdbApproval)
        Me.Panel2.Controls.Add(Me.rdbAll)
        Me.Panel2.Location = New System.Drawing.Point(102, 107)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(260, 23)
        Me.Panel2.TabIndex = 6
        '
        'rdbExceptApproval
        '
        Me.rdbExceptApproval.AutoSize = True
        Me.rdbExceptApproval.Location = New System.Drawing.Point(138, 3)
        Me.rdbExceptApproval.Name = "rdbExceptApproval"
        Me.rdbExceptApproval.Size = New System.Drawing.Size(118, 17)
        Me.rdbExceptApproval.TabIndex = 2
        Me.rdbExceptApproval.TabStop = True
        Me.rdbExceptApproval.Text = "Except Approval"
        Me.rdbExceptApproval.UseVisualStyleBackColor = True
        '
        'rdbApproval
        '
        Me.rdbApproval.AutoSize = True
        Me.rdbApproval.Location = New System.Drawing.Point(52, 3)
        Me.rdbApproval.Name = "rdbApproval"
        Me.rdbApproval.Size = New System.Drawing.Size(76, 17)
        Me.rdbApproval.TabIndex = 1
        Me.rdbApproval.TabStop = True
        Me.rdbApproval.Text = "Approval"
        Me.rdbApproval.UseVisualStyleBackColor = True
        '
        'rdbAll
        '
        Me.rdbAll.AutoSize = True
        Me.rdbAll.Location = New System.Drawing.Point(3, 3)
        Me.rdbAll.Name = "rdbAll"
        Me.rdbAll.Size = New System.Drawing.Size(39, 17)
        Me.rdbAll.TabIndex = 0
        Me.rdbAll.TabStop = True
        Me.rdbAll.Text = "All"
        Me.rdbAll.UseVisualStyleBackColor = True
        '
        'dtpFrom
        '
        Me.dtpFrom.CustomFormat = "dd/MM/yyyy"
        Me.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpFrom.Location = New System.Drawing.Point(102, 69)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Size = New System.Drawing.Size(111, 21)
        Me.dtpFrom.TabIndex = 3
        '
        'dtpTo
        '
        Me.dtpTo.CustomFormat = "dd/MM/yyyy"
        Me.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpTo.Location = New System.Drawing.Point(257, 69)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Size = New System.Drawing.Size(105, 21)
        Me.dtpTo.TabIndex = 5
        '
        'lblDateFrom
        '
        Me.lblDateFrom.AutoSize = True
        Me.lblDateFrom.Location = New System.Drawing.Point(22, 73)
        Me.lblDateFrom.Name = "lblDateFrom"
        Me.lblDateFrom.Size = New System.Drawing.Size(67, 13)
        Me.lblDateFrom.TabIndex = 2
        Me.lblDateFrom.Text = "Date From"
        '
        'lblPartyName
        '
        Me.lblPartyName.AutoSize = True
        Me.lblPartyName.Location = New System.Drawing.Point(22, 32)
        Me.lblPartyName.Name = "lblPartyName"
        Me.lblPartyName.Size = New System.Drawing.Size(74, 13)
        Me.lblPartyName.TabIndex = 0
        Me.lblPartyName.Text = "Party Name"
        '
        'lblTo
        '
        Me.lblTo.AutoSize = True
        Me.lblTo.Location = New System.Drawing.Point(219, 73)
        Me.lblTo.Name = "lblTo"
        Me.lblTo.Size = New System.Drawing.Size(21, 13)
        Me.lblTo.TabIndex = 4
        Me.lblTo.Text = "To"
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(313, 146)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 9
        Me.btnExit.Text = "&Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(207, 146)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 8
        Me.btnNew.Text = "&New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnView
        '
        Me.btnView.Location = New System.Drawing.Point(101, 146)
        Me.btnView.Name = "btnView"
        Me.btnView.Size = New System.Drawing.Size(100, 30)
        Me.btnView.TabIndex = 7
        Me.btnView.Text = "&View"
        Me.btnView.UseVisualStyleBackColor = True
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem, Me.ViewToolStripMenuItem, Me.ViewAllToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(138, 92)
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(137, 22)
        Me.NewToolStripMenuItem.Text = "&New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(137, 22)
        Me.ExitToolStripMenuItem.Text = "&Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'ViewToolStripMenuItem
        '
        Me.ViewToolStripMenuItem.Name = "ViewToolStripMenuItem"
        Me.ViewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2
        Me.ViewToolStripMenuItem.Size = New System.Drawing.Size(137, 22)
        Me.ViewToolStripMenuItem.Text = "&View"
        Me.ViewToolStripMenuItem.Visible = False
        '
        'ViewAllToolStripMenuItem
        '
        Me.ViewAllToolStripMenuItem.Name = "ViewAllToolStripMenuItem"
        Me.ViewAllToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.ViewAllToolStripMenuItem.Size = New System.Drawing.Size(137, 22)
        Me.ViewAllToolStripMenuItem.Text = "&ViewAll"
        Me.ViewAllToolStripMenuItem.Visible = False
        '
        'W_RunningBalance
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1021, 618)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.grpControls)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "W_RunningBalance"
        Me.Text = "Running Balance"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.grpControls.ResumeLayout(False)
        Me.grpControls.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblTo As System.Windows.Forms.Label
    Friend WithEvents lblDateFrom As System.Windows.Forms.Label
    Friend WithEvents dtpTo As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtpFrom As System.Windows.Forms.DateTimePicker
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnView As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmbPartyName As System.Windows.Forms.ComboBox
    Friend WithEvents lblPartyName As System.Windows.Forms.Label
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents rdbExceptApproval As System.Windows.Forms.RadioButton
    Friend WithEvents rdbApproval As System.Windows.Forms.RadioButton
    Friend WithEvents rdbAll As System.Windows.Forms.RadioButton
    Friend WithEvents ViewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents grpControls As System.Windows.Forms.GroupBox
    Friend WithEvents ViewAllToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class
