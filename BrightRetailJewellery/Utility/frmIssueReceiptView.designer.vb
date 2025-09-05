<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmIssueReceiptView
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
        Dim DataGridViewCellStyle15 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.Panel5 = New System.Windows.Forms.Panel
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.txtremark2 = New System.Windows.Forms.TextBox
        Me.chkAsOnDate = New System.Windows.Forms.CheckBox
        Me.txtremark1 = New System.Windows.Forms.TextBox
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.PictureBox3 = New System.Windows.Forms.PictureBox
        Me.PictureBox4 = New System.Windows.Forms.PictureBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.btnGenerate = New System.Windows.Forms.Button
        Me.Label8 = New System.Windows.Forms.Label
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.rbtReceipt = New System.Windows.Forms.RadioButton
        Me.rbtIssue = New System.Windows.Forms.RadioButton
        Me.rbtBoth = New System.Windows.Forms.RadioButton
        Me.cmbmetal_OWN = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.cmbcostcentre_OWN = New System.Windows.Forms.ComboBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.btnnew = New System.Windows.Forms.Button
        Me.cmbfrmcostcentre_OWN = New GControl.GCheckedComboBox
        Me.btnexit = New System.Windows.Forms.Button
        Me.btnexport = New System.Windows.Forms.Button
        Me.Btnsearch = New System.Windows.Forms.Button
        Me.pnlmark = New System.Windows.Forms.Panel
        Me.rbtunmark = New System.Windows.Forms.RadioButton
        Me.rbtmark = New System.Windows.Forms.RadioButton
        Me.rbtall = New System.Windows.Forms.RadioButton
        Me.Label1 = New System.Windows.Forms.Label
        Me.dtptodate = New GiriDatePicker.DatePicker(Me.components)
        Me.Label7 = New System.Windows.Forms.Label
        Me.dtpfromdate = New GiriDatePicker.DatePicker(Me.components)
        Me.exportContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.actualToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem
        Me.searchToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem
        Me.exportToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem
        Me.exitToolStripMenuItem3 = New System.Windows.Forms.ToolStripMenuItem
        Me.Panel6 = New System.Windows.Forms.Panel
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.chkcheck = New System.Windows.Forms.CheckBox
        Me.gridview = New System.Windows.Forms.DataGridView
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.Panel5.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.pnlmark.SuspendLayout()
        Me.exportContextMenuStrip1.SuspendLayout()
        Me.Panel6.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.gridview, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel5
        '
        Me.Panel5.BackColor = System.Drawing.Color.SteelBlue
        Me.Panel5.Controls.Add(Me.GroupBox1)
        Me.Panel5.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Panel5.Location = New System.Drawing.Point(0, 0)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(1022, 140)
        Me.Panel5.TabIndex = 2
        '
        'GroupBox1
        '
        Me.GroupBox1.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox1.Controls.Add(Me.Label10)
        Me.GroupBox1.Controls.Add(Me.txtremark2)
        Me.GroupBox1.Controls.Add(Me.chkAsOnDate)
        Me.GroupBox1.Controls.Add(Me.txtremark1)
        Me.GroupBox1.Controls.Add(Me.Panel2)
        Me.GroupBox1.Controls.Add(Me.Label9)
        Me.GroupBox1.Controls.Add(Me.btnGenerate)
        Me.GroupBox1.Controls.Add(Me.Label8)
        Me.GroupBox1.Controls.Add(Me.Panel1)
        Me.GroupBox1.Controls.Add(Me.cmbmetal_OWN)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.cmbcostcentre_OWN)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.btnnew)
        Me.GroupBox1.Controls.Add(Me.cmbfrmcostcentre_OWN)
        Me.GroupBox1.Controls.Add(Me.btnexit)
        Me.GroupBox1.Controls.Add(Me.btnexport)
        Me.GroupBox1.Controls.Add(Me.Btnsearch)
        Me.GroupBox1.Controls.Add(Me.pnlmark)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.dtptodate)
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.dtpfromdate)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(1022, 140)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = " "
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(214, 19)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(25, 13)
        Me.Label10.TabIndex = 23
        Me.Label10.Text = "To "
        '
        'txtremark2
        '
        Me.txtremark2.Location = New System.Drawing.Point(97, 117)
        Me.txtremark2.Name = "txtremark2"
        Me.txtremark2.Size = New System.Drawing.Size(252, 21)
        Me.txtremark2.TabIndex = 22
        '
        'chkAsOnDate
        '
        Me.chkAsOnDate.AutoSize = True
        Me.chkAsOnDate.BackColor = System.Drawing.SystemColors.Control
        Me.chkAsOnDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkAsOnDate.Location = New System.Drawing.Point(3, 18)
        Me.chkAsOnDate.Name = "chkAsOnDate"
        Me.chkAsOnDate.Size = New System.Drawing.Size(91, 17)
        Me.chkAsOnDate.TabIndex = 1
        Me.chkAsOnDate.Text = "As On Date"
        Me.chkAsOnDate.UseVisualStyleBackColor = False
        '
        'txtremark1
        '
        Me.txtremark1.Location = New System.Drawing.Point(97, 90)
        Me.txtremark1.Name = "txtremark1"
        Me.txtremark1.Size = New System.Drawing.Size(252, 21)
        Me.txtremark1.TabIndex = 21
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.Label5)
        Me.Panel2.Controls.Add(Me.Label3)
        Me.Panel2.Controls.Add(Me.Label6)
        Me.Panel2.Controls.Add(Me.PictureBox3)
        Me.Panel2.Controls.Add(Me.PictureBox4)
        Me.Panel2.Location = New System.Drawing.Point(605, 16)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(345, 27)
        Me.Panel2.TabIndex = 15
        Me.Panel2.Visible = False
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(207, 7)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(128, 13)
        Me.Label5.TabIndex = 2
        Me.Label5.Text = "[D] Duplicate Print"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(121, 8)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(59, 13)
        Me.Label3.TabIndex = 1
        Me.Label3.Text = "Pending"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(32, 8)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(55, 13)
        Me.Label6.TabIndex = 0
        Me.Label6.Text = "Marked"
        '
        'PictureBox3
        '
        Me.PictureBox3.BackColor = System.Drawing.Color.PaleGoldenrod
        Me.PictureBox3.Location = New System.Drawing.Point(93, 6)
        Me.PictureBox3.Name = "PictureBox3"
        Me.PictureBox3.Size = New System.Drawing.Size(22, 14)
        Me.PictureBox3.TabIndex = 1
        Me.PictureBox3.TabStop = False
        '
        'PictureBox4
        '
        Me.PictureBox4.BackColor = System.Drawing.Color.PowderBlue
        Me.PictureBox4.Location = New System.Drawing.Point(4, 6)
        Me.PictureBox4.Name = "PictureBox4"
        Me.PictureBox4.Size = New System.Drawing.Size(22, 14)
        Me.PictureBox4.TabIndex = 0
        Me.PictureBox4.TabStop = False
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(9, 121)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(59, 13)
        Me.Label9.TabIndex = 16
        Me.Label9.Text = "Remark2"
        '
        'btnGenerate
        '
        Me.btnGenerate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnGenerate.Location = New System.Drawing.Point(570, 62)
        Me.btnGenerate.Name = "btnGenerate"
        Me.btnGenerate.Size = New System.Drawing.Size(103, 28)
        Me.btnGenerate.TabIndex = 12
        Me.btnGenerate.Text = "&Generate"
        Me.btnGenerate.UseVisualStyleBackColor = True
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(9, 92)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(59, 13)
        Me.Label8.TabIndex = 20
        Me.Label8.Text = "Remark1"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.rbtReceipt)
        Me.Panel1.Controls.Add(Me.rbtIssue)
        Me.Panel1.Controls.Add(Me.rbtBoth)
        Me.Panel1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Panel1.Location = New System.Drawing.Point(412, 113)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(222, 22)
        Me.Panel1.TabIndex = 0
        '
        'rbtReceipt
        '
        Me.rbtReceipt.AutoSize = True
        Me.rbtReceipt.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbtReceipt.Location = New System.Drawing.Point(82, 3)
        Me.rbtReceipt.Name = "rbtReceipt"
        Me.rbtReceipt.Size = New System.Drawing.Size(67, 17)
        Me.rbtReceipt.TabIndex = 1
        Me.rbtReceipt.Text = "Receipt"
        Me.rbtReceipt.UseVisualStyleBackColor = True
        Me.rbtReceipt.Visible = False
        '
        'rbtIssue
        '
        Me.rbtIssue.AutoSize = True
        Me.rbtIssue.Checked = True
        Me.rbtIssue.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbtIssue.Location = New System.Drawing.Point(11, 3)
        Me.rbtIssue.Name = "rbtIssue"
        Me.rbtIssue.Size = New System.Drawing.Size(56, 17)
        Me.rbtIssue.TabIndex = 0
        Me.rbtIssue.TabStop = True
        Me.rbtIssue.Text = "Issue"
        Me.rbtIssue.UseVisualStyleBackColor = True
        '
        'rbtBoth
        '
        Me.rbtBoth.AutoSize = True
        Me.rbtBoth.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbtBoth.Location = New System.Drawing.Point(163, 3)
        Me.rbtBoth.Name = "rbtBoth"
        Me.rbtBoth.Size = New System.Drawing.Size(51, 17)
        Me.rbtBoth.TabIndex = 2
        Me.rbtBoth.Text = "Both"
        Me.rbtBoth.UseVisualStyleBackColor = True
        Me.rbtBoth.Visible = False
        '
        'cmbmetal_OWN
        '
        Me.cmbmetal_OWN.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbmetal_OWN.FormattingEnabled = True
        Me.cmbmetal_OWN.Location = New System.Drawing.Point(97, 65)
        Me.cmbmetal_OWN.Name = "cmbmetal_OWN"
        Me.cmbmetal_OWN.Size = New System.Drawing.Size(82, 21)
        Me.cmbmetal_OWN.TabIndex = 9
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(9, 69)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(37, 13)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "Metal"
        '
        'cmbcostcentre_OWN
        '
        Me.cmbcostcentre_OWN.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbcostcentre_OWN.FormattingEnabled = True
        Me.cmbcostcentre_OWN.Location = New System.Drawing.Point(97, 40)
        Me.cmbcostcentre_OWN.Name = "cmbcostcentre_OWN"
        Me.cmbcostcentre_OWN.Size = New System.Drawing.Size(251, 21)
        Me.cmbcostcentre_OWN.TabIndex = 7
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(9, 43)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(72, 13)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "CostCentre"
        '
        'btnnew
        '
        Me.btnnew.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnnew.Location = New System.Drawing.Point(466, 62)
        Me.btnnew.Name = "btnnew"
        Me.btnnew.Size = New System.Drawing.Size(103, 28)
        Me.btnnew.TabIndex = 11
        Me.btnnew.Text = "New [F3]"
        Me.btnnew.UseVisualStyleBackColor = True
        '
        'cmbfrmcostcentre_OWN
        '
        Me.cmbfrmcostcentre_OWN.CheckOnClick = True
        Me.cmbfrmcostcentre_OWN.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.cmbfrmcostcentre_OWN.DropDownHeight = 1
        Me.cmbfrmcostcentre_OWN.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbfrmcostcentre_OWN.FormattingEnabled = True
        Me.cmbfrmcostcentre_OWN.IntegralHeight = False
        Me.cmbfrmcostcentre_OWN.Location = New System.Drawing.Point(96, 39)
        Me.cmbfrmcostcentre_OWN.Name = "cmbfrmcostcentre_OWN"
        Me.cmbfrmcostcentre_OWN.Size = New System.Drawing.Size(251, 22)
        Me.cmbfrmcostcentre_OWN.TabIndex = 7
        Me.cmbfrmcostcentre_OWN.ValueSeparator = ", "
        '
        'btnexit
        '
        Me.btnexit.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnexit.Location = New System.Drawing.Point(777, 62)
        Me.btnexit.Name = "btnexit"
        Me.btnexit.Size = New System.Drawing.Size(103, 28)
        Me.btnexit.TabIndex = 14
        Me.btnexit.Text = "Exit[F12]"
        Me.btnexit.UseVisualStyleBackColor = True
        '
        'btnexport
        '
        Me.btnexport.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnexport.Location = New System.Drawing.Point(673, 62)
        Me.btnexport.Name = "btnexport"
        Me.btnexport.Size = New System.Drawing.Size(103, 28)
        Me.btnexport.TabIndex = 13
        Me.btnexport.Text = "Export [X]"
        Me.btnexport.UseVisualStyleBackColor = True
        '
        'Btnsearch
        '
        Me.Btnsearch.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Btnsearch.Location = New System.Drawing.Point(362, 62)
        Me.Btnsearch.Name = "Btnsearch"
        Me.Btnsearch.Size = New System.Drawing.Size(103, 28)
        Me.Btnsearch.TabIndex = 10
        Me.Btnsearch.Text = "&Search"
        Me.Btnsearch.UseVisualStyleBackColor = True
        '
        'pnlmark
        '
        Me.pnlmark.Controls.Add(Me.rbtunmark)
        Me.pnlmark.Controls.Add(Me.rbtmark)
        Me.pnlmark.Controls.Add(Me.rbtall)
        Me.pnlmark.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlmark.Location = New System.Drawing.Point(363, 16)
        Me.pnlmark.Name = "pnlmark"
        Me.pnlmark.Size = New System.Drawing.Size(212, 22)
        Me.pnlmark.TabIndex = 5
        '
        'rbtunmark
        '
        Me.rbtunmark.AutoSize = True
        Me.rbtunmark.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbtunmark.Location = New System.Drawing.Point(122, 2)
        Me.rbtunmark.Name = "rbtunmark"
        Me.rbtunmark.Size = New System.Drawing.Size(70, 17)
        Me.rbtunmark.TabIndex = 2
        Me.rbtunmark.Text = "Pending"
        Me.rbtunmark.UseVisualStyleBackColor = True
        '
        'rbtmark
        '
        Me.rbtmark.AutoSize = True
        Me.rbtmark.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbtmark.Location = New System.Drawing.Point(49, 2)
        Me.rbtmark.Name = "rbtmark"
        Me.rbtmark.Size = New System.Drawing.Size(67, 17)
        Me.rbtmark.TabIndex = 1
        Me.rbtmark.Text = "Marked"
        Me.rbtmark.UseVisualStyleBackColor = True
        '
        'rbtall
        '
        Me.rbtall.AutoSize = True
        Me.rbtall.Checked = True
        Me.rbtall.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbtall.Location = New System.Drawing.Point(7, 2)
        Me.rbtall.Name = "rbtall"
        Me.rbtall.Size = New System.Drawing.Size(39, 17)
        Me.rbtall.TabIndex = 0
        Me.rbtall.TabStop = True
        Me.rbtall.Text = "All"
        Me.rbtall.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(223, 43)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(21, 13)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "To"
        '
        'dtptodate
        '
        Me.dtptodate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtptodate.Location = New System.Drawing.Point(246, 17)
        Me.dtptodate.Mask = "##-##-####"
        Me.dtptodate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtptodate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtptodate.Name = "dtptodate"
        Me.dtptodate.Seperator = Global.Microsoft.VisualBasic.ChrW(45)
        Me.dtptodate.Size = New System.Drawing.Size(102, 21)
        Me.dtptodate.TabIndex = 4
        Me.dtptodate.Text = "08-11-1753"
        Me.dtptodate.Value = New Date(1753, 11, 8, 0, 0, 0, 0)
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(24, 19)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(67, 13)
        Me.Label7.TabIndex = 1
        Me.Label7.Text = "From Date"
        '
        'dtpfromdate
        '
        Me.dtpfromdate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpfromdate.Location = New System.Drawing.Point(97, 16)
        Me.dtpfromdate.Mask = "##-##-####"
        Me.dtpfromdate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpfromdate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpfromdate.Name = "dtpfromdate"
        Me.dtpfromdate.Seperator = Global.Microsoft.VisualBasic.ChrW(45)
        Me.dtpfromdate.Size = New System.Drawing.Size(102, 21)
        Me.dtpfromdate.TabIndex = 2
        Me.dtpfromdate.Text = "08-11-1753"
        Me.dtpfromdate.Value = New Date(1753, 11, 8, 0, 0, 0, 0)
        '
        'exportContextMenuStrip1
        '
        Me.exportContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.actualToolStripMenuItem1, Me.searchToolStripMenuItem1, Me.exportToolStripMenuItem2, Me.exitToolStripMenuItem3, Me.NewToolStripMenuItem})
        Me.exportContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.exportContextMenuStrip1.Size = New System.Drawing.Size(155, 136)
        '
        'actualToolStripMenuItem1
        '
        Me.actualToolStripMenuItem1.Name = "actualToolStripMenuItem1"
        Me.actualToolStripMenuItem1.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.G), System.Windows.Forms.Keys)
        Me.actualToolStripMenuItem1.Size = New System.Drawing.Size(154, 22)
        Me.actualToolStripMenuItem1.Text = "Generate"
        '
        'searchToolStripMenuItem1
        '
        Me.searchToolStripMenuItem1.Name = "searchToolStripMenuItem1"
        Me.searchToolStripMenuItem1.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.searchToolStripMenuItem1.Size = New System.Drawing.Size(238, 22)
        Me.searchToolStripMenuItem1.Text = "Search"
        '
        'exportToolStripMenuItem2
        '
        Me.exportToolStripMenuItem2.Name = "exportToolStripMenuItem2"
        Me.exportToolStripMenuItem2.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.X), System.Windows.Forms.Keys)
        Me.exportToolStripMenuItem2.Size = New System.Drawing.Size(154, 22)
        Me.exportToolStripMenuItem2.Text = "Export"
        '
        'exitToolStripMenuItem3
        '
        Me.exitToolStripMenuItem3.Name = "exitToolStripMenuItem3"
        Me.exitToolStripMenuItem3.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.exitToolStripMenuItem3.Size = New System.Drawing.Size(238, 22)
        Me.exitToolStripMenuItem3.Text = "Exit"
        '
        'Panel6
        '
        Me.Panel6.BackColor = System.Drawing.Color.Silver
        Me.Panel6.Controls.Add(Me.GroupBox2)
        Me.Panel6.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel6.Location = New System.Drawing.Point(0, 140)
        Me.Panel6.Name = "Panel6"
        Me.Panel6.Size = New System.Drawing.Size(1022, 588)
        Me.Panel6.TabIndex = 19
        '
        'GroupBox2
        '
        Me.GroupBox2.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox2.Controls.Add(Me.chkcheck)
        Me.GroupBox2.Controls.Add(Me.gridview)
        Me.GroupBox2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox2.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(1022, 588)
        Me.GroupBox2.TabIndex = 0
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = " "
        '
        'chkcheck
        '
        Me.chkcheck.AutoSize = True
        Me.chkcheck.BackColor = System.Drawing.SystemColors.Control
        Me.chkcheck.Location = New System.Drawing.Point(4, 18)
        Me.chkcheck.Name = "chkcheck"
        Me.chkcheck.Size = New System.Drawing.Size(15, 14)
        Me.chkcheck.TabIndex = 0
        Me.chkcheck.UseVisualStyleBackColor = False
        '
        'gridview
        '
        Me.gridview.AllowUserToAddRows = False
        Me.gridview.AllowUserToDeleteRows = False
        Me.gridview.BackgroundColor = System.Drawing.Color.Gray
        Me.gridview.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
        DataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle15.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle15.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle15.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle15.SelectionBackColor = System.Drawing.SystemColors.ControlLightLight
        DataGridViewCellStyle15.SelectionForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.gridview.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle15
        Me.gridview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridview.Cursor = System.Windows.Forms.Cursors.Default
        Me.gridview.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridview.Location = New System.Drawing.Point(3, 17)
        Me.gridview.Name = "gridview"
        Me.gridview.ReadOnly = True
        Me.gridview.RowHeadersVisible = False
        Me.gridview.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridview.Size = New System.Drawing.Size(1016, 568)
        Me.gridview.TabIndex = 0
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(154, 22)
        Me.NewToolStripMenuItem.Text = "New"
        '
        'frmIssueReceiptView
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(1022, 728)
        Me.ContextMenuStrip = Me.exportContextMenuStrip1
        Me.Controls.Add(Me.Panel6)
        Me.Controls.Add(Me.Panel5)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmIssueReceiptView"
        Me.Text = "JJ Form Report "
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.Panel5.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.pnlmark.ResumeLayout(False)
        Me.pnlmark.PerformLayout()
        Me.exportContextMenuStrip1.ResumeLayout(False)
        Me.Panel6.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        CType(Me.gridview, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel5 As System.Windows.Forms.Panel
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents dtptodate As GiriDatePicker.DatePicker
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents dtpfromdate As GiriDatePicker.DatePicker
    Friend WithEvents pnlmark As System.Windows.Forms.Panel
    Friend WithEvents rbtunmark As System.Windows.Forms.RadioButton
    Friend WithEvents rbtmark As System.Windows.Forms.RadioButton
    Friend WithEvents rbtall As System.Windows.Forms.RadioButton
    Friend WithEvents btnexit As System.Windows.Forms.Button
    Friend WithEvents btnexport As System.Windows.Forms.Button
    Friend WithEvents Btnsearch As System.Windows.Forms.Button
    Friend WithEvents exportContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents actualToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents searchToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents exportToolStripMenuItem2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents exitToolStripMenuItem3 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmbfrmcostcentre_OWN As GControl.GCheckedComboBox
    Friend WithEvents Panel6 As System.Windows.Forms.Panel
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents btnnew As System.Windows.Forms.Button
    Friend WithEvents chkcheck As System.Windows.Forms.CheckBox
    Friend WithEvents cmbcostcentre_OWN As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents rbtBoth As System.Windows.Forms.RadioButton
    Friend WithEvents rbtReceipt As System.Windows.Forms.RadioButton
    Friend WithEvents rbtIssue As System.Windows.Forms.RadioButton
    Friend WithEvents cmbmetal_OWN As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents btnGenerate As System.Windows.Forms.Button
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents PictureBox3 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox4 As System.Windows.Forms.PictureBox
    Friend WithEvents chkAsOnDate As System.Windows.Forms.CheckBox
    Friend WithEvents gridview As System.Windows.Forms.DataGridView
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txtremark1 As System.Windows.Forms.TextBox
    Friend WithEvents txtremark2 As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class
