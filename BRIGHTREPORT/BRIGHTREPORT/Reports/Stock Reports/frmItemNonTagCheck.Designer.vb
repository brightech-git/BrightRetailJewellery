<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmItemNonTagCheck
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
        Me.tabmain = New System.Windows.Forms.TabControl
        Me.tabEntry = New System.Windows.Forms.TabPage
        Me.btn_Exit = New System.Windows.Forms.Button
        Me.btn_New = New System.Windows.Forms.Button
        Me.btn_Open = New System.Windows.Forms.Button
        Me.btn_Save = New System.Windows.Forms.Button
        Me.txtNwt = New System.Windows.Forms.TextBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.txtGWt = New System.Windows.Forms.TextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.txtPcs = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.cmbSubItem = New System.Windows.Forms.ComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.cmbItem = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.cmbItemcount = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.tabView = New System.Windows.Forms.TabPage
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.gridview = New System.Windows.Forms.DataGridView
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ViewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.tabmain.SuspendLayout()
        Me.tabEntry.SuspendLayout()
        Me.tabView.SuspendLayout()
        CType(Me.gridview, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'tabmain
        '
        Me.tabmain.Controls.Add(Me.tabEntry)
        Me.tabmain.Controls.Add(Me.tabView)
        Me.tabmain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabmain.Location = New System.Drawing.Point(0, 0)
        Me.tabmain.Name = "tabmain"
        Me.tabmain.SelectedIndex = 0
        Me.tabmain.Size = New System.Drawing.Size(516, 358)
        Me.tabmain.TabIndex = 0
        '
        'tabEntry
        '
        Me.tabEntry.Controls.Add(Me.btn_Exit)
        Me.tabEntry.Controls.Add(Me.btn_New)
        Me.tabEntry.Controls.Add(Me.btn_Open)
        Me.tabEntry.Controls.Add(Me.btn_Save)
        Me.tabEntry.Controls.Add(Me.txtNwt)
        Me.tabEntry.Controls.Add(Me.Label7)
        Me.tabEntry.Controls.Add(Me.txtGWt)
        Me.tabEntry.Controls.Add(Me.Label6)
        Me.tabEntry.Controls.Add(Me.txtPcs)
        Me.tabEntry.Controls.Add(Me.Label4)
        Me.tabEntry.Controls.Add(Me.cmbSubItem)
        Me.tabEntry.Controls.Add(Me.Label3)
        Me.tabEntry.Controls.Add(Me.cmbItem)
        Me.tabEntry.Controls.Add(Me.Label2)
        Me.tabEntry.Controls.Add(Me.cmbItemcount)
        Me.tabEntry.Controls.Add(Me.Label1)
        Me.tabEntry.Location = New System.Drawing.Point(4, 22)
        Me.tabEntry.Name = "tabEntry"
        Me.tabEntry.Padding = New System.Windows.Forms.Padding(3)
        Me.tabEntry.Size = New System.Drawing.Size(508, 332)
        Me.tabEntry.TabIndex = 0
        Me.tabEntry.Text = "Entry"
        Me.tabEntry.UseVisualStyleBackColor = True
        '
        'btn_Exit
        '
        Me.btn_Exit.Location = New System.Drawing.Point(333, 222)
        Me.btn_Exit.Name = "btn_Exit"
        Me.btn_Exit.Size = New System.Drawing.Size(87, 39)
        Me.btn_Exit.TabIndex = 15
        Me.btn_Exit.Text = "Exit [F12]"
        Me.btn_Exit.UseVisualStyleBackColor = True
        '
        'btn_New
        '
        Me.btn_New.Location = New System.Drawing.Point(233, 222)
        Me.btn_New.Name = "btn_New"
        Me.btn_New.Size = New System.Drawing.Size(87, 39)
        Me.btn_New.TabIndex = 14
        Me.btn_New.Text = "New [f3]"
        Me.btn_New.UseVisualStyleBackColor = True
        '
        'btn_Open
        '
        Me.btn_Open.Location = New System.Drawing.Point(133, 222)
        Me.btn_Open.Name = "btn_Open"
        Me.btn_Open.Size = New System.Drawing.Size(87, 39)
        Me.btn_Open.TabIndex = 13
        Me.btn_Open.Text = "View   [F2]"
        Me.btn_Open.UseVisualStyleBackColor = True
        '
        'btn_Save
        '
        Me.btn_Save.Location = New System.Drawing.Point(26, 222)
        Me.btn_Save.Name = "btn_Save"
        Me.btn_Save.Size = New System.Drawing.Size(94, 39)
        Me.btn_Save.TabIndex = 12
        Me.btn_Save.Text = "Save [F1]"
        Me.btn_Save.UseVisualStyleBackColor = True
        '
        'txtNwt
        '
        Me.txtNwt.Location = New System.Drawing.Point(116, 170)
        Me.txtNwt.Name = "txtNwt"
        Me.txtNwt.Size = New System.Drawing.Size(100, 20)
        Me.txtNwt.TabIndex = 11
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(30, 174)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(41, 13)
        Me.Label7.TabIndex = 10
        Me.Label7.Text = "Net Wt"
        '
        'txtGWt
        '
        Me.txtGWt.Location = New System.Drawing.Point(116, 139)
        Me.txtGWt.Name = "txtGWt"
        Me.txtGWt.Size = New System.Drawing.Size(100, 20)
        Me.txtGWt.TabIndex = 9
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(30, 143)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(40, 13)
        Me.Label6.TabIndex = 8
        Me.Label6.Text = "Grs Wt"
        '
        'txtPcs
        '
        Me.txtPcs.Location = New System.Drawing.Point(116, 108)
        Me.txtPcs.Name = "txtPcs"
        Me.txtPcs.Size = New System.Drawing.Size(100, 20)
        Me.txtPcs.TabIndex = 7
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(33, 112)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(39, 13)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Pieces"
        '
        'cmbSubItem
        '
        Me.cmbSubItem.FormattingEnabled = True
        Me.cmbSubItem.Location = New System.Drawing.Point(116, 77)
        Me.cmbSubItem.Name = "cmbSubItem"
        Me.cmbSubItem.Size = New System.Drawing.Size(211, 21)
        Me.cmbSubItem.TabIndex = 5
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(30, 81)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(80, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Sub Item Name"
        '
        'cmbItem
        '
        Me.cmbItem.FormattingEnabled = True
        Me.cmbItem.Location = New System.Drawing.Point(116, 46)
        Me.cmbItem.Name = "cmbItem"
        Me.cmbItem.Size = New System.Drawing.Size(211, 21)
        Me.cmbItem.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(30, 50)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(58, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Item Name"
        '
        'cmbItemcount
        '
        Me.cmbItemcount.FormattingEnabled = True
        Me.cmbItemcount.Location = New System.Drawing.Point(116, 15)
        Me.cmbItemcount.Name = "cmbItemcount"
        Me.cmbItemcount.Size = New System.Drawing.Size(211, 21)
        Me.cmbItemcount.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(30, 19)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(67, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Item Counter"
        '
        'tabView
        '
        Me.tabView.Controls.Add(Me.Label8)
        Me.tabView.Controls.Add(Me.Label5)
        Me.tabView.Controls.Add(Me.gridview)
        Me.tabView.Location = New System.Drawing.Point(4, 22)
        Me.tabView.Name = "tabView"
        Me.tabView.Padding = New System.Windows.Forms.Padding(3)
        Me.tabView.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tabView.Size = New System.Drawing.Size(508, 332)
        Me.tabView.TabIndex = 1
        Me.tabView.Text = "View"
        Me.tabView.UseVisualStyleBackColor = True
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.ForeColor = System.Drawing.Color.Red
        Me.Label8.Location = New System.Drawing.Point(449, 316)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(53, 13)
        Me.Label8.TabIndex = 2
        Me.Label8.Text = "Esc-Back"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.ForeColor = System.Drawing.Color.Red
        Me.Label5.Location = New System.Drawing.Point(9, 317)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(96, 13)
        Me.Label5.TabIndex = 1
        Me.Label5.Text = "Press Enter  to edit"
        '
        'gridview
        '
        Me.gridview.AllowUserToAddRows = False
        Me.gridview.AllowUserToDeleteRows = False
        Me.gridview.AllowUserToResizeRows = False
        Me.gridview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridview.Dock = System.Windows.Forms.DockStyle.Top
        Me.gridview.Location = New System.Drawing.Point(3, 3)
        Me.gridview.Name = "gridview"
        Me.gridview.ReadOnly = True
        Me.gridview.Size = New System.Drawing.Size(502, 307)
        Me.gridview.TabIndex = 0
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SaveToolStripMenuItem, Me.ViewToolStripMenuItem, Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(153, 114)
        '
        'SaveToolStripMenuItem
        '
        Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        Me.SaveToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.SaveToolStripMenuItem.Text = "Save"
        Me.SaveToolStripMenuItem.Visible = False
        '
        'ViewToolStripMenuItem
        '
        Me.ViewToolStripMenuItem.Name = "ViewToolStripMenuItem"
        Me.ViewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2
        Me.ViewToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.ViewToolStripMenuItem.Text = "View"
        Me.ViewToolStripMenuItem.Visible = False
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'frmItemNonTagCheck
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(516, 358)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.tabmain)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmItemNonTagCheck"
        Me.tabmain.ResumeLayout(False)
        Me.tabEntry.ResumeLayout(False)
        Me.tabEntry.PerformLayout()
        Me.tabView.ResumeLayout(False)
        Me.tabView.PerformLayout()
        CType(Me.gridview, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents tabmain As System.Windows.Forms.TabControl
    Friend WithEvents tabEntry As System.Windows.Forms.TabPage
    Friend WithEvents tabView As System.Windows.Forms.TabPage
    Friend WithEvents cmbItemcount As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmbSubItem As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents cmbItem As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtNwt As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtGWt As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtPcs As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents btn_Exit As System.Windows.Forms.Button
    Friend WithEvents btn_New As System.Windows.Forms.Button
    Friend WithEvents btn_Open As System.Windows.Forms.Button
    Friend WithEvents btn_Save As System.Windows.Forms.Button
    Friend WithEvents gridview As System.Windows.Forms.DataGridView
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ViewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class
