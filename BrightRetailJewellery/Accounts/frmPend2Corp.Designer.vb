<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPend2Corp
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
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.cmbCostCentre_MAN = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.gridView = New System.Windows.Forms.DataGridView()
        Me.btnTransfer = New System.Windows.Forms.Button()
        Me.lblFromDate = New System.Windows.Forms.Label()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tabMain = New System.Windows.Forms.TabControl()
        Me.tabGeneral = New System.Windows.Forms.TabPage()
        Me.PnlBody = New System.Windows.Forms.Panel()
        Me.gbSummary = New System.Windows.Forms.GroupBox()
        Me.dgvexcel1 = New System.Windows.Forms.DataGridView()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.DgTrfSummary = New System.Windows.Forms.DataGridView()
        Me.DgvView = New System.Windows.Forms.DataGridView()
        Me.pnlTop = New System.Windows.Forms.Panel()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.CmbMetal_MAN = New System.Windows.Forms.ComboBox()
        Me.chkCmbCategory = New BrighttechPack.CheckedComboBox()
        Me.Label30 = New System.Windows.Forms.Label()
        Me.lblToDate = New System.Windows.Forms.Label()
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.CmbCompany_MAN = New System.Windows.Forms.ComboBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.lblHelp = New System.Windows.Forms.Label()
        Me.ChKSAL = New System.Windows.Forms.CheckBox()
        Me.ChkSr = New System.Windows.Forms.CheckBox()
        Me.Chkpurchase = New System.Windows.Forms.CheckBox()
        Me.ChkWithMisc = New System.Windows.Forms.CheckBox()
        Me.ChkAll = New System.Windows.Forms.CheckBox()
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.lblTrsDate = New System.Windows.Forms.Label()
        Me.PnlBottom = New System.Windows.Forms.Panel()
        Me.gridViewtotal = New System.Windows.Forms.DataGridView()
        Me.tabView = New System.Windows.Forms.TabPage()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txtGrid = New System.Windows.Forms.TextBox()
        Me.btnBack = New System.Windows.Forms.Button()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.RadioButton1 = New System.Windows.Forms.RadioButton()
        Me.RadioButton2 = New System.Windows.Forms.RadioButton()
        Me.RadioButton3 = New System.Windows.Forms.RadioButton()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.ComboBox2 = New System.Windows.Forms.ComboBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.DatePicker1 = New BrighttechPack.DatePicker(Me.components)
        Me.DatePicker2 = New BrighttechPack.DatePicker(Me.components)
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.tabMain.SuspendLayout()
        Me.tabGeneral.SuspendLayout()
        Me.PnlBody.SuspendLayout()
        Me.gbSummary.SuspendLayout()
        CType(Me.dgvexcel1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DgTrfSummary, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DgvView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlTop.SuspendLayout()
        Me.PnlBottom.SuspendLayout()
        CType(Me.gridViewtotal, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabView.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(784, 109)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 21
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(484, 109)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 19
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(384, 109)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 18
        Me.btnSearch.Text = "&Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'cmbCostCentre_MAN
        '
        Me.cmbCostCentre_MAN.FormattingEnabled = True
        Me.cmbCostCentre_MAN.Location = New System.Drawing.Point(88, 69)
        Me.cmbCostCentre_MAN.Name = "cmbCostCentre_MAN"
        Me.cmbCostCentre_MAN.Size = New System.Drawing.Size(290, 21)
        Me.cmbCostCentre_MAN.TabIndex = 7
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 73)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(76, 13)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Cost Centre"
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Location = New System.Drawing.Point(8, 6)
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.Size = New System.Drawing.Size(834, 382)
        Me.gridView.TabIndex = 13
        '
        'btnTransfer
        '
        Me.btnTransfer.Enabled = False
        Me.btnTransfer.Location = New System.Drawing.Point(584, 109)
        Me.btnTransfer.Name = "btnTransfer"
        Me.btnTransfer.Size = New System.Drawing.Size(100, 30)
        Me.btnTransfer.TabIndex = 20
        Me.btnTransfer.Text = "&Transfer"
        Me.btnTransfer.UseVisualStyleBackColor = True
        '
        'lblFromDate
        '
        Me.lblFromDate.AutoSize = True
        Me.lblFromDate.Location = New System.Drawing.Point(12, 25)
        Me.lblFromDate.Name = "lblFromDate"
        Me.lblFromDate.Size = New System.Drawing.Size(72, 13)
        Me.lblFromDate.TabIndex = 0
        Me.lblFromDate.Text = "As On Date"
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(119, 48)
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(118, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(118, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'tabMain
        '
        Me.tabMain.Controls.Add(Me.tabGeneral)
        Me.tabMain.Controls.Add(Me.tabView)
        Me.tabMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabMain.Location = New System.Drawing.Point(0, 0)
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(934, 621)
        Me.tabMain.TabIndex = 0
        '
        'tabGeneral
        '
        Me.tabGeneral.Controls.Add(Me.PnlBody)
        Me.tabGeneral.Controls.Add(Me.pnlTop)
        Me.tabGeneral.Controls.Add(Me.PnlBottom)
        Me.tabGeneral.Location = New System.Drawing.Point(4, 22)
        Me.tabGeneral.Name = "tabGeneral"
        Me.tabGeneral.Padding = New System.Windows.Forms.Padding(3)
        Me.tabGeneral.Size = New System.Drawing.Size(926, 595)
        Me.tabGeneral.TabIndex = 0
        Me.tabGeneral.Text = "General"
        Me.tabGeneral.UseVisualStyleBackColor = True
        '
        'PnlBody
        '
        Me.PnlBody.Controls.Add(Me.gbSummary)
        Me.PnlBody.Controls.Add(Me.DgvView)
        Me.PnlBody.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PnlBody.Location = New System.Drawing.Point(3, 163)
        Me.PnlBody.Name = "PnlBody"
        Me.PnlBody.Size = New System.Drawing.Size(920, 319)
        Me.PnlBody.TabIndex = 18
        '
        'gbSummary
        '
        Me.gbSummary.BackColor = System.Drawing.Color.Lavender
        Me.gbSummary.Controls.Add(Me.dgvexcel1)
        Me.gbSummary.Controls.Add(Me.Label3)
        Me.gbSummary.Controls.Add(Me.Label4)
        Me.gbSummary.Controls.Add(Me.DgTrfSummary)
        Me.gbSummary.Location = New System.Drawing.Point(212, 6)
        Me.gbSummary.Name = "gbSummary"
        Me.gbSummary.Size = New System.Drawing.Size(688, 307)
        Me.gbSummary.TabIndex = 1
        Me.gbSummary.TabStop = False
        Me.gbSummary.Visible = False
        '
        'dgvexcel1
        '
        Me.dgvexcel1.AllowUserToAddRows = False
        Me.dgvexcel1.AllowUserToDeleteRows = False
        Me.dgvexcel1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvexcel1.Location = New System.Drawing.Point(10, 37)
        Me.dgvexcel1.Name = "dgvexcel1"
        Me.dgvexcel1.ReadOnly = True
        Me.dgvexcel1.Size = New System.Drawing.Size(240, 150)
        Me.dgvexcel1.TabIndex = 3
        Me.dgvexcel1.Visible = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.Red
        Me.Label3.Location = New System.Drawing.Point(5, 286)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(84, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "*Esc to Close"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(249, 13)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(133, 13)
        Me.Label4.TabIndex = 1
        Me.Label4.Text = " Transfer Summary"
        '
        'DgTrfSummary
        '
        Me.DgTrfSummary.AllowUserToAddRows = False
        Me.DgTrfSummary.AllowUserToDeleteRows = False
        Me.DgTrfSummary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DgTrfSummary.Location = New System.Drawing.Point(2, 29)
        Me.DgTrfSummary.Name = "DgTrfSummary"
        Me.DgTrfSummary.ReadOnly = True
        Me.DgTrfSummary.Size = New System.Drawing.Size(684, 252)
        Me.DgTrfSummary.TabIndex = 0
        '
        'DgvView
        '
        Me.DgvView.AllowUserToAddRows = False
        Me.DgvView.AllowUserToDeleteRows = False
        Me.DgvView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DgvView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DgvView.Location = New System.Drawing.Point(0, 0)
        Me.DgvView.Name = "DgvView"
        Me.DgvView.RowHeadersVisible = False
        Me.DgvView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.DgvView.Size = New System.Drawing.Size(920, 319)
        Me.DgvView.TabIndex = 0
        '
        'pnlTop
        '
        Me.pnlTop.Controls.Add(Me.btnExport)
        Me.pnlTop.Controls.Add(Me.CmbMetal_MAN)
        Me.pnlTop.Controls.Add(Me.chkCmbCategory)
        Me.pnlTop.Controls.Add(Me.Label30)
        Me.pnlTop.Controls.Add(Me.lblToDate)
        Me.pnlTop.Controls.Add(Me.dtpTo)
        Me.pnlTop.Controls.Add(Me.CmbCompany_MAN)
        Me.pnlTop.Controls.Add(Me.Label11)
        Me.pnlTop.Controls.Add(Me.Label5)
        Me.pnlTop.Controls.Add(Me.lblHelp)
        Me.pnlTop.Controls.Add(Me.ChKSAL)
        Me.pnlTop.Controls.Add(Me.ChkSr)
        Me.pnlTop.Controls.Add(Me.Chkpurchase)
        Me.pnlTop.Controls.Add(Me.ChkWithMisc)
        Me.pnlTop.Controls.Add(Me.ChkAll)
        Me.pnlTop.Controls.Add(Me.btnTransfer)
        Me.pnlTop.Controls.Add(Me.btnNew)
        Me.pnlTop.Controls.Add(Me.cmbCostCentre_MAN)
        Me.pnlTop.Controls.Add(Me.btnSearch)
        Me.pnlTop.Controls.Add(Me.dtpFrom)
        Me.pnlTop.Controls.Add(Me.lblTrsDate)
        Me.pnlTop.Controls.Add(Me.lblFromDate)
        Me.pnlTop.Controls.Add(Me.btnExit)
        Me.pnlTop.Controls.Add(Me.Label1)
        Me.pnlTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlTop.Location = New System.Drawing.Point(3, 3)
        Me.pnlTop.Name = "pnlTop"
        Me.pnlTop.Size = New System.Drawing.Size(920, 160)
        Me.pnlTop.TabIndex = 0
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(684, 109)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 23
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'CmbMetal_MAN
        '
        Me.CmbMetal_MAN.FormattingEnabled = True
        Me.CmbMetal_MAN.Location = New System.Drawing.Point(88, 94)
        Me.CmbMetal_MAN.Name = "CmbMetal_MAN"
        Me.CmbMetal_MAN.Size = New System.Drawing.Size(290, 21)
        Me.CmbMetal_MAN.TabIndex = 9
        '
        'chkCmbCategory
        '
        Me.chkCmbCategory.CheckOnClick = True
        Me.chkCmbCategory.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkCmbCategory.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCategory.DropDownHeight = 1
        Me.chkCmbCategory.FormattingEnabled = True
        Me.chkCmbCategory.IntegralHeight = False
        Me.chkCmbCategory.Location = New System.Drawing.Point(88, 117)
        Me.chkCmbCategory.Name = "chkCmbCategory"
        Me.chkCmbCategory.Size = New System.Drawing.Size(290, 22)
        Me.chkCmbCategory.TabIndex = 11
        Me.chkCmbCategory.ValueSeparator = ", "
        '
        'Label30
        '
        Me.Label30.AutoSize = True
        Me.Label30.Location = New System.Drawing.Point(12, 122)
        Me.Label30.Name = "Label30"
        Me.Label30.Size = New System.Drawing.Size(60, 13)
        Me.Label30.TabIndex = 10
        Me.Label30.Text = "Category"
        Me.Label30.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblToDate
        '
        Me.lblToDate.AutoSize = True
        Me.lblToDate.Location = New System.Drawing.Point(174, 25)
        Me.lblToDate.Name = "lblToDate"
        Me.lblToDate.Size = New System.Drawing.Size(51, 13)
        Me.lblToDate.TabIndex = 2
        Me.lblToDate.Text = "To Date"
        Me.lblToDate.Visible = False
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(232, 21)
        Me.dtpTo.Mask = "##/##/####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(78, 21)
        Me.dtpTo.TabIndex = 3
        Me.dtpTo.Text = "06/03/9998"
        Me.dtpTo.Value = New Date(9998, 3, 6, 0, 0, 0, 0)
        Me.dtpTo.Visible = False
        '
        'CmbCompany_MAN
        '
        Me.CmbCompany_MAN.FormattingEnabled = True
        Me.CmbCompany_MAN.Location = New System.Drawing.Point(88, 45)
        Me.CmbCompany_MAN.Name = "CmbCompany_MAN"
        Me.CmbCompany_MAN.Size = New System.Drawing.Size(290, 21)
        Me.CmbCompany_MAN.TabIndex = 5
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(12, 49)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(62, 13)
        Me.Label11.TabIndex = 4
        Me.Label11.Text = "Company"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(12, 97)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(37, 13)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "Metal"
        '
        'lblHelp
        '
        Me.lblHelp.AutoSize = True
        Me.lblHelp.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHelp.ForeColor = System.Drawing.Color.Red
        Me.lblHelp.Location = New System.Drawing.Point(762, 9)
        Me.lblHelp.Name = "lblHelp"
        Me.lblHelp.Size = New System.Drawing.Size(152, 13)
        Me.lblHelp.TabIndex = 22
        Me.lblHelp.Text = "[Ctrl+S]  for Summary"
        Me.lblHelp.Visible = False
        '
        'ChKSAL
        '
        Me.ChKSAL.AutoSize = True
        Me.ChKSAL.Location = New System.Drawing.Point(763, 71)
        Me.ChKSAL.Name = "ChKSAL"
        Me.ChKSAL.Size = New System.Drawing.Size(124, 17)
        Me.ChKSAL.TabIndex = 17
        Me.ChKSAL.Text = "With Party Sales "
        Me.ChKSAL.UseVisualStyleBackColor = True
        '
        'ChkSr
        '
        Me.ChkSr.AutoSize = True
        Me.ChkSr.Location = New System.Drawing.Point(508, 71)
        Me.ChkSr.Name = "ChkSr"
        Me.ChkSr.Size = New System.Drawing.Size(128, 17)
        Me.ChkSr.TabIndex = 15
        Me.ChkSr.Text = "With Sales Return"
        Me.ChkSr.UseVisualStyleBackColor = True
        '
        'Chkpurchase
        '
        Me.Chkpurchase.AutoSize = True
        Me.Chkpurchase.Location = New System.Drawing.Point(646, 71)
        Me.Chkpurchase.Name = "Chkpurchase"
        Me.Chkpurchase.Size = New System.Drawing.Size(107, 17)
        Me.Chkpurchase.TabIndex = 16
        Me.Chkpurchase.Text = "With Purchase"
        Me.Chkpurchase.UseVisualStyleBackColor = True
        '
        'ChkWithMisc
        '
        Me.ChkWithMisc.AutoSize = True
        Me.ChkWithMisc.Location = New System.Drawing.Point(384, 71)
        Me.ChkWithMisc.Name = "ChkWithMisc"
        Me.ChkWithMisc.Size = New System.Drawing.Size(114, 17)
        Me.ChkWithMisc.TabIndex = 14
        Me.ChkWithMisc.Text = "With Misc Issue"
        Me.ChkWithMisc.UseVisualStyleBackColor = True
        '
        'ChkAll
        '
        Me.ChkAll.AutoSize = True
        Me.ChkAll.Location = New System.Drawing.Point(384, 47)
        Me.ChkAll.Name = "ChkAll"
        Me.ChkAll.Size = New System.Drawing.Size(80, 17)
        Me.ChkAll.TabIndex = 13
        Me.ChkAll.Text = "Check All"
        Me.ChkAll.UseVisualStyleBackColor = True
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(88, 21)
        Me.dtpFrom.Mask = "##/##/####"
        Me.dtpFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom.Size = New System.Drawing.Size(78, 21)
        Me.dtpFrom.TabIndex = 1
        Me.dtpFrom.Text = "06/03/9998"
        Me.dtpFrom.Value = New Date(9998, 3, 6, 0, 0, 0, 0)
        '
        'lblTrsDate
        '
        Me.lblTrsDate.AutoSize = True
        Me.lblTrsDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTrsDate.Location = New System.Drawing.Point(384, 25)
        Me.lblTrsDate.Name = "lblTrsDate"
        Me.lblTrsDate.Size = New System.Drawing.Size(75, 13)
        Me.lblTrsDate.TabIndex = 12
        Me.lblTrsDate.Text = "Date From"
        '
        'PnlBottom
        '
        Me.PnlBottom.Controls.Add(Me.gridViewtotal)
        Me.PnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.PnlBottom.Location = New System.Drawing.Point(3, 482)
        Me.PnlBottom.Name = "PnlBottom"
        Me.PnlBottom.Size = New System.Drawing.Size(920, 110)
        Me.PnlBottom.TabIndex = 19
        '
        'gridViewtotal
        '
        Me.gridViewtotal.AllowUserToAddRows = False
        Me.gridViewtotal.AllowUserToDeleteRows = False
        Me.gridViewtotal.BackgroundColor = System.Drawing.SystemColors.ControlDark
        Me.gridViewtotal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.gridViewtotal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridViewtotal.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridViewtotal.Location = New System.Drawing.Point(0, 0)
        Me.gridViewtotal.MultiSelect = False
        Me.gridViewtotal.Name = "gridViewtotal"
        Me.gridViewtotal.ReadOnly = True
        Me.gridViewtotal.RowHeadersVisible = False
        Me.gridViewtotal.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridViewtotal.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal
        Me.gridViewtotal.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridViewtotal.Size = New System.Drawing.Size(920, 110)
        Me.gridViewtotal.TabIndex = 0
        '
        'tabView
        '
        Me.tabView.Controls.Add(Me.Label10)
        Me.tabView.Controls.Add(Me.txtGrid)
        Me.tabView.Controls.Add(Me.btnBack)
        Me.tabView.Controls.Add(Me.gridView)
        Me.tabView.Location = New System.Drawing.Point(4, 22)
        Me.tabView.Name = "tabView"
        Me.tabView.Padding = New System.Windows.Forms.Padding(3)
        Me.tabView.Size = New System.Drawing.Size(926, 595)
        Me.tabView.TabIndex = 1
        Me.tabView.Text = "View"
        Me.tabView.UseVisualStyleBackColor = True
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(598, 421)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(151, 13)
        Me.Label10.TabIndex = 21
        Me.Label10.Text = "S - Transfer Summary"
        '
        'txtGrid
        '
        Me.txtGrid.Location = New System.Drawing.Point(217, 394)
        Me.txtGrid.Name = "txtGrid"
        Me.txtGrid.Size = New System.Drawing.Size(125, 21)
        Me.txtGrid.TabIndex = 18
        Me.txtGrid.Visible = False
        '
        'btnBack
        '
        Me.btnBack.Location = New System.Drawing.Point(8, 412)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(100, 30)
        Me.btnBack.TabIndex = 2
        Me.btnBack.Text = "&Back"
        Me.btnBack.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(121, 155)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(85, 21)
        Me.Label6.TabIndex = 12
        Me.Label6.Text = "Metal"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox1
        '
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Location = New System.Drawing.Point(217, 155)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(280, 21)
        Me.ComboBox1.TabIndex = 13
        '
        'RadioButton1
        '
        Me.RadioButton1.AutoSize = True
        Me.RadioButton1.Location = New System.Drawing.Point(218, 291)
        Me.RadioButton1.Name = "RadioButton1"
        Me.RadioButton1.Size = New System.Drawing.Size(70, 17)
        Me.RadioButton1.TabIndex = 8
        Me.RadioButton1.TabStop = True
        Me.RadioButton1.Text = "Purchase"
        Me.RadioButton1.UseVisualStyleBackColor = True
        '
        'RadioButton2
        '
        Me.RadioButton2.AutoSize = True
        Me.RadioButton2.Location = New System.Drawing.Point(218, 257)
        Me.RadioButton2.Name = "RadioButton2"
        Me.RadioButton2.Size = New System.Drawing.Size(86, 17)
        Me.RadioButton2.TabIndex = 7
        Me.RadioButton2.TabStop = True
        Me.RadioButton2.Text = "Sales Return"
        Me.RadioButton2.UseVisualStyleBackColor = True
        '
        'RadioButton3
        '
        Me.RadioButton3.AutoSize = True
        Me.RadioButton3.Location = New System.Drawing.Point(218, 223)
        Me.RadioButton3.Name = "RadioButton3"
        Me.RadioButton3.Size = New System.Drawing.Size(80, 17)
        Me.RadioButton3.TabIndex = 6
        Me.RadioButton3.TabStop = True
        Me.RadioButton3.Text = "Partly Sales"
        Me.RadioButton3.UseVisualStyleBackColor = True
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(122, 89)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(56, 13)
        Me.Label7.TabIndex = 0
        Me.Label7.Text = "Date From"
        '
        'ComboBox2
        '
        Me.ComboBox2.FormattingEnabled = True
        Me.ComboBox2.Location = New System.Drawing.Point(218, 119)
        Me.ComboBox2.Name = "ComboBox2"
        Me.ComboBox2.Size = New System.Drawing.Size(279, 21)
        Me.ComboBox2.TabIndex = 5
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(122, 123)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(62, 13)
        Me.Label8.TabIndex = 4
        Me.Label8.Text = "Cost Centre"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(430, 326)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(100, 30)
        Me.Button1.TabIndex = 11
        Me.Button1.Text = "Exit [F12]"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(318, 89)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(20, 13)
        Me.Label9.TabIndex = 2
        Me.Label9.Text = "To"
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(324, 326)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(100, 30)
        Me.Button2.TabIndex = 10
        Me.Button2.Text = "New [F3]"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(218, 326)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(100, 30)
        Me.Button3.TabIndex = 9
        Me.Button3.Text = "&Search"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'DatePicker1
        '
        Me.DatePicker1.Location = New System.Drawing.Point(345, 85)
        Me.DatePicker1.Mask = "##/##/####"
        Me.DatePicker1.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.DatePicker1.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.DatePicker1.Name = "DatePicker1"
        Me.DatePicker1.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.DatePicker1.Size = New System.Drawing.Size(94, 20)
        Me.DatePicker1.TabIndex = 3
        Me.DatePicker1.Text = "06/03/9998"
        Me.DatePicker1.Value = New Date(9998, 3, 6, 0, 0, 0, 0)
        '
        'DatePicker2
        '
        Me.DatePicker2.Location = New System.Drawing.Point(217, 85)
        Me.DatePicker2.Mask = "##/##/####"
        Me.DatePicker2.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.DatePicker2.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.DatePicker2.Name = "DatePicker2"
        Me.DatePicker2.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.DatePicker2.Size = New System.Drawing.Size(94, 20)
        Me.DatePicker2.TabIndex = 1
        Me.DatePicker2.Text = "06/03/9998"
        Me.DatePicker2.Value = New Date(9998, 3, 6, 0, 0, 0, 0)
        '
        'frmPend2Corp
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(934, 621)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.tabMain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmPend2Corp"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Pending Transfer"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.tabMain.ResumeLayout(False)
        Me.tabGeneral.ResumeLayout(False)
        Me.PnlBody.ResumeLayout(False)
        Me.gbSummary.ResumeLayout(False)
        Me.gbSummary.PerformLayout()
        CType(Me.dgvexcel1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DgTrfSummary, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DgvView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlTop.ResumeLayout(False)
        Me.pnlTop.PerformLayout()
        Me.PnlBottom.ResumeLayout(False)
        CType(Me.gridViewtotal, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabView.ResumeLayout(False)
        Me.tabView.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents cmbCostCentre_MAN As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents btnTransfer As System.Windows.Forms.Button
    Friend WithEvents lblFromDate As System.Windows.Forms.Label
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tabMain As System.Windows.Forms.TabControl
    Friend WithEvents tabGeneral As System.Windows.Forms.TabPage
    Friend WithEvents tabView As System.Windows.Forms.TabPage
    Friend WithEvents btnBack As System.Windows.Forms.Button
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents DatePicker1 As BrighttechPack.DatePicker
    Friend WithEvents DatePicker2 As BrighttechPack.DatePicker
    Friend WithEvents RadioButton1 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton2 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton3 As System.Windows.Forms.RadioButton
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents ComboBox2 As System.Windows.Forms.ComboBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents txtGrid As System.Windows.Forms.TextBox
    Friend WithEvents gbSummary As System.Windows.Forms.GroupBox
    Friend WithEvents DgTrfSummary As System.Windows.Forms.DataGridView
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents lblTrsDate As System.Windows.Forms.Label
    Friend WithEvents pnlTop As System.Windows.Forms.Panel
    Friend WithEvents PnlBody As System.Windows.Forms.Panel
    Friend WithEvents DgvView As System.Windows.Forms.DataGridView
    Friend WithEvents ChkAll As System.Windows.Forms.CheckBox
    Friend WithEvents PnlBottom As System.Windows.Forms.Panel
    Friend WithEvents ChkWithMisc As System.Windows.Forms.CheckBox
    Friend WithEvents ChkSr As System.Windows.Forms.CheckBox
    Friend WithEvents Chkpurchase As System.Windows.Forms.CheckBox
    Friend WithEvents ChKSAL As System.Windows.Forms.CheckBox
    Friend WithEvents gridViewtotal As System.Windows.Forms.DataGridView
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents lblHelp As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents CmbCompany_MAN As System.Windows.Forms.ComboBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents lblToDate As System.Windows.Forms.Label
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents chkCmbCategory As BrighttechPack.CheckedComboBox
    Friend WithEvents Label30 As System.Windows.Forms.Label
    Friend WithEvents CmbMetal_MAN As System.Windows.Forms.ComboBox
    Friend WithEvents btnExport As Button
    Friend WithEvents dgvexcel1 As DataGridView
End Class
