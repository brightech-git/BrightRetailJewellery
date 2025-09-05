<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MonthwiseIncentiveMaster
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
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.btnTemplate = New System.Windows.Forms.Button
        Me.btnImport = New System.Windows.Forms.Button
        Me.btnexport = New System.Windows.Forms.Button
        Me.cmbcostcentre_OWN = New System.Windows.Forms.ComboBox
        Me.Label12 = New System.Windows.Forms.Label
        Me.btnopen = New System.Windows.Forms.Button
        Me.btnSave = New System.Windows.Forms.Button
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.cmbMonth = New System.Windows.Forms.ComboBox
        Me.cmbMetal_OWN = New System.Windows.Forms.ComboBox
        Me.cmbEntryType_OWN = New System.Windows.Forms.ComboBox
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.tabmain = New System.Windows.Forms.TabControl
        Me.Tabempwiseinc = New System.Windows.Forms.TabPage
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.txt_Gridcell = New System.Windows.Forms.TextBox
        Me.dtGrid = New System.Windows.Forms.DataGridView
        Me.Tabbackendinc = New System.Windows.Forms.TabPage
        Me.Panel5 = New System.Windows.Forms.Panel
        Me.GroupBox4 = New System.Windows.Forms.GroupBox
        Me.txtGridB = New System.Windows.Forms.TextBox
        Me.gridviewB = New System.Windows.Forms.DataGridView
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.lblTo = New System.Windows.Forms.Label
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.Label10 = New System.Windows.Forms.Label
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.lbltargetmonth = New System.Windows.Forms.Label
        Me.lblexmnth = New System.Windows.Forms.Label
        Me.cmbMonthB_OWN = New System.Windows.Forms.ComboBox
        Me.cmbMonth_TarB_OWN = New System.Windows.Forms.ComboBox
        Me.btnexportB = New System.Windows.Forms.Button
        Me.btnopenB = New System.Windows.Forms.Button
        Me.Label11 = New System.Windows.Forms.Label
        Me.cmbentrytypeB_OWN = New System.Windows.Forms.ComboBox
        Me.BtnExitB = New System.Windows.Forms.Button
        Me.btnNewB = New System.Windows.Forms.Button
        Me.btnsaveB = New System.Windows.Forms.Button
        Me.Button1 = New System.Windows.Forms.Button
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Button2 = New System.Windows.Forms.Button
        Me.Button3 = New System.Windows.Forms.Button
        Me.Button4 = New System.Windows.Forms.Button
        Me.ComboBox1 = New System.Windows.Forms.ComboBox
        Me.ComboBox2 = New System.Windows.Forms.ComboBox
        Me.ComboBox3 = New System.Windows.Forms.ComboBox
        Me.Button5 = New System.Windows.Forms.Button
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.Button6 = New System.Windows.Forms.Button
        Me.Button7 = New System.Windows.Forms.Button
        Me.Button8 = New System.Windows.Forms.Button
        Me.ComboBox4 = New System.Windows.Forms.ComboBox
        Me.ComboBox5 = New System.Windows.Forms.ComboBox
        Me.ComboBox6 = New System.Windows.Forms.ComboBox
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.saveToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem
        Me.openToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem
        Me.newToolStripMenuItem3 = New System.Windows.Forms.ToolStripMenuItem
        Me.exitToolStripMenuItem4 = New System.Windows.Forms.ToolStripMenuItem
        Me.ExportToolStToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.chkflatincentive = New System.Windows.Forms.CheckBox
        Me.lbldel = New System.Windows.Forms.Label
        Me.Panel1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.tabmain.SuspendLayout()
        Me.Tabempwiseinc.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.dtGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Tabbackendinc.SuspendLayout()
        Me.Panel5.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        CType(Me.gridviewB, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel4.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.Transparent
        Me.Panel1.Controls.Add(Me.GroupBox2)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(3, 3)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(906, 94)
        Me.Panel1.TabIndex = 0
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.lbldel)
        Me.GroupBox2.Controls.Add(Me.chkflatincentive)
        Me.GroupBox2.Controls.Add(Me.btnTemplate)
        Me.GroupBox2.Controls.Add(Me.btnImport)
        Me.GroupBox2.Controls.Add(Me.btnexport)
        Me.GroupBox2.Controls.Add(Me.cmbcostcentre_OWN)
        Me.GroupBox2.Controls.Add(Me.Label12)
        Me.GroupBox2.Controls.Add(Me.btnopen)
        Me.GroupBox2.Controls.Add(Me.btnSave)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Controls.Add(Me.btnExit)
        Me.GroupBox2.Controls.Add(Me.btnNew)
        Me.GroupBox2.Controls.Add(Me.cmbMonth)
        Me.GroupBox2.Controls.Add(Me.cmbMetal_OWN)
        Me.GroupBox2.Controls.Add(Me.cmbEntryType_OWN)
        Me.GroupBox2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox2.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(906, 94)
        Me.GroupBox2.TabIndex = 0
        Me.GroupBox2.TabStop = False
        '
        'btnTemplate
        '
        Me.btnTemplate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnTemplate.Location = New System.Drawing.Point(430, 58)
        Me.btnTemplate.Name = "btnTemplate"
        Me.btnTemplate.Size = New System.Drawing.Size(80, 30)
        Me.btnTemplate.TabIndex = 12
        Me.btnTemplate.Text = "Template"
        Me.btnTemplate.UseVisualStyleBackColor = True
        '
        'btnImport
        '
        Me.btnImport.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnImport.Location = New System.Drawing.Point(509, 58)
        Me.btnImport.Name = "btnImport"
        Me.btnImport.Size = New System.Drawing.Size(80, 30)
        Me.btnImport.TabIndex = 13
        Me.btnImport.Text = "Import"
        Me.btnImport.UseVisualStyleBackColor = True
        '
        'btnexport
        '
        Me.btnexport.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnexport.Location = New System.Drawing.Point(588, 58)
        Me.btnexport.Name = "btnexport"
        Me.btnexport.Size = New System.Drawing.Size(78, 30)
        Me.btnexport.TabIndex = 14
        Me.btnexport.Text = "Export[X]"
        Me.btnexport.UseVisualStyleBackColor = True
        '
        'cmbcostcentre_OWN
        '
        Me.cmbcostcentre_OWN.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbcostcentre_OWN.FormattingEnabled = True
        Me.cmbcostcentre_OWN.Location = New System.Drawing.Point(232, 34)
        Me.cmbcostcentre_OWN.Name = "cmbcostcentre_OWN"
        Me.cmbcostcentre_OWN.Size = New System.Drawing.Size(176, 21)
        Me.cmbcostcentre_OWN.TabIndex = 4
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(163, 37)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(69, 13)
        Me.Label12.TabIndex = 3
        Me.Label12.Text = "Costcentre"
        '
        'btnopen
        '
        Me.btnopen.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnopen.Location = New System.Drawing.Point(163, 58)
        Me.btnopen.Name = "btnopen"
        Me.btnopen.Size = New System.Drawing.Size(90, 30)
        Me.btnopen.TabIndex = 9
        Me.btnopen.Text = "Open[F2]"
        Me.btnopen.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.Location = New System.Drawing.Point(252, 58)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(90, 30)
        Me.btnSave.TabIndex = 10
        Me.btnSave.Text = "Save[F1]"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(413, 37)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(41, 13)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "Month"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(412, 16)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(37, 13)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Metal"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(163, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(69, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Entry Type"
        '
        'btnExit
        '
        Me.btnExit.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExit.Location = New System.Drawing.Point(665, 58)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(78, 30)
        Me.btnExit.TabIndex = 15
        Me.btnExit.Text = "Exit[F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNew.Location = New System.Drawing.Point(341, 58)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(90, 30)
        Me.btnNew.TabIndex = 11
        Me.btnNew.Text = "New[F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'cmbMonth
        '
        Me.cmbMonth.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbMonth.FormattingEnabled = True
        Me.cmbMonth.Location = New System.Drawing.Point(454, 34)
        Me.cmbMonth.Name = "cmbMonth"
        Me.cmbMonth.Size = New System.Drawing.Size(176, 21)
        Me.cmbMonth.TabIndex = 8
        '
        'cmbMetal_OWN
        '
        Me.cmbMetal_OWN.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbMetal_OWN.FormattingEnabled = True
        Me.cmbMetal_OWN.Location = New System.Drawing.Point(454, 10)
        Me.cmbMetal_OWN.Name = "cmbMetal_OWN"
        Me.cmbMetal_OWN.Size = New System.Drawing.Size(176, 21)
        Me.cmbMetal_OWN.TabIndex = 6
        '
        'cmbEntryType_OWN
        '
        Me.cmbEntryType_OWN.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbEntryType_OWN.FormattingEnabled = True
        Me.cmbEntryType_OWN.Items.AddRange(New Object() {"INSERT", "UPDATE"})
        Me.cmbEntryType_OWN.Location = New System.Drawing.Point(232, 10)
        Me.cmbEntryType_OWN.Name = "cmbEntryType_OWN"
        Me.cmbEntryType_OWN.Size = New System.Drawing.Size(176, 21)
        Me.cmbEntryType_OWN.TabIndex = 2
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.tabmain)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(920, 438)
        Me.Panel2.TabIndex = 1
        '
        'tabmain
        '
        Me.tabmain.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
        Me.tabmain.Controls.Add(Me.Tabempwiseinc)
        Me.tabmain.Controls.Add(Me.Tabbackendinc)
        Me.tabmain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabmain.Location = New System.Drawing.Point(0, 0)
        Me.tabmain.Name = "tabmain"
        Me.tabmain.SelectedIndex = 0
        Me.tabmain.Size = New System.Drawing.Size(920, 438)
        Me.tabmain.TabIndex = 1
        '
        'Tabempwiseinc
        '
        Me.Tabempwiseinc.Controls.Add(Me.Panel3)
        Me.Tabempwiseinc.Controls.Add(Me.Panel1)
        Me.Tabempwiseinc.Location = New System.Drawing.Point(4, 25)
        Me.Tabempwiseinc.Name = "Tabempwiseinc"
        Me.Tabempwiseinc.Padding = New System.Windows.Forms.Padding(3)
        Me.Tabempwiseinc.Size = New System.Drawing.Size(912, 409)
        Me.Tabempwiseinc.TabIndex = 0
        Me.Tabempwiseinc.Text = "Tabempwiseinc"
        Me.Tabempwiseinc.UseVisualStyleBackColor = True
        '
        'Panel3
        '
        Me.Panel3.BackColor = System.Drawing.Color.Transparent
        Me.Panel3.Controls.Add(Me.GroupBox1)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel3.Location = New System.Drawing.Point(3, 97)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(906, 309)
        Me.Panel3.TabIndex = 1
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.txt_Gridcell)
        Me.GroupBox1.Controls.Add(Me.dtGrid)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(906, 309)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        '
        'txt_Gridcell
        '
        Me.txt_Gridcell.Font = New System.Drawing.Font("Microsoft Sans Serif", 13.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_Gridcell.Location = New System.Drawing.Point(273, 205)
        Me.txt_Gridcell.Name = "txt_Gridcell"
        Me.txt_Gridcell.Size = New System.Drawing.Size(100, 27)
        Me.txt_Gridcell.TabIndex = 1
        Me.txt_Gridcell.Visible = False
        '
        'dtGrid
        '
        Me.dtGrid.AllowUserToAddRows = False
        Me.dtGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dtGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dtGrid.Location = New System.Drawing.Point(3, 16)
        Me.dtGrid.Name = "dtGrid"
        Me.dtGrid.RowHeadersVisible = False
        Me.dtGrid.Size = New System.Drawing.Size(900, 290)
        Me.dtGrid.TabIndex = 0
        '
        'Tabbackendinc
        '
        Me.Tabbackendinc.Controls.Add(Me.Panel5)
        Me.Tabbackendinc.Controls.Add(Me.Panel4)
        Me.Tabbackendinc.Location = New System.Drawing.Point(4, 25)
        Me.Tabbackendinc.Name = "Tabbackendinc"
        Me.Tabbackendinc.Padding = New System.Windows.Forms.Padding(3)
        Me.Tabbackendinc.Size = New System.Drawing.Size(912, 409)
        Me.Tabbackendinc.TabIndex = 1
        Me.Tabbackendinc.Text = "Tabbackendinc"
        Me.Tabbackendinc.UseVisualStyleBackColor = True
        '
        'Panel5
        '
        Me.Panel5.BackColor = System.Drawing.Color.Transparent
        Me.Panel5.Controls.Add(Me.GroupBox4)
        Me.Panel5.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel5.Location = New System.Drawing.Point(3, 100)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(906, 306)
        Me.Panel5.TabIndex = 1
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.txtGridB)
        Me.GroupBox4.Controls.Add(Me.gridviewB)
        Me.GroupBox4.Controls.Add(Me.dtpTo)
        Me.GroupBox4.Controls.Add(Me.lblTo)
        Me.GroupBox4.Controls.Add(Me.dtpFrom)
        Me.GroupBox4.Controls.Add(Me.Label10)
        Me.GroupBox4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox4.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(906, 306)
        Me.GroupBox4.TabIndex = 0
        Me.GroupBox4.TabStop = False
        '
        'txtGridB
        '
        Me.txtGridB.Font = New System.Drawing.Font("Microsoft Sans Serif", 13.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtGridB.Location = New System.Drawing.Point(252, 138)
        Me.txtGridB.Name = "txtGridB"
        Me.txtGridB.Size = New System.Drawing.Size(100, 27)
        Me.txtGridB.TabIndex = 1
        Me.txtGridB.Visible = False
        '
        'gridviewB
        '
        Me.gridviewB.AllowUserToAddRows = False
        Me.gridviewB.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridviewB.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridviewB.Location = New System.Drawing.Point(3, 16)
        Me.gridviewB.Name = "gridviewB"
        Me.gridviewB.Size = New System.Drawing.Size(900, 287)
        Me.gridviewB.TabIndex = 0
        '
        'dtpTo
        '
        Me.dtpTo.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpTo.Location = New System.Drawing.Point(831, 326)
        Me.dtpTo.Mask = "##/##/####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(72, 21)
        Me.dtpTo.TabIndex = 0
        Me.dtpTo.Text = "07/03/9998"
        Me.dtpTo.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        Me.dtpTo.Visible = False
        '
        'lblTo
        '
        Me.lblTo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTo.Location = New System.Drawing.Point(812, 326)
        Me.lblTo.Name = "lblTo"
        Me.lblTo.Size = New System.Drawing.Size(24, 21)
        Me.lblTo.TabIndex = 14
        Me.lblTo.Text = "To"
        Me.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblTo.Visible = False
        '
        'dtpFrom
        '
        Me.dtpFrom.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpFrom.Location = New System.Drawing.Point(741, 326)
        Me.dtpFrom.Mask = "##/##/####"
        Me.dtpFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom.Size = New System.Drawing.Size(72, 21)
        Me.dtpFrom.TabIndex = 13
        Me.dtpFrom.Text = "07/03/9998"
        Me.dtpFrom.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        Me.dtpFrom.Visible = False
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(668, 325)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(36, 13)
        Me.Label10.TabIndex = 12
        Me.Label10.Text = "From"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label10.Visible = False
        '
        'Panel4
        '
        Me.Panel4.BackColor = System.Drawing.Color.Transparent
        Me.Panel4.Controls.Add(Me.GroupBox3)
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel4.Location = New System.Drawing.Point(3, 3)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(906, 97)
        Me.Panel4.TabIndex = 0
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.lbltargetmonth)
        Me.GroupBox3.Controls.Add(Me.lblexmnth)
        Me.GroupBox3.Controls.Add(Me.cmbMonthB_OWN)
        Me.GroupBox3.Controls.Add(Me.cmbMonth_TarB_OWN)
        Me.GroupBox3.Controls.Add(Me.btnexportB)
        Me.GroupBox3.Controls.Add(Me.btnopenB)
        Me.GroupBox3.Controls.Add(Me.Label11)
        Me.GroupBox3.Controls.Add(Me.cmbentrytypeB_OWN)
        Me.GroupBox3.Controls.Add(Me.BtnExitB)
        Me.GroupBox3.Controls.Add(Me.btnNewB)
        Me.GroupBox3.Controls.Add(Me.btnsaveB)
        Me.GroupBox3.Location = New System.Drawing.Point(2, 3)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(906, 94)
        Me.GroupBox3.TabIndex = 0
        Me.GroupBox3.TabStop = False
        '
        'lbltargetmonth
        '
        Me.lbltargetmonth.AutoSize = True
        Me.lbltargetmonth.Enabled = False
        Me.lbltargetmonth.Location = New System.Drawing.Point(72, 61)
        Me.lbltargetmonth.Name = "lbltargetmonth"
        Me.lbltargetmonth.Size = New System.Drawing.Size(71, 13)
        Me.lbltargetmonth.TabIndex = 5
        Me.lbltargetmonth.Text = "Target Month"
        '
        'lblexmnth
        '
        Me.lblexmnth.AutoSize = True
        Me.lblexmnth.Location = New System.Drawing.Point(75, 38)
        Me.lblexmnth.Name = "lblexmnth"
        Me.lblexmnth.Size = New System.Drawing.Size(52, 13)
        Me.lblexmnth.TabIndex = 3
        Me.lblexmnth.Text = "Ex-Month"
        '
        'cmbMonthB_OWN
        '
        Me.cmbMonthB_OWN.FormattingEnabled = True
        Me.cmbMonthB_OWN.Location = New System.Drawing.Point(144, 37)
        Me.cmbMonthB_OWN.Name = "cmbMonthB_OWN"
        Me.cmbMonthB_OWN.Size = New System.Drawing.Size(121, 21)
        Me.cmbMonthB_OWN.TabIndex = 4
        '
        'cmbMonth_TarB_OWN
        '
        Me.cmbMonth_TarB_OWN.Enabled = False
        Me.cmbMonth_TarB_OWN.FormattingEnabled = True
        Me.cmbMonth_TarB_OWN.Location = New System.Drawing.Point(144, 60)
        Me.cmbMonth_TarB_OWN.Name = "cmbMonth_TarB_OWN"
        Me.cmbMonth_TarB_OWN.Size = New System.Drawing.Size(121, 21)
        Me.cmbMonth_TarB_OWN.TabIndex = 6
        '
        'btnexportB
        '
        Me.btnexportB.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnexportB.Location = New System.Drawing.Point(561, 31)
        Me.btnexportB.Name = "btnexportB"
        Me.btnexportB.Size = New System.Drawing.Size(92, 30)
        Me.btnexportB.TabIndex = 10
        Me.btnexportB.Text = "Export[x]"
        Me.btnexportB.UseVisualStyleBackColor = True
        '
        'btnopenB
        '
        Me.btnopenB.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnopenB.Location = New System.Drawing.Point(286, 31)
        Me.btnopenB.Name = "btnopenB"
        Me.btnopenB.Size = New System.Drawing.Size(92, 30)
        Me.btnopenB.TabIndex = 7
        Me.btnopenB.Text = "Open[F2]"
        Me.btnopenB.UseVisualStyleBackColor = True
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(75, 14)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(69, 13)
        Me.Label11.TabIndex = 1
        Me.Label11.Text = "Entry Type"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbentrytypeB_OWN
        '
        Me.cmbentrytypeB_OWN.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbentrytypeB_OWN.FormattingEnabled = True
        Me.cmbentrytypeB_OWN.Items.AddRange(New Object() {"INSERT", "UPDATE", "EXTENSION"})
        Me.cmbentrytypeB_OWN.Location = New System.Drawing.Point(144, 14)
        Me.cmbentrytypeB_OWN.Name = "cmbentrytypeB_OWN"
        Me.cmbentrytypeB_OWN.Size = New System.Drawing.Size(121, 21)
        Me.cmbentrytypeB_OWN.TabIndex = 2
        '
        'BtnExitB
        '
        Me.BtnExitB.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnExitB.Location = New System.Drawing.Point(652, 31)
        Me.BtnExitB.Name = "BtnExitB"
        Me.BtnExitB.Size = New System.Drawing.Size(92, 30)
        Me.BtnExitB.TabIndex = 11
        Me.BtnExitB.Text = "Exit[F12]"
        Me.BtnExitB.UseVisualStyleBackColor = True
        '
        'btnNewB
        '
        Me.btnNewB.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNewB.Location = New System.Drawing.Point(468, 31)
        Me.btnNewB.Name = "btnNewB"
        Me.btnNewB.Size = New System.Drawing.Size(94, 30)
        Me.btnNewB.TabIndex = 9
        Me.btnNewB.Text = "New[F3]"
        Me.btnNewB.UseVisualStyleBackColor = True
        '
        'btnsaveB
        '
        Me.btnsaveB.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnsaveB.Location = New System.Drawing.Point(377, 31)
        Me.btnsaveB.Name = "btnsaveB"
        Me.btnsaveB.Size = New System.Drawing.Size(92, 30)
        Me.btnsaveB.TabIndex = 8
        Me.btnsaveB.Text = "Save[F1]"
        Me.btnsaveB.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button1.Location = New System.Drawing.Point(350, 24)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(80, 38)
        Me.Button1.TabIndex = 7
        Me.Button1.Text = "Save"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(7, 60)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(42, 14)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "Month"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(7, 38)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(37, 14)
        Me.Label5.TabIndex = 2
        Me.Label5.Text = "Metal"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(6, 16)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(65, 14)
        Me.Label6.TabIndex = 0
        Me.Label6.Text = "Entry Type"
        '
        'Button2
        '
        Me.Button2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button2.Location = New System.Drawing.Point(506, 24)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(80, 38)
        Me.Button2.TabIndex = 10
        Me.Button2.Text = "Exit"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button3.Location = New System.Drawing.Point(428, 24)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(80, 38)
        Me.Button3.TabIndex = 8
        Me.Button3.Text = "New"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Button4
        '
        Me.Button4.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button4.Location = New System.Drawing.Point(272, 24)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(80, 38)
        Me.Button4.TabIndex = 6
        Me.Button4.Text = "View"
        Me.Button4.UseVisualStyleBackColor = True
        '
        'ComboBox1
        '
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Location = New System.Drawing.Point(71, 57)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(199, 21)
        Me.ComboBox1.TabIndex = 5
        '
        'ComboBox2
        '
        Me.ComboBox2.FormattingEnabled = True
        Me.ComboBox2.Location = New System.Drawing.Point(71, 35)
        Me.ComboBox2.Name = "ComboBox2"
        Me.ComboBox2.Size = New System.Drawing.Size(199, 21)
        Me.ComboBox2.TabIndex = 3
        '
        'ComboBox3
        '
        Me.ComboBox3.FormattingEnabled = True
        Me.ComboBox3.Items.AddRange(New Object() {"INSERT", "UPDATE"})
        Me.ComboBox3.Location = New System.Drawing.Point(71, 13)
        Me.ComboBox3.Name = "ComboBox3"
        Me.ComboBox3.Size = New System.Drawing.Size(199, 21)
        Me.ComboBox3.TabIndex = 1
        '
        'Button5
        '
        Me.Button5.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button5.Location = New System.Drawing.Point(350, 24)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(80, 38)
        Me.Button5.TabIndex = 7
        Me.Button5.Text = "Save"
        Me.Button5.UseVisualStyleBackColor = True
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(7, 60)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(42, 14)
        Me.Label7.TabIndex = 4
        Me.Label7.Text = "Month"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(7, 38)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(37, 14)
        Me.Label8.TabIndex = 2
        Me.Label8.Text = "Metal"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(6, 16)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(65, 14)
        Me.Label9.TabIndex = 0
        Me.Label9.Text = "Entry Type"
        '
        'Button6
        '
        Me.Button6.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button6.Location = New System.Drawing.Point(506, 24)
        Me.Button6.Name = "Button6"
        Me.Button6.Size = New System.Drawing.Size(80, 38)
        Me.Button6.TabIndex = 10
        Me.Button6.Text = "Exit"
        Me.Button6.UseVisualStyleBackColor = True
        '
        'Button7
        '
        Me.Button7.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button7.Location = New System.Drawing.Point(428, 24)
        Me.Button7.Name = "Button7"
        Me.Button7.Size = New System.Drawing.Size(80, 38)
        Me.Button7.TabIndex = 8
        Me.Button7.Text = "New"
        Me.Button7.UseVisualStyleBackColor = True
        '
        'Button8
        '
        Me.Button8.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button8.Location = New System.Drawing.Point(272, 24)
        Me.Button8.Name = "Button8"
        Me.Button8.Size = New System.Drawing.Size(80, 38)
        Me.Button8.TabIndex = 6
        Me.Button8.Text = "View"
        Me.Button8.UseVisualStyleBackColor = True
        '
        'ComboBox4
        '
        Me.ComboBox4.FormattingEnabled = True
        Me.ComboBox4.Location = New System.Drawing.Point(71, 57)
        Me.ComboBox4.Name = "ComboBox4"
        Me.ComboBox4.Size = New System.Drawing.Size(199, 21)
        Me.ComboBox4.TabIndex = 5
        '
        'ComboBox5
        '
        Me.ComboBox5.FormattingEnabled = True
        Me.ComboBox5.Location = New System.Drawing.Point(71, 35)
        Me.ComboBox5.Name = "ComboBox5"
        Me.ComboBox5.Size = New System.Drawing.Size(199, 21)
        Me.ComboBox5.TabIndex = 3
        '
        'ComboBox6
        '
        Me.ComboBox6.FormattingEnabled = True
        Me.ComboBox6.Items.AddRange(New Object() {"INSERT", "UPDATE"})
        Me.ComboBox6.Location = New System.Drawing.Point(71, 13)
        Me.ComboBox6.Name = "ComboBox6"
        Me.ComboBox6.Size = New System.Drawing.Size(199, 21)
        Me.ComboBox6.TabIndex = 1
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.saveToolStripMenuItem1, Me.openToolStripMenuItem2, Me.newToolStripMenuItem3, Me.exitToolStripMenuItem4, Me.ExportToolStToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(225, 114)
        '
        'saveToolStripMenuItem1
        '
        Me.saveToolStripMenuItem1.Name = "saveToolStripMenuItem1"
        Me.saveToolStripMenuItem1.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.saveToolStripMenuItem1.Size = New System.Drawing.Size(224, 22)
        Me.saveToolStripMenuItem1.Text = "saveToolStripMenuItem1"
        '
        'openToolStripMenuItem2
        '
        Me.openToolStripMenuItem2.Name = "openToolStripMenuItem2"
        Me.openToolStripMenuItem2.ShortcutKeys = System.Windows.Forms.Keys.F2
        Me.openToolStripMenuItem2.Size = New System.Drawing.Size(224, 22)
        Me.openToolStripMenuItem2.Text = "openToolStripMenuItem2"
        '
        'newToolStripMenuItem3
        '
        Me.newToolStripMenuItem3.Name = "newToolStripMenuItem3"
        Me.newToolStripMenuItem3.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.newToolStripMenuItem3.Size = New System.Drawing.Size(224, 22)
        Me.newToolStripMenuItem3.Text = "newToolStripMenuItem3"
        '
        'exitToolStripMenuItem4
        '
        Me.exitToolStripMenuItem4.Name = "exitToolStripMenuItem4"
        Me.exitToolStripMenuItem4.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.exitToolStripMenuItem4.Size = New System.Drawing.Size(224, 22)
        Me.exitToolStripMenuItem4.Text = "exitToolStripMenuItem4"
        '
        'ExportToolStToolStripMenuItem
        '
        Me.ExportToolStToolStripMenuItem.Name = "ExportToolStToolStripMenuItem"
        Me.ExportToolStToolStripMenuItem.Size = New System.Drawing.Size(224, 22)
        Me.ExportToolStToolStripMenuItem.Text = "ExportToolStrip"
        '
        'chkflatincentive
        '
        Me.chkflatincentive.AutoSize = True
        Me.chkflatincentive.Location = New System.Drawing.Point(3, 10)
        Me.chkflatincentive.Name = "chkflatincentive"
        Me.chkflatincentive.Size = New System.Drawing.Size(161, 17)
        Me.chkflatincentive.TabIndex = 0
        Me.chkflatincentive.Text = "Flat Incentive For Showroom"
        Me.chkflatincentive.UseVisualStyleBackColor = True
        '
        'lbldel
        '
        Me.lbldel.AutoSize = True
        Me.lbldel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbldel.ForeColor = System.Drawing.Color.Red
        Me.lbldel.Location = New System.Drawing.Point(749, 72)
        Me.lbldel.Name = "lbldel"
        Me.lbldel.Size = New System.Drawing.Size(129, 13)
        Me.lbldel.TabIndex = 16
        Me.lbldel.Text = "Press [Del] for Delete"
        '
        'MonthwiseIncentiveMaster
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.DarkGray
        Me.ClientSize = New System.Drawing.Size(920, 438)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.Panel2)
        Me.KeyPreview = True
        Me.Name = "MonthwiseIncentiveMaster"
        Me.Text = "Monthwise Incentive"
        Me.Panel1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.tabmain.ResumeLayout(False)
        Me.Tabempwiseinc.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.dtGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Tabbackendinc.ResumeLayout(False)
        Me.Panel5.ResumeLayout(False)
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        CType(Me.gridviewB, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel4.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents dtGrid As System.Windows.Forms.DataGridView
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents cmbMonth As System.Windows.Forms.ComboBox
    Friend WithEvents cmbMetal_OWN As System.Windows.Forms.ComboBox
    Friend WithEvents cmbEntryType_OWN As System.Windows.Forms.ComboBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txt_Gridcell As System.Windows.Forms.TextBox
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents tabmain As System.Windows.Forms.TabControl
    Friend WithEvents Tabempwiseinc As System.Windows.Forms.TabPage
    Friend WithEvents Tabbackendinc As System.Windows.Forms.TabPage
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents ComboBox2 As System.Windows.Forms.ComboBox
    Friend WithEvents ComboBox3 As System.Windows.Forms.ComboBox
    Friend WithEvents Panel5 As System.Windows.Forms.Panel
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents gridviewB As System.Windows.Forms.DataGridView
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents Button5 As System.Windows.Forms.Button
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Button6 As System.Windows.Forms.Button
    Friend WithEvents Button7 As System.Windows.Forms.Button
    Friend WithEvents Button8 As System.Windows.Forms.Button
    Friend WithEvents ComboBox4 As System.Windows.Forms.ComboBox
    Friend WithEvents ComboBox5 As System.Windows.Forms.ComboBox
    Friend WithEvents ComboBox6 As System.Windows.Forms.ComboBox
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents lblTo As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents txtGridB As System.Windows.Forms.TextBox
    Friend WithEvents BtnExitB As System.Windows.Forms.Button
    Friend WithEvents btnNewB As System.Windows.Forms.Button
    Friend WithEvents btnsaveB As System.Windows.Forms.Button
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents cmbentrytypeB_OWN As System.Windows.Forms.ComboBox
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents saveToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents openToolStripMenuItem2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents newToolStripMenuItem3 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents exitToolStripMenuItem4 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnopenB As System.Windows.Forms.Button
    Friend WithEvents btnopen As System.Windows.Forms.Button
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents cmbcostcentre_OWN As System.Windows.Forms.ComboBox
    Friend WithEvents ExportToolStToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnexportB As System.Windows.Forms.Button
    Friend WithEvents lblexmnth As System.Windows.Forms.Label
    Friend WithEvents cmbMonthB_OWN As System.Windows.Forms.ComboBox
    Friend WithEvents cmbMonth_TarB_OWN As System.Windows.Forms.ComboBox
    Friend WithEvents lbltargetmonth As System.Windows.Forms.Label
    Friend WithEvents btnImport As System.Windows.Forms.Button
    Friend WithEvents btnTemplate As System.Windows.Forms.Button
    Friend WithEvents btnexport As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents chkflatincentive As System.Windows.Forms.CheckBox
    Friend WithEvents lbldel As System.Windows.Forms.Label
End Class
