<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMeltingReceipt
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmbSmith_OWN = New System.Windows.Forms.ComboBox()
        Me.txtBagNo = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtPurity_PER = New System.Windows.Forms.TextBox()
        Me.txtWeight_WET = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtRemark1 = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtRemark2 = New System.Windows.Forms.TextBox()
        Me.lblBullionRate = New System.Windows.Forms.Label()
        Me.txtRate_AMT = New System.Windows.Forms.TextBox()
        Me.GridView = New System.Windows.Forms.DataGridView()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.gridViewPendingBagNo = New System.Windows.Forms.DataGridView()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.btnOpen = New System.Windows.Forms.Button()
        Me.tabMain = New System.Windows.Forms.TabControl()
        Me.tabGeneral = New System.Windows.Forms.TabPage()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.CmbCompany_OWN = New System.Windows.Forms.ComboBox()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.txtLess_WET = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lblMetal = New System.Windows.Forms.Label()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.txtPurewt_WET = New System.Windows.Forms.TextBox()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.txtScrapwt_WET = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.txtTouchper = New System.Windows.Forms.TextBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.txtReceivedWt_WET = New System.Windows.Forms.TextBox()
        Me.txtSampleWt_WET = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtNetWt_WET = New System.Windows.Forms.TextBox()
        Me.dtpDate = New BrighttechPack.DatePicker(Me.components)
        Me.cmbReceivedCat = New System.Windows.Forms.ComboBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.txtMcharge_AMT = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txtWastage_WET = New System.Windows.Forms.TextBox()
        Me.tabView = New System.Windows.Forms.TabPage()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.btnBAck = New System.Windows.Forms.Button()
        Me.gridViewOpen = New System.Windows.Forms.DataGridView()
        CType(Me.GridView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridViewPendingBagNo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.tabMain.SuspendLayout()
        Me.tabGeneral.SuspendLayout()
        Me.tabView.SuspendLayout()
        CType(Me.gridViewOpen, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 65)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(77, 13)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Smith Name"
        '
        'cmbSmith_OWN
        '
        Me.cmbSmith_OWN.FormattingEnabled = True
        Me.cmbSmith_OWN.Location = New System.Drawing.Point(136, 61)
        Me.cmbSmith_OWN.Name = "cmbSmith_OWN"
        Me.cmbSmith_OWN.Size = New System.Drawing.Size(345, 21)
        Me.cmbSmith_OWN.TabIndex = 6
        '
        'txtBagNo
        '
        Me.txtBagNo.Location = New System.Drawing.Point(136, 87)
        Me.txtBagNo.Name = "txtBagNo"
        Me.txtBagNo.Size = New System.Drawing.Size(74, 21)
        Me.txtBagNo.TabIndex = 9
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 91)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(48, 13)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "Bag No"
        '
        'txtPurity_PER
        '
        Me.txtPurity_PER.Location = New System.Drawing.Point(136, 139)
        Me.txtPurity_PER.Name = "txtPurity_PER"
        Me.txtPurity_PER.Size = New System.Drawing.Size(74, 21)
        Me.txtPurity_PER.TabIndex = 13
        '
        'txtWeight_WET
        '
        Me.txtWeight_WET.Location = New System.Drawing.Point(136, 165)
        Me.txtWeight_WET.Name = "txtWeight_WET"
        Me.txtWeight_WET.Size = New System.Drawing.Size(74, 21)
        Me.txtWeight_WET.TabIndex = 17
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(6, 13)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(34, 13)
        Me.Label9.TabIndex = 0
        Me.Label9.Text = "Date"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(6, 143)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(40, 13)
        Me.Label4.TabIndex = 12
        Me.Label4.Text = "Purity"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(6, 169)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(108, 13)
        Me.Label6.TabIndex = 16
        Me.Label6.Text = "Bag Gross Weight"
        '
        'txtRemark1
        '
        Me.txtRemark1.Location = New System.Drawing.Point(136, 347)
        Me.txtRemark1.MaxLength = 50
        Me.txtRemark1.Name = "txtRemark1"
        Me.txtRemark1.Size = New System.Drawing.Size(345, 21)
        Me.txtRemark1.TabIndex = 39
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(6, 351)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(52, 13)
        Me.Label7.TabIndex = 38
        Me.Label7.Text = "Remark"
        '
        'txtRemark2
        '
        Me.txtRemark2.Location = New System.Drawing.Point(136, 373)
        Me.txtRemark2.MaxLength = 50
        Me.txtRemark2.Name = "txtRemark2"
        Me.txtRemark2.Size = New System.Drawing.Size(345, 21)
        Me.txtRemark2.TabIndex = 40
        '
        'lblBullionRate
        '
        Me.lblBullionRate.AutoSize = True
        Me.lblBullionRate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBullionRate.ForeColor = System.Drawing.Color.Red
        Me.lblBullionRate.Location = New System.Drawing.Point(380, 9)
        Me.lblBullionRate.Name = "lblBullionRate"
        Me.lblBullionRate.Size = New System.Drawing.Size(84, 13)
        Me.lblBullionRate.TabIndex = 2
        Me.lblBullionRate.Text = "Bullion Rate"
        '
        'txtRate_AMT
        '
        Me.txtRate_AMT.Location = New System.Drawing.Point(390, 295)
        Me.txtRate_AMT.Name = "txtRate_AMT"
        Me.txtRate_AMT.Size = New System.Drawing.Size(74, 21)
        Me.txtRate_AMT.TabIndex = 36
        '
        'GridView
        '
        Me.GridView.AllowUserToAddRows = False
        Me.GridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GridView.Location = New System.Drawing.Point(9, 438)
        Me.GridView.Name = "GridView"
        Me.GridView.ReadOnly = True
        Me.GridView.Size = New System.Drawing.Size(846, 112)
        Me.GridView.TabIndex = 46
        '
        'btnAdd
        '
        Me.btnAdd.Location = New System.Drawing.Point(135, 402)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(100, 30)
        Me.btnAdd.TabIndex = 41
        Me.btnAdd.Text = "&Add"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(241, 402)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(100, 30)
        Me.btnSave.TabIndex = 42
        Me.btnSave.Text = "Save [F1]"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(452, 402)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 44
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(557, 402)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 45
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'gridViewPendingBagNo
        '
        Me.gridViewPendingBagNo.AllowUserToAddRows = False
        Me.gridViewPendingBagNo.AllowUserToDeleteRows = False
        Me.gridViewPendingBagNo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridViewPendingBagNo.Location = New System.Drawing.Point(487, 20)
        Me.gridViewPendingBagNo.Name = "gridViewPendingBagNo"
        Me.gridViewPendingBagNo.ReadOnly = True
        Me.gridViewPendingBagNo.RowHeadersVisible = False
        Me.gridViewPendingBagNo.Size = New System.Drawing.Size(368, 244)
        Me.gridViewPendingBagNo.TabIndex = 47
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SaveToolStripMenuItem, Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem, Me.OpenToolStripMenuItem})
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
        'OpenToolStripMenuItem
        '
        Me.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem"
        Me.OpenToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2
        Me.OpenToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.OpenToolStripMenuItem.Text = "Open"
        '
        'btnOpen
        '
        Me.btnOpen.Location = New System.Drawing.Point(346, 402)
        Me.btnOpen.Name = "btnOpen"
        Me.btnOpen.Size = New System.Drawing.Size(100, 30)
        Me.btnOpen.TabIndex = 43
        Me.btnOpen.Text = "Open [F2]"
        Me.btnOpen.UseVisualStyleBackColor = True
        '
        'tabMain
        '
        Me.tabMain.Controls.Add(Me.tabGeneral)
        Me.tabMain.Controls.Add(Me.tabView)
        Me.tabMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabMain.Location = New System.Drawing.Point(0, 0)
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(887, 612)
        Me.tabMain.TabIndex = 0
        '
        'tabGeneral
        '
        Me.tabGeneral.Controls.Add(Me.Label22)
        Me.tabGeneral.Controls.Add(Me.Label21)
        Me.tabGeneral.Controls.Add(Me.CmbCompany_OWN)
        Me.tabGeneral.Controls.Add(Me.Label19)
        Me.tabGeneral.Controls.Add(Me.txtLess_WET)
        Me.tabGeneral.Controls.Add(Me.Label3)
        Me.tabGeneral.Controls.Add(Me.lblMetal)
        Me.tabGeneral.Controls.Add(Me.Label18)
        Me.tabGeneral.Controls.Add(Me.txtPurewt_WET)
        Me.tabGeneral.Controls.Add(Me.Label17)
        Me.tabGeneral.Controls.Add(Me.txtScrapwt_WET)
        Me.tabGeneral.Controls.Add(Me.Label5)
        Me.tabGeneral.Controls.Add(Me.Label16)
        Me.tabGeneral.Controls.Add(Me.txtTouchper)
        Me.tabGeneral.Controls.Add(Me.Label15)
        Me.tabGeneral.Controls.Add(Me.txtReceivedWt_WET)
        Me.tabGeneral.Controls.Add(Me.txtSampleWt_WET)
        Me.tabGeneral.Controls.Add(Me.Label8)
        Me.tabGeneral.Controls.Add(Me.txtNetWt_WET)
        Me.tabGeneral.Controls.Add(Me.dtpDate)
        Me.tabGeneral.Controls.Add(Me.Label9)
        Me.tabGeneral.Controls.Add(Me.GridView)
        Me.tabGeneral.Controls.Add(Me.cmbReceivedCat)
        Me.tabGeneral.Controls.Add(Me.cmbSmith_OWN)
        Me.tabGeneral.Controls.Add(Me.btnOpen)
        Me.tabGeneral.Controls.Add(Me.Label2)
        Me.tabGeneral.Controls.Add(Me.gridViewPendingBagNo)
        Me.tabGeneral.Controls.Add(Me.btnSave)
        Me.tabGeneral.Controls.Add(Me.Label4)
        Me.tabGeneral.Controls.Add(Me.btnExit)
        Me.tabGeneral.Controls.Add(Me.Label14)
        Me.tabGeneral.Controls.Add(Me.btnNew)
        Me.tabGeneral.Controls.Add(Me.Label12)
        Me.tabGeneral.Controls.Add(Me.lblBullionRate)
        Me.tabGeneral.Controls.Add(Me.Label13)
        Me.tabGeneral.Controls.Add(Me.btnAdd)
        Me.tabGeneral.Controls.Add(Me.Label6)
        Me.tabGeneral.Controls.Add(Me.txtMcharge_AMT)
        Me.tabGeneral.Controls.Add(Me.Label7)
        Me.tabGeneral.Controls.Add(Me.txtWeight_WET)
        Me.tabGeneral.Controls.Add(Me.txtBagNo)
        Me.tabGeneral.Controls.Add(Me.txtRemark2)
        Me.tabGeneral.Controls.Add(Me.Label11)
        Me.tabGeneral.Controls.Add(Me.Label1)
        Me.tabGeneral.Controls.Add(Me.txtPurity_PER)
        Me.tabGeneral.Controls.Add(Me.txtRemark1)
        Me.tabGeneral.Controls.Add(Me.txtWastage_WET)
        Me.tabGeneral.Controls.Add(Me.txtRate_AMT)
        Me.tabGeneral.Location = New System.Drawing.Point(4, 22)
        Me.tabGeneral.Name = "tabGeneral"
        Me.tabGeneral.Padding = New System.Windows.Forms.Padding(3)
        Me.tabGeneral.Size = New System.Drawing.Size(879, 586)
        Me.tabGeneral.TabIndex = 0
        Me.tabGeneral.Text = "General"
        Me.tabGeneral.UseVisualStyleBackColor = True
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Location = New System.Drawing.Point(216, 221)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(76, 13)
        Me.Label22.TabIndex = 25
        Me.Label22.Text = "(Testing Wt)"
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Location = New System.Drawing.Point(216, 195)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(62, 13)
        Me.Label21.TabIndex = 22
        Me.Label21.Text = "(Dust Wt)"
        '
        'CmbCompany_OWN
        '
        Me.CmbCompany_OWN.FormattingEnabled = True
        Me.CmbCompany_OWN.Location = New System.Drawing.Point(136, 35)
        Me.CmbCompany_OWN.Name = "CmbCompany_OWN"
        Me.CmbCompany_OWN.Size = New System.Drawing.Size(344, 21)
        Me.CmbCompany_OWN.TabIndex = 4
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Location = New System.Drawing.Point(6, 39)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(62, 13)
        Me.Label19.TabIndex = 3
        Me.Label19.Text = "Company"
        '
        'txtLess_WET
        '
        Me.txtLess_WET.Location = New System.Drawing.Point(390, 139)
        Me.txtLess_WET.Name = "txtLess_WET"
        Me.txtLess_WET.Size = New System.Drawing.Size(74, 21)
        Me.txtLess_WET.TabIndex = 15
        Me.txtLess_WET.Text = "0.000"
        Me.txtLess_WET.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(292, 143)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(92, 13)
        Me.Label3.TabIndex = 14
        Me.Label3.Text = "Stone/Less WT"
        '
        'lblMetal
        '
        Me.lblMetal.AutoSize = True
        Me.lblMetal.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMetal.ForeColor = System.Drawing.Color.Red
        Me.lblMetal.Location = New System.Drawing.Point(380, 69)
        Me.lblMetal.Name = "lblMetal"
        Me.lblMetal.Size = New System.Drawing.Size(0, 13)
        Me.lblMetal.TabIndex = 7
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(6, 325)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(75, 13)
        Me.Label18.TabIndex = 32
        Me.Label18.Text = "Pure Weight"
        '
        'txtPurewt_WET
        '
        Me.txtPurewt_WET.Location = New System.Drawing.Point(136, 321)
        Me.txtPurewt_WET.Name = "txtPurewt_WET"
        Me.txtPurewt_WET.Size = New System.Drawing.Size(74, 21)
        Me.txtPurewt_WET.TabIndex = 33
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(6, 247)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(82, 13)
        Me.Label17.TabIndex = 26
        Me.Label17.Text = "Scrap Weight"
        '
        'txtScrapwt_WET
        '
        Me.txtScrapwt_WET.Location = New System.Drawing.Point(136, 243)
        Me.txtScrapwt_WET.Name = "txtScrapwt_WET"
        Me.txtScrapwt_WET.Size = New System.Drawing.Size(74, 21)
        Me.txtScrapwt_WET.TabIndex = 27
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(6, 221)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(92, 13)
        Me.Label5.TabIndex = 23
        Me.Label5.Text = "Sample Weight"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(292, 169)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(56, 13)
        Me.Label16.TabIndex = 18
        Me.Label16.Text = "Touch %"
        '
        'txtTouchper
        '
        Me.txtTouchper.Location = New System.Drawing.Point(390, 165)
        Me.txtTouchper.Name = "txtTouchper"
        Me.txtTouchper.Size = New System.Drawing.Size(74, 21)
        Me.txtTouchper.TabIndex = 19
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(6, 273)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(109, 13)
        Me.Label15.TabIndex = 28
        Me.Label15.Text = "Recd. Grs. Weight"
        '
        'txtReceivedWt_WET
        '
        Me.txtReceivedWt_WET.Location = New System.Drawing.Point(136, 269)
        Me.txtReceivedWt_WET.Name = "txtReceivedWt_WET"
        Me.txtReceivedWt_WET.Size = New System.Drawing.Size(74, 21)
        Me.txtReceivedWt_WET.TabIndex = 29
        '
        'txtSampleWt_WET
        '
        Me.txtSampleWt_WET.Location = New System.Drawing.Point(136, 217)
        Me.txtSampleWt_WET.Name = "txtSampleWt_WET"
        Me.txtSampleWt_WET.Size = New System.Drawing.Size(74, 21)
        Me.txtSampleWt_WET.TabIndex = 24
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(6, 299)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(68, 13)
        Me.Label8.TabIndex = 30
        Me.Label8.Text = "Net Weight"
        '
        'txtNetWt_WET
        '
        Me.txtNetWt_WET.Location = New System.Drawing.Point(136, 295)
        Me.txtNetWt_WET.Name = "txtNetWt_WET"
        Me.txtNetWt_WET.Size = New System.Drawing.Size(74, 21)
        Me.txtNetWt_WET.TabIndex = 31
        '
        'dtpDate
        '
        Me.dtpDate.Location = New System.Drawing.Point(136, 9)
        Me.dtpDate.Mask = "##/##/####"
        Me.dtpDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpDate.Name = "dtpDate"
        Me.dtpDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpDate.Size = New System.Drawing.Size(100, 21)
        Me.dtpDate.TabIndex = 1
        Me.dtpDate.Text = "06/03/9998"
        Me.dtpDate.Value = New Date(9998, 3, 6, 0, 0, 0, 0)
        '
        'cmbReceivedCat
        '
        Me.cmbReceivedCat.FormattingEnabled = True
        Me.cmbReceivedCat.Location = New System.Drawing.Point(136, 113)
        Me.cmbReceivedCat.Name = "cmbReceivedCat"
        Me.cmbReceivedCat.Size = New System.Drawing.Size(345, 21)
        Me.cmbReceivedCat.TabIndex = 11
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(6, 195)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(129, 13)
        Me.Label14.TabIndex = 20
        Me.Label14.Text = "Melting Loss/Wastage"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(292, 299)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(33, 13)
        Me.Label12.TabIndex = 34
        Me.Label12.Text = "Rate"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(292, 325)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(55, 13)
        Me.Label13.TabIndex = 35
        Me.Label13.Text = "Mcharge"
        '
        'txtMcharge_AMT
        '
        Me.txtMcharge_AMT.Location = New System.Drawing.Point(390, 321)
        Me.txtMcharge_AMT.Name = "txtMcharge_AMT"
        Me.txtMcharge_AMT.Size = New System.Drawing.Size(74, 21)
        Me.txtMcharge_AMT.TabIndex = 37
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(6, 117)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(116, 13)
        Me.Label11.TabIndex = 10
        Me.Label11.Text = "Received Category"
        '
        'txtWastage_WET
        '
        Me.txtWastage_WET.Location = New System.Drawing.Point(136, 191)
        Me.txtWastage_WET.Name = "txtWastage_WET"
        Me.txtWastage_WET.Size = New System.Drawing.Size(74, 21)
        Me.txtWastage_WET.TabIndex = 21
        '
        'tabView
        '
        Me.tabView.Controls.Add(Me.Label20)
        Me.tabView.Controls.Add(Me.btnPrint)
        Me.tabView.Controls.Add(Me.btnExport)
        Me.tabView.Controls.Add(Me.Label10)
        Me.tabView.Controls.Add(Me.btnBAck)
        Me.tabView.Controls.Add(Me.gridViewOpen)
        Me.tabView.Location = New System.Drawing.Point(4, 22)
        Me.tabView.Name = "tabView"
        Me.tabView.Padding = New System.Windows.Forms.Padding(3)
        Me.tabView.Size = New System.Drawing.Size(873, 586)
        Me.tabView.TabIndex = 1
        Me.tabView.Text = "View"
        Me.tabView.UseVisualStyleBackColor = True
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label20.ForeColor = System.Drawing.Color.Red
        Me.Label20.Location = New System.Drawing.Point(116, 458)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(89, 13)
        Me.Label20.TabIndex = 5
        Me.Label20.Text = "[ENTER] Edit"
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(217, 425)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 30)
        Me.btnPrint.TabIndex = 4
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(112, 425)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 3
        Me.btnExport.Text = "Export[X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.Color.Red
        Me.Label10.Location = New System.Drawing.Point(8, 458)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(74, 13)
        Me.Label10.TabIndex = 2
        Me.Label10.Text = "[C] Cancel"
        '
        'btnBAck
        '
        Me.btnBAck.Location = New System.Drawing.Point(8, 425)
        Me.btnBAck.Name = "btnBAck"
        Me.btnBAck.Size = New System.Drawing.Size(100, 30)
        Me.btnBAck.TabIndex = 1
        Me.btnBAck.Text = "&Back"
        Me.btnBAck.UseVisualStyleBackColor = True
        '
        'gridViewOpen
        '
        Me.gridViewOpen.AllowUserToAddRows = False
        Me.gridViewOpen.AllowUserToDeleteRows = False
        Me.gridViewOpen.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridViewOpen.Location = New System.Drawing.Point(8, 6)
        Me.gridViewOpen.Name = "gridViewOpen"
        Me.gridViewOpen.ReadOnly = True
        Me.gridViewOpen.Size = New System.Drawing.Size(871, 413)
        Me.gridViewOpen.TabIndex = 0
        '
        'frmMeltingReceipt
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(887, 612)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.tabMain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmMeltingReceipt"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Melting Receipt"
        CType(Me.GridView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridViewPendingBagNo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.tabMain.ResumeLayout(False)
        Me.tabGeneral.ResumeLayout(False)
        Me.tabGeneral.PerformLayout()
        Me.tabView.ResumeLayout(False)
        Me.tabView.PerformLayout()
        CType(Me.gridViewOpen, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents txtBagNo As System.Windows.Forms.TextBox
    Friend WithEvents cmbSmith_OWN As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtPurity_PER As System.Windows.Forms.TextBox
    Friend WithEvents txtWeight_WET As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtRemark1 As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtRemark2 As System.Windows.Forms.TextBox
    Friend WithEvents lblBullionRate As System.Windows.Forms.Label
    Friend WithEvents txtRate_AMT As System.Windows.Forms.TextBox
    Friend WithEvents GridView As System.Windows.Forms.DataGridView
    Friend WithEvents btnAdd As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents gridViewPendingBagNo As System.Windows.Forms.DataGridView
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnOpen As System.Windows.Forms.Button
    Friend WithEvents tabMain As System.Windows.Forms.TabControl
    Friend WithEvents tabGeneral As System.Windows.Forms.TabPage
    Friend WithEvents tabView As System.Windows.Forms.TabPage
    Friend WithEvents btnBAck As System.Windows.Forms.Button
    Friend WithEvents gridViewOpen As System.Windows.Forms.DataGridView
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents cmbReceivedCat As System.Windows.Forms.ComboBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents txtMcharge_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtWastage_WET As System.Windows.Forms.TextBox
    Friend WithEvents dtpDate As BrighttechPack.DatePicker
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtNetWt_WET As System.Windows.Forms.TextBox
    Friend WithEvents txtSampleWt_WET As System.Windows.Forms.TextBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents txtReceivedWt_WET As System.Windows.Forms.TextBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents txtTouchper As System.Windows.Forms.TextBox
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents txtScrapwt_WET As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents txtPurewt_WET As System.Windows.Forms.TextBox
    Friend WithEvents lblMetal As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtLess_WET As System.Windows.Forms.TextBox
    Friend WithEvents CmbCompany_OWN As System.Windows.Forms.ComboBox
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents Label20 As Label
    Friend WithEvents Label21 As Label
    Friend WithEvents Label22 As Label
End Class
