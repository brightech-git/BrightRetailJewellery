<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmGSTR1A
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.gridView = New System.Windows.Forms.DataGridView()
        Me.ContextMenuStrip2 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AutoReziseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.pnlHeading = New System.Windows.Forms.Panel()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExtToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FindToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.btnView_Search = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.chkhsndesc = New System.Windows.Forms.CheckBox()
        Me.chkb2bprjb = New System.Windows.Forms.CheckBox()
        Me.chkdate = New System.Windows.Forms.CheckBox()
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpbFrom = New BrighttechPack.DatePicker(Me.components)
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.lblFindHelp = New System.Windows.Forms.Label()
        Me.chkGroupByCostcentre = New System.Windows.Forms.CheckBox()
        Me.chkHsnRate = New System.Windows.Forms.CheckBox()
        Me.btnJson = New System.Windows.Forms.Button()
        Me.cmbMetal = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.ChkBillPrefic = New System.Windows.Forms.CheckBox()
        Me.ChkSR = New System.Windows.Forms.CheckBox()
        Me.ChkAdvGst = New System.Windows.Forms.CheckBox()
        Me.dtpFrom = New System.Windows.Forms.DateTimePicker()
        Me.chkCmbCostCentre = New BrighttechPack.CheckedComboBox()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.rbtAll = New System.Windows.Forms.RadioButton()
        Me.rbtSS = New System.Windows.Forms.RadioButton()
        Me.rbtChit = New System.Windows.Forms.RadioButton()
        Me.rbtCDNRUL = New System.Windows.Forms.RadioButton()
        Me.rbtDocs = New System.Windows.Forms.RadioButton()
        Me.rbtHSN = New System.Windows.Forms.RadioButton()
        Me.rbtEXEM = New System.Windows.Forms.RadioButton()
        Me.rbtATADJ = New System.Windows.Forms.RadioButton()
        Me.rbtAT = New System.Windows.Forms.RadioButton()
        Me.rbtExp = New System.Windows.Forms.RadioButton()
        Me.rbtCDNRU = New System.Windows.Forms.RadioButton()
        Me.rbtCDNR = New System.Windows.Forms.RadioButton()
        Me.rbtB2B = New System.Windows.Forms.RadioButton()
        Me.rbtB2CS = New System.Windows.Forms.RadioButton()
        Me.rbtB2CL = New System.Windows.Forms.RadioButton()
        Me.Panel2.SuspendLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip2.SuspendLayout()
        Me.pnlHeading.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.gridView)
        Me.Panel2.Controls.Add(Me.pnlHeading)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 136)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(1028, 480)
        Me.Panel2.TabIndex = 1
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.ContextMenuStrip = Me.ContextMenuStrip2
        Me.gridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView.Location = New System.Drawing.Point(0, 25)
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.RowHeadersVisible = False
        Me.gridView.RowTemplate.Height = 21
        Me.gridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridView.Size = New System.Drawing.Size(1028, 455)
        Me.gridView.TabIndex = 0
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
        '
        'pnlHeading
        '
        Me.pnlHeading.Controls.Add(Me.lblTitle)
        Me.pnlHeading.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlHeading.Location = New System.Drawing.Point(0, 0)
        Me.pnlHeading.Name = "pnlHeading"
        Me.pnlHeading.Size = New System.Drawing.Size(1028, 25)
        Me.pnlHeading.TabIndex = 0
        '
        'lblTitle
        '
        Me.lblTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(1028, 25)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "Title"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.ExtToolStripMenuItem, Me.FindToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(138, 70)
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(137, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ExtToolStripMenuItem
        '
        Me.ExtToolStripMenuItem.Name = "ExtToolStripMenuItem"
        Me.ExtToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExtToolStripMenuItem.Size = New System.Drawing.Size(137, 22)
        Me.ExtToolStripMenuItem.Text = "Exit"
        Me.ExtToolStripMenuItem.Visible = False
        '
        'FindToolStripMenuItem
        '
        Me.FindToolStripMenuItem.Name = "FindToolStripMenuItem"
        Me.FindToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.F), System.Windows.Forms.Keys)
        Me.FindToolStripMenuItem.Size = New System.Drawing.Size(137, 22)
        Me.FindToolStripMenuItem.Text = "Find"
        '
        'btnView_Search
        '
        Me.btnView_Search.Location = New System.Drawing.Point(78, 102)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(100, 30)
        Me.btnView_Search.TabIndex = 15
        Me.btnView_Search.Text = "&View"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(578, 102)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 20
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(178, 102)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 16
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(5, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(67, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Tax Period"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(5, 55)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(76, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Cost Centre"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(278, 102)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 17
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(378, 102)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 30)
        Me.btnPrint.TabIndex = 18
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.chkhsndesc)
        Me.Panel1.Controls.Add(Me.chkb2bprjb)
        Me.Panel1.Controls.Add(Me.chkdate)
        Me.Panel1.Controls.Add(Me.dtpTo)
        Me.Panel1.Controls.Add(Me.dtpbFrom)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.lblFindHelp)
        Me.Panel1.Controls.Add(Me.chkGroupByCostcentre)
        Me.Panel1.Controls.Add(Me.chkHsnRate)
        Me.Panel1.Controls.Add(Me.btnJson)
        Me.Panel1.Controls.Add(Me.cmbMetal)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.ChkBillPrefic)
        Me.Panel1.Controls.Add(Me.ChkSR)
        Me.Panel1.Controls.Add(Me.ChkAdvGst)
        Me.Panel1.Controls.Add(Me.dtpFrom)
        Me.Panel1.Controls.Add(Me.chkCmbCostCentre)
        Me.Panel1.Controls.Add(Me.btnPrint)
        Me.Panel1.Controls.Add(Me.btnExport)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.btnNew)
        Me.Panel1.Controls.Add(Me.btnExit)
        Me.Panel1.Controls.Add(Me.btnView_Search)
        Me.Panel1.Controls.Add(Me.Panel3)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1028, 136)
        Me.Panel1.TabIndex = 0
        '
        'chkhsndesc
        '
        Me.chkhsndesc.AutoSize = True
        Me.chkhsndesc.Location = New System.Drawing.Point(843, 95)
        Me.chkhsndesc.Name = "chkhsndesc"
        Me.chkhsndesc.Size = New System.Drawing.Size(147, 17)
        Me.chkhsndesc.TabIndex = 24
        Me.chkhsndesc.Text = "HSN With Descrpition"
        Me.chkhsndesc.UseVisualStyleBackColor = True
        Me.chkhsndesc.Visible = False
        '
        'chkb2bprjb
        '
        Me.chkb2bprjb.AutoSize = True
        Me.chkb2bprjb.Location = New System.Drawing.Point(843, 113)
        Me.chkb2bprjb.Name = "chkb2bprjb"
        Me.chkb2bprjb.Size = New System.Drawing.Size(270, 17)
        Me.chkb2bprjb.TabIndex = 23
        Me.chkb2bprjb.Text = "B2B Include Purchase Return And Jobwork"
        Me.chkb2bprjb.UseVisualStyleBackColor = True
        Me.chkb2bprjb.Visible = False
        '
        'chkdate
        '
        Me.chkdate.AutoSize = True
        Me.chkdate.Location = New System.Drawing.Point(193, 8)
        Me.chkdate.Name = "chkdate"
        Me.chkdate.Size = New System.Drawing.Size(53, 17)
        Me.chkdate.TabIndex = 2
        Me.chkdate.Text = "Date"
        Me.chkdate.UseVisualStyleBackColor = True
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(193, 28)
        Me.dtpTo.Mask = "##/##/####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(93, 21)
        Me.dtpTo.TabIndex = 5
        Me.dtpTo.Text = "07/03/9998"
        Me.dtpTo.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'dtpbFrom
        '
        Me.dtpbFrom.Location = New System.Drawing.Point(79, 28)
        Me.dtpbFrom.Mask = "##/##/####"
        Me.dtpbFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpbFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpbFrom.Name = "dtpbFrom"
        Me.dtpbFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpbFrom.Size = New System.Drawing.Size(93, 21)
        Me.dtpbFrom.TabIndex = 4
        Me.dtpbFrom.Text = "07/03/9998"
        Me.dtpbFrom.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(173, 32)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(20, 13)
        Me.Label2.TabIndex = 21
        Me.Label2.Text = "To"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(5, 32)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(36, 13)
        Me.Label5.TabIndex = 3
        Me.Label5.Text = "From"
        '
        'lblFindHelp
        '
        Me.lblFindHelp.AutoSize = True
        Me.lblFindHelp.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFindHelp.ForeColor = System.Drawing.Color.Red
        Me.lblFindHelp.Location = New System.Drawing.Point(685, 101)
        Me.lblFindHelp.Name = "lblFindHelp"
        Me.lblFindHelp.Size = New System.Drawing.Size(109, 16)
        Me.lblFindHelp.TabIndex = 22
        Me.lblFindHelp.Text = "Find [Ctrl + F]"
        '
        'chkGroupByCostcentre
        '
        Me.chkGroupByCostcentre.AutoSize = True
        Me.chkGroupByCostcentre.Location = New System.Drawing.Point(689, 117)
        Me.chkGroupByCostcentre.Name = "chkGroupByCostcentre"
        Me.chkGroupByCostcentre.Size = New System.Drawing.Size(143, 17)
        Me.chkGroupByCostcentre.TabIndex = 21
        Me.chkGroupByCostcentre.Text = "Group By CostName"
        Me.chkGroupByCostcentre.UseVisualStyleBackColor = True
        Me.chkGroupByCostcentre.Visible = False
        '
        'chkHsnRate
        '
        Me.chkHsnRate.AutoSize = True
        Me.chkHsnRate.Location = New System.Drawing.Point(843, 77)
        Me.chkHsnRate.Name = "chkHsnRate"
        Me.chkHsnRate.Size = New System.Drawing.Size(107, 17)
        Me.chkHsnRate.TabIndex = 14
        Me.chkHsnRate.Text = "HSN with Rate"
        Me.chkHsnRate.UseVisualStyleBackColor = True
        Me.chkHsnRate.Visible = False
        '
        'btnJson
        '
        Me.btnJson.Location = New System.Drawing.Point(478, 102)
        Me.btnJson.Name = "btnJson"
        Me.btnJson.Size = New System.Drawing.Size(100, 30)
        Me.btnJson.TabIndex = 19
        Me.btnJson.Text = "JSON"
        Me.btnJson.UseVisualStyleBackColor = True
        '
        'cmbMetal
        '
        Me.cmbMetal.FormattingEnabled = True
        Me.cmbMetal.Location = New System.Drawing.Point(79, 77)
        Me.cmbMetal.Name = "cmbMetal"
        Me.cmbMetal.Size = New System.Drawing.Size(207, 21)
        Me.cmbMetal.TabIndex = 9
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(5, 81)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(37, 13)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "Metal"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ChkBillPrefic
        '
        Me.ChkBillPrefic.AutoSize = True
        Me.ChkBillPrefic.Location = New System.Drawing.Point(843, 57)
        Me.ChkBillPrefic.Name = "ChkBillPrefic"
        Me.ChkBillPrefic.Size = New System.Drawing.Size(76, 17)
        Me.ChkBillPrefic.TabIndex = 13
        Me.ChkBillPrefic.Text = "BillPrefix"
        Me.ChkBillPrefic.UseVisualStyleBackColor = True
        Me.ChkBillPrefic.Visible = False
        '
        'ChkSR
        '
        Me.ChkSR.AutoSize = True
        Me.ChkSR.Location = New System.Drawing.Point(843, 37)
        Me.ChkSR.Name = "ChkSR"
        Me.ChkSR.Size = New System.Drawing.Size(124, 17)
        Me.ChkSR.TabIndex = 12
        Me.ChkSR.Text = "Less SalesReturn"
        Me.ChkSR.UseVisualStyleBackColor = True
        Me.ChkSR.Visible = False
        '
        'ChkAdvGst
        '
        Me.ChkAdvGst.AutoSize = True
        Me.ChkAdvGst.Location = New System.Drawing.Point(843, 17)
        Me.ChkAdvGst.Name = "ChkAdvGst"
        Me.ChkAdvGst.Size = New System.Drawing.Size(186, 17)
        Me.ChkAdvGst.TabIndex = 11
        Me.ChkAdvGst.Text = "Advance Adjusted With GST"
        Me.ChkAdvGst.UseVisualStyleBackColor = True
        Me.ChkAdvGst.Visible = False
        '
        'dtpFrom
        '
        Me.dtpFrom.CustomFormat = "MMM-yyyy"
        Me.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpFrom.Location = New System.Drawing.Point(79, 5)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Size = New System.Drawing.Size(99, 21)
        Me.dtpFrom.TabIndex = 1
        '
        'chkCmbCostCentre
        '
        Me.chkCmbCostCentre.CheckOnClick = True
        Me.chkCmbCostCentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCostCentre.DropDownHeight = 1
        Me.chkCmbCostCentre.FormattingEnabled = True
        Me.chkCmbCostCentre.IntegralHeight = False
        Me.chkCmbCostCentre.Location = New System.Drawing.Point(79, 51)
        Me.chkCmbCostCentre.Name = "chkCmbCostCentre"
        Me.chkCmbCostCentre.Size = New System.Drawing.Size(207, 22)
        Me.chkCmbCostCentre.TabIndex = 7
        Me.chkCmbCostCentre.ValueSeparator = ", "
        '
        'Panel3
        '
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel3.Controls.Add(Me.rbtAll)
        Me.Panel3.Controls.Add(Me.rbtSS)
        Me.Panel3.Controls.Add(Me.rbtChit)
        Me.Panel3.Controls.Add(Me.rbtCDNRUL)
        Me.Panel3.Controls.Add(Me.rbtDocs)
        Me.Panel3.Controls.Add(Me.rbtHSN)
        Me.Panel3.Controls.Add(Me.rbtEXEM)
        Me.Panel3.Controls.Add(Me.rbtATADJ)
        Me.Panel3.Controls.Add(Me.rbtAT)
        Me.Panel3.Controls.Add(Me.rbtExp)
        Me.Panel3.Controls.Add(Me.rbtCDNRU)
        Me.Panel3.Controls.Add(Me.rbtCDNR)
        Me.Panel3.Controls.Add(Me.rbtB2B)
        Me.Panel3.Controls.Add(Me.rbtB2CS)
        Me.Panel3.Controls.Add(Me.rbtB2CL)
        Me.Panel3.Location = New System.Drawing.Point(293, 13)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(545, 85)
        Me.Panel3.TabIndex = 10
        '
        'rbtAll
        '
        Me.rbtAll.AutoSize = True
        Me.rbtAll.Location = New System.Drawing.Point(471, 41)
        Me.rbtAll.Name = "rbtAll"
        Me.rbtAll.Size = New System.Drawing.Size(45, 17)
        Me.rbtAll.TabIndex = 14
        Me.rbtAll.Text = "ALL"
        Me.rbtAll.UseVisualStyleBackColor = True
        '
        'rbtSS
        '
        Me.rbtSS.AutoSize = True
        Me.rbtSS.Location = New System.Drawing.Point(300, 41)
        Me.rbtSS.Name = "rbtSS"
        Me.rbtSS.Size = New System.Drawing.Size(139, 17)
        Me.rbtSS.TabIndex = 10
        Me.rbtSS.Text = "AT (Scheme Adjust)"
        Me.rbtSS.UseVisualStyleBackColor = True
        '
        'rbtChit
        '
        Me.rbtChit.AutoSize = True
        Me.rbtChit.Location = New System.Drawing.Point(300, 23)
        Me.rbtChit.Name = "rbtChit"
        Me.rbtChit.Size = New System.Drawing.Size(99, 17)
        Me.rbtChit.TabIndex = 9
        Me.rbtChit.Text = "AT (Scheme)"
        Me.rbtChit.UseVisualStyleBackColor = True
        '
        'rbtCDNRUL
        '
        Me.rbtCDNRUL.AutoSize = True
        Me.rbtCDNRUL.Location = New System.Drawing.Point(117, 23)
        Me.rbtCDNRUL.Name = "rbtCDNRUL"
        Me.rbtCDNRUL.Size = New System.Drawing.Size(182, 17)
        Me.rbtCDNRUL.TabIndex = 5
        Me.rbtCDNRUL.Text = "CDNR (Unregistered Small)"
        Me.rbtCDNRUL.UseVisualStyleBackColor = True
        '
        'rbtDocs
        '
        Me.rbtDocs.AutoSize = True
        Me.rbtDocs.Location = New System.Drawing.Point(471, 23)
        Me.rbtDocs.Name = "rbtDocs"
        Me.rbtDocs.Size = New System.Drawing.Size(60, 17)
        Me.rbtDocs.TabIndex = 13
        Me.rbtDocs.Text = "DOCS"
        Me.rbtDocs.UseVisualStyleBackColor = True
        '
        'rbtHSN
        '
        Me.rbtHSN.AutoSize = True
        Me.rbtHSN.Location = New System.Drawing.Point(471, 6)
        Me.rbtHSN.Name = "rbtHSN"
        Me.rbtHSN.Size = New System.Drawing.Size(49, 17)
        Me.rbtHSN.TabIndex = 12
        Me.rbtHSN.Text = "HSN"
        Me.rbtHSN.UseVisualStyleBackColor = True
        '
        'rbtEXEM
        '
        Me.rbtEXEM.AutoSize = True
        Me.rbtEXEM.Location = New System.Drawing.Point(300, 59)
        Me.rbtEXEM.Name = "rbtEXEM"
        Me.rbtEXEM.Size = New System.Drawing.Size(127, 17)
        Me.rbtEXEM.TabIndex = 11
        Me.rbtEXEM.Text = "EXEM (Exempted)"
        Me.rbtEXEM.UseVisualStyleBackColor = True
        '
        'rbtATADJ
        '
        Me.rbtATADJ.AutoSize = True
        Me.rbtATADJ.Location = New System.Drawing.Point(300, 6)
        Me.rbtATADJ.Name = "rbtATADJ"
        Me.rbtATADJ.Size = New System.Drawing.Size(163, 17)
        Me.rbtATADJ.TabIndex = 8
        Me.rbtATADJ.Text = "ATADJ (Advance Adjust)"
        Me.rbtATADJ.UseVisualStyleBackColor = True
        '
        'rbtAT
        '
        Me.rbtAT.AutoSize = True
        Me.rbtAT.Location = New System.Drawing.Point(117, 59)
        Me.rbtAT.Name = "rbtAT"
        Me.rbtAT.Size = New System.Drawing.Size(102, 17)
        Me.rbtAT.TabIndex = 7
        Me.rbtAT.Text = "AT (Advance)"
        Me.rbtAT.UseVisualStyleBackColor = True
        '
        'rbtExp
        '
        Me.rbtExp.AutoSize = True
        Me.rbtExp.Location = New System.Drawing.Point(117, 41)
        Me.rbtExp.Name = "rbtExp"
        Me.rbtExp.Size = New System.Drawing.Size(104, 17)
        Me.rbtExp.TabIndex = 6
        Me.rbtExp.Text = "EXP (Exports)"
        Me.rbtExp.UseVisualStyleBackColor = True
        '
        'rbtCDNRU
        '
        Me.rbtCDNRU.AutoSize = True
        Me.rbtCDNRU.Location = New System.Drawing.Point(117, 6)
        Me.rbtCDNRU.Name = "rbtCDNRU"
        Me.rbtCDNRU.Size = New System.Drawing.Size(146, 17)
        Me.rbtCDNRU.TabIndex = 4
        Me.rbtCDNRU.Text = "CDNR (Unregistered)"
        Me.rbtCDNRU.UseVisualStyleBackColor = True
        '
        'rbtCDNR
        '
        Me.rbtCDNR.AutoSize = True
        Me.rbtCDNR.Location = New System.Drawing.Point(12, 59)
        Me.rbtCDNR.Name = "rbtCDNR"
        Me.rbtCDNR.Size = New System.Drawing.Size(59, 17)
        Me.rbtCDNR.TabIndex = 3
        Me.rbtCDNR.Text = "CDNR"
        Me.rbtCDNR.UseVisualStyleBackColor = True
        '
        'rbtB2B
        '
        Me.rbtB2B.AutoSize = True
        Me.rbtB2B.Location = New System.Drawing.Point(12, 5)
        Me.rbtB2B.Name = "rbtB2B"
        Me.rbtB2B.Size = New System.Drawing.Size(48, 17)
        Me.rbtB2B.TabIndex = 0
        Me.rbtB2B.Text = "B2B"
        Me.rbtB2B.UseVisualStyleBackColor = True
        '
        'rbtB2CS
        '
        Me.rbtB2CS.AutoSize = True
        Me.rbtB2CS.Checked = True
        Me.rbtB2CS.Location = New System.Drawing.Point(12, 41)
        Me.rbtB2CS.Name = "rbtB2CS"
        Me.rbtB2CS.Size = New System.Drawing.Size(95, 17)
        Me.rbtB2CS.TabIndex = 2
        Me.rbtB2CS.TabStop = True
        Me.rbtB2CS.Text = "B2C (Small)"
        Me.rbtB2CS.UseVisualStyleBackColor = True
        '
        'rbtB2CL
        '
        Me.rbtB2CL.AutoSize = True
        Me.rbtB2CL.Location = New System.Drawing.Point(12, 23)
        Me.rbtB2CL.Name = "rbtB2CL"
        Me.rbtB2CL.Size = New System.Drawing.Size(95, 17)
        Me.rbtB2CL.TabIndex = 1
        Me.rbtB2CL.Text = "B2C (Large)"
        Me.rbtB2CL.UseVisualStyleBackColor = True
        '
        'frmGSTR1A
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1028, 616)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmGSTR1A"
        Me.Text = "GSTR1"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.Panel2.ResumeLayout(False)
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip2.ResumeLayout(False)
        Me.pnlHeading.ResumeLayout(False)
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents pnlHeading As System.Windows.Forms.Panel
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExtToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnView_Search As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents chkCmbCostCentre As BrighttechPack.CheckedComboBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents ContextMenuStrip2 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents AutoReziseToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents rbtCDNRU As System.Windows.Forms.RadioButton
    Friend WithEvents rbtCDNR As System.Windows.Forms.RadioButton
    Friend WithEvents rbtB2B As System.Windows.Forms.RadioButton
    Friend WithEvents rbtB2CS As System.Windows.Forms.RadioButton
    Friend WithEvents rbtB2CL As System.Windows.Forms.RadioButton
    Friend WithEvents rbtATADJ As System.Windows.Forms.RadioButton
    Friend WithEvents rbtAT As System.Windows.Forms.RadioButton
    Friend WithEvents rbtExp As System.Windows.Forms.RadioButton
    Friend WithEvents rbtDocs As System.Windows.Forms.RadioButton
    Friend WithEvents rbtHSN As System.Windows.Forms.RadioButton
    Friend WithEvents rbtEXEM As System.Windows.Forms.RadioButton
    Friend WithEvents dtpFrom As System.Windows.Forms.DateTimePicker
    Friend WithEvents rbtCDNRUL As System.Windows.Forms.RadioButton
    Friend WithEvents ChkAdvGst As System.Windows.Forms.CheckBox
    Friend WithEvents ChkSR As System.Windows.Forms.CheckBox
    Friend WithEvents rbtSS As System.Windows.Forms.RadioButton
    Friend WithEvents rbtChit As System.Windows.Forms.RadioButton
    Friend WithEvents ChkBillPrefic As CheckBox
    Friend WithEvents rbtAll As RadioButton
    Friend WithEvents cmbMetal As ComboBox
    Friend WithEvents Label4 As Label
    Friend WithEvents btnJson As Button
    Friend WithEvents chkHsnRate As CheckBox
    Friend WithEvents chkGroupByCostcentre As CheckBox
    Friend WithEvents lblFindHelp As Label
    Friend WithEvents FindToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents chkdate As CheckBox
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents dtpbFrom As BrighttechPack.DatePicker
    Friend WithEvents Label2 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents chkb2bprjb As CheckBox
    Friend WithEvents chkhsndesc As CheckBox
End Class
