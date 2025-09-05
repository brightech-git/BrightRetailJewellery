<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CatalogView
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(CatalogView))
        Me.tabMain = New System.Windows.Forms.TabControl
        Me.tabGeneral = New System.Windows.Forms.TabPage
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.pnlContainer = New System.Windows.Forms.Panel
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.rbtCrystalView = New System.Windows.Forms.RadioButton
        Me.rbtList = New System.Windows.Forms.RadioButton
        Me.rbtGrid = New System.Windows.Forms.RadioButton
        Me.Label10 = New System.Windows.Forms.Label
        Me.txtSubItem = New System.Windows.Forms.TextBox
        Me.txtItem = New System.Windows.Forms.TextBox
        Me.chkLstSubItem = New System.Windows.Forms.CheckedListBox
        Me.chkLstItem = New System.Windows.Forms.CheckedListBox
        Me.Label20 = New System.Windows.Forms.Label
        Me.Label19 = New System.Windows.Forms.Label
        Me.btnNew = New System.Windows.Forms.Button
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.Label14 = New System.Windows.Forms.Label
        Me.txtDiaWtTo_Wet = New System.Windows.Forms.TextBox
        Me.Label12 = New System.Windows.Forms.Label
        Me.txtGrsWtTo_Wet = New System.Windows.Forms.TextBox
        Me.Label15 = New System.Windows.Forms.Label
        Me.txtNetWtTo_Wet = New System.Windows.Forms.TextBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.txtDiaWtFrom_Wet = New System.Windows.Forms.TextBox
        Me.txtGrsWtFrom_Wet = New System.Windows.Forms.TextBox
        Me.txtNetWtFrom_Wet = New System.Windows.Forms.TextBox
        Me.btnExit = New System.Windows.Forms.Button
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.Label13 = New System.Windows.Forms.Label
        Me.txtStnWtTo_Wet = New System.Windows.Forms.TextBox
        Me.Label16 = New System.Windows.Forms.Label
        Me.Label17 = New System.Windows.Forms.Label
        Me.txtStnWtFrom_Wet = New System.Windows.Forms.TextBox
        Me.chkLstStoneSubItem = New System.Windows.Forms.CheckedListBox
        Me.chkLstStoneItem = New System.Windows.Forms.CheckedListBox
        Me.btnSearch = New System.Windows.Forms.Button
        Me.tabView = New System.Windows.Forms.TabPage
        Me.pnlImage = New System.Windows.Forms.Panel
        Me.picImage = New System.Windows.Forms.PictureBox
        Me.pnlRightPart = New System.Windows.Forms.Panel
        Me.Label26 = New System.Windows.Forms.Label
        Me.lblCatSno = New System.Windows.Forms.Label
        Me.Label24 = New System.Windows.Forms.Label
        Me.btnShopCart = New System.Windows.Forms.Button
        Me.cmbOrderName_OWN = New System.Windows.Forms.ComboBox
        Me.dgvStoneDetail = New System.Windows.Forms.DataGridView
        Me.Label23 = New System.Windows.Forms.Label
        Me.lblDiaPcsWt = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label22 = New System.Windows.Forms.Label
        Me.lblPcsWt = New System.Windows.Forms.Label
        Me.Label21 = New System.Windows.Forms.Label
        Me.lblItemType = New System.Windows.Forms.Label
        Me.Label18 = New System.Windows.Forms.Label
        Me.lblSubItem = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.lblItemName = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label27 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.lblStyleNo = New System.Windows.Forms.Label
        Me.lblItemId = New System.Windows.Forms.Label
        Me.Label25 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.pnlFooter = New System.Windows.Forms.Panel
        Me.dgvFooter = New System.Windows.Forms.DataGridView
        Me.dgv = New System.Windows.Forms.DataGridView
        Me.tabGridView = New System.Windows.Forms.TabPage
        Me.gridDetailView = New System.Windows.Forms.DataGridView
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.Label28 = New System.Windows.Forms.Label
        Me.btnPrint = New System.Windows.Forms.Button
        Me.btnExport = New System.Windows.Forms.Button
        Me.chkStudColumn = New System.Windows.Forms.CheckBox
        Me.tabMain.SuspendLayout()
        Me.tabGeneral.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.pnlContainer.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.tabView.SuspendLayout()
        Me.pnlImage.SuspendLayout()
        CType(Me.picImage, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlRightPart.SuspendLayout()
        CType(Me.dgvStoneDetail, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlFooter.SuspendLayout()
        CType(Me.dgvFooter, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgv, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabGridView.SuspendLayout()
        CType(Me.gridDetailView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'tabMain
        '
        Me.tabMain.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
        Me.tabMain.Controls.Add(Me.tabGeneral)
        Me.tabMain.Controls.Add(Me.tabView)
        Me.tabMain.Controls.Add(Me.tabGridView)
        Me.tabMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabMain.Location = New System.Drawing.Point(0, 0)
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(1021, 642)
        Me.tabMain.TabIndex = 0
        '
        'tabGeneral
        '
        Me.tabGeneral.ContextMenuStrip = Me.ContextMenuStrip1
        Me.tabGeneral.Controls.Add(Me.pnlContainer)
        Me.tabGeneral.Location = New System.Drawing.Point(4, 25)
        Me.tabGeneral.Name = "tabGeneral"
        Me.tabGeneral.Padding = New System.Windows.Forms.Padding(3)
        Me.tabGeneral.Size = New System.Drawing.Size(1013, 613)
        Me.tabGeneral.TabIndex = 0
        Me.tabGeneral.Text = "General"
        Me.tabGeneral.UseVisualStyleBackColor = True
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
        'pnlContainer
        '
        Me.pnlContainer.Controls.Add(Me.chkStudColumn)
        Me.pnlContainer.Controls.Add(Me.GroupBox1)
        Me.pnlContainer.Controls.Add(Me.btnNew)
        Me.pnlContainer.Controls.Add(Me.GroupBox2)
        Me.pnlContainer.Controls.Add(Me.btnExit)
        Me.pnlContainer.Controls.Add(Me.GroupBox3)
        Me.pnlContainer.Controls.Add(Me.btnSearch)
        Me.pnlContainer.Location = New System.Drawing.Point(116, 69)
        Me.pnlContainer.Name = "pnlContainer"
        Me.pnlContainer.Size = New System.Drawing.Size(583, 415)
        Me.pnlContainer.TabIndex = 0
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.rbtCrystalView)
        Me.GroupBox1.Controls.Add(Me.rbtList)
        Me.GroupBox1.Controls.Add(Me.rbtGrid)
        Me.GroupBox1.Controls.Add(Me.Label10)
        Me.GroupBox1.Controls.Add(Me.txtSubItem)
        Me.GroupBox1.Controls.Add(Me.txtItem)
        Me.GroupBox1.Controls.Add(Me.chkLstSubItem)
        Me.GroupBox1.Controls.Add(Me.chkLstItem)
        Me.GroupBox1.Controls.Add(Me.Label20)
        Me.GroupBox1.Controls.Add(Me.Label19)
        Me.GroupBox1.Location = New System.Drawing.Point(14, 15)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(556, 179)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "General"
        '
        'rbtCrystalView
        '
        Me.rbtCrystalView.AutoSize = True
        Me.rbtCrystalView.Location = New System.Drawing.Point(199, 40)
        Me.rbtCrystalView.Name = "rbtCrystalView"
        Me.rbtCrystalView.Size = New System.Drawing.Size(97, 17)
        Me.rbtCrystalView.TabIndex = 3
        Me.rbtCrystalView.TabStop = True
        Me.rbtCrystalView.Text = "Crystal View"
        Me.rbtCrystalView.UseVisualStyleBackColor = True
        '
        'rbtList
        '
        Me.rbtList.AutoSize = True
        Me.rbtList.Location = New System.Drawing.Point(150, 39)
        Me.rbtList.Name = "rbtList"
        Me.rbtList.Size = New System.Drawing.Size(44, 17)
        Me.rbtList.TabIndex = 2
        Me.rbtList.Text = "List"
        Me.rbtList.UseVisualStyleBackColor = True
        '
        'rbtGrid
        '
        Me.rbtGrid.AutoSize = True
        Me.rbtGrid.Checked = True
        Me.rbtGrid.Location = New System.Drawing.Point(93, 39)
        Me.rbtGrid.Name = "rbtGrid"
        Me.rbtGrid.Size = New System.Drawing.Size(49, 17)
        Me.rbtGrid.TabIndex = 1
        Me.rbtGrid.TabStop = True
        Me.rbtGrid.Text = "Grid"
        Me.rbtGrid.UseVisualStyleBackColor = True
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(10, 41)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(67, 13)
        Me.Label10.TabIndex = 0
        Me.Label10.Text = "View Style"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtSubItem
        '
        Me.txtSubItem.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtSubItem.Location = New System.Drawing.Point(278, 80)
        Me.txtSubItem.Name = "txtSubItem"
        Me.txtSubItem.Size = New System.Drawing.Size(270, 21)
        Me.txtSubItem.TabIndex = 7
        '
        'txtItem
        '
        Me.txtItem.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtItem.Location = New System.Drawing.Point(10, 80)
        Me.txtItem.Name = "txtItem"
        Me.txtItem.Size = New System.Drawing.Size(262, 21)
        Me.txtItem.TabIndex = 4
        '
        'chkLstSubItem
        '
        Me.chkLstSubItem.CheckOnClick = True
        Me.chkLstSubItem.FormattingEnabled = True
        Me.chkLstSubItem.Items.AddRange(New Object() {"gd", "sdf", "sfd", "sf"})
        Me.chkLstSubItem.Location = New System.Drawing.Point(278, 102)
        Me.chkLstSubItem.Name = "chkLstSubItem"
        Me.chkLstSubItem.Size = New System.Drawing.Size(270, 68)
        Me.chkLstSubItem.TabIndex = 8
        '
        'chkLstItem
        '
        Me.chkLstItem.FormattingEnabled = True
        Me.chkLstItem.Items.AddRange(New Object() {"gd", "sdf", "sfd", "sf"})
        Me.chkLstItem.Location = New System.Drawing.Point(10, 102)
        Me.chkLstItem.Name = "chkLstItem"
        Me.chkLstItem.Size = New System.Drawing.Size(262, 68)
        Me.chkLstItem.TabIndex = 5
        '
        'Label20
        '
        Me.Label20.Location = New System.Drawing.Point(281, 64)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(78, 14)
        Me.Label20.TabIndex = 6
        Me.Label20.Text = "Sub Item"
        Me.Label20.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label19
        '
        Me.Label19.Location = New System.Drawing.Point(10, 64)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(78, 14)
        Me.Label19.TabIndex = 3
        Me.Label19.Text = "Item"
        Me.Label19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(122, 380)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 4
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Label14)
        Me.GroupBox2.Controls.Add(Me.txtDiaWtTo_Wet)
        Me.GroupBox2.Controls.Add(Me.Label12)
        Me.GroupBox2.Controls.Add(Me.txtGrsWtTo_Wet)
        Me.GroupBox2.Controls.Add(Me.Label15)
        Me.GroupBox2.Controls.Add(Me.txtNetWtTo_Wet)
        Me.GroupBox2.Controls.Add(Me.Label11)
        Me.GroupBox2.Controls.Add(Me.Label4)
        Me.GroupBox2.Controls.Add(Me.txtDiaWtFrom_Wet)
        Me.GroupBox2.Controls.Add(Me.txtGrsWtFrom_Wet)
        Me.GroupBox2.Controls.Add(Me.txtNetWtFrom_Wet)
        Me.GroupBox2.Location = New System.Drawing.Point(14, 196)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(240, 131)
        Me.GroupBox2.TabIndex = 1
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Item Detail"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(6, 70)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(45, 13)
        Me.Label14.TabIndex = 5
        Me.Label14.Text = "Net Wt"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtDiaWtTo_Wet
        '
        Me.txtDiaWtTo_Wet.Location = New System.Drawing.Point(165, 104)
        Me.txtDiaWtTo_Wet.MaxLength = 9
        Me.txtDiaWtTo_Wet.Name = "txtDiaWtTo_Wet"
        Me.txtDiaWtTo_Wet.Size = New System.Drawing.Size(69, 21)
        Me.txtDiaWtTo_Wet.TabIndex = 10
        Me.txtDiaWtTo_Wet.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(6, 32)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(59, 13)
        Me.Label12.TabIndex = 2
        Me.Label12.Text = "Gross Wt"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtGrsWtTo_Wet
        '
        Me.txtGrsWtTo_Wet.Location = New System.Drawing.Point(165, 28)
        Me.txtGrsWtTo_Wet.MaxLength = 9
        Me.txtGrsWtTo_Wet.Name = "txtGrsWtTo_Wet"
        Me.txtGrsWtTo_Wet.Size = New System.Drawing.Size(69, 21)
        Me.txtGrsWtTo_Wet.TabIndex = 4
        Me.txtGrsWtTo_Wet.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(6, 108)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(69, 13)
        Me.Label15.TabIndex = 8
        Me.Label15.Text = "DiaWt (Cr)"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtNetWtTo_Wet
        '
        Me.txtNetWtTo_Wet.Location = New System.Drawing.Point(165, 66)
        Me.txtNetWtTo_Wet.MaxLength = 9
        Me.txtNetWtTo_Wet.Name = "txtNetWtTo_Wet"
        Me.txtNetWtTo_Wet.Size = New System.Drawing.Size(69, 21)
        Me.txtNetWtTo_Wet.TabIndex = 7
        Me.txtNetWtTo_Wet.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label11
        '
        Me.Label11.Location = New System.Drawing.Point(167, 13)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(68, 14)
        Me.Label11.TabIndex = 1
        Me.Label11.Text = "To"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(92, 12)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(69, 14)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "From"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtDiaWtFrom_Wet
        '
        Me.txtDiaWtFrom_Wet.Location = New System.Drawing.Point(92, 104)
        Me.txtDiaWtFrom_Wet.MaxLength = 9
        Me.txtDiaWtFrom_Wet.Name = "txtDiaWtFrom_Wet"
        Me.txtDiaWtFrom_Wet.Size = New System.Drawing.Size(69, 21)
        Me.txtDiaWtFrom_Wet.TabIndex = 9
        Me.txtDiaWtFrom_Wet.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtGrsWtFrom_Wet
        '
        Me.txtGrsWtFrom_Wet.Location = New System.Drawing.Point(92, 28)
        Me.txtGrsWtFrom_Wet.MaxLength = 9
        Me.txtGrsWtFrom_Wet.Name = "txtGrsWtFrom_Wet"
        Me.txtGrsWtFrom_Wet.Size = New System.Drawing.Size(69, 21)
        Me.txtGrsWtFrom_Wet.TabIndex = 3
        Me.txtGrsWtFrom_Wet.Text = "1000.000"
        Me.txtGrsWtFrom_Wet.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtNetWtFrom_Wet
        '
        Me.txtNetWtFrom_Wet.Location = New System.Drawing.Point(92, 66)
        Me.txtNetWtFrom_Wet.MaxLength = 9
        Me.txtNetWtFrom_Wet.Name = "txtNetWtFrom_Wet"
        Me.txtNetWtFrom_Wet.Size = New System.Drawing.Size(69, 21)
        Me.txtNetWtFrom_Wet.TabIndex = 6
        Me.txtNetWtFrom_Wet.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(230, 380)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 5
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.Label13)
        Me.GroupBox3.Controls.Add(Me.txtStnWtTo_Wet)
        Me.GroupBox3.Controls.Add(Me.Label16)
        Me.GroupBox3.Controls.Add(Me.Label17)
        Me.GroupBox3.Controls.Add(Me.txtStnWtFrom_Wet)
        Me.GroupBox3.Controls.Add(Me.chkLstStoneSubItem)
        Me.GroupBox3.Controls.Add(Me.chkLstStoneItem)
        Me.GroupBox3.Location = New System.Drawing.Point(260, 196)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(310, 131)
        Me.GroupBox3.TabIndex = 2
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Stone Detail"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(6, 67)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(60, 13)
        Me.Label13.TabIndex = 2
        Me.Label13.Text = "Sub Item"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtStnWtTo_Wet
        '
        Me.txtStnWtTo_Wet.Location = New System.Drawing.Point(157, 103)
        Me.txtStnWtTo_Wet.MaxLength = 9
        Me.txtStnWtTo_Wet.Name = "txtStnWtTo_Wet"
        Me.txtStnWtTo_Wet.Size = New System.Drawing.Size(69, 21)
        Me.txtStnWtTo_Wet.TabIndex = 6
        Me.txtStnWtTo_Wet.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(6, 27)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(34, 13)
        Me.Label16.TabIndex = 0
        Me.Label16.Text = "Item"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(6, 107)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(74, 13)
        Me.Label17.TabIndex = 4
        Me.Label17.Text = "Weight (Gr)"
        Me.Label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtStnWtFrom_Wet
        '
        Me.txtStnWtFrom_Wet.Location = New System.Drawing.Point(84, 103)
        Me.txtStnWtFrom_Wet.MaxLength = 9
        Me.txtStnWtFrom_Wet.Name = "txtStnWtFrom_Wet"
        Me.txtStnWtFrom_Wet.Size = New System.Drawing.Size(69, 21)
        Me.txtStnWtFrom_Wet.TabIndex = 5
        Me.txtStnWtFrom_Wet.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'chkLstStoneSubItem
        '
        Me.chkLstStoneSubItem.CheckOnClick = True
        Me.chkLstStoneSubItem.FormattingEnabled = True
        Me.chkLstStoneSubItem.Items.AddRange(New Object() {"gd", "sdf", "sfd", "sf"})
        Me.chkLstStoneSubItem.Location = New System.Drawing.Point(84, 63)
        Me.chkLstStoneSubItem.Name = "chkLstStoneSubItem"
        Me.chkLstStoneSubItem.Size = New System.Drawing.Size(219, 36)
        Me.chkLstStoneSubItem.TabIndex = 3
        '
        'chkLstStoneItem
        '
        Me.chkLstStoneItem.CheckOnClick = True
        Me.chkLstStoneItem.FormattingEnabled = True
        Me.chkLstStoneItem.Items.AddRange(New Object() {"gd", "sdf", "sfd", "sf"})
        Me.chkLstStoneItem.Location = New System.Drawing.Point(84, 23)
        Me.chkLstStoneItem.Name = "chkLstStoneItem"
        Me.chkLstStoneItem.Size = New System.Drawing.Size(219, 36)
        Me.chkLstStoneItem.TabIndex = 1
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(14, 380)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 3
        Me.btnSearch.Text = "&Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'tabView
        '
        Me.tabView.BackColor = System.Drawing.Color.White
        Me.tabView.Controls.Add(Me.pnlImage)
        Me.tabView.Controls.Add(Me.pnlFooter)
        Me.tabView.Location = New System.Drawing.Point(4, 25)
        Me.tabView.Name = "tabView"
        Me.tabView.Size = New System.Drawing.Size(1013, 613)
        Me.tabView.TabIndex = 4
        Me.tabView.Text = "View"
        '
        'pnlImage
        '
        Me.pnlImage.AutoScroll = True
        Me.pnlImage.Controls.Add(Me.picImage)
        Me.pnlImage.Controls.Add(Me.pnlRightPart)
        Me.pnlImage.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlImage.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlImage.Location = New System.Drawing.Point(0, 0)
        Me.pnlImage.Name = "pnlImage"
        Me.pnlImage.Size = New System.Drawing.Size(1013, 416)
        Me.pnlImage.TabIndex = 6
        '
        'picImage
        '
        Me.picImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.picImage.Location = New System.Drawing.Point(271, 16)
        Me.picImage.Name = "picImage"
        Me.picImage.Size = New System.Drawing.Size(447, 394)
        Me.picImage.TabIndex = 1
        Me.picImage.TabStop = False
        '
        'pnlRightPart
        '
        Me.pnlRightPart.BackColor = System.Drawing.SystemColors.Control
        Me.pnlRightPart.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlRightPart.Controls.Add(Me.Label26)
        Me.pnlRightPart.Controls.Add(Me.lblCatSno)
        Me.pnlRightPart.Controls.Add(Me.Label24)
        Me.pnlRightPart.Controls.Add(Me.btnShopCart)
        Me.pnlRightPart.Controls.Add(Me.cmbOrderName_OWN)
        Me.pnlRightPart.Controls.Add(Me.dgvStoneDetail)
        Me.pnlRightPart.Controls.Add(Me.Label23)
        Me.pnlRightPart.Controls.Add(Me.lblDiaPcsWt)
        Me.pnlRightPart.Controls.Add(Me.Label6)
        Me.pnlRightPart.Controls.Add(Me.Label5)
        Me.pnlRightPart.Controls.Add(Me.Label22)
        Me.pnlRightPart.Controls.Add(Me.lblPcsWt)
        Me.pnlRightPart.Controls.Add(Me.Label21)
        Me.pnlRightPart.Controls.Add(Me.lblItemType)
        Me.pnlRightPart.Controls.Add(Me.Label18)
        Me.pnlRightPart.Controls.Add(Me.lblSubItem)
        Me.pnlRightPart.Controls.Add(Me.Label7)
        Me.pnlRightPart.Controls.Add(Me.Label3)
        Me.pnlRightPart.Controls.Add(Me.Label9)
        Me.pnlRightPart.Controls.Add(Me.lblItemName)
        Me.pnlRightPart.Controls.Add(Me.Label2)
        Me.pnlRightPart.Controls.Add(Me.Label27)
        Me.pnlRightPart.Controls.Add(Me.Label8)
        Me.pnlRightPart.Controls.Add(Me.lblStyleNo)
        Me.pnlRightPart.Controls.Add(Me.lblItemId)
        Me.pnlRightPart.Controls.Add(Me.Label25)
        Me.pnlRightPart.Controls.Add(Me.Label1)
        Me.pnlRightPart.Dock = System.Windows.Forms.DockStyle.Right
        Me.pnlRightPart.Location = New System.Drawing.Point(773, 0)
        Me.pnlRightPart.Name = "pnlRightPart"
        Me.pnlRightPart.Size = New System.Drawing.Size(240, 416)
        Me.pnlRightPart.TabIndex = 2
        '
        'Label26
        '
        Me.Label26.AutoSize = True
        Me.Label26.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label26.ForeColor = System.Drawing.Color.Red
        Me.Label26.Location = New System.Drawing.Point(114, 376)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(119, 13)
        Me.Label26.TabIndex = 6
        Me.Label26.Text = "Press [S] to Print"
        '
        'lblCatSno
        '
        Me.lblCatSno.AutoSize = True
        Me.lblCatSno.Location = New System.Drawing.Point(125, 331)
        Me.lblCatSno.Name = "lblCatSno"
        Me.lblCatSno.Size = New System.Drawing.Size(49, 13)
        Me.lblCatSno.TabIndex = 5
        Me.lblCatSno.Text = "CatSno"
        Me.lblCatSno.Visible = False
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.Location = New System.Drawing.Point(7, 331)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(77, 13)
        Me.Label24.TabIndex = 4
        Me.Label24.Text = "Order Name"
        '
        'btnShopCart
        '
        Me.btnShopCart.Image = CType(resources.GetObject("btnShopCart.Image"), System.Drawing.Image)
        Me.btnShopCart.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnShopCart.Location = New System.Drawing.Point(6, 376)
        Me.btnShopCart.Name = "btnShopCart"
        Me.btnShopCart.Size = New System.Drawing.Size(108, 34)
        Me.btnShopCart.TabIndex = 3
        Me.btnShopCart.Text = "&Add Order"
        Me.btnShopCart.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnShopCart.UseVisualStyleBackColor = True
        '
        'cmbOrderName_OWN
        '
        Me.cmbOrderName_OWN.FormattingEnabled = True
        Me.cmbOrderName_OWN.Location = New System.Drawing.Point(6, 349)
        Me.cmbOrderName_OWN.Name = "cmbOrderName_OWN"
        Me.cmbOrderName_OWN.Size = New System.Drawing.Size(224, 21)
        Me.cmbOrderName_OWN.TabIndex = 2
        '
        'dgvStoneDetail
        '
        Me.dgvStoneDetail.AllowUserToAddRows = False
        Me.dgvStoneDetail.AllowUserToDeleteRows = False
        Me.dgvStoneDetail.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
        Me.dgvStoneDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvStoneDetail.Location = New System.Drawing.Point(6, 158)
        Me.dgvStoneDetail.Name = "dgvStoneDetail"
        Me.dgvStoneDetail.ReadOnly = True
        Me.dgvStoneDetail.Size = New System.Drawing.Size(227, 170)
        Me.dgvStoneDetail.TabIndex = 1
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.Location = New System.Drawing.Point(94, 138)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(12, 13)
        Me.Label23.TabIndex = 0
        Me.Label23.Text = ":"
        '
        'lblDiaPcsWt
        '
        Me.lblDiaPcsWt.AutoSize = True
        Me.lblDiaPcsWt.Location = New System.Drawing.Point(104, 138)
        Me.lblDiaPcsWt.Name = "lblDiaPcsWt"
        Me.lblDiaPcsWt.Size = New System.Drawing.Size(93, 13)
        Me.lblDiaPcsWt.TabIndex = 0
        Me.lblDiaPcsWt.Text = "Dia Pcs/Weight"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(3, 138)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(93, 13)
        Me.Label6.TabIndex = 0
        Me.Label6.Text = "Dia Pcs/Weight"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(3, 117)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(94, 13)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "Pcs/Grs Weight"
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Location = New System.Drawing.Point(94, 117)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(12, 13)
        Me.Label22.TabIndex = 0
        Me.Label22.Text = ":"
        '
        'lblPcsWt
        '
        Me.lblPcsWt.AutoSize = True
        Me.lblPcsWt.Location = New System.Drawing.Point(104, 117)
        Me.lblPcsWt.Name = "lblPcsWt"
        Me.lblPcsWt.Size = New System.Drawing.Size(94, 13)
        Me.lblPcsWt.TabIndex = 0
        Me.lblPcsWt.Text = "Pcs/Grs Weight"
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Location = New System.Drawing.Point(94, 96)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(12, 13)
        Me.Label21.TabIndex = 0
        Me.Label21.Text = ":"
        '
        'lblItemType
        '
        Me.lblItemType.AutoSize = True
        Me.lblItemType.Location = New System.Drawing.Point(104, 96)
        Me.lblItemType.Name = "lblItemType"
        Me.lblItemType.Size = New System.Drawing.Size(66, 13)
        Me.lblItemType.TabIndex = 0
        Me.lblItemType.Text = "Item Type"
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(94, 75)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(12, 13)
        Me.Label18.TabIndex = 0
        Me.Label18.Text = ":"
        '
        'lblSubItem
        '
        Me.lblSubItem.AutoSize = True
        Me.lblSubItem.Location = New System.Drawing.Point(104, 75)
        Me.lblSubItem.Name = "lblSubItem"
        Me.lblSubItem.Size = New System.Drawing.Size(60, 13)
        Me.lblSubItem.TabIndex = 0
        Me.lblSubItem.Text = "Sub Item"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(3, 96)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(66, 13)
        Me.Label7.TabIndex = 0
        Me.Label7.Text = "Item Type"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(3, 75)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(60, 13)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Sub Item"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(94, 54)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(12, 13)
        Me.Label9.TabIndex = 0
        Me.Label9.Text = ":"
        '
        'lblItemName
        '
        Me.lblItemName.AutoSize = True
        Me.lblItemName.Location = New System.Drawing.Point(104, 54)
        Me.lblItemName.Name = "lblItemName"
        Me.lblItemName.Size = New System.Drawing.Size(71, 13)
        Me.lblItemName.TabIndex = 0
        Me.lblItemName.Text = "Item Name"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(3, 54)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(71, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Item Name"
        '
        'Label27
        '
        Me.Label27.AutoSize = True
        Me.Label27.Location = New System.Drawing.Point(94, 14)
        Me.Label27.Name = "Label27"
        Me.Label27.Size = New System.Drawing.Size(12, 13)
        Me.Label27.TabIndex = 0
        Me.Label27.Text = ":"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(94, 33)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(12, 13)
        Me.Label8.TabIndex = 0
        Me.Label8.Text = ":"
        '
        'lblStyleNo
        '
        Me.lblStyleNo.AutoSize = True
        Me.lblStyleNo.Location = New System.Drawing.Point(104, 14)
        Me.lblStyleNo.Name = "lblStyleNo"
        Me.lblStyleNo.Size = New System.Drawing.Size(50, 13)
        Me.lblStyleNo.TabIndex = 0
        Me.lblStyleNo.Text = "Item Id"
        '
        'lblItemId
        '
        Me.lblItemId.AutoSize = True
        Me.lblItemId.Location = New System.Drawing.Point(104, 33)
        Me.lblItemId.Name = "lblItemId"
        Me.lblItemId.Size = New System.Drawing.Size(50, 13)
        Me.lblItemId.TabIndex = 0
        Me.lblItemId.Text = "Item Id"
        '
        'Label25
        '
        Me.Label25.AutoSize = True
        Me.Label25.Location = New System.Drawing.Point(3, 14)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(55, 13)
        Me.Label25.TabIndex = 0
        Me.Label25.Text = "Style No"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(3, 33)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(50, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Item Id"
        '
        'pnlFooter
        '
        Me.pnlFooter.Controls.Add(Me.dgvFooter)
        Me.pnlFooter.Controls.Add(Me.dgv)
        Me.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlFooter.Location = New System.Drawing.Point(0, 416)
        Me.pnlFooter.Name = "pnlFooter"
        Me.pnlFooter.Size = New System.Drawing.Size(1013, 197)
        Me.pnlFooter.TabIndex = 5
        '
        'dgvFooter
        '
        Me.dgvFooter.AllowUserToAddRows = False
        Me.dgvFooter.AllowUserToDeleteRows = False
        Me.dgvFooter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvFooter.Dock = System.Windows.Forms.DockStyle.Top
        Me.dgvFooter.Location = New System.Drawing.Point(0, 100)
        Me.dgvFooter.Name = "dgvFooter"
        Me.dgvFooter.ReadOnly = True
        Me.dgvFooter.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal
        Me.dgvFooter.Size = New System.Drawing.Size(1013, 89)
        Me.dgvFooter.TabIndex = 2
        '
        'dgv
        '
        Me.dgv.AllowUserToAddRows = False
        Me.dgv.AllowUserToDeleteRows = False
        Me.dgv.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgv.Dock = System.Windows.Forms.DockStyle.Top
        Me.dgv.Location = New System.Drawing.Point(0, 0)
        Me.dgv.Name = "dgv"
        Me.dgv.ReadOnly = True
        Me.dgv.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.dgv.Size = New System.Drawing.Size(1013, 100)
        Me.dgv.TabIndex = 2
        '
        'tabGridView
        '
        Me.tabGridView.Controls.Add(Me.gridDetailView)
        Me.tabGridView.Controls.Add(Me.Panel2)
        Me.tabGridView.Location = New System.Drawing.Point(4, 25)
        Me.tabGridView.Name = "tabGridView"
        Me.tabGridView.Size = New System.Drawing.Size(1013, 613)
        Me.tabGridView.TabIndex = 5
        Me.tabGridView.Text = "GridView"
        Me.tabGridView.UseVisualStyleBackColor = True
        '
        'gridDetailView
        '
        Me.gridDetailView.AllowUserToAddRows = False
        Me.gridDetailView.AllowUserToDeleteRows = False
        Me.gridDetailView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridDetailView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridDetailView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridDetailView.Location = New System.Drawing.Point(0, 35)
        Me.gridDetailView.Name = "gridDetailView"
        Me.gridDetailView.ReadOnly = True
        Me.gridDetailView.RowHeadersVisible = False
        Me.gridDetailView.RowTemplate.Height = 44
        Me.gridDetailView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridDetailView.Size = New System.Drawing.Size(1013, 578)
        Me.gridDetailView.TabIndex = 0
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.Label28)
        Me.Panel2.Controls.Add(Me.btnPrint)
        Me.Panel2.Controls.Add(Me.btnExport)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(1013, 35)
        Me.Panel2.TabIndex = 1
        '
        'Label28
        '
        Me.Label28.AutoSize = True
        Me.Label28.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label28.ForeColor = System.Drawing.Color.Red
        Me.Label28.Location = New System.Drawing.Point(220, 11)
        Me.Label28.Name = "Label28"
        Me.Label28.Size = New System.Drawing.Size(119, 13)
        Me.Label28.TabIndex = 7
        Me.Label28.Text = "Press [S] to Print"
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(114, 2)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 30)
        Me.btnPrint.TabIndex = 1
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(8, 1)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 0
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'chkStudColumn
        '
        Me.chkStudColumn.AutoSize = True
        Me.chkStudColumn.Checked = True
        Me.chkStudColumn.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkStudColumn.Location = New System.Drawing.Point(27, 343)
        Me.chkStudColumn.Name = "chkStudColumn"
        Me.chkStudColumn.Size = New System.Drawing.Size(162, 17)
        Me.chkStudColumn.TabIndex = 41
        Me.chkStudColumn.Text = "Stud Details In columns"
        Me.chkStudColumn.UseVisualStyleBackColor = True
        '
        'CatalogView
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1021, 642)
        Me.ControlBox = False
        Me.Controls.Add(Me.tabMain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "CatalogView"
        Me.Text = "Tag Catalog"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.tabMain.ResumeLayout(False)
        Me.tabGeneral.ResumeLayout(False)
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.pnlContainer.ResumeLayout(False)
        Me.pnlContainer.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.tabView.ResumeLayout(False)
        Me.pnlImage.ResumeLayout(False)
        CType(Me.picImage, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlRightPart.ResumeLayout(False)
        Me.pnlRightPart.PerformLayout()
        CType(Me.dgvStoneDetail, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlFooter.ResumeLayout(False)
        CType(Me.dgvFooter, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgv, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabGridView.ResumeLayout(False)
        CType(Me.gridDetailView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents tabMain As System.Windows.Forms.TabControl
    Friend WithEvents tabGeneral As System.Windows.Forms.TabPage
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents gridDetailView As System.Windows.Forms.DataGridView
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents txtDiaWtTo_Wet As System.Windows.Forms.TextBox
    Friend WithEvents txtGrsWtTo_Wet As System.Windows.Forms.TextBox
    Friend WithEvents txtNetWtTo_Wet As System.Windows.Forms.TextBox
    Friend WithEvents txtDiaWtFrom_Wet As System.Windows.Forms.TextBox
    Friend WithEvents txtGrsWtFrom_Wet As System.Windows.Forms.TextBox
    Friend WithEvents txtNetWtFrom_Wet As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents txtSubItem As System.Windows.Forms.TextBox
    Friend WithEvents txtItem As System.Windows.Forms.TextBox
    Friend WithEvents chkLstSubItem As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkLstItem As System.Windows.Forms.CheckedListBox
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents txtStnWtTo_Wet As System.Windows.Forms.TextBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents txtStnWtFrom_Wet As System.Windows.Forms.TextBox
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents chkLstStoneSubItem As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkLstStoneItem As System.Windows.Forms.CheckedListBox
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tabView As System.Windows.Forms.TabPage
    Friend WithEvents rbtList As System.Windows.Forms.RadioButton
    Friend WithEvents rbtGrid As System.Windows.Forms.RadioButton
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents tabGridView As System.Windows.Forms.TabPage
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents pnlContainer As System.Windows.Forms.Panel
    Friend WithEvents pnlImage As System.Windows.Forms.Panel
    Friend WithEvents picImage As System.Windows.Forms.PictureBox
    Friend WithEvents pnlFooter As System.Windows.Forms.Panel
    Friend WithEvents dgvFooter As System.Windows.Forms.DataGridView
    Friend WithEvents dgv As System.Windows.Forms.DataGridView
    Friend WithEvents pnlRightPart As System.Windows.Forms.Panel
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents lblPcsWt As System.Windows.Forms.Label
    Friend WithEvents dgvStoneDetail As System.Windows.Forms.DataGridView
    Friend WithEvents lblItemId As System.Windows.Forms.Label
    Friend WithEvents lblItemName As System.Windows.Forms.Label
    Friend WithEvents lblSubItem As System.Windows.Forms.Label
    Friend WithEvents lblDiaPcsWt As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents lblItemType As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents btnShopCart As System.Windows.Forms.Button
    Friend WithEvents cmbOrderName_OWN As System.Windows.Forms.ComboBox
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents Label27 As System.Windows.Forms.Label
    Friend WithEvents lblStyleNo As System.Windows.Forms.Label
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents lblCatSno As System.Windows.Forms.Label
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents Label28 As System.Windows.Forms.Label
    Friend WithEvents rbtCrystalView As System.Windows.Forms.RadioButton
    Friend WithEvents chkStudColumn As System.Windows.Forms.CheckBox
End Class
