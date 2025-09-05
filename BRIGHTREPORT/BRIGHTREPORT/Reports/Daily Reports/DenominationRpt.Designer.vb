<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DenominationRpt
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
        Me.rbtPayment = New System.Windows.Forms.RadioButton
        Me.rbtReceipt = New System.Windows.Forms.RadioButton
        Me.rbtBoth = New System.Windows.Forms.RadioButton
        Me.Label2 = New System.Windows.Forms.Label
        Me.dtpDate = New BrighttechPack.DatePicker(Me.components)
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnSearch = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.chkGroupbyCashCounter = New System.Windows.Forms.CheckBox
        Me.txtSystemId = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.GrpContainer.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GrpContainer
        '
        Me.GrpContainer.Controls.Add(Me.txtSystemId)
        Me.GrpContainer.Controls.Add(Me.chkGroupbyCashCounter)
        Me.GrpContainer.Controls.Add(Me.rbtPayment)
        Me.GrpContainer.Controls.Add(Me.rbtReceipt)
        Me.GrpContainer.Controls.Add(Me.rbtBoth)
        Me.GrpContainer.Controls.Add(Me.Label1)
        Me.GrpContainer.Controls.Add(Me.Label2)
        Me.GrpContainer.Controls.Add(Me.dtpDate)
        Me.GrpContainer.Controls.Add(Me.btnExit)
        Me.GrpContainer.Controls.Add(Me.btnSearch)
        Me.GrpContainer.Controls.Add(Me.btnNew)
        Me.GrpContainer.Location = New System.Drawing.Point(199, 51)
        Me.GrpContainer.Name = "GrpContainer"
        Me.GrpContainer.Size = New System.Drawing.Size(385, 189)
        Me.GrpContainer.TabIndex = 0
        Me.GrpContainer.TabStop = False
        '
        'rbtPayment
        '
        Me.rbtPayment.AutoSize = True
        Me.rbtPayment.Location = New System.Drawing.Point(256, 57)
        Me.rbtPayment.Name = "rbtPayment"
        Me.rbtPayment.Size = New System.Drawing.Size(75, 17)
        Me.rbtPayment.TabIndex = 4
        Me.rbtPayment.TabStop = True
        Me.rbtPayment.Text = "Payment"
        Me.rbtPayment.UseVisualStyleBackColor = True
        '
        'rbtReceipt
        '
        Me.rbtReceipt.AutoSize = True
        Me.rbtReceipt.Location = New System.Drawing.Point(183, 57)
        Me.rbtReceipt.Name = "rbtReceipt"
        Me.rbtReceipt.Size = New System.Drawing.Size(67, 17)
        Me.rbtReceipt.TabIndex = 3
        Me.rbtReceipt.TabStop = True
        Me.rbtReceipt.Text = "Receipt"
        Me.rbtReceipt.UseVisualStyleBackColor = True
        '
        'rbtBoth
        '
        Me.rbtBoth.AutoSize = True
        Me.rbtBoth.Location = New System.Drawing.Point(129, 57)
        Me.rbtBoth.Name = "rbtBoth"
        Me.rbtBoth.Size = New System.Drawing.Size(51, 17)
        Me.rbtBoth.TabIndex = 2
        Me.rbtBoth.TabStop = True
        Me.rbtBoth.Text = "Both"
        Me.rbtBoth.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(63, 24)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(34, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Date"
        '
        'dtpDate
        '
        Me.dtpDate.Location = New System.Drawing.Point(129, 21)
        Me.dtpDate.Mask = "##-##-####"
        Me.dtpDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpDate.Name = "dtpDate"
        Me.dtpDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpDate.Size = New System.Drawing.Size(95, 21)
        Me.dtpDate.TabIndex = 1
        Me.dtpDate.Text = "29/09/2010"
        Me.dtpDate.Value = New Date(2010, 9, 29, 0, 0, 0, 0)
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(256, 144)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(117, 30)
        Me.btnExit.TabIndex = 10
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(10, 144)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(117, 30)
        Me.btnSearch.TabIndex = 8
        Me.btnSearch.Text = "&Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(133, 144)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(117, 30)
        Me.btnNew.TabIndex = 9
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
        'chkGroupbyCashCounter
        '
        Me.chkGroupbyCashCounter.AutoSize = True
        Me.chkGroupbyCashCounter.Location = New System.Drawing.Point(131, 86)
        Me.chkGroupbyCashCounter.Name = "chkGroupbyCashCounter"
        Me.chkGroupbyCashCounter.Size = New System.Drawing.Size(162, 17)
        Me.chkGroupbyCashCounter.TabIndex = 5
        Me.chkGroupbyCashCounter.Text = "Group by Cash Counter"
        Me.chkGroupbyCashCounter.UseVisualStyleBackColor = True
        '
        'txtSystemId
        '
        Me.txtSystemId.Location = New System.Drawing.Point(129, 109)
        Me.txtSystemId.Name = "txtSystemId"
        Me.txtSystemId.Size = New System.Drawing.Size(95, 21)
        Me.txtSystemId.TabIndex = 7
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(63, 112)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(60, 13)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Sytem Id"
        '
        'DenominationRpt
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(812, 448)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.GrpContainer)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "DenominationRpt"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "DenominationRpt"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.GrpContainer.ResumeLayout(False)
        Me.GrpContainer.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GrpContainer As System.Windows.Forms.GroupBox
    Friend WithEvents dtpDate As BrighttechPack.DatePicker
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents rbtPayment As System.Windows.Forms.RadioButton
    Friend WithEvents rbtReceipt As System.Windows.Forms.RadioButton
    Friend WithEvents rbtBoth As System.Windows.Forms.RadioButton
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents txtSystemId As System.Windows.Forms.TextBox
    Friend WithEvents chkGroupbyCashCounter As System.Windows.Forms.CheckBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
End Class
