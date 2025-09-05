<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmGstUpdates
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
        Me.tabMain = New System.Windows.Forms.TabControl
        Me.tabItem = New System.Windows.Forms.TabPage
        Me.gridItemView = New System.Windows.Forms.DataGridView
        Me.tabSubItem = New System.Windows.Forms.TabPage
        Me.gridSubItemView = New System.Windows.Forms.DataGridView
        Me.pnlTop = New System.Windows.Forms.Panel
        Me.btnSearch = New System.Windows.Forms.Button
        Me.cmbOpenItemName = New System.Windows.Forms.ComboBox
        Me.Label18 = New System.Windows.Forms.Label
        Me.tabAchead = New System.Windows.Forms.TabPage
        Me.gridAcheadView = New System.Windows.Forms.DataGridView
        Me.pnlAcTop = New System.Windows.Forms.Panel
        Me.btnOpenSearch = New System.Windows.Forms.Button
        Me.txtSearch = New System.Windows.Forms.TextBox
        Me.Label23 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.cmbOpenGroupName = New System.Windows.Forms.ComboBox
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.lblHelp = New System.Windows.Forms.Label
        Me.btnRefresh = New System.Windows.Forms.Button
        Me.btnExit = New System.Windows.Forms.Button
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.RefreshToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.tabMain.SuspendLayout()
        Me.tabItem.SuspendLayout()
        CType(Me.gridItemView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabSubItem.SuspendLayout()
        CType(Me.gridSubItemView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlTop.SuspendLayout()
        Me.tabAchead.SuspendLayout()
        CType(Me.gridAcheadView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlAcTop.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'tabMain
        '
        Me.tabMain.Controls.Add(Me.tabItem)
        Me.tabMain.Controls.Add(Me.tabSubItem)
        Me.tabMain.Controls.Add(Me.tabAchead)
        Me.tabMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabMain.Location = New System.Drawing.Point(0, 0)
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(442, 538)
        Me.tabMain.TabIndex = 0
        '
        'tabItem
        '
        Me.tabItem.Controls.Add(Me.gridItemView)
        Me.tabItem.Location = New System.Drawing.Point(4, 22)
        Me.tabItem.Name = "tabItem"
        Me.tabItem.Padding = New System.Windows.Forms.Padding(3)
        Me.tabItem.Size = New System.Drawing.Size(434, 512)
        Me.tabItem.TabIndex = 0
        Me.tabItem.Text = "Item Updation"
        Me.tabItem.UseVisualStyleBackColor = True
        '
        'gridItemView
        '
        Me.gridItemView.AllowUserToAddRows = False
        Me.gridItemView.AllowUserToDeleteRows = False
        Me.gridItemView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridItemView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridItemView.Location = New System.Drawing.Point(3, 3)
        Me.gridItemView.Name = "gridItemView"
        Me.gridItemView.RowHeadersVisible = False
        Me.gridItemView.RowTemplate.Height = 20
        Me.gridItemView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridItemView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridItemView.Size = New System.Drawing.Size(428, 506)
        Me.gridItemView.TabIndex = 8
        '
        'tabSubItem
        '
        Me.tabSubItem.Controls.Add(Me.gridSubItemView)
        Me.tabSubItem.Controls.Add(Me.pnlTop)
        Me.tabSubItem.Location = New System.Drawing.Point(4, 22)
        Me.tabSubItem.Name = "tabSubItem"
        Me.tabSubItem.Padding = New System.Windows.Forms.Padding(3)
        Me.tabSubItem.Size = New System.Drawing.Size(434, 512)
        Me.tabSubItem.TabIndex = 1
        Me.tabSubItem.Text = "SubItem Updation"
        Me.tabSubItem.UseVisualStyleBackColor = True
        '
        'gridSubItemView
        '
        Me.gridSubItemView.AllowUserToAddRows = False
        Me.gridSubItemView.AllowUserToDeleteRows = False
        Me.gridSubItemView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridSubItemView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridSubItemView.Location = New System.Drawing.Point(3, 36)
        Me.gridSubItemView.Name = "gridSubItemView"
        Me.gridSubItemView.RowHeadersVisible = False
        Me.gridSubItemView.RowTemplate.Height = 20
        Me.gridSubItemView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridSubItemView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridSubItemView.Size = New System.Drawing.Size(428, 473)
        Me.gridSubItemView.TabIndex = 7
        '
        'pnlTop
        '
        Me.pnlTop.Controls.Add(Me.btnSearch)
        Me.pnlTop.Controls.Add(Me.cmbOpenItemName)
        Me.pnlTop.Controls.Add(Me.Label18)
        Me.pnlTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlTop.Location = New System.Drawing.Point(3, 3)
        Me.pnlTop.Name = "pnlTop"
        Me.pnlTop.Size = New System.Drawing.Size(428, 33)
        Me.pnlTop.TabIndex = 0
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(270, 1)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 2
        Me.btnSearch.Text = "&Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'cmbOpenItemName
        '
        Me.cmbOpenItemName.FormattingEnabled = True
        Me.cmbOpenItemName.Location = New System.Drawing.Point(80, 6)
        Me.cmbOpenItemName.Name = "cmbOpenItemName"
        Me.cmbOpenItemName.Size = New System.Drawing.Size(184, 21)
        Me.cmbOpenItemName.TabIndex = 1
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(3, 10)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(71, 13)
        Me.Label18.TabIndex = 0
        Me.Label18.Text = "I&tem Name"
        Me.Label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tabAchead
        '
        Me.tabAchead.Controls.Add(Me.gridAcheadView)
        Me.tabAchead.Controls.Add(Me.pnlAcTop)
        Me.tabAchead.Location = New System.Drawing.Point(4, 22)
        Me.tabAchead.Name = "tabAchead"
        Me.tabAchead.Size = New System.Drawing.Size(434, 512)
        Me.tabAchead.TabIndex = 2
        Me.tabAchead.Text = "Account Updation"
        Me.tabAchead.UseVisualStyleBackColor = True
        '
        'gridAcheadView
        '
        Me.gridAcheadView.AllowUserToAddRows = False
        Me.gridAcheadView.AllowUserToDeleteRows = False
        Me.gridAcheadView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridAcheadView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridAcheadView.Location = New System.Drawing.Point(0, 58)
        Me.gridAcheadView.Name = "gridAcheadView"
        Me.gridAcheadView.RowHeadersVisible = False
        Me.gridAcheadView.RowTemplate.Height = 20
        Me.gridAcheadView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridAcheadView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridAcheadView.Size = New System.Drawing.Size(434, 454)
        Me.gridAcheadView.TabIndex = 8
        '
        'pnlAcTop
        '
        Me.pnlAcTop.Controls.Add(Me.btnOpenSearch)
        Me.pnlAcTop.Controls.Add(Me.txtSearch)
        Me.pnlAcTop.Controls.Add(Me.Label23)
        Me.pnlAcTop.Controls.Add(Me.Label8)
        Me.pnlAcTop.Controls.Add(Me.cmbOpenGroupName)
        Me.pnlAcTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlAcTop.Location = New System.Drawing.Point(0, 0)
        Me.pnlAcTop.Name = "pnlAcTop"
        Me.pnlAcTop.Size = New System.Drawing.Size(434, 58)
        Me.pnlAcTop.TabIndex = 0
        '
        'btnOpenSearch
        '
        Me.btnOpenSearch.Location = New System.Drawing.Point(313, 21)
        Me.btnOpenSearch.Name = "btnOpenSearch"
        Me.btnOpenSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnOpenSearch.TabIndex = 4
        Me.btnOpenSearch.Text = "Search"
        Me.btnOpenSearch.UseVisualStyleBackColor = True
        '
        'txtSearch
        '
        Me.txtSearch.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtSearch.Location = New System.Drawing.Point(93, 30)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(214, 21)
        Me.txtSearch.TabIndex = 3
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.BackColor = System.Drawing.Color.Transparent
        Me.Label23.Location = New System.Drawing.Point(8, 33)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(47, 13)
        Me.Label23.TabIndex = 2
        Me.Label23.Text = "&Search"
        Me.Label23.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.BackColor = System.Drawing.Color.Transparent
        Me.Label8.Location = New System.Drawing.Point(8, 6)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(79, 13)
        Me.Label8.TabIndex = 0
        Me.Label8.Text = "&Group Name"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbOpenGroupName
        '
        Me.cmbOpenGroupName.FormattingEnabled = True
        Me.cmbOpenGroupName.Location = New System.Drawing.Point(93, 3)
        Me.cmbOpenGroupName.Name = "cmbOpenGroupName"
        Me.cmbOpenGroupName.Size = New System.Drawing.Size(214, 21)
        Me.cmbOpenGroupName.TabIndex = 1
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.lblHelp)
        Me.Panel1.Controls.Add(Me.btnRefresh)
        Me.Panel1.Controls.Add(Me.btnExit)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 538)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(442, 38)
        Me.Panel1.TabIndex = 1
        '
        'lblHelp
        '
        Me.lblHelp.AutoSize = True
        Me.lblHelp.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHelp.ForeColor = System.Drawing.Color.Red
        Me.lblHelp.Location = New System.Drawing.Point(6, 12)
        Me.lblHelp.Name = "lblHelp"
        Me.lblHelp.Size = New System.Drawing.Size(0, 13)
        Me.lblHelp.TabIndex = 9
        '
        'btnRefresh
        '
        Me.btnRefresh.Location = New System.Drawing.Point(238, 3)
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(100, 30)
        Me.btnRefresh.TabIndex = 8
        Me.btnRefresh.Text = "Refresh [F5]"
        Me.btnRefresh.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(338, 3)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 7
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExitToolStripMenuItem, Me.RefreshToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(133, 48)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(132, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'RefreshToolStripMenuItem
        '
        Me.RefreshToolStripMenuItem.Name = "RefreshToolStripMenuItem"
        Me.RefreshToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5
        Me.RefreshToolStripMenuItem.Size = New System.Drawing.Size(132, 22)
        Me.RefreshToolStripMenuItem.Text = "Refresh"
        Me.RefreshToolStripMenuItem.Visible = False
        '
        'frmGstUpdates
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(442, 576)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.tabMain)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmGstUpdates"
        Me.Text = "GST Master Updates"
        Me.tabMain.ResumeLayout(False)
        Me.tabItem.ResumeLayout(False)
        CType(Me.gridItemView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabSubItem.ResumeLayout(False)
        CType(Me.gridSubItemView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlTop.ResumeLayout(False)
        Me.pnlTop.PerformLayout()
        Me.tabAchead.ResumeLayout(False)
        CType(Me.gridAcheadView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlAcTop.ResumeLayout(False)
        Me.pnlAcTop.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents tabMain As System.Windows.Forms.TabControl
    Friend WithEvents tabItem As System.Windows.Forms.TabPage
    Friend WithEvents tabSubItem As System.Windows.Forms.TabPage
    Friend WithEvents tabAchead As System.Windows.Forms.TabPage
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gridSubItemView As System.Windows.Forms.DataGridView
    Friend WithEvents gridItemView As System.Windows.Forms.DataGridView
    Friend WithEvents gridAcheadView As System.Windows.Forms.DataGridView
    Friend WithEvents btnRefresh As System.Windows.Forms.Button
    Friend WithEvents pnlTop As System.Windows.Forms.Panel
    Friend WithEvents pnlAcTop As System.Windows.Forms.Panel
    Friend WithEvents cmbOpenItemName As System.Windows.Forms.ComboBox
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents cmbOpenGroupName As System.Windows.Forms.ComboBox
    Friend WithEvents txtSearch As System.Windows.Forms.TextBox
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents btnOpenSearch As System.Windows.Forms.Button
    Friend WithEvents lblHelp As System.Windows.Forms.Label
    Friend WithEvents RefreshToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class
