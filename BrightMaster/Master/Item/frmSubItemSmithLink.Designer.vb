<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSubItemSmithLink
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
        Me.tbMain = New System.Windows.Forms.TabControl
        Me.tbGen = New System.Windows.Forms.TabPage
        Me.GRPfIELDS = New System.Windows.Forms.GroupBox
        Me.chkSmithAll = New System.Windows.Forms.CheckBox
        Me.chkSubItemAll = New System.Windows.Forms.CheckBox
        Me.chkItemAll = New System.Windows.Forms.CheckBox
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.chkListItem = New System.Windows.Forms.CheckedListBox
        Me.pnlSubItem = New System.Windows.Forms.Panel
        Me.chkListAccode = New System.Windows.Forms.CheckedListBox
        Me.pnlItem = New System.Windows.Forms.Panel
        Me.chkListSubItem = New System.Windows.Forms.CheckedListBox
        Me.gridShow = New System.Windows.Forms.DataGridView
        Me.bnExit = New System.Windows.Forms.Button
        Me.bnNew = New System.Windows.Forms.Button
        Me.bnOpen = New System.Windows.Forms.Button
        Me.bnSave = New System.Windows.Forms.Button
        Me.tbView = New System.Windows.Forms.TabPage
        Me.Label4 = New System.Windows.Forms.Label
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.cmbSmith_OWN = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.cmbSubItem_OWN = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.cmbItem_OWN = New System.Windows.Forms.ComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.btnSearch_Own = New System.Windows.Forms.Button
        Me.gridView = New System.Windows.Forms.DataGridView
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.tbMain.SuspendLayout()
        Me.tbGen.SuspendLayout()
        Me.GRPfIELDS.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.pnlSubItem.SuspendLayout()
        Me.pnlItem.SuspendLayout()
        CType(Me.gridShow, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tbView.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'tbMain
        '
        Me.tbMain.Controls.Add(Me.tbGen)
        Me.tbMain.Controls.Add(Me.tbView)
        Me.tbMain.Location = New System.Drawing.Point(-1, 17)
        Me.tbMain.Name = "tbMain"
        Me.tbMain.SelectedIndex = 0
        Me.tbMain.Size = New System.Drawing.Size(725, 597)
        Me.tbMain.TabIndex = 0
        '
        'tbGen
        '
        Me.tbGen.Controls.Add(Me.GRPfIELDS)
        Me.tbGen.Location = New System.Drawing.Point(4, 22)
        Me.tbGen.Name = "tbGen"
        Me.tbGen.Padding = New System.Windows.Forms.Padding(3)
        Me.tbGen.Size = New System.Drawing.Size(717, 571)
        Me.tbGen.TabIndex = 0
        Me.tbGen.Text = "Gen"
        Me.tbGen.UseVisualStyleBackColor = True
        '
        'GRPfIELDS
        '
        Me.GRPfIELDS.BackColor = System.Drawing.Color.Transparent
        Me.GRPfIELDS.Controls.Add(Me.chkSmithAll)
        Me.GRPfIELDS.Controls.Add(Me.chkSubItemAll)
        Me.GRPfIELDS.Controls.Add(Me.chkItemAll)
        Me.GRPfIELDS.Controls.Add(Me.Panel1)
        Me.GRPfIELDS.Controls.Add(Me.pnlSubItem)
        Me.GRPfIELDS.Controls.Add(Me.pnlItem)
        Me.GRPfIELDS.Controls.Add(Me.gridShow)
        Me.GRPfIELDS.Controls.Add(Me.bnExit)
        Me.GRPfIELDS.Controls.Add(Me.bnNew)
        Me.GRPfIELDS.Controls.Add(Me.bnOpen)
        Me.GRPfIELDS.Controls.Add(Me.bnSave)
        Me.GRPfIELDS.Location = New System.Drawing.Point(-2, 2)
        Me.GRPfIELDS.Name = "GRPfIELDS"
        Me.GRPfIELDS.Size = New System.Drawing.Size(721, 567)
        Me.GRPfIELDS.TabIndex = 0
        Me.GRPfIELDS.TabStop = False
        '
        'chkSmithAll
        '
        Me.chkSmithAll.AutoSize = True
        Me.chkSmithAll.Location = New System.Drawing.Point(408, 15)
        Me.chkSmithAll.Name = "chkSmithAll"
        Me.chkSmithAll.Size = New System.Drawing.Size(83, 17)
        Me.chkSmithAll.TabIndex = 5
        Me.chkSmithAll.Text = "Smith Name"
        Me.chkSmithAll.UseVisualStyleBackColor = True
        '
        'chkSubItemAll
        '
        Me.chkSubItemAll.AutoSize = True
        Me.chkSubItemAll.Location = New System.Drawing.Point(211, 15)
        Me.chkSubItemAll.Name = "chkSubItemAll"
        Me.chkSubItemAll.Size = New System.Drawing.Size(65, 17)
        Me.chkSubItemAll.TabIndex = 3
        Me.chkSubItemAll.Text = "SubItem"
        Me.chkSubItemAll.UseVisualStyleBackColor = True
        '
        'chkItemAll
        '
        Me.chkItemAll.AutoSize = True
        Me.chkItemAll.Location = New System.Drawing.Point(14, 17)
        Me.chkItemAll.Name = "chkItemAll"
        Me.chkItemAll.Size = New System.Drawing.Size(46, 17)
        Me.chkItemAll.TabIndex = 1
        Me.chkItemAll.Text = "Item"
        Me.chkItemAll.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.chkListItem)
        Me.Panel1.Location = New System.Drawing.Point(8, 38)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(191, 404)
        Me.Panel1.TabIndex = 2
        '
        'chkListItem
        '
        Me.chkListItem.CheckOnClick = True
        Me.chkListItem.FormattingEnabled = True
        Me.chkListItem.HorizontalScrollbar = True
        Me.chkListItem.Location = New System.Drawing.Point(6, 5)
        Me.chkListItem.Name = "chkListItem"
        Me.chkListItem.ScrollAlwaysVisible = True
        Me.chkListItem.Size = New System.Drawing.Size(184, 394)
        Me.chkListItem.TabIndex = 0
        '
        'pnlSubItem
        '
        Me.pnlSubItem.Controls.Add(Me.chkListAccode)
        Me.pnlSubItem.Location = New System.Drawing.Point(402, 39)
        Me.pnlSubItem.Name = "pnlSubItem"
        Me.pnlSubItem.Size = New System.Drawing.Size(312, 403)
        Me.pnlSubItem.TabIndex = 6
        '
        'chkListAccode
        '
        Me.chkListAccode.CheckOnClick = True
        Me.chkListAccode.FormattingEnabled = True
        Me.chkListAccode.HorizontalScrollbar = True
        Me.chkListAccode.Location = New System.Drawing.Point(5, 4)
        Me.chkListAccode.Name = "chkListAccode"
        Me.chkListAccode.ScrollAlwaysVisible = True
        Me.chkListAccode.Size = New System.Drawing.Size(304, 394)
        Me.chkListAccode.TabIndex = 0
        '
        'pnlItem
        '
        Me.pnlItem.Controls.Add(Me.chkListSubItem)
        Me.pnlItem.Location = New System.Drawing.Point(205, 38)
        Me.pnlItem.Name = "pnlItem"
        Me.pnlItem.Size = New System.Drawing.Size(191, 404)
        Me.pnlItem.TabIndex = 4
        '
        'chkListSubItem
        '
        Me.chkListSubItem.CheckOnClick = True
        Me.chkListSubItem.FormattingEnabled = True
        Me.chkListSubItem.HorizontalScrollbar = True
        Me.chkListSubItem.Location = New System.Drawing.Point(0, 4)
        Me.chkListSubItem.Name = "chkListSubItem"
        Me.chkListSubItem.ScrollAlwaysVisible = True
        Me.chkListSubItem.Size = New System.Drawing.Size(185, 394)
        Me.chkListSubItem.TabIndex = 0
        '
        'gridShow
        '
        Me.gridShow.AllowUserToAddRows = False
        Me.gridShow.AllowUserToDeleteRows = False
        Me.gridShow.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.gridShow.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridShow.Location = New System.Drawing.Point(5, 484)
        Me.gridShow.Name = "gridShow"
        Me.gridShow.ReadOnly = True
        Me.gridShow.RowHeadersVisible = False
        Me.gridShow.RowTemplate.Height = 20
        Me.gridShow.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridShow.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridShow.Size = New System.Drawing.Size(352, 75)
        Me.gridShow.TabIndex = 34
        Me.gridShow.Visible = False
        '
        'bnExit
        '
        Me.bnExit.Location = New System.Drawing.Point(397, 525)
        Me.bnExit.Name = "bnExit"
        Me.bnExit.Size = New System.Drawing.Size(100, 35)
        Me.bnExit.TabIndex = 10
        Me.bnExit.Text = "Exit [F12]"
        Me.bnExit.UseVisualStyleBackColor = True
        '
        'bnNew
        '
        Me.bnNew.Location = New System.Drawing.Point(601, 484)
        Me.bnNew.Name = "bnNew"
        Me.bnNew.Size = New System.Drawing.Size(100, 35)
        Me.bnNew.TabIndex = 9
        Me.bnNew.Text = "New [F3]"
        Me.bnNew.UseVisualStyleBackColor = True
        '
        'bnOpen
        '
        Me.bnOpen.Location = New System.Drawing.Point(499, 484)
        Me.bnOpen.Name = "bnOpen"
        Me.bnOpen.Size = New System.Drawing.Size(100, 35)
        Me.bnOpen.TabIndex = 8
        Me.bnOpen.Text = "Open [F2]"
        Me.bnOpen.UseVisualStyleBackColor = True
        '
        'bnSave
        '
        Me.bnSave.Location = New System.Drawing.Point(397, 484)
        Me.bnSave.Name = "bnSave"
        Me.bnSave.Size = New System.Drawing.Size(100, 35)
        Me.bnSave.TabIndex = 7
        Me.bnSave.Text = "Save [F1]"
        Me.bnSave.UseVisualStyleBackColor = True
        '
        'tbView
        '
        Me.tbView.Controls.Add(Me.Label4)
        Me.tbView.Controls.Add(Me.GroupBox1)
        Me.tbView.Controls.Add(Me.gridView)
        Me.tbView.Location = New System.Drawing.Point(4, 22)
        Me.tbView.Name = "tbView"
        Me.tbView.Padding = New System.Windows.Forms.Padding(3)
        Me.tbView.Size = New System.Drawing.Size(717, 571)
        Me.tbView.TabIndex = 1
        Me.tbView.Text = "View"
        Me.tbView.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.Red
        Me.Label4.Location = New System.Drawing.Point(22, 545)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(119, 13)
        Me.Label4.TabIndex = 35
        Me.Label4.Text = "Press F9 for Search"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.cmbSmith_OWN)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.cmbSubItem_OWN)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.cmbItem_OWN)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.btnSearch_Own)
        Me.GroupBox1.Location = New System.Drawing.Point(6, 6)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(707, 69)
        Me.GroupBox1.TabIndex = 34
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Search"
        '
        'cmbSmith_OWN
        '
        Me.cmbSmith_OWN.FormattingEnabled = True
        Me.cmbSmith_OWN.Items.AddRange(New Object() {"ALL"})
        Me.cmbSmith_OWN.Location = New System.Drawing.Point(386, 42)
        Me.cmbSmith_OWN.Name = "cmbSmith_OWN"
        Me.cmbSmith_OWN.Size = New System.Drawing.Size(231, 21)
        Me.cmbSmith_OWN.TabIndex = 14
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(384, 21)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(33, 13)
        Me.Label1.TabIndex = 15
        Me.Label1.Text = "Smith"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbSubItem_OWN
        '
        Me.cmbSubItem_OWN.FormattingEnabled = True
        Me.cmbSubItem_OWN.Location = New System.Drawing.Point(172, 42)
        Me.cmbSubItem_OWN.Name = "cmbSubItem_OWN"
        Me.cmbSubItem_OWN.Size = New System.Drawing.Size(207, 21)
        Me.cmbSubItem_OWN.TabIndex = 13
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(171, 21)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(46, 13)
        Me.Label2.TabIndex = 13
        Me.Label2.Text = "SubItem"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbItem_OWN
        '
        Me.cmbItem_OWN.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbItem_OWN.FormattingEnabled = True
        Me.cmbItem_OWN.Location = New System.Drawing.Point(6, 42)
        Me.cmbItem_OWN.Name = "cmbItem_OWN"
        Me.cmbItem_OWN.Size = New System.Drawing.Size(159, 21)
        Me.cmbItem_OWN.TabIndex = 12
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(7, 21)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(27, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Item"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnSearch_Own
        '
        Me.btnSearch_Own.Location = New System.Drawing.Point(622, 36)
        Me.btnSearch_Own.Name = "btnSearch_Own"
        Me.btnSearch_Own.Size = New System.Drawing.Size(79, 30)
        Me.btnSearch_Own.TabIndex = 15
        Me.btnSearch_Own.Text = "  Search"
        Me.btnSearch_Own.UseVisualStyleBackColor = True
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Location = New System.Drawing.Point(8, 88)
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.RowHeadersVisible = False
        Me.gridView.RowTemplate.Height = 20
        Me.gridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridView.Size = New System.Drawing.Size(705, 441)
        Me.gridView.TabIndex = 33
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
        'frmSubItemSmithLink
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(726, 630)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.tbMain)
        Me.KeyPreview = True
        Me.Name = "frmSubItemSmithLink"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "SubItem Smith Link"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.tbMain.ResumeLayout(False)
        Me.tbGen.ResumeLayout(False)
        Me.GRPfIELDS.ResumeLayout(False)
        Me.GRPfIELDS.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.pnlSubItem.ResumeLayout(False)
        Me.pnlItem.ResumeLayout(False)
        CType(Me.gridShow, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tbView.ResumeLayout(False)
        Me.tbView.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents tbMain As System.Windows.Forms.TabControl
    Friend WithEvents tbGen As System.Windows.Forms.TabPage
    Friend WithEvents GRPfIELDS As System.Windows.Forms.GroupBox
    Friend WithEvents gridShow As System.Windows.Forms.DataGridView
    Friend WithEvents bnExit As System.Windows.Forms.Button
    Friend WithEvents bnNew As System.Windows.Forms.Button
    Friend WithEvents bnOpen As System.Windows.Forms.Button
    Friend WithEvents bnSave As System.Windows.Forms.Button
    Friend WithEvents tbView As System.Windows.Forms.TabPage
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents chkSmithAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkSubItemAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkItemAll As System.Windows.Forms.CheckBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents chkListItem As System.Windows.Forms.CheckedListBox
    Friend WithEvents pnlSubItem As System.Windows.Forms.Panel
    Friend WithEvents chkListAccode As System.Windows.Forms.CheckedListBox
    Friend WithEvents pnlItem As System.Windows.Forms.Panel
    Friend WithEvents chkListSubItem As System.Windows.Forms.CheckedListBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents cmbSmith_OWN As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmbSubItem_OWN As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents cmbItem_OWN As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents btnSearch_Own As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class
