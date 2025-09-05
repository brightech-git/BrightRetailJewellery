<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCentRateUpdate
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
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tabMain = New System.Windows.Forms.TabControl()
        Me.tabGeneral = New System.Windows.Forms.TabPage()
        Me.grpControl = New System.Windows.Forms.GroupBox()
        Me.txtToCent_WET = New System.Windows.Forms.TextBox()
        Me.txtFromCent_WET = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.chkWebTag = New System.Windows.Forms.CheckBox()
        Me.chkTag = New System.Windows.Forms.CheckBox()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.rbtMinValue = New System.Windows.Forms.RadioButton()
        Me.rbtMaxValue = New System.Windows.Forms.RadioButton()
        Me.btnView = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.chkLstSubItem = New System.Windows.Forms.CheckedListBox()
        Me.chkLstItem = New System.Windows.Forms.CheckedListBox()
        Me.chkSubItemSelectAll = New System.Windows.Forms.CheckBox()
        Me.chkItemSelectAll = New System.Windows.Forms.CheckBox()
        Me.tabView = New System.Windows.Forms.TabPage()
        Me.gridView = New System.Windows.Forms.DataGridView()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.btnBack = New System.Windows.Forms.Button()
        Me.btnUpdate = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.tabMain.SuspendLayout()
        Me.tabGeneral.SuspendLayout()
        Me.grpControl.SuspendLayout()
        Me.tabView.SuspendLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(119, 48)
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
        'tabMain
        '
        Me.tabMain.Controls.Add(Me.tabGeneral)
        Me.tabMain.Controls.Add(Me.tabView)
        Me.tabMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabMain.ItemSize = New System.Drawing.Size(57, 18)
        Me.tabMain.Location = New System.Drawing.Point(0, 0)
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(881, 483)
        Me.tabMain.TabIndex = 0
        '
        'tabGeneral
        '
        Me.tabGeneral.Controls.Add(Me.grpControl)
        Me.tabGeneral.Location = New System.Drawing.Point(4, 22)
        Me.tabGeneral.Name = "tabGeneral"
        Me.tabGeneral.Padding = New System.Windows.Forms.Padding(3)
        Me.tabGeneral.Size = New System.Drawing.Size(873, 457)
        Me.tabGeneral.TabIndex = 0
        Me.tabGeneral.Text = "General"
        Me.tabGeneral.UseVisualStyleBackColor = True
        '
        'grpControl
        '
        Me.grpControl.BackColor = System.Drawing.SystemColors.Control
        Me.grpControl.Controls.Add(Me.txtToCent_WET)
        Me.grpControl.Controls.Add(Me.txtFromCent_WET)
        Me.grpControl.Controls.Add(Me.Label2)
        Me.grpControl.Controls.Add(Me.Label1)
        Me.grpControl.Controls.Add(Me.chkWebTag)
        Me.grpControl.Controls.Add(Me.chkTag)
        Me.grpControl.Controls.Add(Me.btnNew)
        Me.grpControl.Controls.Add(Me.rbtMinValue)
        Me.grpControl.Controls.Add(Me.rbtMaxValue)
        Me.grpControl.Controls.Add(Me.btnView)
        Me.grpControl.Controls.Add(Me.btnExit)
        Me.grpControl.Controls.Add(Me.chkLstSubItem)
        Me.grpControl.Controls.Add(Me.chkLstItem)
        Me.grpControl.Controls.Add(Me.chkSubItemSelectAll)
        Me.grpControl.Controls.Add(Me.chkItemSelectAll)
        Me.grpControl.Location = New System.Drawing.Point(197, 87)
        Me.grpControl.Name = "grpControl"
        Me.grpControl.Size = New System.Drawing.Size(453, 331)
        Me.grpControl.TabIndex = 0
        Me.grpControl.TabStop = False
        '
        'txtToCent_WET
        '
        Me.txtToCent_WET.Location = New System.Drawing.Point(197, 259)
        Me.txtToCent_WET.Name = "txtToCent_WET"
        Me.txtToCent_WET.Size = New System.Drawing.Size(44, 21)
        Me.txtToCent_WET.TabIndex = 11
        '
        'txtFromCent_WET
        '
        Me.txtFromCent_WET.Location = New System.Drawing.Point(91, 259)
        Me.txtFromCent_WET.Name = "txtFromCent_WET"
        Me.txtFromCent_WET.Size = New System.Drawing.Size(44, 21)
        Me.txtFromCent_WET.TabIndex = 9
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(143, 262)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(47, 13)
        Me.Label2.TabIndex = 10
        Me.Label2.Text = "ToCent"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(22, 262)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(63, 13)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "FromCent"
        '
        'chkWebTag
        '
        Me.chkWebTag.AutoSize = True
        Me.chkWebTag.Location = New System.Drawing.Point(328, 234)
        Me.chkWebTag.Name = "chkWebTag"
        Me.chkWebTag.Size = New System.Drawing.Size(118, 17)
        Me.chkWebTag.TabIndex = 7
        Me.chkWebTag.Text = "Web Tag Update"
        Me.chkWebTag.UseVisualStyleBackColor = True
        '
        'chkTag
        '
        Me.chkTag.AutoSize = True
        Me.chkTag.Checked = True
        Me.chkTag.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkTag.Location = New System.Drawing.Point(231, 234)
        Me.chkTag.Name = "chkTag"
        Me.chkTag.Size = New System.Drawing.Size(90, 17)
        Me.chkTag.TabIndex = 6
        Me.chkTag.Text = "Tag Update"
        Me.chkTag.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(127, 287)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 13
        Me.btnNew.Text = "New [F1]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'rbtMinValue
        '
        Me.rbtMinValue.AutoSize = True
        Me.rbtMinValue.Location = New System.Drawing.Point(108, 234)
        Me.rbtMinValue.Name = "rbtMinValue"
        Me.rbtMinValue.Size = New System.Drawing.Size(79, 17)
        Me.rbtMinValue.TabIndex = 5
        Me.rbtMinValue.TabStop = True
        Me.rbtMinValue.Text = "Min Value"
        Me.rbtMinValue.UseVisualStyleBackColor = True
        '
        'rbtMaxValue
        '
        Me.rbtMaxValue.AutoSize = True
        Me.rbtMaxValue.Location = New System.Drawing.Point(18, 234)
        Me.rbtMaxValue.Name = "rbtMaxValue"
        Me.rbtMaxValue.Size = New System.Drawing.Size(83, 17)
        Me.rbtMaxValue.TabIndex = 4
        Me.rbtMaxValue.TabStop = True
        Me.rbtMaxValue.Text = "Max Value"
        Me.rbtMaxValue.UseVisualStyleBackColor = True
        '
        'btnView
        '
        Me.btnView.Location = New System.Drawing.Point(21, 287)
        Me.btnView.Name = "btnView"
        Me.btnView.Size = New System.Drawing.Size(100, 30)
        Me.btnView.TabIndex = 12
        Me.btnView.Text = "View"
        Me.btnView.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(231, 287)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 14
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'chkLstSubItem
        '
        Me.chkLstSubItem.FormattingEnabled = True
        Me.chkLstSubItem.Location = New System.Drawing.Point(231, 37)
        Me.chkLstSubItem.Name = "chkLstSubItem"
        Me.chkLstSubItem.Size = New System.Drawing.Size(201, 180)
        Me.chkLstSubItem.TabIndex = 3
        '
        'chkLstItem
        '
        Me.chkLstItem.FormattingEnabled = True
        Me.chkLstItem.Location = New System.Drawing.Point(18, 37)
        Me.chkLstItem.Name = "chkLstItem"
        Me.chkLstItem.Size = New System.Drawing.Size(201, 180)
        Me.chkLstItem.TabIndex = 1
        '
        'chkSubItemSelectAll
        '
        Me.chkSubItemSelectAll.AutoSize = True
        Me.chkSubItemSelectAll.Location = New System.Drawing.Point(234, 19)
        Me.chkSubItemSelectAll.Name = "chkSubItemSelectAll"
        Me.chkSubItemSelectAll.Size = New System.Drawing.Size(79, 17)
        Me.chkSubItemSelectAll.TabIndex = 2
        Me.chkSubItemSelectAll.Text = "Sub Item"
        Me.chkSubItemSelectAll.UseVisualStyleBackColor = True
        '
        'chkItemSelectAll
        '
        Me.chkItemSelectAll.AutoSize = True
        Me.chkItemSelectAll.Location = New System.Drawing.Point(21, 19)
        Me.chkItemSelectAll.Name = "chkItemSelectAll"
        Me.chkItemSelectAll.Size = New System.Drawing.Size(53, 17)
        Me.chkItemSelectAll.TabIndex = 0
        Me.chkItemSelectAll.Text = "Item"
        Me.chkItemSelectAll.UseVisualStyleBackColor = True
        '
        'tabView
        '
        Me.tabView.Controls.Add(Me.gridView)
        Me.tabView.Controls.Add(Me.Panel2)
        Me.tabView.Controls.Add(Me.Panel1)
        Me.tabView.Location = New System.Drawing.Point(4, 22)
        Me.tabView.Name = "tabView"
        Me.tabView.Padding = New System.Windows.Forms.Padding(3)
        Me.tabView.Size = New System.Drawing.Size(873, 457)
        Me.tabView.TabIndex = 1
        Me.tabView.Text = "View"
        Me.tabView.UseVisualStyleBackColor = True
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView.Location = New System.Drawing.Point(8, 45)
        Me.gridView.MultiSelect = False
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.Size = New System.Drawing.Size(862, 409)
        Me.gridView.TabIndex = 2
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.btnExport)
        Me.Panel2.Controls.Add(Me.btnBack)
        Me.Panel2.Controls.Add(Me.btnUpdate)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel2.Location = New System.Drawing.Point(8, 3)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(862, 42)
        Me.Panel2.TabIndex = 32
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(229, 5)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 31
        Me.btnExport.Text = "&Export"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'btnBack
        '
        Me.btnBack.Location = New System.Drawing.Point(117, 5)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(100, 30)
        Me.btnBack.TabIndex = 30
        Me.btnBack.Text = "&Back"
        Me.btnBack.UseVisualStyleBackColor = True
        '
        'btnUpdate
        '
        Me.btnUpdate.Location = New System.Drawing.Point(5, 5)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(100, 30)
        Me.btnUpdate.TabIndex = 29
        Me.btnUpdate.Text = "Update"
        Me.btnUpdate.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Left
        Me.Panel1.Location = New System.Drawing.Point(3, 3)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(5, 451)
        Me.Panel1.TabIndex = 31
        '
        'frmCentRateUpdate
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(881, 483)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.tabMain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmCentRateUpdate"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Cent Rate Update"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.tabMain.ResumeLayout(False)
        Me.tabGeneral.ResumeLayout(False)
        Me.grpControl.ResumeLayout(False)
        Me.grpControl.PerformLayout()
        Me.tabView.ResumeLayout(False)
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tabMain As System.Windows.Forms.TabControl
    Friend WithEvents tabGeneral As System.Windows.Forms.TabPage
    Friend WithEvents tabView As System.Windows.Forms.TabPage
    Friend WithEvents grpControl As System.Windows.Forms.GroupBox
    Friend WithEvents chkWebTag As System.Windows.Forms.CheckBox
    Friend WithEvents chkTag As System.Windows.Forms.CheckBox
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents rbtMinValue As System.Windows.Forms.RadioButton
    Friend WithEvents rbtMaxValue As System.Windows.Forms.RadioButton
    Friend WithEvents btnView As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents chkLstSubItem As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkLstItem As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkSubItemSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkItemSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents btnUpdate As System.Windows.Forms.Button
    Friend WithEvents btnBack As System.Windows.Forms.Button
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtFromCent_WET As System.Windows.Forms.TextBox
    Friend WithEvents txtToCent_WET As System.Windows.Forms.TextBox
    Friend WithEvents btnExport As Button
End Class
