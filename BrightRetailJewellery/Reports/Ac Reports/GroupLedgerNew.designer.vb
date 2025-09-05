<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class GroupLedgerNew
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
        Me.chkMultiSelect = New System.Windows.Forms.CheckBox
        Me.cmbAcName = New System.Windows.Forms.ComboBox
        Me.DgvSearch = New System.Windows.Forms.DataGridView
        Me.Label4 = New System.Windows.Forms.Label
        Me.chklstAcname = New BrighttechPack.CheckedComboBox
        Me.txtAcname = New System.Windows.Forms.TextBox
        Me.ChkSepCol = New System.Windows.Forms.CheckBox
        Me.ChkNilBalance = New System.Windows.Forms.CheckBox
        Me.chkacgrpSelectAll = New System.Windows.Forms.CheckBox
        Me.chkCmbCostCentre = New BrighttechPack.CheckedComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.GrpConf = New System.Windows.Forms.GroupBox
        Me.chksmithbal = New System.Windows.Forms.CheckBox
        Me.chkconfletter = New System.Windows.Forms.CheckBox
        Me.chkgpletterwithled = New System.Windows.Forms.CheckBox
        Me.chklstgroup = New System.Windows.Forms.CheckedListBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.rbtSummary = New System.Windows.Forms.RadioButton
        Me.rbtDetailed = New System.Windows.Forms.RadioButton
        Me.Label1 = New System.Windows.Forms.Label
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnSearch = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.GrpContainer.SuspendLayout()
        CType(Me.DgvSearch, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GrpConf.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GrpContainer
        '
        Me.GrpContainer.Controls.Add(Me.chkMultiSelect)
        Me.GrpContainer.Controls.Add(Me.cmbAcName)
        Me.GrpContainer.Controls.Add(Me.DgvSearch)
        Me.GrpContainer.Controls.Add(Me.Label4)
        Me.GrpContainer.Controls.Add(Me.chklstAcname)
        Me.GrpContainer.Controls.Add(Me.txtAcname)
        Me.GrpContainer.Controls.Add(Me.ChkSepCol)
        Me.GrpContainer.Controls.Add(Me.ChkNilBalance)
        Me.GrpContainer.Controls.Add(Me.chkacgrpSelectAll)
        Me.GrpContainer.Controls.Add(Me.chkCmbCostCentre)
        Me.GrpContainer.Controls.Add(Me.Label3)
        Me.GrpContainer.Controls.Add(Me.GrpConf)
        Me.GrpContainer.Controls.Add(Me.chklstgroup)
        Me.GrpContainer.Controls.Add(Me.Label2)
        Me.GrpContainer.Controls.Add(Me.rbtSummary)
        Me.GrpContainer.Controls.Add(Me.rbtDetailed)
        Me.GrpContainer.Controls.Add(Me.Label1)
        Me.GrpContainer.Controls.Add(Me.dtpTo)
        Me.GrpContainer.Controls.Add(Me.dtpFrom)
        Me.GrpContainer.Controls.Add(Me.btnExit)
        Me.GrpContainer.Controls.Add(Me.btnSearch)
        Me.GrpContainer.Controls.Add(Me.btnNew)
        Me.GrpContainer.Location = New System.Drawing.Point(249, 16)
        Me.GrpContainer.Name = "GrpContainer"
        Me.GrpContainer.Size = New System.Drawing.Size(355, 466)
        Me.GrpContainer.TabIndex = 0
        Me.GrpContainer.TabStop = False
        '
        'chkMultiSelect
        '
        Me.chkMultiSelect.AutoSize = True
        Me.chkMultiSelect.Location = New System.Drawing.Point(24, 248)
        Me.chkMultiSelect.Name = "chkMultiSelect"
        Me.chkMultiSelect.Size = New System.Drawing.Size(91, 17)
        Me.chkMultiSelect.TabIndex = 9
        Me.chkMultiSelect.Text = "Multi Select"
        Me.chkMultiSelect.UseVisualStyleBackColor = True
        '
        'cmbAcName
        '
        Me.cmbAcName.FormattingEnabled = True
        Me.cmbAcName.Location = New System.Drawing.Point(25, 271)
        Me.cmbAcName.Name = "cmbAcName"
        Me.cmbAcName.Size = New System.Drawing.Size(300, 21)
        Me.cmbAcName.TabIndex = 11
        '
        'DgvSearch
        '
        Me.DgvSearch.AllowUserToAddRows = False
        Me.DgvSearch.AllowUserToDeleteRows = False
        Me.DgvSearch.AllowUserToResizeRows = False
        Me.DgvSearch.BackgroundColor = System.Drawing.SystemColors.ControlDarkDark
        Me.DgvSearch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DgvSearch.Location = New System.Drawing.Point(22, 445)
        Me.DgvSearch.Name = "DgvSearch"
        Me.DgvSearch.ReadOnly = True
        Me.DgvSearch.Size = New System.Drawing.Size(300, 10)
        Me.DgvSearch.TabIndex = 21
        Me.DgvSearch.Visible = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(24, 225)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(66, 13)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "A/C Name"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chklstAcname
        '
        Me.chklstAcname.CheckOnClick = True
        Me.chklstAcname.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chklstAcname.DropDownHeight = 1
        Me.chklstAcname.FormattingEnabled = True
        Me.chklstAcname.IntegralHeight = False
        Me.chklstAcname.Location = New System.Drawing.Point(23, 270)
        Me.chklstAcname.Name = "chklstAcname"
        Me.chklstAcname.Size = New System.Drawing.Size(300, 22)
        Me.chklstAcname.TabIndex = 12
        Me.chklstAcname.ValueSeparator = ", "
        '
        'txtAcname
        '
        Me.txtAcname.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.txtAcname.Location = New System.Drawing.Point(22, 272)
        Me.txtAcname.Name = "txtAcname"
        Me.txtAcname.Size = New System.Drawing.Size(300, 21)
        Me.txtAcname.TabIndex = 10
        Me.txtAcname.Visible = False
        '
        'ChkSepCol
        '
        Me.ChkSepCol.AutoSize = True
        Me.ChkSepCol.Location = New System.Drawing.Point(151, 319)
        Me.ChkSepCol.Name = "ChkSepCol"
        Me.ChkSepCol.Size = New System.Drawing.Size(183, 17)
        Me.ChkSepCol.TabIndex = 16
        Me.ChkSepCol.Text = "Seperate Column for Cr/Dr"
        Me.ChkSepCol.UseVisualStyleBackColor = True
        '
        'ChkNilBalance
        '
        Me.ChkNilBalance.AutoSize = True
        Me.ChkNilBalance.Location = New System.Drawing.Point(31, 321)
        Me.ChkNilBalance.Name = "ChkNilBalance"
        Me.ChkNilBalance.Size = New System.Drawing.Size(117, 17)
        Me.ChkNilBalance.TabIndex = 15
        Me.ChkNilBalance.Text = "With Nil balance"
        Me.ChkNilBalance.UseVisualStyleBackColor = True
        '
        'chkacgrpSelectAll
        '
        Me.chkacgrpSelectAll.AutoSize = True
        Me.chkacgrpSelectAll.Location = New System.Drawing.Point(24, 86)
        Me.chkacgrpSelectAll.Name = "chkacgrpSelectAll"
        Me.chkacgrpSelectAll.Size = New System.Drawing.Size(79, 17)
        Me.chkacgrpSelectAll.TabIndex = 6
        Me.chkacgrpSelectAll.Text = "Ac Group"
        Me.chkacgrpSelectAll.UseVisualStyleBackColor = True
        '
        'chkCmbCostCentre
        '
        Me.chkCmbCostCentre.CheckOnClick = True
        Me.chkCmbCostCentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCostCentre.DropDownHeight = 1
        Me.chkCmbCostCentre.FormattingEnabled = True
        Me.chkCmbCostCentre.IntegralHeight = False
        Me.chkCmbCostCentre.Location = New System.Drawing.Point(23, 57)
        Me.chkCmbCostCentre.Name = "chkCmbCostCentre"
        Me.chkCmbCostCentre.Size = New System.Drawing.Size(300, 22)
        Me.chkCmbCostCentre.TabIndex = 5
        Me.chkCmbCostCentre.ValueSeparator = ", "
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(22, 42)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(76, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Cost Centre"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'GrpConf
        '
        Me.GrpConf.Controls.Add(Me.chksmithbal)
        Me.GrpConf.Controls.Add(Me.chkconfletter)
        Me.GrpConf.Controls.Add(Me.chkgpletterwithled)
        Me.GrpConf.Location = New System.Drawing.Point(24, 342)
        Me.GrpConf.Name = "GrpConf"
        Me.GrpConf.Size = New System.Drawing.Size(298, 61)
        Me.GrpConf.TabIndex = 17
        Me.GrpConf.TabStop = False
        '
        'chksmithbal
        '
        Me.chksmithbal.AutoSize = True
        Me.chksmithbal.Location = New System.Drawing.Point(6, 33)
        Me.chksmithbal.Name = "chksmithbal"
        Me.chksmithbal.Size = New System.Drawing.Size(137, 17)
        Me.chksmithbal.TabIndex = 2
        Me.chksmithbal.Text = "With Smith Balance"
        Me.chksmithbal.UseVisualStyleBackColor = True
        '
        'chkconfletter
        '
        Me.chkconfletter.AutoSize = True
        Me.chkconfletter.Location = New System.Drawing.Point(6, 12)
        Me.chkconfletter.Name = "chkconfletter"
        Me.chkconfletter.Size = New System.Drawing.Size(138, 17)
        Me.chkconfletter.TabIndex = 0
        Me.chkconfletter.Text = "Conformation letter"
        Me.chkconfletter.UseVisualStyleBackColor = True
        '
        'chkgpletterwithled
        '
        Me.chkgpletterwithled.AutoSize = True
        Me.chkgpletterwithled.Location = New System.Drawing.Point(144, 13)
        Me.chkgpletterwithled.Name = "chkgpletterwithled"
        Me.chkgpletterwithled.Size = New System.Drawing.Size(94, 17)
        Me.chkgpletterwithled.TabIndex = 1
        Me.chkgpletterwithled.Text = "With Ledger"
        Me.chkgpletterwithled.UseVisualStyleBackColor = True
        '
        'chklstgroup
        '
        Me.chklstgroup.FormattingEnabled = True
        Me.chklstgroup.Location = New System.Drawing.Point(22, 106)
        Me.chklstgroup.Name = "chklstgroup"
        Me.chklstgroup.Size = New System.Drawing.Size(300, 100)
        Me.chklstgroup.TabIndex = 7
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(24, 19)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(34, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Date"
        '
        'rbtSummary
        '
        Me.rbtSummary.AutoSize = True
        Me.rbtSummary.Location = New System.Drawing.Point(151, 299)
        Me.rbtSummary.Name = "rbtSummary"
        Me.rbtSummary.Size = New System.Drawing.Size(81, 17)
        Me.rbtSummary.TabIndex = 14
        Me.rbtSummary.TabStop = True
        Me.rbtSummary.Text = "Summary"
        Me.rbtSummary.UseVisualStyleBackColor = True
        '
        'rbtDetailed
        '
        Me.rbtDetailed.AutoSize = True
        Me.rbtDetailed.Location = New System.Drawing.Point(31, 299)
        Me.rbtDetailed.Name = "rbtDetailed"
        Me.rbtDetailed.Size = New System.Drawing.Size(72, 17)
        Me.rbtDetailed.TabIndex = 13
        Me.rbtDetailed.TabStop = True
        Me.rbtDetailed.Text = "Detailed"
        Me.rbtDetailed.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(201, 18)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(21, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "To"
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(240, 15)
        Me.dtpTo.Mask = "##-##-####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(80, 21)
        Me.dtpTo.TabIndex = 3
        Me.dtpTo.Text = "29/09/2010"
        Me.dtpTo.Value = New Date(2010, 9, 29, 0, 0, 0, 0)
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(112, 15)
        Me.dtpFrom.Mask = "##-##-####"
        Me.dtpFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom.Size = New System.Drawing.Size(82, 21)
        Me.dtpFrom.TabIndex = 1
        Me.dtpFrom.Text = "29/09/2010"
        Me.dtpFrom.Value = New Date(2010, 9, 29, 0, 0, 0, 0)
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(230, 409)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 20
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(18, 409)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 18
        Me.btnSearch.Text = "&Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(124, 409)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 19
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExitToolStripMenuItem, Me.NewToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(118, 48)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'GroupLedgerNew
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(852, 530)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.GrpContainer)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "GroupLedgerNew"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Group Ledger"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.GrpContainer.ResumeLayout(False)
        Me.GrpContainer.PerformLayout()
        CType(Me.DgvSearch, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GrpConf.ResumeLayout(False)
        Me.GrpConf.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GrpContainer As System.Windows.Forms.GroupBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents rbtSummary As System.Windows.Forms.RadioButton
    Friend WithEvents rbtDetailed As System.Windows.Forms.RadioButton
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents chklstgroup As System.Windows.Forms.CheckedListBox
    Friend WithEvents GrpConf As System.Windows.Forms.GroupBox
    Friend WithEvents chkconfletter As System.Windows.Forms.CheckBox
    Friend WithEvents chkgpletterwithled As System.Windows.Forms.CheckBox
    Friend WithEvents chkacgrpSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkCmbCostCentre As BrighttechPack.CheckedComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents chksmithbal As System.Windows.Forms.CheckBox
    Friend WithEvents ChkNilBalance As System.Windows.Forms.CheckBox
    Friend WithEvents ChkSepCol As System.Windows.Forms.CheckBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtAcname As System.Windows.Forms.TextBox
    Friend WithEvents chklstAcname As BrighttechPack.CheckedComboBox
    Friend WithEvents DgvSearch As System.Windows.Forms.DataGridView
    Friend WithEvents cmbAcName As System.Windows.Forms.ComboBox
    Friend WithEvents chkMultiSelect As System.Windows.Forms.CheckBox
End Class
