<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class InterestAnalysys
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
        Me.chkTDS = New System.Windows.Forms.CheckBox
        Me.chkDefInt = New System.Windows.Forms.CheckBox
        Me.chkInclDate = New System.Windows.Forms.CheckBox
        Me.btnPrint = New System.Windows.Forms.Button
        Me.btnExport = New System.Windows.Forms.Button
        Me.btn_Post = New System.Windows.Forms.Button
        Me.grpOutput = New System.Windows.Forms.GroupBox
        Me.rbtDetailWise = New System.Windows.Forms.RadioButton
        Me.rbtSummaryWise = New System.Windows.Forms.RadioButton
        Me.cmbAcname = New System.Windows.Forms.ComboBox
        Me.chkcmbCompany = New BrighttechPack.CheckedComboBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.txtDrInterestPer = New System.Windows.Forms.TextBox
        Me.txtCrInterestPer = New System.Windows.Forms.TextBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.cmbAcGroup_Man = New System.Windows.Forms.ComboBox
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
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.grpFiltration.SuspendLayout()
        Me.grpOutput.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.gridview, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.cmbGridShortCut.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpFiltration
        '
        Me.grpFiltration.Controls.Add(Me.chkTDS)
        Me.grpFiltration.Controls.Add(Me.chkDefInt)
        Me.grpFiltration.Controls.Add(Me.chkInclDate)
        Me.grpFiltration.Controls.Add(Me.btnPrint)
        Me.grpFiltration.Controls.Add(Me.btnExport)
        Me.grpFiltration.Controls.Add(Me.btn_Post)
        Me.grpFiltration.Controls.Add(Me.grpOutput)
        Me.grpFiltration.Controls.Add(Me.cmbAcname)
        Me.grpFiltration.Controls.Add(Me.chkcmbCompany)
        Me.grpFiltration.Controls.Add(Me.Label8)
        Me.grpFiltration.Controls.Add(Me.txtDrInterestPer)
        Me.grpFiltration.Controls.Add(Me.txtCrInterestPer)
        Me.grpFiltration.Controls.Add(Me.Label7)
        Me.grpFiltration.Controls.Add(Me.Label6)
        Me.grpFiltration.Controls.Add(Me.Label5)
        Me.grpFiltration.Controls.Add(Me.Label4)
        Me.grpFiltration.Controls.Add(Me.cmbAcGroup_Man)
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
        Me.grpFiltration.Size = New System.Drawing.Size(1026, 157)
        Me.grpFiltration.TabIndex = 0
        Me.grpFiltration.TabStop = False
        '
        'chkTDS
        '
        Me.chkTDS.AutoSize = True
        Me.chkTDS.Location = New System.Drawing.Point(772, 56)
        Me.chkTDS.Name = "chkTDS"
        Me.chkTDS.Size = New System.Drawing.Size(48, 17)
        Me.chkTDS.TabIndex = 13
        Me.chkTDS.Text = "TDS"
        Me.chkTDS.UseVisualStyleBackColor = True
        '
        'chkDefInt
        '
        Me.chkDefInt.AutoSize = True
        Me.chkDefInt.Location = New System.Drawing.Point(351, 83)
        Me.chkDefInt.Name = "chkDefInt"
        Me.chkDefInt.Size = New System.Drawing.Size(81, 17)
        Me.chkDefInt.TabIndex = 14
        Me.chkDefInt.Text = "Def Interest"
        Me.chkDefInt.UseVisualStyleBackColor = True
        '
        'chkInclDate
        '
        Me.chkInclDate.AutoSize = True
        Me.chkInclDate.Location = New System.Drawing.Point(772, 87)
        Me.chkInclDate.Name = "chkInclDate"
        Me.chkInclDate.Size = New System.Drawing.Size(83, 17)
        Me.chkInclDate.TabIndex = 19
        Me.chkInclDate.Text = "Incl To date"
        Me.chkInclDate.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(763, 116)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(90, 30)
        Me.btnPrint.TabIndex = 25
        Me.btnPrint.Text = "&Print [P]"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(671, 116)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(90, 30)
        Me.btnExport.TabIndex = 24
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'btn_Post
        '
        Me.btn_Post.Enabled = False
        Me.btn_Post.Location = New System.Drawing.Point(581, 116)
        Me.btn_Post.Name = "btn_Post"
        Me.btn_Post.Size = New System.Drawing.Size(90, 30)
        Me.btn_Post.TabIndex = 23
        Me.btn_Post.Text = "Post"
        Me.btn_Post.UseVisualStyleBackColor = True
        '
        'grpOutput
        '
        Me.grpOutput.Controls.Add(Me.rbtDetailWise)
        Me.grpOutput.Controls.Add(Me.rbtSummaryWise)
        Me.grpOutput.Location = New System.Drawing.Point(87, 111)
        Me.grpOutput.Name = "grpOutput"
        Me.grpOutput.Size = New System.Drawing.Size(213, 34)
        Me.grpOutput.TabIndex = 8
        Me.grpOutput.TabStop = False
        '
        'rbtDetailWise
        '
        Me.rbtDetailWise.AutoSize = True
        Me.rbtDetailWise.Checked = True
        Me.rbtDetailWise.Location = New System.Drawing.Point(6, 14)
        Me.rbtDetailWise.Name = "rbtDetailWise"
        Me.rbtDetailWise.Size = New System.Drawing.Size(64, 17)
        Me.rbtDetailWise.TabIndex = 0
        Me.rbtDetailWise.TabStop = True
        Me.rbtDetailWise.Text = "Detailed"
        Me.rbtDetailWise.UseVisualStyleBackColor = True
        '
        'rbtSummaryWise
        '
        Me.rbtSummaryWise.AutoSize = True
        Me.rbtSummaryWise.Location = New System.Drawing.Point(110, 14)
        Me.rbtSummaryWise.Name = "rbtSummaryWise"
        Me.rbtSummaryWise.Size = New System.Drawing.Size(68, 17)
        Me.rbtSummaryWise.TabIndex = 1
        Me.rbtSummaryWise.Text = "Summary"
        Me.rbtSummaryWise.UseVisualStyleBackColor = True
        '
        'cmbAcname
        '
        Me.cmbAcname.FormattingEnabled = True
        Me.cmbAcname.Location = New System.Drawing.Point(435, 52)
        Me.cmbAcname.Name = "cmbAcname"
        Me.cmbAcname.Size = New System.Drawing.Size(324, 21)
        Me.cmbAcname.TabIndex = 12
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
        'txtDrInterestPer
        '
        Me.txtDrInterestPer.Location = New System.Drawing.Point(528, 84)
        Me.txtDrInterestPer.Name = "txtDrInterestPer"
        Me.txtDrInterestPer.Size = New System.Drawing.Size(61, 20)
        Me.txtDrInterestPer.TabIndex = 16
        '
        'txtCrInterestPer
        '
        Me.txtCrInterestPer.Location = New System.Drawing.Point(696, 84)
        Me.txtCrInterestPer.Name = "txtCrInterestPer"
        Me.txtCrInterestPer.Size = New System.Drawing.Size(61, 20)
        Me.txtCrInterestPer.TabIndex = 18
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(435, 85)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(81, 13)
        Me.Label7.TabIndex = 15
        Me.Label7.Text = "Debit Interest %"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(601, 87)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(83, 13)
        Me.Label6.TabIndex = 17
        Me.Label6.Text = "Credit Interest %"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(348, 54)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(51, 13)
        Me.Label5.TabIndex = 11
        Me.Label5.Text = "Ac Name"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Location = New System.Drawing.Point(348, 23)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(52, 13)
        Me.Label4.TabIndex = 9
        Me.Label4.Text = "Ac Group"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbAcGroup_Man
        '
        Me.cmbAcGroup_Man.FormattingEnabled = True
        Me.cmbAcGroup_Man.Location = New System.Drawing.Point(435, 20)
        Me.cmbAcGroup_Man.Name = "cmbAcGroup_Man"
        Me.cmbAcGroup_Man.Size = New System.Drawing.Size(324, 21)
        Me.cmbAcGroup_Man.TabIndex = 10
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
        Me.btnView_Search.Location = New System.Drawing.Point(305, 116)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(90, 30)
        Me.btnView_Search.TabIndex = 20
        Me.btnView_Search.Text = "&View"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(489, 116)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(90, 30)
        Me.btnExit.TabIndex = 22
        Me.btnExit.Text = "Exit[F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(397, 116)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(90, 30)
        Me.btnNew.TabIndex = 21
        Me.btnNew.Text = "New[F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.GroupBox1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 157)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1026, 431)
        Me.Panel1.TabIndex = 2
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.gridview)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(1026, 431)
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
        Me.gridview.Size = New System.Drawing.Size(1020, 412)
        Me.gridview.TabIndex = 0
        '
        'cmbGridShortCut
        '
        Me.cmbGridShortCut.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ResizeToolStripMenuItem})
        Me.cmbGridShortCut.Name = "ContextMenuStrip2"
        Me.cmbGridShortCut.Size = New System.Drawing.Size(136, 26)
        '
        'ResizeToolStripMenuItem
        '
        Me.ResizeToolStripMenuItem.CheckOnClick = True
        Me.ResizeToolStripMenuItem.Name = "ResizeToolStripMenuItem"
        Me.ResizeToolStripMenuItem.Size = New System.Drawing.Size(135, 22)
        Me.ResizeToolStripMenuItem.Text = "Auto Resize"
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(92, 48)
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.ShowShortcutKeys = False
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(91, 22)
        Me.NewToolStripMenuItem.Text = "New"
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.ShowShortcutKeys = False
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(91, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        '
        'InterestAnalysys
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1026, 588)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.grpFiltration)
        Me.KeyPreview = True
        Me.Name = "InterestAnalysys"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Interest Analysis"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.grpFiltration.ResumeLayout(False)
        Me.grpFiltration.PerformLayout()
        Me.grpOutput.ResumeLayout(False)
        Me.grpOutput.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        CType(Me.gridview, System.ComponentModel.ISupportInitialize).EndInit()
        Me.cmbGridShortCut.ResumeLayout(False)
        Me.ContextMenuStrip1.ResumeLayout(False)
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
    Friend WithEvents cmbAcGroup_Man As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtDrInterestPer As System.Windows.Forms.TextBox
    Friend WithEvents txtCrInterestPer As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents chkcmbCompany As BrighttechPack.CheckedComboBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents cmbAcname As System.Windows.Forms.ComboBox
    Friend WithEvents grpOutput As System.Windows.Forms.GroupBox
    Friend WithEvents rbtDetailWise As System.Windows.Forms.RadioButton
    Friend WithEvents rbtSummaryWise As System.Windows.Forms.RadioButton
    Friend WithEvents btn_Post As System.Windows.Forms.Button
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents cmbGridShortCut As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ResizeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents chkInclDate As System.Windows.Forms.CheckBox
    Friend WithEvents chkDefInt As System.Windows.Forms.CheckBox
    Friend WithEvents chkTDS As System.Windows.Forms.CheckBox
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class
