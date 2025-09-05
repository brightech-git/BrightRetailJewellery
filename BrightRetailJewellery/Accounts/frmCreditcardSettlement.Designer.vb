<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCreditcardSettlement
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
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.btntemplate = New System.Windows.Forms.Button
        Me.gridView_OWN = New System.Windows.Forms.DataGridView
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.btnSearch = New System.Windows.Forms.Button
        Me.cmbGenerateVoucher_OWN = New System.Windows.Forms.ComboBox
        Me.cmbstaxac_OWN = New System.Windows.Forms.ComboBox
        Me.cmbschargesAC_OWN = New System.Windows.Forms.ComboBox
        Me.cmbchargesAC_OWN = New System.Windows.Forms.ComboBox
        Me.BtnSave = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.cmbBank_MAN = New System.Windows.Forms.ComboBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.btnNew = New System.Windows.Forms.Button
        Me.btnExit = New System.Windows.Forms.Button
        Me.Label5 = New System.Windows.Forms.Label
        Me.lblStatus = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.txtPath = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.btnPath = New System.Windows.Forms.Button
        Me.dtpTranDate = New BrighttechPack.DatePicker(Me.components)
        Me.lblBankName = New System.Windows.Forms.Label
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.saveToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem
        Me.GroupBox1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        CType(Me.gridView_OWN, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Panel2)
        Me.GroupBox1.Controls.Add(Me.Panel1)
        Me.GroupBox1.Controls.Add(Me.lblBankName)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 11)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(998, 616)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.btntemplate)
        Me.Panel2.Controls.Add(Me.gridView_OWN)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(3, 134)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(992, 479)
        Me.Panel2.TabIndex = 26
        '
        'btntemplate
        '
        Me.btntemplate.Location = New System.Drawing.Point(580, 27)
        Me.btntemplate.Name = "btntemplate"
        Me.btntemplate.Size = New System.Drawing.Size(100, 30)
        Me.btntemplate.TabIndex = 17
        Me.btntemplate.Text = "Template"
        Me.btntemplate.UseVisualStyleBackColor = True
        Me.btntemplate.Visible = False
        '
        'gridView_OWN
        '
        Me.gridView_OWN.AllowUserToAddRows = False
        Me.gridView_OWN.AllowUserToDeleteRows = False
        Me.gridView_OWN.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView_OWN.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView_OWN.Location = New System.Drawing.Point(0, 0)
        Me.gridView_OWN.Name = "gridView_OWN"
        Me.gridView_OWN.ReadOnly = True
        Me.gridView_OWN.RowHeadersVisible = False
        Me.gridView_OWN.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridView_OWN.Size = New System.Drawing.Size(992, 479)
        Me.gridView_OWN.TabIndex = 0
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.btnSearch)
        Me.Panel1.Controls.Add(Me.cmbGenerateVoucher_OWN)
        Me.Panel1.Controls.Add(Me.cmbstaxac_OWN)
        Me.Panel1.Controls.Add(Me.cmbschargesAC_OWN)
        Me.Panel1.Controls.Add(Me.cmbchargesAC_OWN)
        Me.Panel1.Controls.Add(Me.BtnSave)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.Label7)
        Me.Panel1.Controls.Add(Me.cmbBank_MAN)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.btnNew)
        Me.Panel1.Controls.Add(Me.btnExit)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.lblStatus)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.txtPath)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.btnPath)
        Me.Panel1.Controls.Add(Me.dtpTranDate)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(3, 17)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(992, 117)
        Me.Panel1.TabIndex = 0
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(102, 80)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 15
        Me.btnSearch.Text = "Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'cmbGenerateVoucher_OWN
        '
        Me.cmbGenerateVoucher_OWN.FormattingEnabled = True
        Me.cmbGenerateVoucher_OWN.Location = New System.Drawing.Point(123, 8)
        Me.cmbGenerateVoucher_OWN.Name = "cmbGenerateVoucher_OWN"
        Me.cmbGenerateVoucher_OWN.Size = New System.Drawing.Size(114, 21)
        Me.cmbGenerateVoucher_OWN.TabIndex = 3
        '
        'cmbstaxac_OWN
        '
        Me.cmbstaxac_OWN.FormattingEnabled = True
        Me.cmbstaxac_OWN.Location = New System.Drawing.Point(622, 32)
        Me.cmbstaxac_OWN.Name = "cmbstaxac_OWN"
        Me.cmbstaxac_OWN.Size = New System.Drawing.Size(332, 21)
        Me.cmbstaxac_OWN.TabIndex = 11
        '
        'cmbschargesAC_OWN
        '
        Me.cmbschargesAC_OWN.FormattingEnabled = True
        Me.cmbschargesAC_OWN.Location = New System.Drawing.Point(622, 8)
        Me.cmbschargesAC_OWN.Name = "cmbschargesAC_OWN"
        Me.cmbschargesAC_OWN.Size = New System.Drawing.Size(332, 21)
        Me.cmbschargesAC_OWN.TabIndex = 9
        '
        'cmbchargesAC_OWN
        '
        Me.cmbchargesAC_OWN.FormattingEnabled = True
        Me.cmbchargesAC_OWN.Location = New System.Drawing.Point(123, 54)
        Me.cmbchargesAC_OWN.Name = "cmbchargesAC_OWN"
        Me.cmbchargesAC_OWN.Size = New System.Drawing.Size(309, 21)
        Me.cmbchargesAC_OWN.TabIndex = 7
        '
        'BtnSave
        '
        Me.BtnSave.Location = New System.Drawing.Point(203, 81)
        Me.BtnSave.Name = "BtnSave"
        Me.BtnSave.Size = New System.Drawing.Size(100, 30)
        Me.BtnSave.TabIndex = 16
        Me.BtnSave.Text = "Save[F1]"
        Me.BtnSave.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(658, 93)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(64, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Tran Date"
        Me.Label2.Visible = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(3, 34)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(119, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "CreditCard Account"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(436, 35)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(187, 13)
        Me.Label7.TabIndex = 10
        Me.Label7.Text = "CreditCard ServiceTax Account"
        '
        'cmbBank_MAN
        '
        Me.cmbBank_MAN.FormattingEnabled = True
        Me.cmbBank_MAN.Location = New System.Drawing.Point(123, 31)
        Me.cmbBank_MAN.Name = "cmbBank_MAN"
        Me.cmbBank_MAN.Size = New System.Drawing.Size(309, 21)
        Me.cmbBank_MAN.TabIndex = 5
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(436, 12)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(168, 13)
        Me.Label6.TabIndex = 8
        Me.Label6.Text = "Creditcard Charges Account"
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(304, 81)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 17
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(404, 82)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 18
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(3, 58)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(85, 13)
        Me.Label5.TabIndex = 6
        Me.Label5.Text = "Bank Account"
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatus.ForeColor = System.Drawing.Color.Red
        Me.lblStatus.Location = New System.Drawing.Point(510, 91)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(50, 13)
        Me.lblStatus.TabIndex = 19
        Me.lblStatus.Text = "Label3"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(4, 12)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(111, 13)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "Generate Voucher"
        '
        'txtPath
        '
        Me.txtPath.Location = New System.Drawing.Point(622, 56)
        Me.txtPath.Name = "txtPath"
        Me.txtPath.Size = New System.Drawing.Size(332, 21)
        Me.txtPath.TabIndex = 13
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(437, 59)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(51, 13)
        Me.Label3.TabIndex = 12
        Me.Label3.Text = "FilePath"
        '
        'btnPath
        '
        Me.btnPath.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPath.Location = New System.Drawing.Point(960, 59)
        Me.btnPath.Name = "btnPath"
        Me.btnPath.Size = New System.Drawing.Size(20, 21)
        Me.btnPath.TabIndex = 14
        Me.btnPath.Text = "?"
        Me.btnPath.UseVisualStyleBackColor = True
        '
        'dtpTranDate
        '
        Me.dtpTranDate.Location = New System.Drawing.Point(778, 90)
        Me.dtpTranDate.Mask = "##/##/####"
        Me.dtpTranDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTranDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTranDate.Name = "dtpTranDate"
        Me.dtpTranDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTranDate.Size = New System.Drawing.Size(74, 21)
        Me.dtpTranDate.TabIndex = 1
        Me.dtpTranDate.Text = "06/03/9998"
        Me.dtpTranDate.Value = New Date(9998, 3, 6, 0, 0, 0, 0)
        Me.dtpTranDate.Visible = False
        '
        'lblBankName
        '
        Me.lblBankName.AutoSize = True
        Me.lblBankName.BackColor = System.Drawing.Color.LightGoldenrodYellow
        Me.lblBankName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBankName.Location = New System.Drawing.Point(11, 597)
        Me.lblBankName.Name = "lblBankName"
        Me.lblBankName.Size = New System.Drawing.Size(88, 13)
        Me.lblBankName.TabIndex = 1
        Me.lblBankName.Text = "Bank Name :"
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem, Me.saveToolStripMenuItem1})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(224, 70)
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(223, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(223, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'saveToolStripMenuItem1
        '
        Me.saveToolStripMenuItem1.Name = "saveToolStripMenuItem1"
        Me.saveToolStripMenuItem1.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.saveToolStripMenuItem1.Size = New System.Drawing.Size(223, 22)
        Me.saveToolStripMenuItem1.Text = "saveToolStripMenuItem1"
        '
        'frmCreditcardSettlement
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
        Me.Name = "frmCreditcardSettlement"
        Me.Text = "Credit Card Settlement"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        CType(Me.gridView_OWN, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents gridView_OWN As System.Windows.Forms.DataGridView
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblBankName As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents dtpTranDate As BrighttechPack.DatePicker
    Friend WithEvents btnPath As System.Windows.Forms.Button
    Friend WithEvents txtPath As System.Windows.Forms.TextBox
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents cmbBank_MAN As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents BtnSave As System.Windows.Forms.Button
    Friend WithEvents saveToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmbstaxac_OWN As System.Windows.Forms.ComboBox
    Friend WithEvents cmbschargesAC_OWN As System.Windows.Forms.ComboBox
    Friend WithEvents cmbchargesAC_OWN As System.Windows.Forms.ComboBox
    Friend WithEvents cmbGenerateVoucher_OWN As System.Windows.Forms.ComboBox
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents btntemplate As System.Windows.Forms.Button
End Class
