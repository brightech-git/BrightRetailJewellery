<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmChqPrintFormat
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
        Me.grpFields = New System.Windows.Forms.GroupBox
        Me.txtTranLimit_AMT = New System.Windows.Forms.TextBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.btnOpen = New System.Windows.Forms.Button
        Me.btnSave = New System.Windows.Forms.Button
        Me.txtchqformat_NUM = New System.Windows.Forms.TextBox
        Me.txtNoOfLeafes_NUM = New System.Windows.Forms.TextBox
        Me.txtCheqNo_NUM = New System.Windows.Forms.TextBox
        Me.cmbBankName_MAN = New System.Windows.Forms.ComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.cmballig_OWN = New System.Windows.Forms.ComboBox
        Me.cmbformattype = New System.Windows.Forms.ComboBox
        Me.cmbfont = New System.Windows.Forms.ComboBox
        Me.btnEdit1 = New System.Windows.Forms.Button
        Me.chkactive = New System.Windows.Forms.CheckBox
        Me.chkisItalic = New System.Windows.Forms.CheckBox
        Me.chkIsCentre = New System.Windows.Forms.CheckBox
        Me.chkIsMedium = New System.Windows.Forms.CheckBox
        Me.chkIsUnderline = New System.Windows.Forms.CheckBox
        Me.chkIsCondenses = New System.Windows.Forms.CheckBox
        Me.chkdouble = New System.Windows.Forms.CheckBox
        Me.chkisbold = New System.Windows.Forms.CheckBox
        Me.chklblprint = New System.Windows.Forms.CheckBox
        Me.txtcolwidth_NUM = New System.Windows.Forms.TextBox
        Me.txtPrintcol_NUM = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.txtlbldesc = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.txtcolname = New System.Windows.Forms.TextBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.btnexit12 = New System.Windows.Forms.Button
        Me.btnnew1 = New System.Windows.Forms.Button
        Me.btnsave1 = New System.Windows.Forms.Button
        Me.txtPrintRow_DEC = New System.Windows.Forms.TextBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label13 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.GridView = New System.Windows.Forms.DataGridView
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ExitToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem
        Me.saveToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem
        Me.EditToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem
        Me.NewToolStripMenuItem3 = New System.Windows.Forms.ToolStripMenuItem
        Me.FontDialog1 = New System.Windows.Forms.FontDialog
        Me.grpFields.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.GridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpFields
        '
        Me.grpFields.Controls.Add(Me.txtTranLimit_AMT)
        Me.grpFields.Controls.Add(Me.Label7)
        Me.grpFields.Controls.Add(Me.btnExit)
        Me.grpFields.Controls.Add(Me.btnNew)
        Me.grpFields.Controls.Add(Me.btnOpen)
        Me.grpFields.Controls.Add(Me.btnSave)
        Me.grpFields.Controls.Add(Me.txtchqformat_NUM)
        Me.grpFields.Controls.Add(Me.txtNoOfLeafes_NUM)
        Me.grpFields.Controls.Add(Me.txtCheqNo_NUM)
        Me.grpFields.Controls.Add(Me.cmbBankName_MAN)
        Me.grpFields.Controls.Add(Me.Label3)
        Me.grpFields.Controls.Add(Me.Label8)
        Me.grpFields.Controls.Add(Me.Label2)
        Me.grpFields.Controls.Add(Me.Label1)
        Me.grpFields.Location = New System.Drawing.Point(253, 171)
        Me.grpFields.Name = "grpFields"
        Me.grpFields.Size = New System.Drawing.Size(497, 229)
        Me.grpFields.TabIndex = 0
        Me.grpFields.TabStop = False
        '
        'txtTranLimit_AMT
        '
        Me.txtTranLimit_AMT.AcceptsTab = True
        Me.txtTranLimit_AMT.Location = New System.Drawing.Point(134, 124)
        Me.txtTranLimit_AMT.MaxLength = 25
        Me.txtTranLimit_AMT.Name = "txtTranLimit_AMT"
        Me.txtTranLimit_AMT.Size = New System.Drawing.Size(213, 20)
        Me.txtTranLimit_AMT.TabIndex = 9
        Me.txtTranLimit_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label7
        '
        Me.Label7.Location = New System.Drawing.Point(28, 127)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(108, 21)
        Me.Label7.TabIndex = 8
        Me.Label7.Text = "Transaction Limit"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(349, 168)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 13
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(243, 168)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 12
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnOpen
        '
        Me.btnOpen.Location = New System.Drawing.Point(137, 168)
        Me.btnOpen.Name = "btnOpen"
        Me.btnOpen.Size = New System.Drawing.Size(100, 30)
        Me.btnOpen.TabIndex = 11
        Me.btnOpen.Text = "Open [F2]"
        Me.btnOpen.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(31, 168)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(100, 30)
        Me.btnSave.TabIndex = 10
        Me.btnSave.Text = "Save [F1]"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'txtchqformat_NUM
        '
        Me.txtchqformat_NUM.Location = New System.Drawing.Point(305, 98)
        Me.txtchqformat_NUM.MaxLength = 4
        Me.txtchqformat_NUM.Name = "txtchqformat_NUM"
        Me.txtchqformat_NUM.Size = New System.Drawing.Size(44, 20)
        Me.txtchqformat_NUM.TabIndex = 5
        Me.txtchqformat_NUM.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtNoOfLeafes_NUM
        '
        Me.txtNoOfLeafes_NUM.Location = New System.Drawing.Point(134, 99)
        Me.txtNoOfLeafes_NUM.MaxLength = 4
        Me.txtNoOfLeafes_NUM.Name = "txtNoOfLeafes_NUM"
        Me.txtNoOfLeafes_NUM.Size = New System.Drawing.Size(67, 20)
        Me.txtNoOfLeafes_NUM.TabIndex = 7
        Me.txtNoOfLeafes_NUM.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtCheqNo_NUM
        '
        Me.txtCheqNo_NUM.Location = New System.Drawing.Point(134, 72)
        Me.txtCheqNo_NUM.MaxLength = 20
        Me.txtCheqNo_NUM.Name = "txtCheqNo_NUM"
        Me.txtCheqNo_NUM.Size = New System.Drawing.Size(216, 20)
        Me.txtCheqNo_NUM.TabIndex = 3
        Me.txtCheqNo_NUM.Text = "12345678901234567890"
        '
        'cmbBankName_MAN
        '
        Me.cmbBankName_MAN.FormattingEnabled = True
        Me.cmbBankName_MAN.Location = New System.Drawing.Point(134, 45)
        Me.cmbBankName_MAN.Name = "cmbBankName_MAN"
        Me.cmbBankName_MAN.Size = New System.Drawing.Size(315, 21)
        Me.cmbBankName_MAN.TabIndex = 1
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(28, 99)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(100, 21)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "No of Leaves"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label8
        '
        Me.Label8.Location = New System.Drawing.Point(204, 99)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(95, 21)
        Me.Label8.TabIndex = 4
        Me.Label8.Text = "Cheque Format"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(28, 72)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(100, 21)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Cheque No"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(28, 45)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(100, 21)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Bank Acc Name"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.cmballig_OWN)
        Me.GroupBox1.Controls.Add(Me.cmbformattype)
        Me.GroupBox1.Controls.Add(Me.cmbfont)
        Me.GroupBox1.Controls.Add(Me.btnEdit1)
        Me.GroupBox1.Controls.Add(Me.chkactive)
        Me.GroupBox1.Controls.Add(Me.chkisItalic)
        Me.GroupBox1.Controls.Add(Me.chkIsCentre)
        Me.GroupBox1.Controls.Add(Me.chkIsMedium)
        Me.GroupBox1.Controls.Add(Me.chkIsUnderline)
        Me.GroupBox1.Controls.Add(Me.chkIsCondenses)
        Me.GroupBox1.Controls.Add(Me.chkdouble)
        Me.GroupBox1.Controls.Add(Me.chkisbold)
        Me.GroupBox1.Controls.Add(Me.chklblprint)
        Me.GroupBox1.Controls.Add(Me.txtcolwidth_NUM)
        Me.GroupBox1.Controls.Add(Me.txtPrintcol_NUM)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.txtlbldesc)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.txtcolname)
        Me.GroupBox1.Controls.Add(Me.Label11)
        Me.GroupBox1.Controls.Add(Me.btnexit12)
        Me.GroupBox1.Controls.Add(Me.btnnew1)
        Me.GroupBox1.Controls.Add(Me.btnsave1)
        Me.GroupBox1.Controls.Add(Me.txtPrintRow_DEC)
        Me.GroupBox1.Controls.Add(Me.Label9)
        Me.GroupBox1.Controls.Add(Me.Label13)
        Me.GroupBox1.Controls.Add(Me.Label12)
        Me.GroupBox1.Controls.Add(Me.Label10)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(898, 158)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        '
        'cmballig_OWN
        '
        Me.cmballig_OWN.FormattingEnabled = True
        Me.cmballig_OWN.Location = New System.Drawing.Point(638, 43)
        Me.cmballig_OWN.Name = "cmballig_OWN"
        Me.cmballig_OWN.Size = New System.Drawing.Size(85, 21)
        Me.cmballig_OWN.TabIndex = 15
        '
        'cmbformattype
        '
        Me.cmbformattype.FormattingEnabled = True
        Me.cmbformattype.Location = New System.Drawing.Point(86, 12)
        Me.cmbformattype.Name = "cmbformattype"
        Me.cmbformattype.Size = New System.Drawing.Size(73, 21)
        Me.cmbformattype.TabIndex = 1
        '
        'cmbfont
        '
        Me.cmbfont.FormattingEnabled = True
        Me.cmbfont.Location = New System.Drawing.Point(635, 15)
        Me.cmbfont.Name = "cmbfont"
        Me.cmbfont.Size = New System.Drawing.Size(202, 21)
        Me.cmbfont.TabIndex = 7
        '
        'btnEdit1
        '
        Me.btnEdit1.Location = New System.Drawing.Point(110, 106)
        Me.btnEdit1.Name = "btnEdit1"
        Me.btnEdit1.Size = New System.Drawing.Size(100, 30)
        Me.btnEdit1.TabIndex = 26
        Me.btnEdit1.Text = "Edit[F2]"
        Me.btnEdit1.UseVisualStyleBackColor = True
        '
        'chkactive
        '
        Me.chkactive.AutoSize = True
        Me.chkactive.Location = New System.Drawing.Point(619, 76)
        Me.chkactive.Name = "chkactive"
        Me.chkactive.Size = New System.Drawing.Size(56, 17)
        Me.chkactive.TabIndex = 24
        Me.chkactive.Text = "Active"
        Me.chkactive.UseVisualStyleBackColor = True
        '
        'chkisItalic
        '
        Me.chkisItalic.AutoSize = True
        Me.chkisItalic.Location = New System.Drawing.Point(538, 76)
        Me.chkisItalic.Name = "chkisItalic"
        Me.chkisItalic.Size = New System.Drawing.Size(59, 17)
        Me.chkisItalic.TabIndex = 23
        Me.chkisItalic.Text = "Is Italic"
        Me.chkisItalic.UseVisualStyleBackColor = True
        '
        'chkIsCentre
        '
        Me.chkIsCentre.AutoSize = True
        Me.chkIsCentre.Location = New System.Drawing.Point(464, 76)
        Me.chkIsCentre.Name = "chkIsCentre"
        Me.chkIsCentre.Size = New System.Drawing.Size(68, 17)
        Me.chkIsCentre.TabIndex = 22
        Me.chkIsCentre.Text = "Is Centre"
        Me.chkIsCentre.UseVisualStyleBackColor = True
        '
        'chkIsMedium
        '
        Me.chkIsMedium.AutoSize = True
        Me.chkIsMedium.Location = New System.Drawing.Point(390, 76)
        Me.chkIsMedium.Name = "chkIsMedium"
        Me.chkIsMedium.Size = New System.Drawing.Size(74, 17)
        Me.chkIsMedium.TabIndex = 21
        Me.chkIsMedium.Text = "Is Medium"
        Me.chkIsMedium.UseVisualStyleBackColor = True
        '
        'chkIsUnderline
        '
        Me.chkIsUnderline.AutoSize = True
        Me.chkIsUnderline.Location = New System.Drawing.Point(304, 76)
        Me.chkIsUnderline.Name = "chkIsUnderline"
        Me.chkIsUnderline.Size = New System.Drawing.Size(86, 17)
        Me.chkIsUnderline.TabIndex = 20
        Me.chkIsUnderline.Text = "Is UnderLine"
        Me.chkIsUnderline.UseVisualStyleBackColor = True
        '
        'chkIsCondenses
        '
        Me.chkIsCondenses.AutoSize = True
        Me.chkIsCondenses.Location = New System.Drawing.Point(218, 76)
        Me.chkIsCondenses.Name = "chkIsCondenses"
        Me.chkIsCondenses.Size = New System.Drawing.Size(85, 17)
        Me.chkIsCondenses.TabIndex = 19
        Me.chkIsCondenses.Text = "Is Condense"
        Me.chkIsCondenses.UseVisualStyleBackColor = True
        '
        'chkdouble
        '
        Me.chkdouble.AutoSize = True
        Me.chkdouble.Location = New System.Drawing.Point(146, 76)
        Me.chkdouble.Name = "chkdouble"
        Me.chkdouble.Size = New System.Drawing.Size(71, 17)
        Me.chkdouble.TabIndex = 18
        Me.chkdouble.Text = "Is Double"
        Me.chkdouble.UseVisualStyleBackColor = True
        '
        'chkisbold
        '
        Me.chkisbold.AutoSize = True
        Me.chkisbold.Location = New System.Drawing.Point(84, 76)
        Me.chkisbold.Name = "chkisbold"
        Me.chkisbold.Size = New System.Drawing.Size(58, 17)
        Me.chkisbold.TabIndex = 17
        Me.chkisbold.Text = "Is Bold"
        Me.chkisbold.UseVisualStyleBackColor = True
        '
        'chklblprint
        '
        Me.chklblprint.AutoSize = True
        Me.chklblprint.Location = New System.Drawing.Point(6, 76)
        Me.chklblprint.Name = "chklblprint"
        Me.chklblprint.Size = New System.Drawing.Size(75, 17)
        Me.chklblprint.TabIndex = 16
        Me.chklblprint.Text = "Is Lbl Print"
        Me.chklblprint.UseVisualStyleBackColor = True
        '
        'txtcolwidth_NUM
        '
        Me.txtcolwidth_NUM.Location = New System.Drawing.Point(86, 36)
        Me.txtcolwidth_NUM.Name = "txtcolwidth_NUM"
        Me.txtcolwidth_NUM.Size = New System.Drawing.Size(73, 20)
        Me.txtcolwidth_NUM.TabIndex = 9
        '
        'txtPrintcol_NUM
        '
        Me.txtPrintcol_NUM.Location = New System.Drawing.Point(231, 38)
        Me.txtPrintcol_NUM.Name = "txtPrintcol_NUM"
        Me.txtPrintcol_NUM.Size = New System.Drawing.Size(73, 20)
        Me.txtPrintcol_NUM.TabIndex = 11
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(162, 40)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(66, 13)
        Me.Label4.TabIndex = 10
        Me.Label4.Text = "Print Column"
        '
        'txtlbldesc
        '
        Me.txtlbldesc.Location = New System.Drawing.Point(384, 38)
        Me.txtlbldesc.MaxLength = 20
        Me.txtlbldesc.Name = "txtlbldesc"
        Me.txtlbldesc.Size = New System.Drawing.Size(179, 20)
        Me.txtlbldesc.TabIndex = 13
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(307, 38)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(70, 21)
        Me.Label5.TabIndex = 12
        Me.Label5.Text = "Label Descp"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(4, 35)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(80, 21)
        Me.Label6.TabIndex = 8
        Me.Label6.Text = "ColumnWidth"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtcolname
        '
        Me.txtcolname.Location = New System.Drawing.Point(384, 16)
        Me.txtcolname.Name = "txtcolname"
        Me.txtcolname.Size = New System.Drawing.Size(179, 20)
        Me.txtcolname.TabIndex = 5
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(8, 16)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(66, 13)
        Me.Label11.TabIndex = 0
        Me.Label11.Text = "Format Type"
        '
        'btnexit12
        '
        Me.btnexit12.Location = New System.Drawing.Point(321, 106)
        Me.btnexit12.Name = "btnexit12"
        Me.btnexit12.Size = New System.Drawing.Size(100, 30)
        Me.btnexit12.TabIndex = 28
        Me.btnexit12.Text = "Exit [F12]"
        Me.btnexit12.UseVisualStyleBackColor = True
        '
        'btnnew1
        '
        Me.btnnew1.Location = New System.Drawing.Point(214, 106)
        Me.btnnew1.Name = "btnnew1"
        Me.btnnew1.Size = New System.Drawing.Size(100, 30)
        Me.btnnew1.TabIndex = 27
        Me.btnnew1.Text = "New [F3]"
        Me.btnnew1.UseVisualStyleBackColor = True
        '
        'btnsave1
        '
        Me.btnsave1.Location = New System.Drawing.Point(6, 106)
        Me.btnsave1.Name = "btnsave1"
        Me.btnsave1.Size = New System.Drawing.Size(100, 30)
        Me.btnsave1.TabIndex = 25
        Me.btnsave1.Text = "Save [F1]"
        Me.btnsave1.UseVisualStyleBackColor = True
        '
        'txtPrintRow_DEC
        '
        Me.txtPrintRow_DEC.Location = New System.Drawing.Point(231, 15)
        Me.txtPrintRow_DEC.MaxLength = 20
        Me.txtPrintRow_DEC.Name = "txtPrintRow_DEC"
        Me.txtPrintRow_DEC.Size = New System.Drawing.Size(73, 20)
        Me.txtPrintRow_DEC.TabIndex = 3
        '
        'Label9
        '
        Me.Label9.Location = New System.Drawing.Point(162, 14)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(99, 21)
        Me.Label9.TabIndex = 2
        Me.Label9.Text = "Print Row"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label13
        '
        Me.Label13.Location = New System.Drawing.Point(571, 44)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(61, 21)
        Me.Label13.TabIndex = 14
        Me.Label13.Text = "Alignment"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label12
        '
        Me.Label12.Location = New System.Drawing.Point(571, 17)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(83, 21)
        Me.Label12.TabIndex = 6
        Me.Label12.Text = "Font Name"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label10
        '
        Me.Label10.Location = New System.Drawing.Point(306, 17)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(83, 21)
        Me.Label10.TabIndex = 4
        Me.Label10.Text = "Column Name"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.GroupBox1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(898, 158)
        Me.Panel1.TabIndex = 2
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.GroupBox2)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 158)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(898, 272)
        Me.Panel2.TabIndex = 3
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.GridView)
        Me.GroupBox2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox2.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(898, 272)
        Me.GroupBox2.TabIndex = 0
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = " "
        '
        'GridView
        '
        Me.GridView.AllowUserToAddRows = False
        Me.GridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GridView.Location = New System.Drawing.Point(3, 16)
        Me.GridView.Name = "GridView"
        Me.GridView.Size = New System.Drawing.Size(892, 253)
        Me.GridView.TabIndex = 0
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExitToolStripMenuItem1, Me.saveToolStripMenuItem2, Me.EditToolStripMenuItem1, Me.NewToolStripMenuItem3})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(225, 92)
        '
        'ExitToolStripMenuItem1
        '
        Me.ExitToolStripMenuItem1.Name = "ExitToolStripMenuItem1"
        Me.ExitToolStripMenuItem1.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem1.Size = New System.Drawing.Size(224, 22)
        Me.ExitToolStripMenuItem1.Text = "ExitToolStripMenuItem1"
        '
        'saveToolStripMenuItem2
        '
        Me.saveToolStripMenuItem2.Name = "saveToolStripMenuItem2"
        Me.saveToolStripMenuItem2.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.saveToolStripMenuItem2.Size = New System.Drawing.Size(224, 22)
        Me.saveToolStripMenuItem2.Text = "SaveToolStripMenuItem2"
        '
        'EditToolStripMenuItem1
        '
        Me.EditToolStripMenuItem1.Name = "EditToolStripMenuItem1"
        Me.EditToolStripMenuItem1.ShortcutKeys = System.Windows.Forms.Keys.F2
        Me.EditToolStripMenuItem1.Size = New System.Drawing.Size(224, 22)
        Me.EditToolStripMenuItem1.Text = "EditToolStripMenuItem1"
        '
        'NewToolStripMenuItem3
        '
        Me.NewToolStripMenuItem3.Name = "NewToolStripMenuItem3"
        Me.NewToolStripMenuItem3.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem3.Size = New System.Drawing.Size(224, 22)
        Me.NewToolStripMenuItem3.Text = "NewToolStripMenuItem3"
        '
        'frmChqPrintFormat
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(898, 430)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.KeyPreview = True
        Me.Name = "frmChqPrintFormat"
        Me.Text = "frmChqPrintFormat"
        Me.grpFields.ResumeLayout(False)
        Me.grpFields.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        CType(Me.GridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpFields As System.Windows.Forms.GroupBox
    Friend WithEvents txtTranLimit_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnOpen As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents txtchqformat_NUM As System.Windows.Forms.TextBox
    Friend WithEvents txtNoOfLeafes_NUM As System.Windows.Forms.TextBox
    Friend WithEvents txtCheqNo_NUM As System.Windows.Forms.TextBox
    Friend WithEvents cmbBankName_MAN As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents btnexit12 As System.Windows.Forms.Button
    Friend WithEvents btnnew1 As System.Windows.Forms.Button
    Friend WithEvents btnsave1 As System.Windows.Forms.Button
    Friend WithEvents txtPrintRow_DEC As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents txtcolname As System.Windows.Forms.TextBox
    Friend WithEvents txtcolwidth_NUM As System.Windows.Forms.TextBox
    Friend WithEvents txtPrintcol_NUM As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtlbldesc As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents chkIsCentre As System.Windows.Forms.CheckBox
    Friend WithEvents chkIsMedium As System.Windows.Forms.CheckBox
    Friend WithEvents chkIsUnderline As System.Windows.Forms.CheckBox
    Friend WithEvents chkIsCondenses As System.Windows.Forms.CheckBox
    Friend WithEvents chkdouble As System.Windows.Forms.CheckBox
    Friend WithEvents chkisbold As System.Windows.Forms.CheckBox
    Friend WithEvents chklblprint As System.Windows.Forms.CheckBox
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents GridView As System.Windows.Forms.DataGridView
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ExitToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents saveToolStripMenuItem2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem3 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents chkactive As System.Windows.Forms.CheckBox
    Friend WithEvents chkisItalic As System.Windows.Forms.CheckBox
    Friend WithEvents FontDialog1 As System.Windows.Forms.FontDialog
    Friend WithEvents EditToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnEdit1 As System.Windows.Forms.Button
    Friend WithEvents cmbfont As System.Windows.Forms.ComboBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents cmbformattype As System.Windows.Forms.ComboBox
    Friend WithEvents cmballig_OWN As System.Windows.Forms.ComboBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
End Class
