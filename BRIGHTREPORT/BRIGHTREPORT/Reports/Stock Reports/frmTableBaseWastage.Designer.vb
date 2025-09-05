<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTableBaseWastage
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
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.searchToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem
        Me.newToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem
        Me.exitToolStripMenuItem3 = New System.Windows.Forms.ToolStripMenuItem
        Me.exportToolStripMenuItem4 = New System.Windows.Forms.ToolStripMenuItem
        Me.printToolStripMenuItem5 = New System.Windows.Forms.ToolStripMenuItem
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.btnExport = New System.Windows.Forms.Button
        Me.Label5 = New System.Windows.Forms.Label
        Me.btnPrint = New System.Windows.Forms.Button
        Me.Panel5 = New System.Windows.Forms.Panel
        Me.rbtwast = New System.Windows.Forms.RadioButton
        Me.rbttable = New System.Windows.Forms.RadioButton
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.rbtSummary = New System.Windows.Forms.RadioButton
        Me.rbtDetailed = New System.Windows.Forms.RadioButton
        Me.btnView_Search = New System.Windows.Forms.Button
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.gridFlag = New System.Windows.Forms.DataGridView
        Me.lblTitle = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.chkCmbItemCounter = New BrighttechPack.CheckedComboBox
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.ChkcmbCostcentre = New BrighttechPack.CheckedComboBox
        Me.Chkcmbmetal = New BrighttechPack.CheckedComboBox
        Me.chklsttablecode = New BrighttechPack.CheckedComboBox
        Me.chklstsubitem = New BrighttechPack.CheckedComboBox
        Me.chkcmbitemname = New BrighttechPack.CheckedComboBox
        Me.ContextMenuStrip1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.Panel5.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.gridFlag, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.searchToolStripMenuItem1, Me.newToolStripMenuItem2, Me.exitToolStripMenuItem3, Me.exportToolStripMenuItem4, Me.printToolStripMenuItem5})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(273, 114)
        '
        'searchToolStripMenuItem1
        '
        Me.searchToolStripMenuItem1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.searchToolStripMenuItem1.Name = "searchToolStripMenuItem1"
        Me.searchToolStripMenuItem1.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.searchToolStripMenuItem1.Size = New System.Drawing.Size(272, 22)
        Me.searchToolStripMenuItem1.Text = "searchToolStripMenuItem1"
        Me.searchToolStripMenuItem1.Visible = False
        '
        'newToolStripMenuItem2
        '
        Me.newToolStripMenuItem2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.newToolStripMenuItem2.Name = "newToolStripMenuItem2"
        Me.newToolStripMenuItem2.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.newToolStripMenuItem2.Size = New System.Drawing.Size(272, 22)
        Me.newToolStripMenuItem2.Text = "newToolStripMenuItem2"
        Me.newToolStripMenuItem2.Visible = False
        '
        'exitToolStripMenuItem3
        '
        Me.exitToolStripMenuItem3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.exitToolStripMenuItem3.Name = "exitToolStripMenuItem3"
        Me.exitToolStripMenuItem3.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.exitToolStripMenuItem3.Size = New System.Drawing.Size(272, 22)
        Me.exitToolStripMenuItem3.Text = "exitToolStripMenuItem3"
        Me.exitToolStripMenuItem3.Visible = False
        '
        'exportToolStripMenuItem4
        '
        Me.exportToolStripMenuItem4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.exportToolStripMenuItem4.Name = "exportToolStripMenuItem4"
        Me.exportToolStripMenuItem4.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.X), System.Windows.Forms.Keys)
        Me.exportToolStripMenuItem4.Size = New System.Drawing.Size(272, 22)
        Me.exportToolStripMenuItem4.Text = "exportToolStripMenuItem4"
        Me.exportToolStripMenuItem4.Visible = False
        '
        'printToolStripMenuItem5
        '
        Me.printToolStripMenuItem5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.printToolStripMenuItem5.Name = "printToolStripMenuItem5"
        Me.printToolStripMenuItem5.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.P), System.Windows.Forms.Keys)
        Me.printToolStripMenuItem5.Size = New System.Drawing.Size(272, 22)
        Me.printToolStripMenuItem5.Text = "printToolStripMenuItem5"
        Me.printToolStripMenuItem5.Visible = False
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.GroupBox1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(942, 159)
        Me.Panel1.TabIndex = 1
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.chkCmbItemCounter)
        Me.GroupBox1.Controls.Add(Me.Label8)
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.dtpFrom)
        Me.GroupBox1.Controls.Add(Me.btnExport)
        Me.GroupBox1.Controls.Add(Me.ChkcmbCostcentre)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.btnPrint)
        Me.GroupBox1.Controls.Add(Me.Panel5)
        Me.GroupBox1.Controls.Add(Me.Panel4)
        Me.GroupBox1.Controls.Add(Me.btnView_Search)
        Me.GroupBox1.Controls.Add(Me.btnExit)
        Me.GroupBox1.Controls.Add(Me.Chkcmbmetal)
        Me.GroupBox1.Controls.Add(Me.btnNew)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.chklsttablecode)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.chklstsubitem)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.chkcmbitemname)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(942, 159)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Filteration"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.Red
        Me.Label7.Location = New System.Drawing.Point(667, 15)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(258, 13)
        Me.Label7.TabIndex = 19
        Me.Label7.Text = "* Stock will be consider with approval Item."
        '
        'Label6
        '
        Me.Label6.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(6, 14)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(72, 21)
        Me.Label6.TabIndex = 0
        Me.Label6.Text = "As On Date"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnExport
        '
        Me.btnExport.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExport.Location = New System.Drawing.Point(294, 119)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 18
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(329, 41)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(79, 21)
        Me.Label5.TabIndex = 10
        Me.Label5.Text = "Cost Centre"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnPrint
        '
        Me.btnPrint.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPrint.Location = New System.Drawing.Point(399, 119)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 30)
        Me.btnPrint.TabIndex = 19
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'Panel5
        '
        Me.Panel5.Controls.Add(Me.rbtwast)
        Me.Panel5.Controls.Add(Me.rbttable)
        Me.Panel5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Panel5.Location = New System.Drawing.Point(509, 91)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(236, 27)
        Me.Panel5.TabIndex = 15
        '
        'rbtwast
        '
        Me.rbtwast.AutoSize = True
        Me.rbtwast.Checked = True
        Me.rbtwast.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbtwast.Location = New System.Drawing.Point(120, 5)
        Me.rbtwast.Name = "rbtwast"
        Me.rbtwast.Size = New System.Drawing.Size(96, 17)
        Me.rbtwast.TabIndex = 1
        Me.rbtwast.TabStop = True
        Me.rbtwast.Text = "Wast% base"
        Me.rbtwast.UseVisualStyleBackColor = True
        '
        'rbttable
        '
        Me.rbttable.AutoSize = True
        Me.rbttable.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbttable.Location = New System.Drawing.Point(9, 5)
        Me.rbttable.Name = "rbttable"
        Me.rbttable.Size = New System.Drawing.Size(95, 17)
        Me.rbttable.TabIndex = 0
        Me.rbttable.TabStop = True
        Me.rbttable.Text = "Table Based"
        Me.rbttable.UseVisualStyleBackColor = True
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.rbtSummary)
        Me.Panel4.Controls.Add(Me.rbtDetailed)
        Me.Panel4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Panel4.Location = New System.Drawing.Point(330, 92)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(176, 25)
        Me.Panel4.TabIndex = 14
        '
        'rbtSummary
        '
        Me.rbtSummary.AutoSize = True
        Me.rbtSummary.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbtSummary.Location = New System.Drawing.Point(90, 5)
        Me.rbtSummary.Name = "rbtSummary"
        Me.rbtSummary.Size = New System.Drawing.Size(81, 17)
        Me.rbtSummary.TabIndex = 1
        Me.rbtSummary.TabStop = True
        Me.rbtSummary.Text = "Summary"
        Me.rbtSummary.UseVisualStyleBackColor = True
        '
        'rbtDetailed
        '
        Me.rbtDetailed.AutoSize = True
        Me.rbtDetailed.Checked = True
        Me.rbtDetailed.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbtDetailed.Location = New System.Drawing.Point(9, 5)
        Me.rbtDetailed.Name = "rbtDetailed"
        Me.rbtDetailed.Size = New System.Drawing.Size(72, 17)
        Me.rbtDetailed.TabIndex = 0
        Me.rbtDetailed.TabStop = True
        Me.rbtDetailed.Text = "Detailed"
        Me.rbtDetailed.UseVisualStyleBackColor = True
        '
        'btnView_Search
        '
        Me.btnView_Search.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnView_Search.Location = New System.Drawing.Point(84, 119)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(100, 30)
        Me.btnView_Search.TabIndex = 16
        Me.btnView_Search.Text = "Search"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExit.Location = New System.Drawing.Point(504, 119)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 20
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNew.Location = New System.Drawing.Point(189, 119)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 17
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(6, 41)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(72, 21)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "Metal"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(328, 14)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(89, 21)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "Table Code"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(6, 95)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(82, 21)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Sub Item Name"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(6, 68)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(79, 21)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Item Name"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Panel2
        '
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Panel2.Location = New System.Drawing.Point(0, 612)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(942, 8)
        Me.Panel2.TabIndex = 0
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.GroupBox2)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Panel3.Location = New System.Drawing.Point(0, 159)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(942, 453)
        Me.Panel3.TabIndex = 3
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.gridFlag)
        Me.GroupBox2.Controls.Add(Me.lblTitle)
        Me.GroupBox2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox2.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(942, 453)
        Me.GroupBox2.TabIndex = 0
        Me.GroupBox2.TabStop = False
        '
        'gridFlag
        '
        Me.gridFlag.AllowUserToAddRows = False
        Me.gridFlag.AllowUserToDeleteRows = False
        Me.gridFlag.AllowUserToResizeRows = False
        Me.gridFlag.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridFlag.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridFlag.Location = New System.Drawing.Point(3, 46)
        Me.gridFlag.Name = "gridFlag"
        Me.gridFlag.ReadOnly = True
        Me.gridFlag.RowHeadersVisible = False
        Me.gridFlag.Size = New System.Drawing.Size(936, 404)
        Me.gridFlag.TabIndex = 0
        '
        'lblTitle
        '
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblTitle.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(3, 17)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(936, 29)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lblTitle.Visible = False
        '
        'Label8
        '
        Me.Label8.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(330, 68)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(84, 21)
        Me.Label8.TabIndex = 12
        Me.Label8.Text = "Item Counter"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkCmbItemCounter
        '
        Me.chkCmbItemCounter.CheckOnClick = True
        Me.chkCmbItemCounter.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbItemCounter.DropDownHeight = 1
        Me.chkCmbItemCounter.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkCmbItemCounter.FormattingEnabled = True
        Me.chkCmbItemCounter.IntegralHeight = False
        Me.chkCmbItemCounter.Location = New System.Drawing.Point(420, 67)
        Me.chkCmbItemCounter.Name = "chkCmbItemCounter"
        Me.chkCmbItemCounter.Size = New System.Drawing.Size(236, 22)
        Me.chkCmbItemCounter.TabIndex = 13
        Me.chkCmbItemCounter.ValueSeparator = ", "
        '
        'dtpFrom
        '
        Me.dtpFrom.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpFrom.Location = New System.Drawing.Point(84, 14)
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
        'ChkcmbCostcentre
        '
        Me.ChkcmbCostcentre.CheckOnClick = True
        Me.ChkcmbCostcentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.ChkcmbCostcentre.DropDownHeight = 1
        Me.ChkcmbCostcentre.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkcmbCostcentre.FormattingEnabled = True
        Me.ChkcmbCostcentre.IntegralHeight = False
        Me.ChkcmbCostcentre.Location = New System.Drawing.Point(419, 40)
        Me.ChkcmbCostcentre.Name = "ChkcmbCostcentre"
        Me.ChkcmbCostcentre.Size = New System.Drawing.Size(236, 22)
        Me.ChkcmbCostcentre.TabIndex = 11
        Me.ChkcmbCostcentre.ValueSeparator = ", "
        '
        'Chkcmbmetal
        '
        Me.Chkcmbmetal.CheckOnClick = True
        Me.Chkcmbmetal.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.Chkcmbmetal.DropDownHeight = 1
        Me.Chkcmbmetal.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Chkcmbmetal.FormattingEnabled = True
        Me.Chkcmbmetal.IntegralHeight = False
        Me.Chkcmbmetal.Location = New System.Drawing.Point(84, 40)
        Me.Chkcmbmetal.Name = "Chkcmbmetal"
        Me.Chkcmbmetal.Size = New System.Drawing.Size(236, 22)
        Me.Chkcmbmetal.TabIndex = 3
        Me.Chkcmbmetal.ValueSeparator = ", "
        '
        'chklsttablecode
        '
        Me.chklsttablecode.CheckOnClick = True
        Me.chklsttablecode.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chklsttablecode.DropDownHeight = 1
        Me.chklsttablecode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chklsttablecode.FormattingEnabled = True
        Me.chklsttablecode.IntegralHeight = False
        Me.chklsttablecode.Location = New System.Drawing.Point(420, 13)
        Me.chklsttablecode.Name = "chklsttablecode"
        Me.chklsttablecode.Size = New System.Drawing.Size(124, 22)
        Me.chklsttablecode.TabIndex = 9
        Me.chklsttablecode.ValueSeparator = ", "
        '
        'chklstsubitem
        '
        Me.chklstsubitem.CheckOnClick = True
        Me.chklstsubitem.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chklstsubitem.DropDownHeight = 1
        Me.chklstsubitem.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chklstsubitem.FormattingEnabled = True
        Me.chklstsubitem.IntegralHeight = False
        Me.chklstsubitem.Location = New System.Drawing.Point(84, 94)
        Me.chklstsubitem.Name = "chklstsubitem"
        Me.chklstsubitem.Size = New System.Drawing.Size(236, 22)
        Me.chklstsubitem.TabIndex = 7
        Me.chklstsubitem.ValueSeparator = ", "
        '
        'chkcmbitemname
        '
        Me.chkcmbitemname.CheckOnClick = True
        Me.chkcmbitemname.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkcmbitemname.DropDownHeight = 1
        Me.chkcmbitemname.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkcmbitemname.FormattingEnabled = True
        Me.chkcmbitemname.IntegralHeight = False
        Me.chkcmbitemname.Location = New System.Drawing.Point(84, 67)
        Me.chkcmbitemname.Name = "chkcmbitemname"
        Me.chkcmbitemname.Size = New System.Drawing.Size(236, 22)
        Me.chkcmbitemname.TabIndex = 5
        Me.chkcmbitemname.ValueSeparator = ", "
        '
        'frmTableBaseWastage
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(942, 620)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Cursor = System.Windows.Forms.Cursors.Arrow
        Me.KeyPreview = True
        Me.Name = "frmTableBaseWastage"
        Me.Text = "frmTableBaseWastage"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.Panel5.ResumeLayout(False)
        Me.Panel5.PerformLayout()
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        CType(Me.gridFlag, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents gridFlag As System.Windows.Forms.DataGridView
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnView_Search As System.Windows.Forms.Button
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents chklstsubitem As BrighttechPack.CheckedComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents chkcmbitemname As BrighttechPack.CheckedComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents chklsttablecode As BrighttechPack.CheckedComboBox
    Friend WithEvents searchToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents newToolStripMenuItem2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents exitToolStripMenuItem3 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents exportToolStripMenuItem4 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents printToolStripMenuItem5 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents Panel5 As System.Windows.Forms.Panel
    Friend WithEvents rbttable As System.Windows.Forms.RadioButton
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents rbtSummary As System.Windows.Forms.RadioButton
    Friend WithEvents rbtDetailed As System.Windows.Forms.RadioButton
    Friend WithEvents Chkcmbmetal As BrighttechPack.CheckedComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents rbtwast As System.Windows.Forms.RadioButton
    Friend WithEvents ChkcmbCostcentre As BrighttechPack.CheckedComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents chkCmbItemCounter As BrighttechPack.CheckedComboBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
End Class
