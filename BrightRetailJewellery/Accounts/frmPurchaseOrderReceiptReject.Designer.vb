<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPurchaseOrderReceiptReject
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
        Me.components = New System.ComponentModel.Container()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnOpen = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.gridView = New System.Windows.Forms.DataGridView()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cmbSubItemName_Man = New System.Windows.Forms.ComboBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtItemName = New System.Windows.Forms.TextBox()
        Me.txtItemCode_Num_Man = New System.Windows.Forms.TextBox()
        Me.cmbCostCentre_Man = New System.Windows.Forms.ComboBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label24 = New System.Windows.Forms.Label()
        Me.txtDiamondPieces_Num = New System.Windows.Forms.TextBox()
        Me.txtDidmondWeight_Wet = New System.Windows.Forms.TextBox()
        Me.txtNetWeight_Wet = New System.Windows.Forms.TextBox()
        Me.Label27 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.rbtPurchaseOrder = New System.Windows.Forms.RadioButton()
        Me.rbtPurchaseReceipt = New System.Windows.Forms.RadioButton()
        Me.rbtPurchaseReject = New System.Windows.Forms.RadioButton()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmbDesigner_Man = New System.Windows.Forms.ComboBox()
        Me.txt_Num_Man = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.tabGeneral = New System.Windows.Forms.TabControl()
        Me.tabMain = New System.Windows.Forms.TabPage()
        Me.dtpDate = New BrighttechPack.DatePicker(Me.components)
        Me.tabOpen = New System.Windows.Forms.TabPage()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.gridViewEdit = New System.Windows.Forms.DataGridView()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btnExcel = New System.Windows.Forms.Button()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.btnBack = New System.Windows.Forms.Button()
        Me.cmbPurchaseType_Man = New System.Windows.Forms.ComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.tabGeneral.SuspendLayout()
        Me.tabMain.SuspendLayout()
        Me.tabOpen.SuspendLayout()
        Me.Panel2.SuspendLayout()
        CType(Me.gridViewEdit, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnAdd
        '
        Me.btnAdd.Location = New System.Drawing.Point(44, 324)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(100, 30)
        Me.btnAdd.TabIndex = 23
        Me.btnAdd.Text = "&Add"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Enabled = False
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.ImageKey = "(none)"
        Me.btnSave.Location = New System.Drawing.Point(155, 324)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(100, 30)
        Me.btnSave.TabIndex = 24
        Me.btnSave.Text = "Save [F1]"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnOpen
        '
        Me.btnOpen.Location = New System.Drawing.Point(268, 324)
        Me.btnOpen.Name = "btnOpen"
        Me.btnOpen.Size = New System.Drawing.Size(100, 30)
        Me.btnOpen.TabIndex = 25
        Me.btnOpen.Text = "Open [F2]"
        Me.btnOpen.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(379, 324)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 26
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(490, 324)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 27
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Location = New System.Drawing.Point(40, 360)
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.RowHeadersVisible = False
        Me.gridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridView.Size = New System.Drawing.Size(862, 246)
        Me.gridView.TabIndex = 28
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(41, 53)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(34, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Date"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbSubItemName_Man
        '
        Me.cmbSubItemName_Man.FormattingEnabled = True
        Me.cmbSubItemName_Man.Location = New System.Drawing.Point(155, 185)
        Me.cmbSubItemName_Man.Name = "cmbSubItemName_Man"
        Me.cmbSubItemName_Man.Size = New System.Drawing.Size(269, 21)
        Me.cmbSubItemName_Man.TabIndex = 16
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(41, 155)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(68, 13)
        Me.Label8.TabIndex = 12
        Me.Label8.Text = "Item Code"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(41, 189)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(97, 13)
        Me.Label9.TabIndex = 15
        Me.Label9.Text = "Sub Item Name"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtItemName
        '
        Me.txtItemName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtItemName.Enabled = False
        Me.txtItemName.Location = New System.Drawing.Point(214, 151)
        Me.txtItemName.MaxLength = 50
        Me.txtItemName.Name = "txtItemName"
        Me.txtItemName.Size = New System.Drawing.Size(210, 21)
        Me.txtItemName.TabIndex = 14
        '
        'txtItemCode_Num_Man
        '
        Me.txtItemCode_Num_Man.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtItemCode_Num_Man.Location = New System.Drawing.Point(155, 151)
        Me.txtItemCode_Num_Man.MaxLength = 8
        Me.txtItemCode_Num_Man.Name = "txtItemCode_Num_Man"
        Me.txtItemCode_Num_Man.Size = New System.Drawing.Size(53, 21)
        Me.txtItemCode_Num_Man.TabIndex = 13
        '
        'cmbCostCentre_Man
        '
        Me.cmbCostCentre_Man.FormattingEnabled = True
        Me.cmbCostCentre_Man.Location = New System.Drawing.Point(155, 117)
        Me.cmbCostCentre_Man.Name = "cmbCostCentre_Man"
        Me.cmbCostCentre_Man.Size = New System.Drawing.Size(269, 21)
        Me.cmbCostCentre_Man.TabIndex = 11
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(41, 121)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(76, 13)
        Me.Label6.TabIndex = 10
        Me.Label6.Text = "Cost Centre"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(41, 223)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(43, 13)
        Me.Label15.TabIndex = 17
        Me.Label15.Text = "Pieces"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.Location = New System.Drawing.Point(41, 262)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(69, 13)
        Me.Label24.TabIndex = 19
        Me.Label24.Text = "Grs Weight"
        Me.Label24.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtDiamondPieces_Num
        '
        Me.txtDiamondPieces_Num.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtDiamondPieces_Num.Location = New System.Drawing.Point(155, 219)
        Me.txtDiamondPieces_Num.MaxLength = 8
        Me.txtDiamondPieces_Num.Name = "txtDiamondPieces_Num"
        Me.txtDiamondPieces_Num.Size = New System.Drawing.Size(70, 21)
        Me.txtDiamondPieces_Num.TabIndex = 18
        Me.txtDiamondPieces_Num.Text = "123456789"
        '
        'txtDidmondWeight_Wet
        '
        Me.txtDidmondWeight_Wet.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtDidmondWeight_Wet.Location = New System.Drawing.Point(155, 258)
        Me.txtDidmondWeight_Wet.MaxLength = 13
        Me.txtDidmondWeight_Wet.Name = "txtDidmondWeight_Wet"
        Me.txtDidmondWeight_Wet.Size = New System.Drawing.Size(121, 21)
        Me.txtDidmondWeight_Wet.TabIndex = 20
        Me.txtDidmondWeight_Wet.Text = "12345678.00"
        '
        'txtNetWeight_Wet
        '
        Me.txtNetWeight_Wet.Location = New System.Drawing.Point(155, 292)
        Me.txtNetWeight_Wet.MaxLength = 13
        Me.txtNetWeight_Wet.Name = "txtNetWeight_Wet"
        Me.txtNetWeight_Wet.Size = New System.Drawing.Size(121, 21)
        Me.txtNetWeight_Wet.TabIndex = 22
        Me.txtNetWeight_Wet.Text = "12345678.00"
        '
        'Label27
        '
        Me.Label27.AutoSize = True
        Me.Label27.Location = New System.Drawing.Point(41, 296)
        Me.Label27.Name = "Label27"
        Me.Label27.Size = New System.Drawing.Size(68, 13)
        Me.Label27.TabIndex = 21
        Me.Label27.Text = "Net Weight"
        Me.Label27.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(41, 87)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(58, 13)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "Designer"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'rbtPurchaseOrder
        '
        Me.rbtPurchaseOrder.AutoSize = True
        Me.rbtPurchaseOrder.Checked = True
        Me.rbtPurchaseOrder.Location = New System.Drawing.Point(155, 19)
        Me.rbtPurchaseOrder.Name = "rbtPurchaseOrder"
        Me.rbtPurchaseOrder.Size = New System.Drawing.Size(58, 17)
        Me.rbtPurchaseOrder.TabIndex = 1
        Me.rbtPurchaseOrder.TabStop = True
        Me.rbtPurchaseOrder.Text = "Order"
        Me.rbtPurchaseOrder.UseVisualStyleBackColor = True
        '
        'rbtPurchaseReceipt
        '
        Me.rbtPurchaseReceipt.AutoSize = True
        Me.rbtPurchaseReceipt.Location = New System.Drawing.Point(254, 19)
        Me.rbtPurchaseReceipt.Name = "rbtPurchaseReceipt"
        Me.rbtPurchaseReceipt.Size = New System.Drawing.Size(67, 17)
        Me.rbtPurchaseReceipt.TabIndex = 2
        Me.rbtPurchaseReceipt.Text = "Receipt"
        Me.rbtPurchaseReceipt.UseVisualStyleBackColor = True
        '
        'rbtPurchaseReject
        '
        Me.rbtPurchaseReject.AutoSize = True
        Me.rbtPurchaseReject.Location = New System.Drawing.Point(363, 21)
        Me.rbtPurchaseReject.Name = "rbtPurchaseReject"
        Me.rbtPurchaseReject.Size = New System.Drawing.Size(61, 17)
        Me.rbtPurchaseReject.TabIndex = 3
        Me.rbtPurchaseReject.Text = "Reject"
        Me.rbtPurchaseReject.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(41, 21)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(90, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Purchase Type"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SaveToolStripMenuItem, Me.OpenToolStripMenuItem, Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(123, 92)
        '
        'SaveToolStripMenuItem
        '
        Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        Me.SaveToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.SaveToolStripMenuItem.Text = "Save"
        Me.SaveToolStripMenuItem.Visible = False
        '
        'OpenToolStripMenuItem
        '
        Me.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem"
        Me.OpenToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2
        Me.OpenToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.OpenToolStripMenuItem.Text = "Open"
        Me.OpenToolStripMenuItem.Visible = False
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'cmbDesigner_Man
        '
        Me.cmbDesigner_Man.FormattingEnabled = True
        Me.cmbDesigner_Man.Location = New System.Drawing.Point(155, 83)
        Me.cmbDesigner_Man.Name = "cmbDesigner_Man"
        Me.cmbDesigner_Man.Size = New System.Drawing.Size(269, 21)
        Me.cmbDesigner_Man.TabIndex = 9
        '
        'txt_Num_Man
        '
        Me.txt_Num_Man.Location = New System.Drawing.Point(343, 49)
        Me.txt_Num_Man.Name = "txt_Num_Man"
        Me.txt_Num_Man.Size = New System.Drawing.Size(81, 21)
        Me.txt_Num_Man.TabIndex = 7
        Me.txt_Num_Man.Visible = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(262, 53)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(19, 13)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "..."
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label4.Visible = False
        '
        'tabGeneral
        '
        Me.tabGeneral.Controls.Add(Me.tabMain)
        Me.tabGeneral.Controls.Add(Me.tabOpen)
        Me.tabGeneral.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabGeneral.Location = New System.Drawing.Point(0, 0)
        Me.tabGeneral.Name = "tabGeneral"
        Me.tabGeneral.SelectedIndex = 0
        Me.tabGeneral.Size = New System.Drawing.Size(1248, 640)
        Me.tabGeneral.TabIndex = 0
        '
        'tabMain
        '
        Me.tabMain.Controls.Add(Me.Label4)
        Me.tabMain.Controls.Add(Me.txt_Num_Man)
        Me.tabMain.Controls.Add(Me.cmbDesigner_Man)
        Me.tabMain.Controls.Add(Me.Label1)
        Me.tabMain.Controls.Add(Me.rbtPurchaseReject)
        Me.tabMain.Controls.Add(Me.rbtPurchaseReceipt)
        Me.tabMain.Controls.Add(Me.rbtPurchaseOrder)
        Me.tabMain.Controls.Add(Me.Label3)
        Me.tabMain.Controls.Add(Me.Label15)
        Me.tabMain.Controls.Add(Me.txtNetWeight_Wet)
        Me.tabMain.Controls.Add(Me.Label27)
        Me.tabMain.Controls.Add(Me.txtDiamondPieces_Num)
        Me.tabMain.Controls.Add(Me.Label24)
        Me.tabMain.Controls.Add(Me.cmbCostCentre_Man)
        Me.tabMain.Controls.Add(Me.txtDidmondWeight_Wet)
        Me.tabMain.Controls.Add(Me.Label6)
        Me.tabMain.Controls.Add(Me.cmbSubItemName_Man)
        Me.tabMain.Controls.Add(Me.Label8)
        Me.tabMain.Controls.Add(Me.Label9)
        Me.tabMain.Controls.Add(Me.txtItemName)
        Me.tabMain.Controls.Add(Me.txtItemCode_Num_Man)
        Me.tabMain.Controls.Add(Me.dtpDate)
        Me.tabMain.Controls.Add(Me.Label2)
        Me.tabMain.Controls.Add(Me.btnAdd)
        Me.tabMain.Controls.Add(Me.gridView)
        Me.tabMain.Controls.Add(Me.btnExit)
        Me.tabMain.Controls.Add(Me.btnSave)
        Me.tabMain.Controls.Add(Me.btnOpen)
        Me.tabMain.Controls.Add(Me.btnNew)
        Me.tabMain.Location = New System.Drawing.Point(4, 22)
        Me.tabMain.Name = "tabMain"
        Me.tabMain.Padding = New System.Windows.Forms.Padding(3)
        Me.tabMain.Size = New System.Drawing.Size(1240, 614)
        Me.tabMain.TabIndex = 0
        Me.tabMain.Text = "tabMain"
        Me.tabMain.UseVisualStyleBackColor = True
        '
        'dtpDate
        '
        Me.dtpDate.Location = New System.Drawing.Point(155, 49)
        Me.dtpDate.Mask = "##/##/####"
        Me.dtpDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpDate.Name = "dtpDate"
        Me.dtpDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpDate.Size = New System.Drawing.Size(96, 21)
        Me.dtpDate.TabIndex = 5
        Me.dtpDate.Text = "07/03/9998"
        Me.dtpDate.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'tabOpen
        '
        Me.tabOpen.Controls.Add(Me.Panel2)
        Me.tabOpen.Controls.Add(Me.Panel1)
        Me.tabOpen.Location = New System.Drawing.Point(4, 22)
        Me.tabOpen.Name = "tabOpen"
        Me.tabOpen.Padding = New System.Windows.Forms.Padding(3)
        Me.tabOpen.Size = New System.Drawing.Size(1240, 614)
        Me.tabOpen.TabIndex = 1
        Me.tabOpen.Text = "tabOpen"
        Me.tabOpen.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.gridViewEdit)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(3, 80)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(1234, 531)
        Me.Panel2.TabIndex = 1
        '
        'gridViewEdit
        '
        Me.gridViewEdit.AllowUserToAddRows = False
        Me.gridViewEdit.AllowUserToDeleteRows = False
        Me.gridViewEdit.AllowUserToResizeColumns = False
        Me.gridViewEdit.AllowUserToResizeRows = False
        Me.gridViewEdit.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridViewEdit.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridViewEdit.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridViewEdit.Location = New System.Drawing.Point(0, 0)
        Me.gridViewEdit.Name = "gridViewEdit"
        Me.gridViewEdit.ReadOnly = True
        Me.gridViewEdit.RowHeadersVisible = False
        Me.gridViewEdit.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridViewEdit.Size = New System.Drawing.Size(1234, 531)
        Me.gridViewEdit.TabIndex = 0
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.btnExcel)
        Me.Panel1.Controls.Add(Me.btnPrint)
        Me.Panel1.Controls.Add(Me.btnCancel)
        Me.Panel1.Controls.Add(Me.Label11)
        Me.Panel1.Controls.Add(Me.btnDelete)
        Me.Panel1.Controls.Add(Me.btnSearch)
        Me.Panel1.Controls.Add(Me.Label10)
        Me.Panel1.Controls.Add(Me.Label7)
        Me.Panel1.Controls.Add(Me.dtpTo)
        Me.Panel1.Controls.Add(Me.dtpFrom)
        Me.Panel1.Controls.Add(Me.btnBack)
        Me.Panel1.Controls.Add(Me.cmbPurchaseType_Man)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(3, 3)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1234, 77)
        Me.Panel1.TabIndex = 0
        '
        'btnExcel
        '
        Me.btnExcel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExcel.ImageKey = "(none)"
        Me.btnExcel.Location = New System.Drawing.Point(1079, 15)
        Me.btnExcel.Name = "btnExcel"
        Me.btnExcel.Size = New System.Drawing.Size(100, 30)
        Me.btnExcel.TabIndex = 11
        Me.btnExcel.TabStop = False
        Me.btnExcel.Text = "Excel"
        Me.btnExcel.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnPrint.ImageKey = "(none)"
        Me.btnPrint.Location = New System.Drawing.Point(973, 15)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 30)
        Me.btnPrint.TabIndex = 10
        Me.btnPrint.TabStop = False
        Me.btnPrint.Text = "Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancel.ImageKey = "(none)"
        Me.btnCancel.Location = New System.Drawing.Point(761, 15)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(100, 30)
        Me.btnCancel.TabIndex = 8
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.ForeColor = System.Drawing.Color.Red
        Me.Label11.Location = New System.Drawing.Point(850, 58)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(121, 13)
        Me.Label11.TabIndex = 12
        Me.Label11.Text = "*Press Enter To Edit"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnDelete
        '
        Me.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnDelete.ImageKey = "(none)"
        Me.btnDelete.Location = New System.Drawing.Point(655, 15)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(100, 30)
        Me.btnDelete.TabIndex = 7
        Me.btnDelete.Text = "Delete"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSearch.ImageKey = "(none)"
        Me.btnSearch.Location = New System.Drawing.Point(549, 15)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 6
        Me.btnSearch.Text = "Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(416, 24)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(20, 13)
        Me.Label10.TabIndex = 4
        Me.Label10.Text = "To"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(268, 24)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(36, 13)
        Me.Label7.TabIndex = 2
        Me.Label7.Text = "From"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(445, 20)
        Me.dtpTo.Mask = "##/##/####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(96, 21)
        Me.dtpTo.TabIndex = 5
        Me.dtpTo.Text = "07/03/9998"
        Me.dtpTo.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(312, 20)
        Me.dtpFrom.Mask = "##/##/####"
        Me.dtpFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom.Size = New System.Drawing.Size(96, 21)
        Me.dtpFrom.TabIndex = 3
        Me.dtpFrom.Text = "07/03/9998"
        Me.dtpFrom.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'btnBack
        '
        Me.btnBack.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnBack.ImageKey = "(none)"
        Me.btnBack.Location = New System.Drawing.Point(867, 15)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(100, 30)
        Me.btnBack.TabIndex = 9
        Me.btnBack.TabStop = False
        Me.btnBack.Text = "Back[ESC]"
        Me.btnBack.UseVisualStyleBackColor = True
        '
        'cmbPurchaseType_Man
        '
        Me.cmbPurchaseType_Man.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbPurchaseType_Man.FormattingEnabled = True
        Me.cmbPurchaseType_Man.Location = New System.Drawing.Point(53, 20)
        Me.cmbPurchaseType_Man.Name = "cmbPurchaseType_Man"
        Me.cmbPurchaseType_Man.Size = New System.Drawing.Size(207, 21)
        Me.cmbPurchaseType_Man.TabIndex = 1
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(10, 24)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(34, 13)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "Type"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'frmPurchaseOrderReceiptReject
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1248, 640)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.tabGeneral)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmPurchaseOrderReceiptReject"
        Me.Text = "Purchase Order/Receipt/Reject"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.tabGeneral.ResumeLayout(False)
        Me.tabMain.ResumeLayout(False)
        Me.tabMain.PerformLayout()
        Me.tabOpen.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        CType(Me.gridViewEdit, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnAdd As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnOpen As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents dtpDate As BrighttechPack.DatePicker
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents cmbSubItemName_Man As System.Windows.Forms.ComboBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txtItemName As System.Windows.Forms.TextBox
    Friend WithEvents txtItemCode_Num_Man As System.Windows.Forms.TextBox
    Friend WithEvents cmbCostCentre_Man As System.Windows.Forms.ComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents txtDiamondPieces_Num As System.Windows.Forms.TextBox
    Friend WithEvents txtDidmondWeight_Wet As System.Windows.Forms.TextBox
    Friend WithEvents txtNetWeight_Wet As System.Windows.Forms.TextBox
    Friend WithEvents Label27 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents rbtPurchaseOrder As System.Windows.Forms.RadioButton
    Friend WithEvents rbtPurchaseReceipt As System.Windows.Forms.RadioButton
    Friend WithEvents rbtPurchaseReject As System.Windows.Forms.RadioButton
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmbDesigner_Man As System.Windows.Forms.ComboBox
    Friend WithEvents txt_Num_Man As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents tabGeneral As System.Windows.Forms.TabControl
    Friend WithEvents tabMain As System.Windows.Forms.TabPage
    Friend WithEvents tabOpen As System.Windows.Forms.TabPage
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents btnBack As System.Windows.Forms.Button
    Friend WithEvents cmbPurchaseType_Man As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents gridViewEdit As System.Windows.Forms.DataGridView
    Friend WithEvents btnExcel As Button
    Friend WithEvents btnPrint As Button
End Class
