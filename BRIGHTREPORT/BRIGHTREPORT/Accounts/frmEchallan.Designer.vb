<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmEchallan
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
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.btnNew = New System.Windows.Forms.Button
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.Label3 = New System.Windows.Forms.Label
        Me.dtpDepositDate = New BrighttechPack.DatePicker(Me.components)
        Me.Label2 = New System.Windows.Forms.Label
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.txtViewSlipNo_NUM = New System.Windows.Forms.TextBox
        Me.btnReport = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.btnTransfer = New System.Windows.Forms.Button
        Me.LblWords = New System.Windows.Forms.Label
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnView = New System.Windows.Forms.Button
        Me.TxtActype = New System.Windows.Forms.TextBox
        Me.lblActype = New System.Windows.Forms.Label
        Me.TxtAcno = New System.Windows.Forms.TextBox
        Me.Txtslipno = New System.Windows.Forms.TextBox
        Me.lblslipno = New System.Windows.Forms.Label
        Me.TxtAmount = New System.Windows.Forms.TextBox
        Me.lblAmount = New System.Windows.Forms.Label
        Me.lbldate = New System.Windows.Forms.Label
        Me.lblacno = New System.Windows.Forms.Label
        Me.cmbBankname = New System.Windows.Forms.ComboBox
        Me.lblbankname = New System.Windows.Forms.Label
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.gridView = New System.Windows.Forms.DataGridView
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AutoReziseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.View = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ViewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ReportToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.Panel1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.View.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.btnNew)
        Me.Panel1.Controls.Add(Me.dtpTo)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.dtpDepositDate)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.GroupBox1)
        Me.Panel1.Controls.Add(Me.btnTransfer)
        Me.Panel1.Controls.Add(Me.LblWords)
        Me.Panel1.Controls.Add(Me.dtpFrom)
        Me.Panel1.Controls.Add(Me.btnExit)
        Me.Panel1.Controls.Add(Me.btnView)
        Me.Panel1.Controls.Add(Me.TxtActype)
        Me.Panel1.Controls.Add(Me.lblActype)
        Me.Panel1.Controls.Add(Me.TxtAcno)
        Me.Panel1.Controls.Add(Me.Txtslipno)
        Me.Panel1.Controls.Add(Me.lblslipno)
        Me.Panel1.Controls.Add(Me.TxtAmount)
        Me.Panel1.Controls.Add(Me.lblAmount)
        Me.Panel1.Controls.Add(Me.lbldate)
        Me.Panel1.Controls.Add(Me.lblacno)
        Me.Panel1.Controls.Add(Me.cmbBankname)
        Me.Panel1.Controls.Add(Me.lblbankname)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(806, 181)
        Me.Panel1.TabIndex = 0
        '
        'btnNew
        '
        Me.btnNew.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNew.Location = New System.Drawing.Point(208, 146)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 18
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'dtpTo
        '
        Me.dtpTo.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpTo.Location = New System.Drawing.Point(247, 6)
        Me.dtpTo.Mask = "##/##/####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(45)
        Me.dtpTo.Size = New System.Drawing.Size(75, 21)
        Me.dtpTo.TabIndex = 3
        Me.dtpTo.Text = "07-11-2014"
        Me.dtpTo.Value = New Date(2014, 11, 7, 0, 0, 0, 0)
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(191, 9)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(52, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Date To"
        '
        'dtpDepositDate
        '
        Me.dtpDepositDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpDepositDate.Location = New System.Drawing.Point(110, 122)
        Me.dtpDepositDate.Mask = "##/##/####"
        Me.dtpDepositDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpDepositDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpDepositDate.Name = "dtpDepositDate"
        Me.dtpDepositDate.Seperator = Global.Microsoft.VisualBasic.ChrW(45)
        Me.dtpDepositDate.Size = New System.Drawing.Size(75, 21)
        Me.dtpDepositDate.TabIndex = 13
        Me.dtpDepositDate.Text = "07-11-2014"
        Me.dtpDepositDate.Value = New Date(2014, 11, 7, 0, 0, 0, 0)
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(31, 125)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(81, 13)
        Me.Label2.TabIndex = 12
        Me.Label2.Text = "Deposit Date"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.txtViewSlipNo_NUM)
        Me.GroupBox1.Controls.Add(Me.btnReport)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Location = New System.Drawing.Point(329, 7)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(283, 57)
        Me.GroupBox1.TabIndex = 17
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Duplicate Print"
        '
        'txtViewSlipNo_NUM
        '
        Me.txtViewSlipNo_NUM.BackColor = System.Drawing.SystemColors.Window
        Me.txtViewSlipNo_NUM.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtViewSlipNo_NUM.Location = New System.Drawing.Point(60, 26)
        Me.txtViewSlipNo_NUM.Name = "txtViewSlipNo_NUM"
        Me.txtViewSlipNo_NUM.Size = New System.Drawing.Size(100, 21)
        Me.txtViewSlipNo_NUM.TabIndex = 1
        '
        'btnReport
        '
        Me.btnReport.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnReport.Location = New System.Drawing.Point(166, 22)
        Me.btnReport.Name = "btnReport"
        Me.btnReport.Size = New System.Drawing.Size(100, 30)
        Me.btnReport.TabIndex = 2
        Me.btnReport.Text = "Report [F5]"
        Me.btnReport.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(9, 29)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(43, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "SlipNo"
        '
        'btnTransfer
        '
        Me.btnTransfer.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnTransfer.Location = New System.Drawing.Point(308, 146)
        Me.btnTransfer.Name = "btnTransfer"
        Me.btnTransfer.Size = New System.Drawing.Size(100, 30)
        Me.btnTransfer.TabIndex = 15
        Me.btnTransfer.Text = "Transfer"
        Me.btnTransfer.UseVisualStyleBackColor = True
        '
        'LblWords
        '
        Me.LblWords.AutoSize = True
        Me.LblWords.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblWords.ForeColor = System.Drawing.Color.Red
        Me.LblWords.Location = New System.Drawing.Point(332, 103)
        Me.LblWords.Name = "LblWords"
        Me.LblWords.Size = New System.Drawing.Size(0, 13)
        Me.LblWords.TabIndex = 12
        '
        'dtpFrom
        '
        Me.dtpFrom.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpFrom.Location = New System.Drawing.Point(110, 6)
        Me.dtpFrom.Mask = "##/##/####"
        Me.dtpFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(45)
        Me.dtpFrom.Size = New System.Drawing.Size(75, 21)
        Me.dtpFrom.TabIndex = 1
        Me.dtpFrom.Text = "07-11-2014"
        Me.dtpFrom.Value = New Date(2014, 11, 7, 0, 0, 0, 0)
        '
        'btnExit
        '
        Me.btnExit.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExit.Location = New System.Drawing.Point(408, 146)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 16
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnView
        '
        Me.btnView.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnView.Location = New System.Drawing.Point(108, 146)
        Me.btnView.Name = "btnView"
        Me.btnView.Size = New System.Drawing.Size(100, 30)
        Me.btnView.TabIndex = 14
        Me.btnView.Text = "View [F2]"
        Me.btnView.UseVisualStyleBackColor = True
        '
        'TxtActype
        '
        Me.TxtActype.BackColor = System.Drawing.SystemColors.Window
        Me.TxtActype.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtActype.Location = New System.Drawing.Point(110, 75)
        Me.TxtActype.Name = "TxtActype"
        Me.TxtActype.ReadOnly = True
        Me.TxtActype.Size = New System.Drawing.Size(212, 21)
        Me.TxtActype.TabIndex = 9
        '
        'lblActype
        '
        Me.lblActype.AutoSize = True
        Me.lblActype.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblActype.Location = New System.Drawing.Point(31, 79)
        Me.lblActype.Name = "lblActype"
        Me.lblActype.Size = New System.Drawing.Size(46, 13)
        Me.lblActype.TabIndex = 8
        Me.lblActype.Text = "Actype"
        '
        'TxtAcno
        '
        Me.TxtAcno.BackColor = System.Drawing.SystemColors.Window
        Me.TxtAcno.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtAcno.Location = New System.Drawing.Point(110, 52)
        Me.TxtAcno.Name = "TxtAcno"
        Me.TxtAcno.ReadOnly = True
        Me.TxtAcno.Size = New System.Drawing.Size(212, 21)
        Me.TxtAcno.TabIndex = 7
        '
        'Txtslipno
        '
        Me.Txtslipno.BackColor = System.Drawing.SystemColors.Window
        Me.Txtslipno.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Txtslipno.Location = New System.Drawing.Point(694, 13)
        Me.Txtslipno.Name = "Txtslipno"
        Me.Txtslipno.ReadOnly = True
        Me.Txtslipno.Size = New System.Drawing.Size(100, 21)
        Me.Txtslipno.TabIndex = 3
        Me.Txtslipno.Visible = False
        '
        'lblslipno
        '
        Me.lblslipno.AutoSize = True
        Me.lblslipno.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblslipno.Location = New System.Drawing.Point(643, 16)
        Me.lblslipno.Name = "lblslipno"
        Me.lblslipno.Size = New System.Drawing.Size(43, 13)
        Me.lblslipno.TabIndex = 2
        Me.lblslipno.Text = "SlipNo"
        Me.lblslipno.Visible = False
        '
        'TxtAmount
        '
        Me.TxtAmount.BackColor = System.Drawing.SystemColors.Window
        Me.TxtAmount.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtAmount.Location = New System.Drawing.Point(110, 98)
        Me.TxtAmount.Name = "TxtAmount"
        Me.TxtAmount.ReadOnly = True
        Me.TxtAmount.Size = New System.Drawing.Size(212, 21)
        Me.TxtAmount.TabIndex = 11
        '
        'lblAmount
        '
        Me.lblAmount.AutoSize = True
        Me.lblAmount.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAmount.Location = New System.Drawing.Point(31, 101)
        Me.lblAmount.Name = "lblAmount"
        Me.lblAmount.Size = New System.Drawing.Size(51, 13)
        Me.lblAmount.TabIndex = 10
        Me.lblAmount.Text = "Amount"
        '
        'lbldate
        '
        Me.lbldate.AutoSize = True
        Me.lbldate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbldate.Location = New System.Drawing.Point(31, 9)
        Me.lbldate.Name = "lbldate"
        Me.lbldate.Size = New System.Drawing.Size(67, 13)
        Me.lbldate.TabIndex = 0
        Me.lbldate.Text = "Date From"
        '
        'lblacno
        '
        Me.lblacno.AutoSize = True
        Me.lblacno.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblacno.Location = New System.Drawing.Point(31, 55)
        Me.lblacno.Name = "lblacno"
        Me.lblacno.Size = New System.Drawing.Size(71, 13)
        Me.lblacno.TabIndex = 6
        Me.lblacno.Text = "Account No"
        '
        'cmbBankname
        '
        Me.cmbBankname.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbBankname.FormattingEnabled = True
        Me.cmbBankname.Location = New System.Drawing.Point(110, 29)
        Me.cmbBankname.Name = "cmbBankname"
        Me.cmbBankname.Size = New System.Drawing.Size(212, 21)
        Me.cmbBankname.TabIndex = 5
        '
        'lblbankname
        '
        Me.lblbankname.AutoSize = True
        Me.lblbankname.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblbankname.Location = New System.Drawing.Point(31, 33)
        Me.lblbankname.Name = "lblbankname"
        Me.lblbankname.Size = New System.Drawing.Size(73, 13)
        Me.lblbankname.TabIndex = 4
        Me.lblbankname.Text = "Bank Name"
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.gridView)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 181)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(806, 295)
        Me.Panel2.TabIndex = 1
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.ContextMenuStrip = Me.ContextMenuStrip1
        Me.gridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView.Location = New System.Drawing.Point(0, 0)
        Me.gridView.Name = "gridView"
        Me.gridView.RowHeadersVisible = False
        Me.gridView.Size = New System.Drawing.Size(806, 295)
        Me.gridView.TabIndex = 0
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AutoReziseToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(140, 26)
        '
        'AutoReziseToolStripMenuItem
        '
        Me.AutoReziseToolStripMenuItem.CheckOnClick = True
        Me.AutoReziseToolStripMenuItem.Name = "AutoReziseToolStripMenuItem"
        Me.AutoReziseToolStripMenuItem.Size = New System.Drawing.Size(139, 22)
        Me.AutoReziseToolStripMenuItem.Text = "AutoRezise"
        '
        'View
        '
        Me.View.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ViewToolStripMenuItem, Me.ReportToolStripMenuItem, Me.ExToolStripMenuItem, Me.NewToolStripMenuItem})
        Me.View.Name = "View"
        Me.View.Size = New System.Drawing.Size(138, 92)
        '
        'ViewToolStripMenuItem
        '
        Me.ViewToolStripMenuItem.Name = "ViewToolStripMenuItem"
        Me.ViewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2
        Me.ViewToolStripMenuItem.Size = New System.Drawing.Size(137, 22)
        Me.ViewToolStripMenuItem.Text = "View"
        Me.ViewToolStripMenuItem.Visible = False
        '
        'ReportToolStripMenuItem
        '
        Me.ReportToolStripMenuItem.Name = "ReportToolStripMenuItem"
        Me.ReportToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5
        Me.ReportToolStripMenuItem.Size = New System.Drawing.Size(137, 22)
        Me.ReportToolStripMenuItem.Text = "Report"
        Me.ReportToolStripMenuItem.Visible = False
        '
        'ExToolStripMenuItem
        '
        Me.ExToolStripMenuItem.Name = "ExToolStripMenuItem"
        Me.ExToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExToolStripMenuItem.Size = New System.Drawing.Size(137, 22)
        Me.ExToolStripMenuItem.Text = "Exit"
        Me.ExToolStripMenuItem.Visible = False
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(137, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'frmEchallan
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(806, 476)
        Me.ContextMenuStrip = Me.View
        Me.ControlBox = False
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmEchallan"
        Me.Text = "E-Chellan"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.View.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents lblbankname As System.Windows.Forms.Label
    Friend WithEvents cmbBankname As System.Windows.Forms.ComboBox
    Friend WithEvents lblacno As System.Windows.Forms.Label
    Friend WithEvents lbldate As System.Windows.Forms.Label
    Friend WithEvents Txtslipno As System.Windows.Forms.TextBox
    Friend WithEvents lblslipno As System.Windows.Forms.Label
    Friend WithEvents TxtAmount As System.Windows.Forms.TextBox
    Friend WithEvents lblAmount As System.Windows.Forms.Label
    Friend WithEvents TxtAcno As System.Windows.Forms.TextBox
    Friend WithEvents TxtActype As System.Windows.Forms.TextBox
    Friend WithEvents lblActype As System.Windows.Forms.Label
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents btnReport As System.Windows.Forms.Button
    Friend WithEvents btnView As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents View As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ViewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ReportToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents AutoReziseToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LblWords As System.Windows.Forms.Label
    Friend WithEvents btnTransfer As System.Windows.Forms.Button
    Friend WithEvents txtViewSlipNo_NUM As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents dtpDepositDate As BrighttechPack.DatePicker
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class
