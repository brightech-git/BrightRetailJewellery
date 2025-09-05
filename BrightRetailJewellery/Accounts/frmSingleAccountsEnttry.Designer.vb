<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSingleAccountsEnttry
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
        Me.cmbCostCenter_MAN = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.gridView_OWN = New System.Windows.Forms.DataGridView
        Me.txtGrid_OWN = New System.Windows.Forms.TextBox
        Me.lstSearch = New System.Windows.Forms.ListBox
        Me.DataGridView1 = New System.Windows.Forms.DataGridView
        Me.txtNarration1 = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.btnSave = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.btnExit = New System.Windows.Forms.Button
        Me.Grouper1 = New CodeVendor.Controls.Grouper
        Me.lblbud = New System.Windows.Forms.Label
        Me.dtpDate = New BrighttechPack.DatePicker(Me.components)
        Me.lblTitle = New System.Windows.Forms.Label
        Me.grpGridView = New CodeVendor.Controls.Grouper
        Me.DgvSearch = New System.Windows.Forms.DataGridView
        Me.Grouper3 = New CodeVendor.Controls.Grouper
        Me.Dgv1Search = New System.Windows.Forms.DataGridView
        Me.btnView = New System.Windows.Forms.Button
        Me.lblPendAmt = New System.Windows.Forms.Label
        Me.lblAmountNWords = New System.Windows.Forms.Label
        Me.txtNarration2 = New System.Windows.Forms.TextBox
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.RefreshAcNameToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ViewF4ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SaveF1ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.pnlContainer_OWN = New System.Windows.Forms.Panel
        CType(Me.gridView_OWN, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Grouper1.SuspendLayout()
        Me.grpGridView.SuspendLayout()
        CType(Me.DgvSearch, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Grouper3.SuspendLayout()
        CType(Me.Dgv1Search, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.pnlContainer_OWN.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmbCostCenter_MAN
        '
        Me.cmbCostCenter_MAN.FormattingEnabled = True
        Me.cmbCostCenter_MAN.Location = New System.Drawing.Point(280, 83)
        Me.cmbCostCenter_MAN.Name = "cmbCostCenter_MAN"
        Me.cmbCostCenter_MAN.Size = New System.Drawing.Size(215, 22)
        Me.cmbCostCenter_MAN.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(190, 87)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(84, 14)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Cost Centre"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(11, 87)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(38, 14)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Date"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'gridView_OWN
        '
        Me.gridView_OWN.AllowUserToAddRows = False
        Me.gridView_OWN.AllowUserToDeleteRows = False
        Me.gridView_OWN.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView_OWN.Location = New System.Drawing.Point(7, 25)
        Me.gridView_OWN.Name = "gridView_OWN"
        Me.gridView_OWN.ReadOnly = True
        Me.gridView_OWN.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridView_OWN.Size = New System.Drawing.Size(995, 356)
        Me.gridView_OWN.TabIndex = 0
        '
        'txtGrid_OWN
        '
        Me.txtGrid_OWN.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtGrid_OWN.Font = New System.Drawing.Font("Verdana", 12.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtGrid_OWN.Location = New System.Drawing.Point(381, 114)
        Me.txtGrid_OWN.Name = "txtGrid_OWN"
        Me.txtGrid_OWN.Size = New System.Drawing.Size(96, 21)
        Me.txtGrid_OWN.TabIndex = 2
        '
        'lstSearch
        '
        Me.lstSearch.FormattingEnabled = True
        Me.lstSearch.ItemHeight = 14
        Me.lstSearch.Items.AddRange(New Object() {"Dr", "Cr"})
        Me.lstSearch.Location = New System.Drawing.Point(193, 212)
        Me.lstSearch.Name = "lstSearch"
        Me.lstSearch.Size = New System.Drawing.Size(69, 32)
        Me.lstSearch.TabIndex = 1
        Me.lstSearch.Visible = False
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Location = New System.Drawing.Point(14, 378)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.ReadOnly = True
        Me.DataGridView1.Size = New System.Drawing.Size(974, 21)
        Me.DataGridView1.TabIndex = 14
        '
        'txtNarration1
        '
        Me.txtNarration1.Location = New System.Drawing.Point(85, 24)
        Me.txtNarration1.MaxLength = 100
        Me.txtNarration1.Name = "txtNarration1"
        Me.txtNarration1.Size = New System.Drawing.Size(782, 22)
        Me.txtNarration1.TabIndex = 3
        Me.txtNarration1.Text = "01234567890123456789012345678901234567890123456789"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(10, 33)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(70, 14)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Narration"
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(122, 79)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(106, 34)
        Me.btnSave.TabIndex = 6
        Me.btnSave.Text = "Save"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(237, 79)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(106, 34)
        Me.btnNew.TabIndex = 7
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(349, 80)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(106, 34)
        Me.btnExit.TabIndex = 8
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'Grouper1
        '
        Me.Grouper1.BackgroundColor = System.Drawing.Color.Lavender
        Me.Grouper1.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.Grouper1.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.Grouper1.BorderColor = System.Drawing.Color.Transparent
        Me.Grouper1.BorderThickness = 1.0!
        Me.Grouper1.Controls.Add(Me.lblbud)
        Me.Grouper1.Controls.Add(Me.dtpDate)
        Me.Grouper1.Controls.Add(Me.lblTitle)
        Me.Grouper1.Controls.Add(Me.Label1)
        Me.Grouper1.Controls.Add(Me.cmbCostCenter_MAN)
        Me.Grouper1.Controls.Add(Me.Label2)
        Me.Grouper1.CustomGroupBoxColor = System.Drawing.Color.White
        Me.Grouper1.GroupImage = Nothing
        Me.Grouper1.GroupTitle = ""
        Me.Grouper1.Location = New System.Drawing.Point(3, -5)
        Me.Grouper1.Name = "Grouper1"
        Me.Grouper1.Padding = New System.Windows.Forms.Padding(20)
        Me.Grouper1.PaintGroupBox = False
        Me.Grouper1.RoundCorners = 10
        Me.Grouper1.ShadowColor = System.Drawing.Color.DarkGray
        Me.Grouper1.ShadowControl = False
        Me.Grouper1.ShadowThickness = 3
        Me.Grouper1.Size = New System.Drawing.Size(1011, 121)
        Me.Grouper1.TabIndex = 0
        '
        'lblbud
        '
        Me.lblbud.AutoSize = True
        Me.lblbud.ForeColor = System.Drawing.Color.Crimson
        Me.lblbud.Location = New System.Drawing.Point(761, 100)
        Me.lblbud.Name = "lblbud"
        Me.lblbud.Size = New System.Drawing.Size(52, 14)
        Me.lblbud.TabIndex = 5
        Me.lblbud.Text = "Label4"
        Me.lblbud.Visible = False
        '
        'dtpDate
        '
        Me.dtpDate.Location = New System.Drawing.Point(70, 84)
        Me.dtpDate.Mask = "##/##/####"
        Me.dtpDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpDate.Name = "dtpDate"
        Me.dtpDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpDate.Size = New System.Drawing.Size(114, 22)
        Me.dtpDate.TabIndex = 1
        Me.dtpDate.Text = "06/03/9998"
        Me.dtpDate.Value = New Date(9998, 3, 6, 0, 0, 0, 0)
        '
        'lblTitle
        '
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblTitle.Font = New System.Drawing.Font("Verdana", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.lblTitle.Location = New System.Drawing.Point(20, 20)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(971, 45)
        Me.lblTitle.TabIndex = 4
        Me.lblTitle.Text = "JOURNAL ENTRY"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'grpGridView
        '
        Me.grpGridView.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpGridView.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpGridView.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpGridView.BorderColor = System.Drawing.Color.Transparent
        Me.grpGridView.BorderThickness = 1.0!
        Me.grpGridView.Controls.Add(Me.DgvSearch)
        Me.grpGridView.Controls.Add(Me.gridView_OWN)
        Me.grpGridView.Controls.Add(Me.DataGridView1)
        Me.grpGridView.Controls.Add(Me.txtGrid_OWN)
        Me.grpGridView.Controls.Add(Me.lstSearch)
        Me.grpGridView.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpGridView.GroupImage = Nothing
        Me.grpGridView.GroupTitle = ""
        Me.grpGridView.Location = New System.Drawing.Point(3, 112)
        Me.grpGridView.Name = "grpGridView"
        Me.grpGridView.Padding = New System.Windows.Forms.Padding(20)
        Me.grpGridView.PaintGroupBox = False
        Me.grpGridView.RoundCorners = 10
        Me.grpGridView.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpGridView.ShadowControl = False
        Me.grpGridView.ShadowThickness = 3
        Me.grpGridView.Size = New System.Drawing.Size(1011, 381)
        Me.grpGridView.TabIndex = 1
        '
        'DgvSearch
        '
        Me.DgvSearch.AllowUserToAddRows = False
        Me.DgvSearch.AllowUserToDeleteRows = False
        Me.DgvSearch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DgvSearch.Location = New System.Drawing.Point(452, 114)
        Me.DgvSearch.Name = "DgvSearch"
        Me.DgvSearch.ReadOnly = True
        Me.DgvSearch.Size = New System.Drawing.Size(101, 35)
        Me.DgvSearch.TabIndex = 5
        '
        'Grouper3
        '
        Me.Grouper3.BackgroundColor = System.Drawing.Color.Lavender
        Me.Grouper3.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.Grouper3.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.Grouper3.BorderColor = System.Drawing.Color.Transparent
        Me.Grouper3.BorderThickness = 1.0!
        Me.Grouper3.Controls.Add(Me.Dgv1Search)
        Me.Grouper3.Controls.Add(Me.btnView)
        Me.Grouper3.Controls.Add(Me.lblPendAmt)
        Me.Grouper3.Controls.Add(Me.lblAmountNWords)
        Me.Grouper3.Controls.Add(Me.txtNarration2)
        Me.Grouper3.Controls.Add(Me.Label3)
        Me.Grouper3.Controls.Add(Me.txtNarration1)
        Me.Grouper3.Controls.Add(Me.btnExit)
        Me.Grouper3.Controls.Add(Me.btnSave)
        Me.Grouper3.Controls.Add(Me.btnNew)
        Me.Grouper3.CustomGroupBoxColor = System.Drawing.Color.White
        Me.Grouper3.GroupImage = Nothing
        Me.Grouper3.GroupTitle = ""
        Me.Grouper3.Location = New System.Drawing.Point(4, 490)
        Me.Grouper3.Name = "Grouper3"
        Me.Grouper3.Padding = New System.Windows.Forms.Padding(20)
        Me.Grouper3.PaintGroupBox = False
        Me.Grouper3.RoundCorners = 10
        Me.Grouper3.ShadowColor = System.Drawing.Color.DarkGray
        Me.Grouper3.ShadowControl = False
        Me.Grouper3.ShadowThickness = 3
        Me.Grouper3.Size = New System.Drawing.Size(1011, 137)
        Me.Grouper3.TabIndex = 2
        '
        'Dgv1Search
        '
        Me.Dgv1Search.AllowUserToAddRows = False
        Me.Dgv1Search.AllowUserToDeleteRows = False
        Me.Dgv1Search.AllowUserToResizeRows = False
        Me.Dgv1Search.BackgroundColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Dgv1Search.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.Dgv1Search.Location = New System.Drawing.Point(86, 46)
        Me.Dgv1Search.Name = "Dgv1Search"
        Me.Dgv1Search.ReadOnly = True
        Me.Dgv1Search.Size = New System.Drawing.Size(466, 10)
        Me.Dgv1Search.TabIndex = 4
        Me.Dgv1Search.Visible = False
        '
        'btnView
        '
        Me.btnView.Location = New System.Drawing.Point(461, 80)
        Me.btnView.Name = "btnView"
        Me.btnView.Size = New System.Drawing.Size(106, 34)
        Me.btnView.TabIndex = 9
        Me.btnView.Text = "View [F4]"
        Me.btnView.UseVisualStyleBackColor = False
        Me.btnView.Visible = False
        '
        'lblPendAmt
        '
        Me.lblPendAmt.AutoSize = True
        Me.lblPendAmt.ForeColor = System.Drawing.Color.Red
        Me.lblPendAmt.Location = New System.Drawing.Point(874, 30)
        Me.lblPendAmt.Name = "lblPendAmt"
        Me.lblPendAmt.Size = New System.Drawing.Size(52, 14)
        Me.lblPendAmt.TabIndex = 10
        Me.lblPendAmt.Text = "Label4"
        '
        'lblAmountNWords
        '
        Me.lblAmountNWords.AutoSize = True
        Me.lblAmountNWords.ForeColor = System.Drawing.Color.Red
        Me.lblAmountNWords.Location = New System.Drawing.Point(575, 86)
        Me.lblAmountNWords.Name = "lblAmountNWords"
        Me.lblAmountNWords.Size = New System.Drawing.Size(52, 14)
        Me.lblAmountNWords.TabIndex = 10
        Me.lblAmountNWords.Text = "Label4"
        '
        'txtNarration2
        '
        Me.txtNarration2.Location = New System.Drawing.Point(86, 51)
        Me.txtNarration2.MaxLength = 100
        Me.txtNarration2.Name = "txtNarration2"
        Me.txtNarration2.Size = New System.Drawing.Size(782, 22)
        Me.txtNarration2.TabIndex = 5
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem, Me.RefreshAcNameToolStripMenuItem, Me.ViewF4ToolStripMenuItem, Me.SaveF1ToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(202, 114)
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(201, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(201, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'RefreshAcNameToolStripMenuItem
        '
        Me.RefreshAcNameToolStripMenuItem.Name = "RefreshAcNameToolStripMenuItem"
        Me.RefreshAcNameToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5
        Me.RefreshAcNameToolStripMenuItem.Size = New System.Drawing.Size(201, 22)
        Me.RefreshAcNameToolStripMenuItem.Text = "Refresh AcName"
        Me.RefreshAcNameToolStripMenuItem.Visible = False
        '
        'ViewF4ToolStripMenuItem
        '
        Me.ViewF4ToolStripMenuItem.Name = "ViewF4ToolStripMenuItem"
        Me.ViewF4ToolStripMenuItem.Size = New System.Drawing.Size(201, 22)
        Me.ViewF4ToolStripMenuItem.Text = "View                               F4"
        '
        'SaveF1ToolStripMenuItem
        '
        Me.SaveF1ToolStripMenuItem.Name = "SaveF1ToolStripMenuItem"
        Me.SaveF1ToolStripMenuItem.Size = New System.Drawing.Size(201, 22)
        Me.SaveF1ToolStripMenuItem.Text = "Save                              F1"
        '
        'pnlContainer_OWN
        '
        Me.pnlContainer_OWN.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.pnlContainer_OWN.Controls.Add(Me.Grouper1)
        Me.pnlContainer_OWN.Controls.Add(Me.grpGridView)
        Me.pnlContainer_OWN.Controls.Add(Me.Grouper3)
        Me.pnlContainer_OWN.Location = New System.Drawing.Point(0, 0)
        Me.pnlContainer_OWN.Name = "pnlContainer_OWN"
        Me.pnlContainer_OWN.Size = New System.Drawing.Size(1024, 640)
        Me.pnlContainer_OWN.TabIndex = 21
        '
        'frmSingleAccountsEnttry
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.ClientSize = New System.Drawing.Size(1024, 640)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.pnlContainer_OWN)
        Me.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmSingleAccountsEnttry"
        Me.Text = "Accounts Entry"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.gridView_OWN, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Grouper1.ResumeLayout(False)
        Me.Grouper1.PerformLayout()
        Me.grpGridView.ResumeLayout(False)
        Me.grpGridView.PerformLayout()
        CType(Me.DgvSearch, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Grouper3.ResumeLayout(False)
        Me.Grouper3.PerformLayout()
        CType(Me.Dgv1Search, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.pnlContainer_OWN.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents cmbCostCenter_MAN As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents gridView_OWN As System.Windows.Forms.DataGridView
    Friend WithEvents txtGrid_OWN As System.Windows.Forms.TextBox
    Friend WithEvents lstSearch As System.Windows.Forms.ListBox
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents txtNarration1 As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents Grouper1 As CodeVendor.Controls.Grouper
    Friend WithEvents grpGridView As CodeVendor.Controls.Grouper
    Friend WithEvents Grouper3 As CodeVendor.Controls.Grouper
    Friend WithEvents txtNarration2 As System.Windows.Forms.TextBox
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents pnlContainer_OWN As System.Windows.Forms.Panel
    Friend WithEvents dtpDate As BrighttechPack.DatePicker
    Friend WithEvents lblAmountNWords As System.Windows.Forms.Label
    Friend WithEvents RefreshAcNameToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DgvSearch As System.Windows.Forms.DataGridView
    Friend WithEvents lblPendAmt As System.Windows.Forms.Label
    Friend WithEvents btnView As System.Windows.Forms.Button
    Friend WithEvents ViewF4ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SaveF1ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblbud As System.Windows.Forms.Label
    Friend WithEvents Dgv1Search As System.Windows.Forms.DataGridView
End Class
