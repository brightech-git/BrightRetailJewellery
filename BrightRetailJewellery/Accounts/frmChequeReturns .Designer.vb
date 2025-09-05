<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmChequeReturns
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
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.Label = New System.Windows.Forms.Label
        Me.dtpGridRealise = New System.Windows.Forms.DateTimePicker
        Me.chkTranDate = New System.Windows.Forms.CheckBox
        Me.txtChqNo = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.lblhint = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.gridView_OWN = New System.Windows.Forms.DataGridView
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.btnSearch = New System.Windows.Forms.Button
        Me.cmbBank_MAN = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.chkCmbCostCentre = New BrighttechPack.CheckedComboBox
        Me.dtpChequeDateTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpChequeDateFrom = New BrighttechPack.DatePicker(Me.components)
        Me.dtpTranDateTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpTranDateFrom = New BrighttechPack.DatePicker(Me.components)
        Me.Label3 = New System.Windows.Forms.Label
        Me.GroupBox1.SuspendLayout()
        CType(Me.gridView_OWN, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.chkCmbCostCentre)
        Me.GroupBox1.Controls.Add(Me.Label)
        Me.GroupBox1.Controls.Add(Me.dtpGridRealise)
        Me.GroupBox1.Controls.Add(Me.chkTranDate)
        Me.GroupBox1.Controls.Add(Me.txtChqNo)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.dtpChequeDateTo)
        Me.GroupBox1.Controls.Add(Me.lblhint)
        Me.GroupBox1.Controls.Add(Me.dtpChequeDateFrom)
        Me.GroupBox1.Controls.Add(Me.dtpTranDateTo)
        Me.GroupBox1.Controls.Add(Me.Label8)
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.dtpTranDateFrom)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.gridView_OWN)
        Me.GroupBox1.Controls.Add(Me.btnExit)
        Me.GroupBox1.Controls.Add(Me.btnNew)
        Me.GroupBox1.Controls.Add(Me.btnSearch)
        Me.GroupBox1.Controls.Add(Me.cmbBank_MAN)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 11)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(998, 616)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        '
        'Label
        '
        Me.Label.AutoSize = True
        Me.Label.Location = New System.Drawing.Point(12, 16)
        Me.Label.Name = "Label"
        Me.Label.Size = New System.Drawing.Size(76, 13)
        Me.Label.TabIndex = 0
        Me.Label.Text = "Cost Centre"
        Me.Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'dtpGridRealise
        '
        Me.dtpGridRealise.CustomFormat = "dd/MM/yyyy"
        Me.dtpGridRealise.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpGridRealise.Location = New System.Drawing.Point(817, 98)
        Me.dtpGridRealise.Name = "dtpGridRealise"
        Me.dtpGridRealise.Size = New System.Drawing.Size(93, 21)
        Me.dtpGridRealise.TabIndex = 21
        Me.dtpGridRealise.Visible = False
        '
        'chkTranDate
        '
        Me.chkTranDate.AutoSize = True
        Me.chkTranDate.Checked = True
        Me.chkTranDate.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkTranDate.Location = New System.Drawing.Point(15, 68)
        Me.chkTranDate.Name = "chkTranDate"
        Me.chkTranDate.Size = New System.Drawing.Size(83, 17)
        Me.chkTranDate.TabIndex = 4
        Me.chkTranDate.Text = "Tran Date"
        Me.chkTranDate.UseVisualStyleBackColor = True
        '
        'txtChqNo
        '
        Me.txtChqNo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtChqNo.Location = New System.Drawing.Point(131, 119)
        Me.txtChqNo.MaxLength = 30
        Me.txtChqNo.Name = "txtChqNo"
        Me.txtChqNo.Size = New System.Drawing.Size(212, 21)
        Me.txtChqNo.TabIndex = 13
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 123)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(70, 13)
        Me.Label2.TabIndex = 12
        Me.Label2.Text = "Cheque No"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblhint
        '
        Me.lblhint.AutoSize = True
        Me.lblhint.BackColor = System.Drawing.Color.LightGoldenrodYellow
        Me.lblhint.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblhint.Location = New System.Drawing.Point(565, 599)
        Me.lblhint.Name = "lblhint"
        Me.lblhint.Size = New System.Drawing.Size(0, 13)
        Me.lblhint.TabIndex = 18
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(227, 98)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(21, 13)
        Me.Label8.TabIndex = 10
        Me.Label8.Text = "To"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(12, 98)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(115, 13)
        Me.Label7.TabIndex = 8
        Me.Label7.Text = "Cheque Date From"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(227, 72)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(21, 13)
        Me.Label5.TabIndex = 6
        Me.Label5.Text = "To"
        '
        'gridView_OWN
        '
        Me.gridView_OWN.AllowUserToAddRows = False
        Me.gridView_OWN.AllowUserToDeleteRows = False
        Me.gridView_OWN.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView_OWN.Location = New System.Drawing.Point(6, 149)
        Me.gridView_OWN.Name = "gridView_OWN"
        Me.gridView_OWN.ReadOnly = True
        Me.gridView_OWN.RowHeadersVisible = False
        Me.gridView_OWN.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridView_OWN.Size = New System.Drawing.Size(986, 460)
        Me.gridView_OWN.TabIndex = 20
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(558, 113)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 16
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(452, 113)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 15
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(349, 113)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 14
        Me.btnSearch.Text = "Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'cmbBank_MAN
        '
        Me.cmbBank_MAN.FormattingEnabled = True
        Me.cmbBank_MAN.Location = New System.Drawing.Point(131, 38)
        Me.cmbBank_MAN.Name = "cmbBank_MAN"
        Me.cmbBank_MAN.Size = New System.Drawing.Size(286, 21)
        Me.cmbBank_MAN.TabIndex = 3
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 41)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(122, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Bank Account Name"
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(129, 48)
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'chkCmbCostCentre
        '
        Me.chkCmbCostCentre.CheckOnClick = True
        Me.chkCmbCostCentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCostCentre.DropDownHeight = 1
        Me.chkCmbCostCentre.FormattingEnabled = True
        Me.chkCmbCostCentre.IntegralHeight = False
        Me.chkCmbCostCentre.Location = New System.Drawing.Point(131, 10)
        Me.chkCmbCostCentre.Name = "chkCmbCostCentre"
        Me.chkCmbCostCentre.Size = New System.Drawing.Size(286, 22)
        Me.chkCmbCostCentre.TabIndex = 1
        Me.chkCmbCostCentre.ValueSeparator = ", "
        '
        'dtpChequeDateTo
        '
        Me.dtpChequeDateTo.Location = New System.Drawing.Point(250, 92)
        Me.dtpChequeDateTo.Mask = "##/##/####"
        Me.dtpChequeDateTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpChequeDateTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpChequeDateTo.Name = "dtpChequeDateTo"
        Me.dtpChequeDateTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpChequeDateTo.Size = New System.Drawing.Size(93, 21)
        Me.dtpChequeDateTo.TabIndex = 11
        Me.dtpChequeDateTo.Text = "06/03/9998"
        Me.dtpChequeDateTo.Value = New Date(9998, 3, 6, 0, 0, 0, 0)
        '
        'dtpChequeDateFrom
        '
        Me.dtpChequeDateFrom.Location = New System.Drawing.Point(131, 92)
        Me.dtpChequeDateFrom.Mask = "##/##/####"
        Me.dtpChequeDateFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpChequeDateFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpChequeDateFrom.Name = "dtpChequeDateFrom"
        Me.dtpChequeDateFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpChequeDateFrom.Size = New System.Drawing.Size(93, 21)
        Me.dtpChequeDateFrom.TabIndex = 9
        Me.dtpChequeDateFrom.Text = "06/03/9998"
        Me.dtpChequeDateFrom.Value = New Date(9998, 3, 6, 0, 0, 0, 0)
        '
        'dtpTranDateTo
        '
        Me.dtpTranDateTo.Location = New System.Drawing.Point(250, 65)
        Me.dtpTranDateTo.Mask = "##/##/####"
        Me.dtpTranDateTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTranDateTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTranDateTo.Name = "dtpTranDateTo"
        Me.dtpTranDateTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTranDateTo.Size = New System.Drawing.Size(93, 21)
        Me.dtpTranDateTo.TabIndex = 7
        Me.dtpTranDateTo.Text = "06/03/9998"
        Me.dtpTranDateTo.Value = New Date(9998, 3, 6, 0, 0, 0, 0)
        '
        'dtpTranDateFrom
        '
        Me.dtpTranDateFrom.Location = New System.Drawing.Point(131, 65)
        Me.dtpTranDateFrom.Mask = "##/##/####"
        Me.dtpTranDateFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTranDateFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTranDateFrom.Name = "dtpTranDateFrom"
        Me.dtpTranDateFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTranDateFrom.Size = New System.Drawing.Size(93, 21)
        Me.dtpTranDateFrom.TabIndex = 5
        Me.dtpTranDateFrom.Text = "06/03/9998"
        Me.dtpTranDateFrom.Value = New Date(9998, 3, 6, 0, 0, 0, 0)
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(732, 127)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(140, 13)
        Me.Label3.TabIndex = 22
        Me.Label3.Text = "Press R Cheque Return"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'frmChequeReturns
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1022, 640)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.GroupBox1)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MinimizeBox = False
        Me.Name = "frmChequeReturns"
        Me.Text = "Cheque Returns"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.gridView_OWN, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents cmbBank_MAN As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents gridView_OWN As System.Windows.Forms.DataGridView
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents dtpTranDateTo As BrighttechPack.DatePicker
    Friend WithEvents dtpTranDateFrom As BrighttechPack.DatePicker
    Friend WithEvents lblhint As System.Windows.Forms.Label
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents dtpChequeDateTo As BrighttechPack.DatePicker
    Friend WithEvents dtpChequeDateFrom As BrighttechPack.DatePicker
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtChqNo As System.Windows.Forms.TextBox
    Friend WithEvents chkTranDate As System.Windows.Forms.CheckBox
    Friend WithEvents dtpGridRealise As System.Windows.Forms.DateTimePicker
    Friend WithEvents chkCmbCostCentre As BrighttechPack.CheckedComboBox
    Friend WithEvents Label As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
End Class
