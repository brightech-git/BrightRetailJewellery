<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmpurchasevatannexture1
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
        Me.grpFiltration = New System.Windows.Forms.GroupBox
        Me.rbtannextureII = New System.Windows.Forms.RadioButton
        Me.rbtannextureI = New System.Windows.Forms.RadioButton
        Me.btnExport = New System.Windows.Forms.Button
        Me.chkcmbCompany = New BrighttechPack.CheckedComboBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.chkCmbCostCentre = New BrighttechPack.CheckedComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.Label2 = New System.Windows.Forms.Label
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.btnView_Search = New System.Windows.Forms.Button
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.gridview = New System.Windows.Forms.DataGridView
        Me.cmbGridShortCut = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ResizeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.grpFiltration.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.gridview, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.cmbGridShortCut.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpFiltration
        '
        Me.grpFiltration.Controls.Add(Me.rbtannextureII)
        Me.grpFiltration.Controls.Add(Me.rbtannextureI)
        Me.grpFiltration.Controls.Add(Me.btnExport)
        Me.grpFiltration.Controls.Add(Me.chkcmbCompany)
        Me.grpFiltration.Controls.Add(Me.Label8)
        Me.grpFiltration.Controls.Add(Me.chkCmbCostCentre)
        Me.grpFiltration.Controls.Add(Me.Label3)
        Me.grpFiltration.Controls.Add(Me.Label1)
        Me.grpFiltration.Controls.Add(Me.dtpTo)
        Me.grpFiltration.Controls.Add(Me.Label2)
        Me.grpFiltration.Controls.Add(Me.dtpFrom)
        Me.grpFiltration.Controls.Add(Me.btnView_Search)
        Me.grpFiltration.Controls.Add(Me.btnExit)
        Me.grpFiltration.Controls.Add(Me.btnNew)
        Me.grpFiltration.Dock = System.Windows.Forms.DockStyle.Top
        Me.grpFiltration.Location = New System.Drawing.Point(0, 0)
        Me.grpFiltration.Name = "grpFiltration"
        Me.grpFiltration.Size = New System.Drawing.Size(1026, 121)
        Me.grpFiltration.TabIndex = 0
        Me.grpFiltration.TabStop = False
        '
        'rbtannextureII
        '
        Me.rbtannextureII.AutoSize = True
        Me.rbtannextureII.Location = New System.Drawing.Point(418, 45)
        Me.rbtannextureII.Name = "rbtannextureII"
        Me.rbtannextureII.Size = New System.Drawing.Size(82, 17)
        Me.rbtannextureII.TabIndex = 23
        Me.rbtannextureII.Text = "Annexture II"
        Me.rbtannextureII.UseVisualStyleBackColor = True
        Me.rbtannextureII.Visible = False
        '
        'rbtannextureI
        '
        Me.rbtannextureI.AutoSize = True
        Me.rbtannextureI.Checked = True
        Me.rbtannextureI.Location = New System.Drawing.Point(340, 45)
        Me.rbtannextureI.Name = "rbtannextureI"
        Me.rbtannextureI.Size = New System.Drawing.Size(79, 17)
        Me.rbtannextureI.TabIndex = 22
        Me.rbtannextureI.TabStop = True
        Me.rbtannextureI.Text = "Annexture I"
        Me.rbtannextureI.UseVisualStyleBackColor = True
        Me.rbtannextureI.Visible = False
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(500, 76)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(90, 30)
        Me.btnExport.TabIndex = 21
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'chkcmbCompany
        '
        Me.chkcmbCompany.CheckOnClick = True
        Me.chkcmbCompany.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkcmbCompany.DropDownHeight = 1
        Me.chkcmbCompany.FormattingEnabled = True
        Me.chkcmbCompany.IntegralHeight = False
        Me.chkcmbCompany.Location = New System.Drawing.Point(87, 52)
        Me.chkcmbCompany.Name = "chkcmbCompany"
        Me.chkcmbCompany.Size = New System.Drawing.Size(227, 21)
        Me.chkcmbCompany.TabIndex = 5
        Me.chkcmbCompany.ValueSeparator = ", "
        '
        'Label8
        '
        Me.Label8.Location = New System.Drawing.Point(15, 50)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(56, 21)
        Me.Label8.TabIndex = 4
        Me.Label8.Text = "Company"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkCmbCostCentre
        '
        Me.chkCmbCostCentre.CheckOnClick = True
        Me.chkCmbCostCentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCostCentre.DropDownHeight = 1
        Me.chkCmbCostCentre.FormattingEnabled = True
        Me.chkCmbCostCentre.IntegralHeight = False
        Me.chkCmbCostCentre.Location = New System.Drawing.Point(88, 85)
        Me.chkCmbCostCentre.Name = "chkCmbCostCentre"
        Me.chkCmbCostCentre.Size = New System.Drawing.Size(226, 21)
        Me.chkCmbCostCentre.TabIndex = 7
        Me.chkCmbCostCentre.ValueSeparator = ", "
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(15, 86)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(67, 21)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Cost Centre"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(15, 24)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(56, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Date From"
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(221, 21)
        Me.dtpTo.Mask = "##/##/####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(93, 20)
        Me.dtpTo.TabIndex = 3
        Me.dtpTo.Text = "07/03/9998"
        Me.dtpTo.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(194, 25)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(20, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "To"
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(87, 21)
        Me.dtpFrom.Mask = "##/##/####"
        Me.dtpFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom.Size = New System.Drawing.Size(93, 20)
        Me.dtpFrom.TabIndex = 1
        Me.dtpFrom.Text = "07/03/9998"
        Me.dtpFrom.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'btnView_Search
        '
        Me.btnView_Search.Location = New System.Drawing.Point(320, 76)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(90, 30)
        Me.btnView_Search.TabIndex = 17
        Me.btnView_Search.Text = "&View"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(590, 76)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(90, 30)
        Me.btnExit.TabIndex = 19
        Me.btnExit.Text = "Exit[F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(410, 76)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(90, 30)
        Me.btnNew.TabIndex = 18
        Me.btnNew.Text = "New[F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.GroupBox1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 121)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1026, 467)
        Me.Panel1.TabIndex = 2
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.gridview)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(1026, 467)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = " "
        '
        'gridview
        '
        Me.gridview.AllowUserToAddRows = False
        Me.gridview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridview.ContextMenuStrip = Me.cmbGridShortCut
        Me.gridview.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridview.Location = New System.Drawing.Point(3, 16)
        Me.gridview.Name = "gridview"
        Me.gridview.RowHeadersVisible = False
        Me.gridview.Size = New System.Drawing.Size(1020, 448)
        Me.gridview.TabIndex = 0
        '
        'cmbGridShortCut
        '
        Me.cmbGridShortCut.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ResizeToolStripMenuItem})
        Me.cmbGridShortCut.Name = "ContextMenuStrip2"
        Me.cmbGridShortCut.Size = New System.Drawing.Size(143, 26)
        '
        'ResizeToolStripMenuItem
        '
        Me.ResizeToolStripMenuItem.CheckOnClick = True
        Me.ResizeToolStripMenuItem.Name = "ResizeToolStripMenuItem"
        Me.ResizeToolStripMenuItem.Size = New System.Drawing.Size(142, 22)
        Me.ResizeToolStripMenuItem.Text = "Auto Resize"
        '
        'frmpurchasevatannexture1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1026, 588)
        Me.ControlBox = False
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.grpFiltration)
        Me.KeyPreview = True
        Me.Name = "frmpurchasevatannexture1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Purchase Vat Annexture I"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.grpFiltration.ResumeLayout(False)
        Me.grpFiltration.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        CType(Me.gridview, System.ComponentModel.ISupportInitialize).EndInit()
        Me.cmbGridShortCut.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpFiltration As System.Windows.Forms.GroupBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents btnView_Search As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents gridview As System.Windows.Forms.DataGridView
    Friend WithEvents chkCmbCostCentre As BrighttechPack.CheckedComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents chkcmbCompany As BrighttechPack.CheckedComboBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents cmbGridShortCut As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ResizeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents rbtannextureII As System.Windows.Forms.RadioButton
    Friend WithEvents rbtannextureI As System.Windows.Forms.RadioButton
End Class
