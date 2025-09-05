<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmDealerstudded
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtbx_itemid = New System.Windows.Forms.TextBox()
        Me.txtbx_stoneid = New System.Windows.Forms.TextBox()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.btnOpen = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.cmbobx_item_OWN = New System.Windows.Forms.ComboBox()
        Me.cmbobx_subitm_OWN = New System.Windows.Forms.ComboBox()
        Me.cmbobx_stn_OWN = New System.Windows.Forms.ComboBox()
        Me.cmbobx_stnsubitm_OWN = New System.Windows.Forms.ComboBox()
        Me.tabmain = New System.Windows.Forms.TabControl()
        Me.tabgenral = New System.Windows.Forms.TabPage()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtbx_stnrate = New System.Windows.Forms.TextBox()
        Me.cmbobx_Acname_OWN = New System.Windows.Forms.ComboBox()
        Me.cmbobx_unit = New System.Windows.Forms.ComboBox()
        Me.cmbobx_calmode = New System.Windows.Forms.ComboBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Tabview = New System.Windows.Forms.TabPage()
        Me.gridview_detail = New System.Windows.Forms.DataGridView()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.cmbobxAcNametab2_OWN = New System.Windows.Forms.ComboBox()
        Me.cmbobxItmNametab2_OWN = New System.Windows.Forms.ComboBox()
        Me.btnBack = New System.Windows.Forms.Button()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Label81 = New System.Windows.Forms.Label()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContextMenuStrip2 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AutoReziseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tabmain.SuspendLayout()
        Me.tabgenral.SuspendLayout()
        Me.Tabview.SuspendLayout()
        CType(Me.gridview_detail, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.ContextMenuStrip2.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(206, 157)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(34, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Item"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(206, 193)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(60, 13)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Sub Item"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(206, 230)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(40, 13)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "Stone"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(206, 266)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(97, 13)
        Me.Label4.TabIndex = 11
        Me.Label4.Text = "Stone Sub Item"
        '
        'txtbx_itemid
        '
        Me.txtbx_itemid.Location = New System.Drawing.Point(308, 153)
        Me.txtbx_itemid.Name = "txtbx_itemid"
        Me.txtbx_itemid.Size = New System.Drawing.Size(97, 21)
        Me.txtbx_itemid.TabIndex = 3
        '
        'txtbx_stoneid
        '
        Me.txtbx_stoneid.Location = New System.Drawing.Point(308, 226)
        Me.txtbx_stoneid.Name = "txtbx_stoneid"
        Me.txtbx_stoneid.Size = New System.Drawing.Size(97, 21)
        Me.txtbx_stoneid.TabIndex = 9
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(519, 403)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(98, 30)
        Me.btnNew.TabIndex = 22
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnOpen
        '
        Me.btnOpen.Location = New System.Drawing.Point(413, 403)
        Me.btnOpen.Name = "btnOpen"
        Me.btnOpen.Size = New System.Drawing.Size(98, 30)
        Me.btnOpen.TabIndex = 21
        Me.btnOpen.Text = "Open [F2]"
        Me.btnOpen.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(625, 403)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(98, 30)
        Me.btnExit.TabIndex = 23
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(307, 403)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(98, 30)
        Me.btnSave.TabIndex = 20
        Me.btnSave.Text = "Save [F1]"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'cmbobx_item_OWN
        '
        Me.cmbobx_item_OWN.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cmbobx_item_OWN.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbobx_item_OWN.FormattingEnabled = True
        Me.cmbobx_item_OWN.Location = New System.Drawing.Point(413, 153)
        Me.cmbobx_item_OWN.Name = "cmbobx_item_OWN"
        Me.cmbobx_item_OWN.Size = New System.Drawing.Size(343, 21)
        Me.cmbobx_item_OWN.TabIndex = 4
        '
        'cmbobx_subitm_OWN
        '
        Me.cmbobx_subitm_OWN.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cmbobx_subitm_OWN.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbobx_subitm_OWN.FormattingEnabled = True
        Me.cmbobx_subitm_OWN.Location = New System.Drawing.Point(308, 189)
        Me.cmbobx_subitm_OWN.Name = "cmbobx_subitm_OWN"
        Me.cmbobx_subitm_OWN.Size = New System.Drawing.Size(448, 21)
        Me.cmbobx_subitm_OWN.TabIndex = 7
        '
        'cmbobx_stn_OWN
        '
        Me.cmbobx_stn_OWN.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cmbobx_stn_OWN.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbobx_stn_OWN.FormattingEnabled = True
        Me.cmbobx_stn_OWN.Location = New System.Drawing.Point(413, 226)
        Me.cmbobx_stn_OWN.Name = "cmbobx_stn_OWN"
        Me.cmbobx_stn_OWN.Size = New System.Drawing.Size(343, 21)
        Me.cmbobx_stn_OWN.TabIndex = 10
        '
        'cmbobx_stnsubitm_OWN
        '
        Me.cmbobx_stnsubitm_OWN.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cmbobx_stnsubitm_OWN.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbobx_stnsubitm_OWN.FormattingEnabled = True
        Me.cmbobx_stnsubitm_OWN.Location = New System.Drawing.Point(308, 262)
        Me.cmbobx_stnsubitm_OWN.Name = "cmbobx_stnsubitm_OWN"
        Me.cmbobx_stnsubitm_OWN.Size = New System.Drawing.Size(448, 21)
        Me.cmbobx_stnsubitm_OWN.TabIndex = 13
        '
        'tabmain
        '
        Me.tabmain.Controls.Add(Me.tabgenral)
        Me.tabmain.Controls.Add(Me.Tabview)
        Me.tabmain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabmain.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabmain.Location = New System.Drawing.Point(0, 0)
        Me.tabmain.Name = "tabmain"
        Me.tabmain.SelectedIndex = 0
        Me.tabmain.Size = New System.Drawing.Size(1005, 574)
        Me.tabmain.TabIndex = 0
        '
        'tabgenral
        '
        Me.tabgenral.Controls.Add(Me.Label8)
        Me.tabgenral.Controls.Add(Me.txtbx_stnrate)
        Me.tabgenral.Controls.Add(Me.cmbobx_Acname_OWN)
        Me.tabgenral.Controls.Add(Me.cmbobx_unit)
        Me.tabgenral.Controls.Add(Me.cmbobx_calmode)
        Me.tabgenral.Controls.Add(Me.Label7)
        Me.tabgenral.Controls.Add(Me.Label6)
        Me.tabgenral.Controls.Add(Me.Label5)
        Me.tabgenral.Controls.Add(Me.cmbobx_stnsubitm_OWN)
        Me.tabgenral.Controls.Add(Me.btnNew)
        Me.tabgenral.Controls.Add(Me.Label1)
        Me.tabgenral.Controls.Add(Me.btnOpen)
        Me.tabgenral.Controls.Add(Me.cmbobx_stn_OWN)
        Me.tabgenral.Controls.Add(Me.btnExit)
        Me.tabgenral.Controls.Add(Me.btnSave)
        Me.tabgenral.Controls.Add(Me.Label2)
        Me.tabgenral.Controls.Add(Me.cmbobx_subitm_OWN)
        Me.tabgenral.Controls.Add(Me.Label3)
        Me.tabgenral.Controls.Add(Me.cmbobx_item_OWN)
        Me.tabgenral.Controls.Add(Me.Label4)
        Me.tabgenral.Controls.Add(Me.txtbx_itemid)
        Me.tabgenral.Controls.Add(Me.txtbx_stoneid)
        Me.tabgenral.Location = New System.Drawing.Point(4, 22)
        Me.tabgenral.Name = "tabgenral"
        Me.tabgenral.Padding = New System.Windows.Forms.Padding(3)
        Me.tabgenral.Size = New System.Drawing.Size(855, 548)
        Me.tabgenral.TabIndex = 0
        Me.tabgenral.Text = "TabPage1"
        Me.tabgenral.UseVisualStyleBackColor = True
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(206, 366)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(70, 13)
        Me.Label8.TabIndex = 18
        Me.Label8.Text = "Stone Rate"
        '
        'txtbx_stnrate
        '
        Me.txtbx_stnrate.Location = New System.Drawing.Point(308, 362)
        Me.txtbx_stnrate.Name = "txtbx_stnrate"
        Me.txtbx_stnrate.Size = New System.Drawing.Size(97, 21)
        Me.txtbx_stnrate.TabIndex = 19
        '
        'cmbobx_Acname_OWN
        '
        Me.cmbobx_Acname_OWN.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cmbobx_Acname_OWN.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbobx_Acname_OWN.FormattingEnabled = True
        Me.cmbobx_Acname_OWN.Location = New System.Drawing.Point(308, 115)
        Me.cmbobx_Acname_OWN.Name = "cmbobx_Acname_OWN"
        Me.cmbobx_Acname_OWN.Size = New System.Drawing.Size(448, 21)
        Me.cmbobx_Acname_OWN.TabIndex = 1
        '
        'cmbobx_unit
        '
        Me.cmbobx_unit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbobx_unit.FormattingEnabled = True
        Me.cmbobx_unit.Items.AddRange(New Object() {"CARAT", "GRAM"})
        Me.cmbobx_unit.Location = New System.Drawing.Point(308, 329)
        Me.cmbobx_unit.Name = "cmbobx_unit"
        Me.cmbobx_unit.Size = New System.Drawing.Size(97, 21)
        Me.cmbobx_unit.TabIndex = 17
        '
        'cmbobx_calmode
        '
        Me.cmbobx_calmode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbobx_calmode.FormattingEnabled = True
        Me.cmbobx_calmode.Items.AddRange(New Object() {"WEIGHT", "PIECE"})
        Me.cmbobx_calmode.Location = New System.Drawing.Point(308, 296)
        Me.cmbobx_calmode.Name = "cmbobx_calmode"
        Me.cmbobx_calmode.Size = New System.Drawing.Size(97, 21)
        Me.cmbobx_calmode.TabIndex = 15
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(206, 333)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(29, 13)
        Me.Label7.TabIndex = 16
        Me.Label7.Text = "Unit"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(206, 300)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(66, 13)
        Me.Label6.TabIndex = 14
        Me.Label6.Text = "Calc Mode"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(206, 119)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(58, 13)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "Ac Name"
        '
        'Tabview
        '
        Me.Tabview.Controls.Add(Me.gridview_detail)
        Me.Tabview.Controls.Add(Me.Panel1)
        Me.Tabview.Controls.Add(Me.Panel2)
        Me.Tabview.Location = New System.Drawing.Point(4, 22)
        Me.Tabview.Name = "Tabview"
        Me.Tabview.Padding = New System.Windows.Forms.Padding(3)
        Me.Tabview.Size = New System.Drawing.Size(997, 548)
        Me.Tabview.TabIndex = 1
        Me.Tabview.Text = "TabPage2"
        Me.Tabview.UseVisualStyleBackColor = True
        '
        'gridview_detail
        '
        Me.gridview_detail.AllowUserToAddRows = False
        Me.gridview_detail.AllowUserToDeleteRows = False
        Me.gridview_detail.AllowUserToResizeColumns = False
        Me.gridview_detail.AllowUserToResizeRows = False
        Me.gridview_detail.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.gridview_detail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridview_detail.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridview_detail.Location = New System.Drawing.Point(3, 119)
        Me.gridview_detail.Name = "gridview_detail"
        Me.gridview_detail.Size = New System.Drawing.Size(991, 397)
        Me.gridview_detail.TabIndex = 1
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.btnPrint)
        Me.Panel1.Controls.Add(Me.Label10)
        Me.Panel1.Controls.Add(Me.Label9)
        Me.Panel1.Controls.Add(Me.cmbobxAcNametab2_OWN)
        Me.Panel1.Controls.Add(Me.cmbobxItmNametab2_OWN)
        Me.Panel1.Controls.Add(Me.btnBack)
        Me.Panel1.Controls.Add(Me.btnExport)
        Me.Panel1.Controls.Add(Me.btnSearch)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(3, 3)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(991, 116)
        Me.Panel1.TabIndex = 0
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(346, 69)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 30)
        Me.btnPrint.TabIndex = 7
        Me.btnPrint.Text = "Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(571, 20)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(71, 13)
        Me.Label10.TabIndex = 3
        Me.Label10.Text = "Item Name"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(131, 20)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(58, 13)
        Me.Label9.TabIndex = 1
        Me.Label9.Text = "Ac Name"
        '
        'cmbobxAcNametab2_OWN
        '
        Me.cmbobxAcNametab2_OWN.FormattingEnabled = True
        Me.cmbobxAcNametab2_OWN.Location = New System.Drawing.Point(134, 36)
        Me.cmbobxAcNametab2_OWN.Name = "cmbobxAcNametab2_OWN"
        Me.cmbobxAcNametab2_OWN.Size = New System.Drawing.Size(434, 21)
        Me.cmbobxAcNametab2_OWN.TabIndex = 2
        Me.cmbobxAcNametab2_OWN.Text = "ALL"
        '
        'cmbobxItmNametab2_OWN
        '
        Me.cmbobxItmNametab2_OWN.FormattingEnabled = True
        Me.cmbobxItmNametab2_OWN.Location = New System.Drawing.Point(574, 36)
        Me.cmbobxItmNametab2_OWN.Name = "cmbobxItmNametab2_OWN"
        Me.cmbobxItmNametab2_OWN.Size = New System.Drawing.Size(372, 21)
        Me.cmbobxItmNametab2_OWN.TabIndex = 4
        Me.cmbobxItmNametab2_OWN.Text = "ALL"
        '
        'btnBack
        '
        Me.btnBack.Location = New System.Drawing.Point(6, 36)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(100, 30)
        Me.btnBack.TabIndex = 0
        Me.btnBack.Text = "&Back"
        Me.btnBack.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnExport.Location = New System.Drawing.Point(240, 69)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 6
        Me.btnExport.Text = "&Export"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(134, 69)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 5
        Me.btnSearch.Text = "&Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.Label81)
        Me.Panel2.Controls.Add(Me.lblStatus)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel2.Location = New System.Drawing.Point(3, 516)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(991, 29)
        Me.Panel2.TabIndex = 2
        '
        'Label81
        '
        Me.Label81.AutoSize = True
        Me.Label81.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label81.ForeColor = System.Drawing.Color.Red
        Me.Label81.Location = New System.Drawing.Point(3, 9)
        Me.Label81.Name = "Label81"
        Me.Label81.Size = New System.Drawing.Size(153, 13)
        Me.Label81.TabIndex = 0
        Me.Label81.Text = "*Press Escape to Back"
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.ForeColor = System.Drawing.Color.Red
        Me.lblStatus.Location = New System.Drawing.Point(153, 9)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(103, 13)
        Me.lblStatus.TabIndex = 1
        Me.lblStatus.Text = "*Hit Enter to Edit"
        Me.lblStatus.Visible = False
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
        'ContextMenuStrip2
        '
        Me.ContextMenuStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AutoReziseToolStripMenuItem})
        Me.ContextMenuStrip2.Name = "ContextMenuStrip2"
        Me.ContextMenuStrip2.Size = New System.Drawing.Size(133, 26)
        '
        'AutoReziseToolStripMenuItem
        '
        Me.AutoReziseToolStripMenuItem.CheckOnClick = True
        Me.AutoReziseToolStripMenuItem.Name = "AutoReziseToolStripMenuItem"
        Me.AutoReziseToolStripMenuItem.Size = New System.Drawing.Size(132, 22)
        Me.AutoReziseToolStripMenuItem.Text = "AutoRezise"
        Me.AutoReziseToolStripMenuItem.Visible = False
        '
        'frmDealerstudded
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1005, 574)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.tabmain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmDealerstudded"
        Me.Text = "DEALER STUDDED"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.tabmain.ResumeLayout(False)
        Me.tabgenral.ResumeLayout(False)
        Me.tabgenral.PerformLayout()
        Me.Tabview.ResumeLayout(False)
        CType(Me.gridview_detail, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ContextMenuStrip2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents txtbx_itemid As TextBox
    Friend WithEvents txtbx_stoneid As TextBox
    Friend WithEvents btnNew As Button
    Friend WithEvents btnOpen As Button
    Friend WithEvents btnExit As Button
    Friend WithEvents btnSave As Button
    Friend WithEvents cmbobx_item_OWN As ComboBox
    Friend WithEvents cmbobx_subitm_OWN As ComboBox
    Friend WithEvents cmbobx_stn_OWN As ComboBox
    Friend WithEvents cmbobx_stnsubitm_OWN As ComboBox
    Friend WithEvents tabmain As TabControl
    Friend WithEvents tabgenral As TabPage
    Friend WithEvents Tabview As TabPage
    Friend WithEvents Label8 As Label
    Friend WithEvents txtbx_stnrate As TextBox
    Friend WithEvents cmbobx_Acname_OWN As ComboBox
    Friend WithEvents cmbobx_unit As ComboBox
    Friend WithEvents cmbobx_calmode As ComboBox
    Friend WithEvents Label7 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents ContextMenuStrip1 As ContextMenuStrip
    Friend WithEvents SaveToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents OpenToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents gridview_detail As DataGridView
    Friend WithEvents Panel1 As Panel
    Friend WithEvents btnBack As Button
    Friend WithEvents btnExport As Button
    Friend WithEvents btnSearch As Button
    Friend WithEvents Label81 As Label
    Friend WithEvents lblStatus As Label
    Friend WithEvents Panel2 As Panel
    Friend WithEvents cmbobxAcNametab2_OWN As ComboBox
    Friend WithEvents cmbobxItmNametab2_OWN As ComboBox
    Friend WithEvents Label10 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents ContextMenuStrip2 As ContextMenuStrip
    Friend WithEvents AutoReziseToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents btnPrint As Button
End Class
