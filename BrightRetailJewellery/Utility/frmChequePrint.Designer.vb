<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmChequePrint
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
        Me.tabMain = New System.Windows.Forms.TabControl
        Me.tabGeneral = New System.Windows.Forms.TabPage
        Me.cmbxAccode = New System.Windows.Forms.ComboBox
        Me.cmbxAcname = New System.Windows.Forms.ComboBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.txtpicheight = New System.Windows.Forms.TextBox
        Me.txtpicwidth = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblchequemm = New System.Windows.Forms.Label
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.txtwidth = New System.Windows.Forms.TextBox
        Me.txtyaxis = New System.Windows.Forms.TextBox
        Me.txtxaxis = New System.Windows.Forms.TextBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.txtdtSpace = New System.Windows.Forms.Label
        Me.cmbxDateSpace = New System.Windows.Forms.ComboBox
        Me.chkbxDateFormat = New System.Windows.Forms.ComboBox
        Me.chkbxtextline1 = New System.Windows.Forms.CheckBox
        Me.chkbxtextline2 = New System.Windows.Forms.CheckBox
        Me.chkbxtextline3 = New System.Windows.Forms.CheckBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.chkbxnotabove = New System.Windows.Forms.CheckBox
        Me.chkbxamt = New System.Windows.Forms.CheckBox
        Me.chkbxdate = New System.Windows.Forms.CheckBox
        Me.chkbxbareer = New System.Windows.Forms.CheckBox
        Me.chkbxacpayee = New System.Windows.Forms.CheckBox
        Me.chkbxpayeeline1 = New System.Windows.Forms.CheckBox
        Me.chkbxamtwords2 = New System.Windows.Forms.CheckBox
        Me.chkbxpayeeline2 = New System.Windows.Forms.CheckBox
        Me.chkbxamtwords1 = New System.Windows.Forms.CheckBox
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.txtfonteffect_underline = New System.Windows.Forms.TextBox
        Me.btn_setfont = New System.Windows.Forms.Button
        Me.txtfontstyle = New System.Windows.Forms.TextBox
        Me.txtfontsize = New System.Windows.Forms.TextBox
        Me.txtfontname = New System.Windows.Forms.TextBox
        Me.txtfonteffect_strikeout = New System.Windows.Forms.TextBox
        Me.btnSave = New System.Windows.Forms.Button
        Me.btnSelectImage = New System.Windows.Forms.Button
        Me.btnView_Search = New System.Windows.Forms.Button
        Me.lblLayoutId = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.lblLayoutName = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.txt_bearer = New System.Windows.Forms.TextBox
        Me.txt_amt = New System.Windows.Forms.TextBox
        Me.txt_notaboveamt = New System.Windows.Forms.TextBox
        Me.txt_date = New System.Windows.Forms.TextBox
        Me.txt_textline3 = New System.Windows.Forms.TextBox
        Me.txt_textline2 = New System.Windows.Forms.TextBox
        Me.txt_textline1 = New System.Windows.Forms.TextBox
        Me.txt_amtword2 = New System.Windows.Forms.TextBox
        Me.txt_amtword1 = New System.Windows.Forms.TextBox
        Me.txt_payee2 = New System.Windows.Forms.TextBox
        Me.txt_payee1 = New System.Windows.Forms.TextBox
        Me.txt_acpayee = New System.Windows.Forms.TextBox
        Me.picbxImage = New System.Windows.Forms.PictureBox
        Me.tabView = New System.Windows.Forms.TabPage
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.gridView = New System.Windows.Forms.DataGridView
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.btnBack = New System.Windows.Forms.Button
        Me.btnSearch = New System.Windows.Forms.Button
        Me.txtLayoutNameSearch = New System.Windows.Forms.TextBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.TextBox2 = New System.Windows.Forms.TextBox
        Me.FontDialog1 = New System.Windows.Forms.FontDialog
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog
        Me.tabMain.SuspendLayout()
        Me.tabGeneral.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.Panel1.SuspendLayout()
        CType(Me.picbxImage, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabView.SuspendLayout()
        Me.Panel4.SuspendLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel3.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'tabMain
        '
        Me.tabMain.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
        Me.tabMain.Controls.Add(Me.tabGeneral)
        Me.tabMain.Controls.Add(Me.tabView)
        Me.tabMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabMain.ItemSize = New System.Drawing.Size(10, 5)
        Me.tabMain.Location = New System.Drawing.Point(0, 0)
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(984, 576)
        Me.tabMain.TabIndex = 0
        '
        'tabGeneral
        '
        Me.tabGeneral.Controls.Add(Me.cmbxAccode)
        Me.tabGeneral.Controls.Add(Me.cmbxAcname)
        Me.tabGeneral.Controls.Add(Me.GroupBox1)
        Me.tabGeneral.Controls.Add(Me.GroupBox2)
        Me.tabGeneral.Controls.Add(Me.Panel2)
        Me.tabGeneral.Controls.Add(Me.btnExit)
        Me.tabGeneral.Controls.Add(Me.btnNew)
        Me.tabGeneral.Controls.Add(Me.GroupBox3)
        Me.tabGeneral.Controls.Add(Me.btnSave)
        Me.tabGeneral.Controls.Add(Me.btnSelectImage)
        Me.tabGeneral.Controls.Add(Me.btnView_Search)
        Me.tabGeneral.Controls.Add(Me.lblLayoutId)
        Me.tabGeneral.Controls.Add(Me.Label8)
        Me.tabGeneral.Controls.Add(Me.lblLayoutName)
        Me.tabGeneral.Controls.Add(Me.Label7)
        Me.tabGeneral.Controls.Add(Me.Panel1)
        Me.tabGeneral.Location = New System.Drawing.Point(4, 9)
        Me.tabGeneral.Name = "tabGeneral"
        Me.tabGeneral.Padding = New System.Windows.Forms.Padding(3)
        Me.tabGeneral.Size = New System.Drawing.Size(976, 563)
        Me.tabGeneral.TabIndex = 0
        Me.tabGeneral.Text = "General"
        Me.tabGeneral.UseVisualStyleBackColor = True
        '
        'cmbxAccode
        '
        Me.cmbxAccode.Enabled = False
        Me.cmbxAccode.FormattingEnabled = True
        Me.cmbxAccode.Location = New System.Drawing.Point(77, 36)
        Me.cmbxAccode.Name = "cmbxAccode"
        Me.cmbxAccode.Size = New System.Drawing.Size(121, 21)
        Me.cmbxAccode.TabIndex = 16
        '
        'cmbxAcname
        '
        Me.cmbxAcname.FormattingEnabled = True
        Me.cmbxAcname.Location = New System.Drawing.Point(77, 10)
        Me.cmbxAcname.Name = "cmbxAcname"
        Me.cmbxAcname.Size = New System.Drawing.Size(286, 21)
        Me.cmbxAcname.TabIndex = 15
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.txtpicheight)
        Me.GroupBox1.Controls.Add(Me.txtpicwidth)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.lblchequemm)
        Me.GroupBox1.Location = New System.Drawing.Point(797, 412)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(167, 100)
        Me.GroupBox1.TabIndex = 14
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Cheque Details"
        Me.GroupBox1.Visible = False
        '
        'txtpicheight
        '
        Me.txtpicheight.Location = New System.Drawing.Point(76, 72)
        Me.txtpicheight.Name = "txtpicheight"
        Me.txtpicheight.ReadOnly = True
        Me.txtpicheight.Size = New System.Drawing.Size(68, 21)
        Me.txtpicheight.TabIndex = 0
        Me.txtpicheight.TabStop = False
        '
        'txtpicwidth
        '
        Me.txtpicwidth.Location = New System.Drawing.Point(76, 35)
        Me.txtpicwidth.Name = "txtpicwidth"
        Me.txtpicwidth.ReadOnly = True
        Me.txtpicwidth.Size = New System.Drawing.Size(68, 21)
        Me.txtpicwidth.TabIndex = 2
        Me.txtpicwidth.TabStop = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 76)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(43, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Height"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 38)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(39, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Width"
        '
        'lblchequemm
        '
        Me.lblchequemm.Location = New System.Drawing.Point(6, 16)
        Me.lblchequemm.Name = "lblchequemm"
        Me.lblchequemm.Size = New System.Drawing.Size(95, 19)
        Me.lblchequemm.TabIndex = 0
        Me.lblchequemm.Text = "Cheque in (mm)"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.txtwidth)
        Me.GroupBox2.Controls.Add(Me.txtyaxis)
        Me.GroupBox2.Controls.Add(Me.txtxaxis)
        Me.GroupBox2.Controls.Add(Me.Label10)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Controls.Add(Me.Label5)
        Me.GroupBox2.Controls.Add(Me.Label4)
        Me.GroupBox2.Location = New System.Drawing.Point(369, 458)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(183, 97)
        Me.GroupBox2.TabIndex = 13
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Particular Details"
        '
        'txtwidth
        '
        Me.txtwidth.Location = New System.Drawing.Point(140, 35)
        Me.txtwidth.Name = "txtwidth"
        Me.txtwidth.ReadOnly = True
        Me.txtwidth.Size = New System.Drawing.Size(37, 21)
        Me.txtwidth.TabIndex = 4
        Me.txtwidth.TabStop = False
        '
        'txtyaxis
        '
        Me.txtyaxis.Location = New System.Drawing.Point(55, 73)
        Me.txtyaxis.Name = "txtyaxis"
        Me.txtyaxis.ReadOnly = True
        Me.txtyaxis.Size = New System.Drawing.Size(37, 21)
        Me.txtyaxis.TabIndex = 6
        Me.txtyaxis.TabStop = False
        '
        'txtxaxis
        '
        Me.txtxaxis.Location = New System.Drawing.Point(55, 35)
        Me.txtxaxis.Name = "txtxaxis"
        Me.txtxaxis.ReadOnly = True
        Me.txtxaxis.Size = New System.Drawing.Size(37, 21)
        Me.txtxaxis.TabIndex = 2
        Me.txtxaxis.TabStop = False
        '
        'Label10
        '
        Me.Label10.Location = New System.Drawing.Point(6, 16)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(95, 19)
        Me.Label10.TabIndex = 0
        Me.Label10.Text = "Particular in (mm)"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(5, 38)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(43, 13)
        Me.Label3.TabIndex = 1
        Me.Label3.Text = "X Axis"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(96, 38)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(39, 13)
        Me.Label5.TabIndex = 3
        Me.Label5.Text = "Width"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(7, 76)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(42, 13)
        Me.Label4.TabIndex = 5
        Me.Label4.Text = "Y Axis"
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.txtdtSpace)
        Me.Panel2.Controls.Add(Me.cmbxDateSpace)
        Me.Panel2.Controls.Add(Me.chkbxDateFormat)
        Me.Panel2.Controls.Add(Me.chkbxtextline1)
        Me.Panel2.Controls.Add(Me.chkbxtextline2)
        Me.Panel2.Controls.Add(Me.chkbxtextline3)
        Me.Panel2.Controls.Add(Me.Label6)
        Me.Panel2.Controls.Add(Me.chkbxnotabove)
        Me.Panel2.Controls.Add(Me.chkbxamt)
        Me.Panel2.Controls.Add(Me.chkbxdate)
        Me.Panel2.Controls.Add(Me.chkbxbareer)
        Me.Panel2.Controls.Add(Me.chkbxacpayee)
        Me.Panel2.Controls.Add(Me.chkbxpayeeline1)
        Me.Panel2.Controls.Add(Me.chkbxamtwords2)
        Me.Panel2.Controls.Add(Me.chkbxpayeeline2)
        Me.Panel2.Controls.Add(Me.chkbxamtwords1)
        Me.Panel2.Location = New System.Drawing.Point(772, 59)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(192, 347)
        Me.Panel2.TabIndex = 6
        '
        'txtdtSpace
        '
        Me.txtdtSpace.AutoSize = True
        Me.txtdtSpace.Location = New System.Drawing.Point(6, 326)
        Me.txtdtSpace.Name = "txtdtSpace"
        Me.txtdtSpace.Size = New System.Drawing.Size(73, 13)
        Me.txtdtSpace.TabIndex = 15
        Me.txtdtSpace.Text = "Date Space"
        '
        'cmbxDateSpace
        '
        Me.cmbxDateSpace.FormattingEnabled = True
        Me.cmbxDateSpace.Location = New System.Drawing.Point(79, 322)
        Me.cmbxDateSpace.Name = "cmbxDateSpace"
        Me.cmbxDateSpace.Size = New System.Drawing.Size(47, 21)
        Me.cmbxDateSpace.TabIndex = 14
        '
        'chkbxDateFormat
        '
        Me.chkbxDateFormat.FormattingEnabled = True
        Me.chkbxDateFormat.Location = New System.Drawing.Point(79, 273)
        Me.chkbxDateFormat.Name = "chkbxDateFormat"
        Me.chkbxDateFormat.Size = New System.Drawing.Size(90, 21)
        Me.chkbxDateFormat.TabIndex = 12
        '
        'chkbxtextline1
        '
        Me.chkbxtextline1.AutoSize = True
        Me.chkbxtextline1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkbxtextline1.Location = New System.Drawing.Point(6, 176)
        Me.chkbxtextline1.Name = "chkbxtextline1"
        Me.chkbxtextline1.Size = New System.Drawing.Size(85, 17)
        Me.chkbxtextline1.TabIndex = 7
        Me.chkbxtextline1.Text = "Text Line1"
        Me.chkbxtextline1.UseVisualStyleBackColor = True
        '
        'chkbxtextline2
        '
        Me.chkbxtextline2.AutoSize = True
        Me.chkbxtextline2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkbxtextline2.Location = New System.Drawing.Point(6, 201)
        Me.chkbxtextline2.Name = "chkbxtextline2"
        Me.chkbxtextline2.Size = New System.Drawing.Size(89, 17)
        Me.chkbxtextline2.TabIndex = 8
        Me.chkbxtextline2.Text = "Text Line 2"
        Me.chkbxtextline2.UseVisualStyleBackColor = True
        '
        'chkbxtextline3
        '
        Me.chkbxtextline3.AutoSize = True
        Me.chkbxtextline3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkbxtextline3.Location = New System.Drawing.Point(6, 226)
        Me.chkbxtextline3.Name = "chkbxtextline3"
        Me.chkbxtextline3.Size = New System.Drawing.Size(89, 17)
        Me.chkbxtextline3.TabIndex = 9
        Me.chkbxtextline3.Text = "Text Line 3"
        Me.chkbxtextline3.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label6.Location = New System.Drawing.Point(6, 5)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(79, 13)
        Me.Label6.TabIndex = 0
        Me.Label6.Text = "Select Items"
        '
        'chkbxnotabove
        '
        Me.chkbxnotabove.AutoSize = True
        Me.chkbxnotabove.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkbxnotabove.Location = New System.Drawing.Point(6, 251)
        Me.chkbxnotabove.Name = "chkbxnotabove"
        Me.chkbxnotabove.Size = New System.Drawing.Size(85, 17)
        Me.chkbxnotabove.TabIndex = 10
        Me.chkbxnotabove.Text = "Not Above"
        Me.chkbxnotabove.UseVisualStyleBackColor = True
        '
        'chkbxamt
        '
        Me.chkbxamt.AutoSize = True
        Me.chkbxamt.Checked = True
        Me.chkbxamt.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkbxamt.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkbxamt.Location = New System.Drawing.Point(6, 301)
        Me.chkbxamt.Name = "chkbxamt"
        Me.chkbxamt.Size = New System.Drawing.Size(70, 17)
        Me.chkbxamt.TabIndex = 13
        Me.chkbxamt.Text = "Amount"
        Me.chkbxamt.UseVisualStyleBackColor = True
        '
        'chkbxdate
        '
        Me.chkbxdate.AutoSize = True
        Me.chkbxdate.Checked = True
        Me.chkbxdate.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkbxdate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkbxdate.Location = New System.Drawing.Point(6, 276)
        Me.chkbxdate.Name = "chkbxdate"
        Me.chkbxdate.Size = New System.Drawing.Size(53, 17)
        Me.chkbxdate.TabIndex = 11
        Me.chkbxdate.Text = "Date"
        Me.chkbxdate.UseVisualStyleBackColor = True
        '
        'chkbxbareer
        '
        Me.chkbxbareer.AutoSize = True
        Me.chkbxbareer.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkbxbareer.Location = New System.Drawing.Point(6, 151)
        Me.chkbxbareer.Name = "chkbxbareer"
        Me.chkbxbareer.Size = New System.Drawing.Size(65, 17)
        Me.chkbxbareer.TabIndex = 6
        Me.chkbxbareer.Text = "Bareer"
        Me.chkbxbareer.UseVisualStyleBackColor = True
        '
        'chkbxacpayee
        '
        Me.chkbxacpayee.AutoSize = True
        Me.chkbxacpayee.Checked = True
        Me.chkbxacpayee.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkbxacpayee.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkbxacpayee.Location = New System.Drawing.Point(6, 26)
        Me.chkbxacpayee.Name = "chkbxacpayee"
        Me.chkbxacpayee.Size = New System.Drawing.Size(87, 17)
        Me.chkbxacpayee.TabIndex = 1
        Me.chkbxacpayee.Text = "A/C Payee"
        Me.chkbxacpayee.UseVisualStyleBackColor = True
        '
        'chkbxpayeeline1
        '
        Me.chkbxpayeeline1.AutoSize = True
        Me.chkbxpayeeline1.Checked = True
        Me.chkbxpayeeline1.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkbxpayeeline1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkbxpayeeline1.Location = New System.Drawing.Point(6, 51)
        Me.chkbxpayeeline1.Name = "chkbxpayeeline1"
        Me.chkbxpayeeline1.Size = New System.Drawing.Size(91, 17)
        Me.chkbxpayeeline1.TabIndex = 2
        Me.chkbxpayeeline1.Text = "PayeeLine1"
        Me.chkbxpayeeline1.UseVisualStyleBackColor = True
        '
        'chkbxamtwords2
        '
        Me.chkbxamtwords2.AutoSize = True
        Me.chkbxamtwords2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkbxamtwords2.Location = New System.Drawing.Point(6, 126)
        Me.chkbxamtwords2.Name = "chkbxamtwords2"
        Me.chkbxamtwords2.Size = New System.Drawing.Size(121, 17)
        Me.chkbxamtwords2.TabIndex = 5
        Me.chkbxamtwords2.Text = "Amount Words 2"
        Me.chkbxamtwords2.UseVisualStyleBackColor = True
        '
        'chkbxpayeeline2
        '
        Me.chkbxpayeeline2.AutoSize = True
        Me.chkbxpayeeline2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkbxpayeeline2.Location = New System.Drawing.Point(6, 76)
        Me.chkbxpayeeline2.Name = "chkbxpayeeline2"
        Me.chkbxpayeeline2.Size = New System.Drawing.Size(91, 17)
        Me.chkbxpayeeline2.TabIndex = 3
        Me.chkbxpayeeline2.Text = "PayeeLine2"
        Me.chkbxpayeeline2.UseVisualStyleBackColor = True
        '
        'chkbxamtwords1
        '
        Me.chkbxamtwords1.AutoSize = True
        Me.chkbxamtwords1.Checked = True
        Me.chkbxamtwords1.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkbxamtwords1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkbxamtwords1.Location = New System.Drawing.Point(6, 101)
        Me.chkbxamtwords1.Name = "chkbxamtwords1"
        Me.chkbxamtwords1.Size = New System.Drawing.Size(121, 17)
        Me.chkbxamtwords1.TabIndex = 4
        Me.chkbxamtwords1.Text = "Amount Words 1"
        Me.chkbxamtwords1.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(544, 417)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 11
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(438, 417)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 10
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.txtfonteffect_underline)
        Me.GroupBox3.Controls.Add(Me.btn_setfont)
        Me.GroupBox3.Controls.Add(Me.txtfontstyle)
        Me.GroupBox3.Controls.Add(Me.txtfontsize)
        Me.GroupBox3.Controls.Add(Me.txtfontname)
        Me.GroupBox3.Controls.Add(Me.txtfonteffect_strikeout)
        Me.GroupBox3.Location = New System.Drawing.Point(34, 453)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(262, 105)
        Me.GroupBox3.TabIndex = 12
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Font Details"
        '
        'txtfonteffect_underline
        '
        Me.txtfonteffect_underline.Location = New System.Drawing.Point(145, 46)
        Me.txtfonteffect_underline.Name = "txtfonteffect_underline"
        Me.txtfonteffect_underline.ReadOnly = True
        Me.txtfonteffect_underline.Size = New System.Drawing.Size(109, 21)
        Me.txtfonteffect_underline.TabIndex = 3
        Me.txtfonteffect_underline.TabStop = False
        Me.txtfonteffect_underline.Visible = False
        '
        'btn_setfont
        '
        Me.btn_setfont.AutoSize = True
        Me.btn_setfont.Location = New System.Drawing.Point(106, 76)
        Me.btn_setfont.Name = "btn_setfont"
        Me.btn_setfont.Size = New System.Drawing.Size(72, 29)
        Me.btn_setfont.TabIndex = 5
        Me.btn_setfont.TabStop = False
        Me.btn_setfont.Text = "Set Font"
        Me.btn_setfont.UseVisualStyleBackColor = True
        '
        'txtfontstyle
        '
        Me.txtfontstyle.Location = New System.Drawing.Point(7, 46)
        Me.txtfontstyle.Name = "txtfontstyle"
        Me.txtfontstyle.ReadOnly = True
        Me.txtfontstyle.Size = New System.Drawing.Size(109, 21)
        Me.txtfontstyle.TabIndex = 2
        Me.txtfontstyle.TabStop = False
        '
        'txtfontsize
        '
        Me.txtfontsize.Location = New System.Drawing.Point(145, 19)
        Me.txtfontsize.Name = "txtfontsize"
        Me.txtfontsize.ReadOnly = True
        Me.txtfontsize.Size = New System.Drawing.Size(109, 21)
        Me.txtfontsize.TabIndex = 1
        Me.txtfontsize.TabStop = False
        '
        'txtfontname
        '
        Me.txtfontname.Location = New System.Drawing.Point(7, 19)
        Me.txtfontname.Name = "txtfontname"
        Me.txtfontname.ReadOnly = True
        Me.txtfontname.Size = New System.Drawing.Size(109, 21)
        Me.txtfontname.TabIndex = 0
        Me.txtfontname.TabStop = False
        '
        'txtfonteffect_strikeout
        '
        Me.txtfonteffect_strikeout.Location = New System.Drawing.Point(7, 72)
        Me.txtfonteffect_strikeout.Name = "txtfonteffect_strikeout"
        Me.txtfonteffect_strikeout.ReadOnly = True
        Me.txtfonteffect_strikeout.Size = New System.Drawing.Size(70, 21)
        Me.txtfonteffect_strikeout.TabIndex = 4
        Me.txtfonteffect_strikeout.TabStop = False
        Me.txtfonteffect_strikeout.Visible = False
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(220, 417)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(100, 30)
        Me.btnSave.TabIndex = 8
        Me.btnSave.Text = "Save [F1]"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnSelectImage
        '
        Me.btnSelectImage.Location = New System.Drawing.Point(114, 416)
        Me.btnSelectImage.Name = "btnSelectImage"
        Me.btnSelectImage.Size = New System.Drawing.Size(100, 30)
        Me.btnSelectImage.TabIndex = 7
        Me.btnSelectImage.Text = "&Select Image"
        Me.btnSelectImage.UseVisualStyleBackColor = True
        '
        'btnView_Search
        '
        Me.btnView_Search.Location = New System.Drawing.Point(332, 417)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(100, 30)
        Me.btnView_Search.TabIndex = 9
        Me.btnView_Search.Text = "&View"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'lblLayoutId
        '
        Me.lblLayoutId.AutoSize = True
        Me.lblLayoutId.Location = New System.Drawing.Point(17, 39)
        Me.lblLayoutId.Name = "lblLayoutId"
        Me.lblLayoutId.Size = New System.Drawing.Size(48, 13)
        Me.lblLayoutId.TabIndex = 2
        Me.lblLayoutId.Text = "Accode"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label8.Location = New System.Drawing.Point(412, 37)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(349, 13)
        Me.Label8.TabIndex = 5
        Me.Label8.Text = "* Click on Selected Textbox Change Font Name, Size, Style"
        '
        'lblLayoutName
        '
        Me.lblLayoutName.AutoSize = True
        Me.lblLayoutName.Location = New System.Drawing.Point(17, 13)
        Me.lblLayoutName.Name = "lblLayoutName"
        Me.lblLayoutName.Size = New System.Drawing.Size(54, 13)
        Me.lblLayoutName.TabIndex = 0
        Me.lblLayoutName.Text = "AcName"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label7.Location = New System.Drawing.Point(412, 10)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(242, 13)
        Me.Label7.TabIndex = 4
        Me.Label7.Text = "* Double Click Resize And Move TextBox"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.txt_bearer)
        Me.Panel1.Controls.Add(Me.txt_amt)
        Me.Panel1.Controls.Add(Me.txt_notaboveamt)
        Me.Panel1.Controls.Add(Me.txt_date)
        Me.Panel1.Controls.Add(Me.txt_textline3)
        Me.Panel1.Controls.Add(Me.txt_textline2)
        Me.Panel1.Controls.Add(Me.txt_textline1)
        Me.Panel1.Controls.Add(Me.txt_amtword2)
        Me.Panel1.Controls.Add(Me.txt_amtword1)
        Me.Panel1.Controls.Add(Me.txt_payee2)
        Me.Panel1.Controls.Add(Me.txt_payee1)
        Me.Panel1.Controls.Add(Me.txt_acpayee)
        Me.Panel1.Controls.Add(Me.picbxImage)
        Me.Panel1.Location = New System.Drawing.Point(6, 62)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(760, 344)
        Me.Panel1.TabIndex = 2
        '
        'txt_bearer
        '
        Me.txt_bearer.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.txt_bearer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txt_bearer.Cursor = System.Windows.Forms.Cursors.SizeAll
        Me.txt_bearer.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_bearer.Location = New System.Drawing.Point(612, 93)
        Me.txt_bearer.Name = "txt_bearer"
        Me.txt_bearer.ReadOnly = True
        Me.txt_bearer.Size = New System.Drawing.Size(67, 21)
        Me.txt_bearer.TabIndex = 9
        Me.txt_bearer.TabStop = False
        Me.txt_bearer.Text = "xxxxxxxxxx"
        '
        'txt_amt
        '
        Me.txt_amt.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.txt_amt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txt_amt.Cursor = System.Windows.Forms.Cursors.SizeAll
        Me.txt_amt.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_amt.Location = New System.Drawing.Point(612, 196)
        Me.txt_amt.Name = "txt_amt"
        Me.txt_amt.ReadOnly = True
        Me.txt_amt.Size = New System.Drawing.Size(100, 21)
        Me.txt_amt.TabIndex = 10
        Me.txt_amt.TabStop = False
        Me.txt_amt.Text = "Amount"
        '
        'txt_notaboveamt
        '
        Me.txt_notaboveamt.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.txt_notaboveamt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txt_notaboveamt.Cursor = System.Windows.Forms.Cursors.SizeAll
        Me.txt_notaboveamt.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_notaboveamt.Location = New System.Drawing.Point(363, 305)
        Me.txt_notaboveamt.Name = "txt_notaboveamt"
        Me.txt_notaboveamt.ReadOnly = True
        Me.txt_notaboveamt.Size = New System.Drawing.Size(156, 21)
        Me.txt_notaboveamt.TabIndex = 11
        Me.txt_notaboveamt.TabStop = False
        Me.txt_notaboveamt.Text = "Not Above [Amount]"
        '
        'txt_date
        '
        Me.txt_date.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.txt_date.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txt_date.Cursor = System.Windows.Forms.Cursors.SizeAll
        Me.txt_date.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_date.Location = New System.Drawing.Point(604, 18)
        Me.txt_date.Name = "txt_date"
        Me.txt_date.ReadOnly = True
        Me.txt_date.Size = New System.Drawing.Size(124, 21)
        Me.txt_date.TabIndex = 8
        Me.txt_date.TabStop = False
        Me.txt_date.Text = "dd MM YYYY"
        '
        'txt_textline3
        '
        Me.txt_textline3.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.txt_textline3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txt_textline3.Cursor = System.Windows.Forms.Cursors.SizeAll
        Me.txt_textline3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_textline3.Location = New System.Drawing.Point(393, 151)
        Me.txt_textline3.Name = "txt_textline3"
        Me.txt_textline3.ReadOnly = True
        Me.txt_textline3.Size = New System.Drawing.Size(100, 21)
        Me.txt_textline3.TabIndex = 7
        Me.txt_textline3.TabStop = False
        Me.txt_textline3.Text = "Text Line3"
        '
        'txt_textline2
        '
        Me.txt_textline2.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.txt_textline2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txt_textline2.Cursor = System.Windows.Forms.Cursors.SizeAll
        Me.txt_textline2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_textline2.Location = New System.Drawing.Point(393, 104)
        Me.txt_textline2.Name = "txt_textline2"
        Me.txt_textline2.ReadOnly = True
        Me.txt_textline2.Size = New System.Drawing.Size(100, 21)
        Me.txt_textline2.TabIndex = 6
        Me.txt_textline2.TabStop = False
        Me.txt_textline2.Text = "Text Line 2"
        '
        'txt_textline1
        '
        Me.txt_textline1.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.txt_textline1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txt_textline1.Cursor = System.Windows.Forms.Cursors.SizeAll
        Me.txt_textline1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_textline1.Location = New System.Drawing.Point(393, 57)
        Me.txt_textline1.Name = "txt_textline1"
        Me.txt_textline1.ReadOnly = True
        Me.txt_textline1.Size = New System.Drawing.Size(100, 21)
        Me.txt_textline1.TabIndex = 5
        Me.txt_textline1.TabStop = False
        Me.txt_textline1.Text = "Text Line 1"
        '
        'txt_amtword2
        '
        Me.txt_amtword2.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.txt_amtword2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txt_amtword2.Cursor = System.Windows.Forms.Cursors.SizeAll
        Me.txt_amtword2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_amtword2.Location = New System.Drawing.Point(58, 220)
        Me.txt_amtword2.Name = "txt_amtword2"
        Me.txt_amtword2.ReadOnly = True
        Me.txt_amtword2.Size = New System.Drawing.Size(256, 21)
        Me.txt_amtword2.TabIndex = 9
        Me.txt_amtword2.TabStop = False
        Me.txt_amtword2.Text = "Amount In Words Line2"
        '
        'txt_amtword1
        '
        Me.txt_amtword1.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.txt_amtword1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txt_amtword1.Cursor = System.Windows.Forms.Cursors.SizeAll
        Me.txt_amtword1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_amtword1.Location = New System.Drawing.Point(58, 137)
        Me.txt_amtword1.Name = "txt_amtword1"
        Me.txt_amtword1.ReadOnly = True
        Me.txt_amtword1.Size = New System.Drawing.Size(256, 21)
        Me.txt_amtword1.TabIndex = 4
        Me.txt_amtword1.TabStop = False
        Me.txt_amtword1.Text = "Amount In Words Line1"
        '
        'txt_payee2
        '
        Me.txt_payee2.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.txt_payee2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txt_payee2.Cursor = System.Windows.Forms.Cursors.SizeAll
        Me.txt_payee2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_payee2.Location = New System.Drawing.Point(101, 177)
        Me.txt_payee2.Name = "txt_payee2"
        Me.txt_payee2.ReadOnly = True
        Me.txt_payee2.Size = New System.Drawing.Size(256, 21)
        Me.txt_payee2.TabIndex = 3
        Me.txt_payee2.TabStop = False
        Me.txt_payee2.Text = "Payee Name Line2"
        '
        'txt_payee1
        '
        Me.txt_payee1.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.txt_payee1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txt_payee1.Cursor = System.Windows.Forms.Cursors.SizeAll
        Me.txt_payee1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_payee1.Location = New System.Drawing.Point(101, 93)
        Me.txt_payee1.Name = "txt_payee1"
        Me.txt_payee1.ReadOnly = True
        Me.txt_payee1.Size = New System.Drawing.Size(256, 21)
        Me.txt_payee1.TabIndex = 2
        Me.txt_payee1.TabStop = False
        Me.txt_payee1.Text = "Payee Name Line1"
        '
        'txt_acpayee
        '
        Me.txt_acpayee.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.txt_acpayee.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txt_acpayee.Cursor = System.Windows.Forms.Cursors.SizeAll
        Me.txt_acpayee.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_acpayee.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txt_acpayee.Location = New System.Drawing.Point(9, 10)
        Me.txt_acpayee.Name = "txt_acpayee"
        Me.txt_acpayee.ReadOnly = True
        Me.txt_acpayee.Size = New System.Drawing.Size(70, 21)
        Me.txt_acpayee.TabIndex = 1
        Me.txt_acpayee.TabStop = False
        Me.txt_acpayee.Text = "A/C Payee"
        '
        'picbxImage
        '
        Me.picbxImage.BackColor = System.Drawing.Color.Gray
        Me.picbxImage.Dock = System.Windows.Forms.DockStyle.Fill
        Me.picbxImage.Location = New System.Drawing.Point(0, 0)
        Me.picbxImage.Name = "picbxImage"
        Me.picbxImage.Size = New System.Drawing.Size(760, 344)
        Me.picbxImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.picbxImage.TabIndex = 0
        Me.picbxImage.TabStop = False
        '
        'tabView
        '
        Me.tabView.Controls.Add(Me.Panel4)
        Me.tabView.Controls.Add(Me.Panel3)
        Me.tabView.Location = New System.Drawing.Point(4, 9)
        Me.tabView.Name = "tabView"
        Me.tabView.Padding = New System.Windows.Forms.Padding(3)
        Me.tabView.Size = New System.Drawing.Size(976, 563)
        Me.tabView.TabIndex = 1
        Me.tabView.Text = "View"
        Me.tabView.UseVisualStyleBackColor = True
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.gridView)
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Left
        Me.Panel4.Location = New System.Drawing.Point(3, 71)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(589, 489)
        Me.Panel4.TabIndex = 13
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView.Location = New System.Drawing.Point(0, 0)
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.RowHeadersVisible = False
        Me.gridView.Size = New System.Drawing.Size(589, 489)
        Me.gridView.TabIndex = 0
        Me.gridView.TabStop = False
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.btnBack)
        Me.Panel3.Controls.Add(Me.btnSearch)
        Me.Panel3.Controls.Add(Me.txtLayoutNameSearch)
        Me.Panel3.Controls.Add(Me.Label11)
        Me.Panel3.Controls.Add(Me.Label9)
        Me.Panel3.Controls.Add(Me.TextBox2)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel3.Location = New System.Drawing.Point(3, 3)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(970, 68)
        Me.Panel3.TabIndex = 0
        '
        'btnBack
        '
        Me.btnBack.Location = New System.Drawing.Point(399, 9)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(99, 29)
        Me.btnBack.TabIndex = 3
        Me.btnBack.Text = "&Back [Esc]"
        Me.btnBack.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(289, 9)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(99, 29)
        Me.btnSearch.TabIndex = 2
        Me.btnSearch.Text = "&Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'txtLayoutNameSearch
        '
        Me.txtLayoutNameSearch.Location = New System.Drawing.Point(105, 13)
        Me.txtLayoutNameSearch.MaxLength = 50
        Me.txtLayoutNameSearch.Name = "txtLayoutNameSearch"
        Me.txtLayoutNameSearch.Size = New System.Drawing.Size(173, 21)
        Me.txtLayoutNameSearch.TabIndex = 1
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.ForeColor = System.Drawing.Color.Black
        Me.Label11.Location = New System.Drawing.Point(16, 16)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(78, 13)
        Me.Label11.TabIndex = 0
        Me.Label11.Text = "LayoutName"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Label9.Location = New System.Drawing.Point(765, 26)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(57, 13)
        Me.Label9.TabIndex = 4
        Me.Label9.Text = "LayoutId"
        Me.Label9.Visible = False
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(849, 25)
        Me.TextBox2.MaxLength = 50
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(72, 21)
        Me.TextBox2.TabIndex = 5
        Me.TextBox2.Visible = False
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SaveToolStripMenuItem, Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(118, 70)
        '
        'SaveToolStripMenuItem
        '
        Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        Me.SaveToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
        Me.SaveToolStripMenuItem.Text = "Save"
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
        Me.NewToolStripMenuItem.Text = "New"
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "Select Image"
        Me.OpenFileDialog1.Filter = "JPEG(*.jpg)|*.jpg|Bitmap(*.bmp)|*.bmp|GIF(*.gif)|*.gif"
        '
        'frmChequePrint
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(984, 576)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.tabMain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmChequePrint"
        Me.Text = "Cheque Format"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.tabMain.ResumeLayout(False)
        Me.tabGeneral.ResumeLayout(False)
        Me.tabGeneral.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.picbxImage, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabView.ResumeLayout(False)
        Me.Panel4.ResumeLayout(False)
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents tabMain As System.Windows.Forms.TabControl
    Friend WithEvents tabGeneral As System.Windows.Forms.TabPage
    Friend WithEvents tabView As System.Windows.Forms.TabPage
    Friend WithEvents FontDialog1 As System.Windows.Forms.FontDialog
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnView_Search As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents chkbxamt As System.Windows.Forms.CheckBox
    Friend WithEvents chkbxbareer As System.Windows.Forms.CheckBox
    Friend WithEvents chkbxdate As System.Windows.Forms.CheckBox
    Friend WithEvents chkbxnotabove As System.Windows.Forms.CheckBox
    Friend WithEvents chkbxtextline3 As System.Windows.Forms.CheckBox
    Friend WithEvents chkbxtextline2 As System.Windows.Forms.CheckBox
    Friend WithEvents chkbxtextline1 As System.Windows.Forms.CheckBox
    Friend WithEvents chkbxamtwords2 As System.Windows.Forms.CheckBox
    Friend WithEvents chkbxamtwords1 As System.Windows.Forms.CheckBox
    Friend WithEvents chkbxpayeeline2 As System.Windows.Forms.CheckBox
    Friend WithEvents chkbxpayeeline1 As System.Windows.Forms.CheckBox
    Friend WithEvents chkbxacpayee As System.Windows.Forms.CheckBox
    Friend WithEvents btnSelectImage As System.Windows.Forms.Button
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents btn_setfont As System.Windows.Forms.Button
    Friend WithEvents txtfonteffect_underline As System.Windows.Forms.TextBox
    Friend WithEvents txtfontstyle As System.Windows.Forms.TextBox
    Friend WithEvents txtfontsize As System.Windows.Forms.TextBox
    Friend WithEvents txtfontname As System.Windows.Forms.TextBox
    Friend WithEvents txtfonteffect_strikeout As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents txtwidth As System.Windows.Forms.TextBox
    Friend WithEvents txtyaxis As System.Windows.Forms.TextBox
    Friend WithEvents txtxaxis As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents txtpicheight As System.Windows.Forms.TextBox
    Friend WithEvents txtpicwidth As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblchequemm As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents lblLayoutId As System.Windows.Forms.Label
    Friend WithEvents lblLayoutName As System.Windows.Forms.Label
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents txtLayoutNameSearch As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents btnBack As System.Windows.Forms.Button
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents txt_bearer As System.Windows.Forms.TextBox
    Friend WithEvents txt_amt As System.Windows.Forms.TextBox
    Friend WithEvents txt_notaboveamt As System.Windows.Forms.TextBox
    Friend WithEvents txt_date As System.Windows.Forms.TextBox
    Friend WithEvents txt_textline3 As System.Windows.Forms.TextBox
    Friend WithEvents txt_textline2 As System.Windows.Forms.TextBox
    Friend WithEvents txt_textline1 As System.Windows.Forms.TextBox
    Friend WithEvents txt_amtword2 As System.Windows.Forms.TextBox
    Friend WithEvents txt_amtword1 As System.Windows.Forms.TextBox
    Friend WithEvents txt_payee2 As System.Windows.Forms.TextBox
    Friend WithEvents txt_payee1 As System.Windows.Forms.TextBox
    Friend WithEvents txt_acpayee As System.Windows.Forms.TextBox
    Friend WithEvents picbxImage As System.Windows.Forms.PictureBox
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents chkbxDateFormat As System.Windows.Forms.ComboBox
    Friend WithEvents cmbxDateSpace As System.Windows.Forms.ComboBox
    Friend WithEvents txtdtSpace As System.Windows.Forms.Label
    Friend WithEvents cmbxAccode As System.Windows.Forms.ComboBox
    Friend WithEvents cmbxAcname As System.Windows.Forms.ComboBox
End Class
