<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCounterItemGrp
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
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.tabMain = New System.Windows.Forms.TabControl
        Me.tabTarget = New System.Windows.Forms.TabPage
        Me.Label1 = New System.Windows.Forms.Label
        Me.cmbItemName = New System.Windows.Forms.ComboBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.cmbItemCounter = New System.Windows.Forms.ComboBox
        Me.gridViewTarget = New System.Windows.Forms.DataGridView
        Me.Label5 = New System.Windows.Forms.Label
        Me.btnSave1 = New System.Windows.Forms.Button
        Me.btnOpen1 = New System.Windows.Forms.Button
        Me.btnNew1 = New System.Windows.Forms.Button
        Me.btnExit1 = New System.Windows.Forms.Button
        Me.btnDelete1 = New System.Windows.Forms.Button
        Me.ContextMenuStrip2 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SaveToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem
        Me.NewToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem
        Me.OpenToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem
        Me.ContextMenuStrip1.SuspendLayout()
        Me.tabMain.SuspendLayout()
        Me.tabTarget.SuspendLayout()
        CType(Me.gridViewTarget, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip2.SuspendLayout()
        Me.SuspendLayout()
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SaveToolStripMenuItem, Me.OpenToolStripMenuItem, Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(123, 92)
        '
        'SaveToolStripMenuItem
        '
        Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        Me.SaveToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.SaveToolStripMenuItem.Text = "Save"
        Me.SaveToolStripMenuItem.Visible = False
        '
        'OpenToolStripMenuItem
        '
        Me.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem"
        Me.OpenToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2
        Me.OpenToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.OpenToolStripMenuItem.Text = "Open"
        Me.OpenToolStripMenuItem.Visible = False
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'tabMain
        '
        Me.tabMain.Controls.Add(Me.tabTarget)
        Me.tabMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabMain.Location = New System.Drawing.Point(0, 0)
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(736, 323)
        Me.tabMain.TabIndex = 0
        '
        'tabTarget
        '
        Me.tabTarget.Controls.Add(Me.Label1)
        Me.tabTarget.Controls.Add(Me.cmbItemName)
        Me.tabTarget.Controls.Add(Me.Label7)
        Me.tabTarget.Controls.Add(Me.cmbItemCounter)
        Me.tabTarget.Controls.Add(Me.gridViewTarget)
        Me.tabTarget.Controls.Add(Me.Label5)
        Me.tabTarget.Controls.Add(Me.btnSave1)
        Me.tabTarget.Controls.Add(Me.btnOpen1)
        Me.tabTarget.Controls.Add(Me.btnNew1)
        Me.tabTarget.Controls.Add(Me.btnExit1)
        Me.tabTarget.Controls.Add(Me.btnDelete1)
        Me.tabTarget.Location = New System.Drawing.Point(4, 22)
        Me.tabTarget.Name = "tabTarget"
        Me.tabTarget.Padding = New System.Windows.Forms.Padding(3)
        Me.tabTarget.Size = New System.Drawing.Size(728, 297)
        Me.tabTarget.TabIndex = 1
        Me.tabTarget.Text = "Target"
        Me.tabTarget.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Location = New System.Drawing.Point(8, 37)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(71, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Item Name"
        '
        'cmbItemName
        '
        Me.cmbItemName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.cmbItemName.FormattingEnabled = True
        Me.cmbItemName.Location = New System.Drawing.Point(99, 33)
        Me.cmbItemName.Name = "cmbItemName"
        Me.cmbItemName.Size = New System.Drawing.Size(316, 21)
        Me.cmbItemName.TabIndex = 3
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.BackColor = System.Drawing.Color.Transparent
        Me.Label7.Location = New System.Drawing.Point(8, 10)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(84, 13)
        Me.Label7.TabIndex = 0
        Me.Label7.Text = "Item Counter"
        '
        'cmbItemCounter
        '
        Me.cmbItemCounter.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.cmbItemCounter.FormattingEnabled = True
        Me.cmbItemCounter.Location = New System.Drawing.Point(99, 6)
        Me.cmbItemCounter.Name = "cmbItemCounter"
        Me.cmbItemCounter.Size = New System.Drawing.Size(316, 21)
        Me.cmbItemCounter.TabIndex = 1
        '
        'gridViewTarget
        '
        Me.gridViewTarget.AllowUserToAddRows = False
        Me.gridViewTarget.AllowUserToDeleteRows = False
        Me.gridViewTarget.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridViewTarget.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.gridViewTarget.Location = New System.Drawing.Point(3, 97)
        Me.gridViewTarget.Name = "gridViewTarget"
        Me.gridViewTarget.ReadOnly = True
        Me.gridViewTarget.Size = New System.Drawing.Size(722, 197)
        Me.gridViewTarget.TabIndex = 12
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.ForeColor = System.Drawing.Color.Red
        Me.Label5.Location = New System.Drawing.Point(421, 41)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(103, 13)
        Me.Label5.TabIndex = 11
        Me.Label5.Text = "*Hit Enter to Edit"
        Me.Label5.Visible = False
        '
        'btnSave1
        '
        Me.btnSave1.Location = New System.Drawing.Point(99, 61)
        Me.btnSave1.Name = "btnSave1"
        Me.btnSave1.Size = New System.Drawing.Size(100, 30)
        Me.btnSave1.TabIndex = 6
        Me.btnSave1.Text = "Save [F1]"
        Me.btnSave1.UseVisualStyleBackColor = True
        '
        'btnOpen1
        '
        Me.btnOpen1.Location = New System.Drawing.Point(207, 61)
        Me.btnOpen1.Name = "btnOpen1"
        Me.btnOpen1.Size = New System.Drawing.Size(100, 30)
        Me.btnOpen1.TabIndex = 7
        Me.btnOpen1.Text = "Open [F2]"
        Me.btnOpen1.UseVisualStyleBackColor = True
        '
        'btnNew1
        '
        Me.btnNew1.Location = New System.Drawing.Point(315, 61)
        Me.btnNew1.Name = "btnNew1"
        Me.btnNew1.Size = New System.Drawing.Size(100, 30)
        Me.btnNew1.TabIndex = 8
        Me.btnNew1.Text = "New [F3]"
        Me.btnNew1.UseVisualStyleBackColor = True
        '
        'btnExit1
        '
        Me.btnExit1.Location = New System.Drawing.Point(423, 61)
        Me.btnExit1.Name = "btnExit1"
        Me.btnExit1.Size = New System.Drawing.Size(100, 30)
        Me.btnExit1.TabIndex = 9
        Me.btnExit1.Text = "Exit [F12]"
        Me.btnExit1.UseVisualStyleBackColor = True
        '
        'btnDelete1
        '
        Me.btnDelete1.Location = New System.Drawing.Point(531, 61)
        Me.btnDelete1.Name = "btnDelete1"
        Me.btnDelete1.Size = New System.Drawing.Size(100, 30)
        Me.btnDelete1.TabIndex = 10
        Me.btnDelete1.Text = "&Delete"
        Me.btnDelete1.UseVisualStyleBackColor = True
        '
        'ContextMenuStrip2
        '
        Me.ContextMenuStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SaveToolStripMenuItem1, Me.NewToolStripMenuItem1, Me.ExitToolStripMenuItem1, Me.OpenToolStripMenuItem1})
        Me.ContextMenuStrip2.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip2.Size = New System.Drawing.Size(123, 92)
        '
        'SaveToolStripMenuItem1
        '
        Me.SaveToolStripMenuItem1.Name = "SaveToolStripMenuItem1"
        Me.SaveToolStripMenuItem1.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.SaveToolStripMenuItem1.Size = New System.Drawing.Size(122, 22)
        Me.SaveToolStripMenuItem1.Text = "Save"
        Me.SaveToolStripMenuItem1.Visible = False
        '
        'NewToolStripMenuItem1
        '
        Me.NewToolStripMenuItem1.Name = "NewToolStripMenuItem1"
        Me.NewToolStripMenuItem1.ShortcutKeys = System.Windows.Forms.Keys.F2
        Me.NewToolStripMenuItem1.Size = New System.Drawing.Size(122, 22)
        Me.NewToolStripMenuItem1.Text = "New"
        Me.NewToolStripMenuItem1.Visible = False
        '
        'ExitToolStripMenuItem1
        '
        Me.ExitToolStripMenuItem1.Name = "ExitToolStripMenuItem1"
        Me.ExitToolStripMenuItem1.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.ExitToolStripMenuItem1.Size = New System.Drawing.Size(122, 22)
        Me.ExitToolStripMenuItem1.Text = "Exit"
        Me.ExitToolStripMenuItem1.Visible = False
        '
        'OpenToolStripMenuItem1
        '
        Me.OpenToolStripMenuItem1.Name = "OpenToolStripMenuItem1"
        Me.OpenToolStripMenuItem1.ShortcutKeys = System.Windows.Forms.Keys.F4
        Me.OpenToolStripMenuItem1.Size = New System.Drawing.Size(122, 22)
        Me.OpenToolStripMenuItem1.Text = "Open"
        Me.OpenToolStripMenuItem1.Visible = False
        '
        'frmCounterItemGrp
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(736, 323)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.tabMain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmCounterItemGrp"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "COUNTER ITEM GROUP"
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.tabMain.ResumeLayout(False)
        Me.tabTarget.ResumeLayout(False)
        Me.tabTarget.PerformLayout()
        CType(Me.gridViewTarget, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tabMain As System.Windows.Forms.TabControl
    Friend WithEvents tabTarget As System.Windows.Forms.TabPage
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents cmbItemCounter As System.Windows.Forms.ComboBox
    Friend WithEvents gridViewTarget As System.Windows.Forms.DataGridView
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents btnSave1 As System.Windows.Forms.Button
    Friend WithEvents btnOpen1 As System.Windows.Forms.Button
    Friend WithEvents btnNew1 As System.Windows.Forms.Button
    Friend WithEvents btnExit1 As System.Windows.Forms.Button
    Friend WithEvents btnDelete1 As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip2 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents SaveToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmbItemName As System.Windows.Forms.ComboBox
End Class
