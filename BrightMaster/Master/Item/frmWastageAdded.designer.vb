<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmWastageAdded
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
        Me.txtMc_Amt = New System.Windows.Forms.TextBox
        Me.txtTouch_Amt = New System.Windows.Forms.TextBox
        Me.cmbSubItem = New System.Windows.Forms.ComboBox
        Me.cmbItem = New System.Windows.Forms.ComboBox
        Me.cmbPartyName = New System.Windows.Forms.ComboBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.btnOpen = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.btnSave = New System.Windows.Forms.Button
        Me.btnExit = New System.Windows.Forms.Button
        Me.Label8 = New System.Windows.Forms.Label
        Me.tabMain = New System.Windows.Forms.TabControl
        Me.tabGeneral = New System.Windows.Forms.TabPage
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.rbtSmith = New System.Windows.Forms.RadioButton
        Me.Label17 = New System.Windows.Forms.Label
        Me.rbtCustomer = New System.Windows.Forms.RadioButton
        Me.txtSno = New System.Windows.Forms.TextBox
        Me.tabView = New System.Windows.Forms.TabPage
        Me.gridView = New System.Windows.Forms.DataGridView
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.pnlOption = New System.Windows.Forms.Panel
        Me.rbtOpenCustomer = New System.Windows.Forms.RadioButton
        Me.rbtOpenSmith = New System.Windows.Forms.RadioButton
        Me.btnOpenView = New System.Windows.Forms.Button
        Me.Label13 = New System.Windows.Forms.Label
        Me.cmbOpenParty = New System.Windows.Forms.ComboBox
        Me.cmbOpenItem = New System.Windows.Forms.ComboBox
        Me.cmbOpenSubItem = New System.Windows.Forms.ComboBox
        Me.Label14 = New System.Windows.Forms.Label
        Me.Label15 = New System.Windows.Forms.Label
        Me.ContextMenuStrip1.SuspendLayout()
        Me.tabMain.SuspendLayout()
        Me.tabGeneral.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.tabView.SuspendLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel3.SuspendLayout()
        Me.pnlOption.SuspendLayout()
        Me.SuspendLayout()
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SaveToolStripMenuItem, Me.OpenToolStripMenuItem, Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(131, 92)
        '
        'SaveToolStripMenuItem
        '
        Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        Me.SaveToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(130, 22)
        Me.SaveToolStripMenuItem.Text = "Save"
        Me.SaveToolStripMenuItem.Visible = False
        '
        'OpenToolStripMenuItem
        '
        Me.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem"
        Me.OpenToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2
        Me.OpenToolStripMenuItem.Size = New System.Drawing.Size(130, 22)
        Me.OpenToolStripMenuItem.Text = "Open"
        Me.OpenToolStripMenuItem.Visible = False
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(130, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(130, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'txtMc_Amt
        '
        Me.txtMc_Amt.Location = New System.Drawing.Point(102, 178)
        Me.txtMc_Amt.MaxLength = 8
        Me.txtMc_Amt.Name = "txtMc_Amt"
        Me.txtMc_Amt.Size = New System.Drawing.Size(95, 21)
        Me.txtMc_Amt.TabIndex = 12
        Me.txtMc_Amt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtTouch_Amt
        '
        Me.txtTouch_Amt.Location = New System.Drawing.Point(102, 147)
        Me.txtTouch_Amt.MaxLength = 5
        Me.txtTouch_Amt.Name = "txtTouch_Amt"
        Me.txtTouch_Amt.Size = New System.Drawing.Size(95, 21)
        Me.txtTouch_Amt.TabIndex = 10
        Me.txtTouch_Amt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'cmbSubItem
        '
        Me.cmbSubItem.FormattingEnabled = True
        Me.cmbSubItem.Location = New System.Drawing.Point(101, 113)
        Me.cmbSubItem.Name = "cmbSubItem"
        Me.cmbSubItem.Size = New System.Drawing.Size(228, 21)
        Me.cmbSubItem.TabIndex = 8
        '
        'cmbItem
        '
        Me.cmbItem.FormattingEnabled = True
        Me.cmbItem.Location = New System.Drawing.Point(101, 80)
        Me.cmbItem.Name = "cmbItem"
        Me.cmbItem.Size = New System.Drawing.Size(228, 21)
        Me.cmbItem.TabIndex = 6
        Me.cmbItem.Text = "123456789012345678901234567890"
        '
        'cmbPartyName
        '
        Me.cmbPartyName.FormattingEnabled = True
        Me.cmbPartyName.Location = New System.Drawing.Point(101, 49)
        Me.cmbPartyName.Name = "cmbPartyName"
        Me.cmbPartyName.Size = New System.Drawing.Size(291, 21)
        Me.cmbPartyName.TabIndex = 4
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.Color.Transparent
        Me.Label5.Location = New System.Drawing.Point(17, 150)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(41, 13)
        Me.Label5.TabIndex = 9
        Me.Label5.Text = "Touch"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Location = New System.Drawing.Point(17, 117)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(60, 13)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "Sub Item"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Location = New System.Drawing.Point(17, 84)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(34, 13)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Item"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Location = New System.Drawing.Point(17, 53)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(74, 13)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Party Name"
        '
        'btnOpen
        '
        Me.btnOpen.Location = New System.Drawing.Point(126, 210)
        Me.btnOpen.Name = "btnOpen"
        Me.btnOpen.Size = New System.Drawing.Size(100, 30)
        Me.btnOpen.TabIndex = 14
        Me.btnOpen.Text = "Open [F2]"
        Me.btnOpen.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(232, 210)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 15
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(20, 210)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(100, 30)
        Me.btnSave.TabIndex = 13
        Me.btnSave.Text = "Save [F1]"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(338, 210)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 16
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.BackColor = System.Drawing.Color.Transparent
        Me.Label8.Location = New System.Drawing.Point(17, 181)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(58, 13)
        Me.Label8.TabIndex = 11
        Me.Label8.Text = "MCharge"
        '
        'tabMain
        '
        Me.tabMain.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
        Me.tabMain.Controls.Add(Me.tabGeneral)
        Me.tabMain.Controls.Add(Me.tabView)
        Me.tabMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabMain.Location = New System.Drawing.Point(0, 0)
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(1021, 663)
        Me.tabMain.TabIndex = 0
        '
        'tabGeneral
        '
        Me.tabGeneral.ContextMenuStrip = Me.ContextMenuStrip1
        Me.tabGeneral.Controls.Add(Me.GroupBox1)
        Me.tabGeneral.Controls.Add(Me.txtSno)
        Me.tabGeneral.Location = New System.Drawing.Point(4, 25)
        Me.tabGeneral.Name = "tabGeneral"
        Me.tabGeneral.Padding = New System.Windows.Forms.Padding(3)
        Me.tabGeneral.Size = New System.Drawing.Size(1013, 634)
        Me.tabGeneral.TabIndex = 0
        Me.tabGeneral.Text = "General"
        Me.tabGeneral.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.BackColor = System.Drawing.Color.Transparent
        Me.GroupBox1.Controls.Add(Me.rbtSmith)
        Me.GroupBox1.Controls.Add(Me.Label17)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.rbtCustomer)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.cmbPartyName)
        Me.GroupBox1.Controls.Add(Me.txtMc_Amt)
        Me.GroupBox1.Controls.Add(Me.cmbItem)
        Me.GroupBox1.Controls.Add(Me.btnNew)
        Me.GroupBox1.Controls.Add(Me.btnOpen)
        Me.GroupBox1.Controls.Add(Me.cmbSubItem)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.btnExit)
        Me.GroupBox1.Controls.Add(Me.txtTouch_Amt)
        Me.GroupBox1.Controls.Add(Me.Label8)
        Me.GroupBox1.Controls.Add(Me.btnSave)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Location = New System.Drawing.Point(330, 156)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(447, 254)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        '
        'rbtSmith
        '
        Me.rbtSmith.AutoSize = True
        Me.rbtSmith.Location = New System.Drawing.Point(188, 20)
        Me.rbtSmith.Name = "rbtSmith"
        Me.rbtSmith.Size = New System.Drawing.Size(58, 17)
        Me.rbtSmith.TabIndex = 2
        Me.rbtSmith.TabStop = True
        Me.rbtSmith.Text = "Smith"
        Me.rbtSmith.UseVisualStyleBackColor = True
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.BackColor = System.Drawing.Color.Transparent
        Me.Label17.Location = New System.Drawing.Point(17, 22)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(35, 13)
        Me.Label17.TabIndex = 0
        Me.Label17.Text = "Type"
        '
        'rbtCustomer
        '
        Me.rbtCustomer.AutoSize = True
        Me.rbtCustomer.Location = New System.Drawing.Point(101, 20)
        Me.rbtCustomer.Name = "rbtCustomer"
        Me.rbtCustomer.Size = New System.Drawing.Size(63, 17)
        Me.rbtCustomer.TabIndex = 1
        Me.rbtCustomer.TabStop = True
        Me.rbtCustomer.Text = "Dealer"
        Me.rbtCustomer.UseVisualStyleBackColor = True
        '
        'txtSno
        '
        Me.txtSno.Location = New System.Drawing.Point(139, 264)
        Me.txtSno.Name = "txtSno"
        Me.txtSno.Size = New System.Drawing.Size(83, 21)
        Me.txtSno.TabIndex = 1
        Me.txtSno.Visible = False
        '
        'tabView
        '
        Me.tabView.Controls.Add(Me.gridView)
        Me.tabView.Controls.Add(Me.Panel3)
        Me.tabView.Location = New System.Drawing.Point(4, 25)
        Me.tabView.Name = "tabView"
        Me.tabView.Padding = New System.Windows.Forms.Padding(3)
        Me.tabView.Size = New System.Drawing.Size(1013, 634)
        Me.tabView.TabIndex = 1
        Me.tabView.Text = "View"
        Me.tabView.UseVisualStyleBackColor = True
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Location = New System.Drawing.Point(5, 131)
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.Size = New System.Drawing.Size(1002, 494)
        Me.gridView.TabIndex = 1
        '
        'Panel3
        '
        Me.Panel3.BackColor = System.Drawing.Color.Transparent
        Me.Panel3.Controls.Add(Me.pnlOption)
        Me.Panel3.Controls.Add(Me.btnOpenView)
        Me.Panel3.Controls.Add(Me.Label13)
        Me.Panel3.Controls.Add(Me.cmbOpenParty)
        Me.Panel3.Controls.Add(Me.cmbOpenItem)
        Me.Panel3.Controls.Add(Me.cmbOpenSubItem)
        Me.Panel3.Controls.Add(Me.Label14)
        Me.Panel3.Controls.Add(Me.Label15)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel3.Location = New System.Drawing.Point(3, 3)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(1007, 122)
        Me.Panel3.TabIndex = 0
        '
        'pnlOption
        '
        Me.pnlOption.Controls.Add(Me.rbtOpenCustomer)
        Me.pnlOption.Controls.Add(Me.rbtOpenSmith)
        Me.pnlOption.Location = New System.Drawing.Point(73, 10)
        Me.pnlOption.Name = "pnlOption"
        Me.pnlOption.Size = New System.Drawing.Size(200, 25)
        Me.pnlOption.TabIndex = 0
        '
        'rbtOpenCustomer
        '
        Me.rbtOpenCustomer.AutoSize = True
        Me.rbtOpenCustomer.Location = New System.Drawing.Point(12, 3)
        Me.rbtOpenCustomer.Name = "rbtOpenCustomer"
        Me.rbtOpenCustomer.Size = New System.Drawing.Size(63, 17)
        Me.rbtOpenCustomer.TabIndex = 0
        Me.rbtOpenCustomer.TabStop = True
        Me.rbtOpenCustomer.Text = "Dealer"
        Me.rbtOpenCustomer.UseVisualStyleBackColor = True
        '
        'rbtOpenSmith
        '
        Me.rbtOpenSmith.AutoSize = True
        Me.rbtOpenSmith.Location = New System.Drawing.Point(111, 3)
        Me.rbtOpenSmith.Name = "rbtOpenSmith"
        Me.rbtOpenSmith.Size = New System.Drawing.Size(58, 17)
        Me.rbtOpenSmith.TabIndex = 1
        Me.rbtOpenSmith.TabStop = True
        Me.rbtOpenSmith.Text = "Smith"
        Me.rbtOpenSmith.UseVisualStyleBackColor = True
        '
        'btnOpenView
        '
        Me.btnOpenView.Location = New System.Drawing.Point(319, 87)
        Me.btnOpenView.Name = "btnOpenView"
        Me.btnOpenView.Size = New System.Drawing.Size(75, 25)
        Me.btnOpenView.TabIndex = 7
        Me.btnOpenView.Text = "View"
        Me.btnOpenView.UseVisualStyleBackColor = True
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.BackColor = System.Drawing.Color.Transparent
        Me.Label13.Location = New System.Drawing.Point(5, 45)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(74, 13)
        Me.Label13.TabIndex = 1
        Me.Label13.Text = "Party Name"
        '
        'cmbOpenParty
        '
        Me.cmbOpenParty.FormattingEnabled = True
        Me.cmbOpenParty.Location = New System.Drawing.Point(85, 41)
        Me.cmbOpenParty.Name = "cmbOpenParty"
        Me.cmbOpenParty.Size = New System.Drawing.Size(291, 21)
        Me.cmbOpenParty.TabIndex = 2
        '
        'cmbOpenItem
        '
        Me.cmbOpenItem.FormattingEnabled = True
        Me.cmbOpenItem.Location = New System.Drawing.Point(85, 65)
        Me.cmbOpenItem.Name = "cmbOpenItem"
        Me.cmbOpenItem.Size = New System.Drawing.Size(228, 21)
        Me.cmbOpenItem.TabIndex = 4
        Me.cmbOpenItem.Text = "123456789012345678901234567890"
        '
        'cmbOpenSubItem
        '
        Me.cmbOpenSubItem.FormattingEnabled = True
        Me.cmbOpenSubItem.Location = New System.Drawing.Point(85, 89)
        Me.cmbOpenSubItem.Name = "cmbOpenSubItem"
        Me.cmbOpenSubItem.Size = New System.Drawing.Size(228, 21)
        Me.cmbOpenSubItem.TabIndex = 6
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.BackColor = System.Drawing.Color.Transparent
        Me.Label14.Location = New System.Drawing.Point(5, 69)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(34, 13)
        Me.Label14.TabIndex = 3
        Me.Label14.Text = "Item"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.BackColor = System.Drawing.Color.Transparent
        Me.Label15.Location = New System.Drawing.Point(5, 93)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(60, 13)
        Me.Label15.TabIndex = 5
        Me.Label15.Text = "Sub Item"
        '
        'frmWastageAdded
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1021, 663)
        Me.ControlBox = False
        Me.Controls.Add(Me.tabMain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmWastageAdded"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Dealer Wastage Added"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.tabMain.ResumeLayout(False)
        Me.tabGeneral.ResumeLayout(False)
        Me.tabGeneral.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.tabView.ResumeLayout(False)
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.pnlOption.ResumeLayout(False)
        Me.pnlOption.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents txtMc_Amt As System.Windows.Forms.TextBox
    Friend WithEvents txtTouch_Amt As System.Windows.Forms.TextBox
    Friend WithEvents cmbSubItem As System.Windows.Forms.ComboBox
    Friend WithEvents cmbItem As System.Windows.Forms.ComboBox
    Friend WithEvents cmbPartyName As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnOpen As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents tabMain As System.Windows.Forms.TabControl
    Friend WithEvents tabGeneral As System.Windows.Forms.TabPage
    Friend WithEvents tabView As System.Windows.Forms.TabPage
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents btnOpenView As System.Windows.Forms.Button
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents cmbOpenParty As System.Windows.Forms.ComboBox
    Friend WithEvents cmbOpenItem As System.Windows.Forms.ComboBox
    Friend WithEvents cmbOpenSubItem As System.Windows.Forms.ComboBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents txtSno As System.Windows.Forms.TextBox
    Friend WithEvents rbtSmith As System.Windows.Forms.RadioButton
    Friend WithEvents rbtCustomer As System.Windows.Forms.RadioButton
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents rbtOpenSmith As System.Windows.Forms.RadioButton
    Friend WithEvents rbtOpenCustomer As System.Windows.Forms.RadioButton
    Friend WithEvents pnlOption As System.Windows.Forms.Panel
End Class
