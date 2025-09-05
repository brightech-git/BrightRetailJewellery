<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAgeWiseAnalysis
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
        Me.pnlMain = New System.Windows.Forms.Panel()
        Me.tabMain = New System.Windows.Forms.TabControl()
        Me.tabGen = New System.Windows.Forms.TabPage()
        Me.pnlInput = New System.Windows.Forms.Panel()
        Me.grprangedetail = New System.Windows.Forms.GroupBox()
        Me.gridrange = New System.Windows.Forms.DataGridView()
        Me.grpControls = New System.Windows.Forms.GroupBox()
        Me.chkDetails = New System.Windows.Forms.CheckBox()
        Me.chkCmbCompany = New BrighttechPack.CheckedComboBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.chkCmbtablecode = New BrighttechPack.CheckedComboBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.chkCounterWise = New System.Windows.Forms.CheckBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.chkitemgrp = New System.Windows.Forms.CheckBox()
        Me.ChkDesignerGrp = New System.Windows.Forms.CheckBox()
        Me.Chktablegrpwise = New System.Windows.Forms.CheckBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.cmbmetal = New System.Windows.Forms.ComboBox()
        Me.chkrangefrmmaster = New System.Windows.Forms.CheckBox()
        Me.rangepanel = New System.Windows.Forms.Panel()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtFromDay1_NUM = New System.Windows.Forms.TextBox()
        Me.txtFromDay3_NUM = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txtToDay1_NUM = New System.Windows.Forms.TextBox()
        Me.txtToDay3_NUM = New System.Windows.Forms.TextBox()
        Me.txtFromDay2_NUM = New System.Windows.Forms.TextBox()
        Me.txtFromDay4_NUM = New System.Windows.Forms.TextBox()
        Me.txtToDay2_NUM = New System.Windows.Forms.TextBox()
        Me.txtToDay4_NUM = New System.Windows.Forms.TextBox()
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.lblto = New System.Windows.Forms.Label()
        Me.chkason = New System.Windows.Forms.CheckBox()
        Me.chkActualDate = New System.Windows.Forms.CheckBox()
        Me.chkSubItem = New System.Windows.Forms.CheckBox()
        Me.dtpAsOnDate = New BrighttechPack.DatePicker(Me.components)
        Me.btnNew = New System.Windows.Forms.Button()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnView_Search = New System.Windows.Forms.Button()
        Me.chkWithStone = New System.Windows.Forms.CheckBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.cmbCategory = New System.Windows.Forms.ComboBox()
        Me.cmbCostCentre = New System.Windows.Forms.ComboBox()
        Me.cmbSubItem = New System.Windows.Forms.ComboBox()
        Me.cmbDesigner = New System.Windows.Forms.ComboBox()
        Me.cmbCounter = New System.Windows.Forms.ComboBox()
        Me.cmbItem = New System.Windows.Forms.ComboBox()
        Me.tabView = New System.Windows.Forms.TabPage()
        Me.pnlView = New System.Windows.Forms.Panel()
        Me.pnlFooter = New System.Windows.Forms.Panel()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.btnBack = New System.Windows.Forms.Button()
        Me.pnlGridView = New System.Windows.Forms.Panel()
        Me.pnlgrid = New System.Windows.Forms.Panel()
        Me.gridView_OWN = New System.Windows.Forms.DataGridView()
        Me.ContextMenuStrip2 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ResizeToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.gridViewHead = New System.Windows.Forms.DataGridView()
        Me.pnlTitle = New System.Windows.Forms.Panel()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ResizeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.pnlMain.SuspendLayout()
        Me.tabMain.SuspendLayout()
        Me.tabGen.SuspendLayout()
        Me.pnlInput.SuspendLayout()
        Me.grprangedetail.SuspendLayout()
        CType(Me.gridrange, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpControls.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.rangepanel.SuspendLayout()
        Me.tabView.SuspendLayout()
        Me.pnlView.SuspendLayout()
        Me.pnlFooter.SuspendLayout()
        Me.pnlGridView.SuspendLayout()
        Me.pnlgrid.SuspendLayout()
        CType(Me.gridView_OWN, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip2.SuspendLayout()
        CType(Me.gridViewHead, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlTitle.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlMain
        '
        Me.pnlMain.Controls.Add(Me.tabMain)
        Me.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlMain.Location = New System.Drawing.Point(0, 0)
        Me.pnlMain.Name = "pnlMain"
        Me.pnlMain.Size = New System.Drawing.Size(1022, 640)
        Me.pnlMain.TabIndex = 0
        '
        'tabMain
        '
        Me.tabMain.Controls.Add(Me.tabGen)
        Me.tabMain.Controls.Add(Me.tabView)
        Me.tabMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabMain.Location = New System.Drawing.Point(0, 0)
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(1022, 640)
        Me.tabMain.TabIndex = 0
        '
        'tabGen
        '
        Me.tabGen.Controls.Add(Me.pnlInput)
        Me.tabGen.Location = New System.Drawing.Point(4, 22)
        Me.tabGen.Name = "tabGen"
        Me.tabGen.Padding = New System.Windows.Forms.Padding(3)
        Me.tabGen.Size = New System.Drawing.Size(1014, 614)
        Me.tabGen.TabIndex = 0
        Me.tabGen.Text = "Gen"
        Me.tabGen.UseVisualStyleBackColor = True
        '
        'pnlInput
        '
        Me.pnlInput.Controls.Add(Me.grprangedetail)
        Me.pnlInput.Controls.Add(Me.grpControls)
        Me.pnlInput.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlInput.Location = New System.Drawing.Point(3, 3)
        Me.pnlInput.Name = "pnlInput"
        Me.pnlInput.Size = New System.Drawing.Size(1008, 608)
        Me.pnlInput.TabIndex = 0
        '
        'grprangedetail
        '
        Me.grprangedetail.Controls.Add(Me.gridrange)
        Me.grprangedetail.Location = New System.Drawing.Point(757, 27)
        Me.grprangedetail.Name = "grprangedetail"
        Me.grprangedetail.Size = New System.Drawing.Size(200, 251)
        Me.grprangedetail.TabIndex = 1
        Me.grprangedetail.TabStop = False
        Me.grprangedetail.Text = "Available Range"
        '
        'gridrange
        '
        Me.gridrange.AllowUserToAddRows = False
        Me.gridrange.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridrange.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridrange.Location = New System.Drawing.Point(3, 17)
        Me.gridrange.Name = "gridrange"
        Me.gridrange.Size = New System.Drawing.Size(194, 231)
        Me.gridrange.TabIndex = 0
        '
        'grpControls
        '
        Me.grpControls.Controls.Add(Me.chkDetails)
        Me.grpControls.Controls.Add(Me.chkCmbCompany)
        Me.grpControls.Controls.Add(Me.Label12)
        Me.grpControls.Controls.Add(Me.chkCmbtablecode)
        Me.grpControls.Controls.Add(Me.Panel1)
        Me.grpControls.Controls.Add(Me.Label1)
        Me.grpControls.Controls.Add(Me.Label4)
        Me.grpControls.Controls.Add(Me.cmbmetal)
        Me.grpControls.Controls.Add(Me.chkrangefrmmaster)
        Me.grpControls.Controls.Add(Me.rangepanel)
        Me.grpControls.Controls.Add(Me.dtpTo)
        Me.grpControls.Controls.Add(Me.lblto)
        Me.grpControls.Controls.Add(Me.chkason)
        Me.grpControls.Controls.Add(Me.chkActualDate)
        Me.grpControls.Controls.Add(Me.chkSubItem)
        Me.grpControls.Controls.Add(Me.dtpAsOnDate)
        Me.grpControls.Controls.Add(Me.btnNew)
        Me.grpControls.Controls.Add(Me.Label8)
        Me.grpControls.Controls.Add(Me.btnExit)
        Me.grpControls.Controls.Add(Me.Label2)
        Me.grpControls.Controls.Add(Me.btnView_Search)
        Me.grpControls.Controls.Add(Me.chkWithStone)
        Me.grpControls.Controls.Add(Me.Label6)
        Me.grpControls.Controls.Add(Me.Label3)
        Me.grpControls.Controls.Add(Me.Label5)
        Me.grpControls.Controls.Add(Me.Label7)
        Me.grpControls.Controls.Add(Me.cmbCategory)
        Me.grpControls.Controls.Add(Me.cmbCostCentre)
        Me.grpControls.Controls.Add(Me.cmbSubItem)
        Me.grpControls.Controls.Add(Me.cmbDesigner)
        Me.grpControls.Controls.Add(Me.cmbCounter)
        Me.grpControls.Controls.Add(Me.cmbItem)
        Me.grpControls.Location = New System.Drawing.Point(323, 10)
        Me.grpControls.Name = "grpControls"
        Me.grpControls.Size = New System.Drawing.Size(419, 593)
        Me.grpControls.TabIndex = 0
        Me.grpControls.TabStop = False
        '
        'chkDetails
        '
        Me.chkDetails.AutoSize = True
        Me.chkDetails.Location = New System.Drawing.Point(258, 321)
        Me.chkDetails.Name = "chkDetails"
        Me.chkDetails.Size = New System.Drawing.Size(65, 17)
        Me.chkDetails.TabIndex = 5
        Me.chkDetails.Text = "Details"
        Me.chkDetails.UseVisualStyleBackColor = True
        '
        'chkCmbCompany
        '
        Me.chkCmbCompany.CheckOnClick = True
        Me.chkCmbCompany.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCompany.DropDownHeight = 1
        Me.chkCmbCompany.FormattingEnabled = True
        Me.chkCmbCompany.IntegralHeight = False
        Me.chkCmbCompany.Location = New System.Drawing.Point(112, 44)
        Me.chkCmbCompany.Name = "chkCmbCompany"
        Me.chkCmbCompany.Size = New System.Drawing.Size(247, 22)
        Me.chkCmbCompany.TabIndex = 5
        Me.chkCmbCompany.ValueSeparator = ", "
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(6, 47)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(62, 13)
        Me.Label12.TabIndex = 4
        Me.Label12.Text = "Company"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkCmbtablecode
        '
        Me.chkCmbtablecode.CheckOnClick = True
        Me.chkCmbtablecode.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbtablecode.DropDownHeight = 1
        Me.chkCmbtablecode.FormattingEnabled = True
        Me.chkCmbtablecode.IntegralHeight = False
        Me.chkCmbtablecode.Location = New System.Drawing.Point(112, 293)
        Me.chkCmbtablecode.Name = "chkCmbtablecode"
        Me.chkCmbtablecode.Size = New System.Drawing.Size(247, 22)
        Me.chkCmbtablecode.TabIndex = 21
        Me.chkCmbtablecode.ValueSeparator = ","
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.chkCounterWise)
        Me.Panel1.Controls.Add(Me.Label11)
        Me.Panel1.Controls.Add(Me.chkitemgrp)
        Me.Panel1.Controls.Add(Me.ChkDesignerGrp)
        Me.Panel1.Controls.Add(Me.Chktablegrpwise)
        Me.Panel1.Location = New System.Drawing.Point(6, 520)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(397, 25)
        Me.Panel1.TabIndex = 27
        '
        'chkCounterWise
        '
        Me.chkCounterWise.AutoSize = True
        Me.chkCounterWise.Location = New System.Drawing.Point(109, 5)
        Me.chkCounterWise.Name = "chkCounterWise"
        Me.chkCounterWise.Size = New System.Drawing.Size(72, 17)
        Me.chkCounterWise.TabIndex = 1
        Me.chkCounterWise.Text = "Counter"
        Me.chkCounterWise.UseVisualStyleBackColor = True
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(5, 6)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(61, 13)
        Me.Label11.TabIndex = 0
        Me.Label11.Text = "Group By"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkitemgrp
        '
        Me.chkitemgrp.AutoSize = True
        Me.chkitemgrp.Location = New System.Drawing.Point(185, 5)
        Me.chkitemgrp.Name = "chkitemgrp"
        Me.chkitemgrp.Size = New System.Drawing.Size(53, 17)
        Me.chkitemgrp.TabIndex = 2
        Me.chkitemgrp.Text = "Item"
        Me.chkitemgrp.UseVisualStyleBackColor = True
        '
        'ChkDesignerGrp
        '
        Me.ChkDesignerGrp.AutoSize = True
        Me.ChkDesignerGrp.Location = New System.Drawing.Point(307, 5)
        Me.ChkDesignerGrp.Name = "ChkDesignerGrp"
        Me.ChkDesignerGrp.Size = New System.Drawing.Size(77, 17)
        Me.ChkDesignerGrp.TabIndex = 4
        Me.ChkDesignerGrp.Text = "Designer"
        Me.ChkDesignerGrp.UseVisualStyleBackColor = True
        '
        'Chktablegrpwise
        '
        Me.Chktablegrpwise.AutoSize = True
        Me.Chktablegrpwise.Location = New System.Drawing.Point(247, 5)
        Me.Chktablegrpwise.Name = "Chktablegrpwise"
        Me.Chktablegrpwise.Size = New System.Drawing.Size(57, 17)
        Me.Chktablegrpwise.TabIndex = 3
        Me.Chktablegrpwise.Text = "Table"
        Me.Chktablegrpwise.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 296)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(72, 13)
        Me.Label1.TabIndex = 20
        Me.Label1.Text = "Table Code"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(6, 105)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(37, 13)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "Metal"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbmetal
        '
        Me.cmbmetal.FormattingEnabled = True
        Me.cmbmetal.Location = New System.Drawing.Point(112, 101)
        Me.cmbmetal.Name = "cmbmetal"
        Me.cmbmetal.Size = New System.Drawing.Size(247, 21)
        Me.cmbmetal.TabIndex = 9
        '
        'chkrangefrmmaster
        '
        Me.chkrangefrmmaster.AutoSize = True
        Me.chkrangefrmmaster.Location = New System.Drawing.Point(115, 321)
        Me.chkrangefrmmaster.Name = "chkrangefrmmaster"
        Me.chkrangefrmmaster.Size = New System.Drawing.Size(137, 17)
        Me.chkrangefrmmaster.TabIndex = 22
        Me.chkrangefrmmaster.Text = "Range From Master"
        Me.chkrangefrmmaster.UseVisualStyleBackColor = True
        '
        'rangepanel
        '
        Me.rangepanel.Controls.Add(Me.Label9)
        Me.rangepanel.Controls.Add(Me.txtFromDay1_NUM)
        Me.rangepanel.Controls.Add(Me.txtFromDay3_NUM)
        Me.rangepanel.Controls.Add(Me.Label10)
        Me.rangepanel.Controls.Add(Me.txtToDay1_NUM)
        Me.rangepanel.Controls.Add(Me.txtToDay3_NUM)
        Me.rangepanel.Controls.Add(Me.txtFromDay2_NUM)
        Me.rangepanel.Controls.Add(Me.txtFromDay4_NUM)
        Me.rangepanel.Controls.Add(Me.txtToDay2_NUM)
        Me.rangepanel.Controls.Add(Me.txtToDay4_NUM)
        Me.rangepanel.Location = New System.Drawing.Point(112, 343)
        Me.rangepanel.Name = "rangepanel"
        Me.rangepanel.Size = New System.Drawing.Size(247, 142)
        Me.rangepanel.TabIndex = 24
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(2, 5)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(36, 13)
        Me.Label9.TabIndex = 0
        Me.Label9.Text = "From"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtFromDay1_NUM
        '
        Me.txtFromDay1_NUM.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtFromDay1_NUM.Location = New System.Drawing.Point(3, 24)
        Me.txtFromDay1_NUM.MaxLength = 6
        Me.txtFromDay1_NUM.Name = "txtFromDay1_NUM"
        Me.txtFromDay1_NUM.Size = New System.Drawing.Size(79, 21)
        Me.txtFromDay1_NUM.TabIndex = 2
        Me.txtFromDay1_NUM.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtFromDay3_NUM
        '
        Me.txtFromDay3_NUM.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtFromDay3_NUM.Location = New System.Drawing.Point(3, 83)
        Me.txtFromDay3_NUM.MaxLength = 6
        Me.txtFromDay3_NUM.Name = "txtFromDay3_NUM"
        Me.txtFromDay3_NUM.Size = New System.Drawing.Size(79, 21)
        Me.txtFromDay3_NUM.TabIndex = 6
        Me.txtFromDay3_NUM.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(88, 5)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(21, 13)
        Me.Label10.TabIndex = 1
        Me.Label10.Text = "To"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtToDay1_NUM
        '
        Me.txtToDay1_NUM.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtToDay1_NUM.Location = New System.Drawing.Point(91, 24)
        Me.txtToDay1_NUM.MaxLength = 6
        Me.txtToDay1_NUM.Name = "txtToDay1_NUM"
        Me.txtToDay1_NUM.Size = New System.Drawing.Size(79, 21)
        Me.txtToDay1_NUM.TabIndex = 3
        Me.txtToDay1_NUM.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtToDay3_NUM
        '
        Me.txtToDay3_NUM.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtToDay3_NUM.Location = New System.Drawing.Point(91, 83)
        Me.txtToDay3_NUM.MaxLength = 6
        Me.txtToDay3_NUM.Name = "txtToDay3_NUM"
        Me.txtToDay3_NUM.Size = New System.Drawing.Size(79, 21)
        Me.txtToDay3_NUM.TabIndex = 7
        Me.txtToDay3_NUM.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtFromDay2_NUM
        '
        Me.txtFromDay2_NUM.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtFromDay2_NUM.Location = New System.Drawing.Point(3, 54)
        Me.txtFromDay2_NUM.MaxLength = 6
        Me.txtFromDay2_NUM.Name = "txtFromDay2_NUM"
        Me.txtFromDay2_NUM.Size = New System.Drawing.Size(79, 21)
        Me.txtFromDay2_NUM.TabIndex = 4
        Me.txtFromDay2_NUM.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtFromDay4_NUM
        '
        Me.txtFromDay4_NUM.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtFromDay4_NUM.Location = New System.Drawing.Point(3, 111)
        Me.txtFromDay4_NUM.MaxLength = 6
        Me.txtFromDay4_NUM.Name = "txtFromDay4_NUM"
        Me.txtFromDay4_NUM.Size = New System.Drawing.Size(79, 21)
        Me.txtFromDay4_NUM.TabIndex = 8
        Me.txtFromDay4_NUM.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtToDay2_NUM
        '
        Me.txtToDay2_NUM.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtToDay2_NUM.Location = New System.Drawing.Point(91, 54)
        Me.txtToDay2_NUM.MaxLength = 6
        Me.txtToDay2_NUM.Name = "txtToDay2_NUM"
        Me.txtToDay2_NUM.Size = New System.Drawing.Size(79, 21)
        Me.txtToDay2_NUM.TabIndex = 5
        Me.txtToDay2_NUM.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtToDay4_NUM
        '
        Me.txtToDay4_NUM.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtToDay4_NUM.Location = New System.Drawing.Point(91, 111)
        Me.txtToDay4_NUM.MaxLength = 6
        Me.txtToDay4_NUM.Name = "txtToDay4_NUM"
        Me.txtToDay4_NUM.Size = New System.Drawing.Size(79, 21)
        Me.txtToDay4_NUM.TabIndex = 9
        Me.txtToDay4_NUM.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(240, 17)
        Me.dtpTo.Mask = "##/##/####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(93, 21)
        Me.dtpTo.TabIndex = 3
        Me.dtpTo.Text = "07/03/9998"
        Me.dtpTo.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'lblto
        '
        Me.lblto.Location = New System.Drawing.Point(208, 16)
        Me.lblto.Name = "lblto"
        Me.lblto.Size = New System.Drawing.Size(25, 21)
        Me.lblto.TabIndex = 2
        Me.lblto.Text = "To"
        Me.lblto.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkason
        '
        Me.chkason.AutoSize = True
        Me.chkason.Location = New System.Drawing.Point(9, 19)
        Me.chkason.Name = "chkason"
        Me.chkason.Size = New System.Drawing.Size(82, 17)
        Me.chkason.TabIndex = 0
        Me.chkason.Text = "DateFrom"
        Me.chkason.UseVisualStyleBackColor = True
        '
        'chkActualDate
        '
        Me.chkActualDate.AutoSize = True
        Me.chkActualDate.Location = New System.Drawing.Point(209, 496)
        Me.chkActualDate.Name = "chkActualDate"
        Me.chkActualDate.Size = New System.Drawing.Size(151, 17)
        Me.chkActualDate.TabIndex = 26
        Me.chkActualDate.Text = "Based On Actual Date"
        Me.chkActualDate.UseVisualStyleBackColor = True
        '
        'chkSubItem
        '
        Me.chkSubItem.AutoSize = True
        Me.chkSubItem.Location = New System.Drawing.Point(9, 167)
        Me.chkSubItem.Name = "chkSubItem"
        Me.chkSubItem.Size = New System.Drawing.Size(79, 17)
        Me.chkSubItem.TabIndex = 12
        Me.chkSubItem.Text = "&Sub Item"
        Me.chkSubItem.UseVisualStyleBackColor = True
        '
        'dtpAsOnDate
        '
        Me.dtpAsOnDate.Location = New System.Drawing.Point(112, 17)
        Me.dtpAsOnDate.Mask = "##/##/####"
        Me.dtpAsOnDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpAsOnDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpAsOnDate.Name = "dtpAsOnDate"
        Me.dtpAsOnDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpAsOnDate.Size = New System.Drawing.Size(93, 21)
        Me.dtpAsOnDate.TabIndex = 1
        Me.dtpAsOnDate.Text = "07/03/9998"
        Me.dtpAsOnDate.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(167, 550)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 29
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(6, 343)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(36, 13)
        Me.Label8.TabIndex = 23
        Me.Label8.Text = "Days"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(270, 550)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 30
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 77)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(60, 13)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "Category"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnView_Search
        '
        Me.btnView_Search.Location = New System.Drawing.Point(64, 550)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(100, 30)
        Me.btnView_Search.TabIndex = 28
        Me.btnView_Search.Text = "&View"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'chkWithStone
        '
        Me.chkWithStone.AutoSize = True
        Me.chkWithStone.Location = New System.Drawing.Point(115, 496)
        Me.chkWithStone.Name = "chkWithStone"
        Me.chkWithStone.Size = New System.Drawing.Size(88, 17)
        Me.chkWithStone.TabIndex = 25
        Me.chkWithStone.Text = "&With Stone"
        Me.chkWithStone.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(6, 232)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(53, 13)
        Me.Label6.TabIndex = 16
        Me.Label6.Text = "Counter"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 137)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(34, 13)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "Item"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(6, 198)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(58, 13)
        Me.Label5.TabIndex = 14
        Me.Label5.Text = "Designer"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(6, 266)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(76, 13)
        Me.Label7.TabIndex = 18
        Me.Label7.Text = "Cost Centre"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbCategory
        '
        Me.cmbCategory.FormattingEnabled = True
        Me.cmbCategory.Location = New System.Drawing.Point(112, 73)
        Me.cmbCategory.Name = "cmbCategory"
        Me.cmbCategory.Size = New System.Drawing.Size(247, 21)
        Me.cmbCategory.TabIndex = 7
        '
        'cmbCostCentre
        '
        Me.cmbCostCentre.FormattingEnabled = True
        Me.cmbCostCentre.Location = New System.Drawing.Point(112, 262)
        Me.cmbCostCentre.Name = "cmbCostCentre"
        Me.cmbCostCentre.Size = New System.Drawing.Size(247, 21)
        Me.cmbCostCentre.TabIndex = 19
        '
        'cmbSubItem
        '
        Me.cmbSubItem.FormattingEnabled = True
        Me.cmbSubItem.Location = New System.Drawing.Point(112, 162)
        Me.cmbSubItem.Name = "cmbSubItem"
        Me.cmbSubItem.Size = New System.Drawing.Size(247, 21)
        Me.cmbSubItem.TabIndex = 13
        '
        'cmbDesigner
        '
        Me.cmbDesigner.FormattingEnabled = True
        Me.cmbDesigner.Location = New System.Drawing.Point(112, 194)
        Me.cmbDesigner.Name = "cmbDesigner"
        Me.cmbDesigner.Size = New System.Drawing.Size(247, 21)
        Me.cmbDesigner.TabIndex = 15
        '
        'cmbCounter
        '
        Me.cmbCounter.FormattingEnabled = True
        Me.cmbCounter.Location = New System.Drawing.Point(112, 228)
        Me.cmbCounter.Name = "cmbCounter"
        Me.cmbCounter.Size = New System.Drawing.Size(247, 21)
        Me.cmbCounter.TabIndex = 17
        '
        'cmbItem
        '
        Me.cmbItem.FormattingEnabled = True
        Me.cmbItem.Location = New System.Drawing.Point(112, 132)
        Me.cmbItem.Name = "cmbItem"
        Me.cmbItem.Size = New System.Drawing.Size(247, 21)
        Me.cmbItem.TabIndex = 11
        '
        'tabView
        '
        Me.tabView.Controls.Add(Me.pnlView)
        Me.tabView.Location = New System.Drawing.Point(4, 22)
        Me.tabView.Name = "tabView"
        Me.tabView.Padding = New System.Windows.Forms.Padding(3)
        Me.tabView.Size = New System.Drawing.Size(1014, 614)
        Me.tabView.TabIndex = 1
        Me.tabView.Text = "View"
        Me.tabView.UseVisualStyleBackColor = True
        '
        'pnlView
        '
        Me.pnlView.Controls.Add(Me.pnlFooter)
        Me.pnlView.Controls.Add(Me.pnlGridView)
        Me.pnlView.Controls.Add(Me.pnlTitle)
        Me.pnlView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlView.Location = New System.Drawing.Point(3, 3)
        Me.pnlView.Name = "pnlView"
        Me.pnlView.Size = New System.Drawing.Size(1008, 608)
        Me.pnlView.TabIndex = 0
        '
        'pnlFooter
        '
        Me.pnlFooter.Controls.Add(Me.btnPrint)
        Me.pnlFooter.Controls.Add(Me.btnExport)
        Me.pnlFooter.Controls.Add(Me.btnBack)
        Me.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlFooter.Location = New System.Drawing.Point(0, 565)
        Me.pnlFooter.Name = "pnlFooter"
        Me.pnlFooter.Size = New System.Drawing.Size(1008, 43)
        Me.pnlFooter.TabIndex = 2
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(755, 3)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 30)
        Me.btnPrint.TabIndex = 0
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(649, 3)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 0
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'btnBack
        '
        Me.btnBack.Location = New System.Drawing.Point(543, 3)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(100, 30)
        Me.btnBack.TabIndex = 0
        Me.btnBack.Text = "Back [Esc]"
        Me.btnBack.UseVisualStyleBackColor = True
        '
        'pnlGridView
        '
        Me.pnlGridView.Controls.Add(Me.pnlgrid)
        Me.pnlGridView.Controls.Add(Me.gridViewHead)
        Me.pnlGridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlGridView.Location = New System.Drawing.Point(0, 30)
        Me.pnlGridView.Name = "pnlGridView"
        Me.pnlGridView.Size = New System.Drawing.Size(1008, 578)
        Me.pnlGridView.TabIndex = 1
        '
        'pnlgrid
        '
        Me.pnlgrid.Controls.Add(Me.gridView_OWN)
        Me.pnlgrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlgrid.Location = New System.Drawing.Point(0, 20)
        Me.pnlgrid.Name = "pnlgrid"
        Me.pnlgrid.Size = New System.Drawing.Size(1008, 558)
        Me.pnlgrid.TabIndex = 3
        '
        'gridView_OWN
        '
        Me.gridView_OWN.AllowUserToAddRows = False
        Me.gridView_OWN.AllowUserToDeleteRows = False
        Me.gridView_OWN.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridView_OWN.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
        Me.gridView_OWN.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView_OWN.ContextMenuStrip = Me.ContextMenuStrip2
        Me.gridView_OWN.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView_OWN.Location = New System.Drawing.Point(0, 0)
        Me.gridView_OWN.MultiSelect = False
        Me.gridView_OWN.Name = "gridView_OWN"
        Me.gridView_OWN.ReadOnly = True
        Me.gridView_OWN.RowHeadersVisible = False
        Me.gridView_OWN.RowTemplate.Height = 17
        Me.gridView_OWN.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridView_OWN.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.gridView_OWN.Size = New System.Drawing.Size(1008, 558)
        Me.gridView_OWN.TabIndex = 1
        '
        'ContextMenuStrip2
        '
        Me.ContextMenuStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ResizeToolStripMenuItem1})
        Me.ContextMenuStrip2.Name = "ContextMenuStrip2"
        Me.ContextMenuStrip2.Size = New System.Drawing.Size(107, 26)
        '
        'ResizeToolStripMenuItem1
        '
        Me.ResizeToolStripMenuItem1.CheckOnClick = True
        Me.ResizeToolStripMenuItem1.Name = "ResizeToolStripMenuItem1"
        Me.ResizeToolStripMenuItem1.Size = New System.Drawing.Size(106, 22)
        Me.ResizeToolStripMenuItem1.Text = "Resize"
        '
        'gridViewHead
        '
        Me.gridViewHead.AllowUserToAddRows = False
        Me.gridViewHead.AllowUserToDeleteRows = False
        Me.gridViewHead.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridViewHead.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
        Me.gridViewHead.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridViewHead.Dock = System.Windows.Forms.DockStyle.Top
        Me.gridViewHead.Location = New System.Drawing.Point(0, 0)
        Me.gridViewHead.Name = "gridViewHead"
        Me.gridViewHead.ReadOnly = True
        Me.gridViewHead.Size = New System.Drawing.Size(1008, 20)
        Me.gridViewHead.TabIndex = 2
        '
        'pnlTitle
        '
        Me.pnlTitle.Controls.Add(Me.lblTitle)
        Me.pnlTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlTitle.Location = New System.Drawing.Point(0, 0)
        Me.pnlTitle.Name = "pnlTitle"
        Me.pnlTitle.Size = New System.Drawing.Size(1008, 30)
        Me.pnlTitle.TabIndex = 0
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(412, 10)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(36, 13)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "Title"
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExitToolStripMenuItem, Me.NewToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(118, 48)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ResizeToolStripMenuItem
        '
        Me.ResizeToolStripMenuItem.Name = "ResizeToolStripMenuItem"
        Me.ResizeToolStripMenuItem.Size = New System.Drawing.Size(32, 19)
        '
        'frmAgeWiseAnalysis
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1022, 640)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.pnlMain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmAgeWiseAnalysis"
        Me.Text = "AgeWiseAnalysis"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.pnlMain.ResumeLayout(False)
        Me.tabMain.ResumeLayout(False)
        Me.tabGen.ResumeLayout(False)
        Me.pnlInput.ResumeLayout(False)
        Me.grprangedetail.ResumeLayout(False)
        CType(Me.gridrange, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpControls.ResumeLayout(False)
        Me.grpControls.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.rangepanel.ResumeLayout(False)
        Me.rangepanel.PerformLayout()
        Me.tabView.ResumeLayout(False)
        Me.pnlView.ResumeLayout(False)
        Me.pnlFooter.ResumeLayout(False)
        Me.pnlGridView.ResumeLayout(False)
        Me.pnlgrid.ResumeLayout(False)
        CType(Me.gridView_OWN, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip2.ResumeLayout(False)
        CType(Me.gridViewHead, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlTitle.ResumeLayout(False)
        Me.pnlTitle.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnlMain As System.Windows.Forms.Panel
    Friend WithEvents txtToDay4_NUM As System.Windows.Forms.TextBox
    Friend WithEvents txtToDay2_NUM As System.Windows.Forms.TextBox
    Friend WithEvents txtFromDay4_NUM As System.Windows.Forms.TextBox
    Friend WithEvents txtFromDay2_NUM As System.Windows.Forms.TextBox
    Friend WithEvents txtToDay3_NUM As System.Windows.Forms.TextBox
    Friend WithEvents txtToDay1_NUM As System.Windows.Forms.TextBox
    Friend WithEvents txtFromDay3_NUM As System.Windows.Forms.TextBox
    Friend WithEvents txtFromDay1_NUM As System.Windows.Forms.TextBox
    Friend WithEvents cmbCostCentre As System.Windows.Forms.ComboBox
    Friend WithEvents cmbDesigner As System.Windows.Forms.ComboBox
    Friend WithEvents cmbItem As System.Windows.Forms.ComboBox
    Friend WithEvents cmbCounter As System.Windows.Forms.ComboBox
    Friend WithEvents cmbSubItem As System.Windows.Forms.ComboBox
    Friend WithEvents cmbCategory As System.Windows.Forms.ComboBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnView_Search As System.Windows.Forms.Button
    Friend WithEvents chkWithStone As System.Windows.Forms.CheckBox
    Friend WithEvents grpControls As System.Windows.Forms.GroupBox
    Friend WithEvents gridView_OWN As System.Windows.Forms.DataGridView
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tabMain As System.Windows.Forms.TabControl
    Friend WithEvents tabGen As System.Windows.Forms.TabPage
    Friend WithEvents pnlInput As System.Windows.Forms.Panel
    Friend WithEvents tabView As System.Windows.Forms.TabPage
    Friend WithEvents pnlView As System.Windows.Forms.Panel
    Friend WithEvents pnlGridView As System.Windows.Forms.Panel
    Friend WithEvents pnlTitle As System.Windows.Forms.Panel
    Friend WithEvents pnlFooter As System.Windows.Forms.Panel
    Friend WithEvents btnBack As System.Windows.Forms.Button
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents gridViewHead As System.Windows.Forms.DataGridView
    Friend WithEvents pnlgrid As System.Windows.Forms.Panel
    Friend WithEvents dtpAsOnDate As BrighttechPack.DatePicker
    Friend WithEvents chkCounterWise As System.Windows.Forms.CheckBox
    Friend WithEvents chkSubItem As System.Windows.Forms.CheckBox
    Friend WithEvents chkActualDate As System.Windows.Forms.CheckBox
    Friend WithEvents chkason As System.Windows.Forms.CheckBox
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents lblto As System.Windows.Forms.Label
    Friend WithEvents rangepanel As System.Windows.Forms.Panel
    Friend WithEvents chkrangefrmmaster As System.Windows.Forms.CheckBox
    Friend WithEvents grprangedetail As System.Windows.Forms.GroupBox
    Friend WithEvents gridrange As System.Windows.Forms.DataGridView
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents cmbmetal As System.Windows.Forms.ComboBox
    Friend WithEvents chkitemgrp As System.Windows.Forms.CheckBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Chktablegrpwise As System.Windows.Forms.CheckBox
    Friend WithEvents ChkDesignerGrp As System.Windows.Forms.CheckBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents chkCmbtablecode As BrighttechPack.CheckedComboBox
    Friend WithEvents ResizeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ContextMenuStrip2 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ResizeToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents chkCmbCompany As BrighttechPack.CheckedComboBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents chkDetails As CheckBox
    'Friend WithEvents flexGrid As AxMSFlexGridLib.AxMSFlexGrid
End Class
